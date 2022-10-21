using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DomandeOnline.Data
{
	public partial class Progetto
	{
		public static class Tipo
		{
			public const string ITALIA = "ITALIA";
			public const string ESTERO = "ESTERO";
		}

		public List<Regione> RegioniAmmesse;
		public List<Provincia> ProvinceAmmesse;
		public bool Asse1NEET { get { return ProvinceAmmesse != null; } }
		public bool Asse1bisDisoccupati { get { return RegioniAmmesse != null; } }
	}

	public class ProgettoDto
	{
		public string CodiceProgetto { get; set; }
		public string TipoProgetto { get; set; }
		public int CodiceSede { get; set; }
		public string IndirizzoSede { get; set; }
		public string Area { get; set; }
		public string TitoloProgetto { get; set; }
		public string CodiceEnte { get; set; }
		public string NomeEnte { get; set; }
		public string Regione { get; set; }
		public string Provincia { get; set; }
		public string Comune { get; set; }
		public string Settore { get; set; }
		public string Misure { get; set; }
		public int NumeroGiovaniMinoriOpportunità { get; set; }
		public string EsteroUE { get; set; }
		public string Tutoraggio { get; set; }
		public bool Preferito { get; set; }
		public int NumeroDomande { get; set; }
		public int? IdTipoGG { get; set; }
		public string EnteAttuatore { get; set; }

	}
}