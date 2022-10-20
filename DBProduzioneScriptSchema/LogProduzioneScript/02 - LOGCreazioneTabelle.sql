USE [LOG]
GO
/****** Object:  Table [dbo].[DGSCULogs]    Script Date: 12/08/2022 16:01:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DGSCULogs](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Message] [nvarchar](max) NULL,
	[MessageTemplate] [nvarchar](max) NULL,
	[Level] [nvarchar](max) NULL,
	[TimeStamp] [datetime] NULL,
	[Exception] [nvarchar](max) NULL,
	[Properties] [nvarchar](max) NULL,
	[Username] [varchar](50) NULL,
	[IPAddress] [varchar](20) NULL,
	[ApplicationName] [varchar](100) NULL,
 CONSTRAINT [PK_DGSCULogs] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Log]    Script Date: 12/08/2022 16:01:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Log](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[TimeStamp] [datetime] NOT NULL,
	[StartTime] [datetime] NULL,
	[Duration] [int] NULL,
	[IdLevel] [int] NOT NULL,
	[IpAddress] [varchar](20) NULL,
	[Username] [varchar](50) NULL,
	[Name] [varchar](200) NULL,
	[Ente] [varchar](200) NULL,
	[CodiceFiscaleEnte] [varchar](200) NULL,
	[IdEventType] [int] NULL,
	[EntityId] [int] NULL,
	[EntityName] [varchar](100) NULL,
	[ApplicationName] [varchar](100) NULL,
	[Action] [varchar](100) NULL,
	[Controller] [varchar](100) NULL,
	[Method] [varchar](20) NULL,
	[Message] [varchar](max) NULL,
	[Exception] [varchar](max) NULL,
	[Parameter] [varchar](max) NULL,
	[SessionId] [varchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LogEvent]    Script Date: 12/08/2022 16:01:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LogEvent](
	[Id] [int] NOT NULL,
	[Descrizione] [nvarchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LogLevel]    Script Date: 12/08/2022 16:01:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LogLevel](
	[Id] [int] NOT NULL,
	[Descrizione] [nvarchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[Log]  WITH CHECK ADD FOREIGN KEY([IdEventType])
REFERENCES [dbo].[LogEvent] ([Id])
GO
ALTER TABLE [dbo].[Log]  WITH CHECK ADD FOREIGN KEY([IdLevel])
REFERENCES [dbo].[LogLevel] ([Id])
GO
