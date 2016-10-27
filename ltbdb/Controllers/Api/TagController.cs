using log4net;
using ltbdb.Core.Filter;
using ltbdb.Core.Services;
using ltbdb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ltbdb.Controllers.Api
{
	[LogError(Order = 0)]
	public class TagController : ApiController
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(TagController));

		private readonly TagService Tag;

		public TagController(TagService tag)
		{
			Tag = tag;
		}

		[HttpGet]
		public dynamic List()
		{
			return Tag.Get();
		}
	}
}
