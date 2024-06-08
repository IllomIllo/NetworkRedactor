using System;
using System.Drawing;
using System.Windows.Forms;

namespace NetworkRedactor
{
    public partial class DeviceSettingsForm : Form
    {
        private TextBox ipTextBox;
        private TextBox nameTextBox;
        private TextBox lnNameTextBox;
        private Button okButton;
        private Button cancelButton;
        private string enteredIP;
        private string enteredName;

        public DeviceSettingsForm(string deviceName, string ipAddress, string lnName)
        {
            this.DoubleBuffered = true;
            InitializeComponent();
            this.Text = "Settings for " + deviceName;

            Label ipLabel = new Label();
            ipLabel.Text = "IP Address:";
            ipLabel.Location = new Point(10, 10);
            ipLabel.AutoSize = true;

            ipTextBox = new TextBox();
            ipTextBox.Text = ipAddress;
            ipTextBox.Location = new Point(100, 10);
            ipTextBox.Width = 150;

            Label nameLabel = new Label();
            nameLabel.Text = "Name:";
            nameLabel.Location = new Point(10, 50);
            nameLabel.AutoSize = true;

            nameTextBox = new TextBox();
            nameTextBox.Text = deviceName;
            nameTextBox.Location = new Point(100, 50);
            nameTextBox.Width = 150;

            Label lnNameLabel = new Label();
            lnNameLabel.Text = "Local Network:";
            lnNameLabel.Location = new Point(10, 90);
            lnNameLabel.AutoSize = true;

            lnNameTextBox = new TextBox();
            lnNameTextBox.Text = lnName;
            lnNameTextBox.Location = new Point(100, 90);
            lnNameTextBox.Width = 150;
            lnNameTextBox.Enabled = false;



            okButton = new Button();
            okButton.Text = "OK";
            okButton.Location = new Point(100, 130);
            okButton.Click += new EventHandler(OkButton_Click);

            cancelButton = new Button();
            cancelButton.Text = "Cancel";
            cancelButton.Location = new Point(180, 130);
            cancelButton.Click += new EventHandler(CancelButton_Click);

            this.Controls.Add(ipLabel);
            this.Controls.Add(ipTextBox);
            this.Controls.Add(nameLabel);
            this.Controls.Add(nameTextBox);
            this.Controls.Add(lnNameLabel);
            this.Controls.Add(lnNameTextBox);
            this.Controls.Add(okButton);
            this.Controls.Add(cancelButton);
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            enteredIP = ipTextBox.Text;
            enteredName = nameTextBox.Text;
            
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        public string GetEnteredIP()
        {
            return enteredIP;
        }

        public string GetEnteredName() 
        { 
            return enteredName; 
        }
    }
}
