using SqlDataMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ltbdb.DomainServices
{
	public class DatabaseContext
	{
		public SqlConfig Config { get; private set; }
		public SqlContext Context { get; private set; }

		public DatabaseContext()
		{
			this.Config = HttpContext.Current.Items["config"] as SqlConfig;
			this.Context = HttpContext.Current.Items["context"] as SqlContext;
		}
	}
}