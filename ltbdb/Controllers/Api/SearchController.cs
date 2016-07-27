using log4net;
using ltbdb.Core.Filter;
using ltbdb.Core.Services;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace ltbdb.Controllers.Api
{
	[LogError(Order = 0)]
    public class SearchController : ApiController
    {
		private static readonly ILog Log = LogManager.GetLogger(typeof(SearchController));

		private readonly BookService Book;
		private readonly TagService Tag;
		private readonly CategoryService Category;

		public SearchController(BookService book, TagService tag, CategoryService category)
		{
			Book = book;
			Tag = tag;
			Category = category;
		}

		[HttpGet]
		public dynamic Title(string term)
		{
			return Book.Suggestion(term ?? "");
		}

		[HttpGet]
		public dynamic Tags(string term)
		{
			return Tag.Get().Where(w => w.Name.ToLower().Contains(term.ToLower())).Select(s => s.Name);
		}
    }
}
