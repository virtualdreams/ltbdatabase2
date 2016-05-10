using AutoMapper;
using ltbdb.DomainServices;
using ltbdb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ltbdb.Areas.Admin.Controllers
{
	[Authorize]
    public class CategoryController : Controller
    {
		[HttpGet]
        public ActionResult Index()
        {
			var _categories = Category.Get().OrderBy(o => o.Name);

			var categories = Mapper.Map<CategoryModel[]>(_categories);

			var view = new CategoryViewContainer { Categories = categories };

            return View(view);
        }

		[HttpGet]
		public ActionResult Edit(int? id)
		{
			var _category = Category.Get(id ?? 0);

			var category = Mapper.Map<CategoryModel>(_category);

			var view = new CategoryEditContainer { Category = category };

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

		[HttpPost]
		public ActionResult Edit(CategoryModel model)
		{
			if (!ModelState.IsValid)
			{
				var view = new CategoryEditContainer { Category = model };

				return View("edit", view);
			}

			var category = Mapper.Map<Category>(model);
			Category.Set(category);

			return RedirectToAction("index", "category");
		}
    }
}
