using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RegistrazioneSistemaUnico.Models
{
	public class ProtocolloOLP
	{
		public int Id { get; set; }
		public DateTime? DataProtocollazione { get; set; }
		public DateTime? DataProtocollo { get; set; }
		public string NumeroProtocollo { get; set; }
		public DateTime? DataInvioEmail { get; set; }

	}
}
