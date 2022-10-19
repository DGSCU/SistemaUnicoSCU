using Logger.Attributes;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.VisualBasic.CompilerServices;
using Newtonsoft.Json;
using RegistrazioneSistemaUnico.Data;
using RegistrazioneSistemaUnico.Services;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using RegistrazioneSistemaUnico.Models;
using SpidService;

namespace RegistrazioneSistemaUnico.Helpers
{
	public class CustomActionFilter : IActionFilter
	{
		public void OnActionExecuting(ActionExecutingContext context)
		{

			string controllerName = (string)context.RouteData.Values["Controller"];
			string actionName = (string)context.RouteData.Values["Action"];
			DateTime startTime = DateTime.Now;
			string type = context.HttpContext.Request.Method;
			var parameters = context.ActionArguments;

			Dictionary<string, object> logParameters = new Dictionary<string, object>();

			/* Esclusione parametri dal log */
			foreach (var item in parameters)
			{
				Dictionary<string, object> element = new Dictionary<string, object>();
				if (item.Value is string text)
				{
					logParameters.Add(item.Key, text);
					continue;
				}
				if (item.Value ==null)
				{
					logParameters.Add(item.Key, "");
					continue;
				}
				foreach (var property in item.Value.GetType().GetProperties())
				{
					if (property.GetCustomAttribute<NoLogAttribute>() == null)
					{
						element.Add(property.Name, property.GetValue(item.Value));
					}
				}
				logParameters.Add(item.Key, element);
			}
			if (context.Controller is SmartController)
			{
				SmartController controller = context.Controller as SmartController;
				string sessionId= context?.HttpContext?.Session?.Id;
				//Refresh Token JWT
				string token=context.HttpContext?.Request?.Cookies["jwtToken"];
				if (token!=null)
				{
					userInfo userInfo = TokenService.GetUser(token);
					string newToken = TokenService.GenerateSimpleToken(userInfo);
					context.HttpContext.Response.Cookies.Append("jwtToken", newToken);
				}
				//Utente da Claims (JWT)
				ClaimsPrincipal user = context?.HttpContext?.User;
				if (user != null)
				{
					var userInfo = user.Claims
						.FirstOrDefault(c => c.Type == "User")?.Value;
					if (!string.IsNullOrEmpty(userInfo))
					{
						controller.utente= JsonConvert.DeserializeObject<userInfo>(userInfo);
					}
				}

				//Utente da sessione
				//if (controller.HttpContext.Session.GetString("Utente") != null)
				//{
				//	controller.utente = JsonConvert.DeserializeObject<userInfo>(controller.HttpContext.Session.GetString("Utente"));
				//}
				//controller.Log.Information("Accesso pagina", parameters: logParameters);
				controller.SetLog(type,controllerName,actionName, sessionId);

				controller.TempData["controllerName"] = controllerName;
				controller.TempData["actionName"] = actionName;

			}
		}

		public void OnActionExecuted(ActionExecutedContext context)
		{
			if (context.Controller is SmartController controller)
			{

				if (context.Exception!=null)
				{
					context.HttpContext.Session.SetString("ErrorLogged","Yes");
					controller.Log.Critical(LogEvent.ERRORE, context.Exception.Message,context.Exception);
				}
				else
				{
					context.HttpContext.Session.SetString("NoError","Yes");
					controller.Log.Trace("Azione Completata");
				}

			}
		}
	}
}
