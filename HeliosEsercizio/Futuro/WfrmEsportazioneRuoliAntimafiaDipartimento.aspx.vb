Imports Logger.Data
Imports System.Collections.Generic
Imports System.Data.SqlClient
Imports System.IO

Public Class WfrmEsportazioneRuoliAntimafiaDipartimento
    Inherits SmartPage

    Dim strsql As String
    Dim dtrgenerico As SqlClient.SqlDataReader
    Dim dtsGenerico As DataSet
    Const INDEX_SELEZIONA = 0
    Const INDEX_CODICEREGIONE = 1
    Const INDEX_DENOMINAZIONE = 2
    Const INDEX_SEZIONE=3
    Const INDEX_STATODICHIARAZIONEANTIMAFIA = 4
    Const INDEX_DataChiusuraFase=5
    Const INDEX_DATAINVIOBDNA = 6
    Const INDEX_IDENTE = 8
    Const INDEX_CODICEFISCALE_ENTE = 9
    Const INDEX_TIPOLOGIA = 10
    Const INDEX_PROVINCIA = 11
    Const INDEX_COMUNE = 12

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Response.ExpiresAbsolute = #1/1/1980#
        Response.AddHeader("Pragma", "no-cache")
        If Not Session("LogIn") Is Nothing Then
            If Session("LogIn") = False Then 'verifico validità log-in
                Response.Redirect("LogOn.aspx")
            End If
        Else
            Response.Redirect("LogOn.aspx")
        End If

        If Session("TipoUtente") <> "U" Then
            divRicerca.Visible = False
            divForbidden.Visible = True
            Exit Sub
        End If

        If Not IsPostBack Then
            divRicerca.Visible = True
            divEsporta.Visible = False
            hdsElencoEntiPerAntiMafia.Value = Guid.NewGuid().ToString

            'popolo combo classiaccreditamento

            'Aggiunto da Alessandra  Taballione il 20/06/2005
            'Inseriment nuovo filtro di Ricerca
            Dim mydataset As DataSet
            Dim strsql As String = "SELECT '0' AS idSezione,'Seleziona' AS classeAccreditamento "
            strsql &= "FROM SezioniAlboSCU "
            strsql &= "UNION "
            strsql &= "SELECT S.idSezione , S.Sezione AS classeAccreditamento "
            strsql &= "FROM SezioniAlboSCU S JOIN classiaccreditamento C "
            strsql &= "ON S.IdClasseAccreditamento = C.IDClasseAccreditamento "
            strsql &= "WHERE(C.MINSEDI > 0) "


            'popolo combo classiaccreditamento
            mydataset = ClsServer.DataSetGenerico(strsql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))

            ddlClasseAttribuita.DataSource = mydataset
            ddlClasseAttribuita.DataValueField = "idSezione"
            ddlClasseAttribuita.DataTextField = "classeAccreditamento"
            ddlClasseAttribuita.DataBind()

            mydataset.Dispose()
            mydataset = Nothing

            'popolo combo Stato Ente

            strsql = "SELECT '0' AS IdStatoEnte,'Seleziona' AS StatoEnte "
            strsql &= "UNION "
            strsql &= "SELECT IdStatoEnte, StatoEnte "
            strsql &= "FROM StatiEnti "
            strsql &= "WHERE idstatoente in (3,6,7,8,9,11) "


            'popolo combo classiaccreditamento
            mydataset = ClsServer.DataSetGenerico(strsql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))

            ddlStatoEnte.DataSource = mydataset
            ddlStatoEnte.DataValueField = "IdStatoEnte"
            ddlStatoEnte.DataTextField = "StatoEnte"
            ddlStatoEnte.DataBind()

            mydataset.Dispose()
            mydataset = Nothing

            If Session("CodiceRegioneEnte") <> "" Then
                txtCodiceEnte.Text = Session("txtCodEnte")
            End If
        End If
    End Sub


    Protected Sub cmdRicerca_Click(sender As Object, e As EventArgs) Handles cmdRicerca.Click
        PopolaGriglia()
    End Sub

    Private Sub PopolaGriglia()
        strsql = "select IDente, Denominazione, CodiceRegione, Sezione, UltimaEsportazioneDati, Stato,  DataChiusuraFase, CodiceFiscale, Tipologia, Provincia+' ('+Targa+')' provincia, comune, StatoEnte,IdStatoEnte from VW_statoantimafia where 1=1 "
        'eventuali condizioni supplementari
        If txtdenominazione.Text <> "" Then
            strsql = strsql & " and denominazione like '" & ClsServer.NoApice(txtdenominazione.Text) & "%'"
        End If

        If txtCodiceEnte.Text <> "" Then
            strsql = strsql & " and CodiceRegione  like '" & ClsServer.NoApice(txtCodiceEnte.Text) & "%'"
        End If

        If ddlClasseAttribuita.SelectedItem.Text <> "Seleziona" Then
            strsql = strsql & " and IdSezione='" & ClsServer.NoApice(ddlClasseAttribuita.SelectedItem.Value) & "'"
        End If

        If ddlStatoAntimafia.SelectedItem.Value > "0" Then
            strsql = strsql & " and  StatoN=" & ClsServer.NoApice(ddlStatoAntimafia.SelectedItem.Value)
        End If

        If ddlStatoEnte.SelectedItem.Text <> "Seleziona" Then
            strsql = strsql & " and IdStatoEnte='" & ClsServer.NoApice(ddlStatoEnte.SelectedItem.Value) & "'"
        End If

        'fine condizioni supplementari
        strsql += " order by  denominazione"

        Dim mydtsGenerico = ClsServer.DataSetGenerico(strsql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))

        Session(hdsElencoEntiPerAntiMafia.Value) = mydtsGenerico
        PreparaStampaRicerca(mydtsGenerico)

        dgRisultatoRicerca.CurrentPageIndex = 0 'strano non dovrebbe servire
        dgRisultatoRicerca.DataSource = Session(hdsElencoEntiPerAntiMafia.Value) 'valorizzo griglia
        dgRisultatoRicerca.DataBind()

        If dgRisultatoRicerca.Items.Count = 0 Then 'se la griglia è vuota la nascondo
            lblmess.Visible = True
            lblmess.Text = "La ricerca non ha prodotto alcun risultato"
            dgRisultatoRicerca.Visible = False
            cmdEsporta.Visible = False
        Else
            lblmess.Visible = False
            lblmess.Text = ""
            dgRisultatoRicerca.Visible = True
            cmdEsporta.Visible = True
        End If
        'ControllaSelezione()

    End Sub

    Private Sub PreparaStampaRicerca(dts As System.Data.DataSet)
        '*********************************************************************************
        'blocco per la creazione della datatable per la stampa della ricerca

        'nome e posizione di lettura delle colopnne a base 0
        Dim NomeColonne(10) As String
        Dim NomiCampiColonne(10) As String
        'nome della colonna 
        'e posizione nella griglia di lettura
        NomeColonne(0) = "Cod. Ente"
        NomeColonne(1) = "Denominazione"
        NomeColonne(2) = "Sezione"
        NomeColonne(3) = "Stato Ente"
        NomeColonne(4) = "Stato Dichiarazione Antimafia"
        NomeColonne(5) = "Chiusura aggiornamento"
        NomeColonne(6) = "Data Ultima Esportazione per BDNA"

        NomiCampiColonne(0) = "Codiceregione"
        NomiCampiColonne(1) = "Denominazione"
        NomiCampiColonne(2) = "Sezione"
        NomiCampiColonne(3) = "StatoEnte"
        NomiCampiColonne(4) = "stato"
        NomiCampiColonne(5) = "DataChiusuraFase"
        NomiCampiColonne(6) = "UltimaEsportazioneDati"

        'carico un datatable che userò poi nella pagina di stampa
        'il numero delle colonne è a base 0
        CaricaDataTablePerStampaRicerca(dts, 6, NomeColonne, NomiCampiColonne)

        '*********************************************************************************

    End Sub

    'routine che carica la datatable che caricherà dinamicamente la datagrid della stampa delle ricerche
    Sub CaricaDataTablePerStampaRicerca(ByVal DataSetDaScorrere As DataSet, ByVal NColonne As Integer, ByVal NomiColonne() As String, ByVal NomiCampiColonne() As String)
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
        Session("DtbRicercaCSV") = dt

    End Sub

    Private Sub dgRisultatoRicerca_ItemCommand(source As Object, e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgRisultatoRicerca.ItemCommand

        Select Case e.CommandName
            Case "Select"
                Dim idEnte As String = e.Item.Cells(INDEX_IDENTE).Text
                LblEnteEsportazione.Text = ""
                lblMsgEsportazione.CssClass = "msgConferma"
                LblMsgEsportazione.Text = ""
                ApriCSV1.Visible = False
                divEsporta.Visible = True
                divRicerca.Visible = False
                Dim infoRuoli As New clsRuoloAntimafia.InfoAdeguamentoAntimafia(idEnte, Session("Conn"), False)
                hdIdEnteFase.Value = infoRuoli.IdEnteFaseAntimafia
                HdEnte.Value = idEnte
                Dim nRuoli As Integer = 0
                strsql = "Select count(id) as nRuoli from RuoliAntimafia where IdEnteFaseAntimafia = @IdEnteFaseAntimafia and idente=@idente"
                Dim EnteCommand As New SqlCommand(strsql, Session("conn"))
                EnteCommand.Parameters.AddWithValue("@IdEnteFaseAntimafia", CInt(hdIdEnteFase.Value))
                EnteCommand.Parameters.AddWithValue("@idente", CInt(idEnte))
                Dim dtrEnte As SqlDataReader = EnteCommand.ExecuteReader()
                If dtrEnte.Read Then
                    nRuoli = CInt("0" & dtrEnte("nRuoli"))
                End If
                dtrEnte.Close()
                If nRuoli = 0 Then
                    btnTest.Visible = False
                    btnExport.Visible = False
                Else
                    btnTest.Visible = True
                    btnExport.Visible = True
                End If
                LblEnteEsportazione.Text = e.Item.Cells(INDEX_CODICEREGIONE).Text & " - " _
                    & e.Item.Cells(INDEX_DENOMINAZIONE).Text _
                    & " - CF: " & e.Item.Cells(INDEX_CODICEFISCALE_ENTE).Text _
                    & " - Tipologia: " & e.Item.Cells(INDEX_TIPOLOGIA).Text _
                    & " - Provincia: " & e.Item.Cells(INDEX_PROVINCIA).Text _
                    & " - Comune: " & e.Item.Cells(INDEX_COMUNE).Text _
                    & "<p>Numero ruoli antimafia da esportare: " & nRuoli
        End Select

    End Sub

    Private Sub dgRisultatoRicerca_PageIndexChanged(source As Object, e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgRisultatoRicerca.PageIndexChanged
        dgRisultatoRicerca.CurrentPageIndex = e.NewPageIndex
        dgRisultatoRicerca.DataSource = Session(hdsElencoEntiPerAntiMafia.Value)
        dgRisultatoRicerca.DataBind()
        'ControllaSelezione()
        lblmess.Text = ""
    End Sub

    Private Sub btnChiudi_Click(sender As Object, e As System.EventArgs) Handles btnChiudi.Click
        hdIdEnteFase.Value = String.Empty
        divEsporta.Visible = False
        divRicerca.Visible = True
        PopolaGriglia()
    End Sub

    Private Sub cmdChiudi_Click(sender As Object, e As System.EventArgs) Handles cmdChiudi.Click
        Response.Redirect("WfrmMain.aspx")
    End Sub

    Sub Esporta(isTest As Boolean)

        strsql = "Select 1 as Ordine," &
        " '""'+case when c.ComuneNazionale = 1 then RIGHT('00000000000'+ e.CodiceFiscale, 11 ) else e.codicefiscale end + '""' as CodiceFiscaleEnte," &
        " '""'+a.CodiceFiscale+'""' as CodiceFiscale, '""'+ r.RuoloAntiMafia+'""' as RuoloAntimafia, null campo5, null campo6, Cognome, Nome " &
        " from ruoliantimafia a inner join ElencoRuoliAntimafia r" &
        " on a.IdElencoRuoliAntimafia = r.Id inner join" &
        " enti e on a.idente=e.idente inner join" &
        " entisedi s on e.idente=s.idente inner join" &
        " statientisedi st on st.idstatoentesede=s.idstatoentesede inner join" &
        " entiseditipi t ON s.IDEnteSede = t.IDEnteSede inner join" &
        " comuni c ON s.IDComune =c.IDComune " &
        " where t.idtiposede = 1 and (st.attiva=1 or st.DefaultStato=1) and" &
        " a.IdEnteFaseAntimafia = " & hdIdEnteFase.Value & " and a.idente = " & HdEnte.Value


        Dim mydtsGenerico = ClsServer.DataSetGenerico(strsql, Session("Conn"))
        Dim _clsR As New clsRuoloAntimafia
        lblMsgEsportazione.Text = ""
        lblMsgEsportazione.Visible = True

        Try
            LblMsgEsportazione.Text = "Esportazione dei dati avvenuta correttamente. Clicca su  <b>Download CSV</B> per scaricare il file"

            Dim NomeColonne(7) As String
            Dim NomiCampiColonne(7) As String
            'nome della colonna 
            'e posizione nella griglia di lettura
            NomeColonne(0) = " "
            NomeColonne(1) = "Codice Fiscale Ente"
            NomeColonne(2) = "Codice Fiscale"
            NomeColonne(3) = "Ruolo Antimafia"
            NomeColonne(4) = "campo5"
            NomeColonne(5) = "campo6"
            NomeColonne(6) = "Cognome"
            NomeColonne(7) = "Nome"
            NomiCampiColonne(0) = "Ordine"
            NomiCampiColonne(1) = "CodiceFiscaleEnte"
            NomiCampiColonne(2) = "CodiceFiscale"
            NomiCampiColonne(3) = "RuoloAntimafia"
            NomiCampiColonne(4) = "campo5"
            NomiCampiColonne(5) = "campo6"
            NomiCampiColonne(6) = "Cognome"
            NomiCampiColonne(7) = "Nome"

            CaricaDataTablePerStampa(mydtsGenerico, IIf(isTest, 7, 5), NomeColonne, NomiCampiColonne)
            Dim dtbRicerca As DataTable = Session("DtbRicerca")
            StampaCSV(dtbRicerca, isTest)
            '_clsR.ExportCSV(mydtsGenerico, Session("Utente") & NomeFile & Format(DateTime.Now, "ddMMyyyyhhmmss") & ".csv", Page)

            If isTest = False Then
                strsql = "update RuoliAntimafia set UltimaEsportazioneDati = GETDATE() where IdEnteFaseAntimafia = " & CInt(hdIdEnteFase.Value) & " and idente = " & HdEnte.Value

                Dim EnteCommand As New SqlCommand(strsql, Session("conn"))
                'EnteCommand.Parameters.AddWithValue("@IdEnteFaseAntimafia", CInt(hdIdEnteFase.Value))
                EnteCommand.ExecuteNonQuery()

                'lblMsgEsportazione.CssClass = "msgConferma"

                Log.Warning(LogEvent.ANTIMAFIA_ESPORTAZIONE_DIPARTIMENTO_INVIO_BDNA, "Utente" & CStr(Session("Username")))
            End If
        Catch ex As Exception
            If ex.Message.Trim.ToUpper <> "THREAD INTERROTTO." Then
                Log.Error(LogEvent.ANTIMAFIA_ESPORTAZIONE_DIPARTIMENTO_ERRORE, "Errore", exception:=ex)
                lblMsgEsportazione.CssClass = "msgErrore"
                lblMsgEsportazione.Text = "Errore nella creazione del CSV. Contattare l'assistenza."
                Exit Sub
            Else

            End If


        End Try
    End Sub

    Private Sub btnTest_Click(sender As Object, e As EventArgs) Handles btnTest.Click
        Esporta(True)
    End Sub

    Private Sub btnExport_Click(sender As Object, e As EventArgs) Handles btnExport.Click
        Esporta(False)
    End Sub
    Function CreaFileCSV(ByVal DTBRicerca As DataTable, ByVal xPrefissoNome As String, ByVal mapPath As String, ByVal IsTest As Boolean) As String

        Dim writer As StreamWriter
        Dim xLinea As String = String.Empty
        Dim i As Int64
        Dim j As Int64
        Dim nomeUnivoco As String
        Dim url As String
        If IsTest Then
            nomeUnivoco = xPrefissoNome & "_RuoliAntiMafiaInvioBDNA_Test_" & Year(Now) & Month(Now) & Day(Now) & Hour(Now) & Minute(Now) & Second(Now)
        Else
            nomeUnivoco = xPrefissoNome & "_RuoliAntiMafiaInvioBDNA_" & Year(Now) & Month(Now) & Day(Now) & Hour(Now) & Minute(Now) & Second(Now)
        End If

        writer = New StreamWriter(mapPath & "\" & nomeUnivoco & ".CSV")
        'Creazione dell'inntestazione del CSV
        Dim intNumCol As Int64 = DTBRicerca.Columns.Count
        If Not IsTest Then intNumCol -= 2 'elimina nome e cognome alla fine
        If False Then
            For i = 0 To intNumCol - 1
                xLinea &= DTBRicerca.Columns.Item(CInt(i)).ColumnName() & ";"
            Next
            writer.WriteLine(xLinea.Substring(0, xLinea.Length - 1))
        End If
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
            'writer.WriteLine(xLinea.Substring(0, xLinea.Length - 1))
            writer.WriteLine(xLinea)
            xLinea = vbNullString

        Next
        url = "download\" & nomeUnivoco & ".CSV"

        writer.Close()
        writer = Nothing
        Return url
    End Function
    Private Sub StampaCSV(ByVal dtbRicerca As DataTable, ByVal istest As Boolean)
        Dim path As String
        Dim xPrefissoNome As String
        Dim url As String
        Dim utility As ClsUtility = New ClsUtility()

        If dtbRicerca.Rows.Count = 0 Then
            ApriCSV1.Visible = False
        Else
            xPrefissoNome = Left(LblEnteEsportazione.Text, InStr(LblEnteEsportazione.Text, " ") - 1)
            path = Server.MapPath("download")
            url = CreaFileCSV(dtbRicerca, xPrefissoNome, path, istest)
            ApriCSV1.Visible = True
            ApriCSV1.NavigateUrl = url
        End If
    End Sub
    Sub CaricaDataTable()

        '*********************************************************************************
        'blocco per la creazione della datatable per la stampa della ricerca
        'nome e posizione di lettura delle colopnne a base 0
        Dim NomeColonne(3) As String
        Dim NomiCampiColonne(3) As String
        'nome della colonna 
        'e posizione nella griglia di lettura
        NomeColonne(0) = ""
        NomeColonne(1) = "Codice Fiscale Ente"
        NomeColonne(2) = "Codice Fiscale"
        NomeColonne(3) = "Ruolo Antimafia"
        'NomeColonne(4) = "Variazioni"
        'NomeColonne(5) = "Visto"
        'NomeColonne(5) = "Indirizzo"
        'NomeColonne(6) = "Comune"
        'NomeColonne(7) = "Telefono"
        'NomeColonne(8) = "NMaxVolontari"
        'NomeColonne(9) = "Titolo di Possedimento"
        'NomeColonne(10) = "Normativa81"
        'NomeColonne(11) = "Conformita"
        'NomeColonne(12) = "Soggetto Estero"
        'NomeColonne(13) = "Dichiarazione di Soggetto Estero"


        'If Session("TipoUtente") <> "E" Then
        '    NomeColonne(14) = "Presenza Certificazione"
        'End If


        NomiCampiColonne(0) = "Ordine"
        NomiCampiColonne(1) = "CodiceFiscaleEnte"
        NomiCampiColonne(2) = "CodiceFiscale"
        NomiCampiColonne(3) = "RuoloAntimafia"
        'NomiCampiColonne(4) = "Variazioni"
        'NomiCampiColonne(5) = "FlgValidazione"
        'NomiCampiColonne(5) = "Indirizzo"
        'NomiCampiColonne(6) = "Comune"
        'NomiCampiColonne(7) = "Telefono"
        'NomiCampiColonne(8) = "NMaxVolontari"
        'NomiCampiColonne(9) = "TitoloPossedimento"
        'NomiCampiColonne(10) = "Normativa81"
        'NomiCampiColonne(11) = "Conformita"
        'NomiCampiColonne(12) = "Soggettoestero"
        'NomiCampiColonne(13) = "DichSoggettoEstero"
        'If Session("TipoUtente") <> "E" Then
        '    NomiCampiColonne(14) = "Certificazione"
        'End If

        'carico un datatable che userò poi nella pagina di stampa
        'il numero delle colonne è a base 0
        'If Session("TipoUtente") <> "E" Then
        '    CaricaDataTablePerStampa(dtsGenerico, 14, NomeColonne, NomiCampiColonne)
        'Else
        '    CaricaDataTablePerStampa(dtsGenerico, 13, NomeColonne, NomiCampiColonne)
        'End If
        CaricaDataTablePerStampa(dtsGenerico, 5, NomeColonne, NomiCampiColonne)
        'CaricaDataTablePerStampa(dtsGenerico, 8, NomeColonne, NomiCampiColonne)

        '*********************************************************************************


    End Sub

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
    Sub ControllaSelezione()
        Dim item As DataGridItem
        For Each item In dgRisultatoRicerca.Items
            Dim mybutton As ImageButton = DirectCast(item.FindControl("ImgEnte"), ImageButton)
            Select Case dgRisultatoRicerca.Items(item.ItemIndex).Cells(INDEX_STATODICHIARAZIONEANTIMAFIA).Text.Trim
                Case "Dati antimafia in aggiornamento"
                    mybutton.Enabled = False
                Case Else
                    mybutton.Enabled = True
            End Select
            'check.Enabled = False
            dgRisultatoRicerca.Items(item.ItemIndex).Cells(5).Text = Replace(dgRisultatoRicerca.Items(item.ItemIndex).Cells(5).Text, " | ", "</br>")
        Next
    End Sub

    Private Sub cmdEsporta_Click(sender As Object, e As System.EventArgs) Handles cmdEsporta.Click
        cmdEsporta.Visible = False
        Dim dtbRicerca As DataTable = Session("DtbRicercaCSV")
        StampaCSVRicerca(dtbRicerca)
    End Sub

    Private Sub StampaCSVRicerca(ByVal dtbRicerca As DataTable)
        Dim path As String
        Dim xPrefissoNome As String
        Dim url As String
        Dim utility As ClsUtility = New ClsUtility()

        If dtbRicerca.Rows.Count = 0 Then
            ApriCSV2.Visible = False
            CmdEsporta.Visible = False
        Else
            xPrefissoNome = Session("Utente")
            path = Server.MapPath("download")
            url = CreaFileCSVRicerca(dtbRicerca, xPrefissoNome, path)
            ApriCSV2.Visible = True
            ApriCSV2.NavigateUrl = url
        End If

    End Sub

    Function CreaFileCSVRicerca(ByVal DTBRicerca As DataTable, ByVal xPrefissoNome As String, ByVal mapPath As String) As String

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
