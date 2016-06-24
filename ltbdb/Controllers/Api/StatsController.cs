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
		public dynamic Books()
		{
			var t = Book.Get().Count();

			return new
			{
				total = t,
				used = t,
				unused = 0
			};
		}

		[HttpGet]
		public dynamic Categories()
		{
			var t = Category.Get();
			return new
			{
				total = t.Count(),
				used = t.Where(w => Book.GetByCategory(w.Id).Count() > 0).Count(),
				unused = t.Where(w => Book.GetByCategory(w.Id).Count() == 0).Count()
			};
		}

		[HttpGet]
		public dynamic Stories()
		{
			var t = Book.Get().SelectMany(s => s.Stories).Count();

			return new
			{
				total = t,
				used = t,
				unused = 0
			};
		}

		[HttpGet]
		public dynamic Tags()
		{
			var t = Tag.Get();

			return new
			{
				total = t.Count(),
				used = t.Where(w => w.References != 0).Count(),
				unused = t.Where(w => w.References == 0).Count()
			};
		}
	}
}
