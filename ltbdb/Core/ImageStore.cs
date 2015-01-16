using CS.Helper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Drawing;
using Castle.Core.Logging;
using System.Diagnostics;

namespace ltbdb.Core
{
	static public class ImageStore
	{
		static readonly string thumbnailDirectory = "thumb";

		/// <summary>
		/// Save image to storage and create a thumbnail.
		/// </summary>
		/// <param name="stream">The image stream.</param>
		/// <param name="createThumbnail">Create also a thumbnail.</param>
		/// <returns>The name of the created file.</returns>
		static public string Save(Stream stream, bool createThumbnail = true)
		{
			var log = MvcApplication.Container.Resolve<ILogger>();

			if (stream == null)
				throw new Exception("Stream must not be null.");

			var filename = String.Format("{0}.jpg", GetFilename());
			var imageStorage = GetStoragePath();
			var thumbStorage = GetThumbPath();
			
			var imagePath = Path.Combine(imageStorage, filename);
			var thumbPath = Path.Combine(thumbStorage, filename);

			log.DebugFormat("Image path: {0}", imagePath);
			log.DebugFormat("Thumb path: {0}", thumbPath);

			GraphicsMagick.GraphicsImage = GlobalConfig.Get().GraphicsMagick;

			try
			{
				using (var output = File.Create(imagePath))
				{
					GraphicsMagick.PInvoke(stream, output, "convert - -background white -flatten jpg:-");
				}

				stream.Position = 0;

				//check if thumbnail directory exists
				if (!Directory.Exists(thumbStorage))
				{
					Directory.CreateDirectory(thumbStorage);
				}

				using (var output = File.Create(thumbPath))
				{
					GraphicsMagick.PInvoke(stream, output, "convert - -background white -flatten -resize 200x200 jpg:-");
				}

				return filename;
			}
			catch (Exception ex)
			{
				log.ErrorFormat(ex.ToString());

				if (File.Exists(imagePath))
				{
					File.Delete(imagePath);
				}

				if (File.Exists(thumbPath))
				{
					File.Delete(thumbPath);
				}
				
				return null;
			}
		}

		/// <summary>
		/// Test if image exists in store.
		/// </summary>
		/// <param name="filename">The filename.</param>
		/// <param name="thumbnail">Test for thumbnail.</param>
		/// <returns>True if exists.</returns>
		static public bool Exists(string filename, bool thumbnail = false)
		{
			if (String.IsNullOrEmpty(filename))
				return false;

			if (!thumbnail)
			{
				var storage = GetStoragePath();
				var path = Path.Combine(storage, filename);

				return File.Exists(path);
			}
			else
			{
				var storage = GetThumbPath();
				var path = Path.Combine(storage, filename);

				return File.Exists(path);
			}
		}

		/// <summary>
		/// Delete a image from storage.
		/// </summary>
		/// <param name="filename">The filename.</param>
		/// <param name="thumbnail">Delete thumbnail also.</param>
		static public void Remove(string filename, bool thumbnail = true)
		{
			if (Exists(filename))
			{
				var storage = GetStoragePath();
				var path = Path.Combine(storage, filename);
				File.Delete(path);
			}

			if (Exists(filename, true))
			{
				var storage = GetThumbPath();
				var path = Path.Combine(storage, filename);
				File.Delete(path);
			}
		}

		/// <summary>
		/// Get web path.
		/// </summary>
		/// <param name="filename">The image filename.</param>
		/// <param name="preferThumbnail">Prefer to load thumbnails over full sized images.</param>
		/// <returns>The web path.</returns>
		static public string GetWebPath(string filename, bool preferThumbnail = false)
		{
			DirectoryInfo di = new DirectoryInfo(GetStoragePath());
			
			if (preferThumbnail)
			{
				if (Exists(filename, true))
				{
					return String.Format("/{0}/{1}/{2}", di.Name, thumbnailDirectory, filename);
				}
			}
			
			if (Exists(filename))
			{
				return String.Format("/{0}/{1}", di.Name, filename);
			}

			return "/content/no-image.png";
		}

		/// <summary>
		/// Generate a new file name from guid.
		/// </summary>
		/// <returns></returns>
		static private string GetFilename()
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

		/// <summary>
		/// Get the absolute thumbnail path.
		/// </summary>
		/// <returns></returns>
		static private string GetThumbPath()
		{
			return Path.Combine(GetStoragePath(), thumbnailDirectory);
		}
	}
}