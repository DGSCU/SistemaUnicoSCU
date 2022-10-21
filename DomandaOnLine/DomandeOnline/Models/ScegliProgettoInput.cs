using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DomandeOnline.Models
{
	public class TipologiaProgramma
    {
		public string Valore { get; set; }
		public string Descrizione { get; set; }
		public bool IsSelezionato { get; set; }
    }

	public class ScegliProgettoInput
	{
		public string CodiceProgetto {get; set;}
		public string NomeProgetto {get; set;}
		public string Regione {get; set;}
		public string Provincia {get; set;}
		public string Comune {get; set;}
		public string Settore {get; set;}
		public string Area {get; set;}
		public string Programma { get; set;}
		public int? Obiettivo { get; set;}
		public int? Ambito { get; set; }
		public int? MinoreOpportunita { get; set; }
		public bool? GaranziaGiovani { get; set; }
		public string TipoGaranziaGiovani { get; set; }
		public string TipoRicerca {get; set;}
		public string AggiungiPreferito {get; set;}
		public string RimuoviPreferito {get; set;}
		public string CodiceEnte {get; set;}
		public string Ente {get; set;}
		public int? NuovaPagina {get; set;}
		public string Misure {get; set;}
		public string Minori {get; set;}
		public string TipoMisure {get; set;}
		public string Nazione {get; set;}
		public bool? SoloPreferiti {get; set;}
		public int? Pagina {get; set;}
		public int? ElementiPerPagina { get; set; }
		public int? ScrollPosition { get; set; }
		public bool HasProgrammi { get; set; }
		public bool? IsDigitale { get; set; }
		public List<TipologiaProgramma> ListaTipologiaProgramma { get; set; }
        public string TipologiaProgrammaScelta { get; set; }
		public int IdBando { get; set; }
	}
}