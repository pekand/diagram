using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Diagram
{
    public class Tick
    {
        
        public static Timer timer(int interval, EventHandler tick)
        {
            Timer timer = new Timer();
            timer.Interval = 200;
            timer.Tick += tick;
            timer.Enabled = true;
            return timer;
        }
    }
}
