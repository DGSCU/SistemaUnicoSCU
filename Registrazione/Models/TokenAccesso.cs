using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RegistrazioneSistemaUnico.Models
{
	public class TokenAccesso
	{
		[Required]
		[Key]
		public Guid Id { get; set; }
		
		[Required]
		public DateTime DataScadenza { get; set; }
		
		[Required]
		public string CodiceFiscale { get; set; }
		
		[Required]
		public string CodiceFiscaleEnte { get; set; }

		[Required]
		public string Albo { get; set; }

		[Required]
		public string Username { get; set; }

		[Required]
		public bool Utilizzato { get; set; }
	}
}
