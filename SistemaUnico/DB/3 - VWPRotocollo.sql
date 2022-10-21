use Registrazione
GO
IF OBJECT_ID (N'VWProtocollo', N'V') IS NOT NULL 
BEGIN
	DROP VIEW VWProtocollo
END
IF OBJECT_ID (N'VWProtocolloPresentazione', N'V') IS NOT NULL 
BEGIN
	DROP VIEW VWProtocolloPresentazione
END
IF OBJECT_ID (N'VWProtocolloAntimafia', N'V') IS NOT NULL 
BEGIN
	DROP VIEW VWProtocolloAntimafia
END
IF OBJECT_ID (N'VWDatiProtocolloAntimafia', N'V') IS NOT NULL 
BEGIN
	DROP VIEW VWDatiProtocolloAntimafia
END



GO
CREATE VIEW [dbo].[VWProtocollo]
AS
SELECT
	P.Id,
	P.TipoDomanda,
	P.DataProtocollazione,
	P.NumeroProtocollo,
	P.DataProtocollo,
	P.DataInvioEmail,
	BinData,
	E.Denominazione,
	E.CodiceFiscale CodiceFiscaleEnte,
	--E.DataRichiestaAccreditamento,
	ef.DataFineFase as DataRichiestaAccreditamento,
	E.Email,
	E.CodiceRegione,
	E.EmailCertificata PEC,
	RL.CodiceFiscale CodiceFiscaleRL,
	RL.Nome,
	RL.Cognome,
	A.FileName,
	A.IdEnteFase
FROM 
	unscsviluppo.dbo.ProtocolloDomanda P JOIN
	unscsviluppo.dbo.Enti E ON E.IDEnte =P.idente	LEFT JOIN
	unscsviluppo.dbo.EntiDocumenti A ON 
		P.IdAllegatoDomanda =A.IdEnteDocumento JOIN
	unscsviluppo.dbo.entepersonale RL ON
		RL.IDEnte =E.IDEnte JOIN 
	unscsviluppo.dbo.entepersonaleruoli R ON
		R.IDRuolo=4 AND
		R.IDEntePersonale=RL.IDEntePersonale
	INNER JOIN unscsviluppo.dbo.EntiFasi ef ON A.IdEnteFase = ef.IdEnteFase
where rl.DataFineValidità is null and r.DataFineValidità is null and r.Accreditato in (0,1)
GO

CREATE VIEW [dbo].[VWProtocolloPresentazione]
AS
	SELECT 
		Id,
		IdEnte,
		TipoDomanda,
		DataProtocollazione,
		DataProtocollo,
		NumeroProtocollo,
		DataInvioEmail,
		IdEnteFase
	FROM
		unscsviluppo.dbo.ProtocolloDomanda P
GO

CREATE VIEW [dbo].[VWProtocolloAntimafia]
AS
	SELECT 
		Id,
		IdEnteFaseAntimafia,
		DataProtocollazione,
		DataProtocollo,
		NumeroProtocollo,
		DataInvioEmail
	FROM
		unscsviluppo.dbo.ProtocolloAntimafia P
GO

CREATE VIEW [dbo].[VWDatiProtocolloAntimafia]
AS
SELECT
	P.Id,
	DataProtocollazione,
	NumeroProtocollo,
	DataProtocollo,
	DataInvioEmail,
	BinData,
	E.Denominazione,
	E.CodiceFiscale CodiceFiscaleEnte,
	F.DataChiusuraFase DataChiusuraAntimafia,
	E.Email,
	E.CodiceRegione,
	E.EmailCertificata PEC,
	RL.CodiceFiscale CodiceFiscaleRL,
	RL.Nome,
	RL.Cognome ,
	A.FileName
FROM 
	unscsviluppo.dbo.ProtocolloAntimafia P JOIN
	unscsviluppo.dbo.EntiFasiAntimafia F ON F.IdEnteFaseAntimafia =P.IdEnteFaseAntimafia	LEFT JOIN
	unscsviluppo.dbo.Enti E ON E.IDEnte =F.idente	LEFT JOIN
	unscsviluppo.dbo.Allegato A ON 
		A.IdAllegato =F.IdAllegatoComunicazioneAntimafia JOIN
	unscsviluppo.dbo.entepersonale RL ON
		RL.IDEnte =E.IDEnte JOIN 
	unscsviluppo.dbo.entepersonaleruoli R ON
		R.IDRuolo=4 AND
		R.IDEntePersonale=RL.IDEntePersonale
GO

