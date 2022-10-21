BEGIN TRAN
	IF OBJECT_ID('DomandeZip', 'U') IS NOT NULL DROP TABLE DomandeZip


	CREATE TABLE DomandeZip
	(
		Id INT PRIMARY KEY IDENTITY(1,1),
		IdBando INT,
		CodiceEnte NVARCHAR(10),
		NomeFile NVARCHAR(200),
		Progressivo INT,
		DimensioneByte INT,
		DataInizioProcesso DATETIME,
		DataFineProcesso DATETIME
	)

	ALTER TABLE DomandaPartecipazione
		ADD IdZip INT
		CONSTRAINT FK_DomandaOnline_Zip FOREIGN KEY (IdZip) REFERENCES DomandeZip(Id)

	CREATE NONCLUSTERED INDEX [IX_DomandaPartecipazione_IdZip] ON [dbo].[DomandaPartecipazione]
	(
		[IdZip] ASC
	)
COMMIT

/*
DROP INDEX [IX_DomandaPartecipazione_IdZip] ON DomandaPartecipazione
ALTER TABLE DomandaPartecipazione
DROP CONSTRAINT [FK_DomandaOnline_Zip]
ALTER TABLE DomandaPartecipazione
DROP COLUMN IdZip

*/