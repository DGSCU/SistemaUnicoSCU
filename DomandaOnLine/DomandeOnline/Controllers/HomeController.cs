using DomandeOnline.Code;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using DomandeOnline.Models;
using Microsoft.AspNet.Identity.Owin;
using DomandeOnline.Data;
using System.Linq;

namespace DomandeOnline.Controllers
{
	[Authorize]
	public class HomeController : SmartController
	{
		[AllowAnonymous]
		public ActionResult Index(string CodiceProgetto)
		{
			if (!string.IsNullOrEmpty(CodiceProgetto))
			{
				System.Web.HttpContext.Current.Session["CodiceProgetto"] = CodiceProgetto;
			}
			if (User.IsInRole(Role.AMMINISTRATORE))
			{	
				return RedirectToAction("Index", "Users");
			}
			if (User.IsInRole(Role.OPERATORE))
			{
				return RedirectToAction("ElencoRichieste", "RichiestaCredenziali");
			}
			if (User.IsInRole(Role.UTENTE_SPID) || User.IsInRole(Role.UTENTE_CREDENZIALI))
			{
				return RedirectToAction("SelezionaBando", "DomandaPartecipazione");
			}
			//Nel caso non abbia un ruolo, il ruolo viene creato
			if (User.Identity.IsAuthenticated)
			{
				var utente = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>()
						.FindById(User.Identity.GetUserId());
				var userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
				if (utente != null)
				{
					if (string.IsNullOrEmpty(utente.Spidcode) && !string.IsNullOrEmpty(utente.CodiceFiscale))
					{
						userManager.AddToRole(User.Identity.GetUserId(), Role.UTENTE_CREDENZIALI);

					}
					else if (!string.IsNullOrEmpty(utente.Spidcode) && !string.IsNullOrEmpty(utente.CodiceFiscale))
					{
						userManager.AddToRole(User.Identity.GetUserId(), Role.UTENTE_SPID);
					}
					ApplicationSignInManager SignInManager = HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
					SignInManager.SignIn(utente, false, false);
					return RedirectToAction("SelezionaBando", "DomandaPartecipazione");
				}
			}
			return View();
		}

		[AllowAnonymous]
		public ActionResult Regolamento()
		{
			return View();
		}

		[AllowAnonymous]
		public ActionResult Contact()
		{
			ViewBag.Message = "Your contact page.";

			return View();
		}

		[AllowAnonymous]
		[HttpPost]
		public ActionResult AcceptCookies(bool accetto = true)
		{
			if (Request.Cookies["CookieAccepted"] == null)
			{
				Response.Cookies["CookieAccepted"].Value = accetto ? "1" : "0";
			}
			return View("Index");
		}


	}
}