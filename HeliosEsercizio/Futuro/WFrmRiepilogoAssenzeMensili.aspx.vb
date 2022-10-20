Imports System.IO

Public Class WFrmRiepilogoAssenzeMensili
    Inherits System.Web.UI.Page

    Dim dtrgenerico As Data.SqlClient.SqlDataReader

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        '***Generata da Gianluigi Paesani in data:05/07/04
        'Inserire qui il codice utente necessario per inizializzare la pagina
        If Not Session("LogIn") Is Nothing Then
            If Session("LogIn") = False Then 'verifico validità log-in
                Response.Redirect("LogOn.aspx")
            End If
        Else
            Response.Redirect("LogOn.aspx")
        End If
        If dgRisultatoRicerca.Items.Count > 0 Then
            CmdEsporta.Visible = True
        Else
            CmdEsporta.Visible = False
        End If
        If IsPostBack = False Then
            ddlmesiRitardo.Items.Add("")
            ddlmesiRitardo.Items.Add("= 0")
            ddlmesiRitardo.Items.Add("= 1")
            ddlmesiRitardo.Items.Add("= 2")
            ddlmesiRitardo.Items.Add("= 3")
            ddlmesiRitardo.Items.Add("> 3")
        End If

    End Sub

    Private Sub cmdChiudi_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdChiudi.Click
        Response.Redirect("WfrmMain.aspx")
    End Sub

    Private Sub dgRisultatoRicerca_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgRisultatoRicerca.PageIndexChanged
        dgRisultatoRicerca.Visible = True
        'passo valore pagina
        EseguiRicerca(1, e.NewPageIndex)
    End Sub

    Private Sub EseguiRicerca(ByVal bytVerifica As Byte, Optional ByVal bytpage As Integer = 0)
        '***Generata da Gianluigi Paesani in data:05/07/04
        '***Eseguo comando di ricerca
        Dim Mydataset As New DataSet
        Dim strquery As String
        'controllo se la chiamata viene effettuata dal link di pagina o dal pulsante ricerca
        If bytVerifica = 1 Then dgRisultatoRicerca.CurrentPageIndex = bytpage
        strquery = "Select IDEnte, CodiceRegione, Denominazione, Telefono, Email,DataPartenza," & _
                   "DataUltimaConferma, UltimoMeseConfermato,MesiNonConfermati,volontaripagabili,volontarinonpagabili,Indirizzo " & _
                   "From VW_RiepilogoMensileAssenze  Where IdEnte <> 0 "
        If Trim(txtDenominazione.Text) <> "" Then
            strquery = strquery & " and Denominazione like '" & Replace(txtDenominazione.Text, "'", "''") & "%' "
        End If
        If Trim(txtCodEnte.Text) <> "" Then
            strquery = strquery & " and Codiceregione = '" & Replace(txtCodEnte.Text, "'", "''") & "' "
        End If
        If ddlmesiRitardo.SelectedItem.Text <> "" Then
            strquery = strquery & " and mesinonconfermati " & ddlmesiRitardo.SelectedItem.Text & ""
        End If
        Mydataset = ClsServer.DataSetGenerico(strquery, Session("conn"))
        dgRisultatoRicerca.DataSource = Mydataset
        CaricaDataTablePerStampa(Mydataset)
        dgRisultatoRicerca.DataBind() 'valorizzo griglia
        If dgRisultatoRicerca.Items.Count = 0 Then 'se la griglia e vuota la nascondo
            lblmessaggio.Text = "La ricerca non ha prodotto alcun risultato"
            dgRisultatoRicerca.Visible = False
            CmdEsporta.Visible = False
        Else
            dgRisultatoRicerca.Visible = True
            CmdEsporta.Visible = True
        End If
    End Sub

    Sub CaricaDataTablePerStampa(ByVal DataSetDaScorrere As DataSet)
        Dim dt As New DataTable
        Dim dr As DataRow
        Dim i As Integer
        Dim x As Integer

        Dim NomeColonne(10) As String
        Dim NomiCampiColonne(10) As String

        'nome della colonna 
        'e posizione nella griglia di lettura
        NomeColonne(0) = "Cod. Ente"
        NomeColonne(1) = "Denominazione"
        NomeColonne(2) = "Telefono"
        NomeColonne(3) = "Email"
        NomeColonne(4) = "Data Partenza"
        NomeColonne(5) = "Data Ultima Conferma"
        NomeColonne(6) = "Ultimo Mese Confermato"
        NomeColonne(7) = "Mesi non Confermati"
        NomeColonne(8) = "Vol. non Pagabili"
        NomeColonne(9) = "Vol. Pagabili"
        NomeColonne(10) = "Indirizzo"

        NomiCampiColonne(0) = "Codiceregione"
        NomiCampiColonne(1) = "Denominazione"
        NomiCampiColonne(2) = "Telefono"
        NomiCampiColonne(3) = "Email"
        NomiCampiColonne(4) = "DataPartenza"
        NomiCampiColonne(5) = "DataUltimaConferma"
        NomiCampiColonne(6) = "UltimoMeseConfermato"
        NomiCampiColonne(7) = "Mesinonconfermati"
        NomiCampiColonne(8) = "VolontarinonPagabili"
        NomiCampiColonne(9) = "VolontariPagabili"
        NomiCampiColonne(10) = "Indirizzo"

        'carico i nomi delle colonne che andrò a stampare nella datagrid
        For x = 0 To 10
            dt.Columns.Add(New DataColumn(NomeColonne(x), GetType(String)))
        Next

        'carico il datatable con il risultato della query della ricerca, in qusto caso delle risorse
        If DataSetDaScorrere.Tables(0).Rows.Count > 0 Then
            For i = 1 To DataSetDaScorrere.Tables(0).Rows.Count
                dr = dt.NewRow()
                For x = 0 To 10
                    dr(x) = DataSetDaScorrere.Tables(0).Rows.Item(i - 1).Item(NomiCampiColonne(x))
                Next
                dt.Rows.Add(dr)
            Next
        End If

        'passo alla sessione la datatable che ho appena creato e che userò per il databinding della datagrid della stampa
        Session("DtbRicerca") = dt

    End Sub

    Private Sub cmdSalva_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdSalva.Click
        lblmessaggio.Text = ""
        dgRisultatoRicerca.CurrentPageIndex = 0
        EseguiRicerca(0)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
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