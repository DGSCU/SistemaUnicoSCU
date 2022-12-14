USE [unscsviluppo]
GO
/****** Object:  StoredProcedure [dbo].[SP_REGISTRAZIONE_ACQUISIZIONE]    Script Date: 03/11/2021 09:53:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



ALTER PROCEDURE [dbo].[SP_REGISTRAZIONE_ACQUISIZIONE]
			@IdRegistrazione int = 0
	        ,@DataInserimento datetime = NULL
			,@CodiceFiscaleEnte varchar(50) 
			,@CodiceFiscaleLegaleRappresentante varchar(50)
			,@Denominazione varchar(200) = ''
			,@NomeLegaleRappresentante varchar(255)
			,@CognomeLegaleRappresentante varchar(255)
			,@DataNascitaLegaleRappresentante datetime
			,@ComuneNascitaLegaleRappresentante varchar(50)
			,@DataNominaRappresentanteLegale datetime = NULL
			,@EnteTitolare bit 
			,@IdCategoriaEnte int = NULL
			,@IdTipologiaEnte int
			,@IdProvinciaEnte int
			,@IdComuneEnte int
			,@Via varchar(255)
			,@Civico varchar(50)
			,@CAP varchar(50)
			,@Telefono varchar(60)
			,@Email varchar(100)
			,@PEC varchar(100)
			,@Sito varchar(100)
			,@DichiarazionePrivacy bit
			,@DichiarazioneRappresentanteLegale bit
			,@IdDocumento int
			,@VariazioneRappresentanteLegale bit
			,@DataProtocollazione datetime
			,@NumeroProtocollo varchar(50)
			,@DataProtocollo datetime
			,@DataInvioEmail datetime
			,@IdDocumentoNomina int = null
			,@Albo varchar(3) = 'SCU'
            ,@Esito tinyint output			-- Esito aggiornamento: 0-Errore 1-Aggiornamento effettuato
            ,@messaggio varchar(1000) output	-- eventuale messaggio di ritorno da visualizzare all'utente

AS
/*
Creata il:		26/04/2021
Data Ultima Modifica:	--
Funzionalità: 
SP lanciata da REGISTRAZIONE per
	- caso 1. ente titolare esistente. cf legale esiste. creazione nuovo utente legale rappresentante per ente
		CONDIZIONI. @EnteTitolare = 1
					@CodiceFiscaleEnte = Codice Fiscale esistente di ente titolare. 
					@CodiceFiscaleLegaleRappresentante = Codice Fiscale di risorsa con ruolo responsabile legale per l'ente 					
	- caso 2. nuovo ente titolare. creazione ente, creare legale rappresentante, creare utenza
		CONDIZIONI. @EnteTitolare = 1
					NON esiste ente titolare con codice fiscale = @CodiceFiscaleEnte
	- caso 3. ente di accoglienza. cf legale esiste. creazione nuovo utente legale rappresentante per ente (profilo ad hoc)
		CONDIZIONI. @EnteTitolare = 0
					@CodiceFiscaleLegaleRappresentante = RLEntiAccoglienza.[RLCodiceFiscale]
	- caso 4. ente titolare esistente. cf legale non esiste. creare nuovo legale rappresentante. creare nuova utenza legale rappresentante. 
		CONDIZIONI. @EnteTitolare = 1
					@CodiceFiscaleEnte = Codice Fiscale esistente di ente titolare. 
					@CodiceFiscaleLegaleRappresentante <> Codice Fiscale di risorsa con ruolo responsabile legale per l'ente 					
*/ 

--variabili locali
declare @Caso tinyint = 0
		,@IdEnteTitolare int = 0
		,@IdEnteAccoglienza int = 0
		,@IdLegaleRappresentante int = 0
		,@Username varchar(50) = '0000000000'
		,@IdComuneNascita int
		,@IdEntePersonale int = 0
		,@Tipologia nvarchar(255) = NULL
		,@Privato bit = NULL
		,@IdEnteSede int
		,@Prefisso varchar(4)
		,@IdAllegato int = NULL
	SET @Messaggio = ''
	SET @Esito = 1

	
BEGIN TRY

IF @Albo = 'SCU'
BEGIN
	--individuo caso
	IF @EnteTitolare = 1
	BEGIN
		SELECT 
			@Tipologia = Descrizione,
			@Privato = Privato
		FROM
			TipologieEnti
		WHERE 
			IdTipologieEnti=@IdTipologiaEnte

		SET @Prefisso=SUBSTRING(@Telefono,1,2)
		SET @Telefono=SUBSTRING(@Telefono,3,100)

		select @IdEnteTitolare = a.IDEnte from enti a 
				where a.CodiceFiscale = @CodiceFiscaleEnte 
					and a.IdClasseAccreditamentoRichiesta in (8,9) and IDStatoEnte in (3,6,8,9)
		IF @IdEnteTitolare = 0
			set @Caso = 2
		ELSE
			BEGIN	
				select @IdLegaleRappresentante = b.IDEntePersonale from enti a
				inner join entepersonale b on a.IDEnte = b.IDEnte
				inner join entepersonaleruoli c on b.IDEntePersonale = c.IDEntePersonale
				where a.idente = @IdEnteTitolare
					and b.CodiceFiscale = @CodiceFiscaleLegaleRappresentante
					and c.IDRuolo = 4
					and b.DataFineValidità is null and c.DataFineValidità is null 
					and c.Accreditato in (0,1)

				IF @IdLegaleRappresentante = 0
					SET @Caso = 4
				ELSE
					SET @Caso = 1
			END
	END

	IF @EnteTitolare = 0 AND EXISTS(SELECT a.IDRLEntiAccoglienza 
										FROM RLEntiAccoglienza a 
										WHERE a.RLCodiceFiscale = @CodiceFiscaleLegaleRappresentante 
											AND a.CodiceFiscaleEnte = @CodiceFiscaleEnte and Visibile = 1)
	BEGIN
		SET @Caso = 3
	END
	IF @VariazioneRappresentanteLegale =1 and @EnteTitolare = 1
		AND EXISTS (select a.IDEnte from enti a where a.CodiceFiscale = @CodiceFiscaleEnte) --AGGIUNTA PER RISOLUZIONE PROBLEMA COMUNE DI MARSCIANO
	BEGIN
		SET @Caso = 5
	END
	IF @Caso = 0
	BEGIN
		SET @Esito = 0
		SET @messaggio = 'Caso non previsto. Nessuna operazione effettuata'
	END

	--INIZIO AGGIORNAMENTI
	IF @CASO = 1 
	BEGIN
		--creazione nuovo utente legale rappresentante per ente
		SET @Username = 'ELT' + RIGHT(@Username + CONVERT(VARCHAR(50),@IdLegaleRappresentante),7)

		IF NOT EXISTS (SELECT USERNAME FROM EntiPassword WHERE Username = @Username)
		BEGIN
			insert into EntiPassword (idente,Username,Abilitato,HeliosRead,DurataPassword,CambioPassword,PasswordDaInviare,DataModificaPassword,IdEntePersonale)
			values (@IdEnteTitolare,@Username,1,0,180,0,0,GETDATE(),@IdLegaleRappresentante)

			INSERT INTO AssociaUtenteGruppo (UserName,IdProfilo)
			SELECT @Username, VALORE FROM Configurazioni WHERE Parametro = 'DEFAULT_PROFILO_ELT'
		END
		PRINT '1'

			IF @IdDocumentoNomina IS NOT NULL
			BEGIN

				INSERT INTO EntiDocumenti(
					IdEnteFase,
					BinData,
					FileName,
					DataInserimento,
					UsernameInserimento,
					HashValue,
					IdTipoAllegato)
				SELECT
					NULL,	--NULL IN QUESTO CASO
					Blob,
					'NOMINARL_'+@CodiceFiscaleEnte+dbo.FN_Estensione_File(NomeFile),
					GETDATE(),
					'ADMIN',
					Hash,
					1
				FROM Registrazione.dbo.Documento
				WHERE id=@IdDocumentoNomina
				
				SET @IdAllegato = SCOPE_IDENTITY()
				UPDATE
					Enti
				SET
					IdAllegatoDocumentoNomina=@IdAllegato
				WHERE
					IdEnte=@IdEnteTitolare
			END

	END
	IF @Caso = 2
	BEGIN
		--Controllo esistenza ente
		IF EXISTS (Select CodiceFiscale from VW_REGISTRAZIONE_CF_ENTI_IMPEGNATI Where CodiceFiscale=@CodiceFiscaleEnte)
		BEGIN
			SET @Esito = 0
			SET @messaggio = 'Codice fiscale ente già presente'
			RETURN
		END
		IF EXISTS (Select Denominazione from VW_REGISTRAZIONE_CF_ENTI_IMPEGNATI Where Denominazione=@Denominazione)
		BEGIN
			SET @Esito = 0
			SET @messaggio = 'Denominazione ente già presente'
			RETURN
		END

		--nuovo ente titolare. 
		--creazione ente, creare legale rappresentante, creare utenza
		--1. CREAZIONE ENTE
			--print '1. CREAZIONE ENTE'
			Insert into enti 
				(albo,denominazione,email,EmailCertificata,noterichiestaregistrazione,datacreazionerecord
				,Telefonorichiestaregistrazione,prefissoTelefonorichiestaregistrazione,codicefiscale
				,IdRegioneAppartenenza,IdRegioneCompetenza,DataRicezioneCartacea,Tipologia,DataNominaRL,Http)
			values ('SCU',@Denominazione,@Email,@PEC, @CognomeLegaleRappresentante + ' ' + @NomeLegaleRappresentante,getdate()
					,ISNULL(@Telefono,''),ISNULL(@Prefisso,''),@CodiceFiscaleEnte
				,null,null,getdate(),@Tipologia,@DataNominaRappresentanteLegale,@Sito)

			SET @IdEnteTitolare = SCOPE_IDENTITY()

			Insert into CronologiaEntiStati (idente,idstatoEnte,datacronologia,idtipocronologia)
			values (@IdEnteTitolare,4,getdate(),0) 

			Insert into CronologiaMailEnti(idente,NuovaEmail, NuovaPEC, Username, DataModifica) 
			VALUES (@IdEnteTitolare,@Email,@PEC,'ADMIN', getdate())

			Update enti set idstatoente=6, FlagForzaturaAccreditamento=1 where idente=@IdEnteTitolare

			/*Simone Curti - Aggiunto sede legale*/
			INSERT INTO entisedi (IDEnte,Denominazione,Indirizzo,Civico,IDComune,CAP,PrefissoTelefono,Telefono,Http,Email,IDStatoEnteSede,DataCreazioneRecord)
			VALUES(@IdEnteTitolare,'SEDE PRINC. - ' + coalesce(@Denominazione, ''),@via,@Civico,@IdComuneEnte,@CAP,'06',@Telefono,@sito,@Email,4,GETDATE())
			SET @IdEnteSede= SCOPE_IDENTITY()
			INSERT INTO entiseditipi VALUES(@IdEnteSede,1)



			declare @appouserente varchar(50) = '0000000000'
			set @appouserente = 'E96' + RIGHT(@appouserente + CONVERT(VARCHAR(50),@IdEnteTitolare+7218),7)

			insert into entipassword 
				(IdEnte, Username, Password, Password1, 
				DataModificaPassword, DurataPassword, CambioPassword, PasswordDaInviare) 
			values (@IdEnteTitolare,@appouserente,'','', getdate(),180,1,0) 

			insert into AssociaUtenteGruppo(username,idprofilo) 
			select  @appouserente ,idprofilo from Profili where abilitato=1 and Tipo='E' and defaultTipo=1

			Insert into CronologiaEntiStati 
				(idente, idstatoEnte,datacronologia,note,idtipocronologia,UsernameAccreditatore)
			values (@IdEnteTitolare,4,getdate(),'', 0,'ADMIN')

			Insert into CronologiaPasswordEnti (Username, Password, DataCronologia) 
			values (@appouserente,'', getdate())

			Insert into EntiFasi (idente, tipofase, datainiziofase, datafinefase, stato,UserNameInizioFase) 
			select @IdEnteTitolare, 1, getdate(), DATEADD(S,-1,DATEADD(D,convert(int,(select valore from configurazioni where parametro = 'durata_accr'))+1,DBO.FORMATODATADT(GetDate()))), 1, 'ADMIN'
			
			DECLARE @IdEnteFase int = scope_identity()

			insert into EntiFasiAntimafia(IdEnte,TipoFase,DataInizioFase,DataFineFase,Stato,ScadenzaNotificata,DataNotifica,UsernameInizioFase,UsernameFineFase,UsernameValutazione,DataValutazione,IdEnteFaseRiferimento,InLavorazione,UltimaEsportazioneDati,DataChiusuraFase)
				SELECT ef.IdEnte, TipoFase, DataInizioFase, DataFineFase, Stato, ScadenzaNotificata, DataNotifica, UsernameInizioFase, UsernameFineFase, UsernameValutazione, DataValutazione, @IdEnteFase, InLavorazione, null, null
				FROM EntiFasi ef where IdEnteFase=@IdEnteFase

			/*Simone Curti - Aggiunto Documento Nomina*/
			IF @IdDocumentoNomina IS NOT NULL
			BEGIN
				/*
				INSERT INTO Allegato 
				SELECT 
					@IdEnteTitolare,1,
					Blob,
					'NOMINARL_'+@CodiceFiscaleEnte+RIGHT(NomeFile, LEN(NomeFile)-charindex('.', NomeFile)+1),
					Hash,
					Dimensione,GETDATE(),
					'ADMIN',
					@IdEnteFase
				FROM Registrazione.dbo.Documento WHERE id=@IdDocumentoNomina
				*/
				INSERT INTO EntiDocumenti(
					IdEnteFase,
					BinData,
					FileName,
					DataInserimento,
					UsernameInserimento,
					HashValue,
					IdTipoAllegato)
				SELECT
					@IdEnteFase,
					Blob,
					'NOMINARL_'+@CodiceFiscaleEnte+dbo.FN_Estensione_File(NomeFile),
					GETDATE(),
					'ADMIN',
					Hash,
					1
				FROM Registrazione.dbo.Documento
				WHERE id=@IdDocumentoNomina
				
				SET @IdAllegato = SCOPE_IDENTITY()
				UPDATE
					Enti
				SET
					IdAllegatoDocumentoNomina=@IdAllegato
				WHERE
					IdEnte=@IdEnteTitolare
			END
				
		 --2. CREAZIONE LEGALE RAPPRESENTANTE
			--print '2. CREAZIONE LEGALE RAPPRESENTANTE'
			select @IdComuneNascita = IDComune from comuni where cf = @ComuneNascitaLegaleRappresentante and (CodiceISTAT is not null or CodiceIstatDismesso is not null)

			INSERT INTO entepersonale (IDENTE,Cognome,NOME,Abilitato,IDComuneNascita,DataNascita,CodiceFiscale,DataCreazioneRecord,UsernameInseritore,EsperienzaServizioCivile,Corso)
			VALUES (@IdEnteTitolare, @CognomeLegaleRappresentante,@NomeLegaleRappresentante,0,@IdComuneNascita,@DataNascitaLegaleRappresentante,@CodiceFiscaleLegaleRappresentante,GETDATE(),'ADMIN',0,0)

			SET @IdLegaleRappresentante = SCOPE_IDENTITY()

			INSERT INTO entepersonaleruoli (IDEntePersonale,IDRuolo,DataInizioValidità,ACCREDITATO,DataAccreditamento,UsernameAccreditatore,Principale,Visibilità,Forzatura,DataInseritore,UserNameInseritore)
			VALUES (@IdLegaleRappresentante,4,GETDATE(),1,GETDATE(),'ADMIN',1,0,0,GETDATE(),'ADMIN')

		--3. CREAZIONE UTENZA LEGALE RAPPRESENTANTE
			--print '3. CREAZIONE UTENZA LEGALE RAPPRESENTANTE'
			SET @Username = 'ELT' + RIGHT(@Username + CONVERT(VARCHAR(50),@IdLegaleRappresentante),7)
			--creazione nuovo utente legale rappresentante per ente
			insert into EntiPassword (idente,Username,Abilitato,HeliosRead,DurataPassword,CambioPassword,PasswordDaInviare,DataModificaPassword,IdEntePersonale)
			values (@IdEnteTitolare,@Username,1,0,180,0,0,GETDATE(),@IdLegaleRappresentante)

			INSERT INTO AssociaUtenteGruppo (UserName,IdProfilo)
			SELECT @Username, VALORE FROM Configurazioni WHERE Parametro = 'DEFAULT_PROFILO_ELT'

			PRINT '2'


	END
	IF @CASO = 3
	BEGIN
		--ente di accoglienza. cf legale abilitato. 
		--creazione nuovo utente legale rappresentante per ente (profilo ad hoc)
		 --1. CREAZIONE LEGALE RAPPRESENTANTE SU ENTE PERSONALE SE NON ESITE
			 SELECT @IdEnteAccoglienza = b.IDEnte,
					@IdEnteTitolare = a.IdEnteTitolare
											FROM RLEntiAccoglienza a 
											LEFT JOIN ENTI B on a.CodiceFiscaleEnte = b.CodiceFiscale
											WHERE a.RLCodiceFiscale = @CodiceFiscaleLegaleRappresentante 
												AND a.CodiceFiscaleEnte = @CodiceFiscaleEnte
												AND a.Visibile = 1
			IF @IdEnteAccoglienza IS NULL
			BEGIN
				IF EXISTS (Select CodiceFiscale from VW_REGISTRAZIONE_CF_ENTI_IMPEGNATI Where CodiceFiscale=@CodiceFiscaleEnte)
				BEGIN
					SET @Esito = 0
					SET @messaggio = 'Codice fiscale ente già presente'
					RETURN
				END
				IF EXISTS (Select Denominazione from VW_REGISTRAZIONE_CF_ENTI_IMPEGNATI Where Denominazione=@Denominazione)
				BEGIN
					SET @Esito = 0
					SET @messaggio = 'Denominazione ente già presente'
					RETURN
				END
				
						--1. CREAZIONE ENTE
				--print '1. CREAZIONE ENTE'
				SET @Prefisso=SUBSTRING(@Telefono,1,2)
				SET @Telefono=SUBSTRING(@Telefono,3,100)
				SELECT 
					@Tipologia = Descrizione
				FROM
					TipologieEnti
				WHERE 
					IdTipologieEnti=@IdTipologiaEnte
				Insert into enti 
					(albo,denominazione,email,EmailCertificata,noterichiestaregistrazione,datacreazionerecord
					,Telefonorichiestaregistrazione,prefissoTelefonorichiestaregistrazione,codicefiscale
					,IdRegioneAppartenenza,IdRegioneCompetenza,DataRicezioneCartacea,Tipologia,DataNominaRL,Http,CodiceFiscaleRL)
				values ('SCU',@Denominazione,@Email,@PEC, @CognomeLegaleRappresentante + ' ' + @NomeLegaleRappresentante,getdate()
						,@Telefono,@Prefisso,@CodiceFiscaleEnte
					,null,null,getdate(),@Tipologia,@DataNominaRappresentanteLegale,@Sito,@CodiceFiscaleLegaleRappresentante)
			
				SET @IdEnteAccoglienza = SCOPE_IDENTITY()

				Insert into CronologiaEntiStati (idente,idstatoEnte,datacronologia,idtipocronologia)
				values (@IdEnteAccoglienza,4,getdate(),0) 

				Insert into CronologiaMailEnti(idente,NuovaEmail, NuovaPEC, Username, DataModifica) 
				VALUES (@IdEnteAccoglienza,@Email,@PEC,'ADMIN', getdate())

				Update enti set idstatoente=6, FlagForzaturaAccreditamento=1,IdClasseAccreditamentoRichiesta=5 where idente=@IdEnteAccoglienza


				insert into entirelazioni (IDEntePadre,IDEnteFiglio,IDTipoRelazione,DataInizioValidità,DataFineValidità,DataStipula,DataScadenza)
				VALUES
				(@IdEnteTitolare,@IdEnteAccoglienza,1,GETDATE(),NULL,NULL,NULL )

				INSERT INTO entisedi (IDEnte,Denominazione,Indirizzo,Civico,IDComune,CAP,PrefissoTelefono,Telefono,Http,Email,IDStatoEnteSede,DataCreazioneRecord)
					VALUES(@IdEnteAccoglienza,'SEDE PRINC. - ' + coalesce(@Denominazione, ''),@via,@Civico,@IdComuneEnte,@CAP,'06',@Telefono,@sito,@Email,4,GETDATE())
					SET @IdEnteSede= SCOPE_IDENTITY()
					INSERT INTO entiseditipi VALUES(@IdEnteSede,1)
			END
			ELSE BEGIN --Simone Curti 03-11-2121 - Aggiunta modifica Dati Rappresentante legale per enti di accoglienza esistenti
				UPDATE enti SET 
					DataNominaRL=@DataNominaRappresentanteLegale,
					CodiceFiscaleRL=@CodiceFiscaleLegaleRappresentante
				WHERE
					IDEnte=@IdEnteAccoglienza
			END
			select @IdLegaleRappresentante = b.IDEntePersonale from enti a
			inner join entepersonale b on a.IDEnte = b.IDEnte
			inner join entepersonaleruoli c on b.IDEntePersonale = c.IDEntePersonale
			where a.idente = @IdEnteAccoglienza
				and b.CodiceFiscale = @CodiceFiscaleLegaleRappresentante
				and c.IDRuolo = 4
				and b.DataFineValidità is null and c.DataFineValidità is null 
				and c.Accreditato in (0,1)

			IF @IdLegaleRappresentante = 0
			BEGIN
				select @IdComuneNascita = IDComune from comuni where cf = @ComuneNascitaLegaleRappresentante and (CodiceISTAT is not null or CodiceIstatDismesso is not null)

				IF @IdComuneNascita IS NULL 
					SET @IdComuneNascita = 38715 --IMPOSTO DEFAULT COMUNE GENERICO (ITALIA) IN CASO DI COMUNE NON INDIVIDUATO DA CODICE BELFIORE

				INSERT INTO entepersonale (IDENTE,Cognome,NOME,Abilitato,IDComuneNascita,DataNascita,CodiceFiscale,DataCreazioneRecord,UsernameInseritore,EsperienzaServizioCivile,Corso)
				VALUES (@IdEnteAccoglienza, @CognomeLegaleRappresentante,@NomeLegaleRappresentante,0,@IdComuneNascita,@DataNascitaLegaleRappresentante,@CodiceFiscaleLegaleRappresentante,GETDATE(),'ADMIN',0,0)

				SET @IdLegaleRappresentante = SCOPE_IDENTITY()

				INSERT INTO entepersonaleruoli (IDEntePersonale,IDRuolo,DataInizioValidità,ACCREDITATO,DataAccreditamento,UsernameAccreditatore,Principale,Visibilità,Forzatura,DataInseritore,UserNameInseritore)
				VALUES (@IdLegaleRappresentante,4,GETDATE(),1,GETDATE(),'ADMIN',1,0,0,GETDATE(),'ADMIN')
			END

		--3. CREAZIONE UTENZA LEGALE RAPPRESENTANTE
			--print '3. CREAZIONE UTENZA LEGALE RAPPRESENTANTE'
			SET @Username = 'ELA' + RIGHT(@Username + CONVERT(VARCHAR(50),@IdLegaleRappresentante),7)
			--creazione nuovo utente legale rappresentante per ente ACCOGLIENZA (l'ente di riferimento è sempre il titolare)
			insert into EntiPassword (idente,Username,Abilitato,HeliosRead,DurataPassword,CambioPassword,PasswordDaInviare,DataModificaPassword,IdEntePersonale)
			values (@IdEnteAccoglienza,@Username,1,0,180,0,0,GETDATE(),@IdLegaleRappresentante)

			INSERT INTO AssociaUtenteGruppo (UserName,IdProfilo)
			SELECT @Username, VALORE FROM Configurazioni WHERE Parametro = 'DEFAULT_PROFILO_ELA'

			PRINT '3'
			IF @IdDocumentoNomina IS NOT NULL
			BEGIN

				INSERT INTO EntiDocumenti(
					IdEnteFase,
					BinData,
					FileName,
					DataInserimento,
					UsernameInserimento,
					HashValue,
					IdTipoAllegato)
				SELECT
					NULL,	--NULL IN QUESTO CASO
					Blob,
					'NOMINARL_'+@CodiceFiscaleEnte+dbo.FN_Estensione_File(NomeFile),
					GETDATE(),
					'ADMIN',
					Hash,
					1
				FROM Registrazione.dbo.Documento
				WHERE id=@IdDocumentoNomina
				
				SET @IdAllegato = SCOPE_IDENTITY()
				UPDATE
					Enti
				SET
					IdAllegatoDocumentoNomina=@IdAllegato
				WHERE
					IdEnte=@IdEnteTitolare
			END

	END
	IF @CASO = 4
	BEGIN
		--ente titolare esistente. cf legale non esiste. 
		--creare nuovo legale rappresentante. creare nuova utenza legale rappresentante. 
		
		--1. CREAZIONE LEGALE RAPPRESENTANTE 
		SELECT @IdEntePersonale = IDENTEPERSONALE FROM entepersonale 
		WHERE IDENTE = @IdEnteTitolare AND CodiceFiscale = @CodiceFiscaleLegaleRappresentante
			and datafinevalidità is null

		IF @IdEntePersonale = 0
			BEGIN
				--CF NON ESISTENTE PER ENTE
				select @IdComuneNascita = IDComune from comuni where cf = @ComuneNascitaLegaleRappresentante and (CodiceISTAT is not null or CodiceIstatDismesso is not null)

				IF @IdComuneNascita IS NULL 
					SET @IdComuneNascita = 38715 --IMPOSTO DEFAULT COMUNE GENERICO (ITALIA) IN CASO DI COMUNE NON INDIVIDUATO DA CODICE BELFIORE

				INSERT INTO entepersonale (IDENTE,Cognome,NOME,Abilitato,IDComuneNascita,DataNascita,CodiceFiscale,DataCreazioneRecord,UsernameInseritore,EsperienzaServizioCivile,Corso)
				VALUES (@IdEnteTitolare, @CognomeLegaleRappresentante,@NomeLegaleRappresentante,0,@IdComuneNascita,@DataNascitaLegaleRappresentante,@CodiceFiscaleLegaleRappresentante,GETDATE(),'ADMIN',0,0)

				SET @IdLegaleRappresentante = SCOPE_IDENTITY()

				INSERT INTO entepersonaleruoli (IDEntePersonale,IDRuolo,DataInizioValidità,ACCREDITATO,DataAccreditamento,UsernameAccreditatore,Principale,Visibilità,Forzatura,DataInseritore,UserNameInseritore)
				VALUES (@IdLegaleRappresentante,4,GETDATE(),1,GETDATE(),'ADMIN',1,0,0,GETDATE(),'ADMIN')
			END
		ELSE
			BEGIN
				--ESISTE CF PER L'ENTE AGGIUNGO SOLO RUOLO (stato accreditato per default)
				set @IdLegaleRappresentante = @IdEntePersonale

				INSERT INTO entepersonaleruoli (IDEntePersonale,IDRuolo,DataInizioValidità,ACCREDITATO,DataAccreditamento,UsernameAccreditatore,Principale,Visibilità,Forzatura,DataInseritore,UserNameInseritore)
				VALUES (@IdLegaleRappresentante,4,GETDATE(),1,GETDATE(),'ADMIN',1,0,0,GETDATE(),'ADMIN')
			END

		SET @Username = 'ELT' + RIGHT(@Username + CONVERT(VARCHAR(50),@IdLegaleRappresentante),7)
		--2. CREAZIONE UTENZA LEGALE RAPPRESENTANTE
		insert into EntiPassword (idente,Username,Abilitato,HeliosRead,DurataPassword,CambioPassword,PasswordDaInviare,DataModificaPassword,IdEntePersonale)
		values (@IdEnteTitolare,@Username,1,0,180,0,0,GETDATE(),@IdLegaleRappresentante)

		INSERT INTO AssociaUtenteGruppo (UserName,IdProfilo)
		SELECT @Username, VALORE FROM Configurazioni WHERE Parametro = 'DEFAULT_PROFILO_ELT'


		/*Simone Curti - Aggiunto Documento Nomina*/
		IF @IdDocumentoNomina IS NOT NULL
			BEGIN
				--INSERT INTO Allegato 
				--SELECT 
				--	@IdEnteTitolare,1,
				--	Blob,
				--	'NOMINARL_'+@CodiceFiscaleEnte+RIGHT(NomeFile, LEN(NomeFile)-charindex('.', NomeFile)+1),
				--	Hash,
				--	Dimensione,
				--	GETDATE(),
				--	'ADMIN',
				--	NULL
				--FROM Registrazione.dbo.Documento WHERE id=@IdDocumentoNomina

				INSERT INTO EntiDocumenti(
					IdEnteFase,
					BinData,
					FileName,
					DataInserimento,
					UsernameInserimento,
					HashValue,
					IdTipoAllegato)
				SELECT
					@IdEnteFase,
					Blob,
					'NOMINARL_'+@CodiceFiscaleEnte+dbo.FN_Estensione_File(NomeFile),
					GETDATE(),
					'ADMIN',
					Hash,
					1
				FROM Registrazione.dbo.Documento
				WHERE id=@IdDocumentoNomina

				SET @IdAllegato = SCOPE_IDENTITY()
				UPDATE
					Enti
				SET
					IdAllegatoDocumentoNomina=@IdAllegato
				WHERE
					IdEnte=@IdEnteTitolare
			END
	END
	--Variazione Rappresentante LEgale
	IF @CASO = 5
	BEGIN
		select @IdEnteTitolare = a.IDEnte from enti a 
				where a.CodiceFiscale = @CodiceFiscaleEnte 
					--CF NON ESISTENTE PER ENTE
				select @IdComuneNascita = IDComune from comuni where cf = @ComuneNascitaLegaleRappresentante and (CodiceISTAT is not null or CodiceIstatDismesso is not null)

		IF @IdComuneNascita IS NULL 
			SET @IdComuneNascita = 38715 --IMPOSTO DEFAULT COMUNE GENERICO (ITALIA) IN CASO DI COMUNE NON INDIVIDUATO DA CODICE BELFIORE

		INSERT INTO entepersonale (IDENTE,Cognome,NOME,Abilitato,IDComuneNascita,DataNascita,CodiceFiscale,DataCreazioneRecord,UsernameInseritore,EsperienzaServizioCivile,Corso)
		VALUES (@IdEnteTitolare, @CognomeLegaleRappresentante,@NomeLegaleRappresentante,0,@IdComuneNascita,@DataNascitaLegaleRappresentante,@CodiceFiscaleLegaleRappresentante,GETDATE(),'ADMIN',0,0)

		SET @IdLegaleRappresentante = SCOPE_IDENTITY()

		INSERT INTO entepersonaleruoli (IDEntePersonale,IDRuolo,DataInizioValidità,ACCREDITATO,DataAccreditamento,UsernameAccreditatore,Principale,Visibilità,Forzatura,DataInseritore,UserNameInseritore)
		VALUES (@IdLegaleRappresentante,4,GETDATE(),1,GETDATE(),'ADMIN',1,0,0,GETDATE(),'ADMIN')

		SET @Username = 'ELT' + RIGHT(@Username + CONVERT(VARCHAR(50),@IdLegaleRappresentante),7)
		--2. CREAZIONE UTENZA LEGALE RAPPRESENTANTE
		insert into EntiPassword (idente,Username,Abilitato,HeliosRead,DurataPassword,CambioPassword,PasswordDaInviare,DataModificaPassword,IdEntePersonale)
		values (@IdEnteTitolare,@Username,1,0,180,0,0,GETDATE(),@IdLegaleRappresentante)

		INSERT INTO AssociaUtenteGruppo (UserName,IdProfilo)
		SELECT @Username, VALORE FROM Configurazioni WHERE Parametro = 'DEFAULT_PROFILO_ELT'

		/*Simone Curti - Aggiunto Documento Nomina*/
		IF @IdDocumentoNomina IS NOT NULL
			BEGIN
				--INSERT INTO Allegato 
				--SELECT 
				--	@IdEnteTitolare,1,
				--	Blob,
				--	NomeFile,
				--	Hash,
				--	Dimensione,
				--	GETDATE(),
				--	'ADMIN',
				--	NULL
				--FROM Registrazione.dbo.Documento WHERE id=@IdDocumentoNomina
				--SET @IdAllegato = SCOPE_IDENTITY()
				INSERT INTO EntiDocumenti(
					IdEnteFase,
					BinData,
					FileName,
					DataInserimento,
					UsernameInserimento,
					HashValue,
					IdTipoAllegato)
				SELECT
					@IdEnteFase,
					Blob,
					'NOMINARL_'+@CodiceFiscaleEnte+dbo.FN_Estensione_File(NomeFile),
					GETDATE(),
					'ADMIN',
					Hash,
					1
				FROM Registrazione.dbo.Documento
				WHERE id=@IdDocumentoNomina
				UPDATE
					Enti
				SET
					IdAllegatoDocumentoNomina=@IdAllegato
				WHERE
					IdEnte=@IdEnteTitolare
			END
	END
END

IF @Albo = 'SCN'
BEGIN
/*
	- caso 1. ente titolare esistente. cf legale esiste. creazione nuovo utente legale rappresentante per ente
		CONDIZIONI. @EnteTitolare = 1
					@CodiceFiscaleEnte = Codice Fiscale esistente di ente titolare. 
					@CodiceFiscaleLegaleRappresentante = Codice Fiscale di risorsa con ruolo responsabile legale per l'ente 					
	- caso 4. ente titolare esistente. cf legale non esiste. creare nuovo legale rappresentante. creare nuova utenza legale rappresentante. 
		CONDIZIONI. @EnteTitolare = 1
					@CodiceFiscaleEnte = Codice Fiscale esistente di ente titolare. 
					@CodiceFiscaleLegaleRappresentante <> Codice Fiscale di risorsa con ruolo responsabile legale per l'ente 					
*/
	--individuo caso
	IF @EnteTitolare = 1
	BEGIN
		SELECT 
			@Tipologia = Descrizione,
			@Privato = Privato
		FROM
			TipologieEnti
		WHERE 
			IdTipologieEnti=@IdTipologiaEnte

		SET @Prefisso=SUBSTRING(@Telefono,1,2)
		SET @Telefono=SUBSTRING(@Telefono,3,100)

		select @IdEnteTitolare = a.IDEnte from enti a 
				where a.CodiceFiscaleArchivio = @CodiceFiscaleEnte 
					and a.IdClasseAccreditamentoRichiesta in (1,2,3,4) and IDStatoEnte in (10)
		IF @IdEnteTitolare = 0
			set @Caso = 0 --scn solo titolari in stato 10 (Ex SCN con Prog)
		ELSE
			BEGIN	
				select @IdLegaleRappresentante = b.IDEntePersonale from enti a
				inner join entepersonale b on a.IDEnte = b.IDEnte
				inner join entepersonaleruoli c on b.IDEntePersonale = c.IDEntePersonale
				where a.idente = @IdEnteTitolare
					and b.CodiceFiscale = @CodiceFiscaleLegaleRappresentante
					and c.IDRuolo = 4
					and b.DataFineValidità is null and c.DataFineValidità is null 
					and c.Accreditato in (0,1)

				IF @IdLegaleRappresentante = 0
					SET @Caso = 4
				ELSE
					SET @Caso = 1
			END
	END
	ELSE
		SET @Caso = 0

	IF @Caso = 0
	BEGIN
		SET @Esito = 0
		SET @messaggio = 'Caso non previsto. Nessuna operazione effettuata'
	END

	--INIZIO AGGIORNAMENTI
	IF @CASO = 1 
	BEGIN
		--creazione nuovo utente legale rappresentante per ente
		SET @Username = 'ELT' + RIGHT(@Username + CONVERT(VARCHAR(50),@IdLegaleRappresentante),7)

		IF NOT EXISTS (SELECT USERNAME FROM EntiPassword WHERE Username = @Username)
		BEGIN
			insert into EntiPassword (idente,Username,Abilitato,HeliosRead,DurataPassword,CambioPassword,PasswordDaInviare,DataModificaPassword,IdEntePersonale)
			values (@IdEnteTitolare,@Username,1,0,180,0,0,GETDATE(),@IdLegaleRappresentante)

			INSERT INTO AssociaUtenteGruppo (UserName,IdProfilo)
			SELECT @Username, VALORE FROM Configurazioni WHERE Parametro = 'DEFAULT_PROFILO_ELT'
		END
		PRINT '1'

			IF @IdDocumentoNomina IS NOT NULL
			BEGIN

				INSERT INTO EntiDocumenti(
					IdEnteFase,
					BinData,
					FileName,
					DataInserimento,
					UsernameInserimento,
					HashValue,
					IdTipoAllegato)
				SELECT
					NULL,	--NULL IN QUESTO CASO
					Blob,
					'NOMINARL_'+@CodiceFiscaleEnte+dbo.FN_Estensione_File(NomeFile),
					GETDATE(),
					'ADMIN',
					Hash,
					1
				FROM Registrazione.dbo.Documento
				WHERE id=@IdDocumentoNomina
				
				SET @IdAllegato = SCOPE_IDENTITY()
				UPDATE
					Enti
				SET
					IdAllegatoDocumentoNomina=@IdAllegato
				WHERE
					IdEnte=@IdEnteTitolare
			END

	END

	IF @CASO = 4 
	BEGIN
		--ente titolare esistente. cf legale non esiste. 
		--creare nuovo legale rappresentante. creare nuova utenza legale rappresentante. 
		
		--1. CREAZIONE LEGALE RAPPRESENTANTE 
		SELECT @IdEntePersonale = IDENTEPERSONALE FROM entepersonale 
		WHERE IDENTE = @IdEnteTitolare AND CodiceFiscale = @CodiceFiscaleLegaleRappresentante
			and datafinevalidità is null

		IF @IdEntePersonale = 0
			BEGIN
				--CF NON ESISTENTE PER ENTE
				select @IdComuneNascita = IDComune from comuni where cf = @ComuneNascitaLegaleRappresentante and (CodiceISTAT is not null or CodiceIstatDismesso is not null)

				IF @IdComuneNascita IS NULL 
					SET @IdComuneNascita = 38715 --IMPOSTO DEFAULT COMUNE GENERICO (ITALIA) IN CASO DI COMUNE NON INDIVIDUATO DA CODICE BELFIORE

				INSERT INTO entepersonale (IDENTE,Cognome,NOME,Abilitato,IDComuneNascita,DataNascita,CodiceFiscale,DataCreazioneRecord,UsernameInseritore,EsperienzaServizioCivile,Corso)
				VALUES (@IdEnteTitolare, @CognomeLegaleRappresentante,@NomeLegaleRappresentante,0,@IdComuneNascita,@DataNascitaLegaleRappresentante,@CodiceFiscaleLegaleRappresentante,GETDATE(),'ADMIN',0,0)

				SET @IdLegaleRappresentante = SCOPE_IDENTITY()

				INSERT INTO entepersonaleruoli (IDEntePersonale,IDRuolo,DataInizioValidità,ACCREDITATO,DataAccreditamento,UsernameAccreditatore,Principale,Visibilità,Forzatura,DataInseritore,UserNameInseritore)
				VALUES (@IdLegaleRappresentante,4,GETDATE(),1,GETDATE(),'ADMIN',1,0,0,GETDATE(),'ADMIN')
			END
		ELSE
			BEGIN
				--ESISTE CF PER L'ENTE AGGIUNGO SOLO RUOLO (stato accreditato per default)
				set @IdLegaleRappresentante = @IdEntePersonale

				INSERT INTO entepersonaleruoli (IDEntePersonale,IDRuolo,DataInizioValidità,ACCREDITATO,DataAccreditamento,UsernameAccreditatore,Principale,Visibilità,Forzatura,DataInseritore,UserNameInseritore)
				VALUES (@IdLegaleRappresentante,4,GETDATE(),1,GETDATE(),'ADMIN',1,0,0,GETDATE(),'ADMIN')
			END

		SET @Username = 'ELT' + RIGHT(@Username + CONVERT(VARCHAR(50),@IdLegaleRappresentante),7)
		--2. CREAZIONE UTENZA LEGALE RAPPRESENTANTE
		insert into EntiPassword (idente,Username,Abilitato,HeliosRead,DurataPassword,CambioPassword,PasswordDaInviare,DataModificaPassword,IdEntePersonale)
		values (@IdEnteTitolare,@Username,1,0,180,0,0,GETDATE(),@IdLegaleRappresentante)

		INSERT INTO AssociaUtenteGruppo (UserName,IdProfilo)
		SELECT @Username, VALORE FROM Configurazioni WHERE Parametro = 'DEFAULT_PROFILO_ELT'


		/*Simone Curti - Aggiunto Documento Nomina*/
		IF @IdDocumentoNomina IS NOT NULL
			BEGIN
				--INSERT INTO Allegato 
				--SELECT 
				--	@IdEnteTitolare,1,
				--	Blob,
				--	'NOMINARL_'+@CodiceFiscaleEnte+RIGHT(NomeFile, LEN(NomeFile)-charindex('.', NomeFile)+1),
				--	Hash,
				--	Dimensione,
				--	GETDATE(),
				--	'ADMIN',
				--	NULL
				--FROM Registrazione.dbo.Documento WHERE id=@IdDocumentoNomina

				INSERT INTO EntiDocumenti(
					IdEnteFase,
					BinData,
					FileName,
					DataInserimento,
					UsernameInserimento,
					HashValue,
					IdTipoAllegato)
				SELECT
					@IdEnteFase,
					Blob,
					'NOMINARL_'+@CodiceFiscaleEnte+dbo.FN_Estensione_File(NomeFile),
					GETDATE(),
					'ADMIN',
					Hash,
					1
				FROM Registrazione.dbo.Documento
				WHERE id=@IdDocumentoNomina

				SET @IdAllegato = SCOPE_IDENTITY()
				UPDATE
					Enti
				SET
					IdAllegatoDocumentoNomina=@IdAllegato
				WHERE
					IdEnte=@IdEnteTitolare
			END
	END

END
	RETURN

END TRY
BEGIN CATCH
	SET @esito = 0	
	SET @messaggio = 'ERRORE IMPREVISTO: ' + char(10) + char(13) + 'Si prega di contattare il servizio di assistenza e fornire le seguenti informazioni. CF_E: ' + isnull(@CodiceFiscaleEnte,'ND') + ' CF_LR: ' + isnull(@CodiceFiscaleLegaleRappresentante,'ND') + ' Caso: ' + ISNULL(CONVERT(VARCHAR(100),@Caso),'ND') + ' RIF_ET: ' +isnull(CONVERT(VARCHAR(100),@IdEnteTitolare),'ND') + ' RIF_LR: ' + ISNULL(CONVERT(VARCHAR(100),@IdLegaleRappresentante),'ND') + ' RIF_EA:' + ISNULL(CONVERT(VARCHAR(100),@IdEnteAccoglienza),'ND') + ' DN_RL: ' + ISNULL(DBO.FORMATODATA(@DataNominaRappresentanteLegale),'ND') + ' F_ET: ' + CONVERT(VARCHAR(100),@EnteTitolare) + ' VAR_RL: ' + CONVERT(VARCHAR(10),@VariazioneRappresentanteLegale)	print ERROR_MESSAGE()
END CATCH



