using AutoMapper;
using log4net;
using ltbdb.Core;
using ltbdb.DomainServices;
using ltbdb.Models;
using ltbdb.Core.Helpers;
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
			var _books = Book.Search(q.TrimOrNull() ?? "").OrderBy(o => o.Category.Id);
			var _page = _books.Skip(ofs ?? 0).Take(GlobalConfig.Get().ItemsPerPage);

			var books = Mapper.Map<BookModel[]>(_page);
			var pageOffset = new PageOffset(ofs ?? 0, GlobalConfig.Get().ItemsPerPage, _books.Count());

			var view = new BookViewSearchContainer { Books = books, Query = q.TrimOrNull(), PageOffset = pageOffset };

			return View(view);
		}

		[ChildActionOnly]
		public ActionResult Tags()
		{
			var _tags = Tag.Get().Where(s => s.References != 0).OrderByDescending(o => o.References).Take(5);

			var tags = Mapper.Map<TagModel[]>(_tags);
			
			return View("_PartialTags", tags);
		}

		[ChildActionOnly]
		public ActionResult Categories()
		{
			var _categories = Category.Get().Where(s => Category.Get(s.Id).GetBooks().Count() > 0);

			var categories = Mapper.Map<CategoryModel[]>(_categories);

			return View("_PartialCategories", categories);
		}

		[HttpGet]
		public ActionResult AcSearch(string term)
		{
			if (Request.IsAjaxRequest())
			{
				var suggestions = Book.SuggestionList(term ?? "");
				return new JsonResult { Data = suggestions, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
			}
			return new EmptyResult();
		}

		[HttpGet]
		public ActionResult AcTag(string term)
		{
			if (Request.IsAjaxRequest())
			{
				var suggestions = Tag.Get().Where(w => w.Name.ToLower().Contains(term.ToLower())).Select(s => s.Name).ToArray();
				return new JsonResult
				{
					Data = suggestions,
					JsonRequestBehavior = JsonRequestBehavior.AllowGet
				};
			}
			return new EmptyResult();
		}
    }
}
