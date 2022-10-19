using Microsoft.AspNetCore.Http;
using RegistrazioneSistemaUnico.Helpers;
using RegistrazioneSistemaUnico.Helpers.Attributes;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RegistrazioneSistemaUnico.Models.Forms
{
	public class FileForm
	{
		[Required(ErrorMessage = "Non è stato caricato il documento.")]
		public IFormFile Documento { get; set; }

		[Summary("Dichiarazione Privacy")]
		[DisplayName("Ho letto l'informativa della Privacy.")]
		[Required(ErrorMessage = "Dichiarazione Obbligatoria")]
		public bool? DichiarazionePrivacy { get; set; }

		//[Summary("Dichiarazione Privacy")]
		//[DisplayName("Do il consenso al trattamento dei dati.")]
		//[Required(ErrorMessage = "Dichiarazione Obbligatoria")]
		//public bool? Consenso { get; set; }

		[Summary("Dichiarazione Rappresentante")]
		[DisplayName("Dichiaro di essere il Rappresentante Legale dell'Ente.")]
		[Required(ErrorMessage = "Dichiarazione Obbligatoria")]
		public bool? DichiarazioneRappresentanteLegale { get; set; }


		public bool? Download { get; set; }
	}
}
