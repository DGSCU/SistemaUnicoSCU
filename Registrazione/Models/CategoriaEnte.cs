using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RegistrazioneSistemaUnico.Models
{
	public class CategoriaEnte
	{
		public CategoriaEnte(){
			Tipologie = new HashSet<TipologiaEnte>();
			Registrazioni = new HashSet<Registrazione>();
		}
		public int Id{ get; set; }
		[Required]
		[DisplayName("Descrizione")]
		public string Descrizione { get; set; }

		public ICollection<TipologiaEnte> Tipologie { get; set; }
		
		public ICollection<Registrazione> Registrazioni { get; set; }


	}
}
