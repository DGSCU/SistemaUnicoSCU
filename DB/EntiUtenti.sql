USE [unscsviluppo]
GO

/****** Object:  Table [dbo].[EntiUtenti]    Script Date: 07/12/2021 11:25:33 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[EntiUtenti](
	[idEnteUtente] [int] IDENTITY(1,1) NOT NULL,
	[IDEnte] [int] NOT NULL,
	[CodiceFiscale] [nvarchar](50) NOT NULL,
	[Cognome] [nvarchar](50) NULL,
	[Nome] [nvarchar](50) NULL,
	[Stato] [bit] NULL,
	[Visibile] [bit] NULL
) ON [PRIMARY]
GO

