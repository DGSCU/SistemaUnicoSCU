Imports System.Data.SqlClient
Imports System.Drawing.Printing
Imports System.Drawing.Imaging
Imports System.Web.UI
Imports System.Drawing
Imports System.IO
Public Class WfrmNavigaSedi
    Inherits System.Web.UI.Page
    Dim strsql As String
    Dim dtrgenerico As SqlClient.SqlDataReader
    Dim dtsGenerico As DataSet
    Private Sub ChiudiDataReader(ByRef dataReader As SqlDataReader)
        If Not dataReader Is Nothing Then
            dataReader.Close()
            dataReader = Nothing
        End If
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim dtrGenericoLocal As SqlClient.SqlDataReader
        If IsPostBack = False Then

            dtrGenericoLocal = ClsServer.CreaDatareader("Select tiposede,idtiposede from tipiSedi order by idtiposede desc ", Session("conn"))
            Do While dtrGenericoLocal.Read()
                ddlTipologia.Items.Add(dtrGenericoLocal("tiposede"))
            Loop

            'ddlTipologia.SelectedIndex = 1
            'ddlTipologia.Enabled = False
            ChiudiDataReader(dtrGenericoLocal)
            ddlstato.Items.Add("")
            dtrGenericoLocal = ClsServer.CreaDatareader("select statoEnteSede from StatiEntiSedi", Session("conn"))
            Do While dtrGenericoLocal.Read()
                ddlstato.Items.Add(dtrGenericoLocal("statoEnteSede"))
            Loop
            ChiudiDataReader(dtrGenericoLocal)

            RicercaSediEnte()

        End If

    End Sub
    Private Sub RicercaSediEnte(Optional ByVal bytpage As Integer = 0)


        Dim sqlDAP As New SqlClient.SqlDataAdapter
        dtsGenerico = New DataSet
        Dim strNomeStore As String = "SP_ACCREDITAMENTO_RICERCA_SEDE_NEW"

        Try
            sqlDAP = New SqlClient.SqlDataAdapter(strNomeStore, CType(Session("conn"), SqlClient.SqlConnection))
            sqlDAP.SelectCommand.CommandType = CommandType.StoredProcedure
            sqlDAP.SelectCommand.Parameters.Add("@IdEnte", SqlDbType.NVarChar, 50).Value = Request.QueryString("IdEnte")

            If txtdenominazione.Text <> "" Then
                sqlDAP.SelectCommand.Parameters.Add("@DenominazioneSede", SqlDbType.NVarChar, 200).Value = Trim(txtdenominazione.Text)
            End If

            If ddlstato.SelectedItem.Text <> "" Then
                sqlDAP.SelectCommand.Parameters.Add("@Stato", SqlDbType.NVarChar, 50).Value = ddlstato.SelectedItem.Text
            End If

            If txtregione.Text <> "" Then
                sqlDAP.SelectCommand.Parameters.Add("@Regioni", SqlDbType.NVarChar, 200).Value = Trim(txtregione.Text)
            End If

            If txtProvincia.Text <> "" Then
                sqlDAP.SelectCommand.Parameters.Add("@Provincia", SqlDbType.NVarChar, 200).Value = Trim(txtProvincia.Text)
            End If

            If txtComune.Text <> "" Then
                sqlDAP.SelectCommand.Parameters.Add("@Comune", SqlDbType.NVarChar, 200).Value = Trim(txtComune.Text)
            End If

            If txtIndirizzo.Text <> "" Then
                sqlDAP.SelectCommand.Parameters.Add("@Indirizzo", SqlDbType.NVarChar, 200).Value = Trim(txtIndirizzo.Text)
            End If

            If txtCodSedeAtt.Text <> "" Then
                sqlDAP.SelectCommand.Parameters.Add("@CodiceSedeAttuazione", SqlDbType.Int).Value = CInt(IIf(txtCodSedeAtt.Text = "", Nothing, txtCodSedeAtt.Text))
            End If

            If ddlTipologia.SelectedItem.Text <> "" Then
                sqlDAP.SelectCommand.Parameters.Add("@Tipologia", SqlDbType.NVarChar, 100).Value = IIf(ddlTipologia.SelectedItem.Text = "", Nothing, ddlTipologia.SelectedItem.Text)
            End If


            sqlDAP.Fill(dtsGenerico)

            CaricaDataGrid(dgRisultatoRicerca, bytpage)

            'sqlDAP.SelectCommand.Connection.Close()


        Catch ex As Exception
            Response.Write(ex.Message.ToString())
            Exit Sub
        End Try

        If (dgRisultatoRicerca.Items.Count > 0) Then
            CmdEsporta.Visible = True
            ApriCSV1.Visible = False
            dgRisultatoRicerca.Caption = "Risultato Ricerca Sedi Ente"
        Else
            CmdEsporta.Visible = False
            ApriCSV1.Visible = False
            dgRisultatoRicerca.Caption = "La ricerca non ha prodotto alcun risultato"
        End If
    End Sub
    Sub CaricaDataGrid(ByRef GridDaCaricare As DataGrid, Optional ByVal bytpage As Integer = 0) 'valorizzo la datagrid passata
        GridDaCaricare.CurrentPageIndex = bytpage
        GridDaCaricare.DataSource = dtsGenerico
        GridDaCaricare.DataBind()



        '*********************************************************************************
        'blocco per la creazione della datatable per la stampa della ricerca
        'nome e posizione di lettura delle colopnne a base 0
        Dim NomeColonne(11) As String
        Dim NomiCampiColonne(11) As String
        'nome della colonna 
        'e posizione nella griglia di lettura
        NomeColonne(0) = "Stato"
        NomeColonne(1) = "Ente Sede"
        NomeColonne(2) = "Ente"
        'NomeColonne(3) = "Tipologia"
        NomeColonne(3) = "Cod.Sede Attuaz."
        NomeColonne(4) = "Indirizzo"
        NomeColonne(5) = "Comune"
        NomeColonne(6) = "Telefono"
        'If Session("TipoUtente") <> "E" Then
        '    NomeColonne(8) = "Presenza Certificazione"
        'End If
        NomeColonne(7) = "Palazzina"
        NomeColonne(8) = "Scala"
        NomeColonne(9) = "Piano"
        NomeColonne(10) = "Interno"

        NomiCampiColonne(0) = "Stato"
        NomiCampiColonne(1) = "Sede"
        NomiCampiColonne(2) = "Ente"
        'NomiCampiColonne(3) = "Tiposede"
        NomiCampiColonne(3) = "NSedi"
        NomiCampiColonne(4) = "Indirizzo"
        NomiCampiColonne(5) = "Comune"
        NomiCampiColonne(6) = "Telefono"
        'If Session("TipoUtente") <> "E" Then
        '    NomiCampiColonne(8) = "Certificazione"
        'End If
        NomiCampiColonne(7) = "Palazzina"
        NomiCampiColonne(8) = "Scala"
        NomiCampiColonne(9) = "Piano"
        NomiCampiColonne(10) = "Interno"
        'carico un datatable che userò poi nella pagina di stampa
        'il numero delle colonne è a base 0
        'If Session("TipoUtente") <> "E" Then
        '    CaricaDataTablePerStampa(dtsGenerico, 8, NomeColonne, NomiCampiColonne)
        'Else
        '    CaricaDataTablePerStampa(dtsGenerico, 7, NomeColonne, NomiCampiColonne)
        'End If
        CaricaDataTablePerStampa(dtsGenerico, 10, NomeColonne, NomiCampiColonne)

        '*********************************************************************************

        If GridDaCaricare.Items.Count = 0 Then

            If Request.QueryString("esporta") = "si" Then
                CmdEsporta.Visible = False
            End If
        Else

            If Request.QueryString("esporta") = "si" Then
                CmdEsporta.Visible = True
                GridDaCaricare.Columns(0).Visible = False
            End If
        End If



        If Not dtsGenerico Is Nothing Then
            dtsGenerico = Nothing
        End If
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
    End Sub

    Sub CaricaDataTablePerStampa(ByVal DataSetDaScorrere As DataSet, ByVal NColonne As Integer, ByVal NomiColonne() As String, ByVal NomiCampiColonne() As String)
        Dim dt As New DataTable
        Dim dr As DataRow
        Dim i As Integer
        Dim x As Integer

        'carico i nomi delle colonne che andrò a stampare nella datagrid
        For x = 0 To NColonne
            dt.Columns.Add(New DataColumn(NomiColonne(x), GetType(String)))
        Next
        'DataSetDaScorrere = New DataSet()
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
    Protected Sub cmdEsporta_Click(ByVal sender As Object, ByVal e As EventArgs) Handles CmdEsporta.Click
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

        Dim writer As StreamWriter
        Dim xLinea As String = String.Empty
        Dim i As Int64
        Dim j As Int64
        Dim nomeUnivoco As String
        Dim url As String
        nomeUnivoco = xPrefissoNome & "ExpDatiSedi" & Year(Now) & Month(Now) & Day(Now) & Hour(Now) & Minute(Now) & Second(Now)
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
    Private Sub dgRisultatoRicerca_PageIndexChanged(source As Object, e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgRisultatoRicerca.PageIndexChanged
        RicercaSediEnte(e.NewPageIndex)
        dgRisultatoRicerca.SelectedIndex = -1
        dgRisultatoRicerca.EditItemIndex = -1
        'dgRisultatoRicerca.CurrentPageIndex = e.NewPageIndex
        'CaricaDataGrid(dgRisultatoRicerca)
        'CaricaDataGrid(dgRisultatoRicerca, e.NewPageIndex)

    End Sub

    Protected Sub cmdChiudi_Click(sender As Object, e As EventArgs) Handles cmdChiudi.Click
        If Request.QueryString("VengoDa") = 1 Then
            Response.Redirect("~/WfrmNavigaEnte.aspx?IdEnte=" & Request.QueryString("IdEnte") & "&VengoDa=" & 2 & "&CodiceFiscale=" & Request.QueryString("CodiceFiscale"))
        End If
        If Request.QueryString("VengoDa") = 3 Then
            Response.Redirect("~/WfrmNavigaEntiAccoglienza.aspx?IdEnte=" & Request.QueryString("IdEnte") & "&VengoDa=" & 2 & "&CodiceFiscale=" & Request.QueryString("CodiceFiscale") & "&Denominazione=" & Request.QueryString("Denominazione") & "&CodiceRegione=" & Request.QueryString("CodiceRegione") & "&CF=" & Request.QueryString("CF") & "&Tipologia=" & Request.QueryString("Tipologia") & "&ClasseAccreditamento=" & Request.QueryString("ClasseAccreditamento") & "&Stato=" & Request.QueryString("Stato") & "&Pagina=" & Request.QueryString("Pagina") & "&StatoEnte=" & Request.QueryString("StatoEnte"))
        End If
    End Sub

    Protected Sub cmdRicerca_Click(sender As Object, e As EventArgs) Handles cmdRicerca.Click
        RicercaSediEnte(0)
    End Sub
End Class