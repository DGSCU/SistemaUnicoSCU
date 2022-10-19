using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace RegistrazioneSistemaUnico.Helpers.Attributes
{
	public class TelefonoAttribute : ValidationAttribute
	{
		public TelefonoAttribute(string errorMessage = "Numero non valido") : base(errorMessage)
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
			string numero = (string)value;
			if (string.IsNullOrEmpty(numero))
			{
				return true;
			}
			return Regex.Match(text, @"^\+?([0-9]|([0-9] )|([0-9]-))*[0-9]$").Success;
		}
	}
}
