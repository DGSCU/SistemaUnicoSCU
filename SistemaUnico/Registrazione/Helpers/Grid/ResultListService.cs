using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace RegistrazioneSistemaUnico.Helpers.Grid
{
	/// <summary>
	/// Classe di gestione delle liste e griglie
	/// </summary>
	public static class ResultListService
	{
		/// <summary>
		/// Classe generica di gestione dell'ordinamento e paginazione di una query
		/// </summary>
		/// <typeparam name="Entity">
		/// Tipo dell'entità di cui si effettua la query
		/// </typeparam>
		/// <param name="query">quesry da ordinare e paginare</param>
		/// <param name="model">filtri da applicare alla query</param>
		/// <param name="mapperConfiguration">Eventuale mapping da effettuare</param>
		/// <returns></returns>
		public static ResultList<Entity> GetOrderedList<Entity>(IQueryable<Entity> query, ListFilter model)
		{
			int count = query.Count();
			List<string> columnsToOrder = new List<string>();
			List<string> errors = new List<string>();
			string orderColumn = model.OrderColumns;
			if (!string.IsNullOrEmpty(orderColumn) && orderColumn != "default")
			{
				if (orderColumn.EndsWith(" ASC", System.StringComparison.OrdinalIgnoreCase))
				{
					model.OrderType = OrderType.Ascending;
					orderColumn = orderColumn.Remove(orderColumn.Length - 4);
				}
				if (orderColumn.EndsWith(" DESC", System.StringComparison.OrdinalIgnoreCase))
				{
					model.OrderType = OrderType.Descending;
					orderColumn = orderColumn.Remove(orderColumn.Length - 5);
				}
				string[] columns = orderColumn.Split(",");

				foreach (string column in columns)
				{
					string colonna = column.Trim();
					if (colonna.StartsWith("[") && colonna.EndsWith("]"))
					{
						colonna = colonna.Remove(colonna.Length - 1).Substring(1);
					}
					/*PropertyInfo property = typeof(Entity).GetProperty(colonna,
																		BindingFlags.SetProperty |
																		BindingFlags.IgnoreCase |
																		BindingFlags.Public |
																		BindingFlags.Instance);
					if (property == null)
					{
						errors.Add($"Impossibile ordinare per la colonna \"{column}\" - tale ordinamento verrà ignorato");
					}
					else
					{
						columnsToOrder.Add($"{property.Name}");
					}*/
					columnsToOrder.Add($"{colonna}");

				}
			}
			string orderInstruction = null;
			if (columnsToOrder.Count > 0)
			{
				string orderAscending = model.OrderType == OrderType.Ascending ? "ASC" : "DESC";
				orderInstruction = $"{string.Join(",", columnsToOrder)} {orderAscending}";
				int c = 0;
				IOrderedQueryable<Entity> orderQuery = query.OrderBy($"{columnsToOrder.First()} {orderAscending}");
				foreach (var column in columnsToOrder)
				{
					if (c++ == 0)
					{
						continue;
					}
					else
					{
						orderQuery = orderQuery.ThenBy($"{column} { orderAscending}");
					}
				}
				query = orderQuery;
			}
			/* Paginazione */
			int page = model.Page;
			int recordsPerPage = model.RecordsPerPage <= 0 ? count : model.RecordsPerPage;
			if (page < 1)
			{
				page = 1;
				errors.Add("La pagina non può essere inferiore a 1. È stata impostata con il valore di default 1");
			}
			if (recordsPerPage < 1)
			{
				recordsPerPage = 20;
				errors.Add("Il numero di elementi per pagina non può essere inferiore a 1. È stata impostata con il valore di default 20");
			}

			int pageCount = Math.Max((count + recordsPerPage - 1) / recordsPerPage, 1);
			if (page > pageCount)
			{
				page = pageCount;
				errors.Add("Inserito un numero di pagina maggiore delle pagine totali. È stato impostato l'ultimo numero di pagina");
			}
			if (recordsPerPage <= count)
			{
				query = query.Skip((page - 1) * recordsPerPage).Take(recordsPerPage);
			}

			ResultList<Entity> result = new ResultList<Entity>
			{
				Count = count,
				Page = page,
				PageCount = pageCount,
				RecordsPerPage = recordsPerPage,
				List = query.ToList(),
				Message = string.Join(Environment.NewLine, errors),
				Order = orderInstruction
			};
			return result;
		}
	}

}
