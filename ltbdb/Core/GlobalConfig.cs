using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Singleton;
using System.Configuration;
using CS.Helper;
using log4net;

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

		public int ItemsPerPage { get; private set; }
		public int RecentItems { get; private set; }
		public string Username { get; private set; }
		public string Password { get; private set; }
		public string Storage { get; private set; }
		public string GraphicsMagick { get; private set; }

		#endregion
		
		#region Read configuration
		private void ReadConfiguration()
		{
			Log.InfoFormat("Load configuration...");

			ConfigFile cf = new ConfigFile(IOHelper.ConvertToFullPath("./App_Data/application.conf"));

			this.ItemsPerPage = cf.GetValue<int>("items_per_page", 18);
			Log.InfoFormat("Set items per page to {0}", this.ItemsPerPage);

			this.RecentItems = cf.GetValue<int>("recent_items", 18);
			Log.InfoFormat("Set recently added items to {0}", this.RecentItems);

			this.Storage = cf.GetValue<string>("storage", "");
			Log.InfoFormat("Set storage path to {0}", this.Storage);

			this.GraphicsMagick = cf.GetValue<string>("gm", "gm");
			Log.InfoFormat("Set graphics magick executable to {0}", this.GraphicsMagick);

			this.Username = cf.GetValue("username", "");
			this.Password = cf.GetValue("password", "");
			
			Log.InfoFormat("Load configuration finished");
		}
		#endregion
	}
}
