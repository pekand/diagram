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
        /// bring form to foreground </summary>
        public static void bringToFront(Form form)
        {
            Program.log.write("bringToFront");
            Tick.timer(200, (t, args) =>
            {
                if (t is Timer)
                {
                    Timer timer = t as Timer;

                    Program.log.write("bringToFront: tick");
                    ((Timer)t).Enabled = false;

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
            });
        }

        public static Bitmap extractSystemIcon(string path)
        {
#if !MONO
            try
            {
                Icon ico = Icon.ExtractAssociatedIcon(path);
                return ico.ToBitmap();
            }
            catch (Exception e)
            {
                Program.log.write("get exe icon error: " + e.Message);
            }

            return null;
#else
            return null;
#endif

        }

        public static Bitmap extractLnkIcon(string path)
        {
#if !MONO
            try
            {
                var shl = new Shell32.Shell();
                string lnkPath = System.IO.Path.GetFullPath(path);
                var dir = shl.NameSpace(System.IO.Path.GetDirectoryName(lnkPath));
                var itm = dir.Items().Item(System.IO.Path.GetFileName(lnkPath));
                var lnk = (Shell32.ShellLinkObject)itm.GetLink;

                String strIcon;
                lnk.GetIconLocation(out strIcon);
                Icon awIcon = Icon.ExtractAssociatedIcon(strIcon);

                return awIcon.ToBitmap();
            }
            catch (Exception e)
            {
                Program.log.write("get exe icon error: " + e.Message);
            }

            return null;
#else
            return null;
#endif

        }
    }
}
