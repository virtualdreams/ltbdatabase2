using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using System.IO;
using log4net;
using ltbdb.Controllers;
using ltbdb.Core.Helpers;

namespace ltbdb
{
	// Hinweis: Anweisungen zum Aktivieren des klassischen Modus von IIS6 oder IIS7 
	// finden Sie unter "http://go.microsoft.com/?LinkId=9394801".
	public class MvcApplication : System.Web.HttpApplication
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(MvcApplication));

		protected void Application_Start()
		{
			log4net.Config.XmlConfigurator.Configure(new FileInfo(IOHelper.ConvertToFullPath("./App_Data/log4net.xml")));

			MvcHandler.DisableMvcResponseHeader = true;
			
			AreaRegistration.RegisterAllAreas();

			WebApiConfig.Register(GlobalConfiguration.Configuration);
			FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
			RouteConfig.RegisterRoutes(RouteTable.Routes);

			AutomapperConfig.RegisterAutomapper();
		}

		protected void Application_End()
		{
			
		}

		protected void Application_Error(object sender, EventArgs e)
		{
			if (Context.IsCustomErrorEnabled)
			{
				ExceptionHandler(Server.GetLastError());
			}
		}

		private void ExceptionHandler(Exception ex)
		{
			HttpException exception = ex as HttpException ?? new HttpException(500, "Internal Server Error", ex);

			Log.Fatal("ExceptionHandler", exception);

			Response.Clear();
			Server.ClearError();

			RouteData routeData = new RouteData();
			routeData.Values.Add("controller", "error");
			routeData.Values.Add("action", "index");
			routeData.Values.Add("exception", exception);

			switch (exception.GetHttpCode())
			{
				case 404:
					routeData.Values["action"] = "http404";
					break;

				default:
					routeData.Values["action"] = "index";
					break;
			}

			Response.TrySkipIisCustomErrors = true;
			IController controller = new ErrorController();
			controller.Execute(new RequestContext(new HttpContextWrapper(Context), routeData));
		}
	}
}