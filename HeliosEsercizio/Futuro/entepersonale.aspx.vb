Imports System.Data.SqlClient
Imports System.Drawing
Imports System.IO
Imports System.Security.Cryptography
Imports System.Text.RegularExpressions.Regex
Imports Logger.Data
Imports System.Collections.Generic

Public Class entepersonale
    Inherits SmartPage
    Dim dtrgenerico As SqlClient.SqlDataReader
    Dim bandiera As Integer
    Dim strsql As String
    Dim strIdEntefase As String
    Dim LogParametri As New Hashtable
    Dim MyTransaction As System.Data.SqlClient.SqlTransaction


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        checkSpid()
        'Classe di default dei messaggi di errore
        lblErr.Visible = False
        lblErroreUpload.Visible = False
        rowAllegato.Style.Remove("background-color")
        'variabili che mi consentiranno di creare via codice la datagrid dei ruoli
        Dim dtR As DataRow
        Dim dtT As New DataTable
        Dim strsql As String
        'variabile contatore
        Dim i As Integer

        Dim abilitato As Integer
        Dim dati As Integer
        Dim annullamodifica As Integer
        Dim visualizzadatiaccreditati As Integer
        Dim annullacancellazione As Integer
        Dim cancellazione As Integer
        Dim messaggio As String


        'controllo se è stato effettuato il login
        If Not Session("LogIn") Is Nothing Then
            'se non è stato effettuato login
            If Session("LogIn") = False Then
                'carico la pagina LogOut dove svuoto eventuali session aperte
                Response.Redirect("LogOn.aspx")
            End If
        Else
            'carico la pagina LogOut dove svuoto eventuali session aperte
            Response.Redirect("LogOn.aspx")
        End If

        Dim _cE As New clsEnte
        strIdEntefase = _cE.RicavaIdEnteFase(Session("IdEnte"), Session("Conn"))


        If Request.QueryString("Denominazione") Is Nothing Then

            lgContornoPagina.InnerText = "Risorsa Ente"
        Else

            lgContornoPagina.InnerText = "Risorsa Ente - " & Request.QueryString("Denominazione")
        End If

        If (Request.QueryString("Acquisito") = "Vero") Then
            lblInfoRisorsa.Text = "Risorsa Acquisita"
        End If

        If (Request.QueryString("Cancellato") = "Vero") Then
            lblInfoRisorsa.Text = "Risorsa Cancellata"
        End If

        IdVal1.Value = Request.Form("IdVal1")




        If Page.IsPostBack = False Then

            Dim SelComune As New clsSelezionaComune
            Dim blnEstero As Boolean = False
            SelComune.CaricaProvinciaNazione(ddlProvinciaNascita, blnEstero, Session("Conn"))
            SelComune.CaricaProvinciaNazione(ddlProvinciaResidenza, blnEstero, Session("Conn"))
        End If

        If Not Session("entePersonaleMostraInfo") Is Nothing Then
            Session("entePersonaleMostraInfo") = Nothing
            alertTitolo.InnerText = "Informazione"
            lblAlert.Text = "Si ricorda che in caso di modifica di un responsabile è necessario caricare il documento relativo alla nomina della nuova Struttura di Gestione nella ""Sezione Ente Titolare""."
            mpeALERT.Show()
        End If

        Dim strATTIVITA As Integer = -1
        Dim strBANDOATTIVITA As Integer = -1
        Dim strENTEPERSONALE As Integer = -1
        Dim strENTITA As Integer = -1
        Dim strIDENTE As Integer = -1

        If Request.QueryString("intPersonaleEnteAssociato") <> "" Then
            If ClsUtility.SICUREZZA_VERIFICA_AUTORIZZAZIONI(Session("conn"), Session("IdEnte"), Session("txtCodEnte"), strATTIVITA, strBANDOATTIVITA, Request.QueryString("intPersonaleEnteAssociato"), strENTITA, strIDENTE) = 1 Then
                If Not dtrgenerico Is Nothing Then
                    dtrgenerico.Close()
                    dtrgenerico = Nothing
                End If
            Else
                If Not dtrgenerico Is Nothing Then
                    dtrgenerico.Close()
                    dtrgenerico = Nothing
                End If
                Log.Warning(LogEvent.HELIOS_ERRORE_AUTORIZZAZIONE, parameters:=Request.QueryString)
                Response.Redirect("wfrmAnomaliaDati.aspx")
            End If
        End If

        If Page.IsPostBack = False Then
            Session("LoadCVId") = Nothing
            Session("IdEnteFaseArt") = Nothing

            'aggiunto il 19/07/2017 da s.c.
            Dim AlboEnte As String
            AlboEnte = ClsUtility.TrovaAlboEnte(Session("IdEnte"), Session("Conn"))

            '    LoadMaschera()
            'CaricaCombo()
            '1. RICHIAMO STORE CHE VERIFICA L'ACCESSO MASCHERA DELL' ENTE
            Accesso_Maschera_Risorsa(Session("TipoUtente"), Session("IDEnte"), IIf(Request.QueryString("intPersonaleEnteAssociato") = Nothing, 0, Request.QueryString("intPersonaleEnteAssociato")), abilitato, dati, annullamodifica, annullacancellazione, visualizzadatiaccreditati, messaggio, cancellazione)

            '2.PERSONALIZZO CAMPI NON VISIBILI SE SONO ENTE O UNSC/REGIONE
            PersonalizzoNoVisibilitaCampi(AlboEnte)

            '3.ABILITO/DISABILITO MASCHERA SE E' MODIFICABILE O IN LETTERA
            If abilitato = 0 Then ' -- 0: maschera sola lettura		1: maschera in modifica
                CampiDisabilitati()
            End If


            If Session("ModIns") = 1 Then 'modifica   
                'CARICA DATI
                CaricaAnagrafica()
                CaricaRuoliModifica(Request.QueryString("intPersonaleEnteAssociato"), AlboEnte)

                If messaggio <> "" Then
                    lblErr.Visible = True
                    lblErr.Text = messaggio
                End If
            Else
                'PersonalizzanelPostBack()
                CaricaRuoliInserimento(AlboEnte)
            End If

            'If annullamodifica = 0 Then '-- 0: funzione non abilitata	1: funzione abilitata
            '    'pulsante
            '    cmdAnnullaModifica.Visible = False
            'End If
            If annullacancellazione = 0 Then  '-- 0: funzione non abilitata	1: funzione abilitata
                'pulsante
                cmdAnnullaCancellazione.Visible = False
            Else
                imgCancella.Visible = False
            End If
            'If visualizzadatiaccreditati = 0 Then '-- 0: funzione non abilitata	1: funzione abilitata
            '    'pulsante
            '    cmdVisualizzaDatiAccreditati.Visible = False
            'End If



            '4. CONTROLLI SPECIFICI A SECONDO DELL'UTENTE (E/U/R)

            'personale acquisito
            If Request.QueryString("Acquisito") = "Vero" Or Request.QueryString("Cancellato") = "Vero" Then
                'disabilitp i campi...
                CampiDisabilitati()
            End If
            ClearSessionCV()
            CaricaCV()
            If Session("TipoUtente") = "E" Then
                If Session("ModIns") = 1 Then 'modifica   
                    If ControlloRisorsaAccreditata(Request.QueryString("intPersonaleEnteAssociato")) = True Then
                        ddlEsperienzaServizioCivile.Enabled = False
                        chkCorsoOlp.Enabled = False
                        ddlCorso.Enabled = False
                    End If
                End If
                'controllo stato ebnte
                If ClsUtility.ControllaStatoEntePerBloccareMaschereAnagrafica(Session("IdEnte"), IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn"))) = True Then
                    'If ClsUtility.ControllaStatoEntePerBloccareMaschereAnagrafica(Session("IdEnte"), Session("conn")) = True Then
                    ''CampiDisabilitati()
                    'stampo a pagina un'hidden per controllare lato client
                    'se devo disabilitare o meno i campi HTML
                    'Response.Write("<input type=hidden name=chkStatoEnte value=""True"">")
                    chkStatoEnte.Value = "True"
                    'Imgerrore.Visible = True
                    lblErr.Visible = True
                    lblErr.Text = "Non è possibile " & IIf(Session("ModIns") = 2, "inserire", "modificare") & " la risorsa."
                    If cmdSalvaDoc.Visible Then lblErr.Text = lblErr.Text & " È possibile aggiornare il CV."
                    cmdSalva.Visible = False
                    imgCancella.Visible = False
                    imgRilascia.Visible = False
                    imgRisorsaVirtuale.Visible = False
                    'lblRisorsaVirtuale.Visible = False
                    'imgRuoli.Visible = False
                    Exit Sub
                Else
                    'stampo a pagina un'hidden per controllare lato client
                    'se devo disabilitare o meno i campi HTML
                    'Response.Write("<input type=hidden name=chkStatoEnte value=""False"">")
                    chkStatoEnte.Value = "False"
                End If
            Else
                'stampo a pagina un'hidden per controllare lato client
                'se devo disabilitare o meno i campi HTML
                'Response.Write("<input type=hidden name=chkStatoEnte value=""False"">")
                chkStatoEnte.Value = "False"
            End If


            'FZ controllo per disabilitare la maschera nel caso sia un'"R" che sta 
            'controllando i progetti di un' ente che nn ha la stessa regiojneee di competenza
            If ClsUtility.ControlloRegioneCompetenza(Session("TipoUtente"), Session("idEnte"), Session("Utente"), Session("IdStatoEnte"), IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn"))) = False Then
                ''CampiDisabilitati()
                'stampo a pagina un'hidden per controllare lato client
                'se devo disabilitare o meno i campi HTML
                'Response.Write("<input type=hidden name=chkStatoEnte value=""True"">")
                chkStatoEnte.Value = "True"
                'Imgerrore.Visible = True
                lblErr.Visible = True
                lblErr.Text = "Attenzione, l'ente non è di propria competenza. Impossibile effettuare modifiche."
                cmdSalva.Visible = False
                imgCancella.Visible = False
                imgRilascia.Visible = False
                imgRisorsaVirtuale.Visible = False
                'lblRisorsaVirtuale.Visible = False

                'imgRuoli.Visible = False


                'dtgRuoloPrincipale.Columns(5).Visible = False
                dtgRuoli.Columns(8).Visible = False

            End If
            'FZ fine controllo


        Else
            'PersonalizzaFuoriPostBack()
        End If

    End Sub

    Private Sub CaricaCV()
        '***Gestione CV****
        If Session("LoadCVId") IsNot Nothing Then
            'Se Il CV è caricato nel DB
            'Recupero File da DB
            CaricaAllegatoFromDB(Session("LoadCVId"), "LoadedCV", rowNoCV, rowCV, txtCVFilename, txtCVHash, txtCVData)
        End If
        If False Then
            Dim CV As Allegato = Session("LoadedCV")

            If CV IsNot Nothing Then
                'Se Il CV è caricato in Sessione (Inserimento)
                rowNoCV.Visible = False
                rowCV.Visible = True
                txtCVFilename.Text = CV.Filename
                txtCVHash.Text = CV.Hash
                txtCVData.Text = CV.DataInserimento.ToString("dd/MM/yyyy HH:mm:ss")
            Else
                'Se Il CV non è ancora caricato
                rowNoCV.Visible = True
                rowCV.Visible = False
            End If
        End If
    End Sub

    Private Sub ddlProvinciaNascita_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlProvinciaNascita.SelectedIndexChanged
        Dim SelComune As New clsSelezionaComune
        SelComune.CaricaComuniNascita(ddlComuneNascita, ddlProvinciaNascita.SelectedValue, Session("Conn"))
    End Sub

    Private Sub ddlProvinciaResidenza_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlProvinciaResidenza.SelectedIndexChanged
        Dim SelComune As New clsSelezionaComune
        SelComune.CaricaComuni(ddlComuneResidenza, ddlProvinciaResidenza.SelectedValue, Session("Conn"))
    End Sub

    <System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()>
    Public Shared Function GetCompletionList(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As List(Of String)

        Dim conn As SqlConnection = New SqlConnection
        conn.ConnectionString = ConfigurationManager.ConnectionStrings("unscproduzionenewConnectionString").ConnectionString

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

    Private Sub imgCap_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgCap.Click

        Dim selCap As New clsSelezionaComune

        txtCAP.Text = selCap.RitornaCap(ddlComuneResidenza.SelectedValue, txtIndirizzo.Text.Trim, txtCivico.Text.Trim, Session("conn"))

    End Sub

    Private Sub Accesso_Maschera_Risorsa(ByVal TipoUtente As String, ByVal IdEnte As Integer, ByVal IdEntePersonale As Integer, ByRef abilitato As Integer, ByRef dati As Integer, ByRef annullamodifica As Integer, ByRef annullacancellazione As Integer, ByRef visualizzadatiaccreditati As Integer, ByRef messaggio As String, ByRef cancellazione As Integer)

        'REALIZZATA DA: SIMONA CORDELLA 
        'DATA REALIZZAZIONE:  08/08/2014
        'FUNZIONALITA':  Verifica le condizioni di accreditamento e le eventuali modifiche in corso sui dati della risorsa
        '	             e ritorna all'' applicazione la configurazione da applicare alla maschera di gestione
        '               @abilitato  			    -- 0: maschera sola lettura		1: maschera in modifica
        '               @dati 					    -- 0: dati tabelle reali		1: dati tabelle variazioni
        '               @annullamodifica  			-- 0: funzione non abilitata	1: funzione abilitata
        '               @visualizzadatiaccreditati 	-- 0: funzione non abilitata	1: funzione abilitata
        '               @annullacancellazione       -- 0: funzione non abilitata	1: funzione abilitata
        '               @cancellazione              -- 0: funzione non abilitata	1: funzione abilitata
        '               @messaggio                  -- eventuale messaggio di ritorno da visualizzare all'utente



        Try
            Dim SqlCmd As SqlClient.SqlCommand


            SqlCmd = New SqlClient.SqlCommand
            SqlCmd.CommandText = "SP_ACCREDITAMENTO_ACCESSOMASCHERA_RISORSE"
            SqlCmd.CommandType = CommandType.StoredProcedure
            SqlCmd.Connection = Session("Conn")
            SqlCmd.Parameters.Add("@TipoUtente", SqlDbType.VarChar).Value = TipoUtente
            SqlCmd.Parameters.Add("@IdEnte", SqlDbType.Int).Value = IdEnte
            SqlCmd.Parameters.Add("@IdEntePersonale", SqlDbType.Int).Value = IdEntePersonale

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


            Dim sparam7 As SqlClient.SqlParameter
            sparam7 = New SqlClient.SqlParameter
            sparam7.ParameterName = "@cancellazione"
            'sparam1.Size = 100
            sparam7.SqlDbType = SqlDbType.TinyInt
            sparam7.Direction = ParameterDirection.Output
            SqlCmd.Parameters.Add(sparam7)

            SqlCmd.ExecuteNonQuery()


            abilitato = SqlCmd.Parameters("@abilitato").Value
            dati = SqlCmd.Parameters("@dati").Value
            annullamodifica = SqlCmd.Parameters("@annullamodifica").Value
            visualizzadatiaccreditati = SqlCmd.Parameters("@visualizzadatiaccreditati").Value
            annullacancellazione = SqlCmd.Parameters("@annullacancellazione").Value
            messaggio = SqlCmd.Parameters("@messaggio").Value
            cancellazione = SqlCmd.Parameters("@cancellazione").Value
        Catch ex As Exception
            Log.Error(LogEvent.RISORSE_ENTE_ERRORE, "Cancellazione", ex)

            lblErr.Visible = True
            lblErr.Text = ex.Message
        Finally
            'ConnX.Close()
            'SqlCmd = Nothing
        End Try

    End Sub

    Sub PersonalizzoNoVisibilitaCampi(ByVal AlboEnte As String)
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
        'If (Session("TipoUtente") = "U" Or Session("TipoUtente") = "R") Then
        'Verifico che l'Ente in Sessione  sia un ente di 3 o 4 classe
        strsql = "select ca.idclasseaccreditamento from classiaccreditamento ca " &
        " inner join enti on ca.idclasseaccreditamento=enti.idclasseaccreditamentoRichiesta " &
        " where idente = " & Session("IDEnte") & ""
        dtrgenerico = ClsServer.CreaDatareader(strsql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
        If dtrgenerico.HasRows = True Then
            dtrgenerico.Read()
            If dtrgenerico("idclasseaccreditamento") = "3" Or dtrgenerico("idclasseaccreditamento") = "4" Then
                If Request.QueryString("tipoazione") <> "Modifica" Then

                    If (AlboEnte = "" Or AlboEnte = "SCU") Then
                        imgRisorsaVirtuale.Visible = False
                    Else
                        imgRisorsaVirtuale.Visible = True
                    End If

                    imgRisorsaVirtuale.Visible = True
                    'lblRisorsaVirtuale.Visible = True
                Else
                    imgRisorsaVirtuale.Visible = False
                    'lblRisorsaVirtuale.Visible = False
                End If
            Else
                imgRisorsaVirtuale.Visible = False
                'lblRisorsaVirtuale.Visible = False
            End If
        End If
        'Else
        'imgRisorsaVirtuale.Visible = False
        'lblRisorsaVirtuale.Visible = False
        'End If
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
        'controllo se esiste un tipo di azione
        If Not Request.QueryString("tipoazione") Is Nothing Then
            'se si tratta di modifica
            If Request.QueryString("tipoazione") = "Modifica" Then
                'vado a controllare se si tratta di risorsa cancellata se Acquisito è nothing vuol dire che si tratta di risorsa cancellata
                If Request.QueryString("Cancellato") = "Vero" Then
                    imgCancella.Visible = False
                End If
                'contorllo se si tratta di risorsa acquisita
                If Request.QueryString("Acquisito") = "Vero" Then
                    'nascondo il pulsante di cancellazione
                    imgCancella.Visible = False
                Else
                    'mostro il pulsante di cancellazione
                    imgRilascia.Visible = False
                End If
                'imposto la sessione di controllo Modifica/Inserimento = 1 (modifica)
                If Session("TipoUtente") = "E" Then
                    dtgRuoli.Columns(8).Visible = False
                    dtgRuoli.Columns(9).Visible = False
                    lblCorso.Visible = False
                    chkCorsoOlp.Visible = False
                End If
                If Session("TipoUtente") = "U" Then
                    lblCorso.Visible = True
                    chkCorsoOlp.Visible = True
                End If
                Session("ModIns") = 1
            Else
                'dtgRuoloPrincipale.Columns(6).Visible = False
                dtgRuoli.Columns(8).Visible = False
                dtgRuoli.Columns(9).Visible = False

                'nascondo i pulsanti perchè la variabile di tipo azione non è valorizzata
                imgCancella.Visible = False
                imgRilascia.Visible = False
                'setto la variabile di Modifica/Inserimento = 2 (inserimento)
                Session("ModIns") = 2
            End If
        End If

        lblErr.Text = ""
        lblErr.Text = ""


    End Sub

    'routine che mi disabilita i campi se si tratta di una risorsa acquisita
    Sub CampiDisabilitati()
        Dim Grigio As Color
        Dim Rosso As Color

        Rosso = Color.Gainsboro

        txtCellulare.ReadOnly = True
        txtCivico.ReadOnly = True
        txtCodiceFiscale.ReadOnly = True
        txtCognome.ReadOnly = True
        ddlComuneNascita.Enabled = False
        ddlComuneResidenza.Enabled = False
        txtDataNascita.ReadOnly = True
        txtEmail.ReadOnly = True
        txtFax.ReadOnly = True
        txtIndirizzo.ReadOnly = True
        TxtDettaglioRecapito.ReadOnly = True
        txtNome.ReadOnly = True
        txtPosizione.ReadOnly = True
        ddlProvinciaNascita.Enabled = False
        ddlProvinciaResidenza.Enabled = False
        txtTelefono.ReadOnly = True
        txtTitolo.ReadOnly = True
        txtCAP.ReadOnly = True
        chkCorsoOlp.Enabled = False
        ddlCorso.Enabled = False
        ddlEsperienzaServizioCivile.Enabled = False
        btnEliminaCV.Visible = False
        btnModificaCV.Visible = False
        'txtCellulare.BackColor = Grigio
        'txtCivico.BackColor = Grigio
        'txtCodiceFiscale.BackColor = Grigio
        'txtCognome.BackColor = Grigio
        'txtComuneNascita.BackColor = Grigio
        'txtComuneResidenza.BackColor = Grigio
        'txtDataNascita.BackColor = Grigio
        'txtEmail.BackColor = Grigio
        'txtFax.BackColor = Grigio
        'txtIndirizzo.BackColor = Grigio
        'txtNome.BackColor = Grigio
        'txtPosizione.BackColor = Grigio
        'txtProvinciaNascita.BackColor = Grigio
        'txtProvinciaResidenza.BackColor = Grigio
        'txtTelefono.BackColor = Grigio
        'txtTitolo.BackColor = Grigio

        txtCellulare.BackColor = Rosso
        txtCivico.BackColor = Rosso
        txtCodiceFiscale.BackColor = Rosso
        txtCognome.BackColor = Rosso
        ddlComuneNascita.BackColor = Rosso
        ddlComuneResidenza.BackColor = Rosso
        txtDataNascita.BackColor = Rosso
        txtEmail.BackColor = Rosso
        txtFax.BackColor = Rosso
        txtIndirizzo.BackColor = Rosso
        TxtDettaglioRecapito.BackColor = Rosso
        txtNome.BackColor = Rosso
        txtPosizione.BackColor = Rosso
        ddlProvinciaNascita.BackColor = Rosso
        ddlProvinciaResidenza.BackColor = Rosso
        txtTelefono.BackColor = Rosso
        txtTitolo.BackColor = Rosso
        txtCAP.BackColor = Rosso
        cmdSalva.Visible = False
        rowNoCV.Visible = False
        btnEliminaCV.Visible = False
        btnModificaCV.Visible = False
        dtgRuoli.Enabled = False

        'controllo possibilità sostituzione documenti
        AbilitaSostituzioneDocumenti()

    End Sub

    Private Sub AbilitaSostituzioneDocumenti()
        Dim intIdEnte As Integer
        Dim intIdEntePersonale As Integer
        intIdEnte = Session("IdEnte")
        intIdEntePersonale = IIf(Request.QueryString("intPersonaleEnteAssociato") = Nothing, 0, Request.QueryString("intPersonaleEnteAssociato"))

        'ABILITA SOSTITUZIONE DOCUMENTI ART2/ART10
        lblErr.Visible = False
        Try
            Dim SqlCmd As SqlClient.SqlCommand


            SqlCmd = New SqlClient.SqlCommand
            SqlCmd.CommandText = "SP_ACCREDITAMENTO_INTEGRAZIONE_DOCUMENTI_RISORSA"
            SqlCmd.CommandType = CommandType.StoredProcedure
            SqlCmd.Connection = Session("Conn")
            SqlCmd.Parameters.Add("@IdEnte", SqlDbType.Int).Value = intIdEnte
            SqlCmd.Parameters.Add("@IdEntePersonale", SqlDbType.Int).Value = intIdEntePersonale

            Dim sparamIdEnteFase As SqlClient.SqlParameter
            sparamIdEnteFase = New SqlClient.SqlParameter
            sparamIdEnteFase.ParameterName = "@IdEnteFase"
            'sparam1.Size = 100
            sparamIdEnteFase.SqlDbType = SqlDbType.Int
            sparamIdEnteFase.Direction = ParameterDirection.Output
            SqlCmd.Parameters.Add(sparamIdEnteFase)

            Dim sparamIdEnteFaseArt_2_10 As SqlClient.SqlParameter
            sparamIdEnteFaseArt_2_10 = New SqlClient.SqlParameter
            sparamIdEnteFaseArt_2_10.ParameterName = "@IdEnteFaseArt_2_10"
            'sparam1.Size = 100
            sparamIdEnteFaseArt_2_10.SqlDbType = SqlDbType.Int
            sparamIdEnteFaseArt_2_10.Direction = ParameterDirection.Output
            SqlCmd.Parameters.Add(sparamIdEnteFaseArt_2_10)

            Dim sparam1 As SqlClient.SqlParameter
            sparam1 = New SqlClient.SqlParameter
            sparam1.ParameterName = "@AbilitaCV"
            'sparam1.Size = 100
            sparam1.SqlDbType = SqlDbType.TinyInt
            sparam1.Direction = ParameterDirection.Output
            SqlCmd.Parameters.Add(sparam1)

            SqlCmd.ExecuteNonQuery()

            Session("IdEnteFaseArt") = SqlCmd.Parameters("@IdEnteFaseArt_2_10").Value

            If SqlCmd.Parameters("@AbilitaCV").Value = 1 Then
                btnModificaCV.Visible = True
                cmdSalvaDoc.Visible = True

                rowCV.Visible = True
                rowNoCV.Visible = False
            End If

        Catch ex As Exception
            MessaggiAlert(ex.Message)


        End Try
    End Sub

    'pulsante abilitato solo nel caso in cui i tratyta di una risorsa acquisita
    Private Sub imgRilascia_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgRilascia.Click
        'variabile stringa locale che uso per effettuare l'update
        Dim strUpdateCommand As String
        Dim myCommand As New SqlClient.SqlCommand

        Dim idPersonaleAcquisito = CInt(Request.QueryString("intPersonaleEnteAcquisito"))
        Dim entity = New Entity With {
         .Id = idPersonaleAcquisito,
         .Name = "personaleacquisito"
        }
        Try

            'stringa per l'update relativa al rilascio della risorsa
            strUpdateCommand = "update personaleacquisito set Approvato=5, DataFineValidità=GetDate() where IDPersonaleAcquisito='" & CInt(Request.QueryString("intPersonaleEnteAcquisito")) & "'"
            'assegno i parametri necessari al sqlcommand per eseguire l'operazione sulla base dati
            myCommand.Connection = IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn"))
            myCommand.CommandText = strUpdateCommand
            'eseguo l'update
            myCommand.ExecuteNonQuery()
            'torno alla pagina di ricerca
            Response.Redirect("ricercaentepersonale.aspx")
        Catch ex As Exception
            Log.Error(LogEvent.RISORSE_ENTE_ERRORE, "Rilascio Risorsa", ex, entity:=entity)

            'Response.Write(ex.Message.ToString())
        End Try
    End Sub

    Sub CaricaAnagrafica()
        'stringa che conterrà lea query di ricerca della persona selezionata
        Dim strCommand As String
        'datareader locale
        Dim dtrCaricaPersonaleEnte As System.Data.SqlClient.SqlDataReader
        'dataset locale
        Dim dtsCaricaPersonaleEnte As System.Data.DataSet
        'se la variabile è settata a 1 vuol dire che si tratta di modifica

        Dim accreditato As String

        Dim IDComuneNascita As Object
        Dim IDComuneResidenza As Object

        Dim IDProvinciaNascita As Object
        Dim IDProvinciaResidenza As Object

        Dim ProvinceNazionaliNascita As Object
        Dim ProvinceNazionaliResidenza As Object

        accreditato = ""
        ' il 26/05/2010 da simona cordella aggiuntocampo in visualizzazione AnnoCorsoAggiornamento
        If Session("ModIns") = 1 Then
            'preparo la query per caricare i dati della risorsa selezionata
            strCommand = "select a.IDEntePersonale,a.IDEnte,a.Cognome,a.Nome,a.Titolo,a.Posizione,"
            strCommand = strCommand & "a.Cellulare,a.Email,a.Telefono,a.Fax,a.Username,a.Password,"
            strCommand = strCommand & "a.IDComuneNascita,c.IDprovincia as IDprovinciaNascita,a.IDComuneResidenza,e.IDProvincia as IDProvinciaResidenza,a.DataNascita,"
            strCommand = strCommand & "a.Indirizzo,a.Civico,a.NumeroDelibera,a.DataDelibera,"
            strCommand = strCommand & "a.Inquadramento,a.Legge,a.TitoloCurriculum,a.DurataAttività,"
            strCommand = strCommand & "a.CodiceFiscale,a.Cap, "
            strCommand = strCommand & "a.DettaglioRecapito, "
            strCommand = strCommand & "case isnull(a.CorsoOLP,'') "
            strCommand = strCommand & "when '' then '0' "
            strCommand = strCommand & "when 0 then '0' "
            strCommand = strCommand & "when 1 then '1' "
            strCommand = strCommand & "end as CorsoOlp, "
            strCommand = strCommand & "a.EsperienzaServizioCivile, "
            strCommand = strCommand & "a.Corso, "
            strCommand = strCommand & "a.RequisitiMinimi, "
            strCommand = strCommand & "a.IdAllegatoCV, "
            strCommand = strCommand & "CASE ISNULL(a.AnnoCorsoAggiornamento,0) WHEN 0 THEN 'Non Indicato' else a.AnnoCorsoAggiornamento  end as AnnoCorsoAggiornamento, "
            strCommand = strCommand & "b.Denominazione as ComuneNascita,c.Provincia as ProvinciaNascita, "
            strCommand = strCommand & "d.Denominazione as ComuneResidenza,e.Provincia as ProvinciaResidenza, "
            strCommand = strCommand & "c.ProvinceNazionali as ProvinceNazionaliNascita, e.ProvinceNazionali as ProvinceNazionaliResidenza "
            strCommand = strCommand & "from entepersonale as a "
            strCommand = strCommand & "inner join comuni as b on b.IDComune=a.IDComuneNascita "
            strCommand = strCommand & "inner join provincie as c on b.IDProvincia=c.IDprovincia "
            strCommand = strCommand & "left join comuni as d on d.IDComune=a.IDComuneResidenza "
            strCommand = strCommand & "left join provincie as e on e.IDProvincia=d.IDprovincia "
            strCommand = strCommand & " where "
            strCommand = strCommand & "IDEntePersonale='" & Request.QueryString("intPersonaleEnteAssociato") & "'"

            'controllo e chiudo se aperto il datareader
            If Not dtrCaricaPersonaleEnte Is Nothing Then
                dtrCaricaPersonaleEnte.Close()
                dtrCaricaPersonaleEnte = Nothing
            End If

            'eseguo la query
            dtrCaricaPersonaleEnte = ClsServer.CreaDatareader(strCommand, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
            If dtrCaricaPersonaleEnte.HasRows = True Then
                dtrCaricaPersonaleEnte.Read()
                'carico i dati nelle txt
                txtCellulare.Text = IIf(IsDBNull(dtrCaricaPersonaleEnte("Cellulare")) = True, "", dtrCaricaPersonaleEnte("Cellulare"))
                txtCivico.Text = IIf(IsDBNull(dtrCaricaPersonaleEnte("Civico")) = True, "", dtrCaricaPersonaleEnte("Civico"))
                txtCodiceFiscale.Text = IIf(IsDBNull(dtrCaricaPersonaleEnte("CodiceFiscale")) = True, "", dtrCaricaPersonaleEnte("CodiceFiscale"))
                txtCognome.Text = IIf(IsDBNull(dtrCaricaPersonaleEnte("Cognome")) = True, "", dtrCaricaPersonaleEnte("Cognome"))



                'se è il primo caricamento della pagina stampo le hidden per gli id dei comuni
                If Page.IsPostBack = False Then
                    Response.Write("<input type=hidden name=IdComuneNascita value=""" & dtrCaricaPersonaleEnte("IDComuneNascita") & """>")
                    Response.Write("<input type=hidden name=IdComuneResidenza value=""" & dtrCaricaPersonaleEnte("IDComuneResidenza") & """>")
                    If IsDBNull(dtrCaricaPersonaleEnte("IDComuneResidenza")) = True Then
                        txtIDComunes.Value = ""
                    Else
                        txtIDComunes.Value = dtrCaricaPersonaleEnte("IDComuneResidenza")
                    End If
                End If
                txtDataNascita.Text = IIf(IsDBNull(dtrCaricaPersonaleEnte("DataNascita")) = True, "", dtrCaricaPersonaleEnte("DataNascita"))
                txtEmail.Text = IIf(IsDBNull(dtrCaricaPersonaleEnte("Email")) = True, "", dtrCaricaPersonaleEnte("Email"))
                txtFax.Text = IIf(IsDBNull(dtrCaricaPersonaleEnte("Fax")) = True, "", dtrCaricaPersonaleEnte("Fax"))
                txtIndirizzo.Text = IIf(IsDBNull(dtrCaricaPersonaleEnte("Indirizzo")) = True, "", dtrCaricaPersonaleEnte("Indirizzo"))
                Session("IdComune") = IIf(IsDBNull(dtrCaricaPersonaleEnte("IDComuneResidenza")) = True, "", dtrCaricaPersonaleEnte("IDComuneResidenza"))
                TxtDettaglioRecapito.Text = IIf(IsDBNull(dtrCaricaPersonaleEnte("DettaglioRecapito")) = True, "", dtrCaricaPersonaleEnte("DettaglioRecapito"))
                txtNome.Text = IIf(IsDBNull(dtrCaricaPersonaleEnte("Nome")) = True, "", dtrCaricaPersonaleEnte("Nome"))
                txtPosizione.Text = IIf(IsDBNull(dtrCaricaPersonaleEnte("Posizione")) = True, "", dtrCaricaPersonaleEnte("Posizione"))
                txtTelefono.Text = IIf(IsDBNull(dtrCaricaPersonaleEnte("Telefono")) = True, "", dtrCaricaPersonaleEnte("Telefono"))
                txtTitolo.Text = IIf(IsDBNull(dtrCaricaPersonaleEnte("Titolo")) = True, "", dtrCaricaPersonaleEnte("Titolo"))

                'Carico L'Id dell'allegato CV se presente
                If Not IsDBNull(dtrCaricaPersonaleEnte("IdAllegatoCV")) Then
                    Session("LoadCVId") = dtrCaricaPersonaleEnte("IdAllegatoCV")
                Else
                    Session("LoadCVId") = Nothing
                End If

                IDProvinciaNascita = dtrCaricaPersonaleEnte("IDprovinciaNascita")
                IDComuneNascita = dtrCaricaPersonaleEnte("IDComuneNascita")
                ProvinceNazionaliNascita = IIf(IsDBNull(dtrCaricaPersonaleEnte("ProvinceNazionaliNascita")), "True", dtrCaricaPersonaleEnte("ProvinceNazionaliNascita"))

                IDProvinciaResidenza = dtrCaricaPersonaleEnte("IDProvinciaResidenza")

                IDComuneResidenza = dtrCaricaPersonaleEnte("IDComuneResidenza")
                ProvinceNazionaliResidenza = IIf(IsDBNull(dtrCaricaPersonaleEnte("ProvinceNazionaliResidenza")), "True", dtrCaricaPersonaleEnte("ProvinceNazionaliResidenza"))

                txtCAP.Text = IIf(IsDBNull(dtrCaricaPersonaleEnte("Cap")) = True, "", dtrCaricaPersonaleEnte("Cap"))
                If dtrCaricaPersonaleEnte("CorsoOlp") = "1" Then
                    chkCorsoOlp.Checked = True
                End If
                ddlCorso.SelectedValue = IIf(IsDBNull(dtrCaricaPersonaleEnte("Corso")) = True, 0, dtrCaricaPersonaleEnte("Corso"))
                ddlEsperienzaServizioCivile.SelectedValue = IIf(IsDBNull(dtrCaricaPersonaleEnte("EsperienzaServizioCivile")) = True, 0, dtrCaricaPersonaleEnte("EsperienzaServizioCivile"))
                'Anno Aggiormanento
                LblAnnoCorso.Text = "Anno Aggiornamento: " & dtrCaricaPersonaleEnte("AnnoCorsoAggiornamento")
                'hidden per nascondere l'id della risorsa selezionata
                'Response.Write("<input type=hidden name=IdEntePersonale value=""" & dtrCaricaPersonaleEnte("IDEntePersonale") & """>")
                IdEntePersonale.Value = dtrCaricaPersonaleEnte("IDEntePersonale")
                If Not dtrCaricaPersonaleEnte Is Nothing Then
                    dtrCaricaPersonaleEnte.Close()
                    dtrCaricaPersonaleEnte = Nothing
                End If
            End If
            If Not dtrCaricaPersonaleEnte Is Nothing Then
                dtrCaricaPersonaleEnte.Close()
                dtrCaricaPersonaleEnte = Nothing
            End If

            Dim SelComune As New clsSelezionaComune

            If ProvinceNazionaliNascita.ToString = "False" Then
                ChkEsteroNascita.Checked = True
                Dim blnEstero As Boolean = True
                SelComune.CaricaProvinciaNazione(ddlProvinciaNascita, blnEstero, Session("Conn"))
                ddlComuneNascita.Items.Clear()

            End If

            ddlProvinciaNascita.SelectedValue = IIf(IsDBNull(IDProvinciaNascita) = True, 0, IDProvinciaNascita)
            SelComune.CaricaComuniNascita(ddlComuneNascita, ddlProvinciaNascita.SelectedValue, Session("Conn"))
            ddlComuneNascita.SelectedValue = IIf(IsDBNull(IDComuneNascita) = True, 0, IDComuneNascita)

            If ProvinceNazionaliResidenza.ToString = "False" Then
                ChkEsteroResidenza.Checked = True
                Dim blnEstero As Boolean = True
                SelComune.CaricaProvinciaNazione(ddlProvinciaResidenza, blnEstero, Session("Conn"))
                ddlComuneResidenza.Items.Clear()

            End If

            ddlProvinciaResidenza.SelectedValue = IIf(IsDBNull(IDProvinciaResidenza) = True, 0, IDProvinciaResidenza)
            SelComune.CaricaComuni(ddlComuneResidenza, ddlProvinciaResidenza.SelectedValue, Session("Conn"))
            ddlComuneResidenza.SelectedValue = IIf(IsDBNull(IDComuneResidenza) = True, 0, IDComuneResidenza)

            '28.01.2006 jonathani
            'controllo se la risorsa ha almeno un ruolo accreditato per 
            'disabilitare il campo codice fiscale
            strCommand = "select accreditato from entepersonaleruoli "
            strCommand = strCommand & "inner join entepersonale on entepersonale.identepersonale=entepersonaleruoli.identepersonale "
            strCommand = strCommand & "where entepersonaleruoli.identepersonale='" & Request.QueryString("intPersonaleEnteAssociato") & "' and entepersonaleruoli.accreditato=1"

            'eseguo la query
            dtrCaricaPersonaleEnte = ClsServer.CreaDatareader(strCommand, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
            If dtrCaricaPersonaleEnte.HasRows Then
                txtCodiceFiscale.ReadOnly = True
                txtCodiceFiscale.BackColor = txtCodiceFiscale.BackColor.Gainsboro
                If Session("TipoUtente") = "E" Then
                    accreditato = "1"
                Else
                    accreditato = ""
                End If
            End If

            If Not dtrCaricaPersonaleEnte Is Nothing Then
                dtrCaricaPersonaleEnte.Close()
                dtrCaricaPersonaleEnte = Nothing
            End If

            'se si tratta si modifica carico i dati dal database
            If Session("dtvRuoli") Is Nothing Then
                'preparo la query sui ruoli della risorsa
                strCommand = "select a.IDEntePersonaleRuolo,a.IDEntePersonale,a.IDRuolo,a.DataInizioValidità,a.Visibilità,a.Principale,case a.Accreditato when 1 then 'Iscritto' when 0 then 'Da Accreditare' when -1 then 'Non Iscritto' else 'Non Definito' end as Accreditato, "
                strCommand = strCommand & "case a.Principale when 1 then '<img src=images/selezionato_small.png border=0 style=''width:20px;height:20px''>' end Principale,"
                strCommand = strCommand & "b.IDRuolo as IDentificativoRuolo,"
                strCommand = strCommand & "b.Ruolo, "
                strCommand = strCommand & "b.IDTipoRuolo, b.FlagControlloProvincia, b.IDTipoQuantità, "
                strCommand = strCommand & "b.ValoreQuantità, b.MaxRuoliSecondari "
                strCommand = strCommand & "from entepersonaleruoli as a "
                strCommand = strCommand & "inner join ruoli as b on a.IDRuolo=b.IDRuolo "
                strCommand = strCommand & "where a.datafinevalidità is null and a.IDEntePersonale='" & Request.QueryString("intPersonaleEnteAssociato") & "' and  b.Nascosto = 0 "
                If Session("TipoUtente") = "E" Then
                    strCommand = strCommand & " and RuoloAccreditamento=1 "
                End If
                strCommand = strCommand & " order by a.Principale desc"
                'controllo e chiudo se aperto il datareader
                If Not dtrCaricaPersonaleEnte Is Nothing Then
                    dtrCaricaPersonaleEnte.Close()
                    dtrCaricaPersonaleEnte = Nothing
                End If
                'eseguo la query
                dtrCaricaPersonaleEnte = ClsServer.CreaDatareader(strCommand, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
                'datarow e datatable che userò per caricare da codice il contenuto della datagrid
                Dim dtR As DataRow
                Dim dtT As New DataTable
                'variabile contatore
                Dim i As Integer
                'variavile che userò per il count dei ruoli
                Dim RecordCount As Integer
                'imposto i nomi delle colonne della datagrid
                dtT.Columns.Add(New DataColumn("IDRuolo", GetType(String)))
                dtT.Columns.Add(New DataColumn("Ruolo", GetType(String)))
                dtT.Columns.Add(New DataColumn("Principale", GetType(String)))
                dtT.Columns.Add(New DataColumn("Accreditato", GetType(String)))
                dtT.Columns.Add(New DataColumn("Visibilità", GetType(String)))

                dtT.Columns.Add()
                'se ci sono dei ruoli
                If dtrCaricaPersonaleEnte.HasRows = True Then
                    dtrCaricaPersonaleEnte.Read()
                    'controllo se il campo è vuoto
                    If dtrCaricaPersonaleEnte.Item("Ruolo").ToString <> "" Then
                        'stampo una hidden con il valore dell'idruolo
                        Response.Write("<input type=hidden name=IdModVal value=""" & dtrCaricaPersonaleEnte.Item("IDRuolo") & """>")
                        'inizio a caricare la prima riga
                        dtR = dtT.NewRow()
                        dtR(0) = dtrCaricaPersonaleEnte.Item("IDRuolo")
                        IdModVal1.Value = "" & dtrCaricaPersonaleEnte.Item("IDRuolo") & ""
                        'carico la txt del ruolo readonly a seconda della tipologia di risorsa (Acquisito/Non Acquisito)
                        If Request.QueryString("Acquisito") = "Vero" Or Request.QueryString("Cancellato") = "Vero" Then
                            dtR(1) = "<input type=text style=""width:300px; border-style:none"" readonly name=Val1 value=""" & dtrCaricaPersonaleEnte.Item("Ruolo") & """>"
                        Else
                            dtR(1) = "<input type=text style=""width:300px; border-style:none"" readonly name=Val1 value=""" & dtrCaricaPersonaleEnte.Item("Ruolo") & """>"
                        End If
                        dtR(2) = "<img src=images/selezionato_small.png border=0 style='width:20px;height:20px'>"
                        'carico la txt dell'accreditamento readonly a seconda della tipologia di risorsa (Acquisito/Non Acquisito)
                        If Request.QueryString("Acquisito") = "Vero" Or Request.QueryString("Cancellato") = "Vero" Then
                            dtR(3) = "<input type=text style=""width:170px; border-style:none"" readonly name=Accr1 value=""" & dtrCaricaPersonaleEnte.Item("Accreditato") & """>"
                        Else
                            dtR(3) = "<input type=text style=""width:170px; border-style:none"" readonly name=Accr1 value=""" & dtrCaricaPersonaleEnte.Item("Accreditato") & """>"
                        End If
                        'carico la combo della visibilità a seconda del dato del database
                        'e a seconda della tipologia di risorsa in modifica (Request.QueryString("Acquisito") = "Vero" disabilito)
                        'la combo
                        '0 privata
                        '1 Riservata
                        '2 Pubblica
                        Select Case CInt(dtrCaricaPersonaleEnte.Item("Visibilità"))
                            Case 0
                                If Request.QueryString("Acquisito") = "Vero" Or Request.QueryString("Cancellato") = "Vero" Then
                                    dtR(4) = "<select disabled=true name=""ddlVisibilita1"">" & vbCrLf & "<option selected value=""0"">Privata</option>" & vbCrLf & "<option value=""1"">Riservata</option>" & vbCrLf & "<option value=""2"">Pubblica</option>" & vbCrLf & "</select>"
                                Else
                                    dtR(4) = "<select name=""ddlVisibilita1"">" & vbCrLf & "<option selected value=""0"">Privata</option>" & vbCrLf & "<option value=""1"">Riservata</option>" & vbCrLf & "<option value=""2"">Pubblica</option>" & vbCrLf & "</select>"
                                End If
                            Case 1
                                If Request.QueryString("Acquisito") = "Vero" Or Request.QueryString("Cancellato") = "Vero" Then
                                    dtR(4) = "<select disabled=true name=""ddlVisibilita1"">" & vbCrLf & "<option value=""0"">Privata</option>" & vbCrLf & "<option selected value=""1"">Riservata</option>" & vbCrLf & "<option value=""2"">Pubblica</option>" & vbCrLf & "</select>"
                                Else
                                    dtR(4) = "<select name=""ddlVisibilita1"">" & vbCrLf & "<option value=""0"">Privata</option>" & vbCrLf & "<option selected value=""1"">Riservata</option>" & vbCrLf & "<option value=""2"">Pubblica</option>" & vbCrLf & "</select>"
                                End If
                            Case 2
                                If Request.QueryString("Acquisito") = "Vero" Or Request.QueryString("Cancellato") = "Vero" Then
                                    dtR(4) = "<select disabled=true name=""ddlVisibilita1"">" & vbCrLf & "<option value=""0"">Privata</option>" & vbCrLf & "<option value=""1"">Riservata</option>" & vbCrLf & "<option selected value=""2"">Pubblica</option>" & vbCrLf & "</select>"
                                Else
                                    dtR(4) = "<select name=""ddlVisibilita1"">" & vbCrLf & "<option value=""0"">Privata</option>" & vbCrLf & "<option value=""1"">Riservata</option>" & vbCrLf & "<option selected value=""2"">Pubblica</option>" & vbCrLf & "</select>"
                                End If
                        End Select

                        'aggiungo la riga creata
                        dtT.Rows.Add(dtR)

                        If accreditato = "1" Then
                            If dtrCaricaPersonaleEnte.Item("Ruolo").ToString = "Formatore" Then
                                'rimosso il 15/11/2016 da s.c.
                                ' ddlCorso.Enabled = False
                            End If
                            accreditato = ""
                        End If
                    Else
                        'riga vuota
                        dtR = dtT.NewRow()
                        dtR(0) = ""
                        dtR(1) = "<input type=text style=""width:300px; border-style:none"" readonly name=Val1 value="""">"
                        dtR(2) = "<img src=images/selezionato_small.png border=0 style='width:20px;height:20px'>"
                        dtR(3) = "<input type=text style=""width:170px; border-style:none"" readonly name=Accr1 value="""">"
                        dtR(4) = "<select disabled=true name=""ddlVisibilita1"">" & vbCrLf & "<option value=""0"">Privata</option>" & vbCrLf & "<option value=""1"">Riservata</option>" & vbCrLf & "<option value=""2"">Pubblica</option>" & vbCrLf & "</select>"

                        dtT.Rows.Add(dtR)
                    End If
                    'inizio a caricare le altre tre righe partendo dalla seconda
                    i = 2
                    'carico i restanti ruoli dal database
                    Do While dtrCaricaPersonaleEnte.Read
                        'carico i valori della prima riga
                        dtR = dtT.NewRow
                        'prima colonna = idruolo
                        dtR(0) = dtrCaricaPersonaleEnte.Item("IDRuolo")
                        'Request.QueryString("Acquisito") = "Vero" i campi devono essere disabilitati
                        If Request.QueryString("Acquisito") = "Vero" Or Request.QueryString("Cancellato") = "Vero" Then
                            dtR(1) = "<input readonly type=text style=""width:300px; border-style:none"" readonly name=Val" & i & " value=""" & dtrCaricaPersonaleEnte.Item("Ruolo") & """>"
                        Else
                            dtR(1) = "<input type=text readonly style=""width:300px; border-style:none"" name=Val" & i & " value=""" & dtrCaricaPersonaleEnte.Item("Ruolo") & """>"
                        End If
                        dtR(2) = ""
                        'Request.QueryString("Acquisito") = "Vero" i campi devono essere disabilitati
                        If Request.QueryString("Acquisito") = "Vero" Or Request.QueryString("Cancellato") = "Vero" Then
                            dtR(3) = "<input type=text readonly style=""width:170px; border-style:none"" name=Accr" & i & " value=""" & dtrCaricaPersonaleEnte.Item("Accreditato") & """>"
                        Else
                            dtR(3) = "<input type=text style=""width:170px; border-style:none"" readonly name=Accr" & i & " value=""" & dtrCaricaPersonaleEnte.Item("Accreditato") & """>"
                        End If
                        'carico la combo della visibilità a seconda del dato del database
                        'e a seconda della tipologia di risorsa in modifica (Request.QueryString("Acquisito") = "Vero" disabilito)
                        'la combo
                        '0 privata
                        '1 Riservata
                        '2 Pubblica
                        Select Case CInt(dtrCaricaPersonaleEnte.Item("Visibilità"))
                            Case 0
                                If Request.QueryString("Acquisito") = "Vero" Or Request.QueryString("Cancellato") = "Vero" Then
                                    dtR(4) = "<select disabled=true name=""ddlVisibilita" & i & """>" & vbCrLf & "<option selected value=""0"">Privata</option>" & vbCrLf & "<option value=""1"">Riservata</option>" & vbCrLf & "<option value=""2"">Pubblica</option>" & vbCrLf & "</select>"
                                Else
                                    dtR(4) = "<select name=""ddlVisibilita" & i & """>" & vbCrLf & "<option selected value=""0"">Privata</option>" & vbCrLf & "<option value=""1"">Riservata</option>" & vbCrLf & "<option value=""2"">Pubblica</option>" & vbCrLf & "</select>"
                                End If
                            Case 1
                                If Request.QueryString("Acquisito") = "Vero" Or Request.QueryString("Cancellato") = "Vero" Then
                                    dtR(4) = "<select disabled=true name=""ddlVisibilita" & i & """>" & vbCrLf & "<option value=""0"">Privata</option>" & vbCrLf & "<option selected value=""1"">Riservata</option>" & vbCrLf & "<option value=""2"">Pubblica</option>" & vbCrLf & "</select>"
                                Else
                                    dtR(4) = "<select name=""ddlVisibilita" & i & """>" & vbCrLf & "<option value=""0"">Privata</option>" & vbCrLf & "<option selected value=""1"">Riservata</option>" & vbCrLf & "<option value=""2"">Pubblica</option>" & vbCrLf & "</select>"
                                End If
                            Case 2
                                If Request.QueryString("Acquisito") = "Vero" Or Request.QueryString("Cancellato") = "Vero" Then
                                    dtR(4) = "<select disabled=true name=""ddlVisibilita" & i & """>" & vbCrLf & "<option value=""0"">Privata</option>" & vbCrLf & "<option value=""1"">Riservata</option>" & vbCrLf & "<option selected value=""2"">Pubblica</option>" & vbCrLf & "</select>"
                                Else
                                    dtR(4) = "<select name=""ddlVisibilita" & i & """>" & vbCrLf & "<option value=""0"">Privata</option>" & vbCrLf & "<option value=""1"">Riservata</option>" & vbCrLf & "<option selected value=""2"">Pubblica</option>" & vbCrLf & "</select>"
                                End If
                        End Select

                        'aggiungo la riga al datatable
                        dtT.Rows.Add(dtR)
                        'aggiorno la variabile contatore
                        i = i + 1
                    Loop
                    'il loop mi aggiunge un item di troppo al contatore così faccio il -1
                    'per portarlo al valore necessario per i controlli
                    i = i - 1
                    'controllo quanti item ho creato su 4 e se minore di 4 aggiungo i restanti item vuoti alla datagrid
                    If i < 4 Then
                        Select Case i
                            'se ne mancano tre aggiungo tre item e i mi fa anche da caption per gli item creati
                            Case 1
                                For i = 2 To 4
                                    dtR = dtT.NewRow()
                                    dtR(0) = ""
                                    dtR(1) = "<input readonly type=text style=""width:300px; border-style:none"" readonly name=Val" & i.ToString & " value="""">"
                                    dtR(2) = ""
                                    dtR(3) = "<input readonly type=text style=""width:170px; border-style:none"" readonly name=Accr" & i.ToString & " value="""">"
                                    dtR(4) = "<select disabled=true name=""ddlVisibilita" & i & """>" & vbCrLf & "<option selected value=""0"">Privata</option>" & vbCrLf & "<option value=""1"">Riservata</option>" & vbCrLf & "<option value=""2"">Pubblica</option>" & vbCrLf & "</select>"

                                    dtT.Rows.Add(dtR)
                                Next
                                'se ne mancano due aggiungo due item e i mi fa anche da caption per gli item creati
                            Case 2
                                For i = 3 To 4
                                    dtR = dtT.NewRow()
                                    dtR(0) = ""
                                    dtR(1) = "<input readonly type=text style=""width:300px; border-style:none"" readonly name=Val" & i.ToString & " value="""">"
                                    dtR(2) = ""
                                    dtR(3) = "<input readonly type=text style=""width:170px; border-style:none"" readonly name=Accr" & i.ToString & " value="""">"
                                    dtR(4) = "<select disabled=true name=""ddlVisibilita" & i & """>" & vbCrLf & "<option selected value=""0"">Privata</option>" & vbCrLf & "<option value=""1"">Riservata</option>" & vbCrLf & "<option value=""2"">Pubblica</option>" & vbCrLf & "</select>"

                                    dtT.Rows.Add(dtR)
                                Next
                                'se ne manca un item aggiungo un item e i mi fa anche da caption per gli item creati
                            Case 3
                                dtR = dtT.NewRow()
                                dtR(0) = ""
                                dtR(1) = "<input type=text style=""width:300px; border-style:none"" readonly name=Val4 value="""">"
                                dtR(2) = ""
                                dtR(3) = "<input type=text style=""width:170px; border-style:none"" readonly name=Accr4 value="""">"
                                dtR(4) = "<select disabled=true name=""ddlVisibilita4"">" & vbCrLf & "<option selected value=""0"">Privata</option>" & vbCrLf & "<option value=""1"">Riservata</option>" & vbCrLf & "<option value=""2"">Pubblica</option>" & vbCrLf & "</select>"

                                dtT.Rows.Add(dtR)
                        End Select
                    End If
                End If
                '********************************************************
                'controllo e chiudo se aperto il datareader
                If Not dtrCaricaPersonaleEnte Is Nothing Then
                    dtrCaricaPersonaleEnte.Close()
                    dtrCaricaPersonaleEnte = Nothing
                End If
                'assegno alla datagrid il datatable creato
                'dtgRuoloPrincipale.DataSource = dtT
                'dtgRuoloPrincipale.DataBind()
                '***************************************************
                'creo la hidden che mi consente di verificare se ho selezionato o no ruoli
                Response.Write("<input type=hidden name=chkRuoli value=""False"">")
            Else 'If Session("dtvRuoli") Is Nothing Then
                'altrimenti vengo da nuova selezione
                '.DataSource = Session("dtvRuoli")
                'dtgRuoloPrincipale.DataBind()

                dtgRuoli.DataSource = Session("dtvRuoli")
                dtgRuoli.DataBind()

                Session("dtvRuoli") = Nothing
                Response.Write("<input type=hidden name=chkRuoli value=""True"">")
            End If
        End If
        'controllo e chiudo se aperto il datareader
        If Not dtrCaricaPersonaleEnte Is Nothing Then
            dtrCaricaPersonaleEnte.Close()
            dtrCaricaPersonaleEnte = Nothing
        End If

    End Sub

    'Sub PersonalizzanelPostBack()
    '    Dim dtR As DataRow
    '    Dim dtT As New DataTable
    '    Dim strsql As String
    '    'variabile contatore
    '    Dim i As Integer
    '    'se si tratta di inserimento
    '    If Session("ModIns") = 2 Then
    '        'controllo se i valori dei quattro campi hidden preimpostati siano vuoti
    '        If Request.Form("IdVal1") Is Nothing Or Request.Form("IdVal1") = "" Then
    '            'pulisco la datagrid dei ruoli
    '            dtgRuoloPrincipale.DataSource = Nothing
    '            dtgRuoloPrincipale.DataBind()
    '            'setto le intestazioni delle colonne
    '            dtT.Columns.Add(New DataColumn("IDRuolo", GetType(String)))
    '            dtT.Columns.Add(New DataColumn("Ruolo", GetType(String)))
    '            dtT.Columns.Add(New DataColumn("Principale", GetType(String)))
    '            dtT.Columns.Add(New DataColumn("Accreditato", GetType(String)))
    '            dtT.Columns.Add(New DataColumn("Visibilità", GetType(String)))

    '            dtT.Columns.Add()
    '            'alla riga assegno la riga delle intestazioni appena creata
    '            dtR = dtT.NewRow()
    '            'carico la prima riga della datagrid
    '            dtR(0) = ""
    '            dtR(1) = "<input type=text style=""width:300px; border-style:none"" readonly name=Val1 value=""Nessun Ruolo Selezionato."">"
    '            dtR(2) = "<img src=images/selezionato_small.png border=0 style='width:20px;height:20px'>"
    '            dtR(3) = "<input type=text style=""width:170px; border-style:none"" readonly name=Accr1 value="""">"
    '            dtR(4) = "<select disabled=true name=""ddlVisibilita1"">" & vbCrLf & "<option value=""0"">Privata</option>" & vbCrLf & "<option value=""1"">Riservata</option>" & vbCrLf & "<option value=""2"">Pubblica</option>" & vbCrLf & "</select>"

    '            'aggiungo la riga appena caricata alla datatable
    '            dtT.Rows.Add(dtR)
    '            'carico le altre tre righe con campi e txt vuote
    '            For i = 2 To 4
    '                dtR = dtT.NewRow()
    '                dtR(0) = ""
    '                dtR(1) = "<input type=text style=""width:300px; border-style:none"" readonly name=Val" & i & " value="""">"
    '                dtR(2) = ""
    '                dtR(3) = "<input type=text style=""width:170px; border-style:none"" readonly name=Accr" & i & " value="""">"
    '                dtR(4) = "<select disabled=true name=""ddlVisibilita" & i.ToString & """>" & vbCrLf & "<option value=""0"">Privata</option>" & vbCrLf & "<option value=""1"">Riservata</option>" & vbCrLf & "<option value=""2"">Pubblica</option>" & vbCrLf & "</select>"

    '                dtT.Rows.Add(dtR)
    '            Next
    '            'assegno alla datagrid la datatable appena creata
    '            dtgRuoloPrincipale.DataSource = dtT
    '            dtgRuoloPrincipale.DataBind()
    '        End If
    '    End If
    'End Sub

    Private Function ControlloRisorsaAccreditata(ByVal IDEntePersonale As Integer) As Boolean
        '22/08/2014
        'Verifica se esiste almeno una risorsa accreditata
        'rendo non modificabili i   campi      ddlEsperienzaServizioCivile,        chkCorsoOlp.        ddlCorso()

        Dim item As DataGridItem
        Dim IDRuolo As String
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
        For Each item In dtgRuoli.Items
            IDRuolo = item.Cells(0).Text
            If IDRuolo = "&nbsp;" Then
                IDRuolo = -1
            End If
            'If (Session("TipoUtente") = "U" Or Session("TipoUtente") = "R") Then
            'Verifico che l'Ente in Sessione  sia un ente di 3 o 4 classe
            strsql = "select Accreditato,idruolo  From entepersonaleruoli where IDEntePersonale= " & IDEntePersonale & " and IDRuolo =" & IDRuolo & " "

            dtrgenerico = ClsServer.CreaDatareader(strsql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
            If dtrgenerico.HasRows = True Then
                dtrgenerico.Read()
                If dtrgenerico("Accreditato") = 1 And dtrgenerico("idruolo") = 2 Then
                    If Not dtrgenerico Is Nothing Then
                        dtrgenerico.Close()
                        dtrgenerico = Nothing
                    End If
                    Return True
                    Exit Function
                End If
            End If
            If Not dtrgenerico Is Nothing Then
                dtrgenerico.Close()
                dtrgenerico = Nothing
            End If
        Next

        Return False

    End Function

    'Sub PersonalizzaFuoriPostBack()
    '    Dim dtR As DataRow
    '    Dim dtT As New DataTable
    '    Dim strsql As String
    '    'variabile contatore
    '    Dim i As Integer
    '    'se si tratta di inserimento
    '    If Session("ModIns") = 2 Then
    '        'controlo se la prima riga è vuota
    '        If Not Request.Form("IdVal1") Is Nothing Or Request.Form("IdVal1") <> "" Or Request.Form("IdVal1") <> "&nbsp;" Then
    '            'se non lo è imposto le colonne della datagrid
    '            dtT.Columns.Add(New DataColumn("IDRuolo", GetType(String)))
    '            dtT.Columns.Add(New DataColumn("Ruolo", GetType(String)))
    '            dtT.Columns.Add(New DataColumn("Principale", GetType(String)))
    '            dtT.Columns.Add(New DataColumn("Accreditato", GetType(String)))
    '            dtT.Columns.Add(New DataColumn("Visibilità", GetType(String)))

    '            dtT.Columns.Add()
    '            dtR = dtT.NewRow()
    '            'assegno Request.Form("Val1") alla txt Val1 della descrizione del ruolo (valore postato nel refresh della pagina)
    '            dtR(0) = ""
    '            dtR(1) = "<input type=text style=""width:300px; border-style:none"" readonly name=Val1 value=""" & Request.Form("Val1") & """>"
    '            dtR(2) = "<img src=images/selezionato_small.png border=0 style='width:20px;height:20px'>"
    '            dtR(3) = "<input type=text style=""width:300px; border-style:none"" readonly name=Accr1 value=""" & Request.Form("Accr1") & """>"
    '            'controllo e seleziono l'item nella combo della Visibilità con il valore selezionato prima del refresh (Request.Form("ddlVisibilita1") legge il value della combo)
    '            If Request.Form("ddlVisibilita1") = "0" Then
    '                dtR(4) = "<select name=""ddlVisibilita1"">" & vbCrLf & "<option selected value=""0"">Privata</option>" & vbCrLf & "<option value=""1"">Riservata</option>" & vbCrLf & "<option value=""2"">Pubblica</option>" & vbCrLf & "</select>"
    '            End If
    '            'controllo e carico la combo della Visibilità con il valore selezionato prima del refresh (Request.Form("ddlVisibilita1") legge il value della combo)
    '            If Request.Form("ddlVisibilita1") = "1" Then
    '                dtR(4) = "<select name=""ddlVisibilita1"">" & vbCrLf & "<option value=""0"">Privata</option>" & vbCrLf & "<option selected value=""1"">Riservata</option>" & vbCrLf & "<option value=""2"">Pubblica</option>" & vbCrLf & "</select>"
    '            End If
    '            'controllo e carico la combo della Visibilità con il valore selezionato prima del refresh (Request.Form("ddlVisibilita1") legge il value della combo)
    '            If Request.Form("ddlVisibilita1") = "2" Then
    '                dtR(4) = "<select name=""ddlVisibilita1"">" & vbCrLf & "<option value=""0"">Privata</option>" & vbCrLf & "<option value=""1"">Riservata</option>" & vbCrLf & "<option selected value=""2"">Pubblica</option>" & vbCrLf & "</select>"
    '            End If
    '            'controllo e carico la combo della Visibilità con il valore selezionato prima del refresh (Request.Form("ddlVisibilita1") legge il value della combo e se nothing carico cmq la combo vuota)
    '            If Request.Form("ddlVisibilita1") Is Nothing Then
    '                dtR(4) = "<select disabled name=""ddlVisibilita1"">" & vbCrLf & "<option selected value=""0"">Privata</option>" & vbCrLf & "<option value=""1"">Riservata</option>" & vbCrLf & "<option value=""2"">Pubblica</option>" & vbCrLf & "</select>"
    '            End If

    '            'carico la riga appena creata sulla datatable
    '            dtT.Rows.Add(dtR)
    '            'faccio la stessa cosa per le restanti tre righe
    '            For i = 2 To 4
    '                'controllo se c'era qualcosa nella prima riga
    '                If Request.Form("IdVal" & i.ToString) <> "" Then
    '                    dtR = dtT.NewRow()
    '                    dtR(0) = ""
    '                    'assegno Request.Form("Val di i") alla txt Val di i della descrizione del ruolo (valore postato nel refresh della pagina)
    '                    dtR(1) = "<input type=text style=""width:300px; border-style:none"" readonly name=Val" & i & " value=""" & Request.Form("Val" & i.ToString) & """>"
    '                    dtR(2) = ""
    '                    'assegno Request.Form("Accr di i") alla txt Accr di i accreditamento ruolo (valore postato nel refresh della pagina)
    '                    dtR(3) = "<input type=text style=""width:170px; border-style:none"" readonly name=Accr" & i & " value=""" & Request.Form("Accr" & i.ToString) & """>"
    '                    'controllo e seleziono l'item nella combo della Visibilità con il valore selezionato prima del refresh (Request.Form("ddlVisibilita di i") legge il value della combo)
    '                    If Request.Form("ddlVisibilita" & i.ToString) = "0" Then
    '                        dtR(4) = "<select name=""ddlVisibilita" & i.ToString & """>" & vbCrLf & "<option selected value=""0"">Privata</option>" & vbCrLf & "<option value=""1"">Riservata</option>" & vbCrLf & "<option value=""2"">Pubblica</option>" & vbCrLf & "</select>"
    '                    End If
    '                    'controllo e seleziono l'item nella combo della Visibilità con il valore selezionato prima del refresh (Request.Form("ddlVisibilita1") legge il value della combo)
    '                    If Request.Form("ddlVisibilita" & i.ToString) = "1" Then
    '                        dtR(4) = "<select name=""ddlVisibilita" & i.ToString & """>" & vbCrLf & "<option value=""0"">Privata</option>" & vbCrLf & "<option selected value=""1"">Riservata</option>" & vbCrLf & "<option value=""2"">Pubblica</option>" & vbCrLf & "</select>"
    '                    End If
    '                    'controllo e seleziono l'item nella combo della Visibilità con il valore selezionato prima del refresh (Request.Form("ddlVisibilita1") legge il value della combo)
    '                    If Request.Form("ddlVisibilita" & i.ToString) = "2" Then
    '                        dtR(4) = "<select name=""ddlVisibilita" & i.ToString & """>" & vbCrLf & "<option value=""0"">Privata</option>" & vbCrLf & "<option value=""1"">Riservata</option>" & vbCrLf & "<option selected value=""2"">Pubblica</option>" & vbCrLf & "</select>"
    '                    End If

    '                    'carico la riga appena creata alla datatable
    '                    dtT.Rows.Add(dtR)
    '                Else  'If Request.Form("IdVal" & i.ToString) <> "" Then
    '                    'carico una riga vuota se non avevo nulla prima del refresh
    '                    dtR = dtT.NewRow()
    '                    dtR(0) = ""
    '                    dtR(1) = "<input type=text style=""width:300px; border-style:none"" readonly name=Val" & i & " value="""">"
    '                    dtR(2) = ""
    '                    dtR(3) = "<input type=text style=""width:170px; border-style:none"" readonly name=Accr" & i & " value="""">"
    '                    dtR(4) = "<select disabled=true name=""ddlVisibilita" & i.ToString & """>" & vbCrLf & "<option selected value=""0"">Privata</option>" & vbCrLf & "<option value=""1"">Riservata</option>" & vbCrLf & "<option value=""2"">Pubblica</option>" & vbCrLf & "</select>"

    '                    dtT.Rows.Add(dtR)
    '                End If
    '            Next
    '            'se non ho finito di caricare le righe finisco di farlo
    '            If i < 4 Then
    '                Select Case i
    '                    Case 2
    '                        For i = 3 To 4
    '                            dtR = dtT.NewRow()
    '                            dtR(0) = ""
    '                            dtR(1) = "<input type=text style=""width:300px; border-style:none"" readonly name=Val" & i.ToString & " value="""">"
    '                            dtR(2) = ""
    '                            dtR(3) = "<input type=text style=""width:170px; border-style:none"" readonly name=Accr" & i.ToString & " value="""">"
    '                            dtR(4) = "<select disabled=true name=""ddlVisibilita" & i & """>" & vbCrLf & "<option selected value=""0"">Privata</option>" & vbCrLf & "<option value=""1"">Riservata</option>" & vbCrLf & "<option value=""2"">Pubblica</option>" & vbCrLf & "</select>"

    '                            dtT.Rows.Add(dtR)
    '                        Next
    '                    Case 3
    '                        dtR = dtT.NewRow()
    '                        dtR(0) = ""
    '                        dtR(1) = "<input type=text style=""width:300px; border-style:none"" readonly name=Val4 value="""">"
    '                        dtR(2) = ""
    '                        dtR(3) = "<input type=text style=""width:170px; border-style:none"" readonly name=Accr4 value="""">"
    '                        dtR(4) = "<select disabled=true name=""ddlVisibilita4"">" & vbCrLf & "<option selected value=""0"">Privata</option>" & vbCrLf & "<option value=""1"">Riservata</option>" & vbCrLf & "<option value=""2"">Pubblica</option>" & vbCrLf & "</select>"

    '                        dtT.Rows.Add(dtR)
    '                End Select
    '            End If
    '            'If Not Request.Form("IdVal1") Is Nothing Or Request.Form("IdVal1") <> "" Or Request.Form("IdVal1") <> "&nbsp;" Then
    '        Else
    '            'carico le tre righe restanti a vuoto
    '            For i = 2 To 4
    '                dtR = dtT.NewRow()
    '                dtR(0) = ""
    '                dtR(1) = "<input type=text style=""width:300px; border-style:none"" readonly name=Val" & i & " value="""">"
    '                dtR(2) = ""
    '                dtR(3) = "<input type=text style=""width:170px; border-style:none"" readonly name=Accr" & i & " value="""">"
    '                dtR(4) = "<select disabled=true name=""ddlVisibilita" & i.ToString & """>" & vbCrLf & "<option selected value=""0"">Privata</option>" & vbCrLf & "<option value=""1"">Riservata</option>" & vbCrLf & "<option value=""2"">Pubblica</option>" & vbCrLf & "</select>"

    '                dtT.Rows.Add(dtR)
    '            Next
    '        End If
    '        'assegno la datatable creata al source della datagrid
    '        dtgRuoloPrincipale.DataSource = dtT
    '        dtgRuoloPrincipale.DataBind()
    '    End If
    '    'controllo se refreshando avevo selezionato un comune così creo la hidden con il value del suo id
    '    If Not Request.Form("txtIDComuneNascita") Is Nothing Then
    '        If Request.Form("txtIDComuneNascita") <> "" Then
    '            Response.Write("<input type=hidden name=IdComuneNascita value=""" & IIf(Trim(Request.Form("txtIDComuneNascita")) = "", 0, CInt(Trim(Request.Form("txtIDComuneNascita")))) & """>")
    '        Else
    '            Response.Write("<input type=hidden name=IdComuneNascita value="""">")
    '        End If
    '    End If
    '    'controllo se refreshando avevo selezionato un comune così creo la hidden con il value del suo id
    '    If Not Request.Form("txtIDComuneResidenza") Is Nothing Then
    '        If Request.Form("txtIDComuneResidenza") <> "" Then
    '            Response.Write("<input type=hidden name=IdComuneResidenza value=""" & IIf(Trim(Request.Form("txtIDComuneResidenza")) = "", 0, CInt(Trim(Request.Form("txtIDComuneResidenza")))) & """>")
    '        Else
    '            Response.Write("<input type=hidden name=IdComuneResidenza value="""">")
    '        End If
    '    End If
    'End Sub

    Private Sub cmdSalva_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdSalva.Click
        Dim CV As Allegato = Session("LoadedCV")

        If CV IsNot Nothing Then
            'CV.Filename = "CV_" & txtCodiceFiscale.Text & System.IO.Path.GetExtension(CV.Filename)
            Session("LoadedCV") = CV
            txtCVFilename.Text = CV.Filename
        End If
        'variabile stringa per la insert nella base dati
        Dim strInsertCommand As String = ""
        'adapter che conterrà l'id del tizio inserito
        Dim dtrIdMax As SqlClient.SqlDataReader
        'dtr per controllo esistenza codice fiscale
        Dim dtrCheckCodFis As SqlClient.SqlDataReader
        'variabile contatore
        Dim i As Integer
        Dim blnCheckCodFis As Boolean = False
        Dim strCodCatasto As String
        Dim strNewCF As String
        Dim cmdGenerico As SqlClient.SqlCommand
        Dim strValoreRitornoStore As String

        lblErr.Text = ""
        lblErr.Visible = False


        If controlliSalvataggioServer() = False Then
            Exit Sub
        End If
        'aggiunto il 19/07/2017 da s.c.
        Dim AlboEnte As String
        AlboEnte = ClsUtility.TrovaAlboEnte(Session("IdEnte"), Session("Conn"))

        If txtCAP.Text = "" And ddlComuneResidenza.SelectedIndex = 0 And txtCivico.Text = "" And txtIndirizzo.Text = "" Then

        Else
            '''''Controllo esistenza cap
            ''''If ClsUtility.ControllaEsistenzaCap(IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")), Trim(txtCAP.Text), IIf(Request.Form("txtIDComuneResidenza") = "", txtIDComunes.Text, Request.Form("txtIDComuneResidenza")), "") = False Then
            ''''    'If Session("ModIns") = 1 Then
            ''''    '    CaricaAnagrafica()
            ''''    'End If
            ''''    lblErr.Visible = True
            ''''    lblErr.Text = "Il CAP inserito non è congruo rispetto al comune selezionato."
            ''''    Imgerrore.Visible = True
            ''''    messaggiosopra.Text = ""
            ''''    messaggiosopra.Visible = False
            ''''    Exit Sub
            ''''    '***********************************************************
            ''''End If

            Dim strMiaCausale As String = ""
            If ClsUtility.CAP_VERIFICA(IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")),
              strMiaCausale, bandiera, Trim(txtCAP.Text), ddlComuneResidenza.SelectedItem.Value.ToString, "", "", txtIndirizzo.Text, txtCivico.Text) = False Then
                'Ripristino lo stato del tasto
                cmdSalva.Visible = True
                'Inserisco il Messaggio di Errore
                MessaggiAlert(strMiaCausale)
                If Not dtrgenerico Is Nothing Then
                    dtrgenerico.Close()
                    dtrgenerico = Nothing
                End If
                Exit Sub
            End If

        End If

        'Dim strCodCatasto As String
        'Dim strNewCF As String
        'If Len(txtCodiceFiscale.Text) <> 16 Then
        '    Response.Write("<script>")
        '    Response.Write("alert('CCodice Fiscale non corretto.')")
        '    Response.Write("</script>")
        '    Exit Sub
        'Else
        '    'ricavo il codice catastale del comune
        '    strCodCatasto = ClsUtility.GetCodiceCatasto(Session("conn"), CInt(Request.Form("txtIDComuneNascita")))
        '    'genero il CF con i dati anagrafici del volontario
        '    strNewCF = ClsUtility.CreaCF(Trim(Replace(txtCognome.Text, "'", "''")), Trim(Replace(txtNome.Text, "'", "''")), Trim(txtDataNascita.Text), strCodCatasto, Trim(strSesso))
        '    'lo confronto con il CF inserito nel CSV
        '    If UCase(Trim(txtCodiceFiscale.Text)) <> UCase(strNewCF) Then
        '        'verifo eventuale OMOCODIA
        '        If ClsUtility.VerificaOmocodia(UCase(strNewCF), UCase(Trim(txtCodiceFiscale.Text))) = False Then
        '            Response.Write("<script>")
        '            Response.Write("alert('Codice Fiscale non congruente con i dati inseriti.')")
        '            Response.Write("</script>")
        '            Exit Sub
        '        End If
        '    End If
        'End If
        'IMPOSSIBILE VERIFICARE IL CF IN MANIERA CORRETTA IN QUANTO MANCA IL DATO RELATIVO AL SESSO.
        'Antonello---------------------------------
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
        '''If verificamulticap() = True Then
        '''    If verificaindirizzo() = True Then
        '''        'procedo con l'inserimento
        '''        bandiera = 0
        '''    Else
        '''        messaggiosopra.Visible = True
        '''        messaggiosopra.ForeColor = Color.Red
        '''        messaggiosopra.Text = "Impossibili  inserire l'indirizzo scelto.Indirizzo non presente in banca dati contattare assistenza Helios."
        '''        Exit Sub
        '''    End If
        '''Else
        '''    'metto il marcatore 1 chiamato bandiera
        '''    'inserisco l'indirizzo
        '''    bandiera = 1
        '''End If
        '''If Not dtrgenerico Is Nothing Then
        '''    dtrgenerico.Close()
        '''    dtrgenerico = Nothing
        '''End If
        '------------------------------------------------

        Dim blnCheckSesso As Boolean = True


        'controllo codice fiscale
        If txtCodiceFiscale.Text <> "" Then
            If NazionalitaItaliana(ddlComuneNascita.SelectedItem.Text) = True Or NazionalitaItaliana(ddlComuneResidenza.SelectedItem.Text) = True Then
                If Len(txtCodiceFiscale.Text) <> 16 Then
                    MessaggiAlert("Codice Fiscale non corretto.")
                    If Not dtrgenerico Is Nothing Then
                        dtrgenerico.Close()
                        dtrgenerico = Nothing
                    End If
                    Exit Sub
                End If
                'ricavo il codice catastale del comune
                strCodCatasto = ClsUtility.GetCodiceCatasto(Session("conn"), ddlComuneNascita.SelectedValue)
                'genero il CF con i dati anagrafici del volontario
                strNewCF = ClsUtility.CreaCF(Trim(Replace(txtCognome.Text, "'", "''")), Trim(Replace(txtNome.Text, "'", "''")), Trim(txtDataNascita.Text), strCodCatasto, "M")
                'lo confronto con il CF inserito nel CSV
                If UCase(Trim(txtCodiceFiscale.Text)) <> UCase(strNewCF) Then
                    'genero il CF con i dati anagrafici del volontario
                    strNewCF = ClsUtility.CreaCF(Trim(Replace(txtCognome.Text, "'", "''")), Trim(Replace(txtNome.Text, "'", "''")), Trim(txtDataNascita.Text), strCodCatasto, "F")
                    If UCase(Trim(txtCodiceFiscale.Text)) <> UCase(strNewCF) Then
                        'verifo eventuale OMOCODIA
                        If ClsUtility.VerificaOmocodia(UCase(strNewCF), UCase(Trim(txtCodiceFiscale.Text))) = False Then
                            blnCheckSesso = False
                        Else
                            blnCheckSesso = True
                        End If
                    End If
                Else
                    blnCheckSesso = True
                End If
                If blnCheckSesso = False Then
                    MessaggiAlert("Codice Fiscale non corretto.")
                    If Not dtrgenerico Is Nothing Then
                        dtrgenerico.Close()
                        dtrgenerico = Nothing
                    End If
                    Exit Sub
                End If
            End If
        Else
            MessaggiAlert("Inserire il Codice Fiscale.")
            If Not dtrgenerico Is Nothing Then
                dtrgenerico.Close()
                dtrgenerico = Nothing
            End If
            Exit Sub
        End If
        'Dim EntePers As Integer
        'If Request.QueryString("intPersonaleEnteAssociato") = Nothing Then
        '    EntePers = 0
        'Else
        '    EntePers = Request.QueryString("intPersonaleEnteAssociato")
        'End If
        'If ControlloInserimentoFormatore(Session("ModIns"), EntePers) = True Then
        '    If ddlEsperienzaServizioCivile.SelectedValue = 0 Then
        '        MessaggiAlert("E' necessario indicare il campo Esperienza Servizio Civile.")
        '        If Not dtrgenerico Is Nothing Then
        '            dtrgenerico.Close()
        '            dtrgenerico = Nothing
        '        End If
        '        Exit Sub
        '    End If
        '    If ddlCorso.SelectedValue = 0 Then
        '        MessaggiAlert("Il Corso di Formazione è un campo obbligatorio.")
        '        If Not dtrgenerico Is Nothing Then
        '            dtrgenerico.Close()
        '            dtrgenerico = Nothing
        '        End If
        '        Exit Sub
        '    End If
        'End If
        'ControlloFormatore()

        Dim EntePers As Integer
        If Request.QueryString("intPersonaleEnteAssociato") = Nothing Then
            EntePers = 0
        Else
            EntePers = Request.QueryString("intPersonaleEnteAssociato")
        End If
        If ControlloInserimentoFormatore(Session("ModIns"), EntePers, AlboEnte) = True Then


            If ddlEsperienzaServizioCivile.SelectedValue = 0 Then
                MessaggiAlert("E' necessario indicare il campo Esperienza Servizio Civile.")
                If Not dtrgenerico Is Nothing Then
                    dtrgenerico.Close()
                    dtrgenerico = Nothing
                End If
                Exit Sub
            End If
            If ddlCorso.SelectedValue = 0 Then
                MessaggiAlert("Il Corso di Formazione è un campo obbligatorio.")
                If Not dtrgenerico Is Nothing Then
                    dtrgenerico.Close()
                    dtrgenerico = Nothing
                End If
                Exit Sub
            End If
        End If


        'Controllo CV

        If CV Is Nothing Then
            MessaggiAlert("È necessario inserire il Curriculum Vitae")
            Exit Sub
        End If

        Dim idEntePersonale As Integer? = Request.QueryString("intPersonaleEnteAssociato")
        Dim sqlControlloCV As String = "SELECT COUNT(*) FROM entidocumenti A JOIN entepersonale P ON P.IdAllegatoCV = A.IdEnteDocumento "
        sqlControlloCV += "WHERE HashValue=@Hash "
        If idEntePersonale.HasValue Then
            sqlControlloCV = sqlControlloCV + " AND IdEntePersonale<>@IdEntePersonale"
        End If
        Dim commandControlloCV As New SqlCommand(sqlControlloCV, Session("conn"))
        commandControlloCV.Parameters.AddWithValue("@IdEnte", CInt(Session("idEnte")))
        commandControlloCV.Parameters.AddWithValue("@IdEntePersonale", idEntePersonale)
        commandControlloCV.Parameters.AddWithValue("@Hash", CV.Hash)
        Dim numeroAllegati As Integer = commandControlloCV.ExecuteScalar
        If numeroAllegati > 0 Then
            MessaggiAlert("Il Curriculum Vitae inserito è stato già associato ad un altro soggetto")
            rowAllegato.Style.Add("background-color", "coral")
            Exit Sub
        End If

        'inserimento
        If Session("ModIns") = 2 Then 'inserimento

            'blocco try che verifica se la insert va a buon fine
            Try
                'If Request.Form("chkCodiceFiscale") <> "2" Then
                'carico la stringa della select di controllo esistenza codicefiscale
                strInsertCommand = "select CodiceFiscale from entepersonale "
                strInsertCommand = strInsertCommand & "where CodiceFiscale='" & Trim(txtCodiceFiscale.Text) & "'"
                'controllo e chiudo se aperto il datareader
                If Not dtrCheckCodFis Is Nothing Then
                    dtrCheckCodFis.Close()
                    dtrCheckCodFis = Nothing
                End If
                'eseguo la query
                dtrCheckCodFis = ClsServer.CreaDatareader(strInsertCommand, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
                'se ci sono dei record
                If dtrCheckCodFis.HasRows = True Then
                    'leggo il datareader
                    dtrCheckCodFis.Read()
                    'If Request.Form("chkCodiceFiscale") <> "2" Then
                    'carico la stringa della select di controllo esistenza codicefiscale
                    strInsertCommand = "select CodiceFiscale from entepersonale "
                    strInsertCommand = strInsertCommand & "where idente='" & Session("IdEnte") & "' and CodiceFiscale='" & Trim(txtCodiceFiscale.Text) & "'"
                    'controllo e chiudo se aperto il datareader
                    If Not dtrCheckCodFis Is Nothing Then
                        dtrCheckCodFis.Close()
                        dtrCheckCodFis = Nothing
                    End If
                    'eseguo la query
                    dtrCheckCodFis = ClsServer.CreaDatareader(strInsertCommand, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
                    'leggo il datareader
                    dtrCheckCodFis.Read()
                    'se ci sono dei record
                    If dtrCheckCodFis.HasRows = True Then
                        If Not dtrCheckCodFis Is Nothing Then
                            dtrCheckCodFis.Close()
                            dtrCheckCodFis = Nothing
                        End If

                        strInsertCommand = "select * from entepersonaleruoli as a "
                        strInsertCommand += "inner join entepersonale as b on a.identepersonale=b.identepersonale "
                        'gianluigi 06/08/2007  se ente e ruoloaccreditamento=1 lascio inserire
                        'If Session("TipoUtente") = "E" Then
                        strInsertCommand += "inner join ruoli c on a.idruolo=c.idruolo "
                        'End If
                        strInsertCommand += " where c.Nascosto = 0 and b.codicefiscale='" & Trim(txtCodiceFiscale.Text) & "' and b.idente='" & Session("IdEnte") & "'"  ' and a.idruolo in ('1','5','6')"
                        'gianluigi 06/08/2007  se ente e ruoloaccreditamento=1 lascio inserire
                        If Session("TipoUtente") = "E" Then
                            strInsertCommand += " and RuoloAccreditamento = 1"
                        End If

                        'controllo e chiudo se aperto il datareader
                        If Not dtrCheckCodFis Is Nothing Then
                            dtrCheckCodFis.Close()
                            dtrCheckCodFis = Nothing
                        End If
                        'eseguo la query
                        dtrCheckCodFis = ClsServer.CreaDatareader(strInsertCommand, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
                        'leggo il datareader
                        dtrCheckCodFis.Read()
                        'se ci sono dei record
                        If Not dtrCheckCodFis.HasRows Then
                            'coloro di grigio la txt del codice fiscale
                            txtCodiceFiscale.BackColor = Color.FromArgb(242, 185, 63)
                            'avverto l'utente dell'esistenza del codice fiscale che si vuole inseirire
                            lblErr.Text = "La Risorsa e' stata salvata. Attenzione, il Codice Fiscale risulta essere gia' in uso."
                            Log.Warning(LogEvent.RISORSE_ENTE_INSERIMENTO, "Codice Fiscale già in uso")
                            blnCheckCodFis = True
                            'controllo e chiudo se aperto il datareader
                            If Not dtrCheckCodFis Is Nothing Then
                                dtrCheckCodFis.Close()
                                dtrCheckCodFis = Nothing
                            End If
                        Else
                            lblErr.Text = "La Risorsa non e' stata salvata. Il Codice Fiscale risulta essere gia' in uso per questo ente."
                            lblErr.Visible = True
                            Log.Warning(LogEvent.RISORSE_ENTE_ERRORE_INSERIMENTO, "Codice Fiscale già in uso")
                            blnCheckCodFis = False
                            If Not dtrCheckCodFis Is Nothing Then
                                dtrCheckCodFis.Close()
                                dtrCheckCodFis = Nothing
                            End If
                        End If
                        If Not dtrCheckCodFis Is Nothing Then
                            dtrCheckCodFis.Close()
                            dtrCheckCodFis = Nothing
                        End If

                    Else
                        'coloro di grigio la txt del codice fiscale
                        txtCodiceFiscale.BackColor = Color.FromArgb(242, 185, 63)
                        'avverto l'utente dell'esistenza del codice fiscale che si vuole inseirire
                        lblErr.Text = "La Risorsa e' stata salvata. Attenzione, il Codice Fiscale risulta essere gia' in uso."
                        lblErr.Visible = True
                        blnCheckCodFis = True
                        'controllo e chiudo se aperto il datareader
                        If Not dtrCheckCodFis Is Nothing Then
                            dtrCheckCodFis.Close()
                            dtrCheckCodFis = Nothing
                        End If
                    End If
                    'Response.Write("<input type=hidden name=PassaCodiceFiscale value='1'>")
                    'Else
                    '    Response.Write("<input type=hidden name=PassaCodiceFiscale value='0'>")



                Else
                    If Not dtrCheckCodFis Is Nothing Then
                        dtrCheckCodFis.Close()
                        dtrCheckCodFis = Nothing
                    End If

                    'Leucci 25/09/2018
                    'la stringa conterrà o il messaggio di errore o l'id del ruolo principale
                    strValoreRitornoStore = ControlloIdRuoli()

                    'controllo se si tratta di un id o di un messaggio d'errore
                    If IsNumeric(strValoreRitornoStore) = False Then
                        lblErr.Visible = True
                        lblErr.Text = strValoreRitornoStore
                        Exit Sub
                    End If


                    'Creazione Allegato
                    Dim idAllegato = InsertCV(CV, strIdEntefase)

                    'caricamento della stringa contenente l'istruzione per la insert nella tabella entepersonale
                    strInsertCommand = "insert into entepersonale (IDEnte,Cognome,Nome,Titolo,Posizione,Cellulare,Email,Telefono,Fax,IDComuneNascita,DataNascita,IDComuneResidenza,Indirizzo,Civico,Cap,CodiceFiscale,DataCreazioneRecord,UsernameInseritore,CorsoOLP,EsperienzaServizioCivile,Corso,DettaglioRecapito,FlagIndirizzoValido,IdAllegatoCV) "
                    strInsertCommand = strInsertCommand & "values (" & CInt(Session("idEnte")) & ",'" & Replace(Trim(txtCognome.Text), "'", "''") & "','" & Replace(Trim(txtNome.Text), "'", "''") & "','" & Replace(Trim(txtTitolo.Text), "'", "''") & "','" & Replace(Trim(txtPosizione.Text), "'", "''") & "',"
                    strInsertCommand = strInsertCommand & "'" & Trim(txtCellulare.Text) & "','" & Replace(Trim(txtEmail.Text), "'", "''") & "',"
                    strInsertCommand = strInsertCommand & "'" & Trim(txtTelefono.Text) & "','" & Replace(Trim(txtFax.Text), "'", "''") & "',"

                    If ddlComuneNascita.SelectedIndex > 0 Then
                        strInsertCommand = strInsertCommand & CInt(ddlComuneNascita.SelectedValue) & ",'" & Day(txtDataNascita.Text) & "-" & Month(txtDataNascita.Text) & "-" & Year(txtDataNascita.Text) & "',"
                    Else
                        strInsertCommand = strInsertCommand & "null,'" & Day(txtDataNascita.Text) & "-" & Month(txtDataNascita.Text) & "-" & Year(txtDataNascita.Text) & "',"
                    End If

                    If ddlComuneResidenza.SelectedIndex > 0 Then
                        strInsertCommand = strInsertCommand & CInt(ddlComuneResidenza.SelectedValue) & ",'" & Replace(txtIndirizzo.Text, "'", "''") & "','" & Replace(txtCivico.Text, "'", "''") & "','" & txtCAP.Text & "','" & Replace(txtCodiceFiscale.Text, "'", "''") & "',GetDate(),'" & Session("Utente") & "'," & IIf(chkCorsoOlp.Checked = True, 1, 0) & "," & ddlEsperienzaServizioCivile.SelectedValue & "," & ddlCorso.SelectedValue & ",'" & Replace(TxtDettaglioRecapito.Text, "'", "''") & "'," & bandiera & " "
                    Else
                        strInsertCommand = strInsertCommand & "null,'" & Replace(txtIndirizzo.Text, "'", "''") & "','" & Replace(txtCivico.Text, "'", "''") & "','" & txtCAP.Text & "','" & Replace(txtCodiceFiscale.Text, "'", "''") & "',GetDate(),'" & Session("Utente") & "'," & IIf(chkCorsoOlp.Checked = True, 1, 0) & "," & ddlEsperienzaServizioCivile.SelectedValue & "," & ddlCorso.SelectedValue & ",'" & Replace(TxtDettaglioRecapito.Text, "'", "''") & "'," & bandiera & " "
                    End If

                    strInsertCommand = strInsertCommand & "," & idAllegato & ")"
                    Dim intIdEntePersonaleLocal As Integer
                    Dim myCommand As New SqlClient.SqlCommand
                    'sql command che mi esegue la insert
                    myCommand = New SqlClient.SqlCommand(strInsertCommand, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
                    myCommand.ExecuteNonQuery()
                    myCommand.Dispose()
                    lblErr.Text = vbNullString
                    'trovo l'id max dell'utente appena inserito per associarlo
                    'al tizio appena aggiunto
                    strInsertCommand = "select top 1 IDEntePersonale from entepersonale where CodiceFiscale ='" & Trim(txtCodiceFiscale.Text) & "' order by identepersonale desc"
                    'controllo e chiudo se aperto il datareader
                    If Not dtrIdMax Is Nothing Then
                        dtrIdMax.Close()
                        dtrIdMax = Nothing
                    End If
                    'eseguo la query
                    dtrIdMax = ClsServer.CreaDatareader(strInsertCommand, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
                    dtrIdMax.Read()
                    'ulteriore controllo se non trovo id
                    If dtrIdMax.HasRows = True Then
                        'variabile locale che prende l'id max
                        intIdEntePersonaleLocal = CInt(dtrIdMax.Item("IdEntePersonale"))
                        'controllo e chiudo se aperto il datareader
                        If Not dtrIdMax Is Nothing Then
                            dtrIdMax.Close()
                            dtrIdMax = Nothing
                        End If
                        ' '' ''vado a controlare e ad inserire il primo e principale ruolo
                        '' ''If Request.Form("IdVal1") <> "" Then
                        '' ''    strInsertCommand = "insert into entepersonaleruoli (IDEntePersonale,"
                        '' ''    strInsertCommand = strInsertCommand & "IDRuolo,DataInizioValidità,Principale,Visibilità,DataInseritore,UserNameInseritore) values "
                        '' ''    strInsertCommand = strInsertCommand & "('" & intIdEntePersonaleLocal.ToString & "','"
                        '' ''    strInsertCommand = strInsertCommand & Request.Form("IdVal1") & "',getdate(),1," & CInt(Request.Form("ddlVisibilita1")) & ",GetDate(),'" & Session("Utente") & "')"
                        '' ''    'sql command momentaneo che mi esegue la insert
                        '' ''    myCommand = New SqlClient.SqlCommand(strInsertCommand, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
                        '' ''    myCommand.ExecuteNonQuery()
                        '' ''    myCommand.Dispose()
                        '' ''End If
                        ' '' ''inserisco, se presente il ruolo secondario
                        '' ''If Request.Form("IdVal2") <> "" Then
                        '' ''    strInsertCommand = "insert into entepersonaleruoli (IDEntePersonale,"
                        '' ''    strInsertCommand = strInsertCommand & "IDRuolo,DataInizioValidità,Principale,Visibilità,DataInseritore,UserNameInseritore) values "
                        '' ''    strInsertCommand = strInsertCommand & "('" & intIdEntePersonaleLocal.ToString & "','"
                        '' ''    strInsertCommand = strInsertCommand & Request.Form("IdVal2") & "',getdate(),0," & CInt(Request.Form("ddlVisibilita2")) & ",GetDate(),'" & Session("Utente") & "')"
                        '' ''    myCommand = New SqlClient.SqlCommand(strInsertCommand, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
                        '' ''    myCommand.ExecuteNonQuery()
                        '' ''    myCommand.Dispose()
                        '' ''End If
                        ' '' ''inserisco, se presente il ruolo secondario
                        '' ''If Request.Form("IdVal3") <> "" Then
                        '' ''    strInsertCommand = "insert into entepersonaleruoli (IDEntePersonale,"
                        '' ''    strInsertCommand = strInsertCommand & "IDRuolo,DataInizioValidità,Principale,Visibilità,DataInseritore,UserNameInseritore) values "
                        '' ''    strInsertCommand = strInsertCommand & "('" & intIdEntePersonaleLocal.ToString & "','"
                        '' ''    strInsertCommand = strInsertCommand & Request.Form("IdVal3") & "',getdate(),0," & CInt(Request.Form("ddlVisibilita3")) & ",GetDate(),'" & Session("Utente") & "')"
                        '' ''    myCommand = New SqlClient.SqlCommand(strInsertCommand, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
                        '' ''    myCommand.ExecuteNonQuery()
                        '' ''    myCommand.Dispose()
                        '' ''End If
                        ' '' ''inserisco, se presente, il ruolo secondario
                        '' ''If Request.Form("IdVal4") <> "" Then
                        '' ''    strInsertCommand = "insert into entepersonaleruoli (IDEntePersonale,"
                        '' ''    strInsertCommand = strInsertCommand & "IDRuolo,DataInizioValidità,Principale,Visibilità,DataInseritore,UserNameInseritore) values "
                        '' ''    strInsertCommand = strInsertCommand & "('" & intIdEntePersonaleLocal.ToString & "','"
                        '' ''    strInsertCommand = strInsertCommand & Request.Form("IdVal4") & "',getdate(),0," & CInt(Request.Form("ddlVisibilita4")) & ",GetDate(),'" & Session("Utente") & "')"
                        '' ''    myCommand = New SqlClient.SqlCommand(strInsertCommand, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
                        '' ''    myCommand.ExecuteNonQuery()
                        '' ''    myCommand.Dispose()
                        '' ''End If

                        'Leucci 25/09/2018
                        'la stringa conterrà o il messaggio di errore o l'id del ruolo principale
                        'strValoreRitornoStore = ControlloIdRuoli()

                        ''controllo se si tratta di un id o di un messaggio d'errore
                        'If IsNumeric(strValoreRitornoStore) = False Then
                        '    lblErr.Visible = True
                        '    lblErr.Text = strValoreRitornoStore
                        '    Exit Sub
                        'End If

                        For Each item In dtgRuoli.Items
                            Dim check As CheckBox = DirectCast(item.FindControl("chkAssegnaRuolo"), CheckBox)

                            'controllo se la check è spuntata o meno
                            If check.Checked = True Then
                                strInsertCommand = "insert into entepersonaleruoli (IDEntePersonale,"
                                strInsertCommand = strInsertCommand & "IDRuolo,DataInizioValidità,Principale,Visibilità,DataInseritore,UserNameInseritore) values "
                                strInsertCommand = strInsertCommand & "('" & intIdEntePersonaleLocal.ToString & "','"
                                strInsertCommand = strInsertCommand & item.Cells(0).Text & "',getdate(),0,0,GetDate(),'" & Session("Utente") & "')"
                                myCommand = New SqlClient.SqlCommand(strInsertCommand, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
                                myCommand.ExecuteNonQuery()
                                myCommand.Dispose()
                            End If

                        Next

                    End If
                    'controllo e chiudo se aperto il datareader
                    If Not dtrIdMax Is Nothing Then
                        dtrIdMax.Close()
                        dtrIdMax = Nothing
                    End If
                    lblErr.CssClass = "msgInfo"
                    lblErr.Visible = True
                    lblErr.Text = "Inserimento Effettuato"
                    Dim entity = New Entity() With {
                     .Id = intIdEntePersonaleLocal,
                     .Name = "EntePersonale"
                    }
                    Log.Information(LogEvent.RISORSE_ENTE_INSERIMENTO, entity:=entity)
                    SvuotaCampi()
                    CaricaRuoliInserimento(AlboEnte)
                End If
                'controllo e chiudo se aperto il datareader
                If Not dtrCheckCodFis Is Nothing Then
                    dtrCheckCodFis.Close()
                    dtrCheckCodFis = Nothing
                End If
                'End If
                If blnCheckCodFis = True Then

                    'Leucci 25/09/2018   
                    'la stringa conterrà o il messaggio di errore o l'id del ruolo principale
                    strValoreRitornoStore = ControlloIdRuoli()

                    'controllo se si tratta di un id o di un messaggio d'errore
                    If IsNumeric(strValoreRitornoStore) = False Then
                        lblErr.Visible = True
                        lblErr.Text = strValoreRitornoStore
                        Log.Warning(LogEvent.HELIOS_ERRORE_AUTORIZZAZIONE)
                        Exit Sub
                    End If

                    'Creazione Allegato

                    Dim idAllegato As Integer = InsertCV(CV, strIdEntefase)


                    'caricamento della stringa contenente l'istruzione per la insert nella tabella entepersonale
                    strInsertCommand = "insert into entepersonale (IDEnte,Cognome,Nome,Titolo,Posizione,Cellulare,Email,Telefono,Fax,IDComuneNascita,DataNascita,IDComuneResidenza,Indirizzo,Civico,Cap,CodiceFiscale,DataCreazioneRecord,UsernameInseritore,CorsoOLP,EsperienzaServizioCivile,Corso,DettaglioRecapito,FlagIndirizzoValido,idAllegatoCV) "
                    strInsertCommand = strInsertCommand & "values (" & CInt(Session("idEnte")) & ",'" & Replace(Trim(txtCognome.Text), "'", "''") & "','" & Replace(Trim(txtNome.Text), "'", "''") & "','" & Replace(Trim(txtTitolo.Text), "'", "''") & "','" & Replace(Trim(txtPosizione.Text), "'", "''") & "',"
                    strInsertCommand = strInsertCommand & "'" & Trim(txtCellulare.Text) & "','" & Replace(Trim(txtEmail.Text), "'", "''") & "',"
                    strInsertCommand = strInsertCommand & "'" & Trim(txtTelefono.Text) & "','" & Replace(Trim(txtFax.Text), "'", "''") & "',"
                    'If Request.Form("txtIDComuneNascita") <> "" Then
                    If ddlComuneNascita.SelectedIndex > 0 Then
                        strInsertCommand = strInsertCommand & CInt(ddlComuneNascita.SelectedValue) & ",'" & Day(txtDataNascita.Text) & "-" & Month(txtDataNascita.Text) & "-" & Year(txtDataNascita.Text) & "',"
                    Else
                        strInsertCommand = strInsertCommand & "null,'" & Day(txtDataNascita.Text) & "-" & Month(txtDataNascita.Text) & "-" & Year(txtDataNascita.Text) & "',"
                    End If
                    ' If Request.Form("txtIDComuneResidenza") <> "" Then
                    If ddlComuneResidenza.SelectedIndex > 0 Then
                        strInsertCommand = strInsertCommand & CInt(ddlComuneResidenza.SelectedValue) & ",'" & Replace(txtIndirizzo.Text, "'", "''") & "','" & Replace(txtCivico.Text, "'", "''") & "','" & txtCAP.Text & "','" & Replace(txtCodiceFiscale.Text, "'", "''") & "',GetDate(),'" & Session("Utente") & "'," & IIf(chkCorsoOlp.Checked = True, 1, 0) & "," & ddlEsperienzaServizioCivile.SelectedValue & "," & ddlCorso.SelectedValue & ",'" & Replace(TxtDettaglioRecapito.Text, "'", "''") & "'," & bandiera & " "
                    Else
                        strInsertCommand = strInsertCommand & "null,'" & Replace(txtIndirizzo.Text, "'", "''") & "','" & Replace(txtCivico.Text, "'", "''") & "','" & txtCAP.Text & "','" & UCase(Replace(txtCodiceFiscale.Text, "'", "''")) & "',GetDate(),'" & Session("Utente") & "'," & IIf(chkCorsoOlp.Checked = True, 1, 0) & "," & ddlEsperienzaServizioCivile.SelectedValue & "," & ddlCorso.SelectedValue & ",'" & Replace(TxtDettaglioRecapito.Text, "'", "''") & "'," & bandiera & " "
                    End If
                    strInsertCommand = strInsertCommand & ", " & idAllegato & ")"
                    Dim myCommand As New SqlClient.SqlCommand
                    Dim intIdEntePersonaleLocal As Integer
                    'sql command che mi esegue la insert
                    myCommand = New SqlClient.SqlCommand(strInsertCommand, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
                    myCommand.ExecuteNonQuery()
                    myCommand.Dispose()
                    lblErr.Text = vbNullString
                    'trovo l'id max dell'utente appena inserito per associarlo
                    'al tizio appena aggiunto
                    strInsertCommand = "select top 1 IDEntePersonale from entepersonale where CodiceFiscale ='" & Trim(txtCodiceFiscale.Text) & "' order by identepersonale desc"
                    'controllo e chiudo se aperto il datareader
                    If Not dtrIdMax Is Nothing Then
                        dtrIdMax.Close()
                        dtrIdMax = Nothing
                    End If
                    'eseguo la query
                    dtrIdMax = ClsServer.CreaDatareader(strInsertCommand, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
                    dtrIdMax.Read()
                    'ulteriore controllo se non trovo id
                    If dtrIdMax.HasRows = True Then
                        'variabile locale che prende l'id max
                        intIdEntePersonaleLocal = CInt(dtrIdMax.Item("IdEntePersonale"))
                        'controllo e chiudo se aperto il datareader
                        If Not dtrIdMax Is Nothing Then
                            dtrIdMax.Close()
                            dtrIdMax = Nothing
                        End If
                        '' ''vado a controlare e ad inserire il primo e principale ruolo
                        ' ''    If Request.Form("IdVal1") <> "" Then
                        ' ''        strInsertCommand = "insert into entepersonaleruoli (IDEntePersonale,"
                        ' ''        strInsertCommand = strInsertCommand & "IDRuolo,DataInizioValidità,Principale,Visibilità,DataInseritore,UserNameInseritore) values "
                        ' ''        strInsertCommand = strInsertCommand & "('" & intIdEntePersonaleLocal.ToString & "','"
                        ' ''        strInsertCommand = strInsertCommand & Request.Form("IdVal1") & "',getdate(),1," & CInt(Request.Form("ddlVisibilita1")) & ",GetDate(),'" & Session("Utente") & "')"
                        ' ''        'sql command momentaneo che mi esegue la insert
                        ' ''        myCommand = New SqlClient.SqlCommand(strInsertCommand, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
                        ' ''        myCommand.ExecuteNonQuery()
                        ' ''        myCommand.Dispose()
                        ' ''    End If
                        ' ''    'inserisco, se presente il ruolo secondario
                        ' ''    If Request.Form("IdVal2") <> "" Then
                        ' ''        strInsertCommand = "insert into entepersonaleruoli (IDEntePersonale,"
                        ' ''        strInsertCommand = strInsertCommand & "IDRuolo,DataInizioValidità,Principale,Visibilità,DataInseritore,UserNameInseritore) values "
                        ' ''        strInsertCommand = strInsertCommand & "('" & intIdEntePersonaleLocal.ToString & "','"
                        ' ''        strInsertCommand = strInsertCommand & Request.Form("IdVal2") & "',getdate(),0," & CInt(Request.Form("ddlVisibilita2")) & ",GetDate(),'" & Session("Utente") & "')"
                        ' ''        myCommand = New SqlClient.SqlCommand(strInsertCommand, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
                        ' ''        myCommand.ExecuteNonQuery()
                        ' ''        myCommand.Dispose()
                        ' ''    End If
                        ' ''    'inserisco, se presente il ruolo secondario
                        ' ''    If Request.Form("IdVal3") <> "" Then
                        ' ''        strInsertCommand = "insert into entepersonaleruoli (IDEntePersonale,"
                        ' ''        strInsertCommand = strInsertCommand & "IDRuolo,DataInizioValidità,Principale,Visibilità,DataInseritore,UserNameInseritore) values "
                        ' ''        strInsertCommand = strInsertCommand & "('" & intIdEntePersonaleLocal.ToString & "','"
                        ' ''        strInsertCommand = strInsertCommand & Request.Form("IdVal3") & "',getdate(),0," & CInt(Request.Form("ddlVisibilita3")) & ",GetDate(),'" & Session("Utente") & "')"
                        ' ''        myCommand = New SqlClient.SqlCommand(strInsertCommand, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
                        ' ''        myCommand.ExecuteNonQuery()
                        ' ''        myCommand.Dispose()
                        ' ''    End If
                        ' ''    'inserisco, se presente, il ruolo secondario
                        ' ''    If Request.Form("IdVal4") <> "" Then
                        ' ''        strInsertCommand = "insert into entepersonaleruoli (IDEntePersonale,"
                        ' ''        strInsertCommand = strInsertCommand & "IDRuolo,DataInizioValidità,Principale,Visibilità,DataInseritore,UserNameInseritore) values "
                        ' ''        strInsertCommand = strInsertCommand & "('" & intIdEntePersonaleLocal.ToString & "','"
                        ' ''        strInsertCommand = strInsertCommand & Request.Form("IdVal4") & "',getdate(),0," & CInt(Request.Form("ddlVisibilita4")) & ",GetDate(),'" & Session("Utente") & "')"
                        ' ''        myCommand = New SqlClient.SqlCommand(strInsertCommand, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
                        ' ''        myCommand.ExecuteNonQuery()
                        ' ''        myCommand.Dispose()
                        ' ''    End If


                        'Leucci 25/09/2018
                        'la stringa conterrà o il messaggio di errore o l'id del ruolo principale
                        'strValoreRitornoStore = ControlloIdRuoli()

                        ''controllo se si tratta di un id o di un messaggio d'errore
                        'If IsNumeric(strValoreRitornoStore) = False Then
                        '    lblErr.Visible = True
                        '    lblErr.Text = strValoreRitornoStore
                        '    Exit Sub
                        'End If

                        For Each item In dtgRuoli.Items
                            Dim check As CheckBox = DirectCast(item.FindControl("chkAssegnaRuolo"), CheckBox)

                            'controllo se la check è spuntata o meno
                            If check.Checked = True Then
                                strInsertCommand = "insert into entepersonaleruoli (IDEntePersonale,"
                                strInsertCommand = strInsertCommand & "IDRuolo,DataInizioValidità,Principale,Visibilità,DataInseritore,UserNameInseritore) values "
                                strInsertCommand = strInsertCommand & "('" & intIdEntePersonaleLocal.ToString & "','"
                                strInsertCommand = strInsertCommand & item.Cells(0).Text & "',getdate(),0,0,GetDate(),'" & Session("Utente") & "')"
                                myCommand = New SqlClient.SqlCommand(strInsertCommand, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
                                myCommand.ExecuteNonQuery()
                                myCommand.Dispose()
                            End If

                        Next
                    End If
                    'controllo e chiudo se aperto il datareader
                    If Not dtrIdMax Is Nothing Then
                        dtrIdMax.Close()
                        dtrIdMax = Nothing
                    End If
                    lblErr.Visible = True
                    lblErr.Text = "Inserimento Effettuato"
                    Dim entity = New Entity() With {
                     .Id = intIdEntePersonaleLocal,
                     .Name = "EntePersonale"
                    }
                    Log.Information(LogEvent.RISORSE_ENTE_INSERIMENTO, entity:=entity)
                    SvuotaCampi()
                    CaricaRuoliInserimento(AlboEnte)
                End If

                'nel caso in cui ci sia un errore stampo a inizio pagina il 
                'messaggio dell'errore generato in fase di esecuzione
            Catch ex As Exception
                Log.Error(LogEvent.RISORSE_ENTE_ERRORE, "Inserimento", ex)

                'Response.Write(ex.Message.ToString())
            End Try
            'pulisco i campi

            'mofifica
        ElseIf Session("ModIns") = 1 Then  'modifica
            'blocco try per l'update
            Try

                'carico la stringa della select di controllo esistenza codicefiscale
                strInsertCommand = "select CodiceFiscale from entepersonale "
                strInsertCommand = strInsertCommand & "where identepersonale<>'" & Request.QueryString("intPersonaleEnteAssociato") & "' and CodiceFiscale='" & Trim(txtCodiceFiscale.Text) & "'"
                'controllo e chiudo se aperto il datareader
                If Not dtrCheckCodFis Is Nothing Then
                    dtrCheckCodFis.Close()
                    dtrCheckCodFis = Nothing
                End If
                'eseguo la query
                dtrCheckCodFis = ClsServer.CreaDatareader(strInsertCommand, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
                'leggo il datareader
                dtrCheckCodFis.Read()
                'se ci sono dei record
                If dtrCheckCodFis.HasRows = True Then
                    'If Request.Form("chkCodiceFiscale") <> "2" Then
                    'carico la stringa della select di controllo esistenza codicefiscale
                    strInsertCommand = "select CodiceFiscale from entepersonale "
                    strInsertCommand = strInsertCommand & "where identepersonale<>'" & Request.QueryString("intPersonaleEnteAssociato") & "' and idente='" & Session("IdEnte") & "' and CodiceFiscale='" & Trim(txtCodiceFiscale.Text) & "'"
                    'controllo e chiudo se aperto il datareader
                    If Not dtrCheckCodFis Is Nothing Then
                        dtrCheckCodFis.Close()
                        dtrCheckCodFis = Nothing
                    End If
                    'eseguo la query
                    dtrCheckCodFis = ClsServer.CreaDatareader(strInsertCommand, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
                    'leggo il datareader
                    dtrCheckCodFis.Read()
                    'se ci sono dei record
                    If dtrCheckCodFis.HasRows = True Then
                        If Not dtrCheckCodFis Is Nothing Then
                            dtrCheckCodFis.Close()
                            dtrCheckCodFis = Nothing
                        End If

                        strInsertCommand = "select * from entepersonaleruoli as a "
                        strInsertCommand = strInsertCommand & "inner join entepersonale as b on a.identepersonale=b.identepersonale "
                        'If Session("TipoUtente") = "E" Then
                        strInsertCommand += " inner join ruoli c on a.idruolo=c.idruolo "
                        'End If
                        strInsertCommand = strInsertCommand & " where c.Nascosto = 0 and  b.codicefiscale='" & Trim(txtCodiceFiscale.Text) & "' and b.idente='" & Session("IdEnte") & "'"
                        'gianluigi 06/08/2007  se ente e ruoloaccreditamento=1 
                        If Session("TipoUtente") = "E" Then
                            strInsertCommand += " and RuoloAccreditamento = 1"
                        End If
                        'controllo e chiudo se aperto il datareader
                        If Not dtrCheckCodFis Is Nothing Then
                            dtrCheckCodFis.Close()
                            dtrCheckCodFis = Nothing
                        End If
                        'eseguo la query
                        dtrCheckCodFis = ClsServer.CreaDatareader(strInsertCommand, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
                        'leggo il datareader
                        dtrCheckCodFis.Read()
                        'se ci sono dei record
                        If dtrCheckCodFis.HasRows = True Then
                            'coloro di grigio la txt del codice fiscale
                            txtCodiceFiscale.BackColor = Color.FromArgb(242, 185, 63)
                            'avverto l'utente dell'esistenza del codice fiscale che si vuole inseirire
                            lblErr.Text = "La Risorsa e' stata salvata. Attenzione, il Codice Fiscale risulta essere gia' in uso."
                            Log.Warning(LogEvent.RISORSE_ENTE_MODIFICA, "Codice Fiscale già in uso")
                            blnCheckCodFis = True
                            'controllo e chiudo se aperto il datareader
                            If Not dtrCheckCodFis Is Nothing Then
                                dtrCheckCodFis.Close()
                                dtrCheckCodFis = Nothing
                            End If
                        Else
                            lblErr.Text = "La Risorsa non e' stata salvata. Il Codice Fiscale risulta essere gia' in uso per questo ente."
                            Log.Warning(LogEvent.RISORSE_ENTE_ERRORE_MODIFICA, "Codice Fiscale già in uso per l'ente")
                            blnCheckCodFis = False
                            If Not dtrCheckCodFis Is Nothing Then
                                dtrCheckCodFis.Close()
                                dtrCheckCodFis = Nothing
                            End If
                        End If
                        If Not dtrCheckCodFis Is Nothing Then
                            dtrCheckCodFis.Close()
                            dtrCheckCodFis = Nothing
                        End If
                    Else
                        'coloro di grigio la txt del codice fiscale
                        txtCodiceFiscale.BackColor = Color.FromArgb(242, 185, 63)
                        'avverto l'utente dell'esistenza del codice fiscale che si vuole inseirire
                        lblErr.Text = "La Risorsa e' stata salvata. Attenzione, il Codice Fiscale risulta essere gia' in uso."
                        Log.Warning(LogEvent.RISORSE_ENTE_MODIFICA, "Codice Fiscale già in uso")
                        blnCheckCodFis = True
                        'controllo e chiudo se aperto il datareader
                        If Not dtrCheckCodFis Is Nothing Then
                            dtrCheckCodFis.Close()
                            dtrCheckCodFis = Nothing
                        End If
                    End If
                    'Response.Write("<input type=hidden name=PassaCodiceFiscale value='1'>")
                    'Else
                    '    Response.Write("<input type=hidden name=PassaCodiceFiscale value='0'>")
                Else
                    If Not dtrCheckCodFis Is Nothing Then
                        dtrCheckCodFis.Close()
                        dtrCheckCodFis = Nothing
                    End If

                    '***Gestione CV***
                    Dim idPersonaleAttuale = CInt(Request.QueryString("intPersonaleEnteAssociato"))
                    Dim CVAttuale As Allegato = Session("LoadedCV")
                    Dim idAllegatoAttuale As Integer = 0
                    'Controllo se è stato caricato un nuovo CV (l'id dell'allegato in sessione è vuoto)
                    If Session("LoadCVId") Is Nothing Then
                        CVAttuale.Id = InsertCV(CVAttuale, strIdEntefase)
                        idAllegatoAttuale = CVAttuale.Id
                        'Se è presente il CV ed è stato cambiato lo aggiorno
                    ElseIf CVAttuale IsNot Nothing AndAlso CVAttuale.Updated Then
                        If CercaAllegato(CV, strIdEntefase) Then
                            AggiornaAllegato(CV, strIdEntefase)
                        Else
                            Dim insertAllegatoCommand As New SqlCommand("", Session("conn"))
                            idAllegatoAttuale = InsertCV(CV, strIdEntefase)
                            Session("LoadCVId") = idAllegatoAttuale
                            insertAllegatoCommand.Dispose()
                        End If
                    End If
                    '*** Fine Gestione CV***

                    'caricamento della stringa contenente l'istruzione per l'operazione di update nella tabella entepersonale
                    strInsertCommand = "update entepersonale set IDEnte=" & CInt(Session("idEnte")) & ","
                    strInsertCommand = strInsertCommand & "Cognome='" & Replace(txtCognome.Text, "'", "''") & "',"
                    strInsertCommand = strInsertCommand & "Nome='" & Replace(txtNome.Text, "'", "''") & "',"
                    strInsertCommand = strInsertCommand & "Titolo='" & Replace(txtTitolo.Text, "'", "''") & "',"
                    strInsertCommand = strInsertCommand & "Posizione='" & Replace(txtPosizione.Text, "'", "''") & "',"
                    strInsertCommand = strInsertCommand & "Cellulare='" & txtCellulare.Text & "',"
                    strInsertCommand = strInsertCommand & "Email='" & txtEmail.Text & "',"
                    strInsertCommand = strInsertCommand & "Telefono='" & txtTelefono.Text & "',"
                    strInsertCommand = strInsertCommand & "Fax='" & txtFax.Text & "',"
                    If ddlComuneNascita.SelectedIndex > 0 Then
                        strInsertCommand = strInsertCommand & "IDComuneNascita=" & CInt(ddlComuneNascita.SelectedValue) & ","
                    Else
                        strInsertCommand = strInsertCommand & "IDComuneNascita=null,"
                    End If
                    strInsertCommand = strInsertCommand & "DataNascita='" & Day(txtDataNascita.Text) & "-" & Month(txtDataNascita.Text) & "-" & Year(txtDataNascita.Text) & "',"
                    If ddlComuneResidenza.SelectedIndex > 0 Then
                        strInsertCommand = strInsertCommand & "IDComuneResidenza=" & CInt(ddlComuneResidenza.SelectedValue) & ","
                    Else
                        strInsertCommand = strInsertCommand & "IDComuneResidenza=null,"
                    End If
                    strInsertCommand = strInsertCommand & "Indirizzo='" & Replace(txtIndirizzo.Text, "'", "''") & "',"
                    strInsertCommand = strInsertCommand & "DettaglioRecapito='" & Replace(TxtDettaglioRecapito.Text, "'", "''") & "',"
                    strInsertCommand = strInsertCommand & "FlagIndirizzoValido=" & bandiera & ","
                    strInsertCommand = strInsertCommand & "Civico='" & Replace(txtCivico.Text, "'", "''") & "',"
                    strInsertCommand = strInsertCommand & "Cap='" & txtCAP.Text & "',"
                    strInsertCommand = strInsertCommand & "CodiceFiscale='" & Replace(txtCodiceFiscale.Text, "'", "''") & "',"
                    strInsertCommand = strInsertCommand & "CorsoOLP=" & IIf(chkCorsoOlp.Checked = True, 1, 0) & ","
                    strInsertCommand = strInsertCommand & "EsperienzaServizioCivile=" & ddlEsperienzaServizioCivile.SelectedValue & ","
                    strInsertCommand = strInsertCommand & "Corso=" & ddlCorso.SelectedValue
                    If idAllegatoAttuale > 0 Then
                        strInsertCommand = strInsertCommand & ",IdAllegatoCV = " & idAllegatoAttuale & " "
                    End If
                    strInsertCommand = strInsertCommand & " where IDEntePersonale=" & CInt(Request.QueryString("intPersonaleEnteAssociato"))

                    'sql command momentaneo che mi esegue l'update
                    Dim myCommand As New SqlClient.SqlCommand(strInsertCommand, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
                    myCommand.ExecuteNonQuery()
                    lblErr.Text = vbNullString
                    lblErr.CssClass = "msgInfo"
                    lblErr.Visible = True
                    lblErr.Text = "Modifica effettuata con successo"
                    Dim entity = New Entity() With {
                     .Id = idPersonaleAttuale,
                     .Name = "EntePersonale"
                    }
                    Log.Information(LogEvent.RISORSE_ENTE_MODIFICA, entity:=entity)
                    'la stringa conterrà o il messaggio di errore o l'id del ruolo principale
                    strValoreRitornoStore = ControlloIdRuoli(Request.QueryString("intPersonaleEnteAssociato"))

                    'controllo se si tratta di un id o di un messaggio d'errore
                    If IsNumeric(strValoreRitornoStore) = False Then
                        lblErr.Visible = True
                        lblErr.Text = strValoreRitornoStore
                        Log.Warning(LogEvent.HELIOS_ERRORE_AUTORIZZAZIONE)

                        Exit Sub
                    End If

                    'mi preparo per i ruoli
                    ' ''For i = 1 To 4
                    ' ''    'controllo se ci sono ruoli da aggiornare
                    ' ''    If Request.Form("Val" & i.ToString) <> "" Then
                    ' ''        'preparo la stringa dell'update
                    ' ''        strInsertCommand = "update entepersonaleruoli set Visibilità=" & CInt(Request.Form("ddlVisibilita" & i.ToString))
                    ' ''        'controllo quale variabile passare nell'update
                    ' ''        If Request.Form("IdEntePersonale") <> "" Then
                    ' ''            strInsertCommand = strInsertCommand & " where IDEntePersonale=" & CInt(Request.Form("IdEntePersonale"))
                    ' ''        Else
                    ' ''            strInsertCommand = strInsertCommand & " where IDEntePersonale=" & CInt(Request.QueryString("intPersonaleEnteAssociato"))
                    ' ''        End If
                    ' ''        'controllo quali idRuolo aggiornare
                    ' ''        If Request.Form("IdVal" & i.ToString) <> "" Then
                    ' ''            strInsertCommand = strInsertCommand & " and IDRuolo=" & Request.Form("IdVal" & i.ToString)
                    ' ''        Else
                    ' ''            strInsertCommand = strInsertCommand & " and IDRuolo=" & CInt(dtgRuoloPrincipale.Items(i - 1).Cells(0).Text)
                    ' ''        End If
                    ' ''        'passo i parametri al sqlcommand e eseguo la query
                    ' ''        myCommand.Connection = IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn"))
                    ' ''        myCommand.CommandText = strInsertCommand
                    ' ''        myCommand.ExecuteNonQuery()
                    ' ''    End If
                    ' ''Next

                    'ciclo gli item della datagrid per vedere lo stato della check "assegna ruolo"
                    For Each item In dtgRuoli.Items
                        Dim check As CheckBox = DirectCast(item.FindControl("chkAssegnaRuolo"), CheckBox)
                        If CLng(item.Cells(2).Text) <> 0 Then
                            'controllo se la check è spuntata o meno
                            If check.Checked = False Then 'faccio l'update
                                'controllo se il ruolo era stato cancellato
                                'faccio una select case sullo stato del ruolo
                                'Select Case item.Cells(4).Text
                                'Case "Accreditato"
                                strsql = "update entepersonaleruoli set DataFineValidità=GetDate(), UserNameCancellazione='" & Session("Utente") & "' "
                                strsql = strsql & "where identepersonaleruolo='" & item.Cells(2).Text & "'"
                                cmdGenerico = New SqlClient.SqlCommand(strsql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
                                cmdGenerico.ExecuteNonQuery()

                                'preparo la insert nella cronologiaenterpersonaleruoli
                                strsql = "insert into cronologiaentepersonaleruoli (IdEntePersonaleRuolo, Accreditato, DataCronologia, IdTipoCronologia, UserNameAccreditatore, Forzatura, UserNameInseritore, datainseritore) "
                                strsql = strsql & "select identepersonaleruolo, accreditato, getdate(), '1', usernameaccreditatore, '0', usernameinseritore, datainseritore from entepersonaleruoli where identepersonaleruolo='" & item.Cells(2).Text & "'"


                                'sql command momentaneo che mi esegue la insert
                                cmdGenerico = New SqlClient.SqlCommand(strsql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
                                cmdGenerico.ExecuteNonQuery()

                                'danilo accreditamento 2014 imposto flag richiesta cancellazione se ente e ruolo accreditato
                                If item.Cells(4).Text = "Iscritto" And Session("TipoUtente") = "E" Then
                                    strsql = "update entepersonaleruoli set richiestacancellazione = 1, UserNameRichiestaCancellazione='" & Session("Utente") & "', datarichiestacancellazione = getdate() "
                                    strsql = strsql & "where identepersonaleruolo='" & item.Cells(2).Text & "'"
                                    cmdGenerico = New SqlClient.SqlCommand(strsql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
                                    cmdGenerico.ExecuteNonQuery()

                                End If

                                '    Case "Da Accreditare"
                                '        strsql = "update entepersonaleruoli set DataFineValidità=GetDate(), UserNameCancellazione='" & Session("Utente") & "' "
                                '        strsql = strsql & "where identepersonaleruolo='" & item.Cells(2).Text & "'"
                                '    Case "Non Accreditato"
                                '        strsql = "update entepersonaleruoli set DataFineValidità=GetDate(), UserNameCancellazione='" & Session("Utente") & "' "
                                '        strsql = strsql & "where identepersonaleruolo='" & item.Cells(2).Text & "'"
                                'End Select
                            Else 'faccio l'update
                                If item.Cells(6).Text <> "&nbsp;" Then
                                    'ripristino lo stato del ruolo a "da accreditre" e setto a null la data di fine validità
                                    strsql = "update entepersonaleruoli set DataFineValidità=null, UserNameCancellazione=null, Accreditato=0 "
                                    strsql = strsql & "where identepersonaleruolo='" & item.Cells(2).Text & "'"
                                    cmdGenerico = New SqlClient.SqlCommand(strsql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
                                    cmdGenerico.ExecuteNonQuery()

                                    'danilo accreditamento 2014 aggiorno ad "accreditato" se flag richiesta cancellazione
                                    strsql = "update entepersonaleruoli set DataFineValidità=null, UserNameCancellazione=null, Accreditato=1, richiestacancellazione = null, UserNameRichiestaCancellazione=null, datarichiestacancellazione = null "
                                    strsql = strsql & "where identepersonaleruolo='" & item.Cells(2).Text & "' and richiestacancellazione = 1"
                                    cmdGenerico = New SqlClient.SqlCommand(strsql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
                                    cmdGenerico.ExecuteNonQuery()

                                    'preparo la insert nella cronologiaenterpersonaleruoli
                                    strsql = "insert into cronologiaentepersonaleruoli (IdEntePersonaleRuolo, Accreditato, DataCronologia, IdTipoCronologia, UserNameAccreditatore, Forzatura, UserNameInseritore, datainseritore) "
                                    strsql = strsql & "select identepersonaleruolo, accreditato, getdate(), '1', usernameaccreditatore, '0', usernameinseritore, datainseritore from entepersonaleruoli where identepersonaleruolo='" & item.Cells(2).Text & "'"

                                    'sql command momentaneo che mi esegue la insert
                                    cmdGenerico = New SqlClient.SqlCommand(strsql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
                                    cmdGenerico.ExecuteNonQuery()
                                    '    Case "Da Accreditare"
                                    '        strsql = "update entepersonaleruoli set DataFineValidità=GetDate(), UserNameCancellazione='" & Session("Utente") & "' "
                                    '        strsql = strsql & "where identepersonaleruolo='" & item.Cells(2).Text & "'"
                                    '    Case "Non Accreditato"
                                    '        strsql = "update entepersonaleruoli set DataFineValidità=GetDate(), UserNameCancellazione='" & Session("Utente") & "' "
                                    '        strsql = strsql & "where identepersonaleruolo='" & item.Cells(2).Text & "'"
                                    'End Select
                                End If
                                'strsql = "update entepersonaleruoli set DataFineValidità=GetDate(), UserNameCancellazione='" & Session("Utente") & "' "
                                'strsql = strsql & "where identepersonaleruolo='" & item.Cells(2).Text & "'"
                                ''strsql = "update entepersonaleruoli set (IDEntePersonale,"
                                ''strsql = strsql & "IDRuolo,DataInizioValidità,Principale,Visibilità,DataInseritore,UserNameInseritore) values "
                                ''strsql = strsql & "('" & intIdEntePersonaleLocal.ToString & "','"
                                ''strsql = strsql & Request.Form("IdVal1") & "',getdate(),1," & CInt(Request.Form("ddlVisibilita1")) & ",GetDate(),'" & Session("Utente") & "')"
                                ''sql command momentaneo che mi esegue la insert
                                'cmdGenerico = New SqlClient.SqlCommand(strsql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
                                'cmdGenerico.ExecuteNonQuery()
                            End If


                            'cmdGenerico.Dispose()
                        Else 'se il ruolo non era stato assegnato controllo se è stata spuntata la check dell'assegnazione
                            'e vado a fare l'inser in entepersonaleruoli
                            If check.Checked = True Then 'insert in entepersonaleruoli e cronologia
                                If item.Cells(6).Text <> "&nbsp;" Then
                                    'ripristino lo stato del ruolo a "da accreditre" e setto a null la data di fine validità
                                    strsql = "update entepersonaleruoli set DataFineValidità=null, UserNameCancellazione=null, Accreditato=0 "
                                    strsql = strsql & "where identepersonaleruolo='" & item.Cells(2).Text & "'"
                                    cmdGenerico = New SqlClient.SqlCommand(strsql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
                                    cmdGenerico.ExecuteNonQuery()

                                    'danilo accreditamento 2014 aggiorno ad "accreditato" se flag richiesta cancellazione
                                    strsql = "update entepersonaleruoli set DataFineValidità=null, UserNameCancellazione=null, Accreditato=1, richiestacancellazione = null, UserNameRichiestaCancellazione=null, datarichiestacancellazione = null "
                                    strsql = strsql & "where identepersonaleruolo='" & item.Cells(2).Text & "' and richiestacancellazione = 1"
                                    cmdGenerico = New SqlClient.SqlCommand(strsql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
                                    cmdGenerico.ExecuteNonQuery()

                                    'preparo la insert nella cronologiaenterpersonaleruoli
                                    strsql = "insert into cronologiaentepersonaleruoli (IdEntePersonaleRuolo, Accreditato, DataCronologia, IdTipoCronologia, UserNameAccreditatore, Forzatura, UserNameInseritore, datainseritore) "
                                    strsql = strsql & "select identepersonaleruolo, accreditato, getdate(), '1', usernameaccreditatore, '0', usernameinseritore, datainseritore from entepersonaleruoli where identepersonaleruolo='" & item.Cells(2).Text & "'"

                                    'sql command momentaneo che mi esegue la insert
                                    cmdGenerico = New SqlClient.SqlCommand(strsql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
                                    cmdGenerico.ExecuteNonQuery()
                                    '    Case "Da Accreditare"
                                    '        strsql = "update entepersonaleruoli set DataFineValidità=GetDate(), UserNameCancellazione='" & Session("Utente") & "' "
                                    '        strsql = strsql & "where identepersonaleruolo='" & item.Cells(2).Text & "'"
                                    '    Case "Non Accreditato"
                                    '        strsql = "update entepersonaleruoli set DataFineValidità=GetDate(), UserNameCancellazione='" & Session("Utente") & "' "
                                    '        strsql = strsql & "where identepersonaleruolo='" & item.Cells(2).Text & "'"
                                    'End Select
                                Else
                                    'preparo la insert nella cronologiaenterpersonaleruoli
                                    strsql = "insert into cronologiaentepersonaleruoli (IdEntePersonaleRuolo, Accreditato, DataCronologia, IdTipoCronologia, UserNameAccreditatore, Forzatura, UserNameInseritore, datainseritore) "
                                    strsql = strsql & "select identepersonaleruolo, accreditato, getdate(), '1', usernameaccreditatore, '0', usernameinseritore, datainseritore from entepersonaleruoli where identepersonaleruolo='" & item.Cells(2).Text & "'"

                                    'sql command momentaneo che mi esegue la insert
                                    cmdGenerico = New SqlClient.SqlCommand(strsql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
                                    cmdGenerico.ExecuteNonQuery()
                                    'cmdGenerico.Dispose()

                                    'controllo se si tratta di un ruolo principale
                                    If strValoreRitornoStore = item.Cells(0).Text Then
                                        strsql = "insert into entepersonaleruoli (IDEntePersonale,"
                                        strsql = strsql & "IDRuolo,DataInizioValidità,Principale,DataInseritore,UserNameInseritore) values "
                                        strsql = strsql & "('" & idEntePersonale.Value & "','" & item.Cells(0).Text & "',GetDate(),1,GetDate(),'" & Session("Utente") & "')"
                                    Else
                                        strsql = "insert into entepersonaleruoli (IDEntePersonale,"
                                        strsql = strsql & "IDRuolo,DataInizioValidità,Principale,DataInseritore,UserNameInseritore) values "
                                        strsql = strsql & "('" & idEntePersonale.Value & "','" & item.Cells(0).Text & "',GetDate(),0,GetDate(),'" & Session("Utente") & "')"
                                    End If

                                    cmdGenerico = New SqlClient.SqlCommand(strsql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
                                    cmdGenerico.ExecuteNonQuery()
                                End If
                            End If
                        End If
                    Next
                    'carico l'anagrafica
                    CaricaAnagrafica()
                    CaricaRuoliModifica(idEntePersonale.Value, AlboEnte)
                End If
                'controllo e chiudo se aperto il datareader
                If Not dtrCheckCodFis Is Nothing Then
                    dtrCheckCodFis.Close()
                    dtrCheckCodFis = Nothing
                End If
                'End If

                '***Gestione CV***
                Dim idPersonale = CInt(Request.QueryString("intPersonaleEnteAssociato"))
                Dim idAllegato As Integer = 0
                Dim _cE As New clsEnte

                'Controllo se è stato caricato un nuovo CV (l'id dell'allegato in sessione è vuoto)
                If Session("LoadCVId") Is Nothing Then
                    Dim insertAllegatoCommand As New SqlCommand("", Session("conn"))
                    idAllegato = InsertCV(CV, strIdEntefase)
                    Session("LoadCVId") = idAllegato
                    insertAllegatoCommand.Dispose()
                    'Se è presente il CV ed è stato cambiato lo aggiorno
                ElseIf CV IsNot Nothing AndAlso CV.Updated Then
                    If CercaAllegato(CV, strIdEntefase) Then
                        AggiornaAllegato(CV, strIdEntefase)
                    Else
                        Dim insertAllegatoCommand As New SqlCommand("", Session("conn"))
                        idAllegato = InsertCV(CV, strIdEntefase)
                        Session("LoadCVId") = idAllegato
                        insertAllegatoCommand.Dispose()
                    End If
                End If
                '*** Fine Gestione CV***
                If blnCheckCodFis = True Then
                    'caricamento della stringa contenente l'istruzione per l'operazione di update nella tabella entepersonale
                    strInsertCommand = "update entepersonale set IDEnte=" & CInt(Session("idEnte")) & ","
                    strInsertCommand = strInsertCommand & "Cognome='" & Replace(txtCognome.Text, "'", "''") & "',"
                    strInsertCommand = strInsertCommand & "Nome='" & Replace(txtNome.Text, "'", "''") & "',"
                    strInsertCommand = strInsertCommand & "Titolo='" & Replace(txtTitolo.Text, "'", "''") & "',"
                    strInsertCommand = strInsertCommand & "Posizione='" & Replace(txtPosizione.Text, "'", "''") & "',"
                    strInsertCommand = strInsertCommand & "Cellulare='" & txtCellulare.Text & "',"
                    strInsertCommand = strInsertCommand & "Email='" & txtEmail.Text & "',"
                    strInsertCommand = strInsertCommand & "Telefono='" & txtTelefono.Text & "',"
                    strInsertCommand = strInsertCommand & "Fax='" & txtFax.Text & "',"
                    If ddlComuneNascita.SelectedIndex > 0 Then
                        strInsertCommand = strInsertCommand & "IDComuneNascita=" & CInt(ddlComuneNascita.SelectedValue) & ","
                    Else
                        strInsertCommand = strInsertCommand & "IDComuneNascita=null,"
                    End If
                    strInsertCommand = strInsertCommand & "DataNascita='" & Day(txtDataNascita.Text) & "-" & Month(txtDataNascita.Text) & "-" & Year(txtDataNascita.Text) & "',"
                    If ddlComuneResidenza.SelectedIndex > 0 Then
                        strInsertCommand = strInsertCommand & "IDComuneResidenza=" & CInt(ddlComuneResidenza.SelectedValue) & ","
                    Else
                        strInsertCommand = strInsertCommand & "IDComuneResidenza=null,"
                    End If
                    strInsertCommand = strInsertCommand & "Indirizzo='" & Replace(txtIndirizzo.Text, "'", "''") & "',"
                    strInsertCommand = strInsertCommand & "DettaglioRecapito='" & Replace(TxtDettaglioRecapito.Text, "'", "''") & "',"
                    strInsertCommand = strInsertCommand & "FlagIndirizzoValido=" & bandiera & ","
                    strInsertCommand = strInsertCommand & "Civico='" & Replace(txtCivico.Text, "'", "''") & "',"
                    strInsertCommand = strInsertCommand & "Cap='" & txtCAP.Text & "',"
                    strInsertCommand = strInsertCommand & "CodiceFiscale='" & Replace(txtCodiceFiscale.Text, "'", "''") & "',"
                    strInsertCommand = strInsertCommand & "CorsoOLP=" & IIf(chkCorsoOlp.Checked = True, 1, 0) & ","
                    strInsertCommand = strInsertCommand & "EsperienzaServizioCivile=" & ddlEsperienzaServizioCivile.SelectedValue & ","
                    strInsertCommand = strInsertCommand & "Corso=" & ddlCorso.SelectedValue
                    If idAllegato > 0 Then
                        strInsertCommand = strInsertCommand & ",IdAllegatoCV = " & idAllegato
                    End If
                    strInsertCommand = strInsertCommand & " where IDEntePersonale=" & CInt(Request.QueryString("intPersonaleEnteAssociato"))
                    'sql command momentaneo che mi esegue l'update
                    Dim myCommand As New SqlClient.SqlCommand(strInsertCommand, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
                    myCommand.ExecuteNonQuery()
                    lblErr.Text = vbNullString


                    'la stringa conterrà o il messaggio di errore o l'id del ruolo principale
                    strValoreRitornoStore = ControlloIdRuoli(Request.QueryString("intPersonaleEnteAssociato"))

                    'controllo se si tratta di un id o di un messaggio d'errore
                    If IsNumeric(strValoreRitornoStore) = False Then
                        lblErr.Visible = True
                        lblErr.Text = strValoreRitornoStore
                        Log.Warning(LogEvent.HELIOS_ERRORE_AUTORIZZAZIONE)
                        Exit Sub
                    End If

                    'mi preparo per i ruoli
                    '' ''For i = 1 To 4
                    '' ''    'controllo se ci sono ruoli da aggiornare
                    '' ''    If Request.Form("Val" & i.ToString) <> "" Then
                    '' ''        'preparo la stringa dell'update
                    '' ''        strInsertCommand = "update entepersonaleruoli set Visibilità=" & CInt(Request.Form("ddlVisibilita" & i.ToString))
                    '' ''        'controllo quale variabile passare nell'update
                    '' ''        If Request.Form("IdEntePersonale") <> "" Then
                    '' ''            strInsertCommand = strInsertCommand & " where IDEntePersonale=" & CInt(Request.Form("IdEntePersonale"))
                    '' ''        Else
                    '' ''            strInsertCommand = strInsertCommand & " where IDEntePersonale=" & CInt(Request.QueryString("intPersonaleEnteAssociato"))
                    '' ''        End If
                    '' ''        'controllo quali idRuolo aggiornare
                    '' ''        If Request.Form("IdVal" & i.ToString) <> "" Then
                    '' ''            strInsertCommand = strInsertCommand & " and IDRuolo=" & Request.Form("IdVal" & i.ToString)
                    '' ''        Else
                    '' ''            strInsertCommand = strInsertCommand & " and IDRuolo=" & CInt(dtgRuoloPrincipale.Items(i - 1).Cells(0).Text)
                    '' ''        End If
                    '' ''        'passo i parametri al sqlcommand e eseguo la query
                    '' ''        myCommand.Connection = IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn"))
                    '' ''        myCommand.CommandText = strInsertCommand
                    '' ''        myCommand.ExecuteNonQuery()
                    '' ''    End If
                    '' ''Next

                    'ciclo gli item della datagrid per vedere lo stato della check "assegna ruolo"
                    For Each item In dtgRuoli.Items
                        Dim check As CheckBox = DirectCast(item.FindControl("chkAssegnaRuolo"), CheckBox)
                        If CLng(item.Cells(2).Text) <> 0 Then
                            'controllo se la check è spuntata o meno
                            If check.Checked = False Then 'faccio l'update
                                'controllo se il ruolo era stato cancellato
                                'faccio una select case sullo stato del ruolo
                                'Select Case item.Cells(4).Text
                                'Case "Accreditato"
                                strsql = "update entepersonaleruoli set DataFineValidità=GetDate(), UserNameCancellazione='" & Session("Utente") & "' "
                                strsql = strsql & "where identepersonaleruolo='" & item.Cells(2).Text & "'"
                                cmdGenerico = New SqlClient.SqlCommand(strsql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
                                cmdGenerico.ExecuteNonQuery()

                                'preparo la insert nella cronologiaenterpersonaleruoli
                                strsql = "insert into cronologiaentepersonaleruoli (IdEntePersonaleRuolo, Accreditato, DataCronologia, IdTipoCronologia, UserNameAccreditatore, Forzatura, UserNameInseritore, datainseritore) "
                                strsql = strsql & "select identepersonaleruolo, accreditato, getdate(), '1', usernameaccreditatore, '0', usernameinseritore, datainseritore from entepersonaleruoli where identepersonaleruolo='" & item.Cells(2).Text & "'"


                                'sql command momentaneo che mi esegue la insert
                                cmdGenerico = New SqlClient.SqlCommand(strsql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
                                cmdGenerico.ExecuteNonQuery()

                                'danilo accreditamento 2014 imposto flag richiesta cancellazione se ente e ruolo accreditato
                                If item.Cells(4).Text = "Iscritto" And Session("TipoUtente") = "E" Then
                                    strsql = "update entepersonaleruoli set richiestacancellazione = 1, UserNameRichiestaCancellazione='" & Session("Utente") & "', datarichiestacancellazione = getdate() "
                                    strsql = strsql & "where identepersonaleruolo='" & item.Cells(2).Text & "'"
                                    cmdGenerico = New SqlClient.SqlCommand(strsql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
                                    cmdGenerico.ExecuteNonQuery()

                                End If

                                '    Case "Da Accreditare"
                                '        strsql = "update entepersonaleruoli set DataFineValidità=GetDate(), UserNameCancellazione='" & Session("Utente") & "' "
                                '        strsql = strsql & "where identepersonaleruolo='" & item.Cells(2).Text & "'"
                                '    Case "Non Accreditato"
                                '        strsql = "update entepersonaleruoli set DataFineValidità=GetDate(), UserNameCancellazione='" & Session("Utente") & "' "
                                '        strsql = strsql & "where identepersonaleruolo='" & item.Cells(2).Text & "'"
                                'End Select
                            Else 'faccio l'update
                                If item.Cells(6).Text <> "&nbsp;" Then
                                    'ripristino lo stato del ruolo a "da accreditre" e setto a null la data di fine validità
                                    strsql = "update entepersonaleruoli set DataFineValidità=null, UserNameCancellazione=null, Accreditato=0 "
                                    strsql = strsql & "where identepersonaleruolo='" & item.Cells(2).Text & "'"
                                    cmdGenerico = New SqlClient.SqlCommand(strsql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
                                    cmdGenerico.ExecuteNonQuery()

                                    'danilo accreditamento 2014 aggiorno ad "accreditato" se flag richiesta cancellazione
                                    strsql = "update entepersonaleruoli set DataFineValidità=null, UserNameCancellazione=null, Accreditato=1, richiestacancellazione = null, UserNameRichiestaCancellazione=null, datarichiestacancellazione = null "
                                    strsql = strsql & "where identepersonaleruolo='" & item.Cells(2).Text & "' and richiestacancellazione = 1"
                                    cmdGenerico = New SqlClient.SqlCommand(strsql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
                                    cmdGenerico.ExecuteNonQuery()

                                    'preparo la insert nella cronologiaenterpersonaleruoli
                                    strsql = "insert into cronologiaentepersonaleruoli (IdEntePersonaleRuolo, Accreditato, DataCronologia, IdTipoCronologia, UserNameAccreditatore, Forzatura, UserNameInseritore, datainseritore) "
                                    strsql = strsql & "select identepersonaleruolo, accreditato, getdate(), '1', usernameaccreditatore, '0', usernameinseritore, datainseritore from entepersonaleruoli where identepersonaleruolo='" & item.Cells(2).Text & "'"

                                    'sql command momentaneo che mi esegue la insert
                                    cmdGenerico = New SqlClient.SqlCommand(strsql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
                                    cmdGenerico.ExecuteNonQuery()
                                    '    Case "Da Accreditare"
                                    '        strsql = "update entepersonaleruoli set DataFineValidità=GetDate(), UserNameCancellazione='" & Session("Utente") & "' "
                                    '        strsql = strsql & "where identepersonaleruolo='" & item.Cells(2).Text & "'"
                                    '    Case "Non Accreditato"
                                    '        strsql = "update entepersonaleruoli set DataFineValidità=GetDate(), UserNameCancellazione='" & Session("Utente") & "' "
                                    '        strsql = strsql & "where identepersonaleruolo='" & item.Cells(2).Text & "'"
                                    'End Select
                                End If
                                'strsql = "update entepersonaleruoli set DataFineValidità=GetDate(), UserNameCancellazione='" & Session("Utente") & "' "
                                'strsql = strsql & "where identepersonaleruolo='" & item.Cells(2).Text & "'"
                                ''strsql = "update entepersonaleruoli set (IDEntePersonale,"
                                ''strsql = strsql & "IDRuolo,DataInizioValidità,Principale,Visibilità,DataInseritore,UserNameInseritore) values "
                                ''strsql = strsql & "('" & intIdEntePersonaleLocal.ToString & "','"
                                ''strsql = strsql & Request.Form("IdVal1") & "',getdate(),1," & CInt(Request.Form("ddlVisibilita1")) & ",GetDate(),'" & Session("Utente") & "')"
                                ''sql command momentaneo che mi esegue la insert
                                'cmdGenerico = New SqlClient.SqlCommand(strsql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
                                'cmdGenerico.ExecuteNonQuery()
                            End If


                            'cmdGenerico.Dispose()
                        Else 'se il ruolo non era stato assegnato controllo se è stata spuntata la check dell'assegnazione
                            'e vado a fare l'inser in entepersonaleruoli
                            If check.Checked = True Then 'insert in entepersonaleruoli e cronologia
                                If item.Cells(6).Text <> "&nbsp;" Then
                                    'ripristino lo stato del ruolo a "da accreditre" e setto a null la data di fine validità
                                    strsql = "update entepersonaleruoli set DataFineValidità=null, UserNameCancellazione=null, Accreditato=0 "
                                    strsql = strsql & "where identepersonaleruolo='" & item.Cells(2).Text & "'"
                                    cmdGenerico = New SqlClient.SqlCommand(strsql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
                                    cmdGenerico.ExecuteNonQuery()

                                    'danilo accreditamento 2014 aggiorno ad "accreditato" se flag richiesta cancellazione
                                    strsql = "update entepersonaleruoli set DataFineValidità=null, UserNameCancellazione=null, Accreditato=1, richiestacancellazione = null, UserNameRichiestaCancellazione=null, datarichiestacancellazione = null "
                                    strsql = strsql & "where identepersonaleruolo='" & item.Cells(2).Text & "' and richiestacancellazione = 1"
                                    cmdGenerico = New SqlClient.SqlCommand(strsql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
                                    cmdGenerico.ExecuteNonQuery()

                                    'preparo la insert nella cronologiaenterpersonaleruoli
                                    strsql = "insert into cronologiaentepersonaleruoli (IdEntePersonaleRuolo, Accreditato, DataCronologia, IdTipoCronologia, UserNameAccreditatore, Forzatura, UserNameInseritore, datainseritore) "
                                    strsql = strsql & "select identepersonaleruolo, accreditato, getdate(), '1', usernameaccreditatore, '0', usernameinseritore, datainseritore from entepersonaleruoli where identepersonaleruolo='" & item.Cells(2).Text & "'"

                                    'sql command momentaneo che mi esegue la insert
                                    cmdGenerico = New SqlClient.SqlCommand(strsql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
                                    cmdGenerico.ExecuteNonQuery()
                                    '    Case "Da Accreditare"
                                    '        strsql = "update entepersonaleruoli set DataFineValidità=GetDate(), UserNameCancellazione='" & Session("Utente") & "' "
                                    '        strsql = strsql & "where identepersonaleruolo='" & item.Cells(2).Text & "'"
                                    '    Case "Non Accreditato"
                                    '        strsql = "update entepersonaleruoli set DataFineValidità=GetDate(), UserNameCancellazione='" & Session("Utente") & "' "
                                    '        strsql = strsql & "where identepersonaleruolo='" & item.Cells(2).Text & "'"
                                    'End Select
                                Else
                                    'preparo la insert nella cronologiaenterpersonaleruoli
                                    strsql = "insert into cronologiaentepersonaleruoli (IdEntePersonaleRuolo, Accreditato, DataCronologia, IdTipoCronologia, UserNameAccreditatore, Forzatura, UserNameInseritore, datainseritore) "
                                    strsql = strsql & "select identepersonaleruolo, accreditato, getdate(), '1', usernameaccreditatore, '0', usernameinseritore, datainseritore from entepersonaleruoli where identepersonaleruolo='" & item.Cells(2).Text & "'"

                                    'sql command momentaneo che mi esegue la insert
                                    cmdGenerico = New SqlClient.SqlCommand(strsql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
                                    cmdGenerico.ExecuteNonQuery()
                                    'cmdGenerico.Dispose()

                                    'controllo se si tratta di un ruolo principale
                                    If strValoreRitornoStore = item.Cells(0).Text Then
                                        strsql = "insert into entepersonaleruoli (IDEntePersonale,"
                                        strsql = strsql & "IDRuolo,DataInizioValidità,Principale,DataInseritore,UserNameInseritore) values "
                                        strsql = strsql & "('" & idEntePersonale.Value & "','" & item.Cells(0).Text & "',GetDate(),1,GetDate(),'" & Session("Utente") & "')"
                                    Else
                                        strsql = "insert into entepersonaleruoli (IDEntePersonale,"
                                        strsql = strsql & "IDRuolo,DataInizioValidità,Principale,DataInseritore,UserNameInseritore) values "
                                        strsql = strsql & "('" & idEntePersonale.Value & "','" & item.Cells(0).Text & "',GetDate(),0,GetDate(),'" & Session("Utente") & "')"
                                    End If

                                    cmdGenerico = New SqlClient.SqlCommand(strsql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
                                    cmdGenerico.ExecuteNonQuery()
                                End If
                            End If
                        End If
                    Next

                    'carico l'anagrafica
                    CaricaAnagrafica()
                    CaricaRuoliModifica(idEntePersonale.Value, AlboEnte)
                    Dim entity As New Entity With {
                     .Id = idEntePersonale.Value,
                     .Name = "EntePersonale"
                    }
                    Log.Information(LogEvent.RISORSE_ENTE_MODIFICA)

                End If

                'nel caso in cui ci sia un errore stampo a inizio pagina il 
                'messaggio dell'errore generato in fase di esecuzione
            Catch ex As Exception
                Log.Error(LogEvent.RISORSE_ENTE_ERRORE, "Aggiornamento dati", ex)
                MessaggiAlert("Errore nell'aggiornamento dati.")
            End Try
        End If
        'controllo e chiudo se aperto il datareader
        If Not dtrCheckCodFis Is Nothing Then
            dtrCheckCodFis.Close()
            dtrCheckCodFis = Nothing
        End If
        'controllo e chiudo se aperto il datareader
        If Not dtrIdMax Is Nothing Then
            dtrIdMax.Close()
            dtrIdMax = Nothing
        End If
        'Else

        '    CaricaAnagrafica()
        '    '    SvuotaCampi()
        'End If
        If blnCheckCodFis = True Then
            lblErr.Visible = True
            lblErr.Text = lblErr.Text & " La Risorsa e' stata salvata. Attenzione, il Codice Fiscale era gia' in uso."
            'Response.Write("<script language=""javascript"">" & vbCrLf)
            '' Response.Write("<!--" & vbCrLf)
            'Response.Write("alert(""La Risorsa e' stata salvata. Attenzione, il Codice Fiscale era gia' in uso."");" & vbCrLf)
            ''Response.Write("//-->" & vbCrLf)
            'Response.Write("</script>" & vbCrLf)
        End If
        Session("IdComune") = Nothing
    End Sub
    Private Function InsertCV(ByVal CV As Allegato, ByVal IdEnteFase As Integer) As Integer
        'Inserisce in entidocumenti un nuovo curriculum
        Dim UserName As String = If(Session("CodiceFiscaleUtente"), Session("Account"))
        Dim insertAllegatoCommand As New SqlCommand("", Session("conn"))
        Dim idAllegato As Integer = SalvaAllegato(CV, TipoFile.CURRICULUM_RISORSA, IdEnteFase, insertAllegatoCommand)
        insertAllegatoCommand.Dispose()
        Return idAllegato
    End Function
    Private Sub MessaggiAlert(ByVal strMessaggio)
        lblErr.Visible = True
        lblErr.CssClass = "msgErrore"
        lblErr.Text = strMessaggio
    End Sub

    Private Sub MessaggiPopup(ByVal strMessaggio)
        lblErroreUpload.Visible = True
        lblErroreUpload.Text = strMessaggio
        popUpload.Show()
    End Sub

    Private Sub MessaggiSuccess(ByVal strMessaggio)
        lblErr.Visible = True
        lblErr.CssClass = "msgInfo"
        lblErr.Text = strMessaggio
    End Sub

    Private Function NazionalitaItaliana(ByVal pComune As String) As Boolean
        Dim dtrNazioneBase As Data.SqlClient.SqlDataReader
        Dim strsql As String

        strsql = "SELECT Nazioni.NazioneBase FROM Nazioni " &
           "INNER JOIN Regioni ON Regioni.IdNazione = Nazioni.IdNazione " &
           "INNER JOIN Provincie ON Provincie.IdRegione = Regioni.IdRegione " &
           "INNER JOIN Comuni ON Comuni.IdProvincia = Provincie.IdProvincia " &
           "WHERE Comuni.Denominazione = '" & ClsServer.NoApice(pComune) & "'"

        dtrNazioneBase = ClsServer.CreaDatareader(strsql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))

        dtrNazioneBase.Read()
        If dtrNazioneBase.HasRows = False Then
            NazionalitaItaliana = False
        Else
            NazionalitaItaliana = dtrNazioneBase("NazioneBase")
        End If

        dtrNazioneBase.Close()
        dtrNazioneBase = Nothing
    End Function

    Private Function ControlloInserimentoFormatore(ByVal ModIns As Integer, ByVal IDEntePersonale As Integer, ByVal AlboEnte As String) As Boolean
        '10/12/2014
        'Verifico se si sta inserrendo in Formatore(nuova risorsa)
        'i campi chkCorsoOlp e ddlCorso sono obbigatori
        Dim item As DataGridItem
        Dim IDRuolo As String ' = "2"

        'aggiunto il 19/07/2017 da s.c.
        'If (AlboEnte = "" Or AlboEnte = "SCU") Then
        '    IDRuolo = "21" 'Responsabile della Formazione e Valorizzazione delle Competenze
        'Else
        '    IDRuolo = "2" 'Formatore
        'End If


        If ModIns = 2 Then 'inserimento
            For Each item In dtgRuoli.Items
                'verifico se è  stato inserito un formatore
                ' If ModIns = 2 Then 'inserimento

                Dim check As CheckBox = DirectCast(item.FindControl("chkAssegnaRuolo"), CheckBox)

                'controllo se la check è spuntata o meno
                If check.Checked = True Then
                    If (item.Cells(0).Text = 2 Or item.Cells(0).Text = 21) Then
                        Return True
                        Exit Function
                    End If
                End If

                'If (Request.Form("IdVal1") = IDRuolo _
                '        Or Request.Form("IdVal2") = IDRuolo _
                '        Or Request.Form("IdVal3") = IDRuolo _
                '        Or Request.Form("IdVal4") = IDRuolo) Then
                '    Return True
                '    Exit Function
                'End If
                'Else
                '    IDRuolo = item.Cells(0).Text
                '    If IDRuolo = "&nbsp;" Then
                '        IDRuolo = -1
                '    End If
                '    If IDRuolo <> -1 Then


                '        'strsql = "select IDRuolo  From ruoli where IDEntePersonale= " & IDEntePersonale & " and IDRuolo =" & IDRuolo & " "

                '        'dtrgenerico = ClsServer.CreaDatareader(strsql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
                '        'If dtrgenerico.HasRows = True Then
                '        '    dtrgenerico.Read()
                '        '    If dtrgenerico("Accreditato") = 1 Then
                '        '        If Not dtrgenerico Is Nothing Then
                '        '            dtrgenerico.Close()
                '        '            dtrgenerico = Nothing
                '        '        End If
                '        '        Return True
                '        '        Exit Function
                '        '    End If
                '        'End If



                '        If IDRuolo = 2 Then
                '            Return True
                '            Exit Function
                '        End If
                '    End If
                'End If
            Next
        Else 'modifica controllo i ruoli assegnati alla risorsa 


            For Each item In dtgRuoli.Items
                'verifico se è  stato inserito un formatore
                ' If ModIns = 2 Then 'inserimento


                Dim check As CheckBox = DirectCast(item.FindControl("chkAssegnaRuolo"), CheckBox)

                'controllo se la check è spuntata o meno
                If check.Checked = True And item.Cells(1).Text <> "1" Then
                    'If (item.Cells(0).Text = IDRuolo) Then
                    If (item.Cells(0).Text = 2 Or item.Cells(0).Text = 21) Then
                        Return True
                        Exit Function

                    End If
                End If

            Next

            'strsql = "select IDRuolo  From entepersonaleruoli where IDEntePersonale= " & IDEntePersonale & " and IDRuolo =2 and datafinevalidità is null"
            'dtrgenerico = ClsServer.CreaDatareader(strsql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
            'If dtrgenerico.HasRows = True Then
            '    If Not dtrgenerico Is Nothing Then
            '        dtrgenerico.Close()
            '        dtrgenerico = Nothing
            '    End If
            '    Return True
            '    Exit Function
            'End If



        End If


        'If Not dtrgenerico Is Nothing Then
        '    dtrgenerico.Close()
        '    dtrgenerico = Nothing
        'End If


        Return False
    End Function

    'Pulisco le txt nel caso di salvataggio effettuato
    Sub SvuotaCampi()
        'pulisco i campi a video
        ddlCorso.SelectedValue = 0
        ddlEsperienzaServizioCivile.SelectedValue = 0
        txtCellulare.Text = ""
        txtCivico.Text = ""
        txtCodiceFiscale.Text = ""
        txtCognome.Text = ""
        ddlComuneNascita.SelectedIndex = 0
        ddlComuneResidenza.SelectedIndex = 0
        txtDataNascita.Text = ""
        txtEmail.Text = ""
        txtFax.Text = ""
        txtIndirizzo.Text = ""
        TxtDettaglioRecapito.Text = ""
        TxtDettaglioRecapito.Text = ""
        txtNome.Text = ""
        txtPosizione.Text = ""
        txtTelefono.Text = ""
        txtTitolo.Text = ""
        ddlProvinciaNascita.SelectedIndex = 0
        ddlProvinciaResidenza.SelectedIndex = 0
        txtCAP.Text = ""
        chkCorsoOlp.Checked = False
        ClearSessionCV()
        Session("LoadCVId") = Nothing
        'Session("ModIns") = 0
        'lblErr.Text = ""
        'Imgerrore.Visible = False
        'txtCodiceFiscale.BackColor = txtCodiceFiscale.BackColor.White
        'datarow e datatable che utilizzerò per caricare a vuoto la datagrid
        'Dim dtR As DataRow
        'Dim dtT As New DataTable
        ''variabile contatore
        'Dim i As Integer
        ''carico le colonne della datagrid
        'dtT.Columns.Add(New DataColumn("IDRuolo", GetType(String)))
        'dtT.Columns.Add(New DataColumn("Ruolo", GetType(String)))
        'dtT.Columns.Add(New DataColumn("Principale", GetType(String)))
        'dtT.Columns.Add(New DataColumn("Accreditato", GetType(String)))
        'dtT.Columns.Add(New DataColumn("Visibilità", GetType(String)))

        'dtT.Columns.Add()
        'dtR = dtT.NewRow()
        ''carico la prima riga a se perchè devo stampare a video "Nessun Ruolo Selezionato."
        'dtR(0) = ""
        'dtR(1) = "<input type=text style=""width: 250; background-color: #C9DFFF; border=0 none; ; font-family:verdana; color:#000080; font-size:7pt; font-weight:normal"" readonly name=Val1 value=""Nessun Ruolo Selezionato."">"
        'dtR(2) = "<img src=images/selezionato_small.png border=0 style='width:20px;height:20px'>"
        'dtR(3) = "<input type=text style=""width: 50; background-color: #C9DFFF; border=0 none; ; font-family:verdana; color:#000080; font-size:7pt; font-weight:normal"" readonly name=Accr1 value="""">"
        'dtR(4) = "<select disabled=true name=""ddlVisibilita1"">" & vbCrLf & "<option value=""0"">Privata</option>" & vbCrLf & "<option value=""1"">Riservata</option>" & vbCrLf & "<option value=""2"">Pubblica</option>" & vbCrLf & "</select>"

        'dtT.Rows.Add(dtR)
        ''carico le restanti tre righe vuote
        'For i = 2 To 4
        '    dtR = dtT.NewRow()
        '    dtR(0) = ""
        '    dtR(1) = "<input type=text style=""width: 250; background-color: #C9DFFF; border=0 none; ; font-family:verdana; color:#000080; font-size:7pt; font-weight:normal"" readonly name=Val" & i & " value="""">"
        '    dtR(2) = ""
        '    dtR(3) = "<input type=text style=""width: 50; background-color: #C9DFFF; border=0 none; ; font-family:verdana; color:#000080; font-size:7pt; font-weight:normal"" readonly name=Accr" & i & " value="""">"
        '    dtR(4) = "<select disabled=true name=""ddlVisibilita" & i.ToString & """>" & vbCrLf & "<option value=""0"">Privata</option>" & vbCrLf & "<option value=""1"">Riservata</option>" & vbCrLf & "<option value=""2"">Pubblica</option>" & vbCrLf & "</select>"

        '    dtT.Rows.Add(dtR)
        'Next
        ''assegno alla datagrid la datatable appena creata
        'dtgRuoloPrincipale.DataSource = dtT
        'dtgRuoloPrincipale.DataBind()
    End Sub

    Private Sub imgCancella_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles imgCancella.Click
        'variabile stringa locale che uso per effettuare l'update
        Dim strUpdateCommand As String
        Dim myCommand As New SqlClient.SqlCommand
        Dim idPersonaleEnte = CInt(Request.QueryString("intPersonaleEnteAssociato"))
        Dim entity As New Entity With {
         .Id = idPersonaleEnte,
         .Name = "PersonaleEnte"
        }
        'controllo se la risorsa in modifica è una risorsa acquisita per cui non permetto la sua cancellazione
        'True se è acquisita e quindi non cancellabile, False se cancellabile
        If CheckRisorsaAcquisita(CInt(Request.QueryString("intPersonaleEnteAssociato"))) = True Then
            'Imgerrore.Visible = True
            Log.Warning(LogEvent.RISORSE_ENTE_ERRORE_ELIMINAZIONE, "Ruolo acquisito", entity:=entity)
            MessaggiAlert("Non è possibile cancellare la risorsa perchè risulta avere un ruolo acquisito.")
            Exit Sub
        End If

        'dalla store ricavo le informazioni necessarie per stabilire se posso o no "cancellare" la risorsa
        'se la store restituisce false posso procedere con le operazioni sulla base dati
        'update sulla datafinevalidità settata a quella dell'operazione
        If LeggiStore(CInt(Request.QueryString("intPersonaleEnteAssociato"))) = False Then

            If HasRuoloResponsabileIscritto(idPersonaleEnte) Then   'se ha un ruolo di responsabile accreditato=ISCRITTO

                If EsisteCambioStrutturaGestioneNonValutato() Then
                    'impedisco l'eliminazione con popup
                    alertTitolo.InnerText = "Impossibile cancellare la risorsa"
                    lblAlert.Text = "E' già in corso di valutazione un cambio della Struttura di Gestione.<br>E' eventualmente possibile richiedere via PEC al Dipartimento la rinuncia all'Adeguamento precedentemente presentato e non ancora valutato."
                    mpeALERT.Show()
                    Exit Sub
                Else
                    Session("entePersonaleMostraInfo") = True
                End If

            End If

            Try
                If Session("TipoUtente") = "E" Then
                    'carico la stringa con la query per l'update RICHIESTACANCELLAZIONE=1
                    strUpdateCommand = "update entepersonale set richiestacancellazione=1,UserNameRichiestaCancellazione='" & Session("Utente") & "', datarichiestacancellazione = getdate(),DataFineValidità=GetDate(), UsernameCancellazione='" & Session("Utente") & "'  where IDEntePersonale='" & Request.QueryString("intPersonaleEnteAssociato") & "'"
                    'setto i parametri del sqlcommand necessari per eseguire l'operazione
                    myCommand = New SqlClient.SqlCommand(strUpdateCommand, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
                    myCommand.ExecuteNonQuery()
                    'distruggo dal garbage collection il sqlcommand
                    myCommand.Dispose()
                Else
                    'carico la stringa con la query per l'update RICHIESTACANCELLAZIONE=1
                    strUpdateCommand = "update entepersonale set DataFineValidità=GetDate(), UsernameCancellazione='" & Session("Utente") & "'  where IDEntePersonale='" & Request.QueryString("intPersonaleEnteAssociato") & "'"
                    'setto i parametri del sqlcommand necessari per eseguire l'operazione
                    myCommand = New SqlClient.SqlCommand(strUpdateCommand, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
                    myCommand.ExecuteNonQuery()
                    'distruggo dal garbage collection il sqlcommand
                    myCommand.Dispose()
                End If

                'tutti i ruoli accreditati vedere gestioneruoli con condizione accreditato e utente =E
                'modificato da simona il 0/09/2014 accreditamento 2014
                If Session("TipoUtente") = "E" And ControlloRisorsaAccreditata(Request.QueryString("intPersonaleEnteAssociato")) = True Then 'item.Cells(4).Text = "Accreditato" And
                    CancellaRuoliAccreditati(Request.QueryString("intPersonaleEnteAssociato"))
                End If
                'ritorno alla pagina di ricerca
                'Response.Redirect("ricercaentepersonale.aspx")
                'Response.Redirect("entepersonale.aspx")
                Log.Information(LogEvent.RISORSE_ENTE_CANCELLAZIONE, entity:=entity)
                Response.Redirect("entepersonale.aspx?VengoDaProgetti=" & Request.QueryString("VengoDaProgetti") & "&intPersonaleEnteAcquisito=" & Request.QueryString("intPersonaleEnteAcquisito") & "&intPersonaleEnteAssociato=" & Request.QueryString("intPersonaleEnteAssociato") & "&intEnteAssociato=" & Request.QueryString("intEnteAssociato") & "&Denominazione=" & Request.QueryString("Denominazione") & "&Cancellato=" & Request.QueryString("Cancellato") & "&tipoazione=" & "Modifica" & "")
            Catch ex As Exception

                Log.Error(LogEvent.RISORSE_ENTE_ERRORE, "Cancellazione", ex, entity:=entity)

                'Response.Write(ex.Message.ToString())
            End Try
        Else
            'Imgerrore.Visible = True
            Log.Warning(LogEvent.RISORSE_ENTE_ERRORE_ELIMINAZIONE, "Risorsa Impegnata", entity:=entity)
            MessaggiAlert("Non e' possibile cancellare la risorsa risulta essere impegnata.")
        End If
    End Sub

    Function CheckRisorsaAcquisita(ByVal IdRisorsa As Integer) As Boolean
        'variabile che uso per caricare la select per verificare se è possivbile o meno cancellare la risorsa
        Dim strQuery As String
        'datareader che utilizzo per accedere alla base dati e per verificare
        Dim dtrCheckCancellazione As Data.SqlClient.SqlDataReader

        strQuery = "select * from personaleacquisito as a "
        strQuery = strQuery & "inner join entepersonaleruoli as b on a.IDEntePersonaleRuolo=b.IDEntePersonaleRuolo "
        strQuery = strQuery & "inner join entepersonale as c on  b.IDEntePersonale=c.IDEntePersonale "
        strQuery = strQuery & "where c.identepersonale=" & IdRisorsa & " and a.approvato in (0,2,3)"

        'eseguo la query
        dtrCheckCancellazione = ClsServer.CreaDatareader(strQuery, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))

        'controllo se ci sono record. Se ci sono record vuol dire che per almeno un ruolo
        'la risorsa è stata acquisita da un altro ente, quindi incancellabile
        If dtrCheckCancellazione.HasRows = True Then
            CheckRisorsaAcquisita = True
        Else
            CheckRisorsaAcquisita = False
        End If

        'chiudo il datareader
        If Not dtrCheckCancellazione Is Nothing Then
            dtrCheckCancellazione.Close()
            dtrCheckCancellazione = Nothing
        End If

        'rimando il valore alle if in fase di cancellazione
        Return CheckRisorsaAcquisita
    End Function

    'routine che uso per capire se la risorsa è "cancellabile" (= false)
    Private Function LeggiStore(ByVal IdEntePersonale As Integer) As Boolean
        'sqlcommand per eseguire la store
        Dim CustOrderHist As SqlClient.SqlCommand
        CustOrderHist = New SqlClient.SqlCommand
        'istanzio i parametri necessari al suo corretto utilizzo
        CustOrderHist.CommandType = CommandType.StoredProcedure
        CustOrderHist.CommandText = "SP_VERIFICA_IMPEGNO_RISORSA"
        CustOrderHist.Connection = IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn"))

        'primo parametro necessario all'escuzione della store
        Dim sparam As SqlClient.SqlParameter
        sparam = New SqlClient.SqlParameter
        'assegno al primo parametro il nome del primo parametro di ingresso
        sparam.ParameterName = "@IdEntePersonale"
        'e ne definisco il tipo
        sparam.SqlDbType = SqlDbType.Int
        'al command aggiungo il primo parametro appena settato
        CustOrderHist.Parameters.Add(sparam)

        'secondo parametro necessario all'escuzione della store
        Dim sparam1 As SqlClient.SqlParameter
        sparam1 = New SqlClient.SqlParameter
        'assegno al secondo parametro il nome del primo parametro di ingresso
        sparam1.ParameterName = "@Valore"
        'e ne definisco il tipo
        sparam1.SqlDbType = SqlDbType.Bit
        'definisco il tipo di utilizzo che necessito da "@Valore"
        sparam1.Direction = ParameterDirection.Output
        'al command aggiungo il secondo parametro appena settato
        CustOrderHist.Parameters.Add(sparam1)

        'datareader che assegno come risultato dell'esecuzione del command CustOrderHist
        Dim Reader As SqlClient.SqlDataReader
        'controllo e chiudo il datareader
        If Not Reader Is Nothing Then
            Reader.Close()
            Reader = Nothing
        End If
        'passo al sqlcommand il valore in ingresso dell'id dell'entepersonale
        CustOrderHist.Parameters("@IdEntePersonale").Value = IdEntePersonale
        'eseguo il command
        Reader = CustOrderHist.ExecuteReader()
        'assegno il risultato alla funzione
        LeggiStore = CustOrderHist.Parameters("@Valore").Value
        'controllo e chiudo il datareader
        If Not Reader Is Nothing Then
            Reader.Close()
            Reader = Nothing
        End If
    End Function

    Private Sub CancellaRuoliAccreditati(ByVal IdEntePersonale As Integer)
        '09/09/2014
        'Cancello solo i ruoli accreditati con datafinevalidità =null (solo per gli utenti di tipo E)

        Dim item As DataGridItem
        Dim IDRuolo As String
        Dim myCommand As New SqlClient.SqlCommand

        For Each item In dtgRuoli.Items
            IDRuolo = item.Cells(0).Text

            If IDRuolo <> "&nbsp;" Then
                strsql = "UPDATE entepersonaleruoli SET "
                strsql = strsql & " richiestacancellazione = 1, UserNameRichiestaCancellazione='" & Session("Utente") & "', "
                strsql = strsql & " datarichiestacancellazione = getdate() "
                strsql = strsql & " WHERE identepersonale='" & Request.QueryString("intPersonaleEnteAssociato") & "' "
                strsql = strsql & " and idRuolo =" & IDRuolo & " and DataFineValidità is null and Accreditato=1 "
                myCommand = New SqlClient.SqlCommand(strsql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
                myCommand.ExecuteNonQuery()
                myCommand.Dispose()
            End If

        Next
    End Sub

    Private Sub cmdChiudi_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdChiudi.Click
        'controllo se vengo dall'albero in gestione enti
        If Not Request.QueryString("VengoDa") Is Nothing Then
            'controllo se la variabile è valorizzata
            If Request.QueryString("VengoDa") <> "" Then
                'faccio la response.redirect verso l'albero
                Response.Redirect(Request.QueryString("VengoDa").ToString)
            End If
        End If
        'controllo di che operazione si tratta Modifica/Inserimento
        If Not Request.QueryString("tipoazione") Is Nothing Then
            'se si tratta di modifica
            If Request.QueryString("tipoazione") = "Modifica" Then
                'VengoDaRicercaTotale=true& Aggiunto DA Antonello il 18/10/2005 se vengo dalla ricerca Risorse Totali
                If Request.QueryString("VengoDaRicercaTotale") = "true" Then
                    Response.Redirect("RicercaRisorseTot.aspx?VengoDaProgetti=" & Request.QueryString("VengoDaProgetti"))
                Else
                    'ritorno i ricerca che di default carica tutte le risorse relative all'ente loggato
                    Response.Redirect("ricercaentepersonale.aspx?VengoDaProgetti=" & Request.QueryString("VengoDaProgetti"))
                End If
            Else
                'se si tratta di inserimento torno alla main
                Response.Redirect("WfrmMain.aspx")
            End If
        End If
        Session("IdComune") = Nothing
    End Sub


    'Private Sub dtgRuoloPrincipale_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtgRuoloPrincipale.SelectedIndexChanged
    '    'se si tratta di modifica
    '    If Session("ModIns") = 1 Then
    '        'controllo se è selezionato un item nella datagrid
    '        If Not dtgRuoloPrincipale.SelectedItem Is Nothing Then
    '            'mando l'utente nella pagina informativa della risorsa
    '            Select Case dtgRuoloPrincipale.SelectedIndex()
    '                Case 0
    '                    If dtgRuoloPrincipale.SelectedItem().Cells(0).Text <> "&nbsp;" Then
    '                        'primo item della datagrid
    '                        Response.Redirect("WfrmAlbero.aspx?Tipologia=Ruoli&IDEntePersonale=" & CInt(Request.QueryString("intPersonaleEnteAssociato")) & "&IDRuolo=" & IIf(Request.Form("IdVal1") = "", CInt(dtgRuoloPrincipale.SelectedItem().Cells(0).Text), Request.Form("IdVal1")) & "")
    '                    End If
    '                Case 1
    '                    If dtgRuoloPrincipale.SelectedItem().Cells(0).Text <> "&nbsp;" Then
    '                        'secondo item della datagrid
    '                        Response.Redirect("WfrmAlbero.aspx?Tipologia=Ruoli&IDEntePersonale=" & CInt(Request.QueryString("intPersonaleEnteAssociato")) & "&IDRuolo=" & IIf(Request.Form("IdVal2") = "", CInt(dtgRuoloPrincipale.SelectedItem().Cells(0).Text), Request.Form("IdVal2")) & "")
    '                    End If
    '                Case 2
    '                    If dtgRuoloPrincipale.SelectedItem().Cells(0).Text <> "&nbsp;" Then
    '                        'terzo item della datagrid
    '                        Response.Redirect("WfrmAlbero.aspx?Tipologia=Ruoli&IDEntePersonale=" & CInt(Request.QueryString("intPersonaleEnteAssociato")) & "&IDRuolo=" & IIf(Request.Form("IdVal3") = "", CInt(dtgRuoloPrincipale.SelectedItem().Cells(0).Text), Request.Form("IdVal3")) & "")
    '                    End If
    '                Case 3
    '                    If dtgRuoloPrincipale.SelectedItem().Cells(0).Text <> "&nbsp;" Then
    '                        'quarto item della datagrid
    '                        Response.Redirect("WfrmAlbero.aspx?Tipologia=Ruoli&IDEntePersonale=" & CInt(Request.QueryString("intPersonaleEnteAssociato")) & "&IDRuolo=" & IIf(Request.Form("IdVal4") = "", CInt(dtgRuoloPrincipale.SelectedItem().Cells(0).Text), Request.Form("IdVal4")) & "")
    '                    End If
    '            End Select
    '        End If
    '    Else
    '        'non permetto la selezione in inserimento
    '        dtgRuoloPrincipale.SelectedIndex = -1
    '    End If
    'End Sub

    Private Sub dtgRuoli_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dtgRuoli.ItemCommand
        'generato da Bagnani Jonathan
        'il 04/06/2004
        'controllo quale elemento "select" è stato selezionato nella 
        'datagrid dei ruoli
        'controllo se esiste un ruolo per il pulsante selezionato
        If e.Item.Cells(0).Text <> "&nbsp;" Then
            Select Case e.CommandName
                'se ho selezionato la colonna dei dettagli
                Case "Dettagli"
                    'stampo a pagina uno script che mi apre la popup dei dettagli
                    Response.Write("<script>" & vbCrLf)
                    Response.Write("window.open(""dettaglirisorsa.aspx?idruolo=" & e.Item.Cells(0).Text & "&idrisorsa=" & CInt(Request.QueryString("intPersonaleEnteAssociato")) & """, """", ""height=500,toolbar=no,location=no,menubar=no,scrollbars=yes,resizable=yes"")" & vbCrLf)
                    Response.Write("</script>")
                Case "Sostituzione"
                    If Session("TipoUtente") = "U" Or Session("TipoUtente") = "R" Then

                        If e.Item.Cells(2).Text <> "0" Then
                            Response.Redirect("WfrmAlbero.aspx?Tipologia=Ruoli&IDEntePersonale=" & CInt(Request.QueryString("intPersonaleEnteAssociato")) & "&IDRuolo=" & IIf(Request.Form("IdVal1") = "", CInt(e.Item.Cells(0).Text), Request.Form("IdVal1")) & "")
                        End If
                    End If
            End Select
        End If
        'hidden per nascondere l'id della risorsa selezionata
        'Response.Write("<input type=hidden name=IdEntePersonale value=""" & CInt(Request.QueryString("intPersonaleEnteAssociato")) & """>")
        IdEntePersonale.Value = Request.QueryString("intPersonaleEnteAssociato")
    End Sub

    'Private Sub txtCodiceFiscale_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtCodiceFiscale.TextChanged
    '    Dim dtrCheckCodFis As SqlClient.SqlDataReader
    '    Dim strsql As String
    '    lblErr.Text = vbNullString

    '    'diverso da modifica
    '    If Session("ModIns") <> 1 Then
    '        strsql = "select CodiceFiscale, IDEnte from entepersonale " & _
    '                 "where CodiceFiscale = '" & Trim(txtCodiceFiscale.Text) & "'"

    '        'controllo e chiudo se aperto il datareader
    '        If Not dtrCheckCodFis Is Nothing Then
    '            dtrCheckCodFis.Close()
    '            dtrCheckCodFis = Nothing
    '        End If
    '        'eseguo la query
    '        dtrCheckCodFis = ClsServer.CreaDatareader(strsql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
    '        dtrCheckCodFis.Read()

    '        If dtrCheckCodFis.HasRows = True Then
    '            'se esiste controllo l'ente

    '            If dtrCheckCodFis("IdEnte") = Session("IdEnte") Then
    '                'se è stesso ente faccio sparire il bottone conferma + alert
    '                cmdSalva.Visible = False
    '                'lblErr.Visible = True
    '                'lblErr.Text = "La Risorsa non puo' essere salvata. Attenzione, il Codice Fiscale e' gia' presente."

    '                'Response.Write("<script language=""javascript"">" & vbCrLf)
    '                'Response.Write("alert(""La Risorsa non puo' essere salvata. Attenzione, il Codice Fiscale e' gia' presente."");" & vbCrLf)
    '                'Response.Write("</script>" & vbCrLf)
    '            Else
    '                'se è stesso ente faccio apparire il bottone conferma + alert
    '                cmdSalva.Visible = True
    '                'lblErr.Visible = True
    '                'lblErr.Text = lblErr.Text & "Attenzione, la Risorsa è associata ad un altro Ente."

    '                'Response.Write("<script language=""javascript"">" & vbCrLf)
    '                'Response.Write("alert(""Attenzione, la Risorsa è associata ad un altro Ente."");" & vbCrLf)
    '                'Response.Write("</script>" & vbCrLf)
    '            End If
    '        Else
    '            'faccio apparire il bottone conferma
    '            cmdSalva.Visible = True
    '        End If
    '        'controllo e chiudo se aperto il datareader
    '        If Not dtrCheckCodFis Is Nothing Then
    '            dtrCheckCodFis.Close()
    '            dtrCheckCodFis = Nothing
    '        End If
    '    End If
    'End Sub

    'Private Sub dtgRuoloPrincipale_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dtgRuoloPrincipale.PageIndexChanged
    '    dtgRuoloPrincipale.CurrentPageIndex = e.NewPageIndex
    '    CaricaAnagrafica()
    'End Sub

    Private Sub cmdAnnullaCancellazione_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdAnnullaCancellazione.Click
        AnnullaRichiestaCancellazione(Request.QueryString("intPersonaleEnteAssociato"))
        Log.Information(LogEvent.RISORSE_ENTE_ANNULLA_CANCELLAZIONE)
        Response.Redirect("entepersonale.aspx?VengoDaProgetti=" & Request.QueryString("VengoDaProgetti") & "&intPersonaleEnteAcquisito=" & Request.QueryString("intPersonaleEnteAcquisito") & "&intPersonaleEnteAssociato=" & Request.QueryString("intPersonaleEnteAssociato") & "&intEnteAssociato=" & Request.QueryString("intEnteAssociato") & "&Denominazione=" & Request.QueryString("Denominazione") & "&Acquisito=" & Request.QueryString("Acquisito") & "&tipoazione=" & "Modifica" & "")

    End Sub

    Private Sub AnnullaRichiestaCancellazione(ByVal IdEntePersonale As Integer)
        '15/09/2014
        'Annullo la richiesta  di cancellazione
        Dim myCommand As New SqlClient.SqlCommand

        strsql = "update entepersonale set richiestacancellazione=0,UserNameRichiestaCancellazione=null, datarichiestacancellazione = null,DataFineValidità=null, UsernameCancellazione=null  where IDEntePersonale='" & IdEntePersonale & "'"
        'setto i parametri del sqlcommand necessari per eseguire l'operazione
        myCommand = New SqlClient.SqlCommand(strsql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
        myCommand.ExecuteNonQuery()

        strsql = "UPDATE entepersonaleruoli SET "
        strsql = strsql & " richiestacancellazione = 0,UserNameCancellazione = null, DataFineValidità=null,UserNameRichiestaCancellazione=null, "
        strsql = strsql & " datarichiestacancellazione = null "
        strsql = strsql & " WHERE identepersonale='" & IdEntePersonale & "' and richiestacancellazione = 1 "
        strsql = strsql & "  and Accreditato=1 "
        myCommand = New SqlClient.SqlCommand(strsql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
        myCommand.ExecuteNonQuery()
        myCommand.Dispose()

    End Sub

    Function controlliSalvataggioServer() As Boolean
        Dim campiValidi As Boolean = True
        If txtCognome.Text.Trim = String.Empty Then
            lblErr.Visible = True
            lblErr.Text = lblErr.Text & "Inserire il Cognome."
            lblErr.Text = lblErr.Text & " </br>"
            campiValidi = False
        End If

        If txtNome.Text.Trim = String.Empty Then
            lblErr.Visible = True
            lblErr.Text = lblErr.Text & "Inserire il Nome."
            lblErr.Text = lblErr.Text & " </br>"
            campiValidi = False
        End If

        If txtCodiceFiscale.Text.Trim = String.Empty Then
            lblErr.Visible = True
            lblErr.Text = lblErr.Text & "Inserire il Codice Fiscale."
            lblErr.Text = lblErr.Text & " </br>"
            campiValidi = False
        Else
            If txtCodiceFiscale.Text.Length <> 16 Then
                lblErr.Visible = True
                lblErr.Text = lblErr.Text & "Codice Fiscale non corretto."
                lblErr.Text = lblErr.Text & " </br>"
                campiValidi = False
            End If
        End If
        'Dim regex As Regex = New Regex("^[_a-z0-9-]+(.[a-z0-9-]+)@[a-z0-9-]+(.[a-z0-9-]+)*(.[a-z]{2,4})$")
        Dim regex As Regex = New Regex("^(?("")("".+?""@)|(([0-9a-zA-Z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-zA-Z])@))(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,6}))$")

        Dim match As Match = regex.Match(txtEmail.Text.Trim)

        If txtEmail.Text.Trim <> String.Empty AndAlso match.Success = False Then
            lblErr.Visible = True
            lblErr.Text = lblErr.Text & "Il campo 'Email' non è valido."
            lblErr.Text = lblErr.Text & " </br>"
            campiValidi = False
        End If

        Dim telefono As Int64
        Dim telefonoInteger As Boolean
        telefonoInteger = Int64.TryParse(txtTelefono.Text.Trim, telefono)

        If txtTelefono.Text.Trim <> String.Empty AndAlso txtTelefono.Text.Length > 11 Then
            lblErr.Visible = True
            lblErr.Text = lblErr.Text & "Il campo 'Telefono' può contenere massimo 11 caratteri numerici."
            lblErr.Text = lblErr.Text & " </br>"
            campiValidi = False
        End If

        If txtTelefono.Text.Trim <> String.Empty AndAlso telefonoInteger = False Then
            lblErr.Visible = True
            lblErr.Text = lblErr.Text & "Il campo 'Telefono' può contenere solo numeri."
            lblErr.Text = lblErr.Text & " </br>"
            campiValidi = False
        End If



        Dim fax As Int64
        Dim faxInteger As Boolean
        faxInteger = Int64.TryParse(txtFax.Text.Trim, fax)
        If txtFax.Text.Trim <> String.Empty AndAlso txtTelefono.Text.Length > 11 Then
            lblErr.Visible = True
            lblErr.Text = lblErr.Text & "Il campo 'Fax' può contenere massimo 11 caratteri numerici."
            lblErr.Text = lblErr.Text & " </br>"
            campiValidi = False
        End If

        If txtFax.Text.Trim <> String.Empty AndAlso faxInteger = False Then
            lblErr.Visible = True
            lblErr.Text = lblErr.Text & "Il campo 'Fax' può contenere solo numeri."
            lblErr.Text = lblErr.Text & " </br>"
            campiValidi = False
        End If




        'If txtCellulare.Text.Trim <> String.Empty AndAlso IsNumeric(txtCellulare.Text.Trim) = False Then
        '    lblErr.Visible = True
        '    lblErr.Text = lblErr.Text & "Il campo 'Cellulare' può contenere solo numeri."
        '    lblErr.Text = lblErr.Text & " </br>"
        '    campiValidi = False
        'End If








        Dim cellulare As Int64
        Dim cellulareInteger As Boolean


        cellulareInteger = Int64.TryParse(txtCellulare.Text.Trim, cellulare)
        If txtCellulare.Text.Trim <> String.Empty AndAlso txtTelefono.Text.Length > 11 Then
            lblErr.Visible = True
            lblErr.Text = lblErr.Text & "Il campo 'Cellulare' può contenere massimo 11 caratteri numerici."
            lblErr.Text = lblErr.Text & " </br>"
            campiValidi = False
        End If
        If txtCellulare.Text.Trim <> String.Empty AndAlso cellulareInteger = False Then
            lblErr.Visible = True
            lblErr.Text = lblErr.Text & "Il campo 'Cellulare' può contenere solo numeri."
            lblErr.Text = lblErr.Text & " </br>"
            campiValidi = False
        End If

        If txtDataNascita.Text.Trim = String.Empty Then
            lblErr.Visible = True
            lblErr.Text = lblErr.Text & "Inserire la Data di Nascita."
            lblErr.Text = lblErr.Text & " </br>"
            campiValidi = False
        Else


            Dim dataNascita As Date
            Dim data As String = txtDataNascita.Text
            If Len(txtDataNascita.Text) < 10 Then
                lblErr.Visible = True
                lblErr.Text = lblErr.Text & "La 'Data di nascita' non è valida. Inserire la data nel formato GG/MM/AAAA."
                lblErr.Text = lblErr.Text & " </br>"
                campiValidi = False
            End If
            If (Date.TryParse(data, dataNascita) = False) Then
                ' If IsDate(data) = False Then
                lblErr.Visible = True
                lblErr.Text = lblErr.Text & "La 'Data di nascita' non è valida. Inserire la data nel formato GG/MM/AAAA."
                lblErr.Text = lblErr.Text & " </br>"
                campiValidi = False
            End If
        End If
        If ddlComuneNascita.SelectedIndex <= 0 Then
            lblErr.Visible = True
            lblErr.Text = lblErr.Text & "Selezionare Provincia e Comune di Nascita."
            lblErr.Text = lblErr.Text & " </br>"
            campiValidi = False
        End If

        If (txtCAP.Text.Trim <> String.Empty Or ddlComuneResidenza.SelectedIndex > 0 Or txtIndirizzo.Text.Trim <> String.Empty Or txtCivico.Text.Trim <> String.Empty) Then

            If ddlComuneResidenza.SelectedIndex <= 0 Then
                lblErr.Visible = True
                lblErr.Text = lblErr.Text & "Selezionare il Comune di Residenza."
                lblErr.Text = lblErr.Text & " </br>"
                campiValidi = False
            End If

            If txtIndirizzo.Text.Trim = String.Empty Then
                lblErr.Visible = True
                lblErr.Text = lblErr.Text & "Compilare il campo Indirizzo."
                lblErr.Text = lblErr.Text & " </br>"
                campiValidi = False
            End If

            If txtCivico.Text.Trim = String.Empty Then
                lblErr.Visible = True
                lblErr.Text = lblErr.Text & "Immettere il numero Civico."
                lblErr.Text = lblErr.Text & " </br>"
                campiValidi = False
            End If

            If txtCAP.Text.Trim = String.Empty Then
                lblErr.Visible = True
                lblErr.Text = lblErr.Text & "Inserire il Cap."
                lblErr.Text = lblErr.Text & " </br>"
                campiValidi = False
            End If

        End If

        Dim numeroCivico As Integer
        Dim numeroCivicoInteger As Boolean
        numeroCivicoInteger = Integer.TryParse(txtCivico.Text.Trim, numeroCivico)

        Dim CAP As Integer
        Dim CAPInteger As Boolean
        CAPInteger = Integer.TryParse(txtCAP.Text.Trim, CAP)

        If txtCAP.Text.Trim <> String.Empty AndAlso CAPInteger = False Then
            lblErr.Visible = True
            lblErr.Text = lblErr.Text & "Il campo 'C.A.P.' può contenere solo numeri."
            lblErr.Text = lblErr.Text & " </br>"
            campiValidi = False
        End If

        Dim checked As Boolean
        checked = False
        For Each item In dtgRuoli.Items
            Dim check As CheckBox = DirectCast(item.FindControl("chkAssegnaRuolo"), CheckBox)
            'controllo se la check è spuntata o meno
            If check.Checked = True Then
                checked = True
                Exit For
            End If
        Next
        If checked = False Then
            lblErr.Visible = True
            lblErr.Text = lblErr.Text & "Selezionare almeno un ruolo."
            lblErr.Text = lblErr.Text & " </br>"
            campiValidi = False
        End If

        If Session("LoadedCV") Is Nothing Then
            lblErr.Visible = True
            lblErr.Text = lblErr.Text & "È necessario inserire il Curriculum Vitae"
            lblErr.Text = lblErr.Text & " </br>"
            campiValidi = False
        End If

        Return campiValidi

    End Function

    'Private Sub imgRuoli_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles imgRuoli.Click

    '    If (Request.QueryString("Acquisito") = "Vero" Or Request.QueryString("Cancellato") = "Vero") Then
    '        Exit Sub
    '    End If

    '    If (chkStatoEnte.Value = "True") Then
    '        Exit Sub
    '    End If

    '    If Session("ModIns") = "1" Then

    '        Response.Write("<script>")
    '        Response.Write("window.open('gestioneruoli.aspx?IdPersonaleEnte=" & IdEntePersonale.Value & "', '', 'height=500,width=620,dependent=no,scrollbars=yes,status=no')")
    '        Response.Write("</script>")

    '    Else

    '        Response.Write("<script>")
    '        Response.Write("window.open('gestioneruoli.aspx','','height=500,width=620,dependent=no,scrollbars=yes,status=no')")
    '        Response.Write("</script>")

    '    End If

    'End Sub

    'routine che carica tutti i ruoli e i relativi stati sulla risorsa in inserimento
    Sub CaricaRuoliInserimento(ByVal AlboEnte As String)
        'variabile stringa per le query
        Dim strsql As String
        Dim dtsRuoli As Data.DataSet

        strsql = "select Ruoli.IdRuolo, '' as Accreditato, 0 as identepersonaleruolo, Ruoli.Ruolo, '' as Stato, "
        strsql = strsql & " ruoli.ruoloaccreditamento, '' as DataFineValidità "
        strsql = strsql & " FROM Ruoli "
        strsql = strsql & " LEFT JOIN OrdinamentoRuoli O ON Ruoli.IDRuolo =O.IdRuolo"
        strsql = strsql & " WHERE  Ruoli.Nascosto = 0 AND Ruoli.IdRuolo<>4" 'Escludo il rappresentnte legale
        If Session("TipoUtente") = "E" Then
            strsql = strsql & " and Ruoli.RuoloAccreditamento=1 "
        End If

        If AlboEnte = "" Then
            strsql &= " AND (Albo = 'SCU' OR Albo is null)"
        Else
            strsql &= " AND (Albo = '" & AlboEnte & "' OR Albo is null)"
        End If

        strsql = strsql & " order by isnull(o.Ordinamento,99)"

        dtsRuoli = ClsServer.DataSetGenerico(strsql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))

        dtgRuoli.DataSource = dtsRuoli
        dtgRuoli.DataBind()

        CaricaChekOLP()

    End Sub

    'routine che carica tutti i ruoli e i relativi stati sulla risorsa in modifica
    Sub CaricaRuoliModifica(ByVal identepersonale As String, ByVal AlboEnte As String)
        'variabile stringa per le query
        Dim strsql As String
        Dim dtsRuoli As Data.DataSet

        'case entepersonale.IdRuolo when isnull then '' else ruoli.idruolo end as IdRuolo

        'strsql = "case entepersonale.IdRuolo when isnull then '' else ruoli.idruolo end as IdRuolo,isnull(entepersonaleruoli.Accreditato,'') as Accreditato,ruoli.Ruolo,case entepersonaleruoli.Accreditato when 1 then 'Accreditato' when 0 then 'Da Accreditare' when -1 then 'Non Accreditato' else 'Non Definito' end as Stato, entepersonaleruoli.DataFineValidità, entepersonaleruoli.IDEntePersonaleRuolo, "
        'strsql = strsql & "entepersonaleruoli.IDRuolo, entepersonaleruoli.DataInizioValidità "
        'strsql = strsql & "FROM ruoli LEFT OUTER JOIN "
        'strsql = strsql & "entepersonaleruoli ON entepersonaleruoli.IDRuolo = ruoli.IDRuolo AND entepersonaleruoli.IDEntePersonale = 12"

        strsql = "SELECT case entepersonaleruoli.IdRuolo when null then '' else ruoli.idruolo end as IdRuolo, entepersonaleruoli.accreditato, isnull(entepersonaleruoli.identepersonaleruolo,0) as identepersonaleruolo, ruoli.Ruolo, case when entepersonaleruoli.datafinevalidità is null then case entepersonaleruoli.Accreditato when 1 then CASE WHEN E.IdStatoEnte=6 THEN 'Iscritto' ELSE 'Iscritto' END when 0 then 'Da Iscrivere' when -1 then 'Non Iscritto' else '' end else '' end as Stato, ruoli.ruoloaccreditamento, "
        strsql = strsql & "isnull(case len(day(entepersonaleruoli.DataFineValidità)) when 1 then '0' + convert(varchar(20),day(entepersonaleruoli.DataFineValidità)) "
        strsql = strsql & "else convert(varchar(20),day(entepersonaleruoli.DataFineValidità))  end + '/' + "
        strsql = strsql & "(case len(month(entepersonaleruoli.DataFineValidità)) when 1 then '0' + convert(varchar(20),month(entepersonaleruoli.DataFineValidità)) "
        strsql = strsql & "else convert(varchar(20),month(entepersonaleruoli.DataFineValidità))  end + '/' + "
        strsql = strsql & "Convert(varchar(20), Year(entepersonaleruoli.DataFineValidità))), '') as DataFineValidità, "
        strsql = strsql & "entepersonaleruoli.DataInizioValidità "
        strsql = strsql & "FROM ruoli LEFT OUTER JOIN "
        strsql = strsql & "entepersonaleruoli ON entepersonaleruoli.IDRuolo = ruoli.IDRuolo AND entepersonaleruoli.IDEntePersonale ='" & identepersonale & "' "
        strsql = strsql & " LEFT JOIN EntePersonale EP ON entepersonaleruoli.IDEntePersonale = EP.Identepersonale"
        strsql = strsql & " LEFT JOIN Enti E ON E.IDEnte = Ep.Idente"
        strsql = strsql & " LEFT JOIN OrdinamentoRuoli O ON Ruoli.IDRuolo =O.IdRuolo"
        strsql = strsql & " WHERE  Ruoli.Nascosto = 0 AND (Ruoli.IdRuolo<>4 or (Ruoli.IdRuolo=4 AND entepersonaleruoli.IdRuolo=4))" 'Escludo il rappresentnte legale
        If Session("TipoUtente") = "E" Then
            strsql = strsql & " AND RuoloAccreditamento=1 "
        End If
        If AlboEnte = "" Then
            strsql &= " AND (ruoli.Albo = 'SCU' OR ruoli.Albo is null)"
        Else
            strsql &= " AND (ruoli.Albo = '" & AlboEnte & "' OR ruoli.Albo is null)"
        End If

        strsql = strsql & " order by isnull(o.Ordinamento,99)"

        dtsRuoli = ClsServer.DataSetGenerico(strsql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))

        dtgRuoli.DataSource = dtsRuoli
        dtgRuoli.DataBind()

        CaricaChek()
        CaricaChekOLP()
        'ColoraCelle() 'Tolto riferimento (viene gestito il colore nel ItemDataBound)

    End Sub
    'carico le check in fase di modifica
    'se è OLP lo disabilito
    Private Sub CaricaChekOLP()
        'realizzato da jonathani 25/01/2006
        Dim item As DataGridItem
        For Each item In dtgRuoli.Items
            Dim check As CheckBox = DirectCast(item.FindControl("chkAssegnaRuolo"), CheckBox)
            If item.Cells(5).Text = False And Session("TipoUtente") = "E" Then
                check.Enabled = False
            End If
        Next
    End Sub

    'carico le check in fase di modifica
    'se c'è la data di inserimento il check è ceccato
    Private Sub CaricaChek()
        'realizzato da jonathani 25/01/2006
        Dim item As DataGridItem
        For Each item In dtgRuoli.Items
            Dim check As CheckBox = DirectCast(item.FindControl("chkAssegnaRuolo"), CheckBox)
            If (CLng(item.Cells(2).Text) <> 0 And item.Cells(6).Text = "&nbsp;") Then
                check.Checked = True
            Else
                check.Checked = False
            End If
        Next
    End Sub

    Private Sub ColoraCelle()
        'Generato da jonathani il 26/01/06
        'VAriazione del Colore secondo lo stato del ruolo.
        'Attiva=Verde;Presentata=gialla;Cancellata=Rossa;Sospesa=
        Dim item As DataGridItem
        Dim color As New System.Drawing.Color
        Dim x As Integer
        Dim txt As String
        Dim blnCheckVis As Boolean = True
        For Each item In dtgRuoli.Items
            For x = 0 To 9
                Select Case item.Cells(4).Text
                    Case "Iscritto"
                        color = System.Drawing.Color.LightGreen
                        dtgRuoli.Items(item.ItemIndex).Cells(x).BackColor = color
                    Case "Da Iscrivere"
                        color = System.Drawing.Color.Khaki
                        dtgRuoli.Items(item.ItemIndex).Cells(x).BackColor = color
                    Case "Non Iscritto"
                        color = System.Drawing.Color.LightSalmon
                        dtgRuoli.Items(item.ItemIndex).Cells(x).BackColor = color
                End Select
                'cancellato
                If item.Cells(6).Text <> "&nbsp;" Then
                    color = System.Drawing.Color.LightSalmon
                    dtgRuoli.Items(item.ItemIndex).Cells(x).BackColor = color
                End If
            Next
        Next
    End Sub

    'controlli precendenti le operazioni di insert o update
    'di ritorno mi aspetto il messaggio d'errore come stringa se qualcosa non 
    'è andato a buon fine, altrimenti l'id del ruolo principale
    Function ControlloIdRuoli(Optional ByVal identepersonale As String = "-1") As String
        'datagrid item che uso per vedere lo stato della check sulla datagrid
        Dim item As DataGridItem
        'variabile che utilizzo per crearmi una stringa contenente tutti gli id ruoli nuovi selezionati separati da ;
        Dim strBloccoIDRUOLINuovi As String = ""
        'variabile che utilizzo per crearmi una stringa contenente tutti gli id ruoli vecchi selezionati separati da ;
        Dim strBloccoIDRUOLIVecchi As String = ""
        If identepersonale <> "" Then
            'ciclo gli item della datagrid per vedere lo stato della check "assegna ruolo"
            For Each item In dtgRuoli.Items
                Dim check As CheckBox = DirectCast(item.FindControl("chkAssegnaRuolo"), CheckBox)
                'controllo se la check è spuntata o meno
                If check.Checked = True Then
                    'il ruolo esisteva
                    If item.Cells(4).Text = "&nbsp;" Then
                        If strBloccoIDRUOLINuovi = "" Then
                            strBloccoIDRUOLINuovi = item.Cells(0).Text() & ";"
                        Else
                            strBloccoIDRUOLINuovi = strBloccoIDRUOLINuovi & item.Cells(0).Text & ";"
                        End If
                    End If
                Else
                    If item.Cells(4).Text <> "&nbsp;" Then
                        If strBloccoIDRUOLIVecchi = "" Then
                            strBloccoIDRUOLIVecchi = item.Cells(0).Text() & ";"
                        Else
                            strBloccoIDRUOLIVecchi = strBloccoIDRUOLIVecchi & item.Cells(0).Text & ";"
                        End If
                    End If
                End If
            Next
        Else
            'ciclo gli item della datagrid per vedere lo stato della check "assegna ruolo"
            For Each item In dtgRuoli.Items
                Dim check As CheckBox = DirectCast(item.FindControl("chkAssegnaRuolo"), CheckBox)
                'controllo se la check è spuntata o meno
                If check.Checked = True Then
                    'controllo se è il primo
                    If strBloccoIDRUOLINuovi = "" Then
                        strBloccoIDRUOLINuovi = item.Cells(0).Text() & ";"
                    Else
                        strBloccoIDRUOLINuovi = strBloccoIDRUOLINuovi & item.Cells(0).Text & ";"
                    End If
                End If
            Next
        End If

        Try
            'eseguo la store
            ControlloIdRuoli = StoreControlliRuoli(identepersonale, strBloccoIDRUOLINuovi, strBloccoIDRUOLIVecchi, Session("TipoUtente"), txtCodiceFiscale.Text)

            Return ControlloIdRuoli

        Catch ex As Exception
            Log.Error(LogEvent.RISORSE_ENTE_ERRORE, "Controllo Ruoli")
            ControlloIdRuoli = "Errore durante l'esecuzione della store. Contattare l'assistenza tecnica."
            Return ControlloIdRuoli
        End Try

    End Function

    Private Function StoreControlliRuoli(ByVal strIdEntePersonale As String, ByVal strIdNuoviRuoli As String, ByVal strIdVecchiRuoli As String, ByVal strTipoUtente As String, ByVal strCodiceFiscale As String) As String
        Dim CustOrderHist As SqlClient.SqlCommand
        CustOrderHist = New SqlClient.SqlCommand
        CustOrderHist.CommandType = CommandType.StoredProcedure
        CustOrderHist.CommandText = "SP_CONTROLLA_RUOLI"
        CustOrderHist.Connection = IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn"))

        '@IdEntePersonale int, 
        '@NuoviRuoli nvarchar(50),
        '@VecchiRuoli nvarchar(50),
        '@TipoUtente nvarchar(5),
        '@Messaggio nvarchar(1000) Output

        Dim paramIdEntePersonale As SqlClient.SqlParameter
        paramIdEntePersonale = New SqlClient.SqlParameter
        paramIdEntePersonale.ParameterName = "@IdEntePersonale"
        paramIdEntePersonale.SqlDbType = SqlDbType.Int
        CustOrderHist.Parameters.Add(paramIdEntePersonale)

        Dim paramIDRuoliNuovi As SqlClient.SqlParameter
        paramIDRuoliNuovi = New SqlClient.SqlParameter
        paramIDRuoliNuovi.ParameterName = "@NuoviRuoli"
        paramIDRuoliNuovi.SqlDbType = SqlDbType.NVarChar
        CustOrderHist.Parameters.Add(paramIDRuoliNuovi)

        Dim paramIDRuoliVecchi As SqlClient.SqlParameter
        paramIDRuoliVecchi = New SqlClient.SqlParameter
        paramIDRuoliVecchi.ParameterName = "@VecchiRuoli"
        paramIDRuoliVecchi.SqlDbType = SqlDbType.NVarChar
        CustOrderHist.Parameters.Add(paramIDRuoliVecchi)

        Dim paramTipologiaUtente As SqlClient.SqlParameter
        paramTipologiaUtente = New SqlClient.SqlParameter
        paramTipologiaUtente.ParameterName = "@TipoUtente"
        paramTipologiaUtente.SqlDbType = SqlDbType.NVarChar
        CustOrderHist.Parameters.Add(paramTipologiaUtente)

        Dim paramCodiceFiscale As SqlClient.SqlParameter
        paramCodiceFiscale = New SqlClient.SqlParameter
        paramCodiceFiscale.ParameterName = "@CodiceFiscale"
        paramCodiceFiscale.Size = 20
        paramCodiceFiscale.SqlDbType = SqlDbType.NVarChar
        CustOrderHist.Parameters.Add(paramCodiceFiscale)

        Dim paramIdEnte As SqlClient.SqlParameter
        paramIdEnte = New SqlClient.SqlParameter
        paramIdEnte.ParameterName = "@IdEnte"
        paramIdEnte.Size = 20
        paramIdEnte.SqlDbType = SqlDbType.NVarChar
        CustOrderHist.Parameters.Add(paramIdEnte)

        Dim paramMessaggio As SqlClient.SqlParameter
        paramMessaggio = New SqlClient.SqlParameter
        paramMessaggio.ParameterName = "@Messaggio"
        paramMessaggio.Size = 1000
        paramMessaggio.SqlDbType = SqlDbType.NVarChar
        paramMessaggio.Direction = ParameterDirection.Output
        CustOrderHist.Parameters.Add(paramMessaggio)



        'Dim paramMessaggioErrore As SqlClient.SqlParameter
        'paramMessaggioErrore = New SqlClient.SqlParameter
        'paramMessaggioErrore.ParameterName = "@MessaggioErrore"
        'paramMessaggioErrore.SqlDbType = SqlDbType.NVarChar
        'paramMessaggioErrore.Direction = ParameterDirection.Output
        'CustOrderHist.Parameters.Add(paramMessaggioErrore)


        Dim Reader As SqlClient.SqlDataReader
        CustOrderHist.Parameters("@IdEntePersonale").Value = strIdEntePersonale
        CustOrderHist.Parameters("@NuoviRuoli").Value = strIdNuoviRuoli
        CustOrderHist.Parameters("@VecchiRuoli").Value = strIdVecchiRuoli
        CustOrderHist.Parameters("@TipoUtente").Value = strTipoUtente
        CustOrderHist.Parameters("@CodiceFiscale").Value = strCodiceFiscale
        CustOrderHist.Parameters("@IdEnte").Value = Session("IdEnte")

        Reader = CustOrderHist.ExecuteReader()
        ' Insert code to read through the datareader.
        'If CustOrderHist.Parameters("@Valore").Value = 0 Then
        StoreControlliRuoli = CustOrderHist.Parameters("@Messaggio").Value
        If Not Reader Is Nothing Then
            Reader.Close()
            Reader = Nothing
        End If
        'End If
        'connessione.Close()
    End Function

    Private Sub dtgRuoli_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgRuoli.ItemDataBound

        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then

            Dim ImgDettagliRisorsa As ImageButton = DirectCast(e.Item.FindControl("ImgDettagliRisorsa"), ImageButton)

            Dim idEntePersonaleRuolo As Integer
            Dim dataFineValidita As Object
            Dim idRuolo As Integer


            idEntePersonaleRuolo = IIf(IsDBNull(e.Item.DataItem("identepersonaleruolo")), 0, CInt(e.Item.DataItem("identepersonaleruolo")))
            dataFineValidita = IIf(IsDBNull(e.Item.DataItem("datafinevalidità")), String.Empty, e.Item.DataItem("datafinevalidità"))
            idRuolo = IIf(IsDBNull(e.Item.DataItem("idRuolo")), 0, CInt(e.Item.DataItem("idRuolo")))

            If (idEntePersonaleRuolo <> 0 And dataFineValidita.ToString = String.Empty) Then
                ImgDettagliRisorsa.Visible = True
            Else
                ImgDettagliRisorsa.Visible = False
            End If

            If idRuolo = 4 Then
                e.Item.BackColor = Color.LightGray
                Dim chkAssegnaRuolo As CheckBox = DirectCast(e.Item.FindControl("chkAssegnaRuolo"), CheckBox)
                chkAssegnaRuolo.Enabled = False
                txtCodiceFiscale.ReadOnly = True
                txtCodiceFiscale.Enabled = False
            Else

                Select Case e.Item.Cells(4).Text
                    Case "Iscritto"
                        e.Item.BackColor = Color.LightGreen
                    Case "Da Iscrivere"
                        e.Item.BackColor = Color.Khaki
                    Case "Non Iscritto"
                        e.Item.BackColor = Color.LightSalmon
                End Select
                'cancellato
                If e.Item.Cells(6).Text <> "&nbsp;" Then
                    e.Item.BackColor = Color.LightSalmon
                End If
            End If


        End If

    End Sub

    Private Sub ChkEsteroNascita_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ChkEsteroNascita.CheckedChanged
        Dim SelComune As New clsSelezionaComune
        Dim blnEstero As Boolean = ChkEsteroNascita.Checked
        SelComune.CaricaProvinciaNazione(ddlProvinciaNascita, blnEstero, Session("Conn"))
        ddlComuneNascita.Items.Clear()
    End Sub

    Private Sub ChkEsteroResidenza_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ChkEsteroResidenza.CheckedChanged
        Dim SelComune As New clsSelezionaComune
        Dim blnEstero As Boolean = ChkEsteroResidenza.Checked
        SelComune.CaricaProvinciaNazione(ddlProvinciaResidenza, blnEstero, Session("Conn"))
        ddlComuneResidenza.Items.Clear()
    End Sub

    Private Sub cmdAllega_Click(sender As Object, e As EventArgs) Handles cmdAllega.Click
        'Verifica se è stato inserito il file
        If fileCV.PostedFile Is Nothing Or String.IsNullOrEmpty(fileCV.PostedFile.FileName) Then
            MessaggiPopup("Non è stato scelto nessun file per il caricamento del Curriculum Vitae")
            Exit Sub
        End If
        'Controllo Tipo File
        If clsGestioneDocumentiAccreditamento.VerificaEstensioneFileAccreditamento(fileCV) = False Then
            MessaggiPopup("Il formato file del Curriculum Vitae non è corretto. È possibile associare documenti nel formato .PDF o .PDF.P7M")
            Exit Sub
        End If
        'Controlli dimensioni del file
        Dim fs = fileCV.PostedFile.InputStream
        Dim iLen As Integer = CInt(fs.Length)
        If iLen <= 0 Then
            MessaggiPopup("Attenzione. Impossibile caricare un Curriculum Vitae vuoto.")
            Exit Sub
        End If
        If iLen > 20971520 Then
            MessaggiPopup("Attenzione. La dimensione massima del Curriculum Vitae è di 20 MB.")
            Exit Sub
        End If
        Dim bBLOBStorage = ClsServer.StreamToByte(fs)

        fs.Close()

        Dim hashValue As String
        hashValue = GeneraHash(bBLOBStorage)

        Dim estensione As String = System.IO.Path.GetExtension(fileCV.PostedFile.FileName)
        If UCase(estensione) = ".P7M" Then estensione = ".pdf.p7m"

        'Salvo File In Sessione
        Dim CV As New Allegato() With {
         .Id = Session("LoadCVId"),
         .Updated = True,
         .Blob = bBLOBStorage,
         .Filename = "CV_" & txtCodiceFiscale.Text & estensione,
         .Hash = hashValue,
         .Filesize = iLen,
         .DataInserimento = Date.Now
        }
        Session("LoadedCV") = CV
        'Se Il CV è caricato in Sessione (Inserimento)
        rowNoCV.Visible = False
        rowCV.Visible = True
        txtCVFilename.Text = CV.Filename
        txtCVHash.Text = CV.Hash
        txtCVData.Text = CV.DataInserimento.ToString("dd/MM/yyyy HH:mm:ss")
        MessaggiSuccess("Curriculum Vitae Caricato Correttamente")
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

    Private Sub ClearSessionCV()
        Session("LoadedCV") = Nothing
        rowNoCV.Visible = True
        rowCV.Visible = False
    End Sub


    Private Sub btnEliminaCV_Click(sender As Object, e As ImageClickEventArgs) Handles btnEliminaCV.Click
        ClearSessionCV()

    End Sub

    Private Sub btnDownloadCV_Click(sender As Object, e As ImageClickEventArgs) Handles btnDownloadCV.Click
        If Session("LoadedCV") Is Nothing Then
            MessaggiAlert("Nessun File caricato")
            ClearSessionCV()
            Log.Warning(LogEvent.RISORSE_ENTE_DOWNLOAD_CV, "Nessun file Caricato")
            Exit Sub
        End If
        Log.Information(LogEvent.RISORSE_ENTE_DOWNLOAD_CV)
        Dim CV As Allegato = Session("LoadedCV")
        Response.Clear()
        Response.ContentType = "Application/pdf"
        Response.AddHeader("Content-Disposition", "attachment; filename=" & CV.Filename)
        Response.BinaryWrite(CV.Blob)
        Response.End()
    End Sub

    Private Function InserisciAllegati(trans As SqlClient.SqlTransaction, ByRef IDAllegato As Integer, allegato As Allegato, ByRef erroreRitorno As String, Optional ByVal IdEnteFiglio As Integer = 0, Optional TipoOperazione As String = "") As Boolean
        Dim SqlCmd As New SqlClient.SqlCommand()
        LogParametri.Clear()
        Try
            'Prendo gli allegati salvati in sessione

            SqlCmd.CommandText = "sp_InsAllegato"
            SqlCmd.CommandType = CommandType.StoredProcedure
            SqlCmd.Transaction = trans
            SqlCmd.Connection = Session("Conn")
            SqlCmd.Parameters.Clear()

            SqlCmd.Parameters.Add("@IdEnte", SqlDbType.Int).Value = IdEnteFiglio
            LogParametri.Add("@IdEnte", IdEnteFiglio)
            'Tipo allegato Atto di nomina 

            SqlCmd.Parameters.Add("@IdTipoAllegato", SqlDbType.Int).Value = allegato.IdTipoAllegato
            LogParametri.Add("@IdTipoAllegato", allegato.IdTipoAllegato)
            'Blob
            SqlCmd.Parameters.Add("@BinData", SqlDbType.VarBinary, -1).Value = allegato.Blob
            LogParametri.Add("@BinData", "BlobAllegato")
            'FileName
            SqlCmd.Parameters.Add("@FileName", SqlDbType.VarChar, 255).Value = allegato.Filename
            LogParametri.Add("@FileName", allegato.Filename)
            'Hash
            SqlCmd.Parameters.Add("@Hash", SqlDbType.VarChar, 100).Value = allegato.Hash
            LogParametri.Add("@Hash", allegato.Hash)
            'FileLength
            SqlCmd.Parameters.Add("@FileLength", SqlDbType.BigInt, 100).Value = allegato.Filesize
            LogParametri.Add("@FileLength", allegato.Filesize)

            'DataInserimento
            SqlCmd.Parameters.Add("@DataIns", SqlDbType.DateTime, 100).Value = allegato.DataInserimento
            LogParametri.Add("@DataIns", allegato.DataInserimento)

            'UserNAme
            SqlCmd.Parameters.Add("@UserName", SqlDbType.VarChar, 20).Value = Session("Utente").ToString
            LogParametri.Add("@UserName", Session("Utente").ToString)

            If TipoOperazione <> String.Empty Then
                SqlCmd.Parameters.Add("@TipoOperazione", SqlDbType.Char, 1).Value = TipoOperazione
                LogParametri.Add("@TipoOperazione", TipoOperazione)
            Else
                SqlCmd.Parameters.Add("@TipoOperazione", SqlDbType.Char, 1).Value = DBNull.Value
                LogParametri.Add("@TipoOperazione", String.Empty)
            End If

            If Not Session("IdEnteFaseArt") Is Nothing Then
                'IdEnteFaseDestinazione
                SqlCmd.Parameters.Add("@IdEnteFaseDestinazione", SqlDbType.Int).Value = Session("IdEnteFaseArt").ToString
                LogParametri.Add("@IdEnteFaseDestinazione", Session("IdEnteFaseArt").ToString)
            End If

            SqlCmd.Parameters.Add("@IdAllegato", SqlDbType.Int)
            SqlCmd.Parameters("@IdAllegato").Direction = ParameterDirection.Output

            'Esito aggiornamento: 0-Errore 1-Aggiornamento effettuato
            SqlCmd.Parameters.Add("@Esito", SqlDbType.TinyInt)
            SqlCmd.Parameters("@Esito").Direction = ParameterDirection.Output

            SqlCmd.Parameters.Add("@messaggio", SqlDbType.VarChar)
            SqlCmd.Parameters("@messaggio").Size = 1000
            SqlCmd.Parameters("@messaggio").Direction = ParameterDirection.Output

            SqlCmd.ExecuteNonQuery()
            If (SqlCmd.Parameters("@Esito").Value()) <> 0 Then ' inseriemento ok
                IDAllegato = CInt(SqlCmd.Parameters("@IdAllegato").Value().ToString())
                Dim entita As New Logger.Data.Entity
                entita.Id = IDAllegato
                Select Case allegato.IdTipoAllegato
                    Case 4
                        entita.Name = "CV_PERSONALE"
                End Select
                Log.Information(Logger.Data.LogEvent.RISORSE_ENTE_ART2_MODIFICA_CORRETTA, SqlCmd.Parameters("@messaggio").Value, LogParametri, entita)
                InserisciAllegati = True
                Exit Function
            Else
                trans.Rollback()
                erroreRitorno = SqlCmd.Parameters("@messaggio").Value()
                Log.Warning(Logger.Data.LogEvent.RISORSE_ENTE_ART2_MODIFICA_ANOMALIE, SqlCmd.Parameters("@messaggio").Value, LogParametri)
                InserisciAllegati = False
                Exit Function
            End If
        Catch ex As Exception
            trans.Rollback()
            erroreRitorno = ex.Message
            Log.Error(Logger.Data.LogEvent.RISORSE_ENTE_ART2_MODIFICA_ERRATA, ex.Message, ex, LogParametri)
            InserisciAllegati = False
            Exit Function
        End Try

    End Function

    Private Function ConfrontaAllegati(ListaAllegati As List(Of Allegato), allegatoSessione As Allegato, ByRef idAllegato As Integer) As Boolean
        Dim allegatoTrovato As Allegato

        allegatoTrovato = ListaAllegati.Find(Function(l) l.IdTipoAllegato = allegatoSessione.IdTipoAllegato)

        If allegatoTrovato IsNot Nothing Then
            If allegatoTrovato.Hash = allegatoSessione.Hash Then
                idAllegato = allegatoTrovato.Id
                ConfrontaAllegati = True
                Exit Function
            End If
        End If

        idAllegato = 0

    End Function

    Protected Sub cmdSalvaDoc_Click(sender As Object, e As EventArgs) Handles cmdSalvaDoc.Click
        lblErr.Text = ""
        lblErr.Visible = False

        Try
            Dim CV As Allegato = Session("LoadedCV")

            If CV IsNot Nothing Then
                'CV.Filename = "CV_" & txtCodiceFiscale.Text & System.IO.Path.GetExtension(CV.Filename)
                Session("LoadedCV") = CV
                txtCVFilename.Text = CV.Filename
            Else
                MessaggiAlert("È necessario inserire il Curriculum Vitae")
                Exit Sub
            End If

            Dim idEntePersonale As Integer? = Request.QueryString("intPersonaleEnteAssociato")
            If idEntePersonale Is Nothing Then
                MessaggiAlert("Risorsa non trovata")
                Exit Sub
            End If

            If Not CV.Updated Then
                MessaggiAlert("Il CV non risulta modificato. Salvataggio non effettuato.")
                Exit Sub
            End If

            Dim sqlControlloHash As String = "SELECT HashValue FROM entidocumenti A JOIN entepersonale P ON P.IdAllegatoCV = A.IdEnteDocumento and IdEntePersonale=" & idEntePersonale
            Dim commandControlloHash As New SqlCommand(sqlControlloHash, Session("conn"))
            Dim strHash As String = commandControlloHash.ExecuteScalar
            If Not String.IsNullOrEmpty(strHash) AndAlso strHash = CV.Hash Then
                MessaggiAlert("Il CV inserito risulta uguale al precedente. Salvataggio non effettuato.")
                Exit Sub
            End If

            Dim sqlControlloCV As String = "SELECT COUNT(*) FROM entidocumenti A JOIN entepersonale P ON P.IdAllegatoCV = A.IdEnteDocumento "
            sqlControlloCV += "WHERE HashValue=@Hash "
            If idEntePersonale.HasValue Then
                sqlControlloCV = sqlControlloCV + " AND IdEntePersonale<>@IdEntePersonale"
            End If
            Dim commandControlloCV As New SqlCommand(sqlControlloCV, Session("conn"))
            commandControlloCV.Parameters.AddWithValue("@IdEnte", CInt(Session("idEnte")))
            commandControlloCV.Parameters.AddWithValue("@IdEntePersonale", idEntePersonale)
            commandControlloCV.Parameters.AddWithValue("@Hash", CV.Hash)
            Dim numeroAllegati As Integer = commandControlloCV.ExecuteScalar
            If numeroAllegati > 0 Then
                MessaggiAlert("Il Curriculum Vitae inserito è stato già associato ad un altro soggetto")
                rowAllegato.Style.Add("background-color", "coral")
                Exit Sub
            End If

            MyTransaction = CType(Session("Conn"), System.Data.SqlClient.SqlConnection).BeginTransaction(Session("IdEnte") & "_" & Session("Utente"))

            Dim IDAllegatoCV As Integer
            Dim erroreRitorno As String

            CV.IdTipoAllegato = 4 'tipoallegato cv

            If Not (InserisciAllegati(MyTransaction, IDAllegatoCV, CV, erroreRitorno)) Then
                MyTransaction = Nothing
                MessaggiAlert(erroreRitorno)
                Exit Sub
            End If

            Dim sqlUpdateCV As String = "Update entepersonale set IdAllegatoCV=" & IDAllegatoCV & " where identepersonale=" & idEntePersonale
            Dim commandUpdateCV As New SqlCommand(sqlUpdateCV, Session("conn"), MyTransaction)
            commandUpdateCV.ExecuteNonQuery()

            MessaggiSuccess("CV modificato con successo.")
            MyTransaction.Commit()
        Catch ex As Exception
            MyTransaction = Nothing
            MessaggiAlert(ex.Message)
        End Try
    End Sub

    Protected Sub ChkAssegnaRuoloChange(sender As Object, e As EventArgs)
        Dim gr As DataGridItem              'riga
        Dim chkBox As CheckBox
        Dim IdEntePersonaleRuolo As Integer = 0
        Dim IdRuolo As Integer = 0
        Dim isIscritto As Boolean = False

        chkBox = CType(sender, CheckBox)
        gr = chkBox.Parent.Parent           'ricavo la riga

        IdEntePersonaleRuolo = Integer.Parse(gr.Cells(2).Text)

        If chkBox.Checked = False Then                                  'se è passato a false (è stato deselezionato)

            If IsRuoloResponsabileIscritto(IdEntePersonaleRuolo) Then   'se è un ruolo di responsabile ed è accreditato=ISCRITTO

                If EsisteCambioStrutturaGestioneNonValutato() Then
                    'impedisco l'eliminazione con popup
                    chkBox.Checked = True
                    alertTitolo.InnerText = "Impossibile cancellare il ruolo"
                    lblAlert.Text = "E' già in corso di valutazione un cambio della Struttura di Gestione.<br>E' eventualmente possibile richiedere via PEC al Dipartimento la rinuncia all'Adeguamento precedentemente presentato e non ancora valutato."
                    mpeALERT.Show()
                Else
                    alertTitolo.InnerText = "Informazione"
                    lblAlert.Text = "Si ricorda che in caso di modifica di un responsabile è necessario caricare il documento relativo alla nomina della nuova Struttura di Gestione nella ""Sezione Ente Titolare""."
                    mpeALERT.Show()
                End If

            End If

        End If

    End Sub

    Private Function HasRuoloResponsabileIscritto(IdEntePersonale) As Boolean

        Dim SqlCmd As New SqlClient.SqlCommand

        SqlCmd.CommandText = "SP_CONTROLLO_PERSONALE_RUOLO_RESPONSABILE_ISCRITTO"
        SqlCmd.CommandType = CommandType.StoredProcedure
        SqlCmd.Connection = Session("Conn")
        SqlCmd.Parameters.Add("@IdEntePersonale", SqlDbType.Int).Value = IdEntePersonale

        Dim Esito As SqlClient.SqlParameter
        Esito = New SqlClient.SqlParameter
        Esito.ParameterName = "@Esito"
        'sparam1.Size = 100
        Esito.SqlDbType = SqlDbType.Int
        Esito.Direction = ParameterDirection.Output
        SqlCmd.Parameters.Add(Esito)

        SqlCmd.ExecuteNonQuery()

        If SqlCmd.Parameters("@Esito").Value = 1 Then Return True Else Return False

    End Function

    Private Function IsRuoloResponsabileIscritto(IdEntePersonaleRuolo) As Boolean

        Dim SqlCmd As New SqlClient.SqlCommand

        SqlCmd.CommandText = "SP_CONTROLLO_RUOLO_RESPONSABILE_ISCRITTO"
        SqlCmd.CommandType = CommandType.StoredProcedure
        SqlCmd.Connection = Session("Conn")
        SqlCmd.Parameters.Add("@IdEntePersonaleRuolo", SqlDbType.Int).Value = IdEntePersonaleRuolo

        Dim Esito As SqlClient.SqlParameter
        Esito = New SqlClient.SqlParameter
        Esito.ParameterName = "@Esito"
        'sparam1.Size = 100
        Esito.SqlDbType = SqlDbType.Int
        Esito.Direction = ParameterDirection.Output
        SqlCmd.Parameters.Add(Esito)

        SqlCmd.ExecuteNonQuery()

        If SqlCmd.Parameters("@Esito").Value = 1 Then Return True Else Return False

    End Function

    Private Function EsisteCambioStrutturaGestioneNonValutato() As Boolean

        Dim SqlCmd As New SqlClient.SqlCommand

        SqlCmd.CommandText = "SP_CONTROLLO_CAMBIO_STRUTTURA_GESTIONE_NON_VALUTATO"
        SqlCmd.CommandType = CommandType.StoredProcedure
        SqlCmd.Connection = Session("Conn")
        SqlCmd.Parameters.Add("@IdEnte", SqlDbType.Int).Value = Integer.Parse(Session("IdEnte"))

        Dim Esito As SqlClient.SqlParameter
        Esito = New SqlClient.SqlParameter
        Esito.ParameterName = "@Esito"
        'sparam1.Size = 100
        Esito.SqlDbType = SqlDbType.Int
        Esito.Direction = ParameterDirection.Output
        SqlCmd.Parameters.Add(Esito)

        SqlCmd.ExecuteNonQuery()

        If SqlCmd.Parameters("@Esito").Value = 0 Then Return True Else Return False

    End Function

End Class