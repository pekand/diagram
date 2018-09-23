using System;
using System.Windows.Forms;

namespace Diagram
{
    public partial class LineWidthForm : Form //UID4672738884
    {
        public delegate void LineWidthFormTrackbarChangedEventHandler(int value);
        public event LineWidthFormTrackbarChangedEventHandler trackbarStateChanged;
		private System.Windows.Forms.TrackBar trackBar1;

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.trackBar1 = new System.Windows.Forms.TrackBar();
			((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
			this.SuspendLayout();
			// 
			// trackBar1
			// 
			this.trackBar1.Location = new System.Drawing.Point(12, 12);
			this.trackBar1.Minimum = 1;
			this.trackBar1.Name = "trackBar1";
			this.trackBar1.Size = new System.Drawing.Size(432, 45);
			this.trackBar1.TabIndex = 0;
			this.trackBar1.Value = 1;
			this.trackBar1.ValueChanged += new System.EventHandler(this.trackBar1_ValueChanged);
			// 
			// LineWidthForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(469, 66);
			this.Controls.Add(this.trackBar1);
			this.Icon = global::Diagram.Properties.Resources.ico_diagramico_forms;
			this.MaximizeBox = false;
			this.Name = "LineWidthForm";
			this.Text = "Line width";
			this.Load += new System.EventHandler(this.LineWidthForm_Load);
			((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

        public LineWidthForm()
        {
            InitializeComponent();
        }

        public void LineWidthForm_Load(object sender, EventArgs e)
        {

        }

        public void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            if (this.trackbarStateChanged != null)
                this.trackbarStateChanged(this.trackBar1.Value);
        }

        public void setValue(long value)
        {
            this.trackBar1.Value = (int)value; // TODO: scale long to int
        }
        
    }
}
