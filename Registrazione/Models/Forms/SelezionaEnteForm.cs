using RegistrazioneSistemaUnico.Helpers;
using RegistrazioneSistemaUnico.Helpers.Attributes;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RegistrazioneSistemaUnico.Models.Forms
{
	public class SelezionaEnteForm
	{
		/// <summary>
		/// codice Fiscalle dell'Ente. Deve essere una partita IVA formalemente corretta
		/// </summary>
		[DisplayName("Codice fiscale Ente")]
		[Required(ErrorMessage = "Campo obbligatorio")]
		[Placeholder("Inserire il codice fiscale")]
		[PartitaIva(ErrorMessage = "Codice Fiscale Ente non valido")]
		public string CodiceFiscaleEnte { get; set; }


		[Required(ErrorMessage = "Campo obbligatorio")]
		[DisplayName("Categoria Ente")]
		[Placeholder("Selezionare la categoria dell'ente")]
		public int? IdCategoriaEnte { get; set; }

		[Required(ErrorMessage = "Campo obbligatorio")]
		[DisplayName("Categoria Ente")]
		[Placeholder("Selezionare sedell'ente")]
		public bool? EnteTitolare { get; set; }
		
		
		/// <summary>
		/// indica se è si è avviato dall'accesso;
		/// </summary>
		public bool? fromAccesso { get; set; }


	}
}
