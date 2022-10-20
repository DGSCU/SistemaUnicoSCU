Imports System.Data.SqlClient
Imports System.Drawing
Imports System.Linq
Imports System.Security.Cryptography
Imports System.Collections.Generic

Public Class WfrmAnagraficaEnte
    Inherits SmartPage
    Dim strsql As String
    Dim dtrgenerico As SqlClient.SqlDataReader
    Dim dtsGenerico As DataSet
    Dim bandiera As Integer
    Dim cmdGenerico As SqlClient.SqlCommand
    Dim selComune As New clsSelezionaComune
    Dim helper As Helper = New Helper()
    Shared isVariazione As Boolean
    Shared flgForzaVariazione As Boolean
    Protected Const ANNIPRECEDENTI As Integer = 3
    Protected AnniCaricamento(2) As String
    Protected SettoriEsperienzeDB As New Dictionary(Of String, String)
    Dim pLog As Hashtable


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim AlboEnte As String
        If Not Session("LogIn") Is Nothing Then
            If Session("LogIn") = False Then
                Response.Redirect("LogOn.aspx")
            End If
        Else
            Response.Redirect("LogOn.aspx")
        End If

        checkSpid()
        'Pulizia dati
        lblErroreAttoCostitutivo.Text = ""
        lblErroreUploadDelibera.Text = ""
        lblErroreStatuto.Text = ""
        lblErroreDeliberaAdesione.Text = ""
        lblErroreImpegnoEtico.Text = ""
        lblErroreAttoNomina.Text = ""
        If IsPostBack = False Then
            Dim SelComune As New clsSelezionaComune
            Dim blnEstero As Boolean = False
            SelComune.CaricaProvinciaNazione(ddlProvincia, blnEstero, Session("Conn"))
            Session("EsperienzeAreeSettore") = Nothing
            Session("IDClasseAccreditamento") = Nothing
            Session("LoadedAN") = Nothing
            Session("LoadedStatuto") = Nothing
            Session("LoadedAC") = Nothing
            Session("LoadedDeliberaAdesione") = Nothing
            Session("LoadedDelibera") = Nothing
            Session("IdEnteFaseArt") = Nothing
            Session("ElencoSettoriDB") = Nothing

            lblMessaggio.Text = ""


            AlboEnte = ClsUtility.TrovaAlboEnte(Session("IdEnte"), Session("Conn"))
            CaricaCombo(AlboEnte)
            CaricaSettori(AlboEnte)
            LoadMaschera()
            GestisciStyleDatiModificati()


            'CONTROLLI SPECIFICI A SECONDO DELL'UTENTE (E/U/R)
            'Aggiunto da Alessandra Taballione il 15.02.2005
            'Controllo se ha una relazione con ente Padre o come figlio
            strsql = "select enti.denominazione as entepadre,tipirelazioni.tiporelazione from entirelazioni " &
            " inner join enti on enti.idente=entirelazioni.identepadre " &
            " inner join TipiRelazioni on TipiRelazioni.idTipoRelazione=EntiRelazioni.idtiporelazione " &
            " where identeFiglio=" & Session("IdEnte") & " and datafineValidità is null "
            If Not dtrgenerico Is Nothing Then
                dtrgenerico.Close()
                dtrgenerico = Nothing
            End If
            dtrgenerico = ClsServer.CreaDatareader(strsql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
            If dtrgenerico.HasRows = True Then
                dtrgenerico.Read()
                lblaccordo.Visible = True
                imgAccordo.Visible = True
            End If
            If Not dtrgenerico Is Nothing Then
                dtrgenerico.Close()
                dtrgenerico = Nothing
            End If

            'modificato il 27/09/2013 da s.c.
            'stato ente ; ATTIVO o ADEGUAMENTO  e anagrafica modificabile, la PEC  non deve essere modificabile.La firma modificabile se non indicata.
            'stato ente ; REGISTRATO o ISTRUTTORIA e anagrafica modificabile, PEC e FIRMA devono essere editabile e non obbligatori
            If Session("TipoUtente") = "E" Then
                If lblStato.Text = "Attivo" Or lblStato.Text = "In Adeguamento" Then
                    'txtEmailpec.ReadOnly = False
                    'txtEmailpec.BackColor = Color.White
                    If ChkFirma.Checked = False Then
                        ChkFirma.Enabled = True
                    End If
                ElseIf lblStato.Text = "Registrato" Or lblStato.Text = "Istruttoria" Then
                    'txtEmailpec.ReadOnly = False
                    'txtEmailpec.BackColor = Color.White
                    'ChkFirma.Enabled = True
                End If
            End If


            'Controllo Tasto Rescissione
            strsql = "Select * From RescissioniAccordi Where IdEnte = '" & Session("IdEnte") & "'"
            dtrgenerico = ClsServer.CreaDatareader(strsql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
            If dtrgenerico.HasRows = True Then
                CmdRescissione.Visible = True
            End If
            dtrgenerico.Close()
            dtrgenerico = Nothing

            If ddlTipologia.SelectedValue = "Privato" Then
                rowNoDeliberaAdesione.Visible = False
                rowDeliberaAdesione.Visible = False
            End If
        Else

        End If

        'Richiamo i controlli Formali lato Client sul Salvataggio
        'Controlli email 
        chkVeroEmail.Attributes("onclick") = "javascript:ControlloCheckedVero()"
        chkFalsoEmail.Attributes("onclick") = "javascript:ControlloCheckedFalso()"
        cmdEmail.Attributes("onclick") = "javascript:ControlloEmailOrdinaria()"
        'Controlli chek http 
        chkFalsohttp.Attributes("onclick") = "javascript:ControlloCheckedFalsoHttp()"
        chkVerohttp.Attributes("onclick") = "javascript:ControlloCheckedVeroHttp()"
        cmdHttp.Attributes("onclick") = "javascript:Openhttp()"

        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If

        'If (lblStato.Text = "Attivo" Or lblStato.Text = "Istruttoria" Or lblStato.Text = "In Adeguamento") Then
        txtdenominazione.ReadOnly = True
        txtdenominazione.BackColor = Color.Gainsboro
        txtCodFis.ReadOnly = True
        txtCodFis.BackColor = Color.Gainsboro
        txtUtenza.ReadOnly = True
        txtUtenza.BackColor = Color.Gainsboro

        AlboEnte = ClsUtility.TrovaAlboEnte(Session("IdEnte"), Session("Conn"))
        If AlboEnte = "SCN" Then
            If ddlTipologia.SelectedItem.Text = "Pubblico" Then
                ddlGiuridica.Visible = True
                Label6.Visible = True
                Response.Write("<input type=hidden name=chkGiuridica value=""true"">")
            Else
                ddlGiuridica.Visible = False
                Label6.Visible = False
                Response.Write("<input type=hidden name=chkGiuridica value=""false"">")
            End If
        End If
        If AlboEnte = "SCU" Then
            If ddlTipologia.SelectedItem IsNot Nothing _
                And ddlTipologia.SelectedItem.Text <> "" Then
                ddlGiuridica.Visible = True
                Label6.Visible = True
                Response.Write("<input type=hidden name=chkGiuridica value=""true"">")
            Else
                Label6.Visible = False
                ddlGiuridica.Visible = False
            End If
        End If

        If ddlGiuridica.SelectedItem IsNot Nothing AndAlso ddlGiuridica.SelectedItem.Text Like "Altro*" Or
            ddlGiuridica.SelectedItem IsNot Nothing AndAlso ddlGiuridica.SelectedItem.Text = "Altro ente" Then
            divAltraTipologia.Visible = True
        Else
            divAltraTipologia.Visible = False
        End If

        'FZ controllo per disabilitare la maschera nel caso sia un'"R" che sta 
        'controllando i progetti di un' ente che nn ha la stessa regiojneee di competenza
        If ClsUtility.ControlloRegioneCompetenza(Session("TipoUtente"), Session("idEnte"), Session("Utente"), Session("IdStatoEnte"), IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn"))) = False Then
            BloccaMaschera("Attenzione, l'ente non è di propria competenza. Impossibile effettuare modifiche.")
        End If

        If Session("TipoUtente") <> "U" Then
            CmdInforScu1.Visible = False
        End If
    End Sub

    Private Sub ddlProvincia_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlProvincia.SelectedIndexChanged
        Dim SelComune As New clsSelezionaComune

        SelComune.CaricaComuni(ddlComune, ddlProvincia.SelectedValue, Session("Conn"))

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

    Private Sub ChkEstero_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ChkEstero.CheckedChanged
        Dim SelComune As New clsSelezionaComune
        Dim blnEstero As Boolean = ChkEstero.Checked
        SelComune.CaricaProvinciaNazione(ddlProvincia, blnEstero, Session("Conn"))
        ddlComune.Items.Clear()

        If blnEstero = True Then
            lblComune.Text = "Località"
            lblCAP.Text = "Codice località"
        Else
            lblComune.Text = "Comune"
            lblCAP.Text = "CAP"
        End If


    End Sub

    Private Sub imgCap_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgCap.Click
        Dim selCap As New clsSelezionaComune
        txtCAP.Text = selCap.RitornaCap(ddlComune.SelectedValue, txtIndirizzo.Text.Trim, txtCivico.Text.Trim, Session("conn"))
    End Sub

    Sub CaricaCombo(ByVal AlboEnte As String)
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
        strsql = "select idstatoente from enti where idente=" & Session("IdEnte")
        dtrgenerico = ClsServer.CreaDatareader(strsql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
        Dim TipoEnte As String
        dtrgenerico.Read()
        TipoEnte = dtrgenerico("IdStatoEnte")

        ddlTipologia.Items.Add("")
        ddlTipologia.Items.Add("Pubblico")
        ddlTipologia.Items.Add("Privato")
        If AlboEnte = "SCN" Then
            'Caricamento delle combo
            If TipoEnte = "6" Or TipoEnte = "8" Then 'ente registrato o istruttoria
                strsql = "select '' descrizione , 0 idtipologieenti union select descrizione, idtipologieenti from tipologieenti where privato = 0 and abilitata=1 order by idtipologieenti"
            Else
                strsql = "select '' descrizione , 0 idtipologieenti union select descrizione, idtipologieenti from tipologieenti where privato = 0 order by idtipologieenti"
            End If
            If Not dtrgenerico Is Nothing Then
                dtrgenerico.Close()
                dtrgenerico = Nothing
            End If
            dtrgenerico = ClsServer.CreaDatareader(strsql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))

            ddlGiuridica.DataSource = dtrgenerico
            ddlGiuridica.DataValueField = "idtipologieenti"
            ddlGiuridica.DataTextField = "Descrizione"
            ddlGiuridica.DataBind()
        End If

        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
        'modificato il 18/07/2017 da Simona Cordella CLASSI/ALBO ENTE

        strsql = "SELECT '0' AS IDSEZIONE,'Seleziona' AS Sezione "
        strsql &= "FROM SezioniAlboSCU "
        strsql &= "UNION "
        strsql &= "SELECT S.IdSezione , S.Sezione AS Sezione "
        strsql &= "FROM SezioniAlboSCU S JOIN classiaccreditamento C "
        strsql &= "ON S.IdClasseAccreditamento = C.IDClasseAccreditamento "
        strsql &= "WHERE(C.MINSEDI > 0) "

        'strsql = " SELECT '0' as idclasseAccreditamento,'Seleziona' as classeAccreditamento  from ClasseRegioneAccreditamento "
        'strsql &= " UNION "
        'strsql &= " SELECT idclasseAccreditamento,classeAccreditamento "
        'strsql &= " FROM ClasseRegioneAccreditamento  "
        'strsql &= " WHERE minsedi > 0 "




        If AlboEnte = "" Then
            strsql &= " and (C.Albo = 'SCU' OR C.Albo is null) "
        Else
            strsql &= " and (C.Albo = '" & AlboEnte & "' OR C.Albo is null) "
        End If

        strsql &= "ORDER BY IDSEZIONE"


        dtrgenerico = ClsServer.CreaDatareader(strsql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
        ddlClassi.DataSource = dtrgenerico
        ddlClassi.DataValueField = "IDSEZIONE"
        ddlClassi.DataTextField = "Sezione"
        ddlClassi.DataBind()
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
    End Sub

    Sub CaricaSettori(ByVal AlboEnte As String)
        'modificato il 18/07/2017 da Simona Cordella SETTORI/ALBO ENTE
        'aggiunto il 19/08/2014 da Simona Cordella
        Dim dtsSettori As DataSet
        Dim strSql As String

        strSql = "select IDMacroAmbitoAttività, Codifica, Codifica + ' - ' + MacroAmbitoAttività as MacroAmbitoAttività, IDIperAmbitoAttività FROM macroambitiattività"
        If AlboEnte = "" Then
            strSql &= " WHERE (ALBO='SCU' OR ALBO IS NULL) "
        Else
            strSql &= " WHERE (ALBO='" & AlboEnte & "' OR ALBO IS NULL) "
        End If
        strSql &= " order by Codifica"
        dtsSettori = ClsServer.DataSetGenerico(strSql, Session("conn"))
        'controllo se ci sono dei record
        If dtsSettori.Tables(0).Rows.Count > 0 Then
            dtgSettori.DataSource = dtsSettori
            dtgSettori.DataBind()
        End If
    End Sub

    Private Sub LoadMaschera()

        Dim abilitato As Integer
        Dim dati As Integer
        Dim annullamodifica As Integer
        Dim visualizzadatiaccreditati As Integer
        Dim annullacancellazione As Integer
        Dim messaggio As String
        Dim DA As New SqlDataAdapter
        Dim dtEnte As New DataTable
        Dim dtConfigurazione As New DataTable

        
        'RICHIAMO STORE CHE VERIFICA L'ACCESSO MASCHERA DELL' ENTE
        Accesso_Maschera_Ente(Session("TipoUtente"), Session("IDEnte"), abilitato, dati, annullamodifica, annullacancellazione, visualizzadatiaccreditati, messaggio)

        'Verifico se l'utente può effettuare un modifica senza effettuare un adeguamento
        'ADC 02/03/2022
        strsql = " SELECT IDENTE,IDSTATOENTE,CODICEREGIONE,isnull(flagforzaturaaccreditamento ,0) FLGABILITAZIONE,DATACREAZIONERECORD FROM ENTI WHERE IDENTE = @IdEnte "

        Dim SqlCmd As New SqlClient.SqlCommand
        Try
            SqlCmd.CommandText = strsql
            SqlCmd.CommandType = CommandType.Text
            SqlCmd.Connection = Session("Conn")
            SqlCmd.Parameters.Add("@IdEnte", SqlDbType.Int).Value = Session("IDEnte")
            DA.SelectCommand = SqlCmd
            DA.Fill(dtEnte)
        Catch ex As Exception
            MessaggiAlert(ex.Message) 
        End Try

        Dim CodRegione, idStatoEnte As String
        If dtEnte IsNot Nothing AndAlso dtEnte.Rows.Count > 0 Then
            Dim DataCreazioneEnte = CDate(dtEnte.Rows(0)("DATACREAZIONERECORD"))
            Session("DataCreazioneEnte") = DataCreazioneEnte
            If Not IsDBNull(dtEnte.Rows(0)("IDSTATOENTE")) Then
                idStatoEnte = dtEnte.Rows(0)("IDSTATOENTE")
                Select Case idStatoEnte

                    Case 3, 9 'Attivo o adeguamento
                        'Session("DATAACCREDITAMENTO") = CDate(dtEnte.Rows(0)("DATAACCREDITAMENTO"))
                        cmdModNoAdeguamento.Visible = True
                    Case 8 'Istruttoria
                        'Session("DATAACCREDITAMENTO") = CDate(dtEnte.Rows(0)("DATAACCREDITAMENTO"))
                        If Not IsDBNull(dtEnte.Rows(0)("CODICEREGIONE")) Then
                            CodRegione = dtEnte.Rows(0)("CODICEREGIONE")
                            If Not CodRegione Like ("CP%") Then
                                cmdModNoAdeguamento.Visible = True
                            Else
                                cmdModNoAdeguamento.Visible = False
                            End If
                        End If
                    Case Else
                        cmdModNoAdeguamento.Visible = False
                End Select
            End If
        End If

        'PERSONALIZZO CAMPI NON VISIBILI SE SONO ENTE O UNSC/REGIONE
        PersonalizzoNoVisibilitaCampi()

        'CARICA DATI
        If dati = 0 Then ' -- 0: dati tabelle reali        1: dati tabelle variazioni
            PopolaMaschera()
            isVariazione = False
        Else
            PopolaMascheraVariazione() 'PopolaMaschera dati variazione
            isVariazione = True
        End If

        'ABILITO/DISABILITO MASCHERA SE E' MODIFICABILE O IN LETTERA
        If abilitato = 0 Then ' -- 0: maschera sola lettura        1: maschera in modifica
            BloccaMaschera(messaggio)
        End If

        If annullamodifica = 0 Then '-- 0: funzione non abilitata    1: funzione abilitata
            'pulsante
            cmdAnnullaModifica.Visible = False
        Else
            cmdAnnullaModifica.Visible = True
        End If
        If visualizzadatiaccreditati = 0 Then '-- 0: funzione non abilitata    1: funzione abilitata
            'pulsante
            cmdVisualizzaDatiAccreditati.Visible = False
        Else
            cmdVisualizzaDatiAccreditati.Visible = True
        End If
        If messaggio <> "" Then

            lblMessaggio.Text = messaggio
        End If

        If ddlTipologia.SelectedValue = "Privato" Then
            chkSenzaScopoLucro.Visible = True
        Else
            chkSenzaScopoLucro.Visible = False
        End If

        If Session("TipoUTente") = "U" Then
            cmdPdfSettori.Visible = True
        End If

        AttivaCronologia()

        AttivaPulsantiModArt2Art10()
    End Sub

    Private Sub AttivaCronologia()
        For Each gRow As GridViewRow In dtgSettori.Rows

            Dim btnCronologia1 As Button = DirectCast(gRow.FindControl("btnCron1"), Button)
            Dim btnCronologia2 As Button = DirectCast(gRow.FindControl("btnCron2"), Button)
            Dim btnCronologia3 As Button = DirectCast(gRow.FindControl("btnCron3"), Button)
            Dim txtDesEsperienza3 As TextBox = DirectCast(gRow.FindControl("txtDesEsperienza3"), TextBox)
            Dim txtDesEsperienza2 As TextBox = DirectCast(gRow.FindControl("txtDesEsperienza2"), TextBox)
            Dim txtDesEsperienza1 As TextBox = DirectCast(gRow.FindControl("txtDesEsperienza1"), TextBox)

            If Not txtDesEsperienza3 Is Nothing AndAlso txtDesEsperienza3.Visible = True AndAlso txtDesEsperienza3.Text <> "" Then btnCronologia3.Visible = True And Not Session("TipoUtente") = "E" Else btnCronologia3.Visible = False
            If Not txtDesEsperienza2 Is Nothing AndAlso txtDesEsperienza2.Visible = True AndAlso txtDesEsperienza2.Text <> "" Then btnCronologia2.Visible = True And Not Session("TipoUtente") = "E" Else btnCronologia2.Visible = False
            If Not txtDesEsperienza1 Is Nothing AndAlso txtDesEsperienza1.Visible = True AndAlso txtDesEsperienza1.Text <> "" Then btnCronologia1.Visible = True And Not Session("TipoUtente") = "E" Else btnCronologia1.Visible = False

        Next
    End Sub

    Private Sub AttivaPulsantiModArt2Art10()
        'se utente di tipo dipartimento rende visibili in maschera i pulsanti per abilitare/disabilitare le modifiche Art2/Art10

        Dim sqlDAP As New SqlClient.SqlDataAdapter
        Dim dataSet As New DataSet
        Dim strNomeStore As String = "[SP_ACCREDITAMENTO_ELENCO_ABILITAZIONI_MOD_ENTE_ART2_ART10]"


        Try
            sqlDAP = New SqlClient.SqlDataAdapter(strNomeStore, CType(Session("conn"), SqlClient.SqlConnection))
            sqlDAP.SelectCommand.CommandType = CommandType.StoredProcedure

            If Session("IdEnte") IsNot Nothing AndAlso Session("IdEnte") <> "-1" Then
                sqlDAP.SelectCommand.Parameters.Add("@idente", SqlDbType.Int).Value = Integer.Parse(Session("IdEnte"))
            Else
                MessaggiAlert("Ente non selezionato")
                Exit Sub
            End If

            sqlDAP.SelectCommand.Parameters.Add("@Utente", SqlDbType.NVarChar, 10).Value = Session("Utente")
            sqlDAP.Fill(dataSet)

            If dataSet.Tables(0).Rows.Count > 0 Then
                'ciclo sulla griglia
                For Each gRow As GridViewRow In dtgSettori.Rows

                    Dim HFIdMacroAttivita As HiddenField = DirectCast(gRow.FindControl("HFIdMacroAttivita"), HiddenField)
                    Dim btnAbilita As Button = DirectCast(gRow.FindControl("btnAbArt2Art10"), Button)
                    Dim btnDisabilita As Button = DirectCast(gRow.FindControl("btnDisabArt2Art10"), Button)

                    'default
                    If btnAbilita IsNot Nothing Then btnAbilita.Visible = False
                    If btnDisabilita IsNot Nothing Then btnDisabilita.Visible = False

                    Dim _row As DataRow() = dataSet.Tables(0).Select("IdMacroAmbitoAttività=" + HFIdMacroAttivita.Value) 'ricerco il settore in quelli per cui abilitare/disabilitare le modifiche
                    If _row.Count > 0 Then
                        If _row(0).Item("abilitato") = "1" Then btnDisabilita.Visible = True Else btnAbilita.Visible = True
                    End If

                Next
            End If

        Catch ex As Exception
            MessaggiAlert("Errore nel recupero informazioni")
            Exit Sub
        End Try

    End Sub

    Private Sub BloccaMaschera(ByVal strmessaggio As String)
        dtgSettori.Enabled = False
        'rendo invisibili tutti i tasti INSERISCI
        For Each gRow As GridViewRow In dtgSettori.Rows
            Dim cmdInserisci As Button = DirectCast(gRow.FindControl("cmdModifica"), Button)
            If cmdInserisci IsNot Nothing Then cmdInserisci.Visible = False
            Dim check As CheckBox = DirectCast(gRow.FindControl("chkSeleziona"), CheckBox)
            check.Enabled = False
        Next


        txtBlocco.Value = "TRUE"
        'txtPartitaIvaArchiviata.ReadOnly = True
        'txtPartitaIvaArchiviata.BackColor = Color.Gainsboro
        txtdenominazione.ReadOnly = True
        txtdenominazione.BackColor = Color.Gainsboro
        txtCodFis.ReadOnly = True
        txtCodFis.BackColor = Color.Gainsboro
        'txtPartitaIva.ReadOnly = True
        'txtPartitaIva.BackColor = Color.Gainsboro
        ddlTipologia.Enabled = False
        ddlGiuridica.Enabled = False
        txtRichiedente.ReadOnly = True
        txtRichiedente.BackColor = Color.Gainsboro
        txtprefisso.ReadOnly = True
        txtprefisso.BackColor = Color.Gainsboro
        txtTelefono.ReadOnly = True
        txtTelefono.BackColor = Color.Gainsboro
        '<Aggiunti da alessnadra il 16.05.2005
        txtFax.ReadOnly = True
        txtFax.BackColor = Color.Gainsboro
        txtprefissofax.ReadOnly = True
        txtprefissofax.BackColor = Color.Gainsboro
        txtDataRicezioneCartacea.ReadOnly = True
        txtDataRicezioneCartacea.BackColor = Color.Gainsboro
        '****
        txtEstremiDSG.ReadOnly = True
        txtEstremiDSG.BackColor = Color.Gainsboro
        txtDataCostituzione.ReadOnly = True
        txtDataCostituzione.BackColor = Color.Gainsboro
        txthttp.ReadOnly = True
        txthttp.BackColor = Color.Gainsboro
        txtemail.ReadOnly = True
        txtemail.BackColor = Color.Gainsboro
        txtEmailpec.ReadOnly = True
        txtEmailpec.BackColor = Color.Gainsboro
        ddlClassi.Enabled = False
        'Aggiunto da Alessandra Taballione il 15/11/20005
        'Blocco aggiunto della sede principale dell'Ente
        'Indirizzo
        txtIndirizzo.ReadOnly = True
        txtIndirizzo.BackColor = Color.Gainsboro
        'dettaglioIndirizzo
        TxtDettaglioRecapito.ReadOnly = True
        TxtDettaglioRecapito.BackColor = Color.Gainsboro
        'civico
        txtCivico.ReadOnly = True
        txtCivico.BackColor = Color.Gainsboro
        'cap
        txtCAP.ReadOnly = True
        txtCAP.BackColor = Color.Gainsboro
        imgCap.Enabled = False



        'provincia
        ddlProvincia.Enabled = False
        ddlProvincia.BackColor = Color.Gainsboro
        'comune
        ddlComune.Enabled = False
        ddlComune.BackColor = Color.Gainsboro

        If ddlGiuridica.SelectedItem IsNot Nothing Then
            If ddlGiuridica.SelectedItem.Text <> String.Empty _
            AndAlso ddlGiuridica.SelectedItem.Text = "Altro ai sensi dell’Art. 1, comma 2, D.Lgs. 30-3-2001 n. 165 (specificare)" Then
                If txtAltraTipoEnte.Visible = True Then
                    txtAltraTipoEnte.Enabled = False
                End If
            End If
        End If
        ChkFirma.Enabled = False

        If Session("TipoUtente") <> "U" Then
            CmdInforScu1.Visible = False
        End If

        If (Session("TipoUtente") = "U" Or Session("TipoUtente") = "R") Then
            cmdEmail.Enabled = False
            cmdHttp.Enabled = False
            chkFalsohttp.Enabled = False
            chkVerohttp.Enabled = False
            chkFalsoEmail.Enabled = False
            chkVeroEmail.Enabled = False
            'Sandocan 22/01/2013 
            ChkFirma.Enabled = False
        End If
        txtDataNominaRL.Enabled = False
        
        CmdSalva.Visible = False
        chkSenzaScopoLucro.Enabled = False
        chkFiniIstituzionali.Enabled = False
        chkAttivita.Enabled = False
        chkRapportoAnnuale.Enabled = False
        CmdAttoNomina.Enabled = False
        CmdAttoNomina.BackColor = Color.Gainsboro
        cmdAllegaAttoCostitutivo.Enabled = False
        cmdAllegaAttoCostitutivo.BackColor = Color.Gainsboro
        cmdAllegaStatuto.Enabled = False
        cmdAllegaStatuto.BackColor = Color.Gainsboro
        cmdAllegaImpegnoEtico.Enabled = False
        cmdAllegaImpegnoEtico.BackColor = Color.Gainsboro
        cmdAllegaDeliberaAdesione.Enabled = False
        cmdAllegaDeliberaAdesione.BackColor = Color.Gainsboro
        cmdCaricaDelibera.Enabled = False
        cmdCaricaDelibera.BackColor = Color.Gainsboro

        btnModificaAN.Visible = False
        btnEliminaAN.Visible = False
        btnModificaAttoCostitutivo.Visible = False
        btnEliminaAttoCostitutitvo.Visible = False
        btnModificaCIE.Visible = False
        btnEliminaCIE.Visible = False
        btnModificaDelibera.Visible = False
        btnEliminaDelibera.Visible = False
        btnModificaDeliberaAdesione.Visible = False
        btnEliminaDeliberaAdesione.Visible = False
        btnModificaStatuto.Visible = False
        btnEliminaStatuto.Visible = False

        'controllo possibilità sostituzione art2/art10
        AbilitaSostituzioneArt2Art10()

        MessaggiAlert(strmessaggio)
    End Sub

    Private Sub AbilitaSostituzioneArt2Art10()
        Dim intIdEnte As Integer
        Dim intIdEnteAccoglienza As Integer
        Dim _settoriAbilitati As Boolean = False

        intIdEnte = Session("IdEnte")
        'intIdEnteAccoglienza = IIf(Request.QueryString("id") = Nothing, 0, Request.QueryString("id"))

        'ABILITA SOSTITUZIONE ART2/ART10

        Try
            Dim SqlCmd As SqlClient.SqlCommand


            SqlCmd = New SqlClient.SqlCommand
            SqlCmd.CommandText = "SP_ACCREDITAMENTO_INTEGRAZIONE_ART2_ART10_TITOLARE"
            SqlCmd.CommandType = CommandType.StoredProcedure
            SqlCmd.Connection = Session("Conn")
            SqlCmd.Parameters.Add("@IdEnte", SqlDbType.Int).Value = intIdEnte
            'SqlCmd.Parameters.Add("@IdEnteFiglio", SqlDbType.Int).Value = intIdEnteAccoglienza

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
            sparam1.ParameterName = "@AbilitaAttoNomina"
            'sparam1.Size = 100
            sparam1.SqlDbType = SqlDbType.TinyInt
            sparam1.Direction = ParameterDirection.Output
            SqlCmd.Parameters.Add(sparam1)

            Dim sparam2 As SqlClient.SqlParameter
            sparam2 = New SqlClient.SqlParameter
            sparam2.ParameterName = "@AbilitaAttoCostitutivo"
            'sparam1.Size = 100
            sparam2.SqlDbType = SqlDbType.TinyInt
            sparam2.Direction = ParameterDirection.Output
            SqlCmd.Parameters.Add(sparam2)

            Dim sparam3 As SqlClient.SqlParameter
            sparam3 = New SqlClient.SqlParameter
            sparam3.ParameterName = "@AbilitaDelibera"
            'sparam1.Size = 100
            sparam3.SqlDbType = SqlDbType.TinyInt
            sparam3.Direction = ParameterDirection.Output
            SqlCmd.Parameters.Add(sparam3)

            Dim sparam4 As SqlClient.SqlParameter
            sparam4 = New SqlClient.SqlParameter
            sparam4.ParameterName = "@AbilitaDeliberaAdesione"
            'sparam1.Size = 100
            sparam4.SqlDbType = SqlDbType.TinyInt
            sparam4.Direction = ParameterDirection.Output
            SqlCmd.Parameters.Add(sparam4)

            Dim sparam5 As SqlClient.SqlParameter
            sparam5 = New SqlClient.SqlParameter
            sparam5.ParameterName = "@AbilitaCIE"
            'sparam1.Size = 100
            sparam5.SqlDbType = SqlDbType.TinyInt
            sparam5.Direction = ParameterDirection.Output
            SqlCmd.Parameters.Add(sparam5)

            Dim sparam6 As SqlClient.SqlParameter
            sparam6 = New SqlClient.SqlParameter
            sparam6.ParameterName = "@AbilitaStatuto"
            'sparam1.Size = 100
            sparam6.SqlDbType = SqlDbType.TinyInt
            sparam6.Direction = ParameterDirection.Output
            SqlCmd.Parameters.Add(sparam6)

            Dim sparam7 As SqlClient.SqlParameter
            sparam7 = New SqlClient.SqlParameter
            sparam7.ParameterName = "@AbilitaSettori"
            sparam7.Size = 50
            sparam7.SqlDbType = SqlDbType.VarChar
            sparam7.Direction = ParameterDirection.Output
            SqlCmd.Parameters.Add(sparam7)

            SqlCmd.ExecuteNonQuery()

            Session("IdEnteFaseArt") = SqlCmd.Parameters("@IdEnteFaseArt_2_10").Value

            If SqlCmd.Parameters("@AbilitaAttoNomina").Value = 1 Then
                btnModificaAN.Visible = True
                rowAN.Visible = True
                rowNoAN.Visible = False
            End If
            If SqlCmd.Parameters("@AbilitaAttoCostitutivo").Value = 1 Then
                btnModificaAttoCostitutivo.Visible = True
                rowAttoCostitutivo.Visible = True
                rowNoAttoCostitutivo.Visible = False
            End If
            If SqlCmd.Parameters("@AbilitaDelibera").Value = 1 Then
                btnModificaDelibera.Visible = True
                rowDelibera.Visible = True
                rowNoDelibera.Visible = False
            End If
            If SqlCmd.Parameters("@AbilitaDeliberaAdesione").Value = 1 Then
                btnModificaDeliberaAdesione.Visible = True
                rowDeliberaAdesione.Visible = True
                rowNoDeliberaAdesione.Visible = False
            End If
            If SqlCmd.Parameters("@AbilitaCIE").Value = 1 Then
                btnModificaCIE.Visible = True
                rowImpegnoEtico.Visible = True
                rowNoImpegnoEtico.Visible = False
            End If
            If SqlCmd.Parameters("@AbilitaStatuto").Value = 1 Then
                btnModificaStatuto.Visible = True
                rowStatuto.Visible = True
                rowNoStatuto.Visible = False
            End If
            If Not String.IsNullOrEmpty(SqlCmd.Parameters("@AbilitaSettori").Value) Then
                _settoriAbilitati = AbilitaSettoriArt2Art10(SqlCmd.Parameters("@AbilitaSettori").Value)
            End If

            If SqlCmd.Parameters("@AbilitaAttoNomina").Value = 1 Or _
                SqlCmd.Parameters("@AbilitaAttoCostitutivo").Value = 1 Or
                SqlCmd.Parameters("@AbilitaDelibera").Value = 1 Or _
                SqlCmd.Parameters("@AbilitaDeliberaAdesione").Value = 1 Or _
                SqlCmd.Parameters("@AbilitaCIE").Value = 1 Or _
                SqlCmd.Parameters("@AbilitaStatuto").Value = 1 Or _
                _settoriAbilitati Then

                CmdSalvaDoc.Visible = True
                lblMessaggioArt2Art10.Text = "E' consentita l'integrazione dei dati/documenti richiesti dal Dipartimento con Art.2/Art.10"
                lblMessaggioArt2Art10.Visible = True
            Else
                CmdSalvaDoc.Visible = False
                lblMessaggioArt2Art10.Visible = False
            End If

        Catch ex As Exception

            lblMessaggio.Text = ex.Message

        End Try

    End Sub

    Function AbilitaSettoriArt2Art10(ElencoSettori As String) As Boolean

        Dim _ret As Boolean = False
        Dim _settori As String() = ElencoSettori.Split(",")

        If _settori.Length > 0 Then
            dtgSettori.Enabled = True
            For Each gRow As GridViewRow In dtgSettori.Rows
                Dim HFIdMacroAttivita As HiddenField = DirectCast(gRow.FindControl("HFIdMacroAttivita"), HiddenField)
                Dim divAree As HtmlGenericControl = gRow.Cells(2).FindControl("divArea")
                Dim cmdInserisci As Button = DirectCast(gRow.FindControl("cmdModifica"), Button)
                If Array.Exists(_settori, Function(s) s = HFIdMacroAttivita.Value) Then
                    gRow.Enabled = True
                    'divAree.Style("display") = "block"
                    cmdInserisci.Visible = True
                    _ret = True
                Else
                    gRow.Enabled = False
                    'divAree.Style("display") = "none"
                    cmdInserisci.Visible = False
                End If
            Next
        End If

        Return _ret
    End Function

    Public Sub Accesso_Maschera_Ente(ByVal TipoUtente As String, ByVal IDEnte As Integer, ByRef abilitato As Integer, ByRef dati As Integer, ByRef annullamodifica As Integer, ByRef annullacancellazione As Integer, ByRef visualizzadatiaccreditati As Integer, ByRef messaggio As String)

        'REALIZZATA DA: SIMONA CORDELLA 
        'DATA REALIZZAZIONE:  25/07/2014
        'FUNZIONALITA': Verifica le condizioni di accreditamento e le eventuali modifiche in corso sui dati dell'ente
        '               e ritorna all'applicazione la configurazione da applicare alla maschera di gestion
        '               @abilitato                  -- 0: maschera sola lettura        1: maschera in modifica
        '               @dati                         -- 0: dati tabelle reali        1: dati tabelle variazioni
        '               @annullamodifica              -- 0: funzione non abilitata    1: funzione abilitata
        '               @visualizzadatiaccreditati     -- 0: funzione non abilitata    1: funzione abilitata
        '               @annullacancellazione       -- 0: funzione non abilitata    1: funzione abilitata
        '               @messaggio                  -- eventuale messaggio di ritorno da visualizzare all'utente



        Try
            Dim SqlCmd As SqlClient.SqlCommand
            '            Dim Dt As New DataTable
            '           Dim Da As New SqlClient.SqlDataAdapter(SqlCmd)

            SqlCmd = New SqlClient.SqlCommand
            SqlCmd.CommandText = "SP_ACCREDITAMENTO_ACCESSOMASCHERA_ENTE"
            SqlCmd.CommandType = CommandType.StoredProcedure
            SqlCmd.Connection = Session("Conn")
            SqlCmd.Parameters.Add("@TipoUtente", SqlDbType.VarChar).Value = TipoUtente
            SqlCmd.Parameters.Add("@IdEnte", SqlDbType.Int).Value = IDEnte

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

            lblMessaggio.Text = ex.Message
        Finally
            'ConnX.Close()
            'SqlCmd = Nothing
        End Try

    End Sub

    Sub PersonalizzoNoVisibilitaCampi()
        If Session("TipoUtente") = "E" Then
            imgAccordo.Visible = False
            txtDataRicezioneCartacea.ReadOnly = True
            'imgPIVA.Visible = False
            'imgCodFis.Visible = False
            ImgCronologiaProgetti.Visible = False
            imgCronologiaDocumenti.Visible = False
            ImgEmail_Pec.Visible = False
            If lblStato.Text = "Registrato" Or lblStato.Text = "Istruttoria" Then
                ChkFirma.Enabled = False
            Else
                ChkFirma.Enabled = True
            End If

        End If
        'Inserire qui il codice utente necessario per inizializzare la pagina

        txtBlocco.Value = "FALSE"
        'lblMessaggio.Text = ""
        'Image4.Visible = False
        lblTipoUtente.Value = Session("TipoUtente")
        If (lblTipoUtente.Value = "U" Or lblTipoUtente.Value = "R") Then
            lblIdEnte.Value = Session("idEnte")
            txtdataControllohttp.Visible = True
            txtdataControlloEmail.Visible = True
            chkFalsohttp.Visible = True
            chkVerohttp.Visible = True
            chkFalsoEmail.Visible = True
            chkVeroEmail.Visible = True
            lblDelHttp.Visible = True
            lblDelEmail.Visible = True
            'Aggiunto da Alessandra Taballione il 20.12.2004
            'txtCodRegione.ReadOnly = False
            'txtCodRegione.BackColor = Color.White
            txtEmailpec.Enabled = True
            txtEmailpec.ReadOnly = False
            txtEmailpec.BackColor = Color.White
            ChkFirma.Enabled = True
        Else
            lblIdEnte.Value = Session("idEnte")
            txtdataControllohttp.Visible = False
            txtdataControlloEmail.Visible = False
            chkFalsohttp.Visible = False
            chkVerohttp.Visible = False
            chkFalsoEmail.Visible = False
            chkVeroEmail.Visible = False
            cmdEmail.Visible = False
            cmdHttp.Visible = False
            lblDelHttp.Visible = False
            lblDelEmail.Visible = False
            'Aggiunto da Alessandra Taballione il 20.12.2004
            'txtCodRegione.ReadOnly = True
            'txtCodRegione.BackColor = Color.Gainsboro
            'txtCodRegione.BackColor = Color.Red

        End If
    End Sub

    Private Sub CaricaDettaglioTipologiaEntiSCU(ByVal Tipologia As String, ByVal VengoDA As String)
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
        ddlGiuridica.Items.Clear()
        If Tipologia = "" Then Exit Sub

        If VengoDA = "POPOLAMASCHERA" Then
            strsql = "select Privato,abilitata, isnull(ALBO,'SCU') as ALBO from tipologieenti where Descrizione = '" & Replace(Tipologia, "'", "''") & "' "
            dtrgenerico = ClsServer.CreaDatareader(strsql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))

            If dtrgenerico.HasRows = True Then
                dtrgenerico.Read()
                If dtrgenerico("Privato") = True Then
                    strsql = "select '' descrizione , 0 idtipologieenti,null as Ordinamento union select descrizione, idtipologieenti,Ordinamento from tipologieenti where (privato =1 and abilitata=1 AND  isnull(ALBO,'SCU') ='SCU' and iscrizione = 1) or descrizione = (select tipologia from enti where IDEnte=0" & Session("IdEnte") & ")  order by Ordinamento"
                Else
                    strsql = "select '' descrizione , 0 idtipologieenti,null as Ordinamento union select descrizione, idtipologieenti,Ordinamento from tipologieenti where (privato =0 and abilitata=1 AND  isnull(ALBO,'SCU') ='SCU' and iscrizione = 1) or descrizione = (select tipologia from enti where IDEnte=0" & Session("IdEnte") & ")   order by Ordinamento"
                End If
            End If
        End If
        If VengoDA = "COMBOTIPOLOGIA" Then
            If Tipologia.ToUpper = "PRIVATO" Then
                strsql = "select '' descrizione , 0 idtipologieenti,null as Ordinamento union select descrizione, idtipologieenti,Ordinamento from tipologieenti where (privato =1 and abilitata=1 AND  isnull(ALBO,'SCU') ='SCU' and iscrizione = 1) or descrizione = (select tipologia from enti where IDEnte=0" & Session("IdEnte") & ")  order by Ordinamento"
            Else
                strsql = "select '' descrizione , 0 idtipologieenti,null as Ordinamento union select descrizione, idtipologieenti,Ordinamento from tipologieenti where (privato =0 and abilitata=1 AND  isnull(ALBO,'SCU') ='SCU' and iscrizione = 1) or descrizione = (select tipologia from enti where IDEnte=0" & Session("IdEnte") & ")   order by Ordinamento"
            End If
        End If
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If

        dtrgenerico = ClsServer.CreaDatareader(strsql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))

        ddlGiuridica.DataSource = dtrgenerico
        ddlGiuridica.DataValueField = "idtipologieenti"
        ddlGiuridica.DataTextField = "Descrizione"
        ddlGiuridica.DataBind()

        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If

    End Sub

    Private Sub PopolaMaschera()
        Dim Tipologia As String = ""
        Dim AlboEnte As String = ""
        Dim privato As Integer
        Dim IdComune As String
        Dim chkSenzaLucro As Boolean?
        Dim AltroTipoEnte As String
        Dim RapportAnnuale As Boolean?
        'Generata da Alessandra Taballione il 25/02/04
        'popolamento della Maschera di Gestione dell'Ente
        'Stato Ente
        'Popolamento della Maschera
        'strsql = "Select * from Enti Where idEnte =" & Session("IdEnte") & ""
        strsql = "select day(datacontrolloemail)as ggDCEmail,month(datacontrolloemail)as monthDCEmail,year(datacontrolloemail)as yearDCEmail," &
        " day(datacontrollohttp)as ggDChttp,month(datacontrollohttp)as monthDChttp,year(datacontrollohttp)as yearDChttp, statienti.statoente," &
        " classiaccreditamento.classeaccreditamento,classiaccreditamento.EntiInPartenariato, " &
        " enti.Tipologia,enti.CodiceFiscale,isnull(enti.CodiceRegione,'Assente')as codiceregione," &
        " enti.Datacontrolloemail,enti.Datacontrollohttp," &
        " enti.dataCostituzione,enti.DataRicezioneCartacea,enti.Denominazione,enti.EstremiDeliberaStrutturaGestione," &
        " enti.http,enti.Datacontrollohttp,enti.httpvalido," &
        " enti.Email,enti.Datacontrolloemail,enti.emailvalido,enti.NoteRichiestaRegistrazione," &
        " enti.TelefonoRichiestaRegistrazione," &
        " enti.IdSezione," &
        " enti.PrefissoTelefonoRichiestaRegistrazione,enti.PrefissoFax,enti.Fax,entipassword.Username,enti.dataultimaClasseaccreditamento," &
        " enti.idclasseaccreditamentorichiesta,enti.idclasseaccreditamento,isnull(enti.CodiceFiscaleArchivio,'') as CodiceFiscaleArchivio,enti.PEC,enti.Firma,enti.EmailCertificata, " &
        " isnull(enti.StatoPec,0) as StatoPec,isnull(CausaliPECNegative.Causale,'') as CausalePec, enti.albo, isnull(t.privato,2) as privato,enti.DataNominaRL,enti.AttivitaUltimiTreAnni, enti.AttivitaFiniIstituzionali, enti.AttivitaSenzaLucro, enti.AltroTipoEnte,enti.RapportoAnnuale " &
        " from enti " &
        " inner join classiaccreditamento on (classiaccreditamento.idclasseaccreditamento=enti.idclasseaccreditamento)" &
        " inner join statienti on (statienti.idstatoente=enti.idstatoente) " &
        " left join entipassword on (enti.idente=entipassword.idente)" &
        " left join CausaliPECNegative on enti.IdCAusalePecNegativa = CausaliPECNegative.IdCAusalePecNegativa " &
         " left join classiaccreditamento as classiaccreditamentorichieste on (classiaccreditamento.idclasseaccreditamento=enti.idclasseaccreditamentorichiesta)" &
         " left join tipologieenti t on enti.tipologia = t.descrizione " &
        " where enti.idente=" & Session("IdEnte") & ""
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
        dtrgenerico = ClsServer.CreaDatareader(strsql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
        dtrgenerico.Read()
        If Not IsDBNull(dtrgenerico("StatoEnte")) Then
            lblStato.Text = dtrgenerico("StatoEnte")
        End If
        'Antonello--------------------------------------
        ''carico combo tipologia in relazione allo stato

        If Not IsDBNull(dtrgenerico("AttivitaSenzaLucro")) Then
            chkSenzaLucro = dtrgenerico("AttivitaSenzaLucro")
        End If

        If Not IsDBNull(dtrgenerico("AltroTipoEnte")) Then
            AltroTipoEnte = dtrgenerico("AltroTipoEnte")
        End If

        If Not IsDBNull(dtrgenerico("RapportoAnnuale")) Then
            RapportAnnuale = dtrgenerico("RapportoAnnuale")
        End If


        AlboEnte = dtrgenerico("ALBO")
        privato = dtrgenerico("privato")
        '------------------------------------------------
        If Not IsDBNull(dtrgenerico("Tipologia")) Then
            Tipologia = dtrgenerico("Tipologia")
            'If dtrgenerico("ALBO") = "SCN" Then
            '    If UCase(Tipologia) = "PRIVATO" Then
            '        ddlTipologia.SelectedIndex = trovaindex(Tipologia, ddlTipologia)
            '        ddlGiuridica.Visible = False
            '    Else
            '        ddlTipologia.SelectedIndex = 1
            '        ddlGiuridica.Visible = True
            '        ddlGiuridica.SelectedIndex = trovaindex(Tipologia, ddlGiuridica)
            '        txtpianolocale.Value = ddlGiuridica.SelectedItem.Text
            '    End If
            'Else
            '    CaricaDettaglioTipologiaEntiSCU(Tipologia, "POPOLAMASCHERA")
            '    ddlGiuridica.Visible = True
            '    ddlGiuridica.SelectedIndex = trovaindex(dtrgenerico("Tipologia"), ddlGiuridica)
            '    txtpianolocale.Value = ddlGiuridica.SelectedItem.Text
            'End If
        End If
        txtCodFis.Text = IIf(Not IsDBNull(dtrgenerico("CodiceFiscale")), dtrgenerico("CodiceFiscale"), "")
        lblCodRegione.Text = IIf(Not IsDBNull(dtrgenerico("CodiceRegione")), dtrgenerico("CodiceRegione"), "")
        If Not IsDBNull(dtrgenerico("Datacontrolloemail")) Then
            txtdataControlloEmail.Text = dtrgenerico("ggDCemail") & "/" & IIf(CInt(dtrgenerico("monthDCemail")) < 10, "0" & dtrgenerico("monthDCemail"), dtrgenerico("monthDCemail")) & "/" & dtrgenerico("YearDCemail")
        End If
        If Not IsDBNull(dtrgenerico("Datacontrollohttp")) Then
            txtdataControllohttp.Text = dtrgenerico("ggDChttp") & "/" & IIf(CInt(dtrgenerico("monthDChttp")) < 10, "0" & dtrgenerico("monthDChttp"), dtrgenerico("monthDChttp")) & "/" & dtrgenerico("yearDChttp")
        End If

        'Aggiunto da Andrea Mondello il 26/04/2021 atto di nomina del Rappreesentante Legale inizio
        txtDataNominaRL.Text = IIf(Not IsDBNull(dtrgenerico("DataNominaRL")), dtrgenerico("DataNominaRL"), "")
        'Aggiunto da Andrea Mondello il 26/04/2021 atto di nomina del Rappreesentante Legale fine
        'Aggiunto da Alessandra Taballione il 28/02/2005
        'Implementazione del campo data di costituzione Ente
        txtDataCostituzione.Text = IIf(Not IsDBNull(dtrgenerico("dataCostituzione")), dtrgenerico("DataCostituzione"), "")
        'Aggiunto da Alessandra Taballione il 16/05/2005
        'Implementazione del campo data Richiesta Ente
        txtDataRicezioneCartacea.Text = Left(IIf(Not IsDBNull(dtrgenerico("DataRicezioneCartacea")), dtrgenerico("DataRicezioneCartacea"), ""), 10)
        If (Session("TipoUtente") <> "U" And Session("TipoUtente") <> "R") Then
            txtDataRicezioneCartacea.ReadOnly = True
            txtDataRicezioneCartacea.BackColor = Color.Gainsboro
        End If
        txtdenominazione.Text = IIf(Not IsDBNull(dtrgenerico("Denominazione")), dtrgenerico("Denominazione"), "")
        txtEstremiDSG.Text = IIf(Not IsDBNull(dtrgenerico("EstremiDeliberaStrutturaGestione")), dtrgenerico("EstremiDeliberaStrutturaGestione"), "")
        If Not IsDBNull(dtrgenerico("http")) Then
            txthttp.Text = dtrgenerico("http")
            If Not IsDBNull(dtrgenerico("Datacontrollohttp")) Then
                If dtrgenerico("httpvalido") = True Then
                    chkVerohttp.Checked = True
                    chkFalsohttp.Checked = False
                Else
                    chkVerohttp.Checked = False
                    chkFalsohttp.Checked = True
                End If
            Else
                chkVerohttp.Checked = False
                chkFalsohttp.Checked = False
            End If
        End If
        '-----------------------Antonello -------- Carico Pac e Firma--------
        If Not IsDBNull(dtrgenerico("PEC")) Then
            If dtrgenerico("PEC") = 0 Then
                ChkPec.Value = "False"
            Else
                ChkPec.Value = "True"
            End If
        End If

        If Not IsDBNull(dtrgenerico("AttivitaUltimiTreAnni")) Then
            If dtrgenerico("AttivitaUltimiTreAnni") = True Then
                chkAttivita.Checked = True
            Else
                chkAttivita.Checked = False
            End If
        End If

        If Not IsDBNull(dtrgenerico("AttivitaFiniIstituzionali")) Then
            If dtrgenerico("AttivitaFiniIstituzionali") = True Then
                chkFiniIstituzionali.Checked = True
            Else
                chkFiniIstituzionali.Checked = False
            End If
        End If

        If Not IsDBNull(dtrgenerico("Firma")) Then
            If dtrgenerico("Firma") = 0 Then
                ChkFirma.Checked = False
                ChkFirmaPrima.Value = "False"
            Else
                ChkFirma.Checked = True
                ChkFirmaPrima.Value = "True"
            End If
        End If
        If Not IsDBNull(dtrgenerico("EmailCertificata")) Then
            txtEmailpec.Text = dtrgenerico("EmailCertificata")
        Else
            txtEmailpec.Text = ""
        End If
        '**** ****
        If txtEmailpec.Text = "" Then
            imgStatoPec.Visible = False
        Else
            If Session("TipoUtente") = "E" Then
                imgStatoPec.Visible = False
            Else
                'imgStatoPec.Visible = True
                Select Case dtrgenerico("StatoPec")
                    Case 0 'davalutare
                        imgStatoPec.ImageUrl = "images/alert.png"
                        imgStatoPec.ToolTip = "PEC da verificare"
                        imgStatoPec.AlternateText = "PEC da verificata"
                    Case 1 'valida
                        imgStatoPec.ImageUrl = "images/vistabuona_small.png"
                        imgStatoPec.ToolTip = "PEC verificata"
                        imgStatoPec.AlternateText = "PEC verificata"
                    Case 2 'non valida
                        imgStatoPec.ImageUrl = "images/vistacattiva_small.png"
                        imgStatoPec.ToolTip = "PEC non valida: " & dtrgenerico("CausalePec")
                        imgStatoPec.AlternateText = "PEC non valida"
                End Select
            End If
        End If

        '**********
        '---------------------------------------------------------------------
        If Not IsDBNull(dtrgenerico("Email")) Then
            txtemail.Text = dtrgenerico("Email")
            If Not IsDBNull(dtrgenerico("Datacontrolloemail")) Then
                If dtrgenerico("emailvalido") = True Then
                    chkVeroEmail.Checked = True
                    chkFalsoEmail.Checked = False
                Else
                    chkVeroEmail.Checked = False
                    chkFalsoEmail.Checked = True
                End If
            Else
                chkVeroEmail.Checked = False
                chkFalsoEmail.Checked = False
            End If
        End If
        'Modificato da Alessandra Taballione il 09/06/2004
        'non verrà piu inserito il numero delle sedi ma ci sarà una tabella
        'che conterrà le informazioni relative alla classe Richiesta
        'CodiceFiscaleArchivio, isnull(enti.PartitaIVAArchivio) as PartitaIVAArchivio
        txtCodFisArchiviato.Value = dtrgenerico("CodiceFiscaleArchivio")
        'antonello-------------
        'txtPartitaIvaArchiviata.Text = dtrgenerico("PartitaIVAArchivio")
        'txtPartitaIva.Text = IIf(Not IsDBNull(dtrgenerico("PartitaIva")), dtrgenerico("PartitaIva"), "")
        '------------------------------------------------------------------------------------
        txtRichiedente.Text = IIf(Not IsDBNull(dtrgenerico("NoteRichiestaRegistrazione")), dtrgenerico("NoteRichiestaRegistrazione"), "")
        txtTelefono.Text = IIf(Not IsDBNull(dtrgenerico("TelefonoRichiestaRegistrazione")), dtrgenerico("TelefonoRichiestaRegistrazione"), "")
        txtprefisso.Text = IIf(Not IsDBNull(dtrgenerico("PrefissoTelefonoRichiestaRegistrazione")), dtrgenerico("PrefissoTelefonoRichiestaRegistrazione"), "")
        '**********************BLOCCO FAX**********************************************
        txtprefissofax.Text = IIf(Not IsDBNull(dtrgenerico("PrefissoFax")), dtrgenerico("PrefissoFax"), "")
        txtFax.Text = IIf(Not IsDBNull(dtrgenerico("Fax")), dtrgenerico("Fax"), "")
        '**********************FINE BLOCCO FAX**********************************************
        txtUtenza.Text = IIf(Not IsDBNull(dtrgenerico("Username")), dtrgenerico("Username"), "")
        lblClasse.Text = dtrgenerico("classeaccreditamento")
        'le classi non esistono piu'
        If lblClasse.Text = "Nessuna Classe" Then lblClasse.Text = "Nessuna Sezione"

        lblIdClasse.Value = dtrgenerico("idclasseaccreditamento")
        lblDataClasse.Text = Mid(IIf(Not IsDBNull(dtrgenerico("dataultimaClasseaccreditamento")), dtrgenerico("dataultimaClasseaccreditamento"), ""), 1, 10) 'dtrgenerico("dataultimaClasseaccreditamento")
        lblInPartenariato.Value = dtrgenerico("EntiInPartenariato")
        txtidClasseRichiesta.Value = dtrgenerico("idclasseaccreditamentorichiesta")
        If Not IsDBNull(dtrgenerico("idclasseaccreditamentorichiesta")) And dtrgenerico("idclasseaccreditamentorichiesta") <> 6 Then
            If Not IsDBNull(dtrgenerico("idsezione")) Then
                ddlClassi.SelectedValue = dtrgenerico("idsezione")
            End If
            'metto in sessione l'id della classe dell'ente selezionato
            'per controllare il change della combo delle classi
            'controllo aggiunto da BagaJon il 7/3/2006
            Session("JonIdClasseAccreditamento") = dtrgenerico("idclasseaccreditamentorichiesta")
            txtidClasseRichiesta.Value = dtrgenerico("idclasseaccreditamentorichiesta")
            'Modificato da Alessandra Taballione il 09/06/2004
            'implementazione delle informazioni della classe richiesta
            If Not dtrgenerico Is Nothing Then
                dtrgenerico.Close()
                dtrgenerico = Nothing
            End If
            'dtrgenerico = ClsServer.CreaDatareader("Select * from classiaccreditamento where idclasseaccreditamento=" & ddlClassi.SelectedValue & "", IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))

            dtrgenerico = ClsServer.CreaDatareader("Select * from classiaccreditamento where idclasseaccreditamento=" & txtidClasseRichiesta.Value & "", IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))

            dtrgenerico.Read()
            If dtrgenerico.HasRows = True Then
                lblmaxsedi.Text = dtrgenerico("maxsediTesto")
                lblminSedi.Text = dtrgenerico("minSediTesto")
                lblmaxVol.Text = dtrgenerico("MaxEntitàperannoTesto")
                lblminVol.Text = dtrgenerico("MinEntitàperannoTesto")
            End If
        End If
        If Tipologia <> "" Then
            If AlboEnte = "SCN" Then
                If privato = -1 Then
                    ddlTipologia.SelectedIndex = 2
                    'trovaindex("PRIVATO", ddlTipologia)
                    ddlGiuridica.Visible = False
                    Label6.Visible = False
                Else
                    ddlTipologia.SelectedIndex = 1
                    ddlGiuridica.Visible = True
                    Label6.Visible = True
                    ddlGiuridica.SelectedIndex = trovaindex(Tipologia, ddlGiuridica)
                    txtpianolocale.Value = ddlGiuridica.SelectedItem.Text
                End If
            Else
                Select Case privato
                    Case 0 'PUBBLICO
                        ddlTipologia.SelectedIndex = 1
                    Case -1 'PRIVATO
                        ddlTipologia.SelectedIndex = 2
                    Case 2 'NULL
                        ddlTipologia.SelectedIndex = 0
                End Select
                CaricaDettaglioTipologiaEntiSCU(Tipologia, "POPOLAMASCHERA")
                ddlGiuridica.Visible = True
                Label6.Visible = True
                ddlGiuridica.SelectedIndex = trovaindex(Tipologia, ddlGiuridica)
                If ddlGiuridica.SelectedIndex > -1 Then txtpianolocale.Value = ddlGiuridica.SelectedItem.Text
            End If
        End If


        If ddlTipologia.SelectedValue = "Privato" Then
            If chkSenzaLucro IsNot Nothing Then
                divSenzaScopoLucro.Visible = True
                If (chkSenzaLucro) Then
                    chkSenzaScopoLucro.Checked = True
                Else
                    chkSenzaScopoLucro.Checked = False
                End If
            End If
        Else
            divSenzaScopoLucro.Visible = False
        End If


        If RapportAnnuale Then
            chkRapportoAnnuale.Checked = True
        Else
            chkRapportoAnnuale.Checked = False
        End If



        If ddlGiuridica.SelectedItem IsNot Nothing _
            AndAlso ddlGiuridica.SelectedItem.Text = "Altro ai sensi dell’Art. 1, comma 2, D.Lgs. 30-3-2001 n. 165 (specificare)" Then
            divAltraTipologia.Visible = True
            txtAltraTipoEnte.Text = AltroTipoEnte
        Else
            txtAltraTipoEnte.Text = String.Empty
            divAltraTipologia.Visible = False
        End If


        'Aggiunto da Alesssandra Taballione il 18/05/2005
        'Implementazione del tasto Ripristina 
        'Visibile solo unsc e solo se ente sospeso 
        'Cambio stato da sospeso a registrato
        'con clonologia
        If (Session("TipoUtente") = "U" Or Session("TipoUtente") = "R") Then
            strsql = "select * from statienti " &
            " inner join enti on enti.idstatoente=statienti.idstatoente " &
            " where idente = " & Session("idEnte") & " And sospeso = 1 "
            If Not dtrgenerico Is Nothing Then
                dtrgenerico.Close()
                dtrgenerico = Nothing
            End If
            dtrgenerico = ClsServer.CreaDatareader(strsql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
            If Not dtrgenerico Is Nothing Then
                dtrgenerico.Close()
                dtrgenerico = Nothing
            End If
        End If
        'Aggiunto da Alessandra Taballione il 11/11/2005
        'Ricerca e popolamento dei dati relativi alla sede principale dell'Ente
        strsql = "SELECT  entisedi.identesede,entisedi.idcomune,entisedi.indirizzo,entisedi.DettaglioRecapito,entisedi.civico,entisedi.cap,comuni.denominazione as comune," &
        " provincie.provincia, provincie.idProvincia, provincie.ProvinceNazionali  " &
        " FROM entisedi " &
        " INNER JOIN statientisedi on statientisedi.idstatoentesede=entisedi.idstatoentesede " &
        " INNER JOIN entiseditipi ON entisedi.IDEnteSede = entiseditipi.IDEnteSede " &
        " INNER JOIN comuni ON entisedi.IDComune = comuni.IDComune " &
        " INNER JOIN provincie ON comuni.IDProvincia = provincie.IDProvincia " &
        " WHERE entisedi.IDEnte = " & Session("IdEnte") & " And entiseditipi.idtiposede = 1 and (statientisedi.attiva=1 or statientisedi.DefaultStato=1)"
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
        dtrgenerico = ClsServer.CreaDatareader(strsql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
        If dtrgenerico.HasRows = True Then
            dtrgenerico.Read()
            txtIndirizzo.Text = dtrgenerico("Indirizzo")
            'Antonello--------------------

            TxtDettaglioRecapito.Text = IIf(Not IsDBNull(dtrgenerico("DettaglioRecapito")), dtrgenerico("DettaglioRecapito"), "")
            txtCivico.Text = dtrgenerico("civico")
            txtCAP.Text = dtrgenerico("cap")
            ddlProvincia.SelectedValue = dtrgenerico("idProvincia")
            IdComune = dtrgenerico("idcomune")
            txtIdSede.Value = dtrgenerico("identeSede")
            txtIDComunes.Value = dtrgenerico("idcomune")
            'txtCity.Visible = True
            Session("IdComune") = dtrgenerico("idcomune")
            ChkEstero.Checked = Not CBool(dtrgenerico("ProvinceNazionali"))
            If ChkEstero.Checked = True Then
                lblComune.Text = "Località"
                lblCAP.Text = "Codice località"
            Else
                lblComune.Text = "Comune"
                lblCAP.Text = "CAP"
            End If
            Dim idProvincia As String = dtrgenerico("idprovincia")
            If Not dtrgenerico Is Nothing Then
                dtrgenerico.Close()
                dtrgenerico = Nothing
            End If
            selComune.CaricaProvincie(ddlProvincia, ChkEstero.Checked, Session("Conn"))
            If Not dtrgenerico Is Nothing Then
                dtrgenerico.Close()
                dtrgenerico = Nothing
            End If
            If Not ddlProvincia.SelectedValue Is Nothing Then
                ddlProvincia.SelectedValue = idProvincia
            End If
            If Not Session("IdComune") Is Nothing Then
                selComune.CaricaComuniDaProvincia(ddlComune, idProvincia, Session("Conn"))
                ddlComune.SelectedValue = Session("IdComune")

            End If
        End If
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If

        'Carico gli allegati salvati e li metto in sessione
        CaricaEntiAllegati()
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If

        'richiamo sub per caricare i Settori associati all'Ente
        CaricaEntiSettori()
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If

    End Sub

    Private Sub PopolaMascheraVariazione()
        Dim Tipologia As String = ""
        Dim AlboEnte As String = ""
        Dim privato As Integer
        Dim IdComune As String
        Dim chkSenzaLucro As Boolean?
        Dim AltroTipoEnte As String
        Dim RapportAnnuale As Boolean?
        'Generata da Alessandra Taballione il 25/02/04
        'popolamento della Maschera di Gestione dell'Ente
        'Stato Ente
        'Popolamento della Maschera
        'strsql = "Select * from Enti Where idEnte =" & Session("IdEnte") & ""
        strsql = " Select day(enti.datacontrolloemail)as ggDCEmail,month(enti.datacontrolloemail)as monthDCEmail,year(enti.datacontrolloemail)as yearDCEmail," &
                " day(enti.datacontrollohttp)as ggDChttp,month(enti.datacontrollohttp)as monthDChttp,year(enti.datacontrollohttp)as yearDChttp, statienti.statoente," &
                " classiaccreditamento.classeaccreditamento,classiaccreditamento.EntiInPartenariato, " &
                " enti.Tipologia,enti.CodiceFiscale,isnull(e.CodiceRegione,'Assente')as codiceregione," &
                " enti.Datacontrolloemail,enti.Datacontrollohttp," &
                " enti.dataCostituzione,enti.DataRicezioneCartacea,enti.Denominazione,enti.EstremiDeliberaStrutturaGestione," &
                " enti.http,enti.Datacontrollohttp,enti.httpvalido," &
                " enti.Email,enti.Datacontrolloemail,enti.emailvalido,enti.NoteRichiestaRegistrazione," &
                " enti.TelefonoRichiestaRegistrazione," &
                " enti.IdSezione," &
                " enti.PrefissoTelefonoRichiestaRegistrazione,enti.PrefissoFax,enti.Fax,entipassword.Username,e.dataultimaClasseaccreditamento," &
                " enti.idclasseaccreditamentorichiesta,enti.idclasseaccreditamento,isnull(e.CodiceFiscaleArchivio,'') as CodiceFiscaleArchivio,e.PEC,enti.Firma,enti.EmailCertificata, " &
                " isnull(e.StatoPec,0) as StatoPec,isnull(CausaliPECNegative.Causale,'') as CausalePec, E.ALBO ,  isnull(t.privato,2) as privato," &
                " enti.DataNominaRL,enti.AttivitaUltimiTreAnni, enti.AttivitaFiniIstituzionali, enti.AttivitaSenzaLucro, enti.AltroTipoEnte,enti.RapportoAnnuale" &
                " FROM  Accreditamento_VariazioneEnti enti " &
                " INNER JOIN enti e on enti.idente = e.idente and enti.StatoVariazione=0" &
                " INNER JOIN classiaccreditamento on (classiaccreditamento.idclasseaccreditamento=enti.idclasseaccreditamento)" &
                " INNER JOIN statienti on (statienti.idstatoente=e.idstatoente)" &
                " LEFT JOIN entipassword on (enti.idente=entipassword.idente)" &
                " LEFT JOIN CausaliPECNegative on e.IdCAusalePecNegativa = CausaliPECNegative.IdCAusalePecNegativa" &
                " left join classiaccreditamento as classiaccreditamentorichieste on (classiaccreditamento.idclasseaccreditamento=enti.idclasseaccreditamentorichiesta)" &
                " LEFT JOIN tipologieenti t on enti.tipologia = t.descrizione" &
                " WHERE enti.idente=" & Session("IdEnte") & ""

        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If

        dtrgenerico = ClsServer.CreaDatareader(strsql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
        dtrgenerico.Read()

        'Aggiunto da Andrea Mondello il 26/04/2021 atto di nomina del Rappreesentante Legale inizio
        txtDataNominaRL.Text = IIf(Not IsDBNull(dtrgenerico("DataNominaRL")), dtrgenerico("DataNominaRL"), "")
        'Aggiunto da Andrea Mondello il 26/04/2021 atto di nomina del Rappreesentante Legale fine

        If Not IsDBNull(dtrgenerico("StatoEnte")) Then
            lblStato.Text = dtrgenerico("StatoEnte")
        End If

        If Not IsDBNull(dtrgenerico("AttivitaSenzaLucro")) Then
            chkSenzaLucro = dtrgenerico("AttivitaSenzaLucro")
        End If

        If Not IsDBNull(dtrgenerico("AltroTipoEnte")) Then
            AltroTipoEnte = dtrgenerico("AltroTipoEnte")
        End If

        If Not IsDBNull(dtrgenerico("RapportoAnnuale")) Then
            RapportAnnuale = dtrgenerico("RapportoAnnuale")
        End If

        If Not IsDBNull(dtrgenerico("AttivitaUltimiTreAnni")) Then
            If dtrgenerico("AttivitaUltimiTreAnni") = True Then
                chkAttivita.Checked = True
            Else
                chkAttivita.Checked = False
            End If
        End If

        If Not IsDBNull(dtrgenerico("AttivitaFiniIstituzionali")) Then
            If dtrgenerico("AttivitaFiniIstituzionali") = True Then
                chkFiniIstituzionali.Checked = True
            Else
                chkFiniIstituzionali.Checked = False
            End If
        End If

       

        If RapportAnnuale Then
            chkRapportoAnnuale.Checked = True
        Else
            chkRapportoAnnuale.Checked = False
        End If


        'Antonello--------------------------------------
        ''carico combo tipologia in relazione allo stato
        AlboEnte = dtrgenerico("ALBO")
        privato = dtrgenerico("privato")
        '------------------------------------------------
        If Not IsDBNull(dtrgenerico("Tipologia")) Then
            Tipologia = dtrgenerico("Tipologia")
        End If
        txtCodFis.Text = IIf(Not IsDBNull(dtrgenerico("CodiceFiscale")), dtrgenerico("CodiceFiscale"), "")
        lblCodRegione.Text = IIf(Not IsDBNull(dtrgenerico("CodiceRegione")), dtrgenerico("CodiceRegione"), "")
        If Not IsDBNull(dtrgenerico("Datacontrolloemail")) Then
            txtdataControlloEmail.Text = dtrgenerico("ggDCemail") & "/" & IIf(CInt(dtrgenerico("monthDCemail")) < 10, "0" & dtrgenerico("monthDCemail"), dtrgenerico("monthDCemail")) & "/" & dtrgenerico("YearDCemail")
        End If
        If Not IsDBNull(dtrgenerico("Datacontrollohttp")) Then
            txtdataControllohttp.Text = dtrgenerico("ggDChttp") & "/" & IIf(CInt(dtrgenerico("monthDChttp")) < 10, "0" & dtrgenerico("monthDChttp"), dtrgenerico("monthDChttp")) & "/" & dtrgenerico("yearDChttp")
        End If
        'Aggiunto da Alessandra Taballione il 28/02/2005
        'Implementazione del campo data di costituzione Ente
        txtDataCostituzione.Text = IIf(Not IsDBNull(dtrgenerico("dataCostituzione")), dtrgenerico("DataCostituzione"), "")
        'Aggiunto da Alessandra Taballione il 16/05/2005
        'Implementazione del campo data Richiesta Ente
        txtDataRicezioneCartacea.Text = Left(IIf(Not IsDBNull(dtrgenerico("DataRicezioneCartacea")), dtrgenerico("DataRicezioneCartacea"), ""), 10)
        If (Session("TipoUtente") <> "U" And Session("TipoUtente") <> "R") Then
            txtDataRicezioneCartacea.ReadOnly = True
            txtDataRicezioneCartacea.BackColor = Color.Gainsboro
        End If
        txtdenominazione.Text = IIf(Not IsDBNull(dtrgenerico("Denominazione")), dtrgenerico("Denominazione"), "")
        txtEstremiDSG.Text = IIf(Not IsDBNull(dtrgenerico("EstremiDeliberaStrutturaGestione")), dtrgenerico("EstremiDeliberaStrutturaGestione"), "")
        If Not IsDBNull(dtrgenerico("http")) Then
            txthttp.Text = dtrgenerico("http")
            If Not IsDBNull(dtrgenerico("Datacontrollohttp")) Then
                If dtrgenerico("httpvalido") = True Then
                    chkVerohttp.Checked = True
                    chkFalsohttp.Checked = False
                Else
                    chkVerohttp.Checked = False
                    chkFalsohttp.Checked = True
                End If
            Else
                chkVerohttp.Checked = False
                chkFalsohttp.Checked = False
            End If
        End If
        '-----------------------Antonello -------- Carico Pac e Firma--------
        If Not IsDBNull(dtrgenerico("PEC")) Then
            If dtrgenerico("PEC") = 0 Then
                ChkPec.Value = "False"
            Else
                ChkPec.Value = "True"
            End If
        End If
        If Not IsDBNull(dtrgenerico("Firma")) Then
            If dtrgenerico("Firma") = 0 Then
                ChkFirma.Checked = False
                ChkFirmaPrima.Value = "False"
            Else
                ChkFirma.Checked = True
                ChkFirmaPrima.Value = "True"
            End If
        End If
        If Not IsDBNull(dtrgenerico("EmailCertificata")) Then
            txtEmailpec.Text = dtrgenerico("EmailCertificata")
        Else
            txtEmailpec.Text = ""
        End If
        '**** ****
        If txtEmailpec.Text = "" Then
            imgStatoPec.Visible = False
        Else
            If Session("TipoUtente") = "E" Then
                imgStatoPec.Visible = False
            Else
                'imgStatoPec.Visible = True
                Select Case dtrgenerico("StatoPec")
                    Case 0 'davalutare
                        'imgStatoPec.ImageUrl = "images/PecDaValutare.png"
                        imgStatoPec.ToolTip = "PEC da verificare"
                    Case 1 'valida
                        'imgStatoPec.ImageUrl = "images/PecValida.png"
                        imgStatoPec.ToolTip = "PEC verificata"
                    Case 2 'non valida
                        'imgStatoPec.ImageUrl = "images/PecNegativa.png"
                        imgStatoPec.ToolTip = "PEC non valida: " & dtrgenerico("CausalePec")
                End Select
            End If
        End If

        '**********
        '---------------------------------------------------------------------
        If Not IsDBNull(dtrgenerico("Email")) Then
            txtemail.Text = dtrgenerico("Email")
            If Not IsDBNull(dtrgenerico("Datacontrolloemail")) Then
                If dtrgenerico("emailvalido") = True Then
                    chkVeroEmail.Checked = True
                    chkFalsoEmail.Checked = False
                Else
                    chkVeroEmail.Checked = False
                    chkFalsoEmail.Checked = True
                End If
            Else
                chkVeroEmail.Checked = False
                chkFalsoEmail.Checked = False
            End If
        End If
        'Modificato da Alessandra Taballione il 09/06/2004
        'non verrà piu inserito il numero delle sedi ma ci sarà una tabella
        'che conterrà le informazioni relative alla classe Richiesta
        'CodiceFiscaleArchivio, isnull(enti.PartitaIVAArchivio) as PartitaIVAArchivio
        txtCodFisArchiviato.Value = dtrgenerico("CodiceFiscaleArchivio")
        'antonello-------------
        'txtPartitaIvaArchiviata.Text = dtrgenerico("PartitaIVAArchivio")
        'txtPartitaIva.Text = IIf(Not IsDBNull(dtrgenerico("PartitaIva")), dtrgenerico("PartitaIva"), "")
        '------------------------------------------------------------------------------------
        txtRichiedente.Text = IIf(Not IsDBNull(dtrgenerico("NoteRichiestaRegistrazione")), dtrgenerico("NoteRichiestaRegistrazione"), "")
        txtTelefono.Text = IIf(Not IsDBNull(dtrgenerico("TelefonoRichiestaRegistrazione")), dtrgenerico("TelefonoRichiestaRegistrazione"), "")
        txtprefisso.Text = IIf(Not IsDBNull(dtrgenerico("PrefissoTelefonoRichiestaRegistrazione")), dtrgenerico("PrefissoTelefonoRichiestaRegistrazione"), "")
        '**********************BLOCCO FAX**********************************************
        txtprefissofax.Text = IIf(Not IsDBNull(dtrgenerico("PrefissoFax")), dtrgenerico("PrefissoFax"), "")
        txtFax.Text = IIf(Not IsDBNull(dtrgenerico("Fax")), dtrgenerico("Fax"), "")
        '**********************FINE BLOCCO FAX**********************************************
        txtUtenza.Text = IIf(Not IsDBNull(dtrgenerico("Username")), dtrgenerico("Username"), "")
        lblClasse.Text = dtrgenerico("classeaccreditamento")
        'le classi non esistono piu'
        If lblClasse.Text = "Nessuna Classe" Then lblClasse.Text = "Nessuna Sezione"
        lblIdClasse.Value = dtrgenerico("idclasseaccreditamento")
        lblDataClasse.Text = Mid(IIf(Not IsDBNull(dtrgenerico("dataultimaClasseaccreditamento")), dtrgenerico("dataultimaClasseaccreditamento"), ""), 1, 10) 'dtrgenerico("dataultimaClasseaccreditamento")
        lblInPartenariato.Value = dtrgenerico("EntiInPartenariato")
        txtidClasseRichiesta.Value = dtrgenerico("idclasseaccreditamentorichiesta")
        If Not IsDBNull(dtrgenerico("idclasseaccreditamentorichiesta")) And dtrgenerico("idclasseaccreditamentorichiesta") <> 6 Then
            If Not IsDBNull(dtrgenerico("idsezione")) Then
                ddlClassi.SelectedValue = dtrgenerico("idsezione")
            End If
            'ddlClassi.SelectedValue = dtrgenerico("idclasseaccreditamentorichiesta")
            'metto in sessione l'id della classe dell'ente selezionato
            'per controllare il change della combo delle classi
            'controllo aggiunto da BagaJon il 7/3/2006
            Session("JonIdClasseAccreditamento") = dtrgenerico("idclasseaccreditamentorichiesta")
            txtidClasseRichiesta.Value = dtrgenerico("idclasseaccreditamentorichiesta")
            'Modificato da Alessandra Taballione il 09/06/2004
            'implementazione delle informazioni della classe richiesta
            If Not dtrgenerico Is Nothing Then
                dtrgenerico.Close()
                dtrgenerico = Nothing
            End If
            dtrgenerico = ClsServer.CreaDatareader("Select * from classiaccreditamento where idclasseaccreditamento=" & txtidClasseRichiesta.Value & "", IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
            dtrgenerico.Read()
            If dtrgenerico.HasRows = True Then
                lblmaxsedi.Text = dtrgenerico("maxsediTesto")
                lblminSedi.Text = dtrgenerico("minSediTesto")
                lblmaxVol.Text = dtrgenerico("MaxEntitàperannoTesto")
                lblminVol.Text = dtrgenerico("MinEntitàperannoTesto")
            End If
        End If

        If Tipologia <> "" Then
            If AlboEnte = "SCN" Then
                If privato = -1 Then
                    ddlTipologia.SelectedIndex = 2
                    ddlGiuridica.Visible = False
                    Label6.Visible = False
                Else
                    ddlTipologia.SelectedIndex = 1
                    ddlGiuridica.Visible = True
                    Label6.Visible = True
                    ddlGiuridica.SelectedIndex = trovaindex(Tipologia, ddlGiuridica)
                    txtpianolocale.Value = ddlGiuridica.SelectedItem.Text
                End If
            Else
                Select Case privato
                    Case 0 'PUBBLICO
                        ddlTipologia.SelectedIndex = 1
                    Case -1 'PRIVATO
                        ddlTipologia.SelectedIndex = 2
                    Case 2 'NULL
                        ddlTipologia.SelectedIndex = 0
                End Select

                If ddlTipologia.SelectedValue = "Privato" Then
                    If chkSenzaLucro IsNot Nothing Then
                        divSenzaScopoLucro.Visible = True
                        If (chkSenzaLucro) Then
                            chkSenzaScopoLucro.Checked = True
                        Else
                            chkSenzaScopoLucro.Checked = False
                        End If
                    End If
                Else
                    divSenzaScopoLucro.Visible = False
                End If

                CaricaDettaglioTipologiaEntiSCU(Tipologia, "POPOLAMASCHERA")
                ddlGiuridica.Visible = True
                Label6.Visible = True
                ddlGiuridica.SelectedIndex = trovaindex(Tipologia, ddlGiuridica)
                txtpianolocale.Value = ddlGiuridica.SelectedItem.Text
            End If
        End If



        'Aggiunto da Alesssandra Taballione il 18/05/2005
        'Implementazione del tasto Ripristina 
        'Visibile solo unsc e solo se ente sospeso 
        'Cambio stato da sospeso a registrato
        'con clonologia
        If (Session("TipoUtente") = "U" Or Session("TipoUtente") = "R") Then
            strsql = "select * from statienti " &
            " inner join enti on enti.idstatoente=statienti.idstatoente " &
            " where idente = " & Session("idEnte") & " And sospeso = 1 "
            If Not dtrgenerico Is Nothing Then
                dtrgenerico.Close()
                dtrgenerico = Nothing
            End If
            dtrgenerico = ClsServer.CreaDatareader(strsql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
            If Not dtrgenerico Is Nothing Then
                dtrgenerico.Close()
                dtrgenerico = Nothing
            End If
        End If
        'Aggiunto da Alessandra Taballione il 11/11/2005
        'Ricerca e popolamento dei dati relativi alla sede principale dell'Ente
        strsql = " SELECT entisedi.identesede,entisedi.idcomune,entisedi.indirizzo,entisedi.DettaglioRecapito," &
                 "  entisedi.civico,entisedi.cap,comuni.denominazione as comune,provincie.provincia, provincie.idprovincia, provincie.ProvinceNazionali " &
                 " FROM Accreditamento_VariazioneEnti entisedi " &
                 " INNER JOIN comuni ON entisedi.IDComune = comuni.IDComune " &
                 " INNER JOIN provincie ON comuni.IDProvincia = provincie.IDProvincia " &
                 " WHERE entisedi.IDEnte = " & Session("IdEnte") & " And statovariazione = 0"
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
        dtrgenerico = ClsServer.CreaDatareader(strsql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
        If dtrgenerico.HasRows = True Then
            dtrgenerico.Read()
            txtIndirizzo.Text = dtrgenerico("Indirizzo")
            'Antonello--------------------

            TxtDettaglioRecapito.Text = IIf(Not IsDBNull(dtrgenerico("DettaglioRecapito")), dtrgenerico("DettaglioRecapito"), "")
            txtCivico.Text = dtrgenerico("civico")
            txtCAP.Text = dtrgenerico("cap")
            txtIdSede.Value = dtrgenerico("identeSede")
            txtIDComunes.Value = dtrgenerico("idcomune")
            'txtCity.Visible = True
            Session("IdComune") = dtrgenerico("idcomune")
            ChkEstero.Checked = Not CBool(dtrgenerico("ProvinceNazionali"))
            If ChkEstero.Checked = True Then
                lblComune.Text = "Località"
                lblCAP.Text = "Codice località"
            Else
                lblComune.Text = "Comune"
                lblCAP.Text = "CAP"
            End If
            Dim idProvincia As String = dtrgenerico("idprovincia")
            If Not dtrgenerico Is Nothing Then
                dtrgenerico.Close()
                dtrgenerico = Nothing
            End If
            selComune.CaricaProvincie(ddlProvincia, ChkEstero.Checked, Session("Conn"))
            If Not dtrgenerico Is Nothing Then
                dtrgenerico.Close()
                dtrgenerico = Nothing
            End If
            If Not ddlProvincia.SelectedValue Is Nothing Then
                ddlProvincia.SelectedValue = idProvincia
            End If
            If Not Session("IdComune") Is Nothing Then
                selComune.CaricaComuniDaProvincia(ddlComune, idProvincia, Session("Conn"))
                ddlComune.SelectedValue = Session("IdComune")

            End If
        End If
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If

        'Carico gli allegati salvati e li metto in sessione
        CaricaEntiAllegatiVariazioni()

        'richiamo sub per caricare i Settori associati all'Ente
        CaricaEntiSettoriVariazione()

    End Sub

    Private Sub MessaggiAlert(ByVal strMessaggio)
        'Realizzata da Alessandra Taballione 26/02/04
        'Private sub per la gestione dell'immagine e del messaggio
        'per eventuali comunicazione all'Utente

        lblMessaggio.ForeColor = Color.Red
        lblMessaggio.Text = strMessaggio
        Exit Sub
    End Sub

    Private Function trovaindex(ByVal stringa As String, ByVal obj As Object)
        'Generata da Alessandra Taballione il 26/02/04
        'Ricerca nella DropDownList passata l'index Corrispondente al testo in essa selezionato.
        Dim i As Integer
        For i = 0 To obj.Items.Count - 1
            If UCase(obj.Items(i).Text) = UCase(stringa) Then
                trovaindex = i
                Exit For
            End If
        Next
    End Function
    Sub CaricaEntiSettori()
        'aggiunto il 19/08/2014 da Simona Cordella
        'Dim dtrSettori As SqlClient.SqlDataReader
        Dim blnAbilitaSettori As Boolean
        Dim EsperienzeAreeSettore As New List(Of clsEsperienzeAree)
        Dim dtSettori As New DataTable

        Session("ElencoSettoriDB") = Nothing
        dtSettori = ElencoSettori()
        Session("ElencoSettoriDB") = dtSettori

        SettoriEsperienzeDB.Clear()

        blnAbilitaSettori = AbilitaSettori()

        'rendo tutti i cmdinserisci invisibili, poi vengono abilitati dopo se serve
        For Each gRow As GridViewRow In dtgSettori.Rows
            Dim cmdInserisci As Button = DirectCast(gRow.FindControl("cmdModifica"), Button)
            If Not cmdInserisci Is Nothing Then
                cmdInserisci.Visible = False
            End If
        Next

        If dtSettori IsNot Nothing AndAlso dtSettori.Rows.Count > 0 Then 'Esistono settori legate ad esperienze quindi le carico in maschera
            For Each dr As DataRow In dtSettori.Rows
                Dim esperienzaArea As New clsEsperienzeAree
                Dim esperienze(2) As String
                Dim anniEsperienze(2) As Integer
                If IsDBNull(dr("IDSETTORE")) Then
                    For Each gRow As GridViewRow In dtgSettori.Rows
                        Dim check As CheckBox = DirectCast(gRow.FindControl("chkSeleziona"), CheckBox)
                        Dim HFIdMacroAttivita As HiddenField = DirectCast(gRow.FindControl("HFIdMacroAttivita"), HiddenField)
                        Dim cmdInserisci As Button = DirectCast(gRow.FindControl("cmdModifica"), Button)
                        If dr("IDMACROATTIVITA").ToString() = HFIdMacroAttivita.Value Then
                            check.Checked = True
                            ChkChange(check, New EventArgs)
                            cmdInserisci.Visible = If(dr("NuovoInserimento") = "0", False, True)
                        End If
                        If blnAbilitaSettori = False Then
                            check.Enabled = blnAbilitaSettori
                        End If
                    Next
                    Continue For
                End If
                For i = 0 To 2 'Anni e Esperienza
                    Select Case i
                        Case 0
                            esperienze(i) = dr("I_ESPERIENZA").ToString()
                            anniEsperienze(i) = CInt(dr("I_ANNOESPERIENZA").ToString())
                        Case 1
                            esperienze(i) = dr("II_ESPERIENZA").ToString()
                            anniEsperienze(i) = CInt(dr("II_ANNOESPERIENZA").ToString())
                        Case 2
                            esperienze(i) = dr("III_ESPERIENZA").ToString()
                            anniEsperienze(i) = CInt(dr("III_ANNOESPERIENZA").ToString())
                    End Select
                Next
                esperienzaArea.IdSettore = CInt(dr("IDMACROATTIVITA").ToString())
                esperienzaArea.AnnoEsperienza = anniEsperienze.ToList()
                esperienzaArea.DescrizioneEsperienza = esperienze.ToList()
                esperienzaArea.AreeSelezionate = dr("AREEINTERVENTO").ToString().Split(",").ToList()

                EsperienzeAreeSettore.Add(esperienzaArea)
                Session("EsperienzeAreeSettore") = EsperienzeAreeSettore

                For Each gRow As GridViewRow In dtgSettori.Rows
                    Dim check As CheckBox = DirectCast(gRow.FindControl("chkSeleziona"), CheckBox)
                    Dim HFIdMacroAttivita As HiddenField = DirectCast(gRow.FindControl("HFIdMacroAttivita"), HiddenField)
                    Dim cmdInserisci As Button = DirectCast(gRow.FindControl("cmdModifica"), Button)
                    If dr("IDMACROATTIVITA").ToString() = HFIdMacroAttivita.Value Then
                        check.Checked = True
                        ChkChange(check, New EventArgs)
                        cmdInserisci.Visible = If(dr("NuovoInserimento") = "0", False, True)  'possono essere editate SOLO esperienze/aree inserite in una fase aperta di iscrizione
                        hfRigaSelezionata.Value = gRow.RowIndex
                        AggiungiEsperienzaAmbiti()
                    End If

                    If blnAbilitaSettori = False Then
                        check.Enabled = blnAbilitaSettori
                    End If

                Next
            Next

        End If

    End Sub

    Function ElencoSettori() As DataTable
        'chiamata dalla CaricaEntiSettori, legge da db i settori dalle tabelle reali indicando se sono nuovi settori (inseriti nella eventuale fase di iscrizione ancora aperta)
        'sostituisce la query scolpita a codice presente precedentemente

        Dim sqlDAP As New SqlClient.SqlDataAdapter
        Dim dataTable As New DataTable
        Dim strNomeStore As String = "[SP_ACCREDITAMENTO_ELENCO_SETTORI]"


        Try
            sqlDAP = New SqlClient.SqlDataAdapter(strNomeStore, CType(Session("conn"), SqlClient.SqlConnection))
            sqlDAP.SelectCommand.CommandType = CommandType.StoredProcedure

            If Session("IdEnte") IsNot Nothing AndAlso Session("IdEnte") <> "-1" Then
                sqlDAP.SelectCommand.Parameters.Add("@idente", SqlDbType.Int).Value = Integer.Parse(Session("IdEnte"))
            Else
                MessaggiAlert("Ente non selezionato")
                Exit Function
            End If

            sqlDAP.Fill(dataTable)

        Catch ex As Exception
            MessaggiAlert("Errore nel recupero informazioni")
            Exit Function
        End Try

        Return dataTable
    End Function

    Function ElencoSettoriEsperienzePrecedenti() As DataTable


        Dim sqlDAP As New SqlClient.SqlDataAdapter
        Dim dataTable As New DataTable
        Dim strNomeStore As String = "[SP_ACCREDITAMENTO_GET_SETTORI_ESPERIENZE_PREC]"

        Try
            sqlDAP = New SqlClient.SqlDataAdapter(strNomeStore, CType(Session("conn"), SqlClient.SqlConnection))
            sqlDAP.SelectCommand.CommandType = CommandType.StoredProcedure

            If Session("IdEnte") IsNot Nothing AndAlso Session("IdEnte") <> "-1" Then
                sqlDAP.SelectCommand.Parameters.Add("@idente", SqlDbType.Int).Value = Integer.Parse(Session("IdEnte"))
            Else
                MessaggiAlert("Ente non selezionato")
                Exit Function
            End If

            sqlDAP.Fill(dataTable)

        Catch ex As Exception
            MessaggiAlert("Errore nel recupero informazioni")
            Exit Function
        End Try

        Return dataTable
    End Function


    Function ElencoVariazioniSettori() As DataTable
        'chiamata dalla CaricaEntiSettoriVariazione, legge da db le variazioni settori in corso indicando se sono nuovi settori (inseriti nella fase attuale)
        'sostituisce la query scolpita a codice presente precedentemente

        Dim sqlDAP As New SqlClient.SqlDataAdapter
        Dim dataTable As New DataTable
        Dim strNomeStore As String = "[SP_ACCREDITAMENTO_ELENCO_VARIAZIONI_SETTORI]"


        Try
            sqlDAP = New SqlClient.SqlDataAdapter(strNomeStore, CType(Session("conn"), SqlClient.SqlConnection))
            sqlDAP.SelectCommand.CommandType = CommandType.StoredProcedure

            If Session("IdEnte") IsNot Nothing AndAlso Session("IdEnte") <> "-1" Then
                sqlDAP.SelectCommand.Parameters.Add("@idente", SqlDbType.Int).Value = Integer.Parse(Session("IdEnte"))
            Else
                MessaggiAlert("Ente non selezionato")
                Exit Function
            End If

            sqlDAP.Fill(dataTable)

        Catch ex As Exception
            MessaggiAlert("Errore nel recupero informazioni")
            Exit Function
        End Try

        Return dataTable
    End Function

    Sub CaricaEntiSettoriVariazione()

        'aggiunto il 19/08/2014 da Simona Cordella
        'Dim dtrSettori As SqlClient.SqlDataReader
        Dim blnAbilitaSettori As Boolean
        Dim EsperienzeAreeSettore As New List(Of clsEsperienzeAree)
        Dim dtSettori As New DataTable

        Session("ElencoSettoriDB") = Nothing
        dtSettori = ElencoVariazioniSettori()
        Session("ElencoSettoriDB") = dtSettori

        SettoriEsperienzeDB.Clear()

        blnAbilitaSettori = AbilitaSettori()

        'rendo tutti i cmdinserisci invisibili, poi vengono abilitati dopo se serve
        For Each gRow As GridViewRow In dtgSettori.Rows
            Dim cmdInserisci As Button = DirectCast(gRow.FindControl("cmdModifica"), Button)
            If Not cmdInserisci Is Nothing Then
                cmdInserisci.Visible = False
            End If
        Next


        If dtSettori IsNot Nothing And dtSettori.Rows.Count > 0 Then 'Esistono settori legate ad esperienze quindi le carico in maschera

            For Each dr As DataRow In dtSettori.Rows
                Dim esperienzaArea As New clsEsperienzeAree
                Dim esperienze(2) As String
                Dim anniEsperienze(2) As Integer

                If IsDBNull(dr("IDSETTORE")) Then
                    For Each gRow As GridViewRow In dtgSettori.Rows
                        Dim check As CheckBox = DirectCast(gRow.FindControl("chkSeleziona"), CheckBox)
                        Dim HFIdMacroAttivita As HiddenField = DirectCast(gRow.FindControl("HFIdMacroAttivita"), HiddenField)
                        Dim cmdInserisci As Button = DirectCast(gRow.FindControl("cmdModifica"), Button)
                        If dr("IDMACROATTIVITA").ToString() = HFIdMacroAttivita.Value Then
                            check.Checked = True
                            ChkChange(check, New EventArgs)
                            cmdInserisci.Visible = If(dr("NuovoInserimento") = "0", False, True)
                        End If

                        If blnAbilitaSettori = False Then
                            check.Enabled = blnAbilitaSettori
                        End If

                    Next
                    Continue For
                End If
                For i = 0 To 2 'Anni e Esperienza
                    Select Case i
                        Case 0
                            esperienze(i) = dr("I_ESPERIENZA").ToString()
                            anniEsperienze(i) = CInt(dr("I_ANNOESPERIENZA").ToString())
                        Case 1
                            esperienze(i) = dr("II_ESPERIENZA").ToString()
                            anniEsperienze(i) = CInt(dr("II_ANNOESPERIENZA").ToString())
                        Case 2
                            esperienze(i) = dr("III_ESPERIENZA").ToString()
                            anniEsperienze(i) = CInt(dr("III_ANNOESPERIENZA").ToString())
                    End Select
                Next
                esperienzaArea.IdSettore = CInt(dr("IDSETTORE").ToString())
                esperienzaArea.AnnoEsperienza = anniEsperienze.ToList()
                esperienzaArea.DescrizioneEsperienza = esperienze.ToList()
                esperienzaArea.AreeSelezionate = dr("AREEINTERVENTO").ToString().Split(",").ToList()

                EsperienzeAreeSettore.Add(esperienzaArea)
                Session("EsperienzeAreeSettore") = EsperienzeAreeSettore

                For Each gRow As GridViewRow In dtgSettori.Rows
                    Dim check As CheckBox = DirectCast(gRow.FindControl("chkSeleziona"), CheckBox)
                    Dim HFIdMacroAttivita As HiddenField = DirectCast(gRow.FindControl("HFIdMacroAttivita"), HiddenField)
                    Dim cmdInserisci As Button = DirectCast(gRow.FindControl("cmdModifica"), Button)
                    If dr("IDSETTORE").ToString() = HFIdMacroAttivita.Value Then
                        check.Checked = True
                        ChkChange(check, New EventArgs)
                        cmdInserisci.Visible = If(dr("NuovoInserimento") = "0", False, True)  'possono essere editate SOLO esperienze/aree inserite in questo adeguamento
                        hfRigaSelezionata.Value = gRow.RowIndex
                        AggiungiEsperienzaAmbiti()
                    Else
                        'cmdInserisci.Visible = False
                    End If

                    If blnAbilitaSettori = False Then
                        check.Enabled = blnAbilitaSettori
                    End If
                Next
            Next
        End If

        ''aggiunto il 20/08/2014 da Simona Cordella
        'Dim dtrSettori As SqlClient.SqlDataReader
        ''variabile stringa locale per costruire la query per le aree
        'Dim strSql As String

        'If Not dtrSettori Is Nothing Then
        '    dtrSettori.Close()
        '    dtrSettori = Nothing
        'End If
        'strSql = "SELECT Settore_A,Settore_B,Settore_C,Settore_D,Settore_E,Settore_F, isnull(Settore_G,0) as Settore_G FROM Accreditamento_VariazioneEnti WHERE IdEnte = " & Session("IdEnte") & " and statovariazione=0 "
        'dtrSettori = ClsServer.CreaDatareader(strSql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))

        'If dtrSettori.HasRows = True Then
        '    dtrSettori.Read()

        '    'SETTORI INTERVENTO
        '    Dim check As CheckBox
        '    check = dtgSettori.Rows.Item(0).FindControl("chkSeleziona")
        '    check.Checked = dtrSettori("Settore_A")
        '    check = dtgSettori.Rows.Item(1).FindControl("chkSeleziona")
        '    check.Checked = dtrSettori("Settore_B")
        '    check = dtgSettori.Rows.Item(2).FindControl("chkSeleziona")
        '    check.Checked = dtrSettori("Settore_C")
        '    check = dtgSettori.Rows.Item(3).FindControl("chkSeleziona")
        '    check.Checked = dtrSettori("Settore_D")
        '    check = dtgSettori.Rows.Item(4).FindControl("chkSeleziona")
        '    check.Checked = dtrSettori("Settore_E")

        '    If dtgSettori.Rows.Count = 7 Then 'SCU
        '        check = dtgSettori.Rows.Item(5).FindControl("chkSeleziona")
        '        check.Checked = dtrSettori("Settore_F")
        '        check = dtgSettori.Rows.Item(6).FindControl("chkSeleziona")
        '        check.Checked = dtrSettori("Settore_G")
        '    Else
        '        check = dtgSettori.Rows.Item(5).FindControl("chkSeleziona")
        '        check.Checked = dtrSettori("Settore_G")
        '    End If

        'End If
        'If Not dtrSettori Is Nothing Then
        '    dtrSettori.Close()
        '    dtrSettori = Nothing
        'End If
    End Sub

    Private Sub ddlClassi_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlClassi.SelectedIndexChanged

        strsql = "SELECT * FROM CLASSIACCREDITAMENTO "
        strsql += "WHERE IDCLASSEACCREDITAMENTO = "
        strsql += "(SELECT IDCLASSEACCREDITAMENTO FROM SEZIONIALBOSCU WHERE IDSEZIONE =" & ddlClassi.SelectedValue & ") "
        Session("IDClasseAccreditamento") = Nothing

        dtrgenerico = ClsServer.CreaDatareader(strsql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))

        dtrgenerico.Read()
        If dtrgenerico.HasRows = True Then
            Session("IDClasseAccreditamento") = dtrgenerico("IDClasseAccreditamento")
            lblmaxsedi.Text = dtrgenerico("maxsediTesto")
            lblminSedi.Text = dtrgenerico("minSediTesto")
            lblmaxVol.Text = dtrgenerico("MaxEntitàperannoTesto")
            lblminVol.Text = dtrgenerico("MinEntitàperannoTesto")
        Else
            lblmaxsedi.Text = ""
            lblminSedi.Text = ""
            lblmaxVol.Text = ""
            lblminVol.Text = ""
            Session("IDClasseAccreditamento") = Nothing
        End If
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
    End Sub

    Private Sub ddlTipologia_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlTipologia.SelectedIndexChanged
        Dim AlboEnte As String
        Dim Obbligatorio As String = "(*)"
        AlboEnte = ClsUtility.TrovaAlboEnte(Session("IdEnte"), Session("Conn"))
        If AlboEnte = "SCN" Then
            If ddlTipologia.SelectedValue.ToUpper = "PUBBLICO" Then
                ddlGiuridica.Visible = True
                Label6.Visible = True
                rowNoDeliberaAdesione.Visible = True
                lblDeliberaAdesione.Text = Obbligatorio & lblDeliberaAdesione.Text
                If lblStatutoEnte.Text.IndexOf(Obbligatorio) <> -1 Then
                    lblStatutoEnte.Text = Mid(lblStatutoEnte.Text, Obbligatorio.Length + 1, lblStatutoEnte.Text.Length)
                End If
                If lblAttoCostitutivoEnte.Text.IndexOf(Obbligatorio) <> -1 Then
                    lblAttoCostitutivoEnte.Text = Mid(lblAttoCostitutivoEnte.Text, Obbligatorio.Length + 1, lblAttoCostitutivoEnte.Text.Length)
                End If
                divSenzaScopoLucro.Visible = False
                If ddlGiuridica.SelectedItem IsNot Nothing Then
                    If ddlGiuridica.SelectedItem.Text <> String.Empty AndAlso ddlGiuridica.SelectedItem.Text Like "Altro*" Then
                        divAltraTipologia.Visible = True
                    Else
                        divAltraTipologia.Visible = False
                    End If
                End If
            ElseIf ddlTipologia.SelectedValue.ToUpper = "PRIVATO" Then
                If Session("LoadedDeliberaAdesione") Is Nothing Then
                    rowNoDeliberaAdesione.Visible = False
                Else
                    rowNoDeliberaAdesione.Visible = False
                    rowDeliberaAdesione.Visible = False
                    Session("LoadedDeliberaAdesione") = Nothing
                End If
                divAltraTipologia.Visible = False
                txtAltraTipoEnte.Text = String.Empty
                divSenzaScopoLucro.Visible = True
                ddlGiuridica.Visible = False
                Label6.Visible = False
                rowNoDeliberaAdesione.Visible = False
                lblAttoCostitutivoEnte.Text = Obbligatorio & lblAttoCostitutivoEnte.Text
                lblStatutoEnte.Text = Obbligatorio & lblStatutoEnte.Text
                If lblDeliberaAdesione.Text.IndexOf(Obbligatorio) <> -1 Then
                    lblDeliberaAdesione.Text = Mid(lblDeliberaAdesione.Text, Obbligatorio.Length + 1, lblDeliberaAdesione.Text.Length)
                End If
            Else
                ddlGiuridica.Visible = False
                divAltraTipologia.Visible = False
                txtAltraTipoEnte.Text = String.Empty
                Label6.Visible = False
            End If

        Else
            If ddlTipologia.SelectedValue.ToUpper = "PUBBLICO" Then
                rowNoDeliberaAdesione.Visible = True
                ddlGiuridica.Visible = True
                Label6.Visible = True
                lblDeliberaAdesione.Text = Obbligatorio & lblDeliberaAdesione.Text
                If lblStatutoEnte.Text.IndexOf(Obbligatorio) <> -1 Then
                    lblStatutoEnte.Text = Mid(lblStatutoEnte.Text, Obbligatorio.Length + 1, lblStatutoEnte.Text.Length)
                End If
                If lblAttoCostitutivoEnte.Text.IndexOf(Obbligatorio) <> -1 Then
                    lblAttoCostitutivoEnte.Text = Mid(lblAttoCostitutivoEnte.Text, Obbligatorio.Length + 1, lblAttoCostitutivoEnte.Text.Length)
                End If
                CaricaDettaglioTipologiaEntiSCU(ddlTipologia.SelectedItem.Text, "COMBOTIPOLOGIA")
                divSenzaScopoLucro.Visible = False
                If ddlGiuridica.SelectedItem IsNot Nothing Then
                    If ddlGiuridica.SelectedItem.Text <> String.Empty AndAlso ddlGiuridica.SelectedItem.Text Like "Altro*" Then
                        divAltraTipologia.Visible = True
                    Else
                        divAltraTipologia.Visible = False
                    End If
                End If
            ElseIf ddlTipologia.SelectedValue.ToUpper = "PRIVATO" Then
                divAltraTipologia.Visible = False
                txtAltraTipoEnte.Text = String.Empty

                If Session("LoadedDeliberaAdesione") Is Nothing Then
                    rowNoDeliberaAdesione.Visible = False
                Else
                    rowNoDeliberaAdesione.Visible = False
                    rowDeliberaAdesione.Visible = False
                    Session("LoadedDeliberaAdesione") = Nothing
                End If
                ddlGiuridica.Visible = True
                Label6.Visible = True
                lblAttoCostitutivoEnte.Text = Obbligatorio & lblAttoCostitutivoEnte.Text
                lblStatutoEnte.Text = Obbligatorio & lblStatutoEnte.Text
                If lblDeliberaAdesione.Text.IndexOf(Obbligatorio) <> -1 Then
                    lblDeliberaAdesione.Text = Mid(lblDeliberaAdesione.Text, Obbligatorio.Length + 1, lblDeliberaAdesione.Text.Length)
                End If
                CaricaDettaglioTipologiaEntiSCU(ddlTipologia.SelectedItem.Text, "COMBOTIPOLOGIA")
                divSenzaScopoLucro.Visible = True
            Else
                ddlGiuridica.Visible = False
                divAltraTipologia.Visible = False
                txtAltraTipoEnte.Text = String.Empty
                Label6.Visible = False
            End If

        End If
        If ddlTipologia.SelectedValue = "Privato" Then
            chkSenzaScopoLucro.Visible = True
        Else
            chkSenzaScopoLucro.Checked = False
            chkSenzaScopoLucro.Visible = False
        End If
    End Sub

    Private Sub CmdSalva_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdSalva.Click

        If ValidazioneServerSalva() = True Then
            SalvaEnte()
            GestisciStyleDatiModificati()
        Else
            Log.Information(Logger.Data.LogEvent.ANAGRAFICAENTE_VALIDAZIONECAMPI, , pLog, )
        End If

    End Sub

    Private Function VerificaCheck() As Boolean
        'controllo è  stata checcato almeno un settore per il salvataggio
        VerificaCheck = False
        Dim item As DataGridItem
        For Each GridViewRow In dtgSettori.Rows
            Dim check As CheckBox = DirectCast(GridViewRow.FindControl("chkSeleziona"), CheckBox)
            If check.Checked = True Then
                VerificaCheck = True
            End If
        Next
        Return VerificaCheck
    End Function

    Private Function CalcolaData(ByVal datax As String) As String
        Dim data
        CalcolaData = ""
        'data inizio 
        data = CStr(Year(datax))
        If Len(CStr(Month(datax))) > 1 Then
            data = data & CStr(Month(datax))
        Else
            data = data & "0" & CStr(Month(datax))
        End If
        If Len(CStr(Day(datax))) > 1 Then
            data = data & CStr(Day(datax))
        Else
            data = data & "0" & Day(datax)
        End If
        CalcolaData = data
    End Function

    Private Sub ModificaEnte(ByVal AlboEnte As String,
                            ByVal IdAllegatoAN As Integer,
                            ByVal IdAllegatoDelibera As Integer,
                            ByVal IdAllegatoAttoCostitutivo As Integer,
                            ByVal IdAllegatoStatuto As Integer,
                            ByVal IdAllegatoDeliberaAdesione As Integer,
                            ByVal IDAllegatoImpegnoEtico As Integer,
                            trans As SqlClient.SqlTransaction)
        'Realizzata da : Simona Cordella
        'Creata il:        20/08/2014
        'Funzionalità: richiamo store SP_ACCREDITAMENTO_MODIFICAMASCHERA_ENTE per l'aggiornamento dell'anagrafica dell'ente
        Dim IdClasseAccreditamento As Integer
        Dim SqlCmd As New SqlClient.SqlCommand
        Try
            SqlCmd.CommandText = "SP_ACCREDITAMENTO_MODIFICAMASCHERA_ENTE"
            SqlCmd.CommandType = CommandType.StoredProcedure
            SqlCmd.Connection = Session("Conn")
            SqlCmd.Transaction = trans

            SqlCmd.Parameters.Add("@IdEnte", SqlDbType.Int).Value = Session("IdEnte")
            SqlCmd.Parameters.Add("@Denominazione", SqlDbType.VarChar).Value = txtdenominazione.Text 'denominazioneente
            'Codice Fiscale
            If Trim(txtCodFis.Text) <> "" Then
                SqlCmd.Parameters.Add("@CodiceFiscale", SqlDbType.VarChar).Value = txtCodFis.Text 'codice fiscale
            Else
                SqlCmd.Parameters.Add("@CodiceFiscale", SqlDbType.VarChar).Value = DBNull.Value 'codice fiscale
            End If
            If Trim(txtDataRicezioneCartacea.Text) <> "" Then
                SqlCmd.Parameters.Add("@DataRicezioneCartacea", SqlDbType.DateTime).Value = txtDataRicezioneCartacea.Text
            Else
                SqlCmd.Parameters.Add("@DataRicezioneCartacea", SqlDbType.DateTime).Value = DBNull.Value
            End If

            SqlCmd.Parameters.Add("@NoteRichiestaRegistrazione", SqlDbType.VarChar).Value = txtRichiedente.Text
            If AlboEnte = "SCN" Then
                If ddlTipologia.SelectedItem.Text = "Privato" Then
                    SqlCmd.Parameters.Add("@Tipologia", SqlDbType.VarChar).Value = ddlTipologia.SelectedItem.Text
                Else
                    SqlCmd.Parameters.Add("@Tipologia", SqlDbType.VarChar).Value = ddlGiuridica.SelectedItem.Text
                End If
            Else
                SqlCmd.Parameters.Add("@Tipologia", SqlDbType.VarChar, 255).Value = ddlGiuridica.SelectedItem.Text
            End If

            SqlCmd.Parameters.Add("@PrefissoTelefonoRichiestaRegistrazione", SqlDbType.VarChar).Value = txtprefisso.Text  'Prefisso telefono
            SqlCmd.Parameters.Add("@TelefonoRichiestaRegistrazione", SqlDbType.VarChar).Value = txtTelefono.Text 'telefono

            If Trim(txtprefissofax.Text) <> "" Then
                SqlCmd.Parameters.Add("@PrefissoFax", SqlDbType.VarChar).Value = txtprefissofax.Text
            Else
                SqlCmd.Parameters.Add("@PrefissoFax", SqlDbType.VarChar).Value = DBNull.Value
            End If
            'fax
            If Trim(txtFax.Text) <> "" Then
                SqlCmd.Parameters.Add("@Fax", SqlDbType.VarChar).Value = txtFax.Text
            Else
                SqlCmd.Parameters.Add("@Fax", SqlDbType.VarChar).Value = DBNull.Value
            End If

            If Trim(txtEstremiDSG.Text) <> "" Then
                SqlCmd.Parameters.Add("@EstremiDeliberaStrutturaGestione", SqlDbType.VarChar).Value = txtEstremiDSG.Text
            Else
                SqlCmd.Parameters.Add("@EstremiDeliberaStrutturaGestione", SqlDbType.VarChar).Value = DBNull.Value
            End If

            If Trim(txtDataCostituzione.Text) <> "" Then
                SqlCmd.Parameters.Add("@DataCostituzione", SqlDbType.DateTime).Value = txtDataCostituzione.Text
            Else
                SqlCmd.Parameters.Add("@DataCostituzione", SqlDbType.DateTime).Value = DBNull.Value
            End If

            If ddlClassi.SelectedValue = 0 Then
                'classe di default 
                SqlCmd.Parameters.Add("@IdClasseAccreditamentoRichiesta", SqlDbType.Int).Value = 6 'idclasseaccreditamento richiesta
                SqlCmd.Parameters.Add("@IDClasseAccreditamento", SqlDbType.Int).Value = 6 'idclasseaccreditamento
            Else
                IdClasseAccreditamento = CInt(Session("IDClasseAccreditamento").ToString())
                SqlCmd.Parameters.Add("@IdClasseAccreditamentoRichiesta", SqlDbType.Int).Value = IdClasseAccreditamento 'IdClasseAccreditamento richiesta
                SqlCmd.Parameters.Add("@IDClasseAccreditamento", SqlDbType.Int).Value = IdClasseAccreditamento 'IdClasseAccreditamento
            End If

            'Test per forzare errore stored errore inizio
            'SqlCmd.Parameters.Add("@IdClasseAccreditamentoRichiesta", SqlDbType.Int).Value = 0
            'SqlCmd.Parameters.Add("@IDClasseAccreditamento", SqlDbType.Int).Value = 0
            'Test per forzare errore stored fine

            If Trim(txthttp.Text) = "" Then
                SqlCmd.Parameters.Add("@Http", SqlDbType.VarChar).Value = DBNull.Value
            Else
                SqlCmd.Parameters.Add("@Http", SqlDbType.VarChar).Value = txthttp.Text
            End If

            If Trim(txtdataControllohttp.Text) = "" Then
                SqlCmd.Parameters.Add("@DataControlloHttp", SqlDbType.DateTime).Value = DBNull.Value 'DataControlloHttp
                SqlCmd.Parameters.Add("@HttpValido", SqlDbType.Bit).Value = 0 'HttpValido 
            Else
                Dim HttpValido As Byte
                SqlCmd.Parameters.Add("@DataControlloHttp", SqlDbType.DateTime).Value = txtdataControllohttp.Text 'DataControlloHttp

                If chkFalsohttp.Checked = True Then
                    HttpValido = 0
                End If
                If chkVerohttp.Checked = True Then
                    HttpValido = 1
                End If
                If chkFalsohttp.Checked = False And chkVerohttp.Checked = False Then
                    HttpValido = 0
                End If
                SqlCmd.Parameters.Add("@HttpValido", SqlDbType.Bit).Value = HttpValido 'HttpValido 
            End If
            If Trim(txtemail.Text) = "" Then
                SqlCmd.Parameters.Add("@Email", SqlDbType.VarChar).Value = DBNull.Value
            Else
                SqlCmd.Parameters.Add("@Email", SqlDbType.VarChar).Value = txtemail.Text
            End If
            'Data Controllo Email
            If Trim(txtdataControlloEmail.Text) = "" Then
                SqlCmd.Parameters.Add("@DataControlloEmail", SqlDbType.DateTime).Value = DBNull.Value 'DataControlloHttp
                SqlCmd.Parameters.Add("@EmailValido", SqlDbType.Bit).Value = 0 'HttpValido 
            Else
                Dim EmailValido As Byte
                SqlCmd.Parameters.Add("@DataControlloEmail", SqlDbType.DateTime).Value = txtdataControlloEmail.Text
                If chkFalsoEmail.Checked = True Then
                    EmailValido = 0
                End If
                If chkVeroEmail.Checked = True Then
                    EmailValido = 1
                End If
                If chkFalsoEmail.Checked = False And chkVeroEmail.Checked = False Then
                    EmailValido = 0
                End If
                SqlCmd.Parameters.Add("@EmailValido", SqlDbType.Bit).Value = EmailValido 'EmailValido 
            End If
            If Trim(txtEmailpec.Text) = "" Then
                SqlCmd.Parameters.Add("@EmailCertificata", SqlDbType.VarChar).Value = DBNull.Value
            Else
                SqlCmd.Parameters.Add("@EmailCertificata", SqlDbType.VarChar).Value = txtEmailpec.Text
            End If

            If ChkFirma.Checked = True Then
                SqlCmd.Parameters.Add("@Firma", SqlDbType.Bit).Value = 1
            Else
                SqlCmd.Parameters.Add("@Firma", SqlDbType.Bit).Value = 0
            End If

            'SEDE
            If txtIdSede.Value <> "" Then
                SqlCmd.Parameters.Add("@IdEnteSede", SqlDbType.Int).Value = txtIdSede.Value ' IdEnteSede
            Else
                SqlCmd.Parameters.Add("@IdEnteSede", SqlDbType.Int).Value = 0 ' IdEnteSede
            End If
            ' If Trim(Request.Form("txtIDComune")) = "" Then
            'SqlCmd.Parameters.Add("@IdComune", SqlDbType.Int).Value = txtIDComunes.Value
            'Else
            'SqlCmd.Parameters.Add("@IdComune", SqlDbType.Int).Value = Request.Form("txtIDComune")
            ' End If
            SqlCmd.Parameters.Add("@IdComune", SqlDbType.Int).Value = ddlComune.SelectedValue

            SqlCmd.Parameters.Add("@CAP", SqlDbType.VarChar).Value = txtCAP.Text ' CAP
            SqlCmd.Parameters.Add("@Indirizzo", SqlDbType.VarChar).Value = txtIndirizzo.Text ' indirizzo
            SqlCmd.Parameters.Add("@Civico", SqlDbType.VarChar).Value = txtCivico.Text ' Civico
            SqlCmd.Parameters.Add("@DettaglioRecapito", SqlDbType.VarChar).Value = TxtDettaglioRecapito.Text ' DettaglioRecapito

            'SETTORI INTERVENTO
            Dim check As CheckBox
            check = dtgSettori.Rows.Item(0).FindControl("chkSeleziona")
            If check.Checked = True Then
                SqlCmd.Parameters.Add("@Settore_A", SqlDbType.Bit).Value = 1
            Else
                SqlCmd.Parameters.Add("@Settore_A", SqlDbType.Bit).Value = 0
            End If
            check = dtgSettori.Rows.Item(1).FindControl("chkSeleziona")
            If check.Checked = True Then
                SqlCmd.Parameters.Add("@Settore_B", SqlDbType.Bit).Value = 1
            Else
                SqlCmd.Parameters.Add("@Settore_B", SqlDbType.Bit).Value = 0
            End If
            check = dtgSettori.Rows.Item(2).FindControl("chkSeleziona")
            If check.Checked = True Then
                SqlCmd.Parameters.Add("@Settore_C", SqlDbType.Bit).Value = 1
            Else
                SqlCmd.Parameters.Add("@Settore_C", SqlDbType.Bit).Value = 0
            End If
            check = dtgSettori.Rows.Item(3).FindControl("chkSeleziona")
            If check.Checked = True Then
                SqlCmd.Parameters.Add("@Settore_D", SqlDbType.Bit).Value = 1
            Else
                SqlCmd.Parameters.Add("@Settore_D", SqlDbType.Bit).Value = 0
            End If
            check = dtgSettori.Rows.Item(4).FindControl("chkSeleziona")
            If check.Checked = True Then
                SqlCmd.Parameters.Add("@Settore_E", SqlDbType.Bit).Value = 1
            Else
                SqlCmd.Parameters.Add("@Settore_E", SqlDbType.Bit).Value = 0
            End If
            'check = dtgSettori.Items.Item(5).FindControl("chkSeleziona")
            'If check.Checked = True Then
            '    SqlCmd.Parameters.Add("@Settore_F", SqlDbType.Bit).Value = 1
            'Else
            '    SqlCmd.Parameters.Add("@Settore_F", SqlDbType.Bit).Value = 0
            'End If

            'If dtgSettori.Items.Count = 7 Then 'SCU 
            '    check = dtgSettori.Items.Item(6).FindControl("chkSeleziona")
            '    If check.Checked = True Then
            '        SqlCmd.Parameters.Add("@Settore_G", SqlDbType.Bit).Value = 1
            '    Else
            '        SqlCmd.Parameters.Add("@Settore_G", SqlDbType.Bit).Value = 0
            '    End If
            'Else
            '    SqlCmd.Parameters.Add("@Settore_G", SqlDbType.Bit).Value = 0
            'End If

            If dtgSettori.Rows.Count = 7 Then 'SCU 
                check = dtgSettori.Rows.Item(5).FindControl("chkSeleziona")
                If check.Checked = True Then
                    SqlCmd.Parameters.Add("@Settore_F", SqlDbType.Bit).Value = 1
                Else
                    SqlCmd.Parameters.Add("@Settore_F", SqlDbType.Bit).Value = 0
                End If
                check = dtgSettori.Rows.Item(6).FindControl("chkSeleziona")
                If check.Checked = True Then
                    SqlCmd.Parameters.Add("@Settore_G", SqlDbType.Bit).Value = 1
                Else
                    SqlCmd.Parameters.Add("@Settore_G", SqlDbType.Bit).Value = 0
                End If
            Else

                SqlCmd.Parameters.Add("@Settore_F", SqlDbType.Bit).Value = 0

                check = dtgSettori.Rows.Item(5).FindControl("chkSeleziona")
                If check.Checked = True Then
                    SqlCmd.Parameters.Add("@Settore_G", SqlDbType.Bit).Value = 1
                Else
                    SqlCmd.Parameters.Add("@Settore_G", SqlDbType.Bit).Value = 0
                End If
            End If

            SqlCmd.Parameters.Add("@UsernameRichiesta", SqlDbType.VarChar).Value = Session("Utente")

            If Trim(txtDataNominaRL.Text) <> "" Then
                SqlCmd.Parameters.Add("@DataNominaRL", SqlDbType.DateTime).Value = txtDataNominaRL.Text
            Else
                SqlCmd.Parameters.Add("@DataNominaRL", SqlDbType.DateTime).Value = DBNull.Value
            End If


            If ddlClassi.SelectedItem IsNot Nothing Then
                If ddlClassi.SelectedValue = 0 Then
                    SqlCmd.Parameters.Add("@IdSezione", SqlDbType.Int).Value = DBNull.Value
                    SqlCmd.Parameters.Add("@IdSezioneRichiesta", SqlDbType.Int).Value = DBNull.Value
                Else
                    SqlCmd.Parameters.Add("@IdSezione", SqlDbType.Int).Value = CInt(ddlClassi.SelectedValue.ToString())
                    SqlCmd.Parameters.Add("@IdSezioneRichiesta", SqlDbType.Int).Value = CInt(ddlClassi.SelectedValue.ToString())
                End If
            End If

            If IdAllegatoAN <> 0 Then
                SqlCmd.Parameters.Add("@IdAllegatoNomina", SqlDbType.Int).Value = IdAllegatoAN
            Else
                SqlCmd.Parameters.Add("@IdAllegatoNomina", SqlDbType.Int).Value = DBNull.Value
            End If
            'Tolto l'allegato nella maschera lo passo a null al DB per reimpstarlo in un altra sezione inizio
            SqlCmd.Parameters.Add("@IdAllegatoRTDP", SqlDbType.Int).Value = DBNull.Value

            'Tolto l'allegato nella maschera lo passo a null al DB per reimpstarlo in un altra sezione fine
            If IdAllegatoDelibera <> 0 Then
                SqlCmd.Parameters.Add("@IdAllegatoDelibera", SqlDbType.Int).Value = IdAllegatoDelibera
            Else
                SqlCmd.Parameters.Add("@IdAllegatoDelibera", SqlDbType.Int).Value = DBNull.Value
            End If

            If IdAllegatoAttoCostitutivo <> 0 Then
                SqlCmd.Parameters.Add("@IdAllegatoAttoCostitutivo", SqlDbType.Int).Value = IdAllegatoAttoCostitutivo
            Else
                SqlCmd.Parameters.Add("@IdAllegatoAttoCostitutivo", SqlDbType.Int).Value = DBNull.Value
            End If

            If IdAllegatoStatuto <> 0 Then
                SqlCmd.Parameters.Add("@IdAllegatoStatuto", SqlDbType.Int).Value = IdAllegatoStatuto
            Else
                SqlCmd.Parameters.Add("@IdAllegatoStatuto", SqlDbType.Int).Value = DBNull.Value
            End If

            If IdAllegatoDeliberaAdesione <> 0 Then
                SqlCmd.Parameters.Add("@IdAllegatoDeliberaAdesione", SqlDbType.Int).Value = IdAllegatoDeliberaAdesione
            Else
                SqlCmd.Parameters.Add("@IdAllegatoDeliberaAdesione", SqlDbType.Int).Value = DBNull.Value
            End If

            If IDAllegatoImpegnoEtico <> 0 Then
                SqlCmd.Parameters.Add("@IdAllegatoImpegnoEtico", SqlDbType.Int).Value = IDAllegatoImpegnoEtico
            Else
                SqlCmd.Parameters.Add("@IdAllegatoImpegnoEtico", SqlDbType.Int).Value = DBNull.Value
            End If

            If (chkAttivita.Checked) Then
                SqlCmd.Parameters.Add("@AttivitaUltimiTreAnni", SqlDbType.Bit).Value = 1
            Else
                SqlCmd.Parameters.Add("@AttivitaUltimiTreAnni", SqlDbType.Bit).Value = 0
            End If


            If (chkFiniIstituzionali.Checked) Then
                SqlCmd.Parameters.Add("@AttivitaFiniIstituzionali", SqlDbType.Bit).Value = 1
            Else
                SqlCmd.Parameters.Add("@AttivitaFiniIstituzionali", SqlDbType.Bit).Value = 0
            End If

            If ddlTipologia.SelectedValue = "Privato" Then
                If (chkSenzaScopoLucro.Checked) Then
                    SqlCmd.Parameters.Add("@AttivitaSenzaLucro", SqlDbType.Bit).Value = 1
                Else
                    SqlCmd.Parameters.Add("@AttivitaSenzaLucro", SqlDbType.Bit).Value = 0
                End If
            Else
                SqlCmd.Parameters.Add("@AttivitaSenzaLucro", SqlDbType.Bit).Value = 0
            End If

            If ddlGiuridica.SelectedItem.Text = "Altro ai sensi dell’Art. 1, comma 2, D.Lgs. 30-3-2001 n. 165 (specificare)" Then
                If txtAltraTipoEnte.Text <> String.Empty Then
                    SqlCmd.Parameters.Add("@AltroTipoEnte", SqlDbType.NVarChar, 255).Value = txtAltraTipoEnte.Text
                Else
                    SqlCmd.Parameters.Add("@AltroTipoEnte", SqlDbType.NVarChar, 255).Value = DBNull.Value
                End If
            Else
                SqlCmd.Parameters.Add("@AltroTipoEnte", SqlDbType.NVarChar, 255).Value = DBNull.Value
            End If


            If chkRapportoAnnuale.Checked Then
                SqlCmd.Parameters.Add("@RapportoAnnuale", SqlDbType.Bit).Value = 1
            Else
                SqlCmd.Parameters.Add("@RapportoAnnuale", SqlDbType.Bit).Value = 0
            End If

            If flgForzaVariazione Then
                SqlCmd.Parameters.Add("@FlagForzaturaVariazione", SqlDbType.Bit).Value = 1
            Else
                SqlCmd.Parameters.Add("@FlagForzaturaVariazione", SqlDbType.Bit).Value = 0
            End If

            'Esito aggiornamento: 0-Errore 1-Aggiornamento effettuato
            SqlCmd.Parameters.Add("@Esito", SqlDbType.TinyInt)
            SqlCmd.Parameters("@Esito").Direction = ParameterDirection.Output

            SqlCmd.Parameters.Add("@messaggio", SqlDbType.VarChar)
            SqlCmd.Parameters("@messaggio").Size = 1000
            SqlCmd.Parameters("@messaggio").Direction = ParameterDirection.Output

            SqlCmd.ExecuteNonQuery()
            '@messaggio varchar(1000) output
            If SqlCmd.Parameters("@Esito").Value = 0 Then
                trans.Rollback()
                MessaggiAlert(SqlCmd.Parameters("@messaggio").Value())
                Log.Warning(Logger.Data.LogEvent.ANAGRAFICAENTE_MODIFICA_ERRATA, SqlCmd.Parameters("@messaggio").Value())
            End If
        Catch Sqlex As SqlException
            lblMessaggio.Text = Sqlex.Message
            trans.Rollback()
            Log.Error(Logger.Data.LogEvent.ANAGRAFICAENTE_MODIFICA_ERRATA, Sqlex.Message)
            Exit Sub
        Catch ex As Exception
            lblMessaggio.Text = ex.Message
            trans.Rollback()
            Log.Error(Logger.Data.LogEvent.ANAGRAFICAENTE_MODIFICA_ERRATA, ex.Message)
            Exit Sub
        End Try

        'Aggiorno Aree d'intervento per i settori selezionati inizio
        If (AggiornaSettoriAreeIntervento(trans)) Then
            trans = Nothing
        Else
            trans = Nothing
            Exit Sub
        End If
        'Aggiorno Aree d'intervento per i settori selezionati fine

        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
        'Aggiorno la Session della Denominazione
        strsql = "Select denominazione from Enti where idente=" & Session("idEnte") & ""
        dtrgenerico = ClsServer.CreaDatareader(strsql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
        dtrgenerico.Read()
        Session("denominazione") = dtrgenerico("denominazione")
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
        If ddlTipologia.SelectedItem.Text = "Pubblico" Then
            txtpianolocale.Value = ddlGiuridica.SelectedItem.Text
        Else
            txtpianolocale.Value = ""
        End If


    End Sub

    Private Sub MessaggiConvalida(ByVal strMessaggio)
        'Realizzata da Alessandra Taballione 26/02/04
        'Private sub per la gestione dell'immagine e del messaggio
        'per eventuali comunicazione all'Utente

        lblMessaggio.ForeColor = Color.Navy
        lblMessaggio.Text = strMessaggio
        Exit Sub
    End Sub

    Private Sub CmdRescissione_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdRescissione.Click
        '***Generata da Gianluigi Paesani in data:07/05/04
        '***Questa routine gestisce l'eliminazione della relazione tramite la 
        '***cancellazione logica di essa (con la vorizzazione del campo datafinevalidità) lasciandone traccia.
        Dim mycomm As Data.SqlClient.SqlCommand 'esegue update
        Dim dtrgenerico As Data.SqlClient.SqlDataReader
        Dim MyQuery As New System.Collections.ArrayList

        '*****************************************************************************************
        'Modificato il 7/09/2005 da Amilcare Paolella
        '*****************************************************************************************
        'Prelevo il C.F. e la P.I. presenti nei campi CodiceFiscale e PartitaIVA dell'Ente da annullare
        strsql = "Select CodiceFiscale From Enti where IdEnte='" & lblIdEnte.Value & "'"
        Dim strCodFis As String  'Codice Fiscale
        'Dim strPartIva As String 'Partita IVA
        Dim dtrCodFis As Data.SqlClient.SqlDataReader
        dtrCodFis = ClsServer.CreaDatareader(strsql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
        dtrCodFis.Read()
        strCodFis = Trim(IIf(IsDBNull(dtrCodFis.GetValue(0)), "", dtrCodFis.GetValue(0)))
        'strPartIva = Trim(IIf(IsDBNull(dtrCodFis.GetValue(1)), "", dtrCodFis.GetValue(1)))
        dtrCodFis.Close()
        dtrCodFis = Nothing
        '*****************************************************************************************

        'Trovo IDEnteRelazione
        Dim IntIdEnteRelazione As Integer
        dtrgenerico = ClsServer.CreaDatareader("select IdEnteRelazione from EntiRelazioni Where IdEnteFiglio = " & lblIdEnte.Value, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
        dtrgenerico.Read()
        IntIdEnteRelazione = dtrgenerico.Item("IdEnteRelazione")
        dtrgenerico.Close()

        'inizio controllo per constatare se vi siano sedi associate all'ente OK
        dtrgenerico = ClsServer.CreaDatareader("select idAssociaEntiRelazioniSedi from AssociaEntiRelazioniSedi a" &
                                               " inner join entirelazioni b on a.IdEnteRelazione=b.IDEnteRelazione" &
                                               " where b.identerelazione=" & IntIdEnteRelazione & "", IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
        If dtrgenerico.HasRows = True Then 'se non vi sono sedi eseguo eliminazione
            'eseguo comando dopo controllo formale
            dtrgenerico.Close()
            dtrgenerico = Nothing

            'Elimino Inclusione sediAttuazione
            strsql = "Delete from AssociaEntiRelazioniSediattuazioni  where identerelazione = " & IntIdEnteRelazione
            MyQuery.Add(strsql)

            'Elimino Inclusione sedi
            strsql = "Delete from AssociaEntiRelazioniSedi where identerelazione = " & IntIdEnteRelazione
            MyQuery.Add(strsql)
        End If

        strsql = "Update Entirelazioni Set datafinevalidità=getdate() where identerelazione=" & IntIdEnteRelazione
        MyQuery.Add(strsql)

        'Modifico stato a Sospeso codice fiscale e partita iva
        'Controllo se il codice fiscale o la partita iva siano nulli; in tal caso non modifico il dato inn archivio
        If strCodFis.ToString = "" Then
            strsql = "Update Enti Set IdStatoente = (Select Idstatoente from statienti where sospeso = 1) Where Idente = " & lblIdEnte.Value
        Else
            strsql = "Update Enti Set IdStatoente = (Select idstatoente from statienti where sospeso = 1),CodiceFiscale = Null,CodiceFiscaleArchivio = '" & strCodFis.ToString & "' Where Idente = " & lblIdEnte.Value
        End If
        MyQuery.Add(strsql)

        'Aggiunto da Alessandra Taballione il 11/07/2005
        'Cancello sedi e sedi di attuazione
        'sedi
        strsql = "Update Entisedi Set IdStatoentesede = 2 From Entisedi Where Idente = " & lblIdEnte.Value
        MyQuery.Add(strsql)

        'sedi attuazioni Proprie
        strsql = "Update Entisediattuazioni Set IdStatoentesede = 2 From Entisediattuazioni Where Identesede In (select identesede From entisedi Where idente = " & lblIdEnte.Value & ")"
        MyQuery.Add(strsql)

        'Elimino l'utenza   
        strsql = "Update Enti Set password = NULL where Idente = " & lblIdEnte.Value
        MyQuery.Add(strsql)

        strsql = "Delete From AssociaUtenteGruppo Where Username = '" & txtUtenza.Text & "'"
        MyQuery.Add(strsql)

        strsql = "Update RescissioniAccordi Set DataConferma = CONVERT(Datetime,GetDate(),103),UtenteConferma = '" & Session("Utente") & "',Stato = 1 Where IdEnte = " & lblIdEnte.Value
        MyQuery.Add(strsql)

        If ClsServer.EseguiQueryColl(MyQuery, Session.SessionID, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn"))) = False Then
            'Messaggio di errore
        Else
            Response.Redirect("LogOn.aspx")
        End If

    End Sub

    Private Sub cmdAnnullaModifica_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdAnnullaModifica.Click
        '20/08/2014
        'annullo le modifiche effettua nell'anagrafica dell'ente
        Dim AlboEnte As String
        Dim x As Integer
        For x = 0 To dtgSettori.Rows.Count - 1
            Dim check As CheckBox
            check = dtgSettori.Rows.Item(x).FindControl("chkSeleziona")
            check.Checked = False
        Next

        AnnullaModificaDocumenti(Session("IDEnte"))

        Dim StrSql As String
        StrSql = "UPDATE Accreditamento_VariazioneEnti SET StatoVariazione=2, UsernameCancellazione ='" & Session("Utente") & "', DataCancellazione= getdate() where IDEnte =" & Session("IDEnte") & " And statovariazione = 0"
        cmdGenerico = ClsServer.EseguiSqlClient(StrSql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))

        StrSql = "UPDATE Enti SET RichiestaModifica =0,UsernameRichiestaModifica=null,DataRichiestaModifica=null where IDEnte =" & Session("IDEnte") & " "
        cmdGenerico = ClsServer.EseguiSqlClient(StrSql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))

        ClearDeliberaAdesione()
        ClearSessionAN()
        ClearSessionAttoCostitutivo()
        ClearSessionStatuto()
        ClearSessionDelibera()
        ClearSessionImpegnoEtico()
        ddlClassi.SelectedIndex = 0
        Session("EsperienzeAreeSettore") = Nothing
        MessaggiConvalida("Annullamento modifica effettuato con successo.")
        AlboEnte = ClsUtility.TrovaAlboEnte(Session("IdEnte"), Session("Conn"))


        'PER L'ENTE VENGONO STORICIZZATE LE ARRE D'INTERVENTO DI OGNI SETTORE INIZIO

        Try
            Dim SqlCmd As New SqlCommand
            SqlCmd.CommandText = "SP_UPD_AREE_ESPERIENZE_SETTORE"
            SqlCmd.CommandType = CommandType.StoredProcedure
            SqlCmd.Connection = Session("Conn")
            SqlCmd.Parameters.Clear()
            SqlCmd.Parameters.Add("@IdEnte", SqlDbType.Int).Value = Session("IdEnte")
            SqlCmd.Parameters.Add("@UsernameRichiesta", SqlDbType.NVarChar, 10).Value = Session("Utente")
            'Esito aggiornamento: 0-Errore 1-Aggiornamento effettuato
            SqlCmd.Parameters.Add("@Esito", SqlDbType.TinyInt)
            SqlCmd.Parameters("@Esito").Direction = ParameterDirection.Output

            SqlCmd.Parameters.Add("@messaggio", SqlDbType.VarChar)
            SqlCmd.Parameters("@messaggio").Size = 1000
            SqlCmd.Parameters("@messaggio").Direction = ParameterDirection.Output
            SqlCmd.ExecuteNonQuery()
            MessaggiConvalida(SqlCmd.Parameters("@messaggio").Value())
            If SqlCmd.Parameters("@Esito").Value = 0 Then
                Exit Sub
            End If
        Catch ex As Exception
            Exit Sub
        End Try

        'cronologia annullamento
        Dim IdEnteFase As Integer
        StrSql = "select IdEnteFase from EntiFasi where TipoFase = 2 and GETDATE() between DataInizioFase and DataFineFase and IdEnte = " & Session("IdEnte")
        'dtrFase = ClsServer.CreaDataTable(StrSql, Session("Conn"))
        Dim myDatatable As DataTable = ClsServer.CreaDataTable(StrSql, False, Session("Conn"))
        If Not myDatatable Is Nothing AndAlso myDatatable.Rows.Count > 0 Then
            IdEnteFase = Integer.Parse(myDatatable.Rows(0)(0))
        End If
        If IdEnteFase > 0 Then
            Dim sqlInsertCronologia As String = "insert into Accreditamento_VariazioneEnteEsperienzaSettore_CONSISTENTI"
            sqlInsertCronologia += " SELECT ees.IdEnte, ees.IdSettore, ees.I_Esperienza, ees.II_Esperienza, ees.III_Esperienza,"
            sqlInsertCronologia += " ees.I_AnnoEsperienza, ees.II_AnnoEsperienza, ees.III_AnnoEsperienza, ees.AreeIntervento,"
            sqlInsertCronologia += " getdate(),	'Annulla Modifiche', @IdEnteFase, @UserName"
            sqlInsertCronologia += " FROM  EnteEsperienzaSettore ees where ees.idente=@IdEnte and DataFineValidita is null"

            Dim InsertCronologia As New SqlCommand(sqlInsertCronologia, Session("conn"))
            InsertCronologia.Parameters.AddWithValue("@IdEnteFase", IdEnteFase)
            InsertCronologia.Parameters.AddWithValue("@UserName", Session("Utente"))
            InsertCronologia.Parameters.AddWithValue("@IdEnte", Session("IdEnte"))
            InsertCronologia.ExecuteNonQuery()
        End If

        CaricaSettori(AlboEnte)
        LoadMaschera()
        GestisciStyleDatiModificati()
    End Sub

    Private Sub SalvaEnte()
        Dim comuni As Integer
        'controlli formali
        lblMessaggio.Text = ""
        comuni = 0
        bandiera = 0 ' metto questa variabile a zero variabile che serve nell'inserimento di un indirizzo inserito manualmente
        Dim AlboEnte As String

        AlboEnte = ClsUtility.TrovaAlboEnte(Session("IdEnte"), Session("Conn"))
        'NON FUNZIONA
        If txtCAP.Text = "" And ChkEstero.Checked = False Then
            lblMessaggio.Text = "Il CAP inserito non è congruo rispetto al comune selezionato."
            Exit Sub
        End If


        If AlboEnte = "SCN" Then
            If ddlTipologia.SelectedItem.Text = "Pubblico" Then
                If txtpianolocale.Value <> "Piano di Zona" And ddlGiuridica.SelectedItem.Text = "Piano di Zona" Then

                    lblMessaggio.ForeColor = Color.Red
                    lblMessaggio.Text = "Impossibile inserire la Tipologia Piano di Zona in quanto non e' piu' una tipologia Accreditabile"
                    Exit Sub
                End If
            Else
                txtpianolocale.Value = ""
            End If

        End If
        'controllo se il comune e' nazionale o estero per l'obbligatorieta' della sede legale sul territorio nazionale
        'strsql = "SELECT ComuneNazionale FROM Comuni where idComune=" & IIf(Request.Form("txtIdComune") = "", txtIDComunes.Value, Request.Form("txtIdComune")) & " and ComuneNazionale=1"
        strsql = "SELECT ComuneNazionale FROM Comuni where idComune=" & ddlComune.SelectedValue & " and ComuneNazionale=1"
        dtrgenerico = ClsServer.CreaDatareader(strsql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
        dtrgenerico.Read()
        If dtrgenerico.HasRows = False Then

            lblMessaggio.ForeColor = Color.Red
            lblMessaggio.Text = "Attenzione!!!La Sede Legale deve essere obbligatoriamente nel territorio Nazionale"
            If Not dtrgenerico Is Nothing Then
                dtrgenerico.Close()
                dtrgenerico = Nothing
            End If
            Exit Sub
        End If
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If

        'Antonello---------------------------------

        Dim strMiaCausale As String = ""
        'If ClsUtility.CAP_VERIFICA(IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")), _
        '      strMiaCausale, bandiera, Trim(txtCAP.Text), IIf(Request.Form("txtIdComune") = "", txtIDComunes.Value, Request.Form("txtIdComune")), "", "", txtIndirizzo.Text, txtCivico.Text) = False Then

        If ClsUtility.CAP_VERIFICA(IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")),
           strMiaCausale, bandiera, Trim(txtCAP.Text), ddlComune.SelectedValue, "", "", txtIndirizzo.Text, txtCivico.Text) = False Then
            'Ripristino lo stato del tasto
            CmdSalva.Visible = True
            'Inserisco il Messaggio di Errore
            MessaggiAlert(strMiaCausale)
            If Not dtrgenerico Is Nothing Then
                dtrgenerico.Close()
                dtrgenerico = Nothing
            End If
            Exit Sub
        End If
        '*************************************************
        'AGGIUNTO DA SIMONA CORDELLA IL 18/10/2017
        ' richiamo store per controllo codicefiscale e demoniazione ente nell'albo
        Dim ESITO As String
        Dim strMsgEmail As String = ""
        Dim strMsg As String = ""
        Dim intIDE As Integer = 0

        If Request.QueryString("azione") <> "Ins" Then
            intIDE = lblIdEnte.Value
        End If

        ESITO = ClsUtility.STORE_VERIFICA_USABILITA_ENTE_ALBO(Replace(Trim(txtCodFis.Text), "'", "''"), Replace(Trim(txtdenominazione.Text), "'", "''"), AlboEnte, intIDE, strMsgEmail, strMsg, Session("conn"))
        If ESITO = "NO" Then
            lblMessaggio.Text = strMsg
            Exit Sub
        End If

        '**************

        ''Richiamo la Routine di Modifica dell'Ente
        ''controllo denominazione Ente
        'If Trim(txtdenominazione.Text) <> "" Then
        '    'Verifico se la denominazione è gia presente nell'anagrafica Enti
        '    'Modifica query da Alessandra Taballione il  09/09/2005
        '    strsql = "Select idente from enti " & _
        '    " inner join statienti on statienti.idstatoente=enti.idstatoente" & _
        '    " where denominazione='" & Replace(Trim(txtdenominazione.Text), "'", "''") & "' and idente <> " & lblIdEnte.Value & "" & _
        '    " and statienti.idstatoente not in (4,5,7,10,11)"
        '    If Not dtrgenerico Is Nothing Then
        '        dtrgenerico.Close()
        '        dtrgenerico = Nothing
        '    End If
        '    dtrgenerico = ClsServer.CreaDatareader(strsql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
        '    'se gia esistente viene bloccato l'inserimento
        '    If dtrgenerico.HasRows = True Then

        '        lblMessaggio.ForeColor = Color.Red
        '        lblMessaggio.Text = "La denominazione inserita è già presente in archivio!"
        '        If Not dtrgenerico Is Nothing Then
        '            dtrgenerico.Close()
        '            dtrgenerico = Nothing
        '        End If
        '        Exit Sub
        '    End If
        '    If Not dtrgenerico Is Nothing Then
        '        dtrgenerico.Close()
        '        dtrgenerico = Nothing
        '    End If
        'End If
        If Trim(txtDataCostituzione.Text) <> "" Then
            Dim ente As String
            'Verifico che la data di costituzione sia maggiore della data inizioValidità
            strsql = "select f.denominazione as figlio, p.denominazione as padre, * from entirelazioni " &
            " inner join enti f on f.idente=entirelazioni.identefiglio" &
            " inner join enti p on p.idente=entirelazioni.identePadre " &
            " where identepadre=" & Session("IdEnte") & " or identeFiglio=" & Session("IdEnte") & ""
            If Not dtrgenerico Is Nothing Then
                dtrgenerico.Close()
                dtrgenerico = Nothing
            End If
            'eseguo la query per confrontare date
            dtrgenerico = ClsServer.CreaDatareader(strsql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
            If dtrgenerico.HasRows = True Then
                dtrgenerico.Read()
                If dtrgenerico("Padre") = Session("Denominazione") Then
                    ente = dtrgenerico("figlio")
                Else
                    ente = dtrgenerico("padre")
                End If
                If Not IsDBNull(dtrgenerico("datastipula")) Then
                    If CDbl(CalcolaData(txtDataCostituzione.Text)) > CDbl(CalcolaData(dtrgenerico("datastipula"))) Then

                        lblMessaggio.ForeColor = Color.Red
                        lblMessaggio.Text = "Non è possibile effettuare la modifica dell'Ente perchè la data di Costituzione inserita è successiva alla data di Registrazione Accordo con l'ente " & ente & " ."
                        If Not dtrgenerico Is Nothing Then
                            dtrgenerico.Close()
                            dtrgenerico = Nothing
                        End If
                        Exit Sub
                    End If
                End If
            End If
        End If
        'If Trim(txtCodFis.Text) <> "" Then
        '    'Aggiunto da Alessandra Taballione il 19/07/2005
        '    strsql = "select * from eccezionicodicifiscalienti where codicefiscale='" & Replace(Trim(txtCodFis.Text), "'", "''") & "'"
        '    If Not dtrgenerico Is Nothing Then
        '        dtrgenerico.Close()
        '        dtrgenerico = Nothing
        '    End If
        '    dtrgenerico = ClsServer.CreaDatareader(strsql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
        '    If dtrgenerico.HasRows = False Then
        '        If Not dtrgenerico Is Nothing Then
        '            dtrgenerico.Close()
        '            dtrgenerico = Nothing
        '        End If
        '        'Verifico se il codice fiscale è gia presente nell'anagrafica Enti
        '        strsql = "Select idente from enti where codicefiscale='" & Replace(Trim(txtCodFis.Text), "'", "''") & "'"
        '        If Request.QueryString("Ins") <> "Vero" Then
        '            strsql = strsql & " and idente <> " & Session("IdEnte") & ""
        '        End If
        '        If Not dtrgenerico Is Nothing Then
        '            dtrgenerico.Close()
        '            dtrgenerico = Nothing
        '        End If
        '        dtrgenerico = ClsServer.CreaDatareader(strsql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
        '        'se gia esistente viene bloccato l'inserimento
        '        If dtrgenerico.HasRows = True Then
        '            MessaggiAlert("Il codice fiscale è già presente in Archivio.")
        '            If Not dtrgenerico Is Nothing Then
        '                dtrgenerico.Close()
        '                dtrgenerico = Nothing
        '            End If
        '            ''Aggiunto da Alessandra Taballione il 31/08/2005
        '            'Response.Write("<script>" & vbCrLf)
        '            'Response.Write("window.open(""DettaglioEnte.aspx?codfis=" & txtCodFis.Text & """, """", ""width=700px,height=300px,toolbar=no,location=no,menubar=no,scrollbars=yes"")" & vbCrLf)
        '            'Response.Write("</script>")

        '            Exit Sub
        '        End If
        '    End If
        'End If
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
        If AlboEnte = "SCN" Then
            'ADC 02/02/2006 Query che controlla la compatibilita' sulle tipologie
            If (ddlGiuridica.SelectedValue = "0" And UCase(ddlTipologia.SelectedValue) = "PUBBLICO") Or UCase(ddlTipologia.SelectedValue) = "" Then
                MessaggiAlert("Impossibile effettuare il salvataggio. Inserire il Tipo Ente.")
                Exit Sub
            End If
        End If
        If AlboEnte = "SCU" Then
            If (ddlGiuridica.SelectedValue = "0") Or UCase(ddlTipologia.SelectedValue) = "" Then
                MessaggiAlert("Impossibile effettuare il salvataggio. Inserire il Tipo Ente.")
                Exit Sub
            End If
        End If
        If AlboEnte = "SCU" Then
            'questo controllo sono se ENTE è di SCN 
            If (ddlGiuridica.SelectedValue <> "2" And ddlGiuridica.SelectedValue <> "24") Or UCase(ddlTipologia.SelectedValue) = "PRIVATO" Then
                strsql = "select a.idente from enti a " &
                        " inner join tipologieenti te1 on a.tipologia = te1.descrizione" &
                        " inner join entirelazioni er on a.idente = er.identepadre" &
                        " inner join enti b on b.idente = er.identefiglio" &
                        " inner join tipologieenti te2 on b.tipologia = te2.descrizione" &
                        " inner join statienti se on se.idstatoente = b.idstatoente" &
                        " where a.idente = " & Session("idente") & " And er.datafinevalidità Is null And se.sospeso <> 1" &
                        " and te1.statale = 1 and te2.statale = 1 and b.FlagEccezioniEntiStataliFiglio = 0"

                'eseguo la query per verificare compatibilità
                dtrgenerico = ClsServer.CreaDatareader(strsql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
                If dtrgenerico.HasRows = True Then
                    MessaggiAlert("Impossibile cambiare la tipologia. Esistono enti di tipo 'Amministrazione Statale' in accordo.")
                    If Not dtrgenerico Is Nothing Then
                        dtrgenerico.Close()
                        dtrgenerico = Nothing
                    End If
                    Exit Sub
                End If

                If Not dtrgenerico Is Nothing Then
                    dtrgenerico.Close()
                    dtrgenerico = Nothing
                End If

            End If
        End If

        VerificaClassi()

        Dim ListaAllegati As New List(Of Allegato)

        ListaAllegati = CaricaEntiAllegatiDaDB()

        'Inserisco prima gli allegati che poi inserisco o aggiorno sull'ente
        Dim IDAllegatoAn As Integer
        Dim IDAllegatoRTDP As Integer
        Dim IDAllegatoDelibera As Integer
        Dim IDAllegatoDeliberaAdesione As Integer
        Dim IDAllegatoStatuto As Integer
        Dim IDAllegatoAttoCostitutivo As Integer
        Dim IDAllegatoImpegnoEtico As Integer
        Dim IDAllegato As Integer
        Dim trans As SqlClient.SqlTransaction = Session("conn").BeginTransaction(IsolationLevel.ReadCommitted)

        'Verifico se allegati già presenti sul DB 

        If (Session("LoadedAN") IsNot Nothing) Then
            ' confronto l'allegato preso dal Db con l'allegato presente in sessione
            If Not ConfrontaAllegati(ListaAllegati, DirectCast(Session("LoadedAN"), Allegato), IDAllegato) Then 'Verifico se gli allegati presi dal DB corrispondono con gli allegati presenti in sessione
                If (InserisciAllegati(trans, IDAllegato, DirectCast(Session("LoadedAN"), Allegato))) Then
                    IDAllegatoAn = IDAllegato
                Else
                    trans = Nothing
                    Exit Sub
                End If
            Else
                IDAllegatoAn = IDAllegato
            End If
        End If

        If (Session("LoadedDelibera") IsNot Nothing) Then
            If Not ConfrontaAllegati(ListaAllegati, DirectCast(Session("LoadedDelibera"), Allegato), IDAllegato) Then 'Verifico se gli allegati presi dal DB corrispondono con gli allegati presenti in sessione
                If (InserisciAllegati(trans, IDAllegato, DirectCast(Session("LoadedDelibera"), Allegato))) Then
                    IDAllegatoDelibera = IDAllegato
                Else
                    trans = Nothing
                    Exit Sub
                End If
            Else
                IDAllegatoDelibera = IDAllegato
            End If
        End If

        If (Session("LoadedAC") IsNot Nothing) Then
            If Not ConfrontaAllegati(ListaAllegati, DirectCast(Session("LoadedAC"), Allegato), IDAllegato) Then 'Verifico se gli allegati presi dal DB corrispondono con gli allegati presenti in sessione
                If (InserisciAllegati(trans, IDAllegato, DirectCast(Session("LoadedAC"), Allegato))) Then
                    IDAllegatoAttoCostitutivo = IDAllegato
                Else
                    trans = Nothing
                    Exit Sub
                End If
            Else
                IDAllegatoAttoCostitutivo = IDAllegato
            End If
        End If

        If (Session("LoadedStatuto") IsNot Nothing) Then
            If Not ConfrontaAllegati(ListaAllegati, DirectCast(Session("LoadedStatuto"), Allegato), IDAllegato) Then 'Verifico se gli allegati presi dal DB corrispondono con gli allegati presenti in sessione
                If (InserisciAllegati(trans, IDAllegato, DirectCast(Session("LoadedStatuto"), Allegato))) Then
                    IDAllegatoStatuto = IDAllegato
                Else
                    trans = Nothing
                    Exit Sub
                End If
            Else
                IDAllegatoStatuto = IDAllegato
            End If
        End If

        If (Session("LoadedDeliberaAdesione") IsNot Nothing) Then
            If Not ConfrontaAllegati(ListaAllegati, DirectCast(Session("LoadedDeliberaAdesione"), Allegato), IDAllegato) Then 'Verifico se gli allegati presi dal DB corrispondono con gli allegati presenti in sessione
                If (InserisciAllegati(trans, IDAllegato, DirectCast(Session("LoadedDeliberaAdesione"), Allegato))) Then
                    IDAllegatoDeliberaAdesione = IDAllegato
                Else
                    trans = Nothing
                    Exit Sub
                End If
            Else
                IDAllegatoDeliberaAdesione = IDAllegato
            End If
        End If

        If (Session("LoadedCartaImpegnoEtico") IsNot Nothing) Then
            If Not ConfrontaAllegati(ListaAllegati, DirectCast(Session("LoadedCartaImpegnoEtico"), Allegato), IDAllegato) Then 'Verifico se gli allegati presi dal DB corrispondono con gli allegati presenti in sessione
                If (InserisciAllegati(trans, IDAllegato, DirectCast(Session("LoadedCartaImpegnoEtico"), Allegato))) Then
                    IDAllegatoImpegnoEtico = IDAllegato
                Else
                    trans = Nothing
                    Exit Sub
                End If
            Else
                IDAllegatoImpegnoEtico = IDAllegato
            End If
        End If

        ModificaEnte(AlboEnte, IDAllegatoAn, IDAllegatoDelibera, IDAllegatoAttoCostitutivo, IDAllegatoStatuto, IDAllegatoDeliberaAdesione, IDAllegatoImpegnoEtico, trans)

        LoadMaschera()
        GestisciStyleDatiModificati()
        Session("IdComune") = Nothing
    End Sub

    Function IsUnchangedSettoreEsperienze(RigaDati As DataRow, EsperienzeAreeSettore As List(Of clsEsperienzeAree)) As Boolean
        'Questa funzione restituisce true se il confronto tra i dati (in maschera contenuti nella EsperienzeAreeSettore solo se ci sono esperienze inserite) 
        'che verranno salvati sono uguali alla riga nel datatable
        Dim _idSettore As Integer

        If EsperienzeAreeSettore.Count = 0 Then 'Non c'è nessuna esperienza per nessun settore in maschera
            If RigaDati.Item("IDSETTORE") IsNot DBNull.Value Then
                Return False    'il settore aveva esperienze caricate
            Else
                Return True     'il settore non aveva esperienze caricate
            End If
        End If

        'se il settore aveva esperienze caricate
        If RigaDati.Item("IDSETTORE") IsNot DBNull.Value Then
            _idSettore = Integer.Parse(RigaDati.Item("IDSETTORE"))
            'se non esistono dati in maschera per il settore
            If Not EsperienzeAreeSettore.Exists(Function(s) s.IdSettore = Integer.Parse(RigaDati.Item("IDMACROATTIVITA"))) Then
                Return False
            Else
                'ci sono sia esperienze caricate che dati in maschera
                'bisogna testare campo per campo
            End If
        Else
            'il settore non aveva esperienze caricate
            If EsperienzeAreeSettore.Exists(Function(s) s.IdSettore = Integer.Parse(RigaDati.Item("IDMACROATTIVITA"))) Then
                'esistono i dati in maschera per il settore
                Return False
            Else
                'non esistono i dati in maschera per il settore
                Return True
            End If
        End If

        'rimangono da testare campo per campo i dati caricati con i dati in maschera
        Dim EsperienzaSettore = EsperienzeAreeSettore.Find((Function(esperienza As clsEsperienzeAree) esperienza.IdSettore = _idSettore))

        If RigaDati("AREEINTERVENTO") <> String.Join(",", EsperienzaSettore.AreeSelezionate) Then Return False 'settori di intervento diversi
        If Integer.Parse(RigaDati("III_ANNOESPERIENZA")) <> EsperienzaSettore.AnnoEsperienza(2) Then Return False
        If Integer.Parse(RigaDati("II_ANNOESPERIENZA")) <> EsperienzaSettore.AnnoEsperienza(1) Then Return False
        If Integer.Parse(RigaDati("I_ANNOESPERIENZA")) <> EsperienzaSettore.AnnoEsperienza(0) Then Return False
        If RigaDati("III_ESPERIENZA") <> EsperienzaSettore.DescrizioneEsperienza(2) Then Return False
        If RigaDati("II_ESPERIENZA") <> EsperienzaSettore.DescrizioneEsperienza(1) Then Return False
        If RigaDati("I_ESPERIENZA") <> EsperienzaSettore.DescrizioneEsperienza(0) Then Return False

        Return True
    End Function

    Function VerificaSettoriCancellatiReinseriti() As String
        'Questa funzione restituisce, se esistono, la lista dei settori cancellati e poi reinseriti in maschera nella stessa fase
        'La cosa non è ammessa e deve essere fatta in due fasi di adeguamento successive, la prima per cancellare, la seconda per reinserire
        'Il confronto viene fatto usando i dati reali
        'Ci sono due casi
        '   1) si è deselezionato il settore e poi, senza salvare, selezionato di nuovo
        '   2) si è deselezionato il settore, salvato, e poi selezionato di nuovo
        'In entrambi i casi è importante il campo aggiuntivo "NuovoInserimento" contenuto nel datatable che, per ogni settore caricato, indica se è un settore appena 
        'inserito nella eventuale fase attualmente aperta (che quindi può essere eliminato e/o modificato come si vuole)

        Dim _ret As String = ""
        Dim EsperienzeAreeSettore As New List(Of clsEsperienzeAree)
        Dim _settoriDaDb As DataTable
        _settoriDaDb = ElencoSettori()  'dati reali

        If Session("EsperienzeAreeSettore") IsNot Nothing Then
            EsperienzeAreeSettore = CType(Session("EsperienzeAreeSettore"), List(Of clsEsperienzeAree))
        Else
            EsperienzeAreeSettore.Clear()
        End If

        'If Session("ElencoSettoriDB") IsNot Nothing Then
        '_settoriDaDb = Session("ElencoSettoriDB")

        If _settoriDaDb.Rows.Count > 0 Then
            'ciclo su tutti i settori selezionati in griglia
            For Each gRow As GridViewRow In dtgSettori.Rows
                Dim check As CheckBox = DirectCast(gRow.FindControl("chkSeleziona"), CheckBox)
                'se è selezionato
                If check.Checked Then
                    'cerco il settore nei dati reali
                    Dim HFIdMacroAttivita As HiddenField = DirectCast(gRow.FindControl("HFIdMacroAttivita"), HiddenField)
                    If HFIdMacroAttivita IsNot Nothing Then
                        Dim _riga As DataRow() = _settoriDaDb.Select("IDMACROATTIVITA=" & HFIdMacroAttivita.Value)
                        'se l'ho trovato e non è un nuovo inserimento
                        If _riga.Count > 0 AndAlso _riga(0).Item("NuovoInserimento") = "0" Then
                            'se è cambiato allora è un errore (è stato cancellato e reinserito, l'unico modo di cambiarlo)
                            If Not IsUnchangedSettoreEsperienze(_riga(0), EsperienzeAreeSettore) Then
                                If String.IsNullOrEmpty(_ret) Then
                                    _ret = _riga(0).Item("Codifica")
                                Else
                                    _ret += "," + _riga(0).Item("Codifica")
                                End If
                            End If
                        End If
                    End If
                End If
            Next
        End If
        'End If

        Return _ret
    End Function

    Function ValidazioneServerSalva() As Boolean

        'Dim regex As Regex = New Regex("^[_a-z0-9-]+(.[a-z0-9-]+)@[a-z0-9-]+(.[a-z0-9-]+)*(.[a-z]{2,4})$")
        Dim regex As Regex = New Regex("^(?("")("".+?""@)|(([0-9a-zA-Z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-zA-Z])@))(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,6}))$")
        Dim matchPEC As Match = regex.Match(txtEmailpec.Text.Trim)
        Dim result As Boolean = True
        Dim EsperienzeAreeSettore As New List(Of clsEsperienzeAree)
        Dim isVecchioEnte As Boolean
        Dim dtConfigurazione As New DataTable
        Dim DataInizioNuovaIscrizione As Date
        Dim DataFineBloccoControlli As Date
        Dim DataCreazioneEnte As Date
        Dim DA As New SqlDataAdapter

        pLog = New Hashtable()
        pLog.Add("IDEnte", Session("IdEnte"))
        flgForzaVariazione = False

        lblMessaggio.Text = ""
        strsql = "SELECT PARAMETRO,VALORE FROM CONFIGURAZIONI WHERE PARAMETRO IN ('DATAINIZIONUOVAISCRIZIONE','DATAFINEBLOCCOCONTROLLI')"

        Dim SqlCmd = New SqlClient.SqlCommand
        Try
            SqlCmd.CommandText = strsql
            SqlCmd.CommandType = CommandType.Text
            SqlCmd.Connection = Session("Conn")
            DA.SelectCommand = SqlCmd
            DA.Fill(dtConfigurazione)
        Catch ex As Exception
            MessaggiAlert(ex.Message)
        End Try

        For Each dr As DataRow In dtConfigurazione.Rows
            If dr("PARAMETRO") = "DATAINIZIONUOVAISCRIZIONE" Then
                DataInizioNuovaIscrizione = CDate(dr("VALORE").ToString())
            End If

            If dr("PARAMETRO") = "DATAFINEBLOCCOCONTROLLI" Then
                DataFineBloccoControlli = CDate(dr("VALORE").ToString())
            End If
        Next

        DataCreazioneEnte = CDate(Session("DataCreazioneEnte"))

        If DataCreazioneEnte < DataInizioNuovaIscrizione Then
            isVecchioEnte = True
        Else
            isVecchioEnte = False
        End If


        ''''ADC 02/03/2022 SE ente prima del 01/06/2021  THEN
        Dim dataAccr = CDate("01/06/2021")

        If CDate(Session("DataCreazioneEnte")).Date >= dataAccr.Date Then
            If (isVecchioEnte AndAlso Date.Now.Date >= DataFineBloccoControlli) OrElse isVecchioEnte = False Then
                If Session("LoadedAN") Is Nothing Then
                    lblMessaggio.Text = lblMessaggio.Text & "L'atto di nomina del rappresentante legale non è stato caricato."
                    lblMessaggio.Text = lblMessaggio.Text & " </br>"
                    result = False
                Else
                    pLog.Add("AttoNomina", DirectCast(Session("LoadedAN"), Allegato).Filename)
                End If

                If Session("LoadedDelibera") Is Nothing Then
                    lblMessaggio.Text = lblMessaggio.Text & "La delibera degli organi decisionali sulla definizione nominativa della struttura di gestione non è stata caricata"
                    lblMessaggio.Text = lblMessaggio.Text & " </br>"
                    result = False
                Else
                    pLog.Add("DeliberaStrutturaGestione", DirectCast(Session("LoadedDelibera"), Allegato).Filename)
                End If

                pLog.Add("Attività", IIf(chkAttivita.Checked, "Si", "No"))
                If Not chkAttivita.Checked Then
                    lblMessaggio.Text = lblMessaggio.Text & "La casella dichiarante lo svolgimento delle attività da almeno 3 anni non è stata selezionata"
                    lblMessaggio.Text = lblMessaggio.Text & " </br>"
                    result = False
                End If

                pLog.Add("Fini Istituzionali", IIf(chkFiniIstituzionali.Checked, "Si", "No"))
                If Not chkFiniIstituzionali.Checked Then
                    lblMessaggio.Text = lblMessaggio.Text & "La casella dichiarante la conformità  tra gli scopi istituzionali e le finalità dell'ente non è stata selezionata"
                    lblMessaggio.Text = lblMessaggio.Text & " </br>"
                    result = False
                End If

                pLog.Add("Senza Scopo di Lucro", IIf(chkSenzaScopoLucro.Checked, "Si", "No"))
                If ddlTipologia.SelectedValue = "Privato" AndAlso Not chkSenzaScopoLucro.Checked Then
                    lblMessaggio.Text = lblMessaggio.Text & "La casella dichiarante di essere senza scopo di lucro non è stata selezionata"
                    lblMessaggio.Text = lblMessaggio.Text & " </br>"
                    result = False
                End If

                'Dim ListaHash As New List(Of String)
                Dim ListaHash As New Hashtable
                Dim HashDoppi As New List(Of String)
                'If Session("LoadedAN") IsNot Nothing Then
                '    ListaHash.Add(DirectCast(Session("LoadedAN"), Allegato).Hash)
                'End If

                'If Session("LoadedAC") IsNot Nothing Then
                '    ListaHash.Add(DirectCast(Session("LoadedAC"), Allegato).Hash)
                '    pLog.Add("AttoCostitutivo", DirectCast(Session("LoadedAC"), Allegato).Filename)
                'End If

                'If ddlTipologia.SelectedValue.ToUpper = "PUBBLICO" Or ddlTipologia.SelectedValue = String.Empty Then
                '    If Session("LoadedDeliberaAdesione") IsNot Nothing Then
                '        ListaHash.Add(DirectCast(Session("LoadedDeliberaAdesione"), Allegato).Hash)
                '        pLog.Add("DeliberaAdesione", DirectCast(Session("LoadedDeliberaAdesione"), Allegato).Filename)
                '    End If
                'End If


                'If Session("LoadedStatuto") IsNot Nothing Then
                '    ListaHash.Add(DirectCast(Session("LoadedStatuto"), Allegato).Hash)
                '    pLog.Add("Statuto", DirectCast(Session("LoadedStatuto"), Allegato).Filename)
                'End If

                'If Session("LoadedDelibera") IsNot Nothing Then
                '    ListaHash.Add(DirectCast(Session("LoadedDelibera"), Allegato).Hash)
                'End If


                'If Session("LoadedCartaImpegnoEtico") IsNot Nothing Then
                '    ListaHash.Add(DirectCast(Session("LoadedCartaImpegnoEtico"), Allegato).Hash)
                'End If
                If Session("LoadedAN") IsNot Nothing Then
                    ListaHash.Add("AN", DirectCast(Session("LoadedAN"), Allegato).Hash)
                End If

                If Session("LoadedAC") IsNot Nothing Then
                    ListaHash.Add("AC", DirectCast(Session("LoadedAC"), Allegato).Hash)
                    pLog.Add("AttoCostitutivo", DirectCast(Session("LoadedAC"), Allegato).Filename)
                End If

                If ddlTipologia.SelectedValue.ToUpper = "PUBBLICO" Or ddlTipologia.SelectedValue = String.Empty Then
                    If Session("LoadedDeliberaAdesione") IsNot Nothing Then
                        ListaHash.Add("DA", DirectCast(Session("LoadedDeliberaAdesione"), Allegato).Hash)
                        pLog.Add("DeliberaAdesione", DirectCast(Session("LoadedDeliberaAdesione"), Allegato).Filename)
                    End If
                End If


                If Session("LoadedStatuto") IsNot Nothing Then
                    ListaHash.Add("ST", DirectCast(Session("LoadedStatuto"), Allegato).Hash)
                    pLog.Add("Statuto", DirectCast(Session("LoadedStatuto"), Allegato).Filename)
                End If

                If Session("LoadedDelibera") IsNot Nothing Then
                    ListaHash.Add("GS", DirectCast(Session("LoadedDelibera"), Allegato).Hash)
                End If


                If Session("LoadedCartaImpegnoEtico") IsNot Nothing Then
                    ListaHash.Add("CI", DirectCast(Session("LoadedCartaImpegnoEtico"), Allegato).Hash)
                End If


                'Verifico se ci sono allegati duplicati in maschera inizio
                Dim doppione As Boolean = False
                For Each item As DictionaryEntry In ListaHash
                    For i = 0 To ListaHash.Count - 1
                        If (item.Key <> ListaHash.Keys(i)) AndAlso (item.Value = ListaHash.Values(i)) Then
                            If Not (HashDoppi.Contains(item.Value)) Then
                                If (item.Key <> "ST" And item.Key <> "AC") Then
                                    lblMessaggio.Text = lblMessaggio.Text & "Un allegato con codice Hash '" & item.Value & "' è stato inserito più di una volta in maschera per questo Ente  </br>"
                                    HashDoppi.Add(item.Value)
                                    doppione = True
                                ElseIf (ListaHash.Keys(i) <> "ST" And ListaHash.Keys(i) <> "AC") Then
                                    HashDoppi.Add(item.Value)
                                    lblMessaggio.Text = lblMessaggio.Text & "Un allegato con codice Hash '" & item.Value & "' è stato inserito più di una volta in maschera per questo Ente  </br>"
                                    doppione = True
                                End If
                            End If
                        End If
                    Next
                Next
                If doppione Then result = False

                If ddlTipologia.SelectedValue <> String.Empty Then
                    If ddlTipologia.SelectedValue.ToUpper = "PUBBLICO" Then
                        If Session("LoadedDeliberaAdesione") Is Nothing Then
                            lblMessaggio.Text = lblMessaggio.Text & "Allegare la Delibera"
                            lblMessaggio.Text = lblMessaggio.Text & " </br>"
                            result = False
                        End If
                        If Session("LoadedAC") IsNot Nothing AndAlso Session("LoadedStatuto") Is Nothing Then
                            lblMessaggio.Text = lblMessaggio.Text & "Allegare lo Statuto dell'Ente"
                            lblMessaggio.Text = lblMessaggio.Text & " </br>"
                            result = False
                        End If
                        If Session("LoadedAC") Is Nothing AndAlso Session("LoadedStatuto") IsNot Nothing Then
                            lblMessaggio.Text = lblMessaggio.Text & "Allegare l'Atto Costitutivo dell'Ente"
                            lblMessaggio.Text = lblMessaggio.Text & " </br>"
                            result = False
                        End If
                    End If
                End If

                If ddlTipologia.SelectedValue.ToUpper = "PRIVATO" Then
                    If Session("LoadedStatuto") Is Nothing Then
                        lblMessaggio.Text = lblMessaggio.Text & "Allegare lo Statuto dell'Ente"
                        lblMessaggio.Text = lblMessaggio.Text & " </br>"
                        result = False
                    End If
                    If Session("LoadedAC") Is Nothing Then
                        lblMessaggio.Text = lblMessaggio.Text & "Allegare l'Atto Costitutivo dell'Ente"
                        lblMessaggio.Text = lblMessaggio.Text & " </br>"
                        result = False
                    End If
                End If

                If Session("LoadedCartaImpegnoEtico") Is Nothing Then
                    lblMessaggio.Text = lblMessaggio.Text & "Allegare la Carta Impegno Etico."
                    lblMessaggio.Text = lblMessaggio.Text & " </br>"
                    result = False
                Else
                    pLog.Add("CartaImpegnoEtico", DirectCast(Session("LoadedCartaImpegnoEtico"), Allegato).Filename)
                End If

                If Not chkRapportoAnnuale.Checked Then
                    lblMessaggio.Text = lblMessaggio.Text & "Non è stata spuntata l'accettazione del rapporto annuale"
                    lblMessaggio.Text = lblMessaggio.Text & " </br>"
                    result = False
                End If
            Else
                If Session("IdStatoEnte") <> "6" AndAlso isVecchioEnte Then 'Forzo l'applicativo alla variazione
                    flgForzaVariazione = True
                End If
                'controllo i settori
                If VerificaCheck() = False Then
                    lblMessaggio.ForeColor = Color.Red
                    lblMessaggio.Text = "E' necessario indicare almeno un Settore di Intervento."
                    lblMessaggio.Text = lblMessaggio.Text & " </br>"
                    Exit Function
                End If
            End If


        Else  'ADC 02/03/2022



            If Session("IdStatoEnte") <> "6" AndAlso isVecchioEnte Then 'Forzo l'applicativo alla variazione
                flgForzaVariazione = True
            End If
            'controllo i settori
            If VerificaCheck() = False Then
                lblMessaggio.ForeColor = Color.Red
                lblMessaggio.Text = "E' necessario indicare almeno un Settore di Intervento."
                lblMessaggio.Text = lblMessaggio.Text & " </br>"
                Exit Function
            End If




        End If 'ADC 02/03/2022

        'Lascio questo controllo in modo che sia valido per i vecchi e per i nuovi enti iscritti
        If Session("IdStatoEnte") = "6" Then 'Ente fase d'iscrizione
            If Session("EsperienzeAreeSettore") Is Nothing Then
                lblMessaggio.Text = lblMessaggio.Text & "Non è stato selezionato nessun settore con le relative esperienze"
                lblMessaggio.Text = lblMessaggio.Text & " </br>"
                result = False
            Else
                EsperienzeAreeSettore = CType(Session("EsperienzeAreeSettore"), List(Of clsEsperienzeAree))
                If EsperienzeAreeSettore.Count = 0 Then
                    lblMessaggio.Text = lblMessaggio.Text & "Non è stato selezionato nessun settore con le relative esperienze"
                    lblMessaggio.Text = lblMessaggio.Text & " </br>"
                    result = False
                Else
                    'devo verificare che, per TUTTI i settori inseriti, ci siano esperienze inserite
                    For Each gRow As GridViewRow In dtgSettori.Rows
                        Dim check As CheckBox = DirectCast(gRow.FindControl("chkSeleziona"), CheckBox)
                        If check.Checked Then
                            Dim HFIdMacroAttivita As HiddenField = DirectCast(gRow.FindControl("HFIdMacroAttivita"), HiddenField)
                            If Not EsperienzeAreeSettore.Exists(Function(s) s.IdSettore = Integer.Parse(HFIdMacroAttivita.Value)) Then
                                lblMessaggio.Text = lblMessaggio.Text & "Sono stati selezionati dei settori di cui si richiede l'inserimento delle esperienze"
                                lblMessaggio.Text = lblMessaggio.Text & " </br>"
                                result = False
                                Exit For
                            End If
                        End If
                    Next
                End If
            End If
        Else
            'devo verificare che, per TUTTI i settori NUOVI inseriti, ci siano esperienze inserite quindi testo anche la visibilità del pulsante inserisci
            EsperienzeAreeSettore = CType(Session("EsperienzeAreeSettore"), List(Of clsEsperienzeAree))
            For Each gRow As GridViewRow In dtgSettori.Rows
                Dim cmdInserisci As Button = DirectCast(gRow.FindControl("cmdModifica"), Button)
                Dim check As CheckBox = DirectCast(gRow.FindControl("chkSeleziona"), CheckBox)
                If check.Checked AndAlso cmdInserisci.Visible Then
                    If EsperienzeAreeSettore IsNot Nothing Then
                        Dim HFIdMacroAttivita As HiddenField = DirectCast(gRow.FindControl("HFIdMacroAttivita"), HiddenField)
                        If Not EsperienzeAreeSettore.Exists(Function(s) s.IdSettore = Integer.Parse(HFIdMacroAttivita.Value)) Then
                            lblMessaggio.Text = lblMessaggio.Text & "Sono stati selezionati dei settori di cui si richiede l'inserimento delle esperienze"
                            lblMessaggio.Text = lblMessaggio.Text & " </br>"
                            result = False
                            Exit For
                        End If
                    Else
                        lblMessaggio.Text = lblMessaggio.Text & "Sono stati selezionati dei settori di cui si richiede l'inserimento delle esperienze"
                        lblMessaggio.Text = lblMessaggio.Text & " </br>"
                        result = False
                        Exit For
                    End If
                End If
            Next
        End If

        If txtEmailpec.Text.Trim <> String.Empty AndAlso matchPEC.Success = False Then
            lblMessaggio.Text = lblMessaggio.Text & "Il campo 'Email PEC' non e' nel formato valido."
            lblMessaggio.Text = lblMessaggio.Text & " </br>"
            result = False
        End If

        If txtEmailpec.Text.Trim <> String.Empty _
            AndAlso txtemail.Text.Trim <> String.Empty Then
            If txtemail.Text.Trim = txtEmailpec.Text.Trim Then
                lblMessaggio.Text = lblMessaggio.Text & "Il campo 'Email e il campo Email PEC non possono essere uguali"
                lblMessaggio.Text = lblMessaggio.Text & " </br>"
                result = False
            End If
        End If


        pLog.Add("Email", txtemail.Text.Trim)
        If txtemail.Text.Trim = String.Empty Then

            lblMessaggio.Text = lblMessaggio.Text & "Inserire il campo Email."
            lblMessaggio.Text = lblMessaggio.Text & " </br>"
            result = False
        End If

        Dim matchEmail As Match = regex.Match(txtemail.Text.Trim)
        If matchEmail.Success = False Then
            lblMessaggio.Text = lblMessaggio.Text & "Il campo 'Email' non e' nel formato valido."
            lblMessaggio.Text = lblMessaggio.Text & " </br>"
            result = False
        End If


        pLog.Add("DenEnte", txtdenominazione.Text)
        If txtdenominazione.Text.Trim = String.Empty Then

            lblMessaggio.Text = lblMessaggio.Text & "Inserire Denominazione."
            lblMessaggio.Text = lblMessaggio.Text & " </br>"
            result = False

        End If

        pLog.Add("CodFiscaleEnte", txtCodFis.Text)
        If txtCodFis.Text.Trim = String.Empty AndAlso lblStato.Text <> "Ex SCN con Prog" Then

            lblMessaggio.Text = lblMessaggio.Text & "Inserire il Codice Fiscale."
            lblMessaggio.Text = lblMessaggio.Text & " </br>"
            result = False

        End If

        If ddlTipologia.SelectedItem IsNot Nothing Then
            If ddlTipologia.SelectedItem.Text = String.Empty Then
                lblMessaggio.Text = lblMessaggio.Text & "Selezionare il Tipo Ente (Pubblico - Privato)"
                lblMessaggio.Text = lblMessaggio.Text & " </br>"
                result = False
            End If
        Else
            pLog.Add("TipoEnte", ddlTipologia.SelectedItem.Text)
        End If

        If ddlGiuridica.SelectedItem IsNot Nothing Then
            If ddlGiuridica.SelectedItem.Text = String.Empty Then
                lblMessaggio.Text = lblMessaggio.Text & "Selezionare la Denominazione tipologia (Università - Fondazione etc.)"
                lblMessaggio.Text = lblMessaggio.Text & " </br>"
                result = False
            Else
                If ddlGiuridica.SelectedItem.Text = "Altro ai sensi dell’Art. 1, comma 2, D.Lgs. 30-3-2001 n. 165 (specificare)" Then
                    If txtAltraTipoEnte.Text = String.Empty Then
                        lblMessaggio.Text = lblMessaggio.Text & "Inserire il campo 'Altra tipologia di ente'"
                        lblMessaggio.Text = lblMessaggio.Text & " </br>"
                        result = False
                    Else
                        If txtAltraTipoEnte.Text.Length < 2 Then
                            lblMessaggio.Text = lblMessaggio.Text & "Il campo 'Altra tipologia di ente' non può essere minore di due caratteri"
                            lblMessaggio.Text = lblMessaggio.Text & " </br>"
                            result = False
                        End If
                    End If
                End If
                pLog.Add("DenTipologia", ddlGiuridica.SelectedItem.Text)
            End If
        Else
            lblMessaggio.Text = lblMessaggio.Text & "Selezionare il Tipo Ente per caricare l'elenco delle denominazioni delle tipologie "
            lblMessaggio.Text = lblMessaggio.Text & " </br>"
            result = False
        End If






        'If Session("LoadedRTDP") Is Nothing Then
        '    lblMessaggio.Text = lblMessaggio.Text & "L'atto di designazione del Responsabile del trattamento dei dati personali non è stata caricato."
        '    lblMessaggio.Text = lblMessaggio.Text & " </br>"
        '    result = False
        'End If
        pLog.Add("Http", txthttp.Text)
        If txthttp.Text.Trim = String.Empty Then
            lblMessaggio.Text = lblMessaggio.Text & "Inserire il campo Http."
            lblMessaggio.Text = lblMessaggio.Text & " </br>"
            result = False
        End If


        'pLog.Add("RichiedenteAccount", txtRichiedente.Text.Trim)
        'If txtRichiedente.Text.Trim = String.Empty Then
        '    lblMessaggio.Text = lblMessaggio.Text & "Inserire il Richiedente Account."
        '    lblMessaggio.Text = lblMessaggio.Text & " </br>"
        '    result = False
        'End If

        If txtprefisso.Text.Trim = String.Empty Then
            lblMessaggio.Text = lblMessaggio.Text & "Inserire il prefisso Telefonico."
            lblMessaggio.Text = lblMessaggio.Text & " </br>"
            result = False
        End If

        Dim prefissoTelefono As Integer
        Dim prefissoTelefonoInteger As Boolean
        prefissoTelefonoInteger = Integer.TryParse(txtprefisso.Text.Trim, prefissoTelefono)
        pLog.Add("PrefissoTel", txtprefisso.Text.Trim)

        If prefissoTelefonoInteger = False Then
            lblMessaggio.Text = lblMessaggio.Text & "Il Prefisso Telefonico puo' contenere solo numeri."
            lblMessaggio.Text = lblMessaggio.Text & " </br>"
            result = False
        End If

        pLog.Add("Telefono", txtTelefono.Text.Trim)

        If txtTelefono.Text.Trim = String.Empty Then
            lblMessaggio.Text = lblMessaggio.Text & "Inserire il numero Telefonico."
            lblMessaggio.Text = lblMessaggio.Text & " </br>"
            result = False
        End If

        Dim numeroTelefono As Int64
        Dim numeroTelefonoInteger As Boolean
        numeroTelefonoInteger = Int64.TryParse(txtTelefono.Text.Trim, numeroTelefono)

        If numeroTelefonoInteger = False Then
            lblMessaggio.Text = lblMessaggio.Text & "Il Telefono puo' contenere solo numeri."
            lblMessaggio.Text = lblMessaggio.Text & " </br>"
            result = False
        End If


        Dim prefissoFax As Integer
        Dim prefissoFaxInteger As Boolean
        prefissoFaxInteger = Integer.TryParse(txtprefissofax.Text.Trim, prefissoFax)

        pLog.Add("prefissoFax", txtprefissofax.Text.Trim)

        If txtprefissofax.Text.Trim <> String.Empty AndAlso prefissoFaxInteger = False Then
            lblMessaggio.Text = lblMessaggio.Text & "Il Prefisso Fax puo' contenere solo numeri."
            lblMessaggio.Text = lblMessaggio.Text & " </br>"
            result = False
        End If

        Dim numeroFax As Integer
        Dim numeroFaxInteger As Boolean
        numeroFaxInteger = Integer.TryParse(txtFax.Text.Trim, numeroFax)

        pLog.Add("Fax", txtFax.Text.Trim)

        If txtFax.Text.Trim <> String.Empty AndAlso numeroFaxInteger = False Then

            lblMessaggio.Text = lblMessaggio.Text & "Il Fax puo' contenere solo numeri."
            lblMessaggio.Text = lblMessaggio.Text & " </br>"
            result = False
        End If

        pLog.Add("DeliberaGestioneStruttura", txtEstremiDSG.Text.Trim)
        If txtEstremiDSG.Text.Trim.Length > 254 Then

            lblMessaggio.Text = lblMessaggio.Text & "Attenzione, e' stato raggiunto il numero massimo di caratteri per il campo Estremi Delibera."
            lblMessaggio.Text = lblMessaggio.Text & " </br>"
            result = False
        End If

        pLog.Add("IndirizzoSedeLegale", txtIndirizzo.Text.Trim)
        If txtIndirizzo.Text.Trim = String.Empty Then

            lblMessaggio.Text = lblMessaggio.Text & "Inserire l'indirizzo della sede Legale."
            lblMessaggio.Text = lblMessaggio.Text & " </br>"
            result = False

        End If

        pLog.Add("CivicoSedeLegale", txtCivico.Text.Trim)
        If txtCivico.Text.Trim = String.Empty Then

            lblMessaggio.Text = lblMessaggio.Text & "Inserire il numero civico della sede Legale."
            lblMessaggio.Text = lblMessaggio.Text & " </br>"
            result = False

        End If

        If ddlProvincia.SelectedIndex <= 0 Then

            lblMessaggio.Text = lblMessaggio.Text & "Inserire la provincia della sede Legale."
            lblMessaggio.Text = lblMessaggio.Text & " </br>"
            result = False
        Else
            pLog.Add("ProvinciaSedeLegale", ddlProvincia.SelectedIndex)
        End If

        If ddlComune.SelectedIndex <= 0 Then

            lblMessaggio.Text = lblMessaggio.Text & "Inserire il comune della sede Legale."
            lblMessaggio.Text = lblMessaggio.Text & " </br>"
            result = False
        Else
            pLog.Add("ComuneSedeLegale", ddlComune.SelectedIndex)
        End If

        pLog.Add("CAPSedeLEgale", txtCAP.Text.Trim)
        If txtCAP.Text.Trim = String.Empty And ChkEstero.Checked = False Then
            lblMessaggio.Text = lblMessaggio.Text & "Inserire il CAP della sede Legale."
            lblMessaggio.Text = lblMessaggio.Text & " </br>"
            result = False
        End If


        pLog.Add("dataRicezioneCartacea", txtDataRicezioneCartacea.Text.Trim)
        Dim dataRicezioneCartacea As Date
        If (txtDataRicezioneCartacea.Text.Trim <> String.Empty AndAlso Date.TryParse(txtDataRicezioneCartacea.Text, dataRicezioneCartacea) = False) Then

            lblMessaggio.Text = lblMessaggio.Text & "La 'Data Ricezione Ente' non è valida. Inserire la data nel formato GG/MM/AAAA."
            lblMessaggio.Text = lblMessaggio.Text & " </br>"
            result = False
        End If


        'If Session("LoadedAC") IsNot Nothing AndAlso txtDataCostituzione.Text = String.Empty Then
        '    lblMessaggio.Text = lblMessaggio.Text & "Se è stato inserito l'Atto Costitutivo, è obbligatorio inserire la data di costituzione dell'Ente"
        '    lblMessaggio.Text = lblMessaggio.Text & " </br>"
        '    result = False
        'End If

        pLog.Add("dataCostituzione", txtDataCostituzione.Text.Trim)
        Dim dataCostituzione As Date
        If (txtDataCostituzione.Text.Trim <> String.Empty AndAlso Date.TryParse(txtDataCostituzione.Text, dataCostituzione) = False) Then

            lblMessaggio.Text = lblMessaggio.Text & "La 'Data Costituzione' non è valida. Inserire la data nel formato GG/MM/AAAA."
            lblMessaggio.Text = lblMessaggio.Text & " </br>"
            result = False
            'ElseIf dataCostituzione > Date.Today.AddYears(-3) Then
            '    lblMessaggio.Text = lblMessaggio.Text & "L'Ente non ottempera al vincolo di avere 3 anni di esperienza nel settore"
            '    lblMessaggio.Text = lblMessaggio.Text & " </br>"
            '    result = False
        End If

        If ddlClassi.SelectedItem IsNot Nothing Then
            If ddlClassi.SelectedItem.Text.Trim() = "Seleziona" Then
                lblMessaggio.Text = lblMessaggio.Text & "La sezione richiesta dall' ente non è stata selezionata"
                lblMessaggio.Text = lblMessaggio.Text & " </br>"
                result = False
            Else
                pLog.Add("Classi/Sezione", ddlClassi.SelectedItem.Text)
            End If
        End If

        pLog.Add("Firma", IIf(ChkFirma.Checked, "Si", "No"))
        If Not ChkFirma.Checked Then
            lblMessaggio.Text = lblMessaggio.Text & "La casella dichiarante il possesso di Firma Elettronica non è stata selezionata"
            lblMessaggio.Text = lblMessaggio.Text & " </br>"
            result = False
        End If

        Dim _settoriReinseriti As String = VerificaSettoriCancellatiReinseriti()
        If Not String.IsNullOrEmpty(_settoriReinseriti) Then
            If _settoriReinseriti.Length > 1 Then
                lblMessaggio.Text = lblMessaggio.Text & "I settori " & _settoriReinseriti & " sono stati cancellati e reinseriti. " & If(cmdAnnullaModifica.Visible, "Annullare le modifiche", "Impossibile salvare le modifiche")
                lblMessaggio.Text = lblMessaggio.Text & " </br>"
            Else
                lblMessaggio.Text = lblMessaggio.Text & "Il settore " & _settoriReinseriti & " è stato cancellato e reinserito. " & If(cmdAnnullaModifica.Visible, "Annullare le modifiche", "Impossibile salvare le modifiche")
                lblMessaggio.Text = lblMessaggio.Text & " </br>"
            End If
            result = False
        End If


        If Not result Then
            MessaggiAlert(lblMessaggio.Text)
        End If

        Return result

    End Function

    Private Sub imgAccordo_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgAccordo.Click

        Response.Write("<SCRIPT>" & vbCrLf)
        Response.Write("window.open('WfrmVisualizzaAccordoEnte.aspx','Accordo','width=650,height=350,toolbar=no,location=no,menubar=no,scrollbars=yes,resizable=yes')")
        Response.Write("</SCRIPT>")

    End Sub

    Private Sub imbComuni_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imbComuni.Click
        Response.Write("<SCRIPT>" & vbCrLf)
        Response.Write("window.open('WFrmAssociaEntiComuni.aspx?Id=" & Session("IdEnte") & "&Blocco=" & txtBlocco.Value & "&tipoAmbito=" & ddlGiuridica.SelectedValue & "&tipoSettore=" & ddlTipologia.SelectedValue & "','Comuni','height=500,toolbar=no,location=no,menubar=no,scrollbars=yes,resizable=yes')")
        Response.Write("</SCRIPT>")
    End Sub

    Private Sub imgCronologiaDocumenti_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgCronologiaDocumenti.Click
        Response.Write("<SCRIPT>" & vbCrLf)
        Response.Write("window.open('cronologiadocumenti.aspx?','CronologiaDocumenti','height=500,toolbar=no,location=no,menubar=no,scrollbars=yes,resizable=yes')")
        Response.Write("</SCRIPT>")
    End Sub

    Protected Sub ImgCronologiaProgetti_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImgCronologiaProgetti.Click
        Response.Write("<SCRIPT>" & vbCrLf)
        Response.Write("window.open('cronologiadocumenti.aspx?VengoDa=progetti','CronologiaProgetti','height=500,toolbar=no,location=no,menubar=no,scrollbars=yes,resizable=yes')")
        Response.Write("</SCRIPT>")
    End Sub

    Protected Sub ImgEmail_Pec_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImgEmail_Pec.Click
        Response.Write("<SCRIPT>" & vbCrLf)
        Response.Write("window.open('cronologiaMailEnti.aspx','cronologia','height=500,toolbar=no,location=no,menubar=no,scrollbars=yes,resizable=yes')")
        Response.Write("</SCRIPT>")
    End Sub

    Private Sub cmdVisualizzaDatiAccreditati_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdVisualizzaDatiAccreditati.Click
        Response.Write("<SCRIPT>" & vbCrLf)
        Response.Write("window.open ('informazioniente.aspx','info','height=600,menubar=no,scrollbars=yes,resizable=yes')")
        Response.Write("</SCRIPT>")
    End Sub

    Private Sub infoClassi_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles infoClassi.Click
        Response.Write("<SCRIPT>" & vbCrLf)
        Response.Write("window.open('WfrmInfoClassi.aspx?1=1', 'InfoClassi', 'height=320,width=450,toolbar=no,location=no,menubar=no,scrollbars=yes')")
        Response.Write("</SCRIPT>")
    End Sub

    Private Sub GestisciStyleDatiModificati()
        Dim dtrGenerico As SqlClient.SqlDataReader
        Dim strsql As String
        Dim MyCommand As SqlClient.SqlDataAdapter
        Dim DsSedi As DataSet = New DataSet
        Dim idEnte As Int32 = Session("IdEnte")
        Dim ALBOENTE As String = ClsUtility.TrovaAlboEnte(idEnte, Session("conn"))

        strsql = "select day(datacontrolloemail)as ggDCEmail,month(datacontrolloemail)as monthDCEmail,year(datacontrolloemail)as yearDCEmail," &
                  " day(datacontrollohttp)as ggDChttp,month(datacontrollohttp)as monthDChttp,year(datacontrollohttp)as yearDChttp, statienti.statoente," &
                  " classiaccreditamento.classeaccreditamento,classiaccreditamento.EntiInPartenariato, " &
                  " enti.Tipologia,enti.CodiceFiscale,isnull(enti.CodiceRegione,'Assente')as codiceregione," &
                  " enti.Datacontrolloemail,enti.Datacontrollohttp," &
                  " enti.dataCostituzione,enti.DataRicezioneCartacea,enti.Denominazione,enti.EstremiDeliberaStrutturaGestione," &
                  " enti.http,enti.Datacontrollohttp,enti.httpvalido," &
                  " enti.Email,enti.EmailCertificata,enti.Datacontrolloemail,enti.emailvalido,enti.PartitaIva,enti.NoteRichiestaRegistrazione," &
                  " enti.TelefonoRichiestaRegistrazione, CASE isnull(Enti.Firma,0) WHEN 0 THEN 'NO' ELSE 'SI' END AS FIRMA,  " &
                  " enti.PrefissoTelefonoRichiestaRegistrazione,enti.PrefissoFax,enti.Fax,entipassword.Username,enti.dataultimaClasseaccreditamento," &
                  " enti.idclasseaccreditamentorichiesta,classiaccreditamentorichieste.classeaccreditamento as classeaccreditamentorichiesta,enti.idclasseaccreditamento,isnull(enti.CodiceFiscaleArchivio,'') as CodiceFiscaleArchivio, isnull(enti.PartitaIVAArchivio,'') as PartitaIVAArchivio " &
                  " ,SezioniAlboSCU.Sezione " &
                  " ,DataNominaRL " &
                  " from enti " &
                  " inner join classiaccreditamento on (classiaccreditamento.idclasseaccreditamento=enti.idclasseaccreditamento)" &
                  " inner join statienti on (statienti.idstatoente=enti.idstatoente) " &
                  " left join SezioniAlboSCU on enti.idsezione = SezioniAlboSCU.idsezione " &
                  " left join entipassword on (enti.idente=entipassword.idente)" &
                    " left join classiaccreditamento as classiaccreditamentorichieste on (classiaccreditamentorichieste.idclasseaccreditamento=enti.idclasseaccreditamentorichiesta)" &
                  " where enti.idente=" & idEnte & ""
        If Not dtrGenerico Is Nothing Then
            dtrGenerico.Close()
            dtrGenerico = Nothing
        End If
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
            If ALBOENTE = "SCN" Then
                If Not IsDBNull(dtrGenerico("Tipologia")) Then
                    Dim tipologia As String = If(ddlTipologia.SelectedItem Is Nothing, "", ddlTipologia.SelectedItem.Text)
                    If (tipologia = "Privato") Then
                        If Not (String.Equals(tipologia, dtrGenerico("Tipologia"), StringComparison.InvariantCultureIgnoreCase)) Then
                            helper.ModificaStyleDatiModificati(ddlTipologia)
                            helper.ModificaStyleDatiModificati(ddlGiuridica)
                        Else
                            helper.RipristinaStyleDatiModificati(ddlTipologia)
                            helper.RipristinaStyleDatiModificati(ddlGiuridica)
                        End If
                    Else
                        Dim giuridica As String = If(ddlGiuridica.SelectedItem Is Nothing, "", ddlGiuridica.SelectedItem.Text)
                        If Not (String.Equals(giuridica, dtrGenerico("Tipologia"), StringComparison.InvariantCultureIgnoreCase)) Then
                            helper.ModificaStyleDatiModificati(ddlTipologia)
                            helper.ModificaStyleDatiModificati(ddlGiuridica)
                        Else
                            helper.RipristinaStyleDatiModificati(ddlTipologia)
                            helper.RipristinaStyleDatiModificati(ddlGiuridica)
                        End If
                    End If
                End If
            End If
            If ALBOENTE = "SCU" Then
                If Not IsDBNull(dtrGenerico("Tipologia")) Then
                    Dim giuridica As String = If(ddlGiuridica.SelectedItem Is Nothing, "", ddlGiuridica.SelectedItem.Text)
                    If Not (String.Equals(giuridica, dtrGenerico("Tipologia"), StringComparison.InvariantCultureIgnoreCase)) Then
                        helper.ModificaStyleDatiModificati(ddlTipologia)
                        helper.ModificaStyleDatiModificati(ddlGiuridica)
                    Else
                        helper.RipristinaStyleDatiModificati(ddlTipologia)
                        helper.RipristinaStyleDatiModificati(ddlGiuridica)
                    End If
                End If
            End If

            If Not IsDBNull(dtrGenerico("CodiceFiscale")) Then
                If Not (String.Equals(txtCodFis.Text, dtrGenerico("CodiceFiscale"), StringComparison.InvariantCultureIgnoreCase)) Then
                    helper.ModificaStyleDatiModificati(txtCodFis)
                Else
                    helper.RipristinaStyleDatiModificati(txtCodFis)
                End If
            End If
            If Not IsDBNull(dtrGenerico("NoteRichiestaRegistrazione")) Then
                If Not (String.Equals(txtRichiedente.Text, dtrGenerico("NoteRichiestaRegistrazione"), StringComparison.InvariantCultureIgnoreCase)) Then
                    helper.ModificaStyleDatiModificati(txtRichiedente)
                Else
                    helper.RipristinaStyleDatiModificati(txtRichiedente)
                End If
            End If
            If Not IsDBNull(dtrGenerico("dataCostituzione")) Then
                If Not (String.Equals(txtDataCostituzione.Text, dtrGenerico("dataCostituzione"), StringComparison.InvariantCultureIgnoreCase)) Then
                    helper.ModificaStyleDatiModificati(txtDataCostituzione)
                Else
                    helper.RipristinaStyleDatiModificati(txtDataCostituzione)
                End If
            End If
            If Not IsDBNull(dtrGenerico("http")) Then
                If Not (String.Equals(txthttp.Text, dtrGenerico("http"), StringComparison.InvariantCultureIgnoreCase)) Then
                    helper.ModificaStyleDatiModificati(txthttp)
                Else
                    helper.RipristinaStyleDatiModificati(txthttp)
                End If
            End If
            If Not IsDBNull(dtrGenerico("Email")) Then
                If Not (String.Equals(txtemail.Text, dtrGenerico("Email"), StringComparison.InvariantCultureIgnoreCase)) Then
                    helper.ModificaStyleDatiModificati(txtemail)
                Else
                    helper.RipristinaStyleDatiModificati(txtemail)
                End If
            End If
            If Not IsDBNull(dtrGenerico("classeaccreditamentorichiesta")) Then
                If dtrGenerico("classeaccreditamentorichiesta") <> "Nessuna Sezione" Then
                    Dim classi As String = If(ddlClassi.SelectedItem Is Nothing, "", ddlClassi.SelectedItem.Text)

                    If Not (String.Equals(classi, If(dtrGenerico("Sezione") Is DBNull.Value, "Seleziona", dtrGenerico("Sezione")), StringComparison.InvariantCultureIgnoreCase)) Then

                        helper.ModificaStyleDatiModificati(ddlClassi)
                    Else
                        helper.RipristinaStyleDatiModificati(ddlClassi)
                    End If
                Else
                    If ddlClassi.SelectedItem.Text <> "Seleziona" Then
                        helper.ModificaStyleDatiModificati(ddlClassi)
                    Else
                        helper.RipristinaStyleDatiModificati(ddlClassi)
                    End If
                End If

            End If

            If Not IsDBNull(dtrGenerico("PrefissoTelefonoRichiestaRegistrazione")) Then
                Dim telefono As String = IIf(Not IsDBNull(dtrGenerico("PrefissoTelefonoRichiestaRegistrazione")), dtrGenerico("PrefissoTelefonoRichiestaRegistrazione"), "") & " - " & IIf(Not IsDBNull(dtrGenerico("TelefonoRichiestaRegistrazione")), dtrGenerico("TelefonoRichiestaRegistrazione"), "")
                Dim telefonoVisualizzato As String = txtprefisso.Text & " - " & txtTelefono.Text
                If Not (String.Equals(telefonoVisualizzato, telefono, StringComparison.InvariantCultureIgnoreCase)) Then
                    helper.ModificaStyleDatiModificati(txtTelefono)
                    helper.ModificaStyleDatiModificati(txtprefisso)
                Else
                    helper.RipristinaStyleDatiModificati(txtTelefono)
                    helper.RipristinaStyleDatiModificati(txtprefisso)
                End If
            End If

            If Not IsDBNull(dtrGenerico("PrefissoFax")) Then
                Dim fax As String = IIf(Not IsDBNull(dtrGenerico("PrefissoFax")), dtrGenerico("PrefissoFax"), "") & " - " & IIf(Not IsDBNull(dtrGenerico("Fax")), dtrGenerico("Fax"), "")
                Dim faxVisualizzato As String = txtprefissofax.Text & " - " & txtFax.Text
                If Not (String.Equals(faxVisualizzato, fax, StringComparison.InvariantCultureIgnoreCase)) Then
                    helper.ModificaStyleDatiModificati(txtFax)
                    helper.ModificaStyleDatiModificati(txtprefissofax)
                Else
                    helper.RipristinaStyleDatiModificati(txtFax)
                    helper.RipristinaStyleDatiModificati(txtprefissofax)
                End If
            End If
            If Not IsDBNull(dtrGenerico("EstremiDeliberaStrutturaGestione")) Then
                If Not (String.Equals(txtEstremiDSG.Text, dtrGenerico("EstremiDeliberaStrutturaGestione"), StringComparison.InvariantCultureIgnoreCase)) Then
                    helper.ModificaStyleDatiModificati(txtEstremiDSG)
                Else
                    helper.RipristinaStyleDatiModificati(txtEstremiDSG)
                End If
            End If

            If Not IsDBNull(dtrGenerico("DataNominaRL")) Then
                If Not (String.Equals(txtDataNominaRL.Text, Date.Parse(dtrGenerico("DataNominaRL")).ToString("dd/MM/yyyy"), StringComparison.InvariantCultureIgnoreCase)) Then
                    helper.ModificaStyleDatiModificati(txtDataNominaRL)
                Else
                    helper.RipristinaStyleDatiModificati(txtDataNominaRL)
                End If
            Else
                'il vecchio dato è null
                If Not String.IsNullOrEmpty(txtDataNominaRL.Text) Then
                    helper.ModificaStyleDatiModificati(txtDataNominaRL)
                Else
                    helper.RipristinaStyleDatiModificati(txtDataNominaRL)
                End If
            End If

        End If
        strsql = "SELECT  entisedi.identesede,entisedi.idcomune,entisedi.indirizzo, entisedi.DettaglioRecapito, entisedi.civico,entisedi.cap,comuni.denominazione as comune," &
                " provincie.provincia " &
                " FROM entisedi " &
                " INNER JOIN statientisedi on statientisedi.idstatoentesede=entisedi.idstatoentesede " &
                " INNER JOIN entiseditipi ON entisedi.IDEnteSede = entiseditipi.IDEnteSede " &
                " INNER JOIN comuni ON entisedi.IDComune = comuni.IDComune " &
                " INNER JOIN provincie ON comuni.IDProvincia = provincie.IDProvincia " &
            " WHERE entisedi.IDEnte = " & idEnte & " And entiseditipi.idtiposede = 1 and (statientisedi.attiva=1 or statientisedi.DefaultStato=1)"
        If Not dtrGenerico Is Nothing Then
            dtrGenerico.Close()
            dtrGenerico = Nothing
        End If

        dtrGenerico = ClsServer.CreaDatareader(strsql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
        dtrGenerico.Read()
        If dtrGenerico.HasRows = True Then
            If Not IsDBNull(dtrGenerico("provincia")) Then
                Dim provincia As String = If(ddlProvincia.SelectedItem Is Nothing, "", ddlProvincia.SelectedItem.Text)
                If Not (String.Equals(provincia, dtrGenerico("provincia"), StringComparison.InvariantCultureIgnoreCase)) Then
                    helper.ModificaStyleDatiModificati(ddlProvincia)
                Else
                    helper.RipristinaStyleDatiModificati(ddlProvincia)
                End If
            End If
            If Not IsDBNull(dtrGenerico("comune")) Then
                Dim comune As String = If(ddlComune.SelectedItem Is Nothing, "", ddlComune.SelectedItem.Text)
                If Not (String.Equals(comune, dtrGenerico("comune"), StringComparison.InvariantCultureIgnoreCase)) Then
                    helper.ModificaStyleDatiModificati(ddlComune)
                Else
                    helper.RipristinaStyleDatiModificati(ddlComune)
                End If
            End If
            If Not IsDBNull(dtrGenerico("indirizzo")) Then
                If Not (String.Equals(txtIndirizzo.Text, dtrGenerico("indirizzo"), StringComparison.InvariantCultureIgnoreCase)) Then
                    helper.ModificaStyleDatiModificati(txtIndirizzo)
                Else
                    helper.RipristinaStyleDatiModificati(txtIndirizzo)
                End If
            End If
            If Not IsDBNull(dtrGenerico("Civico")) Then
                If Not (String.Equals(txtCivico.Text, dtrGenerico("Civico"), StringComparison.InvariantCultureIgnoreCase)) Then
                    helper.ModificaStyleDatiModificati(txtCivico)
                Else
                    helper.RipristinaStyleDatiModificati(txtCivico)
                End If
            End If
            If Not IsDBNull(dtrGenerico("Cap")) Then
                If Not (String.Equals(txtCAP.Text, dtrGenerico("Cap"), StringComparison.InvariantCultureIgnoreCase)) Then
                    helper.ModificaStyleDatiModificati(txtCAP)
                Else
                    helper.RipristinaStyleDatiModificati(txtCAP)
                End If
            End If
            If Not IsDBNull(dtrGenerico("DettaglioRecapito")) Then
                If Not (String.Equals(TxtDettaglioRecapito.Text, dtrGenerico("DettaglioRecapito"), StringComparison.InvariantCultureIgnoreCase)) Then
                    helper.ModificaStyleDatiModificati(TxtDettaglioRecapito)
                Else
                    helper.RipristinaStyleDatiModificati(TxtDettaglioRecapito)
                End If
            End If

        End If
        If Not dtrGenerico Is Nothing Then
            dtrGenerico.Close()
            dtrGenerico = Nothing
        End If


        'Verifico Allegati Stefano Rossetti 05/07/2021
        Dim dtAllegati As DataTable
        Dim _allegato = New Allegato
        RipristinaPulsantiAllegati()

        strsql = String.Empty

        strsql = String.Empty
        strsql = "SELECT Coalesce(IdAllegatoDeliberaStrutturaGestione,0) IdAllegatoDeliberaStrutturaGestione,"
        strsql += " Coalesce(IdAllegatoDocumentoNomina,0) IdAllegatoDocumentoNomina,"
        strsql += " Coalesce(IdAllegatoRTDPersonali,0) IdAllegatoRTDPersonali,"
        strsql += " Coalesce(IdAllegatoAttoCostitutivo,0) IdAllegatoAttoCostitutivo,"
        strsql += " Coalesce(IdAllegatoStatuto,0) IdAllegatoStatuto,"
        strsql += " Coalesce(IdAllegatoDeliberaAdesione,0) IdAllegatoDeliberaAdesione,"
        strsql += " Coalesce(IdAllegatoImpegnoEtico,0) IdAllegatoImpegnoEtico"
        strsql += " FROM enti WHERE idente = " & Session("idEnte")
        dtAllegati = ClsServer.DataTableGenerico(strsql, Session("Conn"))

        Try

            If dtAllegati IsNot Nothing _
            AndAlso dtAllegati.Rows.Count > 0 Then
                If Not IsNothing(Session("LoadedAC")) Then
                    _allegato = Session("LoadedAC")
                    If CInt(_allegato.Id.ToString()) <> CInt(dtAllegati.Rows(0)("IdAllegatoAttoCostitutivo")) Then
                        helper.ModificaStyleDatiModificati(btnDownloadAttoCostitutivo)
                        helper.ModificaStyleDatiModificati(btnEliminaAttoCostitutitvo)
                        helper.ModificaStyleDatiModificati(btnModificaAttoCostitutivo)
                    End If
                End If
                If Not IsNothing(Session("LoadedStatuto")) Then
                    _allegato = Session("LoadedStatuto")
                    If CInt(_allegato.Id.ToString()) <> CInt(dtAllegati.Rows(0)("IdAllegatoStatuto")) Then
                        helper.ModificaStyleDatiModificati(btnDownloadStatuto)
                        helper.ModificaStyleDatiModificati(btnEliminaStatuto)
                        helper.ModificaStyleDatiModificati(btnModificaStatuto)
                    End If
                End If
                If Not IsNothing(Session("LoadedDeliberaAdesione")) Then
                    _allegato = Session("LoadedDeliberaAdesione")
                    If AllegatoModificato(_allegato, dtAllegati.Rows(0)("IdAllegatoDeliberaAdesione")) Then
                        helper.ModificaStyleDatiModificati(btnDownloadDeliberaAdesione)
                        helper.ModificaStyleDatiModificati(btnEliminaDeliberaAdesione)
                        helper.ModificaStyleDatiModificati(btnModificaDeliberaAdesione)
                    End If
                End If
                If Not IsNothing(Session("LoadedCartaImpegnoEtico")) Then
                    _allegato = Session("LoadedCartaImpegnoEtico")
                    If AllegatoModificato(_allegato, dtAllegati.Rows(0)("IdAllegatoImpegnoEtico")) Then
                        helper.ModificaStyleDatiModificati(btnDownloadCIE)
                        helper.ModificaStyleDatiModificati(btnEliminaCIE)
                        helper.ModificaStyleDatiModificati(btnModificaCIE)
                    End If
                End If
                If Not IsNothing(Session("LoadedAN")) Then
                    _allegato = Session("LoadedAN")
                    If AllegatoModificato(_allegato, dtAllegati.Rows(0)("IdAllegatoDocumentoNomina")) Then
                        helper.ModificaStyleDatiModificati(btnDownloadAN)
                        helper.ModificaStyleDatiModificati(btnEliminaAN)
                        helper.ModificaStyleDatiModificati(btnModificaAN)
                    End If
                End If
                If Not IsNothing(Session("LoadedDelibera")) Then
                    _allegato = Session("LoadedDelibera")
                    If AllegatoModificato(_allegato, dtAllegati.Rows(0)("IdAllegatoDeliberaStrutturaGestione")) Then
                        helper.ModificaStyleDatiModificati(btnDownloadDelibera)
                        helper.ModificaStyleDatiModificati(btnEliminaDelibera)
                        helper.ModificaStyleDatiModificati(btnModificaDelibera)
                    End If
                End If
            End If
        Catch ex As Exception

        End Try
        'ModificaStyleEntiSettori(idEnte)
        ModificaStyleEntiSettori()
    End Sub

    Sub ModificaStyleEsperienze(_row As DataRow(), nomeCampoEsperienza As String, textboxEsperienza As TextBox, btnCronologia As Button)
        If _row.Length > 0 Then
            'si da per scontato che ci sia solo la prima riga
            If IsDBNull(_row(0)(nomeCampoEsperienza)) Then
                If String.IsNullOrEmpty(textboxEsperienza.Text) Then
                    helper.RipristinaStyleDatiModificati(textboxEsperienza)
                    btnCronologia.Visible = False
                Else
                    helper.ModificaStyleDatiModificati(textboxEsperienza)   'dati non esistenti come dati reali ma esistenti in maschera
                    btnCronologia.Visible = True And Not Session("TipoUtente") = "E"
                End If
            Else
                If String.IsNullOrEmpty(textboxEsperienza.Text) OrElse Not String.Equals(textboxEsperienza.Text, _row(0)(nomeCampoEsperienza).ToString, StringComparison.InvariantCultureIgnoreCase) Then
                    helper.ModificaStyleDatiModificati(textboxEsperienza)   'dati diversi tra maschera e dati reali (comprende il caso di dati cancellati in maschera)
                    btnCronologia.Visible = True And Not Session("TipoUtente") = "E"
                Else
                    helper.RipristinaStyleDatiModificati(textboxEsperienza)
                    btnCronologia.Visible = False
                End If
            End If
        Else
            If String.IsNullOrEmpty(textboxEsperienza.Text) Then
                helper.RipristinaStyleDatiModificati(textboxEsperienza)
                btnCronologia.Visible = False
            Else
                helper.ModificaStyleDatiModificati(textboxEsperienza)       'dati non esistenti come dati reali ma esistenti in maschera
                btnCronologia.Visible = True And Not Session("TipoUtente") = "E"
            End If
        End If
    End Sub

    Sub ModificaStyleEntiSettori()
        'Confronta i dati caricati in maschera (Settori/aree/esperienze/ambiti) con i dati che si caricherebbero dalle tabelle reali:
        'se siamo in variazione potrebbero essere diversi e quindi dobbiamo indicare quali sono i dati modificati
        'in caso di modifica si agisce sulle check delle macroaree e sulle textbox delle esperienze

        Dim EsperienzeAreeSettore As New List(Of clsEsperienzeAree) 'contiene i dati caricati in maschera da confrontare con quelli reali 
        'NB NON contiene le macroaree che non hanno esperienze ma che sono selezionate (caso di enti preesistenti), vanno trovate in maschera

        If Session("EsperienzeAreeSettore") IsNot Nothing Then EsperienzeAreeSettore = CType(Session("EsperienzeAreeSettore"), List(Of clsEsperienzeAree))

        'strsql = "SELECT ES.IDMACROAMBITOATTIVITÀ IDMACROATTIVITA,EES.IDSETTORE, EES.I_ESPERIENZA,EES.II_ESPERIENZA,EES.III_ESPERIENZA,"
        'strsql = strsql & " EES.I_ANNOESPERIENZA,EES.II_ANNOESPERIENZA,EES.III_ANNOESPERIENZA,EES.AREEINTERVENTO, MA.MacroAmbitoAttività"
        'strsql = strsql & " FROM ENTISETTORI ES "
        'strsql = strsql & " LEFT JOIN ENTEESPERIENZASETTORE EES"
        'strsql = strsql & " ON ES.IDENTE = EES.IDENTE"
        'strsql = strsql & " AND ES.IDMACROAMBITOATTIVITÀ = EES.IDSETTORE"
        'strsql = strsql & " LEFT JOIN macroambitiattività MA on MA.IDMacroAmbitoAttività = es.IdMacroAmbitoAttività"
        'strsql = strsql & " WHERE ES.IDENTE = " & Session("IdEnte") & " AND EES.DATAFINEVALIDITA IS NULL "

        Dim dtSettori As New DataTable
        'dtSettori = ClsServer.DataTableGenerico(strsql, Session("Conn"))

        Session("ElencoSettoriPrecedenti") = Nothing
        dtSettori = ElencoSettoriEsperienzePrecedenti()
        Session("ElencoSettoriPrecedenti") = dtSettori

        'devo ciclare la maschera per fare tutti i confronti
        For Each gRow As GridViewRow In dtgSettori.Rows
            Dim chkMacroArea As CheckBox = DirectCast(gRow.FindControl("chkSeleziona"), CheckBox)
            'individuo la macroarea in maschera
            Dim HFIdMacroAttivita As HiddenField = DirectCast(gRow.FindControl("HFIdMacroAttivita"), HiddenField)
            'cerco la macroarea nei dati reali  il check precedente (dati reali) è sul campo IDMACROATTIVITA
            Dim _row As DataRow() = dtSettori.Select("IDMACROATTIVITA=" + HFIdMacroAttivita.Value)  'cerco il campo che indica che il settore era selezionato

            'controllo se è stata selezionata in maschera e confronto con il dato reale
            If (_row.Length = 0 And chkMacroArea.Checked) Or (_row.Length > 0 And Not chkMacroArea.Checked) Then
                helper.ModificaStyleDatiModificati(chkMacroArea)
            Else
                helper.RipristinaStyleDatiModificati(chkMacroArea)
            End If

            'uso il trucco di confrontare con le esperienze precedenti SOLO se si può modificarle cioè è abilitato il pulsante inserisci
            'se si dovrà abilitarlo sempre per utenti dipartimento si dovrà usare la Session("ElencoSettoriDB") e il campo NuovoInserimento
            Dim cmdInserisci As Button = DirectCast(gRow.FindControl("cmdModifica"), Button)

            'se la macroarea è chekkata allora ci sono le textbox delle esperienze ed ha senso vedere se evidenziare eventuali modifiche
            If chkMacroArea.Checked And Not Session("TipoUtente") = "E" And Not cmdInserisci Is Nothing AndAlso cmdInserisci.Visible Then
                _row = dtSettori.Select("IDSETTORE=" + HFIdMacroAttivita.Value)
                'individuo le esperienze in maschera ed i bottoni della cronologia da rendere visibili nel caso serva
                Dim txtDesEsperienza3 As TextBox = gRow.Cells(2).FindControl("txtDesEsperienza3")
                Dim txtDesEsperienza2 As TextBox = gRow.Cells(2).FindControl("txtDesEsperienza2")
                Dim txtDesEsperienza1 As TextBox = gRow.Cells(2).FindControl("txtDesEsperienza1")
                Dim bntVariazioni3 As Button = gRow.Cells(2).FindControl("btnEsp3")
                Dim bntVariazioni2 As Button = gRow.Cells(2).FindControl("btnEsp2")
                Dim bntVariazioni1 As Button = gRow.Cells(2).FindControl("btnEsp1")

                'controllo esperienze eliminate/modificate
                ModificaStyleEsperienze(_row, "I_ESPERIENZA", txtDesEsperienza3, bntVariazioni3)
                ModificaStyleEsperienze(_row, "II_ESPERIENZA", txtDesEsperienza2, bntVariazioni2)
                ModificaStyleEsperienze(_row, "III_ESPERIENZA", txtDesEsperienza1, bntVariazioni1)
            End If
        Next

    End Sub

    Sub ModificaStyleEntiSettori(ByVal idEnte As Int32)
        ''aggiunto il 20/08/2014 da Simona Cordella
        'Dim dtsSettori As DataSet
        ''variabile stringa locale per costruire la query per le aree
        'Dim strSql As String
        'Dim result As New List(Of String)
        'strSql = "SELECT macroambitiattività.IDMacroAmbitoAttività  FROM macroambitiattività INNER JOIN EntiSettori ON macroambitiattività.IDMacroAmbitoAttività = EntiSettori.IdMacroAmbitoAttività WHERE  EntiSettori.IdEnte = " & idEnte

        'dtsSettori = ClsServer.DataSetGenerico(strSql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))


        ''controllo se ci sono dei record
        'If dtsSettori.Tables(0).Rows.Count > 0 Then
        '    result = ConvertiDataTableInListaDiStringhe(dtsSettori)
        '    Dim check As CheckBox
        '    Dim idMacroAmbito As String
        '    Dim idMacroAmbitoVariazione As String
        '    For i As Integer = 0 To dtgSettori.Rows.Count - 1
        '        check = dtgSettori.Rows.Item(i).FindControl("chkSeleziona")
        '        idMacroAmbito = dtgSettori.Rows.Item(i).Cells(1).Text
        '        idMacroAmbitoVariazione = (From item In result Where item.ToString() = idMacroAmbito.ToString).FirstOrDefault

        '        If (((idMacroAmbito = idMacroAmbitoVariazione And check.Checked = False)) Or
        '            ((idMacroAmbitoVariazione Is Nothing Or idMacroAmbitoVariazione = String.Empty) And check.Checked = True)) Then
        '            helper.ModificaStyleDatiModificati(dtgSettori)
        '            Exit For
        '        Else
        '            helper.RipristinaStyleDatiModificati(dtgSettori)
        '        End If
        '    Next
        'End If

        Try
            Dim SqlCmd As SqlClient.SqlCommand
            '            Dim Dt As New DataTable
            '           Dim Da As New SqlClient.SqlDataAdapter(SqlCmd)

            SqlCmd = New SqlClient.SqlCommand
            SqlCmd.CommandText = "SP_ACCREDITAMENTO_VERIFICA_VARIAZIONE_SETTORI"
            SqlCmd.CommandType = CommandType.StoredProcedure
            SqlCmd.Connection = Session("Conn")
            SqlCmd.Parameters.Add("@IdEnte", SqlDbType.Int).Value = idEnte

            Dim sparam1 As SqlClient.SqlParameter
            sparam1 = New SqlClient.SqlParameter
            sparam1.ParameterName = "@VARIAZIONE"
            sparam1.SqlDbType = SqlDbType.TinyInt
            sparam1.Direction = ParameterDirection.Output
            SqlCmd.Parameters.Add(sparam1)

            Dim sparam6 As SqlClient.SqlParameter
            sparam6 = New SqlClient.SqlParameter
            sparam6.ParameterName = "@messaggio"
            sparam6.Size = 1000
            sparam6.SqlDbType = SqlDbType.VarChar
            sparam6.Direction = ParameterDirection.Output
            SqlCmd.Parameters.Add(sparam6)

            SqlCmd.ExecuteNonQuery()

            If SqlCmd.Parameters("@VARIAZIONE").Value = 1 Then
                helper.ModificaStyleDatiModificati(dtgSettori)
            Else
                helper.RipristinaStyleDatiModificati(dtgSettori)
            End If

        Catch ex As Exception
            lblMessaggio.Text = ex.Message
        Finally
            'ConnX.Close()
            'SqlCmd = Nothing
        End Try
    End Sub
    Private Function ConvertiDataTableInListaDiStringhe(info As DataSet) As List(Of String)
        Dim result As New List(Of String)
        For Each table As DataTable In info.Tables
            For Each row As DataRow In table.Rows
                For Each column As DataColumn In table.Columns
                    result.Add(row.Item(column))
                Next

            Next
        Next
        Return result
    End Function

    Private Sub ddlComune_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddlComune.SelectedIndexChanged
        txtIDComunes.Value = ddlComune.SelectedValue
    End Sub
    Private Sub ddlComune_TextChanged(sender As Object, e As System.EventArgs) Handles ddlComune.TextChanged
        If ddlComune.SelectedItem.Text <> "" Then
            txtIDComunes.Value = ddlComune.SelectedValue
        End If
    End Sub

    Protected Sub OpenWindow()
        Dim codfis As String = txtCodFis.Text
        Dim url As String = "WfrmNavigaEnte.aspx?CodiceFiscale=" & codfis
        Dim s As String = "window.open('" & url + "', 'popup_window', 'width=1024,height=500,left=100,top=100,scrollbars=yes,resizable=yes');"
        Page.ClientScript.RegisterStartupScript(Me.GetType(), "script", s, True)

    End Sub

    Protected Sub CmdInforScu1_Click(sender As Object, e As EventArgs) Handles CmdInforScu1.Click
        OpenWindow()
    End Sub

    Protected Sub ChkChange(sender As Object, e As EventArgs)
        Dim gr As GridViewRow
        Dim chkBox As CheckBox
        Dim url As String
        Dim istruzione As String
        'Dim EsperienzeSettore As New clsAmbitiEsperienza
        Dim contaRiga As Integer = 3
        Dim divAreaIntervento As HtmlGenericControl
        Dim IDSettore As String
        Dim divAree As HtmlGenericControl
        Dim lblAnno3 As Label
        Dim lblAnno2 As Label
        Dim lblAnno1 As Label
        Dim txtEsperienza3 As TextBox
        Dim txtEsperienza2 As TextBox
        Dim txtEsperienza1 As TextBox
        chkBox = CType(sender, CheckBox)
        Dim EsperienzeAreeSettore As New List(Of clsEsperienzeAree)
        Dim dtSettori As New DataTable

        gr = chkBox.Parent.Parent
        IDSettore = CStr(dtgSettori.DataKeys(gr.RowIndex).Values(0))

        'Dim cmdInserisci As Button = DirectCast(gr.FindControl("cmdModifica"), Button)

        'If SettoriEsperienzeDB IsNot Nothing AndAlso SettoriEsperienzeDB.Count > 0 Then
        '    If SettoriEsperienzeDB("MacroAttivita_" & IDSettore) IsNot Nothing AndAlso SettoriEsperienzeDB("MacroAttivita_" & IDSettore) <> String.Empty Then
        '        If SettoriEsperienzeDB("IDSettore_" & IDSettore) IsNot Nothing AndAlso SettoriEsperienzeDB("IDSettore_" & IDSettore) <> String.Empty Then
        '            cmdInserisci.Visible = True
        '        Else
        '            cmdInserisci.Visible = False
        '        End If
        '    Else
        '        cmdInserisci.Visible = True
        '    End If

        'End If

        divAreaIntervento = dtgSettori.Rows(gr.RowIndex).Cells(2).FindControl("divAreaIntervento") 'recupero la div annidata nella griglia

        lblAnno3 = dtgSettori.Rows(gr.RowIndex).Cells(2).FindControl("lblAnnoEsperienza3")
        lblAnno2 = dtgSettori.Rows(gr.RowIndex).Cells(2).FindControl("lblAnnoEsperienza2")
        lblAnno1 = dtgSettori.Rows(gr.RowIndex).Cells(2).FindControl("lblAnnoEsperienza1")
        txtEsperienza3 = dtgSettori.Rows(gr.RowIndex).Cells(2).FindControl("txtDesEsperienza3")
        txtEsperienza2 = dtgSettori.Rows(gr.RowIndex).Cells(2).FindControl("txtDesEsperienza2")
        txtEsperienza1 = dtgSettori.Rows(gr.RowIndex).Cells(2).FindControl("txtDesEsperienza1")
        divAree = dtgSettori.Rows(gr.RowIndex).Cells(2).FindControl("divArea")
        Dim cmdInserisci As Button = DirectCast(gr.FindControl("cmdModifica"), Button)
        If chkBox.Checked Then
            divAreaIntervento.Style("display") = "block"
            divAree.Style("display") = "block"
            cmdInserisci.Visible = True
            CaricaAreeIntervento(IDSettore)
            CaricaAnniEsperienza()
        Else
            'Page.ClientScript.RegisterStartupScript(Me.GetType(), "confirm", "<script type=text/javascript>if (!confirm('Attenzione! Verranno cancellate tutte le esperienze del settore scelto.\r\n Si vuole procedere?')){false;}{</script>")
            cmdInserisci.Visible = False
            cmdInserisci.Text = "Inserisci"
            If Session("EsperienzeAreeSettore") IsNot Nothing Then
                EsperienzeAreeSettore = CType(Session("EsperienzeAreeSettore"), List(Of clsEsperienzeAree))

                Dim EsperienzaSettore = EsperienzeAreeSettore.Find((Function(esperienza As clsEsperienzeAree) esperienza.IdSettore = IDSettore))

                'Se è stata modificata un esperienza la rimuovo per poi ricaricarla aggiornata
                If EsperienzaSettore IsNot Nothing Then
                    EsperienzeAreeSettore.Remove(EsperienzaSettore)
                    Session("EsperienzeAreeSettore") = EsperienzeAreeSettore
                End If

            End If

            divAree.InnerHtml = ""
            divAree.Style("display") = "none"
            divAreaIntervento.Style("display") = "none"
            txtEsperienza1.Text = String.Empty
            txtEsperienza2.Text = String.Empty
            txtEsperienza3.Text = String.Empty
            lblAnno3.Text = "Non impostato"
            lblAnno2.Text = "Non impostato"
            lblAnno1.Text = "Non impostato"
        End If
    End Sub

    Private Sub cmdModifica_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim gr As GridViewRow
        Dim bottone = CType(sender, Button)
        Dim url As String
        Dim istruzione As String
        Dim IDSettore = CStr(dtgSettori.DataKeys(gr.RowIndex).Values(0))
        gr = bottone.Parent.Parent
    End Sub
    Protected Sub dtgSettori_RowCommand(sender As Object, e As GridViewCommandEventArgs)
        Dim url As String
        Dim istruzione As String
        Dim index As Integer


        Dim row As GridViewRow = DirectCast(DirectCast(e.CommandSource, Control).NamingContainer, GridViewRow)

        index = row.RowIndex
        Dim IDSettore = CStr(dtgSettori.DataKeys(index).Values(0))

        If e.CommandName = "ApriModifica" Then
            hfRigaSelezionata.Value = index
            'url = "WfrmDettaglioSettore.aspx?Settore=" & IDSettore
            'istruzione = "window.open('" & url + "', 'popup_window', 'width=1024,height=500,left=100,top=100,scrollbars=yes,resizable=yes');"
            'Page.ClientScript.RegisterStartupScript(Me.GetType(), "script", istruzione, True)
            mpe_EsperienzaAree.Show()
            CaricaAreeIntervento(IDSettore)
            CaricaAnniEsperienza()
            lblMsgAreeInervento.Text = String.Empty
        End If

        If e.CommandName = "MostraVariazioni1" Then
            MostraVariazioni(IDSettore, row.Cells(2).FindControl("txtDesEsperienza1"), "1", DirectCast(row.Cells(2).FindControl("lblAnnoEsperienza1"), Label).Text, DirectCast(row.Cells(2).FindControl("lblAnnoEsperienza1").NamingContainer.FindControl("lblSettoriIntervento"), Label).Text)
        End If
        If e.CommandName = "MostraVariazioni2" Then
            MostraVariazioni(IDSettore, row.Cells(2).FindControl("txtDesEsperienza2"), "2", DirectCast(row.Cells(2).FindControl("lblAnnoEsperienza2"), Label).Text, DirectCast(row.Cells(2).FindControl("lblAnnoEsperienza2").NamingContainer.FindControl("lblSettoriIntervento"), Label).Text)
        End If
        If e.CommandName = "MostraVariazioni3" Then
            MostraVariazioni(IDSettore, row.Cells(2).FindControl("txtDesEsperienza3"), "3", DirectCast(row.Cells(2).FindControl("lblAnnoEsperienza3"), Label).Text, DirectCast(row.Cells(2).FindControl("lblAnnoEsperienza3").NamingContainer.FindControl("lblSettoriIntervento"), Label).Text)
        End If

        If e.CommandName = "MostraCronologia1" Then
            MostraCronologia(IDSettore, "1", DirectCast(row.Cells(2).FindControl("lblAnnoEsperienza1"), Label).Text, DirectCast(row.Cells(2).FindControl("lblAnnoEsperienza1").NamingContainer.FindControl("lblSettoriIntervento"), Label).Text)
        End If
        If e.CommandName = "MostraCronologia2" Then
            MostraCronologia(IDSettore, "2", DirectCast(row.Cells(2).FindControl("lblAnnoEsperienza2"), Label).Text, DirectCast(row.Cells(2).FindControl("lblAnnoEsperienza2").NamingContainer.FindControl("lblSettoriIntervento"), Label).Text)
        End If
        If e.CommandName = "MostraCronologia3" Then
            MostraCronologia(IDSettore, "3", DirectCast(row.Cells(2).FindControl("lblAnnoEsperienza3"), Label).Text, DirectCast(row.Cells(2).FindControl("lblAnnoEsperienza3").NamingContainer.FindControl("lblSettoriIntervento"), Label).Text)
        End If

        If e.CommandName = "AbModArt2Art10" Then
            AbilitaModArt2Art10(IDSettore, True)
            AttivaPulsantiModArt2Art10()
        End If
        If e.CommandName = "DisabModArt2Art10" Then
            AbilitaModArt2Art10(IDSettore, False)
            AttivaPulsantiModArt2Art10()
        End If
    End Sub

    Sub AbilitaModArt2Art10(IdMacroAmbitoAttività As Integer, abilita As Boolean)
        Dim SqlCmd As New SqlClient.SqlCommand()
        Try
            SqlCmd.Connection = Session("Conn")
            SqlCmd.CommandText = "SP_ACCREDITAMENTO_ABILITA_MOD_ENTE_ART2_ART10"
            SqlCmd.CommandType = CommandType.StoredProcedure
            SqlCmd.Parameters.Clear()
            SqlCmd.Parameters.Add("@IdEnte", SqlDbType.Int).Value = Session("IdEnte")
            SqlCmd.Parameters.Add("@IdMacroAmbitoAttività", SqlDbType.Int).Value = IdMacroAmbitoAttività
            SqlCmd.Parameters.Add("@abilita", SqlDbType.Bit).Value = abilita
            SqlCmd.Parameters.Add("@Utente", SqlDbType.NVarChar, 10).Value = Session("Utente")
            SqlCmd.ExecuteNonQuery()
        Catch ex As Exception
            MessaggiAlert("Errore nell'aggiornamento")
            Exit Sub
        End Try
    End Sub

    Sub MostraCronologia(idSettore As String, indiceEsperienza As String, Anno As String, DescrSettore As String)
        'Carica da db i dati cronologici reali dell'esperienza e li mette in sessione
        'poi apre la maschera della cronologia
        Dim _dt As New DataTable
        Dim _strSQL As String = ""



        _strSQL = "Select IdEnteFase [Fase],convert(char(10), DataRiferimento, 103) as [Data Presentazione], TipoEvento as [Tipo Evento],UserName [UserName],"

        Select Case indiceEsperienza
            Case 1
                _strSQL += "III_Esperienza as Esperienza"
            Case 2
                _strSQL += "II_Esperienza as Esperienza"
            Case Else
                _strSQL += "I_Esperienza as Esperienza"
        End Select

        _strSQL += ", IdStorico as Id from Accreditamento_VariazioneEnteEsperienzaSettore_CONSISTENTI where IdEnte=" + Session("IdEnte") + " and idsettore=" + idSettore
        _strSQL += " ORDER BY IdStorico Desc"

        _dt = ClsServer.DataTableGenerico(_strSQL, Session("Conn"))

        If _dt Is Nothing OrElse _dt.Rows.Count = 0 Then    'se non c'è niente esco, poi si farà + preciso e renderà invisibile il pulsante
            Exit Sub
        End If

        Session("TitoloXCronologia") = "Cronologia variazioni esperienza anno " & Anno & " settore " & DescrSettore
        Session("Cronologia") = _dt

        'stampo a pagina uno script che mi apre la popup della cronologia
        Response.Write("<script>" & vbCrLf)
        Response.Write("window.open(""Cronologia.aspx"", """", ""height=600,width=1250,toolbar=no,location=no,menubar=no,scrollbars=yes,resizable=yes"")" & vbCrLf)
        Response.Write("</script>")

    End Sub

    Sub MostraVariazioni(idSettore As String, txtEsperienza As TextBox, indiceEsperienza As String, Anno As String, DescrSettore As String)
        'Carica da db il dato reale dell'esperienza e lo mette in sessione insieme al dato in variazione (quello in maschera)
        'poi apre la maschera del compare
        Dim _dt As DataTable = Session("ElencoSettoriPrecedenti")

        Dim TestoPrecedente = ""
        Dim TestoAttuale = txtEsperienza.Text
        Dim _nomeCampo As String

        Select Case indiceEsperienza
            Case 1
                _nomeCampo = "III_Esperienza"
            Case 2
                _nomeCampo = "II_Esperienza"
            Case Else
                _nomeCampo = "I_Esperienza"
        End Select

        'Select Case indiceEsperienza
        '    Case 1
        '        _strSQL = "Select III_Esperienza as TestoPrecedente"
        '    Case 2
        '        _strSQL = "Select II_Esperienza as TestoPrecedente"
        '    Case Else
        '        _strSQL = "Select I_Esperienza as TestoPrecedente"
        'End Select

        '_strSQL += " FROM EnteEsperienzaSettore where idente=" + Session("IdEnte") + " and idsettore=" + idSettore + " and DataFineValidita is null"

        '_dt = ClsServer.DataTableGenerico(_strSQL, Session("Conn"))


        If Not _dt Is Nothing Then
            Dim _row As DataRow() = _dt.Select("IDSETTORE=" + idSettore)
            If _row.Length > 0 Then
                If Not IsDBNull(_row(0)(_nomeCampo)) Then
                    TestoPrecedente = _row(0)(_nomeCampo)
                End If
            End If
        End If

        Session("TestoPrecedenteXCompare") = TestoPrecedente
        Session("TestoAttualeXCompare") = TestoAttuale
        Session("TitoloXCompare") = "Variazione esperienza anno " & Anno & " settore " & DescrSettore

        'stampo a pagina uno script che mi apre la popup delle variazioni
        Response.Write("<script>" & vbCrLf)
        Response.Write("window.open(""compare.aspx"", """", ""height=500,width=900,toolbar=no,location=no,menubar=no,scrollbars=yes,resizable=yes"")" & vbCrLf)
        Response.Write("</script>")

    End Sub

    Private Sub CaricaAreeIntervento(IDSettore As String)
        Dim strSQl = ""
        Dim dtAmbiti As DataTable
        Dim EsperienzeAreeSettore As New List(Of clsEsperienzeAree)

        strSQl = ""
        strSQl += "SELECT A.CODIFICA, (M.CODIFICA + A.CODIFICA  + ' - ' + A.AreaIntervento) AS AREAINTERVENTO,"
        strSQl += "A.IDSETTORE AS IDSETTORE, M.MACROAMBITOATTIVITÀ AS SETTORE, A.id AS IDAREA "
        strSQl += "FROM AREEESPERIENZA A "
        strSQl += "INNER JOIN MACROAMBITIATTIVITÀ M "
        strSQl += "ON A.IdSettore = M.IDMacroAmbitoAttività "
        strSQl += "WHERE A.IDSETTORE= " & IDSettore

        dtAmbiti = ClsServer.DataTableGenerico(strSQl, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
        grdAmbiti.DataSource = dtAmbiti
        grdAmbiti.DataBind()

        If IDSettore <> "6" Then 'Estero 
            lblTitoloEsperienzaArea.Text = "Dettaglio Settore - " & dtAmbiti.Rows(0)("SETTORE").ToString()
        Else
            lblTitoloEsperienzaArea.Text = "Dettaglio Settore - Estero "
        End If


        'Recupero l'esperienze per quel settore specifico 
        If Session("EsperienzeAreeSettore") IsNot Nothing Then
            EsperienzeAreeSettore = CType(Session("EsperienzeAreeSettore"), List(Of clsEsperienzeAree))
            Dim EsperienzaSettore = EsperienzeAreeSettore.Find((Function(esperienza As clsEsperienzeAree) esperienza.IdSettore = IDSettore))
            If EsperienzaSettore IsNot Nothing Then
                Dim Conta = 1
                For Each anno As Integer In EsperienzaSettore.AnnoEsperienza
                    Select Case Conta
                        Case 1
                            txtAnno3.Text = anno
                        Case 2
                            txtAnno2.Text = anno
                        Case 3
                            txtAnno1.Text = anno
                    End Select
                    Conta = Conta + 1
                Next
                Conta = 1
                For Each esperienza As String In EsperienzaSettore.DescrizioneEsperienza
                    Select Case Conta
                        Case 1
                            txtEsperienza3.Text = esperienza
                        Case 2
                            txtEsperienza2.Text = esperienza
                        Case 3
                            txtEsperienza1.Text = esperienza
                    End Select
                    Conta = Conta + 1
                Next

                For Each gr As GridViewRow In grdAmbiti.Rows
                    If gr.RowType = DataControlRowType.DataRow Then
                        For Each areaSelezionata As String In EsperienzaSettore.AreeSelezionate
                            Dim HFIDAREA = DirectCast(grdAmbiti.Rows(gr.RowIndex).Cells(4).FindControl("HFIDAREA"), HiddenField)
                            If (areaSelezionata = HFIDAREA.Value) Then
                                Dim chk As CheckBox = grdAmbiti.Rows(gr.RowIndex).Cells(2).FindControl("chkSeleziona")
                                chk.Checked = True
                            End If
                        Next
                    End If
                Next
            Else
                txtEsperienza3.Text = String.Empty
                txtEsperienza2.Text = String.Empty
                txtEsperienza1.Text = String.Empty
                txtAnno1.Text = String.Empty
                txtAnno2.Text = String.Empty
                txtAnno3.Text = String.Empty
            End If
        End If

    End Sub
    Private Function CaricaAnniEsperienza()

        For i = 1 To ANNIPRECEDENTI
            Select Case i
                Case 1
                    If Not IsNumeric(txtAnno3.Text) Then txtAnno3.Text = CStr(Year(Now.Date) - i)
                    txtAnno3.Enabled = False
                    AnniCaricamento(i - 1) = txtAnno3.Text
                Case 2
                    If Not IsNumeric(txtAnno2.Text) Then txtAnno2.Text = CStr(Year(Now.Date) - i)
                    txtAnno2.Enabled = False
                    AnniCaricamento(i - 1) = txtAnno2.Text
                Case 3
                    If Not IsNumeric(txtAnno1.Text) Then txtAnno1.Text = CStr(Year(Now.Date) - i)
                    txtAnno1.Enabled = False
                    AnniCaricamento(i - 1) = txtAnno1.Text
            End Select
        Next

    End Function
    Protected Sub CmdConferma_Click(sender As Object, e As EventArgs) Handles CmdConferma.Click
        Dim dtTipologiaEnte As DataTable
        Dim tipologiaEnte As String
        Dim naturaGiuridicaEnte As String
        Dim idCategoriaEntePubblico As Byte

        lblMsgAreeInervento.Text = String.Empty

        If ddlTipologia.SelectedItem IsNot Nothing Then
            tipologiaEnte = ddlTipologia.SelectedItem.Text
        End If

        If ddlGiuridica.SelectedItem IsNot Nothing Then
            naturaGiuridicaEnte = ddlGiuridica.SelectedItem.Text
        End If

        If tipologiaEnte = String.Empty Then
            lblMsgAreeInervento.Text = "Specificare la tipologia dell'ente prima di compilare il dettaglio di un settore"
            mpe_EsperienzaAree.Show()
            Exit Sub
        End If

        If naturaGiuridicaEnte = String.Empty Then
            lblMsgAreeInervento.Text = "Selezionare la denominazione della tipologia"
            mpe_EsperienzaAree.Show()
            Exit Sub
        End If


        ''Se selezionate 
        'If tipologiaEnte = "Pubblico" Then
        '    strsql = "SELECT IdCategoriaEntePubblico FROM TipologieEnti "
        '    strsql &= "WHERE IdTipologieEnti=" & ddlGiuridica.SelectedValue
        '    dtTipologiaEnte = New DataTable
        '    dtTipologiaEnte = ClsServer.DataTableGenerico(strsql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
        '    If dtTipologiaEnte IsNot Nothing And dtTipologiaEnte.Rows.Count > 0 Then
        '        idCategoriaEntePubblico = CInt(dtTipologiaEnte.Rows(0)("IdCategoriaEntePubblico"))
        '    End If
        'End If

        'If tipologiaEnte = "Pubblico" Then
        '    If idCategoriaEntePubblico = 0 _
        '    Or idCategoriaEntePubblico = 1 Then
        '        AggiungiEsperienzaAmbiti()
        '    Else
        '        If (VerificaAree()) Then
        '            AggiungiEsperienzaAmbiti()
        '        Else
        '            mpe_EsperienzaAree.Show()
        '        End If
        '    End If
        'Else
        '    AggiungiEsperienzaAmbiti()
        'End If

        If VerificaAree() Then
            AggiungiEsperienzaAmbiti()
        Else
            mpe_EsperienzaAree.Show()
        End If


    End Sub
    Private Function VerificaAree() As Boolean
        Dim Esperienza(ANNIPRECEDENTI - 1) As String
        Dim contaAmbitiSelezionati As Integer = 0


        For i = 0 To ANNIPRECEDENTI - 1
            Select Case i
                Case 0
                    Esperienza(0) = txtEsperienza3.Text.Trim
                Case 1
                    Esperienza(1) = txtEsperienza2.Text.Trim
                Case 2
                    Esperienza(2) = txtEsperienza1.Text.Trim
            End Select
        Next

        CaricaAnniEsperienza()

        For i = 0 To Esperienza.Length - 1
            If Esperienza(i) = String.Empty Then
                lblMsgAreeInervento.Text = "Esperienza anno " & AnniCaricamento(i) & " mancante"
                VerificaAree = False
                Exit Function
            Else
                If Esperienza(i).Length < 500 Or Esperienza(i).Length > 1500 Then
                    lblMsgAreeInervento.Text = "L' esperienza per l'anno " & AnniCaricamento(i) & " deve essere composta da un minimo di 500 ad un massimo di 1500 caratteri. Attualmente ne sono stati inseriti " & Esperienza(i).Length
                    VerificaAree = False
                    Exit Function
                End If
            End If
        Next


        For Each gr As GridViewRow In grdAmbiti.Rows
            If gr.RowType = DataControlRowType.DataRow Then
                Dim chk As CheckBox = gr.Cells(2).FindControl("chkSeleziona")
                If chk IsNot Nothing Then
                    If chk.Checked Then
                        contaAmbitiSelezionati = contaAmbitiSelezionati + 1
                    End If
                End If
            End If
        Next

        If contaAmbitiSelezionati = 0 Then
            lblMsgAreeInervento.Text = "È obbligatorio selezionare almeno un'area d'intervento "
            VerificaAree = False
            Exit Function
        End If

        VerificaAree = True

    End Function
    Private Sub AggiungiEsperienzaAmbiti()

        'Dim esperienzaAmbiti As New clsAmbitiEsperienza()
        Dim istruzione As String
        Dim rigaSelezionata As GridViewRow
        Dim rigsSelezionataAmbiti As GridViewRow
        Dim ContaRiga As Integer = 1
        Dim EsperienzeAreeSettore As New List(Of clsEsperienzeAree)
        Dim IDSettore As Integer
        Dim AreeSelezionate As New List(Of String)

        'rigaSelezionata.indiceRigaSelezionata = hfRigaSelezionata.Value

        If Session("EsperienzeAreeSettore") IsNot Nothing Then
            EsperienzeAreeSettore = CType(Session("EsperienzeAreeSettore"), List(Of clsEsperienzeAree))
        Else
            EsperienzeAreeSettore.Clear()
        End If

        If Not hfRigaSelezionata.Value = String.Empty Then
            rigaSelezionata = dtgSettori.Rows(hfRigaSelezionata.Value)
        End If

        If Not hfRigaAmbiti.Value = String.Empty Then
            rigsSelezionataAmbiti = grdAmbiti.Rows(hfRigaAmbiti.Value)
        End If

        Dim EsperienzaArea As New clsEsperienzeAree()

        Dim lblAnnoEsperienza3 As Label = rigaSelezionata.Cells(2).FindControl("lblAnnoEsperienza3")
        Dim lblAnnoEsperienza2 As Label = rigaSelezionata.Cells(2).FindControl("lblAnnoEsperienza2")
        Dim lblAnnoEsperienza1 As Label = rigaSelezionata.Cells(2).FindControl("lblAnnoEsperienza1")

        Dim txtDesEsperienza3 As TextBox = rigaSelezionata.Cells(2).FindControl("txtDesEsperienza3")
        Dim txtDesEsperienza2 As TextBox = rigaSelezionata.Cells(2).FindControl("txtDesEsperienza2")
        Dim txtDesEsperienza1 As TextBox = rigaSelezionata.Cells(2).FindControl("txtDesEsperienza1")
        Dim Area As HtmlGenericControl = rigaSelezionata.Cells(2).FindControl("divArea")

        lblAnnoEsperienza3.Text = Trim(txtAnno3.Text)
        lblAnnoEsperienza2.Text = Trim(txtAnno2.Text)
        lblAnnoEsperienza1.Text = Trim(txtAnno1.Text)
        txtDesEsperienza3.Text = Trim(txtEsperienza3.Text)
        txtDesEsperienza2.Text = Trim(txtEsperienza2.Text)
        txtDesEsperienza1.Text = Trim(txtEsperienza1.Text)


        Area.InnerHtml = "<fieldset><legend>Aree d'intervento selezionate</legend>"
        Area.InnerHtml += "<table>"
        For Each gr As GridViewRow In grdAmbiti.Rows
            If gr.RowType = DataControlRowType.DataRow Then
                Dim chk As CheckBox = gr.Cells(2).FindControl("chkSeleziona")
                If chk IsNot Nothing Then
                    If chk.Checked Then
                        Dim HFIDAREA = DirectCast(grdAmbiti.Rows(gr.RowIndex).Cells(4).FindControl("HFIDAREA"), HiddenField)
                        Dim HFIDSettore = DirectCast(grdAmbiti.Rows(gr.RowIndex).Cells(3).FindControl("HFIDSettore"), HiddenField)
                        Area.InnerHtml += "<tr><td><label runat=""server"" ID =""lblValoreAreaIntervento_" & ContaRiga & """>" & grdAmbiti.Rows(gr.RowIndex).Cells(1).Text & "<label></td></tr>"
                        AreeSelezionate.Add(HFIDAREA.Value)
                        IDSettore = CInt(HFIDSettore.Value)
                        ContaRiga = ContaRiga + 1
                    End If
                End If
            End If
        Next

        Area.InnerHtml += "</table></fieldset>"

        'Carico l'entità EsperienzaArea inizio


        Dim EsperienzaSettore = EsperienzeAreeSettore.Find((Function(esperienza As clsEsperienzeAree) esperienza.IdSettore = IDSettore))

        'Se è stata modificata un esperienza la rimuovo per poi ricaricarla aggiornata
        If EsperienzaSettore IsNot Nothing Then
            EsperienzeAreeSettore.Remove(EsperienzaSettore)
        End If

        Dim AnniEsperienza As New List(Of Integer)
        Dim DescrizioneEsperienza As New List(Of String)

        EsperienzaArea.IdSettore = IDSettore
        AnniEsperienza.Insert(0, CInt(lblAnnoEsperienza3.Text))
        AnniEsperienza.Insert(1, CInt(lblAnnoEsperienza2.Text))
        AnniEsperienza.Insert(2, CInt(lblAnnoEsperienza1.Text))
        EsperienzaArea.AnnoEsperienza = AnniEsperienza

        DescrizioneEsperienza.Insert(0, txtDesEsperienza3.Text)
        DescrizioneEsperienza.Insert(1, txtDesEsperienza2.Text)
        DescrizioneEsperienza.Insert(2, txtDesEsperienza1.Text)

        EsperienzaArea.DescrizioneEsperienza = DescrizioneEsperienza
        EsperienzaArea.AreeSelezionate = AreeSelezionate
        EsperienzaArea.AreeCompetenza = Area.InnerHtml

        EsperienzeAreeSettore.Add(EsperienzaArea)

        'Carico l'entità EsperienzaArea fine

        'Metto tutto in sessione
        Session("EsperienzeAreeSettore") = EsperienzeAreeSettore

        Dim cmdInserisci As Button = DirectCast(rigaSelezionata.FindControl("cmdModifica"), Button)
        If Not cmdInserisci Is Nothing Then
            cmdInserisci.Text = "Modifica"
        End If
        mpe_EsperienzaAree.Hide()


        'esperienzaAmbiti.IdSettore = IDSettore
        'esperienzaAmbiti.DictEsperienze = New Dictionary(Of String, String)
        'esperienzaAmbiti.LstAmbitiScelti = New Dictionary(Of String, String)

        'For Each gr As GridViewRow In grdAmbiti.Rows
        '    If gr.RowType = DataControlRowType.DataRow Then
        '        Dim chk As CheckBox = gr.Cells(2).FindControl("chkSeleziona")
        '        If chk IsNot Nothing Then
        '            If chk.Checked Then
        '                esperienzaAmbiti.LstAmbitiScelti.Add(grdAmbiti.DataKeys(gr.RowIndex).Value, grdAmbiti.Rows(gr.RowIndex).Cells(1).Text) 'AMBITO ATTIVITA
        '            End If
        '        End If
        '    End If
        'Next

        'esperienzaAmbiti.DictEsperienze.Add(Trim(txtAnno1.Text), Trim(txtEsperienza1.Text))
        'esperienzaAmbiti.DictEsperienze.Add(Trim(txtAnno2.Text), Trim(txtEsperienza2.Text))
        'esperienzaAmbiti.DictEsperienze.Add(Trim(txtAnno3.Text), Trim(txtEsperienza3.Text))




        'Session.Add("EsperienzaAree", esperienzaAmbiti)
        'istruzione = "window.opener.location.reload(true);window.close();"

        'Page.ClientScript.RegisterStartupScript(Me.GetType(), "script", istruzione, True)

    End Sub

    Protected Sub ddlGiuridica_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlGiuridica.SelectedIndexChanged

        If ddlGiuridica.SelectedItem IsNot Nothing And ddlGiuridica.SelectedItem.Text Like "Altro*" Or
            ddlGiuridica.SelectedItem IsNot Nothing And ddlGiuridica.SelectedItem.Text = "Altro ente" Then
            divAltraTipologia.Visible = True
        Else
            divAltraTipologia.Visible = False
            txtAltraTipoEnte.Text = String.Empty
        End If

    End Sub
    Private Sub cmdAllegaAttoNomina_Click(sender As Object, e As EventArgs) Handles cmdAllegaAttoNomina.Click
        lblMessaggio.Text = String.Empty
        'Verifica se è stato inserito il file
        If fileAN.PostedFile Is Nothing Or String.IsNullOrEmpty(fileAN.PostedFile.FileName) Then
            lblErroreAttoNomina.Text = "Non è stato scelto nessun file per il caricamento dell' atto di nomina del Rappresentante Legale"
            mpeAttoNomina.Show()
            Exit Sub
        End If
        'Controllo Tipo File
        If clsGestioneDocumentiAccreditamento.VerificaEstensioneFileAccreditamento(fileAN) = False Then
            lblErroreAttoNomina.Text = "Il formato file dell' atto di nomina del Rappresentante Legale non è corretto. È possibile associare documenti nel formato .PDF o .PDF.P7M"
            mpeAttoNomina.Show()
            Exit Sub
        End If
        'Controlli dimensioni del file
        Dim fs = fileAN.PostedFile.InputStream
        Dim iLen As Integer = CInt(fs.Length - 1)
        Dim bBLOBStorage(iLen) As Byte

        If iLen <= 0 Then
            lblErroreAttoNomina.Text = "Attenzione. Impossibile caricare un atto di nomina del Rappresentante Legale  vuoto."
            mpeAttoNomina.Show()
            Exit Sub
        End If
        If iLen > 20971520 Then
            lblErroreAttoNomina.Text = "Attenzione. La dimensione massima dell atto di nomina del Rappresentante Legale è di 20 MB."
            mpeAttoNomina.Show()
            Exit Sub
        End If

        bBLOBStorage = ClsServer.StreamToByte(fs)

        fs.Close()

        Dim hashValue As String
        hashValue = GeneraHash(bBLOBStorage)

        Dim estensione As String = System.IO.Path.GetExtension(fileAN.PostedFile.FileName)
        If UCase(estensione) = ".P7M" Then estensione = ".pdf.p7m"

        'Salvo File In Sessione
        Dim AN As New Allegato() With {
         .Updated = True,
         .Blob = bBLOBStorage,
         .Filename = "NOMINARL_" & txtCodFis.Text & estensione,
         .Hash = hashValue,
         .Filesize = iLen,
         .DataInserimento = Date.Now,
         .IdTipoAllegato = TipoFile.DOCUMENTO_NOMINA
        }

        'If EsistenzaAllegatoEnteDB(AN.Hash) = False Then
        '    lblErroreImpegnoEtico.Text = "È stato inserito un allegato " & AN.Filename & " che è già presente in archivio per questo Ente"
        '    AN = Nothing
        '    mpeImpegnoEtico.Show()
        '    Exit Sub
        'End If

        Session("LoadedAN") = AN
        'Se Il CV è caricato in Sessione (Inserimento)
        rowNoAN.Visible = False
        rowAN.Visible = True
        txtANFilename.Text = AN.Filename
        txtANHash.Text = AN.Hash
        txtANData.Text = AN.DataInserimento.ToString("dd/MM/yyyy HH:mm:ss")
        MessaggiConvalida("Atto di Nomina caricato correttamente")
    End Sub

    Private Function GeneraHash(ByVal FileinByte() As Byte) As String
        Dim tmpHash() As Byte

        tmpHash = New MD5CryptoServiceProvider().ComputeHash(FileinByte)

        GeneraHash = ByteArrayToString(tmpHash)
        Return GeneraHash
    End Function

    Private Sub MessaggiSuccess(ByVal strMessaggio)
        lblMessaggio.CssClass = "MsgInfo"
        lblMessaggio.Text = strMessaggio
        lblMessaggio.Focus()
    End Sub

    'Protected Sub cmdAllegaDelibera_Click(sender As Object, e As EventArgs) Handles cmdAllegaDelibera.Click

    'End Sub

    Private Function ByteArrayToString(ByVal arrInput() As Byte) As String
        Dim i As Integer
        Dim sOutput As New StringBuilder(arrInput.Length)
        For i = 0 To arrInput.Length - 1
            sOutput.Append(arrInput(i).ToString("X2"))
        Next
        Return sOutput.ToString()
    End Function

    Private Sub ClearSessionAN()
        lblMessaggio.Text = String.Empty
        Session("LoadedAN") = Nothing
        rowNoAN.Visible = True
        rowAN.Visible = False
        MessaggiConvalida("Atto di nomina del Rappresentante Legale eliminato correttamente")
    End Sub

    Private Sub ClearSessionDelibera()
        lblMessaggio.Text = String.Empty
        Session("LoadedDelibera") = Nothing
        rowNoDelibera.Visible = True
        rowDelibera.Visible = False
        MessaggiConvalida("Delibera degli organi decisionali sulla definizione nominativa della struttura di gestione eliminata correttamente")
    End Sub

    Private Sub ClearSessionRTDP()
        lblMessaggio.Text = String.Empty
        Session("LoadedRTDP") = Nothing
        'rowNoRTDP.Visible = True
        'rowRTDP.Visible = False
        MessaggiConvalida("Atto di designazione del Responsabile del trattamento dei dati personali eliminato correttamente")
    End Sub

    Private Sub ClearSessionAttoCostitutivo()
        lblMessaggio.Text = String.Empty
        Session("LoadedAC") = Nothing
        rowNoAttoCostitutivo.Visible = True
        rowAttoCostitutivo.Visible = False
        MessaggiConvalida("Atto Costitutivo dell'Ente eliminato correttamente")
    End Sub

    Private Sub ClearSessionStatuto()
        lblMessaggio.Text = String.Empty
        Session("LoadedStatuto") = Nothing
        rowNoStatuto.Visible = True
        rowStatuto.Visible = False
        MessaggiConvalida("Statuto dell'Ente eliminato correttamente")
    End Sub

    Private Sub ClearDeliberaAdesione()
        lblMessaggio.Text = String.Empty
        Session("LoadedDeliberaAdesione") = Nothing
        rowNoDeliberaAdesione.Visible = True
        rowDeliberaAdesione.Visible = False
        MessaggiConvalida("Delibera eliminata correttamente")
    End Sub

    'Private Sub CaricaAttoNomina()
    '    '***Gestione Atto di nomina rappresentante legale****
    '    If Session("LoadANId") IsNot Nothing Then
    '        'Se l' Atto di nomina è caricato nel DB
    '        'Recupero File da DB
    '        Dim sqlGetAllegato = "SELECT Top 1 FileName,HashValue,len(BinData) FileLength,BinData,DataInserimento, IdEnteDocumento, IdEnteFase, Stato From entidocumenti WHERE idEnteDocumento = @idAllegato"
    '        Dim getAllegatoCommand As New SqlCommand(sqlGetAllegato, Session("conn"))
    '        getAllegatoCommand.Parameters.AddWithValue("@idAllegato", Session("LoadANId"))
    '        Dim dtrAllegato As SqlDataReader = getAllegatoCommand.ExecuteReader()
    '        If dtrAllegato.Read Then
    '            Dim filename As String = dtrAllegato("FileName")
    '            Dim hashValue As String = dtrAllegato("HashValue")
    '            Dim filelength As Integer = CInt(dtrAllegato("FileLength"))
    '            Dim blob As Byte() = dtrAllegato("BinData")
    '            Dim dataInserimento As Date = dtrAllegato("DataInserimento")
    '            dtrAllegato.Close()
    '            getAllegatoCommand.Dispose()
    '            rowNoAN.Visible = False
    '            rowAN.Visible = True
    '            txtANFilename.Text = filename
    '            txtANData.Text = dataInserimento.ToString("dd/MM/yyyy HH:mm:ss")
    '            Session("LoadedAN") = New Allegato() With {
    '             .Blob = blob,
    '             .Filename = "NOMINARL_" & txtCodFis.Text & System.IO.Path.GetExtension(filename),
    '             .Hash = hashValue,
    '             .Filesize = filelength,
    '             .DataInserimento = dataInserimento
    '            }

    '        Else
    '            dtrAllegato.Close()
    '            ClearSessionAN()
    '        End If
    '    End If
    '    Dim AN As Allegato = Session("LoadedAN")

    '    If AN IsNot Nothing Then
    '        'Se Il CV è caricato in Sessione (Inserimento)
    '        rowNoAN.Visible = False
    '        rowAN.Visible = True
    '        txtANFilename.Text = AN.Filename
    '        txtANHash.Text = AN.Hash
    '        txtANData.Text = AN.DataInserimento.ToString("dd/MM/yyyy HH:mm:ss")
    '    Else
    '        'Se Il CV non è ancora caricato
    '        rowNoAN.Visible = True
    '        rowAN.Visible = False
    '    End If
    'End Sub

    Private Sub ClearSessionImpegnoEtico()
        lblMessaggio.Text = String.Empty
        Session("LoadedCartaImpegnoEtico") = Nothing
        rowNoImpegnoEtico.Visible = True
        rowImpegnoEtico.Visible = False
        MessaggiConvalida("Carta Impegno Etico eliminata correttamente")
    End Sub



    Protected Sub btnEliminaAN_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles btnEliminaAN.Click
        ClearSessionAN()
    End Sub

    Protected Sub btnDownloadAN_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles btnDownloadAN.Click
        If Session("LoadedAN") Is Nothing Then
            MessaggiAlert("Nessun File caricato")
            ClearSessionAN()
            Exit Sub
        End If
        Dim AN As Allegato = Session("LoadedAN")
        Response.Clear()
        Response.ContentType = "Application/pdf"
        Response.AddHeader("Content-Disposition", "attachment; filename=" & AN.Filename)
        Response.BinaryWrite(AN.Blob)
        Response.End()
    End Sub

    Protected Sub CmdAttoNomina_Click(sender As Object, e As EventArgs) Handles CmdAttoNomina.Click
        mpeAttoNomina.Show()
    End Sub

    Protected Sub btnModificaAN_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles btnModificaAN.Click
        mpeAttoNomina.Show()
    End Sub

    Protected Sub cmdCaricaDelibera_Click(sender As Object, e As EventArgs) Handles cmdCaricaDelibera.Click
        mpeDelibera.Show()
    End Sub

    Protected Sub btnDownloadDelibera_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles btnDownloadDelibera.Click
        If Session("LoadedDelibera") Is Nothing Then
            MessaggiAlert("Nessun File caricato")
            ClearSessionDelibera()
            Exit Sub
        End If
        Dim Delibera As Allegato = Session("LoadedDelibera")
        Response.Clear()
        Response.ContentType = "Application/pdf"
        Response.AddHeader("Content-Disposition", "attachment; filename=" & Delibera.Filename)
        Response.BinaryWrite(Delibera.Blob)
        Response.End()
    End Sub

    Protected Sub btnModificaDelibera_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles btnModificaDelibera.Click
        mpeDelibera.Show()
    End Sub

    Protected Sub btnEliminaDelibera_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles btnEliminaDelibera.Click
        ClearSessionDelibera()
    End Sub

    Private Sub CaricaDeliberaStrutturaGestione()
        '***Gestione CV****
        If Session("LoadedDeliberaId") IsNot Nothing Then
            CaricaAllegatoFromDB(Session("LoadedDeliberaId"), "LoadedDeliberaId", rowNoDelibera, rowDelibera, txtDeliberaFilename, txtDeliberaHash, txtDeliberaData)
        End If
        Dim Delibera As Allegato = Session("LoadedDelibera")

        If Delibera IsNot Nothing Then
            'Se Il CV è caricato in Sessione (Inserimento)
            rowNoDelibera.Visible = False
            rowDelibera.Visible = True
            txtDeliberaFilename.Text = Delibera.Filename
            txtDeliberaHash.Text = Delibera.Hash
            txtDeliberaData.Text = Delibera.DataInserimento.ToString("dd/MM/yyyy HH:mm:ss")
        Else
            'Se Il CV non è ancora caricato
            rowNoDelibera.Visible = True
            rowDelibera.Visible = False
        End If
    End Sub

    Protected Sub cmdAllegaDelibera_Click(sender As Object, e As EventArgs) Handles cmdAllegaDelibera.Click
        lblMessaggio.Text = String.Empty
        'Verifica se è stato inserito il file
        If fileDelibera.PostedFile Is Nothing Or String.IsNullOrEmpty(fileDelibera.PostedFile.FileName) Then
            lblErroreUploadDelibera.Text = "Non è stato scelto nessun file per il caricamento della Nomina della struttra di gestione"
            mpeDelibera.Show()
            Exit Sub
        End If
        'Controllo Tipo File
        If clsGestioneDocumentiAccreditamento.VerificaEstensioneFileAccreditamento(fileDelibera) = False Then
            lblErroreUploadDelibera.Text = "Il formato file della Nomina della struttra di gestione non è corretto. È possibile associare documenti nel formato .PDF o .PDF.P7M"
            mpeDelibera.Show()
            Exit Sub
        End If
        'Controlli dimensioni del file
        Dim fs = fileDelibera.PostedFile.InputStream
        Dim iLen As Integer = CInt(fs.Length)
        Dim bBLOBStorage(iLen) As Byte

        If iLen <= 0 Then
            lblErroreUploadDelibera.Text = "Attenzione. Impossibile caricare la Nomina della struttra di gestione vuota."
            mpeDelibera.Show()
            Exit Sub
        End If
        If iLen > 20971520 Then
            lblErroreUploadDelibera.Text = "Attenzione. La dimensione massima della Nomina della struttura di gestione è di 20 MB."
            mpeDelibera.Show()
            Exit Sub
        End If

        bBLOBStorage = ClsServer.StreamToByte(fs)

        fs.Close()

        Dim hashValue As String
        hashValue = GeneraHash(bBLOBStorage)

        Dim estensione As String = System.IO.Path.GetExtension(fileDelibera.PostedFile.FileName)
        If UCase(estensione) = ".P7M" Then estensione = ".pdf.p7m"

        'Salvo File In Sessione
        Dim Delibera As New Allegato() With {
         .Updated = True,
         .Blob = bBLOBStorage,
         .Filename = "STRUTTURAGESTIONE_" & txtCodFis.Text & estensione,
         .Hash = hashValue,
         .Filesize = iLen,
         .DataInserimento = Date.Now,
        .IdTipoAllegato = TipoFile.DELIBERA
        }

        'If EsistenzaAllegatoEnteDB(Delibera.Hash) = False Then
        '    lblErroreImpegnoEtico.Text = "È stato inserito un allegato " & Delibera.Filename & " che è già presente in archivio per questo Ente"
        '    Delibera = Nothing
        '    mpeImpegnoEtico.Show()
        '    Exit Sub
        'End If

        Session("LoadedDelibera") = Delibera
        'Se Il CV è caricato in Sessione (Inserimento)
        rowNoDelibera.Visible = False
        rowDelibera.Visible = True
        txtDeliberaFilename.Text = Delibera.Filename
        txtDeliberaHash.Text = Delibera.Hash
        txtDeliberaData.Text = Delibera.DataInserimento.ToString("dd/MM/yyyy HH:mm:ss")
        MessaggiConvalida("Nomina della struttra di gestione caricata correttamente")
    End Sub

    'Protected Sub CmdAttoDesignazione_Click(sender As Object, e As EventArgs) Handles CmdAttoDesignazione.Click
    '    mpeRTDP.Show()
    'End Sub

    'Protected Sub cmdAllegaRTDP_Click(sender As Object, e As EventArgs) Handles cmdAllegaRTDP.Click
    '    lblMessaggio.Text = String.Empty
    '    'Verifica se è stato inserito il file
    '    If fileRTDP.PostedFile Is Nothing Or String.IsNullOrEmpty(fileRTDP.PostedFile.FileName) Then
    '        MessaggiAlert("Non è stato scelto nessun file per il caricamento dell'atto di designazione del responsabile del trattamento dei dati personali")
    '        Exit Sub
    '    End If
    '    'Controllo Tipo File
    '    If clsGestioneDocumentiAccreditamento.VerificaEstensioneFileAccreditamento(fileRTDP) = False Then
    '        MessaggiAlert("Il formato file dell'atto di designazione del responsabile del trattamento dei dati personali non è corretto. È possibile associare documenti nel formato .PDF o .PDF.P7M")
    '        Exit Sub
    '    End If
    '    'Controlli dimensioni del file
    '    Dim fs = fileRTDP.PostedFile.InputStream
    '    Dim iLen As Integer = CInt(fs.Length)
    '    Dim bBLOBStorage(iLen) As Byte

    '    If iLen <= 0 Then
    '        MessaggiAlert("Attenzione. Impossibile caricare un atto di designazione del responsabile del trattamento dei dati personali vuoto.")
    '        Exit Sub
    '    End If
    '    If iLen > 20971520 Then
    '        MessaggiAlert("Attenzione. La dimensione massima dell'atto di designazione del responsabile del trattamento dei dati personali è di 20 MB.")
    '        Exit Sub
    '    End If
    '    Dim numBytesToRead As Integer = CType(fs.Length, Integer)
    '    Dim numBytesRead As Integer = 0

    '    While (numBytesToRead > 0)
    '        ' Read may return anything from 0 to numBytesToRead.
    '        Dim n As Integer = fs.Read(bBLOBStorage, numBytesRead, numBytesToRead)
    '        ' Break when the end of the file is reached.
    '        If (n = 0) Then
    '            Exit While
    '        End If
    '        numBytesRead = (numBytesRead + n)
    '        numBytesToRead = (numBytesToRead - n)

    '    End While

    '    fs.Close()

    '    Dim hashValue As String
    '    hashValue = GeneraHash(bBLOBStorage)

    '    'Salvo File In Sessione
    '    Dim RTDP As New Allegato() With {
    '     .Updated = True,
    '     .Blob = bBLOBStorage,
    '     .Filename = fileRTDP.PostedFile.FileName,
    '     .Hash = hashValue,
    '     .Filesize = iLen,
    '     .DataInserimento = Date.Now
    '    }
    '    Session("LoadedRTDP") = RTDP
    '    'Se Il CV è caricato in Sessione (Inserimento)
    '    rowNoRTDP.Visible = False
    '    rowRTDP.Visible = True
    '    txtRTDPFilename.Text = RTDP.Filename
    '    txtRTDPHash.Text = RTDP.Hash
    '    txtRTDPData.Text = RTDP.DataInserimento.ToString("dd/MM/yyyy HH:mm:ss")
    '    MessaggiSuccess("Atto di designazione del responsabile del trattamento dei dati personali caricato Correttamente")
    'End Sub

    'Protected Sub btnEliminaRTDP_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles btnEliminaRTDP.Click
    '    ClearSessionRTDP()
    'End Sub

    'Private Sub CaricaRTDP()
    '    '***Gestione Atto di designazione responsabile del trattamento dei dati personali****
    '    If Session("LoadRTDPId") IsNot Nothing Then
    '        'Se l' Atto di nomina è caricato nel DB
    '        'Recupero File da DB
    '        Dim sqlGetAllegato = "SELECT Top 1 FileName,HashValue,FileLength,BinData,DataInserimento From Allegato WHERE idAllegato = @idAllegato"
    '        Dim getAllegatoCommand As New SqlCommand(sqlGetAllegato, Session("conn"))
    '        getAllegatoCommand.Parameters.AddWithValue("@idAllegato", Session("LoadRTDPId"))
    '        Dim dtrAllegato As SqlDataReader = getAllegatoCommand.ExecuteReader()
    '        If dtrAllegato.Read Then
    '            Dim filename As String = dtrAllegato("FileName")
    '            Dim hashValue As String = dtrAllegato("HashValue")
    '            Dim filelength As Integer = CInt(dtrAllegato("FileLength"))
    '            Dim blob As Byte() = dtrAllegato("BinData")
    '            Dim dataInserimento As Date = dtrAllegato("DataInserimento")
    '            dtrAllegato.Close()
    '            getAllegatoCommand.Dispose()
    '            rowNoRTDP.Visible = False
    '            rowRTDP.Visible = True
    '            txtRTDPFilename.Text = filename
    '            txtRTDPData.Text = dataInserimento.ToString("dd/MM/yyyy HH:mm:ss")
    '            Session("LoadedRTDP") = New Allegato() With {
    '             .Blob = blob,
    '             .Filename = filename,
    '             .Hash = hashValue,
    '             .Filesize = filelength,
    '             .DataInserimento = dataInserimento
    '            }

    '        Else
    '            dtrAllegato.Close()
    '            ClearSessionAN()
    '        End If
    '    End If
    '    Dim RTDP As Allegato = Session("LoadedRTDP")

    '    If RTDP IsNot Nothing Then
    '        'Se Il CV è caricato in Sessione (Inserimento)
    '        rowNoRTDP.Visible = False
    '        rowRTDP.Visible = True
    '        txtRTDPFilename.Text = RTDP.Filename
    '        txtRTDPHash.Text = RTDP.Hash
    '        txtRTDPData.Text = RTDP.DataInserimento.ToString("dd/MM/yyyy HH:mm:ss")
    '    Else
    '        'Se Il CV non è ancora caricato
    '        rowNoRTDP.Visible = True
    '        rowRTDP.Visible = False
    '    End If
    'End Sub

    'Protected Sub btnModificaRTDP_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles btnModificaRTDP.Click
    '    mpeRTDP.Show()
    'End Sub

    'Protected Sub btnDownloadRTDP_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles btnDownloadRTDP.Click
    '    If Session("LoadedRTDP") Is Nothing Then
    '        MessaggiAlert("Nessun File caricato")
    '        ClearSessionAN()
    '        Exit Sub
    '    End If
    '    Dim RTDP As Allegato = Session("LoadedRTDP")
    '    Response.Clear()
    '    Response.ContentType = "Application/pdf"
    '    Response.AddHeader("Content-Disposition", "attachment; filename=" & RTDP.Filename)
    '    Response.BinaryWrite(RTDP.Blob)
    '    Response.End()
    'End Sub
    Private Function InserisciAllegati(trans As SqlClient.SqlTransaction, ByRef IDAllegato As Integer, allegato As Allegato) As Boolean
        Dim SqlCmd As New SqlClient.SqlCommand()

        Try
            'Prendo gli allegati salvati in sessione


            SqlCmd.CommandText = "sp_InsAllegato"
            SqlCmd.CommandType = CommandType.StoredProcedure
            SqlCmd.Transaction = trans
            SqlCmd.Connection = Session("Conn")
            SqlCmd.Parameters.Clear()
            SqlCmd.Parameters.Add("@IdEnte", SqlDbType.Int).Value = Session("IdEnte")
            'Tipo allegato Atto di nomina 

            SqlCmd.Parameters.Add("@IdTipoAllegato", SqlDbType.Int).Value = allegato.IdTipoAllegato
            'Blob
            SqlCmd.Parameters.Add("@BinData", SqlDbType.VarBinary, -1).Value = allegato.Blob
            'FileName
            SqlCmd.Parameters.Add("@FileName", SqlDbType.VarChar, 255).Value = allegato.Filename
            'Hash
            SqlCmd.Parameters.Add("@Hash", SqlDbType.VarChar, 100).Value = allegato.Hash
            'FileLength
            SqlCmd.Parameters.Add("@FileLength", SqlDbType.BigInt, 100).Value = allegato.Filesize

            'DataInserimento
            SqlCmd.Parameters.Add("@DataIns", SqlDbType.DateTime, 100).Value = allegato.DataInserimento

            'UserNAme
            SqlCmd.Parameters.Add("@UserName", SqlDbType.VarChar, 20).Value = Session("Utente").ToString

            If Not Session("IdEnteFaseArt") Is Nothing Then
                'IdEnteFaseDestinazione
                SqlCmd.Parameters.Add("@IdEnteFaseDestinazione", SqlDbType.Int).Value = Session("IdEnteFaseArt").ToString
                'LogParametri.Add("@IdEnteFaseDestinazione", Session("IdEnteFaseArt").ToString)
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
                Log.Information(Logger.Data.LogEvent.ANAGRAFICAENTE_INSERIMENTO_ALLEGATI_CORRETTA, SqlCmd.Parameters("@messaggio").Value())
                InserisciAllegati = True
                Exit Function
            Else
                trans.Rollback()
                Log.Warning(Logger.Data.LogEvent.ANAGRAFICAENTE_INSERIMENTO_ALLEGATI_ERRATA, SqlCmd.Parameters("@messaggio").Value())
                InserisciAllegati = False
                Exit Function
            End If
        Catch ex As Exception
            lblMessaggio.Text = ex.Message
            trans.Rollback()
            Log.Error(Logger.Data.LogEvent.ANAGRAFICAENTE_INSERIMENTO_ALLEGATI_ERRATA, ex.Message)
            InserisciAllegati = False
            Exit Function
        End Try

    End Function



    Private Sub VerificaClassi()

        If Session("IDClasseAccreditamento") Is Nothing _
            And ddlClassi.SelectedValue <> "0" Then

            strsql = "SELECT * FROM CLASSIACCREDITAMENTO "
            strsql += "WHERE IDCLASSEACCREDITAMENTO = "
            strsql += "(SELECT IDCLASSEACCREDITAMENTO FROM SEZIONIALBOSCU WHERE IDSEZIONE =" & ddlClassi.SelectedValue & ") "

            dtrgenerico = ClsServer.CreaDatareader(strsql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))

            dtrgenerico.Read()
            If dtrgenerico.HasRows = True Then
                Session("IDClasseAccreditamento") = dtrgenerico("IDClasseAccreditamento")
            End If

            If Not dtrgenerico Is Nothing Then
                dtrgenerico.Close()
                dtrgenerico = Nothing
            End If

        End If

    End Sub

    Private Function AggiornaSettoriAreeIntervento(trans As SqlClient.SqlTransaction) As Boolean

        Dim EsperienzeAreeSettore As New List(Of clsEsperienzeAree)
        Dim SqlCmd As New SqlClient.SqlCommand()
        Dim esito As Boolean = True
        Dim contaEsperienze As Integer

        If Session("IdStatoEnte") = "6" OrElse (Session("IdStatoEnte") <> "6" AndAlso Session("EsperienzeAreeSettore") IsNot Nothing) Then
            If Session("EsperienzeAreeSettore") IsNot Nothing Then
                EsperienzeAreeSettore = CType(Session("EsperienzeAreeSettore"), List(Of clsEsperienzeAree))
            Else
                EsperienzeAreeSettore.Clear()
            End If

            Try

                'PER L'ENTE VENGONO STORICIZZATE LE ARRE D'INTERVENTO DI OGNI SETTORE INIZIO
                SqlCmd.CommandText = "SP_UPD_AREE_ESPERIENZE_SETTORE"
                SqlCmd.CommandType = CommandType.StoredProcedure
                SqlCmd.Transaction = trans
                SqlCmd.Connection = Session("Conn")
                SqlCmd.Parameters.Clear()
                SqlCmd.Parameters.Add("@IdEnte", SqlDbType.Int).Value = Session("IdEnte")
                SqlCmd.Parameters.Add("@UsernameRichiesta", SqlDbType.NVarChar, 10).Value = Session("Utente")
                'Esito aggiornamento: 0-Errore 1-Aggiornamento effettuato
                SqlCmd.Parameters.Add("@Esito", SqlDbType.TinyInt)
                SqlCmd.Parameters("@Esito").Direction = ParameterDirection.Output

                SqlCmd.Parameters.Add("@messaggio", SqlDbType.VarChar)
                SqlCmd.Parameters("@messaggio").Size = 1000
                SqlCmd.Parameters("@messaggio").Direction = ParameterDirection.Output
                SqlCmd.ExecuteNonQuery()

                MessaggiConvalida(SqlCmd.Parameters("@messaggio").Value())
                If SqlCmd.Parameters("@Esito").Value = 0 Then
                    esito = False
                    trans.Rollback()
                    AggiornaSettoriAreeIntervento = False
                    Exit Function
                End If

                'PER L'ENTE VENGONO STORICIZZATE LE ARRE D'INTERVENTO DI OGNI SETTORE FINE


                SqlCmd.CommandText = "SP_INS_AREE_ESPERIENZE_SETTORE"
                SqlCmd.CommandType = CommandType.StoredProcedure
                SqlCmd.Transaction = trans
                SqlCmd.Connection = Session("Conn")


                For Each EsperienzaArea As clsEsperienzeAree In EsperienzeAreeSettore
                    Dim areeEsperienze As String = String.Empty
                    SqlCmd.Parameters.Clear()
                    SqlCmd.Parameters.Add("@IdEnte", SqlDbType.Int).Value = Session("IdEnte")
                    SqlCmd.Parameters.Add("@IdSettore", SqlDbType.Int).Value = EsperienzaArea.IdSettore
                    SqlCmd.Parameters.Add("@I_Esperienza", SqlDbType.VarChar, 1500).Value = EsperienzaArea.DescrizioneEsperienza(0)
                    SqlCmd.Parameters.Add("@II_Esperienza", SqlDbType.VarChar, 1500).Value = EsperienzaArea.DescrizioneEsperienza(1)
                    SqlCmd.Parameters.Add("@III_Esperienza", SqlDbType.VarChar, 1500).Value = EsperienzaArea.DescrizioneEsperienza(2)
                    SqlCmd.Parameters.Add("@I_AnnoEsperienza", SqlDbType.Int).Value = EsperienzaArea.AnnoEsperienza(0)
                    SqlCmd.Parameters.Add("@II_AnnoEsperienza", SqlDbType.Int).Value = EsperienzaArea.AnnoEsperienza(1)
                    SqlCmd.Parameters.Add("@III_AnnoEsperienza", SqlDbType.Int).Value = EsperienzaArea.AnnoEsperienza(2)

                    For Each area In EsperienzaArea.AreeSelezionate
                        areeEsperienze += area + ","
                    Next

                    areeEsperienze = Left(areeEsperienze, areeEsperienze.Length - 1)

                    SqlCmd.Parameters.Add("@AreeIntervento", SqlDbType.VarChar, -1).Value = areeEsperienze


                    SqlCmd.Parameters.Add("@UsernameRichiesta", SqlDbType.NVarChar, 10).Value = Session("Utente")

                    'Esito aggiornamento: 0-Errore 1-Aggiornamento effettuato
                    SqlCmd.Parameters.Add("@Esito", SqlDbType.TinyInt)
                    SqlCmd.Parameters("@Esito").Direction = ParameterDirection.Output

                    SqlCmd.Parameters.Add("@messaggio", SqlDbType.VarChar)
                    SqlCmd.Parameters("@messaggio").Size = 1000
                    SqlCmd.Parameters("@messaggio").Direction = ParameterDirection.Output


                    SqlCmd.ExecuteNonQuery()
                    '@messaggio varchar(1000) output
                    If SqlCmd.Parameters("@Esito").Value = 0 Then
                        esito = False
                        Exit For
                    End If
                Next
                If esito = True Then
                    trans.Commit()
                    MessaggiSuccess(SqlCmd.Parameters("@messaggio").Value())
                    Log.Information(Logger.Data.LogEvent.ANAGRAFICAENTE_MODIFICA_SETTORI_CORRETTA, SqlCmd.Parameters("@messaggio").Value)
                    AggiornaSettoriAreeIntervento = True
                    Exit Function
                Else
                    trans.Rollback()
                    MessaggiAlert(SqlCmd.Parameters("@messaggio").Value())
                    Log.Warning(Logger.Data.LogEvent.ANAGRAFICAENTE_MODIFICA_SETTORI_ERRATA, SqlCmd.Parameters("@messaggio").Value)
                    AggiornaSettoriAreeIntervento = False
                    Exit Function
                End If
            Catch ex As Exception
                lblMessaggio.Text = ex.Message
                trans.Rollback()
                Log.Error(Logger.Data.LogEvent.ANAGRAFICAENTE_MODIFICA_SETTORI_ERRATA, SqlCmd.Parameters("@messaggio").Value)
                AggiornaSettoriAreeIntervento = False
                Exit Function
            End Try
        Else
            MessaggiSuccess("OPERAZIONE AVVENUTA CON SUCCESSO")
            trans.Commit()
            AggiornaSettoriAreeIntervento = True
            Exit Function
        End If

    End Function

    Private Sub CaricaEntiAllegati()
        Dim dtAllegati As DataTable
        Session("LoadedAN") = Nothing

        strsql = String.Empty
        strsql &= "SELECT coalesce(IdTipoAllegato, 0) IdTipoAllegato, FileName, BinData, DataInserimento, len(BinData) FileLength, HashValue, IdEnteDocumento, IdEnteFase, Stato FROM EntiDocumenti D "
        strsql &= " INNER JOIN ENTI E ON E.IdEnte= " & Session("IdEnte")
        strsql &= " AND D.IdEnteDocumento in (E.IdAllegatoDeliberaStrutturaGestione, E.IdAllegatoDocumentoNomina, "
        strsql &= "E.IdAllegatoRTDPersonali, E.IdAllegatoAttoCostitutivo, E.IdAllegatoStatuto,  "
        strsql &= "E.IdAllegatoDeliberaAdesione, E.IdAllegatoImpegnoEtico)"

        dtAllegati = ClsServer.DataTableGenerico(strsql, Session("Conn"))

        If dtAllegati IsNot Nothing _
        AndAlso dtAllegati.Rows.Count > 0 Then
            'metto in sessione gli allegati

            For Each dr As DataRow In dtAllegati.Rows
                If CInt(dr("IdTipoAllegato").ToString()) = TipoFile.DOCUMENTO_NOMINA Then
                    CaricaAllegatoFromDatarow(dr, "LoadedAN", rowNoAN, rowAN, txtANFilename, txtANHash, txtANData)
                End If
                If CInt(dr("IdTipoAllegato").ToString()) = TipoFile.DELIBERA Then
                    CaricaAllegatoFromDatarow(dr, "LoadedDelibera", rowNoDelibera, rowDelibera, txtDeliberaFilename, txtDeliberaHash, txtDeliberaData)
                End If
                If CInt(dr("IdTipoAllegato").ToString()) = TipoFile.ATTO_COSTITUTIVO_ENTE Then
                    CaricaAllegatoFromDatarow(dr, "LoadedAC", rowNoAttoCostitutivo, rowAttoCostitutivo, txtACFilename, txtACHash, txtACData)
                End If

                If CInt(dr("IdTipoAllegato").ToString()) = TipoFile.STATUTO_ENTE Then
                    CaricaAllegatoFromDatarow(dr, "LoadedStatuto", rowNoStatuto, rowStatuto, txtStatutoFilename, txtStatutoHash, txtStatutoData)
                End If


                If ddlTipologia.SelectedValue = String.Empty OrElse ddlTipologia.SelectedValue.ToUpper = "PUBBLICO" Then
                    If CInt(dr("IdTipoAllegato").ToString()) = TipoFile.DELIBERA_COSTITUZIONE_ENTE Then
                        CaricaAllegatoFromDatarow(dr, "LoadedDeliberaAdesione", rowNoDeliberaAdesione, rowDeliberaAdesione, txtDeliberaAdesioneFileName, txtDeliberaAdesioneHash, txtDeliberaAdesioneData)
                    End If
                Else
                    rowNoDeliberaAdesione.Visible = False
                    rowDeliberaAdesione.Visible = False
                End If

                If CInt(dr("IdTipoAllegato").ToString()) = TipoFile.CARTA_IMPEGNO_ETICO Then
                    CaricaAllegatoFromDatarow(dr, "LoadedCartaImpegnoEtico", rowNoImpegnoEtico, rowImpegnoEtico, txtCIEFilename, txtCIEHash, txtCIEData)
                End If
            Next
        End If
    End Sub

    Protected Sub cmdAllegaAttoCostitutivo_Click(sender As Object, e As EventArgs) Handles cmdAllegaAttoCostitutivo.Click
        mpeAttoCostitutivo.Show()
    End Sub

    Protected Sub btnDownloadAttoCostitutivo_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles btnDownloadAttoCostitutivo.Click
        If Session("LoadedAC") Is Nothing Then
            MessaggiAlert("Nessun File caricato")
            ClearSessionAttoCostitutivo()
            Exit Sub
        End If
        Dim AC As Allegato = Session("LoadedAC")
        Response.Clear()
        Response.ContentType = "Application/pdf"
        Response.AddHeader("Content-Disposition", "attachment; filename=" & AC.Filename)
        Response.BinaryWrite(AC.Blob)
        Response.End()
    End Sub

    Protected Sub btnModificaAttoCostitutivo_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles btnModificaAttoCostitutivo.Click
        mpeAttoCostitutivo.Show()
    End Sub

    Protected Sub btnEliminaAttoCostitutitvo_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles btnEliminaAttoCostitutitvo.Click
        ClearSessionAttoCostitutivo()
    End Sub

    Protected Sub cmdAllegaAC_Click(sender As Object, e As EventArgs) Handles cmdAllegaAC.Click
        lblMessaggio.Text = String.Empty
        'Verifica se è stato inserito il file
        If fileAttoCostitutivo.PostedFile Is Nothing Or String.IsNullOrEmpty(fileAttoCostitutivo.PostedFile.FileName) Then
            lblErroreAttoCostitutivo.Text = "Non è stato scelto nessun file per il caricamento dell' atto costitutivo dell'ente"
            mpeAttoCostitutivo.Show()
            Exit Sub
        End If
        'Controllo Tipo File
        If clsGestioneDocumentiAccreditamento.VerificaEstensioneFileAccreditamento(fileAttoCostitutivo) = False Then
            lblErroreAttoCostitutivo.Text = "Il formato file  dell' atto costitutivo dell'ente non è corretto. È possibile associare documenti nel formato .PDF o .PDF.P7M"
            mpeAttoCostitutivo.Show()
            Exit Sub
        End If
        'Controlli dimensioni del file
        Dim fs = fileAttoCostitutivo.PostedFile.InputStream
        Dim iLen As Integer = CInt(fs.Length)
        Dim bBLOBStorage(iLen) As Byte

        If iLen <= 0 Then
            lblErroreAttoCostitutivo.Text = "Attenzione. Impossibile caricare l'atto costitutivo dell'ente vuoto."
            mpeAttoCostitutivo.Show()
            Exit Sub
        End If
        If iLen > 20971520 Then
            lblErroreAttoCostitutivo.Text = "Attenzione. La dimensione massima dell atto di nomina del Rappresentante Legale è di 20 MB."
            mpeAttoCostitutivo.Show()
            Exit Sub
        End If
        bBLOBStorage = ClsServer.StreamToByte(fs)
        fs.Close()

        Dim hashValue As String
        hashValue = GeneraHash(bBLOBStorage)

        Dim estensione As String = System.IO.Path.GetExtension(fileAttoCostitutivo.PostedFile.FileName)
        If UCase(estensione) = ".P7M" Then estensione = ".pdf.p7m"

        'Salvo File In Sessione
        Dim AC As New Allegato() With {
         .Updated = True,
         .Blob = bBLOBStorage,
        .Filename = "ATTOCOSTITUTIVO_" & txtCodFis.Text & estensione,
         .Hash = hashValue,
         .Filesize = iLen,
         .DataInserimento = Date.Now,
         .IdTipoAllegato = TipoFile.ATTO_COSTITUTIVO_ENTE
        }

        'If EsistenzaAllegatoEnteDB(AC.Hash) = False Then
        '    lblErroreImpegnoEtico.Text = "È stato inserito un allegato " & AC.Filename & " che è già presente in archivio per questo Ente"
        '    AC = Nothing
        '    mpeImpegnoEtico.Show()
        '    Exit Sub
        'End If

        Session("LoadedAC") = AC
        'Se Il CV è caricato in Sessione (Inserimento)
        rowNoAttoCostitutivo.Visible = False
        rowAttoCostitutivo.Visible = True
        txtACFilename.Text = AC.Filename
        txtACHash.Text = AC.Hash
        txtACData.Text = AC.DataInserimento.ToString("dd/MM/yyyy HH:mm:ss")
        MessaggiConvalida("ATTO COSTITUTIVO DELL'ENTE CORRETTAMENTE")
    End Sub

    Protected Sub cmdAllegaStatuto_Click(sender As Object, e As EventArgs) Handles cmdAllegaStatuto.Click
        mpeStatuto.Show()
    End Sub

    Protected Sub btnDownloadStatuto_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles btnDownloadStatuto.Click
        If Session("LoadedStatuto") Is Nothing Then
            MessaggiAlert("Nessun File caricato")
            ClearSessionStatuto()
            Exit Sub
        End If
        Dim ST As Allegato = Session("LoadedStatuto")
        Response.Clear()
        Response.ContentType = "Application/pdf"
        Response.AddHeader("Content-Disposition", "attachment; filename=" & ST.Filename)
        Response.BinaryWrite(ST.Blob)
        Response.End()
    End Sub

    Protected Sub btnModificaStatuto_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles btnModificaStatuto.Click
        mpeStatuto.Show()
    End Sub

    Protected Sub btnEliminaStatuto_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles btnEliminaStatuto.Click
        ClearSessionStatuto()
    End Sub

    Protected Sub cmdAllegaST_Click(sender As Object, e As EventArgs) Handles cmdAllegaST.Click
        lblMessaggio.Text = String.Empty
        'Verifica se è stato inserito il file
        If fileStatuto.PostedFile Is Nothing Or String.IsNullOrEmpty(fileStatuto.PostedFile.FileName) Then
            lblErroreStatuto.Text = "Non è stato scelto nessun file per il caricamento dello Statuto dell'ente"
            mpeStatuto.Show()
            Exit Sub
        End If
        'Controllo Tipo File
        If clsGestioneDocumentiAccreditamento.VerificaEstensioneFileAccreditamento(fileStatuto) = False Then
            lblErroreStatuto.Text = "Il formato file  dello Statuto dell'ente non è corretto. È possibile associare documenti nel formato .PDF o .PDF.P7M"
            mpeStatuto.Show()
            Exit Sub
        End If
        'Controlli dimensioni del file
        Dim fs = fileStatuto.PostedFile.InputStream
        Dim iLen As Integer = CInt(fs.Length)
        Dim bBLOBStorage(iLen) As Byte

        If iLen <= 0 Then
            lblErroreStatuto.Text = "Attenzione. Impossibile caricare lo Statuto dell'ente vuoto."
            mpeStatuto.Show()
            Exit Sub
        End If
        If iLen > 20971520 Then
            lblErroreStatuto.Text = "Attenzione. La dimensione massima dello Statuto è di 20 MB."
            mpeStatuto.Show()
            Exit Sub
        End If
        bBLOBStorage = ClsServer.StreamToByte(fs)

        fs.Close()

        Dim hashValue As String
        hashValue = GeneraHash(bBLOBStorage)

        Dim estensione As String = System.IO.Path.GetExtension(fileStatuto.PostedFile.FileName)
        If UCase(estensione) = ".P7M" Then estensione = ".pdf.p7m"

        'Salvo File In Sessione
        Dim ST As New Allegato() With {
         .Updated = True,
         .Blob = bBLOBStorage,
         .Filename = "STATUTO_" & txtCodFis.Text & estensione,
         .Hash = hashValue,
         .Filesize = iLen,
         .DataInserimento = Date.Now,
        .IdTipoAllegato = TipoFile.STATUTO_ENTE
        }

        'If EsistenzaAllegatoEnteDB(ST.Hash) = False Then
        '    lblErroreImpegnoEtico.Text = "È stato inserito un allegato " & ST.Filename & " che è già presente in archivio per questo Ente"
        '    ST = Nothing
        '    mpeImpegnoEtico.Show()
        '    Exit Sub
        'End If

        Session("LoadedStatuto") = ST
        'Se lo statuto è caricato in Sessione (Inserimento)
        rowNoStatuto.Visible = False
        rowStatuto.Visible = True
        txtStatutoFilename.Text = ST.Filename
        txtStatutoHash.Text = ST.Hash
        txtStatutoData.Text = ST.DataInserimento.ToString("dd/MM/yyyy HH:mm:ss")
        MessaggiConvalida("STATUTO DELL'ENTE CARICATO CORRETTAMENTE")
    End Sub

    Protected Sub btnDownloadDeliberaAdesione_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles btnDownloadDeliberaAdesione.Click
        If Session("LoadedDeliberaAdesione") Is Nothing Then
            MessaggiAlert("Nessun File caricato")
            ClearDeliberaAdesione()
            Exit Sub
        End If
        Dim DelAdesione As Allegato = Session("LoadedDeliberaAdesione")
        Response.Clear()
        Response.ContentType = "Application/pdf"
        Response.AddHeader("Content-Disposition", "attachment; filename=" & DelAdesione.Filename)
        Response.BinaryWrite(DelAdesione.Blob)
        Response.End()
    End Sub

    Protected Sub btnModificaDeliberaAdesione_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles btnModificaDeliberaAdesione.Click
        mpeDeliberaAdesione.Show()
    End Sub

    Protected Sub btnEliminaDeliberaAdesione_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles btnEliminaDeliberaAdesione.Click
        ClearDeliberaAdesione()
    End Sub

    Protected Sub cmdAllegaDA_Click(sender As Object, e As EventArgs) Handles cmdAllegaDA.Click
        lblMessaggio.Text = String.Empty
        'Verifica se è stato inserito il file
        If fileDeliberaAdesione.PostedFile Is Nothing Or String.IsNullOrEmpty(fileDeliberaAdesione.PostedFile.FileName) Then
            lblErroreDeliberaAdesione.Text = "Non è stato scelto nessun file per il caricamento della delibera "
            mpeDeliberaAdesione.Show()
            Exit Sub
        End If
        'Controllo Tipo File
        If clsGestioneDocumentiAccreditamento.VerificaEstensioneFileAccreditamento(fileDeliberaAdesione) = False Then
            lblErroreDeliberaAdesione.Text = "Il formato file  della delibera di non è corretto. È possibile associare documenti nel formato .PDF o .PDF.P7M"
            mpeDeliberaAdesione.Show()
            Exit Sub
        End If

        'Controlli dimensioni del file
        Dim fs = fileDeliberaAdesione.PostedFile.InputStream
        Dim iLen As Integer = CInt(fs.Length)
        Dim bBLOBStorage(iLen) As Byte

        If iLen <= 0 Then
            lblErroreDeliberaAdesione.Text = "Attenzione. Impossibile caricare la delibera dell'ente vuoto."
            mpeDeliberaAdesione.Show()
            Exit Sub
        End If

        If iLen > 20971520 Then
            lblErroreDeliberaAdesione.Text = "Attenzione. La dimensione massima della delibera è di 20 MB."
            mpeDeliberaAdesione.Show()
            Exit Sub
        End If

        bBLOBStorage = ClsServer.StreamToByte(fs)

        fs.Close()

        Dim hashValue As String
        hashValue = GeneraHash(bBLOBStorage)

        Dim estensione As String = System.IO.Path.GetExtension(fileDeliberaAdesione.PostedFile.FileName)
        If UCase(estensione) = ".P7M" Then estensione = ".pdf.p7m"

        'Salvo File In Sessione
        Dim DA As New Allegato() With {
         .Updated = True,
         .Blob = bBLOBStorage,
         .Filename = "DELIBERA_" & txtCodFis.Text & estensione,
         .Hash = hashValue,
         .Filesize = iLen,
         .DataInserimento = Date.Now,
        .IdTipoAllegato = TipoFile.DELIBERA_COSTITUZIONE_ENTE
        }

        'If EsistenzaAllegatoEnteDB(DA.Hash) = False Then
        '    lblErroreImpegnoEtico.Text = "È stato inserito un allegato " & DA.Filename & " che è già presente in archivio per questo Ente"
        '    DA = Nothing
        '    mpeImpegnoEtico.Show()
        '    Exit Sub
        'End If

        Session("LoadedDeliberaAdesione") = DA
        'Se lo statuto è caricato in Sessione (Inserimento)
        rowNoDeliberaAdesione.Visible = False
        rowDeliberaAdesione.Visible = True
        txtDeliberaAdesioneFileName.Text = DA.Filename
        txtDeliberaAdesioneHash.Text = DA.Hash
        txtDeliberaAdesioneData.Text = DA.DataInserimento.ToString("dd/MM/yyyy HH:mm:ss")
        MessaggiConvalida("DELIBERA ADESIONE CARICATA CORRETTAMENTE")
    End Sub

    Protected Sub cmdAllegaDeliberaAdesione_Click(sender As Object, e As EventArgs) Handles cmdAllegaDeliberaAdesione.Click
        mpeDeliberaAdesione.Show()
    End Sub

    Protected Sub cmdAllegaImpegnoEtico_Click(sender As Object, e As EventArgs) Handles cmdAllegaImpegnoEtico.Click
        mpeImpegnoEtico.Show()
    End Sub

    Protected Sub btnDownloadCIE_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles btnDownloadCIE.Click
        If Session("LoadedCartaImpegnoEtico") Is Nothing Then
            MessaggiAlert("Nessun File caricato")
            ClearSessionImpegnoEtico()
            Exit Sub
        End If
        Dim CIE As Allegato = Session("LoadedCartaImpegnoEtico")
        Response.Clear()
        Response.ContentType = "Application/pdf"
        Response.AddHeader("Content-Disposition", "attachment; filename=" & CIE.Filename)
        Response.BinaryWrite(CIE.Blob)
        Response.End()
    End Sub

    Protected Sub btnModificaCIE_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles btnModificaCIE.Click
        mpeImpegnoEtico.Show()
    End Sub

    Protected Sub btnEliminaCIE_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles btnEliminaCIE.Click
        ClearSessionImpegnoEtico()
    End Sub

    Protected Sub cmdAllegaIE_Click(sender As Object, e As EventArgs) Handles cmdAllegaIE.Click
        lblMessaggio.Text = String.Empty
        'Verifica se è stato inserito il file
        If fileDeliberaAdesione.PostedFile Is Nothing Or String.IsNullOrEmpty(fileImpegnoEtico.PostedFile.FileName) Then
            lblErroreImpegnoEtico.Text = "Non è stato scelto nessun file per il caricamento della Carta d'Impegno Etico"
            mpeImpegnoEtico.Show()
            Exit Sub
        End If
        'Controllo Tipo File
        If clsGestioneDocumentiAccreditamento.VerificaEstensioneFileAccreditamento(fileImpegnoEtico) = False Then
            lblErroreImpegnoEtico.Text = "Il formato file  della della Carta d'Impegno Etico non è corretto. È possibile associare documenti nel formato .PDF o .PDF.P7M"
            mpeImpegnoEtico.Show()
            Exit Sub
        End If
        'Controlli dimensioni del file
        Dim fs = fileImpegnoEtico.PostedFile.InputStream
        Dim iLen As Integer = CInt(fs.Length)
        Dim bBLOBStorage(iLen) As Byte

        If iLen <= 0 Then
            lblErroreImpegnoEtico.Text = "Attenzione. Impossibile caricare la Carta d'Impegno Etico dell'ente vuoto."
            mpeImpegnoEtico.Show()
            Exit Sub
        End If
        If iLen > 20971520 Then
            lblErroreImpegnoEtico.Text = "Attenzione. La dimensione massima della Carta d'Impegno Etico è di 20 MB."
            mpeImpegnoEtico.Show()
            Exit Sub
        End If


        bBLOBStorage = ClsServer.StreamToByte(fs)
        fs.Close()

        Dim hashValue As String
        hashValue = GeneraHash(bBLOBStorage)

        Dim estensione As String = System.IO.Path.GetExtension(fileImpegnoEtico.PostedFile.FileName)
        If UCase(estensione) = ".P7M" Then estensione = ".pdf.p7m"

        'Salvo File In Sessione
        Dim CIE As New Allegato() With {
         .Updated = True,
         .Blob = bBLOBStorage,
         .Filename = "CARTAIMPEGNOETICO_" & txtCodFis.Text & estensione,
         .Hash = hashValue,
         .Filesize = iLen,
         .DataInserimento = Date.Now,
        .IdTipoAllegato = TipoFile.CARTA_IMPEGNO_ETICO
        }

        'If EsistenzaAllegatoEnteDB(CIE.Hash) = False Then
        '    lblErroreImpegnoEtico.Text = "È stato inserito un allegato " & CIE.Filename & " che è già presente in archivio per questo Ente"
        '    CIE = Nothing
        '    mpeImpegnoEtico.Show()
        '    Exit Sub
        'End If

        Session("LoadedCartaImpegnoEtico") = CIE
        'Se lo statuto è caricato in Sessione (Inserimento)
        rowNoImpegnoEtico.Visible = False
        rowImpegnoEtico.Visible = True
        txtCIEFilename.Text = CIE.Filename
        txtCIEHash.Text = CIE.Hash
        txtCIEData.Text = CIE.DataInserimento.ToString("dd/MM/yyyy HH:mm:ss")
        MessaggiConvalida("Carta Impegno Etico caricata correttamente")
    End Sub

    Private Function CaricaEntiAllegatiDaDB() As List(Of Allegato)

        Dim dtAllegati As DataTable
        Dim ListaAllegato As New List(Of Allegato)

        strsql = "SELECT IdEnteDocumento, IdTipoAllegato, FileName, HashValue, IdEnteFase "
        strsql &= "FROM entidocumenti A "
        strsql &= "   INNER JOIN " & IIf(isVariazione, "Accreditamento_VariazioneEnti", "ENTI") & " E on E.IdEnte=" & Session("IdEnte")
        strsql &= " WHERE A.IdEnteDocumento in (E.IdAllegatoDeliberaStrutturaGestione, E.IdAllegatoDocumentoNomina, "
        strsql &= "E.IdAllegatoRTDPersonali, E.IdAllegatoAttoCostitutivo, E.IdAllegatoStatuto, "
        strsql &= "E.IdAllegatoDeliberaAdesione, E.IdAllegatoImpegnoEtico)"
        If isVariazione Then strsql &= " AND E.StatoVariazione = 0"

        dtAllegati = ClsServer.DataTableGenerico(strsql, Session("Conn"))

        If dtAllegati IsNot Nothing _
        AndAlso dtAllegati.Rows.Count > 0 Then
            For Each dr As DataRow In dtAllegati.Rows
                If CInt(dr("IdTipoAllegato").ToString()) = TipoFile.DOCUMENTO_NOMINA Then
                    Dim AllegatoAN As New Allegato
                    AllegatoAN.Id = dr("IdEnteDocumento")
                    AllegatoAN.Filename = CStr(dr("FileName"))
                    AllegatoAN.Blob = Nothing
                    AllegatoAN.DataInserimento = Nothing
                    AllegatoAN.Filesize = Nothing
                    AllegatoAN.Hash = dr("HashValue").ToString()
                    AllegatoAN.IdTipoAllegato = CInt(dr("IdTipoAllegato").ToString())
                    ListaAllegato.Add(AllegatoAN)
                End If

                If CInt(dr("IdTipoAllegato").ToString()) = TipoFile.DELIBERA Then
                    Dim AllegatoDelibera As New Allegato
                    AllegatoDelibera.Id = dr("IdEnteDocumento")
                    AllegatoDelibera.Filename = dr("FileName").ToString()
                    AllegatoDelibera.Blob = Nothing
                    AllegatoDelibera.DataInserimento = Nothing
                    AllegatoDelibera.Filesize = Nothing
                    AllegatoDelibera.Hash = dr("HashValue").ToString()
                    AllegatoDelibera.IdTipoAllegato = CInt(dr("IdTipoAllegato").ToString())
                    ListaAllegato.Add(AllegatoDelibera)
                End If

                If CInt(dr("IdTipoAllegato").ToString()) = TipoFile.ATTO_COSTITUTIVO_ENTE Then
                    Dim AllegatoAttoCostitutivo As New Allegato
                    AllegatoAttoCostitutivo.Id = dr("IdEnteDocumento")
                    AllegatoAttoCostitutivo.Filename = dr("FileName").ToString()
                    AllegatoAttoCostitutivo.Blob = Nothing
                    AllegatoAttoCostitutivo.DataInserimento = Nothing
                    AllegatoAttoCostitutivo.Filesize = Nothing
                    AllegatoAttoCostitutivo.Hash = dr("HashValue").ToString()
                    AllegatoAttoCostitutivo.IdTipoAllegato = CInt(dr("IdTipoAllegato").ToString())
                    ListaAllegato.Add(AllegatoAttoCostitutivo)
                End If

                If CInt(dr("IdTipoAllegato").ToString()) = TipoFile.STATUTO_ENTE Then
                    Dim AllegatoStatuto As New Allegato
                    AllegatoStatuto.Id = dr("IdEnteDocumento")
                    AllegatoStatuto.Filename = dr("FileName").ToString()
                    AllegatoStatuto.Blob = Nothing
                    AllegatoStatuto.DataInserimento = Nothing
                    AllegatoStatuto.Filesize = Nothing
                    AllegatoStatuto.Hash = dr("HashValue").ToString()
                    AllegatoStatuto.IdTipoAllegato = CInt(dr("IdTipoAllegato").ToString())
                    ListaAllegato.Add(AllegatoStatuto)
                End If

                If ddlTipologia.SelectedValue = String.Empty OrElse ddlTipologia.SelectedValue.ToUpper = "PUBBLICO" Then
                    If CInt(dr("IdTipoAllegato").ToString()) = TipoFile.DELIBERA_COSTITUZIONE_ENTE Then
                        Dim AllegatoDeliberaAdesione As New Allegato
                        AllegatoDeliberaAdesione.Id = dr("IdEnteDocumento")
                        AllegatoDeliberaAdesione.Filename = dr("FileName").ToString()
                        AllegatoDeliberaAdesione.Blob = Nothing
                        AllegatoDeliberaAdesione.DataInserimento = Nothing
                        AllegatoDeliberaAdesione.Filesize = Nothing
                        AllegatoDeliberaAdesione.Hash = dr("HashValue").ToString()
                        AllegatoDeliberaAdesione.IdTipoAllegato = CInt(dr("IdTipoAllegato").ToString())
                        ListaAllegato.Add(AllegatoDeliberaAdesione)
                    End If
                End If

                If CInt(dr("IdTipoAllegato").ToString()) = TipoFile.CARTA_IMPEGNO_ETICO Then
                    Dim AllegatoImpegnoEtico As New Allegato
                    AllegatoImpegnoEtico.Id = dr("IdEnteDocumento")
                    AllegatoImpegnoEtico.Filename = dr("FileName").ToString()
                    AllegatoImpegnoEtico.Blob = Nothing
                    AllegatoImpegnoEtico.DataInserimento = Nothing
                    AllegatoImpegnoEtico.Filesize = Nothing
                    AllegatoImpegnoEtico.Hash = dr("HashValue").ToString()
                    AllegatoImpegnoEtico.IdTipoAllegato = CInt(dr("IdTipoAllegato").ToString())
                    ListaAllegato.Add(AllegatoImpegnoEtico)
                End If
            Next
        End If

        CaricaEntiAllegatiDaDB = ListaAllegato

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

    Protected Sub cmdModNoAdeguamento_Click(sender As Object, e As EventArgs) Handles cmdModNoAdeguamento.Click
        Response.Redirect("WfrmAnagraficaEnteNoAdeguamento.aspx")
    End Sub


    Private Sub CaricaEntiAllegatiVariazioni()
        Dim dtAllegati As DataTable
        Session("LoadedAN") = Nothing

        strsql = String.Empty
        strsql &= "SELECT FileName, BinData, DataInserimento, len(BinData) FileLength, HashValue, IdTipoAllegato, IdEnteDocumento, IdEnteFase, Stato FROM entidocumenti A"
        strsql &= " JOIN Accreditamento_VariazioneEnti E ON E.IdEnte= " & Session("IdEnte")
        strsql &= " AND E.StatoVariazione = 0"
        strsql &= " AND A.IdEnteDocumento in (E.IdAllegatoDeliberaStrutturaGestione, E.IdAllegatoDocumentoNomina, E.IdAllegatoRTDPersonali, "
        strsql &= " E.IdAllegatoAttoCostitutivo, E.IdAllegatoStatuto, E.IdAllegatoDeliberaAdesione, E.IdAllegatoImpegnoEtico)"

        dtAllegati = ClsServer.DataTableGenerico(strsql, Session("Conn"))

        If dtAllegati IsNot Nothing _
        AndAlso dtAllegati.Rows.Count > 0 Then
            'metto in sessione gli allegati

            For Each dr As DataRow In dtAllegati.Rows
                If CInt(dr("IdTipoAllegato").ToString()) = TipoFile.DOCUMENTO_NOMINA Then
                    CaricaAllegatoFromDatarow(dr, "LoadedAN", rowNoAN, rowAN, txtANFilename, txtANHash, txtANData)
                End If
                If CInt(dr("IdTipoAllegato").ToString()) = TipoFile.DELIBERA Then
                    CaricaAllegatoFromDatarow(dr, "LoadedDelibera", rowNoDelibera, rowDelibera, txtDeliberaFilename, txtDeliberaHash, txtDeliberaData)
                End If
                If CInt(dr("IdTipoAllegato").ToString()) = TipoFile.ATTO_COSTITUTIVO_ENTE Then
                    CaricaAllegatoFromDatarow(dr, "LoadedAC", rowNoAttoCostitutivo, rowAttoCostitutivo, txtACFilename, txtACHash, txtACData)
                End If
                If CInt(dr("IdTipoAllegato").ToString()) = TipoFile.STATUTO_ENTE Then
                    CaricaAllegatoFromDatarow(dr, "LoadedStatuto", rowNoStatuto, rowStatuto, txtStatutoFilename, txtStatutoHash, txtStatutoData)
                End If

                If ddlTipologia.SelectedValue = String.Empty OrElse ddlTipologia.SelectedValue.ToUpper = "PUBBLICO" Then

                    If CInt(dr("IdTipoAllegato").ToString()) = TipoFile.DELIBERA_COSTITUZIONE_ENTE Then
                        CaricaAllegatoFromDatarow(dr, "LoadedDeliberaAdesione", rowNoDeliberaAdesione, rowDeliberaAdesione, txtDeliberaAdesioneFileName, txtDeliberaAdesioneHash, txtDeliberaAdesioneData)
                    End If
                Else
                    rowNoDeliberaAdesione.Visible = False
                    rowDeliberaAdesione.Visible = False
                End If

                If CInt(dr("IdTipoAllegato").ToString()) = TipoFile.CARTA_IMPEGNO_ETICO Then
                    CaricaAllegatoFromDatarow(dr, "LoadedCartaImpegnoEtico", rowNoImpegnoEtico, rowImpegnoEtico, txtCIEFilename, txtCIEHash, txtCIEData)
                End If
            Next
        End If
    End Sub

    Private Sub cmdChiudi_Click(sender As Object, e As EventArgs) Handles cmdChiudi.Click
        'controllo se vengo dall'albero in gestione enti
        If Not Request.QueryString("VengoDa") Is Nothing Then
            'controllo se la variabile è valorizzata
            If Request.QueryString("VengoDa") <> "" Then
                'faccio la response.redirect verso l'albero
                Response.Redirect(Request.QueryString("VengoDa").ToString)
            End If
        End If
        Session("IdComune") = Nothing
        Response.Redirect("DettaglioFunzioni.aspx?IdVoceMenu=2")
    End Sub

    Sub RipristinaPulsantiAllegati()
        helper.RipristinaStyleDatiModificati(btnDownloadAttoCostitutivo)
        helper.RipristinaStyleDatiModificati(btnEliminaAttoCostitutitvo)
        helper.RipristinaStyleDatiModificati(btnModificaAttoCostitutivo)

        helper.RipristinaStyleDatiModificati(btnDownloadAN)
        helper.RipristinaStyleDatiModificati(btnEliminaAN)
        helper.RipristinaStyleDatiModificati(btnModificaAN)

        helper.RipristinaStyleDatiModificati(btnDownloadDelibera)
        helper.RipristinaStyleDatiModificati(btnEliminaDelibera)
        helper.RipristinaStyleDatiModificati(btnModificaDelibera)

        helper.RipristinaStyleDatiModificati(btnDownloadCIE)
        helper.RipristinaStyleDatiModificati(btnEliminaCIE)
        helper.RipristinaStyleDatiModificati(btnModificaCIE)

        helper.RipristinaStyleDatiModificati(btnDownloadDeliberaAdesione)
        helper.RipristinaStyleDatiModificati(btnEliminaDeliberaAdesione)
        helper.RipristinaStyleDatiModificati(btnModificaDeliberaAdesione)

        helper.RipristinaStyleDatiModificati(btnDownloadStatuto)
        helper.RipristinaStyleDatiModificati(btnEliminaStatuto)
        helper.RipristinaStyleDatiModificati(btnModificaStatuto)

    End Sub
    Sub ModificaPulsantiAllegati()
        helper.ModificaStyleDatiModificati(btnDownloadAttoCostitutivo)
        helper.ModificaStyleDatiModificati(btnEliminaAttoCostitutitvo)
        helper.ModificaStyleDatiModificati(btnModificaAttoCostitutivo)

        helper.ModificaStyleDatiModificati(btnDownloadAN)
        helper.ModificaStyleDatiModificati(btnEliminaAN)
        helper.ModificaStyleDatiModificati(btnModificaAN)

        helper.ModificaStyleDatiModificati(btnDownloadDelibera)
        helper.ModificaStyleDatiModificati(btnEliminaDelibera)
        helper.ModificaStyleDatiModificati(btnModificaDelibera)

        helper.ModificaStyleDatiModificati(btnDownloadCIE)
        helper.ModificaStyleDatiModificati(btnEliminaCIE)
        helper.ModificaStyleDatiModificati(btnModificaCIE)

        helper.ModificaStyleDatiModificati(btnDownloadDeliberaAdesione)
        helper.ModificaStyleDatiModificati(btnEliminaDeliberaAdesione)
        helper.ModificaStyleDatiModificati(btnModificaDeliberaAdesione)

        helper.ModificaStyleDatiModificati(btnDownloadStatuto)
        helper.ModificaStyleDatiModificati(btnEliminaStatuto)
        helper.ModificaStyleDatiModificati(btnModificaStatuto)
    End Sub
    Function getLabel(ByVal gRow As GridViewRow, ByVal contr As String) As String
        Dim label As Label = DirectCast(gRow.FindControl(contr), Label)
        Return Replace(label.Text, vbCrLf, "<br><br>")
    End Function
    Function getTextBox(ByVal gRow As GridViewRow, ByVal contr As String) As String
        Dim tb As TextBox = DirectCast(gRow.FindControl(contr), TextBox)
        Return Replace(tb.Text, vbCrLf, "<br><br>")
    End Function
    Function settoriHTML() As String
        Dim s As String = ""
        For Each gRow As GridViewRow In dtgSettori.Rows
            Dim check As CheckBox = DirectCast(gRow.FindControl("chkSeleziona"), CheckBox)
            If check.Checked Then
                s &= String.Format("<h1>{0}</h1>", getLabel(gRow, "lblSettoriIntervento"))
                s &= String.Format("<b>{0}</b>: {1}<br><br>", getLabel(gRow, "lblAnnoEsperienza3"), getTextBox(gRow, "txtDesEsperienza3"))
                s &= String.Format("<b>{0}</b>: {1}<br><br>", getLabel(gRow, "lblAnnoEsperienza2"), getTextBox(gRow, "txtDesEsperienza2"))
                s &= String.Format("<b>{0}</b>: {1}<br><br>", getLabel(gRow, "lblAnnoEsperienza1"), getTextBox(gRow, "txtDesEsperienza1"))

            End If
        Next
        Return s
    End Function
    Private Sub cmdPdfSettori_Click(sender As Object, e As EventArgs) Handles cmdPdfSettori.Click
        Dim oW As New AsposeWord()
        oW.open(Server.MapPath("download/Master/templateSettori.docx"))
        oW.addFieldValue("nomeEnte", txtdenominazione.Text)
        oW.addFieldHtml("htmlSettori", settoriHTML())
        oW.merge()
        Response.Clear()
        Response.ContentType = "Application/pdf"
        Response.AddHeader("Content-Disposition", "attachment; filename=" & txtCodFis.Text & "_esperienze_settori.pdf")
        Response.BinaryWrite(oW.pdfBytes)
        Response.End()
    End Sub

    Private Function AbilitaSettori() As Boolean
        Dim dtrGen As SqlClient.SqlDataReader
        Dim blnAdeguamento As Boolean
        If Not dtrGen Is Nothing Then
            dtrGen.Close()
            dtrGen = Nothing
        End If
        strsql = "select IdEnteFase from EntiFasi where TipoFase = 2 and GETDATE() between DataInizioFase and DataFineFase and IdEnte = " & Session("IdEnte")
        dtrGen = ClsServer.CreaDatareader(strsql, Session("Conn"))
        If dtrGen.HasRows = True Then
            blnAdeguamento = True
        Else
            blnAdeguamento = False
        End If
        dtrGen.Close()
        dtrGen = Nothing

        Dim blnAbilitaSettori As Boolean = True
        strsql = "SELECT VALORE  FROM Configurazioni where Parametro='BLOCCO_ACCREDITAMENTO'"
        If Not dtrGen Is Nothing Then
            dtrGen.Close()
            dtrGen = Nothing
        End If
        dtrGen = ClsServer.CreaDatareader(strsql, Session("conn"))
        dtrGen.Read()

        If dtrGen("Valore") = "SI" And Session("TipoUtente") = "E" And blnAdeguamento Then
            blnAbilitaSettori = False
        End If
        If Not dtrGen Is Nothing Then
            dtrGen.Close()
            dtrGen = Nothing
        End If
        Return blnAbilitaSettori

    End Function

    'Private Sub CmdSalvaDoc_Click(sender As Object, e As System.EventArgs) Handles CmdSalvaDoc.Click
    '    Dim AlboEnte As String
    '    VerificaClassi()

    '    AlboEnte = ClsUtility.TrovaAlboEnte(Session("IdEnte"), Session("Conn"))

    '    Dim ListaAllegati As New List(Of Allegato)

    '    ListaAllegati = CaricaEntiAllegatiDaDB()

    '    'Inserisco prima gli allegati che poi inserisco o aggiorno sull'ente
    '    Dim IDAllegatoAn As Integer
    '    Dim IDAllegatoRTDP As Integer
    '    Dim IDAllegatoDelibera As Integer
    '    Dim IDAllegatoDeliberaAdesione As Integer
    '    Dim IDAllegatoStatuto As Integer
    '    Dim IDAllegatoAttoCostitutivo As Integer
    '    Dim IDAllegatoImpegnoEtico As Integer
    '    Dim IDAllegato As Integer
    '    Dim trans As SqlClient.SqlTransaction = Session("conn").BeginTransaction(IsolationLevel.ReadCommitted)

    '    'Verifico se allegati già presenti sul DB 

    '    If (Session("LoadedAN") IsNot Nothing) Then
    '        ' confronto l'allegato preso dal Db con l'allegato presente in sessione
    '        If Not ConfrontaAllegati(ListaAllegati, DirectCast(Session("LoadedAN"), Allegato), IDAllegato) Then 'Verifico se gli allegati presi dal DB corrispondono con gli allegati presenti in sessione
    '            If (InserisciAllegati(trans, IDAllegato, DirectCast(Session("LoadedAN"), Allegato))) Then
    '                IDAllegatoAn = IDAllegato
    '            Else
    '                trans = Nothing
    '                Exit Sub
    '            End If
    '        Else
    '            IDAllegatoAn = IDAllegato
    '        End If
    '    End If

    '    If (Session("LoadedDelibera") IsNot Nothing) Then
    '        If Not ConfrontaAllegati(ListaAllegati, DirectCast(Session("LoadedDelibera"), Allegato), IDAllegato) Then 'Verifico se gli allegati presi dal DB corrispondono con gli allegati presenti in sessione
    '            If (InserisciAllegati(trans, IDAllegato, DirectCast(Session("LoadedDelibera"), Allegato))) Then
    '                IDAllegatoDelibera = IDAllegato
    '            Else
    '                trans = Nothing
    '                Exit Sub
    '            End If
    '        Else
    '            IDAllegatoDelibera = IDAllegato
    '        End If
    '    End If

    '    If (Session("LoadedAC") IsNot Nothing) Then
    '        If Not ConfrontaAllegati(ListaAllegati, DirectCast(Session("LoadedAC"), Allegato), IDAllegato) Then 'Verifico se gli allegati presi dal DB corrispondono con gli allegati presenti in sessione
    '            If (InserisciAllegati(trans, IDAllegato, DirectCast(Session("LoadedAC"), Allegato))) Then
    '                IDAllegatoAttoCostitutivo = IDAllegato
    '            Else
    '                trans = Nothing
    '                Exit Sub
    '            End If
    '        Else
    '            IDAllegatoAttoCostitutivo = IDAllegato
    '        End If
    '    End If

    '    If (Session("LoadedStatuto") IsNot Nothing) Then
    '        If Not ConfrontaAllegati(ListaAllegati, DirectCast(Session("LoadedStatuto"), Allegato), IDAllegato) Then 'Verifico se gli allegati presi dal DB corrispondono con gli allegati presenti in sessione
    '            If (InserisciAllegati(trans, IDAllegato, DirectCast(Session("LoadedStatuto"), Allegato))) Then
    '                IDAllegatoStatuto = IDAllegato
    '            Else
    '                trans = Nothing
    '                Exit Sub
    '            End If
    '        Else
    '            IDAllegatoStatuto = IDAllegato
    '        End If
    '    End If

    '    If (Session("LoadedDeliberaAdesione") IsNot Nothing) Then
    '        If Not ConfrontaAllegati(ListaAllegati, DirectCast(Session("LoadedDeliberaAdesione"), Allegato), IDAllegato) Then 'Verifico se gli allegati presi dal DB corrispondono con gli allegati presenti in sessione
    '            If (InserisciAllegati(trans, IDAllegato, DirectCast(Session("LoadedDeliberaAdesione"), Allegato))) Then
    '                IDAllegatoDeliberaAdesione = IDAllegato
    '            Else
    '                trans = Nothing
    '                Exit Sub
    '            End If
    '        Else
    '            IDAllegatoDeliberaAdesione = IDAllegato
    '        End If
    '    End If

    '    If (Session("LoadedCartaImpegnoEtico") IsNot Nothing) Then
    '        If Not ConfrontaAllegati(ListaAllegati, DirectCast(Session("LoadedCartaImpegnoEtico"), Allegato), IDAllegato) Then 'Verifico se gli allegati presi dal DB corrispondono con gli allegati presenti in sessione
    '            If (InserisciAllegati(trans, IDAllegato, DirectCast(Session("LoadedCartaImpegnoEtico"), Allegato))) Then
    '                IDAllegatoImpegnoEtico = IDAllegato
    '            Else
    '                trans = Nothing
    '                Exit Sub
    '            End If
    '        Else
    '            IDAllegatoImpegnoEtico = IDAllegato
    '        End If
    '    End If

    '    If Session("IdStatoEnte") <> 8 Then ' se adeguamento
    '        ModificaEnte(AlboEnte, IDAllegatoAn, IDAllegatoDelibera, IDAllegatoAttoCostitutivo, IDAllegatoStatuto, IDAllegatoDeliberaAdesione, IDAllegatoImpegnoEtico, trans)
    '    Else 'iscrizione
    '        Dim SqlCmd As New SqlClient.SqlCommand

    '        strsql = "UPDATE ENTI SET " & _
    '                " IdAllegatoDocumentoNomina = " & IDAllegatoAn & " " & _
    '                "	,idAllegatoDeliberaStrutturaGestione = " & IDAllegatoDelibera & " " & _
    '                "	,IdAllegatoAttoCostitutivo = " & IDAllegatoAttoCostitutivo & " " & _
    '                "	,IdAllegatoStatuto = " & IDAllegatoStatuto & " " & _
    '                "	,IdAllegatoDeliberaAdesione = " & IDAllegatoDeliberaAdesione & " " & _
    '                "	,IdAllegatoImpegnoEtico = " & IDAllegatoImpegnoEtico & " " & _
    '                " WHERE idente = " & Session("IdEnte").ToString
    '        SqlCmd.CommandText = strsql
    '        SqlCmd.CommandType = CommandType.Text
    '        SqlCmd.Connection = Session("Conn")
    '        SqlCmd.Transaction = trans

    '        SqlCmd.ExecuteNonQuery()


    '        trans.Commit()
    '    End If




    '    LoadMaschera()
    '    GestisciStyleDatiModificati()
    '    Session("IdComune") = Nothing
    'End Sub

    Protected Sub CmdSalvaDoc_Click(sender As Object, e As EventArgs) Handles CmdSalvaDoc.Click
        Dim AlboEnte As String
        VerificaClassi()

        AlboEnte = ClsUtility.TrovaAlboEnte(Session("IdEnte"), Session("Conn"))

        Dim ListaAllegati As New List(Of Allegato)

        ListaAllegati = CaricaEntiAllegatiDaDB()

        'Inserisco prima gli allegati che poi inserisco o aggiorno sull'ente
        Dim IDAllegatoAn As Integer
        Dim IDAllegatoRTDP As Integer
        Dim IDAllegatoDelibera As Integer
        Dim IDAllegatoDeliberaAdesione As Integer
        Dim IDAllegatoStatuto As Integer
        Dim IDAllegatoAttoCostitutivo As Integer
        Dim IDAllegatoImpegnoEtico As Integer
        Dim IDAllegato As Integer
        Dim trans As SqlClient.SqlTransaction = Session("conn").BeginTransaction(IsolationLevel.ReadCommitted)

        'Verifico se allegati già presenti sul DB 

        If (Session("LoadedAN") IsNot Nothing) Then
            ' confronto l'allegato preso dal Db con l'allegato presente in sessione
            If Not ConfrontaAllegati(ListaAllegati, DirectCast(Session("LoadedAN"), Allegato), IDAllegato) Then 'Verifico se gli allegati presi dal DB corrispondono con gli allegati presenti in sessione
                If (InserisciAllegati(trans, IDAllegato, DirectCast(Session("LoadedAN"), Allegato))) Then
                    IDAllegatoAn = IDAllegato
                Else
                    trans = Nothing
                    Exit Sub
                End If
            Else
                IDAllegatoAn = IDAllegato
            End If
        End If

        If (Session("LoadedDelibera") IsNot Nothing) Then
            If Not ConfrontaAllegati(ListaAllegati, DirectCast(Session("LoadedDelibera"), Allegato), IDAllegato) Then 'Verifico se gli allegati presi dal DB corrispondono con gli allegati presenti in sessione
                If (InserisciAllegati(trans, IDAllegato, DirectCast(Session("LoadedDelibera"), Allegato))) Then
                    IDAllegatoDelibera = IDAllegato
                Else
                    trans = Nothing
                    Exit Sub
                End If
            Else
                IDAllegatoDelibera = IDAllegato
            End If
        End If

        If (Session("LoadedAC") IsNot Nothing) Then
            If Not ConfrontaAllegati(ListaAllegati, DirectCast(Session("LoadedAC"), Allegato), IDAllegato) Then 'Verifico se gli allegati presi dal DB corrispondono con gli allegati presenti in sessione
                If (InserisciAllegati(trans, IDAllegato, DirectCast(Session("LoadedAC"), Allegato))) Then
                    IDAllegatoAttoCostitutivo = IDAllegato
                Else
                    trans = Nothing
                    Exit Sub
                End If
            Else
                IDAllegatoAttoCostitutivo = IDAllegato
            End If
        End If

        If (Session("LoadedStatuto") IsNot Nothing) Then
            If Not ConfrontaAllegati(ListaAllegati, DirectCast(Session("LoadedStatuto"), Allegato), IDAllegato) Then 'Verifico se gli allegati presi dal DB corrispondono con gli allegati presenti in sessione
                If (InserisciAllegati(trans, IDAllegato, DirectCast(Session("LoadedStatuto"), Allegato))) Then
                    IDAllegatoStatuto = IDAllegato
                Else
                    trans = Nothing
                    Exit Sub
                End If
            Else
                IDAllegatoStatuto = IDAllegato
            End If
        End If

        If (Session("LoadedDeliberaAdesione") IsNot Nothing) Then
            If Not ConfrontaAllegati(ListaAllegati, DirectCast(Session("LoadedDeliberaAdesione"), Allegato), IDAllegato) Then 'Verifico se gli allegati presi dal DB corrispondono con gli allegati presenti in sessione
                If (InserisciAllegati(trans, IDAllegato, DirectCast(Session("LoadedDeliberaAdesione"), Allegato))) Then
                    IDAllegatoDeliberaAdesione = IDAllegato
                Else
                    trans = Nothing
                    Exit Sub
                End If
            Else
                IDAllegatoDeliberaAdesione = IDAllegato
            End If
        End If

        If (Session("LoadedCartaImpegnoEtico") IsNot Nothing) Then
            If Not ConfrontaAllegati(ListaAllegati, DirectCast(Session("LoadedCartaImpegnoEtico"), Allegato), IDAllegato) Then 'Verifico se gli allegati presi dal DB corrispondono con gli allegati presenti in sessione
                If (InserisciAllegati(trans, IDAllegato, DirectCast(Session("LoadedCartaImpegnoEtico"), Allegato))) Then
                    IDAllegatoImpegnoEtico = IDAllegato
                Else
                    trans = Nothing
                    Exit Sub
                End If
            Else
                IDAllegatoImpegnoEtico = IDAllegato
            End If
        End If

        If Session("IdStatoEnte") <> 8 Then ' se adeguamento
            ModificaEnte(AlboEnte, IDAllegatoAn, IDAllegatoDelibera, IDAllegatoAttoCostitutivo, IDAllegatoStatuto, IDAllegatoDeliberaAdesione, IDAllegatoImpegnoEtico, trans)
        Else 'iscrizione
            Dim SqlCmd As New SqlClient.SqlCommand

            strsql = "UPDATE ENTI SET " & _
                    " IdAllegatoDocumentoNomina = " & IDAllegatoAn & " " & _
                    "	,idAllegatoDeliberaStrutturaGestione = " & IDAllegatoDelibera & " " & _
                    "	,IdAllegatoAttoCostitutivo = " & IDAllegatoAttoCostitutivo & " " & _
                    "	,IdAllegatoStatuto = " & IDAllegatoStatuto & " " & _
                    "	,IdAllegatoDeliberaAdesione = " & IDAllegatoDeliberaAdesione & " " & _
                    "	,IdAllegatoImpegnoEtico = " & IDAllegatoImpegnoEtico & " " & _
                    " WHERE idente = " & Session("IdEnte").ToString
            SqlCmd.CommandText = strsql
            SqlCmd.CommandType = CommandType.Text
            SqlCmd.Connection = Session("Conn")
            SqlCmd.Transaction = trans

            SqlCmd.ExecuteNonQuery()

            'Aggiorno Aree d'intervento per i settori selezionati inizio
            If (AggiornaSettoriAreeIntervento(trans)) Then 'la commit viene fatta all'interno
                trans = Nothing
            Else
                trans = Nothing
                Exit Sub
            End If

        End If

        LoadMaschera()
        GestisciStyleDatiModificati()
        Session("IdComune") = Nothing
    End Sub

    Sub AnnullaModificaDocumenti(IdEnte As String)
        'VA CHIAMATA PRIMA DELL'ANNULLAMENTO RECORD ACCREDITAMENTO
        Dim SqlCmd As New SqlCommand
        SqlCmd.CommandText = "SP_ACCREDITAMENTO_ANNULLA_MODIFICA_DOCUMENTI_ENTE_TITOLARE"
        SqlCmd.CommandType = CommandType.StoredProcedure
        SqlCmd.Connection = Session("Conn")
        SqlCmd.Parameters.Clear()
        SqlCmd.Parameters.Add("@IdEnte", SqlDbType.Int).Value = IdEnte

        SqlCmd.ExecuteNonQuery()
    End Sub

End Class

