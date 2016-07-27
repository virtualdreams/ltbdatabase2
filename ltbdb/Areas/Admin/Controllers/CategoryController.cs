using AutoMapper;
using log4net;
using ltbdb.Core.Filter;
using ltbdb.Core.Models;
using ltbdb.Core.Services;
using ltbdb.Models;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ltbdb.Areas.Admin.Controllers
{
	[Authorize]
	[LogError(Order = 0)]
	[HandleError(View = "Error", Order = 99)]
    public class CategoryController : Controller
    {
		private static readonly ILog Log = LogManager.GetLogger(typeof(CategoryController));

		private readonly BookService Book;
		private readonly TagService Tag;
		private readonly CategoryService Category;

		public CategoryController(BookService book, TagService tag, CategoryService category)
		{
			Book = book;
			Tag = tag;
			Category = category;
		}

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
			if (_category == null)
				throw new HttpException(404, "Ressource not found.");

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
			Category.Save(category);

			return RedirectToAction("index", "category");
		}
    }
}
