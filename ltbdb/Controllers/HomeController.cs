using AutoMapper;
using log4net;
using ltbdb.Core.Filter;
using ltbdb.Core.Helpers;
using ltbdb.Core.Models;
using ltbdb.Core.Services;
using ltbdb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace ltbdb.Controllers
{
	[LogError(Order = 0)]
	[HandleError(View = "Error", Order=99)]
    public class HomeController : Controller
    {
		private static readonly ILog Log = LogManager.GetLogger(typeof(HomeController));

		private readonly BookService Book;
		private readonly CategoryService Category;
		private readonly TagService Tag;

		public HomeController(BookService book, CategoryService category, TagService tag)
		{
			Book = book;
			Category = category;
			Tag = tag;
		}

		[HttpGet]
        public ActionResult Index()
        {
			var _books = Book.GetRecentlyAdded();

			var books = Mapper.Map<BookModel[]>(_books);

			var view = new BookViewContainer { Books = books };

			return View(view);
        }

		[ValidateInput(false)]
		[HttpGet]
		public ActionResult Search(string q, int? ofs)
		{
			var _books = Book.Search(q ?? String.Empty);
			var _page = _books.Skip(ofs ?? 0).Take(GlobalConfig.Get().ItemsPerPage);

			var books = Mapper.Map<BookModel[]>(_page);
			var offset = new PageOffset(ofs ?? 0, GlobalConfig.Get().ItemsPerPage, _books.Count());

			var view = new BookViewSearchContainer
			{
				Books = books,
				Query = q,
				PageOffset = offset
			};

			return View(view);
		}

		[ChildActionOnly]
		public ActionResult Tags()
		{
			var _tags = Tag.Get().Take(5);

			var view = new TagViewContainer { Tags = _tags };

			return View("_PartialTags", view);
		}

		[ChildActionOnly]
		public ActionResult Categories()
		{
			var _categories = Category.Get();

			var view = new CategoryViewContainer { Categories = _categories };

			return View("_PartialCategories", view);
		}
    }
}
