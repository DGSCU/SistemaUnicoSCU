Imports System.IO
Imports System.Text.RegularExpressions
Imports System.Collections.Generic
Imports System.Data.SqlClient
Imports Ionic.Zip
Imports Logger.Data
Imports System.Security.Cryptography

Public Class WfrmImportRisorseSediEnti
	Inherits SmartPage
	Private Writer As StreamWriter
	Private strNote As String
	Private Tot As Integer
	Private TotOk As Integer
	Private TotKo As Integer
	Private NomeUnivoco As String
	Private xIdAttivita As Integer
	Private strSql As String
	Private dtrGenreico As SqlClient.SqlDataReader


	Sub DisabilitaDivMaschera()
		'DivEsp.Visible = False
		' DivCorso.Visible = False
		DivRuolo.Visible = False
		DivTitoloGiuridico.Visible = False
        DivAmbiti.Visible = False
        DivAttivita.Visible = False
        DivTipologia.Visible = False
        DivSettori.Visible = False
        DivTipoRelazione.Visible = False
        DivImportDocumentiEnte.Visible = False
        DivEsp.Visible = False
        DivCorso.Visible = False
        DivImportCV.Visible = False
    End Sub
	Sub DisabilitaMaschera()
		lblTipoImport.Visible = False
		hplFileRicerca.Visible = False
		LblUp.Visible = False
		txtSelFile.Visible = False
		CmdElabora.Visible = False
	End Sub
	Sub ImpostaMaschera(ByVal Chiamante As String, ByVal AlboEnte As String)
		'TipoImp: riso= risorse, sedi=sedi, enti=enti figlio legati a enti padri
		Dim StrTitoloLegend As String
		Dim StrTitolo As String
		Dim strTestoRicerca As String
		DisabilitaDivMaschera()
		Select Case Chiamante
			Case "riso"
				StrTitoloLegend = "Importazione Risorse"
				StrTitolo = "Import nuove Risorse"
				strTestoRicerca = "Scaricare il file dell Risorse cliccando qui"
				hplFileRicerca.NavigateUrl = "~/download/Master/risorse.csv?v=1.1"
				hplFileRicerca.Text = "File risorse.csv"
				DivRuolo.Visible = True
                DivEsp.Visible = True
                DivCorso.Visible = True
                DivImportCV.Visible = True
			Case "sedi"
				StrTitoloLegend = "Importazione Sedi"
				StrTitolo = "Import nuove Sedi"
				strTestoRicerca = "Scaricare il file delle Sedi cliccando qui"
				hplFileRicerca.NavigateUrl = "~/download/Master/sedi.csv?v=1.1"
				hplFileRicerca.Text = "File sedi.csv"
				DivTitoloGiuridico.Visible = True
				'Response.Write("sedi.csv")
			Case "enti"
				'Response.Write("entiFigli.csv")
				StrTitoloLegend = "Importazione Enti"
				StrTitolo = "Import nuovi Enti di Accoglienza"
				strTestoRicerca = "Scaricare il file Enti di Accoglienza cliccando qui"

				If (AlboEnte = "" Or AlboEnte = "SCU") Then
					hplFileRicerca.NavigateUrl = "~/download/Master/entiAccoglienzaSCU.csv?v=1.1"
					hplFileRicerca.Text = "File entiAccoglienzaSCU.csv"
				Else
					hplFileRicerca.NavigateUrl = "~/download/Master/entiFigli.csv?v=1.1"
					hplFileRicerca.Text = "File entiFigli.csv"
				End If

                DivAmbiti.Visible = True
                DivAttivita.Visible = True
                DivTipologia.Visible = True
                DivTipoRelazione.Visible = True
                DivSettori.Visible = True
                DivImportDocumentiEnte.Visible = True
        End Select

		lblTitoloLegend.Text = StrTitoloLegend
		lblTitolo.Text = StrTitolo
		lblTipoImport.Text = strTestoRicerca
	End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        checkSpid()
        Dim BLOCCO_ACCREDITAMENTO As String
        If Page.IsPostBack = False Then

            Dim AlboEnte As String
            AlboEnte = ClsUtility.TrovaAlboEnte(Session("IdEnte"), Session("conn"))

            'Imposto in sessione il tipo di Import che va effettuato
            Session("TipoImport") = Request.QueryString("TipoImp")
            ImpostaMaschera(Session("TipoImport"), AlboEnte)

            If ClsUtility.ControllaStatoEntePerBloccareMaschereAnagrafica(Session("IdEnte"), IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn"))) = True Then
                DisabilitaDivMaschera()
                DisabilitaMaschera()
                lblMessaggioErrore.Visible = True
                lblMessaggioErrore.Text = "Non e' possibile effettuare l'import dei dati; l'ente non e' attualmente abilitato."

                Exit Sub
            End If


            'Controllo se lo stato dell'Ente permette l'importazione.(Attivo o Registrato)
            Dim strSql As String = ""
            Dim dtrGen As SqlClient.SqlDataReader
            strSql = "SELECT Flagforzaturaaccreditamento FROM enti  " &
               "WHERE enti.IDEnte = " & Session("IdEnte") & " and isnull(flagforzaturaaccreditamento,0) = 1 "
            'INNER JOIN statienti ON enti.IDStatoEnte = statienti.IDStatoEnte
            'AND (statienti.PresentazioneProgetti = 1 OR " & _
            ' "(statienti.PresentazioneProgetti = 0 AND statienti.DefaultStato = 0 " & _
            ' "AND statienti.Chiuso = 0 AND statienti.Sospeso = 0 AND statienti.Istruttoria = 0))"
            dtrGen = ClsServer.CreaDatareader(strSql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
            If dtrGen.HasRows = False Then
                'Lo stato dell'ente non è abilitato all'importazione
                dtrGen.Close()
                dtrGen = Nothing
                lblMessaggioErrore.Visible = True
                lblMessaggioErrore.Text = "Non e' possibile effettuare l'import dei dati; l'ente e' attualmente in fase di valutazione."
                DisabilitaMaschera()
                Exit Sub
            Else
                dtrGen.Close()
                dtrGen = Nothing
            End If

            If Session("TipoImport") = "enti" Or Session("TipoImport") = "sedi" Then
                '*** MODIFICA RICHIESTA IL 20/04/2017 BLOCCO IMPORTAZIONE ENTI FIGLIO (accreditamento/adeguamento) ****
                Dim blnAdeguamento As Boolean
                strSql = "select IdEnteFase from EntiFasi where TipoFase = 2 and GETDATE() between DataInizioFase and DataFineFase and IdEnte = " & Session("IdEnte")
                dtrGen = ClsServer.CreaDatareader(strSql, Session("Conn"))
                If dtrGen.HasRows = True Then
                    blnAdeguamento = True
                Else
                    blnAdeguamento = False
                End If
                dtrGen.Close()
                dtrGen = Nothing

                strSql = "SELECT VALORE  FROM Configurazioni where Parametro='BLOCCO_ACCREDITAMENTO'"
                If Not dtrGen Is Nothing Then
                    dtrGen.Close()
                    dtrGen = Nothing
                End If
                dtrGen = ClsServer.CreaDatareader(strSql, Session("conn"))
                dtrGen.Read()

                BLOCCO_ACCREDITAMENTO = dtrGen("Valore")
                If Not dtrGen Is Nothing Then
                    dtrGen.Close()
                    dtrGen = Nothing
                End If

                strSql = "SELECT IDENTE FROM EntiFasi WHERE IdEnte  = " & Session("IdEnte") & " AND Stato=1 AND TipoFase = 2 AND DataInizioFase > '19/04/2017'"
                dtrGen = ClsServer.CreaDatareader(strSql, Session("Conn"))
                If dtrGen.HasRows = True And ((BLOCCO_ACCREDITAMENTO = "SI" And blnAdeguamento) Or AlboEnte = "SCN") Then
                    dtrGen.Close()
                    dtrGen = Nothing
                    lblMessaggioErrore.Visible = True
                    If Session("TipoImport") = "enti" Then
                        lblMessaggioErrore.Text = "Non e' possibile effettuare l'importazione di nuovi enti di accoglienza."
                    Else
                        lblMessaggioErrore.Text = "Non e' possibile effettuare l'importazione di nuove sedi."
                    End If

                    DisabilitaMaschera()
                    Exit Sub
                End If
                If Not dtrGen Is Nothing Then
                    dtrGen.Close()
                    dtrGen = Nothing
                End If
                '************************************************
            End If
            ''TipoImp: riso= risorse, sedi=sedi, enti=enti figlio legati a enti padri
            'Select Case Session("TipoImport")
            '    Case "riso"
            '        'lblTipoImport.Text = "Risorse"
            '        ''Caricamento della tabella delle note
            '        'dtgRuoli.DataSource = ClsServer.DataSetGenerico("Select Ruolo,DescrAbb From Ruoli Where Import = 1", IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
            '        'dtgRuoli.DataBind()
            '    Case "sedi"
            '        'lblTipoImport.Text = "Sedi"
            '    Case "enti"
            '        'lblTipoImport.Text = "Enti con Vincoli ACF o Accordo di Partenariato"
            '        ''Caricamento della tabella delle note
            '        'dtgTipologia.DataSource = ClsServer.DataSetGenerico("Select Descrizione,CodiceImport From TipologieEnti WHERE Abilitata=1", IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
            '        'dtgTipologia.DataBind()
            '        ''Caricamento della tabella delle note
            '        'dtgRelazione.DataSource = ClsServer.DataSetGenerico("Select TipoRelazione,CodiceImport From TipiRelazioni", IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
            '        'dtgRelazione.DataBind()
            'End Select


            'FZ controllo per disabilitare la maschera nel caso sia un'"R" che sta 
            'controllando i progetti di un' ente che nn ha la stessa regiojneee di competenza
            If ClsUtility.ControlloRegioneCompetenza(Session("TipoUtente"), Session("idEnte"), Session("Utente"), Session("IdStatoEnte"), IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn"))) = False Then
                'lblmessaggiosopra.ForeColor = Color.Red
                lblMessaggioErrore.Text = "Attenzione, l'ente non è di propria competenza. Impossibile effettuare modifiche."
                DisabilitaMaschera()

            End If
            'FZ fine controllo
        End If
    End Sub

	Public Sub CaricaTitoliGiuridici()
		'ROUTINE CHE CARICA I TITOLI GIURIDICI

		Dim strSQL As String
		Dim dsTitoliStudio As SqlDataReader
		Dim AlboEnte As String

		AlboEnte = ClsUtility.TrovaAlboEnte(Session("IdEnte"), Session("conn"))

		strSQL = "Select TitoloGiuridico, DescrizioneAbbreviata "
		strSQL &= " From TitoliGiuridici "
		If AlboEnte = "" Then
			strSQL &= " WHERE (ALBO='SCU' OR ALBO IS NULL) "
		Else
			strSQL &= " WHERE (ALBO='" & AlboEnte & "' OR ALBO IS NULL) "
		End If

		strSQL &= " Order by DescrizioneAbbreviata"


		dsTitoliStudio = ClsServer.CreaDatareader(strSQL, Session("conn"))


		Response.Write("<fieldset>")
		Response.Write("<ul>")
		Do While dsTitoliStudio.Read
			Response.Write("<li><strong>" & dsTitoliStudio.Item("DescrizioneAbbreviata") & "</strong> per <strong>" & dsTitoliStudio.Item("TitoloGiuridico") & "</strong></li>")
		Loop
		Response.Write("</ul>")
		Response.Write("</fieldset>")


		dsTitoliStudio.Close()
		dsTitoliStudio = Nothing
	End Sub

	Public Sub CaricaAmbiti()
		'ROUTINE CHE CARICA I TITOLI GIURIDICI

		Dim AlboEnte As String = ClsUtility.TrovaAlboEnte(Session("IdEnte"), Session("conn"))

		Dim strSQL As String
		Dim dsTitoliStudio As SqlDataReader

		strSQL = "Select IDMacroAmbitoAttività, MacroAmbitoAttività From macroambitiattività"
		If AlboEnte = "" Then
			strSQL &= " WHERE (ALBO='SCU' OR ALBO IS NULL) "
		Else
			strSQL &= " WHERE (ALBO='" & AlboEnte & "' OR ALBO IS NULL)"
		End If
		strSQL &= " Order by IDMacroAmbitoAttività"

		dsTitoliStudio = ClsServer.CreaDatareader(strSQL, Session("conn"))

		' Dim strLocal As String

		'Do While dsTitoliStudio.Read
		'    strLocal = strLocal & "<tr style=FONT:icon;COLOR:navy;><td>-</td><td>" & dsTitoliStudio.Item("MacroAmbitoAttività") & "</td></tr>"
		'Loop

		'Response.Write("<table>" & strLocal & "</table>")

		Response.Write("<fieldset>")
		Response.Write("<ul>")
		Do While dsTitoliStudio.Read
			Response.Write("<li><strong>" & dsTitoliStudio.Item("MacroAmbitoAttività") & "</strong></li>")
		Loop
		Response.Write("</ul>")
		Response.Write("</fieldset>")




		dsTitoliStudio.Close()
		dsTitoliStudio = Nothing
	End Sub

    Public Sub CaricaSettoriAree()
        'ROUTINE CHE CARICA il div SettoriAree

        Dim AlboEnte As String = ClsUtility.TrovaAlboEnte(Session("IdEnte"), Session("conn"))

        Dim strSQL As String
        Dim dsSettoriAree As SqlDataReader

        strSQL = "Select m.Codifica,m.MacroAmbitoAttività,m.Codifica+ae.Codifica as aecodifica,ae.AreaIntervento From macroambitiattività m	inner join AreeEsperienza ae on ae.IdSettore=m.IDMacroAmbitoAttività"
        If AlboEnte = "" Then
            strSQL &= " WHERE (ALBO='SCU' OR ALBO IS NULL) "
        Else
            strSQL &= " WHERE (ALBO='" & AlboEnte & "' OR ALBO IS NULL)"
        End If
        strSQL &= " and isnull(ae.DataFineValidita,'20501231')>GETDATE() and isnull(ae.DataInizioValidita,'20000101')<GETDATE()"
        strSQL &= " Order by IDMacroAmbitoAttività,cast (ae.Codifica as int)"

        dsSettoriAree = ClsServer.CreaDatareader(strSQL, Session("conn"))

        Dim settore As String = ""
        Response.Write("<fieldset>")
        Response.Write("<ul>")
        Do While dsSettoriAree.Read
            If dsSettoriAree.Item("MacroAmbitoAttività") <> settore Then
                If settore <> "" Then
                    Response.Write("</ul>")
                End If
                Response.Write("<li><strong>" & dsSettoriAree.Item("Codifica") & " - " & dsSettoriAree.Item("MacroAmbitoAttività") & "</strong></li>")
                Response.Write("<ul>")
                settore = dsSettoriAree.Item("MacroAmbitoAttività")
            End If
            Response.Write("<li><strong>" & dsSettoriAree.Item("aecodifica") & "</strong> - " & dsSettoriAree.Item("AreaIntervento") & "</li>")
        Loop
        Response.Write("</ul></ul>")
        Response.Write("</fieldset>")




        dsSettoriAree.Close()
        dsSettoriAree = Nothing
    End Sub

	Public Sub CaricaTipologiaEnte()

		Dim strSQL As String
		Dim dsTipologia As SqlDataReader
		Dim AlboEnte As String
		Dim str(1) As String
		str(0) = "PUBBLICO"
		str(1) = "PRIVATO"
		AlboEnte = ClsUtility.TrovaAlboEnte(Session("IdEnte"), Session("conn"))


		strSQL = "Select Descrizione,CodiceImport From TipologieEnti WHERE Abilitata=1 and privato=0 and Iscrizione=1 "
		If AlboEnte = "" Then
			strSQL &= " AND (ALBO='SCU' OR ALBO IS NULL) "
		Else
			strSQL &= " AND (ALBO='" & AlboEnte & "' OR ALBO IS NULL) "
		End If
		strSQL &= " order by Ordinamento"
		dsTipologia = ClsServer.CreaDatareader(strSQL, Session("conn"))
		Response.Write("<fieldset>")
		Response.Write("<ul>")
		Response.Write("<li><strong>" & str(0) & "</strong>")
		Response.Write("<ul>")
		Do While dsTipologia.Read
			Response.Write("<li><strong>" & dsTipologia.Item("CodiceImport") & "</strong> per <strong>" & dsTipologia.Item("Descrizione") & "</strong></li>")
		Loop
		Response.Write("</li>")
		Response.Write("</ul>")
		dsTipologia.Close()
		dsTipologia = Nothing


		strSQL = "Select Descrizione,CodiceImport From TipologieEnti WHERE Abilitata=1 and privato=1  and Iscrizione=1 "
		If AlboEnte = "" Then
			strSQL &= " AND (ALBO='SCU' OR ALBO IS NULL) "
		Else
			strSQL &= " AND (ALBO='" & AlboEnte & "' OR ALBO IS NULL) "
		End If
		strSQL &= " order by Ordinamento"
		dsTipologia = ClsServer.CreaDatareader(strSQL, Session("conn"))
		Response.Write("<br/>")
		Response.Write("<li><strong>" & str(1) & "</strong>")
		Response.Write("<ul>")
		Do While dsTipologia.Read
			Response.Write("<li><strong>" & dsTipologia.Item("CodiceImport") & "</strong> per <strong>" & dsTipologia.Item("Descrizione") & "</strong></li>")
		Loop

		dsTipologia.Close()
		dsTipologia = Nothing

		Response.Write("</li>")
		Response.Write("</ul>")
		Response.Write("</ul>")
		Response.Write("</fieldset>")

	End Sub

	Public Sub CaricaTipoRelazione()

		Dim AlboEnte As String = ClsUtility.TrovaAlboEnte(Session("IdEnte"), Session("conn"))

		Dim strSQL As String
		Dim dsTipo As SqlDataReader

		strSQL = "Select TipoRelazione,CodiceImport From TipiRelazioni"

		If AlboEnte = "" Then
			strSQL &= " WHERE (ALBO='SCU' OR ALBO IS NULL) "
		Else
			strSQL &= " WHERE (ALBO='" & AlboEnte & "' OR ALBO IS NULL)"
		End If
		dsTipo = ClsServer.CreaDatareader(strSQL, Session("conn"))
		Response.Write("<fieldset>")
		Response.Write("<ul>")
		Do While dsTipo.Read
			Response.Write("<li><strong>" & dsTipo.Item("CodiceImport") & "</strong> per <strong>" & dsTipo.Item("TipoRelazione") & "</strong></li>")
		Loop
		Response.Write("</ul>")
		Response.Write("</fieldset>")
		dsTipo.Close()
		dsTipo = Nothing
	End Sub
	Public Sub CaricaRuolo()
		Dim AlboEnte As String = ClsUtility.TrovaAlboEnte(Session("IdEnte"), Session("conn"))

		Dim strSQL As String
		Dim dsRuolo As SqlDataReader

		strSQL = "Select Ruolo,DescrAbb "
		strSQL &= " From Ruoli "
		strSQL &= " LEFT JOIN OrdinamentoRuoli O ON Ruoli.IDRuolo =O.IdRuolo"
		strSQL &= " Where Import = 1"
		If AlboEnte = "" Then
			strSQL &= " AND (ALBO='SCU' OR ALBO IS NULL) "
		Else
			strSQL &= " AND (ALBO='" & AlboEnte & "' OR ALBO IS NULL)"
		End If
		If Session("IdStatoEnte") = 6 Then
			strSQL &= "AND DescrAbb not in ('OLP') "
		End If
		strSQL &= " order by isnull(Ordinamento,99)"

		dsRuolo = ClsServer.CreaDatareader(strSQL, Session("conn"))
		Response.Write("<fieldset>")
		Response.Write("<ul>")
		Do While dsRuolo.Read
			Response.Write("<li><strong>" & dsRuolo.Item("DescrAbb") & "</strong> per <strong>" & dsRuolo.Item("Ruolo") & "</strong></li>")
		Loop
		Response.Write("</ul>")
		Response.Write("</fieldset>")
		dsRuolo.Close()
		dsRuolo = Nothing
	End Sub
	Protected Sub CmdChiudi_Click(sender As Object, e As EventArgs) Handles CmdChiudi.Click
		Response.Redirect("WfrmMain.aspx")
	End Sub

	Protected Sub CmdElabora_Click(sender As Object, e As EventArgs) Handles CmdElabora.Click
		UpLoad()
	End Sub

	Private Sub UpLoad()
		'--- salvataggio del file sul server
		Dim swErr As Boolean
		swErr = False

		If txtSelFile.PostedFile.FileName.ToString <> "" Then
			Try
				NomeUnivoco = Session("TipoImport") & Session("IdEnte") & "_" & Session("Utente") & "_" & Year(Now) & Month(Now) & Day(Now) & Hour(Now) & Minute(Now) & Second(Now)
				Dim file As String
				Dim estensione As String
				file = LCase(txtSelFile.FileName.ToString)
				estensione = file.Substring(file.Length - 4)
				If estensione <> ".csv" Then
					lblMessaggioErrore.Visible = True
					lblMessaggioErrore.Text = "Selezionare il file nel formato CSV."
					Exit Sub
				End If

				txtSelFile.PostedFile.SaveAs(Server.MapPath("upload") & "\" & NomeUnivoco & ".csv")

				CreaTabTemp()

			Catch exc As Exception
				swErr = True
				'Response.Write("<script>" & vbCrLf)
				'Response.Write("parent.fraUp.CambiaImmagine('filenoncaricato.jpg','0')" & vbCrLf)
				'Response.Write("</script>")
				CancellaTabellaTemp()
			End Try

			If swErr = False Then
				Select Case Session("TipoImport")
					Case "riso"
						LeggiCSVriso()
					Case "sedi"
						LeggiCSVsedi()
					Case "enti"
						LeggiCSVenti()
				End Select

				'Response.Write("<script>" & vbCrLf)
				'Response.Write("parent.fraUp.CambiaImmagine('allegatocompletato.jpg','1')" & vbCrLf)
				'Response.Write("</script>")
			End If

		Else
			lblMessaggioErrore.Visible = True
			lblMessaggioErrore.Text = "Selezionare il file da inviare."
			Exit Sub

		End If

	End Sub

	Private Sub CreaTabTemp()
		Dim strSql As String
		Dim cmdCreateTempTable As SqlClient.SqlCommand

		CancellaTabellaTemp()

		Select Case Session("TipoImport")
			Case "riso"
				strSql = "CREATE TABLE #IMP_RISORSE ( " &
				   "[Ruolo] [varchar] (255) COLLATE Latin1_General_CI_AS NULL ," &
				   "[CodiceFiscale] [varchar] (255) COLLATE Latin1_General_CI_AS NULL ," &
				   "[Cognome] [varchar] (255) COLLATE Latin1_General_CI_AS NULL ," &
				   "[Nome] [varchar] (255) COLLATE Latin1_General_CI_AS NULL ," &
				   "[Data] [datetime] NULL ," &
				   "[CodiceIstat] [varchar] (255) COLLATE Latin1_General_CI_AS NULL, " &
				   "[EsperienzaServizioCivile] [tinyint], " &
				   "[Corso] [tinyint], " &
				   "[IdRuolo] [int] NULL " &
				   ") ON [PRIMARY]"
			Case "sedi"
				strSql = "CREATE TABLE #IMP_SEDI ( " &
					"[CodiceFiscaleSede] [varchar] (255) COLLATE Latin1_General_CI_AS NULL ," &
					"[Denominazione] [varchar] (255) COLLATE Latin1_General_CI_AS NULL ," &
					"[CodiceIstatComune] [varchar] (255) COLLATE Latin1_General_CI_AS NULL ," &
					"[CittaEstera] [varchar] (100) COLLATE Latin1_General_CI_AS NULL ," &
					"[Indirizzo] [varchar] (255) COLLATE Latin1_General_CI_AS NULL ," &
					"[Civico] [varchar] (255) COLLATE Latin1_General_CI_AS NULL ," &
					"[CAP] [varchar] (10) COLLATE Latin1_General_CI_AS NULL ," &
					"[PrefissoTel] [varchar] (255) COLLATE Latin1_General_CI_AS NULL ," &
					"[Telefono] [varchar] (255) COLLATE Latin1_General_CI_AS NULL, " &
					"[Palazzina] [nvarchar] (5) COLLATE DATABASE_DEFAULT, " &
					"[Scala] [nvarchar] (5) COLLATE DATABASE_DEFAULT, " &
					"[Piano] [smallint], " &
					"[Interno] [nvarchar] (5) COLLATE DATABASE_DEFAULT, " &
					"[IdTitoloGiuridico] [int], " &
					"[UserNameUltimaModifica] [nvarchar] (10) COLLATE DATABASE_DEFAULT, " &
					"[NMaxVolontari] [smallint], " &
					"[FlagMaxVolontari] [tinyint], " &
					"[UsernameMaxVolontari] [nvarchar] (50) COLLATE DATABASE_DEFAULT, " &
					"[Normativa81] [tinyint], " &
					"[Conformita] [tinyint], " &
					"[Soggettoestero] [nvarchar] (50)  COLLATE DATABASE_DEFAULT, " &
					"[Dichiarazionesoggettoestero] [tinyint], " &
					"[Flgalertnomesede] [tinyint], " &
					"[Flgalertindirizzogoogle] [tinyint], " &
					"[Flgalertindirizzo] [tinyint] " &
					") ON [PRIMARY]"
			Case "enti"
                Dim larghezzaCampoEsperienza As Integer = 1500      'larghezza massima del campo in maschera per descrivere le esperienze nei vari settori
				Dim larghezzaCampoAreeIntervento As Integer = 1000  'contiene le aree di intervento selezionate separate da "," quindi 4 caratteri per ogni area di intervento (per settore)

				strSql = "CREATE TABLE #IMP_ENTI ( " &
				  "[Denominazione] [varchar] (255) COLLATE Latin1_General_CI_AS NULL ," &
				  "[CodiceFiscale] [varchar] (255) COLLATE Latin1_General_CI_AS NULL ," &
				  "[Tipologia] [varchar] (255) COLLATE Latin1_General_CI_AS NULL ," &
				  "[AltroTipoEnte] [varchar] (255) COLLATE Latin1_General_CI_AS NULL ," &
				  "[TipoRelazione] [varchar] (255) COLLATE Latin1_General_CI_AS NULL ," &
				  "[CodiceIstatComune] [varchar] (255) COLLATE Latin1_General_CI_AS NULL ," &
				  "[Indirizzo] [varchar] (255) COLLATE Latin1_General_CI_AS NULL ," &
				  "[Civico] [varchar] (255) COLLATE Latin1_General_CI_AS NULL ," &
				  "[CAP] [varchar] (10) COLLATE Latin1_General_CI_AS NULL ," &
				  "[PrefissoTel] [varchar] (255) COLLATE Latin1_General_CI_AS NULL ," &
				  "[Telefono] [varchar] (255) COLLATE Latin1_General_CI_AS NULL ," &
				  "[Assistenza] [varchar] (255) COLLATE Latin1_General_CI_AS NULL, " &
				  "[ProtezioneCivile] [varchar] (255) COLLATE Latin1_General_CI_AS NULL, " &
				  "[Ambiente] [varchar] (255) COLLATE Latin1_General_CI_AS NULL, " &
				  "[PatrimonioArtistico] [varchar] (255) COLLATE Latin1_General_CI_AS NULL, " &
				  "[PromozioneCulturale] [varchar] (255) COLLATE Latin1_General_CI_AS NULL, " &
				  "[Estero] [varchar] (255) COLLATE Latin1_General_CI_AS NULL, " &
				  "[Agricoltura] [varchar] (255) COLLATE Latin1_General_CI_AS NULL " &
				",[Data Nomina Rappresentante Legale] [varchar] (255) COLLATE Latin1_General_CI_AS NULL " &
				",[Codice Fiscale Rappresentante Legale] [varchar] (255) COLLATE Latin1_General_CI_AS NULL " &
				",[Attivita negli ultimi tre anni] [varchar] (255) COLLATE Latin1_General_CI_AS NULL " &
				",[Attivita per Fini Istituzionali] [varchar] (255) COLLATE Latin1_General_CI_AS NULL " &
				",[Attivita senza scopo di Lucro] [varchar] (255) COLLATE Latin1_General_CI_AS NULL " &
				",[Ultimo Anno Assistenza] [varchar] (255) COLLATE Latin1_General_CI_AS NULL " &
				",[Esperienza Ultimo Anno Assistenza] [varchar] (" & larghezzaCampoEsperienza.ToString & ") COLLATE Latin1_General_CI_AS NULL " &
				",[Penultimo Anno Assistenza] [varchar] (255) COLLATE Latin1_General_CI_AS NULL " &
				",[Esperienza Penultimo Anno Assistenza] [varchar] (" & larghezzaCampoEsperienza.ToString & ") COLLATE Latin1_General_CI_AS NULL " &
				",[Terzultimo Anno Assistenza] [varchar] (255) COLLATE Latin1_General_CI_AS NULL " &
				",[Esperienza Terzultimo Anno Assistenza] [varchar] (" & larghezzaCampoEsperienza.ToString & ") COLLATE Latin1_General_CI_AS NULL " &
				",[Aree d'Intervento Assistenza] [varchar] (" & larghezzaCampoAreeIntervento.ToString & ") COLLATE Latin1_General_CI_AS NULL " &
				",[Ultimo Anno Protezione Civile] [varchar] (255) COLLATE Latin1_General_CI_AS NULL " &
				",[Esperienza Ultimo Anno Protezione Civile] [varchar] (" & larghezzaCampoEsperienza.ToString & ") COLLATE Latin1_General_CI_AS NULL " &
				",[Penultimo Anno Protezione Civile] [varchar] (255) COLLATE Latin1_General_CI_AS NULL " &
				",[Esperienza Penultimo Anno Protezione Civile] [varchar] (" & larghezzaCampoEsperienza.ToString & ") COLLATE Latin1_General_CI_AS NULL " &
				",[Terzultimo Anno Protezione Civile] [varchar] (255) COLLATE Latin1_General_CI_AS NULL " &
				",[Esperienza Terzultimo Anno Protezione Civile] [varchar] (" & larghezzaCampoEsperienza.ToString & ") COLLATE Latin1_General_CI_AS NULL " &
				",[Aree d'Intervento Protezione Civile] [varchar] (" & larghezzaCampoAreeIntervento.ToString & ") COLLATE Latin1_General_CI_AS NULL " &
				",[Ultimo Anno Ambiente] [varchar] (255) COLLATE Latin1_General_CI_AS NULL " &
				",[Esperienza Ultimo Anno Ambiente] [varchar] (" & larghezzaCampoEsperienza.ToString & ") COLLATE Latin1_General_CI_AS NULL " &
				",[Penultimo Anno Ambiente] [varchar] (255) COLLATE Latin1_General_CI_AS NULL " &
				",[Esperienza Penultimo Anno Ambiente] [varchar] (" & larghezzaCampoEsperienza.ToString & ") COLLATE Latin1_General_CI_AS NULL " &
				",[Terzultimo Anno Ambiente] [varchar] (255) COLLATE Latin1_General_CI_AS NULL " &
				",[Esperienza Terzultimo Anno Ambiente] [varchar] (" & larghezzaCampoEsperienza.ToString & ") COLLATE Latin1_General_CI_AS NULL " &
				",[Aree d'Intervento Ambiente] [varchar] (" & larghezzaCampoAreeIntervento.ToString & ") COLLATE Latin1_General_CI_AS NULL " &
				",[Ultimo Anno Patrimonio Artistico] [varchar] (255) COLLATE Latin1_General_CI_AS NULL " &
				",[Esperienza Ultimo Anno Patrimonio Artistico] [varchar] (" & larghezzaCampoEsperienza.ToString & ") COLLATE Latin1_General_CI_AS NULL " &
				",[Penultimo Anno Patrimonio Artistico] [varchar] (255) COLLATE Latin1_General_CI_AS NULL " &
				",[Esperienza Penultimo Anno Patrimonio Artistico] [varchar] (" & larghezzaCampoEsperienza.ToString & ") COLLATE Latin1_General_CI_AS NULL " &
				",[Terzultimo Anno Patrimonio Artistico] [varchar] (255) COLLATE Latin1_General_CI_AS NULL " &
				",[Esperienza Terzultimo Anno Patrimonio Artistico] [varchar] (" & larghezzaCampoEsperienza.ToString & ") COLLATE Latin1_General_CI_AS NULL " &
				",[Aree d'Intervento Patrimonio Artistico] [varchar] (" & larghezzaCampoAreeIntervento.ToString & ") COLLATE Latin1_General_CI_AS NULL " &
				",[Ultimo Anno Promozione Culturale] [varchar] (255) COLLATE Latin1_General_CI_AS NULL " &
				",[Esperienza Ultimo Anno Promozione Culturale] [varchar] (" & larghezzaCampoEsperienza.ToString & ") COLLATE Latin1_General_CI_AS NULL " &
				",[Penultimo Anno Promozione Culturale] [varchar] (255) COLLATE Latin1_General_CI_AS NULL " &
				",[Esperienza Penultimo Anno Promozione Culturale] [varchar] (" & larghezzaCampoEsperienza.ToString & ") COLLATE Latin1_General_CI_AS NULL " &
				",[Terzultimo Anno Promozione Culturale] [varchar] (255) COLLATE Latin1_General_CI_AS NULL " &
				",[Esperienza Terzultimo Anno Promozione Culturale] [varchar] (" & larghezzaCampoEsperienza.ToString & ") COLLATE Latin1_General_CI_AS NULL " &
				",[Aree d'Intervento Promozione Culturale] [varchar] (" & larghezzaCampoAreeIntervento.ToString & ") COLLATE Latin1_General_CI_AS NULL " &
				",[Ultimo Anno Estero] [varchar] (255) COLLATE Latin1_General_CI_AS NULL " &
				",[Esperienza Ultimo Anno Estero] [varchar] (" & larghezzaCampoEsperienza.ToString & ") COLLATE Latin1_General_CI_AS NULL " &
				",[Penultimo Anno Estero] [varchar] (255) COLLATE Latin1_General_CI_AS NULL " &
				",[Esperienza Penultimo Anno Estero] [varchar] (" & larghezzaCampoEsperienza.ToString & ") COLLATE Latin1_General_CI_AS NULL " &
				",[Terzultimo Anno Estero] [varchar] (255) COLLATE Latin1_General_CI_AS NULL " &
				",[Esperienza Terzultimo Anno Estero] [varchar] (" & larghezzaCampoEsperienza.ToString & ") COLLATE Latin1_General_CI_AS NULL " &
				",[Aree d'Intervento Estero] [varchar] (" & larghezzaCampoAreeIntervento.ToString & ") COLLATE Latin1_General_CI_AS NULL " &
				",[Ultimo Anno Agricoltura] [varchar] (255) COLLATE Latin1_General_CI_AS NULL " &
				",[Esperienza Ultimo Anno Agricoltura] [varchar] (" & larghezzaCampoEsperienza.ToString & ") COLLATE Latin1_General_CI_AS NULL " &
				",[Penultimo Anno Agricoltura] [varchar] (255) COLLATE Latin1_General_CI_AS NULL " &
				",[Esperienza Penultimo Anno Agricoltura] [varchar] (" & larghezzaCampoEsperienza.ToString & ") COLLATE Latin1_General_CI_AS NULL " &
				",[Terzultimo Anno Agricoltura] [varchar] (255) COLLATE Latin1_General_CI_AS NULL " &
				",[Esperienza Terzultimo Anno Agricoltura] [varchar] (" & larghezzaCampoEsperienza.ToString & ") COLLATE Latin1_General_CI_AS NULL " &
				",[Aree d'Intervento Agricoltura] [varchar] (" & larghezzaCampoAreeIntervento.ToString & ") COLLATE Latin1_General_CI_AS NULL " &
				  ") ON [PRIMARY]"


		End Select

		cmdCreateTempTable = New SqlClient.SqlCommand
		cmdCreateTempTable.CommandText = strSql
		cmdCreateTempTable.Connection = Session("conn")
		cmdCreateTempTable.ExecuteNonQuery()
		cmdCreateTempTable.Dispose()
	End Sub

	'ok
	'Funzione di controllo per l'import delle "RISORSE"
	Private Sub LeggiCSVriso()
		'--- lettura del file
		Dim Reader As StreamReader
		Dim xLinea As String
		Dim ArrCampi() As String
		Dim swErr As Boolean
		Dim AppoNote As String

		Dim AlboEnte As String
		AlboEnte = ClsUtility.TrovaAlboEnte(Session("IdEnte"), Session("conn"))


		'--- Leggo il file di input e scrivo quello di output
		Reader = New StreamReader(Server.MapPath("upload") & "\" & NomeUnivoco & ".CSV", System.Text.Encoding.Default, False)
		Writer = New StreamWriter(Server.MapPath("download") & "\" & NomeUnivoco & "_1" & ".CSV", False, System.Text.Encoding.Default)

		'--- intestazione

		xLinea = Reader.ReadLine()
		Writer.WriteLine("Note;" & xLinea)

		'Rilettura del file CSV con gli Errori Per definire gli errori di incongruenza con i ruoli
		Dim MyControllo As New ControlloRuoli

		'--- scorro le righe
		xLinea = Reader.ReadLine()
		While (xLinea <> "")
			swErr = False
			Tot = Tot + 1
			ArrCampi = CreaArray(xLinea)

			If UBound(ArrCampi) < 8 Then
				'--- se i campi non sono tutti errore
				strNote = "Il numero delle colonne inserite è minore di quello richieste."
				swErr = True
				TotKo = TotKo + 1
			Else
				If UBound(ArrCampi) > 8 Then
					'--- se i campi sono troppi errore
					strNote = "Il numero delle colonne inserite è maggiore di quello richieste."
					swErr = True
					TotKo = TotKo + 1
				Else
					'Ruolo    
					If Trim(ArrCampi(0)) = vbNullString Then
						strNote = strNote & "Il campo Ruolo e' un campo obbligatorio."
						ArrCampi(8) = "0" 'Imposto a zero il ruolo
						swErr = True
					Else
						If Len(ArrCampi(0)) > 10 Then
							strNote = strNote & "Il campo Ruolo puo' contenere massimo 10 caratteri."
							ArrCampi(8) = "0" 'Imposto a zero il ruolo
							swErr = True
						Else
							If VerificaRuolo(ArrCampi) = False Then
								strNote = strNote & "Il Ruolo inserito non è presente o non può essere importato."
								swErr = True
							End If
						End If
					End If

					'CodiceFiscale
					If Trim(ArrCampi(1)) = vbNullString Then
						strNote = strNote & "Il campo CodiceFiscale e' un campo obbligatorio."
						swErr = True
					Else
						If UnivocitaCodiceFiscale(Trim(ArrCampi(1)), ArrCampi(0)) = True Then
							strNote = strNote & "Il ruolo associato alla risorsa e' gia' presente nella base dati o nel file."
							swErr = True
						Else
							If CongruenzaCodiceFiscale(Trim(ArrCampi(1)), Trim(ArrCampi(2)), Trim(ArrCampi(3)), Trim(ArrCampi(4)), "F", Trim(ArrCampi(5))) = False And CongruenzaCodiceFiscale(Trim(ArrCampi(1)), Trim(ArrCampi(2)), Trim(ArrCampi(3)), Trim(ArrCampi(4)), "M", Trim(ArrCampi(5))) = False Then

								strNote = strNote & "CodiceFiscale non congruente con i dati inseriti."
								swErr = True
							End If
						End If
					End If

					'cognome
					If Trim(ArrCampi(2)) = vbNullString Then
						strNote = strNote & "Il campo Nome e' un campo obbligatorio."
						swErr = True
					Else
						If Len(ArrCampi(2)) > 100 Then
							strNote = strNote & "Il campo Nome puo' contenere massimo 100 caratteri."
							swErr = True
						End If
					End If

					'Nome
					If Trim(ArrCampi(3)) = vbNullString Then
						strNote = strNote & "Il campo Nome e' un campo obbligatorio."
						swErr = True
					Else
						If Len(ArrCampi(3)) > 100 Then
							strNote = strNote & "Il campo Nome puo' contenere massimo 100 caratteri."
							swErr = True
						End If
					End If

					'DataNascita
					If Trim(ArrCampi(4)) = vbNullString Then
						strNote = strNote & "Il campo DataNascita e' un campo obbligatorio."
						swErr = True
					Else
						If IsDate(ArrCampi(4)) = False Then
							strNote = strNote & "Il campo DataNascita non e' nel formato corretto."
							swErr = True
						Else
							If Len(Trim(ArrCampi(4))) = 8 Then
								ArrCampi(4) = Trim(ArrCampi(4))
								ArrCampi(4) = Left(ArrCampi(4), 6) & "19" & Right(ArrCampi(4), 2)
							End If
						End If
					End If

					'ComuneNascita
					If Trim(ArrCampi(5)) = vbNullString Then
						strNote = strNote & "Il campo CodiceISTATComuneNascita e' un campo obbligatorio."
						swErr = True
					Else
						If VerificaComuneNascita(Trim(ArrCampi(5))) = False Then
							strNote = strNote & "Il CodiceISTATComuneNascita inserito non esiste."
							swErr = True
						End If
					End If

					'EsperienzaServizioCivile

					If (Trim(ArrCampi(0).ToString) = "FORM" Or Trim(ArrCampi(0).ToString) = "RFORM") Then

						If IsNumeric(Trim(ArrCampi(6))) = True Then
							If Trim(ArrCampi(6)) = vbNullString Then
								strNote = strNote & "Il campo EsperienzaServizioCivile e' un campo obbligatorio se il ruolo e' Formatore."
								swErr = True
							Else
								If Trim(ArrCampi(6)) < 1 Or Trim(ArrCampi(6)) > 2 Then
									strNote = strNote & "Il valore EsperienzaServizioCivile inserito e' errato."
									swErr = True
								End If
							End If
						Else
							strNote = strNote & "Il valore EsperienzaServizioCivile inserito e' errato."
							swErr = True
						End If
					Else

						If Trim(ArrCampi(6)) <> vbNullString Then
							strNote = strNote & "Il campo EsperienzaServizioCivile per il ruolo indicato non va specificato."
							swErr = True
						End If
						'If IsNumeric(Trim(ArrCampi(6))) = False Then
						'    strNote = strNote & "Il valore EsperienzaServizioCivile inserito e' errato."
						'    swErr = True
						'End If


					End If


					'CORSO

					If (Trim(ArrCampi(0).ToString) = "FORM" Or Trim(ArrCampi(0).ToString) = "RFORM") Then
						If IsNumeric(Trim(ArrCampi(6))) = True Then
							If Trim(ArrCampi(7)) = vbNullString Then
								strNote = strNote & "Il campo Corso di Formazione e' un campo obbligatorio se il ruolo e' Formatore."
								swErr = True
							Else
								If Trim(ArrCampi(7)) < 1 Or Trim(ArrCampi(7)) > 3 Then
									strNote = strNote & "Il valore Corso di Formazione inserito e' errato."
									swErr = True
								End If
							End If
						Else
							strNote = strNote & "Il valore Corso di Formazione inserito e' errato."
							swErr = True
						End If
					Else

						If Trim(ArrCampi(7)) <> vbNullString Then
							strNote = strNote & "Il campo Corso di Formazione per il ruolo indicato non va specificato."
							swErr = True
						End If
						'If IsNumeric(Trim(ArrCampi(6))) = False Then
						'    strNote = strNote & "Il valore Corso di Formazione inserito e' errato."
						'    swErr = True
						'End If

					End If

					If swErr = False Then
						ScriviTabTemp(ArrCampi, AlboEnte)
					Else
						TotKo = TotKo + 1
					End If
				End If






			End If

			'Aggiungo la riga letta alla classe dei ruoli
			'  MyControllo.Ruoli.Add(Trim(ArrCampi(1)), Trim(ArrCampi(6)), "")
			MyControllo.Ruoli.Add(Trim(ArrCampi(1)), Trim(ArrCampi(8)), "")
			Writer.WriteLine(strNote & ";" & xLinea)
			strNote = vbNullString
			xLinea = Reader.ReadLine()
		End While

		Reader.Close()
		Writer.Close()

		'Accorpamento delle risorse
		MyControllo.Ruoli.RimuoviErrati()
		MyControllo.Ruoli.AccorpaRisorse()




		'Per ogni reggruppamento Creato Controllo la congruenza dei ruoli
		Dim IntX As Integer
		Dim DtrPersonale As System.Data.SqlClient.SqlDataReader
		Dim MyCommand As System.Data.SqlClient.SqlCommand
		Dim RuoliUnivoci As New Dictionary(Of String, Boolean)
		RuoliUnivoci.Add("21", False)
		RuoliUnivoci.Add("22", False)
		RuoliUnivoci.Add("23", False)
		RuoliUnivoci.Add("24", False)
		RuoliUnivoci.Add("25", False)
		RuoliUnivoci.Add("26", False)

		For IntX = 0 To MyControllo.Ruoli.Count - 1
			Dim IntEntePersonale As Long
			'Controllo se l'Utente in questione è gia esistente o è nuovo
			DtrPersonale = ClsServer.CreaDatareader("Select IdEntePersonale From EntePersonale Where IdEnte = " & Session("IdEnte") & " And CodiceFiscale = '" & MyControllo.Ruoli(IntX).CodiceFiscale & "'", IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
			If DtrPersonale.HasRows = True Then
				DtrPersonale.Read()
				IntEntePersonale = DtrPersonale.Item("IdEntePersonale")
			Else
				IntEntePersonale = -1
			End If
			DtrPersonale.Close()

			'Eseguo la Store Procedure SP_Controlla_Ruoli
			MyCommand = New System.Data.SqlClient.SqlCommand
			MyCommand.Connection = IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn"))
			MyCommand.CommandType = CommandType.StoredProcedure
			MyCommand.CommandText = "SP_CONTROLLA_RUOLI"

			'Variabile - @IdEntePersonale - In
			Dim sparam As SqlClient.SqlParameter
			sparam = New SqlClient.SqlParameter
			sparam.ParameterName = "@IdEntePersonale"
			sparam.SqlDbType = SqlDbType.Int
			MyCommand.Parameters.Add(sparam)

			'Variabile - @NuoviRuoli - In
			Dim sparam1 As SqlClient.SqlParameter
			sparam1 = New SqlClient.SqlParameter
			sparam1.ParameterName = "@NuoviRuoli"
			sparam1.SqlDbType = SqlDbType.NVarChar
			sparam1.Size = 50
			MyCommand.Parameters.Add(sparam1)

			'Variabile - @VecchiRuoli - In
			Dim sparam2 As SqlClient.SqlParameter
			sparam2 = New SqlClient.SqlParameter
			sparam2.ParameterName = "@VecchiRuoli"
			sparam2.SqlDbType = SqlDbType.NVarChar
			sparam2.Size = 50
			MyCommand.Parameters.Add(sparam2)

			'Variabile - @TipoUtente - In
			Dim sparam3 As SqlClient.SqlParameter
			sparam3 = New SqlClient.SqlParameter
			sparam3.ParameterName = "@TipoUtente"
			sparam3.SqlDbType = SqlDbType.NVarChar
			sparam3.Size = 5
			MyCommand.Parameters.Add(sparam3)

			Dim paramCodiceFiscale As SqlClient.SqlParameter
			paramCodiceFiscale = New SqlClient.SqlParameter
			paramCodiceFiscale.ParameterName = "@CodiceFiscale"
			paramCodiceFiscale.Size = 20
			paramCodiceFiscale.SqlDbType = SqlDbType.NVarChar
			MyCommand.Parameters.Add(paramCodiceFiscale)

			Dim paramIdEnte As SqlClient.SqlParameter
			paramIdEnte = New SqlClient.SqlParameter
			paramIdEnte.ParameterName = "@IdEnte"
			paramIdEnte.Size = 20
			paramIdEnte.SqlDbType = SqlDbType.NVarChar
			MyCommand.Parameters.Add(paramIdEnte)

			'Variabile - @Messaggio - Out
			Dim sparam4 As SqlClient.SqlParameter
			sparam4 = New SqlClient.SqlParameter
			sparam4.ParameterName = "@Messaggio"
			sparam4.SqlDbType = SqlDbType.NVarChar
			sparam4.Size = 1000
			sparam4.Direction = ParameterDirection.Output
			MyCommand.Parameters.Add(sparam4)

			MyCommand.Parameters("@IdEntePersonale").Value = IntEntePersonale
			MyCommand.Parameters("@NuoviRuoli").Value = MyControllo.Ruoli(IntX).IdRuolo & ";"
			MyCommand.Parameters("@VecchiRuoli").Value = ""
			MyCommand.Parameters("@CodiceFiscale").Value = MyControllo.Ruoli(IntX).CodiceFiscale
			MyCommand.Parameters("@IdEnte").Value = Session("IdEnte")
			MyCommand.Parameters("@TipoUtente").Value = Session("TipoUtente")
			DtrPersonale = MyCommand.ExecuteReader()
			MyControllo.Ruoli(IntX).Note = MyCommand.Parameters("@Messaggio").Value
			For Each ruolo In MyControllo.Ruoli(IntX).IdRuolo.Split(";")
				If RuoliUnivoci.ContainsKey(ruolo) Then
					If RuoliUnivoci(ruolo) Then
						MyControllo.Ruoli(IntX).Note = "Ruolo già presente per altra risorsa."
					Else
						RuoliUnivoci(ruolo) = True
					End If
				End If
			Next
			DtrPersonale.Close()
			Next


			'Inserisco gli errori sul file di log
			'Apro il file creato precedentemente
			Reader = New StreamReader(Server.MapPath("download") & "\" & NomeUnivoco & "_1" & ".CSV", System.Text.Encoding.Default, False)
		Writer = New StreamWriter(Server.MapPath("download") & "\" & NomeUnivoco & ".CSV", False, System.Text.Encoding.Default)

		'Reinserisco l'intestazione
		xLinea = Reader.ReadLine()
		Writer.WriteLine(xLinea)

		'Ciclo gli elementi 
		xLinea = Reader.ReadLine()
		Dim BlnTrovato As Boolean
		Do While xLinea <> ""
			BlnTrovato = False
			For IntX = 0 To MyControllo.Ruoli.Count - 1
				If PrelevaCodiceFiscale(xLinea).ToUpper = MyControllo.Ruoli(IntX).CodiceFiscale.ToUpper Then
					If IsNumeric(MyControllo.Ruoli(IntX).Note) = False Then 'Le note contengono il messaggio di errore
						If PrelevaNote(xLinea).Trim = "" Then
							TotKo = TotKo + 1
							TotOk = TotOk - 1
						End If
						Writer.WriteLine(PrelevaNote(xLinea) & MyControllo.Ruoli(IntX).Note & ";" & PrelevaContenuti(xLinea))
						BlnTrovato = True
						Exit For
					End If
				End If
			Next
			If BlnTrovato = False Then
				Writer.WriteLine(xLinea)
			End If
			xLinea = Reader.ReadLine()
		Loop
		Reader.Close()
		Writer.Close()

		'Elimino il file log di appoggio
		Dim MyFile As System.IO.File
		MyFile.Delete(Server.MapPath("download") & "\" & NomeUnivoco & "_1" & ".CSV")

		Response.Redirect("WfrmRisultatoImportRisorseSediEnti.aspx?NomeFile=" & NomeUnivoco & "&Tot=" & Tot & "&TotOk=" & TotOk & "&TotKo=" & TotKo)
		''--- reindirizzo la pagina sottostante
		'Response.Write("<script>" & vbCrLf)
		'Response.Write("parent.Naviga('WfrmRisultatoImportRisorseSediEnti.aspx?NomeFile=" & NomeUnivoco & "&Tot=" & Tot & "&TotOk=" & TotOk & "&TotKo=" & TotKo & "')" & vbCrLf)
		'Response.Write("</script>")
	End Sub

	Private Function PrelevaNote(ByVal Linea As String) As String
		PrelevaNote = Linea.Substring(0, Linea.IndexOf(";"))
	End Function

	Private Function PrelevaCodiceFiscale(ByVal Linea As String) As String
		'Trovo la posizione del secondo ";"
		Dim IntPuntoVirgola As Integer
		IntPuntoVirgola = Linea.IndexOf(";", Linea.IndexOf(";") + 1)
		IntPuntoVirgola = IntPuntoVirgola + 1
		PrelevaCodiceFiscale = Linea.Substring(IntPuntoVirgola, Linea.IndexOf(";", IntPuntoVirgola) - IntPuntoVirgola)
	End Function

	Private Function PrelevaContenuti(ByVal Linea As String) As String
		PrelevaContenuti = Linea.Substring(Linea.IndexOf(";") + 1)
	End Function

	Function CheckEsistenzaTitoloGiuridico(ByVal DescrizioneAbbreviata As String, ByVal AlboEnte As String) As Boolean
		Dim dtrLocal As SqlClient.SqlDataReader
		Dim strSql As String


		strSql = "Select DescrizioneAbbreviata from TitoliGiuridici "
		strSql &= "where DescrizioneAbbreviata='" & Replace(DescrizioneAbbreviata, "'", "''") & "'"

		If AlboEnte = "" Then
			strSql &= " AND (ALBO='SCU'OR ALBO IS NULL) " '
		Else
			strSql &= " AND (ALBO='" & AlboEnte & "'OR ALBO IS NULL) " '
		End If
		dtrLocal = ClsServer.CreaDatareader(strSql, Session("conn"))

		CheckEsistenzaTitoloGiuridico = dtrLocal.HasRows

		dtrLocal.Close()
		dtrLocal = Nothing

	End Function
	Private Function TitoloGiuridico_ObbligoScandeza(ByVal DescrizioneAbbreviata As String, ByVal AlboEnte As String) As Boolean
		Dim dtrLocal As SqlClient.SqlDataReader
		Dim strSql As String

		Dim blnObbligoScadenza As Boolean


		strSql = "Select obbligoScadenza FROM TitoliGiuridici where  DescrizioneAbbreviata='" & Replace(DescrizioneAbbreviata, "'", "''") & "'"
		If AlboEnte = "" Then
			strSql &= " AND (ALBO='SCU' OR ALBO IS NULL) "
		Else
			strSql &= " AND (ALBO='" & AlboEnte & "' OR ALBO IS NULL) " '
		End If
		dtrLocal = ClsServer.CreaDatareader(strSql, Session("conn"))

		dtrLocal.Read()

		blnObbligoScadenza = dtrLocal("obbligoScadenza")
		dtrLocal.Close()
		dtrLocal = Nothing
		Return blnObbligoScadenza
	End Function
    Private Function getRegioneCompetenzaEnte() As Integer
        Dim strSql = "select t2.idRegioneCompetenza from enti t1 inner join sezionialboscu t2 on t2.idSezione = t1.idSezione where t1.idEnte=" & Session("IdEnte")
        Dim dtrLocal As SqlClient.SqlDataReader = ClsServer.CreaDatareader(strSql, Session("conn"))
        getRegioneCompetenzaEnte = 0
        If dtrLocal.HasRows = True Then
            dtrLocal.Read()
            getRegioneCompetenzaEnte = dtrLocal("idRegioneCompetenza")
        End If
        If Not dtrLocal Is Nothing Then
            dtrLocal.Close()
            dtrLocal = Nothing
        End If
    End Function
    Private Function getRegioneCompetenzaComune(ByVal codiceIstat As String) As Integer
        codiceIstat = Replace(codiceIstat, "'", "")
        Dim strSql = "select coalesce(idRegioneCompetenza, -1) idRegioneCompetenza from comuni t1 inner join provincie t2 on t2.idprovincia=t1.idprovincia where t1.codiceIstat='" & codiceIstat & "'"

        Dim dtrLocal As SqlClient.SqlDataReader = ClsServer.CreaDatareader(strSql, Session("conn"))
        getRegioneCompetenzaComune = 0
        If dtrLocal.HasRows = True Then
            dtrLocal.Read()
            getRegioneCompetenzaComune = dtrLocal("idRegioneCompetenza")
        End If
        If Not dtrLocal Is Nothing Then
            dtrLocal.Close()
            dtrLocal = Nothing
        End If
    End Function
    'ok
    'Funzione di controllo per l'import delle "SEDI"
	Private Sub LeggiCSVsedi()
		'--- lettura del file
		Dim Reader As StreamReader
		Dim xLinea As String
		Dim ArrCampi() As String
		Dim swErr As Boolean
		Dim AppoNome As String
		Dim AppoIndirizzo
		Dim b As Boolean
		Dim gm As New GoogleMaps(ConfigurationSettings.AppSettings("GoogleKey"))
		Dim Stato, denComune As String
		'variabile contatore per cercare riga eventuale
        Dim intRighe As Integer = 1
        Dim erroreIndirizzo = False 'se vero occorre cercare su googlemap
        Dim regioneCompetenza As Integer = getRegioneCompetenzaEnte()

		Dim AlboEnte As String
		AlboEnte = ClsUtility.TrovaAlboEnte(Session("IdEnte"), Session("conn"))




		Dim VAR_CODICEFISCALE As String = ""                       '0   CodiceFiscale (partita IVA)

		Dim VAR_DENOMINAZIONE As String = ""                       '1   'Denominazione "

		Dim VAR_ISTATCOMUNE As String = ""                         '2   'Istat Comune

		Dim VAR_CITTAESTERA As String = ""                         '3   'Città Estera

		Dim VAR_INDIRIZZO As String = ""                           '4   'Indirizzo"

		Dim VAR_CIVICO As String = ""                              '5   'Civico

		Dim VAR_CAP As String = ""                                 '6   'Cap

		Dim VAR_PREFISSO As String = ""                            '7   'Prefisso

		Dim VAR_TELEFONO As String = ""                            '8   'Telefono

		Dim VAR_PALAZZINA As String = ""                           '9   'Palazzina

		Dim VAR_SCALA As String = ""                              '10  'Scala

		Dim VAR_PIANO As String = ""                              '11  'Piano

		Dim VAR_INTERNO As String = ""                          '12  'Interno

		Dim VAR_TITOLOGIURIDICO As String = ""                    '13  'Titolo Giuridico

		Dim VAR_VOLONTARIALLOCABILI As String = ""                '14  'Volontari Allocabibli

		Dim VAR_VOLMAGGIORI20 As String = ""                      '15  'VOLONTARI MAGGIORE DI 20

		Dim VAR_NORMATIVA_81 As String = ""                       '16 NORMATIVA_81

		Dim VAR_CONFORMITA As String = ""                          '17 CONFORMITA'

		Dim VAR_SOGGETTO_ESTERO As String = ""                     '18 SOGGETTO ESTERO

		Dim VAR_DICHIARAZIONE_SOGGETTO_ESTERO As String = ""       '19 DICHIARAZIONE SOGGETTO ESTERO

		Dim VAR_FLG_ALERT_NOME_SEDE As String = ""                 '20 FLAG SE NOME SEDE ANOMALO

		Dim VAR_FLG_ALERT_INDIRIZZO_GOOGLE As String = ""          '21 FLAG NON CORRISPONDENZA API GOOGLE

		Dim VAR_FLG_ALERT_INDIRIZZO = ""                           '22 INDIRIZZO NON CORRETTO??? DA VERIFICARE




		Dim NomeSedi As New List(Of String) ' Dictionary(Of String, String)
		Dim IndirizziSedi As New List(Of String)   ''''Dictionary(Of String, String)

		Dim sqlSediNome = "SELECT A.IDENTESEDE, dbo.FN_Normalizza_Nome_Sede(b.Denominazione) as NomeSede" &
					" FROM entisediattuazioni a INNER JOIN entisedi b on a.identesede = b.identesede" &
					" WHERE a.IDEntecapofila = @IdEnte AND a.idstatoEntesede  in(1,4)"


		Dim cmdNomiSedi As New SqlCommand(sqlSediNome, Session("conn"))
		cmdNomiSedi.Parameters.AddWithValue("@IdEnte", Session("IdEnte"))
		Dim readerNomiSede = cmdNomiSedi.ExecuteReader()


		Try
			While readerNomiSede.Read
				NomeSedi.Add(readerNomiSede("NomeSede").ToString.ToUpper)
			End While
		Catch ex As Exception
			Log.Error(LogEvent.IMPORTAZIONE_SEDI_ERRORE_LETTURA_DB, "Caricamento dati", ex)
			txtErroreCV.Text = "Errore nel recupero delle informazioni."
			Exit Sub
		Finally
			readerNomiSede.Close()
		End Try

		Dim sqlSediIndirizzi = "SELECT e.IDEnteSede, REPLACE(coalesce(e.Indirizzo,'')+coalesce(e.Civico,'')+coalesce(e.CAP, '')+coalesce(c.codiceistat, '')+coalesce(e.Palazzina, ''), ' ', '') AS Indirizzo FROM entisedi e inner join comuni c on e.idcomune =c.IDComune"


		Dim cmdIndirizziSedi As New SqlCommand(sqlSediIndirizzi, Session("conn"))
		Dim readerIndirizziSede = cmdIndirizziSedi.ExecuteReader()


		Try
			While readerIndirizziSede.Read
				IndirizziSedi.Add(readerIndirizziSede("Indirizzo").ToString.ToUpper)
			End While
		Catch ex As Exception
			Log.Error(LogEvent.IMPORTAZIONE_SEDI_ERRORE_LETTURA_DB, "Caricamento dati", ex)
			txtErroreCV.Text = "Errore nel recupero delle informazioni."
			Exit Sub
		Finally
			readerIndirizziSede.Close()
		End Try

		'--- Leggo il file di input e scrivo quello di output
		Reader = New StreamReader(Server.MapPath("upload") & "\" & NomeUnivoco & ".CSV", System.Text.Encoding.Default, False)
		Writer = New StreamWriter(Server.MapPath("download") & "\" & NomeUnivoco & ".CSV", False, System.Text.Encoding.Default)

		'--- intestazione

		xLinea = Reader.ReadLine()
		Writer.WriteLine("Note;" & xLinea)

		'creo un array che metto in sessione per concatenarlo alle note in fase di segnalazione errori
		Dim vSegnalazioneIndirizzo() As String
		ReDim vSegnalazioneIndirizzo(0)

		'--- scorro le righe
		xLinea = Reader.ReadLine()






		Try

			Dim intContaACFC As Integer = 0
			Dim intContaPartner As Integer = 0
			Dim chkNSedi As Boolean = True

            While (xLinea <> "" And Trim(Replace(xLinea, ";", "")) <> "")
                intRighe = intRighe + 1
                swErr = False
                erroreIndirizzo = False
                Tot = Tot + 1
                ArrCampi = CreaArray(xLinea)

                If UBound(ArrCampi) < 19 Then
                    '--- se i campi non sono tutti errore
                    strNote = "Il numero delle colonne inserite è minore di quello richieste."
                    swErr = True
                    TotKo = TotKo + 1
                Else
                    If UBound(ArrCampi) > 19 Then
                        '--- se i campi sono troppi errore
                        strNote = "Il numero delle colonne inserite è maggiore di quello richieste."
                        swErr = True
                        TotKo = TotKo + 1
                    Else


                        '------------------------------------------------------------------------
                        Try
                            VAR_CODICEFISCALE = Trim(ArrCampi(0))
                        Catch ex As Exception
                            VAR_CODICEFISCALE = ""
                        End Try

                        Try
                            VAR_DENOMINAZIONE = Trim(ArrCampi(1))
                        Catch ex As Exception
                            VAR_DENOMINAZIONE = ""
                        End Try

                        Try
                            VAR_ISTATCOMUNE = Trim(ArrCampi(2))
                        Catch ex As Exception
                            VAR_ISTATCOMUNE = ""
                        End Try

                        Try
                            VAR_CITTAESTERA = Trim(ArrCampi(3))
                        Catch ex As Exception
                            VAR_CITTAESTERA = ""
                        End Try

                        Try
                            VAR_INDIRIZZO = Trim(ArrCampi(4))
                        Catch ex As Exception
                            VAR_INDIRIZZO = ""
                        End Try

                        Try
                            VAR_CIVICO = Trim(ArrCampi(5))
                        Catch ex As Exception
                            VAR_CIVICO = ""
                        End Try

                        Try
                            VAR_CAP = Trim(ArrCampi(6))
                        Catch ex As Exception
                            VAR_CAP = ""
                        End Try

                        Try
                            VAR_PREFISSO = Trim(ArrCampi(7))
                        Catch ex As Exception
                            VAR_PREFISSO = ""
                        End Try


                        Try
                            VAR_TELEFONO = Trim(ArrCampi(8))
                        Catch ex As Exception
                            VAR_TELEFONO = ""
                        End Try


                        Try
                            VAR_PALAZZINA = Trim(ArrCampi(9))
                        Catch ex As Exception
                            VAR_PALAZZINA = ""
                        End Try

                        'Try
                        '	VAR_SCALA = Trim(ArrCampi(10))
                        'Catch ex As Exception
                        '	VAR_SCALA = ""
                        'End Try
                        'If VAR_SCALA = "" Then VAR_SCALA = "ND"
                        VAR_SCALA = "ND"
                        'Try
                        '	VAR_PIANO = Trim(ArrCampi(11))
                        'Catch ex As Exception
                        '	VAR_PIANO = ""
                        'End Try
                        'If VAR_PIANO = "" Then VAR_PIANO = "0"
                        VAR_PIANO = "0"
                        'Try
                        '	VAR_INTERNO = Trim(ArrCampi(12))
                        'Catch ex As Exception
                        '	VAR_INTERNO = ""
                        'End Try
                        'If VAR_INTERNO = "" Then VAR_INTERNO = "ND"
                        VAR_INTERNO = "ND"
                        Try
                            VAR_TITOLOGIURIDICO = Trim(ArrCampi(10))
                        Catch ex As Exception
                            VAR_TITOLOGIURIDICO = ""
                        End Try

                        Try
                            VAR_VOLONTARIALLOCABILI = Trim(ArrCampi(11))
                        Catch ex As Exception
                            VAR_VOLONTARIALLOCABILI = ""
                        End Try

                        Try
                            VAR_VOLMAGGIORI20 = Trim(ArrCampi(12))
                        Catch ex As Exception
                            VAR_VOLMAGGIORI20 = ""
                        End Try

                        '------------------------------------------------------------------------

                        Try
                            VAR_NORMATIVA_81 = Trim(ArrCampi(13)).ToUpper
                        Catch ex As Exception
                            VAR_NORMATIVA_81 = ""
                        End Try

                        Try
                            VAR_CONFORMITA = Trim(ArrCampi(14)).ToUpper
                        Catch ex As Exception
                            VAR_CONFORMITA = ""
                        End Try

                        Try
                            VAR_SOGGETTO_ESTERO = Trim(ArrCampi(15))
                        Catch ex As Exception
                            VAR_SOGGETTO_ESTERO = ""
                        End Try

                        Try
                            VAR_DICHIARAZIONE_SOGGETTO_ESTERO = Trim(ArrCampi(16)).ToUpper
                        Catch ex As Exception
                            VAR_DICHIARAZIONE_SOGGETTO_ESTERO = ""
                        End Try


                        'CodiceFiscale (partita IVA)
                        If VAR_CODICEFISCALE = vbNullString Then
                            strNote = strNote & "Il campo CodiceFiscale e' un campo obbligatorio. "
                            swErr = True
                        Else
                            If EsistenzaCodiceFiscaleEnti(VAR_CODICEFISCALE) = False Then
                                strNote = strNote & "Il Codice Fiscale dell'ente non è presente nella base dati. "
                                swErr = True
                            Else
                                If CongruenzaCodiceFiscaleEnti(VAR_CODICEFISCALE) = False Then
                                    strNote = strNote & "Il Codice Fiscale non risulta associato all'ente. "
                                    swErr = True
                                End If
                            End If
                        End If

                        'Denominazione    
                        If VAR_DENOMINAZIONE = vbNullString Then
                            strNote = strNote & "La denominazione e' un campo obbligatorio. "
                            swErr = True
                        Else
                            If Len(VAR_DENOMINAZIONE) > 200 Then
                                strNote = strNote & "La denominazione puo' contenere massimo 200 caratteri. "
                                swErr = True
                            End If
                        End If

                        'Istat Comune
                        If VAR_ISTATCOMUNE = vbNullString Then
                            strNote = strNote & "Il campo CodiceISTATComune e' un campo obbligatorio. "
                            swErr = True
                        Else
                            If VerificaComune(VAR_ISTATCOMUNE) = False Then
                                strNote = strNote & "Il CodiceISTATComune inserito non esiste. "
                                swErr = True
                            Else

 
                                If VerificaNazioneComune(VAR_ISTATCOMUNE) = False Then
                                    'CittaEstera
                                    If VAR_CITTAESTERA = vbNullString Then
                                        strNote = strNote & "Il campo CittaEstera e' un campo obbligatorio. "
                                        swErr = True
                                    End If
                                Else
                                    'Citta italiana
                                    If VAR_CITTAESTERA <> vbNullString Then
                                        strNote = strNote & "Il campo CittaEstera non deve essere indicato. "
                                        swErr = True
                                    End If
                                    'se la sezione di competenza non e' nazionale, la regione di competenza deve coincidere con la regione del comune
                                    If regioneCompetenza <> 22 AndAlso regioneCompetenza <> getRegioneCompetenzaComune(VAR_ISTATCOMUNE) Then
                                        strNote = strNote & "Il comune non appartiene alla regione di competenza. "
                                        swErr = True
                                    End If
                                End If
                            End If

                        End If

                        Dim indirizzoOkDb As Boolean = False 'l'indirizzo e' ok sul database interno? se si, evitare il controllo google

                        'Indirizzo
                        If VAR_INDIRIZZO = vbNullString Then
                            strNote = strNote & "L'indirizzo e' un campo obbligatorio. "
                            swErr = True
                        Else
                            'CONTROLLO ESISTENZA INDIRIZZO
                            'CONTROLLO AGGIUNTO DA THE GREAT SOUTHERN JONKILL
                            '27 marzo 2009
                            'vado a prendere le informazioni del comune che si sta tentando di inserire
                            Dim informazioniComune As New clsInformazioniComune(Session("conn"), , VAR_ISTATCOMUNE, )
                            Dim informazioniComunePerID As New clsInformazioniComune(Session("conn"), , , informazioniComune.getIdComune_byCodiceIstat)
                            'controllo se l'indirizzo è valido, quindi ho un idcomune con indirizzi
                            If informazioniComune.getIdComune_byCodiceIstat > 0 Then
                                'dichiaro la classe contenente le informazioni dell'indirizzo
                                Dim chkIndirizzo As New clsIndirizzi(VAR_INDIRIZZO, informazioniComune.getIdComune_byCodiceIstat, Session("conn"))
                                'se il comune ha gli indirizzi vado a controllare se l'indirizzo esiste
                                If informazioniComunePerID.checkEsistenzaIndirizzi = True Then
                                    'se l'indirizzo che si sta inserendo non esiste mando un messaggio all'utente
                                    If chkIndirizzo.CheckIndirizzo = False Then
                                        erroreIndirizzo = True
                                        If False Then
                                            If vSegnalazioneIndirizzo(0) = "" Then
                                                vSegnalazioneIndirizzo(0) = "L'indirizzo non &egrave; valido: <a style=" & """" & "cursor:pointer" & """" & " onclick=" & """" & "ApriSuggerimento('" & informazioniComune.getIdComune_byCodiceIstat & "','" & Replace(VAR_INDIRIZZO, "'", "|") & "')" & """" & "><font color=" & """" & "blue" & """" & "><b>SUGGERIMENTO</b></font></a>. "
                                            Else
                                                ReDim Preserve vSegnalazioneIndirizzo(UBound(vSegnalazioneIndirizzo) + 1)
                                                vSegnalazioneIndirizzo(UBound(vSegnalazioneIndirizzo)) = "L'indirizzo non &egrave; valido: <a style=" & """" & "cursor:pointer" & """" & " onclick=" & """" & "ApriSuggerimento('" & informazioniComune.getIdComune_byCodiceIstat & "','" & Replace(VAR_INDIRIZZO, "'", "|") & "')" & """" & "><font color=" & """" & "blue" & """" & "><b>SUGGERIMENTO</b></font></a>. "
                                            End If
                                            strNote = strNote & "L'indirizzo non e' valido. "
                                            swErr = True
                                        End If
                                    Else
                                        indirizzoOkDb = True 'indirizzo gia' controllato sul database, non serve controllarlo su googlemaps
                                    End If
                                End If
                            End If
                        End If


                        'CheckEsistenzaCAP
                        If VAR_CAP = vbNullString Then
                            If VerificaNazioneComune(VAR_ISTATCOMUNE) = True Then
                                strNote = strNote & "Il campo CAP e' un campo obbligatorio. "
                                swErr = True
                            End If
                        Else
                            'se il comune è nazionale devo controllare il se c'è congruenza fra codice istat e cap
                            If VerificaNazioneComune(VAR_ISTATCOMUNE) = True Then
                                'If CheckEsistenzaCAP(Trim(ArrCampi(3)), Trim(ArrCampi(6))) = False Then
                                '    strNote = strNote & "Il CAP non è congruo con i dati inseriti."
                                '    swErr = True
                                'End If
                                Dim strCausale As String
                                Dim blnLocal As Boolean

                                'PIANO
                                If Not erroreIndirizzo Then

                                    If VAR_PIANO <> vbNullString Then
                                        If IsNumeric(VAR_PIANO) = True Then
                                            If ClsUtility.CAP_VERIFICA(Session("conn"), strCausale, blnLocal, VAR_CAP, "0", "", VAR_ISTATCOMUNE, VAR_INDIRIZZO, VAR_CIVICO) = False Then
                                                strNote = strNote & strCausale & ". "
                                                swErr = True
                                            End If
                                        End If
                                    Else
                                        If ClsUtility.CAP_VERIFICA(Session("conn"), strCausale, blnLocal, VAR_CAP, "0", "", VAR_ISTATCOMUNE, VAR_INDIRIZZO, VAR_CIVICO) = False Then
                                            strNote = strNote & strCausale & ". "
                                            swErr = True
                                        End If
                                    End If
                                End If
                            End If
                        End If


                        'Prefisso
                        If VAR_PREFISSO = vbNullString Then
                            strNote = strNote & "Il campo prefisso e' un campo obbligatorio. "
                            swErr = True
                        Else
                            If IsNumeric(VAR_PREFISSO) = False Then
                                strNote = strNote & "Il campo prefisso deve essere un numero. "
                                swErr = True
                            End If
                        End If

                        'Telefono
                        If VAR_TELEFONO = vbNullString Then
                            strNote = strNote & "Il campo Prefisso e' un campo obbligatorio. "
                            swErr = True
                        Else
                            If IsNumeric(VAR_TELEFONO) = False Then
                                strNote = strNote & "Il campo prefisso deve essere un numero. "
                                swErr = True
                            End If
                        End If

                        'Univocità righe
                        'If CheckUnviRiga(zeriCF(Trim(ArrCampi(0))), Trim(ArrCampi(1)), Trim(ArrCampi(2)), Trim(ArrCampi(3)), Trim(ArrCampi(4)), Trim(ArrCampi(5))) = False Then
                        '    strNote = strNote & "Le Sede inserita è già esistente."
                        '    swErr = True
                        'End If
                        If VAR_PIANO <> vbNullString Then
                            If IsNumeric(VAR_PIANO) = True Then
                                If CheckDoppiIndirizzi(zeriCF(VAR_CODICEFISCALE), VAR_ISTATCOMUNE, VAR_INDIRIZZO, VAR_CIVICO, VAR_PALAZZINA, VAR_SCALA, VAR_PIANO, VAR_INTERNO) = False Then
                                    strNote = strNote & "L'indirizzo indicato è già utilizzato per un'altra sede. "
                                    swErr = True
                                End If
                            End If
                        Else
                            If CheckDoppiIndirizzi(zeriCF(VAR_CODICEFISCALE), VAR_ISTATCOMUNE, VAR_INDIRIZZO, VAR_CIVICO, VAR_PALAZZINA, VAR_SCALA, VAR_PIANO, VAR_INTERNO) = False Then
                                strNote = strNote & "L'indirizzo indicato è già utilizzato per un'altra sede. "
                                swErr = True
                            End If
                        End If
                        If CheckDoppiaDenominazione(zeriCF(VAR_CODICEFISCALE), VAR_DENOMINAZIONE, AlboEnte) = False Then
                            strNote = strNote & "La denominazione della sede è già utilizzata. "
                            swErr = True
                        End If

                        'PALAZZINA
                        If VAR_PALAZZINA <> vbNullString Then
                            If Len(VAR_PALAZZINA) > 5 Then
                                strNote = strNote & "Il campo Palazzina puo' contenere massimo 5 caratteri. "
                                swErr = True
                            End If
                        End If
                        If AlboEnte = "SCU" Then
                            If VAR_PALAZZINA = vbNullString Then
                                strNote = strNote & "Il campo Palazzina e' un campo obbligatorio. Indicare ND nel caso non sia previsto. "
                                swErr = True
                            End If
                        End If
                        'SCALA
                        If VAR_SCALA <> vbNullString Then
                            If Len(VAR_SCALA) > 5 Then
                                strNote = strNote & "Il campo Scala puo' contenere massimo 5 caratteri. "
                                swErr = True
                            End If
                        End If
                        If AlboEnte = "SCU" Then
                            If VAR_SCALA = vbNullString Then
                                strNote = strNote & "Il campo Scala e' un campo obbligatorio. Indicare ND nel caso non sia previsto. "
                                swErr = True
                            End If
                        End If

                        'PIANO
                        If VAR_PIANO <> vbNullString Then
                            If IsNumeric(VAR_PIANO) = False Then
                                strNote = strNote & "Il campo Piano puo' contenere solo numeri. "
                                swErr = True
                            End If
                        End If
                        If AlboEnte = "SCU" Then
                            If VAR_PIANO = vbNullString Then
                                strNote = strNote & "Il campo Piano e' un campo obbligatorio. Indicare 0 nel caso non sia previsto. "
                                swErr = True
                            End If
                        End If
                        'INTERNO
                        If VAR_INTERNO <> vbNullString Then
                            If Len(VAR_INTERNO) > 5 Then
                                strNote = strNote & "Il campo Interno puo' contenere massimo 5 caratteri. "
                                swErr = True
                            End If
                        End If

                        If AlboEnte = "SCU" Then
                            If VAR_INTERNO = vbNullString Then
                                strNote = strNote & "Il campo Interno e' un campo obbligatorio. Indicare ND nel caso non sia previsto. "
                                swErr = True
                            End If
                        End If
                        'TITOLO GIURIDICO
                        If VAR_TITOLOGIURIDICO = vbNullString Then
                            strNote = strNote & "Il campo Titolo di Diponibilità e' un campo obbligatorio. "
                            swErr = True
                        Else
                            If Len(VAR_TITOLOGIURIDICO) > 5 Then
                                strNote = strNote & "Il campo Titolo di Disponibilità puo' contenere massimo 5 caratteri. "
                                swErr = True
                            End If
                            'CONTROLLO ESISTENZA CODICE ABBREVIATO TITOLO GIURIDICO
                            If CheckEsistenzaTitoloGiuridico(VAR_TITOLOGIURIDICO, AlboEnte) = False Then
                                strNote = strNote & "Verificare il codice del Titolo di Disponibilità. "
                                swErr = True
                                'Else

                                '    If TitoloGiuridico_ObbligoScandeza(Trim(ArrCampi(12)), AlboEnte) = True Then
                                '        'DATA STIPULA CONTRATTO
                                '        If Trim(ArrCampi(15)) = vbNullString Then
                                '            strNote = strNote & "Il campo Data Stipula Contratto e' un campo obbligatorio."
                                '            swErr = True
                                '        Else
                                '            'controllo se è un formato data
                                '            If IsDate(Trim(ArrCampi(15))) = False Then
                                '                strNote = strNote & "La Data Stipula Contratto non e' nel formato corretto."
                                '                swErr = True
                                '            End If

                                '        End If
                                '        'DATA SCADENZA CONTRATTO
                                '        If Trim(ArrCampi(16)) = vbNullString Then
                                '            strNote = strNote & "Il campo  Data Scadenza Contratto e' un campo obbligatorio."
                                '            swErr = True
                                '        Else
                                '            'controllo se è un formato data
                                '            If IsDate(Trim(ArrCampi(16))) = False Then
                                '                strNote = strNote & "La Data Scadenza Contratto non e' nel formato corretto."
                                '                swErr = True
                                '            End If
                                '        End If
                                '        'confronto tra date 
                                '        If (Trim(ArrCampi(16)) <> String.Empty) And (Trim(ArrCampi(15)) <> String.Empty) Then
                                '            If CDate(ArrCampi(15)) > CDate(ArrCampi(16)) Then
                                '                strNote = strNote & "La Data Scadenza Contratto deve essere successiva alla Data Stipula."
                                '                swErr = True
                                '            End If
                                '        End If
                                '    Else 'VERIFICO SE SONO STATE INDICATE LE DATE ANCHE SE NON SONO PREVISTE PER IL TITOLO GIURIDICO INDICATO
                                '        If (Trim(ArrCampi(16)) <> String.Empty) Or (Trim(ArrCampi(15)) <> String.Empty) Then
                                '            strNote = strNote & "Le Data Stipula e Scadenza Contratto non sono previste per il Titolo Giuridico indicato."
                                '            swErr = True
                                '        End If
                                '    End If
                            End If
                        End If

                        'VOLONTARI ALLOCABILI
                        If VAR_VOLONTARIALLOCABILI = vbNullString Then
                            strNote = strNote & "Il campo Volontari Allocabili e' un campo obbligatorio. "
                            swErr = True
                        Else
                            'VOLONTARI ALLOCABILI
                            If IsNumeric(VAR_VOLONTARIALLOCABILI) = False Then
                                strNote = strNote & "Il campo Volontari Allocabili deve contenere solo numeri. "
                                swErr = True
                            ElseIf VAR_VOLONTARIALLOCABILI = 0 Then
                                strNote = strNote & "Il campo Volontari Allocabili deve essere maggiore di 0. "
                                swErr = True
                            End If


                        End If
                        'VOLONTARI MAGGIORE DI 20
                        If VAR_VOLMAGGIORI20 = vbNullString Then
                            strNote = strNote & "Il campo Volontari Maggiore di 20 e' un campo obbligatorio. "
                            swErr = True
                        Else
                            If Len(VAR_VOLMAGGIORI20) > 2 Then
                                strNote = strNote & "Il campo Volontari Maggiore di 20 puo' contenere massimo 2 caratteri. "
                                swErr = True
                            End If
                            If UCase(VAR_VOLMAGGIORI20) <> "SI" And UCase(VAR_VOLMAGGIORI20) <> "NO" Then
                                strNote = strNote & "Il campo Volontari Allocabili deve contenere solo Si o No. "
                                swErr = True
                            End If
                        End If
                        'CONTROLLO SUL CAMPO NMAXVOLONTARI
                        If IsNumeric(VAR_VOLONTARIALLOCABILI) = True Then
                            If VAR_VOLONTARIALLOCABILI <> vbNullString And VAR_VOLMAGGIORI20 <> vbNullString Then
                                If VAR_VOLONTARIALLOCABILI > 20 And UCase(VAR_VOLMAGGIORI20) = "NO" Then
                                    strNote = strNote & "Quando il numero Volontari Allocabili e' maggiore di 20 e' necessario mettere SI nella colonna Volontari Maggiore di 20. "
                                    swErr = True
                                End If
                            End If
                        End If

                        If VerificaNazioneComune(VAR_ISTATCOMUNE) = True Then
                            If VAR_NORMATIVA_81.ToUpper <> "SI" Then
                                strNote = strNote + "Per le sedi nazionali è obbligatoria la dichiarazione di rispetto della normativa 81. "
                                swErr = True
                            End If
                        Else
                            If VAR_TITOLOGIURIDICO.ToUpper = "LDA" Then
                                If VAR_SOGGETTO_ESTERO = String.Empty Then
                                    strNote = strNote + "Quando il titolo di possedimento è Lettera di Accordo va dichiarato soggetto estero. "
                                    swErr = True
                                End If

                                If VAR_DICHIARAZIONE_SOGGETTO_ESTERO = "" Then
                                    strNote = strNote + "Quando il Titolo di Possedimento è Lettera di Accordo il campo Conformità Lettera di accordo è obbligatorio. "
                                    swErr = True
                                Else
                                    If VAR_DICHIARAZIONE_SOGGETTO_ESTERO <> "CON" And VAR_DICHIARAZIONE_SOGGETTO_ESTERO <> "ATT" Then
                                        strNote = strNote + "Quando il Titolo di Possedimento è Lettera di Accordo il campo Conformità Lettera di accordo deve essere CON o ATT. "
                                        swErr = True
                                    End If
                                End If

                            Else
                                If VAR_CONFORMITA = "" Then
                                    strNote = strNote + "Per le Sedi Estere con Titolo di Possedimento diverso da è Lettera di Accordo il campo Conformità Titolo giuridico è obbligatorio. "
                                    swErr = True
                                Else
                                    If VAR_CONFORMITA <> "CON" And VAR_CONFORMITA <> "ATT" Then
                                        strNote = strNote + "Per le Sedi Estere con Titolo di Possedimento diverso da è Lettera di Accordo il campo Conformità Titolo giuridico deve essere CON o ATT. "
                                        swErr = True
                                    End If
                                End If

                            End If

                        End If
                        'AppoNome = ""
                        'AppoNome = Regex.Replace(VAR_DENOMINAZIONE, "[0-9]| ", String.Empty).Trim.ToUpper
                        Dim ripetizione As Boolean = False
                        If VAR_DENOMINAZIONE.Length > 3 Then
                            For idx = 0 To VAR_DENOMINAZIONE.Length - 4
                                If VAR_DENOMINAZIONE.Substring(idx, 1) = VAR_DENOMINAZIONE.Substring(idx + 1, 1) And VAR_DENOMINAZIONE.Substring(idx, 1) = VAR_DENOMINAZIONE.Substring(idx + 2, 1) And VAR_DENOMINAZIONE.Substring(idx, 1) = VAR_DENOMINAZIONE.Substring(idx + 3, 1) Then
                                    ripetizione = True
                                End If
                            Next
                        End If
                        If VAR_DENOMINAZIONE.Length > 5 Then
                            For idx = 0 To VAR_DENOMINAZIONE.Length - 6
                                If VAR_DENOMINAZIONE.Substring(idx, 2) = VAR_DENOMINAZIONE.Substring(idx + 2, 2) And VAR_DENOMINAZIONE.Substring(idx, 2) = VAR_DENOMINAZIONE.Substring(idx + 2, 2) Then
                                    ripetizione = True
                                End If
                            Next
                        End If

                        'If NomeSedi.Contains(AppoNome) Then
                        'strNote = strNote + "[La Denominazione della sede differisce da altra sede solo per caratteri numerici]. "
                        'VAR_FLG_ALERT_NOME_SEDE = "1"
                        'Else
                        'NomeSedi.Add(AppoNome)
                        'VAR_FLG_ALERT_NOME_SEDE = "0"
                        'End If

                        If ripetizione = True Then
                            strNote = strNote + "[Stesse sequenze di lettere ripetute nel nome sede]. "
                            VAR_FLG_ALERT_NOME_SEDE = "1"
                        Else
                            VAR_FLG_ALERT_NOME_SEDE = "0"
                        End If
                        AppoIndirizzo = ""
                        AppoIndirizzo = Replace(VAR_INDIRIZZO & VAR_CIVICO & VAR_CAP & VAR_ISTATCOMUNE & VAR_PALAZZINA, " ", "").ToUpper
                        If IndirizziSedi.Contains(AppoIndirizzo) Then
                            strNote = strNote + "Indirizzo Corrispondente a quello di altra sede"
                            VAR_FLG_ALERT_INDIRIZZO = "1"
                            'swErr = True
                        Else
                            IndirizziSedi.Add(AppoIndirizzo)
                            VAR_FLG_ALERT_INDIRIZZO = "0"
                        End If

                        If VerificaNazioneComune(VAR_ISTATCOMUNE) = True Then
                            Stato = "IT"
                        Else

                            Stato = CaricaCodiceStato(VAR_ISTATCOMUNE)
                        End If


                        If Not indirizzoOkDb Or erroreIndirizzo Then
                            Dim localita As String
                            If VAR_ISTATCOMUNE & "" <> "" Then
                                localita = getNomeComune(VAR_ISTATCOMUNE)
                            End If
                            If VAR_CITTAESTERA <> "" Then localita = VAR_CITTAESTERA
                            b = gm.checkAddress(Stato, localita, VAR_INDIRIZZO, VAR_CIVICO, VAR_CAP)
                            If b = False Then
                                'If gm.GoogleFormattedAddress = "" Then
                                '	'		strNote = strNote + "Indirizzo non trovato da google. "
                                '	strNote += "[" & gm.lastError & "] "
                                'Else
                                '	strNote += "[" & gm.lastError & " - Google suggerisce questo indirizzo " & gm.GoogleFormattedAddress & ".] "
                                'End If
                                Dim suggerimentoGoogle As String = ""
                                If gm.GoogleFormattedAddress <> "" Then
                                    suggerimentoGoogle = "Google suggerisce: " & gm.GoogleFormattedAddress
                                End If
                                strNote += "[ATTENZIONE: L’indirizzo digitato non trova riscontro in Google Maps. Si prega di controllare la correttezza di quanto inserito. In caso di permanenza dell’anomalia, il Dipartimento si riserva di effettuare tutti i successivi controlli di merito. " & suggerimentoGoogle & "]"
                                VAR_FLG_ALERT_INDIRIZZO_GOOGLE = 1
                                'swErr = True
                            Else
                                VAR_FLG_ALERT_INDIRIZZO_GOOGLE = 0
                            End If

                        Else

                            VAR_FLG_ALERT_INDIRIZZO_GOOGLE = 0 'gia' controllato su db, lo diamo per buono anche su google
                        End If


                        'qui
                        If VerificaNazioneComune(VAR_ISTATCOMUNE) = True Then
                            If CheckTipologiaEnte(zeriCF(VAR_CODICEFISCALE)) = False Then
                                intContaACFC = intContaACFC + 1
                            Else
                                intContaPartner = intContaPartner + 1
                            End If
                        End If


                        ArrCampi(17) = VAR_FLG_ALERT_NOME_SEDE
                        ArrCampi(18) = VAR_FLG_ALERT_INDIRIZZO_GOOGLE
                        ArrCampi(19) = VAR_FLG_ALERT_INDIRIZZO

                        If swErr = False Then
                            ScriviTabTemp(ArrCampi, AlboEnte)
                        Else
                            TotKo = TotKo + 1
                        End If
                    End If
                End If
                Dim CONTA As Integer = intRighe
                Writer.WriteLine(strNote & ";" & xLinea)
                strNote = vbNullString
                xLinea = Reader.ReadLine()

            End While

			'passo gli array che contentgono le segnalazioni 
			'sull'indirizzo ad una sessione, 
			'così da controllare nella visualizzazione delle note
			'gli eventuali suggerimenti
			If vSegnalazioneIndirizzo(0) <> "" Then
				Session("ArraySegnalazioneIndirizzo") = vSegnalazioneIndirizzo
			End If

			Reader.Close()
			Writer.Close()

			Dim strLocal As String = EseguiStoreVerificaSediPartner(Session("idente"), intContaACFC, intContaPartner)
			If strLocal <> "OK" Then
				chkNSedi = False
				TotKo = Tot
				TotOk = 0
				Response.Redirect("WfrmRisultatoImportRisorseSediEnti.aspx?NSediSuperate=true&NomeFile=" & NomeUnivoco & "&Tot=" & Tot & "&TotOk=" & TotOk & "&TotKo=" & TotKo)
				'--- reindirizzo la pagina sottostante
				'Response.Write("<script>" & vbCrLf)
				'Response.Write("parent.Naviga('WfrmRisultatoImportRisorseSediEnti.aspx?NSediSuperate=true&NomeFile=" & NomeUnivoco & "&Tot=" & Tot & "&TotOk=" & TotOk & "&TotKo=" & TotKo & "')" & vbCrLf)
				'Response.Write("</script>")
			Else
				Response.Redirect("WfrmRisultatoImportRisorseSediEnti.aspx?NomeFile=" & NomeUnivoco & "&Tot=" & Tot & "&TotOk=" & TotOk & "&TotKo=" & TotKo)
				'--- reindirizzo la pagina sottostante
				'Response.Write("<script>" & vbCrLf)
				'Response.Write("parent.Naviga('WfrmRisultatoImportRisorseSediEnti.aspx?NomeFile=" & NomeUnivoco & "&Tot=" & Tot & "&TotOk=" & TotOk & "&TotKo=" & TotKo & "')" & vbCrLf)
				'Response.Write("</script>")
			End If

		Catch ex As Exception
			'Log.Critical(LogEvent.IMPORTAZIONE_SEDI_INSERIMENTO_ERRORE, ex.Message)
			'Response.Write(xLinea.ToString)
			'Response.Write(intRighe.ToString)
			'Response.Write(ex.Message.ToString)
		End Try


	End Sub

	Function EseguiStoreVerificaSediPartner(ByVal IdEnte As Integer, ByVal intSediAcf As Integer, ByVal intSediPartner As Integer) As String
		'AUTORE: Antonello Di Croce
		'DESCRIZIONE: richiamo la SP per la cancellazione dei volontari in stato registrato da una graduatoria
		'DATA: 05/01/2007
		Dim Reader As SqlClient.SqlDataReader

		Try

			Dim sReturnValue As String
			Dim MyCommand As New SqlClient.SqlCommand
			MyCommand.CommandType = CommandType.StoredProcedure
			MyCommand.CommandText = "SP_VERIFICA_SEDI_PARTNER"
			MyCommand.Connection = Session("conn")

			'PRIMO PARAMETRO 
			Dim sparam As SqlClient.SqlParameter
			sparam = New SqlClient.SqlParameter
			sparam.ParameterName = "@IdEnte"
			sparam.SqlDbType = SqlDbType.Int
			MyCommand.Parameters.Add(sparam)

			'SECONDO PARAMETRO 
			Dim sparam1 As SqlClient.SqlParameter
			sparam1 = New SqlClient.SqlParameter
			sparam1.ParameterName = "@NAggiunto"
			sparam1.SqlDbType = SqlDbType.Int
			MyCommand.Parameters.Add(sparam1)

			'TERZO PARAMETRO 
			Dim sparamACF As SqlClient.SqlParameter
			sparamACF = New SqlClient.SqlParameter
			sparamACF.ParameterName = "@NAggiuntoACF"
			sparamACF.SqlDbType = SqlDbType.Int
			MyCommand.Parameters.Add(sparamACF)

			'@NAggiuntoACF

			'PARAMETRO OUTPUT
			Dim sparam2 As SqlClient.SqlParameter
			sparam2 = New SqlClient.SqlParameter
			sparam2.ParameterName = "@Valore"
			sparam2.Size = 20
			sparam2.SqlDbType = SqlDbType.Int
			sparam2.Direction = ParameterDirection.Output
			MyCommand.Parameters.Add(sparam2)

			'PARAMETRO OUTPUT
			Dim sparam3 As SqlClient.SqlParameter
			sparam3 = New SqlClient.SqlParameter
			sparam3.ParameterName = "@Causale"
			sparam3.Size = 200
			sparam3.SqlDbType = SqlDbType.VarChar
			sparam3.Direction = ParameterDirection.Output
			MyCommand.Parameters.Add(sparam3)



			MyCommand.Parameters("@IdEnte").Value = IdEnte
			MyCommand.Parameters("@NAggiunto").Value = intSediPartner
			MyCommand.Parameters("@NAggiuntoACF").Value = intSediAcf

			MyCommand.ExecuteNonQuery()

			sReturnValue = MyCommand.Parameters("@Causale").Value
			If Not Reader Is Nothing Then
				Reader.Close()
				Reader = Nothing
			End If
			Return sReturnValue

		Catch ex As Exception
			If Not Reader Is Nothing Then
				Reader.Close()
				Reader = Nothing
			End If
			Return 1
		End Try
	End Function
    Function getNomeComune(ByVal codiceIstat As String) As String
        getNomeComune = ""
        Dim dtrLocal As SqlClient.SqlDataReader
        Dim strSql As String

        strSql = "Select denominazione from comuni where codiceistat='" & Replace(codiceIstat, "'", "") & "'"

        dtrLocal = ClsServer.CreaDatareader(strSql, Session("conn"))  'Correzione ADC 27/10/2021
        If dtrLocal.HasRows Then
            dtrLocal.Read()
            getNomeComune = dtrLocal("denominazione")
        End If
        dtrLocal.Close()
        dtrLocal = Nothing
        'conn.Close()
        'conn = Nothing

    End Function
	'TRUE E' UN PARTNER
	'FALSE E' il PAPA' o ACFC
	Function CheckTipologiaEnte(ByVal CodiceFiscale As String) As Boolean
		Dim dtrLocal As SqlClient.SqlDataReader
		Dim strSql As String

		strSql = "Select CodiceFiscale FROM Enti where CodiceFiscale='" & Replace(CodiceFiscale, "'", "''") & "' AND IdEnte in (SELECT IdEnteFiglio from entirelazioni where identepadre=" & Session("IdEnte") & " AND IdTipoRelazione=4)"

		dtrLocal = ClsServer.CreaDatareader(strSql, Session("conn"))

		CheckTipologiaEnte = dtrLocal.HasRows

		dtrLocal.Close()
		dtrLocal = Nothing

	End Function

	Function CheckEsistenzaCodiceFiscale(ByVal CodiceFiscale As String) As Boolean
		Dim dtrLocal As SqlClient.SqlDataReader
		Dim strSql As String

		strSql = "Select CodiceFiscale FROM #IMP_ENTI where CodiceFiscale='" & Replace(CodiceFiscale, "'", "''") & "'"

		dtrLocal = ClsServer.CreaDatareader(strSql, Session("conn"))

		CheckEsistenzaCodiceFiscale = dtrLocal.HasRows

		dtrLocal.Close()
		dtrLocal = Nothing

	End Function

	'ok
	'Funzione di controllo per l'import degli "ENTI in partnerariato"
    Private Sub LeggiCSVenti()
        '--- lettura del file
        Dim Reader As StreamReader
        Dim xLinea As String
        Dim ArrCampi() As String
        Dim swErr As Boolean
        Dim AppoNote As String
        Dim strNoteCap As String
        Dim NColonna As Integer

        Dim ESITO As String
        Dim strMsgEmail As String = ""
        Dim strMsg As String = ""
        Dim intIDE As Integer = 0



        Dim AlboEnte As String
        AlboEnte = ClsUtility.TrovaAlboEnte(Session("IdEnte"), Session("conn"))
        If AlboEnte = "SCN" Then
			NColonna = 16
		Else
			NColonna = 71 'era 16
		End If

        Try
            '--- Leggo il file di input e scrivo quello di output
            'Reader = New StreamReader(Server.MapPath("upload") & "\" & NomeUnivoco & ".CSV")
            Reader = New StreamReader(Server.MapPath("upload") & "\" & NomeUnivoco & ".CSV", System.Text.Encoding.Default, False)
            Writer = New StreamWriter(Server.MapPath("download") & "\" & NomeUnivoco & ".CSV", False, System.Text.Encoding.Default)

            '--- intestazione

            xLinea = Reader.ReadLine()
            Writer.WriteLine("Note;" & xLinea)

            'creo un array che metto in sessione per concatenarlo alle note in fase di segnalazione errori
            Dim vSegnalazioneIndirizzo() As String
            ReDim vSegnalazioneIndirizzo(0)

            '--- scorro le righe
            xLinea = Reader.ReadLine()
            While (xLinea <> "")
                swErr = False
                Tot = Tot + 1
                ArrCampi = CreaArray(xLinea)

                If UBound(ArrCampi) < NColonna Then
                    '--- se i campi non sono tutti errore
                    strNote = "Il numero delle colonne inserite è minore di quello richieste."
                    swErr = True
                    TotKo = TotKo + 1
                Else
                    If UBound(ArrCampi) > NColonna Then
                        '--- se i campi sono troppi errore
                        strNote = "Il numero delle colonne inserite è maggiore di quello richieste."
                        swErr = True
                        TotKo = TotKo + 1
                    Else
                        'Denominazione    
                        If Trim(ArrCampi(0)) = vbNullString Then
                            strNote = strNote & "La denominazione e' un campo obbligatorio."
                            swErr = True
                        Else
                            If Len(ArrCampi(0)) > 200 Then
                                strNote = strNote & "La denominazione puo' contenere massimo 200 caratteri."
                                swErr = True
                            Else
                                If DenUnivEnte(ArrCampi(0), AlboEnte) = False Then
                                    strNote = strNote & "La denominazione dell'ente deve essere univoca."
                                    swErr = True
                                End If
                            End If
                        End If
                        ''*************************************************
                        ''AGGIUNTO DA SIMONA CORDELLA IL 18/10/2017
                        '' richiamo store per controllo codicefiscale e demoniazione ente nell'albo
                        'ESITO = ClsUtility.STORE_VERIFICA_USABILITA_ENTE_ALBO("", Trim(ArrCampi(0)), "SCU", intIDE, strMsgEmail, strMsg, Session("conn"))
                        'If ESITO = "NO" Then
                        '    strNote = strNote & strMsg
                        '    swErr = True
                        'End If
                        ''**************


                        'CodiceFiscale (partita IVA)
                        If Trim(ArrCampi(1)) = vbNullString Then
                            strNote = strNote & "Il campo CodiceFiscale e' un campo obbligatorio."
                            swErr = True
                        Else
                            If DenUnivCodFis(Trim(ArrCampi(1)), AlboEnte) = False Then
                                strNote = strNote & "Il Codice Fiscale deve essere univoco."
                                swErr = True
                            End If

                            ''*************************************************
                            ''AGGIUNTO DA SIMONA CORDELLA IL 18/10/2017
                            '' richiamo store per controllo codicefiscale e demoniazione ente nell'albo
                            'ESITO = ClsUtility.STORE_VERIFICA_USABILITA_ENTE_ALBO(Trim(ArrCampi(1)), "", "SCU", intIDE, strMsgEmail, strMsg, Session("conn"))
                            'If ESITO = "NO" Then
                            '    strNote = strNote & strMsg
                            '    swErr = True
                            'End If
                            ''**************
                        End If

						''Denominazione    
						'If Trim(ArrCampi(0)) = vbNullString Then
						'    strNote = strNote & "La denominazione e' un campo obbligatorio."
						'    swErr = True
						'Else
						'    If Len(ArrCampi(0)) > 200 Then
						'        strNote = strNote & "La denominazione puo' contenere massimo 200 caratteri."
						'        swErr = True
						'    Else
						'        If DenUnivEnte(ArrCampi(0)) = False Then
						'            strNote = strNote & "La denominazione dell'ente deve essere univoca."
						'            swErr = True
						'        End If
						'    End If
						'End If

						''CodiceFiscale (partita IVA)
						'If Trim(ArrCampi(1)) = vbNullString Then
						'    strNote = strNote & "Il campo CodiceFiscale e' un campo obbligatorio."
						'    swErr = True
						'Else
						'    'If UnivocitaCodiceFiscaleEnti(Trim(ArrCampi(1))) = True Then
						'    '    strNote = strNote & "Il Codice Fiscale dell'ente è gia' presente nella base dati."
						'    '    swErr = True
						'    'End If
						'If DenUnivCodFis(Trim(ArrCampi(1))) = False Then
						'        strNote = strNote & "Il Codice Fiscale deve essere univoco."
						'        swErr = True
						'    End If
						'End If

						'Tipologia
						If Trim(ArrCampi(2)) = vbNullString Then
							strNote = strNote & "Il campo Tipologia e' un campo obbligatorio."
							swErr = True
						Else
							If CheckEsistTipEnte(Trim(ArrCampi(2)), AlboEnte) = False Then
								strNote = strNote & "Il campo Tipologia non esiste."
								swErr = True
							Else
								If CheckTipEnte(Trim(ArrCampi(2))) = False And AlboEnte = "SCN" Then
									strNote = strNote & "Il campo Tipologia non è compatibile con la tipologia dell'Ente Titolare."
									swErr = True
								End If
							End If
						End If

						If Trim(ArrCampi(2)).ToUpper = "ALTRO" And Trim(ArrCampi(3)) = String.Empty Then
							strNote = strNote & "Se si indica 'ALTRO' nel campo Tipologia va' specificato il campo 'Altro Tipo Ente'"
							swErr = True
						End If


						'Tipo Relazione
						If Trim(ArrCampi(4)) = vbNullString Then
							strNote = strNote & "Il campo Tipo Relazione e' un campo obbligatorio."
							swErr = True
						Else
							If AlboEnte = "SCN" Then
								If UCase(Trim(ArrCampi(4))) <> "FED" And UCase(Trim(ArrCampi(4))) <> "CON" And UCase(Trim(ArrCampi(4))) <> "ASS" And UCase(Trim(ArrCampi(4))) <> "PAR" And UCase(Trim(ArrCampi(4))) <> "CAN" Then
									strNote = strNote & "Il campo Tipo Relazione non esiste."
									swErr = True
								Else
									If ClsRicEnteTipRel(Trim(ArrCampi(4)) = False) Then
										strNote = strNote & "Il campo Tipo Relazione non è compatibile con la classe dell'Ente Titolare."
										swErr = True
									End If
								End If
                            Else
								If UCase(Trim(ArrCampi(4))) <> "FED" And UCase(Trim(ArrCampi(4))) <> "CON" And UCase(Trim(ArrCampi(4))) <> "ASS" And UCase(Trim(ArrCampi(4))) <> "CAN" And UCase(Trim(ArrCampi(4))) <> "CNT" Then
									strNote = strNote & "Il campo Tipo Relazione non esiste."
									swErr = True
								End If
							End If

                        End If

						'Istat Comune
						If Trim(ArrCampi(5)) = vbNullString Then
							strNote = strNote & "Il campo CodiceISTATComune e' un campo obbligatorio."
							swErr = True
						Else
							If VerificaComune(Trim(ArrCampi(5))) = False Then
								strNote = strNote & "Il CodiceISTATComune inserito non esiste."
								swErr = True
							End If
						End If

						'Indirizzo
						If Trim(ArrCampi(6)) = vbNullString Then
							strNote = strNote & "L'indirizzo e' un campo obbligatorio."
							swErr = True
						Else
							'CONTROLLO ESISTENZA INDIRIZZO
							'CONTROLLO AGGIUNTO DA THE GREAT SOUTHERN JONKILL
							'27 marzo 2009
							'vado a prendere le informazioni del comune che si sta tentando di inserire
							Dim informazioniComune As New clsInformazioniComune(Session("conn"), , ArrCampi(5), )
							Dim informazioniComunePerID As New clsInformazioniComune(Session("conn"), , , informazioniComune.getIdComune_byCodiceIstat)
                            'controllo se l'indirizzo è valido, quindi ho un idcomune con indirizzi
                            If informazioniComune.getIdComune_byCodiceIstat > 0 Then
								'dichiaro la classe contenente le informazioni dell'indirizzo
								Dim chkIndirizzo As New clsIndirizzi(ArrCampi(6), informazioniComune.getIdComune_byCodiceIstat, Session("conn"))
								'se il comune ha gli indirizzi vado a controllare se l'indirizzo esiste
								If informazioniComunePerID.checkEsistenzaIndirizzi = True Then
                                    'se l'indirizzo che si sta inserendo non esiste mando un messaggio all'utente
                                    If chkIndirizzo.CheckIndirizzo = False Then
                                        If vSegnalazioneIndirizzo(0) = "" Then
                                            vSegnalazioneIndirizzo(0) = "L'indirizzo non &egrave; valido: <a style=" & """" & "cursor:pointer" & """" & " onclick=" & """" & "ApriSuggerimento('" & informazioniComune.getIdComune_byCodiceIstat & "','" & Replace(ArrCampi(5), "'", "|") & "')" & """" & "><font color=" & """" & "blue" & """" & "><b>SUGGERIMENTO</b></font></a> ."
                                        Else
                                            ReDim Preserve vSegnalazioneIndirizzo(UBound(vSegnalazioneIndirizzo) + 1)
                                            vSegnalazioneIndirizzo(UBound(vSegnalazioneIndirizzo)) = "L'indirizzo non &egrave; valido: <a style=" & """" & "cursor:pointer" & """" & " onclick=" & """" & "ApriSuggerimento('" & informazioniComune.getIdComune_byCodiceIstat & "','" & Replace(ArrCampi(5), "'", "|") & "')" & """" & "><font color=" & """" & "blue" & """" & "><b>SUGGERIMENTO</b></font></a> ."
                                        End If
                                        strNote = strNote & "L'indirizzo non e' valido."
                                        swErr = True
                                    End If
                                End If
                            End If
                        End If

						'Civico
						If Trim(ArrCampi(7)) = vbNullString Then
							strNote = strNote & "Il campo Civico e' un campo obbligatorio."
							swErr = True
						End If

						'CheckEsistenzaCAP
						If Trim(ArrCampi(8)) = vbNullString Then
							strNote = strNote & "Il campo CAP e' un campo obbligatorio."
							swErr = True
						Else
							'se il comune è nazionale devo controllare il se c'è congruenza fra codice istat e cap
							If VerificaNazioneComune(Trim(ArrCampi(5))) = True Then
								'If CheckEsistenzaCAP(Trim(ArrCampi(4)), Trim(ArrCampi(7))) = False Then
								'    strNote = strNote & "Il campo CAP non e' congruo con i dati inseriti."
								'    swErr = True
								'End If
								Dim strCausale As String
								Dim blnLocal As Boolean

								If ClsUtility.CAP_VERIFICA(Session("conn"), strCausale, blnLocal, Trim(ArrCampi(8)), "0", "", Trim(ArrCampi(5)), Trim(ArrCampi(6)), Trim(ArrCampi(7))) = False Then
									strNote = strNote & strCausale & ". "
									swErr = True
								End If
							End If
						End If

						'Prefisso
						If Trim(ArrCampi(9)) = vbNullString Then
							strNote = strNote & "Il campo Prefisso e' un campo obbligatorio."
							swErr = True
						Else
							If IsNumeric(Trim(ArrCampi(9))) = False Then
								strNote = strNote & "Il campo prefisso deve essere un numero."
								swErr = True
							End If
						End If

						'Telefono
						If Trim(ArrCampi(10)) = vbNullString Then
							strNote = strNote & "Il campo Prefisso e' un campo obbligatorio."
							swErr = True
						Else
							If IsNumeric(Trim(ArrCampi(10))) = False Then
								strNote = strNote & "Il campo prefisso deve essere un numero."
								swErr = True
							End If
						End If

						'/==================================SETTORI DI INTERVENTO=====================================\
						'NUOVA GESTIONE IMPORT ENTI IN ACCORDO CON LE INFORMAZIONI SUI SETTORI DI INTERVENTO PER OGNI ENTE
						'MODIFICA EFFETTUATA DA JONASTRONZ IL
						'15/06/2009
						'ASSISTENZA
						If Trim(ArrCampi(11)) = vbNullString Then
							strNote = strNote & "Il campo Assistenza e' un campo obbligatorio."
							swErr = True
						Else
							If UCase(Trim(ArrCampi(11))) <> "SI" And UCase(Trim(ArrCampi(11))) <> "NO" Then
								strNote = strNote & "Il campo Assistenza deve contenere SI o NO."
								swErr = True
							End If
						End If
						'Protezione CIVILE
						If Trim(ArrCampi(12)) = vbNullString Then
							strNote = strNote & "Il campo Protezione Civile e' un campo obbligatorio."
							swErr = True
						Else
							If UCase(Trim(ArrCampi(12))) <> "SI" And UCase(Trim(ArrCampi(12))) <> "NO" Then
								strNote = strNote & "Il campo Protezione Civile deve contenere SI o NO."
								swErr = True
							End If
						End If
						'AMBIENTE
						If Trim(ArrCampi(13)) = vbNullString Then
							strNote = strNote & "Il campo Ambiente e' un campo obbligatorio."
							swErr = True
						Else
							If UCase(Trim(ArrCampi(13))) <> "SI" And UCase(Trim(ArrCampi(13))) <> "NO" Then
								strNote = strNote & "Il campo Ambiente deve contenere SI o NO."
								swErr = True
							End If
						End If
						'PaATRIMONIO ARTISTICO
						If Trim(ArrCampi(14)) = vbNullString Then
							strNote = strNote & "Il campo Patrimonio Artistico e' un campo obbligatorio."
							swErr = True
						Else
							If UCase(Trim(ArrCampi(14))) <> "SI" And UCase(Trim(ArrCampi(14))) <> "NO" Then
								strNote = strNote & "Il campo Patrimonio Artistico deve contenere SI o NO."
								swErr = True
							End If
						End If
						'PROMOZIONE CULTURALE
						If Trim(ArrCampi(15)) = vbNullString Then
							strNote = strNote & "Il campo Promozione Culturale e' un campo obbligatorio."
							swErr = True
						Else
							If UCase(Trim(ArrCampi(15))) <> "SI" And UCase(Trim(ArrCampi(15))) <> "NO" Then
								strNote = strNote & "Il campo Promozione Culturale deve contenere SI o NO."
								swErr = True
							End If
						End If
						'ESTERO
						If Trim(ArrCampi(16)) = vbNullString Then
							strNote = strNote & "Il campo Estero e' un campo obbligatorio."
							swErr = True
						Else
							If UCase(Trim(ArrCampi(16))) <> "SI" And UCase(Trim(ArrCampi(16))) <> "NO" Then
								strNote = strNote & "Il campo Estero deve contenere SI o NO."
								swErr = True
							End If
						End If

						If NColonna = 71 Then 'se il numero di colonne previsto è quello del SCU
							'AGRICOLTURA
							If Trim(ArrCampi(17)) = vbNullString Then
								strNote = strNote & "Il campo Agricoltura e' un campo obbligatorio."
								swErr = True
							Else
								If UCase(Trim(ArrCampi(17))) <> "SI" And UCase(Trim(ArrCampi(17))) <> "NO" Then
									strNote = strNote & "Il campo Agricoltura deve contenere SI o NO."
									swErr = True


								End If
							End If

							'/=============================================================================================================\
							'NUOVA GESTIONE IMPORT ENTI CON DATI RAPPRESENTANTE LEGALE, NUOVI CHECK, ANNI, DESCRIZIONI ED AREE DI INTERVENTO
							'data nomina rl
							If Trim(ArrCampi(18)) = vbNullString Then
								strNote = strNote & "Il campo Data Nomina Rappresentante Legale e' un campo obbligatorio."
								swErr = True
							Else
								Dim dataRl As Date
								If Trim(ArrCampi(18)).Length <> 10 OrElse Date.TryParse(Trim(ArrCampi(18)), dataRl) = False Then
									strNote = strNote & "Il campo Data Nomina Rappresentante Legale non e' valido: inserire la data nel formato GG/MM/AAAA."
									swErr = True
								End If
							End If

							'codice fiscale rl
							If Trim(ArrCampi(19)) = vbNullString Then
								strNote = strNote & "Il campo Codice Fiscale Rappresentante Legale e' un campo obbligatorio."
								swErr = True
							Else
								Dim dataRl As Date
								If Trim(ArrCampi(19)).Length <> 16 Then 'questo controllo forse va affinato (univocità?)
									strNote = strNote & "Il campo Codice Fiscale Rappresentante Legale non è valido."
									swErr = True
								End If
							End If

							'Attivita negli ultimi tre anni
							If Trim(ArrCampi(20)) = vbNullString Then
								strNote = strNote & "Il campo Attivita negli ultimi tre anni e' un campo obbligatorio."
								swErr = True
							Else
								If UCase(Trim(ArrCampi(20))) <> "SI" Then
									strNote = strNote & "Il campo Attivita negli ultimi tre anni deve contenere SI."
									swErr = True
								End If
							End If

							'Attivita negli ultimi tre anni
							If Trim(ArrCampi(21)) = vbNullString Then
								strNote = strNote & "Il campo Attivita per Fini Istituzionali e' un campo obbligatorio."
								swErr = True
							Else
								If UCase(Trim(ArrCampi(21))) <> "SI" Then
									strNote = strNote & "Il campo Attivita per Fini Istituzionali deve contenere SI."
									swErr = True
								End If
							End If

							'Attivita negli ultimi tre anni
							'devo controllare anche il campo tipologia che mi serve per sapere se ente pubblico o privato
							If Trim(ArrCampi(2)) = vbNullString OrElse CheckEsistTipEnte(Trim(ArrCampi(2)), AlboEnte) = False OrElse (CheckTipEnte(Trim(ArrCampi(2))) = False And AlboEnte = "SCN") Then
								strNote = strNote & "Il campo Attivita senza scopo di Lucro dipende dal campo Tipologia."
								swErr = True
							Else
								If IsPrivatoByCodiceImport(Trim(ArrCampi(2))) Then
									If Trim(ArrCampi(22)) = vbNullString Then
										strNote = strNote & "Il campo Attivita senza scopo di Lucro e' un campo obbligatorio."
										swErr = True
									Else
										If UCase(Trim(ArrCampi(22))) <> "SI" Then
											strNote = strNote & "Il campo Attivita senza scopo di Lucro deve contenere SI."
											swErr = True
										End If
									End If
								Else
									If Trim(ArrCampi(22)) <> vbNullString AndAlso Trim(ArrCampi(22)) <> "" Then
										strNote = strNote & "Il campo Attivita senza scopo di Lucro non va compilato per Enti Pubblici."
										swErr = True
									End If
								End If
							End If

							swErr = swErr Or ErroreSettore(ArrCampi, "Assistenza", "A", 23, 11, strNote)
							swErr = swErr Or ErroreSettore(ArrCampi, "Protezione Civile", "B", 30, 12, strNote)
							swErr = swErr Or ErroreSettore(ArrCampi, "Ambiente", "C", 37, 13, strNote)
							swErr = swErr Or ErroreSettore(ArrCampi, "Patrimonio Artistico", "D", 44, 14, strNote)
							swErr = swErr Or ErroreSettore(ArrCampi, "Promozione Culturale", "E", 51, 15, strNote)
							swErr = swErr Or ErroreSettore(ArrCampi, "Estero", "G", 58, 16, strNote)                    'attenzione in maschera sono in ordine diverso rispetto all'excel
							swErr = swErr Or ErroreSettore(ArrCampi, "Agricoltura", "F", 65, 17, strNote)               'attenzione in maschera sono in ordine diverso rispetto all'excel
							'FINE 'NUOVA GESTIONE IMPORT ENTI CON DATI RAPPRESENTANTE LEGALE, NUOVI CHECK, ANNI, DESCRIZIONI ED AREE DI INTERVENTO
							'/===================================================================================================================\

						End If

						'/============================================================================================\

						If swErr = False Then
                            ScriviTabTemp(ArrCampi, AlboEnte)
                        Else
                            TotKo = TotKo + 1
                        End If
                    End If
                End If
                Writer.WriteLine(strNote & ";" & xLinea)
                strNote = vbNullString
                xLinea = Reader.ReadLine()
            End While

            'passo gli array che contentgono le segnalazioni 
            'sull'indirizzo ad una sessione, 
            'così da controllare nella visualizzazione delle note
            'gli eventuali suggerimenti
            If vSegnalazioneIndirizzo(0) <> "" Then
                Session("ArraySegnalazioneIndirizzo") = vSegnalazioneIndirizzo
            End If

            Reader.Close()
            Writer.Close()

            Response.Redirect("WfrmRisultatoImportRisorseSediEnti.aspx?NomeFile=" & NomeUnivoco & "&Tot=" & Tot & "&TotOk=" & TotOk & "&TotKo=" & TotKo)
            ''--- reindirizzo la pagina sottostante
            'Response.Write("<script>" & vbCrLf)
            'Response.Write("parent.Naviga('WfrmRisultatoImportRisorseSediEnti.aspx?NomeFile=" & NomeUnivoco & "&Tot=" & Tot & "&TotOk=" & TotOk & "&TotKo=" & TotKo & "')" & vbCrLf)
            'Response.Write("</script>")
        Catch ex As Exception
            Log.Error(LogEvent.ENTI_ERRORE_IMPORTAZIONE_MASSIVA, "Errore lettura CSV", ex)
            lblMessaggioErrore.Visible = True
            lblMessaggioErrore.Text = "Errore lettura CSV."
            Exit Sub
        End Try

    End Sub
	'ok
    Function ErroreSettore(ByRef ArrCampi As String(), ByVal NomeSettore As String, ByVal CodificaSettore As String, ByVal IndiceCampoUltimoAnno As Integer, ByVal IndiceCampoSettoreSiNo As Integer, ByRef strNote As String) As Boolean
        Dim swErr As Boolean = False
        Dim isCampoSettoreSiNoCompilato As Boolean = False

        'Serve per decidere che errori eventualmente dare a seconda che il corrispettivo SI/NO sia compilato o no
        If Trim(ArrCampi(IndiceCampoSettoreSiNo)) <> vbNullString AndAlso UCase(Trim(ArrCampi(IndiceCampoSettoreSiNo))) = "SI" Then
            isCampoSettoreSiNoCompilato = True
        End If

        'Ultimo Anno
        If isCampoSettoreSiNoCompilato Then
            If Trim(ArrCampi(IndiceCampoUltimoAnno)) <> vbNullString AndAlso Trim(ArrCampi(IndiceCampoUltimoAnno)) <> "" Then
                Dim anno As Integer
                If Not Integer.TryParse(Trim(ArrCampi(IndiceCampoUltimoAnno)), anno) OrElse anno <> DateTime.Now.Year - 1 Then
                    strNote = strNote & "Il campo Ultimo Anno " & NomeSettore & " deve contenere " & (DateTime.Now.Year - 1).ToString & "."
                    swErr = True
                End If
            Else
                strNote = strNote & "Il campo Ultimo Anno " & NomeSettore & " e' un campo obbligatorio."
                swErr = True
            End If
        Else
            If Trim(ArrCampi(IndiceCampoUltimoAnno)) <> vbNullString AndAlso Trim(ArrCampi(IndiceCampoUltimoAnno)) <> "" Then
                strNote = strNote & "Il campo Ultimo Anno " & NomeSettore & " va compilato solo se il campo " & NomeSettore & " e' SI."
                swErr = True
            End If
        End If

        'Esperienza ultimo anno
        If isCampoSettoreSiNoCompilato Then
            If Trim(ArrCampi(IndiceCampoUltimoAnno + 1)) = vbNullString AndAlso Trim(ArrCampi(IndiceCampoUltimoAnno + 1)) = "" Then
                strNote = strNote & "Il campo Esperienza Ultimo Anno " & NomeSettore & " e' un campo obbligatorio."
                swErr = True
            End If
        Else
            If Trim(ArrCampi(IndiceCampoUltimoAnno + 1)) <> vbNullString AndAlso Trim(ArrCampi(IndiceCampoUltimoAnno + 1)) <> "" Then
                strNote = strNote & "Il campo Esperienza Ultimo Anno " & NomeSettore & " va compilato solo se il campo " & NomeSettore & " e' SI."
                swErr = True
            End If
        End If

        'Penultimo Anno
        If isCampoSettoreSiNoCompilato Then
            If Trim(ArrCampi(IndiceCampoUltimoAnno + 2)) <> vbNullString AndAlso Trim(ArrCampi(IndiceCampoUltimoAnno + 2)) <> "" Then
                Dim anno As Integer
                If Not Integer.TryParse(Trim(ArrCampi(IndiceCampoUltimoAnno + 2)), anno) OrElse anno <> DateTime.Now.Year - 2 Then
                    strNote = strNote & "Il campo Penultimo Anno " & NomeSettore & " deve contenere " & (DateTime.Now.Year - 2).ToString & "."
                    swErr = True
                End If
            Else
                strNote = strNote & "Il campo Penultimo Anno " & NomeSettore & " e' un campo obbligatorio."
                swErr = True
            End If
        Else
            If Trim(ArrCampi(IndiceCampoUltimoAnno + 2)) <> vbNullString AndAlso Trim(ArrCampi(IndiceCampoUltimoAnno + 2)) <> "" Then
                strNote = strNote & "Il campo Penultimo Anno " & NomeSettore & " va compilato solo se il campo " & NomeSettore & " e' SI."
                swErr = True
            End If
        End If

        'Esperienza penultimo anno
        If isCampoSettoreSiNoCompilato Then
            If Trim(ArrCampi(IndiceCampoUltimoAnno + 3)) = vbNullString AndAlso Trim(ArrCampi(IndiceCampoUltimoAnno + 3)) = "" Then
                strNote = strNote & "Il campo Esperienza Penultimo Anno " & NomeSettore & " e' un campo obbligatorio."
                swErr = True
            End If
        Else
            If Trim(ArrCampi(IndiceCampoUltimoAnno + 3)) <> vbNullString AndAlso Trim(ArrCampi(IndiceCampoUltimoAnno + 3)) <> "" Then
                strNote = strNote & "Il campo Esperienza Penultimo Anno " & NomeSettore & " va compilato solo se il campo " & NomeSettore & " e' SI."
                swErr = True
            End If
        End If

        'Terzultimo Anno
        If isCampoSettoreSiNoCompilato Then
            If Trim(ArrCampi(IndiceCampoUltimoAnno + 4)) <> vbNullString AndAlso Trim(ArrCampi(IndiceCampoUltimoAnno + 4)) <> "" Then
                Dim anno As Integer
                If Not Integer.TryParse(Trim(ArrCampi(IndiceCampoUltimoAnno + 4)), anno) OrElse anno <> DateTime.Now.Year - 3 Then
                    strNote = strNote & "Il campo Terzultimo Anno " & NomeSettore & " deve contenere " & (DateTime.Now.Year - 3).ToString & "."
                    swErr = True
                End If
            Else
                strNote = strNote & "Il campo Terzultimo Anno " & NomeSettore & " e' un campo obbligatorio."
                swErr = True
            End If
        Else
            If Trim(ArrCampi(IndiceCampoUltimoAnno + 4)) <> vbNullString AndAlso Trim(ArrCampi(IndiceCampoUltimoAnno + 4)) <> "" Then
                strNote = strNote & "Il campo Terzultimo Anno " & NomeSettore & " va compilato solo se il campo " & NomeSettore & " e' SI."
                swErr = True
            End If
        End If

        'Esperienza terzultimo anno
        If isCampoSettoreSiNoCompilato Then
            If Trim(ArrCampi(IndiceCampoUltimoAnno + 5)) = vbNullString AndAlso Trim(ArrCampi(IndiceCampoUltimoAnno + 5)) = "" Then
                strNote = strNote & "Il campo Esperienza Terzultimo Anno " & NomeSettore & " e' un campo obbligatorio."
                swErr = True
            End If
        Else
            If Trim(ArrCampi(IndiceCampoUltimoAnno + 5)) <> vbNullString AndAlso Trim(ArrCampi(IndiceCampoUltimoAnno + 5)) <> "" Then
                strNote = strNote & "Il campo Esperienza Terzultimo Anno " & NomeSettore & " va compilato solo se il campo " & NomeSettore & " e' SI."
                swErr = True
            End If
        End If

        'Aree d'intervento
        If isCampoSettoreSiNoCompilato Then
            If Trim(ArrCampi(IndiceCampoUltimoAnno + 6)) = vbNullString AndAlso Trim(ArrCampi(IndiceCampoUltimoAnno + 6)) = "" Then
                strNote = strNote & "Il campo Aree d'intervento " & NomeSettore & " e' un campo obbligatorio."
                swErr = True
            Else
                Dim _idString As String = ""    'serve a convertire programmaticamente la stringa contenente i codici area concatenati (es. A01,A02,A03) in una stringa contenente gli id corrispondenti (es. 11,23,45)
                Dim _elencoCodiciAreeIntervento As Dictionary(Of String, Integer) = GetCodificheAreeIntervento(CodificaSettore)
                Dim _elencoAreeCsv = CreaArray(Trim(ArrCampi(IndiceCampoUltimoAnno + 6)), ",").ToList
                For Each c As String In _elencoAreeCsv.Distinct
                    If Not _elencoCodiciAreeIntervento.ContainsKey(c) Then
                        strNote = strNote & "Il campo Aree d'intervento " & NomeSettore & " contiene un codice Area(" & c & ") non valido."
                        swErr = True
                    ElseIf _elencoAreeCsv.FindAll(Function(s) s = c).Count > 1 Then
                        strNote = strNote & "Il campo Aree d'intervento " & NomeSettore & " contiene più volte il codice Area " & c & "."
                        swErr = True
                    Else
                        _idString += _elencoCodiciAreeIntervento(c).ToString + ","
                    End If
                Next
                If Not swErr Then ArrCampi(IndiceCampoUltimoAnno + 6) = _idString.TrimEnd(","c) 'cambio la stringa contenente i codici delle aree di intervento con quella contenente i suoi id
            End If
        End If
        Return swErr
    End Function


    'ok
    Private Function CreaArray(ByVal pLinea As String, Optional ByVal separatore As String = ";") As String()
        Dim TmpArr As String()
        Dim DefArr As String()
        Dim i As Integer
        Dim x As Integer

        TmpArr = Split(pLinea, separatore)

        For i = 0 To UBound(TmpArr)
            If i = 0 Then
                ReDim DefArr(0)
            Else
                ReDim Preserve DefArr(UBound(DefArr) + 1)
            End If
            If Left(TmpArr(i), 1) = Chr(34) And Right(TmpArr(i), 1) = Chr(34) Then
                TmpArr(i) = Mid(TmpArr(i), 2, Len(TmpArr(i)) - 2)
            End If

            TmpArr(i) = TmpArr(i).Replace("""""", """")
            DefArr(UBound(DefArr)) = TmpArr(i)
        Next

		'Nel caso si tratti dell'importazione delle risorse aggiungo una colonna relativa all'Id del ruolo
		If Session("TipoImport") = "riso" Then
			ReDim Preserve DefArr(UBound(DefArr) + 1)
		End If
		'Nel caso si tratti dell'importazione delle sede aggiungo tre colonne per i fla di controllo

		If Session("TipoImport") = "sedi" Then
			ReDim Preserve DefArr(UBound(DefArr) + 3)
		End If
		CreaArray = DefArr

    End Function

	Function RestituisciIDTitoloGiuridico(ByVal DescrizioneAbbreviata As String) As Integer
		Dim dtrLocal As SqlClient.SqlDataReader
		Dim strSql As String

		strSql = "Select IdTitoloGiuridico from TitoliGiuridici where DescrizioneAbbreviata='" & Replace(DescrizioneAbbreviata, "'", "''") & "'"

		dtrLocal = ClsServer.CreaDatareader(strSql, Session("conn"))

		If dtrLocal.HasRows = True Then
			dtrLocal.Read()
			RestituisciIDTitoloGiuridico = dtrLocal("IdTitoloGiuridico")
		End If

		dtrLocal.Close()
		dtrLocal = Nothing

		Return RestituisciIDTitoloGiuridico

	End Function

	'ok
	Private Sub ScriviTabTemp(ByVal pArray() As String, ByVal AlboEnte As String)
		'--- scrive nella tab temporanea
		Dim cmdTemp As SqlClient.SqlCommand
		Dim strsql As String

		Try
			Select Case Session("TipoImport")
				Case "riso"
					strsql = "INSERT INTO #IMP_RISORSE " &
							 "(Ruolo, " &
							 "CodiceFiscale, " &
							 "Cognome, " &
							 "Nome, " &
							 "Data, " &
							 "CodiceIstat, " &
							 "EsperienzaServizioCivile, " &
							 "Corso, " &
							 "IdRuolo " &
							 ") " &
							 "values " &
							 "('" & Trim(ClsServer.NoApice(pArray(0))) & "', " &
							 "'" & Trim(ClsServer.NoApice(pArray(1))) & "', " &
							 "'" & Trim(ClsServer.NoApice(pArray(2))) & "', " &
							 "'" & Trim(ClsServer.NoApice(pArray(3))) & "', " &
							 "convert(datetime,'" & Trim(pArray(4)) & "',103), " &
							 "'" & Trim(ClsServer.NoApice(pArray(5))) & "', " &
							  "'" & Trim(ClsServer.NoApice(pArray(6))) & "', " &
							   "'" & Trim(ClsServer.NoApice(pArray(7))) & "', " &
							 "'" & pArray(8) & "') "
				Case "sedi"
					strsql = "INSERT INTO #IMP_SEDI " &
							 "(CodiceFiscaleSede, " &
							 "Denominazione, " &
							 "CodiceIstatComune, " &
							 "CittaEstera, " &
							 "Indirizzo, " &
							 "Civico, " &
							 "CAP, " &
							 "PrefissoTel, " &
							 "Telefono, " &
							 "Palazzina, " &
							 "Scala, " &
							 "Piano, " &
							 "Interno, " &
							 "IdTitoloGiuridico, " &
							 "UserNameUltimaModifica, " &
							 "NMaxVolontari, " &
							 "FlagMaxVolontari, " &
							 "UsernameMaxVolontari, " &
							 "Normativa81, " &
							 "Conformita, " &
							 "Soggettoestero, " &
							 "Dichiarazionesoggettoestero, " &
							 "Flgalertnomesede, " &
							 "Flgalertindirizzogoogle, " &
							 "Flgalertindirizzo " &
							 ") " &
							 "values " &
							 "('" & zeriCF(Trim(ClsServer.NoApice(pArray(0)))) & "', " &
							 "'" & Trim(ClsServer.NoApice(pArray(1))) & "', " &
							 "'" & Trim(ClsServer.NoApice(pArray(2))) & "', " &
							 "'" & Trim(ClsServer.NoApice(pArray(3))) & "', " &
							 "'" & Trim(ClsServer.NoApice(pArray(4))) & "', " &
							 "'" & Trim(ClsServer.NoApice(pArray(5))) & "', " &
							 "'" & Trim(ClsServer.NoApice(pArray(6))) & "', " &
							 "'" & Trim(ClsServer.NoApice(pArray(7))) & "', " &
							 "'" & Trim(ClsServer.NoApice(pArray(8))) & "', " &
							 "'" & Trim(ClsServer.NoApice(pArray(9))) & "', " &
							 "'ND', 0,'ND'," &
							 "'" & RestituisciIDTitoloGiuridico(Trim(ClsServer.NoApice(pArray(10)))) & "', " &
							"'" & Session("Utente") & "', " &
							"" & Trim(ClsServer.NoApice(pArray(11))) & ", "

					Select Case UCase(Trim(ClsServer.NoApice(pArray(12))))
						Case "SI"
							strsql = strsql & "1, "
						Case "NO"
							strsql = strsql & "0, "
					End Select

					strsql = strsql & "'" & Session("Utente") & "',  "
					If UCase(Trim(ClsServer.NoApice(pArray(13)))) = "SI" Then
						strsql = strsql & "1, "
					Else
						strsql = strsql & "0, "
					End If
					If UCase(Trim(ClsServer.NoApice(pArray(14)))) = "ATT" Then
						strsql = strsql & "1, "
					Else
						strsql = strsql & "0, "
					End If

					strsql = strsql & "'" & Trim(ClsServer.NoApice(pArray(15))) & "', "

					If UCase(Trim(ClsServer.NoApice(pArray(16)))) = "ATT" Then
						strsql = strsql & "1, "
					Else
						strsql = strsql & "0, "
					End If

					strsql = strsql & Trim(ClsServer.NoApice(pArray(17))) & ", " &
					Trim(ClsServer.NoApice(pArray(18))) & ", " &
					Trim(ClsServer.NoApice(pArray(19)))
					'"'" & Trim(ClsServer.NoApice(pArray(20))) & "', " &
					'"'" & Trim(ClsServer.NoApice(pArray(21))) & "', " &
					'"'" & Trim(ClsServer.NoApice(pArray(22))) & "' " &
					strsql = strsql & ")"

				Case "enti"
					strsql = "INSERT INTO #IMP_ENTI " &
					   "(Denominazione, " &
					   "CodiceFiscale, " &
					   "Tipologia, " &
					   "AltroTipoEnte, " &
					   "TipoRelazione, " &
					   "CodiceIstatComune, " &
					   "Indirizzo, " &
					   "Civico, " &
					   "CAP, " &
					   "PrefissoTel, " &
					   "Telefono, " &
					   "Assistenza, " &
					   "ProtezioneCivile, " &
					   "Ambiente, " &
					   "PatrimonioArtistico, " &
					   "PromozioneCulturale, " &
					   "Estero, " &
					   "Agricoltura " &
					",[Data Nomina Rappresentante Legale]" &
					",[Codice Fiscale Rappresentante Legale]" &
					",[Attivita negli ultimi tre anni]" &
					",[Attivita per Fini Istituzionali]" &
					",[Attivita senza scopo di Lucro]" &
					",[Ultimo Anno Assistenza]" &
					",[Esperienza Ultimo Anno Assistenza]" &
					",[Penultimo Anno Assistenza]" &
					",[Esperienza Penultimo Anno Assistenza]" &
					",[Terzultimo Anno Assistenza]" &
					",[Esperienza Terzultimo Anno Assistenza]" &
					",[Aree d'Intervento Assistenza]" &
					",[Ultimo Anno Protezione Civile]" &
					",[Esperienza Ultimo Anno Protezione Civile]" &
					",[Penultimo Anno Protezione Civile]" &
					",[Esperienza Penultimo Anno Protezione Civile]" &
					",[Terzultimo Anno Protezione Civile]" &
					",[Esperienza Terzultimo Anno Protezione Civile]" &
					",[Aree d'Intervento Protezione Civile]" &
					",[Ultimo Anno Ambiente]" &
					",[Esperienza Ultimo Anno Ambiente]" &
					",[Penultimo Anno Ambiente]" &
					",[Esperienza Penultimo Anno Ambiente]" &
					",[Terzultimo Anno Ambiente]" &
					",[Esperienza Terzultimo Anno Ambiente]" &
					",[Aree d'Intervento Ambiente]" &
					",[Ultimo Anno Patrimonio Artistico]" &
					",[Esperienza Ultimo Anno Patrimonio Artistico]" &
					",[Penultimo Anno Patrimonio Artistico]" &
					",[Esperienza Penultimo Anno Patrimonio Artistico]" &
					",[Terzultimo Anno Patrimonio Artistico]" &
					",[Esperienza Terzultimo Anno Patrimonio Artistico]" &
					",[Aree d'Intervento Patrimonio Artistico]" &
					",[Ultimo Anno Promozione Culturale]" &
					",[Esperienza Ultimo Anno Promozione Culturale]" &
					",[Penultimo Anno Promozione Culturale]" &
					",[Esperienza Penultimo Anno Promozione Culturale]" &
					",[Terzultimo Anno Promozione Culturale]" &
					",[Esperienza Terzultimo Anno Promozione Culturale]" &
					",[Aree d'Intervento Promozione Culturale]" &
					",[Ultimo Anno Estero]" &
					",[Esperienza Ultimo Anno Estero]" &
					",[Penultimo Anno Estero]" &
					",[Esperienza Penultimo Anno Estero]" &
					",[Terzultimo Anno Estero]" &
					",[Esperienza Terzultimo Anno Estero]" &
					",[Aree d'Intervento Estero]" &
					",[Ultimo Anno Agricoltura]" &
					",[Esperienza Ultimo Anno Agricoltura]" &
					",[Penultimo Anno Agricoltura]" &
					",[Esperienza Penultimo Anno Agricoltura]" &
					",[Terzultimo Anno Agricoltura]" &
					",[Esperienza Terzultimo Anno Agricoltura]" &
					",[Aree d'Intervento Agricoltura]" &
					   ") " &
					   "values " &
					   "('" & Trim(ClsServer.NoApice(pArray(0))) & "', " &
					   "'" & zeriCF(Trim(ClsServer.NoApice(pArray(1)))) & "', " &
					   "'" & Trim(ClsServer.NoApice(pArray(2))) & "', " &
					   "'" & Trim(ClsServer.NoApice(pArray(3))) & "', " &
					   "'" & Trim(ClsServer.NoApice(pArray(4))) & "', " &
					   "'" & Trim(ClsServer.NoApice(pArray(5))) & "', " &
					   "'" & Trim(ClsServer.NoApice(pArray(6))) & "', " &
					   "'" & Trim(ClsServer.NoApice(pArray(7))) & "', " &
					   "'" & Trim(ClsServer.NoApice(pArray(8))) & "', " &
					   "'" & Trim(ClsServer.NoApice(pArray(9))) & "', " &
					   "'" & Trim(ClsServer.NoApice(pArray(10))) & "', "

					Select Case UCase(Trim(ClsServer.NoApice(pArray(11))))
						Case "SI"
							strsql = strsql & "1, "
						Case "NO"
							strsql = strsql & "0, "
					End Select
					Select Case UCase(Trim(ClsServer.NoApice(pArray(12))))
						Case "SI"
							strsql = strsql & "1, "
						Case "NO"
							strsql = strsql & "0, "
					End Select
					Select Case UCase(Trim(ClsServer.NoApice(pArray(13))))
						Case "SI"
							strsql = strsql & "1, "
						Case "NO"
							strsql = strsql & "0, "
					End Select
					Select Case UCase(Trim(ClsServer.NoApice(pArray(14))))
						Case "SI"
							strsql = strsql & "1, "
						Case "NO"
							strsql = strsql & "0, "
					End Select
					Select Case UCase(Trim(ClsServer.NoApice(pArray(15))))
						Case "SI"
							strsql = strsql & "1, "
						Case "NO"
							strsql = strsql & "0, "
					End Select
					Select Case UCase(Trim(ClsServer.NoApice(pArray(16))))
						Case "SI"
							strsql = strsql & "1,"
						Case "NO"
							strsql = strsql & "0, "
					End Select
					If AlboEnte = "SCN" Then
                        strsql = strsql & "0 "
                    Else
						Select Case UCase(Trim(ClsServer.NoApice(pArray(17))))
							Case "SI"
								strsql = strsql & "1 "
							Case "NO"
								strsql = strsql & "0 "
						End Select
					End If
					strsql += ",'" + Trim(ClsServer.NoApice(pArray(18))) + "'"      'data nomina rl
					strsql += ",'" + Trim(ClsServer.NoApice(pArray(19))) + "'"      'codice fiscale rl
					Select Case UCase(Trim(ClsServer.NoApice(pArray(20))))
						Case "SI"
							strsql = strsql & ",1"
						Case "NO"
							strsql = strsql & ",0 "
					End Select
					Select Case UCase(Trim(ClsServer.NoApice(pArray(21))))
						Case "SI"
							strsql = strsql & ",1"
						Case "NO"
							strsql = strsql & ",0"
					End Select
					Select Case UCase(Trim(ClsServer.NoApice(pArray(22))))  'Attivita senza scopo di Lucro se il campo non è compilato si mette 0 default (i controlli sono stati fatti prima)
						Case "SI"
							strsql = strsql & ",1"
						Case Else
							strsql = strsql & ",0"
					End Select
					For i As Integer = 23 To 71
						strsql += ",'" + Trim(ClsServer.NoApice(pArray(i))) + "'"
					Next
					strsql = strsql & ")"

			End Select

			cmdTemp = New SqlClient.SqlCommand
			cmdTemp.CommandText = strsql
			cmdTemp.Connection = Session("conn")
			cmdTemp.ExecuteNonQuery()
			cmdTemp.Dispose()
			TotOk = TotOk + 1

        Catch exc As Exception
            Log.Error(LogEvent.ENTI_ERRORE_IMPORTAZIONE_MASSIVA, "Errore nella creazione della tabella temporanea", exc)
            strNote = "Errore generico."
            TotKo = TotKo + 1

		End Try
	End Sub

#Region "Controlli e funzioni per le RISORSE"
	Private Function CongruenzaCodiceFiscale(ByVal pCodiceFiscale As String, ByVal pCognome As String, ByVal pNome As String, ByVal pDataNascita As String, ByVal pSesso As String, ByVal pComune As String) As Boolean
		'--- verifica la coerenza tra codfis e cognome, nome, data nascita) As Boolean
		'--- verifica la coerenza tra codfis e cognome, nome, data nascita

		Dim TutteLeLettere As String = "ABCDEFGHIJKLMNOPQRSTUVWXYZ"
		Dim TuttiINumeri As String = "0123456789"
		Dim TuttiGliOmocodici As String = "LMNPQRSTUV"
		Dim TutteLeVocali As String = "AEIOU"
		Dim TutteLeConsonanti As String = "BCDFGHJKLMNPQRSTVWXYZ"
		Dim CodMese As String = "ABCDEHLMPRST"
		Dim swErr As Boolean = False
		Dim Vocali As String
		Dim Consonanti As String
		Dim xCodCognome As String
		Dim xCodNome As String
		Dim tmpGiornoNascitaM As Integer
		Dim tmpGiornoNascitaF As Integer
		Dim tmpValore As String
		Dim i As Integer
		CongruenzaCodiceFiscale = True
		pCodiceFiscale = UCase(pCodiceFiscale)
		If Len(pCodiceFiscale) <> 16 Then
			CongruenzaCodiceFiscale = False
			Exit Function
		End If

		'--- cognome e nome stringa
		For i = 1 To 6
			If InStr(TutteLeLettere, Mid(pCodiceFiscale, i, 1)) = -1 Then
				'CongruenzaCodiceFiscale = "La sezione Cognome Nome non è nel formato corretto."
				CongruenzaCodiceFiscale = False
				Exit Function
			End If
		Next i

		'--- anno numerico
		For i = 7 To 8
			If InStr(TuttiINumeri, Mid(pCodiceFiscale, i, 1)) = -1 Then
				If InStr(TuttiGliOmocodici, Mid(pCodiceFiscale, i, 1)) = -1 Then
					'CongruenzaCodiceFiscale = "La sezione Anno non è nel formato corretto."
					CongruenzaCodiceFiscale = False
					Exit Function
				End If
			End If
		Next i

		'--- mese stringa
		For i = 9 To 9
			If InStr(TutteLeLettere, Mid(pCodiceFiscale, i, 1)) = -1 Then
				'CongruenzaCodiceFiscale = "La sezione Mese non è nel formato corretto."
				CongruenzaCodiceFiscale = False
				Exit Function
			End If
		Next i

		'--- giorno numerico
		For i = 10 To 11
			If InStr(TuttiINumeri, Mid(pCodiceFiscale, i, 1)) = -1 Then
				If InStr(TuttiGliOmocodici, Mid(pCodiceFiscale, i, 1)) = -1 Then
					'CongruenzaCodiceFiscale = "La sezione Giorno non è nel formato corretto."
					CongruenzaCodiceFiscale = False
					Exit Function
				End If
			End If
		Next i

		'--- primo carattere comune stringa
		For i = 12 To 12
			If InStr(TutteLeLettere, Mid(pCodiceFiscale, i, 1)) = -1 Then
				'CongruenzaCodiceFiscale = "La sezione Comune non è nel formato corretto."
				CongruenzaCodiceFiscale = False
				Exit Function
			End If
		Next i

		'--- 3 caratteri comune numerico
		For i = 13 To 15
			If InStr(TuttiINumeri, Mid(pCodiceFiscale, i, 1)) = -1 Then
				If InStr(TuttiGliOmocodici, Mid(pCodiceFiscale, i, 1)) = -1 Then
					'CongruenzaCodiceFiscale = "La sezione Comune  non è nel formato corretto."
					CongruenzaCodiceFiscale = False
					Exit Function
				End If
			End If
		Next i

		'--- ultimo carattere di controllo stringa
		For i = 16 To 16
			If InStr(TutteLeLettere, Mid(pCodiceFiscale, i, 1)) = -1 Then
				'CongruenzaCodiceFiscale = "La sezione Carattere di Controllo non è nel formato corretto."
				CongruenzaCodiceFiscale = False
				Exit Function
			End If
		Next i

		'--- FINE CONTROLLO FORMALE
		'--- Controllo Cognome
		If pCognome = vbNullString Then

		End If
		pCognome = UCase(pCognome)
		For i = 1 To Len(pCognome)
			If InStr(TutteLeVocali, Mid(pCognome, i, 1)) > 0 Then
				Vocali = Vocali + Mid(pCognome, i, 1)
			Else
				If InStr(TutteLeConsonanti, Mid(pCognome, i, 1)) > 0 Then
					Consonanti = Consonanti + Mid(pCognome, i, 1)
				End If
			End If
			If Len(Consonanti) = 3 Then
				Exit For
			End If
		Next i
		If Len(Consonanti) < 3 Then
			Consonanti = Consonanti + Mid(Vocali, 1, 3 - Len(Consonanti))
			For i = Len(Consonanti) + 1 To 3
				Consonanti = Consonanti & "X"
			Next i
		End If
		xCodCognome = Consonanti

		If xCodCognome <> Mid(pCodiceFiscale, 1, 3) Then
			'errore sul cognome
			'CongruenzaCodiceFiscale = "La sezione Cognome non è congruente."
			CongruenzaCodiceFiscale = False
			Exit Function
		End If

		'--- Controllo Nome
		Consonanti = vbNullString
		Vocali = vbNullString
		pNome = UCase(pNome)
		For i = 1 To Len(pNome)
			If InStr(TutteLeVocali, Mid(pNome, i, 1)) > 0 Then
				Vocali = Vocali + Mid(pNome, i, 1)
			Else
				If InStr(TutteLeConsonanti, Mid(pNome, i, 1)) > 0 Then
					Consonanti = Consonanti + Mid(pNome, i, 1)
				End If
			End If
		Next i

		If Len(Consonanti) >= 4 Then
			Consonanti = Mid(Consonanti, 1, 1) + Mid(Consonanti, 3, 2)
		Else
			If Len(Consonanti) < 3 Then
				Consonanti = Consonanti + Mid(Vocali, 1, 3 - Len(Consonanti))
				For i = Len(Consonanti) + 1 To 3
					Consonanti = Consonanti & "X"
				Next i
			End If
		End If
		xCodNome = Consonanti

		If xCodNome <> Mid(pCodiceFiscale, 4, 3) Then
			'CongruenzaCodiceFiscale = "La sezione Nome non è congruente."
			CongruenzaCodiceFiscale = False
			Exit Function
		End If

		'--- Controllo Anno	
		tmpValore = DecodificaOmocodici(Mid(pCodiceFiscale, 7, 1)) & DecodificaOmocodici(Mid(pCodiceFiscale, 8, 1))
		If IsNumeric(tmpValore) = True Then
			If tmpValore <> Mid(pDataNascita, 9, 2) Then
				'CongruenzaCodiceFiscale = "La sezione Anno non è congruente."
				CongruenzaCodiceFiscale = False
				Exit Function
			End If
		Else
			CongruenzaCodiceFiscale = False
			Exit Function
		End If
		'--- Controllo Mese				
		If Mid(pCodiceFiscale, 9, 1) <> Mid(CodMese, Mid(pDataNascita, 4, 2), 1) Then
			'CongruenzaCodiceFiscale = "La sezione Mese non è congruente."
			CongruenzaCodiceFiscale = False
			Exit Function
		End If


		'--- Controllo Giorno
		tmpGiornoNascitaF = Mid(pDataNascita, 1, 2) + 40
		tmpGiornoNascitaM = Mid(pDataNascita, 1, 2)

		tmpValore = DecodificaOmocodici(Mid(pCodiceFiscale, 10, 1)) + DecodificaOmocodici(Mid(pCodiceFiscale, 11, 1))
		If UCase(Trim(pSesso)) = "F" Then
			If IsNumeric(tmpValore) = True Then
				If CInt(tmpValore) <> tmpGiornoNascitaF Then
					'CongruenzaCodiceFiscale = "La sezione Giorno non è congruente."
					CongruenzaCodiceFiscale = False
					Exit Function
				End If
			Else
				CongruenzaCodiceFiscale = False
				Exit Function
			End If
		Else
			If IsNumeric(tmpValore) = True Then
				If CInt(tmpValore) <> tmpGiornoNascitaM Then
					'CongruenzaCodiceFiscale = "La sezione Giorno non è congruente."
					CongruenzaCodiceFiscale = False
					Exit Function
				End If
			Else
				CongruenzaCodiceFiscale = False
				Exit Function
			End If
		End If

		'manca controllo comune istat
		' pComune
	End Function

	Private Function DecodificaOmocodici(ByVal pValore As String) As String
		Dim TuttiGliOmocodici As String = "LMNPQRSTUV"

		If InStr(TuttiGliOmocodici, pValore) > 0 Then

			Select Case pValore
				Case Is = "L"
					DecodificaOmocodici = "0"

				Case "M"
					DecodificaOmocodici = "1"

				Case "N"
					DecodificaOmocodici = "2"

				Case "P"
					DecodificaOmocodici = "3"

				Case "Q"
					DecodificaOmocodici = "4"

				Case "R"
					DecodificaOmocodici = "5"

				Case "S"
					DecodificaOmocodici = "6"

				Case "T"
					DecodificaOmocodici = "7"

				Case "U"
					DecodificaOmocodici = "8"

				Case "V"
					DecodificaOmocodici = "9"

			End Select
		Else
			DecodificaOmocodici = pValore
		End If

	End Function

	'Ritorna TRUE se esiste già il codice fiscale
	Private Function UnivocitaCodiceFiscale(ByVal pCodiceFiscale As String, ByVal pDescrRuolo As String) As Boolean
		Dim dtrCodiceFiscale As Data.SqlClient.SqlDataReader

		strSql = "  SELECT entepersonale.IDEntePersonale FROM entepersonale INNER JOIN entepersonaleruoli ON entepersonale.IDEntePersonale = entepersonaleruoli.IDEntePersonale " &
				 " INNER JOIN ruoli ON entepersonaleruoli.IDRuolo = ruoli.IDRuolo " &
				 " WHERE entepersonale.CodiceFiscale = '" & pCodiceFiscale & "' AND ruoli.DescrAbb = '" & pDescrRuolo & "' AND " &
				 " entepersonale.IdEnte = " & Session("IdEnte") & " " '& _
		'" UNION SELECT entepersonale.IDEntePersonale FROM #IMP_RISORSE As a  " & _
		'" INNER JOIN entepersonale on  A.CodiceFiscale = entepersonale.CodiceFiscale  " & _
		'" INNER JOIN Ruoli C ON a.Ruolo = C.DescrAbb " & _
		'" WHERE a.CodiceFiscale = '" & pCodiceFiscale & "'"

		dtrCodiceFiscale = ClsServer.CreaDatareader(strSql, Session("conn"))

		UnivocitaCodiceFiscale = dtrCodiceFiscale.HasRows

		dtrCodiceFiscale.Close()

		strSql = "  SELECT count(*) Conta  FROM #IMP_RISORSE " &
				 " WHERE CodiceFiscale = '" & pCodiceFiscale & "' AND ruolo = '" & pDescrRuolo & "' "

		dtrCodiceFiscale = ClsServer.CreaDatareader(strSql, Session("conn"))

		dtrCodiceFiscale.Read()

		If dtrCodiceFiscale("Conta") > 0 Then
			UnivocitaCodiceFiscale = True
		End If

		dtrCodiceFiscale.Close()

	End Function

	'Ritorna TRUE se il comune esiste
	Private Function VerificaComune(ByVal pCodiceISTAT As String) As Boolean
		Dim dtrComuni As SqlClient.SqlDataReader

		strSql = "Select IdComune from Comuni " &
		"where CodiceISTAT= '" & ClsServer.NoApice(pCodiceISTAT) & "'"
		dtrComuni = ClsServer.CreaDatareader(strSql, Session("conn"))
		VerificaComune = dtrComuni.HasRows

		dtrComuni.Close()
		dtrComuni = Nothing

	End Function

	Private Function VerificaComuneNascita(ByVal pCodiceISTAT As String) As Boolean
		'***********************
		'CREATA DA SIMONA CORDELLA IL 10/03/2017
		'FUNZIONE CHE RENDE VISIBILI I COMUNI DISMESSI SOLAMENTE PER IL CARICAMENTO DEI COMUNI DI NASCITA
		'***********************

		Dim dtrComuni As SqlClient.SqlDataReader
		Dim strSql As String

		strSql = " Select IdComune from Comuni " &
				 " where (CodiceISTAT= '" & ClsServer.NoApice(pCodiceISTAT) & "' OR CodiceIstatDismesso='" & ClsServer.NoApice(pCodiceISTAT) & "')"
		dtrComuni = ClsServer.CreaDatareader(strSql, Session("conn"))
		VerificaComuneNascita = dtrComuni.HasRows

		dtrComuni.Close()
		dtrComuni = Nothing

	End Function

	'Ritorna TRUE se il comune è nazionale
	Private Function VerificaNazioneComune(ByVal pCodiceISTAT As String) As Boolean
		Dim dtrComuni As SqlClient.SqlDataReader

		strSql = "Select ComuneNazionale from Comuni " &
		"where CodiceISTAT= '" & ClsServer.NoApice(pCodiceISTAT) & "'"
		dtrComuni = ClsServer.CreaDatareader(strSql, Session("conn"))

		If dtrComuni.HasRows = True Then
			dtrComuni.Read()
			VerificaNazioneComune = dtrComuni("ComuneNazionale")
		Else
			VerificaNazioneComune = False
		End If

		dtrComuni.Close()
		dtrComuni = Nothing

		Return VerificaNazioneComune

	End Function

	'Funzione per il controllo della correttezza della descrizione abbreviata del ruolo (se la descrizione è giusta ritorna TRUE)
	Private Function VerificaRuolo(ByRef pRuolo As String()) As Boolean
		Dim dtrRuolo As Data.SqlClient.SqlDataReader

		Dim AlboEnte As String
		AlboEnte = ClsUtility.TrovaAlboEnte(Session("IdEnte"), Session("conn"))

		strSql = "SELECT IdRuolo FROM ruoli " &
				 "WHERE DescrAbb = '" & ClsServer.NoApice(pRuolo(0)) & "' and Import = 1"
		If AlboEnte = "" Then
			strSql &= " AND (ALBO='SCU' OR ALBO IS NULL) "
		Else
			strSql &= " AND (ALBO='" & AlboEnte & "' OR ALBO IS NULL)"
		End If

		dtrRuolo = ClsServer.CreaDatareader(strSql, Session("conn"))

		If dtrRuolo.HasRows = True Then
			dtrRuolo.Read()
			pRuolo(8) = dtrRuolo.Item("IdRuolo")
			VerificaRuolo = True
		Else
			pRuolo(8) = "0"
			VerificaRuolo = False
		End If
		dtrRuolo.Close()
	End Function

#End Region

	'ok
#Region "Controlli e funzioni per le SEDI"
	'Ritorna TRUE se esiste già il codice fiscale e l'ente e nello stato giusto
	Private Function EsistenzaCodiceFiscaleEnti(ByVal pCodiceFiscale As String) As Boolean
		Dim dtrCodiceFiscale As Data.SqlClient.SqlDataReader
		Dim strsql As String

		strsql = "SELECT IDEnte FROM Enti " &
				 "INNER JOIN statienti ON statienti.IDStatoEnte = enti.IDStatoEnte " &
				 "WHERE CodiceFiscale = '" & zeriCF(pCodiceFiscale) & "' " &
				 "AND ((statienti.Istruttoria = 1 OR statienti.PresentazioneProgetti = 1) " &
				 "OR (statienti.Istruttoria = 0 AND statienti.PresentazioneProgetti = 0 " &
				 "AND statienti.Sospeso = 0 AND statienti.Chiuso = 0 AND statienti.DefaultStato = 0))"

		dtrCodiceFiscale = ClsServer.CreaDatareader(strsql, Session("conn"))

		EsistenzaCodiceFiscaleEnti = dtrCodiceFiscale.HasRows

		dtrCodiceFiscale.Close()

	End Function

	'Ritorna TRUE se l'ente selezonato è il padre o un ente figlio con accordo attivo
	Private Function CongruenzaCodiceFiscaleEnti(ByVal pCodiceFiscale As String) As Boolean
		Dim dtrCodiceFiscale As Data.SqlClient.SqlDataReader
		Dim strsql As String

		strsql = "SELECT Enti.IdEnte FROM Enti Where Enti.IdEnte = " & Session("IdEnte") & " And Enti.CodiceFiscale = '" & zeriCF(pCodiceFiscale) & "' " &
				 "UNION " &
				 "SELECT Enti.IdEnte FROM EntiRelazioni INNER JOIN Enti ON EntiRelazioni.IdEnteFiglio = Enti.IdEnte AND EntiRelazioni.DataFineValidità IS NULL " &
				 "WHERE EntiRelazioni.IdEntePadre = " & Session("IdEnte") & " AND Enti.CodiceFiscale = '" & zeriCF(pCodiceFiscale) & "'"
		dtrCodiceFiscale = ClsServer.CreaDatareader(strsql, Session("conn"))
		CongruenzaCodiceFiscaleEnti = dtrCodiceFiscale.HasRows
		dtrCodiceFiscale.Close()
	End Function

	'Controllo univocità riga (ritorna TRUE se la riga è univoca)
	Private Function CheckUnviRiga(ByVal myCF As String, ByVal myDenom As String, ByVal myDenomAttuaz As String, ByVal myCodIstat As String, ByVal myIndirizzo As String, ByVal myCivico As String) As Boolean
		Dim DtrCheckUnivRiga As SqlClient.SqlDataReader
		strSql = "SELECT enti.CodiceFiscale FROM enti INNER JOIN entisedi ON enti.IDEnte = entisedi.IDEnte " &
				 "INNER JOIN entisediattuazioni ON entisedi.IDEnteSede = entisediattuazioni.IDEnteSede " &
				 "INNER JOIN comuni ON entisedi.IDComune = comuni.IDComune " &
				 "WHERE enti.CodiceFiscale = '" & Replace(myCF, "'", "''") & "' AND entisedi.Denominazione = '" & Replace(myDenom, "'", "''") & "' " &
				 "AND entisediattuazioni.Denominazione = '" & Replace(myDenomAttuaz, "'", "''") & "' AND comuni.CodiceISTAT = '" & Replace(myCodIstat, "'", "''") & "' " &
				 "AND entisedi.Indirizzo = '" & Replace(myIndirizzo, "'", "''") & "' AND entisedi.Civico = '" & Replace(myCivico, "'", "''") & "' " &
				 "UNION SELECT CodiceFiscaleSede FROM #IMP_SEDI As a " &
				 "WHERE a.CodiceFiscaleSede = '" & Replace(myCF, "'", "''") & "' AND a.Denominazione = '" & Replace(myDenom, "'", "''") & "' " &
				 "AND a.Dettaglio = '" & Replace(myDenomAttuaz, "'", "''") & "' AND a.CodiceIstatComune = '" & Replace(myCodIstat, "'", "''") & "' " &
				 "AND a.Indirizzo = '" & Replace(myIndirizzo, "'", "''") & "' AND a.Civico = '" & Replace(myCivico, "'", "''") & "' "
		DtrCheckUnivRiga = ClsServer.CreaDatareader(strSql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
		If DtrCheckUnivRiga.HasRows = True Then
			DtrCheckUnivRiga.Close()
			DtrCheckUnivRiga = Nothing
			Return False
		Else
			DtrCheckUnivRiga.Close()
			DtrCheckUnivRiga = Nothing
			Return True
		End If
	End Function

	'Controllo univocità riga (ritorna TRUE se la riga è univoca)
	Private Function CheckDoppiIndirizzi(ByVal myCF As String, ByVal myCodIstat As String, ByVal myIndirizzo As String, ByVal myCivico As String, ByVal myPalazzina As String, ByVal myScala As String, ByVal myPiano As String, ByVal myInterno As String) As Boolean

		Dim DtrCheckUnivRiga As SqlClient.SqlDataReader

		If myPiano = "" Then myPiano = "999"

		'strSql = "SELECT enti.CodiceFiscale FROM enti INNER JOIN entisediattuazioni ON enti.IDEnte = entisediattuazioni.IDEnteCapoFila " &
		'		 "INNER JOIN entisedi ON entisedi.IDEnteSede = entisediattuazioni.IDEnteSede " &
		'		 "INNER JOIN comuni ON entisedi.IDComune = comuni.IDComune " &
		'		 "WHERE entisediattuazioni.idstatoentesede in (1,4) and enti.IdEnte = '" & Session("IdEnte") & "' " &
		'		 "AND comuni.CodiceISTAT = '" & Replace(myCodIstat, "'", "''") & "' " &
		'		 "AND entisedi.Indirizzo = '" & Replace(myIndirizzo, "'", "''") & "' AND isnull(entisedi.Civico,'') = '" & Replace(myCivico, "'", "''") & "' " &
		'		 "AND isnull(entisedi.Palazzina,'') = '" & Replace(myPalazzina, "'", "''") & "' AND isnull(entisedi.Scala,'') = '" & Replace(myScala, "'", "''") & "' " &
		'		 "AND isnull(entisedi.Piano,999) = " & Replace(myPiano, "'", "''") & " AND isnull(entisedi.Interno,'') = '" & Replace(myInterno, "'", "''") & "' " &
		'		 "UNION SELECT CodiceFiscaleSede FROM #IMP_SEDI As a " &
		'		 "WHERE a.CodiceIstatComune = '" & Replace(myCodIstat, "'", "''") & "' " &
		'		 "AND a.Indirizzo = '" & Replace(myIndirizzo, "'", "''") & "' AND a.Civico = '" & Replace(myCivico, "'", "''") & "' " &
		'		 "AND a.Palazzina = '" & Replace(myPalazzina, "'", "''") & "' AND a.Scala = '" & Replace(myScala, "'", "''") & "' " &
		'		 "AND a.Piano = " & Replace(myPiano, "'", "''") & " AND a.Interno = '" & Replace(myInterno, "'", "''") & "' "

		'****** Stefano Rossetti tolto il controllo per scala piano interno
		strSql = "SELECT enti.CodiceFiscale FROM enti INNER JOIN entisediattuazioni ON enti.IDEnte = entisediattuazioni.IDEnteCapoFila " &
				 "INNER JOIN entisedi ON entisedi.IDEnteSede = entisediattuazioni.IDEnteSede " &
				 "INNER JOIN comuni ON entisedi.IDComune = comuni.IDComune " &
				 "WHERE entisediattuazioni.idstatoentesede in (1,4) and enti.IdEnte = '" & Session("IdEnte") & "' " &
				 "AND comuni.CodiceISTAT = '" & Replace(myCodIstat, "'", "''") & "' " &
				 "AND entisedi.Indirizzo = '" & Replace(myIndirizzo, "'", "''") & "' AND isnull(entisedi.Civico,'') = '" & Replace(myCivico, "'", "''") & "' " &
				 "AND isnull(entisedi.Palazzina,'') = '" & Replace(myPalazzina, "'", "''") & "' " &
				 "UNION SELECT CodiceFiscaleSede FROM #IMP_SEDI As a " &
				 "WHERE a.CodiceIstatComune = '" & Replace(myCodIstat, "'", "''") & "' " &
				 "AND a.Indirizzo = '" & Replace(myIndirizzo, "'", "''") & "' AND a.Civico = '" & Replace(myCivico, "'", "''") & "' " &
				 "AND a.Palazzina = '" & Replace(myPalazzina, "'", "''") & "'"

		DtrCheckUnivRiga = ClsServer.CreaDatareader(strSql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
		If DtrCheckUnivRiga.HasRows = True Then
			DtrCheckUnivRiga.Close()
			DtrCheckUnivRiga = Nothing
			Return False
		Else
			DtrCheckUnivRiga.Close()
			DtrCheckUnivRiga = Nothing
			Return True
		End If
	End Function

	'Controllo univocità riga (ritorna TRUE se la riga è univoca)
	Private Function CheckDoppiaDenominazione(ByVal myCF As String, ByVal myDenom As String, ByVal strAlbo As String) As Boolean
		Dim DtrCheckUnivRiga As SqlClient.SqlDataReader

		' 1. controllare se il cf è del padre/figlio ricavo id SCU VISTA su DB and 

		strSql = "SELECT DISTINCT enti.CodiceFiscale FROM enti INNER JOIN entisedi ON enti.IDEnte = entisedi.IDEnte " &
				 "INNER JOIN entisediattuazioni ON entisedi.IDEnteSede = entisediattuazioni.IDEnteSede " &
				 "INNER JOIN comuni ON entisedi.IDComune = comuni.IDComune " &
				 "WHERE entisediattuazioni.idstatoentesede in (1,4) and enti.CodiceFiscale = '" & Replace(myCF, "'", "''") & "'  AND entisedi.Denominazione = '" & Replace(myDenom, "'", "''") & "' AND ENTI.ALBO = '" & strAlbo & "'" &
				 "UNION SELECT CodiceFiscaleSede FROM #IMP_SEDI As a " &
				 "WHERE a.CodiceFiscaleSede = '" & Replace(myCF, "'", "''") & "' AND a.Denominazione = '" & Replace(myDenom, "'", "''") & "' "
		DtrCheckUnivRiga = ClsServer.CreaDatareader(strSql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
		If DtrCheckUnivRiga.HasRows = True Then
			DtrCheckUnivRiga.Close()
			DtrCheckUnivRiga = Nothing
			Return False
		Else
			DtrCheckUnivRiga.Close()
			DtrCheckUnivRiga = Nothing
			Return True
		End If
	End Function


	'Controllo esistenza CAP (ritorna TRUE se esiste)
	Private Function CheckEsistenzaCAP(ByVal CodiceIstat As Integer, ByVal CAP As Integer) As Boolean
		Dim DtrCheckCAP As SqlClient.SqlDataReader
		strSql = "SELECT VW_CAPCOMUNI.CAP, VW_CAPCOMUNI.codiceistat FROM VW_CAPCOMUNI WHERE CAP=" & CAP & " AND codiceistat=" & CodiceIstat

		DtrCheckCAP = ClsServer.CreaDatareader(strSql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))

		CheckEsistenzaCAP = DtrCheckCAP.HasRows

		DtrCheckCAP.Close()
		DtrCheckCAP = Nothing

		Return CheckEsistenzaCAP

	End Function

#End Region

	'ok
#Region "Controlli e funzioni per gli ENTI"
	'Ritorna TRUE se esiste già il codice fiscale
	Private Function UnivocitaCodiceFiscaleEnti(ByVal pCodiceFiscale As String) As Boolean
		Dim dtrCodiceFiscale As Data.SqlClient.SqlDataReader
		Dim strsql As String

		strsql = "SELECT IDEnte FROM Enti WHERE CodiceFiscale ='" & zeriCF(pCodiceFiscale) & "'"

		dtrCodiceFiscale = ClsServer.CreaDatareader(strsql, Session("conn"))

		UnivocitaCodiceFiscaleEnti = dtrCodiceFiscale.HasRows

		dtrCodiceFiscale.Close()

	End Function

	'Funzione per ripristinare gli zeri in testa al codice fiscale
	Private Function zeriCF(ByVal myCF As String) As String
		Dim strNewCF As String = ""
		Dim IntX As Int16
		For IntX = 1 To 11 - myCF.Length
			strNewCF &= "0"
		Next
		strNewCF &= myCF
		Return strNewCF
	End Function

	'Funzione per il controllo dell'uuniviocità della denominazione dell'Ente (ritorna TRUE se la denominazione non esiste)
	Private Function DenUnivEnte(ByVal myDen As String, ByVal strAlbo As String) As Boolean
		strSql = "SELECT enti.Denominazione FROM entirelazioni " &
				 "INNER JOIN enti ON entirelazioni.IDEnteFiglio = enti.IDEnte " &
				 "WHERE ENTI.IDSTATOENTE <> 7 AND entirelazioni.IDEntePadre = " & Session("IdEnte") & "  " &
				 " AND enti.Denominazione = '" & ClsServer.NoApice(myDen).Trim & "' AND ENTI.ALBO = '" & strAlbo & "' " &
				 "UNION SELECT Denominazione FROM #IMP_ENTI WHERE Denominazione = '" & ClsServer.NoApice(myDen).Trim & "'"
		dtrGenreico = ClsServer.CreaDatareader(strSql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
		If dtrGenreico.HasRows = True Then
			dtrGenreico.Close()
			dtrGenreico = Nothing
			Return False
		Else
			dtrGenreico.Close()
			dtrGenreico = Nothing
			Return True
		End If
	End Function

	'Funzione per il controllo dell'uuniviocità della denominazione dell'Ente (ritorna TRUE se la denominazione non esiste)
	Private Function DenUnivCodFis(ByVal myCodiceFiscale As String, ByVal strAlbo As String) As Boolean
		strSql = " SELECT enti.CodiceFiscale FROM enti " &
				 " WHERE enti.CodiceFiscale = '" & zeriCF(ClsServer.NoApice(myCodiceFiscale).Trim) & "' " &
				 " AND ENTI.ALBO = '" & strAlbo & "' " &
				 " UNION SELECT CodiceFiscale FROM #IMP_ENTI WHERE CodiceFiscale = '" & zeriCF(ClsServer.NoApice(myCodiceFiscale).Trim) & "'"
		dtrGenreico = ClsServer.CreaDatareader(strSql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
		If dtrGenreico.HasRows = True Then
			dtrGenreico.Close()
			dtrGenreico = Nothing
			Return False
		Else
			dtrGenreico.Close()
			dtrGenreico = Nothing
			Return True
		End If
	End Function

	'Funzione per il controlle sull'esistenza della tipologia (abbreviata) (Ritorna TRUE se la tipologia esiste)
	Private Function CheckEsistTipEnte(ByVal myTip As String, ByVal AlboEnte As String) As Boolean
        strSql = "SELECT CodiceImport FROM TipologieEnti WHERE CodiceImport ='" & Trim(myTip) & "' and iscrizione = 1"
		If AlboEnte = "" Then
			strSql &= " AND (ALBO='SCU' OR ALBO IS NULL) "
		Else
			strSql &= " AND (ALBO='" & AlboEnte & "' OR ALBO IS NULL) "
		End If

		dtrGenreico = ClsServer.CreaDatareader(strSql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
		If dtrGenreico.HasRows = True Then
			dtrGenreico.Close()
			dtrGenreico = Nothing
			Return True
		Else
			dtrGenreico.Close()
			dtrGenreico = Nothing
			Return False
		End If
	End Function

	'Funzione per controllo della compatibilità tra le tipologie dell'ente padre e dell'ent figlio(Ritorna TRUE se le tipologie sono compatibili)
	Private Function CheckTipEnte(ByVal myTip As String) As Boolean
		strSql = "SELECT TipologieEnti.Statale FROM TipologieEnti WHERE TipologieEnti.CodiceImport = '" & myTip & "'"
		dtrGenreico = ClsServer.CreaDatareader(strSql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
		dtrGenreico.Read()
		If dtrGenreico.Item("Statale") = True Then
			dtrGenreico.Close()
			dtrGenreico = Nothing
			strSql = "SELECT TipologieEnti.Statale FROM enti " &
							 "INNER JOIN TipologieEnti ON enti.Tipologia = TipologieEnti.Descrizione " &
							 "WHERE enti.IDEnte = " & Session("IdEnte") & " AND TipologieEnti.CodiceImport='" & myTip & "'"
			dtrGenreico = ClsServer.CreaDatareader(strSql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
			If dtrGenreico.HasRows = True Then
				dtrGenreico.Close()
				dtrGenreico = Nothing
				Return True
			Else
				dtrGenreico.Close()
				dtrGenreico = Nothing
				Return False
			End If
		Else
			dtrGenreico.Close()
			dtrGenreico = Nothing
			Return True
		End If

	End Function

	'Funzione per il controlle della compatibilità tra la relazione richiesta è la classe dell'Ente padre(ritorna TRUE se c'è compatibilità)
	Private Function ClsRicEnteTipRel(ByVal myRel As String) As Boolean
		If UCase(myRel) = "PAR" Then
			strSql = "SELECT IdClasseAccreditamentoRichiesta as IdRic FROM enti WHERE IDEnte =" & Session("IdEnte")
			dtrGenreico = ClsServer.CreaDatareader(strSql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
			dtrGenreico.Read()
			If CInt(dtrGenreico.Item("IdRic")) < 3 Then
				dtrGenreico.Close()
				dtrGenreico = Nothing
				Return True
			Else
				dtrGenreico.Close()
				dtrGenreico = Nothing
				Return False
			End If
		Else
			Return True
		End If
	End Function
#End Region

	'Classe Utilizzata per il controllo dei ruoli
	Public Class ControlloRuoli
		Dim MyRuoli As New RuoliCollection

		Public Property Ruoli() As RuoliCollection
			Get
				Return MyRuoli
			End Get
			Set(ByVal Value As RuoliCollection)
				MyRuoli = Value
			End Set
		End Property

		Public Class RuoliCollection
			Private MyRuoliCollection As New System.Collections.ArrayList

			'Proprietà che restituisce un singolo orario selezionato
			Default Public Property Item(ByVal Index As Integer) As Ruolo
				Get
					Return CType(MyRuoliCollection(Index), Ruolo)
				End Get
				Set(ByVal Value As Ruolo)
					MyRuoliCollection(Index) = Value
				End Set
			End Property

			'Rutine per l'inserimento di un nuovo orario
			Sub Add(ByVal CodiceFiscale As String, ByVal IdRuolo As String, ByVal Note As String)
				Dim MyRuolo As New Ruolo
				MyRuolo.CodiceFiscale = CodiceFiscale
				MyRuolo.IdRuolo = IdRuolo
				MyRuolo.Note = Note
				MyRuoliCollection.Add(MyRuolo)
				MyRuolo = Nothing
			End Sub

			'Proprità che Indica il numero di Orari presenti nell'array
			Public ReadOnly Property Count()
				Get
					Return MyRuoliCollection.Count
				End Get
			End Property

			'Routine per l'eliminazione di un Indirizzo
			Sub RemoveAt(ByVal Index As Integer)
				MyRuoliCollection.RemoveAt(Index)
			End Sub

			'Routine per la cancellazione di tutti gli elementi dall'array
			Sub Clear()
				MyRuoliCollection.Clear()
			End Sub

			'Unisce tutte le persone con la stesso CodiceFiscale
			Sub AccorpaRisorse()
				Dim IntY As Integer = 0
				Do While IntY <= Me.Count - 1
					Dim IntX As Integer
					For IntX = Me.Count - 1 To 0 Step -1
						If Me.Item(IntY).CodiceFiscale = Me.Item(IntX).CodiceFiscale And IntY <> IntX Then
							Me.Item(IntY).IdRuolo = Me.Item(IntY).IdRuolo & ";" & Me.Item(IntX).IdRuolo
							'Eliminazione la riga
							Me.RemoveAt(IntX)
						End If
					Next
					IntY = IntY + 1
				Loop
			End Sub

			'Rimuove le risorse che hanno un ruolo errato (Ruolo = 0)
			Sub RimuoviErrati()
				Dim IntX As Integer
				For IntX = Me.Count - 1 To 0 Step -1
					If Me.Item(IntX).IdRuolo = "0" Or Me.Item(IntX).CodiceFiscale.Trim = "" Then
						Me.RemoveAt(IntX)
					End If
				Next
			End Sub
		End Class

		Class Ruolo
			Private MyCodiceFiscale As String
			Private MyIdRuolo As String
			Private MyNote As String

			'Codice Fiscale
			Public Property CodiceFiscale() As String
				Get
					Return MyCodiceFiscale
				End Get
				Set(ByVal Value As String)
					MyCodiceFiscale = Value
				End Set
			End Property

			'Ruoli
			Public Property IdRuolo() As String
				Get
					Return MyIdRuolo
				End Get
				Set(ByVal Value As String)
					MyIdRuolo = Value
				End Set
			End Property

			'Note
			Public Property Note() As String
				Get
					Return MyNote
				End Get
				Set(ByVal Value As String)
					MyNote = Value
				End Set
			End Property
		End Class

	End Class

	Private Sub CancellaTabellaTemp()
		Dim strSql As String
		Dim cmdCanTempTable As SqlClient.SqlCommand
		Try
			Select Case Session("TipoImport")
				Case "riso"
					strSql = "DROP TABLE #IMP_RISORSE"
				Case "sedi"
					strSql = "DROP TABLE #IMP_SEDI"
				Case "enti"
					strSql = "DROP TABLE #IMP_ENTI"
			End Select

			cmdCanTempTable = New SqlClient.SqlCommand
			cmdCanTempTable.CommandText = strSql
			cmdCanTempTable.Connection = Session("conn")
			cmdCanTempTable.ExecuteNonQuery()
		Catch e As Exception
		End Try

		cmdCanTempTable.Dispose()
	End Sub

	Private Sub btnCaricaCV_Click(sender As Object, e As EventArgs) Handles btnCaricaCV.Click
        'If FileCV.PostedFile Is Nothing OrElse FileCV.PostedFile.ContentType <> "application/x-zip-compressed" Then
        '	txtErroreCV.Text = "Formato file Non valido. Deve essere un formato Zip"
        '	Exit Sub
        'End If
		txtErroreCV.Text = ""
		Dim UniqueName = Guid.NewGuid.ToString()
		Dim FilezipPath = Server.MapPath("reports\export\") & UniqueName & ".zip"
		FileCV.PostedFile.SaveAs(FilezipPath)
		Dim zip As ZipFile
		Try
			zip = New ZipFile(FilezipPath)
		Catch ex As Exception
			Log.Error(LogEvent.RISORSE_ENTE_ERRORE_IMPORTAZIONE_CV)
			txtErroreCV.Text = "Formato file Non valido. Zip non valido"
			File.Delete(FilezipPath)
			Exit Sub
		End Try
		Dim filesDaElaborare As New List(Of Allegato)
		Dim filesConErrore As New Dictionary(Of String, String)
		Dim risorseEnte As New Dictionary(Of String, Risorsa)

        Dim sqlRisorseEnte = "SELECT DISTINCT P.CodiceFiscale,A.IdEnteDocumento, A.HashValue,P.IdEntePersonale FROM entepersonale P LEFT JOIN  EntiDocumenti A ON A.IdEnteDocumento = P.IdAllegatoCV inner join entepersonaleruoli epr on P.IDEntePersonale = epr.IDEntePersonale inner join ruoli r on epr.IDRuolo = r.IDRuolo WHERE p.DataFineValidità IS NULL and epr.DataFineValidità is null and R.Import = 1 and R.DescrAbb is not null and R.RuoloAccreditamento = 1 and isnull(r.albo,'SCU') = 'SCU' AND P.IdEnte=@IdEnte"
        'Dim sqlRisorseEnte = "SELECT P.CodiceFiscale,A.IdAllegato, A.HashValue,P.IdEntePersonale FROM entepersonale P LEFT JOIN  Allegato A ON A.IdAllegato = P.IdAllegatoCV WHERE DataFineValidità IS NULL AND P.IdEnte=@IdEnte"
        Dim cmdRisorseEnte As New SqlCommand(sqlRisorseEnte, Session("conn"))
        cmdRisorseEnte.Parameters.AddWithValue("@IdEnte", Session("IdEnte"))
        Dim readerRisorseEnte = cmdRisorseEnte.ExecuteReader()
        Try
            While readerRisorseEnte.Read
                risorseEnte.Add(readerRisorseEnte("CodiceFiscale"), New Risorsa() With {
                .CodiceFiscale = IIf(IsDBNull(readerRisorseEnte("CodiceFiscale")) = True, Nothing, readerRisorseEnte("CodiceFiscale")),
                .HashCurriculum = IIf(IsDBNull(readerRisorseEnte("HashValue")) = True, Nothing, readerRisorseEnte("HashValue")),
                .IdAllegato = IIf(IsDBNull(readerRisorseEnte("IdEnteDocumento")) = True, Nothing, readerRisorseEnte("IdEnteDocumento")),
                .IdEntePersonale = readerRisorseEnte("IdEntePersonale")
               })
            End While
        Catch ex As Exception
            Log.Error(LogEvent.RISORSE_ENTE_ERRORE_IMPORTAZIONE_CV, "Caricamento dati", ex)
            txtErroreCV.Text = "Errore nel recupero delle informazioni."
            File.Delete(FilezipPath)
            Exit Sub
        Finally
            readerRisorseEnte.Close()
        End Try

        For Each file As ZipEntry In zip
            Dim NomeFile As String = file.FileName
            If UCase(Right(NomeFile, 4)) <> ".PDF" And UCase(Right(NomeFile, 8)) <> ".PDF.P7M" Then
                filesConErrore.Add(NomeFile, "Formato File non corretto")
                Continue For
            End If
            Dim NomeCurriculum As String = NomeFile.Split(".")(0)
            If UCase(Left(NomeCurriculum, 3)) <> "CV_" Then
                filesConErrore.Add(NomeFile, "Nome File non corretto")
                Continue For
            End If
            Dim CodiceFiscale As String = NomeCurriculum.Split("_")(1)
            If Not risorseEnte.ContainsKey(CodiceFiscale) Then
                filesConErrore.Add(NomeFile, "Codice Fiscale non presente tra le risorse")
                Continue For
            End If
            Dim blob As Byte()
            Dim ms As New MemoryStream
            file.Extract(ms)
            blob = ms.ToArray
            Dim hash As String = GeneraHash(blob)
            Dim fileUguale As String = Nothing
            For Each item As Allegato In filesDaElaborare
                If item.Hash = hash Then
                    fileUguale = item.Filename
                    Exit For
                End If
            Next
            'Aggiorno virtualmente l'hash
            risorseEnte(CodiceFiscale).HashCurriculum = hash
            If fileUguale IsNot Nothing AndAlso Not filesConErrore.ContainsKey(NomeFile) Then
                filesConErrore.Add(NomeFile, "file identico a """ & fileUguale & """")
                Continue For
            End If
            Dim nuovoAllegato = New Allegato With {
             .Blob = blob,
             .Filename = NomeFile,
             .Hash = hash,
             .LinkId = risorseEnte(CodiceFiscale).IdEntePersonale
            }
            If risorseEnte(CodiceFiscale) IsNot Nothing AndAlso risorseEnte(CodiceFiscale).IdAllegato.HasValue Then
                nuovoAllegato.Id = risorseEnte(CodiceFiscale).IdAllegato
            End If
            filesDaElaborare.Add(nuovoAllegato)

        Next
        zip.Dispose()
        File.Delete(FilezipPath)
        'Verifica duplicazione codici Hash
        For Each allegato As Allegato In filesDaElaborare
            For Each risorsa In risorseEnte
                If allegato.Hash = risorsa.Value.HashCurriculum And risorsa.Value.IdEntePersonale <> allegato.LinkId AndAlso Not filesConErrore.ContainsKey(allegato.Filename) Then
                    filesConErrore.Add(allegato.Filename, "File duplicato per la risorsa """ & risorsa.Key & """")
                End If
            Next
        Next

        If filesConErrore.Count = 0 Then

            Dim _cE As New clsEnte
            Dim strIdEntefase As String = _cE.RicavaIdEnteFase(Session("IdEnte"), Session("Conn"))

            Dim tran As SqlTransaction = Session("conn").BeginTransaction(Session("IdEnte") & "_" & Session("Utente"))
            Try
                For Each allegato As Allegato In filesDaElaborare
                    'Verifico se il CV deve essere aggiornato o inserito
                    If allegato.Id.HasValue Then
                        'Modifica
                        If CercaAllegato(allegato, strIdEntefase) Then
                            AggiornaAllegato(allegato, strIdEntefase)
                        Else
                            'Creazione Allegato
                            Dim idAllegato As Integer = SalvaAllegato(allegato, TipoFile.CURRICULUM_RISORSA, strIdEntefase, tran)
                            Dim sqlUpdatePersonale = "UPDATE entepersonale SET IdAllegatoCV=@IdAllegatoCV WHERE IdEntePersonale=@IdEntePersonale"
                            Dim cmdUpdatePersonale As New SqlCommand(sqlUpdatePersonale, Session("conn"), tran)
                            cmdUpdatePersonale.Parameters.AddWithValue("@IdAllegatoCV", idAllegato)
                            cmdUpdatePersonale.Parameters.AddWithValue("@IdEntePersonale", allegato.LinkId)
                            cmdUpdatePersonale.ExecuteNonQuery()
                        End If
                    Else
                        'Insert
                        'Creazione Allegato
                        Dim idAllegato As Integer = SalvaAllegato(allegato, TipoFile.CURRICULUM_RISORSA, strIdEntefase, tran)
                        Dim sqlUpdatePersonale = "UPDATE entepersonale SET IdAllegatoCV=@IdAllegatoCV WHERE IdEntePersonale=@IdEntePersonale"
                        Dim cmdUpdatePersonale As New SqlCommand(sqlUpdatePersonale, Session("conn"), tran)
                        cmdUpdatePersonale.Parameters.AddWithValue("@IdAllegatoCV", idAllegato)
                        cmdUpdatePersonale.Parameters.AddWithValue("@IdEntePersonale", allegato.LinkId)
                        cmdUpdatePersonale.ExecuteNonQuery()
                    End If
                Next
                tran.Commit()
                lblElaborazioneCV.Text = "Elaborazione effettuata correttamente. Sono stati importati " & filesDaElaborare.Count & " Curricula"
                lblElaborazioneCV.CssClass = String.Empty
                lstErroriCV.Text = String.Empty
                modalCVResult.Show()
                Log.Information(LogEvent.RISORSE_ENTE_IMPORTAZIONE_CV)
            Catch Ex As Exception
                tran.Rollback()
                Log.Error(LogEvent.RISORSE_ENTE_ERRORE_IMPORTAZIONE_CV, "Scrittura Database", Ex)
                txtErroreCV.Text = "Errore nel caricamento del file ZIP."
                File.Delete(FilezipPath)
                Exit Sub
            End Try
        Else

            Dim htmlListErrore As New StringBuilder("<table class=""table"" cellspacing=""0"" rules=""all"" border=""1"" style=""width: 100%;""><tr><th>Nome File</th><th>Problema</th></tr>")
            lblElaborazioneCV.Text = "Si sono verificati i seguenti errori nell'importazione dei Curricula Vitae"
            lblElaborazioneCV.CssClass = "msgErrore"
            For Each errore In filesConErrore
                htmlListErrore.Append("<tr class=""tr"" align=""center""><td>" & errore.Key & "</td><td>" & errore.Value & "</td></tr>")
            Next
            htmlListErrore.Append("</table>")
            lstErroriCV.Text = htmlListErrore.ToString()
            Log.Warning(LogEvent.RISORSE_ENTE_ERRORE_IMPORTAZIONE_CV, parameters:=filesConErrore)
            modalCVResult.Show()

        End If



    End Sub

	Private Class Risorsa
		Private _CodiceFiscale As String
		Public Property CodiceFiscale() As String
			Get
				Return _CodiceFiscale
			End Get
			Set(ByVal value As String)
				_CodiceFiscale = value
			End Set
		End Property
		Private _HashCurriculum As String
		Public Property HashCurriculum() As String
			Get
				Return _HashCurriculum
			End Get
			Set(ByVal value As String)
				_HashCurriculum = value
			End Set
		End Property
		Private _IdAllegato As Integer?
		Public Property IdAllegato() As Integer?
			Get
				Return _IdAllegato
			End Get
			Set(ByVal value As Integer?)
				_IdAllegato = value
			End Set
		End Property
		Private _IdPersonaleEnte As Integer
		Public Property IdEntePersonale() As Integer
			Get
				Return _IdPersonaleEnte
			End Get
			Set(ByVal value As Integer)
				_IdPersonaleEnte = value
			End Set
		End Property
	End Class
	Private Function GeneraHash(FileinByte() As Byte) As String
		Dim tmpHash() As Byte

		tmpHash = New MD5CryptoServiceProvider().ComputeHash(FileinByte)

		GeneraHash = ByteArrayToString(tmpHash)
		Return GeneraHash
	End Function
	Private Function ByteArrayToString(arrInput() As Byte) As String
		Dim i As Integer
		Dim sOutput As New StringBuilder(arrInput.Length)
		For i = 0 To arrInput.Length - 1
			sOutput.Append(arrInput(i).ToString("X2"))
		Next
		Return sOutput.ToString()
	End Function


	Private Class EntePerDocumentiAllegati
        Private _CodiceFiscale As String
        Public Property CodiceFiscale() As String
            Get
                Return _CodiceFiscale
            End Get
            Set(ByVal value As String)
                _CodiceFiscale = value
            End Set
        End Property
        Private _HashDocumentiAllegati As String
        Public Property HashDocumentiAllegati() As String
            Get
                Return _HashDocumentiAllegati
            End Get
            Set(ByVal value As String)
                _HashDocumentiAllegati = value
            End Set
        End Property
        Private _IdAllegato As Integer?
        Public Property IdAllegato() As Integer?
            Get
                Return _IdAllegato
            End Get
            Set(ByVal value As Integer?)
                _IdAllegato = value
            End Set
        End Property
        Private _IdEnte As Integer
        Public Property IdEnte() As Integer
            Get
                Return _IdEnte
            End Get
            Set(ByVal value As Integer)
                _IdEnte = value
            End Set
        End Property
        Private _IsEnteTitolare As Boolean
        Public Property IsEnteTitolare() As Boolean
            Get
                Return _IsEnteTitolare
            End Get
            Set(ByVal value As Boolean)
                _IsEnteTitolare = value
            End Set
        End Property
        Private _IsPrivato As Boolean
        Public Property IsPrivato() As Boolean
            Get
                Return _IsPrivato
            End Get
            Set(ByVal value As Boolean)
                _IsPrivato = value
            End Set
        End Property
    End Class

    '_tipoDocumentoEnte corrisponde al value della ddlDocumentiEnte
    Sub ElaboraFileDocumentiAllegati(ByVal tipoDocumentoEnte As Integer)

        'definizione dati variabili
        Dim nomeCampoAllegato As String
        Dim logMessaggioErrori As Integer
        Dim logMessaggio As Integer
        Dim prefissoFile As String
        Dim tipoAllegato As Integer
        Dim prefissoMessaggio As String
        Dim oggettoMessaggio As String

        Select Case tipoDocumentoEnte
            Case 1
                nomeCampoAllegato = "IdAllegatoAttoCostitutivo"
                logMessaggioErrori = LogEvent.ENTI_ERRORE_IMPORTAZIONE_ATTICOSTITUTIVI
                logMessaggio = LogEvent.ENTI_IMPORTAZIONE_ATTICOSTITUTIVI
                prefissoFile = "ATTOCOSTITUTIVO_"
                tipoAllegato = TipoFile.ATTO_COSTITUTIVO_ENTE
                prefissoMessaggio = " stati importati "
                oggettoMessaggio = " Atti Costitutivi"
            Case 2
                nomeCampoAllegato = "IdAllegatoStatuto"
                logMessaggioErrori = LogEvent.ENTI_ERRORE_IMPORTAZIONE_STATUTI
                logMessaggio = LogEvent.ENTI_IMPORTAZIONE_STATUTI
                prefissoFile = "STATUTO_"
                tipoAllegato = TipoFile.STATUTO_ENTE
                prefissoMessaggio = " stati importati "
                oggettoMessaggio = " Statuti"
            Case 3
                nomeCampoAllegato = "IdAllegatoDeliberaAdesione"
                logMessaggioErrori = LogEvent.ENTI_ERRORE_IMPORTAZIONE_DELIBERE
                logMessaggio = LogEvent.ENTI_IMPORTAZIONE_DELIBERE
                prefissoFile = "DELIBERA_"
                tipoAllegato = TipoFile.DELIBERA_COSTITUZIONE_ENTE
                prefissoMessaggio = " state importate "
                oggettoMessaggio = " Delibere"
            Case 4
                nomeCampoAllegato = "IdAllegatoImpegnoEtico"
                logMessaggioErrori = LogEvent.ENTI_ERRORE_IMPORTAZIONE_CARTEIMPEGNOETICO
                logMessaggio = LogEvent.ENTI_ERRORE_IMPORTAZIONE_CARTEIMPEGNOETICO
                prefissoFile = "CARTAIMPEGNOETICO_"
                tipoAllegato = TipoFile.CARTA_IMPEGNO_ETICO
                prefissoMessaggio = " state importate "
                oggettoMessaggio = " Carte per l'Impegno Etico"
            Case Else
                Log.Error(LogEvent.ENTI_ERRORE_IMPORTAZIONE_CARTEIMPEGNOETICO, message:="TipoDocumento " + tipoDocumentoEnte + " non definito")
                txtErroreImportDocumenti.Text = "Errore nel tipo documento"
                CmdChiudi.Focus()   'per scrollare la pagina
                Exit Sub
        End Select


        txtErroreImportDocumenti.Text = ""
        Dim UniqueName = Guid.NewGuid.ToString()
        Dim FilezipPath = Server.MapPath("reports\export\") & UniqueName & ".zip"
        fileDocumenti.PostedFile.SaveAs(FilezipPath)
        Dim zip As ZipFile
        Try
            zip = New ZipFile(FilezipPath)
        Catch ex As Exception
            Log.Error(logMessaggioErrori)
            txtErroreImportDocumenti.Text = "Formato file Non valido. Zip non valido"
            File.Delete(FilezipPath)
            CmdChiudi.Focus()   'per scrollare la pagina
            Exit Sub
        End Try
        Dim filesDaElaborare As New List(Of Allegato)
        Dim filesConErrore As New Dictionary(Of String, String)
        Dim entiDocumentiAllegati As New Dictionary(Of String, EntePerDocumentiAllegati)

        Dim sqlentidocumentiallegati = "select e.IDEnte,e.CodiceFiscale,e.IsEnteTitolare,a.IdAllegato,a.HashValue,t.Privato from"
        sqlentidocumentiallegati += " (select IdEnte,1 as IsEnteTitolare, CodiceFiscale," + nomeCampoAllegato + ",Tipologia"
        sqlentidocumentiallegati += " from enti where IDEnte=@IdEnte union select"
        sqlentidocumentiallegati += " e.IDEnte,0 as IsEnteTitolare, CodiceFiscale," + nomeCampoAllegato + ",Tipologia"
        sqlentidocumentiallegati += " from entirelazioni er inner join enti e on er.IDEnteFiglio=e.IDEnte"
        sqlentidocumentiallegati += " where datafinevalidità is null and er.IDEntePadre=@IdEnte) E"
        sqlentidocumentiallegati += " inner join TipologieEnti T on E.Tipologia=t.Descrizione"
        sqlentidocumentiallegati += " left join Allegato A ON A.IdAllegato = E." + nomeCampoAllegato
        Dim cmdentiDocumentiAllegati As New SqlCommand(sqlentidocumentiallegati, Session("conn"))
        cmdentiDocumentiAllegati.Parameters.AddWithValue("@IdEnte", Session("IdEnte"))
        Dim readerentiDocumentiAllegati = cmdentiDocumentiAllegati.ExecuteReader()
        Try
            While readerentiDocumentiAllegati.Read
                entiDocumentiAllegati.Add(readerentiDocumentiAllegati("CodiceFiscale"), New EntePerDocumentiAllegati() With {
                .CodiceFiscale = If(IsDBNull(readerentiDocumentiAllegati("CodiceFiscale")) = True, Nothing, readerentiDocumentiAllegati("CodiceFiscale")),
                .HashDocumentiAllegati = IIf(IsDBNull(readerentiDocumentiAllegati("HashValue")) = True, Nothing, readerentiDocumentiAllegati("HashValue")),
                .IdAllegato = If(IsDBNull(readerentiDocumentiAllegati("IdAllegato")) = True, Nothing, readerentiDocumentiAllegati("IdAllegato")),
                .IdEnte = readerentiDocumentiAllegati("IdEnte"),
                .IsEnteTitolare = If(readerentiDocumentiAllegati("IsEnteTitolare") = "1", True, False),
                .IsPrivato = If(readerentiDocumentiAllegati("Privato") = "1", True, False)
               })
            End While
        Catch ex As Exception
            Log.Error(logMessaggioErrori, "Caricamento dati", ex)
            txtErroreImportDocumenti.Text = "Errore nel recupero delle informazioni."
            File.Delete(FilezipPath)
            CmdChiudi.Focus()   'per scrollare la pagina
            Exit Sub
        Finally
            readerentiDocumentiAllegati.Close()
        End Try

        For Each file As ZipEntry In zip
            Dim NomeDocumento As String = file.FileName
            If UCase(Right(NomeDocumento, 4)) <> ".PDF" And UCase(Right(NomeDocumento, 8)) <> ".PDF.P7M" Then
                filesConErrore.Add(NomeDocumento, "Formato File non corretto")
                Continue For
            End If
            Dim NomeCarta As String = NomeDocumento.Split(".")(0)
            If UCase(Left(NomeCarta, prefissoFile.Length)) <> prefissoFile Then
                filesConErrore.Add(NomeDocumento, "Nome File non corretto")
                Continue For
            End If
            Dim CodiceFiscale As String = NomeCarta.Split("_")(1)
            If Not entiDocumentiAllegati.ContainsKey(CodiceFiscale) Then
                filesConErrore.Add(NomeDocumento, "Codice Fiscale non presente tra gli enti di accoglienza")
                Continue For
            End If
            If tipoAllegato = TipoFile.DELIBERA_COSTITUZIONE_ENTE AndAlso entiDocumentiAllegati(CodiceFiscale).IsPrivato Then
                filesConErrore.Add(NomeDocumento, "Delibera non prevista per ente """ & CodiceFiscale & """")
                Continue For
            End If
            Dim blob As Byte()
            Dim ms As New MemoryStream
            file.Extract(ms)
            blob = ms.ToArray
            Dim hash As String = GeneraHash(blob)
            Dim fileUguale As String = Nothing
            For Each item As Allegato In filesDaElaborare
                If item.Hash = hash Then
                    fileUguale = item.Filename
                    Exit For
                End If
            Next
            'Aggiorno virtualmente l'hash
            entiDocumentiAllegati(CodiceFiscale).HashDocumentiAllegati = hash
            If fileUguale IsNot Nothing Then
                filesConErrore.Add(NomeDocumento, "file identico a """ & fileUguale & """")
                Continue For
            End If


			'ATTENZIONE!!! TODO!!! Se ente titolare e tipoallegato carta impegno etico (ho messo il campo apposta nella classe) BISOGNA VERIFICARE LA FIRMA DEL FILE!!!!!!!


			'NB!!!  il campo idpersonale dell'allegato viene usato per contenere l'idente (l'allegato è usato anche per importare i cv e uso la stessa classe senza crearne un altra)
			Dim nuovoAllegato = New Allegato With {
			 .Blob = blob,
			 .Filename = NomeDocumento,
			 .Hash = hash,
			 .LinkId = entiDocumentiAllegati(CodiceFiscale).IdEnte
			}
			If entiDocumentiAllegati(CodiceFiscale) IsNot Nothing AndAlso entiDocumentiAllegati(CodiceFiscale).IdAllegato.HasValue Then
				nuovoAllegato.Id = entiDocumentiAllegati(CodiceFiscale).IdAllegato
			End If
            filesDaElaborare.Add(nuovoAllegato)

        Next
        zip.Dispose()
        File.Delete(FilezipPath)
        'Verifica duplicazione codici Hash
        For Each allegato As Allegato In filesDaElaborare
            For Each EntePerDocumentoAllegato In entiDocumentiAllegati
				'NB!!!  il campo LinkId dell'allegato viene usato per contenere l'idente (l'allegato è usato anche per importare i cv e uso la stessa classe senza crearne un altra)
				If allegato.Hash = EntePerDocumentoAllegato.Value.HashDocumentiAllegati And EntePerDocumentoAllegato.Value.IdEnte <> allegato.LinkId Then
					filesConErrore.Add(allegato.Filename, "File duplicato per l'Ente """ & EntePerDocumentoAllegato.Key & """")
				End If
			Next
        Next

        If filesConErrore.Count = 0 Then

            Dim _cE As New clsEnte
            Dim strIdEntefase As String = _cE.RicavaIdEnteFase(Session("IdEnte"), Session("Conn"))

            Dim tran As SqlTransaction = Session("conn").BeginTransaction(Session("IdEnte") & "_" & Session("Utente"))
            Try
				For Each allegato As Allegato In filesDaElaborare
					'Sempre Insert
					'Creazione Allegato
					Dim idAllegato As Integer = SalvaAllegato(allegato, tipoAllegato, strIdEntefase)

					Dim sqlUpdatePersonale = "UPDATE enti SET " + nomeCampoAllegato + "=@IdAllegato WHERE IdEnte=@IdEnte"
					Dim cmdUpdatePersonale As New SqlCommand(sqlUpdatePersonale, Session("conn"), tran)
					cmdUpdatePersonale.Parameters.AddWithValue("@IdAllegato", idAllegato)
					'NB!!!  il campo idpersonale dell'allegato viene usato per contenere l'idente (l'allegato è usato anche per importare i cv e uso la stessa classe senza crearne un altra)
					cmdUpdatePersonale.Parameters.AddWithValue("@IdEnte", allegato.LinkId)
					cmdUpdatePersonale.ExecuteNonQuery()
				Next
				tran.Commit()
                lblElaborazioneDocumenti.Text = "Elaborazione effettuata correttamente. Sono" & prefissoMessaggio & filesDaElaborare.Count & oggettoMessaggio
                lblElaborazioneDocumenti.CssClass = String.Empty
                lstErroriImportazioneDocumentiEnte.Text = String.Empty
                modalImportDocumentiEnteResult.Show()
                Log.Information(logMessaggio)
            Catch Ex As Exception
                tran.Rollback()
                Log.Error(logMessaggioErrori, "Scrittura Database", Ex)
                txtErroreImportDocumenti.Text = "Errore nel caricamento dei documenti."
                CmdChiudi.Focus()   'per scrollare la pagina
                Exit Sub
            End Try
        Else

            Dim htmlListErrore As New StringBuilder("<table class=""table"" cellspacing=""0"" rules=""all"" border=""1"" style=""width: 100%;""><tr><th>Nome File</th><th>Problema</th></tr>")
            lblElaborazioneDocumenti.Text = "Si sono verificati i seguenti errori nell'importazione " & oggettoMessaggio
            lblElaborazioneDocumenti.CssClass = "msgErrore"
            For Each errore In filesConErrore
                htmlListErrore.Append("<tr class=""tr"" align=""center""><td>" & errore.Key & "</td><td>" & errore.Value & "</td></tr>")
            Next
            htmlListErrore.Append("</table>")
            lstErroriImportazioneDocumentiEnte.Text = htmlListErrore.ToString()
            Log.Warning(logMessaggioErrori, parameters:=filesConErrore)
            modalImportDocumentiEnteResult.Show()

        End If

    End Sub

    Protected Sub btnElaboraDocumenti_Click(sender As Object, e As EventArgs) Handles btnElaboraDocumenti.Click
        If ddlDocumentiEnte.SelectedValue = 0 Then
            txtErroreImportDocumenti.Text = "Selezionare il tipo dei documenti da importare"
            CmdChiudi.Focus()   'per scrollare la pagina
            Exit Sub
        End If

        If Not fileDocumenti.HasFile Then
            txtErroreImportDocumenti.Text = "Selezionare il file dei documenti da importare"
            CmdChiudi.Focus()   'per scrollare la pagina
            Exit Sub
        End If

        ElaboraFileDocumentiAllegati(ddlDocumentiEnte.SelectedValue)

    End Sub

	Private Sub ddlDocumentiEnte_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddlDocumentiEnte.SelectedIndexChanged
		divCarteImpegnoEtico.Visible = False
		divDelibera.Visible = False
		divAttoCostitutivo.Visible = False
        divStatuto.Visible = False
        divNessunaSelezione.Visible = False

        txtErroreImportDocumenti.Text = ""

        If ddlDocumentiEnte.SelectedValue = 0 Then
            divNessunaSelezione.Visible = True
            pTipoFile.InnerText = "Non è stato selezionato nessun Tipo Documento"
            CmdChiudi.Focus()   'per scrollare la pagina
        End If
		If ddlDocumentiEnte.SelectedValue = 4 Then
			divCarteImpegnoEtico.Visible = True
			pTipoFile.InnerText = "Elaborazione File Importazione Carte di Impegno Etico"
			CmdChiudi.Focus()   'per scrollare la pagina
		End If

		If ddlDocumentiEnte.SelectedValue = 3 Then
			divDelibera.Visible = True
			pTipoFile.InnerText = "Elaborazione File Importazione Delibere"
			CmdChiudi.Focus()   'per scrollare la pagina
		End If

		If ddlDocumentiEnte.SelectedValue = 2 Then
			divStatuto.Visible = True
			pTipoFile.InnerText = "Elaborazione File Importazione Statuti"
			CmdChiudi.Focus()   'per scrollare la pagina
		End If

		If ddlDocumentiEnte.SelectedValue = 1 Then
			divAttoCostitutivo.Visible = True
			pTipoFile.InnerText = "Elaborazione File Importazione Atti Costitutivi"
			CmdChiudi.Focus()   'per scrollare la pagina
		End If

	End Sub
	Private Function CaricaNomeComune(idIstat As String) As String
		Dim dtrComuni As SqlClient.SqlDataReader

		strSql = "Select Denominazione from Comuni " &
		"where CodiceISTAT= '" & ClsServer.NoApice(idIstat) & "'"
		dtrComuni = ClsServer.CreaDatareader(strSql, Session("conn"))
		'VerificaComune = dtrComuni.HasRows
		If dtrComuni.HasRows = True Then
			dtrComuni.Read()
			CaricaNomeComune = dtrComuni("Denominazione")
		Else
			CaricaNomeComune = ""
		End If
		dtrComuni.Close()
		dtrComuni = Nothing
	End Function
	Private Function CaricaCodiceStato(idIstat As String) As String
		Dim dtrComuni As SqlClient.SqlDataReader

		strSql = "Select Codice from Nazioni " &
		"where Nazione= '" & ClsServer.NoApice(idIstat) & "'"
		dtrComuni = ClsServer.CreaDatareader(strSql, Session("conn"))
		'VerificaComune = dtrComuni.HasRows
		If dtrComuni.HasRows = True Then
			dtrComuni.Read()
			CaricaCodiceStato = dtrComuni("Codice")
		Else
			CaricaCodiceStato = ""
		End If
		dtrComuni.Close()
		dtrComuni = Nothing
	End Function

	Protected Sub btnCaricaLDA_Click(sender As Object, e As EventArgs) Handles btnCaricaLDA.Click
		'If FileLDA.PostedFile Is Nothing OrElse FileLDA.PostedFile.ContentType <> "application/x-zip-compressed" Then
		'	txtErroreLDA.Text = "Formato file Non valido. Deve essere un formato Zip"
		'	Exit Sub
		'End If
		txtErroreLDA.Text = ""
		Dim UniqueName = Guid.NewGuid.ToString()
		Dim FilezipPath = Server.MapPath("reports\export\") & UniqueName & ".zip"
		FileLDA.PostedFile.SaveAs(FilezipPath)
		Dim zip As ZipFile
		Try
			zip = New ZipFile(FilezipPath)
		Catch ex As Exception
			Log.Error(LogEvent.IMPORTAZIONE_SEDI_ERRORE_IMPORTAZIONE_LDA)
			txtErroreLDA.Text = "Formato file Non valido. Zip non valido"
			File.Delete(FilezipPath)
			Exit Sub
		End Try
		Dim filesDaElaborare As New List(Of Allegato)
		Dim filesConErrore As New Dictionary(Of String, String)
		Dim SediEnte As New Dictionary(Of String, SedeEnte)

		'Dim sqlSediEnte = "SELECT S.Denominazione,A.IdAllegato, A.HashValue,S.IdEnteSede FROM entisedi S LEFT JOIN  Allegato A ON A.IdAllegato = S.IdAllegatoLDA WHERE S.DataScadenzaContratto IS NULL AND P.IdEnte=@IdEnte"
		Dim sqlSediEnte = "Select s.IDENTESEDE, s.Denominazione,a.IdAllegato, a.HashValue" &
							" FROM entisediattuazioni c inner join entisedi s on c.identesede = s.identesede " &
							" INNER JOIN enti e ON c.IDENTECAPOFILA = e.IDENTE" &
							" Left JOIN Allegato a ON s.IdAllegatoLSE = a.IdAllegato" &
							" where e.IdEnte=@IdEnte and idTitoloGiuridico=7" &
							" and s.DataScadenzaContratto is null"

		Dim cmdSediEnte As New SqlCommand(sqlSediEnte, Session("conn"))
		cmdSediEnte.Parameters.AddWithValue("@IdEnte", Session("IdEnte"))
		Dim readerSediEnte = cmdSediEnte.ExecuteReader()
		Try
			While readerSediEnte.Read
				SediEnte.Add(Regex.Replace(readerSediEnte("Denominazione"), "[\\ \/ \:  \* \?  \"" <> \|]", String.Empty).ToUpper, New SedeEnte() With {
				.NomeSede = IIf(IsDBNull(readerSediEnte("Denominazione")) = True, Nothing, Regex.Replace(readerSediEnte("Denominazione"), "[\\ \/ \:  \* \?  \"" <> \|]", String.Empty).ToUpper),
				.HashLDA = IIf(IsDBNull(readerSediEnte("HashValue")) = True, Nothing, readerSediEnte("HashValue")),
				.IdAllegato = IIf(IsDBNull(readerSediEnte("IdAllegato")) = True, Nothing, readerSediEnte("IdAllegato")),
				.IdEnteSede = readerSediEnte("IdEnteSede")
			})
			End While
		Catch ex As Exception
			Log.Error(LogEvent.IMPORTAZIONE_SEDI_ERRORE_IMPORTAZIONE_LDA, "Caricamento dati", ex)
			txtErroreLDA.Text = "Errore nel recupero delle informazioni."
			File.Delete(FilezipPath)
			Exit Sub
		Finally
			readerSediEnte.Close()
		End Try

		For Each file As ZipEntry In zip
			Dim NomeFile As String = file.FileName
			If UCase(Right(NomeFile, 4)) <> ".PDF" And UCase(Right(NomeFile, 8)) <> ".PDF.P7M" Then
				filesConErrore.Add(NomeFile, "Formato File non corretto")
				Continue For
			End If
			Dim NomeLDA As String = NomeFile.Split(".")(0)
			If UCase(Left(NomeLDA, 4)) <> "LDA_" Then
				filesConErrore.Add(NomeFile, "Nome File non corretto")
				Continue For
			End If
			Dim NomeSede As String = Regex.Replace(NomeLDA.Split("_")(1), "[\\ \/ \:  \* \?  \"" <> \|]", String.Empty).ToUpper
			If Not SediEnte.ContainsKey(NomeSede) Then
				filesConErrore.Add(NomeFile, "Nome Sede non presente tra le sedi estere con titolo di proriet&agrave; idoneo")
				Continue For
			End If
			Dim blob As Byte()
			Dim ms As New MemoryStream
			file.Extract(ms)
			blob = ms.ToArray
			Dim hash As String = GeneraHash(blob)
			Dim fileUguale As String = Nothing
			For Each item As Allegato In filesDaElaborare
				If item.Hash = hash Then
					fileUguale = item.Filename
					Exit For
				End If
			Next
			'Aggiorno virtualmente l'hash
			SediEnte(NomeSede).HashLDA = hash
			If fileUguale IsNot Nothing Then

				filesConErrore.Add(NomeFile, "file identico a """ & fileUguale & """")
				Continue For
			End If
			Dim nuovoAllegato = New Allegato With {
				.Blob = blob,
				.Filename = NomeFile,
				.Hash = hash,
				.LinkId = SediEnte(NomeSede).IdEnteSede
			}
			If SediEnte(NomeSede) IsNot Nothing AndAlso SediEnte(NomeSede).IdAllegato.HasValue Then
				nuovoAllegato.Id = SediEnte(NomeSede).IdAllegato
			End If
			filesDaElaborare.Add(nuovoAllegato)

		Next
		zip.Dispose()
		File.Delete(FilezipPath)
		'Verifica duplicazione codici Hash
		For Each allegato As Allegato In filesDaElaborare
			For Each Sede In SediEnte
				If allegato.Hash = Sede.Value.HashLDA And Sede.Value.IdEnteSede <> allegato.LinkId Then
					If Not filesConErrore.ContainsKey(allegato.Filename) Then
						filesConErrore.Add(allegato.Filename, "File duplicato già presente per la sede denominata """ & Sede.Key & """")
					End If
				End If
			Next
		Next
        Dim dtrFase As SqlDataReader 'dichiarazione datareader

        Dim _cE As New clsEnte
        Dim strIdEntefase As String = _cE.RicavaIdEnteFase(Session("IdEnte"), Session("Conn"))

		If filesConErrore.Count = 0 Then
			Dim tran As SqlTransaction = Session("conn").BeginTransaction(Session("IdEnte") & "_" & Session("Utente"))
			Try
				For Each allegato As Allegato In filesDaElaborare
					'Verifico se il LDA deve essere aggiornato o inserito
					If allegato.Id.HasValue Then
						'Modifica
						Dim updateLDACommand As New SqlCommand("", Session("conn"), tran)
						AggiornaAllegato(allegato, updateLDACommand, strIdEntefase)
						updateLDACommand.Dispose()
					Else
						'Insert
						'Creazione Allegato
						Dim insertAllegatoCommand As New SqlCommand("", Session("conn"), tran)
						Dim idAllegato As Integer = SalvaAllegato(allegato, TipoFile.LETTERA_SEDE_ESTERA, strIdEntefase, insertAllegatoCommand)
						insertAllegatoCommand.Dispose()
						Dim sqlUpdatePersonale = "UPDATE EntiSedi SET IdAllegatoLSE=@IdAllegatoLSE WHERE IdEnteSede=@IdEntesede"
						Dim cmdUpdatePersonale As New SqlCommand(sqlUpdatePersonale, Session("conn"), tran)
						cmdUpdatePersonale.Parameters.AddWithValue("@IdAllegatoLSE", idAllegato)
						cmdUpdatePersonale.Parameters.AddWithValue("@IdEnteSede", allegato.LinkId)
						cmdUpdatePersonale.ExecuteNonQuery()
						insertAllegatoCommand.Dispose()
					End If
				Next
				tran.Commit()
				lblElaborazioneLDA.Text = "Elaborazione effettuata correttamente. Sono state importate " & filesDaElaborare.Count & " Lettere di Accordo"
				lblElaborazioneLDA.CssClass = String.Empty
				lstErroriLDA.Text = String.Empty
				modalLDAResult.Show()
				Log.Information(LogEvent.IMPORTAZIONE_SEDI_IMPORTAZIONE_LDA)
			Catch Ex As Exception
				tran.Rollback()
				Log.Error(LogEvent.IMPORTAZIONE_SEDI_ERRORE_IMPORTAZIONE_LDA, "Scrittura Database", Ex)
			End Try
		Else

			Dim htmlListErrore As New StringBuilder("<table class=""table"" cellspacing=""0"" rules=""all"" border=""1"" style=""width: 100%;""><tr><th>Nome File</th><th>Problema</th></tr>")
			lblElaborazioneLDA.Text = "Si sono verificati i seguenti errori nell'importazione delle Lettere di Accordo"
			lblElaborazioneLDA.CssClass = "msgErrore"
			For Each errore In filesConErrore
				htmlListErrore.Append("<tr class=""tr"" align=""center""><td>" & errore.Key & "</td><td>" & errore.Value & "</td></tr>")
			Next
			htmlListErrore.Append("</table>")
			lstErroriLDA.Text = htmlListErrore.ToString()
			Log.Warning(LogEvent.IMPORTAZIONE_SEDI_ERRORE_IMPORTAZIONE_LDA, parameters:=filesConErrore)
			modalLDAResult.Show()

		End If

	End Sub
	Private Class SedeEnte
		Private _NomeSede As String
		Public Property NomeSede() As String
			Get
				Return _NomeSede
			End Get
			Set(ByVal value As String)
				_NomeSede = value
			End Set
		End Property
		Private _HashLDA As String
		Public Property HashLDA() As String
			Get
				Return _HashLDA
			End Get
			Set(ByVal value As String)
				_HashLDA = value
			End Set
		End Property
		Private _IdAllegato As Integer?
		Public Property IdAllegato() As Integer?
			Get
				Return _IdAllegato
			End Get
			Set(ByVal value As Integer?)
				_IdAllegato = value
			End Set
		End Property
		Private _IdEnteSede As Integer
		Public Property IdEnteSede() As Integer
			Get
				Return _IdEnteSede
			End Get
			Set(ByVal value As Integer)
				_IdEnteSede = value
			End Set
		End Property
	End Class

    Function IsPrivatoByCodiceImport(ByVal CodiceImport As String) As Boolean
        Dim dtrLocal As SqlClient.SqlDataReader
        Dim strSql As String
        Dim _ret As Boolean = Nothing

        strSql = "select Privato from TipologieEnti where CodiceImport='" & Replace(CodiceImport, "'", "''") & "' and ordinamento is not null"

        dtrLocal = ClsServer.CreaDatareader(strSql, Session("conn"))

        If dtrLocal.HasRows = True Then
            dtrLocal.Read()
            _ret = If(dtrLocal("Privato") = "1", True, False)
        End If

        dtrLocal.Close()
        dtrLocal = Nothing

        Return _ret
    End Function

    Function GetCodificheAreeIntervento(ByVal CodificaSettore As String) As Dictionary(Of String, Integer)
        Dim strSql As String
        Dim _ret As New Dictionary(Of String, Integer)

        strSql = "select ae.Id,mac.Codifica+ae.Codifica as CodificaArea from AreeEsperienza ae inner join macroambitiattività mac on ae.IdSettore=mac.IDMacroAmbitoAttività"
        strSql += " where mac.Codifica='" + CodificaSettore + "' and isnull(ae.DataFineValidita,'20501231')>GETDATE() and isnull(ae.DataInizioValidita,'20000101')<GETDATE() order by ae.DataInizioValidita desc"

        Using dtrLocal As SqlDataReader = ClsServer.CreaDatareader(strSql, Session("conn"))
            With dtrLocal
                While dtrLocal.Read()
                    If Not _ret.ContainsKey(dtrLocal("CodificaArea")) Then _ret.Add(dtrLocal("CodificaArea"), Integer.Parse(dtrLocal("Id")))
                End While
                .Close()
            End With
        End Using

        Return _ret
    End Function

End Class