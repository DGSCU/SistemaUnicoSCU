using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DomandeOnline.Models
{
	public class Indirizzo
	{
		public string Via { get; set; }
		public string Civico { get; set; }
		public string CAP { get; set; }
		public string Comune { get; set; }
		public string Provincia { get; set; }
	}
}