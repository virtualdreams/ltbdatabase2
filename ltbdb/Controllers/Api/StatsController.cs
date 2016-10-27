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
		private readonly CategoryService Category;
		private readonly TagService Tag;

		public StatsController(BookService book, CategoryService category, TagService tag)
		{
			Book = book;
			Category = category;
			Tag = tag;
		}

		[HttpGet]
		public dynamic List()
		{
			var _books = Book.Get().Count();
			var _categories = Category.Get().Count();
			var _stories = Book.Get().SelectMany(s => s.Stories).Distinct().Count();
			var _tags = Tag.Get().Count();

			var _stats = new
			{
				Books = _books,
				Categories = _categories,
				Stories = _stories,
				Tags = _tags
			};

			return _stats;
		}
	}
}
