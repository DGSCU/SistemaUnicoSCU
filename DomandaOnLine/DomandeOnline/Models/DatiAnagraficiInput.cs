using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DomandeOnline.Models
{
	public class DatiAnagraficiInput
	{
		public int? IdNazioneCittadinanza { get; set; }

		[DisplayName("Comune di Residenza")]
		[MaxLength(200)]
		public string ComuneResidenza { get; set; }

		[DisplayName("Provincia di Residenza")]
		[MaxLength(200)]
		public string ProvinciaResidenza { get; set; }

		[DisplayName("Via di Residenza")]
		[MaxLength(200)]
		public string ViaResidenza { get; set; }

		[DisplayName("Numero civico di Residenza")]
		[MaxLength(200)]
		public string CivicoResidenza { get; set; }

		[DisplayName("CAP di Residenza")]
		[MaxLength(200)]
		public string CapResidenza { get; set; }

		[DisplayName("Comune Recapito")]
		[MaxLength(200)]
		public string ComuneRecapito { get; set; }

		[DisplayName("Provincia Recapito")]
		[MaxLength(200)]
		public string ProvinciaRecapito { get; set; }

		[DisplayName("Via Recapito")]
		[MaxLength(200)]
		public string ViaRecapito { get; set; }

		[DisplayName("Numero Civico Recapito")]
		[MaxLength(200)]
		public string CivicoRecapito { get; set; }

		[DisplayName("CAP Recapito")]
		[MaxLength(200)]
		public string CapRecapito { get; set; }

		public int? IdTitoloStudio { get; set; }

		public string FormazioneAnagraficaIstituto { get; set; }
		public string FormazioneAnagraficaDisciplina { get; set; }
		public bool? FormazioneAnagraficaItalia { get; set; }
		public int? FormazioneAnagraficaAnno { get; set; }
		public string FormazioneAnagraficaEnte { get; set; }

		public int? IdSpecializzazione { get; set; }

		public string CodiceMinoriOpportunita { get; set; }
		public bool? DichiarazioneMinoriOpportunita { get; set; }

		public int? IdMotivazione { get; set; }

		public string CodiceDichiarazioneCittadinanza { get; set; }

		public bool? PresaVisione445 { get; set; }

		public bool? NonCondanneOk { get; set; }

		public bool? TrasferimentoSedeOk { get; set; }

		public bool? TrasferimentoProgettoOk { get; set; }

		public bool? AltreDichiarazioniOk { get; set; }

		public bool? PrivacyPresaVisione { get; set; }

		public bool? PrivacyConsenso { get; set; }

		public bool? ConfermaResidenza { get; set; }

		public bool? ResidenzaEstera { get; set; }

		public bool? ResidenzaEsteraButton { get; set; }

		public bool? DichiarazioneResidenzaOK { get; set; }
		public bool? DichiarazioneRequisitiGaranziaGiovani { get; set; }
		public DateTime? DataPresaInCaricoGaranziaGiovani { get; set; }
		public int? DataPresaInCaricoGaranziaGiovaniGiorno { get; set; }
		public int? DataPresaInCaricoGaranziaGiovaniMese { get; set; }
		public int? DataPresaInCaricoGaranziaGiovaniAnno { get; set; }
		public bool? DataPresaInCaricoGaranziaGiovaniErrore { get; set; }
		public string DataPresaInCaricoGaranziaGiovaniText { get; set; }
		public string LuogoPresaInCaricoGaranziaGiovani { get; set; }
		public DateTime? DataDIDGaranziaGiovani { get; set; }
		public int? DataDIDGaranziaGiovaniGiorno { get; set; }
		public int? DataDIDGaranziaGiovaniMese { get; set; }
		public int? DataDIDGaranziaGiovaniAnno { get; set; }
		public bool? DataDIDGaranziaGiovaniErrore { get; set; }
		public string DataDIDGaranziaGiovaniText { get; set; }
		public string LuogoDIDGaranziaGiovani { get; set; }
		public bool? AlternativaRequisitiGaranziaGiovani { get; set; }

		[DisplayName("Nazione Residenza")]
		[MaxLength(200)]
		public string NazioneResidenza { get; set; }

		[DisplayName("Indirizzo Completo Residenza")]
		[MaxLength(200)]
		public string ResidenzaIndirizzoCompleto { get; set; }

		public int? IscrizioneSuperioreAnno { get; set; }

		[DisplayName("Istituto iscrizione scuola media superiore")]
		[MaxLength(200)]
		public string IscrizioneSuperioreIstituto { get; set; }

		public int? IscrizioneLaureaAnno { get; set; }

		[DisplayName("Facoltà iscrizione di laurea")]
		[MaxLength(200)] public string IscrizioneLaureaCorso { get; set; }

		[DisplayName("Istituto iscrizione di laurea")]
		[MaxLength(200)]
		public string IscrizioneLaureaIstituto { get; set; }

		public string Action { get; set; }

	}
}