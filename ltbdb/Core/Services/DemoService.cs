using ltbdb.Core.Database;
using SqlDataMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ltbdb.Core.Services
{
	public class DemoService: DatabaseContextNew
	{
		public DemoService(SqlConfig config, SqlContext context)
			: base(config, context)
		{ }

		public void DoNothing()
		{
			var books = BookEntity.GetAll().ToList();
		}
	}
}