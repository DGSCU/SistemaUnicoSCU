using DomandeOnline.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;
using Logger;
using System.Configuration;
using Logger.Output;

[assembly: OwinStartupAttribute(typeof(DomandeOnline.Startup))]
namespace DomandeOnline
{
	public partial class Startup
	{
		public void Configuration(IAppBuilder app)
		{
			Logger.Logger.SetApplicationName("DomandaOnline");
			LogDB logDB = new LogDB("");
			Logger.Logger.AddOutput(logDB);

			Logger.Logger Log = new Logger.Logger(null,null,null);
			Log.Information(LogEvent.DOMANDA_ONLINE,"Avviata Applicazione");
			ConfigureAuth(app);

			/** Creazione Ruoli e utenza di amministrazione **/
			using (var context = new ApplicationDbContext())
			{
				var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
				userManager.UserValidator = new UserValidator<ApplicationUser>(userManager)
				{
					AllowOnlyAlphanumericUserNames = false,
					RequireUniqueEmail = false
				};
				using (var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context)))
				{
					string[] ruoli = Role.Ruoli;
					foreach (string ruolo in ruoli)
					{
						if (!roleManager.RoleExists(ruolo))
						{
							roleManager.Create(new IdentityRole(ruolo));
						}
					}

					try
					{
						if (ConfigurationManager.AppSettings["Admins"] != null)
						{
							string[] adminsList = ConfigurationManager.AppSettings["Admins"].Split(',');

							foreach (string admin in adminsList)
							{
								if (userManager.FindByName(admin) == null)
								{
									ApplicationUser user = new ApplicationUser
									{
										UserName = admin
									};

									var result = userManager.Create(user, System.Web.Security.Membership.GeneratePassword(10, 2) + "0aB!");
									if (result.Succeeded)
									{
										userManager.AddToRole(user.Id, Role.INTERNO);
										userManager.AddToRole(user.Id, Role.AMMINISTRATORE);
									}
								}
							}

						}
					}
					catch (System.Exception e)
					{
						Log.Error(LogEvent.DOMANDA_ONLINE, "Errore nella creazione dgegli amministratori",e);
						throw;
					}
				}
			}
		}
	}
}
