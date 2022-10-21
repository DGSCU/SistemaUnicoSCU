using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RegistrazioneSistemaUnico.Models
{
	public class Statistiche
	{
		public int Totale { get; set; }
		public int EntiRegistrati { get; set; }
		public int EntiNonRegistrati { get; set; }
		public int NuoviEntiRegistrati { get; set; }
		public int VariazioniRappresentanteLegale { get; set; }
		public int EntiTitolari { get; set; }
		public int EntiDiAccoglienza { get; set; }
		public int EntiTitolariRegistrati { get; set; }
		public int EntiDiAccoglienzaRegistrati { get; set; }
		public int EntiTitolariPrivatiRegistrati { get; set; }
		public int EntiTitolariPubbliciRegistrati { get; set; }
		public int EntiAccoglienzaPrivatiRegistrati { get; set; }
		public int EntiAccoglienzaPubbliciRegistrati { get; set; }
		[NotMapped]
		public int Accessi { get; set; }
		[NotMapped]
		public int UtentiDistinti { get; set; }

		[NotMapped]
		public int TotaleRegistrazioni { get; set; }
	}
}
