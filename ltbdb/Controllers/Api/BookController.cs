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
        public IEnumerable<ltbdb.Models.WebService.Book> Get()
        {
			var books = Book.Get().OrderBy(o => o.Category.Id);

			return Mapper.Map<ltbdb.Models.WebService.Book[]>(books);
        }

		public ltbdb.Models.WebService.Book Get(int id)
        {
			return Mapper.Map<ltbdb.Models.WebService.Book>(Book.Get(id));
        }
    }
}
