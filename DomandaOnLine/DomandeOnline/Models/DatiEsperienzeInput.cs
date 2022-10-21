using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DomandeOnline.Models
{
	public class DatiEsperienzeInput
	{
		public bool? PrecedentiEnte { get; set; }

		[DisplayName("Precedenti Esperienze con l'Ente")]
		[MaxLength(1000)]
		public string PrecedentiEnteDescrizione { get; set; }
		public bool? PrecedentiAltriEnti { get; set; }

		[DisplayName("Precedenti Esperienze con altri enti")]
		[MaxLength(1000)]
		public string PrecedentiAltriEntiDescrizione { get; set; }
		public bool? PrecedentiImpiego { get; set; }

		[DisplayName("Precedenti Esperienze in altri impieghi")]
		[MaxLength(1000)]
		public string PrecedentiImpiegoDescrizione { get; set; }

		public int? IdTitoloStudio { get; set; }

		public bool? FormazioneItalia { get; set; }

		[DisplayName("Tipo / Settore / Disciplina")]
		[MaxLength(200)]
		public string FormazioneDisciplina { get; set; }

		[DisplayName("Istituto")]
		[MaxLength(200)]
		public string FormazioneIstituto { get; set; }

		public int? FormazioneAnno { get; set; }

		public DateTime? FormazioneData { get; set; }

		[DisplayName("Ente che ha adottato il provvedimento")]
		[MaxLength(200)]
		public string FormazioneEnte { get; set; }

		public int? IscrizioneSuperioreAnno { get; set; }

		[DisplayName("Istituto iscrizione scuola media superiore")]
		[MaxLength(200)]
		public string IscrizioneSuperioreIstituto { get; set; }

		public int? IscrizioneLaureaAnno { get; set; }

		[DisplayName("Facoltà iscrizione di laurea")]
		[MaxLength(200)] public string IscrizioneLaureaCorso { get; set; }

		[DisplayName("Istituto iscrizione di laurea")]
		[MaxLength(200)]
		public string IscrizioneLaureaIstituto { get; set; }

		[DisplayName("Corsi")]
		[MaxLength(1000)]
		public string CorsiEffettuati { get; set; }

		[DisplayName("Specializzazioni")]
		[MaxLength(1000)]
		public string Specializzazioni { get; set; }

		[DisplayName("Competenze")]
		[MaxLength(1000)]
		public string Competenze { get; set; }

		[DisplayName("Altro")]
		[MaxLength(1000)]
		public string Altro { get; set; }
	}
}