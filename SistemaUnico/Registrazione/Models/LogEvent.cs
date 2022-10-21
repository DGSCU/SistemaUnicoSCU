using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RegistrazioneSistemaUnico.Data
{


	public class LogEvent
	{
		public const int ERRORE = 1;
		public const int REGISTRAZIONE_DATI_NON_INTEGRI = 2;
		public const int REGISTRAZIONE_GIA_EFFETTUATA = 3;
		public const int REGISTRAZIONE_ENTE_INESISTENTE = 4;
		public const int IDENTITA_NON_VALIDA = 5;
		public const int AUTORIZZAZIONE_SCADUTA = 6;
		public const int ACCESSO_NON_AUTORIZZATO = 7;
		public const int PAGINA_INESISTENTE = 8;
		public const int ACCESSO_EFFETTUATO = 9;
		public const int DISCONNESSIONE = 10;
		public const int INIZIO_REGISTRAZIONE = 11;
		public const int INSERITI_DATI = 12;
		public const int CANCELLATI_DATI = 13;
		public const int DOWNLOAD_DOCUMENTO = 14;
		public const int REGISTRAZIONE_EFFETTUATA = 15;
		public const int ERRORE_PROTOCOLLO = 16;
		public const int PROTOCOLLO_EFFETTUATO = 17;
		public const int ERRORE_FIRMA = 18;
		public const int ERRORE_ACCESSO_HELIOS = 19;
		public const int ERRORE_ACCESSO_SPID = 20;

		public LogEvent(){
			Logs = new HashSet<Log>();
		}
		public int Id { get; set; }
		public string Descrizione { get; set; }
		public IEnumerable<Log> Logs { get; set; }
	}
}
