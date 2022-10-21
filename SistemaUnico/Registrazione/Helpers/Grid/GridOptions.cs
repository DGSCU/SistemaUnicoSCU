using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RegistrazioneSistemaUnico.Helpers.Grid
{
	public enum GridPaginatorType
	{
		None = 0,
		Top = 1,
		Bottom = 2,
		Both = 3
	}
	public class GridOptions : ListOptions
	{
		public GridOptions()
		{
			Paginator = GridPaginatorType.Top;
			OrderColumns = true;
			AjaxPost = false;
		}
		public string GridId { get; set; }
		public string EmptyText { get; set; }
		public bool OrderColumns { get; set; }
		/// <summary>
		/// Indica se la chiamata verrà fatta tramite submit o Ajax (default false)
		/// </summary>
		public bool AjaxPost { get; set; }
		public string RowTemplate { get; set; }
		public GridPaginatorType Paginator { get; set; }
		public bool ShowResults { get; set; } = true;
		public Func<object, string> CustomRowClass { get; set; }
	}
}


