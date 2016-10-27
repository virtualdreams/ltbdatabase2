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

		private readonly CategoryService Category;

		public CategoryController(CategoryService category)
		{
			Category = category;
		}

		[HttpGet]
		public dynamic List()
		{
			return Category.Get();
		}
	}
}
