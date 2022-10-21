--USE DomandeOnline
BEGIN TRY
BEGIN TRAN

	IF COL_LENGTH('dbo.SUSCN_DOL_PROGETTI_DISPONIBILI', 'IDParticolaritaEntità') IS NOT NULL
		ALTER TABLE SUSCN_DOL_PROGETTI_DISPONIBILI DROP COLUMN IDParticolaritàEntita
	IF OBJECT_ID('FK_Progetto_Programma') IS NOT NULL 
		ALTER TABLE SUSCN_DOL_PROGETTI_DISPONIBILI DROP CONSTRAINT FK_Progetto_Programma
	IF COL_LENGTH('dbo.SUSCN_DOL_PROGETTI_DISPONIBILI', 'IDProgramma') IS NOT NULL
		ALTER TABLE SUSCN_DOL_PROGETTI_DISPONIBILI DROP COLUMN IDProgramma
	IF COL_LENGTH('dbo.SUSCN_DOL_PROGETTI_DISPONIBILI', 'IdTipoGG') IS NOT NULL
		ALTER TABLE SUSCN_DOL_PROGETTI_DISPONIBILI DROP COLUMN IdTipoGG
	IF COL_LENGTH('dbo.SUSCN_DOL_PROGETTI_DISPONIBILI', 'DichiarazioneResidenzaOK') IS NOT NULL
		ALTER TABLE SUSCN_DOL_PROGETTI_DISPONIBILI DROP COLUMN DichiarazioneResidenzaOK
	IF COL_LENGTH('dbo.SUSCN_DOL_PROGETTI_DISPONIBILI', 'DichiarazioneRequisitiGaranziaGiovani') IS NOT NULL
		ALTER TABLE SUSCN_DOL_PROGETTI_DISPONIBILI DROP COLUMN DichiarazioneRequisitiGaranziaGiovani
	IF COL_LENGTH('dbo.SUSCN_DOL_PROGETTI_DISPONIBILI', 'DataPresaInCaricoGaranziaGiovani') IS NOT NULL
		ALTER TABLE SUSCN_DOL_PROGETTI_DISPONIBILI DROP COLUMN DataPresaInCaricoGaranziaGiovani
	IF COL_LENGTH('dbo.SUSCN_DOL_PROGETTI_DISPONIBILI', 'LuogoPresaInCaricoGaranziaGiovani') IS NOT NULL
		ALTER TABLE SUSCN_DOL_PROGETTI_DISPONIBILI DROP COLUMN LuogoPresaInCaricoGaranziaGiovani
	IF COL_LENGTH('dbo.SUSCN_DOL_PROGETTI_DISPONIBILI', 'DataDIDGaranziaGiovani') IS NOT NULL
		ALTER TABLE SUSCN_DOL_PROGETTI_DISPONIBILI DROP COLUMN DataDIDGaranziaGiovani
	IF COL_LENGTH('dbo.SUSCN_DOL_PROGETTI_DISPONIBILI', 'LuogoDIDGaranziaGiovani') IS NOT NULL
		ALTER TABLE SUSCN_DOL_PROGETTI_DISPONIBILI DROP COLUMN LuogoDIDGaranziaGiovani
	IF COL_LENGTH('dbo.SUSCN_DOL_PROGETTI_DISPONIBILI', 'AlternativaRequisitiGaranziaGiovani') IS NOT NULL
		ALTER TABLE SUSCN_DOL_PROGETTI_DISPONIBILI DROP COLUMN AlternativaRequisitiGaranziaGiovani
	IF COL_LENGTH('dbo.SUSCN_DOL_PROGETTI_DISPONIBILI', 'LinkSintesi') IS NOT NULL
		ALTER TABLE SUSCN_DOL_PROGETTI_DISPONIBILI DROP COLUMN LinkSintesi
	IF COL_LENGTH('dbo.SUSCN_DOL_PROGETTI_DISPONIBILI', 'DichiarazioneMinoriOpportunita') IS NOT NULL
		ALTER TABLE SUSCN_DOL_PROGETTI_DISPONIBILI DROP COLUMN DichiarazioneMinoriOpportunita


	IF OBJECT_ID('SUSCN_DOL_OBIETTIVO_PROGRAMMA') IS NOT NULL
		DROP TABLE SUSCN_DOL_OBIETTIVO_PROGRAMMA
	IF OBJECT_ID('SUSCN_DOL_AMBITO') IS NOT NULL
		DROP TABLE SUSCN_DOL_AMBITO
	IF OBJECT_ID('SUSCN_DOL_PROGRAMMA') IS NOT NULL
		DROP TABLE SUSCN_DOL_PROGRAMMA
	IF OBJECT_ID('SUSCN_DOL_GARANZIA_GIOVANI') IS NOT NULL
		DROP TABLE SUSCN_DOL_GARANZIA_GIOVANI
	IF OBJECT_ID('SUSCN_DOL_OBIETTIVO') IS NOT NULL
		DROP TABLE SUSCN_DOL_OBIETTIVO
	IF OBJECT_ID('RequisitiGaranziaGiovani') IS NOT NULL
		DROP TABLE RequisitiGaranziaGiovani
	IF OBJECT_ID('FK_DomandaPartecipazione_MotivoAnnullamento') IS NOT NULL 
		ALTER TABLE DomandaPartecipazione DROP CONSTRAINT FK_DomandaPartecipazione_MotivoAnnullamento
	IF COL_LENGTH('dbo.DomandaPartecipazione', 'DataRichiestaAnnullamento') IS NOT NULL
		ALTER TABLE DomandaPartecipazione DROP COLUMN DataRichiestaAnnullamento
	IF COL_LENGTH('dbo.DomandaPartecipazione', 'IdMotivazioneAnnullamento') IS NOT NULL
		ALTER TABLE DomandaPartecipazione DROP COLUMN IdMotivazioneAnnullamento
	IF COL_LENGTH('dbo.DomandaPartecipazione', 'FormazioneAnagraficaDisciplina') IS NOT NULL
		ALTER TABLE DomandaPartecipazione DROP COLUMN FormazioneAnagraficaDisciplina
	IF COL_LENGTH('dbo.DomandaPartecipazione', 'FormazioneAnagraficaAnno') IS NOT NULL
		ALTER TABLE DomandaPartecipazione DROP COLUMN FormazioneAnagraficaAnno
	IF COL_LENGTH('dbo.DomandaPartecipazione', 'FormazioneAnagraficaItalia') IS NOT NULL
		ALTER TABLE DomandaPartecipazione DROP COLUMN FormazioneAnagraficaItalia
	IF COL_LENGTH('dbo.DomandaPartecipazione', 'FormazioneAnagraficaIstituto') IS NOT NULL
		ALTER TABLE DomandaPartecipazione DROP COLUMN FormazioneAnagraficaIstituto
	IF COL_LENGTH('dbo.DomandaPartecipazione', 'FormazioneAnagraficaEnte') IS NOT NULL
		ALTER TABLE DomandaPartecipazione DROP COLUMN FormazioneAnagraficaEnte
	IF COL_LENGTH('dbo.DomandaPartecipazione', 'DichiarazioneResidenzaOK') IS NOT NULL
		ALTER TABLE DomandaPartecipazione DROP COLUMN DichiarazioneResidenzaOK
	IF COL_LENGTH('dbo.DomandaPartecipazione', 'DichiarazioneRequisitiGaranziaGiovani') IS NOT NULL
		ALTER TABLE DomandaPartecipazione DROP COLUMN DichiarazioneRequisitiGaranziaGiovani
	IF COL_LENGTH('dbo.DomandaPartecipazione', 'DataPresaInCaricoGaranziaGiovani') IS NOT NULL
		ALTER TABLE DomandaPartecipazione DROP COLUMN DataPresaInCaricoGaranziaGiovani
	IF COL_LENGTH('dbo.DomandaPartecipazione', 'LuogoPresaInCaricoGaranziaGiovani') IS NOT NULL
		ALTER TABLE DomandaPartecipazione DROP COLUMN LuogoPresaInCaricoGaranziaGiovani
	IF COL_LENGTH('dbo.DomandaPartecipazione', 'DataDIDGaranziaGiovani') IS NOT NULL
		ALTER TABLE DomandaPartecipazione DROP COLUMN DataDIDGaranziaGiovani
	IF COL_LENGTH('dbo.DomandaPartecipazione', 'LuogoDIDGaranziaGiovani') IS NOT NULL
		ALTER TABLE DomandaPartecipazione DROP COLUMN LuogoDIDGaranziaGiovani
	IF COL_LENGTH('dbo.DomandaPartecipazione', 'AlternativaRequisitiGaranziaGiovani') IS NOT NULL
		ALTER TABLE DomandaPartecipazione DROP COLUMN AlternativaRequisitiGaranziaGiovani


	IF OBJECT_ID('PK_SUSCN_DOL_CODICE_FISCALE_VOLONTARIO') IS NOT NULL 
		ALTER TABLE SUSCN_DOL_CODICE_FISCALE_VOLONTARIO DROP CONSTRAINT PK_SUSCN_DOL_CODICE_FISCALE_VOLONTARIO
	IF COL_LENGTH('dbo.SUSCN_DOL_CODICE_FISCALE_VOLONTARIO', 'TipoServizio') IS NOT NULL
		ALTER TABLE SUSCN_DOL_CODICE_FISCALE_VOLONTARIO DROP COLUMN TipoServizio

	IF OBJECT_ID('MotivoAnnullamento') IS NOT NULL
		DROP TABLE MotivoAnnullamento
	IF OBJECT_ID('SUSCN_DOL_MINORE_OPPORTUNITA') IS NOT NULL
		DROP TABLE SUSCN_DOL_MINORE_OPPORTUNITA
	
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
		IdAmbitoAzione INT NOT NULL
			CONSTRAINT FK_Programma_Garanzia_AMBITO FOREIGN KEY REFERENCES SUSCN_DOL_AMBITO(IdAmbitoAzione),
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


	CREATE TABLE SUSCN_DOL_MINORE_OPPORTUNITA(
		IDParticolarità int NOT NULL PRIMARY KEY,
		Descrizione varchar(100) NOT NULL
	)

	CREATE Table MotivoAnnullamento (
		Id INT PRIMARY Key IDENTITY (1,1),
		Descrizione NVARCHAR(100) NOT NULL,
		Selezionabile Bit NOT NULL
	)

	INSERT INTO MotivoAnnullamento VALUES
	('Cancellazione Progetto',0),
	('Errore nella compilazione',1),
	('Modifica del progetto scelto',1),
	('Non voglio più partecipare al Bando',1),
	('Altro/Preferisco non rispondere',1)
	
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
		IDParticolritàEntità INT NULL
			CONSTRAINT FK_Progetto_ParticolaritàEntità FOREIGN KEY REFERENCES SUSCN_DOL_MINORE_OPPORTUNITA(IDParticolarità),
		IdProgramma INT NULL
			CONSTRAINT FK_Progetto_Programma FOREIGN KEY REFERENCES SUSCN_DOL_PROGRAMMA(IdProgramma),
		LinkSintesi nvarchar(200),
		DichiarazioneMinoriOpportunita bit NULL

	ALTER TABLE SUSCN_DOL_CODICE_FISCALE_VOLONTARIO
	ADD
		TipoServizio VARCHAR(2) NOT NULL CONSTRAINT DF_TipoServizioVolontario DEFAULT 'SC',
		CONSTRAINT PK_SUSCN_DOL_CODICE_FISCALE_VOLONTARIO PRIMARY KEY (CodiceFiscale,DataInizioServizio,DataFineServizio,TipoServizio)
	ALTER TABLE SUSCN_DOL_CODICE_FISCALE_VOLONTARIO DROP CONSTRAINT DF_TipoServizioVolontario

	CREATE TABLE RequisitiGaranziaGiovani(
		TipoGG varchar(100) NOT NULL,
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

	COMMIT

END TRY

	BEGIN CATCH
		IF(@@TRANCOUNT > 0)
			ROLLBACK;
		THROW
	END CATCH