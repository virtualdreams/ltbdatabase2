using log4net;
using ltbdb.Core.Filter;
using ltbdb.Core.Models;
using ltbdb.Core.Services;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Net.Http;
using System.Net;

namespace ltbdb.Controllers.Api
{
	[LogError(Order = 0)]
	public class BookController : ApiController
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(BookController));

		private readonly BookService Book;

		public BookController(BookService book)
		{
			Book = book;
		}

		[HttpGet]
		public IEnumerable<dynamic> List()
		{
			foreach (var book in Book.Get().OrderBy(o => o.Category).ThenBy(o => o.Number))
			{
				yield return new
				{
					Number = book.Number,
					Title = book.Title,
					Category = book.Category,
					Created = book.Created,
					Filename = book.Filename,
					Stories = book.Stories,
					Tags = book.Tags
				};
			}
			
			//foreach (var book in Book.Get().OrderBy(o => o.Category.Name).ThenBy(o => o.Number))
			//{
			//	yield return new
			//	{
			//		Number = book.Number,
			//		Title = book.Name,
			//		Category = book.Category.Name,
			//		Created = book.Created,
			//		Filename = book.Filename,
			//		Stories = book.Stories,
			//		Tags = Tag.GetByBook(book.Id).Select(s => s.Name)
			//	};
			//}
		}
	}
}
