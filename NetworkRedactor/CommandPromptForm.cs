using static NetworkRedactor.MainForm;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Linq;
using System;

namespace NetworkRedactor
{
    public partial class CommandPromptForm : Form
    {
        private List<Device> devices;

        public CommandPromptForm(List<Device> devices)
        {
            this.DoubleBuffered = true;
            InitializeComponent();
            this.devices = devices;
        }

        
        private void ExecutePing(string sourceDeviceName, string targetDeviceName)
        {
           var sourceDevice = devices.FirstOrDefault(d => d.Name == sourceDeviceName);
           var targetDevice = devices.FirstOrDefault(d => d.Name == targetDeviceName);

           if (sourceDevice != null && targetDevice != null)
           {
               bool result = sourceDevice.Ping(sourceDevice, targetDevice);
               resultTextBox.AppendText($"Ping from {sourceDeviceName} to {targetDeviceName}: {(result ? "Success" : "Failed")}\n");
           }
           else
           {
               resultTextBox.AppendText("Invalid device names.\n");
           }
        }

        private void executeButton_Click(object sender, EventArgs e)
        {
           string command = commandTextBox.Text;
           var parts = command.Split(' ');
           if (parts.Length == 3 && parts[0].ToLower() == "ping")
           {
               ExecutePing(parts[1], parts[2]);
           }
           else
           {
               resultTextBox.AppendText("Invalid command.\n");
           }
        }

        
    }
}