using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace CS.Helper
{
	static public class StringHelper
	{
		/// <summary>
		/// Escape string for mysql.
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>
		static public string Escape(this string str)
		{
			if (!String.IsNullOrEmpty(str))
				return str.Replace("\\", "\\\\").Replace("'", "\\'").Replace("\"", "\\\"");
			return str;
		}

		/// <summary>
		/// Filter string from forbidden characters.
		/// </summary>
		/// <param name="str"></param>
		/// <param name="filter"></param>
		/// <returns></returns>
		static public string Filter(this string str, string filter)
		{
			if (String.IsNullOrEmpty(filter))
				throw new ArgumentNullException("filter");

			if (!String.IsNullOrEmpty(str))
			{
				return Regex.Replace(str, String.Format("[{0}]", filter), "");
			}
			return str;
		}
	}
}