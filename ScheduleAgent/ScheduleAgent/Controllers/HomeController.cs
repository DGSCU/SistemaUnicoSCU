using ScheduleAgent.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ScheduleAgent.Controllers
{
	public class HomeController : Controller
	{
		public ActionResult Index(string messaggio,string errore)
		{
			using (HeliosContext context = new HeliosContext())
			{
				var errori = context.LogScheduleAgent
					.Where(x => x.Level == "Error" || x.Level == "Fatal")
					.OrderByDescending(x => x.Id)
					.Select(x => new LogDto
					{
						Id = x.Id,
						Messaggio = x.Message,
						Errore = x.Errore,
						Timestamp = x.Timestamp
					})
					.Take(20)
					.ToList();
				
				return View(errori);
			}
		}

		public ActionResult Errore(int? Id)
		{
			using (HeliosContext context = new HeliosContext())
			{
				var errore = context.LogScheduleAgent
					.Where(x => x.Id == Id)
					.SingleOrDefault();

				return View(errore);
			}
		}

		public ActionResult Contact()
		{
			ViewBag.Message = "Your contact page.";

			return View();
		}

		public ActionResult DownloadDomanda(int? Id)
		{
			using (DomandeOnlineContext context = new DomandeOnlineContext())
			{
				var file = context.DomandaPartecipazione
					.Where(x => x.Id == Id)
					.Select(x=>x.FileDomanda)
					.SingleOrDefault();
				return File(file,"application/pdf");
			}
		}
		public ActionResult DownloadCV(int? Id)
		{
			using (DomandeOnlineContext context = new DomandeOnlineContext())
			{
				var file = context.DomandaPartecipazione
					.Where(x => x.Id == Id)
					.Select(x => x.AllegatoCV)
					.SingleOrDefault();
				return File(file, "application/pdf");
			}
		}
	}
}