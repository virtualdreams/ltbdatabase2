using System.Web.Mvc;

namespace ltbdb.Areas.Admin
{
	public class AdminAreaRegistration : AreaRegistration
	{
		public override string AreaName
		{
			get
			{
				return "admin";
			}
		}

		public override void RegisterArea(AreaRegistrationContext context)
		{
			context.MapRoute(
				"Admin_default",
				"admin/{controller}/{action}/{id}",
				new { controller = "home", action = "index", id = UrlParameter.Optional },
				namespaces: new[] { "ltbdb.Areas.Admin.Controllers" }
			);
		}
	}
}
