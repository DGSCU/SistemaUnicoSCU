USE [DomandaOnline]
GO
/****** Object:  Table [dbo].[__MigrationHistory]    Script Date: 12/08/2022 15:32:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[__MigrationHistory](
	[MigrationId] [nvarchar](150) NOT NULL,
	[ContextKey] [nvarchar](300) NOT NULL,
	[Model] [varbinary](max) NOT NULL,
	[ProductVersion] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK_dbo.__MigrationHistory] PRIMARY KEY CLUSTERED 
(
	[MigrationId] ASC,
	[ContextKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[_BK_PRESENTAZIONE]    Script Date: 12/08/2022 15:32:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[_BK_PRESENTAZIONE](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[DataPresentazione] [datetime] NULL,
	[DataAnnullamento] [datetime] NULL,
	[DataRichiestaAnnullamento] [datetime] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetRoles]    Script Date: 12/08/2022 15:32:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetRoles](
	[Id] [nvarchar](128) NOT NULL,
	[Name] [nvarchar](256) NOT NULL,
 CONSTRAINT [PK_dbo.AspNetRoles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUserClaims]    Script Date: 12/08/2022 15:32:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserClaims](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [nvarchar](128) NOT NULL,
	[ClaimType] [nvarchar](max) NULL,
	[ClaimValue] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.AspNetUserClaims] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUserLogins]    Script Date: 12/08/2022 15:32:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserLogins](
	[LoginProvider] [nvarchar](128) NOT NULL,
	[ProviderKey] [nvarchar](128) NOT NULL,
	[UserId] [nvarchar](128) NOT NULL,
 CONSTRAINT [PK_dbo.AspNetUserLogins] PRIMARY KEY CLUSTERED 
(
	[LoginProvider] ASC,
	[ProviderKey] ASC,
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUserRoles]    Script Date: 12/08/2022 15:32:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserRoles](
	[UserId] [nvarchar](128) NOT NULL,
	[RoleId] [nvarchar](128) NOT NULL,
 CONSTRAINT [PK_dbo.AspNetUserRoles] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUsers]    Script Date: 12/08/2022 15:32:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUsers](
	[Id] [nvarchar](128) NOT NULL,
	[CodiceFiscale] [varchar](30) NULL,
	[Nome] [nvarchar](max) NULL,
	[Cognome] [nvarchar](max) NULL,
	[Spidcode] [nvarchar](max) NULL,
	[Genere] [nvarchar](max) NULL,
	[LuogoNascita] [nvarchar](max) NULL,
	[NazioneNascita] [nvarchar](max) NULL,
	[Telefono] [nvarchar](max) NULL,
	[Indirizzo] [nvarchar](max) NULL,
	[Documento] [nvarchar](max) NULL,
	[ExpirationDate] [nvarchar](max) NULL,
	[DataNascita] [datetime] NULL,
	[Cittadinanza] [nvarchar](max) NULL,
	[Email] [nvarchar](256) NULL,
	[EmailConfirmed] [bit] NOT NULL,
	[PasswordHash] [nvarchar](max) NULL,
	[SecurityStamp] [nvarchar](max) NULL,
	[PhoneNumber] [nvarchar](max) NULL,
	[PhoneNumberConfirmed] [bit] NOT NULL,
	[TwoFactorEnabled] [bit] NOT NULL,
	[LockoutEndDateUtc] [datetime] NULL,
	[LockoutEnabled] [bit] NOT NULL,
	[AccessFailedCount] [int] NOT NULL,
	[UserName] [nvarchar](256) NOT NULL,
 CONSTRAINT [PK_dbo.AspNetUsers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BKP_DomandaPartecipazione]    Script Date: 12/08/2022 15:32:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BKP_DomandaPartecipazione](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CodiceFiscale] [nvarchar](30) NOT NULL,
	[GruppoBando] [int] NOT NULL,
	[UserIdInserimento] [nvarchar](128) NOT NULL,
	[DataInserimento] [datetime] NOT NULL,
	[UserIdModifica] [nvarchar](128) NULL,
	[DataModifica] [datetime] NULL,
	[CodiceProgettoSelezionato] [nvarchar](22) NULL,
	[CodiceSedeSelezionata] [int] NULL,
	[UserIdPresentazione] [nvarchar](128) NULL,
	[DataPresentazione] [datetime] NULL,
	[DataAnnullamento] [datetime] NULL,
	[AllegatoCV] [varbinary](max) NULL,
	[NomeFileCV] [nvarchar](200) NULL,
	[Nome] [nvarchar](100) NULL,
	[Cognome] [nvarchar](100) NULL,
	[Genere] [nvarchar](7) NULL,
	[DataNascita] [date] NULL,
	[LuogoNascita] [nvarchar](200) NULL,
	[NazioneNascita] [nvarchar](200) NULL,
	[Cittadinanza] [nvarchar](200) NULL,
	[Telefono] [nvarchar](200) NULL,
	[Email] [nvarchar](200) NULL,
	[ComuneResidenza] [nvarchar](200) NULL,
	[ProvinciaResidenza] [nvarchar](200) NULL,
	[ViaResidenza] [nvarchar](200) NULL,
	[CivicoResidenza] [nvarchar](200) NULL,
	[CapResidenza] [nvarchar](200) NULL,
	[ComuneRecapito] [nvarchar](200) NULL,
	[ProvinciaRecapito] [nvarchar](200) NULL,
	[ViaRecapito] [nvarchar](200) NULL,
	[CivicoRecapito] [nvarchar](200) NULL,
	[CapRecapito] [nvarchar](200) NULL,
	[CodiceMinoriOpportunita] [nvarchar](5) NULL,
	[IdMotivazione] [int] NULL,
	[CodiceDichiarazioneCittadinanza] [nvarchar](5) NULL,
	[NonCondanneOk] [bit] NULL,
	[TrasferimentoSedeOk] [bit] NULL,
	[TrasferimentoProgettoOk] [bit] NULL,
	[AltreDichiarazioniOk] [bit] NULL,
	[IdTitoloStudio] [int] NULL,
	[PrivacyPresaVisione] [bit] NULL,
	[PrivacyConsenso] [bit] NULL,
	[PrecedentiEnte] [bit] NULL,
	[PrecedentiEnteDescrizione] [nvarchar](1000) NULL,
	[PrecedentiAltriEnti] [bit] NULL,
	[PrecedentiAltriEntiDescrizione] [nvarchar](1000) NULL,
	[PrecedentiImpiego] [bit] NULL,
	[PrecedentiImpiegoDescrizione] [nvarchar](1000) NULL,
	[IdTitoloStudioEsperienze] [int] NULL,
	[FormazioneDisciplina] [nvarchar](200) NULL,
	[FormazioneAnno] [int] NULL,
	[FormazioneData] [date] NULL,
	[FormazioneItalia] [bit] NULL,
	[FormazioneIstituto] [nvarchar](200) NULL,
	[FormazioneEnte] [nvarchar](200) NULL,
	[IscrizioneSuperioreAnno] [int] NULL,
	[IscrizioneSuperioreIstituto] [nvarchar](200) NULL,
	[IscrizioneLaureaAnno] [int] NULL,
	[IscrizioneLaureaCorso] [nvarchar](200) NULL,
	[IscrizioneLaureaIstituto] [nvarchar](200) NULL,
	[CorsiEffettuati] [nvarchar](1000) NULL,
	[Specializzazioni] [nvarchar](1000) NULL,
	[Competenze] [nvarchar](1000) NULL,
	[Altro] [nvarchar](1000) NULL,
	[FileDomanda] [varbinary](max) NULL,
	[DataInvioEmailAnnullamento] [datetime] NULL,
	[NazioneResidenza] [nvarchar](200) NULL,
	[IndirizzoCompletoResidenza] [nvarchar](200) NULL,
	[ResidenzaEstera] [bit] NULL,
	[ConfermaResidenza] [bit] NULL,
	[IdZip] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CodiceBelfiore]    Script Date: 12/08/2022 15:32:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CodiceBelfiore](
	[Codice] [nvarchar](4) NOT NULL,
	[Nazione] [varchar](50) NOT NULL,
	[Comune] [varchar](50) NULL,
 CONSTRAINT [PK_CodiceBelfiore] PRIMARY KEY CLUSTERED 
(
	[Codice] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Comune]    Script Date: 12/08/2022 15:32:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Comune](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Nome] [nvarchar](50) NULL,
	[CodiceCatastale] [nvarchar](4) NULL,
	[IdPRovincia] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Configurazione]    Script Date: 12/08/2022 15:32:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Configurazione](
	[Nome] [nvarchar](50) NOT NULL,
	[Valore] [nvarchar](200) NOT NULL,
	[Descrizione] [nvarchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[Nome] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DbVersion]    Script Date: 12/08/2022 15:32:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DbVersion](
	[Version] [int] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DomandaPartecipazione]    Script Date: 12/08/2022 15:32:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DomandaPartecipazione](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CodiceFiscale] [nvarchar](30) NOT NULL,
	[GruppoBando] [int] NOT NULL,
	[UserIdInserimento] [nvarchar](128) NOT NULL,
	[DataInserimento] [datetime] NOT NULL,
	[UserIdModifica] [nvarchar](128) NULL,
	[DataModifica] [datetime] NULL,
	[CodiceProgettoSelezionato] [nvarchar](22) NULL,
	[CodiceSedeSelezionata] [int] NULL,
	[UserIdPresentazione] [nvarchar](128) NULL,
	[DataPresentazione] [datetime] NULL,
	[DataAnnullamento] [datetime] NULL,
	[AllegatoCV] [varbinary](max) NULL,
	[NomeFileCV] [nvarchar](200) NULL,
	[Nome] [nvarchar](100) NULL,
	[Cognome] [nvarchar](100) NULL,
	[Genere] [nvarchar](7) NULL,
	[DataNascita] [date] NULL,
	[LuogoNascita] [nvarchar](200) NULL,
	[NazioneNascita] [nvarchar](200) NULL,
	[Cittadinanza] [nvarchar](200) NULL,
	[Telefono] [nvarchar](200) NULL,
	[Email] [nvarchar](200) NULL,
	[ComuneResidenza] [nvarchar](200) NULL,
	[ProvinciaResidenza] [nvarchar](200) NULL,
	[ViaResidenza] [nvarchar](200) NULL,
	[CivicoResidenza] [nvarchar](200) NULL,
	[CapResidenza] [nvarchar](200) NULL,
	[ComuneRecapito] [nvarchar](200) NULL,
	[ProvinciaRecapito] [nvarchar](200) NULL,
	[ViaRecapito] [nvarchar](200) NULL,
	[CivicoRecapito] [nvarchar](200) NULL,
	[CapRecapito] [nvarchar](200) NULL,
	[CodiceMinoriOpportunita] [nvarchar](5) NULL,
	[IdMotivazione] [int] NULL,
	[CodiceDichiarazioneCittadinanza] [nvarchar](5) NULL,
	[NonCondanneOk] [bit] NULL,
	[TrasferimentoSedeOk] [bit] NULL,
	[TrasferimentoProgettoOk] [bit] NULL,
	[AltreDichiarazioniOk] [bit] NULL,
	[IdTitoloStudio] [int] NULL,
	[PrivacyPresaVisione] [bit] NULL,
	[PrivacyConsenso] [bit] NULL,
	[PrecedentiEnte] [bit] NULL,
	[PrecedentiEnteDescrizione] [nvarchar](1000) NULL,
	[PrecedentiAltriEnti] [bit] NULL,
	[PrecedentiAltriEntiDescrizione] [nvarchar](1000) NULL,
	[PrecedentiImpiego] [bit] NULL,
	[PrecedentiImpiegoDescrizione] [nvarchar](1000) NULL,
	[IdTitoloStudioEsperienze] [int] NULL,
	[FormazioneDisciplina] [nvarchar](200) NULL,
	[FormazioneAnno] [int] NULL,
	[FormazioneData] [date] NULL,
	[FormazioneItalia] [bit] NULL,
	[FormazioneIstituto] [nvarchar](200) NULL,
	[FormazioneEnte] [nvarchar](200) NULL,
	[IscrizioneSuperioreAnno] [int] NULL,
	[IscrizioneSuperioreIstituto] [nvarchar](200) NULL,
	[IscrizioneLaureaAnno] [int] NULL,
	[IscrizioneLaureaCorso] [nvarchar](200) NULL,
	[IscrizioneLaureaIstituto] [nvarchar](200) NULL,
	[CorsiEffettuati] [nvarchar](1000) NULL,
	[Specializzazioni] [nvarchar](1000) NULL,
	[Competenze] [nvarchar](1000) NULL,
	[Altro] [nvarchar](1000) NULL,
	[FileDomanda] [varbinary](max) NULL,
	[DataInvioEmailAnnullamento] [datetime] NULL,
	[NazioneResidenza] [nvarchar](200) NULL,
	[IndirizzoCompletoResidenza] [nvarchar](200) NULL,
	[ResidenzaEstera] [bit] NULL,
	[ConfermaResidenza] [bit] NULL,
	[IdZip] [int] NULL,
	[DataRichiestaAnnullamento] [datetime] NULL,
	[IdMotivazioneAnnullamento] [int] NULL,
	[FormazioneAnagraficaDisciplina] [nvarchar](200) NULL,
	[FormazioneAnagraficaAnno] [int] NULL,
	[FormazioneAnagraficaItalia] [bit] NULL,
	[FormazioneAnagraficaIstituto] [nvarchar](200) NULL,
	[FormazioneAnagraficaEnte] [nvarchar](200) NULL,
	[DichiarazioneResidenzaOK] [bit] NULL,
	[DichiarazioneRequisitiGaranziaGiovani] [bit] NULL,
	[DataPresaInCaricoGaranziaGiovani] [date] NULL,
	[LuogoPresaInCaricoGaranziaGiovani] [nvarchar](200) NULL,
	[DataDIDGaranziaGiovani] [date] NULL,
	[LuogoDIDGaranziaGiovani] [nvarchar](200) NULL,
	[AlternativaRequisitiGaranziaGiovani] [bit] NULL,
	[DichiarazioneMinoriOpportunita] [bit] NULL,
	[NumeroDomande] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DomandaVariata]    Script Date: 12/08/2022 15:32:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DomandaVariata](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[DataPresentazione] [datetime] NULL,
	[DataCaricamento] [datetime] NULL,
	[Email] [nvarchar](200) NULL,
	[CodiceFiscale] [nvarchar](30) NOT NULL,
	[FileVecchio] [varbinary](max) NULL,
	[IdDomanda] [int] NULL,
	[DataInvioEmail] [datetime] NULL,
	[TextOld] [nvarchar](max) NULL,
	[TextNew] [nvarchar](max) NULL,
	[Dichiarazione] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DomandeZip]    Script Date: 12/08/2022 15:32:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DomandeZip](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[IdBando] [int] NULL,
	[CodiceEnte] [nvarchar](10) NULL,
	[NomeFile] [nvarchar](200) NULL,
	[Progressivo] [int] NULL,
	[DimensioneByte] [int] NULL,
	[DataInizioProcesso] [datetime] NULL,
	[DataFineProcesso] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Foglio1$]    Script Date: 12/08/2022 15:32:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Foglio1$](
	[idVolontario] [float] NULL,
	[Cognome] [nvarchar](255) NULL,
	[Nome] [nvarchar](255) NULL,
	[CodiceFiscale] [nvarchar](255) NULL,
	[DataNascita] [nvarchar](255) NULL,
	[Sesso] [nvarchar](255) NULL,
	[CodiceISTATComuneNascita] [float] NULL,
	[Categoria] [nvarchar](255) NULL,
	[CodiceISTATComuneResidenza] [float] NULL,
	[Indirizzo] [nvarchar](255) NULL,
	[NumeroCivico] [float] NULL,
	[Cap] [float] NULL,
	[CodiceVolontario] [nvarchar](255) NULL,
	[idProgetto] [float] NULL,
	[DataInizioPrevista] [datetime] NULL,
	[DataFinePrevista] [datetime] NULL,
	[idEnteSede] [float] NULL,
	[CodiceSedePrimoGiorno] [float] NULL,
	[idTipoPosto] [float] NULL,
	[SubentroStessoProgetto] [float] NULL,
	[SubentroAltriProgetti] [float] NULL,
	[Telefono] [float] NULL,
	[idTitoloStudio] [float] NULL,
	[ConseguimentoTitoloStudio] [nvarchar](255) NULL,
	[Email] [nvarchar](255) NULL,
	[CodiceISTATComuneDomicilio] [float] NULL,
	[IndirizzoDomicilio] [nvarchar](255) NULL,
	[NumeroCivicoDomicilio] [float] NULL,
	[CapDomicilio] [float] NULL,
	[DettaglioRecapitoRes] [nvarchar](255) NULL,
	[DettaglioRecapitoDom] [nvarchar](255) NULL,
	[idStatoCivile] [float] NULL,
	[CodiceFiscaleConiuge] [nvarchar](255) NULL,
	[FlagIndirizzoValidoRes] [float] NULL,
	[FlagIndirizzoValidoDom] [float] NULL,
	[AnomaliaCF] [float] NULL,
	[idFascicolo] [nvarchar](255) NULL,
	[codFascicolo] [nvarchar](255) NULL,
	[idStatoVolontario] [float] NULL,
	[idMotivoEsclusione] [nvarchar](255) NULL,
	[note] [nvarchar](255) NULL,
	[dataModifica] [datetime] NULL,
	[usrModifica] [nvarchar](255) NULL,
	[IBAN] [nvarchar](255) NULL,
	[BIC] [nvarchar](255) NULL,
	[oreFormazioneEffettuate] [nvarchar](255) NULL,
	[username] [nvarchar](255) NULL,
	[password] [nvarchar](255) NULL,
	[abilitato] [float] NULL,
	[utenteModificaPassword] [nvarchar](255) NULL,
	[dataModificaPassword] [datetime] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Genere]    Script Date: 12/08/2022 15:32:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Genere](
	[Codice] [nvarchar](1) NOT NULL,
	[Nome] [nvarchar](20) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Codice] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Log]    Script Date: 12/08/2022 15:32:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Log](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Message] [nvarchar](max) NULL,
	[MessageTemplate] [nvarchar](max) NULL,
	[Level] [nvarchar](128) NULL,
	[TimeStamp] [datetime] NOT NULL,
	[Exception] [nvarchar](max) NULL,
	[Properties] [nvarchar](max) NULL,
 CONSTRAINT [PK_Log] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LOGAnomalia]    Script Date: 12/08/2022 15:32:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LOGAnomalia](
	[uid] [varchar](36) NOT NULL,
	[timestamp] [datetime] NOT NULL,
	[message] [nvarchar](max) NULL,
	[properties] [nvarchar](max) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LOGCVPrima]    Script Date: 12/08/2022 15:32:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LOGCVPrima](
	[UserIdModifica] [nvarchar](4000) NULL,
	[timestamp] [datetime] NOT NULL,
	[message] [nvarchar](max) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LOGDatiModificatiPrima]    Script Date: 12/08/2022 15:32:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LOGDatiModificatiPrima](
	[UserIdModifica] [nvarchar](4000) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LogProva]    Script Date: 12/08/2022 15:32:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LogProva](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Message] [nvarchar](max) NULL,
	[Level] [nvarchar](128) NULL,
	[TimeStamp] [datetime] NOT NULL,
	[Exception] [nvarchar](max) NULL,
	[UserId] [nvarchar](max) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LOGUltimoCVInserito]    Script Date: 12/08/2022 15:32:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LOGUltimoCVInserito](
	[UserIdModifica] [nvarchar](4000) NULL,
	[timestamp] [datetime] NOT NULL,
	[message] [nvarchar](max) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LOGUltimoCVRimosso]    Script Date: 12/08/2022 15:32:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LOGUltimoCVRimosso](
	[UserIdModifica] [nvarchar](4000) NULL,
	[timestamp] [datetime] NOT NULL,
	[message] [nvarchar](max) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Motivazione]    Script Date: 12/08/2022 15:32:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Motivazione](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Descrizione] [nvarchar](100) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MotivoAnnullamento]    Script Date: 12/08/2022 15:32:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MotivoAnnullamento](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Descrizione] [nvarchar](100) NOT NULL,
	[Selezionabile] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Nazione]    Script Date: 12/08/2022 15:32:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Nazione](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Nome] [nvarchar](50) NULL,
	[CodiceISO2] [nvarchar](2) NULL,
	[CodiceISO3] [nvarchar](3) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ProgettoPreferito]    Script Date: 12/08/2022 15:32:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProgettoPreferito](
	[CodiceFiscale] [nvarchar](30) NOT NULL,
	[CodiceProgetto] [nvarchar](22) NOT NULL,
	[CodiceSede] [int] NOT NULL,
 CONSTRAINT [PK_ProgettoPreferito] PRIMARY KEY NONCLUSTERED 
(
	[CodiceFiscale] ASC,
	[CodiceProgetto] ASC,
	[CodiceSede] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 70) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Provincia]    Script Date: 12/08/2022 15:32:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Provincia](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Nome] [nvarchar](50) NULL,
	[Sigla] [nvarchar](2) NULL,
	[idRegione] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Regione]    Script Date: 12/08/2022 15:32:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Regione](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Nome] [nvarchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RequisitiGaranziaGiovani]    Script Date: 12/08/2022 15:32:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RequisitiGaranziaGiovani](
	[TipoGG] [varchar](100) NOT NULL,
	[IdRegione] [int] NULL,
	[IdProvincia] [int] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RichiestaCredenziali]    Script Date: 12/08/2022 15:32:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RichiestaCredenziali](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[DataRichiesta] [datetime] NULL,
	[IdStato] [int] NULL,
	[DataApprovazione] [datetime] NULL,
	[UtenteApprovazione] [nvarchar](256) NULL,
	[NoteApprovazione] [nvarchar](max) NULL,
	[DataAnnullamento] [datetime] NULL,
	[UtenteAnnullamento] [nvarchar](256) NULL,
	[NoteAnnullamento] [nvarchar](max) NULL,
	[Nome] [nvarchar](100) NULL,
	[Cognome] [nvarchar](100) NULL,
	[CodiceGenere] [nvarchar](1) NULL,
	[DataNascita] [date] NULL,
	[LuogoNascita] [nvarchar](200) NULL,
	[IdNazioneNascita] [int] NULL,
	[CodiceFiscale] [nvarchar](30) NULL,
	[IdNazioneCittadinanza] [int] NULL,
	[Email] [nvarchar](100) NULL,
	[Telefono] [nvarchar](20) NULL,
	[Allegato] [varbinary](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Specializzazione]    Script Date: 12/08/2022 15:32:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Specializzazione](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Descrizione] [nvarchar](100) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[StatoRichiestaCredenziali]    Script Date: 12/08/2022 15:32:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[StatoRichiestaCredenziali](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Nome] [nvarchar](50) NULL,
	[Descrizione] [nvarchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SUSCN_DOL_AMBITO]    Script Date: 12/08/2022 15:32:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SUSCN_DOL_AMBITO](
	[IdAmbitoAzione] [int] NOT NULL,
	[Descrizione] [nvarchar](255) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[IdAmbitoAzione] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SUSCN_DOL_BANDOGRUPPO]    Script Date: 12/08/2022 15:32:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SUSCN_DOL_BANDOGRUPPO](
	[Gruppo] [int] NOT NULL,
	[Descrizione] [nvarchar](1000) NULL,
	[Scadenza] [nvarchar](50) NOT NULL,
	[DataScadenza] [datetime] NOT NULL,
	[DataScadenzaGraduatorie] [datetime] NULL,
	[GiorniPostScadenza] [int] NULL,
	[programmi] [bit] NULL,
	[DataFineAnnullamento] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[Gruppo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SUSCN_DOL_CODICE_FISCALE_VOLONTARIO]    Script Date: 12/08/2022 15:32:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SUSCN_DOL_CODICE_FISCALE_VOLONTARIO](
	[CodiceFiscale] [nvarchar](30) NOT NULL,
	[DataInizioServizio] [datetime] NOT NULL,
	[DataFineServizio] [datetime] NOT NULL,
	[TipoServizio] [varchar](3) NOT NULL,
 CONSTRAINT [PK_SUSCN_DOL_CODICE_FISCALE_VOLONTARIO] PRIMARY KEY CLUSTERED 
(
	[CodiceFiscale] ASC,
	[DataInizioServizio] ASC,
	[DataFineServizio] ASC,
	[TipoServizio] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SUSCN_DOL_GARANZIA_GIOVANI]    Script Date: 12/08/2022 15:32:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SUSCN_DOL_GARANZIA_GIOVANI](
	[IdTipoGG] [int] NOT NULL,
	[Regione] [varchar](50) NOT NULL,
	[Descrizione] [varchar](100) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[IdTipoGG] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SUSCN_DOL_GEOGRAFICO_ITALIA]    Script Date: 12/08/2022 15:32:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SUSCN_DOL_GEOGRAFICO_ITALIA](
	[REGIONE] [nvarchar](50) NOT NULL,
	[PROVINCIA] [nvarchar](50) NOT NULL,
	[COMUNE] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_SUSCN_DOL_GEOGRAFICO_ITALIA] PRIMARY KEY NONCLUSTERED 
(
	[REGIONE] ASC,
	[PROVINCIA] ASC,
	[COMUNE] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 70) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SUSCN_DOL_MINORE_OPPORTUNITA]    Script Date: 12/08/2022 15:32:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SUSCN_DOL_MINORE_OPPORTUNITA](
	[IDParticolarità] [int] NOT NULL,
	[Descrizione] [varchar](100) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[IDParticolarità] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SUSCN_DOL_OBIETTIVO]    Script Date: 12/08/2022 15:32:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SUSCN_DOL_OBIETTIVO](
	[IdObiettivo] [int] NOT NULL,
	[Obiettivo] [nvarchar](50) NULL,
	[Descrizione] [nvarchar](255) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[IdObiettivo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SUSCN_DOL_OBIETTIVO_PROGRAMMA]    Script Date: 12/08/2022 15:32:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SUSCN_DOL_OBIETTIVO_PROGRAMMA](
	[IdProgramma] [int] NOT NULL,
	[IdObiettivo] [int] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SUSCN_DOL_PROGETTI_DISPONIBILI]    Script Date: 12/08/2022 15:32:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SUSCN_DOL_PROGETTI_DISPONIBILI](
	[CodiceEnte] [nvarchar](10) NULL,
	[NomeEnte] [nvarchar](200) NOT NULL,
	[Sito] [nvarchar](100) NULL,
	[CodiceProgetto] [nvarchar](22) NOT NULL,
	[TitoloProgetto] [nvarchar](255) NOT NULL,
	[TipoProgetto] [nvarchar](6) NOT NULL,
	[CodiceSede] [int] NOT NULL,
	[IndirizzoSede] [nvarchar](306) NULL,
	[Regione] [nvarchar](50) NOT NULL,
	[Provincia] [nvarchar](50) NOT NULL,
	[Comune] [nvarchar](100) NOT NULL,
	[Settore] [nvarchar](500) NULL,
	[Area] [nvarchar](150) NOT NULL,
	[NumeroPostiDisponibili] [smallint] NULL,
	[Gruppo] [int] NOT NULL,
	[Misure] [varchar](2) NOT NULL,
	[DurataProgettoMesi] [tinyint] NOT NULL,
	[NumeroGiovaniMinoriOpportunità] [smallint] NOT NULL,
	[EsteroUE] [nvarchar](2) NOT NULL,
	[Tutoraggio] [nvarchar](2) NOT NULL,
	[DataAnnullamento] [datetime] NULL,
	[IDParticolaritàEntità] [int] NULL,
	[IdProgramma] [int] NULL,
	[LinkSintesi] [nvarchar](1000) NULL,
	[EnteAttuatore] [nvarchar](200) NULL,
	[NumeroDomande] [int] NULL,
 CONSTRAINT [PK_SUSCN_DOL_PROGETTI_DISPONIBILI] PRIMARY KEY NONCLUSTERED 
(
	[CodiceProgetto] ASC,
	[CodiceSede] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SUSCN_DOL_PROGETTI_DISPONIBILI2]    Script Date: 12/08/2022 15:32:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SUSCN_DOL_PROGETTI_DISPONIBILI2](
	[CodiceEnte] [nvarchar](10) NULL,
	[NomeEnte] [nvarchar](200) NOT NULL,
	[Sito] [nvarchar](100) NULL,
	[CodiceProgetto] [nvarchar](22) NOT NULL,
	[TitoloProgetto] [nvarchar](255) NOT NULL,
	[TipoProgetto] [nvarchar](6) NOT NULL,
	[CodiceSede] [int] NOT NULL,
	[IndirizzoSede] [nvarchar](306) NULL,
	[Regione] [nvarchar](50) NOT NULL,
	[Provincia] [nvarchar](50) NOT NULL,
	[Comune] [nvarchar](100) NOT NULL,
	[Settore] [nvarchar](500) NULL,
	[Area] [nvarchar](150) NOT NULL,
	[NumeroPostiDisponibili] [smallint] NULL,
	[Gruppo] [int] NOT NULL,
	[Misure] [varchar](2) NOT NULL,
	[DurataProgettoMesi] [tinyint] NOT NULL,
	[NumeroGiovaniMinoriOpportunità] [smallint] NOT NULL,
	[EsteroUE] [nvarchar](2) NOT NULL,
	[Tutoraggio] [nvarchar](2) NOT NULL,
	[DataAnnullamento] [datetime] NULL,
	[IDParticolaritàEntità] [int] NULL,
	[IdProgramma] [int] NULL,
	[LinkSintesi] [nvarchar](1000) NULL,
	[EnteAttuatore] [nvarchar](200) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SUSCN_DOL_PROGRAMMA]    Script Date: 12/08/2022 15:32:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SUSCN_DOL_PROGRAMMA](
	[IdProgramma] [int] NOT NULL,
	[Titolo] [nvarchar](255) NOT NULL,
	[IdAmbitoAzione] [int] NOT NULL,
	[IdTipoGG] [int] NULL,
	[IsDigitale] [bit] NULL,
	[IsAmbientale] [bit] NULL,
	[IsAutofinanziato] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[IdProgramma] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SUSCN_DOL_STORICO_PROGETTI]    Script Date: 12/08/2022 15:32:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SUSCN_DOL_STORICO_PROGETTI](
	[DataStorico] [datetime] NULL,
	[CodiceEnte] [nvarchar](10) NULL,
	[NomeEnte] [nvarchar](200) NOT NULL,
	[Sito] [nvarchar](100) NULL,
	[CodiceProgetto] [nvarchar](22) NOT NULL,
	[TitoloProgetto] [nvarchar](255) NOT NULL,
	[TipoProgetto] [nvarchar](6) NOT NULL,
	[CodiceSede] [int] NOT NULL,
	[IndirizzoSede] [nvarchar](306) NULL,
	[Regione] [nvarchar](50) NOT NULL,
	[Provincia] [nvarchar](50) NOT NULL,
	[Comune] [nvarchar](100) NOT NULL,
	[Settore] [nvarchar](500) NULL,
	[Area] [nvarchar](150) NOT NULL,
	[NumeroPostiDisponibili] [smallint] NULL,
	[Gruppo] [int] NOT NULL,
	[Misure] [varchar](2) NOT NULL,
	[DurataProgettoMesi] [tinyint] NOT NULL,
	[NumeroGiovaniMinoriOpportunità] [smallint] NOT NULL,
	[EsteroUE] [nvarchar](2) NOT NULL,
	[Tutoraggio] [nvarchar](2) NOT NULL,
	[DataAnnullamento] [datetime] NULL,
	[IDParticolaritàEntità] [int] NULL,
	[IdProgramma] [int] NULL,
	[LinkSintesi] [nvarchar](1000) NULL,
	[EnteAttuatore] [nvarchar](200) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SUSCN_DOL_VOLONTARIO_INTERRUZIONE_COVID]    Script Date: 12/08/2022 15:32:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SUSCN_DOL_VOLONTARIO_INTERRUZIONE_COVID](
	[CodiceFiscale] [nvarchar](16) NOT NULL,
	[AnnoInterruzione] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[CodiceFiscale] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TitoloStudio]    Script Date: 12/08/2022 15:32:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TitoloStudio](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Descrizione] [nvarchar](100) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TokenCredenziali]    Script Date: 12/08/2022 15:32:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TokenCredenziali](
	[Id] [uniqueidentifier] NOT NULL,
	[code] [nvarchar](max) NOT NULL,
	[UserId] [nvarchar](128) NOT NULL,
	[DataGenerazione] [datetime] NOT NULL,
	[Scadenza] [datetime] NOT NULL,
	[DataUtilizzo] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[TokenCredenziali] ADD  DEFAULT (newid()) FOR [Id]
GO
ALTER TABLE [dbo].[AspNetUserClaims]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AspNetUserClaims_dbo.AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserClaims] CHECK CONSTRAINT [FK_dbo.AspNetUserClaims_dbo.AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[AspNetUserLogins]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AspNetUserLogins_dbo.AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserLogins] CHECK CONSTRAINT [FK_dbo.AspNetUserLogins_dbo.AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[AspNetUserRoles]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetRoles_RoleId] FOREIGN KEY([RoleId])
REFERENCES [dbo].[AspNetRoles] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserRoles] CHECK CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetRoles_RoleId]
GO
ALTER TABLE [dbo].[AspNetUserRoles]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserRoles] CHECK CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[Comune]  WITH CHECK ADD  CONSTRAINT [FK_Comune_Provincia] FOREIGN KEY([IdPRovincia])
REFERENCES [dbo].[Provincia] ([Id])
GO
ALTER TABLE [dbo].[Comune] CHECK CONSTRAINT [FK_Comune_Provincia]
GO
ALTER TABLE [dbo].[DomandaPartecipazione]  WITH CHECK ADD  CONSTRAINT [FK_DomandaMotivazione] FOREIGN KEY([IdMotivazione])
REFERENCES [dbo].[Motivazione] ([Id])
GO
ALTER TABLE [dbo].[DomandaPartecipazione] CHECK CONSTRAINT [FK_DomandaMotivazione]
GO
ALTER TABLE [dbo].[DomandaPartecipazione]  WITH CHECK ADD  CONSTRAINT [FK_DomandaOnline_Zip] FOREIGN KEY([IdZip])
REFERENCES [dbo].[DomandeZip] ([Id])
GO
ALTER TABLE [dbo].[DomandaPartecipazione] CHECK CONSTRAINT [FK_DomandaOnline_Zip]
GO
ALTER TABLE [dbo].[DomandaPartecipazione]  WITH CHECK ADD  CONSTRAINT [FK_DomandaPartecipazione_MotivoAnnullamento] FOREIGN KEY([IdMotivazioneAnnullamento])
REFERENCES [dbo].[MotivoAnnullamento] ([Id])
GO
ALTER TABLE [dbo].[DomandaPartecipazione] CHECK CONSTRAINT [FK_DomandaPartecipazione_MotivoAnnullamento]
GO
ALTER TABLE [dbo].[DomandaPartecipazione]  WITH CHECK ADD  CONSTRAINT [FK_DomandaProgetto] FOREIGN KEY([CodiceProgettoSelezionato], [CodiceSedeSelezionata])
REFERENCES [dbo].[SUSCN_DOL_PROGETTI_DISPONIBILI] ([CodiceProgetto], [CodiceSede])
GO
ALTER TABLE [dbo].[DomandaPartecipazione] CHECK CONSTRAINT [FK_DomandaProgetto]
GO
ALTER TABLE [dbo].[DomandaPartecipazione]  WITH CHECK ADD  CONSTRAINT [FK_DomandaTitoloStudio] FOREIGN KEY([IdTitoloStudio])
REFERENCES [dbo].[TitoloStudio] ([Id])
GO
ALTER TABLE [dbo].[DomandaPartecipazione] CHECK CONSTRAINT [FK_DomandaTitoloStudio]
GO
ALTER TABLE [dbo].[DomandaPartecipazione]  WITH CHECK ADD  CONSTRAINT [FK_DomandaTitoloStudioEsperienze] FOREIGN KEY([IdTitoloStudioEsperienze])
REFERENCES [dbo].[TitoloStudio] ([Id])
GO
ALTER TABLE [dbo].[DomandaPartecipazione] CHECK CONSTRAINT [FK_DomandaTitoloStudioEsperienze]
GO
ALTER TABLE [dbo].[ProgettoPreferito]  WITH CHECK ADD  CONSTRAINT [FK_ProgettoPreferito] FOREIGN KEY([CodiceProgetto], [CodiceSede])
REFERENCES [dbo].[SUSCN_DOL_PROGETTI_DISPONIBILI] ([CodiceProgetto], [CodiceSede])
GO
ALTER TABLE [dbo].[ProgettoPreferito] CHECK CONSTRAINT [FK_ProgettoPreferito]
GO
ALTER TABLE [dbo].[Provincia]  WITH CHECK ADD  CONSTRAINT [FK_Provincia_Regione] FOREIGN KEY([idRegione])
REFERENCES [dbo].[Regione] ([Id])
GO
ALTER TABLE [dbo].[Provincia] CHECK CONSTRAINT [FK_Provincia_Regione]
GO
ALTER TABLE [dbo].[RequisitiGaranziaGiovani]  WITH CHECK ADD  CONSTRAINT [FK_RequisitiGaranziaGiovaniProvincia] FOREIGN KEY([IdProvincia])
REFERENCES [dbo].[Provincia] ([Id])
GO
ALTER TABLE [dbo].[RequisitiGaranziaGiovani] CHECK CONSTRAINT [FK_RequisitiGaranziaGiovaniProvincia]
GO
ALTER TABLE [dbo].[RequisitiGaranziaGiovani]  WITH CHECK ADD  CONSTRAINT [FK_RequisitiGaranziaGiovaniRegione] FOREIGN KEY([IdRegione])
REFERENCES [dbo].[Regione] ([Id])
GO
ALTER TABLE [dbo].[RequisitiGaranziaGiovani] CHECK CONSTRAINT [FK_RequisitiGaranziaGiovaniRegione]
GO
ALTER TABLE [dbo].[RichiestaCredenziali]  WITH CHECK ADD  CONSTRAINT [FK_RichiestaCredenziali_Genere] FOREIGN KEY([CodiceGenere])
REFERENCES [dbo].[Genere] ([Codice])
GO
ALTER TABLE [dbo].[RichiestaCredenziali] CHECK CONSTRAINT [FK_RichiestaCredenziali_Genere]
GO
ALTER TABLE [dbo].[RichiestaCredenziali]  WITH CHECK ADD  CONSTRAINT [FK_RichiestaCredenziali_NazioneCittadinanza] FOREIGN KEY([IdNazioneCittadinanza])
REFERENCES [dbo].[Nazione] ([Id])
GO
ALTER TABLE [dbo].[RichiestaCredenziali] CHECK CONSTRAINT [FK_RichiestaCredenziali_NazioneCittadinanza]
GO
ALTER TABLE [dbo].[RichiestaCredenziali]  WITH CHECK ADD  CONSTRAINT [FK_RichiestaCredenziali_NazioneNascita] FOREIGN KEY([IdNazioneNascita])
REFERENCES [dbo].[Nazione] ([Id])
GO
ALTER TABLE [dbo].[RichiestaCredenziali] CHECK CONSTRAINT [FK_RichiestaCredenziali_NazioneNascita]
GO
ALTER TABLE [dbo].[RichiestaCredenziali]  WITH CHECK ADD  CONSTRAINT [FK_RichiestaCredenziali_Stato] FOREIGN KEY([IdStato])
REFERENCES [dbo].[StatoRichiestaCredenziali] ([Id])
GO
ALTER TABLE [dbo].[RichiestaCredenziali] CHECK CONSTRAINT [FK_RichiestaCredenziali_Stato]
GO
ALTER TABLE [dbo].[SUSCN_DOL_OBIETTIVO_PROGRAMMA]  WITH CHECK ADD  CONSTRAINT [FK_ObiettivoProgamma_Programma] FOREIGN KEY([IdProgramma])
REFERENCES [dbo].[SUSCN_DOL_PROGRAMMA] ([IdProgramma])
GO
ALTER TABLE [dbo].[SUSCN_DOL_OBIETTIVO_PROGRAMMA] CHECK CONSTRAINT [FK_ObiettivoProgamma_Programma]
GO
ALTER TABLE [dbo].[SUSCN_DOL_OBIETTIVO_PROGRAMMA]  WITH CHECK ADD  CONSTRAINT [FK_ObiettivoProramma_Obiettivo] FOREIGN KEY([IdObiettivo])
REFERENCES [dbo].[SUSCN_DOL_OBIETTIVO] ([IdObiettivo])
GO
ALTER TABLE [dbo].[SUSCN_DOL_OBIETTIVO_PROGRAMMA] CHECK CONSTRAINT [FK_ObiettivoProramma_Obiettivo]
GO
ALTER TABLE [dbo].[SUSCN_DOL_PROGETTI_DISPONIBILI]  WITH CHECK ADD  CONSTRAINT [FK_GruppoProgetti] FOREIGN KEY([Gruppo])
REFERENCES [dbo].[SUSCN_DOL_BANDOGRUPPO] ([Gruppo])
GO
ALTER TABLE [dbo].[SUSCN_DOL_PROGETTI_DISPONIBILI] CHECK CONSTRAINT [FK_GruppoProgetti]
GO
ALTER TABLE [dbo].[SUSCN_DOL_PROGETTI_DISPONIBILI]  WITH CHECK ADD  CONSTRAINT [FK_Progetto_ParticolaritàEntità] FOREIGN KEY([IDParticolaritàEntità])
REFERENCES [dbo].[SUSCN_DOL_MINORE_OPPORTUNITA] ([IDParticolarità])
GO
ALTER TABLE [dbo].[SUSCN_DOL_PROGETTI_DISPONIBILI] CHECK CONSTRAINT [FK_Progetto_ParticolaritàEntità]
GO
ALTER TABLE [dbo].[SUSCN_DOL_PROGETTI_DISPONIBILI]  WITH CHECK ADD  CONSTRAINT [FK_Progetto_Programma] FOREIGN KEY([IdProgramma])
REFERENCES [dbo].[SUSCN_DOL_PROGRAMMA] ([IdProgramma])
GO
ALTER TABLE [dbo].[SUSCN_DOL_PROGETTI_DISPONIBILI] CHECK CONSTRAINT [FK_Progetto_Programma]
GO
ALTER TABLE [dbo].[SUSCN_DOL_PROGRAMMA]  WITH CHECK ADD  CONSTRAINT [FK_Programma_Garanzia_AMBITO] FOREIGN KEY([IdAmbitoAzione])
REFERENCES [dbo].[SUSCN_DOL_AMBITO] ([IdAmbitoAzione])
GO
ALTER TABLE [dbo].[SUSCN_DOL_PROGRAMMA] CHECK CONSTRAINT [FK_Programma_Garanzia_AMBITO]
GO
ALTER TABLE [dbo].[SUSCN_DOL_PROGRAMMA]  WITH CHECK ADD  CONSTRAINT [FK_Programma_Garanzia_Giovani] FOREIGN KEY([IdTipoGG])
REFERENCES [dbo].[SUSCN_DOL_GARANZIA_GIOVANI] ([IdTipoGG])
GO
ALTER TABLE [dbo].[SUSCN_DOL_PROGRAMMA] CHECK CONSTRAINT [FK_Programma_Garanzia_Giovani]
GO
