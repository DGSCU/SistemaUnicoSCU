using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RegistrazioneSistemaUnico.Models
{
	public class TipologiaEnte
	{
		public TipologiaEnte(){
			Registrazioni = new HashSet<Registrazione>();
		}

		public int Id{ get; set; }
		[Required]
		[DisplayName("Descrizione")]
		public string Descrizione { get; set; }
		public int IdCategoriaEnte { get; set; }
/*		public DateTime DataInserimento { get; set; }
		public string UtenteInserimento { get; set; }
		public DateTime? DataCancellazione { get; set; }
		public string UtenteCancellazione { get; set; }*/
	
		public CategoriaEnte Categoria{ get; set; }
		
		public IEnumerable<Registrazione> Registrazioni { get; set; }
	}
}
