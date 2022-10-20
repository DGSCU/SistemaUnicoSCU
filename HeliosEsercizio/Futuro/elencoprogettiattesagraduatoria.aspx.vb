Imports System.Data.SqlClient
Imports System.IO

Public Class elencoprogettiattesagraduatoria
    Inherits System.Web.UI.Page
#Region "Utility"
    Private Sub ChiudiDataReader(ByRef dataReader As SqlDataReader)
        If Not dataReader Is Nothing Then
            dataReader.Close()
            dataReader = Nothing
        End If
    End Sub

    Private Sub VerificaSessione()
        If Not Session("LogIn") Is Nothing Then
            If Session("LogIn") = False Then
                Response.Redirect("LogOn.aspx")
            End If
        Else
            Response.Redirect("LogOn.aspx")
        End If
    End Sub

    Private Sub NascondiMenuLaterale()
        Session("TP") = True
    End Sub

#End Region
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load
        VerificaSessione()
        If Page.IsPostBack = False Then
            CaricaCompetenze()
        End If
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

                strSQL = "select IdRegioneCompetenza,Descrizione,CodiceRegioneCompetenza,left(CodiceRegioneCompetenza,1)from RegioniCompetenze "
                strSQL = strSQL & " order by left(CodiceRegioneCompetenza,1),descrizione "
                'chiudo il datareader se aperto
                If Not dtrCompetenze Is Nothing Then
                    dtrCompetenze.Close()
                    dtrCompetenze = Nothing
                End If

                'eseguo la query
                dtrCompetenze = ClsServer.CreaDatareader(strSQL, Session("conn"))
                'assegno il datadearder alla combo caricando così descrizione e id
                ddlRegioni.DataSource = dtrCompetenze
                ddlRegioni.Items.Add("")
                ddlRegioni.DataTextField = "Descrizione"
                ddlRegioni.DataValueField = "IDRegioneCompetenza"
                ddlRegioni.DataBind()
                'chiudo il datareader se aperto
                If Not dtrCompetenze Is Nothing Then
                    dtrCompetenze.Close()
                    dtrCompetenze = Nothing
                End If
            End If
            'Controllo abilitazione scelta
            If Session("TipoUtente") = "U" Then
                ddlRegioni.Enabled = True
                ddlRegioni.SelectedValue = 22

            Else
                'CboCompetenza.SelectedIndex = 1
                ddlRegioni.Enabled = False
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
                    ddlRegioni.SelectedValue = dtrCompetenze("IdRegioneCompetenza")
                    If dtrCompetenze("Heliosread") = True Then
                        ddlRegioni.Enabled = True
                    End If

                End If

            End If
        Catch ex As SqlClient.SqlException
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

    Private Sub imgChiudi_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles cmdChiudi.Click
        Response.Redirect("WfrmMain.aspx")
    End Sub

    Protected Sub cmdGenera_Click(sender As Object, e As EventArgs) 'Handles cmdGenera.Click
        Response.Write("<script type=""text/javascript"">" & vbCrLf)
        Response.Write("window.open(""stampaprogettiattesagraduatoria.aspx?strIdRegioneCompetenza=" & ddlRegioni.SelectedValue & """, ""Visualizza"", ""width=670,height=300,dependent=no,scrollbars=yes,status=no,resizable=yes"")" & vbCrLf)
        Response.Write("</script>")
    End Sub
    Private Function CreateDataSource() As DataTable
        Dim values(12) As Object
        Dim i As Integer
        Dim strSQL As String
        Dim dtrLocal As SqlClient.SqlDataReader
        Dim myCommand As SqlClient.SqlCommand
        Dim dt As New DataTable("Progetti")

        dt.Columns.Add(New DataColumn("CodiceEnte", Type.GetType("System.String")))
        dt.Columns.Add(New DataColumn("DenominazioneEnte", Type.GetType("System.String")))
        dt.Columns.Add(New DataColumn("CodiceProgetto", Type.GetType("System.String")))
        dt.Columns.Add(New DataColumn("TitoloProgetto", Type.GetType("System.String")))
        dt.Columns.Add(New DataColumn("Settore", Type.GetType("System.String")))
        dt.Columns.Add(New DataColumn("AreaIntervento", Type.GetType("System.String")))
        dt.Columns.Add(New DataColumn("VolontariRichiesti", Type.GetType("System.String")))
        dt.Columns.Add(New DataColumn("VolontariEffettivi", Type.GetType("System.String")))
        dt.Columns.Add(New DataColumn("Punteggio", Type.GetType("System.String")))
        dt.Columns.Add(New DataColumn("Limitazioni", Type.GetType("System.String")))
         
        If ddlRegioni.SelectedValue = 22 Then
            dt.Columns.Add(New DataColumn("Provincia", Type.GetType("System.String")))
            dt.Columns.Add(New DataColumn("Comune", Type.GetType("System.String")))
            dt.Columns.Add(New DataColumn("Regione", Type.GetType("System.String")))

            strSQL = "SELECT  isnull(replace(replace(replace(replace(replace(replace(replace(CODICEENTE,'°',''),'ì','i'''),'é','e'''),'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'') as CodiceEnte, "
            strSQL = strSQL & "isnull(replace(replace(replace(replace(replace(replace(replace(DENOMINAZIONEENTE,'°',''),'ì','i'''),'é','e'''),'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'') as DenominazioneEnte, "
            strSQL = strSQL & "isnull(replace(replace(replace(replace(replace(replace(replace(CODICEPROGETTO,'°',''),'ì','i'''),'é','e'''),'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'') as CodiceProgetto, "
            strSQL = strSQL & "isnull(replace(replace(replace(replace(replace(replace(replace(TITOLOPROGETTO,'°',''),'ì','i'''),'é','e'''),'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'') as TitoloProgetto, "
            strSQL = strSQL & "isnull(replace(replace(replace(replace(replace(replace(replace(SETTORE,'°',''),'ì','i'''),'é','e'''),'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'') as SETTORE, "
            strSQL = strSQL & "isnull(replace(replace(replace(replace(replace(replace(replace(AREAINTERVENTO,'°',''),'ì','i'''),'é','e'''),'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'') as AreaIntervento, "
            strSQL = strSQL & "VolontariRic as VolontariRichiesti, "
            strSQL = strSQL & "VolontariEff as VolontariEffettivi, "
            strSQL = strSQL & "PUNTEGGIO, "
            strSQL = strSQL & "isnull(replace(replace(replace(replace(replace(replace(replace(Regione,'°',''),'ì','i'''),'é','e'''),'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'') as Regione, "
            strSQL = strSQL & "isnull(replace(replace(replace(replace(replace(replace(replace(Provincia,'°',''),'ì','i'''),'é','e'''),'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'') as Provincia, "
            strSQL = strSQL & "isnull(replace(replace(replace(replace(replace(replace(replace(Comune,'°',''),'ì','i'''),'é','e'''),'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'') as Comune, "
            strSQL = strSQL & "Limitazioni "
            strSQL = strSQL & "FROM VW_ELENCO_PROGETTI_GRADUATORIA_PROVINCIA "
            strSQL = strSQL & "Where IdRegioneCompetenza=" & ddlRegioni.SelectedValue & " AND IdStatoAttività=9 AND AssociazioneAutomatica=1 "
            strSQL = strSQL & " AND MacroTipoProgetto like '" & Session("FiltroVisibilita") & "'"
            strSQL = strSQL & " ORDER BY Punteggio desc, codiceente asc, codiceprogetto asc"


        Else
            strSQL = "SELECT isnull(replace(replace(replace(replace(replace(replace(replace(CODICEENTE,'°',''),'ì','i'''),'é','e'''),'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'') as CodiceEnte, "
            strSQL = strSQL & "isnull(replace(replace(replace(replace(replace(replace(replace(DENOMINAZIONEENTE,'°',''),'ì','i'''),'é','e'''),'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'') as DenominazioneEnte, "
            strSQL = strSQL & "isnull(replace(replace(replace(replace(replace(replace(replace(CODICEPROGETTO,'°',''),'ì','i'''),'é','e'''),'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'') as CodiceProgetto, "
            strSQL = strSQL & "isnull(replace(replace(replace(replace(replace(replace(replace(TITOLOPROGETTO,'°',''),'ì','i'''),'é','e'''),'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'') as TitoloProgetto, "
            strSQL = strSQL & "isnull(replace(replace(replace(replace(replace(replace(replace(SETTORE,'°',''),'ì','i'''),'é','e'''),'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'') as SETTORE, "
            strSQL = strSQL & "isnull(replace(replace(replace(replace(replace(replace(replace(AREAINTERVENTO,'°',''),'ì','i'''),'é','e'''),'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'') as AreaIntervento, "
            strSQL = strSQL & "VolontariRic as VolontariRichiesti, "
            strSQL = strSQL & "VolontariEff as VolontariEffettivi, "
            strSQL = strSQL & "PUNTEGGIO, "
            strSQL = strSQL & "Limitazioni "
            strSQL = strSQL & "FROM VW_ELENCO_PROGETTI "
            strSQL = strSQL & "WHERE IdRegioneCompetenza=" & ddlRegioni.SelectedValue & " AND IdStatoAttività=9 AND AssociazioneAutomatica=1 "
            strSQL = strSQL & " AND MacroTipoProgetto like '" & Session("FiltroVisibilita") & "'"
            strSQL = strSQL & "ORDER BY Punteggio desc, codiceente asc, codiceprogetto asc"
        End If

        myCommand = New SqlClient.SqlCommand
        myCommand.Connection = Session("conn")
        myCommand.CommandText = strSQL

        dtrLocal = myCommand.ExecuteReader

        If dtrLocal.HasRows = True Then
            'dtrLocal.Read()
            dt.Load(dtrLocal)
           
        End If

        ChiudiDataReader(dtrLocal)

        Return dt

    End Function
    Protected Sub cmdEsporta_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdEsporta.Click
        lblInfo.Text = String.Empty
        cmdEsporta.Visible = False
        Dim dtbRicerca As DataTable = CreateDataSource()
        Session("dtbRicerca") = dtbRicerca
        CaricaDataGrid()
        StampaCSV(dtbRicerca)

    End Sub

    Private Sub StampaCSV(ByVal dtbRicerca As DataTable)
        Dim path As String
        Dim xPrefissoNome As String
        Dim url As String
        Dim utility As ClsUtility = New ClsUtility()

        If dtbRicerca.Rows.Count = 0 Then
            ApriCSV1.Visible = False
            cmdEsporta.Visible = False
            lblInfo.Text = "Non ci sono progetti in attesa di graduatoria."

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
    Private Sub CaricaDataGrid()
        ' Creo manualmente l'istanza del controllo DataGrid
        Dim grid As New DataGrid
        grid.ShowHeader = True

        grid.HeaderStyle.Font.Bold = True

        '' 1) imposto la sorgente dei dati per la griglia
        ''grid.DataSource = Session("appDtsRisRicerca").Tables(0)
        'grid.DataSource = dtsGenerico
        'grid.DataBind()
        Dim dataTable As DataTable = Session("dtbRicerca")
        Dim dimensioneTabella As Int16
        If ddlRegioni.SelectedValue = 22 Then
            dimensioneTabella = 13
        Else
            dimensioneTabella = 10
        End If
        Dim NomeColonne(dimensioneTabella) As String
        Dim NomiCampiColonne(dimensioneTabella) As String
        NomeColonne(1) = "CodiceEnte"
        NomeColonne(2) = "DenominazioneEnte"
        NomeColonne(3) = "CodiceProgetto"
        NomeColonne(4) = "TitoloProgetto"
        NomeColonne(5) = "Settore"
        NomeColonne(6) = "AreaIntervento"
        NomeColonne(7) = "VolontariRichiesti"
        NomeColonne(8) = "VolontariEffettivi"
        NomeColonne(9) = "Punteggio"
        NomeColonne(0) = "Limitazioni"


        NomiCampiColonne(1) = "CodiceEnte"
        NomiCampiColonne(2) = "DenominazioneEnte"
        NomiCampiColonne(3) = "CodiceProgetto"
        NomiCampiColonne(4) = "TitoloProgetto"
        NomiCampiColonne(5) = "Settore"
        NomiCampiColonne(6) = "AreaIntervento"
        NomiCampiColonne(7) = "VolontariRichiesti"
        NomiCampiColonne(8) = "VolontariEffettivi"
        NomiCampiColonne(9) = "Punteggio"
        NomiCampiColonne(0) = "Limitazioni"

        If ddlRegioni.SelectedValue = 22 Then
            NomeColonne(10) = "Provincia"
            NomiCampiColonne(10) = "Provincia"
            NomeColonne(11) = "Comune"
            NomiCampiColonne(11) = "Comune"
            NomeColonne(12) = "Regione"
            NomiCampiColonne(12) = "Regione"

        End If
        CaricaDataTablePerStampa(dataTable, dimensioneTabella - 1, NomeColonne, NomiCampiColonne)


    End Sub
    Sub CaricaDataTablePerStampa(ByVal DataSetDaScorrere As DataTable, ByVal NColonne As Integer, ByVal NomiColonne() As String, ByVal NomiCampiColonne() As String)
        Dim dt As New DataTable
        Dim dr As DataRow
        Dim i As Integer
        Dim x As Integer

        'carico i nomi delle colonne che andrò a stampare nella datagrid
        For x = 0 To NColonne
            dt.Columns.Add(New DataColumn(NomiColonne(x), GetType(String)))
        Next

        'carico il datatable con il risultato della query della ricerca, in qusto caso delle risorse
        If DataSetDaScorrere.Rows.Count > 0 Then
            For i = 1 To DataSetDaScorrere.Rows.Count
                dr = dt.NewRow()
                For x = 0 To NColonne
                    dr(x) = DataSetDaScorrere.Rows.Item(i - 1).Item(NomiCampiColonne(x))
                Next
                dt.Rows.Add(dr)
            Next
        End If

        'passo alla sessione la datatable che ho appena creato e che userò per il databinding della datagrid della stampa
        Session("DtbRicerca") = dt

    End Sub

    Protected Sub ddlRegioni_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlRegioni.SelectedIndexChanged
        cmdEsporta.Visible = True
        ApriCSV1.Visible = False
    End Sub
End Class