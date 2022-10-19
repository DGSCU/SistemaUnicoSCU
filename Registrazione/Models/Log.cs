using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RegistrazioneSistemaUnico.Data
{
	public class Log
	{
		public int Id { get; set; }
		public DateTime TimeStamp { get; set; }
		public int IdLevel { get; set; }
		public string IpAddress { get; set; }
		public string Username { get; set; }
		public string Name { get; set; }
		public string CodiceFiscaleEnte { get; set; }
		public int? IdEventType { get; set; }
		public int? EntityId { get; set; }
		/// <summary>
		/// Indica il messagio associato al log
		/// </summary>
		public string Message { get; set; }
		/// <summary>
		/// Contiene il testo dell'eventuale eccezione
		/// </summary>
		public string Exception { get; set; }
		public string Parameter { get; set; }

		public LogLevel LogLevel { get; set; }
		public LogEvent LogEvent { get; set; }
		public Ente Ente { get; set; }

		public string Voce
		{
			get
			{
				if (string.IsNullOrEmpty(Message))
					return LogEvent?.Descrizione;
				else 
					return $"{LogEvent?.Descrizione} ({Message})";
			}
		}
	}
}
