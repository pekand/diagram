using System;
using System.Windows.Forms;

namespace Diagram
{
    public partial class NewPasswordForm : Form //UID2816442898
    {
        public Main main = null;

        private System.Windows.Forms.Label labelNewPassword1;
        private System.Windows.Forms.Label labelNewPassword2;
        private System.Windows.Forms.TextBox editNewPassword1;
        private System.Windows.Forms.TextBox editNewPassword2;
        private System.Windows.Forms.Button buttonOk;
        private System.Windows.Forms.Button buttonCancel;

        public bool cancled = false;
        public bool buttonok = false;

        public NewPasswordForm(Main main)
        {
            this.main = main;
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.labelNewPassword1 = new System.Windows.Forms.Label();
            this.labelNewPassword2 = new System.Windows.Forms.Label();
            this.editNewPassword1 = new System.Windows.Forms.TextBox();
            this.editNewPassword2 = new System.Windows.Forms.TextBox();
            this.buttonOk = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            //
            // labelNewPassword1
            //
            this.labelNewPassword1.AutoSize = true;
            this.labelNewPassword1.Location = new System.Drawing.Point(16, 31);
            this.labelNewPassword1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelNewPassword1.Name = "labelNewPassword1";
            this.labelNewPassword1.Size = new System.Drawing.Size(103, 17);
            this.labelNewPassword1.TabIndex = 2;
            this.labelNewPassword1.Text = "New password:";
            //
            // labelNewPassword2
            //
            this.labelNewPassword2.AutoSize = true;
            this.labelNewPassword2.Location = new System.Drawing.Point(16, 63);
            this.labelNewPassword2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelNewPassword2.Name = "labelNewPassword2";
            this.labelNewPassword2.Size = new System.Drawing.Size(103, 17);
            this.labelNewPassword2.TabIndex = 3;
            this.labelNewPassword2.Text = "New password:";
            //
            // editNewPassword1
            //
            this.editNewPassword1.Location = new System.Drawing.Point(144, 27);
            this.editNewPassword1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.editNewPassword1.Name = "editNewPassword1";
            this.editNewPassword1.Size = new System.Drawing.Size(347, 22);
            this.editNewPassword1.TabIndex = 5;
            this.editNewPassword1.UseSystemPasswordChar = true;
            //
            // editNewPassword2
            //
            this.editNewPassword2.Location = new System.Drawing.Point(144, 59);
            this.editNewPassword2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.editNewPassword2.Name = "editNewPassword2";
            this.editNewPassword2.Size = new System.Drawing.Size(347, 22);
            this.editNewPassword2.TabIndex = 6;
            this.editNewPassword2.UseSystemPasswordChar = true;
            //
            // buttonOk
            //
            this.buttonOk.Location = new System.Drawing.Point(144, 91);
            this.buttonOk.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.Size = new System.Drawing.Size(85, 34);
            this.buttonOk.TabIndex = 7;
            this.buttonOk.Text = "Ok";
            this.buttonOk.UseVisualStyleBackColor = true;
            this.buttonOk.Click += new System.EventHandler(this.buttonOk_Click);
            //
            // buttonCancel
            //
            this.buttonCancel.Location = new System.Drawing.Point(237, 91);
            this.buttonCancel.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(96, 34);
            this.buttonCancel.TabIndex = 8;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            //
            // NewPasswordForm
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(508, 135);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOk);
            this.Controls.Add(this.editNewPassword2);
            this.Controls.Add(this.editNewPassword1);
            this.Controls.Add(this.labelNewPassword2);
            this.Controls.Add(this.labelNewPassword1);
            this.Icon = Properties.Resources.ico_diagramico_forms;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "NewPasswordForm";
            this.Text = "NewPasswordForm";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.NewPasswordForm_FormClosed);
            this.Load += new System.EventHandler(this.NewPasswordForm_Load);
        }

        public void Clear()
        {
            this.editNewPassword1.Text = "";
            this.editNewPassword2.Text = "";
            buttonok = false;
            cancled = false;
        }

        public string GetPassword()
        {
            return this.editNewPassword1.Text;
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            if (this.editNewPassword1.Text != this.editNewPassword2.Text)
            {
                MessageBox.Show("The new password does not match!");
                return;
            }

            this.cancled = false;
            buttonok = true;
            this.Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.cancled = true;
            this.Close();
        }

        private void NewPasswordForm_Load(object sender, EventArgs e)
        {
            cancled = false;
            this.Left = (Screen.PrimaryScreen.Bounds.Width - this.Width) / 2;
            this.Top = (Screen.PrimaryScreen.Bounds.Height - this.Height) / 2;
            this.ActiveControl = editNewPassword1;
        }

        private void NewPasswordForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            cancled = true;

            if (buttonok)
            {
                cancled = false;
            }
        }

    }
}
