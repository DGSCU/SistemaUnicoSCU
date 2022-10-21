using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace RegistrazioneSistemaUnico.Models
{
	public class Documento
	{
		public Documento(){
			Registrazioni = new HashSet<Registrazione>();
			RegistrazioniNomina = new HashSet<Registrazione>();
		}
		public int Id { get; set; }
		public string NomeFile { get; set; }
		public int Dimensione { get; set; }
		public byte[] Blob { get; set; }
		public string MimeType { get; set; }
		public string Hash { get; set; }
		public ICollection<Registrazione> Registrazioni { get; set; }
		public ICollection<Registrazione> RegistrazioniNomina { get; set; }

		[JsonIgnore]
		[NotMapped]
		public IFormFile File { get; set; }

		/// <summary>
		/// Indica se è stato richiesto di cancellare il file
		/// </summary>

		[NotMapped]
		public bool? Delete { get; set; }
		
		/// <summary>
		/// Indica se è stato richiesto di scaricare il file
		/// </summary>
		[NotMapped]
		public bool? Download { get; set; }


		public bool Load(){
			if (File == null)
				return false;
			using (var ms = new MemoryStream())
			{
				File.CopyTo(ms);
				Blob = ms.ToArray();
			}
			Dimensione = (int)File.Length;
			NomeFile = File.FileName;
			MimeType = File.ContentType;
			using (var sha1 = new System.Security.Cryptography.SHA1CryptoServiceProvider())
			{
				Hash= string.Concat(sha1.ComputeHash(Blob).Select(x => x.ToString("X2")));
			}
			return true;
		}
	}
}
