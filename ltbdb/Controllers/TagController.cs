using AutoMapper;
using log4net;
using ltbdb.Core;
using ltbdb.Core.Filter;
using ltbdb.Core.Helpers;
using ltbdb.Core.Models;
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
    public class TagController : Controller
    {
		private static readonly ILog Log = LogManager.GetLogger(typeof(TagController));

		private readonly BookService Book;
		private readonly TagService Tag;

		public TagController(BookService book, TagService tag)
		{
			Book = book;
			Tag = tag;
		}

		[HttpGet]
		public ActionResult Index()
		{
			var _tags = Tag.Get().OrderBy(o => o);

			var view = new TagViewContainer
			{
				Tags = _tags
			};

			return View(view);
		}

		[HttpGet]
		public ActionResult View(string id, int? ofs)
		{
			var _books = Book.GetByTag(id ?? String.Empty);
			var _page = _books.Skip(ofs ?? 0).Take(GlobalConfig.Get().ItemsPerPage);

			var books = Mapper.Map<BookModel[]>(_books);
			var offset = new PageOffset(ofs ?? 0, GlobalConfig.Get().ItemsPerPage, _books.Count());

			var view = new BookViewTagContainer
			{
				Books = books,
				Tag = id,
				PageOffset = offset
			};

			return View(view);
		}
    }
}
