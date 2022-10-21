CREATE PROCEDURE SP_AnnullaDomanda(
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

	UPDATE
		DomandaPartecipazione
	SET
		DataAnnullamento=GETDATE()
	WHERE 
		Id=@IdDomanda
	INSERT INTO
		DomandaPartecipazione

	SELECT
		CodiceFiscale,
		GruppoBando,
		UserIdInserimento,
		GETDATE(),--Data Inserimento
		NULL,--User ID Modifica
		NULL,--Data Modifica
		CodiceProgettoSelezionato,
		CodiceSedeSelezionata,
		NULL,--UserIdPresentazione
		NULL,--DataPresentazione
		NULL,--DataAnnullamento
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
		NULL,--FileDomanda
		NULL,--DataInvioEmailAnnullamento
		NazioneResidenza,
		ResidenzaEstera,
		ConfermaResidenza,
		IndirizzoCompletoResidenza,
		NULL,--IdZip
		NULL,--DataRichiestaAnnullamento
		NULL,--IdMotivazioneAnnullamento
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
	FROM DomandaPartecipazione
	WHERE 
		Id=@IdDomanda