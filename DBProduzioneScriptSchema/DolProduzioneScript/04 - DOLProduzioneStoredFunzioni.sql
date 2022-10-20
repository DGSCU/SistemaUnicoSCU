USE [DomandaOnline]
GO
/****** Object:  StoredProcedure [dbo].[rientratiConSpid]    Script Date: 12/08/2022 15:43:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
Elenco degli utenti che hanno fatto richiesta di credenziali, queste sono state rifiutate o annullate, quindi sono rientrati con SPID
*/
create procedure [dbo].[rientratiConSpid]
as
BEGIN
	set nocount on

	select familyName,name,fiscalNumber FROM SQLDATI.SPID.[dbo].[authenticated]
	where RIGHT(fiscalNumber,16) in 
	(select CodiceFiscale  from RichiestaCredenziali
	where IdStato>2 and LEN(codicefiscale)=16)
END
GO
/****** Object:  StoredProcedure [dbo].[riepiloghiDOL]    Script Date: 12/08/2022 15:43:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--USE DomandaOnline
-- =============================================
-- Author:		<Celestino>
-- Create date: <23/12/2020>
-- Description:	<Riepilogo domande on line> aggiornato il <28/12/2020> - modificato nomi campi e ordinamento
-- =============================================

CREATE procedure [dbo].[riepiloghiDOL]
@bando int=63
as
BEGIN

	DROP TABLE IF EXISTS #Inserite
	DROP TABLE IF EXISTS #Presentate
	DROP TABLE IF EXISTS #Annullate

	SELECT
		CAST(DataInserimento as date) [Data],
		COUNT(*) [Domande Inserite],
		COUNT(CASE WHEN U.Spidcode IS NULL THEN 1 END) [Domande Inserite Credenziali]
	INTO #Inserite
	FROM 
		DomandaPartecipazione D JOIN
		AspNetUsers U ON U.Id=D.UserIdInserimento
	WHERE
		GruppoBando = @bando 
	GROUP BY
		CAST(DataInserimento as date)
	SELECT
		CAST(DataPresentazione as date) [Data],
		COUNT(*) [Domande Presentate],
		COUNT(CASE WHEN U.Spidcode IS NULL THEN 1 END) [Domande Presentate Credenziali]
	INTO #Presentate
	FROM 
		DomandaPartecipazione D JOIN
		AspNetUsers U ON U.Id=D.UserIdInserimento
	WHERE
		GruppoBando = @bando 
		and DataPresentazione IS NOT NULL
	GROUP BY
		CAST(DataPresentazione as date)
	SELECT
		CAST(DataRichiestaAnnullamento as date) [Data],
		COUNT(*) [Domande Annullate],
		COUNT(CASE WHEN U.Spidcode IS NULL THEN 1 END) [Domande Annullate Credenziali]
	INTO #Annullate
	FROM 
		DomandaPartecipazione D JOIN
		AspNetUsers U ON U.Id=D.UserIdInserimento
	WHERE
		GruppoBando = @bando 
		and DataRichiestaAnnullamento IS NOT NULL
	GROUP BY
		CAST(DataRichiestaAnnullamento as date)

	SELECT
		COALESCE(I.Data,P.data,A.Data) Data,
		COALESCE(I.[Domande Inserite],0) [Domande Inserite],
		COALESCE(I.[Domande Inserite Credenziali],0) [Domande Inserite Credenziali],
		COALESCE(P.[Domande Presentate],0) [Domande Presentate],
		COALESCE(P.[Domande Presentate Credenziali],0)[Domande Presentate Credenziali],
		COALESCE(A.[Domande Annullate],0) [Domande Annullate],
		COALESCE(A.[Domande Annullate Credenziali],0)[Domande Annullate Credenziali]
	FROM
		#Inserite I FULL JOIN
		#Presentate P ON P.Data=I.Data FULL JOIN
		#Annullate A ON A.Data=I.Data OR P.Data=A.Data
	ORDER BY COALESCE(I.Data,P.data,A.Data)
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

END
GO
/****** Object:  StoredProcedure [dbo].[riepilogo]    Script Date: 12/08/2022 15:43:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[riepilogo]
as
begin
	set nocount on
	select convert(varchar,dataReg,103) as data,isnull(NDomandeInserite,0) as NDomandeInserite, ISNULL(NDomandePresentate,0) NDomandePresentate,NAccessiSPID from
	(SELECT  count(IdTransResp) as NAccessiSPID, Convert(datetime, substring([Resp_IssueInstant],0,12) ,103) as dataReg
	  FROM SQLDATI.SPID.[dbo].[RegistoTran_Response]
	group by  Convert(datetime, substring([Resp_IssueInstant],0,12) ,103) ) as a left join 
	(SELECT count([Id]) NDomandeInserite,
		 CAST( DataInserimento AS DATE) as datains
	  FROM [dbo].[DomandaPartecipazione]
	   group by  CAST( DataInserimento AS DATE)) b on a.dataReg = b.datains left join
	(SELECT count([Id]) NDomandePresentate,CAST( DataPresentazione AS DATE) as datapresentazione
	  FROM [dbo].[VW_DomandaPresentata]  
	group by  CAST( DataPresentazione AS DATE)) as c on a.dataReg=c.datapresentazione
	where dataReg>'04/07/2019'
	order by dataReg
end
GO
/****** Object:  StoredProcedure [dbo].[riepilogo1]    Script Date: 12/08/2022 15:43:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[riepilogo1]
as
begin
	set nocount on
	--select convert(varchar,dataReg,103) as data,isnull(domandeInserite,0) as domandeInserite, ISNULL(InseriteConCredenziali,0) InseriteConCredenziali, ISNULL(domandePresentate,0) domandePresentate,ISNULL(PresentateConCredenziali,0) PresentateConCredenziali ,NAccessiSPID,isnull(approvati,0) CredenzialiApprovate from
	--(SELECT  count(IdTransResp) as NAccessiSPID, substring([Resp_IssueInstant],0,12)  as dataReg
	--  FROM SQLDATI.SPID.[dbo].[RegistoTran_Response]
	--group by  substring([Resp_IssueInstant],0,12) ) as a left join 
	--(select convert(varchar,a.DataPresentazione,103) as data,COUNT(DataPresentazione) as domandePresentate, SUM(isnull(n,0)) as PresentateConCredenziali from DomandaPartecipazione a left join 
	--(select CodiceFiscale,1 as n from RichiestaCredenziali where IdStato=2) b on a.CodiceFiscale=b.CodiceFiscale
	--where DataAnnullamento is null and not DataPresentazione is null
	--group by convert(varchar,a.DataPresentazione,103)) as b 
	-- on a.dataReg=b.data left join
	--(select convert(varchar,a.DataInserimento,103) as data,COUNT(DataInserimento) as domandeInserite, SUM(isnull(n,0)) as InseriteConCredenziali from DomandaPartecipazione a left join 
	--(select CodiceFiscale,1 as n from RichiestaCredenziali where IdStato=2) b on a.CodiceFiscale=b.CodiceFiscale
	--where DataAnnullamento is null 
	--group by convert(varchar,a.DataInserimento,103)) c on a.dataReg=c.data left join
	--(select convert(varchar,DataApprovazione,103) dataApprovazione,COUNT(*) approvati from RichiestaCredenziali
	--where IdStato=2
	--group by convert(varchar,DataApprovazione,103) ) d on a.dataReg=d.dataApprovazione
	--where convert(datetime,dataReg,103)>convert(datetime,'03/09/2019',103)
	--order by convert(datetime,dataReg,103)
-- Aggiornata by Cele 09/01/2019 14:13
	select convert(varchar,dataReg,103) as data,isnull(domandeInserite,0) as domandeInserite, ISNULL(InseriteConCredenziali,0) InseriteConCredenziali, ISNULL(domandePresentate,0) domandePresentate,ISNULL(PresentateConCredenziali,0) PresentateConCredenziali ,NAccessiSPID,isnull(approvati,0) CredenzialiApprovate from
	(SELECT  count(IdTransResp) as NAccessiSPID, substring([Resp_IssueInstant],0,12)  as dataReg
	  FROM SQLDATI.SPID.[dbo].[RegistoTran_Response]
	group by  substring([Resp_IssueInstant],0,12) ) as a left join 

	(select convert(varchar,a.DataPresentazione,103) as data,COUNT(DataPresentazione) as domandePresentate, SUM(b.spidUsr) as PresentateConCredenziali from DomandaPartecipazione a left join 
	(select id,case when Spidcode is null  then 1 else 0 END as spidUsr from  AspNetUsers) b on a.UserIdPresentazione=b.id
	where DataAnnullamento is null and not DataPresentazione is null
	group by convert(varchar,a.DataPresentazione,103)) as b 
	 on a.dataReg=b.data left join

	(select convert(varchar,a.DataInserimento,103) as data,COUNT(DataInserimento) as domandeInserite, SUM(isnull(n,0)) as InseriteConCredenziali from DomandaPartecipazione a left join 
	(select id,case when Spidcode is null  then 1 else 0 END as n from  AspNetUsers) b on a.UserIdInserimento=b.id
	where DataAnnullamento is null 
	group by convert(varchar,a.DataInserimento,103)) c on a.dataReg=c.data left join
	(select convert(varchar,DataApprovazione,103) dataApprovazione,COUNT(*) approvati from RichiestaCredenziali
	where IdStato=2
	group by convert(varchar,DataApprovazione,103) ) d on a.dataReg=d.dataApprovazione
	where convert(datetime,dataReg,103)>convert(datetime,'01/07/2020',103)
	order by convert(datetime,dataReg,103)

end
GO
/****** Object:  StoredProcedure [dbo].[SP_AggiornamentoDati]    Script Date: 12/08/2022 15:43:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[SP_AggiornamentoDati]
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
			SintesiProgetto,
			EnteAttuatore
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
			GiorniPostScadenza,
			Programmi
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
			CodiceFiscale,
			AnnoInterruzione
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
			IdTipoGG,
			IsDigitale,
			IsAmbientale,
			IsAutofinanziato
		INTO
			#Programma
		FROM SQLSUSCN.unscproduzione.dbo.VW_DOL_programmi

		Select 
			IdProgramma,
			IdObiettivo
		INTO
			#ObiettivoProgramma
		FROM SQLSUSCN.unscproduzione.dbo.VW_DOL_programmiObiettivi

		Select 
			IdObiettivo,
			Obiettivo,
			Descrizione
		INTO
			#Obiettivo
		FROM
			SQLSUSCN.unscproduzione.dbo.VW_DOL_Obiettivi

		SELECT
			IdAmbitoAzione,
			Descrizione
		INTO
			#Ambito
		FROM
			SQLSUSCN.unscproduzione.dbo.VW_DOL_AmbitiAzione

		SELECT
			IDParticolarità,
			Descrizione
		INTO
			#MinoreOpportunita
		FROM
			SQLSUSCN.unscproduzione.dbo.VW_DOL_ParticolaritàEntità
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
			SQLSUSCN.unscproduzione.dbo.VW_DOL_tipiGG


	COMMIT 		
	PRINT 'Fine Lettura Dati'
	BEGIN TRAN
	BEGIN TRY
		DELETE SUSCN_DOL_CODICE_FISCALE_VOLONTARIO
		INSERT INTO SUSCN_DOL_CODICE_FISCALE_VOLONTARIO(
				CodiceFiscale,
				DataInizioServizio,
				DataFineServizio,
				TipoServizio)
			Select 
				CodiceFiscale,
				DataInizioServizio,
				DataFineServizio,
				TipoServizio
			FROM #CodiceFiscale
		PRINT 'Codici Fiscali aggiornati'

		DELETE SUSCN_DOL_VOLONTARIO_INTERRUZIONE_COVID
		INSERT INTO SUSCN_DOL_VOLONTARIO_INTERRUZIONE_COVID(
			CodiceFiscale,
			AnnoInterruzione
		 )
			Select 
				CodiceFiscale,
				AnnoInterruzione
			FROM #VolontariCovid
		PRINT 'Codici Fiscali Interruzione Covid aggiornati'


		DELETE SUSCN_DOL_GEOGRAFICO_ITALIA
		INSERT INTO SUSCN_DOL_GEOGRAFICO_ITALIA(
			REGIONE,
			PROVINCIA,
			COMUNE)
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


		INSERT INTO SUSCN_DOL_OBIETTIVO(
			IdObiettivo,
			Obiettivo,
			Descrizione)
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

		INSERT INTO SUSCN_DOL_AMBITO(
			IdAmbitoAzione,
			Descrizione)
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

		INSERT INTO SUSCN_DOL_GARANZIA_GIOVANI(
			IdTipoGG,
			Regione,
			Descrizione)
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
			P.IdTipoGG=PH.IdTipoGG,
			P.IsDigitale=PH.IsDigitale,
			P.IsAmbientale=PH.IsAmbientale,
			P.IsAutofinanziato=PH.IsAutofinanziato
		FROM
			(SELECT
				IdProgramma,
				Titolo,
				IdAmbitoAzione,
				IdTipoGG,
				IsDigitale,
				IsAmbientale,
				IsAutofinanziato
			FROM
				#Programma
			EXCEPT 
			SELECT
				IdProgramma,
				Titolo,
				IdAmbitoAzione,
				IdTipoGG,
				IsDigitale,
				IsAmbientale,
				IsAutofinanziato
			FROM 
				SUSCN_DOL_PROGRAMMA
			)PH 
			JOIN
			SUSCN_DOL_PROGRAMMA P ON
			 P.IdProgramma=PH.IdProgramma
		PRINT 'Programmi Aggiornati'

		
		INSERT INTO SUSCN_DOL_PROGRAMMA(
			IdProgramma,
			Titolo,
			IdAmbitoAzione,
			IdTipoGG,
			IsDigitale,
			IsAmbientale,
			IsAutofinanziato)
		SELECT
			P.IdProgramma,
			P.Titolo,
			P.IdAmbitoAzione,
			P.IdTipoGG,
			P.IsDigitale,
			P.IsAmbientale,
			P.IsAutofinanziato
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
		INSERT INTO SUSCN_DOL_OBIETTIVO_PROGRAMMA(
			IdProgramma,
			IdObiettivo)
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


		INSERT INTO SUSCN_DOL_MINORE_OPPORTUNITA(
			IDParticolarità,
			Descrizione
		)
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
		INSERT INTO SUSCN_DOL_BANDOGRUPPO (
			Gruppo,
			Descrizione,
			Scadenza,
			DataScadenza,
			DataScadenzaGraduatorie,
			GiorniPostScadenza,
			Programmi
		)
		SELECT
			GH.Gruppo,
			GH.Descrizione,
			GH.Scadenza,
			GH.DataScadenza,
			GH.DataScadenzaGraduatorie,
			GH.GiorniPostScadenza,
			GH.Programmi
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
		/*Storico*/
		INSERT INTO SUSCN_DOL_STORICO_PROGETTI
		SELECT
			GetDate(),
			P.[CodiceEnte],
			P.[NomeEnte],
			P.[Sito],
			P.[CodiceProgetto],
			P.[TitoloProgetto],
			P.[TipoProgetto],
			P.[CodiceSede],
			P.[IndirizzoSede],
			P.[Regione],
			P.[Provincia],
			P.[Comune],
			P.[Settore],
			P.[Area],
			P.[NumeroPostiDisponibili],
			P.[Gruppo],
			P.[Misure],
			P.[DurataProgettoMesi],
			P.[NumeroGiovaniMinoriOpportunità],
			P.[EsteroUE],
			P.[Tutoraggio],
			P.DataAnnullamento,
			P.IDParticolaritàEntità,
			P.IdProgramma,
			P.LinkSintesi,
			P.EnteAttuatore
		FROM
			SUSCN_DOL_PROGETTI_DISPONIBILI P LEFT JOIN
			#Progetto PH ON
					P.CodiceProgetto = PH.CodiceProgetto
				AND P.CodiceSede = PH.CodiceSede
		WHERE
				P.DataAnnullamento IS NULL
			AND	P.Gruppo IN (Select Gruppo from #Bando)
			AND PH.CodiceProgetto IS NULL

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
		/** Storico **/
		INSERT INTO SUSCN_DOL_STORICO_PROGETTI
		SELECT
			GetDate(),
			P.[CodiceEnte],
			P.[NomeEnte],
			P.[Sito],
			P.[CodiceProgetto],
			P.[TitoloProgetto],
			P.[TipoProgetto],
			P.[CodiceSede],
			P.[IndirizzoSede],
			P.[Regione],
			P.[Provincia],
			P.[Comune],
			P.[Settore],
			P.[Area],
			P.[NumeroPostiDisponibili],
			P.[Gruppo],
			P.[Misure],
			P.[DurataProgettoMesi],
			P.[NumeroGiovaniMinoriOpportunità],
			P.[EsteroUE],
			P.[Tutoraggio],
			P.DataAnnullamento,
			P.IDParticolaritàEntità,
			P.IdProgramma,
			P.LinkSintesi,
			P.EnteAttuatore
		FROM
			SUSCN_DOL_PROGETTI_DISPONIBILI P JOIN
			#Progetto PH ON
					P.CodiceProgetto = PH.CodiceProgetto
				AND P.CodiceSede = PH.CodiceSede
		WHERE
				P.DataAnnullamento IS NOT NULL
			AND	P.Gruppo IN (Select Gruppo from #Bando)
		/*Aggiornamento*/
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
		PRINT 'Progetti Dis-Annullati'

		/*Storico*/
		INSERT INTO SUSCN_DOL_STORICO_PROGETTI
		SELECT
			GetDate(),
			P.[CodiceEnte],
			P.[NomeEnte],
			P.[Sito],
			P.[CodiceProgetto],
			P.[TitoloProgetto],
			P.[TipoProgetto],
			P.[CodiceSede],
			P.[IndirizzoSede],
			P.[Regione],
			P.[Provincia],
			P.[Comune],
			P.[Settore],
			P.[Area],
			P.[NumeroPostiDisponibili],
			P.[Gruppo],
			P.[Misure],
			P.[DurataProgettoMesi],
			P.[NumeroGiovaniMinoriOpportunità],
			P.[EsteroUE],
			P.[Tutoraggio],
			P.DataAnnullamento,
			P.IDParticolaritàEntità,
			P.IdProgramma,
			P.LinkSintesi,
			P.EnteAttuatore
		FROM
			SUSCN_DOL_PROGETTI_DISPONIBILI P LEFT JOIN
			#Progetto PH ON
					P.CodiceProgetto = PH.CodiceProgetto
				AND P.CodiceSede = PH.CodiceSede
		WHERE
				P.DataAnnullamento IS NULL
			AND	P.Gruppo IN (Select Gruppo from #Bando)
			AND PH.CodiceProgetto IS NULL

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
			PH.SintesiProgetto,
			PH.EnteAttuatore,
			0
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
			SintesiProgetto,
			EnteAttuatore
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
			LinkSintesi,
			EnteAttuatore
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
			P.LinkSintesi=PH.SintesiProgetto,
			P.EnteAttuatore=PH.EnteAttuatore
		FROM
			SUSCN_DOL_PROGETTI_DISPONIBILI P JOIN
			#ProgettoDaModificare PH ON
					P.CodiceProgetto = PH.CodiceProgetto
				AND P.CodiceSede = PH.CodiceSede
		PRINT 'Progetti Aggiornati'

		INSERT INTO
			SUSCN_DOL_STORICO_PROGETTI
		SELECT
			GetDate(),
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
			null,
			P.IDParticolaritàEntità,
			P.IdProgramma,
			P.SintesiProgetto,
			P.EnteAttuatore
		FROM
			#ProgettoDaModificare P

	COMMIT
	END TRY

	BEGIN CATCH
		IF(@@TRANCOUNT > 0)
			ROLLBACK;
		THROW
	END CATCH
GO
/****** Object:  StoredProcedure [dbo].[SP_AggiornamentoDati_bk20220802]    Script Date: 12/08/2022 15:43:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[SP_AggiornamentoDati_bk20220802]
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
			SintesiProgetto,
			EnteAttuatore
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
			GiorniPostScadenza,
			Programmi
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
			CodiceFiscale,
			AnnoInterruzione
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
			IdTipoGG,
			IsDigitale
		INTO
			#Programma
		FROM SQLSUSCN.unscproduzione.dbo.VW_DOL_programmi

		Select 
			IdProgramma,
			IdObiettivo
		INTO
			#ObiettivoProgramma
		FROM SQLSUSCN.unscproduzione.dbo.VW_DOL_programmiObiettivi

		Select 
			IdObiettivo,
			Obiettivo,
			Descrizione
		INTO
			#Obiettivo
		FROM
			SQLSUSCN.unscproduzione.dbo.VW_DOL_Obiettivi

		SELECT
			IdAmbitoAzione,
			Descrizione
		INTO
			#Ambito
		FROM
			SQLSUSCN.unscproduzione.dbo.VW_DOL_AmbitiAzione

		SELECT
			IDParticolarità,
			Descrizione
		INTO
			#MinoreOpportunita
		FROM
			SQLSUSCN.unscproduzione.dbo.VW_DOL_ParticolaritàEntità
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
			SQLSUSCN.unscproduzione.dbo.VW_DOL_tipiGG


	COMMIT 		
	PRINT 'Fine Lettura Dati'
	BEGIN TRAN
	BEGIN TRY
		DELETE SUSCN_DOL_CODICE_FISCALE_VOLONTARIO
		INSERT INTO SUSCN_DOL_CODICE_FISCALE_VOLONTARIO(
				CodiceFiscale,
				DataInizioServizio,
				DataFineServizio,
				TipoServizio)
			Select 
				CodiceFiscale,
				DataInizioServizio,
				DataFineServizio,
				TipoServizio
			FROM #CodiceFiscale
		PRINT 'Codici Fiscali aggiornati'

		DELETE SUSCN_DOL_VOLONTARIO_INTERRUZIONE_COVID
		INSERT INTO SUSCN_DOL_VOLONTARIO_INTERRUZIONE_COVID(
			CodiceFiscale,
			AnnoInterruzione
		 )
			Select 
				CodiceFiscale,
				AnnoInterruzione
			FROM #VolontariCovid
		PRINT 'Codici Fiscali Interruzione Covid aggiornati'


		DELETE SUSCN_DOL_GEOGRAFICO_ITALIA
		INSERT INTO SUSCN_DOL_GEOGRAFICO_ITALIA(
			REGIONE,
			PROVINCIA,
			COMUNE)
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


		INSERT INTO SUSCN_DOL_OBIETTIVO(
			IdObiettivo,
			Obiettivo,
			Descrizione)
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

		INSERT INTO SUSCN_DOL_AMBITO(
			IdAmbitoAzione,
			Descrizione)
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

		INSERT INTO SUSCN_DOL_GARANZIA_GIOVANI(
			IdTipoGG,
			Regione,
			Descrizione)
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
			P.IdTipoGG=PH.IdTipoGG,
			P.IsDigitale=PH.IsDigitale
		FROM
			(SELECT
				IdProgramma,
				Titolo,
				IdAmbitoAzione,
				IdTipoGG,
				IsDigitale
			FROM
				#Programma
			EXCEPT 
			SELECT
				IdProgramma,
				Titolo,
				IdAmbitoAzione,
				IdTipoGG,
				IsDigitale
			FROM 
				SUSCN_DOL_PROGRAMMA
			)PH 
			JOIN
			SUSCN_DOL_PROGRAMMA P ON
			 P.IdProgramma=PH.IdProgramma
		PRINT 'Programmi Aggiornati'

		
		INSERT INTO SUSCN_DOL_PROGRAMMA(
			IdProgramma,
			Titolo,
			IdAmbitoAzione,
			IdTipoGG,
			IsDigitale)
		SELECT
			P.IdProgramma,
			P.Titolo,
			P.IdAmbitoAzione,
			P.IdTipoGG,
			P.IsDigitale
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
		INSERT INTO SUSCN_DOL_OBIETTIVO_PROGRAMMA(
			IdProgramma,
			IdObiettivo)
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


		INSERT INTO SUSCN_DOL_MINORE_OPPORTUNITA(
			IDParticolarità,
			Descrizione
		)
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
		INSERT INTO SUSCN_DOL_BANDOGRUPPO (
			Gruppo,
			Descrizione,
			Scadenza,
			DataScadenza,
			DataScadenzaGraduatorie,
			GiorniPostScadenza,
			Programmi
		)
		SELECT
			GH.Gruppo,
			GH.Descrizione,
			GH.Scadenza,
			GH.DataScadenza,
			GH.DataScadenzaGraduatorie,
			GH.GiorniPostScadenza,
			GH.Programmi
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
		/*Storico*/
		INSERT INTO SUSCN_DOL_STORICO_PROGETTI
		SELECT
			GetDate(),
			P.[CodiceEnte],
			P.[NomeEnte],
			P.[Sito],
			P.[CodiceProgetto],
			P.[TitoloProgetto],
			P.[TipoProgetto],
			P.[CodiceSede],
			P.[IndirizzoSede],
			P.[Regione],
			P.[Provincia],
			P.[Comune],
			P.[Settore],
			P.[Area],
			P.[NumeroPostiDisponibili],
			P.[Gruppo],
			P.[Misure],
			P.[DurataProgettoMesi],
			P.[NumeroGiovaniMinoriOpportunità],
			P.[EsteroUE],
			P.[Tutoraggio],
			P.DataAnnullamento,
			P.IDParticolaritàEntità,
			P.IdProgramma,
			P.LinkSintesi,
			P.EnteAttuatore
		FROM
			SUSCN_DOL_PROGETTI_DISPONIBILI P LEFT JOIN
			#Progetto PH ON
					P.CodiceProgetto = PH.CodiceProgetto
				AND P.CodiceSede = PH.CodiceSede
		WHERE
				P.DataAnnullamento IS NULL
			AND	P.Gruppo IN (Select Gruppo from #Bando)
			AND PH.CodiceProgetto IS NULL

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
		/** Storico **/
		INSERT INTO SUSCN_DOL_STORICO_PROGETTI
		SELECT
			GetDate(),
			P.[CodiceEnte],
			P.[NomeEnte],
			P.[Sito],
			P.[CodiceProgetto],
			P.[TitoloProgetto],
			P.[TipoProgetto],
			P.[CodiceSede],
			P.[IndirizzoSede],
			P.[Regione],
			P.[Provincia],
			P.[Comune],
			P.[Settore],
			P.[Area],
			P.[NumeroPostiDisponibili],
			P.[Gruppo],
			P.[Misure],
			P.[DurataProgettoMesi],
			P.[NumeroGiovaniMinoriOpportunità],
			P.[EsteroUE],
			P.[Tutoraggio],
			P.DataAnnullamento,
			P.IDParticolaritàEntità,
			P.IdProgramma,
			P.LinkSintesi,
			P.EnteAttuatore
		FROM
			SUSCN_DOL_PROGETTI_DISPONIBILI P JOIN
			#Progetto PH ON
					P.CodiceProgetto = PH.CodiceProgetto
				AND P.CodiceSede = PH.CodiceSede
		WHERE
				P.DataAnnullamento IS NOT NULL
			AND	P.Gruppo IN (Select Gruppo from #Bando)
		/*Aggiornamento*/
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
		PRINT 'Progetti Dis-Annullati'

		/*Storico*/
		INSERT INTO SUSCN_DOL_STORICO_PROGETTI
		SELECT
			GetDate(),
			P.[CodiceEnte],
			P.[NomeEnte],
			P.[Sito],
			P.[CodiceProgetto],
			P.[TitoloProgetto],
			P.[TipoProgetto],
			P.[CodiceSede],
			P.[IndirizzoSede],
			P.[Regione],
			P.[Provincia],
			P.[Comune],
			P.[Settore],
			P.[Area],
			P.[NumeroPostiDisponibili],
			P.[Gruppo],
			P.[Misure],
			P.[DurataProgettoMesi],
			P.[NumeroGiovaniMinoriOpportunità],
			P.[EsteroUE],
			P.[Tutoraggio],
			P.DataAnnullamento,
			P.IDParticolaritàEntità,
			P.IdProgramma,
			P.LinkSintesi,
			P.EnteAttuatore
		FROM
			SUSCN_DOL_PROGETTI_DISPONIBILI P LEFT JOIN
			#Progetto PH ON
					P.CodiceProgetto = PH.CodiceProgetto
				AND P.CodiceSede = PH.CodiceSede
		WHERE
				P.DataAnnullamento IS NULL
			AND	P.Gruppo IN (Select Gruppo from #Bando)
			AND PH.CodiceProgetto IS NULL

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
			PH.SintesiProgetto,
			PH.EnteAttuatore,
			0
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
			SintesiProgetto,
			EnteAttuatore
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
			LinkSintesi,
			EnteAttuatore
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
			P.LinkSintesi=PH.SintesiProgetto,
			P.EnteAttuatore=PH.EnteAttuatore
		FROM
			SUSCN_DOL_PROGETTI_DISPONIBILI P JOIN
			#ProgettoDaModificare PH ON
					P.CodiceProgetto = PH.CodiceProgetto
				AND P.CodiceSede = PH.CodiceSede
		PRINT 'Progetti Aggiornati'

		INSERT INTO
			SUSCN_DOL_STORICO_PROGETTI
		SELECT
			GetDate(),
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
			null,
			P.IDParticolaritàEntità,
			P.IdProgramma,
			P.SintesiProgetto,
			P.EnteAttuatore
		FROM
			#ProgettoDaModificare P

	COMMIT
	END TRY

	BEGIN CATCH
		IF(@@TRANCOUNT > 0)
			ROLLBACK;
		THROW
	END CATCH
GO
/****** Object:  StoredProcedure [dbo].[SP_AggiornamentoDati_Old]    Script Date: 12/08/2022 15:43:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_AggiornamentoDati_Old]
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
			SintesiProgetto,
			EnteAttuatore
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
			GiorniPostScadenza,
			Programmi
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
			CodiceFiscale,
			AnnoInterruzione
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
			IdTipoGG,
			IsDigitale
		INTO
			#Programma
		FROM SQLSUSCN.unscproduzione.dbo.VW_DOL_programmi

		Select 
			IdProgramma,
			IdObiettivo
		INTO
			#ObiettivoProgramma
		FROM SQLSUSCN.unscproduzione.dbo.VW_DOL_programmiObiettivi

		Select 
			IdObiettivo,
			Obiettivo,
			Descrizione
		INTO
			#Obiettivo
		FROM
			SQLSUSCN.unscproduzione.dbo.VW_DOL_Obiettivi

		SELECT
			IdAmbitoAzione,
			Descrizione
		INTO
			#Ambito
		FROM
			SQLSUSCN.unscproduzione.dbo.VW_DOL_AmbitiAzione

		SELECT
			IDParticolarità,
			Descrizione
		INTO
			#MinoreOpportunita
		FROM
			SQLSUSCN.unscproduzione.dbo.VW_DOL_ParticolaritàEntità
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
			SQLSUSCN.unscproduzione.dbo.VW_DOL_tipiGG


	COMMIT 		
	PRINT 'Fine Lettura Dati'
	BEGIN TRAN
	BEGIN TRY
		DELETE SUSCN_DOL_CODICE_FISCALE_VOLONTARIO
		INSERT INTO SUSCN_DOL_CODICE_FISCALE_VOLONTARIO(
				CodiceFiscale,
				DataInizioServizio,
				DataFineServizio,
				TipoServizio)
			Select 
				CodiceFiscale,
				DataInizioServizio,
				DataFineServizio,
				TipoServizio
			FROM #CodiceFiscale
		PRINT 'Codici Fiscali aggiornati'

		DELETE SUSCN_DOL_VOLONTARIO_INTERRUZIONE_COVID
		INSERT INTO SUSCN_DOL_VOLONTARIO_INTERRUZIONE_COVID(
			CodiceFiscale,
			AnnoInterruzione
		 )
			Select 
				CodiceFiscale,
				AnnoInterruzione
			FROM #VolontariCovid
		PRINT 'Codici Fiscali Interruzione Covid aggiornati'


		DELETE SUSCN_DOL_GEOGRAFICO_ITALIA
		INSERT INTO SUSCN_DOL_GEOGRAFICO_ITALIA(
			REGIONE,
			PROVINCIA,
			COMUNE)
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


		INSERT INTO SUSCN_DOL_OBIETTIVO(
			IdObiettivo,
			Obiettivo,
			Descrizione)
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

		INSERT INTO SUSCN_DOL_AMBITO(
			IdAmbitoAzione,
			Descrizione)
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

		INSERT INTO SUSCN_DOL_GARANZIA_GIOVANI(
			IdTipoGG,
			Regione,
			Descrizione)
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
			P.IdTipoGG=PH.IdTipoGG,
			P.IsDigitale=PH.IsDigitale
		FROM
			(SELECT
				IdProgramma,
				Titolo,
				IdAmbitoAzione,
				IdTipoGG,
				IsDigitale
			FROM
				#Programma
			EXCEPT 
			SELECT
				IdProgramma,
				Titolo,
				IdAmbitoAzione,
				IdTipoGG,
				IsDigitale
			FROM 
				SUSCN_DOL_PROGRAMMA
			)PH 
			JOIN
			SUSCN_DOL_PROGRAMMA P ON
			 P.IdProgramma=PH.IdProgramma
		PRINT 'Programmi Aggiornati'

		
		INSERT INTO SUSCN_DOL_PROGRAMMA(
			IdProgramma,
			Titolo,
			IdAmbitoAzione,
			IdTipoGG,
			IsDigitale)
		SELECT
			P.IdProgramma,
			P.Titolo,
			P.IdAmbitoAzione,
			P.IdTipoGG,
			P.IsDigitale
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
		INSERT INTO SUSCN_DOL_OBIETTIVO_PROGRAMMA(
			IdProgramma,
			IdObiettivo)
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


		INSERT INTO SUSCN_DOL_MINORE_OPPORTUNITA(
			IDParticolarità,
			Descrizione
		)
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
		INSERT INTO SUSCN_DOL_BANDOGRUPPO (
			Gruppo,
			Descrizione,
			Scadenza,
			DataScadenza,
			DataScadenzaGraduatorie,
			GiorniPostScadenza,
			Programmi
		)
		SELECT
			GH.Gruppo,
			GH.Descrizione,
			GH.Scadenza,
			GH.DataScadenza,
			GH.DataScadenzaGraduatorie,
			GH.GiorniPostScadenza,
			GH.Programmi
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
		/*Storico*/
		INSERT INTO SUSCN_DOL_STORICO_PROGETTI
		SELECT
			GetDate(),
			P.[CodiceEnte],
			P.[NomeEnte],
			P.[Sito],
			P.[CodiceProgetto],
			P.[TitoloProgetto],
			P.[TipoProgetto],
			P.[CodiceSede],
			P.[IndirizzoSede],
			P.[Regione],
			P.[Provincia],
			P.[Comune],
			P.[Settore],
			P.[Area],
			P.[NumeroPostiDisponibili],
			P.[Gruppo],
			P.[Misure],
			P.[DurataProgettoMesi],
			P.[NumeroGiovaniMinoriOpportunità],
			P.[EsteroUE],
			P.[Tutoraggio],
			P.DataAnnullamento,
			P.IDParticolaritàEntità,
			P.IdProgramma,
			P.LinkSintesi,
			P.EnteAttuatore
		FROM
			SUSCN_DOL_PROGETTI_DISPONIBILI P LEFT JOIN
			#Progetto PH ON
					P.CodiceProgetto = PH.CodiceProgetto
				AND P.CodiceSede = PH.CodiceSede
		WHERE
				P.DataAnnullamento IS NULL
			AND	P.Gruppo IN (Select Gruppo from #Bando)
			AND PH.CodiceProgetto IS NULL

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
		/** Storico **/
		INSERT INTO SUSCN_DOL_STORICO_PROGETTI
		SELECT
			GetDate(),
			P.[CodiceEnte],
			P.[NomeEnte],
			P.[Sito],
			P.[CodiceProgetto],
			P.[TitoloProgetto],
			P.[TipoProgetto],
			P.[CodiceSede],
			P.[IndirizzoSede],
			P.[Regione],
			P.[Provincia],
			P.[Comune],
			P.[Settore],
			P.[Area],
			P.[NumeroPostiDisponibili],
			P.[Gruppo],
			P.[Misure],
			P.[DurataProgettoMesi],
			P.[NumeroGiovaniMinoriOpportunità],
			P.[EsteroUE],
			P.[Tutoraggio],
			P.DataAnnullamento,
			P.IDParticolaritàEntità,
			P.IdProgramma,
			P.LinkSintesi,
			P.EnteAttuatore
		FROM
			SUSCN_DOL_PROGETTI_DISPONIBILI P JOIN
			#Progetto PH ON
					P.CodiceProgetto = PH.CodiceProgetto
				AND P.CodiceSede = PH.CodiceSede
		WHERE
				P.DataAnnullamento IS NOT NULL
			AND	P.Gruppo IN (Select Gruppo from #Bando)
		/*Aggiornamento*/
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
		PRINT 'Progetti Dis-Annullati'

		/*Storico*/
		INSERT INTO SUSCN_DOL_STORICO_PROGETTI
		SELECT
			GetDate(),
			P.[CodiceEnte],
			P.[NomeEnte],
			P.[Sito],
			P.[CodiceProgetto],
			P.[TitoloProgetto],
			P.[TipoProgetto],
			P.[CodiceSede],
			P.[IndirizzoSede],
			P.[Regione],
			P.[Provincia],
			P.[Comune],
			P.[Settore],
			P.[Area],
			P.[NumeroPostiDisponibili],
			P.[Gruppo],
			P.[Misure],
			P.[DurataProgettoMesi],
			P.[NumeroGiovaniMinoriOpportunità],
			P.[EsteroUE],
			P.[Tutoraggio],
			P.DataAnnullamento,
			P.IDParticolaritàEntità,
			P.IdProgramma,
			P.LinkSintesi,
			P.EnteAttuatore
		FROM
			SUSCN_DOL_PROGETTI_DISPONIBILI P LEFT JOIN
			#Progetto PH ON
					P.CodiceProgetto = PH.CodiceProgetto
				AND P.CodiceSede = PH.CodiceSede
		WHERE
				P.DataAnnullamento IS NULL
			AND	P.Gruppo IN (Select Gruppo from #Bando)
			AND PH.CodiceProgetto IS NULL

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
			PH.SintesiProgetto,
			PH.EnteAttuatore
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
			SintesiProgetto,
			EnteAttuatore
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
			LinkSintesi,
			EnteAttuatore
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
			P.LinkSintesi=PH.SintesiProgetto,
			P.EnteAttuatore=PH.EnteAttuatore
		FROM
			SUSCN_DOL_PROGETTI_DISPONIBILI P JOIN
			#ProgettoDaModificare PH ON
					P.CodiceProgetto = PH.CodiceProgetto
				AND P.CodiceSede = PH.CodiceSede
		PRINT 'Progetti Aggiornati'

		INSERT INTO
			SUSCN_DOL_STORICO_PROGETTI
		SELECT
			GetDate(),
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
			null,
			P.IDParticolaritàEntità,
			P.IdProgramma,
			P.SintesiProgetto,
			P.EnteAttuatore
		FROM
			#ProgettoDaModificare P

	COMMIT
	END TRY

	BEGIN CATCH
		IF(@@TRANCOUNT > 0)
			ROLLBACK;
		THROW
	END CATCH
GO
/****** Object:  StoredProcedure [dbo].[SP_AnnullaDomanda]    Script Date: 12/08/2022 15:43:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_AnnullaDomanda](
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
GO
/****** Object:  StoredProcedure [dbo].[SP_AnnullaDomanda2]    Script Date: 12/08/2022 15:43:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_AnnullaDomanda2](
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
		DomandaPartecipazione D JOIN
		SUSCN_DOL_PROGETTI_DISPONIBILI P ON
			P.CodiceProgetto=D.CodiceProgettoSelezionato AND
			P.CodiceSede = D.CodiceSedeSelezionata
	WHERE 
		Id=@IdDomanda
GO
/****** Object:  StoredProcedure [dbo].[SP_DOL_REPORT_DOMANDE]    Script Date: 12/08/2022 15:43:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[SP_DOL_REPORT_DOMANDE]
AS
create table #finale
(Localizzazione varchar(100) null,
NumeroDomandeNAZ int null,
NumeroDomandeRPA int null,
TotaleNumeroDomande int null)


insert into #finale (Localizzazione,TotaleNumeroDomande)
select case f.ComuneNazionale when 1 then h.Regione else 'Estero' end as Localizzazione, COUNT(distinct a.id) as NumeroDomande 
from [sqldol].domandaonline.dbo.[VW_DomandaPresentata] a
inner join sqlsuscn.unscproduzione.dbo.attività b on a.codiceprogettoselezionato = b.CodiceEnte 
inner join sqlsuscn.unscproduzione.dbo.attivitàentisediattuazione c on b.IDAttività = c.IDAttività and a.codicesedeselezionata = c.IDEnteSedeAttuazione 
inner join sqlsuscn.unscproduzione.dbo.entisediattuazioni d on c.IDEnteSedeAttuazione = d.IDEnteSedeAttuazione 
inner join sqlsuscn.unscproduzione.dbo.entisedi e on d.IDEnteSede = e.IDEnteSede 
inner join sqlsuscn.unscproduzione.dbo.comuni f on e.IDComune = f.IDComune 
inner join sqlsuscn.unscproduzione.dbo.provincie g on f.IDProvincia = g.IDProvincia 
inner join sqlsuscn.unscproduzione.dbo.regioni h on g.IDRegione = h.IDRegione 
inner join sqlsuscn.unscproduzione.dbo.nazioni i on h.IDNazione = i.IDNazione 
inner join sqlsuscn.unscproduzione.dbo.enti l on b.IDEntePresentante = l.IDEnte 
--inner join classiaccreditamento m on l.IDClasseAccreditamento  = m.IDClasseAccreditamento 
--inner join RegioniCompetenze n on l.IdRegioneCompetenza = n.IdRegioneCompetenza 
--inner join RegioniCompetenze o on l.IdRegioneAppartenenza = o.IdRegioneCompetenza 
group by case f.ComuneNazionale when 1 then h.Regione else 'Estero' end 
order by 1

select case f.ComuneNazionale when 1 then h.Regione else 'Estero' end as Localizzazione, COUNT(distinct a.id) as NumeroDomandeNAZ 
into #naz
from [sqldol].domandaonline.dbo.[VW_DomandaPresentata] a
inner join sqlsuscn.unscproduzione.dbo.attività b on a.codiceprogettoselezionato = b.CodiceEnte 
inner join sqlsuscn.unscproduzione.dbo.attivitàentisediattuazione c on b.IDAttività = c.IDAttività and a.codicesedeselezionata = c.IDEnteSedeAttuazione 
inner join sqlsuscn.unscproduzione.dbo.entisediattuazioni d on c.IDEnteSedeAttuazione = d.IDEnteSedeAttuazione 
inner join sqlsuscn.unscproduzione.dbo.entisedi e on d.IDEnteSede = e.IDEnteSede 
inner join sqlsuscn.unscproduzione.dbo.comuni f on e.IDComune = f.IDComune 
inner join sqlsuscn.unscproduzione.dbo.provincie g on f.IDProvincia = g.IDProvincia 
inner join sqlsuscn.unscproduzione.dbo.regioni h on g.IDRegione = h.IDRegione 
inner join sqlsuscn.unscproduzione.dbo.nazioni i on h.IDNazione = i.IDNazione 
inner join sqlsuscn.unscproduzione.dbo.enti l on b.IDEntePresentante = l.IDEnte 
--inner join classiaccreditamento m on l.IDClasseAccreditamento  = m.IDClasseAccreditamento 
--inner join RegioniCompetenze n on l.IdRegioneCompetenza = n.IdRegioneCompetenza 
--inner join RegioniCompetenze o on l.IdRegioneAppartenenza = o.IdRegioneCompetenza 
where l.IdRegioneCompetenza = 22
group by case f.ComuneNazionale when 1 then h.Regione else 'Estero' end 

update #finale set NumeroDomandeNAZ = b.NumeroDomandeNAZ
from #finale a 
left join #naz b on a.Localizzazione = b.Localizzazione

select case f.ComuneNazionale when 1 then h.Regione else 'Estero' end as Localizzazione, COUNT(distinct a.id) as NumeroDomandeRPA 
into #rpa
from [sqldol].domandaonline.dbo.[VW_DomandaPresentata] a
inner join sqlsuscn.unscproduzione.dbo.attività b on a.codiceprogettoselezionato = b.CodiceEnte 
inner join sqlsuscn.unscproduzione.dbo.attivitàentisediattuazione c on b.IDAttività = c.IDAttività and a.codicesedeselezionata = c.IDEnteSedeAttuazione 
inner join sqlsuscn.unscproduzione.dbo.entisediattuazioni d on c.IDEnteSedeAttuazione = d.IDEnteSedeAttuazione 
inner join sqlsuscn.unscproduzione.dbo.entisedi e on d.IDEnteSede = e.IDEnteSede 
inner join sqlsuscn.unscproduzione.dbo.comuni f on e.IDComune = f.IDComune 
inner join sqlsuscn.unscproduzione.dbo.provincie g on f.IDProvincia = g.IDProvincia 
inner join sqlsuscn.unscproduzione.dbo.regioni h on g.IDRegione = h.IDRegione 
inner join sqlsuscn.unscproduzione.dbo.nazioni i on h.IDNazione = i.IDNazione 
inner join sqlsuscn.unscproduzione.dbo.enti l on b.IDEntePresentante = l.IDEnte 
--inner join classiaccreditamento m on l.IDClasseAccreditamento  = m.IDClasseAccreditamento 
--inner join RegioniCompetenze n on l.IdRegioneCompetenza = n.IdRegioneCompetenza 
--inner join RegioniCompetenze o on l.IdRegioneAppartenenza = o.IdRegioneCompetenza 
where l.IdRegioneCompetenza <> 22
group by case f.ComuneNazionale when 1 then h.Regione else 'Estero' end 

update #finale set NumeroDomandeRPA = b.NumeroDomanderpa
from #finale a 
left join #rpa b on a.Localizzazione = b.Localizzazione

update #finale set NumeroDomandeNAZ = isnull(NumeroDomandeNAZ,0),NumeroDomandeRPA = isnull(NumeroDomandeRPA,0)
select * from #finale WHERE Localizzazione <> 'Estero'
select * from #finale WHERE Localizzazione = 'Estero'
				
GO
