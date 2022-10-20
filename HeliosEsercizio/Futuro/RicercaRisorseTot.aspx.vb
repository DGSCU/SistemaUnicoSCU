Imports System.Drawing.Printing
Imports System.Drawing.Imaging
Imports System.Web.UI
Imports System.Drawing
Imports System.IO

Public Class RicercaRisorseTot
    Inherits System.Web.UI.Page
    Dim dtsRisRicerca As DataSet
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Inserire qui il codice utente necessario per inizializzare la pagina
        'controllo se si tratta di una ricerca, controllo necessario per far si che non si perde il pageindex
        If Not Request.Form("checkpage") Is Nothing Then
            If Request.Form("checkpage") = "True" Then
                dtgRisultatoRicerca.CurrentPageIndex = 0
            End If
        End If
        'controllo login effettuato
        If Not Session("LogIn") Is Nothing Then
            If Session("LogIn") = False Then
                Response.Redirect("LogOn.aspx")
            End If
        Else
            Response.Redirect("LogOn.aspx")
        End If
        ''''''''''''''''''''se non sono state fatte selezioni esco dalla routine
        '''''''''''''''''''If Not dtgRisultatoRicerca.SelectedItem Is Nothing Then
        '''''''''''''''''''    Exit Sub
        '''''''''''''''''''End If
        'controllo se si tratta di primo caricamento della pagina
        If Page.IsPostBack = False Then
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
            If Session("CodiceRegioneEnte") <> "" Then
                TxtCodEnte.Text = Session("txtCodEnte")
            End If
            CaricaCompetenze()
        End If
      

        If UCase(Request.QueryString("esporta")) = "SI" Then
            dtgRisultatoRicerca.Columns(0).Visible = False
            If dtgRisultatoRicerca.Items.Count > 0 Then
                CmdEsporta.Visible = False
                'imgStampa.Visible = False
            Else
                CmdEsporta.Visible = False
                'imgStampa.Visible = False
            End If
        Else
            If dtgRisultatoRicerca.Items.Count > 0 Then
                'imgStampa.Visible = True
                CmdEsporta.Visible = True
            Else
                CmdEsporta.Visible = False
                'imgStampa.Visible = False
            End If
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
                strSQL = "select IdRegioneCompetenza,Descrizione,CodiceRegioneCompetenza,left(CodiceRegioneCompetenza,1)from RegioniCompetenze "
                strSQL = strSQL & "union "
                strSQL = strSQL & "select '0',' Tutti ','','A' from RegioniCompetenze order by left(CodiceRegioneCompetenza,1),descrizione "
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
                CboCompetenza.SelectedIndex = 1
            Else
                CboCompetenza.Enabled = False
                'preparo la query
                strSQL = "select b.IdRegioneCompetenza from RegioniCompetenze a "
                strSQL = strSQL & "INNER JOIN utentiunsc b ON a.idregionecompetenza = b.idregionecompetenza "
                strSQL = strSQL & "where b.username = '" & Session("Utente") & "'"
                'chiudo il datareader se aperto
                If Not dtrCompetenze Is Nothing Then
                    dtrCompetenze.Close()
                    dtrCompetenze = Nothing
                End If
                'eseguo la query
                dtrCompetenze = ClsServer.CreaDatareader(strSQL, Session("conn"))
                dtrCompetenze.Read()
                If dtrCompetenze.HasRows = True Then
                    CboCompetenza.SelectedValue = dtrCompetenze("IdRegioneCompetenza")
                    CboCompetenza.Enabled = True
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

    Private Sub ColoraCelle()
        'Generato da Alessandra Taballione il 15/06/04
        'VAriazione del Colore secondo lo stato della risorsa.
        'Accreditata=Verde;da accreditare=gialla;non accreditata=Rossa
        Dim item As DataGridItem
        Dim intConta As Integer
        For Each item In dtgRisultatoRicerca.Items
            If dtgRisultatoRicerca.Items(item.ItemIndex).Cells(11).Text = "Cancellata" Then
                For intConta = 0 To 18
                    dtgRisultatoRicerca.Items(item.ItemIndex).Cells(intConta).BackColor = Color.LightSalmon
                Next
            Else
                Select Case dtgRisultatoRicerca.Items(item.ItemIndex).Cells(6).Text
                    Case "Iscritto"
                        For intConta = 0 To 18
                            dtgRisultatoRicerca.Items(item.ItemIndex).Cells(intConta).BackColor = Color.LightGreen
                        Next
                    Case "Da Valutare"
                        If Mid(dtgRisultatoRicerca.Items(item.ItemIndex).Cells(12).Text, 1, 1) = "N" Then
                            For intConta = 0 To 18
                                dtgRisultatoRicerca.Items(item.ItemIndex).Cells(intConta).BackColor = Color.WhiteSmoke
                            Next
                        Else
                            For intConta = 0 To 18
                                dtgRisultatoRicerca.Items(item.ItemIndex).Cells(intConta).BackColor = Color.Khaki
                            Next
                        End If
                    Case "Chiuso"
                        For intConta = 0 To 18
                            dtgRisultatoRicerca.Items(item.ItemIndex).Cells(intConta).BackColor = Color.LightSalmon
                        Next
                End Select
            End If
        Next
    End Sub

    'routine che carica la griglia delle risorse al primo caricamento della pagina a seconda dell'ente loggato
    Sub CaricaGriglia(ByVal IdEnte As Integer)
        'variabile stringa per la insert nella base dati
        Dim strSQL As String
        Dim strSQLFiltriRicerca As String
        'pulisco la stringa dei filtri di ricerca
        strSQLFiltriRicerca = ""
        Try
            'preparo la query sulle risorse relative all'ente loggato Acquisito e NOn
            strSQL = "select 0 ordina, d.Denominazione, EntePersonale.IDEntePersonale, EntePersonale.IDEnte, "
            strSQL = strSQL & "EntePersonale.Cognome + ' ' + EntePersonale.Nome as Nominativo,  EntePersonale.Titolo, "
            strSQL = strSQL & "EntePersonale.Posizione, EntePersonale.DataFineValidità, case when EntePersonale.datafinevalidità is null then 'Attiva'	when not EntePersonale.datafinevalidità is null then 'Cancellata' end as Stato, "
            strSQL = strSQL & "case b.Principale when 1 then ruoli.Ruolo end as Ruolo, "
            'strSQL = strSQL & "(SELECT count(*) from entepersonaleruoli where IDEntePersonale=b.IDEntePersonale and Principale=0) as RuoliSecondari, "
            strSQL = strSQL & "case b.Accreditato "
            strSQL = strSQL & "when 1 then 'Iscritto' "
            strSQL = strSQL & "when 0 then 'Da Valutare' "
            strSQL = strSQL & "when -1 then 'Chiuso' "
            strSQL = strSQL & "else 'Non Definito' end as Accreditato,  "
            strSQL = strSQL & "'Propria' as Tipologia, '6' as Approvato, NULL IDPersonaleAcquisito "
            strSQL = strSQL & "from EntePersonale "
            strSQL = strSQL & "inner join entepersonaleruoli as b on EntePersonale.IDEntePersonale=b.IDEntePersonale inner join ruoli on b.IDRuolo=ruoli.IDRuolo "
            strSQL = strSQL & "inner join enti as d on EntePersonale.IDEnte=d.IDEnte where EntePersonale.IDEnte=" & CInt(Session("idEnte")) & " and b.Principale=1 "
            strSQL = strSQL & "union "
            strSQL = strSQL & "SELECT 1 ordina, enti.Denominazione, entepersonale.IDEntePersonale,  enti.IDEnte, "
            strSQL = strSQL & "entepersonale.Cognome + ' ' + entepersonale.Nome as Nominativo,  entepersonale.Titolo, "
            strSQL = strSQL & "entepersonale.Posizione, entepersonale.DataFineValidità, case when EntePersonale.datafinevalidità is null then 'Attiva'	when not EntePersonale.datafinevalidità is null then 'Cancellata' end as Stato, "
            'strSQL = strSQL & "case entepersonaleruoli.Principale when 1 then ruoli.Ruolo end as Ruolo, "
            strSQL = strSQL & "ruoli.Ruolo as Ruolo, " 'SOSTITUISCE RIGA PRECEDENTE. MODIFICA DA METTERE BY DANILO
            'strSQL = strSQL & "(SELECT count(*) from entepersonaleruoli where IDEntePersonale=entepersonaleruoli.IDEntePersonale and entepersonaleruoli.Principale=0) as RuoliSecondari, "
            strSQL = strSQL & "case entepersonaleruoli.Accreditato "
            strSQL = strSQL & "when 1 then 'Iscritto' "
            strSQL = strSQL & "when 0 then 'Da Valutare' "
            strSQL = strSQL & "when -1 then 'Non Chiuso' "
            strSQL = strSQL & "else 'Non Definito' end as Accreditato,  "
            strSQL = strSQL & "'Acquisito' as Tipologia, personaleacquisito.Approvato as Approvato, personaleacquisito.IDPersonaleAcquisito "
            strSQL = strSQL & "FROM entepersonaleruoli "
            strSQL = strSQL & "inner join ruoli on entepersonaleruoli.IDRuolo=ruoli.IDRuolo INNER JOIN personaleacquisito ON entepersonaleruoli.IDEntePersonaleRuolo = personaleacquisito.IDEntePersonaleRuolo "
            strSQL = strSQL & "INNER JOIN entepersonale ON entepersonaleruoli.IDEntePersonale = entepersonale.IDEntePersonale INNER JOIN enti ON entepersonale.IDEnte = enti.IDEnte "
            strSQL = strSQL & "WHERE (personaleacquisito.IDEnteAcquirente=" & IdEnte & ") AND (personaleacquisito.Approvato=3) order by ordina, entepersonale.Datafinevalidità, Nominativo"

            'eseguo la query
            dtsRisRicerca = ClsServer.DataSetGenerico(strSQL, Session("conn"))
            'assegno il dataset alla griglia del risultato
            dtgRisultatoRicerca.DataSource = dtsRisRicerca
            dtgRisultatoRicerca.DataBind()

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
            'NomeColonne(5) = "Competenza"

            NomiCampiColonne(0) = "Nominativo"
            NomiCampiColonne(1) = "Ruolo"
            NomiCampiColonne(2) = "Posizione"
            NomiCampiColonne(3) = "Accreditato"
            NomiCampiColonne(4) = "Tipologia"
            'NomiCampiColonne(5) = "Competenza"
            'carico un datatable che userò poi nella pagina di stampa
            'il numero delle colonne è a base 0
            CaricaDataTablePerStampa(dtsRisRicerca, 4, NomeColonne, NomiCampiColonne)

            '*********************************************************************************

        Catch ex As Exception
            Response.Write(ex.Message.ToString())
        End Try
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
                strSQL = "select IDRuolo,Ruolo from ruoli "
                strSQL = strSQL & "union "
                strSQL = strSQL & "select '0',' Selezionare ' from ruoli order by ruolo"
                'chiudo il datareader se aperto
                If Not dtrRuoloPrincipale Is Nothing Then
                    dtrRuoloPrincipale.Close()
                    dtrRuoloPrincipale = Nothing
                End If
                'eseguo la query
                dtrRuoloPrincipale = ClsServer.CreaDatareader(strSQL, Session("conn"))
                'assegno il datadearder alla combo caricando così descrizione e id
                ddlRuolo.DataSource = dtrRuoloPrincipale
                ddlRuolo.Items.Add("")
                ddlRuolo.DataTextField = "Ruolo"
                ddlRuolo.DataValueField = "IDRuolo"
                ddlRuolo.DataBind()
                'chiudo il datareader se aperto
                If Not dtrRuoloPrincipale Is Nothing Then
                    dtrRuoloPrincipale.Close()
                    dtrRuoloPrincipale = Nothing
                End If
            End If
        Catch ex As Exception
            Response.Write(ex.Message.ToString())
        End Try

    End Sub
    Sub RicercaServizi()
        Dim Strsql As String
        Dim dtsServizi As DataSet
        Strsql = "SELECT sistemi.idsistema,sistemi.sistema,Enti.denominazione,Enti.Codiceregione " & _
        " FROM entisistemi " & _
        " inner join sistemi on sistemi.idsistema=entisistemi.idsistema " & _
        " inner join Enti on enti.idente=entisistemi.idente " & _
        " INNER Join " & _
        " EntiAcquisizioneServizi ON entisistemi.IDEnteSistema = EntiAcquisizioneServizi.idEnteSistema " & _
        " WHERE  Sistemi.Nascosto=0 and  (EntiAcquisizioneServizi.idEnteSecondario = " & CInt(Session("idEnte")) & ")"

        dtsServizi = ClsServer.DataSetGenerico(Strsql, Session("conn"))
        dgServizi.DataSource = dtsServizi
        Session("RicercaServizi") = dtsServizi
        dgServizi.DataBind()
        If dgServizi.Items.Count = 0 Then
            dgServizi.Visible = False
        End If
    End Sub

    Function checkFiltroFase() As Boolean
        lblErroreFiltroFase.Visible = False
        If Not String.IsNullOrEmpty(txtFiltroFase.Text) AndAlso Not Integer.TryParse(txtFiltroFase.Text, Nothing) Then
            lblErroreFiltroFase.Visible = True
            Return False
        End If
        Return True
    End Function

    Sub RicercaPersonaliEnte()
        'variabile stringa per la insert nella base dati
        Dim strSQL As String
        'variabile stringa per ctreazione filtri di ricerca da aggiungere nella query
        Dim strSQLFiltriRicerca As String
        'variabile stringa per ctreazione filtri di ricerca per i ruoli da aggiungere nella query
        Dim strSQLFiltriRicercaRuoli As String
        'adapter che conterrà l'id del tizio inserito
        strSQLFiltriRicerca = ""
        strSQLFiltriRicercaRuoli = ""
        Dim UtenteRegioneCompetenza As String
        Dim strSQLX As String

        If Not checkFiltroFase() Then Exit Sub

        'datareader che conterrà l'la descrizione della regione
        Dim dtrCompetenze As System.Data.SqlClient.SqlDataReader
        strSQLX = "select a.Descrizione from RegioniCompetenze a "
        strSQLX = strSQLX & "INNER JOIN utentiunsc b ON a.idregionecompetenza = b.idregionecompetenza "
        strSQLX = strSQLX & "where b.username = '" & Session("Utente") & "'"

        'controllo se utente o ente regionale
        'eseguo la query
        dtrCompetenze = ClsServer.CreaDatareader(strSQLX, Session("conn"))
        dtrCompetenze.Read()
        If dtrCompetenze.HasRows = True Then
            UtenteRegioneCompetenza = dtrCompetenze("Descrizione")
        End If
        If Not dtrCompetenze Is Nothing Then
            dtrCompetenze.Close()
            dtrCompetenze = Nothing
        End If
        Try
            'controllo i filtri di ricerca da inserire nella query
            '******************************************************************
            If TxtCodEnte.Text <> "" Then
                strSQLFiltriRicerca = strSQLFiltriRicerca & "and (d.CodiceRegione = '" & Replace(TxtCodEnte.Text, "'", "''") & "') "
            End If

            If TxtDenominazione.Text <> "" Then
                strSQLFiltriRicerca = strSQLFiltriRicerca & "and (d.Denominazione like '" & Replace(TxtDenominazione.Text, "'", "''") & "%') "
            End If

            If txtCognome.Text <> "" Then
                strSQLFiltriRicerca = strSQLFiltriRicerca & "and (EntePersonale.Cognome like '" & Replace(txtCognome.Text, "'", "''") & "%') "
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
            If TxtCodiceFiscale.Text <> "" Then
                strSQLFiltriRicerca = strSQLFiltriRicerca & " and EntePersonale.CodiceFiscale = '" & ClsServer.NoApice(TxtCodiceFiscale.Text) & "'"
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

            If DDLesperienza.SelectedValue <> "0" Then
                strSQLFiltriRicerca = strSQLFiltriRicerca & "and (isnull(EntePersonale.EsperienzaServizioCivile,0)=" & DDLesperienza.SelectedValue & ")"
            End If

            If DDLcorso.SelectedValue <> "0" Then
                strSQLFiltriRicerca = strSQLFiltriRicerca & "and (isnull(EntePersonale.Corso,0)=" & DDLcorso.SelectedValue & ")"
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

            'If CboCompetenza.SelectedValue <> 0 Then
            'strSQL = strSQL & " And enti.IdRegioneCompetenza = " & CboCompetenza.SelectedValue
            'End If
            'blocco filtri di ricerca
            '******************************************************************
            'preparo la query per la ricerca delle risorse acquisite e non con l'aggiunta dei filtri
            strSQL = "select 0 ordina, d.Denominazione,d.CodiceRegione as CodReg,regionicompetenze.descrizione as competenza,'" & Replace(UtenteRegioneCompetenza, "'", "''") & "' as UtenteCompetenza, EntePersonale.IDEntePersonale, EntePersonale.IDEnte as IDEnte,Entepersonale.EsperienzaServizioCivile,Entepersonale.Corso, "
            strSQL = strSQL & "EntePersonale.Cognome + ' ' + EntePersonale.Nome as Nominativo,  EntePersonale.Titolo, "
            strSQL = strSQL & "EntePersonale.Posizione, EntePersonale.DataFineValidità, case when EntePersonale.datafinevalidità is null then case when b.datafinevalidità is null then  'Attiva' else 'Cancellata' End	when not EntePersonale.datafinevalidità is null then 'Cancellata' end as Stato, "
            strSQL = strSQL & "ruoli.Ruolo, "
            'strSQL = strSQL & "(SELECT count(*) from entepersonaleruoli where IDEntePersonale=b.IDEntePersonale and Principale=0) as RuoliSecondari, "
            strSQL = strSQL & "case b.Accreditato "
            strSQL = strSQL & "when 1 then 'Iscritto' "
            strSQL = strSQL & "when 0 then 'Da Valutare' "
            strSQL = strSQL & "when -1 then 'Chiuso' "
            strSQL = strSQL & "else 'Non Definito' end as Accreditato, "
            strSQL = strSQL & "'Propria' as Tipologia, '6' as Approvato, NULL as IDPersonaleAcquisito,b.UsernameInseritore,  "
            strSQL = strSQL & "case isnull(EsperienzaServizioCivile,0) "
            strSQL = strSQL & "when 1 then 'Si' "
            strSQL = strSQL & "when 2 then 'No' "
            strSQL = strSQL & "when 0 then '-' "
            strSQL = strSQL & "else 'Non Definito' end as EsperienzaBis, "
            strSQL = strSQL & "case isnull(Corso,0) "
            strSQL = strSQL & "when 1 then 'Fatto' "
            strSQL = strSQL & "when 2 then 'Da Fare' "
            strSQL = strSQL & "when 3 then 'Non Necessario' "
            strSQL = strSQL & "when 0 then '-' "
            strSQL = strSQL & "else 'Non Definito' end as CorsoBis, b.DataInseritore "
            strSQL = strSQL & "from EntePersonale "
            strSQL = strSQL & "inner join entepersonaleruoli as b on EntePersonale.IDEntePersonale=b.IDEntePersonale inner join ruoli on b.IDRuolo=ruoli.IDRuolo "
            strSQL = strSQL & "inner join enti as d on EntePersonale.IDEnte=d.IDEnte  "
            strSQL = strSQL & "inner Join regionicompetenze on d.idregionecompetenza = regionicompetenze.idregionecompetenza "

            'la validità del FiltroFase è stata controllata prima
            If Not String.IsNullOrEmpty(txtFiltroFase.Text) Then
                strSQL = strSQL & " left join EntiFasi_Risorse efr on efr.IdEntePersonaleRuolo=b.IDEntePersonaleRuolo "
                strSQL = strSQL & " left join EntiFasi_Personale efp on efp.IdEntePersonale=entepersonale.IDEntePersonale "
            End If

            strSQL = strSQL & " where'1'='1' "
            'strSQL = strSQL & "and EntePersonale.IDEnte=" & CInt(Session("idEnte")) & " "
            If strSQLFiltriRicerca <> "" Then
                strSQL = strSQL & strSQLFiltriRicerca
            End If
            If CboCompetenza.SelectedValue <> 0 Then
                strSQL = strSQL & " And d.IdRegioneCompetenza = " & CboCompetenza.SelectedValue
            End If
            strSQL = strSQL & " union "
            strSQL = strSQL & "SELECT 1 ordina, d.Denominazione,d.CodiceRegione as CodReg,regionicompetenze.descrizione as competenza,'" & Replace(UtenteRegioneCompetenza, "'", "''") & "' as UtenteCompetenza, entepersonale.IDEntePersonale,  d.IDEnte as IDEnte,Entepersonale.EsperienzaServizioCivile,Entepersonale.Corso, "
            strSQL = strSQL & "entepersonale.Cognome + ' ' + entepersonale.Nome as Nominativo,  entepersonale.Titolo, "
            strSQL = strSQL & "entepersonale.Posizione, entepersonale.DataFineValidità, case when EntePersonale.datafinevalidità is null then case when b.datafinevalidità is null then  'Attiva' else 'Cancellata' End	when not EntePersonale.datafinevalidità is null then 'Cancellata' end as Stato, "
            'strSQL = strSQL & "case entepersonaleruoli.Principale when 1 then ruoli.Ruolo end as Ruolo, "
            strSQL = strSQL & "ruoli.Ruolo as Ruolo, " 'SOSTITUISCE RIGA PRECEDENTE. MODIFICA DA METTERE BY DANILO
            'strSQL = strSQL & "(SELECT count(*) from entepersonaleruoli where IDEntePersonale=entepersonaleruoli.IDEntePersonale and entepersonaleruoli.Principale=0) as RuoliSecondari, "
            'strSQL = strSQL & "NULL as RuoliSecondari, " 'SOSTITUISCE RIGA PRECEDENTE. MODIFICA DA METTERE BY DANILO
            strSQL = strSQL & "case b.Accreditato "
            strSQL = strSQL & "when 1 then 'Iscritto' "
            strSQL = strSQL & "when 0 then 'Da Valutare' "
            strSQL = strSQL & "when -1 then 'Chiuso' "
            strSQL = strSQL & "else 'Non Definito' end as Accreditato,  "
            strSQL = strSQL & "'Acquisito' as Tipologia, personaleacquisito.Approvato as Approvato, personaleacquisito.IDPersonaleAcquisito,b.UsernameInseritore,  "
            strSQL = strSQL & "case isnull(EsperienzaServizioCivile,0) "
            strSQL = strSQL & "when 1 then 'Si' "
            strSQL = strSQL & "when 2 then 'No' "
            strSQL = strSQL & "when 0 then '-' "
            strSQL = strSQL & "else 'Non Definito' end as EsperienzaBis, "
            strSQL = strSQL & "case isnull(Corso,0) "
            strSQL = strSQL & "when 1 then 'Fatto' "
            strSQL = strSQL & "when 2 then 'Da Fare' "
            strSQL = strSQL & "when 3 then 'Non Necessario' "
            strSQL = strSQL & "when 0 then '-' "
            strSQL = strSQL & "else 'Non Definito' end as CorsoBis, b.DataInseritore "
            strSQL = strSQL & "FROM entepersonaleruoli as b "
            strSQL = strSQL & "inner join ruoli on b.IDRuolo=ruoli.IDRuolo INNER JOIN personaleacquisito ON b.IDEntePersonaleRuolo = personaleacquisito.IDEntePersonaleRuolo "
            strSQL = strSQL & "INNER JOIN entepersonale ON b.IDEntePersonale = entepersonale.IDEntePersonale"
            'strSQL = strSQL & "INNER JOIN enti ON entepersonale.IDEnte = enti.IDEnte "
            strSQL = strSQL & " inner join enti as d on EntePersonale.IDEnte=d.IDEnte "
            strSQL = strSQL & "inner Join regionicompetenze on d.idregionecompetenza = regionicompetenze.idregionecompetenza "

            'la validità del FiltroFase è stata controllata prima
            If Not String.IsNullOrEmpty(txtFiltroFase.Text) Then
                strSQL = strSQL & " left join EntiFasi_Risorse efr on efr.IdEntePersonaleRuolo=b.IDEntePersonaleRuolo "
                strSQL = strSQL & " left join EntiFasi_Personale efp on efp.IdEntePersonale=entepersonale.IDEntePersonale "
            End If

            strSQL = strSQL & " where '1'='1' "
            strSQL = strSQL & "And (personaleacquisito.Approvato=3) " 'and getdate() between isnull(entepersonaleruoli.DataInizioValidità,'2000-01-01') and isnull(entepersonaleruoli.DataFineValidità,'2030-01-01')"
            If strSQLFiltriRicercaRuoli <> "" Then
                strSQL = strSQL & strSQLFiltriRicercaRuoli
            Else
                If strSQLFiltriRicerca <> "" Then
                    strSQL = strSQL & strSQLFiltriRicerca
                End If
            End If
            'If CboCompetenza.SelectedValue <> 0 Then
            'strSQL = strSQL & " And enti.IdRegioneCompetenza = " & CboCompetenza.SelectedValue
            'End If
            strSQL = strSQL & " order by ordina, entepersonale.Datafinevalidità, Nominativo"
            'fine preparazione query
            '************************
            'eseguo la query e assegno il dataset alla datagrid del risultato della ricerca assegno 
            dtsRisRicerca = ClsServer.DataSetGenerico(strSQL, Session("conn"))
            dtgRisultatoRicerca.DataSource = dtsRisRicerca
            Session("RisultatoRicerca") = dtsRisRicerca
            dtgRisultatoRicerca.DataBind()
            If dtgRisultatoRicerca.Items.Count > 0 Then
                CmdEsporta.Visible = True

            Else
                CmdEsporta.Visible = False

            End If
            ColoraCelle()

            '*********************************************************************************
            'blocco per la creazione della datatable per la stampa della ricerca

            'nome e posizione di lettura delle colopnne a base 0
            Dim NomeColonne(10) As String
            Dim NomiCampiColonne(10) As String
            'nome della colonna 
            'e posizione nella griglia di lettura
            NomeColonne(0) = "Nominativo"
            NomeColonne(1) = "Ruoli"
            NomeColonne(2) = "Posizione"
            NomeColonne(3) = "Accreditato"
            NomeColonne(4) = "Tipologia"
            NomeColonne(5) = "Stato"
            NomeColonne(6) = "Codice Ente"
            NomeColonne(7) = "Esperienza SC"
            NomeColonne(8) = "Corso"
            NomeColonne(9) = "Competenza"
            NomeColonne(10) = "Data Inserimento"

            NomiCampiColonne(0) = "Nominativo"
            NomiCampiColonne(1) = "Ruolo"
            NomiCampiColonne(2) = "Posizione"
            NomiCampiColonne(3) = "Accreditato"
            NomiCampiColonne(4) = "Tipologia"
            NomiCampiColonne(5) = "Stato"
            NomiCampiColonne(6) = "CodReg"
            NomiCampiColonne(7) = "EsperienzaBis"
            NomiCampiColonne(8) = "CorsoBis"
            NomiCampiColonne(9) = "Competenza"
            NomiCampiColonne(10) = "DataInseritore"
            'carico un datatable che userò poi nella pagina di stampa
            'il numero delle colonne è a base 0
            CaricaDataTablePerStampa(dtsRisRicerca, 10, NomeColonne, NomiCampiColonne)

            '*********************************************************************************

            'controllaChek()
        Catch ex As Exception
            Response.Write(ex.Message.ToString())
        End Try
    End Sub

    Private Sub dtgRisultatoRicerca_PageIndexChanged(ByVal source As System.Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dtgRisultatoRicerca.PageIndexChanged
        If Not checkFiltroFase() Then Exit Sub
        Call RicercaPersonaliEnte()
        dtgRisultatoRicerca.CurrentPageIndex = e.NewPageIndex
        dtgRisultatoRicerca.DataSource = Session("RisultatoRicerca")
        dtgRisultatoRicerca.DataBind()
        dtgRisultatoRicerca.SelectedIndex = -1

        'cambio la pagina riassegnando il dataset dichiarato pubblico a tutt ala pagina 
        'dtgRisultatoRicerca.DataSource = dtsRisRicerca

        
        ColoraCelle()

        'controllaChek()
    End Sub

    'routine per caricare checkbox nella griglia
    'Private Sub controllaChek()
    '    Dim item As DataGridItem
    '    Dim i As Integer
    '    For Each item In dtgRisultatoRicerca.Items
    '        Dim check As CheckBox = DirectCast(item.FindControl("canc"), CheckBox)
    '        Select Case dtgRisultatoRicerca.Items(item.ItemIndex).Cells(8).Text
    '            Case 3
    '                check.Visible = True
    '                check.Checked = False
    '            Case 6
    '                check.Visible = False
    '            Case Else
    '                'Si
    '                check.Visible = False
    '        End Select
    '    Next
    'End Sub

    Private Sub dtgRisultatoRicerca_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dtgRisultatoRicerca.SelectedIndexChanged
        Dim intPersonaleEnteAcquisito As Integer

        'ADC--------------------------------------------------------------------
        If Session("TipoUtente") = "U" Then
            Session("CodiceRegioneEnte") = dtgRisultatoRicerca.SelectedItem.Cells(13).Text
            Session("Denominazione") = dtgRisultatoRicerca.SelectedItem.Cells(10).Text
            Session("IdEnte") = dtgRisultatoRicerca.SelectedItem.Cells(2).Text
            'Response.Redirect("entepersonale.aspx?VengoDaProgetti=" & Request.QueryString("VengoDaProgetti") & "")
            'Response.Redirect("entepersonale.aspx?VengoDaProgetti=" & Request.QueryString("VengoDaProgetti") & "&tipoazione=" & "Modifica" & "")
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
                    Response.Redirect("entepersonale.aspx?VengoDaRicercaTotale=true&VengoDaProgetti=" & Request.QueryString("VengoDaProgetti") & "&intPersonaleEnteAcquisito=" & intPersonaleEnteAcquisito & "&intPersonaleEnteAssociato=" & CInt(dtgRisultatoRicerca.SelectedItem.Cells(1).Text) & "&intEnteAssociato=" & CInt(dtgRisultatoRicerca.SelectedItem.Cells(2).Text) & "&Denominazione=" & dtgRisultatoRicerca.SelectedItem.Cells(10).Text & "&Acquisito=" & "Vero" & "&tipoazione=" & "Modifica" & "")
                Else
                    Response.Redirect("entepersonale.aspx?VengoDaRicercaTotale=true&VengoDaProgetti=" & Request.QueryString("VengoDaProgetti") & "&intPersonaleEnteAcquisito=" & intPersonaleEnteAcquisito & "&intPersonaleEnteAssociato=" & CInt(dtgRisultatoRicerca.SelectedItem.Cells(1).Text) & "&intEnteAssociato=" & CInt(dtgRisultatoRicerca.SelectedItem.Cells(2).Text) & "&Acquisito=" & "Falso" & "&tipoazione=" & "Modifica" & "")
                End If
            Else
                Response.Redirect("entepersonale.aspx?VengoDaRicercaTotale=true&VengoDaProgetti=" & Request.QueryString("VengoDaProgetti") & "&intPersonaleEnteAcquisito=" & intPersonaleEnteAcquisito & "&intPersonaleEnteAssociato=" & CInt(dtgRisultatoRicerca.SelectedItem.Cells(1).Text) & "&intEnteAssociato=" & CInt(dtgRisultatoRicerca.SelectedItem.Cells(2).Text) & "&Denominazione=" & dtgRisultatoRicerca.SelectedItem.Cells(10).Text & "&Cancellato=" & "Vero" & "&tipoazione=" & "Modifica" & "")
            End If
        Else
            '-----------------------------------------------------------------------------------------------------------------
            'controllo se è selezionato un item nella griglia
            If Not dtgRisultatoRicerca.SelectedItem Is Nothing Then
                If dtgRisultatoRicerca.SelectedItem.Cells(16).Text = dtgRisultatoRicerca.SelectedItem.Cells(17).Text Then
                    Session("CodiceRegioneEnte") = dtgRisultatoRicerca.SelectedItem.Cells(13).Text
                    Session("Denominazione") = dtgRisultatoRicerca.SelectedItem.Cells(10).Text
                    Session("IdEnte") = dtgRisultatoRicerca.SelectedItem.Cells(2).Text
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
                    If dtgRisultatoRicerca.SelectedItem.Cells(16).Text = dtgRisultatoRicerca.SelectedItem.Cells(17).Text Then 'competenza e' uguale a competenza di chi si e' loggato allora

                        If dtgRisultatoRicerca.SelectedItem.Cells(11).Text = "Attiva" Then
                            'controllo e faccio il redirect alla pagina di modifica postando il vlore Acquisito
                            'che prendi vero o falso
                            If dtgRisultatoRicerca.SelectedItem.Cells(7).Text = "Acquisito" Then
                                Response.Redirect("entepersonale.aspx?VengoDaRicercaTotale=true&VengoDaProgetti=" & Request.QueryString("VengoDaProgetti") & "&intPersonaleEnteAcquisito=" & intPersonaleEnteAcquisito & "&intPersonaleEnteAssociato=" & CInt(dtgRisultatoRicerca.SelectedItem.Cells(1).Text) & "&intEnteAssociato=" & CInt(dtgRisultatoRicerca.SelectedItem.Cells(2).Text) & "&Denominazione=" & dtgRisultatoRicerca.SelectedItem.Cells(10).Text & "&Acquisito=" & "Vero" & "&tipoazione=" & "Modifica" & "")
                            Else
                                Response.Redirect("entepersonale.aspx?VengoDaRicercaTotale=true&VengoDaProgetti=" & Request.QueryString("VengoDaProgetti") & "&intPersonaleEnteAcquisito=" & intPersonaleEnteAcquisito & "&intPersonaleEnteAssociato=" & CInt(dtgRisultatoRicerca.SelectedItem.Cells(1).Text) & "&intEnteAssociato=" & CInt(dtgRisultatoRicerca.SelectedItem.Cells(2).Text) & "&Acquisito=" & "Falso" & "&tipoazione=" & "Modifica" & "")
                            End If
                        Else
                            Response.Redirect("entepersonale.aspx?VengoDaRicercaTotale=true&VengoDaProgetti=" & Request.QueryString("VengoDaProgetti") & "&intPersonaleEnteAcquisito=" & intPersonaleEnteAcquisito & "&intPersonaleEnteAssociato=" & CInt(dtgRisultatoRicerca.SelectedItem.Cells(1).Text) & "&intEnteAssociato=" & CInt(dtgRisultatoRicerca.SelectedItem.Cells(2).Text) & "&Denominazione=" & dtgRisultatoRicerca.SelectedItem.Cells(10).Text & "&Cancellato=" & "Vero" & "&tipoazione=" & "Modifica" & "")
                        End If
                        Exit Sub
                    End If
                Else
                    'nel caso in cui la regione di comptetenza della risorsa è diversa da quella dell'utente 
                    'mostro il messaggio informativo all'utente
                    lblmessaggiosopra.Visible = True
                    lblmessaggiosopra.Text = "Attenzione. La risorsa non è di propria competenza."
                    lblmessaggiosopra.ForeColor = Color.Red
                End If
            Else
                Exit Sub
            End If
        End If
    End Sub

    
    Protected Sub cmdChiudi_Click(sender As Object, e As EventArgs) Handles cmdChiudi.Click
        Response.Redirect("WfrmMain.aspx")
    End Sub

    Protected Sub cmdRicerca_Click(sender As Object, e As EventArgs) Handles cmdRicerca.Click
        dtgRisultatoRicerca.CurrentPageIndex = 0
        'RicercaPersonaliEnte(CInt(Session("idEnte")))
        RicercaPersonaliEnte()
        RicercaServizi()
    End Sub

    Protected Sub CmdEsporta_Click(sender As Object, e As EventArgs) Handles CmdEsporta.Click

        CmdEsporta.Visible = False
        StampaCSV(Session("DtbRicerca"))

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
            ApriCSV.Visible = False
            'CmdEsportaElenco.Visible = False
        Else
            xPrefissoNome = Session("Utente")
            NomeUnivoco = xPrefissoNome & "ExpElencoRisorse" & Year(Now) & Month(Now) & Day(Now) & Hour(Now) & Minute(Now) & Second(Now)
            Writer = New StreamWriter(Server.MapPath("download") & "\" & NomeUnivoco & ".CSV")
            'Creazione dell'inntestazione del CSV
            Dim intNumCol As Int64 = DTBRicerca.Columns.Count
            For i = 0 To intNumCol - 3
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

            ApriCSV.Visible = True
            ApriCSV.NavigateUrl = "download\" & NomeUnivoco & ".CSV"

            Writer.Close()
            Writer = Nothing

        End If

    End Function
End Class