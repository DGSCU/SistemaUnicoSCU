using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using Logger.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PdfSharp;
using PdfSharp.Pdf;
using RegistrazioneSistemaUnico.Data;
using RegistrazioneSistemaUnico.Helpers;
using RegistrazioneSistemaUnico.Models;
using RegistrazioneSistemaUnico.Models.Forms;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using TheArtOfDev.HtmlRenderer.PdfSharp;

namespace RegistrazioneSistemaUnico.Controllers
{
	[Authorize]
	public class RegistrazioneController : SmartController
	{

		private IViewRenderService renderService;
		public RegistrazioneController(RegistrazioneContext registrazioneContext, IViewRenderService renderService) : base(registrazioneContext)
		{
			this.renderService = renderService;
		}



		/// <summary>
		/// Metodo per valorizzare gli elementi delle combo.
		/// I dati vengono messi nella viewData in base ai dati di registrazione.
		/// </summary>
		/// <param name="registrazione"></param>
		private void CaricaCombo(Registrazione registrazione)
		{
			ViewData["IdTipologiaEnte"] = context.TipologiaEnte
				.Where(t => /*!t.DataCancellazione.HasValue && */t.IdCategoriaEnte == registrazione.IdCategoriaEnte)
				.ToDictionary(k => k.Id.ToString(), v => v.Descrizione);

			ViewData["IdProvinciaEnte"] = Comuni.ElencoProvince
				.OrderBy(c => c.Nome)
				.ToDictionary(k => k.Id.ToString(), v => v.Nome);

			if (registrazione.IdProvinciaEnte != null)
			{
				ViewData["IdComuneEnte"] = Comuni.ElencoComuni
					.Where(c => c.IdProvincia == registrazione.IdProvinciaEnte)
					.OrderBy(c => c.Nome)
					.ToDictionary(k => k.Id.ToString(), v => v.Nome);
			}
			else
			{
				ViewData["IdComuneEnte"] = new Dictionary<string, string>();
			}

		}


		/// <summary>
		/// Inizio registrazione dall'elenco degli Enti già presenti a sistema.
		/// Si potrebbero recuperare le informazioni da Helios.
		/// </summary>
		/// <param name="CodiceFiscaleEnte"></param>
		/// <returns></returns>
		public IActionResult RegistraEnte(string CodiceFiscaleEnte,string Albo)
		{
			UtenteEnte ente = context.UtenteEnte.FirstOrDefault(e => 
				e.CodiceFiscaleEnte == CodiceFiscaleEnte &&
				e.CodiceFiscale == User.Identity.Name &&
				e.Albo ==Albo);
			if (ente == null)
			{
				Log.Warning(LogEvent.REGISTRAZIONE_ENTE_INESISTENTE, $"-  CF {CodiceFiscaleEnte})");
				return RedirectToAction("Index", "Accesso");
			}
			var enteGiaRegistrato = context.UtenteEnte.Any(x =>
				x.CodiceFiscaleEnte == CodiceFiscaleEnte &&
				x.CodiceFiscale != null &&
				x.CodiceFiscale != User.Identity.Name &&
				x.Albo == "SCU");
			Registrazione registrazione = new Registrazione()
			{
				IdCategoriaEnte = ente.IdCategoriaEnte,
				EnteTitolare = ente.CodiceFiscaleEntePadre == null,
				Denominazione = ente.Denominazione,
				CodiceFiscaleEnte = CodiceFiscaleEnte,
				DataNominaRappresentanteLegale = ente.DataNominaRappresentanteLegale,
				IdTipologiaEnte = ente.IdTipologiaEnte,
				IdProvinciaEnte = ente.IdProvinciaEnte,
				IdComuneEnte = ente.IdComuneEnte,
				Via = ente.Via,
				Civico = ente.Civico,
				CAP = ente.CAP,
				Telefono = ente.Telefono,
				Email = ente.Email,
				PEC = ente.PEC,
				Sito = ente.Sito,
				VariazioneRappresentanteLegale = enteGiaRegistrato,
				Albo = Albo
			};
			SalvaRegistrazione(registrazione);
			Log.SetEnte(ente.CodiceFiscaleEnte);
			Log.Information(LogEvent.INIZIO_REGISTRAZIONE, $"Ente già presente");
			return RedirectToAction("Index");
		}

		//public IActionResult SelezionaTipologia()
		//{
		//	ViewData["IdCategoriaEnte"] = context.CategoriaEnte
		//		.ToDictionary(k => k.Id.ToString(), v => v.Descrizione);
		//	return View();
		//}

		[HttpPost]
		public IActionResult SelezionaTipologia(SelezionaEnteForm model)
		{
			#region Controlli
			//Vengono puliti gli input rimuovendo spazi e aggiungenzo gli zeri al CF dell'ente
			Utils.RimuoviSpazi(model);
			model.CodiceFiscaleEnte = model.CodiceFiscaleEnte?.PadLeft(11, '0');
			//Rieseguo i controlli sui campi dopo averli puliti
			ModelState.Clear();
			TryValidateModel(model);

			//Se arrivo sulla pagina senza aver selezionato il tipo di ente da registrare torno alla pagina di accesso
			if (!model.EnteTitolare.HasValue)
			{
				return RedirectToAction("Index", "Accesso");
			}
			//Controlli ente di accoglienza
			if (model.EnteTitolare == false)
			{
				if (!string.IsNullOrEmpty(model.CodiceFiscaleEnte))
				{
					var esisteEntePadre = context.UtenteEnte
						.Where(x => x.CodiceFiscaleEnte == model.CodiceFiscaleEnte && x.CodiceFiscaleEntePadre != null && x.Albo =="SCU")
						.FirstOrDefault();
					if (esisteEntePadre == null)
					{
						ModelState.AddModelError("CodiceFiscaleEnte", "Non si è autorizzati all'accesso da parte dell'ente Titolare.");
					}
					else
					{
						//model.IdCategoriaEnte = esisteEntePadre.IdCategoriaEnte;
						ModelState.Clear();
						TryValidateModel(model);
					}

				}
			}
			else
			{
				//Controlli Ente Titolare
				if (!string.IsNullOrEmpty(model.CodiceFiscaleEnte))
				{

					if (context.Registrazione
						.Where(x => x.CodiceFiscaleEnte == model.CodiceFiscaleEnte &&
									x.CodiceFiscaleRappresentanteLegale == User.Identity.Name && 
									x.Albo == "SCU")
						.Any())
					{
						ModelState.AddModelError("CodiceFiscaleEnte", "È già stata effettuata la registrazione per l'ente.");
					}
					if (context.UtenteEnte
						.Where(x => x.CodiceFiscaleEnte == model.CodiceFiscaleEnte &&
									x.CodiceFiscaleEntePadre != null &&
									x.Albo == "SCU")
						.Any())
					{
						ModelState.AddModelError("CodiceFiscaleEnte", "L'Ente risulta essere un'Ente di Accoglienza.");
					}
					if (context.UtenteEnte
						.Where(x => x.CodiceFiscaleEnte == model.CodiceFiscaleEnte &&
									x.IdCategoriaEnte != model.IdCategoriaEnte &&
									x.IdCategoriaEnte != null &&
									x.Albo == "SCU")
						.Any())
					{
						ModelState.AddModelError("IdCategoriaEnte", "L'ente non è della categoria selezionata.");
					}
				}
			}
			#endregion
			if (ModelState.IsValid)
			{
				//Imposto in sessione i valori impostati
				Registrazione registrazione = new Registrazione()
				{
					IdCategoriaEnte = model.IdCategoriaEnte,
					EnteTitolare = model.EnteTitolare,
					CodiceFiscaleEnte = model.CodiceFiscaleEnte,
					Albo = "SCU"
				};
				Log.SetEnte(model.CodiceFiscaleEnte);
				// Verifica se è è già presente l'ente (variazione di Rappresentante Legale)
				var enteGiaRegistrato = context.UtenteEnte.FirstOrDefault(x =>
					x.CodiceFiscaleEnte == registrazione.CodiceFiscaleEnte && 
					x.CodiceFiscale != User.Identity.Name &&
					x.Albo == "SCU");
				if (enteGiaRegistrato != null)
				{
					registrazione.Denominazione = enteGiaRegistrato.Denominazione;
					registrazione.Email = enteGiaRegistrato.Email;
					registrazione.VariazioneRappresentanteLegale = true;
					Log.Information(LogEvent.INIZIO_REGISTRAZIONE, $"Variazione Rappresentante Legale");
				}
				else
				{
					registrazione.VariazioneRappresentanteLegale = false;
					Log.Information(LogEvent.INIZIO_REGISTRAZIONE, $"Nuovo Ente {(model.EnteTitolare == true ? "Titolare" : "di Accoglienza")}");
				}
				SalvaRegistrazione(registrazione);

				return RedirectToAction("Index");
			}
			//Se provengo dal form di accesso non visualizzo gli errori
			if (model.fromAccesso == true)
			{
				ModelState.Clear();
			}
			ViewData["IdCategoriaEnte"] = context.CategoriaEnte
				.ToDictionary(k => k.Id.ToString(), v => v.Descrizione);
			return View(model);
		}

		[HttpGet]
		public IActionResult VariazioneRappresentante(){
			return View();
		}


		/// <summary>
		/// Gestisce la variazione del rappresentante legale di un ente del Servizio Civile Nazionale (SCN) - Non è possibile 
		/// </summary>
		/// <returns></returns>
		[HttpPost]
		public IActionResult VariazioneRappresentante(VariazioneRappresentanteForm model)
		{
			#region Controlli
			//Vengono puliti gli input rimuovendo spazi e aggiungenzo gli zeri al CF dell'ente
			Utils.RimuoviSpazi(model);
			model.CodiceFiscaleEnte = model.CodiceFiscaleEnte?.PadLeft(11, '0');
			//Rieseguo i controlli sui campi dopo averli puliti
			ModelState.Clear();
			TryValidateModel(model);
			UtenteEnte ente=null;
			//Controlli Ente Titolare
			if (ModelState.IsValid)
			{
				ente = context.UtenteEnte
					.Where(x => x.CodiceFiscaleEnte == model.CodiceFiscaleEnte &&
								x.Albo == "SCN")
					.FirstOrDefault();

				if (context.Registrazione
					.Where(x => x.CodiceFiscaleEnte == model.CodiceFiscaleEnte &&
								x.Albo=="SCN" &&
								x.CodiceFiscaleRappresentanteLegale == User.Identity.Name)
					.Any())
				{
					ModelState.AddModelError("CodiceFiscaleEnte", "È già stata effettuata la registrazione per l'ente.");
				}
				else
				if (ente==null)
				{
					ModelState.AddModelError("CodiceFiscaleEnte", "L'ente non è presente nell'albo del Servizio Civile Nazionale.");
				}
				else if (ente.CodiceFiscale== User.Identity.Name)
				{
					ModelState.AddModelError("CodiceFiscaleEnte", "Il rappresentante legale risulta coincidere con l'utente connesso. Procedere con la registrazione");
				}
				else if (ente.CodiceFiscaleEntePadre != null)
				{
					ModelState.AddModelError("CodiceFiscaleEnte", "L'Ente risulta essere un'Ente di Accoglienza.");
				}
			}
			#endregion
			if (ModelState.IsValid)
			{
				//Imposto in sessione i valori impostati
				Registrazione registrazione = new Registrazione()
				{
					IdCategoriaEnte = ente?.IdCategoriaEnte,
					EnteTitolare = true,
					CodiceFiscaleEnte = model.CodiceFiscaleEnte,
					Denominazione = ente.Denominazione,
					Email = ente.Email,
					VariazioneRappresentanteLegale = true,
					Albo="SCN"
			};
				Log.SetEnte(model.CodiceFiscaleEnte);
				Log.Information(LogEvent.INIZIO_REGISTRAZIONE, $"Variazione Rappresentante Legale");
				SalvaRegistrazione(registrazione);

				return RedirectToAction("Index");
			}
			//Se provengo dal form di accesso non visualizzo gli errori
			if (model.fromAccesso == true)
			{
				ModelState.Clear();
			}
			return View(model);
		}


		[HttpGet]
		public IActionResult Index()
		{
			Registrazione registrazione = GetRegistrazione();
			//if (context.UtenteEnte.Any(x=>x.CodiceFiscaleEnte==registrazione.CodiceFiscaleEnte && x.CodiceFiscale != User.Identity.Name))
			//{
			//	TempData["Warning"] = "Attenzione, per questo Ente risulta un Rappresentante Legale diverso. Si sta procedendo alla variazione di Rappresentente Legale.";
			//	return RedirectToAction("Variazione");
			//}
			if (registrazione == null)
			{
				return RedirectToAction("SelezionaTipologia");
			}
			//Controlli variazione Rappresentante legale
			if (registrazione.VariazioneRappresentanteLegale == true)
			{
				if (
				string.IsNullOrEmpty(registrazione.CodiceFiscaleEnte) ||
				string.IsNullOrEmpty(registrazione.Denominazione)
				)
				{
					return RedirectToAction("Index", "Accesso");
				}

			}
			else
			//Controlli Registrazione
			{

				if (
				registrazione.IdCategoriaEnte == null ||
				!registrazione.EnteTitolare.HasValue /*||
				context.UtenteEnte.Any(x => x.CodiceFiscaleEnte == registrazione.CodiceFiscaleEnte && x.CodiceFiscale != User.Identity.Name && x.Albo ==registrazione.Albo)*/
				)
				{
					return RedirectToAction("Index","Accesso");
				}

			}
			CaricaCombo(registrazione);

			return View(registrazione);
		}

		public IActionResult Cancella()
		{
			HttpContext.Session.Remove("Registrazione");
			Log.Information(LogEvent.CANCELLATI_DATI);
			return RedirectToAction("Index", "Accesso");
		}


		[HttpGet]
		public IActionResult Variazione()
		{
			Registrazione registrazione = GetRegistrazione();
			VariazioneRappresentanteLegale variazione = GetVariazione();
			return View(variazione);
		}

		[HttpPost]
		public IActionResult Variazione(VariazioneRappresentanteLegale model)
		{
			SalvaVariazione(model);
			if (ModelState["CodiceFiscaleRappresentanteLegale"]?.Errors?.Count == 0 && model.CodiceFiscaleRappresentanteLegale.ToUpper() != utente.FiscalNumber.ToUpper())
			{
				ModelState.AddModelError("CodiceFiscaleRappresentanteLegale", "Il codice fiscale non corrisponde all'utente connesso");
			}
			var enteGiaRegistrato = context.UtenteEnte.FirstOrDefault(x => x.CodiceFiscaleEnte == model.CodiceFiscaleEnte && x.CodiceFiscale != User.Identity.Name);
			if (model.DataNominaRappresentanteLegale <= enteGiaRegistrato.DataNominaRappresentanteLegale)
			{
				ModelState.AddModelError("DataNominaRappresentanteLegale", "La data di nomina risulta essere antecedente alla data di nomina dell'attuale Rappresentante Legale");
			}
			if (ModelState.IsValid)
			{

				return View("Riepilogo", model);
			}
			return Variazione();
		}


		[HttpPost]
		public IActionResult Riepilogo(Registrazione registrazione)
		{
			Utils.RimuoviSpazi(registrazione);
			registrazione.CodiceFiscaleRappresentanteLegale = (registrazione.CodiceFiscaleRappresentanteLegale??"").ToUpper();
			registrazione.CodiceFiscaleEnte = registrazione.CodiceFiscaleEnte?.PadLeft(11, '0');
			SalvaRegistrazione(registrazione);
			#region Lettura dati
			if (registrazione.DocumentoNomina?.Delete == true)
			{
				registrazione.DocumentoNomina = null;
			}
			if (registrazione.DocumentoNomina?.Download == true)
			{
				return File(registrazione.DocumentoNomina.Blob, registrazione.DocumentoNomina.MimeType, registrazione.DocumentoNomina.NomeFile);
			}
			ControllaRegistrazione(registrazione);


			#endregion

			if (!ModelState.IsValid)
			{
				CaricaCombo(registrazione);
				return View("Index", registrazione);
			}
			/*** Inserimento informazioni Registrazione ***/
			registrazione.Nome = utente.Name;
			registrazione.Cognome = utente.Surname;
			registrazione.DocumentoPdf = null;
			SalvaRegistrazione(registrazione);

			Log.Information(LogEvent.INSERITI_DATI);
			return View(registrazione);
		}



		[HttpGet]
		public IActionResult Riepilogo()
		{

			Registrazione registrazione = GetRegistrazione();
			if (registrazione == null)
			{
				return RedirectToAction("Index", "Accesso");
			}
			ControllaRegistrazione(registrazione);

			if (!ModelState.IsValid)
			{
				CaricaCombo(registrazione);
				return View("Index", registrazione);
			}

			return View(registrazione);
		}

		public IActionResult Download()
		{
			Registrazione registrazione = GetRegistrazione();
			if (registrazione == null)
			{
				return RedirectToAction("Index", "Accesso");
			}
			ControllaRegistrazione(registrazione);
			if (!ModelState.IsValid)
			{
				CaricaCombo(registrazione);
				return View("Index");
			}

			byte[] file;
			if (registrazione.DocumentoPdf == null)
			{
				registrazione.DocumentoPdf = GetPdfDomanda(registrazione);
				SalvaRegistrazione(registrazione);
			}
			Log.Information(LogEvent.DOWNLOAD_DOCUMENTO);
			return File(registrazione.DocumentoPdf, "application/pdf", "Documento.pdf");
		}

		public IActionResult Presenta(FileForm model)
		{
			ViewData["ModelFileForm"] = model;
			IFormFile documento = model.Documento;
			Registrazione registrazione = GetRegistrazione();
			if (registrazione == null)
			{
				return RedirectToAction("Index", "Accesso");
			}
			ModelState.Clear();
			/** Verifica integrità dati registrazione**/
			ControllaRegistrazione(registrazione);
			if (!ModelState.IsValid
				|| !registrazione.VariazioneRappresentanteLegale.HasValue
				|| registrazione.VariazioneRappresentanteLegale == false && (
				   registrazione.Comune == null
				|| registrazione.Provincia == null
				|| registrazione.Nome == null
				|| registrazione.Cognome == null
				|| registrazione.Categoria == null
				|| registrazione.TipologiaEnte == null
				)
			)
			{
				Log.Warning(LogEvent.REGISTRAZIONE_DATI_NON_INTEGRI);
				CaricaCombo(registrazione);
				return RedirectToAction("Index", "Registrazione");
			}
			if (model.Download == true)
			{
				ModelState.Clear();
				return Download();
			}
			TryValidateModel(model);

			if (!ModelState.IsValid)
			{
				/*string error = "";
				if (model.DichiarazionePrivacy != true)
				{
					error += "È necessario dare il consenso al trattamento dei dati.<br/>";
				}
				if (model.DichiarazioneRappresentanteLegale != true)
				{
					error += "È necessario dichiarare di essere il Rappresentante Legale.<br/>";
				}
				if (model.Documento == null)
				{
					error += "Non è stato caricato il documento.<br/>";
				}
				ViewData["Error"] = error;*/
				return View("Riepilogo", registrazione);
			}
			if (registrazione.DocumentoPdf == null)
			{
				ModelState.AddModelError("Documento", "Occorre scaricare il documento.");
				return View("Riepilogo", registrazione);
			}



			if (documento != null && documento.Length > 0)
			{
				byte[] file = GetPdfDomanda(registrazione);
				byte[] blob;
				using (var ms = new MemoryStream())
				{
					documento.CopyTo(ms);
					blob = ms.ToArray();
				}

				if (Parametri.ControlloFirma == "true")
				{

					/*** Controlli sulla Firma ***/
					DocumentoFirmato certificato = DocumentoFirmato.Carica(blob);
					if (certificato.NumeroFirme == 0)
					{
						using var sha1 = new System.Security.Cryptography.SHA1CryptoServiceProvider();
						ModelState.AddModelError("Documento", "Il documento non è stato firmato digitalmente");
						ViewData["Error"] = "Il documento non è stato firmato digitalmente";
						Documento documentoErrato = new Documento()
						{
							Blob = blob,
							NomeFile = documento.FileName,
							Dimensione = (int)documento.Length,
							Hash = string.Concat(sha1.ComputeHash(blob).Select(x => x.ToString("X2"))),
							MimeType = documento.ContentType
						};
						context.Documento.Add(documentoErrato);
						context.SaveChanges();
						Entity entity = new Entity()
						{
							Id = documentoErrato.Id,
							Name = "Documento"
						};
						Log.Warning(LogEvent.ERRORE_FIRMA, "Firma non presente", entity: entity);
						return View("Riepilogo", registrazione);
					}
					if (!certificato.VerificaFirma(registrazione.CodiceFiscaleRappresentanteLegale))
					{
						using var sha1 = new System.Security.Cryptography.SHA1CryptoServiceProvider();
						ModelState.AddModelError("Documento", "Non è presente la firma del Rappresentante legale");
						ViewData["Error"] = "Non è presente la firma del Rappresentante legale";
						Documento documentoErrato = new Documento()
						{
							Blob = blob,
							NomeFile = documento.FileName,
							Dimensione = (int)documento.Length,
							Hash = string.Concat(sha1.ComputeHash(blob).Select(x => x.ToString("X2"))),
							MimeType = documento.ContentType
						};
						context.Documento.Add(documentoErrato);
						context.SaveChanges();
						Entity entity = new Entity()
						{
							Id = documentoErrato.Id,
							Name = "Documento"
						};
						Log.Warning(LogEvent.ERRORE_FIRMA, "CF non corrispondente", entity: entity);
						return View("Riepilogo", registrazione);
					}
					if (!certificato.VerificaDocumento(registrazione.DocumentoPdf))
					{
						using var sha1 = new System.Security.Cryptography.SHA1CryptoServiceProvider();
						ModelState.AddModelError("Documento", "Il documento firmato non corrisponde al documento scaricato.");
						ViewData["Error"] = "Il documento firmato non corrisponde.";
						Documento documentoErrato = new Documento()
						{
							Blob = blob,
							NomeFile = documento.FileName,
							Dimensione = (int)documento.Length,
							Hash = string.Concat(sha1.ComputeHash(blob).Select(x => x.ToString("X2"))),
							MimeType = documento.ContentType
						};
						context.Documento.Add(documentoErrato);
						context.SaveChanges();
						Entity entity = new Entity()
						{
							Id = documentoErrato.Id,
							Name = "Documento"
						};
						Log.Warning(LogEvent.ERRORE_FIRMA, "documento non corrispondente", entity: entity);

						return View("Riepilogo", registrazione);
					}
				}

				using (TransactionScope scope = new TransactionScope())
				{
					/* Controlli Dati*/
					context.Comune
						.SingleOrDefault(x => x.Id == registrazione.IdComuneEnte);
					context.Provincia
						.SingleOrDefault(x => x.Id == registrazione.IdProvinciaEnte);
					context.TipologiaEnte
						.SingleOrDefault(x => x.Id == registrazione.IdTipologiaEnte);
					context.CategoriaEnte
						.SingleOrDefault(x => x.Id == registrazione.IdCategoriaEnte);
					context.UtenteEnte
						.Where(x => x.CodiceFiscaleEnte == registrazione.CodiceFiscaleEnte);
					bool giàpresente = context.Registrazione
						.Where(x =>
							x.CodiceFiscaleRappresentanteLegale == User.Identity.Name &&
							x.CodiceFiscaleEnte == registrazione.CodiceFiscaleEnte &&
							x.Albo==registrazione.Albo)
						.Any();

					//TODO Controlli Su Enti presenti in Helios
					if (giàpresente)
					{
						TempData["Warning"] = "Registrazione già effettuata. Si può accedere al Sitema per conto dell'Ente";
						Log.Warning(LogEvent.REGISTRAZIONE_GIA_EFFETTUATA);
						return RedirectToAction("Index", "Accesso");
					}
					using var sha1 = new System.Security.Cryptography.SHA1CryptoServiceProvider();

					Documento documentoRegistrazione = new Documento()
					{
						Blob = blob,
						NomeFile = documento.FileName,
						Dimensione = (int)documento.Length,
						Hash = string.Concat(sha1.ComputeHash(blob).Select(x => x.ToString("X2"))),
						MimeType = documento.ContentType
					};
					Soggetto soggetto = context.Soggetto.FirstOrDefault(s => s.CodiceFiscale == registrazione.CodiceFiscaleRappresentanteLegale);
					if (soggetto == null)
					{
						context.Soggetto.Add(new Soggetto
						{
							CodiceFiscale = utente.FiscalNumber,
							Nome = utente.Name,
							Cognome = utente.Surname
						});
					}
					else
					{
						soggetto.Nome = utente.Name;
						soggetto.Cognome = utente.Surname;
					}

					registrazione.Documento = documentoRegistrazione;
					registrazione.DataInserimento = DateTime.Now;
					registrazione.DichiarazionePrivacy = true;
					registrazione.DichiarazioneRappresentanteLegale = true;
					/* Annullo i valori per evitare di inserire gli oggetti*/
					registrazione.Categoria = null;
					registrazione.Comune = null;
					registrazione.Provincia = null;
					registrazione.TipologiaEnte = null;

					context.Registrazione.Add(registrazione);
					context.SaveChanges();

					var result = context.RegistraEnte(
						registrazione.Id,
						registrazione.DataInserimento,
						registrazione.CodiceFiscaleEnte,
						registrazione.CodiceFiscaleRappresentanteLegale ?? "".ToUpper(),
						registrazione.Denominazione,
						utente.Name,
						utente.Surname,
						DateTime.Parse(utente.DateOfBirth),
						utente.PlaceOfBirth?? registrazione.CodiceFiscaleRappresentanteLegale.Substring(11, 4).ToUpper(),
						registrazione.DataNominaRappresentanteLegale,
						registrazione.EnteTitolare,
						registrazione.IdCategoriaEnte,
						registrazione.IdTipologiaEnte,
						registrazione.IdProvinciaEnte,
						registrazione.IdComuneEnte,
						registrazione.Via,
						registrazione.Civico,
						registrazione.CAP,
						registrazione.Telefono,
						registrazione.Email,
						registrazione.PEC,
						registrazione.Sito,
						registrazione.DichiarazionePrivacy,
						registrazione.DichiarazioneRappresentanteLegale,
						registrazione.IdDocumento,
						registrazione.VariazioneRappresentanteLegale,
						registrazione.DataProtocollazione,
						registrazione.NumeroProtocollo,
						registrazione.DataProtocollo,
						registrazione.DataInvioEmail,
						registrazione.IdDocumentoNomina,
						registrazione.Albo
					);
					if (!result.Esito)
					{
						ViewData["Error"] = result.Messaggio;
						return View("Riepilogo", registrazione);
					}

					TempData["Message"] = "Registrazione Effettuata. Si può accedere all'applicativo Unico";
					//Eliminazione dati sessione;
					HttpContext.Session.Remove("Registrazione");

					scope.Complete();
					Log.Information(LogEvent.REGISTRAZIONE_EFFETTUATA,
									entity: new Entity(registrazione.Id, "Registrazione"));
					return RedirectToAction("Index", "Accesso");
				}
			}
			ViewData["Error"] = "Caricare un documento";
			return View("Riepilogo", registrazione);

		}

		[AllowAnonymous]
		[HttpPost]
		public IActionResult GetComuni(int idProvincia)
		{
			var comuni = Comuni.ElencoComuni
				.Where(c => c.IdProvincia == idProvincia)
				.OrderBy(c => c.Nome)
				.Select(c => new { text = c.Nome, value = c.Id })
				.ToList();

			return Json(comuni);
		}



		private byte[] GetPdfDomanda(Registrazione registrazione)
		{
			using (MemoryStream streamDomanda = new MemoryStream())
			{
				string html = renderService.RenderToStringAsync("RiepilogoRegistrazione", registrazione).Result;

				PdfSharp.Pdf.PdfDocument pdfDomanda = PdfGenerator.GeneratePdf(html, PageSize.A4, 40);
				pdfDomanda.Save(streamDomanda);
				return streamDomanda.ToArray();
			}
		}

		private void SalvaRegistrazione(Registrazione registrazione)
		{
			JsonSerializerSettings jsonSettings = new JsonSerializerSettings();
			jsonSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
			Documento documentoNomina = registrazione.DocumentoNomina;
			if (documentoNomina?.File != null)
			{
				documentoNomina.Load();
			}
			else
			{
				if (documentoNomina?.Delete == true)
				{
					registrazione.DocumentoNomina = null;
				}
				else
				{
					registrazione.DocumentoNomina = GetRegistrazione()?.DocumentoNomina;
				}
			}
			HttpContext.Session.SetString("Registrazione", JsonConvert.SerializeObject(registrazione, jsonSettings));
		}

		private void SalvaVariazione(VariazioneRappresentanteLegale variazione)
		{
			JsonSerializerSettings jsonSettings = new JsonSerializerSettings();
			jsonSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
			HttpContext.Session.SetString("VariazioneRappresentanteLegale", JsonConvert.SerializeObject(variazione, jsonSettings));
		}

		private void ControllaRegistrazione(Registrazione registrazione)
		{
			if (registrazione.VariazioneRappresentanteLegale == true)
			{
				/** CONTROLLI VARIAZIONE **/
				ModelState.Clear();
				VariazioneRappresentanteLegale variazione = new VariazioneRappresentanteLegale()
				{
					CodiceFiscaleEnte = registrazione.CodiceFiscaleEnte,
					CodiceFiscaleRappresentanteLegale = registrazione.CodiceFiscaleRappresentanteLegale.ToUpper(),
					DataNominaRappresentanteLegale = registrazione.DataNominaRappresentanteLegale,
					Denominazione = registrazione.Denominazione
				};
				TryValidateModel(variazione);
				if ((ModelState["CodiceFiscaleRappresentanteLegale"]?.Errors?.Count ?? 0) == 0 && (registrazione.CodiceFiscaleRappresentanteLegale ?? "").ToUpper() != utente.FiscalNumber.ToUpper())
				{
					ModelState.AddModelError("CodiceFiscaleRappresentanteLegale", "Il codice fiscale non corrisponde all'utente connesso");
				}
				var enteGiaRegistrato = context.UtenteEnte.FirstOrDefault(x => x.CodiceFiscaleEnte == registrazione.CodiceFiscaleEnte && x.CodiceFiscale != User.Identity.Name);
				if (registrazione.DataNominaRappresentanteLegale <= enteGiaRegistrato.DataNominaRappresentanteLegale)
				{
					ModelState.AddModelError("DataNominaRappresentanteLegale", "La data di nomina risulta essere antecedente alla data di nomina dell'attuale Rappresentante Legale");
				}
				if (registrazione.DocumentoNomina?.Blob == null)
				{
					ModelState.AddModelError("DocumentoNomina", "È necessario allegare l'atto nomina/altro");
				}
				else if (!IsPDF(registrazione.DocumentoNomina.Blob))
				{
					ModelState.AddModelError("DocumentoNomina", "Il documento deve essere un file PDF valido");
				}
				/** FINECONTROLLI VARIAZIONE **/
			}
			else
			{
				/** CONTROLLI REGISTRAZIONE **/
				//Ricalcolo dopo rimozione spazi
				ModelState.Clear();
				TryValidateModel(registrazione);
				Comune comuneSelezionato = Comuni.ElencoComuni
					.Where(c => c.Id == registrazione.IdComuneEnte)
					.FirstOrDefault();
				Provincia provinciaSelezionata = Comuni.ElencoProvince
					.Where(c => c.Id == registrazione.IdProvinciaEnte)
					.FirstOrDefault();
				CategoriaEnte categoria = context.CategoriaEnte.FirstOrDefault(c => c.Id == registrazione.IdCategoriaEnte);
				TipologiaEnte tipologia = context.TipologiaEnte.FirstOrDefault(c => c.Id == registrazione.IdTipologiaEnte);

				//Tolgo obbligatorietà PEC Per gli enti di accoglienza
				if (registrazione.EnteTitolare == true && registrazione.PEC == null)
				{
					ModelState.AddModelError("PEC", "Campo obbligatorio");
				}

				if ((ModelState["CodiceFiscaleRappresentanteLegale"]?.Errors?.Count ?? 0) == 0 && registrazione.CodiceFiscaleRappresentanteLegale.ToUpper() != utente.FiscalNumber.ToUpper())
				{
					ModelState.AddModelError("CodiceFiscaleRappresentanteLegale", "Il codice fiscale non corrisponde all'utente connesso");
				}
				if (registrazione.IdComuneEnte != null && comuneSelezionato == null)
				{
					ModelState.AddModelError("IdComuneEnte", "Comune non valido");
				}
				else if (comuneSelezionato != null && registrazione.IdProvinciaEnte != null && comuneSelezionato.IdProvincia != registrazione.IdProvinciaEnte)
				{
					ModelState.AddModelError("IdComuneEnte", "Comune non valido per la provincia selezionata");
				}
				if (registrazione.IdProvinciaEnte != null && provinciaSelezionata == null)
				{
					ModelState.AddModelError("IdProvinciaEnte", "Provincia non valida");
				}
				if (registrazione.IdCategoriaEnte != null && categoria == null)
				{
					ModelState.AddModelError("IdCategoriaEnte", "Categoria ente non valida");
				}
				if (registrazione.IdTipologiaEnte != null && tipologia == null)
				{
					ModelState.AddModelError("IdTipologiaEnte", "Tipologia ente non valida");
				}
				if (registrazione.DocumentoNomina?.Blob != null && !IsPDF(registrazione.DocumentoNomina.Blob))
				{
					ModelState.AddModelError("DocumentoNomina", "Il documento deve essere un file PDF valido");
				}
				registrazione.Comune = comuneSelezionato;
				registrazione.Provincia = provinciaSelezionata;

				registrazione.Categoria = categoria;
				registrazione.TipologiaEnte = tipologia;

				/***  FINE CONTROLLI REGISTRAZIONE***/
			}

		}
		/// <summary>
		/// Verifica se il file è un documento PDF leggibile
		/// </summary>
		/// <param name="bytes">File PDF</param>
		/// <returns>Indica se il file è un PDF o meno</returns>
		private static bool IsPDF(byte[] bytes)
		{
			
			var sb = new StringBuilder();
			try
			{
				var reader = new PdfReader(bytes);
				var numberOfPages = reader.NumberOfPages;

				for (var currentPageIndex = 1; currentPageIndex <= numberOfPages; currentPageIndex++)
				{
					sb.Append(PdfTextExtractor.GetTextFromPage(reader, currentPageIndex));
				}
			}
			catch (Exception)
			{
				return false;
			}
			return true;
		}
	}



}
