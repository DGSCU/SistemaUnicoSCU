using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RegistrazioneSistemaUnico.Helpers.Attributes
{
	public class DisplayBooleanAttribute:Attribute
	{
		public DisplayBooleanAttribute(string trueText = "Sì", string falseText = "No")
		{
			TrueText = trueText;
			FalseText = falseText;
		}

		public string TrueText { get; set; }
		public string FalseText { get; set; }
	}
}
