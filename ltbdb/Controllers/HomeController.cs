﻿using AutoMapper;
using Castle.Core.Logging;
using ltbdb.Core;
using ltbdb.DomainServices;
using ltbdb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace ltbdb.Controllers
{
	[HandleError(View = "Error")]
    public class HomeController : Controller
    {
		public ILogger log { get; set; }

		[HttpGet]
        public ActionResult Index()
        {
			var result = new Store().GetRecentlyAdded();

			var books = Mapper.Map<BookModel[]>(result);

			var view = new BookViewModel { Books = books };

			return View(view);
        }

		[ValidateInput(false)]
		[HttpGet]
		public ActionResult Search(string q, int? ofs)
		{
			var _books = new Store().Search(q ?? "");
			var _page = _books.Skip(ofs ?? 0).Take(GlobalConfig.Get().ItemsPerPage);

			var books = Mapper.Map<BookModel[]>(_page);
			var pageOffset = new PageOffset(ofs ?? 0, GlobalConfig.Get().ItemsPerPage, _books.Count());

			var view = new BookViewSearchModel { Books = books, Query = q, PageOffset = pageOffset };

			return View(view);
		}

		[ChildActionOnly]
		public ActionResult Tags()
		{
			var _tags = new Store().GetTags().Where(s => s.References != 0);

			var tags = Mapper.Map<TagModel[]>(_tags);
			
			return View("_PartialTags", tags);
		}

		[ChildActionOnly]
		public ActionResult Categories()
		{
			var _categories = new Store().GetCategories();

			var categories = Mapper.Map<CategoryModel[]>(_categories);

			return View("_PartialCategories", categories);
		}
    }
}
