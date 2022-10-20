Imports System.Data.SqlClient
Imports System.IO
Imports System.Drawing

Public Class WfrmEditorModelliDownload
    Inherits System.Web.UI.Page
    Public strSql As String
    Public dtsRisRicerca As DataSet
    Public dtrgenerico As Data.SqlClient.SqlDataReader
    Dim dtsGenerico As DataSet
    Dim debug As Boolean = False

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
        'Inserire qui il codice utente necessario per inizializzare la pagina

        If Page.IsPostBack = False Then
            ChiudiDataReader(dtrgenerico)
            strSql = "select UserName,IdRegioneCompetenza,RegCompe,IdArea,IdModello,Area,IdUtenteArea,NomeLogico,NomeFisico,Path,UsernameProprietario,dbo.FormatoData(DataCreazione) as DataCreazione ,Descrizione from VW_Editor_ElencoModelli_1 where idmodello=" & Request.QueryString("model") & " and username = '" & Session("Utente") & "'"
            dtrgenerico = ClsServer.CreaDatareader(strSql, Session("conn"))

            If dtrgenerico.HasRows = True Then
                dtrgenerico.Read()
                urlDownload.Visible = False
                lblNomedoc.Text = dtrgenerico("NomeFisico")
                lblnomelogico.Text = dtrgenerico("NomeLogico")
                lbldescrizione.Text = dtrgenerico("descrizione")
                lblUltimamodifica.Text = Format(dtrgenerico("DataCreazione"), "Short Date")
                lblUsername.Text = dtrgenerico("UserNameProprietario")
                path.Value = dtrgenerico("Path")
                area.Value = dtrgenerico("IdArea")
                regionecompetenza.Value = dtrgenerico("IdRegioneCompetenza")
                If Not dtrgenerico Is Nothing Then
                    dtrgenerico.Close()
                    dtrgenerico = Nothing
                End If
            End If
            CaricaGriglia()

        End If
    End Sub
    'routine che carica la datatable che caricherà dinamicamente la datagrid della stampa delle ricerche
    Sub CaricaDataTablePerStampa(ByVal DataSetDaScorrere As DataSet, ByVal NColonne As Integer, ByVal NomiColonne() As String, ByVal NomiCampiColonne() As String)
        Dim dt As New DataTable
        Dim dr As DataRow
        Dim i As Integer
        Dim x As Integer

        'carico i nomi delle colonne che andrò a stampare nella datagrid
        For x = 0 To NColonne
            dt.Columns.Add(New DataColumn(NomiColonne(x), GetType(String)))
        Next

        'carico il datatable con il risultato della query della ricerca, in qusto caso delle risorse
        If DataSetDaScorrere.Tables(0).Rows.Count > 0 Then
            For i = 1 To DataSetDaScorrere.Tables(0).Rows.Count
                dr = dt.NewRow()
                For x = 0 To NColonne
                    dr(x) = DataSetDaScorrere.Tables(0).Rows.Item(i - 1).Item(NomiCampiColonne(x))
                Next
                dt.Rows.Add(dr)
            Next
        End If

        'passo alla sessione la datatable che ho appena creato e che userò per il databinding della datagrid della stampa
        Session("DtbRicerca") = dt

    End Sub
    Private Sub CaricaGriglia()
        strSql = "select IdArea,IdTag,NomeTag,Descrizione,Sequenza,TipoTag,IdModello,gruppo, '&#60;'+NomeTag+'&#62;' as CodiceTag from VW_Editor_Tag where idModello=" & Request.QueryString("model") & " order by gruppo "
        dtsRisRicerca = ClsServer.DataSetGenerico(strSql, Session("conn"))
        dtgRisultatoRicerca.DataSource = dtsRisRicerca
        dtgRisultatoRicerca.DataBind()

        '*********************************************************************************
        'blocco per la creazione della datatable per la stampa della ricerca
        'nome e posizione di lettura delle colopnne a base 0
        Dim NomeColonne(3) As String
        Dim NomiCampiColonne(3) As String
        'nome della colonna 
        'e posizione nella griglia di lettura
        NomeColonne(0) = "Nome Tag"
        NomeColonne(1) = "Descrizione Tag"
        NomeColonne(2) = "Gruppo"


        NomiCampiColonne(0) = "NomeTag"
        NomiCampiColonne(1) = "Descrizione"
        NomiCampiColonne(2) = "gruppo"


        'carico un datatable che userò poi nella pagina di stampa
        'il numero delle colonne è a base 0
        CaricaDataTablePerStampa(dtsRisRicerca, 2, NomeColonne, NomiCampiColonne)

        '********************************************************************************
    End Sub

    

    Private Sub dtgRisultatoRicerca_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dtgRisultatoRicerca.PageIndexChanged
        dtgRisultatoRicerca.SelectedIndex = -1
        dtgRisultatoRicerca.EditItemIndex = -1
        dtgRisultatoRicerca.CurrentPageIndex = e.NewPageIndex
        CaricaGriglia()
    End Sub
    Private Sub imgScarica_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles imgScarica.Click
        ReimpostaMessaggiNotifica()
        Dim Percorso As String
        Percorso = path.Value
        urlDownload.Visible = True
        Dim wsLocal As New WS_Editor.WSMetodiDocumentazione
        wsLocal.Url = ConfigurationManager.AppSettings("URL_WS_Documentazione")
        Dim strPathLocale As String = ClsUtility.TrovaPathLocale(Percorso, Session("conn"))
        Dim dataBuffer As Byte() = Convert.FromBase64String(wsLocal.RecuperaTemplate(Percorso, lblNomedoc.Text))
        Dim fs As FileStream
        fs = New FileStream(Server.MapPath(strPathLocale & lblNomedoc.Text), FileMode.Create, FileAccess.Write)
        If (dataBuffer.Length > 0) Then
            fs.Write(dataBuffer, 0, dataBuffer.Length)
        End If
        urlDownload.NavigateUrl = Replace(strPathLocale, "\", "/") & lblNomedoc.Text
        fs.Close()
        urlDownload.Text = lblNomedoc.Text
    End Sub

    Private Sub cmdUpload_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles cmdUpload.Click
        ReimpostaMessaggiNotifica()
        Dim pathcopy As String
        Dim pathCopyStor As String
        Dim pathpulita As String
        Dim Path As String
        Dim NomeLogico As String
        Dim NomeFisico As String
        Dim Reader As StreamReader
        Dim strcontenuto As String

        strSql = "SELECT Editor_Modelli.NomeLogico, Editor_Modelli.NomeFisico, Editor_ModelliCompetenze.Path, Editor_ModelliCompetenze.IdModelloCompetenza " & _
            "FROM Editor_Modelli " & _
            "INNER JOIN Editor_ModelliCompetenze ON Editor_ModelliCompetenze.IdModello = Editor_Modelli.IdModello " & _
            "WHERE Editor_Modelli.IDModello = " & Request.QueryString("model") & " and Editor_Modelli.idArea=" & area.Value &
                " and Editor_ModelliCompetenze.IdRegioneCompetenza=" & regionecompetenza.Value
        dtrgenerico = ClsServer.CreaDatareader(strSql, Session("conn"))

        If dtrgenerico.HasRows = True Then
            dtrgenerico.Read()
            Path = dtrgenerico("Path")
            NomeLogico = dtrgenerico("NomeLogico")
            NomeFisico = dtrgenerico("NomeFisico")
        End If

        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
        Dim nomefilenuovo As String = lblNomedoc.Text & "_" & Session("Utente") & "_" & Year(Now) & "_" & Month(Now) & "_" & Day(Now) & "_ORA_" & Hour(Now) & "_" & Minute(Now) & "_" & Second(Now) & ".rtf"
        pathpulita = Path
        pathcopy = pathpulita & lblNomedoc.Text
        pathCopyStor = pathpulita + "Storico\" & nomefilenuovo

        pathpulita = pathpulita + "Storico\"

        Try

            Dim wsLocale As New WS_Editor.WSMetodiDocumentazione

            wsLocale.Url = ConfigurationSettings.AppSettings("URL_WS_Documentazione")

            wsLocale.ScriviTemplate(pathpulita, wsLocale.RecuperaTemplate(Path, lblNomedoc.Text), nomefilenuovo)

            wsLocale.Dispose()

        Catch ex As Exception
            Response.Write(ex.Message)
        End Try

        If Not (txtSelFile.PostedFile Is Nothing) Then
            Try
                Dim cmdGenerico As SqlClient.SqlCommand
                Dim NomeFileCaricato As String
                NomeFileCaricato = txtSelFile.PostedFile.FileName
                Dim StrigaArrey() As String = NomeFileCaricato.Split("\")
                Dim risultato As String = StrigaArrey(StrigaArrey.Length - 1).ToString()
                If UCase(lblNomedoc.Text) = UCase(risultato) Then

                    Dim strPercorsoTemporaneoTemplate As String = ClsUtility.TrovaPathLocale(Path, Session("conn"))

                    txtSelFile.PostedFile.SaveAs(Server.MapPath(strPercorsoTemporaneoTemplate) & NomeFisico)

                    Dim wsLocale As New WS_Editor.WSMetodiDocumentazione

                    wsLocale.Url = ConfigurationSettings.AppSettings("URL_WS_Documentazione")

                    wsLocale.ScriviTemplate(Path, ClsUtility.ConvertFileToBase64(Replace(Server.MapPath(strPercorsoTemporaneoTemplate), "\", "/") & NomeFisico), NomeFisico)

                    wsLocale.Dispose()

                    Reader = New StreamReader(Server.MapPath(strPercorsoTemporaneoTemplate) & NomeFisico, System.Text.Encoding.Default, False)
                    strcontenuto = Reader.ReadToEnd()
                    Reader.Close()

                    'INSERT  EDITOR_CRONOLOGIAMODELLI

                    Dim cmdinsert As Data.SqlClient.SqlCommand
                    strSql = "INSERT INTO  Editor_CronologiaModelli (IdModelloCompetenza,IdArea,NomeLogico,NomeFisico,Path,UserNameProprietario,DataCreazione,DataStoricizzazione) "
                    strSql = strSql & "SELECT IdModelloCompetenza,'" & area.Value & "','" & ClsServer.NoApice(NomeLogico) & "','" & ClsServer.NoApice(nomefilenuovo) & "','" & pathpulita & "', UsernameProprietario, DataCreazione, getdate() " & _
                       "FROM Editor_ModelliCompetenze " & _
                       "WHERE IDModello = " & Request.QueryString("model") & " and IdRegioneCompetenza=" & regionecompetenza.Value

                    cmdinsert = New SqlClient.SqlCommand(strSql, Session("conn"))
                    cmdinsert.ExecuteNonQuery()
                    cmdinsert.Dispose()

                    'AGGIORNAMENTO TABELLE EDITOR_MODELLI 
                    If Not dtrgenerico Is Nothing Then
                        dtrgenerico.Close()
                        dtrgenerico = Nothing
                    End If

                    strSql = "Update Editor_ModelliCompetenze set UserNameProprietario='" & Session("Utente") & "'," & _
                           " DataCreazione = getdate(), " & _
                           " Contenuto = '" & strcontenuto.Replace("'", "''") & "' " & _
                          " where idModello=" & Request.QueryString("model") & " and IdRegioneCompetenza=" & regionecompetenza.Value

                    cmdGenerico = ClsServer.EseguiSqlClient(strSql, Session("conn"))

                    ChiudiDataReader(dtrgenerico)

                Else
                    Response.Write("<script language=""javascript"">" & vbCrLf)
                    Response.Write("alert(""Nome del file non congruo.Prego rinominare il file in locale"");" & vbCrLf)
                    Response.Write("</script>" & vbCrLf)
                End If
                msgConferma.Text = "Aggiornamento eseguito con successo."
            Catch exc As Exception
                Response.Write(exc.Message)
            End Try
        End If
    End Sub

    Private Sub cmdChiudi_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles cmdChiudi.Click
        Response.Redirect("WfrmEditorElencoModelli.aspx")
    End Sub


    Private Sub CmdCronologia_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles CmdCronologia.Click
        Response.Redirect("WfrmEditorElencoRipristinoModelli.aspx?idModello=" & Request.QueryString("model"))

    End Sub

    Protected Sub cmdEsporta_Click(ByVal sender As Object, ByVal e As EventArgs) Handles CmdEsporta.Click
        CmdEsporta.Visible = False
        Dim dtbRicerca As DataTable = Session("DtbRicerca")
        StampaCSV(dtbRicerca)
    End Sub
    Private Sub StampaCSV(ByVal dtbRicerca As DataTable)
        Dim path As String
        Dim xPrefissoNome As String
        Dim url As String
        Dim utility As ClsUtility = New ClsUtility()

        If dtbRicerca.Rows.Count = 0 Then
            ApriCSV1.Visible = False
            CmdEsporta.Visible = False
        Else
            xPrefissoNome = Session("Utente")
            path = Server.MapPath("download")
            url = CreaFileCSV(dtbRicerca, xPrefissoNome, path)
            ApriCSV1.Visible = True
            ApriCSV1.NavigateUrl = url
        End If
    End Sub
    Function CreaFileCSV(ByVal DTBRicerca As DataTable, ByVal xPrefissoNome As String, ByVal mapPath As String) As String

        Dim writer As StreamWriter
        Dim xLinea As String = String.Empty
        Dim i As Int64
        Dim j As Int64
        Dim nomeUnivoco As String
        Dim url As String
        nomeUnivoco = xPrefissoNome & "ExpDati" & Year(Now) & Month(Now) & Day(Now) & Hour(Now) & Minute(Now) & Second(Now)
        writer = New StreamWriter(mapPath & "\" & nomeUnivoco & ".CSV")
        'Creazione dell'inntestazione del CSV
        Dim intNumCol As Int64 = DTBRicerca.Columns.Count
        For i = 0 To intNumCol - 1
            xLinea &= DTBRicerca.Columns.Item(CInt(i)).ColumnName() & ";"
        Next
        writer.WriteLine(xLinea)
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

            writer.WriteLine(xLinea)
            xLinea = vbNullString

        Next
        url = "download\" & nomeUnivoco & ".CSV"

        writer.Close()
        writer = Nothing
        Return url
    End Function

    Private Sub ReimpostaMessaggiNotifica()
        msgConferma.Text = String.Empty
        msgInfo.Text = String.Empty
        msgErrore.Text = String.Empty
    End Sub

End Class