using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RegistrazioneSistemaUnico.Data;
using RegistrazioneSistemaUnico.Helpers;
using RegistrazioneSistemaUnico.Models;
using RegistrazioneSistemaUnico.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using SpidService;
using static RegistrazioneSistemaUnico.Services.TokenService;

namespace RegistrazioneSistemaUnico.Controllers
{
	public class AccessoController : SmartController
	{


		public AccessoController(RegistrazioneContext registrazioneContext) : base(registrazioneContext)
		{
		}


		public IActionResult Spid(Accesso parameters)
		{
			if (Parametri.SimulazioneSPID!="true")
			{
				return NotFound();
			}
			return View(parameters);
		}


		[HttpGet]
		public IActionResult LoginSpid(string token)
		{
			ServiceClient service = new ServiceClient(ServiceClient.EndpointConfiguration.BasicHttpBinding_IService, Parametri.SpidServiceEndpoint);
			userInfo result;
			try
			{
				result = service.getUserInfo(token);
			}
			catch (Exception ex)
			{
				Log.Error(LogEvent.ERRORE_ACCESSO_SPID, "Errore Access al Servizio SPID",ex);
				TempData["Error"] = "Errore nell'accesso con SPID. Consultare il supporto tecnico";
				return RedirectToAction("Index", "Home");
			}
			if (result.Spidcode==null || result.Spidcode.First() == '-')
			{
				Log.Error(LogEvent.ERRORE_ACCESSO_SPID, result.Spidcode);
				TempData["Error"] = $"Errore nell'accesso con SPID {result.Spidcode}";
				return RedirectToAction("Index", "Home");
			}
			int i = result.FiscalNumber.IndexOf("-");
			if (i >= 0)
			{
				result.FiscalNumber=(result.FiscalNumber.Substring(i + 1, result.FiscalNumber.Length - i - 1));
			}
			string jwtToken = GenerateSimpleToken(result);
			Response.Cookies.Append("jwtToken", jwtToken);
			Log.SetUsername(result.FiscalNumber, $"{result.Name} {result.Surname}".Trim());


			Log.Information(LogEvent.ACCESSO_EFFETTUATO);
			// Redirect in base ai dati della sessione
			Registrazione registrazione = GetRegistrazione();
			if (registrazione == null)
			{
				return RedirectToAction("Index");
			}
			//Variato Utente
			if (registrazione.CodiceFiscaleRappresentanteLegale != result.FiscalNumber)
			{
				HttpContext.Session.Remove("Registrazione");
				return RedirectToAction("Index");
			}
			if (!string.IsNullOrEmpty(registrazione.CodiceFiscaleEnte))
			{
				Log.SetEnte(registrazione.CodiceFiscaleEnte);
			}
			if (registrazione.Nome == null)
			{
				return RedirectToAction("Index", "Registrazione");
			}
			return RedirectToAction("Riepilogo", "Registrazione");
		}

		[HttpPost]
		public IActionResult LoginSpid(Accesso parameters)
		{
			if (Parametri.SimulazioneSPID != "true")
			{
				return NotFound();
			}
			if (ModelState.IsValid)
			{
				ServiceClient service = new ServiceClient(ServiceClient.EndpointConfiguration.BasicHttpBinding_IService, Parametri.SpidServiceEndpoint);

				userInfo result2 = service.getUserInfo(parameters.CodiceFiscale);
				userInfo result = new userInfo()
				{
					Name = parameters.Nome,
					Surname = parameters.Cognome,
					DateOfBirth = parameters.DataNascita.Value.ToString("yyyy-MM-dd"),
					FiscalNumber = parameters.CodiceFiscale.ToUpper()
				};

				string jwtToken = TokenService.GenerateSimpleToken(result);
				Response.Cookies.Append("jwtToken", jwtToken);
				Log.SetUsername(result.FiscalNumber, $"{result.Name} {result.Surname}".Trim());


				Log.Information(LogEvent.ACCESSO_EFFETTUATO);
				// Redirect in base ai dati della sessione
				Registrazione registrazione = GetRegistrazione();
				if (registrazione == null )
				{
					return RedirectToAction("Index");
				}
				//Variato Utente
				if (registrazione.CodiceFiscaleRappresentanteLegale != result.FiscalNumber)
				{
					HttpContext.Session.Remove("Registrazione");
					return RedirectToAction("Index");
				}
				if (!string.IsNullOrEmpty(registrazione.CodiceFiscaleEnte))
				{
					Log.SetEnte(registrazione.CodiceFiscaleEnte);
				}
				if (registrazione.Nome == null)
				{
					return RedirectToAction("Index", "Registrazione");
				}
				return RedirectToAction("Riepilogo", "Registrazione");
			}

			return View("Spid", parameters);

		}


		[Authorize]
		public IActionResult Index()
		{
			List<UtenteEnte> enti = context.UtenteEnte
				.Where(x => x.CodiceFiscale == utente.FiscalNumber)
				.ToList();
			return View(enti);
		}



		[Authorize]
		[HttpPost]
		public IActionResult Accedi(string Username, string Albo)
		{
			UtenteEnte ente = context.UtenteEnte
				.Where(
					x => x.CodiceFiscale == utente.FiscalNumber && 
					x.Username == Username && 
					x.Albo==Albo)
				.FirstOrDefault();
			if (ente == null)
			{
				ViewData["Error"] = "Utente non abilitato all'accesso";
				return RedirectToAction("Index");
			}
			TokenAccesso tokenAccesso = new TokenAccesso()
			{
				CodiceFiscaleEnte = ente.CodiceFiscaleEnte,
				CodiceFiscale = utente.FiscalNumber,
				Albo = ente.Albo,
				Username = ente.Username,
				Utilizzato = false,
				DataScadenza = DateTime.UtcNow.AddSeconds(Parametri.TokenAccessoDurataSecondi)
			};
			context.Add(tokenAccesso);
			context.SaveChanges();
			string token = tokenAccesso.Id.ToString();
			return Redirect($"{Parametri.UrlAccessoHelios}/?token={token}");
		}


		/// <summary>
		/// Metodo per la verifica del token di accesso al sistema unico
		/// </summary>
		/// <param name="token"></param>
		/// <returns></returns>
		[HttpGet]
		public IActionResult CheckToken(string token)
		{
			/*TokenAccessoInfo tokenInfo = VerifyAccessoToken(token);
			if (!tokenInfo.IsValid)
			{
				Log.Error(LogEvent.ERRORE_ACCESSO_HELIOS, "Token non valido",parameters:token);
				return Json(new { success = false,message = "Token non valido" });

			}
			if (tokenInfo.IsExpired)
			{
				Log.Error(LogEvent.ERRORE_ACCESSO_HELIOS, "Token scaduto", parameters: token);
				return Json(new { success = false, message = "Token non valido" });

			}*/
			Guid tokenId;
			if(!Guid.TryParse(token,out tokenId)){
				Log.Error(LogEvent.ERRORE_ACCESSO_HELIOS, "Token non valido", parameters: token);
				return Json(new { success = false, message = "Token non valido" });
			};

			using (TransactionScope scope = new TransactionScope())
			{
				TokenAccesso tokenAccesso = context.TokenAccesso.FirstOrDefault(t => t.Id == tokenId);
				if (tokenAccesso == null)
				{
					Log.Error(LogEvent.ERRORE_ACCESSO_HELIOS, "Token non trovato", parameters: token);
					return Json(new { success = false, message = "Token non valido" });
				}
				//if (tokenAccesso.CodiceFiscale != tokenInfo.Token.CodiceFiscale)
				//{
				//	Log.Error(LogEvent.ERRORE_ACCESSO_HELIOS, "CF Utente alterato", parameters: token);
				//	return Json(new { success = false, message = "Token non valido" });
				//}
				//if (tokenAccesso.CodiceFiscaleEnte != tokenInfo.Token.CodiceFiscaleEnte)
				//{
				//	Log.Error(LogEvent.ERRORE_ACCESSO_HELIOS, "CF Ente alterato", parameters: token);
				//	return Json(new { success = false, message = "Token non valido" });
				//}
				//if (tokenAccesso.Albo != tokenInfo.Token.Albo)
				//{
				//	Log.Error(LogEvent.ERRORE_ACCESSO_HELIOS, "Albo alterato", parameters: token);
				//	return Json(new { success = false, message = "Token non valido" });
				//}
				if (tokenAccesso.Utilizzato)
				{
					Log.Error(LogEvent.ERRORE_ACCESSO_HELIOS, "Token già utilizzato", parameters: token);
					return Json(new { success = false, message = "Token non valido" });

				}
				tokenAccesso.Utilizzato = true;
				context.SaveChanges();
				scope.Complete();
				return Json(new {	success = true,
									CodiceFiscaleEnte = tokenAccesso.CodiceFiscaleEnte,
									CodiceFiscale = tokenAccesso.CodiceFiscale,
									Username = tokenAccesso.Username,
									albo = tokenAccesso.Albo
				});
			}

		}


		public IActionResult Logoff()
		{
			Log.Information(LogEvent.DISCONNESSIONE);
			Response.Cookies.Delete("jwtToken");
			HttpContext.Session.Remove("Registrazione");
			return RedirectToAction("Index","Home");
		}



	}

}
