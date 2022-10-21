using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DomandeOnline.Data
{
	public class Documento
	{
		public int Id { get; set; }
        public byte[] FileDomanda { get; set; }
		public DomandaPartecipazione DomandaPartecipazione { get; set; }

	}
}