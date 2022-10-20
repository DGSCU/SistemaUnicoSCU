Imports System.Data.SqlClient
Imports System.IO

Public Class WfrmProgrammiDocumenti
    Inherits System.Web.UI.Page

    Dim MyDataSet As New DataSet
    Dim MyDataTable As DataTable
    'Public Shared IdEntePresentante As Integer
    Dim dtrgenerico As SqlClient.SqlDataReader
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
        'effettua il redirect alla pagina di Login nel caso al sessione sia invalida
        VerificaSessione()

        If Request.QueryString("idProgramma") <> "" Then
            Dim strATTIVITA As Integer = -1
            Dim strBANDOATTIVITA As Integer = -1
            Dim strENTEPERSONALE As Integer = -1
            Dim strENTITA As Integer = -1
            Dim strIDENTE As Integer = -1

            If ClsUtility.SICUREZZA_VERIFICA_AUTORIZZAZIONI(Session("conn"), Session("IdEnte"), Session("txtCodEnte"), strATTIVITA, strBANDOATTIVITA, strENTEPERSONALE, strENTITA, strIDENTE, Request.QueryString("idProgramma")) = 1 Then
                ChiudiDataReader(dtrgenerico)
            Else
                ChiudiDataReader(dtrgenerico)
                Response.Redirect("wfrmAnomaliaDati.aspx")
            End If
            If Session("TipoUtente") = "E" Then
                Dim abilitato As Integer
                abilitato = ClsUtility.LoadProgrammiAbilitaModificaEnte(Request.QueryString("idProgramma"), Session("Conn"))
                If abilitato = 0 Then
                    BloccaMaschera("L'ente non è abilitato modifica")
                Else
                    'controllo se coprogrammante. allora personalizzo funzioni
                    If ClsUtility.ProgrammiLimitaFunzioniCoprogrammante(Request.QueryString("idProgramma"), Session("IdEnte"), Session("Conn")) Then
                        BloccaMaschera("L'ente coprogrammante non può effettuare modifiche al programma")
                        dtgAttivitaDocumenti.Columns(0).Visible = False 'blocco anche download documenti
                    End If
                End If


            End If
            If Session("TipoUtente") = "R" Then
                BloccaMaschera("Utente non abilitato alla modifica")
            End If
        End If
        If Page.IsPostBack = False Then
            'verifico se esistono ancora documenti da assegnare ai progetti
            CaricaIntestazione(Request.QueryString("idProgramma"))
            CaricaGriglia()
            If LblStatoIstanza.Text <> "Registrata" And LblStatoIstanza.Text <> "Non presente" Then
                dtgAttivitaDocumenti.Columns(5).Visible = False
                LblSel.Visible = False
                txtSelFile.Visible = False
                cmdUpload.Visible = False
                imgApplica.Visible = False
                imgPrefissiDocumenti.Visible = False
            End If
            If LblStatoIstanza.Text = "Non presente" And LblStatoProgramma.Text <> "Registrato" Then
                dtgAttivitaDocumenti.Columns(5).Visible = False
                LblSel.Visible = False
                txtSelFile.Visible = False
                cmdUpload.Visible = False
                imgApplica.Visible = False
                imgPrefissiDocumenti.Visible = False
            End If
            If Request.QueryString("VengoDa") = "Istanza" Or Request.QueryString("VengoDa") = "Verifica" Then
                imgApplica.Visible = False
            End If
        End If
    End Sub
    Sub CaricaIntestazione(ByVal IdProgramma As Integer)
        Dim strSql As String
        Dim rtsProg As SqlClient.SqlDataReader

        strSql = " SELECT programmi.titolo, programmi.codiceprogramma as codice,statoprogramma,isnull(StatoBandoProgramma,'Non presente') as StatoBandoProgramma,programmi.identeproponente " & _
                 " FROM programmi inner join statiprogrammi on programmi.idstatoProgramma = statiprogrammi.idstatoprogramma " & _
                 " left join bandiprogrammi on programmi.idbandoprogramma = bandiprogrammi.idbandoprogramma " & _
                 " left join statiBandiProgrammi on bandiProgrammi.IdStatoBandoProgramma=statiBandiProgrammi.IdStatoBandoprogramma " & _
                 " WHERE programmi.IDprogramma = " & IdProgramma
        rtsProg = ClsServer.CreaDatareader(strSql, Session("Conn"))
        If rtsProg.HasRows = True Then
            rtsProg.Read()
            lblTitolo.Text = "" & rtsProg("Titolo")
            LblCodice.Text = "" & rtsProg("Codice")
            LblStatoProgramma.Text = "" & rtsProg("statoprogramma")
            LblStatoIstanza.Text = "" & rtsProg("StatoBandoProgramma")
            Session("IdEntePresentanteProgramma") = rtsProg("IdEnteProponente")
        End If
        rtsProg.Close()
        rtsProg = Nothing
    End Sub
    Sub CaricaGriglia()
        Dim strSql As String
        Try


            strSql = "SELECT IdProgrammaDocumento, FileName, DataInserimento,hashvalue FROM ProgrammiDocumenti a " & _
                    " left join PrefissiProgrammiDocumenti b on left(a.filename, charindex('_',a.filename)) = b.prefisso  " & _
                    " where idprogramma =" & Request.QueryString("idProgramma") & " " & _
                    " order by isnull(b.ordine,99) "
            Session("sqlDataSet") = ClsServer.DataSetGenerico(strSql, Session("conn"))

            dtgAttivitaDocumenti.DataSource = Session("sqlDataSet")
            dtgAttivitaDocumenti.DataBind()

        Catch ex As Exception
            dtgAttivitaDocumenti.CurrentPageIndex = dtgAttivitaDocumenti.CurrentPageIndex - 1
            dtgAttivitaDocumenti.DataBind()
        Finally
            If dtgAttivitaDocumenti.Items.Count = 0 Then
                imgEsporta.Visible = False
                imgApplica.Visible = False
            Else
                imgEsporta.Visible = True
                'imgApplica.Visible = True
            End If
        End Try

    End Sub

    Private Sub cmdUpload_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles cmdUpload.Click
        Try
            Dim msg As String
            Dim PrefissoFile As String = ""
            lblmsg.Text = ""
            hlScarica.Visible = False
            hlDw.Visible = False
            If clsGestioneDocumenti.VerificaEstensioneFile(txtSelFile) = False Then
                lblmsg.Text = "Il formato del file non è corretto.E' possibile associare documenti nel formato .PDF o .PDF.P7M"
                Exit Sub
            End If
            If clsGestioneDocumenti.VerificaPrefissiDocumentiProgramma(txtSelFile, Session("conn"), PrefissoFile, Session("Sistema")) = False Then
                lblmsg.Text = "Utilizzare uno dei prefissi consentiti per il nome del file."
                Exit Sub
            End If
            msg = clsGestioneDocumenti.CaricaDocumentoProgramma(Request.QueryString("idProgramma"), Session("Utente"), txtSelFile, Session("IdEntePresentanteProgramma"), Session("conn"), PrefissoFile)
            If msg <> "" Then
                lblmsg.Text = msg
            End If

            CaricaGriglia()

        Catch ex As Exception
            lblmsg.Text = "Si è verificato un errore non gestito. Contattare l'assistenza."
        Finally
            cmdUpload.Enabled = True
        End Try

    End Sub

    Private Sub dtgAttivitaDocumenti_PageIndexChanged(ByVal source As System.Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dtgAttivitaDocumenti.PageIndexChanged
        dtgAttivitaDocumenti.CurrentPageIndex = e.NewPageIndex
        dtgAttivitaDocumenti.DataSource = Session("sqlDataSet")
        dtgAttivitaDocumenti.DataBind()
        dtgAttivitaDocumenti.SelectedIndex = -1
        dtgAttivitaDocumenti.EditItemIndex = -1
    End Sub

    Private Sub dtgAttivitaDocumenti_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dtgAttivitaDocumenti.ItemCommand
        Dim objHLink As HyperLink

        Select Case e.CommandName
            Case "Cancella"
                clsGestioneDocumenti.RimuoviDocumentoProgramma(e.Item.Cells(1).Text, Session("Conn"))
                CaricaGriglia()

                hlDw.Visible = False
                lblmsg.Text = ""
                hlScarica.Visible = False
                divDownloadFile.Visible = False
            Case "Download"
                objHLink = clsGestioneDocumenti.RecuperaDocumentoProgramma(e.Item.Cells(1).Text, Session("Conn"))
                divDownloadFile.Visible = True
                hlScarica.Visible = True
                hlScarica.Text = "Download File '" & objHLink.Text & "'"
                hlScarica.NavigateUrl = objHLink.NavigateUrl
                hlDw.Visible = False
                imgEsporta.Visible = True
        End Select

    End Sub
    Private Sub imgEsporta_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles imgEsporta.Click
        Esportazione()
    End Sub
    Sub Esportazione()
        Dim arrParam(0) As SqlParameter
        Dim NomeFile As String

        arrParam(0) = New SqlClient.SqlParameter
        arrParam(0).ParameterName = "@IdProgramma"
        arrParam(0).SqlDbType = SqlDbType.Int
        arrParam(0).Value = Request.QueryString("idProgramma")

        MyDataTable = New DataTable("DocumentiProgramma")
        MyDataTable = ExecuteDataTable("SP_EsportaDocumenti_Programma", arrParam)
        MyDataSet.Tables.Add(MyDataTable)
        OutputXls(MyDataTable, "DocumentiProgramma", NomeFile)

        hlDw.NavigateUrl = "download" & "\" + NomeFile
        hlDw.Target = "_blank"
        hlDw.Visible = True
        imgEsporta.Visible = False

    End Sub

    Public Function ExecuteDataTable(ByVal storedProcedureName As String, ByVal ParamArray arrParam() As SqlParameter) As DataTable
        Dim dt As DataTable

        ' Define the command 
        Dim cmd As New SqlCommand
        cmd.Connection = Session("Conn")
        cmd.CommandType = CommandType.StoredProcedure
        cmd.CommandText = storedProcedureName

        ' Handle the parameters 
        If Not arrParam.Length = 0 Then
            For Each param As SqlParameter In arrParam
                cmd.Parameters.Add(param)
            Next
        End If

        ' Define the data adapter and fill the dataset 
        Dim da As New SqlDataAdapter(cmd)
        dt = New DataTable
        da.Fill(dt)

        Return dt
    End Function

    Private Function OutputXls(ByVal Datasource As DataTable, ByVal Tipofile As String, ByRef NomeFile As String) As Boolean

        NomeFile = Session("Utente") & "_" & Tipofile & "_" & Format(DateTime.Now, "ddMMyyyyhhmmss") & "_" & ".csv"

        Dim stringWrite As System.IO.StringWriter = New System.IO.StringWriter

        If File.Exists(Server.MapPath("download") & "\" & NomeFile) Then
            File.Delete((Server.MapPath("download") & "\" & NomeFile))
        End If
        SaveTextToFile(MyDataTable, NomeFile)
        Return True
    End Function
    Sub SaveTextToFile(ByVal DTBRicerca As DataTable, ByVal NomeFile As String)

        Dim Writer As StreamWriter
        Dim xLinea As String
        Dim i As Int64
        Dim j As Int64
        Dim NomeUnivoco As String

        If DTBRicerca.Rows.Count = 0 Then
            'lblErr.Text = lblErr.Text & "La ricerca non ha prodotto nessun risultato."
        Else
            'xPrefissoNome = Session("Utente")
            NomeUnivoco = NomeFile
            Writer = New StreamWriter(Server.MapPath("download") & "\" & NomeUnivoco)
            'Creazione dell'inntestazione del CSV
            Dim intNumCol As Int64 = DTBRicerca.Columns.Count
            For i = 0 To intNumCol - 1
                xLinea &= DTBRicerca.Columns.Item(CInt(i)).ColumnName() & ";"
            Next
            Writer.WriteLine(xLinea)
            xLinea = vbNullString

            'Scorro tutte le righe del datatable e riempio il CSV
            For i = 0 To DTBRicerca.Rows.Count - 1
                For j = 0 To intNumCol - 1
                    If IsDBNull(DTBRicerca.Rows(CInt(i)).Item(CInt(j))) = True Then
                        xLinea &= vbNullString & ";"
                    Else
                        xLinea &= ClsUtility.FormatExport(DTBRicerca.Rows(CInt(i)).Item(CInt(j))) & ";"
                    End If
                Next
                Writer.WriteLine(xLinea)
                xLinea = vbNullString
            Next
            Writer.Close()
            Writer = Nothing
        End If
    End Sub

    Private Function StatoDocumenti(ByVal idattivita As Integer) As Boolean
        Dim strSql As String
        Dim dtrCount As SqlClient.SqlDataReader
        Dim blnReturn As Boolean
        strSql = " SELECT  NElaborazioniMancanti FROM LockDocumentiEnte l " & _
                 " INNER JOIN Attività a on a.identepresentante = l.idente WHERE a.idattività = " & idattivita
        dtrCount = ClsServer.CreaDatareader(strSql, Session("conn"))
        blnReturn = dtrCount.HasRows
        ChiudiDataReader(dtrCount)
        Return blnReturn
    End Function
    Protected Sub cmdChiudi_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdChiudi.Click
        'If Request.QueryString("VengoDa") = Costanti.VENGO_DA_ACCETTAZIONE_PROGETTI Then
        '    Response.Redirect("assegnazionevincoliprogetti.aspx?idattivita=" & Request.QueryString("idattivita") & "&tipologia=" & Request.QueryString("tipologia") & "&Nazionale=" & Request.QueryString("Nazionale"))
        'ElseIf (Request.QueryString("VengoDa") = Costanti.VENGO_DA_VALUTAZIONE_QUALITA) Then
        '    Response.Redirect("WfrmValutazioneQual.aspx?idprogetto=" & Request.QueryString("idattivita"))
        'ElseIf (Request.QueryString("VengoDa") = Costanti.VENGO_DA_ISTANZA) Then
        '    'DA MANDARE SULLA MASCHERA CHIAMANTE SIA IN INSERIMENTO CHE IN MODIFICA
        '    If Request.QueryString("Verso") = "Mod" Then
        '        Response.Redirect("wfrmIstanzaPresentazione.aspx?id=" & Request.QueryString("id") & "&DataFine=" & Request.QueryString("DataFine") & "&DataInizio=" & Request.QueryString("DataInizio") & "&Verso=" & Request.QueryString("Verso") & "&Stato=" & Request.QueryString("Stato") & "&Arrivo=" & Request.QueryString("Arrivo") & "&idBA=" & Request.QueryString("idBA"))
        '        Exit Sub
        '    Else
        '        Response.Redirect("WfrmIstanzaPresentazione.aspx?Verso=Ins&VediEnte=1")
        '    End If
        'ElseIf Request.QueryString("VengoDa") = Costanti.VENGO_DA_VERIFICA Then
        '    Response.Redirect("verificarequisiti.aspx?IdEnte=" & Session("IdEnte") & "&IdVerifica=" & Request.QueryString("IdVerifica"))
        'Else

        '    Response.Redirect(ClsUtility.TrovaAlboProgetto(Request.QueryString("IdAttivita"), Session("Conn")) & "?Modifica=" & Request.QueryString("Modifica") & "&Nazionale=" & Request.QueryString("Nazionale") & "&IdAttivita=" & CInt(Request.QueryString("IdAttivita")) & "&VengoDa=" & Request.QueryString("VengoDa"))
        '    'Response.Redirect("TabProgetti.aspx?Modifica=" & Request.QueryString("Modifica") & "&Nazionale=" & Request.QueryString("Nazionale") & "&IdAttivita=" & CInt(Request.QueryString("IdAttivita")) & "&VengoDa=" & Request.QueryString("VengoDa"))
        'End If
        If (Request.QueryString("VengoDa") = Costanti.VENGO_DA_ISTANZA_PROGRAMMI) Then
            'DA MANDARE SULLA MASCHERA CHIAMANTE SIA IN INSERIMENTO CHE IN MODIFICA
            If Request.QueryString("idBP") Is Nothing Then
                Response.Redirect("WfrmProgrammiIstanza.aspx?Verso=Ins&VediEnte=1")
            Else
                Response.Redirect("WfrmProgrammiIstanza.aspx?idBP=" & Request.QueryString("idBP"))
            End If
        Else

            Select Case Request.QueryString("VengoDa")

                Case "AccettazioneProgramma"

                    Response.Redirect("assegnazionevincoliprogrammi.aspx?idprogramma=" & Request.QueryString("idProgramma") & "&VengoDa=" & Request.QueryString("VengoDa") & "&Nazionale=1" & "")

                Case "ValQualita"

                    Response.Redirect("WfrmValutazioneQualProgrammi.aspx?idprogramma=" & Request.QueryString("idProgramma") & "&VengoDa=" & Request.QueryString("VengoDa") & "")
                Case Else

                    Response.Redirect("WfrmProgrammi.aspx?idProgramma=" & Request.QueryString("idProgramma"))

            End Select

            'If Request.QueryString("VengoDa") = "AccettazioneProgramma" Then
            '    Response.Redirect("assegnazionevincoliprogrammi.aspx?idprogramma=" & Request.QueryString("idProgramma") & "&VengoDa=" & Request.QueryString("VengoDa") & "&Nazionale=1" & "")

            'Else
            '    Response.Redirect("WfrmProgrammi.aspx?idProgramma=" & Request.QueryString("idProgramma"))
            'End If

        End If


    End Sub
    Private Sub BloccaMaschera(ByVal strmessaggio As String)
        'txtSelFile.Disabled = True
        'cmdUpload.Enabled = False
        'dtgAttivitaDocumenti.Columns(5).Visible = False
        ''cmdSalva.Enabled = False



        dtgAttivitaDocumenti.Columns(5).Visible = False
        LblSel.Visible = False
        txtSelFile.Visible = False
        cmdUpload.Visible = False
        imgApplica.Visible = False
        imgPrefissiDocumenti.Visible = False
        lblmsg.Text = strmessaggio
    End Sub
End Class