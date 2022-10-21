using System.Transactions;
using DomandeOnline.Code;
using DomandeOnline.Data;
using DomandeOnline.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using Serilog.Context;
using System.Configuration;
using System.Text.RegularExpressions;

namespace DomandeOnline.Controllers
{
	[Authorize]
	public class RichiestaCredenzialiController : SmartController
	{
		[AllowAnonymous]
		public ActionResult Informazioni()
		{
			return View();
		}

		[AllowAnonymous]
		public ActionResult Index()
		{
			using (var context = new Entities())
			{
				try
				{
					var maxBytesText = context.Configurazione
							.Where(c => c.Nome == Configurazione.MAXBYTESRICHIESTACREDENZIALI)
							.Select(x => x.Valore)
							.SingleOrDefault();
					ViewData["MaxBytes"] = Utils.FileDimensionToString(int.Parse(maxBytesText));
				}
				catch (Exception e)
				{
					Log.Error(LogEvent.DOMANDA_ONLINE, "Errore nel caricamento del file di configurazione",e);
					throw;
				}
				try
				{
					var nazioni = context.Nazione.OrderBy(x => x.Nome);
					ViewData["Nazioni"] = nazioni.ToList();
				}
				catch (Exception e)
				{
					Log.Error(LogEvent.DOMANDA_ONLINE, "Errore nel caricamento delle nazioni",e);
					throw;
				}
				try
				{
					var generi = context.Genere.OrderByDescending(x => x.Nome);
					ViewData["Generi"] = generi.ToList();
				}
				catch (Exception e)
				{
					Log.Error(LogEvent.DOMANDA_ONLINE, "Errore nel caricamento dei generi",e);
					throw;
				}
				return View();
			}
		}

		[HttpPost]
		[AllowAnonymous]
		public ActionResult RichiestaCredenziali(RichiestaCredenzialiInput parameters)
		{
			using (LogContext.PushProperty("Action", System.Reflection.MethodBase.GetCurrentMethod().Name))
			using (LogContext.PushProperty("Controller", System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name))
			using (LogContext.PushProperty("User", User.Identity.GetUserId()))
			using (LogContext.PushProperty("Parameters", parameters,true))
			using (var context = new Entities())
			{
				/* Memorizzazione dei parametri impostati */
				TempData["RichiestaCredenziali"] = parameters;
				Nazione nazioneNascita = null;
				Nazione nazioneCittadinanza = null;
				RichiestaCredenziali richiestaPresente = null;
				Genere genere = null;
				string maxBytesText = null;

				#region Lettura dati

				try
				{
					nazioneNascita = context.Nazione
						.Where(nazione => nazione.Id == parameters.IdNazioneNascita)
						.SingleOrDefault();

					nazioneCittadinanza = context.Nazione
						.Where(nazione => nazione.Id == parameters.IdNazioneCittadinanza)
						.SingleOrDefault();

					richiestaPresente = context.RichiestaCredenziali
						.Where(r => r.CodiceFiscale == parameters.CodiceFiscale && r.IdStato != StatoRichiestaCredenziali.RIFIUTATA)
						.FirstOrDefault();

					genere = context.Genere
						.Where(g => g.Codice == parameters.CodiceGenere)
						.SingleOrDefault();

					maxBytesText = context.Configurazione
						.Where(c => c.Nome == Configurazione.MAXBYTESRICHIESTACREDENZIALI)
						.Select(x => x.Valore)
						.SingleOrDefault();

				}
				catch (Exception e)
				{
					TempData["Errore"] = $"Errore interno! Riprovare più tardi.";
					Log.Error(LogEvent.DOMANDA_ONLINE, "Errore nell'accesso ai dati",e);
					return RedirectToAction("Index");
				}

				#endregion

				#region Controlli
				List<string> errori = new List<string>();
				byte[] file = null;
				/* Errori Bloccanti */
				if (string.IsNullOrEmpty(maxBytesText))
				{
					Log.Error(LogEvent.DOMANDA_ONLINE,"Errore nei parametri di configurazione MaxBytesRichiestaCredenziali non presente");
					TempData["Errore"] = "Errore Interno";
					return RedirectToAction("Index");
				}
				else if (!int.TryParse(maxBytesText, out int maxBytes))
				{
					Log.Error(LogEvent.DOMANDA_ONLINE, $"Errore nei parametri di configurazione MaxBytesRichiestaCredenziali non valorizzato correttamento valore = {maxBytesText}");
					TempData["Errore"] = "Errore Interno";
					return RedirectToAction("Index");
				}
				else if (richiestaPresente != null && richiestaPresente.IdStato == StatoRichiestaCredenziali.APERTA)
				{
					errori.Add($"Attenzione! Richiesta già avviata, attendere l'esito della richeta sulla e-mail fornita");
				}
				else if (richiestaPresente != null && richiestaPresente.IdStato == StatoRichiestaCredenziali.ACCETTATA)
				{
					errori.Add("Attenzione! La richiesta è stata già effettuata. Accedere con le credenziali fornite via e-mail o procedere al recupero delle credenziali");
				}
				else if (nazioneCittadinanza!=null && nazioneCittadinanza.CodiceISO2 == Nazione.ITALIA)
				{
					errori.Add("Attenzione! Non è possibile fare una richiesta di credenziali per cittadini italiani. Accedere tramite SPID");
				}
				else
				{
					if (string.IsNullOrEmpty(parameters.Nome))
					{
						errori.Add("Non è stato inserito il Nome");
					}
					if (string.IsNullOrEmpty(parameters.Cognome))
					{
						errori.Add("Non è stato inserito il Cognome");
					}
					if (parameters.CodiceGenere == null)
					{
						errori.Add("Non è stato inserito il Genere");
					}
					else
					if (genere == null)
					{
						errori.Add("Inserito un genere non corretto. I valori ammessi sono 'M' o 'F'");
					}
					if (!parameters.DataNascita.HasValue)
					{
						errori.Add("Non è stata inserita la data di nascita");
					}
					if (string.IsNullOrEmpty(parameters.LuogoNascita))
					{
						errori.Add("Non è stato inserito il luogo di nascita");
					}
					if (nazioneNascita == null)
					{
						errori.Add("Nazione di nascita inesistente");
					}
					if (string.IsNullOrEmpty(parameters.CodiceFiscale))
					{
						errori.Add("Non è stato inserito il codice fiscale");
					}
					else /* TODO Controlli Codice Fiscale */
					{
						parameters.CodiceFiscale = Regex.Replace(parameters.CodiceFiscale, @"\r|\n|\t| ", String.Empty);
						parameters.CodiceFiscale = parameters.CodiceFiscale.Trim();
						parameters.CodiceFiscale = parameters.CodiceFiscale.ToUpper();
					}
					if (nazioneCittadinanza == null)
					{
						errori.Add("Nazione di cittadinanza inesistente");
					}
					if (string.IsNullOrEmpty(parameters.Email))
					{
						errori.Add("Non è stata inserita l'email");
					}
					if (!(new EmailAddressAttribute().IsValid(parameters.Email)))
					{
						errori.Add("È stata inserita un'email non valida");
					}
					if (parameters.Email!=parameters.RipetiEmail)
					{
						errori.Add("Le email con coincidono");
					}
					if (string.IsNullOrEmpty(parameters.Telefono))
					{
						errori.Add("Non è stato inserito il numero di cellulare");
					}
					if (parameters.Allegato == null)
					{
						errori.Add($"Non è stato inserito l'allegato");
					}
					else
					if (parameters.Allegato.ContentLength < 4)
					{
						errori.Add($"Allegato non valido o vuoto");
					}
					else if (parameters.Allegato.ContentLength > maxBytes)
					{
						errori.Add($"Le dimesioni dell'allegato sono troppo grandi (dimensione massima {Utils.FileDimensionToString(maxBytes)})");
					}
					else
					{
						file = new byte[parameters.Allegato.ContentLength];
						parameters.Allegato.InputStream.Read(file, 0, parameters.Allegato.ContentLength);
						if (file[0] != 0x25 || file[1] != 0x50 || file[2] != 0x44 || file[3] != 0x46)
						{
							errori.Add($"Formato dell'allegato non corretto il file deve essere di tipo PDF");
						}
					}
					if (!(parameters.OkPrivacy??false))
					{
						errori.Add("Non è stata data conferma della presa visione dell'informativa della privacy");
					}
					if (!(parameters.PrivacyConsenso ?? false))
					{
						errori.Add("Non è stato dato il consenso della privacy");
					}
				}

				#endregion

				#region Elaborazione Dati
				TempData["Errore"] = string.Join("<br>",errori);

				/*TempData["indirizzo"] = indirizzo;
				TempData["cittaResidenza"] = cittaResidenza;
				TempData["cap"] = cap;
				TempData["idNazioneResidenza"] = idNazioneResidenza;*/

				if (errori.Count>0)
				{
					Log.Information(LogEvent.DOMANDA_ONLINE,"Richiesta credenziali - Esito negativo. Errori: {@Errori}",errori);
					return RedirectToAction("Index");
				}
				#endregion

				#region Scrittura Database
				try
				{
					RichiestaCredenziali richiesta = new RichiestaCredenziali()
					{
						IdStato = StatoRichiestaCredenziali.APERTA,
						DataRichiesta = DateTime.Now,
						Nome = parameters.Nome,
						Cognome = parameters.Cognome,
						Genere = genere,
						DataNascita = parameters.DataNascita,
						LuogoNascita = parameters.LuogoNascita,
						NazioneNascita = nazioneNascita,
						CodiceFiscale = parameters.CodiceFiscale,
						Cittadinanza = nazioneCittadinanza,
						Email = parameters.Email,
						Telefono = parameters.Telefono,
						Allegato = file
					};
					context.RichiestaCredenziali.Add(richiesta);
					context.SaveChanges();
					return View("Conferma", richiesta);
				}
				catch (Exception e)
				{
					TempData["Errore"] = $"Errore interno";
					Log.Error(LogEvent.DOMANDA_ONLINE, "Errore nel inserimento della RichiestaCredenziali nel database",e);
					return RedirectToAction("Index");
				}
				#endregion
			}
		}

		[Authorize(Roles =Role.OPERATORE)]
		public ActionResult ElencoRichieste(ElencoCredenzialiInput parametri)
		{
			using (LogContext.PushProperty("Action", System.Reflection.MethodBase.GetCurrentMethod().Name))
			using (LogContext.PushProperty("Controller", System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name))
			using (LogContext.PushProperty("User", User.Identity.GetUserId()))
			using (var context = new Entities())
			{
				/* Valori default */
				parametri.Pagina = parametri.NuovaPagina ?? parametri.Pagina ?? 1;
				parametri.ElementiPerPagina = parametri.ElementiPerPagina ?? 20;
				parametri.IdStato = parametri.IdStato ?? StatoRichiestaCredenziali.APERTA;
				try
				{
					var stati = context.StatoRichiestaCredenziali.ToList();

					var richieste = context.RichiestaCredenziali
						.Include(x => x.Genere)
						.Include(x => x.Stato)
						.Include(x => x.NazioneNascita)
						.Include(x => x.Cittadinanza);
						
					if(parametri.IdStato>0)
					{
						richieste = richieste.Where(x => x.IdStato == parametri.IdStato);
					}
					if (!string.IsNullOrEmpty(parametri.CodiceFiscale))
					{
						richieste = richieste.Where(x => x.CodiceFiscale.Contains(parametri.CodiceFiscale));
					}
					if (!string.IsNullOrEmpty(parametri.Nome))
					{
						richieste = richieste.Where(x => x.Nome.Contains(parametri.Nome));
					}
					if (!string.IsNullOrEmpty(parametri.Cognome))
					{
						richieste = richieste.Where(x => x.Cognome .Contains(parametri.Cognome));
					}
					int numeroRichieste = richieste.Count();
					int numeroPagine = (numeroRichieste / parametri.ElementiPerPagina.Value) + (numeroRichieste % parametri.ElementiPerPagina.Value == 0 ? 0 : 1);
					if (parametri.Pagina > numeroPagine) parametri.Pagina = numeroPagine;
					if (parametri.Pagina < 1) parametri.Pagina = 1;
					parametri.NumeroElementi = numeroRichieste;
					parametri.NumeroPagine = numeroPagine;
					ViewData["Stati"] = stati;
					ViewData["parametri.Pagina"] = parametri.Pagina;
					ViewData["NumeroPagine"] = numeroPagine;
					ViewData["parametri.ElementiPerPagina"] = parametri.ElementiPerPagina;
					ViewData["NumeroElementi"] = numeroRichieste;
					ViewData["ElencoCredenziali"] = parametri;
					var richiesteDto= richieste
						.Select(x => new RichiestaCredenzialiDto()
						{
							Id = x.Id,
							DataRichiesta = x.DataRichiesta,
							Stato = x.Stato.Nome,
							DataApprovazione = x.DataApprovazione,
							UtenteApprovazione = x.UtenteApprovazione,
							NoteApprovazione = x.NoteApprovazione,
							DataAnnullamento = x.DataAnnullamento,
							UtenteAnnullamento = x.UtenteAnnullamento,
							NoteAnnullamento = x.NoteAnnullamento,
							Nome = x.Nome,
							Cognome = x.Cognome,
							Genere = x.Genere.Nome,
							DataNascita = x.DataNascita,
							LuogoNascita = x.LuogoNascita,
							NazioneNascita = x.NazioneNascita.Nome,
							CodiceFiscale = x.CodiceFiscale,
							NazioneCittadinanza = x.Cittadinanza.Nome,
							Email = x.Email,
							Telefono = x.Telefono
						})
						.OrderByDescending(x => x.DataRichiesta);
					;
					return View(richiesteDto
						.Skip((parametri.Pagina.Value - 1) * parametri.ElementiPerPagina.Value)
						.Take(parametri.ElementiPerPagina.Value).ToList());
				}
				catch (Exception e)
				{
					Log.Error(LogEvent.DOMANDA_ONLINE, "Errore nella lettura delle RichiestaCredenziali nel Db",e);
					throw e;
				}
			}
		}

		[Authorize(Roles =Role.OPERATORE)]
		public ActionResult DownloadAllegato(int id)
		{
			using (LogContext.PushProperty("Action", System.Reflection.MethodBase.GetCurrentMethod().Name))
			using (LogContext.PushProperty("Controller", System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name))
			using (LogContext.PushProperty("User", User.Identity.GetUserId()))
			using (LogContext.PushProperty("Id", id))
			using (var context = new Entities())
			{
				try
				{
					var richiesta = context.RichiestaCredenziali
								.Where(x => x.Id == id)
								.SingleOrDefault();
					if (richiesta == null)
					{
						Log.Error(LogEvent.DOMANDA_ONLINE,$"Errore RichiestaCredenziali con id={id} non trovata");
						return HttpNotFound();
					}
					Log.Information(LogEvent.DOMANDA_ONLINE, $"Scaricato Allegato Richiesta Credenziali con id={id} ");
					return File(richiesta.Allegato, "application/pdf");
				}
				catch (Exception e)
				{
					Log.Error(LogEvent.DOMANDA_ONLINE, "Errore nella lettura del DB",e);
					throw;
				}
			}
		}

		[Authorize(Roles = Role.OPERATORE)]
		public ActionResult Dettaglio(int id)
		{
			using (LogContext.PushProperty("Action", System.Reflection.MethodBase.GetCurrentMethod().Name))
			using (LogContext.PushProperty("Controller", System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name))
			using (LogContext.PushProperty("User", User.Identity.GetUserId()))
			using (LogContext.PushProperty("Id", id))
			using (var context = new Entities())
			{
				try
				{
					var richiesta = context.RichiestaCredenziali
								.Where(x => x.Id == id)
								.Include(x => x.Genere)
								.Include(x => x.Stato)
								.Include(x => x.NazioneNascita)
								.Include(x => x.Cittadinanza)
								.SingleOrDefault();
					if (richiesta == null)
					{
						Log.Error(LogEvent.DOMANDA_ONLINE,$"Errore RichiestaCredenziali con id={id} non trovata");
						return HttpNotFound();
					}
					return View(richiesta);
				}
				catch (Exception e)
				{
					Log.Error(LogEvent.DOMANDA_ONLINE, "Errore nlella lettura del DB",e);
					throw;
				}
			}
		}

		[Authorize(Roles = Role.OPERATORE)]
		[HttpPost]
		public ActionResult Accetta(int id, string note)
		{
			using (LogContext.PushProperty("Action", System.Reflection.MethodBase.GetCurrentMethod().Name))
			using (LogContext.PushProperty("Controller", System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name))
			using (LogContext.PushProperty("User", User.Identity.GetUserId()))
			using (LogContext.PushProperty("Id", id))
			using (LogContext.PushProperty("Note", note))
			{
				try
				{
					using (var scope = new TransactionScope())
					{
						using (var context = new Entities())
						using (var userContext = new ApplicationDbContext())
						{
							/* Salvataggio richiesta */
							RichiestaCredenziali richiesta;
							try
							{

								richiesta = context.RichiestaCredenziali
									.Where(x => x.Id == id)
									.SingleOrDefault();
								if (richiesta == null)
								{
									Log.Error(LogEvent.DOMANDA_ONLINE,$"Errore RichiestaCredenziali con id={id} non trovata");
									return Json(new { success = false, message = "Richiesta non trovata" });
								}
								if (richiesta.IdStato!= StatoRichiestaCredenziali.APERTA)
								{
									return Json(new { success = false, message = "Richiesta già elaborata" });
								}
								richiesta.IdStato = StatoRichiestaCredenziali.ACCETTATA;
								richiesta.NoteApprovazione = note;
								richiesta.UtenteApprovazione = User.Identity.Name;
								richiesta.DataApprovazione = DateTime.Now;
								context.SaveChanges();
							}
							catch (Exception e)
							{
								Log.Error(LogEvent.DOMANDA_ONLINE, "Errore nell'update di RichiestaCredenziali",e);
								return Json(new { success = false, message = $"Errore di accesso ai dati {e.Message}" });
							}
							/* Creazione utenza */
							ApplicationUser user;
							var userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
							try
							{
								user = userManager.FindByName(richiesta.CodiceFiscale);
								if (user == null)
								{
									user = new ApplicationUser
									{
										UserName = richiesta.CodiceFiscale,
										Email = richiesta.Email,
										CodiceFiscale = richiesta.CodiceFiscale,
										Nome = richiesta.Nome,
										Cognome = richiesta.Cognome,
										Genere = richiesta.Genere.Nome,
										DataNascita = richiesta.DataNascita,
										LuogoNascita = richiesta.LuogoNascita,
										NazioneNascita = richiesta.NazioneNascita.Nome,
										Cittadinanza = richiesta.Cittadinanza.Nome,
										Telefono = richiesta.Telefono
									};
									//Password generata casualmente - La password sarà successivamente impostata dall'utente
									string passwordGenerated = System.Web.Security.Membership.GeneratePassword(10, 2) + "0aB!";//Pospongo i caratteri obbligatori
									var resultCreate = userManager.Create(user, passwordGenerated);
									if (!resultCreate.Succeeded)
									{
										Log.Error(LogEvent.DOMANDA_ONLINE,$"Errore nella creazione dell'utenza {string.Join(", ", resultCreate.Errors)}");
										return Json(new { success = false, message = $"Errore nella crezione dell'utenza\n{string.Join("\n", resultCreate.Errors)}" });
									}
									var resultRoleAdded = userManager.AddToRole(user.Id, Role.UTENTE_CREDENZIALI);
									if (!resultRoleAdded.Succeeded)
									{
										Log.Error(LogEvent.DOMANDA_ONLINE,$"Errore nell'associazione del ruolo all'utenza {string.Join(", ", resultRoleAdded.Errors)}");
										return Json(new { success = false, message = $"Errore nella crezione dell'utenza\n{string.Join("\n", resultRoleAdded.Errors)}" });
									}
								}else
								{
									user.UserName = richiesta.CodiceFiscale;
									user.Email = richiesta.Email;
									user.CodiceFiscale = richiesta.CodiceFiscale;
									user.Nome = richiesta.Nome;
									user.Cognome = richiesta.Cognome;
									user.Genere = richiesta.Genere.Nome;
									user.DataNascita = richiesta.DataNascita;
									user.LuogoNascita = richiesta.LuogoNascita;
									user.NazioneNascita = richiesta.NazioneNascita.Nome;
									user.Cittadinanza = richiesta.Cittadinanza.Nome;
									user.Telefono = richiesta.Telefono;
									userManager.Update(user);
								}
							}
							catch (Exception e)
							{
								Log.Error(LogEvent.DOMANDA_ONLINE, "Errore nella crezione dell'utenza",e);
								return Json(new { success = false, message = $"Errore nella crezione dell'utenza {e.Message}" });
							}
							TokenCredenziali token;
							try
							{
								token = new TokenCredenziali()
								{
									Id = Guid.NewGuid(),
									DataGenerazione = DateTime.Now,
									Scadenza = DateTime.Now.AddHours(24),
									UserId = user.Id,
									code = userManager.GeneratePasswordResetToken(user.Id)
								};
								context.TokenCredenziali.Add(token);
								context.SaveChanges();
							}
							catch (Exception e)
							{
								Log.Error(LogEvent.DOMANDA_ONLINE, "Errore nella creazione del token",e);
								return Json(new { success = false, message = $"Errore nella generazione del token:\n {e.Message}" });
							}
							string urlSito = ConfigurationManager.AppSettings["UrlSito"];
							using (var emailService = new Email.ServiceClient())
								try
								{
									string urlToken = Url.Action("ImpostaPassword", "Account", new { userId = user.Id, tokenId = token.Id, code = token.code }, Request.Url.Scheme);
									urlToken = urlSito + urlToken.Substring(urlToken.IndexOf("/Account/ImpostaPassword", 0));

									if(!emailService.emailNew(
										"supporto@serviziocivile.it",
										richiesta.Email,
										"Credenziali di accesso alla piattaforma DOL – Richiesta accettata",
										$"La tua richiesta di credenziali è stata accettata.<br />Per inserire la password vai a questo <a href=\"{ urlToken}\">link</a>.<br />" +
										$"La nuova password dovrà essere di almeno 12 caratteri, avere almeno una lettera minuscola, almeno una lettera maiuscola, almeno un numero e almeno un carattere non alfanumerico.<br />" +
										$"Si ricorda che per l'accesso occorre inserire il codice fiscale come nome utente",
										"",
										true,
										"",
										true))
									{
										Log.Error(LogEvent.DOMANDA_ONLINE,"Errore nell'invio dell'email delle credenziali");
										throw new Exception("Errore generico invio email");
									}
									scope.Complete();
								}
								catch (Exception e)
								{
									Log.Error(LogEvent.DOMANDA_ONLINE, "Errore nell'invio della email",e);
									return Json(new { success = false, message = $"Errore nell'invio della email:\n {e.Message}" });
								}
							Log.Information(LogEvent.DOMANDA_ONLINE, $"Accettata Domanda token={token.Id} code={token.code}");
							return Json(
								new
								{
									success = true,
								}
							);

						}
					}
				}
				catch (Exception e)
				{
					Log.Error(LogEvent.DOMANDA_ONLINE, "Errore imprevisto",e);
					return Json(new { success = false, message = $"Errore imprevisto:\n {e.Message}" });
				}
			}
		}

		[Authorize(Roles =Role.OPERATORE)]
		[HttpPost]
		public ActionResult Rifiuta(int id, string note)
		{
			using (LogContext.PushProperty("Action", System.Reflection.MethodBase.GetCurrentMethod().Name))
			using (LogContext.PushProperty("Controller", System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name))
			using (LogContext.PushProperty("User", User.Identity.GetUserId()))
			using (LogContext.PushProperty("Id", id))
			using (LogContext.PushProperty("Note", note))
			using (var scope = new TransactionScope())
			{
				RichiestaCredenziali richiesta;
				if (string.IsNullOrEmpty(note))
				{
					return Json(new { success = false, message = "È necessario inserire delle note per il rifiuto" });
				}
				try
				{
					using (var context = new Entities())
					{
						richiesta = context.RichiestaCredenziali.Where(x => x.Id == id).SingleOrDefault();
						if (richiesta == null)
						{
							Log.Error(LogEvent.DOMANDA_ONLINE,$"Errore RichiestaCredenziali con id={id} non trovata");
							return Json(new { success = false, message = "Richiesta non trovata" });
						}
						if (richiesta.IdStato != StatoRichiestaCredenziali.APERTA)
						{
							return Json(new { success = false, message = "Richiesta già elaborata" });
						}
						richiesta.IdStato = StatoRichiestaCredenziali.RIFIUTATA;
						richiesta.NoteApprovazione = note;
						richiesta.UtenteApprovazione = User.Identity.Name;
						richiesta.DataApprovazione = DateTime.Now;
						context.SaveChanges();
					}
				}
				catch (Exception e)
				{
					Log.Error(LogEvent.DOMANDA_ONLINE, "Errore nell'update di RichiestaCredenziali",e);
					return Json(new { success = false, message = $"Errore di accesso ai dati {e.Message}" });
				}
				using (var emailService = new Email.ServiceClient())
					try
					{
						if(!emailService.email(
							"supporto@serviziocivile.it",
							richiesta.Email,
							"Credenziali di accesso alla piattaforma DOL – Richiesta rifiutata",
							$"La tua richiesta di credenziali è stata rifiutata con la seguente motivazione:\n" +
							note +
							$"\n\nProva ad effettuare una nuova registrazione o contatta la casella di posta domandaonline@serviziocivile.it",
							"",
							true,
							""))
						{
							throw new Exception("Errore generico invio email");
						}
						scope.Complete();
					}
					catch (Exception e)
					{
						Log.Error(LogEvent.DOMANDA_ONLINE, "Errore nell'invio della email",e);
						return Json(new { success = false, message = $"Errore nell'invio della email:\n {e.Message}" });
					}
				return Json(
					new
					{
						success = true,
					}
				);
			}
		}

		[Authorize(Roles =Role.OPERATORE)]
		[HttpPost]
		public ActionResult Annulla(int id, string note)
		{
			using (LogContext.PushProperty("Action", System.Reflection.MethodBase.GetCurrentMethod().Name))
			using (LogContext.PushProperty("Controller", System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name))
			using (LogContext.PushProperty("User", User.Identity.GetUserId()))
			using (LogContext.PushProperty("Id", id))
			using (LogContext.PushProperty("Note", note))
			using (var scope = new TransactionScope())
			{
				try
				{
					if (string.IsNullOrEmpty(note))
					{
						return Json(new { success = false, message = "È necessario inserire delle note per l'annullamento" });
					}
					using (var context = new Entities())
					{
						RichiestaCredenziali richiesta = context.RichiestaCredenziali.Where(x => x.Id == id).SingleOrDefault();
						if (richiesta == null)
						{
							Log.Error(LogEvent.DOMANDA_ONLINE,$"Errore RichiestaCredenziali con id={id} non trovata");
							return Json(new { success = false, message = "Richiesta non trovata" });
						}
						richiesta.IdStato = StatoRichiestaCredenziali.ANNULLATA;
						richiesta.NoteAnnullamento = note;
						richiesta.UtenteAnnullamento = User.Identity.Name;
						richiesta.DataAnnullamento = DateTime.Now;
						context.SaveChanges();
					}
					//TODO Invio Mail??
					scope.Complete();
					return Json(
						new
						{
							success = true,
						}
					);
				}
				catch (Exception e)
				{
					Log.Error(LogEvent.DOMANDA_ONLINE, "Errore nell'update di RichiestaCredenziali",e);
					return Json(new { success = false, message = $"Errore di accesso ai dati {e.Message}" });
				}
			}
		}
	}
}