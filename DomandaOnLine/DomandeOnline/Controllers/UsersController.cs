using DomandeOnline.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DomandeOnline.Controllers
{


	[Authorize(Roles = Role.AMMINISTRATORE)]
	public class UsersController : SmartController
	{
		private IQueryable<IdentityUser> GetUtentiInterni(ApplicationDbContext context)
		{
			try
			{
				var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
				string idRuoloInterno = roleManager.Roles.Where(r => r.Name == Role.INTERNO).Select(x => x.Id).SingleOrDefault();
				var users = context.Users
					.Include(x => x.Roles)
					.Where(x => x.Roles
						.Where(r => r.RoleId == idRuoloInterno)
						.Select(role => role.UserId)
						.Contains(x.Id));
				return users;
			}
			catch (Exception e)
			{
				Log.Error(LogEvent.DOMANDA_ONLINE,"Errore nella lettura degli utenti interni dal DB",e);
				throw new Exception ("Errore nella lettura degli utenti interni dal DB",e);
			}
		}

		/// <summary>
		/// Restituisce la pagina con l'elenco degli utenti interni
		/// </summary>
		/// <returns></returns>
		public ActionResult Index()
		{
			using (ApplicationDbContext context = new ApplicationDbContext()) {
				try
				{
					var users = GetUtentiInterni(context)
								.Select(user => new Utente
								{
									Id = user.Id,
									Nome = user.UserName,
									Ruoli = from userRole in user.Roles
											join role in context.Roles on
									userRole.RoleId equals role.Id
											where role.Name != Role.INTERNO
											select role.Name
								}
								);
					return View(users.ToList());
				}
				catch (Exception e)
				{
					Log.Error(LogEvent.DOMANDA_ONLINE, "Errore nella lettura degli utenti interni dal DB",e);
					throw new Exception("Errore nella lettura degli utenti interni dal DB", e);
				}
			}
		}

		[HttpPost]
		public ActionResult AddRoleToUser(string idUtente, string ruolo)
		{
			using (ApplicationDbContext context = new ApplicationDbContext())
			{
				try
				{
					var userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
					var result = userManager.AddToRole(idUtente, ruolo);
					if (result.Succeeded)
					{
						return Json(new { success = true });
					}
					else
					{
						Log.Error(LogEvent.DOMANDA_ONLINE,$"Errore nell'aggiunta del ruolo {ruolo} all'utente con id {idUtente}.",parameters: result.Errors);
						return Json(new { success = false, message = string.Join("\n", result.Errors) });
					}

				}
				catch (Exception e)
				{
					Log.Error(LogEvent.DOMANDA_ONLINE, $"Errore nell'aggiunta del ruolo {ruolo} all'utente con id {idUtente}");
					throw new Exception($"Errore nell'aggiunta del ruolo all'utente", e);
				}
			}
		}

		[HttpPost]
		public ActionResult RemoveRoleToUser(string idUtente, string ruolo)
		{
			using (ApplicationDbContext context = new ApplicationDbContext())
			{
				try
				{
					var userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
					var result = userManager.RemoveFromRole(idUtente, ruolo);
					if (result.Succeeded)
					{
						return Json(new { success = true });
					}
					else
					{
						Log.Error(LogEvent.DOMANDA_ONLINE, $"Errore nella rimozione del ruolo {ruolo} dall'utente con id {idUtente}.", parameters: result.Errors);
						return Json(new { success = false, message = string.Join("\n", result.Errors) });
					}
				}
				catch (Exception e)
				{
					Log.Error(LogEvent.DOMANDA_ONLINE,$"Errore nella rimozione del ruolo {ruolo} dall'utente con id {idUtente}", e);
					throw new Exception($"Errore nella rimozione del ruolo dall'utente", e);
				}
			}
		}


		public ActionResult Dettaglio(string id)
		{
			using (ApplicationDbContext context = new ApplicationDbContext())
			{
				try
				{
					var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
					var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
					ViewData["Ruoli"] = roleManager.Roles.Where(r => Role.RuoliInterni.Contains(r.Name)).ToList();
					ApplicationUser user = userManager.FindById(id);
					if (user == null)
					{
						Log.Error(LogEvent.DOMANDA_ONLINE, $"Tentativo di visualizzarte i dettagli dell'utente inesistente con id {id}");
						return HttpNotFound("Utente Inesistente");
					}
					return View(user);
				}
				catch (Exception e)
				{
					Log.Error(LogEvent.DOMANDA_ONLINE,$"Errore nella lettura dei dati relativi all'utente con id {id}");
					throw;
				}
			}
		}

		[HttpPost]
		public ActionResult AddUser(string userName)
		{
			using (ApplicationDbContext context = new ApplicationDbContext())
			{
				try
				{
					/* Gestione dominio Opzionale */
					string dominio = ConfigurationManager.AppSettings["Dominio"];
					if (dominio != null && !userName.Contains("\\"))
					{
						userName = dominio + "\\" + userName;
					}
					ApplicationUser user = new ApplicationUser()
					{
						UserName = userName
					};
					string passwordGenerated = System.Web.Security.Membership.GeneratePassword(10, 2) + "0aB!";//Pospongo i caratteri obbligatori
					var userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
					var result = userManager.Create(user, passwordGenerated);
					if (result.Succeeded)
					{
						userManager.AddToRole(user.Id, Role.INTERNO);
						return Json(new { success = true });
					}
					else
					{
						Log.Error(LogEvent.DOMANDA_ONLINE, $"Errore nell'inserimento dell'utente con username {userName}", parameters: result.Errors);
						return Json(new { success = false, message = string.Join("\n", result.Errors) });
					}
				}
				catch (Exception e)
				{
					Log.Error(LogEvent.DOMANDA_ONLINE, $"Errore nell'inserimento dell'utente con username {userName}.");
					throw;
				}
			}

		}

		public ActionResult Modifica()
		{
			return View();
		}

		public ActionResult Sospendi(int id)
		{
			return View();
		}

		public ActionResult Riattiva(int id)
		{
			return View();
		}

		[HttpPost]
		public ActionResult Elimina(string idUtente)
		{
			using (ApplicationDbContext context = new ApplicationDbContext())
			{
				try
				{
					var userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
					var utente = userManager.FindById(idUtente);
					var result = userManager.Delete(utente);
					if (result.Succeeded)
					{
						return Json(new { success = true });
					}
					else
					{
						Log.Error(LogEvent.DOMANDA_ONLINE, $"Errore nella rimozione dell'utente {utente.UserName}.", parameters: result.Errors);
						return Json(new { success = false, message = string.Join("\n", result.Errors) });
					}
				}
				catch (Exception e)
				{
					Log.Error(LogEvent.DOMANDA_ONLINE, $"Errore nella rimozione dell'utente con id {idUtente}", e);
					throw new Exception($"Errore nella rimozione del ruolo dall'utente", e);
				}
			}
		}


	}
}