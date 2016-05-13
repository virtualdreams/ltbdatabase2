using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Web.Http;

namespace ltbdb
{
	public static class WebApiConfig
	{
		public static void Register(HttpConfiguration config)
		{
			//config.Routes.MapHttpRoute(
			//	name: "CategoryApi",
			//	routeTemplate: "api/category/{action}/{id}",
			//	defaults: new { controller = "category", id = RouteParameter.Optional }
			//);

			//config.Routes.MapHttpRoute(
			//	name: "StatsApi",
			//	routeTemplate: "api/stats/{action}",
			//	defaults: new { controller = "stats" }
			//);

			//config.Routes.MapHttpRoute(
			//	name: "SearchApi",
			//	routeTemplate: "api/search/{action}/{term}",
			//	defaults: new { controller = "search", term = RouteParameter.Optional }
			//);

			//config.Routes.MapHttpRoute(
			//	name: "DefaultApi",
			//	routeTemplate: "api/{controller}/{id}",
			//	defaults: new { id = RouteParameter.Optional }
			//);

			config.Routes.MapHttpRoute(
				name: "DefaultApi",
				routeTemplate: "api/{controller}/{action}/{id}",
				defaults: new { id = RouteParameter.Optional }
			);

			config.Formatters.Add(new BrowserJsonFormatter());
		}
	}

	/// <summary>
	/// http://stackoverflow.com/questions/9847564/how-do-i-get-asp-net-web-api-to-return-json-instead-of-xml-using-chrome/20556625#20556625
	/// </summary>
	public class BrowserJsonFormatter : JsonMediaTypeFormatter
	{
		public BrowserJsonFormatter()
		{
			this.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));
			this.SerializerSettings.Formatting = Formatting.Indented;
		}

		public override void SetDefaultContentHeaders(Type type, HttpContentHeaders headers, MediaTypeHeaderValue mediaType)
		{
			base.SetDefaultContentHeaders(type, headers, mediaType);
			headers.ContentType = new MediaTypeHeaderValue("application/json");
		}
	}
}
