using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PdfSharp;
using PdfSharp.Pdf;
using RegistrazioneSistemaUnico.Data;
using RegistrazioneSistemaUnico.Helpers;
using RegistrazioneSistemaUnico.Helpers.Grid;
using RegistrazioneSistemaUnico.Models;
using RegistrazioneSistemaUnico.Models.Forms;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using TheArtOfDev.HtmlRenderer.PdfSharp;

namespace RegistrazioneSistemaUnico.Controllers
{
	[Authorize(Roles ="Negata")]
	public class StatisticheController : SmartController
	{

		public StatisticheController(RegistrazioneContext registrazioneContext,LogContext logContext) : base(registrazioneContext, logContext)
		{
		}
		[HttpGet]
		public IActionResult Index(){
			
			if (HttpContext.Session.GetString("StatisticheForm")!=null)
			{
				StatisticheForm filters = JsonConvert.DeserializeObject<StatisticheForm>(HttpContext.Session.GetString("LogForm"));
				return Index(filters);

			}

			return Index(new StatisticheForm());
		}
		[HttpPost]
		public IActionResult Index(StatisticheForm filters)
		{
			if (!filters.DataDa.HasValue)
			{
				filters.DataDa = DateTime.Today.AddMonths(-1);
			}
			if (!filters.DataA.HasValue)
			{
				filters.DataA = DateTime.Today;
			}
			if (ModelState.IsValid)
			{
				if (!string.IsNullOrEmpty(filters.OrarioDa))
				{
					string[] fields = filters.OrarioDa.Split(":");
					int hours = int.Parse(fields[0]);
					int minutes = int.Parse(fields[1]);
					filters.DataDa=filters.DataDa.Value
						.AddHours(hours)
						.AddMinutes(minutes);
				}
				if (string.IsNullOrEmpty(filters.OrarioA)){
					filters.DataA = filters.DataA.Value.AddHours(23).AddMinutes(59);
				}
				else{
					string[] fields = filters.OrarioA.Split(":");
					int hours = int.Parse(fields[0]);
					int minutes = int.Parse(fields[1]);
					filters.DataA = filters.DataA.Value
						.AddHours(hours)
						.AddMinutes(minutes);
				}
			}



			IQueryable<Log> logsAccessi = logContext.Log
				.Where(x=>x.IdEventType==LogEvent.ACCESSO_EFFETTUATO);

			Statistiche statistiche = context.Statistiche
				.FirstOrDefault();

			IQueryable<Registrazione> andamento = context.Registrazione;
			if (filters.DataDa.HasValue)
			{
				logsAccessi = logsAccessi
					.Where(x => x.TimeStamp >= filters.DataDa);
				andamento = andamento
					.Where(x => x.DataInserimento >= filters.DataDa);
			}else{
				andamento = andamento.Where(x =>
					 x.DataInserimento >= DateTime.Today.AddDays(-30));
			}
			if (filters.DataA.HasValue)
			{
				logsAccessi = logsAccessi
					.Where(x => x.TimeStamp <= filters.DataA.Value.AddDays(1));
				andamento = andamento
					.Where(x => x.DataInserimento <= filters.DataA.Value);
			}
			else
			{
				andamento = andamento.Where(x =>
					x.DataInserimento <= DateTime.Today);
			}


			//Gestione andamento orario
			if (filters.DataDa.Value.Date == filters.DataA.Value.Date)
			{
				var datiAndamento = andamento
					.OrderBy(x => x.DataInserimento)
					.GroupBy(x => x.DataInserimento.Hour)
					.Select(r => new 
					{
						Data = r.Key,
						Registrazioni = r.Count()
					})
					.ToList();
				StringBuilder datiAndamentoX = new StringBuilder();
				StringBuilder datiAndamentoY = new StringBuilder();
				int oraMin = filters.DataDa.Value.Hour;
				int oraMax = filters.DataA.Value.Hour;
				for (int ora = oraMin; ora <= oraMax; ora++)
				{
					var data = datiAndamento.SingleOrDefault(d => d.Data == ora);
					datiAndamentoX.Append($"'{ora.ToString().PadLeft(2,'0')}:00',");
					datiAndamentoY.Append($"{data?.Registrazioni??0},");
				}
				ViewData["Andamento"] = datiAndamentoY.ToString();
				ViewData["AndamentoXLabel"] = "Ora";
				ViewData["AndamentoX"] = datiAndamentoX.ToString();
				statistiche.TotaleRegistrazioni = datiAndamento.Sum(x => x.Registrazioni);
			}
			//Gestione andamento Giornaliero
			else
			{
				List<StatisticheAndamento> datiAndamento= andamento
					.OrderBy(x => x.DataInserimento)
					.GroupBy(x => x.DataInserimento.Date)
					.Select(r => new StatisticheAndamento()
					{
						Data = r.Key,
						Registrazioni = r.Count()
					})
					.ToList();
				StringBuilder datiAndamentoX = new StringBuilder();
				StringBuilder datiAndamentoY = new StringBuilder();
				foreach (DateTime giorno in EachDay(datiAndamento.Min(s=>s.Data), datiAndamento.Max(s => s.Data)))
				{
					StatisticheAndamento dato = datiAndamento.SingleOrDefault(d => d.Data == giorno);
					datiAndamentoX.Append($"'{giorno:dd/MM/yyyy}',");
					datiAndamentoY.Append($"{dato?.Registrazioni??0},");
				}
				ViewData["Andamento"] = datiAndamentoY.ToString();
				ViewData["AndamentoXLabel"] = "Giorno";
				ViewData["AndamentoX"] = datiAndamentoX.ToString();
				statistiche.TotaleRegistrazioni = datiAndamento.Sum(x => x.Registrazioni);
			}
			statistiche.Accessi = logsAccessi.Count();
			statistiche.UtentiDistinti = logsAccessi
				.Select(x=>x.Username)
				.Distinct()
				.Count();
			ViewData["Statistiche"] = statistiche;
			return View(filters);
		}

		public IEnumerable<DateTime> EachDay(DateTime from, DateTime thru)
		{
			for (var day = from.Date; day.Date <= thru.Date; day = day.AddDays(1))
				yield return day;
		}
	}
}
