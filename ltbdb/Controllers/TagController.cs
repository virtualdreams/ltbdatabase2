using AutoMapper;
using Castle.Core.Logging;
using ltbdb.Core;
using ltbdb.Core.Filter;
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
    public class TagController : Controller
    {
		public ILogger log { get; set; }

		public ActionResult Index()
		{
			var _tags = Tag.Get().Where(s => s.References != 0).OrderBy(o => o.Name);

			var tags = Mapper.Map<TagModel[]>(_tags);

			var view = new TagViewModel { Tags = tags };

			return View(view);
		}

        public ActionResult View(int? id, int? ofs)
        {
			var _tag = Tag.Get(id ?? 0);
			var _books = _tag.GetBooks();
			var _page = _books.Skip(ofs ?? 0).Take(GlobalConfig.Get().ItemsPerPage);

			var tag = Mapper.Map<TagModel>(_tag);
			var books = Mapper.Map<BookModel[]>(_books);
			var pageOffset = new PageOffset(ofs ?? 0, GlobalConfig.Get().ItemsPerPage, _books.Count());

			var view = new BookViewTagModel { Books = books, Tag = tag, PageOffset = pageOffset };

			return View(view);
        }

		[OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
		[HttpGet]
		public ActionResult Add(int? id)
		{
			var view = new AddTagModel { Id = id ?? 0, Tag = "" };

			return View("_PartialAddTag", view);
		}

		[AjaxAuthorize]
		[OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
		[HttpPost]
		public ActionResult Add(AddTagModel model)
		{
			if (!ModelState.IsValid)
			{
				return View("_PartialAddTag", model);
			}

			// TODO Add tags -> make this stuff better

			var _tags = Book.Get(model.Id).AddTags(model.Tag.Split(',').Select(s => s.Trim()).ToArray());

			var tags = Mapper.Map<TagModel[]>(_tags);

			return new JsonResult { Data = new { tags = tags, bookid = model.Id }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
		}

		[AjaxAuthorize]
		[OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
		[HttpPost]
		public ActionResult Unlink(int? id, int? bookid)
		{
			if (!Request.IsAjaxRequest())
				return new EmptyResult();

			return new JsonResult { Data = new { Success = Book.Get(bookid ?? 0).Unlink(id ?? 0) }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
		}
    }
}
