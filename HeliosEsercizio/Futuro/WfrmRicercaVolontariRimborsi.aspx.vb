Imports System.IO

Public Class WfrmRicercaVolontariRimborsi
    Inherits System.Web.UI.Page

    Dim dtrGenerico As SqlClient.SqlDataReader
    Dim strquery As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If (Session("TipoUtente") = "U" Or Session("TipoUtente") = "R") Then
            divTipoUtente.Visible = True
        Else
            divTipoUtente.Visible = False
        End If

        'Inserire qui il codice utente necessario per inizializzare la pagina
        If Not Session("LogIn") Is Nothing Then
            If Session("LogIn") = False Then 'verifico validità log-in
                Response.Redirect("LogOn.aspx")
            End If
        Else
            Response.Redirect("LogOn.aspx")
        End If
        If IsPostBack = False Then
            If Request.QueryString("VengoDa") = "Inserimento" Then
                PersonalizzaMaschera()
            Else
                ddlstato.Items.Add("Seleziona")
                ddlstato.Items.Add("Proposto")
                ddlstato.Items.Add("Confermato")
                ddlstato.Items.Add("Respinto")
            End If
            'CaricaGriglia()
        End If

    End Sub

    Private Sub PersonalizzaMaschera()
        'Generato da Alessandra Taballione il 02/12/2004
        'Personalizzazione della Maschera in fase di Ricerca per Modifica Rimborso.
        lblAl.Visible = False
        lbldal.Visible = False
        lblStato.Visible = False
        txtdatadal.Visible = False
        txtdataal.Visible = False
        ddlstato.Visible = False
        divDataRiferimento.Visible = False
    End Sub

    Sub CaricaGriglia()
        Dim strSql As String
        Dim strWhere As String
        Dim MyDataSet As DataSet

        lblMessaggiErrore.Visible = False

        'DESCRIZIONE: routine che carica la griglia con tutti i progetti con stato attività=1
        'AUTORE: Michele d'Ascenzio    
        'DATA: 03/11/2004
        'Modificata da Alessandra taballione il 02/12/2004
        Try

            'Richiamo routine per la ricerca dei volontati e assegno al dataset i risultato della store 
            MyDataSet = LoadRiceraVolontariRimborsi(Request.QueryString("VengoDa"))

            dgVolontari.DataSource = MyDataSet
            Session("appDtsRisRicerca") = MyDataSet

            'If (Session("TipoUtente") = "U" Or Session("TipoUtente") = "R") Then
            '    dgVolontari.Columns(10).Visible = True
            'Else
            '    dgVolontari.Columns(10).Visible = False
            'End If

            dgVolontari.DataBind()

            Dim NomeColonne(10) As String
            Dim NomiCampiColonne(10) As String
            'nome della colonna 
            'e posizione nella griglia di lettura
            NomeColonne(0) = "Codice Volontario"
            NomeColonne(1) = "Nominativo"
            NomeColonne(2) = "Cod. Fiscale"
            NomeColonne(3) = "Data Nascita"
            NomeColonne(4) = "Comune Nascita"
            NomeColonne(5) = "Progetto"
            NomeColonne(6) = "Ente"
            NomeColonne(7) = "Tot Rimborsi"
            NomeColonne(8) = "Rimborsi Confermati"
            NomeColonne(9) = "Rimborsi Respinti"
            NomeColonne(10) = "Rimborsi Proposti"

            NomiCampiColonne(0) = "CodiceVolontario"
            NomiCampiColonne(1) = "Nominativo"
            NomiCampiColonne(2) = "CodiceFiscale"
            NomiCampiColonne(3) = "DataNascita"
            NomiCampiColonne(4) = "ComuneNascita"
            NomiCampiColonne(5) = "Progetto"
            NomiCampiColonne(6) = "Ente"
            NomiCampiColonne(7) = "TotRimborsi"
            NomiCampiColonne(8) = "NConfermati"
            NomiCampiColonne(9) = "NRespinti"
            NomiCampiColonne(10) = "NProposti"
            'carico un datatable che userò poi nella pagina di stampa
            'il numero delle colonne è a base 0
            Session("DtbRicerca") = ClsServer.CaricaDataTablePerStampa(MyDataSet, 10, NomeColonne, NomiCampiColonne)


            If dgVolontari.Items.Count = 0 Then
                dgVolontari.Visible = False
                lblmessaggio.Text = "Nessun Dato estratto."
                CmdEsporta.Visible = False
            Else
                dgVolontari.Visible = True
                lblmessaggio.Text = "Risultato Ricerca Rimborsi Volontari."
                CmdEsporta.Visible = True
            End If

        Catch ex As Exception
        End Try
    End Sub

    Private Sub cmdChiudi_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdChiudi.Click
        'chiamo homepage
        Response.Redirect("WfrmMain.aspx")
    End Sub

    Private Sub dgVolontari_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgVolontari.PageIndexChanged
        'AUTORE: MIchele d'Ascenzio
        'DATA: 03/11/2004
        'Cambia pag della Griglia
        dgVolontari.CurrentPageIndex = e.NewPageIndex
        dgVolontari.DataSource = Session("appDtsRisRicerca")
        dgVolontari.DataBind()
        dgVolontari.SelectedIndex = -1
    End Sub

    Private Sub cmdRicerca_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdRicerca.Click

        If controlliSalvataggioServer() = True Then
            CaricaGriglia()
        End If

    End Sub

    Private Sub dgVolontari_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgVolontari.SelectedIndexChanged
        If (Session("TipoUtente") = "U" Or Session("TipoUtente") = "R") Then
            Session("IdEnte") = dgVolontari.SelectedItem.Cells(2).Text
            Session("Denominazione") = dgVolontari.SelectedItem.Cells(11).Text
        End If
        Response.Redirect("WfrmGestioneRimborsoVolontari.aspx?IdAttivita=" & dgVolontari.SelectedItem.Cells(1).Text & "&IdEntita=" & dgVolontari.SelectedItem.Cells(0).Text & "&VengoDa=" & Request.QueryString("VengoDa") & "&Op=" & Request.QueryString("Op"))
    End Sub

    Private Sub CmdEsporta_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdEsporta.Click
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

    Function controlliSalvataggioServer() As Boolean

        Dim dataRiferimentoDal As Date
        If txtdatadal.Text.Trim <> String.Empty Then
            If (Date.TryParse(txtdatadal.Text, dataRiferimentoDal) = False) Then
                lblMessaggiErrore.Visible = True
                lblMessaggiErrore.Text = "Il formato della data è incorretto: il formato deve essere GG/MM/AAAA."
                txtdatadal.Focus()
                Return False
            End If
        End If
       
        Dim dataRiferimentoAl As Date
        If txtdataal.Text.Trim <> String.Empty Then
            If (Date.TryParse(txtdataal.Text, dataRiferimentoAl) = False) Then
                lblMessaggiErrore.Visible = True
                lblMessaggiErrore.Text = "Il formato della data è incorretto: il formato deve essere GG/MM/AAAA."
                txtdataal.Focus()
                Return False
            End If
        End If
       

        Return True

    End Function


    Private Function LoadRiceraVolontariRimborsi(ByVal VengoDA As String) As DataSet
        'REALIZZATA DA: DOMENICO DI FAZIO 
        'DATA REALIZZAZIONE:  25/03/2015
        'FUNZIONALITA': RICHIAMO STORE PER LA RICERCA DEI VOLONTARI GESTIONE RIMBORSI
        Dim sqlDAP As New SqlClient.SqlDataAdapter
        Dim dataSet As New DataSet
        Dim strNomeStore As String = "[SP_RICERCA_VOLONTARI_RIMBORSI]"
        Dim StatoRimborso As String

        Try
            sqlDAP = New SqlClient.SqlDataAdapter(strNomeStore, CType(Session("conn"), SqlClient.SqlConnection))
            sqlDAP.SelectCommand.CommandType = CommandType.StoredProcedure
            sqlDAP.SelectCommand.Parameters.Add("@UserName", SqlDbType.VarChar).Value = Session("Utente")
            sqlDAP.SelectCommand.Parameters.Add("@IDEnte", SqlDbType.VarChar).Value = Session("IdEnte")
            sqlDAP.SelectCommand.Parameters.Add("@NomeEnte", SqlDbType.VarChar).Value = txtEnte.Text.Replace("'", "''")
            sqlDAP.SelectCommand.Parameters.Add("@CodiceEnte", SqlDbType.VarChar).Value = txtCodEnte.Text.Replace("'", "''")

            sqlDAP.SelectCommand.Parameters.Add("@Cognome", SqlDbType.VarChar).Value = txtCognome.Text.Replace("'", "''")
            sqlDAP.SelectCommand.Parameters.Add("@Nome", SqlDbType.VarChar).Value = txtNome.Text.Replace("'", "''")
            sqlDAP.SelectCommand.Parameters.Add("@CodiceVolontario", SqlDbType.VarChar).Value = TxtCodVolontario.Text.Replace("'", "''")
            sqlDAP.SelectCommand.Parameters.Add("@Titolo", SqlDbType.VarChar).Value = txtProgetto.Text.Replace("'", "''")
            sqlDAP.SelectCommand.Parameters.Add("@CodiceProgetto", SqlDbType.VarChar).Value = txtCodProgetto.Text.Replace("'", "''")

            sqlDAP.SelectCommand.Parameters.Add("@DataRiferimentoDAL", SqlDbType.VarChar).Value = txtdatadal.Text
            sqlDAP.SelectCommand.Parameters.Add("@DataRiferimentoAL", SqlDbType.VarChar).Value = txtdataal.Text

            If ddlstato.Visible = False Then
                StatoRimborso = "0"
            Else
                Select Case ddlstato.SelectedItem.Text
                    Case "Seleziona"
                        StatoRimborso = "0"
                    Case "Proposto"
                        StatoRimborso = "1"
                    Case "Confermato"
                        StatoRimborso = "2"
                    Case "Respinto"
                        StatoRimborso = "3"
                End Select
            End If

            sqlDAP.SelectCommand.Parameters.Add("@StatoRimborso", SqlDbType.VarChar).Value = StatoRimborso
            sqlDAP.SelectCommand.Parameters.Add("@VengoDA", SqlDbType.VarChar).Value = VengoDA
            sqlDAP.SelectCommand.Parameters.Add("@FiltroVisibilita", SqlDbType.VarChar).Value = Session("FiltroVisibilita")
            sqlDAP.Fill(dataSet)

            LoadRiceraVolontariRimborsi = dataSet


        Catch ex As Exception
            Response.Write(ex.Message.ToString())
            Exit Function
        End Try
    End Function
End Class