using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RegistrazioneSistemaUnico.Data;
using RegistrazioneSistemaUnico.Helpers;
using RegistrazioneSistemaUnico.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace RegistrazioneSistemaUnico.Controllers
{
	public class ProtocollazioneController : SmartController
	{
		private static object Semaforo = new object();
		private IViewRenderService renderService;
		public ProtocollazioneController(RegistrazioneContext registrazioneContext, IViewRenderService renderService) : base(registrazioneContext)
		{
			this.renderService = renderService;
		}
		public IActionResult Index()
		{
			lock (Semaforo)
			{
				List<int> idRegistrazioniDaProtocollare = context.Registrazione
					.Where(r => r.DataInvioEmail == null)
					.Select(r => r.Id)
					.ToList();

				foreach (int idRegistrazione in idRegistrazioniDaProtocollare)
				{
					Registrazione registrazione = context.Registrazione
						.Include(r => r.Documento)
						.SingleOrDefault(r => r.Id == idRegistrazione);

					registrazione.DataProtocollazione = DateTime.Now;
					context.SaveChanges();
					registrazione.DataInvioEmail = DateTime.Now;
					context.SaveChanges();
				}
				return View();
			}
		}
	}
}
