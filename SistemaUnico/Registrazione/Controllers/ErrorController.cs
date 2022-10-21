using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Logger;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Http.Features;
using System.Dynamic;
using Newtonsoft.Json;
using RegistrazioneSistemaUnico.Helpers;
using RegistrazioneSistemaUnico.Data;

namespace RegistrazioneSistemaUnico.Controllers
{
	public class ErrorController : Controller
	{

		public IActionResult Index()
		{
			var a = HttpContext.Session.Keys;
			if (HttpContext.Session.Keys.Any(x => x == "ErrorLogged"))
			{
				HttpContext.Session.Remove("ErrorLogged");
			}
			else
			{
				var exceptionHandler = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
				var httpRequestFeature = HttpContext.Features.Get<IHttpRequestFeature>();
				var exception = exceptionHandler?.Error;

				dynamic properties = new ExpandoObject();


				try
				{
					foreach (var formObject in HttpContext.Request.Form)
					{
						try
						{
							object value = null;
							value = JsonConvert.DeserializeObject(formObject.Key);
							((IDictionary<string, object>)properties).Add("Body", value);
						}
						catch
						{
							if (formObject.Value.Count == 1)
							{
								((IDictionary<string, object>)properties).Add(formObject.Key, formObject.Value.First());
							}
							else
							{
								((IDictionary<string, object>)properties).Add(formObject.Key, formObject.Value);
							}
						}

					}
				}
				catch (Exception)
				{
					properties = null;
				}

				var response = HttpContext.Response;
				var path = httpRequestFeature.RawTarget;
				var connection = HttpContext.Connection;
				var idClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "User")?.Value;
				int? userId = int.TryParse(idClaim, out int N) ? (int?)N : null;

				string controller = path.Substring(1).Split('/').FirstOrDefault();
				string action = path.Substring(1).Split('/').Skip(1).FirstOrDefault();

				string ipAddress = connection.RemoteIpAddress.ToString();
				string sessionId = HttpContext?.Session?.Id;

				Logger.Logger log = new Logger.Logger(
					controller,
					action ?? "Index",
					httpRequestFeature.Method,
					ipAddress: ipAddress,
					sessionId: sessionId
					);
				log.Critical(LogEvent.ERRORE,$"Errore nel rendering Razor: {exception.Message}", exception);
			}

			return View();
		}

		public IActionResult Unauthorized(){

			return View();
		}

		public IActionResult NotFound()
		{

			return View();
		}

		public IActionResult AuthenticationExpired()
		{
			TempData["Error"] = "I dati di autorizzazione sono scaduti per inattività. Rieffettuare l'accesso";
			return RedirectToAction("Index","Home");
		}
	}
}