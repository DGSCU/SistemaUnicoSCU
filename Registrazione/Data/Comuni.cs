using Microsoft.EntityFrameworkCore;
using RegistrazioneSistemaUnico.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RegistrazioneSistemaUnico.Data
{
	public class Comuni
	{
		private static List<Comune> comuni=new List<Comune>();
		private static List<Provincia> province=new List<Provincia>();
		private static DateTime? timeout;
		private static string connectionString;

		private const int timeoutMinutes = 5;

		public static void SetConnectionString(string connectionString)
		{
			Comuni.connectionString = connectionString;
		}

		private static void AggiornaDati()
		{
			if (string.IsNullOrEmpty(connectionString))
			{
				throw new ApplicationException("Stringa di connessione non configurata per l'accesso ai dati dei comuni. Utilizzare il metodo Comuni.SetConnectionString");
			}
			lock (comuni)
			{
				using (RegistrazioneContext context = new RegistrazioneContext(
					new DbContextOptionsBuilder<RegistrazioneContext>().UseSqlServer(connectionString).Options
				))
				{
					
					comuni = context.Comune
					.Include(c => c.Provincia)
					.ToList();
					province = context.Provincia
						.Include(c => c.Comuni)
						.ToList();
					timeout = DateTime.Now.AddMinutes(timeoutMinutes);
				}
			}
		}

		public static List<Comune> ElencoComuni{ get{
				if (timeout == null || DateTime.Now > timeout || comuni == null||province==null)
				{
					AggiornaDati();
				}
				return comuni;
		} }
		public static List<Provincia> ElencoProvince
		{
			get
			{
				if (timeout == null || DateTime.Now > timeout || comuni == null || province == null)
				{
					AggiornaDati();
				}
				return province;
			}
		}

	}
}
