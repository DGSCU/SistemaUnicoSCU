Imports System.Drawing.Printing
Imports System.Drawing.Imaging
Imports System.Web.UI
Imports System.Drawing
Imports System.IO
Public Class ricercaprogetti
    Inherits System.Web.UI.Page
    Private WithEvents pndocument As PrintDocument
    Dim dtrGenerico As SqlClient.SqlDataReader
    Dim strquery As String

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        '***Generata da Gianluigi Paesani in data:05/07/04
        'Inserire qui il codice utente necessario per inizializzare la pagina
        If Not Session("LogIn") Is Nothing Then
            If Session("LogIn") = False Then 'verifico validità log-in
                Response.Redirect("LogOn.aspx")
            End If
        Else
            Response.Redirect("LogOn.aspx")
        End If
        If ClsUtility.ForzaPresenzaSanzioneProgetto(Session("Utente"), Session("conn")) = False Then
            dgRisultatoRicerca.Columns(16).Visible = False
            ddlSegnalazioneSanzione.Visible = False
            lblSanzione.Visible = False
        End If

        If IsPostBack = False Then

            CaricaCompetenze()

            'scarico la session della datatable per la ricerca così che in una nuova pagina non verrà 
            'erroneamente visualizzato alcun item
            Session("DtbRicerca") = Nothing
            CmdEsporta.Visible = False

            'richiamo sub dove popolo combo
            CaricaPrima()

            'controllo provenienza chiamata
            If Request.QueryString("VengoDa") = "Valutare" Then
                lblDenEnte.Text = "Den. Ente"
            Else
                lblDenEnte.Visible = False
                txtDenominazioneEnte.Visible = False
                If Request.QueryString("VengoDa") <> "Valutare" Then
                    'txtCodReg.Text = Mid(Session("CodiceRegioneEnte"), 2, 7)
                    txtCodReg.Text = Session("txtCodEnte")
                End If
            End If
            If Not IsNothing(Request.QueryString("Bando")) Then
                ddlStatoAttivita.SelectedIndex = -1
                'txtCodReg.Text = Mid(Session("CodiceRegioneEnte"), 2, 7)
                txtCodReg.Text = Session("txtCodEnte")
                EseguiRicerca(0)
            End If
        Else
            CmdEsporta.Visible = True
        End If
        If UCase(Request.QueryString("esporta")) = "SI" Then
            CmdEsporta.Visible = False
        End If

        'Modifica fatta da AMILCARE PAOLELLA in Data: 10/08/05 **********
        'Controllo sul tipo di utente
        If Session("TipoUtente") = "E" Then
            'Disabilito la possibilità di selezionare la denominazione dell'ente
            lblDenEnte.Visible = False
            txtDenominazioneEnte.Visible = False
            lblCodReg.Visible = False
            txtCodReg.Visible = False
            'txtcomune.Visible = False
            'lblComune.Visible = False
            'txtProvincia.Visible = False
            'lblProvncia.Visible = False
            'txtRegione.Visible = False
            'lblRegione.Visible = False
        Else
            lblDenEnte.Visible = True
            txtDenominazioneEnte.Visible = True
            lblCodReg.Visible = True
            txtCodReg.Visible = True
        End If
        'aggiunto il 05/05/2014 da simona cordella
        If ClsUtility.ForzaStatoValutazione(Session("Utente"), Session("conn")) Then
            LblStatoValutazione.Visible = True
            ddlStatoValutazione.Visible = True
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

                strSQL = "select IdRegioneCompetenza,Descrizione,CodiceRegioneCompetenza,left(CodiceRegioneCompetenza,1)from RegioniCompetenze where IdRegioneCompetenza <> 22 "
                strSQL = strSQL & " union "
                strSQL = strSQL & " select '0',' TUTTI ','','A' "
                strSQL = strSQL & " union "
                strSQL = strSQL & " select '-1',' NAZIONALE ','','B' "
                strSQL = strSQL & " union "
                strSQL = strSQL & " select '-2',' REGIONALE ','','C' "
                strSQL = strSQL & " union "
                strSQL = strSQL & " select '-3',' NON DEFINITO ','','D' "
                strSQL = strSQL & "  from RegioniCompetenze order by left(CodiceRegioneCompetenza,1),descrizione "
                'chiudo il datareader se aperto
                If Not dtrCompetenze Is Nothing Then
                    dtrCompetenze.Close()
                    dtrCompetenze = Nothing
                End If

                'eseguo la query
                dtrCompetenze = ClsServer.CreaDatareader(strSQL, Session("conn"))
                'assegno il datadearder alla combo caricando così descrizione e id
                CboCompetenza.DataSource = dtrCompetenze
                CboCompetenza.Items.Add("")
                CboCompetenza.DataTextField = "Descrizione"
                CboCompetenza.DataValueField = "IDRegioneCompetenza"
                CboCompetenza.DataBind()
                'chiudo il datareader se aperto
                If Not dtrCompetenze Is Nothing Then
                    dtrCompetenze.Close()
                    dtrCompetenze = Nothing
                End If
            End If

            'Controllo abilitazione scelta
            If Session("TipoUtente") = "U" Then
                CboCompetenza.Enabled = True
                CboCompetenza.SelectedIndex = 0
                Session("IdRegCompetenza") = 22
            Else
                If Session("TipoUtente") = "R" Then
                    CboCompetenza.Enabled = False
                End If
                'CboCompetenza.SelectedIndex = 1
                'CboCompetenza.Enabled = False
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
                    CboCompetenza.SelectedValue = dtrCompetenze("IdRegioneCompetenza")
                    Session("IdRegCompetenza") = dtrCompetenze("IdRegioneCompetenza")
                    If dtrCompetenze("Heliosread") = True Then
                        CboCompetenza.Enabled = True
                    End If
                End If

                'If Session("TipoUtente") = "R" Then
                '    CboCompetenza.Enabled = False
                'End If

            End If

        Catch ex As Exception
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

    Private Sub cmdChiudi_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles CmdChiudi.Click
        '***Generata da Gianluigi Paesani in data:05/07/04
        'chiamo homepage
        Response.Redirect("WfrmMain.aspx")
    End Sub

    Private Sub CaricaPrima()
        '***Generata da Gianluigi Paesani in data:05/07/04
        '***Carico combo settore
        ddlMaccCodAmAtt.DataSource = MakeParentTable("select idmacroambitoattività, codifica + ' - ' + MacroAmbitoAttività as Macro from macroambitiattività")
        ddlMaccCodAmAtt.DataTextField = "ParentItem"
        ddlMaccCodAmAtt.DataValueField = "id"
        ddlMaccCodAmAtt.DataBind()

        ''***Carico combo area intervento
        'ddlCodAmAtt.Items.Add("")
        'ddlCodAmAtt.Enabled = False

        '***carico combo stati attivita
        ddlStatoAttivita.DataSource = MakeParentTable("select idstatoattività, statoattività from statiattività")
        ddlStatoAttivita.DataTextField = "ParentItem"
        ddlStatoAttivita.DataValueField = "id"
        ddlStatoAttivita.DataBind()

        '*****Carico Combo Bandi
        'modificato da s.c. il 03/12/2014 controllo filtro visibilità
        Dim strsql As String
        strsql = "SELECT DISTINCT Bando.idBando,bando.bandobreve, bando.annobreve"
        strsql = strsql & " FROM bando"
        strsql = strsql & " INNER JOIN AssociaBandoTipiProgetto abtp on abtp.idbando =  bando.idbando"
        strsql = strsql & " INNER JOIN TipiProgetto  tp on abtp.idtipoprogetto = tp.idtipoprogetto"
        strsql = strsql & " WHERE tp.MacroTipoProgetto like '" & Session("FiltroVisibilita") & "'"
        strsql = strsql & " ORDER BY bando.annobreve desc"
        '"select idbando, bandobreve from Bando order by annobreve desc"
        DdlBando.DataSource = MakeParentTable(strsql)
        DdlBando.DataTextField = "ParentItem"
        DdlBando.DataValueField = "id"
        DdlBando.DataBind()

        If Request.QueryString("VengoDa") = "Valutare" Then
            DdlBando.SelectedValue = Request.QueryString("IdBando")
        End If

        'visualizzo valore predefinito
        If Request.QueryString("VengoDa") = "Valutare" Then
            Dim dtrgenerico As Data.SqlClient.SqlDataReader
            dtrgenerico = ClsServer.CreaDatareader("select idstatoattività from statiattività where daValutare=1", Session("conn"))
            Do While dtrgenerico.Read
                'posizione combo stato su item predefinito
                ddlStatoAttivita.SelectedValue = dtrgenerico.GetValue(0)
            Loop
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If

        If Session("VisualizzaStatoProgetti") = False Then
            ddlStatoAttivita.Enabled = False
        End If

        '** mod. da s.c. il 27/11/2014 FiltroVisibilita
        DdlTipiProgetto.DataSource = ClsServer.CreaDataTable("Select idTipoProgetto,Descrizione from TipiProgetto where MacroTipoProgetto like '" & Session("FiltroVisibilita") & "' ", True, Session("conn"))
        DdlTipiProgetto.DataTextField = "Descrizione"
        DdlTipiProgetto.DataValueField = "idTipoProgetto"
        DdlTipiProgetto.DataBind()

        CaricaComboDurata()
    End Sub

    Private Function MakeParentTable(ByVal strquery As String) As DataSet
        '***Generata da Gianluigi Paesani in data:05/07/04
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
        ' Add the column to the table.
        myDataTable.Columns.Add(myDataColumn)
        ' Make the ID column the primary key column. da verificare?????????
        'Dim PrimaryKeyColumns(0) As DataColumn
        'PrimaryKeyColumns(0) = myDataTable.Columns("id"))
        'myDataTable.PrimaryKey = PrimaryKeyColumns)
        dtrgenerico = ClsServer.CreaDatareader(strquery, Session("conn"))
        'Instantiate the DataSet variable.
        'mydataset = New DataSet
        ' Add the new DataTable to the DataSet.
        'mydataset.Tables.Add(myDataTable)
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


    Private Sub EseguiRicerca(ByVal bytVerifica As Byte, Optional ByVal bytpage As Integer = 0)
        '***Generata da Gianluigi Paesani in data:05/07/04
        '***Eseguo comando di ricerca
        Dim Mydataset As New DataSet
        'controllo se la chiamata viene effettuata dal link di pagina o dal pulsante ricerca
        If bytVerifica = 1 Then dgRisultatoRicerca.CurrentPageIndex = bytpage
        dgRisultatoRicerca.Visible = True

        If Request.QueryString("VengoDa") = "Valutare" Then
            '(isNull(a.NumeroPostiNoVittoNoAlloggio,0)+ isnull(a.NumeroPostiVittoAlloggio,0) + isNull(a.NumeroPostiVitto,0)) as VolRic,
            strquery = "select distinct b.denominazione, a.titolo, c.bando," & _
            " g.macroambitoattività + ' / ' + f.ambitoattività as Ambito," & _
            " convert(varchar,isnull(a.dataultimostato,''),3) as Data,a.idattività,b.idente, 0 as modificabile, a.IdTipoProgetto, "
            If Session("VisualizzaStatoProgetti") = True Then
                strquery = strquery & "e.statoattività , "
            Else
                'controllo il bit VisualizzaStato su Bando per nascondere o meno lo stato del progetto di bandi in valutazione
                strquery = strquery & "case isnull(c.VisualizzaStato,1) when 1 then e.statoattività else '' end as statoattività, "
            End If
            'a.SegnalazioneSanzione as SegnalazioneSanzione"
            strquery = strquery & " YEAR(a.DataUltimoStato) AS Expr1, MONTH(a.DataUltimoStato) AS Expr2, DAY(a.DataUltimoStato) AS Expr3, case when a.idattivitàcapofila is null then isnull(a.codiceente,'') else isnull(a.codiceente,'') + '(SUB-PROGETTO)' end as codiceente,RegioniCompetenze.Descrizione as RegioneCompetenza,"
            strquery = strquery & " Case isnull(a.SegnalazioneSanzione,0) When 0 then 'No' When 1 then '<img src=images/Anomalie.bmp alt=Sanzioni onclick=VisualizzaSanzioneProg('+ convert(varchar, a.IDAttività) + ','+ convert(varchar, a.IDEntePresentante) + ') STYLE=cursor:hand title=Sanzione border=0>' End as SegnalazioneSanzione, a.nmesi, "
            strquery = strquery & " CASE a.Fami WHEN 1 THEN  convert(varchar,(isNull(a.NumeroPostiNoVittoNoAlloggio,0)+ isnull(a.NumeroPostiVittoAlloggio,0) + isNull(a.NumeroPostiVitto,0))) + ' (Fami:' + convert(varchar,isnull(a.NumeroPostifami,0)) + ')' "
            strquery = strquery & "	ELSE convert(varchar,(isNull(a.NumeroPostiNoVittoNoAlloggio,0)+ isnull(a.NumeroPostiVittoAlloggio,0) + isNull(a.NumeroPostiVitto,0))) end  VolRic,entireferenti.CodiceRegione,l.titolo as TitoloProgramma,l.CodiceProgramma  "
            strquery = strquery & " FROM entisediattuazioni INNER JOIN"
            strquery = strquery & " attivitàentisediattuazione ON entisediattuazioni.IDEnteSedeAttuazione = attivitàentisediattuazione.IDEnteSedeAttuazione INNER JOIN"
            strquery = strquery & " entisedi ON entisediattuazioni.IDEnteSede = entisedi.IDEnteSede INNER JOIN"
            strquery = strquery & " comuni ON entisedi.IDComune = comuni.IDComune INNER JOIN"
            strquery = strquery & " provincie ON comuni.IDProvincia = provincie.IDProvincia INNER JOIN"
            strquery = strquery & " regioni ON provincie.IDRegione = regioni.IDRegione INNER JOIN"
            strquery = strquery & " enti enti_1 ON entisedi.IDEnte = enti_1.IDEnte RIGHT OUTER JOIN"
            strquery = strquery & " attività a INNER JOIN"
            strquery = strquery & " enti b ON a.IDEntePresentante = b.IDEnte LEFT OUTER JOIN"
            strquery = strquery & " enti entireferenti ON a.IDEnteReferenteProgramma = entireferenti.IDEnte LEFT OUTER JOIN "
            strquery = strquery & " ATTIVITàENTICOPROGETTAZIONE AEC ON A.IDATTIVITà = AEC.IDATTIVITà LEFT OUTER JOIN"
            strquery = strquery & " BandiAttività h ON a.IDBandoAttività = h.IdBandoAttività LEFT OUTER JOIN"
            strquery = strquery & " bando c ON c.IDBando = h.IdBando "
            strquery = strquery & " LEFT OUTER JOIN RegioniCompetenze ON a.IdRegioneCompetenza=RegioniCompetenze.IdRegioneCompetenza "
            strquery = strquery & " INNER JOIN TipiProgetto ON a.IdTipoProgetto = TipiProgetto.IdTipoProgetto "
            strquery = strquery & " INNER JOIN AssociaProfiliTipiProgetto ON TipiProgetto.IdTipoProgetto = AssociaProfiliTipiProgetto.IdTipoProgetto "
            strquery = strquery & " INNER JOIN Profili ON AssociaProfiliTipiProgetto.IdProfilo = Profili.IdProfilo "
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
            strquery = strquery & " LEFT JOIN BANDORICORSI ON a.IDBANDORICORSO = BANDORICORSI.IDBANDORICORSO "
            strquery = strquery & " inner join statibando d on c.idstatobando=d.idstatobando INNER JOIN"
            strquery = strquery & " statiattività e ON a.IDStatoAttività = e.IDStatoAttività INNER JOIN"
            strquery = strquery & " ambitiattività f ON f.IDAmbitoAttività = a.IDAmbitoAttività INNER JOIN"
            strquery = strquery & " macroambitiattività g ON f.IDMacroAmbitoAttività = g.IDMacroAmbitoAttività ON attivitàentisediattuazione.IDAttività = a.IDAttività LEFT OUTER JOIN"
            strquery = strquery & " statiBandiAttività i ON h.IdStatoBandoAttività = i.IdStatoBandoAttività"
            strquery = strquery & " LEFT JOIN PROGRAMMI l ON a.idprogramma = l.idprogramma"
            strquery = strquery & " where a.idattività is not null"
            If Session("TipoUtente") = "E" Then
                strquery = strquery & " and CASE ISNULL(BANDORICORSI.IDBANDORICORSO,0) WHEN 0 THEN isnull(c.enteabilitato,1) ELSE isnull(BANDORICORSI.enteabilitato,1) END = 1"
            End If
        Else
            '(isNull(a.NumeroPostiNoVittoNoAlloggio,0)+ isnull(a.NumeroPostiVittoAlloggio,0) + isNull(a.NumeroPostiVitto,0)) as VolRic,
            strquery = "select distinct b.denominazione, a.titolo, c.bando," & _
            " g.macroambitoattività + ' / ' + f.ambitoattività as Ambito," & _
            " convert(varchar,isnull(a.dataultimostato,''),3) as Data,a.idattività,b.idente," & _
            " convert(tinyint,isnull(e.defaultstato,0)) + convert(tinyint,isnull(i.defaultstato,0)) modificabile, a.IdTipoProgetto, "
            If Session("VisualizzaStatoProgetti") = True Then
                strquery = strquery & "e.statoattività , "
            Else
                'controllo il bit VisualizzaStato su Bando per nascondere o meno lo stato del progetto di bandi in valutazione
                strquery = strquery & "case isnull(c.VisualizzaStato,1) when 1 then e.statoattività else '' end as statoattività, "
            End If
            strquery = strquery & " YEAR(a.DataUltimoStato) AS Expr1, MONTH(a.DataUltimoStato) AS Expr2, DAY(a.DataUltimoStato) AS Expr3, case when a.idattivitàcapofila is null then isnull(a.codiceente,'') else isnull(a.codiceente,'') + '(SUB-PROGETTO)' end as codiceente, RegioniCompetenze.Descrizione as RegioneCompetenza,"
            strquery = strquery & " Case isnull(a.SegnalazioneSanzione,0) When 0 then 'No' When 1 then '<img src=images/Anomalie.bmp alt=Sanzioni onclick=VisualizzaSanzioneProg('+ convert(varchar, a.IDAttività) + ','+ convert(varchar, a.IDEntePresentante) + ') STYLE=cursor:hand title=Sanzione border=0>' End as SegnalazioneSanzione, a.nmesi, "
            strquery = strquery & " CASE a.Fami WHEN 1 THEN  convert(varchar,(isNull(a.NumeroPostiNoVittoNoAlloggio,0)+ isnull(a.NumeroPostiVittoAlloggio,0) + isNull(a.NumeroPostiVitto,0))) + ' (Fami:' + convert(varchar,isnull(a.NumeroPostifami,0)) + ')' "
            strquery = strquery & "	ELSE convert(varchar,(isNull(a.NumeroPostiNoVittoNoAlloggio,0)+ isnull(a.NumeroPostiVittoAlloggio,0) + isNull(a.NumeroPostiVitto,0))) end  VolRic,entireferenti.CodiceRegione,l.titolo as TitoloProgramma,l.CodiceProgramma  "
            strquery = strquery & " FROM entisediattuazioni INNER JOIN"
            strquery = strquery & " attivitàentisediattuazione ON entisediattuazioni.IDEnteSedeAttuazione = attivitàentisediattuazione.IDEnteSedeAttuazione INNER JOIN"
            strquery = strquery & " entisedi ON entisediattuazioni.IDEnteSede = entisedi.IDEnteSede INNER JOIN"
            strquery = strquery & " comuni ON entisedi.IDComune = comuni.IDComune INNER JOIN"
            strquery = strquery & " provincie ON comuni.IDProvincia = provincie.IDProvincia INNER JOIN"
            strquery = strquery & " regioni ON provincie.IDRegione = regioni.IDRegione INNER JOIN"
            strquery = strquery & " enti enti_1 ON entisedi.IDEnte = enti_1.IDEnte RIGHT OUTER JOIN"
            strquery = strquery & " attività a INNER JOIN"
            strquery = strquery & " enti b ON a.IDEntePresentante = b.IDEnte LEFT OUTER JOIN "
            strquery = strquery & " enti entireferenti ON a.IDEnteReferenteProgramma = entireferenti.IDEnte LEFT OUTER JOIN "
            strquery = strquery & " ATTIVITàENTICOPROGETTAZIONE AEC ON A.IDATTIVITà = AEC.IDATTIVITà LEFT OUTER JOIN"
            strquery = strquery & " BandiAttività h ON a.IDBandoAttività = h.IdBandoAttività LEFT OUTER JOIN"
            strquery = strquery & " bando c ON c.IDBando = h.IdBando "
            strquery = strquery & " LEFT OUTER JOIN RegioniCompetenze ON a.IdRegioneCompetenza=RegioniCompetenze.IdRegioneCompetenza "
            strquery = strquery & " INNER JOIN TipiProgetto ON a.IdTipoProgetto = TipiProgetto.IdTipoProgetto "
            strquery = strquery & " INNER JOIN AssociaProfiliTipiProgetto ON TipiProgetto.IdTipoProgetto = AssociaProfiliTipiProgetto.IdTipoProgetto "
            strquery = strquery & " INNER JOIN Profili ON AssociaProfiliTipiProgetto.IdProfilo = Profili.IdProfilo "
            '============================================================================================================================
            '====================================================30/09/2008==============================================================
            '=======MODIFICA EFFETTUATA DA Kronk (99 scimmie saltavano sul letto, una cadde in terra e si ruppe il cervelletto...)=======
            '=================AGGIUNTA JOIN SU PROFILO READ PER ABILITARE LEGGERE LE ABILITAZIONI ANCHE DELL'UTENTE READ=================
            '============================================================================================================================
            '

            If Session("Read") <> "1" Then
                strquery = strquery & " INNER JOIN	AssociaUtenteGruppo ON Profili.IdProfilo = AssociaUtenteGruppo.IdProfilo "
            Else
                strquery = strquery & " INNER JOIN	AssociaUtenteGruppo ON Profili.IdProfilo = AssociaUtenteGruppo.IdProfiloRead "
            End If
            strquery = strquery & " LEFT JOIN BANDORICORSI ON a.IDBANDORICORSO = BANDORICORSI.IDBANDORICORSO "
            strquery = strquery & " INNER JOIN"
            strquery = strquery & " statiattività e ON a.IDStatoAttività = e.IDStatoAttività INNER JOIN"
            strquery = strquery & " ambitiattività f ON f.IDAmbitoAttività = a.IDAmbitoAttività INNER JOIN"
            strquery = strquery & " macroambitiattività g ON f.IDMacroAmbitoAttività = g.IDMacroAmbitoAttività ON attivitàentisediattuazione.IDAttività = a.IDAttività LEFT OUTER JOIN"
            strquery = strquery & " statiBandiAttività i ON h.IdStatoBandoAttività = i.IdStatoBandoAttività"
            strquery = strquery & " LEFT JOIN PROGRAMMI l ON a.idprogramma = l.idprogramma"
            strquery = strquery & " where a.idattività is not null"
            If Session("TipoUtente") = "E" Then
                strquery = strquery & " and CASE ISNULL(BANDORICORSI.IDBANDORICORSO,0) WHEN 0 THEN isnull(c.enteabilitato,1) ELSE isnull(BANDORICORSI.enteabilitato,1) END = 1"
            End If
        End If

        'imposto eventuali parametri
        If ddlStatoAttivita.SelectedItem.Text <> "" Then
            strquery = strquery & " and e.idstatoattività=" & ddlStatoAttivita.SelectedValue & ""
        End If

        If DdlTipiProgetto.SelectedItem.Text <> "" Then
            strquery = strquery & " and a.idTipoProgetto=" & DdlTipiProgetto.SelectedValue & ""
        End If

        If txtTitoloProgetto.Text <> "" Then
            strquery = strquery & " and a.titolo like '" & ClsServer.NoApice(Trim(txtTitoloProgetto.Text)) & "%'"
        End If

        If DdlBando.SelectedItem.Text <> "" Then
            strquery = strquery & " and c.bandobreve = '" & ClsServer.NoApice(DdlBando.SelectedItem.Text) & "'"
        End If

        If CStr(Session("TipoUtente")) = "E" Then
            strquery = strquery & " and (b.idente = '" & CStr(Session("IdEnte")) & "' OR AEC.IDENTE = '" & CStr(Session("IdEnte")) & "')"
        End If

        If txtDenominazioneEnte.Text <> "" Then
            strquery = strquery & " and b.denominazione like '" & ClsServer.NoApice(Trim(txtDenominazioneEnte.Text)) & "%'"
        End If

        If CStr(Session("TipoUtente")) <> "E" Then
            If txtCodReg.Text <> "" Then
                strquery = strquery & " and b.CodiceRegione like '" & ClsServer.NoApice(txtCodReg.Text) & "%'"
            End If
        End If

        If txtDenominazioneEnteSecondario.Text <> "" Then
            strquery = strquery & " and enti_1.denominazione like '" & ClsServer.NoApice(Trim(txtDenominazioneEnteSecondario.Text)) & "%'"
        End If

        If txtRegione.Text <> "" Then
            strquery = strquery & " and regioni.regione  like '" & ClsServer.NoApice(Trim(txtRegione.Text)) & "%'"
        End If

        If txtProvincia.Text <> "" Then
            strquery = strquery & " and provincie.provincia  like '" & ClsServer.NoApice(Trim(txtProvincia.Text)) & "%'"
        End If

        If txtcomune.Text <> "" Then
            strquery = strquery & " and comuni.denominazione  like '" & ClsServer.NoApice(Trim(txtcomune.Text)) & "%'"
        End If


        If TxtCodPog.Text <> "" Then
            strquery = strquery & " and a.CodiceEnte  like '" & ClsServer.NoApice(Trim(TxtCodPog.Text)) & "%'"
        End If
        'adc------------------------------------------------------------------
        If txtTitoloProgramma.Text <> "" Then
            strquery = strquery & " and l.titolo like '" & ClsServer.NoApice(Trim(txtTitoloProgramma.Text)) & "%'"
        End If
        If TxtCodProgramma.Text <> "" Then
            strquery = strquery & " and l.CodiceProgramma  like '" & ClsServer.NoApice(Trim(TxtCodProgramma.Text)) & "%'"
        End If
        '----------------------------------------------------------------------
        If txtProgramma.Text <> "" Then
            strquery = strquery & " and entireferenti.CodiceRegione  = '" & ClsServer.NoApice(Trim(txtProgramma.Text)) & "'"
        End If
        Dim myidcompetenzaselezionato As Integer

        If CboCompetenza.SelectedValue <> "" Then
            myidcompetenzaselezionato = CboCompetenza.SelectedValue
            Select Case CboCompetenza.SelectedValue
                Case 0
                    strquery = strquery & " "
                Case -1
                    strquery = strquery & " And a.IdRegioneCompetenza = 22"
                Case -2
                    strquery = strquery & " And a.IdRegioneCompetenza <> 22 And not a.IdRegioneCompetenza is null "
                Case -3
                    strquery = strquery & " And a.IdRegioneCompetenza is null "
                Case Else
                    strquery = strquery & " And a.IdRegioneCompetenza = " & CboCompetenza.SelectedValue
            End Select
        Else
            myidcompetenzaselezionato = 0
        End If
        If Session("TipoUtente") = "R" And CboCompetenza.Enabled = True And myidcompetenzaselezionato <> Session("IdRegCompetenza") Then
            strquery = strquery & " and (tipiprogetto.scheda = 'S20' or a.IdRegioneCompetenza = " & Session("IdRegCompetenza") & ") " 'aggiungere condizione se competenza selezionata <> competenza utente
        End If


        'If ddlMaccCodAmAtt.SelectedItem.Text = "" And ddlCodAmAtt.SelectedItem.Text = "" Then

        'Else
        If ddlMaccCodAmAtt.SelectedItem.Text <> "" Then
            '    strquery = strquery & " and f.idambitoattività=" & ddlCodAmAtt.SelectedValue & ""
            'Else
            strquery = strquery & " and f.idmacroambitoattività=" & ddlMaccCodAmAtt.SelectedValue & ""
        End If
        'End If
        'aggiunto da simona cordella il 03/08/2017
        If ddlDurataProgetto.SelectedValue <> 0 Then
            strquery = strquery & " and a.nmesi =" & ddlDurataProgetto.SelectedValue
        End If

        'Aggiunto il 15/12/2011 SANDOKAN
        If ddlSegnalazioneSanzione.SelectedItem.Text <> "Tutti" Then
            strquery = strquery & " and isnull(a.SegnalazioneSanzione,0) =" & ddlSegnalazioneSanzione.SelectedValue
        End If
        'Aggiunto il 05/05/2014 da SIMONA CORDELLA
        If ddlStatoValutazione.SelectedItem.Text <> "Tutti" Then
            strquery = strquery & " and isnull(a.StatoValutazione,0) =" & ddlStatoValutazione.SelectedValue
        End If

        'a.GiovaniMinoriOpportunità, a.Fami, a.NumeroPostiFami
        If ddlGiovaniMinoriOp.SelectedItem.Text <> "Tutti" Then
            strquery = strquery & " and isnull(a.GiovaniMinoriOpportunità,0) = " & ddlGiovaniMinoriOp.SelectedValue
        End If
        If ddlFami.SelectedItem.Text <> "Tutti" Then
            strquery = strquery & " and isnull(a.Fami,0) = " & ddlFami.SelectedValue
        End If
        '** agg. da s.c. il 27/11/2014 FiltroVisibilita
        strquery = strquery & " and TipiProgetto.MacroTipoProgetto like '" & Session("FiltroVisibilita") & "' "

        strquery = strquery & " and AssociaUtenteGruppo.username='" & Replace(Session("Utente"), "'", "''") & "'  order by year(a.dataultimostato)desc,month(a.dataultimostato)desc,day(a.dataultimostato)desc,1,2"


        Mydataset = ClsServer.DataSetGenerico(strquery, Session("conn"))

        dgRisultatoRicerca.Columns(16).Visible = ClsUtility.ForzaPresenzaSanzioneProgetto(Session("Utente"), Session("conn"))
        dgRisultatoRicerca.DataSource = Mydataset
        dgRisultatoRicerca.DataBind() 'valorizzo griglia

        '*********************************************************************************
        'blocco per la creazione della datatable per la stampa della ricerca

        'nome e posizione di lettura delle colopnne a base 0
        Dim NomeColonne(11) As String
        Dim NomiCampiColonne(11) As String
        'nome della colonna 
        'e posizione nella griglia di lettura
        NomeColonne(0) = "Codice Progetto"
        NomeColonne(1) = "Denominazione"
        NomeColonne(2) = "Titolo"
        NomeColonne(3) = "Bando"
        NomeColonne(4) = "Settore/Area Intervento"
        'NomeColonne(4) = "Data Ultimo Stato"
        NomeColonne(5) = "Stato Progetto"
        NomeColonne(6) = "Volontari Concessi"
        NomeColonne(7) = "Competenza"
        NomeColonne(8) = "Durata (mesi)"
        NomeColonne(9) = "Titolo Programma"
        NomeColonne(10) = "Codice Programma"
        NomeColonne(11) = "Ente Referente Programma"

        NomiCampiColonne(0) = "codiceente"
        NomiCampiColonne(1) = "denominazione"
        NomiCampiColonne(2) = "titolo"
        NomiCampiColonne(3) = "bando"
        NomiCampiColonne(4) = "ambito"
        'NomiCampiColonne(4) = "data"
        NomiCampiColonne(5) = "statoattività"
        NomiCampiColonne(6) = "VolRic"
        NomiCampiColonne(7) = "RegioneCompetenza"
        NomiCampiColonne(8) = "NMesi"
        NomiCampiColonne(9) = "TitoloProgramma"
        NomiCampiColonne(10) = "CodiceProgramma"
        NomiCampiColonne(11) = "CodiceRegione"
        'carico un datatable che userò poi nella pagina di stampa
        'il numero delle colonne è a base 0
        CaricaDataTablePerStampa(Mydataset, 11, NomeColonne, NomiCampiColonne)

        '*********************************************************************************

        'Verifico le Abilitazione Dell'Utente
        strquery = "SELECT AssociaUtenteGruppo.username, VociMenu.IdVoceMenu,VociMenu.descrizione, VociMenu.VoceMenu, VociMenu.ImmaginePassiva, VociMenu.ImmagineAttiva, VociMenu.Link,"
        strquery = strquery & " VociMenu.IdVoceMenuPadre"
        strquery = strquery & " FROM VociMenu"
        strquery = strquery & " INNER JOIN AbilitazioniProfiliVociMenu ON VociMenu.IdVoceMenu = AbilitazioniProfiliVociMenu.IdVoceMenu"
        strquery = strquery & " INNER JOIN	Profili ON AbilitazioniProfiliVociMenu.IdProfilo = Profili.IdProfilo"
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
        strquery = strquery & " LEFT JOIN 	RestrizioniUtenteVociMenu ON AssociaUtenteGruppo.UserName = RestrizioniUtenteVociMenu.UserName and RestrizioniUtenteVociMenu.IdVoceMenu=VociMenu.IdVoceMenu"
        strquery = strquery & " WHERE (VociMenu.descrizione = 'Accettazione Progetti'or VociMenu.descrizione = 'Valutazione Qualità Progetti')"
        strquery = strquery & " AND AssociaUtenteGruppo.username ='" & Session("Utente") & "' and (RestrizioniUtenteVociMenu.TipoRestrizione <> 0 or RestrizioniUtenteVociMenu.TipoRestrizione is null)"

        If Not dtrGenerico Is Nothing Then
            dtrGenerico.Close()
            dtrGenerico = Nothing
        End If

        dtrGenerico = ClsServer.CreaDatareader(strquery, Session("conn"))

        Do While dtrGenerico.Read
            Select Case dtrGenerico("descrizione")
                Case "Accettazione Progetti"
                    dgRisultatoRicerca.Columns(1).Visible = True
                Case "Valutazione Qualità Progetti"
                    dgRisultatoRicerca.Columns(2).Visible = True
            End Select
        Loop
        If Not dtrGenerico Is Nothing Then
            dtrGenerico.Close()
            dtrGenerico = Nothing
        End If
        '***********************************

        If dgRisultatoRicerca.Items.Count = 0 Then 'se la griglia e vuota la nascondo
            lblmessaggio.Text = "La ricerca non ha prodotto alcun risultato"
            dgRisultatoRicerca.Visible = False
        End If
        If dgRisultatoRicerca.Items.Count > 0 And UCase(Request.QueryString("esporta")) = "SI" Then
            CmdEsportaElenco.Visible = True
            dgRisultatoRicerca.Columns(0).Visible = False
            dgRisultatoRicerca.Columns(1).Visible = False
            dgRisultatoRicerca.Columns(2).Visible = False
        Else
            CmdEsportaElenco.Visible = False
        End If

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

    Private Sub dgRisultatoRicerca_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgRisultatoRicerca.ItemCommand
        Select Case e.CommandName
            Case "Select"
                If Session("tipoutente") <> "E" Then
                    'Response.Redirect("TabProgetti.aspx?IdAttivita=" & CInt(e.Item.Cells(8).Text) & "&Nazionale=" & CInt(e.Item.Cells(11).Text) & "&Modifica=" & CInt(e.Item.Cells(10).Text) & "")
                    Session("idente") = CInt(e.Item.Cells(9).Text)
                    Session("denominazione") = e.Item.Cells(3).Text
                End If
                Dim strVengoda As String
                strVengoda = "Response.Redirect(""" & ClsUtility.TrovaAlboProgetto(e.Item.Cells(8).Text, Session("Conn")) & "?Nazionale=""" & e.Item.Cells(11).Text & """&Modifica=""" & e.Item.Cells(10).Text & """&IdAttivita=""" & e.Item.Cells(8).Text & ")"
                'strVengoda = "Response.Redirect(""TabProgetti.aspx?Nazionale=""" & e.Item.Cells(11).Text & """&Modifica=""" & e.Item.Cells(10).Text & """&IdAttivita=""" & e.Item.Cells(8).Text & ")"
                'Response.Redirect("TabProgetti.aspx?VengoDa=" & strVengoda & "&Nazionale=" & dgRisultatoRicerca.SelectedItem.Cells(9).Text & "&Modifica=" & dgRisultatoRicerca.SelectedItem.Cells(8).Text & "&IdAttivita=" & dgRisultatoRicerca.SelectedItem.Cells(6).Text & "")
                Dim paginaInModifica As String
                If (dgRisultatoRicerca.Columns(2).Visible = True Or dgRisultatoRicerca.Columns(2).Visible = True) Then
                    paginaInModifica = "1"
                Else
                    paginaInModifica = e.Item.Cells(10).Text
                End If

                Response.Redirect(ClsUtility.TrovaAlboProgetto(e.Item.Cells(8).Text, Session("Conn")) & "?Nazionale=" & e.Item.Cells(11).Text & "&Modifica=" & paginaInModifica & "&IdAttivita=" & e.Item.Cells(8).Text & "")
                'Response.Redirect("TabProgetti.aspx?Nazionale=" & e.Item.Cells(11).Text & "&Modifica=" & paginaInModifica & "&IdAttivita=" & e.Item.Cells(8).Text & "")
            Case "accettazione"
                If Session("tipoutente") <> "E" Then
                    Session("idente") = CInt(e.Item.Cells(9).Text)
                End If

                'Verifico se è possibile effettuare la valutazione del Progetto
                strquery = "select attività.idattività," & _
                " case isnull(convert(smallint,statibandiAttività.Attivo),-1)when -1 then 0 else convert(smallint,statibandiAttività.Attivo) end as attivoStatoAttBando," & _
                " case  isnull(convert(smallint,statibando.invalutazione),-1) when -1 then 0 else convert(smallint,statibando.invalutazione) end as invalutazione," & _
                " statiattività.idstatoattività," & _
                " statiattività.dagraduare,statiattività.daVAlutare, statiattività.respinta " & _
                " from attività " & _
                " inner join enti on attività.identepresentante=enti.idente " & _
                " left join bandiAttività  on attività.idbandoattività=bandiAttività.idbandoattività " & _
                " left join  statibandiAttività on bandiAttività.idstatobandoattività=statibandiAttività.idstatobandoattività" & _
                " left join bando  on bando.idbando=bandiAttività.idbando " & _
                " left join statibando  on bando.idstatobando=statibando.idstatobando " & _
                " inner join statiattività  on attività.idstatoattività=statiattività.idstatoattività " & _
                " where attività.idattività=" & CInt(e.Item.Cells(8).Text) & ""
                If Not dtrGenerico Is Nothing Then
                    dtrGenerico.Close()
                    dtrGenerico = Nothing
                End If

                dtrGenerico = ClsServer.CreaDatareader(strquery, Session("conn"))
                dtrGenerico.Read()
                If dtrGenerico("invalutazione") = 1 And dtrGenerico("attivoStatoAttBando") = 1 Then
                    If Not dtrGenerico Is Nothing Then
                        dtrGenerico.Close()
                        dtrGenerico = Nothing
                    End If
                    'Response.Redirect("WfrmAlbero.aspx?idattivita=" & CInt(e.Item.Cells(8).Text) & "&tipologia=Progetti&Arrivo=ricercaprogetti.aspx")
                    Response.Redirect("assegnazionevincoliprogetti.aspx?idattivita=" & CInt(e.Item.Cells(8).Text) & "&tipologia=Progetti&Vengoda=" & Request.QueryString("VengoDa"))
                Else
                    'Messaggio
                    lblmessaggio.Text = "Non è possibile effettuare l'Accettazione del Progetto."
                    If Not dtrGenerico Is Nothing Then
                        dtrGenerico.Close()
                        dtrGenerico = Nothing
                    End If
                    Exit Sub
                End If

            Case "valutazione"
                'Verifico se è possibile effettuare la valutazione del Progetto
                strquery = "select attività.idattività," & _
                " case  isnull(convert(smallint,statibando.invalutazione),-1) when -1 then 0 else convert(smallint,statibando.invalutazione) end as invalutazione," & _
                " statiattività.idstatoattività," & _
                " statiattività.dagraduare,statiattività.Attiva, statiattività.respinta " & _
                " from attività " & _
                " inner join enti on attività.identepresentante=enti.idente " & _
                " left join bandiAttività  on attività.idbandoattività=bandiAttività.idbandoattività " & _
                " left join bando  on bando.idbando=bandiAttività.idbando " & _
                " left join statibando  on bando.idstatobando=statibando.idstatobando " & _
                " inner join statiattività  on attività.idstatoattività=statiattività.idstatoattività " & _
                " where attività.idattività=" & CInt(e.Item.Cells(8).Text) & ""
                If Not dtrGenerico Is Nothing Then
                    dtrGenerico.Close()
                    dtrGenerico = Nothing
                End If
                dtrGenerico = ClsServer.CreaDatareader(strquery, Session("conn"))
                dtrGenerico.Read()
                'mod il 23/09/2011 è possibile entrare nella scheda di valutazione solo x i progetti che sono nel seguente stato:
                'ATTIVO,TERMINATO,NON ATTIVABILE,IN ATTESA DI GRADUATORIA,CHIUSO PER MANCANZA RICHIESTE
                If dtrGenerico("idstatoattività") = 1 Or dtrGenerico("idstatoattività") = 2 Or dtrGenerico("idstatoattività") = 6 Or dtrGenerico("idstatoattività") = 9 Or dtrGenerico("idstatoattività") = 12 Then
                    If Not dtrGenerico Is Nothing Then
                        dtrGenerico.Close()
                        dtrGenerico = Nothing
                    End If
                    'Context.Items.Add("idprogetto", CInt(e.Item.Cells(8).Text))
                    'Context.Items.Add("Pagina", "WfrmValutazioneQual.aspx")
                    'Server.Transfer("WfrmValutazioneQual.aspx?idprogetto=" & CInt(e.Item.Cells(8).Text))
                    Response.Redirect(ClsUtility.RitornaMascheraValutazioneProgetto(CInt(e.Item.Cells(8).Text), Session("conn")) & "?idprogetto=" & CInt(e.Item.Cells(8).Text))

                Else
                    'Messaggio
                    lblmessaggio.Text = "Non è possibile effettuare la valutazione del Progetto."
                    If Not dtrGenerico Is Nothing Then
                        dtrGenerico.Close()
                        dtrGenerico = Nothing
                    End If
                    Exit Sub
                End If

        End Select
    End Sub

    Private Sub dgRisultatoRicerca_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgRisultatoRicerca.PageIndexChanged
        '***Generata da Gianluigi Paesani in data:05/07/04 
        '***ripasso i parametri precedentemente salvati
        dgRisultatoRicerca.Visible = True
        dgRisultatoRicerca.CurrentPageIndex = 0
        txtTitoloProgetto.Text = txtTitoloProgetto1.Text
        txtDenominazioneEnte.Text = txtDenominazioneEnte1.Text
        ddlMaccCodAmAtt.SelectedIndex = CInt(IIf(ddlMaccCodAmAtt1.Text = "", -1, ddlMaccCodAmAtt1.Text))
        ddlStatoAttivita.SelectedIndex = CInt(IIf(ddlStatoAttivita1.Text = "", -1, ddlStatoAttivita1.Text))
        'If ddlMaccCodAmAtt1.Text = "" Then
        '    ddlCodAmAtt.DataSource = Nothing
        '    ddlCodAmAtt.Enabled = False
        '    ddlCodAmAtt.Items.Add("")
        '    ddlCodAmAtt.SelectedIndex = 0
        'Else
        '    ddlCodAmAtt.SelectedIndex = CInt(ddlCodAmAtt1.Text)
        '    ddlCodAmAtt.Enabled = True
        'End If
        'passo valore pagina
        EseguiRicerca(1, e.NewPageIndex)
    End Sub

    Protected Sub CmdRicerca_Click(ByVal sender As Object, ByVal e As EventArgs) Handles CmdRicerca.Click
        '***Generata da Gianluigi Paesani in data:05/07/04
        '*** 'salvo parametri per controllo link pagine
        lblmessaggio.Text = ""
        txtTitoloProgetto1.Text = txtTitoloProgetto.Text
        txtDenominazioneEnte1.Text = txtDenominazioneEnte.Text
        ddlMaccCodAmAtt1.Text = ddlMaccCodAmAtt.SelectedIndex
        'ddlCodAmAtt1.Text = ddlCodAmAtt.SelectedIndex
        ddlStatoAttivita1.Text = ddlStatoAttivita.SelectedIndex
        dgRisultatoRicerca.CurrentPageIndex = 0
        CmdEsportaElenco.Visible = False
        hlApriElencoProgetti.Visible = False
        hlApriElencoSediProgetti.Visible = False
        hlApriElencoRisorseProgetto.Visible = False

        ApriCSV1.Visible = False
        EseguiRicerca(0)
    End Sub


    Function StampaCSV(ByVal DTBRicerca As DataTable)

        Dim dtrSediAttuazione As Data.SqlClient.SqlDataReader

        Dim Writer As StreamWriter
        Dim xLinea As String
        Dim i As Int64
        Dim j As Int64
        Dim NomeUnivoco As String
        Dim xPrefissoNome As String = vbNullString
        Dim Reader As StreamReader

        If DTBRicerca.Rows.Count = 0 Then
            ApriCSV1.Visible = False
            CmdEsportaElenco.Visible = False
        Else
            xPrefissoNome = Session("Utente")
            NomeUnivoco = xPrefissoNome & "ExpElencoGriglia" & Year(Now) & Month(Now) & Day(Now) & Hour(Now) & Minute(Now) & Second(Now)
            Writer = New StreamWriter(Server.MapPath("download") & "\" & NomeUnivoco & ".CSV")
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

            ApriCSV1.Visible = True
            ApriCSV1.NavigateUrl = "download\" & NomeUnivoco & ".CSV"

            Writer.Close()
            Writer = Nothing

        End If

    End Function

    Private Sub EsportaProgetti()
        Dim StrSql As String
        Dim dtrProgetti As Data.SqlClient.SqlDataReader

        Dim Writer As StreamWriter
        Dim xLinea As String
        Dim i As Integer
        Dim NomeUnivoco As String
        Dim xPrefissoNome As String

        Try
            'If VengoDa = "Valutare" Then
            '    StrSql = "SELECT " & _
            '    "b.CodiceRegione, " & _
            '    "a.IdAttività, " & _
            '    "a.CodiceEnte, " & _
            '    "isnull(replace(replace(replace(replace(replace(replace(replace(a.Titolo,'°',''),'ì','i'''),'é','e'''),'à','a'''),'ò','o'''),'è','e'''),'ù','u'''), ''), " & _
            '    "isnull(replace(replace(replace(replace(replace(replace(replace(c.Bando,'°',''),'ì','i'''),'é','e'''),'à','a'''),'ò','o'''),'è','e'''),'ù','u'''), ''), " & _
            '    "isnull(replace(replace(replace(replace(replace(replace(replace(g.MacroAmbitoAttività,'°',''),'ì','i'''),'é','e'''),'à','a'''),'ò','o'''),'è','e'''),'ù','u'''), ''), " & _
            '    "isnull(replace(replace(replace(replace(replace(replace(replace(f.AmbitoAttività,'°',''),'ì','i'''),'é','e'''),'à','a'''),'ò','o'''),'è','e'''),'ù','u'''), ''), " & _
            '    "convert(varchar,isnull(a.dataultimostato,''),3) as Data " & _
            '    "FROM Attività a " & _
            '    "INNER JOIN Enti b ON a.identepresentante=b.idente " & _
            '    "INNER JOIN bandiAttività h ON a.idbandoattività=h.idbandoattività " & _
            '    "INNER JOIN bando c ON c.idbando=h.idbando " & _
            '    "INNER JOIN Statibando d ON c.idstatobando=d.idstatobando " & _
            '    "INNER JOIN StatiAttività e ON a.idstatoattività=e.idstatoattività " & _
            '    "INNER JOIN AmbitiAttività f ON f.idambitoattività=a.idambitoattività " & _
            '    "INNER JOIN MacroAmbitiAttività g ON f.IDMacroAmbitoAttività=g.IDMacroAmbitoAttività " & _
            '    "INNER JOIN TipiProgetto ON a.IdTipoProgetto = TipiProgetto.IdTipoProgetto " & _
            '    "INNER JOIN AssociaProfiliTipiProgetto ON TipiProgetto.IdTipoProgetto = AssociaProfiliTipiProgetto.IdTipoProgetto " & _
            '    "INNER JOIN Profili ON AssociaProfiliTipiProgetto.IdProfilo = Profili.IdProfilo "

            '    '============================================================================================================================
            '    '====================================================30/09/2008==============================================================
            '    '=======MODIFICA EFFETTUATA DA Kronk (99 scimmie saltavano sul letto, una cadde in terra e si ruppe il cervelletto...)=======
            '    '=================AGGIUNTA JOIN SU PROFILO READ PER ABILITARE LEGGERE LE ABILITAZIONI ANCHE DELL'UTENTE READ=================
            '    '============================================================================================================================
            '    If UCase(Me.TemplateSourceDirectory) <> "/HELIOSREAD" Then
            '        StrSql = StrSql & " INNER JOIN	AssociaUtenteGruppo ON Profili.IdProfilo = AssociaUtenteGruppo.IdProfilo "
            '    Else
            '        StrSql = StrSql & " INNER JOIN	AssociaUtenteGruppo ON Profili.IdProfilo = AssociaUtenteGruppo.IdProfiloRead "
            '    End If

            '    StrSql = StrSql & " LEFT JOIN BANDORICORSI ON a.IDBANDORICORSO = BANDORICORSI.IDBANDORICORSO " & _
            '    "LEFT JOIN StatiBandiAttività i ON h.idstatobandoattività=i.idstatobandoattività " & _
            '    "WHERE d.InValutazione = 1 " & _
            '    " AND a.identepresentante = " & Session("IdEnte") & "  and AssociaUtenteGruppo.username='" & Replace(Session("Utente"), "'", "''") & "' "
            '    If Session("TipoUtente") = "E" Then
            '        StrSql = StrSql & " and CASE ISNULL(BANDORICORSI.IDBANDORICORSO,0) WHEN 0 THEN isnull(c.enteabilitato,1) ELSE isnull(BANDORICORSI.enteabilitato,1) END = 1 "
            '    End If
            'Else
            StrSql = "SELECT DISTINCT " & _
            "b.CodiceRegione, " & _
            "a.IdAttività, " & _
            "a.CodiceEnte, " & _
            "isnull(replace(replace(replace(replace(replace(replace(replace(a.Titolo,'°',''),'ì','i'''),'é','e'''),'à','a'''),'ò','o'''),'è','e'''),'ù','u'''), ''), " & _
            "isnull(replace(replace(replace(replace(replace(replace(replace(c.Bando,'°',''),'ì','i'''),'é','e'''),'à','a'''),'ò','o'''),'è','e'''),'ù','u'''), ''), " & _
            "isnull(replace(replace(replace(replace(replace(replace(replace(g.MacroAmbitoAttività,'°',''),'ì','i'''),'é','e'''),'à','a'''),'ò','o'''),'è','e'''),'ù','u'''), ''), " & _
            "isnull(replace(replace(replace(replace(replace(replace(replace(f.AmbitoAttività,'°',''),'ì','i'''),'é','e'''),'à','a'''),'ò','o'''),'è','e'''),'ù','u'''), ''), " & _
            "a.nmesi, convert(varchar,isnull(a.dataultimostato,''),3) as Data " & _
            "FROM Attività a " & _
            "INNER JOIN Enti b ON a.IdEntePresentante=b.IdEnte " & _
            "LEFT JOIN AttivitàEntiSediAttuazione m ON a.IdAttività = m.IdAttività " & _
            "LEFT JOIN EntiSediAttuazioni n ON m.IdEnteSedeAttuazione = n.IdEnteSedeAttuazione " & _
            "LEFT JOIN EntiSedi o ON n.IdEnteSede = o.IdEnteSede " & _
            "LEFT JOIN Comuni p ON o.IdComune = p.IdComune " & _
            "LEFT JOIN Provincie x ON x.IdProvincia = p.IdProvincia " & _
            "LEFT JOIN BandiAttività h ON a.IdBandoAttività=h.IdBandoAttività " & _
            "LEFT JOIN Bando c on c.IdBando=h.IdBando " & _
            "INNER JOIN StatiAttività e ON a.IdStatoAttività=e.IdStatoAttività " & _
            "INNER JOIN AmbitiAttività f ON f.IdAmbitoAttività=a.IdAmbitoAttività " & _
            "INNER JOIN MacroAmbitiAttività g ON f.IdMacroAmbitoAttività=g.IdMacroAmbitoAttività " & _
            "INNER JOIN TipiProgetto ON a.IdTipoProgetto = TipiProgetto.IdTipoProgetto " & _
            "INNER JOIN AssociaProfiliTipiProgetto ON TipiProgetto.IdTipoProgetto = AssociaProfiliTipiProgetto.IdTipoProgetto " & _
            "INNER JOIN Profili ON AssociaProfiliTipiProgetto.IdProfilo = Profili.IdProfilo "
            '============================================================================================================================
            '====================================================30/09/2008==============================================================
            '=======MODIFICA EFFETTUATA DA Kronk (99 scimmie saltavano sul letto, una cadde in terra e si ruppe il cervelletto...)=======
            '=================AGGIUNTA JOIN SU PROFILO READ PER ABILITARE LEGGERE LE ABILITAZIONI ANCHE DELL'UTENTE READ=================
            '============================================================================================================================
            If Session("Read") <> "1" Then
                StrSql = StrSql & " INNER JOIN	AssociaUtenteGruppo ON Profili.IdProfilo = AssociaUtenteGruppo.IdProfilo "
            Else
                StrSql = StrSql & " INNER JOIN	AssociaUtenteGruppo ON Profili.IdProfilo = AssociaUtenteGruppo.IdProfiloRead "
            End If
            StrSql = StrSql & " LEFT JOIN BANDORICORSI ON a.IDBANDORICORSO = BANDORICORSI.IDBANDORICORSO " & _
            "LEFT JOIN StatiBandiAttività i ON h.IdStatoBandoAttività=i.IdStatoBandoAttività " & _
            "WHERE a.IdAttività is not null " & _
            " AND a.identepresentante = " & Session("IdEnte") & "  and AssociaUtenteGruppo.username='" & Replace(Session("Utente"), "'", "''") & "' "
            If Session("TipoUtente") = "E" Then
                StrSql = StrSql & " and CASE ISNULL(BANDORICORSI.IDBANDORICORSO,0) WHEN 0 THEN isnull(c.enteabilitato,1) ELSE isnull(BANDORICORSI.enteabilitato,1) END = 1 "
            End If
            'End If

            If DdlTipiProgetto.SelectedItem.Text <> "" Then
                StrSql = StrSql & " and a.idTipoProgetto=" & DdlTipiProgetto.SelectedValue & ""
            End If

            If CStr(Session("TipoUtente")) = "E" Then
                StrSql = StrSql & " and b.idente = '" & CStr(Session("IdEnte")) & "'"
            End If

            If txtDenominazioneEnte.Text <> "" And txtDenominazioneEnteSecondario.Text <> "" Then
                StrSql = StrSql & " and b.denominazione  like '" & ClsServer.NoApice(Trim(txtDenominazioneEnte.Text)) & "%'"
            End If
            If TxtCodPog.Text <> "" Then
                StrSql = StrSql & " and a.CodiceEnte  like '" & ClsServer.NoApice(Trim(TxtCodPog.Text)) & "%'"
            End If
            If ddlStatoAttivita.SelectedItem.Text <> "" Then
                StrSql = StrSql & "AND e.IdStatoAttività = " & ddlStatoAttivita.SelectedValue & " "
            End If

            If txtTitoloProgetto.Text <> "" Then
                StrSql = StrSql & "AND a.Titolo LIKE '" & ClsServer.NoApice(txtTitoloProgetto.Text) & "%' "
            End If

            If DdlBando.SelectedItem.Text <> "" Then
                StrSql = StrSql & " and c.bandobreve = '" & ClsServer.NoApice(DdlBando.SelectedItem.Text) & "'"
            End If
            If txtDenominazioneEnte.Text <> "" Then
                StrSql = StrSql & " and b.denominazione like '" & ClsServer.NoApice(Trim(txtDenominazioneEnte.Text)) & "%'"
            End If

            If ddlMaccCodAmAtt.SelectedItem.Text <> "" Then
                StrSql = StrSql & " and f.idmacroambitoattività=" & ddlMaccCodAmAtt.SelectedValue & ""
            End If

            If TxtCodPog.Text <> "" Then
                StrSql = StrSql & " and a.CodiceEnte  like '" & ClsServer.NoApice(Trim(TxtCodPog.Text)) & "%'"
            End If

            If ddlDurataProgetto.SelectedValue <> 0 Then
                StrSql = StrSql & " and a.nmesi =" & ddlDurataProgetto.SelectedValue
            End If
            If txtcomune.Text <> "" Then
                StrSql = StrSql & " and p.Denominazione  like '" & ClsServer.NoApice(Trim(txtcomune.Text)) & "%'"
            End If
            If txtProvincia.Text <> "" Then
                StrSql = StrSql & " and x.Provincia  like '" & ClsServer.NoApice(Trim(txtProvincia.Text)) & "%'"
            End If

            If ddlSegnalazioneSanzione.SelectedItem.Text <> "Tutti" Then
                StrSql = StrSql & " and isnull(a.SegnalazioneSanzione,0) =" & ddlSegnalazioneSanzione.SelectedValue & ""
            End If

            If ddlStatoValutazione.SelectedItem.Text <> "Tutti" Then
                StrSql = StrSql & " and isnull(a.StatoValutazione,0) =" & ddlStatoValutazione.SelectedValue & ""
            End If


            If ddlGiovaniMinoriOp.SelectedItem.Text <> "Tutti" Then
                StrSql = StrSql & " and isnull(a.GiovaniMinoriOpportunità,0) = " & ddlGiovaniMinoriOp.SelectedValue & ""
            End If
            If ddlFami.SelectedItem.Text <> "Tutti" Then
                StrSql = StrSql & " and isnull(a.Fami,0) = " & ddlFami.SelectedValue & ""
            End If
            '** agg. da s.c. il 27/11/2014 FiltroVisibilita
            StrSql = StrSql & " and TipiProgetto.MacroTipoProgetto like '" & Session("FiltroVisibilita") & "' "
            'StrSql = StrSql & "ORDER BY year(a.dataultimostato)desc,month(a.dataultimostato)desc,day(a.dataultimostato)desc,1,2"
            dtrProgetti = ClsServer.CreaDatareader(StrSql, Session("conn"))

            NomeUnivoco = vbNullString
            If dtrProgetti.HasRows = False Then
                lblmessaggio.Text = "Nessun Progetto."
            Else
                While dtrProgetti.Read
                    If NomeUnivoco = vbNullString Then
                        If dtrProgetti(0) = "0" Then
                            xPrefissoNome = Session("Utente")
                        Else
                            xPrefissoNome = dtrProgetti(0)
                        End If
                        NomeUnivoco = xPrefissoNome & "ExpPrg" & Year(Now) & Month(Now) & Day(Now) & Hour(Now) & Minute(Now) & Second(Now)
                        Writer = New StreamWriter(Server.MapPath("download") & "\" & NomeUnivoco & ".CSV")
                        xLinea = "Codice Progetto Interno;Codice Progetto;Titolo;Bando;Settore;Area;Durata (mesi);"
                        Writer.WriteLine(xLinea)
                    End If
                    xLinea = vbNullString
                    '---salto il primo e l'ulimo elemento (nome file & data usata solo per order by)
                    For i = 1 To dtrProgetti.FieldCount - 2
                        If IsDBNull(dtrProgetti(i)) = True Then
                            xLinea = xLinea & ";"
                        Else
                            xLinea = xLinea & ClsUtility.FormatExport(dtrProgetti(i)) & ";"
                        End If
                    Next i
                    Writer.WriteLine(xLinea)
                End While
                hlApriElencoProgetti.Visible = True
                hlApriElencoProgetti.NavigateUrl = "download\" & NomeUnivoco & ".CSV"
                Writer.Close()
                Writer = Nothing
            End If

            dtrProgetti.Close()
            dtrProgetti = Nothing

        Catch ex As Exception
            'lblErr.Text = lblErr.Text & "Errore durante l'esportazione dei Progetti."
            If Not Writer Is Nothing Then
                Writer.Close()
                Writer = Nothing
            End If
            If Not dtrProgetti Is Nothing Then
                dtrProgetti.Close()
                dtrProgetti = Nothing
            End If
        End Try
    End Sub

    Private Sub EsportaSediAttuazione()
        Dim StrSql As String
        Dim dtrSediAttuazione As Data.SqlClient.SqlDataReader

        Dim Writer As StreamWriter
        Dim xLinea As String
        Dim i As Integer
        Dim NomeUnivoco As String
        Dim xPrefissoNome As String

        Try
            'If VengoDa = "Valutare" Then
            '    StrSql = "SELECT distinct " & _
            '    "b.CodiceRegione, " & _
            '    "a.IdAttività, " & _
            '    "a.CodiceEnte, " & _
            '    "isnull(replace(replace(replace(replace(replace(replace(replace(a.Titolo,'°',''),'ì','i'''),'é','e'''),'à','a'''),'ò','o'''),'è','e'''),'ù','u'''), ''), " & _
            '    "isnull(replace(replace(replace(replace(replace(replace(replace(c.Bando,'°',''),'ì','i'''),'é','e'''),'à','a'''),'ò','o'''),'è','e'''),'ù','u'''), ''), " & _
            '    "isnull(replace(replace(replace(replace(replace(replace(replace(g.MacroAmbitoAttività,'°',''),'ì','i'''),'é','e'''),'à','a'''),'ò','o'''),'è','e'''),'ù','u'''), ''), " & _
            '    "isnull(replace(replace(replace(replace(replace(replace(replace(f.AmbitoAttività,'°',''),'ì','i'''),'é','e'''),'à','a'''),'ò','o'''),'è','e'''),'ù','u'''), ''), "
            '    '"isnull(replace(replace(replace(replace(replace(replace(replace(o.Denominazione,'°',''),'ì','i'''),'é','e'''),'à','a'''),'ò','o'''),'è','e'''),'ù','u'''), ''), " & _

            '    StrSql = StrSql & "n.IDEnteSedeAttuazione, " & _
            '    "isnull(replace(replace(replace(replace(replace(replace(replace(n.Denominazione,'°',''),'ì','i'''),'é','e'''),'à','a'''),'ò','o'''),'è','e'''),'ù','u'''), ''), " & _
            '    "isnull(replace(replace(replace(replace(replace(replace(replace(p.Denominazione,'°',''),'ì','i'''),'é','e'''),'à','a'''),'ò','o'''),'è','e'''),'ù','u'''), ''), " & _
            '    "isnull(replace(replace(replace(replace(replace(replace(replace(o.Indirizzo,'°',''),'ì','i'''),'é','e'''),'à','a'''),'ò','o'''),'è','e'''),'ù','u'''), '') + ', ' + o.Civico as Indirizzo, " & _
            '    "o.Cap, " & _
            '    "o.PrefissoTelefono + '/' + o.Telefono as Telefono, " & _
            '    "o.Email, "
            '    StrSql = StrSql & "k.StatoEnteSede, " & _
            '    "ISNULL(m.NumeroPostiVittoAlloggio,0) As NumeroPostiVittoAlloggio, " & _
            '    "ISNULL(m.NumeroPostiVitto,0) As NumeroPostiVitto, " & _
            '    "ISNULL(m.NumeroPostiNoVittoNoAlloggio,0) As NumeroPostiNoVittoNoAlloggio " & _
            '    "FROM Attività a " & _
            '    "INNER JOIN Enti b ON a.identepresentante = b.idente " & _
            '    "INNER JOIN AttivitàEntiSediAttuazione m ON a.IdAttività = m.IdAttività " & _
            '    "INNER JOIN EntiSediAttuazioni n ON m.IdEnteSedeAttuazione = n.IdEnteSedeAttuazione " & _
            '    "INNER JOIN EntiSedi o ON n.IdEnteSede = o.IdEnteSede " & _
            '    "INNER JOIN Comuni p ON o.IdComune = p.IdComune " & _
            '    "INNER JOIN bandiAttività h ON a.idbandoattività=h.idbandoattività " & _
            '    "INNER JOIN bando c ON c.idbando=h.idbando " & _
            '    "INNER JOIN Statibando d ON c.idstatobando=d.idstatobando " & _
            '    "INNER JOIN StatiAttività e ON a.idstatoattività=e.idstatoattività " & _
            '    "INNER JOIN AmbitiAttività f ON f.idambitoattività=a.idambitoattività " & _
            '    "INNER JOIN MacroAmbitiAttività g ON f.IDMacroAmbitoAttività=g.IDMacroAmbitoAttività " & _
            '    "INNER JOIN StatiEntiSedi k ON n.IdStatoEnteSede = k.IdStatoEnteSede " & _
            '    "INNER JOIN TipiProgetto ON a.IdTipoProgetto = TipiProgetto.IdTipoProgetto " & _
            '    "INNER JOIN AssociaProfiliTipiProgetto ON TipiProgetto.IdTipoProgetto = AssociaProfiliTipiProgetto.IdTipoProgetto " & _
            '    "INNER JOIN Profili ON AssociaProfiliTipiProgetto.IdProfilo = Profili.IdProfilo "

            '    '============================================================================================================================
            '    '====================================================30/09/2008==============================================================
            '    '=======MODIFICA EFFETTUATA DA Kronk (99 scimmie saltavano sul letto, una cadde in terra e si ruppe il cervelletto...)=======
            '    '=================AGGIUNTA JOIN SU PROFILO READ PER ABILITARE LEGGERE LE ABILITAZIONI ANCHE DELL'UTENTE READ=================
            '    '============================================================================================================================
            '    If UCase(Me.TemplateSourceDirectory) <> "/HELIOSREAD" Then
            '        StrSql = StrSql & " INNER JOIN	AssociaUtenteGruppo ON Profili.IdProfilo = AssociaUtenteGruppo.IdProfilo "
            '    Else
            '        StrSql = StrSql & " INNER JOIN	AssociaUtenteGruppo ON Profili.IdProfilo = AssociaUtenteGruppo.IdProfiloRead "
            '    End If

            '    StrSql = StrSql & " LEFT JOIN BANDORICORSI ON a.IDBANDORICORSO = BANDORICORSI.IDBANDORICORSO " & _
            '    "LEFT JOIN StatiBandiAttività i ON h.idstatobandoattività=i.idstatobandoattività " & _
            '    "WHERE d.InValutazione = 1 " & _
            '    " AND a.identepresentante = " & Session("IdEnte") & " and AssociaUtenteGruppo.username='" & Replace(Session("Utente"), "'", "''") & "' "
            '    If Session("TipoUtente") = "E" Then
            '        StrSql = StrSql & " and CASE ISNULL(BANDORICORSI.IDBANDORICORSO,0) WHEN 0 THEN isnull(c.enteabilitato,1) ELSE isnull(BANDORICORSI.enteabilitato,1) END = 1 "
            '    End If
            'Else
            StrSql = "SELECT distinct " & _
            "b.CodiceRegione, " & _
            "a.IdAttività, " & _
            "a.CodiceEnte, " & _
            "isnull(replace(replace(replace(replace(replace(replace(replace(a.Titolo,'°',''),'ì','i'''),'é','e'''),'à','a'''),'ò','o'''),'è','e'''),'ù','u'''), ''), " & _
            "isnull(replace(replace(replace(replace(replace(replace(replace(c.Bando,'°',''),'ì','i'''),'é','e'''),'à','a'''),'ò','o'''),'è','e'''),'ù','u'''), ''), " & _
            "isnull(replace(replace(replace(replace(replace(replace(replace(g.MacroAmbitoAttività,'°',''),'ì','i'''),'é','e'''),'à','a'''),'ò','o'''),'è','e'''),'ù','u'''), ''), " & _
            "isnull(replace(replace(replace(replace(replace(replace(replace(f.AmbitoAttività,'°',''),'ì','i'''),'é','e'''),'à','a'''),'ò','o'''),'è','e'''),'ù','u'''), ''), "
            '"isnull(replace(replace(replace(replace(replace(replace(replace(o.Denominazione,'°',''),'ì','i'''),'é','e'''),'à','a'''),'ò','o'''),'è','e'''),'ù','u'''), ''), " & _

            StrSql = StrSql & "n.IDEnteSedeAttuazione, " & _
            "isnull(replace(replace(replace(replace(replace(replace(replace(N.Denominazione,'°',''),'ì','i'''),'é','e'''),'à','a'''),'ò','o'''),'è','e'''),'ù','u'''), ''), " & _
            "isnull(replace(replace(replace(replace(replace(replace(replace(x.Provincia,'°',''),'ì','i'''),'é','e'''),'à','a'''),'ò','o'''),'è','e'''),'ù','u'''), '')  as Provincia, " & _
            "isnull(replace(replace(replace(replace(replace(replace(replace(p.Denominazione,'°',''),'ì','i'''),'é','e'''),'à','a'''),'ò','o'''),'è','e'''),'ù','u'''), ''), " & _
            "isnull(replace(replace(replace(replace(replace(replace(replace(o.Indirizzo,'°',''),'ì','i'''),'é','e'''),'à','a'''),'ò','o'''),'è','e'''),'ù','u'''), '') + ', ' + o.Civico as Indirizzo, " & _
            "o.Cap, " & _
            "o.PrefissoTelefono + '/' + o.Telefono as Telefono, " & _
            "o.Email, "
            '"n.IDEnteSedeAttuazione, " & _
            '"n.Denominazione, " & _

            StrSql = StrSql & "k.StatoEnteSede, " & _
            "ISNULL(m.NumeroPostiVittoAlloggio,0) As NumeroPostiVittoAlloggio, " & _
            "ISNULL(m.NumeroPostiVitto,0) As NumeroPostiVitto, " & _
            "ISNULL(m.NumeroPostiNoVittoNoAlloggio,0) As NumeroPostiNoVittoNoAlloggio,a.nmesi,er.CodiceRegione as CodiceEnteRiferimentoSede " & _
            "FROM Attività a " & _
            "INNER JOIN Enti b ON a.identepresentante = b.idente " & _
            "INNER JOIN AttivitàEntiSediAttuazione m ON a.IdAttività = m.IdAttività " & _
            "INNER JOIN EntiSediAttuazioni n ON m.IdEnteSedeAttuazione = n.IdEnteSedeAttuazione " & _
            "INNER JOIN EntiSedi o ON n.IdEnteSede = o.IdEnteSede " & _
            " INNER JOIN ENTI ER ON O.IDENTE=er.idente " & _
            "INNER JOIN Comuni p ON o.IdComune = p.IdComune " & _
            "INNER JOIN Provincie x ON x.IdProvincia = p.IdProvincia " & _
            "left JOIN bandiAttività h ON a.idbandoattività=h.idbandoattività " & _
            "left JOIN bando c ON c.idbando=h.idbando " & _
            "left JOIN Statibando d ON c.idstatobando=d.idstatobando " & _
            "INNER JOIN StatiAttività e ON a.idstatoattività=e.idstatoattività " & _
            "INNER JOIN AmbitiAttività f ON f.idambitoattività=a.idambitoattività " & _
            "INNER JOIN MacroAmbitiAttività g ON f.IDMacroAmbitoAttività=g.IDMacroAmbitoAttività " & _
            "INNER JOIN StatiEntiSedi k ON n.IdStatoEnteSede = k.IdStatoEnteSede " & _
            "INNER JOIN TipiProgetto ON a.IdTipoProgetto = TipiProgetto.IdTipoProgetto " & _
            "INNER JOIN AssociaProfiliTipiProgetto ON TipiProgetto.IdTipoProgetto = AssociaProfiliTipiProgetto.IdTipoProgetto " & _
            "INNER JOIN Profili ON AssociaProfiliTipiProgetto.IdProfilo = Profili.IdProfilo "
            '============================================================================================================================
            '====================================================30/09/2008==============================================================
            '=======MODIFICA EFFETTUATA DA Kronk (99 scimmie saltavano sul letto, una cadde in terra e si ruppe il cervelletto...)=======
            '=================AGGIUNTA JOIN SU PROFILO READ PER ABILITARE LEGGERE LE ABILITAZIONI ANCHE DELL'UTENTE READ=================
            '============================================================================================================================
            If Session("Read") <> "1" Then
                StrSql = StrSql & " INNER JOIN	AssociaUtenteGruppo ON Profili.IdProfilo = AssociaUtenteGruppo.IdProfilo "
            Else
                StrSql = StrSql & " INNER JOIN	AssociaUtenteGruppo ON Profili.IdProfilo = AssociaUtenteGruppo.IdProfiloRead "
            End If
            StrSql = StrSql & " LEFT JOIN BANDORICORSI ON a.IDBANDORICORSO = BANDORICORSI.IDBANDORICORSO " & _
            "LEFT JOIN StatiBandiAttività i ON h.idstatobandoattività=i.idstatobandoattività " & _
            "WHERE a.IdAttività is not null " & _
            "AND a.identepresentante = " & Session("IdEnte") & " and AssociaUtenteGruppo.username='" & Replace(Session("Utente"), "'", "''") & "' "
            If Session("TipoUtente") = "E" Then
                StrSql = StrSql & " and CASE ISNULL(BANDORICORSI.IDBANDORICORSO,0) WHEN 0 THEN isnull(c.enteabilitato,1) ELSE isnull(BANDORICORSI.enteabilitato,1) END = 1 "
            End If
            'End If

            If DdlTipiProgetto.SelectedItem.Text <> "" Then
                StrSql = StrSql & " and a.idTipoProgetto=" & DdlTipiProgetto.SelectedValue & ""
            End If

            If CStr(Session("TipoUtente")) = "E" Then
                StrSql = StrSql & " and b.idente = '" & CStr(Session("IdEnte")) & "'"
            End If

            If txtDenominazioneEnte.Text <> "" And txtDenominazioneEnteSecondario.Text <> "" Then
                StrSql = StrSql & " and b.denominazione  like '" & ClsServer.NoApice(Trim(txtDenominazioneEnte.Text)) & "%'"
            End If
            If TxtCodPog.Text <> "" Then
                StrSql = StrSql & " and a.CodiceEnte  like '" & ClsServer.NoApice(Trim(TxtCodPog.Text)) & "%'"
            End If
            If ddlStatoAttivita.SelectedItem.Text <> "" Then
                StrSql = StrSql & "AND e.IdStatoAttività = " & ddlStatoAttivita.SelectedValue & " "
            End If

            If txtTitoloProgetto.Text <> "" Then
                StrSql = StrSql & "AND a.Titolo LIKE '" & ClsServer.NoApice(txtTitoloProgetto.Text) & "%' "
            End If

            If DdlBando.SelectedItem.Text <> "" Then
                StrSql = StrSql & " and c.bandobreve = '" & ClsServer.NoApice(DdlBando.SelectedItem.Text) & "'"
            End If
            If txtDenominazioneEnte.Text <> "" Then
                StrSql = StrSql & " and b.denominazione like '" & ClsServer.NoApice(Trim(txtDenominazioneEnte.Text)) & "%'"
            End If

            If ddlMaccCodAmAtt.SelectedItem.Text <> "" Then
                StrSql = StrSql & " and f.idmacroambitoattività=" & ddlMaccCodAmAtt.SelectedValue & ""
            End If

            If TxtCodPog.Text <> "" Then
                StrSql = StrSql & " and a.CodiceEnte  like '" & ClsServer.NoApice(Trim(TxtCodPog.Text)) & "%'"
            End If
            '
            If ddlDurataProgetto.SelectedValue <> 0 Then
                StrSql = StrSql & " and a.nmesi =" & ddlDurataProgetto.SelectedValue
            End If
            If txtProvincia.Text <> "" Then
                StrSql = StrSql & " and x.Provincia  like '" & ClsServer.NoApice(Trim(txtProvincia.Text)) & "%'"
            End If
            If txtcomune.Text <> "" Then
                StrSql = StrSql & " and p.Denominazione  like '" & ClsServer.NoApice(Trim(txtcomune.Text)) & "%'"
            End If
            If ddlSegnalazioneSanzione.SelectedItem.Text <> "Tutti" Then
                StrSql = StrSql & " and isnull(a.SegnalazioneSanzione,0) =" & ddlSegnalazioneSanzione.SelectedValue & ""
            End If

            If ddlStatoValutazione.SelectedItem.Text <> "Tutti" Then
                StrSql = StrSql & " and isnull(a.StatoValutazione,0) =" & ddlStatoValutazione.SelectedValue & ""
            End If


            If ddlGiovaniMinoriOp.SelectedItem.Text <> "Tutti" Then
                StrSql = StrSql & " and isnull(a.GiovaniMinoriOpportunità,0) = " & ddlGiovaniMinoriOp.SelectedValue & ""
            End If
            If ddlFami.SelectedItem.Text <> "Tutti" Then
                StrSql = StrSql & " and isnull(a.Fami,0) = " & ddlFami.SelectedValue & ""
            End If
            '** agg. da s.c. il 27/11/2014 FiltroVisibilita
            StrSql = StrSql & " and TipiProgetto.MacroTipoProgetto like '" & Session("FiltroVisibilita") & "' "
            StrSql = StrSql & "ORDER BY a.CodiceEnte "
            dtrSediAttuazione = ClsServer.CreaDatareader(StrSql, Session("conn"))

            NomeUnivoco = vbNullString
            If dtrSediAttuazione.HasRows = False Then
                lblmessaggio.Text = lblmessaggio.Text & "Nessuna Sede di Attuazione."
            Else
                While dtrSediAttuazione.Read
                    If NomeUnivoco = vbNullString Then
                        If dtrSediAttuazione(0) = "0" Then
                            xPrefissoNome = Session("Utente")
                        Else
                            xPrefissoNome = dtrSediAttuazione(0)
                        End If
                        NomeUnivoco = xPrefissoNome & "ExpPrgAtt" & Year(Now) & Month(Now) & Day(Now) & Hour(Now) & Minute(Now) & Second(Now)
                        Writer = New StreamWriter(Server.MapPath("download") & "\" & NomeUnivoco & ".CSV")
                        xLinea = "Codice Progetto Interno;Codice Progetto;Titolo;Circolare;Settore;Area;CodiceSede;Sede;Provincia;Comune;Indirizzo;CAP;Telefono;E-Mail;Stato;NumeroPostiVittoAlloggio;NumeroPostiVitto;NumeroPostiNoVittoNoAlloggio;Durata (mesi);CodiceEnteRiferimentoSede;"
                        Writer.WriteLine(xLinea)
                    End If
                    xLinea = vbNullString
                    '---salto il primo e l'ulimo elemento (nome file & data usata solo per order by)
                    For i = 1 To dtrSediAttuazione.FieldCount - 1
                        If IsDBNull(dtrSediAttuazione(i)) = True Then
                            xLinea = xLinea & ";"
                        Else
                            xLinea = xLinea & ClsUtility.FormatExport(dtrSediAttuazione(i)) & ";"
                        End If
                    Next i
                    Writer.WriteLine(xLinea)
                End While
                hlApriElencoSediProgetti.Visible = True
                hlApriElencoSediProgetti.NavigateUrl = "download\" & NomeUnivoco & ".CSV"
                Writer.Close()
                Writer = Nothing
            End If

            dtrSediAttuazione.Close()
            dtrSediAttuazione = Nothing

        Catch ex As Exception
            'lblErr.Text = lblErr.Text & "Errore durante l'esportazione delle Sedi di Attuazione."
            If Not Writer Is Nothing Then
                Writer.Close()
                Writer = Nothing
            End If
            If Not dtrSediAttuazione Is Nothing Then
                dtrSediAttuazione.Close()
                dtrSediAttuazione = Nothing
            End If
        End Try

    End Sub

    Private Sub EsportaRisorse()
        Dim strsql As String


        Dim dtrSediAttuazione As Data.SqlClient.SqlDataReader

        Dim Writer As StreamWriter
        Dim xLinea As String
        Dim i As Integer
        Dim NomeUnivoco As String
        Dim xPrefissoNome As String

        Try
            strsql = "SELECT distinct b.CodiceRegione, a.IdAttività, a.CodiceEnte, " & _
            " isnull(replace(replace(replace(replace(replace(replace(replace(a.Titolo,'°',''),'ì','i'''),'é','e'''),'à','a'''),'ò','o'''),'è','e'''),'ù','u'''), ''), " & _
            " isnull(replace(replace(replace(replace(replace(replace(replace(c.Bando,'°',''),'ì','i'''),'é','e'''),'à','a'''),'ò','o'''),'è','e'''),'ù','u'''), ''), " & _
            " isnull(replace(replace(replace(replace(replace(replace(replace(g.MacroAmbitoAttività,'°',''),'ì','i'''),'é','e'''),'à','a'''),'ò','o'''),'è','e'''),'ù','u'''), ''), " & _
            " isnull(replace(replace(replace(replace(replace(replace(replace(f.AmbitoAttività,'°',''),'ì','i'''),'é','e'''),'à','a'''),'ò','o'''),'è','e'''),'ù','u'''), ''), " & _
            " n.IDEnteSedeAttuazione, isnull(replace(replace(replace(replace(replace(replace(replace(N.Denominazione,'°',''),'ì','i'''),'é','e'''),'à','a'''),'ò','o'''),'è','e'''),'ù','u'''), ''), " & _
            " isnull(replace(replace(replace(replace(replace(replace(replace(p.Denominazione,'°',''),'ì','i'''),'é','e'''),'à','a'''),'ò','o'''),'è','e'''),'ù','u'''), ''), " & _
            " isnull(replace(replace(replace(replace(replace(replace(replace(x.Provincia,'°',''),'ì','i'''),'é','e'''),'à','a'''),'ò','o'''),'è','e'''),'ù','u'''), '')  as Provincia, " & _
            " isnull(replace(replace(replace(replace(replace(replace(replace(o.Indirizzo,'°',''),'ì','i'''),'é','e'''),'à','a'''),'ò','o'''),'è','e'''),'ù','u'''), '') + ', ' + o.Civico as Indirizzo, " & _
            " o.Cap, o.PrefissoTelefono + '/' + o.Telefono as Telefono, o.Email, k.StatoEnteSede, " & _
            " isnull(replace(replace(replace(replace(replace(replace(replace(ep.Cognome,'°',''),'ì','i'''),'é','e'''),'à','a'''),'ò','o'''),'è','e'''),'ù','u'''), ''), " & _
            " isnull(replace(replace(replace(replace(replace(replace(replace(ep.nome,'°',''),'ì','i'''),'é','e'''),'à','a'''),'ò','o'''),'è','e'''),'ù','u'''), ''), " & _
            " convert(varchar,isnull(ep.datanascita,''),3) as Datanascita," & _
            " isnull(replace(replace(replace(replace(replace(replace(replace(ruoli.Ruolo,'°',''),'ì','i'''),'é','e'''),'à','a'''),'ò','o'''),'è','e'''),'ù','u'''), ''), " & _
            " isnull(replace(replace(replace(replace(replace(replace(replace(ep.codicefiscale,'°',''),'ì','i'''),'é','e'''),'à','a'''),'ò','o'''),'è','e'''),'ù','u'''), ''),epr.identepersonaleruolo,a.nmesi,er.CodiceRegione as CodiceEnteRiferimentoSede " & _
            " FROM Attività a  " & _
            " INNER JOIN Enti b ON a.identepresentante = b.idente  " & _
            " INNER JOIN AttivitàEntiSediAttuazione m ON a.IdAttività = m.IdAttività  " & _
            " inner join AssociaEntePersonaleRuoliAttivitàEntiSediAttuazione aaaa " & _
            " on aaaa.idattivitàEntesedeattuazione=m.idattivitàEntesedeattuazione " & _
            " inner join entepersonaleruoli epr on epr.identepersonaleruolo=aaaa.identepersonaleruolo " & _
            " inner join ruoli on epr.idruolo=ruoli.idruolo " & _
            " inner join entepersonale ep on  ep.identepersonale=epr.identepersonale " & _
            " INNER JOIN EntiSediAttuazioni n ON m.IdEnteSedeAttuazione = n.IdEnteSedeAttuazione  " & _
            " INNER JOIN EntiSedi o ON n.IdEnteSede = o.IdEnteSede " & _
            " INNER JOIN ENTI ER ON O.IDENTE=er.idente " & _
            " INNER JOIN Comuni p ON o.IdComune = p.IdComune " & _
            " INNER JOIN Provincie x ON x.IdProvincia = p.IdProvincia " & _
            " LEFT JOIN bandiAttività h ON a.idbandoattività=h.idbandoattività  " & _
            " LEFT JOIN bando c ON c.idbando=h.idbando " & _
            " LEFT JOIN Statibando d ON c.idstatobando=d.idstatobando " & _
            " INNER JOIN StatiAttività e ON a.idstatoattività=e.idstatoattività " & _
            " INNER JOIN AmbitiAttività f ON f.idambitoattività=a.idambitoattività " & _
            " INNER JOIN MacroAmbitiAttività g ON f.IDMacroAmbitoAttività=g.IDMacroAmbitoAttività  " & _
            " INNER JOIN StatiEntiSedi k ON n.IdStatoEnteSede = k.IdStatoEnteSede " & _
            " INNER JOIN TipiProgetto ON a.IdTipoProgetto = TipiProgetto.IdTipoProgetto " & _
            " INNER JOIN AssociaProfiliTipiProgetto ON TipiProgetto.IdTipoProgetto = AssociaProfiliTipiProgetto.IdTipoProgetto " & _
            " INNER JOIN Profili ON AssociaProfiliTipiProgetto.IdProfilo = Profili.IdProfilo "
            '============================================================================================================================
            '====================================================30/09/2008==============================================================
            '=======MODIFICA EFFETTUATA DA Kronk (99 scimmie saltavano sul letto, una cadde in terra e si ruppe il cervelletto...)=======
            '=================AGGIUNTA JOIN SU PROFILO READ PER ABILITARE LEGGERE LE ABILITAZIONI ANCHE DELL'UTENTE READ=================
            '============================================================================================================================
            If Session("Read") <> "1" Then
                strsql = strsql & " INNER JOIN	AssociaUtenteGruppo ON Profili.IdProfilo = AssociaUtenteGruppo.IdProfilo "
            Else
                strsql = strsql & " INNER JOIN	AssociaUtenteGruppo ON Profili.IdProfilo = AssociaUtenteGruppo.IdProfiloRead "
            End If
            strsql = strsql & " LEFT JOIN BANDORICORSI ON a.IDBANDORICORSO = BANDORICORSI.IDBANDORICORSO " & _
            " LEFT JOIN StatiBandiAttività i ON h.idstatobandoattività=i.idstatobandoattività " & _
            "   WHERE epr.datafinevalidità is null and a.IdAttività Is Not null And a.identepresentante = " & Session("Idente") & " " & _
            " and AssociaUtenteGruppo.username='" & Replace(Session("Utente"), "'", "''") & "' "
            If Session("TipoUtente") = "E" Then
                strsql = strsql & " and CASE ISNULL(BANDORICORSI.IDBANDORICORSO,0) WHEN 0 THEN isnull(c.enteabilitato,1) ELSE isnull(BANDORICORSI.enteabilitato,1) END = 1 "
            End If
            If Request.QueryString("TipoProgetto") <> "0" And Not Request.QueryString("TipoProgetto") Is Nothing Then
                strsql = strsql & " and a.idTipoProgetto=" & Request.QueryString("TipoProgetto") & ""
            End If

            If CStr(Session("TipoUtente")) = "E" Then
                strsql = strsql & " and b.idente = '" & CStr(Session("IdEnte")) & "'"
            End If


            If DdlTipiProgetto.SelectedItem.Text <> "" Then
                strsql = strsql & " and a.idTipoProgetto=" & DdlTipiProgetto.SelectedValue & ""
            End If

            If CStr(Session("TipoUtente")) = "E" Then
                strsql = strsql & " and b.idente = '" & CStr(Session("IdEnte")) & "'"
            End If

            If txtDenominazioneEnte.Text <> "" And txtDenominazioneEnteSecondario.Text <> "" Then
                strsql = strsql & " and b.denominazione  like '" & ClsServer.NoApice(Trim(txtDenominazioneEnte.Text)) & "%'"
            End If
            If TxtCodPog.Text <> "" Then
                strsql = strsql & " and a.CodiceEnte  like '" & ClsServer.NoApice(Trim(TxtCodPog.Text)) & "%'"
            End If
            If ddlStatoAttivita.SelectedItem.Text <> "" Then
                strsql = strsql & "AND e.IdStatoAttività = " & ddlStatoAttivita.SelectedValue & " "
            End If

            If txtTitoloProgetto.Text <> "" Then
                strsql = strsql & "AND a.Titolo LIKE '" & ClsServer.NoApice(txtTitoloProgetto.Text) & "%' "
            End If

            If DdlBando.SelectedItem.Text <> "" Then
                strsql = strsql & " and c.bandobreve = '" & ClsServer.NoApice(DdlBando.SelectedItem.Text) & "'"
            End If
            If txtDenominazioneEnte.Text <> "" Then
                strsql = strsql & " and b.denominazione like '" & ClsServer.NoApice(Trim(txtDenominazioneEnte.Text)) & "%'"
            End If

            If ddlMaccCodAmAtt.SelectedItem.Text <> "" Then
                strsql = strsql & " and f.idmacroambitoattività=" & ddlMaccCodAmAtt.SelectedValue & ""
            End If

            If TxtCodPog.Text <> "" Then
                strsql = strsql & " and a.CodiceEnte  like '" & ClsServer.NoApice(Trim(TxtCodPog.Text)) & "%'"
            End If

            If ddlDurataProgetto.SelectedValue <> 0 Then
                strsql = strsql & " and a.nmesi =" & ddlDurataProgetto.SelectedValue
            End If
            If txtcomune.Text <> "" Then
                strsql = strsql & " and p.Denominazione  like '" & ClsServer.NoApice(Trim(txtcomune.Text)) & "%'"
            End If
            If txtProvincia.Text <> "" Then
                strsql = strsql & " and x.Provincia  like '" & ClsServer.NoApice(Trim(txtProvincia.Text)) & "%'"
            End If

            If ddlSegnalazioneSanzione.SelectedItem.Text <> "Tutti" Then
                strsql = strsql & " and isnull(a.SegnalazioneSanzione,0) =" & ddlSegnalazioneSanzione.SelectedValue & ""
            End If

            If ddlStatoValutazione.SelectedItem.Text <> "Tutti" Then
                strsql = strsql & " and isnull(a.StatoValutazione,0) =" & ddlStatoValutazione.SelectedValue & ""
            End If


            If ddlGiovaniMinoriOp.SelectedItem.Text <> "Tutti" Then
                strsql = strsql & " and isnull(a.GiovaniMinoriOpportunità,0) = " & ddlGiovaniMinoriOp.SelectedValue & ""
            End If
            If ddlFami.SelectedItem.Text <> "Tutti" Then
                strsql = strsql & " and isnull(a.Fami,0) = " & ddlFami.SelectedValue & ""
            End If
            '** agg. da s.c. il 27/11/2014 FiltroVisibilita
            strsql = strsql & " and TipiProgetto.MacroTipoProgetto like '" & Session("FiltroVisibilita") & "' "
            strsql = strsql & "ORDER BY 1,2,10,7"
            dtrSediAttuazione = ClsServer.CreaDatareader(strsql, Session("conn"))

            NomeUnivoco = vbNullString
            If dtrSediAttuazione.HasRows = False Then
                lblmessaggio.Text = lblmessaggio.Text & "Nessuna Sede di Attuazione."
            Else
                While dtrSediAttuazione.Read
                    If NomeUnivoco = vbNullString Then
                        If dtrSediAttuazione(0) = "0" Then
                            xPrefissoNome = Session("Utente")
                        Else
                            xPrefissoNome = dtrSediAttuazione(0)
                        End If
                        NomeUnivoco = xPrefissoNome & "ExpRisorseProg" & Year(Now) & Month(Now) & Day(Now) & Hour(Now) & Minute(Now) & Second(Now)
                        Writer = New StreamWriter(Server.MapPath("download") & "\" & NomeUnivoco & ".CSV")
                        xLinea = "Codice Progetto Interno;Codice Progetto;Titolo;Circolare;Settore;Area;CodiceSede;Sede;Provincia;Comune;Indirizzo;CAP;Telefono;E-Mail;Stato;Cognome;Nome;Data di Nascita;Ruolo;Codice fiscale;Codice Interno Risorse;Durata (mesi);CodiceEnteRiferimentoSede;"
                        Writer.WriteLine(xLinea)
                    End If
                    xLinea = vbNullString
                    '---salto il primo e l'ulimo elemento (nome file & data usata solo per order by)
                    For i = 1 To dtrSediAttuazione.FieldCount - 1
                        If IsDBNull(dtrSediAttuazione(i)) = True Then
                            xLinea = xLinea & ";"
                        Else
                            xLinea = xLinea & ClsUtility.FormatExport(dtrSediAttuazione(i)) & ";"
                        End If
                    Next i
                    Writer.WriteLine(xLinea)
                End While
                hlApriElencoRisorseProgetto.Visible = True
                hlApriElencoRisorseProgetto.NavigateUrl = "download\" & NomeUnivoco & ".CSV"
                Writer.Close()
                Writer = Nothing
            End If

            dtrSediAttuazione.Close()
            dtrSediAttuazione = Nothing

        Catch ex As Exception
            ' ì(lblErr.Text = lblErr.Text & "Errore durante l'esportazione delle Risorse.")
            If Not Writer Is Nothing Then
                Writer.Close()
                Writer = Nothing
            End If
            If Not dtrSediAttuazione Is Nothing Then
                dtrSediAttuazione.Close()
                dtrSediAttuazione = Nothing
            End If
        End Try

    End Sub

    Private Sub StampaElencoCSV()
        EsportaProgetti()
        EsportaSediAttuazione()
        EsportaRisorse()
    End Sub

    Protected Sub CmdEsporta_Click(ByVal sender As Object, ByVal e As EventArgs) Handles CmdEsporta.Click
        CmdEsporta.Visible = False
        StampaCSV(Session("DtbRicerca"))

    End Sub

    Protected Sub CmdEsportaElenco_Click(ByVal sender As Object, ByVal e As EventArgs) Handles CmdEsportaElenco.Click
        CmdEsportaElenco.Visible = False
        StampaElencoCSV()
    End Sub


    Protected Sub btnSeleziona_Click(ByVal sender As Object, ByVal e As CommandEventArgs)
        'Dim dt As New DataTable
        'dt = Session("MydatasetGriglia")

        'Dim indiceTotale As Integer
        'indiceTotale = dt.Rows.Count
        Dim i As Integer

        For i = 0 To dgRisultatoRicerca.Items.Count
            If dgRisultatoRicerca.Items(i).Cells(8).Text = e.CommandArgument.ToString() Then
                Exit For
            End If
        Next
        If Session("tipoutente") <> "E" Then
            'Response.Redirect("TabProgetti.aspx?IdAttivita=" & CInt(e.Item.Cells(8).Text) & "&Nazionale=" & CInt(e.Item.Cells(11).Text) & "&Modifica=" & CInt(e.Item.Cells(10).Text) & "")
            Session("IdEnte") = Convert.ToInt32(e.CommandArgument("idente")).ToString()
            Session("denominazione") = dgRisultatoRicerca.Items(i).Cells(3).Text
        End If
        Dim strVengoda As String
        'strVengoda = "Response.Redirect(""TabProgetti.aspx?Nazionale=""" & e.Item.Cells(11).Text & """&Modifica=""" & e.Item.Cells(10).Text & """&IdAttivita=""" & e.Item.Cells(8).Text & ")"

        strVengoda = "Response.Redirect(""" & ClsUtility.TrovaAlboProgetto(Request.QueryString("IdAttivita"), Session("Conn")) & "?Nazionale=""" & dgRisultatoRicerca.Items(i).Cells(11).Text & """&Modifica=""" & dgRisultatoRicerca.Items(i).Cells(10).Text & """&IdAttivita=""" & dgRisultatoRicerca.Items(i).Cells(8).Text & ")"
        'strVengoda = "Response.Redirect(""TabProgetti.aspx?Nazionale=""" & dgRisultatoRicerca.Items(i).Cells(11).Text & """&Modifica=""" & dgRisultatoRicerca.Items(i).Cells(10).Text & """&IdAttivita=""" & dgRisultatoRicerca.Items(i).Cells(8).Text & ")"

        'Response.Redirect("TabProgetti.aspx?VengoDa=" & strVengoda & "&Nazionale=" & dgRisultatoRicerca.SelectedItem.Cells(9).Text & "&Modifica=" & dgRisultatoRicerca.SelectedItem.Cells(8).Text & "&IdAttivita=" & dgRisultatoRicerca.SelectedItem.Cells(6).Text & "")
        'Response.Redirect("TabProgetti.aspx?Nazionale=" & e.Item.Cells(11).Text & "&Modifica=" & e.Item.Cells(10).Text & "&IdAttivita=" & e.Item.Cells(8).Text & "")

        Response.Redirect(ClsUtility.TrovaAlboProgetto(Request.QueryString("IdAttivita"), Session("Conn")) & "?Nazionale=" & dgRisultatoRicerca.Items(i).Cells(11).Text & "&Modifica=" & dgRisultatoRicerca.Items(i).Cells(10).Text & "&IdAttivita=" & dgRisultatoRicerca.Items(i).Cells(8).Text & "")
        'Response.Redirect("TabProgetti.aspx?Nazionale=" & dgRisultatoRicerca.Items(i).Cells(11).Text & "&Modifica=" & dgRisultatoRicerca.Items(i).Cells(10).Text & "&IdAttivita=" & dgRisultatoRicerca.Items(i).Cells(8).Text & "")
    End Sub

    Protected Sub btnaccettazione_Click(ByVal sender As Object, ByVal e As CommandEventArgs)

        Dim i As Integer

        For i = 0 To dgRisultatoRicerca.Items.Count
            If dgRisultatoRicerca.Items(i).Cells(8).Text = e.CommandArgument.ToString() Then
                Exit For
            End If
        Next

        If Session("tipoutente") <> "E" Then
            Session("idente") = CInt(dgRisultatoRicerca.Items(i).Cells(9).Text)
        End If

        'Verifico se è possibile effettuare la valutazione del Progetto
        strquery = "select attività.idattività," & _
        " case isnull(convert(smallint,statibandiAttività.Attivo),-1)when -1 then 0 else convert(smallint,statibandiAttività.Attivo) end as attivoStatoAttBando," & _
        " case  isnull(convert(smallint,statibando.invalutazione),-1) when -1 then 0 else convert(smallint,statibando.invalutazione) end as invalutazione," & _
        " statiattività.idstatoattività," & _
        " statiattività.dagraduare,statiattività.daVAlutare, statiattività.respinta " & _
        " from attività " & _
        " inner join enti on attività.identepresentante=enti.idente " & _
        " left join bandiAttività  on attività.idbandoattività=bandiAttività.idbandoattività " & _
        " left join  statibandiAttività on bandiAttività.idstatobandoattività=statibandiAttività.idstatobandoattività" & _
        " left join bando  on bando.idbando=bandiAttività.idbando " & _
        " left join statibando  on bando.idstatobando=statibando.idstatobando " & _
        " inner join statiattività  on attività.idstatoattività=statiattività.idstatoattività " & _
        " where attività.idattività=" & CInt(e.CommandArgument.ToString()) & ""
        If Not dtrGenerico Is Nothing Then
            dtrGenerico.Close()
            dtrGenerico = Nothing
        End If
        dtrGenerico = ClsServer.CreaDatareader(strquery, Session("conn"))
        dtrGenerico.Read()
        If dtrGenerico("invalutazione") = 1 And dtrGenerico("attivoStatoAttBando") = 1 Then
            If Not dtrGenerico Is Nothing Then
                dtrGenerico.Close()
                dtrGenerico = Nothing
            End If
            'Response.Redirect("WfrmAlbero.aspx?idattivita=" & CInt(e.Item.Cells(8).Text) & "&tipologia=Progetti&Arrivo=ricercaprogetti.aspx")
            Response.Redirect("assegnazionevincoliprogetti.aspx?idattivita=" & CInt(e.CommandArgument.ToString()) & "&tipologia=Progetti&Vengoda=" & Request.QueryString("VengoDa"))
        Else
            'Messaggio
            lblmessaggio.Text = "Non è possibile effettuare l'Accettazione del Progetto."
            If Not dtrGenerico Is Nothing Then
                dtrGenerico.Close()
                dtrGenerico = Nothing
            End If
            Exit Sub
        End If
    End Sub

    Protected Sub btnvalutazione_Click(ByVal sender As Object, ByVal e As CommandEventArgs)
        Dim i As Integer

        For i = 0 To dgRisultatoRicerca.Items.Count
            If dgRisultatoRicerca.Items(i).Cells(8).Text = e.CommandArgument.ToString() Then
                Exit For
            End If
        Next

        ' 'Verifico se è possibile effettuare la valutazione del Progetto
        strquery = "select attività.idattività," & _
        " case  isnull(convert(smallint,statibando.invalutazione),-1) when -1 then 0 else convert(smallint,statibando.invalutazione) end as invalutazione," & _
        " statiattività.idstatoattività," & _
        " statiattività.dagraduare,statiattività.Attiva, statiattività.respinta " & _
        " from attività " & _
        " inner join enti on attività.identepresentante=enti.idente " & _
        " left join bandiAttività  on attività.idbandoattività=bandiAttività.idbandoattività " & _
        " left join bando  on bando.idbando=bandiAttività.idbando " & _
        " left join statibando  on bando.idstatobando=statibando.idstatobando " & _
        " inner join statiattività  on attività.idstatoattività=statiattività.idstatoattività " & _
        " where attività.idattività=" & CInt(e.CommandArgument.ToString()) & ""
        If Not dtrGenerico Is Nothing Then
            dtrGenerico.Close()
            dtrGenerico = Nothing
        End If
        dtrGenerico = ClsServer.CreaDatareader(strquery, Session("conn"))
        dtrGenerico.Read()
        'mod il 23/09/2011 è possibile entrare nella scheda di valutazione solo x i progetti che sono nel seguente stato:
        'ATTIVO,TERMINATO,NON ATTIVABILE,IN ATTESA DI GRADUATORIA,CHIUSO PER MANCANZA RICHIESTE
        If dtrGenerico("idstatoattività") = 1 Or dtrGenerico("idstatoattività") = 2 Or dtrGenerico("idstatoattività") = 6 Or dtrGenerico("idstatoattività") = 9 Or dtrGenerico("idstatoattività") = 12 Then
            If Not dtrGenerico Is Nothing Then
                dtrGenerico.Close()
                dtrGenerico = Nothing
            End If
            Context.Items.Add("idprogetto", CInt(e.CommandArgument.ToString()))
            Context.Items.Add("Pagina", "WfrmValutazioneQual.aspx")
            Server.Transfer("WfrmValutazioneQual.aspx")
        Else
            'Messaggio
            lblmessaggio.Text = "Non è possibile effettuare la valutazione del Progetto."
            If Not dtrGenerico Is Nothing Then
                dtrGenerico.Close()
                dtrGenerico = Nothing
            End If
            Exit Sub
        End If

    End Sub

    Protected Sub cmdChiudi_Click(ByVal sender As Object, ByVal e As EventArgs) Handles CmdChiudi.Click
        Response.Redirect("WfrmMain.aspx")
    End Sub
    Private Sub CaricaComboDurata()
        'variabile stringa locale per costruire la query per i settori
        Dim strSql As String

        If Not dtrGenerico Is Nothing Then
            dtrGenerico.Close()
            dtrGenerico = Nothing
        End If
        strSql = " SELECT 0 as NumMesi,'' as nmesi UNION "
        strSql &= " SELECT nmesi as NumMesi ,convert(varchar,nmesi) as nmesi FROM TipiProgettoDettaglio "

        dtrGenerico = ClsServer.CreaDatareader(strSql, Session("conn"))
        If dtrGenerico.HasRows = True Then
            ddlDurataProgetto.DataSource = dtrGenerico
            ddlDurataProgetto.DataTextField = "nmesi"
            ddlDurataProgetto.DataValueField = "NumMesi"
            ddlDurataProgetto.DataBind()
        End If
        If Not dtrGenerico Is Nothing Then
            dtrGenerico.Close()
            dtrGenerico = Nothing
        End If
    End Sub
End Class