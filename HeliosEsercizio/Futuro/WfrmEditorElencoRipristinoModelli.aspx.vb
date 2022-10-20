Imports System.Data.SqlClient
Imports System.IO

Public Class WfrmEditorElencoRipristinoModelli
    Inherits System.Web.UI.Page
    Private NomeFileCaricato As String
    Private NomeUnivoco As String
    Public dtsRisRicerca As DataSet
    Public dtrgenerico As Data.SqlClient.SqlDataReader
    Dim dtsGenerico As DataSet
    Dim cmdGenerico As SqlClient.SqlCommand
    Public strSql As String
    Public AreaId As String

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

        If (IsPostBack = False) Then
            ChiudiDataReader(dtrgenerico)

            strSql = "select UserName,IdRegioneCompetenza,RegCompe,IdArea,IdModello,Area,IdUtenteArea,NomeLogico,NomeFisico,Path,UsernameProprietario,dbo.FormatoData(DataCreazione) as DataCreazione ,Descrizione from VW_Editor_ElencoModelli_1 where idmodello=" & Request.QueryString("idmodello") &
                " and username = '" & Session("Utente") & "'"
            dtrgenerico = ClsServer.CreaDatareader(strSql, Session("conn"))

            If dtrgenerico.HasRows = True Then
                dtrgenerico.Read()
                txtSelFile.Text = dtrgenerico("NomeFisico")
                lblnomelogico.Text = dtrgenerico("NomeLogico")
                lbldescrizione.Text = dtrgenerico("descrizione")
                lblUltimamodifica.Text = Format(dtrgenerico("DataCreazione"), "Short Date")
                lblUsername.Text = dtrgenerico("UserNameProprietario")
                AreaId = dtrgenerico("IdArea")
                IdRegioneCompetenza.Value = dtrgenerico("IdRegioneCompetenza")
                txtIdModello.Value = dtrgenerico("IdModello")
            End If
            ChiudiDataReader(dtrgenerico)
            CaricaGriglia()
        End If
    End Sub
    Private Sub CaricaGriglia()
        strSql = "select Editor_CronologiaModelli.IdCronologiaModello, Editor_CronologiaModelli.IdModelloCompetenza, Editor_CronologiaModelli.IdArea, Editor_CronologiaModelli.NomeLogico, Editor_CronologiaModelli.NomeFisico, Editor_CronologiaModelli.Path, Editor_CronologiaModelli.UserNameProprietario, " &
                " Editor_CronologiaModelli.DataCreazione, Editor_CronologiaModelli.DataStoricizzazione from Editor_CronologiaModelli" &
                " INNER JOIN Editor_ModelliCompetenze ON Editor_ModelliCompetenze.IdModelloCompetenza = Editor_CronologiaModelli.IdModelloCompetenza " &
                " where Editor_ModelliCompetenze.idModello=" & Request.QueryString("idModello") & " AND Editor_ModelliCompetenze.IdRegioneCompetenza=" & IdRegioneCompetenza.Value &
                " order by DataStoricizzazione desc "

        dtsRisRicerca = ClsServer.DataSetGenerico(strSql, Session("conn"))
        dtgRisultatoRicerca.DataSource = dtsRisRicerca
        dtgRisultatoRicerca.DataBind()
        If (dtgRisultatoRicerca.Items.Count > 0) Then
            dtgRisultatoRicerca.Caption = "Cronologia Documento"
        Else
            dtgRisultatoRicerca.Caption = "Cronologia non presente per il documento selezionato"
        End If
    End Sub

    

    Private Sub UpLoad()
        ReimpostaMessaggiNotifica()
        Dim pathcopy As String = String.Empty
        Dim pathCopyStor As String = String.Empty
        Dim Path As String = String.Empty
        Dim NomeLogico As String = String.Empty
        Dim NomeFisico As String = String.Empty
        Dim PathNAZ As String = String.Empty
        Dim NomeLogicoORG As String = String.Empty
        Dim NomeFisicoORG As String = String.Empty

        ChiudiDataReader(dtrgenerico)

        strSql = "SELECT * " & _
            "FROM Editor_CronologiaModelli " & _
            "WHERE IDCronologiaModello = " & dtgRisultatoRicerca.SelectedItem.Cells(0).Text

        dtrgenerico = ClsServer.CreaDatareader(strSql, Session("conn"))

        If dtrgenerico.HasRows = True Then
            dtrgenerico.Read()
            Path = dtrgenerico("Path")
            NomeLogico = dtrgenerico("NomeLogico")
            NomeFisico = dtrgenerico("NomeFisico")
        End If
        ChiudiDataReader(dtrgenerico)

        strSql = "SELECT Editor_Modelli.NomeLogico, Editor_Modelli.NomeFisico, Editor_ModelliCompetenze.Path " & _
                  "FROM Editor_Modelli " & _
                  "INNER JOIN Editor_ModelliCompetenze ON Editor_ModelliCompetenze.IdModello = Editor_Modelli.IdModello " & _
                  "WHERE Editor_ModelliCompetenze.IDModelloCompetenza = " & dtgRisultatoRicerca.SelectedItem.Cells(1).Text
        dtrgenerico = ClsServer.CreaDatareader(strSql, Session("conn"))


        If dtrgenerico.HasRows = True Then
            dtrgenerico.Read()
            PathNAZ = dtrgenerico("Path")
            NomeLogicoORG = dtrgenerico("NomeLogico")
            NomeFisicoORG = dtrgenerico("NomeFisico")
        End If
        ChiudiDataReader(dtrgenerico)

        pathcopy = PathNAZ & NomeFisicoORG
        NomeUnivoco = NomeFisicoORG & "_" & Session("Utente") & "_" & Year(Now) & "_" & Month(Now) & "_" & Day(Now) & "_ORA_" & Hour(Now) & "_" & Minute(Now) & "_" & Second(Now) & ".rtf"
        pathCopyStor = Path & NomeFisicoORG & "_" & Session("Utente") & "_" & Year(Now) & "_" & Month(Now) & "_" & Day(Now) & "_ORA_" & Hour(Now) & "_" & Minute(Now) & "_" & Second(Now) & ".rtf"

        '-----------------------------------------------

        Try
            Dim wsLocale As New WS_Editor.WSMetodiDocumentazione

            wsLocale.Url = ConfigurationManager.AppSettings("URL_WS_Documentazione")

            wsLocale.ScriviTemplate(Path, wsLocale.RecuperaTemplate(PathNAZ, NomeFisicoORG), NomeUnivoco)

            wsLocale.Dispose()

        Catch ex As Exception
            Response.Write(ex.Message)
        End Try


        Try
            Dim wsLocale As New WS_Editor.WSMetodiDocumentazione

            wsLocale.Url = ConfigurationManager.AppSettings("URL_WS_Documentazione")

            wsLocale.ScriviTemplate(PathNAZ, wsLocale.RecuperaTemplate(Path, NomeFisico), NomeFisicoORG)

            wsLocale.Dispose()

        Catch ex As Exception
            Response.Write(ex.Message)
        End Try

        Try
            NomeFileCaricato = txtSelFile.Text
            Dim StrigaArrey() As String = NomeFileCaricato.Split("\")
            Dim risultato As String = StrigaArrey(StrigaArrey.Length - 1).ToString()
            If txtSelFile.Text = risultato Then
                Dim cmdinsert As Data.SqlClient.SqlCommand
                strSql = "INSERT INTO Editor_CronologiaModelli (IdModelloCompetenza,IdArea,NomeLogico,NomeFisico,Path,UserNameProprietario,DataCreazione,DataStoricizzazione) "
                strSql = strSql & "SELECT IdModelloCompetenza,'" & AreaId & "','" & NomeLogico & "','" & NomeUnivoco & "','" & Path & "', UsernameProprietario, DataCreazione, getdate() " & _
                   "FROM Editor_ModelliCompetenze " & _
                   "WHERE IDModello = " & Request.QueryString("idModello") & " and IdRegioneCompetenza='" & IdRegioneCompetenza.Value & "'"

                cmdinsert = New SqlClient.SqlCommand(strSql, Session("conn"))
                cmdinsert.ExecuteNonQuery()
                cmdinsert.Dispose()
                ChiudiDataReader(dtrgenerico)

                strSql = "Update Editor_ModelliCompetenze set UserNameProprietario='" & Session("Utente") & "'," & _
                       " DataCreazione = getdate() " & _
                      " where idModello=" & Request.QueryString("idModello") & " and IdRegioneCompetenza='" & IdRegioneCompetenza.Value & "'"

                cmdGenerico = ClsServer.EseguiSqlClient(strSql, Session("conn"))
                ChiudiDataReader(dtrgenerico)
                msgConferma.Text = "Salvataggio avvenuto con successo."
            Else
                msgErrore.Text = "Nome del file non trovato.Prego verificare il file in locale."
            End If
        Catch exc As Exception
            Response.Write(exc.Message)
        End Try
    End Sub

    Private Sub dtgRisultatoRicerca_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dtgRisultatoRicerca.PageIndexChanged
        dtgRisultatoRicerca.SelectedIndex = -1
        dtgRisultatoRicerca.EditItemIndex = -1
        dtgRisultatoRicerca.CurrentPageIndex = e.NewPageIndex
        CaricaGriglia()
    End Sub

    Private Sub dtgRisultatoRicerca_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dtgRisultatoRicerca.ItemCommand
        Dim Percorso As String

        Select Case e.CommandName
            Case "Documento"
                Percorso = e.Item.Cells(5).Text & e.Item.Cells(4).Text
                Call StampaDocumentiGen(e.Item.Cells(4).Text, e.Item.Cells(5).Text)

        End Select
    End Sub
    Sub StampaDocumentiGen(ByVal NomeFile As String, ByVal Path As String)
        ReimpostaMessaggiNotifica()
        Dim strPathPopoUp As String = String.Empty
        Try
            Dim wsLocal As New WS_Editor.WSMetodiDocumentazione

            wsLocal.Url = ConfigurationManager.AppSettings("URL_WS_Documentazione")

            Dim strPathLocale As String = "documentazione\template\Storico\"

            Dim dataBuffer As Byte() = Convert.FromBase64String(wsLocal.RecuperaTemplate(Path, NomeFile))
            Dim fs As FileStream
            fs = New FileStream(Server.MapPath(strPathLocale & NomeFile), FileMode.Create, FileAccess.Write)
            If (dataBuffer.Length > 0) Then
                fs.Write(dataBuffer, 0, dataBuffer.Length)
            End If
            urlDownload.NavigateUrl = Replace(strPathLocale, "\", "/") & NomeFile
            fs.Close()

            urlDownload.Text = NomeFile
            urlDownload.Visible = True
            urlDownload.Focus()
        Catch ex As Exception
            Response.Write(ex.Message)
        End Try

    End Sub

    Protected Sub cmdChiudi_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdChiudi.Click
        Response.Redirect("WfrmEditorModelliDownload.aspx?model=" & Request.QueryString("idModello"))
    End Sub

    Protected Sub dtgRisultatoRicerca_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles dtgRisultatoRicerca.SelectedIndexChanged
        Call UpLoad()
    End Sub
    Private Sub ReimpostaMessaggiNotifica()
        msgConferma.Text = String.Empty
        msgInfo.Text = String.Empty
        msgErrore.Text = String.Empty
    End Sub
End Class