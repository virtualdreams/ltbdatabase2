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
	public class BookController : ApiController
	{
		[HttpGet]
		public IEnumerable<dynamic> List()
		{
			foreach (var book in Book.Get().OrderBy(o => o.Category.Name).ThenBy(o => o.Number))
			{
				yield return new
				{
					Number = book.Number,
					Title = book.Name,
					Category = book.Category.Name,
					Added = book.Created,
					Stories = book.Stories,
					Tags = book.GetTags().Select(s => s.Name).ToArray()
				};
			}
		}
	}
}
