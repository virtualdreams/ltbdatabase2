using CS.Helper;
using SqlDataMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ltbdb.Core
{
	public class DbContextModule: IHttpModule
	{
		public void Dispose()
		{ }

		public void Init(HttpApplication context)
		{
			context.BeginRequest += context_BeginRequest;
			context.EndRequest += context_EndRequest; 
		}

		private void context_BeginRequest(object sender, EventArgs e)
		{
			HttpApplication application = (HttpApplication)sender;
			HttpContext httpContext = application.Context;

			SqlConfig config = new SqlConfig(IOHelper.ConvertToFullPath(GlobalConfig.Get().Database));

			httpContext.Items.Add("config", config);
			httpContext.Items.Add("context", config.CreateContext());
		}

		private void context_EndRequest(object sender, EventArgs e)
		{
			HttpApplication application = (HttpApplication)sender;
			HttpContext httpContext = application.Context;

			application.Context.Items.Remove("config");
			application.Context.Items.Remove("context");
		}
	}
}