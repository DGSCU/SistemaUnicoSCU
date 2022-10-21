using Microsoft.AspNetCore.Mvc;
using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using SpidService;
using Logger.Data;
using System.Linq;
using RegistrazioneSistemaUnico.Data;
using Newtonsoft.Json;
using System.Net;
using RegistrazioneSistemaUnico.Models;
using RegistrazioneSistemaUnico.Models.Forms;

namespace RegistrazioneSistemaUnico.Helpers
{
	public class SmartController : Controller
	{

		/// <summary>
		/// Infrmazioni sull'utenza con i dati di SPID
		/// </summary>
		public userInfo utente=null;
		/*public readonly UserManager<Utente> userManager;
		public readonly SignInManager<Utente> signInManager;
		public SmartController
		(
			UserManager<Utente> userManager,
			SignInManager<Utente> signInManager)
		{
			this.userManager = userManager;
			this.signInManager = signInManager;
		}

		public Utente CurrentUser
		{
			get
			{
				return userManager.GetUserAsync(HttpContext.User).Result;
			}
		}*/

		/*public ResultModel<Entity> ModelStateResultListError<Entity>(Entity model,string message=null){
			ResultModel<Entity> result = new ResultModel<Entity>(false, message??"Dati non corretti", 
				errors: ModelState
					.Where(m=>m.Value.Errors.Count>0)
					.ToDictionary(
						m => m.Key,
						m => m.Value.Errors.Select(e => e.ErrorMessage)
					.ToArray()
				),
				entity: model);
			return result;
		}*/

		protected readonly RegistrazioneContext context;
		protected readonly LogContext logContext;

		public SmartController(RegistrazioneContext registrazioneContext, LogContext logContext=null)
		{
			this.context = registrazioneContext;
			this.logContext = logContext;
			//this.heliosContext = heliosContext;
		}

		public Logger.Logger Log { get; private set; }

		/// <summary>
		/// Impostazione di configurazione del log. Viene effettuata all'apertura della pagina (vedi metodo OnActionExecuting su CustomFilters)
		/// </summary>
		/// <param name="methodName">nome del metodo (viene recupoerato dal ciamante)</param>
		public void SetLog(string methodName, string controllerName, string actionName, string sessionId)
		{
			string ipAddress =HttpContext.Connection.RemoteIpAddress.ToString();
			var claim = User.Claims.FirstOrDefault(c => c.Type == "User")?.Value;
			userInfo userinfo=null;
			if (claim!=null)
			{
				userinfo = JsonConvert.DeserializeObject<userInfo>(claim);

			}
			string hostname = Dns.GetHostName();
			string hostIp = Dns.GetHostAddresses(hostname)
				.FirstOrDefault(a=>a.AddressFamily==System.Net.Sockets.AddressFamily.InterNetwork)
				.ToString();
			
			Log = new Logger.Logger(
				controllerName,
				actionName,
				methodName,
				username: userinfo?.FiscalNumber,
				name: $"{userinfo?.Name} {userinfo?.Surname}".Trim(),
				ente: GetRegistrazione()?.CodiceFiscaleEnte,
				ipAddress:ipAddress,
				applicationIpAddress: hostIp,
				applicationHost: hostname,
				sessionId:sessionId
			);
		}

		public Registrazione GetRegistrazione()
		{
			if (HttpContext.Session.GetString("Registrazione") != null)
			{
				Registrazione registrazione = JsonConvert.DeserializeObject<Registrazione>(HttpContext.Session.GetString("Registrazione"));

				return registrazione;
			}
			return null;
		}

		public VariazioneRappresentanteLegale GetVariazione()
		{
			if (HttpContext.Session.GetString("VariazioneRappresentanteLegale") != null)
			{
				VariazioneRappresentanteLegale variazione = JsonConvert.DeserializeObject<VariazioneRappresentanteLegale>(HttpContext.Session.GetString("VariazioneRappresentanteLegale"));

				return variazione;
			}
			return null;
		}


	}
}
