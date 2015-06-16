using ltbdb.Core;
using ltbdb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace ltbdb.Controllers
{
	[LogError(Order = 0)]
	[HandleError(View = "Error", Order = 99)]
    public class AccountController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

		[HttpGet]
		public ActionResult Login()
		{
			var login = new LoginModel();

			return View(login);
		}

		[HttpPost]
		public ActionResult Login(LoginModel model, string returnUrl)
		{
			if (!ModelState.IsValid)
			{
				return View("login", model);
			}

			// check login credentials
			if (GlobalConfig.Get().Username.Equals(model.Username, StringComparison.OrdinalIgnoreCase) && GlobalConfig.Get().Password.Equals(model.Password))
			{
				FormsAuthentication.SetAuthCookie(model.Username, false);
			}
			else
			{
				ModelState.AddModelError("failed", "Benutzername oder Passwort falsch.");
				return View("login", model);
			}

			// return to target page.
			if (Url.IsLocalUrl(returnUrl))
			{
				return Redirect(returnUrl);
			}
			else
			{
				return RedirectToAction("index", "home");
			}
		}

		[HttpGet]
		public ActionResult Logout()
		{
			FormsAuthentication.SignOut();

			return RedirectToAction("index", "home");
		}

		[ChildActionOnly]
		public ActionResult Status()
		{
			return View("_PartialLogin");
		}
    }
}
