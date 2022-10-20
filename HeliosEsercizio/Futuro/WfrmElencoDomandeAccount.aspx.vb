Imports System.IO

Public Class WfrmElencoDomandeAccount
    Inherits System.Web.UI.Page
    Dim dsGenerico As DataSet 'dichiarazione dataset

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not Session("LogIn") Is Nothing Then
            If Session("LogIn") = False Then
                Response.Redirect("LogOn.aspx")
            End If
        Else
            Response.Redirect("LogOn.aspx")
        End If
        'Creato da Alessandra Taballione il 10/02/2004
        'codice necessario per inizializzare la pagina
        lblMessaggi.Visible = False
        'dsGenerico = ClsServer.DataSetGenerico(Request.QueryString("strsql"))
        If Not IsNothing(Context.Items("strsql")) Then
            txtstrsql.Value = Context.Items("strsql")
        End If
        dsGenerico = ClsServer.DataSetGenerico(txtstrsql.Value, Session("conn"))

        If IsPostBack = False Then

            CaricaDataGrid(dgRisultatoRicerca)

            If Context.Items("inviapassword") = "si" Then
                dgRisultatoRicerca.Columns(0).Visible = False
                dgRisultatoRicerca.Columns(1).Visible = True
            Else
                dgRisultatoRicerca.Columns(0).Visible = True
                dgRisultatoRicerca.Columns(1).Visible = False
            End If
            If dgRisultatoRicerca.Items.Count = 0 Then

                lblMessaggi.Visible = True
                lblMessaggi.Text = "Non è stato trovato nessun dato corrispondente al criterio di ricerca selezionato."
            End If
        End If

    End Sub

    Sub CaricaDataGrid(ByRef GridDaCaricare As DataGrid) 'valorizzo la datagrid passata
        GridDaCaricare.DataSource = dsGenerico
        GridDaCaricare.DataBind()

        'Aggiunto da Alessandra Taballione il 19/05/2005
        '*********************************************************************************
        'blocco per la creazione della datatable per la stampa della ricerca
        'nome e posizione di lettura delle colopnne a base 0
        Dim NomeColonne(6) As String
        Dim NomiCampiColonne(6) As String
        'nome della colonna 
        'e posizione nella griglia di lettura
        NomeColonne(0) = "Cod. Ente"
        NomeColonne(1) = "Ente"
        NomeColonne(2) = "Cod. Fiscale"
        NomeColonne(3) = "Stato"
        NomeColonne(4) = "Email"
        NomeColonne(5) = "Richiedente"
        NomeColonne(6) = "Telefono"

        NomiCampiColonne(0) = "CodiceRegione"
        NomiCampiColonne(1) = "Denominazione"
        NomiCampiColonne(2) = "Codicefiscale"
        NomiCampiColonne(3) = "StatoEnte"
        NomiCampiColonne(4) = "Email"
        NomiCampiColonne(5) = "Noterichiestaregistrazione"
        NomiCampiColonne(6) = "TelefonorichiestaRegistrazione"

        'carico un datatable che userò poi nella pagina di stampa
        'il numero delle colonne è a base 0
        CaricaDataTablePerStampa(dsGenerico, 6, NomeColonne, NomiCampiColonne)

        '*********************************************************************************
        GridDaCaricare.Visible = True
        If GridDaCaricare.Items.Count = 0 Then
            GridDaCaricare.Visible = False
            CmdEsporta.Visible = False
        Else
            CmdEsporta.Visible = True
        End If
    End Sub

    'Aggiunta da Alessndra Taballione il 19/05/2005
    'routine che carica la datatable che caricherà dinamicamente la datagrid della stampa delle ricerche
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

    Private Sub dgRisultatoRicerca_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgRisultatoRicerca.ItemCommand
        If e.CommandName = "Inoltra" Then
            'Response.Redirect("WfrmaccettaRic.aspx?Ente=" & CStr(e.Item.Cells(2).Text))
            Context.Items.Add("Ente", CStr(e.Item.Cells(3).Text))
            Context.Items.Add("Email", CStr(e.Item.Cells(6).Text))
            Context.Items.Add("password", CStr(e.Item.Cells(10).Text))
            Context.Items.Add("username", CStr(e.Item.Cells(11).Text))
            Context.Items.Add("strsql", txtstrsql.Value)
            Session("IdEnte") = CStr(e.Item.Cells(12).Text)
            Server.Transfer("WebInvioPassword.aspx")
        End If
    End Sub

    Private Sub dgRisultatoRicerca_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgRisultatoRicerca.PageIndexChanged
        dgRisultatoRicerca.SelectedIndex = -1
        dgRisultatoRicerca.EditItemIndex = -1
        dgRisultatoRicerca.CurrentPageIndex = e.NewPageIndex
        CaricaDataGrid(dgRisultatoRicerca)
    End Sub

    Private Sub cmdChiudi_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdChiudi.Click
        If Request.QueryString("VengoDa") = "Password" Then
            Response.Redirect("WfrmRicercaRicAccount.aspx?VengoDa=Password")
        Else
            Response.Redirect("WfrmRicercaRicAccount.aspx")
        End If
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

End Class