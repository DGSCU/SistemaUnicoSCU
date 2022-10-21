using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DomandeOnline.Models
{
	public class RichiestaCredenzialiInput
	{
		public string Nome { get; set; }
		public string Cognome { get; set; }
		public string CodiceGenere { get; set; }
		public DateTime? DataNascita { get; set; }
		public string LuogoNascita { get; set; }
		public int? IdNazioneNascita { get; set; }
		public string CodiceFiscale { get; set; }
		public int? IdNazioneCittadinanza { get; set; }
		public string Email { get; set; }
		public string RipetiEmail { get; set; }
		public string Telefono { get; set; }
		public HttpPostedFileBase Allegato { get; set; }
		public bool? OkPrivacy { get; set; }
		public bool? PrivacyConsenso { get; set; }

	}
}