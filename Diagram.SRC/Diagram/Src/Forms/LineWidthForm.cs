using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Diagram
{
    public partial class LineWidthForm : Form
    {
        public delegate void LineWidthFormTrackbarChangedEventHandler(int value);
        public event LineWidthFormTrackbarChangedEventHandler trackbarStateChanged;

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

        public void setValue(int value)
        {
            this.trackBar1.Value = value;
        }
        
    }
}
