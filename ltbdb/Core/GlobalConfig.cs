using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Singleton;
using System.Configuration;
using Castle.Core.Logging;
using CS.Helper;

namespace ltbdb.Core
{
	public sealed class GlobalConfig: SingletonBase<GlobalConfig>
	{
		private ILogger Log { get; set; }
		
		private GlobalConfig()
		{
			this.Log = MvcApplication.Container.Resolve<ILogger>();

			ReadConfiguration();
		}

		public static GlobalConfig Get()
		{
			return GetInstance();
		}
		
		#region Public members

		public int ItemsPerPage { get; private set; }
		public int RecentItems { get; private set; }

		#endregion
		
		#region Read configuration
		private void ReadConfiguration()
		{
			Log.InfoFormat("Load configuration...");

			ConfigFile cf = new ConfigFile(IOHelper.ConvertToFullPath("./App_Data/application.conf"));
			
			int temp = 0;
			if(Int32.TryParse(cf.GetValue("items_per_page", "12"), out temp))
			{
				this.ItemsPerPage = temp;
				Log.InfoFormat("Set items per page to {0}", this.ItemsPerPage);
			}

			if (Int32.TryParse(cf.GetValue("recent_items", "12"), out temp))
			{
				this.RecentItems = temp;
				Log.InfoFormat("Set recently added items to {0}", this.RecentItems);
			}
			
			
			Log.InfoFormat("Load configuration finished");
		}
		#endregion
	}
}
