using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Singleton;
using System.Configuration;
using log4net;
using ConfigFile;
using ltbdb.Core.Helpers;

namespace ltbdb.Core.Helpers
{
	public sealed class GlobalConfig: SingletonBase<GlobalConfig>
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(GlobalConfig));
		
		private GlobalConfig()
		{
			ReadConfiguration();
		}

		public static GlobalConfig Get()
		{
			return GetInstance();
		}
		
		#region Public members

		/// <summary>
		/// Path to database configuration.
		/// </summary>
		public string Database { get; private set; }
		
		/// <summary>
		/// Items per page to display.
		/// </summary>
		public int ItemsPerPage { get; private set; }

		/// <summary>
		/// Recent items to display.
		/// </summary>
		public int RecentItems { get; private set; }
		
		/// <summary>
		/// The username to login.
		/// </summary>
		public string Username { get; private set; }
		
		/// <summary>
		/// The password to login.
		/// </summary>
		public string Password { get; private set; }
		
		/// <summary>
		/// Storage path, where the images be saved.
		/// </summary>
		public string Storage { get; private set; }
		
		/// <summary>
		/// Path to image, if no cover exists.
		/// </summary>
		public string NoImage { get; private set; }
		
		/// <summary>
		/// Path to GraphicsMagick to process the uploaded images.
		/// </summary>
		public string GraphicsMagick { get; private set; }

		#endregion
		
		#region Read configuration
		private void ReadConfiguration()
		{
			Log.InfoFormat("Load configuration...");

			var config = new ConfigReader(IOHelper.ConvertToFullPath("./App_Data/application.conf"));

			this.Database = config.TryGetValue<string>("database", true);
			Log.InfoFormat("Set database configuration to {0}.", this.Database);

			this.ItemsPerPage = config.GetValue<int>("items_per_page", 18, true);
			Log.InfoFormat("Set items per page to {0}.", this.ItemsPerPage);

			this.RecentItems = config.GetValue<int>("recent_items", 18, true);
			Log.InfoFormat("Set recently added items to {0}.", this.RecentItems);

			this.Storage = config.TryGetValue<string>("storage", true);
			Log.InfoFormat("Set storage path to {0}.", this.Storage);

			this.NoImage = config.GetValue<string>("no_image", "/Content/no-image.png", true);
			Log.InfoFormat("Set no image path to {0}.", this.NoImage);

			this.GraphicsMagick = config.GetValue<string>("gm", "gm", true);
			Log.InfoFormat("Set graphics magick executable to {0}.", this.GraphicsMagick);

			this.Username = config.TryGetValue<string>("username", true);
			this.Password = config.TryGetValue<string>("password", true);
			
			Log.InfoFormat("Load configuration finished.");
		}
		#endregion
	}
}
