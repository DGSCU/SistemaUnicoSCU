using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using iTextSharp.text.pdf.security;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto.Prng;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.X509;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.Pkcs;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace RegistrazioneSistemaUnico.Helpers
{
	/// <summary>
	/// Tipologia di firma
	/// </summary>
	public enum TipoFirma
	{
		/// <summary>
		/// Firma con envelope (file P7m)
		/// </summary>
		CAdES = 1,
		/// <summary>
		/// Firma su PDF (file Pdf)
		/// </summary>
		PAdES = 2
	}

	/// <summary>
	/// Firma associata ad un documento
	/// </summary>
	/// <example>	byte[] file = System.IO.File.ReadAllBytes("C:\\documento.p7m");
	///				DocumentoFirmato documentoFirmato = DocumentoFirmato.Carica(file);
	///</example>
	public class Firma {

		/// <summary>
		/// Codice fiscale del soggetto che ha apposto la firma (può essere vuoto)
		/// </summary>
		public string CodiceFiscale { get; set; }

		/// <summary>
		/// Nominativo del soggetto che ha apposto la firma (può essere vuoto)
		/// </summary>
		public string Nominativo { get; set; }

		/// <summary>
		/// Data in cui è stata apposta la firma
		/// </summary>
		public DateTime? DataFirma { get; set; }

		/// <summary>
		/// Data di inizio validità del certificato
		/// </summary>
		public DateTime DataValiditaDa { get; set; }

		/// <summary>
		/// Data di fine validità del certificato
		/// </summary>
		public DateTime DataValiditaA { get; set; }

		/// <summary>
		/// Nominativo dell'Ente certificatore
		/// </summary>
		public string Certificatore { get; set; }

		/// <summary>
		/// Certificato associato alla firma
		/// </summary>
		public X509Certificate2 Certificato { get; set; }


	}

	/// <summary>
	/// Oggetto contente il documento (p7m o pdf) e le informazioni relative alle firma digitali apposte ad esso.
	/// </summary>
	public class DocumentoFirmato
	{

		DocumentoFirmato() {
			firme = new List<Firma>();
		}
		private const string PREFISSO_CODICE_FISCALE = "SERIALNUMBER=";
		private const string PREFISSO_NOME = "CN=";


		private readonly List<Firma> firme;
		private byte[] fileContent;


		/// <summary>
		/// File originale
		/// </summary>
		public byte[] FileContent { get { return fileContent; } }


		/// <summary>
		/// Lista di firme apposte al documento
		/// </summary>
		public List<Firma> Firme { get { return firme; } }

		/// <summary>
		/// Tipologia di firma
		/// </summary>
		public TipoFirma TipoFirma { get; set; }

		/// <summary>
		/// Verifica la validità di tutte le firme
		/// </summary>
		public DateTime? ValiditaDa
		{
			get
			{
				return firme.Max(f => f.DataValiditaDa);
			}
		}

		/// <summary>
		/// Verifica la validità di tutte le firme
		/// </summary>
		public DateTime? ValiditaA
		{
			get
			{
				return firme.Min(f => f.DataValiditaA);
			}
		}

		/// <summary>
		/// Elenco di codici fiscali dei soggetti che hanno firmato il documento
		/// </summary>
		public List<string> CodiciFiscali
		{
			get
			{
				return firme
					.Where(f => !string.IsNullOrEmpty(f.CodiceFiscale))
					.Select(x => x.CodiceFiscale)
					.ToList();
			}
		}

		public bool HasCertificato { get { return NumeroFirme > 0; } }

		/// <summary>
		/// Elenco dei certificati associati alle firme del documento
		/// </summary>
		public IEnumerable<X509Certificate2> Certificati { get { return firme.Select(f => f.Certificato); } }

		/// <summary>
		/// Numero delle firme apposte al documento;
		/// </summary>
		public int NumeroFirme { get { return firme.Count; } }

		public bool VerificaFirma( 
			string codiceFiscale=null,
			DateTime? allaData=null){
			if (!allaData.HasValue)
			{
				allaData = DateTime.Now;
			}
			IEnumerable<Firma> firme = Firme
				.Where(f=>	allaData>=f.DataValiditaDa&&
							allaData<=f.DataValiditaA);
			if (!string.IsNullOrEmpty(codiceFiscale))
			{
				firme = firme.Where(x => x.CodiceFiscale?.ToUpper() == codiceFiscale?.ToUpper());
			}
			return firme.Any();
		}

		public bool VerificaDocumento(byte[] documento)
		{
			if (documento.SequenceEqual(fileContent))
				return true;
			//Se i file non corrispondono verifico se il testo dei pdf corrisponde
			try
			{
				string testoDocumentoFirmato = PdfToText(documento);
				string testoDocumento = PdfToText(fileContent);

				return testoDocumentoFirmato == testoDocumento;
			}
			catch (Exception)
			{
			}
			return false;


		}
		/// <summary>
		/// Prova a decodificare un byte array ipotizzando che sia in base64. Se non lo e' ritorna 
		/// l'array originale
		/// </summary>
		/// <param name="f">il file in ingresso</param>
		/// <returns>il file decodificato, se era in base64; altrimenti il file originario</returns>
		public static byte[] DecodeBase64(byte[] f )
        {
			byte[] fOut = f;
			try
            {
				String s = System.Text.Encoding.Default.GetString(f);
				if (s.Contains("BEGIN PKCS7"))
				{
					s = System.Text.RegularExpressions.Regex.Replace(s, @"-+BEGIN PKCS7-+[\r\n]", "");
					s = System.Text.RegularExpressions.Regex.Replace(s, @"-+END PKCS7-+[\r\n]?", "");
					s = System.Text.RegularExpressions.Regex.Replace(s, @"[^a-zA-Z0-9\+=\/]", "");
					//					s = s.Replace("\n", "").Replace("\r", "");
				}
				fOut = Convert.FromBase64String(s);
			} catch(Exception)
            {

            }
			return fOut;
		}

		/// <summary>
		/// Funzione per la generazione di generazione di un'istanza di documento firmato dato il file
		/// </summary>
		/// <param name="file"></param>
		/// <returns>Oggetto contenente il file originale e le firme associate.</returns>
		public static DocumentoFirmato Carica(byte[] file)
        {
			if (file==null||file.Length==0)
			{
				throw new ArgumentException("File vuoto o inesistente");
			}
			// se il file e' Base64 encoded, lo traduce
			file = DecodeBase64(file);

			//Verifica Firma CAdES (file P7M)
			DocumentoFirmato documento = new DocumentoFirmato();
			try
			{
				SignedCms cms = new SignedCms();
				
				cms.Decode(file);
				var signers = cms.SignerInfos;
				var certificates = cms.Certificates;

				foreach (var signer in signers)
				{

					X509Certificate2 certificate = signer.Certificate;
					Firma firma = new Firma()
					{
						Certificato = certificate,
						DataFirma = GetDataFirma(signer),
						Certificatore = GetNominativo(certificate.Issuer),
						DataValiditaDa = Convert.ToDateTime(certificate.GetEffectiveDateString()),
						DataValiditaA = Convert.ToDateTime(certificate.GetExpirationDateString()),
						CodiceFiscale = GetCodiceFiscale(certificate.Subject),
						Nominativo = GetNominativo(certificate.Subject)
					};

					documento.TipoFirma = TipoFirma.CAdES;
					documento.firme.Add(firma);
				}
				documento.fileContent = cms.ContentInfo.Content;
			}
			catch{
				// Verifica firme PAdES (su file Pdf)
				try
				{
					PdfReader pdf = new PdfReader(file);
					AcroFields fields = pdf.AcroFields;
					List<string> names = fields.GetSignatureNames();
					foreach (string name in names)
					{
						PdfPKCS7 pk = fields.VerifySignature(name);
						var cal = pk.SignDate;
						Org.BouncyCastle.X509.X509Certificate[] certificates = pk.Certificates;
						if (certificates.Length > 0)
						{
							Org.BouncyCastle.X509.X509Certificate certificatePdf = certificates[0];
							X509Certificate2 certificate = new X509Certificate2(certificatePdf.GetEncoded());
							Firma firma = new Firma()
							{
								Certificato = certificate,
								DataFirma = pk.SignDate,
								Certificatore = GetNominativo(certificate.Issuer),
								DataValiditaDa = Convert.ToDateTime(certificate.GetEffectiveDateString()),
								DataValiditaA = Convert.ToDateTime(certificate.GetExpirationDateString()),
								CodiceFiscale = GetCodiceFiscale(certificate.Subject),
								Nominativo = GetNominativo(certificate.Subject),
						};
							documento.TipoFirma = TipoFirma.PAdES;
							documento.firme.Add(firma);
							documento.fileContent = file;
						}

						//TODO Recupero PDF originale
					}
					
				}
				catch
				{
					// Se non ci sono firme il documento originale è il file stesso.
					documento.fileContent = file;
				}

			}
			return documento;
		}

		/// <summary>
		/// Recupero del codice fiscale dalla stringa dei parametri 
		/// </summary>
		/// <param name="parametri">parmetri separati da virgole assoicati ad un soggetto</param>
		/// <returns>Il codice fiscale estratto dai paramtri. null se non presente</returns>
		private static string GetCodiceFiscale(string parametri){
			string codiceFiscale= parametri
				.Split(',')
				.Select(s => s.Trim())
				.FirstOrDefault(x => x.StartsWith(PREFISSO_CODICE_FISCALE))?
				.Trim();
			return codiceFiscale?.Substring(codiceFiscale.Length - 16);
		}

		/// <summary>
		/// Recupero del nominativo dalla stringa dei parametri 
		/// </summary>
		/// <param name="parametri">parmetri separati da virgole assoicati ad un soggetto</param>
		/// <returns>Il nominativo estratto dai paramtri. null se non presente</returns>
		private static string GetNominativo(string firmatario)
		{
			return firmatario
				.Split(',')
				.Select(s=>s.Trim())
				.FirstOrDefault(x => x.StartsWith(PREFISSO_NOME))?
				.Substring(PREFISSO_NOME.Length);
		}

		/// <summary>
		/// Recupero della data da una firma firma
		/// </summary>
		/// <param name="signer">firma associata ad un documento</param>
		/// <returns>data della firma. Null se non presente</returns>
		private static DateTime? GetDataFirma(SignerInfo signer){
			foreach (var attribute in signer.SignedAttributes)
			{
				foreach (var attributeValue in attribute.Values)
				{
					if (attributeValue is Pkcs9SigningTime signingTime)
					{
						return signingTime.SigningTime;
					}
				}
			}
			return null;
		}

		/// <summary>
		/// Trasforma un file PDF in solo testo
		/// </summary>
		/// <param name="bytes">File PDF</param>
		/// <returns>Testo estratto dal file PDF</returns>
		private static string PdfToText(byte[] bytes)
		{
			var sb = new StringBuilder();

			try
			{
				var reader = new PdfReader(bytes);
				var numberOfPages = reader.NumberOfPages;

				for (var currentPageIndex = 1; currentPageIndex <= numberOfPages; currentPageIndex++)
				{
					sb.Append(PdfTextExtractor.GetTextFromPage(reader, currentPageIndex));
				}
			}
			catch (Exception exception)
			{
				Console.WriteLine(exception.Message);
			}

			return sb.ToString();
		}

		public static byte[] FirmaDocumento(byte[] file) {
			string passCert = "";
			var pass = passCert.ToCharArray();

			Pkcs12Store store;

			byte[] rawdata = GenerateCertificate().RawData;
			MemoryStream memStream = new MemoryStream(rawdata);

			store = new Pkcs12Store(memStream, pass);


			var alias = "";

			// searching for private key
			foreach (string al in store.Aliases)
				if (store.IsKeyEntry(al) && store.GetKey(al).Key.IsPrivate)
				{
					alias = al;
					break;
				}

			var pk = store.GetKey(alias);

			List<Org.BouncyCastle.X509.X509Certificate> chain = store.GetCertificateChain(alias).Select(c => c.Certificate).ToList();

			var parameters = pk.Key as RsaPrivateCrtKeyParameters;



			var reader = new PdfReader(file);


			MemoryStream ms=new MemoryStream();
			var stamper = PdfStamper.CreateSignature(reader, ms, '\0', null, true);

			var appearance = stamper.SignatureAppearance;
			appearance.Reason = "Prova";

			appearance.SetVisibleSignature("ServizioCivile");

			IExternalSignature pks = new PrivateKeySignature(parameters, DigestAlgorithms.SHA256);
			MakeSignature.SignDetached(appearance, pks, chain, null, null, null, 0, CryptoStandard.CMS);

			reader.Close();
			stamper.Close();
			return ms.ToArray();
		}

		static X509Certificate2 GenerateCertificate()
		{
			var keypairgen = new RsaKeyPairGenerator();
			keypairgen.Init(new KeyGenerationParameters(new SecureRandom(new CryptoApiRandomGenerator()), 1024));

			var keypair = keypairgen.GenerateKeyPair();

			var gen = new X509V3CertificateGenerator();

			var CN = new X509Name("CN=ServizioCivile");
			var SN = BigInteger.ProbablePrime(120, new Random());

			gen.SetSerialNumber(SN);
			gen.SetSubjectDN(CN);
			gen.SetIssuerDN(CN);
			gen.SetNotAfter(DateTime.MaxValue);
			gen.SetNotBefore(DateTime.Now.Subtract(new TimeSpan(7, 0, 0, 0)));
			gen.SetSignatureAlgorithm("MD5WithRSA");
			gen.SetPublicKey(keypair.Public);

			var newCert = gen.Generate(keypair.Private);

			return new X509Certificate2(DotNetUtilities.ToX509Certificate((Org.BouncyCastle.X509.X509Certificate)newCert));
		}
	}
}
