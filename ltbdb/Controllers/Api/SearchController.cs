using log4net;
using ltbdb.Core.Filter;
using ltbdb.Core.Services;
using System;
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
		private readonly CategoryService Category;
		private readonly TagService Tag;

		public SearchController(BookService book, CategoryService category, TagService tag)
		{
			Book = book;
			Category = category;
			Tag = tag;
		}

		[HttpGet]
		public dynamic Title(string term)
		{
			return Book.Suggestions(term ?? String.Empty);
		}

		[HttpGet]
		public dynamic Categories(string term)
		{
			return Category.Suggestions(term ?? String.Empty);
		}

		[HttpGet]
		public dynamic Tags(string term)
		{
			return Tag.Suggestions(term ?? String.Empty);
		}
    }
}
