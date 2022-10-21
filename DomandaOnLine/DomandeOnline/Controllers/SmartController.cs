using DomandeOnline.Models;
using Logger;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace DomandeOnline.Controllers
{
	public class SmartController : Controller
	{


		public SmartController()
		{
		}
		//protected override void EndExecute(IAsyncResult asyncResult)
		//{
		//	Log.Trace("Pagina Terminata");
		//	base.EndExecute(asyncResult);
		//}

		protected override IAsyncResult BeginExecute(RequestContext requestContext, AsyncCallback callback, object state)
		{
			string controllerName = (string)requestContext.RouteData.Values["Controller"];
			string actionName = (string)requestContext.RouteData.Values["Action"];
			string type = requestContext.HttpContext.Request.HttpMethod;
			string sessionId = requestContext?.HttpContext?.Session?.SessionID;
			var username = requestContext.HttpContext.GetOwinContext()
			.Authentication?.User?.Identity?.Name;
			SetLog(type, controllerName, actionName, sessionId, username);
			//	.FindById(User.Identity.GetUserId());

			return base.BeginExecute(requestContext, callback, state);
		}
		protected override void OnException(ExceptionContext filterContext)
		{
			Log.Error(LogEvent.DOMANDA_ONLINE,"Errore critico",filterContext.Exception);
		}
		public Logger.Logger Log { get; private set; }

		/// <summary>
		/// Impostazione di configurazione del log. Viene effettuata all'apertura della pagina (vedi metodo OnActionExecuting su CustomFilters)
		/// </summary>
		/// <param name="methodName">nome del metodo (viene recupoerato dal ciamante)</param>
		public void SetLog(string methodName, string controllerName, string actionName, string sessionId,string username)
		{
			string ipAddress = GetIp();
			string hostname = Dns.GetHostName();
			string hostIp = Dns.GetHostAddresses(hostname)
				.FirstOrDefault(a => a.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)?
				.ToString(); ;
			ApplicationUser utente=new ApplicationUser();
			//utente = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>()
			//	.FindById(User.Identity.GetUserId());
			string userId = User?.Identity?.GetUserId();
			if (userId!=null)
			{

			}
			utente.CodiceFiscale = userId;
			Log = new Logger.Logger(
				controllerName,
				actionName,
				methodName,
				username: username,
				ipAddress: ipAddress,
				applicationIpAddress: hostIp,
				applicationHost: hostname,
				sessionId: sessionId
			);
		}

		public string GetIp()
		{
			string ip = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
			if (string.IsNullOrEmpty(ip))
			{
				ip = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
			}
			return ip;
		}
	}
}