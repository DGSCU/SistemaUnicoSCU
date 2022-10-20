USE [Registrazione]
GO
/****** Object:  Table [dbo].[CategoriaEnte]    Script Date: 12/08/2022 12:50:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CategoriaEnte](
	[Id] [int] NOT NULL,
	[Descrizione] [nvarchar](200) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Configurazione]    Script Date: 12/08/2022 12:50:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Configurazione](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Parametro] [varchar](50) NULL,
	[Descrizione] [nvarchar](255) NULL,
	[Valore] [varchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Documento]    Script Date: 12/08/2022 12:50:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Documento](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[NomeFile] [nvarchar](255) NOT NULL,
	[Dimensione] [int] NOT NULL,
	[MimeType] [nvarchar](255) NOT NULL,
	[Hash] [nvarchar](255) NOT NULL,
	[Blob] [varbinary](max) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Registrazione]    Script Date: 12/08/2022 12:50:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Registrazione](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[DataInserimento] [datetime] NOT NULL,
	[CodiceFiscaleEnte] [varchar](50) NOT NULL,
	[CodiceFiscaleRappresentanteLegale] [varchar](50) NOT NULL,
	[Denominazione] [nvarchar](200) NOT NULL,
	[DataNominaRappresentanteLegale] [date] NOT NULL,
	[EnteTitolare] [bit] NULL,
	[IdCategoriaEnte] [int] NULL,
	[IdTipologiaEnte] [int] NULL,
	[IdProvinciaEnte] [int] NULL,
	[IdComuneEnte] [int] NULL,
	[Via] [nvarchar](255) NULL,
	[Civico] [nvarchar](50) NULL,
	[CAP] [nvarchar](10) NULL,
	[Telefono] [varchar](70) NULL,
	[Email] [varchar](100) NULL,
	[PEC] [varchar](100) NULL,
	[Sito] [nvarchar](100) NULL,
	[DichiarazionePrivacy] [bit] NULL,
	[DichiarazioneRappresentanteLegale] [bit] NULL,
	[IdDocumento] [int] NULL,
	[IdDocumentoNomina] [int] NULL,
	[VariazioneRappresentanteLegale] [bit] NULL,
	[DataProtocollazione] [datetime] NULL,
	[NumeroProtocollo] [nvarchar](20) NULL,
	[DataProtocollo] [date] NULL,
	[DataInvioEmail] [datetime] NULL,
	[Albo] [varchar](3) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Soggetto]    Script Date: 12/08/2022 12:50:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Soggetto](
	[CodiceFiscale] [nvarchar](50) NOT NULL,
	[Nome] [nvarchar](255) NULL,
	[Cognome] [nvarchar](255) NULL,
PRIMARY KEY CLUSTERED 
(
	[CodiceFiscale] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[StatoRegistrazione]    Script Date: 12/08/2022 12:50:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[StatoRegistrazione](
	[Id] [int] NOT NULL,
	[Descrizione] [nvarchar](200) NOT NULL,
	[Abilitato] [bit] NOT NULL,
	[Annullato] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TokenAccesso]    Script Date: 12/08/2022 12:50:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TokenAccesso](
	[Id] [uniqueidentifier] NOT NULL,
	[DataScadenza] [datetime] NOT NULL,
	[CodiceFiscale] [varchar](50) NOT NULL,
	[Username] [nvarchar](10) NOT NULL,
	[CodiceFiscaleEnte] [varchar](50) NOT NULL,
	[Albo] [varchar](10) NOT NULL,
	[Utilizzato] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[TokenAccesso] ADD  DEFAULT (newid()) FOR [Id]
GO
ALTER TABLE [dbo].[Registrazione]  WITH CHECK ADD FOREIGN KEY([IdCategoriaEnte])
REFERENCES [dbo].[CategoriaEnte] ([Id])
GO
ALTER TABLE [dbo].[Registrazione]  WITH CHECK ADD FOREIGN KEY([IdDocumento])
REFERENCES [dbo].[Documento] ([Id])
GO
ALTER TABLE [dbo].[Registrazione]  WITH CHECK ADD FOREIGN KEY([IdDocumentoNomina])
REFERENCES [dbo].[Documento] ([Id])
GO
