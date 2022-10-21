using System;

namespace DomandeOnline.Models
{
	public class RichiestaCredenzialiDto
	{
		public int Id { get; set; }
		public DateTime? DataRichiesta { get; set; }
		public string Stato { get; set; }
		public DateTime? DataApprovazione { get; set; }
		public string UtenteApprovazione { get; set; }
		public DateTime? DataAnnullamento { get; set; }
		public string UtenteAnnullamento { get; set; }
		public string NoteAnnullamento { get; set; }
		public string NoteApprovazione { get; set; }
		public string Nome { get; set; }
		public string Cognome { get; set; }
		public string Genere { get; set; }
		public DateTime? DataNascita { get; set; }
		public string LuogoNascita { get; set; }
		public string NazioneNascita { get; set; }
		public string CodiceFiscale { get; set; }
		public string NazioneCittadinanza { get; set; }
		public string Email { get; set; }
		public string Telefono { get; set; }
	}

	public class RichiestaCredenzialiFilter
	{
		public DateTime? DataRichiestaDa { get; set; }
		public DateTime? DataRichiestaA { get; set; }
		public int?  IdStato { get; set; }
		public DateTime? DataApprovazioneDa { get; set; }
		public DateTime? DataApprovazioneA { get; set; }
		public string UtenteApprovazione { get; set; }
		public string Nome { get; set; }
		public string CodiceFiscale { get; set; }
		public string Email { get; set; }
	}
}