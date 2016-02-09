using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Diagram
{
    public partial class ColorPickerForm : Form
    {
        public delegate void ColorPickerFormChangeColor(Color color);
        public event ColorPickerFormChangeColor changeColor;

        public Color color;

        Bitmap bmp = null;

        bool selecting = false;

        int b = 0;

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
			this.ClientSize = new System.Drawing.Size(765, 765);
			this.Icon = global::Diagram.Properties.Resources.ico_diagramico_forms;
			this.MaximizeBox = false;
			this.Name = "ColorPickerForm";
			this.Text = "Color";
			this.Load += new System.EventHandler(this.ColorPickerForm_Load);
			this.Paint += new System.Windows.Forms.PaintEventHandler(this.PaintColorPicker);
			this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ColorPickerForm_MouseDown);
			this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ColorPickerForm_MouseMove);
			this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ColorPickerForm_MouseUp);
			this.MouseWheel += new MouseEventHandler(DiagramApp_MouseWheel);
			this.ResumeLayout(false);

		}

		#endregion

        public void render()
        {
            bmp = new Bitmap(51 * 5 * 3, 51 * 5 * 3);
            Graphics g = Graphics.FromImage(bmp);


            for (int i = 0; i < 51; i++)
                for (int j = 0; j < 51; j++)
                    g.FillRectangle(new SolidBrush(Color.FromArgb(0 + b, i * 5, j * 5)), i * 5, j * 5, 5, 5);

            for (int i = 0; i < 51; i++)
                for (int j = 0; j < 51; j++)
                    g.FillRectangle(new SolidBrush(Color.FromArgb(j * 5, i * 5, 0 + b)), 255 + i * 5, j * 5, 5, 5);

            for (int i = 0; i < 51; i++)
                for (int j = 0; j < 51; j++)
                    g.FillRectangle(new SolidBrush(Color.FromArgb(i * 5, 0 + b, j * 5)), 255 * 2 + i * 5, j * 5, 5, 5);

            for (int i = 0; i < 51; i++)
                for (int j = 0; j < 51; j++)
                    g.FillRectangle(new SolidBrush(Color.FromArgb(i * 5, j * 5, j * 5)), i * 5, 255 + j * 5, 5, 5);

            for (int i = 0; i < 51; i++)
                for (int j = 0; j < 51; j++)
                    g.FillRectangle(new SolidBrush(Color.FromArgb(i * 5, i * 5, j * 5)), 255 + i * 5, 255 + j * 5, 5, 5);

            for (int i = 0; i < 51; i++)
                for (int j = 0; j < 51; j++)
                    g.FillRectangle(new SolidBrush(Color.FromArgb(i * 5, i * 5, i * 5)), 255 * 2 + i * 5, 255 + j * 5, 5, 5);

            for (int i = 0; i < 51; i++)
                for (int j = 0; j < 51; j++)
                    g.FillRectangle(new SolidBrush(Color.FromArgb(255 - b, i * 5, j * 5)), i * 5, 255 * 2 + j * 5, 5, 5);

            for (int i = 0; i < 51; i++)
                for (int j = 0; j < 51; j++)
                    g.FillRectangle(new SolidBrush(Color.FromArgb(j * 5, i * 5, 255 - b)), 255 + i * 5, 255 * 2 + j * 5, 5, 5);

            for (int i = 0; i < 51; i++)
                for (int j = 0; j < 51; j++)
                    g.FillRectangle(new SolidBrush(Color.FromArgb(i * 5, 255 - b, j * 5)), 255 * 2 + i * 5, 255 * 2 + j * 5, 5, 5);

            g.Flush();
        }

        public ColorPickerForm()
        {

            render();
            InitializeComponent();
        }

        Color convert(int x, int y)
        {
            int r,g,b;

            int t =  y * x;

            b = t % 256;
            g = t / 256 % 256;
            r = t / 256 / 256 % 256;

            return Color.FromArgb(r,g,b);
        }

        Color convert(int t)
        {
            int r, g, b;

            b = t % 256;
            g = t / 256 % 256;
            r = t / 256 / 256 % 256;

            return Color.FromArgb(r, g, b);
        }

        private void ColorPickerForm_Load(object sender, EventArgs e)
        {
            
        }

        public void PaintColorPicker(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawImage(bmp, 0, 0);
        }

        private void ColorPickerForm_MouseUp(object sender, MouseEventArgs e)
        {
            selecting = false;

            if (0 <= e.X && e.X <= bmp.Width && 0 <= e.Y && e.Y <= bmp.Height)
            {
                this.color = bmp.GetPixel(e.X, e.Y);
            }

            if (this.changeColor != null)
                this.changeColor(this.color);
        }

        private void ColorPickerForm_MouseDown(object sender, MouseEventArgs e)
        {
            selecting = true;
        }

        private void ColorPickerForm_MouseMove(object sender, MouseEventArgs e)
        {
            if (selecting)
            {
                if (0 < e.X && e.X < bmp.Width && 0 < e.Y && e.Y < bmp.Height) {
                    this.color = bmp.GetPixel(e.X, e.Y);
                }

                if (this.changeColor != null)
                    this.changeColor(this.color);
            }
        }
        // EVENT Mouse Whell
        public void DiagramApp_MouseWheel(object sender, MouseEventArgs e)                             // [MOUSE] [WHELL] [EVENT]
        {
            /*if (e.Delta > 0)
            {
                b = b + 5;
            }
            else
            {
                b = b - 5;
            }

            if (b < 0) b = 0;
            if (b > 255) b = 255; 
            render();
            InitializeComponent();*/
        }
    }
}
