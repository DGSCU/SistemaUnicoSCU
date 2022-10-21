using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RegistrazioneSistemaUnico.Helpers.Attributes
{
	public class MoneyAttribute : Attribute
	{
		public MoneyAttribute(string format = "0.00", string currency = "€")
		{
			Format = format;
			Currency = currency;
		}
		public string Format { get; set; }
		public string Currency { get; set; }

	}
}
