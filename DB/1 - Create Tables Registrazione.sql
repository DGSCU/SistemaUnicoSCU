USE Registrazione



CREATE TABLE Documento(
	Id INT PRIMARY KEY IDENTITY(1,1),
	NomeFile nvarchar(255) NOT NULL,
	Dimensione int NOT NULL,
	MimeType nvarchar(255) NOT NULL,
	Hash nvarchar(255) NOT NULL,
	Blob varbinary(MAX) NOT NULL,
)
CREATE TABLE CategoriaEnte(
	Id INT PRIMARY KEY,
	Descrizione nvarchar(200) NOT NULL,
)

CREATE TABLE StatoRegistrazione(
	Id INT PRIMARY KEY,
	Descrizione nvarchar(200) NOT NULL,
	Abilitato bit NOT NULL,
	Annullato bit NOT NULL
)

CREATE TABLE Registrazione(
	Id INT PRIMARY KEY IDENTITY(1,1),
	DataInserimento DATETIME NOT NULL,
	CodiceFiscaleEnte varchar(50) NOT NULL,
	CodiceFiscaleRappresentanteLegale varchar(50) NOT NULL,
	Denominazione nvarchar(200) NOT NULL,
	DataNominaRappresentanteLegale DATE NOT NULL,
	EnteTitolare bit NULL,
	IdCategoriaEnte int FOREIGN KEY REFERENCES CategoriaEnte (Id) NULL,
	IdTipologiaEnte int NULL,
	IdProvinciaEnte int NULL,
	IdComuneEnte int NULL,
	Via nvarchar(255) NULL,
	Civico nvarchar(50)  NULL,
	CAP nvarchar(10) NULL,
	Telefono varchar(70) NULL,
	Email varchar(100) NULL,
	PEC varchar(100) NULL,
	Sito nvarchar(100) NULL,
	DichiarazionePrivacy BIT NULL,
	DichiarazioneRappresentanteLegale BIT NULL,
	IdDocumento int FOREIGN KEY REFERENCES Documento (Id) NULL,
	IdDocumentoNomina int FOREIGN KEY REFERENCES Documento (Id) NULL,
	VariazioneRappresentanteLegale bit NULL,
	DataProtocollazione DATETIME NULL,
	NumeroProtocollo NVARCHAR(20) NULL,
	DataProtocollo DATE NULL,
	DataInvioEmail DATETIME NULL
)

CREATE TABLE TokenAccesso(
	Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
	DataScadenza DATETIME NOT NULL,
	CodiceFiscale varchar(50) NOT NULL,
	Username nvarchar(10) NOT NULL,
	CodiceFiscaleEnte varchar(50) NOT NULL,
	Albo varchar(10) NOT NULL,
	Utilizzato bit NOT NULL
)


INSERT INTO StatoRegistrazione VALUES
(1,'Inserita',0,0),
(2,'Da approvare',0,0),
(3,'Approvata',1,0),
(4,'Rifiutata',0,1)

INSERT INTO CategoriaEnte VALUES
(1,'Privato'),
(2,'Pubblico')

