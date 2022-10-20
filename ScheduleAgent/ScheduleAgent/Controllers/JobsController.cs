using RazorEngine;
using ScheduleAgent.Class;
using ScheduleAgent.Models;
using ScheduleAgent.Protocollo;
using Serilog;
using Serilog.Context;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Web.Mvc;
using Encoding = System.Text.Encoding;

namespace ScheduleAgent.Controllers
{
	public class JobsController : Controller
	{
		static string RenderViewToString(ControllerContext context,
									string viewPath,
									object model = null,
									bool partial = false)
		{
			// first find the ViewEngine for this view
			ViewEngineResult viewEngineResult;
			if (partial)
				viewEngineResult = ViewEngines.Engines.FindPartialView(context, viewPath);
			else
				viewEngineResult = ViewEngines.Engines.FindView(context, viewPath, null);

			if (viewEngineResult == null)
				throw new FileNotFoundException("View cannot be found.");

			// get the view and attach the model to view data
			var view = viewEngineResult.View;
			context.Controller.ViewData.Model = model;

			string result = null;

			using (var sw = new StringWriter())
			{
				var ctx = new ViewContext(context, view,
											context.Controller.ViewData,
											context.Controller.TempData,
											sw);
				view.Render(ctx, sw);
				result = sw.ToString();
			}

			return result;
		}

		private void StartThread(string template)
		{
			ApplicationGlobals.Instance.EmailDomandeAnnullateTemplate = template;
			ApplicationGlobals.Instance.RunningThreadGuid = Guid.NewGuid().ToString();
			ThreadManager cf = new ThreadManager(ApplicationGlobals.Instance.RunningThreadGuid);
			Thread thread = new Thread(cf.Process);
			thread.Start();
		}

		private void StopThread()
		{
			ApplicationGlobals.Instance.RunningThreadGuid = "";
		}

		public ActionResult ProtocollaDomaneOnline()
		{
			string template = System.IO.File.ReadAllText(HttpContext.Server.MapPath(@"~\Template\EmailDomandeAnnullate.cshtml"));
			StartThread(template);
			return RedirectToAction("Index","Home");
		}
		public ActionResult StopProtocollaDomaneOnline()
		{
			StopThread();
			return RedirectToAction("Index","Home");
		}

		public ActionResult Protocolla(int idPratica)
		{
			try
			{
				ProtocollaPratica(idPratica);
				return View();
			}
			catch (Exception e)
			{
				ViewData["Errore"] = e.Message;
				return View();
			}

		}

		public ActionResult Annulla(int idPratica)
		{
			try
			{
				AnnullaPratica(idPratica);
				return View();
			}
			catch (Exception e)
			{
				ViewData["Errore"] = e.Message;
				return View();
			}

		}
		private string AnnullaPratica(int idPratica)
		{
			Parameters parametri;
			try
			{
				parametri = new Parameters();
			}
			catch (Exception e)
			{
				string errore = "Errore nel recupero dei valori di configurazione";
				throw new Exception(errore, e);
			}
			string messaggio = "";
			/* Recupero token servizio protocollo*/
			string responseToken;
			using (var protocolloAutenticazioneService = new ProtocolloAutenticazione.SIGED_AUTHSoapClient())
			{
				responseToken = protocolloAutenticazioneService.SWS_NEWSESSION(
					parametri.ProtocolloAuthorizationName,
					parametri.ProtocolloAuthorizationLastName,
					parametri.ProtocolloAuthorizationPassword);
			}
			if (responseToken.Substring(0, 8) != "00000 - ")
			{
				string errore = $"Errore nella richiesta di credenziali: {responseToken}";
				throw new Exception(errore);
			}
			string token = responseToken.Substring(8, responseToken.Length - 8);
			List<string> errori = new List<string>();

			DomandaPresentata domanda;
			using (var heliosContext = new HeliosContext())
			using (var protocolloService = new SIGED_WSSoapClient())
			{
				domanda = heliosContext.DomandaPresentata
							.Where(
								x => x.Id == idPratica && 
								x.DataRichiestaAnnullamento!=null &&
								x.DataAnnullamento == null)
							.SingleOrDefault();
				if (domanda==null)
				{
					throw new Exception("Domanda inesistente");
				}
				using (LogContext.PushProperty("Domanda", domanda, true))
				{


					string nomeFile = $"{domanda.CodiceProgettoSelezionato}_{domanda.CodiceSedeSelezionata}_{Format.FormatFileName(domanda.CodiceFiscale)}.txt";
					string nomeFileCompleto = $"{parametri.ProtocolloFilePath}{nomeFile}";
					byte[] file = Encoding.UTF8.GetBytes($"Annullamento Domanda nr.{domanda.Id} Servizio civile - {domanda.CodiceProgettoSelezionato} – {domanda.CodiceSedeSelezionata} - {domanda.CodiceFiscale}");
					string fileBase64 = Convert.ToBase64String(file, 0, file.Length);

					try
					{
						System.IO.File.WriteAllText(nomeFileCompleto, $"Annullamento Domanda nr.{domanda.Id} Servizio civile - {domanda.CodiceProgettoSelezionato} – {domanda.CodiceSedeSelezionata} - {domanda.CodiceFiscale}");

					}
					catch (Exception e)
					{
						throw new Exception("Errore scrittura file su cartella di rete", e);
					}
					string oggetto = $"Annullamento Domanda nr.{domanda.Id} Servizio civile - {domanda.CodiceProgettoSelezionato} – {domanda.CodiceSedeSelezionata} - {domanda.CodiceFiscale}";

					if (domanda.DataFineProtocollazioneAnnullamento == null)
					{

						/* Registro inizio protocollazione */
						try
						{
							domanda.DataInizioProtocollazioneAnnullamento = DateTime.Now;
							heliosContext.SaveChanges();
						}
						catch (Exception e)
						{
							throw new Exception("Errore aggiornamento data inizio protocollazione", e);
						}
						Log.Debug("Aggiornamento data inizio protocollazione completata");

						string anno = DateTime.Today.Year.ToString();
						PROTOCOLLOEX_CREATO response;
						/* Protocollo */
						string mittente = $"{domanda.Nome} {domanda.Cognome} - {domanda.CodiceFiscale}";
						try
						{
							response = protocolloService.CREAPROTOCOLLOEXPRESS(
								token,//SESSIONE
								anno,//ANNO
								parametri.ProtocolloTipo,//TIPOPROTOCOLLO
								"",//CODICEANAGRAFICA
								mittente,//CORRISPONDENTENOMINATIVO
								"",//CORRISPONDENTEINDIRIZZO
								"",//CORRISPONDENTECITTA
								"",//CORRISPONDENTECAP
								"",//CORRISPONDENTEPROVINCIA
								"",//CORRISPONDENTEAZIENDA
								"",//CORRISPONDENTECODICEUNIVOCO
								null,//MULTIANAG
								oggetto,//OGGETTO
								parametri.ProtocolloUnitaResponsabile,//UNITAORGANIZZATIVARESPONSABILE
								parametri.ProtocolloCategoria,//TIPODOCUMENTO
								parametri.ProtocolloCodiceTitolario,//CODICETITOLARIO
								"",//ESTREMI
								"",//DATAESTREMI
								"",//PROTOCOLLORIFERIMENTO
								"",//ALLEGATODESCRIZIONE
								nomeFile,//ALLEGATONOMEFILE
								"",//ALLEGATOBASE64
								nomeFileCompleto,//ALLEGATOPATHFULLFILE
								"",//CODICEDEFAULT
								"",//TIPOALLEGATO
								"1"//MASSIVO
								);
						}
						catch (Exception e)
						{
							throw new Exception("Errore invocazione servizio di protocollo", e);
						}
						if (response.ESITO.Substring(0, 8) != "00000 - ")
						{
							Log.Error("Errore nella protocollazione: {esito}", response.ESITO);
							throw new Exception($"Errore nella protocollazione: { response.ESITO }");

						}

						string numeroProtocollo = $"{anno}.{response.NUMEROPROTOCOLLO.PadLeft(7, '0')}";

						/* Salvataggio dati protocollazione */
						try
						{
							domanda.NUMEROPROTOCOLLOAnnullamento = response.NUMEROPROTOCOLLO;
							domanda.CODICEPROTOCOLLOAnnullamento = response.CODICEPROTOCOLLO;
							domanda.DATAPROTOCOLLOAnnullamento = DateTime.Parse(response.DATAPROTOCOLLO);
							domanda.DataFineProtocollazioneAnnullamento = DateTime.Now;
							domanda.DataAnnullamento = DateTime.Now;

							heliosContext.SaveChanges();
						}
						catch (Exception e)
						{
							throw new Exception("Errore aggiornamento protocollazione nella domanda", e);
						}

						using (var dolContext = new DomandeOnlineContext())
						{
							
							dolContext.SaveChanges();
						}
						messaggio += "Domanda Protocollata\n";
					}



					if (domanda.DataInvioPECAnnullamento == null)
					{
						/* Invio Email Ricevuta */
						string oggettoEmail = $"Annullamento Domanda nr.{domanda.Id} Servizio civile - {domanda.CodiceProgettoSelezionato} – {domanda.CodiceSedeSelezionata}";
						string testoEmail = $"<h3 style=\"text - align: center; \">RICEVUTA &ndash; Annullamento domanda di partecipazione al Servizio civile</h3><p>La richiesta di annullamento della domanda nr. {domanda.Id} &egrave; stata presentata in data {domanda.DataRichiestaAnnullamento.Value.ToString("dd/MM/yyyy")} alle ore {domanda.DataRichiestaAnnullamento.Value.ToString("HH:mm:ss")} ed &egrave; stata protocollata con numero n. {domanda.NUMEROPROTOCOLLOAnnullamento} del {domanda.DATAPROTOCOLLOAnnullamento.Value.ToString("dd/MM/yyyy")}.</p><p>(È ora possibile presentare una nuova domanda)</p><p><br></p><p>	<br></p><p><strong>Non rispondere a questa casella.</strong></p><p>Per informazioni inviare una mail alla casella <a href=\"mailto: {parametri.IndirizzoRispostaMail}\">{parametri.IndirizzoRispostaMail}</a></p>";
						using (MailMessage message = new MailMessage(parametri.IndirizzoInvioMail, domanda.Email)
						{
							Subject = oggettoEmail,
							Body = testoEmail,
							IsBodyHtml = true
						})
						using (SmtpClient client = new SmtpClient(parametri.ServerSMTP)
						{
							UseDefaultCredentials = true
						})
						{
							try
							{
								//message.Attachments.Add(new Attachment(new MemoryStream(file), nomeFile));
								client.Send(message);
							}
							catch (Exception e)
							{
								throw new Exception("Errore nell'invio della mail", e);
							}
						}

						/* Salvataggio invio mail */
						try
						{
							domanda.DataInvioPEC = DateTime.Now;
							heliosContext.SaveChanges();

						}
						catch (Exception e)
						{
							throw new Exception("Errore nell'aggiornamento data invio mail", e);
						}

						/* Eliminazione file protocollato da cartella di rete*/
						System.IO.File.Delete(nomeFileCompleto);
						messaggio += "Email inviata\n";
					}


				}
			}
			return messaggio;
		}


		private string ProtocollaPratica(int idPratica)
		{
			Parameters parametri;
			try
			{
				parametri = new Parameters();
			}
			catch (Exception e)
			{
				string errore = "Errore nel recupero dei valori di configurazione";
				throw new Exception(errore, e);
			}
			string messaggio = "";
			/* Recupero token servizio protocollo*/
			string responseToken;
			using (var protocolloAutenticazioneService = new ProtocolloAutenticazione.SIGED_AUTHSoapClient())
			{
				responseToken = protocolloAutenticazioneService.SWS_NEWSESSION(
					parametri.ProtocolloAuthorizationName,
					parametri.ProtocolloAuthorizationLastName,
					parametri.ProtocolloAuthorizationPassword);
			}
			if (responseToken.Substring(0, 8) != "00000 - ")
			{
				string errore = $"Errore nella richiesta di credenziali: {responseToken}";
				throw new Exception(errore);
			}
			string token = responseToken.Substring(8, responseToken.Length - 8);
			List<string> errori = new List<string>();

			DomandaPresentata domanda;
			using (var heliosContext = new HeliosContext())
			using (var protocolloService = new SIGED_WSSoapClient())
			{
				domanda = heliosContext.DomandaPresentata
							.Where(x => x.Id == idPratica)
							.SingleOrDefault();
				using (LogContext.PushProperty("Domanda", domanda, true))
				{
					/* Salvataggio file su cartella di rete */
					byte[] file;
					try
					{
						using (var dolContext = new DomandeOnlineContext())
						{
							file = dolContext.DomandaPartecipazione
								.Where(x => x.Id == domanda.Id)
								.Select(x => x.FileDomanda)
								.SingleOrDefault();
						}

					}
					catch (Exception e)
					{
						throw new Exception("Errore recupero del file della domanda dal DB",e);
					}

					string nomeFile = $"{domanda.CodiceProgettoSelezionato}_{domanda.CodiceSedeSelezionata}_{Format.FormatFileName(domanda.CodiceFiscale)}.pdf";
					string nomeFileCompleto = $"{parametri.ProtocolloFilePath}{nomeFile}";
					string fileBase64 = Convert.ToBase64String(file, 0, file.Length);

					try
					{
						System.IO.File.WriteAllBytes(nomeFileCompleto, file);

					}
					catch (Exception e)
					{
						throw new Exception("Errore scrittura file su cartella di rete", e);
					}
					string oggetto = $"Domanda nr.{domanda.Id} Servizio civile - {domanda.CodiceProgettoSelezionato} – {domanda.CodiceSedeSelezionata} - {domanda.CodiceFiscale}";

					if (domanda.DataFineProtocollazione==null)
					{

						/* Registro inizio protocollazione */
						try
						{
							domanda.DataInizioProtocollazione = DateTime.Now;
							heliosContext.SaveChanges();
						}
						catch (Exception e)
						{
							throw new Exception("Errore aggiornamento data inizio protocollazione", e);
						}
						Log.Debug("Aggiornamento data inizio protocollazione completata");

						string anno = DateTime.Today.Year.ToString();
						PROTOCOLLOEX_CREATO response;
						/* Protocollo */
						try
						{
							response = protocolloService.CREAPROTOCOLLOEXPRESS(
								token,//SESSIONE
								anno,//ANNO
								parametri.ProtocolloTipo,//TIPOPROTOCOLLO
								"",//CODICEANAGRAFICA
								$"{domanda.Nome} {domanda.Cognome} - {domanda.CodiceFiscale}",//CORRISPONDENTENOMINATIVO
								"",//CORRISPONDENTEINDIRIZZO
								"",//CORRISPONDENTECITTA
								"",//CORRISPONDENTECAP
								"",//CORRISPONDENTEPROVINCIA
								"",//CORRISPONDENTEAZIENDA
								"",//CORRISPONDENTECODICEUNIVOCO
								null,//MULTIANAG
								oggetto,//OGGETTO
								parametri.ProtocolloUnitaResponsabile,//UNITAORGANIZZATIVARESPONSABILE
								parametri.ProtocolloCategoria,//TIPODOCUMENTO
								parametri.ProtocolloCodiceTitolario,//CODICETITOLARIO
								"",//ESTREMI
								"",//DATAESTREMI
								"",//PROTOCOLLORIFERIMENTO
								"",//ALLEGATODESCRIZIONE
								nomeFile,//ALLEGATONOMEFILE
								"",//ALLEGATOBASE64
								nomeFileCompleto,//ALLEGATOPATHFULLFILE
								"",//CODICEDEFAULT
								"",//TIPOALLEGATO
								"1"//MASSIVO
								);
						}
						catch (Exception e)
						{
							throw new Exception("Errore invocazione servizio di protocollo", e);
						}
						if (response.ESITO.Substring(0, 8) != "00000 - ")
						{
							Log.Error("Errore nella protocollazione: {esito}", response.ESITO);
							throw new Exception($"Errore nella protocollazione: { response.ESITO }");

						}

						string numeroProtocollo = $"{anno}.{response.NUMEROPROTOCOLLO.PadLeft(7, '0')}";

						/* Salvataggio dati protocollazione */
						try
						{
							domanda.NUMEROPROTOCOLLO = response.NUMEROPROTOCOLLO;
							domanda.CODICEPROTOCOLLO = response.CODICEPROTOCOLLO;
							domanda.DATAPROTOCOLLO = DateTime.Parse(response.DATAPROTOCOLLO);
							domanda.DataFineProtocollazione = DateTime.Now;
							heliosContext.SaveChanges();
						}
						catch (Exception e)
						{
							throw new Exception("Errore aggiornamento protocollazione nella domanda",e);
						}
						messaggio += "Domanda Protocollata\n";
					}



					if (domanda.DataInvioPEC == null)
					{
						/* Invio Email Ricevuta */
						string oggettoEmail = $"Ricevuta Domanda nr.{domanda.Id} Servizio civile - {domanda.CodiceProgettoSelezionato} – {domanda.CodiceSedeSelezionata}";
						string testoEmail = $"<h3 style=\"text - align: center; \">RICEVUTA &ndash; Domanda di partecipazione al Servizio civile</h3><p>La domanda nr. {domanda.Id} &egrave; stata presentata in data {domanda.DataPresentazione.Value.ToString("dd/MM/yyyy")} alle ore {domanda.DataPresentazione.Value.ToString("HH:mm:ss")} ed &egrave; stata protocollata con numero n. {domanda.NUMEROPROTOCOLLO} del {domanda.DATAPROTOCOLLO.Value.ToString("dd/MM/yyyy")}.</p><p>(Per la partecipazione al bando fanno fede data e orario di presentazione)</p><p><br></p><p>Per conoscere la data delle selezioni consulta il sito dell&rsquo;ente indicato nel bando per il progetto che hai scelto.</p><p>	<br></p><p><strong>Non rispondere a questa casella.</strong></p><p>Per informazioni inviare una mail alla casella <a href=\"mailto: {parametri.IndirizzoRispostaMail}\">{parametri.IndirizzoRispostaMail}</a></p>";
						using (MailMessage message = new MailMessage(parametri.IndirizzoInvioMail, domanda.Email)
						{
							Subject = oggettoEmail,
							Body = testoEmail,
							IsBodyHtml = true
						})
						using (SmtpClient client = new SmtpClient(parametri.ServerSMTP)
						{
							UseDefaultCredentials = true
						})
						{
							try
							{
								message.Attachments.Add(new Attachment(new MemoryStream(file), nomeFile));
								client.Send(message);
							}
							catch (Exception e)
							{
								throw new Exception("Errore nell'invio della mail",e);
							}
						}

						/* Salvataggio invio mail */
						try
						{
							domanda.DataInvioPEC = DateTime.Now;
							heliosContext.SaveChanges();

						}
						catch (Exception e)
						{
							throw new Exception("Errore nell'aggiornamento data invio mail",e);
						}

						/* Eliminazione file protocollato da cartella di rete*/
						System.IO.File.Delete(nomeFileCompleto);
						messaggio += "Email inviata\n";
					}


				}
			}
			return messaggio;
		}


		public static ThreadManager.JobResponse ProtocollaJob() {
			using (LogContext.PushProperty("Job", MethodBase.GetCurrentMethod().Name))
			{

				int numeroDomandeDaProtocollare;
				int numeroDomandeProtocollate;
				/* Recupero valori configurazione */
				Parameters parametri;
				try
				{
					parametri = new Parameters();
				}
				catch (Exception e)
				{
					string errore = "Errore nel recupero dei valori di configurazione";
					throw new Exception(errore,e);
				}

				try
				{
					/* Aggiornamento Dati Domande Online*/
					using (var domandeOnlineContext = new DomandeOnlineContext())
					{
						domandeOnlineContext.Database.CommandTimeout = 180;
						domandeOnlineContext.SP_AggiornamentoDati();
						domandeOnlineContext.SaveChanges();
					}
				}
				catch (Exception e)
				{
					string errore = "Errore nell'esecuzione della Store procedure per l'aggiornamento dei dati da Helios a DOL";
					throw new Exception(errore, e);
				}


				try
				{
					/* Aggiornamento Dati Domande Online*/
					using (var heliosContext = new HeliosContext())

					{
						heliosContext.Database.CommandTimeout = 180;
						heliosContext.SP_DOL_IMPORTAZIONE_DOMANDE();
						heliosContext.SaveChanges();
					}
				}
				catch (Exception e)
				{
					string errore = "Errore nell'esecuzione della Store procedure per l'aggiornamento dei dati Da DOL a Helios";
					throw new Exception(errore, e);
				}

				/* Invio mail Domande annullate */
				string testoEmail;
				int bando;
				using (var domandeOnlineContext = new DomandeOnlineContext())
				{
					bando = domandeOnlineContext.Bando.Max(b => b.Gruppo);
					IQueryable<DomandaPartecipazione> domandeAnnullate;
					int numeroDomande;
					try
					{
						
						domandeAnnullate = domandeOnlineContext.DomandaPartecipazione
							.Where(	x => x.DataAnnullamento != null && 
									x.DataRichiestaAnnullamento==null && x.DataInvioEmailAnnullamento == null &&
									x.GruppoBando==bando);

						numeroDomande = domandeAnnullate.Count();
						using (JobsController controller = new JobsController())
						{
							testoEmail = Razor.Parse(ApplicationGlobals.Instance.EmailDomandeAnnullateTemplate,domandeAnnullate);
							//var key = new RazorEngine.Templating.NameOnlyTemplateKey("EmailTemplate", RazorEngine.Templating.ResolveType.Global, null);
							//RazorEngine.Engine.Razor.AddTemplate(key, new RazorEngine.Templating.LoadedTemplateSource(ApplicationGlobals.Instance.EmailDomandeAnnullateTemplate));
							//StringBuilder sb = new StringBuilder();
							//StringWriter sw = new StringWriter(sb);
							//RazorEngine.Engine.Razor.RunCompile(key, sw, domandeAnnullate.GetType(), domandeAnnullate);
							//string s = sb.ToString();
							//RenderViewToString(ApplicationGlobals.Instance., "EmailDomandeAnnullate", domandeAnnullate, true);
						}
					}
					catch (Exception e)
					{
						string errore = "Errore nell'interrogazione al DB per recuperare le domande annullate";
						throw new Exception(errore, e);
					}
					if (numeroDomande > 0)
					{

						string oggettoEmail = $"Domanda Online - Annullamento domande";
						using (MailMessage message = new MailMessage(parametri.IndirizzoInvioMail, parametri.IndirizzoMailDomandeAnnullate)
						{
							Subject = oggettoEmail,
							Body = testoEmail,
							IsBodyHtml = true
						})
						using (SmtpClient client = new SmtpClient(parametri.ServerSMTP)
						{
							UseDefaultCredentials = true
						})
						{
							try
							{
								client.Send(message);
							}
							catch (Exception e)
							{
								string errore = "Errore nell'invio della mail ";
								Log.Error(e, errore);
							}
						}

						Log.Debug("Invio email completato");

						/*Salvataggio invio mail */
						try
						{
							using (var heliosContext = new HeliosContext()) {
								foreach (DomandaPartecipazione domanda in domandeAnnullate)
								{
									domanda.DataInvioEmailAnnullamento = DateTime.Now;
									DomandaPresentata domandaHelios = heliosContext.DomandaPresentata
										.Where(x => x.Id == domanda.Id)
										.SingleOrDefault();
									if (domandaHelios!=null)
									{
										domandaHelios.DataAnnullamento = domanda.DataAnnullamento;
										heliosContext.SaveChanges();
									}
								}
								domandeOnlineContext.SaveChanges();
							}
						}
						catch (Exception e)
						{
							string errore = "Errore nell'aggiornamento data invio mail";
							Log.Error(e, errore);
						}
						Log.Debug("Aggiornamento data invio email annullate");
					}
				}


				/* Recupero token servizio protocollo*/
				string responseToken;
				using (var protocolloAutenticazioneService = new ProtocolloAutenticazione.SIGED_AUTHSoapClient())
				{
					responseToken = protocolloAutenticazioneService.SWS_NEWSESSION(
						parametri.ProtocolloAuthorizationName,
						parametri.ProtocolloAuthorizationLastName,
						parametri.ProtocolloAuthorizationPassword);
				}
				if (responseToken.Substring(0, 8) != "00000 - ")
				{
					string errore= $"Errore nella richiesta di credenziali: {responseToken}";
					throw new Exception(errore);
				}
				string token = responseToken.Substring(8, responseToken.Length - 8);
				List<string> errori = new List<string>();

				using (var heliosContext = new HeliosContext())
				using (var protocolloService = new Protocollo.SIGED_WSSoapClient())
				{
					IEnumerable<DomandaPresentata> domandeDaProtocollare;

					/* Recupero domande da protocollare */
					try
					{
						domandeDaProtocollare = heliosContext.DomandaPresentata
							.Where(x => x.DataInvioPEC == null &&
								x.GruppoBando==bando
								)
							.OrderBy(x => x.DataPresentazione)
							.ToList();
						numeroDomandeDaProtocollare = domandeDaProtocollare.Count();
						numeroDomandeProtocollate = 0;
						if (numeroDomandeDaProtocollare == 0)
						{
							Log.Information("Nessuna nuova domanda da protocollare");
							//return new ThreadManager.JobResponse()
							//{
							//	Success = true,
							//	Message = "Non è stata trovata nessuna nuova domanda da protocollare"
							//};
						}
						Log.Information($"Inizio protocollazione. Ci sono {numeroDomandeDaProtocollare} domande da protocollare", numeroDomandeDaProtocollare);
					}
					catch (Exception e)
					{
						string errore = "Errore nella lettura delle domande da protocollare dal DB";
						throw new Exception(errore, e);
					}
					int c = 0;
					/* Inizio ciclo sulle domande*/
					foreach (DomandaPresentata domanda in domandeDaProtocollare)
					{
						using (LogContext.PushProperty("Domanda", domanda, true))
						{
							c++;
							Log.Debug("Inizio Ciclo {c}", c);
							/* Salvataggio file su cartella di rete */
							string urlSito;
							byte[] file;
							try
							{
								using (var dolContext = new DomandeOnlineContext())
								{
									file = dolContext.DomandaPartecipazione
										.Where(x => x.Id == domanda.Id)
										.Select(x => x.FileDomanda)
										.SingleOrDefault();
									urlSito = dolContext.DomandaPartecipazione
										.Where(x => x.Id == domanda.Id)
										.Select(x => x.Progetto.Sito)
										.FirstOrDefault();

								}

							}
							catch (Exception e)
							{
								string errore = "Errore recupero del file della domanda dal DB";
								Log.Error(e, errore);
								errori.Add(errore);
								continue;
							}
							string nomeFile = $"{domanda.CodiceProgettoSelezionato}_{domanda.CodiceSedeSelezionata}_{Format.FormatFileName(domanda.CodiceFiscale)}.pdf";
							if (domanda.DataInizioProtocollazione == null)
							{
								Log.Debug("Recuperato file domanda {nomefile}", nomeFile);
								string nomeFileCompleto = $"{parametri.ProtocolloFilePath}{nomeFile}";
								string fileBase64 = Convert.ToBase64String(file, 0, file.Length);

								try
								{
									System.IO.File.WriteAllBytes(nomeFileCompleto, file);

								}
								catch (Exception e)
								{
									string errore = "Errore scrittura file su cartella di rete";
									Log.Error(e, errore);
									errori.Add(errore);
									continue;
								}
								Log.Debug("Scrittura File su cartella di rete completata: {nomeFileCompleto}", nomeFileCompleto);


								string oggetto = $"Domanda nr.{domanda.Id} Servizio civile - {domanda.CodiceProgettoSelezionato} – {domanda.CodiceSedeSelezionata}";

								/* Registro inizio protocollazione */
								try
								{
									domanda.DataInizioProtocollazione = DateTime.Now;
									heliosContext.SaveChanges();
								}
								catch (Exception e)
								{
									string errore = "Errore aggiornamento data inizio protocollazione";
									Log.Error(e, errore);
									errori.Add(errore);
									continue;
								}
								Log.Debug("Aggiornamento data inizio protocollazione completata");

								string anno = DateTime.Today.Year.ToString();
								PROTOCOLLOEX_CREATO response;
								/* Protocollo */
								try
								{
									response = protocolloService.CREAPROTOCOLLOEXPRESS(
										token,//SESSIONE
										anno,//ANNO
										parametri.ProtocolloTipo,//TIPOPROTOCOLLO
										"",//CODICEANAGRAFICA
										$"{domanda.Nome} {domanda.Cognome}",//CORRISPONDENTENOMINATIVO
										"",//CORRISPONDENTEINDIRIZZO
										"",//CORRISPONDENTECITTA
										"",//CORRISPONDENTECAP
										"",//CORRISPONDENTEPROVINCIA
										"",//CORRISPONDENTEAZIENDA
										"",//CORRISPONDENTECODICEUNIVOCO
										null,//MULTIANAG
										oggetto,//OGGETTO
										parametri.ProtocolloUnitaResponsabile,//UNITAORGANIZZATIVARESPONSABILE
										parametri.ProtocolloCategoria,//TIPODOCUMENTO
										parametri.ProtocolloCodiceTitolario,//CODICETITOLARIO
										"",//ESTREMI
										"",//DATAESTREMI
										"",//PROTOCOLLORIFERIMENTO
										"",//ALLEGATODESCRIZIONE
										nomeFile,//ALLEGATONOMEFILE
										"",//ALLEGATOBASE64
										nomeFileCompleto,//ALLEGATOPATHFULLFILE
										"",//CODICEDEFAULT
										"",//TIPOALLEGATO
										"1"//MASSIVO
										);
								}
								catch (Exception e)
								{
									string errore = "Errore invocazione servizio di protocollo";
									Log.Error(e, errore);
									errori.Add(errore);
									continue;
								}
								if (response.ESITO.Substring(0, 8) != "00000 - ")
								{
									Log.Error("Errore nella protocollazione: {esito}", response.ESITO);
									errori.Add($"Errore invocazione servizio di protocollo: {response.ESITO}");
									continue;
								}
								Log.Debug("Protocollazione Completata {@esito}", response);

								string numeroProtocollo = $"{anno}.{response.NUMEROPROTOCOLLO.PadLeft(7, '0')}";

								/* Salvataggio dati protocollazione */
								try
								{
									domanda.NUMEROPROTOCOLLO = response.NUMEROPROTOCOLLO;
									domanda.CODICEPROTOCOLLO = response.CODICEPROTOCOLLO;
									domanda.DATAPROTOCOLLO = DateTime.Parse(response.DATAPROTOCOLLO);
									domanda.DataFineProtocollazione = DateTime.Now;
									heliosContext.SaveChanges();
								}
								catch (Exception e)
								{
									string errore = "Errore aggiornamento protocollazione nella domanda";
									Log.Error(e, errore);
									errori.Add(errore);
									continue;
								}
								Log.Debug("Aggiornamento protocollazione nella domanda completato");


								/* Eliminazione file protocollato da cartella di rete*/
								System.IO.File.Delete(nomeFileCompleto);

							}
							/* Invio Email Ricevuta */
							string oggettoEmail = $"Ricevuta Domanda nr.{domanda.Id} Servizio civile - {domanda.CodiceProgettoSelezionato} – {domanda.CodiceSedeSelezionata}";
							testoEmail =	$"<h3 style=\"text - align: center; \">RICEVUTA &ndash; Domanda di partecipazione al Servizio civile</h3>" +
											$"<p>La domanda nr. {domanda.Id} &egrave; stata presentata in data {domanda.DataPresentazione.Value.ToString("dd/MM/yyyy")} " +
												$"alle ore {domanda.DataPresentazione.Value.ToString("HH:mm:ss")} " +
												$"ed &egrave; stata protocollata con numero n. {domanda.NUMEROPROTOCOLLO} " +
												$"del {domanda.DATAPROTOCOLLO.Value.ToString("dd/MM/yyyy")}.</p>" +
											$"<p>(Per la partecipazione al bando fanno fede data e orario di presentazione)</p><p><br></p>" +
											$"<p>Per conoscere la data delle selezioni consulta il sito " +
												$"<a href=\"{urlSito}\" target=\"_blank\" title=\"Vai al sito(Si apre in un'altra finestra)\">" +
													$"{urlSito}</a>." +
												$"</p><p>	<br></p>" +
											$"<p><strong>Non rispondere a questa casella.</strong></p>" +
											$"<p>Per informazioni inviare una mail alla casella " +
												$"<a href=\"mailto: {parametri.IndirizzoRispostaMail}\">{parametri.IndirizzoRispostaMail}</a></p>";
							using (MailMessage message = new MailMessage(parametri.IndirizzoInvioMail, domanda.Email)
							{
								Subject = oggettoEmail,
								Body = testoEmail,
								IsBodyHtml = true
							})
							using (SmtpClient client = new SmtpClient(parametri.ServerSMTP)
							{
								UseDefaultCredentials = true
							})
							{
								try
								{
									message.Attachments.Add(new Attachment(new MemoryStream(file), nomeFile));
									client.Send(message);
								}
								catch (Exception e)
								{
									string errore = "Errore nell'invio della mail";
									Log.Error(e, errore);
									errori.Add(errore);
									continue;
								}
							}
							Log.Debug("Invio email completato");

							/* Salvataggio invio mail */
							try
							{
								domanda.DataInvioPEC = DateTime.Now;
								heliosContext.SaveChanges();

							}
							catch (Exception e)
							{
								string errore = "Errore nell'aggiornamento data invio PEC";
								Log.Error(e, errore);
								errori.Add(errore);
								continue;
							}
							Log.Debug("Aggiornamento data invio PEC Completata");


							Log.Debug("Eliminazione file protocollato completata");

							numeroDomandeProtocollate++;
						}
					}
				}
				string messaggio = $"Sono state protocollate {numeroDomandeProtocollate} domande su {numeroDomandeDaProtocollare}";
				Log.Information($"Fine Protocollazione. {messaggio}");

				// Annullamento domande
				using (var domandeOnlineContext = new DomandeOnlineContext())
				using (var heliosContext = new HeliosContext())
				using (var protocolloService = new Protocollo.SIGED_WSSoapClient())
				{
					// Domande da annullare (richieste di annullamento)
					List<DomandaPresentata> domandeDaAnnullare;
					try
					{
						domandeDaAnnullare = heliosContext.DomandaPresentata
							.Where(x => x.DataRichiestaAnnullamento != null &&
								 x.DataInvioPECAnnullamento == null)
							.ToList();
					}
					catch (Exception e)
					{
						string errore = "Errore nell'interrogazione al DB per recuperare le richieste di annullamento annullate";
						throw new Exception(errore, e);
					}
					int numeroDomandeDaAnnullare = domandeDaAnnullare.Count();
					int numeroDomandeAnnullate= 0;
					try
					{
						foreach (DomandaPresentata domanda in domandeDaAnnullare)
						{
							using (LogContext.PushProperty("Domanda", domanda, true))
							{


								if (domanda.DataFineProtocollazioneAnnullamento == null)
								{

									string nomeFile = $"{domanda.CodiceProgettoSelezionato}_{domanda.CodiceSedeSelezionata}_{Format.FormatFileName(domanda.CodiceFiscale)}.txt";
									string nomeFileCompleto = $"{parametri.ProtocolloFilePath}{nomeFile}";
									byte[] file = Encoding.UTF8.GetBytes($"Annullamento Domanda nr.{domanda.Id} Servizio civile - {domanda.CodiceProgettoSelezionato} – {domanda.CodiceSedeSelezionata} - {domanda.CodiceFiscale}");

									string fileBase64 = Convert.ToBase64String(file, 0, file.Length);

									try
									{
										System.IO.File.WriteAllText(nomeFileCompleto, $"Annullamento Domanda nr.{domanda.Id} Servizio civile - {domanda.CodiceProgettoSelezionato} – {domanda.CodiceSedeSelezionata} - {domanda.CodiceFiscale}");

									}
									catch (Exception e)
									{
										throw new Exception("Errore scrittura file su cartella di rete", e);
									}
									string oggetto = $"Annullamento Domanda nr.{domanda.Id} Servizio civile - {domanda.CodiceProgettoSelezionato} – {domanda.CodiceSedeSelezionata} - {domanda.CodiceFiscale}";


									/* Registro inizio protocollazione */
									try
									{
										domanda.DataInizioProtocollazioneAnnullamento = DateTime.Now;
										heliosContext.SaveChanges();
									}
									catch (Exception e)
									{
										throw new Exception("Errore aggiornamento data inizio protocollazione", e);
									}
									Log.Debug("Aggiornamento data inizio protocollazione completata");

									string anno = DateTime.Today.Year.ToString();
									PROTOCOLLOEX_CREATO response;
									/* Protocollo */
									string mittente = $"{domanda.Nome} {domanda.Cognome} - {domanda.CodiceFiscale}";
									try
									{
										response = protocolloService.CREAPROTOCOLLOEXPRESS(
											token,//SESSIONE
											anno,//ANNO
											parametri.ProtocolloTipo,//TIPOPROTOCOLLO
											"",//CODICEANAGRAFICA
											mittente,//CORRISPONDENTENOMINATIVO
											"",//CORRISPONDENTEINDIRIZZO
											"",//CORRISPONDENTECITTA
											"",//CORRISPONDENTECAP
											"",//CORRISPONDENTEPROVINCIA
											"",//CORRISPONDENTEAZIENDA
											"",//CORRISPONDENTECODICEUNIVOCO
											null,//MULTIANAG
											oggetto,//OGGETTO
											parametri.ProtocolloUnitaResponsabile,//UNITAORGANIZZATIVARESPONSABILE
											parametri.ProtocolloCategoria,//TIPODOCUMENTO
											parametri.ProtocolloCodiceTitolario,//CODICETITOLARIO
											"",//ESTREMI
											"",//DATAESTREMI
											"",//PROTOCOLLORIFERIMENTO
											"",//ALLEGATODESCRIZIONE
											nomeFile,//ALLEGATONOMEFILE
											"",//ALLEGATOBASE64
											nomeFileCompleto,//ALLEGATOPATHFULLFILE
											"",//CODICEDEFAULT
											"",//TIPOALLEGATO
											"1"//MASSIVO
											);
									}
									catch (Exception e)
									{
										string errore = "Errore invocazione servizio di protocollo per annullamento";
										Log.Error(e, errore);
										errori.Add(errore);
										continue;
									}
									if (response.ESITO.Substring(0, 8) != "00000 - ")
									{
										Log.Error("Errore nella protocollazione annullamento: {esito}", response.ESITO);
										errori.Add($"Errore nella protocollazione annullamento: { response.ESITO }");
										continue;
									}
									System.IO.File.Delete(nomeFileCompleto);

									string numeroProtocollo = $"{anno}.{response.NUMEROPROTOCOLLO.PadLeft(7, '0')}";

									/* Salvataggio dati protocollazione */
									try
									{
										domanda.NUMEROPROTOCOLLOAnnullamento = response.NUMEROPROTOCOLLO;
										domanda.CODICEPROTOCOLLOAnnullamento = response.CODICEPROTOCOLLO;
										domanda.DATAPROTOCOLLOAnnullamento = DateTime.Parse(response.DATAPROTOCOLLO);
										domanda.DataFineProtocollazioneAnnullamento = DateTime.Now;

										heliosContext.SaveChanges();
									}
									catch (Exception e)
									{
										throw new Exception("Errore aggiornamento protocollazione nella domanda", e);
									}


								}
								/* Eliminazione file protocollato da cartella di rete*/

								if (domanda.DataFineProtocollazioneAnnullamento != null && domanda.DataAnnullamento == null)
								{
									try
									{
										domandeOnlineContext.SP_AnnullaDomanda(domanda.Id);
										domandeOnlineContext.SaveChanges();
										domanda.DataAnnullamento = DateTime.Now;
										heliosContext.SaveChanges();

									}
									catch (Exception e)
									{
										string errore = "Errore nell'annullamento della domanda";
										Log.Error(e, errore);
										errori.Add(errore);
										continue;
									}

								}
								if (domanda.DataInvioPECAnnullamento == null)
								{
									/* Invio Email Ricevuta */
									string oggettoEmail = $"Annullamento Domanda nr.{domanda.Id} Servizio civile - {domanda.CodiceProgettoSelezionato} – {domanda.CodiceSedeSelezionata}";
									testoEmail = $"<h3 style=\"text - align: center; \">RICEVUTA &ndash; Annullamento domanda di partecipazione al Servizio civile</h3><p>La informiamo che la domanda nr. {domanda.Id} &egrave; stata annullata in data {domanda.DataAnnullamento.Value.ToString("dd/MM/yyyy")} alle ore {domanda.DataAnnullamento.Value.ToString("HH:mm:ss")} con documento n. {domanda.NUMEROPROTOCOLLOAnnullamento} del {domanda.DATAPROTOCOLLOAnnullamento.Value.ToString("dd/MM/yyyy")}.</p><p>(È ora possibile presentare una nuova domanda)</p><p><br></p><p>	<br></p><p><strong>Non rispondere a questa casella.</strong></p><p>Per informazioni inviare una mail alla casella <a href=\"mailto: {parametri.IndirizzoRispostaMail}\">{parametri.IndirizzoRispostaMail}</a></p>";
									using (MailMessage message = new MailMessage(parametri.IndirizzoInvioMail, domanda.Email)
									{
										Subject = oggettoEmail,
										Body = testoEmail,
										IsBodyHtml = true
									})
									using (SmtpClient client = new SmtpClient(parametri.ServerSMTP)
									{
										UseDefaultCredentials = true
									})
									{
										try
										{
											//message.Attachments.Add(new Attachment(new MemoryStream(file), nomeFile));
											client.Send(message);

										}
										catch (Exception e)
										{
											string errore = "Errore nell'invio della mail di annullamento";
											Log.Error(errore, e);
											errori.Add(errore);
											continue;
										}
										/* Salvataggio invio mail */
										try
										{
											domanda.DataInvioPECAnnullamento = DateTime.Now;
											heliosContext.SaveChanges();
											numeroDomandeAnnullate++;
										}
										catch (Exception e)
										{
											string errore = "Errore nell'aggiornamento data invio mail di annullamento";
											Log.Error(errore, e);
											errori.Add(errore);
										}
									}

								}


							}

						}
					}
					catch (Exception e)
					{
						string errore = "Errore nel processo delle richieste di annullamento";
						throw new Exception(errore, e);
					}
					messaggio += $"<br/>Sono state annullate {numeroDomandeAnnullate} domande su {numeroDomandeDaAnnullare}";

				}

				return new ThreadManager.JobResponse()
				{
					Success = errori.Count == 0,
					Message = messaggio,
					Errors = errori
				};

			}
		}
	}
}