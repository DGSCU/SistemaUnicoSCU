use registrazione
IF OBJECT_ID('VWStatistiche', 'V') IS NOT NULL
    DROP VIEW VWStatistiche
CREATE VIEW VWStatistiche
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
go
select * from VWStatistiche
/*
select * from Registrazione where CodiceFiscaleEnte NOT IN (select CodiceFiscaleEnte from ente)
select * from ente where CodiceFiscaleEnte NOT IN (select CodiceFiscaleEnte from Registrazione)


select E.CodiceFiscaleRappresentanteLegale, R.CodiceFiscaleRappresentanteLegale , e.Denominazione,r.Denominazione from 

Ente E  JOIN
Registrazione R ON e.CodiceFiscaleEnte=R.CodiceFiscaleEnte where
E.CodiceFiscaleRappresentanteLegale <>  R.CodiceFiscaleRappresentanteLegale*/