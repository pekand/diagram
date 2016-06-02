using System;
using System.Drawing;
using System.Windows.Forms;

#if !MONO
using System.Runtime.InteropServices;
#endif

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

#if !MONO
        [DllImport("User32.dll")]
        public static extern Int32 SetForegroundWindow(int hWnd);
#endif
        /// <summary>
        /// load image from file </summary>
        public static void bringToFront(Form form)
        {
            //diagram bring to top hack in windows
            if (form.WindowState == FormWindowState.Minimized)
            {
                form.WindowState = FormWindowState.Normal;
            }

#if !MONO
            SetForegroundWindow(form.Handle.ToInt32());
#endif
            form.TopMost = true;
            form.Focus();
            form.BringToFront();
            form.TopMost = false;
            form.Activate();
        }

    }
}
