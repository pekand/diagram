using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Diagram
{
    class Media
    {
        /// <summary>
        /// get active form screen width </summary>
        public static int screenWidth(Form form)
        {
            return ((Screen.FromControl(form).Bounds.Size.Width > 1920) ? 1920 : Screen.FromControl(form).Bounds.Size.Width);
        }

        /// <summary>
        /// get active form screenn height </summary>
        public static int screenHeight(Form form)
        {
            return Screen.FromControl(form).Bounds.Size.Height;
        }

        /// <summary>
        /// convert hexidecimal html color to Color object </summary>
        public static Color getColor(string color)
        {
            return System.Drawing.ColorTranslator.FromHtml(color);
        }

        /// <summary>
        /// load image from file </summary>
        public static Bitmap getImage(string file)
        {
            try
            {
                return new Bitmap(file);
            }
            catch (Exception e)
            {
                Program.log.write("getImage: " + e.Message);
            }

            return null;
        }

        [DllImport("User32.dll")]
        public static extern Int32 SetForegroundWindow(int hWnd);
        /// <summary>
        /// load image from file </summary>
        public static void bringToFront(Form form)
        {
            SetForegroundWindow(form.Handle.ToInt32());
        }

            
    }
}
