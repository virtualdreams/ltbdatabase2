using AutoMapper;
using log4net;
using ltbdb.Core;
using ltbdb.Core.Filter;
using ltbdb.Core.Models;
using ltbdb.Core.Services;
using ltbdb.Models;
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
		private readonly TagService Tag;
		private readonly CategoryService Category;

		public BookController(BookService book, TagService tag, CategoryService category)
		{
			Book = book;
			Tag = tag;
			Category = category;
		}

		[HttpGet]
		public ActionResult View(int? id)
		{
			var _book = Book.Get(id ?? 0);
			if (_book == null)
				throw new HttpException(404, "Resource not found.");

			var _tags = Tag.GetByBook(_book.Id);

			var book = Mapper.Map<BookModel>(_book);
			var tags = Mapper.Map<TagModel[]>(_tags);

			var view = new BookViewDetailContainer
			{
				Book = book,
				Tags = tags
			};
			
			return View(view);
		}

		[Authorize]
		[HttpGet]
		public ActionResult Create()
		{
			var _book = new Book();
			var _categories = Category.Get();

			var book = Mapper.Map<BookWriteModel>(_book);
			book.Number = null;
			var categories = Mapper.Map<CategoryModel[]>(_categories);

			var view = new BookEditContainer
			{
				Book = book,
				Categories = categories
			};
			
			return View("edit", view);
		}

		[Authorize]
		[HttpGet]
		public ActionResult Edit(int? id)
		{
			var _book = Book.Get(id ?? 0);
			if (_book == null)
				throw new HttpException(404, "Resource not found.");

			var _categories = Category.Get();

			var book = Mapper.Map<BookWriteModel>(_book);
			var categories = Mapper.Map<CategoryModel[]>(_categories);

			var view = new BookEditContainer
			{
				Book = book,
				Categories = categories
			};

			return View("edit", view);
		}

		[Authorize]
		[HttpPost]
		public ActionResult Edit(BookWriteModel model)
		{
			if (!ModelState.IsValid)
			{
				var _categories = Category.Get();

				var categories = Mapper.Map<CategoryModel[]>(_categories);

				var view = new BookEditContainer
				{
					Book = model,
					Categories = categories 
				};

				return View("edit", view);
			}

			var book = Mapper.Map<Book>(model);
			var result = Book.Save(book);

			if (model.Image != null || model.Remove)
			{
				if (model.Remove)
				{
					Book.SetImage(result, null);
				}
				else
				{
					Book.SetImage(result, model.Image.InputStream);
				}
			}

			return RedirectToAction("view", "book", new { id = result.Id });
		}
    }
}
