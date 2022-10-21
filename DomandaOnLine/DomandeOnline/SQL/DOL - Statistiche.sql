USE DomandaOnline

DROP TABLE IF EXISTS #Inserite
DROP TABLE IF EXISTS #Presentate
DROP TABLE IF EXISTS #Annullate

SELECT
	CAST(DataInserimento as date) [Data],
	COUNT(*) DomandeInserite,
	COUNT(CASE WHEN U.Spidcode IS NULL THEN 1 END) DomandeInseriteCredenziali
INTO #Inserite
FROM 
	DomandaPartecipazione D JOIN
	AspNetUsers U ON U.Id=D.UserIdInserimento
WHERE
	GruppoBando = 59 
GROUP BY
	CAST(DataInserimento as date)
SELECT
	CAST(DataPresentazione as date) [Data],
	COUNT(*) DomandePresentate,
	COUNT(CASE WHEN U.Spidcode IS NULL THEN 1 END) DomandePresentateCredenziali
INTO #Presentate
FROM 
	DomandaPartecipazione D JOIN
	AspNetUsers U ON U.Id=D.UserIdInserimento
WHERE
	GruppoBando = 59 
	and DataPresentazione IS NOT NULL
GROUP BY
	CAST(DataPresentazione as date)
SELECT
	CAST(DataRichiestaAnnullamento as date) [Data],
	COUNT(*) DomandeAnnullate,
	COUNT(CASE WHEN U.Spidcode IS NULL THEN 1 END) DomandeAnnullateCredenziali
INTO #Annullate
FROM 
	DomandaPartecipazione D JOIN
	AspNetUsers U ON U.Id=D.UserIdInserimento
WHERE
	GruppoBando = 59 
	and DataRichiestaAnnullamento IS NOT NULL
GROUP BY
	CAST(DataRichiestaAnnullamento as date)

SELECT
	COALESCE(I.Data,P.data,A.Data) Data,
	COALESCE(I.DomandeInserite,0) DomandeInserite,
	COALESCE(I.DomandeInseriteCredenziali,0) DomandeInseriteCredenziali,
	COALESCE(P.DomandePresentate,0) DomandePresentate,
	COALESCE(P.DomandePresentateCredenziali,0)DomandePresentateCredenziali,
	COALESCE(A.DomandeAnnullate,0) DomandeAnnullate,
	COALESCE(A.DomandeAnnullateCredenziali,0)DomandeAnnullateCredenziali
FROM
	#Inserite I FULL JOIN
	#Presentate P ON P.Data=I.Data FULL JOIN
	#Annullate A ON A.Data=I.Data OR P.Data=A.Data
DROP TABLE IF EXISTS #Inserite
DROP TABLE IF EXISTS #Presentate
DROP TABLE IF EXISTS #Annullate

select
	CAST(DataApprovazione as date) Data
	,count(*) Approvazioni
from 
		RichiestaCredenziali where IdStato=2
	and DataApprovazione>='20201221'
group by
	CAST(DataApprovazione as date)

