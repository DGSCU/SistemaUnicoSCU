Imports System.IO
Imports System.Data.SqlClient

Public Class WfrmUtilizzoSede
    Inherits System.Web.UI.Page
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
        If Page.IsPostBack = False Then
            CaricaGriglia()
        End If
    End Sub
    Sub CaricaGriglia()
        Dim MyCommand As SqlClient.SqlDataAdapter
        Dim DsUtilizzoSede As DataSet = New DataSet
        MyCommand = New SqlClient.SqlDataAdapter("SP_RITORNA_PROGETTI_SEDE", CType(Session("conn"), SqlClient.SqlConnection))
        MyCommand.SelectCommand.CommandType = CommandType.StoredProcedure
        MyCommand.SelectCommand.Parameters.Add("@IdEnteSede", SqlDbType.Int).Value = Request.QueryString("id")
        MyCommand.Fill(DsUtilizzoSede)
        dtgUtilizzoSede.CurrentPageIndex = 0
        dtgUtilizzoSede.DataSource = DsUtilizzoSede
        'controllo se ci sono dei record
        If DsUtilizzoSede.Tables(0).Rows.Count > 0 Then
            'al datasource sella combo passo il datareader

            Session("RisultatoGriglia") = DsUtilizzoSede
            dtgUtilizzoSede.Caption = "Utilizzo Sede su Progetti Attivi"
        Else
            dtgUtilizzoSede.Caption = " Nessun Progetto per la Sede."
            CmdEsporta.Visible = False
        End If
        dtgUtilizzoSede.DataBind()


        'blocco per la creazione della datatable per la stampa 

        'nome e posizione di lettura delle colopnne a base 0
        Dim NomeColonne(DsUtilizzoSede.Tables(0).Columns.Count) As String
        Dim NomiCampiColonne(DsUtilizzoSede.Tables(0).Columns.Count) As String
        Dim intX As Integer
        For intX = 0 To DsUtilizzoSede.Tables(0).Columns.Count - 1
            NomeColonne(intX) = DsUtilizzoSede.Tables(0).Columns(intX).ColumnName
            NomiCampiColonne(intX) = DsUtilizzoSede.Tables(0).Columns(intX).ColumnName
        Next
        CaricaDataTablePerStampa(DsUtilizzoSede, DsUtilizzoSede.Tables(0).Columns.Count - 1, NomeColonne, NomiCampiColonne)

    End Sub
    Private Sub dtgUtilizzoSede_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dtgUtilizzoSede.PageIndexChanged
        dtgUtilizzoSede.CurrentPageIndex = e.NewPageIndex
        dtgUtilizzoSede.DataSource = Session("RisultatoGriglia")
        dtgUtilizzoSede.DataBind()
        dtgUtilizzoSede.SelectedIndex = -1
    End Sub
    Sub CaricaDataTablePerStampa(ByVal DataSetDaScorrere As DataSet, ByVal NColonne As Integer, ByVal NomiColonne() As String, ByVal NomiCampiColonne() As String)
        Dim dt As New DataTable
        Dim dr As DataRow
        Dim i As Integer
        Dim x As Integer

        'carico i nomi delle colonne che andrò a stampare nella datagrid
        For x = 0 To NColonne
            dt.Columns.Add(New DataColumn(NomiColonne(x), GetType(String)))
        Next

        'carico il datatable con il risultato della stored
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
    Private Sub CmdEsporta_Click(ByVal sender As Object, ByVal e As EventArgs) Handles CmdEsporta.Click
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

        Dim dtrSediAttuazione As Data.SqlClient.SqlDataReader
        Dim Writer As StreamWriter
        Dim xLinea As String
        Dim i As Int64
        Dim j As Int64
        Dim NomeUnivoco As String
        Dim Reader As StreamReader
        Dim url As String
        NomeUnivoco = xPrefissoNome & "ExpDati" & Year(Now) & Month(Now) & Day(Now) & Hour(Now) & Minute(Now) & Second(Now)
        Writer = New StreamWriter(mapPath & "\" & NomeUnivoco & ".CSV")
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
        url = "download\" & NomeUnivoco & ".CSV"

        Writer.Close()
        Writer = Nothing
        Return url
    End Function

End Class