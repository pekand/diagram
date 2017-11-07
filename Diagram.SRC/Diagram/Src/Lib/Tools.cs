using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diagram
{
    public class Tools
    {
        public static decimal GetScale(decimal scale)
        {
            return (decimal)Math.Pow(2, (double)scale);
        }

        public static decimal StringToDecimal(string text)
        {
            return decimal.Parse(
                text,
                NumberStyles.AllowParentheses |
                NumberStyles.AllowLeadingWhite |
                NumberStyles.AllowTrailingWhite |
                NumberStyles.AllowThousands |
                NumberStyles.AllowDecimalPoint |
                NumberStyles.AllowLeadingSign, 
                NumberFormatInfo.InvariantInfo
            );
        }

    }
}
