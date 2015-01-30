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
			return Mapper.Map<ltbdb.Models.WebService.Book[]>(Book.Get());
        }

		public ltbdb.Models.WebService.Book Get(int id)
        {
			return Mapper.Map<ltbdb.Models.WebService.Book>(Book.Get(id));
        }
    }
}
