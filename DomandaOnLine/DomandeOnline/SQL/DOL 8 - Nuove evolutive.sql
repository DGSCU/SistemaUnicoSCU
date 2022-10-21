USE DomandeOnline
BEGIN TRY
BEGIN TRAN
	DROP TABLE  IF EXISTS  SUSCN_DOL_OBIETTIVO_PROGRAMMA
	DROP TABLE  IF EXISTS  SUSCN_DOL_AMBITO
	DROP TABLE  IF EXISTS  SUSCN_DOL_PROGRAMMA
	DROP TABLE  IF EXISTS  SUSCN_DOL_GARANZIA_GIOVANI
	DROP TABLE  IF EXISTS  SUSCN_DOL_OBIETTIVO
	DROP TABLE  IF EXISTS  RequisitiGaranziaGiovani
	
	ALTER TABLE DomandaPartecipazione DROP CONSTRAINT IF EXISTS FK_DomandaPartecipazione_MotivoAnnullamento
	ALTER TABLE DomandaPartecipazione DROP COLUMN IF EXISTS DataRichiestaAnnullamento
	ALTER TABLE DomandaPartecipazione DROP COLUMN IF EXISTS IdMotivazioneAnnullamento
	ALTER TABLE DomandaPartecipazione DROP COLUMN IF EXISTS FormazioneAnagraficaDisciplina
	ALTER TABLE DomandaPartecipazione DROP COLUMN IF EXISTS FormazioneAnagraficaAnno
	ALTER TABLE DomandaPartecipazione DROP COLUMN IF EXISTS FormazioneAnagraficaItalia
	ALTER TABLE DomandaPartecipazione DROP COLUMN IF EXISTS FormazioneAnagraficaIstituto
	ALTER TABLE DomandaPartecipazione DROP COLUMN IF EXISTS FormazioneAnagraficaEnte
	ALTER TABLE SUSCN_DOL_PROGETTI_DISPONIBILI DROP COLUMN IF EXISTS IDParticolaritàEntità
	ALTER TABLE SUSCN_DOL_PROGETTI_DISPONIBILI DROP CONSTRAINT IF EXISTS FK_Progetto_Programma
	ALTER TABLE SUSCN_DOL_PROGETTI_DISPONIBILI DROP COLUMN IF EXISTS IDProgramma
	ALTER TABLE SUSCN_DOL_PROGETTI_DISPONIBILI DROP COLUMN IF EXISTS IdTipoGG
	ALTER TABLE SUSCN_DOL_CODICE_FISCALE_VOLONTARIO DROP CONSTRAINT IF EXISTS PK_SUSCN_DOL_CODICE_FISCALE_VOLONTARIO
	ALTER TABLE SUSCN_DOL_CODICE_FISCALE_VOLONTARIO DROP COLUMN IF EXISTS TipoServizio


	DROP TABLE  IF EXISTS  MotivoAnnullamento
	
	CREATE TABLE SUSCN_DOL_AMBITO(
		IdAmbitoAzione int PRIMARY KEY NOT NULL,
		Descrizione nvarchar(255) NOT NULL
	)

	CREATE TABLE SUSCN_DOL_GARANZIA_GIOVANI(
		IdTipoGG int NOT NULL PRIMARY KEY,
		Regione varchar(50) NOT NULL,
		Descrizione varchar(100) NOT NULL
	)
	CREATE TABLE 
		SUSCN_DOL_PROGRAMMA(
		IdProgramma INT PRIMARY KEY,
		Titolo NVARCHAR(255) NOT NULL,
		IdAmbitoAzione INT NOT NULL,
		IdTipoGG INT NULL
			CONSTRAINT FK_Programma_Garanzia_Giovani FOREIGN KEY REFERENCES SUSCN_DOL_GARANZIA_GIOVANI(IdTipoGG)
	)

	CREATE TABLE SUSCN_DOL_OBIETTIVO(
		IdObiettivo int PRIMARY KEY NOT NULL,
		Obiettivo nvarchar(50) NULL,
		Descrizione nvarchar(255) NOT NULL
	)

	CREATE TABLE SUSCN_DOL_OBIETTIVO_PROGRAMMA(
		IdProgramma int NOT NULL,
		IdObiettivo int NOT NULL,
		CONSTRAINT FK_ObiettivoProgamma_Programma FOREIGN KEY (IdProgramma) REFERENCES SUSCN_DOL_PROGRAMMA(IdProgramma),
		CONSTRAINT FK_ObiettivoProramma_Obiettivo FOREIGN KEY (IdObiettivo) REFERENCES SUSCN_DOL_OBIETTIVO(IdObiettivo),
	)


	DROP TABLE  IF EXISTS  SUSCN_DOL_MINORE_OPPORTUNITA
	CREATE TABLE SUSCN_DOL_MINORE_OPPORTUNITA(
		IDParticolarità int NOT NULL,
		Descrizione varchar(100) NOT NULL
	)

	CREATE Table MotivoAnnullamento (
		Id INT PRIMARY Key IDENTITY (1,1),
		Descrizione NVARCHAR(100) NOT NULL,
		Selezionabile Bit NOT NULL
	)

	INSERT INTO MotivoAnnullamento VALUES
	('Cancellazione Progetto',0),
	('Motivazione 1',1),
	('Motivazione 2',1),
	('Motivazione 3',1),
	('Motivazione 4',1),
	('Motivazione 5',1)
	
	ALTER TABLE DomandaPartecipazione
	ADD 
		DataRichiestaAnnullamento DATETIME,
		IdMotivazioneAnnullamento INT NULL
			CONSTRAINT FK_DomandaPartecipazione_MotivoAnnullamento FOREIGN KEY REFERENCES MotivoAnnullamento(Id),
		FormazioneAnagraficaDisciplina NVARCHAR(200) NULL,
		FormazioneAnagraficaAnno INT NULL,
		FormazioneAnagraficaItalia BIT NULL,
		FormazioneAnagraficaIstituto NVARCHAR(200) NULL,
		FormazioneAnagraficaEnte NVARCHAR(200) NULL,
		DichiarazioneResidenzaOK bit NULL,
		DichiarazioneRequisitiGaranziaGiovani bit NULL,
		DataPresaInCaricoGaranziaGiovani DATE NULL,
		LuogoPresaInCaricoGaranziaGiovani NVARCHAR(200) NULL,
		DataDIDGaranziaGiovani DATE NULL,
		LuogoDIDGaranziaGiovani NVARCHAR(200) NULL,
		AlternativaRequisitiGaranziaGiovani bit NULL


	ALTER TABLE SUSCN_DOL_PROGETTI_DISPONIBILI
	ADD 
		IDParticolaritàEntità INT NULL,
		IdProgramma INT NULL
			CONSTRAINT FK_Progetto_Programma FOREIGN KEY REFERENCES SUSCN_DOL_PROGRAMMA(IdProgramma),
		IdTipoGG INT NULL

	ALTER TABLE SUSCN_DOL_CODICE_FISCALE_VOLONTARIO
	ADD
		TipoServizio VARCHAR(2) NOT NULL DEFAULT 'SC',
		CONSTRAINT PK_SUSCN_DOL_CODICE_FISCALE_VOLONTARIO PRIMARY KEY (CodiceFiscale,TipoServizio)

	CREATE TABLE SUSCN_DOL_CODICE_FISCALE_VOLONTARIO(
		CodiceFiscale NVARCHAR(30),
		DataInizioServizio DATETIME,
		DataFineServizio DATETIME,
		TipoServizio varchar(2),
		CONSTRAINT PK_SUSCN_DOL_CODICE_FISCALE_VOLONTARIO PRIMARY KEY (CodiceFiscale,DataInizioServizio,DataFineServizio,TipoServizio)
	)


	CREATE TABLE RequisitiGaranziaGiovani(
		TipoGG varchar(100) NOT NULL PRIMARY KEY,
		IdRegione INT NULL
			CONSTRAINT FK_RequisitiGaranziaGiovaniRegione REFERENCES Regione(Id),
		IdProvincia INT NULL
			CONSTRAINT FK_RequisitiGaranziaGiovaniProvincia REFERENCES Provincia(Id),
	)
	INSERT INTO RequisitiGaranziaGiovani VALUES
		('SC Italia - UE / Giovani disoccupati',13,NULL),
		('SC Italia - UE / Giovani disoccupati',14,NULL),
		('SC Italia - UE / Giovani disoccupati',15,NULL),
		('SC Italia - UE / Giovani disoccupati',16,NULL),
		('SC Italia - UE / Giovani disoccupati',17,NULL),
		('SC Italia - UE / Giovani disoccupati',18,NULL),
		('SC Italia - UE / Giovani disoccupati',19,NULL),
		('SC Italia - UE / Giovani disoccupati',20,NULL),
		('SC Italia / Giovani disoccupati',13,NULL),
		('SC Italia / Giovani disoccupati',14,NULL),
		('SC Italia / Giovani disoccupati',15,NULL),
		('SC Italia / Giovani disoccupati',16,NULL),
		('SC Italia / Giovani disoccupati',17,NULL),
		('SC Italia / Giovani disoccupati',18,NULL),
		('SC Italia / Giovani disoccupati',19,NULL),
		('SC Italia / Giovani disoccupati',20,NULL),
		('SC Italia - UE / Giovani NEET',NULL,21),
		('SC Italia / Giovani NEET',NULL,21)

	ROLLBACK

END TRY

	BEGIN CATCH
		IF(@@TRANCOUNT > 0)
			ROLLBACK TRAN;
		THROW
	END CATCH