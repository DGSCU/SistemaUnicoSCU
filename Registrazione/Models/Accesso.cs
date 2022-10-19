using RegistrazioneSistemaUnico.Helpers.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RegistrazioneSistemaUnico.Models
{
	public class Accesso
	{
		[DisplayName("Codice Fiscale")]
		[Required(ErrorMessage = "Campo obbligatorio")]
		[CodiceFiscale]
		public string CodiceFiscale { get; set; }

		[DisplayName("Nome")]
		[Required(ErrorMessage = "Campo obbligatorio")]
		public string Nome { get; set; }

		[DisplayName("Cognome")]
		[Required(ErrorMessage = "Campo obbligatorio")]
		public string Cognome { get; set; }

		[DisplayName("Data di nascita")]
		[Required(ErrorMessage = "Campo obbligatorio")]
		[Date]
		public DateTime? DataNascita { get; set; }
	}
}
