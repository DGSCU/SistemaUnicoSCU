using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RegistrazioneSistemaUnico.Models
{
	public class DatiDomanda
	{
		public int Id { get; set; }
		public int? TipoDomanda { get; set; }
		public DateTime? DataProtocollazione { get; set; }
		public DateTime? DataProtocollo { get; set; }
		public DateTime? DataRichiestaAccreditamento { get; set; }
		public string NumeroProtocollo { get; set; }
		public DateTime? DataInvioEmail { get; set; }
		public byte[] BinData { get; set; }
		public string FileName { get; set; }
		public string Email { get; set; }
		public string PEC { get; set; }
		public string CodiceFiscaleEnte { get; set; }
		public string CodiceFiscaleRL { get; set; }
		public string CodiceRegione { get; set; }
		public string Denominazione { get; set; }
		public string Nome { get; set; }
		public string Cognome { get; set; }
		public int? IdEnteFase { get; set; }

	}
}
