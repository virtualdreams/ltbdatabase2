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
	[HandleError(View = "Error")]
    public class BookController : Controller
    {
		[HttpGet]
		public ActionResult Index()
		{
			return View();
		}

		[HttpGet]
		public ActionResult View(int? id)
		{
			var _book = Book.GetBook(id ?? 0);
			var _tags = _book.GetTags();

			var book = Mapper.Map<BookModel>(_book);
			var tags = Mapper.Map<TagModel[]>(_tags);

			var view = new BookViewDetailModel { Book = book, Tags = tags };
			
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
