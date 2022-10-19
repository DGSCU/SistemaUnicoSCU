using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RegistrazioneSistemaUnico.Data
{


	public class Ente
	{
		public Ente(){
			Logs = new HashSet<Log>();
		}
		public string CodiceFiscale { get; set; }
		public string Denominazione { get; set; }
		public IEnumerable<Log> Logs { get; set; }

	}
}
