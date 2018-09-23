using System;
using System.Windows.Forms;

namespace Diagram
{
    public partial class UpdateForm : Form
    {

        private System.ComponentModel.IContainer components = null;
        
        private System.Windows.Forms.Label labelInfo;
        private System.Windows.Forms.Button buttonYes;
        private System.Windows.Forms.Button buttonNo;
        private System.Windows.Forms.CheckBox checkBoxNewVersion;
        private System.Windows.Forms.LinkLabel linkLabelVisit;
        private System.Windows.Forms.ImageList imageList1;

        private string update = "No";
        private string checkVersion = "Yes";

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.labelInfo = new System.Windows.Forms.Label();
            this.buttonYes = new System.Windows.Forms.Button();
            this.buttonNo = new System.Windows.Forms.Button();
            this.checkBoxNewVersion = new System.Windows.Forms.CheckBox();
            this.linkLabelVisit = new System.Windows.Forms.LinkLabel();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.SuspendLayout();
            // 
            // labelInfo
            // 
            this.labelInfo.AutoSize = true;
            this.labelInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelInfo.Location = new System.Drawing.Point(64, 31);
            this.labelInfo.Name = "labelInfo";
            this.labelInfo.Size = new System.Drawing.Size(323, 24);
            this.labelInfo.TabIndex = 0;
            this.labelInfo.Text = "Do you want install new version now?";
            // 
            // buttonYes
            // 
            this.buttonYes.Location = new System.Drawing.Point(276, 131);
            this.buttonYes.Name = "buttonYes";
            this.buttonYes.Size = new System.Drawing.Size(75, 23);
            this.buttonYes.TabIndex = 1;
            this.buttonYes.Text = "Yes";
            this.buttonYes.UseVisualStyleBackColor = true;
            this.buttonYes.Click += new System.EventHandler(this.buttonYes_Click);
            // 
            // buttonNo
            // 
            this.buttonNo.Location = new System.Drawing.Point(357, 131);
            this.buttonNo.Name = "buttonNo";
            this.buttonNo.Size = new System.Drawing.Size(87, 23);
            this.buttonNo.TabIndex = 2;
            this.buttonNo.Text = "No";
            this.buttonNo.UseVisualStyleBackColor = true;
            this.buttonNo.Click += new System.EventHandler(this.buttonNo_Click);
            // 
            // checkBoxNewVersion
            // 
            this.checkBoxNewVersion.AutoSize = true;
            this.checkBoxNewVersion.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxNewVersion.Location = new System.Drawing.Point(16, 130);
            this.checkBoxNewVersion.Name = "checkBoxNewVersion";
            this.checkBoxNewVersion.Size = new System.Drawing.Size(160, 17);
            this.checkBoxNewVersion.TabIndex = 3;
            this.checkBoxNewVersion.Text = "don\'t  check for new version";
            this.checkBoxNewVersion.UseVisualStyleBackColor = true;
            this.checkBoxNewVersion.CheckedChanged += new System.EventHandler(this.checkBoxNewVersion_CheckedChanged);
            // 
            // linkLabelVisit
            // 
            this.linkLabelVisit.AutoSize = true;
            this.linkLabelVisit.Location = new System.Drawing.Point(208, 68);
            this.linkLabelVisit.Name = "linkLabelVisit";
            this.linkLabelVisit.Size = new System.Drawing.Size(179, 13);
            this.linkLabelVisit.TabIndex = 5;
            this.linkLabelVisit.TabStop = true;
            this.linkLabelVisit.Text = "Visit homepage for more informations";
            // 
            // imageList1
            // 
            this.imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imageList1.ImageSize = new System.Drawing.Size(16, 16);
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // UpdateForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(456, 166);
            this.Controls.Add(this.linkLabelVisit);
            this.Controls.Add(this.checkBoxNewVersion);
            this.Controls.Add(this.buttonNo);
            this.Controls.Add(this.buttonYes);
            this.Controls.Add(this.labelInfo);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "UpdateForm";
            this.Text = "New version is available";
            this.Shown += new System.EventHandler(this.UpdateForm_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public UpdateForm()
        {
            InitializeComponent();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void buttonYes_Click(object sender, EventArgs e)
        {
            this.update = "Yes";
            this.Close();
        }

        private void buttonNo_Click(object sender, EventArgs e)
        {
            this.update = "No";
            this.Close();
        }

        public bool CanUpdate()
        {
            return this.update == "Yes";
        }

        private void checkBoxNewVersion_CheckedChanged(object sender, EventArgs e)
        {
            this.checkVersion = "Yes";
            if (checkBoxNewVersion.Checked) {
                this.checkVersion = "No";
            }
        }

        public bool CanCheckVersion()
        {
            return this.checkVersion == "Yes";
        }

        private void UpdateForm_Shown(object sender, EventArgs e)
        {
            this.update = "No";
            this.checkVersion = "Yes";

            this.CenterToScreen();
        }
    }
}
