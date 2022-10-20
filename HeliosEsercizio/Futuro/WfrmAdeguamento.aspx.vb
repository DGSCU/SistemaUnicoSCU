Imports System.Data.SqlClient
Imports System.Web.Services
Imports System.Drawing
Imports System.Collections.Generic
Imports Futuro.RiepilogoAccreditamento
Imports Logger.Data
Imports System.IO
Imports System.Security.Cryptography
Imports System.Configuration.ConfigurationManager

Public Class WfrmAdeguamento
	Inherits SmartPage
	Private Const DOCUMENTO_PRIVACY As Integer = 1
	Private Const DOCUMENTO_IMPEGNO As Integer = 2
	Private Const CLASSE_NAZIONALE As Integer = 8
	Private Const CLASSE_REGIONALE As Integer = 9

	Private Const PATH_DOC_RIEPILOGO As String = "download\Master\Iscrizione\RiepilogoAdeguamento.docx"
	Private Const PATH_DOC_RIEPILOGO_COORDINATORE As String = "download\Master\Iscrizione\RiepilogoAdeguamentoCoordinatore.docx"
	Private Const TIPO_RAPPRESENTANTE_LEGALE As String = "Rappresentante Legale"
	Private Const TIPO_COORDINATORE_RESPONSABILE As String = "Coordinatore Responsabile del Servizio Civile Universale"

	Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load
		VerificaSessione()
		If Session("IdStatoEnte") = 6 Then
			Response.Redirect("~/WfrmAlbero.aspx?tipologia=Enti&Arrivo=WfrmMain.aspx&VediEnte=1")
			Exit Sub
		End If
		'Verifica Condizioni Per la presentazione
		Dim strRitornoStore As String

		'Lettura Documenti
		If Not Page.IsPostBack Then

			Session("DocumentoDaFirmare") = Nothing

			strRitornoStore = LeggiStoreVerificaCompletamentoAccreditamento(Session("IdEnte"))
			If strRitornoStore <> "" Then
				Response.Redirect("WfrmAlbero.aspx?tipologia=Enti&Arrivo=WfrmMain.aspx&VediEnte=1")
				Exit Sub
			End If
			Dim documentoPrivacy As VersioneDocumento = Nothing
			Dim documentoImpegno As VersioneDocumento = Nothing
			Dim myReader As SqlDataReader
			Try
				'Caricamento documenti privacy / dichiarazione d'impegno
				Dim query As String = "select * from VersioneDocumento WHERE GETDATE() BETWEEN DataInizioValidita And  ISNULL(DataFineValidita,'99990101') ORDER BY Versione"
				myReader = ClsServer.CreaDatareader(query, Session("Conn"))
				While myReader.Read()
					Dim documento As New VersioneDocumento
					documento.IdVersioneDocumento = GetData(myReader("IdVersioneDocumento"))
					documento.IdTipoDocumento = GetData(myReader("IdTipoDocumento"))
					documento.Hash = GetData(myReader("HashValue"))
					documento.Testo = GetData(myReader("Testo"))
					If documento.IdVersioneDocumento = DOCUMENTO_PRIVACY Then
						documentoPrivacy = documento
					ElseIf documento.IdVersioneDocumento = DOCUMENTO_IMPEGNO Then
						documentoImpegno = documento
					End If
				End While
			Catch ex As Exception
				Log.Error(LogEvent.PRESENTAZIONE_ERRORE_ISCRIZIONE, "Lettura Dati Versione Documento", ex)
				lblError.Text = "Errore nella lettura dati"
			Finally
				If myReader IsNot Nothing Then
					myReader.Close()
				End If
			End Try
			If documentoPrivacy IsNot Nothing Then
				txtPrivacy.Text = documentoPrivacy.Testo
				hashPrivacy.Text = " (Hash : " & documentoPrivacy.Hash & ")"
				lnkPrivacy.CommandArgument = documentoPrivacy.IdTipoDocumento
			End If
			If documentoImpegno IsNot Nothing Then
				txtImpegno.Text = documentoImpegno.Testo
				hashImpegno.Text = " (Hash : " & documentoImpegno.Hash & ")"
				lnkImpegno.CommandArgument = documentoImpegno.IdTipoDocumento
			End If
			Dim rdrClasse As SqlDataReader = Nothing
			Dim Sezione As String = Nothing
			Dim IdClasse As Integer = 0
			Dim NomeEnte As String = Nothing
			Dim Privato As Boolean
			Dim NumeroSedi As Integer
			Dim CodiceFiscale As String
			Try
				Dim query As String = "SELECT T.Privato, (SELECT count(*) FROM entisedi S JOIN entisediattuazioni SA on S.IDEnteSede=SA.IDEnteSede WHERE S.IDStatoEnteSede IN (1,4) AND IDEnte IN (select IDEnteFiglio from entirelazioni where  IdEntePadre=@IdEnte UNION Select @IdEnte)) NumeroSedi,  E.Denominazione, E.CodiceFiscale, S.IdClasseAccreditamento,IntestazioneDocumenti FROM enti E LEFT JOIN SezioniAlboSCU S ON E.IdSezione = S.IdSezione LEFT JOIN RegioniCompetenze R On R.IdRegioneCompetenza=S.IdRegioneCompetenza JOIN TipologieEnti T ON T.Descrizione = E.Tipologia Where E.IDEnte  = @IdEnte"
				Dim cmdClasse As New SqlCommand(query, Session("conn"))
				cmdClasse.Parameters.AddWithValue("@IdEnte", Session("IdEnte"))
				rdrClasse = cmdClasse.ExecuteReader
				If rdrClasse.Read() Then
					Sezione = GetData(rdrClasse("IntestazioneDocumenti"))
					IdClasse = GetData(rdrClasse("IdClasseAccreditamento"))
					NomeEnte = GetData(rdrClasse("Denominazione"))
					NumeroSedi = GetData(rdrClasse("NumeroSedi"))
					Privato = GetData(rdrClasse("Privato"))
					CodiceFiscale = GetData(rdrClasse("CodiceFiscale"))
				End If
			Catch ex As Exception
				Log.Error(LogEvent.PRESENTAZIONE_ERRORE_ISCRIZIONE, "Lettura Dati Versione Documento", ex)
				lblError.Text = "Errore nella lettura dati"
			Finally
				If rdrClasse IsNot Nothing Then
					rdrClasse.Close()
				End If
			End Try
			Dim enteManager = New clsEnte()
			'Caricamento Dichiarazione Impegno
			chkImpegno.Checked = enteManager.VerificaDichiarazioneImpegno(Session("IdEnte"), Session("conn"))

			hfCodiceFiscaleEnte.Value = CodiceFiscale
			txtEnte.Text = NomeEnte
			'divStatuto.Visible = Privato
            VisualizzaFile()
            VisualizzaNoteEsplicative()
		Else

		End If
	End Sub

	Private Function LeggiStoreVerificaCompletamentoAccreditamento(ByVal IDEnte As Integer) As String
		'Agg. da Simona Cordella il 16/06/2009
		'richiamo store che verifca se l'ente ha completato tutti gli inserimeni e gli aggiornamenti necessari per effettuare la presentazione della domanda di accrditamento /adeguamento
		Dim intValore As Integer

		Dim CustOrderHist As SqlClient.SqlCommand
		CustOrderHist = New SqlClient.SqlCommand
		CustOrderHist.CommandType = CommandType.StoredProcedure
		CustOrderHist.CommandText = "SP_VERIFICA_COMPLETAMENTO_ACCREDITAMENTO"
		CustOrderHist.Connection = IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn"))

		Dim sparam As SqlClient.SqlParameter
		sparam = New SqlClient.SqlParameter
		sparam.ParameterName = "@IdEnte"
		sparam.SqlDbType = SqlDbType.Int
		CustOrderHist.Parameters.Add(sparam)


		Dim sparam1 As SqlClient.SqlParameter
		sparam1 = New SqlClient.SqlParameter
		sparam1.ParameterName = "@Esito"
		sparam1.SqlDbType = SqlDbType.Int
		sparam1.Direction = ParameterDirection.Output
		CustOrderHist.Parameters.Add(sparam1)

		Dim sparam2 As SqlClient.SqlParameter
		sparam2 = New SqlClient.SqlParameter
		sparam2.ParameterName = "@Motivazione"
		sparam2.SqlDbType = SqlDbType.VarChar
		sparam2.Size = 1000
		sparam2.Direction = ParameterDirection.Output
		CustOrderHist.Parameters.Add(sparam2)

		Dim reader As SqlClient.SqlDataReader
		CustOrderHist.Parameters("@IdEnte").Value = IDEnte
		CustOrderHist.ExecuteScalar()
		intValore = CustOrderHist.Parameters("@Esito").Value
		LeggiStoreVerificaCompletamentoAccreditamento = CustOrderHist.Parameters("@Motivazione").Value
		ChiudiDataReader(reader)
	End Function

	Private Sub ChiudiDataReader(ByRef dataReader As SqlDataReader)
		If Not dataReader Is Nothing Then
			dataReader.Close()
			dataReader = Nothing
		End If
	End Sub

	'Carica tutti i dati dell'ente necessarie al riepilogo.
	Private Function CaricaRiepilogo() As Ente
		'Listadegli enti (Titolare ed accoglienza)
		Dim Enti As New Dictionary(Of Integer, Ente)
		Dim idEnte As Integer = Session("IdEnte")
		'Caricamento delle Aree in cache
		Dim sqlAree As String = "select Id,Codifica,IdSettore,AreaIntervento Area from AreeEsperienza Where DataFineValidita IS NULL"
		Dim cmdAree As New SqlCommand(sqlAree, Session("conn"))
		Dim rdrAree As SqlDataReader
		Dim AreeIntervento As New List(Of AreaIntervento)
		Try
			rdrAree = cmdAree.ExecuteReader()
			While rdrAree.Read()
				AreeIntervento.Add(New AreaIntervento() With {
				.IdArea = GetData(rdrAree("Id")),
				.Codifica = GetData(rdrAree("Codifica")),
				.IdSettore = GetData(rdrAree("IdSettore")),
				.Area = GetData(rdrAree("Area"))
				})
			End While
		Catch ex As Exception
			Throw New Exception("Errore nella lettura delle aree", ex)
		Finally
			If rdrAree IsNot Nothing Then
				rdrAree.Close()
			End If
		End Try
		'Caricamento informazioni Ente Titolare
		Dim sqlCaricaRiepilogo As String = "SELECT E.CodiceRegione,E.IdEnte, S.IdClasseAccreditamento,IntestazioneDocumenti Sezione, T.Privato,CodiceRegione CodiceEnte,Denominazione,Tipologia,DataRichiestaAccreditamento,CodiceFiscale,DataNominaRL,PrefissoTelefonoRichiestaRegistrazione + ' ' + TelefonoRichiestaRegistrazione Telefono,DataCostituzione,S.Sezione Classe,PrefissoFax + ' ' + E.Fax Fax,E.Http Sito,Email,EmailCertificata PEC,CASE WHEN Indirizzo IS NULL THEN NULL ELSE Indirizzo + ', - ' + Cap + ' ' + Comune + ' (' + Provincia +')' END SedeLegale,DataRichiestaEnte  FROM Enti E JOIN SezioniAlboSCU S ON E.IdSezione=S.IdSezione JOIN TipologieEnti T ON T.Descrizione = E.Tipologia LEFT JOIN SezioniAlboSCU SA ON E.IdSezione = SA.IdSezione LEFT JOIN RegioniCompetenze R On R.IdRegioneCompetenza=SA.IdRegioneCompetenza WHERE IDEnte =@IdEnte"
		Dim cmdCaricaRiepilogo As New SqlCommand(sqlCaricaRiepilogo, Session("conn"))
		cmdCaricaRiepilogo.Parameters.AddWithValue("@IdEnte", idEnte)
		Dim rdrCaricaRiepilogo As SqlDataReader
		Dim ente As Ente
		Try
			rdrCaricaRiepilogo = cmdCaricaRiepilogo.ExecuteReader()
			If rdrCaricaRiepilogo.Read() Then
				ente = New Ente() With {
				.IdEnte = GetData(rdrCaricaRiepilogo("IdEnte")),
				.Sezione = GetData(rdrCaricaRiepilogo("Sezione")),
				.IdClasseAccreditamento = GetData(rdrCaricaRiepilogo("IdClasseAccreditamento")),
				.Privato = GetData(rdrCaricaRiepilogo("Privato")),
				.CodiceEnte = GetData(rdrCaricaRiepilogo("CodiceEnte")),
				.Denominazione = GetData(rdrCaricaRiepilogo("Denominazione")),
				.Tipo = GetData(rdrCaricaRiepilogo("Tipologia")),
				.DataRichiesta = GetData(rdrCaricaRiepilogo("DataRichiestaAccreditamento")),
				.CodiceFiscale = GetData(rdrCaricaRiepilogo("CodiceFiscale")),
				.DataNominaRappresentanteLegale = GetData(rdrCaricaRiepilogo("DataNominaRL")),
				.Telefono = GetData(rdrCaricaRiepilogo("Telefono")),
				.DataCostituzione = GetData(rdrCaricaRiepilogo("DataCostituzione")),
				.Classe = GetData(rdrCaricaRiepilogo("Classe")),
				.Fax = GetData(rdrCaricaRiepilogo("Fax")),
				.Sito = GetData(rdrCaricaRiepilogo("Sito")),
				.Email = GetData(rdrCaricaRiepilogo("Email")),
				.PEC = GetData(rdrCaricaRiepilogo("PEC"))
				}
				Enti.Add(ente.IdEnte, ente)
			Else
				Throw New Exception("Dati Ente / Classe non trovati")
			End If
		Catch ex As Exception
			Throw New Exception("Errore Caricamento dati Ente Titolare", ex)
		Finally
			If rdrCaricaRiepilogo IsNot Nothing Then
				rdrCaricaRiepilogo.Close()
			End If
		End Try
		'*** Caricamento Accordi (enti di accoglienza)
		Dim sqlAccordi As String = "SELECT E.IDEnte, C.Privato, T.tipoRelazione, CodiceRegione CodiceEnte, Denominazione, Tipologia, CodiceFiscale, PrefissoTelefonoRichiestaRegistrazione + ' ' + TelefonoRichiestaRegistrazione Telefono, DataCostituzione, PrefissoFax + ' ' + Fax Fax, Http Sito, Email, EmailCertificata PEC, CASE WHEN Indirizzo IS NULL THEN NULL ELSE Indirizzo + ', - ' + Cap + ' ' + Comune + ' (' + Provincia + ')' END SedeLegale, R.DataStipula, R.DataScadenza FROM enti E JOIN entirelazioni R ON R.IDEnteFiglio=E.IDEnte JOIN tipirelazioni T ON T.IDTipoRelazione = R.IDTipoRelazione JOIN TipologieEnti C ON C.Descrizione = E.Tipologia  WHERE R.IDEntePadre IN (select IDEnteFiglio from entirelazioni R JOIN enti E on E.idente=R.identefiglio where IdEntePadre=@IdEnte UNION Select @IdEnte)"
		Dim cmdAccordi As New SqlCommand(sqlAccordi, Session("conn"))
		cmdAccordi.Parameters.AddWithValue("@IdEnte", idEnte)
		Dim rdrAccordi As SqlDataReader
		Try
			rdrAccordi = cmdAccordi.ExecuteReader()
			While rdrAccordi.Read()
				Dim accordo As New Accordo()
				accordo.Privato = GetData(rdrAccordi("Privato"))
				accordo.IdEnte = GetData(rdrAccordi("IdEnte"))
				accordo.CodiceEnte = GetData(rdrAccordi("CodiceEnte"))
				accordo.Denominazione = GetData(rdrAccordi("Denominazione"))
				accordo.Relazione = GetData(rdrAccordi("tipoRelazione"))
				accordo.Tipo = GetData(rdrAccordi("Tipologia"))
				accordo.CodiceFiscale = GetData(rdrAccordi("CodiceFiscale"))
				accordo.Telefono = GetData(rdrAccordi("Telefono"))
				accordo.DataCostituzione = GetData(rdrAccordi("DataCostituzione"))
				accordo.Fax = GetData(rdrAccordi("Fax"))
				accordo.Sito = GetData(rdrAccordi("Sito"))
				accordo.Email = GetData(rdrAccordi("Email"))
				accordo.PEC = GetData(rdrAccordi("PEC"))
				accordo.Relazione = GetData(rdrAccordi("tipoRelazione"))
				accordo.DataStipula = GetData(rdrAccordi("DataStipula"))
				accordo.DataScadenza = GetData(rdrAccordi("DataScadenza"))
				ente.Accordi.Add(accordo)
				Enti.Add(accordo.IdEnte, accordo)
			End While
		Catch ex As Exception
			Throw New Exception("Errore Caricamento accordi Ente", ex)
		Finally
			If rdrAccordi IsNot Nothing Then
				rdrAccordi.Close()
			End If
		End Try
		ente.NumeroFase = RicavaIdEnteFase(ente.IdEnte)
		'Caricamento informazioni Settori
		Dim Settori As New List(Of Settore)
		Dim sqlSettori As String = "select E.IdEnte,IdSettore,Codifica,MacroAmbitoAttività Settore, I_AnnoEsperienza, I_Esperienza, II_AnnoEsperienza, II_Esperienza, III_AnnoEsperienza, III_Esperienza, AreeIntervento from  EnteEsperienzaSettore E JOIN macroambitiattività S On E.IdSettore = S.IDMacroAmbitoAttività JOIN Enti EN On EN.IdEnte=E.IdEnte WHERE EN.IDStatoEnte=6 AND DataFineValidita IS NULL AND E.IDEnte IN (select IDEnteFiglio from entirelazioni where IdEntePadre=@IdEnte UNION Select @IdEnte)"
		Dim cmdSettori As New SqlCommand(sqlSettori, Session("conn"))
		cmdSettori.Parameters.AddWithValue("@IdEnte", idEnte)
		Dim rdrSettori As SqlDataReader
		Try
			rdrSettori = cmdSettori.ExecuteReader()
			While rdrSettori.Read()
				Dim settore As New Settore() With {
					.IdEnte = GetData(rdrSettori("IdEnte")),
					.Nome = GetData(rdrSettori("Settore")),
					.Codifica = GetData(rdrSettori("Codifica")),
					.IdSettore = GetData(rdrSettori("IdSettore")),
					.AreeDecoded = GetData(rdrSettori("AreeIntervento"))
				}
				settore.Esperienze.Add(GetData(rdrSettori("I_AnnoEsperienza")), GetData(rdrSettori("I_Esperienza")))
				settore.Esperienze.Add(GetData(rdrSettori("II_AnnoEsperienza")), GetData(rdrSettori("II_Esperienza")))
				settore.Esperienze.Add(GetData(rdrSettori("III_AnnoEsperienza")), GetData(rdrSettori("III_Esperienza")))
				Settori.Add(settore)
			End While
		Catch ex As Exception
			Throw New Exception("Errore Caricamento dati Settoti ed Esperienze", ex)
		Finally
			If rdrSettori IsNot Nothing Then
				rdrSettori.Close()
			End If
		End Try
		For Each settore In Settori
			For Each area As String In settore.AreeDecoded.Split(",")
				For Each areaIntervento In AreeIntervento
					If areaIntervento.IdArea = area Then
						settore.AreeIntervento.Add(areaIntervento)
						Exit For
					End If
				Next
			Next
			Enti(settore.IdEnte).SettoriIntervento.Add(settore)

		Next
		'Caricamento Risorse
		Dim Risorse As New Dictionary(Of String, Risorsa)
		Dim sqlRisorse As String = "SELECT IdEnte, Nome + ' ' + Cognome Nome, Titolo, Posizione, CodiceFiscale, Email, Telefono, Fax, Cellulare, DataNascita, CASE WHEN PN.IDProvincia IS NULL THEN CN.Denominazione ELSE CN.Denominazione + ' (' + PN.DescrAbb + ')' END LuogoNascita, CASE WHEN CR.IDComune IS NULL THEN NULL WHEN PR.IDProvincia IS NULL THEN CR.Denominazione ELSE E.Indirizzo + ', ' + Civico + ' - ' + E.Cap + ' ' + CR.Denominazione + ' (' + PR.DescrAbb +')' END IndirizzoResidenza, Case EsperienzaServizioCivile WHEN 1 Then 'Fatto' WHEN 2 THEN 'Da Fare' END Esperienza, CASE corso WHEN 1 THEN 'Fatto' WHEN 2 THEN 'Da Fare' WHEN 3 THEN 'Non Necessario' ELSE NULL End  CorsoFormazione FROM entepersonale E  LEFT JOIN	comuni CR on CR.IDComune = E.IDComuneResidenza  LEFT JOIN	provincie PR on PR.IDProvincia = CR.IDProvincia LEFT JOIN	comuni CN on CN.IDComune = E.IDComuneNascita LEFT JOIN	provincie PN on PN.IDProvincia = CN.IDProvincia WHERE DataFineValidità is null AND IDEnte IN (select IDEnteFiglio from entirelazioni R JOIN enti E On E.idente=R.IDEnteFiglio where E.IDStatoEnte=6 AND IdEntePadre=@IdEnte UNION Select @IdEnte)"
		Dim cmdRisorse As New SqlCommand(sqlRisorse, Session("conn"))
		cmdRisorse.Parameters.AddWithValue("@IdEnte", idEnte)
		Dim rdrRisorse As SqlDataReader
		Try
			rdrRisorse = cmdRisorse.ExecuteReader()
			While rdrRisorse.Read()
				Dim risorsa As New Risorsa()
				risorsa.IdEnte = GetData(rdrRisorse("IdEnte"))
				risorsa.Nome = GetData(rdrRisorse("Nome"))
				risorsa.Titolo = GetData(rdrRisorse("Titolo"))
				risorsa.Posizione = GetData(rdrRisorse("Posizione"))
				risorsa.CodiceFiscale = GetData(rdrRisorse("CodiceFiscale"))
				risorsa.Email = GetData(rdrRisorse("Email"))
				risorsa.Telefono = GetData(rdrRisorse("Telefono"))
				risorsa.Fax = GetData(rdrRisorse("Fax"))
				risorsa.Cellulare = GetData(rdrRisorse("Cellulare"))
				risorsa.DataNascita = GetData(rdrRisorse("DataNascita"))
				risorsa.LuogoNascita = GetData(rdrRisorse("LuogoNascita"))
				risorsa.IndirizzoResidenza = GetData(rdrRisorse("IndirizzoResidenza"))
				risorsa.Esperienza = GetData(rdrRisorse("Esperienza"))
				risorsa.CorsoFormazione = GetData(rdrRisorse("CorsoFormazione"))
				If Not Risorse.ContainsKey(risorsa.CodiceFiscale) Then
					Risorse.Add(risorsa.CodiceFiscale, risorsa)
				End If
			End While
		Catch ex As Exception
			Throw New Exception("Errore Caricamento dati Risorse", ex)
		Finally
			If rdrRisorse IsNot Nothing Then
				rdrRisorse.Close()
			End If
		End Try
		For Each risorsa In Risorse
			Enti(risorsa.Value.IdEnte).Risorse.Add(risorsa.Value)
		Next

		'Caricamento Ruoli
		Dim Ruoli As New List(Of Ruolo)
		Dim sqlRuoli As String = "SELECT R.IDRuolo,P.CodiceFiscale,R.Ruolo FROM entepersonaleruoli RP JOIN entepersonale P ON P.IDEntePersonale = RP.IDEntePersonale JOIN ruoli R ON RP.IdRuolo=R.IdRuolo WHERE IDEnte = @IdEnte and rp.DataFineValidità is null and p.DataFineValidità is null"
		Dim cmdRuoli As New SqlCommand(sqlRuoli, Session("conn"))
		cmdRuoli.Parameters.AddWithValue("@IdEnte", idEnte)
		Dim rdrRuoli As SqlDataReader
		Try
			rdrRuoli = cmdRuoli.ExecuteReader()
			While rdrRuoli.Read()
				Dim ruolo As New Ruolo()
				ruolo.IdRuolo = GetData(rdrRuoli("IdRuolo"))
				ruolo.CodiceFiscale = GetData(rdrRuoli("CodiceFiscale"))
				ruolo.Ruolo = GetData(rdrRuoli("Ruolo"))
				Ruoli.Add(ruolo)
			End While
		Catch ex As Exception
			Throw New Exception("Errore Caricamento dati Ruoli", ex)
		Finally
			If rdrRuoli IsNot Nothing Then
				rdrRuoli.Close()
			End If
		End Try
		For Each ruolo In Ruoli
			If Risorse.ContainsKey(ruolo.CodiceFiscale) Then
				Risorse(ruolo.CodiceFiscale).Ruoli.Add(ruolo)
				If ruolo.IdRuolo = 4 Then
					ente.RappresentanteLegale = Risorse(ruolo.CodiceFiscale)
				End If
			End If
		Next

		'Caricamento numero ruoli
		Dim NumeroRuoli As New List(Of Ruolo)
		Dim sqlNumeroRuoli As String = "SELECT ISNULL(SUM(CASE WHEN IdRuolo= 2 THEN 1 END),0) Formatori,ISNULL(SUM(CASE WHEN IdRuolo= 19 THEN 1 END),0) Selettori,ISNULL(SUM(CASE WHEN IdRuolo= 14 THEN 1 END),0) EspertiMonitoraggio FROM entepersonaleruoli RP JOIN entepersonale P ON P.IDEntePersonale = RP.IDEntePersonale WHERE IDEnte = @IdEnte"
		Dim cmdNumeroRuoli As New SqlCommand(sqlNumeroRuoli, Session("conn"))
		cmdNumeroRuoli.Parameters.AddWithValue("@IdEnte", idEnte)
		Dim rdrNumeroRuoli As SqlDataReader
		Try
			rdrNumeroRuoli = cmdNumeroRuoli.ExecuteReader()
			While rdrNumeroRuoli.Read()
				Dim ruolo As New Ruolo()
				ente.Formatori = GetData(rdrNumeroRuoli("Formatori"))
				ente.Selettori = GetData(rdrNumeroRuoli("Selettori"))
				ente.EspertiMonitoraggio = GetData(rdrNumeroRuoli("EspertiMonitoraggio"))
			End While
		Catch ex As Exception
			Throw New Exception("Errore Caricamento dati NumeroRuoli", ex)
		Finally
			If rdrNumeroRuoli IsNot Nothing Then
				rdrNumeroRuoli.Close()
			End If
		End Try


		'***Caricamento Sedi
		Dim Sedi As New List(Of Sede)
		Dim sqlSedi As String = "SELECT IdEnte, S.Denominazione Nome, NMaxVolontari NumeroVolontari, CASE WHEN C.IDComune IS NULL THEN NULL WHEN P.IDProvincia IS NULL THEN C.Denominazione ELSE S.Indirizzo + ', ' + Civico + ' - ' + S.Cap + ' ' + C.Denominazione + ' (' + P.DescrAbb +')' END Indirizzo, CASE WHEN NULLIF(Palazzina,'ND') IS NOT NULL THEN 'Palazzina' + Palazzina + ' ' END, CASE WHEN NULLIF(Scala,'ND') IS NOT NULL THEN 'Scala' + Scala + ' ' END, CASE WHEN NULLIF(Piano,0) IS NOT NULL THEN 'Piano ' + CAST(Piano as varchar(5)) +' ' END, CASE WHEN NULLIF(Interno,'ND') IS NOT NULL THEN 'Interno' + Interno + ' ' END DettaglioRecapito, T.TitoloGiuridico, Http Sito, Email, EST.IDTipoSede,C.ComuneNazionale FROM entisedi S LEFT JOIN entisediattuazioni SA on S.IDEnteSede=SA.IDEnteSede LEFT JOIN comuni C on C.IDComune = S.IDComune LEFT JOIN provincie P on P.IDProvincia = C.IDProvincia LEFT JOIN TitoliGiuridici T ON T.IdTitoloGiuridico =S.IdTitoloGiuridico LEFT JOIN entiseditipi EST ON EST.IDEnteSede = S.IDEnteSede WHERE S.IDStatoEnteSede IN (1,4) AND IDEnte IN (select IDEnteFiglio from entirelazioni R join enti E ON e.IdEnte = R.IDEnteFiglio where IdEntePadre=@IdEnte and E.IDStatoEnte=6 UNION Select @IdEnte)"
		Dim cmdSedi As New SqlCommand(sqlSedi, Session("conn"))
		cmdSedi.Parameters.AddWithValue("@IdEnte", idEnte)
		Dim rdrSedi As SqlDataReader
		Try
			rdrSedi = cmdSedi.ExecuteReader()
			While rdrSedi.Read()
				Dim sede As New Sede
				sede.IdEnte = GetData(rdrSedi("IdEnte"))
				sede.IdTipoSede = GetData(rdrSedi("IDTipoSede"))
				sede.Nome = GetData(rdrSedi("Nome"))
				sede.NumeroVolontari = GetData(rdrSedi("NumeroVolontari"))
				sede.Indirizzo = GetData(rdrSedi("Indirizzo"))
				sede.DettaglioRecapito = GetData(rdrSedi("DettaglioRecapito"))
				sede.TitoloGiuridico = GetData(rdrSedi("TitoloGiuridico"))
				sede.Sito = GetData(rdrSedi("Sito"))
				sede.Email = GetData(rdrSedi("Email"))
				sede.Nazionale = GetData(rdrSedi("ComuneNazionale"))
				Sedi.Add(sede)
			End While
		Catch ex As Exception
			Throw New Exception("Errore Caricamento dati Sedi", ex)
		Finally
			If rdrSedi IsNot Nothing Then
				rdrSedi.Close()
			End If
		End Try
		Dim numeroSedi As Integer = 0
		Dim numeroSediEstere As Integer = 0
		For Each sede In Sedi
			If Enti.ContainsKey(sede.IdEnte) Then
				If sede.IdTipoSede = 1 Then
					Enti(sede.IdEnte).SedeLegale = sede
				Else
					Enti(sede.IdEnte).Sedi.Add(sede)
					numeroSedi += 1
				End If
				If sede.Nazionale = False Then
					numeroSediEstere += 1
				End If
			Else
				'Throw New Exception(Format("Errore inserimento Sede: IdEnte {0} Non presente ", sede.IdEnte))
			End If

		Next
		ente.NumeroSedi = numeroSedi
		ente.NumeroSediEstere = numeroSediEstere
		'Caricamento Documenti
		Try
			Using DsAllegati As New DataSet,
				daAllegati As New SqlDataAdapter("SP_AllegatiEnte", CType(Session("conn"), SqlConnection))

				daAllegati.SelectCommand.CommandType = CommandType.StoredProcedure
				daAllegati.SelectCommand.Parameters.Add("@IdEnte", SqlDbType.Int).Value = idEnte
				daAllegati.Fill(DsAllegati)
				Dim dtAllegati As DataTable = DsAllegati.Tables(0)
				For Each rowAllegato As DataRow In dtAllegati.Rows
					Dim allegato As New RiepilogoAccreditamento.Allegato
					allegato.Id = rowAllegato.Item("IdAllegato")

					allegato.NomeFile = rowAllegato.Item("FileName")

					If rowAllegato.Item("Tipologia") = "Carta Impegno" Then
						allegato.Tipologia = "Dichiarazione Impegno"
					Else
						allegato.Tipologia = rowAllegato.Item("Tipologia")
					End If
					' allegato.Tipologia = rowAllegato.Item("Tipologia")


					allegato.Hash = rowAllegato.Item("HashValue")
					Dim idEnteAllegato As Integer = rowAllegato.Item("IdEnte")
					Enti(idEnteAllegato).Allegati.Add(allegato)
				Next
			End Using
		Catch ex As Exception
			Throw New Exception("Errore nel caricamento dati allegati", ex)
		End Try



		Return ente
    End Function

    Private Sub SalvaNoteEsplicative()
        Dim intIDEnteFase As Integer
        intIDEnteFase = RicavaIdEnteFase(Session("IdEnte"))

        Dim sqlNote As String = "update entifasi set NoteEsplicative = @Note where IdEnteFase=@IdEnteFase"
        Dim cmdNote As New SqlCommand(sqlNote, Session("conn"))
        cmdNote.Parameters.AddWithValue("@Note", txtNoteEsplicative.Text.Trim)
        cmdNote.Parameters.AddWithValue("@IdEnteFase", intIDEnteFase)

        cmdNote.ExecuteNonQuery()

    End Sub

    Private Sub VisualizzaNoteEsplicative()
        Dim intIDEnteFase As Integer
        intIDEnteFase = RicavaIdEnteFase(Session("IdEnte"))

        Dim sqlNote As String = "select isnull(NoteEsplicative,'') from entifasi where IdEnteFase=@IdEnteFase"
        Dim cmdNote As New SqlCommand(sqlNote, Session("conn"))
        cmdNote.Parameters.AddWithValue("@IdEnteFase", intIDEnteFase)

        txtNoteEsplicative.Text = cmdNote.ExecuteScalar()
    End Sub

	Private Sub Riepilogo_Click() Handles Riepilogo.Click
		Dim errori As String = Controlli()

		If Not String.IsNullOrEmpty(errori) Then
			lblError.Text = errori
			Exit Sub
		End If

        SalvaNoteEsplicative()

		Dim ente As Ente
		Try
			ente = CaricaRiepilogo()
		Catch ex As Exception
			Log.Error(LogEvent.PRESENTAZIONE_ERRORE, "Caricamento dati Domanda di Iscrizione", ex)
			lblError.Text = "Errore nella generazione della Domanda di Iscrizione"
			Exit Sub
		End Try
		Dim isRappresentanteLegale As Boolean = ddlTipoRappresentante.SelectedValue = TIPO_RAPPRESENTANTE_LEGALE

		Dim documento As New AsposeWord
		Dim responsabile As Risorsa
		Dim managerEnte = New clsEnte()
		Try
			If isRappresentanteLegale Then
				documento.open(Server.MapPath(PATH_DOC_RIEPILOGO))
				responsabile = managerEnte.GetRappresentanteLegale(Session("IdEnte"), Session("conn"))

			Else
				documento.open(Server.MapPath(PATH_DOC_RIEPILOGO_COORDINATORE))
				responsabile = managerEnte.GetCoordinatoreResponsabile(Session("IdEnte"), Session("conn"))

			End If
		Catch ex As Exception
			Log.Error(LogEvent.PRESENTAZIONE_ERRORE, "Template non valido", ex)
			lblError.Text = "Errore nella generazione della Domanda di Adeguamento"
			Exit Sub
        End Try
        documento.addFieldValue("NoteEsplicative", txtNoteEsplicative.Text.Trim)
		documento.addFieldValue("NomeEnte", ente.Denominazione)
		documento.addFieldValue("CodiceFiscaleEnte", ente.CodiceFiscale)
		documento.addFieldValue("TipoRappresentante", ddlTipoRappresentante.SelectedValue)
		If responsabile IsNot Nothing Then
			documento.addFieldValue("RappresentanteLegaleNome", responsabile.Nome.ToUpper())
			documento.addFieldValue("RappresentanteLegaleLuogoNascita", responsabile.LuogoNascita)
			Dim indirizzo As String = ""
			If Not String.IsNullOrEmpty(responsabile.IndirizzoResidenza) Then
				indirizzo = " e residente in " + responsabile.IndirizzoResidenza
			End If
			documento.addFieldValue("RappresentanteLegaleIndirizzo", indirizzo)
			If responsabile.DataNascita.HasValue Then
				documento.addFieldValue("RappresentanteLegaleDataNascita", responsabile.DataNascita.Value.ToString("dd/MM/yyyy"))
			End If
		End If
		If ente.SedeLegale IsNot Nothing Then
			documento.addFieldValue("IndirizzoEnte", ente.SedeLegale.Indirizzo)
		End If
		documento.addFieldValue("NumeroEnti", ente.Accordi.Count)
		documento.addFieldValue("NumeroSedi", ente.NumeroSedi)
		documento.addFieldValue("NumeroSediNazionali", ente.NumeroSedi - ente.NumeroSediEstere)
		documento.addFieldValue("NumeroSediEstere", ente.NumeroSediEstere)
		If ente.DataNominaRappresentanteLegale.HasValue Then
			documento.addFieldValue("DataNominaRappresentanteLegale", ente.DataNominaRappresentanteLegale.Value.ToString("dd/MM/yyyy"))
		End If
		documento.addFieldValue("Privato", If(ente.Privato, "Privato", "Pubblico"))
		documento.addFieldValue("TipoEnte", ente.Tipo)
		documento.addFieldValue("TelefonoEnte", ente.Telefono)
		documento.addFieldValue("EmailEnte", ente.Email)
		documento.addFieldValue("PecEnte", ente.PEC)
		documento.addFieldValue("SitoEnte", ente.Sito)
		documento.addFieldValue("NumeroFormatori", ente.Formatori)
		documento.addFieldValue("NumeroSelettori", ente.Selettori)
		documento.addFieldValue("NumeroEspertiMonitoraggio", ente.EspertiMonitoraggio)
		documento.addFieldValue("NumeroFase", ente.NumeroFase)
		documento.addFieldValue("CodiceEnte", ente.CodiceEnte)
		documento.addFieldValue("Data", Date.Today.ToString("dd/MM/yyyy"))
		If ente.IdClasseAccreditamento = CLASSE_NAZIONALE Then
			documento.addFieldValue("SezioneEnte", "Nazionale")
		Else
			documento.addFieldValue("SezioneEnte", "della " & Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(ente.Sezione.ToLower()))
		End If
		documento.addFieldValue("SitoEnte", ente.Sito)



		Dim htmlAllegati As New StringBuilder
		htmlAllegati.Append("<style>")
		htmlAllegati.Append("table {width:100%; border-collapse: collapse; font-size:10pt; margin-bottom:1em;}")
		htmlAllegati.Append("table, th, td {border: 1px solid lightgray;}")
		htmlAllegati.Append("table tr:nth-child(even) {background-color:#eee}")
		htmlAllegati.Append("td {padding:1pt; font-family:'courier new';}")
		htmlAllegati.Append(".space {width:50%;height:1em;}")
		htmlAllegati.Append("</style>")

		htmlAllegati.Append(CreaTabellaAllegato(ente, True))

		For Each ente In ente.Accordi
			htmlAllegati.Append(CreaTabellaAllegato(ente, False))
		Next


		documento.addFieldHtml("htmlDocumenti", htmlAllegati.ToString)

		Try
			documento.merge()

			Session("DocumentoDaFirmare") = documento.pdfBytes

		Catch ex As Exception
			Log.Error(LogEvent.PRESENTAZIONE_ERRORE, "Scrittura template", ex)
			lblError.Text = "Errore nella generazione della Domanda di Adeguamento"
			Exit Sub
		End Try

		Log.Information(LogEvent.PRESENTAZIONE_RIEPILOGO)
		Response.Clear()
		Response.ContentType = "Application/pdf"
		Response.AddHeader("Content-Disposition", "attachment; filename=" & "DomandaAdeguamento.pdf")
		Response.BinaryWrite(documento.pdfBytes)
		Response.End()
	End Sub


	Private Function CreaTabellaAllegato(ente As Ente, titolare As Boolean) As String
		If ente.Allegati.Count = 0 Then
			Return String.Empty
		End If
		Dim html As New StringBuilder
		html.Append("<table><tbody>")
		html.Append("<tr style='fort-weight:bold'>")
		html.Append("<th colspan='3'>")
		html.Append("Ente " & If(titolare, "Titolare ", "Accoglienza ") & ente.Denominazione)
		html.Append("</th>")
		html.Append("</tr>")
		If ente.Allegati.Count = 0 Then
			html.Append("<tr><td colspan='3'>Nessun allegato</td></tr>")
		Else
			html.Append("<tr style='fort-weight:bold'>")
			html.Append("<th>Nome File</th>")
			html.Append("<th>Tipologia</th>")
			html.Append("<th>Hash Value</th>")
			html.Append("</tr>")
			Dim rigaPari As Boolean = True
			For Each allegato In ente.Allegati
				html.Append("<tr>")
				html.Append("<td>" & allegato.NomeFile & "</td>")
				html.Append("<td>" & allegato.Tipologia & "</td>")
				html.Append("<td>" & allegato.Hash & "</td>")
				html.Append("</tr>")
			Next

		End If
		html.Append("</tbody></table>")
		html.Append("</br>")
		Return html.ToString()
	End Function


	Private Function GetData(data As Object) As Object
		If IsDBNull(data) Then
			Return Nothing
		End If
		Return data
	End Function

	Private Class VersioneDocumento
		Public Property IdVersioneDocumento As String
		Public Property IdTipoDocumento As Integer
		Public Property Hash As String
		Public Property Testo As String
	End Class

	Protected Sub lnkDownload_Click(sender As LinkButton, e As EventArgs)
		Dim idVersioneDocumento As Integer = sender.CommandArgument

		Dim rdrVersioneDocumento As SqlDataReader = Nothing
		Dim IdTipoDocumento As Integer
		Dim fileName As String = Nothing
		Dim blob As Byte() = Nothing
		Try
			Dim query As String = "select D.IdTipoDocumento, A.BinData Blob, COALESCE(A.filename,D.filename) FileName from VersioneDocumento D LEFT JOIN Allegato A ON A.IdAllegato=D.IdAllegato WHERE GETDATE() BETWEEN D.DataInizioValidita And ISNULL(D.DataFineValidita,'99990101') And IdVersioneDocumento=@IdVersioneDocumento"
			Dim cmVersioneDocumento As New SqlCommand(query, Session("conn"))
			cmVersioneDocumento.Parameters.AddWithValue("@IdVersioneDocumento", idVersioneDocumento)
			rdrVersioneDocumento = cmVersioneDocumento.ExecuteReader
			If rdrVersioneDocumento.Read() Then
				IdTipoDocumento = GetData(rdrVersioneDocumento("IdTipoDocumento"))
				blob = GetData(rdrVersioneDocumento("Blob"))
				fileName = GetData(rdrVersioneDocumento("FileName"))
			Else
				Dim entity As New Entity With {
					.Id = idVersioneDocumento,
					.Name = "VersioneDocumento"
				}
				Log.Warning(LogEvent.PRESENTAZIONE_ERRORE_DOWNLOAD, "Documento inesistente", entity:=entity)
				lblError.Text = "Documento non trovato"
				Exit Sub
			End If
			If blob Is Nothing Then
				blob = File.ReadAllBytes(Server.MapPath(fileName))
				fileName = fileName.Substring(fileName.LastIndexOf("\") + 1)
			End If
		Catch ex As Exception
			Log.Error(LogEvent.PRESENTAZIONE_ERRORE_ISCRIZIONE, "Lettura Dati Versione Documento", ex)
			lblError.Text = "Errore nel Download del documento"
			Exit Sub
		Finally
			If rdrVersioneDocumento IsNot Nothing Then
				rdrVersioneDocumento.Close()
			End If
		End Try
		If IdTipoDocumento = DOCUMENTO_PRIVACY Then
			hfLettoPrivacy.Value = True
		ElseIf IdTipoDocumento = DOCUMENTO_IMPEGNO Then
			hfLettoImpegno.Value = True
		End If
		Response.Clear()
		Response.ContentType = "Application/pdf"
		Response.AddHeader("Content-Disposition", "attachment; filename=" & fileName)
		Response.BinaryWrite(blob)
		Response.End()
	End Sub
	Protected Sub chkPrivacy_CheckedChanged(sender As Object, e As EventArgs)
		hfLettoPrivacy.Value = True
	End Sub

	Protected Sub chkImpegno_CheckedChanged(sender As Object, e As EventArgs)
		hfLettoImpegno.Value = True
	End Sub

	''' <summary>
	''' Classe per la gestione degli allegati
	''' </summary>

	Protected Sub Carta_Click(sender As Object, e As EventArgs)
		Dim blob = File.ReadAllBytes(Server.MapPath("\download\Master\Iscrizione\CartaImpegnoEtico.pdf"))
		Response.Clear()
		Response.ContentType = "Application/pdf"
		Response.AddHeader("Content-Disposition", "attachment; filename=CartaImpegnoEtico.pdf")
		Response.BinaryWrite(blob)
		Response.End()
	End Sub
	Protected Function calcEntiSenzaDichiarazione() As Integer
		Dim dtrLocal As SqlClient.SqlDataReader
        Dim strSql As String
        Dim n As Integer = 0

        strSql = "SELECT"
        strSql &= " 	Count(*) n"
        strSql &= " FROM"
        strSql &= " 	enti a "
        strSql &= " 	inner join entirelazioni b on a.idente = b.identefiglio "
        strSql &= " 	inner join statienti c on a.idstatoente = c.idstatoente"
        strSql &= " 	left join (	SELECT EFE.IdEnte"
        strSql &= " 				FROM entifasi_enti EFE"
        strSql &= " 					INNER JOIN EntiFasi EF on EF.IdEnteFase=EFE.IdEnteFase "
        strSql &= " 				WHERE  EF.Stato=3) PRESENTATE"
        strSql &= " 				on a.IDEnte = PRESENTATE.IdEnte	"
        strSql &= " WHERE"
        strSql &= " 		b.IDEntePadre = " & Session("IdEnte") & " And a.idstatoente = 6 And PRESENTATE.IdEnte Is NULL"

        dtrLocal = ClsServer.CreaDatareader(strSql, Session("conn"))
        If dtrLocal.HasRows Then
            dtrLocal.Read()
            n = dtrLocal("n")
        End If
        dtrLocal.Close()
        dtrLocal = Nothing

        If n = 0 Then   'non ci sono nuovi enti di accoglienza quindi non serve proprio la spunta della dichiarazione di impegno, uso -1
            Return -1
        End If

        strSql = "SELECT"
        strSql &= " 	Count(*) n"
        strSql &= " FROM"
        strSql &= " 	enti a "
        strSql &= " 	inner join entirelazioni b on a.idente = b.identefiglio "
        strSql &= " 	inner join statienti c on a.idstatoente = c.idstatoente"
        strSql &= " 	left join (	SELECT EFE.IdEnte"
        strSql &= " 				FROM entifasi_enti EFE"
        strSql &= " 					INNER JOIN EntiFasi EF on EF.IdEnteFase=EFE.IdEnteFase "
        strSql &= " 				WHERE  EF.Stato=3) PRESENTATE"
        strSql &= " 				on a.IDEnte = PRESENTATE.IdEnte	"
        strSql &= " 	LEFT JOIN EntiDocumenti D ON D.IdEnteDocumento = a.IdAllegatoImpegno"
        strSql &= " WHERE"
        strSql &= " 		b.IDEntePadre = " & Session("IdEnte") & " And a.idstatoente = 6 And PRESENTATE.IdEnte Is NULL"
        strSql &= " 		And D.IdEnteDocumento Is NULL"

        dtrLocal = ClsServer.CreaDatareader(strSql, Session("conn"))
        If dtrLocal.HasRows Then
            dtrLocal.Read()
            n = dtrLocal("n")
        End If
        dtrLocal.Close()
        dtrLocal = Nothing
        Return n
	End Function

	Private Function Controlli() As String
		Dim errori As String = ""
		If Not chkRichiestaIscrizione.Checked Then
			errori = errori + "Non è stata spuntata la richiesta di adeguamento.</br>"
		End If
		If Not chkPrivacy.Checked Then
			errori = errori + "Non è stata spuntata la lettura dell'informativa sulla privacy.</br>"
        End If

        'occorre controllare se tutti gli enti di accoglienza hanno la carta della dichiarazione d'impegno
        Dim nEntiSenzaDichiarazione As Integer = calcEntiSenzaDichiarazione()
		If Not chkImpegno.Checked Then
            If nEntiSenzaDichiarazione > 0 Then errori = errori + "Non è stata spuntata l'accettazione della dichiarazione d'impegno: in questo caso occorre che tutti gli Enti di accoglienza abbiano la dichiarazione di impegno,  ma ci sono ancora " & nEntiSenzaDichiarazione & " Enti di accoglienza che non la possiedono.</br>"
        End If
        If chkImpegno.Checked Then
            If nEntiSenzaDichiarazione = -1 Then errori = errori + "E' stata spuntata l'accettazione della dichiarazione d'impegno a sistema ma non è necessaria in quanto non ci sono nuovi enti di accoglienza.</br>"
            If nEntiSenzaDichiarazione = 0 Then errori = errori + "E' stata spuntata l'accettazione della dichiarazione d'impegno a sistema ma per tutti gli enti di accoglienza è stata inserita la dichiarazione di impegno personalizzata.</br>"
        End If
        If txtNoteEsplicative.Text.Trim.Length > 1000 Then
            errori = errori + "E' stato superato il numero massimo di caratteri inseribili nelle note esplicative (1.000 caratteri). </br>"
        End If
		'If Not chkFiniIstituzionali.Checked Then
		'	errori = errori + "Non è stata spuntata la dichiarazione dei fini istituzionali.</br>"
		'End If
		'If divStatuto.Visible AndAlso Not chkStatuto.Checked Then
		'	errori = errori + "Non è stata spuntata la dichiarazione di esssere senza fini di lucro.</br>"
		'End If
		'If Not chkAttivita.Checked Then
		'	errori = errori + "Non è stata spuntata la dichiarazione di aver svolto attività da almeno 3 anni.</br>"
		'End If

		If String.IsNullOrEmpty(errori) Then
			errori = LeggiStoreVerificaCompletamentoAccreditamento(Session("IdEnte"))
		End If

		Return errori

	End Function
	Protected Sub btnPresenta_Click(sender As Object, e As EventArgs)
		Dim errori As String = Controlli()
		Dim documentoRiepilogo As Allegato = Session("LoadedRiepilogo")
		Dim isRappresentanteLegale As Boolean = ddlTipoRappresentante.SelectedValue = TIPO_RAPPRESENTANTE_LEGALE

		If documentoRiepilogo Is Nothing Then
			errori = errori + "Non è stato caricata la Domanda di Adeguamento.</br>"
		End If

		If Not String.IsNullOrEmpty(errori) Then
			lblError.Text = errori
			Exit Sub
		End If

		Dim managerEnte = New clsEnte()

		Dim _documentoDaFirmare As Byte() = Session("DocumentoDaFirmare")

		If IsNothing(_documentoDaFirmare) Then
			Log.Warning(LogEvent.FIRMA_NON_VALIDA, message:="Non è stata scaricata la Domanda di Adeguamento da firmare ed allegare")
			lblError.Text = ("Non è stata scaricata la Domanda di Adeguamento da firmare ed allegare")
			Exit Sub
		End If

		If Not ConfigurationSettings.AppSettings("DisabilitaFirma") = "true" Then
			'Verifico se è stato o meno scelto il Rappresentante Legale o il coordinatore Responsabile
			Dim responsabile As String
			If isRappresentanteLegale Then
				Dim rappresentanteLegale = managerEnte.GetRappresentanteLegale(Session("IdEnte"), Session("conn"))
				If rappresentanteLegale Is Nothing Then
					lblError.Text = ("Non è presente il Rappresentante Legale dell'Ente")
					Exit Sub
				End If
				responsabile = rappresentanteLegale.CodiceFiscale

			Else
				Dim coordinatore = managerEnte.GetCoordinatoreResponsabile(Session("IdEnte"), Session("conn"))
				If coordinatore Is Nothing Then
					lblError.Text = ("Non è presente il " + TIPO_COORDINATORE_RESPONSABILE)
					Exit Sub
				End If
				responsabile = coordinatore.CodiceFiscale

			End If
			Dim sc As New SignChecker(documentoRiepilogo.Blob)
			If Not sc.checkSignature(responsabile) Then
				Log.Warning(LogEvent.FIRMA_NON_VALIDA, sc.getLog())
				lblError.Text = ("Il documento non è firmato digitalmente o non è firmato dal " & ddlTipoRappresentante.SelectedValue)
				Exit Sub
			End If



			If Not sc.compareSignedNotSigned(documentoRiepilogo.Blob, _documentoDaFirmare) Then
				Log.Warning(LogEvent.FIRMA_NON_VALIDA, message:="Il file firmato non corrisponde alla Domanda di Adeguamento da firmare ed allegare")
				lblError.Text = ("Il file firmato non corrisponde alla Domanda di Adeguamento da firmare ed allegare")
				Exit Sub
			End If

		End If


		'Aggiunto da Alessandra Taballione il 22/04/2005
		'eseguo la query per leggere di che tipologia di classe si tratta
		'CodiceRegione ***********************************
		Dim VstrWhereCondition() As String
		Dim strCodiceGenerato As String
		Dim MyTransaction As System.Data.SqlClient.SqlTransaction 'Transazione che utilizzo per accaertarmi che tutte le operazioni vadano a buon fine
		Dim CmdAggiornaDB As Data.SqlClient.SqlCommand 'Commadn locale che utlizzo per aggiornare i dati nella database
		Dim BlnCodicePresente As Boolean = False
		Dim myQuerySql As String
		Dim myDataReader As SqlDataReader
		Dim idEnteFase As Integer = RicavaIdEnteFase(Session("IdEnte"))

		Try
			'Inizializzo la tranzazione assegnandole un nome univoco
			MyTransaction = CType(IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")), System.Data.SqlClient.SqlConnection).BeginTransaction(Session("IdEnte") & "_" & Session("Utente"))

			'ridimensiono il vettore delle condizioni a zero
			ReDim VstrWhereCondition(0)

			VstrWhereCondition(0) = ""

			'Eseguo la query per leggere di che tipologia di classe si tratta
			myQuerySql = "Select IsNull(CodiceRegione,'') as CodiceRegione,IdClasseAccreditamentoRichiesta From Enti Where IdEnte = " & Session("IdEnte")
			ChiudiDataReader(myDataReader)
			myDataReader = ClsServer.CreaDatareader(myQuerySql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")), , MyTransaction)
			myDataReader.Read()
			Dim codiceRegione As String = myDataReader("CodiceRegione")
			Dim idClasseAccreditamentoRichiesta As Integer = CInt(myDataReader("idclasseaccreditamentoRichiesta"))
			ChiudiDataReader(myDataReader)
			'Generazione del codice NZ nel caso in cui l'ente non abbia il codice NZ
			If codiceRegione = "" Then
				'Associazione della classe e del nuovo codice NZ
				Dim StrCodiceRegione As String
				StrCodiceRegione = CodiceGenerato(idClasseAccreditamentoRichiesta, MyTransaction)
				'ChiudiDataReader(myDataReader) gia' chiuso in CodiceGEnerato

				'If StrCodiceRegione = "NZ" Then
				'    txtCodiceRegione.Value = "NZ00001"
				'Else
				'    txtCodiceRegione.Value = StrCodiceRegione
				'End If
				If StrCodiceRegione = "SU" Then
					txtCodiceRegione.Value = "SU00001"
				Else
					txtCodiceRegione.Value = StrCodiceRegione
				End If
			Else
				BlnCodicePresente = True
				txtCodiceRegione.Value = codiceRegione
			End If

			'Controllo se lo stato dell'ente risultava chiuso
			myQuerySql = "Select StatiEnti.PresentazioneProgetti,StatiEnti.DefaultStato,StatiEnti.Chiuso,StatiEnti.Sospeso,StatiEnti.Istruttoria From Enti " &
					 "INNER JOIN StatiEnti ON StatiEnti.IdStatoEnte = Enti.IdStatoEnte " &
					 "Where IdEnte = " & Session("IdEnte")
			ChiudiDataReader(myDataReader)
			myDataReader = ClsServer.CreaDatareader(myQuerySql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")), , MyTransaction)
			myDataReader.Read()

			'Modifica dello stato dell'ente
			Dim StrCronologia As String = ""
			myQuerySql = "UpDate Enti Set IdRegioneAppartenenza = dbo.RegioneAppartenenza(" & Session("IdEnte") & "),IdRegioneCompetenza = dbo.RegioneCompetenza(" & Session("IdEnte") & "),CodiceRegione = '" & ClsServer.NoApice(txtCodiceRegione.Value) & "'"
			If myDataReader.Item("PresentazioneProgetti") = True And myDataReader.Item("DefaultStato") = False And
				myDataReader.Item("Chiuso") = False And myDataReader.Item("Sospeso") = False And myDataReader.Item("Istruttoria") = False Then 'Imposto lo stato "In Adeguamento" se risulta "Attivo"
				'Mette lo stato di ADEGUAMENTO
				myQuerySql = myQuerySql & ",DataUltimaRichiestaAdeguamento=GetDate(),IdStatoEnte = (Select IdStatoEnte From StatiEnti Where Istruttoria=1 And statoente <> 'Istruttoria') "
				'Inserimento nella cronologia dell'ente della modifica dello stato
				StrCronologia = "Insert Into CronologiaEntiStati(IdEnte,IdStatoEnte,DataCronologia,IdTipoCronologia,UserNameAccreditatore,Forzatura)  select idente,idstatoente,GetDate(),0,'" & ClsServer.NoApice(Session("Utente")) & "',0 from enti where idente=" & Session("idEnte")
			ElseIf myDataReader.Item("PresentazioneProgetti") = False And myDataReader.Item("DefaultStato") = False And
			myDataReader.Item("Chiuso") = False And myDataReader.Item("Istruttoria") = False Then                         'Imposto lo stato "Istruttoria" se risulta "Registrato" o "Chiuso"
				'Mette lo stato di ISTRUTTORIA
				myQuerySql = myQuerySql & ",DataRichiestaAccreditamento=GetDate(),IdStatoEnte = (Select IdStatoEnte From StatiEnti Where Istruttoria = 1 And StatoEnte = 'Istruttoria') "
				'Inserimento nella cronologia dell'ente della modifica dello stato
				StrCronologia = "Insert Into CronologiaEntiStati(IdEnte,IdStatoEnte,DataCronologia,IdTipoCronologia,UserNameAccreditatore,Forzatura)  select idente,idstatoente,GetDate(),0,'" & ClsServer.NoApice(Session("Utente")) & "',0 from enti where idente=" & Session("idEnte")
			End If
			myQuerySql = myQuerySql & " Where IdEnte = " & Session("IdEnte")
			myDataReader.Close()

			'Inserimento dei dati nella base dati
			If StrCronologia.Length <> 0 Then
				CmdAggiornaDB = New SqlClient.SqlCommand(StrCronologia, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")), MyTransaction)
				CmdAggiornaDB.ExecuteNonQuery()
			End If
			CmdAggiornaDB = New SqlClient.SqlCommand(myQuerySql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")), MyTransaction)
			CmdAggiornaDB.ExecuteNonQuery()

			'vado a ciclarmi gli enti figlio e fado a generare il loro codice regione, lo metto nell'array per poi scaricare le operazioni nel db
			myQuerySql = "Select EntiRelazioni.IdEnteFiglio From EntiRelazioni INNER JOIN Enti ON EntiRelazioni.IdEnteFiglio = Enti.IdEnte Where IdEntePadre = " & Session("IdEnte") & " And IsNull(Enti.CodiceRegione,'') = ''"
			myDataReader = ClsServer.CreaDatareader(myQuerySql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")), , MyTransaction)
			While myDataReader.Read
				If VstrWhereCondition(0) = "" Then
					VstrWhereCondition(0) = "where idente=" & CInt(myDataReader("IdEnteFiglio"))
				Else
					ReDim Preserve VstrWhereCondition(UBound(VstrWhereCondition) + 1)
					VstrWhereCondition(UBound(VstrWhereCondition)) = "where idente=" & CInt(myDataReader("identefiglio"))
				End If
			End While
			ChiudiDataReader(myDataReader)
			'controllo se ci sono codici regione da scaricare sugli enti figlio
			If VstrWhereCondition(0) <> "" Then
				'ciclo tutti gli enti figlio
				Dim IntX As Integer
				For IntX = 0 To UBound(VstrWhereCondition)
					'Genero il codice che devo associare all'ente figlio
					myQuerySql = "SELECT MAX(Enti.CodiceRegione) + CHAR(LEFT(MAX(Convert(Varchar(5),ASCII(SUBSTRING " &
							 "(enti_1.CodiceRegione,8,1))) + Convert(varchar(5),right(enti_1.CodiceRegione,2))) + 1, " &
							 "LEN(MAX(Convert(varchar(5),ascii(SUBSTRING ( enti_1.CodiceRegione , 8 , 1 ))) + " &
							 "Convert(Varchar(5),right(enti_1.CodiceRegione,2))) + 1) - 2)) + right(max(convert(varchar(5), " &
							 "ASCII(SUBSTRING(enti_1.CodiceRegione,8,1)))+convert(varchar(5),right(enti_1.CodiceRegione,2))) + 1,2) as CodiceGenerato " &
							 "FROM Enti " &
							 "INNER JOIN entirelazioni ON enti.IDEnte = entirelazioni.IDEntePadre " &
							 "INNER JOIN enti enti_1 ON entirelazioni.IDEnteFiglio = enti_1.IDEnte " &
							 "WHERE (enti.idente=" & CInt(Session("IdEnte")) & ")"

					myDataReader = ClsServer.CreaDatareader(myQuerySql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")), , MyTransaction)
					myDataReader.Read()
					If Not IsDBNull(myDataReader("CodiceGenerato")) Then
						'codice generato
						strCodiceGenerato = myDataReader("CodiceGenerato")
					Else
						'se restituisce null è il primo quindi forzo il primo codice generato in A00 + il codice del padre
						strCodiceGenerato = txtCodiceRegione.Value & "A00"
					End If
					ChiudiDataReader(myDataReader)
					'Eseguo la query di aggiornamento dell'ente figlio
					myQuerySql = "UpDate Enti Set CodiceRegione = '" & strCodiceGenerato & "' " & VstrWhereCondition(IntX)
					CmdAggiornaDB = New SqlClient.SqlCommand(myQuerySql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")), MyTransaction)
					CmdAggiornaDB.ExecuteNonQuery()
				Next
			End If

			'Eseguo la query di aggiornamento competenze degli enti figlio
			myQuerySql = "UpDate Enti Set idregioneappartenenza = c.idregioneappartenenza, idregionecompetenza = c.idregionecompetenza from enti inner join entirelazioni b on idente = b.identefiglio inner join enti c on c.idente = b.identepadre where b.identepadre = " & CInt(Session("IdEnte")) & ""
			CmdAggiornaDB = New SqlClient.SqlCommand(myQuerySql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")), MyTransaction)
			CmdAggiornaDB.ExecuteNonQuery()

			'Controllo se è stato impostato il flag
			myDataReader = ClsServer.CreaDatareader("Select IsNull(FlagForzaturaAccreditamento,0) From Enti Where IdEnte = " & Session("IdEnte"), IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")), , MyTransaction)
			myDataReader.Read()
			If myDataReader.Item(0) = True Then
				myDataReader.Close()

				'Imposto a zero il bit di Flag Forzatura Accreditamento
				myQuerySql = "UpDate Enti Set FlagForzaturaAccreditamento = 0 Where IdEnte = " & Session("IdEnte")
				CmdAggiornaDB = New SqlClient.SqlCommand(myQuerySql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")), MyTransaction)
				CmdAggiornaDB.ExecuteNonQuery()

				'Inserimento della cronologia di chiusura dell'apertura accreditamento
				myQuerySql = "Insert Into CronologiaApertureAccreditamento (IdEnte,Data,Utente,TipoOperazione) Values (" & Session("IdEnte") & ",GetDate(),'" & Session("Utente") & "',2)"
				CmdAggiornaDB = New SqlClient.SqlCommand(myQuerySql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")), MyTransaction)
				CmdAggiornaDB.ExecuteNonQuery()


			Else
				myDataReader.Close()
			End If


			'Inserimento Riepilogo

			Dim insertRiepilogoCommand As New SqlCommand("", IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")), MyTransaction)
			Dim idRiepilogo As Integer = SalvaAllegato(documentoRiepilogo, TipoFile.RICHIESTA_ADEGUAMENTO, idEnteFase, insertRiepilogoCommand)

			'Inserimento protocollo
			myQuerySql = "insert into ProtocolloDomanda (Idente,TipoDomanda,IdAllegatoDomanda) VALUES (" & Session("IdEnte") & ",2," & idRiepilogo & ")"
			CmdAggiornaDB = New SqlClient.SqlCommand(myQuerySql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")), MyTransaction)
			CmdAggiornaDB.ExecuteNonQuery()

			Dim dichiarazioneImpegno As Integer = 0
			If chkImpegno.Checked Then
				dichiarazioneImpegno = 1
			End If
			'Associazione dichiarazione d'impegno
			myQuerySql = "update enti set DichiarazioneImpegno = " & dichiarazioneImpegno & " WHERE IDEnte=" & Session("IdEnte")
			CmdAggiornaDB = New SqlClient.SqlCommand(myQuerySql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")), MyTransaction)
			CmdAggiornaDB.ExecuteNonQuery()

			'Associazione dichiarazione d'impegno e privacy alla fase
			myQuerySql = "update EntiFasi set DichiarazioneImpegno = " & dichiarazioneImpegno & ", Privacy=1 WHERE IDEnteFase=" & idEnteFase
			CmdAggiornaDB = New SqlClient.SqlCommand(myQuerySql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")), MyTransaction)
			CmdAggiornaDB.ExecuteNonQuery()

			If (Session("IdStatoEnte") = 6 Or Session("IdStatoEnte") = 8) Then
				'RISULTATO STORE PROCEDURE DI MIGRAZIONE DATI: False=OK  True=ERR
				If StoreAggiornDB(Session("IdEnte"), MyTransaction) = True Then
					MyTransaction.Rollback(Session("IdEnte") & "_" & Session("Utente"))
					Exit Sub
				End If
			End If
			MyTransaction.Commit()

			'Aggiorno la Session Contenente lo stato dell'ente
			myQuerySql = "Select IdStatoEnte From Enti Where IdEnte = " & Session("IdEnte")
			myDataReader = ClsServer.CreaDatareader(myQuerySql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
			myDataReader.Read()
			Session("IdStatoEnte") = myDataReader.Item("IdStatoEnte")
			myDataReader.Close()
			myDataReader = Nothing

			'aggiorno lo stato della session della forzatura in sessione (parametro per la connessione al db snapshot)
			Session("FlagForzatura") = False
		Catch ex As Exception
			'ROLLBACK
			MyTransaction.Rollback(Session("IdEnte") & "_" & Session("Utente"))
			Log.Error(LogEvent.PRESENTAZIONE_ERRORE, "Salvataggio dati", ex)
			lblError.Text = "Errore nel salvataggio dei dati. Contattare l'assistenza tecnica."
			Exit Sub
		End Try
		Dim intIDEnteFase As Integer
		intIDEnteFase = RicavaIdEnteFase(Session("IdEnte"))
		Dim strMg As String
		Dim Esito As Integer
		ClsServer.ChiudiFaseEnte(intIDEnteFase, Session("Utente"), Session("conn"), strMg, Esito)
		If Request.QueryString("tipologia") = "Enti" Then
			'ChiudiFaseEnte(intIDEnteFase)

			'CHIAMO LO STORED

			Select Case Esito
				Case 0 'ERRORE
					lblError.Text = strMg
					Exit Sub
				Case 1 'GENERAZIONE COPERTINA PRESENTAZIONE SENZA BOXSEDI
                    If AppSettings("IsTest") <> "1" Then StampaCopertina(intIDEnteFase)
					Response.Redirect("WfrmAlbero.aspx?tipologia=Enti&Presenta=1&IDEnteFase=" & intIDEnteFase & "&CodiceRegione='" & txtCodiceRegione.Value & "'")
				Case 2 'GENERAZIONE DEL BOXSEDI (RICHIAMO WSDOCUMENTAZIONE)
					'modificato il 02/01/2018 da s.c.
					'chiamo WfrmInfoPresentazioneAccreditamento 
                    If AppSettings("IsTest") <> "1" Then GenerazioneAllegato6_ElencoSedi(intIDEnteFase)
					Response.Redirect("WfrmInfoPresentazioneAccreditamento.aspx?IDEnteFase=" & intIDEnteFase & "")
			End Select




			'If Esito = 0 Then
			'    msgErrore.Text = strMg
			'    Exit Sub
			'Else
			'    'modificato il 02/01/2018 da s.c.
			'    'chiamo WfrmInfoPresentazioneAccreditamento 
			'    GenerazioneAllegato6_ElencoSedi(intIDEnteFase)
			'    Response.Redirect("WfrmInfoPresentazioneAccreditamento.aspx?IDEnteFase=" & intIDEnteFase & "")

			'    'StampaCopertina(intIDEnteFase)
			'    'Response.Redirect("WfrmAlbero.aspx?tipologia=Enti&Presenta=1&CodiceRegione='" & txtCodiceRegione.Value & "'")
			'End If
		End If
		Response.Redirect("~/WfrmAlbero.aspx?tipologia=Enti&Arrivo=WfrmMain.aspx&VediEnte=1")


	End Sub


	Protected Sub btnUploadRiepilogo_Click(sender As Object, e As EventArgs)
		'Verifica se è stato inserito il file
		If fileRiepilogo.PostedFile Is Nothing Or String.IsNullOrEmpty(fileRiepilogo.PostedFile.FileName) Then
			lblErroreRiepilogo.Text = ("Non è stato scelto nessun file")
			popUploadRiepilogo.Show()
			Exit Sub
		End If
		'Controllo Tipo File
		If Not clsGestioneDocumentiAccreditamento.VerificaEstensioneFileAccreditamento(fileRiepilogo) Then
			lblErroreRiepilogo.Text = ("Il formato file non è corretto. È possibile associare documenti nel formato .PDF o .PDF.P7M")
			popUploadRiepilogo.Show()
			Exit Sub
		End If
		'Controlli dimensioni del file
		Dim fs = fileRiepilogo.PostedFile.InputStream
		Dim iLen As Integer = CInt(fs.Length)
		Dim bBLOBStorage(iLen) As Byte

		If iLen <= 0 Then
			lblErroreRiepilogo.Text = ("Attenzione. Impossibile caricare documento vuoto.")
			popUploadRiepilogo.Show()
			Exit Sub
		End If
		If iLen > 20971520 Then
			lblErroreRiepilogo.Text = ("Attenzione. La dimensione massima della Lettera di accordo con sede estera è di 20 MB.")
			popUploadRiepilogo.Show()
			Exit Sub
		End If
		Dim numBytesToRead As Integer = CType(fs.Length, Integer)
		Dim numBytesRead As Integer = 0

		While (numBytesToRead > 0)
			' Read may return anything from 0 to numBytesToRead.
			Dim n As Integer = fs.Read(bBLOBStorage, numBytesRead, numBytesToRead)
			' Break when the end of the file is reached.
			If (n = 0) Then
				Exit While
			End If
			numBytesRead = (numBytesRead + n)
			numBytesToRead = (numBytesToRead - n)

		End While

		fs.Close()

		Dim hashValue As String
		hashValue = GeneraHash(bBLOBStorage)

		Dim estensione As String = System.IO.Path.GetExtension(fileRiepilogo.PostedFile.FileName)
		If UCase(estensione) = ".P7M" Then estensione = ".pdf.p7m"

		Dim allegato As New Allegato() With {
		 .Blob = bBLOBStorage,
		 .Filename = "DOMANDAADEGUAMENTO_" & hfCodiceFiscaleEnte.Value & estensione,
		 .Hash = hashValue,
		 .DataInserimento = Date.Now,
		 .Filesize = iLen
		}
		Session("LoadedRiepilogo") = allegato
		lblErroreRiepilogo.Text = String.Empty
		VisualizzaFile()
	End Sub

	Sub VisualizzaFile()
		Dim documentoRiepilogo As Allegato = Session("LoadedRiepilogo")

		If documentoRiepilogo IsNot Nothing Then
			'Se Il CV è caricato in Sessione (Inserimento)
			NorowRiepilogo.Visible = False
			rowRiepilogo.Visible = True
			txtCVFilename.Text = documentoRiepilogo.Filename
			txtCVHash.Text = documentoRiepilogo.Hash
			txtCVData.Text = documentoRiepilogo.DataInserimento.ToString("dd/MM/yyyy HH:mm:ss")
		Else
			'Se Il CV non è ancora caricato
			NorowRiepilogo.Visible = True
			rowRiepilogo.Visible = False
		End If

	End Sub

	Private Function GeneraHash(ByVal FileinByte() As Byte) As String
		Dim tmpHash() As Byte

		tmpHash = New MD5CryptoServiceProvider().ComputeHash(FileinByte)

		GeneraHash = ByteArrayToString(tmpHash)
		Return GeneraHash
	End Function
	Private Function ByteArrayToString(ByVal arrInput() As Byte) As String
		Dim i As Integer
		Dim sOutput As New StringBuilder(arrInput.Length)
		For i = 0 To arrInput.Length - 1
			sOutput.Append(arrInput(i).ToString("X2"))
		Next
		Return sOutput.ToString()
	End Function

	Protected Sub btnDeleteRiepilogo_Click(sender As Object, e As ImageClickEventArgs)
		Session("LoadedRiepilogo") = Nothing
		VisualizzaFile()
	End Sub

	Protected Sub btnDownloadRiepilogo_Click(sender As Object, e As ImageClickEventArgs)
		Dim allegato As Allegato = Session("LoadedRiepilogo")
		Dim entity As New Entity() With {
			.Id = allegato.Id,
			.Name = "Allegato"
		}
		Log.Information(LogEvent.PRESENTAZIONE_DOWNLOAD, "Domanda di Adeguamento caricata", entity:=entity)
		Response.Clear()
		Response.ContentType = "Application/pdf"
		Response.AddHeader("Content-Disposition", "attachment; filename=" & allegato.Filename)
		Response.BinaryWrite(allegato.Blob)
		Response.End()
	End Sub

	'creata da jbagnani il 05.01.2005
	Function CodiceGenerato(ByVal idclasseselezionata As Integer, Optional ByVal Transazione As System.Data.SqlClient.SqlTransaction = Nothing) As String
		Dim strLocal As String
		Dim strDescrizioneClasse As String
		Dim intIdClasse As Integer
		Dim strNuovoCodice As String
		Dim myDataReader As SqlDataReader

		strLocal = "select IDClasseAccreditamento, classeaccreditamento from classiaccreditamento where idclasseaccreditamento=" & idclasseselezionata
		ChiudiDataReader(myDataReader)
		'eseguo la query per leggere di che tipologia di classe si tratta
		myDataReader = ClsServer.CreaDatareader(strLocal, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")), , Transazione)

		'controllo quale classe ho appena selezionato
		If myDataReader.HasRows Then
			myDataReader.Read()
			strDescrizioneClasse = IIf(IsDBNull(myDataReader("classeaccreditamento")) = True, "", myDataReader("classeaccreditamento"))
			intIdClasse = myDataReader("IDClasseAccreditamento")
		End If

		ChiudiDataReader(myDataReader)
		'*******************  VECCHIA CREAZIONE CODICE REGION NZ per gli encti SCN**************************
		''preparo la stringa con il codice max + 1
		'strLocal = "select replicate('0', len(max(CodiceRegione)) - 2 - len((max(right(CodiceRegione,len(CodiceRegione) - 2)) + 1))) + convert(varchar,max(right(CodiceRegione,len(CodiceRegione) - 2)) + 1) as MAXX from enti where len(codiceregione)=7"

		''eseguo la query per creare il max codice regione
		'myDataReader = ClsServer.CreaDatareader(strLocal, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")), , Transazione)

		''controllo quale classe ho appena selezionato
		'If myDataReader.HasRows Then
		'    myDataReader.Read()
		'    strNuovoCodice = "NZ" & myDataReader("MAXX")
		'End If
		''e di conseguenza creo la stringa per inserire il codiceregione(codente)
		'Select Case strDescrizioneClasse
		'    Case "Classe 1"
		'        CodiceGenerato = strNuovoCodice
		'    Case "Classe 2"
		'        CodiceGenerato = strNuovoCodice
		'    Case "Classe 3"
		'        CodiceGenerato = strNuovoCodice
		'    Case "Classe 4"
		'        CodiceGenerato = strNuovoCodice
		'    Case Else
		'        CodiceGenerato = "null"
		'End Select
		'******************* fine  VECCHIA CREAZIONE CODICE REGION NZ per gli encti SCN**************************


		'*******************  NUOVA CREAZIONE CODICE REGIONE SU per gli encti SCU **************************
		'creato da simona cordella il 25/07/2017 GENERO IL NUVO CODICE PER GLI ENTI SCU 

		strLocal = "SELECT ISNULL(replicate('0', len(max(CodiceRegione)) - 2 - len((max(right(CodiceRegione,len(CodiceRegione) - 2)) + 1))) + convert(varchar,max(right(CodiceRegione,len(CodiceRegione) - 2)) + 1),'00001') as MAXX from enti where len(codiceregione)=7 AND LEFT(CodiceRegione,2)='CP'"

		'eseguo la query per creare il max codice regione
		myDataReader = ClsServer.CreaDatareader(strLocal, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")), , Transazione)

		If myDataReader.HasRows Then
			myDataReader.Read()
			strNuovoCodice = "CP" & myDataReader("MAXX")
		End If

		' creo la stringa per inserire il codiceregione(codente)
		Select Case intIdClasse
			Case 1 To 4 'CLASSI DA 1 A 4 
				CodiceGenerato = strNuovoCodice
			Case 8, 9 'SCU - Albo Generale E SCU - Sezione RPA
				CodiceGenerato = strNuovoCodice
			Case Else 'Enti Vincoli Associativi, Consortili, Federativi, Canonico-Pastorali -   Nessuna(Classe) - Enti In Partenariato
				CodiceGenerato = "null"
		End Select
		'******************* FINE  NUOVA CREAZIONE CODICE REGIONE SU per gli encti SCU **************************
		ChiudiDataReader(myDataReader)
	End Function

	Private Function RicavaIdEnteFase(ByVal IdEnte As Integer) As Integer
		Dim IdEF As Integer
		Dim myQuerySql As String
		Dim myDataReader As SqlDataReader

		myQuerySql = " Select IdEnteFase from EntiFasi where idente=" & Session("IdEnte") & "  AND STATO =1 and TipoFase IN (1,2) and GETDATE() between DataInizioFase and DataFineFase order by IdEnteFase Desc "
		ChiudiDataReader(myDataReader)
		myDataReader = ClsServer.CreaDatareader(myQuerySql, Session("conn"))
		myDataReader.Read()
		IdEF = myDataReader("IdEnteFase")
		If Not myDataReader Is Nothing Then
			myDataReader.Close()
			myDataReader = Nothing
		End If

		Return IdEF
	End Function

	'store che migra i dati nel DB SnapShot da unsc_ lettura
	Private Function StoreAggiornDB(ByVal IdEnte As Long, ByVal Transazione As System.Data.SqlClient.SqlTransaction) As Boolean
		Dim CustOrderHist As SqlClient.SqlCommand
		CustOrderHist = New SqlClient.SqlCommand
		CustOrderHist.CommandType = CommandType.StoredProcedure
		CustOrderHist.CommandText = "SP_MIGRAZIONE_DATI"
		CustOrderHist.Connection = IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn"))
		CustOrderHist.Transaction = Transazione

		Dim paramEnte As SqlClient.SqlParameter
		paramEnte = New SqlClient.SqlParameter
		paramEnte.ParameterName = "@IdEnte"
		paramEnte.SqlDbType = SqlDbType.Int
		CustOrderHist.Parameters.Add(paramEnte)

		Dim paramValore As SqlClient.SqlParameter
		paramValore = New SqlClient.SqlParameter
		paramValore.ParameterName = "@Valore"
		paramValore.SqlDbType = SqlDbType.Bit
		paramValore.Direction = ParameterDirection.Output
		CustOrderHist.Parameters.Add(paramValore)

		Dim Reader As SqlClient.SqlDataReader
		CustOrderHist.Parameters("@IdEnte").Value = IdEnte
		Reader = CustOrderHist.ExecuteReader()
		StoreAggiornDB = CustOrderHist.Parameters("@Valore").Value
		ChiudiDataReader(Reader)
	End Function

	Sub StampaCopertina(ByVal idEnteFase As Integer)
		Dim numDocumenti As Integer
		Dim script As String
		Dim myQuerySql As String
		Dim myDataReader As SqlDataReader
		myQuerySql = " Select count(isnull(identedocumento,0)) as NumDocumenti from EntiDocumenti where IdEnteFase=" & idEnteFase & ""
		ChiudiDataReader(myDataReader)
		myDataReader = ClsServer.CreaDatareader(myQuerySql, Session("conn"))
		myDataReader.Read()
		numDocumenti = myDataReader("NumDocumenti")

		ChiudiDataReader(myDataReader)
		script = "<script>" & vbCrLf
		If numDocumenti > 0 Then 'copertina con elenco documenti
			script &= "window.open(""WfrmReportistica.aspx?sTipoStampa=39&IDEnteFase=" & idEnteFase & """, """", ""height=800,width=800, ,dependent=no,scrollbars=no,status=no,resizable=yes"")" & vbCrLf
		Else
			script &= "window.open(""WfrmReportistica.aspx?sTipoStampa=40&IDEnteFase=" & idEnteFase & """, """", ""height=800,width=800, ,dependent=no,scrollbars=no,status=no,resizable=yes"")" & vbCrLf
		End If
		script &= ("</script>")
		Response.Write(script)
	End Sub


	Private Sub GenerazioneAllegato6_ElencoSedi(ByVal IdEnteFase As Integer)

		Dim localWS As New WS_Editor.WSMetodiDocumentazione
		Dim ds As DataSet
		Dim i As Integer
		Dim strCodiceProgetto As String
		Dim ResultAsinc As IAsyncResult

		Dim cmdUp As SqlCommand

		'update InLavorazione a 1 
		'aggiunto flag LAVORAZIONE =1 da simona cordella il 02/01/2018
		cmdUp = New Data.SqlClient.SqlCommand(" UPDATE EntiFasi" &
											  " SET InLavorazione= 1  " &
											  " WHERE IdEnteFase=" & IdEnteFase & "", Session("conn"))
		cmdUp.ExecuteNonQuery()
		cmdUp.Dispose()



		'richiamo WSDocumentazione
		localWS.Url = ConfigurationSettings.AppSettings("URL_WS_Documentazione")
		localWS.Timeout = 1000000




		ResultAsinc = localWS.BeginGenerazioneAllegato6_ElencoSedi(IdEnteFase, Session("Utente"), Nothing, "")



	End Sub

	Public Sub Protocolla()
		Dim protocolloAutenticazioneService = New WS_SIGeD_Auth.SIGED_AUTH()
		Dim responseToken As String
		Try

			responseToken = protocolloAutenticazioneService.SWS_NEWSESSION("", "", "")

		Catch e As Exception

			Log.Error(LogEvent.REGISTRAZIONE_ERRORE_PROTOCOLLO, "Connessione Servizio Autenticazione", e)
		End Try

		Dim client As New WS_SIGeD.SIGED_WS()

	End Sub
	Private Sub VerificaSessione()
		If Not Session("LogIn") Is Nothing Then
			If Session("LogIn") = False Then
				Response.Redirect("LogOn.aspx")
			End If
		Else
			Response.Redirect("LogOn.aspx")
		End If
	End Sub

	Protected Sub ddlTipoRappresentante_SelectedIndexChanged(sender As Object, e As EventArgs)
		If Session("DocumentoDaFirmare") IsNot Nothing Then
			Session("DocumentoDaFirmare") = Nothing
            lblError.Text = "Attenzione! È necessario scaricare nuovamente il documento di riepilogo a seguito della variazione del firmatario"
		End If

	End Sub
End Class

