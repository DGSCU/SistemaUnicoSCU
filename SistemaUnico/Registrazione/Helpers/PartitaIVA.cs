using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RegistrazioneSistemaUnico.Helpers
{
	/// <summary>
	/// Description of Partitaiva.	
	/// </summary>
	public class PartitaIva
	{
		private static int[] ListaPari = { 0, 2, 4, 6, 8, 1, 3, 5, 7, 9 };

		public PartitaIva()
		{
		}
		public static bool ControllaPartitaIva(string PartitaIva)
		{
			// normalizziamo la cifra
			//if (PartitaIva.Length < 11)
			//	PartitaIva = PartitaIva.PadLeft(11, '0');
			// lunghezza errata non fa neanche il controllo
			if (PartitaIva.Length != 11)
				return false;
			int Somma = 0;
			for (int k = 0; k < 11; k++)
			{
				string s = PartitaIva.Substring(k, 1);
				// otteniamo contemporaneamente
				// il valore, la posizione e testiamo se ci sono
				// caratteri non numerici
				int i = "0123456789".IndexOf(s);
				if (i == -1)
					return false;
				int x = int.Parse(s);
				if (k % 2 == 1) // Pari perchè iniziamo da zero
					x = ListaPari[i];
				Somma += x;
			}
			return ((Somma % 10 == 0) && (Somma != 0));
		}

	}
}
