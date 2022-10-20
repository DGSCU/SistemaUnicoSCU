Imports System.IO

Public Class WfrmRicercaVolontariSospesi
    Inherits System.Web.UI.Page

#Region " Codice generato da Progettazione Web Form "

    'Chiamata richiesta da Progettazione Web Form.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    Protected WithEvents Form1 As System.Web.UI.HtmlControls.HtmlForm

    'NOTA: la seguente dichiarazione è richiesta da Progettazione Web Form.
    'Non spostarla o rimuoverla.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        InitializeComponent()
    End Sub

#End Region

    Dim dtrGenerico As SqlClient.SqlDataReader
    Dim strquery As String
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Inserire qui il codice utente necessario per inizializzare la pagina
        If Not Session("LogIn") Is Nothing Then
            If Session("LogIn") = False Then 'verifico validità log-in
                Response.Redirect("LogOn.aspx")
            End If
        Else
            Response.Redirect("LogOn.aspx")
        End If
        If Request.QueryString("Op") = "rinuncia" Then
            lblTitolo.Text = "Ricerca Volontari Rinunciatari"
        Else
            lblTitolo.Text = "Ricerca Volontari Esclusi"
        End If
        If dgVolontari Is Nothing Then
            lblmessaggioRicerca.Text = ""
        End If
    End Sub

    Private Sub cmdChiudi_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles cmdChiudi.Click
        Response.Redirect("WfrmMain.aspx")
    End Sub

    Sub CaricaGriglia()
        Dim strSql As String
        Dim strWhere As String
        Dim MyDataSet As DataSet
        'DESCRIZIONE: routine che carica la griglia con tutti i progetti con stato attività=1
        'AUTORE: Michele d'Ascenzio    
        'DATA: 03/11/2004
        'Modificato da Simona Cordella il 15/01/2007
        ' aggiunto filtro di di ricerca per codice volontario
        strSql = "SELECT Entità.IDEntità as IdEntità, " & _
                 "Attività.IDAttività as IdAttività, " & _
                 "AttivitàEntità.IdAttivitàEntità as IdAttivitàEntità, " & _
                 "Enti.IdEnte as IdEnte, " & _
                 "Entità.CodiceVolontario, " & _
                 "Entità.Cognome + ' ' + Entità.Nome as Nominativo, " & _
                 "Entità.CodiceFiscale, " & _
                 "CONVERT(varchar, Entità.DataNascita , 103) as DataNascita, " & _
                 "Comuni.Denominazione as ComuneNascita, " & _
                 "Attività.Titolo as Progetto, " & _
                 "EntiSediAttuazioni.Denominazione as SedeAttuazione, " & _
                 "Enti.Denominazione as Ente, " & _
                 "CASE (SELECT COUNT(IDCronologiaSostituzioni) FROM CronologiaSostituzioni WHERE IdEntitàSostituita = Entità.IDEntità) WHEN 0 THEN 'No' ELSE 'Si' END Sostituito, " & _
                 " ep.denominazione as EntePresentante,Attività.identePresentante" & _
                 " FROM Entità " & _
                 "INNER JOIN Comuni ON Entità.IdComuneNascita = Comuni.IdComune " & _
                 "INNER JOIN AttivitàEntità ON Entità.IdEntità = AttivitàEntità.IdEntità " & _
                 "INNER JOIN StatiAttivitàEntità ON StatiAttivitàEntità.IdStatoAttivitàEntità = AttivitàEntità.IdStatoAttivitàEntità AND StatiAttivitàEntità.DefaultStato = 1 " & _
                 "INNER JOIN AttivitàEntiSediAttuazione ON AttivitàEntità.IDAttivitàEnteSedeAttuazione = AttivitàEntiSediAttuazione.IDAttivitàEnteSedeAttuazione " & _
                 "INNER JOIN EntiSediAttuazioni ON AttivitàEntiSediAttuazione.IDEnteSedeAttuazione = EntiSediAttuazioni.IDEnteSedeAttuazione " & _
                 "INNER JOIN EntiSedi ON EntiSediAttuazioni.IdEnteSede = EntiSedi.IdEnteSede " & _
                 "INNER JOIN Enti ON EntiSedi.IdEnte = Enti.IdEnte " & _
                 "INNER JOIN Attività ON AttivitàEntiSediAttuazione.IdAttività = Attività.IdAttività " & _
                 " inner join enti EP on EP.idente=Attività.identepresentante " & _
                 "INNER JOIN StatiEntità ON Entità.IdStatoEntità = Statientità.IdStatoEntità " & _
                 "INNER JOIN TipiProgetto ON Attività.IdTipoProgetto = TipiProgetto.IdTipoProgetto "
        Select Case ddlFiltro.SelectedValue
            Case Is = "2"
                strWhere = "WHERE EXISTS (SELECT IDCronologiaSostituzioni FROM CronologiaSostituzioni WHERE IdEntitàSostituita = Entità.IDEntità)"
            Case Is = "1"
                strWhere = "WHERE NOT EXISTS (SELECT IDCronologiaSostituzioni FROM CronologiaSostituzioni WHERE IdEntitàSostituita = Entità.IDEntità)"
        End Select

        If strWhere = vbNullString Then
            If Request.QueryString("Op") = "rinuncia" Then
                strWhere = "WHERE StatiEntità.Rinuncia = 1 "
            Else
                strWhere = "WHERE StatiEntità.Sospeso = 1 "
            End If
        Else
            If Request.QueryString("Op") = "rinuncia" Then
                strWhere = strWhere & " AND StatiEntità.Rinuncia = 1 "
            Else
                strWhere = strWhere & " AND StatiEntità.Sospeso = 1 "
            End If
        End If

        If Session("TipoUtente") = "E" Then
            strWhere = strWhere & "AND attività.IdEntepresentante = " & Session("IdEnte") & " "
        End If
        If txtEnte.Text <> vbNullString Then
            strWhere = strWhere & "AND ep.Denominazione like '" & Replace(txtEnte.Text, ",", "''") & "%' "
        End If
        If txtCodEnte.Text <> vbNullString Then
            strWhere = strWhere & "AND ep.CodiceRegione like '" & Replace(txtCodEnte.Text, "'", "''") & "%' "
        End If
        If TxtCodVolontario.Text <> vbNullString Then
            strWhere = strWhere & "AND Entità.CodiceVolontario = '" & Replace(TxtCodVolontario.Text, "'", "''") & "' "
        End If
        If txtCognome.Text <> vbNullString Then
            strWhere = strWhere & "AND Entità.Cognome like '" & Replace(txtCognome.Text, "'", "''") & "%' "
        End If
        If txtNome.Text <> vbNullString Then
            strWhere = strWhere & "AND Entità.Nome like '" & Replace(txtNome.Text, "'", "''") & "%' "
        End If
        If txtProgetto.Text <> vbNullString Then
            strWhere = strWhere & "AND Attività.Titolo like '" & Replace(txtProgetto.Text, "'", "''") & "%' "
        End If
        If txtCodProgetto.Text <> vbNullString Then
            strWhere = strWhere & "AND Attività.CodiceEnte like '" & Replace(txtCodProgetto.Text, "'", "''") & "%'"
        End If
        'FiltroVisibilita
        strWhere = strWhere & " and TipiProgetto.MacroTipoProgetto like '" & Session("FiltroVisibilita") & "' "
        strSql = strSql & strWhere

        MyDataSet = ClsServer.DataSetGenerico(strSql, Session("conn"))

        'assegno il dataset alla griglia del risultato
        dgVolontari.DataSource = MyDataSet
        Session("appDtsRisRicerca") = MyDataSet
        If (Session("TipoUtente") = "U" Or Session("TipoUtente") = "R") Then
            dgVolontari.Columns(11).Visible = True 'denominazione ente
        Else
            dgVolontari.Columns(11).Visible = False
        End If
        dgVolontari.DataBind()
        'Aggiunto da Alessandra Taballione il 29.03.2005
        '*********************************************************************************
        'blocco per la creazione della datatable per la stampa della ricerca
        'nome e posizione di lettura delle colopnne a base 0
        Dim NomeColonne(8) As String
        Dim NomiCampiColonne(8) As String
        NomeColonne(0) = "Codice Volontario"
        NomeColonne(1) = "Nominativo"
        NomeColonne(2) = "Cod.Fiscale"
        NomeColonne(3) = "Data Nascita"
        NomeColonne(4) = "Comune Nascita"
        NomeColonne(5) = "Progetto"
        NomeColonne(6) = "Sede Attuazione"
        NomeColonne(7) = "Ente"
        NomeColonne(8) = "Sostituito"

        NomiCampiColonne(0) = "CodiceVolontario"
        NomiCampiColonne(1) = "Nominativo"
        NomiCampiColonne(2) = "CodiceFiscale"
        NomiCampiColonne(3) = "DataNascita"
        NomiCampiColonne(4) = "ComuneNascita"
        NomiCampiColonne(5) = "Progetto"
        NomiCampiColonne(6) = "SedeAttuazione"
        NomiCampiColonne(7) = "Ente"
        NomiCampiColonne(8) = "Sostituito"
        'carico un datatable che userò poi nella pagina di stampa
        'il numero delle colonne è a base 0
        Session("DtbRicerca") = ClsServer.CaricaDataTablePerStampa(MyDataSet, 8, NomeColonne, NomiCampiColonne)
        If dgVolontari.Items.Count = 0 Then
            CmdEsporta.Visible = False
            lblmessaggioRicerca.Text = "La ricerca non ha prodotto risultati."
        Else
            CmdEsporta.Visible = True
            lblmessaggioRicerca.Text = "Risultato " & lblTitolo.Text
        End If

        '***********************************************************************
        dgVolontari.Visible = True



    End Sub

    Private Sub dgVolontari_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgVolontari.PageIndexChanged

        dgVolontari.CurrentPageIndex = e.NewPageIndex
        dgVolontari.DataSource = Session("appDtsRisRicerca")
        dgVolontari.DataBind()
        dgVolontari.SelectedIndex = -1
    End Sub

    Private Sub cmdRicerca_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles cmdRicerca.Click
        ApriCSV1.Visible = False
        dgVolontari.CurrentPageIndex = 0
        CaricaGriglia()
    End Sub
    Private Sub dgVolontari_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dgVolontari.SelectedIndexChanged
        Try


            If (Session("TipoUtente") = "U" Or Session("TipoUtente") = "R") Then
                Session("IdEnte") = dgVolontari.SelectedItem.Cells(14).Text 'identepresentante
                Session("Denominazione") = dgVolontari.SelectedItem.Cells(11).Text 'denominazione ente
            End If
            Response.Redirect("WfrmGestioneSostituisciVolontari.aspx?IdAttivita=" & dgVolontari.SelectedItem.Cells(1).Text & "&IdEntita=" & dgVolontari.SelectedItem.Cells(0).Text & "&VecchioIdAttivitaEntita=" & dgVolontari.SelectedItem.Cells(13).Text & "&Op=" & Request.QueryString("Op") & "&CodiceFiscale=" & dgVolontari.SelectedItem.Cells(6).Text)
        Catch ex As Exception
            Debug.Print(ex.Message)

        End Try
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

