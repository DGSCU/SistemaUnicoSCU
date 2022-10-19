using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Logger.Data
{
	public class Log
	{
		public int Id { get; set; }
		public DateTime TimeStamp { get { return DateTime.Now; } }
		public DateTime? StartTime { get; set; }
		public int? Duration
		{
			get
			{
				if (!StartTime.HasValue)
				{
					return null;
				}
				return Convert.ToInt32((DateTime.Now - StartTime.Value).TotalMilliseconds);
			}
		}
		public int IdLevel { get; set; }
		public string IpAddress { get; set; }
		public string Username { get; set; }
		public string Name { get; set; }
		public string CodiceFiscaleEnte { get; set; }
		public int? IdUser { get; set; }
		public int? IdEventType { get; set; }
		public int? EntityId { get; set; }

		[MaxLength(128)]
		public string EntityName { get; set; }

		public string ApplicationIpAddress { get; set; }
		public string ApplicationHost { get; set; }
		public string ApplicationName { get; set; }
		public string SessionId { get; set; }

		[MaxLength(128)]
		public string Controller { get; set; }
		[MaxLength(128)]
		public string Action { get; set; }

		public string Method { get; set; }

		/// <summary>
		/// Indica il messagio associato al log
		/// </summary>
		public string Message { get; set; }
		/// <summary>
		/// Contiene il testo dell'eventuale eccezione
		/// </summary>
		public Exception Exception { get; set; }
		public string Parameter { get; set; }
	}
}
