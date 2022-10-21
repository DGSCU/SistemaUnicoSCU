using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace RegistrazioneSistemaUnico.Models
{
	public class Comune
	{
		public Comune(){
			Registrazioni = new HashSet<Registrazione>();
		}

		public int Id{ get; set; }
		
		[Required]
		[MaxLength(50)]
		public string Nome { get; set; }

		[Required]
		public string CodiceCatastale { get; set; }

		[Required]
		public int IdProvincia { get; set; }

		public Provincia Provincia { get; set; }
		
		public virtual ICollection<Registrazione> Registrazioni { get; set; }

	}
}
