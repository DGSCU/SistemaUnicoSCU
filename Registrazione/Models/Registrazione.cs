using Microsoft.AspNetCore.Http;
using RegistrazioneSistemaUnico.Helpers;
using RegistrazioneSistemaUnico.Helpers.Attributes;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace RegistrazioneSistemaUnico.Models
{
	public class Registrazione
	{
		public int Id { get; set; }

		/// <summary>
		/// Data di inserimento della registrazione (sarà automaticamennte valorizzata alla creazione del record)
		/// </summary>
		public DateTime DataInserimento { get; set; }

		/// <summary>
		/// codice Fiscalle dell'Ente. Deve essere una partita IVA formalemente corretta
		/// </summary>
		[DisplayName("Codice Fiscale Ente")]
		[Required(ErrorMessage = "Campo obbligatorio")]
		[Hint("Inserire il Codice Fiscale")]
		[Placeholder("Inserire il codice fiscale")]
		[PartitaIva(ErrorMessage = "Codice Fiscale Ente non valido")]
		public string CodiceFiscaleEnte { get; set; }

		/// <summary>
		/// Codice fiscale del Rappresentante legale. Deve essere un codice fiscale formalmente valido
		/// </summary>
		[DisplayName("Codice Fiscale Rappresentante Legale")]
		[Required(ErrorMessage = "Campo obbligatorio")]
		[Placeholder("Inserire il codice fiscale")]
		[CodiceFiscale]
		public string CodiceFiscaleRappresentanteLegale { get; set; }

		/// <summary>
		/// Denominazione dell'Ente
		/// </summary>
		[Required(ErrorMessage = "Campo obbligatorio")]
		[DisplayName("Denominazione Ente")]
		[Placeholder("Inserire la denominazione")]
		[Length(200,"Il valore non può superare i {0} caratteri")]
		public string Denominazione { get; set; }

		[Required(ErrorMessage = "Campo obbligatorio")]
		[Date(ConsentiDataFutura = false)]
		[DisplayName("Data Nomina Rappresentante Legale")]
		[DataType(DataType.Date,ErrorMessage ="Data non valida")]
		[Placeholder("gg/mm/aaaa")]
		[Hint("Inserire la data di nomina del Rappresentante Legale")]
		public DateTime? DataNominaRappresentanteLegale { get; set; }

		public int? IdCategoriaEnte { get; set; }


		[Required(ErrorMessage = "Campo obbligatorio")]
		[DisplayName("Tipologia Ente")]
		[Placeholder("Selezionare la tipologia dell'Ente")]
		public int? IdTipologiaEnte { get; set; }

		[Required(ErrorMessage = "Campo obbligatorio")]
		[DisplayName("Provincia")]
		[Placeholder("Selezionare la provincia")]
		public int? IdProvinciaEnte { get; set; }

		[Required(ErrorMessage = "Campo obbligatorio")]
		[DisplayName("Comune")]
		[Placeholder("Selezionare il Comune")]
		public int? IdComuneEnte { get; set; }

		[Required(ErrorMessage = "Campo obbligatorio")]
		[DisplayName("Indirizzo")]
		[Placeholder("Inserire l'indirizzo")]
		[Length(200,"Il valore non può superare i {0} caratteri")]
		public string Via { get; set; }

		[Required(ErrorMessage = "Campo obbligatorio")]
		[DisplayName("Numero Civico")]
		[Placeholder("Inserire il numero civico")]
		[Length(10,"Il valore non può superare i {0} caratteri")]
		public string Civico { get; set; }

		[Required(ErrorMessage = "Campo obbligatorio")]
		[Placeholder("Inserire il CAP")]
		[Length(5,ErrorMessage = "Il valore deve essere di {0} caratteri")]
		[Cap]
		public string CAP { get; set; }

		[Required(ErrorMessage = "Campo obbligatorio")]
		[Placeholder("Inserire il numero di telefono")]
		[Length(100,"Il valore non può superare i {0} caratteri")]
		[Telefono]
		public string Telefono { get; set; }

		[DisplayName("E-mail")]
		[Email(ErrorMessage = "Indirizzo e-mail non valido")]
		[Required(ErrorMessage = "Campo obbligatorio")]
		[Placeholder("Inserire l'e-mail")]
		[Length(100,"Il valore non può superare i {0} caratteri")]
		public string Email { get; set; }

		[Hint("Inserire la PEC")]
		[Email(ErrorMessage = "PEC non valida")]
		[Placeholder("Inserire la PEC")]
		[Length(100,"Il valore non può superare i {0} caratteri")]
		public string PEC { get; set; }

		[Required(ErrorMessage = "Campo obbligatorio")]
		[Placeholder("Inserire l'url del sito")]
		[Length(200,"Il valore non può superare i {0} caratteri")]
		[Website]
		public string Sito { get; set; }

		public bool? EnteTitolare { get; set; }

		public int? IdDocumento { get; set; }

		//Data in cui è stata fatta la protocollazione della registrazione.
		public DateTime? DataProtocollazione { get; set; }
		public DateTime? DataProtocollo { get; set; }
		public string NumeroProtocollo { get; set; }

		/// <summary>
		/// Identificativo documento di nomina del Rappresentante Legale
		/// </summary>
		public int? IdDocumentoNomina { get; set; }

		//Data in cui è stata fatta la protocollazione della registrazione.
		public DateTime? DataInvioEmail { get; set; }

		public Comune Comune { get; set; }

		public Provincia Provincia { get; set; }
		public CategoriaEnte Categoria { get; set; }
		public Documento Documento { get; set; }

		[DisplayName("Carica Atto Nomina/Altro")]
		public Documento DocumentoNomina { get; set; }
		public TipologiaEnte TipologiaEnte { get; set; }
		public bool? VariazioneRappresentanteLegale { get; set; }
		public bool? DichiarazionePrivacy { get; set; }
		public bool? DichiarazioneRappresentanteLegale { get; set; }


		public string Indirizzo { get
			{
				return $"{Via}, {Civico} - {CAP} {Comune?.Nome} ({Provincia?.Sigla})";
			}
		}

		public string TipoEnte
		{
			get
			{
				return EnteTitolare==true?"Titolare":"di Accoglienza";
			}
		}

		//Dati non mappati sul DB
		[NotMapped]
		public bool GiaRegistrato { get; set; }

		[NotMapped]
		public string Nome { get; set; }

		[NotMapped]
		public string Cognome { get; set; }

		[NotMapped]
		public string Genere { get; set; }

		[NotMapped]
		public byte[] DocumentoPdf { get; set; }

		public string Albo { get; set; }

	}
}
