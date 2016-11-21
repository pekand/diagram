﻿using System;
using System.Windows.Forms;

/*
 
*/

namespace Diagram
{
    public partial class MainForm : Form
    {
        // parent
        public Main main = null;

        public MainForm(Main main)
        {
            this.main = main;
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(128, 114);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = global::Diagram.Properties.Resources.ico_diagramico_forms;
            this.Name = "MainForm";
            this.ShowInTaskbar = false;
            this.Text = "MainForm";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResumeLayout(false);

        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            Program.log.write("Main form: hide");
            this.Hide();
            BeginInvoke(new MethodInvoker(delegate
            {
                Hide();
            }));
        }

        public void OpenDiagram(String Message)
        {
            main.OpenDiagram(Message);
        }

        public void ExitApplication()
        {
            main.ExitApplication();
        }
    }
}
