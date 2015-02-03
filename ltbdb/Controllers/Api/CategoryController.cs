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
    public class CategoryController : ApiController
    {
		public IEnumerable<ltbdb.Models.WebService.Category> Get()
        {
			var categories = Category.Get().OrderBy(o => o.Id);

			return Mapper.Map<ltbdb.Models.WebService.Category[]>(categories);
        }

		public ltbdb.Models.WebService.Category Get(int id)
        {
			return Mapper.Map<ltbdb.Models.WebService.Category>(Category.Get(id));
        }
    }
}
