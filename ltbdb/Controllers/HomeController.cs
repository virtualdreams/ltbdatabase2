using Castle.Core.Logging;
using ltbdb.DomainServices;
using ltbdb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ltbdb.Controllers
{
	public interface ICastle
	{
		string Hello();
	}


	public class DemoCastle : ICastle
	{

		public string Hello()
		{
			return "Demo";
		}
	}

    public class HomeController : Controller
    {
		public ILogger log { get; set; }
		private ICastle castle;

		public HomeController(ICastle castle)
		{
			this.castle = castle;
		}

		[HttpGet]
        public ActionResult Index()
        {
			BookViewModel view = new BookViewModel();
			view.Books = new BookModel[] { };

			Store store = new Store();
			var recent = store.GetRecentlyAdded();
			var book = store.GetBook(10);

			var tags = store.GetTags();
			var tag = store.GetTag(6);

			//string c = castle.Hello();
			//log.Info(c);
			
			
			return View(view);
        }
    }
}
