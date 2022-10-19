using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RegistrazioneSistemaUnico.Models
{
	public class ProtocolloProgramma
	{
		public int Id { get; set; }
		public DateTime? DataProtocollazione { get; set; }
		public DateTime? DataProtocollo { get; set; }
		public string NumeroProtocollo { get; set; }
		public DateTime? DataInvioEmail { get; set; }
		public DateTime? DataAnnullamento { get; set; }
		public DateTime? DataProtocollazioneAnnullamento { get; set; }
		public string NumeroProtocolloAnnullamento { get; set; }
		public DateTime? DataProtocolloAnnullamento { get; set; }
		public DateTime? DataInvioEmailAnnullamento { get; set; }
	}
}
