using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DomandeOnline.Models
{
	public class Utente
	{
		public string Id { get; set; }
		public string Nome { get; set; }
		public IEnumerable<string> Ruoli { get; set; }
	}
}