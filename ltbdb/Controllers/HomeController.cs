﻿using AutoMapper;
using Castle.Core.Logging;
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
	public interface ICastle
	{
		string Hello();
	}


	public class DemoCastle : ICastle
	{

		public string Hello()
		{
			return "Demo";
		}
	}

    public class HomeController : Controller
    {
		public ILogger log { get; set; }
		private ICastle castle;

		public HomeController(ICastle castle)
		{
			this.castle = castle;
		}

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
		public ActionResult Search(string q)
		{
			var result = new Store().Search(q ?? "").Take(24);

			var books = Mapper.Map<BookModel[]>(result);

			var view = new BookViewSearchModel { Books = books, Query = q };

			return View(view);
		}

		[ChildActionOnly]
		public ActionResult Tags()
		{
			var _tags = new Store().GetTags();

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

		private void Test()
		{
			//Store store = new Store();
			//var recent = store.GetRecentlyAdded();
			//var book1 = store.GetBook(20);
			//var book2 = store.GetBook(300);
			//var book3 = store.GetBook(900);

			//var tags = store.GetTags();
			//var tag = store.GetTag(1);

			//var books = tag.GetBooks();

			//var tags1 = book1.GetTags();
			//var tags2 = book2.GetTags();

			//var stories = store.GetBook(1).GetStories();

			//var search = store.Search("onkel");
		}
    }
}
