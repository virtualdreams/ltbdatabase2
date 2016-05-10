using System;
using System.Collections.Generic;
using System.Linq;
using ltbdb.DomainServices;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web;

namespace ltbdb.Controllers.Api
{
	[Authorize]
	public class StatsController : ApiController
	{
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
				used = t.Where(w => Category.Get(w.Id).GetBooks().Count() > 0).Count(),
				unused = t.Where(w => Category.Get(w.Id).GetBooks().Count() == 0).Count(),
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
