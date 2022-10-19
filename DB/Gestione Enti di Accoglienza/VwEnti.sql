USE [Registrazione]
GO

/****** Object:  View [dbo].[VwEnti]    Script Date: 02/11/2021 09:23:00 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO







--ENTI TITOLARI E ACCOGLIENZA (SOLO SE GIA' REGISTRATI E QUINDI CON RL) SIA SCN CHE SCU
ALTER VIEW [dbo].[VwEnti]
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
from unscsviluppo.dbo.enti a
inner join unscsviluppo.dbo.entepersonale b on a.idente = b.idente 
inner join unscsviluppo.dbo.entepersonaleruoli c on b.IDEntePersonale = c.IDEntePersonale
left Join unscsviluppo.dbo.EntiPassword d on b.IDEntePersonale = d.IdEntePersonale
left join unscsviluppo.dbo.entirelazioni e on a.idente = e.IDEnteFiglio and e.DataFineValidit� is null
left join unscsviluppo.dbo.enti P on e.IDEntePadre = P.IDEnte
left JOIN unscsviluppo.dbo.TipologieEnti T ON T.Descrizione = a.Tipologia
LEFT JOIN (select IdEnte,S.IDComune,C.IDProvincia,Indirizzo,Civico,S.CAP,Telefono,Email,Http from unscsviluppo.dbo.entisedi S JOIN unscsviluppo.dbo.entiseditipi TS ON TS.IDEnteSede =S.IDEnteSede  AND TS.IDTipoSede=1 JOIN unscsviluppo.dbo.comuni C ON C.IDComune = S.IDComune) S ON  S.idente = a.idEnte
LEFT JOIN unscsviluppo.dbo.RLEntiAccoglienza ea on ea.IdEnteAccoglienza= a.IDEnte
where c.IDRuolo = 4 
	and b.DataFineValidit� is null and c.DataFineValidit� is null
	and c.Accreditato in (0,1)
	and ((a.Albo = 'SCU' and a.idstatoente not in (4,5,7)) or (a.albo = 'SCN' and a.IDStatoEnte in (10)))
	--o � un ente Titolare o un Ente di Accoglienza che deve essere stato registrato e abilitato
	and ((ea.RLcodicefiscale is not null and d.Username is not null) or (P.CodiceFiscale is null))
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
from unscsviluppo.dbo.EntiDelegati a
inner join unscsviluppo.dbo.enti b on a.IDEnte = b.idente
inner join unscsviluppo.dbo.EntiPassword c on a.IDDelegato = c.IdDelegato
left join unscsviluppo.dbo.entirelazioni e on b.idente = e.IDEnteFiglio and e.DataFineValidit� is null
left join unscsviluppo.dbo.enti P on e.IDEntePadre = P.IDEnte
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
from unscsviluppo.dbo.enti E
	INNER JOIN unscsviluppo.dbo.statienti SE ON E.IDStatoEnte = SE.IDStatoEnte
	join unscsviluppo.dbo.entirelazioni R on E.idente = R.IDEnteFiglio and R.DataFineValidit� is null
	join unscsviluppo.dbo.enti P on P.idente = R.IDEntePadre
WHERE
	ISNULL(E.CodiceFiscale,'') <>'' AND E.IDSTATOENTE NOT IN (7,10,11) AND E.ALBO = 'SCU'

UNION
--POTENZIALI RL ABILITATI MA NON ANCORA REGISTRATI
SELECT 
	EA.RLCodiceFiscale,
	EA.CodiceFiscaleEnte,
	CASE WHEN E.Denominazione IS NOT NULL THEN E.Denominazione ELSE  'Ente di accoglienza dell'' Ente ' + Et.Denominazione END Denominazione, 
	ET.Albo,
	NULL,
	CAST(0 as bit ) ,
	ET.CodiceFiscale CodiceFiscaleEntePadre,
	CASE WHEN T.Privato=1 THEN 1 ELSE 2 END IdCategoriaEnte,
	E.DataNominaRL DataNominaRappresentanteLegale,
	T.IdTipologieEnti IdTipologiaEnte,
	S.IDProvincia IdProvinciaEnte,
	S.IDComune IdComuneEnte,
	S.Indirizzo Via,
	S.Civico,
	S.CAP,
	S.Telefono,
	S.Email,
	E.EmailCertificata PEC,
	S.Http Sito,
	0 AS UtenzaSede
FROM 
	unscsviluppo.dbo.RLEntiAccoglienza EA JOIN
	unscsviluppo.dbo.Enti ET ON ET.IDEnte = Ea.IdEnteTitolare LEFT JOIN
	unscsviluppo.dbo.Enti E ON E.IDEnte = Ea.IdEnteAccoglienza LEFT JOIN
	(select IdEnte,S.IDComune,C.IDProvincia,Indirizzo,Civico,S.CAP,Telefono,Email,Http from unscsviluppo.dbo.entisedi S JOIN unscsviluppo.dbo.entiseditipi TS ON TS.IDEnteSede =S.IDEnteSede  AND TS.IDTipoSede=1 JOIN unscsviluppo.dbo.comuni C ON C.IDComune = S.IDComune) S ON  S.idente = E.idEnte
	left JOIN unscsviluppo.dbo.TipologieEnti T ON T.Descrizione = E.Tipologia
WHERE
	Visibile=1 AND
	--CodiceFiscaleEnte NOT IN ( select Codicefiscale from unscsviluppo.dbo.Enti where CodiceFiscale is not null)
	CodiceFiscaleEnte NOT IN (
		select	
			isnull(a.codicefiscale,a.codicefiscalearchivio) as CodiceFiscaleEnte
		from unscsviluppo.dbo.enti a
		inner join unscsviluppo.dbo.entepersonale b on a.idente = b.idente 
		inner join unscsviluppo.dbo.entepersonaleruoli c on b.IDEntePersonale = c.IDEntePersonale
		left Join unscsviluppo.dbo.EntiPassword d on b.IDEntePersonale = d.IdEntePersonale
		left join unscsviluppo.dbo.entirelazioni e on a.idente = e.IDEnteFiglio and e.DataFineValidit� is null
		left join unscsviluppo.dbo.enti P on e.IDEntePadre = P.IDEnte
		where c.IDRuolo = 4 
			and b.DataFineValidit� is null and c.DataFineValidit� is null
			and c.Accreditato in (0,1)
			and ((a.Albo = 'SCU' and a.idstatoente not in (4,5,7)) or (a.albo = 'SCN' and a.IDStatoEnte in (10)))
			and d.Username is not null
			and b.CodiceFiscale=EA.RLCodiceFiscale
	)

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
FROM unscsviluppo.DBO.SEDIPASSWORD a
INNER JOIN unscsviluppo.DBO.ENTISEDIATTUAZIONI B ON A.IDENTESEDEATTUAZIONE = B.IDENTESEDEATTUAZIONE
INNER JOIN unscsviluppo.DBO.ENTI C ON B.IDENTECAPOFILA = C.IDENTE
where a.codicefiscale is not null


GO


