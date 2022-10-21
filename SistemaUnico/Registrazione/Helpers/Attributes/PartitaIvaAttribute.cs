	using System.ComponentModel.DataAnnotations;

namespace RegistrazioneSistemaUnico.Helpers.Attributes
{
	public class PartitaIvaAttribute : ValidationAttribute
	{
		public PartitaIvaAttribute(string errorMessage = "Partita IVA non valida") : base(errorMessage)
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
			string partitaIva = (string)value;
			if (string.IsNullOrEmpty(partitaIva))
			{
				return true;
			}
			return PartitaIva.ControllaPartitaIva(partitaIva);
		}
	}
}