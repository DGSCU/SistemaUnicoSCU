using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Logger.Data
{
	public class LogTable
	{
		public int Id { get; set; }
		public DateTime TimeStamp { get; set; }
		public DateTime? StartTime { get; set; }
		public int? Duration { get; set; }
		public int IdLevel { get; set; }
		public string IpAddress { get; set; }
		public string Username { get; set; }
		public string Name { get; set; }
		public string CodiceFiscaleEnte { get; set; }
		public int? IdEventType { get; set; }
		public int? EntityId { get; set; }

		[MaxLength(128)]
		public string EntityName { get; set; }
		public string ApplicationName { get; set; }

		[MaxLength(128)]
		public string Action { get; set; }

		public string Controller { get; set; }
		public string Method { get; set; }

		/// <summary>
		/// Indica il messagio associato al log
		/// </summary>
		public string Message { get; set; }
		/// <summary>
		/// Contiene il testo dell'eventuale eccezione
		/// </summary>
		public string Exception { get; set; }
		public string Parameter { get; set; }
		public Guid? SessionId { get; set; }
		
	}
}
