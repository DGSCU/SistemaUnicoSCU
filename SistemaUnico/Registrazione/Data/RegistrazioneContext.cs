using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using RegistrazioneSistemaUnico.Helpers;
using RegistrazioneSistemaUnico.Models;
namespace RegistrazioneSistemaUnico.Data
{
	public partial class RegistrazioneContext : DbContext
	{
		public RegistrazioneContext()
		{
		}

		public RegistrazioneContext(DbContextOptions<RegistrazioneContext> options)
			: base(options)
		{
		}


		public virtual DbSet<Registrazione> Registrazione { get; set; }  
		public virtual DbSet<CategoriaEnte> CategoriaEnte { get; set; }  
		public virtual DbSet<TipologiaEnte> TipologiaEnte { get; set; }  
		public virtual DbSet<Comune> Comune { get; set; }  
		public virtual DbSet<Provincia> Provincia { get; set; }  
		public virtual DbSet<UtenteEnte> UtenteEnte { get; set; }  
		public virtual DbSet<Documento> Documento { get; set; }  
		public virtual DbSet<Statistiche> Statistiche { get; set; }
		public virtual DbSet<StatisticheAndamento> StatisticheAndamento { get; set; }
		public virtual DbSet<Soggetto> Soggetto { get; set; }
		public virtual DbSet<TokenAccesso> TokenAccesso { get; set; }
		public virtual DbSet<DatiDomanda> DatiDomanda { get; set; }
		public virtual DbSet<DatiDomandaOLP> DatiDomandaOLP { get; set; }
		public virtual DbSet<DatiDomandaTUTORAGGIO> DatiDomandaTUTORAGGIO { get; set; }
		public virtual DbSet<DatiAntimafia> DatiAntimafia { get; set; }
		public virtual DbSet<ProtocolloPresentazione> ProtocolloPresentazione { get; set; }
		public virtual DbSet<ProtocolloAntimafia> ProtocolloAntimafia { get; set; }
		public virtual DbSet<ProtocolloOLP> ProtocolloOLP { get; set; }
		public virtual DbSet<ProtocolloTUTORAGGIO> ProtocolloTUTORAGGIO { get; set; }
		public virtual DbSet<ProtocolloProgramma> ProtocolloProgramma { get; set; }
		public virtual DbSet<DatiDomandaProgramma> DatiDomandaProgramma { get; set; }
		public virtual DbSet<SPResult> Result { get; set; }
		public virtual DbSet<Configurazione> Configurazione { get; set; }



		public SPResult RegistraEnte(
			int? IdRegistrazione,
			DateTime? DataInserimento,
			string CodiceFiscaleEnte,
			string CodiceFiscaleLegaleRappresentante,
			string Denominazione,
			string NomeLegaleRappresentante,
			string CognomeLegaleRappresentante,
			DateTime? DataNascitaLegaleRappresentante,
			string ComuneNascitaLegaleRappresentante,
			DateTime? DataNominaRappresentanteLegale,
			bool? EnteTitolare,
			int? IdCategoriaEnte,
			int? IdTipologiaEnte,
			int? IdProvinciaEnte,
			int? IdComuneEnte,
			string Via,
			string Civico,
			string CAP,
			string Telefono,
			string Email,
			string PEC,
			string Sito,
			bool? DichiarazionePrivacy,
			bool? DichiarazioneRappresentanteLegale,
			int? IdDocumento,
			bool? VariazioneRappresentanteLegale,
			DateTime? DataProtocollazione,
			string NumeroProtocollo,
			DateTime? DataProtocollo,
			DateTime? DataInvioEmail,
			int? IdDocumentoNomina,
			string albo
		)
		{
			List<SqlParameter> parametri = new List<SqlParameter>();
			parametri.Add(new SqlParameter("IdRegistrazione", SqlDbType.Int) { Value = (object)IdRegistrazione ?? DBNull.Value });
			parametri.Add(new SqlParameter("DataInserimento", SqlDbType.DateTime) { Value = (object)DataInserimento ?? DBNull.Value });
			parametri.Add(new SqlParameter("CodiceFiscaleEnte", SqlDbType.VarChar, 50) { Value = (object)CodiceFiscaleEnte ?? DBNull.Value });
			parametri.Add(new SqlParameter("CodiceFiscaleLegaleRappresentante", SqlDbType.VarChar, 50) { Value = (object)CodiceFiscaleLegaleRappresentante ?? DBNull.Value });
			parametri.Add(new SqlParameter("Denominazione", SqlDbType.VarChar, 200) { Value = (object)Denominazione ?? DBNull.Value });
			parametri.Add(new SqlParameter("NomeLegaleRappresentante", SqlDbType.VarChar, 255) { Value = (object)NomeLegaleRappresentante ?? DBNull.Value });
			parametri.Add(new SqlParameter("CognomeLegaleRappresentante", SqlDbType.VarChar, 255) { Value = (object)CognomeLegaleRappresentante ?? DBNull.Value });
			parametri.Add(new SqlParameter("DataNascitaLegaleRappresentante", SqlDbType.DateTime) { Value = (object)DataNascitaLegaleRappresentante ?? DBNull.Value });
			parametri.Add(new SqlParameter("ComuneNascitaLegaleRappresentante", SqlDbType.VarChar, 50) { Value = (object)ComuneNascitaLegaleRappresentante ?? DBNull.Value });
			parametri.Add(new SqlParameter("DataNominaRappresentanteLegale", SqlDbType.DateTime) { Value = (object)DataNominaRappresentanteLegale ?? DBNull.Value });
			parametri.Add(new SqlParameter("EnteTitolare", SqlDbType.Bit) { Value = (object)EnteTitolare ?? DBNull.Value});
			parametri.Add(new SqlParameter("IdCategoriaEnte", SqlDbType.Int) { Value = (object)IdCategoriaEnte ?? DBNull.Value });
			parametri.Add(new SqlParameter("IdTipologiaEnte", SqlDbType.Int) { Value = (object)IdTipologiaEnte ?? DBNull.Value });
			parametri.Add(new SqlParameter("IdProvinciaEnte", SqlDbType.Int) { Value = (object)IdProvinciaEnte ?? DBNull.Value });
			parametri.Add(new SqlParameter("IdComuneEnte", SqlDbType.Int) { Value = (object)IdComuneEnte ?? DBNull.Value });
			parametri.Add(new SqlParameter("Via", SqlDbType.VarChar, 255) { Value = (object)Via ?? DBNull.Value });
			parametri.Add(new SqlParameter("Civico", SqlDbType.VarChar, 50) { Value = (object)Civico ?? DBNull.Value });
			parametri.Add(new SqlParameter("CAP", SqlDbType.VarChar, 50) { Value = (object)CAP ?? DBNull.Value });
			parametri.Add(new SqlParameter("Telefono", SqlDbType.VarChar, 60) { Value = (object)Telefono ?? DBNull.Value });
			parametri.Add(new SqlParameter("Email", SqlDbType.VarChar, 100) { Value = (object)Email ?? DBNull.Value });
			parametri.Add(new SqlParameter("PEC", SqlDbType.VarChar, 100) { Value = (object)PEC ?? DBNull.Value });
			parametri.Add(new SqlParameter("Sito", SqlDbType.VarChar, 100) { Value = (object)Sito ?? DBNull.Value });
			parametri.Add(new SqlParameter("DichiarazionePrivacy", SqlDbType.Bit) { Value = (object)DichiarazionePrivacy ?? DBNull.Value });
			parametri.Add(new SqlParameter("DichiarazioneRappresentanteLegale", SqlDbType.Bit) { Value = (object)DichiarazioneRappresentanteLegale ?? DBNull.Value });
			parametri.Add(new SqlParameter("IdDocumento", SqlDbType.Int) { Value = (object)IdDocumento ?? DBNull.Value });
			parametri.Add(new SqlParameter("VariazioneRappresentanteLegale", SqlDbType.Bit) { Value = (object)VariazioneRappresentanteLegale ?? DBNull.Value });
			parametri.Add(new SqlParameter("DataProtocollazione", SqlDbType.VarChar, 50) { Value = (object)DataProtocollazione ?? DBNull.Value });
			parametri.Add(new SqlParameter("NumeroProtocollo", SqlDbType.DateTime) { Value = (object)NumeroProtocollo ?? DBNull.Value });
			parametri.Add(new SqlParameter("DataProtocollo", SqlDbType.DateTime) { Value = (object)DataProtocollo ?? DBNull.Value });
			parametri.Add(new SqlParameter("DataInvioEmail", SqlDbType.DateTime) { Value = (object)DataInvioEmail ?? DBNull.Value });
			parametri.Add(new SqlParameter("IdDocumentoNomina", SqlDbType.Int) { Value = (object)IdDocumentoNomina ?? DBNull.Value });
			parametri.Add(new SqlParameter("Albo", SqlDbType.VarChar, 50) { Value = (object)albo ?? DBNull.Value });
			SqlParameter esito = new SqlParameter("Esito", SqlDbType.Bit);
			esito.Direction = ParameterDirection.Output;
			parametri.Add(esito);
			SqlParameter messaggio = new SqlParameter("Messaggio", SqlDbType.VarChar, 1000);
			messaggio.Direction = ParameterDirection.Output;
			parametri.Add(messaggio);

			//parametri.Add(new SqlParameter("P_Esito", Esito));
			//parametri.Add(new SqlParameter("P_messaggio", messaggio));
			string dbHelios = Parametri.heliosDB??"unscsviluppo";
			Database.ExecuteSqlRaw($"EXEC {dbHelios}.dbo.SP_REGISTRAZIONE_ACQUISIZIONE " +
				"@IdRegistrazione," +
				"@DataInserimento," +
				"@CodiceFiscaleEnte," +
				"@CodiceFiscaleLegaleRappresentante," +
				"@Denominazione," +
				"@NomeLegaleRappresentante," +
				"@CognomeLegaleRappresentante," +
				"@DataNascitaLegaleRappresentante," +
				"@ComuneNascitaLegaleRappresentante," +
				"@DataNominaRappresentanteLegale," +
				"@EnteTitolare," +
				"@IdCategoriaEnte," +
				"@IdTipologiaEnte," +
				"@IdProvinciaEnte," +
				"@IdComuneEnte," +
				"@Via," +
				"@Civico," +
				"@CAP," +
				"@Telefono," +
				"@Email," +
				"@PEC," +
				"@Sito," +
				"@DichiarazionePrivacy," +
				"@DichiarazioneRappresentanteLegale," +
				"@IdDocumento," +
				"@VariazioneRappresentanteLegale," +
				"@DataProtocollazione," +
				"@NumeroProtocollo," +
				"@DataProtocollo," +
				"@DataInvioEmail," +
				"@IdDocumentoNomina," +
				"@Albo," +
				"@Esito out," +
				"@messaggio out"

		, parametri);


			return new SPResult()
			{
				Esito = (bool)esito.Value,
				Messaggio = (string)messaggio.Value
			};
		}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{

		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<SPResult>(entity =>
			{
					entity.HasNoKey();
			});

			#region Tipologia Ente
			modelBuilder.Entity<TipologiaEnte>(entity =>
			{
				entity.HasKey(e => e.Id);

				entity.ToTable("VWTipologiaEnte");

				entity.Property(e => e.Descrizione)
					.IsRequired()
					.HasMaxLength(200);

				entity.Property(e => e.IdCategoriaEnte)
					.IsRequired();

				//entity.Property(e => e.DataInserimento)
				//	.IsRequired();

				//entity.Property(e => e.UtenteInserimento)
				//	.IsRequired()
				//	.HasMaxLength(200);

				entity.HasOne(e => e.Categoria)
					.WithMany(e => e.Tipologie)
					.HasForeignKey(e => e.IdCategoriaEnte);


			});
			#endregion

			modelBuilder.Entity<Registrazione>(entity =>
			{
				entity.HasKey(e => e.Id);

				entity.ToTable("Registrazione");

				entity.Property(e => e.CodiceFiscaleRappresentanteLegale)
					.IsRequired()
					.HasMaxLength(30)
					/*.HasConversion(
						i => Encryptor.Encrypt(i),
						o => Encryptor.Decrypt(o)
					)*/
					.IsUnicode(false);

				entity.Property(e => e.Via)
					.IsRequired(false);
				entity.Property(e => e.Civico)
					.IsRequired(false);
				entity.Property(e => e.CAP)
					.IsRequired(false);
				entity.Property(e => e.Email)
					.IsRequired(false);
				entity.Property(e => e.PEC)
					.IsRequired(false);
				entity.Property(e => e.Sito)
					.IsRequired(false);
				entity.Property(e => e.Telefono)
					.IsRequired(false);
				entity.Property(e => e.EnteTitolare)
					.IsRequired(false);
				entity.Property(e => e.IdCategoriaEnte)
					.IsRequired(false);
				entity.Property(e => e.IdTipologiaEnte)
					.IsRequired(false);
				entity.Property(e => e.IdProvinciaEnte)
					.IsRequired(false);
				entity.Property(e => e.IdComuneEnte)
					.IsRequired(false); 
				entity.Property(e => e.DataInserimento)
					.IsRequired()
					.HasColumnType("datetime");

				entity.Property(e => e.DataProtocollazione)
					.HasColumnType("datetime");

				entity.Property(e => e.DataInvioEmail)
					.HasColumnType("datetime");


				entity.Property(e => e.DataNominaRappresentanteLegale)
					.HasColumnType("date");

				entity.HasOne(e => e.Comune)
					.WithMany(e => e.Registrazioni)
					.HasForeignKey(e => e.IdComuneEnte);

				entity.HasOne(e => e.Provincia)
					.WithMany(e => e.Registrazioni)
					.HasForeignKey(e => e.IdProvinciaEnte);

				entity.HasOne(e => e.Categoria)
					.WithMany(e => e.Registrazioni)
					.HasForeignKey(e => e.IdCategoriaEnte);

				entity.HasOne(e => e.Documento)
					.WithMany(e => e.Registrazioni)
					.HasForeignKey(e => e.IdDocumento);

				entity.HasOne(e => e.DocumentoNomina)
					.WithMany(e => e.RegistrazioniNomina)
					.HasForeignKey(e => e.IdDocumentoNomina);

				entity.HasOne(e => e.TipologiaEnte)
					.WithMany(e => e.Registrazioni)
					.HasForeignKey(e => e.IdTipologiaEnte);


			});

			#region Comune
			modelBuilder.Entity<Comune>(entity =>
			{
				entity.HasKey(e => e.Id);

				entity.ToTable("VWComune");

				entity.Property(e => e.Nome)
					.IsRequired()
					.HasConversion(
						i => i.ToUpper(),
						o => CultureInfo.CurrentCulture.TextInfo.ToTitleCase(o.ToLower())
					)
					.HasMaxLength(50);

				entity.Property(e => e.CodiceCatastale)
					.IsRequired()
					.IsUnicode(false);

				entity.HasOne(e => e.Provincia)
					.WithMany(e => e.Comuni)
					.HasForeignKey(e => e.IdProvincia);

			});

			#endregion


			#region Provincia
			modelBuilder.Entity<Provincia>(entity =>
			{
				entity.HasKey(e => e.Id);

				entity.ToTable("VWProvincia");

				entity.Property(e => e.Nome)
					.IsRequired()
					.HasMaxLength(50);

				entity.Property(e => e.Sigla)
					.IsRequired()
					.HasMaxLength(2)
					.IsUnicode(false);

			});
			#endregion

			modelBuilder.Entity<UtenteEnte>(entity =>
			{
				entity.HasNoKey();
				entity.ToTable("VwEnti");
				/*entity.Property(e => e.CodiceFiscale)
					.IsRequired()
					.HasMaxLength(30)
					.HasConversion(
						i => Encryptor.Encrypt(i),
						o => Encryptor.Decrypt(o)   
					)
					.IsUnicode(false);*/
			});

			modelBuilder.Entity<Statistiche>(entity =>
			{
				entity.HasNoKey();
				entity.ToTable("VWStatistiche");
			});

			modelBuilder.Entity<StatisticheAndamento>(entity =>
			{
				entity.HasNoKey();
				entity.ToTable("VWAndamento");
			});

			modelBuilder.Entity<DatiDomanda>(entity =>
			{
				entity.ToTable("VWProtocollo");
			});

			modelBuilder.Entity<DatiDomandaOLP>(entity =>
			{
				entity.ToTable("VWDatiDomandaOLP");
			});

			modelBuilder.Entity<DatiDomandaTUTORAGGIO>(entity =>
			{
				entity.ToTable("VWDatiDomandaTUTORAGGIO");
			});

			modelBuilder.Entity<DatiAntimafia>(entity =>
			{
				entity.ToTable("VWDatiProtocolloAntimafia");
			});

			modelBuilder.Entity<ProtocolloPresentazione>(entity =>
			{
				entity.ToTable("VWProtocolloPresentazione");
				entity.Property(e => e.DataProtocollazione)
					.HasColumnType("datetime");
				entity.Property(e => e.DataInvioEmail)
					.HasColumnType("datetime");
			});

			modelBuilder.Entity<ProtocolloAntimafia>(entity =>
			{
				entity.ToTable("VWProtocolloAntimafia");
				entity.Property(e => e.DataProtocollazione)
					.HasColumnType("datetime");
				entity.Property(e => e.DataInvioEmail)
					.HasColumnType("datetime");
			});

			modelBuilder.Entity<ProtocolloOLP>(entity =>
			{
				entity.ToTable("VWProtocolloOLP");
				entity.Property(e => e.DataProtocollazione)
					.HasColumnType("datetime");
				entity.Property(e => e.DataInvioEmail)
					.HasColumnType("datetime");
			});

			modelBuilder.Entity<ProtocolloTUTORAGGIO>(entity =>
			{
				entity.ToTable("VWProtocolloTUTORAGGIO");
				entity.Property(e => e.DataProtocollazione)
					.HasColumnType("datetime");
				entity.Property(e => e.DataInvioEmail)
					.HasColumnType("datetime");
			});

			modelBuilder.Entity<ProtocolloProgramma>(entity =>
			{
				entity.ToTable("VWProtocolloProgetto");
				entity.Property(e => e.DataProtocollazione)
					.HasColumnType("datetime");
				entity.Property(e => e.DataInvioEmail)
					.HasColumnType("datetime");
			});

			modelBuilder.Entity<DatiDomandaProgramma>(entity =>
			{
				entity.ToTable("VWProtocolloDatiProgramma");
				entity.Property(e => e.DataProtocollazione)
					.HasColumnType("datetime");
				entity.Property(e => e.DataInvioEmail)
					.HasColumnType("datetime");
			});


			modelBuilder.Entity<Soggetto>(entity =>
			{
				entity.HasKey(e=>e.CodiceFiscale);
				entity.ToTable("Soggetto");
			});

			modelBuilder.Entity<Configurazione>(entity =>
			{
				entity.HasKey(e => e.Id);
				entity.ToTable("Configurazione");
			});

			OnModelCreatingPartial(modelBuilder);
		}

		partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
	}
}
