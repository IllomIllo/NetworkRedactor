using System.Windows.Forms;

namespace NetworkRedactor
{
    public class DoubleBufferedPanel : Panel
    {
        public DoubleBufferedPanel()
        {
            this.DoubleBuffered = true;
        }
    }

    partial class MainForm
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.toolboxPanel = new System.Windows.Forms.Panel();
            this.workspacePanel = new System.Windows.Forms.Panel();
            this.connectModeCheckbox = new System.Windows.Forms.CheckBox();
            this.deleteModeCheckbox = new System.Windows.Forms.CheckBox();
            this.CommandFormOpen = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // toolboxPanel
            // 
            this.toolboxPanel.BackColor = System.Drawing.SystemColors.ControlText;
            this.toolboxPanel.Location = new System.Drawing.Point(11, 576);
            this.toolboxPanel.Name = "toolboxPanel";
            this.toolboxPanel.Size = new System.Drawing.Size(1079, 96);
            this.toolboxPanel.TabIndex = 1;
            this.toolboxPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.toolboxPanel_Paint);
            this.toolboxPanel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.toolboxPanel_MouseDown);
            // 
            // workspacePanel
            // 
            this.workspacePanel.BackColor = System.Drawing.SystemColors.ControlText;
            this.workspacePanel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.workspacePanel.Location = new System.Drawing.Point(11, 12);
            this.workspacePanel.Name = "workspacePanel";
            this.workspacePanel.Size = new System.Drawing.Size(903, 552);
            this.workspacePanel.TabIndex = 2;
            this.workspacePanel.Paint += new System.Windows.Forms.PaintEventHandler(this.workspacePanel_Paint);
            this.workspacePanel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.workspacePanel_MouseDown);
            this.workspacePanel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.workspacePanel_MouseMove);
            this.workspacePanel.MouseUp += new System.Windows.Forms.MouseEventHandler(this.workspacePanel_MouseUp);
            // 
            // connectModeCheckbox
            // 
            this.connectModeCheckbox.AutoSize = true;
            this.connectModeCheckbox.Font = new System.Drawing.Font("Dark Underground", 10.2F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.connectModeCheckbox.ForeColor = System.Drawing.Color.Lime;
            this.connectModeCheckbox.Location = new System.Drawing.Point(920, 38);
            this.connectModeCheckbox.Name = "connectModeCheckbox";
            this.connectModeCheckbox.Size = new System.Drawing.Size(148, 21);
            this.connectModeCheckbox.TabIndex = 3;
            this.connectModeCheckbox.Text = "Connection mod\r\n";
            this.connectModeCheckbox.UseVisualStyleBackColor = true;
            // 
            // deleteModeCheckbox
            // 
            this.deleteModeCheckbox.AutoSize = true;
            this.deleteModeCheckbox.Font = new System.Drawing.Font("Dark Underground", 10.2F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.deleteModeCheckbox.ForeColor = System.Drawing.Color.Red;
            this.deleteModeCheckbox.Location = new System.Drawing.Point(920, 65);
            this.deleteModeCheckbox.Name = "deleteModeCheckbox";
            this.deleteModeCheckbox.Size = new System.Drawing.Size(112, 21);
            this.deleteModeCheckbox.TabIndex = 4;
            this.deleteModeCheckbox.Text = "Delete mod\r\n";
            this.deleteModeCheckbox.UseVisualStyleBackColor = true;
            // 
            // CommandFormOpen
            // 
            this.CommandFormOpen.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.CommandFormOpen.Font = new System.Drawing.Font("Yu Gothic UI", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.CommandFormOpen.Location = new System.Drawing.Point(920, 497);
            this.CommandFormOpen.Name = "CommandFormOpen";
            this.CommandFormOpen.Size = new System.Drawing.Size(170, 67);
            this.CommandFormOpen.TabIndex = 6;
            this.CommandFormOpen.Text = "Open Command Prompt Form";
            this.CommandFormOpen.UseVisualStyleBackColor = true;
            this.CommandFormOpen.Click += new System.EventHandler(this.CommandFormOpen_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.ClientSize = new System.Drawing.Size(1102, 684);
            this.Controls.Add(this.CommandFormOpen);
            this.Controls.Add(this.workspacePanel);
            this.Controls.Add(this.toolboxPanel);
            this.Controls.Add(this.deleteModeCheckbox);
            this.Controls.Add(this.connectModeCheckbox);
            this.Name = "MainForm";
            this.Text = "Network Editor 1.0";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Panel toolboxPanel;
        private Panel workspacePanel;
        private CheckBox connectModeCheckbox;
        private CheckBox deleteModeCheckbox;
        private Button CommandFormOpen;
    }
}

