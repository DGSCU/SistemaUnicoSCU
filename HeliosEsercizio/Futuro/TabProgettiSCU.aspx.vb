Imports System.Web.UI.WebControls
Imports System.Data.SqlClient
Imports System.Drawing
Imports Logger.Data
Public Class TabProgettiSCU
    Inherits SmartPage

#Region " Dichiarazione Variabii"
    'variabile che utilizzerò per avere sempre vivo nella routine l'id dell'attivitàcaratteristicaorgnaizzativa
    Dim intIDAttivitaCaratteristicaOrganizzativa As Integer
    'variabile che utilizzerò per avere sempre vivo nella routine l'id dell'attivitàcaratteristicaconoscenza
    Dim intIDAttivitaCaratteristicaConoscenza As Integer
    'variabile che utilizzerò per avere sempre vivo nella routine l'id dell'attivitàformazionegenerale
    Dim intIDAttivitaFormazioneGenerale As Integer
    'variabile che utilizzer per avere sempre vivo nella routine l'id dell'attivitàformazionegenerale
    Dim intIDAttivitaFormazioneSpecifica As Integer
    'variabile che utilizzer per avere sempre vivo nella routine l'id dell'attivitàaltroformazione
    Dim intIDAttivitaAltroFormazione As Integer
    'variabile che utilizzer per avere sempre vivo nella routine l'id dell'ambitoattività
    Dim intIDAmbitoAttivita As Integer
    Dim dtrgenerico As SqlClient.SqlDataReader
    Dim errore As String
    Dim VALORE_TRUE As String = "True"
    Dim VALORE_FALSE As String = "False"
    Dim PROGETTO_GARANZIA_GIOVANI As String = "4"
#End Region

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

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load
        'effettua il redirect alla pagina di Login nel caso al sessione sia invalida
        checkSpid(True)
        VerificaSessione()
        If Request.QueryString("IdAttivita") <> "" Then

            Dim strATTIVITA As Integer = -1
            Dim strBANDOATTIVITA As Integer = -1
            Dim strENTEPERSONALE As Integer = -1
            Dim strENTITA As Integer = -1
            Dim strIDENTE As Integer = -1

            If ClsUtility.SICUREZZA_VERIFICA_AUTORIZZAZIONI(Session("conn"), Session("IdEnte"), Session("txtCodEnte"), Request.QueryString("IdAttivita"), strBANDOATTIVITA, strENTEPERSONALE, strENTITA, strIDENTE) = 1 Then

                If Not dtrgenerico Is Nothing Then
                    dtrgenerico.Close()
                    dtrgenerico = Nothing
                End If
            Else
                If Not dtrgenerico Is Nothing Then
                    dtrgenerico.Close()
                    dtrgenerico = Nothing
                End If
                Response.Redirect("wfrmAnomaliaDati.aspx")

            End If
            txtIdAreaPrincipale.Value = txtIdAmbitoAttivita.Value
            txtIdAltro.Value = txtIdAttivitaAltroFormazione.Value
            txtIdAttOrganizzativa.Value = txtIdAttivitaCaratteristicaOrganizzativa.Value
            txtIdFormSpecifica.Value = txtIdAttivitaFormazioneSpecifica.Value
            txtCodAtt.Value = txtCodAttivita.Value
            txtIdMacroAm.Value = strIdMacroAmbito.Value
            txtIdAree.Value = strIdAmbitiModifica.Value
            If Session("TipoUtente") = "E" Then
                Dim abilitato As Integer
                abilitato = ClsUtility.LoadProgettiAbilitaModificaEnte(Request.QueryString("IdAttivita"), Session("Conn"))
                chkVisualizzazione.Value = IIf(abilitato = "0", VALORE_TRUE, VALORE_FALSE)
            Else
                If (Request.QueryString("Modifica") <> String.Empty) Then
                    chkVisualizzazione.Value = IIf(Request.QueryString("Modifica") = "0", VALORE_TRUE, VALORE_FALSE)
                End If
            End If


            If chkVisualizzazione.Value = VALORE_TRUE Then
                ddlSettore.Enabled = False
                ddlArea.Enabled = False
                imgArea.Visible = False
                txtDataInizioPrevista.Enabled = False
            End If

        End If
        If Page.IsPostBack = False Then
            Dim utility As New ClsUtility()
            Dim IdTipologiaProgetto As String
            If Request.QueryString("IdAttivita") <> "" Then
                IdTipologiaProgetto = utility.TipologiaProgettoDaIdAttivita(Request.QueryString("IdAttivita"), Session("conn"))
                If (IdTipologiaProgetto = PROGETTO_GARANZIA_GIOVANI) Then
                    imgDett.Visible = False
                End If
            End If

            ConfiguraPostBackSezioni()
            If Request.QueryString("popup") = "1" Then
                If Session("TipoUtente") = "E" Then
                    imgSediCertificate.Visible = False
                Else
                    imgSediCertificate.Visible = False
                End If
            End If


            If Request.QueryString("Nazionale") <> "3" Then
                DivAltriAmbiti.Visible = True
            Else
                DivAltriAmbiti.Visible = False
            End If

            If Request.QueryString("Nazionale") <> "2" And Request.QueryString("Nazionale") <> "6" Then
                DivSezioneEstero.Visible = False
                lblSezioneEsteroNonPresente.Visible = True
            Else
                DivSezioneEstero.Visible = True
                lblSezioneEsteroNonPresente.Visible = False
            End If
            If Request.QueryString("Nazionale") = 6 Then
                DivEsteroTutoraggio.Visible = False
            Else
                DivEsteroTutoraggio.Visible = True
            End If
            If (Session("TipoUtente") = "U" Or Session("TipoUtente") = "R") And Not Request.QueryString("IdAttivita") Is Nothing Then
                fieldSetPostiRichiesti.Visible = True
            Else
                fieldSetPostiRichiesti.Visible = False
            End If

            If Request.QueryString("IdAttivita") = "" Then
                CaricaComboSettori()
                CaricaComboAreaPrimaVolta()

            Else
                CaricaComboSettori()
            End If
            CaricaComboDurata()
            'controllo la competenza dell'ente
            'se diverso da nazionale, quindi regionale
            'faccio vedere la check CoProgetto
            Dim strsql As String
            Dim dtrLocale As SqlClient.SqlDataReader
            Dim localcommand As New System.Data.SqlClient.SqlCommand

            localcommand.Connection = Session("conn")
            'vado a controlare se posso cancellare l'attività che sto caricando
            strsql = "SELECT RegioniCompetenze.CodiceRegioneCompetenza AS CodiceRegioneCompetenza "
            strsql = strsql & "FROM enti "
            strsql = strsql & "INNER JOIN RegioniCompetenze ON enti.IdRegioneCompetenza = RegioniCompetenze.IdRegioneCompetenza "
            strsql = strsql & "WHERE enti.idente='" & Session("IdEnte") & "'"
            ChiudiDataReader(dtrLocale)
            localcommand.CommandText = strsql
            dtrLocale = localcommand.ExecuteReader
            If dtrLocale.HasRows = True Then
                dtrLocale.Read()
                If dtrLocale("CodiceRegioneCompetenza") <> "NAZ" Then
                    If Request.QueryString("Nazionale") <> 2 Then
                        chkCoProgettato.Visible = True
                        imgCoProgettazione.Visible = True
                        divCoProgettazione.Visible = True
                    Else
                        chkCoProgettato.Visible = False
                        imgCoProgettazione.Visible = False
                        divCoProgettazione.Visible = False
                    End If
                Else
                    chkCoProgettato.Visible = False
                    imgCoProgettazione.Visible = False
                    divCoProgettazione.Visible = False
                End If
            End If
            ChiudiDataReader(dtrLocale)

 
            'controllo se si tratta di un ente 
            'se si tratta di un ente controllo se si tratta di un ente capofila
            'se si tratta di un ente capo fila permetto la modifica
            If Request.QueryString("CheckMess") = VALORE_TRUE Then
                lblMessaggioConferma.Text = "Progetto inserito con successo."
            End If
            If Not Request.QueryString("IdAttivita") Is Nothing Then
                '1-verifica se l'utente ha le abilitazioni per fare la revisione progetto
                '2-verifica che il progetto corrente non ha già una revisione
                '3-verifica che il bando del progetto corrente abbia associato un bando revisione con stato aperto
                If Not Request.QueryString("sMsg") Is Nothing Then
                    lblerrore.Text = Request.QueryString("sMsg")
                End If

                'controllo se ho effettuato il ricorso e se è andato a buon fine
                If Not Request.QueryString("MsgRicorso") Is Nothing Then
                    lblerrore.Text = Request.QueryString("MsgRicorso")
                    imgRicorso.Visible = False
                End If

                Call AbilitaRevisioneProgetto(CInt(Request.QueryString("IdAttivita")))
                If AbilitaDuplicaProgetto(CInt(Request.QueryString("IdAttivita")), Request.QueryString("Nazionale")) = False Then
                    ImgDuplica.Visible = False
                Else
                    ImgDuplica.Visible = False '29/09/2014 sempre invisibile(il duplica veniva usato per i progetti di GG)
                End If
                'richiamo la routine che controlla se il progetto in questione 
                'può esser duplicato per il ricorso
                Call AbilitaRicorsoProgetto(CInt(Request.QueryString("IdAttivita")))
                Dim Visualizzazione As Integer
                If Session("TipoUtente") = "E" Then
                    'Dim abilitato As Integer
                    Visualizzazione = ClsUtility.LoadProgettiAbilitaModificaEnte(Request.QueryString("IdAttivita"), Session("Conn"))
                    'Visualizzazione = IIf(abilitato = 1, 0, 1)
                Else
                    Visualizzazione = CInt(Request.QueryString("Modifica"))
                End If
                CaricaProgetto(CInt(Request.QueryString("IdAttivita")), Visualizzazione)
                If Session("TipoUtente") = "U" Or Session("TipoUtente") = "R" Then
                    If CheckStatoProgetto(Request.QueryString("IdAttivita")) = True Then
                        imgCollegaIstanza.Visible = True
                    End If
                    'If Session("TipoUtente") = "R" Then
                    '    ImgStampa.Visible = False
                    'End If

                Else
                    ImgStampa.Visible = False
                    'disabilio il tipo finanziomento per lgi enti
                    OptFinStatale.Enabled = False
                    OptFinRegionale.Enabled = False
                    OptFinPrivato.Enabled = False
                End If
                If Request.QueryString("Nazionale") <> "3" Then
                    imgDisabili.Visible = False
                End If

                'gestione visualizzazione fascicolo
                If (Session("TipoUtente") = "U" And TxtCodiceFascicolo.Text <> String.Empty) Then
                    IdDivRigaFascicolo.Visible = True
                Else
                    IdDivRigaFascicolo.Visible = False
                End If

            Else
                imgCoProgettazione.Visible = False
                imgSediProgetto.Visible = False
                imgRisorse.Visible = False
                imgSediAssegnazione.Visible = False
                imgVolontari.Visible = False
                imgDett.Visible = False
                imgDisabili.Visible = False
                ImgStampa.Visible = False
                OptFinStatale.Enabled = False
                OptFinRegionale.Enabled = False
                OptFinPrivato.Enabled = False
                OptFinStatale.Checked = True
                OptFinRegionale.Checked = False
                OptFinPrivato.Checked = False
                imgElencoDocumentiProg.Visible = False
                Dim strLocal As String
                Dim dtrLocal As SqlClient.SqlDataReader
                Dim myCommand As New System.Data.SqlClient.SqlCommand
                myCommand.Connection = Session("conn")
                'vado a controlare se posso cancellare l'attività che sto caricando
                strLocal = "select a.Attiva from StatiAttività as a "
                strLocal = strLocal & "inner join attività as b on a.IDStatoAttività=b.IDStatoAttività "
                strLocal = strLocal & "where b.idattività=" & CInt(Request.QueryString("IdAttivita"))
                ChiudiDataReader(dtrLocal)
                myCommand.CommandText = strLocal
                dtrLocal = myCommand.ExecuteReader
                dtrLocal.Read()
                If dtrLocal.HasRows = True Then
                    If dtrLocal("Attiva") = False Then
                        imgVolontari.Visible = False
                    End If
                End If
                ChiudiDataReader(dtrLocal)
            End If

            If ClsUtility.ForzaStatoValutazione(Session("Utente"), Session("conn")) Then
                'LblStatoValutazione.Visible = True
                'ddlStatoValutazione.Visible = True
                'ImgApplicaStatoValutazione.Visible = True
                DivStatoValutazione.Visible = True
            End If
            CostruisciTitolo()

        End If
    End Sub

    Private Sub cmdSalva_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles cmdSalva.Click
        ReimpostaLabelMessaggi()
        If (ValidaCampi()) Then
            'stringa per la query
            Dim strCheckProgetto As String
            'datareader locale per leggere i dati
            Dim dtrLocal As SqlClient.SqlDataReader

            If Not Request.QueryString("IdAttivita") Is Nothing Then
                strCheckProgetto = "select Titolo, IdEntePresentante,idattivitàRevisionata from attività where Titolo='" & Replace((txtTitolo.Text), "'", "''") & "' and IdEntePresentante=" & CInt(Session("IdEnte")) & " and IdAttività<>" & CInt(Request.QueryString("IdAttivita"))
            Else
                strCheckProgetto = "select Titolo, IdEntePresentante,idattivitàRevisionata from attività where Titolo='" & Replace((txtTitolo.Text), "'", "''") & "' and IdEntePresentante=" & CInt(Session("IdEnte"))
            End If

            ChiudiDataReader(dtrLocal)
            dtrLocal = ClsServer.CreaDatareader(strCheckProgetto, Session("conn"))
            dtrLocal.Read()

            'se trovo qualcosa blocco l'inserimento

            If dtrLocal.HasRows = True Then
                If IsDBNull(dtrLocal("idattivitàRevisionata")) = True Then
                    lblerrore.Text = "Il Progetto risulta essere già inserito. Verificare il Titolo."
                    ChiudiDataReader(dtrLocal)
                    Exit Sub
                Else
                End If
            End If
            ChiudiDataReader(dtrLocal)

            If Not Request.QueryString("IdAttivita") Is Nothing Then
                'modifica
                Salva(CInt(Request.QueryString("IdAttivita")))
                'stampo a pagina tutte le hidden che ho utilizzato al primo load della pagina
                'così mi porto dietro i valori
                strIdAmbitiModifica.Value = txtIdAree.Value
                txtIdAttivitaCaratteristicaOrganizzativa.Value = txtIdAttOrganizzativa.Value
                txtIdAttivitaFormazioneSpecifica.Value = txtIdFormSpecifica.Value
                txtIdAttivitaAltroFormazione.Value = txtIdAltro.Value
                txtCodAttivita.Value = txtCodAtt.Value
                strIdMacroAmbito.Value = Request.Form("strIdMacroAttivita")
                chkVisualizzazione.Value = VALORE_FALSE
            Else
                Dim intMaxIdInserimento As Integer
                'inserimento
                intMaxIdInserimento = Salva(0)
                Response.Redirect("TabProgettiSCU.aspx?CheckMess=True&Nazionale=" & Request.QueryString("Nazionale") & "&Modifica=" & "1" & "&IdAttivita=" & intMaxIdInserimento & "")
            End If
        End If

    End Sub

    Sub CaricaProgetto(ByVal IdAttivita As Integer, ByVal Visualizzazione As Integer)
        'stringa per la query
        Dim strCaricaProgetto As String
        Dim blnStatoBando As Boolean
        Dim blnStatoAttività As Boolean
        'datareader locale per leggere i dati
        Dim dtrLocal As SqlClient.SqlDataReader
        'command che eseguirà le query e le insert
        Dim myCommand As New System.Data.SqlClient.SqlCommand

        'chiudo le operazioni sugli ambiti 
        ReDim Session("VOperazioniAmbiti")(0)
        Session("VOperazioniAmbiti")(0) = ""


        Dim strappocompetenza As String = "0"
        'inizializzo la connessione al command
        myCommand.Connection = Session("conn")
        'vado a controlare se posso cancellare l'attività che sto caricando
        strCaricaProgetto = "select a.DefaultStato, a.Cancellata, isnull (b.idregionecompetenza,0) as idregionecompetenza,a.IDStatoAttività  from StatiAttività as a "
        strCaricaProgetto = strCaricaProgetto & "inner join attività as b on a.IDStatoAttività=b.IDStatoAttività "
        strCaricaProgetto = strCaricaProgetto & "where b.idattività=" & IdAttivita

        'controllo e chiudo se aperto il datareader
        If Not dtrLocal Is Nothing Then
            dtrLocal.Close()
            dtrLocal = Nothing
        End If
        'eseguo la query
        myCommand.CommandText = strCaricaProgetto
        dtrLocal = myCommand.ExecuteReader
        'leggo il datareader
        dtrLocal.Read()
        'se ci sono dei record
        If dtrLocal.HasRows = True Then
            If dtrLocal("DefaultStato") = True Then
                imgCancella.Visible = True
            End If
            If Not (dtrLocal("IDStatoAttività") = 1 Or dtrLocal("IDStatoAttività") = 2) Then
                imgVolontari.Visible = False
                imgDett.Visible = False
            End If
            'If dtrLocal("Cancellata") = True Then
            '    imgRipristina.Visible = True
            'End If
            strappocompetenza = (dtrLocal("idregionecompetenza"))
        End If
        'controllo e chiudo se aperto il datareader
        If Not dtrLocal Is Nothing Then
            dtrLocal.Close()
            dtrLocal = Nothing
        End If

        If ClsUtility.ForzaFascicoloInformaticoProgetti(Session("Utente"), Session("conn")) = True And strappocompetenza = "22" Then
            imgAssociaDocumentiProg.Visible = True
        End If
        imgElencoDocumentiProg.Visible = True

        strCaricaProgetto = "select a.IDAttività, a.Titolo, a.CodiceEnte, a.IDEntePresentante, ISNULL(A.DATAINIZIOATTIVITà,A.DATAINIZIOPREVISTA) AS DataInizioPrevista, a.NumeroPostiNoVittoNoAlloggio, "
        strCaricaProgetto = strCaricaProgetto & "a.NumeroPostiVittoAlloggio, a.NumeroPostiVitto, a.IDAmbitoAttività,a.NumeroPostiNoVittoNoAlloggioRic,a.NumeroPostiVittoAlloggioRic, a.NumeroPostiVittoRic, "
        strCaricaProgetto = strCaricaProgetto & "a.DescrizioneContestoTerritoriale, a.DescrizioneContestoPolitico, a.Obiettivi, a.Descrizione, "
        strCaricaProgetto = strCaricaProgetto & "a.OreSettimanali, a.MonteOreAnnuo, a.GiorniServizioSettimanali,a.RiduzioneEnte, "
        strCaricaProgetto = strCaricaProgetto & "a.ObblighiVolontari, a.CoProgettazione,a.idAttivitàRevisionata, b.IdAttivitàCaratteristicheOrganizzative, b.Tutor, b.AltreModalitàPubblicizzazione, "
        strCaricaProgetto = strCaricaProgetto & "b.CriteriSelezione, b.SistemaSelezioneAccreditato, b.PianoMonitoraggio, b.SistemaMonitoraggioAccreditato, "
        strCaricaProgetto = strCaricaProgetto & "b.CondizioniRischio, b.LivelliSicurezza, b.CondizioniDisagio, b.ComunicazioneAutorità, "
        strCaricaProgetto = strCaricaProgetto & "b.CollegamentoSedeItaliana, b.ModalitàRientri, b.AssicurazioneIntegrativa, "
        strCaricaProgetto = strCaricaProgetto & "b.RequisitiVolontari, b.RisorseNecessarie, b.CodiceEnteCriteriSelezione, b.CodiceEntePianoMonitoraggio, b.OrePromozioneSensibilizzazione, c.IdAttivitàCaratteristicheConoscenze, c.CreditiFormativi, "
        strCaricaProgetto = strCaricaProgetto & "c.Tirocini, c.CompetenzeAcquisibili, d.IdAttivitàFormazioneGenerale, d.SedeFormazioneGenerale, "
        strCaricaProgetto = strCaricaProgetto & "d.ModalitàFormazioneGenerale, d.SistemaFormazioneAccreditato, d.DurataFormazioneGenerale, d.CodiceEnteModalitàFormazioneGenerale,d.TipoFormazioneGenerale,d.DurataFormazioneGenerale1,d.DurataFormazioneGenerale2, "
        strCaricaProgetto = strCaricaProgetto & "e.IdAttivitàAltroFormazione, e.ModalitàMonitoraggioFormazione, e.MonitoraggioFormazionePresentato, "
        strCaricaProgetto = strCaricaProgetto & "f.IdAttivitàFormazioneSpecifica, f.SedeFormazioneSpecifica, f.ModalitàFormazioneSpecifica,f.TipoFormazioneSpecifica,f.DurataFormazioneSpecifica1,DurataFormazioneSpecifica2, "
        strCaricaProgetto = strCaricaProgetto & "f.CompetenzeFormatori, f.Durata, f.CodiceEnteModalitàFormazioneSpecifica, g.IDMacroAmbitoAttività, (convert(varchar(10),g.IdAmbitoAttività) + '|' + g.Codifica + ' - ' + g.AmbitoAttività) as IDCod, "
        strCaricaProgetto = strCaricaProgetto & "l.DefaultStato as StatoAttività, isnull(i.DefaultStato,1) as StatoBandoAttività,  "
        strCaricaProgetto = strCaricaProgetto & "a.TipoFinanziamento,l.attiva,l.davalutare,l.dagraduare, a.IDFascicoloPC , a.DescrFascicoloPC , a.CodiceFascicoloPC, isnull(a.StatoValutazione,0) as StatoValutazione, "
        strCaricaProgetto = strCaricaProgetto & "a.NMesi, a.GiovaniMinoriOpportunità, a.NumeroGiovaniMinoriOpportunità, a.EsteroUE, a.Tutoraggio, a.NMesiEsteroUE, a.NMesiTutoraggio "
        strCaricaProgetto = strCaricaProgetto & "from attività as a "
        strCaricaProgetto = strCaricaProgetto & "left join AttivitàCaratteristicheOrganizzative as b on a.IDAttività=b.IdAttività "
        strCaricaProgetto = strCaricaProgetto & "left join AttivitàCaratteristicheConoscenze as c  on a.IDAttività=c.IdAttività "
        strCaricaProgetto = strCaricaProgetto & "left join AttivitàFormazioneGenerale as d  on a.IDAttività=d.IdAttività "
        strCaricaProgetto = strCaricaProgetto & "left join AttivitàAltroFormazione as e  on a.IDAttività=e.IdAttività "
        strCaricaProgetto = strCaricaProgetto & "left join AttivitàFormazioneSpecifica as f  on a.IDAttività=f.IdAttività "
        strCaricaProgetto = strCaricaProgetto & "left join BandiAttività as h  on a.IDBandoAttività=h.IdBandoAttività "
        strCaricaProgetto = strCaricaProgetto & "left join StatiBandiAttività as i  on i.IDStatoBandoAttività=h.IdStatoBandoAttività "
        strCaricaProgetto = strCaricaProgetto & "left join StatiAttività as l  on l.IDStatoAttività=a.IdStatoAttività "
        strCaricaProgetto = strCaricaProgetto & "inner join ambitiattività as g on a.IDAmbitoAttività=g.IdAmbitoAttività "
        strCaricaProgetto = strCaricaProgetto & "where a.IDAttività=" & CInt(Request.QueryString("IdAttivita"))

        'controllo e chiudo se aperto il datareader
        If Not dtrLocal Is Nothing Then
            dtrLocal.Close()
            dtrLocal = Nothing
        End If
        'eseguo la query
        myCommand.CommandText = strCaricaProgetto
        dtrLocal = myCommand.ExecuteReader
        'leggo il datareader
        dtrLocal.Read()
        'se ci sono dei record
        If dtrLocal.HasRows = True Then
            '**********************************************************************************
            'passo alle variabili locali gli id necessari a caricare i dati successivamente
            'strCaricaProgetto = strCaricaProgetto & "l.DefaultStato as StatoAttività, isnull(i.DefaultStato,1) as StatoBandoAttività  "
            If IsDBNull(dtrLocal("StatoAttività")) = False Then
                blnStatoAttività = dtrLocal("StatoAttività")
            End If
            If IsDBNull(dtrLocal("StatoBandoAttività")) = False Then
                blnStatoBando = dtrLocal("StatoBandoAttività")
            End If
            If IsDBNull(dtrLocal("IdAttivitàCaratteristicheOrganizzative")) = False Then
                'stampo a pagina l'id dell'attivitàcaratteristica organizzativa
                txtIdAttivitaCaratteristicaOrganizzativa.Value = dtrLocal("IdAttivitàCaratteristicheOrganizzative")
                intIDAttivitaCaratteristicaOrganizzativa = CInt(dtrLocal("IdAttivitàCaratteristicheOrganizzative"))
            Else
                txtIdAttivitaCaratteristicaOrganizzativa.Value = "0"
                intIDAttivitaCaratteristicaOrganizzativa = 0
            End If
            If IsDBNull(dtrLocal("IdAttivitàAltroFormazione")) = False Then
                'stampo a pagina l'id dell'attivitàaltroformazione
                txtIdAttivitaAltroFormazione.Value = dtrLocal("IdAttivitàAltroFormazione")
                intIDAttivitaAltroFormazione = CInt(dtrLocal("IdAttivitàAltroFormazione"))
            Else
                txtIdAttivitaAltroFormazione.Value = "0"
                intIDAttivitaAltroFormazione = 0
            End If
            If IsDBNull(dtrLocal("IdAttivitàFormazioneSpecifica")) = False Then
                'stampo a pagina l'id dell'attivitàformazionespecifica
                txtIdAttivitaFormazioneSpecifica.Value = dtrLocal("IdAttivitàFormazioneSpecifica")
                intIDAttivitaFormazioneSpecifica = CInt(dtrLocal("IdAttivitàFormazioneSpecifica"))
            Else
                txtIdAttivitaFormazioneSpecifica.Value = "0"
                intIDAttivitaFormazioneSpecifica = 0
            End If
            If IsDBNull(dtrLocal("IDAmbitoAttività")) = False Then
                'stampo a pagina l'id dell'ambitoattività
                txtCodAttivita.Value = dtrLocal("IDCod")
                txtIdAmbitoAttivita.Value = dtrLocal("IDAmbitoAttività")
                intIDAmbitoAttivita = CInt(dtrLocal("IDAmbitoAttività"))
            Else
                'nel caso in cui non ci fosse id ambitoattività
                txtIdAmbitoAttivita.Value = "0"
                intIDAmbitoAttivita = 0
            End If

            If IsDBNull(dtrLocal("CondizioniRischio")) = False Then
                Select Case dtrLocal("CondizioniRischio")
                    Case 0
                        ddlCondizioniRischio.SelectedValue = 0
                    Case 1
                        ddlCondizioniRischio.SelectedValue = 1
                End Select
            End If

            If IsDBNull(dtrLocal("LivelliSicurezza")) = False Then
                Select Case dtrLocal("LivelliSicurezza")
                    Case 0
                        ddlLivelliSicurezza.SelectedValue = 0
                    Case 1
                        ddlLivelliSicurezza.SelectedValue = 1
                End Select
            End If

            If IsDBNull(dtrLocal("CondizioniDisagio")) = False Then
                Select Case dtrLocal("CondizioniDisagio")
                    Case 0
                        ddlCondizioniDisagio.SelectedValue = 0
                    Case 1
                        ddlCondizioniDisagio.SelectedValue = 1
                End Select
            End If

            If IsDBNull(dtrLocal("AssicurazioneIntegrativa")) = False Then
                Select Case dtrLocal("AssicurazioneIntegrativa")
                    Case 0
                        ddlEventualeAssicurazione.SelectedValue = 0
                    Case 1
                        ddlEventualeAssicurazione.SelectedValue = 1
                End Select
            End If

            '**********************************************************************************
            If IsDBNull(dtrLocal("Titolo")) = False Then
                txtTitolo.Text = dtrLocal("Titolo")
            Else
                txtTitolo.Text = ""
            End If
            If IsDBNull(dtrLocal("CodiceEnte")) = False Then
                txtCodiceEnte.Text = dtrLocal("CodiceEnte")
            Else
                txtCodiceEnte.Text = ""
            End If

            If IsDBNull(dtrLocal("DataInizioPrevista")) = False Then
                txtDataInizioPrevista.Text = dtrLocal("DataInizioPrevista")
            Else
                txtDataInizioPrevista.Text = ""
            End If

            If IsDBNull(dtrLocal("OreSettimanali")) = False Then
                CheckMonteOreTipo.SelectedValue = "0"
                txtOreServizioSettimanali.Text = dtrLocal("OreSettimanali")
            Else
                txtOreServizioSettimanali.Text = ""
            End If
            If IsDBNull(dtrLocal("MonteOreAnnuo")) = False Then
                'txtMonteOreAnnuo.Enabled = True
                CheckMonteOreTipo.SelectedValue = "1"
                txtMonteOreAnnuo.Text = dtrLocal("MonteOreAnnuo")
            Else
                txtMonteOreAnnuo.Text = ""
            End If
            If IsDBNull(dtrLocal("GiorniServizioSettimanali")) = False Then
                txtNumGiorniServizio.Text = dtrLocal("GiorniServizioSettimanali")
            Else
                txtNumGiorniServizio.Text = ""
            End If

            If IsDBNull(dtrLocal("NMesi")) = False Then
                ddlDurata.SelectedValue = dtrLocal("NMesi")
            End If
            If dtrLocal("GiovaniMinoriOpportunità") = True Then
                chkGiovaniMinoriOp.Checked = True
                txtNumeroGiovaniMinoriOpportunita.Text = dtrLocal("NumeroGiovaniMinoriOpportunità")
                txtNumeroGiovaniMinoriOpportunita.Enabled = True
            End If

            'parte da commentare dal 15 luglio
            If IsDBNull(dtrLocal("NumeroPostiNoVittoNoAlloggio")) = False Then
                txtNumeroPostiSenzaVittoAlloggio.Text = dtrLocal("NumeroPostiNoVittoNoAlloggio")
            Else
                txtNumeroPostiSenzaVittoAlloggio.Text = "0"
            End If
            If IsDBNull(dtrLocal("NumeroPostiVittoAlloggio")) = False Then
                txtNumeroPostiVittoAlloggio.Text = dtrLocal("NumeroPostiVittoAlloggio")
            Else
                txtNumeroPostiVittoAlloggio.Text = "0"
            End If
            If IsDBNull(dtrLocal("NumeroPostiVitto")) = False Then
                txtNumeroPostiSoloVitto.Text = dtrLocal("NumeroPostiVitto")
            Else
                txtNumeroPostiSoloVitto.Text = "0"
            End If

            txtNumTotVolontaridaImpiegare.Text = CInt(IIf(txtNumeroPostiSoloVitto.Text = "", "0", CInt(txtNumeroPostiSoloVitto.Text))) + CInt(IIf(txtNumeroPostiVittoAlloggio.Text = "", "0", CInt(txtNumeroPostiVittoAlloggio.Text))) + CInt(IIf(txtNumeroPostiSenzaVittoAlloggio.Text = "", "0", CInt(txtNumeroPostiSenzaVittoAlloggio.Text)))

            'Gestione dei dati relativi ai posti Richiesti
            If IsDBNull(dtrLocal("NumeroPostiNoVittoNoAlloggioRic")) = False Then
                TxtNoVittoAlloggioRic.Text = dtrLocal("NumeroPostiNoVittoNoAlloggioRic")
            Else
                TxtNoVittoAlloggioRic.Text = "0"
            End If
            If IsDBNull(dtrLocal("NumeroPostiVittoAlloggioRic")) = False Then
                TxtVittoAlloggioRic.Text = dtrLocal("NumeroPostiVittoAlloggioRic")
            Else
                TxtVittoAlloggioRic.Text = "0"
            End If
            If IsDBNull(dtrLocal("NumeroPostiVittoRic")) = False Then
                TxtVittoRic.Text = dtrLocal("NumeroPostiVittoRic")
            Else
                TxtVittoRic.Text = "0"
            End If
            If IsDBNull(dtrLocal("RiduzioneEnte")) = False Then
                TxtRiduzioneEnte.Text = dtrLocal("RiduzioneEnte")
            Else
                TxtRiduzioneEnte.Text = "0"
            End If
            TxtTotaleRic.Text = CInt(IIf(TxtVittoRic.Text = "", "0", CInt(TxtVittoRic.Text))) + CInt(IIf(TxtVittoAlloggioRic.Text = "", "0", CInt(TxtVittoAlloggioRic.Text))) + CInt(IIf(TxtNoVittoAlloggioRic.Text = "", "0", CInt(TxtNoVittoAlloggioRic.Text)))
            If dtrLocal("EsteroUE") = True Then
                optEstero.Checked = True
                txtMesiPrevistiUE.Text = dtrLocal("NMesiEsteroUE")
                divMesiUE.Visible = True
            End If
            If dtrLocal("Tutoraggio") = True Then
                optTutoraggio.Checked = True
                txtMesiPrevistiTutoraggio.Text = dtrLocal("NMesiTutoraggio")
                divMesiTutoraggio.Visible = True
            End If

            ''****************** agg. da Simona Cordella il 20/11/2008 *************
            'Gestione del Tipo Finanziamento del progetto
            If IsDBNull(dtrLocal("TipoFinanziamento")) = False Then
                Select Case dtrLocal("TipoFinanziamento")
                    Case "0" 'finanziamento statale (fs)
                        OptFinStatale.Checked = True
                        OptFinRegionale.Checked = False
                        OptFinPrivato.Checked = False
                    Case "1" 'finanziamento regionale (fr)
                        OptFinStatale.Checked = False
                        OptFinRegionale.Checked = True
                        OptFinPrivato.Checked = False
                    Case "2" 'finanziamento privato(fp)
                        OptFinStatale.Checked = False
                        OptFinRegionale.Checked = False
                        OptFinPrivato.Checked = True
                End Select
            End If

            If chkCoProgettato.Visible = True Then
                chkCoProgettato.Checked = dtrLocal("CoProgettazione")
            End If
            'aggiunto il 05/05/2014 - statovalutazione
            ddlStatoValutazione.SelectedValue = dtrLocal("StatoValutazione")

            If IsDBNull(dtrLocal("CriteriSelezione")) = False Then
                ddlEventualiCriteri.SelectedValue = dtrLocal("CriteriSelezione")
                If (ddlEventualiCriteri.SelectedValue = "3") Then
                    txtCodEnteEventuali.Enabled = True
                End If
            Else
                ddlEventualiCriteri.SelectedValue = 0
            End If
            If IsDBNull(dtrLocal("CodiceEnteCriteriSelezione")) = False Then
                txtCodEnteEventuali.Enabled = True
                txtCodEnteEventuali.Text = dtrLocal("CodiceEnteCriteriSelezione")
            End If
            If IsDBNull(dtrLocal("CodiceEntePianoMonitoraggio")) = False Then
                txtPianoMonitoraggio.Enabled = True
                txtPianoMonitoraggio.Text = dtrLocal("CodiceEntePianoMonitoraggio")
            End If
            If IsDBNull(dtrLocal("OrePromozioneSensibilizzazione")) = False Then
                txtOrePromozioneSensibilizzazione.Enabled = True
                txtOrePromozioneSensibilizzazione.Text = dtrLocal("OrePromozioneSensibilizzazione")
            End If
            If IsDBNull(dtrLocal("SistemaSelezioneAccreditato")) = False Then
                Select Case dtrLocal("SistemaSelezioneAccreditato")
                    Case False
                        chkSistemaSelezione.Checked = False
                    Case True
                        chkSistemaSelezione.Checked = True
                End Select
            End If
            If IsDBNull(dtrLocal("PianoMonitoraggio")) = False Then
                ddlPianoMonitoraggio.SelectedIndex = dtrLocal("PianoMonitoraggio")
                If (ddlPianoMonitoraggio.SelectedValue = "2") Then
                    txtPianoMonitoraggio.Enabled = True
                Else
                    txtPianoMonitoraggio.Enabled = False
                End If
            Else
                ddlPianoMonitoraggio.SelectedIndex = 0
            End If
            If IsDBNull(dtrLocal("SistemaMonitoraggioAccreditato")) = False Then
                Select Case dtrLocal("SistemaMonitoraggioAccreditato")
                    Case False
                        chkSistemaMonitoraggioAccreditato.Checked = False
                    Case True
                        chkSistemaMonitoraggioAccreditato.Checked = True
                End Select
            End If

            If IsDBNull(dtrLocal("CreditiFormativi")) = False Then
                Select Case dtrLocal("CreditiFormativi")
                    Case "0"
                        ddlCreditiFormativiRiconosciuti.SelectedValue = 0
                    Case "1"
                        ddlCreditiFormativiRiconosciuti.SelectedValue = 1
                End Select
            End If

            If IsDBNull(dtrLocal("Tirocini")) = False Then
                Select Case dtrLocal("Tirocini")
                    Case "0"
                        ddlEventualiTirociniRiconosciuti.SelectedValue = 0
                    Case "1"
                        ddlEventualiTirociniRiconosciuti.SelectedValue = 1
                End Select
            End If

            If IsDBNull(dtrLocal("CompetenzeAcquisibili")) = False Then
                Select Case dtrLocal("CompetenzeAcquisibili")
                    Case "0"
                        ddlCompetenzeAcquisibili.SelectedValue = 0
                    Case "1"
                        ddlCompetenzeAcquisibili.SelectedValue = 1
                End Select
            End If

            'controllo lo stato del progetto se è attivo,proposto o in attesa di graduatoria posso modificare il tipo di finanziamento solo se sono un unsc o regione
            If Session("TipoUtente") = "U" Or Session("TipoUtente") = "R" Then
                If (dtrLocal("attiva") = True) Or (dtrLocal("davalutare") = True) Or (dtrLocal("dagraduare") = True) Then
                    OptFinStatale.Enabled = True
                    OptFinRegionale.Enabled = True
                    OptFinPrivato.Enabled = True
                Else
                    OptFinStatale.Enabled = False
                    OptFinRegionale.Enabled = False
                    OptFinPrivato.Enabled = False
                End If

            Else
                OptFinStatale.Enabled = False
                OptFinRegionale.Enabled = False
                OptFinPrivato.Enabled = False
            End If

            If IsDBNull(dtrLocal("ModalitàFormazioneGenerale")) = False Then
                ddlModalitaAttuazione.SelectedValue = dtrLocal("ModalitàFormazioneGenerale")
                If (ddlModalitaAttuazione.SelectedValue = "2") Then
                    txtModalitaAttuazione.Enabled = True
                End If
            Else
                ddlModalitaAttuazione.SelectedValue = 0
            End If

            If IsDBNull(dtrLocal("DurataFormazioneGenerale")) = False Then
                txtDurata.Text = dtrLocal("DurataFormazioneGenerale")
            Else
                txtDurata.Text = ""
            End If


            'ANTONELLO-----------------------------------------------------------------------
            If IsDBNull(dtrLocal("TipoFormazioneGenerale")) = False Then

                If dtrLocal("TipoFormazioneGenerale") = 1 Then
                    ddlFormazioneGeneraleErogazione.SelectedValue = "unicaTranche"
                End If
                If dtrLocal("TipoFormazioneGenerale") = 2 Then
                    ddlFormazioneGeneraleErogazione.SelectedValue = "percentuale"
                    txt180.Text = CInt(dtrLocal("DurataFormazioneGenerale1"))
                    txt270.Text = CInt(dtrLocal("DurataFormazioneGenerale2"))
                End If

            End If

            If IsDBNull(dtrLocal("CodiceEnteModalitàFormazioneGenerale")) = False Then
                txtModalitaAttuazione.Enabled = True
                txtModalitaAttuazione.Text = dtrLocal("CodiceEnteModalitàFormazioneGenerale")
            End If
            If IsDBNull(dtrLocal("Durata")) = False Then
                txtDurataSpec.Text = dtrLocal("Durata")
            Else
                txtDurataSpec.Text = ""
            End If
            If IsDBNull(dtrLocal("ModalitàFormazioneSpecifica")) = False Then
                ddlmodattuazione.SelectedIndex = dtrLocal("ModalitàFormazioneSpecifica")
                If (ddlmodattuazione.SelectedValue = "2") Then
                    txtmodattuazione.Enabled = True
                End If
            Else
                ddlmodattuazione.SelectedIndex = 0
            End If

            If IsDBNull(dtrLocal("CodiceEnteModalitàFormazioneSpecifica")) = False Then
                txtmodattuazione.Enabled = True
                txtmodattuazione.Text = dtrLocal("CodiceEnteModalitàFormazioneSpecifica")
            End If

            'ANTONELLO-----------------------------------------------------------------------
            If IsDBNull(dtrLocal("TipoFormazioneSpecifica")) = False Then

                If dtrLocal("TipoFormazioneSpecifica") = 1 Then
                    ddlFormazioneSpecificaErogazione.SelectedValue = "unicaTranche"
                End If
                If dtrLocal("TipoFormazioneSpecifica") = 2 Then
                    ddlFormazioneSpecificaErogazione.SelectedValue = "percentuale"
                    txt90S.Text = CInt(dtrLocal("DurataFormazioneSpecifica1"))
                    txt270S.Text = CInt(dtrLocal("DurataFormazioneSpecifica2"))
                End If
            End If

            If IsDBNull(dtrLocal("IDMacroAmbitoAttività")) = False Then
                strIdMacroAmbito.Value = dtrLocal("IDMacroAmbitoAttività")
            Else
                strIdMacroAmbito.Value = String.Empty
            End If
            'riporto i dati del fascicolo in masera
            TxtCodiceFascicolo.Text = "" & dtrLocal("CodiceFascicolopc")
            TxtIdFascicolo.Value = "" & dtrLocal("IDFascicolopc")
            txtDescFasc.Text = "" & dtrLocal("DescrFascicolopc")

            'controllo e chiudo il datareader qualora fosse rimasta appesa la connessione col database
            ChiudiDataReader(dtrLocal)

            If (Session("TipoUtente") = "U" And TxtCodiceFascicolo.Text <> "") Then
                IdDivRigaFascicolo.Visible = True
            End If

            CaricaComboSettori()
            'controllo e chiudo il datareader qualora fosse rimasta appesa la connessione col database
            ChiudiDataReader(dtrLocal)

            'Area - Settore
            strCaricaProgetto = "select (convert(varchar(10),ambitiattività.IdAmbitoAttività) + '|' + ambitiattività.Codifica + ' - ' + ambitiattività.AmbitoAttività) as IDCod,ambitiattività.IdMacroAmbitoAttività as IdMacroAmbitoAttivita ,ambitiattività.AmbitoAttività as AmbitoAttivita,(ambitiattività.Codifica + ' - ' + ambitiattività.AmbitoAttività) as Aree, ambitiattività.IDAmbitoAttività as IDAmbitoAttivita, "
            strCaricaProgetto = strCaricaProgetto & " (macroambitiattività.Codifica + ' - ' + macroambitiattività.MacroAmbitoAttività) as Settore"
            strCaricaProgetto = strCaricaProgetto & " from ambitiattività "
            strCaricaProgetto = strCaricaProgetto & " join macroambitiattività  on ambitiattività.IdMacroAmbitoAttività=macroambitiattività.IdMacroAmbitoAttività "
            strCaricaProgetto = strCaricaProgetto & " where(ambitiattività.IdAmbitoAttività = " & intIDAmbitoAttivita & ")"
            'eseguo la query
            myCommand.CommandText = strCaricaProgetto
            dtrLocal = myCommand.ExecuteReader
            'leggo il datareader
            dtrLocal.Read()
            'controllo se ci sono dei record
            If dtrLocal.HasRows = True Then
                'txtAreaPrincipale.Text = dtrLocal("Aree")
                Dim settore As Int32 = CInt(dtrLocal("IdMacroAmbitoAttivita"))

                Dim AmbitoAttivita = dtrLocal("IDCod")
                ChiudiDataReader(dtrLocal)
                CaricaComboArea(settore)
                ddlSettore.SelectedValue = settore
                ddlArea.SelectedValue = AmbitoAttivita
            Else
                CaricaComboAreaPrimaVolta()
            End If

            'fine blocco caricamento dati relativi al progetto in se (tabelle - ambitiattività)
            '***************************************************************************************************
            'caric0 i partner
            'controllo e chiudo il datareader qualora fosse rimasta appesa la connessione
            ChiudiDataReader(dtrLocal)

            strCaricaProgetto = "select a.IdPartner, a.Denominazione from Partners as a "
            strCaricaProgetto = strCaricaProgetto & "inner join AttivitàPartners as b on a.IdPartner=b.IdPartner "
            strCaricaProgetto = strCaricaProgetto & "where b.IdAttività=" & CInt(Request.QueryString("IdAttivita"))

            'eseguo la query
            myCommand.CommandText = strCaricaProgetto
            dtrLocal = myCommand.ExecuteReader
            'variabile stringa che utilizzo per catricare gli id dei partner inseriti pwer quel progetto
            Dim strValIdPartnerMod As String = ""
            Do While dtrLocal.Read
                strValIdPartnerMod = strValIdPartnerMod & dtrLocal("Denominazione") & "|"
            Loop
            'campo hidden che uso per passare al load della pagina
            'gli id dei partner all'hidden esistente così da 
            'mantenere nel form un hidden su cui poter fare un requset
            strIdPartnerModifica.Value = strValIdPartnerMod
            'controllo e chiudo se aperto il datareader

            ChiudiDataReader(dtrLocal)

            strCaricaProgetto = "select a.idambitoattività as idambitonascosto, "
            strCaricaProgetto = strCaricaProgetto & "(convert(varchar(10),a.IdAmbitoAttività) + '|' + b.Codifica + ' - ' + b.AmbitoAttività) as IdAmbito, "
            strCaricaProgetto = strCaricaProgetto & "(c.Codifica + ' - ' + c.MacroAmbitoAttività + '-' + b.Codifica + ' - ' + b.AmbitoAttività) as Denominazione "
            strCaricaProgetto = strCaricaProgetto & "from attivitàambiti as a "
            strCaricaProgetto = strCaricaProgetto & "inner join ambitiattività as b on a.idambitoattività=b.idambitoattività "
            strCaricaProgetto = strCaricaProgetto & "inner join macroambitiattività as c on b.idmacroambitoattività=c.idmacroambitoattività "
            strCaricaProgetto = strCaricaProgetto & "where a.idattività=" & CInt(Request.QueryString("IdAttivita"))

            'eseguo la query
            myCommand.CommandText = strCaricaProgetto
            dtrLocal = myCommand.ExecuteReader
            'variabile stringa che utilizzo per catricare gli id dei partner inseriti pwer quel progetto
            Dim strValIdAmbitoMod As String = ""
            If dtrLocal.HasRows = True Then
                Do While dtrLocal.Read
                    strValIdAmbitoMod = strValIdAmbitoMod & dtrLocal("IdAmbitoNascosto") & "|"
                    txtArea.Text = txtArea.Text & dtrLocal("Denominazione") & vbCrLf
                Loop
                'campo hidden che uso per passare al load della pagina
                'gli id dei partner all'hidden esistente così da 
                'mantenere nel form un hidden su cui poter fare un requset
                strIdAmbitiModifica.Value = strValIdAmbitoMod
            Else
                strIdAmbitiModifica.Value = String.Empty
            End If
            'controllo e chiudo se aperto il datareader
            ChiudiDataReader(dtrLocal)

            'carico le risorse finanziarie

            strCaricaProgetto = "Select a.Importo, a.VoceSpesa "
            strCaricaProgetto = strCaricaProgetto & "from AttivitàCaratteristicheOrganizzativeRisorseFinanziarie as a "
            strCaricaProgetto = strCaricaProgetto & "where a.IdAttivitàCaratteristicheOrganizzative=" & intIDAttivitaCaratteristicaOrganizzativa

            'eseguo la query
            myCommand.CommandText = strCaricaProgetto
            dtrLocal = myCommand.ExecuteReader

            'carico la txt delle voci spesa
            Do While dtrLocal.Read
            Loop

            ChiudiDataReader(dtrLocal)

            strCaricaProgetto = "select a.IdFormatoreSpecifico, (a.Cognome + ' ' + a.Nome) as Nominativo from FormatoriSpecifici as a "
            strCaricaProgetto = strCaricaProgetto & "inner join AttivitàFormazioneSpecificaFormatori as b on a.IdFormatoreSpecifico=b.IdFormatoreSpecifico "
            strCaricaProgetto = strCaricaProgetto & "where b.IdAttivitàFormazioneSpecifica=" & intIDAttivitaFormazioneSpecifica

            'eseguo la query
            myCommand.CommandText = strCaricaProgetto
            dtrLocal = myCommand.ExecuteReader
            'variabile stringa che utilizzo per catricare gli id delle pubblicita inserite
            Dim strValIdFormatoriMod As String = ""
            strIdFormatoriModifica.Value = strValIdFormatoriMod
            'controllo e chiudo se aperto il datareader
            ChiudiDataReader(dtrLocal)

            '****************************************************************************************************
            'carico le risorse finanziarie

            strCaricaProgetto = "Select a.Importo, a.VoceSpesa "
            strCaricaProgetto = strCaricaProgetto & "from AttivitàAltroFormazioneRisorseFinanziarie as a "
            strCaricaProgetto = strCaricaProgetto & "where a.IdAttivitàAltroFormazione=" & intIDAttivitaAltroFormazione
            'eseguo la query
            myCommand.CommandText = strCaricaProgetto
            dtrLocal = myCommand.ExecuteReader

            'controllo e chiudo se aperto il datareader
            ChiudiDataReader(dtrLocal)

        End If

        Dim strsql As String
        Dim blnCapoFila As Boolean

        ChiudiDataReader(dtrLocal)

        myCommand = New SqlClient.SqlCommand
        myCommand.Connection = Session("conn")
        'vado a controllare se si tratta di un progetto di un ente capofila
        strsql = "SELECT IdEntePresentante from attività where idattività=" & Request.QueryString("IdAttivita")

        myCommand.CommandText = strsql
        dtrLocal = myCommand.ExecuteReader
        If dtrLocal.HasRows = True Then
            dtrLocal.Read()

            If Session("IdEnte") <> dtrLocal("IdEntePresentante") Then
                blnCapoFila = False
            Else
                blnCapoFila = True
            End If
        End If
        ChiudiDataReader(dtrLocal)
        'strCaricaProgetto = strCaricaProgetto & "l.DefaultStato as StatoAttività, isnull(i.DefaultStato,1) as StatoBandoAttività  "
        If Visualizzazione = 0 Then
            InVisualizzazione()
            imgElencoDocumentiProg.Visible = blnCapoFila
        Else
            If blnCapoFila = False Then
                InVisualizzazione()
                imgCoProgettazione.Visible = False
                imgCancella.Visible = False
            Else
                'stampo comunque l'hidden che mi dice se devo visualizzare o meno i pulasnti delle varie popup
                chkVisualizzazione.Value = VALORE_FALSE
            End If
            imgElencoDocumentiProg.Visible = blnCapoFila
        End If

        ChiudiDataReader(dtrLocal)
        'stampo a pagina la descrizione dell'errore e la query


    End Sub


    Function Salva(ByVal ModificaInserimento As Integer) As Integer
        ReimpostaLabelMessaggi()
        Dim strProg As String
        Dim strDtReader As String
        Dim dtrLocal As SqlClient.SqlDataReader
        'variabile integer che uso per avere sempre attivo l'id dell'ultimo record inserito
        'per poter così chiudere il datareader
        Dim intIDMAXATTIVITAFormazioneSpecifica As Integer
        'variabile integer che uso per avere sempre attivo l'id dell'ultimo record inserito
        'per poter così chiudere il datareader
        Dim intIDMAXATTIVITACaratteristicaOrganizzativa As Integer
        'variabile integer che uso per avere sempre attivo l'id dell'ultimo record inserito
        'per poter così chiudere il datareader
        Dim intIDMAXATTIVITAAltroFormazione As Integer
        'variabile integer che uso per avere sempre attivo l'id dell'ultimo record inserito
        'per poter così chiudere il datareader
        Dim idAttivitaMemorizzata As Integer
        'variabile integer che uso per avere sempre attivo lo stato dell'attività da inserire
        'per poter così chiudere il datareader
        Dim intStatoAttivita As Integer
        'command che eseguirà le query e le insert
        Dim myCommand As System.Data.SqlClient.SqlCommand
        'transazione che gestirà il rollback in caso di errore
        Dim MyTransaction As System.Data.SqlClient.SqlTransaction
        'variabile che uso per ciclare i campi hidden degli ID
        Dim intX As Integer
        'variabile che uso per stabilire i vari punti dei campi hidden da cui partitre per prendere i singoli id
        Dim intY As Integer
        'vettore che conterrà lwe VociSpesa
        Dim VstrVociSpesa() As String
        'vettore che conterrà gli importi relativi alle voci spesa
        Dim VstrImporto() As String
        'variabile che uso per controllare l'id della regione di competenza dell'ente
        Dim intIdRegioneCompetenza As Integer
        'variabile che uso per controllare se è Competenza Nazionale
        Dim strIdRegioneCompetenza As String
        'se si tratta di inserimento
        If ModificaInserimento = 0 Then
            myCommand = New System.Data.SqlClient.SqlCommand
            myCommand.Connection = Session("conn")
            Try
                MyTransaction = Session("conn").BeginTransaction(Session("IdEnte") & "_" & Session("Utente"))
                myCommand.Transaction = MyTransaction
                'se si tratta di un progetto a bando nazionale 
                'fisso la gestione direttamente al nazionale
                'If CInt(Request.QueryString("Nazionale")) <> 3 Then
                '    '*********************************************
                '    'controllo se si tratta di un ente con competenza nazionale
                '    'se lo è vado ad inserire nel campo IdRegioneCompetenza il suo id
                '    'altrimenti lascio a null perchè successivamente prendo l'id dall'inserimento 
                '    'delle sedi di attuazione

                '    strDtReader = "select enti.IdRegioneCompetenza, RegioniCompetenze.CodiceRegioneCompetenza from enti INNER JOIN RegioniCompetenze ON RegioniCompetenze.IdRegioneCompetenza=enti.IdRegioneCompetenza where enti.idente='" & Session("IdEnte") & "'"
                '    ChiudiDataReader(dtrLocal)
                '    myCommand.CommandText = strDtReader
                '    dtrLocal = myCommand.ExecuteReader
                '    If dtrLocal.HasRows = True Then
                '        dtrLocal.Read()

                '        intIdRegioneCompetenza = dtrLocal("IdRegioneCompetenza")
                '        strIdRegioneCompetenza = dtrLocal("CodiceRegioneCompetenza")

                '        'Se si sta inserendo un progetto estero e l'ente è regionale, lo tratto come un ente di competenza nazionale
                '        If CInt(Request.QueryString("Nazionale")) = 2 And strIdRegioneCompetenza <> "NAZ" Then       'PROGETTO ESTERO / ENTE REGIONALE
                '            intIdRegioneCompetenza = 22
                '            strIdRegioneCompetenza = "NAZ"
                '        End If
                '    End If
                '    ChiudiDataReader(dtrLocal)
                'Else
                '    intIdRegioneCompetenza = 22
                '    strIdRegioneCompetenza = "NAZ"
                'End If
                'sempre nazionale
                intIdRegioneCompetenza = 22
                strIdRegioneCompetenza = "NAZ"

                If CInt(Request.QueryString("Nazionale")) = 1 Then
                    strProg = "insert into attività (Titolo, CodiceEnte, IdEntePresentante, DescrizioneContestoTerritoriale, "
                Else
                    strProg = "insert into attività (Titolo, CodiceEnte, IdEntePresentante, DescrizioneContestoPolitico, DescrizioneContestoTerritoriale, "
                End If
                strProg = strProg & "Obiettivi, Descrizione, DataInizioAttività, DataFineAttività, DataInizioPrevista, DataFinePrevista, NumeroPostiVittoAlloggio, "
                strProg = strProg & "NumeroPostiNoVittoNoAlloggio, NumeroPostiVitto, IDStatoAttività, IDAmbitoAttività, OreSettimanali, "
                'controllo se si tratta di un ente con competenza nazionale
                'se lo è vado ad inserire nel campo IdRegioneCompetenza il suo id
                'altrimenti lascio a null perchè successivamente prendo l'id dall'inserimento 
                'delle sedi di attuazione
                If strIdRegioneCompetenza = "NAZ" Then
                    strProg = strProg & "MonteOreAnnuo, GiorniServizioSettimanali, ObblighiVolontari, IdTipoProgetto, DataCreazioneRecord, UsernameInseritore,IdRegioneCompetenza,TipoFinanziamento, "
                Else
                    strProg = strProg & "MonteOreAnnuo, GiorniServizioSettimanali, ObblighiVolontari, IdTipoProgetto, DataCreazioneRecord, UsernameInseritore,TipoFinanziamento, "
                End If
                strProg = strProg & " NMesi,GiovaniMinoriOpportunità,NumeroGiovaniMinoriOpportunità, "
                strProg = strProg & " EsteroUE, Tutoraggio, NMesiEsteroUE, NMesiTutoraggio) "


                strProg = strProg & "values "
                strProg = strProg & "('" & Replace(txtTitolo.Text, "'", "''") & "', " ' titolo
                strProg = strProg & "'" & Replace(txtCodiceEnte.Text, "'", "''") & "', " ' Codice Progetto Ente
                strProg = strProg & CInt(Session("IdEnte")) & ", "  ' ente
                ' se si tratta di un progetto nazionale
                If CInt(Request.QueryString("Nazionale")) = 1 Then
                    strProg = strProg & "null, " ' Descrizione Contesto Territoriale
                Else ' progetto estero inserisco anche il contesto politico
                    strProg = strProg & "null, " ' Descrizione Contesto Politico
                    strProg = strProg & "null, " ' Descrizione Contesto Territoriale
                End If
                strProg = strProg & "null, " ' Obiettivi
                strProg = strProg & "null, " ' Descrizione
                strProg = strProg & "null, " ' DataInizioAttività
                strProg = strProg & "null, " ' DataFineAttività

                If Request.Form("txtDataInizioPrevista") <> "" Then
                    strProg = strProg & "'" & Day(Request.Form("txtDataInizioPrevista")) & "-" & Month(Request.Form("txtDataInizioPrevista")) & "-" & Year(Request.Form("txtDataInizioPrevista")) & "', " ' DataInizioPrevista
                    strProg = strProg & " dateadd(year, 1, convert(datetime,'" & Request.Form("txtDataInizioPrevista") & "',103)) - 1, " ' DataFinePrevista
                Else
                    strProg = strProg & "null, " ' DataInizioPrevsat
                    strProg = strProg & "null, " ' DataFinePrevista
                End If

                'fisso a 0 i valori dei vari posti di assegnazione al progetto perchè li leggo poi in modifica dai valori in sedi attuazzioni legate al progetto
                strProg = strProg & "null, " ' Numero POsti Vitto Alloggio
                strProg = strProg & "null, " ' Numero POsti senza Vitto e Alloggio
                strProg = strProg & "null, " ' Numero POsti Vitto

                'trovo l'idsTATOaTTIVITà
                strDtReader = "select IDStatoAttività from statiattività where DefaultStato=1"
                'controllo e chiudo se aperto il datareader
                If Not dtrLocal Is Nothing Then
                    dtrLocal.Close()
                    dtrLocal = Nothing
                End If
                'eseguo la query
                myCommand.CommandText = strDtReader
                dtrLocal = myCommand.ExecuteReader
                'leggo il datareader
                dtrLocal.Read()
                'se ci sono dei record
                If dtrLocal.HasRows = True Then
                    intStatoAttivita = CInt(dtrLocal("IDStatoAttività"))
                End If
                ChiudiDataReader(dtrLocal)
                strProg = strProg & intStatoAttivita & ", " ' STATO ATTIVITA'
                ' strProg = strProg & CInt(txtIdAreaPrincipale.Value) & ", " ' IDAMBITO ATTIVITA' DIVINA

                strProg = strProg & CInt(Mid(ddlArea.SelectedItem.Value, 1, InStr(ddlArea.SelectedItem.Value, "|") - 1)) & ", " ' IDAMBITO ATTIVITA'   ANTONELLO

                If txtOreServizioSettimanali.Text = "" Then
                    strProg = strProg & "null, " ' Numero POsti Vitto
                Else
                    strProg = strProg & CInt(txtOreServizioSettimanali.Text) & ", "  ' oRE sETTIMANALI
                End If
                If txtMonteOreAnnuo.Text = "" Then
                    strProg = strProg & "null, " ' Numero POsti Vitto
                Else
                    strProg = strProg & CInt(txtMonteOreAnnuo.Text) & ", " ' Monte oRE Annuo
                End If
                If txtNumGiorniServizio.Text = "" Then
                    strProg = strProg & "null, " ' giorni
                Else
                    strProg = strProg & CInt(txtNumGiorniServizio.Text) & ", "  ' Giorni Serivizio Settimanali
                End If
                strProg = strProg & "null, " ' Obblighi Volontari
                strProg = strProg & CInt(Request.QueryString("Nazionale")) & ", " ' IdTipoProgetto
                strProg = strProg & "GetDate(), " ' DataCreazione

                'modifica fatta da Indiana Jonathans il 04/07/2006
                'controllo se si tratta di un ente con competenza nazionale
                'se lo è vado ad inserire nel campo IdRegioneCompetenza il suo id
                'altrimenti lascio a null perchè successivamente prendo l'id dall'inserimento 
                'delle sedi di attuazione
                If strIdRegioneCompetenza = "NAZ" Then
                    strProg = strProg & "'" & Trim(Session("Utente")) & "'," ' Usernameinseritore
                    strProg = strProg & "" & intIdRegioneCompetenza & "," ' IdRegioneCompetenza
                Else
                    strProg = strProg & "'" & Trim(Session("Utente")) & "'," ' Usernameinseritore
                End If
                'Controllo sulla tipologia fi finanziamento
                If OptFinStatale.Checked = True Then
                    strProg = strProg & "0"
                ElseIf OptFinRegionale.Checked = True Then
                    strProg = strProg & "1"
                Else
                    strProg = strProg & "2"
                End If
                'aggiunto il 03/08/2017
                strProg = strProg & "," & CInt(ddlDurata.SelectedItem.Text) & ", " ' durataprogetto
                If chkGiovaniMinoriOp.Checked = True Then
                    strProg = strProg & "1, " & txtNumeroGiovaniMinoriOpportunita.Text & ""
                Else
                    strProg = strProg & "0, null "
                End If
                'aggiunto il 03/08/2017
                strProg = strProg & ","
                If Request.QueryString("Nazionale") <> 6 Then
                    If optEstero.Checked = True Then
                        strProg = strProg & " 1, 0, " & txtMesiPrevistiUE.Text & ", null"
                    End If
                    If optTutoraggio.Checked = True Then
                        strProg = strProg & " 0, 1, null, " & txtMesiPrevistiTutoraggio.Text & " "
                    End If
                    If optEstero.Checked = False And optTutoraggio.Checked = False Then 'agg. il 27/09/2017
                        strProg = strProg & " 0, 0, null,null"
                    End If
                Else 'progetto scu estero
                    strProg = strProg & " 0, 0, null,null"
                End If


                strProg = strProg & ")"
                myCommand.CommandText = strProg
                myCommand.ExecuteNonQuery()
                strDtReader = "select @@identity as IDMAx"
                ChiudiDataReader(dtrLocal)
                myCommand.CommandText = strDtReader
                dtrLocal = myCommand.ExecuteReader
                dtrLocal.Read()
                If dtrLocal.HasRows = True Then
                    idAttivitaMemorizzata = CInt(dtrLocal("IDMAx"))
                End If
                ChiudiDataReader(dtrLocal)

                'Inserimento ambiti
                intY = 1
                For intX = 1 To Len(txtIdAree.Value)
                    strProg = "insert into AttivitàAmbiti "
                    strProg = strProg & "(IdAttività, IdAmbitoAttività) "
                    strProg = strProg & "values "
                    strProg = strProg & "(" & idAttivitaMemorizzata & ", "
                    If Mid(txtIdAree.Value, intX, 1) = "|" Then
                        If intY = 1 Then
                            strProg = strProg & CInt(Mid(txtIdAree.Value, 1, intX - 1)) & ") "
                        Else
                            strProg = strProg & CInt(Mid(txtIdAree.Value, intY, intX - intY)) & ") "
                        End If
                        'stabilisco il punto di partenza del nuovo id
                        intY = intX + 1
                        myCommand.CommandText = strProg
                        myCommand.ExecuteNonQuery()
                        ReDim Session("VOperazioniAmbiti")(0)
                        Session("VOperazioniAmbiti")(0) = ""
                    End If
                Next

                'inserimento su AttivitàCaratteristicheOrganizzative
                strProg = "insert into AttivitàcaratteristicheOrganizzative (IdAttività, Tutor, AltreModalitàPubblicizzazione, "
                strProg = strProg & "CriteriSelezione, SistemaSelezioneAccreditato, PianoMonitoraggio,CodiceEnteCriteriSelezione,CodiceEntePianoMonitoraggio, "

                'Progetto Nazionale
                If CInt(Request.QueryString("Nazionale")) = 1 Then
                    strProg = strProg & "SistemaMonitoraggioAccreditato, RequisitiVolontari, RisorseNecessarie, OrePromozioneSensibilizzazione) "
                Else 'Progetto Estero
                    strProg = strProg & "SistemaMonitoraggioAccreditato, RequisitiVolontari, RisorseNecessarie, "
                    strProg = strProg & "CondizioniRischio, LivelliSicurezza, CondizioniDisagio, "
                    strProg = strProg & "ComunicazioneAutorità, CollegamentoSedeItaliana, ModalitàRientri, AssicurazioneIntegrativa, OrePromozioneSensibilizzazione) "
                End If
                strProg = strProg & "values "
                strProg = strProg & "(" & idAttivitaMemorizzata & ", " ' IdAttività
                strProg = strProg & "0, " ' Tutor
                strProg = strProg & "null, " ' AltriTipiPubblicità

                If ddlEventualiCriteri.SelectedValue = 0 Then
                    strProg = strProg & "null, " ' Criteri
                Else
                    strProg = strProg & ddlEventualiCriteri.SelectedValue & ", " ' Criteri
                End If

                If chkSistemaSelezione.Checked = True Then
                    strProg = strProg & "1, " ' Sistema di selezione
                Else
                    strProg = strProg & "0, " ' Sistema di selezione
                End If
                Select Case ddlPianoMonitoraggio.SelectedValue
                    Case 0
                        strProg = strProg & "null, " ' Piano Monitoraggio
                    Case 1
                        strProg = strProg & "1, " ' Piano Monitoraggio
                    Case 2
                        strProg = strProg & "2, " ' Piano Monitoraggio
                End Select
                If txtCodEnteEventuali.Text <> "" Then
                    strProg = strProg & "'" & txtCodEnteEventuali.Text & "', " 'CodiceEnteCriteriSelezione
                Else
                    strProg = strProg & "null, " ' CodiceEnteCriteriSelezione
                End If
                If txtPianoMonitoraggio.Text <> "" Then
                    strProg = strProg & "'" & txtPianoMonitoraggio.Text & "', " 'CodiceEntePianoMonitoraggio
                Else
                    strProg = strProg & "null, " ' CodiceEntePianoMonitoraggio
                End If
                If CInt(Request.QueryString("Nazionale")) = 1 Then
                    If chkSistemaMonitoraggioAccreditato.Checked = True Then
                        strProg = strProg & "1, " ' Sistema di monitoraggio accreditato
                    Else
                        strProg = strProg & "0, " ' Sistema di monitoraggio accreditato
                    End If
                    strProg = strProg & "null, " ' Requisiti
                    strProg = strProg & "null, " ' Risorse Necessarie

                    'OrePromozioneSensibilizzazione
                    If txtOrePromozioneSensibilizzazione.Text <> "" Then
                        strProg = strProg & txtOrePromozioneSensibilizzazione.Text & ")"
                    Else
                        strProg = strProg & "null)"
                    End If
                Else
                    If chkSistemaMonitoraggioAccreditato.Checked = True Then
                        strProg = strProg & "1, " ' Sistema di monitoraggio accreditato
                    Else
                        strProg = strProg & "0, " ' Sistema di monitoraggio accreditato
                    End If
                    strProg = strProg & "null, " ' Requisiti

                    strProg = strProg & "null, " ' Risorse Necessarie

                    Select Case ddlCondizioniRischio.SelectedValue
                        Case 0
                            strProg = strProg & "'0', " ' CondizioniRischio
                        Case 1
                            strProg = strProg & "'1', " ' CondizioniRischio
                        Case Else
                            strProg = strProg & "null, " ' CondizioniRischio
                    End Select

                    Select Case ddlLivelliSicurezza.SelectedValue
                        Case 0
                            strProg = strProg & "'0', " ' LivelliSicurezza
                        Case 1
                            strProg = strProg & "'1', " ' LivelliSicurezza
                        Case Else
                            strProg = strProg & "null, " ' LivelliSicurezza
                    End Select
                    Select Case ddlLivelliSicurezza.SelectedValue
                        Case 0
                            strProg = strProg & "'0', " ' LivelliSicurezza
                        Case 1
                            strProg = strProg & "'1', " ' LivelliSicurezza
                        Case Else
                            strProg = strProg & "null, " ' LivelliSicurezza
                    End Select
                    strProg = strProg & "null, " ' ComunicazioniAutorita
                    strProg = strProg & "null, " ' ComunicazioneSede

                    strProg = strProg & "null, " ' ModalitaRientri

                    Select Case ddlLivelliSicurezza.SelectedValue
                        Case 0
                            strProg = strProg & "'0', " ' LivelliSicurezza
                        Case 1
                            strProg = strProg & "'1', " ' LivelliSicurezza
                        Case Else
                            strProg = strProg & "null, " ' LivelliSicurezza
                    End Select

                    If txtOrePromozioneSensibilizzazione.Text <> "" Then
                        strProg = strProg & txtOrePromozioneSensibilizzazione.Text & ")"
                    Else
                        strProg = strProg & "null)"
                    End If

                End If

                myCommand.CommandText = strProg
                myCommand.ExecuteNonQuery()
                strDtReader = "select @@identity as IDMAx"
                ChiudiDataReader(dtrLocal)
                myCommand.CommandText = strDtReader
                dtrLocal = myCommand.ExecuteReader
                dtrLocal.Read()
                If dtrLocal.HasRows = True Then
                    intIDMAXATTIVITACaratteristicaOrganizzativa = CInt(dtrLocal("IDMAx"))
                End If
                ChiudiDataReader(dtrLocal)

                'blocco uinserimento risorse finanziarie in attivitàcaratteristicheorganizzative
                ReDim VstrImporto(0)
                intY = 1
                For intX = 1 To Len(Importo.Value)
                    If Mid(Importo.Value, intX, 1) = "|" Then
                        If intY = 1 Then
                            VstrImporto(0) = Mid(Importo.Value, 1, intX - 1)
                        Else
                            ReDim Preserve VstrImporto(UBound(VstrImporto) + 1)
                            VstrImporto(UBound(VstrImporto)) = Mid(Importo.Value, intY, intX - intY)
                        End If
                        'stabilisco il punto di partenza del nuovo id
                        intY = intX + 1
                    End If
                Next
                ReDim VstrVociSpesa(0)
                intY = 1
                For intX = 1 To Len(Request.Form("VociSpesa"))
                    If Mid(Request.Form("VociSpesa"), intX, 1) = "|" Then
                        If intY = 1 Then
                            VstrVociSpesa(0) = Mid(Request.Form("VociSpesa"), 1, intX - 1)
                        Else
                            ReDim Preserve VstrVociSpesa(UBound(VstrVociSpesa) + 1)
                            VstrVociSpesa(UBound(VstrVociSpesa)) = Mid(Request.Form("VociSpesa"), intY, intX - intY)
                        End If
                        'stabilisco il punto di partenza del nuovo id
                        intY = intX + 1
                    End If
                Next
                If Request.Form("TotVociSpesa") <> "" Then
                    For intX = 0 To CInt(Request.Form("TotVociSpesa")) - 1
                        strProg = "insert into AttivitàCaratteristicheOrganizzativeRisorseFinanziarie "
                        strProg = strProg & "(IdAttivitàCaratteristicheOrganizzative, Importo, VoceSpesa) "
                        strProg = strProg & "values "
                        strProg = strProg & "(" & intIDMAXATTIVITACaratteristicaOrganizzativa & ", "
                        strProg = strProg & "'" & VstrImporto(intX) & "', "
                        strProg = strProg & "'" & VstrVociSpesa(intX) & "')"
                        myCommand.CommandText = strProg
                        myCommand.ExecuteNonQuery()
                    Next
                End If

                'inserimento su AttivitàCaratteristicheConoscenze
                strProg = "insert into AttivitàCaratteristicheConoscenze (IdAttività, CreditiFormativi, "
                strProg = strProg & "Tirocini, CompetenzeAcquisibili) "
                strProg = strProg & "values "
                strProg = strProg & "(" & idAttivitaMemorizzata & ", " ' IdAttività
                Select Case ddlCreditiFormativiRiconosciuti.SelectedValue
                    Case 0
                        strProg = strProg & "'0', " ' Crediti Riconosciuti
                    Case 1
                        strProg = strProg & "'1', " ' Crediti Riconosciuti
                    Case Else
                        strProg = strProg & "null, " ' Crediti Riconosciuti
                End Select

                Select Case ddlEventualiTirociniRiconosciuti.SelectedValue
                    Case 0
                        strProg = strProg & "'0', " ' Crediti Riconosciuti
                    Case 1
                        strProg = strProg & "'1', " ' Crediti Riconosciuti
                    Case Else
                        strProg = strProg & "null, " ' Crediti Riconosciuti
                End Select

                Select Case ddlCompetenzeAcquisibili.SelectedValue
                    Case 0
                        strProg = strProg & "'0') " ' Crediti Riconosciuti
                    Case 1
                        strProg = strProg & "'1') " ' Crediti Riconosciuti
                    Case Else
                        strProg = strProg & "null) " ' Crediti Riconosciuti
                End Select

                myCommand.CommandText = strProg
                myCommand.ExecuteNonQuery()

                'inserimento su FormazioneGenerale
                strProg = "insert into AttivitàFormazioneGenerale (IdAttività, SedeFormazioneGenerale, "
                strProg = strProg & "ModalitàFormazioneGenerale, SistemaFormazioneAccreditato, DurataFormazioneGenerale,CodiceEnteModalitàFormazioneGenerale,TipoFormazioneGenerale,DurataFormazioneGenerale1,DurataFormazioneGenerale2) "
                strProg = strProg & "values "
                strProg = strProg & "(" & idAttivitaMemorizzata & ", " ' IdAttività
                strProg = strProg & "null, " ' SedeRealizzazione

                Select Case ddlModalitaAttuazione.SelectedValue ' ModalitaAttuazione
                    Case 0
                        strProg = strProg & "null, " ' ModalitaAttuazione
                    Case 1
                        strProg = strProg & "1, " ' ModalitaAttuazione
                    Case 2
                        strProg = strProg & "2, " ' ModalitaAttuazione
                    Case 3
                        strProg = strProg & "3, " ' ModalitaAttuazione
                End Select
                strProg = strProg & "0, " ' SistemaFormazioneAccreditamento
                If txtDurata.Text = "" Then
                    strProg = strProg & "null, " ' Durata
                Else
                    strProg = strProg & CInt(txtDurata.Text) & ", " ' Durata
                End If
                If txtModalitaAttuazione.Text <> "" Then
                    strProg = strProg & "'" & txtModalitaAttuazione.Text & "'," 'CodiceEnteModalitàFormazioneGenerale
                Else
                    strProg = strProg & "null,  "  ' CodiceEnteModalitàFormazioneGenerale
                End If


                If ddlFormazioneGeneraleErogazione.SelectedValue = "unicaTranche" Then
                    strProg = strProg & " 1, "
                    strProg = strProg & "null,"
                    strProg = strProg & "null)"
                End If

                If ddlFormazioneGeneraleErogazione.SelectedValue = "percentuale" Then

                    strProg = strProg & " 2, "

                    If txt180.Text = "" Then
                        strProg = strProg & " null,  "
                    Else
                        strProg = strProg & CInt(txt180.Text) & ", "
                    End If

                    If txt270.Text = "" Then
                        strProg = strProg & "null)"
                    Else
                        strProg = strProg & CInt(txt270.Text) & ")"
                    End If

                End If

                myCommand.CommandText = strProg
                myCommand.ExecuteNonQuery()

                'inserimento su FormazioneSpecifica
                strProg = "insert into AttivitàFormazioneSpecifica (IdAttività, SedeFormazioneSpecifica, "
                strProg = strProg & "ModalitàFormazioneSpecifica, CompetenzeFormatori, Durata,CodiceEnteModalitàFormazioneSpecifica,TipoFormazioneSpecifica,DurataFormazioneSpecifica1,DurataFormazioneSpecifica2) "
                strProg = strProg & "values "
                strProg = strProg & "(" & idAttivitaMemorizzata & ", " ' IdAttività
                strProg = strProg & "null, " ' SedeRealizzazione
                Select Case ddlmodattuazione.SelectedValue ' ModalitaAttuazione
                    Case 0
                        strProg = strProg & "null, " ' ModalitaAttuazione
                    Case 1
                        strProg = strProg & "1, " ' ModalitaAttuazione
                    Case 2
                        strProg = strProg & "2, " ' ModalitaAttuazione
                    Case 3
                        strProg = strProg & "3, " ' ModalitaAttuazione
                End Select
                strProg = strProg & "null, " ' CompetenzeSecificheFormatori
                If txtDurataSpec.Text = "" Then
                    strProg = strProg & "null, " ' Durata
                Else
                    strProg = strProg & CInt(txtDurataSpec.Text) & ", " ' Durata
                End If
                If txtmodattuazione.Text <> "" Then
                    strProg = strProg & "'" & txtmodattuazione.Text & "'," 'CodiceEnteModalitàFormazioneSpecifica
                Else
                    strProg = strProg & "null," ' CodiceEnteModalitàFormazioneSpecifica
                End If

                If ddlFormazioneSpecificaErogazione.SelectedValue = "unicaTranche" Then
                    strProg = strProg & " 1, "
                    strProg = strProg & "null,"
                    strProg = strProg & "null)"
                End If
                If ddlFormazioneSpecificaErogazione.SelectedValue = "percentuale" Then
                    strProg = strProg & " 2, "
                    If txt90S.Text = "" Then
                        strProg = strProg & " null,  "
                    Else
                        strProg = strProg & CInt(txt90S.Text) & ", "
                    End If

                    If txt270S.Text = "" Then
                        strProg = strProg & "null)"
                    Else
                        strProg = strProg & CInt(txt270S.Text) & ")"
                    End If

                End If
                myCommand.CommandText = strProg
                myCommand.ExecuteNonQuery()
                strDtReader = "select @@identity as IDMAx"
                ChiudiDataReader(dtrLocal)
                myCommand.CommandText = strDtReader
                dtrLocal = myCommand.ExecuteReader
                dtrLocal.Read()
                If dtrLocal.HasRows = True Then
                    intIDMAXATTIVITAFormazioneSpecifica = CInt(dtrLocal("IDMAx"))
                End If
                ChiudiDataReader(dtrLocal)

                'blocco inserimento tipi èpubblicità in attivitàFormazioneSpecifica
                intY = 1
                For intX = 1 To Len(txtIdFormatori.Value)
                    strProg = "insert into AttivitàFormazioneSpecificaFormatori "
                    strProg = strProg & "(IdAttivitàFormazioneSpecifica, IdFormatoreSpecifico) "
                    strProg = strProg & "values "
                    strProg = strProg & "(" & intIDMAXATTIVITAFormazioneSpecifica & ", "
                    If Mid(txtIdFormatori.Value, intX, 1) = "|" Then
                        If intY = 1 Then
                            strProg = strProg & CInt(Mid(txtIdFormatori.Value, 1, intX - 1)) & ")"
                        Else
                            strProg = strProg & CInt(Mid(txtIdFormatori.Value, intY, intX - intY)) & ")"
                        End If
                        'stabilisco il punto di partenza del nuovo id
                        intY = intX + 1
                        myCommand.CommandText = strProg
                        myCommand.ExecuteNonQuery()
                    End If
                Next

                'inserimento su Altri elementi della formazione
                strProg = "insert into AttivitàAltroFormazione (IdAttività, ModalitàMonitoraggioFormazione, "
                strProg = strProg & "MonitoraggioFormazionePresentato) "
                strProg = strProg & "values "
                strProg = strProg & "(" & idAttivitaMemorizzata & ", " ' IdAttività
                strProg = strProg & "null, " ' ModalitaMonitoraggio
                strProg = strProg & "0)" ' MonitoraggioPresentato

                myCommand.CommandText = strProg
                myCommand.ExecuteNonQuery()
                strDtReader = "select @@identity as IDMAx"
                ChiudiDataReader(dtrLocal)
                myCommand.CommandText = strDtReader
                dtrLocal = myCommand.ExecuteReader
                dtrLocal.Read()
                If dtrLocal.HasRows = True Then
                    intIDMAXATTIVITAAltroFormazione = CInt(dtrLocal("IDMAx"))
                End If
                ChiudiDataReader(dtrLocal)

                'blocco inserimento risorse finanziarie in attivitàcaratteristicheorganizzative
                ReDim VstrImporto(0)
                intY = 1
                For intX = 1 To Len(AltroImporto.Value)
                    If Mid(AltroImporto.Value, intX, 1) = "|" Then
                        If intY = 1 Then
                            VstrImporto(0) = Mid(AltroImporto.Value, 1, intX - 1)
                        Else
                            ReDim Preserve VstrImporto(UBound(VstrImporto) + 1)
                            VstrImporto(UBound(VstrImporto)) = Mid(AltroImporto.Value, intY, intX - intY)
                        End If
                        'stabilisco il punto di partenza del nuovo id
                        intY = intX + 1
                    End If
                Next
                ReDim VstrVociSpesa(0)
                intY = 1
                For intX = 1 To Len(AltroVociSpesa.Value)
                    If Mid(AltroVociSpesa.Value, intX, 1) = "|" Then
                        If intY = 1 Then
                            VstrVociSpesa(0) = Mid(AltroVociSpesa.Value, 1, intX - 1)
                        Else
                            ReDim Preserve VstrVociSpesa(UBound(VstrVociSpesa) + 1)
                            VstrVociSpesa(UBound(VstrVociSpesa)) = Mid(Request.Form("AltroVociSpesa"), intY, intX - intY)
                        End If
                        'stabilisco il punto di partenza del nuovo id
                        intY = intX + 1
                    End If
                Next
                If Request.Form("TotAltroVociSpesa") <> "" Then
                    For intX = 0 To CInt(Request.Form("TotAltroVociSpesa")) - 1
                        strProg = "insert into AttivitàAltroFormazioneRisorseFinanziarie "
                        strProg = strProg & "(IdAttivitàAltroFormazione, Importo, VoceSpesa) "
                        strProg = strProg & "values "
                        strProg = strProg & "(" & intIDMAXATTIVITAAltroFormazione & ", "
                        strProg = strProg & "'" & VstrImporto(intX) & "', "
                        strProg = strProg & "'" & VstrVociSpesa(intX) & "')"
                        myCommand.CommandText = strProg
                        myCommand.ExecuteNonQuery()
                    Next
                End If
                'chiudo i due vettori per liberare memoria
                VstrImporto = Nothing
                VstrVociSpesa = Nothing

                MyTransaction.Commit()
                lblMessaggioConferma.Text = "Progetto inserito con successo."

            Catch ex As Exception
                Response.Write(strDtReader)
                Response.Write("<br/>")
                Response.Write(strProg)
                Response.Write("<br/>")
                Response.Write(ex.Message.ToString)
                MyTransaction.Rollback(Session("IdEnte") & "_" & Session("Utente"))
            End Try
            MyTransaction.Dispose()
        Else 'MODIFICA
            myCommand = New System.Data.SqlClient.SqlCommand
            myCommand.Connection = Session("conn")
            Try
                MyTransaction = Session("conn").BeginTransaction(Session("IdEnte") & "_" & Session("Utente"))
                myCommand.Transaction = MyTransaction
                'Reimposto il nuovo totale posti richiesti (Lo perde in fase di click)
                If Session("tipoutente") <> "E" Then
                    TxtTotaleRic.Text = CInt(IIf(TxtVittoRic.Text = "", "0", CInt(TxtVittoRic.Text))) + CInt(IIf(TxtVittoAlloggioRic.Text = "", "0", CInt(TxtVittoAlloggioRic.Text))) + CInt(IIf(TxtNoVittoAlloggioRic.Text = "", "0", CInt(TxtNoVittoAlloggioRic.Text)))
                End If
                strProg = "UpDate Attività Set "
                strProg = strProg & "Titolo='" & Replace(txtTitolo.Text, "'", "''") & "', "
                strProg = strProg & "CodiceEnte='" & Replace(txtCodiceEnte.Text, "'", "''") & "', " ' Codice Progetto Ente
                strProg = strProg & "DescrizioneContestoTerritoriale=null, " ' Descrizione Contesto Territoriale
                If CInt(Request.QueryString("Nazionale")) = 2 Then
                    strProg = strProg & "DescrizioneContestoPolitico=null, " ' Descrizione Contesto Territoriale
                End If
                strProg = strProg & "Descrizione=null, " ' Descrizione

                'If Request.Form("txtDataInizioPrevista") <> "" Then
                '    strProg = strProg & "DataInizioPrevista='" & Day(Request.Form("txtDataInizioPrevista")) & "-" & Month(Request.Form("txtDataInizioPrevista")) & "-" & Year(Request.Form("txtDataInizioPrevista")) & "', " ' DataInizioPrevista
                '    strProg = strProg & " DataFinePrevista=dateadd(year, 1, convert(datetime,'" & Request.Form("txtDataInizioPrevista") & "',103)) - 1, " ' DataFinePrevista
                'Else
                '    strProg = strProg & "DataInizioPrevista=null, " ' DataInizioPrevsat
                '    strProg = strProg & "DataFinePrevista=null, " ' DataFinePrevista
                'End If

                'strProg = strProg & "IDAmbitoAttività=" & CInt( txtIdAreaPrincipale.Value) & ", " ' IDAMBITO ATTIVITA' DIVINA
                strProg = strProg & "IDAmbitoAttività=" & CInt(Mid(ddlArea.SelectedItem.Value, 1, InStr(ddlArea.SelectedItem.Value, "|") - 1)) & ", " ' IDAMBITO ATTIVITA'  ANTONELLO
                If txtOreServizioSettimanali.Text = "" Then
                    strProg = strProg & "OreSettimanali=null, " ' Numero POsti Vitto
                Else
                    strProg = strProg & "OreSettimanali=" & CInt(txtOreServizioSettimanali.Text) & ", "  ' oRE sETTIMANALI
                End If

                If txtMonteOreAnnuo.Text = "" Then
                    strProg = strProg & "MonteOreAnnuo=null, " ' Numero POsti Vitto
                Else
                    strProg = strProg & "MonteOreAnnuo=" & CInt(txtMonteOreAnnuo.Text) & ", " ' Monte oRE Annuo
                End If
                If txtNumGiorniServizio.Text = "" Then
                    strProg = strProg & "GiorniServizioSettimanali=null, " ' giorni
                Else
                    strProg = strProg & "GiorniServizioSettimanali=" & CInt(txtNumGiorniServizio.Text) & ", "  ' Giorni Serivizio Settimanali
                End If
                strProg = strProg & "ObblighiVolontari=null, " ' Obblighi Volontari

                strProg = strProg & "IdTipoProgetto=" & CInt(Request.QueryString("Nazionale")) ' IdTipoProgetto
                If (Session("TipoUtente") = "U" Or Session("TipoUtente") = "R") Then
                    'Modifica dei Campi Volontari Richiesti
                    strProg = strProg & ",NumeroPostiNoVittoNoAlloggioRic = " & TxtNoVittoAlloggioRic.Text & ",NumeroPostiVittoAlloggioRic = " & TxtVittoAlloggioRic.Text & ",NumeroPostiVittoRic = " & TxtVittoRic.Text
                    strProg = strProg & ",RiduzioneEnte= " & TxtRiduzioneEnte.Text
                End If

                'Controllo sulla tipologia di finanziamento
                If OptFinStatale.Checked = True Then
                    strProg = strProg & ",TipoFinanziamento =0"
                ElseIf OptFinRegionale.Checked = True Then
                    strProg = strProg & ",TipoFinanziamento =1"
                Else
                    strProg = strProg & ",TipoFinanziamento =2"
                End If
                'strProg = strProg & ",StatoValutazione =" & ddlStatoValutazione.SelectedValue & ""

                ''aggiunto il 03/08/2017
                'strProg = strProg & "," & CInt(ddlDurata.SelectedItem.Text) & ", " ' durataprogetto
                'If chkGiovaniMinoriOp.Checked = True Then
                '    strProg = strProg & "1, " & txtNumeroGiovaniMinoriOpportunita.Text & ""
                'Else
                '    strProg = strProg & "0, null "
                'End If
                ''aggiunto il 03/08/2017
                'strProg = strProg & ","
                'If optEstero.Checked = True Then
                '    strProg = strProg & " 1, 0, " & txtMesiPrevistiUE.Text & ", null"
                'End If
                'If optTutoraggio.Checked = True Then
                '    strProg = strProg & " 0, 1, null, " & txtMesiPrevistiTutoraggio.Text & " "
                'End If

                strProg = strProg & " ,NMesi =" & ddlDurata.SelectedValue & " "
                If chkGiovaniMinoriOp.Checked = True Then
                    strProg = strProg & " ,GiovaniMinoriOpportunità=1, NumeroGiovaniMinoriOpportunità= " & txtNumeroGiovaniMinoriOpportunita.Text & ""
                Else
                    strProg = strProg & " ,GiovaniMinoriOpportunità=0, NumeroGiovaniMinoriOpportunità= null "
                End If
                If optEstero.Checked = True Then
                    strProg = strProg & " ,EsteroUE =1, NMesiEsteroUE= " & txtMesiPrevistiUE.Text & ""
                Else
                    strProg = strProg & " ,EsteroUE =0, NMesiEsteroUE= null "
                End If
                If optTutoraggio.Checked = True Then
                    strProg = strProg & " ,Tutoraggio =1, NMesiTutoraggio= " & txtMesiPrevistiTutoraggio.Text & ""
                Else
                    strProg = strProg & " , Tutoraggio =0, NMesiTutoraggio= null "
                End If


                'If optEstero.Checked = True Then
                '    strProg = strProg & " ,EsteroUE =1, Tutoraggio=0, NMesiEsteroUE= " & txtMesiPrevistiUE.Text & ", NMesiTutoraggio=null "
                'End If
                'If optTutoraggio.Checked = True Then
                '    strProg = strProg & " ,EsteroUE =0, Tutoraggio=1, NMesiEsteroUE= null ,NMesiTutoraggio= " & txtMesiPrevistiTutoraggio.Text & " "
                'End If


                strProg = strProg & " where IDAttività=" & CInt(Request.QueryString("IdAttivita"))
                myCommand.CommandText = strProg
                myCommand.ExecuteNonQuery()
                Dim x As Integer
                myCommand.Connection = Session("conn")
                'controllo se effettivamente sono in modifica
                If Not Request.QueryString("Idattivita") Is Nothing Then
                    If Session("VOperazioniAmbiti")(0) <> "" Then
                        For x = 0 To UBound(Session("VOperazioniAmbiti"))
                            myCommand.CommandText = Session("VOperazioniAmbiti")(x)
                            myCommand.ExecuteNonQuery()
                        Next
                        ReDim Session("VOperazioniAmbiti")(0)
                        Session("VOperazioniAmbiti")(0) = ""
                    End If
                End If

                'inserimento su AttivitàCaratteristicheOrganizzative
                strProg = "update AttivitàcaratteristicheOrganizzative set "
                strProg = strProg & "Tutor=0, " ' Tutor
                strProg = strProg & "AltreModalitàPubblicizzazione=null, " ' AltriTipiPubblicità

                Select Case ddlEventualiCriteri.SelectedValue
                    Case 0
                        strProg = strProg & "CriteriSelezione=null, " ' Criteri
                    Case 1
                        strProg = strProg & "CriteriSelezione=1, " ' Criteri
                    Case 2
                        strProg = strProg & "CriteriSelezione=2, " ' Criteri
                    Case 3
                        strProg = strProg & "CriteriSelezione=3, " ' Criteri
                    Case 4
                        strProg = strProg & "CriteriSelezione=4, " ' Criteri
                End Select
                If chkSistemaSelezione.Checked = True Then
                    strProg = strProg & "SistemaSelezioneAccreditato=1, " ' Sistema di selezione
                Else
                    strProg = strProg & "SistemaSelezioneAccreditato=0, " ' Sistema di selezione
                End If
                Select Case ddlPianoMonitoraggio.SelectedValue
                    Case 0
                        strProg = strProg & "PianoMonitoraggio=null, " ' Piano Monitoraggio
                    Case 1
                        strProg = strProg & "PianoMonitoraggio=1, " ' Piano Monitoraggio
                    Case 2
                        strProg = strProg & "PianoMonitoraggio=2, " ' Piano Monitoraggio
                End Select
                If chkSistemaMonitoraggioAccreditato.Checked = True Then
                    strProg = strProg & "SistemaMonitoraggioAccreditato=1, " ' Sistema di monitoraggio accreditato
                Else
                    strProg = strProg & "SistemaMonitoraggioAccreditato=0, " ' Sistema di monitoraggio accreditato
                End If
                strProg = strProg & "RequisitiVolontari=null, " ' Requisiti
                strProg = strProg & "RisorseNecessarie=null, " ' Risorse Necessarie
                If txtCodEnteEventuali.Text <> "" Then
                    strProg = strProg & "CodiceEnteCriteriSelezione='" & txtCodEnteEventuali.Text & "', " 'CodiceEnteCriteriSelezione
                Else
                    strProg = strProg & "CodiceEnteCriteriSelezione=null, " ' CodiceEnteCriteriSelezione
                End If

                'se si tratta di un progetto nazionale
                If CInt(Request.QueryString("Nazionale")) = 6 Then
                    Select Case ddlCondizioniRischio.SelectedValue
                        Case 0
                            strProg = strProg & "CondizioniRischio='0', " ' CondizioniRischio
                        Case 1
                            strProg = strProg & "CondizioniRischio='1', " ' CondizioniRischio
                    End Select
                    Select Case ddlLivelliSicurezza.SelectedValue
                        Case 0
                            strProg = strProg & "LivelliSicurezza='0', " ' CondizioniDisagio
                        Case 1
                            strProg = strProg & "LivelliSicurezza='1', " ' CondizioniDisagio
                    End Select
                    Select Case ddlCondizioniDisagio.SelectedValue
                        Case 0
                            strProg = strProg & "CondizioniDisagio='0', " ' CondizioniDisagio
                        Case 1
                            strProg = strProg & "CondizioniDisagio='1', " ' CondizioniDisagio
                    End Select
                    strProg = strProg & "ComunicazioneAutorità=null, " ' ComunicazioniAutorita
                    strProg = strProg & "CollegamentoSedeItaliana=null, " ' ComunicazioneSede
                    strProg = strProg & "ModalitàRientri=null, " ' ModalitaRientri
                    Select Case ddlEventualeAssicurazione.SelectedValue
                        Case 0
                            strProg = strProg & "AssicurazioneIntegrativa='0', " ' CondizioniDisagio
                        Case 1
                            strProg = strProg & "AssicurazioneIntegrativa='1', " ' CondizioniDisagio
                        Case Else
                            strProg = strProg & "AssicurazioneIntegrativa=null, " ' CondizioniDisagio
                    End Select
                End If

                If txtPianoMonitoraggio.Text <> "" Then
                    strProg = strProg & "CodiceEntePianoMonitoraggio='" & txtPianoMonitoraggio.Text & "', " 'CodiceEntePianoMonitoraggio
                Else
                    strProg = strProg & "CodiceEntePianoMonitoraggio=null, " ' CodiceEntePianoMonitoraggio
                End If

                If txtOrePromozioneSensibilizzazione.Text <> "" Then
                    strProg = strProg & "OrePromozioneSensibilizzazione=" & txtOrePromozioneSensibilizzazione.Text & " "
                Else
                    strProg = strProg & "OrePromozioneSensibilizzazione=null "
                End If

                strProg = strProg & " where IDAttività=" & CInt(Request.QueryString("IdAttivita"))
                myCommand.CommandText = strProg
                myCommand.ExecuteNonQuery()

                'inserimento su AttivitàCaratteristicheConoscenze
                strProg = "update AttivitàCaratteristicheConoscenze set "
                Select Case ddlCreditiFormativiRiconosciuti.SelectedValue
                    Case 0
                        strProg = strProg & "CreditiFormativi='0', " ' Crediti Riconosciuti
                    Case 1
                        strProg = strProg & "CreditiFormativi='1', " ' Crediti Riconosciuti
                    Case Else
                        strProg = strProg & "CreditiFormativi=null, " ' Crediti Riconosciuti
                End Select

                Select Case ddlEventualiTirociniRiconosciuti.SelectedValue
                    Case 0
                        strProg = strProg & "Tirocini='0', " ' Crediti Riconosciuti
                    Case 1
                        strProg = strProg & "Tirocini='1', " ' Crediti Riconosciuti
                    Case Else
                        strProg = strProg & "Tirocini=null, " ' Crediti Riconosciuti
                End Select

                Select Case ddlCompetenzeAcquisibili.SelectedValue
                    Case 0
                        strProg = strProg & "CompetenzeAcquisibili='0' " ' Crediti Riconosciuti
                    Case 1
                        strProg = strProg & "CompetenzeAcquisibili='1' " ' Crediti Riconosciuti
                    Case Else
                        strProg = strProg & "CompetenzeAcquisibili=null " ' Crediti Riconosciuti
                End Select
                strProg = strProg & " where IDAttività=" & CInt(Request.QueryString("IdAttivita"))

                myCommand.CommandText = strProg
                myCommand.ExecuteNonQuery()

                'inserimento su FormazioneGenerale
                strProg = "update AttivitàFormazioneGenerale set "
                strProg = strProg & "SedeFormazioneGenerale=null, " ' SedeRealizzazione

                Select Case ddlModalitaAttuazione.SelectedValue ' ModalitaAttuazione
                    Case 0
                        strProg = strProg & "ModalitàFormazioneGenerale=null, " ' ModalitaAttuazione
                    Case 1
                        strProg = strProg & "ModalitàFormazioneGenerale=1, " ' ModalitaAttuazione
                    Case 2
                        strProg = strProg & "ModalitàFormazioneGenerale=2, " ' ModalitaAttuazione
                    Case 3
                        strProg = strProg & "ModalitàFormazioneGenerale=3, " ' ModalitaAttuazione
                    Case Else
                        strProg = strProg & "ModalitàFormazioneGenerale=null, " ' ModalitaAttuazione
                End Select
                strProg = strProg & "SistemaFormazioneAccreditato=0, " ' SistemaFormazioneAccreditamento
                If txtDurata.Text = "" Then
                    strProg = strProg & "DurataFormazioneGenerale=null, " ' Durata
                Else
                    strProg = strProg & "DurataFormazioneGenerale=" & CInt(txtDurata.Text) & ", " ' Durata
                End If
                If txtModalitaAttuazione.Text <> "" Then
                    strProg = strProg & "CodiceEnteModalitàFormazioneGenerale='" & txtModalitaAttuazione.Text & "', " 'CodiceEnteModalitàFormazioneGenerale
                Else
                    strProg = strProg & "CodiceEnteModalitàFormazioneGenerale=null, " ' CodiceEnteModalitàFormazioneGenerale
                End If

                If ddlFormazioneGeneraleErogazione.SelectedValue = "unicaTranche" Then
                    strProg = strProg & "TipoFormazioneGenerale=1, "
                    strProg = strProg & "DurataFormazioneGenerale1=null,"
                    strProg = strProg & "DurataFormazioneGenerale2=null"
                End If

                If ddlFormazioneGeneraleErogazione.SelectedValue = "percentuale" Then

                    strProg = strProg & "TipoFormazioneGenerale=2, "

                    If txt180.Text = "" Then
                        strProg = strProg & "DurataFormazioneGenerale1=null,"
                    Else
                        strProg = strProg & "DurataFormazioneGenerale1=" & CInt(txt180.Text) & ", "
                    End If

                    If txt270.Text = "" Then
                        strProg = strProg & "null "
                    Else
                        strProg = strProg & "DurataFormazioneGenerale2=" & CInt(txt270.Text) & ""
                    End If

                End If


                strProg = strProg & " where IDAttività=" & CInt(Request.QueryString("IdAttivita"))

                myCommand.CommandText = strProg
                myCommand.ExecuteNonQuery()

                'inserimento su FormazioneSpecifica
                strProg = "update AttivitàFormazioneSpecifica set "
                strProg = strProg & "SedeFormazioneSpecifica=null, " ' SedeRealizzazione
                Select Case ddlmodattuazione.SelectedValue ' ModalitaAttuazione
                    Case 0
                        strProg = strProg & "ModalitàFormazioneSpecifica=null, " ' ModalitaAttuazione
                    Case 1
                        strProg = strProg & "ModalitàFormazioneSpecifica=1, " ' ModalitaAttuazione
                    Case 2
                        strProg = strProg & "ModalitàFormazioneSpecifica=2, " ' ModalitaAttuazione
                    Case 3
                        strProg = strProg & "ModalitàFormazioneSpecifica=3, " ' ModalitaAttuazione
                    Case Else
                        strProg = strProg & "ModalitàFormazioneSpecifica=null, " ' ModalitaAttuazione
                End Select

                strProg = strProg & "CompetenzeFormatori=null, " ' CompetenzeSecificheFormatori

                If txtDurataSpec.Text = "" Then
                    strProg = strProg & "Durata=null, " ' Durata
                Else
                    strProg = strProg & "Durata=" & CInt(txtDurataSpec.Text) & ", " ' Durata
                End If
                If txtmodattuazione.Text <> "" Then
                    strProg = strProg & "CodiceEnteModalitàFormazioneSpecifica='" & txtmodattuazione.Text & " ', " 'CodiceEnteModalitàFormazioneSpecifica
                Else
                    strProg = strProg & "CodiceEnteModalitàFormazioneSpecifica=null, " ' CodiceEnteModalitàFormazioneSpecifica
                End If

                If ddlFormazioneSpecificaErogazione.SelectedValue = "unicaTranche" Then
                    strProg = strProg & "TipoFormazioneSpecifica=1, "
                    strProg = strProg & "DurataFormazioneSpecifica1=null, "
                    strProg = strProg & "DurataFormazioneSpecifica2=null "
                End If
                If ddlFormazioneSpecificaErogazione.SelectedValue = "percentuale" Then

                    strProg = strProg & "TipoFormazioneSpecifica=2, "

                    If txt90S.Text = "" Then
                        strProg = strProg & "DurataFormazioneSpecifica1=null,"
                    Else
                        strProg = strProg & "DurataFormazioneSpecifica1=" & CInt(txt90S.Text) & ", "
                    End If

                    If txt270S.Text = "" Then
                        strProg = strProg & "DurataFormazioneSpecifica2=null"
                    Else
                        strProg = strProg & "DurataFormazioneSpecifica2=" & CInt(txt270S.Text) & ""
                    End If

                End If

                strProg = strProg & " where IDAttività=" & CInt(Request.QueryString("IdAttivita"))

                myCommand.CommandText = strProg
                myCommand.ExecuteNonQuery()

                'inserimento su Altri elementi della formazione
                strProg = "update AttivitàAltroFormazione set "
                strProg = strProg & "ModalitàMonitoraggioFormazione=null, " ' ModalitaMonitoraggio
                strProg = strProg & "MonitoraggioFormazionePresentato=0 " ' MonitoraggioPresentato
                strProg = strProg & " where IDAttività=" & CInt(Request.QueryString("IdAttivita"))

                myCommand.CommandText = strProg
                myCommand.ExecuteNonQuery()

                MyTransaction.Commit()
                lblMessaggioConferma.Text = "Progetto salvato con successo."



                ReDim Session("VOperazioniAmbiti")(0)
                Session("VOperazioniAmbiti")(0) = ""
            Catch ex As Exception
                Response.Write(strProg)
                Response.Write("<br/>")
                Response.Write(ex.Message.ToString)
                MyTransaction.Rollback(Session("IdEnte") & "_" & Session("Utente"))
            End Try

            MyTransaction.Dispose()
            MaintainScrollPositionOnPostBack = False
            lblMessaggioConferma.Focus()
        End If
        Return idAttivitaMemorizzata
    End Function

    Private Sub imgSediProgetto_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles imgSediProgetto.Click
        'Response.Redirect("WebGestioneSediProgetto.aspx?VengoDa=" & Request.QueryString("VengoDa") & "&IdAttivita=" & CInt(Request.QueryString("IdAttivita")) & "")
        'se l'immagine dei volontari non è visibile vuol dire che anche nella pagina delle sedi 
        'non dovrò visualizzare i volontari

        Dim strsql As String
        Dim myCommand As SqlClient.SqlCommand
        Dim dtrLocal As SqlClient.SqlDataReader
        Dim blnCapoFila As Boolean

        myCommand = New SqlClient.SqlCommand

        myCommand.Connection = Session("conn")

        strsql = "SELECT IdEntePresentante from attività where idattività=" & Request.QueryString("IdAttivita")

        myCommand.CommandText = strsql
        dtrLocal = myCommand.ExecuteReader

        If dtrLocal.HasRows = True Then
            dtrLocal.Read()
            If Session("IdEnte") <> dtrLocal("IdEntePresentante") Then
                blnCapoFila = False
            Else
                blnCapoFila = True
            End If
        End If

        If Not dtrLocal Is Nothing Then
            dtrLocal.Close()
            dtrLocal = Nothing
        End If

        If Request.QueryString("popup") <> "1" Then

            If imgVolontari.Visible = False Then
                Response.Redirect("WebGestioneSediProgetto.aspx?EnteCapoFila=" & blnCapoFila.ToString & "&CoProgettato=" & chkCoProgettato.Checked.ToString & "&strTitoloProgetto=""&blnVisualizzaVolontari=" & "False" & "&Modifica=" & Request.QueryString("Modifica") & "&Nazionale=" & Request.QueryString("Nazionale") & "&IdAttivita=" & CInt(Request.QueryString("IdAttivita")) & "" & "&VengoDa=" & Request.QueryString("VengoDa"))
            Else
                Response.Redirect("WebGestioneSediProgetto.aspx?EnteCapoFila=" & blnCapoFila.ToString & "&CoProgettato=" & chkCoProgettato.Checked.ToString & "&strTitoloProgetto=""&blnVisualizzaVolontari=" & "True" & "&Modifica=" & Request.QueryString("Modifica") & "&Nazionale=" & Request.QueryString("Nazionale") & "&IdAttivita=" & CInt(Request.QueryString("IdAttivita")) & "" & "&VengoDa=" & Request.QueryString("VengoDa"))
            End If
        Else
            If imgVolontari.Visible = False Then
                Response.Redirect("WebGestioneSediProgetto.aspx?EnteCapoFila=" & blnCapoFila.ToString & "&CoProgettato=" & chkCoProgettato.Checked.ToString & "&popup=" & Request.QueryString("popup") & "&strTitoloProgetto=""&blnVisualizzaVolontari=" & "False" & "&Modifica=" & Request.QueryString("Modifica") & "&Nazionale=" & Request.QueryString("Nazionale") & "&IdAttivita=" & CInt(Request.QueryString("IdAttivita")) & "&VengoDa=" & Request.QueryString("VengoDa"))
            Else
                Response.Redirect("WebGestioneSediProgetto.aspx?EnteCapoFila=" & blnCapoFila.ToString & "&CoProgettato=" & chkCoProgettato.Checked.ToString & "&popup=" & Request.QueryString("popup") & "&strTitoloProgetto=""&blnVisualizzaVolontari=" & "True" & "&Modifica=" & Request.QueryString("Modifica") & "&Nazionale=" & Request.QueryString("Nazionale") & "&IdAttivita=" & CInt(Request.QueryString("IdAttivita")) & "&VengoDa=" & Request.QueryString("VengoDa"))
            End If

        End If
    End Sub

    ''' <summary>
    ''' Procedura per abilitare la modifica dei campi
    ''' </summary>
    ''' <remarks></remarks>
    Sub InModifica()
        ddlEventualiCriteri.Enabled = True
        ddlPianoMonitoraggio.Enabled = True
        ddlModalitaAttuazione.Enabled = True
        ddlmodattuazione.Enabled = True
        txtCodEnteEventuali.Enabled = True
        txtPianoMonitoraggio.Enabled = True
        txtModalitaAttuazione.Enabled = True
        txtmodattuazione.Enabled = True
        txtTitolo.Enabled = True
        txtArea.Enabled = True
        txtNumeroPostiVittoAlloggio.Enabled = True
        txtNumeroPostiSenzaVittoAlloggio.Enabled = True
        txtNumeroPostiSoloVitto.Enabled = True
        txtNumTotVolontaridaImpiegare.Enabled = True
        If (Session("TipoUtente") = "U" Or Session("TipoUtente") = "R") Then
            TxtVittoAlloggioRic.Enabled = True
            TxtNoVittoAlloggioRic.Enabled = True
            TxtVittoRic.Enabled = True
            TxtRiduzioneEnte.Enabled = True
        End If
        txtOreServizioSettimanali.Enabled = True
        txtMonteOreAnnuo.Enabled = True
        txtNumGiorniServizio.Enabled = True
        ddlCreditiFormativiRiconosciuti.Enabled = True
        ddlEventualiTirociniRiconosciuti.Enabled = True
        ddlCompetenzeAcquisibili.Enabled = True
        txtDurata.Enabled = True
        txtDurataSpec.Enabled = True
        ddlCondizioniRischio.Enabled = True
        ddlLivelliSicurezza.Enabled = True
        ddlCondizioniDisagio.Enabled = True
        ddlEventualeAssicurazione.Enabled = True
        txtDataInizioPrevista.Enabled = True
        txtCodiceEnte.Enabled = True
        chkSistemaSelezione.Enabled = True
        chkSistemaMonitoraggioAccreditato.Enabled = True
        txtOrePromozioneSensibilizzazione.Enabled = True
        cmdSalva.Visible = True
        chkVisualizzazione.Value = VALORE_FALSE
        ddlFormazioneGeneraleErogazione.Enabled = True
        ddlFormazioneSpecificaErogazione.Enabled = True
        CheckMonteOreTipo.Enabled = True
    End Sub

    ''' <summary>
    ''' Procedura per disabilitare la modifica dei campi
    ''' </summary>
    ''' <remarks></remarks>
    Sub InVisualizzazione()
        ddlSettore.Enabled = False
        ddlArea.Enabled = False

        ddlEventualiCriteri.Enabled = False
        ddlPianoMonitoraggio.Enabled = False
        ddlModalitaAttuazione.Enabled = False
        ddlmodattuazione.Enabled = False
        txtCodEnteEventuali.Enabled = False
        txtPianoMonitoraggio.Enabled = False
        txtModalitaAttuazione.Enabled = False
        txtmodattuazione.Enabled = False
        txtTitolo.Enabled = False
        txtArea.Enabled = False
        txtNumeroPostiVittoAlloggio.Enabled = False
        txtNumeroPostiSenzaVittoAlloggio.Enabled = False
        txtNumeroPostiSoloVitto.Enabled = False


        If (Session("TipoUtente") = "U" Or Session("TipoUtente") = "R") Then
            TxtVittoAlloggioRic.Enabled = False
            TxtNoVittoAlloggioRic.Enabled = False
            TxtVittoRic.Enabled = False
            TxtRiduzioneEnte.Enabled = False
        End If

        txtNumTotVolontaridaImpiegare.Enabled = False
        txtOreServizioSettimanali.Enabled = False
        txtMonteOreAnnuo.Enabled = False
        txtNumGiorniServizio.Enabled = False

        '****************************
        ' agg. da Simona Cordella il 20/11/2008
        'disabilito option del tipo finanziamento
        OptFinStatale.Enabled = False
        OptFinRegionale.Enabled = False
        OptFinPrivato.Enabled = False
        '****************************
        '*******************************
        ' agg. da Simona Cordella il 11/08/2017
        chkGiovaniMinoriOp.Enabled = False
        ddlDurata.Enabled = False
        optEstero.Enabled = False
        optTutoraggio.Enabled = False
        txtMesiPrevistiUE.Enabled = False
        txtMesiPrevistiTutoraggio.Enabled = False
        lkbClearOptEUE_T.Visible = False
        '*******************************

        ddlCreditiFormativiRiconosciuti.Enabled = False
        ddlEventualiTirociniRiconosciuti.Enabled = False
        ddlCompetenzeAcquisibili.Enabled = False
        txtDurata.Enabled = False
        txtDurataSpec.Enabled = False
        ddlCondizioniRischio.Enabled = False
        ddlLivelliSicurezza.Enabled = False
        ddlCondizioniDisagio.Enabled = False
        ddlEventualeAssicurazione.Enabled = False
        txtDataInizioPrevista.Enabled = False
        txtCodiceEnte.Enabled = False
        chkSistemaSelezione.Enabled = False
        chkSistemaMonitoraggioAccreditato.Enabled = False
        txtOrePromozioneSensibilizzazione.Enabled = False
        cmdSalva.Visible = False
        ImgDuplica.Visible = False '29/09/2014 sempre invisibile(il duplica veniva usato per i progetti di GG)
        chkCoProgettato.Enabled = False
        'mod il 23/01/2013 se il progetto è in coporogettazione abilito il pulsante della coprogettazione (in lettura)
        If chkCoProgettato.Checked = False Then
            imgCoProgettazione.Visible = False
            imgElencoDocumentiProg.Visible = True
            divCoProgettazione.Visible = True
        Else

            imgCoProgettazione.Visible = True
            imgElencoDocumentiProg.Visible = False
            divCoProgettazione.Visible = False
        End If
        chkVisualizzazione.Value = VALORE_TRUE
        ddlFormazioneGeneraleErogazione.Enabled = False
        ddlFormazioneSpecificaErogazione.Enabled = False
        CheckMonteOreTipo.Enabled = False
    End Sub

    Private Sub imgRisorse_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles imgRisorse.Click
        Dim strsql As String
        Dim myCommand As SqlClient.SqlCommand
        Dim dtrLocal As SqlClient.SqlDataReader
        Dim blnCapoFila As Boolean

        myCommand = New SqlClient.SqlCommand

        myCommand.Connection = Session("conn")

        strsql = "SELECT IdEntePresentante from attività where idattività=" & Request.QueryString("IdAttivita")

        myCommand.CommandText = strsql
        dtrLocal = myCommand.ExecuteReader

        If dtrLocal.HasRows = True Then
            dtrLocal.Read()
            If Session("IdEnte") <> dtrLocal("IdEntePresentante") Then
                blnCapoFila = False
            Else
                blnCapoFila = True
            End If
        End If

        If Not dtrLocal Is Nothing Then
            dtrLocal.Close()
            dtrLocal = Nothing
        End If
        Response.Redirect("WebGestioneSediProgettoOlp.aspx?EnteCapoFila=" & blnCapoFila.ToString & "&CoProgettato=" & chkCoProgettato.Checked.ToString & "&popup=" & Request.QueryString("popup") & "&Modifica=" & Request.QueryString("Modifica") & "&Nazionale=" & Request.QueryString("Nazionale") & "&IdAttivita=" & CInt(Request.QueryString("IdAttivita")) & "&VengoDa=" & Request.QueryString("VengoDa"))
    End Sub

    Private Sub imgCancella_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles imgCancella.Click
        Dim strLocal As String
        Dim mycommand As New SqlClient.SqlCommand
        mycommand.Connection = Session("conn")
        'update stato attività
        strLocal = "update attività set IDStatoAttività=(select IdStatoAttività from StatiAttività where Cancellata=1) where idattività=" & CInt(Request.QueryString("IdAttivita"))
        mycommand.CommandText = strLocal
        mycommand.ExecuteNonQuery()
        InVisualizzazione()
        lblMessaggioConferma.Text = "L'Attività è stata correttamente cancellata.<br/>"
        'stampo a pagina tutte le hidden che ho utilizzato al primo load della pagina
        'così mi porto letteralmente dietro i valori
        txtIdAmbitoAttivita.Value = txtIdAreaPrincipale.Value
        strIdAmbitiModifica.Value = txtIdAree.Value
        txtIdAttivitaCaratteristicaOrganizzativa.Value = txtIdAttOrganizzativa.Value
        txtIdAttivitaFormazioneSpecifica.Value = txtIdFormSpecifica.Value
        txtIdAttivitaAltroFormazione.Value = txtIdAltro.Value
        txtCodAttivita.Value = txtCodAtt.Value
        strIdMacroAmbito.Value = Request.Form("strIdMacroAttivita")

    End Sub

    Private Sub imgVolontari_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles imgVolontari.Click
        Response.Redirect("elencovolontariprogetto.aspx?popup=" & Request.QueryString("popup") & "&VengoDa=" & "Progetti" & "&strTitoloProgetto=" & Trim(txtTitolo.Text).Replace("#", "") & "&Modifica=" & Request.QueryString("Modifica") & "&Nazionale=" & Request.QueryString("Nazionale") & "&IdAttivita=" & CInt(Request.QueryString("IdAttivita")))
    End Sub

    Private Sub imgSediAssegnazione_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles imgSediAssegnazione.Click
        Response.Redirect("elencosediassegnazione.aspx?popup=" & Request.QueryString("popup") & "&VengoDa=" & "Progetti" & "&strTitoloProgetto=" & Trim(txtTitolo.Text) & "&Modifica=" & Request.QueryString("Modifica") & "&Nazionale=" & Request.QueryString("Nazionale") & "&IdAttivita=" & CInt(Request.QueryString("IdAttivita")))
    End Sub


    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub

    Private Sub imgRipristina_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles imgRipristina.Click
        Dim strLocal As String
        Dim mycommand As New SqlClient.SqlCommand
        ReimpostaLabelMessaggi()
        mycommand.Connection = Session("conn")
        'update stato attività
        strLocal = "update attività set IDStatoAttività=(select IdStatoAttività from StatiAttività where defaultstato=1) where idattività=" & CInt(Request.QueryString("IdAttivita"))
        mycommand.CommandText = strLocal
        mycommand.ExecuteNonQuery()
        InModifica()
        lblMessaggioConferma.Text = "Attività Ripristinata con successo."
        'stampo a pagina tutte le hidden che ho utilizzato al primo load della pagina
        'così mi porto letteralmente dietro i valori
        txtIdAmbitoAttivita.Value = txtIdAreaPrincipale.Value
        strIdAmbitiModifica.Value = txtIdAree.Value
        txtIdAttivitaCaratteristicaOrganizzativa.Value = txtIdAttOrganizzativa.Value
        txtIdAttivitaFormazioneSpecifica.Value = txtIdFormSpecifica.Value
        txtIdAttivitaAltroFormazione.Value = txtIdAltro.Value
        txtCodAttivita.Value = txtCodAtt.Value
        strIdMacroAmbito.Value = Request.Form("strIdMacroAttivita")
    End Sub

    ''' <summary>
    ''' Collegamento del progetto ad un'istanza
    ''' codice generato da sfronatan il 10.10.2005
    ''' controllo se presente l'istanza e il suo stato
    ''' se presente e presentata chiamo la routine LegaProgettoIstanzaEsistentePresentata
    ''' se presente e non presentata chiamo la routine LegaProgettoIstanzaEsistenteNonConfermata
    ''' se non presente chiamo la routine CreaIstanzaLegaProgetto
    ''' </summary>
    Private Sub imgCollegaIstanza_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles imgCollegaIstanza.Click
        ReimpostaLabelMessaggi()
        Dim dtrLocal As SqlClient.SqlDataReader
        Dim myCommand As New System.Data.SqlClient.SqlCommand
        Dim strCheckIstanza As String
        Dim strIdBandoAttivita As String
        Dim strStatoBandoAttivita As String
        'variabile che uso per verificare se il progetto è ancora modificabile o meno
        '0 = non modificabile
        '1 = modificabile
        Dim strCheckModifica As String
        Dim strSql As String = String.Empty
        'idregione di competenza
        Dim strIdRegioneCompetenzaUtente As String
        myCommand.Connection = Session("conn")
        If Session("TipoUtente") = "R" Or Session("TipoUtente") = "U" Then
            strSql = "SELECT UtentiUNSC.IdRegioneCompetenza "
            strSql = strSql & "FROM UtentiUNSC "
            strSql = strSql & "WHERE  (UtentiUNSC.UserName = '" & Session("Utente") & "')"
        End If

        myCommand.CommandText = strSql
        dtrLocal = myCommand.ExecuteReader

        If dtrLocal.HasRows = True Then
            dtrLocal.Read()
            strIdRegioneCompetenzaUtente = dtrLocal("IdRegioneCompetenza")
        End If
        ChiudiDataReader(dtrLocal)

        'vado a controllare se presente istanza
        strCheckIstanza = "select idbandoattività,statibandiattività.statobandoattività from bandiattività "
        strCheckIstanza = strCheckIstanza & "inner join statibandiattività on bandiattività.idstatobandoattività=statibandiattività.idstatobandoattività "
        strCheckIstanza = strCheckIstanza & "where bandiattività.idente='" & Session("IdEnte") & "'"
        strCheckIstanza = strCheckIstanza & "and idbando = (select bando.idbando from bando INNER JOIN AssociaBandoRegioniCompetenze ON Bando.idbando=AssociaBandoRegioniCompetenze.idbando "
        strCheckIstanza = strCheckIstanza & "where bando.AssociazioneAutomatica=1 and AssociaBandoRegioniCompetenze.idregionecompetenza='" & strIdRegioneCompetenzaUtente & "') "

        myCommand.CommandText = strCheckIstanza
        dtrLocal = myCommand.ExecuteReader
        If dtrLocal.HasRows = True Then
            dtrLocal.Read()
            strIdBandoAttivita = dtrLocal("idbandoattività")
            strStatoBandoAttivita = dtrLocal("statobandoattività")
            ChiudiDataReader(dtrLocal)
            'controllo se lo stato dell'istanza è "presentata"
            If strStatoBandoAttivita = "Presentata" Then
                LegaProgettoIstanzaEsistentePresentata(strIdBandoAttivita, Request.QueryString("IdAttivita"))
                strCheckModifica = "0"
                errore = "0"
            ElseIf strStatoBandoAttivita = "Registrata" Then 'l'istanza non è presentata ma è registrata
                LegaProgettoIstanzaEsistenteNonConfermata(strIdBandoAttivita, Request.QueryString("IdAttivita"))
                strCheckModifica = "1"
                errore = "0"
            ElseIf strStatoBandoAttivita = "Approvata" Then
                LegaProgettoIstanzaEsistentePresentata(strIdBandoAttivita, Request.QueryString("IdAttivita"))
                strCheckModifica = "0"
                errore = "0"
            ElseIf strStatoBandoAttivita = "Inammissibile" Then 'l'istanza è inammissibile
                errore = "1"
                strCheckModifica = "1"
            End If
        Else
            ChiudiDataReader(dtrLocal)
            CreaIstanzaLegaProgetto(strIdRegioneCompetenzaUtente, Request.QueryString("IdAttivita"))
            strCheckModifica = "0"
        End If
        ChiudiDataReader(dtrLocal)
        If errore = "1" Then
            lblerrore.Text = "Impossibile procedere con l'operazione, l'istanza di riferimento risulta esistente ma INAMMISSIBILE. Correggere lo stato dell'istanza e ripetere l'operazione."
        End If

    End Sub


    ''' <summary>
    ''' Funzione per il controllo dello stato del progetto 
    ''' agli utenti UNSC viende data la possibilità di collegare il progetto ad un'istanza
    '''lo stato del progetto deve essere registrato true=registrato  false<>registrato
    ''' </summary>
    ''' <param name="IdAttivita"></param>
    ''' <returns > true se registrato false altrimenti</returns>


    ''' <summary>
    ''' Procedura per collegare un progetto ad una istanza già presente
    ''' </summary>
    ''' <param name="IdregioneCompetenzaUtente"></param>
    ''' <param name="IdAttivita"></param>
    ''' <remarks></remarks>
    Sub CreaIstanzaLegaProgetto(ByVal IdregioneCompetenzaUtente As String, ByVal IdAttivita As String)
        Dim strsql As String
        Dim cmdinsert As Data.SqlClient.SqlCommand
        Dim dtrLocal As SqlClient.SqlDataReader
        Dim myCommand As New System.Data.SqlClient.SqlCommand
        Dim strIdBandoAttivita As String
        Dim strIdMax As String
        myCommand.Connection = Session("conn")

        strsql = "insert into "
        strsql = strsql & "bandiattività(idstatobandoattività, idbando,idente, "
        strsql = strsql & "UsernameInseritore, DataCreazioneRecord,UsernamePresentazione,DataPresentazione) "
        strsql = strsql & "select idstatobandoattività,(select bando.idbando from bando INNER JOIN AssociaBandoRegioniCompetenze ON Bando.idbando=AssociaBandoRegioniCompetenze.idbando "
        strsql = strsql & "where bando.AssociazioneAutomatica=1 and AssociaBandoRegioniCompetenze.IdRegioneCompetenza='" & IdregioneCompetenzaUtente & "'),'" & Session("idente") & "','"
        strsql = strsql & Session("Utente") & "',getdate() ,'" & Session("Utente") & "', getdate()"
        strsql = strsql & "from statibandiattività where davalutare=1"

        cmdinsert = New Data.SqlClient.SqlCommand(strsql, Session("conn"))
        cmdinsert.ExecuteNonQuery()
        cmdinsert.Dispose()

        strsql = "select @@identity as strIdMax"

        myCommand.CommandText = strsql
        dtrLocal = myCommand.ExecuteReader

        If dtrLocal.HasRows = True Then
            dtrLocal.Read()
            strIdMax = dtrLocal("strIdMax")
        End If
        ChiudiDataReader(dtrLocal)

        'insert into cronologia
        strsql = "insert into CronologiaAttività "
        strsql = strsql & "(idattività,idstatoattività,datacronologia,idTipoCronologia, "
        strsql = strsql & "usernameaccreditatore) "
        strsql = strsql & "select '" & IdAttivita & "', "
        strsql = strsql & "idstatoattività,getdate(),0,'" & ClsServer.NoApice(Session("Utente")) & "'"
        strsql = strsql & "from attività where idattività='" & IdAttivita & "'"

        cmdinsert = New Data.SqlClient.SqlCommand(strsql, Session("conn"))
        cmdinsert.ExecuteNonQuery()
        cmdinsert.Dispose()

        'update stato attività
        strsql = "update attività "
        strsql = strsql & "set idstatoattività=(select idstatoattività from statiattività "
        strsql = strsql & "where Davalutare=1),idbandoattività="
        strsql = strsql & strIdMax & ", "
        strsql = strsql & "dataultimostato=getdate(), "
        strsql = strsql & "usernamestato='" & ClsServer.NoApice(Session("Utente")) & "',"
        strsql = strsql & "NumeroPostiNoVittoNoAlloggioRic = NumeroPostiNoVittoNoAlloggio, NumeroPostiVittoAlloggioRic = NumeroPostiVittoAlloggio, NumeroPostiVittoRic = NumeroPostiVitto,"
        strsql = strsql & "DataPresentazioneProgetto=getdate() "
        strsql = strsql & "where idattività='" & IdAttivita & "'"

        cmdinsert = New Data.SqlClient.SqlCommand(strsql, Session("conn"))
        cmdinsert.ExecuteNonQuery()
        cmdinsert.Dispose()
        ChiudiDataReader(dtrLocal)
        'update stato attività
        strsql = "select IdBandoAttività from attività "
        strsql = strsql & "where idattività='" & IdAttivita & "'"

        myCommand.CommandText = strsql
        dtrLocal = myCommand.ExecuteReader
        If dtrLocal.HasRows = True Then
            dtrLocal.Read()
            strIdBandoAttivita = dtrLocal("IdBandoAttività")
        End If
        ChiudiDataReader(dtrLocal)
        ClsServer.EseguiStoreGeneraCodiciProgetto(strIdBandoAttivita, "SP_GENERA_CODICI_PROGETTO", Session("conn"))

    End Sub

    Sub LegaProgettoIstanzaEsistentePresentata(ByVal IdbandoAttivita As String, ByVal Idattivita As String)
        Dim strsql As String
        '***Procedo con l'inserimento dell'istanza e modifico lo stato dell'attività
        Dim cmdinsert As Data.SqlClient.SqlCommand

        strsql = "update attività "
        strsql = strsql & "set idstatoattività=(select idstatoattività from statiattività "
        strsql = strsql & "where Davalutare=1),idbandoattività= "
        strsql = strsql & "'" & IdbandoAttivita & "', "
        strsql = strsql & "dataultimostato=getdate(), "
        strsql = strsql & "DataPresentazioneProgetto=getdate(), "
        strsql = strsql & "NumeroPostiNoVittoNoAlloggioRic = NumeroPostiNoVittoNoAlloggio, NumeroPostiVittoAlloggioRic = NumeroPostiVittoAlloggio, NumeroPostiVittoRic = NumeroPostiVitto,"
        strsql = strsql & "usernamestato='" & ClsServer.NoApice(Session("Utente")) & "'"
        strsql = strsql & "where idattività='" & Idattivita & "'"

        cmdinsert = New Data.SqlClient.SqlCommand(strsql, Session("conn"))
        cmdinsert.ExecuteNonQuery()
        cmdinsert.Dispose()
        ClsServer.EseguiStoreGeneraCodiciProgetto(IdbandoAttivita, "SP_GENERA_CODICI_PROGETTO", Session("conn"))

    End Sub

    Sub LegaProgettoIstanzaEsistenteNonConfermata(ByVal IdbandoAttivita As String, ByVal Idattivita As String)
        Dim strsql As String
        '***Procedo con l'inserimento dell'istanza e modifico lo stato dell'attività
        Dim cmdinsert As Data.SqlClient.SqlCommand
        ReimpostaLabelMessaggi()
        strsql = "update attività "
        strsql = strsql & "set idstatoattività=(select idstatoattività from statiattività "
        strsql = strsql & "where Davalutare=1),idbandoattività= "
        strsql = strsql & "'" & IdbandoAttivita & "', "
        strsql = strsql & "dataultimostato=getdate(), "
        strsql = strsql & "usernamestato='" & ClsServer.NoApice(Session("Utente")) & "'"
        strsql = strsql & "where idattività='" & Idattivita & "'"
        cmdinsert = New Data.SqlClient.SqlCommand(strsql, Session("conn"))
        cmdinsert.ExecuteNonQuery()
        cmdinsert.Dispose()
        lblerrore.Text = "Il progetto e' stato legato all'istanza. <br/>"
    End Sub

    Private Sub imgRevisioneProgetto_Click(ByVal sender As Object, ByVal e As EventArgs) Handles imgRevisioneProgetto.Click
        Dim sEsito As String
        ReimpostaLabelMessaggi()

        sEsito = ClsServer.EseguiStoreRevisioneProegtti(CInt(Request.QueryString("IdAttivita")), CStr(Session("Utente")), Session("conn"))

        If sEsito = "ERRORE" Then
            lblerrore.Text = "Errore imprevisto durante l'operazione di 'Revisione Progetto'. Contattare l'assistenza Helios/Futuro."
        Else
            lblMessaggioConferma.Text = "Revisione Progetto terminata con successo."
        End If

    End Sub

    Private Sub imgRicorso_Click(ByVal sender As Object, ByVal e As EventArgs) Handles imgRicorso.Click
        Dim sEsito As String
        ReimpostaLabelMessaggi()

        sEsito = ClsServer.EseguiStoreRicorsoProgetto(CInt(Request.QueryString("IdAttivita")), CStr(Session("Utente")), Session("conn"))

        If sEsito = "ERRORE" Then
            lblerrore.Text = "Errore imprevisto durante l'operazione di 'Ricorso Progetto'. Contattare l'assistenza Helios/Futuro."
        Else
            lblMessaggioConferma.Text = "Ricorso Progetto terminato con successo."
        End If

    End Sub

    Private Sub AbilitaRicorsoProgetto(ByVal IdAttivita As Integer)
        'DESCRIZIONE: 
        '1- Verifica se l'utente ha le abilitazioni per effettuare la duplicazione del progetto per il ricorso 
        '2-verifica che il progetto corrente non ha già un ricorso
        '3-verifica che il bando del progetto corrente abbia associato un bando ricorso con stato aperto
        '4-verifica statoattività del progetto (possono fare ricorso solo gli stati 7,6 e 1)
        Dim strsql As String
        ChiudiDataReader(dtrgenerico)

        '***PRIMA VERIFICA***
        strsql = "SELECT AssociaUtenteGruppo.username, VociMenu.IdVoceMenu, VociMenu.VoceMenu, VociMenu.ImmaginePassiva, VociMenu.ImmagineAttiva, VociMenu.Link," & _
                 " VociMenu.IdVoceMenuPadre" & _
                 " FROM VociMenu" & _
                 " INNER JOIN 	AbilitazioniProfiliVociMenu ON VociMenu.IdVoceMenu = AbilitazioniProfiliVociMenu.IdVoceMenu" & _
                 " INNER JOIN	Profili ON AbilitazioniProfiliVociMenu.IdProfilo = Profili.IdProfilo"

        If UCase(Me.TemplateSourceDirectory) <> "/HELIOSREAD" Then
            strsql = strsql & " INNER JOIN	AssociaUtenteGruppo ON Profili.IdProfilo = AssociaUtenteGruppo.IdProfilo "
        Else
            strsql = strsql & " INNER JOIN	AssociaUtenteGruppo ON Profili.IdProfilo = AssociaUtenteGruppo.IdProfiloRead "
        End If

        strsql = strsql & " LEFT JOIN 	RestrizioniUtenteVociMenu ON AssociaUtenteGruppo.UserName = RestrizioniUtenteVociMenu.UserName and RestrizioniUtenteVociMenu.IdVoceMenu=VociMenu.IdVoceMenu" & _
                 " WHERE VociMenu.descrizione = 'Forza Ricorso Progetto'" & _
                 " AND AssociaUtenteGruppo.username ='" & Session("Utente") & "' and (RestrizioniUtenteVociMenu.TipoRestrizione <> 0 or RestrizioniUtenteVociMenu.TipoRestrizione is null)"
        dtrgenerico = ClsServer.CreaDatareader(strsql, Session("conn"))

        If dtrgenerico.HasRows = True Then
            '***SECONDA VERIFICA***
            dtrgenerico.Close()
            dtrgenerico = Nothing

            strsql = "select idattivitàricorso from attività where idattivitàricorso=" & IdAttivita
            dtrgenerico = ClsServer.CreaDatareader(strsql, Session("conn"))

            If dtrgenerico.HasRows = False Then
                '***TERZA VERIFICA***
                dtrgenerico.Close()
                dtrgenerico = Nothing

                strsql = "SELECT statiBando.* FROM attività INNER JOIN " _
                      & " BandiAttività ON attività.IDBandoAttività = BandiAttività.IdBandoAttività INNER JOIN " _
                      & " bando ON BandiAttività.IdBando = bando.IDBando INNER JOIN " _
                      & " bando bando_1 ON bando.IDBando = bando_1.IDBandoRicorso INNER JOIN " _
                      & " statiBando ON bando_1.IDStatoBando = statiBando.IDStatoBando " _
                      & " WHERE (attività.IDAttività = " & IdAttivita & ") AND (statiBando.InValutazione = 1)"
                dtrgenerico = ClsServer.CreaDatareader(strsql, Session("conn"))

                If dtrgenerico.HasRows = True Then
                    '***QUARTA VERIFICA***
                    dtrgenerico.Close()
                    dtrgenerico = Nothing

                    strsql = "Select IDAttività FROM attività " _
                           & " WHERE  IDStatoAttività IN (7, 6, 1) AND IDAttività =" & IdAttivita
                    dtrgenerico = ClsServer.CreaDatareader(strsql, Session("conn"))

                    If dtrgenerico.HasRows = True Then
                        'Abilito il pulsante di revisione solo se passa tutti e 4 i controlli    
                        imgRicorso.Visible = True
                    End If
                End If
            End If
        End If
        ChiudiDataReader(dtrgenerico)
    End Sub

    Private Sub AbilitaRevisioneProgetto(ByVal IdAttivita As Integer)
        'DESCRIZIONE: 
        '1- Verifica se l'utente ha le abilitazioni per effettuare la revisione progetto
        '2-verifica che il progetto corrente non ha già una revisione
        '3-verifica che il bando del progetto corrente abbia associato un bando revisione con stato aperto
        '4-verifica statoattività del progetto (possono fare la revisione solo gli stati 7,6 e 1)
        'DATA: 13/06/2006
        Dim strsql As String
        ChiudiDataReader(dtrgenerico)

        '***PRIMA VERIFICA***
        strsql = "SELECT AssociaUtenteGruppo.username, VociMenu.IdVoceMenu, VociMenu.VoceMenu, VociMenu.ImmaginePassiva, VociMenu.ImmagineAttiva, VociMenu.Link," & _
                 " VociMenu.IdVoceMenuPadre" & _
                 " FROM VociMenu" & _
                 " INNER JOIN 	AbilitazioniProfiliVociMenu ON VociMenu.IdVoceMenu = AbilitazioniProfiliVociMenu.IdVoceMenu" & _
                 " INNER JOIN	Profili ON AbilitazioniProfiliVociMenu.IdProfilo = Profili.IdProfilo"

        If UCase(Me.TemplateSourceDirectory) <> "/HELIOSREAD" Then
            strsql = strsql & " INNER JOIN	AssociaUtenteGruppo ON Profili.IdProfilo = AssociaUtenteGruppo.IdProfilo "
        Else
            strsql = strsql & " INNER JOIN	AssociaUtenteGruppo ON Profili.IdProfilo = AssociaUtenteGruppo.IdProfiloRead "
        End If

        strsql = strsql & " LEFT JOIN 	RestrizioniUtenteVociMenu ON AssociaUtenteGruppo.UserName = RestrizioniUtenteVociMenu.UserName and RestrizioniUtenteVociMenu.IdVoceMenu=VociMenu.IdVoceMenu" & _
                 " WHERE VociMenu.descrizione = 'Forza Revisione Progetto'" & _
                 " AND AssociaUtenteGruppo.username ='" & Session("Utente") & "' and (RestrizioniUtenteVociMenu.TipoRestrizione <> 0 or RestrizioniUtenteVociMenu.TipoRestrizione is null)"
        dtrgenerico = ClsServer.CreaDatareader(strsql, Session("conn"))

        If dtrgenerico.HasRows = True Then
            '***SECONDA VERIFICA***
            dtrgenerico.Close()
            dtrgenerico = Nothing

            strsql = "select idattivitàrevisionata from attività where idattivitàrevisionata=" & IdAttivita
            dtrgenerico = ClsServer.CreaDatareader(strsql, Session("conn"))

            If dtrgenerico.HasRows = False Then
                '***TERZA VERIFICA***
                dtrgenerico.Close()
                dtrgenerico = Nothing

                strsql = "SELECT statiBando.* FROM attività INNER JOIN " _
                      & " BandiAttività ON attività.IDBandoAttività = BandiAttività.IdBandoAttività INNER JOIN " _
                      & " bando ON BandiAttività.IdBando = bando.IDBando INNER JOIN " _
                      & " bando bando_1 ON bando.IDBando = bando_1.IDBandoRevisionato INNER JOIN " _
                      & " statiBando ON bando_1.IDStatoBando = statiBando.IDStatoBando " _
                      & " WHERE (attività.IDAttività = " & IdAttivita & ") AND (statiBando.InValutazione = 1)"
                dtrgenerico = ClsServer.CreaDatareader(strsql, Session("conn"))

                If dtrgenerico.HasRows = True Then
                    '***QUARTA VERIFICA***
                    dtrgenerico.Close()
                    dtrgenerico = Nothing

                    strsql = "Select IDAttività FROM attività " _
                           & " WHERE  IDStatoAttività IN (7, 6, 1) AND IDAttività =" & IdAttivita
                    dtrgenerico = ClsServer.CreaDatareader(strsql, Session("conn"))

                    If dtrgenerico.HasRows = True Then
                        'Abilito il pulsante di revisione solo se passa tutti e 4 i controlli    
                        imgRevisioneProgetto.Visible = True
                    End If
                End If
            End If
        End If
        ChiudiDataReader(dtrgenerico)
    End Sub

    Private Sub imgCoProgettazione_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles imgCoProgettazione.Click
        Response.Redirect("Wfrm_Cooprogettazione.aspx?CoProgettato=" & chkCoProgettato.Checked.ToString & "&popup=" & Request.QueryString("popup") & "&Modifica=" & Request.QueryString("Modifica") & "&Nazionale=" & Request.QueryString("Nazionale") & "&IdAttivita=" & CInt(Request.QueryString("IdAttivita")) & "&VengoDa=" & Request.QueryString("VengoDa"))
    End Sub

    Private Sub imgSediCertificate_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles imgSediCertificate.Click
        If Request.QueryString("popup") = "1" Then
            Dim strsql As String
            Dim myCommand As SqlClient.SqlCommand
            Dim dtrLocal As SqlClient.SqlDataReader
            Dim blnCapoFila As Boolean

            myCommand = New SqlClient.SqlCommand
            myCommand.Connection = Session("conn")
            strsql = "SELECT IdEntePresentante from attività where idattività=" & Request.QueryString("IdAttivita")
            myCommand.CommandText = strsql
            dtrLocal = myCommand.ExecuteReader

            If dtrLocal.HasRows = True Then
                dtrLocal.Read()
                If Session("IdEnte") <> dtrLocal("IdEntePresentante") Then
                    blnCapoFila = False
                Else
                    blnCapoFila = True
                End If
            End If

            If Not dtrLocal Is Nothing Then
                dtrLocal.Close()
                dtrLocal = Nothing
            End If

            Response.Redirect("WfrmRicercaSediCertificate.aspx?PopSede=1&EnteCapoFila=" & blnCapoFila.ToString & "&popup=" & Request.QueryString("popup") & "&strTitoloProgetto=" & Trim(txtTitolo.Text) & "&blnVisualizzaVolontari=" & "False" & "&Modifica=" & Request.QueryString("Modifica") & "&Nazionale=" & Request.QueryString("Nazionale") & "&IdAttivita=" & CInt(Request.QueryString("IdAttivita")) & "&VengoDa=" & Request.QueryString("VengoDa"))
        End If
    End Sub

    Private Sub imgRiepilogoDocumentiVol_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles imgAssociaDocumentiProg.Click
        Response.Write("<script>")
        Response.Write("window.open('WfrmAssociaDocumentiProgetti.aspx?IdAttivita=" & Request.QueryString("IdAttivita") & "&VengoDa=" & Request.QueryString("VengoDa") & "', 'AssociaDocumenti', 'width=900, height=600,dependent=no,scrollbars=yes,status=yes')")
        Response.Write("</script>")
    End Sub

    Private Sub imgElencoDocumentiProg_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles imgElencoDocumentiProg.Click
        Response.Redirect("wfrmDocumentiProgetto.aspx?Modifica=" & Request.QueryString("Modifica") & "&Nazionale=" & Request.QueryString("Nazionale") & "&IdAttivita=" & CInt(Request.QueryString("IdAttivita")) & "&VengoDa=" & Request.QueryString("VengoDa"))
    End Sub


    Private Sub ImgApplicaStatoValutazione_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles ImgApplicaStatoValutazione.Click
        Dim strProg As String
        Dim myCommand As New System.Data.SqlClient.SqlCommand
        myCommand.Connection = Session("conn")
        strProg = "UpDate Attività Set StatoValutazione =" & ddlStatoValutazione.SelectedValue & ""
        strProg = strProg & " where IDAttività=" & CInt(Request.QueryString("IdAttivita"))
        myCommand.CommandText = strProg
        myCommand.ExecuteNonQuery()
    End Sub

    Private Sub ImgDuplica_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles ImgDuplica.Click
        ReimpostaLabelMessaggi()
        lblMessaggioConferma.Text = LoadStoreDuplicaProgetti(Request.QueryString("IdAttivita"), Session("Utente"))
    End Sub
    Private Function LoadStoreDuplicaProgetti(ByVal IdAttivita As Integer, ByVal UserName As String) As String
        Dim sqlCommand As New SqlClient.SqlCommand
        Dim progettoDuplicato As Boolean
        Dim esito As String = String.Empty

        Try
            sqlCommand.CommandText = "SP_DUPLICA_PROGETTO_GG"
            sqlCommand.CommandType = CommandType.StoredProcedure
            sqlCommand.Connection = Session("conn")
            sqlCommand.Parameters.AddWithValue("@IdAttivita", IdAttivita)
            sqlCommand.Parameters.AddWithValue("@UserName", UserName)
            sqlCommand.Parameters.Add("@Esito", SqlDbType.VarChar, 1000)
            sqlCommand.Parameters("@Esito").Direction = ParameterDirection.Output
            sqlCommand.ExecuteNonQuery()
            progettoDuplicato = sqlCommand.Parameters("@Esito").Value()
            esito = sqlCommand.Parameters("@Esito").Value()
        Catch ex As Exception
            progettoDuplicato = False
        Finally
            If progettoDuplicato = True Then
                lblMessaggioConferma.Text = esito
            Else
                lblerrore.Text = esito
            End If
        End Try
        Return progettoDuplicato
    End Function

    Private Function AbilitaDuplicaProgetto(ByVal IdAttivita As Integer, ByVal TipoProgetto As Integer) As Boolean

        Dim strSql As String
        Dim rtsProg As SqlClient.SqlDataReader
        Dim strStatoIstanza As String
        Dim strStatoProgetto As String

        strSql = " SELECT statoattività,isnull(StatoBandoAttività,'Non presente') as StatoBandoAttività  " & _
                 " FROM attività  inner join statiattività on attività.idstatoattività = statiattività.idstatoattività " & _
                 " left join bandiattività on attività.idbandoattività = bandiattività.idbandoattività " & _
                 " left join statiBandiAttività on bandiattività.IdStatoBandoAttività=statiBandiAttività.IdStatoBandoAttività " & _
                 " WHERE attività.IDAttività = " & IdAttivita
        rtsProg = ClsServer.CreaDatareader(strSql, Session("Conn"))
        If rtsProg.HasRows = True Then
            rtsProg.Read()

            strStatoIstanza = "" & rtsProg("StatoBandoAttività")
            strStatoProgetto = "" & rtsProg("statoattività")
            If Not rtsProg Is Nothing Then
                rtsProg.Close()
                rtsProg = Nothing
            End If
            'verifico statoProgetto 
            '--> =  REGISTRATO 
            '--> <> REGISTRATO --> STATOISTANZA DEVE ESSERE REGISTRATA
            If TipoProgetto = 1 Then
                If strStatoProgetto = "Registrato" Then
                    Return True
                Else
                    If strStatoIstanza = "Registrata" Then
                        Return True
                    Else
                        Return False
                    End If
                End If
            Else
                Return False
            End If

        End If
        If Not rtsProg Is Nothing Then
            rtsProg.Close()
            rtsProg = Nothing
        End If
    End Function

    Protected Sub imgChiudi_Click(ByVal sender As Object, ByVal e As EventArgs) Handles imgChiudi.Click
        If (Request.QueryString("VengoDa") = Costanti.VENGO_DA_ACCETTAZIONE_PROGETTI) Then
            Response.Redirect("assegnazionevincoliprogetti.aspx?idattivita=" & Request.QueryString("idattivita") & "&tipologia=ProgettiValutare")
        ElseIf (Request.QueryString("VengoDa") = Costanti.VENGO_DA_VALUTAZIONE_QUALITA) Then
            Response.Redirect(ClsUtility.RitornaMascheraValutazioneProgetto(Request.QueryString("idattivita"), Session("conn")) & "?idprogetto=" & Request.QueryString("idattivita"))
        Else
            Response.Redirect("WfrmMain.aspx")
        End If

    End Sub



    Private Sub ddlSettore_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlSettore.SelectedIndexChanged
        lblMessaggioConferma.Text = String.Empty

        If ddlSettore.SelectedValue <> "0" Then

            Dim strIdattivita As String
            strIdattivita = Request.QueryString("IdAttivita")
            If ClsUtility.SETTORI_VERIFICA(Session("conn"), strIdattivita, ddlSettore.SelectedValue) = False Then
                lblerroreSettore.Text = "Il settore selezionato non &#232; congruo con quello indicato in fase di accreditamento."
                'CaricaComboAreaPrimaVolta()
                MaintainScrollPositionOnPostBack = False
                lblerrore.Focus()
                Exit Sub
            Else
                If (lblerroreSettore.Text <> String.Empty) Then
                    MaintainScrollPositionOnPostBack = False
                Else
                    MaintainScrollPositionOnPostBack = True
                End If
                lblerroreSettore.Text = String.Empty
            End If
            dtgAmbiti.CurrentPageIndex = 0
            txtArea.Text = String.Empty
            CancellaAmbitiSecondari()
            CaricaComboArea(CInt(ddlSettore.SelectedValue))
            txtIdAreaPrincipale.Value = ddlSettore.SelectedValue
            ddlArea.Enabled = True

            If (DivSelezionaAltriAmbiti.Visible = True) Then
                DivSelezionaAltriAmbiti.Visible = True
                CaricaArea(ddlSettore.SelectedIndex)
                CaricaAmbitiModifica(CInt(Request.QueryString("IdAttivita")))
                ddlSettore.Focus()

            End If



        Else
            CaricaComboArea(0)
            lblerrore.Text = String.Empty
            lblareaerrore.Text = String.Empty
            ddlArea.Enabled = False
            DivSelezionaAltriAmbiti.Visible = False
            If (lblerroreSettore.Text <> String.Empty) Then
                MaintainScrollPositionOnPostBack = False
            Else
                MaintainScrollPositionOnPostBack = True
            End If
            lblerroreSettore.Text = String.Empty
            'ValidaCampi()
        End If
        ddlSettore.Focus()

    End Sub

    Private Sub ddlArea_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlArea.SelectedIndexChanged
        lblMessaggioConferma.Text = String.Empty

        Dim strIdattivita As String
        Dim strIdArea As String
        strIdattivita = Request.QueryString("IdAttivita")
        If ddlArea.SelectedValue <> "0" Then
            strIdArea = Mid(ddlArea.SelectedValue, 1, InStr(ddlArea.SelectedValue, "|") - 1)
            If ClsUtility.CAUSALI_ACCOMPAGNA(Session("conn"), strIdattivita, strIdArea) = False Then
                lblerroreSettore.Text = "Impossibile cambiare causale: eliminare prima i/il sogetti/o ad esso legati/o."
                lblerrore.Text = String.Empty
                lblareaerrore.Text = String.Empty
                Exit Sub
            Else
                lblerroreSettore.Text = String.Empty
                lblerrore.Text = String.Empty
                lblareaerrore.Text = String.Empty
            End If

        Else
            lblerrore.Text = String.Empty
            lblerroreSettore.Text = String.Empty
            lblareaerrore.Text = "Sezione Progetto - Selezionare l'area di intervento. <br/>"
            'ValidaCampi()
        End If
    End Sub

    Private Sub dtgAmbiti_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtgAmbiti.SelectedIndexChanged
        dtgAmbitiSelezionati.DataSource = CreateDataSource()
        dtgAmbitiSelezionati.DataBind()
    End Sub

    Private Sub dtgAmbiti_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dtgAmbiti.PageIndexChanged
        dtgAmbiti.CurrentPageIndex = e.NewPageIndex
        dtgAmbiti.DataSource = Session("appDtsRisRicerca")
        dtgAmbiti.DataBind()
        dtgAmbiti.SelectedIndex = -1
    End Sub

    Private Sub dtgAmbitiSelezionati_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtgAmbitiSelezionati.SelectedIndexChanged
        If dtgAmbitiSelezionati.Items(0).Cells(2).Text <> "Nessun Ambito." And dtgAmbitiSelezionati.Items(0).Cells(2).Text <> "&nbsp;" Then
            dtgAmbitiSelezionati.DataSource = CancellaRecord()
            dtgAmbitiSelezionati.DataBind()
            dtgAmbitiSelezionati.SelectedIndex = -1
            If dtgAmbitiSelezionati.Items.Count = 0 Then
                PulisciDataGrid(dtgAmbitiSelezionati)
            End If
        End If
    End Sub


    Private Sub CaricaComboSettori()
        Dim dtrSettori As SqlClient.SqlDataReader
        'variabile stringa locale per costruire la query per i settori
        Dim strSql As String

        ChiudiDataReader(dtrSettori)
        strSql = "select distinct macroambitiattività.IdMacroAmbitoAttività,(macroambitiattività.Codifica + ' - ' + macroambitiattività.MacroAmbitoAttività) as Settori from macroambitiattività "
        strSql = strSql & "inner join ambitiattività as a on a.IdMacroAmbitoAttività=macroambitiattività.IdMacroAmbitoAttività "
        strSql = strSql & "inner join associaambititipiprogetto as b on b.idambitoattività=a.idambitoattività "
        strSql = strSql & "where b.idtipoprogetto=" & Request.QueryString("Nazionale") & " "
        strSql = strSql & "union "
        strSql = strSql & "select '0','Selezionare' from macroambitiattività "
        strSql = strSql & "order by macroambitiattività.IdMacroAmbitoAttività "
        dtrSettori = ClsServer.CreaDatareader(strSql, Session("conn"))
        If dtrSettori.HasRows = True Then
            ddlSettore.DataSource = dtrSettori
            ddlSettore.DataTextField = "Settori"
            ddlSettore.DataValueField = "IdMacroAmbitoAttività"
            ddlSettore.DataBind()
        End If
        ChiudiDataReader(dtrSettori)
    End Sub

    Private Sub CaricaComboDurata()
        Dim dtrSettori As SqlClient.SqlDataReader
        'variabile stringa locale per costruire la query per i settori
        Dim strSql As String

        ChiudiDataReader(dtrSettori)
        strSql = " SELECT 0 as NumMesi,'' as nmesi UNION "
        strSql &= " SELECT nmesi as NumMesi ,convert(varchar,nmesi) as nmesi FROM TipiProgettoDettaglio "

        dtrSettori = ClsServer.CreaDatareader(strSql, Session("conn"))
        If dtrSettori.HasRows = True Then
            ddlDurata.DataSource = dtrSettori
            ddlDurata.DataTextField = "nmesi"
            ddlDurata.DataValueField = "NumMesi"
            ddlDurata.DataBind()
        End If
        ChiudiDataReader(dtrSettori)
    End Sub

    Private Function MonteOreAnnuo(ByVal strNMensi As String) As Integer
        Dim dtrSettori As SqlClient.SqlDataReader
        'variabile stringa locale per costruire la query per i settori
        Dim strSql As String
        Dim MonteOre As Integer
        ChiudiDataReader(dtrSettori)
        strSql = " select monteoreannuo from TipiProgettoDettaglio where nmesi= " & CInt(strNMensi)

        dtrSettori = ClsServer.CreaDatareader(strSql, Session("conn"))
        If dtrSettori.HasRows = True Then
            dtrSettori.Read()
            MonteOre = dtrSettori("monteoreannuo")
        End If
        ChiudiDataReader(dtrSettori)
        Return MonteOre
    End Function

    Sub CaricaComboAreaPrimaVolta()
        'datareader locale per il caricamento della combo dei settori
        Dim dtrAree As SqlClient.SqlDataReader
        'variabile stringa locale per costruire la query per le aree
        Dim strSql As String
        'controllo e chiudo il datareader qualora fosse rimasta appesa la connessione
        If Not dtrAree Is Nothing Then
            dtrAree.Close()
            dtrAree = Nothing
        End If
        'preparo la query per i settori
        strSql = "select '0' as IDCod,'Selezionare' as Aree order by IDCod"
        'eseguo la query e passo il risultato al datareader
        dtrAree = ClsServer.CreaDatareader(strSql, Session("conn"))
        'controllo se ci sono dei record
        If dtrAree.HasRows = True Then
            'al datasource sella combo passo il datareader
            ddlArea.DataSource = dtrAree
            'Descrizione dei settori
            ddlArea.DataTextField = "Aree"
            'id del settore
            ddlArea.DataValueField = "IDCod"
            ddlArea.DataBind()
        End If

        If Not dtrAree Is Nothing Then
            dtrAree.Close()
            dtrAree = Nothing
        End If
    End Sub

    Sub CaricaComboArea(ByVal idSettoreSelezionato As Integer)
        Dim dtrAree As SqlClient.SqlDataReader
        'variabile stringa locale per costruire la query per le aree
        Dim strSql As String
        'controllo e chiudo il datareader qualora fosse rimasta appesa la connessione
        ChiudiDataReader(dtrAree)

        strSql = "select (convert(varchar(10),ambitiattività.IdAmbitoAttività) + '|' + ambitiattività.Codifica + ' - ' + ambitiattività.AmbitoAttività) as IDCod, (ambitiattività.Codifica + ' - ' + ambitiattività.AmbitoAttività) as Aree "
        strSql = strSql & "from associaambititipiprogetto "
        strSql = strSql & "inner join ambitiattività on associaambititipiprogetto.idambitoattività=ambitiattività.idambitoattività "
        strSql = strSql & "where ambitiattività.IdMacroAmbitoAttività=" & idSettoreSelezionato & " and associaambititipiprogetto.idtipoprogetto=" & Request.QueryString("Nazionale") & " and ambitiattività.attivo=1 "
        strSql = strSql & "union "
        strSql = strSql & "select '0','Selezionare' from ambitiattività order by Aree "
        dtrAree = ClsServer.CreaDatareader(strSql, Session("conn"))
        If dtrAree.HasRows = True Then
            ddlArea.DataSource = dtrAree
            'Descrizione dei settori
            ddlArea.DataTextField = "Aree"
            'id del settore
            ddlArea.DataValueField = "IDCod"
            ddlArea.DataBind()
        End If
        ChiudiDataReader(dtrAree)
    End Sub

    Sub CancellaAmbitiSecondari()
        Dim strSql As String
        Dim myCommand As New SqlClient.SqlCommand
        myCommand.Connection = Session("conn")
        strSql = "delete from AttivitàAmbiti where IdAttività='" & Request.QueryString("IdAttivita") & "'"
        myCommand.CommandText = strSql
        myCommand.ExecuteNonQuery()
        dtgAmbitiSelezionati.DataSource = CancellaRecord()
        dtgAmbitiSelezionati.DataBind()
    End Sub
    Sub CaricaAmbitiModifica(ByVal IdAttivita As Integer)
        'variabile stringa per caricare i partner precedentemente inseriti
        Dim strLocal As String
        Dim dtsLocal As DataSet
        strLocal = "select a.idambitoattività as idambitonascosto, "
        strLocal = strLocal & "(convert(varchar(10),a.IdAmbitoAttività) + '|' + b.Codifica + ' - ' + b.AmbitoAttività) as IdAmbito, "
        strLocal = strLocal & "(c.Codifica + ' - ' + c.MacroAmbitoAttività + '-' + b.Codifica + ' - ' + b.AmbitoAttività) as Denominazione "
        strLocal = strLocal & "from attivitàambiti as a "
        strLocal = strLocal & "inner join ambitiattività as b on a.idambitoattività=b.idambitoattività "
        strLocal = strLocal & "inner join macroambitiattività as c on b.idmacroambitoattività=c.idmacroambitoattività "
        strLocal = strLocal & "where a.idattività=" & IdAttivita

        dtsLocal = ClsServer.DataSetGenerico(strLocal, Session("conn"))
        If dtsLocal.Tables(0).Rows.Count > 0 Then
            dtgAmbitiSelezionati.DataSource = dtsLocal
            dtgAmbitiSelezionati.DataBind()
        Else
            PulisciDataGrid(dtgAmbitiSelezionati)
        End If
    End Sub
    Sub PulisciDataGrid(ByVal GridDaPulire As DataGrid)
        Dim dtRigheVuote As New DataTable
        Dim drRigheVuote As DataRow
        Dim i As Integer
        dtRigheVuote.Columns.Add()
        dtRigheVuote.Columns.Add(New DataColumn("IdAmbito", GetType(String)))
        dtRigheVuote.Columns.Add(New DataColumn("Denominazione", GetType(String)))
        dtRigheVuote.Columns.Add(New DataColumn("IdAmbitoNascosto", GetType(String)))
        drRigheVuote = dtRigheVuote.NewRow()
        drRigheVuote(1) = ""
        drRigheVuote(2) = "Nessun Ambito."
        drRigheVuote(3) = ""
        dtRigheVuote.Rows.Add(drRigheVuote)
        GridDaPulire.DataSource = dtRigheVuote
        GridDaPulire.DataBind()
        GridDaPulire.SelectedIndex = -1

    End Sub
    Function CancellaRecord() As ICollection
        Dim dt As New DataTable
        Dim dr As DataRow

        dt.Columns.Add()
        dt.Columns.Add(New DataColumn("IdAmbito", GetType(String)))
        dt.Columns.Add(New DataColumn("Denominazione", GetType(String)))
        dt.Columns.Add(New DataColumn("IdAmbitoNascosto", GetType(String)))

        Dim x As Integer

        If dtgAmbitiSelezionati.Items.Count = 0 Or dtgAmbitiSelezionati.Items.Count = 1 Then
            If Not Session("VOperazioniAmbiti") Is Nothing Then


                If Session("VOperazioniAmbiti")(0) <> "" Then
                    'delete
                    ReDim Preserve Session("VOperazioniAmbiti")(UBound(Session("VOperazioniAmbiti")) + 1)
                    If Not dtgAmbitiSelezionati.SelectedItem Is Nothing Then
                        Session("VOperazioniAmbiti")(UBound(Session("VOperazioniAmbiti"))) = "delete from AttivitàAmbiti where IdAttività=" & CInt(Request.QueryString("IdAttivita")) & " and IdAmbitoAttività=" & CInt(dtgAmbitiSelezionati.SelectedItem.Cells(3).Text)
                    End If
                Else
                    'delete
                    If Not dtgAmbitiSelezionati.SelectedItem Is Nothing Then
                        Session("VOperazioniAmbiti")(0) = "delete from AttivitàAmbiti where IdAttività=" & CInt(Request.QueryString("IdAttivita")) & " and IdAmbitoAttività=" & CInt(dtgAmbitiSelezionati.SelectedItem.Cells(3).Text)

                    End If
                End If
            End If
            dt = Nothing
        Else
            For x = 1 To dtgAmbitiSelezionati.Items.Count
                If dtgAmbitiSelezionati.SelectedIndex <> x - 1 Then
                    dr = dt.NewRow()
                    dr(1) = dtgAmbitiSelezionati.Items(x - 1).Cells(1).Text
                    dr(2) = dtgAmbitiSelezionati.Items(x - 1).Cells(2).Text
                    dr(3) = dtgAmbitiSelezionati.Items(x - 1).Cells(3).Text
                    dt.Rows.Add(dr)
                Else
                    If Session("VOperazioniAmbiti")(0) <> "" Then
                        ReDim Preserve Session("VOperazioniAmbiti")(UBound(Session("VOperazioniAmbiti")) + 1)
                        Session("VOperazioniAmbiti")(UBound(Session("VOperazioniAmbiti"))) = "delete from AttivitàAmbiti where IdAmbitoAttività=" & CInt(dtgAmbitiSelezionati.Items(x - 1).Cells(3).Text) & " and IdAttività=" & CInt(Request.QueryString("IdAttivita"))
                    Else
                        Session("VOperazioniAmbiti")(0) = "delete from AttivitàAmbiti where IdAmbitoAttività=" & CInt(dtgAmbitiSelezionati.Items(x - 1).Cells(3).Text) & " and IdAttività=" & CInt(Request.QueryString("IdAttivita"))
                    End If
                End If
            Next
        End If

        Dim dv As New DataView(dt)
        Return dv

    End Function
    Function CreateDataSource() As ICollection
        Dim dt As New DataTable
        Dim dr As DataRow
        Dim blnCheck As Boolean = False

        dt.Columns.Add()
        dt.Columns.Add(New DataColumn("IdAmbito", GetType(String)))
        dt.Columns.Add(New DataColumn("Denominazione", GetType(String)))
        dt.Columns.Add(New DataColumn("IdAmbitoNascosto", GetType(String)))

        Dim i As Integer
        Dim x As Integer

        If Not dtgAmbiti.SelectedItem Is Nothing Then
            If dtgAmbitiSelezionati.Items.Count > 0 Then
                If dtgAmbitiSelezionati.Items(0).Cells(2).Text <> "Nessun Ambito." And dtgAmbitiSelezionati.Items(0).Cells(2).Text <> "&nbsp;" Then
                    For x = 1 To dtgAmbitiSelezionati.Items.Count
                        If (dtgAmbitiSelezionati.Items(x - 1).Cells(2).Text = dtgAmbiti.SelectedItem.Cells(2).Text) Then
                            blnCheck = True
                            Exit For
                        End If
                    Next
                    If blnCheck = False Then
                        For i = 1 To dtgAmbitiSelezionati.Items.Count
                            dr = dt.NewRow()
                            dr(1) = dtgAmbitiSelezionati.Items(i - 1).Cells(1).Text
                            dr(2) = dtgAmbitiSelezionati.Items(i - 1).Cells(2).Text
                            dr(3) = dtgAmbitiSelezionati.Items(i - 1).Cells(3).Text
                            dt.Rows.Add(dr)
                        Next

                        dr = dt.NewRow()
                        dr(1) = dtgAmbiti.SelectedItem.Cells(1).Text
                        dr(2) = dtgAmbiti.SelectedItem.Cells(2).Text
                        dr(3) = dtgAmbiti.SelectedItem.Cells(3).Text
                        dt.Rows.Add(dr)

                        If Session("VOperazioniAmbiti")(0) <> "" Then
                            ReDim Preserve Session("VOperazioniAmbiti")(UBound(Session("VOperazioniAmbiti")) + 1)
                            Session("VOperazioniAmbiti")(UBound(Session("VOperazioniAmbiti"))) = "insert into AttivitàAmbiti "
                            Session("VOperazioniAmbiti")(UBound(Session("VOperazioniAmbiti"))) = Session("VOperazioniAmbiti")(UBound(Session("VOperazioniAmbiti"))) & "(IdAttività, IdAmbitoAttività) "
                            Session("VOperazioniAmbiti")(UBound(Session("VOperazioniAmbiti"))) = Session("VOperazioniAmbiti")(UBound(Session("VOperazioniAmbiti"))) & "values "
                            Session("VOperazioniAmbiti")(UBound(Session("VOperazioniAmbiti"))) = Session("VOperazioniAmbiti")(UBound(Session("VOperazioniAmbiti"))) & "(" & CInt(Request.QueryString("IdAttivita")) & ", "
                            Session("VOperazioniAmbiti")(UBound(Session("VOperazioniAmbiti"))) = Session("VOperazioniAmbiti")(UBound(Session("VOperazioniAmbiti"))) & CInt(dtgAmbiti.SelectedItem.Cells(3).Text) & ")"
                        Else
                            Session("VOperazioniAmbiti")(0) = "insert into AttivitàAmbiti "
                            Session("VOperazioniAmbiti")(0) = Session("VOperazioniAmbiti")(0) & "(IdAttività, IdAmbitoAttività) "
                            Session("VOperazioniAmbiti")(0) = Session("VOperazioniAmbiti")(0) & "values "
                            Session("VOperazioniAmbiti")(0) = Session("VOperazioniAmbiti")(0) & "(" & CInt(Request.QueryString("Idattivita")) & ", "
                            Session("VOperazioniAmbiti")(0) = Session("VOperazioniAmbiti")(0) & CInt(dtgAmbiti.SelectedItem.Cells(3).Text) & ")"
                        End If

                        Dim dvII As New DataView(dt)
                        'Session("dvTipiPartner") = dt
                        Return dvII
                    Else
                        For i = 1 To dtgAmbitiSelezionati.Items.Count
                            dr = dt.NewRow()
                            dr(1) = dtgAmbitiSelezionati.Items(i - 1).Cells(1).Text
                            dr(2) = dtgAmbitiSelezionati.Items(i - 1).Cells(2).Text
                            dr(3) = dtgAmbitiSelezionati.Items(i - 1).Cells(3).Text
                            dt.Rows.Add(dr)
                        Next
                        Dim dvII As New DataView(dt)
                        'Session("dvTipiPartner") = dt
                        Return dvII
                    End If
                Else
                    dr = dt.NewRow()
                    dr(1) = dtgAmbiti.SelectedItem.Cells(1).Text
                    dr(2) = dtgAmbiti.SelectedItem.Cells(2).Text
                    dr(3) = dtgAmbiti.SelectedItem.Cells(3).Text
                    dt.Rows.Add(dr)
                    If Session("VOperazioniAmbiti") Is Nothing Then
                        ReDim Session("VOperazioniAmbiti")(0)
                        Session("VOperazioniAmbiti")(0) = ""
                    End If

                    If Session("VOperazioniAmbiti")(0) <> "" Then
                        ReDim Preserve Session("VOperazioniAmbiti")(UBound(Session("VOperazioniAmbiti")) + 1)
                        Session("VOperazioniAmbiti")(UBound(Session("VOperazioniAmbiti"))) = "insert into AttivitàAmbiti "
                        Session("VOperazioniAmbiti")(UBound(Session("VOperazioniAmbiti"))) = Session("VOperazioniAmbiti")(UBound(Session("VOperazioniAmbiti"))) & "(IdAttività, IdAmbitoAttività) "
                        Session("VOperazioniAmbiti")(UBound(Session("VOperazioniAmbiti"))) = Session("VOperazioniAmbiti")(UBound(Session("VOperazioniAmbiti"))) & "values "
                        Session("VOperazioniAmbiti")(UBound(Session("VOperazioniAmbiti"))) = Session("VOperazioniAmbiti")(UBound(Session("VOperazioniAmbiti"))) & "(" & CInt(Request.QueryString("IdAttivita")) & ", "
                        Session("VOperazioniAmbiti")(UBound(Session("VOperazioniAmbiti"))) = Session("VOperazioniAmbiti")(UBound(Session("VOperazioniAmbiti"))) & CInt(dtgAmbiti.SelectedItem.Cells(3).Text) & ")"
                    Else
                        Session("VOperazioniAmbiti")(0) = "insert into AttivitàAmbiti "
                        Session("VOperazioniAmbiti")(0) = Session("VOperazioniAmbiti")(0) & "(IdAttività, IdAmbitoAttività) "
                        Session("VOperazioniAmbiti")(0) = Session("VOperazioniAmbiti")(0) & "values "
                        Session("VOperazioniAmbiti")(0) = Session("VOperazioniAmbiti")(0) & "(" & CInt(Request.QueryString("IdAttivita")) & ", "
                        Session("VOperazioniAmbiti")(0) = Session("VOperazioniAmbiti")(0) & CInt(dtgAmbiti.SelectedItem.Cells(3).Text) & ")"
                    End If
                    Dim dvII As New DataView(dt)
                    'Session("dvTipiPartner") = dt
                    Return dvII
                End If

                For i = 1 To dtgAmbitiSelezionati.Items.Count
                    dr = dt.NewRow()
                    dr(1) = dtgAmbitiSelezionati.Items(i - 1).Cells(1).Text
                    dr(2) = dtgAmbitiSelezionati.Items(i - 1).Cells(2).Text
                    dr(3) = dtgAmbitiSelezionati.Items(i - 1).Cells(3).Text
                    dt.Rows.Add(dr)
                Next

                dr = dt.NewRow()
                dr(1) = dtgAmbiti.SelectedItem.Cells(1).Text
                dr(2) = dtgAmbiti.SelectedItem.Cells(2).Text
                dr(3) = dtgAmbiti.SelectedItem.Cells(3).Text
                dt.Rows.Add(dr)

                If Session("VOperazioniAmbiti")(0) <> "" Then
                    ReDim Preserve Session("VOperazioniAmbiti")(UBound(Session("VOperazioniAmbiti")) + 1)
                    Session("VOperazioniAmbiti")(UBound(Session("VOperazioniAmbiti"))) = "insert into AttivitàAmbiti "
                    Session("VOperazioniAmbiti")(UBound(Session("VOperazioniAmbiti"))) = Session("VOperazioniAmbiti")(UBound(Session("VOperazioniAmbiti"))) & "(IdAttività, IdAmbitoAttività) "
                    Session("VOperazioniAmbiti")(UBound(Session("VOperazioniAmbiti"))) = Session("VOperazioniAmbiti")(UBound(Session("VOperazioniAmbiti"))) & "values "
                    Session("VOperazioniAmbiti")(UBound(Session("VOperazioniAmbiti"))) = Session("VOperazioniAmbiti")(UBound(Session("VOperazioniAmbiti"))) & "(" & CInt(Request.QueryString("IdAttivita")) & ", "
                    Session("VOperazioniAmbiti")(UBound(Session("VOperazioniAmbiti"))) = Session("VOperazioniAmbiti")(UBound(Session("VOperazioniAmbiti"))) & CInt(dtgAmbiti.SelectedItem.Cells(3).Text) & ")"
                Else
                    Session("VOperazioniAmbiti")(0) = "insert into AttivitàAmbiti "
                    Session("VOperazioniAmbiti")(0) = Session("VOperazioniAmbiti")(0) & "(IdAttività, IdAmbitoAttività) "
                    Session("VOperazioniAmbiti")(0) = Session("VOperazioniAmbiti")(0) & "values "
                    Session("VOperazioniAmbiti")(0) = Session("VOperazioniAmbiti")(0) & "(" & CInt(Request.QueryString("IdAttivita")) & ", "
                    Session("VOperazioniAmbiti")(0) = Session("VOperazioniAmbiti")(0) & CInt(dtgAmbiti.SelectedItem.Cells(3).Text) & ")"
                End If

            Else
                dr = dt.NewRow()
                dr(1) = dtgAmbiti.SelectedItem.Cells(1).Text
                dr(2) = dtgAmbiti.SelectedItem.Cells(2).Text
                dr(3) = dtgAmbiti.SelectedItem.Cells(3).Text
                dt.Rows.Add(dr)
                If Session("VOperazioniAmbiti")(0) <> "" Then
                    ReDim Preserve Session("VOperazioniAmbiti")(UBound(Session("VOperazioniAmbiti")) + 1)
                    Session("VOperazioniAmbiti")(UBound(Session("VOperazioniAmbiti"))) = "insert into AttivitàAmbiti "
                    Session("VOperazioniAmbiti")(UBound(Session("VOperazioniAmbiti"))) = Session("VOperazioniAmbiti")(UBound(Session("VOperazioniAmbiti"))) & "(IdAttività, IdAmbitoAttività) "
                    Session("VOperazioniAmbiti")(UBound(Session("VOperazioniAmbiti"))) = Session("VOperazioniAmbiti")(UBound(Session("VOperazioniAmbiti"))) & "values "
                    Session("VOperazioniAmbiti")(UBound(Session("VOperazioniAmbiti"))) = Session("VOperazioniAmbiti")(UBound(Session("VOperazioniAmbiti"))) & "(" & CInt(Request.QueryString("IdAttivita")) & ", "
                    Session("VOperazioniAmbiti")(UBound(Session("VOperazioniAmbiti"))) = Session("VOperazioniAmbiti")(UBound(Session("VOperazioniAmbiti"))) & CInt(dtgAmbiti.SelectedItem.Cells(3).Text) & ")"
                Else
                    Session("VOperazioniAmbiti")(0) = "insert into AttivitàAmbiti "
                    Session("VOperazioniAmbiti")(0) = Session("VOperazioniAmbiti")(0) & "(IdAttività, IdAmbitoAttività) "
                    Session("VOperazioniAmbiti")(0) = Session("VOperazioniAmbiti")(0) & "values "
                    Session("VOperazioniAmbiti")(0) = Session("VOperazioniAmbiti")(0) & "(" & CInt(Request.QueryString("IdAttivita")) & ", "
                    Session("VOperazioniAmbiti")(0) = Session("VOperazioniAmbiti")(0) & CInt(dtgAmbiti.SelectedItem.Cells(3).Text) & ")"
                End If
            End If
        Else
            PulisciDataGrid(dtgAmbitiSelezionati)
        End If

        Dim dv As New DataView(dt)
        'Session("dvTipiPartner") = dt
        Return dv

    End Function

    Private Sub imgConferma_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles imgConferma.Click
        Dim x As Integer
        If dtgAmbitiSelezionati.Items.Count > 0 Then
            If dtgAmbitiSelezionati.Items(0).Cells(2).Text <> "Nessun Ambito." And dtgAmbitiSelezionati.Items(0).Cells(2).Text <> "&nbsp;" Then
                txtArea.Text = String.Empty
                txtIdAree.Value = String.Empty
                Dim areaBuilder As StringBuilder = New StringBuilder()
                Dim idAreeBuilder As StringBuilder = New StringBuilder()
                For x = 1 To dtgAmbitiSelezionati.Items.Count
                    'porto dietro il primo idambitoarea per continuare ad inserirlo in attività
                    If x = 1 Then
                        txtArea.Text = dtgAmbitiSelezionati.Items(x - 1).Cells(3).Text
                    End If
                    areaBuilder.AppendLine(dtgAmbitiSelezionati.Items(x - 1).Cells(2).Text)
                    idAreeBuilder.Append(dtgAmbitiSelezionati.Items(x - 1).Cells(3).Text)
                    idAreeBuilder.Append("|")
                Next
                txtArea.Text = areaBuilder.ToString()
                txtIdAree.Value = idAreeBuilder.ToString()
            Else
                txtArea.Text = String.Empty
                txtIdAree.Value = String.Empty
            End If
        Else
            txtArea.Text = String.Empty
            txtIdAree.Value = String.Empty
        End If
        DivSelezionaAltriAmbiti.Visible = False
    End Sub

    Sub CaricaArea(ByVal idSettoreSelezionato As Integer)
        'dataset locale per il caricamento dei settori
        Dim dtsAree As DataSet
        'variabile stringa locale per costruire la query per le aree
        Dim strSql As String

        'preparo la query per i settori
        strSql = "select (IdAmbitoAttività) as IdAmbitoNascosto, (convert(varchar(10),IdAmbitoAttività) + '|' + Codifica + ' - ' + AmbitoAttività) as IdAmbito,('" & Replace(ddlSettore.SelectedItem.Text, "'", "''") & " - " & "' + Codifica + ' - ' + AmbitoAttività) as Denominazione"
        strSql = strSql & " from ambitiattività where IdMacroAmbitoAttività=" & idSettoreSelezionato & " and ambitiattività.attivo=1"
        strSql = strSql & " order by Denominazione"
        'eseguo la query e passo il risultato al datareader
        dtsAree = ClsServer.DataSetGenerico(strSql, Session("conn"))
        'controllo se ci sono dei record
        If dtsAree.Tables(0).Rows.Count > 0 Then
            'al datasource sella combo passo il datareader
            dtgAmbiti.DataSource = dtsAree
            dtgAmbiti.DataBind()
            Session("appDtsRisRicerca") = dtsAree
        Else
            PulisciDataGrid(dtgAmbiti)
        End If

    End Sub
    Protected Sub imgArea_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgArea.Click
        lblMessaggioConferma.Text = String.Empty
        If (ddlSettore.SelectedIndex <> 0) Then
            DivSelezionaAltriAmbiti.Visible = True
            CaricaArea(ddlSettore.SelectedValue)
            CaricaAmbitiModifica(CInt(Request.QueryString("IdAttivita")))
        Else
            lblerrore.Text = "Selezionare il Settore principale ."
        End If

    End Sub

    Protected Sub btnNascondiElencoAmbiti_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnNascondiElencoAmbiti.Click
        DivSelezionaAltriAmbiti.Visible = False
    End Sub

    Protected Sub imgDisabili_Click(ByVal sender As Object, ByVal e As EventArgs) Handles imgDisabili.Click
        Response.Redirect("WfrmDisabili.aspx?IdAmbitoAttività=" & txtIdAmbitoAttivita.Value & "&Modifica=" & Request.QueryString("Modifica") & "&Nazionale=" & Request.QueryString("Nazionale") & "&IdAttivita=" & CInt(Request.QueryString("IdAttivita")) & "&VengoDa=" & Request.QueryString("VengoDa"))
    End Sub

    Protected Sub imgDett_Click(ByVal sender As Object, ByVal e As EventArgs) Handles imgDett.Click
        Response.Redirect("riepilogoassenzeprogetto.aspx?Modifica=" & Request.QueryString("Modifica") & "&Nazionale=" & Request.QueryString("Nazionale") & "&IdAttivita=" & CInt(Request.QueryString("IdAttivita")) & "&VengoDa=" & Request.QueryString("VengoDa"))
    End Sub

    Protected Sub ddlEventualiCriteri_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlEventualiCriteri.SelectedIndexChanged
        lblMessaggioConferma.Text = String.Empty
        Me.hdnIsPostbackCO.Value = Me.IsPostBack
        If (ddlEventualiCriteri.SelectedValue <> "3") Then
            txtCodEnteEventuali.Text = String.Empty
            txtCodEnteEventuali.Enabled = False
        Else
            txtCodEnteEventuali.Enabled = True
        End If

    End Sub



    Protected Sub ddlModalitaAttuazione_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlModalitaAttuazione.SelectedIndexChanged
        lblMessaggioConferma.Text = String.Empty
        Me.hdnIsPostbackFG.Value = Me.IsPostBack
        If (ddlModalitaAttuazione.SelectedValue = "2") Then
            txtModalitaAttuazione.Enabled = True
        Else
            txtModalitaAttuazione.Enabled = False
            txtModalitaAttuazione.Text = String.Empty
        End If
    End Sub

    Protected Sub ddlmodattuazione_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlmodattuazione.SelectedIndexChanged
        lblMessaggioConferma.Text = String.Empty
        Me.hdnIsPostbackFS.Value = Me.IsPostBack
        If (ddlmodattuazione.SelectedValue = "2") Then
            txtmodattuazione.Enabled = True
        Else
            txtmodattuazione.Enabled = False
            txtmodattuazione.Text = String.Empty
        End If
    End Sub

    Private Function ValidaCampi() As Boolean
        Dim errore As StringBuilder = New StringBuilder()
        Dim campiValidi As Boolean = True
        Dim numeroIntero As Integer
        If (txtTitolo.Text = String.Empty) Then
            errore.AppendLine("Sezione Progetto - Inserire il Titolo. <br/>")
        End If
        If (ddlSettore.SelectedValue = 0) Then
            errore.AppendLine("Sezione Progetto - Selezionare il settore Principale. <br/>")
        End If
        If (ddlArea.SelectedValue = "0") Then
            lblareaerrore.Text = String.Empty
            errore.AppendLine("Sezione Progetto - Selezionare l'area di intervento. <br/>")
        End If
        'aggiunto da simona cordella il 02/08/2017
        If ddlDurata.SelectedValue = 0 Then
            lblareaerrore.Text = String.Empty
            errore.AppendLine("Sezione Progetto - Selezionare la durata del progetto. <br/>")
        End If
        If (Session("TipoUtente") = "U" Or Session("TipoUtente") = "R") And Not Request.QueryString("IdAttivita") Is Nothing Then


            If (Integer.TryParse(txtNumeroPostiVittoAlloggio.Text, numeroIntero) = False) Then
                errore.AppendLine("Sezione Progetto - Il valore inserito dei posti con vitto e alloggio non è valido. <br/>")
            ElseIf (numeroIntero > 32767) Then
                errore.AppendLine("Sezione Progetto - Il valore inserito dei posti con vitto e alloggio  %E8 superiore al numero massimo consentito. <br/>")
                numeroIntero = 0
            End If
            If (Integer.TryParse(txtNumeroPostiSenzaVittoAlloggio.Text, numeroIntero) = False) Then
                errore.AppendLine("Sezione Progetto - Il valore inserito dei posti senza vitto e alloggio non è valido. <br/>")
            ElseIf (numeroIntero > 32767) Then
                errore.AppendLine("Sezione Progetto - Il valore inserito dei posti senza vitto e alloggio  %E8 superiore al numero massimo consentito. <br/>")
                numeroIntero = 0
            End If
            If (Integer.TryParse(txtNumeroPostiSoloVitto.Text, numeroIntero) = False) Then
                errore.AppendLine("Sezione Progetto - Il valore inserito dei posti con vitto  non è valido. <br/>")
            ElseIf (numeroIntero > 32767) Then
                errore.AppendLine("Sezione Progetto - Il valore inserito dei posti con vitto  %E8 superiore al numero massimo consentito. <br/>")
                numeroIntero = 0
            End If
        End If
        If (txtOreServizioSettimanali.Text <> String.Empty) Then
            Dim ore As Double
            If (Double.TryParse(txtOreServizioSettimanali.Text, ore) = False) Then
                errore.AppendLine("Sezione Progetto - Il valore delle ore di Servizio Settimanale non è valido. <br/>")
            Else
                If (chkOreSett.Selected = True) Then
                    If (ore <> 25) Then
                        errore.AppendLine("Sezione Progetto - Inserire 25 ore di Servizio Settimanale. <br/>")
                    ElseIf (ore > 255) Then
                        errore.AppendLine("Sezione Progetto - Il valore inserito e' superiore al numero massimo consentito. <br/>")
                    End If
                ElseIf (chkOreSett.Selected = False) Then
                    If (ore < 12) Then
                        errore.AppendLine("Sezione Progetto - Inserire almeno 12 ore di Servizio Settimanale. <br/>")
                    ElseIf (ore > 25) Then
                        errore.AppendLine("Sezione Progetto - Il valore inserito e' superiore al numero massimo consentito. <br/>")
                    End If
                End If
            End If
        Else
            If (chkOreSett.Selected = True And txtOreServizioSettimanali.Text = String.Empty) Then
                errore.AppendLine("Sezione Progetto - Inserire almeno 12 ore di Servizio Settimanale. <br/>")
            End If
            If (chkOreSett.Selected = False And txtOreServizioSettimanali.Text = String.Empty) Then
                errore.AppendLine("Sezione Progetto - Inserire 25 ore di Servizio Settimanale. <br/>")
            End If
        End If
        If (chkOreSett.Selected = False) Then
            If (txtMonteOreAnnuo.Text = String.Empty) Then
                errore.AppendLine("Sezione Progetto - Inserire un Monte ore Annuo minimo di 1400 ore. <br/>")
            Else
                Dim oreAnnue As Double
                If (Double.TryParse(txtMonteOreAnnuo.Text, oreAnnue) = False) Then
                    errore.AppendLine("Sezione Progetto - Il valore delle ore di Monte ore Annuo non è valido. <br/>")
                Else
                    If (oreAnnue > 32767) Then
                        errore.AppendLine("Sezione Progetto - Il valore inserito del monte ore annuo e' superiore al numero massimo consentito. <br/>")
                    ElseIf (oreAnnue < 1400) Then
                        '    errore.AppendLine("Sezione Progetto - Inserire un Monte ore Annuo minimo di 1400 ore. <br/>")

                    End If
                End If
            End If
        End If
        If (txtNumGiorniServizio.Text = String.Empty) Then
            errore.AppendLine("Sezione Progetto - I giorni di Servizio Settimanali devono essere 5 oppure 6. <br/>")
        Else
            Dim giorniServizioSettimanali As Int16
            If (Int16.TryParse(txtNumGiorniServizio.Text, giorniServizioSettimanali) = False) Then
                errore.AppendLine("Sezione Progetto - Il valore inserito per i giorni di Servizio Settimanali non è valido. <br/>")
            Else
                If (giorniServizioSettimanali <> 6 And giorniServizioSettimanali <> 5) Then
                    errore.AppendLine("Sezione Progetto - I giorni di Servizio Settimanali devono essere 5 oppure 6. <br/>")
                End If
            End If
        End If
        'aggiunto da simona cordella il 02/08/2017
        If chkGiovaniMinoriOp.Checked = True Then
            If (Integer.TryParse(txtNumeroGiovaniMinoriOpportunita.Text, numeroIntero) = False) Then
                errore.AppendLine("Sezione Progetto - Inserire il numero di giovani con minori opportunità. <br/>")
            ElseIf (numeroIntero = 0) Then
                errore.AppendLine("Sezione Progetto - Il numero di giovani con minori opportunità deve essere maggiore di 0. <br/>")
            End If
        End If
        If Request.QueryString("Nazionale") <> "6" Then
            'solo per i progetti in italia
            'If optEstero.Checked = False And optTutoraggio.Checked = False Then
            '    errore.AppendLine("Sezione Progetto - Selezionare Estero(UE) o Tutoraggio. <br/>")
            'End If
            If optEstero.Checked = True Then
                If (Integer.TryParse(txtMesiPrevistiUE.Text, numeroIntero) = False) Then
                    errore.AppendLine("Sezione Progetto - Il valore inserito dei mesi previsti per Estero (UE) non è valido. <br/>")
                ElseIf (numeroIntero > 3) Then
                    errore.AppendLine("Sezione Progetto - Il valore inserito dei mesi previsti per Estero (UE) è superiore al numero massimo consentito. <br/>")
                    numeroIntero = 0
                ElseIf (numeroIntero = 0) Then
                    errore.AppendLine("Sezione Progetto - Il valore inserito dei mesi previsti per Estero (UE) è inferiore al numero massimo consentito. <br/>")
                    numeroIntero = 0
                End If
            End If
            If optTutoraggio.Checked = True Then
                If (Integer.TryParse(txtMesiPrevistiTutoraggio.Text, numeroIntero) = False) Then
                    errore.AppendLine("Sezione Progetto - Il valore inserito dei mesi previsti per il Tutoraggio non è valido. <br/>")
                ElseIf (numeroIntero > 3) Then
                    errore.AppendLine("Sezione Progetto - Il valore inserito dei mesi previsti per il Tutoraggio è superiore al numero massimo consentito. <br/>")
                    numeroIntero = 0
                ElseIf (numeroIntero = 0) Then
                    errore.AppendLine("Sezione Progetto - Il valore inserito dei mesi previsti per il Tutoraggio è inferiore al numero massimo consentito. <br/>")
                    numeroIntero = 0
                End If
            End If
        End If
        If (ddlEventualiCriteri.SelectedValue = 0) Then
            errore.AppendLine("Sezione Caratteristiche Organizzative - Selezionare i Criteri di Selezione. <br/>")
        ElseIf (ddlEventualiCriteri.SelectedValue = 3 And txtCodEnteEventuali.Text = String.Empty) Then
            errore.AppendLine("Sezione Caratteristiche Organizzative - Inserire il codice ente per i Criteri di Selezione. <br/>")
        End If
        If (ddlPianoMonitoraggio.SelectedValue = 0) Then
            errore.AppendLine("Sezione Caratteristiche Organizzative - Selezionare il Piano di Monitoriaggio. <br/>")
        ElseIf (ddlPianoMonitoraggio.SelectedValue = 3 And txtPianoMonitoraggio.Text = String.Empty) Then
            errore.AppendLine("Sezione Caratteristiche Organizzative - Inserire il codice ente per il Piano di Monitoraggio. <br/>")
        End If
        If (txtOrePromozioneSensibilizzazione.Text = String.Empty) Then
            errore.AppendLine("Sezione Caratteristiche Organizzative - Le Ore di Promozione e Sensibilizzazione devono essere specificate. <br/>")
        Else
            Dim orePromozione As Double
            If (Double.TryParse(txtOrePromozioneSensibilizzazione.Text, orePromozione) = False) Then
                errore.AppendLine("Sezione Caratteristiche Organizzative - Il valore inserito per le Ore di Promozione e Sensibilizzazione non è valido. <br/>")
            End If

        End If
        If (ddlCreditiFormativiRiconosciuti.SelectedValue = 10) Then
            errore.AppendLine("Sezione Caratteristiche Conoscenze Acquisite - Selezionare i Crediti Formativi Riconosciuti. <br/>")
        End If
        If (ddlEventualiTirociniRiconosciuti.SelectedValue = 10) Then
            errore.AppendLine("Sezione Caratteristiche Conoscenze Acquisite - Selezionare Gli Eventuali Tirocini Riconosciuti. <br/>")
        End If
        If (ddlCompetenzeAcquisibili.SelectedValue = 10) Then
            errore.AppendLine("Sezione Caratteristiche Conoscenze Acquisite - Selezionare le Competenze Acquisibili. <br/>")
        End If
        If (txtDurata.Text = String.Empty) Then
            errore.AppendLine("Sezione Formazione Generale - Inserire la durata della formazione(Il valore minimo &#232; di 30 ore). <br/>")
        Else
            Dim durata As Double
            If (Double.TryParse(txtDurata.Text, durata) = False) Then
                errore.AppendLine("Sezione Formazione Generale - Il valore inserito per la durata della formazione non è valido. <br/>")
            Else
                If (durata > 32767) Then
                    errore.AppendLine("Sezione Formazione Generale - Il valore inserito per la durata della formazione %E8 superiore al numero massimo consentito. <br/>")
                ElseIf (durata < 30) Then
                    errore.AppendLine("Sezione Formazione Generale - La durata della formazione non puograve; essere inferiore alle 30 ore. <br/>")
                End If
            End If
        End If
        If (ddlModalitaAttuazione.SelectedValue = 0) Then
            errore.AppendLine("Sezione Formazione Generale - Selezionare la Modalit&#224; di Attuazione della Formazione Generale. <br/>")
        End If
        If (ddlModalitaAttuazione.SelectedValue = 2 And txtModalitaAttuazione.Text = String.Empty) Then
            errore.AppendLine("Sezione Formazione Generale - Inserire il codice ente per la Modalit&#224; di Attuazione. <br/>")
        End If
        If (ddlFormazioneGeneraleErogazione.SelectedValue = "selezionare") Then
            errore.AppendLine("Sezione Formazione Generale - Selezionare una Modalit&#224; di Erogazione della Formazione Generale. <br/>")
        ElseIf (ddlFormazioneGeneraleErogazione.SelectedValue = "percentuale" And ddlDurata.SelectedValue <> 12) Then
            errore.AppendLine("Sezione Formazione Generale - Per i progetti minori di 12 mesi, l'erogazione della formazione generale deve essere ad Unica Tranche. <br/>")
        End If


        If (txtDurataSpec.Text = String.Empty) Then
            errore.AppendLine("Sezione Formazione Specifica - Inserire la durata della Formazione Specifica. <br/>")
        Else
            Dim durataFormazioneSpecifica As Double
            Dim durataFormazioneGenerale As Double
            If (Double.TryParse(txtDurataSpec.Text, durataFormazioneSpecifica) = False) Then
                errore.AppendLine("Sezione Formazione Specifica - Il valore inserito per la durata della formazione non è valido. <br/>")
            Else
                If (durataFormazioneSpecifica > 32767) Then
                    errore.AppendLine("Sezione Formazione Specifica - Il valore inserito per la durata della formazione %E8 superiore al numero massimo consentito. <br/>")
                ElseIf (durataFormazioneSpecifica < 50) Then
                    errore.AppendLine("Sezione Formazione Specifica - La durata della formazione non puograve; essere inferiore alle 50 ore. <br/>")
                End If
                If (Double.TryParse(txtDurata.Text, durataFormazioneGenerale)) Then
                    If (durataFormazioneSpecifica + durataFormazioneGenerale < 80) Then
                        errore.AppendLine("Sezione Formazione Specifica - Il valore della durata della Formazione Generale sommato a quello della Formazione Specifica deve essere almeno di 80 ore. <br/>")
                    ElseIf (durataFormazioneSpecifica + durataFormazioneGenerale > 150) Then
                        errore.AppendLine("Sezione Formazione Specifica - Il valore della durata della Formazione Generale sommato a quello della Formazione Specifica non deve superare le 150 ore. <br/>")

                    End If
                End If
            End If
        End If
        If (ddlmodattuazione.SelectedValue = 0) Then
            errore.AppendLine("Sezione Formazione Specifica - Selezionare la Modalit&#224; di Attuazione della Formazione Specifica. <br/>")
        End If
        If (ddlmodattuazione.SelectedValue = 2 And txtModalitaAttuazione.Text = String.Empty) Then
            errore.AppendLine("Sezione Formazione Specifica - Inserire il codice ente per la Modalit&#224; di Attuazione. <br/>")
        End If
        If (ddlFormazioneSpecificaErogazione.SelectedValue = "selezionare") Then
            errore.AppendLine("Sezione Formazione Specifica - Selezionare una Modalit&#224; di Erogazione della Formazione Specifica. <br/>")
        ElseIf (ddlFormazioneSpecificaErogazione.SelectedValue = "percentuale" And ddlDurata.SelectedValue <> 12) Then
            errore.AppendLine("Sezione Formazione Specifica - Per i progetti minori di 12 mesi, l'erogazione della formazione specifica deve essere ad Unica Tranche. <br/>")
        End If
        If (Request.QueryString("Nazionale") = "2") Then
            If (ddlCondizioniRischio.SelectedValue = 10) Then
                errore.AppendLine("Sezione Estero - Selezionare le Particolari Condizioni di Rischio. <br/>")
            End If
            If (ddlLivelliSicurezza.SelectedValue = 10) Then
                errore.AppendLine("Sezione Estero - Selezionare i Livelli Minimi di Sicurezza. <br/>")
            End If
            If (ddlCondizioniDisagio.SelectedValue = 10) Then
                errore.AppendLine("Sezione Estero - Selezionare le Particolari Condizioni di Disagio. <br/>")
            End If
            If (ddlEventualeAssicurazione.SelectedValue = 10) Then
                errore.AppendLine("Sezione Estero - Selezionare l'Eventuale Assicurazione Integrativa. <br/>")
            End If
        End If
        If (errore.ToString() <> String.Empty) Then
            lblerrore.Text = errore.ToString()
            campiValidi = False
        End If

        Return campiValidi

    End Function

    Protected Sub ImgStampa_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ImgStampa.Click
        Response.Write("<script type=""text/javascript"">")
        Response.Write("window.open(""WfrmReportistica.aspx?sTipoStampa=19&IdAttivita=" & CInt(Request.QueryString("IdAttivita")) & "&VengoDa=" & Request.QueryString("VengoDa") & """, ""Report"", ""height=800,width=800, ,dependent=no,scrollbars=no,status=no,resizable=yes"")")
        Response.Write("</script>")
    End Sub

    'sub che controlla lo stato del progetto così che mostro all'utente, qualora esso sia UNSC, 
    'il tasto che permette il collegamento del progetto ad un'istanza
    'lo stato del progetto deve essere registrato
    'true=registrato
    'false<>registrato
    Function CheckStatoProgetto(ByVal IdAttivita As String) As Boolean
        Dim dtrLocal As SqlClient.SqlDataReader
        Dim myCommand As New System.Data.SqlClient.SqlCommand
        Dim strCheckStatoProgetto As String
        Dim strIdregionecompetenzaUtente As String
        Dim strIdregionecompetenzaProgetto As String

        Try
            myCommand.Connection = Session("conn")

            strCheckStatoProgetto = "SELECT UtentiUNSC.IdRegioneCompetenza "
            strCheckStatoProgetto = strCheckStatoProgetto & "FROM UtentiUNSC "
            strCheckStatoProgetto = strCheckStatoProgetto & "WHERE  (UtentiUNSC.UserName = '" & Session("Utente") & "')"

            'eseguo la query
            myCommand.CommandText = strCheckStatoProgetto
            dtrLocal = myCommand.ExecuteReader

            If dtrLocal.HasRows = True Then
                dtrLocal.Read()
                strIdregionecompetenzaUtente = dtrLocal("IdRegioneCompetenza")
            End If

            strCheckStatoProgetto = "select statiattività.statoattività, isnull(attività.idregionecompetenza,0) as idregionecompetenza from statiattività "
            strCheckStatoProgetto = strCheckStatoProgetto & "inner join attività on statiattività.idstatoattività=attività.idstatoattività "
            strCheckStatoProgetto = strCheckStatoProgetto & "where attività.idattività='" & IdAttivita & "'"

            ChiudiDataReader(dtrLocal)
            myCommand.CommandText = strCheckStatoProgetto
            dtrLocal = myCommand.ExecuteReader
            If dtrLocal.HasRows = True Then
                dtrLocal.Read()
                strIdregionecompetenzaProgetto = dtrLocal("idregionecompetenza")
                If dtrLocal("statoattività") = "Registrato" Then
                    CheckStatoProgetto = True
                Else
                    CheckStatoProgetto = False
                End If

                If strIdregionecompetenzaProgetto <> strIdregionecompetenzaUtente Then
                    CheckStatoProgetto = False
                End If
            End If

        Catch ex As Exception
            Response.Write(ex.Message)
        Finally
            ChiudiDataReader(dtrLocal)
        End Try
    End Function


    Protected Sub txtCodEnteEventuali_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txtCodEnteEventuali.TextChanged
        Me.hdnIsPostbackCO.Value = Me.IsPostBack
        If (ddlEventualiCriteri.SelectedValue <> "3") Then
            txtCodEnteEventuali.Text = String.Empty
        End If
    End Sub

    Protected Sub txtPianoMonitoraggio_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txtPianoMonitoraggio.TextChanged
        Me.hdnIsPostbackCO.Value = Me.IsPostBack
        If (ddlPianoMonitoraggio.SelectedValue <> "2") Then
            txtPianoMonitoraggio.Text = String.Empty
        End If
    End Sub

    Protected Sub ddlPianoMonitoraggio_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlPianoMonitoraggio.SelectedIndexChanged
        lblMessaggioConferma.Text = String.Empty
        Me.hdnIsPostbackCO.Value = Me.IsPostBack
        If (ddlPianoMonitoraggio.SelectedValue <> "2") Then
            txtPianoMonitoraggio.Text = String.Empty
            txtPianoMonitoraggio.Enabled = False
        Else
            txtPianoMonitoraggio.Enabled = True
        End If
    End Sub

    Private Sub CostruisciTitolo()
        Dim progetto As StringBuilder = New StringBuilder()
        Dim dataInizio As Date
        Dim dataValida As Boolean = Date.TryParse(txtDataInizioPrevista.Text, dataInizio)
        If Not Request.QueryString("IdAttivita") Is Nothing Then
            progetto.Append("Gestione Progetto: '")
            progetto.Append(txtTitolo.Text)
            progetto.Append("' ")
            If (txtCodiceEnte.Text <> String.Empty) Then
                progetto.Append("( ")
                progetto.Append(txtCodiceEnte.Text)
                If (dataValida = True And dataInizio <= Date.Now) Then
                    progetto.Append(" - ")
                    progetto.Append(txtDataInizioPrevista.Text)
                End If
                progetto.Append(")")
            End If
        Else
            progetto.Append("Gestione Progetto")
        End If
        lblTitoloProgetto.Text = progetto.ToString()
    End Sub

    Protected Sub CheckMonteOreTipo_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles CheckMonteOreTipo.SelectedIndexChanged
        lblMessaggioConferma.Text = String.Empty
        Me.hdnIsPostbackProg.Value = Me.IsPostBack
        If CheckMonteOreTipo.SelectedValue = "0" Or CheckMonteOreTipo.SelectedValue = "selezionare" Then
            txtMonteOreAnnuo.Enabled = False
            txtMonteOreAnnuo.Text = String.Empty
            chkMonteOre.Selected = False
        Else
            If CheckMonteOreTipo.SelectedValue = "1" Then
                'txtMonteOreAnnuo.Enabled = True
                chkOreSett.Selected = False
                If ddlDurata.SelectedValue <> 0 Then
                    txtMonteOreAnnuo.Text = MonteOreAnnuo(ddlDurata.SelectedItem.Text)
                    txtMonteOreAnnuo.Enabled = False
                End If
            End If
        End If
    End Sub

    Protected Sub txtDurata_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txtDurata.TextChanged
        Me.hdnIsPostbackFG.Value = Me.IsPostBack
        Dim durata As Integer
        Dim numeroValido As Boolean = Integer.TryParse(txtDurata.Text, durata)
        If (numeroValido = True And ddlFormazioneGeneraleErogazione.SelectedValue = "percentuale") Then
            txt180.Text = Math.Round((durata * 80) / 100, MidpointRounding.AwayFromZero)
            txt270.Text = CInt(txtDurata.Text - txt180.Text)
        End If

    End Sub

    Protected Sub ddlFormazioneGeneraleErogazione_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlFormazioneGeneraleErogazione.SelectedIndexChanged
        lblMessaggioConferma.Text = String.Empty
        Me.hdnIsPostbackFG.Value = Me.IsPostBack
        Dim durata As Integer
        Dim numeroValido As Boolean = Integer.TryParse(txtDurata.Text, durata)
        If ddlFormazioneGeneraleErogazione.SelectedValue = "unicaTranche" Or ddlFormazioneGeneraleErogazione.SelectedValue = "selezionare" Then
            txt180.Text = "0"
            txt270.Text = "0"
        Else
            If ddlFormazioneGeneraleErogazione.SelectedValue = "percentuale" And numeroValido = True Then
                txt180.Text = Math.Round((durata * 80) / 100, MidpointRounding.AwayFromZero)
                txt270.Text = CInt(txtDurata.Text - txt180.Text)
            End If
        End If
    End Sub


    Protected Sub txtDurataSpec_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txtDurataSpec.TextChanged
        Me.hdnIsPostbackFS.Value = Me.IsPostBack
        Dim durata As Integer
        Dim numeroValido As Boolean = Integer.TryParse(txtDurataSpec.Text, durata)
        If (numeroValido = True And ddlFormazioneSpecificaErogazione.SelectedValue = "percentuale") Then
            txt90S.Text = Math.Round((durata * 70) / 100, MidpointRounding.AwayFromZero)
            txt270S.Text = CInt(txtDurataSpec.Text - txt90S.Text)
        End If
        'Math.Round()
    End Sub

    Protected Sub ddlFormazioneSpecificaErogazione_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlFormazioneSpecificaErogazione.SelectedIndexChanged
        lblMessaggioConferma.Text = String.Empty
        Me.hdnIsPostbackFS.Value = Me.IsPostBack
        Dim durata As Integer
        Dim numeroValido As Boolean = Integer.TryParse(txtDurataSpec.Text, durata)
        If ddlFormazioneSpecificaErogazione.SelectedValue = "unicaTranche" Or ddlFormazioneSpecificaErogazione.SelectedValue = "selezionare" Then
            txt90S.Text = "0"
            txt270S.Text = "0"
        Else
            If ddlFormazioneSpecificaErogazione.SelectedValue = "percentuale" And numeroValido = True Then
                txt90S.Text = Math.Round((durata * 70) / 100, MidpointRounding.AwayFromZero)
                txt270S.Text = CInt(txtDurataSpec.Text - txt90S.Text)
            End If
        End If
    End Sub

    Protected Sub ConfiguraPostBackSezioni()
        Dim isPostBack As Boolean = Me.IsPostBack
        Me.hdnIsPostbackFG.Value = isPostBack
        Me.hdnIsPostbackFS.Value = isPostBack
        Me.hdnIsPostbackProg.Value = isPostBack
        Me.hdnIsPostbackCO.Value = isPostBack
        Me.hdnIsPostbackCCA.Value = isPostBack
        Me.hdnIsPostbackEstero.Value = isPostBack
    End Sub

    Private Sub ReimpostaLabelMessaggi()
        lblerrore.Text = String.Empty
        lblMessaggioConferma.Text = String.Empty
    End Sub

    Protected Sub ddlDurata_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlDurata.SelectedIndexChanged

        If ddlDurata.SelectedValue <> 0 And CheckMonteOreTipo.SelectedValue = "1" Then
            txtMonteOreAnnuo.Text = MonteOreAnnuo(ddlDurata.SelectedItem.Text)
            txtMonteOreAnnuo.Enabled = False
        End If
    End Sub

    Private Sub chkGiovaniMinoriOp_CheckedChanged(sender As Object, e As System.EventArgs) Handles chkGiovaniMinoriOp.CheckedChanged
        'Me.hdnIsPostbackProg.Value = Me.IsPostBack
        ' Me.hdnIsPostbackProg.Value = Me.IsPostBack
        If chkGiovaniMinoriOp.Checked = True Then
            txtNumeroGiovaniMinoriOpportunita.Enabled = True
        Else
            txtNumeroGiovaniMinoriOpportunita.Text = ""
            txtNumeroGiovaniMinoriOpportunita.Enabled = False
        End If
    End Sub

    Protected Sub optEstero_CheckedChanged(sender As Object, e As EventArgs) Handles optEstero.CheckedChanged
        ' Me.hdnIsPostbackProg.Value = Me.IsPostBack
        ' Me.hdnIsPostbackProg.Value = Me.IsPostBack
        If optEstero.Checked = True Then
            divMesiUE.Visible = True
            divMesiTutoraggio.Visible = False
            txtMesiPrevistiTutoraggio.Text = ""
        End If
    End Sub

    Private Sub optTutoraggio_CheckedChanged(sender As Object, e As System.EventArgs) Handles optTutoraggio.CheckedChanged
        'Me.hdnIsPostbackProg.Value = Me.IsPostBack
        ' Me.hdnIsPostbackProg.Value = Me.IsPostBack
        If optTutoraggio.Checked = True Then
            divMesiUE.Visible = False
            divMesiTutoraggio.Visible = True
            txtMesiPrevistiUE.Text = ""
        End If

    End Sub

    Private Sub lkbClearOptEUE_T_Click(sender As Object, e As System.EventArgs) Handles lkbClearOptEUE_T.Click
        optEstero.Checked = False
        optTutoraggio.Checked = False
        txtMesiPrevistiTutoraggio.Text = ""
        txtMesiPrevistiUE.Text = ""
        divMesiTutoraggio.Visible = False
        divMesiUE.Visible = False
    End Sub
End Class