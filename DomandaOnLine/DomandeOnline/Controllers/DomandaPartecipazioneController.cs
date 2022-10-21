using DomandeOnline.Code;
using DomandeOnline.Data;
using DomandeOnline.Exceptions;
using DomandeOnline.Models;
using DomandeOnline.Models.Extensions;
using iTextSharp.text.pdf.parser;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using PdfSharp;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using TheArtOfDev.HtmlRenderer.PdfSharp;

namespace DomandeOnline.Controllers
{

	[Authorize(Roles = Role.UTENTE_CREDENZIALI + "," + Role.UTENTE_SPID)]
	public class DomandaPartecipazioneController : SmartController
	{
		protected string RenderPartialViewToString(string viewName, object model)
		{
			if (string.IsNullOrEmpty(viewName))
				viewName = ControllerContext.RouteData.GetRequiredString("action");

			if (model != null)
				ViewData.Model = model;

			using (StringWriter sw = new StringWriter())
			{
				ViewEngineResult viewResult = ViewEngines.Engines.FindView(ControllerContext, viewName, "_layoutStampa");
				ViewContext viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
				viewResult.View.Render(viewContext, sw);

				return sw.GetStringBuilder().ToString();
			}
		}

		/// <summary>
		/// Restituisce l'id del gruppo del bando scelto
		/// </summary>
		/// <returns>null se non presente nella sessione</returns>
		private int? GetIdGruppo()
		{

			if (System.Web.HttpContext.Current.Session["GruppoSelezionato"] == null)
			{
				using (var context = new Entities())
				{
					var bandi = context.Bando
					.Where(x => DbFunctions.AddDays(x.DataScadenza, x.GiorniPostScadenza ?? 30) >= DateTime.Now);
					if (bandi.Count() == 1)
					{
						return bandi.First().Gruppo;
					}else{
						return null;
					}
				}


			}
			if (!(System.Web.HttpContext.Current.Session["GruppoSelezionato"] is string))
			{
				return null;
			}
			if (!int.TryParse(System.Web.HttpContext.Current.Session["GruppoSelezionato"] as string, out int idGruppo))
			{
				return null;
			}
			return idGruppo;
		}

		private DomandaPartecipazione GetDomanda(Entities context)
		{
			/* Recupero codice fiscale utente */
			//ApplicationUser utente;
			//try
			//{
			//	utente = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>()
			//		.FindById(User.Identity.GetUserId());
			//}
			//catch (Exception e)
			//{
			//	throw new Exception("Errore nella lettura dell'utente dal DB", e);
			//}
			string codiceFiscale = User.Identity.GetCodiceFiscale();
			//if (string.IsNullOrEmpty(utente.CodiceFiscale))
			if (string.IsNullOrEmpty(codiceFiscale))
			{
				throw new AccountException("Utente senza codice fiscale");
			}

			DomandaPartecipazione domanda;
			int? idGruppo = GetIdGruppo();
			if (!context.Bando.Where(x => x.Gruppo == idGruppo && x.DataScadenza > DateTime.Now).Any())
			{
				Log.Information(LogEvent.DOMANDA_ONLINE, $"Operazione effettuata dall'utente a bando scaduto {idGruppo}");
				return null;
			}
			try
			{
				domanda = context.DomandaPartecipazione
				.Where(x => x.CodiceFiscale == /*utente.CodiceFiscale*/ codiceFiscale &&
							x.GruppoBando == idGruppo &&
							x.DataAnnullamento == null)
				.Include(x => x.Progetto)
				.Include(x => x.Progetto.MinoreOpportunita)
				.Include(x => x.Progetto.Programma)
				.Include(x => x.Motivazione)
				.Include(x => x.MotivoAnnullamento)
				.SingleOrDefault();
				if (domanda.Progetto?.Programma?.GaranziaGiovane != null)
				{
					GaranziaGiovani garanziaGiovane = domanda.Progetto?.Programma?.GaranziaGiovane;
					List<Regione> regioniAmmesse = context.RequisitiGaranziaGiovani
						.Where(r => r.TipoGG == garanziaGiovane.Descrizione && r.IdRegione != null)
						.Include(r => r.Regione.Provincia)
						.Select(r => r.Regione)
						.ToList();
					if (regioniAmmesse.Count > 0)
					{
						domanda.Progetto.RegioniAmmesse = regioniAmmesse;
					}
					List<Provincia> provinceAmmesse = context.RequisitiGaranziaGiovani
						.Where(r => r.TipoGG == garanziaGiovane.Descrizione && r.IdProvincia != null)
						.Select(r => r.Provincia)
						.ToList();
					if (provinceAmmesse.Count > 0)
					{
						domanda.Progetto.ProvinceAmmesse = provinceAmmesse;
					}
				}
			}
			catch (Exception e)
			{

				throw new Exception("Errore nella lettura della domanda dal DB", e);
			}

			return domanda;
		}


		/**** Actions ****/
		public ActionResult SelezionaBando()
		{

			try
			{
				using (var context = new Entities())
				{

					var bandi = context.Bando
						.Where(x => DbFunctions.AddDays(x.DataScadenza, x.GiorniPostScadenza ?? 30) >= DateTime.Now);
					if (bandi.Count() == 1)
					{
						return RedirectToAction("Index", new { gruppo = bandi.First().Gruppo });
					}
					return View(bandi.ToList());
				}
			}
			catch (Exception e)
			{
				Log.Error(LogEvent.DOMANDA_ONLINE, "Errore nella lettura del bando dal DB",e);
				return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest, "Errore interno");
			}
		}
        public ActionResult Index(int? gruppo)
		{
			{
				/* Recupero gruppo del bando selezionato (Tramite variabile di sessione) */
				int idGruppo;
				if (gruppo.HasValue)
				{
					/* Viene impostata la variabile di sessione con il gruppo selezionato */
					System.Web.HttpContext.Current.Session["GruppoSelezionato"] = gruppo.ToString();
					idGruppo = gruppo.Value;
				}
				else
				{
					int? idGruppoScelto = GetIdGruppo();
					if (!idGruppoScelto.HasValue)
					{
						return RedirectToAction("SelezionaBando");
					}
					System.Web.HttpContext.Current.Session["GruppoSelezionato"] = idGruppoScelto.ToString();
					idGruppo = idGruppoScelto.Value;
				}

				///* Controllo Se il bando è scaduto */
				//using (var context = new Entities())
				//{
				//	if (!context.Bando.Where(x => x.Gruppo == idGruppo && x.DataScadenza > DateTime.Now).Any())
				//	{
				//		return RedirectToAction("SelezionaBando");
				//	}
				//}
				/* Recupero codice fiscale utente */
				//ApplicationUser utente;
				//try
				//{
				//	utente = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>()
				//		.FindById(User.Identity.GetUserId());
				//}
				//catch (Exception e)
				//{
				//	Log.Error(LogEvent.DOMANDA_ONLINE, "Errore nel recupero del codice fiscale dell'utente",e);
				//	throw e;
				//}
				//if (utente == null)
				//{
				//	return RedirectToAction("LogOff", "Account");
				//}
				//if (string.IsNullOrEmpty(utente.CodiceFiscale))
				if (string.IsNullOrEmpty(User.Identity.GetCodiceFiscale()))
				{
					Log.Error(LogEvent.DOMANDA_ONLINE, "Un utente senza codice fiscale ha acceduto alla pagina della domanda");
					return RedirectToAction("LogOff", "Account");
				}
				/* Controllo età */
				//var eta = GetEta(utente.DataNascita.Value);
				var eta = GetEta(User.Identity.GetDataNascita());
				if (!eta.HasValue)
				{
					Log.Error(LogEvent.DOMANDA_ONLINE, "Un utente senza data di nascita ha acceduto alla pagina della domanda");
					return HttpNotFound("Errore interno");
				}


                //var properties = new { utente.CodiceFiscale, idGruppo };
                var properties = new { CodiceFiscale = User.Identity.GetCodiceFiscale(), IdBando = idGruppo };

				/* Recupero domanda */
				try
				{
					using (var context = new Entities())
					{
						//Codice fiscale utente
						string codiceFiscale = User.Identity.GetCodiceFiscale();
						//ID utente
						string UserID = User.Identity.GetUserId();

						Bando bando = context.Bando.Where(x => x.Gruppo == idGruppo).FirstOrDefault();
						if (bando==null)
						{
							return RedirectToAction("SelezionaBando");
						}

						var domanda = context.DomandaPartecipazione
							.Where(x => x.CodiceFiscale == /*utente.CodiceFiscale*/ codiceFiscale &&
										x.GruppoBando == idGruppo &&
										x.DataAnnullamento == null)
							.Include(x => x.Progetto)
							.Include(x => x.Progetto.MinoreOpportunita)
							.Include(x => x.Progetto.Programma)
							.Include(x => x.Motivazione)
							.Include(x => x.MotivoAnnullamento)
							.SingleOrDefault();
						if (domanda?.Progetto?.Programma?.GaranziaGiovane != null)
						{
							GaranziaGiovani garanziaGiovane = domanda.Progetto?.Programma?.GaranziaGiovane;
							List<Regione> regioniAmmesse = context.RequisitiGaranziaGiovani
								.Where(r => r.TipoGG == garanziaGiovane.Descrizione && r.IdRegione != null)
								.Include(r => r.Regione.Provincia)
								.Select(r => r.Regione)
								.ToList();
							if (regioniAmmesse.Count > 0)
							{
								domanda.Progetto.RegioniAmmesse = regioniAmmesse;
							}
							List<Provincia> provinceAmmesse = context.RequisitiGaranziaGiovani
								.Where(r => r.TipoGG == garanziaGiovane.Descrizione && r.IdProvincia != null)
								.Select(r => r.Provincia)
								.ToList();
							if (provinceAmmesse.Count > 0)
							{
								domanda.Progetto.ProvinceAmmesse = provinceAmmesse;
							}
						}

						/*Se il bando è chiuso ed è stata presentata la domanda e non sono passati i giorni stabiliti viene visualizzata la domanda presentata*/
						if (DateTime.Now.AddDays(-bando.GiorniPostScadenza ?? 30) <= bando.DataScadenza && domanda?.DataPresentazione != null)
						{
							if (DateTime.Now>bando.DataFineAnnullamento)
							{
								ViewData["Scaduto"] = true;
							}
							return View("Domanda", domanda);
						}
						if (DateTime.Now > bando.DataScadenza)
						{
							return View("Domanda", null);
						}

						int etaMassima = 28;
						/* Verifica Volontari COVID */
						if (context.VolontarioInterruzioneCovid
								.Where(x => x.CodiceFiscale == /*utente.CodiceFiscale*/ codiceFiscale && x.AnnoInterruzione==2020)
								.Any())
						{
							etaMassima = 29;
						}

						if ((eta < 18 || eta > etaMassima) && ConfigurationManager.AppSettings["IgnoraControlloEta"] != "true")
						{
							ViewData["Errore"] = $"Non hai i requisiti di età per effettuare la domanda. È richiesta un'età compresa tra 18 e {etaMassima} anni.";
							return View("Errore");
						}

						/* Controlla se l'utente ha già fatto il Servizio Civile (SCU) e Garanzia Giovani (GG) */
						//if (context.VolontarioServizio.Where(x => x.CodiceFiscale == /*utente.CodiceFiscale*/ codiceFiscale).Count() >= 2)
						//{
						//    ViewData["Errore"] = "Hai già effettuato servizio civile";
						//    return View("Errore");
						//}

						/* 
						 * MEV 227
						 * Il bando con ID 63 prevede solo servizio civile
						 */
						if (idGruppo == 63)
						{
							if (context.VolontarioServizio.Any(foo => foo.CodiceFiscale == codiceFiscale && foo.TipoServizio == VolontarioServizio.SERVIZIO_CIVILE))
							{
								ViewData["Errore"] = "Hai già effettuato servizio civile";
								return View("Errore");
							}
						}
						else
						{
                            if (context.VolontarioServizio.Where(x => x.CodiceFiscale == /*utente.CodiceFiscale*/ codiceFiscale).Count() >= 2)
                            {
                                ViewData["Errore"] = "Hai già effettuato servizio civile";
                                return View("Errore");
                            }
                        }

						/*(se non esiste ne crea una nuova)*/
						if (domanda == null)
						{
							domanda = new DomandaPartecipazione()
							{
								CodiceFiscale = /*utente.CodiceFiscale*/ codiceFiscale,
								GruppoBando = idGruppo,
								DataInserimento = DateTime.Now,
								UserIdInserimento = /*utente.Id*/ UserID,
								CV = new CV(),
								Domanda = new Documento()
							};
							context.DomandaPartecipazione.Add(domanda);
							context.SaveChanges();
						}
						ViewData["DatiAnagraficiOK"] = (ControllaDatiAnagrafici(domanda).Count() == 0);
						ViewData["DatiEsperienzeOK"] = (ControllaEsperienze(domanda).Count() == 0);
						ViewData["DatiEsperienzeVuote"] = ControllaDatiEsperienzeVuoti(domanda);
						return View(domanda);
					}
				}
				catch (Exception e)
				{
					Log.Error(LogEvent.DOMANDA_ONLINE, "Errore nel recupero della domanda o nella creazione di una nuova domanda",e,properties);
					throw;
				}
			}
		}

		public ActionResult ScegliProgetto(ScegliProgettoInput parametri)
		{
			var parameters = new { parametri};
			using (var context = new Entities())
			{
				Utils.RimuoviSpazi(parametri);
				DomandaPartecipazione domanda = null;
				try
				{
					domanda = GetDomanda(context);
				}
				catch (AccountException)
				{
					Log.Error(LogEvent.DOMANDA_ONLINE, "Errore nel recupero delle informazioni sull'account");
					return RedirectToAction("LogOff", "Account");
				}
				catch (Exception e)
				{
					throw e;
				}

				if (domanda == null)
				{
					return RedirectToAction("SelezionaBando");
				}
				if (domanda.DataPresentazione != null)
				{
					return RedirectToAction("Index");
				}
				Bando bando= context.Bando.FirstOrDefault(b => b.Gruppo == domanda.GruppoBando);
				parametri.HasProgrammi = bando?.Programmi == true;

                //ID bando 
                parametri.IdBando = domanda.GruppoBando;

				parametri.SoloPreferiti = parametri.SoloPreferiti ?? false;
				parametri.Pagina = parametri.Pagina ?? 1;
				parametri.ElementiPerPagina = parametri.ElementiPerPagina ?? 20;
				if (parametri.CodiceProgetto != System.Web.HttpContext.Current.Session["CodiceProgetto"] as string && Request.HttpMethod == "POST")
				{
					System.Web.HttpContext.Current.Session["CodiceProgetto"] = null;
				}
				if (System.Web.HttpContext.Current.Session["CodiceProgetto"] != null)
				{
					parametri.CodiceProgetto = System.Web.HttpContext.Current.Session["CodiceProgetto"].ToString();
				}


				/* Valori Condizionati */
				/*if (parametri.Misure != "SI")
				{
					parametri.TipoMisure = null;
					parametri.Minori = null;
					parametri.MinoreOpportunita = null;
				}*/
				if (!parametri.GaranziaGiovani ?? false)
				{
					parametri.TipoGaranziaGiovani = null;
				}
				if (parametri.Nazione != "Italia" && parametri.Nazione != null)
				{
					parametri.Comune = null;
					parametri.Provincia = null;
					parametri.Regione = null;
					parametri.TipoMisure = null;
				}
				if (parametri.Regione == null)
				{
					parametri.Provincia = null;
				}
				if (parametri.Provincia == null)
				{
					parametri.Comune = null;
				}

				/* Recupero codice fiscale utente */
				string codiceFiscale;
				try
				{
					//codiceFiscale = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>()
					//	.FindById(User.Identity.GetUserId())
					//	.CodiceFiscale;
					codiceFiscale = User.Identity.GetCodiceFiscale();
				}
				catch (AccountException)
				{
					Log.Error(LogEvent.DOMANDA_ONLINE, "Errore nel recupero delle informazioni sull'account");
					return RedirectToAction("LogOff", "Account");
				}
				catch (Exception e)
				{
					Log.Error(LogEvent.DOMANDA_ONLINE, "Errore nel recupero del codice fiscale dell'utente", e, parameters: parameters);
					throw;
				}

                {
                    /* Recupera tutte le tipologie di servizio civile già fatte dall'utente (Cittadino) */
                    List<string> serviziEffettuati;

					try
					{
						serviziEffettuati = context.VolontarioServizio
									.Where(x => x.CodiceFiscale == codiceFiscale)
									.Select(x => x.TipoServizio)
									.ToList();
					}
					catch (Exception e)
					{
						Log.Error(LogEvent.DOMANDA_ONLINE, "Errore nel recupero dei servizi effettuati", e, parameters: parameters);
						throw;
					}

					bool effettuatoServizioCivile = serviziEffettuati.Any(s => s == VolontarioServizio.SERVIZIO_CIVILE);
					bool effettuatoGaranziaGiovani = serviziEffettuati.Any(s => s == VolontarioServizio.GARANZIA_GIOVANI);
					ViewData["EffettuatoServizioCivile"] = effettuatoServizioCivile;
					ViewData["EffettuatoGaranziaGiovani"] = effettuatoGaranziaGiovani;

					/* 
					 * MEV 227
					 * Se l'utente ha fatto il servizio civile (SCU) non può presentare domanda perchè il bando co ID 63
					 * non prevede la Garanzia Giovani (GG)
					 */
					if (parametri.IdBando == 63)
                    {
                        //Carica lista tipi programma disponibili per il bando con ID 63 
                        parametri.ListaTipologiaProgramma = new List<TipologiaProgramma>();
                        parametri.ListaTipologiaProgramma = GetTipologiaProgramma(parametri.TipologiaProgrammaScelta);
					}
					else
                    {
						if (effettuatoServizioCivile)
						{
							parametri.GaranziaGiovani = true;
						}
						else if (effettuatoGaranziaGiovani)
						{
							parametri.GaranziaGiovani = false;
						}
					}

					try
					{
						/* Verifico se aggiungere preferiti*/
						if (!string.IsNullOrEmpty(parametri.AggiungiPreferito))
						{
							string[] split = parametri.AggiungiPreferito.Split('_');
							if (split.Count() != 2)
							{
								Log.Error(LogEvent.DOMANDA_ONLINE, "Parametri per aggiungere il preferito non corretti", parameters: parameters);
								return HttpNotFound("Parametri per aggiungere il preferito non corretti");
							}
							string codiceProgettoPreferito = split[0];
							if (!int.TryParse(split[1], out int codiceSedePreferito))
							{
								Log.Error(LogEvent.DOMANDA_ONLINE, "Parametri per aggiungere il preferito non corretti", parameters: parameters);
								return HttpNotFound("Parametri per aggiungere il preferito non corretti");
							}
							/* Verifico esistenza del preferito */
							var preferito = context.ProgettoPreferito
								.Where(p => p.CodiceFiscale == codiceFiscale &&
									   p.CodiceProgetto == codiceProgettoPreferito &&
									   p.CodiceSede == codiceSedePreferito)
								.SingleOrDefault();
							/* Se non già presente lo inserisco */
							if (preferito == null)
							{
								context.ProgettoPreferito.Add(new ProgettoPreferito()
								{
									CodiceFiscale = codiceFiscale,
									CodiceProgetto = codiceProgettoPreferito,
									CodiceSede = codiceSedePreferito
								});
								context.SaveChanges();
								Log.Information(LogEvent.DOMANDA_ONLINE, $"Impostato progetto preferito", 
									parameters: new { Progetto = codiceProgettoPreferito, CodiceSede = codiceSedePreferito });
							}
						}
					}
					catch (Exception e)
					{
						Log.Error(LogEvent.DOMANDA_ONLINE, "Errore nel inserimento tra i preferiti", e, parameters: parameters);
						throw;
					}
					/* Verifico se rimuovere preferiti*/
					try
					{
						if (!string.IsNullOrEmpty(parametri.RimuoviPreferito))
						{
							string[] split = parametri.RimuoviPreferito.Split('_');
							if (split.Count() != 2)
							{
								Log.Error(LogEvent.DOMANDA_ONLINE, "Parametri per rimuovere il preferito non corretti", parameters: parameters);
								return HttpNotFound("Parametri per rimuovere il preferito non corretti");
							}
							string codiceProgettoPreferito = split[0];
							if (!int.TryParse(split[1], out int codiceSedePreferito))
							{
								Log.Error(LogEvent.DOMANDA_ONLINE, "Parametri per rimuovere il preferito non corretti", parameters: parameters);
								return HttpNotFound("Parametri per rimuovere il preferito non corretti");
							}
							/* Verifico esistenza del preferito */
							var preferito = context.ProgettoPreferito
								.Where(p => p.CodiceFiscale == codiceFiscale &&
									   p.CodiceProgetto == codiceProgettoPreferito &&
									   p.CodiceSede == codiceSedePreferito)
								.SingleOrDefault();
							/* Se esiste lo elimino */
							if (preferito != null)
							{
								context.ProgettoPreferito.Remove(preferito);
								context.SaveChanges();
								Log.Information(LogEvent.DOMANDA_ONLINE, "Eliminato progetto preferito: Progetto:{codiceProgettoPreferito} Codice Sede:{codiceSedePreferito}", parameters: parameters);
							}
						}
					}
					catch (Exception e)
					{
						Log.Error(LogEvent.DOMANDA_ONLINE, "Errore nella rimozione tra i preferiti", e, parameters: parameters);
						throw;
					}
					/* Recupero nazioni dai progetti (sono salvate nei campi comune, regione e provincia)*/
					try
					{
						var nazioni = context.Progetto
							.Where(p => p.TipoProgetto == Data.Progetto.Tipo.ESTERO)
							.Select(p => p.Regione)
							.Distinct();
						ViewData["Nazioni"] = nazioni.ToList();

						/* Recupero Regioni, provincie e comuni */
						IQueryable<ComuneProgetti> regioni = context.ComuneProgetti.AsQueryable();
						IQueryable<ComuneProgetti> provincie = regioni;
						IQueryable<ComuneProgetti> comuni = regioni;
						/* Filtro comuni e provincie se selezionata una regione */
						if (!string.IsNullOrEmpty(parametri.Regione))
						{
							provincie = provincie.Where(x => x.REGIONE == parametri.Regione);
						}
						else
						{
							provincie = provincie.Where(x => false);
						}
						/* Filtro comuni se selezionata una provincia*/
						if (!string.IsNullOrEmpty(parametri.Provincia))
						{
							comuni = comuni.Where(x => x.PROVINCIA == parametri.Provincia);
						}
						else
						{
							comuni = comuni.Where(x => false);
						}
						/* Imposto le viewdata con le liste dei comuni, provincie e regioni */
						ViewData["Regioni"] = regioni
							.OrderBy(x => x.REGIONE)
							.Select(x => x.REGIONE)
							.Distinct()
							.ToList();
						ViewData["Provincie"] = provincie
							.OrderBy(x => x.PROVINCIA)
							.Select(x => x.PROVINCIA)
							.Distinct()
							.ToList();
						ViewData["Comuni"] = comuni
							.OrderBy(x => x.COMUNE)
							.Select(x => x.COMUNE)
							.Distinct()
							.ToList();
					}
					catch (Exception e)
					{
						Log.Error(LogEvent.DOMANDA_ONLINE, "Errore nel Recupero dei comuni, provincie e regioni", e, parameters: parameters);
						throw;
					}
					/* Recupero Settori*/
					try
					{
						ViewData["Settori"] = context.Progetto
							.Where(x => x.Gruppo == domanda.GruppoBando)
							.OrderBy(x => x.Settore)
							.Select(x => x.Settore)
							.Distinct()
							.ToList();
					}
					catch (Exception e)
					{
						Log.Error(LogEvent.DOMANDA_ONLINE, "Errore nel Recupero dei settori", e, parameters: parameters);
						throw;
					}
					/* Recupero Aree*/
					try
					{
						var aree = context.Progetto.AsQueryable();
						if (!string.IsNullOrEmpty(parametri.Settore))
						{
							aree = aree.Where(x => x.Settore == parametri.Settore);
						}
						ViewData["Aree"] = aree
							.OrderBy(x => x.Area)
							.Select(x => x.Area)
							.Distinct()
							.ToList();
					}
					catch (Exception e)
					{
						Log.Error(LogEvent.DOMANDA_ONLINE, "Errore nel Recupero dei settori", e, parameters: parameters);
						throw;
					}
					/* Recupero Ambiti programmi*/
					try
					{
						var ambiti = context.Ambito.AsQueryable();
						ViewData["Ambiti"] = ambiti
							.OrderBy(x => x.Descrizione)
							.Distinct()
							.ToList();
					}
					catch (Exception e)
					{
						Log.Error(LogEvent.DOMANDA_ONLINE, "Errore nel Recupero degli ambiti", e, parameters: parameters);
						throw;
					}
					/* Recupero Obiettivi programmi*/
					try
					{
						var obiettivi = context.Obiettivo.AsQueryable();
						ViewData["Obiettivi"] = obiettivi
							.OrderBy(x => x.Descrizione)
							.Distinct()
							.ToList();
					}
					catch (Exception e)
					{
						Log.Error(LogEvent.DOMANDA_ONLINE, "Errore nel Recupero degli obiettivi", e, parameters: parameters);
						throw;
					}
					/* Recupero Minori Opportunità*/
					try
					{
						var minoreOpportunita = context.MinoreOpportunita.AsQueryable();
						ViewData["MinoriOpportunita"] = minoreOpportunita
							.OrderBy(x => x.Descrizione)
							.ToList();
					}
					catch (Exception e)
					{
						Log.Error(LogEvent.DOMANDA_ONLINE, "Errore nel Recupero deglle minori opportunità", e, parameters: parameters);
						throw;
					}
					/* Recupero Garanzia Giovani*/
					try
					{
						var programmiBando = context.Progetto.Where(x=>x.Gruppo==bando.Gruppo).Select(x=>x.IdProgramma);
						var programma = context.Programma.AsQueryable();
						var garanzieGiovani = programma
							.Where(x => x.IdTipoGG != null && programmiBando.Contains(x.IdProgramma))
							.Select(x => x.GaranziaGiovane.Descrizione)
							.Distinct()
							.ToList();
						ViewData["GaranzieGiovane"] = garanzieGiovani;

					}
					catch (Exception e)
					{
						Log.Error(LogEvent.DOMANDA_ONLINE, "Errore nel Recupero delle garanzie giovani", e, parameters: parameters);
						throw;
					}
					/* Recupero progetti*/
					try
					{
						var progetti = from p in context.Progetto
									   where p.Gruppo == domanda.GruppoBando &&
												p.DataAnnullamento == null
									   select p;
						/* Applicazione filtri */
						if (parametri.SoloPreferiti.Value)
						{
							progetti = progetti.Where(x => x.Preferiti.Any(p => p.CodiceFiscale == codiceFiscale));
						}
						if (!string.IsNullOrEmpty(parametri.CodiceProgetto))
						{
							progetti = progetti.Where(x => x.CodiceProgetto == parametri.CodiceProgetto);
						}
						if (!string.IsNullOrEmpty(parametri.NomeProgetto))
						{
							progetti = progetti.Where(x => x.TitoloProgetto.Contains(parametri.NomeProgetto));
						}

						if (!string.IsNullOrEmpty(parametri.Regione))
						{
							progetti = progetti.Where(x => x.Regione == parametri.Regione);
						}
						if (!string.IsNullOrEmpty(parametri.Provincia))
						{
							progetti = progetti.Where(x => x.Provincia == parametri.Provincia);
						}
						if (!string.IsNullOrEmpty(parametri.Comune))
						{
							progetti = progetti.Where(x => x.Comune == parametri.Comune);
						}

						/* Se Italia applico filtri su comuni, provincie e regioni*/
						if (parametri.Nazione == "Italia")
						{
							progetti = progetti.Where(x => x.TipoProgetto == Data.Progetto.Tipo.ITALIA);

						}
						else if (parametri.Nazione == "Estero")
						{
							progetti = progetti.Where(x => x.TipoProgetto == Data.Progetto.Tipo.ESTERO);
						}
						else if (parametri.Nazione != null)//Scelto un paese specifico
						{
							progetti = progetti.Where(x => x.Regione == parametri.Nazione);
						}
						if (!string.IsNullOrEmpty(parametri.Settore))
						{
							progetti = progetti.Where(x => x.Settore == parametri.Settore);
						}
						if (!string.IsNullOrEmpty(parametri.Area))
						{
							progetti = progetti.Where(x => x.Area == parametri.Area);
						}
						if (!string.IsNullOrEmpty(parametri.CodiceEnte))
						{
							progetti = progetti.Where(x => x.CodiceEnte == parametri.CodiceEnte);
						}
						if (!string.IsNullOrEmpty(parametri.Ente))
						{
							progetti = progetti.Where(x => x.NomeEnte.Contains(parametri.Ente) || x.EnteAttuatore.Contains(parametri.Ente));
						}
						if (!string.IsNullOrEmpty(parametri.Misure))
						{
							progetti = progetti.Where(x => x.Misure == parametri.Misure);
						}
						if (parametri.Minori == "SI")
						{
							progetti = progetti.Where(x => x.NumeroGiovaniMinoriOpportunità > 0);
						}
						else if (parametri.Minori == "NO")
						{
							progetti = progetti.Where(x => x.NumeroGiovaniMinoriOpportunità == 0);
						}
						if (parametri.MinoreOpportunita.HasValue)
						{
							progetti = progetti.Where(x => x.IDParticolaritàEntità == parametri.MinoreOpportunita.Value);
						}
						switch (parametri.TipoMisure)
						{
							case "N":
								progetti = progetti.Where(x => x.EsteroUE == "NO" && x.Tutoraggio == "NO");
								break;
							case "E":
								progetti = progetti.Where(x => x.EsteroUE == "SI" && x.Tutoraggio == "NO");
								break;
							case "T":
								progetti = progetti.Where(x => x.EsteroUE == "NO" && x.Tutoraggio == "SI");
								break;
							default:
								break;
						}

						//Filtri Programma
						if (!string.IsNullOrEmpty(parametri.Programma) ||
							parametri.Ambito.HasValue ||
							parametri.Obiettivo.HasValue ||
							parametri.GaranziaGiovani==true ||
							parametri.IsDigitale.HasValue ||
							!string.IsNullOrEmpty(parametri.TipoGaranziaGiovani)
                            || parametri.IdBando == 63)
						{
							var programmi = context.Programma.AsQueryable();
							if (!string.IsNullOrEmpty(parametri.Programma))
								programmi = programmi
									.Where(p => p.Titolo.Contains(parametri.Programma));
							if (parametri.Ambito.HasValue)
							{
								programmi = programmi
									.Where(p => p.IdAmbitoAzione == parametri.Ambito);
							}
							if (parametri.Obiettivo.HasValue)
							{
								programmi = programmi
									.Where(p => p.Obiettivi.Select(o => o.IdObiettivo).Contains(parametri.Obiettivo.Value));
							}
							if (parametri.GaranziaGiovani.HasValue)
							{
								if (parametri.GaranziaGiovani.Value)
								{
									programmi = programmi
										.Where(p => p.IdTipoGG != null);
								}
								else
								{
									programmi = programmi
										.Where(p => p.IdTipoGG == null);
								}
							}
							if (!string.IsNullOrEmpty(parametri.TipoGaranziaGiovani))
							{
								var garanziaGiovani = context.GaranziaGiovani
									.Where(g => g.Descrizione == parametri.TipoGaranziaGiovani)
									.Select(g => g.IdTipoGG);
								programmi = programmi.Where(p => garanziaGiovani.Contains(p.IdTipoGG.Value));
							}
							if (parametri.IsDigitale.HasValue)
							{
								programmi = programmi.Where(p => p.IsDigitale==parametri.IsDigitale);
							}

                            //Filtro ricerca gestito solo per bando con ID 63
                            if (parametri.IdBando == 63)
                            {
								//Controlla se l'utente ha scelto una tipologia di programma
								if (string.IsNullOrEmpty(parametri.TipologiaProgrammaScelta))
								{ 
									//Non è stata scelta alcuna tipologia di programma. Si escludono i progetti ai quali 
									//l'utente ha già svolto il servizio civile. Si prende come riferimento la lista dei tipi di servizio 
									//civile già svolti
									if (serviziEffettuati.Contains(VolontarioServizio.SERVIZIO_CIVILE_AMBIENTALE))
                                    {
										programmi = programmi.Where(foo => foo.IsAmbientale == false);
									}

									if (serviziEffettuati.Contains(VolontarioServizio.SERVIZIO_CIVILE_DIGITALE))
									{
										programmi = programmi.Where(foo => foo.IsDigitale == false);
									}

									if (serviziEffettuati.Contains(VolontarioServizio.SERVIZIO_CIVILE))
									{
										programmi = programmi.Where(foo => foo.IsAutofinanziato == false);
									}
								}
								else
                                {
									//E' stata scelta una tipologia di programma. Si estraggono solo i progetti associati ai programmi
									//che hanno la stessa tipologia selezionata dall'utente
									switch (parametri.TipologiaProgrammaScelta)
									{
										case VolontarioServizio.SERVIZIO_CIVILE_AMBIENTALE:
											programmi = programmi.Where(foo => foo.IsAmbientale == true);
											break;
										case VolontarioServizio.SERVIZIO_CIVILE_DIGITALE:
											programmi = programmi.Where(foo => foo.IsDigitale == true);
											break;
										case VolontarioServizio.SERVIZIO_CIVILE:
											programmi = programmi.Where(foo => foo.IsAutofinanziato == true);
											break;
										default:
											break;
									}
								}
                            }

                            progetti = progetti.Where(p => programmi.Select(programma => programma.IdProgramma).Contains(p.IdProgramma.Value));
                        }

                        /*Gestione paginazione*/
                        parametri.Pagina = parametri.NuovaPagina ?? parametri.Pagina;
						int numeroElementi = progetti.Count();
						int numeroPagine = (numeroElementi / parametri.ElementiPerPagina.Value) + (numeroElementi % parametri.ElementiPerPagina == 0 ? 0 : 1);
						if (parametri.Pagina > numeroPagine) parametri.Pagina = numeroPagine;
						if (parametri.Pagina < 1) parametri.Pagina = 1;

						/* parametri paginazione */
						ViewData["Pagina"] = parametri.Pagina;
						ViewData["NumeroPagine"] = numeroPagine;
						ViewData["ElementiPerPagina"] = parametri.ElementiPerPagina;
						ViewData["NumeroElementi"] = numeroElementi;

						TempData["Gruppo"] = domanda.GruppoBando;

						/* Campi ricerca (per valorizzare le ricerche fatte)*/
						TempData["Parametri"] = parametri;

						if (parametri.ScrollPosition.HasValue)
						{
							ViewData["ScrollTo"] = parametri.ScrollPosition;
						}
						ViewData["Domanda"] = domanda;
						var progettiList = progetti.Select(p => new ProgettoDto()
						{
							CodiceProgetto = p.CodiceProgetto,
							TipoProgetto = p.TipoProgetto,
							CodiceSede = p.CodiceSede,
							IndirizzoSede = p.IndirizzoSede,
							TitoloProgetto = p.TitoloProgetto,
							Area = p.Area,
							CodiceEnte = p.CodiceEnte,
							NomeEnte = p.NomeEnte,
							Regione = p.Regione,
							Provincia = p.Provincia,
							Comune = p.Comune,
							Settore = p.Settore,
							Misure = p.Misure,
							NumeroGiovaniMinoriOpportunità = p.NumeroGiovaniMinoriOpportunità,
							EsteroUE = p.EsteroUE,
							Tutoraggio = p.Tutoraggio,
							Preferito = p.Preferiti.Any(x => x.CodiceFiscale == codiceFiscale),
							NumeroDomande = p.DomandePartecipazione.Where(
								  x => x.DataPresentazione != null &&
									   !x.DataAnnullamento.HasValue
								  ).Count(),
							//NumeroDomande= p.NumeroDomande??0,
							IdTipoGG = p.Programma.IdTipoGG,
							EnteAttuatore = p.EnteAttuatore
						})
							.OrderBy(p => p.CodiceProgetto)
							.ThenBy(p => p.CodiceSede)
							.Skip((parametri.Pagina.Value - 1) * parametri.ElementiPerPagina.Value)
							.Take(parametri.ElementiPerPagina.Value)
							.ToList();
						Log.Information(LogEvent.DOMANDA_ONLINE, "Ricercati Progetti", parameters: parameters);
						return View(progettiList);
                    }
                    catch (Exception e)
					{
						Log.Error(LogEvent.DOMANDA_ONLINE, "Errore nel Recupero dei progetti", e, parameters: parameters);
						throw;
					}
                }
            }
        }

        /// <summary>
        /// Visualizza i dettagli di un progetto
        /// </summary>
        /// <param name="codiceProgetto">Identificativo del progetto</param>
        /// <param name="codiceSede">Identificativo della sede</param>
        /// <returns></returns>
        public ActionResult Progetto(string codiceProgetto, int codiceSede)
		{

			try
			{
				using (var context = new Entities())
				{
					var progetto = context.Progetto
						.Where(x => x.CodiceProgetto == codiceProgetto && x.CodiceSede == codiceSede)
						.Include(x => x.Programma)
						.Include(x => x.Programma.Obiettivi)
						.Include(x => x.Programma.Ambito)
						.Include(x => x.Programma.GaranziaGiovane)
						.Include(x => x.MinoreOpportunita)
						.SingleOrDefault();
					if (progetto.NumeroGiovaniMinoriOpportunità > 0)
					{
						var numeroMinoriOpportunita = context.DomandaPartecipazione
								.Where(d => d.CodiceProgettoSelezionato == codiceProgetto &&
											d.CodiceSedeSelezionata == codiceSede &&
											d.DichiarazioneMinoriOpportunita == true &&
											d.DataPresentazione.HasValue &&
											!d.DataAnnullamento.HasValue)
							.Count();
						ViewData["numeroMinoriOpportunita"] = numeroMinoriOpportunita;
					}
					if (progetto == null)
					{
						Log.Error(LogEvent.DOMANDA_ONLINE, "L'utente ha tentato a visualizzare i dettagli di un progetto inesistente");
						return HttpNotFound("Progetto non trovato");
					}
					return View(progetto);
				}
			}
			catch (Exception e)
			{
				Log.Error(LogEvent.DOMANDA_ONLINE, "Errore nella lettura del Progetto dal DB", e);
				return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest, "Errore interno");
			}
		}

		public ActionResult SelezionaProgetto(string codiceProgetto, int codiceSede)
		{
			var parameters = new { codiceProgetto, codiceSede };
			using (var context = new Entities())
			{

				/* Recupero gruppo del Bando */
				if (!int.TryParse(System.Web.HttpContext.Current.Session["GruppoSelezionato"] as string, out int idGruppo))
				{
					return RedirectToAction("SelezionaBando");
				}
				/* Recupero progetto */
				Progetto progetto;
				try
				{
					progetto = context.Progetto
						.Where(x => x.CodiceProgetto == codiceProgetto && x.CodiceSede == codiceSede)
						.SingleOrDefault();
					if (progetto == null)
					{
						Log.Error(LogEvent.DOMANDA_ONLINE, "L'utente ha selezionato un progetto inesistente",parameters: parameters);
						return HttpNotFound("Progetto non trovato");
					}
				}
				catch (Exception e)
				{
					Log.Error(LogEvent.DOMANDA_ONLINE, "Errore nella lettura del progetto", e, parameters: parameters);
					throw;
				}
				/* Recupero informazioni utente */
				ApplicationUser utente;
				try
				{
					utente = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>()
						.FindById(User.Identity.GetUserId());
				}
				catch (Exception e)
				{
					Log.Error(LogEvent.DOMANDA_ONLINE, "Errore nel recupero delle informazioni sull'utente", e, parameters: parameters);
					throw;
				}
				/* Recupero domanda corrente e associazione progetto*/
				try
				{

					DomandaPartecipazione domanda = null;
					try
					{
						domanda = GetDomanda(context);
					}
					catch (AccountException)
					{
						Log.Error(LogEvent.DOMANDA_ONLINE, "Errore nel recupero delle informazioni sull'account");
						return RedirectToAction("LogOff", "Account");
					}
					catch (Exception e)
					{
						throw e;
					}
					if (domanda == null)
					{
						return RedirectToAction("SelezionaBando");
					}
					if (domanda.DataPresentazione != null)
					{
						Log.Error(LogEvent.DOMANDA_ONLINE, "L'utente ha tentato di modificare un progetto ad una domanda già presentata",parameters: parameters);
						return HttpNotFound("Domanda già presentata");
					}
					/* Aggiunta preferiti */
					/* Verifico esistenza del preferito */
					var preferito = context.ProgettoPreferito
						.Where(p => p.CodiceFiscale == utente.CodiceFiscale &&
							   p.CodiceProgetto == codiceProgetto &&
							   p.CodiceSede == codiceSede)
						.SingleOrDefault();
					/* Se non già presente lo inserisco */
					if (preferito == null)
					{
						context.ProgettoPreferito.Add(new ProgettoPreferito()
						{
							CodiceFiscale = utente.CodiceFiscale,
							CodiceProgetto = codiceProgetto,
							CodiceSede = codiceSede
						});
					}


					domanda.CodiceProgettoSelezionato = progetto.CodiceProgetto;
					domanda.CodiceSedeSelezionata = progetto.CodiceSede;
					domanda.DichiarazioneMinoriOpportunita = null;
					domanda.DichiarazioneRequisitiGaranziaGiovani = null;
					domanda.AlternativaRequisitiGaranziaGiovani = null;
					if (!progetto.Asse1bisDisoccupati)
					{
						domanda.DataDIDGaranziaGiovani = null;
						domanda.LuogoDIDGaranziaGiovani = null;
					}
					if ((!progetto.Asse1bisDisoccupati && !progetto.Asse1NEET) || domanda.AlternativaRequisitiGaranziaGiovani==true)
					{
						domanda.DataPresaInCaricoGaranziaGiovani = null;
						domanda.LuogoPresaInCaricoGaranziaGiovani = null;
						domanda.DichiarazioneResidenzaOK = null;
					}

					domanda.UserIdModifica = User.Identity.GetUserId();
					domanda.DataModifica = DateTime.Now;
					context.SaveChanges();
					Log.Information(LogEvent.DOMANDA_ONLINE, "Selezionato progetto", parameters: parameters);

					return RedirectToAction("Index");
				}
				catch (Exception e)
				{
					Log.Error(LogEvent.DOMANDA_ONLINE, "Errore nell associazione del progetto alla domanda", e, parameters: parameters);
					return HttpNotFound("Errore interno");
				}
			}
		}

		public ActionResult DomandePresentate()
		{
			using (var context = new Entities())
			{
				ApplicationUser utente;
				try
				{
					utente = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>()
						.FindById(User.Identity.GetUserId());
				}
				catch (Exception e)
				{
					Log.Error(LogEvent.DOMANDA_ONLINE, "Errore nella lettura dell'utente dal DB", e);
					return RedirectToAction("SelezionaBando");
				}
				if (string.IsNullOrEmpty(utente.CodiceFiscale))
				{
					Log.Error(LogEvent.DOMANDA_ONLINE, "Utente senza codice fiscale");
					return RedirectToAction("SelezionaBando");
				}
				try
				{
					List<Domanda> domande = (from
													d in context.DomandaPartecipazione
												join
										b in context.Bando on d.GruppoBando equals b.Gruppo
												where
													d.CodiceFiscale == utente.CodiceFiscale &&
													d.DataPresentazione != null
												orderby
													d.DataPresentazione descending
												select new Domanda
												{
													IdDomanda = d.Id,
													Bando = b.Descrizione,
													DataPresentazione = d.DataPresentazione.Value
												}
									).ToList();
					return View(domande);
				}
				catch (Exception e)
				{
					Log.Error(LogEvent.DOMANDA_ONLINE,"Errore nella lettura delle domande dal DB", e);
					return RedirectToAction("SelezionaBando");
				}
			}
		}


		public ActionResult DatiAnagrafici()
		{
			using (var context = new Entities())
			{
				DomandaPartecipazione domanda = null;
				try
				{
					domanda = GetDomanda(context);
				}
				catch (AccountException)
				{
					Log.Error(LogEvent.DOMANDA_ONLINE, "Errore nel recupero delle informazioni sull'account");
					return RedirectToAction("LogOff", "Account");
				}
				catch (Exception e)
				{
					throw e;
				}
				if (domanda == null)
				{
					return RedirectToAction("SelezionaBando");
				}
				if (domanda.DataPresentazione != null)
				{
					return RedirectToAction("Index");
				}
				Nazione nazioneCittadinanza = context.Nazione.Where(x => x.Nome == domanda.Cittadinanza).SingleOrDefault();
				int? IdNazioneCittadinanza = nazioneCittadinanza == null ? null : (int?)nazioneCittadinanza.Id;
				return DatiAnagrafici(new DatiAnagraficiInput()
				{
					IdNazioneCittadinanza = IdNazioneCittadinanza,
					ComuneResidenza = domanda.ComuneResidenza,
					ProvinciaResidenza = domanda.ProvinciaResidenza,
					ViaResidenza = domanda.ViaResidenza,
					CivicoResidenza = domanda.CivicoResidenza,
					CapResidenza = domanda.CapResidenza,
					ComuneRecapito = domanda.ComuneRecapito,
					ProvinciaRecapito = domanda.ProvinciaRecapito,
					ViaRecapito = domanda.ViaRecapito,
					CivicoRecapito = domanda.CivicoRecapito,
					CapRecapito = domanda.CapRecapito,
					IdTitoloStudio = domanda.IdTitoloStudio,
					//CodiceMinoriOpportunita = domanda.CodiceMinoriOpportunita,
					DichiarazioneMinoriOpportunita = domanda.DichiarazioneMinoriOpportunita,
					IdMotivazione = domanda.IdMotivazione,
					CodiceDichiarazioneCittadinanza = domanda.CodiceDichiarazioneCittadinanza,
					NonCondanneOk = domanda.NonCondanneOk,
					TrasferimentoSedeOk = domanda.TrasferimentoSedeOk,
					TrasferimentoProgettoOk = domanda.TrasferimentoProgettoOk,
					AltreDichiarazioniOk = domanda.AltreDichiarazioniOk,
					PrivacyPresaVisione = domanda.PrivacyPresaVisione,
					PrivacyConsenso = domanda.PrivacyConsenso,
					ConfermaResidenza = domanda.ConfermaResidenza,
					ResidenzaEstera = domanda.ResidenzaEstera,
					NazioneResidenza = domanda.NazioneResidenza,
					ResidenzaIndirizzoCompleto = domanda.IndirizzoCompletoResidenza
				}
				);
			}
		}

		[HttpPost]
		public ActionResult DatiAnagrafici(DatiAnagraficiInput parametri)
		{
			var parameters = new { parametri };
			using (var context = new Entities())
			{

				var nazioni = context.Nazione.OrderBy(x => x.Nome);
				Nazione cittadinanza = nazioni.Where(x => x.Id == parametri.IdNazioneCittadinanza).SingleOrDefault();
				DomandaPartecipazione domanda = null;
				try
				{
					domanda = GetDomanda(context);
				}
				catch (AccountException)
				{
					Log.Error(LogEvent.DOMANDA_ONLINE, "Errore nel recupero delle informazioni sull'account");
					return RedirectToAction("LogOff", "Account");
				}
				catch (Exception e)
				{
					throw e;
				}
				if (domanda.DataPresentazione != null)
				{
					Log.Error(LogEvent.DOMANDA_ONLINE, "L'utente ha tentato di modificare i dati dopo aver presentato la domanda", parameters: parameters);
					return RedirectToAction("SelezionaBando");
				}
				var provincie = context.Provincia.OrderBy(x => x.Nome);

				/* Gestione Conferma Residenza*/
				if (parametri.Action == "ConfermaResidenza")
				{
					parametri.ConfermaResidenza = true;
					parametri.Action = "salva";
				}

				/* Gestione Residenza */
				if (true && !(parametri.ConfermaResidenza ?? false))
				{
					//ApplicationUser utente = Utils.GetUser(User.Identity.Name);
					Indirizzo indirizzo = Utils.GetIndirizzo(User.Identity.GetIndirizzo());

					if (indirizzo != null)
					{
						parametri.ViaResidenza = parametri.ViaResidenza ?? indirizzo.Via;
						parametri.CivicoResidenza = parametri.CivicoResidenza ?? indirizzo.Civico;
						parametri.ComuneResidenza = parametri.ComuneResidenza ?? indirizzo.Comune;
						parametri.ProvinciaResidenza = parametri.ProvinciaResidenza ?? indirizzo.Provincia;
						parametri.CapResidenza = parametri.CapResidenza ?? indirizzo.CAP;
					}
					else //Nel caso non sia stato possibile calcolare la residenza imposto come se avesse confermato i dati
					{
						parametri.ConfermaResidenza = true;
					}
				}
				else if (User.IsInRole(Role.UTENTE_CREDENZIALI))
				{
					parametri.ConfermaResidenza = true;
				}
				if (parametri.ResidenzaEsteraButton.HasValue)
				{
					parametri.ResidenzaEstera = parametri.ResidenzaEsteraButton;
				}

				if (parametri.ResidenzaEstera ?? false)
				{
					parametri.ComuneResidenza = null;
					parametri.ProvinciaResidenza = null;
					parametri.ViaResidenza = null;
					parametri.CivicoResidenza = null;
					parametri.CapResidenza = null;
				}
				else
				{
					parametri.ResidenzaIndirizzoCompleto = null;
					parametri.NazioneResidenza = null;
				}

				var comuni = context.Comune
					.Where(x => x.Provincia.Nome == parametri.ProvinciaResidenza)
					.OrderBy(x => x.Nome);

				if (domanda == null)
				{
					return RedirectToAction("SelezionaBando");
				}
				if (domanda.DataPresentazione != null)
				{
					Log.Error(LogEvent.DOMANDA_ONLINE, "L'utente ha tentato di modificare una domanda già presentata", parameters: parameters);
					return HttpNotFound("Domanda già presentata");
				}



				if (User.IsInRole(Role.UTENTE_CREDENZIALI))
				{
					parametri.IdNazioneCittadinanza = null;
				}

				if (!domanda.Progetto?.IDParticolaritàEntità.HasValue ?? false || (domanda.Progetto?.NumeroGiovaniMinoriOpportunità == 0))
				{
					parametri.DichiarazioneMinoriOpportunita = null;
				}

				//Tiolo Di studio
				if (parametri.IdTitoloStudio == TitoloStudio.NESSUNO || parametri.IdTitoloStudio == TitoloStudio.ESTERO)
				{
					parametri.FormazioneAnagraficaDisciplina = null;
					parametri.FormazioneAnagraficaItalia = null;
					parametri.FormazioneAnagraficaAnno = null;
					parametri.FormazioneAnagraficaIstituto = null;
					parametri.FormazioneAnagraficaItalia = null;
					parametri.FormazioneAnagraficaEnte = null;
				}

				//Garanzia Giovani
				if (domanda.Progetto?.Programma?.IdTipoGG != null)
				{

					if (!domanda.Progetto.Asse1NEET)
					{
						parametri.DichiarazioneRequisitiGaranziaGiovani = null;
					}
					else
					{
						List<string> provinceAmmesse = domanda.Progetto.ProvinceAmmesse.Select(p => p.Nome).ToList();
						ViewData["ProvinceVietate"] = "\"" + string.Join("\",\"", provinceAmmesse) + "\"";
					}
					if (!domanda.Progetto.Asse1bisDisoccupati)
					{
						parametri.DataDIDGaranziaGiovaniGiorno = null;
						parametri.DataDIDGaranziaGiovaniMese = null;
						parametri.DataDIDGaranziaGiovaniAnno = null;
						parametri.LuogoDIDGaranziaGiovani = null;
					}
					else
					{
						List<string> provinceAmmesse = domanda.Progetto.RegioniAmmesse
							.SelectMany(p => p.Provincia)
							.Select(p => p.Nome).ToList();
						ViewData["ProvinceAmmesse"] = "\"" + string.Join("\",\"", provinceAmmesse) + "\"";
					}

				}
				else //Se il progetto non è di tipo garanzia giovani cancello tutti i dati relativi alla garanzia giovani
				{
					parametri.DichiarazioneRequisitiGaranziaGiovani = null;
					parametri.DataPresaInCaricoGaranziaGiovaniGiorno = null;
					parametri.DataPresaInCaricoGaranziaGiovaniMese = null;
					parametri.DataPresaInCaricoGaranziaGiovaniAnno = null;
					parametri.LuogoPresaInCaricoGaranziaGiovani = null;
					parametri.DataDIDGaranziaGiovaniGiorno = null;
					parametri.DataDIDGaranziaGiovaniMese = null;
					parametri.DataDIDGaranziaGiovaniAnno = null;
					parametri.LuogoDIDGaranziaGiovani = null;
					parametri.AlternativaRequisitiGaranziaGiovani = null;
					parametri.DichiarazioneResidenzaOK = null;
				}


				if (parametri.Action == "salva")
				{

					List<string> listaErrori = new List<string>();
					foreach (PropertyInfo propieta in parametri.GetType().GetProperties())
					{
						var attributoMaxLength = propieta.GetCustomAttributes<MaxLengthAttribute>().FirstOrDefault();
						var attributoDisplayName = propieta.GetCustomAttributes<DisplayNameAttribute>().FirstOrDefault();
						var nomePropieta = attributoDisplayName == null ? propieta.Name : attributoDisplayName.DisplayName;
						if (attributoMaxLength != null)
						{

							string text = propieta.GetValue(parametri) as string;
							if (text != null && text.Length > attributoMaxLength.Length)
							{
								listaErrori.Add($"Inseriti più di {attributoMaxLength.Length} caratteri nel campo {nomePropieta}");
							}
						}
					}
					DateTime? dataDID = null;
					if (parametri.DataDIDGaranziaGiovaniGiorno.HasValue ||
						parametri.DataDIDGaranziaGiovaniMese.HasValue ||
						parametri.DataDIDGaranziaGiovaniAnno.HasValue)
					{
						try
						{
							dataDID = new DateTime(parametri.DataDIDGaranziaGiovaniAnno.Value,
																parametri.DataDIDGaranziaGiovaniMese.Value,
																parametri.DataDIDGaranziaGiovaniGiorno.Value);

						}
						catch (Exception)
						{
							listaErrori.Add($"Data della DID non valida");
						}
					}
					DateTime? dataPresaInCarico = null;
					if (parametri.DataPresaInCaricoGaranziaGiovaniGiorno.HasValue ||
						parametri.DataPresaInCaricoGaranziaGiovaniMese.HasValue ||
						parametri.DataPresaInCaricoGaranziaGiovaniAnno.HasValue)
					{
						try
						{
							dataPresaInCarico = new DateTime(parametri.DataPresaInCaricoGaranziaGiovaniAnno.Value,
																parametri.DataPresaInCaricoGaranziaGiovaniMese.Value,
																parametri.DataPresaInCaricoGaranziaGiovaniGiorno.Value);

						}
						catch (Exception)
						{
							listaErrori.Add($"Data della PresaInCarico non valida");
						}
					}

					if (listaErrori.Count() == 0)
					{
						domanda.Cittadinanza = cittadinanza?.Nome;

						domanda.ComuneResidenza = parametri.ComuneResidenza;
						domanda.ProvinciaResidenza = parametri.ProvinciaResidenza;
						domanda.ViaResidenza = parametri.ViaResidenza;
						domanda.CivicoResidenza = parametri.CivicoResidenza;
						domanda.CapResidenza = parametri.CapResidenza;
						domanda.NazioneResidenza = parametri.NazioneResidenza;
						domanda.IndirizzoCompletoResidenza = parametri.ResidenzaIndirizzoCompleto;

						domanda.ComuneRecapito = parametri.ComuneRecapito;
						domanda.ProvinciaRecapito = parametri.ProvinciaRecapito;
						domanda.ViaRecapito = parametri.ViaRecapito;
						domanda.CivicoRecapito = parametri.CivicoRecapito;
						domanda.CapRecapito = parametri.CapRecapito;
						domanda.CodiceMinoriOpportunita = parametri.CodiceMinoriOpportunita;
						domanda.DichiarazioneMinoriOpportunita = parametri.DichiarazioneMinoriOpportunita;
						domanda.IdMotivazione = parametri.IdMotivazione;
						domanda.CodiceDichiarazioneCittadinanza = parametri.CodiceDichiarazioneCittadinanza;
						domanda.NonCondanneOk = parametri.NonCondanneOk ?? false;
						domanda.TrasferimentoSedeOk = parametri.TrasferimentoSedeOk;
						domanda.TrasferimentoProgettoOk = parametri.TrasferimentoProgettoOk;
						domanda.AltreDichiarazioniOk = parametri.AltreDichiarazioniOk ?? false;
						domanda.IdTitoloStudio = parametri.IdTitoloStudio;
						domanda.FormazioneAnagraficaIstituto = parametri.FormazioneAnagraficaIstituto;
						domanda.FormazioneAnagraficaDisciplina = parametri.FormazioneAnagraficaDisciplina;
						domanda.FormazioneAnagraficaItalia = parametri.FormazioneAnagraficaItalia;
						domanda.FormazioneAnagraficaAnno = parametri.FormazioneAnagraficaAnno;
						domanda.FormazioneAnagraficaEnte = parametri.FormazioneAnagraficaEnte;
						domanda.IscrizioneSuperioreAnno = parametri.IscrizioneSuperioreAnno;
						domanda.IscrizioneSuperioreIstituto = parametri.IscrizioneSuperioreIstituto;
						domanda.IscrizioneLaureaAnno = parametri.IscrizioneLaureaAnno;
						domanda.IscrizioneLaureaCorso = parametri.IscrizioneLaureaCorso;
						domanda.IscrizioneLaureaIstituto = parametri.IscrizioneLaureaIstituto;
						domanda.PrivacyConsenso = parametri.PrivacyConsenso;
						domanda.PrivacyPresaVisione = parametri.PrivacyPresaVisione;
						domanda.ConfermaResidenza = parametri.ConfermaResidenza;
						domanda.ResidenzaEstera = parametri.ResidenzaEstera;

						domanda.DichiarazioneRequisitiGaranziaGiovani = parametri.DichiarazioneRequisitiGaranziaGiovani;
						domanda.DataPresaInCaricoGaranziaGiovani = dataPresaInCarico;
						domanda.LuogoPresaInCaricoGaranziaGiovani = parametri.LuogoPresaInCaricoGaranziaGiovani;
						domanda.DataDIDGaranziaGiovani = dataDID;
						domanda.LuogoDIDGaranziaGiovani = parametri.LuogoDIDGaranziaGiovani;
						domanda.AlternativaRequisitiGaranziaGiovani = parametri.AlternativaRequisitiGaranziaGiovani;
						domanda.DichiarazioneResidenzaOK = parametri.DichiarazioneResidenzaOK;

						domanda.UserIdModifica = User.Identity.GetUserId();
						domanda.DataModifica = DateTime.Now;
						context.SaveChanges();
						Log.Information(LogEvent.DOMANDA_ONLINE, "L'utente ha modificato i dati anagrafici", parameters: parameters);
						TempData["Salvato"] = true;
					}
					else
					{
						TempData["Errore"] = "Errori nel salvataggio dei dati:<br />" + string.Join("<br />", listaErrori);
						parametri.DataPresaInCaricoGaranziaGiovaniErrore = true;
					}
				}
				ViewData["DatiAnagrafici"] = parametri;
				ViewData["Nazioni"] = nazioni.ToList();
				ViewData["Provincie"] = provincie.ToList();
				ViewData["Comuni"] = comuni.ToList();
				ViewData["TitoliStudio"] = context.TitoloStudio
							.OrderBy(x => x.Id)
							.ToList();
				ViewData["Specializzazioni"] = context.Specializzazione
							.OrderBy(x => x.Id)
							.ToList();
				ViewData["Motivazioni"] = context.Motivazione
							.OrderBy(x => x.Id)
							.ToList();
				return View(domanda);
			}
		}

		[HttpPost]
		public ActionResult AllegaCV(HttpPostedFileBase allegato, string action)
		{
			using (var context = new Entities())
			{
				string maxBytesText = null;
				List<string> errori = new List<string>();
				byte[] file = null;
				int maxBytes;
				maxBytesText = context.Configurazione
					.Where(c => c.Nome == Configurazione.MAXBYTESRICHIESTACREDENZIALI)
					.Select(x => x.Valore)
					.SingleOrDefault();
				DomandaPartecipazione domanda = null;
				try
				{
					domanda = GetDomanda(context);
				}
				catch (AccountException)
				{
					Log.Error(LogEvent.DOMANDA_ONLINE, "Errore nel recupero delle informazioni sull'account");
					return RedirectToAction("LogOff", "Account");
				}
				catch (Exception e)
				{
					throw e;
				}
				if (domanda == null)
				{
					return RedirectToAction("SelezionaBando");
				}
				if (domanda.DataPresentazione != null)
				{
					Log.Error(LogEvent.DOMANDA_ONLINE, "L'utente ha tentato di allegare un curriculum ad una domanda già presentata");
					return RedirectToAction("SelezionaBando");
				}
				CV cv = context.CV.FirstOrDefault(x => x.Id == domanda.Id);

				switch (action)
				{
					case "aggiungi":
						if (string.IsNullOrEmpty(maxBytesText))
						{
							Log.Error(LogEvent.DOMANDA_ONLINE, "Errore nei parametri di configurazione MaxBytesRichiestaCredenziali non presente");
							TempData["Errore"] = "Errore Interno";
							return RedirectToAction("DatiEsperienze");
						}
						else if (!int.TryParse(maxBytesText, out maxBytes))
						{
							Log.Error(LogEvent.DOMANDA_ONLINE, $"Errore nei parametri di configurazione MaxBytesRichiestaCredenziali non valorizzato correttamento valore = {maxBytesText}");
							TempData["Errore"] = "Errore Interno";
							return RedirectToAction("DatiEsperienze");
						}
						if (allegato == null)
						{
							errori.Add($"Non è stato inserito l'allegato");
						}
						else if (allegato.ContentLength < 4)
						{
							errori.Add($"Allegato non valido o vuoto");
						}
						else if (allegato.ContentLength > maxBytes)
						{
							errori.Add($"Le dimensioni dell'allegato sono troppo grandi (dimensione massima {Utils.FileDimensionToString(maxBytes)})");
						}
						else
						{
							file = new byte[allegato.ContentLength];
							allegato.InputStream.Read(file, 0, allegato.ContentLength);
							if (file[0] != 0x25 || file[1] != 0x50 || file[2] != 0x44 || file[3] != 0x46)
							{
								errori.Add($"Formato dell'allegato non corretto il file deve essere di tipo PDF");
							}
							try
							{
								using (iTextSharp.text.pdf.PdfReader reader = new iTextSharp.text.pdf.PdfReader(file))
								{
									int a = reader.NumberOfPages;
									if (!reader.IsOpenedWithFullPermissions)
									{
										Log.Information(LogEvent.DOMANDA_ONLINE, "Allegato PDF protetto da password");
										errori.Add($"Non è possibile allegare documenti protetti da password.");
									}
								}
							}
							catch (Exception e)
							{
								Log.Error(LogEvent.DOMANDA_ONLINE, "Allegato PDF non valido",e);
								errori.Add($"È stato allegato un PDF non valido");
							}
						}

						if (errori.Count > 0) TempData["Errore"] = string.Join("<br />", errori);
						if (errori.Count == 0 && file != null)
						{
							cv.AllegatoCV = file;
							domanda.NomeFileCV = allegato.FileName;
							domanda.UserIdModifica = User.Identity.GetUserId();
							domanda.DataModifica = DateTime.Now;
							context.SaveChanges();
							Log.Information(LogEvent.DOMANDA_ONLINE, "Inserito CV");
							TempData["Message"] = "È stato caricato correttamente il Curriculum Vitae";
						}

						break;
					case "rimuovi":
						cv.AllegatoCV = null;
						domanda.NomeFileCV = null;
						domanda.UserIdModifica = User.Identity.GetUserId();
						domanda.DataModifica = DateTime.Now;
						context.SaveChanges();
						Log.Information(LogEvent.DOMANDA_ONLINE, "Rimosso CV");
						break;
					default:
						break;
				}

				return RedirectToAction("DatiEsperienze");
			}
		}

		public ActionResult DownloadCV()
		{
			using (var context = new Entities())
			{
				try
				{
					DomandaPartecipazione domanda = null;
					try
					{
						domanda = GetDomanda(context);
					}
					catch (AccountException)
					{
						Log.Error(LogEvent.DOMANDA_ONLINE, "Errore nel recupero delle informazioni sull'account");
						return RedirectToAction("LogOff", "Account");
					}
					catch (Exception e)
					{
						throw e;
					}
					if (domanda == null)
					{
						return RedirectToAction("SelezionaBando");
					}
					CV cv = context.CV.FirstOrDefault(x => x.Id == domanda.Id);
					if (cv==null || cv.AllegatoCV ==null)
					{
						return RedirectToAction("SelezionaBando");
					}
					Log.Information(LogEvent.DOMANDA_ONLINE, "Scaricato CV");
					return File(cv.AllegatoCV, "application/pdf");
				}
				catch (Exception e)
				{
					Log.Error(LogEvent.DOMANDA_ONLINE, "Errore nella lettura del DB",e);
					throw;
				}
			}
		}

		[HttpPost]
		public ActionResult Download(int idDomanda)
		{
			using (var context = new Entities())
			{
				/* Recupero codice fiscale utente */
				ApplicationUser utente;
				try
				{
					utente = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>()
						.FindById(User.Identity.GetUserId());
				}
				catch (Exception e)
				{
					Log.Error(LogEvent.DOMANDA_ONLINE, "Errore nella lettura dell'utente dal DB", e);
					return RedirectToAction("SelezionaBando");
				}
				if (string.IsNullOrEmpty(utente.CodiceFiscale))
				{
					Log.Error(LogEvent.DOMANDA_ONLINE, "Utente senza codice fiscale");
					return RedirectToAction("SelezionaBando");
				}

				try
				{
					var domanda = context.DomandaPartecipazione.FirstOrDefault(
										x => x.Id == idDomanda &&
										x.CodiceFiscale == utente.CodiceFiscale);
					if (domanda == null)
					{
						Log.Error(LogEvent.DOMANDA_ONLINE, $"Domanda con ID {idDomanda} non presente o non associata al codice fiscale {utente.CodiceFiscale}");
						TempData["Errore"] = "Domanda non presente";
						return RedirectToAction("DomandePresentate");
					}
					var documento = context.Documento.FirstOrDefault(d => d.Id == idDomanda);
					if (documento==null || documento.FileDomanda==null)
					{
						Log.Error(LogEvent.DOMANDA_ONLINE, $"Documento con ID {idDomanda} non presente");
						TempData["Errore"] = "Documento non trovato";
						return RedirectToAction("DomandePresentate");
					}
					Log.Information(LogEvent.DOMANDA_ONLINE, $"Scaricata Domanda con ID {idDomanda}");
					return File(documento.FileDomanda, "application/pdf");
				}
				catch (Exception e)
				{
					Log.Error(LogEvent.DOMANDA_ONLINE, "Errore nella lettura del DB",e);
					TempData["Errore"] = "Errore nel recupero della domanda";
					return RedirectToAction("DomandePresentate");
				}
			}
		}

		[HttpPost]
		public ActionResult DownloadDomanda()
		{
			using (var context = new Entities())
			{
				try
				{
					DomandaPartecipazione domanda = null;
					try
					{
						domanda = GetDomanda(context);
					}
					catch (AccountException)
					{
						Log.Error(LogEvent.DOMANDA_ONLINE, "Errore nel recupero delle informazioni sull'account");
						return RedirectToAction("LogOff", "Account");
					}
					catch (Exception e)
					{
						throw e;
					}
					if (domanda == null)
					{
						return RedirectToAction("SelezionaBando");
					}
					var documento = context.Documento.FirstOrDefault(d => d.Id == domanda.Id);
					if (documento == null || documento.FileDomanda == null)
					{
						Log.Error(LogEvent.DOMANDA_ONLINE, $"Documento con ID {domanda.Id} non presente");
						TempData["Errore"] = "Documento non trovato";
						return RedirectToAction("DomandePresentate");
					}
					Log.Information(LogEvent.DOMANDA_ONLINE, "Scaricata Domanda");
					return File(documento.FileDomanda, "application/pdf");
				}
				catch (Exception e)
				{
					Log.Error(LogEvent.DOMANDA_ONLINE, "Errore nella lettura del DB",e);
					throw;
				}
			}
		}

		[HttpPost]
		public ActionResult Annullamento()
		{
			using (var context = new Entities())
				try
				{
					DomandaPartecipazione domanda = null;
					try
					{
						domanda = GetDomanda(context);
					}
					catch (AccountException)
					{
						Log.Error(LogEvent.DOMANDA_ONLINE, "Errore nel recupero delle informazioni sull'account");
						return RedirectToAction("LogOff", "Account");
					}
					catch (Exception e)
					{
						throw e;
					}
					if (domanda == null)
					{
						return RedirectToAction("SelezionaBando");
					}
					Bando bando = context.Bando.Where(x => x.Gruppo == domanda.GruppoBando).FirstOrDefault();
					if (bando.DataFineAnnullamento < DateTime.Now)
					{
						TempData["Errore"] = "Non è più possibile annullare la domanda. È scaduto il termine ultimo per l'annullamento";
						Log.Warning(LogEvent.DOMANDA_ONLINE, "Annullata la domanda dopo la data di fine annullamento");
						return RedirectToAction("Index");
					}
					ViewData["Motivi"] = context.MotivoAnnullamento
						.Where(m => m.Selezionabile)
						.ToList();
					return View("Annullamento", domanda);
				}
				catch (Exception e)
				{
					Log.Error(LogEvent.DOMANDA_ONLINE, "Errore nella lettura del DB",e);
					throw;
				}
		}

		[HttpPost]
		public ActionResult ConfermaAnnullamento(int? idMotivo)
		{
			using (var context = new Entities())
				try
				{
					DomandaPartecipazione domanda = null;
					try
					{
						domanda = GetDomanda(context);
					}
					catch (AccountException)
					{
						Log.Error(LogEvent.DOMANDA_ONLINE, "Errore nel recupero delle informazioni sull'account");
						return RedirectToAction("LogOff", "Account");
					}
					catch (Exception e)
					{
						throw e;
					}
					Bando bando = context.Bando.Where(x => x.Gruppo == domanda.GruppoBando).FirstOrDefault();
					if (bando.DataFineAnnullamento < DateTime.Now)
					{
						TempData["Errore"] = "Non è più possibile annullare la domanda. È scaduto il termine ultimo per l'annullamento";
						Log.Warning(LogEvent.DOMANDA_ONLINE, "Tentativo di annullare la domanda dopo la data di fine annullamento");
						return RedirectToAction("Index");
					}
					if (domanda == null)
					{
						return RedirectToAction("SelezionaBando");
					}
					if (!idMotivo.HasValue)
					{
						TempData["Errore"] = "Selezionare il motivo";
						Log.Warning(LogEvent.DOMANDA_ONLINE, "Non è stato selezionato il motivo nella richiesta di annullamento");
						return Annullamento();
					}
					MotivoAnnullamento motivo = context.MotivoAnnullamento
						.SingleOrDefault(m => m.Id == idMotivo && m.Selezionabile);
					if (motivo == null)
					{
						TempData["Errore"] = "Motivo selezionato non valido";
						Log.Error(LogEvent.DOMANDA_ONLINE, "È stato selezionato un motivo non valido nella richiesta di annullamento");
						return Annullamento();
					}

					if (domanda.DataRichiestaAnnullamento != null)
					{
						Log.Warning(LogEvent.DOMANDA_ONLINE, "Richiesta annullamento domanda già annullata");
						TempData["Errore"] = "Richiesta annullamento già effettuata";
					}
					else
					{
						TempData["Errore"] = null;
						//Verifica se è l'ultimo giorno del bando
						if (bando.DataScadenza.Date == DateTime.Now.Date)
						{
							//L'ultimo giorno annullo direttamente la domanda
							domanda.IdMotivazioneAnnullamento = idMotivo;
							domanda.DataRichiestaAnnullamento = DateTime.Now;
							context.SP_AnnullaDomanda(domanda.Id);
							context.SaveChanges();
							Log.Information(LogEvent.DOMANDA_ONLINE, "Domanda Annullata l'ultimo giorno del bando");
							TempData["Message"] = "<p>La domanda è stata annullata.</p><p>È possibile inserire una nuova domanda.</p>";
						}
						else
						{
							domanda.IdMotivazioneAnnullamento = idMotivo;
							domanda.DataRichiestaAnnullamento = DateTime.Now;
							context.SaveChanges();
							Log.Information(LogEvent.DOMANDA_ONLINE, "Richiesta annullamento domanda");
						}
					}
					return RedirectToAction("SelezionaBando");
				}
				catch (Exception e)
				{
					Log.Error(LogEvent.DOMANDA_ONLINE, "Errore nella lettura del DB",e);
					throw;
				}
		}


		[HttpPost]
		public ActionResult InserisciAllegato(InserisciAllegatoInput parametri)
		{
			int? idGruppo = GetIdGruppo();
			if (!idGruppo.HasValue)
			{
				Log.Information(LogEvent.DOMANDA_ONLINE, "Inserito Allegato senza avere il gruppo",parameters:parametri);
				return RedirectToAction("SelezionaBando");
			}

			try
			{
				using (var context = new Entities())
				{
					var bandi = context.Bando
						.Where(x => x.DataScadenza >= DateTime.Now);
					if (bandi.Count() == 1)
					{
						return RedirectToAction("Index", new { gruppo = bandi.First().Gruppo });
					}
					return View(bandi.ToList());
				}
			}
			catch (Exception e)
			{
				Log.Error(LogEvent.DOMANDA_ONLINE, "Errore nella lettura del bando dal DB", e, parameters: parametri);
				return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest, "Errore interno");
			}
		}


		public ActionResult DatiEsperienze()
		{
			using (var context = new Entities())
			{
				ViewData["TitoliStudio"] = context.TitoloStudio
							.OrderBy(x => x.Id)
							.ToList();
				ViewData["Specializzazioni"] = context.Specializzazione
							.OrderBy(x => x.Id)
							.ToList();
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
				DomandaPartecipazione domanda = null;
				try
				{
					domanda = GetDomanda(context);
				}
				catch (AccountException)
				{
					Log.Error(LogEvent.DOMANDA_ONLINE, "Errore nel recupero delle informazioni sull'account");
					return RedirectToAction("LogOff", "Account");
				}
				catch (Exception e)
				{
					throw e;
				}
				if (domanda == null)
				{
					return RedirectToAction("SelezionaBando");
				}
				if (domanda.DataPresentazione != null)
				{
					return RedirectToAction("Index");
				}

				//Cognome, nome e codice fiscale utente da esporre sulla View
				this.SetViewDataWithInfoUser();

				return View(domanda);
			}
		}

		[HttpPost]
		public ActionResult DatiEsperienze(DatiEsperienzeInput parametri)
		{
			List<string> parametriInvalidi = Utils.checkCaratteriInvalidi(parametri);
			if (parametriInvalidi.Count > 0)
			{
				TempData["Errore"] = "Attenzione sono stati inseriti dei caratteri invalidi nei seguenti campi:" + string.Join(", ", parametriInvalidi);

			}
			using (var context = new Entities())
			{
				ViewData["TitoliStudio"] = context.TitoloStudio
							.OrderBy(x => x.Id)
							.ToList();
				ViewData["Specializzazioni"] = context.Specializzazione
							.OrderBy(x => x.Id)
							.ToList();
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
				DomandaPartecipazione domanda = null;
				try
				{
					domanda = GetDomanda(context);
				}
				catch (AccountException)
				{
					Log.Error(LogEvent.DOMANDA_ONLINE, "Errore nel recupero delle informazioni sull'account");
					return RedirectToAction("LogOff", "Account");
				}
				catch (Exception e)
				{
					throw e;
				}
				if (domanda == null)
				{
					return RedirectToAction("SelezionaBando");
				}
				if (domanda.DataPresentazione != null)
				{
					Log.Error(LogEvent.DOMANDA_ONLINE, "L'utente ha tentato di modificare i dati delle esperienze ad una domanda già presentata");
					return RedirectToAction("SelezionaBando");
				}
				/*** Controllo consistenza dati ***/
				#region Controlli
				List<string> listaErrori = new List<string>();
				foreach (PropertyInfo propieta in parametri.GetType().GetProperties())
				{
					var attributoMaxLength = propieta.GetCustomAttributes<MaxLengthAttribute>().FirstOrDefault();
					var attributoDisplayName = propieta.GetCustomAttributes<DisplayNameAttribute>().FirstOrDefault();
					var nomePropieta = attributoDisplayName == null ? propieta.Name : attributoDisplayName.DisplayName;
					if (attributoMaxLength != null)
					{

						string text = propieta.GetValue(parametri) as string;
						if (text != null && text.Length > attributoMaxLength.Length)
						{
							listaErrori.Add($"Inseriti più di {attributoMaxLength.Length} caratteri nel campo {nomePropieta}");
						}
					}
				}


				#endregion
				if (listaErrori.Count() == 0)
				{
					domanda.PrecedentiEnte = parametri.PrecedentiEnte;
					domanda.PrecedentiEnteDescrizione = parametri.PrecedentiEnteDescrizione;
					domanda.PrecedentiAltriEnti = parametri.PrecedentiAltriEnti;
					domanda.PrecedentiAltriEntiDescrizione = parametri.PrecedentiAltriEntiDescrizione;
					domanda.PrecedentiImpiego = parametri.PrecedentiImpiego;
					domanda.PrecedentiImpiegoDescrizione = parametri.PrecedentiImpiegoDescrizione;
					domanda.IdTitoloStudioEsperienze = parametri.IdTitoloStudio;
					domanda.FormazioneItalia = parametri.FormazioneItalia;
					domanda.FormazioneDisciplina = parametri.FormazioneDisciplina;
					domanda.FormazioneIstituto = parametri.FormazioneIstituto;
					domanda.FormazioneAnno = parametri.FormazioneAnno;
					domanda.FormazioneData = parametri.FormazioneData;
					domanda.FormazioneEnte = parametri.FormazioneEnte;
					//domanda.IscrizioneSuperioreAnno = parametri.IscrizioneSuperioreAnno;
					//domanda.IscrizioneSuperioreIstituto = parametri.IscrizioneSuperioreIstituto;
					//domanda.IscrizioneLaureaAnno = parametri.IscrizioneLaureaAnno;
					//domanda.IscrizioneLaureaCorso = parametri.IscrizioneLaureaCorso;
					//domanda.IscrizioneLaureaIstituto = parametri.IscrizioneLaureaIstituto;
					domanda.CorsiEffettuati = parametri.CorsiEffettuati;
					domanda.Specializzazioni = parametri.Specializzazioni;
					domanda.Competenze = parametri.Competenze;
					domanda.Altro = parametri.Altro;

					domanda.UserIdModifica = User.Identity.GetUserId();
					domanda.DataModifica = DateTime.Now;
					context.SaveChanges();
					Log.Information(LogEvent.DOMANDA_ONLINE, "Modificati i dati dei titoli ed esperienze",parameters :parametri);
					TempData["Salvato"] = true;
				}
				else
				{
					TempData["Errore"] = "Errori nel salvataggio dei dati:<br />" + string.Join("<br />", listaErrori);
				}

				//Cognome, nome e codice fiscale utente da esporre sulla View
				this.SetViewDataWithInfoUser();

				return View(domanda);
			}
		}



		[HttpPost]
		public ActionResult Riepilogo(RiepilogoInput parametri)
		{
			using (var context = new Entities())
			{
				var nazioni = context.Nazione.OrderBy(x => x.Nome);
				ViewData["Nazioni"] = nazioni.ToList();
				ViewData["TitoliStudio"] = context.TitoloStudio
							.OrderBy(x => x.Id)
							.ToList();
				ViewData["Specializzazioni"] = context.Specializzazione
							.OrderBy(x => x.Id)
							.ToList();
				ApplicationUser utente;
				try
				{
					utente = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>()
						.FindById(User.Identity.GetUserId());
				}
				catch (Exception e)
				{
					Log.Error(LogEvent.DOMANDA_ONLINE, "Errore nella lettura dell'utente dal DB", e, parameters: parametri);
					throw new Exception("Errore nella lettura dell'utente dal DB", e);
				}
				DomandaPartecipazione domanda = null;
				try
				{
					domanda = GetDomanda(context);
				}
				catch (AccountException)
				{
					Log.Error(LogEvent.DOMANDA_ONLINE, "Errore nel recupero delle informazioni sull'account");
					return RedirectToAction("LogOff", "Account");
				}
				catch (Exception e)
				{
					throw e;
				}
				if (domanda == null)
				{
					return RedirectToAction("SelezionaBando");
				}
				if (domanda.DataPresentazione != null)
				{
					return RedirectToAction("Index");
				}

				/*** Valorizzazioni campi presenti sull'utente  ***/
				domanda.Nome = utente.Nome;
				domanda.Cognome = utente.Cognome;
				domanda.CodiceFiscale = utente.CodiceFiscale;
				domanda.Genere = utente.Genere.ToUpper().First() == 'M' ? "M" : "F";
				domanda.LuogoNascita = utente.LuogoNascita;
				domanda.NazioneNascita = utente.NazioneNascita;
				domanda.Telefono = utente.Telefono;
				domanda.Email = utente.Email;
				domanda.DataNascita = utente.DataNascita;
				if (User.IsInRole(Role.UTENTE_CREDENZIALI))
				{
					domanda.Cittadinanza = utente.Cittadinanza;
				}
				/*if (User.IsInRole(Role.UTENTE_SPID))
				{
					var indirizzo = Utils.GetIndirizzo(utente.Indirizzo);
					domanda.ComuneResidenza = indirizzo.Comune;
					domanda.ProvinciaResidenza = indirizzo.Provincia;
					domanda.ViaResidenza = indirizzo.Via;
					domanda.CivicoResidenza = indirizzo.Civico;
					domanda.CapResidenza = indirizzo.CAP;
				}*/

				/* Controlli presentazione della domanda */
				List<string> listaErrori = new List<string>();
				if (domanda.Progetto == null)
				{
					listaErrori.Add("Scegliere il progetto<br>");
				}else{
					//Verifica Progetto
					if (domanda.Progetto.DataAnnullamento != null)
					{
						listaErrori.Add($"Il progetto selezionato è stato annullato. Seleziona un altro progetto");
					}

                    //Recupera tutti i tipi di servizio fatti dall'utente (cittadino) 
                    List<string> serviziEffettuati;
					try
					{
						serviziEffettuati = context.VolontarioServizio
									.Where(x => x.CodiceFiscale == utente.CodiceFiscale)
									.Select(x => x.TipoServizio)
									.ToList();
					}
					catch (Exception e)
					{
						Log.Error(LogEvent.DOMANDA_ONLINE, "Errore nel recupero dei servizi effettuati", parameters: parametri);
						throw e;
					}

					if (domanda.GruppoBando == 63)
                    {
						//Se l'utente ha già fatto il servizio civile non può presentare domanda. Errore grave
						if ((serviziEffettuati != null) && (serviziEffettuati.Contains(VolontarioServizio.SERVIZIO_CIVILE)))
                        {
							ViewData["Errore"] = "Hai già effettuato servizio civile";
							return View("Errore");
						}
					}
					else
                    {
						bool effettuatoServizioCivile = serviziEffettuati.Any(s => s == VolontarioServizio.SERVIZIO_CIVILE);
						bool effettuatoGaranziaGiovani = serviziEffettuati.Any(s => s == VolontarioServizio.GARANZIA_GIOVANI);
						if (domanda.Progetto.Programma != null && effettuatoServizioCivile && !domanda.Progetto.Programma.IdTipoGG.HasValue)
						{
							listaErrori.Add($"Hai già effettuato il servizio civile. Devi selezionare un altra tipologia di progetto");
						}
						if (domanda.Progetto.Programma != null && effettuatoGaranziaGiovani && domanda.Progetto.Programma.IdTipoGG.HasValue)
						{
							listaErrori.Add($"Hai già effettuato un progetto Garanzia Giovani. Devi selezionare un altra tipologia di progetto");
						}
					}
                }

                List<string> listaErroriDatiAnagrafici = ControllaDatiAnagrafici(domanda);
				if (listaErroriDatiAnagrafici.Count > 0)
				{
					string listaErroriDatiAnagraficiTesto = "";
					foreach (string errore in listaErroriDatiAnagrafici)
					{
						listaErroriDatiAnagraficiTesto += $"<li>{errore}</li>";
					}
					listaErrori.Add($"Dati anagrafici incompleti:<ul>{listaErroriDatiAnagraficiTesto}</ul>");
				}
				List<string> listaErroriEsperienze = ControllaEsperienze(domanda);
				if (listaErroriEsperienze.Count > 0)
				{
					string listaErroriEsperienzeTesto = "";
					foreach (string errore in listaErroriEsperienze)
					{
						listaErroriEsperienzeTesto += $"<li>{errore}</li>";
					}
					listaErrori.Add($"Errori presenti nei dati sui titoli ed esperienze:<ul>{listaErroriEsperienzeTesto}</ul>");
				}

				if (listaErrori.Count > 0)
				{
					TempData["Errore"] = string.Join("<br>", listaErrori);
					return RedirectToAction("Index");
				}
				if (!(parametri.Conferma ?? false) && ControllaDatiEsperienzeVuoti(domanda))
				{
					return View("Conferma", domanda);
				}
				/* Presentazione domanda */
				if (parametri.Presenta ?? false)
				{
					domanda.DataPresentazione = DateTime.Now;
					domanda.UserIdPresentazione = User.Identity.GetUserId();
					CV cv = context.CV.FirstOrDefault(x => x.Id == domanda.Id);

					/* Creazione PDF*/
					byte[] filePdf;
					try
					{
						filePdf = GetPdf(domanda,cv);
					}
					catch (Exception e)
					{
						Log.Error(LogEvent.DOMANDA_ONLINE, "Errore nella creazione del file PDF della domanda", e, parameters: parametri);
						throw e;
					}
					try
					{
						var documento = context.Documento.FirstOrDefault(d => d.Id == domanda.Id);
						if (documento == null)
						{
							Log.Error(LogEvent.DOMANDA_ONLINE, $"Documento con ID {domanda.Id} non presente");
							TempData["Errore"] = "Errore Salvataggio della domanda";
							return RedirectToAction("Index");
						}
						documento.FileDomanda = filePdf;
						Progetto progetto = context.Progetto
							.FirstOrDefault(p =>p.CodiceProgetto == domanda.CodiceProgettoSelezionato &&
												p.CodiceSede == domanda.CodiceSedeSelezionata);
						if (progetto!=null)
						{
							progetto.NumeroDomande = context.DomandaPartecipazione
								.Where(d => d.DataAnnullamento==null &&
											d.DataPresentazione != null &&
											d.CodiceProgettoSelezionato == progetto.CodiceProgetto &&
											d.CodiceSedeSelezionata == progetto.CodiceSede)
								.Count()+1;
						}
						context.SaveChanges();
						Log.Information(LogEvent.DOMANDA_ONLINE, "L'utente ha presentato la domanda", parameters: parametri);

						return RedirectToAction("Index");
					}
					catch (Exception e)
					{
						Log.Error(LogEvent.DOMANDA_ONLINE, "Errore nel salvataggio dei deti di presentazione della domanda", e, parameters: parametri);
						throw e;
					}
				}

				if (parametri.Stampa ?? false)
				{
					CV cv = context.CV.FirstOrDefault(x => x.Id == domanda.Id);
					if (cv == null)
					{
						return RedirectToAction("SelezionaBando");
					}

					byte[] filePdf;
					try
					{
						filePdf = GetPdf(domanda,cv);
					}
					catch (Exception e)
					{
						Log.Error(LogEvent.DOMANDA_ONLINE, "Errore nella creazione del file PDF della domanda", e,parameters: parametri);
						throw;
					}
					Log.Information(LogEvent.DOMANDA_ONLINE, "L'utente ha richiesto di stampare la domanda");
					return File(filePdf, "application/pdf");

				}
				Log.Information(LogEvent.DOMANDA_ONLINE, "L'utente ha visualizzato il riepilogo della domanda");
				return View(domanda);
			}

		}

		private byte[] GetPdfDomanda(DomandaPartecipazione domanda)
		{
			using (MemoryStream streamDomanda = new MemoryStream())
			{
				string html = RenderPartialViewToString("Riepilogo", domanda);

				PdfDocument pdfDomanda = PdfGenerator.GeneratePdf(html, PageSize.A4, 40);
				pdfDomanda.Save(streamDomanda);
				return streamDomanda.ToArray();
			}
		}


		private byte[] GetPdf(DomandaPartecipazione domanda, CV cv)
		{
			byte[] pdfDomanda = GetPdfDomanda(domanda);
			using (MemoryStream streamDomanda = new MemoryStream(pdfDomanda))
			using (iTextSharp.text.pdf.PdfReader readerDomanda = new iTextSharp.text.pdf.PdfReader(streamDomanda))
			using (MemoryStream stream = new MemoryStream())
			using (iTextSharp.text.Document doc = new iTextSharp.text.Document())
			using (iTextSharp.text.pdf.PdfCopy pdf = new iTextSharp.text.pdf.PdfCopy(doc, stream))
			{
				doc.Open();
				pdf.AddDocument(readerDomanda);
				if (cv!=null && cv.AllegatoCV != null)
				{
					using (iTextSharp.text.pdf.PdfReader readerCV = new iTextSharp.text.pdf.PdfReader(new MemoryStream(cv.AllegatoCV)))
					{
						for (int i = 0; i < readerCV.NumberOfPages; i++)
						{
							pdf.AddPage(pdf.GetImportedPage(readerCV, i + 1));
						}
					}
				}
				doc.Close();
				return stream.ToArray();
			}

		}

		public ActionResult Informativa()
		{
			return View();
		}

		private bool ControllaDatiEsperienzeVuoti(DomandaPartecipazione domanda)
		{
			bool vuoto =
				!domanda.PrecedentiEnte.HasValue &&
				string.IsNullOrEmpty(domanda.PrecedentiEnteDescrizione) &&
				!domanda.PrecedentiAltriEnti.HasValue &&
				string.IsNullOrEmpty(domanda.PrecedentiAltriEntiDescrizione) &&
				!domanda.PrecedentiImpiego.HasValue &&
				!domanda.IdTitoloStudioEsperienze.HasValue &&
				string.IsNullOrEmpty(domanda.FormazioneDisciplina) &&
				!domanda.FormazioneItalia.HasValue &&
				!domanda.FormazioneData.HasValue &&
				string.IsNullOrEmpty(domanda.FormazioneIstituto) &&
				string.IsNullOrEmpty(domanda.FormazioneEnte) &&
				//!domanda.IscrizioneSuperioreAnno.HasValue &&
				//string.IsNullOrEmpty(domanda.IscrizioneSuperioreIstituto) &&
				//!domanda.IscrizioneLaureaAnno.HasValue &&
				//string.IsNullOrEmpty(domanda.IscrizioneLaureaCorso) &&
				string.IsNullOrEmpty(domanda.CorsiEffettuati) &&
				string.IsNullOrEmpty(domanda.Specializzazioni) &&
				string.IsNullOrEmpty(domanda.Competenze) &&
				string.IsNullOrEmpty(domanda.Altro) &&
				string.IsNullOrEmpty(domanda.NomeFileCV)
			;


			return vuoto;
		}


		private List<string> ControllaEsperienze(DomandaPartecipazione domanda)
		{
			List<string> listaErrori = new List<string>();
			if ((domanda.PrecedentiEnte ?? false) && string.IsNullOrEmpty(domanda.PrecedentiEnteDescrizione))
				listaErrori.Add("Inserire le informazioni sulle precedenti esperienze presso l’Ente");
			if (!(domanda.PrecedentiEnte ?? false) && !string.IsNullOrEmpty(domanda.PrecedentiEnteDescrizione))
				listaErrori.Add("Sono state inserite informazioni sulle precedenti esperienze presso l’Ente, ma non è stato specificato \"Ho avuto\".");
			if ((domanda.PrecedentiAltriEnti ?? false) && string.IsNullOrEmpty(domanda.PrecedentiAltriEntiDescrizione))
				listaErrori.Add("Inserire le informazioni sulle precedenti esperienze presso altri enti");
			if (!(domanda.PrecedentiAltriEnti ?? false) && !string.IsNullOrEmpty(domanda.PrecedentiAltriEntiDescrizione))
				listaErrori.Add("Sono state inserite informazioni sulle precedenti esperienze presso altri enti, ma non è stato specificato \"Ho avuto\"");
			if ((domanda.PrecedentiImpiego ?? false) && string.IsNullOrEmpty(domanda.PrecedentiImpiegoDescrizione))
				listaErrori.Add("Inserire le informazioni sulle precedenti esperienze in settori d’impiego analoghi al progetto");
			if (!(domanda.PrecedentiImpiego ?? false) && !string.IsNullOrEmpty(domanda.PrecedentiImpiegoDescrizione))
				listaErrori.Add("Sono state inserite informazioni sulle precedenti in settori d’impiego analoghi al progetto, ma non è stato specificato \"Ho avuto\"");
			/** Controllo incompletezza dati sul titolo di studio **/
			int campiFormazioneVuoti = 0;
			if (domanda.IdTitoloStudioEsperienze == null)
				campiFormazioneVuoti++;
			if (string.IsNullOrEmpty(domanda.FormazioneDisciplina) && domanda.IdTitoloStudioEsperienze != 1 && domanda.IdTitoloStudioEsperienze != 2)
				campiFormazioneVuoti++;
			if (domanda.FormazioneItalia == null)
				campiFormazioneVuoti++;
			if (domanda.FormazioneAnno == null)
				campiFormazioneVuoti++;
			if (string.IsNullOrEmpty(domanda.FormazioneIstituto))
				campiFormazioneVuoti++;
			if (campiFormazioneVuoti != 0 && campiFormazioneVuoti != 5)
				listaErrori.Add("Dati del titolo di studio incompleti");
			//else if (!(domanda.FormazioneItalia ?? true) && string.IsNullOrEmpty(domanda.FormazioneEnte))
			//	listaErrori.Add("È necessario inserire l'ente che ha rilasciato il provvedimento per i titoli esteri");

			//---- Controlli bassa scolarizzazione ----
			if (domanda.IdTitoloStudioEsperienze.HasValue &&
				domanda.DichiarazioneMinoriOpportunita == true &&
				domanda.Progetto.IDParticolaritàEntità == MinoreOpportunita.BASSA_SCOLARIZZAZIONE &&
				!TitoloStudio.TitoliAmmessiBassaScolarizzazione.Contains(domanda.IdTitoloStudioEsperienze.Value)
				)
			{
				listaErrori.Add("Il titolo di studio ulteriore non è conforme alla richiesta di minore opportuntà per bassa scolarizzazione");
			}

			return listaErrori;
		}

		private List<string> ControllaDatiAnagrafici(DomandaPartecipazione domanda)
		{
			List<string> listaErrori = new List<string>();
			if (User.IsInRole(Role.UTENTE_SPID))
			{
				if (string.IsNullOrEmpty(domanda.Cittadinanza))
					listaErrori.Add("Cittadinanza non presente");
			}
			if (!(domanda.ConfermaResidenza ?? false))
			{
				listaErrori.Add("Non è stato confermato l'indirizzo di residenza");
			}
			else
			{
				if (domanda.ResidenzaEstera ?? false)
				{
					if (string.IsNullOrEmpty(domanda.IndirizzoCompletoResidenza))
						listaErrori.Add("Indirizzo di residenza non presente");
					if (string.IsNullOrEmpty(domanda.NazioneResidenza))
						listaErrori.Add("Nazione di residenza non presente");
				}
				else
				{
					if (string.IsNullOrEmpty(domanda.ComuneResidenza))
						listaErrori.Add("Comune di residenza non presente");
					if (string.IsNullOrEmpty(domanda.ProvinciaResidenza))
						listaErrori.Add("Provincia di residenza non presente");
					if (string.IsNullOrEmpty(domanda.ViaResidenza))
						listaErrori.Add("Indirizzo di residenza non presente");
					if (string.IsNullOrEmpty(domanda.CivicoResidenza))
						listaErrori.Add("Numero Civico di residenza non presente");
					if (string.IsNullOrEmpty(domanda.CapResidenza))
						listaErrori.Add("CAP di residenza non presente");
				}
			}

			/** Controllo incompletezza dati sul recapito **/
			int campiRecapitoVuoti = 0;
			if (string.IsNullOrEmpty(domanda.ComuneRecapito))
				campiRecapitoVuoti++;
			if (string.IsNullOrEmpty(domanda.ViaRecapito))
				campiRecapitoVuoti++;
			if (string.IsNullOrEmpty(domanda.CivicoRecapito))
				campiRecapitoVuoti++;
			if (string.IsNullOrEmpty(domanda.CapRecapito))
				campiRecapitoVuoti++;
			if (string.IsNullOrEmpty(domanda.ProvinciaRecapito))
				campiRecapitoVuoti++;
			if (campiRecapitoVuoti != 0 && campiRecapitoVuoti != 5)
				listaErrori.Add("Dati recapito incompleti");

			if (domanda.Progetto != null &&
				!domanda.DichiarazioneMinoriOpportunita.HasValue &&
				domanda.Progetto.IDParticolaritàEntità.HasValue &&
				domanda.Progetto.NumeroGiovaniMinoriOpportunità > 0)
				listaErrori.Add("Non è stata effettuata la dichiarazione sulle minori opportunità");
			if (domanda.Progetto != null &&
				domanda.DichiarazioneMinoriOpportunita != true &&
				domanda.Progetto.IDParticolaritàEntità.HasValue &&
				domanda.Progetto.NumeroGiovaniMinoriOpportunità == domanda.Progetto.NumeroPostiDisponibili)
				listaErrori.Add("Per la sede selzionata è necessario dichiarare di voler partecipare ai posti riservati");
			/*Controlli formazione*/
			if (domanda.IdTitoloStudio == TitoloStudio.NESSUNO || domanda.IdTitoloStudio == TitoloStudio.ESTERO)
			{

			}
			else
			{
				if (domanda.IdTitoloStudio == null)
					listaErrori.Add("Titolo di studio non presente");
				if (domanda.FormazioneAnagraficaDisciplina == null && domanda.IdTitoloStudio != 1 && domanda.IdTitoloStudio != 2)
					listaErrori.Add("Disciplina titolo di studio non presente");
				if (domanda.FormazioneAnagraficaItalia == null)
					listaErrori.Add("Nazione formazione non presente");
				if (domanda.FormazioneAnagraficaAnno == null)
					listaErrori.Add("Anno formazione non presente");
				if (domanda.FormazioneAnagraficaIstituto == null)
					listaErrori.Add("Istituto formazione non presente");
				//if (!(domanda.FormazioneAnagraficaItalia ?? true) && domanda.FormazioneAnagraficaEnte == null)
				//	listaErrori.Add("È necessario inserire l'ente che ha rilasciato il provvedimento per i titoli esteri");
			}


			/** Controllo incompletezza dati sull'iscrizione superiore **/
			int campiIscrizioneSuperioreVuoti = 0;
			if (domanda.IscrizioneSuperioreAnno == null)
				campiIscrizioneSuperioreVuoti++;
			if (string.IsNullOrEmpty(domanda.IscrizioneSuperioreIstituto))
				campiIscrizioneSuperioreVuoti++;
			if (campiIscrizioneSuperioreVuoti != 0 && campiIscrizioneSuperioreVuoti != 2)
				listaErrori.Add("Dati iscrizione scuola media superiore incompleti");
			/** Controllo incompletezza dati sull'iscrizione corso laurea **/
			int campiIscrizioneLaureaVuoti = 0;
			if (domanda.IscrizioneLaureaAnno == null)
				campiIscrizioneLaureaVuoti++;
			if (string.IsNullOrEmpty(domanda.IscrizioneLaureaCorso))
				campiIscrizioneLaureaVuoti++;
			if (string.IsNullOrEmpty(domanda.IscrizioneLaureaIstituto))
				campiIscrizioneLaureaVuoti++;
			if (campiIscrizioneLaureaVuoti != 0 && campiIscrizioneLaureaVuoti != 3)
				listaErrori.Add("Dati iscrizione corso di laurea incompleti");

			//---- Controlli Garanzia Giovani ----
			//Regioni
			if (domanda.Progetto?.RegioniAmmesse != null)
			{
				List<string> provinceAmmesse = domanda.Progetto.RegioniAmmesse
					.SelectMany(p => p.Provincia)
					.Select(p => p.Nome).ToList();
				List<string> regioniAmmesse = domanda.Progetto.RegioniAmmesse
					.Select(p => p.Nome).ToList();

				if (!provinceAmmesse.Contains(domanda.ProvinciaResidenza))
				{
					listaErrori.Add("Per il progetto selezionato è necessario avere la residenza in una delle seguenti Regioni:" + string.Join(", ", regioniAmmesse));
				}

			}
			//Provincia
			if (domanda.Progetto?.ProvinceAmmesse != null)
			{
				List<string> provinceAmmesse = domanda.Progetto.ProvinceAmmesse.Select(p => p.Nome).ToList();
				if (provinceAmmesse.Contains(domanda.ProvinciaResidenza))
				{
					listaErrori.Add("Per il progetto selezionato non è ammesso risiedere nella provincia di " + string.Join(", ", provinceAmmesse));
				}
				if (!(domanda.DichiarazioneRequisitiGaranziaGiovani ?? false))
				{
					listaErrori.Add("Non è stata effettuata la dichiarazione sui requisiti per la Garanzia Giovani");
				}
			}
			if (domanda.Progetto?.Programma?.IdTipoGG != null)
			{
				if (domanda.ResidenzaEstera ?? false)
				{
					listaErrori.Add("Per il progetto selezionato è necessario risiedere in Italia");
				}
				//Controlli Presa in carico
				if (!domanda.DataPresaInCaricoGaranziaGiovani.HasValue && (domanda.AlternativaRequisitiGaranziaGiovani != true /*|| domanda.Progetto.Asse1bisDisoccupati*/))
				{
					listaErrori.Add("Non è stata inserita la data di presa in carico presso il centro per l'impiego");
				}
				if (domanda.DataPresaInCaricoGaranziaGiovani >= DateTime.Now)
				{
					listaErrori.Add("È stata inserita una data di presa in carico futura");
				}
				if (string.IsNullOrEmpty(domanda.LuogoPresaInCaricoGaranziaGiovani) && domanda.AlternativaRequisitiGaranziaGiovani != true)
				{
					listaErrori.Add("Non è stata inserito il luogo di presa in carico presso il centro per l'impiego");
				}
				if(domanda.AlternativaRequisitiGaranziaGiovani == true &&(
					domanda.DataPresaInCaricoGaranziaGiovani.HasValue||
					!string.IsNullOrEmpty(domanda.LuogoPresaInCaricoGaranziaGiovani))){
					//listaErrori.Add("È stata effettuata la dichiarazione alternativa alla presa in carico e sono stati inseriti i dati sulla presa in carico");
				}
				// Controlli DID
				if (domanda.Progetto.Asse1bisDisoccupati)
				{
					if (!domanda.DataDIDGaranziaGiovani.HasValue)
					{
						listaErrori.Add("Non è stata inserita la data della DID");
					}
					if (domanda.DataDIDGaranziaGiovani >= DateTime.Now)
					{
						listaErrori.Add("È stata inserita una data della DID futura");
					}
					if (string.IsNullOrEmpty(domanda.LuogoDIDGaranziaGiovani))
					{
						listaErrori.Add("Non è stata inserito il luogo di presa in carico presso per la DID");
					}
				}
				if (domanda.Progetto.Asse1NEET)
				{
					if (!domanda.DichiarazioneRequisitiGaranziaGiovani ?? false)
					{
						listaErrori.Add("Non è stata effettuata la dichiarazione sui requisiti della Garanzia Giovani");
					}

				}
			}
			//---- Fine Controlli Garanzia Giovani ----
			//---- Controlli bassa scolarizzazione ----
			if (domanda.IdTitoloStudio.HasValue &&
				domanda.DichiarazioneMinoriOpportunita==true &&
				domanda.Progetto?.IDParticolaritàEntità==MinoreOpportunita.BASSA_SCOLARIZZAZIONE && 
				!TitoloStudio.TitoliAmmessiBassaScolarizzazione.Contains(domanda.IdTitoloStudio.Value)
				)
			{
				listaErrori.Add("Il titolo di studio non è conforme alla richiesta di minore opportuntà per bassa scolarizzazione");
			}
			
			if (domanda.Motivazione == null)
				listaErrori.Add("Non è stata inserita la motivazione della scelta del progetto");
			if (string.IsNullOrEmpty(domanda.CodiceDichiarazioneCittadinanza))
				listaErrori.Add("Non è stata effettuata la dichiarazione di cittadinanza");
			if (!(domanda.NonCondanneOk ?? false))
				listaErrori.Add("Non è stata effettuata la dichiarazione sulle condanne");
			if (domanda.TrasferimentoProgettoOk == null)
				listaErrori.Add("Non è stata effettuata la scelta sul trasferimento del progetto");
			if (domanda.TrasferimentoSedeOk == null)
				listaErrori.Add("Non è stata effettuata la scelta sul trasferimento della sede");
			if (!(domanda.AltreDichiarazioniOk ?? false))
				listaErrori.Add("Non è stata effettuata la dichiarazione finale");
			if (!(domanda.PrivacyPresaVisione ?? false))
				listaErrori.Add("Non è stata indicato di aver letto l'informativa sulla privacy");
			if (domanda.PrivacyConsenso == null)
				listaErrori.Add("Non è stato dato il consenso alla privacy");
			else if (!domanda.PrivacyConsenso.Value)
				listaErrori.Add("Attenzione: non hai espresso il consenso al trattamento e alla comunicazione dei dati, pertanto non è possibile presentare la domanda in quanto tale consenso è necessario al completamento della procedura. I dati delle domande incomplete per le quali non viene espresso il consenso non saranno trattati e saranno cancellati dal sistema.");
			return listaErrori;
		}

		private int? GetEta(DateTime? dataNascita)
		{
			if (!dataNascita.HasValue)
			{
				return null;
			}
			DateTime now = DateTime.Now;
			int age = now.Year - dataNascita.Value.Year;

			if (now.Month < dataNascita.Value.Month || (now.Month == dataNascita.Value.Month && now.Day < dataNascita.Value.Day))
				age--;
			return age;
		}

		/// <summary>
		/// Valorizza il ViewData con il cognome, nome e codice fiscale dell'utente
		/// </summary>
		private void SetViewDataWithInfoUser()
        {
			//Cognome
			ViewData["UserCognome"] = User.Identity.GetCognome();

			//Nome
			ViewData["UserNome"] = User.Identity.GetNome();

			//Codice fiscale
			ViewData["UserCodiceFiscale"] = User.Identity.GetCodiceFiscale();
		}

        /// <summary>
        /// Restituisce la lista delle tipologie di programma disponibili per il bando con ID 63
        /// </summary>
        /// <param name="tipologiaProgrammaScelta">Tipo pragramma scelto dall'utente</param>
        /// <returns>Lista tipi programma selezionabili</returns>
        private List<TipologiaProgramma> GetTipologiaProgramma(string tipologiaProgrammaScelta)
        {
			//Istanzia lista tipologia programmi per il bando con ID 63
			List<TipologiaProgramma> result = new List<TipologiaProgramma>()
            {
                new TipologiaProgramma() { Valore = "", Descrizione = ""} ,
				new TipologiaProgramma() { Valore = VolontarioServizio.SERVIZIO_CIVILE_AMBIENTALE, Descrizione = "Servizio civile ambientale", IsSelezionato = false } ,
				new TipologiaProgramma() { Valore = VolontarioServizio.SERVIZIO_CIVILE_DIGITALE, Descrizione = "Servizio civile digitale", IsSelezionato = false } ,
				new TipologiaProgramma() { Valore = VolontarioServizio.SERVIZIO_CIVILE, Descrizione = "Servizio civile ordinario autofinanziato", IsSelezionato = false }
			};

			//Seleziona l'eventuale tipologia di programma scelta dall'utente
            TipologiaProgramma tipoTmp = result.FirstOrDefault(foo => foo.Valore == tipologiaProgrammaScelta);
            if (tipoTmp != null)
            {
                tipoTmp.IsSelezionato = true;
            }

            return result;
		}

    }
}