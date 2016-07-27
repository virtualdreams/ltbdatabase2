using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web;

namespace ltbdb.Core.Helpers
{
	static public class VersionInfo
	{
		/// <summary>
		/// Get the file version.
		/// </summary>
		public static string FileVersion
		{
			get
			{
				Assembly asm = Assembly.GetExecutingAssembly();
				FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(asm.Location);

				return fvi.FileVersion;
			}
		}

		/// <summary>
		/// Get the product version.
		/// </summary>
		public static string ProductVersion
		{
			get
			{
				Assembly asm = Assembly.GetExecutingAssembly();
				FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(asm.Location);

				return fvi.ProductVersion;
			}
		}

		/// <summary>
		/// Get the git hash if available.
		/// </summary>
		public static string GitHash
		{
			get
			{
				Assembly asm = Assembly.GetExecutingAssembly();
				FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(asm.Location);

				var match = Regex.Match(fvi.ProductVersion, "([0-9a-f]{7,7})", RegexOptions.IgnoreCase);
				if (match.Success)
					return match.Groups[1].Value;
				else
					return "";
			}
		}
	}
}
