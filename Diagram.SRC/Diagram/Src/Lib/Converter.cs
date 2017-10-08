﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace Diagram
{
	public class Converter //UID8493692592
	{
		public static DateTime ToDateAndTime(string s)
		{
			DateTime d;
			bool result = DateTime.TryParse(s, out d);
			if (!result) {
				return DateTime.Now;
			}

			return d;
		}

		public static DateTime ToDate(string s)
		{
			DateTime d;
			bool result = DateTime.TryParseExact(s, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out d);
			if (!result) {
				return DateTime.Now;
			}

			return d;
		}

		public static String DateToString(DateTime d)
		{
			return String.Format("{0:yyyy-MM-dd}", d);
		}



		public static int ToInt(string s)
		{
			int i;
			bool result = Int32.TryParse(s, out i);
			if (!result) {
				return 0;
			}

			return i;
		}

    }
}

