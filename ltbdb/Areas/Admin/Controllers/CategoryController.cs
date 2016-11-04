using AutoMapper;
using log4net;
using ltbdb.Core.Filter;
using ltbdb.Core.Models;
using ltbdb.Core.Services;
using ltbdb.Models;
using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ltbdb.Areas.Admin.Controllers
{
	[Authorize]
	[LogError(Order = 0)]
	[HandleError(View = "Error", Order = 99)]
    public class CategoryController : Controller
    {
		private static readonly ILog Log = LogManager.GetLogger(typeof(CategoryController));

		private readonly BookService Book;
		private readonly CategoryService Category;

		public CategoryController(BookService book, CategoryService category)
		{
			Book = book;
			Category = category;
		}

		[HttpGet]
        public ActionResult Index()
        {
			var _categories = Category.Get().OrderBy(s => s);

			var view = new CategoryViewContainer
			{
				Categories = _categories
			};
			
			return View(view);
        }

		[HttpPost]
		public ActionResult Move(string from, string to)
		{
			Category.Rename(from ?? String.Empty, to ?? String.Empty);

			return Redirect("index");
		}
    }
}
