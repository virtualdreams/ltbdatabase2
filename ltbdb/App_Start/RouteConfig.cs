using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ltbdb
{
	public class RouteConfig
	{
		public static void RegisterRoutes(RouteCollection routes)
		{
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
			routes.IgnoreRoute("images/{*pathInfo}");
			routes.IgnoreRoute("{*favicon}", new { favicon = @"(.*/)?favicon.ico(/.*)?" });
			routes.IgnoreRoute("{*url}", new { url = @".*\.asmx(/.*)?" });

			routes.MapRoute(
				name: "Search",
				url: "search",
				defaults: new { controller = "home", action = "search" }
			);

			routes.MapRoute(
				name: "Tags",
				url: "tags",
				defaults: new { controller = "tag", action = "index" }
			);

			routes.MapRoute(
				name: "Default",
				url: "{controller}/{action}/{id}",
				defaults: new { controller = "home", action = "index", id = UrlParameter.Optional }
			);
		}
	}
}