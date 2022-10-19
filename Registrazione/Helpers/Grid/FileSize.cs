using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RegistrazioneSistemaUnico.Helpers.Grid
{
	public class FileSize
	{
		/// <summary>
		/// Restituisce una string a che indica la dimensione di un file dati i bytes
		/// </summary>
		/// <param name="bytes">numero di bytes</param>
		/// <returns>Stringa con la descrizione della dimensione</returns>
		public static string FormatToString(long bytes)
		{
			List<string> dimensioni = new List<string>
			{
				"B",
				"KB",
				"MB",
				"GB",
				"TB"
			};
			decimal value = bytes;

			foreach (string dimensione in dimensioni)
			{
				if (value < 1024)
				{
					return Math.Round(value, 3) + dimensione;
				}
				value /= 1024;
			}

			return Math.Round(value, 3) + dimensioni.Last();
		}
	}
}
