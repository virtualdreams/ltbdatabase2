using log4net;
using ltbdb.Core.Filter;
using ltbdb.Core.Models;
using ltbdb.Core.Services;
using System.Linq;
using System.Web.Http;

namespace ltbdb.Controllers.Api
{
	[Authorize]
	[LogError(Order = 0)]
	public class StatsController : ApiController
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(StatsController));

		private readonly BookService Book;
		private readonly TagService Tag;
		private readonly CategoryService Category;

		public StatsController(BookService book, TagService tag, CategoryService category)
		{
			Book = book;
			Tag = tag;
			Category = category;
		}

		[HttpGet]
		public dynamic List()
		{
			var books = Book.Get().Count();
			var categories = Category.Get();
			var stories = Book.Get().SelectMany(s => s.Stories).Count();
			var tags = Tag.Get();

			var stats = new
			{
				Books = new {
					Total = books
				},
				Categories = new {
					Total = categories.Count(),
					Used = categories.Where(w => Book.GetByCategory(w.Id).Count() > 0).Count(),
					Unused = categories.Where(w => Book.GetByCategory(w.Id).Count() == 0).Count()
				},
				Stories = new {
					Total = stories
				},
				Tags = new {
					Total = tags.Count(),
					Used = tags.Where(w => w.References != 0).Count(),
					Unused = tags.Where(w => w.References == 0).Count()
				}
			};

			return stats;
		}
	}
}
