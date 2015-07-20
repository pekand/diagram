using System;
using System.Windows.Forms;

namespace Diagram
{
    public partial class AboutForm : Form
    {
        public Main main = null;

        private System.Windows.Forms.Label labelProgramName;
        private System.Windows.Forms.Button buttonOk;
        private System.Windows.Forms.Label labelLicence;
        private System.Windows.Forms.Label labelAuthor;
        private System.Windows.Forms.LinkLabel linkLabelMe;
        private System.Windows.Forms.Label labelLicenceType;
        private Label labelMode;
        private Label labelVersionNumber;
        private Label labelVersion;

        public AboutForm(Main main)
        {
            this.main = main;
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.labelProgramName = new System.Windows.Forms.Label();
            this.buttonOk = new System.Windows.Forms.Button();
            this.labelLicence = new System.Windows.Forms.Label();
            this.labelAuthor = new System.Windows.Forms.Label();
            this.linkLabelMe = new System.Windows.Forms.LinkLabel();
            this.labelLicenceType = new System.Windows.Forms.Label();
            this.labelMode = new System.Windows.Forms.Label();
            this.labelVersionNumber = new System.Windows.Forms.Label();
            this.labelVersion = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // labelProgramName
            // 
            this.labelProgramName.AutoSize = true;
            this.labelProgramName.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.labelProgramName.Location = new System.Drawing.Point(16, 16);
            this.labelProgramName.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelProgramName.Name = "labelProgramName";
            this.labelProgramName.Size = new System.Drawing.Size(241, 37);
            this.labelProgramName.TabIndex = 0;
            this.labelProgramName.Text = "Infinite Diagram";
            // 
            // buttonOk
            // 
            this.buttonOk.Location = new System.Drawing.Point(182, 107);
            this.buttonOk.Margin = new System.Windows.Forms.Padding(2);
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.Size = new System.Drawing.Size(56, 26);
            this.buttonOk.TabIndex = 1;
            this.buttonOk.Text = "OK";
            this.buttonOk.UseVisualStyleBackColor = true;
            this.buttonOk.Click += new System.EventHandler(this.button1_Click);
            // 
            // labelLicence
            // 
            this.labelLicence.AutoSize = true;
            this.labelLicence.Location = new System.Drawing.Point(28, 70);
            this.labelLicence.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelLicence.Name = "labelLicence";
            this.labelLicence.Size = new System.Drawing.Size(44, 13);
            this.labelLicence.TabIndex = 3;
            this.labelLicence.Text = "licence:";
            // 
            // labelAuthor
            // 
            this.labelAuthor.AutoSize = true;
            this.labelAuthor.Location = new System.Drawing.Point(28, 53);
            this.labelAuthor.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelAuthor.Name = "labelAuthor";
            this.labelAuthor.Size = new System.Drawing.Size(40, 13);
            this.labelAuthor.TabIndex = 4;
            this.labelAuthor.Text = "author:";
            // 
            // linkLabelMe
            // 
            this.linkLabelMe.AutoSize = true;
            this.linkLabelMe.Location = new System.Drawing.Point(114, 53);
            this.linkLabelMe.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.linkLabelMe.Name = "linkLabelMe";
            this.linkLabelMe.Size = new System.Drawing.Size(68, 13);
            this.linkLabelMe.TabIndex = 5;
            this.linkLabelMe.TabStop = true;
            this.linkLabelMe.Text = "Andrej Pekar";
            this.linkLabelMe.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // labelLicenceType
            // 
            this.labelLicenceType.AutoSize = true;
            this.labelLicenceType.Location = new System.Drawing.Point(114, 70);
            this.labelLicenceType.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelLicenceType.Name = "labelLicenceType";
            this.labelLicenceType.Size = new System.Drawing.Size(48, 13);
            this.labelLicenceType.TabIndex = 6;
            this.labelLicenceType.Text = "freeware";
            // 
            // labelMode
            // 
            this.labelMode.AutoSize = true;
            this.labelMode.Location = new System.Drawing.Point(28, 97);
            this.labelMode.Name = "labelMode";
            this.labelMode.Size = new System.Drawing.Size(33, 13);
            this.labelMode.TabIndex = 8;
            this.labelMode.Text = "mode";
            // 
            // labelVersionNumber
            // 
            this.labelVersionNumber.AutoSize = true;
            this.labelVersionNumber.Location = new System.Drawing.Point(113, 83);
            this.labelVersionNumber.Name = "labelVersionNumber";
            this.labelVersionNumber.Size = new System.Drawing.Size(43, 13);
            this.labelVersionNumber.TabIndex = 10;
            this.labelVersionNumber.Text = "0.0.000";
            // 
            // labelVersion
            // 
            this.labelVersion.AutoSize = true;
            this.labelVersion.Location = new System.Drawing.Point(28, 83);
            this.labelVersion.Name = "labelVersion";
            this.labelVersion.Size = new System.Drawing.Size(44, 13);
            this.labelVersion.TabIndex = 12;
            this.labelVersion.Text = "version:";
            // 
            // AboutForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(248, 143);
            this.Controls.Add(this.labelVersion);
            this.Controls.Add(this.labelVersionNumber);
            this.Controls.Add(this.labelMode);
            this.Controls.Add(this.labelLicenceType);
            this.Controls.Add(this.linkLabelMe);
            this.Controls.Add(this.labelAuthor);
            this.Controls.Add(this.labelLicence);
            this.Controls.Add(this.buttonOk);
            this.Controls.Add(this.labelProgramName);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = global::Diagram.Properties.Resources.ico_diagramico_forms;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AboutForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "About Infinite Diagram";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.AboutForm_FormClosed);
            this.Load += new System.EventHandler(this.AboutForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void AboutForm_Load(object sender, EventArgs e)
        {
            labelMode.Text = "Debug";
            #if DEBUG
                labelMode.Text = "Debug";
            #else
				labelMode.Text = "";
            #endif

            labelVersionNumber.Text = main.parameters.ApplicationVersion;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("mailto:pekand@gmail.com");
        }

        private void AboutForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            main.aboutForm = null;
        }
    }
}
