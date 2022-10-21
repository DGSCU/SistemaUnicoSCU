USE [DomandaOnline ]
GO

/****** Object:  View [dbo].[VW_DomandaPresentata]    Script Date: 15/12/2020 13:44:47 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

ALTER VIEW [dbo].[VW_DomandaPresentata] AS
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
	D.ResidenzaEstera,
	D.IndirizzoCompletoResidenza,
	D.NazioneResidenza,
	D.ComuneResidenza,
	D.ProvinciaResidenza,
	D.ViaResidenza,
	D.CivicoResidenza,
	D.CapResidenza,
	CASE WHEN D.ResidenzaEstera=1 THEN 
		D.IndirizzoCompletoResidenza 
	ELSE
		CONCAT(D.ViaResidenza,' ',D.CivicoResidenza,' ',D.CapResidenza,' ',D.ComuneResidenza,' ',D.ProvinciaResidenza)
	END IndirizoCompleto,
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
	D.Altro,
	P.Sito,
	DataRichiestaAnnullamento,
	MA.Descrizione MotivoAnnullamento,
	FormazioneAnagraficaDisciplina,
	FormazioneAnagraficaAnno,
	FormazioneAnagraficaItalia,
	FormazioneAnagraficaIstituto,
	FormazioneAnagraficaEnte,
	DichiarazioneResidenzaOK,
	DichiarazioneRequisitiGaranziaGiovani,
	DataPresaInCaricoGaranziaGiovani,
	LuogoPresaInCaricoGaranziaGiovani,
	DataDIDGaranziaGiovani,
	LuogoDIDGaranziaGiovani,
	AlternativaRequisitiGaranziaGiovani,
	DichiarazioneMinoriOpportunita

  FROM
		DomandaPartecipazione D LEFT JOIN
		SUSCN_DOL_PROGETTI_DISPONIBILI P ON
			D.CodiceProgettoSelezionato=P.CodiceProgetto AND
			D.CodiceSedeSelezionata=P.CodiceSede LEFT JOIN
		Motivazione M ON
			D.IdMotivazione=M.Id LEFT JOIN
		TitoloStudio TA ON
			D.IdTitoloStudio=TA.Id LEFT JOIN
		TitoloStudio TE ON
			D.IdTitoloStudioEsperienze=TE.Id LEFT JOIN
		MotivoAnnullamento MA ON
			MA.Id=D.IdMotivazioneAnnullamento
  WHERE
	D.DataPresentazione IS NOT NULL
	AND
	D.DataAnnullamento IS NULL
GO


