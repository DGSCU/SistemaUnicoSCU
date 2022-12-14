USE [DOLTest]
GO
/****** Object:  StoredProcedure [dbo].[SP_AnnullaDomanda]    Script Date: 26/01/2022 15:20:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[SP_AnnullaDomanda](
	@IdDomanda INT
)
AS
	IF EXISTS(
		SELECT Id
		FROM
			DomandaPartecipazione
		WHERE
			Id=@IdDomanda
		AND
			DataAnnullamento IS NOT NULL
	)
	BEGIN
		RETURN
	END
	BEGIN TRAN Annullamento
	DECLARE @Progetto NVARCHAR(22)
	DECLARE @Sede int
	UPDATE
		DomandaPartecipazione
	SET
		DataAnnullamento=GETDATE()
	WHERE 
		Id=@IdDomanda
	INSERT INTO
		DomandaPartecipazione
	(		CodiceFiscale,
		GruppoBando,
		UserIdInserimento,
		DataInserimento,
		UserIdModifica,
		DataModifica,
		CodiceProgettoSelezionato,
		CodiceSedeSelezionata,
		UserIdPresentazione,
		DataPresentazione,
		DataAnnullamento,
		AllegatoCV,
		NomeFileCV,
		Nome,
		Cognome,
		Genere,
		DataNascita,
		LuogoNascita,
		NazioneNascita,
		Cittadinanza,
		Telefono,
		Email,
		ComuneResidenza,
		ProvinciaResidenza,
		ViaResidenza,
		CivicoResidenza,
		CapResidenza,
		ComuneRecapito,
		ProvinciaRecapito,
		ViaRecapito,
		CivicoRecapito,
		CapRecapito,
		CodiceMinoriOpportunita,
		IdMotivazione,
		CodiceDichiarazioneCittadinanza,
		NonCondanneOk,
		TrasferimentoSedeOk,
		TrasferimentoProgettoOk,
		AltreDichiarazioniOk,
		IdTitoloStudio,
		PrivacyPresaVisione,
		PrivacyConsenso,
		PrecedentiEnte,
		PrecedentiEnteDescrizione,
		PrecedentiAltriEnti,
		PrecedentiAltriEntiDescrizione,
		PrecedentiImpiego,
		PrecedentiImpiegoDescrizione,
		IdTitoloStudioEsperienze,
		FormazioneDisciplina,
		FormazioneAnno,
		FormazioneData,
		FormazioneItalia,
		FormazioneIstituto,
		FormazioneEnte,
		IscrizioneSuperioreAnno,
		IscrizioneSuperioreIstituto,
		IscrizioneLaureaAnno,
		IscrizioneLaureaCorso,
		IscrizioneLaureaIstituto,
		CorsiEffettuati,
		Specializzazioni,
		Competenze,
		Altro,
		FileDomanda,
		DataInvioEmailAnnullamento,
		NazioneResidenza,
		ResidenzaEstera,
		ConfermaResidenza,
		IndirizzoCompletoResidenza,
		IdZip,
		DataRichiestaAnnullamento,
		IdMotivazioneAnnullamento,
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
		DichiarazioneMinoriOpportunita)
	SELECT
		D.CodiceFiscale,
		D.GruppoBando,
		D.UserIdInserimento,
		GETDATE(),--Data Inserimento
		D.UserIdModifica,--User ID Modifica
		GETDATE(),--Data Modifica
		D.CodiceProgettoSelezionato,
		D.CodiceSedeSelezionata,
		NULL,--UserIdPresentazione
		NULL,--DataPresentazione
		NULL,--DataAnnullamento
		D.AllegatoCV,
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
		D.ComuneRecapito,
		D.ProvinciaRecapito,
		D.ViaRecapito,
		D.CivicoRecapito,
		D.CapRecapito,
		D.CodiceMinoriOpportunita,
		D.IdMotivazione,
		D.CodiceDichiarazioneCittadinanza,
		D.NonCondanneOk,
		D.TrasferimentoSedeOk,
		D.TrasferimentoProgettoOk,
		D.AltreDichiarazioniOk,
		D.IdTitoloStudio,
		D.PrivacyPresaVisione,
		D.PrivacyConsenso,
		D.PrecedentiEnte,
		D.PrecedentiEnteDescrizione,
		D.PrecedentiAltriEnti,
		D.PrecedentiAltriEntiDescrizione,
		D.PrecedentiImpiego,
		D.PrecedentiImpiegoDescrizione,
		D.IdTitoloStudioEsperienze,
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
		NULL,--FileDomanda
		NULL,--DataInvioEmailAnnullamento
		D.NazioneResidenza,
		D.ResidenzaEstera,
		D.ConfermaResidenza,
		D.IndirizzoCompletoResidenza,
		NULL,--IdZip
		NULL,--DataRichiestaAnnullamento
		NULL,--IdMotivazioneAnnullamento
		D.FormazioneAnagraficaDisciplina,
		D.FormazioneAnagraficaAnno,
		D.FormazioneAnagraficaItalia,
		D.FormazioneAnagraficaIstituto,
		D.FormazioneAnagraficaEnte,
		D.DichiarazioneResidenzaOK,
		D.DichiarazioneRequisitiGaranziaGiovani,
		D.DataPresaInCaricoGaranziaGiovani,
		D.LuogoPresaInCaricoGaranziaGiovani,
		D.DataDIDGaranziaGiovani,
		D.LuogoDIDGaranziaGiovani,
		D.AlternativaRequisitiGaranziaGiovani,
		CASE WHEN P.NumeroGiovaniMinoriOpportunità=0 THEN  NULL ELSE  D.DichiarazioneMinoriOpportunita END
	FROM 
		DomandaPartecipazione D LEFT JOIN
		SUSCN_DOL_PROGETTI_DISPONIBILI P ON
			P.CodiceProgetto=D.CodiceProgettoSelezionato AND
			P.CodiceSede = D.CodiceSedeSelezionata
	WHERE 
		Id=@IdDomanda
	--Recupero il progetto selezionato
	SELECT
		@Progetto=CodiceProgettoSelezionato,
		@Sede=CodiceSedeSelezionata
	FROM
		DomandaPartecipazione
	WHERE
		Id=@IdDomanda
	UPDATE SUSCN_DOL_PROGETTI_DISPONIBILI SET NumeroDomande=(
		SELECT
			COUNT(*)
		FROM 
			DomandaPartecipazione D
		WHERE
			D.DataAnnullamento IS NULL AND
			D.DataPresentazione IS NOT NULL AND
			CodiceProgettoSelezionato =@Progetto AND
			CodiceSedeSelezionata= @Sede
	)
	WHERE
		CodiceProgetto =@Progetto AND
		CodiceSede= @Sede

	COMMIT 	TRAN Annullamento
