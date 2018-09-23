using System;
using System.Windows.Forms;

namespace Diagram
{
    public partial class AboutForm : Form //UID4238351112
    {
        public Main main = null;

        private System.Windows.Forms.Label labelProgramName;
        private System.Windows.Forms.Button buttonOk;
        private System.Windows.Forms.Label labelLicence;
        private System.Windows.Forms.Label labelAuthor;
        private System.Windows.Forms.LinkLabel linkLabelMe;
        private System.Windows.Forms.Label labelLicenceType;
		private System.Windows.Forms.Label labelVersion;
        private System.Windows.Forms.Label labelVersionNumber;
        private System.Windows.Forms.LinkLabel labelHomepage;

        public AboutForm(Main main)
        {
            this.main = main;
            this.InitializeComponent();

            this.labelLicenceType.Text = this.main.options.license;
            this.labelVersionNumber.Text = Os.GetThisAssemblyVersion();
            this.linkLabelMe.Text = this.main.options.author; 
            this.labelHomepage.Text = this.main.options.home_page;
        }

        private void InitializeComponent()
        {
            this.labelProgramName = new System.Windows.Forms.Label();
            this.buttonOk = new System.Windows.Forms.Button();
            this.labelLicence = new System.Windows.Forms.Label();
            this.labelLicenceType = new System.Windows.Forms.Label();
            this.labelAuthor = new System.Windows.Forms.Label();
            this.linkLabelMe = new System.Windows.Forms.LinkLabel();
            this.labelVersion = new System.Windows.Forms.Label();
            this.labelVersionNumber = new System.Windows.Forms.Label();
            this.labelHomepage = new System.Windows.Forms.LinkLabel();
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
            this.buttonOk.Click += new System.EventHandler(this.ButtonOk_Click);
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
            // labelLicenceType
            // 
            this.labelLicenceType.AutoSize = true;
            this.labelLicenceType.Location = new System.Drawing.Point(114, 70);
            this.labelLicenceType.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelLicenceType.Name = "labelLicenceType";
            this.labelLicenceType.Size = new System.Drawing.Size(0, 13);
            this.labelLicenceType.TabIndex = 6;
            this.labelLicence.Text = "lic";
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
            this.linkLabelMe.Size = new System.Drawing.Size(38, 13);
            this.linkLabelMe.TabIndex = 5;
            this.linkLabelMe.TabStop = true;
            this.linkLabelMe.Text = "Author";
            this.linkLabelMe.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LinkLabelMe_LinkClicked);
            // 
            // labelVersion
            // 
            this.labelVersion.AutoSize = true;
            this.labelVersion.Location = new System.Drawing.Point(28, 87);
            this.labelVersion.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelVersion.Name = "labelVersion";
            this.labelVersion.Size = new System.Drawing.Size(44, 13);
            this.labelVersion.TabIndex = 3;
            this.labelVersion.Text = "version:";
            // 
            // labelVersionNumber
            // 
            this.labelVersionNumber.AutoSize = true;
            this.labelVersionNumber.Location = new System.Drawing.Point(114, 87);
            this.labelVersionNumber.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelVersionNumber.Name = "labelVersionNumber";
            this.labelVersionNumber.Size = new System.Drawing.Size(59, 13);
            this.labelVersionNumber.TabIndex = 3;
            this.labelVersionNumber.Text = "0.0";
            // 
            // labelHomepage
            // 
            this.labelHomepage.AutoSize = true;
            this.labelHomepage.Location = new System.Drawing.Point(28, 100);
            this.labelHomepage.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelHomepage.Name = "labelHomepage";
            this.labelHomepage.Size = new System.Drawing.Size(0, 13);
            this.labelHomepage.TabIndex = 3;
            this.labelHomepage.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LabelHomepage_HomepageClicked);
            // 
            // AboutForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(260, 150);
            this.Controls.Add(this.labelLicenceType);
            this.Controls.Add(this.linkLabelMe);
            this.Controls.Add(this.labelAuthor);
            this.Controls.Add(this.labelLicence);
            this.Controls.Add(this.labelVersion); 
            this.Controls.Add(this.labelVersionNumber);
            this.Controls.Add(this.labelHomepage);
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
        }

        private void ButtonOk_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void LinkLabelMe_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("mailto:pekand@gmail.com");
        }

        private void LabelHomepage_HomepageClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(this.main.options.home_page);
        }

        private void AboutForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            
        }
    }
}
