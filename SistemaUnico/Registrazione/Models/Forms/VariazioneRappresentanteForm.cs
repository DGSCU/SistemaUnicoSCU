using RegistrazioneSistemaUnico.Helpers;
using RegistrazioneSistemaUnico.Helpers.Attributes;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RegistrazioneSistemaUnico.Models.Forms
{
	public class VariazioneRappresentanteForm
	{
		/// <summary>
		/// codice Fiscalle dell'Ente. Deve essere una partita IVA formalemente corretta
		/// </summary>
		[DisplayName("Codice fiscale Ente")]
		[Required(ErrorMessage = "Campo obbligatorio")]
		[Placeholder("Inserire il codice fiscale")]
		[PartitaIva(ErrorMessage = "Codice Fiscale Ente non valido")]
		public string CodiceFiscaleEnte { get; set; }

		
		/// <summary>
		/// indica se è si è avviato dall'accesso;
		/// </summary>
		public bool? fromAccesso { get; set; }


	}
}
