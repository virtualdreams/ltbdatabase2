using AutoMapper;
using log4net;
using ltbdb.Core;
using ltbdb.Core.Filter;
using ltbdb.Core.Helpers;
using ltbdb.Core.Services;
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

		private readonly BookService Book;

		public CategoryController(BookService book)
		{
			Book = book;
		}

		[HttpGet]
        public ActionResult Index(int? ofs)
        {
			var _books = Book.Get().OrderBy(o => o.Category);
			var _page = _books.Skip(ofs ?? 0).Take(GlobalConfig.Get().ItemsPerPage);

			var books = Mapper.Map<BookModel[]>(_page);
			var offset = new PageOffset(ofs ?? 0, GlobalConfig.Get().ItemsPerPage, _books.Count());

			var view = new BookViewAllContainer
			{
				Books = books,
				PageOffset = offset
			};

			return View(view);
        }

		[HttpGet]
		public ActionResult View(string id, int? ofs)
		{
			var _books = Book.GetByCategory(id ?? String.Empty);
			if (_books.Count() == 0)
				throw new HttpException(404, "Resource not found.");

			var _page = _books.Skip(ofs ?? 0).Take(GlobalConfig.Get().ItemsPerPage);

			var books = Mapper.Map<BookModel[]>(_page);
			var offset = new PageOffset(ofs ?? 0, GlobalConfig.Get().ItemsPerPage, _books.Count());

			var view = new BookViewCategoryContainer
			{
				Books = books,
				Category = id,
				PageOffset = offset
			};

			return View(view);
		}
    }
}
