Imports System
Imports System.Security
Imports System.Security.Cryptography
Imports System.Text

Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports System.Web.UI
Imports System.Web.UI.Page
Imports System.Web.HttpServerUtility
Imports System.Web.UI.HtmlControls.HtmlInputFile
Imports System.Drawing

Public Class WfrmManualiUtente
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not Session("LogIn") Is Nothing Then
            If Session("LogIn") = False Then 'verifico validità log-in
                Response.Redirect("LogOn.aspx")
            End If
        Else
            Response.Redirect("LogOn.aspx")
        End If
        If Page.IsPostBack = False Then
            hlDownload.Visible = False
            CaricaGriglia(Session("TipoUtente"))
        End If
    End Sub

    Sub CaricaGriglia(ByVal TipoUtente As String)
        Dim strsql As String
        Dim MyDataset As DataSet

        dgManuali.CurrentPageIndex = 0
        strsql = " SELECT  IdManuale, NomeApplicazione, Manuale, Versione, isnull(FileName,'') as FileName,"
        strsql &= " isnull(dbo.formatodata(DataInserimento),'') as DataInserimento,"
        strsql &= " isnull(UsernameInserimento,'') as UsernameInserimento,isnull(HashValue,'') as HashValue"
        strsql &= " FROM Manuali"
        strsql &= " WHERE Visibile = 1 AND NomeApplicazione='" & Session("Sistema") & "'"
        Select Case TipoUtente
            Case "E"
                strsql &= " AND ENTE=1 "
            Case "U"
                strsql &= " AND DGSCN=1 "
            Case "R"
                strsql &= " AND RPA=1 "
        End Select
        strsql &= " ORDER BY ordine"

        MyDataset = ClsServer.DataSetGenerico(strsql, Session("conn"))
        dgManuali.DataSource = MyDataset
        dgManuali.DataBind()

        Session("DtsManuali") = MyDataset
        lblmessaggio.Text = "Elenco Manuali."
        MyDataset.Dispose()
    End Sub

    Protected Sub cmdChiudi_Click(sender As Object, e As EventArgs) Handles cmdChiudi.Click
        Response.Redirect("WfrmMain.aspx")
    End Sub

    Private Sub dgManuali_ItemCommand(source As Object, e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgManuali.ItemCommand
        If e.CommandName = "Download" Then
            LblMsgFile.Text = ""
            If e.Item.Cells(6).Text = "&nbsp;" Then
                hlDownload.Visible = False
                LblMsgFile.Text = "Non è possibile scaricare il documento. File non presente."
                Exit Sub
            End If
            hlDownload.Visible = True
            hlDownload.NavigateUrl = DownloadManuale(e.Item.Cells(0).Text, Session("conn"))
            hlDownload.Text = e.Item.Cells(5).Text
            hlDownload.Target = "_blank"
        End If
    End Sub

    Private Function DownloadManuale(ByVal IdManuale As Integer, ByRef cnLocal As SqlConnection) As String
        Dim da As New SqlDataAdapter _
            ("SELECT BinData,FileName, HashValue, usernameinserimento FROM Manuali WHERE IdManuale = " & IdManuale, cnLocal)
        Dim cb As SqlCommandBuilder = New SqlCommandBuilder(da)
        Dim ds As New DataSet

        Dim user As String
        Dim paht As String
        Try
            Dim oblLocalHLink As New HyperLink

            da.Fill(ds, "_FileTest")
            Dim rw As DataRow
            rw = ds.Tables("_FileTest").Rows(0)

            ' Make sure you have some rows
            Dim i As Integer = ds.Tables("_FileTest").Rows.Count
            If i > 0 Then
                Dim bBLOBStorage() As Byte = _
                ds.Tables("_FileTest").Rows(0)("BinData")
                user = ds.Tables("_FileTest").Rows(0)("usernameinserimento")
                oblLocalHLink.Text = ds.Tables("_FileTest").Rows(0)("Filename")
                oblLocalHLink.NavigateUrl = FileByteToPathManuale(bBLOBStorage, ds.Tables("_FileTest").Rows(0)("Filename"), ds.Tables("_FileTest").Rows(0)("HashValue"))
                paht = FileByteToPathManuale(bBLOBStorage, ds.Tables("_FileTest").Rows(0)("Filename"), ds.Tables("_FileTest").Rows(0)("HashValue"))
            End If

            Return paht
        Catch ex As Exception
            MsgBox(ex.ToString)

        End Try
    End Function

    Private Shared Function FileByteToPathManuale(ByVal dataBuffer As Byte(), ByVal nomeFile As String, ByVal HashValue As String) As String
        'dichiaro una variabile byte che bufferizza (carica in memoria) il file template richiesto
        'e trasformato in base64
        Dim fs As FileStream
        Dim myPath As New System.Web.UI.Page

        If File.Exists(myPath.Server.MapPath("download") & "\" & nomeFile) Then
            File.Delete(myPath.Server.MapPath("download") & "\" & nomeFile)
        End If
        'passo il template al filestream
        fs = New FileStream(myPath.Server.MapPath("download") & "\" & nomeFile, FileMode.Create, FileAccess.Write)
        'ciclo il file bufferizzato e scrivo nel file tramite lo streaming del FileStream
        If (dataBuffer.Length > 0) Then
            fs.Write(dataBuffer, 0, dataBuffer.Length)
        End If

        'chiudo lo streaming
        fs.Close()
        Return "download\" & nomeFile
    End Function

    Private Sub dgManuali_PageIndexChanged(source As Object, e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgManuali.PageIndexChanged
        dgManuali.CurrentPageIndex = e.NewPageIndex
        dgManuali.DataSource = Session("DtsManuali")
        dgManuali.DataBind()
        dgManuali.SelectedIndex = -1

    End Sub
End Class