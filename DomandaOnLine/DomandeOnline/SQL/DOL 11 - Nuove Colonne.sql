IF NOT EXISTS(SELECT 1 FROM sys.columns 
          WHERE Name = N'DataFineAnnullamento'
          AND Object_ID = Object_ID(N'SUSCN_DOL_BANDOGRUPPO'))
BEGIN
	ALTER TABLE SUSCN_DOL_BANDOGRUPPO
	ADD DataFineAnnullamento datetime
END

IF NOT EXISTS(SELECT 1 FROM sys.columns 
          WHERE Name = N'IsDigitale'
          AND Object_ID = Object_ID(N'SUSCN_DOL_PROGRAMMA'))
BEGIN
	ALTER TABLE SUSCN_DOL_PROGRAMMA
	ADD IsDigitale bit
END