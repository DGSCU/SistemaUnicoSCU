Imports System.IO
Public Class WfrmCheckListRicerca
    Inherits System.Web.UI.Page
    '***
    ' CREATA DA SIMONA CORDELLA IL 01/02/2016
    ' NUOVA MASCHERA DI RICERCA CHE UNISCE LA RICERCA INDIVIDUALE E COLLETTIVA
    '(IL RISULTATO VIEDE DIVISO IN DUE GRIGLIE DIVERSE)
    '***
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Page.IsPostBack = False Then
            CaricaCombo()
            If Request.QueryString("menu") Is Nothing Then
                If Not Request.Cookies("SUSCNRicChkPag") Is Nothing Then
                    CaricaParemetri()
                End If
            End If

        End If
    End Sub
    Sub CaricaCombo()
        Dim strSql As String
        Try

            Dim MyDataset As DataSet
            ''CboCodProgRend.Items.Clear()
            'strSql = "SELECT '' as CodiceProgettoRendicontazione UNION SELECT distinct a.CodiceProgettoRendicontazione as CodiceProgettoRendicontazione FROM CheckListPagheCollettivo a INNER JOIN StatiCheckList b on a.idstatochecklist = b.idstatochecklist WHERE(ANNOCOMPETENZA < Year(GETDATE()) Or (ANNOCOMPETENZA = Year(GETDATE()) And MESECOMPETENZA < Month(GETDATE()))) order by 1"
            'MyDataset = ClsServer.DataSetGenerico(strSql, Session("conn"))

            Cbocompetenze.Items.Clear()
            strSql = "SELECT '' as Competenze UNION SELECT Distinct convert(varchar,a.AnnoCompetenza) + '/' +  replicate('0',2-len(convert(varchar,a.MeseCompetenza))) + convert(varchar,a.MeseCompetenza) As Competenze FROM CheckListPagheCollettivo a INNER JOIN StatiCheckList b on a.idstatochecklist = b.idstatochecklist WHERE (ANNOCOMPETENZA<YEAR(GETDATE()) OR  (ANNOCOMPETENZA=YEAR(GETDATE()) AND MESECOMPETENZA<MONTH(GETDATE()))) order by 1"
            MyDataset = ClsServer.DataSetGenerico(strSql, Session("conn"))
            Cbocompetenze.DataSource = MyDataset
            'Cbocompetenze.DataValueField = "IdStatoAssicurativo"
            Cbocompetenze.DataTextField = "Competenze"
            Cbocompetenze.DataBind()

            CboEsecuzionePagamento.Items.Clear()
            strSql = "SELECT '' as EsecuzionePagamento UNION SELECT distinct convert(varchar,a.AnnoPagamento) + '/' +  replicate('0',2-len(convert(varchar,a.MesePagamento))) + convert(varchar,a.MesePagamento) As EsecuzionePagamento FROM CheckListPagheCollettivo a INNER JOIN StatiCheckList b on a.idstatochecklist = b.idstatochecklist WHERE (ANNOCOMPETENZA<YEAR(GETDATE()) OR  (ANNOCOMPETENZA=YEAR(GETDATE()) AND MESECOMPETENZA<MONTH(GETDATE()))) order by 1"
            MyDataset = ClsServer.DataSetGenerico(strSql, Session("conn"))
            CboEsecuzionePagamento.DataSource = MyDataset
            CboEsecuzionePagamento.DataTextField = "EsecuzionePagamento"
            'CboEsecuzionePagamento.DataValueField = "IdCategoriaEntità"
            CboEsecuzionePagamento.DataBind()
            'modificato il 28/02/2017 non estraggo lo stato archiviata (valido solo per le check list della formazione)
            CboStatoChecklist.Items.Clear()
            strSql = "SELECT '' as StatoCheckList,'0' as IdStatoCheckList  UNION  Select StatoCheckList as StatoCheckList,IdStatoCheckList from StatiCheckList where IdStatoCheckList<>4 order by IdStatoCheckList"
            MyDataset = ClsServer.DataSetGenerico(strSql, Session("conn"))
            CboStatoChecklist.DataSource = MyDataset
            CboStatoChecklist.DataTextField = "StatoCheckList"
            'CboStatoChecklist.DataValueField = "IdCategoriaEntità"
            CboStatoChecklist.DataBind()
        Catch ex As Exception

        End Try
    End Sub
    'richiama store SP_CHECKLIST_PRESENZE_RICERCA_COLLETTIVE
    Private Sub RicercaElencoLista()
        dgRisultatoRicerca.DataSource = Nothing
        dgRisultatoRicerca.DataBind()

        Dim sqlDAP As New SqlClient.SqlDataAdapter
        Dim dataSet As New DataSet
        Dim strNomeStore As String = "[SP_CHECKLIST_PRESENZE_RICERCA_COLLETTIVE]"
        dgRisultatoRicerca.CurrentPageIndex = 0

        Try
            sqlDAP = New SqlClient.SqlDataAdapter(strNomeStore, CType(Session("conn"), SqlClient.SqlConnection))
            sqlDAP.SelectCommand.CommandType = CommandType.StoredProcedure
            sqlDAP.SelectCommand.Parameters.Add("@CodiceProgettoRendicontazione", SqlDbType.VarChar).Value = txtCodProgRendi.Text
            sqlDAP.SelectCommand.Parameters.Add("@MeseCompetenza", SqlDbType.VarChar).Value = Cbocompetenze.SelectedValue
            sqlDAP.SelectCommand.Parameters.Add("@MesePagamento", SqlDbType.VarChar).Value = CboEsecuzionePagamento.SelectedValue
            sqlDAP.SelectCommand.Parameters.Add("@StatoCheckList", SqlDbType.VarChar).Value = CboStatoChecklist.SelectedValue
            sqlDAP.SelectCommand.Parameters.Add("@CodiceEnte", SqlDbType.VarChar).Value = txtCodiceEnte.Text
            sqlDAP.SelectCommand.Parameters.Add("@NomeEnte", SqlDbType.VarChar).Value = txtNomeEnte.Text.Replace("'", "''")
            sqlDAP.SelectCommand.Parameters.Add("@TitoloProgetto", SqlDbType.VarChar).Value = txtTitoloProg.Text.Replace("'", "''")
            sqlDAP.SelectCommand.Parameters.Add("@codicechecklist", SqlDbType.VarChar).Value = txtcodicechecklist.Text
            sqlDAP.SelectCommand.Parameters.Add("@nome", SqlDbType.VarChar).Value = txtnome.Text.Replace("'", "''")
            sqlDAP.SelectCommand.Parameters.Add("@cognome", SqlDbType.VarChar).Value = txtcognome.Text.Replace("'", "''")
            sqlDAP.SelectCommand.Parameters.Add("@codicefiscale", SqlDbType.VarChar).Value = txtcodicefiscale.Text.Replace("'", "''")
            sqlDAP.SelectCommand.Parameters.Add("@codicevolontario", SqlDbType.VarChar).Value = txtcodicevolontario.Text.Replace("'", "''")

            sqlDAP.Fill(dataSet)
            Session("appDtsRisRicerca") = dataSet
            dgRisultatoRicerca.DataSource = dataSet
            dgRisultatoRicerca.DataBind()

            ViewState("datasource") = dataSet

            'nome e posizione di lettura delle colopnne a base 0
            Dim NomeColonne(6) As String
            Dim NomiCampiColonne(6) As String
            'nome della colonna 
            'e posizione nella griglia di lettura
            NomeColonne(0) = "Cod Lista"
            NomeColonne(1) = "Cod Prog"
            NomeColonne(2) = "Titolo"
            NomeColonne(3) = "Ente"
            NomeColonne(4) = "Competenza"
            NomeColonne(5) = "Esecuzione Pagamento"
            NomeColonne(6) = "Stato"


            NomiCampiColonne(0) = "codicechecklist"
            NomiCampiColonne(1) = "CodiceProgettoRendicontazione"
            NomiCampiColonne(2) = "Titolo"
            NomiCampiColonne(3) = "Ente"
            NomiCampiColonne(4) = "Competenze"
            NomiCampiColonne(5) = "EsecuzionePagamento"
            NomiCampiColonne(6) = "StatoCheckList"


            'carico un datatable che userò poi nella pagina di stampa
            'il numero delle colonne è a base 0
            CaricaDataTablePerStampaCollettiva(dataSet, 6, NomeColonne, NomiCampiColonne)

        Catch ex As Exception
            Response.Write(ex.Message.ToString())
            Exit Sub
        End Try
    End Sub
    ' richiama store SP_CHECKLIST_PRESENZE_RICERCA_INDIVIDUALE
    Private Sub RicercaElencoListaIndividuale()

        dgRisultatoRicercaIndiduale.DataSource = Nothing
        dgRisultatoRicercaIndiduale.DataBind()
        Dim sqlDAP As New SqlClient.SqlDataAdapter
        Dim dataSetIndi As New DataSet
        Dim strNomeStore As String = "[SP_CHECKLIST_PRESENZE_RICERCA_INDIVIDUALE]"
        dgRisultatoRicercaIndiduale.CurrentPageIndex = 0

        Try
            sqlDAP = New SqlClient.SqlDataAdapter(strNomeStore, CType(Session("conn"), SqlClient.SqlConnection))
            sqlDAP.SelectCommand.CommandType = CommandType.StoredProcedure
            sqlDAP.SelectCommand.Parameters.Add("@CodiceEnte", SqlDbType.VarChar).Value = txtCodiceEnte.Text.Replace("'", "''")
            sqlDAP.SelectCommand.Parameters.Add("@NomeEnte", SqlDbType.VarChar).Value = txtNomeEnte.Text.Replace("'", "''")
            sqlDAP.SelectCommand.Parameters.Add("@CodiceProgettoRendicontazione", SqlDbType.VarChar).Value = txtCodProgRendi.Text.Replace("'", "''")
            sqlDAP.SelectCommand.Parameters.Add("@TitoloProgetto", SqlDbType.VarChar).Value = txtTitoloProg.Text.Replace("'", "''")
            sqlDAP.SelectCommand.Parameters.Add("@MeseCompetenza", SqlDbType.VarChar).Value = Cbocompetenze.SelectedValue.Replace("'", "''")
            sqlDAP.SelectCommand.Parameters.Add("@MesePagamento", SqlDbType.VarChar).Value = CboEsecuzionePagamento.SelectedValue.Replace("'", "''")
            sqlDAP.SelectCommand.Parameters.Add("@StatoCheckList", SqlDbType.VarChar).Value = CboStatoChecklist.SelectedValue.Replace("'", "''")
            sqlDAP.SelectCommand.Parameters.Add("@nome", SqlDbType.VarChar).Value = txtnome.Text.Replace("'", "''")
            sqlDAP.SelectCommand.Parameters.Add("@cognome", SqlDbType.VarChar).Value = txtcognome.Text.Replace("'", "''")
            sqlDAP.SelectCommand.Parameters.Add("@codicefiscale", SqlDbType.VarChar).Value = txtcodicefiscale.Text.Replace("'", "''")
            sqlDAP.SelectCommand.Parameters.Add("@codicevolontario", SqlDbType.VarChar).Value = txtcodicevolontario.Text.Replace("'", "''")
            sqlDAP.SelectCommand.Parameters.Add("@codicechecklist", SqlDbType.VarChar).Value = txtcodicechecklist.Text

            sqlDAP.Fill(dataSetIndi)

            Session("appDtsRisRicercaIndi") = dataSetIndi
            ViewState("datasource") = dataSetIndi

            dgRisultatoRicercaIndiduale.DataSource = dataSetIndi
            dgRisultatoRicercaIndiduale.DataBind()
            'nome e posizione di lettura delle colopnne a base 0
            Dim NomeColonne(8) As String
            Dim NomiCampiColonne(8) As String
            'nome della colonna 
            'e posizione nella griglia di lettura
            NomeColonne(0) = "Cod Lista"
            NomeColonne(1) = "Cod Vol"
            NomeColonne(2) = "Cognome"
            NomeColonne(3) = "Nome"
            NomeColonne(4) = "Codicefiscale"
            NomeColonne(5) = "Cod Prog"
            NomeColonne(6) = "Competenza"
            NomeColonne(7) = "Esecuzione Pagamento"
            NomeColonne(8) = "Stato"


            NomiCampiColonne(0) = "codicechecklist"
            NomiCampiColonne(1) = "CodiceVolontario"
            NomiCampiColonne(2) = "Cognome"
            NomiCampiColonne(3) = "Nome"
            NomiCampiColonne(4) = "CodiceFiscale"
            NomiCampiColonne(5) = "CodiceProgettoRendicontazione"
            NomiCampiColonne(6) = "Competenze"
            NomiCampiColonne(7) = "EsecuzionePagamento"
            NomiCampiColonne(8) = "StatoCheckList"

            'carico un datatable che userò poi nella pagina di stampa
            'il numero delle colonne è a base 0
            CaricaDataTablePerStampaIndividuale(dataSetIndi, 8, NomeColonne, NomiCampiColonne)

        Catch ex As Exception
            Response.Write(ex.Message.ToString())
            Exit Sub
        End Try
    End Sub

    Protected Sub CmdEsporta_Click(sender As Object, e As EventArgs) Handles CmdEsporta.Click
        CmdEsporta.Visible = False
        Select Case ddlTipoCheList.SelectedValue
            Case 0 'TUTTE
                Dim dtbRicerca As DataTable = Session("DtbRicerca")
                StampaCSVCollettive(dtbRicerca)
                Dim dtbRicercaIndi As DataTable = Session("dtbRicercaIndi")
                StampaCSVIndividuale(dtbRicercaIndi)
            Case 1 'COLLETTIVE
                Dim dtbRicerca As DataTable = Session("DtbRicerca")
                StampaCSVCollettive(dtbRicerca)
            Case 2 'INDIVIDUALI
                Dim dtbRicercaIndi As DataTable = Session("dtbRicercaIndi")
                StampaCSVIndividuale(dtbRicercaIndi)
        End Select

    End Sub
    'ABILITA LINK STAMPA CSV ELENCO CHECKLIST COLLETTIVE
    Private Sub StampaCSVCollettive(ByVal dtbRicerca As DataTable)
        Dim path As String
        Dim xPrefissoNome As String
        Dim url As String
        Dim utility As ClsUtility = New ClsUtility()

        If dtbRicerca.Rows.Count = 0 Then
            ApriCSV1.Visible = False
            ApriCSV1Indi.Visible = False
            CmdEsporta.Visible = False
        Else
            xPrefissoNome = Session("Utente")
            path = Server.MapPath("download")
            url = CreaFileCSV(dtbRicerca, xPrefissoNome, path, "ExpDatiCollettive")
            ApriCSV1.Visible = True
            ApriCSV1.NavigateUrl = url
        End If

    End Sub
    'ABILITA LINK STAMPA CSV ELENCO CHECKLIST INDIVIDUALI
    Private Sub StampaCSVIndividuale(ByVal dtbRicerca As DataTable)
        Dim path As String
        Dim xPrefissoNome As String
        Dim url As String
        Dim utility As ClsUtility = New ClsUtility()

        If dtbRicerca.Rows.Count = 0 Then
            ApriCSV1Indi.Visible = False
            CmdEsporta.Visible = False
        Else
            xPrefissoNome = Session("Utente")
            path = Server.MapPath("download")
            url = CreaFileCSV(dtbRicerca, xPrefissoNome, path, "ExpDatiIndividuli")
            ApriCSV1Indi.Visible = True
            ApriCSV1Indi.NavigateUrl = url
        End If
    End Sub
    'CREA FILE CSV
    Function CreaFileCSV(ByVal DTBRicerca As DataTable, ByVal xPrefissoNome As String, ByVal mapPath As String, ByVal Nomefile As String) As String

        Dim dtrSediAttuazione As Data.SqlClient.SqlDataReader
        Dim Writer As StreamWriter
        Dim xLinea As String
        Dim i As Int64
        Dim j As Int64
        Dim NomeUnivoco As String
        Dim Reader As StreamReader
        Dim url As String
        NomeUnivoco = xPrefissoNome & Nomefile & Year(Now) & Month(Now) & Day(Now) & Hour(Now) & Minute(Now) & Second(Now)
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
    'CARICA IL DATATABLE PER LA STMAPA ELENCO CHECKLIST COLLETTIVE
    Sub CaricaDataTablePerStampaCollettiva(ByVal DataSetDaScorrere As DataSet, ByVal NColonne As Integer, ByVal NomiColonne() As String, ByVal NomiCampiColonne() As String)
        Dim dt As New DataTable
        Dim dr As DataRow
        Dim i As Integer
        Dim x As Integer

        'carico i nomi delle colonne che andrò a stampare nella datagrid
        For x = 0 To NColonne
            dt.Columns.Add(New DataColumn(NomiColonne(x), GetType(String)))
        Next

        'carico il datatable con il risultato della query della ricerca, in qusto caso delle risorse
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
    'CARICA IL DATATABLE PER LA STMAPA ELENCO CHECKLIST INDIVIDUALI
    Sub CaricaDataTablePerStampaIndividuale(ByVal DataSetDaScorrere As DataSet, ByVal NColonne As Integer, ByVal NomiColonne() As String, ByVal NomiCampiColonne() As String)
        Dim dt As New DataTable
        Dim dr As DataRow
        Dim i As Integer
        Dim x As Integer

        'carico i nomi delle colonne che andrò a stampare nella datagrid
        For x = 0 To NColonne
            dt.Columns.Add(New DataColumn(NomiColonne(x), GetType(String)))
        Next

        'carico il datatable con il risultato della query della ricerca, in qusto caso delle risorse
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
        Session("DtbRicercaIndi") = dt
    End Sub

    'SUB richiama dal pulsante RICERCA
    Sub Ricerca()
        CmdEsporta.Visible = True

        dgRisultatoRicerca.DataSource = Nothing
        dgRisultatoRicerca.DataBind()
        dgRisultatoRicercaIndiduale.DataSource = Nothing
        dgRisultatoRicercaIndiduale.DataBind()
        Select Case ddlTipoCheList.SelectedValue
            Case 0 'TUTTI
                RicercaElencoLista()
                RicercaElencoListaIndividuale()
            Case 1 'COLLETTIVI
                RicercaElencoLista()
            Case 2 'INDIVIDUALI
                RicercaElencoListaIndividuale()
        End Select
        If dgRisultatoRicerca.Items.Count = 0 And dgRisultatoRicercaIndiduale.Items.Count = 0 Then
            CmdEsporta.Visible = False
        End If
        ApriCSV1.Visible = False
        ApriCSV1Indi.Visible = False
        RicordaRicercaCookies()
    End Sub
    Protected Sub cmdRicerca_Click(sender As Object, e As EventArgs) Handles cmdRicerca.Click
        Ricerca()
    End Sub

    Private Sub dgRisultatoRicerca_ItemCommand(source As Object, e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgRisultatoRicerca.ItemCommand
        'Selezionato
        If e.CommandName = "Selezionato" Then
            If (Session("TipoUtente") = "U") Then
                RicordaRicercaCookies()
                Response.Redirect("~/WfrmCheckListDettaglio.aspx?menu=1&Codchecklist=" & e.Item.Cells(2).Text & "&anno=" & e.Item.Cells(9).Text & "&mese=" & e.Item.Cells(10).Text & "&idLista=" & e.Item.Cells(0).Text)
            Else
                Response.Redirect("page_error.aspx")
            End If
        End If
    End Sub

    Private Sub dgRisultatoRicerca_PageIndexChanged(source As Object, e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgRisultatoRicerca.PageIndexChanged
        Try
            dgRisultatoRicerca.CurrentPageIndex = e.NewPageIndex
            dgRisultatoRicerca.DataSource = Session("appDtsRisRicerca")
            dgRisultatoRicerca.DataBind()
            dgRisultatoRicerca.SelectedIndex = -1
        Catch ex As Exception
            dgRisultatoRicerca.CurrentPageIndex = e.NewPageIndex - 1
            dgRisultatoRicerca.DataSource = Session("appDtsRisRicerca")
            dgRisultatoRicerca.DataBind()
            dgRisultatoRicerca.SelectedIndex = -1
        End Try

    End Sub

    Private Sub dgRisultatoRicercaIndiduale_ItemCommand(source As Object, e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgRisultatoRicercaIndiduale.ItemCommand
        If e.CommandName = "Selezionato" Then
            If (Session("TipoUtente") = "U") Then
                RicordaRicercaCookies()
                Response.Redirect("~/WfrmCheckListDettaglioIndividuale.aspx?Codchecklist=" & e.Item.Cells(3).Text & "&anno=" & e.Item.Cells(11).Text & "&mese=" & e.Item.Cells(12).Text & "&idLista=" & e.Item.Cells(0).Text)
            Else
                Response.Redirect("page_error.aspx")
            End If
        End If
    End Sub

    Private Sub dgRisultatoRicercaIndiduale_PageIndexChanged(source As Object, e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgRisultatoRicercaIndiduale.PageIndexChanged
        Try
            dgRisultatoRicercaIndiduale.CurrentPageIndex = e.NewPageIndex
            dgRisultatoRicercaIndiduale.DataSource = Session("appDtsRisRicercaIndi")
            dgRisultatoRicercaIndiduale.DataBind()
            dgRisultatoRicercaIndiduale.SelectedIndex = -1
        Catch ex As Exception
            dgRisultatoRicercaIndiduale.CurrentPageIndex = e.NewPageIndex - 1
            dgRisultatoRicercaIndiduale.DataSource = Session("appDtsRisRicercaIndi")
            dgRisultatoRicercaIndiduale.DataBind()
            dgRisultatoRicercaIndiduale.SelectedIndex = -1
        End Try

    End Sub
    Sub RicordaRicercaCookies()
        'CREATA IL 02/02/2016
        'Sub che ricorda nei cookies i parametri di ricerca inseriti in maschera; ricorca anche il numero di pagina selezionato in griglia
        Response.Cookies("SUSCNRicChkPag")("txtCodiceEnte") = txtCodiceEnte.Text
        Response.Cookies("SUSCNRicChkPag")("txtNomeEnte") = txtNomeEnte.Text
        Response.Cookies("SUSCNRicChkPag")("txtCodProgRendi") = txtCodProgRendi.Text
        Response.Cookies("SUSCNRicChkPag")("txtTitoloProg") = txtTitoloProg.Text
        Response.Cookies("SUSCNRicChkPag")("txtcodicefiscale") = txtcodicefiscale.Text
        Response.Cookies("SUSCNRicChkPag")("txtcodicevolontario") = txtcodicevolontario.Text
        Response.Cookies("SUSCNRicChkPag")("txtcognome") = txtcognome.Text
        Response.Cookies("SUSCNRicChkPag")("txtnome") = txtnome.Text
        Response.Cookies("SUSCNRicChkPag")("Cbocompetenze") = Cbocompetenze.SelectedValue
        Response.Cookies("SUSCNRicChkPag")("CboEsecuzionePagamento") = CboEsecuzionePagamento.SelectedValue
        Response.Cookies("SUSCNRicChkPag")("CboStatoChecklist") = CboStatoChecklist.Text
        Response.Cookies("SUSCNRicChkPag")("txtcodicechecklist") = txtcodicechecklist.Text
        Response.Cookies("SUSCNRicChkPag")("ddlTipoCheList") = ddlTipoCheList.SelectedValue
        Response.Cookies("SUSCNRicChkPag")("PageCollettive") = dgRisultatoRicerca.CurrentPageIndex
        Response.Cookies("SUSCNRicChkPag")("PageIndividuali") = dgRisultatoRicercaIndiduale.CurrentPageIndex
        Response.Cookies("SUSCNRicChkPag").Expires = DateTime.Now.AddDays(1)
    End Sub
    Sub CaricaParemetri()
        'CREATA IL 02/02/2016
        'Sub che leggi i parametri di ricerca memorizzati nei cookies

        If Not Server.HtmlDecode(Request.Cookies("SUSCNRicChkPag")("txtCodiceEnte")) Is Nothing Then
            txtCodiceEnte.Text = Server.HtmlDecode(Request.Cookies("SUSCNRicChkPag")("txtCodiceEnte")).ToString
        End If
        If Not Server.HtmlDecode(Request.Cookies("SUSCNRicChkPag")("txtNomeEnte")) Is Nothing Then
            txtNomeEnte.Text = Server.HtmlDecode(Request.Cookies("SUSCNRicChkPag")("txtNomeEnte")).ToString
        End If
        If Not Server.HtmlDecode(Request.Cookies("SUSCNRicChkPag")("txtCodProgRendi")) Is Nothing Then
            txtCodProgRendi.Text = Server.HtmlDecode(Request.Cookies("SUSCNRicChkPag")("txtCodProgRendi")).ToString
        End If
        If Not Server.HtmlDecode(Request.Cookies("SUSCNRicChkPag")("txtTitoloProg")) Is Nothing Then
            txtTitoloProg.Text = Server.HtmlDecode(Request.Cookies("SUSCNRicChkPag")("txtTitoloProg")).ToString
        End If
        If Not Server.HtmlDecode(Request.Cookies("SUSCNRicChkPag")("txtcodicefiscale")) Is Nothing Then
            txtcodicefiscale.Text = Server.HtmlDecode(Request.Cookies("SUSCNRicChkPag")("txtcodicefiscale")).ToString
        End If
        If Not Server.HtmlDecode(Request.Cookies("SUSCNRicChkPag")("txtcodicevolontario")) Is Nothing Then
            txtcodicevolontario.Text = Server.HtmlDecode(Request.Cookies("SUSCNRicChkPag")("txtcodicevolontario")).ToString
        End If
        If Not Server.HtmlDecode(Request.Cookies("SUSCNRicChkPag")("txtcognome")) Is Nothing Then
            txtcognome.Text = Server.HtmlDecode(Request.Cookies("SUSCNRicChkPag")("txtcognome")).ToString
        End If
        If Not Server.HtmlDecode(Request.Cookies("SUSCNRicChkPag")("txtnome")) Is Nothing Then
            txtnome.Text = Server.HtmlDecode(Request.Cookies("SUSCNRicChkPag")("txtnome")).ToString
        End If
        If Not Server.HtmlDecode(Request.Cookies("SUSCNRicChkPag")("Cbocompetenze")) Is Nothing Then
            Cbocompetenze.SelectedValue = Server.HtmlDecode(Request.Cookies("SUSCNRicChkPag")("Cbocompetenze")).ToString
        End If
        If Not Server.HtmlDecode(Request.Cookies("SUSCNRicChkPag")("CboEsecuzionePagamento")) Is Nothing Then
            CboEsecuzionePagamento.SelectedValue = Server.HtmlDecode(Request.Cookies("SUSCNRicChkPag")("CboEsecuzionePagamento")).ToString
        End If
        If Not Server.HtmlDecode(Request.Cookies("SUSCNRicChkPag")("CboStatoChecklist")) Is Nothing Then
            CboStatoChecklist.Text = Server.HtmlDecode(Request.Cookies("SUSCNRicChkPag")("CboStatoChecklist")).ToString
        End If
        If Not Server.HtmlDecode(Request.Cookies("SUSCNRicChkPag")("txtcodicechecklist")) Is Nothing Then
            txtcodicechecklist.Text = Server.HtmlDecode(Request.Cookies("SUSCNRicChkPag")("txtcodicechecklist")).ToString
        End If
        If Not Server.HtmlDecode(Request.Cookies("SUSCNRicChkPag")("ddlTipoCheList")) Is Nothing Then
            ddlTipoCheList.SelectedValue = Server.HtmlDecode(Request.Cookies("SUSCNRicChkPag")("ddlTipoCheList")).ToString
        End If
        'richiamo la su per la ricerca
        Ricerca()
        'riporte le griglie al numero di pagina memorizzato nel cookies coorrispondente
        Select Case ddlTipoCheList.SelectedValue
            Case 0 'TUTTE
                If Not Server.HtmlDecode(Request.Cookies("SUSCNRicChkPag")("PageCollettive")) Is Nothing Then
                    Try
                        dgRisultatoRicerca.CurrentPageIndex = Server.HtmlDecode(Request.Cookies("SUSCNRicChkPag")("PageCollettive")).ToString
                        dgRisultatoRicerca.DataSource = Session("appDtsRisRicerca")
                        dgRisultatoRicerca.DataBind()
                        dgRisultatoRicerca.SelectedIndex = -1
                    Catch
                        dgRisultatoRicerca.CurrentPageIndex = Server.HtmlDecode(Request.Cookies("SUSCNRicChkPag")("PageCollettive")).ToString - 1
                        dgRisultatoRicerca.DataSource = Session("appDtsRisRicerca")
                        dgRisultatoRicerca.DataBind()
                        dgRisultatoRicerca.SelectedIndex = -1
                    End Try
                End If
                If Not Server.HtmlDecode(Request.Cookies("SUSCNRicChkPag")("PageIndividuali")) Is Nothing Then
                    Try
                        dgRisultatoRicercaIndiduale.CurrentPageIndex = Server.HtmlDecode(Request.Cookies("SUSCNRicChkPag")("PageIndividuali")).ToString
                        dgRisultatoRicercaIndiduale.DataSource = Session("appDtsRisRicercaIndi")
                        dgRisultatoRicercaIndiduale.DataBind()
                        dgRisultatoRicercaIndiduale.SelectedIndex = -1
                    Catch
                        dgRisultatoRicercaIndiduale.CurrentPageIndex = Server.HtmlDecode(Request.Cookies("SUSCNRicChkPag")("PageIndividuali")).ToString - 1
                        dgRisultatoRicercaIndiduale.DataSource = Session("appDtsRisRicercaIndi")
                        dgRisultatoRicercaIndiduale.DataBind()
                        dgRisultatoRicercaIndiduale.SelectedIndex = -1
                    End Try
                End If
            Case 1 'COLLETTIVA
                If Not Server.HtmlDecode(Request.Cookies("SUSCNRicChkPag")("PageCollettive")) Is Nothing Then
                    Try
                        dgRisultatoRicerca.CurrentPageIndex = Server.HtmlDecode(Request.Cookies("SUSCNRicChkPag")("PageCollettive")).ToString
                        dgRisultatoRicerca.DataSource = Session("appDtsRisRicerca")
                        dgRisultatoRicerca.DataBind()
                        dgRisultatoRicerca.SelectedIndex = -1
                    Catch
                        dgRisultatoRicerca.CurrentPageIndex = Server.HtmlDecode(Request.Cookies("SUSCNRicChkPag")("PageCollettive")).ToString - 1
                        dgRisultatoRicerca.DataSource = Session("appDtsRisRicerca")
                        dgRisultatoRicerca.DataBind()
                        dgRisultatoRicerca.SelectedIndex = -1
                    End Try
                End If
            Case 2 'INDIVIDUALE
                If Not Server.HtmlDecode(Request.Cookies("SUSCNRicChkPag")("PageIndividuali")) Is Nothing Then
                    Try
                        dgRisultatoRicercaIndiduale.CurrentPageIndex = Server.HtmlDecode(Request.Cookies("SUSCNRicChkPag")("PageIndividuali")).ToString
                        dgRisultatoRicercaIndiduale.DataSource = Session("appDtsRisRicercaIndi")
                        dgRisultatoRicercaIndiduale.DataBind()
                        dgRisultatoRicercaIndiduale.SelectedIndex = -1
                    Catch
                        dgRisultatoRicercaIndiduale.CurrentPageIndex = Server.HtmlDecode(Request.Cookies("SUSCNRicChkPag")("PageIndividuali")).ToString - 1
                        dgRisultatoRicercaIndiduale.DataSource = Session("appDtsRisRicercaIndi")
                        dgRisultatoRicercaIndiduale.DataBind()
                        dgRisultatoRicercaIndiduale.SelectedIndex = -1
                    End Try
                End If
        End Select
    End Sub

    Protected Sub cmdChiudi_Click(sender As Object, e As EventArgs) Handles cmdChiudi.Click
        Response.Redirect("WfrmRicercaEnti.aspx")
    End Sub

End Class