Imports System.Data.SqlClient
Imports System.IO

Public Class WfrmProgettidaValutare
    Inherits System.Web.UI.Page
    Dim dtrGenerico As SqlClient.SqlDataReader
    Dim strquery As String
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
    Private Sub CancellaMessaggiInfo()
        msgErrore.Text = String.Empty
        msgInfo.Text = String.Empty
        msgConferma.Text = String.Empty
    End Sub
#End Region
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load
        VerificaSessione()
        If ClsUtility.ForzaPresenzaSanzioneProgetto(Session("Utente"), Session("conn")) = False Then
            dgRisultatoRicerca.Columns(16).Visible = False
            ddlSegnalazioneSanzione.Visible = False
            lblSanzione.Visible = False
        End If
        If IsPostBack = False Then
            CaricaCompetenze()
            'richiamo sub dove popolo combo
            CaricaDatiIniziali()
            'controllo provenienza chiamata
            If Request.QueryString("VengoDa") = "Valutare" Then
                lblDenEnte.Text = "Den. Ente"
            Else
                lblDenEnte.Visible = False
                txtDenominazioneEnte.Visible = False
                If Request.QueryString("VengoDa") <> "Valutare" Then
                    txtDenominazioneEnte.Text = Session("denominazione")
                End If
            End If
            CaricaBando()

            If Session("CodiceRegioneEnte") <> "" Then
                txtCodEnte.Text = Session("txtCodEnte")
            End If
        End If
        If dgRisultatoRicerca.Items.Count > 0 Then
            CmdEsporta.Visible = True
        Else
            CmdEsporta.Visible = False
        End If
        'aggiunto il 05/05/2014 da simona cordella
        If ClsUtility.ForzaStatoValutazione(Session("Utente"), Session("conn")) Then
            LblStatoValutazione.Visible = True
            ddlStatoValutazione.Visible = True
        End If
    End Sub
    Private Sub CaricaBando()
        Dim strsql As String
        strsql = " SELECT Bando.idBando,bando.bandobreve,bando.annobreve  "
        strsql = strsql & " FROM bando"
        strsql = strsql & " INNER JOIN AssociaBandoTipiProgetto abtp on abtp.idbando =  bando.idbando"
        strsql = strsql & " INNER JOIN TipiProgetto  tp on abtp.idtipoprogetto = tp.idtipoprogetto"
        strsql = strsql & " WHERE tp.MacroTipoProgetto like '" & Session("FiltroVisibilita") & "'"
        strsql = strsql & " UNION "
        strsql = strsql & " SELECT  '0',' TUTTI ', 99  from bando "
        strsql = strsql & " ORDER BY Bando.annobreve desc"
        ChiudiDataReader(dtrGenerico)
        dtrGenerico = ClsServer.CreaDatareader(strsql, Session("conn"))
        txtbando.DataSource = dtrGenerico
        txtbando.DataTextField = "bandobreve"
        txtbando.DataValueField = "IdBando"
        txtbando.DataBind()
        ChiudiDataReader(dtrGenerico)
    End Sub

    Private Sub cmdChiudi_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles cmdChiudi.Click
        Response.Redirect("WfrmMain.aspx")
    End Sub
    Sub CaricaCompetenze()
        Dim strSQL As String
        Dim dtrCompetenze As System.Data.SqlClient.SqlDataReader

        Try
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

            ChiudiDataReader(dtrCompetenze)
            dtrCompetenze = ClsServer.CreaDatareader(strSQL, Session("conn"))
            CboCompetenza.DataSource = dtrCompetenze
            CboCompetenza.Items.Add("")
            CboCompetenza.DataTextField = "Descrizione"
            CboCompetenza.DataValueField = "IDRegioneCompetenza"
            CboCompetenza.DataBind()
            ChiudiDataReader(dtrCompetenze)
            If Session("TipoUtente") = "U" Then
                CboCompetenza.Enabled = True
                CboCompetenza.SelectedIndex = 0

            Else
                CboCompetenza.Enabled = False
                strSQL = "select b.IdRegioneCompetenza ,b.Heliosread from RegioniCompetenze a "
                strSQL = strSQL & "INNER JOIN utentiunsc b ON a.idregionecompetenza = b.idregionecompetenza "
                strSQL = strSQL & "where b.username = '" & Session("Utente") & "'"
                ChiudiDataReader(dtrCompetenze)
                dtrCompetenze = ClsServer.CreaDatareader(strSQL, Session("conn"))
                dtrCompetenze.Read()
                If dtrCompetenze.HasRows = True Then
                    CboCompetenza.SelectedValue = dtrCompetenze("IdRegioneCompetenza")
                    If dtrCompetenze("Heliosread") = True Then
                        CboCompetenza.Enabled = True
                    End If

                End If

                If Session("TipoUtente") = "R" Then
                    CboCompetenza.Enabled = False
                End If

            End If
        Catch ex As Exception
            Response.Write(ex.Message.ToString())
        Finally
            ChiudiDataReader(dtrCompetenze)
        End Try

    End Sub
    Private Sub CaricaDatiIniziali()
        Dim dtrgenerico As Data.SqlClient.SqlDataReader
        ddlMaccCodAmAtt.DataSource = MakeParentTable("select idmacroambitoattività, codifica + ' - ' + MacroAmbitoAttività as Macro from macroambitiattività")
        ddlMaccCodAmAtt.DataTextField = "ParentItem"
        ddlMaccCodAmAtt.DataValueField = "id"
        ddlMaccCodAmAtt.DataBind()
        ddlCodAmAtt.Items.Add("")
        ddlCodAmAtt.Enabled = False

        ddlStatoAttivita.DataSource = MakeParentTable("select idstatoattività, statoattività from statiattività")
        ddlStatoAttivita.DataTextField = "ParentItem"
        ddlStatoAttivita.DataValueField = "id"
        ddlStatoAttivita.DataBind()
        If dgRisultatoRicerca.Items.Count > 0 Then
            CmdEsporta.Visible = True
        Else
            CmdEsporta.Visible = False
        End If
        'visualizzo valore predefinito
        If Request.QueryString("VengoDa") = "Valutare" Then
            dtrgenerico = ClsServer.CreaDatareader("select idstatoattività from statiattività where daValutare=1", IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
            Do While dtrgenerico.Read
                'posizione combo stato su item predefinito
                ddlStatoAttivita.SelectedValue = dtrgenerico.GetValue(0)
            Loop
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
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
        myDataTable.Columns.Add(myDataColumn)

        myDataColumn = New DataColumn
        myDataColumn.DataType = System.Type.GetType("System.String")
        myDataColumn.ColumnName = "ParentItem"
        myDataColumn.AutoIncrement = False
        myDataColumn.Caption = "ParentItem"
        myDataColumn.ReadOnly = False
        myDataColumn.Unique = False

        myDataTable.Columns.Add(myDataColumn)

        dtrgenerico = ClsServer.CreaDatareader(strquery, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
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
        ChiudiDataReader(dtrgenerico)

        MakeParentTable = New DataSet
        MakeParentTable.Tables.Add(myDataTable)
    End Function

    Private Sub ddlMaccCodAmAtt_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlMaccCodAmAtt.SelectedIndexChanged
        '***Generata da Gianluigi Paesani in data:05/07/04
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
            ddlCodAmAtt.DataSource = Nothing
            ddlCodAmAtt.Items.Add("")
            ddlCodAmAtt.SelectedIndex = 0
            ddlCodAmAtt.Enabled = False
        End If



    End Sub

    Public Sub cmdRicerca_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles cmdRicerca.Click
        CancellaMessaggiInfo()
        ApriCSV1.Visible = False
        txtTitoloProgetto1.Value = txtTitoloProgetto.Text
        txtbando1.Value = txtbando.SelectedItem.Text
        txtDenominazioneEnte1.Value = txtDenominazioneEnte.Text
        ddlMaccCodAmAtt1.Value = ddlMaccCodAmAtt.SelectedIndex
        ddlCodAmAtt1.Value = ddlCodAmAtt.SelectedIndex
        ddlStatoAttivita1.Value = ddlStatoAttivita.SelectedIndex
        dgRisultatoRicerca.CurrentPageIndex = 0
        EseguiRicerca(0)
    End Sub
    Private Sub EseguiRicerca(ByVal bytVerifica As Byte, Optional ByVal bytpage As Integer = 0)
        Dim Mydataset As New DataSet
        'controllo se la chiamata viene effettuata dal link di pagina o dal pulsante ricerca
        If bytVerifica = 1 Then dgRisultatoRicerca.CurrentPageIndex = bytpage
        If Request.QueryString("VengoDa") = "Valutare" Then
            strquery = "select  distinct b.CodiceRegione, b.denominazione, a.CodiceEnte,a.titolo, c.bando,c.bandobreve," & _
                       " g.codifica + '-' + g.macroambitoattività + ' / ' + f.codifica + '-' + f.ambitoattività as Ambito," & _
                       " convert(varchar,isnull(a.dataultimostato,''),3) as Data,a.idattività,b.idente, " & _
                       " convert(tinyint,isnull(e.defaultstato,0)) + convert(tinyint,isnull(e.defaultstato,0)) modificabile,a.idtipoprogetto, " & _
                       " IsNull(a.NumeroPostiNoVittoNoAlloggio,0) + IsNull(a.NumeroPostiVittoAlloggio,0) + IsNull(a.NumeroPostiVitto,0) As Effettivi, " & _
                       " IsNull(a.NumeroPostiNoVittoNoAlloggioRic,0) + IsNull(a.NumeroPostiVittoAlloggioRic,0) + IsNull(a.NumeroPostiVittoRic,0) As Richiesti,RegioniCompetenze.Descrizione as Competenza, " & _
                       " Case isnull(a.SegnalazioneSanzione,0) When 0 then 'No' When 1 then '<img src=images/Anomalie.bmp onclick=VisualizzaSanzioneProg('+ convert(varchar, a.IDAttività) + ','+ convert(varchar, a.IDEntePresentante) + ') STYLE=cursor:hand title=Sanzione border=0>' End as SegnalazioneSanzione" & _
                       " ,year(a.dataultimostato),month(a.dataultimostato),day(a.dataultimostato),k.titolo, k.CodiceProgramma " & _
                       " from attività a " & _
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

            strquery = strquery & " LEFT JOIN BANDORICORSI ON a.IDBANDORICORSO = BANDORICORSI.IDBANDORICORSO " & _
                       " inner join enti b on a.identepresentante=b.idente" & _
                       " inner join bandiAttività h on a.idbandoattività=h.idbandoattività" & _
                       " inner join  statibandiAttività on h.idstatobandoattività=statibandiAttività.idstatobandoattività " & _
                       " inner join bando c on c.idbando=h.idbando" & _
                       " inner join statibando d on c.idstatobando=d.idstatobando" & _
                       " inner join statiattività e on a.idstatoattività=e.idstatoattività" & _
                       " inner join ambitiattività f on f.idambitoattività=a.idambitoattività" & _
                       " inner join macroambitiattività g on f.IDMacroAmbitoAttività=g.IDMacroAmbitoAttività" & _
                       " Left Join RegioniCompetenze On a.IdRegioneCompetenza = RegioniCompetenze.IdRegioneCompetenza" & _
                       " Left Join Programmi k on a.idProgramma = k.idProgramma " & _
                       " where d.InValutazione = 1 and AssociaUtenteGruppo.username='" & Replace(Session("Utente"), "'", "''") & "'"
            If Session("TipoUtente") = "E" Then
                strquery = strquery & " and CASE ISNULL(BANDORICORSI.IDBANDORICORSO,0) WHEN 0 THEN isnull(c.enteabilitato,1) ELSE isnull(BANDORICORSI.enteabilitato,1) END = 1 "
            End If
        Else
            strquery = "select distinct b.CodiceRegione, b.denominazione, a.CodiceEnte,a.titolo, c.bando,c.bandobreve," & _
                       " g.codifica + '-' + g.macroambitoattività + ' / ' + f.codifica + '-' + f.ambitoattività as Ambito," & _
                       " convert(varchar,isnull(a.dataultimostato,''),3) as Data,a.idattività,b.idente, " & _
                        " IsNull(a.NumeroPostiNoVittoNoAlloggio,0) + IsNull(a.NumeroPostiVittoAlloggio,0) + IsNull(a.NumeroPostiVitto,0) As Effettivi, " & _
                       " IsNull(a.NumeroPostiNoVittoNoAlloggioRic,0) + IsNull(a.NumeroPostiVittoAlloggioRic,0) + IsNull(a.NumeroPostiVittoRic,0) As Richiesti,RegioniCompetenze.Descrizione as Competenza, " & _
                       " Case isnull(a.SegnalazioneSanzione,0) When 0 then 'No' When 1 then '<img src=images/Anomalie.bmp onclick=VisualizzaSanzioneProg('+ convert(varchar, a.IDAttività) + ','+ convert(varchar, a.IDEntePresentante) + ') STYLE=cursor:hand title=Sanzione border=0>' End as SegnalazioneSanzione" & _
                       " ,year(a.dataultimostato),month(a.dataultimostato),day(a.dataultimostato),k.titolo, k.CodiceProgramma " & _
                        " from attività a " & _
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

            strquery = strquery & " LEFT JOIN BANDORICORSI ON a.IDBANDORICORSO = BANDORICORSI.IDBANDORICORSO " & _
                       " inner join enti b on a.identepresentante=b.idente" & _
                       " left join bandiAttività h on a.idbandoattività=h.idbandoattività" & _
                       " left join bando c on c.idbando=h.idbando" & _
                       " inner join statiattività e on a.idstatoattività=e.idstatoattività" & _
                       " inner join ambitiattività f on f.idambitoattività=a.idambitoattività" & _
                       " inner join macroambitiattività g on f.IDMacroAmbitoAttività=g.IDMacroAmbitoAttività" & _
                       " Left Join RegioniCompetenze On a.IdRegioneCompetenza = RegioniCompetenze.IdRegioneCompetenza" & _
                       " Left Join Programmi k on a.idProgramma = k.idProgramma " & _
                       " where a.idattività is not null and AssociaUtenteGruppo.username='" & Replace(Session("Utente"), "'", "''") & "'"
            If Session("TipoUtente") = "E" Then
                strquery = strquery & " and CASE ISNULL(BANDORICORSI.IDBANDORICORSO,0) WHEN 0 THEN isnull(c.enteabilitato,1) ELSE isnull(BANDORICORSI.enteabilitato,1) END = 1 "
            End If
        End If

        If ddlStatoAttivita.SelectedItem.Text <> "" Then
            strquery = strquery & " and e.idstatoattività=" & ddlStatoAttivita.SelectedValue & ""
            If ddlStatoAttivita.SelectedItem.Text = "Proposto" Then
                strquery = strquery & " and d.invalutazione=1 and statibandiAttività.Attivo=1 "
            End If
        End If
        If txtTitoloProgetto.Text <> "" Then
            strquery = strquery & " and a.titolo like '" & ClsServer.NoApice(Trim(txtTitoloProgetto.Text)) & "%'"
        End If
        If txtCodiceProgetto.Text <> String.Empty Then
            strquery = strquery & " and a.CodiceEnte like '" & ClsServer.NoApice(Trim(txtCodiceProgetto.Text)) & "%'"
        End If
        If txtTitoloProgramma.Text <> "" Then
            strquery = strquery & " and k.titolo like '" & ClsServer.NoApice(txtTitoloProgramma.Text) & "%'"
        End If
        If txtCodiceProgramma.Text <> "" Then
            strquery = strquery & " and k.CodiceProgramma like '" & ClsServer.NoApice(txtCodiceProgramma.Text) & "%'"
        End If
        If Trim(txtbando.SelectedItem.Text) <> "TUTTI" Then
            strquery = strquery & " and bandobreve = '" & Replace(txtbando.SelectedItem.Text, "'", "''") & "'"
        End If

        If txtDenominazioneEnte.Text <> "" Then
            strquery = strquery & " and b.Denominazione like '" & ClsServer.NoApice(Trim(txtDenominazioneEnte.Text)) & "%'"
        End If
        If txtCodEnte.Text <> "" Then
            strquery = strquery & " and b.CodiceRegione = '" & ClsServer.NoApice(Trim(txtCodEnte.Text)) & "'"
        End If
        If ddlMaccCodAmAtt.SelectedItem.Text = "" And ddlCodAmAtt.SelectedItem.Text = "" Then
        Else
            If ddlCodAmAtt.SelectedItem.Text <> "" Then
                strquery = strquery & " and f.idambitoattività=" & ddlCodAmAtt.SelectedValue & ""
            Else
                strquery = strquery & " and f.idmacroambitoattività=" & ddlMaccCodAmAtt.SelectedValue & ""
            End If
        End If
        ' Filtro per regioni
        If CboCompetenza.SelectedValue <> "" Then
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
        End If
        If ddlSegnalazioneSanzione.SelectedItem.Text <> "Tutti" Then
            strquery = strquery & " and isnull(a.SegnalazioneSanzione,0) =" & ddlSegnalazioneSanzione.SelectedValue
        End If
        If ddlStatoValutazione.SelectedItem.Text <> "Tutti" Then
            strquery = strquery & " and isnull(a.StatoValutazione,0) =" & ddlStatoValutazione.SelectedValue
        End If
        strquery = strquery & " AND TipiProgetto.MacroTipoProgetto like '" & Session("FiltroVisibilita") & "'"
        strquery = strquery & " order by year(a.dataultimostato),month(a.dataultimostato),day(a.dataultimostato),1,2"

        Mydataset = ClsServer.DataSetGenerico(strquery, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))

        dgRisultatoRicerca.Columns(16).Visible = ClsUtility.ForzaPresenzaSanzioneProgetto(Session("Utente"), Session("conn"))

        dgRisultatoRicerca.DataSource = Mydataset
        CaricaDataTablePerStampa(Mydataset)
        dgRisultatoRicerca.DataBind() 'valorizzo griglia
        ChiudiDataReader(dtrGenerico)
        If dgRisultatoRicerca.Items.Count = 0 Then 'se la griglia e vuota la nascondo
            dgRisultatoRicerca.Caption = "La ricerca non ha prodotto alcun risultato."
            CmdEsporta.Visible = False
        Else
            CmdEsporta.Visible = True
            dgRisultatoRicerca.Caption = "Risultato Ricerca Progetti"
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
        NomeColonne(0) = "Denominazione"
        NomeColonne(1) = "Titolo"
        NomeColonne(2) = "Cod. Ente"
        NomeColonne(3) = "Bando"
        NomeColonne(4) = "Settore / Area Intervento"
        NomeColonne(5) = "Data presentazione"
        NomeColonne(6) = "Posti Effettivi"
        NomeColonne(7) = "Posti Richiesti"
        NomeColonne(8) = "Competenza"

        NomiCampiColonne(0) = "denominazione"
        NomiCampiColonne(1) = "titolo"
        NomiCampiColonne(2) = "CodiceRegione"
        NomiCampiColonne(3) = "bando"
        NomiCampiColonne(4) = "ambito"
        NomiCampiColonne(5) = "data"
        NomiCampiColonne(6) = "Effettivi"
        NomiCampiColonne(7) = "Richiesti"
        NomiCampiColonne(8) = "Competenza"

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

        txtTitoloProgetto.Text = txtTitoloProgetto1.Value
        txtbando.SelectedItem.Text = txtbando1.Value
        txtDenominazioneEnte.Text = txtDenominazioneEnte1.Value
        ddlMaccCodAmAtt.SelectedIndex = CInt(ddlMaccCodAmAtt1.Value)
        ddlStatoAttivita.SelectedIndex = CInt(ddlStatoAttivita1.Value)
        If ddlMaccCodAmAtt1.Value = "0" Then
            ddlCodAmAtt.DataSource = Nothing
            ddlCodAmAtt.Enabled = False
            ddlCodAmAtt.Items.Add("")
            ddlCodAmAtt.SelectedIndex = 0

        Else
            ddlCodAmAtt.SelectedIndex = CInt(ddlCodAmAtt1.Value)
            ddlCodAmAtt.Enabled = True
        End If
        'passo valore pagina
        EseguiRicerca(1, e.NewPageIndex)
    End Sub

    Private Sub dgRisultatoRicerca_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgRisultatoRicerca.ItemCommand

        Select Case e.CommandName
            Case "Select"
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
                " where attività.idattività=" & CInt(e.Item.Cells(9).Text) & ""
                 ChiudiDataReader(dtrGenerico)
                dtrGenerico = ClsServer.CreaDatareader(strquery, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
                dtrGenerico.Read()
                If dtrGenerico("invalutazione") = 1 And dtrGenerico("attivoStatoAttBando") = 1 Then
                    ChiudiDataReader(dtrGenerico)
                    If (Session("TipoUtente") = "U" Or Session("TipoUtente") = "R") Then
                        Session("IdEnte") = e.Item.Cells(10).Text
                        Session("Denominazione") = e.Item.Cells(3).Text
                    End If
                    Response.Redirect("assegnazionevincoliprogetti.aspx?idattivita=" & CInt(e.Item.Cells(9).Text) & "&tipologia=ProgettiValutare")
                Else
                    'Messaggio
                    msgErrore.Text = "Non è possibile effettuare l'Accettazione del Progetto."
                    ChiudiDataReader(dtrGenerico)
                    Exit Sub
                End If
        End Select
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

End Class