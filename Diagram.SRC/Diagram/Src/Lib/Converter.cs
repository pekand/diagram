using System;
using System.Globalization;

namespace Diagram
{
	public class Converter
	{
		public static DateTime toDate(string s)
		{
			DateTime d;
			DateTime.TryParse(s, out d);
			return d;
		}

		public static DateTime toExactDate(string s)
		{
			DateTime d;
			DateTime.TryParseExact(s, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out d);
			return d;
		}

		public static int toInt(string s)
		{
			int i;
			Int32.TryParse (s, out i);
			return i;
		}

	}
}

