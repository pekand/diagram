using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diagram
{

    /// <summary>
    /// color type encapsulation</summary>
    public class ColorType
    {
        public Color color = System.Drawing.Color.Black;

        /*************************************************************************************************************************/
        // CONSTRUCTORS

        public ColorType()
        {
        }

        public ColorType(string htmlColor)
        {
            this.color = System.Drawing.ColorTranslator.FromHtml(htmlColor);
        }

        public ColorType(Color color)
        {
            this.color = color;
        }

        public ColorType(ColorType colorType)
        {
            this.color = colorType.color;
        }

        /*************************************************************************************************************************/
        // SETERS AND GETERS

        public void set(ColorType colorType)
        {
            this.color = colorType.color;
        }

        public void set(string htmlColor)
        {
            this.color = System.Drawing.ColorTranslator.FromHtml(htmlColor);
        }

        public void set(Color color)
        {
            this.color = color;
        }

        public Color get()
        {
            return color;
        }

        /*************************************************************************************************************************/
        // CONVERSION

        /// <summary>
        /// convert system color to string</summary>
        public override string ToString()
        {
            return System.Drawing.ColorTranslator.ToHtml(color);
        }
    }
}
