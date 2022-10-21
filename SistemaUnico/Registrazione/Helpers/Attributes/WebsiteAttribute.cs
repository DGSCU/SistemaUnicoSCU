using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;

namespace RegistrazioneSistemaUnico.Helpers.Attributes
{
	public class WebsiteAttribute : ValidationAttribute
	{
		public WebsiteAttribute(string errorMessage = "indirizzo non valido") : base(errorMessage)
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
			string sito = (string)value;
			if (string.IsNullOrEmpty(sito))
			{
				return true;
			}
			string domain = sito
				.Split("//")
				.Last().Split("/")
				.First();
			try
			{
				IPHostEntry hostInfo = Dns.GetHostByName(domain);
				return true;
			}
			catch (Exception)
			{

				return false;
			}			
		}
	}
}
