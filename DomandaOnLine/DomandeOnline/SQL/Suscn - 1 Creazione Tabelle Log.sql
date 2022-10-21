USE unscproduzione

IF OBJECT_ID('JobSchedule', 'U') IS NOT NULL DROP TABLE JobSchedule
IF OBJECT_ID('JobExecution', 'U') IS NOT NULL DROP TABLE JobExecution
IF OBJECT_ID('Job', 'U') IS NOT NULL DROP TABLE Job
IF OBJECT_ID('LogScheduleAgent', 'U') IS NOT NULL DROP TABLE LogScheduleAgent

CREATE TABLE dbo.Job(
	Id int IDENTITY(1,1) NOT NULL,
	Nome nvarchar(30) NOT NULL,
	Descrizione nvarchar(max) NULL,
 CONSTRAINT PK_Job PRIMARY KEY CLUSTERED (Id ASC)
)
SET IDENTITY_INSERT Job ON
INSERT INTO Job (Id,Nome,Descrizione) VALUES
(1,'Domanda Online','Processo di sincronizzazione dati di Domanda Online.')
SET IDENTITY_INSERT Job OFF

CREATE TABLE dbo.JobSchedule(
	Id int IDENTITY(1,1) NOT NULL,
	IdJob int NOT NULL,
	OraInizio time NOT NULL,
	GiornoSettimana int NULL,
	CONSTRAINT PK_JobSchedule PRIMARY KEY CLUSTERED (Id ASC),
	CONSTRAINT FK_JobSchedule FOREIGN KEY (IdJob) REFERENCES Job(Id) 
)
INSERT INTO JobSchedule (IdJob,OraInizio) VALUES
(1,'02:00')

CREATE TABLE dbo.JobExecution(
	Id int IDENTITY(1,1) NOT NULL,
	IdJob int NOT NULL,
	DataInizioEsecuzione datetime NOT NULL,
	DataFineEsecuzione datetime NULL,
	Esito nvarchar(max) NULL,

	CONSTRAINT PK_JobExecution PRIMARY KEY CLUSTERED (Id ASC),
	CONSTRAINT FK_JobExecution FOREIGN KEY (IdJob) REFERENCES Job(Id) 
)
CREATE TABLE [dbo].[LogScheduleAgent](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Message] [nvarchar](max) NULL,
	[MessageTemplate] [nvarchar](max) NULL,
	[Level] [nvarchar](128) NULL,
	[TimeStamp] [datetime] NOT NULL,
	[Exception] [nvarchar](max) NULL,
	[Properties] [nvarchar](max) NULL,
 CONSTRAINT [PK_LogScheduleAgent] PRIMARY KEY CLUSTERED ([Id] ASC)
)