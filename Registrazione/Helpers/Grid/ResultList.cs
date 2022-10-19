using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RegistrazioneSistemaUnico.Helpers.Grid
{
	/// <summary>
	/// Informazioni relative alla lista restituita, quali numero totale di elementi, paginazione etc.
	/// </summary>
	/// <typeparam name="T">Tipologia dell'elemento della lista</typeparam>
	public class ResultList<T>
	{
		/// <summary>
		/// Numero totale elementi
		/// </summary>
		public int Count { get; set; }

		/// <summary>
		/// Numero di pagina estratta
		/// </summary>
		public int Page { get; set; }

		/// <summary>
		/// Numero di elementi per pagina
		/// </summary>
		public int RecordsPerPage { get; set; }

		/// <summary>
		/// Numero totale di pagine
		/// </summary>
		public int PageCount { get; set; }

		/// <summary>
		/// Informazioni relativi alla ricerca effettuata
		/// </summary>
		public string Message { get; set; }

		/// <summary>
		/// Lista degli oggetti restituiti
		/// </summary>
		public List<T> List { get; set; }


		/// <summary>
		/// Ordinamento effettuato sulla lista
		/// </summary>
		public string Order { get; set; }
	}


}
