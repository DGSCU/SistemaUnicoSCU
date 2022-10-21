using ProtocolloAutenticazioneService;
using ProtocolloNewService;
using ProtocolloService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RegistrazioneSistemaUnico.Helpers
{
	public class Protocollazione
	{

		public enum TipoProtocollo
		{
			Standard=1,
			Registrazione=2,
			Iscrizione=3,
			Adeguamento = 4,
			Antimafia=5,
			OLP =6,
			IstanzaPaogramma= 7,
			TUTORAGGIO =8
		}
		//public static NewProtocollo InviaProtocollo(
		//	string oggetto,
		//	string codiceFascicolo,
		//	string codiceDestinatario,
		//	string pec,
		//	string nomeFile,
		//	byte[]fileBytes)
		// {
		//	PostProtocolloClient wsPost = new PostProtocolloClient( PostProtocolloClient.EndpointConfiguration.BasicHttpBinding_IPostProtocollo, Data.Parametri.ProtocolloNewEndpoint);
		//	Utente ute = new Utente();
		//	Documento doc = new Documento();
		//	Ente ente = new Ente();
		//	Destinatario destinatario = new Destinatario();
		//	Fascicolo fasc = new Fascicolo();
		//	FileToSend file = new FileToSend();
		//	Allegati allegati = new Allegati();
		//	string b64 = Convert.ToBase64String(fileBytes);
		//	ute.Cognome = Data.Parametri.ProtocolloCognomeUtente;

		//	ute.Nome = Data.Parametri.ProtocolloNomeUtente;

		//	doc.Oggetto = oggetto;

		//	fasc.TitolarioP = Data.Parametri.ProtocolloCodiceTitolario;

		//	fasc.AooP = Data.Parametri.ProtocolloUnitaResponsabile;

		//	fasc.CodfascP = codiceFascicolo;

		//	file.NomeFile = nomeFile;

		//	file.FileB64 = b64;

		//	file.FileToSendLocal = "";

		//	destinatario.CodiceDest = codiceDestinatario;

		//	destinatario.EmailPECdest = pec;

		//	allegati.Fileallegati = new string[1] { nomeFile + "#" + b64 };
			
		//	return wsPost.ProtocolloNew(ute, doc, ente, destinatario, allegati, fasc, file);
		//}


		public static PROTOCOLLOEX_CREATO InviaProtocollo(
			string token,
			string oggetto,
			string codiceFascicolo,
			string destinatario,
			string codiceDestinatario,
			string pec,
			string nomeFile,
			byte[] fileBytes,
			TipoProtocollo tipo=TipoProtocollo.Standard)
		{

			string anno = DateTime.Today.Year.ToString();
			SIGED_WSSoapClient client = new SIGED_WSSoapClient(
				SIGED_WSSoapClient.EndpointConfiguration.SIGED_WSSoap,
				Data.Parametri.ProtocolloServiceEndpoint);
			string b64 = Convert.ToBase64String(fileBytes);

			string protocolloCodiceTitolario;
			string protocolloUnitaResponsabile;
			string protocolloFascicolo;
			switch (tipo)
			{
				case TipoProtocollo.Registrazione:
					protocolloCodiceTitolario = Data.Parametri.ProtocolloCodiceTitolarioRegistrazione;
					protocolloUnitaResponsabile = Data.Parametri.ProtocolloUnitaResponsabileRegistrazione;
					protocolloFascicolo = Data.Parametri.ProtocolloFascicoloRegistrazione;
					break;
				case TipoProtocollo.Iscrizione:
					protocolloCodiceTitolario = Data.Parametri.ProtocolloCodiceTitolarioIscrizione;
					protocolloUnitaResponsabile = Data.Parametri.ProtocolloUnitaResponsabileIscrizione;
					protocolloFascicolo = Data.Parametri.ProtocolloFascicoloIscrizione;
					break;
				case TipoProtocollo.Adeguamento:
					protocolloCodiceTitolario = Data.Parametri.ProtocolloCodiceTitolarioAdeguamento;
					protocolloUnitaResponsabile = Data.Parametri.ProtocolloUnitaResponsabileAdeguamento;
					protocolloFascicolo = Data.Parametri.ProtocolloFascicoloAdeguamento;
					break;
				case TipoProtocollo.Antimafia:
					protocolloCodiceTitolario = Data.Parametri.ProtocolloCodiceTitolarioAntimafia;
					protocolloUnitaResponsabile = Data.Parametri.ProtocolloUnitaResponsabileAntimafia;
					protocolloFascicolo = Data.Parametri.ProtocolloFascicoloAntimafia;
					break;
				case TipoProtocollo.OLP:
					protocolloCodiceTitolario = Data.Parametri.ProtocolloCodiceTitolarioOLP;
					protocolloUnitaResponsabile = Data.Parametri.ProtocolloUnitaResponsabileOLP;
					protocolloFascicolo = Data.Parametri.ProtocolloFascicoloOLP;
					break;
				case TipoProtocollo.TUTORAGGIO:
					protocolloCodiceTitolario = Data.Parametri.ProtocolloCodiceTitolarioTUTORAGGIO;
					protocolloUnitaResponsabile = Data.Parametri.ProtocolloUnitaResponsabileTUTORAGGIO;
					protocolloFascicolo = Data.Parametri.ProtocolloFascicoloTUTORAGGIO;
					break;
				case TipoProtocollo.IstanzaPaogramma:
					protocolloCodiceTitolario = Data.Parametri.ProtocolloCodiceTitolarioIstanzaDomanda;
					protocolloUnitaResponsabile = Data.Parametri.ProtocolloUnitaResponsabileIstanzaProgramma;
					protocolloFascicolo = Data.Parametri.ProtocolloFascicoloIstanzaProgramma;
					break;
				case TipoProtocollo.Standard:
				default:
					protocolloCodiceTitolario = Data.Parametri.ProtocolloCodiceTitolarioIscrizione;
					protocolloUnitaResponsabile = Data.Parametri.ProtocolloUnitaResponsabileIscrizione;
					protocolloFascicolo = Data.Parametri.ProtocolloFascicoloIscrizione;
					break;
			}

			PROTOCOLLOEX_CREATO response =client.CREAPROTOCOLLOFULL(
				token,										//SESSIONE
				anno,										//ANNO
				Data.Parametri.ProtocolloTipo,				//TIPOPROTOCOLLO
				null,										//CODICEANAGRAFICA
				destinatario,								//CORRISPONDENTENOMINATIVO
				null,										//CORRISPONDENTEINDIRIZZO
				null,										//CORRISPONDENTECITTA
				null,										//CORRISPONDENTECAP
				null,										//CORRISPONDENTEPROVINCIA
				null,										//CORRISPONDENTEAZIENDA
				codiceDestinatario,							//CORRISPONDENTECODICEUNIVOCO
				null,										//CORRISPONDENTEORG
				pec,										//CORRISPONDENTEPEC
				null,										//CORRISPONDENTEEMAIL
				null,										//MULTIANAG
				oggetto,									//OGGETTO
				protocolloUnitaResponsabile,				//UNITAORGANIZZATIVARESPONSABILE
				Data.Parametri.ProtocolloTipoDocumento,     //TIPODOCUMENTO
				Data.Parametri.ProtocolloTipologia,			//TIPOLOGIA
				protocolloCodiceTitolario,					//CODICETITOLARIO
				null,										//ESTREMI
				null,										//DATAESTREMI
				null,										//PROTOCOLLORIFERIMENTO
				null,										//ALLEGATODESCRIZIONE
				nomeFile,									//ALLEGATONOMEFILE
				b64,										//ALLEGATOBASE64
				null,										//ALLEGATOPATHFULLFILE
				null,										//CODICEDEFAULT
				Data.Parametri.ProtocolloTipoAllegato,		//TIPOALLEGATO
				1											//NOTIFICA
			);

			//if (response.ESITO.Substring(0, 8) != "00000 - ")
			//{
			//	throw new Exception($"Errore nella richiesta delle credenziali: {response.ESITO}");
			//}

			return response;
		}


		public static string GetToken(){
			SIGED_AUTHSoapClient clientAtuenticazione = new SIGED_AUTHSoapClient(
		SIGED_AUTHSoapClient.EndpointConfiguration.SIGED_AUTHSoap,
		Data.Parametri.ProtocolloAutenticazioneServiceEndpoint);

			var responseAutenticazione = clientAtuenticazione.SWS_NEWSESSION(
				Data.Parametri.ProtocolloAuthorizationName,
				Data.Parametri.ProtocolloAuthorizationLastName,
				Data.Parametri.ProtocolloAuthorizationPassword
			);
			if (responseAutenticazione.Substring(0, 8) != "00000 - ")
			{
				throw new Exception($"Errore nella richiesta delle credenziali: {responseAutenticazione}");
			}
			string token = responseAutenticazione.Substring(8, responseAutenticazione.Length - 8);
			return token;
		}

	}
}
