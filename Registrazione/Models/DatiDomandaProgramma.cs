using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RegistrazioneSistemaUnico.Models
{
	public class DatiDomandaProgramma
	{
		
		public int Id { get; set; }
		public byte[] BinData { get; set; }
		public string FileName { get; set; }
		public DateTime? DataPresentazioneSistema { get; set; }
		public DateTime? DataProtocollazione { get; set; }
		public string NumeroProtocollo { get; set; }
		public DateTime? DataProtocollo { get; set; }
		public DateTime? DataInvioEmail { get; set; }
		public string CodiceRegione { get; set; }
		public string Denominazione { get; set; }
		public string CodiceFiscaleEnte { get; set; }
		public string Bando { get; set; }
		public string Email { get; set; }
		public string PEC { get; set; }
		public DateTime? DataAnnullamento { get; set; }
		public DateTime? DataProtocollazioneAnnullamento { get; set; }
		public string NumeroProtocolloAnnullamento { get; set; }
		public DateTime? DataProtocolloAnnullamento { get; set; }
		public DateTime? DataInvioEmailAnnullamento { get; set; }
	}
}
