using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace ltbdb.Core.Helpers
{
	static public class StringExtensions
	{
		/// <summary>
		/// Escape regex characters, except '?' and '*'.
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>
		static public string EscapeRegex(this string str)
		{
			if (String.IsNullOrEmpty(str))
				return String.Empty;

			str = Regex.Replace(str, @"\*{2,}", "*");

			return str
				.Replace(@"\", @"\\")
				.Replace(@"+", @"\+")
				.Replace(@"|", @"\|")
				.Replace(@"(", @"\(")
				.Replace(@")", @"\)")
				.Replace(@"{", @"\{")
				.Replace(@"[", @"\[")
				.Replace(@"^", @"\^")
				.Replace(@"$", @"\$")
				.Replace(@".", @"\.")
				.Replace(@"#", @"\#")
				.Replace(@" ", @"\ ")
				.Replace(@"*", @".*") // wildcard
				.Replace(@"?", @"."); // wildcard
		}

		/// <summary>
		/// Escape string to insert.
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>
		static public string Escape(this string str)
		{
			if (!String.IsNullOrEmpty(str))
			{
				return str
					.Replace("\\", "\\\\")
					.Replace("'", "\\'")
					.Replace("\"", "\\\"");
			}

			return str;
		}

		/// <summary>
		/// Escape string to search for special characters.
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>
		static public string EscapeForSearch(this string str)
		{
			if (!String.IsNullOrEmpty(str))
			{
				return str
					.Replace("_", @"\_")
					.Replace("%", @"\%");
			}

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

		/// <summary>
		/// Trim a string or return null.
		/// </summary>
		/// <param name="str">The string to trim.</param>
		/// <returns>Trimmed string or null.</returns>
		static public string TrimOrNull(this string str)
		{
			if (!String.IsNullOrEmpty(str))
				return str.Trim();

			return null;
		}
	}
}