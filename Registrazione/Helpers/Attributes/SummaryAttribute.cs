using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RegistrazioneSistemaUnico.Helpers.Attributes
{
	public class SummaryAttribute : Attribute
	{
		public SummaryAttribute(string text)
		{
			this.text = text;
		}
		private readonly string text;
		public string Text { get { return text; } }
	}


}
