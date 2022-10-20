Imports System.IO
Public Class RicercaVolontariDocumenti
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If (Session("TipoUtente") = "U" Or Session("TipoUtente") = "R") Then
            divTipoUtente.Visible = True
        Else
            divTipoUtente.Visible = False
        End If

        lblfami.Visible = (Session("Sistema") = "Helios")
        ddlFami.Visible = (Session("Sistema") = "Helios")

        'Inserire qui il codice utente necessario per inizializzare la pagina
        If Not Session("LogIn") Is Nothing Then
            If Session("LogIn") = False Then 'verifico validità log-in
                Response.Redirect("LogOn.aspx")
            End If
        Else
            Response.Redirect("LogOn.aspx")
        End If
        If Page.IsPostBack = False Then
            CaricaComboTipoDocumenti()
        End If

    End Sub

    Private Sub LoadRiceraVolontari()
        'REALIZZATA DA: SIMONA CORDELLA 
        'DATA REALIZZAZIONE:  04/03/2015
        'FUNZIONALITA': RICHIAMO STORE PER LA RICERCA DEI VOLONTARI 
        Dim sqlDAP As New SqlClient.SqlDataAdapter
        Dim dataSet As New DataSet
        Dim strNomeStore As String = "[SP_RICERCA_VOLONTARI_DOCUMENTI]"


        Try
            sqlDAP = New SqlClient.SqlDataAdapter(strNomeStore, CType(Session("conn"), SqlClient.SqlConnection))
            sqlDAP.SelectCommand.CommandType = CommandType.StoredProcedure

            sqlDAP.SelectCommand.Parameters.Add("@UserName", SqlDbType.VarChar).Value = Session("Utente")
            sqlDAP.SelectCommand.Parameters.Add("@Ente", SqlDbType.VarChar).Value = txtEnte.Text.Replace("'", "''")
            sqlDAP.SelectCommand.Parameters.Add("@CodiceEnte", SqlDbType.VarChar).Value = txtCodEnte.Text.Replace("'", "''")
            sqlDAP.SelectCommand.Parameters.Add("@Cognome", SqlDbType.VarChar).Value = txtCognome.Text.Replace("'", "''")
            sqlDAP.SelectCommand.Parameters.Add("@Nome", SqlDbType.VarChar).Value = txtNome.Text.Replace("'", "''")
            sqlDAP.SelectCommand.Parameters.Add("@CodiceVolontario", SqlDbType.VarChar).Value = TxtCodVolontario.Text.Replace("'", "''")
            sqlDAP.SelectCommand.Parameters.Add("@CodiceProgetto", SqlDbType.VarChar).Value = txtCodProgetto.Text.Replace("'", "''")
            sqlDAP.SelectCommand.Parameters.Add("@TitoloProgetto", SqlDbType.VarChar).Value = txtProgetto.Text.Replace("'", "''")
            sqlDAP.SelectCommand.Parameters.Add("@CodiceSede", SqlDbType.VarChar).Value = TxtCodiceSedeAttuazione.Text
            sqlDAP.SelectCommand.Parameters.Add("@IDEnte", SqlDbType.VarChar).Value = Session("IdEnte")
            sqlDAP.SelectCommand.Parameters.Add("@PrefissoMancante", SqlDbType.VarChar).Value = ddlPrefissiDocumenti.SelectedItem.Text.Replace("'", "''")
            sqlDAP.SelectCommand.Parameters.Add("@Sistema", SqlDbType.VarChar).Value = Session("Sistema")
            sqlDAP.SelectCommand.Parameters.Add("@FAMI", SqlDbType.VarChar).Value = ddlFami.SelectedValue
            sqlDAP.Fill(dataSet)

            Session("appDtsRisRicerca") = dataSet
            dgVolontari.DataSource = dataSet
            dgVolontari.DataBind()

        Catch ex As Exception
            Response.Write(ex.Message.ToString())
            Exit Sub
        End Try
    End Sub

    Protected Sub cmdRicerca_Click(sender As Object, e As EventArgs) Handles cmdRicerca.Click
        dgVolontari.CurrentPageIndex = 0
        LoadRiceraVolontari()
        If dgVolontari.Items.Count = 0 Then
            lblmessaggio.Text = "Nessun Dato estratto."
            CmdEsporta.Visible = False
        Else
            lblmessaggio.Text = "Risultato Ricerca Volontari."
            dgVolontari.Visible = True
            CmdEsporta.Visible = True
        End If
    End Sub

    Private Sub dgVolontari_ItemCommand(source As Object, e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgVolontari.ItemCommand
      Select e.CommandName
            Case "Seleziona"
                'Response.Redirect("Presenze.aspx?IdEntita=" & dgVolontari.SelectedItem.Cells(0).Text)
            Case "Consulta"
                If (Session("TipoUtente") = "U" Or Session("TipoUtente") = "R") Then
                    Session("IdEnte") = e.Item.Cells(8).Text
                    Session("Denominazione") = e.Item.Cells(9).Text
                End If
                Response.Redirect("WfrmVisualizzaElencoDocumentiVolontario.aspx?VengoDa=RicercaVolontariDocumenti&IdVol=" & e.Item.Cells(0).Text)
        End Select


    End Sub

    Private Sub dgVolontari_PageIndexChanged(source As Object, e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgVolontari.PageIndexChanged
        dgVolontari.CurrentPageIndex = e.NewPageIndex
        dgVolontari.DataSource = Session("appDtsRisRicerca")
        dgVolontari.DataBind()
        dgVolontari.SelectedIndex = -1
    End Sub

    Protected Sub CmdEsporta_Click(sender As Object, e As EventArgs) Handles CmdEsporta.Click

        CmdEsporta.Visible = False
        CreaStampaCSV(Session("appDtsRisRicerca"))
        Dim dtbRicerca As DataTable = Session("DtbRicerca")
        StampaCSV(dtbRicerca)
    End Sub

    Private Sub CreaStampaCSV(ByVal MyDataSet As DataSet)
        Dim NomeColonne(5) As String
        Dim NomiCampiColonne(5) As String
        'nome della colonna 
        'e posizione nella griglia di lettura
        NomeColonne(0) = "Codice Volontario"
        NomeColonne(1) = "Nominativo"
        NomeColonne(2) = "Cod. Fiscale"
        NomeColonne(3) = "Data inizio Servizio"
        NomeColonne(4) = "Data Fine Servizio"
        NomeColonne(5) = "Totoale Documenti"
 

        NomiCampiColonne(0) = "CodiceVolontario"
        NomiCampiColonne(1) = "Nominativo"
        NomiCampiColonne(2) = "CodiceFiscale"
        NomiCampiColonne(3) = "Datainizioservizio"
        NomiCampiColonne(4) = "Datafineservizio"
        NomiCampiColonne(5) = "totaledocumentipresenti"
 
        'carico un datatable che userò poi nella pagina di stampa
        'il numero delle colonne è a base 0
        Session("DtbRicerca") = ClsServer.CaricaDataTablePerStampa(MyDataSet, 5, NomeColonne, NomiCampiColonne)
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
    Private Sub CaricaComboTipoDocumenti()
        Dim strSql As String
        Dim rstDoc As SqlClient.SqlDataReader


        ddlPrefissiDocumenti.Items.Clear()
        strSql = "SELECT idPrefisso as idPrefisso, Prefisso as Prefisso, ordine   FROM  PrefissiEntitàDocumenti where tipoinserimento=0 and Sistema='" & Session("Sistema") & "' UNION Select '', '',0 order by Ordine"
        rstDoc = ClsServer.CreaDatareader(strSql, Session("conn"))


        If rstDoc.HasRows Then
            ddlPrefissiDocumenti.DataSource = rstDoc
            ddlPrefissiDocumenti.DataTextField = "Prefisso"
            ddlPrefissiDocumenti.DataValueField = "IdPrefisso"
            ddlPrefissiDocumenti.DataBind()
        End If

        If Not rstDoc Is Nothing Then
            rstDoc.Close()
            rstDoc = Nothing
        End If
    End Sub

    Protected Sub cmdChiudi_Click(sender As Object, e As EventArgs) Handles cmdChiudi.Click
        'chiamo homepage
        Response.Redirect("WfrmMain.aspx")
    End Sub
End Class