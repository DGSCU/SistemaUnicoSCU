Imports System.Data.SqlClient
Imports System.IO

Public Class wfrmDocumentiEnti
    Inherits System.Web.UI.Page
    Dim myDataSet As New DataSet
    Dim myDataTable As DataTable
    Dim myDataReader As SqlClient.SqlDataReader
    Dim querySql As String

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
    Private Sub CancellaMessaggiInfo()
        msgErrore.Text = String.Empty
        msgInfo.Text = String.Empty
        msgConferma.Text = String.Empty
    End Sub
    Private Sub NascondiMenuLaterale()
        Session("TP") = True
    End Sub

    Private Function VerificaAbilitazione(ByVal IdEnte As String, ByVal IdEnteFase As String, ByVal Conn As SqlClient.SqlConnection) As Boolean

        '** Verifico se l'ente in sessione è coerente con la fase selezionata
        Dim strSql As String
        Dim dtrgenerico As SqlClient.SqlDataReader
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If

        'Verifica menu sicurezza su funzione accredita
        strSql = "SELECT identefase from entifasi where idente = " & IdEnte & " and identefase = " & IdEnteFase & " and getdate() between datainiziofase and datafinefase"
         dtrgenerico = ClsServer.CreaDatareader(strSql, Conn)

        VerificaAbilitazione = dtrgenerico.Read()
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
        Return VerificaAbilitazione
    End Function

#End Region
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        VerificaSessione()

        If Page.IsPostBack = False Then
            If VerificaAbilitazione(Session("IdEnte"), Request.QueryString("IdEnteFase"), Session("conn")) = False Then
                Response.Redirect("wfrmAnomaliaDati.aspx")
            End If
            hf_IdEnteFase.Value = Request.QueryString("IdEnteFase")
            CaricaGriglia()
            CaricaIntestazioni()
        End If
    End Sub
    Sub CaricaGriglia()
        ChiudiDataReader(myDataReader)
        querySql = "SELECT IdEnteDocumento, FileName, DataInserimento,hashvalue,firmanecessaria FROM EntiDocumenti a " & _
                " left join PrefissiEntiDocumenti b on left(a.filename, charindex('_',a.filename)) = b.prefisso  " & _
                " where identefase =" & hf_IdEnteFase.Value & " " & _
                " order by isnull(b.ordine,99) "
        Session("sqlDataSet") = ClsServer.DataSetGenerico(querySql, Session("conn"))

        dtgAttivitaDocumenti.DataSource = Session("sqlDataSet")
        dtgAttivitaDocumenti.DataBind()
        If dtgAttivitaDocumenti.Items.Count = 0 Then
            imgEsporta.Visible = False
        Else
            imgEsporta.Visible = True
        End If
    End Sub
    Sub CaricaIntestazioni()
        ChiudiDataReader(myDataReader)
        querySql = "SELECT enti.Denominazione, ISNULL(enti.CodiceRegione,'') as CodiceRegione, " & _
                    " EntiFasi.TipoFase, EntiFasi.DataInizioFase, EntiFasi.DataFineFase FROM enti INNER JOIN EntiFasi " & _
                    " ON enti.IDEnte = EntiFasi.IdEnte WHERE EntiFasi.IdEnteFase = " & hf_IdEnteFase.Value & ""

        myDataReader = ClsServer.CreaDatareader(querySql, Session("conn"))
        If myDataReader.HasRows = True Then
            myDataReader.Read()
            lblDenominazioneEnte.Text = myDataReader("Denominazione")
            lblEnte.Text = myDataReader("CodiceRegione")
            lblTipoFase.Text = myDataReader("TipoFase")
            Select Case lblTipoFase.Text
                Case 1
                    lblTipoFase.Text = "Accreditamento"
                Case 2
                    lblTipoFase.Text = "Adeguamento"
                Case 3
                    lblTipoFase.Text = "Articolo 2"
                Case 4
                    lblTipoFase.Text = "Articolo 10"
            End Select
            lblDataInizioFase.Text = myDataReader("DataInizioFase")
            lblDataFineFase.Text = myDataReader("DataFineFase")
            LblIdEnteFase.Text = hf_IdEnteFase.Value
            ChiudiDataReader(myDataReader)
        End If

    End Sub
    Private Sub dtgAttivitaDocumenti_PageIndexChanged(ByVal source As System.Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dtgAttivitaDocumenti.PageIndexChanged
        dtgAttivitaDocumenti.CurrentPageIndex = e.NewPageIndex
        dtgAttivitaDocumenti.DataSource = Session("sqlDataSet")
        dtgAttivitaDocumenti.DataBind()
        dtgAttivitaDocumenti.SelectedIndex = -1
        dtgAttivitaDocumenti.EditItemIndex = -1
    End Sub
    Private Sub cmdUpload_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles cmdUpload.Click
        CancellaMessaggiInfo()
        Try
            Dim msg As String
            Dim PrefissoFile As String = ""
            Dim blnBloccoAccreditamento As Boolean
            hlScarica.Visible = False
            hlDw.Visible = False
            blnBloccoAccreditamento = False

            Dim AlboEnte As String
            AlboEnte = ClsUtility.TrovaAlboEnte(Session("IdEnte"), Session("Conn"))

            If clsGestioneDocumentiAccreditamento.VerificaEstensioneFileAccreditamento(txtSelFile) = False Then
                msgErrore.Text = "Il formato del file non è corretto.E' possibile associare documenti nel formato .PDF o .PDF.P7M"
                Exit Sub
            End If
            If clsGestioneDocumentiAccreditamento.VerificaPrefissiDocumentiAccreditamento(txtSelFile, Session("conn"), PrefissoFile, AlboEnte, blnBloccoAccreditamento, Session("TipoUtente").ToString) = False Then
                If blnBloccoAccreditamento And lblTipoFase.Text <> "Articolo 2" And lblTipoFase.Text <> "Articolo 10" Then
                    msgErrore.Text = "Prefisso non consentito in questa fase di sospensione temporanea di parte delle istanze di adeguamento."
                    Exit Sub
                End If
                If blnBloccoAccreditamento = False Then
                    msgErrore.Text = "Prefisso non valido. Utilizzare uno dei prefissi consentiti per il nome del file."
                    Exit Sub
                End If


            End If
            'PER CONTROLLO NOME FILE UNIVOCO SE VIENE RICHIESTO
            'If clsGestioneDocumentiAccreditamento.ControlloNomeFile(txtSelFile, hf_IdEnteFase.Value, Session("Conn")) = True Then
            '    msgErrore.Text = "Attentione.Il Nome di questo file è già presente."
            '    Exit Sub
            'End If

            msg = clsGestioneDocumentiAccreditamento.CaricaDocumentoAccreditamento(hf_IdEnteFase.Value, Session("Utente"), txtSelFile, Session("IdEnte"), Session("conn"), PrefissoFile)

            If msg <> "" Then
                msgErrore.Text = msg
            End If
            CaricaGriglia()
        Catch ex As Exception
            ex.Message.ToString()
            msgErrore.Text = "Si è verificato un errore non gestito. Contattare l'assistenza."
        End Try

    End Sub

    Private Sub cmdChiudi_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles cmdChiudi.Click
        Dim tipologia As String = Request.QueryString("tipologia")
        Response.Redirect("WfrmRiepilogoFasiEnte.aspx?VengoDa=" & Request.QueryString("VengoDa") & "&IdEnteFse=" & hf_IdEnteFase.Value & "&tipologia=" & tipologia)
    End Sub
    Private Sub OutputXls(ByVal tipofile As String, ByRef nomeFile As String)
        nomeFile = Session("Utente") & "_" & tipofile & "_" & Format(DateTime.Now, "ddMMyyyyhhmmss") & "_" & ".csv"
        If File.Exists(Server.MapPath("download") & "\" & nomeFile) Then
            File.Delete((Server.MapPath("download") & "\" & nomeFile))
        End If
        SaveTextToFile(myDataTable, nomeFile)
    End Sub
    Sub SaveTextToFile(ByVal DTBRicerca As DataTable, ByVal nomeFile As String)

        Dim myWriter As StreamWriter
        Dim xLinea As String = String.Empty
        Dim i As Int64
        Dim j As Int64
        Dim nomeUnivoco As String

        If Not DTBRicerca.Rows.Count = 0 Then
            nomeUnivoco = nomeFile
            myWriter = New StreamWriter(Server.MapPath("download") & "\" & nomeUnivoco)
            Dim intNumCol As Int64 = DTBRicerca.Columns.Count
            For i = 0 To intNumCol - 1
                xLinea &= DTBRicerca.Columns.Item(CInt(i)).ColumnName() & ";"
            Next
            myWriter.WriteLine(xLinea)
            xLinea = vbNullString
            For i = 0 To DTBRicerca.Rows.Count - 1
                For j = 0 To intNumCol - 1
                    If IsDBNull(DTBRicerca.Rows(CInt(i)).Item(CInt(j))) = True Then
                        xLinea &= vbNullString & ";"
                    Else
                        xLinea &= ClsUtility.FormatExport(DTBRicerca.Rows(CInt(i)).Item(CInt(j))) & ";"
                    End If
                Next
                myWriter.WriteLine(xLinea)
                xLinea = vbNullString
            Next
            myWriter.Close()
            myWriter = Nothing
        End If
    End Sub
    Private Sub dtgAttivitaDocumenti_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dtgAttivitaDocumenti.ItemCommand
        CancellaMessaggiInfo()
        Dim objHLink As HyperLink
        Select Case e.CommandName
            Case "Cancella"
                clsGestioneDocumentiAccreditamento.RimuoviDocumentoEnte(e.Item.Cells(1).Text, Session("Conn"))
                CaricaGriglia()
                hlDw.Visible = False
                hlScarica.Visible = False
            Case "Download"
                objHLink = clsGestioneDocumentiAccreditamento.RecuperaDocumentoEnte(e.Item.Cells(1).Text, Session("Conn"))
                divDownloadFile.Visible = True
                hlScarica.Visible = True
                hlScarica.Text = objHLink.Text
                hlScarica.NavigateUrl = objHLink.NavigateUrl
                hlDw.Visible = False
                imgEsporta.Visible = True
        End Select

    End Sub
    Private Sub imgEsporta_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles imgEsporta.Click
        EsportazioneDocumento()
    End Sub
    Sub EsportazioneDocumento()
        Dim arrParam(0) As SqlParameter
        Dim nomeFile As String = String.Empty

        arrParam(0) = New SqlClient.SqlParameter
        arrParam(0).ParameterName = "@identefase"
        arrParam(0).SqlDbType = SqlDbType.Int
        arrParam(0).Value = hf_IdEnteFase.Value

        myDataTable = New DataTable("DocumentiProgetto")
        myDataTable = ExecuteDataTable("SP_EsportaDocumenti_Ente", arrParam)
        myDataSet.Tables.Add(myDataTable)
        OutputXls("DocumentiEnti", nomeFile)

        hlDw.NavigateUrl = "download" & "\" + nomeFile
        hlDw.Target = "_blank"
        hlDw.Visible = True
        imgEsporta.Visible = False

    End Sub
    Public Function ExecuteDataTable(ByVal storedProcedureName As String, ByVal ParamArray arrParam() As SqlParameter) As DataTable
        Dim dataTable As DataTable
        Dim cmd As New SqlCommand
        cmd.Connection = Session("Conn")
        cmd.CommandType = CommandType.StoredProcedure
        cmd.CommandText = storedProcedureName
        If Not arrParam.Length = 0 Then
            For Each param As SqlParameter In arrParam
                cmd.Parameters.Add(param)
            Next
        End If

        Dim dataAdapter As New SqlDataAdapter(cmd)
        dataTable = New DataTable
        dataAdapter.Fill(dataTable)
        Return dataTable
    End Function

End Class