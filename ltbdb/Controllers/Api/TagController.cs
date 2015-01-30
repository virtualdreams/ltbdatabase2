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
    public class TagController : ApiController
    {
		public IEnumerable<ltbdb.Models.WebService.Tag> Get()
        {
			return Mapper.Map<ltbdb.Models.WebService.Tag[]>(Tag.Get());
        }

		public ltbdb.Models.WebService.Tag Get(int id)
        {
			return Mapper.Map<ltbdb.Models.WebService.Tag>(Tag.Get(id));
        }
    }
}
