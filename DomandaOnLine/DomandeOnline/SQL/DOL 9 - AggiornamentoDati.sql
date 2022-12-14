USE [DomandaOnline]
GO
/****** Object:  StoredProcedure [dbo].[SP_AggiornamentoDati]    Script Date: 21/12/2020 12:23:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[SP_AggiornamentoDati]
AS

	PRINT 'Lettura Dati'
	BEGIN TRAN
		IF OBJECT_ID('tempdb..#Progetto') IS NOT NULL DROP TABLE #Progetto
		IF OBJECT_ID('tempdb..#Bando') IS NOT NULL DROP TABLE #Bando
		IF OBJECT_ID('tempdb..#CodiceFiscale') IS NOT NULL DROP TABLE #CodiceFiscale
		IF OBJECT_ID('tempdb..#Geografico') IS NOT NULL DROP TABLE #Geografico
		IF OBJECT_ID('tempdb..#Programma') IS NOT NULL DROP TABLE #Programma
		IF OBJECT_ID('tempdb..#Obiettivo') IS NOT NULL DROP TABLE #Obiettivo
		IF OBJECT_ID('tempdb..#ObiettivoProgramma') IS NOT NULL DROP TABLE #ObiettivoProgramma
		IF OBJECT_ID('tempdb..#Ambito') IS NOT NULL DROP TABLE #Ambito
		IF OBJECT_ID('tempdb..#MinoreOpportunita') IS NOT NULL DROP TABLE #MinoreOpportunita
		IF OBJECT_ID('tempdb..#GaranziaGiovani') IS NOT NULL DROP TABLE #GaranziaGiovani
		IF OBJECT_ID('tempdb..#VolontariCovid') IS NOT NULL DROP TABLE #VolontariCovid
	   
		SELECT
			CodiceEnte,
			NomeEnte,
			Sito,
			CodiceProgetto,
			TitoloProgetto,
			TipoProgetto,
			CodiceSede,
			IndirizzoSede,
			Regione,
			Provincia,
			Comune,
			Settore,
			Area,
			NumeroPostiDisponibili,
			Gruppo,
			Misure,
			DurataProgettoMesi,
			NumeroGiovaniMinoriOpportunità,
			EsteroUE,
			Tutoraggio,
			IDParticolaritàEntità,
			IdProgramma,
			SintesiProgetto
		INTO
			#Progetto
		FROM
			SQLSUSCN.unscproduzione.dbo.VW_DOL_PROGETTI_DISPONIBILI

		SELECT
			Gruppo,
			Descrizione,
			Scadenza,
			DataScadenza,
			DataScadenzaGraduatorie,
			GiorniPostScadenza
		INTO
			#Bando
		FROM
			SQLSUSCN.unscproduzione.dbo.VW_DOL_BANDOGRUPPO
	
		SELECT
			CodiceFiscale,
			DataInizioServizio,
			DataFineServizio,
			TipoServizio
		INTO
			#CodiceFiscale
		FROM
			SQLSUSCN.unscproduzione.dbo.VW_DOL_VOLONTARI_SERVIZIO_EFFETTUATO
		
		SELECT
			CodiceFiscale
		Into
			#VolontariCovid
		FROM
			SQLSUSCN.unscproduzione.dbo.VW_DOL_VOLONTARI_INTERRUZIONE_COVID


		SELECT
			REGIONE,
			PROVINCIA,
			COMUNE
		INTO
			#Geografico
		FROM
			SQLSUSCN.unscproduzione.dbo.VW_DOL_GEOGRAFICO_ITALIA
			
		SELECT
			IdProgramma,
			Titolo,
			IdAmbitoAzione,
			IdTipoGG
		INTO
			#Programma
		FROM SQLSUSCN.unscproduzione.dbo.programmi

		Select 
			IdProgramma,
			IdObiettivo
		INTO
			#ObiettivoProgramma
		FROM SQLSUSCN.unscproduzione.dbo.programmiObiettivi

		Select 
			IdObiettivo,
			Obiettivo,
			Descrizione
		INTO
			#Obiettivo
		FROM
			SQLSUSCN.unscproduzione.dbo.Obiettivi

		SELECT
			IdAmbitoAzione,
			Descrizione
		INTO
			#Ambito
		FROM
			SQLSUSCN.unscproduzione.dbo.AmbitiAzione

		SELECT
			IDParticolarità,
			Descrizione
		INTO
			#MinoreOpportunita
		FROM
			SQLSUSCN.unscproduzione.dbo.ParticolaritàEntità
		WHERE
			Macrotipo='GMO' AND
			Attivo =1

		SELECT
			IdTipoGG,
			Regione,
			Descrizione
		INTO
			#GaranziaGiovani
		FROM
			SQLSUSCN.unscproduzione.dbo.tipiGG


	COMMIT 		
	PRINT 'Fine Lettura Dati'
	BEGIN TRAN
	BEGIN TRY
		DELETE SUSCN_DOL_CODICE_FISCALE_VOLONTARIO
		INSERT INTO SUSCN_DOL_CODICE_FISCALE_VOLONTARIO
			Select 
				CodiceFiscale,
				DataInizioServizio,
				DataFineServizio,
				TipoServizio
			FROM #CodiceFiscale
		PRINT 'Codici Fiscali aggiornati'

		DELETE SUSCN_DOL_VOLONTARIO_INTERRUZIONE_COVID
		INSERT INTO SUSCN_DOL_VOLONTARIO_INTERRUZIONE_COVID
			Select 
				CodiceFiscale
			FROM #VolontariCovid
		PRINT 'Codici Fiscali Interruzione Covid aggiornati'


		DELETE SUSCN_DOL_GEOGRAFICO_ITALIA
		INSERT INTO SUSCN_DOL_GEOGRAFICO_ITALIA
		SELECT
			REGIONE,
			PROVINCIA,
			COMUNE
		FROM #Geografico
		PRINT 'Comuni Aggiornati'




		UPDATE O
		SET
			O.IdObiettivo=OH.IdObiettivo,
			O.Obiettivo=OH.Obiettivo,
			O.Descrizione=OH.Descrizione
		FROM
			#Obiettivo OH JOIN
			SUSCN_DOL_OBIETTIVO O ON
				O.IdObiettivo=OH.IdObiettivo
		PRINT 'Obiettivi Aggiornati'


		INSERT INTO SUSCN_DOL_OBIETTIVO 
		SELECT 
			IdObiettivo,
			Obiettivo,
			Descrizione
		FROM
			#Obiettivo
		WHERE
			IdObiettivo NOT IN
			(SELECT
				IdObiettivo
			FROM
				SUSCN_DOL_OBIETTIVO)
		PRINT 'Obiettivi Inseriti'



		UPDATE A
		SET
			A.IdAmbitoAzione=AH.IdAmbitoAzione,
			A.Descrizione=AH.Descrizione
		FROM
			#Ambito A JOIN
			SUSCN_DOL_AMBITO AH ON
				 A.IdAmbitoAzione=AH.IdAmbitoAzione
		PRINT 'Ambiti Aggiornati'

		INSERT INTO SUSCN_DOL_AMBITO 
		SELECT
			IdAmbitoAzione,
			Descrizione			
		FROM
			#Ambito
		WHERE
			IdAmbitoAzione NOT IN
			(SELECT
				IdAmbitoAzione
			FROM
				SUSCN_DOL_AMBITO
			)
		PRINT 'Ambiti Inseriti'

		UPDATE 
			G
		SET
			G.Descrizione=GA.Descrizione,
			G.Regione=G.Regione
		FROM
			SUSCN_DOL_GARANZIA_GIOVANI G JOIN
			#GaranziaGiovani GA ON
				G.IdTipoGG=GA.IdTipoGG
		PRINT 'Garanzie Giovani Aggiornate'

		INSERT INTO SUSCN_DOL_GARANZIA_GIOVANI
		SELECT
			IdTipoGG,
			Regione,
			Descrizione
		FROM
			#GaranziaGiovani
		WHERE
			IdTipoGG NOT IN
			(SELECT
				IdTipoGG
			FROM
				SUSCN_DOL_GARANZIA_GIOVANI)
		PRINT 'Garanzie Giovani Inseriti'

		UPDATE P
		SET
			P.IdProgramma=PH.IdProgramma,
			P.Titolo=PH.Titolo,
			P.IdAmbitoAzione=PH.IdAmbitoAzione,
			P.IdTipoGG=PH.IdTipoGG
		FROM
			(SELECT
				IdProgramma,
				Titolo,
				IdAmbitoAzione,
				IdTipoGG
			FROM
				#Programma
			EXCEPT 
			SELECT
				IdProgramma,
				Titolo,
				IdAmbitoAzione,
				IdTipoGG			
			FROM 
				SUSCN_DOL_PROGRAMMA
			)PH 
			JOIN
			SUSCN_DOL_PROGRAMMA P ON
			 P.IdProgramma=PH.IdProgramma
		PRINT 'Programmi Aggiornati'

		
		INSERT INTO SUSCN_DOL_PROGRAMMA
		SELECT
			P.IdProgramma,
			P.Titolo,
			P.IdAmbitoAzione,
			P.IdTipoGG
		FROM
			#Programma P
		WHERE P.IdProgramma
			NOT IN
			(SELECT 
				IdProgramma
			FROM
				SUSCN_DOL_PROGRAMMA)
		PRINT 'Nuovi Programmi Inseriti'

		DELETE SUSCN_DOL_OBIETTIVO_PROGRAMMA
		INSERT INTO SUSCN_DOL_OBIETTIVO_PROGRAMMA
		SELECT
			IdProgramma,
			IdObiettivo
		FROM
			#ObiettivoProgramma
		PRINT 'Obiettivi Programmi Aggiornati'


		UPDATE 
			M
		SET
			M.Descrizione=MA.Descrizione
		FROM
			SUSCN_DOL_MINORE_OPPORTUNITA M JOIN
			#MinoreOpportunita MA ON
				M.IDParticolarità=MA.IDParticolarità
		PRINT 'Minori Opportunità Aggiornate'


		INSERT INTO SUSCN_DOL_MINORE_OPPORTUNITA
		SELECT 
			IDParticolarità,
			Descrizione
		FROM 
			#MinoreOpportunita
		WHERE IDParticolarità NOT IN
		(SELECT
			IDParticolarità
		FROM
			SUSCN_DOL_MINORE_OPPORTUNITA)
		PRINT 'Minori Opportunità Inserite'



		/*** Inserimento Bando ****/
		INSERT INTO SUSCN_DOL_BANDOGRUPPO
		SELECT
			GH.Gruppo,
			GH.Descrizione,
			GH.Scadenza,
			GH.DataScadenza,
			GH.DataScadenzaGraduatorie,
			GH.GiorniPostScadenza
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
			(SELECT 
				Gruppo,
				Descrizione,
				Scadenza,
				DataScadenza,
				DataScadenzaGraduatorie,
				GiorniPostScadenza
			FROM
				#Bando
			EXCEPT 
			SELECT 
				Gruppo,
				Descrizione,
				Scadenza,
				DataScadenza,
				DataScadenzaGraduatorie,
				GiorniPostScadenza			
			FROM 
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
			NULL, --DataAnnullamento
			PH.IDParticolaritàEntità,
			PH.IdProgramma,
			PH.SintesiProgetto
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
			CodiceEnte,
			NomeEnte,
			Sito,
			CodiceProgetto,
			TitoloProgetto,
			TipoProgetto,
			CodiceSede,
			IndirizzoSede,
			Regione,
			Provincia,
			Comune,
			Settore,
			Area,
			NumeroPostiDisponibili,
			Gruppo,
			Misure,
			DurataProgettoMesi,
			NumeroGiovaniMinoriOpportunità,
			EsteroUE,
			Tutoraggio,
			IDParticolaritàEntità,
			IdProgramma,
			SintesiProgetto
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
			[Tutoraggio],
			IDParticolaritàEntità,
			IdProgramma,
			LinkSintesi
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
			P.Tutoraggio=PH.Tutoraggio,
			P.IDParticolaritàEntità=PH.IDParticolaritàEntità,
			P.IdProgramma=PH.IdProgramma,
			P.LinkSintesi=PH.SintesiProgetto
		FROM
			SUSCN_DOL_PROGETTI_DISPONIBILI P JOIN
			#ProgettoDaModificare PH ON
					P.CodiceProgetto = PH.CodiceProgetto
				AND P.CodiceSede = PH.CodiceSede
		PRINT 'Progetti Aggiornati'


	COMMIT
	END TRY

	BEGIN CATCH
		IF(@@TRANCOUNT > 0)
			ROLLBACK;
		THROW
	END CATCH
