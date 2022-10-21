using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace RegistrazioneSistemaUnico.Models
{
	public class Soggetto
	{
		public string Nome { get; set; }
		public string Cognome { get; set; }
		public string CodiceFiscale { get; set; }

	}
}
