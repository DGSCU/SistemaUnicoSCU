Imports System.IO
Public Class ver_RicercaProgrammazioneApprovate
    Inherits System.Web.UI.Page

#Region " Codice generato da Progettazione Web Form "

    'Chiamata richiesta da Progettazione Web Form.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents lblProgr As System.Web.UI.WebControls.Label
    Protected WithEvents ddlProgr As System.Web.UI.WebControls.DropDownList
    Protected WithEvents Label13 As System.Web.UI.WebControls.Label
    Protected WithEvents TxtComune As System.Web.UI.WebControls.TextBox
    Protected WithEvents Label14 As System.Web.UI.WebControls.Label
    Protected WithEvents TxtProvincia As System.Web.UI.WebControls.TextBox
    Protected WithEvents CmdRicerca As System.Web.UI.WebControls.Button
    Protected WithEvents cmdChiudi As System.Web.UI.WebControls.Button
    Protected WithEvents imgStampa As System.Web.UI.WebControls.Button
    Protected WithEvents lblNumFascicolo As System.Web.UI.WebControls.Label
    Protected WithEvents TxtNumFascicolo As System.Web.UI.WebControls.TextBox
    Protected WithEvents LblDataProtPresentazione As System.Web.UI.WebControls.Label
    Protected WithEvents TxtDataProtPresentazione As System.Web.UI.WebControls.TextBox
    Protected WithEvents LblDataProtApprovazione As System.Web.UI.WebControls.Label
    Protected WithEvents TxtDataProtApprovazione As System.Web.UI.WebControls.TextBox
    Protected WithEvents lblMessaggio As System.Web.UI.WebControls.Label
    Protected WithEvents dgRisultatoRicerca As System.Web.UI.WebControls.DataGrid
    Protected WithEvents Image5 As System.Web.UI.WebControls.Image
    Protected WithEvents Image1 As System.Web.UI.WebControls.Image

    'NOTA: la seguente dichiarazione è richiesta da Progettazione Web Form.
    'Non spostarla o rimuoverla.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: questa chiamata al metodo è richiesta da Progettazione Web Form.
        'Non modificarla nell'editor del codice.
        InitializeComponent()
    End Sub

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Inserire qui il codice utente necessario per inizializzare la pagina

        If Not Session("LogIn") Is Nothing Then
            If Session("LogIn") = False Then
                Response.Redirect("LogOn.aspx")
            End If
        Else
            Response.Redirect("LogOn.aspx")
        End If

        If IsPostBack = False Then
            TrovaCompetenzaUtente()
            CaricaCompetenze()
            CaricaProgrammazione()
        End If
    End Sub

    Private Sub CmdRicerca_Click(sender As Object, e As EventArgs) Handles CmdRicerca.Click
        EseguiRicerca()
    End Sub

    Private Sub EseguiRicerca()
        Dim dtElenco As New DataSet
        Dim strSql As String
        Dim strNumFascicolo As String

        dgRisultatoRicerca.CurrentPageIndex = 0


        strSql = " SELECT p.IDProgrammazione, p.CodiceFascicolo, p.Descrizione,"
        strSql &= " dbo.FormatoData(p.DataProtPresentazione) as DataProtPresentazione, dbo.FormatoData(p.DataProtApprovazione) as DataProtApprovazione,"
        strSql &= " p.IDFascicolo , p.DescrFascicolo, "
        strSql &= " (SELECT COUNT(tva.IDVerifica) AS idVer from tverificheassociate tva inner join tverifiche tv on tva.idverifica = tv.idverifica "
        strSql &= "  WHERE (tv.IDProgrammazione = p.IDProgrammazione AND tv.IDStatoVerifica >= 5)) AS totVer,"
        strSql &= " r.Descrizione as Competenza "
        strSql &= " FROM TVerificheProgrammazione  p"
        strSql &= " INNER JOIN bando b ON b.IDBando=p.IdBando"
        strSql &= " INNER join AssociaBandoTipiProgetto abtp on abtp.idbando =  b.idbando"
        strSql &= " INNER JOIN TipiProgetto  tp on abtp.idtipoprogetto = tp.idtipoprogetto"
        strSql &= " INNER JOIN RegioniCompetenze r ON r.IdRegioneCompetenza= p.IdRegCompetenza"
        strSql &= " WHERE p.IDStatoProgrammazione = 4 "
        strSql &= " AND tp.MacroTipoProgetto like '" & Session("FiltroVisibilita") & "' "


        If ddlProgr.SelectedValue <> "0" Then
            If UCase(Trim(ddlProgr.SelectedItem.Text)) <> "" Then
                strSql &= " and p.idprogrammazione = " & ddlProgr.SelectedValue & ""
            End If
        End If
        If TxtNumFascicolo.Text.Trim <> "" Then
            strSql &= " and p.CodiceFascicolo like '" & Replace(TxtNumFascicolo.Text, "'", "''") & "%'"
        End If
        If TxtDataProtPresentazione.Text.Trim <> "" Then
            strSql &= " and p.DataProtPresentazione = '" & TxtDataProtPresentazione.Text & "'"
        End If
        If TxtDataProtApprovazione.Text.Trim <> "" Then
            strSql &= " and p.DataProtApprovazione = '" & TxtDataProtApprovazione.Text & "'"
        End If
        If Session("IdRegCompetenza") <> 22 Then
            strSql &= " AND p.IdRegCompetenza= " & Session("IdRegCompetenza") & ""
        End If

        If ddlCompetenza.SelectedValue <> "" Then
            Select Case ddlCompetenza.SelectedValue
                Case 0
                    strSql = strSql & ""
                Case -1
                    strSql &= " AND p.IdRegCompetenza = 22 "
                Case -2
                    strSql &= " AND p.IdRegCompetenza <> 22 And not p.IdRegCompetenza is null "
                Case -3
                    strSql &= " AND  p.IdRegCompetenza is null "
                Case Else
                    strSql &= " AND  p.IdRegCompetenza = '" & ddlCompetenza.SelectedValue & "'"
            End Select
        End If

        strSql &= " GROUP BY p.IDProgrammazione, p.CodiceFascicolo, p.Descrizione, p.DataProtPresentazione, p.DataProtApprovazione, p.IDFascicolo, p.DescrFascicolo,r.Descrizione "
        strSql &= " Order by p.IDProgrammazione DESC "

        Session("dtElenco") = ClsServer.DataSetGenerico(strSql, Session("conn"))
        CaricaDataGrid(dgRisultatoRicerca)

    End Sub
    Private Sub CaricaDataGrid(ByRef GriddaCaricare As DataGrid)
        'assegno il dataset alla griglia del risultato
        GriddaCaricare.DataSource = Session("dtElenco")
        GriddaCaricare.DataBind()
        'blocco per la creazione della datatable per la stampa della ricerca

        'nome e posizione di lettura delle colopnne a base 0
        Dim NomeColonne(4) As String
        Dim NomiCampiColonne(4) As String
        'nome della colonna 
        'e posizione nella griglia di lettura
        NomeColonne(0) = "Numero Fascicolo"
        NomeColonne(1) = "Descr. Programmazione"
        NomeColonne(2) = "Data Protocollo Presentazione"
        NomeColonne(3) = "Data Protocollo Approvazione"
        NomeColonne(4) = "Num. Verifiche"


        NomiCampiColonne(0) = "CodiceFascicolo"
        NomiCampiColonne(1) = "Descrizione"
        NomiCampiColonne(2) = "DataProtPresentazione"
        NomiCampiColonne(3) = "DataProtApprovazione"
        NomiCampiColonne(4) = "totVer"

        'carico un datatable che userò poi nella pagina di stampa
        'il numero delle colonne è a base 0
        Session("DtbRicerca") = ClsServer.CaricaDataTablePerStampa(Session("dtElenco"), 4, NomeColonne, NomiCampiColonne)

        ''*********************************************************************************
        If GriddaCaricare.Items.Count = 0 Then
            imgStampa.Visible = False
            lblMessaggio.Visible = True
            lblMessaggio.Text = "Nessuna informazione presente in archivio."
            GriddaCaricare.Visible = False
        Else
            imgStampa.Visible = True
            lblMessaggio.Visible = False
            GriddaCaricare.Visible = True
        End If
    End Sub

    Private Sub cmdChiudi_Click(sender As Object, e As EventArgs) Handles cmdChiudi.Click
        Response.Redirect("WfrmMain.aspx")
    End Sub

    Private Sub dgRisultatoRicerca_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgRisultatoRicerca.PageIndexChanged
        dgRisultatoRicerca.SelectedIndex = -1
        dgRisultatoRicerca.EditItemIndex = -1
        dgRisultatoRicerca.CurrentPageIndex = e.NewPageIndex
        CaricaDataGrid(dgRisultatoRicerca)
    End Sub

    Private Sub dgRisultatoRicerca_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgRisultatoRicerca.ItemCommand
        Select Case e.CommandName
            Case "Select"
                Response.Redirect("ver_ProgrammazioniApprovate.aspx?IdProgrammazione=" & e.Item.Cells(0).Text)
        End Select
    End Sub

    Private Sub CaricaProgrammazione()
        Dim strsql As String
        'strsql = " Select idprogrammazione , descrizione  from tverificheprogrammazione " & _
        '         " where IDStatoProgrammazione =4"
        'If Session("IdRegCompetenza") <> 22 Then
        '    strsql = strsql & " and IdRegCompetenza =" & Session("IdRegCompetenza") & " "
        'End If
        strsql = " SELECT DISTINCT p.idprogrammazione , p.descrizione  "
        strsql &= " from tverificheprogrammazione P"
        strsql &= " INNER JOIN bando b ON b.IDBando=p.IdBando"
        strsql &= " INNER join AssociaBandoTipiProgetto abtp on abtp.idbando =  b.idbando"
        strsql &= " INNER JOIN TipiProgetto  tp on abtp.idtipoprogetto = tp.idtipoprogetto"
        strsql &= " WHERE p.IDStatoProgrammazione =4 "
        If Session("IdRegCompetenza") <> 22 Then
            strsql &= " and p.IdRegCompetenza =" & Session("IdRegCompetenza") & " "
        End If
        strsql &= " AND tp.MacroTipoProgetto like '" & Session("FiltroVisibilita") & "' "
        strsql &= " Order by p.idprogrammazione desc"

        ddlProgr.DataSource = ClsServer.CreaDataTable(strsql, True, Session("conn"))
        ddlProgr.DataValueField = "idprogrammazione"
        ddlProgr.DataTextField = "descrizione"
        ddlProgr.DataBind()
    End Sub
    Private Sub TrovaCompetenzaUtente()
        'stringa per la query
        Dim strSQL As String

        'datareader che conterrà l'id 
        Dim dtrCompetenze As System.Data.SqlClient.SqlDataReader

        strSQL = "select b.IdRegioneCompetenza ,b.Heliosread from RegioniCompetenze a "
        strSQL = strSQL & "INNER JOIN utentiunsc b ON a.idregionecompetenza = b.idregionecompetenza "
        strSQL = strSQL & "where b.username = '" & Session("Utente") & "'"
        'chiudo il datareader se aperto
        If Not dtrCompetenze Is Nothing Then
            dtrCompetenze.Close()
            dtrCompetenze = Nothing
        End If
        'controllo se utente o ente regionale
        'eseguo la query
        dtrCompetenze = ClsServer.CreaDatareader(strSQL, Session("conn"))
        dtrCompetenze.Read()
        If dtrCompetenze.HasRows = True Then
            Session("IdRegCompetenza") = dtrCompetenze("IdRegioneCompetenza")
            'If dtrCompetenze("Heliosread") = True Then
            '    ddlCompetenza.Enabled = True
            'End If
        End If
        If Not dtrCompetenze Is Nothing Then
            dtrCompetenze.Close()
            dtrCompetenze = Nothing
        End If
    End Sub



    Private Sub StampaCSV(ByVal dtbRicerca As DataTable)
        Dim path As String
        Dim xPrefissoNome As String
        Dim url As String
        Dim utility As ClsUtility = New ClsUtility()

        If dtbRicerca.Rows.Count = 0 Then
            ApriCSV1.Visible = False
            imgStampa.Visible = False
        Else
            xPrefissoNome = Session("Utente")
            path = Server.MapPath("download")
            url = CreaFileCSV(dtbRicerca, xPrefissoNome, path)
            ApriCSV1.Visible = True
            ApriCSV1.NavigateUrl = url
        End If
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


    Protected Sub imgStampa_Click(sender As Object, e As EventArgs) Handles imgStampa.Click
        imgStampa.Visible = False
        Dim dtbRicerca As DataTable = Session("DtbRicerca")
        StampaCSV(dtbRicerca)
    End Sub

    Sub CaricaCompetenze()
        'stringa per la query
        Dim strSQL As String

        'datareader che conterrà l'id 
        Dim dtrCompetenze As System.Data.SqlClient.SqlDataReader

        Try
            'controllo se si tratta del primo caricamento. così leggo i dati nel db una sola volta
            If Page.IsPostBack = False Then
                'preparo la query
                strSQL = "select IdRegioneCompetenza,Descrizione,CodiceRegioneCompetenza,left(CodiceRegioneCompetenza,1)from RegioniCompetenze where IdRegioneCompetenza <> 22 "
                strSQL = strSQL & " union "
                strSQL = strSQL & " select '0',' TUTTI ','','A' "
                strSQL = strSQL & " union "
                strSQL = strSQL & " select '-1',' NAZIONALE ','','B' "
                strSQL = strSQL & " union "
                strSQL = strSQL & " select '-2',' REGIONALE ','','C' "

                strSQL = strSQL & " order by left(CodiceRegioneCompetenza,1),descrizione "


                'chiudo il datareader se aperto
                If Not dtrCompetenze Is Nothing Then
                    dtrCompetenze.Close()
                    dtrCompetenze = Nothing
                End If

                'eseguo la query
                dtrCompetenze = ClsServer.CreaDatareader(strSQL, Session("conn"))
                'assegno il datadearder alla combo caricando così descrizione e id
                ddlCompetenza.DataSource = dtrCompetenze
                ddlCompetenza.Items.Add("")
                ddlCompetenza.DataTextField = "Descrizione"
                ddlCompetenza.DataValueField = "IDRegioneCompetenza"
                ddlCompetenza.DataBind()

                If Not dtrCompetenze Is Nothing Then
                    dtrCompetenze.Close()
                    dtrCompetenze = Nothing
                End If

                'chiudo il datareader se aperto
            End If
            'Controllo abilitazione scelta
            If Session("TipoUtente") = "U" Then
                ddlCompetenza.Enabled = True
                ddlCompetenza.SelectedIndex = 0
            Else
                'CboCompetenza.SelectedIndex = 1
                ddlCompetenza.Enabled = False
                'preparo la query
                strSQL = "select b.IdRegioneCompetenza ,b.Heliosread from RegioniCompetenze a "
                strSQL = strSQL & "INNER JOIN utentiunsc b ON a.idregionecompetenza = b.idregionecompetenza "
                strSQL = strSQL & "where b.username = '" & Session("Utente") & "'"
                'chiudo il datareader se aperto
                If Not dtrCompetenze Is Nothing Then
                    dtrCompetenze.Close()
                    dtrCompetenze = Nothing
                End If
                'controllo se utente o ente regionale
                'eseguo la query
                dtrCompetenze = ClsServer.CreaDatareader(strSQL, Session("conn"))
                dtrCompetenze.Read()
                If dtrCompetenze.HasRows = True Then
                    ddlCompetenza.SelectedValue = dtrCompetenze("IdRegioneCompetenza")
                    If dtrCompetenze("Heliosread") = True Then
                        ddlCompetenza.Enabled = True
                    End If
                    'Session("IdRegCompetenza") = ddlCompetenza.SelectedValue
                End If

                If Session("TipoUtente") = "R" Then
                    ddlCompetenza.Enabled = False
                End If

            End If

        Catch ex As Exception
            Response.Write(ex.Message.ToString())
            If Not dtrCompetenze Is Nothing Then
                dtrCompetenze.Close()
                dtrCompetenze = Nothing
            End If
        End Try
        If Not dtrCompetenze Is Nothing Then
            dtrCompetenze.Close()
            dtrCompetenze = Nothing
        End If
    End Sub
End Class
