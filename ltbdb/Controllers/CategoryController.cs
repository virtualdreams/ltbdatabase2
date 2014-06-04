using AutoMapper;
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
	[HandleError(View = "Error")]
    public class CategoryController : Controller
    {
		public ILogger log { get; set; }

		[HttpGet]
        public ActionResult Index()
        {
			var result = new Store().GetBooks().Take(24);

			var books = Mapper.Map<BookModel[]>(result);

			var view = new BookViewModel { Books = books };

			return View(view);
        }

		[HttpGet]
		public ActionResult View(int? id)
		{
			var _category = new Store().GetCategory(id ?? 0);
			
			var _books = _category.GetBooks().Take(24);

			var category = Mapper.Map<CategoryModel>(_category);
			var books = Mapper.Map<BookModel[]>(_books);

			var view = new BookViewCategoryModel { Books = books, Category = category };

			return View(view);
		}
    }
}
