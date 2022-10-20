USE [Registrazione]
GO
/****** Object:  View [dbo].[VwEnti]    Script Date: 12/08/2022 12:33:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO





--ENTI TITOLARI E ACCOGLIENZA (SOLO SE GIA' REGISTRATI E QUINDI CON RL) SIA SCN CHE SCU
CREATE VIEW [dbo].[VwEnti]
AS
select	
		b.CodiceFiscale as CodiceFiscale,
		isnull(a.codicefiscale,a.codicefiscalearchivio) as CodiceFiscaleEnte, 
		a.Denominazione,
		a.Albo,
		d.Username AS Username,
		CAST(1 as bit ) RappresentanteLegale,
		--case isnull(e.identepadre,-1) when -1 then 1 ELSE 0 END AS Titolare,
		P.CodiceFiscale CodiceFiscaleEntePadre,
		CASE WHEN T.Privato=1 THEN 1 ELSE 2 END IdCategoriaEnte,
		a.DataNominaRL DataNominaRappresentanteLegale,
		T.IdTipologieEnti IdTipologiaEnte,
		S.IDProvincia IdProvinciaEnte,
		S.IDComune IdComuneEnte,
		S.Indirizzo Via,
		S.Civico,
		S.CAP,
		S.Telefono,
		S.Email,
		A.EmailCertificata PEC,
		S.Http Sito,
		0 AS UtenzaSede
from unscproduzione.dbo.enti a
inner join unscproduzione.dbo.entepersonale b on a.idente = b.idente 
inner join unscproduzione.dbo.entepersonaleruoli c on b.IDEntePersonale = c.IDEntePersonale
left Join unscproduzione.dbo.EntiPassword d on b.IDEntePersonale = d.IdEntePersonale
left join unscproduzione.dbo.entirelazioni e on a.idente = e.IDEnteFiglio and e.DataFineValidità is null
left join unscproduzione.dbo.enti P on e.IDEntePadre = P.IDEnte
left JOIN unscproduzione.dbo.TipologieEnti T ON T.Descrizione = a.Tipologia
LEFT JOIN (select IdEnte,S.IDComune,C.IDProvincia,Indirizzo,Civico,S.CAP,Telefono,Email,Http from unscproduzione.dbo.entisedi S JOIN unscproduzione.dbo.entiseditipi TS ON TS.IDEnteSede =S.IDEnteSede  AND TS.IDTipoSede=1 JOIN unscproduzione.dbo.comuni C ON C.IDComune = S.IDComune) S ON  S.idente = a.idEnte
where c.IDRuolo = 4 
	and b.DataFineValidità is null and c.DataFineValidità is null
	and c.Accreditato in (0,1)
	and ((a.Albo = 'SCU' and a.idstatoente not in (4,5,7)) or (a.albo = 'SCN' and a.IDStatoEnte in (10)))
UNION
--DELEGATI
select 	a.CodiceFiscale as CF_Persona,
		isnull(b.codicefiscale,b.codicefiscalearchivio) as CF_Ente,
		b.Denominazione,
		b.Albo,
		c.Username AS Username,
		CAST(0 as bit ) RappresentanteLegale,
		--,case isnull(e.identepadre,-1) when -1 then 1 ELSE 0 END AS Titolare
		P.CodiceFiscale CodiceFiscaleEntePadre,
		NULL,
		NULL,
		NULL,
		NULL,
		NULL,
		NULL,
		NULL,
		NULL,
		NULL,
		NULL,
		NULL,
		NULL,
	0 AS UtenzaSede
from unscproduzione.dbo.EntiDelegati a
inner join unscproduzione.dbo.enti b on a.IDEnte = b.idente
inner join unscproduzione.dbo.EntiPassword c on a.IDDelegato = c.IdDelegato
left join unscproduzione.dbo.entirelazioni e on b.idente = e.IDEnteFiglio and e.DataFineValidità is null
left join unscproduzione.dbo.enti P on e.IDEntePadre = P.IDEnte
where a.Stato = 1 and Visibile = 1

UNION
--ENTI ACCOGLIENZA SCU
select
	NULL,
	E.CodiceFiscale ,
	E.Denominazione,
	E.Albo,
	NULL,
	CAST(1 as bit ),
	P.CodiceFiscale CodiceFiscaleEntePadre,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,	
	NULL,
	0 AS UtenzaSede
from unscproduzione.dbo.enti E
	INNER JOIN unscproduzione.dbo.statienti SE ON E.IDStatoEnte = SE.IDStatoEnte
	join unscproduzione.dbo.entirelazioni R on E.idente = R.IDEnteFiglio and R.DataFineValidità is null
	join unscproduzione.dbo.enti P on P.idente = R.IDEntePadre
WHERE
	ISNULL(E.CodiceFiscale,'') <>'' AND E.IDSTATOENTE NOT IN (7,10,11) AND E.ALBO = 'SCU'

UNION
--POTENZIALI RL ABILITATI MA NON ANCORA REGISTRATI
SELECT 
	EA.RLCodiceFiscale,
	EA.CodiceFiscaleEnte,
	'Ente di accoglienza dell'' Ente ' + Et.Denominazione,
	ET.Albo,
	NULL,
	CAST(0 as bit ) ,
	ET.CodiceFiscale CodiceFiscaleEntePadre,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	0 AS UtenzaSede
FROM 
	unscproduzione.dbo.RLEntiAccoglienza EA JOIN
	unscproduzione.dbo.Enti ET ON ET.IDEnte = Ea.IdEnteTitolare
WHERE
	Visibile=1 AND
	CodiceFiscaleEnte NOT IN ( select Codicefiscale from unscproduzione.dbo.Enti where CodiceFiscale is not null)

UNION
--UTENZE SPID PER SEDE 

select 	a.CodiceFiscale as CF_Persona,
		isnull(C.codicefiscale,C.codicefiscalearchivio) as CF_Ente,
		'Sede n.' + CONVERT(varchar(10),b.identesedeattuazione) + ' dell''Ente "' + C.Denominazione+'"',
		c.Albo,
		a.Username AS Username,
		CAST(0 as bit ) RappresentanteLegale,
		--,case isnull(e.identepadre,-1) when -1 then 1 ELSE 0 END AS Titolare
		NULL,
		NULL,
		NULL,
		NULL,
		NULL,
		NULL,
		NULL,
		NULL,
		NULL,
		NULL,
		NULL,
		NULL,
		NULL,
		1 AS UtenzaSede
FROM unscproduzione.DBO.SEDIPASSWORD a
INNER JOIN unscproduzione.DBO.ENTISEDIATTUAZIONI B ON A.IDENTESEDEATTUAZIONE = B.IDENTESEDEATTUAZIONE
INNER JOIN unscproduzione.DBO.ENTI C ON B.IDENTECAPOFILA = C.IDENTE
where a.codicefiscale is not null
UNION
--Utenti semplici
	select 	a.CodiceFiscale as CF_Persona,
			isnull(b.codicefiscale,b.codicefiscalearchivio) as CF_Ente,
			b.Denominazione,
			b.Albo,
			c.Username AS Username,
			CAST(0 as bit ) RappresentanteLegale,
			--,case isnull(e.identepadre,-1) when -1 then 1 ELSE 0 END AS Titolare
			P.CodiceFiscale CodiceFiscaleEntePadre,
			NULL,
			NULL,
			NULL,
			NULL,
			NULL,
			NULL,
			NULL,
			NULL,
			NULL,
			NULL,
			NULL,
			NULL,
		2 AS UtenzaSede
	from unscproduzione.dbo.EntiUtenti a
	inner join unscproduzione.dbo.enti b on a.IDEnte = b.idente
	inner join unscproduzione.dbo.EntiPassword c on a.idEnteUtente = c.idEnteUtente
	left join unscproduzione.dbo.entirelazioni e on b.idente = e.IDEnteFiglio and e.DataFineValidità is null
	left join unscproduzione.dbo.enti P on e.IDEntePadre = P.IDEnte
	where a.Stato = 1 and Visibile = 1

GO
/****** Object:  View [dbo].[VWStatistiche]    Script Date: 12/08/2022 12:33:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[VWStatistiche]
AS
select 
	count(*) Totale,
	count(CASE WHEN R.Id is not null and E.CodiceFiscaleEnte is not null THEN 1 END) EntiRegistrati,
	count(CASE WHEN R.Id is null THEN 1 END) EntiNonRegistrati,
	count(CASE WHEN E.CodiceFiscaleEnte is null THEN 1 END) NuoviEntiRegistrati ,
	count(CASE WHEN E.CodiceFiscale <>  R.CodiceFiscaleRappresentanteLegale THEN 1 END) VariazioniRappresentanteLegale,
	count(CASE WHEN E.CodiceFiscaleEntePadre IS NULL OR R.EnteTitolare=1 THEN 1 END) EntiTitolari,
	count(CASE WHEN R.EnteTitolare=1 THEN 1 END) EntiTitolariRegistrati,
	count(CASE WHEN E.CodiceFiscaleEntePadre IS NOT NULL OR R.EnteTitolare=0 THEN 1 END) EntiDiAccoglienza,
	count(CASE WHEN R.EnteTitolare=0 THEN 1 END) EntiDiAccoglienzaRegistrati,
	count(CASE WHEN R.EnteTitolare=1 AND R.IdCategoriaEnte=1 THEN 1 END) EntiTitolariPrivatiRegistrati,
	count(CASE WHEN R.EnteTitolare=1 AND R.IdCategoriaEnte=2 THEN 1 END) EntiTitolariPubbliciRegistrati,
	count(CASE WHEN R.EnteTitolare=0 AND R.IdCategoriaEnte=1 THEN 1 END) EntiAccoglienzaPrivatiRegistrati,
	count(CASE WHEN R.EnteTitolare=0 AND R.IdCategoriaEnte=2 THEN 1 END) EntiAccoglienzaPubbliciRegistrati

from

VWEnti E FULL OUTER JOIN
Registrazione R ON e.CodiceFiscaleEnte=R.CodiceFiscaleEnte
GO
/****** Object:  View [dbo].[VWComune]    Script Date: 12/08/2022 12:33:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[VWComune]
AS
select IdComune Id,left(Denominazione,50) Nome,CF CodiceCatastale,IdProvincia from unscproduzione.dbo.comuni Where cf is not null and ComuneNazionale =1
GO
/****** Object:  View [dbo].[VWDatiDomandaOLP]    Script Date: 12/08/2022 12:33:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


  
CREATE VIEW [dbo].[VWDatiDomandaOLP]
AS

SELECT
	I.IdIstanzaSostituzioneOLP Id,
	I.DataProtocollazione,
	I.NumeroProtocollo,
	I.DataProtocollo,
	I.DataInvioEmail,
	I.Domanda,
	I.FileNameDomandaFirmata,
	E.Denominazione,
	E.CodiceFiscale CodiceFiscaleEnte,
	I.DataPresentazione,
	I.DataValutazione,
	E.Email,
	E.CodiceRegione,
	E.EmailCertificata PEC,
	RL.CodiceFiscale CodiceFiscaleRL,
	RL.Nome,
	RL.Cognome
FROM 
	unscproduzione.dbo.IstanzeSostituzioniOLP I JOIN
	unscproduzione.dbo.Enti E ON E.IDEnte =I.idente JOIN
	unscproduzione.dbo.entepersonale RL ON
		RL.IDEnte =E.IDEnte JOIN 
	unscproduzione.dbo.entepersonaleruoli R ON
		R.IDRuolo=4 AND
		R.IDEntePersonale=RL.IDEntePersonale
where r.IDEntePersonaleRuolo in 
	(select max(IDEntePersonaleRuolo)--, sube.idente
		from unscproduzione.dbo.entepersonaleruoli subR 
		inner join unscproduzione.dbo.entepersonale subRL on subRL.IDEntePersonale = subr.IDEntePersonale
		inner join unscproduzione.dbo.enti subE on subrl.IDEnte = sube.IDEnte
		where subr.DataFineValidità is null 
			and subrl.DataFineValidità is null 
			and subr.Accreditato in (0,1) 
			and subR.IDRuolo=4
		group by sube.IDEnte
		)

GO
/****** Object:  View [dbo].[VWDatiProtocolloAntimafia]    Script Date: 12/08/2022 12:33:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
	RL.Cognome,
	A.FileName
FROM 
	unscproduzione.dbo.ProtocolloAntimafia P JOIN
	unscproduzione.dbo.EntiFasiAntimafia F ON F.IdEnteFaseAntimafia =P.IdEnteFaseAntimafia	LEFT JOIN
	unscproduzione.dbo.Enti E ON E.IDEnte =F.idente	LEFT JOIN
	unscproduzione.dbo.Allegato A ON 
		A.IdAllegato =F.IdAllegatoComunicazioneAntimafia JOIN
	unscproduzione.dbo.entepersonale RL ON
		RL.IDEnte =E.IDEnte JOIN 
	unscproduzione.dbo.entepersonaleruoli R ON
		R.IDRuolo=4 AND
		R.IDEntePersonale=RL.IDEntePersonale
where r.IDEntePersonaleRuolo in 
	(select max(IDEntePersonaleRuolo)--, sube.idente
		from unscproduzione.dbo.entepersonaleruoli subR 
		inner join unscproduzione.dbo.entepersonale subRL on subRL.IDEntePersonale = subr.IDEntePersonale
		inner join unscproduzione.dbo.enti subE on subrl.IDEnte = sube.IDEnte
		where subr.DataFineValidità is null 
			and subrl.DataFineValidità is null 
			and subr.Accreditato in (0,1) 
			and subR.IDRuolo=4
		group by sube.IDEnte
		)
GO
/****** Object:  View [dbo].[VwEnti_bk]    Script Date: 12/08/2022 12:33:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




CREATE VIEW [dbo].[VwEnti_bk]
AS
select	
		b.CodiceFiscale as CodiceFiscale,
		isnull(a.codicefiscale,a.codicefiscalearchivio) as CodiceFiscaleEnte, 
		a.Denominazione,
		a.Albo,
		d.Username AS Username,
		CAST(1 as bit ) RappresentanteLegale,
		--case isnull(e.identepadre,-1) when -1 then 1 ELSE 0 END AS Titolare,
		P.CodiceFiscale CodiceFiscaleEntePadre,
		CASE WHEN T.Privato=1 THEN 1 ELSE 2 END IdCategoriaEnte,
		a.DataNominaRL DataNominaRappresentanteLegale,
		T.IdTipologieEnti IdTipologiaEnte,
		S.IDProvincia IdProvinciaEnte,
		S.IDComune IdComuneEnte,
		S.Indirizzo Via,
		S.Civico,
		S.CAP,
		S.Telefono,
		S.Email,
		A.EmailCertificata PEC,
		S.Http Sito

from unscproduzione.dbo.enti a
inner join unscproduzione.dbo.entepersonale b on a.idente = b.idente 
inner join unscproduzione.dbo.entepersonaleruoli c on b.IDEntePersonale = c.IDEntePersonale
left Join unscproduzione.dbo.EntiPassword d on b.IDEntePersonale = d.IdEntePersonale
left join unscproduzione.dbo.entirelazioni e on a.idente = e.IDEnteFiglio and e.DataFineValidità is null
left join unscproduzione.dbo.enti P on e.IDEntePadre = P.IDEnte
left JOIN unscproduzione.dbo.TipologieEnti T ON T.Descrizione = a.Tipologia
LEFT JOIN (select IdEnte,S.IDComune,C.IDProvincia,Indirizzo,Civico,S.CAP,Telefono,Email,Http from unscproduzione.dbo.entisedi S JOIN unscproduzione.dbo.entiseditipi TS ON TS.IDEnteSede =S.IDEnteSede  AND TS.IDTipoSede=1 JOIN unscproduzione.dbo.comuni C ON C.IDComune = S.IDComune) S ON  S.idente = a.idEnte
where c.IDRuolo = 4 
	and b.DataFineValidità is null and c.DataFineValidità is null
	and c.Accreditato in (0,1)
	and a.idstatoente not in (4,5,7)
	and a.Albo = 'SCU'
UNION
select 	a.CodiceFiscale as CF_Persona,
		isnull(b.codicefiscale,b.codicefiscalearchivio) as CF_Ente,
		b.Denominazione,
		b.Albo,
		c.Username AS Username,
		CAST(0 as bit ) RappresentanteLegale,
		--,case isnull(e.identepadre,-1) when -1 then 1 ELSE 0 END AS Titolare
		P.CodiceFiscale CodiceFiscaleEntePadre,
		NULL,
		NULL,
		NULL,
		NULL,
		NULL,
		NULL,
		NULL,
		NULL,
		NULL,
		NULL,
		NULL,
		NULL

from unscproduzione.dbo.EntiDelegati a
inner join unscproduzione.dbo.enti b on a.IDEnte = b.idente
inner join unscproduzione.dbo.EntiPassword c on a.IDDelegato = c.IdDelegato
left join unscproduzione.dbo.entirelazioni e on b.idente = e.IDEnteFiglio and e.DataFineValidità is null
left join unscproduzione.dbo.enti P on e.IDEntePadre = P.IDEnte
where a.Stato = 1 and Visibile = 1
UNION
select
	NULL,
	E.CodiceFiscale ,
	E.Denominazione,
	E.Albo,
	NULL,
	CAST(1 as bit ),
	P.CodiceFiscale CodiceFiscaleEntePadre,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL
from unscproduzione.dbo.enti E
	INNER JOIN unscproduzione.dbo.statienti SE ON E.IDStatoEnte = SE.IDStatoEnte
	join unscproduzione.dbo.entirelazioni R on E.idente = R.IDEnteFiglio and R.DataFineValidità is null
	join unscproduzione.dbo.enti P on P.idente = R.IDEntePadre
WHERE
	ISNULL(E.CodiceFiscale,'') <>'' AND E.IDSTATOENTE NOT IN (7,10,11) AND E.ALBO = 'SCU'
UNION
SELECT 
	EA.RLCodiceFiscale,
	EA.CodiceFiscaleEnte,
	'Ente di accoglienza dell'' Ente ' + Et.Denominazione,
	ET.Albo,
	NULL,
	CAST(0 as bit ) ,
	ET.CodiceFiscale CodiceFiscaleEntePadre,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL
FROM 
	unscproduzione.dbo.RLEntiAccoglienza EA JOIN
	unscproduzione.dbo.Enti ET ON ET.IDEnte = Ea.IdEnteTitolare
WHERE
	Visibile=1 AND
	CodiceFiscaleEnte NOT IN ( select Codicefiscale from unscproduzione.dbo.Enti where CodiceFiscale is not null)

GO
/****** Object:  View [dbo].[VWProtocollo]    Script Date: 12/08/2022 12:33:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
	unscproduzione.dbo.ProtocolloDomanda P JOIN
	unscproduzione.dbo.Enti E ON E.IDEnte =P.idente	LEFT JOIN
	unscproduzione.dbo.EntiDocumenti A ON 
		P.IdAllegatoDomanda =A.IdEnteDocumento JOIN
	unscproduzione.dbo.entepersonale RL ON
		RL.IDEnte =E.IDEnte JOIN 
	unscproduzione.dbo.entepersonaleruoli R ON
		R.IDRuolo=4 AND
		R.IDEntePersonale=RL.IDEntePersonale
	INNER JOIN unscproduzione.dbo.EntiFasi ef ON A.IdEnteFase = ef.IdEnteFase
where rl.DataFineValidità is null and r.DataFineValidità is null and r.Accreditato in (0,1)
and r.IDEntePersonaleRuolo in 
	(select max(IDEntePersonaleRuolo)--, sube.idente
		from unscproduzione.dbo.entepersonaleruoli subR 
		inner join unscproduzione.dbo.entepersonale subRL on subRL.IDEntePersonale = subr.IDEntePersonale
		inner join unscproduzione.dbo.enti subE on subrl.IDEnte = sube.IDEnte
		where subr.DataFineValidità is null 
			and subrl.DataFineValidità is null 
			and subr.Accreditato in (0,1) 
			and subR.IDRuolo=4
		group by sube.IDEnte
		)
GO
/****** Object:  View [dbo].[VWProtocolloAntimafia]    Script Date: 12/08/2022 12:33:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
		unscproduzione.dbo.ProtocolloAntimafia P
GO
/****** Object:  View [dbo].[VWProtocolloDatiProgramma]    Script Date: 12/08/2022 12:33:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



CREATE VIEW [dbo].[VWProtocolloDatiProgramma]
AS
SELECT
	Id,
	BinData,
	FileName,
	DataPresentazioneSistema,
	DataProtocollazione,
	NumeroProtocollo,
	DataProtocollo,
	DataInvioEmail,
	CodiceRegione,
	Denominazione,
	CodiceFiscale CodiceFiscaleEnte,
	B.Bando,
	E.Email,
	E.EmailCertificata PEC,
	DataAnnullamento,
	DataProtocollazioneAnnullamento,
	NumeroProtocolloAnnullamento,
	DataProtocolloAnnullamento,
	DataInvioEmailAnnullamento
FROM
	unscproduzione.dbo.ProtocolloIstanzaProgramma I JOIN
	unscproduzione.dbo.enti E ON e.IDEnte=I.IdEnte JOIN
	unscproduzione.dbo.bando B on B.IDBando=I.IdBando
GO
/****** Object:  View [dbo].[VwProtocolloOLP]    Script Date: 12/08/2022 12:33:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE VIEW [dbo].[VwProtocolloOLP]
AS
SELECT 
      IdIstanzaSostituzioneOLP Id,
      DataProtocollazione,
	  NumeroProtocollo,
      DataProtocollo,
	  DataInvioEmail
  FROM
	unscproduzione.dbo.IstanzeSostituzioniOLP
  WHERE 
	Stato=2 --Presentata
GO
/****** Object:  View [dbo].[VWProtocolloPresentazione]    Script Date: 12/08/2022 12:33:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



CREATE VIEW [dbo].[VWProtocolloPresentazione]
AS
	SELECT 
		Id,
		IdEnte,
		IdEnteFase,
		TipoDomanda,
		DataProtocollazione,
		DataProtocollo,
		NumeroProtocollo,
		DataInvioEmail
	FROM
		unscproduzione.dbo.ProtocolloDomanda P
GO
/****** Object:  View [dbo].[VWProtocolloProgetto]    Script Date: 12/08/2022 12:33:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



CREATE VIEW [dbo].[VWProtocolloProgetto]
AS
SELECT
	Id,
	DataProtocollazione,
	NumeroProtocollo,
	DataProtocollo,
	DataInvioEmail,
	DataAnnullamento,
	DataProtocollazioneAnnullamento,
	NumeroProtocolloAnnullamento,
	DataProtocolloAnnullamento,
	DataInvioEmailAnnullamento	
FROM
	unscproduzione.dbo.ProtocolloIstanzaProgramma I
GO
/****** Object:  View [dbo].[VWProvincia]    Script Date: 12/08/2022 12:33:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[VWProvincia]
AS
select IDProvincia Id,Provincia Nome,DescrAbb Sigla from unscproduzione.dbo.provincie Where ProvinceNazionali=1 and NULLIF(DescrAbb,'') IS NOT NULL
GO
/****** Object:  View [dbo].[VWTipologiaEnte]    Script Date: 12/08/2022 12:33:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[VWTipologiaEnte]
AS
SELECT TOP 100 PERCENT
IdTipologieEnti ID,
REPLACE(descrizione,'(specificare)','') Descrizione,
CASE WHEN Privato=1 THEN 1 ELSE 2 END IdCategoriaEnte
FROM 
	unscproduzione.dbo.TipologieEnti
WHERE Abilitata =1 AND Iscrizione = 1 
ORDER BY Ordinamento
GO
