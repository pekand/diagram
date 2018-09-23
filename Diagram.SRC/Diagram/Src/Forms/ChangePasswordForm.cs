using System;
using System.Security;
using System.Windows.Forms;

namespace Diagram
{
    public class ChangePasswordForm : Form //UID9355910334
    {
        public Main main = null;

        private System.Windows.Forms.Label labelOldPassword;
        private System.Windows.Forms.Label labelNewPassword1;
        private System.Windows.Forms.Label labelNewPassword2;
        private System.Windows.Forms.TextBox editOldPassword;
        private System.Windows.Forms.TextBox editNewPassword1;
        private System.Windows.Forms.TextBox editNewPassword2;
        private System.Windows.Forms.Button buttonOk;
        private System.Windows.Forms.Button buttonCancel;

        public bool cancled = false;
        public SecureString oldpassword = null;
        public bool buttonok = false;

        public ChangePasswordForm(Main main)
        {
            this.main = main;
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.labelOldPassword = new System.Windows.Forms.Label();
            this.labelNewPassword1 = new System.Windows.Forms.Label();
            this.labelNewPassword2 = new System.Windows.Forms.Label();
            this.editOldPassword = new System.Windows.Forms.TextBox();
            this.editNewPassword1 = new System.Windows.Forms.TextBox();
            this.editNewPassword2 = new System.Windows.Forms.TextBox();
            this.buttonOk = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            //
            // labelOldPassword
            //
            this.labelOldPassword.AutoSize = true;
            this.labelOldPassword.Location = new System.Drawing.Point(28, 23);
            this.labelOldPassword.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelOldPassword.Name = "labelOldPassword";
            this.labelOldPassword.Size = new System.Drawing.Size(98, 17);
            this.labelOldPassword.TabIndex = 0;
            this.labelOldPassword.Text = "Old password:";
            //
            // labelNewPassword1
            //
            this.labelNewPassword1.AutoSize = true;
            this.labelNewPassword1.Location = new System.Drawing.Point(20, 55);
            this.labelNewPassword1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelNewPassword1.Name = "labelNewPassword1";
            this.labelNewPassword1.Size = new System.Drawing.Size(103, 17);
            this.labelNewPassword1.TabIndex = 1;
            this.labelNewPassword1.Text = "New password:";
            //
            // labelNewPassword2
            //
            this.labelNewPassword2.AutoSize = true;
            this.labelNewPassword2.Location = new System.Drawing.Point(20, 87);
            this.labelNewPassword2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelNewPassword2.Name = "labelNewPassword2";
            this.labelNewPassword2.Size = new System.Drawing.Size(103, 17);
            this.labelNewPassword2.TabIndex = 2;
            this.labelNewPassword2.Text = "New password:";
            //
            // editOldPassword
            //
            this.editOldPassword.Location = new System.Drawing.Point(135, 20);
            this.editOldPassword.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.editOldPassword.Name = "editOldPassword";
            this.editOldPassword.Size = new System.Drawing.Size(331, 22);
            this.editOldPassword.TabIndex = 3;
            this.editOldPassword.UseSystemPasswordChar = true;
            //
            // editNewPassword1
            //
            this.editNewPassword1.Location = new System.Drawing.Point(135, 52);
            this.editNewPassword1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.editNewPassword1.Name = "editNewPassword1";
            this.editNewPassword1.Size = new System.Drawing.Size(331, 22);
            this.editNewPassword1.TabIndex = 4;
            this.editNewPassword1.UseSystemPasswordChar = true;
            //
            // editNewPassword2
            //
            this.editNewPassword2.Location = new System.Drawing.Point(135, 84);
            this.editNewPassword2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.editNewPassword2.Name = "editNewPassword2";
            this.editNewPassword2.Size = new System.Drawing.Size(329, 22);
            this.editNewPassword2.TabIndex = 5;
            this.editNewPassword2.UseSystemPasswordChar = true;
            //
            // buttonOk
            //
            this.buttonOk.Location = new System.Drawing.Point(133, 116);
            this.buttonOk.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.Size = new System.Drawing.Size(85, 34);
            this.buttonOk.TabIndex = 6;
            this.buttonOk.Text = "Ok";
            this.buttonOk.UseVisualStyleBackColor = true;
            this.buttonOk.Click += new System.EventHandler(this.ButtonOk_Click);
            //
            // buttonCancel
            //
            this.buttonCancel.Location = new System.Drawing.Point(227, 116);
            this.buttonCancel.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(96, 34);
            this.buttonCancel.TabIndex = 7;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.ButtonCancel_Click);
            //
            // ChangePasswordForm
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(483, 158);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOk);
            this.Controls.Add(this.editNewPassword2);
            this.Controls.Add(this.editNewPassword1);
            this.Controls.Add(this.editOldPassword);
            this.Controls.Add(this.labelNewPassword2);
            this.Controls.Add(this.labelNewPassword1);
            this.Controls.Add(this.labelOldPassword);
            this.Icon = Properties.Resources.ico_diagramico_forms;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "ChangePasswordForm";
            this.Text = "ChangePasswordForm";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ChangePasswordForm_FormClosed);
            this.Load += new System.EventHandler(this.ChangePasswordForm_Load);

        }

        public void Clear()
        {
            this.editOldPassword.Text = "";
            this.editNewPassword1.Text = "";
            this.editNewPassword2.Text = "";
            cancled = false;
            buttonok = false;
            this.oldpassword = null;
        }

        public string GetPassword()
        {
            return this.editNewPassword1.Text;
        }


        private void ButtonOk_Click(object sender, EventArgs e)
        {
            if (!Encrypt.CompareSecureString(this.oldpassword, this.editOldPassword.Text))
            {
                MessageBox.Show("Old password is incorrect!");
                return;
            }

            if (this.editNewPassword1.Text != this.editNewPassword2.Text)
            {
                MessageBox.Show("The new password does not match!");
                return;
            }

            if (this.editNewPassword1.Text == "" && MessageBox.Show("Really remove password?", "Confirm remove password", MessageBoxButtons.YesNo) == DialogResult.No)
            {
                return;
            }

            this.cancled = false;
            buttonok = true;
            this.Close();
        }

        private void ButtonCancel_Click(object sender, EventArgs e)
        {
            this.cancled = true;
            this.Close();
        }

        private void ChangePasswordForm_Load(object sender, EventArgs e)
        {
            cancled = false;
            this.Left = (Screen.PrimaryScreen.Bounds.Width - this.Width) / 2;
            this.Top = (Screen.PrimaryScreen.Bounds.Height - this.Height) / 2;
            this.ActiveControl = editOldPassword;
        }

        private void ChangePasswordForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            cancled = true;

            if (buttonok)
            {
                cancled = false;
            }
        }

    }
}
