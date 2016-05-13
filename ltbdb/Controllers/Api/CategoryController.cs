using AutoMapper;
using ltbdb.DomainServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ltbdb.Controllers.Api
{
	[Authorize]
	public class CategoryController : ApiController
	{
		[HttpGet]
		public IEnumerable<dynamic> List()
		{
			var t = Category.Get();
			foreach (var c in t)
			{
				yield return new
				{
					name = c.Name,
					id = c.Id,
					used = Category.Get(c.Id).GetBooks().Count() != 0
				};
			}
		}

		[HttpPost]
		public dynamic Delete(int? id)
		{
			if((id ?? 0) == 0)
				throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "Id not found."));

			return new { Deleted = Category.Delete(id ?? 0) };
		}

		[HttpPost]
		public dynamic Move(int? from, int? to)
		{
			throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "Not implemented yet."));
		}
	}
}
