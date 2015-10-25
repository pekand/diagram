using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Diagram
{
    public class KeyMap
    {
        public static bool parseKey(string key, Keys keyData)
        {

            string[] parts = key.Split('+');
            Keys keyCode = 0;
            Keys code = 0;

            foreach (string part in parts)
            {
                if (part == "CTRL")
                {
                    keyCode = Keys.Control | keyCode;
                    continue;
                }

                if (part == "ALT")
                {
                    keyCode = Keys.Alt | keyCode;
                    continue;
                }

                if (part == "SHIFT")
                {
                    keyCode = Keys.Shift | keyCode;
                    continue;
                }

                if (part == "PAGEUP")
                {
                    keyCode = Keys.PageUp | keyCode;
                    continue;
                }

                if (part == "PAGEDOWN")
                {
                    keyCode = Keys.PageDown | keyCode;
                    continue;
                }

                if (part == "INS")
                {
                    keyCode = Keys.Insert | keyCode;
                    continue;
                }

                if (part == "DEL")
                {
                    keyCode = Keys.Delete | keyCode;
                    continue;
                }

                if (Enum.TryParse(Fonts.FirstCharToUpper(part), out code))
                {
                    keyCode = code | keyCode;
                }
            }

            if (keyCode == keyData)
            {
                return true;
            }

            return false;
        }
    }
}
