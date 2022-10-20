using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ScheduleAgent.Models
{
	public class LogDto
	{
		public int Id { get; set;}
		public string Messaggio { get; set;}
		public DateTime Timestamp { get; set; }
		public string Errore { get; set; }

	}
}