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
    }
}
