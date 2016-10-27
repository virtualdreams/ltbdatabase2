using AutoMapper;
using log4net;
using ltbdb.Core;
using ltbdb.Core.Filter;
using ltbdb.Core.Models;
using ltbdb.Core.Services;
using ltbdb.Models;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace ltbdb.Controllers
{
	[LogError(Order = 0)]
	[HandleError(View = "Error", Order=99)]
    public class BookController : Controller
    {
		private static readonly ILog Log = LogManager.GetLogger(typeof(BookController));

		private readonly BookService Book;

		public BookController(BookService book)
		{
			Book = book;
		}

		[HttpGet]
		public ActionResult View(ObjectId id)
		{
			var _book = Book.GetById(id);
			if (_book == null)
				throw new HttpException(404, "Resource not found.");

			var book = Mapper.Map<BookModel>(_book);

			var view = new BookViewDetailContainer
			{
				Book = book,
			};
			
			return View(view);
		}

		[Authorize]
		[HttpGet]
		public ActionResult Create()
		{
			var _book = new Book();

			var book = Mapper.Map<BookWriteModel>(_book);
			book.Number = null;

			var view = new BookEditContainer
			{
				Book = book
			};
			
			return View("edit", view);
		}

		[Authorize]
		[HttpGet]
		public ActionResult Edit(ObjectId id)
		{
			var _book = Book.GetById(id);
			if (_book == null)
				throw new HttpException(404, "Resource not found.");

			var book = Mapper.Map<BookWriteModel>(_book);

			var view = new BookEditContainer
			{
				Book = book
			};

			return View("edit", view);
		}

		[Authorize]
		[HttpPost]
		public ActionResult Edit(BookWriteModel model)
		{
			if (!ModelState.IsValid)
			{
				var view = new BookEditContainer
				{
					Book = model
				};

				return View("edit", view);
			}

			var book = Mapper.Map<Book>(model);
			var id = ObjectId.Empty;
			if (book.Id == ObjectId.Empty)
				id = Book.Create(book);
			else
				id = Book.Update(book);

			// TODO - save image to gridfs
			//if (model.Image != null || model.Remove)
			//{
			//	if (model.Remove)
			//	{
			//		MySqlBook.SetImage(result, null);
			//	}
			//	else
			//	{
			//		MySqlBook.SetImage(result, model.Image.InputStream);
			//	}
			//}

			return RedirectToAction("view", "book", new { id = id });
		}

		[Authorize]
		[IsAjaxRequest]
		[HttpPost]
		public ActionResult Delete(ObjectId id)
		{
			var _result = Book.Delete(id);

			return new JsonResult { Data = new { Success = _result }, JsonRequestBehavior = JsonRequestBehavior.DenyGet };
		}
    }
}
