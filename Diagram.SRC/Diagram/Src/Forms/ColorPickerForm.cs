using System;
using System.Drawing;
using System.Windows.Forms;

namespace Diagram
{
    public partial class ColorPickerForm : Form //UID2354438225
    {
        public delegate void ColorPickerFormChangeColor(ColorType color);
        public event ColorPickerFormChangeColor changeColor;

        public ColorType color = new ColorType();

        private Bitmap bmp = null;

        bool selecting = false;

        private PictureBox pictureBox1;
        private Position position = new Position();

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
            this.Load += new System.EventHandler(this.ColorPickerForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

		}

        private int cv(int i)
        {
            return (int)(255.0 * ((i * 5.0) / 255.0));
        }

        private SolidBrush br(int r, int g, int b)
        {
            return new SolidBrush(Color.FromArgb(r, g, b));
        }

        private void rc(Graphics gr, int r, int g, int b, int x, int y)
        {
            gr.FillRectangle(br(r, g, b), x, y, 5, 5);
        }

        
        public void render()
        {
            bmp = new Bitmap(51 * 5 * 3, 51 * 5 * 3);
            Graphics g = Graphics.FromImage(bmp);

            // 1,1
            int px = 255 * 0;
            int py = 255 * 0;            
            for (int i = 0; i < 51; i++)
                for (int j = 0; j < 51; j++)
                    rc(g, 
                        0, cv(j), cv(i), 
                        px + i * 5, 
                        py + j * 5
                    );

            // 1,2
            px = 255 * 1;
            py = 255 * 0;
            for (int i = 0; i < 51; i++)
                for (int j = 0; j < 51; j++)
                    rc(g, 
                        cv(j), 0, cv(i), 
                        px + i * 5, 
                        py + j * 5
                    );

            // 1,3
            px = 255 * 2;
            py = 255 * 0;
            for (int i = 0; i < 51; i++)
                 for (int j = 0; j < 51; j++)
                     rc(g, 
                         cv(j), cv(i), 0, 
                         px + i * 5, 
                         py + j * 5
                     );

            // 2,1
            px = 255 * 0;
            py = 255 * 1;
            for (int i = 0; i < 51; i++)
                for (int j = 0; j < 51; j++)
                   rc(g, 
                       128, cv(j), cv(i), 
                       px + i * 5, 
                       py + j * 5
                   );

            // 2,2
            px = 255 * 1;
            py = 255 * 1;
            for (int i = 0; i < 51; i++)
                for (int j = 0; j < 51; j++)
                    rc(g,
                        cv(j), 128, cv(i), 
                        px + i * 5, 
                        py + j * 5
                    );

            // 2,3
            px = 255 * 2;
            py = 255 * 1;
            for (int i = 0; i < 51; i++)
                for (int j = 0; j < 51; j++)
                    rc(g, 
                        cv(j), cv(i), 128, 
                        px + i * 5, 
                        py + j * 5
                    );

            // 3,1
            px = 255 * 0;
            py = 255 * 2;
            for (int i = 0; i < 51; i++)
                for (int j = 0; j < 51; j++)
                    rc(g, 
                        255, cv(j), cv(i), 
                        px + i * 5, 
                        py + j * 5
                    );

            // 3,2
            px = 255 * 1;
            py = 255 * 2;
            for (int i = 0; i < 51; i++)
                for (int j = 0; j < 51; j++)
                    rc(g,
                        cv(j), 255, cv(i),
                        px + i * 5, 
                        py + j * 5
                    );

            // 3,3
            px = 255 * 2;
            py = 255 * 2;
            for (int i = 0; i < 51; i++)
                for (int j = 0; j < 51; j++)
                   rc(g,
                        cv(j), cv(i), 255,
                        px + i * 5, 
                        py + j * 5
                   );

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

        private Color convert(int x, int y)
        {
            int r,g,b;

            int t =  y * x;

            b = t % 256;
            g = t / 256 % 256;
            r = t / 256 / 256 % 256;

            return Color.FromArgb(r,g,b);
        }

        private Color convert(int t)
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
                this.color.Set(bmp.GetPixel(e.X, e.Y));
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
                    this.color.Set(bmp.GetPixel(e.X, e.Y));
                }

                if (this.changeColor != null)
                    this.changeColor(this.color);
            }
        }

        private void ColorPickerForm_Load(object sender, EventArgs e)
        {

        }
    }
}
