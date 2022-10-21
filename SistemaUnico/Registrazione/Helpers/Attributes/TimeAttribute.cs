using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RegistrazioneSistemaUnico.Helpers.Attributes
{
	/// <summary>
	/// Indica che il campo è di tipo orario
	/// </summary>
	public class TimeAttribute : ValidationAttribute
	{
		public TimeAttribute(string errorMessage = "Orario non valido") : base(errorMessage)
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
			if (string.IsNullOrEmpty(text))
			{
				return true;
			}
			string[] fields = text.Split(':');
			if (fields.Length!=2)
			{
				return false;
			}
			if (!int.TryParse(fields[0], out int hours))
			{
				return false;
			}
			if (!int.TryParse(fields[1], out int minutes))
			{
				return false;
			}
			if (hours>23)
			{
				return false;
			}
			if (minutes > 60)
			{
				return false;
			}
			return true;
		}
	}
}
