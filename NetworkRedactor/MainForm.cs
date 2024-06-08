using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace NetworkRedactor
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint, true);
            this.DoubleBuffered = true;
            InitializeComponent();
            EnableDoubleBuffering(workspacePanel);
        }

        private void EnableDoubleBuffering(Panel panel)
        {
            typeof(Panel).InvokeMember("DoubleBuffered",
                BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic,
                null, panel, new object[] { true });
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private List<Device> devices = new List<Device>();
        private List<Connection> connections = new List<Connection>();
        private Device selectedDevice = null;
        private Device clickedDevice = null;
        public static int LNCount = 0;

        public abstract class Device
        {
            public static int RouterCount { get; private set; } = 0;
            public static int ServerCount { get; private set; } = 0;
            public static int PCCount { get; private set; } = 0;

            public string LocalNetworkName = "";
            public string IPAddress { get; set; }
            public int Port { get; set; }

            public string Name { get; set; }
            public int X { get; set; }
            public int Y { get; set; }

            public abstract void Draw(Graphics graphics);

            protected static void IncrementRouterCount() => RouterCount++;
            protected static void IncrementServerCount() => ServerCount++;
            protected static void IncrementPCCount() => PCCount++;

            public static void DecrementRouterCount() => RouterCount--;
            public static void DecrementServerCount() => ServerCount--;
            public static void DecrementPCCount() => PCCount--;

            public Point TopPoint => new Point(X, Y - 25);
            public Point BottomPoint => new Point(X, Y + 25);
            public Point LeftPoint => new Point(X - 25, Y);
            public Point RightPoint => new Point(X + 25, Y);

            public List<Device> ForwardConnectedDevices { get; } = new List<Device>();
            
            public Device(string name) => Name = name;

            public void Connect(Device device)
            {
                if (!ForwardConnectedDevices.Contains(device))
                {
                    ForwardConnectedDevices.Add(device);
                }
            }

            public void Disconnect(Device device)
            {
                ForwardConnectedDevices.Remove(device);
            }

            public bool Ping(Device firstDevice, Device secondDevice)
            {
                //return ForwardConnectedDevices.Contains(targetDevice);
                return firstDevice.LocalNetworkName == secondDevice.LocalNetworkName;
            }
        }

        public class Router : Device
        {
            private Image RouterIcon = Properties.Resources.RouterIcon;
            public Router(string name) : base(name)
            {
                IncrementRouterCount();
                Name += RouterCount.ToString();
            }
            public override void Draw(Graphics graphics)
            {
                // Размеры и координаты для отрисовки ПК
                int size = 50;
                int x = this.X - size / 2;  // Центрирование по X
                int y = this.Y - size / 2;  // Центрирование по Y
                graphics.DrawImage(RouterIcon, x, y, 50, 50);
                graphics.DrawString(this.Name, SystemFonts.DefaultFont, Brushes.White, x + 5, y + size + 5);
            }
        }

        public class Server : Device
        {
            private Image ServerIcon = Properties.Resources.ServerIcon;
            public Server(string name) : base(name)
            {
                IncrementServerCount();
                Name += ServerCount.ToString();
            }
            public override void Draw(Graphics graphics)
            {
                // Размеры и координаты для отрисовки ПК
                int size = 50;
                int x = this.X - size / 2;  // Центрирование по X
                int y = this.Y - size / 2;  // Центрирование по Y
                graphics.DrawImage(ServerIcon, x, y, 50, 50);
                graphics.DrawString(this.Name, SystemFonts.DefaultFont, Brushes.White, x + 5, y + size + 5);
            }
        }

        public class PC : Device
        {
            private Image PCIcon = Properties.Resources.PCIcon;
            public PC(string name) : base(name)
            {
                IncrementPCCount();
                Name += PCCount.ToString();
            }
            public override void Draw(Graphics graphics)
            {
                // Размеры и координаты для отрисовки ПК
                int size = 50;
                int x = this.X - size / 2;  // Центрирование по X
                int y = this.Y - size / 2;  // Центрирование по Y
                graphics.DrawImage(PCIcon, x, y, 50, 50);
                graphics.DrawString(this.Name, SystemFonts.DefaultFont, Brushes.White, x + 5, y + size + 5);
            }
        }

        private void RemoveDevice(Device device)
        {
            if (device is Router)
            {
                Device.DecrementRouterCount();
            }
            else if (device is Server)
            {
                Device.DecrementServerCount();
            }
            else if (device is PC)
            {
                Device.DecrementPCCount();
            }
            devices.Remove(device);
            connections.RemoveAll(c => c.StartDevice == device || c.EndDevice == device);
            workspacePanel.Invalidate();  // Перерисовка рабочего поля
        }

        public class Connection 
        {
            public Device StartDevice { get; set; }
            public Device EndDevice { get; set; }
            public Point MiddlePoint { get; private set; }

            public Connection(Device startDevice, Device endDevice)
            {
                StartDevice = startDevice;
                EndDevice = endDevice;
            }

            public Point GetClosestConnectionPoints(Device device1, Device device2)
            {
                var points = new[]
                {
                Tuple.Create(device1.TopPoint, device2.TopPoint),
                Tuple.Create(device1.BottomPoint, device2.BottomPoint),
                Tuple.Create(device1.LeftPoint, device2.LeftPoint),
                Tuple.Create(device1.RightPoint, device2.RightPoint)
                };

                var minDistance = double.MaxValue;
                Point closestPoint1 = Point.Empty;
                Point closestPoint2 = Point.Empty;

                foreach (var (point1, point2) in points)
                {
                    var distance = Math.Sqrt(Math.Pow(point1.X - point2.X, 2) + Math.Pow(point1.Y - point2.Y, 2));

                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        closestPoint1 = point1;
                        closestPoint2 = point2;
                    }
                }

                return new Point((closestPoint1.X + closestPoint2.X) / 2, (closestPoint1.Y + closestPoint2.Y) / 2);
            }

            public void UpdateConnectionPoints()
            {
                Point connectionPoint = GetClosestConnectionPoints(StartDevice, EndDevice);
                this.MiddlePoint = connectionPoint;
            }

            public void Draw(Graphics graphics) 
            {
                Pen pen = new Pen(Color.White, 2);
                graphics.DrawLine(pen, StartDevice.TopPoint, EndDevice.TopPoint);
            }
        }

        private Device GetSelectedToolboxDevice(Point point)
        {
            int deviceSize = 50; // Размер устройства
            int spacing = 10; // Расстояние между устройствами
            // Определение зон для разных устройств на панели инструментов
            Rectangle routerRect = new Rectangle(spacing, spacing, deviceSize, deviceSize);
            Rectangle serverRect = new Rectangle(2 * spacing + deviceSize, spacing, deviceSize, deviceSize);
            Rectangle pcRect = new Rectangle(3 * spacing + 2 * deviceSize, spacing, deviceSize, deviceSize);
            if (routerRect.Contains(point))
            {               
                return new Router("Router ");
            }
            else if (serverRect.Contains(point))
            {
                return new Server("Server ");              
            }
            else if (pcRect.Contains(point))
            {
                return new PC("PC ");
            }
            return null;
        }
 
        private void toolboxPanel_Paint(object sender, PaintEventArgs e)
        {
            // Отрисовка устройств на панели инструментов
            DrawDevicesOnToolbox(e.Graphics);
        }

        private void DrawDevicesOnToolbox(Graphics graphics)
        {
            int deviceSize = 50; // Размер устройства
            int spacing = 10; // Расстояние между устройствами

            // Отрисовка устройств на панели инструментов
            Image routerIcon = Properties.Resources.RouterIcon;
            graphics.DrawImage(routerIcon, spacing, spacing, deviceSize, deviceSize);
            graphics.DrawString("Router", this.Font, Brushes.White, spacing, spacing + deviceSize + 5);

            Image serverIcon = Properties.Resources.ServerIcon;
            graphics.DrawImage(serverIcon, 2 * spacing + deviceSize, spacing, deviceSize, deviceSize);
            graphics.DrawString("Server", this.Font, Brushes.White, 2 * spacing + deviceSize, spacing + deviceSize + 5);

            Image pcIcon = Properties.Resources.PCIcon;
            graphics.DrawImage(pcIcon, 3 * spacing + 2 * deviceSize, spacing, deviceSize, deviceSize);
            graphics.DrawString("PC", this.Font, Brushes.White, 3 * spacing + 2 * deviceSize, spacing + deviceSize + 5); 
        }

        private void toolboxPanel_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                selectedDevice = GetSelectedToolboxDevice(e.Location);
                if (selectedDevice != null)
                {
                    selectedDevice.X = e.X;
                    selectedDevice.Y = e.Y;
                }
            }
        }

        private void workspacePanel_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            // Clear the background
            e.Graphics.Clear(workspacePanel.BackColor);

            // Draw connections
            foreach (var connection in connections)
            {
                connection.Draw(e.Graphics);
            }

            // Draw devices
            foreach (var device in devices)
            {
                device.Draw(e.Graphics);
            }
        }


        private void workspacePanel_MouseUp(object sender, MouseEventArgs e)
        {
            // Размещение выбранного устройства на рабочем поле
            if (selectedDevice != null && e.Button == MouseButtons.Left)
            {
                // Установка положения для выбранного устройства
                selectedDevice.X = e.X;
                selectedDevice.Y = e.Y;
                // Добавление устройства в коллекцию
                devices.Add(selectedDevice);
                // Перерисовка рабочего поля
                workspacePanel.Invalidate();
                selectedDevice = null;
            }
            
        }
     
        private void workspacePanel_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && !connectModeCheckbox.Checked)
            {
                if (deleteModeCheckbox.Checked)
                {
                    RemoveDeviceOnWorkspacePanelMouseDown(e);
                }
                else
                {
                    MoveDeviceOnWorkspacePanelMouseDown(e);
                }
            }
            else if (e.Button == MouseButtons.Left && connectModeCheckbox.Checked)
            {
                CreateConnectionOnWorkspacePanelMouseDown(e);
            }
            if (e.Button == MouseButtons.Right)
            {
                foreach (var device in devices)
                {
                    if (e.X >= device.X - 25 && e.X <= device.X + 25 &&
                        e.Y >= device.Y - 25 && e.Y <= device.Y + 25)
                    {
                        ShowDeviceSettings(device);
                        break;
                    }
                }
            }
        }

        private void workspacePanel_MouseMove(object sender, MouseEventArgs e)
        {
            // Перемещение выбранного устройства
            if (selectedDevice != null && e.Button == MouseButtons.Left)
            {
                selectedDevice.X = e.X;
                selectedDevice.Y = e.Y;

                // Перерисовка рабочего поля только при необходимости
                workspacePanel.Invalidate();
            }
        }


        // Функция, проверяющая, есть ли уже связь между двумя выбранными устройствами, блокирующая дублирование связей 
        private bool IsAlreadyConnected(Device device1, Device device2)
        {
            return connections.Any(c =>
                (c.StartDevice == device1 && c.EndDevice == device2) ||
                (c.StartDevice == device2 && c.EndDevice == device1));
        }

        // Функция, использующая для корректного разделения перемещения устройств и их соединения (перемещение) 
        private void MoveDeviceOnWorkspacePanelMouseDown(MouseEventArgs e)
        {
            foreach (var device in devices)
            {
                if (e.X >= device.X - 25 && e.X <= device.X + 25 &&
                    e.Y >= device.Y - 25 && e.Y <= device.Y + 25)
                {
                    selectedDevice = device;
                    break;
                }
            }
        }

        // Функция, использующая для корректного разделения перемещения устройств и их соединения (создание связи) 
        private void CreateConnectionOnWorkspacePanelMouseDown(MouseEventArgs e)
        {
            foreach (var device in devices)
            {
                if (e.X >= device.X - 25 && e.X <= device.X + 25 &&
                    e.Y >= device.Y - 25 && e.Y <= device.Y + 25)
                {
                    if (clickedDevice == null)
                    {
                        clickedDevice = device;
                        break;
                    }
                    else if (selectedDevice == null && clickedDevice != device)
                    {
                        selectedDevice = device;
                        // Проверка на уже существующее соединение
                        if (!IsAlreadyConnected(clickedDevice, selectedDevice))
                        {
                            GetLocalNetwork(clickedDevice, selectedDevice);
                            if (kostil == 0)
                            {
                                clickedDevice.Connect(selectedDevice);
                                selectedDevice.Connect(clickedDevice);
                                connections.Add(new Connection(clickedDevice, selectedDevice));
                                workspacePanel.Invalidate();
                            }  
                            else 
                                kostil--;
                        }
                        selectedDevice = null;
                        clickedDevice = null;
                        break;
                    }
                }
            }
        }

        public int kostil = 0;

        public void GetLocalNetwork(Device StartDevice, Device EndDevice)
        {
            if (StartDevice.LocalNetworkName == "" && EndDevice.LocalNetworkName == "")
            {
                LNCount++;
                StartDevice.LocalNetworkName = EndDevice.LocalNetworkName = "LN" + LNCount; 
            }
            else if (StartDevice.LocalNetworkName != "" && EndDevice.LocalNetworkName != "" && StartDevice.LocalNetworkName != EndDevice.LocalNetworkName)
            {
                DialogResult dialogResult = MessageBox.Show("Объединить цепь в локальную сеть?", "???", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    foreach (var iter in devices) 
                    {
                        if (iter.LocalNetworkName == EndDevice.LocalNetworkName) { iter.LocalNetworkName = StartDevice.LocalNetworkName; }
                    }
                }
                else if (dialogResult == DialogResult.No)
                {
                    kostil++;
                }
            }
            else
            {
                if (StartDevice.LocalNetworkName != "")
                {
                    EndDevice.LocalNetworkName = StartDevice.LocalNetworkName;
                }
                else
                    StartDevice.LocalNetworkName = EndDevice.LocalNetworkName;
            }
        }

        // Функция, используемая для удаления объектов с рабочего поля 
        private void RemoveDeviceOnWorkspacePanelMouseDown(MouseEventArgs e)
        {
            foreach (var device in devices.ToList()) // Используем ToList() для безопасного перебора
            {
                if (e.X >= device.X - 25 && e.X <= device.X + 25 &&
                    e.Y >= device.Y - 25 && e.Y <= device.Y + 25)
                {
                    RemoveDevice(device);
                    break; // Выходим из цикла после удаления первого устройства, чтобы не удалять несколько устройств одновременно
                }
            }
        }

        private void ShowDeviceSettings(Device device)
        {
            DeviceSettingsForm settingsForm = new DeviceSettingsForm(device.Name, device.IPAddress, device.LocalNetworkName);
            if (settingsForm.ShowDialog() == DialogResult.OK)
            {
                // Обработка сохранения настроек с полученными данными (если нужно)
                string enteredIP = settingsForm.GetEnteredIP();
                string enteredName = settingsForm.GetEnteredName();
                device.IPAddress = enteredIP;
                device.Name = enteredName;

                workspacePanel.Invalidate();
            }
        }

        private void ShowCommandForm()
        {
            CommandPromptForm settingsForm = new CommandPromptForm(devices);
            if (settingsForm.ShowDialog() == DialogResult.OK)
            {

                workspacePanel.Invalidate();
            }
        }

        private void CommandFormOpen_Click(object sender, EventArgs e)
        {
            ShowCommandForm();
        }
    }
}
