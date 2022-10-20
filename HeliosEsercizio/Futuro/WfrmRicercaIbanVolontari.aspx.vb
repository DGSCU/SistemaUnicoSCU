Imports System.IO
Imports System.Text.RegularExpressions

Public Class WfrmRicercaIbanVolontari
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Session("LogIn") Is Nothing Then
            If Session("LogIn") = False Then 'verifico validità log-in
                Response.Redirect("LogOn.aspx")
            End If
        Else
            Response.Redirect("LogOn.aspx")
        End If
        If Page.IsPostBack = False Then
            CaricaPrima()
            txtCodReg.Text = Session("txtCodEnte")
        End If
    End Sub
    Protected Sub CmdRicerca_Click(ByVal sender As Object, ByVal e As EventArgs) Handles CmdRicerca.Click
        Ricerca()
    End Sub
    Sub Ricerca()
        'verico se il codiceente indicato è corretto
        hlVolontari.Visible = False
        If Session("TipoUtente") <> "E" Then
            Dim blnRic As Boolean = RicordaIdEnte(txtCodReg.Text)
            lblmessaggio.Text = ""
            If blnRic = True Then
                dgRisultatoRicerca.Visible = True
                EseguiRicerca()
            Else
                dgRisultatoRicerca.Visible = False
                lblmessaggio.Text = "Indicare un Codice Ente valido."
            End If
        Else
            EseguiRicerca()
        End If
    End Sub
    Private Sub CaricaPrima()
        '***Carico combo settore
        ddlMaccCodAmAtt.DataSource = ClsServer.CreaDataTable("select idmacroambitoattività, codifica + ' - ' + MacroAmbitoAttività as Macro from macroambitiattività", True, Session("conn"))
        ddlMaccCodAmAtt.DataTextField = "Macro"
        ddlMaccCodAmAtt.DataValueField = "idmacroambitoattività"
        ddlMaccCodAmAtt.DataBind()

        '***Carico combo area intervento
        ddlCodAmAtt.Items.Add("")
        ddlCodAmAtt.Enabled = False

        '*****Carico Combo Bandi
        'Mod. il 03/12/2014 da simona cordella con il filtrovisibilità
        Dim strsql As String

        strsql = "SELECT DISTINCT Bando.idBando,bando.bandobreve,bando.annobreve "
        strsql = strsql & " FROM bando"
        strsql = strsql & " INNER JOIN AssociaBandoTipiProgetto abtp on abtp.idbando =  bando.idbando"
        strsql = strsql & " INNER JOIN TipiProgetto  tp on abtp.idtipoprogetto = tp.idtipoprogetto"
        strsql = strsql & " WHERE Bando.FormazioneGenerale=1 and tp.MacroTipoProgetto like '" & Session("FiltroVisibilita") & "'"
        strsql = strsql & " ORDER BY bando.annobreve desc"
        '"select idbando, bandobreve from Bando where FormazioneGenerale=1"
        DdlBando.DataSource = ClsServer.CreaDataTable(strsql, True, Session("conn"))
        DdlBando.DataTextField = "bandobreve"
        DdlBando.DataValueField = "idbando"
        DdlBando.DataBind()
        'fILTROVISIBILITA
        DdlTipiProgetto.DataSource = ClsServer.CreaDataTable("Select idTipoProgetto,Descrizione from TipiProgetto where MacroTipoProgetto like '" & Session("FiltroVisibilita") & "' ", True, Session("conn"))
        DdlTipiProgetto.DataTextField = "Descrizione"
        DdlTipiProgetto.DataValueField = "idTipoProgetto"
        DdlTipiProgetto.DataBind()

    End Sub

    Private Function RicordaIdEnte(ByVal strCodice As String) As Boolean
        Dim dtrGenerico As SqlClient.SqlDataReader
        Dim strquery As String

        If Not dtrGenerico Is Nothing Then
            dtrGenerico.Close()
            dtrGenerico = Nothing
        End If
        strquery = "select idente,denominazione,codiceregione,r.descrizione as Competenza"
        strquery = strquery & " from enti e"
        strquery = strquery & " inner join RegioniCompetenze  r on e.idregionecompetenza=r.idregionecompetenza"
        strquery = strquery & "  where codiceregione ='" & strCodice & "'"

        dtrGenerico = ClsServer.CreaDatareader(strquery, Session("Conn"))
        If dtrGenerico.HasRows = True Then
            dtrGenerico.Read()
            Session("IdEnte") = dtrGenerico("idente")
            'Session("Ente") = dgRisultatoRicerca.SelectedItem.Cells(1).Text
            Session("Denominazione") = dtrGenerico("denominazione")
            Session("Competenza") = dtrGenerico("Competenza")
            RicordaIdEnte = True
        Else
            If Not dtrGenerico Is Nothing Then
                dtrGenerico.Close()
                dtrGenerico = Nothing
            End If
            Session("IdEnte") = Nothing
            'Session("Ente") = dgRisultatoRicerca.SelectedItem.Cells(1).Text
            Session("Denominazione") = Nothing
            Session("Competenza") = Nothing
            RicordaIdEnte = False
        End If

        If Not dtrGenerico Is Nothing Then
            dtrGenerico.Close()
            dtrGenerico = Nothing
        End If
        Return RicordaIdEnte
    End Function

    Private Sub EseguiRicerca()
        'ByVal bytVerifica As Byte, Optional ByVal bytpage As Integer = 0
        Dim dtrGenerico As SqlClient.SqlDataReader
        Dim strquery As String
        '*****************************************************************************************+
        'AUTORE: Simona Cordella
        'DATA: 29/09/2009
        'DESCRIZONE: effetuo la ricerca dei progetti per poi esportare il file csv dei volontari di ogni singolo progetto
        lblmessaggio.Text = ""
        Dim Mydataset As New DataSet
        dgRisultatoRicerca.Visible = True
        ' Mydataset.Dispose()

        strquery = "select distinct b.denominazione, "
        strquery = strquery & " (a.titolo + ' [' + a.codiceente + ']') as titolo, "

        strquery = strquery & " (isNull(a.NumeroPostiNoVittoNoAlloggio,0)+ isnull(a.NumeroPostiVittoAlloggio,0) + isNull(a.NumeroPostiVitto,0)) as VolRic, c.bando," & _
        " g.macroambitoattività + ' / ' + f.ambitoattività as Ambito," & _
        " a.idattività,b.idente," & _
        " '0' as selezionato, a.IdTipoProgetto,e.statoattività,  "
        'SubQuery estrazione volontari in servizio e terminati durante il sevizio
        strquery = strquery & " (SELECT COUNT(DISTINCT entità.CodiceVolontario) " & _
                " FROM StatiEntità INNER JOIN entità INNER JOIN attivitàentità ON entità.IDEntità = attivitàentità.IDEntità INNER JOIN " & _
                " attivitàentisediattuazione ON attivitàentità.IDAttivitàEnteSedeAttuazione = attivitàentisediattuazione.IDAttivitàEnteSedeAttuazione " & _
                " INNER JOIN attività ON attivitàentisediattuazione.IDAttività = attività.IDAttività INNER JOIN " & _
                " enti ON attività.IDEntePresentante = enti.IDEnte ON StatiEntità.IDStatoEntità = entità.IDStatoEntità " & _
                " inner join tipiprogetto on attività.idtipoprogetto = tipiprogetto.idtipoprogetto " & _
                " WHERE attività.IDAttività = a.IDAttività And (StatiEntità.InServizio = 1 OR StatiEntità.Sospeso = 1  OR StatiEntità.Chiuso = 1) and attivitàentità.EscludiFormazione=0 " & _
                " and ((ENTITà.DataInizioSERVIZIO >= '01/12/2009' and tipiprogetto.nazionebase = 1) or ( ENTITà.DataInizioSERVIZIO >= '16/11/2009' and tipiprogetto.nazionebase = 0)))  AS Volontari  "
        'fine subquery
        strquery = strquery & " FROM entisediattuazioni INNER JOIN" & _
        " attivitàentisediattuazione ON entisediattuazioni.IDEnteSedeAttuazione = attivitàentisediattuazione.IDEnteSedeAttuazione INNER JOIN" & _
        " entisedi ON entisediattuazioni.IDEnteSede = entisedi.IDEnteSede INNER JOIN" & _
        " comuni ON entisedi.IDComune = comuni.IDComune INNER JOIN" & _
        " provincie ON comuni.IDProvincia = provincie.IDProvincia INNER JOIN" & _
        " regioni ON provincie.IDRegione = regioni.IDRegione INNER JOIN" & _
        " enti enti_1 ON entisedi.IDEnte = enti_1.IDEnte RIGHT OUTER JOIN" & _
        " attività a INNER JOIN" & _
        " enti b ON a.IDEntePresentante = b.IDEnte LEFT OUTER JOIN" & _
        " BandiAttività h ON a.IDBandoAttività = h.IdBandoAttività LEFT OUTER JOIN" & _
        " bando c ON c.IDBando = h.IdBando " & _
        " INNER JOIN TipiProgetto ON a.IdTipoProgetto = TipiProgetto.IdTipoProgetto " & _
        " INNER JOIN AssociaProfiliTipiProgetto ON TipiProgetto.IdTipoProgetto = AssociaProfiliTipiProgetto.IdTipoProgetto " & _
        " INNER JOIN Profili ON AssociaProfiliTipiProgetto.IdProfilo = Profili.IdProfilo "

        '============================================================================================================================
        '====================================================30/09/2008==============================================================
        '=======MODIFICA EFFETTUATA DA Kronk (99 scimmie saltavano sul letto, una cadde in terra e si ruppe il cervelletto...)=======
        '=================AGGIUNTA JOIN SU PROFILO READ PER ABILITARE LEGGERE LE ABILITAZIONI ANCHE DELL'UTENTE READ=================
        '============================================================================================================================
        If Session("Read") <> "1" Then
            strquery = strquery & " INNER JOIN	AssociaUtenteGruppo ON Profili.IdProfilo = AssociaUtenteGruppo.IdProfilo "
        Else
            strquery = strquery & " INNER JOIN	AssociaUtenteGruppo ON Profili.IdProfilo = AssociaUtenteGruppo.IdProfiloRead "
        End If

        strquery = strquery & " INNER JOIN" & _
        " statiattività e ON a.IDStatoAttività = e.IDStatoAttività INNER JOIN" & _
        " ambitiattività f ON f.IDAmbitoAttività = a.IDAmbitoAttività INNER JOIN" & _
        " macroambitiattività g ON f.IDMacroAmbitoAttività = g.IDMacroAmbitoAttività ON attivitàentisediattuazione.IDAttività = a.IDAttività LEFT OUTER JOIN" & _
        " statiBandiAttività i ON h.IdStatoBandoAttività = i.IdStatoBandoAttività " & _
        " WHERE a.idattività is not null and e.idstatoattività in (1)   " & _
        " and A.IDATTIVITà IN (SELECT DISTINCT ATTIVITà.IDATTIVITà " & _
        " FROM StatiEntità" & _
        " INNER JOIN entità " & _
        " INNER JOIN attivitàentità ON entità.IDEntità = attivitàentità.IDEntità " & _
        " INNER JOIN  attivitàentisediattuazione ON attivitàentità.IDAttivitàEnteSedeAttuazione = attivitàentisediattuazione.IDAttivitàEnteSedeAttuazione  " & _
        " INNER JOIN attività ON attivitàentisediattuazione.IDAttività = attività.IDAttività " & _
        " inner join tipiprogetto on attività.idtipoprogetto = tipiprogetto.idtipoprogetto" & _
        " INNER JOIN  enti ON attività.IDEntePresentante = enti.IDEnte ON StatiEntità.IDStatoEntità = entità.IDStatoEntità  " & _
        " WHERE(StatiEntità.InServizio = 1 Or StatiEntità.Sospeso = 1 Or StatiEntità.Chiuso = 1) " & _
        " and attivitàentità.EscludiFormazione=0 " & _
        " and ((ENTITà.DataInizioSERVIZIO >= '01/12/2009' and tipiprogetto.nazionebase = 1) or ( ENTITà.DataInizioSERVIZIO >= '16/11/2009' and tipiprogetto.nazionebase = 0))  "
        If CStr(Session("TipoUtente")) = "E" Then
            strquery = strquery & " AND ENTI.CODICEREGIONE = '" & CStr(Session("CodiceRegioneEnte")).Substring(1, 7) & "'"
        Else
            If txtCodReg.Text <> "" Then
                strquery = strquery & "  AND ENTI.CODICEREGIONE = '" & ClsServer.NoApice(txtCodReg.Text) & "'"
            End If
        End If
        '** agg. da s.c. il 27/11/2014 FiltroVisibilita
        strquery = strquery & " and TipiProgetto.MacroTipoProgetto like '" & Session("FiltroVisibilita") & "' "
        strquery = strquery & " )"

        If DdlTipiProgetto.SelectedItem.Text <> "" Then
            strquery = strquery & " and a.idTipoProgetto=" & DdlTipiProgetto.SelectedValue & ""
        End If

        If txtCodProg.Text <> "" Then
            strquery = strquery & " and a.Codiceente like '" & ClsServer.NoApice(Trim(txtCodProg.Text)) & "%'"
        End If

        If txtTitoloProgetto.Text <> "" Then
            strquery = strquery & " and a.titolo like '" & ClsServer.NoApice(Trim(txtTitoloProgetto.Text)) & "%'"
        End If

        If DdlBando.SelectedItem.Text <> "" Then
            strquery = strquery & " and c.bandobreve like '" & ClsServer.NoApice(DdlBando.SelectedItem.Text) & "%'"
        End If

        If CStr(Session("TipoUtente")) = "E" Then
            strquery = strquery & " and b.CodiceRegione like '" & CStr(Session("CodiceRegioneEnte")).Substring(1, 7) & "%'"
        Else
            If txtCodReg.Text <> "" Then
                strquery = strquery & " and b.CodiceRegione like '" & ClsServer.NoApice(txtCodReg.Text) & "%'"
            End If
        End If

        If txtDenominazioneEnte.Text <> "" Then
            strquery = strquery & " and b.denominazione like '" & ClsServer.NoApice(Trim(txtDenominazioneEnte.Text)) & "%'"
        End If


        If ddlMaccCodAmAtt.SelectedItem.Text = "" And ddlCodAmAtt.SelectedItem.Text = "" Then

        Else
            If ddlCodAmAtt.SelectedItem.Text <> "" Then
                strquery = strquery & " and f.idambitoattività=" & ddlCodAmAtt.SelectedValue & ""
            Else
                strquery = strquery & " and f.idmacroambitoattività=" & ddlMaccCodAmAtt.SelectedValue & ""
            End If
        End If

        strquery = strquery & " and AssociaUtenteGruppo.username='" & Replace(Session("Utente"), "'", "''") & "'  ORDER BY e.StatoAttività, b.Denominazione,Titolo"

        Mydataset = ClsServer.DataSetGenerico(strquery, Session("conn"))

        dgRisultatoRicerca.DataSource = Mydataset
        dgRisultatoRicerca.DataBind() 'valorizzo griglia

        Session("DtbRicVol") = Mydataset.Tables(0)


        chkSelDesel.Checked = False
        If dgRisultatoRicerca.Items.Count = 0 Then 'se la griglia e vuota la nascondo
            lblmessaggio.Text = "La ricerca non ha prodotto alcun risultato"
            dgRisultatoRicerca.Visible = False
            chkSelDesel.Visible = False
            imgEsporta.Visible = False
        Else
            imgEsporta.Visible = True
            chkSelDesel.Visible = True
        End If
    End Sub


    Private Sub RiccoraItemSelezionato(Optional ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs = Nothing)
        '*****************************************************************************************+
        'AUTORE: Simona Cordella
        'DATA: 29/09/2009
        'DESCRIZONE: memorizzo in un datatable il cambio di stato delle checkbox di selezione del progetto
        'il datatable memorizzato nella session lo utilizzo per nella maschera di esportazione csv (WfrmEsportazioneOreVolontari)
        '*****************************************************************************************+
        Dim i As Integer
        Dim Mychk As CheckBox
        Dim dtGriglia As DataTable 'appoggio
        Dim rwGriglia As DataRow

        dtGriglia = Session("DtbRicVol")

        '---determino cosa è stato checkato nella pag corrente e lo salvo nella datatable di sessione
        For i = 0 To dgRisultatoRicerca.Items.Count - 1
            Mychk = dgRisultatoRicerca.Items.Item(i).FindControl("chkSelProg")
            rwGriglia = dtGriglia.Rows(i + (dgRisultatoRicerca.CurrentPageIndex * 10))
            If Mychk.Checked = True Then
                rwGriglia.Item(7) = 1
            Else
                rwGriglia.Item(7) = 0
            End If
        Next i

        Session("DtbRicVol") = dtGriglia
        If Not IsNothing(e) Then
            dgRisultatoRicerca.CurrentPageIndex = e.NewPageIndex
        End If
        dgRisultatoRicerca.DataSource = Session("DtbRicVol")
        dgRisultatoRicerca.DataBind()

        '---determino cosa c'era text nella datatable di sessione in e lo visualizzo nella pag che vado a caricare
        For i = 0 To dgRisultatoRicerca.Items.Count - 1
            Mychk = dgRisultatoRicerca.Items.Item(i).FindControl("chkSelProg")
            rwGriglia = dtGriglia.Rows(i + (dgRisultatoRicerca.CurrentPageIndex * 10))
            If rwGriglia.Item(7) = "1" Then
                Mychk.Checked = True
            Else
                Mychk.Checked = False
            End If
        Next i
    End Sub



    Private Sub ddlMaccCodAmAtt_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlMaccCodAmAtt.SelectedIndexChanged
        '***in questo evento gestisco il caricamento dinamico delle combo 
        '***a seconda del settore selezionato se non seleziono nulla dalla combo
        '***popolo completamente la combo delle aree di intervento
        If ddlMaccCodAmAtt.SelectedItem.Text <> "" Then
            ddlCodAmAtt.Enabled = True
            ddlCodAmAtt.DataSource = MakeParentTable("select distinct a.idambitoattività," & _
                                                     " a.codifica + ' ' + a.AmbitoAttività as Ambito from ambitiattività a" & _
                                                     " inner join macroambitiattività b" & _
                                                     " on a.IDMacroAmbitoAttività=b.IDMacroAmbitoAttività" & _
                                                     " where a.IDMacroAmbitoAttività=" & ddlMaccCodAmAtt.SelectedValue & " order by 1")
            ddlCodAmAtt.DataTextField = "ParentItem"
            ddlCodAmAtt.DataValueField = "id"
            ddlCodAmAtt.DataBind()
        Else
            'popolo completamente combo aree di intervento
            ddlCodAmAtt.DataSource = Nothing
            ddlCodAmAtt.Items.Add("")
            ddlCodAmAtt.SelectedIndex = 0
            ddlCodAmAtt.Enabled = False
        End If
    End Sub
    Private Function MakeParentTable(ByVal strquery As String) As DataSet
        '***Inizializzo e carico datatable 
        Dim dtrgenerico As Data.SqlClient.SqlDataReader
        ' Create a new DataTable.
        Dim myDataTable As DataTable = New DataTable
        ' Declare variables for DataColumn and DataRow objects.
        Dim myDataColumn As DataColumn
        Dim myDataRow As DataRow
        ' Create new DataColumn, set DataType, ColumnName and add to DataTable.    
        myDataColumn = New DataColumn
        myDataColumn.DataType = System.Type.GetType("System.Int64")
        myDataColumn.ColumnName = "id"
        myDataColumn.Caption = "id"
        myDataColumn.ReadOnly = True
        myDataColumn.Unique = True
        ' Add the Column to the DataColumnCollection.
        myDataTable.Columns.Add(myDataColumn)
        ' Create second column.
        myDataColumn = New DataColumn
        myDataColumn.DataType = System.Type.GetType("System.String")
        myDataColumn.ColumnName = "ParentItem"
        myDataColumn.AutoIncrement = False
        myDataColumn.Caption = "ParentItem"
        myDataColumn.ReadOnly = False
        myDataColumn.Unique = False
        myDataTable.Columns.Add(myDataColumn)
        dtrgenerico = ClsServer.CreaDatareader(strquery, Session("conn"))
        myDataRow = myDataTable.NewRow()
        myDataRow("id") = 0
        myDataRow("ParentItem") = ""
        myDataTable.Rows.Add(myDataRow)
        Do While dtrgenerico.Read
            myDataRow = myDataTable.NewRow()
            myDataRow("id") = dtrgenerico.GetValue(0)
            myDataRow("ParentItem") = dtrgenerico.GetValue(1)
            myDataTable.Rows.Add(myDataRow)
        Loop

        dtrgenerico.Close()
        dtrgenerico = Nothing

        MakeParentTable = New DataSet
        MakeParentTable.Tables.Add(myDataTable)
    End Function

    Private Sub imgEsporta_Click(ByVal sender As Object, ByVal e As EventArgs) Handles imgEsporta.Click
        '*****************************************************************************************+
        'AUTORE: Guido Testa 
        'DATA: 26/04/2006
        'DESCRIZONE: controllo se è stato selezionato almeno un progetto prima di 
        'avviare la maschera di esportazione
        '*****************************************************************************************+
        Dim dtGriglia As DataTable
        Dim i As Integer
        Dim bolVerifica As Boolean
        bolVerifica = False

        Call RiccoraItemSelezionato()

        dtGriglia = Session("DtbRicVol")

        'verifico se è stato selezionato almeno un progetto
        For i = 0 To dtGriglia.Rows.Count - 1
            If dtGriglia.Rows(i).Item(7) = 1 Then
                bolVerifica = True
                Exit For
            End If
        Next



        If bolVerifica = False Then
            lblmessaggio.Text = "Selezionare almeno un progetto dalla lista"
        Else
            EsportaVolontari()
            'lblmessaggio.Text = ""
            'Response.Write("<script>")
            'Response.Write("window.open('WfrmEsportazioneIbanVolontari.aspx', 'Esporta', 'width=300, height=195, status=no, toolbar=no, location=no, menubar=no, scrollbars=no')")
            'Response.Write("</script>")
        End If
    End Sub

    Private Sub EsportaVolontari()
        '*****************************************************************************************+
        'AUTORE: Simona Cordella
        'DATA: 29/09/2009
        'DESCRIZONE: estraggo tutti i volontari relativamente ai progetti precedentemente selezionati

        Dim StrSql As String
        Dim dtrVolontari As Data.SqlClient.SqlDataReader
        Dim dtrEnte As SqlClient.SqlDataReader
        Dim strCodiceEnte As String
        Dim strDenomEnte As String

        Dim Writer As StreamWriter
        Dim xLinea As String
        Dim i As Integer
        Dim NomeUnivoco As String
        Dim xPrefissoNome As String
        Dim dtIbanVolontari As DataTable
        Dim strInCondition As String

        Try

            'vado a vedere quali sono stati i progetti selezionati per usarli come filtro per la selezione dei relativi volontari
            dtIbanVolontari = Session("DtbRicVol")
            i = 0
            For i = 0 To dtIbanVolontari.Rows.Count - 1
                If dtIbanVolontari.Rows(i).Item(7) = 1 Then
                    strInCondition = strInCondition & dtIbanVolontari.Rows(i).Item(5) & ","
                End If
            Next
            strInCondition = Mid(strInCondition, 1, Len(strInCondition) - 1)


            'trova il nome e la denominazione dell'ente in Sessione
            StrSql = "Select CodiceRegione,Denominazione from enti where idente=" & Session("IdEnte") & ""
            dtrEnte = ClsServer.CreaDatareader(StrSql, Session("conn"))
            NomeUnivoco = vbNullString
            If dtrEnte.HasRows = False Then
                dtrEnte.Close()
                dtrEnte = Nothing
            Else
                dtrEnte.Read()
                ' strCodiceEnte = ""
                'strDenomEnte = ""
                If NomeUnivoco = vbNullString Then

                    xPrefissoNome = Session("Utente")
                    NomeUnivoco = xPrefissoNome & "ExpIbanVol" & Year(Now) & Month(Now) & Day(Now) & Hour(Now) & Minute(Now) & Second(Now)
                    Writer = New StreamWriter(Server.MapPath("download") & "\" & NomeUnivoco & ".CSV", True, System.Text.Encoding.Default)
                    '---intestazioni

                    xLinea = "Denominazione Ente:;"
                    ' Writer.WriteLine(xLinea)
                    'Denominazione Ente
                    If IsDBNull(dtrEnte(0)) = True Then
                        xLinea = xLinea & vbNullString & ";;;"
                    Else
                        xLinea = xLinea & ClsUtility.FormatExport(dtrEnte(1)) & ";;;"
                    End If
                    Writer.WriteLine(xLinea)

                    xLinea = vbNullString

                    xLinea = "Codice Ente:;"
                    'Writer.WriteLine(xLinea)
                    'Codice Ente
                    If IsDBNull(dtrEnte(0)) = True Then
                        xLinea = xLinea & vbNullString & ";;;"
                    Else
                        xLinea = xLinea & ClsUtility.FormatExport(dtrEnte(0)) & ";;;"
                    End If
                    Writer.WriteLine(xLinea)
                End If
            End If
            xLinea = vbNullString

            dtrEnte.Close()
            dtrEnte = Nothing


            StrSql = "SELECT distinct  " & _
                     "entità.CodiceVolontario, " & _
                     "isnull(replace(replace(replace(replace(replace(replace(replace(entità.Cognome,'°',''),'ì','i'''),'é','e'''),'à','a'''),'ò','o'''),'è','e'''),'ù','u'''), '') as Cognome, " & _
                     "isnull(replace(replace(replace(replace(replace(replace(replace(entità.Nome,'°',''),'ì','i'''),'é','e'''),'à','a'''),'ò','o'''),'è','e'''),'ù','u'''), '') as Nome, " & _
                     "entità.IBAN,entità.BIC_SWIFT " & _
                     "FROM entità " & _
                     "INNER JOIN attivitàentità ON entità.IDEntità = attivitàentità.IDEntità " & _
                     "INNER JOIN attivitàentisediattuazione ON attivitàentità.IDAttivitàEnteSedeAttuazione = attivitàentisediattuazione.IDAttivitàEnteSedeAttuazione " & _
                     "INNER JOIN attività ON attivitàentisediattuazione.IDAttività = attività.IDAttività " & _
                     "INNER JOIN entisediattuazioni ON attivitàentisediattuazione.IDEnteSedeAttuazione = entisediattuazioni.IDEnteSedeAttuazione " & _
                     "INNER JOIN enti ON attività.IDEntePresentante = enti.IDEnte " & _
                     "INNER JOIN StatiEntità ON Entità.IdStatoEntità = Statientità.IdStatoEntità " & _
                     "INNER JOIN TipiProgetto ON  TipiProgetto.IdTipoProgetto =  Attività.IdTipoProgetto " & _
                     "WHERE (StatiEntità.InServizio = 1 OR StatiEntità.Sospeso = 1 OR StatiEntità.Chiuso = 1) AND attivitàentità.EscludiFormazione=0 " & _
                     "AND ((Entità.DataInizioServizio >= '01/12/2009' and tipiprogetto.nazionebase = 1) or (Entità.DataInizioServizio >= '16/11/2009' and tipiprogetto.nazionebase = 0))"

            If Session("TipoUtente") = "E" Then
                StrSql = StrSql & "AND Enti.IdEnte = " & Session("IdEnte") & " "
            End If

            StrSql = StrSql & " AND attività.IDAttività in (" & strInCondition & ")"

            StrSql = StrSql & " Order by  isnull(replace(replace(replace(replace(replace(replace(replace(entità.Cognome,'°',''),'ì','i'''),'é','e'''),'à','a'''),'ò','o'''),'è','e'''),'ù','u'''), ''), isnull(replace(replace(replace(replace(replace(replace(replace(entità.Nome,'°',''),'ì','i'''),'é','e'''),'à','a'''),'ò','o'''),'è','e'''),'ù','u'''), '')"

            dtrVolontari = ClsServer.CreaDatareader(StrSql, Session("conn"))

            'NomeUnivoco = vbNullString
            Dim bnlI As Boolean = False

            If dtrVolontari.HasRows = False Then
                lblmessaggio.Text = lblmessaggio.Text & "Nessun Volontario."
            Else
                While dtrVolontari.Read
                    If bnlI = False Then
                        ' xPrefissoNome = Session("Utente")
                        ' NomeUnivoco = xPrefissoNome & "ExpIbanVol" & Year(Now) & Month(Now) & Day(Now) & Hour(Now) & Minute(Now) & Second(Now)
                        'Writer = New StreamWriter(Server.MapPath("download") & "\" & NomeUnivoco & ".CSV")
                        '---intestazioni
                        xLinea = "CODICE VOLONTARIO;COGNOME;NOME;CODICE IBAN;CODICE BIC/SWIFT"
                        Writer.WriteLine(xLinea)
                        bnlI = True
                    End If
                    xLinea = vbNullString

                    '---salto il primo elemento (nome file)
                    'codice volontario
                    If IsDBNull(dtrVolontari(0)) = True Then
                        xLinea = vbNullString & ";"
                    Else
                        xLinea = ClsUtility.FormatExport(dtrVolontari(0)) & ";"
                    End If
                    'cognome
                    If IsDBNull(dtrVolontari(1)) = True Then
                        xLinea = xLinea & vbNullString & ";"
                    Else
                        xLinea = xLinea & ClsUtility.FormatExport(dtrVolontari(1)) & ";"
                    End If
                    'nome
                    If IsDBNull(dtrVolontari(2)) = True Then
                        xLinea = xLinea & vbNullString & ";"
                    Else
                        xLinea = xLinea & ClsUtility.FormatExport(dtrVolontari(2)) & ";"
                    End If
                    'IBAN
                    If IsDBNull(dtrVolontari(3)) = True Then
                        xLinea = xLinea & vbNullString & ";"
                    Else
                        xLinea = xLinea & ClsUtility.FormatExport(dtrVolontari(3)) & ";"
                    End If
                    'BIC_SWIFT
                    If IsDBNull(dtrVolontari(4)) = True Then
                        xLinea = xLinea & vbNullString & ""
                    Else
                        xLinea = xLinea & ClsUtility.FormatExport(dtrVolontari(4)) & ""
                    End If
                    Writer.WriteLine(xLinea)

                End While

                hlVolontari.Visible = True
                hlVolontari.NavigateUrl = "download\" & NomeUnivoco & ".CSV"
                Writer.Close()
                Writer = Nothing
            End If
            dtrVolontari.Close()
            dtrVolontari = Nothing
        Catch ex As Exception
            lblmessaggio.Text = lblmessaggio.Text & "Errore durante l'esportazione dei Volontari."
            If Not Writer Is Nothing Then
                Writer.Close()
                Writer = Nothing
            End If
            If Not dtrVolontari Is Nothing Then
                dtrVolontari.Close()
                dtrVolontari = Nothing
            End If
        End Try
    End Sub




    Protected Sub CmdChiudi_Click1(ByVal sender As Object, ByVal e As EventArgs) Handles CmdChiudi.Click
        If Request.QueryString("VengoDa") = Nothing Then
            Response.Redirect("wfrmImportLibrettiUNSC.aspx")
        Else
            Response.Redirect("wfrmImportIbanEnti.aspx")
        End If

    End Sub

    Protected Sub chkSelDesel_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles chkSelDesel.CheckedChanged

        Dim i As Integer
        Dim Mychk As CheckBox
        Dim dtGriglia As DataTable 'appoggio
        Dim rwGriglia As DataRow

        dtGriglia = Session("DtbRicVol")
        hlVolontari.Visible = False
        '---determino cosa è stato checkato nella pag corrente e lo salvo nella datatable di sessione
        For i = 0 To dgRisultatoRicerca.Items.Count - 1
            Mychk = dgRisultatoRicerca.Items.Item(i).FindControl("chkSelProg")
            
            Mychk.Checked = chkSelDesel.Checked

        Next i
        If (chkSelDesel.Checked) Then
            chkSelDesel.Text = "Deseleziona Tutto"
        Else
            chkSelDesel.Text = "Seleziona Tutto"
        End If

    End Sub




    


End Class