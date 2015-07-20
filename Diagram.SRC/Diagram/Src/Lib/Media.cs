using System.Drawing;
using System.Windows.Forms;

namespace Diagram
{
    class Media
    {
        // SCREEN RESOLUTION
        //TODO Find better solution to find screen resolution on computer with multiple screens
        public static int screenWidth(Form form)
        {
            return ((Screen.FromControl(form).Bounds.Size.Width > 1920) ? 1920 : Screen.FromControl(form).Bounds.Size.Width);
        }

        public static int screenHeight(Form form)
        {
            return Screen.FromControl(form).Bounds.Size.Height;
        }

        public static Color getColor(string color)
        {
            return System.Drawing.ColorTranslator.FromHtml(color);
        }
    }
}
