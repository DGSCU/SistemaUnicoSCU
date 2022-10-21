using System.ComponentModel.DataAnnotations;

namespace RegistrazioneSistemaUnico.Helpers.Attributes
{
	public class CodiceFiscaleAttribute : ValidationAttribute
	{
		public CodiceFiscaleAttribute(string errorMessage = "CodiceFiscale non valido") : base(errorMessage)
		{

		}

		public override bool IsValid(object value)
		{
			if (value == null)
			{
				return true;
			}
			if (!(value is string))
				return false;
			string codiceFiscale = (string)value;
			if (string.IsNullOrEmpty(codiceFiscale))
			{
				return true;
			}
			return CodiceFiscale.ControlloFormaleOK(codiceFiscale);
		}
	}
}
