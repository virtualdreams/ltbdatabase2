using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ltbdb.Core.Filter
{
	/// <summary>
	/// Allow only authorized ajax requests.
	/// </summary>
	public class AjaxAuthorizeAttribute: AuthorizeAttribute
	{
		protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
		{
			if (filterContext.HttpContext.Request.IsAjaxRequest())
			{
				filterContext.HttpContext.Response.StatusCode = 403;
				filterContext.Result = new JsonResult { Data = new { error = "NotAuthorized" }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
			}
			else
			{
				base.HandleUnauthorizedRequest(filterContext);
			}
		}
	}
}