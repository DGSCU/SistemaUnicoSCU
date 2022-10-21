USE [Registrazione]
GO

/****** Object:  View [dbo].[VWDatiProtocolloAntimafia]    Script Date: 21/10/2021 17:41:08 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


ALTER VIEW [dbo].[VWDatiProtocolloAntimafia]
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
	RL.Cognome,
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
where r.IDEntePersonaleRuolo in 
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


