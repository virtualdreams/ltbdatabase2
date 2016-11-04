﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace ltbdb.Core.Helpers
{
	static public class StringExtensions
	{
		/// <summary>
		/// Slugify the string.
		/// </summary>
		/// <param name="value">The string to slugify.</param>
		/// <returns>Slugified string.</returns>
		static public string ToSlug(this string value)
		{
			//First to lower case
			value = value.ToLowerInvariant();

			//Remove all accents
			var bytes = Encoding.GetEncoding("Cyrillic").GetBytes(value);
			value = Encoding.ASCII.GetString(bytes);

			//Replace spaces
			value = Regex.Replace(value, @"\s", "-", RegexOptions.Compiled);

			//Remove invalid chars
			value = Regex.Replace(value, @"[^a-z0-9\s-_]", "", RegexOptions.Compiled);

			//Trim dashes from end
			value = value.Trim('-', '_');

			//Replace double occurences of - or _
			value = Regex.Replace(value, @"([-_]){2,}", "$1", RegexOptions.Compiled);

			return value;
		}

		/// <summary>
		/// Combines two pathes.
		/// </summary>
		/// <param name="baseUri">The base path.</param>
		/// <param name="relativeUri">The additional path.</param>
		/// <returns></returns>
		public static string Combine(this string baseUri, string relativeUri)
		{
			if (baseUri == null) throw new ArgumentNullException("baseUri");
			if (relativeUri == null) throw new ArgumentNullException("relativeUri");

			if (!baseUri.EndsWith("/"))
				baseUri += "/";

			return String.Format("{0}{1}", baseUri, relativeUri);
		}

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
	}
}