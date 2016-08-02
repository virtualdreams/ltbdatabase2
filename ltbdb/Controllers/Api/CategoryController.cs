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
				var _books = Book.GetByCategory(c.Id);

				yield return new
				{
					Name = c.Name,
					Id = c.Id,
					Count = _books.Count(),
					Used = _books.Count() != 0
				};
			}
		}

		[HttpPost]
		public dynamic Delete(int id)
		{
			var _category = Category.Get(id);
			if (_category == null)
				throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "Resource not found."));

			var result = false;
			if (!(Book.GetByCategory(_category.Id).Count() > 0))
				result = Category.Delete(_category);

			return new
			{
				success = result
			};
		}

		[HttpPost]
		public dynamic Move(int? from, int? to)
		{
			var _source = Category.Get(from ?? 0);
			var _target = Category.Get(to ?? 0);

			if(_source == null || _target == null)
				throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "Resource not found."));

			var result = Category.Move(_source, _target);

			return new
			{
				success = result
			};
		}
	}
}
