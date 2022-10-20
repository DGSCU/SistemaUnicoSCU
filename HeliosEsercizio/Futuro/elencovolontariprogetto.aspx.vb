Imports System.IO

Public Class elencovolontariprogetto
    Inherits System.Web.UI.Page
    Dim dtrgenerico As Data.SqlClient.SqlDataReader

#Region "Utility"
    Private Sub ChiudiDataReader(ByRef dataReader As SqlClient.SqlDataReader)
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
#End Region


    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load
        'effettua il redirect alla pagina di Login nel caso al sessione sia invalida
        VerificaSessione()

        If Request.QueryString("IdAttivita") <> "" Then
            Dim strATTIVITA As Integer = -1
            Dim strBANDOATTIVITA As Integer = -1
            Dim strENTEPERSONALE As Integer = -1
            Dim strENTITA As Integer = -1
            Dim strIDENTE As Integer = -1


            If ClsUtility.SICUREZZA_VERIFICA_AUTORIZZAZIONI(Session("conn"), Session("IdEnte"), Session("txtCodEnte"), Request.QueryString("IdAttivita"), strBANDOATTIVITA, strENTEPERSONALE, strENTITA, strIDENTE) = 1 Then
                ChiudiDataReader(dtrgenerico)
            Else
                ChiudiDataReader(dtrgenerico)
                Response.Redirect("wfrmAnomaliaDati.aspx")
            End If


        End If
        If IsPostBack = False Then
            lblprogetto.Text = Request.QueryString("strTitoloProgetto")
            Ricerca()
        Else
            If dgRisultatoRicerca.SelectedItem Is Nothing = True Then
                If (txtCodiceFiscale.Text = "" And txtCognome.Text = "" And txtNome.Text = "" And txtSede.Text = "") Then
                    'carico la griglia con le risorse dell'ente loggato
                    Ricerca()
                End If
            End If
            'chiamo la procedura che mi fa la ricerca e mi carica la griglia
            Ricerca()
        End If
        If Request.QueryString("VengoDa") <> "Progetti" Then
            lblSede.Visible = False
            txtSede.Visible = False
        End If
        caricaPosto()
        If Session("TipoUtente") = "E" Then
            BloccaDataInizio(Request.QueryString("IdAttivita"))
        End If
    End Sub

    Private Sub BloccaDataInizio(ByVal IdPrgetto As Integer)
        'Aggiunto da Simona Cordella il 20/06/2016
        'blocco della data inizio Attività
        Dim strSQL As String
        Dim dataInizioEffettivo As Date
        Dim DataOdierna As Date
        'datareader che conterrà l'id 
        Dim dtrData As System.Data.SqlClient.SqlDataReader
        strSQL = "select isnull(datainizioattività,'31/12/2030') as datainizioattività from attività where idattività= " & IdPrgetto & ""
        dtrData = ClsServer.CreaDatareader(strSQL, Session("conn"))
        If dtrData.HasRows = True Then
            dtrData.Read()
            dataInizioEffettivo = dtrData("datainizioattività")
        End If
        ChiudiDataReader(dtrData)

        'dataInizioEffettivo = Format(dataInizioEffettivo, "dddd, d MMM yyyy")
        ' DataOdierna = Format(, "dddd, d MMM yyyy")

        ' If (Date.TryParse(txtinizioEff.Text, dataInizioEffettivo)) Then
        If (dataInizioEffettivo > Date.Now) Then
            ' colonne 4 e 5 
            dgRisultatoRicerca.Columns.Item(4).Visible = False
            dgRisultatoRicerca.Columns.Item(5).Visible = False
        End If
        'End If

        'Image3.Visible = False
    End Sub

    Sub caricaPosto()
        Dim strSQL As String
        'datareader che conterrà l'id 
        Dim dtrPosto As System.Data.SqlClient.SqlDataReader
        strSQL = "select idtipologiaposto, descrizione from tipologieposto union select '' , ''  "
        ChiudiDataReader(dtrPosto)
        dtrPosto = ClsServer.CreaDatareader(strSQL, Session("conn"))
        ddlPosto.DataSource = dtrPosto
        ddlPosto.Items.Add("")
        ddlPosto.DataTextField = "Descrizione"
        ddlPosto.DataValueField = "IDTipologiaPosto"
        ddlPosto.DataBind()
        ChiudiDataReader(dtrPosto)
    End Sub
    Private Sub cmdChiudi_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles cmdChiudi.Click
        'If Request.QueryString("popup") <> "1" Then
        '    If Request.QueryString("VengoDa") = "Progetti" Then
        '        Response.Redirect("TabProgetti.aspx?Modifica=" & Request.QueryString("Modifica") & "&Nazionale=" & Request.QueryString("Nazionale") & "&IdAttivita=" & CInt(Request.QueryString("IdAttivita")) & "")
        '    Else
        '        Response.Redirect("WebGestioneSediProgetto.aspx?strTitoloProgetto=" & Request.QueryString("strTitoloProgetto") & "&blnVisualizzaVolontari=" & "True" & "&Modifica=" & Request.QueryString("Modifica") & "&Nazionale=" & Request.QueryString("Nazionale") & "&IdAttivita=" & CInt(Request.QueryString("IdAttivita")) & "")
        '    End If
        'Else
        '    If Request.QueryString("VengoDa") = "Progetti" Then
        '        Response.Redirect("TabProgetti.aspx?popup=" & Request.QueryString("popup") & "&Modifica=" & Request.QueryString("Modifica") & "&Nazionale=" & Request.QueryString("Nazionale") & "&IdAttivita=" & CInt(Request.QueryString("IdAttivita")) & "")
        '    Else
        '        Response.Redirect("WebGestioneSediProgetto.aspx?popup=" & Request.QueryString("popup") & "&strTitoloProgetto=" & Request.QueryString("strTitoloProgetto") & "&blnVisualizzaVolontari=" & "True" & "&Modifica=" & Request.QueryString("Modifica") & "&Nazionale=" & Request.QueryString("Nazionale") & "&IdAttivita=" & CInt(Request.QueryString("IdAttivita")) & "")
        '    End If
        'End If
        If Request.QueryString("VengoDa") = "Progetti" Then

            Response.Redirect(ClsUtility.TrovaAlboProgetto(Request.QueryString("IdAttivita"), Session("Conn")) & "?Modifica=" & Request.QueryString("Modifica") & "&Nazionale=" & Request.QueryString("Nazionale") & "&IdAttivita=" & CInt(Request.QueryString("IdAttivita")) & "")
            'Response.Redirect("TabProgetti.aspx?Modifica=" & Request.QueryString("Modifica") & "&Nazionale=" & Request.QueryString("Nazionale") & "&IdAttivita=" & CInt(Request.QueryString("IdAttivita")) & "")
        Else
            Response.Redirect("WebGestioneSediProgetto.aspx?strTitoloProgetto=" & Request.QueryString("strTitoloProgetto") & "&blnVisualizzaVolontari=" & "True" & "&Modifica=" & Request.QueryString("Modifica") & "&Nazionale=" & Request.QueryString("Nazionale") & "&IdAttivita=" & CInt(Request.QueryString("IdAttivita")) & "")
        End If
        
    End Sub

    Private Sub cmdRicerca_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles cmdRicerca.Click
        lblmessaggio.Text = String.Empty
        lblerrore.Text = String.Empty
        CmdEsporta.Visible = True
        ApriCSV1.Visible = False
        dgRisultatoRicerca.CurrentPageIndex = 0
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

    Sub Ricerca()
        '***Eseguo comando di ricerca
        Dim myDataSet As New DataSet
        Dim strQuery As String

        strQuery = "SELECT c.idattività,a.IDEntità, a.Cognome + ' ' + a.Nome as Nominativo, a.CodiceFiscale, "
        strQuery = strQuery & "case len(day(a.DataInizioServizio)) when 1 then '0' + convert(varchar(20),day(a.DataInizioServizio)) "
        strQuery = strQuery & "else convert(varchar(20),day(a.DataInizioServizio))  end + '/' + "
        strQuery = strQuery & "(case len(month(a.DataInizioServizio)) when 1 then '0' + convert(varchar(20),month(a.DataInizioServizio)) "
        strQuery = strQuery & "else convert(varchar(20),month(a.DataInizioServizio))  end + '/' + "
        strQuery = strQuery & "Convert(varchar(20), Year(a.DataInizioServizio))) as DataInizioServizio,  "
        strQuery = strQuery & "case len(day(a.DataFineServizio)) when 1 then '0' + convert(varchar(20),day(a.DataFineServizio)) "
        strQuery = strQuery & "else convert(varchar(20),day(a.DataFineServizio))  end + '/' + "
        strQuery = strQuery & "(case len(month(a.DataFineServizio)) when 1 then '0' + convert(varchar(20),month(a.DataFineServizio)) "
        strQuery = strQuery & "else convert(varchar(20),month(a.DataFineServizio))  end + '/' + "
        strQuery = strQuery & "Convert(varchar(20), Year(a.DataFineServizio))) as DataFineServizio,  "
        strQuery = strQuery & "d.denominazione as DenominazioneSedeAttuazione, d.IdEnteSedeAttuazione, e.denominazione as Sede, e.IdEnteSede, k.descrizione as posto, z.statoentità as statovol, "
        strQuery = strQuery & "max(datafineattivitàentità),(select count(*)-1 "
        strQuery = strQuery & "from attivitàentità where identità =a.identità) as numStorici, f.StatoAttivitàEntità "
        strQuery = strQuery & "FROM entità as a "
        strQuery = strQuery & "inner join attivitàentità as b on a.identità=b.identità "
        strQuery = strQuery & "inner join Tipologieposto as k on b.idtipologiaposto=k.idtipologiaposto "
        strQuery = strQuery & "inner join attivitàentisediattuazione as c on b.idattivitàentesedeattuazione=c.idattivitàentesedeattuazione "
        strQuery = strQuery & "inner join entisediattuazioni as d on c.identesedeattuazione=d.identesedeattuazione "
        strQuery = strQuery & "inner join entisedi as e on d.identesede=e.identesede "
        strQuery = strQuery & "inner join StatiEntità as z on a.idstatoEntità=z.idstatoentità "
        strQuery = strQuery & "inner join StatiAttivitàEntità as f on b.IdStatoAttivitàEntità=f.IdStatoAttivitàEntità "
        If Not Request.QueryString("strIdSedeAttuazione") Is Nothing Then
            strQuery = strQuery & "where c.IdEnteSedeAttuazione=" & CInt(Request.QueryString("strIdSedeAttuazione")) & " and f.DefaultStato=1 and c.idattività=" & CInt(Request.QueryString("IdAttivita")) & " "
        Else
            strQuery = strQuery & "where f.DefaultStato=1 and c.idattività=" & CInt(Request.QueryString("IdAttivita")) & " "
        End If
        'imposto eventuali parametri

        If txtCodiceFiscale.Text <> "" Then
            strQuery = strQuery & " and a.CodiceFiscale='" & ClsServer.NoApice(txtCodiceFiscale.Text) & "' "
        End If

        If txtCognome.Text <> "" Then
            strQuery = strQuery & " and a.Cognome like '" & ClsServer.NoApice(txtCognome.Text) & "%' "
        End If

        If txtNome.Text <> "" Then
            strQuery = strQuery & " and a.Nome like '" & ClsServer.NoApice(txtNome.Text) & "%' "
        End If

        If ddlPosto.SelectedValue <> "0" And ddlPosto.SelectedValue <> "" Then
            strQuery = strQuery & " and b.idtipologiaposto = '" & ddlPosto.SelectedValue & "' "
        End If


        If Request.QueryString("VengoDa") = "Progetti" Then

            If txtSede.Text <> "" Then
                strQuery = strQuery & " and e.denominazione like '" & ClsServer.NoApice(txtSede.Text) & "%' "
            End If
        End If

        strQuery = strQuery & "group by c.idattività,a.IDEntità, a.Cognome + ' ' + a.Nome, "
        strQuery = strQuery & "a.CodiceFiscale, a.DataInizioServizio, a.DataFineServizio, d.denominazione , "
        strQuery = strQuery & "e.denominazione, f.StatoAttivitàEntità, d.IdEnteSedeAttuazione, e.IdEnteSede,k.descrizione,z.statoentità "
        strQuery = strQuery & "having max(datafineattivitàentità)= (select max(datafineattivitàentità) "
        strQuery = strQuery & "from attivitàentità "
        strQuery = strQuery & "inner join attivitàentisediattuazione on attivitàentità.IDAttivitàEnteSedeAttuazione=attivitàentisediattuazione.IDAttivitàEnteSedeAttuazione "
        strQuery = strQuery & "where getdate()>a.DataInizioServizio and   identità =a.identità and attivitàentisediattuazione.idattività = c.idattività) order by a.Cognome + ' ' + a.Nome"

        myDataSet = ClsServer.DataSetGenerico(strQuery, Session("conn"))
        dgRisultatoRicerca.DataSource = myDataSet
        dgRisultatoRicerca.DataBind() 'valorizzo griglia

        Session("LocalDataSet") = myDataSet

        If dgRisultatoRicerca.Items.Count = 0 Then 'se la griglia e vuota la nascondo
            CmdEsporta.Visible = False
            dgRisultatoRicerca.Caption = "La ricerca non ha prodotto alcun risultato"
        Else
            CmdEsporta.Visible = True
            dgRisultatoRicerca.Caption = "Risultato Ricerca Volontari Progetto"
        End If

        Dim NomeColonne(8) As String
        Dim NomiCampiColonne(8) As String

        NomeColonne(0) = "Nominativo"
        NomeColonne(1) = "CodiceFiscale"
        NomeColonne(2) = "Stato Attività Volontario"
        NomeColonne(3) = "Stato Volontario"
        NomeColonne(4) = "Data Inizio Servizio"
        NomeColonne(5) = "Data Fine Servizio"
        NomeColonne(6) = "Sede Attuazione"
        NomeColonne(7) = "Sede"
        NomeColonne(8) = "Tipo Posto"

        NomiCampiColonne(0) = "Nominativo"
        NomiCampiColonne(1) = "CodiceFiscale"
        NomiCampiColonne(2) = "StatoAttivitàEntità"
        NomiCampiColonne(3) = "StatoVol"
        NomiCampiColonne(4) = "DataInizioServizio"
        NomiCampiColonne(5) = "DataFineServizio"
        NomiCampiColonne(6) = "DenominazioneSedeAttuazione"
        NomiCampiColonne(7) = "Sede"
        NomiCampiColonne(8) = "posto"

        'carico un datatable che userò poi nella pagina di stampa
        'il numero delle colonne è a base 0
        CaricaDataTablePerStampa(myDataSet, 8, NomeColonne, NomiCampiColonne)

    End Sub

    Private Sub dgRisultatoRicerca_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgRisultatoRicerca.PageIndexChanged
        dgRisultatoRicerca.CurrentPageIndex = e.NewPageIndex
        dgRisultatoRicerca.DataSource = Session("LocalDataSet")
        dgRisultatoRicerca.DataBind()
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
        Dim Writer As StreamWriter
        Dim xLinea As String = String.Empty
        Dim i As Int64
        Dim j As Int64
        Dim NomeUnivoco As String
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