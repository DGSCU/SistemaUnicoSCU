Imports System.Data.SqlClient
Imports System.Drawing
Imports System.Security.Cryptography
Imports Logger.Data

Public Class WebElencoOlp
    Inherits SmartPage

    Dim query As String
    Dim dataSet As DataSet
    Dim dataReader As SqlClient.SqlDataReader
    Dim sqlCommand As SqlClient.SqlCommand
    Dim ROSSO As Drawing.Color = System.Drawing.ColorTranslator.FromHtml("#FF9966")
    Dim GIALLO As Drawing.Color = System.Drawing.ColorTranslator.FromHtml("#FFFF99")
    Dim VERDE As Drawing.Color = System.Drawing.ColorTranslator.FromHtml("#99FF99")
    Dim NERO As Drawing.Color = System.Drawing.ColorTranslator.FromHtml("#000000")
    Dim BIANCO As Drawing.Color = System.Drawing.ColorTranslator.FromHtml("#FFFFFF")

#Region "Utility"
    Private Sub ChiudiDataReader(ByRef dataReader As SqlDataReader)
        If Not dataReader Is Nothing Then
            dataReader.Close()
            dataReader = Nothing
        End If
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
#End Region
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load
        VerificaSessione()

        Select Case UCase(Request.QueryString("Tiporuolo"))
            Case "RLEA"
                dgRisultatoRicerca.Columns(11).Visible = False
                dgRisultatoRicerca.Columns(18).Visible = True 'ex17
                dgRisultatoRicerca.Columns(23).Visible = False          'AssOLP
                dgRisultatoRicerca.Columns(24).Visible = False          'CancOLP
            Case "TUTOR"
                dgRisultatoRicerca.Columns(11).Visible = False
                'nascondo colonna info (OLP)
                dgRisultatoRicerca.Columns(18).Visible = False 'ex17
                dgRisultatoRicerca.Columns(23).Visible = False          'AssOLP
                dgRisultatoRicerca.Columns(24).Visible = False          'CancOLP
        End Select

        If Request.QueryString("IdAttivita") <> "" Then
            Dim strATTIVITA As Integer = -1
            Dim strBANDOATTIVITA As Integer = -1
            Dim strENTEPERSONALE As Integer = -1
            Dim strENTITA As Integer = -1
            Dim strIDENTE As Integer = -1
            ChiudiDataReader(dataReader)
            If ClsUtility.SICUREZZA_VERIFICA_AUTORIZZAZIONI(Session("conn"), Session("IdEnte"), Session("txtCodEnte"), Request.QueryString("IdAttivita"), strBANDOATTIVITA, strENTEPERSONALE, strENTITA, strIDENTE) = 1 Then
                ChiudiDataReader(dataReader)
            Else
                ChiudiDataReader(dataReader)
                Response.Redirect("wfrmAnomaliaDati.aspx")

            End If


        End If
        If dgRisultatoRicerca.Items.Count = 0 Then
            cmdConferma.Visible = False
        End If
        If IsPostBack = False Then

            Dim SostituzioneOLP_abilitata As Boolean
            Dim identesedeattuazione As Integer = 0
            Dim idattivita As Integer = 0
            Integer.TryParse(Request.QueryString("IdAttivita"), idattivita)
            Integer.TryParse(Request.QueryString("IdSedeAttuazione"), identesedeattuazione)
            SostituzioneOLP_abilitata = SostOlpABilitata(idattivita, identesedeattuazione, Session("TipoUtente"))
            If SostituzioneOLP_abilitata AndAlso UCase(Request.QueryString("Tiporuolo")) = "OLP" Then
                dgRisultatoRicerca.Columns(21).Visible = True 'colonna SostOLP
            Else
                dgRisultatoRicerca.Columns(21).Visible = False 'colonna SostOLP
            End If

            If Not IsNothing(Request.QueryString("Tiporuolo")) Then
                lblRuolo.Text = Request.QueryString("Tiporuolo")
            End If
            'Visualizzazione della Sede e del Progetto

            query = "select attività.idattività,attività.titolo, Entisedi.denominazione " & _
            " from attività " & _
            " inner join attivitàentisediattuazione on(attivitàentisediattuazione.idattività=attività.idattività)" & _
            " inner join entisediattuazioni on " & _
            " (attivitàentisediattuazione.identesedeattuazione=entisediattuazioni.identesedeattuazione) " & _
            " inner join Entisedi on (entisedi.identesede=entisediattuazioni.identesede)" & _
            " where attività.idattività=" & Request.QueryString("IdAttivita") & " and entisediattuazioni.identesedeattuazione=" & Request.QueryString("IdSedeAttuazione") & ""

            If Not dataReader Is Nothing Then
                dataReader.Close()
                dataReader = Nothing
            End If
            dataReader = ClsServer.CreaDatareader(query, Session("conn"))
            dataReader.Read()
            lblSede.Text = dataReader("Denominazione") & " - "
            lblidsedeattuazione.Value = Request.QueryString("IdSedeAttuazione")
            lblProgetto.Text = dataReader("Titolo")
            lblidattEs.Value = Request.QueryString("idattES")
            'Se esiste almeno un OlpAssociato ChekVisualizzaOlp=true
            'Modificata da Alessandra Taballione il 08/07/2005
            query = "Select entepersonale.identePersonale " & _
                    " from entepersonale " & _
                    " inner join entepersonaleruoli on (entepersonaleruoli.identepersonale=entepersonale.identepersonale)" & _
                    " left join associaEntepersonaleRuoliattivitàentisediattuazione a on " & _
                    " (a.idEntepersonaleRuolo=entepersonaleruoli.identepersonaleRuolo)" & _
                    " inner join ruoli on (ruoli.idruolo=entepersonaleruoli.idruolo)" & _
                    " left join attivitàentisediattuazione on (attivitàentisediattuazione.idAttivitàEntesedeAttuazione=a.idAttivitàEntesedeAttuazione) " & _
                    " inner join tipiruoli on (tipiruoli.idTipoRuolo=ruoli.idtiporuolo)" & _
                    " inner join Comuni on (Comuni.idComune=entepersonale.idComuneNascita)" & _
                    " where "
            Select Case UCase(lblRuolo.Text)
                Case "OLP"
                    query = query & " entepersonaleruoli.idruolo=1 "
                Case "RLEA"
                    query = query & " entepersonaleruoli.idruolo=6 "
                Case "TUTOR"
                    query = query & " entepersonaleruoli.idruolo=5 "
            End Select
            'query = query & " and entepersonale.datafinevalidità is null" & _ rimossa il 03/10/2016 per corretta visualizzazione in fase di accesso se olp/rlea cancellati ma associati
            query = query & " and (entepersonaleruoli.Accreditato=1 or entepersonaleruoli.Accreditato=0)and attivitàentisediattuazione.idattività=" & Request.QueryString("IdAttivita") & " and attivitàentisediattuazione.identesedeattuazione=" & lblidsedeattuazione.Value & "" & _
                    " and not a.idassociaEntepersonaleRuoliattivitàentisediattuazione is null"
            ChiudiDataReader(dataReader)
            dataReader = ClsServer.CreaDatareader(query, Session("conn"))
            If dataReader.HasRows = True Then
                chkVisualizzaOlp.Checked = True
            Else
                chkVisualizzaOlp.Checked = False
            End If
            ChiudiDataReader(dataReader)

            Dim Modifica As Integer
            If Session("TipoUtente") = "E" Then
                Modifica = CInt(ClsUtility.LoadProgettiAbilitaModificaEnte(Request.QueryString("IdAttivita"), Session("Conn")))
                If Modifica = "0" Then
                    lblModifica.Value = "False"
                    cmdConferma.Visible = False
                End If
            Else
                Modifica = CInt(Request.QueryString("Modifica"))
            End If

            If Session("TipoUtente") = "R" Then
                query = "select idregionecompetenza from attività where idattività = " & CInt(Request.QueryString("IdAttivita")) & " "

                dataReader = ClsServer.CreaDatareader(query, Session("conn"))
                dataReader.Read()
                If dataReader("idregionecompetenza") = 22 Then
                    'regione che consulta progetto competenza nazionale
                    lblModifica.Value = "False"
                    cmdConferma.Visible = False
                End If
                ChiudiDataReader(dataReader)
            End If

            'Verifico se il Progetto può essere modificato.
            query = "select attività.idattività,statiattività.statoattività,statiattività.defaultstato as defaultAttività," & _
            " case isnull(convert(smallint,statibandiAttività.defaultstato),-1)when -1 then '0'" & _
            " else convert(smallint,statibandiAttività.defaultstato)  end as defaultBando " & _
            " from attività " & _
            " inner join statiattività on (statiattività.idstatoattività=attività.idstatoattività) " & _
            " left join bandiAttività  on (attività.idbandoattività=bandiAttività.idbandoattività) " & _
            " left join statibandiAttività  " & _
            " on (bandiAttività.idstatobandoattività=statibandiAttività.idstatobandoattività) " & _
            " where attività.idattività=" & Request.QueryString("IdAttivita") & ""
            ChiudiDataReader(dataReader)
            dataReader = ClsServer.CreaDatareader(query, Session("conn"))
            dataReader.Read()
            If Modifica = "0" Then
                If dataReader("defaultAttività") = False And dataReader("defaultBando") = False Then
                    lblModifica.Value = "False"
                    cmdConferma.Visible = False
                    lblErrore.Text = "Non è possibile modificare il progetto perchè è attivo o in valutazione."
                End If
            End If
            ChiudiDataReader(dataReader)
            If chkVisualizzaOlp.Checked = True Then
                RicercaOLP(Me.IsPostBack)
            Else
                lblMessaggioInfo.Visible = True
                lblMessaggioInfo.Text = "Premere il pulsante RICERCA per ottenere la lista delle figure professionali disponibili."
            End If

        End If
    End Sub

    Sub CaricaDataGrid(ByRef GridDaCaricare As DataGrid) 'valorizzo la datagrid passata
        GridDaCaricare.DataSource = dataSet
        GridDaCaricare.DataBind()
        'controllaChek()
        ColoraCelle()
        If dgRisultatoRicerca.Items.Count = 0 Then
            If chkVisualizzaOlp.Checked = True Then
                If txtCognome.Text <> "" Or txtNome.Text <> "" Or txtcomuneNascita.Text <> "" Then
                    lblErrore.Text = "Non sono presenti " & lblRuolo.Text & " assegnati al Progetto corrispondenti ai parametri di ricerca selezionati."
                    lblMessaggioInfo.Text = ""
                Else
                    lblErrore.Text = "Non sono presenti " & lblRuolo.Text & " assegnati al Progetto."
                    lblMessaggioInfo.Text = ""
                End If

            Else
                If txtCognome.Text <> "" Or txtNome.Text <> "" Or txtcomuneNascita.Text <> "" Then
                    lblErrore.Text = "Non sono disponibili " & lblRuolo.Text & " da assegnare sul Progetto corrispondenti ai parametri di ricerca selezionati."
                    lblMessaggioInfo.Text = ""
                Else
                    lblErrore.Text = "Non sono disponibili " & lblRuolo.Text & " da assegnare sul Progetto."
                    lblMessaggioInfo.Text = ""
                End If
            End If
            cmdConferma.Visible = False
        Else
            cmdConferma.Visible = False 'pulsante sempre invisibile, cambiato metodo assegnazione (popup)
            lblErrore.Text = String.Empty
        End If
        If lblModifica.Value = "False" Then
            BloccaChek()
        End If
    End Sub

    Private Sub BloccaChek()
        'realizzato da Alessandra Taballione  30/09/04
        Dim item As DataGridItem
        For Each item In dgRisultatoRicerca.Items
            Dim check As CheckBox = DirectCast(item.FindControl("check1"), CheckBox)
            check.Enabled = False
        Next
        For Each item In dgRisultatoRicerca.Items
            Dim check As CheckBox = DirectCast(item.FindControl("chkCorsoOlp"), CheckBox)
            check.Enabled = False
        Next
        For Each item In dgRisultatoRicerca.Items
            Dim check As CheckBox = DirectCast(item.FindControl("chkCorsoOlpNo"), CheckBox)
            check.Enabled = False
        Next
        'ADC 14/01/2021
        For Each item In dgRisultatoRicerca.Items
            Dim check As CheckBox = DirectCast(item.FindControl("ChkCaricamentoCV"), CheckBox)
            check.Enabled = False
        Next
        'MV 03/12/2021 disabilitazione cancellazione OLP, disabilitazione assegna/modifica risorsa
        dgRisultatoRicerca.Columns(24).Visible = False
        dgRisultatoRicerca.Columns(23).Visible = False
    End Sub

    Private Sub RicercaOLP(ByVal mypostback As Boolean)
        lblErrore.Text = String.Empty
        lblmessaggioInfo.Text = String.Empty

        If Session("TipoUtente") = "E" Then
            dgRisultatoRicerca.Columns(16).Visible = False 'Data di Inserimento ex15
        End If

        Dim dtOlp As DataTable = New DataTable
        query = "Select TOP 50 '<IMG width=25 height=20 src=""images/xp1.gif"">' as img,entepersonaleruoli.Accreditato ,entepersonaleruoli.identePersonaleRuolo," & _
                    " case isnull(attivitàentisediattuazione.identesedeattuazione,-1)when -1 then '0'else attivitàentisediattuazione.identesedeattuazione end as identesedeattuazione," & _
                    " case isnull(a.idassociaEntepersonaleRuoliattivitàentisediattuazione,-1)when -1 then 0 else a.idassociaEntepersonaleRuoliattivitàentisediattuazione end as idass," & _
                    " case isnull(a.idassociaEntepersonaleRuoliattivitàentisediattuazione,-1)when -1 then 0 " & _
                    " else 1 end as trovaChec," & _
                    " tipiruoli.tiporuolo, entepersonale.identepersonale,entepersonale.nome + ' ' + entepersonale.cognome as nominativo," & _
                    " entepersonale.email,entepersonale.telefono,entepersonale.datanascita," & _
                    " Comuni.denominazione, entepersonale.idente,entepersonale.cognome,entepersonale.nome,case isnull(entepersonale.CorsoOLP,'0') when 0 then '0' when 1 then '1'  else 1 end as CorsoOLP," & _
                    " isnull(case len(day(entepersonaleruoli.DataFineValidità)) when 1 then '0' + convert(varchar(20),day(entepersonaleruoli.DataFineValidità)) " & _
                    " else convert(varchar(20),day(entepersonaleruoli.DataFineValidità))  end + '/' + " & _
                    " (case len(month(entepersonaleruoli.DataFineValidità)) when 1 then '0' + convert(varchar(20),month(entepersonaleruoli.DataFineValidità)) " & _
                    " else convert(varchar(20),month(entepersonaleruoli.DataFineValidità))  end + '/' + " & _
                    " Convert(varchar(20), Year(entepersonaleruoli.DataFineValidità))), '') as DataFine, " & _
                    " isnull(entepersonaleruoli.UserNameInseritore,'') as UserNameInseritore, entepersonaleruoli.DataInseritore, " & _
                    " dbo.FN_VERIFICA_USABILITA_OLP(" & Request.QueryString("IdAttivita") & ", " & Request.QueryString("idattES") & ",entepersonale.codicefiscale) as ControlloUsabilitaOLP, entepersonale.codicefiscale, " & _
                    " dbo.FN_VERIFICA_USABILITA_RLEA(" & Request.QueryString("IdAttivita") & ", " & Request.QueryString("idattES") & ",entepersonale.codicefiscale) as ControlloUsabilitaRLEA, " & _
                    " dbo.FN_VERIFICA_ABILITA_SCELTA_CARICAMENTOCV(" & Request.QueryString("IdAttivita") & ",  entepersonale.codicefiscale) as ControlloUsabilitaCV, " & _
                    " dbo.FN_VERIFICA_VALORE_SCELTA_CARICAMENTOCV(" & Request.QueryString("IdAttivita") & ", entepersonale.codicefiscale) as EsistenzaCV, " & _
                    " case when tInCorso.IdSostituzioneOLP is not null then tInCorso.IdSostituzioneOLP else 0 end inCorso " & _
                    " from entepersonale " & _
                    " inner join entepersonaleruoli on (entepersonaleruoli.identepersonale=entepersonale.identepersonale)" & _
                    " inner join associaEntepersonaleRuoliattivitàentisediattuazione a " & _
                    " on  (a.idEntepersonaleRuolo=entepersonaleruoli.identepersonaleRuolo)" & _
                    " inner join attivitàentisediattuazione on " & _
                    " (attivitàentisediattuazione.idAttivitàEntesedeAttuazione=a.idAttivitàEntesedeAttuazione)  " & _
                    " inner join ruoli on (ruoli.idruolo=entepersonaleruoli.idruolo)" & _
                    " inner join tipiruoli on (tipiruoli.idTipoRuolo=ruoli.idtiporuolo)" & _
                    " inner join Comuni on (Comuni.idComune=entepersonale.idComuneNascita) " & _
                    " left join(	select " & _
                    " 				IdAttivitàEnteSedeAttuazione,IdEntePersonaleRuoloSostituito,IdSostituzioneOLP " & _
                    " 			from " & _
                    " 				SostituzioniOLP so " & _
                    " 				left join IstanzeSostituzioniOLP iso on iso.IdIstanzaSostituzioneOLP=so.IdIstanzaSostituzioneOLP " & _
                    " 			where " & _
                    " 				iso.stato is null or iso.Stato in(1,2) " & _
                    " 	) tInCorso on tInCorso.IdAttivitàEnteSedeAttuazione=attivitàentisediattuazione.IDAttivitàEnteSedeAttuazione and IdEntePersonaleRuoloSostituito=entepersonaleruoli.IDEntePersonaleRuolo "
        query = query & " where" & _
                " attivitàentisediattuazione.identesedeattuazione=" & lblidsedeattuazione.Value & " and attivitàentisediattuazione.idattività=" & Request.QueryString("IdAttivita") & " "
        If Session("TipoUtente") = "E" Then
            Select Case UCase(lblRuolo.Text)
                Case "OLP"
                    query = query & " and entepersonaleruoli.idruolo=1 and (entepersonaleruoli.Accreditato=1 or entepersonaleruoli.Accreditato=0) "
                Case "RLEA"
                    'se il tasto NUOVO è visibile vuol dire che è un bando straordinario quindi èprendo anche quelli da accreditare, altrimenti solo gli accreditati
                    query = query & " and entepersonaleruoli.idruolo=6 and (entepersonaleruoli.accreditato=1  or entepersonaleruoli.Accreditato=0) "
                Case "TUTOR"
                    query = query & " and entepersonaleruoli.idruolo=5 and (entepersonaleruoli.accreditato=1  or entepersonaleruoli.Accreditato=0) "
            End Select
        Else
            Select Case UCase(lblRuolo.Text)
                Case "OLP"
                    query = query & " and entepersonaleruoli.idruolo=1 "
                Case "RLEA"
                    query = query & " and entepersonaleruoli.idruolo=6 "
                Case "TUTOR"
                    query = query & " and entepersonaleruoli.idruolo=5 "
            End Select
        End If
        If txtCognome.Text <> "" Then
            query = query & " and entepersonale.cognome like '" & Replace(txtCognome.Text, "'", "''") & "%' "
        End If
        If txtNome.Text <> "" Then
            query = query & " and entepersonale.nome like '" & Replace(txtNome.Text, "'", "''") & "%' "
        End If
        If txtcomuneNascita.Text <> "" Then
            query = query & " and Comuni.denominazione like '" & Replace(txtcomuneNascita.Text, "'", "''") & "%' "
        End If
        If chkVisualizzaOlp.Checked = False Then
            If mypostback = False Then
                query = query & " Union Select TOP 50 '<IMG width=25 height=20 src=""images/xp1.gif"">' as img,entepersonaleruoli.Accreditato ,entepersonaleruoli.identePersonaleRuolo," & _
                " '0' as identesedeattuazione," & _
                " '0' as idass, '0' as trovaChec," & _
                " tipiruoli.tiporuolo, entepersonale.identepersonale,entepersonale.nome + ' ' + entepersonale.cognome as nominativo," & _
                " entepersonale.email,entepersonale.telefono,entepersonale.datanascita," & _
                " Comuni.denominazione, entepersonale.idente,entepersonale.cognome,entepersonale.nome,case isnull(entepersonale.CorsoOLP,'0') when 0 then '0' when 1 then '1'  else 1 end as CorsoOLP," & _
                " isnull(case len(day(entepersonaleruoli.DataFineValidità)) when 1 then '0' + convert(varchar(20),day(entepersonaleruoli.DataFineValidità)) " & _
                " else convert(varchar(20),day(entepersonaleruoli.DataFineValidità))  end + '/' + " & _
                " (case len(month(entepersonaleruoli.DataFineValidità)) when 1 then '0' + convert(varchar(20),month(entepersonaleruoli.DataFineValidità)) " & _
                " else convert(varchar(20),month(entepersonaleruoli.DataFineValidità))  end + '/' + " & _
                " Convert(varchar(20), Year(entepersonaleruoli.DataFineValidità))), '') as DataFine, " & _
                " isnull(entepersonaleruoli.UserNameInseritore,'') as UserNameInseritore, entepersonaleruoli.DataInseritore, " & _
                " dbo.FN_VERIFICA_USABILITA_OLP(" & Request.QueryString("IdAttivita") & "," & Request.QueryString("idattES") & " ,entepersonale.codicefiscale) as ControlloUsabilitaOLP, entepersonale.codicefiscale , " & _
                " dbo.FN_VERIFICA_USABILITA_RLEA(" & Request.QueryString("IdAttivita") & ", " & Request.QueryString("idattES") & ",entepersonale.codicefiscale) as ControlloUsabilitaRLEA, " & _
                " dbo.FN_VERIFICA_ABILITA_SCELTA_CARICAMENTOCV(" & Request.QueryString("IdAttivita") & ",  entepersonale.codicefiscale) as ControlloUsabilitaCV , " & _
                " dbo.FN_VERIFICA_VALORE_SCELTA_CARICAMENTOCV(" & Request.QueryString("IdAttivita") & ", entepersonale.codicefiscale) as EsistenzaCV, " & _
                " 0 as inCorso " & _
                " from entepersonale " & _
                " inner join entepersonaleruoli on (entepersonaleruoli.identepersonale=entepersonale.identepersonale)" & _
                " inner join ruoli on (ruoli.idruolo=entepersonaleruoli.idruolo)" & _
                " inner join tipiruoli on (tipiruoli.idTipoRuolo=ruoli.idtiporuolo)" & _
                " inner join Comuni on (Comuni.idComune=entepersonale.idComuneNascita) " & _
                " where entepersonale.idente=" & Session("IdEnte") & " "
                If Session("TipoUtente") = "E" Then
                    Select Case UCase(lblRuolo.Text)
                        Case "OLP"
                            query = query & " and entepersonaleruoli.idruolo=1 and (entepersonaleruoli.Accreditato=1 or entepersonaleruoli.Accreditato=0) "
                        Case "RLEA"
                            'se il tasto NUOVO è visibile vuol dire che è un bando straordinario quindi èprendo anche quelli da accreditare, altrimenti solo gli accreditati
                            query = query & " and entepersonaleruoli.idruolo=6 and (entepersonaleruoli.accreditato=1  or entepersonaleruoli.Accreditato=0) "
                        Case "TUTOR"
                            'se il tasto NUOVO è visibile vuol dire che è un bando straordinario quindi èprendo anche quelli da accreditare, altrimenti solo gli accreditati
                            query = query & " and entepersonaleruoli.idruolo=5 and (entepersonaleruoli.accreditato=1  or entepersonaleruoli.Accreditato=0) "
                    End Select
                Else
                    Select Case UCase(lblRuolo.Text)
                        Case "OLP"
                            query = query & " and entepersonaleruoli.idruolo=1 "
                        Case "RLEA"
                            query = query & " and entepersonaleruoli.idruolo=6 "
                        Case "TUTOR"
                            query = query & " and entepersonaleruoli.idruolo=5 "
                    End Select
                End If
                query = query & " " & _
                " and entepersonale.identepersonale not in ( " & _
                " Select entepersonale.identepersonale " & _
                " from entepersonale " & _
                " inner join entepersonaleruoli on (entepersonaleruoli.identepersonale=entepersonale.identepersonale)" & _
                " inner join associaEntepersonaleRuoliattivitàentisediattuazione a " & _
                " on  (a.idEntepersonaleRuolo=entepersonaleruoli.identepersonaleRuolo)" & _
                " inner join attivitàentisediattuazione on " & _
                " (attivitàentisediattuazione.idAttivitàEntesedeAttuazione=a.idAttivitàEntesedeAttuazione)  " & _
                " inner join ruoli on (ruoli.idruolo=entepersonaleruoli.idruolo)" & _
                " inner join tipiruoli on (tipiruoli.idTipoRuolo=ruoli.idtiporuolo)" & _
                " inner join Comuni on (Comuni.idComune=entepersonale.idComuneNascita) "
                query = query & " where AttivitàEntiSediAttuazione.IdEnteSedeAttuazione = " & lblidsedeattuazione.Value & " And AttivitàEntiSediAttuazione.IdAttività = " & Request.QueryString("IdAttivita") & " )"
                If Session("TipoUtente") = "E" Then
                    Select Case UCase(lblRuolo.Text)
                        Case "OLP"
                            query = query & " and entepersonaleruoli.idruolo=1 and (entepersonaleruoli.Accreditato=1 or entepersonaleruoli.Accreditato=0) "
                        Case "RLEA"
                            'se il tasto NUOVO è visibile vuol dire che è un bando straordinario quindi èprendo anche quelli da accreditare, altrimenti solo gli accreditati
                            query = query & " and entepersonaleruoli.idruolo=6 and (entepersonaleruoli.accreditato=1  or entepersonaleruoli.Accreditato=0) "
                        Case "TUTOR"
                            'se il tasto NUOVO è visibile vuol dire che è un bando straordinario quindi èprendo anche quelli da accreditare, altrimenti solo gli accreditati
                            query = query & " and entepersonaleruoli.idruolo=5 and (entepersonaleruoli.accreditato=1  or entepersonaleruoli.Accreditato=0) "
                    End Select
                Else
                    Select Case UCase(lblRuolo.Text)
                        Case "OLP"
                            query = query & " and entepersonaleruoli.idruolo=1 "
                        Case "RLEA"
                            query = query & " and entepersonaleruoli.idruolo=6 "
                        Case "TUTOR"
                            query = query & " and entepersonaleruoli.idruolo=5 "
                    End Select
                End If
                If txtCognome.Text <> "" Then
                    query = query & " and entepersonale.cognome like '" & Replace(txtCognome.Text, "'", "''") & "%' "
                End If
                If txtNome.Text <> "" Then
                    query = query & " and entepersonale.nome like '" & Replace(txtNome.Text, "'", "''") & "%' "
                End If
                If txtcomuneNascita.Text <> "" Then
                    query = query & " and Comuni.denominazione like '" & Replace(txtcomuneNascita.Text, "'", "''") & "%' "
                End If
            End If
        End If
        query = query & " order by entepersonale.Cognome,entepersonale.nome"
        dataSet = ClsServer.DataSetGenerico(query, Session("conn"))
        CaricaDataGrid(dgRisultatoRicerca)
        dtOlp = ClsServer.CreaDataTable(query, False, Session("conn"))

        'assegno il dataset alla griglia del risultato
        dgRisultatoRicerca.DataSource = dtOlp
        dgRisultatoRicerca.DataBind()
        Session("SessionDtOlp") = dtOlp
        Dim tot As Integer = dtOlp.Rows.Count
        dtOlp.Dispose()
        Prepara()

        'ADC 14/01/2021
        PreparaCV()

        PreparaCorsoOlp()
        controllaChek()
        controllaChekCorsoOlp()
        ColoraCelle()

        If dgRisultatoRicerca.Items.Count = 0 Then
            If chkVisualizzaOlp.Checked = True Then
                If txtCognome.Text <> "" Or txtNome.Text <> "" Or txtcomuneNascita.Text <> "" Then
                    lblErrore.Text = "Non sono presenti " & lblRuolo.Text & " assegnati al Progetto corrispondenti ai parametri di ricerca selezionati."
                    lblMessaggioInfo.Text = ""
                Else
                    lblErrore.Text = "Non sono presenti " & lblRuolo.Text & " assegnati al Progetto."
                    lblMessaggioInfo.Text = ""
                End If
            Else
                If txtCognome.Text <> "" Or txtNome.Text <> "" Or txtcomuneNascita.Text <> "" Then
                    lblErrore.Text = "Non sono disponibili " & lblRuolo.Text & " da assegnare sul Progetto corrispondenti ai parametri di ricerca selezionati."
                    lblMessaggioInfo.Text = ""
                Else
                    If mypostback = False Then
                        lblMessaggioInfo.Text = "Premere il pulsante RICERCA per ottenere la lista delle figure professionali disponibili."
                    Else
                        lblErrore.Text = "Non sono disponibili " & lblRuolo.Text & " da assegnare sul Progetto."
                        lblMessaggioInfo.Text = ""
                    End If
                End If
            End If
            cmdConferma.Visible = False
        Else
            cmdConferma.Visible = False 'pulsante sempre invisibile, cambiato metodo assegnazione (popup)
            If tot >= 50 Then
                lblMessaggioInfo.Text = "Sono stati estratti i primi 50 " & lblRuolo.Text & ".Si prega di utilizzare i filtri per ottimizzare la ricerca."
            Else
                lblMessaggioInfo.Text = ""
            End If

        End If
        If lblModifica.Value = "False" Then
            BloccaChek()
            imgNuovo.Visible = False
        End If

        If Request.QueryString("Nazionale") <> 11 And Request.QueryString("Nazionale") <> 12 Then
            'disabilito funzioni per scelta caricamento cv
            lblNota.Text = ""
            dgRisultatoRicerca.Columns.Item(15).Visible = False
        Else
            lblNota.Visible = False
        End If

        '**********************************************
    End Sub

    Private Sub Prepara()
        Dim i As Integer
        Dim Mychk As CheckBox
        Dim dtGriglia As DataTable 'appoggio
        Dim rwGriglia As DataRow
        Dim clGriglia As DataColumn

        dtGriglia = Session("SessionDtOlp")
        clGriglia = dtGriglia.Columns(5)
        clGriglia.ReadOnly = False
        For i = 0 To dgRisultatoRicerca.Items.Count - 1
            Mychk = dgRisultatoRicerca.Items.Item(i).FindControl("check1")
            rwGriglia = dtGriglia.Rows(i + (dgRisultatoRicerca.CurrentPageIndex * dgRisultatoRicerca.PageSize))
            If rwGriglia.Item(5) = 1 Then
                Mychk.Checked = True
            Else
                Mychk.Checked = False
            End If
        Next i
    End Sub
    'ADC 14/01/2021
    Private Sub PreparaCV()
        Dim i As Integer
        Dim MychkCv As CheckBox
        Dim dtGriglia As DataTable 'appoggio
        Dim rwGriglia As DataRow
        Dim clGriglia As DataColumn

        dtGriglia = Session("SessionDtOlp")

        clGriglia = dtGriglia.Columns(24) 'ADC 14/01/2021'ho messo 16 solo per un appoggio andra' il valore restituito dal datatable
        clGriglia.ReadOnly = False
        For i = 0 To dgRisultatoRicerca.Items.Count - 1
            MychkCv = dgRisultatoRicerca.Items.Item(i).FindControl("ChkCaricamentoCV")
            rwGriglia = dtGriglia.Rows(i + (dgRisultatoRicerca.CurrentPageIndex * dgRisultatoRicerca.PageSize))
            If rwGriglia.Item(24) = 1 Then
                MychkCv.Checked = True
            Else
                MychkCv.Checked = False
            End If
        Next i

        Dim ix As Integer
        Dim dtGriglia1 As DataTable 'appoggio
        Dim rwGriglia1 As DataRow
        Dim clGriglia1 As DataColumn

        dtGriglia1 = Session("SessionDtOlp")

        clGriglia1 = dtGriglia1.Columns(23) 'ADC 14/01/2021'ho messo 16 solo per un appoggio andra' il valore restituito dal datatable
        clGriglia1.ReadOnly = False
        For ix = 0 To dgRisultatoRicerca.Items.Count - 1
            MychkCv = dgRisultatoRicerca.Items.Item(ix).FindControl("ChkCaricamentoCV")
            rwGriglia1 = dtGriglia1.Rows(ix + (dgRisultatoRicerca.CurrentPageIndex * dgRisultatoRicerca.PageSize))
            If rwGriglia1.Item(23) = 1 Then
                MychkCv.Enabled = True
            Else
                MychkCv.Enabled = False
            End If
        Next ix

    End Sub

    Private Sub PreparaCorsoOlp()
        Dim i As Integer
        Dim Mychk As CheckBox
        Dim MychkNo As CheckBox
        Dim MychkAssegnazione As CheckBox
        Dim dtGriglia As DataTable 'appoggio
        Dim rwGriglia As DataRow
        Dim clGriglia As DataColumn

        dtGriglia = Session("SessionDtOlp")
        clGriglia = dtGriglia.Columns(16)
        clGriglia.ReadOnly = False
        For i = 0 To dgRisultatoRicerca.Items.Count - 1
            MychkAssegnazione = dgRisultatoRicerca.Items.Item(i).FindControl("check1")
            Mychk = dgRisultatoRicerca.Items.Item(i).FindControl("chkCorsoOlp")
            MychkNo = dgRisultatoRicerca.Items.Item(i).FindControl("chkCorsoOlpNo")
            rwGriglia = dtGriglia.Rows(i + (dgRisultatoRicerca.CurrentPageIndex * dgRisultatoRicerca.PageSize))
            If MychkAssegnazione.Checked = True Then
                If rwGriglia.Item(16) = "1" Then
                    Mychk.Checked = True
                    MychkNo.Checked = False
                Else
                    Mychk.Checked = False
                    MychkNo.Checked = True
                End If
            Else
                Mychk.Checked = False
                MychkNo.Checked = False
            End If

        Next i
        
        'If rwGriglia.Item(16) = "1" Then
        '    MychkNo.Checked = True
        'Else
        '    MychkNo.Checked = False
        'End If

    End Sub

    Private Sub controllaChek()
        'realizzato da Alessandra Taballione  29/09/04
        'check valorizzati per degli olp inseriti
        Dim item As DataGridItem
        For Each item In dgRisultatoRicerca.Items
            Dim check As CheckBox = DirectCast(item.FindControl("check1"), CheckBox)
            If CDbl(dgRisultatoRicerca.Items(item.ItemIndex).Cells(7).Text) > 0 And dgRisultatoRicerca.Items(item.ItemIndex).Cells(10).Text = lblidsedeattuazione.Value Then
                check.Checked = True
            End If

            'ADC 14/01/2021 ??????


            If UCase(lblRuolo.Text) = "OLP" Then
                If CDbl(dgRisultatoRicerca.Items(item.ItemIndex).Cells(17).Text) = 0 Then 'ex 16
                    check.Visible = False
                End If
            End If
            If UCase(lblRuolo.Text) = "RLEA" Then
                If CDbl(dgRisultatoRicerca.Items(item.ItemIndex).Cells(20).Text) = 0 Then 'ex19
                    check.Visible = False
                End If
            End If
        Next
    End Sub

    Private Sub controllaChekCorsoOlp()
        Dim item As DataGridItem
        For Each item In dgRisultatoRicerca.Items
            Dim check As CheckBox = DirectCast(item.FindControl("chkCorsoOlp"), CheckBox)
            Dim checkNo As CheckBox = DirectCast(item.FindControl("chkCorsoOlpNo"), CheckBox)
            Dim checkAss As CheckBox = DirectCast(item.FindControl("check1"), CheckBox)
            If checkAss.Checked = True Then
                If CDbl(dgRisultatoRicerca.Items(item.ItemIndex).Cells(12).Text) > 0 Then
                    check.Checked = True
                    checkNo.Checked = False
                Else
                    check.Checked = False
                    checkNo.Checked = True
                End If
            Else
                check.Checked = False
                checkNo.Checked = False
            End If


        Next
        'For Each item In dgRisultatoRicerca.Items
        '    Dim check As CheckBox = DirectCast(item.FindControl("chkCorsoOlpNo"), CheckBox)
        '    If CDbl(dgRisultatoRicerca.Items(item.ItemIndex).Cells(12).Text) > 0 Then
        '        check.Checked = True
        '    End If
        'Next
    End Sub
   
    Private Sub ColoraCelle()
        'Generato da Alessandra Taballione il 15/06/04
        'VAriazione del Colore secondo lo stato della sede.
        'Attiva=Verde;Presentata=gialla;Cancellata=Rossa;



        Dim item As DataGridItem
        Dim x As Integer
        For Each item In dgRisultatoRicerca.Items
            For x = 0 To dgRisultatoRicerca.Columns.Count - 1
                dgRisultatoRicerca.Items(item.ItemIndex).Cells(x).ForeColor = NERO
                If Session("TipoUtente") = "E" Then
                    If UCase(lblRuolo.Text) = "OLP" Then
                        If dgRisultatoRicerca.Items(item.ItemIndex).Cells(17).Text = "0" Then 'ex16
                            dgRisultatoRicerca.Items(item.ItemIndex).Cells(x).BackColor = ROSSO
                            'dgRisultatoRicerca.Items(item.ItemIndex).Cells(20).Text = "Cancellata"
                        Else
                            dgRisultatoRicerca.Items(item.ItemIndex).Cells(x).BackColor = BIANCO
                        End If
                    End If
                    If UCase(lblRuolo.Text) = "RLEA" Then
                        If dgRisultatoRicerca.Items(item.ItemIndex).Cells(20).Text = "0" Then 'ex19
                            dgRisultatoRicerca.Items(item.ItemIndex).Cells(x).BackColor = ROSSO
                            'dgRisultatoRicerca.Items(item.ItemIndex).Cells(20).Text = "Cancellata"
                        Else
                            dgRisultatoRicerca.Items(item.ItemIndex).Cells(x).BackColor = BIANCO
                        End If
                    End If
                Else
                    If (dgRisultatoRicerca.Items(item.ItemIndex).Cells(17).Text = "0" And UCase(lblRuolo.Text) = "OLP") Or (dgRisultatoRicerca.Items(item.ItemIndex).Cells(20).Text = "0" And UCase(lblRuolo.Text) = "RLEA") Then 'ex16 e ex19
                        dgRisultatoRicerca.Items(item.ItemIndex).Cells(x).BackColor = ROSSO
                        'dgRisultatoRicerca.Items(item.ItemIndex).Cells(20).Text = "Cancellata"
                    Else
                        If dgRisultatoRicerca.Items(item.ItemIndex).Cells(7).Text = "1" Then
                            'Accreditato
                            dgRisultatoRicerca.Items(item.ItemIndex).Cells(x).BackColor = VERDE
                            'dgRisultatoRicerca.Items(item.ItemIndex).Cells(20).Text = "Attiva"
                        ElseIf dgRisultatoRicerca.Items(item.ItemIndex).Cells(7).Text = "0" Then
                            'Proposto
                            If (dgRisultatoRicerca.Items(item.ItemIndex).Cells(14).Text <> "" And dgRisultatoRicerca.Items(item.ItemIndex).Cells(14).Text <> "&nbsp;") Then
                                If Mid(dgRisultatoRicerca.Items(item.ItemIndex).Cells(14).Text, 1, 1) = "N" Then
                                    dgRisultatoRicerca.Items(item.ItemIndex).Cells(x).BackColor = BIANCO
                                Else
                                    dgRisultatoRicerca.Items(item.ItemIndex).Cells(x).BackColor = GIALLO
                                    'dgRisultatoRicerca.Items(item.ItemIndex).Cells(20).Text = "Presentata"

                                End If
                            Else
                                dgRisultatoRicerca.Items(item.ItemIndex).Cells(x).BackColor = GIALLO
                                'dgRisultatoRicerca.Items(item.ItemIndex).Cells(20).Text = "Presentata"
                            End If
                        ElseIf dgRisultatoRicerca.Items(item.ItemIndex).Cells(7).Text = "-1" Then
                            'Respinto
                            dgRisultatoRicerca.Items(item.ItemIndex).Cells(x).BackColor = ROSSO
                            'dgRisultatoRicerca.Items(item.ItemIndex).Cells(20).Text = "Cancellata"
                        End If
                    End If
                End If
            Next
        Next
    End Sub

    Private Sub cmdSalva_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles cmdSalva.Click
        ReipostaLabelMessaggi()
        dgRisultatoRicerca.CurrentPageIndex = 0
        RicercaOLP(False)
    End Sub

    Private Sub cmdConferma_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles cmdConferma.Click
        ReipostaLabelMessaggi()
        Dim i As Integer
        Dim Mychk As CheckBox
        Dim MychkNo As CheckBox
        Dim MychkAssegna As CheckBox
        Dim MychkCv As CheckBox
        Dim dtGriglia As DataTable 'appoggio
        Dim rwGriglia As DataRow
        Dim clGriglia As DataColumn

        dtGriglia = Session("SessionDtOlp")
        clGriglia = dtGriglia.Columns(5)
        clGriglia.ReadOnly = False
        '---determino cosa è stato checkato nella pag corrente e lo salvo nella datatable di sessione
        For i = 0 To dgRisultatoRicerca.Items.Count - 1
            'Controllo il colore della griglia
            MychkAssegna = dgRisultatoRicerca.Items.Item(i).FindControl("check1")
            rwGriglia = dtGriglia.Rows(i + (dgRisultatoRicerca.CurrentPageIndex * dgRisultatoRicerca.PageSize))
            If MychkAssegna.Checked = True Then
                'Controllo se sia rosso o meno L'ELEMENTO
                If dgRisultatoRicerca.Items(i).Cells(5).BackColor.ToString = ROSSO.ToString Then
                    lblErrore.Text = "Impossibile associare ruoli precedentemente cancellati"
                    Exit Sub
                End If
                rwGriglia.Item(5) = 1
            Else
                rwGriglia.Item(5) = 0
            End If
        Next i


       


        '---------SE E' OLP---------------------------------CONTROLLO SI/NO-----------------------------------------------------
        If UCase(lblRuolo.Text) = "OLP" Then    '---------------------------------------------------22/06/2017



            dtGriglia = Session("SessionDtOlp")
            clGriglia = dtGriglia.Columns(16)
            clGriglia.ReadOnly = False
            '---determino cosa è stato checkato nella pag corrente e lo salvo nella datatable di sessione
            For i = 0 To dgRisultatoRicerca.Items.Count - 1

                Mychk = dgRisultatoRicerca.Items.Item(i).FindControl("chkCorsoOlp")
                MychkNo = dgRisultatoRicerca.Items.Item(i).FindControl("chkCorsoOlpNo")
                MychkAssegna = dgRisultatoRicerca.Items.Item(i).FindControl("check1")


                rwGriglia = dtGriglia.Rows(i + (dgRisultatoRicerca.CurrentPageIndex * dgRisultatoRicerca.PageSize))


                If MychkAssegna.Checked = True Then

                    If Mychk.Checked = True And MychkNo.Checked = True Then

                        lblErrore.Text = "AGGIORNAMENTO NON EFFETTUATO. Sono state indicate entrambe le caselle di frequentazione corso OLP."
                        Exit Sub
                    End If



                    '--------------------------------------------------------------------------------------------------

                    If Mychk.Checked = False And MychkNo.Checked = False And MychkAssegna.Checked = True Then

                        lblErrore.Text = "AGGIORNAMENTO NON EFFETTUATO. Va indicato obbligatoriamente il corso OLP si/no."
                        Exit Sub
                    Else
                        If Mychk.Checked = True Then
                            rwGriglia.Item(16) = "1"
                        Else
                            rwGriglia.Item(16) = "0"
                        End If
                    End If

                    '-------------------------------------------------------------------------------------------
                Else

                    If MychkAssegna.Checked = False And (Mychk.Checked = True Or MychkNo.Checked = True) Then
                        lblErrore.Text = "AGGIORNAMENTO NON EFFETTUATO.Se si indicata  il corso OLP si/no va effettuata l'assegnazione."
                        Exit Sub
                    End If
                End If




            Next i

            Session("SessionDtOlp") = dtGriglia
            clGriglia = dtGriglia.Columns(16)
            clGriglia.ReadOnly = False
            '---determino cosa è stato checkato nella pag corrente e lo salvo nella datatable di sessione
            For i = 0 To dtGriglia.Rows.Count - 1
                aggiornamentoCorsoOLP(dtGriglia.Rows(i).Item(7), dtGriglia.Rows(i).Item(16))
            Next i
        End If    '---------------------------------------------------22/06/2017
        '-----------------------------------------------------------------------------------------------

        ''ADC 18/01/2021
        'clGriglia = dtGriglia.Columns(24)
        'clGriglia.ReadOnly = False
        ''---determino cosa è stato checkato nella pag corrente e lo salvo nella datatable di sessione
        'For i = 0 To dgRisultatoRicerca.Items.Count - 1
        '    'Controllo il colore della griglia
        '    MychkCv = dgRisultatoRicerca.Items.Item(i).FindControl("ChkCaricamentoCV")
        '    rwGriglia = dtGriglia.Rows(i + (dgRisultatoRicerca.CurrentPageIndex * dgRisultatoRicerca.PageSize))
        '    If MychkCv.Checked = True Then
        '        'Controllo se sia rosso o meno L'ELEMENTO
        '        If dgRisultatoRicerca.Items(i).Cells(24).BackColor.ToString = ROSSO.ToString Then
        '            lblErrore.Text = "Impossibile indicare CV"
        '            Exit Sub
        '        End If
        '        rwGriglia.Item(24) = 1
        '    Else
        '        rwGriglia.Item(24) = 0
        '    End If

        'Next i



        Session("SessionDtOlp") = dtGriglia
        clGriglia = dtGriglia.Columns(5)
        clGriglia.ReadOnly = False
        '---determino cosa è stato checkato nella pag corrente e lo salvo nella datatable di sessione
        For i = 0 To dtGriglia.Rows.Count - 1
            If dtGriglia.Rows(i).Item(4) = 0 Then
                If dtGriglia.Rows(i).Item(5) = 1 Then
                    inserimentoOlp(dtGriglia.Rows(i).Item(2), lblidattEs.Value)
                End If
            End If
            If dtGriglia.Rows(i).Item(4) <> 0 Then
                If dtGriglia.Rows(i).Item(5) = 0 And dtGriglia.Rows(i).Item(3) = lblidsedeattuazione.Value Then
                    EliminaOlp(dtGriglia.Rows(i).Item(4))
                Else
                    If dtGriglia.Rows(i).Item(3) <> lblidsedeattuazione.Value Then
                        inserimentoOlp(dtGriglia.Rows(i).Item(2), lblidattEs.Value)
                    End If
                End If
            End If
        Next i

        'ADC 18/01/2021
        dtGriglia = Session("SessionDtOlp")
        clGriglia = dtGriglia.Columns(24)
        clGriglia.ReadOnly = False
        '---determino cosa è stato checkato nella pag corrente e lo salvo nella datatable di sessione
        For i = 0 To dgRisultatoRicerca.Items.Count - 1
            'Controllo il colore della griglia
            MychkCv = dgRisultatoRicerca.Items.Item(i).FindControl("ChkCaricamentoCV")
            rwGriglia = dtGriglia.Rows(i + (dgRisultatoRicerca.CurrentPageIndex * dgRisultatoRicerca.PageSize))
            If MychkCv.Checked = True Then
                rwGriglia.Item(24) = 1
            Else
                rwGriglia.Item(24) = 0
            End If


        Next
        'aggiorno colonna CaricamentoCv (item 24) per gli olp associati (item 5)
        For i = 0 To dtGriglia.Rows.Count - 1
            If dtGriglia.Rows(i).Item(5) = 1 Then
                UpdateOlpCv(dtGriglia.Rows(i).Item(2), lblidattEs.Value, dtGriglia.Rows(i).Item(24))
            End If
        Next i

        If (lblErrore.Text = String.Empty) Then
            lblMessaggioConferma.Text = "Operazione effettuata con successo."
        End If
        ' AssociaEntePersonaleRuoliAttivitàEntiSediAttuazione 
    End Sub
    Private Sub UpdateOlpCv(ByVal identePersonaleRuolo As String, ByVal idattEsA As String, ByVal cv As Integer)
        ChiudiDataReader(dataReader)
        If Request.QueryString("Nazionale") <> 11 And Request.QueryString("Nazionale") <> 12 Then
            cv = 1
        End If
        query = "update associaentepersonaleruoliattivitàentisediattuazione " & _
        "set CaricamentoCv=" & cv & " where  identePersonaleRuolo=" & identePersonaleRuolo & " and idattivitàentesedeattuazione=" & idattEsA & " "
        sqlCommand = ClsServer.EseguiSqlClient(query, Session("conn"))
    End Sub

    Private Sub aggiornamentoCorsoOLP(ByVal identePersonale As String, ByVal intCorso As Integer)
        ChiudiDataReader(dataReader)
        query = "update entepersonale set CorsoOLP=" & intCorso & " where identepersonale='" & identePersonale & "'"
        sqlCommand = ClsServer.EseguiSqlClient(query, Session("conn"))
    End Sub

    Private Sub inserimentoOlp(ByVal identePersonaleRuolo As String, ByVal idattEsA As String)
        query = "Select * from associaentepersonaleruoliattivitàentisediattuazione " & _
        " where  identePersonaleRuolo=" & identePersonaleRuolo & " and idattivitàentesedeattuazione=" & idattEsA & " "
        ChiudiDataReader(dataReader)
        dataReader = ClsServer.CreaDatareader(query, Session("conn"))
        If dataReader.HasRows = False Then
            ChiudiDataReader(dataReader)
            query = "Insert into associaentepersonaleruoliattivitàentisediattuazione " & _
            "(identePersonaleRuolo,idattivitàentesedeattuazione,datacreazioneRecord,UsernameInseritore) values " & _
            "(" & identePersonaleRuolo & "," & idattEsA & ",getdate(),'" & Session("Utente") & "')"
            sqlCommand = ClsServer.EseguiSqlClient(query, Session("conn"))
        End If
    End Sub

    Private Sub EliminaOlp(ByVal idAssEPRAESA As String)
        query = "delete from associaEntepersonaleRuoliattivitàentisediattuazione " & _
        " where idassociaEntepersonaleRuoliattivitàentisediattuazione=" & idAssEPRAESA & " "
        sqlCommand = ClsServer.EseguiSqlClient(query, Session("conn"))
    End Sub


    Private Sub dgRisultatoRicerca_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgRisultatoRicerca.PageIndexChanged
        Dim i As Integer
        Dim Mychk As CheckBox
        Dim MychkNo As CheckBox
        Dim MychkAss As CheckBox
        Dim MychkCv As CheckBox
        Dim dtGriglia As DataTable 'appoggio
        Dim rwGriglia As DataRow
        Dim clGriglia As DataColumn

        dtGriglia = Session("SessionDtOlp")
        clGriglia = dtGriglia.Columns(5)
        clGriglia.ReadOnly = False
        '---determino cosa è stato checkato nella pag corrente e lo salvo nella datatable di sessione
        For i = 0 To dgRisultatoRicerca.Items.Count - 1
            MychkAss = dgRisultatoRicerca.Items.Item(i).FindControl("check1")
            rwGriglia = dtGriglia.Rows(i + (dgRisultatoRicerca.CurrentPageIndex * dgRisultatoRicerca.PageSize))
            If MychkAss.Checked = True Then
                rwGriglia.Item(5) = 1
            Else
                rwGriglia.Item(5) = 0
            End If
        Next i

        dtGriglia = Session("SessionDtOlp")
        clGriglia = dtGriglia.Columns(17) 'ex16
        clGriglia.ReadOnly = False
        '---determino cosa è stato checkato nella pag corrente e lo salvo nella datatable di sessione
        For i = 0 To dgRisultatoRicerca.Items.Count - 1
            Mychk = dgRisultatoRicerca.Items.Item(i).FindControl("chkCorsoOlp")

            'MychkNo = dgRisultatoRicerca.Items.Item(i).FindControl("chkCorsoOlpNo")
            rwGriglia = dtGriglia.Rows(i + (dgRisultatoRicerca.CurrentPageIndex * dgRisultatoRicerca.PageSize))

            If Mychk.Checked = True Then
                rwGriglia.Item(16) = "1"
            Else
                rwGriglia.Item(16) = "0"
            End If


        Next i


        dtGriglia = Session("SessionDtOlp")
        clGriglia = dtGriglia.Columns(24)
        clGriglia.ReadOnly = False
        '---determino cosa è stato checkato nella pag corrente e lo salvo nella datatable di sessione
        For i = 0 To dgRisultatoRicerca.Items.Count - 1
            MychkCv = dgRisultatoRicerca.Items.Item(i).FindControl("ChkCaricamentoCV")


            rwGriglia = dtGriglia.Rows(i + (dgRisultatoRicerca.CurrentPageIndex * dgRisultatoRicerca.PageSize))

            If MychkCv.Checked = True Then
                rwGriglia.Item(24) = "1"
            Else
                rwGriglia.Item(24) = "0"
            End If


        Next i

        Session("SessionDtOlp") = dtGriglia
        dgRisultatoRicerca.CurrentPageIndex = e.NewPageIndex
        dgRisultatoRicerca.DataSource = Session("SessionDtOlp")
        dgRisultatoRicerca.DataBind()

        '---determino cosa c'era text nella datatable di sessione in e lo visualizzo nella pag che vado a caricare
        For i = 0 To dgRisultatoRicerca.Items.Count - 1
            MychkNo = dgRisultatoRicerca.Items.Item(i).FindControl("chkCorsoOlpNo")
            MychkAss = dgRisultatoRicerca.Items.Item(i).FindControl("check1")
            Mychk = dgRisultatoRicerca.Items.Item(i).FindControl("chkCorsoOlp")
            MychkCv = dgRisultatoRicerca.Items.Item(i).FindControl("ChkCaricamentoCV")
            rwGriglia = dtGriglia.Rows(i + (dgRisultatoRicerca.CurrentPageIndex * dgRisultatoRicerca.PageSize))

            If rwGriglia.Item(5) = "1" Then
                MychkAss.Checked = True
                If rwGriglia.Item(16) = "1" Then 'ex16
                    Mychk.Checked = True
                    MychkNo.Checked = False
                Else
                    Mychk.Checked = False
                    MychkNo.Checked = True
                End If
            Else
                MychkAss.Checked = False
                Mychk.Checked = False
                MychkNo.Checked = False
            End If

            If rwGriglia.Item(23) = 1 Then
                MychkCv.Enabled = True
            Else
                MychkCv.Enabled = False
            End If


            If rwGriglia.Item(24) = 1 Then
                MychkCv.Checked = True
            Else
                MychkCv.Checked = False
            End If



        Next i

        '---determino cosa c'era text nella datatable di sessione in e lo visualizzo nella pag che vado a caricare
        For i = 0 To dgRisultatoRicerca.Items.Count - 1
            Mychk = dgRisultatoRicerca.Items.Item(i).FindControl("check1")
            rwGriglia = dtGriglia.Rows(i + (dgRisultatoRicerca.CurrentPageIndex * dgRisultatoRicerca.PageSize))
            If rwGriglia.Item(5) = 1 Then
                Mychk.Checked = True
            Else
                Mychk.Checked = False
            End If
            If UCase(lblRuolo.Text) = "OLP" Then
                If CDbl(rwGriglia.Item(20)) = 0 Then 'controllo usabilità olp
                    Mychk.Visible = False
                End If
            End If
            If UCase(lblRuolo.Text) = "RLEA" Then
                If CDbl(rwGriglia.Item(22)) = 0 Then 'controllo usabilità rlea
                    Mychk.Visible = False
                End If
            End If
        Next i

        ColoraCelle()
        If dgRisultatoRicerca.Items.Count = 0 Then
            If chkVisualizzaOlp.Checked = True Then
                If txtCognome.Text <> "" Or txtNome.Text <> "" Or txtcomuneNascita.Text <> "" Then
                    lblErrore.Text = "Non sono presenti " & lblRuolo.Text & " assegnati al Progetto corrispondenti ai parametri di ricerca selezionati."
                Else
                    lblErrore.Text = "Non sono presenti " & lblRuolo.Text & " assegnati al Progetto."
                End If

            Else
                If txtCognome.Text <> "" Or txtNome.Text <> "" Or txtcomuneNascita.Text <> "" Then
                    lblErrore.Text = "Non sono disponibili " & lblRuolo.Text & " da assegnare sul Progetto corrispondenti ai parametri di ricerca selezionati."
                Else
                    lblErrore.Text = "Non sono disponibili " & lblRuolo.Text & " da assegnare sul Progetto."
                End If
            End If
        Else
            lblErrore.Text = String.Empty
        End If
        If lblModifica.Value = "False" Then
            BloccaChek()
        End If

    End Sub

    Private Sub imgNuovo_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles imgNuovo.Click
        Response.Redirect("InserimentoRisorseProgetti.aspx?Modifica=" & Request.QueryString("Modifica") & "&Nazionale=" & Request.QueryString("Nazionale") & "&Tiporuolo=" & Request.QueryString("Tiporuolo") & "&IdAttivita=" & Request.QueryString("IdAttivita") & "&IdSedeAttuazione=" & Request.QueryString("IdSedeAttuazione") & "&idattES=" & Request.QueryString("idattES") & "&Ruolo=" & lblRuolo.Text & "&VengoDa=" & Request.QueryString("VengoDa") & "")
    End Sub

    Sub ApriPopupAssociaOLP(ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs)
        lblErroreAssegnaRisorsa.Visible = False
        txtProgetto.Text = lblProgetto.Text
        txtSede.Text = lblSede.Text
        If e.Item.Cells(0).Text <> "&nbsp;" AndAlso Not String.IsNullOrEmpty(e.Item.Cells(0).Text) Then         'nominativo
            txtNominativo.Text = e.Item.Cells(0).Text
        Else
            txtNominativo.Text = ""
        End If

        If e.Item.Cells(19).Text <> "&nbsp;" AndAlso Not String.IsNullOrEmpty(e.Item.Cells(19).Text) Then       'codicefiscale
            txtCodiceFiscale.Text = e.Item.Cells(19).Text
        Else
            txtCodiceFiscale.Text = ""
        End If

        If e.Item.Cells(1).Text <> "&nbsp;" AndAlso Not String.IsNullOrEmpty(e.Item.Cells(1).Text) Then         'datanascita
            txtDataNascita.Text = e.Item.Cells(1).Text
        Else
            txtDataNascita.Text = ""
        End If

        If e.Item.Cells(2).Text <> "&nbsp;" AndAlso Not String.IsNullOrEmpty(e.Item.Cells(2).Text) Then         'denominazione
            txtComune.Text = e.Item.Cells(2).Text
        Else
            txtComune.Text = ""
        End If

        If e.Item.Cells(3).Text <> "&nbsp;" AndAlso Not String.IsNullOrEmpty(e.Item.Cells(3).Text) Then         'telefono
            txtTelefono.Text = e.Item.Cells(3).Text
        Else
            txtTelefono.Text = ""
        End If

        If e.Item.Cells(4).Text <> "&nbsp;" AndAlso Not String.IsNullOrEmpty(e.Item.Cells(4).Text) Then         'email
            txtEmail.Text = e.Item.Cells(4).Text
        Else
            txtEmail.Text = ""
        End If

        hfIdEntePersonaleRuolo.Value = e.Item.Cells(8).Text                             'idEntePersonaleRuolo
        Session("LoadedOlpCV") = Nothing

        checkCorsoOlpSI.Checked = False 'azzero i check
        checkCorsoOlpNO.Checked = False

        'se sto in visualizzazione/modifica (non inserimento) cioè esiste un associazione, setto il check olp e provo a caricare il cv
        If e.Item.Cells(6).Text <> "0" Then             'idAss
            If e.Item.Cells(12).Text = "0" Then         'CorsoOLP
                checkCorsoOlpSI.Checked = False
                checkCorsoOlpNO.Checked = True
            Else
                checkCorsoOlpSI.Checked = True
                checkCorsoOlpNO.Checked = False
            End If
            hfIdAssociaEntePersonaleRuoliAttivitàEntiSediAttuazione.Value = e.Item.Cells(6).Text

            CaricaCV(Integer.Parse(hfIdAssociaEntePersonaleRuoliAttivitàEntiSediAttuazione.Value))

            pMesg.InnerText = "Modifica Risorsa"
            lgContornoPagina.InnerText = "Modifica Risorsa"
            cmdAssegnaRisorsa.Text = "Modifica Risorsa"
        Else
            hfIdAssociaEntePersonaleRuoliAttivitàEntiSediAttuazione.Value = ""
            pMesg.InnerText = "Assegna Risorsa"
            lgContornoPagina.InnerText = "Assegna Risorsa"
            cmdAssegnaRisorsa.Text = "Assegna Risorsa"
        End If

        If Session("LoadedOlpCV") Is Nothing Then
            rowNoCV.Visible = True
            rowCV.Visible = False
        Else
            Dim CV As Allegato = Session("LoadedOlpCV")
            Session("LoadedOlpCV") = CV
            txtCVFilename.Text = CV.Filename
            txtCVHash.Text = CV.Hash
            txtCVData.Text = CV.DataInserimento.ToString("dd/MM/yyyy HH:mm:ss")
            rowNoCV.Visible = False
            rowCV.Visible = True
        End If

        If e.Item.Cells(25).Text = "1" Then 'ControlloUsabilitaCV
            chkObbligoCV.Checked = False
            idPobbligatorioCV.Visible = False
            idPnOobbligatorioCV.Visible = True
        Else
            chkObbligoCV.Checked = True
            idPobbligatorioCV.Visible = True
            idPnOobbligatorioCV.Visible = False
        End If

        popAssegnaRisorsa.Show()
    End Sub

    Private Sub dgRisultatoRicerca_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgRisultatoRicerca.ItemCommand
        Select Case e.CommandName
            Case "Select"
                Response.Write("<script>" & vbCrLf)
                Response.Write("window.open(""WfrmVerificaUsabilitaOLP.aspx?tiporuolo=" & Request.QueryString("tiporuolo") & "&IdAttivita=" & Request.QueryString("IdAttivita") & "&IdAttivitaSedeAttuazione=" & lblidattEs.Value & "&CodiceFiscale=" & e.Item.Cells(19).Text & """, """", ""width=650,height=300,toolbar=no,location=no,menubar=no,scrollbars=yes"")" & vbCrLf)
                Response.Write("</script>")
            Case "SostOLP"
                Dim _red As String = "WfrmSostituzioneOlp.aspx?IdAttivita=" & Request.QueryString("IdAttivita") & "&IdAttivitaSedeAttuazione=" & lblidattEs.Value & "&IdEntePersonaleRuolo=" & e.Item.Cells(8).Text & "&IdEnteSedeAttuazione=" & Request.QueryString("IdSedeAttuazione")
                If e.Item.Cells(22).Text <> "0" Then _red += "&IdSostituzioneOLP=" & e.Item.Cells(22).Text
                Response.Redirect(_red)
            Case "AssOLP"
                ApriPopupAssociaOLP(e)
            Case "CancOLP"
                EliminaAssociazione(e)
        End Select
    End Sub
    Protected Sub cmdChiudi_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdChiudi.Click

        Dim Modifica As Integer
        If Session("TipoUtente") = "E" Then
            Modifica = CInt(ClsUtility.LoadProgettiAbilitaModificaEnte(Request.QueryString("IdAttivita"), Session("Conn")))
        Else
            Modifica = CInt(Request.QueryString("Modifica"))
        End If
        Response.Redirect("WebGestioneSediProgettoOlp.aspx?EnteCapoFila=" & Request.QueryString("EnteCapoFila") & "&CoProgettato=" & Request.QueryString("CoProgettato") & "&Nazionale=" & Request.QueryString("Nazionale") & "&Modifica=" & Modifica & "&IdAttivita=" & Request.QueryString("IdAttivita") + "&VengoDa=" & Request.QueryString("VengoDa"))
    End Sub
    Private Sub ReipostaLabelMessaggi()
        lblMessaggioConferma.Text = String.Empty
        lblErrore.Text = String.Empty
        lblMessaggioInfo.Text = String.Empty
    End Sub
    Private Sub ChekCorsoOlpSiNo()


    End Sub

    Private Function SostOlpABilitata(IdAttivita As Integer, IdEnteSedeAttuazione As Integer, TipoUtente As String) As Boolean

        Dim myCommandSP As New System.Data.SqlClient.SqlCommand
        myCommandSP.CommandText = "SP_SOSTITUZIONE_OLP_ABILITAZIONE"
        myCommandSP.CommandType = CommandType.StoredProcedure

        myCommandSP.Connection = Session("conn")
        myCommandSP.Parameters.AddWithValue("@IdEnteSedeAttuazione", IdEnteSedeAttuazione)
        myCommandSP.Parameters.AddWithValue("@IdAttività", IdAttivita)
        myCommandSP.Parameters.AddWithValue("@TipoUtente", TipoUtente)
        myCommandSP.Parameters.Add("@abilitato", SqlDbType.TinyInt)
        myCommandSP.Parameters("@abilitato").Direction = ParameterDirection.Output
        myCommandSP.ExecuteNonQuery()

        If myCommandSP.Parameters("@abilitato").Value = 1 Then Return True Else Return False

    End Function

    Protected Sub dgRisultatoRicerca_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgRisultatoRicerca.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then

            'Se la colonna SostOLP è visibile ha senso modificare i singoli pulsanti rigaxriga=risorsaxrisorsa
            If dgRisultatoRicerca.Columns(21).Visible Then  ' Colonna SostOLP
                Dim _btnSost As ImageButton = DirectCast(e.Item.FindControl("SostOLP"), ImageButton)
                'se non esiste un associazione (tra entepersonaleruolo e AttivitàEntiSediAttuazione) non ha senso sostituire
                If e.Item.DataItem("idass") = 0 Then
                    _btnSost.Visible = False
                Else
                    _btnSost.Visible = True
                    'se c'è una sostituzione in corso uso un altra icona e cambio alternatetext/tooltip
                    If e.Item.DataItem("inCorso") > 0 Then
                        _btnSost.ImageUrl = "images/incorso_small.png"
                        _btnSost.AlternateText = "Sostituisci OLP (in corso)"
                        _btnSost.ToolTip = "Sostituisci OLP (in corso)"
                    End If
                End If
            End If

            'Se la colonna AssOLP è visibile ha senso modificare i singoli pulsanti rigaxriga=risorsaxrisorsa
            If dgRisultatoRicerca.Columns(23).Visible Then  ' Colonna AssOLP
                Dim _btnAss As ImageButton = DirectCast(e.Item.FindControl("AssOLP"), ImageButton)
                'se non esiste un associazione (tra entepersonaleruolo e AttivitàEntiSediAttuazione) e l'OLP NON è usabile si rende invisibile il pulsante
                If e.Item.DataItem("idass") = 0 AndAlso e.Item.DataItem("ControlloUsabilitaOLP") = 0 Then
                    _btnAss.Visible = False
                Else
                    _btnAss.Visible = True
                    'se esiste un associazione si potrebbe voler cambiare icona/alternatetext/tooltip
                End If
            End If

            'Se la colonna CancOLP è visibile ha senso modificare i singoli pulsanti rigaxriga=risorsaxrisorsa
            If dgRisultatoRicerca.Columns(24).Visible Then  ' Colonna CancOLP
                Dim _btnCanc As ImageButton = DirectCast(e.Item.FindControl("CancOLP"), ImageButton)
                'se non esiste un associazione (tra entepersonaleruolo e AttivitàEntiSediAttuazione) si rende invisibile il pulsante
                If e.Item.DataItem("idass") = 0 Then
                    _btnCanc.Visible = False
                Else
                    _btnCanc.Visible = True
                    'chiedere a danilo quali sono gli stati per cui si può cancellare l'associazione
                End If
            End If

        End If
    End Sub

    Sub ErrorePopupCv(strMessaggio)
        lblErroreUploadCV.Visible = True
        lblErroreUploadCV.Text = strMessaggio
        'Log.Information(LogEvent.STRUTTURA_ORGANIZZATIVA_INFO, "Popup con messaggio", parameters:=strMessaggio) bisognerebbe aggungere log
        popAssegnaRisorsa.Show()
        popUploadCV.Show()
    End Sub

    Protected Sub cmdAllegaCV_Click(sender As Object, e As EventArgs) Handles cmdAllegaCV.Click
        'Verifica se è stato inserito il file
        If fileCV.PostedFile Is Nothing Or String.IsNullOrEmpty(fileCV.PostedFile.FileName) Then
            ErrorePopupCv("Non è stato scelto nessun file per il caricamento del CV")
            Exit Sub
        End If
        'Controllo Tipo File
        If VerificaEstensioneFile(fileCV) = False Then
            ErrorePopupCv("Il formato file del CV non è corretto. È possibile associare solo documenti nel formato .PDF")
            Exit Sub
        End If
        'Controlli dimensioni del file
        Dim fs = fileCV.PostedFile.InputStream
        Dim iLen As Integer = CInt(fs.Length - 1)

        If iLen <= 0 Then
            ErrorePopupCv("Attenzione. Impossibile caricare un file vuoto.")
            Exit Sub
        End If

        If iLen > 20971520 Then
            ErrorePopupCv("Attenzione. La dimensione massima file è di 20 MB.")
            Exit Sub
        End If

        Dim bBLOBStorage = ClsServer.StreamToByte(fs)

        fs.Close()

        Dim hashValue As String
        hashValue = GeneraHash(bBLOBStorage)

        Dim estensione As String = System.IO.Path.GetExtension(fileCV.PostedFile.FileName)
        If UCase(estensione) = ".P7M" Then estensione = ".pdf.p7m"

        'Salvo File In Sessione
        Dim CV As New Allegato() With {
         .Updated = True,
         .Blob = bBLOBStorage,
         .Filename = "OLP_" & txtCodiceFiscale.Text & estensione,
         .Hash = hashValue,
         .Filesize = iLen,
         .DataInserimento = Date.Now
        }
        Session("LoadedOlpCV") = CV

        rowNoCV.Visible = False
        rowCV.Visible = True
        txtCVFilename.Text = CV.Filename
        txtCVHash.Text = CV.Hash
        txtCVData.Text = CV.DataInserimento.ToString("dd/MM/yyyy HH:mm:ss")
        lblErroreAssegnaRisorsa.Visible = False
        popAssegnaRisorsa.Show()
    End Sub

    Protected Sub btnEliminaCV_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles btnEliminaCV.Click
        Session("LoadedOlpCV") = Nothing
        rowNoCV.Visible = True
        rowCV.Visible = False
        popAssegnaRisorsa.Show()
    End Sub

    Protected Sub btnDownloadCV_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles btnDownloadCV.Click
        If Session("LoadedOlpCV") IsNot Nothing Then
            Dim SO As Allegato = Session("LoadedOlpCV")
            Response.Clear()
            Response.ContentType = "Application/pdf"
            Response.AddHeader("Content-Disposition", "attachment; filename=" & SO.Filename)
            Response.BinaryWrite(SO.Blob)
            Response.End()
        End If
    End Sub


    Function VerificaEstensioneFile(ByVal objPercorsoFile As HtmlInputFile, Optional ByVal permettiP7m As Boolean = False) As Boolean
        'sono accettati solo documento con estensione .pdf e .pdf.p7m
        Dim NomeFile As String = ""
        Dim Prefisso() As String
        Dim i As Integer
        VerificaEstensioneFile = False

        'estraggo il nome del file 
        For i = Len(objPercorsoFile.Value) To 1 Step -1
            If InStr(Mid(objPercorsoFile.Value, i, 1), "\") Then
                Exit For
            End If
            NomeFile = Mid(objPercorsoFile.Value, i, 1) & NomeFile
        Next

        If UCase(Right(NomeFile, 4)) = ".PDF" Or (permettiP7m And UCase(Right(NomeFile, 8)) = ".PDF.P7M") Then
            '            Or UCase(Right(NomeFile, 8)) = ".PDF.P7M"         Then
            Return True
        Else
            Return False
        End If

    End Function

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

    Function VerificaAssegnaRisorsa() As String
        Dim _ret As String = ""

        'controllo che sia checcato uno e uno solo tra Corso OLP SI e Corso OLP NO
        If checkCorsoOlpSI.Checked = checkCorsoOlpNO.Checked Then
            If checkCorsoOlpSI.Checked Then
                _ret += "Sono state indicate entrambe le caselle di frequentazione corso OLP." + "<br>"
            Else
                _ret += "Va indicato obbligatoriamente il corso OLP si/no." + "<br>"
            End If
        End If

        'controllo che sia stato inserito il curriculum se non è stata checcato "Curriculum presente nel sistema"
        If rowNoCV.Visible AndAlso chkObbligoCV.Checked AndAlso Session("LoadedOlpCV") Is Nothing Then
            _ret += "Inserire il CV dell'OLP" + "<br>"
        End If

        Return _ret
    End Function

    Protected Sub cmdAssegnaRisorsa_Click(sender As Object, e As EventArgs) Handles cmdAssegnaRisorsa.Click
        Dim _errore As String
        lblErroreAssegnaRisorsa.Visible = False

        _errore = VerificaAssegnaRisorsa()

        If _errore <> "" Then
            lblErroreAssegnaRisorsa.Visible = True
            lblErroreAssegnaRisorsa.CssClass = "msgErrore"
            lblErroreAssegnaRisorsa.Text = _errore
            popAssegnaRisorsa.Show()
            Exit Sub
        End If

        Dim _cv As Allegato = Session("LoadedOlpCV")
        Dim _cvSubentrante As Byte() = Nothing
        Dim _hashCV As String = Nothing
        Dim _filenameCV As String = Nothing

        If Not _cv Is Nothing Then
            _cvSubentrante = _cv.Blob
            _hashCV = _cv.Hash
            _filenameCV = _cv.Filename
        End If

        Try

            _errore = AssociaOLP(If(String.IsNullOrEmpty(hfIdAssociaEntePersonaleRuoliAttivitàEntiSediAttuazione.Value), 0, Integer.Parse(hfIdAssociaEntePersonaleRuoliAttivitàEntiSediAttuazione.Value)),
                                If(String.IsNullOrEmpty(hfIdEntePersonaleRuolo.Value), 0, Integer.Parse(hfIdEntePersonaleRuolo.Value)),
                                Integer.Parse(Request.QueryString("idattES")),
                                Session("Utente"),
                                checkCorsoOlpSI.Checked,
                                chkObbligoCV.Checked,
                                _cvSubentrante,
                                _hashCV,
                                _filenameCV)

            If Not String.IsNullOrEmpty(_errore) Then
                lblErroreAssegnaRisorsa.Visible = True
                lblErroreAssegnaRisorsa.CssClass = "msgErrore"
                lblErroreAssegnaRisorsa.Text = _errore
                popAssegnaRisorsa.Show()
            Else
                Response.Redirect(Request.RawUrl)
            End If

        Catch ex As Exception
            lblErroreAssegnaRisorsa.Visible = True
            lblErroreAssegnaRisorsa.CssClass = "msgErrore"
            lblErroreAssegnaRisorsa.Text = "Errore nel salvataggio"
            popAssegnaRisorsa.Show()
        End Try

    End Sub


    Function AssociaOLP(
                                IdAssociaEntePersonaleRuoliAttivitàEntiSediAttuazione As Integer,
                                IdEntePersonaleRuolo As Integer,
                                IdAttivitàEnteSedeAttuazione As Integer,
                                Username As String,
                                CorsoOLP As Boolean,
                                CaricamentoCV As Boolean,
                                CV As Byte(),
                                HashCV As String,
                                FilenameCV As String
                                ) As String

        Dim sqlCMD As New SqlClient.SqlCommand
        Dim strNomeStore As String = "[SP_OLP_ASSOCIA]"

        sqlCMD = New SqlClient.SqlCommand(strNomeStore, CType(Session("conn"), SqlClient.SqlConnection))
        sqlCMD.CommandType = CommandType.StoredProcedure

        sqlCMD.Parameters.Add("@IdAssociaEntePersonaleRuoliAttivitàEntiSediAttuazione", SqlDbType.Int).Value = IdAssociaEntePersonaleRuoliAttivitàEntiSediAttuazione
        sqlCMD.Parameters.Add("@IdEntePersonaleRuolo", SqlDbType.Int).Value = IdEntePersonaleRuolo
        sqlCMD.Parameters.Add("@IdAttivitàEnteSedeAttuazione", SqlDbType.Int).Value = IdAttivitàEnteSedeAttuazione
        sqlCMD.Parameters.Add("@UsernameCreazioneRecord", SqlDbType.NVarChar, 10).Value = Username
        sqlCMD.Parameters.Add("@CorsoOLP", SqlDbType.Bit, 10).Value = CorsoOLP
        sqlCMD.Parameters.Add("@CaricamentoCV", SqlDbType.Bit, 10).Value = CaricamentoCV
        If CV Is Nothing Then
            sqlCMD.Parameters.Add("@CV", SqlDbType.VarBinary).Value = DBNull.Value
            sqlCMD.Parameters.Add("@HashCV", SqlDbType.VarChar, 100).Value = DBNull.Value
            sqlCMD.Parameters.Add("@FilenameCV", SqlDbType.VarChar, 255).Value = DBNull.Value
        Else
            sqlCMD.Parameters.Add("@CV", SqlDbType.VarBinary, -1).Value = CV
            sqlCMD.Parameters.Add("@HashCV", SqlDbType.VarChar, 100).Value = HashCV
            sqlCMD.Parameters.Add("@FilenameCV", SqlDbType.VarChar, 100).Value = FilenameCV
        End If
        sqlCMD.Parameters.Add("@Errore", SqlDbType.VarChar, -1)
        sqlCMD.Parameters("@Errore").Direction = ParameterDirection.Output

        sqlCMD.ExecuteNonQuery()

        Return sqlCMD.Parameters("@Errore").Value
    End Function

    Sub CaricaCV(IdAssociazione As Integer)
        Try
            Session("LoadedOlpCV") = Nothing

            Dim sqlDAP As New SqlClient.SqlDataAdapter
            Dim dataSet As New DataSet
            Dim strNomeStore As String = "[SP_OLP_CARICA_CV_ASSOCIAZIONE]"

            sqlDAP = New SqlClient.SqlDataAdapter(strNomeStore, CType(Session("conn"), SqlClient.SqlConnection))
            sqlDAP.SelectCommand.CommandType = CommandType.StoredProcedure

            sqlDAP.SelectCommand.Parameters.AddWithValue("@IdAssociaEntePersonaleRuoliAttivitàEntiSediAttuazione", IdAssociazione)

            sqlDAP.Fill(dataSet)

            If dataSet.Tables.Count = 1 AndAlso dataSet.Tables(0).Rows.Count = 1 AndAlso dataSet.Tables(0).Rows(0)("BinData") IsNot DBNull.Value Then

                Dim CV As New Allegato() With {
                 .Updated = False,
                 .Blob = DirectCast(dataSet.Tables(0).Rows(0)("BinData"), Byte()),
                 .Filename = dataSet.Tables(0).Rows(0)("FileName"),
                 .Hash = dataSet.Tables(0).Rows(0)("HashValue"),
                 .Filesize = DirectCast(dataSet.Tables(0).Rows(0)("BinData"), Byte()).Length,
                 .DataInserimento = dataSet.Tables(0).Rows(0)("DataInserimento")
                }
                Session("LoadedOlpCV") = CV

            End If

        Catch ex As Exception
            'se c'è un errore semplicemente non viene caricato il cv
            'andrebbe però loggato
        End Try

    End Sub

    Sub EliminaAssociazione(ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs)
        If e.Item.Cells(6).Text <> "0" Then             'idAss
            Try
                Dim _errore As String
                lblErrore.Visible = False
                EliminaAssociazioneOLP(Integer.Parse(e.Item.Cells(6).Text))

                If Not String.IsNullOrEmpty(_errore) Then
                    lblErrore.Visible = True
                    lblErrore.CssClass = "msgErrore"
                    lblErrore.Text = _errore
                Else
                    Response.Redirect(Request.RawUrl)
                End If

            Catch ex As Exception
                lblErrore.Visible = True
                lblErrore.CssClass = "msgErrore"
                lblErrore.Text = "Errore nell'eliminazione"
            End Try
        Else
            lblErrore.Visible = True
            lblErrore.CssClass = "msgErrore"
            lblErrore.Text = "Nessun associazione da eliminare" 'non dovrebbe mai accadere se il pulsante elimina è visibile nel modo corretto
        End If
    End Sub

    Function EliminaAssociazioneOLP(IdAssociazione As Integer)
        Dim sqlCMD As New SqlClient.SqlCommand
        Dim strNomeStore As String = "[SP_OLP_ELIMINA_ASSOCIAZIONE]"

        sqlCMD = New SqlClient.SqlCommand(strNomeStore, CType(Session("conn"), SqlClient.SqlConnection))
        sqlCMD.CommandType = CommandType.StoredProcedure

        sqlCMD.Parameters.Add("@IdAssociaEntePersonaleRuoliAttivitàEntiSediAttuazione", SqlDbType.Int).Value = IdAssociazione
        sqlCMD.Parameters.Add("@Errore", SqlDbType.VarChar, -1)
        sqlCMD.Parameters("@Errore").Direction = ParameterDirection.Output

        sqlCMD.ExecuteNonQuery()

        Return sqlCMD.Parameters("@Errore").Value
    End Function
End Class