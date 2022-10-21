use Registrazione
--Creazione viste per recuperare i comuni e provincie da Helios
GO

IF OBJECT_ID('VWComune', 'V') IS NOT NULL
    DROP VIEW VWComune
IF OBJECT_ID('VWProvincia', 'V') IS NOT NULL
    DROP VIEW VWProvincia
IF OBJECT_ID('VWTipologiaEnte', 'V') IS NOT NULL
    DROP VIEW VWTipologiaEnte

GO
CREATE VIEW VWComune
AS
select IdComune Id,Denominazione Nome,CF CodiceCatastale,IdProvincia from unscsviluppo.dbo.comuni Where cf is not null and ComuneNazionale =1
GO
CREATE VIEW VWProvincia
AS
select IDProvincia Id,Provincia Nome,DescrAbb Sigla from unscsviluppo.dbo.provincie Where ProvinceNazionali=1 and NULLIF(DescrAbb,'') IS NOT NULL
GO
CREATE VIEW VWTipologiaEnte
AS
SELECT TOP 100 PERCENT
IdTipologieEnti ID,
REPLACE(descrizione,'(specificare)','') Descrizione,
CASE WHEN Privato=1 THEN 1 ELSE 2 END IdCategoriaEnte
FROM 
	unscsviluppo.dbo.TipologieEnti
WHERE Abilitata =1 AND Iscrizione = 1 
ORDER BY Ordinamento