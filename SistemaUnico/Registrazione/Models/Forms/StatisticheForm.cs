using Microsoft.AspNetCore.Http;
using RegistrazioneSistemaUnico.Helpers;
using RegistrazioneSistemaUnico.Helpers.Attributes;
using RegistrazioneSistemaUnico.Helpers.Grid;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RegistrazioneSistemaUnico.Models.Forms
{
	public class StatisticheForm
	{

		[DisplayName("Data Inizio")]
		[Date]
		public DateTime? DataDa { get; set; }
		public int? OraDa { get; set; }
		public int? MinutiDa { get; set; }

		[Time]
		public string OrarioDa { get; set; }

		[DisplayName("Data Fine")]
		[Date]
		public DateTime? DataA { get; set; }
		public int? OraA { get; set; }
		public int? MinutiA { get; set; }
		[Time]
		public string OrarioA { get; set; }

	}
}
