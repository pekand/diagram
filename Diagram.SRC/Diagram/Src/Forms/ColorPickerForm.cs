using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
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

        Bitmap bmp = new Bitmap(1000,1000);

        bool selecting = false;

        public ColorPickerForm()
        {
            InitializeComponent();
        }

        private void ColorPickerForm_Load(object sender, EventArgs e)
        {
            Graphics g = Graphics.FromImage(bmp);

            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            

            for (int i = 0; i < 51; i++)
                for (int j = 0; j < 51; j++)
                    g.FillRectangle(new SolidBrush(Color.FromArgb(0, i * 5, j * 5)), i * 5, j * 5, 5, 5);

            for (int i = 0; i < 51; i++)
                for (int j = 0; j < 51; j++)
                    g.FillRectangle(new SolidBrush(Color.FromArgb(j * 5, i * 5, 0)), 255 + i * 5, j * 5, 5, 5);

            for (int i = 0; i < 51; i++)
                for (int j = 0; j < 51; j++)
                    g.FillRectangle(new SolidBrush(Color.FromArgb(i * 5, 0, j * 5)), 255*2 + i * 5, j * 5, 5, 5);

            for (int i = 0; i < 51; i++)
                for (int j = 0; j < 51; j++)
                    g.FillRectangle(new SolidBrush(Color.FromArgb(i * 5, j * 5, j * 5)), i * 5, 255 + j * 5, 5, 5);

            for (int i = 0; i < 51; i++)
                for (int j = 0; j < 51; j++)
                    g.FillRectangle(new SolidBrush(Color.FromArgb(i * 5, i * 5, j * 5)), 255 + i * 5, 255 + j * 5, 5, 5);

            for (int i = 0; i < 51; i++)
                for (int j = 0; j < 51; j++)
                    g.FillRectangle(new SolidBrush(Color.FromArgb(i * 5, i * 5, i * 5)), 255*2 + i * 5, 255 + j * 5, 5, 5);

            g.Flush();
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
                if (0 <= e.X && e.X <= bmp.Width && 0 <= e.Y && e.Y <= bmp.Height) {
                    this.color = bmp.GetPixel(e.X, e.Y);
                }

                if (this.changeColor != null)
                    this.changeColor(this.color);
            }
        }
    }
}
