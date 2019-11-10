using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Diagram
{
    public class DisplayInfo : Control
    {
        delegate void SetTextCallback(string text);

        EventHandler resizeEvent = null;
        PaintEventHandler paintEvent = null;

        public DisplayInfo(Form form)
        {
            this.Parent = form;
            form.Controls.Add(this);            
            InitializeComponent();
            this.Parent.Invalidate();
        }

        public void SetText(string text)
        {
            if (this.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetText);
                this.Invoke(d, new object[] { text });
                (Parent as DiagramView).Invalidate();
            }
            else
            {
                this.Text = text;
            }
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            Control control = (Control)sender;          
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Font drawFont = new Font("Arial", 20);
            SolidBrush drawBrush = new SolidBrush(Color.Black);

            Size textSize = TextRenderer.MeasureText(this.Text, drawFont);

            int Left = this.Parent.Width / 2 - textSize.Width / 2;
            int Top = this.Parent.Height / 2 - textSize.Height / 2;

            PointF drawPoint = new PointF(Left, Top);

            e.Graphics.DrawString(this.Text, drawFont, drawBrush, drawPoint);
        }

        private void InitializeComponent()
        {
            resizeEvent = new EventHandler(this.Form1_Resize);
            paintEvent = new PaintEventHandler(this.Form1_Paint);

            this.Parent.Resize += resizeEvent;
            this.Parent.Paint += paintEvent;

            this.SuspendLayout();
            // 
            // panel1
            // 
            this.Location = new System.Drawing.Point(-10, -10);
            this.Name = "DispleyInfo";
            this.Size = new System.Drawing.Size(10, 10);
            this.TabIndex = 0;
            // 
            // Form1
            // 
            this.ClientSize = new System.Drawing.Size(10, 10);                                                                        
            this.ResumeLayout(false);

        }

        public void RemoveComponent()
        {
            this.Parent.Resize -= resizeEvent;
            this.Parent.Paint -= paintEvent;

            this.Parent.Controls.Remove(this);
        }
    }
}
