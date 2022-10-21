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
using System.Threading.Tasks;
using System.Transactions;
using TheArtOfDev.HtmlRenderer.PdfSharp;

namespace RegistrazioneSistemaUnico.Controllers
{
	[Authorize(Roles ="Negata")]
	public class LogController : SmartController
	{

		public LogController(RegistrazioneContext registrazioneContext, LogContext logContext) : base(registrazioneContext, logContext)
		{
		}
		[HttpGet]
		public IActionResult Index(){
			
			if (HttpContext.Session.GetString("LogForm")!=null)
			{
				LogForm logform = JsonConvert.DeserializeObject<LogForm>(HttpContext.Session.GetString("LogForm"));
				return Index(logform);

			}

			return Index(new LogForm());
		}
		[HttpPost]
		public IActionResult Index(LogForm filters)
		{
			HttpContext.Session.SetString("LogForm", JsonConvert.SerializeObject(filters));
			IQueryable<Log> logs = logContext.Log
				.Include(l => l.LogEvent)
				.Include(l => l.LogLevel)
				.Include(l => l.Ente);
			if (filters.DataDa.HasValue)
			{
				logs = logs.Where(l => l.TimeStamp >= filters.DataDa);
			}
			if (filters.DataA.HasValue)
			{
				logs = logs.Where(l => l.TimeStamp <= filters.DataA);
			}
			if (!string.IsNullOrEmpty(filters.NomeUtente))
			{
				logs = logs.Where(l =>
					l.Username.Contains(filters.NomeUtente) ||
					l.Name.Contains(filters.NomeUtente));
			}
			if (!string.IsNullOrEmpty(filters.NomeEnte))
			{
				logs = logs.Where(l =>
					l.CodiceFiscaleEnte.Contains(filters.NomeEnte) ||
					l.Ente.Denominazione.Contains(filters.NomeEnte));
			}
			if (filters.IdEventType.HasValue){ 
				logs = logs.Where(l => l.IdEventType == filters.IdEventType);
			}
			if (filters.LogLevel.HasValue)
			{
				logs = logs.Where(l => l.IdLevel == filters.LogLevel);
			}else
			{
				logs = logs.Where(l => l.IdLevel != 1);
			}


			var logList=ResultListService.GetOrderedList(logs, filters);
			ViewData["IdEventType"]=logContext.Event
				.OrderBy(c => c.Descrizione)
				.ToDictionary(k => k.Id.ToString(), v => v.Descrizione);

			ViewData["List"] = logList;
			return View(filters);
		}

		public IActionResult Dettaglio(int Id)
		{
			Log log = logContext.Log
				.SingleOrDefault(l => l.Id == Id);
			if (log==null)
			{
				return RedirectToAction("Index","Log");
			}
			Registrazione registrazione = context.Registrazione
				.Include(l=>l.TipologiaEnte)
				.Include(l=>l.Categoria)
				.Include(l=>l.Comune)
				.Include(l=>l.Provincia)
				.SingleOrDefault(l => l.Id == log.EntityId);
			if (registrazione==null)
			{
				return RedirectToAction("Index", "Log");
			}
			return View(registrazione);
		}

		public IActionResult DettaglioErrore(int Id)
		{
			Log log = logContext.Log
				.Include(l=>l.Ente)
				.SingleOrDefault(l => l.Id == Id);
			if (log == null)
			{
				return RedirectToAction("Index", "Log");
			}
			return View(log);

		}

		public IActionResult DettaglioDocumento(int Id)
		{
			Documento documento = context.Documento
				.SingleOrDefault(l => l.Id == Id);
			if (documento == null)
			{
				return RedirectToAction("Index", "Log");
			}
			return File(documento.Blob, "application/pdf", documento.NomeFile);

		}
	}
}
