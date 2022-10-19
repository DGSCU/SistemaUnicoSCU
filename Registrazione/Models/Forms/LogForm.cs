using Microsoft.AspNetCore.Http;
using RegistrazioneSistemaUnico.Helpers;
using RegistrazioneSistemaUnico.Helpers.Attributes;
using RegistrazioneSistemaUnico.Helpers.Grid;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RegistrazioneSistemaUnico.Models.Forms
{
	public class LogForm:ListFilter
	{
		public int? LogLevel { get; set; }
		
		[DisplayName("Operazione Effettuata")]
		public int? IdEventType { get; set; }
		
		[DisplayName("Utente")]
		public string NomeUtente { get; set; }

		[DisplayName("Ente")]
		public string NomeEnte { get; set; }

		[DisplayName("Data Inizio")]
		public DateTime? DataDa { get; set; }
		
		[DisplayName("Data Fine")]
		public DateTime? DataA { get; set; }

	}
}
