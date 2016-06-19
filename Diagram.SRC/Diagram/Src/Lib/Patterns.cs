using System.Text.RegularExpressions;

namespace Diagram
{
    class Patterns
    {
        public static string[] splitCommand(string cmd)
        {
            // ^(?:\s*"?)((?<=")(?:\\.|[^"\\])*(?=")|[^ "]+)(?:"?\s*)(.*)
            string pattern = "";
            pattern += "^";
            pattern += "(?:\\s*\"?)";// skip start space and quote
            pattern += "(";
            pattern += "(?:(?<=\")(?:\\\\.|[^\"\\\\])*(?=\"))";// match command with quotas
            pattern += "|";
            pattern += "(?:[^ \"]+)"; // match command without quotas
            pattern += ")";
            pattern += "(?:\"?\\s*)"; //skip space between command and arguments
            pattern += "(.*)"; // arguments

            MatchCollection matches = Regex.Matches(cmd, pattern);

            string command = "";
            string arguments = "";

            foreach (Match match in matches)
            {
                command = match.Groups[1].Value;
                arguments = match.Groups[2].Value;
            }

            return new string[] { command, arguments };
        }

        public static bool hasHastag(string link, ref string fileName, ref string searchString)
        {
            Match matchFileOpenOnPosition = (new Regex("^([^#]+)#(.*)$")).Match(link.Trim());

            if (matchFileOpenOnPosition.Success)
            {
                fileName = matchFileOpenOnPosition.Groups[1].Value;
                searchString = matchFileOpenOnPosition.Groups[2].Value;
                return true;
            }

            return false;
        }

        public static bool isNumber(string text)
        {
            Match matchNumber = (new Regex("^(\\d+)$")).Match(text);

            if (matchNumber.Success)
            {
                return true;
            }

            return false;
        }

        public static bool isColor(string text)
        {
            Match matchNumber = (new Regex("^#([A-Fa-f0-9]{6}|[A-Fa-f0-9]{3})$")).Match(text);

            if (matchNumber.Success)
            {
                return true;
            }

            return false;
        }
    }
}
