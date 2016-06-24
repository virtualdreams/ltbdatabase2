using ltbdb.Core.Filter;
using System.Web.Mvc;

namespace ltbdb.Areas.Admin.Controllers
{
	[Authorize]
	[LogError(Order = 0)]
	[HandleError(View = "Error", Order = 99)]
	public class StatsController : Controller
	{
		[HttpGet]
		public ActionResult Index()
		{
			return View();
		}
	}
}
