using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RegistrazioneSistemaUnico.Helpers.Attributes
{
	public class DateAttribute : ValidationAttribute
	{
		public DateAttribute(	string errorMessage = "Data non valida",
								string format = "dd/MM/yyyy", 
								string minDate = null, 
								string maxDate = null,
								bool consentiDataFutura = true
								) : base(errorMessage)
		{
			Format = format;
			MinDate = MinDate == null ? (DateTime?)null : DateTime.Parse(minDate);
			MaxDate = MaxDate == null ? (DateTime?)null : DateTime.Parse(maxDate);
			ConsentiDataFutura = consentiDataFutura;
		}
		public string Format { get; set; }
		public DateTime? MinDate { get; set; }
		public DateTime? MaxDate { get; set; }
		public bool ConsentiDataFutura { get; set; }
		public override bool IsValid(object value)
		{
			if (value == null)
			{
				return true;
			}
			DateTime date;
			if (value is DateTime)
			{
				date = (DateTime)value;
			}
			else if (value is string)
			{
				string data = value.ToString();
				if (string.IsNullOrEmpty(data))
				{
					return true;
				}
				string pattern = "[0-9]{1,2}/[0-9]{1,2}/[0-9]{1,4}";
				bool success = Regex.Match(data, pattern).Success;
				if (!success)
				{
					ErrorMessage = "Formato della data non valido";
					return false;
				}
				bool parsed = DateTime.TryParseExact((string)value, Format, CultureInfo.InvariantCulture, DateTimeStyles.None, out date);
				if (!parsed)
				{
					ErrorMessage = "Data non valida";
					return false;
				}
			}else{
				ErrorMessage = "Formatro data non valido";
				return false;
			}
			if (!ConsentiDataFutura && date>DateTime.Today){
				ErrorMessage = $"la data non può essere futura";
				return false;
			}
			if (MinDate.HasValue && MinDate >= date)
			{
				ErrorMessage = $"la data deve essere successiva al {MinDate.Value.ToString("dd/MM/yyyy")}";
				return false;
			}
			if (MaxDate.HasValue && MaxDate <= date)
			{
				ErrorMessage = $"la data deve essere antecedente al {MaxDate.Value.ToString("dd/MM/yyyy")}";
				return false;
			}
			return true;
		}

	}
}
