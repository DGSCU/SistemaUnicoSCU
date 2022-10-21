using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DomandeOnline.Data
{
	public class CV
	{
		public int Id { get; set; }
		public byte[] AllegatoCV { get; set; }
		public DomandaPartecipazione Domanda { get; set; }

	}
}