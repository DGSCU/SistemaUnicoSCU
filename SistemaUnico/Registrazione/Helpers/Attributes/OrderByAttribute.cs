using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RegistrazioneSistemaUnico.Helpers.Attributes
{
	/// <summary>
	/// Indica per quali colonne deve essere ordinato il campo
	/// </summary>
	public class OrderByAttribute : Attribute
	{
		public OrderByAttribute(string columnNames)
		{
			ColumnNames = columnNames;
		}

		public string ColumnNames { get; set; }
	}
}
