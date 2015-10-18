using System;
using System.Drawing;
using System.Xml.Linq;

namespace Diagram
{
    public class Fonts
    {
        /// <summary>
        /// meassure s string size written in font</summary>
        public static SizeF MeasureString(string s, Font font)
        {
            SizeF result;
            using (var image = new Bitmap(1, 1))
            {
                using (var g = Graphics.FromImage(image))
                {
                    result = g.MeasureString(s, font);
                }
            }

            return result;
        }

        /// <summary>
        /// convert xml element to Font object</summary>
        public static Font XmlToFont(XElement element)
        {

            string fontName = "";
            bool bold = false;
            bool italic = false;
            bool underline = false;
            bool strikeout = false;
            float fontSize = 12F;

            foreach (XElement el in element.Descendants())
            {
                try
                {
                    if (el.Name.ToString() == "name")
                    {
                        fontName = el.Value;
                    }

                    if (el.Name.ToString() == "size")
                    {
                        fontSize = float.Parse(el.Value);
                    }

                    if (el.Name.ToString() == "bold")
                    {
                        bold = bool.Parse(el.Value);
                    }

                    if (el.Name.ToString() == "italic")
                    {
                        italic = bool.Parse(el.Value);
                    }

                    if (el.Name.ToString() == "underline")
                    {
                        underline = bool.Parse(el.Value);
                    }

                    if (el.Name.ToString() == "strikeout")
                    {
                        strikeout = bool.Parse(el.Value);
                    }

                }
                catch (Exception ex)
                {
                    Program.log.write("load xml font error: " + ex.Message);
                }
            }

            FontStyle fontStyle = FontStyle.Regular;

            if (bold)
            {
                fontStyle = fontStyle | FontStyle.Bold;
            }

            if (italic)
            {
                fontStyle = fontStyle | FontStyle.Italic;
            }

            if (underline)
            {
                fontStyle = fontStyle | FontStyle.Underline;
            }

            if (strikeout)
            {
                fontStyle = fontStyle | FontStyle.Strikeout;
            }

            Font font = new Font(fontName, fontSize, fontStyle);
            return font;
        }

        /// <summary>
        /// convert Font object to xml </summary>
        public static XElement FontToXml(Font font, string name = "font")
        {
            XElement element = new XElement(name, new XAttribute("type", "font"));

            element.Add(new XElement("name", font.Name));
            element.Add(new XElement("size", font.Size.ToString()));
            element.Add(new XElement("bold", font.Bold));
            element.Add(new XElement("italic", font.Italic));
            element.Add(new XElement("underline", font.Underline));
            element.Add(new XElement("strikeout", font.Strikeout));

            return element;
        }

        /// <summary>
        /// compare fonts by attributes</summary>
        public static bool compare(Font font1, Font font2)
        {
            if (font1.Name != font2.Name) return false;
            if (font1.Size != font2.Size) return false;
            if (font1.Style!= font2.Style) return false;
            return true;
        }

        /// <summary>
        /// convert first character of input string to upper case</summary>
        public static string FirstCharToUpper(string input)
        {
            return input.Substring(0,1).ToUpper() + input.Substring(1).ToLower();
        }
    }
}
