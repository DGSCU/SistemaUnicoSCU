Imports System.Collections.Generic
Imports System.Data.SqlClient
Imports System.Drawing
Imports System.Security.Cryptography
'Imports System.Text.RegularExpressions.Regex
Imports Logger.Data

Public Class WfrmAggiornaSediFuoriAdeguamento
    Inherits SmartPage
    Dim dtsGenerico As DataSet
    Dim strsql As String 'variabile stringa Che contiene stringa SQL
    Dim dtrGenerico As System.Data.SqlClient.SqlDataReader 'dichiarazione datareader
    Dim strNull As String 'variabile stringa che contiene valore NULL
    Dim ajaxHelper As AjaxHelper = New AjaxHelper()
    Dim bandiera As Integer
    Dim rstGenerico As SqlClient.SqlCommand
    Public IDESA As Integer
    Public strIdEnteSede As String
    Dim selComune As New clsSelezionaComune
    Dim helper As Helper = New Helper()
    Public AlboEnte As String
    Public ShowPopUPControllo As String
    'Public AnomaliaIndirizzo, AnomaliaNome As Integer
    Public IndirizzoRicerca As String
    Dim indirizzoErratoHelios As Boolean = False
    Dim indirizzoOkGoogle As Boolean = False
    Dim procediGM As Boolean = False
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
        ShowPopUPControllo = ""
        IndirizzoRicerca = ""
        'AnomaliaIndirizzo = 0
        'AnomaliaNome = 0
        Session("Procedi") = 0
        Session("ProcediGM") = 0
        Session("ProcediSalva") = 0

        procediGM = False

        VerificaSessione()
        If Request.Form("txtCancellaSede") = "DELETE" And Session("okok") = "ok" Then
            Session("attivaDel") = ""
            Session("okok") = ""
            EliminaSede()
        End If
        If Request.Form("txtCancellaSede") = "ANNULLA" And Session("okok") = "ok" Then
            Session("attivaDel") = ""
            Session("okok") = ""
            AnnullaAccreditamento()
        End If
        If Request.Form("txtCancellaSede") = "ANNUINC" And Session("okok") = "ok" Then
            Session("attivaDel") = ""
            Session("okok") = ""
            AnnullaInclusione()
        End If

        If Session("LoadedLSE") IsNot Nothing Then
            rowLSE.Visible = True
        End If

        If IsPostBack = False Then
            'carico la combo degli enti figli per cui poter inserire le sedi
            'Dim AlboEnte As String
            'AlboEnte = ClsUtility.TrovaAlboEnte(Session("IdEnte"), Session("Conn"))
            'If AlboEnte = "SCN" Then
            '    imgInfoPalazzina.Visible = False
            '    imgInfoScala.Visible = False
            '    imgInfoPiano.Visible = False
            '    imgInfoInterno.Visible = False
            'End If
            Session("LoadLSEId") = Nothing
            Session("LoadedLSE") = Nothing
            Session("AnomaliaNome") = 0
            Session("AnomaliaIndirizzo") = 0
            Session("AnomaliaIndirizzoGM") = 0

            AlboEnte = ClsUtility.TrovaAlboEnte(Session("IdEnte"), Session("Conn"))

            CaricaEntiFigli()
            CaricaCombo(AlboEnte)
            Dim idEnteSede As String = Request.QueryString("identesede")
            Session("IdEnteSede") = idEnteSede
            lblidEnte.Value = Request.QueryString("idente")
            IDEnteProprietario.Value = Request.QueryString("idente")
            LoadMaschera(IIf(idEnteSede = Nothing, 0, idEnteSede))
            'EvidenziaDatiModificati(idEnteSede)
            ChiudiDataReader(dtrGenerico)
            'controllo aggiunto da Michael Jonardan il 13.03.2006
            If Session("TipoUtente") = "E" Then
                GestioneCertificazioneNote.Visible = False

                'controllo stato ebnte
                ''If ClsUtility.ControllaStatoEntePerBloccareMaschereAnagrafica(Session("IdEnte"), IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn"))) = True Then
                ''    AbilitaCampiMaschera(False)
                ''    msgErrore.Text = "Non è possibile " & IIf(lblPersonalizza.Value = "inserimento", "inserire", "modificare") & " la sede."
                ''    cmdAccredita.Visible = False
                ''    cmdAnnulla.Visible = False
                ''    cmdAnnullaAccred.Visible = False
                ''    cmdCancella.Visible = False
                ''    cmdIncludi.Visible = False
                ''    cmdRipristina.Visible = False
                ''    cmdSalva.Visible = False
                ''    chkStatoEnte.Value = "True"
                ''    dgRisultatoRicerca.Columns(0).Visible = False
                ''    Exit Sub
                ''Else
                chkStatoEnte.Value = "False"
                ''End If
            Else
                GestioneCertificazioneNote.Visible = True
            End If







            If lblPersonalizza.Value = "Inserimento" Then
                ddlTipologia.AutoPostBack = False
                lblInfoProgetti.Visible = False
                fsOperativita.Visible = False
            End If



            ''If lblPersonalizza.Value <> "Inserimento" Then
            ''    strsql = "select * from entisedi where IdEntesede=" & txtidsede.Value
            ''    'dtrGenerico = ClsServer.EseguiSqlClient(strsql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
            ''    dtrGenerico = ClsServer.CreaDatareader(strsql, Session("conn"))
            ''    dtrGenerico.Read()
            ''    If (Session("TipoUtente") = "U" Or Session("TipoUtente") = "R") Then
            ''        If Not dtrGenerico Is Nothing Then
            ''            dtrGenerico.Close()
            ''            dtrGenerico = Nothing
            ''        End If

            ''    Else

            ''        If UCase(lblStato.Text) = "PRESENTATA" Then

            ''            If Not dtrGenerico Is Nothing Then
            ''                dtrGenerico.Close()
            ''                dtrGenerico = Nothing
            ''            End If
            ''        Else
            ''            txtPrefFax.ReadOnly = False
            ''            txtprefisso.ReadOnly = False

            ''            ddlProvincia.Enabled = False
            ''            txtTelefono.ReadOnly = False
            ''            ddlTipologia.Enabled = False
            ''            cmdEmail.Enabled = False
            ''            cmdHttp.Enabled = False
            ''            lblDataControlloHttp.Visible = False
            ''            lbldataControlloEmail.Visible = False
            ''            chkFalsohttp.Visible = False
            ''            chkVerohttp.Visible = False
            ''            chkFalsoEmail.Visible = False
            ''            chkVeroEmail.Visible = False
            ''            lblsedeInclusa.Visible = True
            ''            ddlEntiFigli.Enabled = False


            ''            If Not dtrGenerico Is Nothing Then
            ''                dtrGenerico.Close()
            ''                dtrGenerico = Nothing
            ''            End If
            ''        End If
            ''    End If
            ''End If
            'End If
            '''''---------------------------FINE ANTONELLO------------------------------------

            '------------------------------------------------------------------------------------------

            'FZ controllo per disabilitare la maschera nel caso sia un'"R" che sta 
            'controllando i progetti di un' ente che nn ha la stessa regiojneee di competenza
            If ClsUtility.ControlloRegioneCompetenza(Session("TipoUtente"), Session("idEnte"), Session("Utente"), Session("IdStatoEnte"), IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn"))) = False Then
                AbilitaCampiMaschera(False)
                msgErrore.Text = "L'ente non è di propria competenza. Impossibile effettuare modifiche."
                cmdAccredita.Visible = False
                cmdAnnulla.Visible = False
                cmdAnnullaAccred.Visible = False
                cmdCancella.Visible = False
                cmdIncludi.Visible = False
                cmdRipristina.Visible = False
                'cmdSalva.Visible = False
                'Response.Write("<input type=hidden name=chkStatoEnte value=""True"">")
                dgRisultatoRicerca.Columns(0).Visible = False

            End If
            'FZ fine controllo
            If txtidsede.Value = "" Then
                lblInfoProgetti.Visible = False
            End If
            If Session("TipoUtente") = "E" Then
                If lblPersonalizza.Value <> "Inserimento" Then

                    'agg. il 26/09/2013  per nuovo accreditamento/adeguamento inizio Ottobre 2013
                    'Verifiche sulla sede 
                    If VerificaPresenzaSedesuProgettiAttivi_InValutazione(Request.QueryString("identesede")) = True Then 'nn modificabile
                        ' AbilitaCampiMaschera(False)
                        'msgErrore.Text = "Non è possibile modificare la sede perchè è associata ad uno o più Progetti in corso, in attesa di avvio o in attesa di valutazione."
                        cmdAccredita.Visible = False
                        cmdAnnulla.Visible = False
                        cmdAnnullaAccred.Visible = False
                        cmdCancella.Visible = False
                        cmdIncludi.Visible = False
                        cmdRipristina.Visible = False
                        'cmdSalva.Visible = False
                        dgRisultatoRicerca.Columns(0).Visible = False

                    End If
                End If
            End If

            If Request.QueryString("tipoazione") = "Inserimento" Then
                lblidEnte.Value = Session("Idente")
                Dim selComune As New clsSelezionaComune
                selComune.CaricaProvincie(ddlProvincia, ChkEstero.Checked, Session("Conn"))
                ddlComune.DataSource = Nothing
                ddlComune.Items.Add("")
                ddlComune.SelectedIndex = 0
                ddlComune.Enabled = False
            End If
            If AlboEnte = "SCU" Then
                lblAltroTitoloGiuridico.Visible = False
                txtAltroTitoloGiuridicio.Visible = False
                If Session("TipoUtente") = "U" Then
                    lblInfoCronologiaSedi.Visible = True
                End If
            Else
                lblInfoCronologiaSedi.Visible = False
                lblAltroTitoloGiuridico.Visible = True
                txtAltroTitoloGiuridicio.Visible = True
            End If
            If Session("TipoUtente") = "U" And Request.QueryString("tipoazione") <> "Inserimento" Then
                If ClsUtility.ControlloRichiestaModificaSede(Request.QueryString("identesede"), Session("Conn")) = True Then
                    imgRichiestaModifica.Visible = False
                    imgAnnullaRichiestaModifica.Visible = True
                Else
                    imgRichiestaModifica.Visible = True
                    imgAnnullaRichiestaModifica.Visible = False
                End If

            End If
        End If
        If txtidsede.Value <> "" Then 'se sono in inserimento sede salto le istruzioni che seguono ADC

            If Session("TipoUtente") = "E" Then
                divSospendiSede.Visible = False
            Else
                'SEDE DI ENTE TRANSITATO SU SCU
                If Not dtrGenerico Is Nothing Then
                    dtrGenerico.Close()
                    dtrGenerico = Nothing
                End If
                strsql = "SELECT entisedi.IDEnteSede,entisediattuazioni.IDEnteSedeAttuazione,entisedi.IDStatoEnteSede,StatiEntiSedi.StatoEnteSede,ChiusureSediTransitate.IdNuovoStato,ChiusureSediTransitate.IdVecchioStato, isnull(ChiusureSediTransitate.definitiva,0) as definitiva  FROM entisedi INNER JOIN entisediattuazioni ON entisedi.IDEnteSede = entisediattuazioni.IDEnteSede INNER JOIN StatiEntiSedi ON entisediattuazioni.IdStatoEnteSede = StatiEntiSedi.IdStatoEnteSede INNER JOIN ChiusureSediTransitate ON entisedi.IDEnteSede=ChiusureSediTransitate.IdEnteSede WHERE ChiusureSediTransitate.IdEnteSede=" & txtidsede.Value
                'dtrGenerico = ClsServer.EseguiSqlClient(strsql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
                dtrGenerico = ClsServer.CreaDatareader(strsql, Session("conn"))
                dtrGenerico.Read()

                If dtrGenerico.HasRows = True Then
                    divSospendiSede.Visible = True
                    If dtrGenerico("IDStatoEnteSede") = 2 Then
                        lblStatoSede.Text = "CHIUSA PER TRANSITO SCU"
                        If dtrGenerico("definitiva") = 0 Then
                            CmdPortaInSospeso.Visible = True
                        Else
                            CmdPortaInSospeso.Visible = False
                        End If
                    Else
                        lblStatoSede.Text = "SOSPESA PER TRANSITO SCU"
                        CmdPortaInSospeso.Visible = False
                    End If
                End If

                'SEDE DI ENTE CHIUSO PER DISMISSIONE ALBO SCN
                If Not dtrGenerico Is Nothing Then
                    dtrGenerico.Close()
                    dtrGenerico = Nothing
                End If
                strsql = "SELECT entisedi.IDEnteSede,entisediattuazioni.IDEnteSedeAttuazione,entisedi.IDStatoEnteSede,StatiEntiSedi.StatoEnteSede,ChiusureSediDismissioneAlboSCN.IdNuovoStato,ChiusureSediDismissioneAlboSCN.IdVecchioStato,isnull(ChiusureSediDismissioneAlboSCN.definitiva,0) as definitiva  FROM entisedi INNER JOIN entisediattuazioni ON entisedi.IDEnteSede = entisediattuazioni.IDEnteSede INNER JOIN StatiEntiSedi ON entisediattuazioni.IdStatoEnteSede = StatiEntiSedi.IdStatoEnteSede INNER JOIN ChiusureSediDismissioneAlboSCN ON entisedi.IDEnteSede=ChiusureSediDismissioneAlboSCN.IdEnteSede WHERE ChiusureSediDismissioneAlboSCN.IdEnteSede=" & txtidsede.Value
                'dtrGenerico = ClsServer.EseguiSqlClient(strsql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
                dtrGenerico = ClsServer.CreaDatareader(strsql, Session("conn"))
                dtrGenerico.Read()

                If dtrGenerico.HasRows = True Then
                    divSospendiSede.Visible = True
                    If dtrGenerico("IDStatoEnteSede") = 2 Then
                        lblStatoSede.Text = "CHIUSA PER DISMISSIONE ALBO SCN"

                        If dtrGenerico("definitiva") = 0 Then
                            CmdPortaInSospeso.Visible = True
                        Else
                            CmdPortaInSospeso.Visible = False
                        End If
                    Else
                        lblStatoSede.Text = "SOSPESA PER DISMISSIONE ALBO SCN"
                        CmdPortaInSospeso.Visible = False
                    End If
                End If

                If Not dtrGenerico Is Nothing Then
                    dtrGenerico.Close()
                    dtrGenerico = Nothing
                End If
            End If
        End If

        For Each i As ListItem In ddlTitoloGiuridico.Items
            If i.Value = 7 Then
                If ChkEstero.Checked = False Then
                    i.Enabled = False
                Else
                    i.Enabled = True
                End If

            End If

        Next
        If ChkEstero.Checked = False Then
            DivItalia.Visible = True
            DivEstero2.Visible = False
        Else
            DivItalia.Visible = False
            If Session("LoadedLSE") Is Nothing Then
                rowNoLSE.Visible = True
                rowLSE.Visible = False
            Else
                rowNoLSE.Visible = False
                rowLSE.Visible = True
            End If
            DivEstero2.Visible = True
            If ddlTitoloGiuridico.SelectedValue = 7 Then
                DivSoggettoEstero.Visible = True
                DivNoSoggettoEstero.Visible = False
            Else
                DivSoggettoEstero.Visible = False
                DivNoSoggettoEstero.Visible = True
            End If
        End If

    End Sub
#Region "Richiesta Modifica Sede"


    Private Sub AggiornaRichiestaModifica(ByVal AvvisoSede As Integer, ByVal IdEnteSede As Integer)
        Dim myQuerySql As String
        Dim Cmd As SqlClient.SqlCommand
        Try
            myQuerySql = "Update EntiSedi Set AvvisoSede =  " & AvvisoSede & " where IdEntesede=" & IdEnteSede
            Cmd = New SqlClient.SqlCommand(myQuerySql, Session("conn"))
            Cmd.ExecuteNonQuery()
            myQuerySql = "INSERT INTO CronologiaEntiSediRichiestaModifica (IdEntesede,AvvisoSede, UsernameRichiesta, DataRichiestaModifica)"
            myQuerySql &= " VALUES (" & IdEnteSede & "," & AvvisoSede & ",'" & Session("Utente") & "',getdate())"
            Cmd = New SqlClient.SqlCommand(myQuerySql, Session("conn"))
            Cmd.ExecuteNonQuery()


        Catch ex As Exception
            msgErrore.Visible = True
            msgErrore.Text = "Contattare l'assistenza."
        End Try

    End Sub


#End Region

#Region "Operazioni DB"
    Private Sub CronologiaStatiEntiSedi()
        'Realizzato da Alessandra Taballione il 21.06.2004
        'Prima di effettuare il cambio dello stato Inserisco il vecchio stato nella CronologiaEntiSedi
        strsql = "insert into CronologiaEntiSedi (identeSede,idstatoentesede,dataCronologia,idtipocronologia,UsernameStato)" &
            " select " & txtidsede.Value & ", idstatoEnteSede,getdate(),0,'" & Session("Utente") & "' " &
            " from entiSedi where identesede= " & txtidsede.Value & ""
        If Not dtrGenerico Is Nothing Then
            dtrGenerico.Close()
            dtrGenerico = Nothing
        End If
        rstGenerico = ClsServer.EseguiSqlClient(strsql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))

    End Sub
    Sub BK()
        If Not dtrGenerico Is Nothing Then
            dtrGenerico.Close()
            dtrGenerico = Nothing
        End If
        'Verifico l'esistenza in entivariazione sedi dell'IDENTESEDE
        ' Se esiste non devo fare nulla  txtidsede.Value su enti variazionisedi
        dtrGenerico = ClsServer.CreaDatareader("select identesede from entivariazionisedi where identesede='" & txtidsede.Value & "'", IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
        dtrGenerico.Read()

        If dtrGenerico.HasRows = False Then
            'insert backup
            strsql = "Insert into entivariazionisedi (identesede,idente,denominazione,indirizzo,DataVariazione,Username) values (" & txtidsede.Value & ","
            If ControllaEnteFiglio(ddlEntiFigli.SelectedValue) = False Then
                strsql = strsql & lblidEnte.Value
            Else
                strsql = strsql & ddlEntiFigli.SelectedValue
            End If
            strsql = strsql & ",'" & Replace(txtCopiaSEDE.Value, "'", "''") & "','" & Replace(txtCopiaIndirizzo.Value, "'", "''") & "', getdate(),'" & Session("Utente") & "')"
            If Not dtrGenerico Is Nothing Then
                dtrGenerico.Close()
                dtrGenerico = Nothing
            End If
            rstGenerico = ClsServer.EseguiSqlClient(strsql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
            If Not dtrGenerico Is Nothing Then
                dtrGenerico.Close()
                dtrGenerico = Nothing
            End If
        End If
        If Not dtrGenerico Is Nothing Then
            dtrGenerico.Close()
            dtrGenerico = Nothing
        End If

    End Sub
    Private Sub CronologiaStatiEntisediAttuazioni()

        'Prima di effettuare il cambio dello stato Inserisco il vecchio stato nella CronologiaEntiSediAttuazioni
        Dim sediAtt As New Collection
        Dim i As Integer

        'strsql = "select * from entisediattuazioni where idEnteSede=" & txtidsede.Value & " and idstatoEntesede=(Select idstatoentesede from statientisedi where DaAccreditare=1 and defaultstato=1)"
        strsql = "select * from entisediattuazioni where idEnteSede=" & txtidsede.Value
        If Not dtrGenerico Is Nothing Then
            dtrGenerico.Close()
            dtrGenerico = Nothing
        End If
        dtrGenerico = ClsServer.CreaDatareader(strsql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
        Do While dtrGenerico.Read()
            sediAtt.Add(dtrGenerico("identesedeattuazione"))
        Loop
        For i = 1 To sediAtt.Count
            'Prima di effettuare il cambio dello stato Inserisco il vecchio stato nella CronologiaEntiSedi
            strsql = "insert into CronologiaEntiSediAttuazione (identeSedeAttuazione,idstatoentesede,dataCronologia,idtipocronologia,UsernameStato)" &
                " select " & sediAtt.Item(i) & ", idstatoEnteSede,getdate(),0,'" & Session("Utente") & "' " &
                " from entiSediAttuazioni where identesedeAttuazione= " & sediAtt.Item(i) & ""
            If Not dtrGenerico Is Nothing Then
                dtrGenerico.Close()
                dtrGenerico = Nothing
            End If
            rstGenerico = ClsServer.EseguiSqlClient(strsql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
        Next
    End Sub

    Private Sub Inserimento()
        Dim strIdEnteCapoFila As String
        ChiudiDataReader(dtrGenerico)
        If ControlloComuneEstero() = False Then ' COMNUE ITALIANO
            If ControllaEnteFiglio(ddlEntiFigli.SelectedValue) = True Then
                'controllo se l'ente figlio è in accordo di partenariato richiamo store 
                strsql = "Select * FROM enti WHERE IDEnte = " & ddlEntiFigli.SelectedValue & "  AND IdClasseAccreditamentoRichiesta = 7 "
                dtrGenerico = ClsServer.CreaDatareader(strsql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
                If dtrGenerico.HasRows = True Then
                    If Not dtrGenerico Is Nothing Then
                        dtrGenerico.Close()
                        dtrGenerico = Nothing
                    End If
                    Dim strMessaggioRitornoStore As String
                    ' richiamo store che controlla il numero delle sedi partner solo se inserisco sedi di un ente partener
                    strMessaggioRitornoStore = LeggiStoreVerificaSediPartner(lblidEnte.Value, 1)
                    If strMessaggioRitornoStore <> "OK" Then
                        msgErrore.Text = strMessaggioRitornoStore
                        Log.Warning(LogEvent.SEDI_INSERIMENTO_ERRORE, strMessaggioRitornoStore)
                        Exit Sub
                    End If
                End If
                ChiudiDataReader(dtrGenerico)
            End If
        End If
        'agg. da simona cordella il 16/06/2009
        'verifico se i dati della sede che sto inserendo (indirizzo,civico,comune,palazzina,scala,piano,intrno) non sono già prensente nel db
        If ControlloDoppioneSedeNuova(lblidEnte.Value, txtIndirizzo.Text, txtCivico.Text, ddlComune.SelectedValue, Trim(TxtPalazzina.Text), Trim(TxtScala.Text), Trim(TxtPiano.Text), Trim(TxtInterno.Text)) = True Then
            cmdSalva.Visible = True
            'Inserisco il Messaggio di Errore
            msgErrore.Text = "Sede già presente in archivio."
            Log.Warning(LogEvent.SEDI_INSERIMENTO_ERRORE, "Sede già presente in archivio.")
            Exit Sub
        End If


        'vado a controllare se esiste già la denominazione che si vuole inserire 
        'per l'ente selezionato oppure per l'ente padre
        If ControllaEnteFiglio(ddlEntiFigli.SelectedValue) = False Then
            dtrGenerico = ClsServer.CreaDatareader("Select identeSede from entisedi where denominazione='" & Replace(txtdenominazione.Text, "'", "''") & "' and idEnte=" & lblidEnte.Value & "", IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
        Else
            dtrGenerico = ClsServer.CreaDatareader("Select identeSede from entisedi where denominazione='" & Replace(txtdenominazione.Text, "'", "''") & "' and idEnte=" & ddlEntiFigli.SelectedValue & "", IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
        End If
        If dtrGenerico.HasRows = True Then
            'Ripristino lo stato del tasto
            cmdSalva.Visible = True
            'Inserisco il Messaggio di Errore
            msgErrore.Text = "La denominazione della Sede è già presente in archivio!"
            dtrGenerico.Close()
            dtrGenerico = Nothing
            Log.Warning(LogEvent.SEDI_INSERIMENTO_ERRORE, "La denominazione della Sede è già presente in archivio! (id:" & ddlEntiFigli.SelectedValue & ")")
            Exit Sub
        End If
        dtrGenerico.Close()
        dtrGenerico = Nothing

        'vado a controllare se esiste già la il codiceentesede che si vuole inserire 
        'per l'ente selezionato oppure per l'ente padre
        If ControllaEnteFiglio(ddlEntiFigli.SelectedValue) = False Then
            'Verifica della Univocità del codiceEnteSede per Ente Padre
            dtrGenerico = ClsServer.CreaDatareader("select codicesedeente from entisedi where idEnte=" & lblidEnte.Value & " and codiceSedeEnte='" & Replace(txtCodice.Value, "'", "''") & "'", IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
        Else
            'Verifica della Univocità del codiceEnteSede per Ente Figlio
            dtrGenerico = ClsServer.CreaDatareader("select codicesedeente from entisedi where idEnte=" & ddlEntiFigli.SelectedValue & " and codiceSedeEnte='" & Replace(txtCodice.Value, "'", "''") & "'", IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
        End If
        If dtrGenerico.HasRows = True Then
            'Ripristino lo stato del tasto
            cmdSalva.Visible = True
            'Inserisco il Messaggio di Errore
            msgErrore.Text = "Il codice Sede è già presente in archivio!"
            dtrGenerico.Close()
            dtrGenerico = Nothing
            Log.Warning(LogEvent.SEDI_INSERIMENTO_ERRORE, "Il codice Sede è già presente in archivio! (Codice:" & txtCodice.Value & ")")
            Exit Sub
        End If
        dtrGenerico.Close()
        dtrGenerico = Nothing


        Dim strMiaCausale As String = ""

        If ddlComune.SelectedValue <> "0" And Not procediGM Then
            If ClsUtility.CAP_VERIFICA(IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")),
                strMiaCausale, bandiera, Trim(txtCap.Text), ddlComune.SelectedValue, "", "", txtIndirizzo.Text, txtCivico.Text) = False Then
                'Ripristino lo stato del tasto
                cmdSalva.Visible = True
                'Inserisco il Messaggio di Errore
                msgErrore.Text = strMiaCausale
                If Not dtrGenerico Is Nothing Then
                    dtrGenerico.Close()
                    dtrGenerico = Nothing

                End If
                Log.Warning(LogEvent.SEDI_INSERIMENTO_ERRORE, strMiaCausale)
                Exit Sub
            End If
        Else

            If ChkEstero.Checked And ddlComune.SelectedValue <= 0 Then
                ddlComune.ClearSelection()
                ddlComune.Items.FindByText(ddlProvincia.SelectedItem.Text).Selected = True
            End If

        End If

        If ChkEstero.Checked And ddlTitoloGiuridico.SelectedValue = 7 Then 'se estero deve esserci l'allegato e lo devo salvare
            Dim LSE As Allegato = Session("LoadedLSE")

            If LSE Is Nothing Then
                msgErrore.Text = "L'allegato Lettera di accordo con sede estera è obbligatorio<br/>"
                Log.Warning(LogEvent.SEDI_INSERIMENTO_ERRORE, "L'allegato Lettera di accordo con sede estera è obbligatorio")
                Exit Sub
            End If
            ' Salva Allegato 
            'Creazione Allegato
            Dim idAllegato As Integer = SalvaAllegato(LSE, TipoFile.LETTERA_SEDE_ESTERA, 0)
            Session("LoadLSEId") = idAllegato
            Log.Information(LogEvent.SEDI_IMPORTAZIONE_LSE)
        End If




        'aggiungere i campi idllegato e errore erroriIndirizzo errorinome
        'Creazione Stringa SQL per l'inserimento della sede dell'Ente


        strNull = "null"
        strsql = "Insert Into entiSedi(idente,denominazione,Indirizzo,DettaglioRecapito,FlagIndirizzoValido, "
        strsql = strsql & "civico,idcomune,cap,PrefissoTelefono,telefono,PrefissoFax,Fax, Palazzina, Scala, Piano, Interno,IdTitoloGiuridico,AltroTitoloGiuridico, http,"
        strsql = strsql & "dataControllohttp,httpvalido,email,datacontrolloEmail,EmailValido, "
        strsql = strsql & "idStatoenteSede,UsernameStato,datacreazionerecord,RiferimentoRimborsi,CittaEstera,IdAllegatoLSE,AnomaliaIndirizzo, AnomaliaNome,NormativaTutela,"
        strsql = strsql & " SoggettoEstero,NonDisponibilitaSede,DisponibilitaSede, AnomaliaIndirizzoGoogle, Localita) values ( "
        'controllo se si tratta di ente figlioo ente padre
        If ControllaEnteFiglio(ddlEntiFigli.SelectedValue) = False Then
            strsql = strsql & lblidEnte.Value & ",'" & Replace(txtdenominazione.Text, "'", "''") & "'"
        Else
            strsql = strsql & ddlEntiFigli.SelectedValue & ",'" & Replace(txtdenominazione.Text, "'", "''") & "'"
        End If
        strsql = strsql & ",'" & Replace(txtIndirizzo.Text, "'", "''") & "','" & Replace(TxtDettaglioRecapito.Text, "'", "''") & "'," & bandiera & ",'" & Replace(txtCivico.Text, "'", "''") & "'"
        strsql = strsql & "," & ddlComune.SelectedValue & ",'" & txtCap.Text & "','" & Trim(txtprefisso.Text) & "','" & txtTelefono.Text & "',"

        'Prefisso FAx
        If Trim(txtPrefFax.Text) <> "" Then
            strsql = strsql & "'" & Trim(txtPrefFax.Text) & "',"
        Else
            strsql = strsql & strNull & ","
        End If

        'FAX
        If Trim(txtfax.Text) <> "" Then
            strsql = strsql & "'" & txtfax.Text & "',"
        Else
            strsql = strsql & strNull & ","
        End If
        '**** agg. da simona cordella il 19/05/2009 per nuovo accreditamento luglio 2009
        'Palazzina
        If Trim(TxtPalazzina.Text) <> "" Then
            strsql = strsql & "'" & Replace(TxtPalazzina.Text, "'", "''") & "',"
        Else
            strsql = strsql & strNull & ","
        End If
        'Scala
        If Trim(TxtScala.Text) <> "" Then
            strsql = strsql & "'" & Replace(TxtScala.Text, "'", "''") & "',"
        Else
            strsql = strsql & strNull & ","
        End If
        'Piano
        If Trim(TxtPiano.Text) <> "" Then
            strsql = strsql & "'" & TxtPiano.Text & "',"
        Else
            strsql = strsql & strNull & ","
        End If
        ' Interno
        If Trim(TxtInterno.Text) <> "" Then
            strsql = strsql & "'" & TxtInterno.Text & "',"
        Else
            strsql = strsql & strNull & ","
        End If
        ' IdTitoloGiuridico
        If ddlTitoloGiuridico.SelectedValue <> 0 Then
            strsql = strsql & " " & ddlTitoloGiuridico.SelectedValue & ","
        Else
            strsql = strsql & strNull & ","
        End If
        ' agg. il 03/07/2009 da s.c.
        'se indico un titolo giuridico =ALTRO specifico nella text il titolo giuridico
        If ddlTitoloGiuridico.SelectedItem.Text = "Altro" Then
            strsql = strsql & " '" & txtAltroTitoloGiuridicio.Text.Replace("'", "''") & "',"
        Else
            strsql = strsql & strNull & ","
        End If
        '*******************
        'Http
        If Trim(txthttp.Text) <> "" Then
            strsql = strsql & "'" & Replace(txthttp.Text, "'", "''") & "',"
        Else
            strsql = strsql & strNull & ","
        End If

        If Trim(lblDataControlloHttp.Value) <> "" Then
            strsql = strsql & "'" & lblDataControlloHttp.Value & "',"
        Else
            strsql = strsql & strNull & ","
        End If

        'Controllo sul check del verificato Http
        If chkVerohttp.Checked = True And chkFalsohttp.Checked = False Then
            strsql = strsql & "1,"
        End If
        If chkVerohttp.Checked = False And chkFalsohttp.Checked = True Then
            strsql = strsql & "0,"
        End If
        If chkVerohttp.Checked = False And chkFalsohttp.Checked = False Then
            strsql = strsql & "0,"
        End If
        If Trim(txtemail.Text) <> "" Then
            strsql = strsql & "'" & Replace(txtemail.Text, "'", "''") & "',"
        Else
            strsql = strsql & strNull & ","
        End If
        If Trim(lbldataControlloEmail.Value) <> "" Then
            strsql = strsql & "'" & lbldataControlloEmail.Value & "',"
        Else
            strsql = strsql & strNull & ","
        End If
        'Controllo sul check del verificato Email
        If chkFalsoEmail.Checked = True And chkVeroEmail.Checked = False Then
            strsql = strsql & "0,"
        End If
        If chkFalsoEmail.Checked = False And chkVeroEmail.Checked = True Then
            strsql = strsql & "1,"
        End If
        If chkFalsoEmail.Checked = False And chkVeroEmail.Checked = False Then
            strsql = strsql & "0,"
        End If
        'trovo stato dell'ente Padre e lo inserisco
        If Not dtrGenerico Is Nothing Then
            dtrGenerico.Close()
            dtrGenerico = Nothing
        End If
        dtrGenerico = ClsServer.CreaDatareader(" SELECT idstatoentesede FROM STATientisedi  where defaultStato=1 ", IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
        dtrGenerico.Read()
        strsql = strsql & " " & dtrGenerico("idstatoentesede") & ",'" & Replace(Session("Utente"), "'", "''") & "',getdate()"
        If chkRiferimentoRimborsi.Checked = True Then
            strsql = strsql & ",1"
        Else
            strsql = strsql & ",0"
        End If

        If ChkEstero.Checked = True Then
            strsql = strsql & "," & "'" & Replace(Trim(txtCity.Text), "'", "''") & "'"
        Else
            strsql = strsql & "," & strNull

        End If
        '**** inserire qui i valori dei nuovi campi
        If ChkEstero.Checked And ddlTitoloGiuridico.SelectedValue = 7 Then
            strsql = strsql & "," & Session("LoadLSEId")
        Else
            strsql = strsql & "," & strNull
        End If
        strsql = strsql & "," & Session("AnomaliaIndirizzo")
        strsql = strsql & "," & Session("AnomaliaNome")

        If ChkEstero.Checked = False Then
            If ChkTutela.Checked Then
                strsql = strsql & ",1"
            Else
                strsql = strsql & ",0"
            End If
        Else
            strsql = strsql & ",0"
        End If

        If ChkEstero.Checked Then


            strsql = strsql & "," & "'" & TxtSoggettoCapoSede.Text & "'"
            If rbSE2.Checked Then
                strsql = strsql & ",1"
            Else
                strsql = strsql & ",0"
            End If
            If rbNoSE1.Checked Then
                strsql = strsql & ",0"
            Else
                strsql = strsql & ",1"
            End If
        Else
            strsql = strsql & ",null,null,null"
        End If
        strsql = strsql & "," & Session("AnomaliaIndirizzoGM")
        If ChkEstero.Checked Then
            strsql = strsql & ",'" & txtLocalita.Text & "'"
        Else
            strsql = strsql & ",''"
        End If
        strsql = strsql & ")"

        If Not dtrGenerico Is Nothing Then
            dtrGenerico.Close()
            dtrGenerico = Nothing
        End If
        rstGenerico = ClsServer.EseguiSqlClient(strsql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
        'inserisco relazione EntisedeTipi - entisedi
        If Not dtrGenerico Is Nothing Then
            dtrGenerico.Close()
            dtrGenerico = Nothing
        End If
        'If ControllaEnteFiglio(ddlEntiFigli.SelectedValue) = False Then
        '    dtrGenerico = ClsServer.CreaDatareader("select identesede from entisedi where idente=" & lblidEnte.Value & " and denominazione='" & Trim(Replace(txtdenominazione.Text, "'", "''")) & "'", IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
        'Else
        '    dtrGenerico = ClsServer.CreaDatareader("select identesede from entisedi where idente=" & ddlEntiFigli.SelectedValue & " and denominazione='" & Trim(Replace(txtdenominazione.Text, "'", "''")) & "'", IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
        'End If
        dtrGenerico = ClsServer.CreaDatareader("select @@identity as identesede ", IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
        dtrGenerico.Read()
        strIdEnteSede = dtrGenerico("identesede")

        strsql = "insert into entiseditipi (identesede,idtiposede) values " &
            " (" & strIdEnteSede & "," & ddlTipologia.SelectedValue.ToString & ")"
        'se si tratta di un ente figlio assegno ad una variabile pubblica l'id della sede appena inserita
        'cos' che lo passo successivamente in fase di inclusione delle sedi
        If Not dtrGenerico Is Nothing Then
            dtrGenerico.Close()
            dtrGenerico = Nothing
        End If
        rstGenerico = ClsServer.EseguiSqlClient(strsql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
        msgConferma.Text = "Inserimento SEDE eseguito con successo."
        msgConferma.Text = msgConferma.Text & "<br/>"
        If Session("AnomaliaNome") = 1 Or Session("AnomaliaIndirizzo") = 1 Or Session("AnomaliaIndirizzoGM") = 1 Then
            Log.Warning(LogEvent.SEDI_INSERIMENTO_ANOMALIE, "IdEnteSede: " & strIdEnteSede)
        Else
            Log.Information(LogEvent.SEDI_INSERIMENTO_CORRETTO, "IdEnteSede: " & strIdEnteSede,,)
        End If

        'PulisciMaschera()
        If Not dtrGenerico Is Nothing Then
            dtrGenerico.Close()
            dtrGenerico = Nothing
        End If
        Dim bytFlagMaxVolontari As Byte
        If TxtNumMaxVol.Text > 20 Then
            bytFlagMaxVolontari = 1
        Else
            bytFlagMaxVolontari = 0
        End If
        If ControllaEnteFiglio(ddlEntiFigli.SelectedValue) = True Then
            IncludiSede()
            If ddlTipologia.SelectedItem.Text = "Operativa" Then

                If Not dtrGenerico Is Nothing Then
                    dtrGenerico.Close()
                    dtrGenerico = Nothing
                End If

                strsql = "SELECT identepadre FROM entirelazioni WHERE DataFinevalidità is null AND identefiglio='" & Session("IdEnte") & "'"

                dtrGenerico = ClsServer.CreaDatareader(strsql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
                If dtrGenerico.HasRows = True Then
                    dtrGenerico.Read()
                    strIdEnteCapoFila = dtrGenerico("identepadre")
                Else
                    strIdEnteCapoFila = Session("IdEnte")
                End If

                If Not dtrGenerico Is Nothing Then
                    dtrGenerico.Close()
                    dtrGenerico = Nothing
                End If

                '***************Aggiunto da Alessandra Taballione il 29/07/2005
                'Aggiunta in sedeAttuazione
                'modificata da Simona Cordella il 11/12/2008
                'aggiungo inserimento del campo CERTIFICAZIONE =2 (DA VALUTARE)-- solo se sono un ente 
                'agg. da simona cordella il 19/05/2009 campo NMAXVOLONTARI
                strsql = "insert into entiSediattuazioni (Denominazione,identeSede,UsernameStato,idStatoEnteSede,UsernameInseritore,DataInserimento,IdEnteCapofila,Certificazione,DataCertificazione ,UserCertificazione ,NMaxVolontari,FlagMaxVolontari, UsernameMaxVolontari,Note ) " &
                            "select '" & Replace(txtdenominazione.Text, "'", "''") & "'," & strIdEnteSede & ","
                strsql = strsql & "'" & Replace(Session("Utente"), "'", "''") & "', entisedi.idstatoEntesede, '" & Session("Utente") & "', getDate(), '" & strIdEnteCapoFila & "', "

                'gestione certificazione per regioni e unsc
                If (Session("TipoUtente") = "U" Or Session("TipoUtente") = "R") Then
                    If ddlCertificazione.SelectedValue = 2 Then
                        strsql = strsql & " " & ddlCertificazione.SelectedValue & ", null , null, "
                    Else
                        strsql = strsql & " " & ddlCertificazione.SelectedValue & ",getdate(),'" & Session("Utente") & "',"
                    End If
                Else
                    strsql = strsql & " 2, null , null, "
                End If
                strsql = strsql & " " & TxtNumMaxVol.Text & ", "
                If bytFlagMaxVolontari = 0 Then
                    strsql = strsql & " " & bytFlagMaxVolontari & ", null "
                Else
                    strsql = strsql & " " & bytFlagMaxVolontari & ", '" & Replace(Session("Utente"), "'", "''") & "' "
                End If
                strsql = strsql & " ,'" & txtNote.Text.Replace("'", "''") & "'"

                strsql = strsql & " from StatiEntiSedi " &
                " inner join entisedi  on StatiEntiSedi.idstatoentesede=entisedi.idstatoentesede where identesede=" & strIdEnteSede & ""
                If Not dtrGenerico Is Nothing Then
                    dtrGenerico.Close()
                    dtrGenerico = Nothing
                End If
                rstGenerico = ClsServer.EseguiSqlClient(strsql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
                'Includo Sede Attuazione
                strsql = "insert into associaentirelazionisediAttuazioni(idEnteSedeattuazione,identeRelazione,DataCreazionerecord)"
                strsql = strsql & " select  @@identity,identerelazione,getdate() from entirelazioni where identePadre= " & Session("IdEnte") & "  and identeFiglio=" & ddlEntiFigli.SelectedValue & ""
                rstGenerico = ClsServer.EseguiSqlClient(strsql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
                '****************
            End If
            'richiamo la Pagina di Gestione della Sede inviando valori di Selezione
            Context.Items.Add("identesede", strIdEnteSede)
            Context.Items.Add("idente", ddlEntiFigli.SelectedValue)
            Context.Items.Add("Ente", ddlEntiFigli.SelectedItem.Text)
            'strsql = "select @@identity as idmaxx from AssociaEntiRelazioniSedi"
            strsql = "Select idassociaentirelazioniSedi from AssociaEntiRelazioniSedi where identesede=" & strIdEnteSede & " "
            ChiudiDataReader(dtrGenerico)
            'dtrGenerico = ClsServer.CreaDatareader(strsql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
            'If dtrGenerico.HasRows Then
            '    dtrGenerico.Read()
            '    Context.Items.Add("acquisita", dtrGenerico("idassociaentirelazioniSedi"))
            'Else
            '    Context.Items.Add("acquisita", "no")
            'End If
            'If Not dtrGenerico Is Nothing Then
            '    dtrGenerico.Close()
            '    dtrGenerico = Nothing
            'End If
            Context.Items.Add("tipoazione", "Modifica")
            strsql = "Select * from statientisedi where defaultstato=1"
            ChiudiDataReader(dtrGenerico)
            dtrGenerico = ClsServer.CreaDatareader(strsql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
            If dtrGenerico.HasRows Then
                dtrGenerico.Read()
            End If
            ChiudiDataReader(dtrGenerico)
            Context.Items.Add("identefiglio", ddlEntiFigli.SelectedValue)
            txtidsede.Value = strIdEnteSede
            SettaValori(strIdEnteSede)

            'Server.Transfer("WfrmAnagraficaSedi.aspx")
        Else
            '***************Aggiunto da Alessandra Taballione il 29/07/2005
            'Aggiunta di sedeAttuazione
            If ddlTipologia.SelectedItem.Text = "Operativa" Then
                ChiudiDataReader(dtrGenerico)

                strsql = "SELECT identepadre FROM entirelazioni WHERE DataFinevalidità is null AND identefiglio='" & Session("IdEnte") & "'"

                dtrGenerico = ClsServer.CreaDatareader(strsql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
                If dtrGenerico.HasRows = True Then
                    dtrGenerico.Read()
                    strIdEnteCapoFila = dtrGenerico("identepadre")
                Else
                    strIdEnteCapoFila = Session("IdEnte")
                End If
                ChiudiDataReader(dtrGenerico)

                '***************Aggiunto da Alessandra Taballione il 29/07/2005
                'Aggiunta in sedeAttuazione
                'modificata da Simona Cordella il 11/12/2008
                'aggiungo inserimento del campo CERTIFICAZIONE =2 (DA VALUTARE)
                strsql = "insert into entiSediattuazioni (Denominazione,identeSede,UsernameStato,idStatoEnteSede,UsernameInseritore,DataInserimento,IdEnteCapoFila,Certificazione,DataCertificazione ,UserCertificazione,NMaxVolontari,FlagMaxVolontari, UsernameMaxVolontari,Note) " &
                            "select '" & Replace(txtdenominazione.Text, "'", "''") & "'," & strIdEnteSede & ","
                strsql = strsql & "'" & Replace(Session("Utente"), "'", "''") & "', entisedi.idstatoEntesede, '" & Session("Utente") & "', getDate(), '" & strIdEnteCapoFila & "',"

                'gestione certificazione per regioni e unsc
                If (Session("TipoUtente") = "U" Or Session("TipoUtente") = "R") Then
                    If ddlCertificazione.SelectedValue = 2 Then
                        strsql = strsql & " " & ddlCertificazione.SelectedValue & ", null , null, "
                    Else
                        strsql = strsql & " " & ddlCertificazione.SelectedValue & ",getdate(),'" & Session("Utente") & "',"
                    End If
                Else
                    strsql = strsql & " 2, null , null, "
                End If
                strsql = strsql & " " & TxtNumMaxVol.Text & ", "

                If bytFlagMaxVolontari = 0 Then
                    strsql = strsql & " " & bytFlagMaxVolontari & ", null "
                Else
                    strsql = strsql & " " & bytFlagMaxVolontari & ", '" & Replace(Session("Utente"), "'", "''") & "' "
                End If
                strsql = strsql & " ,'" & txtNote.Text.Replace("'", "''") & "'"
                strsql = strsql & "from StatiEntiSedi " &
                " inner join entisedi  on StatiEntiSedi.idstatoentesede=entisedi.idstatoentesede where identesede=" & strIdEnteSede & ""
                ChiudiDataReader(dtrGenerico)
                rstGenerico = ClsServer.EseguiSqlClient(strsql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
            End If
            'richiamo la Pagina di Gestione della Sede inviando valori di Selezione
            'Context.Items.Add("identesede", strIdEnteSede)
            'Context.Items.Add("idente", Session("IdEnte"))
            'Context.Items.Add("Ente", Session("Denominazione"))
            'strsql = "select @@identity as idmaxx from AssociaEntiRelazioniSedi"
            strsql = "Select idassociaentirelazioniSedi from AssociaEntiRelazioniSedi where identesede=" & strIdEnteSede & " "
            ChiudiDataReader(dtrGenerico)
            dtrGenerico = ClsServer.CreaDatareader(strsql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
            'If dtrGenerico.HasRows Then
            '    dtrGenerico.Read()
            '    Context.Items.Add("acquisita", dtrGenerico("idassociaentirelazioniSedi"))
            'Else
            '    Context.Items.Add("acquisita", "propria")
            'End If
            ChiudiDataReader(dtrGenerico)
            'Context.Items.Add("tipoazione", "Modifica")
            strsql = "Select * from statientisedi where DefaultStato=1"
            ChiudiDataReader(dtrGenerico)
            dtrGenerico = ClsServer.CreaDatareader(strsql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
            'If dtrGenerico.HasRows Then
            '    dtrGenerico.Read()
            '    Context.Items.Add("stato", dtrGenerico("statoentesede"))
            'End If
            ChiudiDataReader(dtrGenerico)
            SettaValori(strIdEnteSede)
            txtidsede.Value = strIdEnteSede

        End If

        RicercaSediAttuazione(strIdEnteSede)
    End Sub
    Private Sub Modifica()
        'Generato da Alessandra Taballione il 10/03/04
        'Effettuo Modifiche della sede
        Dim dtrTrovaProviciaDB As SqlClient.SqlDataReader
        Dim dtrTrovaProvinciaClient As SqlClient.SqlDataReader
        Dim IntProvinciaDB As Integer
        Dim IntComuneDB As Integer
        Dim ProvinciaDB As String
        Dim ComuneDB As String
        Dim blnProvincia As Boolean
        Dim IntComuneClient As Integer
        Dim IntProvinciaClient As Integer
        Dim ProvinciaClient As String
        Dim ComuneClient As String
        blnProvincia = False

        ChiudiDataReader(dtrGenerico)
        dtrGenerico = ClsServer.CreaDatareader("Select identeSede from entisedi where denominazione='" & Replace(txtdenominazione.Text, "'", "''") & "' and idEnte=" & lblidEnte.Value & " and identeSede <>" & txtidsede.Value & "", IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
        If dtrGenerico.HasRows = True Then
            cmdSalva.Visible = True
            msgErrore.Text = "La denominazione della Sede è già presente in archivio!"
            dtrGenerico.Close()
            dtrGenerico = Nothing
            Exit Sub
        End If
        ChiudiDataReader(dtrGenerico)

        'strsql = "Select idcomune from comuni where denominazione ='" & Replace(ddlComune.Text, "'", "''") & "'"
        '--------------------------adc
        'If Request.Form("txtIDComune") = "" Then
        '    strsql = "Select idcomune from comuni where idComune ='" & ddlComune.SelectedItem.Value & "'"
        'Else
        '    strsql = "Select idcomune from comuni where idComune ='" & Request.Form("txtIDComune") & "'"
        'End If
        'dtrGenerico = ClsServer.CreaDatareader(strsql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
        'dtrGenerico.Read()
        'ChiudiDataReader(dtrGenerico)
        '---------------------------------adc

        If (Session("TipoUtente") = "U" Or Session("TipoUtente") = "R") Then
            If ControlloDoppioneSedeNuovaGlobale(txtidsede.Value, txtIndirizzo.Text, txtCivico.Text, ddlComune.SelectedItem.Value, Trim(TxtPalazzina.Text), Trim(TxtScala.Text), Trim(TxtPiano.Text), Trim(TxtInterno.Text)) = True Then
                cmdSalva.Visible = True
                'Inserisco il Messaggio di Errore
                msgErrore.Text = "L'indirizzo risulta gia' utilizzato dall'Ente in lavorazione o da altro Ente. Impossibile effettuare il salvataggio."
                Exit Sub
            End If
        End If


        Dim strMiaCausale As String = ""
        If ddlComune.SelectedValue <> "0" Then
            If Not indirizzoOkGoogle And Not Session("ProcediGM") = 1 And Not procediGM Then
                If ClsUtility.CAP_VERIFICA(IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")),
                    strMiaCausale, bandiera, Trim(txtCap.Text), ddlComune.SelectedItem.Value, "", "", txtIndirizzo.Text, txtCivico.Text) = False Then
                    'Ripristino lo stato del tasto
                    cmdSalva.Visible = True
                    msgErrore.Text = strMiaCausale
                    ChiudiDataReader(dtrGenerico)
                    Exit Sub
                End If
            End If
        Else
            If ChkEstero.Checked And ddlComune.SelectedValue <= 0 Then
                ddlComune.ClearSelection()
                ddlComune.Items.FindByText(ddlProvincia.SelectedItem.Text).Selected = True
            End If
        End If

        ModificaSedeEnte()
        If Session("TipoUtente") = "U" Then
            If ClsUtility.ControlloRichiestaModificaSede(Request.QueryString("identesede"), Session("conn")) = True Then
                AggiornaRichiestaModifica(0, txtidsede.Value)
                imgAnnullaRichiestaModifica.Visible = False
                imgRichiestaModifica.Visible = True
            End If

        End If

        LoadMaschera(txtidsede.Value)

    End Sub
    'Ritorna FALSE se ci sono attività sulla sede che non permettono la cancellazione
    Private Function ControllaAttivitaSede() As Boolean
        Dim NAttivi As Integer
        Dim NTerminati As Integer
        Dim NProposto As Integer
        Dim NRegistrati As Integer
        Dim NNonAttivabili As Integer
        Dim NRespinti As Integer
        Dim NInAttesaGraduatoria As Integer
        Dim NRitirati As Integer
        Dim NArchiviati As Integer

        'Verifica dei Progetti in corso Attivi,Proposti,daVAlutare o da graduare
        'Mod. il 31/03/2008 da simona cordella
        '
        strsql = "SELECT IDStatoAttività, StatoAttività," &
                    "(SELECT  COUNT(DISTINCT attività.IDAttività)" &
                    " FROM attività INNER JOIN " &
                    " attivitàentisediattuazione ON attività.IDAttività = attivitàentisediattuazione.IDAttività INNER JOIN " &
                    " entisediattuazioni ON attivitàentisediattuazione.IDEnteSedeAttuazione = entisediattuazioni.IDEnteSedeAttuazione " &
                    " WHERE (entisediattuazioni.IDEnteSede =" & txtCodice.Value & ") AND attività.idstatoattività = statiattività.idstatoattività AND " &
                    " attivitàentisediattuazione.numeropostivitto+attivitàentisediattuazione.numeropostivittoalloggio + attivitàentisediattuazione.numeropostinovittonoalloggio<> 0  " &
                    " and (ATTIVITà.idstatoattività in (4,9) or (ATTIVITà.idstatoattività = 1 and (getdate() between datainizioattività and datafineattività or datainizioattività is null)))) AS nprog " &
                    " FROM statiattività order by IdStatoAttività"
        '" AND datafineattività > getdate()) AS nprog " & _
        ChiudiDataReader(dtrGenerico)
        dtrGenerico = ClsServer.CreaDatareader(strsql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
        If dtrGenerico.HasRows = True Then

            While dtrGenerico.Read()
                If dtrGenerico("Idstatoattività") = 1 Then
                    NAttivi = dtrGenerico("NProg")
                End If
                If dtrGenerico("Idstatoattività") = 2 Then
                    NTerminati = dtrGenerico("NProg")
                End If
                If dtrGenerico("Idstatoattività") = 4 Then
                    NProposto = dtrGenerico("NProg")
                End If
                If dtrGenerico("Idstatoattività") = 5 Then
                    NRegistrati = dtrGenerico("NProg")
                End If
                If dtrGenerico("Idstatoattività") = 6 Then
                    NNonAttivabili = dtrGenerico("NProg")
                End If
                If dtrGenerico("Idstatoattività") = 7 Then
                    NRespinti = dtrGenerico("NProg")
                End If
                If dtrGenerico("Idstatoattività") = 9 Then
                    NInAttesaGraduatoria = dtrGenerico("NProg")
                End If
                If dtrGenerico("Idstatoattività") = 10 Then
                    NRitirati = dtrGenerico("NProg")
                End If
                If dtrGenerico("Idstatoattività") = 11 Then
                    NArchiviati = dtrGenerico("NProg")
                End If

            End While

            If Not dtrGenerico Is Nothing Then
                dtrGenerico.Close()
                dtrGenerico = Nothing
            End If

            If NAttivi + NInAttesaGraduatoria + NProposto + NRegistrati > 0 Then
                msgErrore.Text = "Impossibile eliminare la Sede perchè è associata a Progetti Attivi o in fase di valutazione."
                Return False
            End If

        End If
        Return True
    End Function
    Private Sub EliminaSede()
        AccreditamentoEliminaSede(txtidsede.Value)
        SettaValori(txtidsede.Value)
        LoadMaschera(txtidsede.Value)
    End Sub
    Private Sub RipristinaSede()

        'Ripristino della sede
        CronologiaStatiEntiSedi()
        strsql = " update entisedi set idstatoenteSede=4 " &
                            " where idEnteSede=" & txtidsede.Value & " and idente=" & lblidEnte.Value & ""
        If Not dtrGenerico Is Nothing Then
            dtrGenerico.Close()
            dtrGenerico = Nothing
        End If
        rstGenerico = ClsServer.EseguiSqlClient(strsql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
        If Not dtrGenerico Is Nothing Then
            dtrGenerico.Close()
            dtrGenerico = Nothing
        End If
        CronologiaStatiEntisediAttuazioni()
        strsql = " update entisediattuazioni set idstatoenteSede=4 " &
                " where idEnteSede=" & txtidsede.Value & ""
        If Not dtrGenerico Is Nothing Then
            dtrGenerico.Close()
            dtrGenerico = Nothing
        End If
        rstGenerico = ClsServer.EseguiSqlClient(strsql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
        If lblPersonalizza.Value = "Inserimento" Then
            Response.Redirect("WfrmMain.aspx")
        Else
            Response.Redirect("WfrmRicercaSede.aspx.aspx")
        End If
    End Sub
    Private Sub AnnullaInclusione()
        'Realizzato da Alessandra Taballione il 06/03/2004
        'Annulla inclusione della sede
        'Verifica se esistono sedi attuazione incluse
        strsql = "delete associaentirelazionisedi where idEnteSede=" & txtidsede.Value & ""
        If Not dtrGenerico Is Nothing Then
            dtrGenerico.Close()
            dtrGenerico = Nothing
        End If
        rstGenerico = ClsServer.EseguiSqlClient(strsql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
        lblsedeInclusa.Text = "Sede non Inclusa"
        lblacquisita.Value = "no"
        PersonalizzaTasti()
    End Sub
    Private Sub AnnullaInclusioneSedeAttuazione()
        'Generato da Alessandra Taballione il 07/04/04
        'Annullamento dell'inclusione della Sede di Attuazione

        'Agg e Mod. da s.c. il 05/03/2008 per nuovo accreditamento
        'strsql = "delete associaentirelazionisediAttuazioni where idAssociaEntiRelazioniSediAttuazioni=" & txtSedeFisicaInclusa.Text & " "

        strsql = " Delete  from AssociaEntiRelazioniSediAttuazioni " &
                 " Where identesedeattuazione in " &
                 " (Select identesedeattuazione from entisediattuazioni " &
                 "  where idEnteSede=" & txtidsede.Value & ")"

        If Not dtrGenerico Is Nothing Then
            dtrGenerico.Close()
            dtrGenerico = Nothing
        End If
        rstGenerico = ClsServer.EseguiSqlClient(strsql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
        lblsedeInclusa.Visible = True
        lblsedeInclusa.Text = "Sede non Inclusa"

    End Sub
    Private Sub AccreditaSede()
        'Accreditamento della Sede 06/03/2004
        'Cambio dello Stato della Sede e delle Rispettive Sedi di Attuazione
        'da Presentato a Accettato
        CronologiaStatiEntiSedi()
        If ddlTipologia.SelectedItem.Text = "Operativa" Then
            CronologiaStatiEntisediAttuazioni()
        End If
        strsql = " update entisedi set idstatoenteSede =(select idstatoEnteSede from statiEntiSedi where attiva=1) , UsernameStato='" & Session("Utente") & "',DataUltimoStato= GETDATE() " &
                            " where idEnteSede=" & txtidsede.Value & ""
        If Not dtrGenerico Is Nothing Then
            dtrGenerico.Close()
            dtrGenerico = Nothing
        End If
        rstGenerico = ClsServer.EseguiSqlClient(strsql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
        If ddlTipologia.SelectedItem.Text = "Operativa" Then
            'Modificato da Simona Cordella il 12/12/2008
            'Gestione del campo Certificazione = 1(si) 
            strsql = " update entisediAttuazioni set " &
                     " idstatoenteSede =(select idstatoEnteSede from statiEntiSedi where attiva=1), " &
                     " Certificazione = 1 ," &
                     " UserCertificazione ='" & Session("Utente") & "', " &
                     " DataCertificazione =getdate(), " &
                     " UsernameStato='" & Session("Utente") & "', DataUltimoStato= GETDATE() " &
                     " Where idEnteSede=" & txtidsede.Value & " and idstatoEntesede=(Select idstatoentesede from statientisedi where DaAccreditare=1 and defaultstato=1)"
            If Not dtrGenerico Is Nothing Then
                dtrGenerico.Close()
                dtrGenerico = Nothing
            End If
            rstGenerico = ClsServer.EseguiSqlClient(strsql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
        End If
        If lblPersonalizza.Value = "Inserimento" Then
            Response.Redirect("WfrmMain.aspx")
        Else
            If lblsql.Value <> "" Then
                Context.Items.Add("page", lblpage.Value)
                Context.Items.Add("strsql", lblsql.Value)
            End If
            Response.Redirect("WfrmRicercaSede.aspx")
        End If
        'PersonalizzaTasti()
    End Sub
    Private Sub IncludiSede()
        Dim datat As DataTable
        Dim MyRow As DataRow
        'Realizzato da Alessandra Taballione il 10/03/2004
        'inclusione della sede
        strsql = "insert into associaentirelazionisedi(idEnteSede,identeRelazione,DataCreazionerecord) "
        'se non è selezionato un ente figlio inserisco con l'id selezionato in fase di modifica
        If ControllaEnteFiglio(ddlEntiFigli.SelectedValue) = False Then
            strsql = strsql & "select " & txtidsede.Value & " ,identerelazione,getdate() from entirelazioni where identePadre= " & Session("IdEnte") & "  and identeFiglio=" & lblidEnte.Value & ""
        Else 'altrimenti
            strsql = strsql & "select " & strIdEnteSede & " ,identerelazione,getdate() from entirelazioni where identePadre= " & Session("IdEnte") & "  and identeFiglio=" & ddlEntiFigli.SelectedValue & ""
        End If
        If Not dtrGenerico Is Nothing Then
            dtrGenerico.Close()
            dtrGenerico = Nothing
        End If

        rstGenerico = ClsServer.EseguiSqlClient(strsql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
        If ControllaEnteFiglio(ddlEntiFigli.SelectedValue) = True Then
            Exit Sub
        End If
        strsql = "select idAssociaEntiRelazioniSedi as idass from associaentirelazionisedi "
        strsql = strsql & "inner join entirelazioni on (entirelazioni.identerelazione=associaentirelazionisedi.identerelazione) "
        strsql = strsql & "where associaentirelazionisedi.identesede=" & txtidsede.Value & " and "
        strsql = strsql & "entirelazioni.identepadre = " & Session("IdEnte") & " And entirelazioni.identeFiglio = " & lblidEnte.Value & ""
        If Not dtrGenerico Is Nothing Then
            dtrGenerico.Close()
            dtrGenerico = Nothing
        End If
        dtrGenerico = ClsServer.CreaDatareader(strsql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
        dtrGenerico.Read()
        lblacquisita.Value = dtrGenerico("idass")

        strsql = "Select entisediattuazioni.identesedeattuazione "
        strsql = strsql & " from entisediattuazioni left join associaentirelazionisediattuazioni on entisediattuazioni.identesedeattuazione = associaentirelazionisediattuazioni.identesedeattuazione "
        strsql = strsql & " inner join statientisedi on (entisediattuazioni.idstatoentesede=statientisedi.idstatoentesede)"
        strsql = strsql & " where identesede=" & txtidsede.Value & " And (statientisedi.attiva = 1 Or statientisedi.Defaultstato = 1)"
        strsql = strsql & " and associaentirelazionisediattuazioni.idassociaentirelazionisediattuazioni is null "

        'strsql = "Select * from entisediattuazioni where identesede=" & txtidsede.Value & ""
        If Not dtrGenerico Is Nothing Then
            dtrGenerico.Close()
            dtrGenerico = Nothing
        End If
        datat = ClsServer.CreaDataTable(strsql, False, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
        For Each MyRow In datat.Rows
            strsql = "insert into associaentirelazionisediAttuazioni(idEnteSedeattuazione,identeRelazione,DataCreazionerecord)"
            strsql = strsql & " select  " & MyRow.Item("identesedeattuazione") & " ,identerelazione,getdate() from entirelazioni where identePadre= " & Session("IdEnte") & "  and identeFiglio=" & lblidEnte.Value & ""
            rstGenerico = ClsServer.EseguiSqlClient(strsql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
        Next
        If Not dtrGenerico Is Nothing Then
            dtrGenerico.Close()
            dtrGenerico = Nothing
        End If
        '++************************++************************++'
        RicercaSediAttuazione(txtidsede.Value)
        ControllaSedeFisicaInclusa()
        PersonalizzaTasti()
        If Not dtrGenerico Is Nothing Then
            dtrGenerico.Close()
            dtrGenerico = Nothing
        End If

    End Sub

    Sub CaricaEntiFigli()
        'codice generato da nahtanoj il 24?03?2005
        Dim strsql As String
        strsql = "select distinct entirelazioni.IdEnteFiglio, enti.Denominazione from entirelazioni "
        strsql = strsql & "inner join enti on enti.IdEnte=entirelazioni.IdEnteFiglio "
        strsql = strsql & "where entirelazioni.IdEntePadre=" & CInt(Session("IdEnte"))
        strsql = strsql & " and  entirelazioni.datafinevalidità is null and enti.idstatoente in (3)  order by enti.Denominazione"
        ddlEntiFigli.Items.Clear()

        ddlEntiFigli.DataSource = MakeParentTable(strsql)
        ddlEntiFigli.DataTextField = "ParentItem"
        ddlEntiFigli.DataValueField = "id"
        ddlEntiFigli.DataBind()
        If ddlEntiFigli.Items.Count = 1 Then
            ddlEntiFigli.Visible = False
            lblEntefiglio.Visible = False
            imgOpenFigli.Visible = False
        Else

        End If
    End Sub
#End Region

#Region "Eventi"
    Private Sub cmdSalva_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles cmdSalva.Click
        SalvaDati()
        'Dim AlboEnte, ListaAnomalie As String
        'AlboEnte = ClsUtility.TrovaAlboEnte(Session("IdEnte"), Session("Conn"))

        'CancellaMessaggi()
        'If (VerificaValiditaCampi(AlboEnte)) Then
        '    '**** Controllo la plausibilità di nome e indirizzo e in caso mostro il popup con le segnalazioni non bloccanti
        '    If Session("Procedi") = 0 Then
        '        Dim esito As Integer
        '        esito = ControllaNomeIndirizzoSede(ListaAnomalie)

        '        Select Case esito
        '            Case 1
        '                AnomaliaNome = True
        '            Case 2
        '                AnomaliaIndirizzo = True
        '            Case 3
        '                AnomaliaNome = True
        '                AnomaliaIndirizzo = True
        '            Case 4
        '                'inserire cosa fare se va in errore il controllo nome indirizzo
        '            Case Else

        '        End Select
        '        If esito > 0 And esito < 4 Then 'se non risultano anomalie o errore generico
        '            ShowPopUPControllo = "1"
        '            ListaAnomalie = Replace(ListaAnomalie, "|", "<br/>")
        '            lblErroreControlloSede.Text = ListaAnomalie
        '            Exit Sub
        '        End If
        '    Else
        '        ShowPopUPControllo = ""
        '        Session("Procedi") = 0
        '        lblErroreControlloSede.Text = ""
        '    End If



        '    Dim strtab As Integer
        '    strtab = cmdSalva.TabIndex

        '    If lblPersonalizza.Value = "Inserimento" Then
        '        cmdSalva.Visible = False
        '        Session("IdComune") = Nothing
        '        Inserimento()
        '        'dgRisultatoRicerca.Visible = True

        '    Else 'Fase di Modifica della sede 
        '        If txtemail.ReadOnly = False Then
        '            Modifica()
        '            PersonalizzaTasti()
        '            RicercaSediAttuazione(txtidsede.Value)
        '            If (Session("TipoUtente") = "U" Or Session("TipoUtente") = "R") Then
        '                CaricaDatiEntiSediAttuazione(txtidsede.Value)
        '            End If

        '        End If
        '        SettaValori(txtidsede.Value)
        '        EvidenziaDatiModificati(txtidsede.Value)
        '    End If

        'End If
    End Sub
    Private Sub cmdEmail_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        If txtemail.Text <> "" Then
            'lbldataControlloEmail.Value = Date.Today
            chkFalsoEmail.Visible = True
            chkVeroEmail.Visible = True
            'ClsUtility.invioEmail("gabbianella2000@virgilio.it", txtemail.Text, "", "Prova Conferma email", "Ciaoooooooo!!!")
        End If
    End Sub
    Private Sub cmdChiudi_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles cmdChiudi.Click
        'controllo se vengo dall'albero in gestione enti
        If Not Request.QueryString("VengoDa") Is Nothing Then
            'controllo se la variabile è valorizzata
            If Request.QueryString("VengoDa") <> "" Then
                'faccio la response.redirect verso l'albero
                Response.Redirect(Request.QueryString("VengoDa").ToString)
            End If
        End If
        If lblPersonalizza.Value = "Inserimento" Then
            Response.Redirect("WfrmMain.aspx")
        Else
            If lblsql.Value <> "" Then
                Context.Items.Add("page", lblpage.Value)
                Context.Items.Add("strsql", lblsql.Value)
                Server.Transfer("WfrmRicercaSede.aspx?VengoDaProgetti=" & Request.QueryString("VengoDaProgetti"))
                'Response.Redirect("WfrmRicercaSede.aspx")
            Else
                Response.Redirect("WfrmRicercaSede.aspx?VediEnte=1")
            End If
        End If
        Session("IdComune") = Nothing
    End Sub
    Private Sub cmdIncludi_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles cmdIncludi.Click
        IncludiSede()
    End Sub
    Private Sub cmdAnnulla_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles cmdAnnulla.Click
        If txtTipologia.Value = 4 Then 'Sede Operativa
            If dgRisultatoRicerca.Items.Count > 0 Then
                If ControllaSedeInclusa() = True Then
                    msgErrore.Text = "Se sono presenti Sedi di Attuazione Acquisite non è possibile annullare L'inclusione della sede."
                    Exit Sub
                Else
                    'Annullo Inclusione
                    If ControllaAttivitaSede() <> "" Then
                        msgErrore.Text = "Impossibile annullare l'inclusione perchè la sede è legata a Progetti non ancora Chiusi."
                        Exit Sub
                    Else
                        'Annullo Inclusione
                        AnnullaInclusione()
                    End If
                End If
            Else
                'Annullo Inclusione
                If ControllaAttivitaSede() <> "" Then
                    msgErrore.Text = "Impossibile annullare l'inclusione perchè la sede è legata a Progetti non ancora Chiusi."
                    Exit Sub
                Else
                    'Annullo Inclusione
                    AnnullaInclusione()
                End If
            End If
        Else
            'Annullo Inclusione
            If ControllaAttivitaSede() <> "" Then
                msgErrore.Text = "Impossibile annullare l'inclusione perchè la sede è legata a Progetti non ancora Chiusi."
                Exit Sub
            Else
                'Annullo Inclusione
                AnnullaInclusione()
            End If
        End If
    End Sub

    Private Sub ddlTipologia_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlTipologia.SelectedIndexChanged
        CancellaMessaggi()
        If dgRisultatoRicerca.Items.Count > 0 Then
            If ControllaSedeAttiva() = True Then
                If txtTipologia.Value = "4" And ddlTipologia.SelectedValue <> "4" Then
                    ddlTipologia.SelectedIndex = trovaindex("Operativa", ddlTipologia)
                    msgErrore.Text = "Se sono presenti Sedi di Attuazione non è possibile cambiare la tipologia di sede."
                End If
            End If
        Else
        End If

    End Sub
    Private Sub cmdRipristina_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles cmdRipristina.Click
        CancellaMessaggi()
        If checkStatoEnteFiglio(txtidsede.Value) = True Then
            RipristinaSede()
        Else
            msgErrore.Text = "Impossibile ripristinare la Sede perchè l'ente di appartenenza risulta chiuso."
        End If
    End Sub
    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        If Not dtrGenerico Is Nothing Then
            dtrGenerico.Close()
            dtrGenerico = Nothing
        End If
    End Sub
    Private Sub cmdAccredita_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles cmdAccredita.Click
        'Generato da Alessandra Taballione il 07/03/2004
        'Richiamo la sub che effettua l'accreditamento della sede
        AccreditaSede()
    End Sub
    Private Sub cmdAnnullaAccred_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles cmdAnnullaAccred.Click
        CancellaMessaggi()
        If (ControllaAttivitaSede()) Then
            AnnullaAccreditamento()
        End If


    End Sub
    Private Sub txthttp_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txthttp.TextChanged
        If txthttp.Text <> "" Then
            cmdHttp.Enabled = True
            cmdHttp.NavigateUrl = lblhttp.Text & txthttp.Text
        Else
            cmdHttp.Enabled = False
            cmdHttp.NavigateUrl = ""

        End If
    End Sub
    Private Sub txtemail_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        If txtemail.Text <> "" Then
            cmdEmail.Enabled = True
        Else
            cmdEmail.Enabled = False
        End If
    End Sub
#End Region

#Region "Fuzionalita"
    Function dataserver(ByVal data As String) As String
        strsql = "select case len(day(getdate())) when 1 then '0'+ " &
        " Convert(varchar(20), Day(getdate()))" &
        " else convert(varchar(20),day(getdate()))  end + '/'+ " &
        " case len(month(getdate())) when 1 then '0'+ convert(varchar(20),month(getdate())) " &
        " else convert(varchar(20),month(getdate())) end + '/'+ convert(varchar(20),year(getdate()))" &
        " as dataserver"
        If Not dtrGenerico Is Nothing Then
            dtrGenerico.Close()
            dtrGenerico = Nothing
        End If
        'popolamento della maschera dati DB
        dtrGenerico = ClsServer.CreaDatareader(strsql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
        dtrGenerico.Read()
        dataserver = dtrGenerico("dataserver")
        If Not dtrGenerico Is Nothing Then
            dtrGenerico.Close()
            dtrGenerico = Nothing
        End If
    End Function
    'funzione che mi verifica se è selezionato un ente figlio per cui 
    'l'inserimento della sede ricadrà sull'ente selezionato o l'ente padre
    Private Function ControllaEnteFiglio(ByVal intSelezione As Integer) As Boolean
        'controllo l'id selezionato nella ddl degli enti figli
        'se l'id è 0 allora la funzione mi restituirà false
        'altrimenti mi restituirà true il che vuol dire che andrò a fare gli inserimenti 
        'sull'ente figlio selezionato
        If intSelezione = 0 Then
            ControllaEnteFiglio = False
        Else
            ControllaEnteFiglio = True
        End If
        Return ControllaEnteFiglio
    End Function

    Private Function MakeParentTable(ByVal strquery As String) As DataSet
        '***Generata da Gianluigi Paesani in data:05/07/04
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

        If Not dtrGenerico Is Nothing Then
            dtrGenerico.Close()
            dtrGenerico = Nothing
        End If
        dtrGenerico = ClsServer.CreaDatareader(strquery, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))

        myDataRow = myDataTable.NewRow()
        myDataRow("id") = 0
        myDataRow("ParentItem") = "Sede Propria"
        myDataTable.Rows.Add(myDataRow)
        Do While dtrGenerico.Read
            myDataRow = myDataTable.NewRow()
            myDataRow("id") = dtrGenerico.GetValue(0)
            myDataRow("ParentItem") = dtrGenerico.GetValue(1)
            myDataTable.Rows.Add(myDataRow)
        Loop

        dtrGenerico.Close()
        dtrGenerico = Nothing

        MakeParentTable = New DataSet
        MakeParentTable.Tables.Add(myDataTable)
    End Function

    Private Sub popolamaschera_(ByVal idente As Double, ByVal Ente As String)
        lblEnte.Text = Ente
        lblidEnte.Value = idente
        If lblacquisita.Value <> "propria" Then
            AbilitaCampiMaschera(False)
        Else
            strsql = "Select * from statientisedi where statoentesede='" & lblStato.Text & "'"
            If Not dtrGenerico Is Nothing Then
                dtrGenerico.Close()
                dtrGenerico = Nothing
            End If
            dtrGenerico = ClsServer.CreaDatareader(strsql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
            dtrGenerico.Read()
            'If lblStato.Text = "Chiusa" Or lblStato.Text = "Sospesa" Then
            If dtrGenerico("Attiva") = 0 And dtrGenerico("DaAccreditare") = 0 Then
                AbilitaCampiMaschera(False)
            End If
        End If
        If Not dtrGenerico Is Nothing Then
            dtrGenerico.Close()
            dtrGenerico = Nothing
        End If
        'popolamento della maschera dati DB
        dtrGenerico = ClsServer.CreaDatareader("select day(entisedi.datacontrolloemail)as ggDCEmail,month(entisedi.datacontrolloemail)as " &
                    " monthDCEmail,year(entisedi.datacontrolloemail)as yearDCEmail," &
                    " day(entisedi.datacontrollohttp)as ggDChttp,month(entisedi.datacontrollohttp)as monthDChttp," &
                    " year(entisedi.datacontrollohttp)as yearDChttp," &
                    " entisediTipi.idtiposede,comuni.idComune,comuni.denominazione as Comune,provincie.provincia,* from entisedi " &
                    " inner join comuni on (comuni.idcomune=entisedi.idcomune) " &
                    " inner join provincie on (provincie.idprovincia=comuni.idprovincia)" &
                    " left join entisediTipi on (entisediTipi.identesede=entisedi.idEntesede)" &
                    " where entisedi.identeSede=" & Context.Items("identesede") & "", IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
        dtrGenerico.Read()
        If dtrGenerico.HasRows = True Then
            If Not IsDBNull(dtrGenerico("denominazione")) Then
                txtdenominazione.Text = dtrGenerico("denominazione")
                txtidsede.Value = dtrGenerico("idEnteSede")
            End If
            If Not IsDBNull(dtrGenerico("codicesedeEnte")) Then
                txtCodice.Value = dtrGenerico("codicesedeente")
            End If
            If Not IsDBNull(dtrGenerico("indirizzo")) Then
                txtIndirizzo.Text = dtrGenerico("indirizzo")
            End If
            If Not IsDBNull(dtrGenerico("DettaglioRecapito")) Then
                TxtDettaglioRecapito.Text = dtrGenerico("DettaglioRecapito")
            End If
            '**** agg. da simona cordella il 19/05/2009 per nuovo accreditamento luglio 2009
            'palazzina
            If Not IsDBNull(dtrGenerico("Palazzina")) Then
                TxtPalazzina.Text = dtrGenerico("Palazzina")
            End If
            'Scala
            If Not IsDBNull(dtrGenerico("Scala")) Then
                TxtScala.Text = dtrGenerico("Scala")
            End If
            'Piano
            If Not IsDBNull(dtrGenerico("Piano")) Then
                TxtPiano.Text = dtrGenerico("Piano")
            End If
            ' Interno
            If Not IsDBNull(dtrGenerico("Interno")) Then
                TxtInterno.Text = dtrGenerico("Interno")
            End If
            'IdTitoloGiuridico
            If Not IsDBNull(dtrGenerico("IdTitoloGiuridico")) Then
                ddlTitoloGiuridico.SelectedValue = dtrGenerico("IdTitoloGiuridico")
            End If
            '*******************
            If Not IsDBNull(dtrGenerico("Civico")) Then
                txtCivico.Text = dtrGenerico("Civico")
            End If
            If Not IsDBNull(dtrGenerico("Cap")) Then
                txtCap.Text = dtrGenerico("Cap")
            End If
            If Not IsDBNull(dtrGenerico("Comune")) Then
                ddlComune.SelectedValue = dtrGenerico("Comune")
                Session("IdComune") = dtrGenerico("idcomune")
            End If
            If Not IsDBNull(dtrGenerico("Provincia")) Then
                ddlProvincia.SelectedValue = dtrGenerico("Provincia")
            End If
            If Not IsDBNull(dtrGenerico("idtiposede")) Then
                ddlTipologia.SelectedValue = dtrGenerico("idtiposede")
                txtTipologia.Value = dtrGenerico("idtiposede")
            End If
            If Not IsDBNull(dtrGenerico("Telefono")) Then
                txtTelefono.Text = dtrGenerico("Telefono")
            End If
            If Not IsDBNull(dtrGenerico("prefissoTelefono")) Then
                txtprefisso.Text = dtrGenerico("prefissoTelefono")
            End If
            If Not IsDBNull(dtrGenerico("Fax")) Then
                txtfax.Text = dtrGenerico("Fax")
            End If
            If Not IsDBNull(dtrGenerico("prefissoFax")) Then
                txtPrefFax.Text = dtrGenerico("prefissoFax")
            End If
            'If Not IsDBNull(dtrGenerico("datacontrollohttp")) Then
            '    lblDataControlloHttp.Value = dtrGenerico("datacontrollohttp")
            'End If
            If Not IsDBNull(dtrGenerico("Datacontrolloemail")) Then
                lbldataControlloEmail.Value = dtrGenerico("ggDCemail") & "/" & IIf(CInt(dtrGenerico("monthDCemail")) < 10, "0" & dtrGenerico("monthDCemail"), dtrGenerico("monthDCemail")) & "/" & dtrGenerico("YearDCemail")
            End If
            'txtdataControlloEmail.Text = IIf(Not IsDBNull(dtrgenerico("Datacontrolloemail")), dtrgenerico("Datacontrolloemail"), "")
            If Not IsDBNull(dtrGenerico("Datacontrollohttp")) Then
                lblDataControlloHttp.Value = dtrGenerico("ggDChttp") & "/" & IIf(CInt(dtrGenerico("monthDChttp")) < 10, "0" & dtrGenerico("monthDChttp"), dtrGenerico("monthDChttp")) & "/" & dtrGenerico("yearDChttp")
            End If
            If Not IsDBNull(dtrGenerico("http")) Then
                txthttp.Text = dtrGenerico("http")
                If Trim(lblDataControlloHttp.Value) <> "" Then
                    If dtrGenerico("httpvalido") = True Then
                        chkVerohttp.Checked = True
                    Else
                        chkFalsohttp.Checked = True
                    End If
                Else
                    chkVerohttp.Checked = False
                    chkFalsohttp.Checked = False
                End If
            Else
                chkVerohttp.Checked = False
                chkFalsohttp.Checked = False
            End If

            If Not IsDBNull(dtrGenerico("Email")) Then
                txtemail.Text = dtrGenerico("Email")
                If Trim(lbldataControlloEmail.Value) <> "" Then
                    If dtrGenerico("EmailValido") = True Then
                        chkVeroEmail.Checked = True
                    Else
                        chkFalsoEmail.Checked = True
                    End If
                Else
                    chkVeroEmail.Checked = False
                    chkFalsoEmail.Checked = False
                End If
            Else
                chkVeroEmail.Checked = False
                chkFalsoEmail.Checked = False
            End If
            If Not IsDBNull(dtrGenerico("RiferimentoRimborsi")) Then
                If dtrGenerico("RiferimentoRimborsi") = True Then
                    chkRiferimentoRimborsi.Checked = True
                End If
            End If
            If Not dtrGenerico Is Nothing Then
                dtrGenerico.Close()
                dtrGenerico = Nothing
            End If
            PersonalizzaTasti()
            ' RicercaSediAttuazione()
            ControllaSedeFisicaInclusa()
        End If
    End Sub
    Private Sub PersonalizzaTasti()

        strsql = "Select * from statientisedi where statoentesede='" & Trim(Replace(lblStato.Text, "(*)", "")) & "'"
        If Not dtrGenerico Is Nothing Then
            dtrGenerico.Close()
            dtrGenerico = Nothing
        End If
        dtrGenerico = ClsServer.CreaDatareader(strsql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
        dtrGenerico.Read()
        If dtrGenerico.HasRows Then
            If dtrGenerico("IdStatoEnteSede") = 3 And (Session("TipoUtente") = "U" Or Session("TipoUtente") = "R") Then ' Sospesa
                cmdSalva.Visible = True
                cmdIncludi.Visible = False
                cmdCancella.Visible = False
                cmdRipristina.Visible = False
                cmdAnnulla.Visible = False
                cmdChiudi.Visible = True
            Else
                Select Case lblacquisita.Value
                    Case "propria"
                        If dtrGenerico("Attiva") = False And dtrGenerico("DaAccreditare") = False Then
                            If ddlTipologia.SelectedItem.Text = "Operativa" Then
                                'cmdSalva.Visible = False
                                cmdIncludi.Visible = False
                                cmdCancella.Visible = False
                                cmdRipristina.Visible = False
                                cmdAnnulla.Visible = False
                                cmdChiudi.Visible = True
                            Else
                                'cmdSalva.Visible = False
                                cmdIncludi.Visible = False
                                cmdCancella.Visible = False
                                cmdRipristina.Visible = False
                                cmdAnnulla.Visible = False
                                cmdChiudi.Visible = True
                            End If
                        Else 'se Aperta
                            If dtrGenerico("DaAccreditare") = True And (Session("TipoUtente") = "U" Or Session("TipoUtente") = "R") Then
                                cmdAccredita.Visible = True
                                cmdAnnullaAccred.Visible = False
                            End If
                            If dtrGenerico("Attiva") = True And (Session("TipoUtente") = "U" Or Session("TipoUtente") = "R") Then
                                cmdAccredita.Visible = False
                                cmdAnnullaAccred.Visible = False
                            End If
                            If ddlTipologia.SelectedItem.Text = "Operativa" Then
                                cmdSalva.Visible = True
                                cmdIncludi.Visible = False
                                cmdCancella.Visible = True
                                cmdRipristina.Visible = False
                                cmdAnnulla.Visible = False
                                cmdChiudi.Visible = True
                            Else
                                cmdSalva.Visible = True
                                cmdIncludi.Visible = False
                                ' mod.da s.c. il 18/11/2008 nn è più possibile cancellare sedi principali
                                cmdCancella.Visible = False
                                cmdRipristina.Visible = False
                                cmdAnnulla.Visible = False
                                cmdChiudi.Visible = True
                            End If
                        End If
                    Case "no"
                        lblsedeInclusa.Visible = True
                        lblsedeInclusa.Text = "Sede non Inclusa"

                        If dtrGenerico("Attiva") = False And dtrGenerico("DaAccreditare") = False Then
                            'cmdSalva.Visible = False
                            cmdCancella.Visible = False
                            If ControllaEnteFiglio(lblidEnte.Value) = True Then
                                cmdRipristina.Visible = False
                            Else
                                cmdRipristina.Visible = False
                            End If
                            cmdChiudi.Visible = True
                            cmdIncludi.Visible = False
                            cmdAnnulla.Visible = False
                        Else
                            If dtrGenerico("DaAccreditare") = True And (Session("TipoUtente") = "U" Or Session("TipoUtente") = "R") Then
                                cmdAccredita.Visible = True
                                cmdAnnullaAccred.Visible = False
                            End If
                            If dtrGenerico("Attiva") = True And (Session("TipoUtente") = "U" Or Session("TipoUtente") = "R") Then
                                cmdAccredita.Visible = False
                                cmdAnnullaAccred.Visible = False
                            End If

                            cmdSalva.Visible = True
                            'mod. da s.c. il 18/11/2008 nn è possibile cancellare sedi principali 
                            If ddlTipologia.SelectedItem.Text = "Operativa" Then
                                cmdCancella.Visible = True
                            Else
                                cmdCancella.Visible = False
                            End If
                            cmdRipristina.Visible = False
                            cmdChiudi.Visible = True
                            cmdAnnulla.Visible = False
                            cmdIncludi.Visible = False
                        End If
                    Case ""
                        lblsedeInclusa.Text = ""
                        cmdSalva.Visible = True
                        cmdIncludi.Visible = False
                        cmdAnnulla.Visible = False
                        cmdCancella.Visible = False
                        cmdChiudi.Visible = True
                        cmdRipristina.Visible = False
                    Case Else
                        'Case "si"
                        lblsedeInclusa.Visible = True
                        lblsedeInclusa.Text = "Sede Inclusa"
                        If dtrGenerico("Attiva") = False And dtrGenerico("DaAccreditare") = False Then
                            'cmdSalva.Visible = False
                            cmdCancella.Visible = False
                            'cmdRipristina.Visible = False
                            If ControllaEnteFiglio(lblidEnte.Value) = True Then
                                '---------------
                                'mod. il 04/03/2008 da s.c. per accreditamento marzo 2008
                                'cmdRipristina.Visible = True
                                cmdRipristina.Visible = False
                                '----------------
                            Else
                                cmdRipristina.Visible = False
                            End If
                            cmdChiudi.Visible = True
                            cmdIncludi.Visible = False
                            cmdAnnulla.Visible = False
                        Else
                            If dtrGenerico("DaAccreditare") = True And (Session("TipoUtente") = "U" Or Session("TipoUtente") = "R") Then
                                cmdAccredita.Visible = True
                                cmdAnnullaAccred.Visible = False
                            End If
                            'If lblStato.Text = "Attiva" And Session("TipoUtente") = "U" Then
                            If dtrGenerico("Attiva") = True And (Session("TipoUtente") = "U" Or Session("TipoUtente") = "R") Then
                                cmdAccredita.Visible = False
                                cmdAnnullaAccred.Visible = False
                            End If
                            cmdSalva.Visible = True
                            '----------------------------
                            cmdRipristina.Visible = False
                            If ddlTipologia.SelectedItem.Text = "Operativa" Then
                                cmdCancella.Visible = True
                            Else
                                cmdCancella.Visible = False
                            End If
                            cmdChiudi.Visible = True
                            cmdAnnulla.Visible = False
                            cmdIncludi.Visible = False
                        End If
                End Select
            End If
        Else
            lblsedeInclusa.Text = ""
            cmdSalva.Visible = True
            cmdIncludi.Visible = False
            cmdAnnulla.Visible = False
            cmdCancella.Visible = False
            cmdChiudi.Visible = True
            cmdRipristina.Visible = False
        End If
        If Not dtrGenerico Is Nothing Then
            dtrGenerico.Close()
            dtrGenerico = Nothing
        End If
        'controllo se si tratta di inserimento di una sede per un ente figlio
        If Not Context.Items("identefiglio") Is Nothing Then
            ddlEntiFigli.Visible = False
            lblEntefiglio.Visible = False
            imgOpenFigli.Visible = False
        End If

    End Sub

    Private Sub AbilitaCampiMaschera(ByVal isAbilitato As Boolean)
        txtCap.Enabled = isAbilitato
        txtCivico.Enabled = isAbilitato
        txtCodice.Value = isAbilitato
        ddlComune.Enabled = isAbilitato
        ddlProvincia.Enabled = isAbilitato
        txtdenominazione.Enabled = isAbilitato
        txtemail.Enabled = isAbilitato
        txtfax.Enabled = isAbilitato
        txthttp.Enabled = isAbilitato
        txtIndirizzo.Enabled = isAbilitato
        TxtDettaglioRecapito.Enabled = isAbilitato
        TxtPalazzina.Enabled = isAbilitato
        TxtScala.Enabled = isAbilitato
        TxtPiano.Enabled = isAbilitato
        TxtInterno.Enabled = isAbilitato
        TxtNumMaxVol.Enabled = isAbilitato
        ddlTitoloGiuridico.Enabled = isAbilitato
        txtPrefFax.Enabled = isAbilitato
        txtprefisso.Enabled = isAbilitato
        txtTelefono.Enabled = isAbilitato
        ddlTipologia.Enabled = False
        'cmdEmail.Enabled = False
        cmdHttp.Enabled = isAbilitato
        If Session("TipoUtente") <> "E" And isAbilitato = True Then
            'lblDataControlloHttp.Visible = True
            'lbldataControlloEmail.Visible = True
            chkFalsohttp.Visible = True
            chkVerohttp.Visible = True
            chkFalsoEmail.Visible = True
            chkVeroEmail.Visible = True
            checkHttp.Visible = True
            checkEmail.Visible = True
        End If
        cmdCaricaFile.Enabled = isAbilitato
        txtLocalita.Enabled = False
        txtAltroTitoloGiuridicio.Enabled = False
        ddlCertificazione.Enabled = False
        ddlEntiFigli.Enabled = isAbilitato
        cmdAllega.Enabled = isAbilitato
        ChkTutela.Enabled = isAbilitato
        btnModificaLSE.Enabled = isAbilitato
        btnEliminaLSE.Enabled = isAbilitato
        TxtSoggettoCapoSede.Enabled = isAbilitato
        ChkTutela.Enabled = isAbilitato
        rbSE1.Enabled = isAbilitato
        rbSE2.Enabled = isAbilitato
        rbNoSE1.Enabled = isAbilitato
        rbNoSE2.Enabled = isAbilitato

    End Sub

    Private Sub Clear()
        txtCap.Text = ""
        txtCivico.Text = ""
        txtCodice.Value = ""
        txtdenominazione.Text = ""
        txtemail.Text = ""
        txthttp.Text = ""
        txtfax.Text = ""
        txtIndirizzo.Text = ""
        TxtDettaglioRecapito.Text = ""
        TxtPalazzina.Text = ""
        TxtScala.Text = ""
        TxtPiano.Text = ""
        TxtInterno.Text = ""
        TxtNumMaxVol.Text = ""
        ddlTitoloGiuridico.SelectedValue = 0
        txtPrefFax.Text = ""
        txtprefisso.Text = ""
        'ddlProvincia.SelectedValue = ""
        txtTelefono.Text = ""
        'lbldataControlloEmail.Value = ""
        'lblDataControlloHttp.Value = ""
        ddlEntiFigli.SelectedValue = 0
    End Sub
    Private Sub PulisciMaschera()
        'Realizzata da Alessandra taballione 07/03/04
        'Private Sub per la Personalizzazione della Maschera e
        'in particolare la pulizia di tutti i dati presenti nei vari Oggetti
        'e relativa Disabilitazione degli Stessi.
        txtCap.Text = ""
        txtCap.Enabled = False
        txtCivico.Text = ""
        txtCivico.Enabled = False
        txtCodice.Value = ""
        ddlComune.Text = ""
        ddlComune.Enabled = False
        txtdenominazione.Text = ""
        txtdenominazione.Enabled = False
        txtemail.Text = ""
        txtemail.Enabled = False
        txthttp.Text = ""
        txthttp.Enabled = False
        txtfax.Text = ""
        txtfax.Enabled = False
        txtIndirizzo.Text = ""
        txtIndirizzo.Enabled = False
        'dettaglioIndirizzo
        TxtDettaglioRecapito.Text = ""
        TxtDettaglioRecapito.Enabled = False

        '*** agg. da simona cordella il 19/05/2009 per nuovo accreditamento luglio 2009
        TxtPalazzina.Text = ""
        TxtPalazzina.Enabled = False
        TxtScala.Text = ""
        TxtScala.Enabled = False
        TxtPiano.Text = ""
        TxtPiano.Enabled = False
        TxtInterno.Text = ""
        TxtInterno.Enabled = False
        TxtNumMaxVol.Text = ""
        TxtNumMaxVol.Enabled = False
        ddlTitoloGiuridico.SelectedValue = 0
        '***
        txtPrefFax.Text = ""
        txtPrefFax.Enabled = False
        txtprefisso.Text = ""
        txtprefisso.Enabled = False
        'ddlProvincia.SelectedValue = ""
        ddlProvincia.Enabled = False
        txtTelefono.Text = ""
        txtTelefono.Enabled = False
        'ddlTipologia.Items.Clear()
        'ddlTipologia.Enabled = False
        cmdSalva.Enabled = False
        'cmdEmail.Enabled = False
        cmdHttp.Enabled = False
        chkFalsoEmail.Enabled = False
        chkVeroEmail.Enabled = False
        chkFalsohttp.Enabled = False
        chkVerohttp.Enabled = False
        chkVerohttp.Enabled = False
        chkFalsoEmail.Checked = False
        chkVeroEmail.Checked = False
        chkFalsohttp.Checked = False
        chkVerohttp.Checked = False
        'lbldataControlloEmail.Value = ""
        'lblDataControlloHttp.Value = ""
        ddlEntiFigli.SelectedValue = 0


        cmdAllega.Enabled = False
        ChkTutela.Checked = False
        ChkTutela.Enabled = False
        btnModificaLSE.Enabled = False
        btnEliminaLSE.Enabled = False
        TxtSoggettoCapoSede.Text = ""
        TxtSoggettoCapoSede.Enabled = False
        rbSE1.Checked = False
        rbSE1.Enabled = False
        rbSE2.Checked = False
        rbSE2.Enabled = False
        rbNoSE1.Checked = False
        rbNoSE2.Checked = False
        rbNoSE1.Enabled = False
        rbNoSE2.Enabled = False

    End Sub
    Sub CaricaDataGrid(ByRef datagrid As DataGrid)
        Dim appo As String
        appo = dgRisultatoRicerca.CurrentPageIndex
        datagrid.DataSource = dtsGenerico
        datagrid.DataBind()
        If datagrid.Items.Count = 0 Then
            datagrid.Caption = "Non è presente alcuna sede."
        Else
            datagrid.Caption = "Elenco Sedi"




        End If

    End Sub
    Private Sub RicercaSediAttuazione(ByVal IdEnteSede As Integer)
        'Realizzato da Alessandra Taballione 07/03/04
        'Ricerca della sede di attuazione della sede dell'Ente
        'e Richiamo della Sub per il caricamento della dataGrid 
        'che contiene la lista delle sedi di Attuazione della sede dell'ente
        If lblsedeInclusa.Text = "" Then
            strsql = "select entirelazioni.identePadre,entisedi.idente as identefiglio,entisediattuazioni.identesedeAttuazione,entisediattuazioni.denominazione," &
            " entisediattuazioni.identeSede,entisediattuazioni.Note,StatoEnteSede," &
            " case isnull(AssociaentirelazionisediAttuazioni.idAssociaentirelazionisediAttuazioni,-1) " &
            " when -1 then 'propria' else '' end acquisita," &
            " AssociaentirelazionisediAttuazioni.identeRelazione,case isnull(entisediattuazioni.Certificazione,2) when 0 then 'No' when 1 then 'Si' when 2 then 'Da Valutare' end as Certificazione," &
            " entiSedi.IdAllegatoLSE, entiSedi.AnomaliaIndirizzo, entiSedi.AnomaliaNome, entiSedi.NormativaTutela, entiSedi.SoggettoEstero, entiSedi.NonDisponibilitaSede, entiSedi.DisponibilitaSede, entiSedi.AnomaliaIndirizzoGoogle " &
            " from entisediattuazioni " &
            " inner join entisedi on (entisedi.identesede=entisediattuazioni.identesede) " &
            " left join  AssociaentirelazionisediAttuazioni on " &
            " (entisediattuazioni.identesedeAttuazione=AssociaentirelazionisediAttuazioni.identesedeAttuazione)" &
            " left join entirelazioni on (entirelazioni.identerelazione=AssociaentirelazionisediAttuazioni.identerelazione) " &
            " inner join statientisedi on " &
            " (statientisedi.idStatoentesede=entisediattuazioni.idStatoentesede) " &
            " where entisediattuazioni.idEnteSede=" & IdEnteSede & "" 'txtidsede.Value
            dtsGenerico = ClsServer.DataSetGenerico(strsql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
        Else
            strsql = "select entirelazioni.identePadre,entisedi.idente as identefiglio,entisediattuazioni.identesedeAttuazione,entisediattuazioni.denominazione," &
                    " entisediattuazioni.identeSede,entisediattuazioni.Note,StatoEnteSede," &
                    " case isnull(AssociaentirelazionisediAttuazioni.idAssociaentirelazionisediAttuazioni,-1) " &
                    " when -1 then 'no' else convert(varchar(10),AssociaentirelazionisediAttuazioni.idAssociaentirelazionisediAttuazioni) end acquisita," &
                    " AssociaentirelazionisediAttuazioni.identeRelazione,case isnull(entisediattuazioni.Certificazione,2) when 0 then 'No'  when 1 then 'Si' when 2 then 'Da Valutare' end as Certificazione," &
                    " entiSedi.IdAllegatoLSE, entiSedi.AnomaliaIndirizzo, entiSedi.AnomaliaNome, entiSedi.NormativaTutela, entiSedi.SoggettoEstero, entiSedi.NonDisponibilitaSede, entiSedi.DisponibilitaSede, entiSedi.AnomaliaIndirizzoGoogle " &
                    " from entisediattuazioni " &
                    " inner join entisedi on (entisedi.identesede=entisediattuazioni.identesede) " &
                    " left join  AssociaentirelazionisediAttuazioni on " &
                    " (entisediattuazioni.identesedeAttuazione=AssociaentirelazionisediAttuazioni.identesedeAttuazione)" &
                    " left join entirelazioni on (entirelazioni.identerelazione=AssociaentirelazionisediAttuazioni.identerelazione) " &
                    " inner join statientisedi on " &
                    " (statientisedi.idStatoentesede=entisediattuazioni.idStatoentesede) " &
                    " where entisediattuazioni.idEnteSede=" & IdEnteSede & "" 'txtidsede.Value
            dtsGenerico = ClsServer.DataSetGenerico(strsql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
        End If
        CaricaDataGrid(dgRisultatoRicerca)
        ' agg da simona cordella il 11/12/2008
        ' se sono un utente di tipo E rendo la colonna invisibile certificazione invisibile
        If Session("TipoUtente") = "E" Then
            dgRisultatoRicerca.Columns(10).Visible = False
        End If
        controllaChek()
        ColoraCelle()
    End Sub
    Private Sub RicercaSediAttuazioneVariazione()
        'Realizzato da Alessandra Taballione 07/03/04
        'Ricerca della sede di attuazione della sede dell'Ente
        'e Richiamo della Sub per il caricamento della dataGrid 
        'che contiene la lista delle sedi di Attuazione della sede dell'ente
        strsql = " SELECT entirelazioni.identePadre,entisedi.idente as identefiglio, " &
                " esa.identesedeAttuazione, entisediattuazioni.denominazione, " &
                " entisediattuazioni.identeSede, entisediattuazioni.Note, StatoEnteSede, " &
                " case isnull(AssociaentirelazionisediAttuazioni.idAssociaentirelazionisediAttuazioni,-1)  " &
                " when -1 then 'no' else convert(varchar(10),AssociaentirelazionisediAttuazioni.idAssociaentirelazionisediAttuazioni) end acquisita, " &
                "  AssociaentirelazionisediAttuazioni.identeRelazione, " &
                " case isnull(entisediattuazioni.Certificazione,2) when 0 then 'No'  when 1 then 'Si' when 2 then 'Da Valutare' end as Certificazione,  " &
                " entiSedi.IdAllegatoLSE, entiSedi.AnomaliaIndirizzo, entiSedi.AnomaliaNome, entiSedi.NormativaTutela, entiSedi.SoggettoEstero, entiSedi.NonDisponibilitaSede, entiSedi.DisponibilitaSede, entiSedi.AnomaliaIndirizzoGoogle " &
                " FROM Accreditamento_VariazioneSedi entisediattuazioni  " &
                " INNER JOIN entisediattuazioni esa on esa.IdEnteSede = entisediattuazioni.IdEnteSede " &
                " INNER JOIN entisedi on entisedi.identesede=entisediattuazioni.identesede " &
                " LEFT JOIN  AssociaentirelazionisediAttuazioni on esa.identesedeAttuazione=AssociaentirelazionisediAttuazioni.identesedeAttuazione " &
                " LEFT JOIN entirelazioni on entirelazioni.identerelazione=AssociaentirelazionisediAttuazioni.identerelazione " &
                " INNER JOIN statientisedi on statientisedi.idStatoentesede=esa.idStatoentesede " &
                " WHERE entisediattuazioni.idEnteSede=" & txtidsede.Value & " and entisediattuazioni.StatoVariazione =0 "
        dtsGenerico = ClsServer.DataSetGenerico(strsql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))

        CaricaDataGrid(dgRisultatoRicerca)
        ' agg da simona cordella il 11/12/2008
        ' se sono un utente di tipo E rendo la colonna invisibile certificazione invisibile
        If Session("TipoUtente") = "E" Then
            dgRisultatoRicerca.Columns(10).Visible = False
        End If
        controllaChek()
        ColoraCelle()
    End Sub

    Private Function trovaindex(ByVal stringa As String, ByVal obj As Object)
        Dim i As Integer
        For i = 0 To obj.Items.Count
            If obj.Items(i).Text = stringa Then
                trovaindex = i
                Exit For
            End If
        Next
    End Function
    Private Function ControllaSedeAttiva() As Boolean
        'Generato da Alessandra Taballione il 07/03/2004
        'Funzione che controlla in base al caricamento della dataGrid se la 
        'sede Selezionata è Attiva o no.
        Dim conta As Integer
        Dim item As DataGridItem
        For Each item In dgRisultatoRicerca.Items
            If dgRisultatoRicerca.Items(item.ItemIndex).Cells(3).Text = "Accreditata" Or dgRisultatoRicerca.Items(item.ItemIndex).Cells(3).Text = "Presentata" Then
                conta = conta + 1
                Exit For
            End If
        Next
        If conta > 0 Then
            ControllaSedeAttiva = True
        Else
            ControllaSedeAttiva = False
        End If
    End Function
    Private Sub ColoraCelle()
        'Generato da Alessandra Taballione il 01/04/04
        'VAriazione del Colore secondo lo stato della sede.
        'Attiva=Verde;Presentata=gialla;Cancellata=Rossa;Sospesa=
        Dim item As DataGridItem
        Dim intConta As Integer
        Dim img As New ImageButton
        For Each item In dgRisultatoRicerca.Items
            Select Case dgRisultatoRicerca.Items(item.ItemIndex).Cells(3).Text
                Case "Accreditata"
                    For intConta = 0 To 14
                        dgRisultatoRicerca.Items(item.ItemIndex).Cells(intConta).BackColor = Color.LightGreen
                        'dgRisultatoRicerca.Items(item.ItemIndex).Cells(intConta).ForeColor = Color.LightGreen
                    Next
                    'aggiunto il 04/12/2008 da simona cordella
                    'se sono un utene U o R coloro il font della griglio in viola sono le sede è da valutare
                    If (Session("TipoUtente") = "U" Or Session("TipoUtente") = "R") Then
                        'se la sede è da valutare coloro il FONT DI VIOLA
                        Select Case dgRisultatoRicerca.Items(item.ItemIndex).Cells(10).Text
                            Case "Da Valutare"
                                For intConta = 0 To dgRisultatoRicerca.Columns.Count - 1
                                    'dgRisultatoRicerca.Items(item.ItemIndex).Cells(intConta).ForeColor = Color.FromArgb(201, 96, 185)
                                    dgRisultatoRicerca.Items(item.ItemIndex).Cells(intConta).Font.Bold = True
                                Next
                            Case "No"
                                For intConta = 0 To dgRisultatoRicerca.Columns.Count - 1
                                    'dgRisultatoRicerca.Items(item.ItemIndex).Cells(intConta).ForeColor = Color.Red
                                    'dgRisultatoRicerca.Items(item.ItemIndex).Cells(x).Font.Bold = True
                                Next
                        End Select
                    End If
                Case "Presentata"
                    For intConta = 0 To 14
                        dgRisultatoRicerca.Items(item.ItemIndex).Cells(intConta).BackColor = Color.Khaki
                        'dgRisultatoRicerca.Items(item.ItemIndex).Cells(intConta).ForeColor = Color.Khaki
                    Next
                    'aggiunto il 04/12/2008 da simona cordella
                    'se sono un utene U o R coloro il font della griglio in viola sono le sede è da valutare
                    If (Session("TipoUtente") = "U" Or Session("TipoUtente") = "R") Then
                        'se la sede è da valutare coloro il FONT DI VIOLA
                        Select Case dgRisultatoRicerca.Items(item.ItemIndex).Cells(10).Text
                            Case "Da Valutare"
                                For intConta = 0 To dgRisultatoRicerca.Columns.Count - 1
                                    '--dgRisultatoRicerca.Items(item.ItemIndex).Cells(intConta).ForeColor = Color.FromArgb(201, 96, 185)
                                    dgRisultatoRicerca.Items(item.ItemIndex).Cells(intConta).Font.Bold = True
                                Next
                            Case "No"
                                For intConta = 0 To dgRisultatoRicerca.Columns.Count - 1
                                    '-- dgRisultatoRicerca.Items(item.ItemIndex).Cells(intConta).ForeColor = Color.Red
                                    'dgRisultatoRicerca.Items(item.ItemIndex).Cells(x).Font.Bold = True
                                Next
                        End Select
                    End If
                Case "Sospesa"
                    For intConta = 0 To 14
                        dgRisultatoRicerca.Items(item.ItemIndex).Cells(intConta).BackColor = Color.Gainsboro
                        'dgRisultatoRicerca.Items(item.ItemIndex).Cells(intConta).ForeColor = Color.Red
                    Next
                Case "Cancellata"
                    For intConta = 0 To 14
                        dgRisultatoRicerca.Items(item.ItemIndex).Cells(intConta).BackColor = Color.LightSalmon
                        'dgRisultatoRicerca.Items(item.ItemIndex).Cells(intConta).ForeColor = Color.Gainsboro
                    Next
            End Select
            img = DirectCast(item.FindControl("IdImgAlert"), ImageButton)
            img.Visible = False
            img.ToolTip = ""
            If dgRisultatoRicerca.Items(item.ItemIndex).Cells(12).Text = "True" Then
                img.Visible = True
                img.ToolTip = "Il nome presenta anomalie o è duplicato" + vbNewLine
            End If
            'If dgRisultatoRicerca.Items(item.ItemIndex).Cells(13).Text = "True" Then
            '    img.Visible = True
            '    img.ToolTip = img.ToolTip + "Indirizzo corrispondente ad altra sede" + vbNewLine
            'End If
            If dgRisultatoRicerca.Items(item.ItemIndex).Cells(14).Text = "True" Then
                img.Visible = True
                img.ToolTip = img.ToolTip + "Indirizzo non corrispondente a quello trovato da Google"
            End If
            img.Enabled = False
        Next
    End Sub
    Private Function ControllaSedeInclusa() As Boolean
        'Generato da Alessandra Taballione il 07/03/2004
        'Funzione che controlla in base al caricamento della dataGrid se la 
        'sede Selezionata è Attiva o no.
        Dim conta As Integer
        Dim item As DataGridItem
        For Each item In dgRisultatoRicerca.Items
            If dgRisultatoRicerca.Items(item.ItemIndex).Cells(3).Text = "Attiva" Or dgRisultatoRicerca.Items(item.ItemIndex).Cells(3).Text = "Presentata" Then
                If IsNumeric(dgRisultatoRicerca.Items(item.ItemIndex).Cells(7).Text) Then
                    conta = conta + 1
                    Exit For
                End If
            End If
        Next
        If conta > 0 Then
            ControllaSedeInclusa = True
        Else
            ControllaSedeInclusa = False
        End If
    End Function
    Private Sub ControllaSedeFisicaInclusa()
        'Imppletato da Alessandra Taballione il 21.06.2004
        'Se la sede è acquisita specifico l'ente che l'ha acquisita
        strsql = "select associaentirelazionisedi.idassociaentirelazionisedi,entirelazioni.identePadre,entirelazioni.identefiglio,enti.denominazione " &
            " from " &
            " associaentirelazionisedi " &
            " inner join entirelazioni on " &
            " entirelazioni.identerelazione=associaentirelazionisedi.identerelazione " &
            " inner join enti on enti.idente=entirelazioni.identepadre " &
            " where associaentirelazionisedi.identesede=" & txtidsede.Value '& " and identeFiglio=" & lblidEnte.Value & ""
        If Not dtrGenerico Is Nothing Then
            dtrGenerico.Close()
            dtrGenerico = Nothing
        End If
        dtrGenerico = ClsServer.CreaDatareader(strsql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
        dtrGenerico.Read()
        If dtrGenerico.HasRows = True Then
            If dtrGenerico("identePadre") <> Session("idEnte") Then
                lblsedeInclusa.Text = "Sede Inclusa"
                lblsedeInclusa.Visible = True
                lblda.Visible = True
                lbldaEntePadre.Text = IIf(Len(dtrGenerico("denominazione")) > 20, Mid(dtrGenerico("denominazione"), 1, 20) & "...", dtrGenerico("denominazione"))
                lbldaEntePadre.Visible = True
                lblacquisita.Value = dtrGenerico("idassociaentirelazionisedi")
                AbilitaCampiMaschera(False)
                'cmdSalva.Visible = False
                cmdCancella.Visible = False
                cmdRipristina.Visible = False
                cmdChiudi.Visible = True
                cmdIncludi.Visible = False
                cmdAnnulla.Visible = False
                cmdAnnullaAccred.Visible = False
            End If
        End If
        If Not dtrGenerico Is Nothing Then
            dtrGenerico.Close()
            dtrGenerico = Nothing
        End If
    End Sub


    Private Sub controllaChek()
        'realizzato da Alessandra Taballione  08/03/04
        'Verifica delle sedi attuazione incluse o non incluse
        Dim item As DataGridItem
        dgRisultatoRicerca.Columns(5).Visible = False
        For Each item In dgRisultatoRicerca.Items
            Dim check As CheckBox = DirectCast(item.FindControl("check1"), CheckBox)
            Select Case dgRisultatoRicerca.Items(item.ItemIndex).Cells(7).Text
                Case "no"
                    check.Checked = False
                Case "propria"
                    dgRisultatoRicerca.Columns(5).Visible = False
                    Exit For
                Case Else
                    'Si
                    check.Checked = True
            End Select
            check.Enabled = False
        Next
    End Sub
#End Region

    Private Sub chkRiferimentoRimborsi_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkRiferimentoRimborsi.CheckedChanged
        If chkRiferimentoRimborsi.Checked = True Then
            '--- verifico se c'è un'altra sede di riferimento.
            strsql = "SELECT Denominazione " &
                     "FROM EntiSedi " &
                     "WHERE IdEnte = " & Session("IdEnte") & " " &
                     "AND RiferimentoRimborsi = 1"
            If Not dtrGenerico Is Nothing Then
                dtrGenerico.Close()
                dtrGenerico = Nothing
            End If
            dtrGenerico = ClsServer.CreaDatareader(strsql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
            dtrGenerico.Read()
            If dtrGenerico.HasRows = True Then
                '--- c'è un'altra sede di riferimento.
                chkRiferimentoRimborsi.Checked = False
                msgErrore.Text = "Attenzione e' gia' stata scelta " & dtrGenerico(0) & " come sede di riferimento per i rimborsi."
            End If
            ChiudiDataReader(dtrGenerico)
        End If
    End Sub



    Sub AnnullaAccreditamento()
        Context.Items.Add("identesede", txtidsede.Value)
        Context.Items.Add("idente", lblidEnte.Value)
        Context.Items.Add("Ente", lblEnte.Text)
        Context.Items.Add("acquisita", lblacquisita.Value)
        Context.Items.Add("tipoazione", "Modifica")
        Context.Items.Add("stato", lblStato.Text)
        Context.Items.Add("Tipologia", ddlTipologia.SelectedItem.Text)
        Server.Transfer("WfrmAnnullaAccreditamento.aspx")
    End Sub

    'Funzione per controllare se sono presenti dei numeri all'interno di una stringa.(TRUE: c'è almeno un numero)
    Function checkNumeri(ByVal myString As String) As Boolean
        Dim maxIntX As Int16 = myString.Length - 1
        Dim intX As Int16 = 0
        For intX = 0 To maxIntX
            If IsNumeric(myString.Substring(intX, 1)) = True Then
                Return True
                Exit Function
            End If
        Next
        Return False
    End Function

    'Funzione per controllare lo stato dell'ente figlio la cui sede si vuole ripristinare
    'ritorna TRUE se lo stato dell'ente figlio non è "Chiuso"
    Function checkStatoEnteFiglio(ByVal myIntIdEnteFiglio As String) As Boolean
        strsql = "SELECT * FROM entisedi INNER JOIN enti ON entisedi.IDEnte = enti.IDEnte INNER JOIN " &
                 "statienti ON enti.IDStatoEnte = statienti.IDStatoEnte WHERE statienti.Sospeso = 1 " &
                 "AND entisedi.IDEnteSede = " & myIntIdEnteFiglio & " AND entisedi.IDEnte <> " & Session("idEnte")
        If Not dtrGenerico Is Nothing Then
            dtrGenerico.Close()
            dtrGenerico = Nothing
        End If
        dtrGenerico = ClsServer.CreaDatareader(strsql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
        If dtrGenerico.HasRows = True Then
            Return False
        Else
            Return True
        End If
    End Function

    Private Sub AbilitaDisabilitaTxtComune()
        'aggiunto il 10/10/2006 da simona cordella
        'Verifico se l'utente è abilitato al Menu' "Forza Modifica Comune" 
        strsql = "SELECT AssociaUtenteGruppo.username, VociMenu.IdVoceMenu, VociMenu.VoceMenu, VociMenu.ImmaginePassiva, VociMenu.ImmagineAttiva, VociMenu.Link," &
                 " VociMenu.IdVoceMenuPadre" &
                 " FROM VociMenu" &
                 " INNER JOIN 	AbilitazioniProfiliVociMenu ON VociMenu.IdVoceMenu = AbilitazioniProfiliVociMenu.IdVoceMenu" &
                 " INNER JOIN	Profili ON AbilitazioniProfiliVociMenu.IdProfilo = Profili.IdProfilo"
        '============================================================================================================================
        '====================================================30/09/2008==============================================================
        '=======MODIFICA EFFETTUATA DA Kronk (99 scimmie saltavano sul letto, una cadde in terra e si ruppe il cervelletto...)=======
        '=================AGGIUNTA JOIN SU PROFILO READ PER ABILITARE LEGGERE LE ABILITAZIONI ANCHE DELL'UTENTE READ=================
        '============================================================================================================================
        If UCase(Me.TemplateSourceDirectory) <> "/HELIOSREAD" Then
            strsql = strsql & " INNER JOIN	AssociaUtenteGruppo ON Profili.IdProfilo = AssociaUtenteGruppo.IdProfilo "
        Else
            strsql = strsql & " INNER JOIN	AssociaUtenteGruppo ON Profili.IdProfilo = AssociaUtenteGruppo.IdProfiloRead "
        End If
        strsql = strsql & " LEFT JOIN 	RestrizioniUtenteVociMenu ON AssociaUtenteGruppo.UserName = RestrizioniUtenteVociMenu.UserName and RestrizioniUtenteVociMenu.IdVoceMenu=VociMenu.IdVoceMenu" &
                 " WHERE VociMenu.descrizione = 'Forza Modifica Comune'" &
                 " AND AssociaUtenteGruppo.username ='" & Session("Utente") & "' and (RestrizioniUtenteVociMenu.TipoRestrizione <> 0 or RestrizioniUtenteVociMenu.TipoRestrizione is null)"
        dtrGenerico = ClsServer.CreaDatareader(strsql, Session("conn"))
        If dtrGenerico.HasRows = True Then
            ddlComune.Enabled = True
            ddlComune.BackColor = ddlComune.BackColor.White

        Else
            ddlComune.Enabled = False
            ddlComune.BackColor = ddlComune.BackColor.Gainsboro
        End If
        dtrGenerico.Close()
        dtrGenerico = Nothing
    End Sub
    '''Private Function verificamulticap() As Boolean
    '''    If IdComuneRes = "" Then
    '''        IdComuneRes = Request.Form("txtIdComune")
    '''    Else
    '''        IdComuneRes = ddlComune.SelectedItem.Text
    '''    End If
    '''    If Not dtrGenerico Is Nothing Then
    '''        dtrGenerico.Close()
    '''        dtrGenerico = Nothing
    '''    End If
    '''    'strsql = "select multicap from comuni where idcomune='" & Session("IdComune") & "' and multicap=1"
    '''    strsql = "select distinct (idComune) from VW_ValidazioneIndirizzo where idcomune='" & IdComuneRes & "'"
    '''    dtrGenerico = ClsServer.CreaDatareader(strsql, Session("Conn"))
    '''    dtrGenerico.Read()
    '''    If dtrGenerico.HasRows = True Then
    '''        Return True
    '''    End If

    '''    If Not dtrGenerico Is Nothing Then
    '''        dtrGenerico.Close()
    '''        dtrGenerico = Nothing
    '''    End If
    '''End Function
    '''Private Function verificaindirizzo() As Boolean
    '''    If Not dtrGenerico Is Nothing Then
    '''        dtrGenerico.Close()
    '''        dtrGenerico = Nothing
    '''    End If
    '''    strsql = "select * from VW_ValidazioneIndirizzo where idcomune='" & IdComuneRes & "' and indirizzo='" & txtindirizzo.Text & "'"
    '''    dtrGenerico = ClsServer.CreaDatareader(strsql, Session("Conn"))
    '''    dtrGenerico.Read()
    '''    If dtrGenerico.HasRows = True Then
    '''        Return True
    '''    End If

    '''    If Not dtrGenerico Is Nothing Then
    '''        dtrGenerico.Close()
    '''        dtrGenerico = Nothing
    '''    End If

    '''End Function
    Private Function VerificaModificaIndirizzoSede() As Boolean
        'Agg. da sc il 21/04/2009
        'Controllo se sono stati modificati INDIRIZZO, NUMERO CIVICO, CAP o COMUNE 
        Dim strSql As String
        Dim dtrVer As SqlClient.SqlDataReader

        If Not dtrVer Is Nothing Then
            dtrVer.Close()
            dtrVer = Nothing
        End If
        strSql = "SELECT  ISNULL(Indirizzo,'') AS Indirizzo, ISNULL(Civico,'') AS Civico ,ISNULL(IDComune,'') AS IDComune, ISNULL(CAP,'') AS CAP FROM entisedi where idEnteSede=" & txtidsede.Value & " and idente=" & lblidEnte.Value & ""
        dtrVer = ClsServer.CreaDatareader(strSql, Session("conn"))
        If dtrVer.HasRows = True Then
            dtrVer.Read()
            If (dtrVer("Indirizzo") <> txtIndirizzo.Text) Or (dtrVer("Civico") <> txtCivico.Text) Or (dtrVer("IDComune") <> ddlComune.SelectedItem.Value) Or (dtrVer("CAP") <> txtCap.Text) Then
                If Not dtrVer Is Nothing Then
                    dtrVer.Close()
                    dtrVer = Nothing
                End If
                Return True
            Else
                If Not dtrVer Is Nothing Then
                    dtrVer.Close()
                    dtrVer = Nothing
                End If
                Return False
            End If
        End If
    End Function
    Private Sub CaricoComboTitoloGiuridico(ByVal AlboEnte As String)
        'Agg. da Simona Cordella il 19/05/2009
        'Caricamento combo TitoloGiuridico (T
        Dim dtrTG As SqlClient.SqlDataReader

        If Not dtrTG Is Nothing Then
            dtrTG.Close()
            dtrTG = Nothing
        End If

        strsql = " SELECT  0 AS IdTitoloGiuridico, '' AS TitoloGiuridico  "
        strsql = strsql & " From TitoliGiuridici "
        strsql = strsql & " Union "
        strsql = strsql & " SELECT IdTitoloGiuridico, TitoloGiuridico "
        strsql = strsql & " From TitoliGiuridici "

        If AlboEnte = "" Then
            strsql &= " WHERE (ALBO='SCU' OR ALBO IS NULL) "
        Else
            strsql &= " WHERE (ALBO='" & AlboEnte & "' OR ALBO IS NULL) "
        End If

        strsql = strsql & " Order by IdTitoloGiuridico"

        'eseguo la query
        dtrTG = ClsServer.CreaDatareader(strsql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))

        'assegno il datadearder alla combo caricando così descrizione e id
        ddlTitoloGiuridico.DataSource = dtrTG
        ddlTitoloGiuridico.Items.Add("")
        ddlTitoloGiuridico.DataTextField = "TitoloGiuridico"
        ddlTitoloGiuridico.DataValueField = "IdTitoloGiuridico"
        ddlTitoloGiuridico.DataBind()
        If Not dtrTG Is Nothing Then
            dtrTG.Close()
            dtrTG = Nothing
        End If
    End Sub

    Private Function ControlloUtilizzoSede(Optional ByVal IdEnteSede As Integer = 0) As Boolean
        Dim StrSql As String
        Dim rstSede As SqlClient.SqlDataReader
        Dim strPalazzina As String
        Dim strScala As String
        Dim intPiano As Integer
        Dim strInterno As String
        Dim intIdEnteSede As Integer
        'agg. da Simona COrdella il 20/05/2009 per nuovo accreditamento Luglio 2009
        '1. Verifico se la denominazione della sede, l'indirizzo, il numero civico, il cap e il comune già esistono per l'ente
        '2. nel caso di modifica sede escludo la sede che sto modificando
        '3.
        If Not rstSede Is Nothing Then
            rstSede.Close()
            rstSede = Nothing
        End If
        StrSql = "Select identesede,denominazione,indirizzo,civico,idcomune,cap,isnull(palazzina,'') as palazzina,isnull(scala,'') as scala, isnull(piano,'') as piano,isnull(interno,'') as interno  from entisedi "
        StrSql = StrSql & " where denominazione ='" & txtdenominazione.Text.Replace("'", "''") & "' "
        StrSql = StrSql & " and indirizzo='" & txtIndirizzo.Text.Replace("'", "''") & "' "
        StrSql = StrSql & " and civico='" & txtCivico.Text.Replace("'", "''") & "' "
        StrSql = StrSql & " and idcomune =" & ddlComune.SelectedValue & "  and cap='" & txtCap.Text & "'"
        If IdEnteSede <> 0 Then 'fase modifica sede
            StrSql = StrSql & " and identesede <> " & IdEnteSede
        End If
        rstSede = ClsServer.CreaDatareader(StrSql, Session("conn"))
        If rstSede.HasRows = False Then
            'caso in cui nn ci sono sedi con stessa denominazione,civico,cap e comune
            If Not rstSede Is Nothing Then
                rstSede.Close()
                rstSede = Nothing
            End If
            Return False
        Else ' se esistono sedi con stessa denominazione,civico,cap e comune
            rstSede.Read()
            intIdEnteSede = rstSede("identesede")
            strPalazzina = rstSede("palazzina")
            strScala = rstSede("scala")
            intPiano = rstSede("piano")
            strInterno = rstSede("interno")
            If Not rstSede Is Nothing Then
                rstSede.Close()
                rstSede = Nothing
            End If

            StrSql = "Select identesede,denominazione,indirizzo,civico,idcomune,cap,isnull(palazzina,'') as palazzina,isnull(scala,'') as scala, isnull(piano,'') as piano,isnull(interno,'') as interno  from entisedi "
            StrSql = StrSql & " where identesede <> " & intIdEnteSede
            rstSede = ClsServer.CreaDatareader(StrSql, Session("conn"))
            If rstSede.HasRows = False Then
                rstSede.Read()
                If (IsDBNull(rstSede("Palazzina")) Or rstSede("Palazzina") = strPalazzina) Then
                    Return False
                ElseIf (IsDBNull(rstSede("scala")) Or rstSede("scala") = strScala) Then
                    Return False
                ElseIf (IsDBNull(rstSede("piano")) Or rstSede("piano") = intPiano) Then
                    Return False
                ElseIf (IsDBNull(rstSede("interno")) Or rstSede("interno") = strInterno) Then
                    Return False
                End If
            End If
        End If
        If Not rstSede Is Nothing Then
            rstSede.Close()
            rstSede = Nothing
        End If
    End Function

    Private Sub dgRisultatoRicerca_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgRisultatoRicerca.SelectedIndexChanged
        Context.Items.Add("idente", lblidEnte.Value)
        Context.Items.Add("Ente", lblEnte.Text)
        Context.Items.Add("idEnteSede", txtidsede.Value)
        Context.Items.Add("SedeEnte", txtdenominazione.Text)
        Context.Items.Add("acquisita", lblacquisita.Value)
        Context.Items.Add("inclusa", dgRisultatoRicerca.SelectedItem.Cells(7).Text)
        Context.Items.Add("EntePadre", dgRisultatoRicerca.SelectedItem.Cells(8).Text)
        Context.Items.Add("idSedeAttuazione", dgRisultatoRicerca.SelectedItem.Cells(1).Text)
        Context.Items.Add("Personalizza", "Modifica")
        Context.Items.Add("StatoSedeFisica", lblStato.Text)
        Context.Items.Add("strsql", lblsql.Value)
        Context.Items.Add("page", lblpage.Value)
        If idEnteFiglio.Value <> "" Then
            Context.Items.Add("identefiglio", idEnteFiglio.Value)
        End If
        Server.Transfer("WfrmSedeAttuazione.aspx?VengoDaProgetti=" & Request.QueryString("VengoDaProgetti"))

    End Sub
    Private Function GestioneMarcatore() As Boolean
        '** Agg. da simona cordella il 21/05/2009 per nuovo accrditamento
        '** Marcatore a 1 se modifico uno dei campi ( INDIRIZZO - NUMERO CIVICO- CAP - COMUNE)
        '** Mod. il 27/09/2013 da Simona Cordella
        '** nel controllo sono stati aggiunti i campo PALAZZINA,SCALA,PIANO,INTERNO, TIPOGIURIDICO E NUM. MAX VOLONTARI
        Dim blnMarcatore As Boolean = False
        If Not dtrGenerico Is Nothing Then
            dtrGenerico.Close()
            dtrGenerico = Nothing
        End If

        strsql = " Select es.Denominazione, ISNULL(es.Indirizzo,'') AS Indirizzo, ISNULL(es.Civico,'') as Civico, ISNULL(es.IDComune,'') as IDComune, ISNULL(es.CAP,'') as CAP,es.marcatore, "
        strsql &= " ISNULL(es.Palazzina,'') as Palazzina, ISNULL(es.Scala,'')as Scala, ISNULL(CONVERT(VARCHAR(5),es.Piano),'') as Piano, ISNULL(es.Interno,'') as Interno, es.IdTitoloGiuridico, esa.NMaxVolontari"
        strsql &= " FROM entisedi es "
        strsql &= " INNER JOIN entisediAttuazioni esa on es.identesede = esa.identesede"
        strsql &= " WHERE es.IdEntesede=" & txtidsede.Value & " and isnull(es.marcatore,0)=0 "
        dtrGenerico = ClsServer.CreaDatareader(strsql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
        If dtrGenerico.HasRows = True Then
            dtrGenerico.Read()
            If dtrGenerico("Marcatore") = False Then
                ''txtdenominazione.Text <> dtrGenerico("Denominazione") _ Or
                If (txtIndirizzo.Text <> dtrGenerico("Indirizzo") _
                        Or txtCivico.Text <> dtrGenerico("Civico") _
                        Or txtCap.Text <> dtrGenerico("CAP") _
                        Or ddlComune.SelectedItem.Value <> dtrGenerico("IDComune") _
                        Or TxtPalazzina.Text <> dtrGenerico("Palazzina") _
                        Or TxtPiano.Text <> dtrGenerico("Piano") _
                        Or TxtInterno.Text <> dtrGenerico("Interno") _
                        Or TxtScala.Text <> dtrGenerico("Scala") _
                        Or ddlTitoloGiuridico.SelectedValue <> dtrGenerico("IdTitoloGiuridico") _
                        Or TxtNumMaxVol.Text <> dtrGenerico("NMaxVolontari")) Then
                    blnMarcatore = True
                End If
            Else
                blnMarcatore = False
            End If
        End If
        If Not dtrGenerico Is Nothing Then
            dtrGenerico.Close()
            dtrGenerico = Nothing
        End If
        Return blnMarcatore
    End Function
    Private Function ControlloSede() As Boolean
        'funzione che controlla se la sede è sede di progetto
        'se è sede di progetto(TRUE) imposto la sede a SOSPESA = 3
        'altrimenti la sede è CANCELLATA =2
        Dim dtrgenerico As Data.SqlClient.SqlDataReader

        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
        'Controllo se sono presenti progetti sulle sedi dell'ente 
        strsql = "select attività.IdAttività from attivitàEntiSediAttuazione " &
        " inner join entisediattuazioni on entisediattuazioni.identesedeattuazione=attivitàEntiSediAttuazione.identesedeattuazione " &
        " inner join attività on (attivitàEntiSediAttuazione.idattività=attività.idattività)" &
        " inner join statiattività on (attività.idstatoattività=statiattività.idstatoattività)" &
        " inner join entisedi on entisediattuazioni.identesede = entisedi.identesede " &
        " where entisediattuazioni.identesede=" & txtCodice.Value & " and (statiattività.Attiva=1 or  statiattività.DaGraduare=1 or statiattività.DaValutare=1 or  statiattività.DefaultStato=1)"
        dtrgenerico = ClsServer.CreaDatareader(strsql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
        If dtrgenerico.HasRows = True Then
            'Ci sono dei progetti, SOSPENDO la sede
            If Not dtrgenerico Is Nothing Then
                dtrgenerico.Close()
                dtrgenerico = Nothing
            End If
            Return True
        Else
            'Non ci sono progetti, CANCELLO la sede
            If Not dtrgenerico Is Nothing Then
                dtrgenerico.Close()
                dtrgenerico = Nothing
            End If
            Return False
        End If

    End Function

    Private Function LeggiStoreVerificaSediPartner(ByVal IDEnte As Integer, ByVal NAggiunto As Integer) As String
        'Agg. da Simona Cordella il 05/06/2009
        'richiamo store che controlla se l'ente può inserire altre sedi
        Dim intValore As Integer

        Dim CustOrderHist As SqlClient.SqlCommand
        CustOrderHist = New SqlClient.SqlCommand
        CustOrderHist.CommandType = CommandType.StoredProcedure
        CustOrderHist.CommandText = "SP_VERIFICA_SEDI_PARTNER"
        CustOrderHist.Connection = IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn"))

        Dim sparam As SqlClient.SqlParameter
        sparam = New SqlClient.SqlParameter
        sparam.ParameterName = "@IdEnte"
        sparam.SqlDbType = SqlDbType.Int
        CustOrderHist.Parameters.Add(sparam)


        Dim sparam1 As SqlClient.SqlParameter
        sparam1 = New SqlClient.SqlParameter
        sparam1.ParameterName = "@NAggiunto"
        sparam1.SqlDbType = SqlDbType.Int
        CustOrderHist.Parameters.Add(sparam1)

        Dim sparam2 As SqlClient.SqlParameter
        sparam2 = New SqlClient.SqlParameter
        sparam2.ParameterName = "@Valore"
        sparam2.SqlDbType = SqlDbType.Int
        sparam2.Direction = ParameterDirection.Output
        CustOrderHist.Parameters.Add(sparam2)

        Dim sparam3 As SqlClient.SqlParameter
        sparam3 = New SqlClient.SqlParameter
        sparam3.ParameterName = "@Causale"
        sparam3.SqlDbType = SqlDbType.VarChar
        sparam3.Size = 200
        sparam3.Direction = ParameterDirection.Output
        CustOrderHist.Parameters.Add(sparam3)

        Dim Reader As SqlClient.SqlDataReader
        CustOrderHist.Parameters("@IdEnte").Value = IDEnte
        CustOrderHist.Parameters("@NAggiunto").Value = NAggiunto
        Reader = CustOrderHist.ExecuteReader()
        ' Insert code to read through the datareader.
        'If CustOrderHist.Parameters("@Valore").Value = 0 Then
        intValore = CustOrderHist.Parameters("@Valore").Value
        LeggiStoreVerificaSediPartner = CustOrderHist.Parameters("@Causale").Value

        If Not Reader Is Nothing Then
            Reader.Close()
            Reader = Nothing
        End If
    End Function
    Private Function TrovaFlagMaxVolontari(ByVal IDEnteSede As Integer) As Integer
        'Creata il 09/06/2009 da simona cordella
        'funzione che ritorna un vaolore 0 o 1 (valore del flagMaxVolontari tabella EntiSediAttuaione)
        Dim strsql As String
        Dim rstMax As SqlClient.SqlDataReader
        If Not rstMax Is Nothing Then
            rstMax.Close()
            rstMax = Nothing
        End If

        strsql = " Select FlagMaxVolontari From entisediattuazioni where identeSede = " & IDEnteSede
        rstMax = ClsServer.CreaDatareader(strsql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
        If rstMax.HasRows = True Then
            rstMax.Read()
            If "" & rstMax("FlagMaxVolontari") = "" Then
                TrovaFlagMaxVolontari = 2
            Else

                If rstMax("FlagMaxVolontari") = True Then
                    TrovaFlagMaxVolontari = 1
                Else
                    TrovaFlagMaxVolontari = 0
                End If
            End If
        End If
        If Not rstMax Is Nothing Then
            rstMax.Close()
            rstMax = Nothing
        End If
        Return TrovaFlagMaxVolontari
    End Function

    Private Function ControlloObbligoLocalizzazioneEntiAmbitoTerritoriale() As Boolean
        'Creata da Simona Cordella il 10/06/2009
        'Verifica la tipologia dell'ente padre o figlio
        'Se la tipologia dell'ente prevede OBBLIGOLOCALIZZAZIONE =TRUE 
        'possono inserire o modificare sedi, solo nei comuni che sono stati indicati nella tabella ENTIAMBITOTERRITORIALE

        'RITORNA TRUE se la sede appartiene al comune indicato con OBBLIGOLOCALIZZAZIONE = TRUE oppure se la tipologia dell'ente nn prevede l'obbligolocalizzazione
        'RITORNA FALSE se la tipologia dell'ente prevede OBBLIGOLOCALIZZAZIONE = TRUE ma il comune  non è stato indicato nella tabella ENTIAMBITOTERRITORIALE
        Dim dtrTerritorio As SqlClient.SqlDataReader
        Dim StrSql As String

        If Not dtrTerritorio Is Nothing Then
            dtrTerritorio.Close()
            dtrTerritorio = Nothing
        End If

        ControlloObbligoLocalizzazioneEntiAmbitoTerritoriale = True

        'Ricavo la tipologia dell'Ente e flag ObbligoLocalizzazione
        StrSql = "Select enti.Tipologia,TipologieEnti.ObbligoLocalizzazione "
        StrSql = StrSql & " FROM enti "
        StrSql = StrSql & " INNER JOIN TipologieEnti ON enti.Tipologia = TipologieEnti.Descrizione"
        StrSql = StrSql & " WHERE "
        If ControllaEnteFiglio(ddlEntiFigli.SelectedValue) = False Then
            'Verifica della Univocità del codiceEnteSede per Ente Padre
            StrSql = StrSql & " idEnte = " & lblidEnte.Value & ""
        Else
            'Verifica della Univocità del codiceEnteSede per Ente Figlio
            StrSql = StrSql & " idEnte = " & ddlEntiFigli.SelectedValue & ""
        End If
        dtrTerritorio = ClsServer.CreaDatareader(StrSql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
        If dtrTerritorio.HasRows = False Then
            ControlloObbligoLocalizzazioneEntiAmbitoTerritoriale = True
        Else
            dtrTerritorio.Read()
            If dtrTerritorio("ObbligoLocalizzazione") = True Then
                If Not dtrTerritorio Is Nothing Then
                    dtrTerritorio.Close()
                    dtrTerritorio = Nothing
                End If
                'verifico se il comune della sede che è stato indicato in inserimento/modifica è presente nella tabella EntiAmbitoTerritoriale
                StrSql = "Select IdComune,IDEnte from EntiAmbitoTerritoriale  WHERE "
                'If Request.Form("txtIDComune") = "" Then
                StrSql = StrSql & " idComune ='" & ddlComune.SelectedItem.Value & "'"
                'Else
                '    StrSql = StrSql & " idComune ='" & Request.Form("txtIDComune") & "'"
                'End If
                StrSql = StrSql & " AND "
                If ControllaEnteFiglio(ddlEntiFigli.SelectedValue) = False Then
                    'Verifica della Univocità del codiceEnteSede per Ente Padre
                    StrSql = StrSql & " idEnte = " & lblidEnte.Value & ""
                Else
                    'Verifica della Univocità del codiceEnteSede per Ente Figlio
                    StrSql = StrSql & " idEnte = " & ddlEntiFigli.SelectedValue & ""
                End If
                dtrTerritorio = ClsServer.CreaDatareader(StrSql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
                If dtrTerritorio.HasRows = False Then
                    'comune nn presente in tabella
                    ControlloObbligoLocalizzazioneEntiAmbitoTerritoriale = False
                Else
                    'comune presente in tabella
                    ControlloObbligoLocalizzazioneEntiAmbitoTerritoriale = True
                End If
            End If
        End If
        If Not dtrTerritorio Is Nothing Then
            dtrTerritorio.Close()
            dtrTerritorio = Nothing
        End If
        Return ControlloObbligoLocalizzazioneEntiAmbitoTerritoriale

    End Function
    Private Function ControlloDoppioneSedeNuova(ByVal IdEnte As Integer, ByVal Indirizzo As String, ByVal Civico As String, ByVal IdComune As Integer, ByVal Palazzina As String, ByVal Scala As String, ByVal Piano As String, ByVal Interno As String) As Boolean
        'Aggiunto da simona Cordella il 16/06/2009
        'Richimo funzione sql che controlla se i dati in input per l'ente sono già presenti nel db
        'la funzione sql ritorna 
        '       0 : se non esistono 
        '       1 : se esistono del db
        'la funzione ControlloDoppioneSedeNuova ritorna 
        '       FALSE : se posso inserire la sede
        '       TRUE  : se non posso continuare l'inserimento(invio messaggio bloccante)

        Dim strSql As String
        Dim dtrNuovaSede As SqlClient.SqlDataReader

        strSql = " Select dbo.DoppioneNuovaSede(" & IdEnte & ",'" & Indirizzo.Replace("'", "''") & "', "
        strSql = strSql & " '" & Civico.Replace("'", "''") & "'," & IdComune & ",'" & Palazzina.Replace("'", "''") & "', "
        strSql = strSql & " '" & Scala.Replace("'", "''") & "','" & Piano.Replace("'", "''") & "','" & Interno & "') as DoppioneNuovaSede"

        If Not dtrNuovaSede Is Nothing Then
            dtrNuovaSede.Close()
            dtrNuovaSede = Nothing
        End If
        dtrNuovaSede = ClsServer.CreaDatareader(strSql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
        If dtrNuovaSede.HasRows = True Then
            dtrNuovaSede.Read()
            If dtrNuovaSede("DoppioneNuovaSede") = 1 Then
                ControlloDoppioneSedeNuova = True
            Else
                ControlloDoppioneSedeNuova = False
            End If

        End If
        If Not dtrNuovaSede Is Nothing Then
            dtrNuovaSede.Close()
            dtrNuovaSede = Nothing
        End If
        Return ControlloDoppioneSedeNuova
    End Function
    Private Function ControlloDoppioneSedeNuovaGlobale(ByVal IdEnteSede As Integer, ByVal Indirizzo As String, ByVal Civico As String, ByVal IdComune As Integer, ByVal Palazzina As String, ByVal Scala As String, ByVal Piano As String, ByVal Interno As String) As Boolean
        'Aggiunto da Antonello il 10/01/2014
        'Richimo funzione sql che controlla se i dati in input per Sede sono già presenti nel db
        'la funzione sql ritorna 
        '       0 : se non esistono 
        '       1 : se esistono del db
        'la funzione ControlloDoppioneSedeNuovaGlobale ritorna 
        '       FALSE : se posso modificare la sede
        '       TRUE  : se non posso continuare la modifica(invio messaggio bloccante)

        Dim strSql As String
        Dim dtrNuovaSedeGlobale As SqlClient.SqlDataReader

        strSql = " Select dbo.DoppioneNuovaSedeGlobale(" & IdEnteSede & ",'" & Indirizzo.Replace("'", "''") & "', "
        strSql = strSql & " '" & Civico.Replace("'", "''") & "'," & IdComune & ",'" & Palazzina.Replace("'", "''") & "', "
        strSql = strSql & " '" & Scala.Replace("'", "''") & "','" & Piano.Replace("'", "''") & "','" & Interno & "') as DoppioneNuovaSedeGlobale"

        If Not dtrNuovaSedeGlobale Is Nothing Then
            dtrNuovaSedeGlobale.Close()
            dtrNuovaSedeGlobale = Nothing
        End If
        dtrNuovaSedeGlobale = ClsServer.CreaDatareader(strSql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
        If dtrNuovaSedeGlobale.HasRows = True Then
            dtrNuovaSedeGlobale.Read()
            If dtrNuovaSedeGlobale("DoppioneNuovaSedeGlobale") = 1 Then
                ControlloDoppioneSedeNuovaGlobale = True
            Else
                ControlloDoppioneSedeNuovaGlobale = False
            End If

        End If
        If Not dtrNuovaSedeGlobale Is Nothing Then
            dtrNuovaSedeGlobale.Close()
            dtrNuovaSedeGlobale = Nothing
        End If
        Return ControlloDoppioneSedeNuovaGlobale
    End Function
    Private Sub CaricaDatiEntiSediAttuazione(ByVal IDEnteSede As Integer)
        If Not dtrGenerico Is Nothing Then
            dtrGenerico.Close()
            dtrGenerico = Nothing
        End If
        'AGG DA SIMONA CORDELLA IL 19/05/2009 per nuovo accreditamento luglio 2009
        'visualizzo campi da  entisediattuazioni (NMaxVolontari,certificazione,datacertificazione ,usercertificazione,note)
        'verifico se la sede attuazione ha l'inidirizzo duplicato
        dtrGenerico = ClsServer.CreaDatareader("select NMaxVolontari,dbo.DoppioneSede(IdEnteSedeAttuazione) as DoppioneSede," &
                        " Isnull(Entisediattuazioni.Certificazione,2) as Certificazione, Isnull(dbo.FormatoData(DataCertificazione),'') as DataCertificazione, Isnull(Entisediattuazioni.UserCertificazione,'') as UserCertificazione,Note,Isnull(Entisediattuazioni.Segnalazione,0) as Segnalazione " &
                        " From entisediattuazioni " &
                        " where identeSede=" & IDEnteSede & "", IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
        If dtrGenerico.HasRows = True Then
            dtrGenerico.Read()
            TxtNumMaxVol.Text = "" & dtrGenerico("NMaxVolontari")

            'agg da simona cordella il 30/05/2009
            'visualizzazione e gestione del flag certificazione
            ddlCertificazione.SelectedValue = dtrGenerico("Certificazione")
            Session("intStatoPrecCertificazione") = dtrGenerico("Certificazione")
            TxtUserCert.Text = dtrGenerico("UserCertificazione")
            TxtDataCert.Text = dtrGenerico("DataCertificazione")
            txtNote.Text = "" & dtrGenerico("Note")
            If dtrGenerico("DoppioneSede") = 1 Then
                msgErrore.Text = "Attenzione. L'indirizzo indicato risulta utilizzato da altra Sede."
            End If

            If dtrGenerico("Segnalazione") = True Then
                If Not dtrGenerico Is Nothing Then
                    dtrGenerico.Close()
                    dtrGenerico = Nothing
                End If
                If ClsUtility.ForzaPresenzaSanzione(Session("Utente"), Session("conn")) = True Then
                    lblSanzione.Visible = True
                    lblSanzione.Text = "Sede Sanzionata"
                    imgSanzione.Visible = True
                    divSedeSanzionata.Visible = True
                End If
                If ClsUtility.ForzaRipristinoSanzione(Session("Utente"), Session("conn")) = True Then
                    imgRipristinaSanzione.Visible = True
                End If
            Else
                lblSanzione.Visible = False
                imgSanzione.Visible = False
                imgRipristinaSanzione.Visible = False
                divSedeSanzionata.Visible = False
            End If
        End If
        If Not dtrGenerico Is Nothing Then
            dtrGenerico.Close()
            dtrGenerico = Nothing
        End If
    End Sub
    Private Sub CaricaDatiEntiSediAttuazioneVariazione(ByVal IDEnteSede As Integer)
        If Not dtrGenerico Is Nothing Then
            dtrGenerico.Close()
            dtrGenerico = Nothing
        End If
        'AGG DA SIMONA CORDELLA IL 19/05/2009 per nuovo accreditamento luglio 2009
        'visualizzo campi da  entisediattuazioni (NMaxVolontari,certificazione,datacertificazione ,usercertificazione,note)
        'verifico se la sede attuazione ha l'inidirizzo duplicato
        strsql = "select entisediattuazioni.NMaxVolontari,dbo.DoppioneSede(IdEnteSedeAttuazione) as DoppioneSede, " &
                " Isnull(Entisediattuazioni.Certificazione,2) as Certificazione, " &
                " Isnull(dbo.FormatoData(entisediattuazioni.DataCertificazione),'') as DataCertificazione," &
                " Isnull(Entisediattuazioni.UserCertificazione,'') as UserCertificazione,entisediattuazioni.Note," &
                " Isnull(Entisediattuazioni.RipristinoSegnalazione,0) as Segnalazione " &
                " From Accreditamento_VariazioneSedi entisediattuazioni " &
                " INNER JOIN entisediattuazioni esa on esa.IdEnteSede = entisediattuazioni.IdEnteSede " &
                " WHERE entisediattuazioni.identeSede= " & IDEnteSede & " and entisediattuazioni.StatoVariazione =0 "
        dtrGenerico = ClsServer.CreaDatareader(strsql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
        If dtrGenerico.HasRows = True Then
            dtrGenerico.Read()
            TxtNumMaxVol.Text = "" & dtrGenerico("NMaxVolontari")

            'agg da simona cordella il 30/05/2009
            'visualizzazione e gestione del flag certificazione
            ddlCertificazione.SelectedValue = dtrGenerico("Certificazione")
            Session("intStatoPrecCertificazione") = dtrGenerico("Certificazione")
            TxtUserCert.Text = dtrGenerico("UserCertificazione")
            TxtDataCert.Text = dtrGenerico("DataCertificazione")
            txtNote.Text = "" & dtrGenerico("Note")
            If dtrGenerico("DoppioneSede") = 1 Then
                msgErrore.Text = "Attenzione. L'indirizzo indicato risulta utilizzato da altra Sede."
            End If

            If UCase(dtrGenerico("Segnalazione")) = "SI" Then
                If Not dtrGenerico Is Nothing Then
                    dtrGenerico.Close()
                    dtrGenerico = Nothing
                End If
                If ClsUtility.ForzaPresenzaSanzione(Session("Utente"), Session("conn")) = True Then
                    lblSanzione.Visible = True
                    lblSanzione.Text = "Sede Sanzionata"
                    imgSanzione.Visible = True
                    divSedeSanzionata.Visible = True
                End If
                If ClsUtility.ForzaRipristinoSanzione(Session("Utente"), Session("conn")) = True Then
                    imgRipristinaSanzione.Visible = True
                End If
            Else
                lblSanzione.Visible = False
                imgSanzione.Visible = False
                imgRipristinaSanzione.Visible = False
                divSedeSanzionata.Visible = False
            End If
        End If
        If Not dtrGenerico Is Nothing Then
            dtrGenerico.Close()
            dtrGenerico = Nothing
        End If
    End Sub
    Private Sub ddlTitoloGiuridico_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlTitoloGiuridico.SelectedIndexChanged
        Dim blnObbligoScadenza As Boolean
        If AlboEnte = "SCN" Then
            If ddlTitoloGiuridico.SelectedItem.Text = "Altro" Then
                txtAltroTitoloGiuridicio.ReadOnly = False
            Else
                txtAltroTitoloGiuridicio.Text = ""
                txtAltroTitoloGiuridicio.ReadOnly = True
            End If
        Else

            ' blnObbligoScadenza = TitoloGiuridico_ObbligoScandeza(ddlTitoloGiuridico.SelectedValue)

        End If

    End Sub
    'Private Function TitoloGiuridico_ObbligoScandeza(ByVal IdTitoloGiuridico As Integer) As Boolean
    '    Dim blnObbligoScadenza As Boolean
    '    ChiudiDataReader(dtrGenerico)
    '    strsql = "Select obbligoScadenza FROM TitoliGiuridici where IdTitoloGiuridico =" & IdTitoloGiuridico
    '    dtrGenerico = ClsServer.CreaDatareader(strsql, Session("Conn"))
    '    dtrGenerico.Read()
    '    blnObbligoScadenza = dtrGenerico("obbligoScadenza")
    '    ChiudiDataReader(dtrGenerico)
    '    Return blnObbligoScadenza
    'End Function

    Private Function ControlloComuneEstero() As Boolean
        'Agg da Simona Cordella
        ''controllo se la sede è di tipo estero
        'FALSE : SEDE IN ITALIA
        'TRUE  : SEDE ESTERO
        ControlloComuneEstero = True

        strsql = "Select * from comuni where comunenazionale=1 and  "
        'If Request.Form("txtIDComune") = "" Then
        strsql = strsql & " idComune ='" & ddlComune.SelectedItem.Value & "'"
        'Else
        '    strsql = strsql & " idComune ='" & Request.Form("txtIDComune") & "'"
        'End If
        If Not dtrGenerico Is Nothing Then
            dtrGenerico.Close()
            dtrGenerico = Nothing
        End If
        dtrGenerico = ClsServer.CreaDatareader(strsql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
        If dtrGenerico.HasRows = True Then
            ControlloComuneEstero = False
        End If
        If Not dtrGenerico Is Nothing Then
            dtrGenerico.Close()
            dtrGenerico = Nothing
        End If
        Return ControlloComuneEstero
    End Function

    Private Sub imgSanzione_Click(ByVal sender As Object, ByVal e As EventArgs) Handles imgSanzione.Click
        'agg il 12/07/2011 da simona cordella
        'richiamo popup per la visualizzazione della tipologia di sanzione applicata alla sede
        IDESA = dgRisultatoRicerca.Items(0).Cells(1).Text()

        Response.Write("<script>" & vbCrLf)
        Response.Write("window.open(""WfrmSedeSanzionata.aspx?IdEnteSedeAttuazione=" & IDESA & "&IdEnte=" & Session("IdEnte") & """, ""SedeSanzionata"", ""width=950, height=600, toolbar=no, location=no, menubar=no, scrollbars=yes"")" & vbCrLf)
        Response.Write("</script>")
        'window.open("WfrmSedeSanzionata.aspx?IDEnteSedeAttuazione=" + IDEnteSedeAttuazione + "&IdEnte=" + IdEnte	, "SedeSanzionata", "width=950, height=600, toolbar=no, location=no, menubar=no, scrollbars=yes");
    End Sub

    Private Sub imgRipristinaSanzione_Click(ByVal sender As Object, ByVal e As EventArgs) Handles imgRipristinaSanzione.Click
        'agg il 12/07/2011 da simona cordella
        'imposto la variabile a TRUE che serve per ll salvataggio dei dati in ENTISEDIATTUAZIONI
        txtRipristinoSanzione.Value = "SI"
    End Sub

    Private Function VerificaPresenzaSedesuProgettiAttivi_InValutazione(ByVal IdSede As Integer) As Boolean
        '*** Creata il 26/09/2013 da Simona Cordella
        '*** Verifico se la sede Accreditata che si vuole modificare nn sia utilizzata su Progetti Attivi ancora in corso,in attesa di graduatoria e in attesa di avvio(proposto)
        '*** non è possibile apportare modifiche alla stessa

        Dim strSql As String
        Dim dtrSede As SqlClient.SqlDataReader

        strSql = " SELECT  dbo.formatodata(attività.DataFineAttività),attività.idattività, dbo.formatodata(getdate()) "
        strSql &= " FROM entisedi "
        strSql &= " INNER Join entisediattuazioni ON entisedi.IDEnteSede = entisediattuazioni.IDEnteSede "
        strSql &= " INNER JOIN attivitàentisediattuazione ON entisediattuazioni.IDEnteSedeAttuazione = attivitàentisediattuazione.IDEnteSedeAttuazione "
        strSql &= " INNER JOIN attività ON attivitàentisediattuazione.IDAttività = attività.IDAttività"
        strSql &= " WHERE entisedi.IDEnteSede = " & IdSede & " AND (attività.IDStatoAttività in(1,4,9))"
        strSql &= " AND  isnull(attività.DataFineAttività,getdate()) >= getdate()"
        dtrSede = ClsServer.CreaDatareader(strSql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))

        VerificaPresenzaSedesuProgettiAttivi_InValutazione = dtrSede.HasRows

        If Not dtrSede Is Nothing Then
            dtrSede.Close()
            dtrSede = Nothing
        End If
    End Function
    Private Sub Accesso_Maschera_Sede(ByVal TipoUtente As String, ByVal IdEnte As Integer, ByVal IdEnteSede As Integer, ByRef abilitato As Integer, ByRef dati As Integer, ByRef annullamodifica As Integer, ByRef annullacancellazione As Integer, ByRef visualizzadatiaccreditati As Integer, ByRef messaggio As String)

        'REALIZZATA DA: SIMONA CORDELLA 
        'DATA REALIZZAZIONE:  29/07/2014
        'FUNZIONALITA':  Verifica le condizioni di accreditamento e le eventuali modifiche in corso sui dati della sede
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
            SqlCmd.CommandText = "SP_ACCREDITAMENTO_ACCESSOMASCHERA_SEDE"
            SqlCmd.CommandType = CommandType.StoredProcedure
            SqlCmd.Connection = Session("Conn")
            SqlCmd.Parameters.Add("@TipoUtente", SqlDbType.VarChar).Value = TipoUtente
            SqlCmd.Parameters.Add("@IdEnte", SqlDbType.Int).Value = IdEnte
            SqlCmd.Parameters.Add("@IdEnteSede", SqlDbType.Int).Value = IdEnteSede

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
            'msgInfo.Text = SqlCmd.Parameters("@messaggio").Value
        Catch ex As Exception
            msgErrore.Text = ex.Message
        End Try

    End Sub
    Sub PersonalizzoNoVisibilitaCampi()


        'personalizzazione della maschera in base alla tipologia di Utente 
        lblTipoUtente.Value = Session("TipoUtente")
        'agg da simona cordella il 29/05/2009
        'visualizzazione e gestione del flag certificazione parte in "Da Valutare"
        ddlCertificazione.SelectedValue = 2

        If (Session("TipoUtente") = "U" Or Session("TipoUtente") = "R") Then
            lblDataControlloHttp.Visible = True
            lbldataControlloEmail.Visible = True
            chkFalsohttp.Visible = True
            chkVerohttp.Visible = True
            chkFalsoEmail.Visible = True
            chkVeroEmail.Visible = True
            cmdHttp.Visible = True
            cmdEmail.Visible = True
        Else
            'lblDataControlloHttp.Visible = False
            'lbldataControlloEmail.Visible = False
            chkFalsohttp.Visible = False
            chkVerohttp.Visible = False
            chkFalsoEmail.Visible = False
            chkVeroEmail.Visible = False
            cmdHttp.Visible = False
            cmdEmail.Visible = False
            '** agg da sc 
            checkHttp.Visible = False
            checkEmail.Visible = False
            '**
        End If
        If lblPersonalizza.Value <> "Modifica" Then
            If (Not IsNothing(Request.QueryString("tipoazione"))) And (Context.Items("tipoazione") Is Nothing) Then
                lblPersonalizza.Value = Request.QueryString("tipoazione")
            Else
                lblPersonalizza.Value = Context.Items("tipoazione")
            End If
        End If
        If Not IsNothing(Request.QueryString("acquisita")) Then
            lblacquisita.Value = Request.QueryString("acquisita")
            If lblacquisita.Value <> "propria" Then
                chkRiferimentoRimborsi.Visible = False
            End If
        End If
        If Not IsNothing(Request.QueryString("stato")) Then
            'mod. il 05/03/2008 da s.c.
            'riporta lo stato della sede attuazione e non della sede fisica
            lblStato.Text = Request.QueryString("stato")
        End If

        ChiudiDataReader(dtrGenerico)
        lblEnte.Text = Session("Denominazione")
        'Inserimento
        If lblPersonalizza.Value = "Inserimento" Then
            Session("IdComune") = Nothing
            ddlTipologia.Enabled = False
            lblCodiceSede.Visible = False
            txtCodice.Visible = False
            ddlTipologia.AutoPostBack = False

            labelStato.Visible = False
            lblStato.Text = "Inserimento"
            lblInfoProgetti.Visible = False
            If Not dtrGenerico Is Nothing Then
                dtrGenerico.Close()
                dtrGenerico = Nothing
            End If
            PersonalizzaTasti()
        Else 'Modifica
            'Modificato da Alessandra Taballione il 07.06.2005
            'lblCodiceSede.Visible = True
            'txtCodice.Visible = True
            ddlTipologia.Enabled = False
            ' lblEntefiglio.Visible = False
            ' ddlEntiFigli.Visible = False
            'imgOpenFigli.Visible = False

            If lblidEnte.Value <> Session("IdEnte").ToString Then
                idEnteFiglio.Value = lblidEnte.Value
            End If
        End If
    End Sub
    Sub CaricaCombo(ByVal AlboEnte As String)
        ChiudiDataReader(dtrGenerico)
        dtrGenerico = ClsServer.CreaDatareader("Select tiposede,idtiposede from tipiSedi order by idtiposede desc ", IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
        ddlTipologia.DataSource = dtrGenerico
        ddlTipologia.DataTextField = "TipoSede"
        ddlTipologia.DataValueField = "idTipoSede"
        ddlTipologia.DataBind()
        ddlTipologia.SelectedIndex = 0
        ChiudiDataReader(dtrGenerico)
        CaricoComboTitoloGiuridico(AlboEnte)


    End Sub
    Sub PopolaMaschera(ByVal IdEnteSede As Integer)
        ChiudiDataReader(dtrGenerico)
        Dim idProvincia As String
        Dim idComune As String
        Dim ObbligoScadenza As Boolean

        'popolamento della maschera dati DB
        dtrGenerico = ClsServer.CreaDatareader("select day(entisedi.datacontrolloemail)as ggDCEmail,month(entisedi.datacontrolloemail)as " &
                    " monthDCEmail,year(entisedi.datacontrolloemail)as yearDCEmail," &
                    " day(entisedi.datacontrollohttp)as ggDChttp,month(entisedi.datacontrollohttp)as monthDChttp," &
                    " year(entisedi.datacontrollohttp)as yearDChttp," &
                    " entisediTipi.idtiposede,comuni.idComune,comuni.denominazione as Comune,provincie.provincia,provincie.idprovincia,provincie.ProvinceNazionali ,StatiEntiSedi.statoentesede,* from entisedi " &
                    " inner join StatiEntiSedi on (entisedi.IdStatoEnteSede=dbo.StatiEntiSedi.IdStatoEnteSede) " &
                    " inner join comuni on (comuni.idcomune=entisedi.idcomune) " &
                    " inner join provincie on (provincie.idprovincia=comuni.idprovincia)" &
                    " left join entisediTipi on (entisediTipi.identesede=entisedi.idEntesede)" &
                    " where entisedi.identeSede=" & IdEnteSede & "", IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
        dtrGenerico.Read()
        For Each Itm As ListItem In ddlEntiFigli.Items
            If Itm.Value = IDEnteProprietario.Value Then
                ddlEntiFigli.ClearSelection()
                Itm.Selected = True
            End If
        Next
        If dtrGenerico.HasRows = True Then

            'aggiunto il 05/03/2008 da s.c.
            ChkEstero.Checked = Not CBool(dtrGenerico("ProvinceNazionali"))
            lblStato.Text = "" & dtrGenerico("statoentesede")
            If Not IsDBNull(dtrGenerico("denominazione")) Then
                txtdenominazione.Text = dtrGenerico("denominazione")
                txtidsede.Value = dtrGenerico("idEnteSede")
                'lblEnte.Text = dtrGenerico("denominazione")
            End If
            If Not IsDBNull(dtrGenerico("identesede")) Then
                txtCodice.Value = dtrGenerico("identesede")
            End If
            If Not IsDBNull(dtrGenerico("indirizzo")) Then
                txtIndirizzo.Text = dtrGenerico("indirizzo")
            End If
            If Not IsDBNull(dtrGenerico("DettaglioRecapito")) Then
                TxtDettaglioRecapito.Text = dtrGenerico("DettaglioRecapito")
            End If

            '**** agg. da simona cordella il 19/05/2009 per nuovo accreditamento luglio 2009
            'palazzina
            If Not IsDBNull(dtrGenerico("Palazzina")) Then
                TxtPalazzina.Text = dtrGenerico("Palazzina")
            End If
            'Scala
            If Not IsDBNull(dtrGenerico("Scala")) Then
                TxtScala.Text = dtrGenerico("Scala")
            End If
            'Piano
            If Not IsDBNull(dtrGenerico("Piano")) Then
                TxtPiano.Text = dtrGenerico("Piano")
            End If
            ' Interno
            If Not IsDBNull(dtrGenerico("Interno")) Then
                TxtInterno.Text = dtrGenerico("Interno")
            End If
            'IdTitoloGiuridico
            If Not IsDBNull(dtrGenerico("IdTitoloGiuridico")) Then
                ddlTitoloGiuridico.SelectedValue = dtrGenerico("IdTitoloGiuridico")
            End If
            'AltroTitoloGiuridico -- agg. il 03/07/2009 da s.c.
            If Not IsDBNull(dtrGenerico("AltroTitoloGiuridico")) Then
                txtAltroTitoloGiuridicio.ReadOnly = False
                txtAltroTitoloGiuridicio.Text = dtrGenerico("AltroTitoloGiuridico")
            End If

            '*******************

            If Not IsDBNull(dtrGenerico("Civico")) Then
                txtCivico.Text = dtrGenerico("Civico")
            End If
            If Not IsDBNull(dtrGenerico("Cap")) Then
                txtCap.Text = dtrGenerico("Cap")
            End If
            If Not IsDBNull(dtrGenerico("idprovincia")) Then
                idProvincia = dtrGenerico("idprovincia")
            End If
            If Not IsDBNull(dtrGenerico("idcomune")) Then
                idComune = dtrGenerico("idcomune")
            End If

            If Not IsDBNull(dtrGenerico("idtiposede")) Then
                ddlTipologia.SelectedValue = dtrGenerico("idtiposede")
                txtTipologia.Value = dtrGenerico("idtiposede")
            End If
            If Not IsDBNull(dtrGenerico("Telefono")) Then
                txtTelefono.Text = dtrGenerico("Telefono")
            End If
            If Not IsDBNull(dtrGenerico("prefissoTelefono")) Then
                txtprefisso.Text = dtrGenerico("prefissoTelefono")
            End If
            If Not IsDBNull(dtrGenerico("Fax")) Then
                txtfax.Text = dtrGenerico("Fax")
            End If
            If Not IsDBNull(dtrGenerico("prefissoFax")) Then
                txtPrefFax.Text = dtrGenerico("prefissoFax")
            End If
            'If Not IsDBNull(dtrGenerico("datacontrollohttp")) Then
            '    lblDataControlloHttp.Value = dtrGenerico("datacontrollohttp")
            'End If
            If Not IsDBNull(dtrGenerico("Datacontrolloemail")) Then
                lbldataControlloEmail.Value = dtrGenerico("ggDCemail") & "/" & IIf(CInt(dtrGenerico("monthDCemail")) < 10, "0" & dtrGenerico("monthDCemail"), dtrGenerico("monthDCemail")) & "/" & dtrGenerico("YearDCemail")
            End If
            'txtdataControlloEmail.Text = IIf(Not IsDBNull(dtrgenerico("Datacontrolloemail")), dtrgenerico("Datacontrolloemail"), "")
            If Not IsDBNull(dtrGenerico("Datacontrollohttp")) Then
                lblDataControlloHttp.Value = dtrGenerico("ggDChttp") & "/" & IIf(CInt(dtrGenerico("monthDChttp")) < 10, "0" & dtrGenerico("monthDChttp"), dtrGenerico("monthDChttp")) & "/" & dtrGenerico("yearDChttp")
            End If
            If Not IsDBNull(dtrGenerico("http")) Then
                txthttp.Text = dtrGenerico("http")
                cmdHttp.NavigateUrl = lblhttp.Text & dtrGenerico("http")
                If Trim(lblDataControlloHttp.Value) <> "" Then
                    If dtrGenerico("httpvalido") = True Then
                        chkVerohttp.Checked = True
                    Else
                        chkFalsohttp.Checked = True
                    End If
                Else
                    chkVerohttp.Checked = False
                    chkFalsohttp.Checked = False
                End If
            Else
                chkVerohttp.Checked = False
                chkFalsohttp.Checked = False
            End If
            If Not IsDBNull(dtrGenerico("RiferimentoRimborsi")) Then
                If dtrGenerico("RiferimentoRimborsi") = True Then
                    chkRiferimentoRimborsi.Checked = True
                End If
            End If

            'If Not IsDBNull(dtrGenerico("datacontrolloEmail")) Then
            '    lbldataControlloEmail.Value = dtrGenerico("datacontrolloEmail")
            'End If
            If Not IsDBNull(dtrGenerico("Email")) Then
                txtemail.Text = dtrGenerico("Email")
                If Trim(lbldataControlloEmail.Value) <> "" Then
                    If dtrGenerico("EmailValido") = True Then
                        chkVeroEmail.Checked = True
                    Else
                        chkFalsoEmail.Checked = True
                    End If
                Else
                    chkVeroEmail.Checked = False
                    chkFalsoEmail.Checked = False
                End If
            Else
                chkVeroEmail.Checked = False
                chkFalsoEmail.Checked = False
            End If


            If Not IsDBNull(dtrGenerico("NormativaTutela")) Then
                If dtrGenerico("NormativaTutela") = True Then
                    ChkTutela.Checked = True
                Else
                    ChkTutela.Checked = False
                End If
            End If

            If Not IsDBNull(dtrGenerico("SoggettoEstero")) Then
                TxtSoggettoCapoSede.Text = dtrGenerico("SoggettoEstero")
            End If

            If Not IsDBNull(dtrGenerico("NonDisponibilitaSede")) Then
                If dtrGenerico("NonDisponibilitaSede") = True Then
                    rbSE2.Checked = True
                    rbSE1.Checked = False
                Else
                    rbSE2.Checked = False
                    rbSE1.Checked = True
                End If
            End If

            If Not IsDBNull(dtrGenerico("DisponibilitaSede")) Then
                If dtrGenerico("DisponibilitaSede") = True Then
                    rbNoSE1.Checked = False
                    rbNoSE2.Checked = True
                Else
                    rbNoSE1.Checked = True
                    rbNoSE2.Checked = False
                End If
            End If

            If Not IsDBNull(dtrGenerico("IdAllegatoLSE")) Then
                Session("LoadLSEId") = dtrGenerico("IdAllegatoLSE")
            Else
                Session("LoadLSEId") = Nothing
            End If

            If Not IsDBNull(dtrGenerico("Localita")) Then
                txtLocalita.Text = dtrGenerico("Localita")
            Else
                txtLocalita.Text = String.Empty
            End If

            If ChkEstero.Checked = True Then
                lblComune.Text = "Localit&agrave;"
                lblCap.Text = "Codice localit&agrave;"
                If Not IsDBNull(dtrGenerico("CittaEstera")) Then
                    txtCity.Text = dtrGenerico("CittaEstera")
                End If
                city.Visible = True
                ddlComune.Visible = False
                txtLocalita.Visible = True
                DivItalia.Visible = False
                DivEstero2.Visible = True

                If Not IsDBNull(dtrGenerico("IdTitoloGiuridico")) Then
                    If dtrGenerico("IdTitoloGiuridico") = 7 Then
                        DivSoggettoEstero.Visible = True
                        DivNoSoggettoEstero.Visible = False
                    Else
                        DivSoggettoEstero.Visible = False
                        DivNoSoggettoEstero.Visible = True
                    End If
                End If
            Else
                city.Visible = False
                ddlComune.Visible = True
                txtLocalita.Visible = False
                DivItalia.Visible = True
                DivEstero2.Visible = False
            End If
            'If Not dtrGenerico Is Nothing Then
            '    dtrGenerico.Close()
            '    dtrGenerico = Nothing
            'End If
            ' mod. il 05/03/2008 da simona cordella
            ' questo controllo è stato postato per verifica lo stato della sede fisica dopo il caricamento della maschere

            strsql = "Select * from statientisedi where statoentesede='" & Trim(Replace(lblStato.Text, "(*)", "")) & "'"
            ChiudiDataReader(dtrGenerico)
            'If Not dtrGenerico Is Nothing Then
            '    dtrGenerico.Close()
            '    dtrGenerico = Nothing
            'End If
            dtrGenerico = ClsServer.CreaDatareader(strsql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
            dtrGenerico.Read()
            'If lblStato.Text = "Chiusa" Or lblStato.Text = "Sospesa" Then

            'If dtrGenerico("Attiva") = 0 And dtrGenerico("DaAccreditare") = 0 Then

            ' Aggiunto 22/01/2019 Da Luigi Leucci
            Select Case dtrGenerico("IdStatoEnteSede")
                Case 2, 5   ' Cancellata - Richiesta Cancellazione
                   ' AbilitaCampiMaschera(False)
                Case 3      ' Sospesa
                    'If Session("TipoUtente") = "E" Then AbilitaCampiMaschera(False)

                Case 1, 4   ' Accreditata - Presentata

            End Select
            'If dtrGenerico("Attiva") = 0 And dtrGenerico("DaAccreditare") = 0 And Session("TipoUtente") <> "U" And Session("TipoUtente") <> "R" Then
            '    AbilitaCampiMaschera(False)
            'End If
            ChiudiDataReader(dtrGenerico)


            selComune.CaricaProvincie(ddlProvincia, ChkEstero.Checked, Session("Conn"))
            If Not idProvincia Is Nothing Then
                ddlProvincia.SelectedValue = idProvincia
            End If
            ChiudiDataReader(dtrGenerico)
            If Not idComune Is Nothing Then
                selComune.CaricaComuniDaProvincia(ddlComune, idProvincia, Session("Conn"))
                ddlComune.SelectedValue = idComune
                Session("IdComune") = idComune
            End If
            If ChkEstero.Checked = True Then
                city.Visible = True
            Else
                city.Visible = False
            End If

            ChiudiDataReader(dtrGenerico)

            DisabilitaCampo(ddlProvincia)
            DisabilitaCampo(ddlComune)
            'DisabilitaCampo(ddlEntiFigli)
            DisabilitaCampo(txtIndirizzo)
            DisabilitaCampo(txtLocalita)
            DisabilitaCampo(txtCap)
            DisabilitaCampo(txtCivico)
            DisabilitaCampo(TxtPalazzina)
            'DisabilitaCampo(ddlTitoloGiuridico)
            DisabilitaCampo(ChkTutela)
            DisabilitaCampo(TxtDettaglioRecapito)
            DisabilitaCampo(ChkEstero)
            DisabilitaCampo(TxtSoggettoCapoSede)
            DisabilitaCampo(rbSE2)
            DisabilitaCampo(rbSE1)
            DisabilitaCampo(rbNoSE1)
            DisabilitaCampo(rbNoSE2)
            DisabilitaCampo(btnEliminaLSE)
            DisabilitaCampo(btnModificaLSE)
            DisabilitaCampo(txtLocalita)
            DisabilitaCampo(txtCity)
            PersonalizzaTasti()
            ControllaSedeFisicaInclusa()
            RicercaSediAttuazione(IdEnteSede)
            'ControllaSedeFisicaInclusa()
            'ClearSessionLSE()
            CaricaLSE()

        End If
        CaricaDatiEntiSediAttuazione(IdEnteSede)
    End Sub
    Private Sub PopolaMascheraVariazione(ByVal IdEnteSede As Integer)
        ChiudiDataReader(dtrGenerico)
        Dim idProvincia As String
        Dim idComune As String
        Dim ObbligoScadenza As Boolean

        'popolamento della maschera dati DB
        dtrGenerico = ClsServer.CreaDatareader("select day(entisedi.datacontrolloemail)as ggDCEmail,month(entisedi.datacontrolloemail)as  " &
                            " monthDCEmail,year(entisedi.datacontrolloemail)as yearDCEmail, " &
                            " day(entisedi.datacontrollohttp)as ggDChttp,month(entisedi.datacontrollohttp)as monthDChttp, " &
                            " year(entisedi.datacontrollohttp)as yearDChttp, " &
                            "entisediTipi.idtiposede,comuni.idComune,comuni.denominazione as Comune,provincie.provincia,provincie.idprovincia,provincie.ProvinceNazionali ,StatiEntiSedi.statoentesede,* 	 " &
                            "from Accreditamento_VariazioneSedi entisedi  " &
                            "inner join entisedi es on es.identeSede =entisedi.identeSede " &
                            "inner join StatiEntiSedi on (es.IdStatoEnteSede=dbo.StatiEntiSedi.IdStatoEnteSede)  " &
                            "inner join comuni on (comuni.idcomune=entisedi.idcomune)  " &
                            "inner join provincie on (provincie.idprovincia=comuni.idprovincia) " &
                            "left join entisediTipi on (entisediTipi.identesede=entisedi.idEntesede) " &
                            " where entisedi.identeSede=" & IdEnteSede & "  and entiSedi.StatoVariazione =0", IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
        dtrGenerico.Read()
        For Each Itm As ListItem In ddlEntiFigli.Items
            If Itm.Value = IDEnteProprietario.Value Then
                ddlEntiFigli.ClearSelection()
                Itm.Selected = True
            End If
        Next
        If dtrGenerico.HasRows = True Then
            'aggiunto il 05/03/2008 da s.c.
            ChkEstero.Checked = Not CBool(dtrGenerico("ProvinceNazionali"))

            lblStato.Text = "" & dtrGenerico("statoentesede")
            If Not IsDBNull(dtrGenerico("denominazione")) Then
                txtdenominazione.Text = dtrGenerico("denominazione")
                txtidsede.Value = dtrGenerico("idEnteSede")
            End If
            If Not IsDBNull(dtrGenerico("identesede")) Then
                txtCodice.Value = dtrGenerico("identesede")
            End If
            If Not IsDBNull(dtrGenerico("indirizzo")) Then
                txtIndirizzo.Text = dtrGenerico("indirizzo")
            End If
            If Not IsDBNull(dtrGenerico("DettaglioRecapito")) Then
                TxtDettaglioRecapito.Text = dtrGenerico("DettaglioRecapito")
            End If

            '**** agg. da simona cordella il 19/05/2009 per nuovo accreditamento luglio 2009
            'palazzina
            If Not IsDBNull(dtrGenerico("Palazzina")) Then
                TxtPalazzina.Text = dtrGenerico("Palazzina")
            End If
            'Scala
            If Not IsDBNull(dtrGenerico("Scala")) Then
                TxtScala.Text = dtrGenerico("Scala")
            End If
            'Piano
            If Not IsDBNull(dtrGenerico("Piano")) Then
                TxtPiano.Text = dtrGenerico("Piano")
            End If
            ' Interno
            If Not IsDBNull(dtrGenerico("Interno")) Then
                TxtInterno.Text = dtrGenerico("Interno")
            End If
            'IdTitoloGiuridico
            If Not IsDBNull(dtrGenerico("IdTitoloGiuridico")) Then
                ddlTitoloGiuridico.SelectedValue = dtrGenerico("IdTitoloGiuridico")
            End If
            'AltroTitoloGiuridico -- agg. il 03/07/2009 da s.c.
            If Not IsDBNull(dtrGenerico("AltroTitoloGiuridico")) Then
                txtAltroTitoloGiuridicio.ReadOnly = False
                txtAltroTitoloGiuridicio.Text = dtrGenerico("AltroTitoloGiuridico")
            End If

            '*******************

            If Not IsDBNull(dtrGenerico("Civico")) Then
                txtCivico.Text = dtrGenerico("Civico")
            End If
            If Not IsDBNull(dtrGenerico("Cap")) Then
                txtCap.Text = dtrGenerico("Cap")
            End If
            If Not IsDBNull(dtrGenerico("idprovincia")) Then
                idProvincia = dtrGenerico("idprovincia")
            End If
            If Not IsDBNull(dtrGenerico("idcomune")) Then
                idComune = dtrGenerico("idcomune")
            End If
            If Not IsDBNull(dtrGenerico("idtiposede")) Then
                ddlTipologia.SelectedValue = dtrGenerico("idtiposede")
                txtTipologia.Value = dtrGenerico("idtiposede")
            End If
            If Not IsDBNull(dtrGenerico("Telefono")) Then
                txtTelefono.Text = dtrGenerico("Telefono")
            End If
            If Not IsDBNull(dtrGenerico("prefissoTelefono")) Then
                txtprefisso.Text = dtrGenerico("prefissoTelefono")
            End If
            If Not IsDBNull(dtrGenerico("Fax")) Then
                txtfax.Text = dtrGenerico("Fax")
            End If
            If Not IsDBNull(dtrGenerico("prefissoFax")) Then
                txtPrefFax.Text = dtrGenerico("prefissoFax")
            End If

            If Not IsDBNull(dtrGenerico("Datacontrolloemail")) Then
                lbldataControlloEmail.Value = dtrGenerico("ggDCemail") & "/" & IIf(CInt(dtrGenerico("monthDCemail")) < 10, "0" & dtrGenerico("monthDCemail"), dtrGenerico("monthDCemail")) & "/" & dtrGenerico("YearDCemail")
            End If
            If Not IsDBNull(dtrGenerico("Datacontrollohttp")) Then
                lblDataControlloHttp.Value = dtrGenerico("ggDChttp") & "/" & IIf(CInt(dtrGenerico("monthDChttp")) < 10, "0" & dtrGenerico("monthDChttp"), dtrGenerico("monthDChttp")) & "/" & dtrGenerico("yearDChttp")
            End If
            If Not IsDBNull(dtrGenerico("http")) Then
                txthttp.Text = dtrGenerico("http")
                cmdHttp.NavigateUrl = lblhttp.Text & dtrGenerico("http")
                If Trim(lblDataControlloHttp.Value) <> "" Then
                    If dtrGenerico("httpvalido") = True Then
                        chkVerohttp.Checked = True
                    Else
                        chkFalsohttp.Checked = True
                    End If
                Else
                    chkVerohttp.Checked = False
                    chkFalsohttp.Checked = False
                End If
            Else
                chkVerohttp.Checked = False
                chkFalsohttp.Checked = False
            End If
            If Not IsDBNull(dtrGenerico("RiferimentoRimborsi")) Then
                If dtrGenerico("RiferimentoRimborsi") = True Then
                    chkRiferimentoRimborsi.Checked = True
                End If
            End If
            'città stera
            If ChkEstero.Checked = True Then
                If Not IsDBNull(dtrGenerico("CittaEstera")) Then
                    txtCity.Text = dtrGenerico("CittaEstera")
                End If
            End If

            If Not IsDBNull(dtrGenerico("Email")) Then
                txtemail.Text = dtrGenerico("Email")
                If Trim(lbldataControlloEmail.Value) <> "" Then
                    If dtrGenerico("EmailValido") = True Then
                        chkVeroEmail.Checked = True
                    Else
                        chkFalsoEmail.Checked = True
                    End If
                Else
                    chkVeroEmail.Checked = False
                    chkFalsoEmail.Checked = False
                End If
            Else
                chkVeroEmail.Checked = False
                chkFalsoEmail.Checked = False
            End If


            If Not IsDBNull(dtrGenerico("NormativaTutela")) Then
                If dtrGenerico("NormativaTutela") = True Then
                    ChkTutela.Checked = True
                Else
                    ChkTutela.Checked = False
                End If
            End If

            If Not IsDBNull(dtrGenerico("SoggettoEstero")) Then
                TxtSoggettoCapoSede.Text = dtrGenerico("SoggettoEstero")
            End If

            If Not IsDBNull(dtrGenerico("NonDisponibilitaSede")) Then
                If dtrGenerico("NonDisponibilitaSede") = True Then
                    rbSE2.Checked = True
                    rbSE1.Checked = False
                Else
                    rbSE2.Checked = False
                    rbSE1.Checked = True
                End If
            End If

            If Not IsDBNull(dtrGenerico("DisponibilitaSede")) Then
                If dtrGenerico("DisponibilitaSede") = True Then
                    rbNoSE1.Checked = False
                    rbNoSE2.Checked = True
                Else
                    rbNoSE1.Checked = True
                    rbNoSE2.Checked = False
                End If
            End If

            If Not IsDBNull(dtrGenerico("IdAllegatoLSE")) Then
                Session("LoadLSEId") = dtrGenerico("IdAllegatoLSE")
            Else
                Session("LoadLSEId") = Nothing
            End If
            If Not IsDBNull(dtrGenerico("Localita")) Then
                txtLocalita.Text = dtrGenerico("Localita")
            Else
                txtLocalita.Text = String.Empty
            End If

            If ChkEstero.Checked = True Then
                lblComune.Text = "Localit&agrave;"
                lblCap.Text = "Codice localit&agrave;"
                ddlComune.Visible = False
                txtLocalita.Visible = True
                DivItalia.Visible = False
                DivEstero2.Visible = True

                If Not IsDBNull(dtrGenerico("IdTitoloGiuridico")) Then
                    If dtrGenerico("IdTitoloGiuridico") = 7 Then
                        DivSoggettoEstero.Visible = True
                        DivNoSoggettoEstero.Visible = False
                    Else
                        DivSoggettoEstero.Visible = False
                        DivNoSoggettoEstero.Visible = True
                    End If
                End If
            Else
                ddlComune.Visible = True
                txtLocalita.Visible = False
            End If






            ChiudiDataReader(dtrGenerico)

            strsql = "Select * from statientisedi where statoentesede='" & Trim(Replace(lblStato.Text, "(*)", "")) & "'"
            ChiudiDataReader(dtrGenerico)
            dtrGenerico = ClsServer.CreaDatareader(strsql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
            dtrGenerico.Read()

            ' Aggiunto 22/01/2019 Da Luigi Leucci
            Select Case dtrGenerico("IdStatoEnteSede")
                Case 2, 5   ' Cancellata - Richiesta Cancellazione
                   ' AbilitaCampiMaschera(False)
                Case 3      ' Sospesa
                  ' If Session("TipoUtente") = "E" Then AbilitaCampiMaschera(False)

                Case 1, 4   ' Accreditata - Presentata

            End Select
            'If dtrGenerico("Attiva") = 0 And dtrGenerico("DaAccreditare") = 0 Then
            '    AbilitaCampiMaschera(False)
            'End If
            ChiudiDataReader(dtrGenerico)
            selComune.CaricaProvincie(ddlProvincia, ChkEstero.Checked, Session("Conn"))
            If Not idProvincia Is Nothing Then
                ddlProvincia.SelectedValue = idProvincia
            End If
            ChiudiDataReader(dtrGenerico)
            If Not idComune Is Nothing Then
                selComune.CaricaComuniDaProvincia(ddlComune, idProvincia, Session("Conn"))
                ddlComune.SelectedValue = idComune
                Session("IdComune") = idComune
            End If
            ChiudiDataReader(dtrGenerico)

            'ObbligoScadenza = TitoloGiuridico_ObbligoScandeza(ddlTitoloGiuridico.SelectedValue)
            'VisualizzaDateScadenze(ObbligoScadenza)
            DisabilitaCampo(ddlProvincia)
            DisabilitaCampo(ddlComune)
            'DisabilitaCampo(ddlEntiFigli)
            DisabilitaCampo(txtIndirizzo)
            DisabilitaCampo(txtLocalita)
            DisabilitaCampo(txtCap)
            DisabilitaCampo(txtCivico)
            DisabilitaCampo(TxtPalazzina)
            'DisabilitaCampo(ddlTitoloGiuridico)
            DisabilitaCampo(ChkTutela)
            DisabilitaCampo(TxtDettaglioRecapito)
            DisabilitaCampo(ChkEstero)
            DisabilitaCampo(TxtSoggettoCapoSede)
            DisabilitaCampo(rbSE2)
            DisabilitaCampo(rbSE1)
            DisabilitaCampo(rbNoSE1)
            DisabilitaCampo(rbNoSE2)
            DisabilitaCampo(btnEliminaLSE)
            DisabilitaCampo(btnModificaLSE)
            DisabilitaCampo(txtLocalita)
            DisabilitaCampo(txtCity)

            PersonalizzaTasti()
            ControllaSedeFisicaInclusa()
            RicercaSediAttuazioneVariazione()
            CaricaLSE()
        End If
        CaricaDatiEntiSediAttuazioneVariazione(IdEnteSede)
    End Sub
    Private Sub ModificaSedeEnte()
        'Creata il:		22/08/2014
        'Funzionalità: richiamo store SP_ACCREDITAMENTO_MODIFICAMASCHERA_SEDE per l'aggiornamento dell'anagrafica della sede
        Dim LSE As Allegato = Session("LoadedLSE")
        Dim hashValue As String = ""
        Dim idAllegatoOld As String = ""
        '      Dim sqlSediEnte = "Select IDENTESEDE, IdAllegato FROM entisedi where IdEnteSede=@IdEnteSede"
        '      Dim cmdSediEnte As New SqlCommand(sqlSediEnte, Session("conn"))
        '      cmdSediEnte.Parameters.AddWithValue("@IdEnteSede", Session("IdEnteSede"))
        '      Dim readerSediEnte = cmdSediEnte.ExecuteReader()
        'Try
        '	While readerSediEnte.Read
        '              idAllegatoOld = IIf(IsDBNull(readerSediEnte("IdAllegato")) = True, String.Empty, readerSediEnte("IdAllegato"))
        '          End While
        'Catch ex As Exception
        '	'da verificare
        'Finally
        '	readerSediEnte.Close()
        'End Try
        Dim SqlCmd As New SqlClient.SqlCommand
        Try
            SqlCmd.CommandText = "SP_MODIFICA_SEDE_FUORI_ADEGUAMENTO"
            SqlCmd.CommandType = CommandType.StoredProcedure
            SqlCmd.Connection = Session("Conn")
            If ddlEntiFigli.SelectedValue = 0 Then
                SqlCmd.Parameters.Add("@IdEnte", SqlDbType.Int).Value = CInt(Session("IDEnte"))
            Else
                SqlCmd.Parameters.Add("@IdEnte", SqlDbType.Int).Value = ddlEntiFigli.SelectedValue
            End If
            SqlCmd.Parameters.Add("@IdEnteSede", SqlDbType.Int).Value = txtCodice.Value
            SqlCmd.Parameters.Add("@Denominazione", SqlDbType.VarChar).Value = txtdenominazione.Text 'denominazioneente
            SqlCmd.Parameters.Add("@NMaxVolontari", SqlDbType.SmallInt).Value = TxtNumMaxVol.Text
            SqlCmd.Parameters.Add("@PrefissoTelefono", SqlDbType.VarChar).Value = txtprefisso.Text  'Prefisso telefono
            SqlCmd.Parameters.Add("@Telefono", SqlDbType.VarChar).Value = txtTelefono.Text
            If ddlTitoloGiuridico.SelectedValue = 0 Then
                SqlCmd.Parameters.Add("@IdTitoloGiuridico", SqlDbType.Int).Value = DBNull.Value
            Else
                SqlCmd.Parameters.Add("@IdTitoloGiuridico", SqlDbType.Int).Value = ddlTitoloGiuridico.SelectedValue
            End If
            SqlCmd.Parameters.Add("@UsernameRichiesta", SqlDbType.VarChar).Value = Session("Utente")
            SqlCmd.Parameters.Add("@Esito", SqlDbType.TinyInt)
            SqlCmd.Parameters("@Esito").Direction = ParameterDirection.Output
            SqlCmd.Parameters.Add("@messaggio", SqlDbType.VarChar)
            SqlCmd.Parameters("@messaggio").Size = 1000
            SqlCmd.Parameters("@messaggio").Direction = ParameterDirection.Output
            SqlCmd.Parameters.Add("@AnomaliaNome", SqlDbType.Bit).Value = Session("AnomaliaNome")
            '**** inserire qui i valori dei nuovi campi
            SqlCmd.ExecuteNonQuery()
            msgConferma.Text = "Modifica SEDE eseguita con successo."
            msgConferma.Text = msgConferma.Text & "<br/>"
        Catch ex As Exception
            msgErrore.Text = ex.Message
        Finally

        End Try
    End Sub

    Private Sub cmdAnnullaModifica_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdAnnullaModifica.Click
        '20/08/2014
        'annullo le modifiche effettua nell'anagrafica dell'ente
        Dim cmdGenerico As SqlClient.SqlCommand
        Dim StrSql As String
        StrSql = "UPDATE Accreditamento_VariazioneSedi SET StatoVariazione=2, UsernameCancellazione ='" & Session("Utente") & "', DataCancellazione= getdate() where IDEnteSede =" & txtCodice.Value & " And statovariazione = 0"
        cmdGenerico = ClsServer.EseguiSqlClient(StrSql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))

        StrSql = "UPDATE EntiSedi SET RichiestaModifica =0,UsernameRichiestaModifica=null,DataRichiestaModifica=null where IDEnteSede =" & txtCodice.Value & ""
        cmdGenerico = ClsServer.EseguiSqlClient(StrSql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))

        StrSql = "UPDATE EntiSediAttuazioni SET RichiestaModifica =0,UsernameRichiestaModifica=null,DataRichiestaModifica=null where IDEnteSede =" & txtCodice.Value & " "
        cmdGenerico = ClsServer.EseguiSqlClient(StrSql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))



        msgConferma.Text = "Annullamento modifica effettuato con successo."
        SettaValori(txtCodice.Value)

        LoadMaschera(txtCodice.Value)
        EvidenziaDatiModificati(txtidsede.Value)
    End Sub
    Sub SettaValori(ByVal IdEnteSede As Integer)
        If Not dtrGenerico Is Nothing Then
            dtrGenerico.Close()
            dtrGenerico = Nothing
        End If
        strsql = "select es.IDEnte,e.denominazione AS Ente from EntiSedi es inner join enti e on es.idente=e.idente where ES.IDEnteSede= " & IdEnteSede
        dtrGenerico = ClsServer.CreaDatareader(strsql, Session("conn"))
        dtrGenerico.Read()
        lblEnte.Text = dtrGenerico("Ente")
        lblidEnte.Value = dtrGenerico("idente")
        If lblidEnte.Value <> Session("IdEnte") Then
            idEnteFiglio.Value = lblidEnte.Value
        End If
        'Context.Items("tipoazione") = lblPersonalizza.Value
        'lblStato.Text = Context.Items("stato")
        If Not dtrGenerico Is Nothing Then
            dtrGenerico.Close()
            dtrGenerico = Nothing
        End If
    End Sub
    Private Sub LoadMaschera(ByVal IdEnteSede As Integer)
        Dim abilitato As Integer
        Dim dati As Integer
        Dim annullamodifica As Integer
        Dim visualizzadatiaccreditati As Integer
        Dim annullacancellazione As Integer
        Dim messaggio As String

        Clear()
        '1. RICHIAMO STORE CHE VERIFICA L'ACCESSO MASCHERA DELLA SEDE 
        Accesso_Maschera_Sede(Session("TipoUtente"), Session("IDEnte"), IdEnteSede, abilitato, dati, annullamodifica, annullacancellazione, visualizzadatiaccreditati, messaggio)

        '2.PERSONALIZZO CAMPI NON VISIBILI SE SONO ENTE O UNSC/REGIONE
        PersonalizzoNoVisibilitaCampi()
        ''3.ABILITO/DISABILITO MASCHERA SE E' MODIFICABILE O IN LETTERA
        abilitato = 1
        If abilitato = 0 Then ' -- 0: maschera sola lettura		1: maschera in modifica
            msgInfo.CssClass = "msgErrore"
            AbilitaCampiMaschera(False)
        Else
            msgInfo.CssClass = "msgInfo"
            AbilitaCampiMaschera(True)
        End If
        'CARICA DATI
        If lblPersonalizza.Value <> "Inserimento" Then
            If dati = 0 Then ' -- 0: dati tabelle reali		1: dati tabelle variazioni
                PopolaMaschera(IdEnteSede)
            Else
                PopolaMascheraVariazione(IdEnteSede) 'PopolaMaschera dati variazione
            End If
            If abilitato <> 0 Then
                'VisualizzaDateScadenze(TitoloGiuridico_ObbligoScandeza(ddlTitoloGiuridico.SelectedValue))
            End If

        End If
        '*******Non in fuoriadeguamento
        'If annullamodifica = 0 Then '-- 0: funzione non abilitata	1: funzione abilitata
        '    'pulsante
        cmdAnnullaModifica.Visible = False
        'Else
        '    cmdAnnullaModifica.Visible = True
        'End If
        'If annullacancellazione = 0 Then  '-- 0: funzione non abilitata	1: funzione abilitata
        '    'pulsante
        cmdAnnullaCancellazione.Visible = False
        'Else
        '    cmdAnnullaCancellazione.Visible = True
        'End If
        'If visualizzadatiaccreditati = 0 Then '-- 0: funzione non abilitata	1: funzione abilitata
        '    'pulsante
        cmdVisualizzaDatiAccreditati.Visible = False
        'Else
        '    cmdVisualizzaDatiAccreditati.Visible = True
        'End If
        'If abilitato = 0 Then
        cmdCancella.Visible = False
        '    'cmdSalva.Visible = False
        'End If
    End Sub

    Private Sub cmdAnnullaCancellazione_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdAnnullaCancellazione.Click
        AccreditamentoAnnullaEliminaSede(txtidsede.Value)
        SettaValori(txtidsede.Value)
        LoadMaschera(txtidsede.Value)
        EvidenziaDatiModificati(txtidsede.Value)
    End Sub

    Private Sub AccreditamentoEliminaSede(ByVal IdEnteSede As Integer)
        'Creata il:		25/08/2014
        'Funzionalità: richiamo store SP_ACCREDITAMENTO_ELIMINAMASCHERA_SEDE per il ripristino della formazione


        Dim SqlCmd As New SqlClient.SqlCommand
        Try
            SqlCmd.CommandText = "SP_ACCREDITAMENTO_ELIMINAMASCHERA_SEDE"
            SqlCmd.CommandType = CommandType.StoredProcedure
            SqlCmd.Connection = Session("Conn")

            SqlCmd.Parameters.Add("@IdEnteSede ", SqlDbType.Int).Value = IdEnteSede
            SqlCmd.Parameters.Add("@UsernameRichiesta", SqlDbType.VarChar).Value = Session("Utente")

            'Esito aggiornamento: 0-Errore 1-Aggiornamento effettuato
            SqlCmd.Parameters.Add("@Esito", SqlDbType.TinyInt)
            SqlCmd.Parameters("@Esito").Direction = ParameterDirection.Output

            SqlCmd.Parameters.Add("@messaggio", SqlDbType.VarChar)
            SqlCmd.Parameters("@messaggio").Size = 1000
            SqlCmd.Parameters("@messaggio").Direction = ParameterDirection.Output

            SqlCmd.ExecuteNonQuery()

            msgConferma.Text = SqlCmd.Parameters("@messaggio").Value()
            msgConferma.Text = msgConferma.Text & "<br/>"
        Catch ex As Exception
            msgErrore.Text = ex.Message
        Finally

        End Try
    End Sub

    Private Sub AccreditamentoAnnullaEliminaSede(ByVal IdEnteSede As Integer)
        'Creata il:		25/08/2014
        'Funzionalità: richiamo store SP_ACCREDITAMENTO_ANNULLAELIMINAMASCHERA_SEDE per il ripristino della formazione

        Dim SqlCmd As New SqlClient.SqlCommand
        Try
            SqlCmd.CommandText = "SP_ACCREDITAMENTO_ANNULLAELIMINAMASCHERA_SEDE"
            SqlCmd.CommandType = CommandType.StoredProcedure
            SqlCmd.Connection = Session("Conn")

            SqlCmd.Parameters.Add("@IdEnteSede ", SqlDbType.Int).Value = IdEnteSede
            SqlCmd.Parameters.Add("@UsernameRichiesta", SqlDbType.VarChar).Value = Session("Utente")

            'Esito aggiornamento: 0-Errore 1-Aggiornamento effettuato
            SqlCmd.Parameters.Add("@Esito", SqlDbType.TinyInt)
            SqlCmd.Parameters("@Esito").Direction = ParameterDirection.Output

            SqlCmd.Parameters.Add("@messaggio", SqlDbType.VarChar)
            SqlCmd.Parameters("@messaggio").Size = 1000
            SqlCmd.Parameters("@messaggio").Direction = ParameterDirection.Output

            SqlCmd.ExecuteNonQuery()

            msgConferma.Text = SqlCmd.Parameters("@messaggio").Value()
            msgConferma.Text = msgConferma.Text & "<br/>"
        Catch ex As Exception
            msgErrore.Text = ex.Message
        Finally

        End Try
    End Sub


    Protected Sub imgOpenFigli_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs) Handles imgOpenFigli.Click
        Response.Write("<script>" & vbCrLf)
        Response.Write("window.open(""WfrmPopUpEntiFiglio.aspx?identita=1"", ""Visualizza"", ""width=670,height=300,dependent=no,scrollbars=yes,status=no,resizable=yes"")" & vbCrLf)
        Response.Write("</script>")
    End Sub

#Region "Gestione indirizzi"
    Private Sub ChkEstero_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ChkEstero.CheckedChanged
        Dim selComune As New clsSelezionaComune
        selComune.CaricaProvincie(ddlProvincia, ChkEstero.Checked, Session("Conn"))
        ddlComune.DataSource = Nothing
        ddlComune.Items.Add("")
        ddlComune.SelectedIndex = 0
        CancellaMessaggi()

        If ChkEstero.Checked = True Then
            city.Visible = True
            ddlComune.Visible = False
            txtLocalita.Visible = True
            lblComune.Text = "Localit&agrave;"
            lblCap.Text = "Codice localit&agrave;"
        Else
            ddlComune.Visible = True
            txtLocalita.Visible = False
            city.Visible = False
            lblComune.Text = "<strong>(*)</strong>Comune"
            lblCap.Text = "<strong>(*)</strong>C.A.P."
        End If
    End Sub
    Private Sub ddlProvincia_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlProvincia.SelectedIndexChanged
        Dim selComune As New clsSelezionaComune
        ddlComune.Enabled = True
        ddlComune = selComune.CaricaComuni(ddlComune, ddlProvincia.SelectedValue, Session("Conn"))
    End Sub
    Protected Sub imgCap_Click(ByVal sender As Object, ByVal e As EventArgs) Handles imgCap.Click
        msgErrore.Text = String.Empty
        Dim selCap As New clsSelezionaComune
        If ddlComune.SelectedValue = String.Empty And txtCivico.Text = String.Empty And txtIndirizzo.Text = String.Empty Then
            msgErrore.Text = "Per ottenere il C.A.P. della residenza è necessario indicare almeno il comune e l'indirizzo di residenza."
        Else
            txtCap.Text = selCap.RitornaCap(ddlComune.SelectedValue, txtIndirizzo.Text, txtCivico.Text, Session("conn"))
        End If
    End Sub



#End Region

    Private Function VerificaValiditaCampi(ByVal AlboEnte As String) As Boolean
        Dim utility As ClsUtility = New ClsUtility()
        Dim campiValidi As Boolean = True
        Dim numero As Int32
        Dim dataTmp As Date
        Dim campoObbligatorio As String = "Il campo {0} è obbligatorio.<br/>"
        Dim campoObbligatorioSCU As String = "Il campo {0} è obbligatorio."
        Dim campoObbligatorioSCU1 As String = "Indicare {0} nel caso non sia previsto.<br/>"
        Dim numeroNonValido As String = "Il valore di '{0}' non è valido. Inserire solo numeri.<br/>"
        Dim emailNonValida As String = "Il valore di '{0}' non è valido. Inserire un indirizzo email valido.<br/>"
        Dim LungezzaErrata As String = "Il campo {0} può contenere massimo 5 caratteri.<br/>"
        Dim messaggioDataValida As String = "Il valore di '{0}' non è valido. Inserire la data nel formato gg/mm/aaaa.<br/>"
        Dim CittaEstera As String = "Il campo {0} è obbligatorio."
        Dim NomeAnomalo As String = "Il campo {0} è obbligatorio."
        Dim IndirizzoAnomalo As String = "Il campo {0} è obbligatorio."
        Dim ListaAnomalie As String = ""
        Dim CheckIndirizzo As Boolean = True
        Dim regioneCompetenza As Integer = getRegioneCompetenzaEnte()
        If (txtdenominazione.Text = String.Empty) Then
            msgErrore.Text = msgErrore.Text + String.Format(campoObbligatorio, "Sede")
            campiValidi = False
        End If

        If (TxtNumMaxVol.Text = String.Empty) Then
            msgErrore.Text = msgErrore.Text + String.Format(campoObbligatorio, "N° Vol.")
            campiValidi = False
        Else
            If (Int32.TryParse(TxtNumMaxVol.Text, numero) = False) Then
                msgErrore.Text = msgErrore.Text + String.Format(numeroNonValido, "N° Vol.")
                campiValidi = False
            Else
                If CInt(TxtNumMaxVol.Text) <= 0 Then
                    msgErrore.Text = msgErrore.Text + String.Format(numeroNonValido, "N° Vol.")
                    campiValidi = False
                End If
            End If

        End If

        If (txtprefisso.Text = String.Empty) Then
            msgErrore.Text = msgErrore.Text + String.Format(campoObbligatorio, "Prefisso Telefono")
            campiValidi = False
        Else
            If (Int32.TryParse(txtprefisso.Text, numero) = False) Then
                msgErrore.Text = msgErrore.Text + String.Format(numeroNonValido, "Prefisso Telefono")
                campiValidi = False
            End If
        End If
        If (txtTelefono.Text = String.Empty) Then
            msgErrore.Text = msgErrore.Text + String.Format(campoObbligatorio, "Telefono")
            campiValidi = False
        Else
            Try
                If (Int64.TryParse(txtTelefono.Text, numero) = False) Then
                    msgErrore.Text = msgErrore.Text + String.Format(numeroNonValido, "Telefono")
                    campiValidi = False
                End If
            Catch ex As Exception

            End Try

        End If

        Dim EnteOriginale, enteScelto As Integer
        If ddlEntiFigli.SelectedValue = 0 Then
            enteScelto = CInt(Session("IdEnte"))
        Else
            enteScelto = ddlEntiFigli.SelectedValue
        End If
        Dim IdEnteP As SqlClient.SqlCommand
        IdEnteP = New SqlClient.SqlCommand
        IdEnteP.CommandType = CommandType.Text
        IdEnteP.CommandText = "Select Idente from entisedi where identesede = @IdEnteSede"
        IdEnteP.Connection = IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn"))

        Dim sparam As SqlClient.SqlParameter
        sparam = New SqlClient.SqlParameter
        sparam.ParameterName = "@IdEnteSede"
        sparam.SqlDbType = SqlDbType.Int
        IdEnteP.Parameters.Add(sparam)
        Dim Reader As SqlClient.SqlDataReader
        IdEnteP.Parameters("@IdEnteSede").Value = CInt(Session("IDEnteSede"))

        Reader = IdEnteP.ExecuteReader()
        If Reader.Read Then
            EnteOriginale = CInt(Reader("IDEnte"))
            Reader.Close()
            IdEnteP.Dispose()
        Else
            Reader.Close()
        End If
        ' Insert code to read through the datareader.
        'If CustOrderHist.Parameters("@Valore").Value = 0 Then


        If Not Reader Is Nothing Then
            Reader.Close()
            Reader = Nothing
        End If
        If EnteOriginale <> enteScelto Then
            Dim VerificaSedi As SqlClient.SqlCommand
            VerificaSedi = New SqlClient.SqlCommand
            VerificaSedi.CommandType = CommandType.Text
            VerificaSedi.CommandText = "select COUNT(identesedeattuazione) as NSedi" &
                    " from entisedi a1 inner join entisediattuazioni b1 on a1.IDEnteSede = b1.IDEnteSede" &
                    " where a1.IDEnte = @IDEnte and b1.idstatoentesede in (1,3,4)"

            VerificaSedi.Connection = IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn"))

            Dim sparamSedi As SqlClient.SqlParameter
            sparamSedi = New SqlClient.SqlParameter
            sparamSedi.ParameterName = "@IDEnte"
            sparamSedi.SqlDbType = SqlDbType.Int
            VerificaSedi.Parameters.Add(sparamSedi)
            Dim ReaderSedi As SqlClient.SqlDataReader
            VerificaSedi.Parameters("@IDEnte").Value = EnteOriginale

            ReaderSedi = VerificaSedi.ExecuteReader()
            If ReaderSedi.Read Then
                If CInt(ReaderSedi("NSedi")) < 2 Then
                    msgErrore.Text = msgErrore.Text + "Impossibile spostare ad altro ente l'unica sede di attuazione. Se si desidera eliminare questa sede è obbligatorio aprire una fase di adeguamento e procedere all'eliminazione dell'ente senza più sedi"
                    campiValidi = False
                End If

                ReaderSedi.Close()
                VerificaSedi.Dispose()
            Else
                ReaderSedi.Close()
            End If

            ' Insert code to read through the datareader.
            'If CustOrderHist.Parameters("@Valore").Value = 0 Then


            If Not ReaderSedi Is Nothing Then
                ReaderSedi.Close()
                ReaderSedi = Nothing
            End If
        End If
        Return campiValidi

    End Function
    Private Function ValidaInteri(ByVal valore As String, ByVal nomeCampo As String) As Boolean
        Dim numero As Int32
        Dim campiValidi As Boolean = True
        Dim messaggioNumeroNonValido As String = "Il valore di '{0}' non è valido. Inserire solo numeri.<br/>"

        If (Int32.TryParse(valore, numero) = False) Then
            msgErrore.Text = msgErrore.Text + String.Format(messaggioNumeroNonValido, nomeCampo)
            campiValidi = False
        End If
        Return campiValidi

    End Function
    Private Sub CancellaMessaggi()
        msgErrore.Text = String.Empty
        msgInfo.Text = String.Empty
        msgConferma.Text = String.Empty
    End Sub

    Protected Sub cmdCancella_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdCancella.Click

        Dim strMsg As String = ""
        If Session("Tipoutente") = "U" Then
            If AccreditamentoVincoloCancellazione(Session("IdEnte"), Request.QueryString("identesede"), strMsg) = False Then
                'apro div per invio messaggio e per richiedere la conferma della cacellazione
                divChiusuraSede.Visible = True
                lblChiudiSede.Text = strMsg
                cmdCancella.Visible = False
            Else
                cmdCancella.Visible = True
                divChiusuraSede.Visible = False
                EliminaSede()

            End If
        Else

            cmdCancella.Visible = True
            divChiusuraSede.Visible = False
            EliminaSede()
        End If

    End Sub

    <System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()>
    Public Shared Function GetCompletionList(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As List(Of String)

        'sull'estero l'autocomplete non ha piu' senso

        If Not IsNumeric(contextKey) Then Return Nothing
        Dim conn As SqlConnection = New SqlConnection
        conn.ConnectionString = ConfigurationManager _
             .ConnectionStrings("unscproduzionenewConnectionString").ConnectionString


        Dim cmd As SqlCommand = New SqlCommand
        cmd.CommandText = " Select Top 30 CAP_INDIRIZZI.Indirizzo as CityName FROM  CAP_INDIRIZZI WHERE (CAP_INDIRIZZI.Indirizzo LIKE '%" + prefixText.Replace("'", "''") + "%') and idcomune='" & contextKey & "'  ORDER BY CAP_INDIRIZZI.Indirizzo"
        cmd.Connection = conn
        conn.Open()

        Dim oReader As SqlDataReader = cmd.ExecuteReader
        Dim indirizzi As List(Of String) = New List(Of String)

        While oReader.Read
            indirizzi.Add(oReader.GetString(0))
        End While


        If Not oReader Is Nothing Then
            oReader.Close()
            oReader = Nothing
            conn.Close()
        End If
        Return indirizzi


    End Function

    Sub EvidenziaDatiModificati(ByVal IdEnteSede As Integer)
        Dim dtrGenerico As SqlClient.SqlDataReader
        Dim strsql As String
        Dim DsSedi As DataSet = New DataSet
        If Not dtrGenerico Is Nothing Then
            dtrGenerico.Close()
            dtrGenerico = Nothing
        End If
        'popolamento della maschera dati DB

        strsql = " SELECT comuni.idComune,comuni.denominazione as Comune,provincie.provincia,StatiEntiSedi.statoentesede," &
                 " entisediattuazioni.idEnteSedeAttuazione,tipisedi.tiposede," &
                 " entisedi.Denominazione, entisedi.Indirizzo,entisedi.DettaglioRecapito, entisedi.civico, entisedi.cap, " &
                 " isnull(entisedi.prefissoTelefono,'') as prefissoTelefono, isnull(entisedi.Telefono,'') as telefono," &
                 " entisedi.Palazzina,entisedi.Piano,entisedi.scala,entisedi.interno, " &
                 " isnull(entisedi.prefissofax,'') as prefissoFax, isnull(entisedi.fax,'') as fax ,entisediattuazioni.NMaxVolontari," &
                 " entisedi.http, entisedi.email, TitoliGiuridici.TitoloGiuridico, entisedi.AltroTitoloGiuridico, " &
                 " isnull(dbo.FormatoData(entisedi.DataStipulaContratto),'') as DataStipulaContratto,   " &
                 " isnull(dbo.FormatoData(entisedi.DataScadenzaContratto),'') as DataScadenzaContratto,entisedi.CittaEstera," &
                 " IdAllegatoLSE, AnomaliaIndirizzo, AnomaliaNome, NormativaTutela, SoggettoEstero, NonDisponibilitaSede, DisponibilitaSede, AnomaliaIndirizzoGoogle, entisedi.idTitoloGiuridico" &
                 " FROM entisedi " &
                 " INNER JOIN entisediattuazioni on entisedi.identesede = entisediattuazioni.identesede " &
                 " INNER JOIN StatiEntiSedi on (entisedi.IdStatoEnteSede=dbo.StatiEntiSedi.IdStatoEnteSede) " &
                 " INNER JOIN comuni on (comuni.idcomune=entisedi.idcomune) " &
                 " INNER JOIN provincie on (provincie.idprovincia=comuni.idprovincia)" &
                 " LEFT JOIN entisediTipi on (entisediTipi.identesede=entisedi.idEntesede)" &
                 " INNER JOIN tipiSedi on entisediTipi.idtiposede = tipiSedi.idtiposede" &
                 " INNER JOIN TitoliGiuridici on TitoliGiuridici.IdTitoloGiuridico = entisedi.IdTitoloGiuridico" &
                 " WHERE entisedi.identeSede = " & IdEnteSede & ""

        dtrGenerico = ClsServer.CreaDatareader(strsql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
        dtrGenerico.Read()
        If dtrGenerico.HasRows = True Then
            If Not IsDBNull(dtrGenerico("denominazione")) Then
                If Not (String.Equals(txtdenominazione.Text, dtrGenerico("denominazione"), StringComparison.InvariantCultureIgnoreCase)) Then
                    helper.ModificaStyleDatiModificati(txtdenominazione)
                Else
                    helper.RipristinaStyleDatiModificati(txtdenominazione)
                End If
            End If

            'If Not IsDBNull(dtrGenerico("idEnteSedeAttuazione")) Then
            '    If Not (String.Equals(ddlEntiFigli.SelectedItem.Text, dtrGenerico("idEnteSedeAttuazione"), StringComparison.InvariantCultureIgnoreCase)) Then
            '        helper.ModificaStyleDatiModificati(ddlEntiFigli)
            '    Else
            '        helper.RipristinaStyleDatiModificati(ddlEntiFigli)
            '    End If
            'End If
            If Not IsDBNull(dtrGenerico("indirizzo")) Then
                If Not (String.Equals(txtIndirizzo.Text, dtrGenerico("indirizzo"), StringComparison.InvariantCultureIgnoreCase)) Then
                    helper.ModificaStyleDatiModificati(txtIndirizzo)
                Else
                    helper.RipristinaStyleDatiModificati(txtIndirizzo)
                End If
            End If
            If Not IsDBNull(dtrGenerico("DettaglioRecapito")) Then
                If Not (String.Equals(TxtDettaglioRecapito.Text, dtrGenerico("DettaglioRecapito"), StringComparison.InvariantCultureIgnoreCase)) Then
                    helper.ModificaStyleDatiModificati(TxtDettaglioRecapito)
                Else
                    helper.RipristinaStyleDatiModificati(TxtDettaglioRecapito)
                End If
            End If
            If Not IsDBNull(dtrGenerico("Palazzina")) Then
                If Not (String.Equals(TxtPalazzina.Text, dtrGenerico("Palazzina"), StringComparison.InvariantCultureIgnoreCase)) Then
                    helper.ModificaStyleDatiModificati(TxtPalazzina)
                Else
                    helper.RipristinaStyleDatiModificati(TxtPalazzina)
                End If
            End If
            If Not IsDBNull(dtrGenerico("Civico")) Then
                If Not (String.Equals(txtCivico.Text, dtrGenerico("Civico"), StringComparison.InvariantCultureIgnoreCase)) Then
                    helper.ModificaStyleDatiModificati(txtCivico)
                Else
                    helper.RipristinaStyleDatiModificati(txtCivico)
                End If
            End If
            If Not IsDBNull(dtrGenerico("Scala")) Then
                If Not (String.Equals(TxtScala.Text, dtrGenerico("Scala"), StringComparison.InvariantCultureIgnoreCase)) Then
                    helper.ModificaStyleDatiModificati(TxtScala)
                Else
                    helper.RipristinaStyleDatiModificati(TxtScala)
                End If
            End If
            If Not IsDBNull(dtrGenerico("Piano")) Then
                If Not (String.Equals(TxtPiano.Text, dtrGenerico("Piano"), StringComparison.InvariantCultureIgnoreCase)) Then
                    helper.ModificaStyleDatiModificati(TxtPiano)
                Else
                    helper.RipristinaStyleDatiModificati(TxtPiano)
                End If
            End If
            If Not IsDBNull(dtrGenerico("Interno")) Then
                If Not (String.Equals(TxtInterno.Text, dtrGenerico("Interno"), StringComparison.InvariantCultureIgnoreCase)) Then
                    helper.ModificaStyleDatiModificati(TxtInterno)
                Else
                    helper.RipristinaStyleDatiModificati(TxtInterno)
                End If
            End If
            If Not IsDBNull(dtrGenerico("TipoSede")) Then
                If Not (String.Equals(ddlTipologia.SelectedItem.Text, dtrGenerico("TipoSede"), StringComparison.InvariantCultureIgnoreCase)) Then
                    helper.ModificaStyleDatiModificati(ddlTipologia)
                Else
                    helper.RipristinaStyleDatiModificati(ddlTipologia)
                End If
            End If
            If Not IsDBNull(dtrGenerico("TitoloGiuridico")) Then
                If Not (String.Equals(ddlTitoloGiuridico.SelectedItem.Text, dtrGenerico("TitoloGiuridico"), StringComparison.InvariantCultureIgnoreCase)) Then
                    helper.ModificaStyleDatiModificati(ddlTitoloGiuridico)
                Else
                    helper.RipristinaStyleDatiModificati(ddlTitoloGiuridico)
                End If
            End If
            If Not IsDBNull(dtrGenerico("AltroTitoloGiuridico")) Then
                If Not (String.Equals(txtAltroTitoloGiuridicio.Text, dtrGenerico("AltroTitoloGiuridico"), StringComparison.InvariantCultureIgnoreCase)) Then
                    helper.ModificaStyleDatiModificati(txtAltroTitoloGiuridicio)
                Else
                    helper.RipristinaStyleDatiModificati(txtAltroTitoloGiuridicio)
                End If
            End If
            If Not IsDBNull(dtrGenerico("NMaxVolontari")) Then
                If Not (String.Equals(TxtNumMaxVol.Text, dtrGenerico("NMaxVolontari"), StringComparison.InvariantCultureIgnoreCase)) Then
                    helper.ModificaStyleDatiModificati(TxtNumMaxVol)
                Else
                    helper.RipristinaStyleDatiModificati(TxtNumMaxVol)
                End If
            End If
            If Not IsDBNull(dtrGenerico("Cap")) Then
                If Not (String.Equals(txtCap.Text, dtrGenerico("Cap"), StringComparison.InvariantCultureIgnoreCase)) Then
                    helper.ModificaStyleDatiModificati(txtCap)
                Else
                    helper.RipristinaStyleDatiModificati(txtCap)
                End If
            End If
            If Not IsDBNull(dtrGenerico("prefissoTelefono")) Then
                If Not (String.Equals(txtprefisso.Text, dtrGenerico("prefissoTelefono"), StringComparison.InvariantCultureIgnoreCase)) Then
                    helper.ModificaStyleDatiModificati(txtprefisso)
                Else
                    helper.RipristinaStyleDatiModificati(txtprefisso)
                End If
            End If
            If Not IsDBNull(dtrGenerico("Telefono")) Then
                If Not (String.Equals(txtTelefono.Text, dtrGenerico("telefono"), StringComparison.InvariantCultureIgnoreCase)) Then
                    helper.ModificaStyleDatiModificati(txtTelefono)
                Else
                    helper.RipristinaStyleDatiModificati(txtTelefono)
                End If
            End If
            If Not IsDBNull(dtrGenerico("prefissoFax")) Then
                If Not (String.Equals(txtPrefFax.Text, dtrGenerico("prefissoFax"), StringComparison.InvariantCultureIgnoreCase)) Then
                    helper.ModificaStyleDatiModificati(txtPrefFax)
                Else
                    helper.RipristinaStyleDatiModificati(txtPrefFax)
                End If
            End If
            If Not IsDBNull(dtrGenerico("Fax")) Then
                If Not (String.Equals(txtfax.Text, dtrGenerico("Fax"), StringComparison.InvariantCultureIgnoreCase)) Then
                    helper.ModificaStyleDatiModificati(txtfax)
                Else
                    helper.RipristinaStyleDatiModificati(txtfax)
                End If
            End If
            If Not IsDBNull(dtrGenerico("http")) Then
                If Not (String.Equals(txthttp.Text, dtrGenerico("http"), StringComparison.InvariantCultureIgnoreCase)) Then
                    helper.ModificaStyleDatiModificati(txthttp)
                Else
                    helper.RipristinaStyleDatiModificati(txthttp)
                End If
            End If
            If Not IsDBNull(dtrGenerico("email")) Then
                If Not (String.Equals(txtemail.Text, dtrGenerico("email"), StringComparison.InvariantCultureIgnoreCase)) Then
                    helper.ModificaStyleDatiModificati(txtemail)
                Else
                    helper.RipristinaStyleDatiModificati(txtemail)
                End If
            End If
            If ChkEstero.Checked = True Then
                If Not IsDBNull(dtrGenerico("CittaEstera")) Then
                    If Not (String.Equals(txtCity.Text, dtrGenerico("CittaEstera"), StringComparison.InvariantCultureIgnoreCase)) Then
                        helper.ModificaStyleDatiModificati(txtCity)
                    Else
                        helper.RipristinaStyleDatiModificati(txtCity)
                    End If
                End If
                If Not IsDBNull(dtrGenerico("IdTitoloGiuridico")) Then
                    If dtrGenerico("IdTitoloGiuridico") = 7 Then

                        If Not IsDBNull(dtrGenerico("IdAllegatoLSE")) Then
                            If Not (String.Equals(Session("LoadLSEId").ToString, dtrGenerico("IdAllegatoLSE").ToString, StringComparison.InvariantCultureIgnoreCase)) Then
                                helper.ModificaStyleDatiModificati(btnDownloadLSE)
                                helper.ModificaStyleDatiModificati(btnEliminaLSE)
                                helper.ModificaStyleDatiModificati(btnModificaLSE)
                            Else
                                helper.RipristinaStyleDatiModificati(btnDownloadLSE)
                                helper.RipristinaStyleDatiModificati(btnEliminaLSE)
                                helper.RipristinaStyleDatiModificati(btnModificaLSE)
                            End If
                        End If

                        If Not IsDBNull(dtrGenerico("SoggettoEstero")) Then
                            If Not (String.Equals(TxtSoggettoCapoSede.Text, dtrGenerico("SoggettoEstero"), StringComparison.InvariantCultureIgnoreCase)) Then
                                helper.ModificaStyleDatiModificati(TxtSoggettoCapoSede)
                            Else
                                helper.RipristinaStyleDatiModificati(TxtSoggettoCapoSede)
                            End If
                        End If
                        Dim myValueSE As Boolean
                        If rbSE1.Checked Then
                            myValueSE = False
                        Else
                            myValueSE = True
                        End If
                        If Not IsDBNull(dtrGenerico("NonDisponibilitaSede")) Then
                            If Not (String.Equals(myValueSE.ToString, dtrGenerico("NonDisponibilitaSede").ToString, StringComparison.InvariantCultureIgnoreCase)) Then
                                helper.ModificaStyleDatiModificati(rbSE1)
                                helper.ModificaStyleDatiModificati(rbSE2)
                            Else
                                helper.RipristinaStyleDatiModificati(rbSE1)
                                helper.RipristinaStyleDatiModificati(rbSE2)
                            End If
                        End If
                    Else
                        Dim myValue As Boolean
                        If rbNoSE1.Checked Then
                            myValue = False
                        Else
                            myValue = True
                        End If
                        If Not IsDBNull(dtrGenerico("DisponibilitaSede")) Then
                            If Not (String.Equals(myValue.ToString <> dtrGenerico("DisponibilitaSede").ToString, StringComparison.InvariantCultureIgnoreCase)) Then
                                helper.ModificaStyleDatiModificati(rbNoSE1)
                                helper.ModificaStyleDatiModificati(rbNoSE2)
                            Else
                                helper.RipristinaStyleDatiModificati(rbNoSE1)
                                helper.RipristinaStyleDatiModificati(rbNoSE2)
                            End If
                        End If
                    End If

                End If
            Else
                If Not IsDBNull(dtrGenerico("NormativaTutela")) Then
                    If String.Equals(ChkTutela.Checked.ToString <> dtrGenerico("NormativaTutela").ToString, StringComparison.InvariantCultureIgnoreCase) Then
                        helper.ModificaStyleDatiModificati(ChkTutela)
                    Else
                        helper.RipristinaStyleDatiModificati(ChkTutela)
                    End If
                End If
            End If ' Fine  chkestero

        End If 'Fine dtgenerico



        'If Not IsDBNull(dtrGenerico("NormativaTutela")) Then
        '    If dtrGenerico("NormativaTutela") = True Then
        '        ChkTutela.Checked = True
        '    Else
        '        ChkTutela.Checked = False
        '    End If
        'End If

        'If Not IsDBNull(dtrGenerico("SoggettoEstero")) Then
        '    TxtSoggettoCapoSede.Text = dtrGenerico("SoggettoEstero")
        'End If

        'If Not IsDBNull(dtrGenerico("NonDisponibilitaSede")) Then
        '    If dtrGenerico("NonDisponibilitaSede") = True Then
        '        chkNoDisponibilita.Checked = True
        '    Else
        '        chkNoDisponibilita.Checked = False
        '    End If
        'End If

        'If Not IsDBNull(dtrGenerico("DisponibilitaSede")) Then
        '    If dtrGenerico("DisponibilitaSede") = True Then
        '        rbNoSE1.Checked = False
        '        rbNoSE2.Checked = True
        '    Else
        '        rbNoSE1.Checked = True
        '        rbNoSE2.Checked = False
        '    End If
        'End If

        'If Not IsDBNull(dtrGenerico("IdAllegatoLSE")) Then
        '    Session("LoadLSEId") = dtrGenerico("IdAllegatoLSE")
        'Else
        '    Session("LoadLSEId") = Nothing
        'End If
        'If ChkEstero.Checked = True Then
        '    DivEstero.Visible = True
        '    DivEstero2.Visible = True

        '    If Not IsDBNull(dtrGenerico("IdTitoloGiuridico")) Then
        '        If dtrGenerico("IdTitoloGiuridico") = 7 Then
        '            DivSoggettoEstero.Visible = True
        '            DivNoSoggettoEstero.Visible = False
        '        Else
        '            DivSoggettoEstero.Visible = False
        '            DivNoSoggettoEstero.Visible = True
        '        End If
        '    End If
        'End If

        ChiudiDataReader(dtrGenerico)

    End Sub

    Protected Sub cmdVisualizzaDatiAccreditati_Click(sender As Object, e As EventArgs) Handles cmdVisualizzaDatiAccreditati.Click
        Response.Write("<script>" & vbCrLf)
        Response.Write("window.open(""informazionientesede.aspx?IdEnteSede=" & Request.QueryString("identesede") & """, """", ""width=600, height=600, toolbar=no, location=no, menubar=no,resizable=yes ,scrollbars=yes"")" & vbCrLf)
        Response.Write("</script>")
    End Sub


    Protected Sub imgRichiestaModifica_Click(sender As Object, e As EventArgs) Handles imgRichiestaModifica.Click
        Try
            AggiornaRichiestaModifica(1, Request.QueryString("identesede"))
            msgConferma.Visible = True
            msgConferma.Text = "Aggiornamento effettuato con successo."
            imgAnnullaRichiestaModifica.Visible = True
            imgRichiestaModifica.Visible = False
        Catch ex As Exception
            msgErrore.Visible = True
            msgErrore.Text = "Contattare l'assistenza."
        End Try
    End Sub

    Protected Sub imgAnnullaRichiestaModifica_Click(sender As Object, e As EventArgs) Handles imgAnnullaRichiestaModifica.Click
        Try
            AggiornaRichiestaModifica(0, Request.QueryString("identesede"))
            msgConferma.Visible = True
            msgConferma.Text = "Aggiornamento effettuato con successo."
            imgAnnullaRichiestaModifica.Visible = False
            imgRichiestaModifica.Visible = True
        Catch ex As Exception
            msgErrore.Visible = True
            msgErrore.Text = "Contattare l'assistenza."
        End Try
    End Sub
    Private Function AccreditamentoVincoloCancellazione(ByVal IdEnte As Integer, ByVal IdEnteAccoglienza As Integer, ByRef mess As String) As Boolean
        'Creata il:		17/04/2018
        'Funzionalità: richiamo store SP_VINCOLO_SEDI_VERIFICA_CANCELLAZIONE per verificare se la cancellazione richiesta non faccia decadere l'accreditamento

        Dim blbEsito As Boolean
        Dim SqlCmd As New SqlClient.SqlCommand
        Try
            SqlCmd.CommandText = "[SP_VINCOLO_SEDI_VERIFICA_CANCELLAZIONE]"
            SqlCmd.CommandType = CommandType.StoredProcedure
            SqlCmd.Connection = Session("Conn")

            SqlCmd.Parameters.Add("@IdEnte", SqlDbType.Int).Value = IdEnte
            SqlCmd.Parameters.Add("@IdSoggetto", SqlDbType.Int).Value = IdEnteAccoglienza
            SqlCmd.Parameters.Add("@TipoSoggetto", SqlDbType.VarChar).Value = "SEDE"


            'Esito aggiornamento: 0-Errore 1-Aggiornamento effettuato
            SqlCmd.Parameters.Add("@Valore", SqlDbType.TinyInt)
            SqlCmd.Parameters("@Valore").Direction = ParameterDirection.Output
            'Esito aggiornamento: 0-Errore 1-Aggiornamento effettuato
            SqlCmd.Parameters.Add("@Messaggio", SqlDbType.VarChar)
            SqlCmd.Parameters("@messaggio").Size = 1000
            SqlCmd.Parameters("@Messaggio").Direction = ParameterDirection.Output

            SqlCmd.ExecuteNonQuery()

            blbEsito = SqlCmd.Parameters("@Valore").Value()
            mess = SqlCmd.Parameters("@Messaggio").Value()
            Return blbEsito
        Catch ex As Exception
            msgErrore.Text = ex.Message
        Finally

        End Try
    End Function

    Protected Sub cmdConfermaChiusura_Click(sender As Object, e As EventArgs) Handles cmdConfermaChiusura.Click
        cmdCancella.Visible = True
        divChiusuraSede.Visible = False
        EliminaSede()
    End Sub


    Protected Sub cmdAnnullaChiusura_Click(sender As Object, e As EventArgs) Handles cmdAnnullaChiusura.Click
        cmdCancella.Visible = True
        divChiusuraSede.Visible = False
    End Sub

    Protected Sub CmdPortaInSospeso_Click(sender As Object, e As EventArgs) Handles CmdPortaInSospeso.Click
        RipristinaSedeCancellataPerTransitoSCU()
    End Sub
    Private Sub RipristinaSedeCancellataPerTransitoSCU()
        'txtidsede.Value
        'Ripristino della sede
        CronologiaStatiEntiSedi()
        strsql = " update entisedi set idstatoenteSede=3 " &
                            " where idEnteSede=" & txtidsede.Value & " and idente=" & lblidEnte.Value & ""
        If Not dtrGenerico Is Nothing Then
            dtrGenerico.Close()
            dtrGenerico = Nothing
        End If
        rstGenerico = ClsServer.EseguiSqlClient(strsql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
        If Not dtrGenerico Is Nothing Then
            dtrGenerico.Close()
            dtrGenerico = Nothing
        End If
        CronologiaStatiEntisediAttuazioni()
        strsql = " update entisediattuazioni set idstatoenteSede=3 " &
                " where idEnteSede=" & txtidsede.Value & ""
        If Not dtrGenerico Is Nothing Then
            dtrGenerico.Close()
            dtrGenerico = Nothing
        End If
        rstGenerico = ClsServer.EseguiSqlClient(strsql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))

        lblStatoSede.Text = "SEDE IN STATO DI SOSPESA"
        LoadMaschera(txtidsede.Value)
        'PopolaMaschera(txtidsede.Value)
        'If lblPersonalizza.Value = "Inserimento" Then
        '    Response.Redirect("WfrmMain.aspx")
        'Else
        'Response.Redirect("WfrmRicercaSede.aspx?IdEnteSedeAttuazioneADC=" & dgRisultatoRicerca.Items(0).Cells(1).Text())
        'End If
    End Sub

    Protected Sub imgGoogleMaps_Click(sender As Object, e As ImageClickEventArgs) Handles imgGoogleMaps.Click
        Dim campoObbligatorio As String = "Il campo {0} è obbligatorio.<br>"
        Dim MessaggioErrore As String = "{0} - Indirizzo trovato da Google: [{1}]"
        Dim MessaggioCorretto As String = "L'indirizzo appare corretto - Indirizzo trovato da Google: [{0}]"
        CancellaMessaggi()
        If (ddlProvincia.SelectedItem.Text = String.Empty) Then
            msgErrore.Text = msgErrore.Text + String.Format(campoObbligatorio, "Provincia/Nazione")
        End If
        If (ddlComune.SelectedItem.Text = String.Empty And ChkEstero.Checked = False) Then
            msgErrore.Text = msgErrore.Text + String.Format(campoObbligatorio, "Comune")
        End If
        If (txtIndirizzo.Text = String.Empty) Then
            msgErrore.Text = msgErrore.Text + String.Format(campoObbligatorio, "Indirizzo domicilio")
        End If
        If (txtCivico.Text = String.Empty) Then
            msgErrore.Text = msgErrore.Text + String.Format(campoObbligatorio, "Numero Civico")
        End If
        If (txtCap.Text = String.Empty And ChkEstero.Checked = False) Then
            msgErrore.Text = msgErrore.Text + String.Format(campoObbligatorio, "C.A.P.")
        End If
        Dim gm As New GoogleMaps(ConfigurationSettings.AppSettings("GoogleKey"))

        '**** controllo con le api di google****

        'If msgErrore.Text = "" Then
        '    Dim stato As String
        '    If ChkEstero.Checked Then
        '        stato = RitornaCodiceStato(ddlProvincia.SelectedItem.Text)
        '    Else
        '        stato = "IT"
        '    End If

        '    Dim b As Boolean = gm.checkAddress(stato, txtCity.Text, txtIndirizzo.Text, txtCivico.Text, txtCap.Text)

        '    If b Then
        '        lblGeolocalizzazione.Text = String.Format(MessaggioCorretto, gm.GoogleFormattedAddress)
        '        lblErrGeolocalizzazione.Text = ""
        '        lblGeolocalizzazione.Visible = True
        '        lblErrGeolocalizzazione.Visible = False
        '    Else
        '        lblErrGeolocalizzazione.Text = String.Format(MessaggioErrore, gm.lastError, gm.GoogleFormattedAddress)
        '        lblGeolocalizzazione.Text = ""
        '        lblGeolocalizzazione.Visible = False
        '        lblErrGeolocalizzazione.Visible = True
        '    End If
        '    ShowPopUPControllo = 2
        'End If
        If msgErrore.Text = "" Then
            ShowPopUPControllo = 3
            IndirizzoRicerca = txtIndirizzo.Text & " " & txtCivico.Text & " " & txtCap.Text & " " & ddlComune.SelectedItem.Text & " " & ddlProvincia.SelectedItem.Text
            'Dim script As String = "<SCRIPT>" & vbCrLf & "window.open('" & lnk & "','_blank');" & vbCrLf & "</SCRIPT>"
            Dim script = "<script src=""https://maps.googleapis.com/maps/api/js?key=" & ConfigurationSettings.AppSettings("GoogleKey") & "&callback=initMap&libraries=places&v=weekly"" async></script>"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "IndirizzoGoogle", script)

        End If

    End Sub
    Public Function ControllaNomeIndirizzoSede(ByRef Messaggio As String) As Integer
        Dim SqlCmd As SqlClient.SqlCommand
        Dim test2 As Integer
        Dim test As String
        test = Session("IdEnte")
        test2 = CInt(Session("IdEnte"))
        Messaggio = ""
        Try
            SqlCmd = New SqlClient.SqlCommand
            SqlCmd.CommandText = "ControllaNomeIndirizzoSede"
            SqlCmd.CommandType = CommandType.StoredProcedure
            SqlCmd.Connection = Session("Conn")
            SqlCmd.Parameters.Add("@IdEnte", SqlDbType.Int).Value = CInt(Session("IdEnte"))




            SqlCmd.Parameters.Add("@NomeSede", SqlDbType.VarChar, 50).Value = txtdenominazione.Text
            If lblPersonalizza.Value = "Inserimento" Then
                SqlCmd.Parameters.Add("@idEnteSede", SqlDbType.Int).Value = 0
            Else
                SqlCmd.Parameters.Add("@idEnteSede", SqlDbType.Int).Value = CInt(Session("IdEnteSede"))
            End If
            SqlCmd.Parameters.Add("@Indirizzo", SqlDbType.VarChar, 255).Value = txtIndirizzo.Text
            SqlCmd.Parameters.Add("@Civico", SqlDbType.VarChar, 50).Value = txtCivico.Text
            SqlCmd.Parameters.Add("@Cap", SqlDbType.VarChar, 10).Value = txtCap.Text
            SqlCmd.Parameters.Add("@IdComune", SqlDbType.Int).Value = ddlComune.SelectedValue
            SqlCmd.Parameters.Add("@Palazzina", SqlDbType.VarChar, 10).Value = TxtPalazzina.Text
            SqlCmd.Parameters.Add("@Scala", SqlDbType.VarChar, 10).Value = TxtScala.Text
            SqlCmd.Parameters.Add("@Piano", SqlDbType.Int).Value = TxtPiano.Text
            SqlCmd.Parameters.Add("@Interno", SqlDbType.VarChar, 10).Value = TxtInterno.Text
            Dim sparam1 As SqlClient.SqlParameter
            sparam1 = New SqlClient.SqlParameter
            sparam1.ParameterName = "@Esito"
            sparam1.SqlDbType = SqlDbType.Int
            sparam1.Direction = ParameterDirection.Output
            SqlCmd.Parameters.Add(sparam1)

            Dim sparam2 As SqlClient.SqlParameter
            sparam2 = New SqlClient.SqlParameter
            sparam2.ParameterName = "@Messaggio"
            sparam2.SqlDbType = SqlDbType.VarChar
            sparam2.Size = 255
            sparam2.Direction = ParameterDirection.Output
            SqlCmd.Parameters.Add(sparam2)

            SqlCmd.ExecuteNonQuery()
            If SqlCmd.Parameters("@Esito").Value > 0 Then
                Messaggio = SqlCmd.Parameters("@Messaggio").Value
            End If
            Return SqlCmd.Parameters("@Esito").Value

        Catch ex As Exception
            Messaggio = "Errore nel controllo formale di nome e indirizzo"
            Return 4
        End Try
    End Function

    Protected Sub cmdAllega_Click(sender As Object, e As EventArgs) Handles cmdAllega.Click
        'Verifica se è stato inserito il file
        CancellaMessaggi()
        If fileLSE.PostedFile Is Nothing Or String.IsNullOrEmpty(fileLSE.PostedFile.FileName) Then
            MessaggiPopup("Non è stato scelto nessun file per il caricamento della Lettera di accordo con sede estera")
            Exit Sub
        End If
        'Controllo Tipo File
        If clsGestioneDocumentiAccreditamento.VerificaEstensioneFileAccreditamento(fileLSE) = False Then
            MessaggiPopup("Il formato file della Lettera di accordo con sede estera non è corretto. È possibile associare documenti nel formato .PDF o .PDF.P7M")
            Exit Sub
        End If
        'Controlli dimensioni del file
        Dim fs = fileLSE.PostedFile.InputStream
        Dim iLen As Integer = CInt(fs.Length)
        Dim bBLOBStorage(iLen) As Byte

        If iLen <= 0 Then
            MessaggiPopup("Attenzione. Impossibile caricare documento vuoto.")
            Exit Sub
        End If
        If iLen > 20971520 Then
            MessaggiPopup("Attenzione. La dimensione massima della Lettera di accordo con sede estera è di 20 MB.")
            Exit Sub
        End If
        Dim numBytesToRead As Integer = CType(fs.Length, Integer)
        Dim numBytesRead As Integer = 0

        While (numBytesToRead > 0)
            ' Read may return anything from 0 to numBytesToRead.
            Dim n As Integer = fs.Read(bBLOBStorage, numBytesRead, numBytesToRead)
            ' Break when the end of the file is reached.
            If (n = 0) Then
                Exit While
            End If
            numBytesRead = (numBytesRead + n)
            numBytesToRead = (numBytesToRead - n)

        End While

        fs.Close()

        Dim hashValue As String
        hashValue = GeneraHash(bBLOBStorage)
        Dim esito As Boolean
        esito = ControllaHV(hashValue)
        If esito Then
            Session("LoadLSEId") = Nothing
            'Salvo File In Sessione
            Dim LSE As New Allegato() With {
             .Id = Session("LoadLSEId"),
             .Updated = True,
             .Blob = bBLOBStorage,
             .Filename = fileLSE.PostedFile.FileName,
             .Hash = hashValue,
             .Filesize = iLen,
            .DataInserimento = Date.Now
            }
            Session("LoadedLSE") = LSE
            'Se Il LSE è caricato in Sessione (Inserimento)
            rowNoLSE.Visible = False
            rowLSE.Visible = True
            txtLSEFilename.Text = LSE.Filename
            txtLSEHash.Text = LSE.Hash
            txtLSEData.Text = LSE.DataInserimento.ToString("dd/MM/yyyy HH:mm:ss")
            MessaggiSuccess("Lettera di accordo con sede estera caricata correttamente")
        Else
            msgErrore.Text = "Attenzione. File già presente per questo ente"
        End If

    End Sub


    Private Sub MessaggiPopup(ByVal strMessaggio)
        lblErroreUpload.Visible = True
        lblErroreUpload.Text = strMessaggio
        popUpload.Show()
    End Sub

    Private Function GeneraHash(ByVal FileinByte() As Byte) As String
        Dim tmpHash() As Byte

        tmpHash = New MD5CryptoServiceProvider().ComputeHash(FileinByte)

        GeneraHash = ByteArrayToString(tmpHash)
        Return GeneraHash
    End Function
    Private Function ByteArrayToString(ByVal arrInput() As Byte) As String
        Dim i As Integer
        Dim sOutput As New StringBuilder(arrInput.Length)
        For i = 0 To arrInput.Length - 1
            sOutput.Append(arrInput(i).ToString("X2"))
        Next
        Return sOutput.ToString()
    End Function
    Private Sub MessaggiSuccess(ByVal strMessaggio)
        msgErrore.Visible = True
        msgErrore.CssClass = "msgInfo"
        msgErrore.Text = strMessaggio
    End Sub

    Protected Sub cmdProcedi_Click(sender As Object, e As EventArgs) Handles cmdProcedi.Click
        Session("Procedi") = 1
        SalvaDati()
    End Sub
    Private Sub SalvaDati()
        Dim AlboEnte, ListaAnomalie As String
        AlboEnte = ClsUtility.TrovaAlboEnte(Session("IdEnte"), Session("Conn"))
        CancellaMessaggi()
        indirizzoErratoHelios = False
        If (VerificaValiditaCampi(AlboEnte) Or indirizzoErratoHelios = True) Then
            Dim esito As Integer
            '**** Controllo la plausibilità di nome e indirizzo e in caso mostro il popup con le segnalazioni non bloccanti
            If Session("Procedi") = 0 And Session("ProcediSalva") = 0 Then
                esito = ControllaNomeIndirizzoSede(ListaAnomalie)
                Select Case esito
                    Case 0
                        Session("AnomaliaNome") = 0
                        Session("AnomaliaIndirizzo") = 0
                    Case 1
                        Session("AnomaliaNome") = 1
                        Session("AnomaliaIndirizzo") = 0
                    Case 2
                        Session("AnomaliaNome") = 0
                        Session("AnomaliaIndirizzo") = 1
                    Case 3
                        Session("AnomaliaNome") = 1
                        Session("AnomaliaIndirizzo") = 1
                    Case Else
                        'inserire cosa fare se va in errore il controllo nome indirizzo
                End Select

                If esito = 1 Or esito = 3 Then 'se risultano anomalie per il nome
                    ShowPopUPControllo = "1"
                    ListaAnomalie = Replace(ListaAnomalie, "|", "<br/>")
                    lblErroreControlloSede.Text = ListaAnomalie
                    lblErroreControlloSede.Visible = True
                    divSpiegazioni.Visible = True
                    'If ChkEstero.Checked = True Then
                    '    rowLSE.Visible = True
                    'End If
                    Exit Sub
                End If
                'If esito = 2 Or esito = 3 Then
                '    msgErrore.Text = msgErrore.Text + "Attenzione: a questo indirizzo risultano già presenti altre sedi<br/>"
                '    Exit Sub
                'End If
            Else
                ShowPopUPControllo = ""
                Session("Procedi") = 0
                lblErroreControlloSede.Text = ""
                lblErroreControlloSede.Visible = False
            End If

            '*****controlla l'indirizzo su googlemaps solo nel caso in cui viene rilevato un errore di indirizzo
            'If esito = 4 Or (Session("ProcediGM") = 0 And indirizzoErratoHelios) Then
            '    Dim campoObbligatorio As String = "Il campo {0} è obbligatorio.<br>"
            '    'Dim MessaggioErrore As String = "{0} - Indirizzo trovato da Google: [{1}]"
            '    Dim MessaggioErroreNonTrovato As String = "Google non ha trovato questo indirizzo."
            '    Dim MessaggioErroreDiverso As String = "Google suggerisce questo altro indirizzo:<br/>{0}.<br/>"
            '    Dim MessaggioCorretto As String = "L'indirizzo appare corretto - Indirizzo trovato da Google: [{0}]"
            '    CancellaMessaggi()
            '    If (ddlProvincia.SelectedItem.Text = String.Empty) Then
            '        msgErrore.Text = msgErrore.Text + String.Format(campoObbligatorio, "Provincia/Nazione")
            '    End If

            '    If (ddlComune.SelectedItem.Text = String.Empty And ChkEstero.Checked = False) Then
            '        msgErrore.Text = msgErrore.Text + String.Format(campoObbligatorio, "Comune")
            '    End If
            '    If (txtIndirizzo.Text = String.Empty) Then
            '        msgErrore.Text = msgErrore.Text + String.Format(campoObbligatorio, "Indirizzo domicilio")
            '    End If
            '    If (txtCivico.Text = String.Empty) Then
            '        msgErrore.Text = msgErrore.Text + String.Format(campoObbligatorio, "Numero Civico")
            '    End If
            '    If (txtCap.Text = String.Empty And ChkEstero.Checked = False) Then
            '        msgErrore.Text = msgErrore.Text + String.Format(campoObbligatorio, "C.A.P.")
            '    End If

            '    Dim gm As New GoogleMaps(ConfigurationSettings.AppSettings("GoogleKey"))
            '    Dim stato As String
            '    If ChkEstero.Checked Then
            '        stato = RitornaCodiceStato(ddlProvincia.SelectedItem.Text)
            '    Else
            '        stato = "IT"
            '    End If

            '    Dim localita As String = ddlComune.SelectedItem.Text
            '    If localita = "" Then localita = txtCity.Text
            '    If ChkEstero.Checked Then localita = txtCity.Text
            '    Dim b As Boolean = gm.checkAddress(stato, localita, txtIndirizzo.Text, txtCivico.Text, txtCap.Text)

            '    '    'If b Then
            '    '    lblGeolocalizzazione.Text = String.Format(MessaggioCorretto, gm.GoogleFormattedAddress)
            '    '    lblErrGeolocalizzazione.Text = ""
            '    '    lblGeolocalizzazione.Visible = True
            '    '    lblErrGeolocalizzazione.Visible = False
            '    'Else
            '    If b = False Then
            '        'If gm.GoogleFormattedAddress = "" Then
            '        '    lblErrGeolocalizzazione.Text = gm.lastError
            '        'Else
            '        '    lblErrGeolocalizzazione.Text = gm.lastError & "<br>" & String.Format(MessaggioErroreDiverso, gm.GoogleFormattedAddress)
            '        'End If

            '        'lblGeolocalizzazione.Text = ""
            '        'lblGeolocalizzazione.Visible = False
            '        'lblErrGeolocalizzazione.Visible = True

            '        ShowPopUPControllo = 2
            '        Session("AnomaliaIndirizzoGM") = 1
            '        'If ChkEstero.Checked = True Then
            '        '    rowLSE.Visible = True
            '        'End If
            '        lblGeolocalizzazione.Text = ""
            '        lblGeolocalizzazione.Visible = False
            '        Dim messaggioGoogle As String = "<Strong>ATTENZIONE :</strong></br>L’indirizzo digitato non trova riscontro in Google Maps. Si prega di controllare la correttezza di quanto inserito.</br>In caso di permanenza dell’anomalia, il Dipartimento si riserva di effettuare tutti i successivi controlli di merito.</strong>"
            '        If gm.GoogleFormattedAddress <> "" Then messaggioGoogle &= "<br>Google Maps suggerisce: <b>" & gm.GoogleFormattedAddress & "</b>"
            '        lblErrGeolocalizzazione.Text = messaggioGoogle
            '        lblErrGeolocalizzazione.Visible = True
            '        Exit Sub
            '    Else
            '        lblErrGeolocalizzazione.Text = ""
            '        lblGeolocalizzazione.Text = ""
            '        lblGeolocalizzazione.Visible = True
            '        lblErrGeolocalizzazione.Visible = False
            '        ShowPopUPControllo = 0
            '        esito = 0
            '        Session("AnomaliaIndirizzoGM") = 0
            '        Me.indirizzoOkGoogle = True
            '    End If
            'Else
            '    indirizzoOkGoogle = True
            '    Session("ProcediGM") = 0
            '    lblErrGeolocalizzazione.Text = ""
            '    lblGeolocalizzazione.Text = ""
            '    lblGeolocalizzazione.Visible = False
            '    lblErrGeolocalizzazione.Visible = False
            '    ShowPopUPControllo = 0
            'End If
            If Session("ProcediSalva") = 0 Then
                ShowPopUPControllo = 4
                Exit Sub
            End If

            Dim strtab As Integer
            strtab = cmdSalva.TabIndex

            'If lblPersonalizza.Value = "Inserimento" Then
            '    cmdSalva.Visible = False
            '    Session("IdComune") = Nothing
            '    Inserimento()
            '    'dgRisultatoRicerca.Visible = True

            'Else 'Fase di Modifica della sede 
            If txtemail.ReadOnly = False Then
                Modifica()
                PersonalizzaTasti()
                RicercaSediAttuazione(txtidsede.Value)
                If (Session("TipoUtente") = "U" Or Session("TipoUtente") = "R") Then
                    CaricaDatiEntiSediAttuazione(txtidsede.Value)
                End If

            End If
            SettaValori(txtidsede.Value)
            EvidenziaDatiModificati(txtidsede.Value)
        End If
        'End If
    End Sub
    Private Function RitornaCodiceStato(Nome As String) As String
        Dim TCodici As DataTable
        Dim SqlCmd As New SqlCommand
        Dim dataAdapter As SqlDataAdapter = New SqlDataAdapter
        TCodici = New DataTable
        RitornaCodiceStato = ""

        SqlCmd.CommandText = "SELECT Codice FROM nazioni WHERE  (Nazione = '" & Nome & "')"
        SqlCmd.CommandType = CommandType.Text
        SqlCmd.Connection = Session("conn")
        dataAdapter.SelectCommand = SqlCmd
        dataAdapter.Fill(TCodici)
        If TCodici.Rows.Count > 0 Then
            RitornaCodiceStato = TCodici(0).Item("Codice").ToString
        End If
    End Function

    Protected Sub cmdProcediGeolocalizzazione_Click(sender As Object, e As EventArgs) Handles cmdProcediGeolocalizzazione.Click
        Session("ProcediGM") = 1
        procediGM = 1
        SalvaDati()
    End Sub
    Private Sub CaricaLSE()
        '***Gestione LSE****
        If Session("LoadLSEId") IsNot Nothing Then
            'Se Il LSE è caricato nel DB
            'Recupero File da DB
            Dim sqlGetAllegato = "SELECT Top 1 FileName,HashValue,FileLength,BinData,DataInserimento From Allegato WHERE idAllegato = @idAllegato"
            Dim AllegatoCommand As New SqlCommand(sqlGetAllegato, Session("conn"))
            AllegatoCommand.Parameters.AddWithValue("@idAllegato", Session("LoadLSEId"))
            Dim dtrAllegato As SqlDataReader = AllegatoCommand.ExecuteReader()
            If dtrAllegato.Read Then
                Dim filename As String = dtrAllegato("FileName")
                Dim hashValue As String = dtrAllegato("HashValue")
                Dim filelength As Integer = CInt(dtrAllegato("FileLength"))
                Dim blob As Byte() = dtrAllegato("BinData")
                Dim dataInserimento As Date = dtrAllegato("DataInserimento")
                dtrAllegato.Close()
                AllegatoCommand.Dispose()
                rowNoLSE.Visible = False
                rowLSE.Visible = True
                txtLSEFilename.Text = filename
                txtLSEData.Text = dataInserimento.ToString("dd/MM/yyyy HH:mm:ss")
                Session("LoadedLSE") = New Allegato() With {
                 .Id = Session("LoadLSEId"),
                 .Blob = blob,
                 .Filename = filename,
                 .Hash = hashValue,
                 .Filesize = filelength,
                 .DataInserimento = dataInserimento
                }

            Else
                dtrAllegato.Close()
                ' ClearSessionLSE()
            End If
        End If
        Dim LSE As Allegato = Session("LoadedLSE")

        If LSE IsNot Nothing Then
            'Se Il LSE è caricato in Sessione (Inserimento)
            rowNoLSE.Visible = False
            rowLSE.Visible = True
            txtLSEFilename.Text = LSE.Filename
            txtLSEHash.Text = LSE.Hash
            txtLSEData.Text = LSE.DataInserimento.ToString("dd/MM/yyyy HH:mm:ss")
        Else
            'Se LSE non è ancora caricato
            rowNoLSE.Visible = True
            rowLSE.Visible = False
        End If
    End Sub

    Private Sub ClearSessionLSE()
        Session("LoadedLSE") = Nothing
        'Session("LoadLSEId") = Nothing
        rowNoLSE.Visible = True
        rowLSE.Visible = False
    End Sub


    Protected Sub btnEliminaLSE_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles btnEliminaLSE.Click
        ClearSessionLSE()
    End Sub

    Protected Sub btnDownloadLSE_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles btnDownloadLSE.Click
        If Session("LoadedLSE") Is Nothing Then
            msgErrore.Text = "Nessun File caricato"
            ClearSessionLSE()
            Log.Warning(LogEvent.SEDI_DOWNLOAD_LSE, "Nessun file Caricato")
            Exit Sub
        End If
        Log.Information(LogEvent.SEDI_DOWNLOAD_LSE)
        Dim LSE As Allegato = Session("LoadedLSE")
        Response.Clear()
        Response.ContentType = "Application/pdf"
        Response.AddHeader("Content-Disposition", "attachment; filename=" & LSE.Filename)
        Response.BinaryWrite(LSE.Blob)
        Response.End()
    End Sub

    Function ControllaHV(hvControllo As String) As Boolean
        'ControllaHV =
        Dim esito As Integer = 0
        'Recupero File da DB
        Try
            Dim SqlCmd As SqlClient.SqlCommand


            SqlCmd = New SqlClient.SqlCommand
            SqlCmd.CommandText = "Controlla_HashValue"
            SqlCmd.CommandType = CommandType.StoredProcedure
            SqlCmd.Connection = Session("Conn")
            SqlCmd.Parameters.Add("@Hashvalue", SqlDbType.VarChar, 100).Value = hvControllo
            SqlCmd.Parameters.Add("@IdEnte", SqlDbType.Int).Value = Session("IdEnte")

            Dim sparam As SqlClient.SqlParameter
            sparam = New SqlClient.SqlParameter
            sparam.ParameterName = "@Esito"
            sparam.SqlDbType = SqlDbType.TinyInt
            sparam.Direction = ParameterDirection.Output
            SqlCmd.Parameters.Add(sparam)

            SqlCmd.ExecuteNonQuery()
            If SqlCmd.Parameters("@Esito").Value() = 1 Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            Return False
        End Try


    End Function

    Private Function getRegioneCompetenzaEnte() As Integer
        Dim strSql = "select t2.idRegioneCompetenza from enti t1 inner join sezionialboscu t2 on t2.idSezione = t1.idSezione where t1.idEnte=" & Session("IdEnte")
        Dim dtrLocal As SqlClient.SqlDataReader = ClsServer.CreaDatareader(strSql, Session("conn"))
        getRegioneCompetenzaEnte = 0
        If dtrLocal.HasRows = True Then
            dtrLocal.Read()
            getRegioneCompetenzaEnte = dtrLocal("idRegioneCompetenza")
        End If
        If Not dtrLocal Is Nothing Then
            dtrLocal.Close()
            dtrLocal = Nothing
        End If
    End Function
    Private Function getRegioneCompetenzaComune(ByVal IDComune As Integer) As Integer
        ' IDComune = Replace(codiceIstat, "'", "")
        'Dim strSql = "select coalesce(IDRegione, -1) IdRegione from comuni t1 inner join provincie t2 on t2.idprovincia=t1.idprovincia where t1.IDComune='" & IDComune & "'"
        Dim strSql = "select coalesce(idRegioneCompetenza, -1) idRegioneCompetenza from comuni t1 inner join provincie t2 on t2.idprovincia=t1.idprovincia where t1.IDComune=" & IDComune
        Dim dtrLocal As SqlClient.SqlDataReader = ClsServer.CreaDatareader(strSql, Session("conn"))
        getRegioneCompetenzaComune = 0
        If dtrLocal.HasRows = True Then
            dtrLocal.Read()
            getRegioneCompetenzaComune = dtrLocal("idRegioneCompetenza")
        End If
        If Not dtrLocal Is Nothing Then
            dtrLocal.Close()
            dtrLocal = Nothing
        End If
    End Function
    Sub DisabilitaCampo(controllo As WebControl)
        controllo.Enabled = False
        controllo.Style.Add("color", "lightgrey")
    End Sub

    Function VerificAltriCampi()
        Dim utility As ClsUtility = New ClsUtility()
        Dim campiValidi As Boolean = True
        Dim numero As Int32
        Dim dataTmp As Date
        Dim campoObbligatorio As String = "Il campo {0} è obbligatorio.<br/>"
        Dim campoObbligatorioSCU As String = "Il campo {0} è obbligatorio."
        Dim campoObbligatorioSCU1 As String = "Indicare {0} nel caso non sia previsto.<br/>"
        Dim numeroNonValido As String = "Il valore di '{0}' non è valido. Inserire solo numeri.<br/>"
        Dim emailNonValida As String = "Il valore di '{0}' non è valido. Inserire un indirizzo email valido.<br/>"
        Dim LungezzaErrata As String = "Il campo {0} può contenere massimo 5 caratteri.<br/>"
        Dim messaggioDataValida As String = "Il valore di '{0}' non è valido. Inserire la data nel formato gg/mm/aaaa.<br/>"
        Dim CittaEstera As String = "Il campo {0} è obbligatorio."
        Dim NomeAnomalo As String = "Il campo {0} è obbligatorio."
        Dim IndirizzoAnomalo As String = "Il campo {0} è obbligatorio."
        Dim ListaAnomalie As String = ""
        Dim CheckIndirizzo As Boolean = True
        Dim regioneCompetenza As Integer = getRegioneCompetenzaEnte()
        If (ddlProvincia.SelectedItem.Text = String.Empty) Then
            msgErrore.Text = msgErrore.Text + String.Format(campoObbligatorio, "Provincia/Nazione")
            campiValidi = False
            CheckIndirizzo = False
        End If

        If (ddlComune.SelectedItem.Text = String.Empty) Then
            'If ChkEstero.Checked Then
            '    msgErrore.Text = msgErrore.Text + String.Format(campoObbligatorio, "Localit&agrave;")
            'Else
            If ChkEstero.Checked = False Then
                msgErrore.Text = msgErrore.Text + String.Format(campoObbligatorio, "Comune")
                campiValidi = False
                CheckIndirizzo = False
            End If

        End If
        If (txtIndirizzo.Text = String.Empty) Then
            msgErrore.Text = msgErrore.Text + String.Format(campoObbligatorio, "Indirizzo domicilio")
            campiValidi = False
            CheckIndirizzo = False
        End If
        If (txtCivico.Text = String.Empty) Then
            msgErrore.Text = msgErrore.Text + String.Format(campoObbligatorio, "Numero Civico")
            campiValidi = False
            CheckIndirizzo = False
        End If
        If (txtCap.Text = String.Empty And ChkEstero.Checked = False) Then
            msgErrore.Text = msgErrore.Text + String.Format(campoObbligatorio, "C.A.P.")
            campiValidi = False
            CheckIndirizzo = False
        End If
        'If (TxtPiano.Text <> String.Empty) Then

        '    campiValidi = ValidaInteri(TxtPiano.Text, "Piano")
        'End If
        TxtScala.Text = "ND"
        TxtPiano.Text = "0"
        TxtInterno.Text = "ND"
        If (TxtPiano.Text <> String.Empty) Then
            If (Int32.TryParse(TxtPiano.Text, numero) = False) Then
                msgErrore.Text = msgErrore.Text + String.Format(numeroNonValido, "Piano")
                campiValidi = False
            End If
        End If

        If Len(TxtPalazzina.Text) > 5 Then
            msgErrore.Text = msgErrore.Text + String.Format(LungezzaErrata, "Palazzina")
            campiValidi = False

        End If

        If Len(TxtScala.Text) > 5 Then
            msgErrore.Text = msgErrore.Text + String.Format(LungezzaErrata, "Scala")
            campiValidi = False

        End If
        If Len(TxtInterno.Text) > 5 Then
            msgErrore.Text = msgErrore.Text + String.Format(LungezzaErrata, "Interno")
            campiValidi = False

        End If

        If AlboEnte = "SCU" Then
            If (TxtPalazzina.Text = String.Empty) Then
                msgErrore.Text = msgErrore.Text + String.Format(campoObbligatorioSCU, "Palazzina")
                msgErrore.Text = msgErrore.Text + String.Format(campoObbligatorioSCU1, "ND")
                campiValidi = False
            End If
            If (TxtScala.Text = String.Empty) Then
                msgErrore.Text = msgErrore.Text + String.Format(campoObbligatorioSCU, "Scala")
                msgErrore.Text = msgErrore.Text + String.Format(campoObbligatorioSCU1, "ND")
                campiValidi = False
            End If
            If (TxtPiano.Text = String.Empty) Then
                msgErrore.Text = msgErrore.Text + String.Format(campoObbligatorioSCU, "Piano")
                msgErrore.Text = msgErrore.Text + String.Format(campoObbligatorioSCU1, "0")
                campiValidi = False
            End If
            If (TxtInterno.Text = String.Empty) Then
                msgErrore.Text = msgErrore.Text + String.Format(campoObbligatorioSCU, "Interno")
                msgErrore.Text = msgErrore.Text + String.Format(campoObbligatorioSCU1, "ND")
                campiValidi = False
            End If
        End If
        Dim strMiaCausale As String = ""
        If ddlComune.SelectedValue <> "0" And CheckIndirizzo = True And Not indirizzoOkGoogle And Not procediGM Then
            If ClsUtility.CAP_VERIFICA(IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")),
                strMiaCausale, bandiera, Trim(txtCap.Text), ddlComune.SelectedValue, "", "", txtIndirizzo.Text, txtCivico.Text) = False Then
                indirizzoErratoHelios = True
                'Ripristino lo stato del tasto
                cmdSalva.Visible = True
                'Inserisco il Messaggio di Errore
                msgErrore.Text = strMiaCausale + "</br>"
                If Not dtrGenerico Is Nothing Then
                    dtrGenerico.Close()
                    dtrGenerico = Nothing

                End If
                Log.Warning(LogEvent.SEDI_INSERIMENTO_ERRORE, strMiaCausale)
                'campiValidi = False
            End If
        Else

            If ChkEstero.Checked And ddlComune.SelectedValue <= 0 Then
                ddlComune.ClearSelection()
                ddlComune.Items.FindByText(ddlProvincia.SelectedItem.Text).Selected = True
            End If

        End If
        If (txtPrefFax.Text <> String.Empty) Then
            If ValidaInteri(txtPrefFax.Text, "Prefisso Fax") = False Then
                'msgErrore.Text = msgErrore.Text + String.Format(numeroNonValido, "Prefisso Fax")
                campiValidi = False
            End If
        End If
        If (txtfax.Text <> String.Empty) Then
            If ValidaInteri(txtfax.Text, "Fax") = False Then
                'msgErrore.Text = msgErrore.Text + String.Format(numeroNonValido, "Fax")
                campiValidi = False
            End If

        End If


        If (ddlTitoloGiuridico.SelectedItem.Value <= 0) Then
            msgErrore.Text = msgErrore.Text + String.Format(campoObbligatorio, "Titolo di disponibilità")
            campiValidi = False
        End If
        If ChkEstero.Checked = False And ddlComune.SelectedValue <> 0 Then
            If regioneCompetenza <> 22 AndAlso regioneCompetenza <> getRegioneCompetenzaComune(ddlComune.SelectedValue) Then
                msgErrore.Text = msgErrore.Text + "Il comune non appartiene alla regione di competenza.</br>"
                campiValidi = False
            End If

        End If

        If (txtemail.Text <> String.Empty) Then
            If ClsUtility.IsValidEmail(txtemail.Text) = False Then
                msgErrore.Text = msgErrore.Text + String.Format(emailNonValida, "e-mail")
                campiValidi = False
            End If
        End If
        If ChkEstero.Checked = True Then
            If (Trim(txtCity.Text) = String.Empty) Then

                msgErrore.Text = msgErrore.Text + String.Format(CittaEstera, "Città Estera")
                campiValidi = False

            End If
        End If
        If ChkEstero.Checked = True Then
            If (ddlProvincia.SelectedItem.Text = String.Empty) Then
                msgErrore.Text = msgErrore.Text + String.Format(campoObbligatorio, "Provincia/Nazione")
                campiValidi = False
            End If
        End If

        If ChkEstero.Checked Then
            If ddlTitoloGiuridico.SelectedValue = 7 Then
                Dim LSE As Allegato = Session("LoadedLSE")
                If LSE Is Nothing Then
                    msgErrore.Text = msgErrore.Text + "L'allegato Lettera di accordo con sede estera è obbligatorio<br/>"
                    campiValidi = False
                End If
                If TxtSoggettoCapoSede.Text = "" Then
                    msgErrore.Text = msgErrore.Text + String.Format(campoObbligatorio, "Soggetto estero cui è in capo la Sede")
                    '   "Il campo Soggetto estero cui è in capo la Sede è Obbligatorio<br/>"
                    campiValidi = False
                End If
                If rbSE1.Checked = False And rbSE2.Checked = False Then
                    msgErrore.Text = msgErrore.Text + "La dichiarazione di Non Disponibilità della sede è obbligatoria<br/>"
                    campiValidi = False
                End If
            Else
                If rbNoSE1.Checked = False And rbNoSE2.Checked = False Then
                    msgErrore.Text = msgErrore.Text + "La dichiarazione di avere nella proria disponibilità la sede è obbligatoria<br/>"
                    campiValidi = False
                End If

            End If
        Else
            If ChkTutela.Checked = False Then
                msgErrore.Text = msgErrore.Text + "La dichiarazione di rispetto della normativa a tutela della salute è obbligatoria</br>"
                campiValidi = False
            End If
        End If

        Return campiValidi


    End Function

    Private Sub cmdProcediSalvataggio_Click(sender As Object, e As EventArgs) Handles cmdProcediSalvataggio.Click
        Session("ProcediSalva") = 1
        SalvaDati()
    End Sub

    Private Sub ddlEntiFigli_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlEntiFigli.SelectedIndexChanged
        If ddlEntiFigli.SelectedItem.Text <> "Sede Propria" Then
            chkRiferimentoRimborsi.Visible = False
            IDEnteProprietario.Value = ddlEntiFigli.SelectedValue
        Else
            chkRiferimentoRimborsi.Visible = True
            IDEnteProprietario.Value = Session("IdEnte")
        End If
    End Sub
End Class