using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ltbdb.Areas.Admin.Controllers
{
	[Authorize]
	public class StatsController : Controller
	{
		public ActionResult Index()
		{
			return View();
		}
	}
}
