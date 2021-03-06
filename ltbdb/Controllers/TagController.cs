﻿using AutoMapper;
using log4net;
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
	[LogError(Order = 0)]
	[HandleError(View = "Error", Order=99)]
    public class TagController : Controller
    {
		private static readonly ILog Log = LogManager.GetLogger(typeof(TagController));

		public ActionResult Index()
		{
			var _tags = Tag.Get();
			var _referencedTags = _tags.Where(s => s.References != 0).OrderBy(o => o.Name);
			var _unreferencedTags = _tags.Where(s => s.References == 0).OrderBy(o => o.Name);

			var referencedTags = Mapper.Map<TagModel[]>(_referencedTags);
			var unreferencedTags = Mapper.Map<TagModel[]>(_unreferencedTags);

			var view = new TagViewContainer { Tags = referencedTags, UnreferencedTags = unreferencedTags };

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

			var view = new BookViewTagContainer { Books = books, Tag = tag, PageOffset = pageOffset };

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

			var _tags = Book.Get(model.Id).AddTags(model.Tag.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim()).Where(w => !String.IsNullOrEmpty(w)).ToArray());

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

			return new JsonResult { Data = new { success = Book.Get(bookid ?? 0).Unlink(id ?? 0) }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
		}

		[Authorize]
		[HttpGet]
		public ActionResult Edit(int? id)
		{
			var _tag = Tag.Get(id ?? 0);

			var tag = Mapper.Map<TagModel>(_tag);

			var view = new TagEditContainer { Tag = tag };

			return View("edit", view);
		}

		[Authorize]
		[HttpPost]
		public ActionResult Edit(TagModel model)
		{
			if (!ModelState.IsValid)
			{
				var view = new TagEditContainer { Tag = model };

				return View("edit", view);
			}

			var tag = Mapper.Map<Tag>(model);
			Tag.Set(tag);

			return RedirectToAction("index", "tag");
		}

		[AjaxAuthorize]
		[HttpPost]
		public ActionResult Delete(int? id)
		{
			if (!Request.IsAjaxRequest())
				return new EmptyResult();

			return new JsonResult { Data = new { success = Tag.Delete(id ?? 0) }, JsonRequestBehavior = JsonRequestBehavior.DenyGet };
		}
    }
}
