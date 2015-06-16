using ltbdb.Core;
using ltbdb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ltbdb.Controllers
{
	[LogError(Order = 0)]
	[HandleError(View = "Error", Order=99)]
    public class ErrorController : Controller
    {
        public ActionResult Index()
        {
			var exception = RouteData.Values["exception"] as Exception ?? new Exception("Internal Server Error");

			var view = new ErrorModel { Exception = exception };

			return View(view);
        }

		public ActionResult Http404()
		{
			return View();
		}
    }
}
