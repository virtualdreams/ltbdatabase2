using AutoMapper;
using ltbdb.DomainServices;
using ltbdb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ltbdb.Controllers
{
    public class BookController : Controller
    {
		[HttpGet]
		public ActionResult Index()
		{
			return View();
		}

		[HandleError(View="Error")]
		[HttpGet]
		public ActionResult View(int? id)
		{
			var _book = new Store().GetBook(id ?? 0);
			var _stories = _book.GetStories();
			var _tags = _book.GetTags();

			var book = Mapper.Map<BookModel>(_book);
			var tags = Mapper.Map<TagModel[]>(_tags);
			var stories = Mapper.Map<StoryModel[]>(_stories);

			var view = new BookViewDetailModel { Book = book, Tags = tags, Stories = stories };
			
			return View(view);
		}

		[HttpGet]
		public ActionResult Create()
		{
			return View("Edit");
		}

		[HttpGet]
		public ActionResult Edit(int? id)
		{
			return View();
		}
    }
}
