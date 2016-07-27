﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Drawing;
using System.Diagnostics;
using log4net;

namespace ltbdb.Core.Helpers
{
	/// <summary>
	/// Type of image to request from storage.
	/// </summary>
	public enum ImageType
	{
		/// <summary>
		/// Get the normal version if available, otherwise none.
		/// </summary>
		Normal,

		/// <summary>
		/// Get the thumbnail version if available, otherwise none.
		/// </summary>
		Thumbnail,

		/// <summary>
		/// Prefer thumbnail over normal version if available, otherwise none,
		/// </summary>
		PreferThumbnail
	}

	static public class ImageStore
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(ImageStore));

		static readonly string thumbnailDirectory = "thumb";

		/// <summary>
		/// Save image to storage and create a thumbnail.
		/// </summary>
		/// <param name="stream">The image stream.</param>
		/// <param name="createThumbnail">Create also a thumbnail.</param>
		/// <returns>The name of the created file.</returns>
		static public string Save(Stream stream, bool createThumbnail = true)
		{
			if (stream == null)
				throw new Exception("Stream must not null.");

			var filename = String.Format("{0}.jpg", GetFilename());
			var imageStorage = GetStoragePath();
			var thumbStorage = GetThumbPath();
			
			var imagePath = Path.Combine(imageStorage, filename);
			var thumbPath = Path.Combine(thumbStorage, filename);

			Log.InfoFormat("Image path: {0}", imagePath);
			Log.InfoFormat("Thumb path: {0}", thumbPath);

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
				Log.ErrorFormat(ex.Message);

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
		/// <param name="imageType">Select type of image to load.</param>
		/// <returns>The web path.</returns>
		static public string GetWebPath(string filename, ImageType imageType = ImageType.Normal)
		{
			var _directory = new DirectoryInfo(GetStoragePath());
			
			switch (imageType)
			{
				case ImageType.Thumbnail:
					if (Exists(filename, true))
						return String.Format("/{0}/{1}/{2}", _directory.Name, thumbnailDirectory, filename);
					goto default;

				case ImageType.PreferThumbnail:
					if (Exists(filename, true))
						return String.Format("/{0}/{1}/{2}", _directory.Name, thumbnailDirectory, filename);
					goto case ImageType.Normal;

				case ImageType.Normal:
					if (Exists(filename))
						return String.Format("/{0}/{1}", _directory.Name, filename);
					goto default;

				default:
					return GlobalConfig.Get().NoImage.TrimStart('.');
			}
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