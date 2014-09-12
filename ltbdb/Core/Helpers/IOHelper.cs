using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

namespace CS.Helper
{
	/// <summary>
	/// Helpers for file IO
	/// </summary>
	public class IOHelper
	{
		/// <summary>
		/// Get th full path for the given filename.
		/// </summary>
		/// <param name="filename">The filename as file, absolute filepath or relative filepath</param>
		/// <returns>The full path</returns>
		public static string ConvertToFullPath(string filename)
		{
			if (String.IsNullOrEmpty(filename))
			{
				throw new ArgumentNullException("filename");
			}

			string tmp = "";
			try
			{
				string applicationBaseDirectory = AppDomain.CurrentDomain.BaseDirectory;
				if (applicationBaseDirectory != null)
				{
					Uri uri = new Uri(applicationBaseDirectory);
					if (uri.IsFile)
					{
						tmp = uri.LocalPath;
					}
				}
			}
			catch
			{
			}
			if (!String.IsNullOrEmpty(tmp))
			{
				return Path.GetFullPath(Path.Combine(tmp, filename));
			}
			return Path.GetFullPath(filename);
		}
	}
}
