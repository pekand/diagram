using System.Windows.Forms;

namespace Diagram
{
    partial class ColorPickerForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // ColorPickerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(765, 510);
            this.MaximizeBox = false;
            this.Name = "ColorPickerForm";
            this.Text = "Color";
            this.Load += new System.EventHandler(this.ColorPickerForm_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.PaintColorPicker);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ColorPickerForm_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ColorPickerForm_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ColorPickerForm_MouseUp);
            this.ResumeLayout(false);

        }

        #endregion
    }
}