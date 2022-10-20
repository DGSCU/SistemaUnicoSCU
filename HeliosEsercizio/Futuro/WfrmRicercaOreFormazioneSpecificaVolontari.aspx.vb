Imports System.Data.SqlClient
Imports System.IO

Public Class WfrmRicercaOreFormazioneSpecificaVolontari
    Inherits System.Web.UI.Page

    Dim dtrGenerico As SqlClient.SqlDataReader
    Dim strquery As String
    Public sEsitoRicerca As Boolean
    Dim PROGETTO_GARANZIA_GIOVANI As String = "4"

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
#End Region
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        VerificaSessione()

        If (Session("TipoUtente") = "U" Or Session("TipoUtente") = "R") Then
            divCodiceEnte.Visible = True
        Else
            divCodiceEnte.Visible = False
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

            txtCodReg.Text = Mid(Session("CodiceRegioneEnte"), 2, 7)

        End If

        'Controllo sul tipo di utente
        If Session("TipoUtente") = "E" Then
            'Disabilito la possibilità di selezionare la denominazione dell'ente
            lblDenEnte.Visible = False
            txtDenominazioneEnte.Visible = False

            divCodiceEnte.Visible = False
        Else
            lblDenEnte.Visible = True
            txtDenominazioneEnte.Visible = True

            divCodiceEnte.Visible = True
        End If

        If Request.QueryString("VediEnte") = 0 Then     'UNSC/Regione
            lblDenEnte.Visible = True
        Else                                            'Ente
            lblDenEnte.Visible = False
            divCodiceEnte.Visible = False
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
        strsql = strsql & " WHERE tp.idtipoprogetto = 4 and tp.MacroTipoProgetto like '" & Session("FiltroVisibilita") & "'"
        strsql = strsql & " ORDER BY bando.annobreve desc"
        '"select idbando, bandobreve from Bando where FormazioneGenerale=1"
        DdlBando.DataSource = MakeParentTable(strsql)
        DdlBando.DataTextField = "ParentItem"
        DdlBando.DataValueField = "id"
        DdlBando.DataBind()

       
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
        Response.Redirect("WfrmImportOreFormazioneSpecificaVolontari.aspx")
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
        lblErrore.Text = String.Empty
        lblmessaggio.Text = String.Empty
        lblConferma.Text = String.Empty

        If Session("TipoUtente") = "E" Then
            If DdlBando.SelectedItem.Text <> "" Then
                txtDenominazioneEnte1.Value = txtDenominazioneEnte.Text
                ddlMaccCodAmAtt1.Value = ddlMaccCodAmAtt.SelectedIndex
                ddlCodAmAtt1.Value = ddlCodAmAtt.SelectedIndex
                ddlStatoAttivita1.Value = ddlStatoAttivita.SelectedIndex
                dgRisultatoRicerca.CurrentPageIndex = 0
                EseguiRicerca(0)
            Else
                lblErrore.Text = "Occorre selezionare una circolare."
            End If
        Else
            txtDenominazioneEnte1.Value = txtDenominazioneEnte.Text
            ddlMaccCodAmAtt1.Value = ddlMaccCodAmAtt.SelectedIndex
            ddlCodAmAtt1.Value = ddlCodAmAtt.SelectedIndex
            ddlStatoAttivita1.Value = ddlStatoAttivita.SelectedIndex
            dgRisultatoRicerca.CurrentPageIndex = 0
            EseguiRicerca(0)
        End If

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
        " a.idattività as IdAttivita,b.idente," & _
        " '0' as selezionato, a.IdTipoProgetto,e.statoattività,  " & _
        " (SELECT     isnull(SUM(entità.OreFormazioneSpecifica), 0) " & _
        " FROM entità INNER JOIN " & _
        " attivitàentità ON entità.IDEntità = attivitàentità.IDEntità INNER JOIN " & _
        " attivitàentisediattuazione ON  attivitàentità.IDAttivitàEnteSedeAttuazione = attivitàentisediattuazione.IDAttivitàEnteSedeAttuazione INNER JOIN " & _
        " attività ON attivitàentisediattuazione.IDAttività = attività.IDAttività " & _
        " WHERE (attività.IDAttività = a.IDAttività)) AS OreFormazioneSpecifica, "
        strquery = strquery & " (SELECT CASE WHEN (SELECT  COUNT(DISTINCT entità.CodiceVolontario) " & _
                " FROM StatiEntità INNER JOIN entità INNER JOIN attivitàentità ON entità.IDEntità = attivitàentità.IDEntità INNER JOIN " & _
                " attivitàentisediattuazione ON attivitàentità.IDAttivitàEnteSedeAttuazione = attivitàentisediattuazione.IDAttivitàEnteSedeAttuazione " & _
                " INNER JOIN attività ON attivitàentisediattuazione.IDAttività = attività.IDAttività INNER JOIN " & _
                " enti ON attività.IDEntePresentante = enti.IDEnte ON StatiEntità.IDStatoEntità = entità.IDStatoEntità " & _
                " WHERE attività.IDAttività = a.IDAttività And (StatiEntità.InServizio = 1 OR StatiEntità.Sospeso = 1  OR  StatiEntità.chiuso = 1)) <> 0 THEN "
        strquery = strquery & " '1' "
        strquery = strquery & " else '0' End) as VisualizzaLinkVolontari, "
        strquery = strquery & " (SELECT COUNT(DISTINCT entità.CodiceVolontario) " & _
                " FROM StatiEntità INNER JOIN entità INNER JOIN attivitàentità ON entità.IDEntità = attivitàentità.IDEntità INNER JOIN " & _
                " attivitàentisediattuazione ON attivitàentità.IDAttivitàEnteSedeAttuazione = attivitàentisediattuazione.IDAttivitàEnteSedeAttuazione " & _
                " INNER JOIN attività ON attivitàentisediattuazione.IDAttività = attività.IDAttività INNER JOIN " & _
                " enti ON attività.IDEntePresentante = enti.IDEnte ON StatiEntità.IDStatoEntità = entità.IDStatoEntità " & _
                " WHERE attività.IDAttività = a.IDAttività And (StatiEntità.InServizio = 1 OR StatiEntità.Sospeso = 1  OR StatiEntità.Chiuso = 1) and attivitàentità.EscludiFormazione=0)  AS Volontari,  "
        strquery = strquery & " QuestionarioStoricoProgetti.NumeroVolontari as VolQuest, DurataFormazioneGenerale as OrePrev, Convert(VarChar,AttivitàFormazioneGenerale.dataproroga,103) as dataproroga,  "
        strquery = strquery & "(case convert(varchar,c.PianificazioneFormazione) when '0' then 'Non Necessario' else case isnull(ltrim(rtrim(AttivitàFormazioneGenerale.sedecorso)),'') WHEN ''  THEN 'No' else 'Si' end end ) as DatiPianificazione, "
        strquery = strquery & " (case TipiProgetto.nazionebase when 0 then 'Estero' else 'Italia' end) as nazionebase,c.idbando "

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

        strquery = strquery & " and a.idTipoProgetto=" & PROGETTO_GARANZIA_GIOVANI & ""

        'imposto eventuali parametri
        If ddlStatoAttivita.SelectedItem.Text <> "" Then
            strquery = strquery & " and e.idstatoattività=" & ddlStatoAttivita.SelectedValue & ""
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
            'Else
            '    'prendo solo i bandi con il campo FormazioneGenerale=1
            '    strquery = strquery & " and c.FormazioneGenerale=1 "
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
        'se vengo dalla ricerca dei questionari forzo il  caricamento dei progetti con ore formazione
        If Request.QueryString("VengoDa") = "Ricerca" Then      'estrazione questionari            
            If Request.QueryString("VediEnte") = 0 Then     'UNSC/Regione
                optOreTutte.Checked = True
            Else
                optOreSi.Checked = True
            End If
        End If

        If optOreSi.Checked = True Then
            strquery = strquery & " and (SELECT     isnull(SUM(entità.OreFormazioneSpecifica), 0) " & _
                " FROM entità INNER JOIN " & _
                " attivitàentità ON entità.IDEntità = attivitàentità.IDEntità INNER JOIN " & _
                " attivitàentisediattuazione ON  attivitàentità.IDAttivitàEnteSedeAttuazione = attivitàentisediattuazione.IDAttivitàEnteSedeAttuazione INNER JOIN " & _
                " attività ON attivitàentisediattuazione.IDAttività = attività.IDAttività " & _
                " WHERE (attività.IDAttività = a.IDAttività)) > 0 "
        ElseIf optOreNo.Checked = True Then
            strquery = strquery & " and (SELECT     isnull(SUM(entità.OreFormazioneSpecifica), 0) " & _
                " FROM entità INNER JOIN " & _
                " attivitàentità ON entità.IDEntità = attivitàentità.IDEntità INNER JOIN " & _
                " attivitàentisediattuazione ON  attivitàentità.IDAttivitàEnteSedeAttuazione = attivitàentisediattuazione.IDAttivitàEnteSedeAttuazione INNER JOIN " & _
                " attività ON attivitàentisediattuazione.IDAttività = attività.IDAttività " & _
                " WHERE (attività.IDAttività = a.IDAttività)) = 0 "
        End If

        'filtrovisibilita 02/12/2014 da s.c.
        strquery = strquery & " and TipiProgetto.Macrotipoprogetto like '" & Session("FiltroVisibilita") & "'"

        strquery = strquery & " and AssociaUtenteGruppo.username='" & Replace(Session("Utente"), "'", "''") & "'  ORDER BY e.StatoAttività, b.Denominazione,Titolo"

        Mydataset = ClsServer.DataSetGenerico(strquery, Session("conn"))
        dgRisultatoRicerca.DataSource = Mydataset
        dgRisultatoRicerca.DataBind() 'valorizzo griglia

        Session("DtbRicVol") = Mydataset.Tables(0)

        If dgRisultatoRicerca.Items.Count = 0 Then 'se la griglia e vuota la nascondo
            dgRisultatoRicerca.Caption = "La ricerca non ha prodotto alcun risultato"
            checkSelDesel.Visible = False
            checkSelDesel.Checked = False
            checkSelDesel.Text = "Seleziona Tutto"
        Else
            checkSelDesel.Visible = True
            dgRisultatoRicerca.Caption = "Risultato Ricerca Progetti"
        End If

        If dgRisultatoRicerca.Items.Count > 0 Then
            checkSelDesel.Visible = True
            If Request.QueryString("VengoDa") = "Ricerca" Then      'estrazione questionari
                CmdEsporta.Visible = False

                If Request.QueryString("VediEnte") = 0 Then     'UNSC/Regione
                    dgRisultatoRicerca.Columns(10).Visible = True       'questionario
                    dgRisultatoRicerca.Columns(11).Visible = True       'volontari questionario
                    dgRisultatoRicerca.Columns(12).Visible = True       'Ore previste
                    dgRisultatoRicerca.Columns(13).Visible = True       'DataProroga
                    dgRisultatoRicerca.Columns(22).Visible = True       'Dati pianificazione

                    Call VisualizzaCaricamentoQuest()
                End If

            Else
                CmdEsporta.Visible = True
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



    Private Sub VisualizzaCaricamentoQuest()
        Dim item As DataGridItem
        Dim color As New System.Drawing.Color
        Dim Mychk As CheckBox
        Dim x As Integer
        Dim intStato As Integer

        For Each item In dgRisultatoRicerca.Items

            ''vedo se è stato fatto il questionario
            If VediQuestionario(dgRisultatoRicerca.Items(item.ItemIndex).Cells(9).Text) = True Then
                dgRisultatoRicerca.Items(item.ItemIndex).Cells(11).Text = "Si"
            Else
                dgRisultatoRicerca.Items(item.ItemIndex).Cells(11).Text = "No"
            End If

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
        bEsitoData = False

        If sTipo = 1 Then               'Gestione temporale stampa questionario     <=150 gg
            iGiorni = 150
        ElseIf sTipo = 2 Then           'Gestione temporale conferma caricamento ore <=90 gg
            iGiorni = 90
        End If

        If sTipo = 1 Then           'verifo eventuali proroghe

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

        strSQL = "SELECT isnull(datediff(dd,DataInizioAttività,getdate()),0) as DiffGG, Titolo " & _
                " FROM attività WHERE IDAttività = " & IdProg

        dtrGenerico = ClsServer.CreaDatareader(strSQL, Session("conn"))
        dtrGenerico.Read()

        If dtrGenerico.HasRows Then
            sTitolo = dtrGenerico("Titolo")
            If sTipo = 1 Then               'Gestione temporale stampa questionario     <=150 gg

                If dtrGenerico("DiffGG") > iGiorni Then
                    bEsitoData = True
                End If

            ElseIf sTipo = 2 Then           'Gestione temporale conferma caricamento ore <=90 gg

                If dtrGenerico("DiffGG") <= iGiorni Then
                    bEsitoData = True
                End If

            End If
        End If

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
        ChiudiDataReader(dtrgenerico)
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

    Private Function OreFormazioneSpecifica(ByVal IdProg As Integer) As Boolean
        '*****************************************************************************************+
        'AUTORE: Guido Testa 
        'DATA: 03/05/2006
        'DESCRIZONE: verifca se sono state caricate le ore di formazione sui volontari relativi al progetto indicato come parametro
        Dim StrSQL As String
        Dim bolEsito As Boolean
        bolEsito = False

        StrSQL = "SELECT  ISNULL(SUM(entità.OreFormazioneSpecifica), 0) as Ore " & _
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



    Private Function VediQuestionario(ByVal IdProg As Integer) As Boolean
        '*****************************************************************************************+
        'AUTORE: Guido Testa 
        'DATA: 03/05/2006
        'DESCRIZONE: verifca se è stato compilato il questionario per il progetto indicato come parametro

        Dim StrSQL As String
        Dim bolEsito As Boolean
        bolEsito = False

        StrSQL = "SELECT attività.IDAttività " _
               & " FROM BandiAttività INNER JOIN " _
               & " attività ON BandiAttività.IdBandoAttività = attività.IDBandoAttività INNER JOIN " _
               & " QuestionarioStoricoProgetti ON BandiAttività.IdBandoAttività = QuestionarioStoricoProgetti.IdBandoAttività " _
               & " WHERE (attività.IDAttività = " & IdProg & ")"

        dtrGenerico = ClsServer.CreaDatareader(StrSQL, Session("conn"))

        bolEsito = dtrGenerico.HasRows
        dtrGenerico.Close()
        dtrGenerico = Nothing

        Return bolEsito

    End Function

    Private Sub dgRisultatoRicerca_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgRisultatoRicerca.PageIndexChanged
        Call RiccoraItemSelezionato(e)
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

    Private Sub dgRisultatoRicerca_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgRisultatoRicerca.ItemCommand
        Select Case e.CommandName
            Case "VisualizzaVolontari"
                Dim idAttivita As String = (e.Item.Cells(9).Text)
                Dim visualizzaVolontari As Boolean = CBool(e.Item.Cells(17).Text)
                If (visualizzaVolontari) Then
                    Response.Write("<script>" & vbCrLf)
                    Response.Write("window.open(""WfrmElencoVolontari.aspx?IdAttivita=" & idAttivita & " &Spec=1"", """", ""width=800,height=600,toolbar=no,location=no,menubar=no,scrollbars=yes,resizable=yes"")" & vbCrLf)
                    Response.Write("</script>")
                End If

        End Select
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

    Private Sub checkSelDesel_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles checkSelDesel.CheckedChanged
        Dim i As Integer
        Dim Mychk As CheckBox

        hlVolontari.Visible = False

        '---determino cosa è stato checkato nella pag corrente e lo salvo nella datatable di sessione
        For i = 0 To dgRisultatoRicerca.Items.Count - 1
            Mychk = dgRisultatoRicerca.Items.Item(i).FindControl("chkSelProg")

            If (Mychk.Enabled = True) Then
                Mychk.Checked = checkSelDesel.Checked
            End If
        Next i
        If (checkSelDesel.Checked) Then
            checkSelDesel.Text = "Deseleziona Tutto"

        Else
            checkSelDesel.Text = "Seleziona Tutto"
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
        dtGriglia = Session("DtbRicVol")

        'verifico se è stato selezionato almeno un progetto
        For i = 0 To dtGriglia.Rows.Count - 1
            If dtGriglia.Rows(i).Item(7) = 1 Then
                bolVerifica = True
                Exit For
            End If
        Next
        If bolVerifica = False Then
            lblErrore.Text = "Selezionare almeno un progetto dalla lista"
        Else
            lblErrore.Text = String.Empty

            EsportaVolontari()
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
                     "entisedi.CAP AS CapSedeProgetto, isnull(entità.OreFormazioneSpecifica,0) as OreFormazioneSpecifica,attività.IDAttività " & _
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
                        xLinea = "CodiceVolontario;Cognome;Nome;Sesso;CodiceFiscale;DataNascita;CodiceEnte;Denominazione;CodiceProgetto;Titolo;ComuneSedeProgetto;ProvinciaSedeProgetto;RegioneSedeProgetto;IndirizzoSedeProgetto;CAP;OreFormazioneSpecifica"
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
            End If

        Catch ex As Exception
            lblErrore.Text = lblmessaggio.Text & "Errore durante l'esportazione dei Volontari."
  
        Finally
            ChiudiDataReader(dtrVolontari)
            If Not Writer Is Nothing Then
                Writer.Close()
                Writer = Nothing
            End If
        End Try
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
        Dim strDataProroga As String
        bEsitoData = False

        'Gestione temporale stampa questionario     <=150 gg
        iGiorni = 150

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



        strSQL = "SELECT isnull(datediff(dd,DataInizioAttività,getdate()),0) as DiffGG, Titolo " & _
                " FROM attività WHERE IDAttività = " & IdProg

        dtrGenerico = ClsServer.CreaDatareader(strSQL, Session("conn"))
        dtrGenerico.Read()

        If dtrGenerico.HasRows Then
            If dtrGenerico("DiffGG") > iGiorni Then
                bEsitoData = True
            End If
        End If
        ChiudiDataReader(dtrGenerico)
        Return bEsitoData

    End Function

    Private Sub dgRisultatoRicerca_ItemDataBound(ByVal source As Object, ByVal e As DataGridItemEventArgs) Handles dgRisultatoRicerca.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            e.Item.Cells(14).Enabled = CBool(e.Item.Cells(17).Text)
            If Not (CBool(e.Item.Cells(17).Text)) Then
                e.Item.Cells(14).ToolTip = "Pulsante non abilitato"
            Else
                e.Item.Cells(14).ToolTip = "Visualizza Volontari"
            End If
        End If


    End Sub
End Class