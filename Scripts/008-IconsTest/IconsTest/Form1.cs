using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IconsTest
{
    public partial class Form1 : Form
    {
        public string IconToBytes(Icon icon)
        {
            try
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    icon.Save(ms);
                    byte[] bytes = ms.ToArray();
                    return Convert.ToBase64String(bytes);
                }
            }
            catch (Exception e)
            {
                return "";
            }
        }

        public Icon BytesToIcon(string icon)
        {
            try
            {
                byte[] bytes = Convert.FromBase64String(icon);
                using (MemoryStream ms = new MemoryStream(bytes))
                {
                    return new Icon(ms);
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            form2.Show();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Icon icon = new Icon("b.ico");
            string b4 = this.IconToBytes(icon);
            this.Icon = this.BytesToIcon(b4);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Icon icon = new Icon("a.ico");
            string b4 = this.IconToBytes(icon);
            this.Icon = this.BytesToIcon(b4);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Icon icon = new Icon("b.ico");
            string b4 = this.IconToBytes(icon);
            this.Icon = this.BytesToIcon(b4);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            OpenFileDialog openIconDialog= new OpenFileDialog();

            openIconDialog.Filter = "icons (*.ico)|*.ico";
            openIconDialog.FilterIndex = 1;
            openIconDialog.RestoreDirectory = true;

            if (openIconDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    Icon icon = new Icon(openIconDialog.FileName);
                    string b4 = this.IconToBytes(icon);
                    this.Icon = this.BytesToIcon(b4);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }
        }
    }
}
