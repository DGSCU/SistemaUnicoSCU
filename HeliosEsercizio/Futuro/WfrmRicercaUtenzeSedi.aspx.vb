Imports System.IO
Public Class WfrmRicercaUtenzeSedi
    Inherits System.Web.UI.Page

    Dim UtenzeSPID As Boolean

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'Inserire qui il codice utente necessario per inizializzare la pagina
        If Not Session("LogIn") Is Nothing Then
            If Session("LogIn") = False Then 'verifico validità log-in
                Response.Redirect("LogOn.aspx")
            End If
        Else
            Response.Redirect("LogOn.aspx")
        End If

        UtenzeSPID = UtenzeSedeSPID()

        If IsPostBack = False Then
            If VerificaAbilitazione(Session("Utente"), Session("conn")) = False Then
                Response.Redirect("wfrmAnomaliaDati.aspx")
            End If

            If UtenzeSPID Then
                divNominativo.Visible = True
            Else
                divNominativo.Visible = False
            End If
        End If

    End Sub

    Protected Sub cmdRicerca_Click(sender As Object, e As EventArgs) Handles cmdRicerca.Click
        dgSedi.CurrentPageIndex = 0
        LoadRiceraSede()
        If dgSedi.Items.Count = 0 Then
            lblmessaggio.Text = "Nessun Dato estratto."
            CmdEsporta.Visible = False
        Else
            lblmessaggio.Text = "Risultato Ricerca Sedi."
            dgSedi.Visible = True
            CmdEsporta.Visible = True
        End If
    End Sub

    Private Sub LoadRiceraSede()
        'REALIZZATA DA: SIMONA CORDELLA 
        'DATA REALIZZAZIONE:  05/03/2015
        'FUNZIONALITA': RICHIAMO STORE PER LA RICERCA DELLE SEDI 
        Dim sqlDAP As New SqlClient.SqlDataAdapter
        Dim dataSet As New DataSet
        Dim strNomeStore As String = "[SP_UTENZA_SEDE_RICERCA]"


        Try
            sqlDAP = New SqlClient.SqlDataAdapter(strNomeStore, CType(Session("conn"), SqlClient.SqlConnection))
            sqlDAP.SelectCommand.CommandType = CommandType.StoredProcedure

            sqlDAP.SelectCommand.Parameters.Add("@IDEnte", SqlDbType.VarChar).Value = Session("IdEnte")
            sqlDAP.SelectCommand.Parameters.Add("@DenominazioneSede", SqlDbType.VarChar).Value = TxtDenSede.Text.Replace("'", "''")
            sqlDAP.SelectCommand.Parameters.Add("@IDEnteSedeAttuazione", SqlDbType.VarChar).Value = txtIdEnteSedeAttuazione.Text
            sqlDAP.SelectCommand.Parameters.Add("@Comune", SqlDbType.VarChar).Value = txtComune.Text.Replace("'", "''")
            sqlDAP.SelectCommand.Parameters.Add("@Provincia", SqlDbType.VarChar).Value = TxtProvincia.Text.Replace("'", "''")
            sqlDAP.SelectCommand.Parameters.Add("@Regione", SqlDbType.VarChar).Value = TxtRegione.Text.Replace("'", "''")
            sqlDAP.SelectCommand.Parameters.Add("@Stato", SqlDbType.VarChar).Value = ddlStatoUtenza.SelectedValue

            If UtenzeSPID Then
                sqlDAP.SelectCommand.Parameters.Add("@Cognome", SqlDbType.VarChar).Value = txtCognome.Text.Replace("'", "''")
                sqlDAP.SelectCommand.Parameters.Add("@Nome", SqlDbType.VarChar).Value = txtNome.Text.Replace("'", "''")
            End If


            sqlDAP.Fill(dataSet)

            Session("appDtsRisRicerca") = dataSet
            dgSedi.DataSource = dataSet
            dgSedi.DataBind()

        Catch ex As Exception
            Response.Write(ex.Message.ToString())
            Exit Sub
        End Try
    End Sub

    Private Sub dgSedi_PageIndexChanged(source As Object, e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgSedi.PageIndexChanged
        dgSedi.CurrentPageIndex = e.NewPageIndex
        dgSedi.DataSource = Session("appDtsRisRicerca")
        dgSedi.DataBind()
        dgSedi.SelectedIndex = -1
    End Sub

    Protected Sub dgSedi_SelectedIndexChanged(sender As Object, e As EventArgs) Handles dgSedi.SelectedIndexChanged
        Response.Redirect("WfrmGestioneSottoUtenza.aspx?IdEnteSedeAttuazione=" & dgSedi.SelectedItem.Cells(1).Text)
    End Sub

    Private Sub CmdEsporta_Click(sender As Object, e As System.EventArgs) Handles CmdEsporta.Click
        CmdEsporta.Visible = False
        CreaStampaCSV(Session("appDtsRisRicerca"))
        Dim dtbRicerca As DataTable = Session("DtbRicerca")
        StampaCSV(dtbRicerca)
    End Sub
    Private Sub CreaStampaCSV(ByVal MyDataSet As DataSet)
        Dim NomeColonne(4) As String
        Dim NomiCampiColonne(4) As String
        'nome della colonna 
        'e posizione nella griglia di lettura
        NomeColonne(0) = "Codice Sede"
        NomeColonne(1) = "Sede"
        NomeColonne(2) = "Comune"
        NomeColonne(3) = "Regione"
        NomeColonne(4) = "Stato Utenza"

        NomiCampiColonne(0) = "IdEnteSedeAttuazione"
        NomiCampiColonne(1) = "Denominazionesede"
        NomiCampiColonne(2) = "Comune"
        NomiCampiColonne(3) = "Regione"
        NomiCampiColonne(4) = "StatoUtenza"
        'carico un datatable che userò poi nella pagina di stampa
        'il numero delle colonne è a base 0
        Session("DtbRicerca") = ClsServer.CaricaDataTablePerStampa(MyDataSet, 4, NomeColonne, NomiCampiColonne)
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

    Protected Sub cmdChiudi_Click(sender As Object, e As EventArgs) Handles cmdChiudi.Click
        Response.Redirect("WfrmMain.aspx")
    End Sub

    Private Function VerificaAbilitazione(ByVal Utente As String, ByVal Conn As SqlClient.SqlConnection) As Boolean

        '** Verifico se l'utenza è abilitata alla visibilità del flag che consente il caricamento della programamzione con volontari e progetti terminati
        '** profilio menu creato appositamente per le richieste regionali
        Dim strSql As String
        Dim dtrgenerico As SqlClient.SqlDataReader
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If

        'Verifica menu sicurezza su funzione accredita
        strSql = "SELECT AssociaUtenteGruppo.username, VociMenu.IdVoceMenu, VociMenu.VoceMenu, VociMenu.ImmaginePassiva, VociMenu.ImmagineAttiva, VociMenu.Link," & _
                 " VociMenu.IdVoceMenuPadre" & _
                 " FROM VociMenu " & _
                 " INNER JOIN AbilitazioniProfiliVociMenu ON VociMenu.IdVoceMenu = AbilitazioniProfiliVociMenu.IdVoceMenu" & _
                 " INNER JOIN Profili ON AbilitazioniProfiliVociMenu.IdProfilo = Profili.IdProfilo" & _
                 " INNER JOIN AssociaUtenteGruppo ON Profili.IdProfilo = AssociaUtenteGruppo.IdProfilo" & _
                 " LEFT JOIN  RestrizioniUtenteVociMenu ON AssociaUtenteGruppo.UserName = RestrizioniUtenteVociMenu.UserName and RestrizioniUtenteVociMenu.IdVoceMenu=VociMenu.IdVoceMenu" & _
                 " WHERE VociMenu.descrizione = 'Gestione Utenze Sedi'" & _
                 " AND AssociaUtenteGruppo.username ='" & Utente & "' and (RestrizioniUtenteVociMenu.TipoRestrizione <> 0 or RestrizioniUtenteVociMenu.TipoRestrizione is null)"
        dtrgenerico = ClsServer.CreaDatareader(strSql, Conn)

        VerificaAbilitazione = dtrgenerico.Read()
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
        Return VerificaAbilitazione
    End Function

    Private Function UtenzeSedeSPID() As Boolean
        Dim _ret As Boolean = False
        Dim strsql As String
        Dim dtrGen As SqlClient.SqlDataReader
        strsql = "SELECT VALORE  FROM Configurazioni where Parametro='UTENZE_SEDE_SPID'"
        If Not dtrGen Is Nothing Then
            dtrGen.Close()
            dtrGen = Nothing
        End If
        dtrGen = ClsServer.CreaDatareader(strsql, Session("conn"))
        dtrGen.Read()
        If dtrGen.HasRows Then
            If dtrGen("Valore") = "1" Then _ret = True
        End If
        If Not dtrGen Is Nothing Then
            dtrGen.Close()
            dtrGen = Nothing
        End If
        Return _ret
    End Function
End Class