using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DomandeOnline.Models
{
	public class ElencoCredenzialiInput
	{
		public int? IdStato { get; set; }
		public string CodiceFiscale { get; set; }
		public string Nome { get; set; }
		public string Cognome { get; set; }
		public int? NuovaPagina { get; set; }
		public int? Pagina { get; set; }
		public int? NumeroPagine { get; set; }
		public int? NumeroElementi { get; set; }
		public int? ElementiPerPagina { get; set; }
	}
}