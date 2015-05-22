using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Singleton;
using System.Configuration;
using log4net;
using ConfigFile;
using CS.Helper;

namespace ltbdb.Core
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

		public string Database { get; private set; }
		public int ItemsPerPage { get; private set; }
		public int RecentItems { get; private set; }
		public string Username { get; private set; }
		public string Password { get; private set; }
		public string Storage { get; private set; }
		public string NoImage { get; private set; }
		public string GraphicsMagick { get; private set; }

		#endregion
		
		#region Read configuration
		private void ReadConfiguration()
		{
			Log.InfoFormat("Load configuration...");

			var config = new ConfigReader(IOHelper.ConvertToFullPath("./App_Data/application.conf"));

			this.Database = config.GetValue<string>("database", "");
			Log.InfoFormat("Set database configuration to {0}", this.Database);

			this.ItemsPerPage = config.GetValue<int>("items_per_page", 18);
			Log.InfoFormat("Set items per page to {0}", this.ItemsPerPage);

			this.RecentItems = config.GetValue<int>("recent_items", 18);
			Log.InfoFormat("Set recently added items to {0}", this.RecentItems);

			this.Storage = config.GetValue<string>("storage", "");
			Log.InfoFormat("Set storage path to {0}", this.Storage);

			this.NoImage = config.GetValue<string>("no_image", "/Content/no-image.png");
			Log.InfoFormat("Set no image path to {0}", this.NoImage);

			this.GraphicsMagick = config.GetValue<string>("gm", "gm");
			Log.InfoFormat("Set graphics magick executable to {0}", this.GraphicsMagick);

			this.Username = config.GetValue("username", "");
			this.Password = config.GetValue("password", "");
			
			Log.InfoFormat("Load configuration finished");
		}
		#endregion
	}
}
