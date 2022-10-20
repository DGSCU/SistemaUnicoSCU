Imports System.Drawing
Public Class WfrmVisualizzaElencoDocumentiVolontario
    Inherits System.Web.UI.Page
    Dim dtrgenerico As SqlClient.SqlDataReader
    Dim INDEX_DGELENCODOCUMENTI_RIFERIMENTO_TEMPORALE As Byte = 5
    Dim INDEX_DGELENCODOCUMENTI_ORE_FORMAZIONE_SPECIFICA As Byte = 12
    Const DOCUMENTO_ORE_FORMAZIONE_SPECIFICA As String = "FORMSPECVOL"
    Dim INDEX_DGVOLONTARI_TIPODOCUMENTO As Byte = 6

    'specchio  ADC
    Dim INDEX_DGELENCODOCUMENTI_RIFERIMENTO_TEMPORALE_GEN As Byte = 5
    Dim INDEX_DGELENCODOCUMENTI_ORE_FORMAZIONE As Byte = 13
    Const DOCUMENTO_ORE_FORMAZIONE As String = "FORMGENVOL"
    Dim INDEX_DGVOLONTARI_TIPODOCUMENTO_GEN As Byte = 6
    '

    'REALIZZATA DA: SIMONA CORDELLA 
    'DATA REALIZZAZIONE:  20/03/2015
    'FUNZIONALITA': VISUALIZZAZIONE ELENCO DOCUMENTI VOLONTARIO

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Session("LogIn") Is Nothing Then
            If Session("LogIn") = False Then 'verifico validità log-in
                Response.Redirect("LogOn.aspx")
            End If
        Else
            Response.Redirect("LogOn.aspx")
        End If
        '--------------------INIZIO SICUREZZA-----------------------------------
        'IDENTE  Session("IdEnte")
        'CODICEENTE  NZ Session("codiceregione")  or Session("txtCodEnte")

        Dim strATTIVITA As Integer = -1
        Dim strBANDOATTIVITA As Integer = -1
        Dim strENTEPERSONALE As Integer = -1
        Dim strENTITA As Integer = -1
        Dim strIDENTE As Integer = -1

        If ClsUtility.SICUREZZA_VERIFICA_AUTORIZZAZIONI(Session("conn"), Session("IdEnte"), Session("txtCodEnte"), strATTIVITA, strBANDOATTIVITA, strENTEPERSONALE, Request.QueryString("IdVol"), strIDENTE) = 1 Then

            If Not dtrgenerico Is Nothing Then
                dtrgenerico.Close()
                dtrgenerico = Nothing
            End If
        Else
            If Not dtrgenerico Is Nothing Then
                dtrgenerico.Close()
                dtrgenerico = Nothing
            End If
            Response.Redirect("wfrmAnomaliaDati.aspx")

        End If

        '---------------------FINE SICUREZZA---------------------------
        If Page.IsPostBack = False Then
            hlDownload.Visible = False
            CaricaDatVolontario(Request.QueryString("IdVol"))
            CaricaComboTipoDocumenti()
            LoadGriglia()
        End If
    End Sub

    Private Sub CaricaDatVolontario(ByVal IdEntità As Integer)
        Dim strSql As String
        Dim rstVol As SqlClient.SqlDataReader


        ddlTipoDocumenti.Items.Clear()
        strSql = " SELECT entità.CodiceVolontario, entità.Cognome + ' ' +entità. Nome as Volontario,ISNULL(entità.iBAN,'ASSENTE') AS iban, " & _
                 " dbo.FormatoData(entità.DataInizioServizio) as DIS ,dbo.FormatoData(entità.DataFineServizio) as DFS,StatiEntità.StatoEntità  " & _
                 " FROM Entità " & _
                 " INNER JOIN StatiEntità on entità.IDStatoEntità = StatiEntità.IDStatoEntità " & _
                 " WHERE entità.identità = " & IdEntità
        rstVol = ClsServer.CreaDatareader(strSql, Session("conn"))

        If rstVol.HasRows Then
            rstVol.Read()
            lblCodiceVol.Text = rstVol("CodiceVolontario")
            lblVolontario.Text = rstVol("Volontario")
            LblIban.Text = rstVol("Iban")
            LblDataInizioServizio.Text = rstVol("DIS")
            LblDataFineServizio.Text = rstVol("DFS")
            LblStato.Text = rstVol("StatoEntità")
        End If

        If Not rstVol Is Nothing Then
            rstVol.Close()
            rstVol = Nothing
        End If
    End Sub

    Private Sub CaricaComboTipoDocumenti()
        Dim strSql As String
        Dim rstDoc As SqlClient.SqlDataReader


        ddlTipoDocumenti.Items.Clear()
        strSql = "SELECT idPrefisso as idPrefisso, Prefisso as Prefisso, ordine  FROM  PrefissiEntitàDocumenti WHERE TipoInserimento in (0,4) and Sistema='" & Session("Sistema") & "' UNION Select 0, 'Seleziona',0  order by Ordine"
        rstDoc = ClsServer.CreaDatareader(strSql, Session("conn"))


        If rstDoc.HasRows Then
            ddlTipoDocumenti.DataSource = rstDoc
            ddlTipoDocumenti.DataTextField = "Prefisso"
            ddlTipoDocumenti.DataValueField = "IdPrefisso"
            ddlTipoDocumenti.DataBind()
        End If

        If Not rstDoc Is Nothing Then
            rstDoc.Close()
            rstDoc = Nothing
        End If
    End Sub

    Private Sub CaricaElencoDocumenti(ByVal IdEntità As Integer, ByVal TipoDocumento As String)

        'REALIZZATA DA: SIMONA CORDELLA 
        'DATA REALIZZAZIONE:  09/03/2015
        'FUNZIONALITA': RICHIAMO STORE PER CARICAMENTO DELLA GRIGLIA DEI DOCUMENTI

        Dim sqlDAP As New SqlClient.SqlDataAdapter
        Dim dataSet As New DataSet
        Dim strNomeStore As String = "[SP_VOLONTARI_ELENCO_DOCUMENTI]"

        Try
            sqlDAP = New SqlClient.SqlDataAdapter(strNomeStore, CType(Session("conn"), SqlClient.SqlConnection))
            sqlDAP.SelectCommand.CommandType = CommandType.StoredProcedure

            sqlDAP.SelectCommand.Parameters.Add("@IdEntità", SqlDbType.VarChar).Value = IdEntità
            sqlDAP.SelectCommand.Parameters.Add("@TipoDocumento", SqlDbType.VarChar).Value = TipoDocumento
            sqlDAP.Fill(dataSet)

            ' Session("appDtsRisRicerca") = dataSet
            dgElencoDocumenti.DataSource = dataSet
            dgElencoDocumenti.DataBind()

            If Session("Sistema") = "Helios" And Session("TipoUtente") = "E" Then
                dgElencoDocumenti.Columns.Item(8).Visible = False
            End If

        Catch ex As Exception
            LblMsgFile.Visible = True
            LblMsgFile.Text = "Si è verificato un errore non gestito. Contattare l'assistenza."
            LblMsgFile.ForeColor = Color.Red
            Exit Sub
        End Try

    End Sub

    Protected Sub cmdRicerca_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdRicerca.Click
        'dgElencoDocumenti.CurrentPageIndex = 0
        hlDownload.Visible = False
        lblmessaggio.Visible = False
        LoadGriglia()
    End Sub
    Protected Sub cmdChiudi_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdChiudi.Click
        If Request.QueryString("ProVengoDa") = 1 Then
            Response.Redirect("WfrmCheckListDettaglio.aspx?idLista=" & Request.QueryString("IdLista"))
        End If
        If Request.QueryString("ProVengoDa") = 2 Then
            Response.Redirect("WfrmCheckListDettaglioIndividuale.aspx?idLista=" & Request.QueryString("IdLista"))
        End If
        If Request.QueryString("ProVengoDa") = 3 Then
            'Response.Redirect("WfrmRiceraVolontariValidaDocumenti.aspx?")
            Response.Redirect("WfrmCheckListDettaglioRimborsoViaggio.aspx?IdVol=" & Request.QueryString("IdVol") & "&idLista=" & Request.QueryString("IdLista") & "&idEntitaRimborso=" & Request.QueryString("idEntitaRimborso") & "&ProVengoDa=" & 3)
        End If
        If Request.QueryString("ProVengoDa") = 4 Then
            'Response.Redirect("WfrmRiceraVolontariValidaDocumenti.aspx?")
            Response.Redirect("WfrmRiceraVolontariValidaDocumenti.aspx?IdVol=" & Request.QueryString("IdVol") & "&Ente=" & Request.QueryString("Ente") & "&CodEnte=" & Request.QueryString("CodEnte") & "&Cognome=" & Request.QueryString("Cognome") & "&Nome=" & Request.QueryString("Nome") & "&CodVolontario=" & Request.QueryString("CodVolontario") & "&CodProgetto=" & Request.QueryString("CodProgetto") & "&CodSede=" & Request.QueryString("CodSede") & "&Doc=" & Request.QueryString("Doc") & "&StatoDoc=" & Request.QueryString("StatoDoc") & "&DataIS=" & Request.QueryString("DataIS") & "&IdAttivita=" & Request.QueryString("IdAttività") & "&IdEntita=" & Request.QueryString("IdEntita") & "&IdRegione=" & Request.QueryString("IdRegione") & "&ProVengoDa=" & 4)
        End If
        If Request.QueryString("ProVengoDa") = 6 Then
            'Response.Redirect("WfrmRiceraVolontariValidaDocumenti.aspx?")
            Response.Redirect("WfrmCheckListDettaglioFormazione.aspx?IdVol=" & Request.QueryString("IdVol") & "&idLista=" & Request.QueryString("IdLista") & "&Ente=" & Request.QueryString("Ente") & "&CodEnte=" & Request.QueryString("CodEnte") & "&Cognome=" & Request.QueryString("Cognome") & "&Nome=" & Request.QueryString("Nome") & "&CodVolontario=" & Request.QueryString("CodVolontario") & "&CodProgetto=" & Request.QueryString("CodProgetto") & "&CodSede=" & Request.QueryString("CodSede") & "&Doc=" & Request.QueryString("Doc") & "&StatoDoc=" & Request.QueryString("StatoDoc") & "&DataIS=" & Request.QueryString("DataIS") & "&IdAttivita=" & Request.QueryString("IdAttivita") & "&IdEntita=" & Request.QueryString("IdEntita") & "&IdRegione=" & Request.QueryString("IdRegione") & "&ProVengoDa=" & 6)
        End If
        If Request.QueryString("VengoDa") Is Nothing Then
            Response.Redirect("WfrmVolontari.aspx?IdVol=" & Request.QueryString("IdVol") & "&idattivita=" & Request.QueryString("idattivita"))
        Else
            Response.Redirect("ricercavolontaridocumenti.aspx")
        End If
    End Sub

    Private Sub dgElencoDocumenti_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgElencoDocumenti.ItemCommand
        Dim strSql As String
        Dim rstDoc As SqlClient.SqlDataReader
        Dim msg As String
        Dim Esito As String
        Select Case e.CommandName
            Case "Download"
                hlDownload.Visible = True
                hlDownload.NavigateUrl = clsGestioneDocumenti.RecuperaDocumentoVolontario(e.Item.Cells(0).Text, Session("Utente"), Session("conn"))
                hlDownload.Text = e.Item.Cells(2).Text
                hlDownload.Target = "_blank"
            Case "Elimina"
                If e.Item.Cells(7).Text = "Da Validare" Then
                    'ANTONELLO and Sistema='" & Session("Sistema") & "'
                    strSql = " SELECT idPrefisso as idPrefisso, Prefisso as Prefisso, ordine  " & _
                             " FROM  PrefissiEntitàDocumenti where tipoinserimento=0 and Sistema='" & Session("Sistema") & "' " & _
                             " and left(PrefissiEntitàDocumenti.prefisso, charindex('_',PrefissiEntitàDocumenti.Prefisso)-1) = '" & e.Item.Cells(6).Text & "' "
                    rstDoc = ClsServer.CreaDatareader(strSql, Session("conn"))
                    If rstDoc.HasRows = True Then
                        If Not rstDoc Is Nothing Then
                            rstDoc.Close()
                            rstDoc = Nothing
                        End If
                        'cancello la sede 
                        clsGestioneDocumenti.RimuoviPresenze(e.Item.Cells(0).Text, Session("Conn"))
                        'messaggio di conferma cancellazione
                        lblmessaggio.Visible = True
                        lblmessaggio.Text = "Cancellazione Effettuata"
                        lblmessaggio.ForeColor = System.Drawing.ColorTranslator.FromHtml("#3a4f63")
                        LblMsgFile.Text = ""
                        'load griglia
                        LoadGriglia()
                    Else
                        If Not rstDoc Is Nothing Then
                            rstDoc.Close()
                            rstDoc = Nothing
                        End If
                        LblMsgFile.Text = ""
                        lblmessaggio.Visible = True
                        lblmessaggio.Text = "Impossibile cancellare il documento selezionato."
                        lblmessaggio.ForeColor = System.Drawing.ColorTranslator.FromHtml("#3a4f63")
                    End If
                Else
                    LblMsgFile.Text = ""
                    lblmessaggio.Visible = True
                    lblmessaggio.Text = "Impossibile cancellare il documento selezionato."
                    lblmessaggio.ForeColor = System.Drawing.ColorTranslator.FromHtml("#3a4f63")
                End If

            Case "Valida"
                If VerificoEsistenzaCheckList(CInt(e.Item.Cells(0).Text)) = True Then
                    lblmessaggio.Visible = True
                    lblmessaggio.Text = "Impossibile modificare lo stato del documento perchè associato ad una CheckList Confermata."
                    lblmessaggio.ForeColor = System.Drawing.ColorTranslator.FromHtml("#C00000")
                Else
                    msg = ClsServer.ConfermaDocumento(Session("Utente"), CInt(e.Item.Cells(0).Text), 1, e.Item.Cells(6).Text, Session("IdEnte"), Session("Conn"), msg, Esito)
                    If Esito = "NEGATIVO" Then
                        lblmessaggio.ForeColor = System.Drawing.ColorTranslator.FromHtml("#C00000")
                    Else
                        lblmessaggio.ForeColor = System.Drawing.ColorTranslator.FromHtml("#3a4f63")
                    End If
                    lblmessaggio.Text = msg
                    'lblmessaggio.ForeColor = System.Drawing.ColorTranslator.FromHtml("#3a4f63")
                    lblmessaggio.Visible = True
                    LblMsgFile.Text = ""

                    LoadGriglia()
                End If
            Case "NonValida"
                If VerificoEsistenzaCheckList(CInt(e.Item.Cells(0).Text)) = True Then
                    lblmessaggio.Visible = True
                    lblmessaggio.Text = "Impossibile modificare lo stato del documento perchè associato ad una CheckList Confermata."
                    lblmessaggio.ForeColor = System.Drawing.ColorTranslator.FromHtml("#C00000")
                Else
                    msg = ClsServer.ConfermaDocumento(Session("Utente"), e.Item.Cells(0).Text, 2, e.Item.Cells(6).Text, Session("IdEnte"), Session("Conn"))

                    If Esito = "NEGATIVO" Then
                        lblmessaggio.ForeColor = System.Drawing.ColorTranslator.FromHtml("#C00000")
                    Else
                        lblmessaggio.ForeColor = System.Drawing.ColorTranslator.FromHtml("#3a4f63")
                    End If

                    LoadGriglia()
                    lblmessaggio.Visible = True
                    lblmessaggio.Text = msg
                    lblmessaggio.ForeColor = System.Drawing.ColorTranslator.FromHtml("#3a4f63")
                    LblMsgFile.Text = ""
                End If
            Case "Dettaglio"
                If e.Item.Cells(6).Text = "PRESENZE" Then
                    Dim MyDataset As DataSet
                    Dim anno As String
                    Dim mese As String
                    strSql = "SELECT * from EntitàDocumenti where IdEntitàDocumento=" & e.Item.Cells(0).Text
                    MyDataset = ClsServer.DataSetGenerico(strSql, Session("conn"))

                    If MyDataset.Tables(0).Rows.Count <> 0 Then
                        anno = MyDataset.Tables(0).Rows(0).Item("anno")
                        mese = MyDataset.Tables(0).Rows(0).Item("mese")
                    End If
                    MyDataset.Dispose()

                    'Richiamo dettaglio presenze
                    Response.Redirect("Presenze.aspx?VengoDa=DocumentiVolontario&Mese=" & mese & "&Anno=" & anno & "&IdEntita=" & Request.QueryString("IdVol"))
                Else
                    'ricavo l'idattività del volontario
                    'select aesa.IDAttività from attivitàentità 			inner join attivitàentisediattuazione aesa on aesa.IDAttivitàEnteSedeAttuazione =attivitàentità.IDAttivitàEnteSedeAttuazione                    where(attivitàentità.IDEntità = 826000)
                    Dim MyDataset As DataSet
                    Dim IDAttività As String

                    strSql = " SELECT aesa.IDAttività FROM Attivitàentità 	" & _
                             " INNER JOIN Attivitàentisediattuazione aesa on aesa.IDAttivitàEnteSedeAttuazione =attivitàentità.IDAttivitàEnteSedeAttuazione  " & _
                             " WHERE Attivitàentità.IDEntità =" & Request.QueryString("IdVol")
                    MyDataset = ClsServer.DataSetGenerico(strSql, Session("conn"))

                    If MyDataset.Tables(0).Rows.Count <> 0 Then
                        IDAttività = MyDataset.Tables(0).Rows(0).Item("IDAttività")
                    End If
                    MyDataset.Dispose()


                    'richiamo dettaglio rimborsi
                    Response.Redirect("WfrmGestioneRimborsoVolontari.aspx?VengoDa=DocumentiVolontario&IdAttivita=" & IDAttività & "&IdEntita=" & Request.QueryString("IdVol") & "&Op=" & Request.QueryString("Op"))
                End If
        End Select

    End Sub

    Protected Sub cmdUpload_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdUpload.Click
        Try
            Dim msg As String
            Dim PrefissoFile As String = ""
            LblMsgFile.Text = ""
            lblmessaggio.Text = ""
            'hlScarica.Visible = False
            hlDownload.Visible = False
            If txtSelFile.Value = "" Then
                LblMsgFile.Visible = True
                LblMsgFile.Text = "E' necessario selezionare il file."
                LblMsgFile.ForeColor = Color.Red
                Exit Sub
            End If
            If clsGestioneDocumenti.VerificaEstensioneFileVolontario(txtSelFile) = False Then
                LblMsgFile.Visible = True
                LblMsgFile.Text = "Il formato del file non è corretto.E' possibile associare documenti nel formato .PDF o .PDF.P7M"
                LblMsgFile.ForeColor = Color.Red
                Exit Sub
            End If
            If clsGestioneDocumenti.VerificaPrefissiDocumentoVolontario(txtSelFile, Session("conn"), PrefissoFile, Session("Sistema")) = False Then
                LblMsgFile.Visible = True
                LblMsgFile.Text = "Utilizzare uno dei prefissi consentiti per il nome del file."
                LblMsgFile.ForeColor = Color.Red
                Exit Sub
            End If


            '*** modifcato da simona cordella il 04/05/2016
            Dim Esito As String = ""
            Dim Messaggio As String = ""
            '** controllo se il prefisso del file(AUTOCERTIFICAZIONEREQUISITI,IDENTITA',PRESEINCARICO) sono da caricare in base al bando del volontario
            clsGestioneDocumenti.VolontarioVerificaPrefisso(Request.QueryString("IdVol"), PrefissoFile, Session("conn"), Esito, Messaggio)
            If Esito = "NEGATIVO" Then
                LblMsgFile.Visible = True
                LblMsgFile.Text = Messaggio
                LblMsgFile.ForeColor = Color.Red
                Exit Sub
            End If
            '******


            msg = clsGestioneDocumenti.CaricaDocumentoEntità(Request.QueryString("IdVol"), Session("Utente"), txtSelFile, Session("conn"), PrefissoFile)
            If msg = "ok" Then
                LblMsgFile.Visible = True
                LblMsgFile.Text = "Documento Associato"
                LblMsgFile.ForeColor = System.Drawing.ColorTranslator.FromHtml("#3a4f63")
            Else
                LblMsgFile.Visible = True
                LblMsgFile.Text = msg
                LblMsgFile.ForeColor = Color.Red
            End If

            CaricaElencoDocumenti(Request.QueryString("IdVol"), ddlTipoDocumenti.SelectedValue)

        Catch ex As Exception
            LblMsgFile.Visible = True
            LblMsgFile.Text = "Si è verificato un errore non gestito. Contattare l'assistenza."
            LblMsgFile.ForeColor = Color.Red
        Finally
            cmdUpload.Enabled = True
        End Try

    End Sub

    Sub LoadGriglia()
        CaricaElencoDocumenti(Request.QueryString("IdVol"), ddlTipoDocumenti.SelectedValue)
        If dgElencoDocumenti.Items.Count = 0 Then
            lblmessaggio.Visible = True
            lblmessaggio.Text = "Nessun documento estratto."
        End If

        If Session("TipoUtente") = "U" Then

            imgStoricoNotifiche.Visible = True

            dgElencoDocumenti.Columns(4).Visible = False 'hashvalue
            If VerificaAbilitazioneMenuValidazioneDocumenti(Session("Utente")) = True Then
                dgElencoDocumenti.Columns(8).Visible = True 'pulsante "valida documento"
                dgElencoDocumenti.Columns(9).Visible = True 'pulsante "respingi documento"
                dgElencoDocumenti.Columns(10).Visible = True 'pulsante "DETTAGLIO documento"
                CmdNotifica.Visible = True
                'notifica visibile
            Else
                dgElencoDocumenti.Columns(8).Visible = False 'pulsante "valida documento"
                dgElencoDocumenti.Columns(9).Visible = False 'pulsante "respingi documento"
                dgElencoDocumenti.Columns(10).Visible = False 'pulsante "DETTAGLIO documento"
                CmdNotifica.Visible = False
                'notifica invisibile
            End If
        End If
        If Session("TipoUtente") = "E" Then
            dgElencoDocumenti.Columns(4).Visible = True 'hashvalue
        End If
        If Session("Sistema") = "Helios" Then
            dgElencoDocumenti.Columns(5).Visible = False 'hashvalue
        End If
    End Sub
    Private Function VerificaAbilitazioneMenuValidazioneDocumenti(ByVal Utente As String) As Boolean
        'Agg da  Simona Cordella il 30/04/2015
        'Verifico se l'utene U è autorizzato alla visualizzazione del menu ValidazioneDocumenti
        Dim strSql As String
        Dim dtrgenerico As SqlClient.SqlDataReader
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
        'Verifica menu sicurezza su funzione accredita
        strSql = "SELECT AssociaUtenteGruppo.username, VociMenu.IdVoceMenu, VociMenu.VoceMenu, VociMenu.ImmaginePassiva, VociMenu.ImmagineAttiva, VociMenu.Link," & _
                 " VociMenu.IdVoceMenuPadre" & _
                 " FROM VociMenu " & _
                 " INNER JOIN AbilitazioniProfiliVociMenu ON VociMenu.IdVoceMenu = AbilitazioniProfiliVociMenu.IdVoceMenu" & _
                 " INNER JOIN Profili ON AbilitazioniProfiliVociMenu.IdProfilo = Profili.IdProfilo" & _
                 " INNER JOIN AssociaUtenteGruppo ON Profili.IdProfilo = AssociaUtenteGruppo.IdProfilo" & _
                 " LEFT JOIN  RestrizioniUtenteVociMenu ON AssociaUtenteGruppo.UserName = RestrizioniUtenteVociMenu.UserName and RestrizioniUtenteVociMenu.IdVoceMenu=VociMenu.IdVoceMenu" & _
                 " WHERE VociMenu.descrizione = 'Validazione Documenti'" & _
                 " AND AssociaUtenteGruppo.username ='" & Utente & "' and (RestrizioniUtenteVociMenu.TipoRestrizione <> 0 or RestrizioniUtenteVociMenu.TipoRestrizione is null)"
        dtrgenerico = ClsServer.CreaDatareader(strSql, Session("conn"))

        VerificaAbilitazioneMenuValidazioneDocumenti = dtrgenerico.Read()
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
    End Function
    Protected Sub OpenWindow()

        Dim url As String = "WfrmCheckListNotificaMailPresenze.aspx?IdVol=" & Request.QueryString("IdVol") & "&VengoDa=" & 4

        Dim s As String = "window.open('" & url + "', 'popup_window', 'width=800,height=600,left=100,top=100,resizable=yes');"

        ClientScript.RegisterStartupScript(Me.GetType(), "script", s, True)

    End Sub
    Private Sub CmdNotifica_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdNotifica.Click
        OpenWindow()
    End Sub

    Private Sub dgElencoDocumenti_ItemDataBound(ByVal source As Object, ByVal e As DataGridItemEventArgs) Handles dgElencoDocumenti.ItemDataBound

        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            If String.Equals(e.Item.Cells(INDEX_DGVOLONTARI_TIPODOCUMENTO).Text, DOCUMENTO_ORE_FORMAZIONE_SPECIFICA, StringComparison.InvariantCultureIgnoreCase) Then
                e.Item.Cells(INDEX_DGELENCODOCUMENTI_RIFERIMENTO_TEMPORALE).Text = e.Item.Cells(INDEX_DGELENCODOCUMENTI_ORE_FORMAZIONE_SPECIFICA).Text
                e.Item.Cells(INDEX_DGELENCODOCUMENTI_RIFERIMENTO_TEMPORALE).Visible = True
            End If
            If String.Equals(e.Item.Cells(INDEX_DGVOLONTARI_TIPODOCUMENTO_GEN).Text, DOCUMENTO_ORE_FORMAZIONE, StringComparison.InvariantCultureIgnoreCase) Then
                e.Item.Cells(INDEX_DGELENCODOCUMENTI_RIFERIMENTO_TEMPORALE_GEN).Text = e.Item.Cells(INDEX_DGELENCODOCUMENTI_ORE_FORMAZIONE).Text
                e.Item.Cells(INDEX_DGELENCODOCUMENTI_RIFERIMENTO_TEMPORALE_GEN).Visible = True
            End If
        End If
    End Sub

    Private Function VerificoEsistenzaCheckList(ByVal IdEntitàDocumento As Integer) As Boolean

        Dim strsql As String
        Dim MyDataset As DataSet
        ' Dim identitadocumento As Integer
        Dim blnVerifica As Boolean = False


        'strsql = " Select EntitàDocumenti.IdEntitàDocumento as IdEntitàDocumento,EntitàRimborsi.identità " & _
        '        " from EntitàRimborsi  " & _
        '        " inner join EntitàDocumenti  on EntitàRimborsi.IDEntitàRimborso =EntitàDocumenti.IdEntitàRimborso " & _
        '        " where EntitàRimborsi.IdEntitàDocumento=" & IdEntitàDocumento
        'MyDataset = ClsServer.DataSetGenerico(strsql, Session("conn"))
        'If MyDataset.Tables(0).Rows.Count <> 0 Then
        '    identitadocumento = MyDataset.Tables(0).Rows(0).Item("IdEntitàDocumento")
        '    MyDataset.Dispose()
        strsql = " Select * from VW_CHECKLIST_DOCUMENTI_VOLONTARI where IdEntitàDocumento=" & IdEntitàDocumento
        MyDataset = ClsServer.DataSetGenerico(strsql, Session("conn"))
        If MyDataset.Tables(0).Rows.Count <> 0 Then
            blnVerifica = True
        End If

        'End If
        MyDataset.Dispose()
        Return blnVerifica
    End Function



    Private Sub imgStoricoNotifiche_Click(sender As Object, e As System.EventArgs) Handles imgStoricoNotifiche.Click
        'Response.Redirect("WfrmReportistica.aspx?sTipoStampa=36&IdBandoAttivita=" & txtidbandoAttivita.Text)
        Dim JScript As String

        JScript = "<script>" & vbCrLf
        JScript &= "window.open(""WfrmVisualizzaStoricoNotifiche.aspx?IdTipoNotifica=1&IdEntita=" & Request.QueryString("IdVol") & """, """", ""height=768,width=1024, ,dependent=no,scrollbars=yes,status=no,resizable=yes"")" & vbCrLf
        JScript &= ("</script>")
        Response.Write(JScript)
    End Sub

    Private Function TrovaBandoVolontario(ByVal IdEntità As Integer) As Boolean
        Dim strsql As String
        Dim rstBando As SqlClient.SqlDataReader

        strsql = " SELECT DISTINCT BandiAttività.IdBando"
        strsql &= "FROM         entità "
        strsql &= " INNER JOIN attivitàentità ON entità.IDEntità = attivitàentità.IDEntità "
        strsql &= " INNER JOIN attivitàentisediattuazione ON attivitàentità.IDAttivitàEnteSedeAttuazione = attivitàentisediattuazione.IDAttivitàEnteSedeAttuazione "
        strsql &= "INNER JOIN attività ON attivitàentisediattuazione.IDAttività = attività.IDAttività "
        strsql &= "INNER JOIN BandiAttività ON attività.IDBandoAttività = BandiAttività.IdBandoAttività"
        strsql &= "WHERE entità.IDEntità= " & IdEntità & ""

    End Function

    Protected Sub imgPrefissiDocumenti_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles imgPrefissiDocumenti.Click

    End Sub
End Class