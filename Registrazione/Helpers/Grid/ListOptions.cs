using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RegistrazioneSistemaUnico.Helpers.Grid
{
	public class ListOptions
	{
		public string ColumnNames { get; set; }
		public List<Column> CustomColumns { get; set; }

		/// <summary>
		/// Indica se la prima riga del file Excel è una riga di intestazione o meno
		/// </summary>
		public bool HasHeader { get; set; } = true;

	}
}
