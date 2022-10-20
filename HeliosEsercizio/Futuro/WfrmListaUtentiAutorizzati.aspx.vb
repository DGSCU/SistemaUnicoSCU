Imports System.IO

Public Class WfrmListaUtentiAutorizzati
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim sqlDAP As New SqlClient.SqlDataAdapter
        Dim dataSet As New DataSet
        Dim strNomeStore As String = "[SP_LISTA_UTENTI_AUTORIZZATI]"
        Try
            sqlDAP = New SqlClient.SqlDataAdapter(strNomeStore, CType(Session("conn"), SqlClient.SqlConnection))
            sqlDAP.SelectCommand.CommandType = CommandType.StoredProcedure
            sqlDAP.SelectCommand.Parameters.Add("@IdEnte", SqlDbType.Int).Value = Integer.Parse(Session("IdEnte"))
            sqlDAP.Fill(dataSet)
            Session("appDtsRisRicerca") = dataSet
            dtgRisultatoRicerca.DataSource = dataSet
            dtgRisultatoRicerca.DataBind()
        Catch ex As Exception
            Response.Write(ex.Message.ToString())
            Exit Sub
        End Try

    End Sub

    Protected Sub dtgRisultatoRicerca_SelectedIndexChanged(sender As Object, e As EventArgs) Handles dtgRisultatoRicerca.SelectedIndexChanged

    End Sub

    Protected Sub CmdEsporta_Click(sender As Object, e As EventArgs) Handles CmdEsporta.Click
        'CmdEsporta.Visible = False
        CreaStampaCSV(Session("appDtsRisRicerca"))
        Dim dtbRicerca As DataTable = Session("DtbRicerca")
        StampaCSV(dtbRicerca)
    End Sub

    Private Sub CreaStampaCSV(ByVal MyDataSet As DataSet)
        Dim NomeColonne(6) As String
        Dim NomiCampiColonne(6) As String
        'nome della colonna 
        'e posizione nella griglia di lettura
        NomeColonne(0) = "Nominativo"
        NomeColonne(1) = "Codice Fiscale"
        NomeColonne(2) = "Ruolo"
        NomeColonne(3) = "Codice Fiscale Ente"
        NomeColonne(4) = "Cod. Ente"
        NomeColonne(5) = "Denominazione Ente"
        NomeColonne(6) = "Dettagli"

        NomiCampiColonne(0) = "Nominativo"
        NomiCampiColonne(1) = "CF_Persona"
        NomiCampiColonne(2) = "TipoUtente"
        NomiCampiColonne(3) = "CF_ENTE"
        NomiCampiColonne(4) = "CodiceEnte"
        NomiCampiColonne(5) = "DenominazioneEnte"
        NomiCampiColonne(6) = "Dettagli"
        'carico un datatable che userò poi nella pagina di stampa
        'il numero delle colonne è a base 0
        Session("DtbRicerca") = ClsServer.CaricaDataTablePerStampa(MyDataSet, 6, NomeColonne, NomiCampiColonne)
    End Sub

    Private Sub StampaCSV(ByVal dtbRicerca As DataTable)
        Dim path As String
        Dim xPrefissoNome As String
        Dim url As String
        Dim utility As ClsUtility = New ClsUtility()

        If dtbRicerca.Rows.Count = 0 Then
            ApriCSV1.Visible = False
            'CmdEsporta.Visible = False
        Else
            xPrefissoNome = Session("Utente")
            path = Server.MapPath("download")
            url = CreaFileCSV(dtbRicerca, xPrefissoNome, path)
            ApriCSV1.Visible = True
            ApriCSV1.NavigateUrl = url
        End If
    End Sub

    Function CreaFileCSV(ByVal DTBRicerca As DataTable, ByVal xPrefissoNome As String, ByVal mapPath As String) As String

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