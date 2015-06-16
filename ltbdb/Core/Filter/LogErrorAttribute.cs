using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ltbdb.Core
{
	public class LogErrorAttribute : FilterAttribute, IExceptionFilter
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(LogErrorAttribute));

		public void OnException(ExceptionContext context)
		{
			if (context != null && context.Exception != null)
			{
				Log.Fatal("LogErrorsFilter", context.Exception);
			}
		}
	}
}