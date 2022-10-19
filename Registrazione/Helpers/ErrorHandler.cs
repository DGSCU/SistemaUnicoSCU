using Logger.Attributes;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.VisualBasic.CompilerServices;
using Newtonsoft.Json;
using RegistrazioneSistemaUnico.Data;
using RegistrazioneSistemaUnico.Services;
using SpidService;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using static RegistrazioneSistemaUnico.Services.TokenService;

namespace RegistrazioneSistemaUnico.Helpers
{
	/// <summary>
	/// Classe di gestione degli errori Http
	/// </summary>
	public class ErrorHandler
	{
		/// <summary>
		/// Gestisce il redirect a seguito di errori http
		/// </summary>
		/// <param name="context"></param>
		public static void DefalultHandler(StatusCodeContext context)
		{
			var response = context.HttpContext.Response;
			var request = context.HttpContext.Request;
			var connection = context.HttpContext.Connection;
			var httpRequestFeature = context.HttpContext.Features.Get<IHttpRequestFeature>();
			var idClaim = context.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "User")?.Value;
			string subsite = "";
			if (!string.IsNullOrEmpty(Configuration.SubSite))
			{
				subsite = ($"/{Configuration.SubSite}");
			}
			

			//Errori di autenticazione
			if (response.StatusCode == (int)HttpStatusCode.Unauthorized ||
				response.StatusCode == (int)HttpStatusCode.Forbidden)
			{

				int? userId = int.TryParse(idClaim, out int N) ? (int?)N : null;
				string controller = request.Path.Value.Substring(1).Split('/').FirstOrDefault();
				string action = request.Path.Value.Substring(1).Split('/').Skip(1).FirstOrDefault();

				string ipAddress = connection.RemoteIpAddress.ToString();
				string sessionId = context?.HttpContext?.Session?.Id;

				Logger.Logger log = new Logger.Logger(
					controller,
					action??"Index",
					request.Method,
					ipAddress:ipAddress,
					sessionId: sessionId
					);

				//Verifica se è scaduto il token
				string token = request.Cookies["jwtToken"];
				TokenInfo tokenInfo = VerifyToken(token);
				if (tokenInfo.IsValid&&tokenInfo.IsExpired)
				{
					log.SetUsername(tokenInfo.Username, $"{tokenInfo?.UserInfo?.Surname} {tokenInfo?.UserInfo?.Name}".Trim());
					log.Warning(LogEvent.AUTORIZZAZIONE_SCADUTA);
					response.Cookies.Delete("jwtToken");
					response.Redirect($"{subsite}/Error/AuthenticationExpired");
				}else{ 
					log.Warning(LogEvent.ACCESSO_NON_AUTORIZZATO);
					response.Redirect($"{subsite}/Error/Unauthorized");
				 }
			}else if (		response.StatusCode == (int)HttpStatusCode.NotFound
						||	response.StatusCode == (int)HttpStatusCode.MethodNotAllowed)
			{
				int? userId = int.TryParse(idClaim, out int N) ? (int?)N : null;
				string controller = request.Path.Value.Substring(1).Split('/').FirstOrDefault();
				string action = request.Path.Value.Substring(1).Split('/').Skip(1).FirstOrDefault();

				string ipAddress = connection.RemoteIpAddress.ToString();
				string sessionId = context?.HttpContext?.Session?.Id;

				Logger.Logger log = new Logger.Logger(
					controller,
					action ?? "Index",
					request.Method,
					ipAddress: ipAddress,
					sessionId: sessionId

					);

				log.Warning(LogEvent.PAGINA_INESISTENTE); ;
				response.Redirect($"{subsite}/Error/NotFound");
			}


		}
	}

}
