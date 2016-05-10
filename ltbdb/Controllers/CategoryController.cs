using AutoMapper;
using log4net;
using ltbdb.Core;
using ltbdb.DomainServices;
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
    public class CategoryController : Controller
    {
		private static readonly ILog Log = LogManager.GetLogger(typeof(CategoryController));

		[HttpGet]
        public ActionResult Index(int? ofs)
        {
			var _books = Book.Get().OrderBy(o => o.Category.Id);
			var _page = _books.Skip(ofs ?? 0).Take(GlobalConfig.Get().ItemsPerPage);

			var books = Mapper.Map<BookModel[]>(_page);
			var pageOffset = new PageOffset(ofs ?? 0, GlobalConfig.Get().ItemsPerPage, _books.Count());

			var view = new BookViewAllContainer { Books = books, PageOffset = pageOffset};

			return View(view);
        }

		[HttpGet]
		public ActionResult View(int? id, int? ofs)
		{
			var _category = Category.Get(id ?? 0);
			var _books = _category.GetBooks();
			var _page = _books.Skip(ofs ?? 0).Take(GlobalConfig.Get().ItemsPerPage);

			var category = Mapper.Map<CategoryModel>(_category);
			var books = Mapper.Map<BookModel[]>(_page);
			var pageOffset = new PageOffset(ofs ?? 0, GlobalConfig.Get().ItemsPerPage, _books.Count());

			var view = new BookViewCategoryContainer { Books = books, Category = category, PageOffset = pageOffset };

			return View(view);
		}
    }
}
