Imports System.IO
Public Class WfrmCheckListElencoFormazione
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Page.IsPostBack = False Then
            CaricaCombo()
            CaricaComboCausaleArchiviazione()
        End If
    End Sub
    Protected Sub cmdRicerca_Click(sender As Object, e As EventArgs) Handles cmdRicerca.Click
        CmdEsporta.Visible = True
        RicercaElencoLista()
        ApriCSV1.Visible = False
    End Sub
    Protected Sub cmdChiudi_Click(sender As Object, e As EventArgs) Handles cmdChiudi.Click
        Response.Redirect("WfrmRicercaEnti.aspx")
    End Sub
    Sub CaricaCombo()
        Dim strSql As String
        Try

            Dim MyDataset As DataSet

            CboStatoChecklist.Items.Clear()
            strSql = "SELECT '' as StatoCheckList,'0' as IdStatoCheckList  UNION  Select StatoCheckList as StatoCheckList,IdStatoCheckList from StatiCheckList order by IdStatoCheckList"
            MyDataset = ClsServer.DataSetGenerico(strSql, Session("conn"))
            CboStatoChecklist.DataSource = MyDataset
            CboStatoChecklist.DataTextField = "StatoCheckList"
            'CboStatoChecklist.DataValueField = "IdCategoriaEntità"
            CboStatoChecklist.DataBind()
        Catch ex As Exception

        End Try
    End Sub

    Sub CaricaComboCausaleArchiviazione()
        Dim strSql As String
        Try

            Dim MyDataset As DataSet

            CboCausaleArchiviazione.Items.Clear()
            strSql = "SELECT '' as CAUSALE ,'0' as IdCausaleArchiviazione  UNION  Select Causale as CAUSALE,IdCausaleArchiviazione from CheckListCausaliArchiviazione WHERE ABILITATO=1 AND Tipochecklist='F' order by IdCausaleArchiviazione"
            MyDataset = ClsServer.DataSetGenerico(strSql, Session("conn"))
            CboCausaleArchiviazione.DataSource = MyDataset
            CboCausaleArchiviazione.DataTextField = "CAUSALE"
            CboCausaleArchiviazione.DataValueField = "IdCausaleArchiviazione"
            CboCausaleArchiviazione.DataBind()
        Catch ex As Exception

        End Try
    End Sub
    Private Sub RicercaElencoLista()

        Dim sqlDAP As New SqlClient.SqlDataAdapter
        Dim dataSet As New DataSet
        Dim strNomeStore As String = "[SP_CHECKLIST_RICERCA_FORMAZIONE]"
        dgRisultatoRicerca.CurrentPageIndex = 0

        Try
            sqlDAP = New SqlClient.SqlDataAdapter(strNomeStore, CType(Session("conn"), SqlClient.SqlConnection))
            sqlDAP.SelectCommand.CommandType = CommandType.StoredProcedure

            sqlDAP.SelectCommand.Parameters.Add("@CodiceProgettoRendicontazione", SqlDbType.VarChar).Value = txtCodProgRendi.Text
            sqlDAP.SelectCommand.Parameters.Add("@StatoCheckList", SqlDbType.VarChar).Value = CboStatoChecklist.SelectedValue
            sqlDAP.SelectCommand.Parameters.Add("@CodiceEnte", SqlDbType.VarChar).Value = txtCodiceEnte.Text
            sqlDAP.SelectCommand.Parameters.Add("@NomeEnte", SqlDbType.VarChar).Value = txtNomeEnte.Text
            sqlDAP.SelectCommand.Parameters.Add("@TitoloProgetto", SqlDbType.VarChar).Value = txtTitoloProg.Text
            sqlDAP.SelectCommand.Parameters.Add("@codicechecklist", SqlDbType.VarChar).Value = txtcodicechecklist.Text
            sqlDAP.SelectCommand.Parameters.Add("@IdCausaleArchiviazione", SqlDbType.VarChar).Value = cboCausaleArchiviazione.SelectedValue

            sqlDAP.Fill(dataSet)

            Session("appDtsRisRicerca") = dataSet
            ViewState("datasource") = dataSet

            dgRisultatoRicerca.DataSource = dataSet
            dgRisultatoRicerca.DataBind()


            'nome e posizione di lettura delle colopnne a base 0
            Dim NomeColonne(6) As String
            Dim NomiCampiColonne(6) As String
            'nome della colonna 
            'e posizione nella griglia di lettura
            NomeColonne(0) = "Cod Lista"
            NomeColonne(1) = "Cod Prog"
            NomeColonne(2) = "Titolo"
            NomeColonne(3) = "Ente"
            NomeColonne(4) = "Importo"
            NomeColonne(5) = "Stato Formazione"
            NomeColonne(6) = "Stato"


            NomiCampiColonne(0) = "codicechecklist"
            NomiCampiColonne(1) = "CodiceProgettoRendicontazione"
            NomiCampiColonne(2) = "Titolo"
            NomiCampiColonne(3) = "Ente"
            NomiCampiColonne(4) = "Importo"
            NomiCampiColonne(5) = "StatoFormazione"
            NomiCampiColonne(6) = "StatoCheckList"


            'carico un datatable che userò poi nella pagina di stampa
            'il numero delle colonne è a base 0
            CaricaDataTablePerStampa(dataSet, 6, NomeColonne, NomiCampiColonne)
        Catch ex As Exception
            Response.Write(ex.Message.ToString())
            Exit Sub
        End Try
    End Sub

    Private Sub dgRisultatoRicerca_ItemCommand(source As Object, e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgRisultatoRicerca.ItemCommand
        'Selezionato
        If e.CommandName = "Selezionato" Then
            'Session("IdEnte") = e.Item.Cells(21).Text
            'Session("Denominazione") = e.Item.Cells(22).Text
            If (Session("TipoUtente") = "U") Then
                Response.Redirect("~/WfrmCheckListDettaglioFormazione.aspx?menu=1&Codchecklist=" & e.Item.Cells(3).Text & "&idLista=" & e.Item.Cells(0).Text & "&IdAttivita=" & e.Item.Cells(1).Text)
            Else
                Response.Redirect("page_error.aspx")
            End If
        End If
    End Sub

    Private Sub dgRisultatoRicerca_PageIndexChanged(source As Object, e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgRisultatoRicerca.PageIndexChanged

        dgRisultatoRicerca.CurrentPageIndex = e.NewPageIndex
        dgRisultatoRicerca.DataSource = Session("appDtsRisRicerca")
        dgRisultatoRicerca.DataBind()
        dgRisultatoRicerca.SelectedIndex = -1
    End Sub
    Protected Sub CmdEsporta_Click(sender As Object, e As EventArgs) Handles CmdEsporta.Click
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
        NomeUnivoco = xPrefissoNome & "ExpDatiFormazione" & Year(Now) & Month(Now) & Day(Now) & Hour(Now) & Minute(Now) & Second(Now)
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
    Sub CaricaDataTablePerStampa(ByVal DataSetDaScorrere As DataSet, ByVal NColonne As Integer, ByVal NomiColonne() As String, ByVal NomiCampiColonne() As String)
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

    Protected Sub dgRisultatoRicerca_SelectedIndexChanged(sender As Object, e As EventArgs) Handles dgRisultatoRicerca.SelectedIndexChanged

    End Sub
End Class