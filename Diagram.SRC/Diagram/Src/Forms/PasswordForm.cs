using System;
using System.Windows.Forms;

namespace Diagram
{
    public partial class PasswordForm : Form
    {
        public Main main = null;

        private System.Windows.Forms.Button buttonOk;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Label labelPassword;
        private System.Windows.Forms.TextBox editPassword;

        public bool cancled = false;

        public PasswordForm(Main main)
        {
            this.main = main;
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.buttonOk = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.labelPassword = new System.Windows.Forms.Label();
            this.editPassword = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // buttonOk
            // 
            this.buttonOk.Location = new System.Drawing.Point(79, 35);
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.Size = new System.Drawing.Size(75, 31);
            this.buttonOk.TabIndex = 4;
            this.buttonOk.Text = "Ok";
            this.buttonOk.UseVisualStyleBackColor = true;
            this.buttonOk.Click += new System.EventHandler(this.buttonOk_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(160, 35);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 32);
            this.buttonCancel.TabIndex = 5;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // labelPassword
            // 
            this.labelPassword.AutoSize = true;
            this.labelPassword.Location = new System.Drawing.Point(20, 12);
            this.labelPassword.Name = "labelPassword";
            this.labelPassword.Size = new System.Drawing.Size(56, 13);
            this.labelPassword.TabIndex = 6;
            this.labelPassword.Text = "Password:";
            // 
            // editPassword
            // 
            this.editPassword.Location = new System.Drawing.Point(79, 9);
            this.editPassword.Name = "editPassword";
            this.editPassword.Size = new System.Drawing.Size(283, 20);
            this.editPassword.TabIndex = 8;
            this.editPassword.UseSystemPasswordChar = true;
            this.editPassword.KeyDown += new System.Windows.Forms.KeyEventHandler(this.editPassword_KeyDown);
            // 
            // PasswordForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(371, 74);
            this.Controls.Add(this.editPassword);
            this.Controls.Add(this.labelPassword);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOk);
            this.Icon = global::Diagram.Properties.Resources.ico_diagramico_forms;
            this.Name = "PasswordForm";
            this.Text = "Password";
            this.Load += new System.EventHandler(this.PasswordForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void PasswordForm_Load(object sender, EventArgs e)
        {
            cancled = false;
            this.Left = (Screen.PrimaryScreen.Bounds.Width - this.Width)/ 2;
            this.Top = (Screen.PrimaryScreen.Bounds.Height - this.Height) / 2;
            this.ActiveControl = editPassword;
        }

        public void Clear()
        {
            this.editPassword.Text = "";
            cancled = false;
        }

        public string GetPassword() {
            return this.editPassword.Text;
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            this.cancled = false;
            this.Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.cancled = true;
            this.Close();
        }

        public void CloseForm()
        {
            main.passwordForm = null;
        }

        private void editPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                buttonOk_Click(sender, e);
            }
        }
    }
}
