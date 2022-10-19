using Logger.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using PdfSharp;
using ProtocolloService;
using RegistrazioneSistemaUnico.Data;
using RegistrazioneSistemaUnico.Helpers;
using RegistrazioneSistemaUnico.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Transactions;
using TheArtOfDev.HtmlRenderer.PdfSharp;

namespace RegistrazioneSistemaUnico.Controllers
{
	public class HomeController : SmartController
	{

		private IViewRenderService renderService;
		private IHostingEnvironment Environment;
		private IConfiguration Configuration;
		public HomeController(RegistrazioneContext registrazioneContext, IViewRenderService renderService, IHostingEnvironment env, IConfiguration configuration) : base(registrazioneContext)
		{
			this.renderService = renderService;
			this.Environment = env;
			this.Configuration = configuration;
		}


		public IActionResult LoginSpid(string token)
		{
			return View();
		}

		public IActionResult Index()
		{
			if (User.Identity.IsAuthenticated)
			{
				return RedirectToAction("Index","Accesso");
			}
			return View();
		}

		public IActionResult Privacy()
		{
			//Build the File Path.
			string path = Path.Combine(this.Environment.ContentRootPath, "Documenti/") + "Privacy.pdf";

			//Read the File data into Byte Array.
			byte[] bytes = System.IO.File.ReadAllBytes(path);
			return File(bytes, "application/pdf","Privacy.pdf");
		}


		public IActionResult LoadParameters(){
			Parametri.CaricaParametri(Configuration);
			return Json(new { result = "Ok"});
		}

		public static Registrazione Locker = new Registrazione();
		public IActionResult Protocollo()
		{
			if (string.IsNullOrEmpty(Parametri.ProtocolloAutenticazioneServiceEndpoint))
				return NotFound();
			DateTime datainizio = DateTime.Now;
			Mutex mut = new Mutex(false,"Protocollazione");
			try
			{
				if (mut.WaitOne(0, false))
				{
					Locker.DataInserimento = DateTime.Now;
					string token = Protocollazione.GetToken();

					/* Registrazioni */
					List<Registrazione> registrazioniDaProtocollare = context.Registrazione
						.Where(r => r.DataInvioEmail == null)
						.ToList();
					foreach (Registrazione registrazione in registrazioniDaProtocollare)
					{
						try
						{
							ProtocollaRegistrazione(registrazione,token);
						}
						catch (Exception ex)
						{
							Log.Error(LogEvent.ERRORE_PROTOCOLLO, exception: ex);
						}
					}

					List<ProtocolloPresentazione> protocolliDaEffettuare = context.ProtocolloPresentazione
						.Where(x => x.DataInvioEmail == null)
						.ToList();
					foreach (ProtocolloPresentazione protocollo in protocolliDaEffettuare)
					{
						try
						{
							ProtocollaDomande(protocollo, token);
						}
						catch (Exception ex)
						{
							Log.Error(LogEvent.ERRORE_PROTOCOLLO, exception: ex);
						}

					}

					List<ProtocolloAntimafia> protocolliAntimafiaDaEffettuare = context.ProtocolloAntimafia
						.Where(x => x.DataInvioEmail == null)
						.ToList();
					foreach (ProtocolloAntimafia protocollo in protocolliAntimafiaDaEffettuare)
					{
						try
						{
							ProtocollaAntimafia(protocollo, token);
						}
						catch (Exception ex)
						{
							Log.Error(LogEvent.ERRORE_PROTOCOLLO, exception: ex);
						}

					}
					List<ProtocolloOLP> protocolliOLPDaEffettuare = context.ProtocolloOLP
						.Where(x => x.DataInvioEmail == null)
						.ToList();
					foreach (ProtocolloOLP protocollo in protocolliOLPDaEffettuare)
					{
						try
						{
							ProtocollaDomandeOLP(protocollo, token);
						}
						catch (Exception ex)
						{
							Log.Error(LogEvent.ERRORE_PROTOCOLLO, exception: ex);
						}
					}

					List<ProtocolloTUTORAGGIO> protocolliTUTORAGGIODaEffettuare = context.ProtocolloTUTORAGGIO
						.Where(x => x.DataInvioEmail == null)
						.ToList();
					foreach (ProtocolloTUTORAGGIO protocollo in protocolliTUTORAGGIODaEffettuare)
					{
						try
						{
							ProtocollaDomandeTUTORAGGIO(protocollo, token);
						}
						catch (Exception ex)
						{
							Log.Error(LogEvent.ERRORE_PROTOCOLLO, exception: ex);
						}
					}

					List<ProtocolloProgramma> protocolliProgettiDaEffettuare = context.ProtocolloProgramma
						.Where(x => x.DataInvioEmail == null)
						.ToList();
					foreach (ProtocolloProgramma protocollo in protocolliProgettiDaEffettuare)
					{
						try
						{
							ProtocollaDomandeProgrammi(protocollo, token);
						}
						catch (Exception ex)
						{
							Log.Error(LogEvent.ERRORE_PROTOCOLLO, exception: ex);
						}
					}
					List<ProtocolloProgramma> protocolliProgettiDaAnnullare = context.ProtocolloProgramma
						.Where(x => x.DataInvioEmail != null && x.DataAnnullamento!=null && x.DataInvioEmailAnnullamento == null)
						.ToList();
					foreach (ProtocolloProgramma protocollo in protocolliProgettiDaAnnullare)
					{
						try
						{
							ProtocollaAnnullaDomandeProgrammi(protocollo, token);
						}
						catch (Exception ex)
						{
							Log.Error(LogEvent.ERRORE_PROTOCOLLO, exception: ex);
						}
					}
					return Json(new { result = "Ok", TimeStart=datainizio ,TimeCompleted = DateTime.Now });
				}
				else
				{
					return Json(new { status="già Avviato", TimeStart = datainizio, TimeCompleted = DateTime.Now });
				}
			}
			finally
			{
				mut.Close();
			}

		}

		private void ProtocollaDomandeProgrammi(ProtocolloProgramma protocollo, string token)
		{
			DatiDomandaProgramma datiDomanda = context.DatiDomandaProgramma
				.FirstOrDefault(x => x.Id == protocollo.Id);
			//Entità per i log
			Entity entity = new Entity()
			{
				Id = protocollo.Id,
				Name = "ProtocolloProgramma"
			};
			Log.SetEnte(datiDomanda.CodiceFiscaleEnte);
			DateTime? dataProtocollo = null;
			string numeroProtocollo = protocollo.NumeroProtocollo;
			if (protocollo.DataProtocollazione == null)
			{
				string anno = DateTime.Today.Year.ToString();
				PROTOCOLLOEX_CREATO responseProtocollo;
				string allegatonomefile = datiDomanda.FileName;
				//string oggetto = "ALB#SCU#ISTANZAPROGRAMMA " + datiDomanda.Denominazione;
				string oggetto = $"{Parametri.ProtocolloOggettarioIstanzaProgramma} {datiDomanda.Denominazione}";
				string fileBase64 = Convert.ToBase64String(datiDomanda.BinData, 0, datiDomanda.BinData.Length);

				try
				{
					responseProtocollo = Protocollazione.InviaProtocollo(
						token,
						oggetto,
						null,
						datiDomanda.Denominazione,
						datiDomanda.CodiceRegione,
						datiDomanda.PEC,
						datiDomanda.FileName,
						datiDomanda.BinData,
						Protocollazione.TipoProtocollo.IstanzaPaogramma
					);
				}
				catch (Exception e)
				{
					Log.Error(LogEvent.ERRORE_PROTOCOLLO, "Connessione Servizio Protocollo", e, entity: entity);
					throw e;
				}
				if (responseProtocollo.ESITO.Substring(0, 8) != "00000 - ")
				{
					Exception e = new Exception($"Risposta Protocollo: {responseProtocollo.ESITO}");
					Log.Error(LogEvent.ERRORE_PROTOCOLLO, "Risposta Servizio", e, entity: entity);
					throw e;
				}
				DateTime date;
				if (DateTime.TryParseExact(responseProtocollo.DATAPROTOCOLLO, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
				{
					dataProtocollo = date;
				}
				protocollo.DataProtocollazione = DateTime.Now;
				protocollo.DataProtocollo = dataProtocollo;
				protocollo.NumeroProtocollo = responseProtocollo.NUMEROPROTOCOLLO;
				numeroProtocollo = protocollo.NumeroProtocollo;

				context.SaveChanges();
			}
			if (protocollo.DataProtocollo == null || string.IsNullOrEmpty(protocollo.NumeroProtocollo))
			{
				//Faccio finta di aver inviato la mail
				protocollo.DataInvioEmail = DateTime.Now;
				context.SaveChanges();
				Log.Error(LogEvent.ERRORE_PROTOCOLLO, "Dati risposta non valorizzati", parameters: new { protocollo.DataProtocollo, protocollo.NumeroProtocollo });
			}
			else
			{
				string destinatari;
				if (string.IsNullOrEmpty(Parametri.OverrideEmail))
				{
					destinatari = $"{datiDomanda.Email}";
				}
				else
				{
					destinatari = Parametri.OverrideEmail;
				}
				using (MailMessage mail = new MailMessage(Parametri.ServerSMTPMailFrom, destinatari))
				using (SmtpClient client = new SmtpClient(Parametri.ServerSMTPEndpoint))
				{
					string testoEmail;
					string oggettoEmail;
					testoEmail = $"<p>Gentile Utente, il Dipartimento ha ricevuto sul sistema Helios l’istanza di presentazione programmi relativa all’avviso \"{datiDomanda.Bando}\" per l’ente {datiDomanda.CodiceRegione} – {datiDomanda.Denominazione}.</p>" +
					$"<p>L’istanza di presentazione è stata presentata a sistema in data {datiDomanda.DataPresentazioneSistema?.ToString("dd/MM/yyyy")} ed ora {datiDomanda.DataPresentazioneSistema?.ToString("HH:mm:ss")} e protocollata il {protocollo.DataProtocollo?.ToString("dd/MM/yyyy")} con protocollo n.{protocollo.NumeroProtocollo}.</p>" +
					$"<p>Copia dell’istanza di presentazione è allegata a questa e-mail.</p>" +
					$"<p>La presente e-mail è stata generata automaticamente da un indirizzo di posta elettronica di solo invio; si chiede pertanto di non rispondere al messaggio.</p>" +
					$"<p></p>";
					oggettoEmail = $"Istanza di presentazione programmi {datiDomanda.Denominazione} {datiDomanda.CodiceRegione}";

					mail.Subject = oggettoEmail;
					mail.Body = testoEmail;
					mail.IsBodyHtml = true;
					if (datiDomanda.BinData != null)
					{
						Attachment file = new Attachment(new MemoryStream(datiDomanda.BinData), datiDomanda.FileName);
						mail.Attachments.Add(file);
					}
					if (string.IsNullOrEmpty(Parametri.ServerSMTPUsername))
					{
						client.UseDefaultCredentials = true;
					}
					else
					{
						NetworkCredential basicAuthInfo = new NetworkCredential(Parametri.ServerSMTPUsername, Parametri.ServerSMTPPassword);
						client.UseDefaultCredentials = false;
						client.Credentials = basicAuthInfo;
						//client.EnableSsl = true;
					}
					if (!string.IsNullOrEmpty(Parametri.ServerSMTPPort))
					{
						client.Port = int.Parse(Parametri.ServerSMTPPort);
					}

					try
					{
						client.Send(mail);
					}
					catch (Exception e)
					{
						Log.Error(LogEvent.ERRORE_PROTOCOLLO, "Invio e-mail", e, entity: entity);
						throw e;
					}
					protocollo.DataInvioEmail = DateTime.Now;
					context.SaveChanges();
					Log.Information(LogEvent.PROTOCOLLO_EFFETTUATO, entity: entity, message: $"Protocollo n.{numeroProtocollo}");
				}
			}
		}

		private void ProtocollaAnnullaDomandeProgrammi(ProtocolloProgramma protocollo, string token)
		{
			DatiDomandaProgramma datiDomanda = context.DatiDomandaProgramma
				.FirstOrDefault(x => x.Id == protocollo.Id);
			//Entità per i log
			Entity entity = new Entity()
			{
				Id = protocollo.Id,
				Name = "ProtocolloAnnullaProgramma"
			};
			Log.SetEnte(datiDomanda.CodiceFiscaleEnte);
			DateTime? dataProtocollo = null;
			string numeroProtocollo = protocollo.NumeroProtocolloAnnullamento;
			if (protocollo.DataProtocollazioneAnnullamento == null)
			{
				string anno = DateTime.Today.Year.ToString();
				PROTOCOLLOEX_CREATO responseProtocollo;
				//string oggetto = "ALB#SCU#ISTANZAPROGRAMMA " + datiDomanda.Denominazione;
				string oggetto = $"{Parametri.ProtocolloOggettarioIstanzaProgramma} Annullamento presentazione Prot.{datiDomanda.NumeroProtocollo} {datiDomanda.Denominazione}";

				//Creo (in memoria) un semplice txt di annullamento da protocollare
				string nomeFile = $"AnnullamentoPresentazioneProt_{datiDomanda.NumeroProtocollo}.txt";
				byte[] file = Encoding.UTF8.GetBytes($"Annullamento Presentazione Prot.{datiDomanda.NumeroProtocollo} Ente - {datiDomanda.CodiceRegione} {datiDomanda.Denominazione}");
				//

				try
				{
					responseProtocollo = Protocollazione.InviaProtocollo(
						token,
						oggetto,
						null,
						datiDomanda.Denominazione,
						datiDomanda.CodiceRegione,
						datiDomanda.PEC,
						nomeFile, // datiDomanda.FileName,
						file, // datiDomanda.BinData,
						Protocollazione.TipoProtocollo.IstanzaPaogramma
					);
				}
				catch (Exception e)
				{
					Log.Error(LogEvent.ERRORE_PROTOCOLLO, "Connessione Servizio Protocollo", e, entity: entity);
					throw e;
				}
				if (responseProtocollo.ESITO.Substring(0, 8) != "00000 - ")
				{
					Exception e = new Exception($"Risposta Protocollo: {responseProtocollo.ESITO}");
					Log.Error(LogEvent.ERRORE_PROTOCOLLO, "Risposta Servizio", e, entity: entity);
					throw e;
				}
				DateTime date;
				if (DateTime.TryParseExact(responseProtocollo.DATAPROTOCOLLO, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
				{
					dataProtocollo = date;
				}
				protocollo.DataProtocollazioneAnnullamento = DateTime.Now;
				protocollo.DataProtocolloAnnullamento = dataProtocollo;
				protocollo.NumeroProtocolloAnnullamento = responseProtocollo.NUMEROPROTOCOLLO;
				numeroProtocollo = protocollo.NumeroProtocolloAnnullamento;

				context.SaveChanges();
			}
			if (protocollo.DataProtocolloAnnullamento == null || string.IsNullOrEmpty(protocollo.NumeroProtocolloAnnullamento))
			{
				//Faccio finta di aver inviato la mail
				protocollo.DataInvioEmailAnnullamento = DateTime.Now;
				context.SaveChanges();
				Log.Error(LogEvent.ERRORE_PROTOCOLLO, "Dati risposta non valorizzati", parameters: new { protocollo.DataProtocolloAnnullamento, protocollo.NumeroProtocolloAnnullamento });
			}
			else
			{
				string destinatari;
				if (string.IsNullOrEmpty(Parametri.OverrideEmail))
				{
					destinatari = $"{datiDomanda.Email}";
				}
				else
				{
					destinatari = Parametri.OverrideEmail;
				}
				using (MailMessage mail = new MailMessage(Parametri.ServerSMTPMailFrom, destinatari))
				using (SmtpClient client = new SmtpClient(Parametri.ServerSMTPEndpoint))
				{
					string testoEmail;
					string oggettoEmail;
					testoEmail = $"<p>Gentile Utente, il Dipartimento ha ricevuto sul sistema Helios la richiesta di annullamento dell’istanza di presentazione programmi, protocollata il {datiDomanda.DataProtocollo?.ToString("dd/MM/yyyy")} con protocollo n.{datiDomanda.NumeroProtocollo}, relativa all’avviso \"{datiDomanda.Bando}\" per l’ente {datiDomanda.CodiceRegione} – {datiDomanda.Denominazione}.</p>" +
					$"<p>L’istanza di presentazione è stata annullata in data {datiDomanda.DataAnnullamento?.ToString("dd/MM/yyyy")} ed ora {datiDomanda.DataAnnullamento?.ToString("HH:mm:ss")} e protocollata il {protocollo.DataProtocolloAnnullamento?.ToString("dd/MM/yyyy")} con protocollo n.{protocollo.NumeroProtocolloAnnullamento}.</p>" +
					//$"<p>Copia dell’istanza di presentazione è allegata a questa e-mail.</p>" +
					$"<p>La presente e-mail è stata generata automaticamente da un indirizzo di posta elettronica di solo invio; si chiede pertanto di non rispondere al messaggio.</p>" +
					$"<p></p>";
					oggettoEmail = $"Annullamento istanza di presentazione programmi {datiDomanda.Denominazione} {datiDomanda.CodiceRegione}";

					mail.Subject = oggettoEmail;
					mail.Body = testoEmail;
					mail.IsBodyHtml = true;
					//if (datiDomanda.BinData != null)
					//{
					//	Attachment file = new Attachment(new MemoryStream(datiDomanda.BinData), datiDomanda.FileName);
					//	mail.Attachments.Add(file);
					//}
					if (string.IsNullOrEmpty(Parametri.ServerSMTPUsername))
					{
						client.UseDefaultCredentials = true;
					}
					else
					{
						NetworkCredential basicAuthInfo = new NetworkCredential(Parametri.ServerSMTPUsername, Parametri.ServerSMTPPassword);
						client.UseDefaultCredentials = false;
						client.Credentials = basicAuthInfo;
						//client.EnableSsl = true;
					}
					if (!string.IsNullOrEmpty(Parametri.ServerSMTPPort))
					{
						client.Port = int.Parse(Parametri.ServerSMTPPort);
					}

					try
					{
						client.Send(mail);
					}
					catch (Exception e)
					{
						Log.Error(LogEvent.ERRORE_PROTOCOLLO, "Invio e-mail", e, entity: entity);
						throw e;
					}
					protocollo.DataInvioEmailAnnullamento = DateTime.Now;
					context.SaveChanges();
					Log.Information(LogEvent.PROTOCOLLO_EFFETTUATO, entity: entity, message: $"Protocollo n.{numeroProtocollo}");
				}
			}
		}

		private void ProtocollaAntimafia(ProtocolloAntimafia protocollo, string token)
		{
			DatiAntimafia datiDomanda = context.DatiAntimafia
				.FirstOrDefault(x => x.Id == protocollo.Id);
			//Entità per i log
			Entity entity = new Entity()
			{
				Id = protocollo.Id,
				Name = "ProtocolloAntimafia"
			};
			Log.SetEnte(datiDomanda.CodiceFiscaleEnte);
			DateTime? dataProtocollo = null;
			string numeroProtocollo = protocollo.NumeroProtocollo;
			if (protocollo.DataProtocollazione == null)
			{
				string anno = DateTime.Today.Year.ToString();
				PROTOCOLLOEX_CREATO responseProtocollo;
				string allegatonomefile = $"COMUNICAZIONE_{datiDomanda.CodiceFiscaleEnte}{Path.GetExtension(datiDomanda.FileName)}";
				//string oggetto = "ALB#SCU#ANTIMAFIA " + datiDomanda.Denominazione;
				string oggetto = $"{Parametri.ProtocolloOggettarioAntimafia} {datiDomanda.Denominazione}";
				string mittente = $"{datiDomanda.Cognome} {datiDomanda.Nome}";
				if (datiDomanda.FileName.EndsWith("pdf.p7m"))
				{
					allegatonomefile = $"COMUNICAZIONE_{datiDomanda.CodiceFiscaleEnte}.pdf.p7m";
				}
				string fileBase64 = Convert.ToBase64String(datiDomanda.BinData, 0, datiDomanda.BinData.Length);

				try
				{
					//responseProtocollo = Protocollazione.InviaProtocollo(
					//	oggetto,
					//	Parametri.ProtocolloFascicolo,
					//	datiDomanda.Denominazione,
					//	datiDomanda.PEC,
					//	allegatonomefile,
					//	datiDomanda.BinData
					//);
					responseProtocollo = Protocollazione.InviaProtocollo(
						token,
						oggetto,
						null,
						datiDomanda.Denominazione,
						datiDomanda.CodiceRegione,
						datiDomanda.PEC,
						allegatonomefile,
						datiDomanda.BinData,
						Protocollazione.TipoProtocollo.Antimafia
					);
				}
				catch (Exception e)
				{
					Log.Error(LogEvent.ERRORE_PROTOCOLLO, "Connessione Servizio Protocollo", e, entity: entity);
					throw e;
				}
				if (responseProtocollo.ESITO.Substring(0, 8) != "00000 - ")
				{
					Exception e = new Exception($"Rispota Protocollo: {responseProtocollo.ESITO}");
					Log.Error(LogEvent.ERRORE_PROTOCOLLO, "Risposta Servizio", e, entity: entity);
					throw e;
				}
				DateTime date;
				if (DateTime.TryParseExact(responseProtocollo.DATAPROTOCOLLO, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
				{
					dataProtocollo = date;
				}
				protocollo.DataProtocollazione = DateTime.Now;
				protocollo.DataProtocollo = dataProtocollo;
				protocollo.NumeroProtocollo = responseProtocollo.NUMEROPROTOCOLLO;
				numeroProtocollo = protocollo.NumeroProtocollo;

				context.SaveChanges();
			}
			if (protocollo.DataProtocollo == null || string.IsNullOrEmpty(protocollo.NumeroProtocollo))
			{
				//Faccio finta di aver inviato la mail
				protocollo.DataInvioEmail = DateTime.Now;
				context.SaveChanges();
				Log.Error(LogEvent.ERRORE_PROTOCOLLO, "Dati risposta non valorizzati", parameters: new { protocollo.DataProtocollo, protocollo.NumeroProtocollo });
			}
			else
			{
				string destinatari;
				if (string.IsNullOrEmpty(Parametri.OverrideEmail))
				{
					destinatari = $"{datiDomanda.Email}";
				}
				else
				{
					destinatari = Parametri.OverrideEmail;
				}
				using (MailMessage mail = new MailMessage(Parametri.ServerSMTPMailFrom, destinatari))
				using (SmtpClient client = new SmtpClient(Parametri.ServerSMTPEndpoint))
				{
					string textdomanda = "iscrizione";
					string testoEmail;
					string oggettoEmail;
					testoEmail = $"<p>Gentile Utente, il Dipartimento ha ricevuto sul sistema Helios la comunicazione di aggiornamento dei dati antimafia dell’ente {datiDomanda.CodiceRegione} {datiDomanda.Denominazione}</p>" +
					$"<p>La comunicazione di aggiornamento dei dati antimafia è stata protocollata il {dataProtocollo?.ToString("dd/MM/yyyy")} con protocollo n.{numeroProtocollo}.</p>" +
					$"<p>Il Legale Rappresentante è {datiDomanda.Nome.ToUpper()} {datiDomanda.Cognome.ToUpper()} C.F. {datiDomanda.CodiceFiscaleRL.ToUpper()}.</p>" +
					$"<p>Per le successive interlocuzioni con il Dipartimento, l’ente utilizzerà esclusivamente la casella di posta certificata, associata alla domanda di {textdomanda}, {datiDomanda.PEC}. L’ente è tenuto a comunicare tempestivamente le eventuali variazioni della casella di posta certificata.</ p>" +
					$"<p>Copia della comunicazione di aggiornamento dei dati antimafia presentata è allegata a questa e-mail.</p>" +
					$"<p>La presente e-mail è stata generata automaticamente da un indirizzo di posta elettronica di solo invio; si chiede pertanto di non rispondere al messaggio.</p>";
					oggettoEmail = $"Comunicazione di aggiornamento dei dati antimafia {datiDomanda.Denominazione} {datiDomanda.CodiceRegione}";

					mail.Subject = oggettoEmail;
					mail.Body = testoEmail;
					mail.IsBodyHtml = true;
					if (datiDomanda.BinData != null)
					{
						Attachment file = new Attachment(new MemoryStream(datiDomanda.BinData), $"DOMANDA_{datiDomanda.CodiceFiscaleEnte}{Path.GetExtension(datiDomanda.FileName)}");
						mail.Attachments.Add(file);
					}
					if (string.IsNullOrEmpty(Parametri.ServerSMTPUsername))
					{
						client.UseDefaultCredentials = true;
					}
					else
					{
						NetworkCredential basicAuthInfo = new NetworkCredential(Parametri.ServerSMTPUsername, Parametri.ServerSMTPPassword);
						client.UseDefaultCredentials = false;
						client.Credentials = basicAuthInfo;
						//client.EnableSsl = true;
					}
					if (!string.IsNullOrEmpty(Parametri.ServerSMTPPort))
					{
						client.Port = int.Parse(Parametri.ServerSMTPPort);
					}

					try
					{
						client.Send(mail);
					}
					catch (Exception e)
					{
						Log.Error(LogEvent.ERRORE_PROTOCOLLO, "Invio e-mail", e, entity: entity);
						throw e;
					}
					protocollo.DataInvioEmail = DateTime.Now;
					context.SaveChanges();
					Log.Information(LogEvent.PROTOCOLLO_EFFETTUATO, entity: entity, message: $"Protocollo n.{numeroProtocollo}");
				}
			}
		}

		private void ProtocollaDomande(ProtocolloPresentazione protocollo, string token)
		{
			DatiDomanda datiDomanda = context.DatiDomanda
				.FirstOrDefault(x => x.Id == protocollo.Id);
			//Entità per i log
			Entity entity = new Entity()
			{
				Id = protocollo.Id,
				Name = "Protocollo"
			};
			Log.SetEnte(datiDomanda.CodiceFiscaleEnte);
			DateTime? dataProtocollo = null;
			string numeroProtocollo = protocollo.NumeroProtocollo;
			if (protocollo.DataProtocollazione == null)
			{
				string anno = DateTime.Today.Year.ToString();
				PROTOCOLLOEX_CREATO responseProtocollo;

				string fileBase64 = null;
				string allegatonomefile = null;
				string oggetto = null;
				Protocollazione.TipoProtocollo tipoProtocollo = Protocollazione.TipoProtocollo.Standard;
				if (datiDomanda.TipoDomanda == 1)
				{
					allegatonomefile = $"DOMANDA_{datiDomanda.CodiceFiscaleEnte}{Path.GetExtension(datiDomanda.FileName)}";
					if (datiDomanda.FileName.EndsWith("pdf.p7m"))
					{
						allegatonomefile = $"DOMANDA_{datiDomanda.CodiceFiscaleEnte}.pdf.p7m";
					}
					//oggetto = "ALB#SCU#ISCRIZ " + datiDomanda.Denominazione;
					oggetto = $"{Parametri.ProtocolloOggettarioIscrizione} {datiDomanda.Denominazione}";
					tipoProtocollo = Protocollazione.TipoProtocollo.Iscrizione;
				}
				if (datiDomanda.TipoDomanda == 2)
				{
					allegatonomefile = $"DOMANDA_{datiDomanda.CodiceFiscaleEnte}{Path.GetExtension(datiDomanda.FileName)}";
					if (datiDomanda.FileName.EndsWith("pdf.p7m"))
					{
						allegatonomefile = $"DOMANDA_{datiDomanda.CodiceFiscaleEnte}.pdf.p7m";
					}
					//oggetto = "ALB#SCU#ADEGU " + datiDomanda.Denominazione;
					oggetto = $"{Parametri.ProtocolloOggettarioAdeguamento} {datiDomanda.Denominazione}";
					tipoProtocollo = Protocollazione.TipoProtocollo.Adeguamento;
				}
				if (datiDomanda.TipoDomanda == 3)
				{
					oggetto = "ALB#SCU#AVVIOPROC " + datiDomanda.Denominazione;
					allegatonomefile = $"AVVIO_PROCEDIMENTO_{datiDomanda.CodiceFiscaleEnte}.rtf";
					datiDomanda.BinData = CreaAvvioProcedimento(datiDomanda);
				}
				string mittente = $"{datiDomanda.Cognome} {datiDomanda.Nome}";
				datiDomanda.FileName = allegatonomefile;

				try
				{
					//responseProtocollo = Protocollazione.InviaProtocollo(
					//	oggetto,
					//	Parametri.ProtocolloFascicolo,
					//	datiDomanda.Denominazione,
					//	datiDomanda.PEC,
					//	allegatonomefile,
					//	datiDomanda.BinData
					//);
					responseProtocollo = Protocollazione.InviaProtocollo(
						token,
						oggetto,
						null,
						datiDomanda.Denominazione,
						datiDomanda.CodiceRegione,
						datiDomanda.PEC,
						allegatonomefile,
						datiDomanda.BinData,
						tipoProtocollo
					);
				}
				catch (Exception e)
				{
					Log.Error(LogEvent.ERRORE_PROTOCOLLO, "Connessione Servizio Protocollo", e, entity: entity);
					throw e;
				}
				if (responseProtocollo.ESITO.Substring(0, 8) != "00000 - ")
				{
					Exception e = new Exception($"Rispota Protocollo: {responseProtocollo.ESITO}");
					Log.Error(LogEvent.ERRORE_PROTOCOLLO, "Risposta Servizio Autenticazione", e, entity: entity);
					throw e;
				}
				DateTime date;
				if (DateTime.TryParseExact(responseProtocollo.DATAPROTOCOLLO, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
				{
					dataProtocollo = date;
				}
				protocollo.IdEnteFase = datiDomanda.IdEnteFase;
				protocollo.DataProtocollazione = DateTime.Now;
				protocollo.DataProtocollo = dataProtocollo;
				protocollo.NumeroProtocollo = responseProtocollo.NUMEROPROTOCOLLO;
				context.SaveChanges();
				datiDomanda.DataProtocollo = dataProtocollo;
				datiDomanda.NumeroProtocollo = responseProtocollo.NUMEROPROTOCOLLO;
				Log.Information(LogEvent.PROTOCOLLO_EFFETTUATO, entity: entity, message: $"Protocollo n.{numeroProtocollo}");

			}
			//INVIO MAIL
			if (protocollo.DataProtocollo == null || string.IsNullOrEmpty(protocollo.NumeroProtocollo))
			{
				//Faccio finta di aver inviato la mail
				protocollo.DataInvioEmail = DateTime.Now;
				context.SaveChanges();
				Log.Error(LogEvent.ERRORE_PROTOCOLLO, "Dati risposta non valorizzati", parameters: new { protocollo.DataProtocollo, protocollo.NumeroProtocollo });
			}
			else
			{
				//Il tipo domanda 3 è la lettera di avvio procedimento che richiede l'invio della PEC
				if (datiDomanda.TipoDomanda == 4)
				{
					try
					{
						InviaPECDomanda(datiDomanda);
						protocollo.DataInvioEmail = DateTime.Now;
						context.SaveChanges();
						Log.Information(LogEvent.PROTOCOLLO_EFFETTUATO, entity: entity, message: $"Inviata PEC Protocollo n.{numeroProtocollo}");

					}
					catch (Exception e)
					{
						Log.Error(LogEvent.ERRORE_PROTOCOLLO, "Invio PEC", e, entity: entity);
					}
				}
				else
				{

					try
					{
						InviaMailDomanda(datiDomanda);
						protocollo.DataInvioEmail = DateTime.Now;
						context.SaveChanges();
						Log.Information(LogEvent.PROTOCOLLO_EFFETTUATO, entity: entity, message: $"Inviate e-mail Protocollo n.{numeroProtocollo}");

					}
					catch (Exception e)
					{
						Log.Error(LogEvent.ERRORE_PROTOCOLLO, "Invio e-mail", e, entity: entity);
					}
				}
			}
		}

		private void ProtocollaDomandeTUTORAGGIO(ProtocolloTUTORAGGIO protocollo, string token)
		{
			DatiDomandaTUTORAGGIO datiDomanda = context.DatiDomandaTUTORAGGIO
				.FirstOrDefault(x => x.Id == protocollo.Id);
			//Entità per i log
			Entity entity = new Entity()
			{
				Id = protocollo.Id,
				Name = "Protocollo"
			};
			Log.SetEnte(datiDomanda.CodiceFiscaleEnte);
			DateTime? dataProtocollo = null;
			string numeroProtocollo = protocollo.NumeroProtocollo;
			if (protocollo.DataProtocollazione == null)
			{
				string anno = DateTime.Today.Year.ToString();
				PROTOCOLLOEX_CREATO responseProtocollo;

				//string allegatonomefile = $"DOMANDA_{datiDomanda.CodiceFiscaleEnte}{Path.GetExtension(datiDomanda.FileNameDomandaFirmata)}";
				string allegatonomefile = datiDomanda.FileNameDomandaFirmata;

				//string oggetto = "ALB#SCU#ISCRIZ " + datiDomanda.Denominazione;
				string oggetto = $"{Parametri.ProtocolloOggettarioTUTORAGGIO} {datiDomanda.Denominazione}";

				//if (datiDomanda.FileNameDomandaFirmata.EndsWith("pdf.p7m"))
				//{
				//	allegatonomefile = $"DOMANDA_{datiDomanda.CodiceFiscaleEnte}.pdf.p7m";
				//}

				string mittente = $"{datiDomanda.Cognome} {datiDomanda.Nome}";
				//datiDomanda.FileNameDomandaFirmata = allegatonomefile;

				try
				{
					responseProtocollo = Protocollazione.InviaProtocollo(
						token,
						oggetto,
						null,
						datiDomanda.Denominazione,
						datiDomanda.CodiceRegione,
						datiDomanda.PEC,
						allegatonomefile,
						datiDomanda.Domanda,
						Protocollazione.TipoProtocollo.TUTORAGGIO
					);
				}
				catch (Exception e)
				{
					Log.Error(LogEvent.ERRORE_PROTOCOLLO, "Connessione Servizio Protocollo", e, entity: entity);
					throw e;
				}
				if (responseProtocollo.ESITO.Substring(0, 8) != "00000 - ")
				{
					Exception e = new Exception($"Risposta Protocollo: {responseProtocollo.ESITO}");
					Log.Error(LogEvent.ERRORE_PROTOCOLLO, "Risposta Servizio Autenticazione", e, entity: entity);
					throw e;
				}
				DateTime date;
				if (DateTime.TryParseExact(responseProtocollo.DATAPROTOCOLLO, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
				{
					dataProtocollo = date;
				}
				protocollo.DataProtocollazione = DateTime.Now;
				protocollo.DataProtocollo = dataProtocollo;
				protocollo.NumeroProtocollo = responseProtocollo.NUMEROPROTOCOLLO;
				context.SaveChanges();
				datiDomanda.DataProtocollo = dataProtocollo;
				datiDomanda.NumeroProtocollo = responseProtocollo.NUMEROPROTOCOLLO;
				Log.Information(LogEvent.PROTOCOLLO_EFFETTUATO, entity: entity, message: $"Protocollo n.{numeroProtocollo}");

			}
			//INVIO MAIL
			if (protocollo.DataProtocollo == null || string.IsNullOrEmpty(protocollo.NumeroProtocollo))
			{
				//Faccio finta di aver inviato la mail
				protocollo.DataInvioEmail = DateTime.Now;
				context.SaveChanges();
				Log.Error(LogEvent.ERRORE_PROTOCOLLO, "Dati risposta non valorizzati", parameters: new { protocollo.DataProtocollo, protocollo.NumeroProtocollo });
			}
			else
			{
				try
				{
					InviaMailTUTORAGGIO(datiDomanda);
					protocollo.DataInvioEmail = DateTime.Now;
					context.SaveChanges();
					Log.Information(LogEvent.PROTOCOLLO_EFFETTUATO, entity: entity, message: $"Inviate e-mail Protocollo n.{numeroProtocollo}");

				}
				catch (Exception e)
				{
					Log.Error(LogEvent.ERRORE_PROTOCOLLO, "Invio e-mail", e, entity: entity);
				}
			}
		}


		private void ProtocollaDomandeOLP(ProtocolloOLP protocollo, string token)
		{
			DatiDomandaOLP datiDomanda = context.DatiDomandaOLP
				.FirstOrDefault(x => x.Id == protocollo.Id);
			//Entità per i log
			Entity entity = new Entity()
			{
				Id = protocollo.Id,
				Name = "Protocollo"
			};
			Log.SetEnte(datiDomanda.CodiceFiscaleEnte);
			DateTime? dataProtocollo = null;
			string numeroProtocollo = protocollo.NumeroProtocollo;
			if (protocollo.DataProtocollazione == null)
			{
				string anno = DateTime.Today.Year.ToString();
				PROTOCOLLOEX_CREATO responseProtocollo;

				string allegatonomefile = $"DOMANDA_{datiDomanda.CodiceFiscaleEnte}{Path.GetExtension(datiDomanda.FileNameDomandaFirmata)}";
				//string oggetto = "ALB#SCU#ISCRIZ " + datiDomanda.Denominazione;
				string oggetto = $"{Parametri.ProtocolloOggettarioOLP} {datiDomanda.Denominazione}";
				if (datiDomanda.FileNameDomandaFirmata.EndsWith("pdf.p7m"))
				{
					allegatonomefile = $"DOMANDA_{datiDomanda.CodiceFiscaleEnte}.pdf.p7m";
				}

				string mittente = $"{datiDomanda.Cognome} {datiDomanda.Nome}";
				datiDomanda.FileNameDomandaFirmata = allegatonomefile;

				try
				{
					responseProtocollo = Protocollazione.InviaProtocollo(
						token,
						oggetto,
						null,
						datiDomanda.Denominazione,
						datiDomanda.CodiceRegione,
						datiDomanda.PEC,
						allegatonomefile,
						datiDomanda.Domanda,
						Protocollazione.TipoProtocollo.OLP
					);
				}
				catch (Exception e)
				{
					Log.Error(LogEvent.ERRORE_PROTOCOLLO, "Connessione Servizio Protocollo", e, entity: entity);
					throw e;
				}
				if (responseProtocollo.ESITO.Substring(0, 8) != "00000 - ")
				{
					Exception e = new Exception($"Rispota Protocollo: {responseProtocollo.ESITO}");
					Log.Error(LogEvent.ERRORE_PROTOCOLLO, "Risposta Servizio Autenticazione", e, entity: entity);
					throw e;
				}
				DateTime date;
				if (DateTime.TryParseExact(responseProtocollo.DATAPROTOCOLLO, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
				{
					dataProtocollo = date;
				}
				protocollo.DataProtocollazione = DateTime.Now;
				protocollo.DataProtocollo = dataProtocollo;
				protocollo.NumeroProtocollo = responseProtocollo.NUMEROPROTOCOLLO;
				context.SaveChanges();
				datiDomanda.DataProtocollo = dataProtocollo;
				datiDomanda.NumeroProtocollo = responseProtocollo.NUMEROPROTOCOLLO;
				Log.Information(LogEvent.PROTOCOLLO_EFFETTUATO, entity: entity, message: $"Protocollo n.{numeroProtocollo}");

			}
			//INVIO MAIL
			if (protocollo.DataProtocollo == null || string.IsNullOrEmpty(protocollo.NumeroProtocollo))
			{
				//Faccio finta di aver inviato la mail
				protocollo.DataInvioEmail = DateTime.Now;
				context.SaveChanges();
				Log.Error(LogEvent.ERRORE_PROTOCOLLO, "Dati risposta non valorizzati", parameters: new { protocollo.DataProtocollo, protocollo.NumeroProtocollo });
			}
			else
			{
				try
				{
					InviaMailOLP(datiDomanda);
					protocollo.DataInvioEmail = DateTime.Now;
					context.SaveChanges();
					Log.Information(LogEvent.PROTOCOLLO_EFFETTUATO, entity: entity, message: $"Inviate e-mail Protocollo n.{numeroProtocollo}");

				}
				catch (Exception e)
				{
					Log.Error(LogEvent.ERRORE_PROTOCOLLO, "Invio e-mail", e, entity: entity);
				}
			}
		}


		private void InviaPECDomanda(DatiDomanda datiDomanda)
		{
			//TODO Invio PEC
		}

		private void InviaMailDomanda(DatiDomanda datiDomanda)
		{
			string destinatari;
			if (string.IsNullOrEmpty(Parametri.OverrideEmail))
			{
				destinatari = $"{datiDomanda.Email}";
			}
			else
			{
				destinatari = Parametri.OverrideEmail;
			}
			using (MailMessage mail = new MailMessage(Parametri.ServerSMTPMailFrom, destinatari))
			using (SmtpClient client = new SmtpClient(Parametri.ServerSMTPEndpoint))
			{
				string textdomanda = "iscrizione";
				string testoEmail="";
				string oggettoEmail="";
				if (datiDomanda.TipoDomanda == 1)
				{
					testoEmail = $"<p>Gentile Utente, il Dipartimento ha ricevuto sul sistema Helios la domanda di {textdomanda} all’Albo SCU dell’ente {datiDomanda.CodiceRegione} {datiDomanda.Denominazione}</p>" +
					$"<p>L'ente {datiDomanda.Denominazione} è ora identificato con il codice provvisorio {datiDomanda.CodiceRegione}</p>" +
					$"<p>La domanda di {textdomanda} è stata presentata il {datiDomanda.DataRichiestaAccreditamento?.ToString("dd/MM/yyyy")} e protocollata il {datiDomanda.DataProtocollo?.ToString("dd/MM/yyyy")} con protocollo n.{datiDomanda.NumeroProtocollo}.</p>" +
					$"<p>Il Legale Rappresentante è {datiDomanda.Nome.ToUpper()} {datiDomanda.Cognome.ToUpper()} C.F. {datiDomanda.CodiceFiscaleRL.ToUpper()}.</p>" +
					$"<p>Per le successive interlocuzioni con il Dipartimento, l’ente utilizzerà esclusivamente la casella di posta certificata, associata alla domanda di {textdomanda}, {datiDomanda.PEC}. L’ente è tenuto a comunicare tempestivamente le eventuali variazioni della casella di posta certificata.</ p>" +
					$"<p>Copia della domanda di iscrizione presentata è allegata a questa e-mail.</p>" +
					$"<p>La presente e-mail è stata generata automaticamente da un indirizzo di posta elettronica di solo invio; si chiede pertanto di non rispondere al messaggio.</p>";
					oggettoEmail = $"Domanda di {textdomanda} Albo degli enti di servizio civile universale {datiDomanda.Denominazione} {datiDomanda.CodiceRegione}";
				}
				else if (datiDomanda.TipoDomanda == 2)
				{
					textdomanda = "adeguamento";
					testoEmail = $"<p>Gentile Utente, il Dipartimento ha ricevuto sul sistema Helios la domanda di adeguamento dell’iscrizione all’Albo SCU dell’ente {datiDomanda.CodiceRegione} {datiDomanda.Denominazione}</p>" +
					$"<p>La domanda di {textdomanda} è stata presentata il {datiDomanda.DataRichiestaAccreditamento?.ToString("dd/MM/yyyy")} e protocollata il {datiDomanda.DataProtocollo?.ToString("dd/MM/yyyy")} con protocollo n.{datiDomanda.NumeroProtocollo}.</p>" +
					$"<p>Il Legale Rappresentante è {datiDomanda.Nome.ToUpper()} {datiDomanda.Cognome.ToUpper()} C.F. {datiDomanda.CodiceFiscaleRL.ToUpper()}.</p>" +
					$"<p>Per le successive interlocuzioni con il Dipartimento, l’ente utilizzerà esclusivamente la casella di posta certificata, associata alla domanda di {textdomanda}, {datiDomanda.PEC}. L’ente è tenuto a comunicare tempestivamente le eventuali variazioni della casella di posta certificata.</ p>" +
					$"<p>Copia della domanda di adeguamento presentata è allegata a questa e-mail.</p>" +
					$"<p>La presente e-mail è stata generata automaticamente da un indirizzo di posta elettronica di solo invio; si chiede pertanto di non rispondere al messaggio.</p>";
					oggettoEmail = $"Domanda di {textdomanda} Albo degli enti di servizio civile universale {datiDomanda.Denominazione} {datiDomanda.CodiceRegione}";
				}


				mail.Subject = oggettoEmail;
				mail.Body = testoEmail;
				mail.IsBodyHtml = true;
				if (datiDomanda.BinData != null)
				{
					Attachment file = new Attachment(new MemoryStream(datiDomanda.BinData), datiDomanda.FileName);
					mail.Attachments.Add(file);
				}
				if (string.IsNullOrEmpty(Parametri.ServerSMTPUsername))
				{
					client.UseDefaultCredentials = true;
				}
				else
				{
					NetworkCredential basicAuthInfo = new NetworkCredential(Parametri.ServerSMTPUsername, Parametri.ServerSMTPPassword);
					client.UseDefaultCredentials = false;
					client.Credentials = basicAuthInfo;
					//client.EnableSsl = true;
				}
				if (!string.IsNullOrEmpty(Parametri.ServerSMTPPort))
				{
					client.Port = int.Parse(Parametri.ServerSMTPPort);
				}

				try
				{
					client.Send(mail);
				}
				catch (Exception e)
				{
					throw new Exception ("Errore nell'invio della mail",e);
				}

			}
		}

		private void InviaMailTUTORAGGIO(DatiDomandaTUTORAGGIO datiDomanda)
		{
			string destinatari;
			if (string.IsNullOrEmpty(Parametri.OverrideEmail))
			{
				destinatari = $"{datiDomanda.Email}";
			}
			else
			{
				destinatari = Parametri.OverrideEmail;
			}
			using (MailMessage mail = new MailMessage(Parametri.ServerSMTPMailFrom, destinatari))
			using (SmtpClient client = new SmtpClient(Parametri.ServerSMTPEndpoint))
			{
				string testoEmail = $"<p>Gentile Utente, il Dipartimento ha ricevuto sul sistema Helios la domanda di richiesta Tutoraggio dell’ente {datiDomanda.CodiceRegione} {datiDomanda.Denominazione}</p>" +
					$"<p>La domanda è stata presentata il {datiDomanda.DataPresentazione?.ToString("dd/MM/yyyy")} e protocollata il {datiDomanda.DataProtocollo?.ToString("dd/MM/yyyy")} con protocollo n.{datiDomanda.NumeroProtocollo}.</p>" +
					$"<p>Copia della domanda di richiesta Tutoraggio presentata è allegata a questa e-mail.</p>" +
					$"<p>La presente e-mail è stata generata automaticamente da un indirizzo di posta elettronica di solo invio; si chiede pertanto di non rispondere al messaggio.</p>";
				string oggettoEmail = $"Domanda di richiesta Tutoraggio - {datiDomanda.Denominazione} {datiDomanda.CodiceRegione}";


				mail.Subject = oggettoEmail;
				mail.Body = testoEmail;
				mail.IsBodyHtml = true;
				if (datiDomanda.Domanda != null)
				{
					Attachment file = new Attachment(new MemoryStream(datiDomanda.Domanda), datiDomanda.FileNameDomandaFirmata);
					mail.Attachments.Add(file);
				}
				if (string.IsNullOrEmpty(Parametri.ServerSMTPUsername))
				{
					client.UseDefaultCredentials = true;
				}
				else
				{
					NetworkCredential basicAuthInfo = new NetworkCredential(Parametri.ServerSMTPUsername, Parametri.ServerSMTPPassword);
					client.UseDefaultCredentials = false;
					client.Credentials = basicAuthInfo;
					//client.EnableSsl = true;
				}
				if (!string.IsNullOrEmpty(Parametri.ServerSMTPPort))
				{
					client.Port = int.Parse(Parametri.ServerSMTPPort);
				}

				try
				{
					client.Send(mail);
				}
				catch (Exception e)
				{
					throw new Exception("Errore nell'invio della mail", e);
				}

			}
		}


		private void InviaMailOLP(DatiDomandaOLP datiDomanda)
		{
			string destinatari;
			if (string.IsNullOrEmpty(Parametri.OverrideEmail))
			{
				destinatari = $"{datiDomanda.Email}";
			}
			else
			{
				destinatari = Parametri.OverrideEmail;
			}
			using (MailMessage mail = new MailMessage(Parametri.ServerSMTPMailFrom, destinatari))
			using (SmtpClient client = new SmtpClient(Parametri.ServerSMTPEndpoint))
			{
				string testoEmail = $"<p>Gentile Utente, il Dipartimento ha ricevuto sul sistema Helios la domanda di sostituzione Operatori Locali di Progetto (OLP) dell’ente {datiDomanda.CodiceRegione} {datiDomanda.Denominazione}</p>" +
					$"<p>La domanda di sostituzione è stata presentata il {datiDomanda.DataPresentazione?.ToString("dd/MM/yyyy")} e protocollata il {datiDomanda.DataProtocollo?.ToString("dd/MM/yyyy")} con protocollo n.{datiDomanda.NumeroProtocollo}.</p>" +
					$"<p>Copia della domanda di sostituzione presentata è allegata a questa e-mail.</p>" +
					$"<p>La presente e-mail è stata generata automaticamente da un indirizzo di posta elettronica di solo invio; si chiede pertanto di non rispondere al messaggio.</p>";
				string oggettoEmail = $"Domanda di sostituzione Operatori Locali di Progetto (OLP) - {datiDomanda.Denominazione} {datiDomanda.CodiceRegione}";


				mail.Subject = oggettoEmail;
				mail.Body = testoEmail;
				mail.IsBodyHtml = true;
				if (datiDomanda.Domanda != null)
				{
					Attachment file = new Attachment(new MemoryStream(datiDomanda.Domanda), datiDomanda.FileNameDomandaFirmata);
					mail.Attachments.Add(file);
				}
				if (string.IsNullOrEmpty(Parametri.ServerSMTPUsername))
				{
					client.UseDefaultCredentials = true;
				}
				else
				{
					NetworkCredential basicAuthInfo = new NetworkCredential(Parametri.ServerSMTPUsername, Parametri.ServerSMTPPassword);
					client.UseDefaultCredentials = false;
					client.Credentials = basicAuthInfo;
					//client.EnableSsl = true;
				}
				if (!string.IsNullOrEmpty(Parametri.ServerSMTPPort))
				{
					client.Port = int.Parse(Parametri.ServerSMTPPort);
				}

				try
				{
					client.Send(mail);
				}
				catch (Exception e)
				{
					throw new Exception("Errore nell'invio della mail", e);
				}

			}
		}


		//Genera un file rtf Dati i dati della domanda
		private byte[] CreaAvvioProcedimento(DatiDomanda datiDomanda)
		{
			string denominazione = GetRtfUnicodeEscapedString(datiDomanda.Denominazione);
			string rtf = System.IO.File.ReadAllText(Environment.ContentRootPath+ "\\Templates\\LetteraAvvioProcedimento.rtf");
			rtf=rtf.Replace("<nomeEnte>", denominazione);
			rtf=rtf.Replace("<numeroProtocolloDomanda>", datiDomanda.NumeroProtocollo);
			rtf=rtf.Replace("<dataProtocolloDomanda>", datiDomanda.DataProtocollo?.ToString("dd/MM/yyyy"));
			rtf=rtf.Replace("<codiceEnte>", datiDomanda.CodiceRegione);
			rtf=rtf.Replace("<tipoProcedimento>", datiDomanda.TipoDomanda==1? "iscrizione": "adeguamento dell\\rquoteiscrizione");
			rtf=rtf.Replace("<idEnteFase>", datiDomanda.IdEnteFase.ToString());
			rtf=rtf.Replace("<fineProcedimento>", datiDomanda.DataProtocollo?.AddDays(180).ToString("dd/MM/yyyy"));
			return Encoding.Default.GetBytes(rtf);
		}

		//Converte i caratteri speciali per RTF
		private string GetRtfUnicodeEscapedString(string s)
		{
			var sb = new StringBuilder();
			foreach (var c in s)
			{
				if (c == '\\' || c == '{' || c == '}')
					sb.Append(@"\" + c);
				else if (c <= 0x7f)
					sb.Append(c);
				else
					sb.Append("\\u" + Convert.ToUInt32(c) + "?");
			}
			return sb.ToString();
		}

		private void ProtocollaRegistrazione(Registrazione registrazione,string token)
		{
			//Entità per i log
			Entity entity = new Entity()
			{
				Id = registrazione.Id,
				Name = "Registrazione"
			};
			Log.SetEnte(registrazione.CodiceFiscaleEnte);

			Soggetto soggetto = context.Soggetto.FirstOrDefault(s => s.CodiceFiscale == registrazione.CodiceFiscaleRappresentanteLegale);

			Documento documento = context.Documento
				.SingleOrDefault(d => d.Id == registrazione.IdDocumento);
			if (registrazione.DataProtocollazione == null)
			{
				string anno = DateTime.Today.Year.ToString();

				string allegatonomefile = "domandaRegistrazione" + Path.GetExtension(documento.NomeFile);
				if (documento.NomeFile.EndsWith("pdf.p7m"))
				{
					allegatonomefile = "domandaRegistrazione.pdf.p7m";
				}
				//string oggetto = "ALB#SCU#REGISTR " + registrazione.Denominazione;
				string oggetto = $"{Parametri.ProtocolloOggettarioRegistrazione} {registrazione.Denominazione}";
				string mittente = $"{soggetto.Cognome} {soggetto.Nome}";
				string fileBase64 = Convert.ToBase64String(documento.Blob, 0, documento.Blob.Length);
				PROTOCOLLOEX_CREATO responseProtocollo = null;
				try
				{
					//responseProtocollo = Protocollazione.InviaProtocollo(
					//	oggetto,
					//	Parametri.ProtocolloFascicolo,
					//	registrazione.Denominazione,
					//	registrazione.PEC,
					//	allegatonomefile,
					//	documento.Blob
					//);
					responseProtocollo = Protocollazione.InviaProtocollo(
						token,
						oggetto,
						null,
						registrazione.Denominazione,
						null,
						registrazione.PEC,
						allegatonomefile,
						documento.Blob
					);
				}
				catch (Exception e)
				{
					Log.Error(LogEvent.ERRORE_PROTOCOLLO, "Connessione Servizio Protocollo", e, entity: entity);
					throw e;
				}
				if (responseProtocollo.ESITO.Substring(0, 8) != "00000 - ")
				{
					Exception e = new Exception($"Rispota Protocollo: {responseProtocollo.ESITO}");
					Log.Error(LogEvent.ERRORE_PROTOCOLLO, "Risposta Servizio Autenticazione", e, entity: entity);
					throw e;
				}
				DateTime date;
				DateTime? dataProtocollo = null;
				if (DateTime.TryParseExact(responseProtocollo.DATAPROTOCOLLO, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
				{
					dataProtocollo = date;
				}
				registrazione.DataProtocollazione = DateTime.Now;
				registrazione.DataProtocollo = dataProtocollo;
				registrazione.NumeroProtocollo = responseProtocollo.NUMEROPROTOCOLLO;
				context.SaveChanges();
			}
			if (registrazione.DataProtocollo == null || string.IsNullOrEmpty(registrazione.NumeroProtocollo))
			{
				//Faccio finta di aver inviato la mail
				registrazione.DataInvioEmail = DateTime.Now;
				context.SaveChanges();
				Log.Error(LogEvent.ERRORE_PROTOCOLLO, "Dati risposta non valorizzati", parameters: new { registrazione.DataProtocollo, registrazione.NumeroProtocollo });
			}
			else
			{

				string destinatari;
				if (string.IsNullOrEmpty(Parametri.OverrideEmail))
				{
					destinatari = $"{registrazione.Email}";
				}
				else
				{
					destinatari = Parametri.OverrideEmail;
				}
				using (MailMessage mail = new MailMessage(Parametri.ServerSMTPMailFrom, destinatari))
				using (SmtpClient client = new SmtpClient(Parametri.ServerSMTPEndpoint))
				{

					string oggettoEmail = "Sistema Unico - Registrazione  utenza Servizio civile ";

					string testoEmail = $"<p>Si comunica che è stata effettuata la registrazione del Legale Rappresentante dell'ente \"{registrazione.Denominazione}\" per l'accesso al Sistema Unico.</p><p>Il Legale Rappresentante registrato è {soggetto.Cognome} {soggetto.Nome} C.F. {soggetto.CodiceFiscale}.</p><p>La richiesta di registrazione è stata protocollata in data {registrazione.DataProtocollo?.ToString("dd/MM/yyyy")} con protocollo N°{registrazione.NumeroProtocollo}.</p><p>In allegato è presente il documento di registrazione.</p><p>La presente e-mail è stata generata automaticamente da un indirizzo di posta elettronica di solo invio; si chiede pertanto di non rispondere al messaggio.</p><p>Per segnalazioni inviare una PEC a <a href=\"maito:giovanieserviziocivile@pec.governo.it\">giovanieserviziocivile@pec.governo.it</a></p>";
					mail.Subject = oggettoEmail;
					mail.Body = testoEmail;
					mail.IsBodyHtml = true;
					if (documento != null)
					{
						Attachment file = new Attachment(new MemoryStream(documento.Blob), documento.NomeFile);
						mail.Attachments.Add(file);
					}
					if (string.IsNullOrEmpty(Parametri.ServerSMTPUsername))
					{
						client.UseDefaultCredentials = true;
					}
					else
					{
						NetworkCredential basicAuthInfo = new NetworkCredential(Parametri.ServerSMTPUsername, Parametri.ServerSMTPPassword);
						client.UseDefaultCredentials = false;
						client.Credentials = basicAuthInfo;
						//client.EnableSsl = true;
					}
					if (!string.IsNullOrEmpty(Parametri.ServerSMTPPort))
					{
						client.Port = int.Parse(Parametri.ServerSMTPPort);
					}

					try
					{
						client.Send(mail);
					}
					catch (Exception e)
					{
						Log.Error(LogEvent.ERRORE_PROTOCOLLO, "Invio e-mail", e, entity: entity);
						throw e;
					}
					registrazione.DataInvioEmail = DateTime.Now;
					context.SaveChanges();
					Log.Information(LogEvent.PROTOCOLLO_EFFETTUATO, entity: entity, message: $"Protocollo N°{registrazione.NumeroProtocollo}");
				}
			}
		}
	}
}
