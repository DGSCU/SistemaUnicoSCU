DECLARE @Version INT=2
DECLARE @CurrentVersion INT=(SELECT Version FROM DbVersion)
IF (@CurrentVersion=@Version)
	BEGIN
		PRINT 'Script già eseguito'
		RETURN
	END
IF (@CurrentVersion<>@Version-1)
	BEGIN
		PRINT COALESCE('Versione DB Errata Versione Attuale=',@CurrentVersion,' - Versione Necessaria=',@Version-1)
		RETURN
	END

BEGIN TRY
	BEGIN TRAN
	UPDATE DbVersion SET Version=@Version
		
	ALTER TABLE
		DomandaPartecipazione
	ADD
		NazioneResidenza nvarchar(200) NULL,
		IndirizzoCompletoResidenza nvarchar(200) NULL,
		ResidenzaEstera bit NULL,
		ConfermaResidenza bit NULL
	COMMIT
END TRY

BEGIN CATCH
	PRINT ERROR_MESSAGE ( )
	ROLLBACK

END CATCH
