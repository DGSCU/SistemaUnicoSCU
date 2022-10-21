using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DomandeOnline.Data
{
	public partial class TitoloStudio
	{
		public const int NESSUNO = -1;
		public const int SCUOLA_PRIMARIA = 1;
		public const int DIPLOMA_1_GRADO = 2;
		public const int DIPLOMA_2_GRADO = 3;
		public const int LAUREA_TRIENNALE = 4;
		public const int LAUREA_SPECIALISTICA = 5;
		public const int ESTERO = 6;

		public static readonly List<int> TitoliAmmessiBassaScolarizzazione = new List<int>()
		{
			NESSUNO,
			SCUOLA_PRIMARIA,
			DIPLOMA_1_GRADO
		};
	}
}