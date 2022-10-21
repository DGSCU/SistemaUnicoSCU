using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RegistrazioneSistemaUnico.Models
{
	public class UtenteEnte
	{
		public string CodiceFiscale { get; set; }
		public string CodiceFiscaleEnte { get; set; }
		public string CodiceFiscaleEntePadre { get; set; }
		public string Denominazione { get; set; }
		public string Albo { get; set; }
		public string Username { get; set; }
		public bool? RappresentanteLegale { get; set; }
		public int? IdCategoriaEnte { get; set; }
		public DateTime? DataNominaRappresentanteLegale { get; set; }
		public int? IdTipologiaEnte { get; set; }
		public int? IdProvinciaEnte { get; set; }
		public int? IdComuneEnte { get; set; }
		public string Via { get; set; }
		public string Civico { get; set; }
		public string CAP { get; set; }
		public string Telefono { get; set; }
		public string Email { get; set; }
		public string PEC { get; set; }
		public string Sito { get; set; }
		public int? UtenzaSede { get; set; }



	}
}
