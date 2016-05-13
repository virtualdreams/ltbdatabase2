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
		public IEnumerable<ltbdb.Models.WebService.Book> List()
		{
			var books = Book.Get().OrderBy(o => o.Category.Id).ThenBy(o => o.Number);

			return Mapper.Map<ltbdb.Models.WebService.Book[]>(books);
		}
		
		//[HttpGet]
		//public IEnumerable<ltbdb.Models.WebService.Book> Get()
		//{
		//	var books = Book.Get().OrderBy(o => o.Category.Id);

		//	return Mapper.Map<ltbdb.Models.WebService.Book[]>(books);
		//}

		//[HttpGet]
		//public ltbdb.Models.WebService.Book Get(int id)
		//{
		//	return Mapper.Map<ltbdb.Models.WebService.Book>(Book.Get(id));
		//}
	}
}
