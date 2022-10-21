using DomandeOnline.Data;
using DomandeOnline.Models;
using Microsoft.AspNet.Identity.Owin;
using Serilog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Web;

namespace DomandeOnline.Code
{
	public class Utils
	{
		/// <summary>
		/// Restituisce una string a che indica la dimensione di un file dati i bytes
		/// </summary>
		/// <param name="bytes">numero di bytes</param>
		/// <returns>Stringa con la descrizione della dimensione</returns>
		public static string FileDimensionToString(int bytes) {
			List<string> dimensioni = new List<string>
			{
				"B",
				"KB",
				"MB",
				"GB",
				"TB"
			};
			decimal value = bytes;

			foreach (string dimensione in dimensioni)
			{
				if (value < 1024)
				{
					return Math.Round(value, 3) + dimensione;
				}
				value /= 1024;
			}

			return Math.Round(value, 3) + dimensioni.Last();
		}



		public static byte[] Encrypt(byte[] input)
		{
			string key = ConfigurationManager.AppSettings.Get("ChipherKey");
			PasswordDeriveBytes pdb =
			  new PasswordDeriveBytes(key, // Change this
			  new byte[] { 0x43, 0x87, 0x23, 0x72 }); // Change this
			MemoryStream ms = new MemoryStream();
			Aes aes = new AesManaged();
			aes.Key = pdb.GetBytes(aes.KeySize / 8);
			aes.IV = pdb.GetBytes(aes.BlockSize / 8);
			CryptoStream cs = new CryptoStream(ms,
			  aes.CreateEncryptor(), CryptoStreamMode.Write);
			cs.Write(input, 0, input.Length);
			cs.Close();
			return ms.ToArray();
		}
		public static byte[] Decrypt(byte[] input)
		{
			string key = ConfigurationManager.AppSettings.Get("ChipherKey");
			PasswordDeriveBytes pdb =
			  new PasswordDeriveBytes(key, // Change this
			  new byte[] { 0x43, 0x87, 0x23, 0x72 }); // Change this
			MemoryStream ms = new MemoryStream();
			Aes aes = new AesManaged();
			aes.Key = pdb.GetBytes(aes.KeySize / 8);
			aes.IV = pdb.GetBytes(aes.BlockSize / 8);
			CryptoStream cs = new CryptoStream(ms,
			  aes.CreateDecryptor(), CryptoStreamMode.Write);
			cs.Write(input, 0, input.Length);
			cs.Close();
			return ms.ToArray();
		}

		/// <summary>
		/// Converte un'email in un email parzialmente nascosta
		/// </summary>
		/// <remarks>
		/// la email visualizzata presenterà i primi quattro caratteri, seguiti da asterischi e il dominio
		/// </remarks>
		/// <example>
		/// l'email mario.rossi@dominio.it sarà convertita in del tipo mari***@dominio.it
		/// </example>
		/// <param name="email">l'email da convertire</param>
		/// <returns>l'email convertita</returns>
		public static string HideEmail(string email)
		{
			string dominio = email.Split('@').Last();
			string indirizzo = email.Substring(0,4).Split('@').First();
			return indirizzo + "****@" + dominio;
		}


		public static string EncryptString(string plaintext)
		{
			MemoryStream ms = new MemoryStream();
			using (TripleDESCryptoServiceProvider TripleDes = new TripleDESCryptoServiceProvider()
			{
				Key = new byte[] { 23, 118, 87, 73, 6, 102, 76, 255, 41, 84, 2, 24, 201, 105, 54, 19, 54, 17, 3, 88, 13, 224, 173, 135 },
				IV = new byte[] { 16, 245, 123, 16, 64, 22, 1, 200 }
			}
			)
			using (CryptoStream encStream = new CryptoStream(ms, TripleDes.CreateEncryptor(), CryptoStreamMode.Write))
			{
				{
					byte[] plaintextBytes = System.Text.Encoding.UTF8.GetBytes(plaintext);
					encStream.Write(plaintextBytes, 0, plaintextBytes.Length);
					encStream.FlushFinalBlock();
					return Convert.ToBase64String(ms.ToArray());
				}
			}
		}

		public static ApplicationUser GetUser(string username)
		{
			ApplicationUserManager userManager = HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
			return userManager.FindByNameAsync(username).GetAwaiter().GetResult();
		}

		public static void RimuoviSpazi(object oggetto)
		{
			foreach (PropertyInfo proprieta in oggetto.GetType().GetProperties())
			{
				if (proprieta.PropertyType == typeof(string))
				{
					string value = (proprieta.GetValue(oggetto) as string);
					if (value!=null)
					{
						proprieta.SetValue(oggetto, value.Trim());
					}
				}
			}
		}

		public static Indirizzo GetIndirizzo(string testo)
		{
			using (var context = new Entities())
			{
				try
				{
					string[] testoSplit = (testo??"").Split(' ');
					int indexCAP = -1;
					for (int i = 0; i < testoSplit.Length; i++)
					{
						if (testoSplit[i].Length == 5 && int.TryParse(testoSplit[i], out int n))
						{
							indexCAP = i;
						}
					}
					if (indexCAP < 0)
					{
						Log.Error("Indirizzo Non formattato correttamente");
						return null;
					}
					string cap = testoSplit[indexCAP];
					string provinciaSigla = testoSplit.Last();
					string provincia = context.Provincia
										.Where(x => x.Sigla == provinciaSigla)
										.Select(x => x.Nome)
										.SingleOrDefault() ?? provinciaSigla;
					string comune = string.Join(" ", testoSplit.Skip(indexCAP + 1).Take(testoSplit.Length - indexCAP - 2));
					string civico = testoSplit[indexCAP - 1];
					string via = string.Join(" ", testoSplit.Take(indexCAP - 1));
					Indirizzo indirizzo = new Indirizzo()
					{
						Via = via,
						Civico = civico,
						CAP = cap,
						Provincia = provincia,
						Comune = comune
					};
					return indirizzo;
				}
				catch (Exception e)
				{
					Log.Error(e, "Errore nella generazione dell'indirizzo");
					return null;
				}
			}
		}

		public static string GetMese(int i){
			switch (i)
			{
				case 1: return "Gennaio";
				case 2: return "Febbraio";
				case 3: return "Marzo";
				case 4: return "Aprile";
				case 5: return "Maggio";
				case 6: return "Giugno";
				case 7: return "Luglio";
				case 8: return "Agosto";
				case 9: return "Settembre";
				case 10: return "Ottobre";
				case 11: return "Novembre";
				case 12: return "Dicembre";
				default: return "Non Valido";
			}

		}


		public static List<string> checkCaratteriInvalidi(object item){
			List<string> campiInvalidi = new List<string>();
			foreach (PropertyInfo property in item.GetType().GetProperties()){
				if (property.PropertyType == typeof(string))
				{
					string testo=(string)property.GetValue(item);
					if (string.IsNullOrEmpty(testo))
					{
						continue;
					}
					if (Regex.Match(testo, @"[\p{C}-[\r\n\t]]").Success)
					{
						campiInvalidi.Add(property.Name);
					}
				}
			}
			return campiInvalidi;

		}
	}
}