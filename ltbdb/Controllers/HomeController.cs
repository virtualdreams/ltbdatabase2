using ltbdb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ltbdb.Controllers
{
    public class HomeController : Controller
    {
		[HttpGet]
        public ActionResult Index()
        {
			BookViewModel view = new BookViewModel();
			view.Books = new BookModel[] { };
			
			return View(view);
        }
    }
}
