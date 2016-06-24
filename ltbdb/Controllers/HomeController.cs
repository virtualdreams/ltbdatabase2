using AutoMapper;
using log4net;
using ltbdb.Core.Filter;
using ltbdb.Core.Helpers;
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
		private readonly TagService Tag;
		private readonly CategoryService Category;

		public HomeController(BookService book, TagService tag, CategoryService category)
		{
			Book = book;
			Tag = tag;
			Category = category;
		}

		[HttpGet]
        public ActionResult Index()
        {
			var _books = Book.RecentlyAdded();

			var books = Mapper.Map<BookModel[]>(_books);

			var view = new BookViewContainer { Books = books };

			return View(view);
        }

		[ValidateInput(false)]
		[HttpGet]
		public ActionResult Search(string q, int? ofs)
		{
			var _books = Book.Search(q ?? String.Empty).OrderBy(o => o.Category.Id);
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

		//TODO move to the right controller
		[ChildActionOnly]
		public ActionResult Tags()
		{
			var _tags = Tag.Get().OrderByDescending(o => o.References).Take(5);

			var tags = Mapper.Map<TagModel[]>(_tags);

			return View("_PartialTags", tags);
		}

		//TODO move to the right controller
		[ChildActionOnly]
		public ActionResult Categories()
		{
			var _categories = Category.Get().Where(w => Book.GetByCategory(w.Id).Count() > 0);

			var categories = Mapper.Map<CategoryModel[]>(_categories);

			var view = new CategoryViewContainer { Categories = categories };

			return View("_PartialCategories", view);
		}
    }
}
