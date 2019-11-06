using System;
using System.Windows.Forms;

namespace Diagram
{
    public partial class Console : Form //UID4944902991
    {
        private TabControl tabControl1;
        private TabPage tabConsole;
        private RichTextBox logedit;
        private TabPage tabFeatures;
        private RichTextBox featuresTextBox;
        public Main main = null;

        public Console(Main main)
        {
            this.main = main;
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabConsole = new System.Windows.Forms.TabPage();
            this.tabFeatures = new System.Windows.Forms.TabPage();
            this.logedit = new System.Windows.Forms.RichTextBox();
            this.featuresTextBox = new System.Windows.Forms.RichTextBox();
            this.tabControl1.SuspendLayout();
            this.tabConsole.SuspendLayout();
            this.tabFeatures.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabConsole);
            this.tabControl1.Controls.Add(this.tabFeatures);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(451, 416);
            this.tabControl1.TabIndex = 0;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // tabConsole
            // 
            this.tabConsole.Controls.Add(this.logedit);
            this.tabConsole.Location = new System.Drawing.Point(4, 22);
            this.tabConsole.Name = "tabConsole";
            this.tabConsole.Padding = new System.Windows.Forms.Padding(3);
            this.tabConsole.Size = new System.Drawing.Size(443, 390);
            this.tabConsole.TabIndex = 0;
            this.tabConsole.Text = "Console";
            this.tabConsole.UseVisualStyleBackColor = true;
            // 
            // tabFeatures
            // 
            this.tabFeatures.Controls.Add(this.featuresTextBox);
            this.tabFeatures.Location = new System.Drawing.Point(4, 22);
            this.tabFeatures.Name = "tabFeatures";
            this.tabFeatures.Padding = new System.Windows.Forms.Padding(3);
            this.tabFeatures.Size = new System.Drawing.Size(443, 390);
            this.tabFeatures.TabIndex = 1;
            this.tabFeatures.Text = "Features";
            this.tabFeatures.UseVisualStyleBackColor = true;
            // 
            // logedit
            // 
            this.logedit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.logedit.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.logedit.Location = new System.Drawing.Point(3, 3);
            this.logedit.Name = "logedit";
            this.logedit.Size = new System.Drawing.Size(437, 384);
            this.logedit.TabIndex = 1;
            this.logedit.Text = "";
            this.logedit.WordWrap = false;
            // 
            // featuresTextBox
            // 
            this.featuresTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.featuresTextBox.Location = new System.Drawing.Point(3, 3);
            this.featuresTextBox.Name = "featuresTextBox";
            this.featuresTextBox.ReadOnly = true;
            this.featuresTextBox.Size = new System.Drawing.Size(437, 384);
            this.featuresTextBox.TabIndex = 0;
            this.featuresTextBox.Text = "";
            // 
            // Console
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(451, 416);
            this.Controls.Add(this.tabControl1);
            this.Icon = global::Diagram.Properties.Resources.ico_diagramico_forms;
            this.Name = "Console";
            this.Text = "Console";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.Console_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabConsole.ResumeLayout(false);
            this.tabFeatures.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        private void Console_Load(object sender, EventArgs e)
        {
            logedit.Text = Program.log.GetText();
        }

        public void RefreshWindow()
        {
            logedit.Text = Program.log.GetText();
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (tabControl1.SelectedTab == tabControl1.TabPages["tabFeatures"])//your specific tabname
            {
                featuresTextBox.Text = Features.getFeatures();
            }

        }
    }
}
