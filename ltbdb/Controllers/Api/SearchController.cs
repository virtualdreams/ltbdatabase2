using ltbdb.Core;
using ltbdb.DomainServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ltbdb.Controllers.Api
{
    public class SearchController : ApiController
    {
		[HttpGet]
		public IEnumerable<string> Title(string term)
		{
			return Book.SuggestionList(term ?? "");
		}

		[HttpGet]
		public IEnumerable<string> Tag(string term)
		{
			return ltbdb.DomainServices.Tag.Get().Where(w => w.Name.ToLower().Contains(term.ToLower())).Select(s => s.Name).ToArray();
		}
    }
}
