using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace RegistrazioneSistemaUnico.Models
{
	public class Provincia
	{
		public Provincia()
		{
			Registrazioni = new HashSet<Registrazione>();
			Comuni = new HashSet<Comune>();
		}


		public int Id{ get; set; }
		
		[Required]
		[MaxLength(50)]
		public string Nome { get; set; }

		[Required]
		public string Sigla { get; set; }

		public virtual ICollection<Comune> Comuni { get; set; }
		public virtual ICollection<Registrazione> Registrazioni { get; set; }

	}
}
