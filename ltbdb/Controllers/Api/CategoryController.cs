using log4net;
using ltbdb.Core.Filter;
using ltbdb.Core.Models;
using ltbdb.Core.Services;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ltbdb.Controllers.Api
{
	[Authorize]
	[LogError(Order = 0)]
	public class CategoryController : ApiController
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
		public IEnumerable<dynamic> List()
		{
			var t = Category.Get();
			foreach (var c in t)
			{
				yield return new
				{
					name = c.Name,
					id = c.Id,
					used = Book.GetByCategory(c.Id).Count() != 0
				};
			}
		}

		[HttpPost]
		public dynamic Delete(int? id)
		{
			throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "Not implemented yet."));
			//if((id ?? 0) == 0)
			//	throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "Id not found."));

			//return new { Deleted = Category.Delete(id ?? 0) };
		}

		[HttpPost]
		public dynamic Move(int? from, int? to)
		{
			throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "Not implemented yet."));
		}
	}
}
