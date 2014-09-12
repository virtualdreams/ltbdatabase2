using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Web;

namespace ltbdb.Core
{
	static public class VersionInfo
	{
		public static string FileVersion
		{
			get
			{
				Assembly asm = Assembly.GetExecutingAssembly();
				FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(asm.Location);
				
				return fvi.FileVersion;
			}
		}

		public static string ProductVersion
		{
			get
			{
				Assembly asm = Assembly.GetExecutingAssembly();
				FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(asm.Location);

				return fvi.ProductVersion;
				
				//var build = ((AssemblyInformationalVersionAttribute)Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyInformationalVersionAttribute), false)[0]).InformationalVersion;
				//return build;
			}
		}
	}
}
