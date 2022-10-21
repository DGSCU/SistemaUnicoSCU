using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DomandeOnline.Models
{
	public class Domanda
	{
		public int IdDomanda { get; set; }
		public string Bando { get; set; }
		public DateTime DataPresentazione { get; set; }
	}
}
