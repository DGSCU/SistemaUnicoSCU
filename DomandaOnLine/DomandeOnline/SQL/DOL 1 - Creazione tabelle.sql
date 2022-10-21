BEGIN TRAN CreaTabelle
BEGIN TRY
	IF OBJECT_ID('Comune', 'U') IS NOT NULL DROP TABLE Comune
	IF OBJECT_ID('Provincia', 'U') IS NOT NULL DROP TABLE Provincia
	IF OBJECT_ID('Regione', 'U') IS NOT NULL DROP TABLE Regione
	IF OBJECT_ID('CodiceBelfiore', 'U') IS NOT NULL DROP TABLE CodiceBelfiore
	IF OBJECT_ID('RichiestaCredenziali', 'U') IS NOT NULL DROP TABLE RichiestaCredenziali
	IF OBJECT_ID('StatoRichiestaCredenziali', 'U') IS NOT NULL DROP TABLE StatoRichiestaCredenziali
	IF OBJECT_ID('Genere', 'U') IS NOT NULL DROP TABLE Genere
	IF OBJECT_ID('Nazione', 'U') IS NOT NULL DROP TABLE Nazione
	IF OBJECT_ID('Configurazione', 'U') IS NOT NULL DROP TABLE Configurazione
	IF OBJECT_ID('TokenCredenziali', 'U') IS NOT NULL DROP TABLE TokenCredenziali
	IF OBJECT_ID('ProgettoPreferito', 'U') IS NOT NULL DROP TABLE ProgettoPreferito
	IF OBJECT_ID('DomandaPartecipazione', 'U') IS NOT NULL DROP TABLE DomandaPartecipazione
	IF OBJECT_ID('SUSCN_DOL_PROGETTI_DISPONIBILI', 'U') IS NOT NULL DROP TABLE SUSCN_DOL_PROGETTI_DISPONIBILI
	IF OBJECT_ID('SUSCN_DOL_GEOGRAFICO_ITALIA', 'U') IS NOT NULL DROP TABLE SUSCN_DOL_GEOGRAFICO_ITALIA
	IF OBJECT_ID('SUSCN_DOL_BANDOGRUPPO', 'U') IS NOT NULL DROP TABLE SUSCN_DOL_BANDOGRUPPO
	IF OBJECT_ID('SUSCN_DOL_CODICE_FISCALE_VOLONTARIO', 'U') IS NOT NULL DROP TABLE SUSCN_DOL_CODICE_FISCALE_VOLONTARIO
	IF OBJECT_ID('Log', 'U') IS NOT NULL DROP TABLE Log
	IF OBJECT_ID('TitoloStudio', 'U') IS NOT NULL DROP TABLE TitoloStudio
	IF OBJECT_ID('Specializzazione', 'U') IS NOT NULL DROP TABLE Specializzazione
	IF OBJECT_ID('Motivazione', 'U') IS NOT NULL DROP TABLE Motivazione
	IF OBJECT_ID('DbVersion', 'U') IS NOT NULL DROP TABLE DbVersion
	IF OBJECT_ID('AspNetUserRoles', 'U') IS NOT NULL DROP TABLE AspNetUserRoles
	IF OBJECT_ID('AspNetRoles', 'U') IS NOT NULL DROP TABLE AspNetRoles	
	IF OBJECT_ID('AspNetUserClaims', 'U') IS NOT NULL DROP TABLE AspNetUserClaims
	IF OBJECT_ID('AspNetUserLogins', 'U') IS NOT NULL DROP TABLE AspNetUserLogins
	IF OBJECT_ID('AspNetUsers', 'U') IS NOT NULL DROP TABLE AspNetUsers
	IF OBJECT_ID('__MigrationHistory', 'U') IS NOT NULL DROP TABLE __MigrationHistory
	
	
	CREATE TABLE Nazione(
		Id int PRIMARY KEY IDENTITY(1,1),
		Nome nvarchar(50),
		CodiceISO2 nvarchar(2),
		CodiceISO3 nvarchar(3)
	)

	CREATE TABLE Regione(
		Id int PRIMARY KEY IDENTITY(1,1),
		Nome nvarchar(50)
	)

	CREATE TABLE Provincia(
		Id int PRIMARY KEY IDENTITY(1,1),
		Nome nvarchar(50),
		Sigla nvarchar(2),
		idRegione int
		CONSTRAINT FK_Provincia_Regione FOREIGN KEY (idRegione) REFERENCES Regione(Id) 
	)

	CREATE TABLE Comune(
		Id int PRIMARY KEY IDENTITY(1,1),
		Nome nvarchar(50),
		CodiceCatastale nvarchar(4),
		IdPRovincia int,
		CONSTRAINT FK_Comune_Provincia FOREIGN KEY (idProvincia) REFERENCES Provincia(Id) 
	)

	CREATE TABLE Genere(
		Codice nvarchar(1) PRIMARY KEY,
		Nome nvarchar(20) NOT NULL,
	) 

	CREATE TABLE StatoRichiestaCredenziali(
		Id int PRIMARY KEY IDENTITY(1,1),
		Nome nvarchar(50),
		Descrizione nvarchar(MAX)
	)


	CREATE TABLE RichiestaCredenziali(
		Id int PRIMARY KEY IDENTITY(1,1),
		DataRichiesta DateTime,
		IdStato int,
		DataApprovazione DateTime,
		UtenteApprovazione nvarchar(256),
		NoteApprovazione nvarchar(MAX),
		DataAnnullamento DateTime,
		UtenteAnnullamento nvarchar(256),
		NoteAnnullamento nvarchar(MAX),
		Nome nvarchar(100),
		Cognome nvarchar(100),
		CodiceGenere nvarchar(1),
		DataNascita Date,
		LuogoNascita nvarchar(200),
		IdNazioneNascita int,
		CodiceFiscale nvarchar(30),
		IdNazioneCittadinanza int,
		Email nvarchar(100),
		Telefono nvarchar(20),
		Allegato varbinary(MAX)

		CONSTRAINT FK_RichiestaCredenziali_Stato FOREIGN KEY (IdStato) REFERENCES StatoRichiestaCredenziali(Id),
		CONSTRAINT FK_RichiestaCredenziali_NazioneNascita FOREIGN KEY (IdNazioneNascita) REFERENCES Nazione(Id) ,
		CONSTRAINT FK_RichiestaCredenziali_NazioneCittadinanza FOREIGN KEY (IdNazioneCittadinanza) REFERENCES Nazione(Id),
		CONSTRAINT FK_RichiestaCredenziali_Genere FOREIGN KEY (CodiceGenere) REFERENCES Genere(Codice) 
	)

	CREATE TABLE Configurazione(
		Nome nvarchar(50) PRIMARY KEY,
		Valore nvarchar(200) NOT NULL,
		Descrizione nvarchar(MAX)
	)

	CREATE TABLE TokenCredenziali(
		Id uniqueidentifier DEFAULT NEWID() PRIMARY KEY ,
		code nvarchar(MAX) NOT NULL,
		UserId nvarchar(128) NOT NULL,
		DataGenerazione DateTime NOT NULL,
		Scadenza DateTime NOT NULL,
		DataUtilizzo DateTime
	)

	CREATE TABLE SUSCN_DOL_BANDOGRUPPO(
		Gruppo int NOT NULL PRIMARY KEY,
		Descrizione nvarchar(200) NOT NULL,
		Scadenza nvarchar(50) NOT NULL,
		DataScadenza datetime NOT NULL,
		DataScadenzaGraduatorie datetime NULL
	)

	CREATE TABLE SUSCN_DOL_PROGETTI_DISPONIBILI(
		CodiceEnte nvarchar(10) NULL,
		NomeEnte nvarchar(200) NOT NULL,
		Sito nvarchar(100) NULL,
		CodiceProgetto nvarchar(22) NOT NULL,
		TitoloProgetto nvarchar(255) NOT NULL,
		TipoProgetto nvarchar(6) NOT NULL,
		CodiceSede int NOT NULL,
		IndirizzoSede nvarchar(306) NULL,
		Regione nvarchar(50) NOT NULL,
		Provincia nvarchar(50) NOT NULL,
		Comune nvarchar(100) NOT NULL,
		Settore nvarchar(100) NOT NULL,
		Area nvarchar(150) NOT NULL,
		NumeroPostiDisponibili smallint NULL,
		Gruppo int NOT NULL,
		Misure varchar(2) NOT NULL,
		DurataProgettoMesi tinyint NOT NULL,
		NumeroGiovaniMinoriOpportunità smallint NOT NULL,
		EsteroUE nvarchar(2) NOT NULL,
		Tutoraggio nvarchar(2) NOT NULL,
		DataAnnullamento datetime NULL,

		CONSTRAINT PK_SUSCN_DOL_PROGETTI_DISPONIBILI PRIMARY KEY NONCLUSTERED (CodiceProgetto, CodiceSede),
		CONSTRAINT FK_GruppoProgetti FOREIGN KEY (Gruppo) REFERENCES SUSCN_DOL_BANDOGRUPPO(Gruppo)
	)

	CREATE TABLE SUSCN_DOL_GEOGRAFICO_ITALIA(
		REGIONE [nvarchar](50) NOT NULL,
		PROVINCIA [nvarchar](50) NOT NULL,
		COMUNE [nvarchar](100) NOT NULL
		CONSTRAINT PK_SUSCN_DOL_GEOGRAFICO_ITALIA PRIMARY KEY NONCLUSTERED (REGIONE, PROVINCIA, COMUNE)
	)

	CREATE TABLE SUSCN_DOL_CODICE_FISCALE_VOLONTARIO(
		CodiceFiscale nvarchar(30),
		DataInizioServizio datetime,
		DataFineServizio datetime,
		CONSTRAINT PK_SUSCN_DOL_CODICE_FISCALE_VOLONTARIO PRIMARY KEY NONCLUSTERED (CodiceFiscale, DataInizioServizio, DataFineServizio)
	)


	CREATE TABLE ProgettoPreferito(
		CodiceFiscale nvarchar(30) NOT NULL,
		CodiceProgetto nvarchar(22) NOT NULL,
		CodiceSede int NOT NULL,
		CONSTRAINT PK_ProgettoPreferito PRIMARY KEY NONCLUSTERED (CodiceFiscale,CodiceProgetto, CodiceSede),
		CONSTRAINT FK_ProgettoPreferito FOREIGN KEY (codiceProgetto,CodiceSede) REFERENCES SUSCN_DOL_PROGETTI_DISPONIBILI(codiceProgetto,CodiceSede)
	)

	CREATE TABLE TitoloStudio(
		Id int PRIMARY KEY IDENTITY(1,1),
		Descrizione nvarchar(100) NOT NULL
	) 

	CREATE TABLE Specializzazione(
		Id int PRIMARY KEY IDENTITY(1,1),
		Descrizione nvarchar(100) NOT NULL
	) 

	CREATE TABLE Motivazione(
		Id int PRIMARY KEY IDENTITY(1,1),
		Descrizione nvarchar(100) NOT NULL
	)

	CREATE TABLE DomandaPartecipazione(
		Id int PRIMARY KEY IDENTITY(1,1),
		CodiceFiscale nvarchar(30) NOT NULL,
		GruppoBando int NOT NULL,
		UserIdInserimento nvarchar(128) NOT NULL,
		DataInserimento datetime NOT NULL,
		UserIdModifica nvarchar(128),
		DataModifica datetime,
		CodiceProgettoSelezionato nvarchar(22),
		CodiceSedeSelezionata int,
		UserIdPresentazione nvarchar(128),
		DataPresentazione datetime,
		DataAnnullamento datetime,
		AllegatoCV varbinary(MAX),
		NomeFileCV nvarchar(200),
		Nome nvarchar(100),
		Cognome nvarchar(100),
		Genere nvarchar(7),
		DataNascita Date,
		LuogoNascita nvarchar(200),
		NazioneNascita nvarchar(200),
		Cittadinanza nvarchar(200),
		Telefono nvarchar(200),
		Email nvarchar(200),
		ComuneResidenza nvarchar(200),
		ProvinciaResidenza nvarchar(200),
		ViaResidenza nvarchar(200),
		CivicoResidenza nvarchar(200),
		CapResidenza nvarchar(200),
		ComuneRecapito nvarchar(200),
		ProvinciaRecapito nvarchar(200),
		ViaRecapito nvarchar(200),
		CivicoRecapito nvarchar(200),
		CapRecapito nvarchar(200),
		CodiceMinoriOpportunita nvarchar(5),
		IdMotivazione int,
		CodiceDichiarazioneCittadinanza nvarchar(5),
		NonCondanneOk bit,
		TrasferimentoSedeOk bit,
		TrasferimentoProgettoOk bit,
		AltreDichiarazioniOk bit,
		IdTitoloStudio int NULL,
		PrivacyPresaVisione bit,
		PrivacyConsenso bit,
		PrecedentiEnte bit,
		PrecedentiEnteDescrizione nvarchar(1000),
		PrecedentiAltriEnti bit,
		PrecedentiAltriEntiDescrizione nvarchar(1000),
		PrecedentiImpiego bit,
		PrecedentiImpiegoDescrizione nvarchar(1000),
		IdTitoloStudioEsperienze int NULL,
		FormazioneDisciplina nvarchar(200),
		FormazioneAnno int,
		FormazioneData Date,
		FormazioneItalia bit,
		FormazioneIstituto nvarchar(200),
		FormazioneEnte nvarchar(200),
		IscrizioneSuperioreAnno int,
		IscrizioneSuperioreIstituto nvarchar(200),
		IscrizioneLaureaAnno int,
		IscrizioneLaureaCorso nvarchar(200),
		IscrizioneLaureaIstituto nvarchar(200),
		CorsiEffettuati nvarchar(1000),
		Specializzazioni nvarchar(1000),
		Competenze nvarchar(1000),
		Altro nvarchar(1000),
		FileDomanda varbinary(MAX),
		DataInvioEmailAnnullamento datetime,

		CONSTRAINT FK_DomandaProgetto FOREIGN KEY (CodiceProgettoSelezionato,CodiceSedeSelezionata) REFERENCES SUSCN_DOL_PROGETTI_DISPONIBILI(codiceProgetto,CodiceSede),
		CONSTRAINT FK_DomandaTitoloStudio FOREIGN KEY (IdTitoloStudio) REFERENCES TitoloStudio(id),
		CONSTRAINT FK_DomandaTitoloStudioEsperienze FOREIGN KEY (IdTitoloStudioEsperienze) REFERENCES TitoloStudio(id),
		CONSTRAINT FK_DomandaMotivazione FOREIGN KEY (IdMotivazione) REFERENCES Motivazione(Id)
	)

	CREATE TABLE Log (
	   Id int IDENTITY(1,1) NOT NULL,
	   Message nvarchar(max) NULL,
	   MessageTemplate nvarchar(max) NULL,
	   Level nvarchar(128) NULL,
	   TimeStamp datetime NOT NULL,
	   Exception nvarchar(max) NULL,
	   Properties nvarchar(max) NULL

	   CONSTRAINT PK_Log PRIMARY KEY CLUSTERED (Id ASC) 
	);

	CREATE TABLE CodiceBelfiore (
	   Codice nvarchar(4) NOT NULL,
	   Nazione varchar(50) NOT NULL,
	   Comune varchar(50),
	   CONSTRAINT PK_CodiceBelfiore PRIMARY KEY CLUSTERED (Codice) 
	);

	CREATE TABLE DbVersion (
		Version INT
	)

	COMMIT TRAN CreaTabelle
END TRY
BEGIN CATCH
	PRINT ERROR_MESSAGE ( )  
	IF @@TRANCOUNT>0
		ROLLBACK TRAN CreaTabelle
END CATCH