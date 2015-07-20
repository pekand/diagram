using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            // 
            // panel2
            // 
            this.panel2 = new SearchPanel(this);
            this.panel2.Location = new System.Drawing.Point(10, 10);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(106, 25);
            this.panel2.TabIndex = 0;
            this.panel2.SearchpanelStateChanged += this.SearchPanelChanged;
            this.Controls.Add(this.panel2);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Resize(object sender, EventArgs e)
        {

        }

        public void SearchPanelChanged(string action, string search) 
        {
            if (action == "search")
            {
                this.BackColor = System.Drawing.Color.DarkBlue;
            }
        }

        private void Form1_Shown(object sender, EventArgs e)
        {

        }

        private void panel1_VisibleChanged(object sender, EventArgs e)
        {

        }

    }
}
