Imports System.IO
Imports System.Text.RegularExpressions
Imports System.Data.SqlClient
Public Class WfrmConfermaPreliminare
    Inherits System.Web.UI.Page
    Private strIDselezionati As String
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
        If Page.IsPostBack = False Then
            CaricaCombo()
            If Request.QueryString("VengoDa") = "ConfermaPreliminareMultipla" Then
                LoadRiceraVolontari()
            End If
        End If

    End Sub
    Sub CaricaCombo()
        Dim SqlCmd As New SqlClient.SqlCommand
        Dim mioAdapter As SqlClient.SqlDataAdapter
        Dim mytable As DataTable
        Dim Reader As SqlClient.SqlDataReader
        Try

            SqlCmd = New SqlClient.SqlCommand("SP_PRESENZE_MESI_V2", Session("Conn"))
            SqlCmd.CommandType = CommandType.StoredProcedure
            mioAdapter = New SqlClient.SqlDataAdapter(SqlCmd)
            mytable = New DataTable()
            mioAdapter.Fill(mytable)

            'Reader = SqlCmd.ExecuteReader()

            CboAnnoMese.DataTextField = "Mensilità"
            CboAnnoMese.DataValueField = "Mensilità"
            CboAnnoMese.DataSource = mytable
            CboAnnoMese.DataBind()
            'ChiudiDataReader(Reader)

            SqlCmd.Dispose()

            If Request.QueryString("VengoDa") = "ConfermaPreliminareMultipla" Then
                CboAnnoMese.SelectedValue = Request.QueryString("AnnoMese")
                CboAnnoMese.SelectedItem.Value = Request.QueryString("AnnoMese")

            End If



        Catch ex As Exception

        End Try

    End Sub
    Private Sub chkSelectAll_CheckedChanged(sender As Object, e As System.EventArgs) Handles chkSelectAll.CheckedChanged
        For Each riga As DataGridItem In dgVolontari.Items
            CType(riga.FindControl("chkSelProg"), CheckBox).Checked = chkSelectAll.Checked
        Next
    End Sub

    Private Sub LoadRiceraVolontari()

        'FUNZIONALITA': RICHIAMO STORE PER LA RICERCA DEI VOLONTARI 
        Dim sqlDAP As New SqlClient.SqlDataAdapter
        Dim dataSet As New DataSet
        Dim strNomeStore As String = "[SP_RICERCA_VOLONTARI_CONFERMA_PRELIMINARE]"

        Try

            sqlDAP = New SqlClient.SqlDataAdapter(strNomeStore, CType(Session("conn"), SqlClient.SqlConnection))
            sqlDAP.SelectCommand.CommandType = CommandType.StoredProcedure
            sqlDAP.SelectCommand.Parameters.Add("@UserName", SqlDbType.VarChar).Value = Session("Utente")
            sqlDAP.SelectCommand.Parameters.Add("@Cognome", SqlDbType.VarChar).Value = txtCognome.Text.Replace("'", "''")
            sqlDAP.SelectCommand.Parameters.Add("@Nome", SqlDbType.VarChar).Value = txtNome.Text.Replace("'", "''")
            sqlDAP.SelectCommand.Parameters.Add("@CodiceVolontario", SqlDbType.VarChar).Value = TxtCodVolontario.Text.Replace("'", "''")
            sqlDAP.SelectCommand.Parameters.Add("@NomeEnte", SqlDbType.VarChar).Value = txtEnte.Text.Replace("'", "''")
            sqlDAP.SelectCommand.Parameters.Add("@CodiceEnte", SqlDbType.VarChar).Value = txtCodEnte.Text.Replace("'", "''")
            sqlDAP.SelectCommand.Parameters.Add("@IDEnte", SqlDbType.VarChar).Value = Session("IdEnte")
            sqlDAP.SelectCommand.Parameters.Add("@Progetto", SqlDbType.VarChar).Value = txtProgetto.Text.Replace("'", "''")
            sqlDAP.SelectCommand.Parameters.Add("@CodiceProgetto", SqlDbType.VarChar).Value = txtCodProgetto.Text.Replace("'", "''")
            sqlDAP.SelectCommand.Parameters.Add("@IDEnteSedeAttuazione", SqlDbType.VarChar).Value = TxtCodiceSedeAttuazione.Text
            sqlDAP.SelectCommand.Parameters.Add("@AnnoMese", SqlDbType.VarChar).Value = CboAnnoMese.SelectedValue
            sqlDAP.SelectCommand.Parameters.Add("@StatoConfermaPreliminare", SqlDbType.VarChar).Value = CboStatoConfermaPreliminare.SelectedValue



            'ADC VA SCOMMENTATA la riga sopra NEL MOMENTO IN CUI ABBIAMO LA STORED GIUSTA
            sqlDAP.SelectCommand.Parameters.Add("@FiltroVisibilita", SqlDbType.VarChar).Value = Session("FiltroVisibilita")
            sqlDAP.Fill(dataSet)

            Session("appDtsRisRicerca") = dataSet
            dgVolontari.DataSource = dataSet
            dgVolontari.DataBind()

            If dgVolontari.Items.Count = 0 Then
                lblmessaggio.Text = "Nessun Dato estratto."
                CmdEsporta.Visible = False
                chkSelectAll.Visible = False
                CmdConfermaPreliminare.Visible = False
            Else
                lblmessaggio.Text = "Risultato Ricerca Volontari."
                dgVolontari.Visible = True
                CmdEsporta.Visible = True
                chkSelectAll.Visible = True
                chkSelectAll.Checked = False
                CmdConfermaPreliminare.Visible = True
            End If
            ViewState("datasource") = dataSet
        Catch ex As Exception
            Response.Write(ex.Message.ToString())
            Exit Sub
        End Try
    End Sub

    Protected Sub cmdRicerca_Click(sender As Object, e As EventArgs) Handles cmdRicerca.Click
        dgVolontari.CurrentPageIndex = 0
        LoadRiceraVolontari()
       
    End Sub
    'ADC ESCLUSO PER PROBLEMI DI MEMORIZZAZIONE VOLONTARI CECCATI NELLE VARIE PAGINE
    'Private Sub dgVolontari_PageIndexChanged(source As Object, e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgVolontari.PageIndexChanged
    '    dgVolontari.CurrentPageIndex = e.NewPageIndex
    '    dgVolontari.DataSource = Session("appDtsRisRicerca")
    '    dgVolontari.DataBind()
    '    dgVolontari.SelectedIndex = -1
    'End Sub

    Protected Sub dgVolontari_SelectedIndexChanged(sender As Object, e As EventArgs) Handles dgVolontari.SelectedIndexChanged
        If (Session("TipoUtente") = "U" Or Session("TipoUtente") = "R") Then
            Session("IdEnte") = dgVolontari.SelectedItem.Cells(2).Text
            Session("Denominazione") = dgVolontari.SelectedItem.Cells(11).Text
        End If
        Dim stranno As String = CboAnnoMese.SelectedValue.ToString.Substring(0, 4)
        Dim strmese As String = CboAnnoMese.SelectedValue.ToString.Substring(5, 2)
        Response.Redirect("Presenze.aspx?IdEntita=" & dgVolontari.SelectedItem.Cells(0).Text & "&anno=" & stranno & "&mese=" & strmese & "&AnnoMese=" & CboAnnoMese.SelectedValue & "&VengoDa=ConfermaPreliminareMultipla")
        'ADC andrà gestito sulla pagina pesenze.aspx il chiudi che riporta qui
    End Sub

    Protected Sub CmdEsporta_Click(sender As Object, e As EventArgs) Handles CmdEsporta.Click

        CmdEsporta.Visible = False
        CreaStampaCSV(Session("appDtsRisRicerca"))
        Dim dtbRicerca As DataTable = Session("DtbRicerca")
        StampaCSV(dtbRicerca)
    End Sub

    Private Sub CreaStampaCSV(ByVal MyDataSet As DataSet)
        Dim NomeColonne(8) As String
        Dim NomiCampiColonne(8) As String
        'nome della colonna 
        'e posizione nella griglia di lettura
        NomeColonne(0) = "Codice Volontario"
        NomeColonne(1) = "Nominativo"
        NomeColonne(2) = "Cod. Fiscale"
        NomeColonne(3) = "Data Nascita"
        NomeColonne(4) = "Comune Nascita"
        NomeColonne(5) = "Progetto"
        NomeColonne(6) = "Ente"
        NomeColonne(7) = "Codice Sede"
        NomeColonne(8) = "Stato Conferma Preliminare"

        NomiCampiColonne(0) = "CodiceVolontario"
        NomiCampiColonne(1) = "Nominativo"
        NomiCampiColonne(2) = "CodiceFiscale"
        NomiCampiColonne(3) = "DataNascita"
        NomiCampiColonne(4) = "ComuneNascita"
        NomiCampiColonne(5) = "Progetto"
        NomiCampiColonne(6) = "Ente"
        NomiCampiColonne(7) = "IdEnteSedeAttuazione"
        NomiCampiColonne(8) = "ConfermaPreliminare"
        'carico un datatable che userò poi nella pagina di stampa
        'il numero delle colonne è a base 0
        Session("DtbRicerca") = ClsServer.CaricaDataTablePerStampa(MyDataSet, 8, NomeColonne, NomiCampiColonne)
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

        Dim dtrVolPreliminari As Data.SqlClient.SqlDataReader
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

    Protected Sub cmdChiudi_Click(sender As Object, e As EventArgs) Handles cmdChiudi.Click
        Response.Redirect("WfrmMain.aspx")
    End Sub

    Protected Sub CmdConfermaPreliminare_Click(sender As Object, e As EventArgs) Handles CmdConfermaPreliminare.Click
        Dim strMg As String
        Dim esito As String

        Dim intNSelezionati As Integer = 0
        Dim intNAggiornati As Integer = 0

        Try
            lblmessaggio.Text = ""
            For Each riga As DataGridItem In dgVolontari.Items
                If CType(riga.FindControl("chkSelProg"), CheckBox).Checked Then
                    strIDselezionati &= "," & riga.Cells(0).Text
                    intNSelezionati = intNSelezionati + 1

                    ClsServer.ConfermaPreliminare(riga.Cells(0).Text, Left(CboAnnoMese.SelectedValue, 4), Right(CboAnnoMese.SelectedValue, 2), Session("Utente"), -1, Session("conn"), strMg, esito)
                    If esito = "POSITIVO" Then
                        intNAggiornati = intNAggiornati + 1
                    End If
                End If
            Next

            lblmessaggio.Visible = True
            If strIDselezionati = "" Then
                lblmessaggio.Text = "Selezionare almeno un volontario dalla lista"
            Else
                LoadRiceraVolontari()
                If intNAggiornati = intNSelezionati Then
                    lblmessaggio.Text = "Conferma Preliminare effettuata."
                Else
                    lblmessaggio.Text = "Sono state effettuate " & intNAggiornati & " conferme su " & intNSelezionati & ". E' possibile effetture la conferma preliminare solo se il calendario è completo. Procedere accedendo ai singoli calendari per verificare le eventuali eccezioni."
                End If

            End If




        Catch ex As Exception

        End Try


    End Sub
End Class