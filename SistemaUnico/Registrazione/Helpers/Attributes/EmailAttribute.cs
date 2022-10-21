using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace RegistrazioneSistemaUnico.Helpers.Attributes
{
	public class EmailAttribute : ValidationAttribute
	{
		public EmailAttribute(string errorMessage = "E-mail non valida") : base(errorMessage)
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
			string email = (string)value;
			if (string.IsNullOrEmpty(email))
			{
				return true;
			}
			return Regex.Match(text, @"^([a-zA-Z0-9][a-zA-Z0-9_\-\.]*)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$").Success;
		}
	}
}
