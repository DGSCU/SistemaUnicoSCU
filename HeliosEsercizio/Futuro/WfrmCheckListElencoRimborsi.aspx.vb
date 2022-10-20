Imports System.IO
Public Class WfrmCheckListElencoRimborsi
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Page.IsPostBack = False Then
            CaricaCombo()
            'RicercaElencoLista()
        End If
    End Sub
    Protected Sub cmdChiudi_Click(sender As Object, e As EventArgs) Handles cmdChiudi.Click
        Response.Redirect("WfrmRicercaEnti.aspx")
    End Sub

    Protected Sub cmdRicerca_Click(sender As Object, e As EventArgs) Handles cmdRicerca.Click
        CmdEsporta.Visible = True
        RicercaElencoLista()
        ApriCSV1.Visible = False
    End Sub
    Private Sub RicercaElencoLista()

        Dim sqlDAP As New SqlClient.SqlDataAdapter
        Dim dataSet As New DataSet
        Dim strNomeStore As String = "[SP_CHECKLIST_RICERCA_RIMBORSI]"
        dgRisultatoRicerca.CurrentPageIndex = 0

        Try
            sqlDAP = New SqlClient.SqlDataAdapter(strNomeStore, CType(Session("conn"), SqlClient.SqlConnection))
            sqlDAP.SelectCommand.CommandType = CommandType.StoredProcedure
            sqlDAP.SelectCommand.Parameters.Add("@CodiceProgettoRendicontazione", SqlDbType.VarChar).Value = txtCodProgRendi.Text
            sqlDAP.SelectCommand.Parameters.Add("@StatoCheckList", SqlDbType.VarChar).Value = CboStatoChecklist.SelectedValue
            sqlDAP.SelectCommand.Parameters.Add("@nome", SqlDbType.VarChar).Value = txtnome.Text
            sqlDAP.SelectCommand.Parameters.Add("@cognome", SqlDbType.VarChar).Value = txtcognome.Text
            sqlDAP.SelectCommand.Parameters.Add("@codicefiscale", SqlDbType.VarChar).Value = txtcodicefiscale.Text
            sqlDAP.SelectCommand.Parameters.Add("@codicevolontario", SqlDbType.VarChar).Value = txtcodicevolontario.Text
            sqlDAP.SelectCommand.Parameters.Add("@dataDal", SqlDbType.VarChar).Value = txtdataDal.Text
            sqlDAP.SelectCommand.Parameters.Add("@dataAl", SqlDbType.VarChar).Value = txtdataAl.Text
            sqlDAP.SelectCommand.Parameters.Add("@codicechecklist", SqlDbType.VarChar).Value = txtcodicechecklist.Text

            sqlDAP.Fill(dataSet)

            Session("appDtsRisRicerca") = dataSet
            ViewState("datasource") = dataSet

            dgRisultatoRicerca.DataSource = dataSet
            dgRisultatoRicerca.DataBind()

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
            NomeColonne(6) = "Importo"
            NomeColonne(7) = "Data"
            NomeColonne(8) = "Stato"


            NomiCampiColonne(0) = "codicechecklist"
            NomiCampiColonne(1) = "CodiceVolontario"
            NomiCampiColonne(2) = "Cognome"
            NomiCampiColonne(3) = "Nome"
            NomiCampiColonne(4) = "CodiceFiscale"
            NomiCampiColonne(5) = "CodiceProgettoRendicontazione"
            NomiCampiColonne(6) = "Importo"
            NomiCampiColonne(7) = "DataRiferimento"
            NomiCampiColonne(8) = "StatoCheckList"

            'carico un datatable che userò poi nella pagina di stampa
            'il numero delle colonne è a base 0
            CaricaDataTablePerStampa(dataSet, 8, NomeColonne, NomiCampiColonne)

        Catch ex As Exception
            Response.Write(ex.Message.ToString())
            Exit Sub
        End Try
    End Sub
    Sub CaricaCombo()
        Dim strSql As String
        Try

            Dim MyDataset As DataSet


            'CboCodProgRend.Items.Clear()
            'strSql = "SELECT '' as CodiceProgettoRendicontazione UNION SELECT distinct a.CodiceProgettoRendicontazione as CodiceProgettoRendicontazione FROM CheckListPagheCollettivo a INNER JOIN StatiCheckList b on a.idstatochecklist = b.idstatochecklist WHERE(ANNOCOMPETENZA < Year(GETDATE()) Or (ANNOCOMPETENZA = Year(GETDATE()) And MESECOMPETENZA < Month(GETDATE()))) order by 1"

            'MyDataset = ClsServer.DataSetGenerico(strSql, Session("conn"))
            'CboCodProgRend.DataSource = MyDataset
            ''CboEsecuzionePagamento.DataValueField = "idstatochecklist"
            'CboCodProgRend.DataTextField = "CodiceProgettoRendicontazione"
            'CboCodProgRend.DataBind()

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

    Private Sub dgRisultatoRicerca_ItemCommand(source As Object, e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgRisultatoRicerca.ItemCommand
        If e.CommandName = "Selezionato" Then
            'Session("IdEnte") = e.Item.Cells(21).Text
            'Session("Denominazione") = e.Item.Cells(22).Text
            If (Session("TipoUtente") = "U") Then
                Response.Redirect("~/WfrmCheckListDettaglioRimborsoViaggio.aspx?Codchecklist=" & e.Item.Cells(3).Text & "&idLista=" & e.Item.Cells(0).Text & "&Data=" & e.Item.Cells(9).Text & "&idEntitaRimborso=" & e.Item.Cells(1).Text)
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
        NomeUnivoco = xPrefissoNome & "ExpDatiRimborso" & Year(Now) & Month(Now) & Day(Now) & Hour(Now) & Minute(Now) & Second(Now)
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
End Class