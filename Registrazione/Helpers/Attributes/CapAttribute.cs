using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace RegistrazioneSistemaUnico.Helpers.Attributes
{
	public class CapAttribute : ValidationAttribute
	{
		public CapAttribute(string errorMessage = "CAP non valido") : base(errorMessage)
		{

		}

		public override bool IsValid(object value)
		{
			if (value == null)
			{
				return true;
			}
			if (!(value is string text))
				return false;
			string codiceFiscale = (string)value;
			if (string.IsNullOrEmpty(codiceFiscale))
			{
				return true;
			}
			return Regex.Match(text, "^[0-9]{5}$").Success;
		}
	}
}
