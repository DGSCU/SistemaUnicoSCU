Imports System.IO
Imports System.Text.RegularExpressions
Imports System.Data.SqlClient
Public Class wfrmExportRimodulazioneProgetti
    Inherits System.Web.UI.Page
    Dim myDataSet As New DataSet
    Dim myDataTable As DataTable
    Dim myDataReader As SqlClient.SqlDataReader
    Private Writer As StreamWriter
    Private strNote As String
    Private Tot As Integer
    Private TotOk As Integer
    Private TotKo As Integer
    Private NomeUnivoco As String
    Dim dtrgenerico As SqlClient.SqlDataReader
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Session("LogIn") Is Nothing Then
            If Session("LogIn") = False Then 'verifico validità log-in
                Response.Redirect("LogOn.aspx")
            End If
        Else
            Response.Redirect("LogOn.aspx")
        End If
    End Sub
    Protected Sub imgEsporta_Click(sender As Object, e As EventArgs) Handles imgEsporta.Click
        EsportazioneTotRimodulazione()
    End Sub

    Protected Sub CmdChiudi_Click(sender As Object, e As EventArgs) Handles CmdChiudi.Click
        Response.Redirect("WfrmMain.aspx")
    End Sub
    Sub EsportazioneTotRimodulazione()
        Dim arrParam(0) As SqlParameter
        Dim nomeFile As String = String.Empty

        arrParam(0) = New SqlClient.SqlParameter
        arrParam(0).ParameterName = "@TUTTI"
        arrParam(0).SqlDbType = SqlDbType.VarChar
        arrParam(0).Value = ddlSiNo.SelectedValue.ToString

        myDataTable = New DataTable("ExportTot")
        myDataTable = ExecuteDataTable("SP_RimodulazionePROGETTI_STATO_CARICAMENTI", arrParam)
        myDataSet.Tables.Add(myDataTable)
        OutputXls("ExpTotaleRimodulazione", nomeFile)

        If myDataTable.Rows.Count > 0 Then
            hlVolontari.NavigateUrl = "download" & "\" + nomeFile
            hlVolontari.Target = "_blank"
            hlVolontari.Visible = True

        Else
            lblMessaggioErrore.Visible = True
            lblMessaggioErrore.Text = " Non risultano Progetti disponibili per Export Rimodulazione"
            hlVolontari.Visible = False
            imgEsporta.Visible = False

        End If

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

    Private Sub OutputXls(ByVal tipofile As String, ByRef nomeFile As String)
        nomeFile = Session("Utente") & "_" & tipofile & "_" & Format(DateTime.Now, "ddMMyyyyhhmmss") & ".csv"
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
            myWriter = New StreamWriter(Server.MapPath("download") & "\" & nomeUnivoco, True, System.Text.Encoding.Default)
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

    Private Function CreaArray(ByVal pLinea As String) As String()
        Dim TmpArr As String()
        Dim DefArr As String()
        Dim i As Integer
        Dim x As Integer

        TmpArr = Split(pLinea, ";")

        For i = 0 To UBound(TmpArr)
            If i = 0 Then
                ReDim DefArr(0)
            Else
                ReDim Preserve DefArr(UBound(DefArr) + 1)
            End If
            If Left(TmpArr(i), 1) = Chr(34) Then
                x = i
                Do While Right(TmpArr(x), 1) <> Chr(34)
                    If x = i Then
                        DefArr(UBound(DefArr)) = Mid(TmpArr(x), 2) & "; "
                    Else
                        DefArr(UBound(DefArr)) = DefArr(UBound(DefArr)) & TmpArr(x) & "; "
                    End If
                    x = x + 1
                Loop
                DefArr(UBound(DefArr)) = DefArr(UBound(DefArr)) & Mid(TmpArr(x), 1, Len(TmpArr(x)) - 1)
                i = x
            Else
                DefArr(UBound(DefArr)) = TmpArr(i)
            End If
        Next

        CreaArray = DefArr

    End Function
End Class