using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;

namespace ScheduleAgent.Class
{
	public class Format
	{
		public class Propieta
		{
			public string Nome { get; set; }
			public string Valore { get; set; }
		}
		public static List<Propieta> GetProperties(string properties)
		{
			XmlDocument document = new XmlDocument();
			document.LoadXml(properties);
			XmlNode root = document.FirstChild;
			List<Propieta> listaPropieta=new List<Propieta>();
			foreach (XmlNode node in root.ChildNodes)
			{
				string nome = node.Attributes["key"].Value;
				listaPropieta.Add(new Propieta()
				{
					Nome = nome,
					Valore= node.InnerText
				});
			}
			return listaPropieta;
		}

		public static string FormatFileName(string fileName)
		{
			string result = fileName;
			string[] caratteriNonValidi = { "\\", "/", ":", "*", "?", "\"", "<", ">", "|" };
			foreach (string carattere in caratteriNonValidi)
			{
				result = result.Replace(carattere, "");
			}
			return result;
		}
	}
}