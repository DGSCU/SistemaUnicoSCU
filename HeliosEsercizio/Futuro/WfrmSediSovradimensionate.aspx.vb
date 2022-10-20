Imports System.IO
Imports System.Data.SqlClient

Public Class WfrmSediSovradimensionate
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
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        VerificaSessione()
        If Page.IsPostBack = False Then
            CaricaGriglia()
        End If
    End Sub
    Sub CaricaGriglia()
        Dim MyCommand As SqlClient.SqlDataAdapter
        Dim DsSediSovradimensionate As DataSet = New DataSet
        MyCommand = New SqlClient.SqlDataAdapter("SP_RITORNA_SEGNALAZIONE_SEDI_SOVRADIMENSIONATE", CType(Session("conn"), SqlClient.SqlConnection))
        MyCommand.SelectCommand.CommandType = CommandType.StoredProcedure
        MyCommand.SelectCommand.Parameters.Add("@IdEnte", SqlDbType.Int).Value = Request.QueryString("id")
        MyCommand.Fill(DsSediSovradimensionate)
        dtgSediSovradimensionate.CurrentPageIndex = 0
        'controllo se ci sono dei record
        If DsSediSovradimensionate.Tables(0).Rows.Count > 0 Then
            'al datasource sella combo passo il datareader
            dtgSediSovradimensionate.DataSource = DsSediSovradimensionate
            Session("RisultatoGriglia") = DsSediSovradimensionate
            dtgSediSovradimensionate.Caption = "Sedi Sovradimensionate"
        Else
            cmdEsporta.Visible = False
            LabelNoSedi.Visible = True

        End If
        dtgSediSovradimensionate.PagerStyle.NextPageText = "&gt;&gt;"
        dtgSediSovradimensionate.PagerStyle.PrevPageText = "&lt;&lt;"
        dtgSediSovradimensionate.PagerStyle.HorizontalAlign = HorizontalAlign.Center
        dtgSediSovradimensionate.PagerStyle.CssClass = "linkPageGrid"
        dtgSediSovradimensionate.PagerStyle.Mode = PagerMode.NumericPages
        dtgSediSovradimensionate.DataBind()


        'blocco per la creazione della datatable per la stampa 

        'nome e posizione di lettura delle colopnne a base 0
        Dim NomeColonne(DsSediSovradimensionate.Tables(0).Columns.Count) As String
        Dim NomiCampiColonne(DsSediSovradimensionate.Tables(0).Columns.Count) As String
        Dim intX As Integer
        For intX = 0 To DsSediSovradimensionate.Tables(0).Columns.Count - 1
            NomeColonne(intX) = DsSediSovradimensionate.Tables(0).Columns(intX).ColumnName
            NomiCampiColonne(intX) = DsSediSovradimensionate.Tables(0).Columns(intX).ColumnName
        Next

        'nome della colonna 
        'e posizione nella griglia di lettura
        'NomeColonne(0) = "Ente"
        'NomeColonne(1) = "Denominazione"
        'NomeColonne(2) = "Competenza"
        'NomeColonne(3) = "Indirizzo"
        'NomeColonne(4) = "Civico"
        'NomeColonne(5) = "Palazzina"
        'NomeColonne(6) = "Scala"
        'NomeColonne(7) = "Piano"
        'NomeColonne(8) = "Interno"
        'NomeColonne(9) = "Comune"
        'NomeColonne(10) = "Codice Sede"


        'NomiCampiColonne(0) = "Codice Ente Capofila"
        'NomiCampiColonne(1) = "Nome Ente Capofila"
        'NomiCampiColonne(2) = "Competenza"
        'NomiCampiColonne(3) = "Indirizzo"
        'NomiCampiColonne(4) = "Civico"
        'NomiCampiColonne(5) = "Palazzina"
        'NomiCampiColonne(6) = "Scala"
        'NomiCampiColonne(7) = "Piano"
        'NomiCampiColonne(8) = "Interno"
        'NomiCampiColonne(9) = "Comune"
        'NomiCampiColonne(10) = "CodiceSede"




        'carico un datatable che userò poi nella pagina di stampa
        'il numero delle colonne è a base 0
        CaricaDataTablePerStampa(DsSediSovradimensionate, DsSediSovradimensionate.Tables(0).Columns.Count - 1, NomeColonne, NomiCampiColonne)

        '*********************************************************************************

        'DsSediCondivise = Nothing
    End Sub
    Private Sub dtgSediCondivise_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dtgSediSovradimensionate.PageIndexChanged
        dtgSediSovradimensionate.CurrentPageIndex = e.NewPageIndex
        dtgSediSovradimensionate.DataSource = Session("RisultatoGriglia")
        dtgSediSovradimensionate.DataBind()
        dtgSediSovradimensionate.SelectedIndex = -1
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

    Protected Sub cmdEsporta_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdEsporta.Click
        cmdEsporta.Visible = False
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
            cmdEsporta.Visible = False
        Else
            xPrefissoNome = Session("Utente")
            path = Server.MapPath("download")
            url = CreaFileCSV(dtbRicerca, xPrefissoNome, path)
            ApriCSV1.Visible = True
            ApriCSV1.NavigateUrl = url
        End If
    End Sub
End Class