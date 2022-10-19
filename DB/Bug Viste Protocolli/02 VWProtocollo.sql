USE [Registrazione]
GO

/****** Object:  View [dbo].[VWProtocollo]    Script Date: 21/10/2021 17:43:06 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

ALTER VIEW [dbo].[VWProtocollo]
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
and r.IDEntePersonaleRuolo in 
	(select max(IDEntePersonaleRuolo)--, sube.idente
		from unscsviluppo.dbo.entepersonaleruoli subR 
		inner join unscsviluppo.dbo.entepersonale subRL on subRL.IDEntePersonale = subr.IDEntePersonale
		inner join unscsviluppo.dbo.enti subE on subrl.IDEnte = sube.IDEnte
		where subr.DataFineValidità is null 
			and subrl.DataFineValidità is null 
			and subr.Accreditato in (0,1) 
			and subR.IDRuolo=4
		group by sube.IDEnte
		)

GO


