Imports System.IO
Public Class WfrmVisualizzaPresenzeMeseVol
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Page.IsPostBack = False Then
            DtgDettaglioPresenze.DataSource = LoadStoreDettaglioPresenze(Session("idEnte"), Request.QueryString("AnnoSel"), Request.QueryString("MeseSel"))
            DtgDettaglioPresenze.DataBind()
        End If
    End Sub
    Private Function LoadStoreDettaglioPresenze(ByVal IdEnte As Integer, ByVal anno As Integer, ByVal mese As Integer) As DataSet
        Dim sqlDAP As New SqlClient.SqlDataAdapter
        Dim dataSet As New DataSet
        Dim strNomeStore As String = "[SP_PRESENZE_DETTAGLIO_MENSILE_ENTE]"
        Dim meseEsteso As String
        Try

            sqlDAP = New SqlClient.SqlDataAdapter(strNomeStore, CType(Session("conn"), SqlClient.SqlConnection))
            sqlDAP.SelectCommand.CommandType = CommandType.StoredProcedure
            sqlDAP.SelectCommand.Parameters.Add("@Idente", SqlDbType.Int).Value = IdEnte
            sqlDAP.SelectCommand.Parameters.Add("@anno", SqlDbType.Int).Value = anno
            sqlDAP.SelectCommand.Parameters.Add("@mese", SqlDbType.Int).Value = mese
            'sqlDAP.SelectCommand.Parameters.Add("@ElencoCursore", ).Direction = ParameterDirection.Output

            sqlDAP.Fill(dataSet)
            Session("appDtsRisRicerca") = dataSet
            Select Case mese

                Case 1
                    meseEsteso = "Gennaio"
                Case 2
                    meseEsteso = "Febbraio"
                Case 3
                    meseEsteso = "Marzo"
                Case 4
                    meseEsteso = "Aprile"
                Case 5
                    meseEsteso = "Maggio"
                Case 6
                    meseEsteso = "Giugno"
                Case 7
                    meseEsteso = "Luglio"
                Case 8
                    meseEsteso = "Agosto"
                Case 9
                    meseEsteso = "Settembre"
                Case 10
                    meseEsteso = "Ottobre"
                Case 11
                    meseEsteso = "Novembre"
                Case 12
                    meseEsteso = "Dicembre"

            End Select

            lblAnnoMese.Text = meseEsteso + " " + CStr(anno)

            Return dataSet

        Catch ex As Exception
            Response.Write(ex.Message.ToString())
            Exit Function
        End Try
    End Function
    Protected Sub cmdChiudi_Click(sender As Object, e As EventArgs) Handles cmdChiudi.Click
        Response.Redirect("WfrmConfermaGeneralePresenzeMensili.aspx")
    End Sub
    Protected Sub CmdEsporta_Click(sender As Object, e As EventArgs) Handles CmdEsporta.Click
        CmdEsporta.Visible = False
        CreaStampaCSV(Session("appDtsRisRicerca"))
        Dim dtbRicerca As DataTable = Session("DtbRicerca")
        StampaCSV(dtbRicerca)
    End Sub

    Private Sub CreaStampaCSV(ByVal MyDataSet As DataSet)
        Dim NomeColonne(7) As String
        Dim NomiCampiColonne(7) As String
        'nome della colonna 
        'e posizione nella griglia di lettura
        NomeColonne(0) = "Codice Volontario"
        NomeColonne(1) = "Nominativo"
        NomeColonne(2) = "Cod. Fiscale"
        NomeColonne(3) = "Data Inizio Servizio"
        NomeColonne(4) = "Data Fine Servizio"
        NomeColonne(5) = "Giorni Totali Indicati"
        NomeColonne(6) = "Giorni Previsti"
        NomeColonne(7) = "Esistenza Foglio Presenze"


        NomiCampiColonne(0) = "CodiceVolontario"
        NomiCampiColonne(1) = "Nominativo"
        NomiCampiColonne(2) = "CodiceFiscale"
        NomiCampiColonne(3) = "DataInizioServizio"
        NomiCampiColonne(4) = "DataFineServizio"
        NomiCampiColonne(5) = "GiorniTotaliIndicati"
        NomiCampiColonne(6) = "GiorniPrevisti"
        NomiCampiColonne(7) = "EsistenzaFoglioPresenza"

        'carico un datatable che userò poi nella pagina di stampa
        'il numero delle colonne è a base 0
        Session("DtbRicerca") = ClsServer.CaricaDataTablePerStampa(MyDataSet, 7, NomeColonne, NomiCampiColonne)
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