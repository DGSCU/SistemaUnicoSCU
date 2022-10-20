using System;
using System.Configuration;
using System.Reflection;

namespace ScheduleAgent.Class
{
	public class Parameters
	{
		public string ProtocolloTipo { get; private set; }
		public string ProtocolloUnitaResponsabile { get; private set; }
		public string ProtocolloCodiceTitolario { get; private set; }
		public string ProtocolloFilePath { get; private set; }
		public string ProtocolloAuthorizationName { get; private set; }
		public string ProtocolloAuthorizationLastName { get; private set; }
		public string ProtocolloAuthorizationPassword { get; private set; }
		public string IndirizzoInvioMail { get; private set; }
		public string ServerSMTP { get; private set; }
		public string IndirizzoRispostaMail { get; private set; }
		public string IndirizzoMailDomandeAnnullate { get; private set; }
		public string ProtocolloCategoria { get; private set; }
		public string IndirizzoToMailInvioErrore { get; private set; }
		public string IndirizzoFromMailInvioErrore { get; private set; }	

		public Parameters()
		{
			foreach (PropertyInfo proprieta in GetType().GetProperties())
			{
				string value = ConfigurationManager.AppSettings[proprieta.Name];
				if (value == null)
				{
					throw new Exception($"Parametro di configurazione \"{proprieta.Name}\" mancante.");
				}
				proprieta.SetValue(this, value);
			}
		}
	}
}