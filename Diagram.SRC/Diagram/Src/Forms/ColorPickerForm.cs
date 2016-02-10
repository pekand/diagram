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
        private PictureBox pictureBox1;
        Position position = new Position();

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(391, 427);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ColorPickerForm_MouseDown);
            this.pictureBox1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ColorPickerForm_MouseMove);
            this.pictureBox1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ColorPickerForm_MouseUp);
            // 
            // ColorPickerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(808, 248);
            this.Controls.Add(this.pictureBox1);
            this.Icon = global::Diagram.Properties.Resources.ico_diagramico_forms;
            this.MaximizeBox = false;
            this.Name = "ColorPickerForm";
            this.Text = "Color";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

		}

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
            InitializeComponent();

            // draw image into box
            render();
            pictureBox1.Image = bmp;

            // create scrollbar
            pictureBox1.Width = bmp.Width;
            pictureBox1.Height = bmp.Height;
            this.Width = bmp.Width+17;
            this.Height = 255;

            this.Left = Screen.FromControl(this).Bounds.Width /2 - this.Width/2;
            this.Top = Screen.FromControl(this).Bounds.Height - this.Height - 100;
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
    }
}
