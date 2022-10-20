Imports System.IO

Public Class WfrmRicercaOreFormazioneVolontari
    Inherits System.Web.UI.Page
    Dim dtrGenerico As SqlClient.SqlDataReader
    Dim strquery As String
    Public sEsitoRicerca As Boolean

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        If Session("TipoUtente") = "U" Then
            Call TuttaPaginaSess()
        End If

        '***Generata da Guido Testa in data:21/07/06
        If Not Session("LogIn") Is Nothing Then
            If Session("LogIn") = False Then 'verifico validità log-in
                Response.Redirect("LogOn.aspx")
            End If
        Else
            Response.Redirect("LogOn.aspx")
        End If

        If Request.QueryString("VengoDa") = "Ricerca" Then
            lgContornoPagina.InnerText = "Ricerca progetti per conferma e stampa Modulo ""F"""
            lblTitolo.Text = "Ricerca progetti per conferma e stampa Modulo ""F"""

            divConfermato.Visible = True
            divOreFormazioneCaricate.Visible = False

        Else
            lgContornoPagina.InnerText = "Ricerca progetti per caricamento ore formazione volontari"
            lblTitolo.Text = "Ricerca progetti per caricamento ore formazione volontari"

            divConfermato.Visible = False
            divOreFormazioneCaricate.Visible = True
        End If

        If (Session("TipoUtente") = "U" Or Session("TipoUtente") = "R") Then
            divCodiceEnte.Visible = True
        Else
            divCodiceEnte.Visible = False
        End If

        If Request.QueryString("VediEnte") = 0 Then
            divStato.Visible = True
            divRimborso.Visible = True
        Else
            divStato.Visible = False
            divRimborso.Visible = False
        End If

        If IsPostBack = False Then
            If Request.QueryString("VengoDa") = "Ricerca" Then      'estrazione questionari
                optOreTutte.Checked = False
                optOreTutte.Enabled = False
                optOreSi.Enabled = False
                optOreNo.Enabled = False
            End If
            'scarico la session della datatable per la ricerca così che in una nuova pagina non verrà 
            'erroneamente visualizzato alcun item
            Session("DtbRicVol") = Nothing

            'richiamo sub dove popolo combo
            CaricaPrima()

            lblDenEnte.Visible = False
            txtDenominazioneEnte.Visible = False
            If Len(Session("CodiceRegioneEnte")) > 5 Then
                txtCodReg.Text = Mid(Session("CodiceRegioneEnte"), 2, 7)
            End If
            'txtCodReg.Text = Mid(Session("CodiceRegioneEnte"), 2, 7)
            If Not Request.QueryString("TipoFormazioneGenerale") Is Nothing Then
                ddlTipoFormazioneGenerale.SelectedValue = Request.QueryString("TipoFormazioneGenerale")
                ddlTipoFormazioneGenerale.Enabled = False
            End If
        End If

        'Controllo sul tipo di utente
        If Session("TipoUtente") = "E" Then
            'Disabilito la possibilità di selezionare la denominazione dell'ente
            lblDenEnte.Visible = False
            txtDenominazioneEnte.Visible = False

            divCodiceEnte.Visible = False
            'lblCodReg.Visible = False
            'txtCodReg.Visible = False
        Else
            lblDenEnte.Visible = True
            txtDenominazioneEnte.Visible = True

            divCodiceEnte.Visible = True
            'lblCodReg.Visible = True
            'txtCodReg.Visible = True
        End If

        If Request.QueryString("VediEnte") = 0 Then     'UNSC/Regione
            lblStato.Visible = True
            ddlStato.Visible = True
            LblRimborso.Visible = True
            ddlRimborso.Visible = True
        Else                                            'Ente
            lblStato.Visible = False
            ddlStato.Visible = False
            LblRimborso.Visible = False
            ddlRimborso.Visible = False
            lblDenEnte.Visible = False

            divCodiceEnte.Visible = False
            'lblCodReg.Visible = False
            'txtCodReg.Visible = False
        End If

    End Sub

    Private Sub CaricaPrima()
        '***Carico combo settore
        ddlMaccCodAmAtt.DataSource = MakeParentTable("select idmacroambitoattività, codifica + ' - ' + MacroAmbitoAttività as Macro from macroambitiattività")
        ddlMaccCodAmAtt.DataTextField = "ParentItem"
        ddlMaccCodAmAtt.DataValueField = "id"
        ddlMaccCodAmAtt.DataBind()

        '***Carico combo area intervento
        ddlCodAmAtt.Items.Add("")
        ddlCodAmAtt.Enabled = False

        '***carico combo stati attivita
        ddlStatoAttivita.DataSource = MakeParentTable("select idstatoattività, statoattività from statiattività where idstatoattività < 3 ")
        ddlStatoAttivita.DataTextField = "ParentItem"
        ddlStatoAttivita.DataValueField = "id"
        ddlStatoAttivita.DataBind()

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
        DdlBando.DataSource = MakeParentTable(strsql)
        DdlBando.DataTextField = "ParentItem"
        DdlBando.DataValueField = "id"
        DdlBando.DataBind()

        DdlTipiProgetto.DataSource = ClsServer.CreaDataTable("Select idTipoProgetto,Descrizione from TipiProgetto  Where MacroTipoProgetto like '" & Session("FiltroVisibilita") & "'", True, Session("conn"))
        DdlTipiProgetto.DataTextField = "Descrizione"
        DdlTipiProgetto.DataValueField = "idTipoProgetto"
        DdlTipiProgetto.DataBind()
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

    Private Sub cmdChiudi_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdChiudi.Click
        Response.Redirect("WfrmImportOreVolontari.aspx")
    End Sub

    Private Sub ddlMaccCodAmAtt_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlMaccCodAmAtt.SelectedIndexChanged
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

    Private Sub cmdRicerca_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdRicerca.Click
        hlVolontari.Visible = False
        '*** 'salvo parametri per controllo link pagine
        If ddlTipoFormazioneGenerale.SelectedItem.Text = "" Then
            lblmessaggio.Visible = True
            lblmessaggio.Text = "Selezionare una tipologia di Erogazione della Formazione."
            Exit Sub
        End If
        If Session("TipoUtente") = "E" Then
            If DdlBando.SelectedItem.Text <> "" Then
                lblmessaggio.Text = ""
                txtTitoloProgetto1.Value = txtTitoloProgetto.Text
                txtDenominazioneEnte1.Value = txtDenominazioneEnte.Text
                ddlMaccCodAmAtt1.Value = ddlMaccCodAmAtt.SelectedIndex
                ddlCodAmAtt1.Value = ddlCodAmAtt.SelectedIndex
                ddlStatoAttivita1.Value = ddlStatoAttivita.SelectedIndex
                dgRisultatoRicerca.CurrentPageIndex = 0

                If controlliServer() = True Then
                    EseguiRicerca(0)
                End If

            Else
                lblmessaggio.Visible = True
                lblmessaggio.Text = "Occorre selezionare una circolare."
            End If
        Else
            lblmessaggio.Text = ""
            txtTitoloProgetto1.Value = txtTitoloProgetto.Text
            txtDenominazioneEnte1.Value = txtDenominazioneEnte.Text
            ddlMaccCodAmAtt1.Value = ddlMaccCodAmAtt.SelectedIndex
            ddlCodAmAtt1.Value = ddlCodAmAtt.SelectedIndex
            ddlStatoAttivita1.Value = ddlStatoAttivita.SelectedIndex
            dgRisultatoRicerca.CurrentPageIndex = 0

            If controlliServer() = True Then
                EseguiRicerca(0)
            End If

        End If
        dgRisultatoRicerca.Columns(23).Visible = ClsUtility.ForzaPresenzaSanzioneProgetto(Session("Utente"), Session("conn"))
    End Sub

    Private Sub EseguiRicerca(ByVal bytVerifica As Byte, Optional ByVal bytpage As Integer = 0)
        '*****************************************************************************************+
        'AUTORE: Guido Testa 
        'DATA: 21/04/2006
        'DESCRIZONE: effetuo la ricerca dei progetti per poi esportare il file csv dei volontari di ogni singolo progetto
        'mod. il 22/05/2007 DA SIMONA CORDELLA
        'condizione per le colonne N° Vol e N° VOl Rimb con campo EscludiRimborso =0
        Dim Mydataset As New DataSet
        Dim blnForzaPresenzaVerifica As Boolean

        dgRisultatoRicerca.Visible = True
        ' Mydataset.Dispose()
        blnForzaPresenzaVerifica = ClsUtility.ForzaPresenzaVerificaProgetto(Session("Utente"), Session("conn"))

        strquery = "select distinct b.denominazione, "
        If Session("TipoUtente") = "U" Then
            If blnForzaPresenzaVerifica Then
                strquery = strquery & " CASE a.IdRegioneCompetenza WHEN 22 THEN"
                strquery = strquery & "     CASE isnull(ProgettoSottopostoVerifica,0) when 0 then (a.titolo + ' [' + a.codiceente + ']') else  (a.titolo + ' [' + a.codiceente + ']')  + ' ' + '<img src=images/Alert.png onclick=VisualizzaVerifiche('''+ convert(varchar, a.codiceente) + ''') STYLE=cursor:hand title=''Progetto Sottoposto a Verifica'' border=0>'end  "
                strquery = strquery & " ELSE a.titolo + ' [' + a.codiceente + ']' END as titolo , "
            Else
                strquery = strquery & " (a.titolo + ' [' + a.codiceente + ']') as titolo, "
            End If
        Else
            strquery = strquery & " (a.titolo + ' [' + a.codiceente + ']') as titolo, "
        End If
        strquery = strquery & " (isNull(a.NumeroPostiNoVittoNoAlloggio,0)+ isnull(a.NumeroPostiVittoAlloggio,0) + isNull(a.NumeroPostiVitto,0)) as VolRic, c.bando," & _
        " g.macroambitoattività + ' / ' + f.ambitoattività as Ambito," & _
        " a.idattività,b.idente," & _
        " '0' as selezionato, a.IdTipoProgetto,e.statoattività,  " & _
        " (SELECT     isnull(SUM(entità.OreFormazione), 0) " & _
        " FROM entità INNER JOIN " & _
        " attivitàentità ON entità.IDEntità = attivitàentità.IDEntità INNER JOIN " & _
        " attivitàentisediattuazione ON  attivitàentità.IDAttivitàEnteSedeAttuazione = attivitàentisediattuazione.IDAttivitàEnteSedeAttuazione INNER JOIN " & _
        " attività ON attivitàentisediattuazione.IDAttività = attività.IDAttività " & _
        " WHERE (attività.IDAttività = a.IDAttività)) AS OreFormazione, "
        'SubQuery che mi consente di metetre il link al questionario solo ai progetti che lo hanno eseguito
        'strquery = strquery & " (SELECT CASE WHEN (SELECT     IDProgetto " & _
        '"   FROM QuestionarioStoricoProgetti WHERE IDProgetto = a.IDAttività) <> '' THEN "
        'strquery = strquery & " '<img src=images/xordini.gif onclick=VisualizzaQuest(' + convert(varchar,a.idattività) + ') STYLE=cursor:hand title=Questionario border=0> ' "
        'strquery = strquery & " else '' End) as Quest, "
        'fine subquery
        strquery = strquery & " (SELECT CASE WHEN (SELECT  COUNT(DISTINCT entità.CodiceVolontario) " & _
                " FROM StatiEntità INNER JOIN entità INNER JOIN attivitàentità ON entità.IDEntità = attivitàentità.IDEntità INNER JOIN " & _
                " attivitàentisediattuazione ON attivitàentità.IDAttivitàEnteSedeAttuazione = attivitàentisediattuazione.IDAttivitàEnteSedeAttuazione " & _
                " INNER JOIN attività ON attivitàentisediattuazione.IDAttività = attività.IDAttività INNER JOIN " & _
                " enti ON attività.IDEntePresentante = enti.IDEnte ON StatiEntità.IDStatoEntità = entità.IDStatoEntità " & _
                " WHERE attività.IDAttività = a.IDAttività And (StatiEntità.InServizio = 1 OR StatiEntità.Sospeso = 1  OR  StatiEntità.chiuso = 1)) <> 0 THEN "
        strquery = strquery & " '<img src=images/Icona_Volontario_small.png onclick=VisualizzaVol(' + convert(varchar,a.idattività) + ') STYLE=cursor:hand title=Volontari border=0>' "
        strquery = strquery & " else '' End) as LinkVol, "
        'SubQuery estrazione volontari in servizio e terminati durante il sevizio
        strquery = strquery & " (SELECT COUNT(DISTINCT entità.CodiceVolontario) " & _
                " FROM StatiEntità INNER JOIN entità INNER JOIN attivitàentità ON entità.IDEntità = attivitàentità.IDEntità INNER JOIN " & _
                " attivitàentisediattuazione ON attivitàentità.IDAttivitàEnteSedeAttuazione = attivitàentisediattuazione.IDAttivitàEnteSedeAttuazione " & _
                " INNER JOIN attività ON attivitàentisediattuazione.IDAttività = attività.IDAttività INNER JOIN " & _
                " enti ON attività.IDEntePresentante = enti.IDEnte ON StatiEntità.IDStatoEntità = entità.IDStatoEntità " & _
                " WHERE attività.IDAttività = a.IDAttività And (StatiEntità.InServizio = 1 OR StatiEntità.Sospeso = 1  OR StatiEntità.Chiuso = 1) and attivitàentità.EscludiFormazione=0)  AS Volontari,  "
        'SubQuery estrazione volontari rinborsabili in servizio e terminati durante il sevizio
        strquery = strquery & " (SELECT  COUNT(DISTINCT entità.CodiceVolontario) " & _
                        " FROM StatiEntità INNER JOIN entità INNER JOIN attivitàentità ON entità.IDEntità = attivitàentità.IDEntità INNER JOIN " & _
                        " attivitàentisediattuazione ON attivitàentità.IDAttivitàEnteSedeAttuazione = attivitàentisediattuazione.IDAttivitàEnteSedeAttuazione " & _
                        " INNER JOIN attività ON attivitàentisediattuazione.IDAttività = attività.IDAttività INNER JOIN ATTIVITàFORMAZIONEGENERALE ON ATTIVITà.IDATTIVITà = ATTIVITàFORMAZIONEGENERALE.IDATTIVITà INNER JOIN " & _
                        " BANDIATTIVITà ON ATTIVITà.IDBANDOATTIVITà = BANDIATTIVITà.IDBANDOATTIVITà INNER JOIN BANDO ON BANDIATTIVITà.IDBANDO = BANDO.IDBANDO INNER JOIN enti ON attività.IDEntePresentante = enti.IDEnte ON StatiEntità.IDStatoEntità = entità.IDStatoEntità " & _
                        " WHERE attività.IDAttività = a.IDAttività And entità.OreFormazione >= CASE WHEN BANDO.REVISIONEFORMAZIONE = 0 THEN 30 ELSE ATTIVITàFORMAZIONEGENERALE.DURATAFORMAZIONEGENERALE END and attivitàentità.EscludiFormazione=0 " & _
                        " And (StatiEntità.InServizio = 1 OR StatiEntità.Sospeso = 1  OR StatiEntità.Chiuso = 1))  AS VolRimborso,  "
        strquery = strquery & " '<img src=images/cronologia_small.png Width=30 Height=30 onclick=VisualizzaCrono(' + convert(varchar,a.idattività) + ') STYLE=cursor:hand  title=Cronologia border=0>' as CronoStampe, QuestionarioStoricoProgetti.NumeroVolontari as VolQuest, DurataFormazioneGenerale as OrePrev, Convert(VarChar,AttivitàFormazioneGenerale.dataproroga,103) as dataproroga,  "
        strquery = strquery & "(CASE AttivitàFormazioneGenerale.ChiedeRimborso WHEN 0  THEN 'No' else 'Si' end) as Chiederimborso, "
        strquery = strquery & "(case convert(varchar,c.PianificazioneFormazione) when '0' then 'Non Necessario' else case isnull(ltrim(rtrim(AttivitàFormazioneGenerale.sedecorso)),'') WHEN ''  THEN 'No' else 'Si' end end ) as DatiPianificazione, "
        strquery = strquery & " (case TipiProgetto.nazionebase when 0 then 'Estero' else 'Italia' end) as nazionebase,c.idbando, "
        strquery = strquery & " isNull(c.RimborsoFormazioneItalia,0) AS RimborsoFormazioneItalia, Isnull(c.RimborsoFormazioneEstero,0) AS RimborsoFormazioneEstero, "
        strquery = strquery & " (CASE isNull(a.SegnalazioneSanzione,0) WHEN 0  THEN 'No' else '<img src=images/Anomalie.bmp onclick=VisualizzaSanzioneProg('+ convert(varchar, a.IDAttività) + ','+ convert(varchar,a.IDEntePresentante) + ') STYLE=cursor:hand title=Sanzione border=0>' end) as PresenzaSanzione "
        'fine subquery
        strquery = strquery & ", case when AttivitàFormazioneGenerale.statoformazione >=2 then dbo.formatodata(AttivitàFormazioneGenerale.dataapprovazione) else '' end as DataApprovazione "

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
        If UCase(Me.TemplateSourceDirectory) <> "/HELIOSREAD" Then
            strquery = strquery & " INNER JOIN	AssociaUtenteGruppo ON Profili.IdProfilo = AssociaUtenteGruppo.IdProfilo "
        Else
            strquery = strquery & " INNER JOIN	AssociaUtenteGruppo ON Profili.IdProfilo = AssociaUtenteGruppo.IdProfiloRead "
        End If

        strquery = strquery & " INNER JOIN" & _
        " statiattività e ON a.IDStatoAttività = e.IDStatoAttività INNER JOIN" & _
        " ambitiattività f ON f.IDAmbitoAttività = a.IDAmbitoAttività INNER JOIN" & _
        " macroambitiattività g ON f.IDMacroAmbitoAttività = g.IDMacroAmbitoAttività ON attivitàentisediattuazione.IDAttività = a.IDAttività LEFT OUTER JOIN" & _
        " statiBandiAttività i ON h.IdStatoBandoAttività = i.IdStatoBandoAttività " & _
        " INNER JOIN AttivitàFormazioneGenerale On a.idattività = AttivitàFormazioneGenerale.idattività " & _
        " LEFT OUTER JOIN QuestionarioStampaProgetti On QuestionarioStampaProgetti.IDAttività = a.IDAttività " & _
        " LEFT OUTER JOIN QuestionarioStoricoProgetti On QuestionarioStoricoProgetti.IdBandoAttività = h.IdBandoAttività and isnull(QuestionarioStoricoProgetti.TipoFormazioneGenerale,1) = AttivitàFormazioneGenerale.TipoFormazioneGenerale " & _
        " WHERE a.idattività is not null and e.idstatoattività in (1,2) "

        'imposto eventuali parametri
        If ddlStatoAttivita.SelectedItem.Text <> "" Then
            strquery = strquery & " and e.idstatoattività=" & ddlStatoAttivita.SelectedValue & ""
        End If

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
            If Session("TipoUtente") = "U" Then
                strquery = strquery & " and c.bandobreve like '" & ClsServer.NoApice(DdlBando.SelectedItem.Text) & "%'"
            Else
                strquery = strquery & " and c.bandobreve = '" & ClsServer.NoApice(DdlBando.SelectedItem.Text) & "'"
            End If
        Else
            'prendo solo i bandi con il campo FormazioneGenerale=1
            'strquery = strquery & " and c.bandobreve in (Select bandobreve From bando Where FormazioneGenerale=1)"
            strquery = strquery & " and c.FormazioneGenerale=1 "
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

        If ddlMaccCodAmAtt.SelectedItem.Text = "" And ddlCodAmAtt.SelectedItem.Text = "" Then

        Else
            If ddlCodAmAtt.SelectedItem.Text <> "" Then
                strquery = strquery & " and f.idambitoattività=" & ddlCodAmAtt.SelectedValue & ""
            Else
                strquery = strquery & " and f.idmacroambitoattività=" & ddlMaccCodAmAtt.SelectedValue & ""
            End If
        End If

        If Request.QueryString("VediEnte") = 0 Then     'UNSC/Regione
            If ddlStato.SelectedValue <> 0 Then         '<> da Tutti filtro per stato
                strquery = strquery & " and AttivitàFormazioneGenerale.StatoFormazione=" & ddlStato.SelectedValue & ""
            End If
            If txtRif.Text <> "" Then
                strquery = strquery & " and QuestionarioStampaProgetti.IdCronologiaStampa=" & txtRif.Text & ""
            End If
            If ddlRimborso.SelectedValue <> 2 Then '<> da Tutti filtro per stato 
                strquery = strquery & " and AttivitàFormazioneGenerale.ChiedeRimborso=" & ddlRimborso.SelectedValue & ""
            End If
        End If
        'Aggiunto il 03/04/2013 da sc
        If ddlSegnalazioneSanzione.SelectedItem.Text <> "Tutti" Then
            strquery = strquery & " and isnull(a.SegnalazioneSanzione,0) =" & ddlSegnalazioneSanzione.SelectedValue
        End If
        'se vengo dalla ricerca dei questionari forzo il  caricamento dei progetti con ore formazione
        If Request.QueryString("VengoDa") = "Ricerca" Then      'estrazione questionari            
            If Request.QueryString("VediEnte") = 0 Then     'UNSC/Regione
                optOreTutte.Checked = True
            Else
                optOreSi.Checked = True
            End If
        End If

        If optOreSi.Checked = True Then
            strquery = strquery & " and (SELECT     isnull(SUM(entità.OreFormazione), 0) " & _
                " FROM entità INNER JOIN " & _
                " attivitàentità ON entità.IDEntità = attivitàentità.IDEntità INNER JOIN " & _
                " attivitàentisediattuazione ON  attivitàentità.IDAttivitàEnteSedeAttuazione = attivitàentisediattuazione.IDAttivitàEnteSedeAttuazione INNER JOIN " & _
                " attività ON attivitàentisediattuazione.IDAttività = attività.IDAttività " & _
                " WHERE (attività.IDAttività = a.IDAttività)) > 0 "
        ElseIf optOreNo.Checked = True Then
            strquery = strquery & " and (SELECT     isnull(SUM(entità.OreFormazione), 0) " & _
                " FROM entità INNER JOIN " & _
                " attivitàentità ON entità.IDEntità = attivitàentità.IDEntità INNER JOIN " & _
                " attivitàentisediattuazione ON  attivitàentità.IDAttivitàEnteSedeAttuazione = attivitàentisediattuazione.IDAttivitàEnteSedeAttuazione INNER JOIN " & _
                " attività ON attivitàentisediattuazione.IDAttività = attività.IDAttività " & _
                " WHERE (attività.IDAttività = a.IDAttività)) = 0 "
        End If
        'aggiunto il 14/04/2015 da s.c.
        strquery = strquery & " and AttivitàFormazioneGenerale.TipoFormazioneGenerale = " & ddlTipoFormazioneGenerale.SelectedValue & " "
        'filtrovisibilita 02/12/2014 da s.c.
        strquery = strquery & " and TipiProgetto.Macrotipoprogetto like '" & Session("FiltroVisibilita") & "'"

        strquery = strquery & " and AssociaUtenteGruppo.username='" & Replace(Session("Utente"), "'", "''") & "'  ORDER BY e.StatoAttività, b.Denominazione,Titolo"

        Mydataset = ClsServer.DataSetGenerico(strquery, Session("conn"))
        dgRisultatoRicerca.DataSource = Mydataset
        dgRisultatoRicerca.DataBind() 'valorizzo griglia

        Session("DtbRicVol") = Mydataset.Tables(0)

        If dgRisultatoRicerca.Items.Count = 0 Then 'se la griglia e vuota la nascondo
            lblmessaggio.Text = "La ricerca non ha prodotto alcun risultato"
            dgRisultatoRicerca.Visible = False
        Else
            chkSelDesel.Visible = True
            'ricerca per esportazione
            If Request.QueryString("VengoDa") <> "Ricerca" Then
                Call ColoraCelle()
            Else
                'ricerca per stampa modulo
                Call ColoraCelleStati()
            End If
        End If

        If dgRisultatoRicerca.Items.Count > 0 Then
            chkSelDesel.Visible = True
            If Request.QueryString("VengoDa") = "Ricerca" Then      'estrazione questionari
                CmdEsporta.Visible = False

                If Request.QueryString("VediEnte") = 0 Then     'UNSC/Regione
                    imgProroga.Visible = True
                    imgApprova.Visible = True
                    imgPagamento.Visible = True
                    ImaFuoriTermine.Visible = True
                    imgRespingi.Visible = True
                    lblStato.Visible = True
                    ddlStato.Visible = True
                    dgRisultatoRicerca.Columns(4).Visible = True        'dataapprovazione
                    dgRisultatoRicerca.Columns(11).Visible = True       'questionario
                    dgRisultatoRicerca.Columns(12).Visible = True       'volontari questionario
                    dgRisultatoRicerca.Columns(13).Visible = True       'Ore previste
                    dgRisultatoRicerca.Columns(15).Visible = True       'Cronologia stampa
                    dgRisultatoRicerca.Columns(16).Visible = True       'Data Proroga
                    dgRisultatoRicerca.Columns(17).Visible = True       'Chiede Rimborso
                    'Antonello
                    dgRisultatoRicerca.Columns(22).Visible = True       'Si/No

                    Call VisualizzaCaricamentoQuest()
                Else                                                    'Ente
                    imgStampa.Visible = True
                    imgInoltra.Visible = True
                    lblStato.Visible = False
                    ddlStato.Visible = False
                End If

            Else
                CmdEsporta.Visible = True
            End If
            If Session("Utente") <> "E" Then
                'aggiunto il 25/08/2014 da simona cordella
                If ForzaRipristinaFormazione(Session("Utente")) = True Then
                    dgRisultatoRicerca.Columns(24).Visible = True
                End If
            End If
            'verifico se ci sono progetti sui quali è possibile effettuare ancora subentri
            Dim i As Integer
            Dim dtGriglia As DataTable
            Dim sTitolo As String

            dtGriglia = Session("DtbRicVol")
            i = 0
            For i = 0 To dtGriglia.Rows.Count - 1
                sEsitoRicerca = Verificagiorni(dtGriglia.Rows(i).Item(5), 2, sTitolo)
                If sEsitoRicerca = True Then
                    Exit Sub
                End If
            Next

        Else
            CmdEsporta.Visible = False
        End If


    End Sub

    Private Sub ColoraCelle()
        Dim item As DataGridItem
        Dim color As New System.Drawing.Color
        Dim Mychk As CheckBox
        Dim x As Integer
        Dim intStato As Integer

        For Each item In dgRisultatoRicerca.Items
            'vedo se sono state caricate lo ore formazione
            If VediOreFormazione(dgRisultatoRicerca.Items(item.ItemIndex).Cells(9).Text) = True Then
                For x = 0 To dgRisultatoRicerca.Columns.Count - 1
                    color = color.Khaki
                    dgRisultatoRicerca.Items(item.ItemIndex).Cells(x).BackColor = color
                Next
            End If

            ''vedo se è stato fatto il questionario
            ''If VediQuestionario(dgRisultatoRicerca.Items(item.ItemIndex).Cells(9).Text) = True Then
            ''For x = 0 To 12
            ''color = color.LightGreen
            ''dgRisultatoRicerca.Items(item.ItemIndex).Cells(x).BackColor = color
            ''Next
            ''End If

            'vedo se è stato confermato dall'ente
            intStato = VediTipoStato(dgRisultatoRicerca.Items(item.ItemIndex).Cells(9).Text)
            If intStato = 1 Or intStato = 2 Then        'confermato dall'ente e approvato da UNSC
                For x = 0 To dgRisultatoRicerca.Columns.Count - 1
                    color = color.LightGreen
                    dgRisultatoRicerca.Items(item.ItemIndex).Cells(x).BackColor = color
                Next
            End If

            'Non consento il caricamento delle ore di formazione a volontari non in servizio
            If dgRisultatoRicerca.Items(item.ItemIndex).Cells(8).Text = 0 Then
                Mychk = dgRisultatoRicerca.Items.Item(item.ItemIndex).FindControl("chkSelProg")
                Mychk.Enabled = False
            End If

            'Non consento il caricamento delle ore di formazione a progetti con stato confermato e approvato
            ''intStato = VediTipoStato(dgRisultatoRicerca.Items(item.ItemIndex).Cells(9).Text)
            ''If intStato <> 0 Then
            ''    Mychk = dgRisultatoRicerca.Items.Item(item.ItemIndex).FindControl("chkSelProg")
            ''    Mychk.Enabled = False
            ''End If
        Next

    End Sub

    Private Sub ColoraCelleStati()
        Dim item As DataGridItem
        Dim color As New System.Drawing.Color
        Dim Mychk As CheckBox
        Dim x As Integer
        Dim intStato As Integer

        For Each item In dgRisultatoRicerca.Items

            'vedo se sono state caricate lo ore formazione
            intStato = VediTipoStato(dgRisultatoRicerca.Items(item.ItemIndex).Cells(9).Text)
            Select Case intStato

                Case Is = 1                 'confermato dall'ente
                    For x = 0 To dgRisultatoRicerca.Columns.Count - 1
                        color = color.Khaki
                        dgRisultatoRicerca.Items(item.ItemIndex).Cells(x).BackColor = color
                    Next

                Case Is = 2                 'approvato dall'UNSC
                    For x = 0 To dgRisultatoRicerca.Columns.Count - 1
                        color = color.LightGreen
                        dgRisultatoRicerca.Items(item.ItemIndex).Cells(x).BackColor = color
                    Next
                Case Is = 3                 'in pagamento
                    For x = 0 To dgRisultatoRicerca.Columns.Count - 1
                        color = color.Plum
                        dgRisultatoRicerca.Items(item.ItemIndex).Cells(x).BackColor = color
                    Next
                Case Is = 4                 'Fuori Termine Helios
                    For x = 0 To dgRisultatoRicerca.Columns.Count - 1
                        color = color.LightSalmon
                        dgRisultatoRicerca.Items(item.ItemIndex).Cells(x).BackColor = color
                    Next
            End Select
        Next
    End Sub

    Private Sub VisualizzaCaricamentoQuest()
        Dim item As DataGridItem
        Dim color As New System.Drawing.Color
        Dim Mychk As CheckBox
        Dim x As Integer
        Dim intStato As Integer

        For Each item In dgRisultatoRicerca.Items

            ' ''vedo se è stato fatto il questionario
            'If VediQuestionario(dgRisultatoRicerca.Items(item.ItemIndex).Cells(9).Text) = True Then
            '    dgRisultatoRicerca.Items(item.ItemIndex).Cells(11).Text = "Si"
            'Else
            '    dgRisultatoRicerca.Items(item.ItemIndex).Cells(11).Text = "No"
            'End If
            dgRisultatoRicerca.Items(item.ItemIndex).Cells(11).Text = VediQuestionario(dgRisultatoRicerca.Items(item.ItemIndex).Cells(9).Text)
        Next

    End Sub

    Private Function Verificagiorni(ByVal IdProg As Integer, ByVal sTipo As Integer, ByRef sTitolo As String) As Boolean
        '*****************************************************************************************+
        'AUTORE: Guido Testa 
        'DATA: 18/07/2006
        'DESCRIZONE: controllo date progetto per effettuare stampa questionario
        Dim strSQL As String
        Dim bEsitoData As Boolean
        Dim iGiorni As Integer
        Dim strDataProroga As String
        Dim intTipoFormazioneGen As Integer
        bEsitoData = False

        If sTipo = 1 Then               'Gestione temporale stampa questionario     <=150 gg
            'iGiorni = 150
            intTipoFormazioneGen = RitornaTipoFormazioneGenerale(IdProg)
            If intTipoFormazioneGen = 1 Then 'UNICATRANCHE
                iGiorni = 180
            Else 'DUE TRANCHE
                iGiorni = 270
            End If
        ElseIf sTipo = 2 Then           'Gestione temporale conferma caricamento ore <=90 gg
            iGiorni = 90
        End If

        If sTipo = 1 Then           'verifo eventuali proroghe

            'vedo se è stata impostata la data proroga
            strSQL = "Select ISNULL(CONVERT(varchar, AttivitàFormazioneGenerale.DataProroga, 103), 0) AS DataProroga, a.titolo From AttivitàFormazioneGenerale " & _
                " INNER JOIN Attività a on a.idattività=AttivitàFormazioneGenerale.idattività " & _
                        " Where a.idattività = " & IdProg
            If Not dtrGenerico Is Nothing Then
                dtrGenerico.Close()
                dtrGenerico = Nothing
            End If

            dtrGenerico = ClsServer.CreaDatareader(strSQL, Session("conn"))
            dtrGenerico.Read()

            If dtrGenerico.HasRows Then
                sTitolo = dtrGenerico("Titolo")
                strDataProroga = dtrGenerico("DataProroga")
            End If
            dtrGenerico.Close()
            dtrGenerico = Nothing

            If strDataProroga <> "0" Then

                strSQL = "SELECT isnull(datediff(dd,DataProroga,getdate()),-1) as DiffGG " & _
                         " FROM AttivitàFormazioneGenerale " & _
                         " WHERE idattività = " & IdProg
                dtrGenerico = ClsServer.CreaDatareader(strSQL, Session("conn"))
                dtrGenerico.Read()

                If dtrGenerico("DiffGG") > 7 Then
                    bEsitoData = True               'proroga scaduta
                Else
                    bEsitoData = False              'proroga 7 giorni valida
                End If
                dtrGenerico.Close()
                dtrGenerico = Nothing

                Return bEsitoData

                Exit Function
            End If

        End If


        strSQL = " Select a.IDAttività,Titolo "
        strSQL &= " FROM Attività a "
        strSQL &= " INNER JOIN AttivitàFormazioneGenerale afg on a.IDAttività=afg.IdAttività"
        strSQL &= " WHERE a.IDAttività=" & IdProg & ""
        If intTipoFormazioneGen = 1 Then 'Formazione UNICATRANCHE
            strSQL &= " AND GETDATE() < afg.DataScadenzaUnicaTranche "
        Else 'Formazione DUE TRANCHE
            strSQL &= " AND GETDATE() < afg.DataScadenzaSecondaTranche "
        End If
        dtrGenerico = ClsServer.CreaDatareader(strSQL, Session("conn"))

        If dtrGenerico.HasRows Then
            dtrGenerico.Read()

            bEsitoData = False
        Else
            bEsitoData = True
        End If

        'strSQL = "SELECT isnull(datediff(dd,DataInizioAttività,getdate()),0) as DiffGG, Titolo " & _
        '        " FROM attività WHERE IDAttività = " & IdProg

        'dtrGenerico = ClsServer.CreaDatareader(strSQL, Session("conn"))
        'dtrGenerico.Read()

        'If dtrGenerico.HasRows Then
        '    sTitolo = dtrGenerico("Titolo")
        '    If sTipo = 1 Then               'Gestione temporale stampa questionario     <=150 gg

        '        If dtrGenerico("DiffGG") > iGiorni Then
        '            bEsitoData = True
        '        End If
        '    ElseIf sTipo = 2 Then           'Gestione temporale conferma caricamento ore <=90 gg

        '        If dtrGenerico("DiffGG") <= iGiorni Then
        '            bEsitoData = True
        '        End If

        '    End If
        'End If

        dtrGenerico.Close()
        dtrGenerico = Nothing

        Return bEsitoData

    End Function

    Private Function ForzaRipristinaFormazione(ByVal StrUtente As String) As Boolean
        '** creata a SIMONA CORDELLA
        '** 25/08/2014
        '** Verifica se l'utente è abilitato alla visualizzazione della colonnna "Ripristina" in griglia

        Dim strSql As String
        Dim dtrgenerico As SqlClient.SqlDataReader
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
        'Verifica menu sicurezza su funzione accredita
        strSql = "SELECT AssociaUtenteGruppo.username, VociMenu.IdVoceMenu, VociMenu.VoceMenu, VociMenu.ImmaginePassiva, VociMenu.ImmagineAttiva, VociMenu.Link," & _
                 " VociMenu.IdVoceMenuPadre" & _
                 " FROM VociMenu " & _
                 " INNER JOIN AbilitazioniProfiliVociMenu ON VociMenu.IdVoceMenu = AbilitazioniProfiliVociMenu.IdVoceMenu" & _
                 " INNER JOIN Profili ON AbilitazioniProfiliVociMenu.IdProfilo = Profili.IdProfilo" & _
                 " INNER JOIN AssociaUtenteGruppo ON Profili.IdProfilo = AssociaUtenteGruppo.IdProfilo" & _
                 " LEFT JOIN  RestrizioniUtenteVociMenu ON AssociaUtenteGruppo.UserName = RestrizioniUtenteVociMenu.UserName and RestrizioniUtenteVociMenu.IdVoceMenu=VociMenu.IdVoceMenu" & _
                 " WHERE VociMenu.descrizione = 'Forza Ripristina Formazione'" & _
                 " AND AssociaUtenteGruppo.username ='" & StrUtente & "' and (RestrizioniUtenteVociMenu.TipoRestrizione <> 0 or RestrizioniUtenteVociMenu.TipoRestrizione is null)"
        dtrgenerico = ClsServer.CreaDatareader(strSql, Session("conn"))

        ForzaRipristinaFormazione = dtrgenerico.Read()
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
    End Function

    Private Function VediOreFormazione(ByVal IdProg As Integer) As Boolean
        '*****************************************************************************************+
        'AUTORE: Guido Testa 
        'DATA: 03/05/2006
        'DESCRIZONE: verifca se sono state caricate le ore di formazione sui volontari relativi al progetto indicato come parametro
        Dim StrSQL As String
        Dim bolEsito As Boolean
        bolEsito = False

        StrSQL = "SELECT  ISNULL(SUM(entità.OreFormazione), 0) as Ore " & _
                 " FROM  entità INNER JOIN " & _
                 " attivitàentità ON entità.IDEntità = attivitàentità.IDEntità INNER JOIN " & _
                 "  attivitàentisediattuazione ON attivitàentità.IDAttivitàEnteSedeAttuazione = attivitàentisediattuazione.IDAttivitàEnteSedeAttuazione INNER JOIN " & _
                 "  attività ON attivitàentisediattuazione.IDAttività = attività.IDAttività " & _
                 "  WHERE  (attività.IDAttività = " & IdProg & ")"

        If Session("idente") <> -1 Then
            StrSQL = StrSQL & " AND (attività.IDEntePresentante = " & Session("idente") & ") "
        End If

        dtrGenerico = ClsServer.CreaDatareader(StrSQL, Session("conn"))

        dtrGenerico.Read()
        If dtrGenerico.Item("Ore") > 0 Then
            bolEsito = True
        End If

        dtrGenerico.Close()
        dtrGenerico = Nothing

        Return bolEsito

    End Function

    Private Function VediTipoStato(ByVal IdProg As Integer) As Integer
        '*****************************************************************************************+
        'AUTORE: Guido Testa 
        'DATA: 03/07/2006
        'DESCRIZONE: trovo lo stato del singolo progetto

        Dim strSQL As String
        Dim intStato As Integer
        'StatoFormazione=0      Registrato
        'StatoFormazione=1      Confermato dall'ente
        'StatoFormazione=2      Approvato dall'UNSC
        'StatoFormazione=3      in pagamento (unsc) 
        'StatoFormazione=4      Fuori Termine Helios

        strSQL = "select isnull(StatoFormazione,0) as StatoFormazione  FROM  AttivitàFormazioneGenerale WHERE idattività=" & IdProg
        dtrGenerico = ClsServer.CreaDatareader(strSQL, Session("conn"))
        dtrGenerico.Read()

        If dtrGenerico.HasRows Then
            intStato = dtrGenerico("StatoFormazione")
        End If
        dtrGenerico.Close()
        dtrGenerico = Nothing

        '23/09/2011
        'SANDOKAN
        'Controlla congruenza data scadenza formazione 150 gg
        'If VerificagiorniConferma(IdProg) = True Then
        '    intStato = -1 'fuori termine
        'End If

        Return intStato
    End Function

    Private Function VediQuestionario(ByVal IdProg As Integer) As String
        '*****************************************************************************************+
        'AUTORE: Guido Testa 
        'DATA: 03/05/2006
        'DESCRIZONE: verifca se è stato compilato il questionario per il progetto indicato come parametro

        Dim StrSQL As String
        Dim strQuestionario As String
        'Dim bolEsito As Boolean
        ' bolEsito = False

        StrSQL = "SELECT attività.IDAttività " _
               & " FROM BandiAttività INNER JOIN " _
               & " attività ON BandiAttività.IdBandoAttività = attività.IDBandoAttività INNER JOIN " _
               & " attivitàFormazioneGenerale ON Attività.IdAttività = attivitàFormazioneGenerale.IDAttività INNER JOIN " _
               & " QuestionarioStoricoProgetti ON BandiAttività.IdBandoAttività = QuestionarioStoricoProgetti.IdBandoAttività and AttivitàFormazioneGenerale.TipoFormazioneGenerale = QuestionarioStoricoProgetti.TipoFormazioneGenerale " _
               & " WHERE (attività.IDAttività = " & IdProg & ")"

        dtrGenerico = ClsServer.CreaDatareader(StrSQL, Session("conn"))

        'bolEsito = dtrGenerico.HasRows
        'dtrGenerico.Close()
        'dtrGenerico = Nothing

        'Return bolEsito
        If dtrGenerico.HasRows = False Then
            If Not dtrGenerico Is Nothing Then
                dtrGenerico.Close()
                dtrGenerico = Nothing
            End If
            '' se non è stato caricato il qeustionario controllo se è scaduto
            StrSQL = "SELECT attivitàFormazioneGenerale.IDAttività, CASE attivitàFormazioneGenerale.TipoFormazioneGenerale  " & _
                    " WHEN 1  THEN CASE WHEN GETDATE() > DataScadenzaQuestionarioUnicaTranche THEN 'NO Scaduto' else 'NO' END   " & _
                    " WHEN 2  THEN CASE WHEN GETDATE() > DataScadenzaQuestionarioDoppiaTranche THEN 'NO Scaduto' else 'NO' END   " & _
                    " END AS Questionario  " & _
                    " FROM BandiAttività " & _
                    " INNER JOIN attività ON BandiAttività.IdBandoAttività = attività.IDBandoAttività " & _
                    " INNER JOIN  attivitàFormazioneGenerale ON Attività.IdAttività = attivitàFormazioneGenerale.IDAttività " & _
                    " WHERE attività.IDAttività = " & IdProg & ""
            dtrGenerico = ClsServer.CreaDatareader(StrSQL, Session("conn"))
            If dtrGenerico.HasRows = True Then
                dtrGenerico.Read()
                strQuestionario = dtrGenerico("Questionario")
            End If
        Else
            strQuestionario = "SI"
        End If
        If Not dtrGenerico Is Nothing Then
            dtrGenerico.Close()
            dtrGenerico = Nothing
        End If

        Return strQuestionario

    End Function

    Private Sub dgRisultatoRicerca_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgRisultatoRicerca.PageIndexChanged
        Call RiccoraItemSelezionato(e)

        If Request.QueryString("VengoDa") <> "Ricerca" Then
            Call ColoraCelle()
        Else
            Call ColoraCelleStati()
        End If
    End Sub

    Private Sub RiccoraItemSelezionato(Optional ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs = Nothing)
        '*****************************************************************************************+
        'AUTORE: Guido Testa 
        'DATA: 26/04/2006
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

    Private Sub imgProroga_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles imgProroga.Click
        '*****************************************************************************************
        Dim dtGriglia As DataTable
        Dim i As Integer
        Dim bolVerifica As Boolean
        Dim intStato As Integer
        Dim strSQL As String
        Dim bolTrovaPrg As Boolean
        Dim sTitolo As String
        bolTrovaPrg = False
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
            lblmessaggio.Text = "Selezionare almeno un progetto dalla lista per effettuare la proroga."
        Else

            'aggiorno lo stato dei progetti selezionati per prorogarli di 10 giorni
            For i = 0 To dtGriglia.Rows.Count - 1
                If dtGriglia.Rows(i).Item(7) = 1 Then
                    intStato = VediTipoStato(dgRisultatoRicerca.Items(i).Cells(9).Text)
                    If (intStato <> 2 And intStato <> 3 And intStato <> 4) Then        'lo stato uguale e a 2 indica i progetti approvati dall'UNSC                  
                        'se non sono già stati confermati li posso prorogare
                        strSQL = "Update AttivitàFormazioneGenerale Set AlertChiusureIniziali=0, StatoFormazione=null,DataConferma=null,UserNameConferma=null, DataProroga=GetDate(),UserNameProroga='" & Session("utente") & "' Where idattività=" & dgRisultatoRicerca.Items(i).Cells(9).Text
                        ClsServer.EseguiSqlClient(strSQL, Session("conn"))
                        bolTrovaPrg = True
                    End If
                End If
            Next

            If bolTrovaPrg = False Then
                lblmessaggio.Text = "Tra i progetti selezionati non ci sono progetti da 'Prorogare'."
            Else
                lblmessaggio.Text = "Operazione terminata con successo"
            End If
        End If

        Call EseguiRicerca(0)
    End Sub

    Private Sub imgPagamento_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles imgPagamento.Click
        'aggiunto il 27/03/2007 da simona cordella 
        'gestione dello stato "in pagamento" solo dopo approvazione del progetto
        Dim dtGriglia As DataTable
        Dim i As Integer
        Dim bolVerifica As Boolean
        Dim intStato As Integer
        Dim strSQL As String
        Dim bolTrovaPrg As Boolean
        bolTrovaPrg = False
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
            lblmessaggio.Text = "Selezionare almeno un progetto dalla lista per il pagamento."
        Else

            'aggiorno lo stato dei progetti selezionati
            For i = 0 To dtGriglia.Rows.Count - 1
                If dtGriglia.Rows(i).Item(7) = 1 Then
                    intStato = VediTipoStato(dgRisultatoRicerca.Items(i).Cells(9).Text)
                    If intStato = 2 Then       'lo stato uguale a 2 indica i progetti approvati dall'unsc
                        strSQL = "Update AttivitàFormazioneGenerale Set AlertChiusureIniziali=0, StatoFormazione=3, DataMandatoInPagamento=GetDate(),UserNameMandatoInPagamento ='" & Session("utente") & "' Where idattività=" & dgRisultatoRicerca.Items(i).Cells(9).Text
                        ClsServer.EseguiSqlClient(strSQL, Session("conn"))
                        bolTrovaPrg = True
                    End If
                End If
            Next

            If bolTrovaPrg = False Then
                lblmessaggio.Text = "Tra i progetti selezionati non ci sono progetti da inviare 'In Pagamento'."
            Else
                lblmessaggio.Text = "Operazione terminata con successo"
            End If

        End If

        Call ColoraCelleStati()

        Call VisualizzaCaricamentoQuest()
    End Sub

    Private Sub ImaFuoriTermine_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ImaFuoriTermine.Click
        'aggiunto il 24/05/2010 da SANDOKAN 
        'gestione dello stato "Fuori Termine Helios"
        Dim dtGriglia As DataTable
        Dim i As Integer
        Dim bolVerifica As Boolean
        Dim intStato As Integer
        Dim strSQL As String
        Dim bolTrovaPrg As Boolean
        bolTrovaPrg = False
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
            lblmessaggio.Text = "Selezionare almeno un progetto dalla lista per il pagamento."
        Else

            'aggiorno lo stato dei progetti selezionati
            For i = 0 To dtGriglia.Rows.Count - 1
                If dtGriglia.Rows(i).Item(7) = 1 Then
                    intStato = VediTipoStato(dgRisultatoRicerca.Items(i).Cells(9).Text)
                    If intStato = 0 Then       'lo stato uguale a 0 indica lo stato della formazione in Registrata
                        strSQL = "Update AttivitàFormazioneGenerale Set StatoFormazione=4, DataFuoriTermine=GetDate(),UserFuoriTermine ='" & Session("utente") & "' Where idattività=" & dgRisultatoRicerca.Items(i).Cells(9).Text
                        ClsServer.EseguiSqlClient(strSQL, Session("conn"))
                        bolTrovaPrg = True
                    End If
                End If
            Next

            If bolTrovaPrg = False Then
                lblmessaggio.Text = "Tra i progetti selezionati non ci sono progetti in stato 'Formazione Registrata'."
            Else
                lblmessaggio.Text = "Operazione terminata con successo"
            End If

        End If

        Call ColoraCelleStati()

        'Call VisualizzaCaricamentoQuest()
    End Sub

    Private Sub imgRespingi_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles imgRespingi.Click
        '*****************************************************************************************
        Dim dtGriglia As DataTable
        Dim i As Integer
        Dim bolVerifica As Boolean
        Dim intStato As Integer
        Dim strSQL As String
        Dim bolTrovaPrg As Boolean
        bolTrovaPrg = False
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
            lblmessaggio.Text = "Selezionare almeno un progetto dalla lista per effettuare l'annullamento."
        Else

            'aggiorno lo stato dei progetti selezionati
            For i = 0 To dtGriglia.Rows.Count - 1
                If dtGriglia.Rows(i).Item(7) = 1 Then
                    intStato = VediTipoStato(dgRisultatoRicerca.Items(i).Cells(9).Text)
                    If intStato = 1 Then       'lo stato uguale e a 1 indica i progetti confermati dall'Ente X I QUALI è POSSIBILE FARE L'ANNULLAMENTO
                        strSQL = "Update AttivitàFormazioneGenerale Set AlertChiusureIniziali=0, StatoFormazione=null,DataConferma=null,UserNameConferma=null, DataAnnullamento=GetDate(),UserNameAnnullamento='" & Session("utente") & "' Where idattività=" & dgRisultatoRicerca.Items(i).Cells(9).Text
                        ClsServer.EseguiSqlClient(strSQL, Session("conn"))
                        bolTrovaPrg = True
                    End If
                End If
            Next

            If bolTrovaPrg = False Then
                lblmessaggio.Text = "Tra i progetti selezionati non ci sono progetti da 'Annullare'."
            Else
                lblmessaggio.Text = "Operazione terminata con successo"
            End If

        End If

        Call ColoraCelleStati()

        Call VisualizzaCaricamentoQuest()
    End Sub

    Private Sub dgRisultatoRicerca_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgRisultatoRicerca.ItemCommand
        '*** AGGIUTNO IL 25/08/2014 DA SIMONA CORDELLA
        '*** La colonna si abilita solo sotto autorizzazione ("Forza Ripristina Formazione")
        '*** Richiamo la store "SP_FORMAZIONE_ANNULLA_STATO"
        If e.CommandName = "Ripristina" Then
            'richiamo store che esegue il ripristino della formazione allo stato precedente
            FormazioneAnnullaStato(CInt(e.Item.Cells(9).Text))
            EseguiRicerca(0)
        End If
    End Sub

    Private Sub FormazioneAnnullaStato(ByVal IdProgetto As Integer)
        'Creata il:		25/08/2014
        'Funzionalità: richiamo store SP_ACCREDITAMENTO_MODIFICAMASCHERA_SEDE per il ripristino della formazione


        Dim SqlCmd As New SqlClient.SqlCommand
        Try
            SqlCmd.CommandText = "SP_FORMAZIONE_ANNULLA_STATO"
            SqlCmd.CommandType = CommandType.StoredProcedure
            SqlCmd.Connection = Session("Conn")

            SqlCmd.Parameters.Add("@IdProgetto ", SqlDbType.Int).Value = IdProgetto
            SqlCmd.Parameters.Add("@UsernameRichiesta", SqlDbType.VarChar).Value = Session("Utente")

            'Esito aggiornamento: 0-Errore 1-Aggiornamento effettuato
            SqlCmd.Parameters.Add("@Esito", SqlDbType.TinyInt)
            SqlCmd.Parameters("@Esito").Direction = ParameterDirection.Output

            SqlCmd.Parameters.Add("@messaggio", SqlDbType.VarChar)
            SqlCmd.Parameters("@messaggio").Size = 1000
            SqlCmd.Parameters("@messaggio").Direction = ParameterDirection.Output

            SqlCmd.ExecuteNonQuery()

            lblmessaggio.Text = SqlCmd.Parameters("@messaggio").Value()

        Catch ex As Exception
            lblmessaggio.Text = ex.Message
        Finally

        End Try
    End Sub

    Private Sub chkSelDesel_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkSelDesel.CheckedChanged
        Dim i As Integer
        Dim Mychk As CheckBox

        hlVolontari.Visible = False

        '---determino cosa è stato checkato nella pag corrente e lo salvo nella datatable di sessione
        For i = 0 To dgRisultatoRicerca.Items.Count - 1
            Mychk = dgRisultatoRicerca.Items.Item(i).FindControl("chkSelProg")

            If (Mychk.Enabled = True) Then
                Mychk.Checked = chkSelDesel.Checked
            End If

        Next i
        If (chkSelDesel.Checked) Then
            chkSelDesel.Text = "Deseleziona Tutto"
        Else
            chkSelDesel.Text = "Seleziona Tutto"
        End If

    End Sub

    Private Sub CmdEsporta_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdEsporta.Click
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

        Call ColoraCelle()

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
            lblmessaggio.Text = ""

            If Request.QueryString("TipoFormazioneGenerale") = 1 Then
                EsportaVolontari() 'unica tranche
            Else
                EsportaVolontariTranche() 'due tranche
            End If
            'Response.Write("<script>")
            'Response.Write("window.open('WfrmEsportazioneOreVolontari.aspx', 'Esporta', 'width=260, height=195, status=no, toolbar=no, location=no, menubar=no, scrollbars=no')")
            'Response.Write("</script>")
        End If

    End Sub

    Private Sub EsportaVolontari()
        '*****************************************************************************************+
        'AUTORE: Guido Testa 
        'DATA: 26/04/2006
        'DESCRIZONE: estraggo tutti i volontari relativamente ai progetti precedentemente selezionati

        Dim StrSql As String
        Dim dtrVolontari As Data.SqlClient.SqlDataReader

        Dim Writer As StreamWriter
        Dim xLinea As String
        Dim i As Integer
        Dim NomeUnivoco As String
        Dim xPrefissoNome As String
        Dim dtOreVolontari As DataTable
        Dim strInCondition As String


        Try

            'vado a vedere quali sono stati i progetti selezionati per usarli come filtro per la selezione dei relativi volontari
            dtOreVolontari = Session("DtbRicVol")
            i = 0
            For i = 0 To dtOreVolontari.Rows.Count - 1
                If dtOreVolontari.Rows(i).Item(7) = 1 Then
                    strInCondition = strInCondition & dtOreVolontari.Rows(i).Item(5) & ","
                End If
            Next
            strInCondition = Mid(strInCondition, 1, Len(strInCondition) - 1)

            StrSql = "SELECT distinct enti.CodiceRegione, " & _
                     "entità.CodiceVolontario, " & _
                     "isnull(replace(replace(replace(replace(replace(replace(replace(entità.Cognome,'°',''),'ì','i'''),'é','e'''),'à','a'''),'ò','o'''),'è','e'''),'ù','u'''), '') as Cognome, " & _
                     "isnull(replace(replace(replace(replace(replace(replace(replace(entità.Nome,'°',''),'ì','i'''),'é','e'''),'à','a'''),'ò','o'''),'è','e'''),'ù','u'''), '') as Nome, " & _
                     "entità.Sesso,entità.codicefiscale,entità.datanascita, " & _
                     "enti.CodiceRegione AS CodiceEnte, " & _
                     "isnull(replace(replace(replace(replace(replace(replace(replace(enti.Denominazione,'°',''),'ì','i'''),'é','e'''),'à','a'''),'ò','o'''),'è','e'''),'ù','u'''), '') as Denominazione, " & _
                     "attività.CodiceEnte AS CodiceProgetto, " & _
                     "isnull(replace(replace(replace(replace(replace(replace(replace(attività.Titolo,'°',''),'ì','i'''),'é','e'''),'à','a'''),'ò','o'''),'è','e'''),'ù','u'''), '') as Titolo, " & _
                     "isnull(replace(replace(replace(replace(replace(replace(replace(comuni_1.Denominazione,'°',''),'ì','i'''),'é','e'''),'à','a'''),'ò','o'''),'è','e'''),'ù','u'''), '') AS ComuneSedeProgetto, " & _
                     "isnull(replace(replace(replace(replace(replace(replace(replace(provincie_1.Provincia,'°',''),'ì','i'''),'é','e'''),'à','a'''),'ò','o'''),'è','e'''),'ù','u'''), '') AS ProvinciaSedeProgetto, " & _
                     "isnull(replace(replace(replace(replace(replace(replace(replace(regioni_1.Regione,'°',''),'ì','i'''),'é','e'''),'à','a'''),'ò','o'''),'è','e'''),'ù','u'''), '') AS RegioneSedeProgetto, " & _
                     "isnull(replace(replace(replace(replace(replace(replace(replace(entisedi.Indirizzo,'°',''),'ì','i'''),'é','e'''),'à','a'''),'ò','o'''),'è','e'''),'ù','u'''), '') AS IndirizzoSedeProgetto, " & _
                     "entisedi.CAP AS CapSedeProgetto, isnull(entità.OreFormazione,0) as OreFormazione,attività.IDAttività " & _
                     "FROM entità " & _
                     "INNER JOIN attivitàentità ON entità.IDEntità = attivitàentità.IDEntità " & _
                     "INNER JOIN attivitàentisediattuazione ON attivitàentità.IDAttivitàEnteSedeAttuazione = attivitàentisediattuazione.IDAttivitàEnteSedeAttuazione " & _
                     "INNER JOIN attività ON attivitàentisediattuazione.IDAttività = attività.IDAttività " & _
                     "INNER JOIN entisediattuazioni ON attivitàentisediattuazione.IDEnteSedeAttuazione = entisediattuazioni.IDEnteSedeAttuazione " & _
                     "INNER JOIN entisedi ON entisediattuazioni.IDEnteSede = entisedi.IDEnteSede " & _
                     "INNER JOIN comuni ON entità.IDComuneNascita = comuni.IDComune " & _
                     "INNER JOIN provincie ON comuni.IDProvincia = provincie.IDProvincia " & _
                     "INNER JOIN comuni comuni_1 ON entisedi.IDComune = comuni_1.IDComune " & _
                     "INNER JOIN provincie provincie_1 ON comuni_1.IDProvincia = provincie_1.IDProvincia " & _
                     "INNER JOIN comuni comuni_2 ON entità.IDComuneResidenza = comuni_2.IDComune " & _
                     "INNER JOIN provincie provincie_2 ON comuni_2.IDProvincia = provincie_2.IDProvincia " & _
                     "INNER JOIN enti ON attività.IDEntePresentante = enti.IDEnte " & _
                     "INNER JOIN regioni ON provincie.IDRegione = regioni.IDRegione " & _
                     "INNER JOIN regioni regioni_1 ON provincie_1.IDRegione = regioni_1.IDRegione " & _
                     "INNER JOIN regioni regioni_2 ON provincie_2.IDRegione = regioni_2.IDRegione " & _
                     "INNER JOIN StatiEntità ON Entità.IdStatoEntità = Statientità.IdStatoEntità " & _
                     "WHERE (StatiEntità.InServizio = 1 OR StatiEntità.Sospeso = 1 OR StatiEntità.Chiuso = 1) AND attivitàentità.EscludiFormazione=0"

            If Session("TipoUtente") = "E" Then
                StrSql = StrSql & "AND Enti.IdEnte = " & Session("IdEnte") & " "
            End If

            StrSql = StrSql & " AND attività.IDAttività in (" & strInCondition & ")"

            StrSql = StrSql & " Order by attività.IDAttività,3,4"

            dtrVolontari = ClsServer.CreaDatareader(StrSql, Session("conn"))

            NomeUnivoco = vbNullString

            If dtrVolontari.HasRows = False Then
                lblmessaggio.Text = lblmessaggio.Text & "Nessun Volontario."
            Else
                While dtrVolontari.Read
                    If NomeUnivoco = vbNullString Then
                        xPrefissoNome = Session("Utente")
                        NomeUnivoco = xPrefissoNome & "ExpOreVol" & Year(Now) & Month(Now) & Day(Now) & Hour(Now) & Minute(Now) & Second(Now)
                        Writer = New StreamWriter(Server.MapPath("download") & "\" & NomeUnivoco & ".CSV")
                        '---intestazioni
                        xLinea = "CodiceVolontario;Cognome;Nome;Sesso;CodiceFiscale;DataNascita;CodiceEnte;Denominazione;CodiceProgetto;Titolo;ComuneSedeProgetto;ProvinciaSedeProgetto;RegioneSedeProgetto;IndirizzoSedeProgetto;CAP;OreFormazione"
                        Writer.WriteLine(xLinea)
                    End If
                    xLinea = vbNullString

                    '---salto il primo elemento (nome file)
                    'codice volontario
                    If IsDBNull(dtrVolontari(1)) = True Then
                        xLinea = vbNullString & ";"
                    Else
                        xLinea = ClsUtility.FormatExport(dtrVolontari(1)) & ";"
                    End If
                    'cognome
                    If IsDBNull(dtrVolontari(2)) = True Then
                        xLinea = xLinea & vbNullString & ";"
                    Else
                        xLinea = xLinea & ClsUtility.FormatExport(dtrVolontari(2)) & ";"
                    End If
                    'nome
                    If IsDBNull(dtrVolontari(3)) = True Then
                        xLinea = xLinea & vbNullString & ";"
                    Else
                        xLinea = xLinea & ClsUtility.FormatExport(dtrVolontari(3)) & ";"
                    End If
                    'sesso
                    If IsDBNull(dtrVolontari(4)) = True Then
                        xLinea = xLinea & vbNullString & ";"
                    Else
                        If dtrVolontari(4) = "0" Then
                            xLinea = xLinea & "M;"
                        Else
                            xLinea = xLinea & "F;"
                        End If
                    End If
                    'codice fiscale
                    If IsDBNull(dtrVolontari(5)) = True Then
                        xLinea = xLinea & vbNullString & ";"
                    Else
                        xLinea = xLinea & ClsUtility.FormatExport(dtrVolontari(5)) & ";"
                    End If
                    'data nascita
                    If IsDBNull(dtrVolontari(6)) = True Then
                        xLinea = xLinea & vbNullString & ";"
                    Else
                        xLinea = xLinea & ClsUtility.FormatExport(dtrVolontari(6)) & ";"
                    End If
                    'codice ente
                    If IsDBNull(dtrVolontari(7)) = True Then
                        xLinea = xLinea & vbNullString & ";"
                    Else
                        xLinea = xLinea & ClsUtility.FormatExport(dtrVolontari(7)) & ";"
                    End If
                    'Denominazione Ente
                    If IsDBNull(dtrVolontari(8)) = True Then
                        xLinea = xLinea & vbNullString & ";"
                    Else
                        xLinea = xLinea & ClsUtility.FormatExport(dtrVolontari(8)) & ";"
                    End If
                    'codice progetto
                    If IsDBNull(dtrVolontari(9)) = True Then
                        xLinea = xLinea & vbNullString & ";"
                    Else
                        xLinea = xLinea & ClsUtility.FormatExport(dtrVolontari(9)) & ";"
                    End If
                    'titolo progetto
                    If IsDBNull(dtrVolontari(10)) = True Then
                        xLinea = xLinea & vbNullString & ";"
                    Else
                        xLinea = xLinea & ClsUtility.FormatExport(dtrVolontari(10)) & ";"
                    End If
                    'Comune Sede Progetto
                    If IsDBNull(dtrVolontari(11)) = True Then
                        xLinea = xLinea & vbNullString & ";"
                    Else
                        xLinea = xLinea & ClsUtility.FormatExport(dtrVolontari(11)) & ";"
                    End If
                    'Provincia Sede Progetto
                    If IsDBNull(dtrVolontari(12)) = True Then
                        xLinea = xLinea & vbNullString & ";"
                    Else
                        xLinea = xLinea & ClsUtility.FormatExport(dtrVolontari(12)) & ";"
                    End If
                    'Regione Sede Progetto
                    If IsDBNull(dtrVolontari(13)) = True Then
                        xLinea = xLinea & vbNullString & ";"
                    Else
                        xLinea = xLinea & ClsUtility.FormatExport(dtrVolontari(13)) & ";"
                    End If
                    'Indirizzo Sede Progetto
                    If IsDBNull(dtrVolontari(14)) = True Then
                        xLinea = xLinea & vbNullString & ";"
                    Else
                        xLinea = xLinea & ClsUtility.FormatExport(dtrVolontari(14)) & ";"
                    End If
                    'CAP
                    If IsDBNull(dtrVolontari(15)) = True Then
                        xLinea = xLinea & vbNullString & ";"
                    Else
                        xLinea = xLinea & ClsUtility.FormatExport(dtrVolontari(15)) & ";"
                    End If
                    'ORE FORMAZIONE
                    If IsDBNull(dtrVolontari(16)) = True Then
                        xLinea = xLinea & vbNullString & ";"
                    Else
                        xLinea = xLinea & ClsUtility.FormatExport(dtrVolontari(16))
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

    Private Sub EsportaVolontariTranche()
        '*****************************************************************************************+
        'AUTORE: Simona Cordella
        'DATA: 10/04/2015
        'DESCRIZONE: estraggo tutti i volontari relativamente ai progetti precedentemente selezionati

        Dim StrSql As String
        Dim dtrVolontari As Data.SqlClient.SqlDataReader

        Dim Writer As StreamWriter
        Dim xLinea As String
        Dim i As Integer
        Dim NomeUnivoco As String
        Dim xPrefissoNome As String
        Dim dtOreVolontari As DataTable
        Dim strInCondition As String


        Try

            'vado a vedere quali sono stati i progetti selezionati per usarli come filtro per la selezione dei relativi volontari
            dtOreVolontari = Session("DtbRicVol")
            i = 0
            For i = 0 To dtOreVolontari.Rows.Count - 1
                If dtOreVolontari.Rows(i).Item(7) = 1 Then
                    strInCondition = strInCondition & dtOreVolontari.Rows(i).Item(5) & ","
                End If
            Next
            strInCondition = Mid(strInCondition, 1, Len(strInCondition) - 1)

            StrSql = "SELECT distinct enti.CodiceRegione, " & _
                     "entità.CodiceVolontario, " & _
                     "isnull(replace(replace(replace(replace(replace(replace(replace(entità.Cognome,'°',''),'ì','i'''),'é','e'''),'à','a'''),'ò','o'''),'è','e'''),'ù','u'''), '') as Cognome, " & _
                     "isnull(replace(replace(replace(replace(replace(replace(replace(entità.Nome,'°',''),'ì','i'''),'é','e'''),'à','a'''),'ò','o'''),'è','e'''),'ù','u'''), '') as Nome, " & _
                     "entità.Sesso,entità.codicefiscale,entità.datanascita, " & _
                     "enti.CodiceRegione AS CodiceEnte, " & _
                     "isnull(replace(replace(replace(replace(replace(replace(replace(enti.Denominazione,'°',''),'ì','i'''),'é','e'''),'à','a'''),'ò','o'''),'è','e'''),'ù','u'''), '') as Denominazione, " & _
                     "attività.CodiceEnte AS CodiceProgetto, " & _
                     "isnull(replace(replace(replace(replace(replace(replace(replace(attività.Titolo,'°',''),'ì','i'''),'é','e'''),'à','a'''),'ò','o'''),'è','e'''),'ù','u'''), '') as Titolo, " & _
                     "isnull(replace(replace(replace(replace(replace(replace(replace(comuni_1.Denominazione,'°',''),'ì','i'''),'é','e'''),'à','a'''),'ò','o'''),'è','e'''),'ù','u'''), '') AS ComuneSedeProgetto, " & _
                     "isnull(replace(replace(replace(replace(replace(replace(replace(provincie_1.Provincia,'°',''),'ì','i'''),'é','e'''),'à','a'''),'ò','o'''),'è','e'''),'ù','u'''), '') AS ProvinciaSedeProgetto, " & _
                     "isnull(replace(replace(replace(replace(replace(replace(replace(regioni_1.Regione,'°',''),'ì','i'''),'é','e'''),'à','a'''),'ò','o'''),'è','e'''),'ù','u'''), '') AS RegioneSedeProgetto, " & _
                     "isnull(replace(replace(replace(replace(replace(replace(replace(entisedi.Indirizzo,'°',''),'ì','i'''),'é','e'''),'à','a'''),'ò','o'''),'è','e'''),'ù','u'''), '') AS IndirizzoSedeProgetto, " & _
                     "entisedi.CAP AS CapSedeProgetto, entità.OreFormazionePrimaTranche as OreFormazionePrimaTranche,entità.OreFormazioneSecondaTranche as OreFormazioneSecondaTranche,attività.IDAttività " & _
                     "FROM entità " & _
                     "INNER JOIN attivitàentità ON entità.IDEntità = attivitàentità.IDEntità " & _
                     "INNER JOIN attivitàentisediattuazione ON attivitàentità.IDAttivitàEnteSedeAttuazione = attivitàentisediattuazione.IDAttivitàEnteSedeAttuazione " & _
                     "INNER JOIN attività ON attivitàentisediattuazione.IDAttività = attività.IDAttività " & _
                     "INNER JOIN entisediattuazioni ON attivitàentisediattuazione.IDEnteSedeAttuazione = entisediattuazioni.IDEnteSedeAttuazione " & _
                     "INNER JOIN entisedi ON entisediattuazioni.IDEnteSede = entisedi.IDEnteSede " & _
                     "INNER JOIN comuni ON entità.IDComuneNascita = comuni.IDComune " & _
                     "INNER JOIN provincie ON comuni.IDProvincia = provincie.IDProvincia " & _
                     "INNER JOIN comuni comuni_1 ON entisedi.IDComune = comuni_1.IDComune " & _
                     "INNER JOIN provincie provincie_1 ON comuni_1.IDProvincia = provincie_1.IDProvincia " & _
                     "INNER JOIN comuni comuni_2 ON entità.IDComuneResidenza = comuni_2.IDComune " & _
                     "INNER JOIN provincie provincie_2 ON comuni_2.IDProvincia = provincie_2.IDProvincia " & _
                     "INNER JOIN enti ON attività.IDEntePresentante = enti.IDEnte " & _
                     "INNER JOIN regioni ON provincie.IDRegione = regioni.IDRegione " & _
                     "INNER JOIN regioni regioni_1 ON provincie_1.IDRegione = regioni_1.IDRegione " & _
                     "INNER JOIN regioni regioni_2 ON provincie_2.IDRegione = regioni_2.IDRegione " & _
                     "INNER JOIN StatiEntità ON Entità.IdStatoEntità = Statientità.IdStatoEntità " & _
                     "WHERE (StatiEntità.InServizio = 1 OR StatiEntità.Sospeso = 1 OR StatiEntità.Chiuso = 1) AND attivitàentità.EscludiFormazione=0"

            If Session("TipoUtente") = "E" Then
                StrSql = StrSql & "AND Enti.IdEnte = " & Session("IdEnte") & " "
            End If

            StrSql = StrSql & " AND attività.IDAttività in (" & strInCondition & ")"

            StrSql = StrSql & " Order by attività.IDAttività, 3, 4"

            dtrVolontari = ClsServer.CreaDatareader(StrSql, Session("conn"))

            NomeUnivoco = vbNullString

            If dtrVolontari.HasRows = False Then
                lblmessaggio.Text = lblmessaggio.Text & "Nessun Volontario."
            Else
                While dtrVolontari.Read
                    If NomeUnivoco = vbNullString Then
                        xPrefissoNome = Session("Utente")
                        NomeUnivoco = xPrefissoNome & "ExpOreVolTranche" & Year(Now) & Month(Now) & Day(Now) & Hour(Now) & Minute(Now) & Second(Now)
                        Writer = New StreamWriter(Server.MapPath("download") & "\" & NomeUnivoco & ".CSV")
                        '---intestazioni
                        xLinea = "CodiceVolontario;Cognome;Nome;Sesso;CodiceFiscale;DataNascita;CodiceEnte;Denominazione;CodiceProgetto;Titolo;ComuneSedeProgetto;ProvinciaSedeProgetto;RegioneSedeProgetto;IndirizzoSedeProgetto;CAP;OreFormazionePrimaTranche;OreFormazioneSecondaTranche"
                        Writer.WriteLine(xLinea)
                    End If
                    xLinea = vbNullString

                    '---salto il primo elemento (nome file)
                    'codice volontario
                    If IsDBNull(dtrVolontari(1)) = True Then
                        xLinea = vbNullString & ";"
                    Else
                        xLinea = ClsUtility.FormatExport(dtrVolontari(1)) & ";"
                    End If
                    'cognome
                    If IsDBNull(dtrVolontari(2)) = True Then
                        xLinea = xLinea & vbNullString & ";"
                    Else
                        xLinea = xLinea & ClsUtility.FormatExport(dtrVolontari(2)) & ";"
                    End If
                    'nome
                    If IsDBNull(dtrVolontari(3)) = True Then
                        xLinea = xLinea & vbNullString & ";"
                    Else
                        xLinea = xLinea & ClsUtility.FormatExport(dtrVolontari(3)) & ";"
                    End If
                    'sesso
                    If IsDBNull(dtrVolontari(4)) = True Then
                        xLinea = xLinea & vbNullString & ";"
                    Else
                        If dtrVolontari(4) = "0" Then
                            xLinea = xLinea & "M;"
                        Else
                            xLinea = xLinea & "F;"
                        End If
                    End If
                    'codice fiscale
                    If IsDBNull(dtrVolontari(5)) = True Then
                        xLinea = xLinea & vbNullString & ";"
                    Else
                        xLinea = xLinea & ClsUtility.FormatExport(dtrVolontari(5)) & ";"
                    End If
                    'data nascita
                    If IsDBNull(dtrVolontari(6)) = True Then
                        xLinea = xLinea & vbNullString & ";"
                    Else
                        xLinea = xLinea & ClsUtility.FormatExport(dtrVolontari(6)) & ";"
                    End If
                    'codice ente
                    If IsDBNull(dtrVolontari(7)) = True Then
                        xLinea = xLinea & vbNullString & ";"
                    Else
                        xLinea = xLinea & ClsUtility.FormatExport(dtrVolontari(7)) & ";"
                    End If
                    'Denominazione Ente
                    If IsDBNull(dtrVolontari(8)) = True Then
                        xLinea = xLinea & vbNullString & ";"
                    Else
                        xLinea = xLinea & ClsUtility.FormatExport(dtrVolontari(8)) & ";"
                    End If
                    'codice progetto
                    If IsDBNull(dtrVolontari(9)) = True Then
                        xLinea = xLinea & vbNullString & ";"
                    Else
                        xLinea = xLinea & ClsUtility.FormatExport(dtrVolontari(9)) & ";"
                    End If
                    'titolo progetto
                    If IsDBNull(dtrVolontari(10)) = True Then
                        xLinea = xLinea & vbNullString & ";"
                    Else
                        xLinea = xLinea & ClsUtility.FormatExport(dtrVolontari(10)) & ";"
                    End If
                    'Comune Sede Progetto
                    If IsDBNull(dtrVolontari(11)) = True Then
                        xLinea = xLinea & vbNullString & ";"
                    Else
                        xLinea = xLinea & ClsUtility.FormatExport(dtrVolontari(11)) & ";"
                    End If
                    'Provincia Sede Progetto
                    If IsDBNull(dtrVolontari(12)) = True Then
                        xLinea = xLinea & vbNullString & ";"
                    Else
                        xLinea = xLinea & ClsUtility.FormatExport(dtrVolontari(12)) & ";"
                    End If
                    'Regione Sede Progetto
                    If IsDBNull(dtrVolontari(13)) = True Then
                        xLinea = xLinea & vbNullString & ";"
                    Else
                        xLinea = xLinea & ClsUtility.FormatExport(dtrVolontari(13)) & ";"
                    End If
                    'Indirizzo Sede Progetto
                    If IsDBNull(dtrVolontari(14)) = True Then
                        xLinea = xLinea & vbNullString & ";"
                    Else
                        xLinea = xLinea & ClsUtility.FormatExport(dtrVolontari(14)) & ";"
                    End If
                    'CAP
                    If IsDBNull(dtrVolontari(15)) = True Then
                        xLinea = xLinea & vbNullString & ";"
                    Else
                        xLinea = xLinea & ClsUtility.FormatExport(dtrVolontari(15)) & ";"
                    End If
                    'ORE FORMAZIONE PRIMA TRANCHE
                    If IsDBNull(dtrVolontari(16)) = True Then
                        xLinea = xLinea & vbNullString & ";"
                    Else
                        xLinea = xLinea & ClsUtility.FormatExport(dtrVolontari(16)) & ";"
                    End If
                    'ORE FORMAZIONE SECONDA TRANCHE
                    If IsDBNull(dtrVolontari(17)) = True Then
                        xLinea = xLinea & vbNullString & ";"
                    Else
                        xLinea = xLinea & ClsUtility.FormatExport(dtrVolontari(17))
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

    Private Sub imgApprova_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles imgApprova.Click
        Dim dtGriglia As DataTable
        Dim i As Integer
        Dim bolVerifica As Boolean
        Dim intStato As Integer
        Dim strSQL As String
        Dim bolTrovaPrg As Boolean
        bolTrovaPrg = False
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
            lblmessaggio.Text = "Selezionare almeno un progetto dalla lista per effettuare l'approvazione."
        Else

            'aggiorno lo stato dei progetti selezionati
            For i = 0 To dtGriglia.Rows.Count - 1
                If dtGriglia.Rows(i).Item(7) = 1 Then
                    intStato = VediTipoStato(dgRisultatoRicerca.Items(i).Cells(9).Text)
                    If intStato = 1 Then       'lo stato uguale e a 1 indica i progetti confermati dall'Ente
                        strSQL = "Update AttivitàFormazioneGenerale Set AlertChiusureIniziali=0, StatoFormazione=2, DataApprovazione=GetDate(),UserNameApprovazione='" & Session("utente") & "' Where idattività=" & dgRisultatoRicerca.Items(i).Cells(9).Text
                        ClsServer.EseguiSqlClient(strSQL, Session("conn"))
                        bolTrovaPrg = True
                    End If
                End If
            Next

            If bolTrovaPrg = False Then
                lblmessaggio.Text = "Tra i progetti selezionati non ci sono progetti da 'Approvare'."
            Else
                lblmessaggio.Text = "Operazione terminata con successo"
            End If

        End If

        Call ColoraCelleStati()

        Call VisualizzaCaricamentoQuest()
    End Sub

    Private Sub imgInoltra_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles imgInoltra.Click

        '*****************************************************************************************
        'MOD: IL 22/05/2007 DA SIMONA CORDELLA
        'GESTIONE DEI CAMPI NUMEROVOLONTARIRIMBORSABILIITALIA, NUMEROVOLONTARIRIMBORSABILIESTERO
        'NUMEROVOLONTARINONRIMBORSABILIITALIA,NUMEROVOLONTARINONRIMBORSABILIESTERO
        Dim dtGriglia As DataTable
        Dim i As Integer
        Dim bolVerifica As Boolean
        Dim intStato As Integer
        Dim strSQL As String
        Dim bolTrovaPrg As Boolean
        Dim bolTrovaAnomalo As Boolean
        Dim sTitolo As String
        Dim NumeroVolRimborsoItalia As Integer = 0
        Dim NumeroVolRimborsoEstero As Integer = 0
        Dim NumeroVolNoRimborsoItalia As Integer = 0
        Dim NumeroVolNoRimborsoEstero As Integer = 0

        Dim ImportoVolRimborsoItalia As Integer = 0
        Dim ImportoVolRimborsoEstero As Integer = 0
        Dim strIDAttività As String = ""

        bolTrovaPrg = False
        bolVerifica = False
        bolTrovaAnomalo = False
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
            lblmessaggio.Text = "Selezionare almeno un progetto dalla lista per effettuare la conferma."
        Else

            'aggiorno lo stato dei progetti selezionati
            For i = 0 To dtGriglia.Rows.Count - 1
                If dtGriglia.Rows(i).Item(7) = 1 Then
                    intStato = VediTipoStato(dgRisultatoRicerca.Items(i).Cells(9).Text)

                    NumeroVolRimborsoItalia = 0
                    NumeroVolNoRimborsoItalia = 0
                    NumeroVolRimborsoEstero = 0
                    NumeroVolNoRimborsoEstero = 0
                    ImportoVolRimborsoItalia = 0
                    ImportoVolRimborsoEstero = 0

                    If dgRisultatoRicerca.Items(i).Cells(18).Text = "Italia" Then 'nazione base ITALIA
                        NumeroVolRimborsoItalia = CInt(dgRisultatoRicerca.Items(i).Cells(10).Text)
                        ImportoVolRimborsoItalia = CalcolaRimborso(NumeroVolRimborsoItalia, "Italia", CInt(dgRisultatoRicerca.Items(i).Cells(20).Text), 0)
                        NumeroVolNoRimborsoItalia = CInt(dgRisultatoRicerca.Items(i).Cells(8).Text) - CInt(dgRisultatoRicerca.Items(i).Cells(10).Text)
                    Else 'nazione base =0 ESTERO
                        NumeroVolRimborsoEstero = CInt(dgRisultatoRicerca.Items(i).Cells(10).Text)
                        ImportoVolRimborsoEstero = CalcolaRimborso(NumeroVolRimborsoEstero, "Estero", 0, CInt(dgRisultatoRicerca.Items(i).Cells(21).Text))
                        NumeroVolNoRimborsoEstero = CInt(dgRisultatoRicerca.Items(i).Cells(8).Text) - CInt(dgRisultatoRicerca.Items(i).Cells(10).Text)
                    End If
                    'ImportoRimborsabileItalia , ImportoRimborsabileEstero 

                    If intStato = 0 Then         'lo stato uguale e a 2 indica i progetti approvati dall'UNSC                  
                        If VerificagiorniConferma(dgRisultatoRicerca.Items(i).Cells(9).Text) = True Then 'superato i gg per la conferma
                            bolTrovaAnomalo = True
                        Else
                            strSQL = "Update AttivitàFormazioneGenerale Set StatoFormazione=1, DataConferma=GetDate(),UserNameConferma='" & Session("utente") & "' ," & _
                                                           " NumeroVolontariRimborsabiliItalia= " & NumeroVolRimborsoItalia & ", NumeroNonVolontariRimborsabiliItalia = " & NumeroVolNoRimborsoItalia & ", " & _
                                                           " NumeroVolontariRimborsabiliEstero=" & NumeroVolRimborsoEstero & " , NumeroNonVolontariRimborsabiliEstero=" & NumeroVolNoRimborsoEstero & ", " & _
                                                           " ImportoRimborsabileItalia=" & ImportoVolRimborsoItalia & ", " & _
                                                           " ImportoRimborsabileEstero=" & ImportoVolRimborsoEstero & "" & _
                                                           " Where idattività=" & dgRisultatoRicerca.Items(i).Cells(9).Text
                            ClsServer.EseguiSqlClient(strSQL, Session("conn"))
                            bolTrovaPrg = True
                            'bolTrovaAnomalo = False

                            ' Serializzazione degli IDAttività selezionati
                            strIDAttività += "," & dgRisultatoRicerca.Items(i).Cells(9).Text
                        End If
                    End If
                End If
            Next

            RimborsoVolontariFAMI(strIDAttività)

            If bolTrovaAnomalo = True Then
                lblmessaggio.Text = "Operazione terminata con successo. Attenzione, per alcuni dei progetti selezionati non e' stato possibile effettuare la conferma."
            ElseIf bolTrovaPrg = True Then
                lblmessaggio.Text = "Operazione terminata con successo"
            Else
                lblmessaggio.Text = "Tra i progetti selezionati non ci sono progetti da 'Confermare'."
            End If


            'If bolTrovaPrg = False Then
            '    lblmessaggio.Text = "Tra i progetti selezionati non ci sono progetti da 'Confermare'."
            'Else

            'End If
        End If


        Call ColoraCelleStati()

    End Sub

    Private Function CalcolaRimborso(ByVal NumeroVolRimb As Integer, ByVal NazioneBase As String, ByVal RimborsoFormazioneItalia As Integer, ByVal RimborsoFormazioneEstero As Integer)
        'Creata da Simona Cordella il 25/03/2008
        'Calcolo a secondo del TipoProgetto, l'importo totale da rimbarare per tutti i volontari rimborsabili del progetto

        Dim StrCalcolo As String = 0
        StrCalcolo = 0
        If NazioneBase = "Italia" Then
            StrCalcolo = NumeroVolRimb * RimborsoFormazioneItalia
        Else
            StrCalcolo = NumeroVolRimb * RimborsoFormazioneEstero
        End If
        Return StrCalcolo
    End Function

    Private Function VerificagiorniConferma(ByVal IdProg As Integer) As Boolean
        '*****************************************************************************************+

        'DESCRIZONE: controllo date progetto per effettuare Conferma ORE
        Dim strSQL As String
        Dim bEsitoData As Boolean
        Dim iGiorni As Integer
        Dim intTipoFormazioneGen As Integer
        Dim strDataProroga As String
        bEsitoData = False

        'Gestione temporale stampa questionario     <=150 gg
        intTipoFormazioneGen = RitornaTipoFormazioneGenerale(IdProg)
        If intTipoFormazioneGen = 1 Then
            iGiorni = 180
        Else
            iGiorni = 270
        End If

        'verifo eventuali proroghe

        'vedo se è stata impostata la data proroga
        strSQL = "Select ISNULL(CONVERT(varchar, DataProroga, 103), 0) AS DataProroga From AttivitàFormazioneGenerale Where idattività = " & IdProg
        If Not dtrGenerico Is Nothing Then
            dtrGenerico.Close()
            dtrGenerico = Nothing
        End If

        dtrGenerico = ClsServer.CreaDatareader(strSQL, Session("conn"))
        dtrGenerico.Read()

        If dtrGenerico.HasRows Then
            strDataProroga = dtrGenerico("DataProroga")
        End If
        If Not dtrGenerico Is Nothing Then
            dtrGenerico.Close()
            dtrGenerico = Nothing
        End If

        If strDataProroga <> "0" Then

            strSQL = "SELECT isnull(datediff(dd,DataProroga,getdate()),-1) as DiffGG " & _
                     " FROM AttivitàFormazioneGenerale " & _
                     " WHERE idattività = " & IdProg
            dtrGenerico = ClsServer.CreaDatareader(strSQL, Session("conn"))
            dtrGenerico.Read()

            If dtrGenerico("DiffGG") <= 7 Then
                '    bEsitoData = True               'proroga scaduta
                ' Else
                bEsitoData = False 'proroga 7 giorni valida
                If Not dtrGenerico Is Nothing Then
                    dtrGenerico.Close()
                    dtrGenerico = Nothing
                End If

                Return bEsitoData

                Exit Function
            End If
   
        End If
        If Not dtrGenerico Is Nothing Then
            dtrGenerico.Close()
            dtrGenerico = Nothing
        End If
 
        strSQL = " Select a.IDAttività "
        strSQL &= " FROM Attività a "
        strSQL &= " INNER JOIN AttivitàFormazioneGenerale afg on a.IDAttività=afg.IdAttività"
        strSQL &= " WHERE a.IDAttività=" & IdProg & ""
        If intTipoFormazioneGen = 1 Then 'Formazione UNICATRANCHE
            strSQL &= " AND GETDATE() < afg.DataScadenzaUnicaTranche "
        Else 'Formazione DUE TRANCHE
            strSQL &= " AND GETDATE() < afg.DataScadenzaSecondaTranche "
        End If

        'strSQL = "SELECT isnull(datediff(dd,DataInizioAttività,getdate()),0) as DiffGG, Titolo " & _
        '        " FROM attività WHERE IDAttività = " & IdProg

        dtrGenerico = ClsServer.CreaDatareader(strSQL, Session("conn"))
       
        If dtrGenerico.HasRows Then
            dtrGenerico.Read()
            bEsitoData = False
        Else
            '    If dtrGenerico("DiffGG") > iGiorni Then
            bEsitoData = True
            '    End If
        End If


        If Not dtrGenerico Is Nothing Then
            dtrGenerico.Close()
            dtrGenerico = Nothing
        End If

        Return bEsitoData

    End Function

    Function controlliServer() As Boolean

        Dim numeroRiferimento As Integer
        Dim numeroRiferimentoInteger As Boolean
        numeroRiferimentoInteger = Integer.TryParse(txtRif.Text.Trim, numeroRiferimento)

        If divStato.Visible = True And txtRif.Text.Trim <> String.Empty Then

            If numeroRiferimentoInteger = False Then
                lblmessaggio.Visible = True
                lblmessaggio.Text = "Il Riferimento essere un numero intero."
                txtRif.Focus()
                Return False
            End If

        End If

        Return True

    End Function

    Private Sub imgStampa_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles imgStampa.Click
        '*****************************************************************************************+
        'AUTORE: Guido Testa 
        'DATA: 26/04/2006
        'DESCRIZONE: controllo se è stato selezionato almeno un progetto prima di 
        'avviare la stampa del modulo
        '*****************************************************************************************
        Dim dtGriglia As DataTable
        Dim i As Integer
        Dim bolVerifica As Boolean
        Dim intStato As Integer
        Dim sTitolo As String
        bolVerifica = False

        Call RiccoraItemSelezionato()

        Call ColoraCelleStati()

        dtGriglia = Session("DtbRicVol")

        'verifico se è stato selezionato almeno un progetto
        For i = 0 To dtGriglia.Rows.Count - 1
            If dtGriglia.Rows(i).Item(7) = 1 Then
                bolVerifica = True
                Exit For
            End If
        Next

        'verifico se ci sono progetti da più di 155 gg (non è possibile stamparli)
        If Session("TipoUtente") <> "U" Then
            For i = 0 To dtGriglia.Rows.Count - 1
                If dtGriglia.Rows(i).Item(7) = 1 Then
                    If Verificagiorni(dtGriglia.Rows(i).Item(5), 1, sTitolo) = True Then
                        lblmessaggio.Text = "Sono scaduti i termini per eseguire la stampa del progetto: '" & sTitolo & "'."
                        Exit Sub
                    End If
                End If
            Next
        End If

        Dim intSomma As Integer = 0
        Dim intTotSomma As Integer = 0
        For i = 0 To dtGriglia.Rows.Count - 1
            If dtGriglia.Rows(i).Item(7) = 1 Then 'colonna selezionato(check
                intSomma = CalcolaRimborso(dtGriglia.Rows(i).Item(13), dtGriglia.Rows(i).Item(20), dtGriglia.Rows(i).Item(22), dtGriglia.Rows(i).Item(23))
                intTotSomma = intTotSomma + intSomma
            End If
        Next




        If bolVerifica = False Then
            lblmessaggio.Text = "Selezionare almeno un progetto dalla lista per effettuare la stampa."
        Else
            'verifico che i progetti selezionati abbiano lo stato di "confermato" impostato
            For i = 0 To dtGriglia.Rows.Count - 1
                If dtGriglia.Rows(i).Item(7) = 1 Then
                    intStato = VediTipoStato(dgRisultatoRicerca.Items(i).Cells(9).Text)
                    If intStato <> 1 Then       'lo stato ugula e a 1 indica i progetti confermati
                        lblmessaggio.Text = "E' possibile eseguire la stampa solo dei progetti confermati."
                        Exit Sub
                    End If
                End If
            Next
            AggiornaImportiPrimadellaStampa_ModuloF(dtGriglia)

            lblmessaggio.Text = ""
            Response.Write("<script>")
            Response.Write("window.open('WfrmDatiStampaQuest.aspx?TipoFormazioneGenerale=" & ddlTipoFormazioneGenerale.SelectedValue & "&Importo=" & intTotSomma & "', 'DatiStampa', 'width=1000, height=600, status=no, toolbar=no, location=no, menubar=no, scrollbars=yes')")
            Response.Write("</script>")
        End If
    End Sub

    Private Function RitornaTipoFormazioneGenerale(ByVal IdProg As Integer) As Integer
        '** creata a SIMONA CORDELLA
        '** 01/06/2015
        '** Ritorna il tipoformazione 1 unicatranche 2 due tranche

        Dim strSql As String
        Dim TipoFormazioneGenerale As Integer
        Dim dtrgenerico As SqlClient.SqlDataReader

        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
        strSql = "SELECT TipoFormazioneGenerale FROM AttivitàFormazioneGenerale WHERE IdAttività = " & IdProg
        dtrgenerico = ClsServer.CreaDatareader(strSql, Session("conn"))
        If dtrgenerico.HasRows = True Then
            dtrgenerico.Read()
            TipoFormazioneGenerale = dtrgenerico("TipoFormazioneGenerale")
        End If
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
        Return TipoFormazioneGenerale
    End Function

    Public Sub TuttaPaginaSess()
        Session("TP") = True
    End Sub

    Sub AggiornaImportiPrimadellaStampa_ModuloF(ByVal dtGriglia As DataTable)
        ' Dim dtGriglia As DataTable
        Dim i As Integer
        'Dim bolVerifica As Boolean
        'Dim intStato As Integer
        Dim strSQL As String
        Dim bolTrovaPrg As Boolean
        'Dim bolTrovaAnomalo As Boolean
        'Dim sTitolo As String
        Dim NumeroVolRimborsoItalia As Integer = 0
        Dim NumeroVolRimborsoEstero As Integer = 0
        Dim NumeroVolNoRimborsoItalia As Integer = 0
        Dim NumeroVolNoRimborsoEstero As Integer = 0

        Dim ImportoVolRimborsoItalia As Integer = 0
        Dim ImportoVolRimborsoEstero As Integer = 0
        Dim strIDAttività As String = ""

        For i = 0 To dtGriglia.Rows.Count - 1
            If dtGriglia.Rows(i).Item(7) = 1 Then
                'intStato = VediTipoStato(dgRisultatoRicerca.Items(i).Cells(9).Text)

                NumeroVolRimborsoItalia = 0
                NumeroVolNoRimborsoItalia = 0
                NumeroVolRimborsoEstero = 0
                NumeroVolNoRimborsoEstero = 0
                ImportoVolRimborsoItalia = 0
                ImportoVolRimborsoEstero = 0

                If dgRisultatoRicerca.Items(i).Cells(18).Text = "Italia" Then 'nazione base ITALIA
                    NumeroVolRimborsoItalia = CInt(dgRisultatoRicerca.Items(i).Cells(10).Text)
                    ImportoVolRimborsoItalia = CalcolaRimborso(NumeroVolRimborsoItalia, "Italia", CInt(dgRisultatoRicerca.Items(i).Cells(20).Text), 0)
                    NumeroVolNoRimborsoItalia = CInt(dgRisultatoRicerca.Items(i).Cells(8).Text) - CInt(dgRisultatoRicerca.Items(i).Cells(10).Text)
                Else 'nazione base =0 ESTERO
                    NumeroVolRimborsoEstero = CInt(dgRisultatoRicerca.Items(i).Cells(10).Text)
                    ImportoVolRimborsoEstero = CalcolaRimborso(NumeroVolRimborsoEstero, "Estero", 0, CInt(dgRisultatoRicerca.Items(i).Cells(21).Text))
                    NumeroVolNoRimborsoEstero = CInt(dgRisultatoRicerca.Items(i).Cells(8).Text) - CInt(dgRisultatoRicerca.Items(i).Cells(10).Text)
                End If
                'ImportoRimborsabileItalia , ImportoRimborsabileEstero 

                'If intStato = 0 Then         'lo stato uguale e a 2 indica i progetti approvati dall'UNSC                  
                'If VerificagiorniConferma(dgRisultatoRicerca.Items(i).Cells(9).Text) = True Then 'superato i gg per la conferma
                'bolTrovaAnomalo = True
                'Else
                strSQL = "Update AttivitàFormazioneGenerale Set " & _
                                               " NumeroVolontariRimborsabiliItalia= " & NumeroVolRimborsoItalia & ", NumeroNonVolontariRimborsabiliItalia = " & NumeroVolNoRimborsoItalia & ", " & _
                                               " NumeroVolontariRimborsabiliEstero=" & NumeroVolRimborsoEstero & " , NumeroNonVolontariRimborsabiliEstero=" & NumeroVolNoRimborsoEstero & ", " & _
                                               " ImportoRimborsabileItalia=" & ImportoVolRimborsoItalia & ", " & _
                                               " ImportoRimborsabileEstero=" & ImportoVolRimborsoEstero & "" & _
                                               " Where idattività=" & dgRisultatoRicerca.Items(i).Cells(9).Text
                ClsServer.EseguiSqlClient(strSQL, Session("conn"))
                bolTrovaPrg = True
                ' Serializzazione degli IDAttività selezionati
                strIDAttività += "," & dgRisultatoRicerca.Items(i).Cells(9).Text

                'bolTrovaAnomalo = False
                ' End If
                'End If
            End If
        Next
        RimborsoVolontariFAMI(strIDAttività)
    End Sub

    Private Sub RimborsoVolontariFAMI(ByVal vlIDAttività As String)
        Dim dtGriglia As DataTable

        If vlIDAttività <> "" Then
            ' Richiamo della storedProcedure
            Dim SqlCmd As New SqlClient.SqlCommand
            vlIDAttività += ","
            Try
                SqlCmd.CommandText = "SP_FormazioneRimborsabiliFAMI"
                SqlCmd.CommandType = CommandType.StoredProcedure
                SqlCmd.Connection = Session("Conn")

                SqlCmd.Parameters.Add("@IDAttività ", SqlDbType.VarChar, 8000).Value = vlIDAttività

                'Esito aggiornamento: 0-Errore 1-Aggiornamento effettuato
                SqlCmd.Parameters.Add("@messaggio", SqlDbType.VarChar, 1000)
                SqlCmd.Parameters("@messaggio").Direction = ParameterDirection.Output

                SqlCmd.ExecuteNonQuery()

                'lblmessaggio.Text = SqlCmd.Parameters("@messaggio").Value()

            Catch ex As Exception
                lblmessaggio.Text = "ERRORE IMPREVISTO DURANTE L'ELABORAZIONE. "
            Finally

            End Try
        End If
    End Sub
End Class