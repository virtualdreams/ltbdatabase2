using ltbdb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ltbdb.Controllers
{
    public class BookController : Controller
    {
		[HttpGet]
		public ActionResult Index()
		{
			return View();
		}

		[HttpGet]
		public ActionResult View(int? id)
		{
			BookDetailModel view = new BookDetailModel { Name = "Der Kolumbusfalter", Id = 1, Number = 1, Category = "Lustiges Taschenbuch" };
			
			return View(view);
		}
    }
}
