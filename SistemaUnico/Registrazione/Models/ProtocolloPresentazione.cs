using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RegistrazioneSistemaUnico.Models
{
	public class ProtocolloPresentazione
	{
		public int Id { get; set; }
		public int IdEnte { get; set; }
		public int? IdEnteFase { get; set; }
		public int? TipoDomanda { get; set; }
		public DateTime? DataProtocollazione { get; set; }
		public DateTime? DataProtocollo { get; set; }
		public string NumeroProtocollo { get; set; }
		public DateTime? DataInvioEmail { get; set; }

	}
}
