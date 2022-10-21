using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace RegistrazioneSistemaUnico.Helpers
{

	public class Encryptor
	{

		#region Static Functions

		/// <summary>
		/// Cifratura di un testo tramite SHA1 256
		/// </summary>
		/// <param name="data">testo da cifrare</param>
		/// <returns>testo cifrato in Base64</returns>
		public static string  Encrypt(string text){
			if (string.IsNullOrEmpty(text))
			{
				return text;
			} 
			return Convert.ToBase64String(Encrypt(Encoding.UTF8.GetBytes(text)));
		}

		/// <summary>
		/// Cifratura di dati binari tramite SHA1 256
		/// </summary>
		/// <param name="data">Dati da cifrare</param>
		/// <returns>dati cifrati</returns>
		public static byte[] Encrypt(byte[] data)
		{
			try
			{
				string Password = "PRova";
				string Salt = "Kosher"; string HashAlgorithm = "SHA1";
				int PasswordIterations = 2;
				string InitialVector = "OFRna73m*aze01xY";
				int KeySize = 256;
				if (data.Length==0)
					return data;
				byte[] InitialVectorBytes = Encoding.ASCII.GetBytes(InitialVector);
				byte[] SaltValueBytes = Encoding.ASCII.GetBytes(Salt);
				byte[] PlainTextBytes = data;
				PasswordDeriveBytes DerivedPassword = new PasswordDeriveBytes(Password, SaltValueBytes, HashAlgorithm, PasswordIterations);
				//byte[] KeyBytes = Encoding.ASCII.GetBytes(Password);
				byte[] KeyBytes = DerivedPassword.GetBytes(KeySize / 8);
				using (AesManaged SymmetricKey = new AesManaged())
				{
					SymmetricKey.Mode = CipherMode.CBC;
					byte[] CipherTextBytes = null;
					using (ICryptoTransform Encryptor = SymmetricKey.CreateEncryptor(KeyBytes, InitialVectorBytes))
					{
						using (MemoryStream MemStream = new MemoryStream())
						{
							using (CryptoStream CryptoStream = new CryptoStream(MemStream, Encryptor, CryptoStreamMode.Write))
							{
								CryptoStream.Write(PlainTextBytes, 0, PlainTextBytes.Length);
								CryptoStream.FlushFinalBlock();
								CipherTextBytes = MemStream.ToArray();
								MemStream.Close();
								CryptoStream.Close();
							}
						}
					}
					SymmetricKey.Clear();
					return CipherTextBytes;
				}
			}
			catch (Exception e)
			{

				throw e;
			}
		}

		/// <summary>
		/// Decifra un testo tramite SHA1 256
		/// </summary>
		/// <param name="data">testo in formato base64</param>
		/// <returns>Testo decifrato</returns>
		public static string Decrypt(string text){
			if (string.IsNullOrEmpty(text))
			{
				return text;
			}
			try
			{
				byte[] data = Convert.FromBase64String(text);
				return Encoding.UTF8.GetString(Decrypt(data));
			}
			catch (Exception e)
			{

				throw new Exception("Testo non valido. Il testo deve essere di in formatoBase64");
			}
		}

		/// <summary>
		/// Decifra dei dati binari tramite SHA1 256
		/// </summary>
		/// <param name="data">dati binari da decifrare</param>
		/// <returns>dati binari decifrati</returns>
		public static byte[] Decrypt(byte[] data)
		{
			try
			{
				string Password = "PRova";
				string Salt = "Kosher"; string HashAlgorithm = "SHA1";
				int PasswordIterations = 2;
				string InitialVector = "OFRna73m*aze01xY";
				int KeySize = 256;
				if (data==null || data.Length==0)
					return data;
				byte[] InitialVectorBytes = Encoding.ASCII.GetBytes(InitialVector);
				byte[] SaltValueBytes = Encoding.ASCII.GetBytes(Salt);
				byte[] CipherTextBytes = data;// Convert.FromBase64String(CipherText);
				PasswordDeriveBytes DerivedPassword = new PasswordDeriveBytes(Password, SaltValueBytes, HashAlgorithm, PasswordIterations);
				//byte[] KeyBytes = Encoding.ASCII.GetBytes(Password);
				byte[] KeyBytes = DerivedPassword.GetBytes(KeySize / 8);
				using (AesManaged SymmetricKey = new AesManaged())
				{
					SymmetricKey.Mode = CipherMode.CBC;
					byte[] PlainTextBytes = new byte[CipherTextBytes.Length];
					int ByteCount = 0;
					using (ICryptoTransform Decryptor = SymmetricKey.CreateDecryptor(KeyBytes, InitialVectorBytes))
					{
						using (MemoryStream MemStream = new MemoryStream(CipherTextBytes))
						{
							using (CryptoStream CryptoStream = new CryptoStream(MemStream, Decryptor, CryptoStreamMode.Read))
							{

								ByteCount = CryptoStream.Read(PlainTextBytes, 0, PlainTextBytes.Length);
								MemStream.Close();
								CryptoStream.Close();
							}
						}
					}
					SymmetricKey.Clear();
					return PlainTextBytes;
				}
			}
			catch (Exception e)
			{

				throw e;
			}
		}

		#endregion
	}
}
