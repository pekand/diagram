using System.Drawing;
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
    }
}
