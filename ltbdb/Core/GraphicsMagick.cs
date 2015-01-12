using Castle.Core.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;

namespace ltbdb.Core
{
	static public class GraphicsMagick
	{
		/// <summary>
		/// Invoke "GraphicsMagick" via command line interface.
		/// </summary>
		/// <param name="stream"></param>
		/// <param name="target"></param>
		/// <param name="arguments"></param>
		static public void PInvoke(Stream stream, string target, string arguments)
		{
			var log = MvcApplication.Container.Resolve<ILogger>();

			using (var process = new Process())
			{
				try
				{
					process.StartInfo = new ProcessStartInfo
					{
						FileName = GlobalConfig.Get().GraphicsMagick,
						Arguments = arguments,
						RedirectStandardInput = true,
						RedirectStandardOutput = true,
						RedirectStandardError = true,
						UseShellExecute = false,
						CreateNoWindow = true
					};

					process.Start();

					stream.CopyTo(process.StandardInput.BaseStream);
					process.StandardInput.Flush();
					process.StandardInput.Close();

					var outputStream = File.Create(target);

					process.StandardOutput.BaseStream.CopyTo(outputStream);
					outputStream.Flush();
					outputStream.Close();

					var error = process.StandardError.ReadToEnd();
					process.WaitForExit();
					
					if (process.ExitCode != 0)
					{
						File.Delete(target);
						throw new Exception(error);
					}
				}
				catch (Exception ex)
				{
					log.ErrorFormat("{0}", ex.ToString());
					throw;
				}
			}
		}
	}
}