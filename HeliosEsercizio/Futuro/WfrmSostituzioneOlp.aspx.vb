Imports System.Security.Cryptography
Imports Logger.Data
Public Class WfrmSostituzioneOlp
    Inherits SmartPage

    Dim ROSSO As Drawing.Color = System.Drawing.ColorTranslator.FromHtml("#FF9966")
    Dim GIALLO As Drawing.Color = System.Drawing.ColorTranslator.FromHtml("#FFFF99")
    Dim VERDE As Drawing.Color = System.Drawing.ColorTranslator.FromHtml("#99FF99")
    Dim NERO As Drawing.Color = System.Drawing.ColorTranslator.FromHtml("#000000")
    Dim BIANCO As Drawing.Color = System.Drawing.ColorTranslator.FromHtml("#FFFFFF")

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        checkSpid()
        'controllo se è stato effettuato il login
        If Not Session("LogIn") Is Nothing Then
            'se non è stato effettuato login
            If Session("LogIn") = False Then
                'carico la pagina LogOut dove svuoto eventuali session aperte
                Response.Redirect("LogOn.aspx")
            End If
        Else
            'carico la pagina LogOut dove svuoto eventuali session aperte
            Response.Redirect("LogOn.aspx")
        End If

        If IsPostBack = False Then

            If Page.Request.UrlReferrer Is Nothing Then
                hfVengoDa.Value = ""
            Else
                hfVengoDa.Value = Page.Request.UrlReferrer.ToString()
            End If

            CaricaDati()

            CaricaComboProvinciaNazione(chkEsteroNascitaIns.Checked)

            ddlMotivazione.Focus()
        End If

    End Sub

    Sub CaricaDati()
        Dim IdAttivita As Integer = 0
        Dim IdAttivitaSedeAttuazione As Integer = 0
        Dim IdEntePersonaleRuolo As Integer = 0
        Dim IdEnteSedeAttuazione As Integer = 0
        Dim IdSostituzioneOLP As Integer = 0

        Integer.TryParse(Request.QueryString("IdAttivita"), IdAttivita)
        Integer.TryParse(Request.QueryString("IdAttivitaSedeAttuazione"), IdAttivitaSedeAttuazione)
        Integer.TryParse(Request.QueryString("IdEntePersonaleRuolo"), IdEntePersonaleRuolo)
        Integer.TryParse(Request.QueryString("IdEnteSedeAttuazione"), IdEnteSedeAttuazione)
        Integer.TryParse(Request.QueryString("IdSostituzioneOLP"), IdSostituzioneOLP)

        If Not CaricaMaschera(IdAttivita, IdAttivitaSedeAttuazione, IdEntePersonaleRuolo, IdEnteSedeAttuazione, IdSostituzioneOLP) Then
            Response.Redirect("wfrmAnomaliaDati.aspx")
        End If
    End Sub

    Function CaricaMaschera(IdAttivita As Integer, IdAttivitaSedeAttuazione As Integer, IdEntePersonaleRuolo As Integer, IdEnteSedeAttuazione As Integer, IdSostituzioneOLP As Integer) As Boolean
        Dim dt As New DataSet

        If IdAttivita > 0 AndAlso IdAttivitaSedeAttuazione > 0 AndAlso IdEntePersonaleRuolo > 0 AndAlso IdEnteSedeAttuazione > 0 Then

            dt = GetDatiSostituzioneOLP(IdAttivita, IdAttivitaSedeAttuazione, IdEntePersonaleRuolo, IdEnteSedeAttuazione, IdSostituzioneOLP, Session("Utente"), Integer.Parse(Session("IdEnte")))

            If dt.Tables.Count = 1 AndAlso dt.Tables(0).Rows.Count = 1 Then

                txtProgetto.Text = dt.Tables(0).Rows(0)("Titolo")
                txtSede.Text = dt.Tables(0).Rows(0)("Sede")
                txtNome.Text = dt.Tables(0).Rows(0)("Nome")
                txtCognome.Text = dt.Tables(0).Rows(0)("Cognome")
                txtDataNascita.Text = dt.Tables(0).Rows(0)("DataNascita")
                txtComune.Text = dt.Tables(0).Rows(0)("Comune")
                hfIdEntePersonaleRuoloSostituito.Value = dt.Tables(0).Rows(0)("hfIdEntePersonaleRuoloSostituito")
                hfCodiceFiscaleSostituito.Value = dt.Tables(0).Rows(0)("CodiceFiscaleSostituito")

                CaricaDatiSubentro(dt.Tables(0).Rows(0))    'Se il record già esiste carica i dati salvati in precedenza

                If Not String.IsNullOrEmpty(hfIdEntePersonaleRuoloSubentrante.Value) AndAlso Not String.IsNullOrEmpty(hfIdSostituzioneOLP.Value) AndAlso (dt.Tables(0).Rows(0)("Stato") = 1 Or dt.Tables(0).Rows(0)("Stato") = 2) Then
                    cmdElimina.Visible = True
                Else
                    cmdElimina.Visible = False
                End If

                Return True
            Else
                Return False
            End If
        Else
            Return False
        End If

    End Function

    Sub BloccaMaschera()
        ddlMotivazione.Enabled = False
        txtAltraMotivazione.Enabled = False
        btnModificaRINUNCIA.Visible = False
        btnEliminaRINUNCIA.Visible = False
        btnSelezionaOLP.Visible = False
        cmdCaricaFileCV.Visible = False
        btnModificaCV.Visible = False
        btnEliminaCV.Visible = False
        cmdElimina.Visible = False
        cmdConferma.Visible = False

    End Sub

    Private Sub CaricaDatiSubentro(riga As DataRow)

        If riga("IdSostituzioneOLP") IsNot DBNull.Value Then
            hfIdSostituzioneOLP.Value = riga("IdSostituzioneOLP")
        Else
            hfIdSostituzioneOLP.Value = ""
        End If

        If riga("CognomeSub") IsNot DBNull.Value Then
            txtCognomeNew.Text = riga("CognomeSub")
        Else
            txtCognomeNew.Text = ""
        End If

        If riga("NomeSub") IsNot DBNull.Value Then
            txtNomeNew.Text = riga("NomeSub")

        Else
            txtNomeNew.Text = ""
        End If

        If riga("DataNascitaSub") IsNot DBNull.Value Then
            txtDataNascitaNew.Text = riga("DataNascitaSub")
        Else
            txtDataNascitaNew.Text = ""
        End If

        If riga("ComuneSub") IsNot DBNull.Value Then
            txtComuneNew.Text = riga("ComuneSub")
        Else
            txtComuneNew.Text = ""
        End If

        If riga("CodiceFiscaleSub") IsNot DBNull.Value Then
            hfCodiceFiscaleSubentrante.Value = riga("CodiceFiscaleSub")
        Else
            hfCodiceFiscaleSubentrante.Value = ""
        End If

        If riga("IdEntePersonaleRuoloSubentrante") IsNot DBNull.Value Then
            hfIdEntePersonaleRuoloSubentrante.Value = riga("IdEntePersonaleRuoloSubentrante")
            hfIdEntePersonaleSubentrante.Value = riga("IdEntePersonaleSubentrante")
            If Session("TipoUtente") = "U" Then divInfoProgetti.Visible = True
        Else
            hfIdEntePersonaleRuoloSubentrante.Value = ""
            hfIdEntePersonaleSubentrante.Value = ""
            divInfoProgetti.Visible = False
        End If

        If riga("Motivazione") IsNot DBNull.Value Then
            rowAltraMotivazione.Visible = True
            txtAltraMotivazione.Text = riga("Motivazione")
            ddlMotivazione.SelectedValue = "1"
        Else
            rowAltraMotivazione.Visible = False
            txtAltraMotivazione.Text = ""
        End If

        If riga("RinunciaSostituito") IsNot DBNull.Value Then
            rowNoRINUNCIA.Visible = False
            rowRINUNCIA.Visible = True
            ddlMotivazione.SelectedValue = "0"

            Dim RINUNCIA As New Allegato() With {
             .Updated = False,
             .Blob = DirectCast(riga("RinunciaSostituito"), Byte()),
             .Filename = "RINUNCIA_" & hfCodiceFiscaleSostituito.Value & ".pdf",
             .Hash = riga("HashRinuncia"),
             .Filesize = DirectCast(riga("RinunciaSostituito"), Byte()).Length,
             .DataInserimento = riga("DataCreazioneRecord")
            }
            Session("LoadedRINUNCIA") = RINUNCIA
            txtRINUNCIAFilename.Text = RINUNCIA.Filename
            txtRINUNCIAHash.Text = RINUNCIA.Hash
            txtRINUNCIAData.Text = RINUNCIA.DataInserimento.ToString("dd/MM/yyyy HH:mm:ss")
        Else
            rowNoRINUNCIA.Visible = Not rowAltraMotivazione.Visible 'l'eventuale motivazione alternativa è stata caricata prima
            rowRINUNCIA.Visible = False
            txtRINUNCIAFilename.Text = ""
            txtRINUNCIAHash.Text = ""
            txtRINUNCIAData.Text = ""
            Session("LoadedRinuncia") = Nothing   'allegato Rinuncia
        End If

        If riga("CVSubentrante") IsNot DBNull.Value Then
            rowNoCV.Visible = False
            rowCV.Visible = True

            Dim CV As New Allegato() With {
             .Updated = False,
             .Blob = DirectCast(riga("CVSubentrante"), Byte()),
             .Filename = "OLP_" & hfCodiceFiscaleSubentrante.Value & ".pdf",
             .Hash = riga("HashCV"),
             .Filesize = DirectCast(riga("CVSubentrante"), Byte()).Length,
             .DataInserimento = riga("DataCreazioneRecord")
            }
            Session("LoadedOlpCV") = CV
            txtCVFilename.Text = CV.Filename
            txtCVHash.Text = CV.Hash
            txtCVData.Text = CV.DataInserimento.ToString("dd/MM/yyyy HH:mm:ss")
            hfCvObbligatorio.Value = "1" 'visto che c'era do per scontato che fosse obbligatorio. se si deve cambiare bisogna cambiare la stored
        Else
            rowNoCV.Visible = False
            rowCV.Visible = False
            txtCVFilename.Text = ""
            txtCVHash.Text = ""
            txtCVData.Text = ""
            Session("LoadedOlpCV") = Nothing   'allegato CV
            hfCvObbligatorio.Value = "0" 'visto che non c'era do per scontato che non fosse obbligatorio. se si deve cambiare bisogna cambiare la stored
        End If

        'Possibilità di valutare la singola sostituzione
        If riga("Stato") IsNot DBNull.Value AndAlso riga("Stato") > 2 Then
            BloccaMaschera()
            If Session("TipoUtente") = "U" AndAlso riga("StatoIstanza") IsNot DBNull.Value AndAlso riga("StatoIstanza") = 2 Then
                cmdValidaPositivamente.Visible = True
                cmdValidaNegativamente.Visible = True
            Else
                cmdValidaPositivamente.Visible = False
                cmdValidaNegativamente.Visible = False
            End If
        End If

        'Dati stato e motivazione respingimento
        If riga("Stato") IsNot DBNull.Value Then
            Select Case riga("Stato")
                Case 1
                    txtStato.Text = "REGISTRATA"
                Case 2
                    txtStato.Text = "ASSOCIATA"
                Case 3
                    txtStato.Text = "PRESENTATA"
                Case 4
                    txtStato.Text = "APPROVATA"
                Case 5
                    txtStato.Text = "RESPINTA"
            End Select

            If riga("MotivazioneRifiuto") IsNot DBNull.Value AndAlso riga("Stato") = 5 Then
                lblMotivoStato.Visible = True
                txtMotivoStato.Text = riga("MotivazioneRifiuto")
                txtMotivoStato.Visible = True
            Else
                lblMotivoStato.Visible = False
                txtMotivoStato.Text = ""
                txtMotivoStato.Visible = False
            End If
        End If

        'Stato Invisibile se utente ente e istanza non valutata
        If Session("TipoUtente") = "E" AndAlso (riga("StatoIstanza") Is DBNull.Value OrElse riga("StatoIstanza") <> 3) Then
            rowStato.Visible = False
        Else
            rowStato.Visible = True
        End If

    End Sub


    Private Function GetDatiSostituzioneOLP(IdAttivita As Integer, IdAttivitaSedeAttuazione As Integer, IdEntePersonaleRuolo As Integer, IdEnteSedeAttuazione As Integer, IdSostituzioneOLP As Integer, Username As String, IdEnte As Integer) As DataSet
        'ritorna i dati con cui riempire la maschera o niente se i parametri sono sbagliati o non si ha il permesso
        Dim sqlDAP As New SqlClient.SqlDataAdapter
        Dim dataSet As New DataSet
        Dim strNomeStore As String = "[SP_SOSTITUZIONE_OLP_GET_DATI]"

        sqlDAP = New SqlClient.SqlDataAdapter(strNomeStore, CType(Session("conn"), SqlClient.SqlConnection))
        sqlDAP.SelectCommand.CommandType = CommandType.StoredProcedure

        sqlDAP.SelectCommand.Parameters.AddWithValue("@IdAttivita", IdAttivita)
        sqlDAP.SelectCommand.Parameters.AddWithValue("@IdAttivitaSedeAttuazione", IdAttivitaSedeAttuazione)
        sqlDAP.SelectCommand.Parameters.AddWithValue("@IdEntePersonaleRuolo", IdEntePersonaleRuolo)
        sqlDAP.SelectCommand.Parameters.AddWithValue("@IdEnteSedeAttuazione", IdEnteSedeAttuazione)
        sqlDAP.SelectCommand.Parameters.AddWithValue("@IdEnte", IdEnte)
        If IdSostituzioneOLP > 0 Then sqlDAP.SelectCommand.Parameters.AddWithValue("@IdSostituzioneOLP", IdSostituzioneOLP)
        sqlDAP.SelectCommand.Parameters.AddWithValue("@Username", Username)

        sqlDAP.Fill(dataSet)

        Return dataSet
    End Function

    Protected Sub ddlMotivazione_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlMotivazione.SelectedIndexChanged
        lblMessaggioSost.Visible = False
        lblSalva.Visible = False
        If ddlMotivazione.SelectedValue = "0" Then
            If Session("LoadedRinuncia") Is Nothing Then
                rowNoRINUNCIA.Visible = True
                rowRINUNCIA.Visible = False
            Else
                rowNoRINUNCIA.Visible = False
                rowRINUNCIA.Visible = True
            End If
            rowAltraMotivazione.Visible = False
        Else
            rowAltraMotivazione.Visible = True
            rowNoRINUNCIA.Visible = False
            rowRINUNCIA.Visible = False
        End If
    End Sub

    Protected Sub btnDownloadRINUNCIA_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles btnDownloadRINUNCIA.Click
        If Session("LoadedRINUNCIA") Is Nothing Then
            lblMessaggioSost.Visible = True
            lblMessaggioSost.CssClass = "msgErrore"
            lblMessaggioSost.Text = "Nessun File caricato"
            ClearSessionRINUNCIA()
            Exit Sub
        End If
        Dim SO As Allegato = Session("LoadedRINUNCIA")
        Response.Clear()
        Response.ContentType = "Application/pdf"
        Response.AddHeader("Content-Disposition", "attachment; filename=" & SO.Filename)
        Response.BinaryWrite(SO.Blob)
        Response.End()
    End Sub

    Protected Sub btnEliminaRINUNCIA_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles btnEliminaRINUNCIA.Click
        ClearSessionRINUNCIA()
        lblSalva.Visible = False
        'lblMessaggioSost.Visible = True
        'lblMessaggioSost.CssClass = "msgInfo"
        'lblMessaggioSost.Text = "Rinuncia eliminata correttamente"
    End Sub

    Protected Sub cmdConferma_Click(sender As Object, e As EventArgs) Handles cmdConferma.Click
        If Not ValidaCampi() Then Exit Sub

        Try
            Dim _errore As String

            Dim _r As Allegato = Session("LoadedRINUNCIA")
            Dim _cv As Allegato = Session("LoadedOlpCV")

            Dim _rinunciaSostituito As Byte() = Nothing
            Dim _cvSubentrante As Byte() = Nothing
            Dim _hashRinuncia As String = Nothing
            Dim _hashCV As String = Nothing

            If Not _r Is Nothing AndAlso rowAltraMotivazione.Visible = False Then
                _rinunciaSostituito = _r.Blob
                _hashRinuncia = _r.Hash
            End If

            If Not _cv Is Nothing Then
                _cvSubentrante = _cv.Blob
                _hashCV = _cv.Hash
            End If

            Dim _motivazione As String = ""
            If rowAltraMotivazione.Visible Then _motivazione = txtAltraMotivazione.Text

            _errore = SalvaSostituzioneOLP(If(String.IsNullOrEmpty(hfIdSostituzioneOLP.Value), 0, Integer.Parse(hfIdSostituzioneOLP.Value)),
                                            Integer.Parse(Session("IdEnte")),
                                            Integer.Parse(Request.QueryString("IdAttivitaSedeAttuazione")),
                                            Integer.Parse(hfIdEntePersonaleRuoloSostituito.Value),
                                            Integer.Parse(hfIdEntePersonaleRuoloSubentrante.Value),
                                            _cvSubentrante,
                                            _hashCV,
                                            _rinunciaSostituito,
                                            _hashRinuncia,
                                            _motivazione,
                                            Session("Utente"))

            CaricaDati()

            If String.IsNullOrEmpty(_errore) Then
                lblSalva.Visible = True
                lblSalva.CssClass = "msgInfo"
                lblSalva.Text = "Salvataggio effettuato correttamente"
            Else
                lblSalva.Visible = True
                lblSalva.CssClass = "msgErrore"
                lblSalva.Text = _errore
            End If

        Catch ex As Exception
            lblSalva.Visible = True
            lblSalva.CssClass = "msgErrore"
            lblSalva.Text = "Errore nel salvataggio"
        End Try

    End Sub

    Protected Sub cmdChiudi_Click(sender As Object, e As EventArgs) Handles cmdChiudi.Click
        If hfVengoDa.Value Is Nothing OrElse hfVengoDa.Value = "" Then
            Response.Redirect("Main.aspx")
        Else
            Response.Redirect(hfVengoDa.Value)
        End If
    End Sub

    Private Sub MessaggiPopup(ByVal strMessaggio As String, ByRef _label As Label, ByRef _popup As AjaxControlToolkit.ModalPopupExtender)
        _label.Visible = True
        _label.Text = strMessaggio
        'Log.Information(LogEvent.STRUTTURA_ORGANIZZATIVA_INFO, "Popup con messaggio", parameters:=strMessaggio)
        _popup.Show()
    End Sub

    Protected Sub cmdAllegaRINUNCIA_Click(sender As Object, e As EventArgs) Handles cmdAllegaRINUNCIA.Click
        'Verifica se è stato inserito il file
        If fileRINUNCIA.PostedFile Is Nothing Or String.IsNullOrEmpty(fileRINUNCIA.PostedFile.FileName) Then
            MessaggiPopup("Non è stato scelto nessun file per il caricamento della Rinuncia", lblErroreUploadRINUNCIA, popUploadRINUNCIA)
            Exit Sub
        End If
        'Controllo Tipo File
        If VerificaEstensioneFile(fileRINUNCIA) = False Then
            MessaggiPopup("Il formato file della Rinuncia non è corretto. È possibile associare solo documenti nel formato .PDF", lblErroreUploadRINUNCIA, popUploadRINUNCIA)
            Exit Sub
        End If
        'Controlli dimensioni del file
        Dim fs = fileRINUNCIA.PostedFile.InputStream
        Dim iLen As Integer = CInt(fs.Length - 1)

        If iLen <= 0 Then
            MessaggiPopup("Attenzione. Impossibile caricare un file vuoto.", lblErroreUploadRINUNCIA, popUploadRINUNCIA)
            Exit Sub
        End If

        If iLen > 20971520 Then
            MessaggiPopup("Attenzione. La dimensione massima file è di 20 MB.", lblErroreUploadRINUNCIA, popUploadRINUNCIA)
            Exit Sub
        End If

        Dim bBLOBStorage = ClsServer.StreamToByte(fs)

        fs.Close()

        Dim hashValue As String
        hashValue = GeneraHash(bBLOBStorage)

        Dim estensione As String = System.IO.Path.GetExtension(fileRINUNCIA.PostedFile.FileName)
        If UCase(estensione) = ".P7M" Then estensione = ".pdf.p7m"

        'Salvo File In Sessione
        Dim RINUNCIA As New Allegato() With {
         .Updated = True,
         .Blob = bBLOBStorage,
         .Filename = "RINUNCIA_" & hfCodiceFiscaleSostituito.Value & estensione,
         .Hash = hashValue,
         .Filesize = iLen,
         .DataInserimento = Date.Now
        }
        Session("LoadedRINUNCIA") = RINUNCIA
        'Se Il CV è caricato in Sessione (Inserimento)
        rowNoRINUNCIA.Visible = False
        rowRINUNCIA.Visible = True
        txtRINUNCIAFilename.Text = RINUNCIA.Filename
        txtRINUNCIAHash.Text = RINUNCIA.Hash
        txtRINUNCIAData.Text = RINUNCIA.DataInserimento.ToString("dd/MM/yyyy HH:mm:ss")
        lblMessaggioSost.Visible = True
        lblMessaggioSost.CssClass = "msgInfo"
        lblMessaggioSost.Text = "Rinuncia caricata correttamente"
        lblSalva.Visible = False

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

    Private Sub ClearSessionRINUNCIA()
        lblMessaggioSost.Visible = False
        Session("LoadedRINUNCIA") = Nothing
        rowNoRINUNCIA.Visible = True
        rowRINUNCIA.Visible = False
    End Sub

    Private Sub ClearSessionCV()
        lblMessaggioCV.Visible = False
        Session("LoadedOlpCV") = Nothing
        rowNoCV.Visible = If(hfCvObbligatorio.Value = "1", True, False)
        rowCV.Visible = False
    End Sub

    Private Sub CaricaOLP()

        Dim sqlDAP As New SqlClient.SqlDataAdapter
        Dim dataSet As New DataSet
        Dim strNomeStore As String = "[SP_SOSTITUZIONE_OLP_RICERCA]"


        Try
            sqlDAP = New SqlClient.SqlDataAdapter(strNomeStore, CType(Session("conn"), SqlClient.SqlConnection))
            sqlDAP.SelectCommand.CommandType = CommandType.StoredProcedure

            sqlDAP.SelectCommand.Parameters.Add("@idente", SqlDbType.Int).Value = Integer.Parse(Session("IdEnte"))
            sqlDAP.SelectCommand.Parameters.Add("@idattivita", SqlDbType.Int).Value = Integer.Parse(Request.QueryString("IdAttivita"))
            sqlDAP.SelectCommand.Parameters.Add("@identesedeattuazione", SqlDbType.Int).Value = Integer.Parse(Request.QueryString("IdEnteSedeAttuazione"))
            sqlDAP.SelectCommand.Parameters.Add("@idattivitasedeattuazione", SqlDbType.Int).Value = Integer.Parse(Request.QueryString("IdAttivitaSedeAttuazione"))
            sqlDAP.SelectCommand.Parameters.Add("@IdEntePersonaleRuoloSostituito", SqlDbType.Int).Value = Integer.Parse(hfIdEntePersonaleRuoloSostituito.Value)
            If Not String.IsNullOrEmpty(txtNomeRicerca.Text) Then sqlDAP.SelectCommand.Parameters.Add("@nome", SqlDbType.NVarChar).Value = txtNomeRicerca.Text
            If Not String.IsNullOrEmpty(txtCognomeRicerca.Text) Then sqlDAP.SelectCommand.Parameters.Add("@cognome", SqlDbType.NVarChar).Value = txtCognomeRicerca.Text
            If Not String.IsNullOrEmpty(txtComuneRicerca.Text) Then sqlDAP.SelectCommand.Parameters.Add("@comune", SqlDbType.NVarChar).Value = txtComuneRicerca.Text

            sqlDAP.Fill(dataSet)

            Session("DtsRicercaOLP") = dataSet
            dgRisultatoRicerca.DataSource = dataSet
            dgRisultatoRicerca.CurrentPageIndex = 0
            dgRisultatoRicerca.DataBind()

            lblSalva.Visible = False
            lblNota.Visible = True
            lblErroreSelezioneOlpSubentrante.Visible = False
            If dataSet.Tables.Count > 0 AndAlso dataSet.Tables(0).Rows.Count > 50 Then
                lblMessaggioInfo.Text = "Sono stati estratti i primi 50 OLP. Si prega di utilizzare i filtri per ottimizzare la ricerca."
            Else
                lblMessaggioInfo.Text = ""
            End If

            popSelezioneOlpSubentrante.Show()
        Catch ex As Exception
            Response.Write(ex.Message.ToString())
            Exit Sub
        End Try
    End Sub


    Protected Sub btnRicercaOLP_Click(sender As Object, e As EventArgs) Handles btnRicercaOLP.Click
        CaricaOLP()
    End Sub

    Function ControllaSelezione(e As System.Web.UI.WebControls.DataGridCommandEventArgs) As Boolean
        Dim check As CheckBox = DirectCast(e.Item.FindControl("chkCorsoOlp"), CheckBox)
        Dim checkNo As CheckBox = DirectCast(e.Item.FindControl("chkCorsoOlpNo"), CheckBox)

        If check.Checked AndAlso checkNo.Checked Then
            lblErroreSelezioneOlpSubentrante.Text = "Sono state indicate entrambe le caselle di frequentazione corso OLP."
            lblErroreSelezioneOlpSubentrante.CssClass = "msgErrore"
            lblErroreSelezioneOlpSubentrante.Visible = True
            Return False
        End If

        If Not check.Checked AndAlso Not checkNo.Checked Then
            lblErroreSelezioneOlpSubentrante.Text = "Va indicato obbligatoriamente il corso OLP si/no."
            lblErroreSelezioneOlpSubentrante.CssClass = "msgErrore"
            lblErroreSelezioneOlpSubentrante.Visible = True
            Return False
        End If

        Try
            aggiornamentoCorsoOLP(Integer.Parse(e.Item.Cells(7).Text), If(check.Checked, 1, 0))
        Catch ex As Exception
            lblErroreSelezioneOlpSubentrante.Text = "Errore nell'aggiornamento del corso OLP."
            lblErroreSelezioneOlpSubentrante.CssClass = "msgErrore"
            lblErroreSelezioneOlpSubentrante.Visible = True
            Return False
        End Try

        Return True
    End Function


    Protected Sub dgRisultatoRicerca_ItemCommand(source As Object, e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgRisultatoRicerca.ItemCommand
        Select Case e.CommandName
            Case "InfoOLP"
                Response.Write("<script>" & vbCrLf)
                Response.Write("window.open(""WfrmVerificaUsabilitaOLP.aspx?tiporuolo=OLP&IdAttivita=" & Request.QueryString("IdAttivita") & "&IdAttivitaSedeAttuazione=" & Request.QueryString("IdAttivitaSedeAttuazione") & "&CodiceFiscale=" & e.Item.Cells(19).Text & """, """", ""width=650,height=300,toolbar=no,location=no,menubar=no,scrollbars=yes"")" & vbCrLf)
                Response.Write("</script>")
                popSelezioneOlpSubentrante.Show()

            Case "SelezionaOLP"

                If Not ControllaSelezione(e) Then
                    popSelezioneOlpSubentrante.Show()
                    Exit Sub
                End If

                txtCognomeNew.Text = e.Item.Cells(20).Text
                txtNomeNew.Text = e.Item.Cells(21).Text
                txtDataNascitaNew.Text = e.Item.Cells(1).Text
                txtComuneNew.Text = e.Item.Cells(22).Text
                hfIdEntePersonaleRuoloSubentrante.Value = e.Item.Cells(7).Text
                hfIdEntePersonaleSubentrante.Value = e.Item.Cells(23).Text
                hfCodiceFiscaleSubentrante.Value = e.Item.Cells(19).Text

                Dim ChkCaricamentoCV As CheckBox = DirectCast(e.Item.FindControl("ChkCaricamentoCV"), CheckBox)
                hfCvObbligatorio.Value = If(ChkCaricamentoCV.Checked, "1", "0")
                rowNoCV.Visible = If(hfCvObbligatorio.Value = "1", True, False)

                'svuoto campi ricerca
                txtNomeRicerca.Text = ""
                txtCognomeRicerca.Text = ""
                txtComuneRicerca.Text = ""

                'svuoto griglia
                dgRisultatoRicerca.DataSource = Nothing
                dgRisultatoRicerca.DataBind()
                Session("DtsRicercaOLP") = Nothing

                lblNota.Visible = False
                lblErroreSelezioneOlpSubentrante.Visible = False
                lblMessaggioInfo.Text = "Premere il pulsante RICERCA per ottenere la lista degli OLP disponibili."
                'annullo CV precedente
                ClearSessionCV()

        End Select
    End Sub

    Protected Sub btnDownloadCV_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles btnDownloadCV.Click
        If Session("LoadedOlpCV") Is Nothing Then
            lblMessaggioCV.Visible = True
            lblMessaggioCV.CssClass = "msgErrore"
            lblMessaggioCV.Text = "Nessun File caricato"
            ClearSessionCV()
            Exit Sub
        End If
        Dim SO As Allegato = Session("LoadedOlpCV")
        Response.Clear()
        Response.ContentType = "Application/pdf"
        Response.AddHeader("Content-Disposition", "attachment; filename=" & SO.Filename)
        Response.BinaryWrite(SO.Blob)
        Response.End()
    End Sub

    Protected Sub btnEliminaCV_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles btnEliminaCV.Click
        ClearSessionCV()
        lblSalva.Visible = False
        'lblMessaggioCV.Visible = True
        'lblMessaggioCV.CssClass = "msgInfo"
        'lblMessaggioCV.Text = "CV eliminato correttamente"
    End Sub

    Protected Sub cmdAllegaCV_Click(sender As Object, e As EventArgs) Handles cmdAllegaCV.Click
        'Verifica se è stato inserito il file
        If fileCV.PostedFile Is Nothing Or String.IsNullOrEmpty(fileCV.PostedFile.FileName) Then
            MessaggiPopup("Non è stato scelto nessun file per il caricamento del CV", lblErroreUploadCV, popUploadCV)
            Exit Sub
        End If
        'Controllo Tipo File
        If VerificaEstensioneFile(fileCV) = False Then
            MessaggiPopup("Il formato file del CV non è corretto. È possibile associare solo documenti nel formato .PDF", lblErroreUploadCV, popUploadCV)
            Exit Sub
        End If
        'Controlli dimensioni del file
        Dim fs = fileCV.PostedFile.InputStream
        Dim iLen As Integer = CInt(fs.Length - 1)

        If iLen <= 0 Then
            MessaggiPopup("Attenzione. Impossibile caricare un file vuoto.", lblErroreUploadCV, popUploadCV)
            Exit Sub
        End If

        If iLen > 20971520 Then
            MessaggiPopup("Attenzione. La dimensione massima file è di 20 MB.", lblErroreUploadCV, popUploadCV)
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
         .Filename = "OLP_" & hfCodiceFiscaleSubentrante.Value & estensione,
         .Hash = hashValue,
         .Filesize = iLen,
         .DataInserimento = Date.Now
        }
        Session("LoadedOlpCV") = CV
        'Se Il CV è caricato in Sessione (Inserimento)
        lblSalva.Visible = False
        rowNoCV.Visible = False
        rowCV.Visible = True
        txtCVFilename.Text = CV.Filename
        txtCVHash.Text = CV.Hash
        txtCVData.Text = CV.DataInserimento.ToString("dd/MM/yyyy HH:mm:ss")
        lblMessaggioCV.Visible = True
        lblMessaggioCV.CssClass = "msgInfo"
        lblMessaggioCV.Text = "CV caricato correttamente"
    End Sub

    Function ValidaCampi() As Boolean
        'esegue la validazione dei campi prima del salvataggio vero e proprio, restituisce False se qualche campo non è valido
        lblSalva.Visible = False
        Dim strErrori As String = ""

        If ddlMotivazione.SelectedValue = "0" AndAlso Session("LoadedRINUNCIA") Is Nothing Then
            strErrori = "Inserire il PDF Rinuncia" + "<br>"
        End If

        If ddlMotivazione.SelectedValue = "1" AndAlso String.IsNullOrEmpty(txtAltraMotivazione.Text) Then
            strErrori = "Inserire la motivazione della sostituzione" + "<br>"
        End If

        If String.IsNullOrEmpty(hfIdEntePersonaleRuoloSubentrante.Value) Then
            strErrori += "Selezionare l'OLP subentrante" + "<br>"
        End If

        If rowNoCV.Visible AndAlso hfCvObbligatorio.Value = "1" AndAlso Session("LoadedOlpCV") Is Nothing Then
            strErrori += "Inserire il CV dell'OLP subentrante" + "<br>"
        End If

        If String.IsNullOrEmpty(strErrori) Then
            Return True
        Else
            lblSalva.Visible = True
            lblSalva.CssClass = "msgErrore"
            lblSalva.Text = strErrori
            Return False
        End If

    End Function

    Function SalvaSostituzioneOLP(
                                IdSostituzioneOLP As Integer,
                                IdEnte As Integer,
                                IdAttivitàEnteSedeAttuazione As Integer,
                                IdEntePersonaleRuoloSostituito As Integer,
                                IdEntePersonaleRuoloSubentrante As Integer,
                                CVSubentrante As Byte(),
                                HashCV As String,
                                RinunciaSostituito As Byte(),
                                HashRinuncia As String,
                                Motivazione As String,
                                UsernameCreazioneRecord As String) As String

        Dim sqlCMD As New SqlClient.SqlCommand
        Dim strNomeStore As String = "[SP_SOSTITUZIONE_OLP_SALVA]"

        sqlCMD = New SqlClient.SqlCommand(strNomeStore, CType(Session("conn"), SqlClient.SqlConnection))
        sqlCMD.CommandType = CommandType.StoredProcedure

        sqlCMD.Parameters.Add("@IdSostituzioneOLP", SqlDbType.Int).Value = IdSostituzioneOLP
        sqlCMD.Parameters.Add("@IdEnte", SqlDbType.Int).Value = IdEnte
        sqlCMD.Parameters.Add("@IdAttivitàEnteSedeAttuazione", SqlDbType.Int).Value = IdAttivitàEnteSedeAttuazione
        sqlCMD.Parameters.Add("@IdEntePersonaleRuoloSostituito", SqlDbType.Int).Value = IdEntePersonaleRuoloSostituito
        sqlCMD.Parameters.Add("@IdEntePersonaleRuoloSubentrante", SqlDbType.Int).Value = IdEntePersonaleRuoloSubentrante

        If CVSubentrante Is Nothing Then
            sqlCMD.Parameters.Add("@CVSubentrante", SqlDbType.VarBinary).Value = DBNull.Value
            sqlCMD.Parameters.Add("@HashCV", SqlDbType.VarChar, 100).Value = DBNull.Value
        Else
            sqlCMD.Parameters.Add("@CVSubentrante", SqlDbType.VarBinary, -1).Value = CVSubentrante
            sqlCMD.Parameters.Add("@HashCV", SqlDbType.VarChar, 100).Value = HashCV
        End If

        If RinunciaSostituito Is Nothing Then
            sqlCMD.Parameters.Add("@RinunciaSostituito", SqlDbType.VarBinary).Value = DBNull.Value
            sqlCMD.Parameters.Add("@HashRinuncia", SqlDbType.VarChar, 100).Value = DBNull.Value
        Else
            sqlCMD.Parameters.Add("@RinunciaSostituito", SqlDbType.VarBinary, -1).Value = RinunciaSostituito
            sqlCMD.Parameters.Add("@HashRinuncia", SqlDbType.VarChar, 100).Value = HashRinuncia
        End If

        If String.IsNullOrEmpty(Motivazione) Then
            sqlCMD.Parameters.Add("@Motivazione", SqlDbType.NVarChar).Value = DBNull.Value
        Else
            sqlCMD.Parameters.Add("@Motivazione", SqlDbType.NVarChar, 1000).Value = Motivazione
        End If

        sqlCMD.Parameters.Add("@UsernameCreazioneRecord", SqlDbType.VarChar, 50).Value = UsernameCreazioneRecord
        sqlCMD.Parameters.Add("@Errore", SqlDbType.VarChar, 1000)
        sqlCMD.Parameters("@Errore").Direction = ParameterDirection.Output

        sqlCMD.ExecuteNonQuery()

        Return sqlCMD.Parameters("@Errore").Value
    End Function

    Protected Sub cmdElimina_Click(sender As Object, e As EventArgs) Handles cmdElimina.Click

        If Not String.IsNullOrEmpty(hfIdEntePersonaleRuoloSubentrante.Value) AndAlso Not String.IsNullOrEmpty(hfIdSostituzioneOLP.Value) Then
            Dim _errore As String
            Dim _istanzaEliminata As Boolean
            Try

                _errore = EliminaSostituzioneOLP(Integer.Parse(hfIdSostituzioneOLP.Value),
                                    Integer.Parse(Session("IdEnte")), _istanzaEliminata)

                If String.IsNullOrEmpty(_errore) Then

                    If hfVengoDa.Value.IndexOf("WfrmIstanzaSostituzioniOLP.aspx") >= 0 Then
                        If Not _istanzaEliminata Then
                            Response.Redirect(hfVengoDa.Value)
                        Else
                            Response.Redirect("WfrmRicercaIstanzeSostituzioniOLP.aspx?Messaggio=Eliminazione Istanza effettuata correttamente")
                        End If
                    ElseIf hfVengoDa.Value.IndexOf("WebElencoOlp.aspx") > 0 Then
                        Response.Redirect(hfVengoDa.Value)
                    Else
                        CaricaDati()
                        lblSalva.Visible = True
                        lblSalva.CssClass = "msgInfo"
                        lblSalva.Text = "Eliminazione effettuata correttamente"
                    End If
                Else
                    lblSalva.Visible = True
                    lblSalva.CssClass = "msgErrore"
                    lblSalva.Text = _errore
                End If
            Catch ex As Exception
                lblSalva.Visible = True
                lblSalva.CssClass = "msgErrore"
                lblSalva.Text = _errore
            End Try

        Else
            lblSalva.Visible = True
            lblSalva.CssClass = "msgErrore"
            lblSalva.Text = "Errore nell'eliminazione"
        End If
    End Sub

    Private Sub dgRisultatoRicerca_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgRisultatoRicerca.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then

            'Colora celle
            If e.Item.DataItem("ControlloUsabilitaOLP") = 0 Then
                e.Item.BackColor = ROSSO
            Else
                e.Item.BackColor = BIANCO
            End If

            ''Check corso OLP: nota si lascia non checcato, va checcato ogni colta
            'Dim check As CheckBox = DirectCast(e.Item.FindControl("chkCorsoOlp"), CheckBox)
            'Dim checkNo As CheckBox = DirectCast(e.Item.FindControl("chkCorsoOlpNo"), CheckBox)
            'If e.Item.DataItem("CorsoOLP") = 1 Then
            '    check.Checked = True
            '    checkNo.Checked = False
            'Else
            '    check.Checked = False
            '    checkNo.Checked = True
            'End If

            'CheckCV
            Dim ChkCaricamentoCV As CheckBox = DirectCast(e.Item.FindControl("ChkCaricamentoCV"), CheckBox)
            If e.Item.DataItem("EsistenzaCV") = 1 Then
                ChkCaricamentoCV.Checked = True
            Else
                ChkCaricamentoCV.Checked = False
            End If
            If e.Item.DataItem("ControlloUsabilitaCV") = 1 Then
                ChkCaricamentoCV.Enabled = True
                ChkCaricamentoCV.Checked = False
            Else
                ChkCaricamentoCV.Enabled = False
            End If

        End If
    End Sub

    Protected Sub dgRisultatoRicerca_PageIndexChanged(source As Object, e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgRisultatoRicerca.PageIndexChanged
        Dim dataSet As New DataSet
        dgRisultatoRicerca.CurrentPageIndex = e.NewPageIndex
        dataSet = Session("DtsRicercaOLP")
        dgRisultatoRicerca.DataSource = DataSet
        dgRisultatoRicerca.DataBind()
        lblNota.Visible = True
        lblErroreSelezioneOlpSubentrante.Visible = False
        popSelezioneOlpSubentrante.Show()
    End Sub

    Private Sub aggiornamentoCorsoOLP(ByVal identePersonaleRuolo As String, ByVal intCorso As Integer)
        Dim query As String = "update entepersonale set CorsoOLP=" & intCorso & " from entepersonale ep inner join entepersonaleruoli epr on ep.IDEntePersonale=epr.IDEntePersonale where epr.IDEntePersonaleRuolo=" & identePersonaleRuolo.ToString
        Dim sqlCommand As SqlClient.SqlCommand = ClsServer.EseguiSqlClient(query, Session("conn"))
    End Sub

    Function EliminaSostituzioneOLP(
                                IdSostituzioneOLP As Integer,
                                IdEnte As Integer,
                                ByRef IstanzaEliminata As Boolean) As String

        Dim sqlCMD As New SqlClient.SqlCommand
        Dim strNomeStore As String = "[SP_SOSTITUZIONE_OLP_ELIMINA]"

        sqlCMD = New SqlClient.SqlCommand(strNomeStore, CType(Session("conn"), SqlClient.SqlConnection))
        sqlCMD.CommandType = CommandType.StoredProcedure

        sqlCMD.Parameters.Add("@IdSostituzioneOLP", SqlDbType.Int).Value = IdSostituzioneOLP
        sqlCMD.Parameters.Add("@IdEnte", SqlDbType.Int).Value = IdEnte
        sqlCMD.Parameters.Add("@IstanzaEliminata", SqlDbType.Bit)
        sqlCMD.Parameters("@IstanzaEliminata").Direction = ParameterDirection.Output
        sqlCMD.Parameters.Add("@Errore", SqlDbType.VarChar, -1)
        sqlCMD.Parameters("@Errore").Direction = ParameterDirection.Output

        sqlCMD.ExecuteNonQuery()

        IstanzaEliminata = sqlCMD.Parameters("@IstanzaEliminata").Value

        Return sqlCMD.Parameters("@Errore").Value
    End Function

    Function ValidaSostituzioneOLP(
                                IdSostituzioneOLP As Integer,
                                IdEnte As Integer,
                                Username As String,
                                Valida As Boolean, MotivazioneRifiuto As String) As String

        Dim sqlCMD As New SqlClient.SqlCommand
        Dim strNomeStore As String = "[SP_SOSTITUZIONE_OLP_ESITA]"

        sqlCMD = New SqlClient.SqlCommand(strNomeStore, CType(Session("conn"), SqlClient.SqlConnection))
        sqlCMD.CommandType = CommandType.StoredProcedure

        sqlCMD.Parameters.Add("@IdSostituzioneOLP", SqlDbType.Int).Value = IdSostituzioneOLP
        sqlCMD.Parameters.Add("@IdEnte", SqlDbType.Int).Value = IdEnte
        sqlCMD.Parameters.Add("@Username", SqlDbType.NVarChar, 10).Value = Username
        sqlCMD.Parameters.Add("@Esito", SqlDbType.Bit).Value = Valida
        sqlCMD.Parameters.Add("@MotivazioneRifiuto", SqlDbType.VarChar, 1000).Value = MotivazioneRifiuto
        sqlCMD.Parameters.Add("@Errore", SqlDbType.VarChar, -1)
        sqlCMD.Parameters("@Errore").Direction = ParameterDirection.Output

        sqlCMD.ExecuteNonQuery()

        Return sqlCMD.Parameters("@Errore").Value
    End Function

    Protected Sub cmdValidaPositivamente_Click(sender As Object, e As EventArgs) Handles cmdValidaPositivamente.Click
        Dim _errore As String
        Try
            _errore = ValidaSostituzioneOLP(Integer.Parse(hfIdSostituzioneOLP.Value),
                                Integer.Parse(Session("IdEnte")), Session("Utente"), True, "")

            If String.IsNullOrEmpty(_errore) Then
                Response.Redirect(hfVengoDa.Value)
            Else
                lblSalva.Visible = True
                lblSalva.CssClass = "msgErrore"
                lblSalva.Text = _errore
            End If
        Catch ex As Exception
            lblSalva.Visible = True
            lblSalva.CssClass = "msgErrore"
            lblSalva.Text = _errore
        End Try
    End Sub

    Protected Sub ValidaNegativamente()
        Dim _errore As String
        lblErroreMotivoRifiuto.Visible = False

        If String.IsNullOrEmpty(txtMotivoRifiuto.Text) Then
            lblErroreMotivoRifiuto.Visible = True
            lblErroreMotivoRifiuto.CssClass = "msgErrore"
            lblErroreMotivoRifiuto.Text = "Motivo Rifiuto obbligatorio."
            popupMotivoRifiuto.Show()
            Exit Sub
        End If
        Try
            _errore = ValidaSostituzioneOLP(Integer.Parse(hfIdSostituzioneOLP.Value),
                                Integer.Parse(Session("IdEnte")), Session("Utente"), False, txtMotivoRifiuto.Text)

            If String.IsNullOrEmpty(_errore) Then
                Response.Redirect(hfVengoDa.Value)
            Else
                lblErroreMotivoRifiuto.Visible = True
                lblErroreMotivoRifiuto.CssClass = "msgErrore"
                lblErroreMotivoRifiuto.Text = _errore
                popupMotivoRifiuto.Show()
            End If
        Catch ex As Exception
            lblErroreMotivoRifiuto.Visible = True
            lblErroreMotivoRifiuto.CssClass = "msgErrore"
            lblErroreMotivoRifiuto.Text = ex.Message
            popupMotivoRifiuto.Show()
        End Try
    End Sub

    Private Sub CaricaComboProvinciaNazione(ByVal blnEstero As Boolean)
        Dim SelComune As New clsSelezionaComune
        ddlProvinciaNascitaIns = SelComune.CaricaProvinciaNazione(ddlProvinciaNascitaIns, blnEstero, Session("Conn"))

    End Sub

    Protected Sub ddlProvinciaNascitaIns_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlProvinciaNascitaIns.SelectedIndexChanged
        Dim SelComune As New clsSelezionaComune
        ddlComuneNascitaIns.Enabled = True
        ddlComuneNascitaIns = SelComune.CaricaComuniNascita(ddlComuneNascitaIns, ddlProvinciaNascitaIns.SelectedValue, Session("Conn"))
        popInserimentoOlp.Show()       'altrimenti scompare il pupup
    End Sub

    Protected Sub chkEsteroNascitaIns_CheckedChanged(sender As Object, e As EventArgs) Handles chkEsteroNascitaIns.CheckedChanged
        CaricaComboProvinciaNazione(chkEsteroNascitaIns.Checked)
        ddlComuneNascitaIns.DataSource = Nothing
        ddlComuneNascitaIns.Items.Add("")
        ddlComuneNascitaIns.SelectedIndex = 0
        popInserimentoOlp.Show()       'altrimenti scompare il pupup
    End Sub

    Private Function VerificaDatiInseriti() As String

        Dim _ret As String = ""
        Dim _datiPerCfOK = True

        'metto in uppercase CF, COGNOME, e NOME
        txtCognomeIns.Text = txtCognomeIns.Text.ToUpper
        txtNomeIns.Text = txtNomeIns.Text.ToUpper
        txtCodiceFiscaleIns.Text = txtCodiceFiscaleIns.Text.ToUpper

        If txtCognomeIns.Text.Trim = "" Then
            _ret += "Cognome obbligatorio" & "</br>"
            _datiPerCfOK = False
        End If

        If txtNomeIns.Text.Trim = "" Then
            _ret += "Nome obbligatorio" & "</br>"
            _datiPerCfOK = False
        End If

        If txtCodiceFiscaleIns.Text.Trim() = "" Then
            _ret += "Codice Fiscale obbligatorio" & "</br>"
            _datiPerCfOK = False
        End If

        If txtDataNascitaIns.Text.Trim() = "" Then
            _ret += "Data di Nascita obbligatoria" & "</br>"
            _datiPerCfOK = False
        Else
            Dim dataNascita As Date
            Dim data As String = txtDataNascita.Text.Trim()
            If Len(data) <> 10 And data <> "" Then 'il controllo sull'obbligatorietà è stato fatto sopra
                _ret += "Data di nascita non valida. Inserire la data nel formato GG/MM/AAAA" & "</br>"
                _datiPerCfOK = False
            ElseIf (Date.TryParse(data, dataNascita) = False) Then
                _ret += "Data di nascita non valida. Inserire la data nel formato GG/MM/AAAA" & "</br>"
                _datiPerCfOK = False
            End If
        End If

        If ddlProvinciaNascitaIns.Text.Trim() = "" Or ddlProvinciaNascitaIns.Text.Trim() = "0" Then
            _ret += "Provincia di Nascita obbligatoria" & "</br>"
            _datiPerCfOK = False
        End If

        If ddlComuneNascitaIns.Text.Trim() = "" Or ddlComuneNascitaIns.Text.Trim() = "0" Then
            _ret += "Comune di Nascita obbligatorio" & "</br>"
            _datiPerCfOK = False
        End If

        Dim regex As Regex

        'Se estero NON faccio il controllo del codice fiscale
        If chkEsteroNascitaIns.Checked = False Then
            'verifica correttezza codice fiscale: il sesso non fa parte dei campi quindi lo calcolo sia maschile che femminile
            If _datiPerCfOK Then 'il controllo sull'obbligatorietà è stato fatto sopra
                If txtCodiceFiscaleIns.Text.Length <> 16 Then
                    _ret += "Codice Fiscale non corretto" & "</br>"
                    _datiPerCfOK = False
                Else
                    regex = New Regex("^[a-zA-Z0-9]*$")

                    If regex.Match(txtCodiceFiscaleIns.Text).Success = False Then
                        _ret += "Il campo Codice Fiscale contiene caratteri non validi" & "</br>"
                        _datiPerCfOK = False
                    Else
                        Dim strCodCatasto As String
                        Dim strCF_M As String
                        Dim strCF_F As String

                        strCodCatasto = ClsUtility.GetCodiceCatasto(Session("conn"), ddlComuneNascitaIns.SelectedValue)
                        strCF_M = ClsUtility.CreaCF(Trim(Replace(txtCognomeIns.Text, "'", "''")), Trim(Replace(txtNomeIns.Text, "'", "''")), Trim(txtDataNascitaIns.Text), strCodCatasto, "M")
                        strCF_F = ClsUtility.CreaCF(Trim(Replace(txtCognomeIns.Text, "'", "''")), Trim(Replace(txtNomeIns.Text, "'", "''")), Trim(txtDataNascitaIns.Text), strCodCatasto, "F")

                        If txtCodiceFiscaleIns.Text.Trim() <> strCF_M And txtCodiceFiscaleIns.Text.Trim() <> strCF_F Then
                            'Verifico Omocodia
                            If ClsUtility.VerificaOmocodia(UCase(strCF_M), UCase(Trim(txtCodiceFiscaleIns.Text))) Or ClsUtility.VerificaOmocodia(UCase(strCF_F), UCase(Trim(txtCodiceFiscaleIns.Text))) Then
                                _datiPerCfOK = True
                            Else
                                _ret += "Codice Fiscale non corretto" & "</br>"
                                _datiPerCfOK = False
                            End If
                        End If
                    End If
                End If
            End If
            '-- fine correttezza codice fiscale
        End If

        Return _ret
    End Function

    Protected Sub btnInserisciOlp_Click(sender As Object, e As EventArgs) Handles btnInserisciOlp.Click
        Dim _errore As String

        lblErroreInserimentoOlp.Visible = False
        lblErroreInserimentoOlp.Text = ""

        _errore = VerificaDatiInseriti()
        If Not String.IsNullOrEmpty(_errore) Then
            lblErroreInserimentoOlp.Text = _errore
            lblErroreInserimentoOlp.CssClass = "msgErrore"
            lblErroreInserimentoOlp.Visible = True
            popInserimentoOlp.Show()       'altrimenti scompare il pupup
            Exit Sub
        End If

        Try
            _errore = InserisciOLP(Integer.Parse(Session("IdEnte")),
                                    txtCognomeIns.Text,
                                    txtNomeIns.Text,
                                    ddlComuneNascitaIns.SelectedValue,
                                    New Date(Integer.Parse(txtDataNascitaIns.Text.Substring(6, 4)), Integer.Parse(txtDataNascitaIns.Text.Substring(3, 2)), Integer.Parse(txtDataNascitaIns.Text.Substring(0, 2))),
                                    txtCodiceFiscaleIns.Text,
                                   Session("Utente"))

            If String.IsNullOrEmpty(_errore) Then
                'azzero i campi
                txtCognomeIns.Text = ""
                txtNomeIns.Text = ""
                txtCodiceFiscaleIns.Text = ""
                txtDataNascitaIns.Text = ""
                ddlComuneNascitaIns.SelectedIndex = 0
                ddlProvinciaNascitaIns.SelectedIndex = 0
                chkEsteroNascitaIns.Checked = False

                popInserimentoOlp.Hide()
                popSelezioneOlpSubentrante.Show()
            Else
                lblErroreInserimentoOlp.Text = _errore
                lblErroreInserimentoOlp.CssClass = "msgErrore"
                lblErroreInserimentoOlp.Visible = True
                popInserimentoOlp.Show()       'altrimenti scompare il pupup
            End If
        Catch ex As Exception
            lblErroreInserimentoOlp.Text = _errore
            lblErroreInserimentoOlp.CssClass = "msgErrore"
            lblErroreInserimentoOlp.Visible = True
            popInserimentoOlp.Show()       'altrimenti scompare il pupup
        End Try

    End Sub

    Function InserisciOLP(
                                IdEnte As Integer,
                                Cognome As String,
                                Nome As String,
                                IdComuneNascita As Integer,
                                DataNascita As Date,
                                CodiceFiscale As String,
                                Username As String) As String

        Dim sqlCMD As New SqlClient.SqlCommand
        Dim strNomeStore As String = "[SP_SOSTITUZIONE_OLP_INSERT_OLP]"

        sqlCMD = New SqlClient.SqlCommand(strNomeStore, CType(Session("conn"), SqlClient.SqlConnection))
        sqlCMD.CommandType = CommandType.StoredProcedure

        sqlCMD.Parameters.Add("@IdEnte", SqlDbType.Int).Value = IdEnte
        sqlCMD.Parameters.Add("@Cognome", SqlDbType.NVarChar, 255).Value = Cognome
        sqlCMD.Parameters.Add("@Nome", SqlDbType.NVarChar, 255).Value = Nome
        sqlCMD.Parameters.Add("@IdComuneNascita", SqlDbType.Int).Value = IdComuneNascita
        sqlCMD.Parameters.Add("@DataNascita", SqlDbType.DateTime).Value = DataNascita
        sqlCMD.Parameters.Add("@CodiceFiscale", SqlDbType.NVarChar, 50).Value = CodiceFiscale
        sqlCMD.Parameters.Add("@Username", SqlDbType.NVarChar, 10).Value = Username
        sqlCMD.Parameters.Add("@Errore", SqlDbType.VarChar, -1)
        sqlCMD.Parameters("@Errore").Direction = ParameterDirection.Output

        sqlCMD.ExecuteNonQuery()

        Return sqlCMD.Parameters("@Errore").Value
    End Function

    Protected Sub btnCloseInserimentoOlp_Click(sender As Object, e As EventArgs) Handles btnCloseInserimentoOlp.Click
        txtCognomeIns.Text = ""
        txtNomeIns.Text = ""
        txtCodiceFiscaleIns.Text = ""
        txtDataNascitaIns.Text = ""
        chkEsteroNascitaIns.Checked = False
        ddlProvinciaNascitaIns.SelectedIndex = 0
        ddlComuneNascitaIns.SelectedIndex = 0
        popSelezioneOlpSubentrante.Show()
    End Sub

    Protected Sub cmdValidaNegativamente_Click(sender As Object, e As EventArgs) Handles cmdValidaNegativamente.Click
        txtMotivoRifiuto.Text = ""
        popupMotivoRifiuto.Show()
    End Sub

    Protected Sub cmdRifiuta_Click(sender As Object, e As EventArgs) Handles cmdRifiuta.Click
        ValidaNegativamente()
    End Sub
End Class