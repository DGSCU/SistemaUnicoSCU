using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace ScheduleAgent
{
	public class MvcApplication : System.Web.HttpApplication
	{
		protected void Application_Start()
		{
			AreaRegistration.RegisterAllAreas();
			FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
			RouteConfig.RegisterRoutes(RouteTable.Routes);
			BundleConfig.RegisterBundles(BundleTable.Bundles);

			Log.Logger = new LoggerConfiguration()
				.MinimumLevel.Information()
				.ReadFrom.AppSettings()
				.Enrich.FromLogContext()
				.CreateLogger();
			Log.Information("Avviata Applicazione");
		}
	}
}
