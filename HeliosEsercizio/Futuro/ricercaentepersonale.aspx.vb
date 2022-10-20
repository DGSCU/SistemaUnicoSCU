Imports System.Drawing
Imports System.IO

Public Class ricercaentepersonale
    Inherits System.Web.UI.Page

    Dim dtsRisRicerca As DataSet
#Region "Utility"
    Private Sub ChiudiDataReader(ByRef dataReader As SqlClient.SqlDataReader)
        If Not dataReader Is Nothing Then
            dataReader.Close()
            dataReader = Nothing
        End If
    End Sub
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'controllo login effettuato
        If Not Session("LogIn") Is Nothing Then
            If Session("LogIn") = False Then
                Response.Redirect("LogOn.aspx")
            End If
        Else
            Response.Redirect("LogOn.aspx")
        End If
        'se non sono state fatte selezioni esco dalla routine
        If Not dtgRisultatoRicerca.SelectedItem Is Nothing Then
            Exit Sub
        End If
        'controllo se si tratta di primo caricamento della pagina
        If Page.IsPostBack = False Then

            'visibilità filtroFase
            If Session("TipoUtente") = "U" Then
                divFiltroFase.Visible = True
            Else
                divFiltroFase.Visible = False
            End If

            'controllo se devo cancellare un servizio
            If Not Request.QueryString("strIDEnteAcquisizione") Is Nothing Then
                If Request.QueryString("AnnullaServizio") = "NO" Then
                    CancellaServizio(Request.QueryString("strIDEnteAcquisizione"))
                Else
                    AnnullaCancellaServizio(Request.QueryString("strIDEnteAcquisizione"))
                End If
            End If
            'scarico la session della datatable per la ricerca così che in una nuova pagina non verrà 
            'erroneamente visualizzato alcun item
            Session("DtbRicerca") = Nothing
            'se si carico i ruoli
            CaricaRuoli()
            'carico la griglia con le risorse dell'ente loggato
            'CaricaGriglia(CInt(Session("idEnte")))
            'Else
            'dtgRisultatoRicerca.CurrentPageIndex = 0
            'se si tratta di un postback carico la griglia delle risorse con i criteri di ricerca
            RicercaPersonaliEnte(Session("idEnte"))
            RicercaServizi()
        End If
        'If UCase(Request.QueryString("esporta")) = "SI" Then
        '    dtgRisultatoRicerca.Columns(0).Visible = False
        '    If dtgRisultatoRicerca.Items.Count > 0 Then
        '        'imgEsporta.Visible = True
        '        CmdEsportaPersonale.Visible = False
        '    Else
        '        'imgEsporta.Visible = False
        '        CmdEsportaPersonale.Visible = False
        '    End If
        'Else
        '    If dtgRisultatoRicerca.Items.Count > 0 Then
        '        CmdEsportaPersonale.Visible = True
        '        'imgEsporta.Visible = False
        '    Else
        '        'imgEsporta.Visible = False
        '        CmdEsportaPersonale.Visible = False
        '    End If
        'End If

    End Sub

    'routine che uso per cancellare il servizio
    Sub CancellaServizio(ByVal strIDEnteAcquisizione As String)
        AccreditamentoEliminaServizi(CInt(strIDEnteAcquisizione))
        ''stringa per la delete
        'Dim strsql As String

        'myCommand = New System.Data.SqlClient.SqlCommand
        'myCommand.Connection = IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn"))
        'Try
        '    strsql = "update EntiAcquisizioneServizi set StatoRichiesta=2, DataAnnullamento=GetDate(), UserNameAnnullamento='" & Session("Utente") & "' where IdEnteAcquisizione='" & strIDEnteAcquisizione & "'"
        '    myCommand.CommandText = strsql
        '    myCommand.ExecuteNonQuery()
        'Catch ex As Exception
        '    Response.Write(ex.Message)
        'End Try
    End Sub

    'routine che uso per cancellare il servizio
    Sub AnnullaCancellaServizio(ByVal strIDEnteAcquisizione As String)
        'Creata da: Simona Cordella
        'Data Crezione: 26/08/2014
        'Descrizione; Richiamo store per ripristinare il servizio acquisito cancellato
        AccreditamentoAnnullaEliminaServizi(CInt(strIDEnteAcquisizione))
    End Sub

    'carico la combo dei ruoli
    Sub CaricaRuoli()
        'stringa per la query
        Dim strSQL As String
        'datareader che conterrà l'id del tizio inserito
        Dim dtrRuoloPrincipale As System.Data.SqlClient.SqlDataReader
        Try
            'controllo se si tratta del primo caricamento. così leggo i dati nel db una sola volta
            If Page.IsPostBack = False Then
                'preparo la query
                'lafaccio union con '0' e 'Selezionare' perchè così aggiungo una riga vuota
                strSQL = "select IDRuolo,Ruolo from ruoli WHERE Nascosto = 0"
                If Session("TipoUtente") = "E" Then
                    strSQL = strSQL & " AND RuoloAccreditamento=1 "
                End If
                strSQL = strSQL & "union "
                strSQL = strSQL & "select '0',' Selezionare ' from ruoli order by ruolo"
                'chiudo il datareader se aperto
                ChiudiDataReader(dtrRuoloPrincipale)

                'eseguo la query
                dtrRuoloPrincipale = ClsServer.CreaDatareader(strSQL, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
                'assegno il datadearder alla combo caricando così descrizione e id
                ddlRuolo.DataSource = dtrRuoloPrincipale
                ddlRuolo.Items.Add("")
                ddlRuolo.DataTextField = "Ruolo"
                ddlRuolo.DataValueField = "IDRuolo"
                ddlRuolo.DataBind()

                'chiudo il datareader se aperto
                ChiudiDataReader(dtrRuoloPrincipale)
 
            End If
        Catch ex As Exception
            Response.Write(ex.Message.ToString())
        End Try

    End Sub

    Function checkFiltroFase() As Boolean
        lblErroreFiltroFase.Visible = False
        If Not String.IsNullOrEmpty(txtFiltroFase.Text) AndAlso Not Integer.TryParse(txtFiltroFase.Text, Nothing) Then
            lblErroreFiltroFase.Visible = True
            Return False
        End If
        Return True
    End Function

    Sub RicercaPersonaliEnte(ByVal IDEnte As Integer)
        'variabile stringa per la insert nella base dati
        Dim strSQL As String
        'variabile stringa per ctreazione filtri di ricerca da aggiungere nella query
        Dim strSQLFiltriRicerca As String
        'variabile stringa per ctreazione filtri di ricerca per i ruoli da aggiungere nella query
        Dim strSQLFiltriRicercaRuoli As String
        'adapter che conterrà l'id del tizio inserito
        strSQLFiltriRicerca = ""
        strSQLFiltriRicercaRuoli = ""

        If Not checkFiltroFase() Then Exit Sub

        If Session("TipoUtente") = "E" Then
            dtgRisultatoRicerca.Columns(13).Visible = False
        End If

        Try
            'controllo i filtri di ricerca da inserire nella query
            '******************************************************************
            If txtCognome.Text <> "" Then
                strSQLFiltriRicerca = strSQLFiltriRicerca & "and (EntePersonale.Cognome like '" & Replace(txtCognome.Text, "'", "''") & "%') "
            End If
            If txtCodFis.Text <> "" Then
                strSQLFiltriRicerca = strSQLFiltriRicerca & "and (EntePersonale.Codicefiscale = '" & Replace(txtCodFis.Text, "'", "''") & "') "
            End If

            If txtNome.Text <> "" Then
                strSQLFiltriRicerca = strSQLFiltriRicerca & "and (EntePersonale.Nome like '" & Replace(txtNome.Text, "'", "''") & "%') "
            End If
            If txtPosizione.Text <> "" Then
                strSQLFiltriRicerca = strSQLFiltriRicerca & "and (EntePersonale.Posizione like '" & Replace(txtPosizione.Text, "'", "''") & "%') "
            End If

            If txtTitolo.Text <> "" Then
                strSQLFiltriRicerca = strSQLFiltriRicerca & "and (EntePersonale.Titolo like '" & Replace(txtTitolo.Text, "'", "''") & "%') "
            End If

            If ddlRuolo.SelectedValue <> "0" Then
                strSQLFiltriRicercaRuoli = strSQLFiltriRicerca & "and (b.IDRuolo=" & ddlRuolo.SelectedValue & ")"
                strSQLFiltriRicerca = strSQLFiltriRicerca & "and (b.IDRuolo=" & ddlRuolo.SelectedValue & ")"
            End If

            If ddlStato.SelectedValue = 1 Then
                'strSQLFiltriRicercaRuoli = strSQLFiltriRicerca & "and (EntePersonale.datafinevalidità is null)"
                strSQLFiltriRicerca = strSQLFiltriRicerca & "and (EntePersonale.datafinevalidità is null)"
            End If

            If ddlStato.SelectedValue = 2 Then
                'strSQLFiltriRicercaRuoli = strSQLFiltriRicerca & "and (not EntePersonale.datafinevalidità is null)"
                strSQLFiltriRicerca = strSQLFiltriRicerca & "and (not EntePersonale.datafinevalidità is null)"
            End If
            Select Case ddlStatoAccr.SelectedValue
                Case -1
                    strSQLFiltriRicerca = strSQLFiltriRicerca & "and b.Accreditato='-1'"
                Case 0
                    strSQLFiltriRicerca = strSQLFiltriRicerca & "and b.Accreditato='0'"
                Case 1
                    strSQLFiltriRicerca = strSQLFiltriRicerca & "and b.Accreditato='1'"
            End Select

            'la validità del FiltroFase è stata controllata prima
            If Not String.IsNullOrEmpty(txtFiltroFase.Text) Then
                strSQLFiltriRicerca = strSQLFiltriRicerca & " and (efp.IdEnteFase =" & Trim(txtFiltroFase.Text) & "or efr.IdEnteFase =" & Trim(txtFiltroFase.Text) & ")"
            End If

            'blocco filtri di ricerca
            '******************************************************************
            'preparo la query per la ricerca delle risorse acquisite e non con l'aggiunta dei filtri
            strSQL = "select 0 ordina, d.Denominazione, EntePersonale.IDEntePersonale, EntePersonale.IDEnte as IDEnte, "
            strSQL = strSQL & "EntePersonale.Cognome + ' ' + EntePersonale.Nome as Nominativo,  EntePersonale.Titolo, "
            strSQL = strSQL & "EntePersonale.Posizione, EntePersonale.DataFineValidità, case when EntePersonale.datafinevalidità is null then 'Attiva'	when not EntePersonale.datafinevalidità is null then 'Cancellata' end as Stato, "
            strSQL = strSQL & "ruoli.Ruolo, "
            strSQL = strSQL & "ruoli.IdRuolo as IdRuolo, " 'Aggiunta Informazione su Id ruolo per controllo Rappresentente legale
            'strSQL = strSQL & "(SELECT count(*) from entepersonaleruoli where IDEntePersonale=b.IDEntePersonale and Principale=0) as RuoliSecondari, "
            strSQL = strSQL & "case b.Accreditato "
            strSQL = strSQL & "when 1 then CASE WHEN d.IDStatoEnte=6 THEN 'Iscritto' ELSE 'Iscritto' END "
            strSQL = strSQL & "when 0 then 'Da Valutare' "
            strSQL = strSQL & "when -1 then 'Chiuso' "
            strSQL = strSQL & "else 'Non Definito' end as Accreditato, "
            strSQL = strSQL & "'Propria' as Tipologia, '6' as Approvato, NULL as IDPersonaleAcquisito,b.UsernameInseritore, b.DataInseritore  "
            strSQL = strSQL & "from EntePersonale "
            strSQL = strSQL & "inner join entepersonaleruoli as b on EntePersonale.IDEntePersonale=b.IDEntePersonale inner join ruoli on b.IDRuolo=ruoli.IDRuolo "
            strSQL = strSQL & "inner join enti as d on EntePersonale.IDEnte=d.IDEnte "

            'la validità del FiltroFase è stata controllata prima
            If Not String.IsNullOrEmpty(txtFiltroFase.Text) Then
                strSQL = strSQL & " left join EntiFasi_Risorse efr on efr.IdEntePersonaleRuolo=b.IDEntePersonaleRuolo "
                strSQL = strSQL & " left join EntiFasi_Personale efp on efp.IdEntePersonale=entepersonale.IDEntePersonale "
            End If

            strSQL = strSQL & " where "
            'If UCase(Request.QueryString("esporta")) = "SI" Then
            strSQL = strSQL & "EntePersonale.IDEnte=" & CInt(Session("idEnte")) & " and b.datafinevalidità is null and  Ruoli.Nascosto = 0 "
            'Else
            'strSQL = strSQL & "EntePersonale.IDEnte=" & CInt(Session("idEnte")) & " "
            'End If
            If Session("TipoUtente") = "E" Then
                strSQL = strSQL & " and RuoloAccreditamento=1 "
            End If
            If strSQLFiltriRicerca <> "" Then
                strSQL = strSQL & strSQLFiltriRicerca
            End If
            strSQL = strSQL & "union "
            strSQL = strSQL & "SELECT 1 ordina, enti.Denominazione, entepersonale.IDEntePersonale,  enti.IDEnte as IDEnte, "
            strSQL = strSQL & "entepersonale.Cognome + ' ' + entepersonale.Nome as Nominativo,  entepersonale.Titolo, "
            strSQL = strSQL & "entepersonale.Posizione, entepersonale.DataFineValidità, case when EntePersonale.datafinevalidità is null then 'Attiva'	when not EntePersonale.datafinevalidità is null then 'Cancellata' end as Stato, "
            'strSQL = strSQL & "case entepersonaleruoli.Principale when 1 then ruoli.Ruolo end as Ruolo, "
            strSQL = strSQL & "ruoli.Ruolo as Ruolo, " 'SOSTITUISCE RIGA PRECEDENTE. MODIFICA DA METTERE BY DANILO
            strSQL = strSQL & "ruoli.IdRuolo as IdRuolo, " 'Aggiunta Informazione su Id ruolo per controllo Rappresentente legale
            'strSQL = strSQL & "(SELECT count(*) from entepersonaleruoli where IDEntePersonale=entepersonaleruoli.IDEntePersonale and entepersonaleruoli.Principale=0) as RuoliSecondari, "
            'strSQL = strSQL & "NULL as RuoliSecondari, " 'SOSTITUISCE RIGA PRECEDENTE. MODIFICA DA METTERE BY DANILO
            strSQL = strSQL & "case b.Accreditato "
            strSQL = strSQL & "when 1 then 'Iscritto' "
            strSQL = strSQL & "when 0 then 'Da Valutare' "
            strSQL = strSQL & "when -1 then 'Chiuso' "
            strSQL = strSQL & "else 'Non Definito' end as Accreditato,  "
            strSQL = strSQL & "'Acquisito' as Tipologia, personaleacquisito.Approvato as Approvato, personaleacquisito.IDPersonaleAcquisito,b.UsernameInseritore,b.DataInseritore  "
            strSQL = strSQL & "FROM entepersonaleruoli as b "
            strSQL = strSQL & "inner join ruoli on b.IDRuolo=ruoli.IDRuolo INNER JOIN personaleacquisito ON b.IDEntePersonaleRuolo = personaleacquisito.IDEntePersonaleRuolo "
            strSQL = strSQL & "INNER JOIN entepersonale ON b.IDEntePersonale = entepersonale.IDEntePersonale INNER JOIN enti ON entepersonale.IDEnte = enti.IDEnte "
            'strSQL = strSQL & "EntePersonale.IDEnte=" & CInt(Session("idEnte")) & " and b.datafinevalidità is null "
            'If UCase(Request.QueryString("esporta")) = "SI" Then
            'la validità del FiltroFase è stata controllata prima
            If Not String.IsNullOrEmpty(txtFiltroFase.Text) Then
                strSQL = strSQL & " left join EntiFasi_Risorse efr on efr.IdEntePersonaleRuolo=b.IDEntePersonaleRuolo "
                strSQL = strSQL & " left join EntiFasi_Personale efp on efp.IdEntePersonale=entepersonale.IDEntePersonale "
            End If
            strSQL = strSQL & "WHERE (personaleacquisito.IDEnteAcquirente=" & CInt(Session("idEnte")) & ") AND (personaleacquisito.Approvato=3) and b.datafinevalidità is null and  Ruoli.Nascosto = 0 "
            'Else
            '    strSQL = strSQL & "WHERE (personaleacquisito.IDEnteAcquirente=" & CInt(Session("idEnte")) & ") AND (personaleacquisito.Approvato=3) "
            'End If
            If Session("TipoUtente") = "E" Then
                strSQL = strSQL & " and RuoloAccreditamento=1 "
            End If
            If strSQLFiltriRicercaRuoli <> "" Then
                strSQL = strSQL & strSQLFiltriRicercaRuoli
            Else
                If strSQLFiltriRicerca <> "" Then
                    strSQL = strSQL & strSQLFiltriRicerca
                End If
            End If
            strSQL = strSQL & " order by ordina, entepersonale.Datafinevalidità, Nominativo"
            'fine preparazione query
            '************************
            'eseguo la query e assegno il dataset alla datagrid del risultato della ricerca assegno 
            dtsRisRicerca = ClsServer.DataSetGenerico(strSQL, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
            dtgRisultatoRicerca.DataSource = dtsRisRicerca
            dtgRisultatoRicerca.DataBind()

            'controllo se vengo da accettazione progetti per nascondere la colonna del pulsante
            If Request.QueryString("VengoDaProgetti") = "AccettazioneProgetti" Then
                dtgRisultatoRicerca.Columns(0).Visible = False
            End If


            If dtgRisultatoRicerca.Items.Count > 0 Then
                CmdEsportaPersonale.Visible = True
            Else
                CmdEsportaPersonale.Visible = False
            End If
            ColoraCelle()

            '*********************************************************************************
            'blocco per la creazione della datatable per la stampa della ricerca

            'nome e posizione di lettura delle colopnne a base 0
            Dim NomeColonne(4) As String
            Dim NomiCampiColonne(4) As String
            'nome della colonna 
            'e posizione nella griglia di lettura
            NomeColonne(0) = "Nominativo"
            NomeColonne(1) = "Ruoli"
            NomeColonne(2) = "Posizione"
            NomeColonne(3) = "Accreditato"
            NomeColonne(4) = "Tipologia"
            NomiCampiColonne(0) = "Nominativo"
            NomiCampiColonne(1) = "Ruolo"
            NomiCampiColonne(2) = "Posizione"
            NomiCampiColonne(3) = "Accreditato"
            NomiCampiColonne(4) = "Tipologia"
            'carico un datatable che userò poi nella pagina di stampa
            'il numero delle colonne è a base 0
            CaricaDataTablePerStampa(dtsRisRicerca, 4, NomeColonne, NomiCampiColonne)

            '*********************************************************************************

            'controllaChek()

            If dtgRisultatoRicerca.Items.Count = 0 Then
                CmdEsportaPersonale.Visible = False
            Else
                CmdEsportaPersonale.Visible = True
            End If

        Catch ex As Exception
            Response.Write(ex.Message.ToString())
        End Try
    End Sub

    Sub RicercaServizi()
        Dim abilitato As Integer
        Dim dati As Integer
        Dim annullamodifica As Integer
        Dim visualizzadatiaccreditati As Integer
        Dim annullacancellazione As Integer
        Dim messaggio As String

        Dim Strsql As String
        Dim dtsServizi As DataSet
        '<img src=images/canc.jpg title='Cancella' border=0>
        Strsql = "SELECT sistemi.idsistema,"
        Strsql = Strsql & " sistemi.sistema,"
        Strsql = Strsql & " Enti.denominazione,"
        Strsql = Strsql & " Enti.Codiceregione,"
        Strsql = Strsql & " case EntiAcquisizioneServizi.StatoRichiesta when 0 then 'Registrato' when 1 then 'Confermato' when 2 then 'Annullato' when 3 then 'Respinto' end as Stato, "
        'Strsql = Strsql & " case EntiAcquisizioneServizi.StatoRichiesta when 0 then '<img src=images/canc.jpg title=""Cancella"" style=""cursor: hand"" onclick=""javascript: CancellaServizio(' + convert(varchar,EntiAcquisizioneServizi.IDEnteAcquisizione) + ')"" border=0>' when 1 then '<img src=images/canc.jpg title=""Cancella"" style=""cursor: hand"" onclick=""javascript: CancellaServizio(' + convert(varchar,EntiAcquisizioneServizi.IDEnteAcquisizione) + ')"" border=0>' when 2 then case EntiAcquisizioneServizi.RichiestaCancellazione when 1 then  '<img src=images/Sostituzione1.gif title=""Annulla Cancellazione"" style=""cursor: hand"" onclick=""javascript: AnnullaCancellaServizio(' + convert(varchar,EntiAcquisizioneServizi.IDEnteAcquisizione) + ')"" border=0>' end  end as Elimina "
        Strsql = Strsql & " EntiAcquisizioneServizi.StatoRichiesta,EntiAcquisizioneServizi.RichiestaCancellazione, EntiAcquisizioneServizi.IDEnteAcquisizione "
        Strsql = Strsql & " FROM entisistemi "
        Strsql = Strsql & " inner join sistemi on sistemi.idsistema=entisistemi.idsistema "
        Strsql = Strsql & " inner join Enti on enti.idente=entisistemi.idente "
        Strsql = Strsql & " INNER Join "
        Strsql = Strsql & " EntiAcquisizioneServizi ON entisistemi.IDEnteSistema = EntiAcquisizioneServizi.idEnteSistema "
        Strsql = Strsql & " WHERE  Sistemi.Nascosto=0 and  (EntiAcquisizioneServizi.idEnteSecondario = " & Session("idEnte") & ")"
        Strsql = Strsql & " UNION "
        Strsql = Strsql & " SELECT 0,"
        Strsql = Strsql & " 'Formazione' as sistema,"
        Strsql = Strsql & " 'Regione' as denominazione,"
        Strsql = Strsql & " 'Regione' as Codiceregione,"
        Strsql = Strsql & " case EntiAcquisizioneServizi.StatoRichiesta when 0 then 'Registrato' when 1 then 'Confermato' when 2 then 'Annullato' when 3 then 'Respinto' end as Stato, "
        'Strsql = Strsql & " case EntiAcquisizioneServizi.StatoRichiesta when 0 then '<img src=images/canc.jpg title=""Cancella"" style=""cursor: hand"" onclick=""javascript: CancellaServizio(' + convert(varchar,EntiAcquisizioneServizi.IDEnteAcquisizione) + ')"" border=0>' when 1 then '<img src=images/canc.jpg title=""Cancella"" style=""cursor: hand"" onclick=""javascript: CancellaServizio(' + convert(varchar,EntiAcquisizioneServizi.IDEnteAcquisizione) + ')"" border=0>' when 2 then case EntiAcquisizioneServizi.RichiestaCancellazione when 1 then  '<img src=images/Sostituzione1.gif title=""Annulla Cancellazione"" style=""cursor: hand"" onclick=""javascript: AnnullaCancellaServizio(' + convert(varchar,EntiAcquisizioneServizi.IDEnteAcquisizione) + ')""  border=0>' end end as Elimina "
        Strsql = Strsql & " EntiAcquisizioneServizi.StatoRichiesta, EntiAcquisizioneServizi.RichiestaCancellazione, EntiAcquisizioneServizi.IDEnteAcquisizione "
        '''Strsql = Strsql & " EntiAcquisizioneServizi.IdEnteAcquisizione,"
        '''Strsql = Strsql & " Enti.codiceregione as codreg,"
        '''Strsql = Strsql & " Enti.denominazione as denominazioneentesecondario"
        Strsql = Strsql & " FROM Enti "
        Strsql = Strsql & " INNER Join "
        Strsql = Strsql & " EntiAcquisizioneServizi ON Enti.IDEnte = EntiAcquisizioneServizi.IdEnteSecondario "
        Strsql = Strsql & " INNER Join "
        Strsql = Strsql & " statiEnti ON enti.IDstatoEnte=statienti.idstatoente "
        'Strsql = Strsql & " WHERE EntiAcquisizioneServizi.IdEnteSistema IS NULL "
        Strsql = Strsql & " WHERE EntiAcquisizioneServizi.IdEnteSistema IS NULL AND (EntiAcquisizioneServizi.idEnteSecondario = '" & Session("idEnte") & "')"



        dtsServizi = ClsServer.DataSetGenerico(Strsql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
        dgServizi.DataSource = dtsServizi
        dgServizi.DataBind()
        'antonello 08/0572008
        If Session("TipoUtente") = "E" Then
            'If ClsUtility.ControllaStatoEntePerBloccareMaschereAnagrafica(Session("IdEnte"), IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn"))) = True Then
            Accesso_Maschera_Servizi(Session("TipoUtente"), Session("IDEnte"), abilitato, dati, annullamodifica, annullacancellazione, visualizzadatiaccreditati, messaggio)
            If abilitato = 0 Then
                dgServizi.Columns(6).Visible = False
                'Else
            End If
            dgServizi.Columns(0).Visible = False
        End If
        If dgServizi.Items.Count = 0 Then
            lblServizi.Visible = False
            dgServizi.Visible = False
        End If

        CaricaDataTablePerStampa1(dtsServizi)

        If dgServizi.Items.Count = 0 Then
            dgServizi.Visible = False
            lblServizi.Visible = False
            'LblStampaServizi.Visible = False
            CmdEsportaServizi.Visible = False
        Else
            dgServizi.Visible = True
            lblServizi.Visible = True
            'LblStampaServizi.Visible = True
            CmdEsportaServizi.Visible = True
        End If


    End Sub

    Private Sub AccreditamentoEliminaServizi(ByVal IdEnteAcquisizione As Integer)
        'Creata il:		26/08/2014
        'Funzionalità: richiamo store SP_ACCREDITAMENTO_ELIMINAMASCHERA_SERVIZI per la cancellazione del servizio

        Dim SqlCmd As New SqlClient.SqlCommand
        Try
            SqlCmd.CommandText = "SP_ACCREDITAMENTO_ELIMINAMASCHERA_SERVIZI"
            SqlCmd.CommandType = CommandType.StoredProcedure
            SqlCmd.Connection = Session("Conn")

            SqlCmd.Parameters.Add("@IdEnteAcquisizione ", SqlDbType.Int).Value = IdEnteAcquisizione
            SqlCmd.Parameters.Add("@UsernameRichiesta", SqlDbType.VarChar).Value = Session("Utente")

            'Esito aggiornamento: 0-Errore 1-Aggiornamento effettuato
            SqlCmd.Parameters.Add("@Esito", SqlDbType.TinyInt)
            SqlCmd.Parameters("@Esito").Direction = ParameterDirection.Output

            SqlCmd.Parameters.Add("@messaggio", SqlDbType.VarChar)
            SqlCmd.Parameters("@messaggio").Size = 1000
            SqlCmd.Parameters("@messaggio").Direction = ParameterDirection.Output

            SqlCmd.ExecuteNonQuery()

            'lblMessaggio.Text = SqlCmd.Parameters("@messaggio").Value()

        Catch ex As Exception
            ' lblMessaggio.Text = ex.Message
        Finally

        End Try
    End Sub

    Private Sub AccreditamentoAnnullaEliminaServizi(ByVal IdEnteAcquisizione As Integer)
        'Creata il:		26/08/2014
        'Funzionalità: richiamo store SP_ACCREDITAMENTO_ANNULLAELIMINAMASCHERA_SERVIZI per il ripristino del servizio

        Dim SqlCmd As New SqlClient.SqlCommand
        Try
            SqlCmd.CommandText = "SP_ACCREDITAMENTO_ANNULLAELIMINAMASCHERA_SERVIZI"
            SqlCmd.CommandType = CommandType.StoredProcedure
            SqlCmd.Connection = Session("Conn")

            SqlCmd.Parameters.Add("@IdEnteAcquisizione ", SqlDbType.Int).Value = IdEnteAcquisizione
            SqlCmd.Parameters.Add("@UsernameRichiesta", SqlDbType.VarChar).Value = Session("Utente")

            'Esito aggiornamento: 0-Errore 1-Aggiornamento effettuato
            SqlCmd.Parameters.Add("@Esito", SqlDbType.TinyInt)
            SqlCmd.Parameters("@Esito").Direction = ParameterDirection.Output

            SqlCmd.Parameters.Add("@messaggio", SqlDbType.VarChar)
            SqlCmd.Parameters("@messaggio").Size = 1000
            SqlCmd.Parameters("@messaggio").Direction = ParameterDirection.Output

            SqlCmd.ExecuteNonQuery()

            'lblMessaggio.Text = SqlCmd.Parameters("@messaggio").Value()

        Catch ex As Exception
            ' lblMessaggio.Text = ex.Message
        Finally

        End Try
    End Sub

    Private Sub ColoraCelle()
        'Generato da Alessandra Taballione il 15/06/04
        'VAriazione del Colore secondo lo stato della risorsa.
        'Accreditata=Verde;da accreditare=gialla;non accreditata=Rossa
        Dim item As DataGridItem
        Dim intConta As Integer
        For Each item In dtgRisultatoRicerca.Items
            If dtgRisultatoRicerca.Items(item.ItemIndex).Cells(11).Text = "Cancellata" Then
                For intConta = 0 To 13
                    dtgRisultatoRicerca.Items(item.ItemIndex).Cells(intConta).BackColor = Color.LightSalmon
                Next
            Else
                Select Case dtgRisultatoRicerca.Items(item.ItemIndex).Cells(6).Text
                    Case "Accreditato"
                        For intConta = 0 To 13
                            dtgRisultatoRicerca.Items(item.ItemIndex).Cells(intConta).BackColor = Color.LightGreen
                        Next
                    Case "Da Valutare"
                        If Mid(dtgRisultatoRicerca.Items(item.ItemIndex).Cells(12).Text, 1, 1) = "N" Then
                            For intConta = 0 To 13
                                dtgRisultatoRicerca.Items(item.ItemIndex).Cells(intConta).BackColor = Color.WhiteSmoke
                            Next
                        Else
                            For intConta = 0 To 13
                                dtgRisultatoRicerca.Items(item.ItemIndex).Cells(intConta).BackColor = Color.Khaki
                            Next
                        End If
                    Case "Chiuso"
                        For intConta = 0 To 13
                            dtgRisultatoRicerca.Items(item.ItemIndex).Cells(intConta).BackColor = Color.LightSalmon
                        Next
                End Select
            End If
        Next
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

    Private Sub Accesso_Maschera_Servizi(ByVal TipoUtente As String, ByVal IdEnte As Integer, ByRef abilitato As Integer, ByRef dati As Integer, ByRef annullamodifica As Integer, ByRef annullacancellazione As Integer, ByRef visualizzadatiaccreditati As Integer, ByRef messaggio As String)

        'REALIZZATA DA: SIMONA CORDELLA 
        'DATA REALIZZAZIONE:  26/08/2014
        'FUNZIONALITA':  Verifica le condizioni di accreditamento e le eventuali modifiche in corso sui dati della risorsa
        '	             e ritorna all'' applicazione la configurazione da applicare alla maschera di gestione
        '               @abilitato  			    -- 0: maschera sola lettura		1: maschera in modifica
        '               @dati 					    -- 0: dati tabelle reali		1: dati tabelle variazioni
        '               @annullamodifica  			-- 0: funzione non abilitata	1: funzione abilitata
        '               @visualizzadatiaccreditati 	-- 0: funzione non abilitata	1: funzione abilitata
        '               @annullacancellazione       -- 0: funzione non abilitata	1: funzione abilitata
        '               @messaggio                  -- eventuale messaggio di ritorno da visualizzare all'utente



        Try
            Dim SqlCmd As SqlClient.SqlCommand


            SqlCmd = New SqlClient.SqlCommand
            SqlCmd.CommandText = "SP_ACCREDITAMENTO_ACCESSOMASCHERA_SERVIZI"
            SqlCmd.CommandType = CommandType.StoredProcedure
            SqlCmd.Connection = Session("Conn")
            SqlCmd.Parameters.Add("@TipoUtente", SqlDbType.VarChar).Value = TipoUtente
            SqlCmd.Parameters.Add("@IdEnte", SqlDbType.Int).Value = IdEnte

            Dim sparam1 As SqlClient.SqlParameter
            sparam1 = New SqlClient.SqlParameter
            sparam1.ParameterName = "@abilitato"
            'sparam1.Size = 100
            sparam1.SqlDbType = SqlDbType.TinyInt
            sparam1.Direction = ParameterDirection.Output
            SqlCmd.Parameters.Add(sparam1)

            Dim sparam2 As SqlClient.SqlParameter
            sparam2 = New SqlClient.SqlParameter
            sparam2.ParameterName = "@dati"
            'sparam1.Size = 100
            sparam2.SqlDbType = SqlDbType.TinyInt
            sparam2.Direction = ParameterDirection.Output
            SqlCmd.Parameters.Add(sparam2)

            Dim sparam3 As SqlClient.SqlParameter
            sparam3 = New SqlClient.SqlParameter
            sparam3.ParameterName = "@annullamodifica"
            'sparam1.Size = 100
            sparam3.SqlDbType = SqlDbType.TinyInt
            sparam3.Direction = ParameterDirection.Output
            SqlCmd.Parameters.Add(sparam3)

            Dim sparam4 As SqlClient.SqlParameter
            sparam4 = New SqlClient.SqlParameter
            sparam4.ParameterName = "@visualizzadatiaccreditati"
            'sparam1.Size = 100
            sparam4.SqlDbType = SqlDbType.TinyInt
            sparam4.Direction = ParameterDirection.Output
            SqlCmd.Parameters.Add(sparam4)

            Dim sparam5 As SqlClient.SqlParameter
            sparam5 = New SqlClient.SqlParameter
            sparam5.ParameterName = "@annullacancellazione"
            'sparam1.Size = 100
            sparam5.SqlDbType = SqlDbType.TinyInt
            sparam5.Direction = ParameterDirection.Output
            SqlCmd.Parameters.Add(sparam5)


            Dim sparam6 As SqlClient.SqlParameter
            sparam6 = New SqlClient.SqlParameter
            sparam6.ParameterName = "@messaggio"
            sparam6.Size = 1000
            sparam6.SqlDbType = SqlDbType.VarChar
            sparam6.Direction = ParameterDirection.Output
            SqlCmd.Parameters.Add(sparam6)

            SqlCmd.ExecuteNonQuery()


            abilitato = SqlCmd.Parameters("@abilitato").Value
            dati = SqlCmd.Parameters("@dati").Value
            annullamodifica = SqlCmd.Parameters("@annullamodifica").Value
            visualizzadatiaccreditati = SqlCmd.Parameters("@visualizzadatiaccreditati").Value
            annullacancellazione = SqlCmd.Parameters("@annullacancellazione").Value
            messaggio = SqlCmd.Parameters("@messaggio").Value
        Catch ex As Exception

        Finally
            'ConnX.Close()
            'SqlCmd = Nothing
        End Try
    End Sub

    Sub CaricaDataTablePerStampa1(ByVal DataSetDaScorrere As DataSet)
        Dim dt As New DataTable
        Dim dr As DataRow
        Dim i As Integer
        Dim x As Integer

        Dim NomeColonne(3) As String
        Dim NomiCampiColonne(3) As String

        'nome della colonna 
        'e posizione nella griglia di lettura

        NomeColonne(0) = "Servizi Acquisiti"
        NomeColonne(1) = "Ente"
        NomeColonne(2) = "Codice Ente"
        NomeColonne(3) = "Stato"


        NomiCampiColonne(0) = "Sistema"
        NomiCampiColonne(1) = "Denominazione"
        NomiCampiColonne(2) = "CodiceRegione"
        NomiCampiColonne(3) = "Stato"

        'carico i nomi delle colonne che andrò a stampare nella datagrid
        For x = 0 To 3
            dt.Columns.Add(New DataColumn(NomeColonne(x), GetType(String)))
        Next

        'carico il datatable con il risultato della query della ricerca, in qusto caso delle risorse
        If DataSetDaScorrere.Tables(0).Rows.Count > 0 Then
            For i = 1 To DataSetDaScorrere.Tables(0).Rows.Count
                dr = dt.NewRow()
                For x = 0 To 3
                    dr(x) = DataSetDaScorrere.Tables(0).Rows.Item(i - 1).Item(NomiCampiColonne(x))
                Next
                dt.Rows.Add(dr)
            Next
        End If

        'passo alla sessione la datatable che ho appena creato e che userò per il databinding della datagrid della stampa
        Session("DtbRicerca1") = dt

    End Sub

    Private Sub dtgRisultatoRicerca_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dtgRisultatoRicerca.PageIndexChanged
        'cambio la pagina riassegnando il dataset dichiarato pubblico a tutt ala pagina
        If Not checkFiltroFase() Then Exit Sub
        dtgRisultatoRicerca.CurrentPageIndex = e.NewPageIndex
        RicercaPersonaliEnte(Session("idEnte"))
        'dtgRisultatoRicerca.DataSource = dtsRisRicerca
        'dtgRisultatoRicerca.DataBind()


        'controllo se vengo da accettazione progetti per nascondere la colonna del pulsante
        If Request.QueryString("VengoDaProgetti") = "AccettazioneProgetti" Then
            dtgRisultatoRicerca.Columns(0).Visible = False
        End If

        ColoraCelle()
        dtgRisultatoRicerca.SelectedIndex = -1
        'controllaChek()
    End Sub

    Private Sub dtgRisultatoRicerca_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtgRisultatoRicerca.SelectedIndexChanged
        Dim intPersonaleEnteAcquisito As Integer

        'controllo se è selezionato un item nella griglia
        If Not dtgRisultatoRicerca.SelectedItem Is Nothing Then
            'assegno alle due variabili pubbliche condivisa nella pagina chiamata entepersonale "idrisorsaselezionata,idente"
            'entepersonale.intEnteAssociato = CInt(dtgRisultatoRicerca.SelectedItem.Cells(2).Text)
            'entepersonale.intPersonaleEnteAssociato = CInt(dtgRisultatoRicerca.SelectedItem.Cells(1).Text)
            'controllo se la selezione è vuota pwerchè se lo è si tratta di risorsa acquisita
            If dtgRisultatoRicerca.SelectedItem.Cells(9).Text <> "&nbsp;" Then
                'assegno alla terza variabile "idrisorsaacquisita"
                intPersonaleEnteAcquisito = CInt(dtgRisultatoRicerca.SelectedItem.Cells(9).Text)
            Else
                intPersonaleEnteAcquisito = 0
            End If
            If dtgRisultatoRicerca.SelectedItem.Cells(11).Text = "Attiva" Then
                'controllo e faccio il redirect alla pagina di modifica postando il vlore Acquisito
                'che prendi vero o falso
                If dtgRisultatoRicerca.SelectedItem.Cells(7).Text = "Acquisito" Then
                    Response.Redirect("entepersonale.aspx?VengoDaProgetti=" & Request.QueryString("VengoDaProgetti") & "&intPersonaleEnteAcquisito=" & intPersonaleEnteAcquisito & "&intPersonaleEnteAssociato=" & CInt(dtgRisultatoRicerca.SelectedItem.Cells(1).Text) & "&intEnteAssociato=" & CInt(dtgRisultatoRicerca.SelectedItem.Cells(2).Text) & "&Denominazione=" & dtgRisultatoRicerca.SelectedItem.Cells(10).Text & "&Acquisito=" & "Vero" & "&tipoazione=" & "Modifica" & "")
                Else
                    Response.Redirect("entepersonale.aspx?VengoDaProgetti=" & Request.QueryString("VengoDaProgetti") & "&intPersonaleEnteAcquisito=" & intPersonaleEnteAcquisito & "&intPersonaleEnteAssociato=" & CInt(dtgRisultatoRicerca.SelectedItem.Cells(1).Text) & "&intEnteAssociato=" & CInt(dtgRisultatoRicerca.SelectedItem.Cells(2).Text) & "&Acquisito=" & "Falso" & "&tipoazione=" & "Modifica" & "")
                End If
            Else
                Response.Redirect("entepersonale.aspx?VengoDaProgetti=" & Request.QueryString("VengoDaProgetti") & "&intPersonaleEnteAcquisito=" & intPersonaleEnteAcquisito & "&intPersonaleEnteAssociato=" & CInt(dtgRisultatoRicerca.SelectedItem.Cells(1).Text) & "&intEnteAssociato=" & CInt(dtgRisultatoRicerca.SelectedItem.Cells(2).Text) & "&Denominazione=" & dtgRisultatoRicerca.SelectedItem.Cells(10).Text & "&Cancellato=" & "Vero" & "&tipoazione=" & "Modifica" & "")
            End If
        End If

    End Sub

    Private Sub cmdChiudi_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdChiudi.Click
        If Request.QueryString("VengoDaProgetti") = "AccettazioneProgetti" Then
            Response.Redirect("assegnazionevincoliprogetti.aspx?idattivita=" & Request.QueryString("idattivita") & "&tipologia=" & Request.QueryString("tipologia") & "&Nazionale=" & Request.QueryString("Nazionale"))
        Else
            Response.Redirect("WfrmMain.aspx")
        End If
    End Sub

    Private Sub dgServizi_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgServizi.PageIndexChanged
        dgServizi.CurrentPageIndex = e.NewPageIndex
        RicercaServizi()
        dgServizi.SelectedIndex = -1
    End Sub

    Private Sub CmdEsportaPersonale_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdEsportaPersonale.Click
        CmdEsportaPersonale.Visible = False
        Dim dtbRicerca As DataTable = Session("DtbRicerca")
        StampaCSVPersonale(dtbRicerca)
    End Sub

    Private Sub StampaCSVPersonale(ByVal dtbRicerca As DataTable)
        Dim path As String
        Dim xPrefissoNome As String
        Dim url As String
        Dim utility As ClsUtility = New ClsUtility()

        If dtbRicerca.Rows.Count = 0 Then
            ApriCSV1Personale.Visible = False
            CmdEsportaPersonale.Visible = False
        Else
            xPrefissoNome = Session("Utente")
            path = Server.MapPath("download")
            url = CreaFileCSV(dtbRicerca, xPrefissoNome, path)
            ApriCSV1Personale.Visible = True
            ApriCSV1Personale.NavigateUrl = url
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

    Private Sub CmdEsportaServizi_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdEsportaServizi.Click
        CmdEsportaServizi.Visible = False
        Dim dtbRicerca As DataTable = Session("DtbRicerca1")
        StampaCSVServizi(dtbRicerca)
    End Sub

    Private Sub StampaCSVServizi(ByVal dtbRicerca As DataTable)
        Dim path As String
        Dim xPrefissoNome As String
        Dim url As String
        Dim utility As ClsUtility = New ClsUtility()

        If dtbRicerca.Rows.Count = 0 Then
            ApriCSV1Servizi.Visible = False
            CmdEsportaServizi.Visible = False
        Else
            xPrefissoNome = Session("Utente")
            path = Server.MapPath("download")
            url = CreaFileCSV(dtbRicerca, xPrefissoNome, path)
            ApriCSV1Servizi.Visible = True
            ApriCSV1Servizi.NavigateUrl = url
        End If

    End Sub

    Private Sub dgServizi_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgServizi.ItemCommand
        lblServizi.Text = ""
        If e.CommandName = "Seleziona" Then

            'Apro la maschera di dettaglio dei Servizi acquisiti
            Response.Write("<script>")
            Response.Write("window.open('ricercaentepersonaleacquisibile.aspx?IdSist=" & e.Item.Cells(1).Text & "&CodEnte=" & e.Item.Cells(4).Text & "','ricerca','height=450,width=1000,dependent=yes,scrollbars=yes,status=no,resizable=no')")
            Response.Write("</script>")

        End If

        If e.CommandName = "Cancella" Then
            Dim IDEnteAcquisizione As String = dgServizi.DataKeys(e.Item.ItemIndex).ToString
            If VerificaFaseServizio(Session("IdEnte"), CInt(IDEnteAcquisizione)) = True Then
                lblServizi.Text = "Non è possibile modifica il Servizio Acquisito perchè associato ad una richiesta di Adeguamento non ancora valutata."
                lblServizi.ForeColor = Color.Red
            Else
                Response.Redirect("ricercaentepersonale.aspx?AnnullaServizio=NO&strIDEnteAcquisizione=" & IDEnteAcquisizione)
            End If


        End If

        If e.CommandName = "Annulla" Then
            Dim IDEnteAcquisizione As String = dgServizi.DataKeys(e.Item.ItemIndex).ToString

            If VerificaFaseServizio(Session("IdEnte"), CInt(IDEnteAcquisizione)) = True Then
                lblServizi.Text = "Non è possibile modifica il Servizio Acquisito perchè associato ad una richiesta di Adeguamento non ancora valutata."
                lblServizi.ForeColor = Color.Red
            Else
                Response.Redirect("ricercaentepersonale.aspx?AnnullaServizio=SI&strIDEnteAcquisizione=" & IDEnteAcquisizione)
            End If
        End If

    End Sub

    Private Sub dgServizi_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgServizi.ItemDataBound

        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then

            Dim ImgElimina As ImageButton = DirectCast(e.Item.FindControl("ImgElimina"), ImageButton)
            Dim ImgAnnulla As ImageButton = DirectCast(e.Item.FindControl("ImgAnnulla"), ImageButton)

            Dim statoRichiesta As String = IIf(IsDBNull(e.Item.DataItem("StatoRichiesta")), String.Empty, e.Item.DataItem("StatoRichiesta"))
            Dim richiestaCancellazione = IIf(IsDBNull(e.Item.DataItem("RichiestaCancellazione")), String.Empty, e.Item.DataItem("RichiestaCancellazione"))

            If statoRichiesta = "0" Or statoRichiesta = "1" Then

                ImgElimina.Visible = True
                ImgAnnulla.Visible = False

            ElseIf statoRichiesta = "2" And richiestaCancellazione = "1" Then

                ImgElimina.Visible = False
                ImgAnnulla.Visible = True
            Else

                ImgElimina.Visible = False
                ImgAnnulla.Visible = False
            End If

        End If

    End Sub

    Protected Sub cmdRicerca_Click(sender As Object, e As EventArgs) Handles cmdRicerca.Click
        dtgRisultatoRicerca.CurrentPageIndex = 0
        RicercaPersonaliEnte(Session("idEnte"))
        RicercaServizi()
    End Sub

    Private Function VerificaFaseServizio(ByVal idEnte As Integer, ByVal idEnteAcquisizione As Integer) As Boolean
        '** Aggiunto da Simona Cordella il 30/11/2015
        '** se ilserivizo selezioantto fa parte di una fase aperte o valutata SI 
        Dim strSql As String
        Dim dtRFase As SqlClient.SqlDataReader
        VerificaFaseServizio = True
        If Session("TipoUtente") <> "E" Then
            VerificaFaseServizio = False
        Else
            'strSql = " SELECT ef.identefase "
            'c FROM EntiFasi ef "
            'strSql &= " inner join EntiFasi_ServiziAcquisiti efsa on ef.IdEnteFase=efsa.IdEnteFase"
            'strSql &= " WHERE  ef.tipofase in (1,2) and ef.IdEnte= " & idEnte & "  and idEnteAcquisizione = " & idEnteAcquisizione
            'strSql &= " AND CASE  ef.stato  "
            'strSql &= "     WHEN 1 then case when  GETDATE() between DataInizioFase and DataFineFase then 'Aperta' ELSE 'Scaduta' end  	"
            'strSql &= "     WHEN 2 then 'Annullata' "
            'strSql &= "     WHEN 3 then 'Presentata' "
            'strSql &= "     WHEN 4 then 'Valutata' end  in ('Aperta','Valutata')"
            strSql = " Select A.idEnteAcquisizione"
            strSql &= " FROM EntiFasi_ServiziAcquisiti A"
            strSql &= " INNER JOIN EntiAcquisizioneServizi B ON A.idEnteAcquisizione = B.idEnteAcquisizione"
            strSql &= " WHERE(B.idEnteAcquisizione = " & idEnteAcquisizione & " And Stato Is NULL)"
            dtRFase = ClsServer.CreaDatareader(strSql, Session("conn"))
            VerificaFaseServizio = dtRFase.HasRows = True
            ChiudiDataReader(dtRFase)
        End If
        Return VerificaFaseServizio
    End Function

    Protected Sub dgServizi_SelectedIndexChanged(sender As Object, e As EventArgs) Handles dgServizi.SelectedIndexChanged

    End Sub

    Private Sub dtgRisultatoRicerca_ItemDataBound(sender As Object, e As DataGridItemEventArgs) Handles dtgRisultatoRicerca.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then

            Dim ImgVolontario As ImageButton = DirectCast(e.Item.FindControl("ImgVolontario"), ImageButton)
            'If (IIf(IsDBNull(e.Item.DataItem("IdRuolo")), 0, CInt(e.Item.DataItem("IdRuolo")))) = 4 Then
            '    ImgVolontario.Visible = False
            'End If
        End If
    End Sub
End Class