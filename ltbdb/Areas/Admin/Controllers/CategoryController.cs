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
		private readonly CategoryService Category;

		public CategoryController(BookService book, CategoryService category)
		{
			Book = book;
			Category = category;
		}

		[HttpGet]
        public ActionResult Index()
        {
            return View();
        }

		[IsAjaxRequest]
		[HttpPost]
		public ActionResult Move(string from, string to)
		{
			var _result = Category.Rename(from.Trim(), to.Trim());
			
			return new JsonResult { Data = new { Success = _result }, JsonRequestBehavior = JsonRequestBehavior.DenyGet };
		}
    }
}
