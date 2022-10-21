using RegistrazioneSistemaUnico.Helpers.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RegistrazioneSistemaUnico.Models.Forms
{
	public class VariazioneRappresentanteLegale
	{
		/// <summary>
		/// codice Fiscalle dell'Ente. Deve essere una partita IVA formalemente corretta
		/// </summary>
		[DisplayName("Codice Fiscale Ente")]
		[Required(ErrorMessage = "Campo obbligatorio")]
		[Hint("Inserire la Codice Fiscale")]
		[Placeholder("Inserire il codice fiscale")]
		[PartitaIva(ErrorMessage = "Codice Fiscale Ente non valido (Partita IVA attesa)")]
		public string CodiceFiscaleEnte { get; set; }

		/// <summary>
		/// Denominazione dell'Ente
		/// </summary>
		[Required(ErrorMessage = "Campo obbligatorio")]
		[DisplayName("Denominazione Ente")]
		[Placeholder("Inserire la denominazione")]
		[Length(200, "Il valore non può superare i {0} caratteri")]
		public string Denominazione { get; set; }


		/// <summary>
		/// Codice fiscale del Rappresentante legale. Deve essere un codice fiscale formalmente valido
		/// </summary>
		[DisplayName("Codice Fiscale Rappresentante Legale")]
		[Required(ErrorMessage = "Campo obbligatorio")]
		[Placeholder("Inserire il codice fiscale")]
		[CodiceFiscale]
		public string CodiceFiscaleRappresentanteLegale { get; set; }


		[Required(ErrorMessage = "Campo obbligatorio")]
		[Date(ConsentiDataFutura = false)]
		[DisplayName("Data Nomina Rappresentante Legale")]
		[DataType(DataType.Date, ErrorMessage = "Data non valida")]
		[Placeholder("gg/mm/aaaa")]
		[Hint("Inserire la data di nomina del Rappresentante Legale")]
		public DateTime? DataNominaRappresentanteLegale { get; set; }
	}
}
