using CS.Helper;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;

namespace ltbdb.Core
{
	public class ImageStore
	{
		/// <summary>
		/// Save a image stream.
		/// </summary>
		/// <param name="stream"></param>
		/// <returns></returns>
		static public string Save(Stream stream)
		{
			if(stream == null)
				throw new Exception("Stream must not be null.");
			
			var name = String.Format("{0}.png", GetName());
			var storage = GetStoragePath();
			var path = Path.Combine(storage, name);

			try
			{
				Image image = Image.FromStream(stream);

				image.Save(path, System.Drawing.Imaging.ImageFormat.Png);

				return name;
			}
			catch (Exception)
			{
				return null;
			}
		}

		/// <summary>
		/// Check if the image exists.
		/// </summary>
		/// <param name="filename">The filename.</param>
		/// <returns>True on success.</returns>
		static public bool Exists(string filename)
		{
			if (String.IsNullOrEmpty(filename))
				return false;

			var storage = GetStoragePath();

			var path = Path.Combine(storage, filename);

			return File.Exists(path);
		}

		/// <summary>
		/// Remove a image from storage.
		/// </summary>
		/// <param name="filename">The filename.</param>
		static public void Remove(string filename)
		{
			if (Exists(filename))
			{
				var storage = GetStoragePath();

				var path = Path.Combine(storage, filename);

				File.Delete(path);
			}
		}

		/// <summary>
		/// Generate a new file name from guid.
		/// </summary>
		/// <returns></returns>
		static private string GetName()
		{
			return Guid.NewGuid().ToString();
		}

		/// <summary>
		/// Get the absolute storage path.
		/// </summary>
		/// <returns></returns>
		static private string GetStoragePath()
		{
			return IOHelper.ConvertToFullPath(GlobalConfig.Get().Storage);
		}
	}
}