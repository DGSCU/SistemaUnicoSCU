using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using DomandeOnline.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using DomandeOnline.Data;
using System.Transactions;
using DomandeOnline.Code;
using Serilog.Context;
using DomandeOnline.Autenticazione;
using System.Xml;
using System.Collections.Generic;
using DomandeOnline.Spid;
using System.Net;
using System.Configuration;

namespace DomandeOnline.Controllers
{
	[Authorize]
	public class AccountController : SmartController
	{
		private ApplicationSignInManager _signInManager;
		private ApplicationUserManager _userManager;

		public AccountController()
		{
		}

		public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager )
		{
			UserManager = userManager;
			SignInManager = signInManager;
		}

		public ApplicationSignInManager SignInManager
		{
			get
			{
				return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
			}
			private set 
			{ 
				_signInManager = value; 
			}
		}

		public ApplicationUserManager UserManager
		{
			get
			{
				return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
			}
			private set
			{
				_userManager = value;
			}
		}

		[HttpPost]
		[AllowAnonymous]
		public ActionResult CheckCredenziali(string userName, string password)
		{
			using (LogContext.PushProperty("Action", System.Reflection.MethodBase.GetCurrentMethod().Name))
			using (LogContext.PushProperty("Controller", System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name))
			using (LogContext.PushProperty("userName", userName))
			{
				Log.SetUsername(userName);
				if (string.IsNullOrEmpty(userName))
				{
					return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "userName non specificato");
				}
				if (string.IsNullOrEmpty(password))
				{
					return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "password non specificato");
				}
				var user = UserManager.FindByName(userName);
				if (user==null)
				{
					Log.Information(LogEvent.DOMANDA_ONLINE,"Tentativo di accesso ad utenza inesistente.",parameters:new { username=userName});
					return Json(new
					{
						success = false,
						message = "Credenziali non valide."
						
					});
				}
				if (user != null && !UserManager.IsInRole(user.Id, Role.UTENTE_CREDENZIALI))
				{
					Log.Information(LogEvent.DOMANDA_ONLINE,"Tentativo di accesso ad utenza senza credenziali", parameters: new { username = userName });
					return Json(new {
						success=false,
						message="Credenziali non valide."
					});
				}
				if(SignInManager.UserManager.CheckPassword(user, password??""))
				{
					Log.Information(LogEvent.DOMANDA_ONLINE,"Tentativo di accesso con credenziali valide", parameters: new { username = userName });
					return Json(new
					{
						success = true,
						message = "Credenziali valide."
					});
				}
				else
				{
					Log.Information(LogEvent.DOMANDA_ONLINE,"Tentativo di accesso con credenziali non valide", parameters: new { username = userName });
					return Json(new
					{
						success = false,
						message = "Credenziali non valide."
					});
				}

			}
		}

		[HttpPost]
		[AllowAnonymous]
		public ActionResult CambiaPasswordJson(string userName, string passwordAttuale, string password)
		{
			using (LogContext.PushProperty("Action", System.Reflection.MethodBase.GetCurrentMethod().Name))
			using (LogContext.PushProperty("Controller", System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name))
			using (LogContext.PushProperty("UserName", userName))
			using (var scope = new TransactionScope())
			{
				using (var context = new Entities())
				{
					var user = UserManager.FindByName(userName);
					if (user == null)
					{
						Log.Information(LogEvent.DOMANDA_ONLINE,"Tentativo di cambio password ad utenza inesistente.");
						return Json(new
						{
							success = false,
							message = "Credenziali non valide."
						});
					}
					if (user != null && !UserManager.IsInRole(user.Id, Role.UTENTE_CREDENZIALI))
					{
						Log.Information(LogEvent.DOMANDA_ONLINE,"Tentativo di cambio password ad utenza senza credenziali");
						return Json(new
						{
							success = false,
							message = "Credenziali non valide."
						});
					}
					if (SignInManager.UserManager.CheckPassword(user, passwordAttuale ?? ""))
					{
						var result = UserManager.ChangePassword(user.Id, passwordAttuale, password);
						if (!result.Succeeded)
						{
							string[] errors = result.Errors
											.First()
											.Split(".".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
							for (int i = 0; i < errors.Length; i++)
							{
								errors[i] += ".";
							} 
							Log.Information(LogEvent.DOMANDA_ONLINE,$"Tentativo di cambio password con errori:{string.Join(", ", errors)}");
							

							return Json(new
							{
								success = false,
								message = "Errore nella password inserita.",
								errors = errors
							}); ;
						}
						scope.Complete();
						Log.Information(LogEvent.DOMANDA_ONLINE,"Password cambiata");
						return Json(new
						{
							success = true,
							message = "Password cambiata."
						});
					}
					else
					{
						Log.Information(LogEvent.DOMANDA_ONLINE,"Tentativo di cambio password con credenziali non valide");
						return Json(new
						{
							success = false,
							message = "Credenziali non valide."
						});
					}


				}
			}
		}

		/// <summary>
		/// Azione per il recupero della password. Viene inviato un token via email con il quale è possibile reimpostare la password
		/// </summary>
		/// <param name="codiceFiscale"></param>
		/// <returns></returns>
		[HttpPost]
		[AllowAnonymous]
		public ActionResult RecuperoPasswordJson(string codiceFiscale, string returnUrl)
		{
			using (LogContext.PushProperty("Action", System.Reflection.MethodBase.GetCurrentMethod().Name))
			using (LogContext.PushProperty("Controller", System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name))
			using (LogContext.PushProperty("CodiceFiscale", codiceFiscale))
			using (LogContext.PushProperty("ReturnUrl", returnUrl))
			{
				if (string.IsNullOrEmpty(codiceFiscale))
				{
					return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "codiceFiscale non specificato");
				}
				try
				{
					using (var scope = new TransactionScope())
					{
						using (var context = new Entities())
						using (var userContext = new ApplicationDbContext())
						{
							ApplicationUser user;
							var userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
							try
							{
								user = userManager.FindByName(codiceFiscale);
								if (user == null || !UserManager.IsInRole(user.Id, Role.UTENTE_CREDENZIALI))
								{
									Log.Information(LogEvent.DOMANDA_ONLINE,"Tentativo di recupero della password per un codice fiscale non presente");
									return Json(new
									{
										success = false,
										message = "Utenza inesistente."
									});
								}

							}
							catch (Exception e)
							{
								Log.Error(LogEvent.DOMANDA_ONLINE,"Errore nel recupero dell'utenza nel tentativo di recupero della password", e);
								return Json(new
								{
									success = false,
									message = "Errore interno. Riprovare più tardi"
								});
							}
							/* Generazione del Token */
							TokenCredenziali token;
							try
							{
								token = new TokenCredenziali()
								{
									Id = Guid.NewGuid(),
									DataGenerazione = DateTime.Now,
									Scadenza = DateTime.Now.AddHours(24),
									UserId = user.Id,
									code = userManager.GeneratePasswordResetToken(user.Id)
								};
								context.TokenCredenziali.Add(token);
								context.SaveChanges();
							}
							catch (Exception e)
							{
								Log.Error(LogEvent.DOMANDA_ONLINE, "Errore nella creazione del token nel DB",e);
								return Json(new
								{
									success = false,
									message = "Errore interno. Riprovare più tardi"
								});
							}
							try
							{
								using (var emailService = new Email.ServiceClient())
								{
									string urlSito = ConfigurationManager.AppSettings["UrlSito"];
									string urlToken = Url.Action("ImpostaPassword", "Account", new { userId = user.Id, tokenId = token.Id, code = token.code , returnUrl = returnUrl }, Request.Url.Scheme);
									urlToken = urlSito + urlToken.Substring(urlToken.IndexOf("/Account/ImpostaPassword", 0));
									if (!emailService.emailNew(
										"supporto@serviziocivile.it",
										user.Email,
										"Credenziali di accesso alla piattaforma DOL – Recupero Password",
										$"Ciao,<br />è stato richiesto di recuperare la password.<br />Per re-impostare la password vai a questo <a href=\"{ urlToken}\">link</a>.<br />" +
										$"La nuova password dovrà essere di almeno 12 caratteri, avere almeno una lettera minuscola, almeno una lettera maiuscola, almeno un numero e almeno un carattere non alfanumerico",
										"",
										true,
										"",
										true))
									{
										throw new Exception("Errore generico invio email");
									}
								}
								scope.Complete();
								Log.Information(LogEvent.DOMANDA_ONLINE,"Richiesta di recupero credenziali effettuata");
								return Json(new
								{
									success = true,
									message = $"È stata inviata una e-mail per il recupero della password all'indirizzo {Utils.HideEmail(user.Email)}"
								}) ;
							}
							catch (Exception e)
							{
								Log.Error(LogEvent.DOMANDA_ONLINE, "Errore nell'invio della email",e);
								return Json(new
								{
									success = false,
									message = "Errore interno. Riprovare più tardi"
								});
							}

						}
					}
				}
				catch (Exception e)
				{
					Log.Error(LogEvent.DOMANDA_ONLINE, "Errore imprevisto",e);
					ViewData["Errore"] = "Errore interno - Riprovare più tardi";
					return View();
				}
			}
		}


		[AllowAnonymous]
		public ActionResult LoginCredenziali()
		{
			return View();
		}

		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public ActionResult LoginCredenziali(string userName, string password, bool? ricordami)
		{
			var parameters = new { userName, ricordami };
			var user = UserManager.FindByName(userName);
			if (user!=null&&!UserManager.IsInRole(user.Id,Role.UTENTE_CREDENZIALI))
			{
				Log.Information(LogEvent.DOMANDA_ONLINE,"Tentativo di accesso ad utenza senza credenziali",parameters: parameters);
				ViewData["Errore"] = "Tentativo di accesso non riuscito.";
				return View();
			}
			SignInStatus result = SignInManager.PasswordSignIn(userName, password, ricordami ?? false, shouldLockout: false);
			switch (result)
			{
				case SignInStatus.Success:
					Log.SetUsername(userName);
					Log.Information(LogEvent.DOMANDA_ONLINE,"Accessto tramite credenziali effettuato");
					return RedirectToAction("Index", "DomandaPartecipazione");
				case SignInStatus.LockedOut:
					ViewData["Errore"] = "Account Bloccato";
					break;
				case SignInStatus.RequiresVerification:
				case SignInStatus.Failure:
				default:
					Log.Information(LogEvent.DOMANDA_ONLINE,"Tentativo di accesso tramite credenziali fallito", parameters: parameters);
					ViewData["Errore"] = "Tentativo di accesso non riuscito.";
					break;
			}
			return View();
		}

		public ActionResult LogOff()
		{
			{
				bool utenteSPID = User.IsInRole(Role.UTENTE_SPID);
				bool utenteInterno= User.IsInRole(Role.INTERNO);
				AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
				Log.Information(LogEvent.DOMANDA_ONLINE,"Logout effettuato");
				if (utenteSPID)
				{
					return Redirect("https://spid.politichegiovanilieserviziocivile.gov.it/Home/LogoutRequest");
				}
				return RedirectToAction("Index", "Home");
			}
		}

		
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (_userManager != null)
				{
					_userManager.Dispose();
					_userManager = null;
				}

				if (_signInManager != null)
				{
					_signInManager.Dispose();
					_signInManager = null;
				}
			}

			base.Dispose(disposing);
		}

		#region Helper
		// Usato per la protezione XSRF durante l'aggiunta di account di accesso esterni
		private const string XsrfKey = "XsrfId";

		private IAuthenticationManager AuthenticationManager
		{
			get
			{
				return HttpContext.GetOwinContext().Authentication;
			}
		}

		private void AddErrors(IdentityResult result)
		{
			foreach (var error in result.Errors)
			{
				ModelState.AddModelError("", error);
			}
		}

		private ActionResult RedirectToLocal(string returnUrl)
		{
			if (Url.IsLocalUrl(returnUrl))
			{
				return Redirect(returnUrl);
			}
			return RedirectToAction("Index", "Home");
		}

		internal class ChallengeResult : HttpUnauthorizedResult
		{
			public ChallengeResult(string provider, string redirectUri)
				: this(provider, redirectUri, null)
			{
			}

			public ChallengeResult(string provider, string redirectUri, string userId)
			{
				LoginProvider = provider;
				RedirectUri = redirectUri;
				UserId = userId;
			}

			public string LoginProvider { get; set; }
			public string RedirectUri { get; set; }
			public string UserId { get; set; }

			public override void ExecuteResult(ControllerContext context)
			{
				var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
				if (UserId != null)
				{
					properties.Dictionary[XsrfKey] = UserId;
				}
				context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
			}
		}
		#endregion


		[HttpGet]
		public ActionResult CambiaPassword()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult CambiaPassword(string passwordAttuale, string password, string ripetiPassword)
		{
			using (LogContext.PushProperty("Action", System.Reflection.MethodBase.GetCurrentMethod().Name))
			using (LogContext.PushProperty("Controller", System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name))
			using (LogContext.PushProperty("User", User.Identity.GetUserId()))
			using (var scope = new TransactionScope())
			{
				using (var context = new Entities())
				{
					/* Controllo coincidenza passwords */
					if (password != ripetiPassword)
					{
						ViewData["Errore"] = "Le passwords non coincidono";
						return View();
					}
					
					var resultReset = UserManager.ChangePassword(User.Identity.GetUserId(), passwordAttuale, password);
					if (!resultReset.Succeeded)
					{
						ViewData["Errore"] = string.Join("<br>", resultReset.Errors).Replace(".",".<br>");
						return View();
					}
					scope.Complete();
					Log.Information(LogEvent.DOMANDA_ONLINE,"Cambiata password");
					return View("ConfermaImpostazionePassword");
				}
			}
		}



		[HttpGet]
		[AllowAnonymous]
		public ActionResult ImpostaPassword(string userId, Guid? tokenId, string code, string returnUrl)
		{
			ApplicationUser user;
			using (var context = new ApplicationDbContext())
			using (var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context)))
			{
				var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

				user = userManager.FindById(userId);
				if (user == null)
				{
					ViewData["Errore"] = "Token non valido";
					return View();
				}
			}
			using (var context = new Entities())
			{
				var token = context.TokenCredenziali.Where(t => t.Id == tokenId && t.UserId == user.Id).SingleOrDefault();
				if (token == null)
				{
					ViewData["Errore"] = "Token non valido";
					return View();
				}
				if (token.Scadenza < DateTime.Now)
				{
					ViewData["Errore"] = "Token scaduto - Procedere nuovamente al recupero della password";
					return View();
				}
				if (token.DataUtilizzo != null)
				{
					ViewData["Errore"] = "Token già utilizzato";
					return View();
				}
			}
			ViewData["UserId"] = userId;
			ViewData["TokenId"] = tokenId;
			ViewData["Code"] = code;
			ViewData["ReturnUrl"] = returnUrl;
			return View();
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		[AllowAnonymous]
		public ActionResult ImpostaPassword(string userId, Guid? tokenId, string code, string password, string ripetiPassword, string returnUrl)
		{
			using (LogContext.PushProperty("Action", System.Reflection.MethodBase.GetCurrentMethod().Name))
			using (LogContext.PushProperty("Controller", System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name))
			using (LogContext.PushProperty("User", User.Identity.GetUserId()))
			using (LogContext.PushProperty("userId", userId))
			using (var scope = new TransactionScope())
			{
				using (var context = new Entities())
				{
					/* Controllo coincidenza passwords */
					if (password!=ripetiPassword)
					{
						ViewData["ErroreRichiesta"] = "Le passwords non coincidono";
						return View("ImpostaPassword");
					}
					/* Recupero utenza */
					ApplicationUser user;
					try
					{
						user = UserManager.FindById(userId);
						if (user == null)
						{
							ViewData["ErroreRichiesta"] = "Utente Inesistente - È possibile che l'utenza sia stata disabilitata. Procedere alla richiesta di credenziali";
							return View("ImpostaPassword");
						}
					}
					catch (Exception e)
					{
						Log.Error(LogEvent.DOMANDA_ONLINE, "Errore nel recupero dell'utenza",e);
						ViewData["ErroreRichiesta"] = "Errore interno";
						return View("ImpostaPassword");
					}
					TokenCredenziali token = context.TokenCredenziali.Where(t=>t.Id==tokenId).SingleOrDefault();
					if (token==null)
					{
						ViewData["ErroreRichiesta"] = "Token non valido";
						return View("ImpostaPassword");
					}
					token.DataUtilizzo = DateTime.Now;
					context.SaveChanges();
					//var resultReset = UserManager.RemovePassword(userId);
					//if (!resultReset.Succeeded)
					//{
					//	ViewData["ErroreRichiesta"] = string.Join("<br>", resultReset.Errors).Replace(".", ".<br>");
					//	return View("ImpostaPassword");
					//}
					//resultReset = UserManager.AddPassword(userId, password);
					var resultReset = UserManager.ResetPassword(user.Id,code,password);
					if (!resultReset.Succeeded)
					{
						ViewData["ErroreRichiesta"] = string.Join("<br>", resultReset.Errors).Replace(".",".<br>");
						return View("ImpostaPassword");
					}
					scope.Complete();
					Log.Information(LogEvent.DOMANDA_ONLINE,"Reimpostata password");
					if (string.IsNullOrEmpty(returnUrl))
					{
						return View("ConfermaImpostazionePassword");
					}
					else {
						return Redirect(returnUrl);
					}
				}
			}
		}

		[HttpGet]
		[AllowAnonymous]
		public ActionResult RecuperoPassword()
		{
			return View();
		}

		/// <summary>
		/// Azione per il recupero della password. Viene inviato un token via email con il quale è possibile reimpostare la password
		/// </summary>
		/// <param name="codiceFiscale"></param>
		/// <returns></returns>
		[HttpPost]
		[ValidateAntiForgeryToken]
		[AllowAnonymous]
		public ActionResult RecuperoPassword(string codiceFiscale)
		{
			using (LogContext.PushProperty("Action", System.Reflection.MethodBase.GetCurrentMethod().Name))
			using (LogContext.PushProperty("Controller", System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name))
			using (LogContext.PushProperty("User", User.Identity.GetUserId()))
			using (LogContext.PushProperty("CodiceFiscale", codiceFiscale))
			{
				try
				{
					using (var scope = new TransactionScope())
					{
						using (var context = new Entities())
						using (var userContext = new ApplicationDbContext())
						{
							ApplicationUser user;
							var userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
							try
							{
								user = userManager.FindByName(codiceFiscale);
								if (user == null || !UserManager.IsInRole(user.Id, Role.UTENTE_CREDENZIALI))
								{
									Log.Information(LogEvent.DOMANDA_ONLINE,"Tentativo di recupero della password per un codice fiscale non presente");
									ViewData["Errore"] = "L'utenza non è presente nel sistema. Procedere con la richiesta di credenziali";
									return View();
								}

							}
							catch (Exception e)
							{
								ViewData["Errore"] = "Errore interno - Riprovare più tardi";
								return View();
							}
							/* Generazione del Token */
							TokenCredenziali token;
							try
							{
								token = new TokenCredenziali()
								{
									Id = Guid.NewGuid(),
									DataGenerazione = DateTime.Now,
									Scadenza = DateTime.Now.AddHours(24),
									UserId = user.Id,
									code = userManager.GeneratePasswordResetToken(user.Id)
								};
								context.TokenCredenziali.Add(token);
								context.SaveChanges();
							}
							catch (Exception e)
							{
								Log.Error(LogEvent.DOMANDA_ONLINE, "Errore nella creazione del token nel DB",e);
								ViewData["Errore"] = "Errore interno - Riprovare più tardi";
								return View();
							}
							try
							{
								using (var emailService = new Email.ServiceClient())
								{
									string urlToken = Url.Action("ImpostaPassword", "Account", new { userId = user.Id, tokenId = token.Id, code = token.code }, Request.Url.Scheme);
									if(!emailService.emailNew(
										"supporto@serviziocivile.it",
										user.Email,
										"Credenziali di accesso alla piattaforma DOL – Recupero Password",
										$"Ciao,<br />è stato richiesto di recuperare la password.<br />Per re-impostare la password vai a questo <a href=\"{ urlToken}\">link</a>.<br />" +
										$"La nuova password dovrà essere di almeno 12 caratteri, avere almeno una lettera minuscola, almeno una lettera maiuscola, almeno un numero e almeno un carattere non alfanumerico",
										"",
										true,
										"",
										true))
									{
										throw new Exception("Errore generico invio email");
									}
								}
								scope.Complete();
								Log.Information(LogEvent.DOMANDA_ONLINE,"Richiesta di recupero credenziali effettuata");
							}
							catch (Exception e)
							{
								Log.Error(LogEvent.DOMANDA_ONLINE, "Errore nell'invio della email",e);
								ViewData["Errore"] = "Errore interno - Riprovare più tardi";
								return View();
							}
							/* Nascondo parzialmente la email */
							ViewData["Email"] = Utils.HideEmail(user.Email);
							return View("ConfermaRecuperoPassword");
						}
					}
				}
				catch (Exception e)
				{
					Log.Error(LogEvent.DOMANDA_ONLINE,"Errore imprevisto",e);
					ViewData["Errore"] = "Errore interno - Riprovare più tardi";
					return View();
				}
			}
		}

		[AllowAnonymous]
		[HttpGet]
		public ActionResult LoginInterno()
		{
			return View();
		}

		[AllowAnonymous]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult LoginInterno(string userName, string password, bool? ricordami)
		{
			using (LogContext.PushProperty("Action", System.Reflection.MethodBase.GetCurrentMethod().Name))
			using (LogContext.PushProperty("Controller", System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name))
			using (LogContext.PushProperty("username", userName))
			using (var wcf = new wsAutenticazioneSoapClient())
			{
				var usernameCifrato = Utils.EncryptString(userName);
				var passwordCifrata = Utils.EncryptString(password);
				string result;
				string esito;
				/* Gestione dominio Opzionale */
				string dominio = ConfigurationManager.AppSettings["Dominio"];
				if (dominio!=null && !userName.Contains("\\") )
				{
					userName = dominio + "\\"+userName;
				}
				userName = userName.Replace("/", "\\");
				try
				{
					result = wcf.EseguiAutenticazione(Utils.EncryptString(userName), Utils.EncryptString(password));
				}
				catch (Exception e )
				{
					Log.Error(LogEvent.DOMANDA_ONLINE, "Errore nella chiamata al servizio di autenticazione",e);
					ViewData["Errore"] = "Errore nell'accesso al sistema";
					return View();
				}
				XmlDocument resultXML = new XmlDocument();
				try
				{
					resultXML.LoadXml(result);
					esito = resultXML.SelectNodes("EsitoAutenicazione/DettaglioEsito/ESITO")[0].InnerText;

				}
				catch (Exception e)
				{
					Log.Error(LogEvent.DOMANDA_ONLINE, "Errore nella lettura XML di risposta del servizio autenticazione",e);
					ViewData["Errore"] = "Errore nell'accesso al sistema";
					return View();
				}
				switch (esito)
				{
					case "POSITIVO":
						var user = UserManager.FindByName(userName);
						if (user==null)
						{
							Log.Information(LogEvent.DOMANDA_ONLINE,"Accesso interno effettuato con utenza non abilitata al sistema DOL");
							ViewData["Errore"] = "Utenza non abilitata all'accesso al Portale Domande Online";
							return View();
						}
						SignInManager.SignIn(user, ricordami ?? false, ricordami ?? false);
						Log.SetUsername(userName);
						Log.Information(LogEvent.DOMANDA_ONLINE,"Accesso interno effettuato");
						return RedirectToAction("Index","Home");
					case "NEGATIVO":
						Log.Information(LogEvent.DOMANDA_ONLINE,"Tentativo accesso interno con credenziali non valide");
						TempData["Errore"] = "Credenziali non valide";
						return View();
					case "ERRORE":
					default:
						Log.Error(LogEvent.DOMANDA_ONLINE,$"Il servizio autenticazione ha risposto con il seguente messaggio: {esito}");
						ViewData["Errore"] = "Errore nell'accesso al sistema";
						return View();
				}
			}
		}
/*
		[AllowAnonymous]
		public ActionResult LoginSPIDTest(int? Id)
		{
			using (LogContext.PushProperty("Action", System.Reflection.MethodBase.GetCurrentMethod().Name))
			using (LogContext.PushProperty("Controller", System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name))
			{
				AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);

				userInfo spid;

				if (Id.HasValue && Id==1)
				{
					spid = new userInfo
					{
						Spidcode = "SPIDTest001",
						CountyOfBirth = "H501",
						FiscalNumber = "CodiceFiscaleTest",
						Address = "Via della Ferratella 1 00100 Roma RM",
						DateOfBirth = "2000-01-01",
						Email = "informatica@serviziocivile.it",
						ExpirationDate = "2019-12-01",
						Gender = "M",
						IdCard = "cartaIdentita AB123456CD comuneRoma 2018-01-01 2028-01-01",
						MobilePhone = "+391234567890",
						Name = "Mario",
						Surname = "Rossi",
						PlaceOfBirth = "H501"
					};

				}
				else
				{
					spid = new userInfo
					{
						Spidcode = "SPIDTest002",
						CountyOfBirth = "H501",
						FiscalNumber = "CodiceFiscaleTest2",
						Address = "altro Compton Close 1 NW1 3QS Z114 EE",
						DateOfBirth = "2000-01-01",
						Email = "informatica@serviziocivile.it",
						ExpirationDate = "2019-12-01",
						Gender = "M",
						IdCard = "cartaIdentita AB123456CD comuneRoma 2018-01-01 2028-01-01",
						MobilePhone = "+391234567890",
						Name = "Mario",
						Surname = "Rossi",
						PlaceOfBirth = "H501"
					};

				}

				string comuneNascita;
				string nazioneNascita;
				try
				{
					using (var context = new Entities())
					{
						CodiceBelfiore codiceBelfiore = context.CodiceBelfiore
							.Where(x => x.Codice == spid.PlaceOfBirth).SingleOrDefault();
						if (codiceBelfiore == null)
						{
							comuneNascita = spid.PlaceOfBirth;
							nazioneNascita = spid.PlaceOfBirth;
						}
						else
						{
							comuneNascita = codiceBelfiore.Comune;
							nazioneNascita = codiceBelfiore.Nazione;
						}
					}
				}
				catch (Exception e)
				{
					Log.Error(LogEvent.DOMANDA_ONLINE,e, "Errore nel recupero dei codici Belfiore");
					TempData["Errore"] = "Errore Interno";
					return RedirectToAction("Index", "Home");
				}
				if (spid.Spidcode.First() != '-')
				{
					Log.Information(LogEvent.DOMANDA_ONLINE,"Accesso con SPID effettuato");
					ApplicationUser user;
					try
					{
						user = UserManager.FindByName(spid.Spidcode);
						if (user == null)
						{
							user = new ApplicationUser
							{
								UserName = spid.Spidcode,
								Nome = spid.Name,
								Cognome = spid.Surname,
								Spidcode = spid.Spidcode,
								Genere = spid.Gender,
								CodiceFiscale = spid.FiscalNumber.Split('-').Last(),
								Email = spid.Email,
								DataNascita = DateTime.Parse(spid.DateOfBirth),
								LuogoNascita = comuneNascita ?? nazioneNascita,
								NazioneNascita = nazioneNascita,
								Telefono = spid.MobilePhone,
								Indirizzo = spid.Address,
								Documento = spid.IdCard,
								ExpirationDate = spid.ExpirationDate
							};
							//Password generata casualmente (Non verrà usata per l'accesso)
							string passwordGenerated = System.Web.Security.Membership.GeneratePassword(10, 2) + "0aB!";//Pospongo i caratteri obbligatori
							var resultCreate = UserManager.Create(user, passwordGenerated);
							if (!resultCreate.Succeeded)
							{
								Log.Error(LogEvent.DOMANDA_ONLINE,"Errore nella creazione dell'utenza {@Errori}", resultCreate.Errors);
								TempData["Errore"] = "Errore Interno";
								return RedirectToAction("Index", "Home");
							}
							var resultRoleAdded = UserManager.AddToRole(user.Id, Role.UTENTE_SPID);
							if (!resultRoleAdded.Succeeded)
							{
								Log.Error(LogEvent.DOMANDA_ONLINE,"Errore nell'associazione del ruolo all'utenza {@Errori}", resultRoleAdded.Errors);
								TempData["Errore"] = "Errore Interno";
								return RedirectToAction("Index", "Home");
							}
							Log.Information(LogEvent.DOMANDA_ONLINE,"Nuova Utenza SPID", user.Id);
						}
						List<string> campiVariati = new List<string>();
						if (user.Nome != spid.Name) { user.Nome = spid.Name; campiVariati.Add("Nome"); }
						if (user.Cognome != spid.Surname) { user.Cognome = spid.Surname; campiVariati.Add("Cognome"); }
						if (user.Spidcode != spid.Spidcode) { user.Spidcode = spid.Spidcode; campiVariati.Add("Spidcode"); }
						if (user.Genere != spid.Gender) { user.Genere = spid.Gender; campiVariati.Add("Genere"); }
						if (user.CodiceFiscale != spid.FiscalNumber.Split('-').Last()) { user.CodiceFiscale = spid.FiscalNumber.Split('-').Last(); campiVariati.Add("CodiceFiscale"); }
						if (user.Email != spid.Email) { user.Email = spid.Email; campiVariati.Add("Email"); }
						if (user.DataNascita != DateTime.Parse(spid.DateOfBirth)) { user.DataNascita = DateTime.Parse(spid.DateOfBirth); campiVariati.Add("LuogoNascita"); }
						if (user.LuogoNascita != (comuneNascita ?? nazioneNascita)) { user.LuogoNascita = (comuneNascita ?? nazioneNascita); campiVariati.Add("LuogoNascita"); }
						if (user.NazioneNascita != nazioneNascita) { user.NazioneNascita = nazioneNascita; campiVariati.Add("NazioneNascita"); }
						if (user.Telefono != spid.MobilePhone) { user.Telefono = spid.MobilePhone; campiVariati.Add("Telefono"); }
						if (user.Indirizzo != spid.Address) { user.Indirizzo = spid.Address; campiVariati.Add("Indirizzo"); }
						if (user.Documento != spid.IdCard) { user.Documento = spid.IdCard; campiVariati.Add("Documento"); }
						user.ExpirationDate = spid.ExpirationDate;
						if (campiVariati.Count > 0)
						{
							Log.Information(LogEvent.DOMANDA_ONLINE,"Campi variati dell'utente che ha acceduto tramite SPID: {@CampiVariati}", campiVariati);
							UserManager.Update(user);
						}
						SignInManager.SignIn(user, true, true);
						Log.Information(LogEvent.DOMANDA_ONLINE,"Accesso con SPID effettuato: {User}", user.Id);
						return RedirectToAction("Index", "Home");
					}
					catch (Exception e)
					{
						Log.Error(LogEvent.DOMANDA_ONLINE,e, "Errore nella crezione dell'utenza");
						TempData["Errore"] = "Errore Interno";
						return RedirectToAction("Index", "Home");
					}

				}
				else
				{
					TempData["Errore"] = $"Errore nel login tramite SPID - {spid.Spidcode}";
					Log.Error(LogEvent.DOMANDA_ONLINE,"Errore nel login tramite SPID. Errore: {Errore}", spid.Spidcode);
					return RedirectToAction("Index", "Home");
				}
			}
		}
*/
		[AllowAnonymous]
		public ActionResult LoginSPID(string token)
		{
			using (LogContext.PushProperty("Action", System.Reflection.MethodBase.GetCurrentMethod().Name))
			using (LogContext.PushProperty("Controller", System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name))
			using (LogContext.PushProperty("Token", token))
			{
				AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
				if (string.IsNullOrEmpty(token))
				{
					TempData["Errore"]="Errore di accesso con SPID";
					Log.Information(LogEvent.DOMANDA_ONLINE,"Accesso SPID senza token");
					return RedirectToAction("Index","Home");
				}
				userInfo spid;
				using(var wcf = new ServiceClient())
				{
					try
					{
						spid = wcf.getUserInfo(token);
					}
					catch (Exception e)
					{
						TempData["Errore"] = "Errore di accesso con SPID";
						Log.Error(LogEvent.DOMANDA_ONLINE,"Errore chiamata servizio SPID",e);
						return RedirectToAction("Index", "Home");
					}
				}
				string comuneNascita;
				string nazioneNascita;
				try
				{
					using (var context = new Entities())
					{
						CodiceBelfiore codiceBelfiore = context.CodiceBelfiore
							.Where(x => x.Codice == spid.PlaceOfBirth).SingleOrDefault();
						if (codiceBelfiore == null)
						{
							comuneNascita = spid.PlaceOfBirth;
							nazioneNascita = spid.PlaceOfBirth;
						}
						else
						{
							comuneNascita = codiceBelfiore.Comune;
							nazioneNascita = codiceBelfiore.Nazione;
						}
					}
				}
				catch (Exception e)
				{
					Log.Error(LogEvent.DOMANDA_ONLINE,"Errore nel recupero dei codici Belfiore",e);
					TempData["Errore"] = "Errore Interno";
					return RedirectToAction("Index", "Home");
				}
				if (spid.Spidcode.First()!='-')
				{
					ApplicationUser user;
					try
					{
						user = UserManager.FindByName(spid.Spidcode);
						if (user == null)
						{
							user = new ApplicationUser
							{
								UserName = spid.Spidcode,
								Nome = spid.Name,
								Cognome = spid.Surname,
								Spidcode = spid.Spidcode,
								Genere = spid.Gender,
								CodiceFiscale = spid.FiscalNumber.Split('-').Last(),
								Email = spid.Email,
								DataNascita = DateTime.Parse(spid.DateOfBirth),
								LuogoNascita = comuneNascita??nazioneNascita,
								NazioneNascita = nazioneNascita,
								Telefono = spid.MobilePhone,
								Indirizzo = spid.Address,
								Documento = spid.IdCard,
								ExpirationDate = spid.ExpirationDate
							};
							//Password generata casualmente (Non verrà usata per l'accesso)
							string passwordGenerated = System.Web.Security.Membership.GeneratePassword(10, 2) + "0aB!";//Pospongo i caratteri obbligatori
							var resultCreate = UserManager.Create(user, passwordGenerated);
							if (!resultCreate.Succeeded)
							{
								Log.Error(LogEvent.DOMANDA_ONLINE,$"Errore nella creazione dell'utenza {string.Join(",",resultCreate.Errors)}");
								TempData["Errore"] = "Errore Interno";
								return RedirectToAction("Index","Home");
							}
							var resultRoleAdded = UserManager.AddToRole(user.Id, Role.UTENTE_SPID);
							if (!resultRoleAdded.Succeeded)
							{
								Log.Error(LogEvent.DOMANDA_ONLINE, $"Errore nell'associazione del ruolo all'utenza {string.Join(", ",resultRoleAdded.Errors)}");
								TempData["Errore"] = "Errore Interno";
								return RedirectToAction("Index", "Home");
							}
							Log.Information(LogEvent.DOMANDA_ONLINE,"Nuova Utenza SPID",user.Id);
						}
						List<string> campiVariati = new List<string>();
						if (user.Nome != spid.Name) { user.Nome = spid.Name; campiVariati.Add("Nome"); }
						if (user.Cognome != spid.Surname) { user.Cognome = spid.Surname; campiVariati.Add("Cognome"); }
						if (user.Spidcode != spid.Spidcode) { user.Spidcode = spid.Spidcode; campiVariati.Add("Spidcode"); }
						if (user.Genere != spid.Gender) { user.Genere = spid.Gender; campiVariati.Add("Genere"); }
						if (user.CodiceFiscale != spid.FiscalNumber.Split('-').Last()) { user.CodiceFiscale = spid.FiscalNumber.Split('-').Last(); campiVariati.Add("CodiceFiscale"); }
						if (user.Email != spid.Email) { user.Email = spid.Email; campiVariati.Add("Email"); }
						if (user.DataNascita != DateTime.Parse(spid.DateOfBirth)) { user.DataNascita = DateTime.Parse(spid.DateOfBirth); campiVariati.Add("LuogoNascita"); }
						if (user.LuogoNascita != (comuneNascita ?? nazioneNascita)) { user.LuogoNascita = (comuneNascita ?? nazioneNascita); campiVariati.Add("LuogoNascita"); }
						if (user.NazioneNascita != nazioneNascita) { user.NazioneNascita = nazioneNascita; campiVariati.Add("NazioneNascita"); }
						if (user.Telefono != spid.MobilePhone) { user.Telefono = spid.MobilePhone; campiVariati.Add("Telefono"); }
						if (user.Indirizzo != spid.Address) { user.Indirizzo = spid.Address; campiVariati.Add("Indirizzo"); }
						if (user.Documento != spid.IdCard) { user.Documento = spid.IdCard; campiVariati.Add("Documento"); }
						user.ExpirationDate = spid.ExpirationDate;
						if (campiVariati.Count>0)
						{
							Log.Information(LogEvent.DOMANDA_ONLINE,"Campi variati dell'utente che ha acceduto tramite SPID: {@CampiVariati}",campiVariati);
							UserManager.Update(user);
						}
						SignInManager.SignIn(user, true, true);
						Log.SetUsername(user.CodiceFiscale);
						Log.Information(LogEvent.DOMANDA_ONLINE,$"Accesso con SPID effettuato: {user.Id}");
						return RedirectToAction("Index","Home");
					}
					catch (Exception e)
					{
						Log.Error(LogEvent.DOMANDA_ONLINE, "Errore nella crezione dell'utenza",e);
						TempData["Errore"] = "Errore Interno";
						return RedirectToAction("Index", "Home");
					}

				}
				else
				{
					TempData["Errore"]=$"Errore nel login tramite SPID - {spid.Spidcode}";
					Log.Error(LogEvent.DOMANDA_ONLINE, $"Errore nel login tramite SPID. Errore: {spid.Spidcode}");
					return RedirectToAction("Index","Home");
				}
			}
		}
	}
}