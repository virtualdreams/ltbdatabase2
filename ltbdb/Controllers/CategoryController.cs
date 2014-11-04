using AutoMapper;
using Castle.Core.Logging;
using ltbdb.Core;
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
    public class CategoryController : Controller
    {
		public ILogger log { get; set; }

		[HttpGet]
        public ActionResult Index(int? ofs)
        {
			var _books = Book.Get();
			var _page = _books.Skip(ofs ?? 0).Take(GlobalConfig.Get().ItemsPerPage);

			var books = Mapper.Map<BookModel[]>(_page);
			var pageOffset = new PageOffset(ofs ?? 0, GlobalConfig.Get().ItemsPerPage, _books.Count());

			var view = new BookViewAllModel { Books = books, PageOffset = pageOffset};

			return View(view);
        }

		[HttpGet]
		public ActionResult View(int? id, int? ofs)
		{
			var _category = Category.Get(id ?? 0);
			var _books = _category.GetBooks();
			var _page = _books.Skip(ofs ?? 0).Take(GlobalConfig.Get().ItemsPerPage);

			var category = Mapper.Map<CategoryModel>(_category);
			var books = Mapper.Map<BookModel[]>(_page);
			var pageOffset = new PageOffset(ofs ?? 0, GlobalConfig.Get().ItemsPerPage, _books.Count());

			var view = new BookViewCategoryModel { Books = books, Category = category, PageOffset = pageOffset };

			return View(view);
		}

		[HttpGet]
		public ActionResult Create()
		{
			var _category = new Category();

			var category = Mapper.Map<CategoryModel>(_category);

			var view = new CategoryEditContainer { Category = category };

			return View("edit", view);
		}

		[HttpGet]
		public ActionResult Edit(int? id)
		{
			var _category = Category.Get(id ?? 0);

			var category = Mapper.Map<CategoryModel>(_category);

			var view = new CategoryEditContainer { Category = category };
			
			return View(view);
		}

		[HttpPost]
		public ActionResult Edit(CategoryModel model)
		{
			if (!ModelState.IsValid)
			{
				var view = new CategoryEditContainer { Category = model };

				return View("edit", view);
			}

			//TODO Save the category.
			var category = Mapper.Map<Category>(model);
			Category.Set(category);

			return RedirectToAction("index", "home");
		}
    }
}
