using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ltbdb.Controllers
{
	[HandleError(View = "Error")]
    public class ErrorController : Controller
    {
        public ActionResult Index()
        {
			var exception = RouteData.Values["exception"] as Exception ?? new Exception("Internal Server Error");

			var view = new HandleErrorInfo(exception, "", "");

            return View(view);
        }

		public ActionResult Http404()
		{
			return View();
		}
    }
}
