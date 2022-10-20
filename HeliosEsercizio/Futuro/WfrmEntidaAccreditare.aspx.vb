Imports System.IO

Public Class WfrmEntidaAccreditare
    Inherits System.Web.UI.Page
    Dim mydataset As DataSet
    Dim dtrgenerico As SqlClient.SqlDataReader

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        '***Generata da Gianluigi Paesani in data:15/06/04
        If Not Session("LogIn") Is Nothing Then 'controlli formali per la session utente
            If Session("LogIn") = False Then
                Response.Redirect("LogOn.aspx") 'verifico validità log-in
            End If
        Else
            Response.Redirect("LogOn.aspx")
        End If

        If IsPostBack = False Then
            'Aggiunto da Alessandra  Taballione il 20/06/2005
            'Inseriment nuovo filtro di Ricerca
            ddlTipoRicerca.Items.Add("")
            ddlTipoRicerca.Items.Add("Nuove Iscrizioni")
            ddlTipoRicerca.Items.Add("Adeguamenti")

            ddlTipologia.Items.Add("")
            ddlTipologia.Items.Add("Pubblico")
            ddlTipologia.Items.Add("Privato")
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

            'popolo combo classiaccreditamento
            mydataset = ClsServer.DataSetGenerico(strsql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))

            If False Then
                mydataset = ClsServer.DataSetGenerico("select '0' as idclasseAccreditamento,'Seleziona' as classeAccreditamento  from classiaccreditamento " &
                    " union " &
                    " Select idclasseAccreditamento,classeAccreditamento " &
                    " from classiaccreditamento where minsedi >0 ", IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
            End If
            ddlClasseRichiesta.DataSource = mydataset
            ddlClasseRichiesta.DataValueField = "idSezione"
            ddlClasseRichiesta.DataTextField = "classeAccreditamento"
            ddlClasseRichiesta.DataBind()
            mydataset.Dispose()
            mydataset = Nothing

            If Session("CodiceRegioneEnte") <> "" Then
                txtCodiceEnte.Text = Session("txtCodEnte")
            End If

        End If
        If dgRisultatoRicerca.Items.Count = 0 Then
            CmdEsporta.Visible = False
        Else
            CmdEsporta.Visible = True
        End If
    End Sub

    Private Sub cmdRicerca_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdRicerca.Click
        '***Generata da Gianluigi Paesani in data:15/06/04
        ApriCSV1.Visible = False
        lblmess.Text = ""
        'salvo parametri per controllo utenza link pagine
        txtdenominazione1.Value = txtdenominazione.Text
        txttipologia1.Value = CStr(ddlTipologia.SelectedIndex)
        Txtclasserichiesta1.Value = CStr(ddlClasseRichiesta.SelectedIndex)
        Txtclasseattribuita1.Value = CStr(ddlClasseAttribuita.SelectedIndex)
        dgRisultatoRicerca.CurrentPageIndex = 0
        'richiano sub per eseguire ricerca
        PopolaGriglia(0)
        If dgRisultatoRicerca.Items.Count = 0 Then
            CmdEsporta.Visible = False
        Else
            CmdEsporta.Visible = True
        End If
    End Sub

    Private Sub PopolaGriglia(ByVal bytVerifica As Byte, Optional ByVal bytpage As Integer = 0)
        'datareader che conterrà l'id regione compentenza
        Dim dtrCompetenze As System.Data.SqlClient.SqlDataReader
        'per le regioni
        Dim strsqlReg As String

        '***Generata da Gianluigi Paesani in data:15/06/04
        '***Valorizzo griglia con risultati ricerca
        lblmess.Text = ""
        'controllo se la chiamata viene effettuata dal link di pagina o dal pulsante ricerca
        If bytVerifica = 1 Then dgRisultatoRicerca.CurrentPageIndex = bytpage
        dgRisultatoRicerca.Visible = True
        Dim strsql As String = "Select a.IDente, a.Denominazione,a.CodiceRegione,a.Tipologia," &
                " coalesce(sezioni.sezione, b.Classeaccreditamento) classeAttribuita, " &
                " coalesce(sezioni.sezione, c.Classeaccreditamento) ClasseRichiesta, " &
                " a.Numerototalesedi,a.Denominazione," &
                " a.Http As http ,a.Email As email ,a.emailcertificata As Pec, " &
                "(Select    (COUNT(*) + (Select  count(*) " &
                " FROM    entisedi INNER JOIN " &
                " entisediattuazioni On entisedi.IDEnteSede = entisediattuazioni.IDEnteSede " &
                " INNER JOIN StatiEntiSedi On entisedi.IDStatoEnteSede = StatiEntiSedi.IdStatoEnteSede " &
                " INNER JOIN StatiEntiSedi StatiEntiSedi_1 " &
                " On entisediattuazioni.IdStatoEnteSede = StatiEntiSedi_1.IdStatoEnteSede " &
                " WHERE (StatiEntiSedi.Attiva = 1 Or StatiEntiSedi.DaAccreditare = 1 ) " &
                " And (StatiEntiSedi_1.Attiva = 1 Or StatiEntiSedi_1.DaAccreditare = 1) " &
                " And (entisedi.IDEnte = a.idente)And (substring(entisedi.usernamestato,1,1) <> 'N') ) )as nsedi " &
                " FROM entisedi " &
                " INNER JOIN entisediattuazioni ON entisedi.IDEnteSede = entisediattuazioni.IDEnteSede " &
                " INNER JOIN StatiEntiSedi ON entisedi.IDStatoEnteSede = StatiEntiSedi.IdStatoEnteSede " &
                " INNER JOIN StatiEntiSedi StatiEntiSedi_1 ON entisediattuazioni.IdStatoEnteSede = StatiEntiSedi_1.IdStatoEnteSede " &
                " INNER JOIN entirelazioni ON entisedi.IDEnte = entirelazioni.IDEnteFiglio  " &
                " INNER JOIN AssociaEntiRelazioniSediAttuazioni " &
                " ON entisediattuazioni.IDEnteSedeAttuazione = AssociaEntiRelazioniSediAttuazioni.IdEnteSedeAttuazione AND " &
                " entirelazioni.IDEnteRelazione = AssociaEntiRelazioniSediAttuazioni.IdEnteRelazione " &
                " WHERE     (StatiEntiSedi.Attiva = 1 or StatiEntiSedi.DaAccreditare = 1) AND (StatiEntiSedi_1.Attiva = 1 or " &
                " StatiEntiSedi_1.DaAccreditare = 1) AND (entirelazioni.IDEntePadre = a.idente) AND (GETDATE() BETWEEN " &
                " ISNULL(entirelazioni.DataInizioValidità, '2000-01-01') AND ISNULL(entirelazioni.DataFineValidità,'2030-01-01')and (substring(entisedi.usernamestato,1,1) <> 'N') )) as Nsedi " &
                " from Enti a " &
                " inner join classiaccreditamento b on" &
                " b.idclasseaccreditamento = a.idclasseaccreditamento " &
                " inner join classiaccreditamento c on  " &
                " c.idclasseaccreditamento=a.idclasseaccreditamentoRichiesta" &
                " inner join statienti d on d.idstatoente=a.idstatoente" &
                " inner join TipologieEnti t on a.tipologia =t.descrizione " &
                " left join SezioniAlboSCU AS sezioni ON sezioni.IdSezione = a.IdSezione " &
                " where d.defaultstato <> 1 and d.istruttoria=1 "
        'imposto eventuali parametri
        If txtdenominazione.Text <> "" Then
            strsql = strsql & " and a.denominazione like '" & ClsServer.NoApice(txtdenominazione.Text) & "%'"
        End If
        ''  " (SELECT COUNT(*) FROM EntiSediAttuazioni " & _
        '" INNER JOIN StatiEntiSedi ON EntiSediAttuazioni.IdStatoEnteSede = StatiEntiSedi.IdStatoEnteSede " & _
        '" INNER JOIN EntiSedi ON EntiSedi.IdEnteSede = EntiSediAttuazioni.IdEnteSede " & _
        '" WHERE (StatiEntiSedi.Attiva = 1 or StatiEntiSedi.DaAccreditare = 1 )AND IdEnte = a.idente) as nsediOld," & _
        If txtCodiceEnte.Text <> "" Then
            strsql = strsql & " and a.CodiceRegione ='" & ClsServer.NoApice(txtCodiceEnte.Text) & "'"
        End If
        'If ddlTipologia.SelectedItem.Text <> "" Then
        '    strsql = strsql & " and a.tipologia='" & ClsServer.NoApice(ddlTipologia.SelectedItem.Text) & "'"
        'End If

        If ddlTipologia.SelectedItem.Text <> "" Then
            If ddlTipologia.SelectedItem.Text = "Privato" Then
                strsql = strsql & " and t.privato =1"
            Else
                strsql = strsql & " and t.privato =0"
            End If

        End If

        If ddlClasseAttribuita.SelectedItem.Text <> "Seleziona" Then
            strsql = strsql & " and  a.IdSezione='" & ddlClasseAttribuita.SelectedValue & "'"
        End If
        If ddlClasseRichiesta.SelectedItem.Text <> "Seleziona" Then
            strsql = strsql & " and  a.IdSezione='" & ddlClasseRichiesta.SelectedValue & "'"
        End If
        'Aggiunto da Alessandra Taballione il 20/06/2005
        'implementazione dei filtri di ricerca
        If ddlTipoRicerca.SelectedItem.Text = "Nuove Iscrizioni" Then
            strsql = strsql & " and b.DefaultClasse=1"
        End If
        If ddlTipoRicerca.SelectedItem.Text = "Adeguamenti" Then
            strsql = strsql & " and (b.defaultClasse <> 1  and b.minSedi > 0) "
        End If

        ''filtro per le regioni
        If Session("TipoUtente") = "R" Then
            'preparo la query
            strsqlReg = "select b.IdRegioneCompetenza from RegioniCompetenze a "
            strsqlReg = strsqlReg & "INNER JOIN utentiunsc b ON a.idregionecompetenza = b.idregionecompetenza "
            strsqlReg = strsqlReg & "where b.username = '" & Session("Utente") & "'"
            'chiudo il datareader se aperto
            If Not dtrCompetenze Is Nothing Then
                dtrCompetenze.Close()
                dtrCompetenze = Nothing
            End If
            'eseguo la query
            dtrCompetenze = ClsServer.CreaDatareader(strsqlReg, Session("conn"))

            If dtrCompetenze.HasRows = True Then
                dtrCompetenze.Read()
                'If dtrCompetenze("Heliosread") = True Then
                strsql = strsql & " And a.IdRegioneCompetenza = " & dtrCompetenze("IdRegioneCompetenza") & " "
                'End If
            End If

            If Not dtrCompetenze Is Nothing Then
                dtrCompetenze.Close()
                dtrCompetenze = Nothing
            End If
        End If

        strsql = strsql & " and c.minsedi > 0 order by a.denominazione"
        mydataset = ClsServer.DataSetGenerico(strsql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
        dgRisultatoRicerca.DataSource = mydataset 'valorizzo griglia
        CaricaDataTablePerStampa(mydataset)
        dgRisultatoRicerca.DataBind()

        If dgRisultatoRicerca.Items.Count = 0 And bytVerifica = 0 Then 'se la griglia e vuota la nascondo
            lblmess.Text = "La ricerca non ha prodotto alcun risultato"
            dgRisultatoRicerca.Visible = False
        End If

    End Sub

    'routine che carica la datatable che caricherà dinamicamente la datagrid della stampa delle ricerche
    Sub CaricaDataTablePerStampa(ByVal DataSetDaScorrere As DataSet)
        Dim dt As New DataTable
        Dim dr As DataRow
        Dim i As Integer
        Dim x As Integer

        Dim NomeColonne(8) As String
        Dim NomiCampiColonne(8) As String

        'nome della colonna 
        'e posizione nella griglia di lettura
        NomeColonne(0) = "Cod.Ente"
        NomeColonne(1) = "Denominazione"
        NomeColonne(2) = "Tipologia"
        NomeColonne(3) = "Sezione Attribuita"
        NomeColonne(4) = "Sezione Richiesta"
        NomeColonne(5) = "Nr.Sedi"
        NomeColonne(6) = "http://"
        NomeColonne(7) = "Email"
        NomeColonne(8) = "Pec"

        NomiCampiColonne(0) = "Codiceregione"
        NomiCampiColonne(1) = "Denominazione"
        NomiCampiColonne(2) = "Tipologia"
        NomiCampiColonne(3) = "ClasseAttribuita"
        NomiCampiColonne(4) = "ClasseRichiesta"
        NomiCampiColonne(5) = "nsedi"
        NomiCampiColonne(6) = "http"
        NomiCampiColonne(7) = "Email"
        NomiCampiColonne(8) = "Pec"

        'carico i nomi delle colonne che andrò a stampare nella datagrid
        For x = 0 To 8
            dt.Columns.Add(New DataColumn(NomeColonne(x), GetType(String)))
        Next

        'carico il datatable con il risultato della query della ricerca, in qusto caso delle risorse
        If DataSetDaScorrere.Tables(0).Rows.Count > 0 Then
            For i = 1 To DataSetDaScorrere.Tables(0).Rows.Count
                dr = dt.NewRow()
                For x = 0 To 8
                    dr(x) = DataSetDaScorrere.Tables(0).Rows.Item(i - 1).Item(NomiCampiColonne(x))
                Next
                dt.Rows.Add(dr)
            Next
        End If

        'passo alla sessione la datatable che ho appena creato e che userò per il databinding della datagrid della stampa
        Session("DtbRicerca") = dt

    End Sub

    Private Sub dgRisultatoRicerca_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgRisultatoRicerca.PageIndexChanged
        '***Generata da Gianluigi Paesani in data:15/06/04
        '***ripasso parametri per consapevolezza dati
        lblmess.Text = ""
        txtdenominazione.Text = txtdenominazione1.Value
        ddlTipologia.SelectedIndex = CInt(txttipologia1.Value)
        ddlClasseRichiesta.SelectedIndex = CInt(Txtclasserichiesta1.Value)
        ddlClasseAttribuita.SelectedIndex = CInt(Txtclasseattribuita1.Value)
        'passo valore pagina
        PopolaGriglia(1, e.NewPageIndex)
    End Sub

    Private Sub cmdChiudi_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdChiudi.Click
        '***Generata da Gianluigi Paesani in data:15/06/04
        '***chiamo homepage
        Response.Redirect("WfrmMain.aspx")
    End Sub

    Private Sub dgRisultatoRicerca_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgRisultatoRicerca.SelectedIndexChanged
        '***Generata da Gianluigi Paesani in data:04/06/04
        '***Passo parametri alla web form WfrmAlbero
        '***e valorizzo le session per l'ente
        Dim strsql As String
        lblmess.Text = ""
        If Not dgRisultatoRicerca.SelectedItem Is Nothing Then
            Session("denominazione") = dgRisultatoRicerca.SelectedItem.Cells(2).Text
            Session("idente") = CInt(dgRisultatoRicerca.SelectedItem.Cells(9).Text)
            Session("txtCodEnte") = dgRisultatoRicerca.SelectedItem.Cells(1).Text 'aggiunta da DS il 29/05/2017
            'Modificato da Alessandra Taballione il 31.03/05
            'se he relazioni visualizzo maschera ElencoEntiin Accordo altrimenti l'albero per l'accreditamento
            'Response.Redirect("Wfrmalbero.aspx?tipologia=Enti&Arrivo=WfrmEntidaAccreditare.aspx")
            strsql = "select identerelazione from entirelazioni where identepadre=" & dgRisultatoRicerca.SelectedItem.Cells(9).Text & "  and datafinevalidità is null"
            If Not dtrgenerico Is Nothing Then
                dtrgenerico.Close()
                dtrgenerico = Nothing
            End If
            dtrgenerico = ClsServer.CreaDatareader(strsql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
            If dtrgenerico.HasRows = True Then
                If Not dtrgenerico Is Nothing Then
                    dtrgenerico.Close()
                    dtrgenerico = Nothing
                End If
                Response.Redirect("WfrmElencoEntiAccordo.aspx?idente=" + dgRisultatoRicerca.SelectedItem.Cells(9).Text)
            Else
                If Not dtrgenerico Is Nothing Then
                    dtrgenerico.Close()
                    dtrgenerico = Nothing
                End If
                Response.Redirect("Wfrmalbero.aspx?tipologia=Enti&Arrivo=WfrmEntidaAccreditare.aspx")
            End If
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