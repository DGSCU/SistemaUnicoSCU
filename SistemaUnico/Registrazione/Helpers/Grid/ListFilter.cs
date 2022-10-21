using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RegistrazioneSistemaUnico.Helpers.Grid
{
	/// <summary>
	/// Tipologia di ordinamento
	/// </summary>
	public enum OrderType
	{
		/// <summary>
		/// cresente
		/// </summary>
		Ascending = 0,
		/// <summary>
		/// Decrescente
		/// </summary>
		Descending = 1
	}

	/// <summary>
	/// Modello generico per la restituzione di una lista di elementi
	/// </summary>
	public abstract class ListFilter
	{
		private int? page;
		private string order;

		/// <summary>
		/// Tipo di ordimanento true=crescente, false=decrescente (Default = true)
		/// </summary>
		public OrderType OrderType { get; set; } = OrderType.Ascending;

		/// <summary>
		/// Ordinamento da applicare (possono essere inserite più colonne separate da virgole)
		/// </summary>
		public string OrderColumns { get { return order ?? CurrentOrder; } set { order = value; } }

		/// <summary>
		/// Attuale ordinamento
		/// </summary>
		public string CurrentOrder { get; set; }

		/// <summary>
		/// Pagina da visualizzare (default 1)
		/// </summary>
		public int Page { get { return page ?? CurrentPage ?? 1; } set { page = value; } }

		/// <summary>
		/// Pagina attuale
		/// </summary>
		public int? CurrentPage { get; set; }

		/// <summary>
		/// Numero massimo di elementi visualizzati per pagina (default 20)
		/// </summary>
		public int RecordsPerPage { get; set; } = 20;
	}
}
