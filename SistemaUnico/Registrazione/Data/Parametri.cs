using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Collections.Generic;

namespace RegistrazioneSistemaUnico.Data
{
	public static class Parametri
	{
		public static string SpidServiceEndpoint { get; set; }
		public static string SpidPageUrl { get; set; }
		public static string ProtocolloAutenticazioneServiceEndpoint { get; set; }
		public static string ProtocolloServiceEndpoint { get; set; }
		public static string ProtocolloNewEndpoint { get; set; }
		public static string ProtocolloCategoria { get; set; }
		public static string ProtocolloTipo { get; set; }
		public static string ProtocolloTipoDocumento { get; set; }
		public static string ProtocolloTipologia { get; set; }
		public static string ProtocolloTipoAllegato { get; set; }
		public static string ProtocolloAuthorizationName { get; set; }
		public static string ProtocolloAuthorizationLastName { get; set; }
		public static string ProtocolloAuthorizationPassword { get; set; }
		public static string ProtocolloFilePath { get; set; }

		public static string ProtocolloUnitaResponsabileRegistrazione { get; set; }
		public static string ProtocolloUnitaResponsabileIscrizione { get; set; }
		public static string ProtocolloUnitaResponsabileAdeguamento { get; set; }
		public static string ProtocolloUnitaResponsabileAntimafia { get; set; }
		public static string ProtocolloUnitaResponsabileOLP { get; set; }
		public static string ProtocolloUnitaResponsabileTUTORAGGIO { get; set; }
		public static string ProtocolloUnitaResponsabileIstanzaProgramma { get; set; }

		public static string ProtocolloCodiceTitolarioRegistrazione { get; set; }
		public static string ProtocolloCodiceTitolarioIscrizione { get; set; }
		public static string ProtocolloCodiceTitolarioAdeguamento { get; set; }
		public static string ProtocolloCodiceTitolarioAntimafia { get; set; }
		public static string ProtocolloCodiceTitolarioOLP { get; set; }
		public static string ProtocolloCodiceTitolarioTUTORAGGIO { get; set; }
		public static string ProtocolloCodiceTitolarioIstanzaDomanda { get; set; }

		public static string ProtocolloOggettarioRegistrazione { get; set; }
		public static string ProtocolloOggettarioIscrizione { get; set; }
		public static string ProtocolloOggettarioAdeguamento { get; set; }
		public static string ProtocolloOggettarioAntimafia { get; set; }
		public static string ProtocolloOggettarioOLP { get; set; }
		public static string ProtocolloOggettarioTUTORAGGIO { get; set; }
		public static string ProtocolloOggettarioIstanzaProgramma { get; set; }

		public static string ProtocolloFascicoloRegistrazione { get; set; }
		public static string ProtocolloFascicoloIscrizione { get; set; }
		public static string ProtocolloFascicoloAdeguamento { get; set; }
		public static string ProtocolloFascicoloAntimafia { get; set; }
		public static string ProtocolloFascicoloOLP { get; set; }
		public static string ProtocolloFascicoloTUTORAGGIO { get; set; }
		public static string ProtocolloFascicoloIstanzaProgramma { get; set; }

		public static string IndirizzoEmail { get; set; }
		public static string ServerSMTPEndpoint { get; set; }
		public static string ServerSMTPPort { get; set; }
		public static string ServerSMTPUsername { get; set; }
		public static string ServerSMTPPassword { get; set; }
		public static string ServerSMTPMailFrom { get; set; }
		public static string OverrideEmail { get; set; }
		public static int TokenAccessoDurataSecondi { get; set; }
		public static string UrlAccessoHelios { get; set; }
		public static string ControlloFirma { get; set; }
		public static string SimulazioneSPID { get; set; }
		public static string heliosDB { get; set; }

		public static void CaricaParametri(IConfiguration configuration){
			Dictionary<string, string> parametri = new Dictionary<string, string>();
			using (RegistrazioneContext context = new RegistrazioneContext(
				new DbContextOptionsBuilder<RegistrazioneContext>()
					.UseSqlServer(configuration.GetConnectionString("RegistrazioneDB"))
					.Options
			))
			{
				parametri = context.Configurazione.ToDictionaryAsync(
					key => key.Parametro,
					value => value.Valore)
					.Result;
			}
			foreach (PropertyInfo property in typeof(Parametri).GetProperties())
			{
				if (property.PropertyType == typeof(int)|| property.PropertyType == typeof(int?))
				{
					int? value = null;
					if (parametri.ContainsKey(property.Name))
					{
						if (int.TryParse(parametri[property.Name], out int intValue))
						{
							value = intValue;
						}
					}
					else
					{
						value = configuration.GetValue<int>($"Parameters:{property.Name}");
					}
					property.SetValue(null, value);
				}
				else
				{
					string value;
					if (parametri.ContainsKey(property.Name))
					{
						value = parametri[property.Name];
					}
					else
					{
						value = configuration.GetValue<string>($"Parameters:{property.Name}");
					}
					property.SetValue(null, value);
				}
			}
		}

	}
}
