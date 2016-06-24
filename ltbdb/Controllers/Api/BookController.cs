using log4net;
using ltbdb.Core.Filter;
using ltbdb.Core.Models;
using ltbdb.Core.Services;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace ltbdb.Controllers.Api
{
	[LogError(Order = 0)]
	public class BookController : ApiController
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
		public IEnumerable<dynamic> List()
		{
			foreach (var book in Book.Get().OrderBy(o => o.Category.Name).ThenBy(o => o.Number))
			{
				yield return new
				{
					Number = book.Number,
					Title = book.Name,
					Category = book.Category.Name,
					Added = book.Created,
					Stories = book.Stories,
					Tags = Tag.GetByBook(book.Id).Select(s => s.Name)
				};
			}
		}
	}
}
