USE DomandaOnline
GO
DROP PROCEDURE IF EXISTS SP_AggiornamentoDati
GO
CREATE PROCEDURE SP_AggiornamentoDati
AS

	PRINT 'Lettura Dati'
	BEGIN TRANSACTION LetturaDati
		IF OBJECT_ID('tempdb..#Progetto') IS NOT NULL DROP TABLE #Progetto
		IF OBJECT_ID('tempdb..#Bando') IS NOT NULL DROP TABLE #Bando
		IF OBJECT_ID('tempdb..#CodiceFiscale') IS NOT NULL DROP TABLE #CodiceFiscale
		IF OBJECT_ID('tempdb..#Geografico') IS NOT NULL DROP TABLE #Geografico
	   
		SELECT
			*
		INTO
			#Progetto
		FROM
			SQLTEST.unscproduzione.dbo.VW_DOL_PROGETTI_DISPONIBILI

		SELECT
			*
		INTO
			#Bando
		FROM
			SQLTEST.unscproduzione.dbo.VW_DOL_BANDOGRUPPO
	
		SELECT
			*
		INTO
			#CodiceFiscale
		FROM
			SQLTEST.unscproduzione.dbo.VW_DOL_VOLONTARI_SERVIZIO_EFFETTUATO
		
		SELECT
			*
		INTO
			#Geografico
		FROM
			SQLTEST.unscproduzione.dbo.VW_DOL_GEOGRAFICO_ITALIA		
	COMMIT TRANSACTION LetturaDati		
	PRINT 'Fine Lettura Dati'
	BEGIN TRAN ScritturaDati
	BEGIN TRY
		DELETE SUSCN_DOL_CODICE_FISCALE_VOLONTARIO
		INSERT INTO SUSCN_DOL_CODICE_FISCALE_VOLONTARIO Select * from #CodiceFiscale
		PRINT 'Codici Fiscali aggiornati'
		DELETE SUSCN_DOL_GEOGRAFICO_ITALIA
		INSERT INTO SUSCN_DOL_GEOGRAFICO_ITALIA Select * from #Geografico
		PRINT 'Comuni Aggiornati'
		/*** Inserimento Bando ****/
		INSERT INTO SUSCN_DOL_BANDOGRUPPO
		SELECT
			GH.Gruppo,
			GH.Descrizione,
			GH.Scadenza,
			GH.DataScadenza,
			GH.DataScadenzaGraduatorie
		FROM
			#Bando GH LEFT JOIN
			SUSCN_DOL_BANDOGRUPPO G ON
			 G.Gruppo=GH.Gruppo
		WHERE
			G.Gruppo IS NULL
		PRINT 'Bandi Inseriti'
		
		UPDATE G
		SET
			G.Descrizione=GH.Descrizione,
			G.Scadenza=GH.Scadenza,
			G.DataScadenza=GH.DataScadenza,
			G.DataScadenzaGraduatorie=GH.DataScadenzaGraduatorie
		FROM
			(SELECT * FROM
				#Bando
			EXCEPT 
			SELECT * FROM 
				SUSCN_DOL_BANDOGRUPPO
			)GH 
			JOIN
			SUSCN_DOL_BANDOGRUPPO G ON
			 G.Gruppo=GH.Gruppo
		PRINT 'Bandi aggiornati'

		/** Annullamento Sedi/Progetti **/
		UPDATE
			P
		SET
			DataAnnullamento=GetDate()
		FROM
			SUSCN_DOL_PROGETTI_DISPONIBILI P LEFT JOIN
			#Progetto PH ON
					P.CodiceProgetto = PH.CodiceProgetto
				AND P.CodiceSede = PH.CodiceSede
		WHERE
				P.DataAnnullamento IS NULL
			AND	P.Gruppo IN (Select Gruppo from #Bando)
			AND PH.CodiceProgetto IS NULL
		PRINT 'Progetti Annullati'

		/** Dis-Annullamento Sedi/Progetti **/
		UPDATE
			P
		SET
			DataAnnullamento=NULL
		FROM
			SUSCN_DOL_PROGETTI_DISPONIBILI P JOIN
			#Progetto PH ON
					P.CodiceProgetto = PH.CodiceProgetto
				AND P.CodiceSede = PH.CodiceSede
		WHERE
				P.DataAnnullamento IS NOT NULL
			AND	P.Gruppo IN (Select Gruppo from #Bando)
		PRINT 'Progetti Annullati'

		UPDATE
			D
		SET
			DataAnnullamento=GetDate()
		FROM
			DomandaPartecipazione D JOIN
			SUSCN_DOL_PROGETTI_DISPONIBILI P ON
					D.CodiceProgettoSelezionato = P.CodiceProgetto
				AND D.CodiceSedeSelezionata = P.CodiceSede
		WHERE
				D.DataAnnullamento IS NULL
			AND	D.DataPresentazione IS NOT NULL
			AND P.DataAnnullamento IS NOT NULL
		PRINT 'Domande Annullate'

		UPDATE
			D
		SET
			D.CodiceProgettoSelezionato= NULL,
			D.CodiceSedeSelezionata = NULL
		FROM
			DomandaPartecipazione D JOIN
			SUSCN_DOL_PROGETTI_DISPONIBILI P ON
					D.CodiceProgettoSelezionato = P.CodiceProgetto
				AND D.CodiceSedeSelezionata = P.CodiceSede
		WHERE
				D.DataPresentazione IS NULL
			AND P.DataAnnullamento IS NOT NULL
		PRINT 'Domande Non presentate con progetto annullato'

		/** Annullamento Sedi/Progetti **/
		INSERT INTO
			SUSCN_DOL_PROGETTI_DISPONIBILI
		SELECT
			PH.[CodiceEnte],
			PH.[NomeEnte],
			PH.[Sito],
			PH.[CodiceProgetto],
			PH.[TitoloProgetto],
			PH.[TipoProgetto],
			PH.[CodiceSede],
			PH.[IndirizzoSede],
			PH.[Regione],
			PH.[Provincia],
			PH.[Comune],
			PH.[Settore],
			PH.[Area],
			PH.[NumeroPostiDisponibili],
			PH.[Gruppo],
			PH.[Misure],
			PH.[DurataProgettoMesi],
			PH.[NumeroGiovaniMinoriOpportunità],
			PH.[EsteroUE],
			PH.[Tutoraggio],
			NULL --DataAnnullamento
		FROM
			#Progetto PH LEFT JOIN
			SUSCN_DOL_PROGETTI_DISPONIBILI P ON
					P.CodiceProgetto = PH.CodiceProgetto
				AND P.CodiceSede = PH.CodiceSede
		WHERE
			P.CodiceProgetto IS NULL
		PRINT 'Progetti Inseriti'

		IF OBJECT_ID('tempdb..#ProgettoDaModificare') IS NOT NULL DROP TABLE #ProgettoDaModificare

		SELECT
			*
		INTO
			#ProgettoDaModificare
		FROM
			#Progetto
		EXCEPT
		SELECT
			[CodiceEnte],
			[NomeEnte],
			[Sito],
			[CodiceProgetto],
			[TitoloProgetto],
			[TipoProgetto],
			[CodiceSede],
			[IndirizzoSede],
			[Regione],
			[Provincia],
			[Comune],
			[Settore],
			[Area],
			[NumeroPostiDisponibili],
			[Gruppo],
			[Misure],
			[DurataProgettoMesi],
			[NumeroGiovaniMinoriOpportunità],
			[EsteroUE],
			[Tutoraggio]
		FROM
			SUSCN_DOL_PROGETTI_DISPONIBILI P
		WHERE
			P.DataAnnullamento IS NULL

		UPDATE
			P
		SET
			P.CodiceEnte=PH.CodiceEnte,
			P.NomeEnte=PH.NomeEnte,
			P.Sito=PH.Sito,
			P.TitoloProgetto=PH.TitoloProgetto,
			P.TipoProgetto=PH.TipoProgetto,
			P.IndirizzoSede=PH.IndirizzoSede,
			P.Regione=PH.Regione,
			P.Provincia=PH.Provincia,
			P.Comune=PH.Comune,
			P.Settore=PH.Settore,
			P.Area=PH.Area,
			P.NumeroPostiDisponibili=PH.NumeroPostiDisponibili,
			P.Gruppo=PH.Gruppo,
			P.Misure=PH.Misure,
			P.DurataProgettoMesi=PH.DurataProgettoMesi,
			P.NumeroGiovaniMinoriOpportunità=PH.NumeroGiovaniMinoriOpportunità,
			P.EsteroUE=PH.EsteroUE,
			P.Tutoraggio=PH.Tutoraggio
		FROM
			SUSCN_DOL_PROGETTI_DISPONIBILI P JOIN
			#ProgettoDaModificare PH ON
					P.CodiceProgetto = PH.CodiceProgetto
				AND P.CodiceSede = PH.CodiceSede
		PRINT 'Progetti Aggiornati'


		COMMIT TRAN ScritturaDati
	END TRY

	BEGIN CATCH
		IF(@@TRANCOUNT > 0)
			ROLLBACK TRAN;
		THROW
	END CATCH
