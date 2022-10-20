Imports System.IO
Imports System
Imports System.Drawing
Imports System.Collections
Imports System.ComponentModel
Public Class WfrmAssicurazioneVolontarioCronoGG
    Inherits System.Web.UI.Page


    Dim strsql As String
    Dim dtrGenerico As SqlClient.SqlDataReader
    Dim dtsGenerico As DataSet
    Dim cmdGenerico As SqlClient.SqlCommand
    Dim volonatrio As String
    Protected WithEvents TD1 As System.Web.UI.HtmlControls.HtmlTableCell
    Dim Lista() As CronoAssGG
    Dim NumFile As Integer
    Public Property ServerPath() As String
        Get
            'Return "c:\test"

            'Return "\\appl\modhelios$\assicurazione"
            Return ConfigurationManager.AppSettings("PercorsoFileAssicurazioniGG")
        End Get
        Set(ByVal Value As String)

        End Set
    End Property
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Page.IsPostBack = False Then

            Dim Crono As CronoAssGG
            Dim filestring As String
            Dim PathInterno As String

            ' ServerPath = 
            Dim files() As String = (Directory.GetFiles(ServerPath))

            Dim MyDt As New DataTable("Files")


            ReDim Lista(files.Length - 1)
            NumFile = 0
            Try
                For Each filestring In files
                    'PathInterno = filestring.Replace(ServerPath, Server.MapPath("download"))
                    'If File.Exists(PathInterno) = False Then
                    '    File.Copy(filestring, PathInterno)
                    'End If


                    Crono = New CronoAssGG(filestring)

                    Lista(NumFile) = Crono
                    NumFile += 1

                Next
                CaricaGriglia()
            Catch ex As Exception
                Dim l As String = ex.Message
            End Try



        End If
    End Sub
    Private Sub dtgRisultatoRicerca_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dtgRisultatoRicerca.PageIndexChanged

        dtgRisultatoRicerca.CurrentPageIndex = e.NewPageIndex
        dtgRisultatoRicerca.DataSource = Session("MyDsCrono")
        dtgRisultatoRicerca.DataBind()

    End Sub


    Private Sub CaricaGriglia()
        Dim dtCSV As DataTable = New DataTable
        Dim rwCSV As DataRow
        Dim clCSV As DataColumn


        Dim linkolumn As New HyperLinkColumn

        Dim strSql As String
        Dim i As Integer

        Dim TmpArr() As String

        Dim Reader As StreamReader
        Dim xLinea As String
        Dim ArrEnte() As String


        clCSV = New DataColumn
        clCSV.DataType = System.Type.GetType("System.DateTime")
        clCSV.ColumnName = "DataCreazione"
        clCSV.Caption = "DataCreazione"
        clCSV.ReadOnly = False
        clCSV.Unique = False
        dtCSV.Columns.Add(clCSV)



        clCSV = New DataColumn
        clCSV.DataType = System.Type.GetType("System.String")
        clCSV.ColumnName = "DataAvvio"
        clCSV.Caption = "DataAvvio"
        clCSV.ReadOnly = False
        clCSV.Unique = False
        dtCSV.Columns.Add(clCSV)


        clCSV = New DataColumn
        clCSV.DataType = System.Type.GetType("System.String")
        clCSV.ColumnName = "NomeUtente"
        clCSV.Caption = "NomeUtente"
        clCSV.ReadOnly = False
        clCSV.Unique = False
        dtCSV.Columns.Add(clCSV)

        clCSV = New DataColumn
        clCSV.DataType = System.Type.GetType("System.String")
        clCSV.ColumnName = "TipoDocumento"
        clCSV.Caption = "TipoDocumento"
        clCSV.ReadOnly = False
        clCSV.Unique = False
        dtCSV.Columns.Add(clCSV)
        clCSV = New DataColumn
        clCSV.DataType = System.Type.GetType("System.String")
        clCSV.ColumnName = "NomeFile"
        clCSV.Caption = "NomeFile"
        clCSV.ReadOnly = False
        clCSV.Unique = False
        dtCSV.Columns.Add(clCSV)
        '
        clCSV = New DataColumn
        clCSV.DataType = System.Type.GetType("System.String")
        clCSV.ColumnName = "IdDoc"
        clCSV.Caption = "IdDoc"
        clCSV.ReadOnly = False
        clCSV.Unique = False
        dtCSV.Columns.Add(clCSV)

        Ordina(Lista)

        Dim myCrono As CronoAssGG

        For i = 0 To Lista.Length - 1
            rwCSV = dtCSV.NewRow
            myCrono = Lista(i)

            Try
                '
                ' 
                '
                rwCSV(0) = myCrono.DataCreazione
                rwCSV(1) = myCrono.DataAvvio
                rwCSV(2) = myCrono.NomeUtente
                rwCSV(3) = myCrono.TipoDocumento
                rwCSV(4) = myCrono.NomeFile
                rwCSV(5) = myCrono.NomeFile.Substring((myCrono.NomeFile.LastIndexOf("\") + 1))
                dtCSV.Rows.Add(rwCSV)
                '
                '
            Catch ex As Exception
                Dim mex As String = ex.Message
            End Try

        Next

        Session("MyDsCrono") = dtCSV

        dtgRisultatoRicerca.DataSource = dtCSV

        dtgRisultatoRicerca.DataBind()

    End Sub
    Private Sub Ordina(ByRef Lista() As CronoAssGG)

        Dim Max As CronoAssGG

        For i As Integer = 0 To Lista.Length - 1
            For j As Integer = i + 1 To Lista.Length - 1
                If Lista(i).DataCreazione < Lista(j).DataCreazione Then
                    Max = Lista(j)
                    Lista(j) = Lista(i)
                    Lista(i) = Max

                End If
            Next

        Next
    End Sub
    Private Sub dtgRisultatoRicerca_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dtgRisultatoRicerca.ItemCommand
        Dim nomedoc As String

        Select Case e.CommandName
            Case "Documento"
                nomedoc = e.Item.Cells(5).Text

                Dim PathInterno As String = Server.MapPath("download") & "\" & nomedoc
                If File.Exists(PathInterno) = False Then
                    File.Copy(ServerPath & "\" & nomedoc, PathInterno)
                End If

                Response.Clear()
                Response.AddHeader("content-disposition", "attachment;filename=" & nomedoc)
                Response.ContentType = "application/vnd.xls"
                Response.Charset = ""
                Response.TransmitFile(PathInterno)
                Response.End()
                'Response.Write("<SCRIPT>" & vbCrLf)
                'Response.Write("window.open('" & PathInterno & "');" & vbCrLf)
                'Response.Write("</SCRIPT>")

        End Select
    End Sub
    Protected Sub cmdChiudi_Click(sender As Object, e As EventArgs) Handles cmdChiudi.Click
        Response.Redirect("WfrmAssicurazioneVolontariGG.aspx")
    End Sub
End Class

Public Class CronoAssGG
    Private _nomeFile As String
    Private _dataCreazione As DateTime
    ' Private _dataAvvio As DateTime

    Sub New(ByVal nomefile As String)
        _nomeFile = nomefile
    End Sub

    Public Property NomeFile() As String
        Get
            Return _nomeFile

        End Get
        Set(ByVal Value As String)
            _nomeFile = Value
        End Set
    End Property
    Public ReadOnly Property DataCreazione() As DateTime

        Get
            Dim arrparm() As String = _nomeFile.Split("_")
            Dim datarray() As Char = arrparm(4).ToCharArray
            _dataCreazione = datarray(0) & datarray(1) & "/" & datarray(2) & datarray(3) & "/" & datarray(4) & datarray(5) & datarray(6) & datarray(7) & " " & datarray(8) & datarray(9) & ":" & datarray(10) & datarray(11) & ":" & datarray(12) & datarray(13)
            Return _dataCreazione
        End Get
    End Property

    Public Property NomeUtente() As String
        Get
            Dim arrparm() As String = _nomeFile.Split("_")
            Return arrparm(1)
        End Get
        Set(ByVal Value As String)

        End Set
    End Property
    Public ReadOnly Property DataAvvio() As String

        Get
            Dim arrparm() As String = _nomeFile.Split("_")
            Dim datarray() As Char = arrparm(2).ToCharArray

            Return datarray(6) & datarray(7) & "/" & datarray(4) & datarray(5) & "/" & datarray(0) & datarray(1) & datarray(2) & datarray(3) '& " " & datarray(8) & datarray(9) & ":" & datarray(10) & datarray(11) & ":" & datarray(12) & datarray(13)
        End Get
    End Property
    Public Property TipoDocumento() As String
        Get
            Dim arrparm() As String = _nomeFile.Split("_")
            Return arrparm(3)
        End Get
        Set(ByVal Value As String)

        End Set
    End Property
End Class