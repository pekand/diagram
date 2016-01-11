using System;
using System.Windows.Forms;

namespace Diagram
{
    public partial class Console : Form
    {
        public Main main = null;

        private System.Windows.Forms.RichTextBox logedit;

        public Console(Main main)
        {
            this.main = main;
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.logedit = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            //
            // logedit
            //
            this.logedit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.logedit.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.logedit.Location = new System.Drawing.Point(0, 0);
            this.logedit.Name = "logedit";
            this.logedit.Size = new System.Drawing.Size(284, 262);
            this.logedit.TabIndex = 0;
            this.logedit.Text = "";
            this.logedit.WordWrap = false;
            this.logedit.KeyUp += new System.Windows.Forms.KeyEventHandler(this.logedit_KeyUp);
            //
            // Console
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.logedit);
            this.Icon = global::Diagram.Properties.Resources.ico_diagramico_forms;
            this.Name = "Console";
            this.Text = "Console";
            this.TopMost = true;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Console_FormClosed);
            this.Load += new System.EventHandler(this.Console_Load);
            this.ResumeLayout(false);

        }

        private void Console_Load(object sender, EventArgs e)
        {
            logedit.Text = Program.log.getText();
        }

        public void refreshWindow()
        {
            logedit.Text = Program.log.getText();
        }

        private void Console_FormClosed(object sender, FormClosedEventArgs e)
        {
            main.console = null;
        }

        private void logedit_KeyUp(object sender, KeyEventArgs e)
        {
            Program.log.setText(this.logedit.Text);
        }
    }
}
