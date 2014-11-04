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
			var _book = new Book();
			var _categories = Category.GetCategories();

			var book = Mapper.Map<BookModel>(_book);
			book.Number = null;
			var categories = Mapper.Map<CategoryModel[]>(_categories);

			var view = new BookEditContainer { Book = book, Categories = categories };
			
			return View("edit", view);
		}

		[HttpGet]
		public ActionResult Edit(int? id)
		{
			var _book = Book.GetBook(id ?? 0);
			var _categories = Category.GetCategories();

			var book = Mapper.Map<BookModel>(_book);
			var categories = Mapper.Map<CategoryModel[]>(_categories);

			var view = new BookEditContainer { Book = book, Categories = categories };

			return View("edit", view);
		}

		[HttpPost]
		public ActionResult Edit(BookModel model)
		{
			if (!ModelState.IsValid)
			{
				var _categories = Category.GetCategories();

				var categories = Mapper.Map<CategoryModel[]>(_categories);

				var view = new BookEditContainer { Book = model, Categories = categories };

				return View("edit", view);
			}

			//TODO Save the book.

			return RedirectToAction("index", "home");
		}
    }
}
