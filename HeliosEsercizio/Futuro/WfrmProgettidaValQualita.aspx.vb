Imports System.Data.SqlClient
Imports System.IO

Public Class WfrmProgettidaValQualita
    Inherits System.Web.UI.Page

    Dim dtrgenerico As Data.SqlClient.SqlDataReader
    Const INDEX_DGRISULTATORICERCA_SELEZIONA As Int16 = 0
    Const INDEX_DGRISULTATORICERCA_DENOMINAZIONE As Int16 = 1
    Const INDEX_DGRISULTATORICERCA_CODICE_PROGETTO As Int16 = 2
    Const INDEX_DGRISULTATORICERCA_TITOLO As Int16 = 3
    Const INDEX_DGRISULTATORICERCA_CODICE_ENTE As Int16 = 4
    Const INDEX_DGRISULTATORICERCA_ID_PROGETTO As Int16 = 8
    Const INDEX_DGRISULTATORICERCA_ID_ENTE As Int16 = 9
    Const INDEX_DGRISULTATORICERCA_STATO_ATTIVITA As Int16 = 14
    Const INDEX_DGRISULTATORICERCA_DENOMINAZIONE_NASCOSTA As Int16 = 18

    Dim statiValidi As List(Of String) = New List(Of String)(New String() {"In Attesa Graduatoria", "Non Attivabile", "Attivo"})

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
    Private Sub NascondiMenuLaterale()
        Session("TP") = True
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
            'richiamo sub dove popolo combo
            CaricaCompetenze()
            CaricaPrima()
            CaricaBando()
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
            If Session("CodiceRegioneEnte") <> "" Then
                txtCodEnte.Text = Session("txtCodEnte")
            End If
            NascondiMenuLaterale()
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
    Private Sub cmdChiudi_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles cmdChiudi.Click
        Response.Redirect("WfrmMain.aspx")
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

            Else
                'CboCompetenza.SelectedIndex = 1
                CboCompetenza.Enabled = False
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

    Private Sub CaricaPrima()
        '***Generata da Gianluigi Paesani in data:05/07/04
        '***Carico combo settore
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

        'visualizzo valore predefinito
        If Request.QueryString("VengoDa") = "Valutare" Then
            If Not dtrgenerico Is Nothing Then
                dtrgenerico.Close()
                dtrgenerico = Nothing
            End If
            dtrgenerico = ClsServer.CreaDatareader("select idstatoattività from statiattività where dagraduare=1", Session("conn"))
            Do While dtrgenerico.Read
                'posizione combo stato su item predefinito
                ddlStatoAttivita.SelectedValue = dtrgenerico.GetValue(0)
            Loop
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
        ddlTipiProgetto.DataSource = ClsServer.CreaDataTable("Select idTipoProgetto,Descrizione from TipiProgetto WHERE MacroTipoProgetto like '" & Session("FiltroVisibilita") & "'", True, Session("conn"))
        ddlTipiProgetto.DataTextField = "Descrizione"
        ddlTipiProgetto.DataValueField = "idTipoProgetto"
        ddlTipiProgetto.DataBind()
    End Sub
    Private Function MakeParentTable(ByVal strquery As String) As DataSet
        '***Generata da Gianluigi Paesani in data:05/07/04
        '***Inizializzo e carico datatable 
        ChiudiDataReader(dtrgenerico)
        Dim myDataTable As DataTable = New DataTable

        Dim myDataColumn As DataColumn
        Dim myDataRow As DataRow  
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

    Private Sub cmdRicerca_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles cmdRicerca.Click
        '***Generata da Gianluigi Paesani in data:05/07/04
        '*** 'salvo parametri per controllo link pagine
        CancellaMessaggiInfo()
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
        Dim strquery As String
        'controllo se la chiamata viene effettuata dal link di pagina o dal pulsante ricerca
        If bytVerifica = 1 Then dgRisultatoRicerca.CurrentPageIndex = bytpage
        If Request.QueryString("VengoDa") = "Valutare" Then 'unsc
            strquery = "select distinct b.CodiceRegione, b.denominazione, a.CodiceEnte,a.titolo, c.bando,c.bandobreve," & _
            " g.macroambitoattività + ' / ' + f.ambitoattività as Ambito,a.punteggioFinale as punteggio," & _
            " case e.dagraduare when 1 then isnull(convert(varchar,a.dataultimostato,3),'') else '' end  Data," & _
            " a.idattività , b.idente,  " & _
            " case a.ConfermaValutazione when 1 then 'si'else 'no' end davalutare, " & _
            " IsNull(a.NumeroPostiNoVittoNoAlloggio,0) + IsNull(a.NumeroPostiVittoAlloggio,0) + IsNull(a.NumeroPostiVitto,0) As Effettivi, " & _
            " IsNull(a.NumeroPostiNoVittoNoAlloggioRic,0) + IsNull(a.NumeroPostiVittoAlloggioRic,0) + IsNull(a.NumeroPostiVittoRic,0) As Richiesti, e.statoattività,RegioniCompetenze.Descrizione as Competenza, " & _
            " Case isnull(a.SegnalazioneSanzione,0) When 0 then 'No' When 1 then '<img src=images/Anomalie.bmp onclick=VisualizzaSanzioneProg('+ convert(varchar, a.IDAttività) + ','+ convert(varchar, a.IDEntePresentante) + ') STYLE=cursor:hand title=Sanzione border=0>' End as SegnalazioneSanzione, " & _
            " case a.Limitazioni when 1 then 'Si'else 'No' end Limitazioni " & _
            " ,year(a.dataultimostato),month(a.dataultimostato),day(a.dataultimostato),k.titolo, k.CodiceProgramma " & _
            " from attività a" & _
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
            " inner join bando c on c.idbando=h.idbando" & _
            " inner join statibando d on c.idstatobando=d.idstatobando" & _
            " inner join statiattività e on a.idstatoattività=e.idstatoattività" & _
            " inner join ambitiattività f on f.idambitoattività=a.idambitoattività" & _
            " inner join macroambitiattività g on f.IDMacroAmbitoAttività=g.IDMacroAmbitoAttività" & _
            " Left Join RegioniCompetenze On a.IdRegioneCompetenza = RegioniCompetenze.IdRegioneCompetenza" & _
            " LEFT JOIN Programmi k on a.idProgramma = k.idProgramma " & _
            " where d.InValutazione = 1 and AssociaUtenteGruppo.username='" & Replace(Session("Utente"), "'", "''") & "'"
            If Session("TipoUtente") = "E" Then
                strquery = strquery & " and CASE ISNULL(BANDORICORSI.IDBANDORICORSO,0) WHEN 0 THEN isnull(c.enteabilitato,1) ELSE isnull(BANDORICORSI.enteabilitato,1) END = 1 "
            End If
            'Else 'ente
            '    strquery = "select distinct b.CodiceRegione, b.denominazione,a.CodiceEnte, a.titolo, c.bando,c.bandobreve," & _
            '    " g.macroambitoattività + ' / ' + f.ambitoattività as Ambito,a.punteggioFinale as punteggio,case a.ConfermaValutazione when 1 then 'si'else 'no' end davalutare, " & _
            '    " case e.dagraduare when 1 then isnull(convert(varchar,a.dataultimostato,3),'') else '' end  Data," & _
            '    " a.idattività,b.idente, " & _
            '    " IsNull(a.NumeroPostiNoVittoNoAlloggio,0) + IsNull(a.NumeroPostiVittoAlloggio,0) + IsNull(a.NumeroPostiVitto,0) As Effettivi, " & _
            '    " IsNull(a.NumeroPostiNoVittoNoAlloggioRic,0) + IsNull(a.NumeroPostiVittoAlloggioRic,0) + IsNull(a.NumeroPostiVittoRic,0) As Richiesti,RegioniCompetenze.Descrizione as Competenza, " & _
            '    " case a.Limitazioni when 1 then 'Si'else 'No' end Limitazioni " & _
            '    " ,year(dataultimostato),month(dataultimostato),day(dataultimostato) " & _
            '    " from attività a" & _
            '    " INNER JOIN TipiProgetto ON a.IdTipoProgetto = TipiProgetto.IdTipoProgetto " & _
            '    " INNER JOIN AssociaProfiliTipiProgetto ON TipiProgetto.IdTipoProgetto = AssociaProfiliTipiProgetto.IdTipoProgetto " & _
            '    " INNER JOIN Profili ON AssociaProfiliTipiProgetto.IdProfilo = Profili.IdProfilo "

            '    '============================================================================================================================
            '    '====================================================30/09/2008==============================================================
            '    '=======MODIFICA EFFETTUATA DA Kronk (99 scimmie saltavano sul letto, una cadde in terra e si ruppe il cervelletto...)=======
            '    '=================AGGIUNTA JOIN SU PROFILO READ PER ABILITARE LEGGERE LE ABILITAZIONI ANCHE DELL'UTENTE READ=================
            '    '============================================================================================================================
            '    If UCase(Me.TemplateSourceDirectory) <> "/HELIOSREAD" Then
            '        strquery = strquery & " INNER JOIN	AssociaUtenteGruppo ON Profili.IdProfilo = AssociaUtenteGruppo.IdProfilo "
            '    Else
            '        strquery = strquery & " INNER JOIN	AssociaUtenteGruppo ON Profili.IdProfilo = AssociaUtenteGruppo.IdProfiloRead "
            '    End If

            '    strquery = strquery & " LEFT JOIN BANDORICORSI ON a.IDBANDORICORSO = BANDORICORSI.IDBANDORICORSO " & _
            '    " inner join enti b on a.identepresentante=b.idente" & _
            '    " left join bandiAttività h on a.idbandoattività=h.idbandoattività" & _
            '    " left join bando c on c.idbando=h.idbando" & _
            '    " inner join statiattività e on a.idstatoattività=e.idstatoattività" & _
            '    " inner join ambitiattività f on f.idambitoattività=a.idambitoattività" & _
            '    " inner join macroambitiattività g on f.IDMacroAmbitoAttività=g.IDMacroAmbitoAttività" & _
            '    " Left Join RegioniCompetenze On a.IdRegioneCompetenza = RegioniCompetenze.IdRegioneCompetenza" & _
            '    " where a.idattività is not null and AssociaUtenteGruppo.username='" & Replace(Session("Utente"), "'", "''") & "'"
            '    If Session("TipoUtente") = "E" Then
            '        strquery = strquery & " and CASE ISNULL(BANDORICORSI.IDBANDORICORSO,0) WHEN 0 THEN isnull(c.enteabilitato,1) ELSE isnull(BANDORICORSI.enteabilitato,1) END = 1 "
            '    End If
        End If

        'imposto eventuali parametri

        If ddlStatoAttivita.SelectedItem.Text <> "" Then
            strquery = strquery & " and e.idstatoattività=" & ddlStatoAttivita.SelectedValue & ""
        End If

        If txtTitoloProgetto.Text <> "" Then
            strquery = strquery & " and a.titolo like '" & ClsServer.NoApice(txtTitoloProgetto.Text) & "%'"
        End If

        If txtCodEnte.Text <> "" Then
            strquery = strquery & " and b.CodiceRegione='" & ClsServer.NoApice(txtCodEnte.Text) & "'"
        End If

        If Trim(txtbando.SelectedItem.Text) <> "" Then
            strquery = strquery & " and bandobreve = '" & Replace(txtbando.SelectedItem.Text, "'", "''") & "'"
        End If
        '---------------------------------ANTONELLO------------------------------------------------------------
        If txtTitoloProgramma.Text <> "" Then
            strquery = strquery & " and k.titolo like '" & ClsServer.NoApice(txtTitoloProgramma.Text) & "%'"
        End If

        If txtCodiceProgramma.Text <> "" Then
            strquery = strquery & " and k.CodiceProgramma like '" & ClsServer.NoApice(txtCodiceProgramma.Text) & "%'"
        End If
        '-----------------------------------------------------------------------------------------------

        If ddlTipiProgetto.SelectedItem.Text <> "" Then
            strquery = strquery & " and a.idTipoProgetto=" & ddlTipiProgetto.SelectedValue & ""
        End If
        If txtDenominazioneEnte.Text <> "" Then
            strquery = strquery & " and b.Denominazione like '" & ClsServer.NoApice(Trim(txtDenominazioneEnte.Text)) & "%'"
        End If
        If txtCodiceProgetto.Text <> String.Empty Then
            strquery = strquery & " and a.CodiceEnte LIKE '" & ClsServer.NoApice(Trim(txtCodiceProgetto.Text)) & "%' "
        End If

        If ddlMaccCodAmAtt.SelectedItem.Text = "" And ddlCodAmAtt.SelectedItem.Text = "" Then

        Else
            If ddlCodAmAtt.SelectedItem.Text <> "" Then
                strquery = strquery & " and f.idambitoattività=" & ddlCodAmAtt.SelectedValue & ""
            Else
                strquery = strquery & " and f.idmacroambitoattività=" & ddlMaccCodAmAtt.SelectedValue & ""
            End If

        End If
        If ddlFiltro.SelectedValue = 1 Then
            strquery = strquery & " and (a.ConfermaValutazione=0 or a.ConfermaValutazione is null) "
        End If
        If ddlFiltro.SelectedValue = 2 Then
            strquery = strquery & " and a.ConfermaValutazione=1 "
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
        'Aggiunto il 15/12/2011 SANDOKAN
        If ddlSegnalazioneSanzione.SelectedItem.Text <> "Tutti" Then
            strquery = strquery & " and isnull(a.SegnalazioneSanzione,0) =" & ddlSegnalazioneSanzione.SelectedValue
        End If
        'Aggiunto il 05/05/2014 da SIMONA CORDELLA
        If ddlStatoValutazione.SelectedItem.Text <> "Tutti" Then
            strquery = strquery & " and isnull(a.StatoValutazione,0) =" & ddlStatoValutazione.SelectedValue
        End If
        'filtrovisibilità 04/12/2014
        strquery = strquery & " AND TipiProgetto.MacroTipoProgetto like '" & Session("FiltroVisibilita") & "'"
        strquery = strquery & " order by year(a.dataultimostato),month(a.dataultimostato),day(a.dataultimostato),1,2"
        Mydataset = ClsServer.DataSetGenerico(strquery, Session("conn"))
        dgRisultatoRicerca.Columns(16).Visible = ClsUtility.ForzaPresenzaSanzioneProgetto(Session("Utente"), Session("conn"))
        dgRisultatoRicerca.DataSource = Mydataset
        CaricaDataTablePerStampa(Mydataset)
        dgRisultatoRicerca.DataBind() 'valorizzo griglia

        If dgRisultatoRicerca.Items.Count = 0 Then 'se la griglia e vuota la nascondo
            dgRisultatoRicerca.Caption = "La ricerca non ha prodotto alcun risultato."
            CmdEsporta.Visible = False
        Else
            dgRisultatoRicerca.Caption = "Risultato Ricerca Progetti"
            CmdEsporta.Visible = True
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
        strsql = strsql & " SELECT  '0','', 99  from bando "
        strsql = strsql & " ORDER BY Bando.annobreve desc"
        ChiudiDataReader(dtrgenerico)
        dtrgenerico = ClsServer.CreaDatareader(strsql, Session("conn"))
        txtbando.DataSource = dtrgenerico
        txtbando.DataTextField = "bandobreve"
        txtbando.DataValueField = "IdBando"
        txtbando.DataBind()
        ChiudiDataReader(dtrgenerico)
    End Sub
    'routine che carica la datatable che caricherà dinamicamente la datagrid della stampa delle ricerche
    Sub CaricaDataTablePerStampa(ByVal DataSetDaScorrere As DataSet)
        Dim dt As New DataTable
        Dim dr As DataRow
        Dim i As Integer
        Dim x As Integer

        Dim NomeColonne(12) As String
        Dim NomiCampiColonne(12) As String

        'nome della colonna 
        'e posizione nella griglia di lettura
        NomeColonne(0) = "Denominazione"
        NomeColonne(1) = "Codice Progetto"
        NomeColonne(2) = "Titolo"
        NomeColonne(3) = "Cod. Ente"
        NomeColonne(4) = "Bando"
        NomeColonne(5) = "Settore / Area Intervento"
        NomeColonne(6) = "Data presentazione"
        NomeColonne(7) = "Valutazioni Confermate"
        NomeColonne(8) = "Punteggio"
        NomeColonne(9) = "Posti Effettivi"
        NomeColonne(10) = "Posti Richiesti"
        NomeColonne(11) = "Competenza"
        NomeColonne(12) = "Limitazioni"

        NomiCampiColonne(0) = "denominazione"
        NomiCampiColonne(1) = "codiceente"
        NomiCampiColonne(2) = "titolo"
        NomiCampiColonne(3) = "CodiceRegione"
        NomiCampiColonne(4) = "bando"
        NomiCampiColonne(5) = "ambito"
        NomiCampiColonne(6) = "data"
        NomiCampiColonne(7) = "davalutare"
        NomiCampiColonne(8) = "punteggio"
        NomiCampiColonne(9) = "Effettivi"
        NomiCampiColonne(10) = "Richiesti"
        NomiCampiColonne(11) = "Competenza"
        NomiCampiColonne(12) = "Limitazioni"
        'carico i nomi delle colonne che andrò a stampare nella datagrid
        For x = 0 To 12
            dt.Columns.Add(New DataColumn(NomeColonne(x), GetType(String)))
        Next

        'carico il datatable con il risultato della query della ricerca, in qusto caso delle risorse
        If DataSetDaScorrere.Tables(0).Rows.Count > 0 Then
            For i = 1 To DataSetDaScorrere.Tables(0).Rows.Count
                dr = dt.NewRow()
                For x = 0 To 12
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
                If (Session("TipoUtente") = "U" Or Session("TipoUtente") = "R") Then
                    Session("IdEnte") = e.Item.Cells(INDEX_DGRISULTATORICERCA_ID_ENTE).Text
                    Session("Denominazione") = e.Item.Cells(INDEX_DGRISULTATORICERCA_DENOMINAZIONE_NASCOSTA).Text
                End If
                Response.Redirect(ClsUtility.RitornaMascheraValutazioneProgetto(e.Item.Cells(INDEX_DGRISULTATORICERCA_ID_PROGETTO).Text, Session("conn")) & "?idprogetto=" & e.Item.Cells(INDEX_DGRISULTATORICERCA_ID_PROGETTO).Text)
        End Select
    End Sub

    Private Sub dgRisultatoRicerca_ItemDataBound(ByVal source As Object, ByVal e As DataGridItemEventArgs) Handles dgRisultatoRicerca.ItemDataBound

        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then

            Dim titolo As String = e.Item.Cells(INDEX_DGRISULTATORICERCA_TITOLO).Text
            Dim denominazione As String = e.Item.Cells(INDEX_DGRISULTATORICERCA_DENOMINAZIONE).Text
            Dim codiceProgetto As String = e.Item.Cells(INDEX_DGRISULTATORICERCA_CODICE_PROGETTO).Text
            Dim codiceEnte As String = e.Item.Cells(INDEX_DGRISULTATORICERCA_CODICE_ENTE).Text
            Dim statoAttivita As String = e.Item.Cells(INDEX_DGRISULTATORICERCA_STATO_ATTIVITA).Text
            If Not statiValidi.Contains(statoAttivita) Then
                Dim image As ImageButton = DirectCast(e.Item.Cells(INDEX_DGRISULTATORICERCA_SELEZIONA).FindControl("SelPrgetti"), ImageButton)
                image.Enabled = False
                image.CssClass = "datagridImageDisabled"
                image.ToolTip = "Progetto non selezionabile"
            End If
            titolo = titolo & " (" & codiceProgetto & ") "
            denominazione = denominazione & " (" & codiceEnte & ") "
            e.Item.Cells(INDEX_DGRISULTATORICERCA_DENOMINAZIONE).Text = denominazione
            e.Item.Cells(INDEX_DGRISULTATORICERCA_TITOLO).Text = titolo

        End If
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