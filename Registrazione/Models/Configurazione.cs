using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RegistrazioneSistemaUnico.Models
{
	public class Configurazione
	{
		public int Id {get;set;}
		public string Parametro { get;set;}
		public string Descrizione { get;set;}
		public string Valore { get;set;}
	}
}
