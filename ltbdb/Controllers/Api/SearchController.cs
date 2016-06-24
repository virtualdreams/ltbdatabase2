using ltbdb.Core.Filter;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace ltbdb.Controllers.Api
{
	//TODO modify return types
	[LogError(Order = 0)]
    public class SearchController : ApiController
    {
		[HttpGet]
		public IEnumerable<string> Title(string term)
		{
			return Enumerable.Empty<string>(); //Book.SuggestionList(term ?? "");
		}

		[HttpGet]
		public IEnumerable<string> Tag(string term)
		{
			return Enumerable.Empty<string>(); //ltbdb.DomainServices.Tag.Get().Where(w => w.Name.ToLower().Contains(term.ToLower())).Select(s => s.Name).ToArray();
		}
    }
}
