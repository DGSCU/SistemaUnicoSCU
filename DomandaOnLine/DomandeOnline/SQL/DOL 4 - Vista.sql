DROP VIEW IF EXISTS VW_DomandaPresentata
GO
CREATE VIEW VW_DomandaPresentata AS
SELECT
	D.Id,
	D.CodiceFiscale,
	D.GruppoBando,
	D.DataInserimento,
	D.UserIdModifica,
	D.DataModifica,
	D.CodiceProgettoSelezionato,
	D.CodiceSedeSelezionata,
	D.DataPresentazione,
	D.DataAnnullamento,
	D.NomeFileCV,
	D.Nome,
	D.Cognome,
	D.Genere,
	D.DataNascita,
	D.LuogoNascita,
	D.NazioneNascita,
	D.Cittadinanza,
	D.Telefono,
	D.Email,
	D.ComuneResidenza,
	D.ProvinciaResidenza,
	D.ViaResidenza,
	D.CivicoResidenza,
	D.CapResidenza,
	CONCAT(D.ViaResidenza,' ',D.CivicoResidenza,' ',D.CapResidenza,' ',D.ComuneResidenza,' ',D.ProvinciaResidenza) IndirizoCompleto,
	D.ComuneRecapito,
	D.ProvinciaRecapito,
	D.ViaRecapito,
	D.CivicoRecapito,
	D.CapRecapito,
	D.CodiceMinoriOpportunita,
	D.IdMotivazione,
	M.Descrizione Motivazione,
	D.CodiceDichiarazioneCittadinanza,
	D.NonCondanneOk,
	D.TrasferimentoSedeOk,
	D.TrasferimentoProgettoOk,
	D.AltreDichiarazioniOk,
	D.IdTitoloStudio,
	TA.Descrizione TitoloStudio,
	D.PrivacyPresaVisione,
	D.PrivacyConsenso,
	D.PrecedentiEnte,
	D.PrecedentiEnteDescrizione,
	D.PrecedentiAltriEnti,
	D.PrecedentiAltriEntiDescrizione,
	D.PrecedentiImpiego,
	D.PrecedentiImpiegoDescrizione,
	D.IdTitoloStudioEsperienze,
	TE.Descrizione TitoloStudioEsperienze,
	D.FormazioneDisciplina,
	D.FormazioneAnno,
	D.FormazioneData,
	D.FormazioneItalia,
	D.FormazioneIstituto,
	D.FormazioneEnte,
	D.IscrizioneSuperioreAnno,
	D.IscrizioneSuperioreIstituto,
	D.IscrizioneLaureaAnno,
	D.IscrizioneLaureaCorso,
	D.IscrizioneLaureaIstituto,
	D.CorsiEffettuati,
	D.Specializzazioni,
	D.Competenze,
	D.Altro
  FROM
		DomandaOnLine.dbo.DomandaPartecipazione D LEFT JOIN
		DomandaOnLine.dbo.Motivazione M ON
			D.IdMotivazione=M.Id LEFT JOIN
		DomandaOnLine.dbo.TitoloStudio TA ON
			D.IdTitoloStudio=TA.Id LEFT JOIN
		DomandaOnLine.dbo.TitoloStudio TE ON
			D.IdTitoloStudioEsperienze=TE.Id
  WHERE
	DataPresentazione IS NOT NULL
	AND
	DataAnnullamento IS NULL
