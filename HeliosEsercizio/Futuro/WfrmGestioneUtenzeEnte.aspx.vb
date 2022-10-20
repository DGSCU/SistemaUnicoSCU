Imports System.Drawing
Imports System.IO
Imports System.Data.SqlClient
Imports System.Text.RegularExpressions.Regex
Public Class WfrmGestioneUtenzeEnte
    'Inherits System.Web.UI.Page
    Inherits SmartPage
    Dim dtsRisRicerca As DataSet
    Dim pLog As Hashtable
    Dim dtrgenerico As SqlClient.SqlDataReader
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        checkSpid()
        'controllo login effettuato
        If Not Session("LogIn") Is Nothing Then
            If Session("LogIn") = False Then
                Response.Redirect("LogOn.aspx")
            End If
        Else
            Response.Redirect("LogOn.aspx")
        End If
        'adc da far funzionare--------------------------------------------------------------------------------------------------------
        Dim strATTIVITA As Integer = -1
        Dim strBANDOATTIVITA As Integer = -1
        Dim strENTEPERSONALE As Integer = -1
        Dim strENTITA As Integer = -1
        Dim strIDENTE As Integer = -1
        If ClsUtility.SICUREZZA_VERIFICA_AUTORIZZAZIONI(Session("conn"), Session("IdEnte"), Session("txtCodEnte"), strATTIVITA, strBANDOATTIVITA, strENTEPERSONALE, strENTITA, Session("IdEnte")) = 1 Then

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
        'fina adc ----------------------------------------------------------------------------------------------------------------------

        'controllo se si tratta di primo caricamento della pagina
        If Page.IsPostBack = False Then
            'se è il primo accesso carico la griglia delle risorse con i criteri di ricerca
            RicercaDelegatiEnte(Session("idEnte"), 0) ' IL SECONDO PARAMETRO VIENE UTILIZZATO O PER RICERCA 1 PER INSERT UPDATE DELETE
        End If

    End Sub
    Sub RicercaDelegatiEnte(ByVal IDEnte As Integer, ByVal TIPO As Integer)
        'variabile stringa per la insert nella base dati
        Dim strSQL As String
        If Session("TipoUtente") <> "E" Then
            cmdNuovo.Visible = False
        End If

        Try
            If TIPO = 1 Then 'RICERCA SENZA FILTRI DOPO INSERT DELETE E UPDATE
                'preparo la query per la ricerca dei idEnteUtente
                strSQL = "SELECT dbo.EntiUtenti.idEnteUtente, dbo.EntiUtenti.IDEnte, dbo.EntiUtenti.CodiceFiscale, dbo.EntiUtenti.Cognome, dbo.EntiUtenti.Nome,"
                strSQL += " CASE WHEN dbo.EntiUtenti.Stato = 'True' THEN 'Abilitato' ELSE 'Disabilitato' END AS Stato, accessi.UltimoAccesso"
                strSQL += " FROM dbo.EntiUtenti LEFT OUTER JOIN"
                strSQL += " (select CodiceFiscale Name,"
                strSQL += " 	CONVERT(Varchar(10), MAX(Dataoraevento), 105) + ' ' + CONVERT(Varchar(10), MAX(Dataoraevento), 108) UltimoAccesso"
                strSQL += "  from "
                strSQL += " 	logaccessi L JOIN "
                strSQL += " 	EntiPassword P ON L.Username= P.Username JOIN"
                strSQL += " 	EntiUtenti D ON D.idEnteUtente = P.idEnteUtente"
                strSQL += " WHERE"
                strSQL += "     descrizione = 'LOGIN' "
                strSQL += " 	AND esito = 1 "
                strSQL += " 	AND P.IDEnte=" & IDEnte
                strSQL += " GROUP BY "
                strSQL += " 	CodiceFiscale)"
                strSQL += "AS accessi ON dbo.EntiUtenti.CodiceFiscale = accessi.Name "
                strSQL += " where idEnte=" & IDEnte & " AND dbo.EntiUtenti.Visibile = 1 "


            Else 'RICERCA CON FILTRI

                'preparo la query per la ricerca dei idEnteUtente
                strSQL = "SELECT dbo.EntiUtenti.idEnteUtente, dbo.EntiUtenti.IDEnte, dbo.EntiUtenti.CodiceFiscale, dbo.EntiUtenti.Cognome, dbo.EntiUtenti.Nome,"
                strSQL += " CASE WHEN dbo.EntiUtenti.Stato = 'True' THEN 'Abilitato' ELSE 'Disabilitato' END AS Stato, accessi.UltimoAccesso"
                strSQL += " FROM dbo.EntiUtenti LEFT OUTER JOIN"
                strSQL += " (select CodiceFiscale Name,"
                strSQL += " 	CONVERT(Varchar(10), MAX(Dataoraevento), 105) + ' ' + CONVERT(Varchar(10), MAX(Dataoraevento), 108) UltimoAccesso"
                strSQL += "  from "
                strSQL += " 	logaccessi L JOIN "
                strSQL += " 	EntiPassword P ON L.Username= P.Username JOIN"
                strSQL += " 	EntiUtenti D ON D.idEnteUtente = P.idEnteUtente"
                strSQL += " WHERE"
                strSQL += "     descrizione = 'LOGIN' "
                strSQL += " 	AND esito = 1 "
                strSQL += " 	AND P.IDEnte=" & IDEnte
                strSQL += " GROUP BY "
                strSQL += " 	CodiceFiscale)"
                strSQL += "AS accessi ON dbo.EntiUtenti.CodiceFiscale = accessi.Name "
                strSQL += " where idEnte=" & IDEnte & " AND dbo.EntiUtenti.Visibile = 1 "




                If txtCognome.Text <> "" Then
                    strSQL += " AND Cognome like '" & Replace(txtCognome.Text, "'", "''") & "%' "
                End If
                If txtNome.Text <> "" Then
                    strSQL += " AND Nome like '" & Replace(txtNome.Text, "'", "''") & "%' "
                End If
                If txtCodiceFiscale.Text <> "" Then
                    strSQL += " AND CodiceFiscale like '" & Replace(txtCodiceFiscale.Text, "'", "''") & "%' "
                End If
                If ddlStato.SelectedValue <> "" Then
                    Select Case ddlStato.SelectedValue
                        Case 0
                            strSQL += " AND dbo.EntiUtenti.Stato=0 "
                        Case 1
                            strSQL += " AND dbo.EntiUtenti.Stato=1 "
                    End Select
                End If


            End If
            strSQL = strSQL & " order by Cognome"
            'fine preparazione query
            '************************
            'eseguo la query e assegno il dataset alla datagrid del risultato della ricerca 
            dtsRisRicerca = ClsServer.DataSetGenerico(strSQL, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
            dtgRisultatoRicerca.DataSource = dtsRisRicerca
            dtgRisultatoRicerca.DataBind()
            Session("ricercadtg") = dtsRisRicerca
            If (dtgRisultatoRicerca.Items.Count > 0) Then
                DivGriglia.Visible = True
            Else
                DivGriglia.Visible = False
            End If
            GestisciPnlInserimento(False)
        Catch ex As Exception
            Response.Write(ex.Message.ToString())
            Log.Information(Logger.Data.LogEvent.DELEGATI_ERRORE_GENERICO, ex.Message)
        End Try
    End Sub

    Protected Sub imgRicerca_Click(sender As Object, e As EventArgs) Handles imgRicerca.Click
        'nascondo la label del messaggio
        lblerrore.Text = ""
        lblMess.Visible = False
        lblMessaggioConferma.Text = ""
        dtgProfiliUtente.DataSource = Nothing
        dtgProfiliUtente.DataBind()
        RicercaDelegatiEnte(Session("idEnte"), 0)
    End Sub

    Private Sub dtgRisultatoRicerca_PageIndexChanged(source As Object, e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dtgRisultatoRicerca.PageIndexChanged
        dtgRisultatoRicerca.CurrentPageIndex = e.NewPageIndex
        dtgRisultatoRicerca.DataSource = Session("ricercadtg")
        dtgRisultatoRicerca.DataBind()
    End Sub


    Private Sub dtgRisultatoRicerca_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles dtgRisultatoRicerca.SelectedIndexChanged
        Try
            GestisciPnlInserimento(True)
            lblTitoloPrincipale.Text = "Ricerca/Modifica utenze ente"
            lblerrore.Text = ""
            lblMessaggioConferma.Text = ""
            TxtHdTipoOperazione.Text = "U"
            If Session("TipoUtente") = "E" Then
                CmdElimina.Visible = True
            End If
            dtgProfiliUtente.Visible = True  'adc
            CmdSalva.Visible = True
            CmdElimina.Visible = True
            DivGriglia.Visible = False
            Dim Strsql
            'preparo la query per la ricerca delle risorse acquisite e non con l'aggiunta dei filtri
            'Strsql = "SELECT dbo.EntiUtenti.idEnteUtente, dbo.EntiUtenti.IDEnte, dbo.EntiUtenti.CodiceFiscale, dbo.EntiUtenti.Cognome, dbo.EntiUtenti.Nome,"
            'Strsql += " CASE WHEN dbo.EntiUtenti.Stato = 'True' THEN 'Abilitato' ELSE 'Disabilitato' END AS Stato, accessi.UltimoAccesso"
            'Strsql += " FROM dbo.EntiUtenti LEFT OUTER JOIN"
            'Strsql += " (SELECT CONVERT(Varchar(10), MAX(StartTime), 105) + ' ' + CONVERT(Varchar(10), MAX(StartTime), 108) AS UltimoAccesso, Username"
            'Strsql += " FROM db.[LOG] GROUP BY Username) AS accessi ON dbo.EntiUtenti.CodiceFiscale = accessi.Username"
            'Strsql += " where idEnteUtente=" & CInt(dtgRisultatoRicerca.SelectedItem.Cells(1).Text)

            Strsql = "SELECT dbo.EntiUtenti.idEnteUtente, dbo.EntiUtenti.IDEnte, dbo.EntiUtenti.CodiceFiscale, dbo.EntiUtenti.Cognome, dbo.EntiUtenti.Nome,"
            Strsql += " CASE WHEN dbo.EntiUtenti.Stato = 'True' THEN 'Abilitato' ELSE 'Disabilitato' END AS Stato, accessi.UltimoAccesso"
            Strsql += " FROM dbo.EntiUtenti LEFT OUTER JOIN"
            Strsql += " (SELECT CONVERT(Varchar(10), MAX(Dataoraevento), 105) + ' ' + CONVERT(Varchar(10), MAX(Dataoraevento), 108) as UltimoAccesso, Username"
            Strsql += " FROM logaccessi GROUP BY Username) AS accessi ON dbo.EntiUtenti.CodiceFiscale = accessi.Username"
            Strsql += " where idEnteUtente=" & CInt(dtgRisultatoRicerca.SelectedItem.Cells(1).Text)


            'Strsql = "SELECT dbo.EntiUtenti.idEnteUtente, dbo.EntiUtenti.IDEnte, dbo.EntiUtenti.CodiceFiscale, dbo.EntiUtenti.Cognome, dbo.EntiUtenti.Nome,"
            'Strsql += " CASE WHEN dbo.EntiUtenti.Stato = 'True' THEN 'Abilitato' ELSE 'Disabilitato' END AS Stato"
            'Strsql += " FROM dbo.EntiUtenti"
            'Strsql += " where idEnteUtente=" & CInt(dtgRisultatoRicerca.SelectedItem.Cells(1).Text)




            'fine preparazione query
            '************************
            Dim SqlCmd As New SqlCommand
            Dim dataAdapter As SqlDataAdapter = New SqlDataAdapter
            Dim tDelegato = New DataTable


            SqlCmd.CommandText = Strsql
            SqlCmd.CommandType = CommandType.Text
            SqlCmd.Connection = Session("conn")
            dataAdapter.SelectCommand = SqlCmd
            dataAdapter.Fill(tDelegato)

            txtCognome.Text = tDelegato.Rows(0)("Cognome")
            txtNome.Text = tDelegato.Rows(0)("Nome")
            txtCodiceFiscale.Text = tDelegato.Rows(0)("CodiceFiscale")
            TxtHdIdDelegato.Text = tDelegato.Rows(0)("idEnteUtente")
            TxtHdIdEnte.Text = tDelegato.Rows(0)("idEnte")
            TxtHdTipoOperazione.Text = "U"

            If tDelegato.Rows(0)("Stato") = "Abilitato" Then
                ddlStato.SelectedIndex = 2
            Else
                ddlStato.SelectedIndex = 1
            End If
            'If Session("TipoUtente") <> "E" Then
            '    txtCodiceFiscale.ReadOnly = True
            '    txtCognome.ReadOnly = True
            '    txtNome.ReadOnly = True
            '    ddlStato.Visible = False
            '    TxtStato.Text = tDelegato.Rows(0)("Stato")
            '    TxtStato.Visible = True

            'End If

            CaricaAbilitazioniAssociata(TxtHdIdDelegato.Text)

        Catch ex As Exception
            Log.Information(Logger.Data.LogEvent.DELEGATI_ERRORE_GENERICO, ex.Message)
        End Try
    End Sub
    Private Sub GestisciPnlInserimento(flVisible As Boolean)
        If Session("TipoUtente") = "E" Then
            If flVisible = True Then
                'pnlInserimento.Visible = True
                cmdNuovo.Visible = False
                TxtHdTipoOperazione.Text = "I"
                CmdSalva.Visible = True
                imgRicerca.Visible = False
                CmdElimina.Visible = False
                CmdAnnulla.Visible = True
                'ddlStato.SelectedIndex = -1
            Else
                'pnlInserimento.Visible = False
                cmdNuovo.Visible = True
                TxtHdTipoOperazione.Text = ""
                CmdSalva.Visible = False
                imgRicerca.Visible = True
                CmdElimina.Visible = False
                CmdAnnulla.Visible = False
                'ddlStato.SelectedIndex = -1
            End If
            'Else
            '    If flVisible = True Then
            '        'pnlInserimento.Visible = True
            '        cmdNuovo.Visible = False
            '        TxtHdTipoOperazione.Text = "I"
            '        CmdSalva.Visible = False
            '        imgRicerca.Visible = True
            '        CmdElimina.Visible = False
            '        CmdAnnulla.Visible = True
            '        ddlStato.SelectedIndex = -1
            '    Else
            '        'pnlInserimento.Visible = False
            '        cmdNuovo.Visible = False
            '        TxtHdTipoOperazione.Text = ""
            '        CmdSalva.Visible = False
            '        imgRicerca.Visible = True
            '        CmdElimina.Visible = False
            '        CmdAnnulla.Visible = False
            '        ddlStato.SelectedIndex = -1
            '    End If
        End If

    End Sub

    Private Function CaricaAbilitazioniAssociata(enteutente As String)
        ' AGGIUNTO DA SC IL 27/12/2016
        'assegnazione multipla dei profili utente
        Dim strsql As String
        Dim MyDataset As DataSet
        'Profili
        dtgProfiliUtente.SelectedIndex = 0
        'dtgProfiliUtente.DataSource = Nothing
        'strSql = "SELECT '0' As IdProfilo,'' As Descrizione FROM Profili "

        If Session("TipoUtente") = "E" Then

            strsql = "SELECT  IdProfilo,Descrizione FROM Profili WHERE Tipo = 'E' and idprofilo= (select valore from configurazioni where parametro = 'DEFAULT_PROFILO_CPP')"

        End If

        MyDataset = ClsServer.DataSetGenerico(strsql, Session("conn"))
        dtgProfiliUtente.DataSource = MyDataset
        dtgProfiliUtente.DataBind()
        dtgProfiliUtente.Visible = True

        If enteutente <> "" Then
            CaricaProfiliAssociati(enteutente)
        End If

        Return enteutente  'adc solo per non dare errore
    End Function
    Sub CaricaProfiliAssociati(enteutente As String)
        Dim strsql As String
        Dim dtrProfili As SqlClient.SqlDataReader
        'prima mi ricava lausername e poi mi prendo il profilo



        strsql = " SELECT idProfilo "
        strsql = strsql & " from  AssociaUtenteGruppo "
        strsql = strsql & " Inner Join entipassword on entipassword.username= AssociaUtenteGruppo.username "
        strsql = strsql & " WHERE entipassword.idEnteUtente='" & enteutente & "'"

        dtrProfili = ClsServer.CreaDatareader(strsql, Session("conn"))

        While dtrProfili.Read()
            For Each item In dtgProfiliUtente.Items
                Dim check As CheckBox = DirectCast(item.FindControl("chkSeleziona"), CheckBox)
                If dtrProfili("idProfilo") = item.Cells(0).Text() Then
                    check.Checked = True
                End If
            Next
        End While
        If Not dtrProfili Is Nothing Then
            dtrProfili.Close()
            dtrProfili = Nothing
        End If

    End Sub
    Protected Sub cmdNuovo_Click(sender As Object, e As EventArgs) Handles cmdNuovo.Click
        GestisciPnlInserimento(True)
        lblTitoloPrincipale.Text = "Inserimento utenza ente"
        lblerrore.Text = ""
        lblMessaggioConferma.Text = ""
        CaricaAbilitazioniAssociata(0)
        DivGriglia.Visible = False
        PulisciCampi()
    End Sub
    Sub PulisciCampi()
        txtCodiceFiscale.Text = ""
        txtCognome.Text = ""
        TxtHdIdDelegato.Text = "0"
        TxtHdIdEnte.Text = Session("IdEnte")
        TxtHdTipoOperazione.Text = "I"
        txtNome.Text = ""
        ddlStato.SelectedIndex = -1
        lblMessaggioConferma.Text = ""
        lblerrore.Text = ""
        'dtgProfiliUtente.DataSource = Nothing
        'dtgProfiliUtente.DataBind()
    End Sub
    Protected Sub CmdElimina_Click(sender As Object, e As EventArgs) Handles CmdElimina.Click
        Try
            Dim newDelegato As New Logger.Data.Entity
            lblerrore.Text = ""
            lblMessaggioConferma.Text = ""
            TxtHdTipoOperazione.Text = "C"
            pLog = New Hashtable()
            'Carico Valori da passare come parameters al log
            pLog.Add("Cognome", txtCognome.Text)
            pLog.Add("Nome", txtNome.Text)
            pLog.Add("CF", txtCodiceFiscale.Text)
            pLog.Add("Stato", ddlStato.SelectedValue)
            pLog.Add("IdEnte", TxtHdIdEnte.Text)
            pLog.Add("idEnteUtente", TxtHdIdDelegato.Text)
            'Dim myCommandSP As New System.Data.SqlClient.SqlCommand
            'myCommandSP.CommandText = "DelegatoInsertUpdate"
            'myCommandSP.CommandType = CommandType.StoredProcedure

            'myCommandSP.Connection = Session("conn")
            'myCommandSP.Parameters.Add("@IdDelegato", SqlDbType.Int)
            'myCommandSP.Parameters("@IdDelegato").Direction = ParameterDirection.InputOutput
            'myCommandSP.Parameters("@IdDelegato").Value = CInt(TxtHdIdDelegato.Text)
            'myCommandSP.Parameters.AddWithValue("@Cognome", txtCognome.Text)
            'myCommandSP.Parameters.AddWithValue("@Nome", txtNome.Text)
            'myCommandSP.Parameters.AddWithValue("@CodiceFiscale", txtCodiceFiscale.Text)
            'myCommandSP.Parameters.AddWithValue("@Stato", ddlStato.SelectedValue)
            'myCommandSP.Parameters.AddWithValue("@IDEnte", CInt(TxtHdIdEnte.Text))
            'myCommandSP.Parameters.AddWithValue("@TipoOperazione", TxtHdTipoOperazione.Text)
            'myCommandSP.Parameters.Add("@Esito", SqlDbType.TinyInt)
            'myCommandSP.Parameters("@Esito").Direction = ParameterDirection.Output
            'myCommandSP.Parameters.Add("@messaggio", SqlDbType.VarChar)
            'myCommandSP.Parameters("@messaggio").Size = 50
            'myCommandSP.Parameters("@messaggio").Direction = ParameterDirection.Output
            'myCommandSP.ExecuteNonQuery()
            'Dim esito = myCommandSP.Parameters("@Esito").Value

            Dim esito As Integer
            Dim messaggio As String
            esito = EseguiSp(messaggio) ' AZIONE 1 CANCELLA PROFILI
            lblTitoloPrincipale.Text = "Ricerca/Modifica utenze ente"
            If esito = 1 Then
                lblMessaggioConferma.Text = "Cancellazione utenza ente avvenuta correttamente"

                newDelegato.Id = CInt(TxtHdIdDelegato.Text)  'qui mettere l'id assegnata al delegato
                newDelegato.Name = "EntiUtenti"

                Log.Information(Logger.Data.LogEvent.DELEGATI_CANCELLAZIONE_CORRETTA, , pLog, newDelegato)
                'GestisciPnlInserimento(False)
                PulisciCampi()
                CmdAnnulla.Visible = True
                CmdElimina.Visible = False
                CmdSalva.Visible = True
                lblTitoloPrincipale.Text = "Ricerca/Modifica utenza ente"
                TxtHdIdDelegato.Text = "I"
                TxtHdIdDelegato.Text = ""
                dtgProfiliUtente.DataSource = Nothing
                dtgProfiliUtente.DataBind()
            Else
                'lblerrore.Text = myCommandSP.Parameters("@messaggio").Value
                lblerrore.Text = messaggio
                newDelegato.Id = CInt(TxtHdIdDelegato.Text) 'qui mettere l'id assegnata al delegato
                newDelegato.Name = "EntiUtenti" 'inserire il codicefiscale del delegato
                Log.Information(Logger.Data.LogEvent.DELEGATI_CANCELLAZIONE_ERRATA, , pLog, newDelegato)
                CmdSalva.Visible = False
            End If
            DivGriglia.Visible = True
            RicercaDelegatiEnte(Session("idEnte"), 1)
        Catch ex As Exception
            lblerrore.Text = "Errore Generico: " & ex.Message
            Log.Information(Logger.Data.LogEvent.DELEGATI_ERRORE_GENERICO, ex.Message)
        End Try
    End Sub

    Protected Sub CmdSalva_Click(sender As Object, e As EventArgs) Handles CmdSalva.Click
        Try

            Dim newDelegato As New Logger.Data.Entity
            Dim strSQL, RegPattern As String
            Dim cControllo, cNome As String


            pLog = New Hashtable()
            lblerrore.Text = ""
            lblMessaggioConferma.Text = ""
            'Carico Valori da passare come parameters al log
            pLog.Add("Cognome", txtCognome.Text)
            pLog.Add("Nome", txtNome.Text)
            pLog.Add("CF", txtCodiceFiscale.Text)
            pLog.Add("Stato", ddlStato.SelectedValue)
            pLog.Add("IdEnte", TxtHdIdEnte.Text)

            txtCodiceFiscale.Text = txtCodiceFiscale.Text.ToUpper
            If txtCognome.Text = "" Then lblerrore.Text += "Campo Cognome Obbligatorio</br>"
            If txtNome.Text = "" Then lblerrore.Text += "Campo Nome Obbligatorio</br>"
            If txtCodiceFiscale.Text = "" Then lblerrore.Text += "Campo Codice Fiscale Obbligatorio</br>"
            If txtCodiceFiscale.Text.Length <> 16 Then lblerrore.Text += "Il campo Codice Fiscale deve contenere 16 caratteri</br>"
            If VerificaCheck() = False Then
                lblerrore.Text += "Selezionare un Profilo</br>"
                Exit Sub
            End If
            'Controllo Codice Fiscale

            Dim NewCF As String = ""
            Dim pos As Integer
            'Sostituzione i caratteri in caso di omocodia per ricreare il cf base
            For p = txtCodiceFiscale.Text.Length To 1 Step -1
                If IsNumeric(Mid$(txtCodiceFiscale.Text, p, 1)) Then
                    pos = p
                    Exit For
                End If
            Next
            NewCF = Left(txtCodiceFiscale.Text, pos)
            ' For I = 1 To txtCodiceFiscale.Text.Length
            For I = pos + 1 To txtCodiceFiscale.Text.Length
                Select Case I
                    Case 7, 8, 10, 11, 13, 14, 15
                        NewCF += DecodificaOmocodici(Mid$(txtCodiceFiscale.Text, I, 1))
                    Case Else
                        NewCF += Mid$(txtCodiceFiscale.Text, I, 1)
                End Select
            Next

            '*******Controllo Formale del codice fiscale
            'Controllo che sia formalmente corretto
            RegPattern = "^[A-Za-z]{6}[0-9]{2}[A-Za-z]{1}[0-9]{2}[A-Za-z]{1}[0-9]{3}[A-Za-z]{1}$"
            If Regex.Match(NewCF, RegPattern).Success = False Then
                lblerrore.Text += "Il campo Codice Fiscale è formalmente errato</br>"
            Else ' se è formalmente corretto controllo i caratteri iniziali per cognome e nome

                cNome = Left(ClsUtility.CreaCF(txtCognome.Text, txtNome.Text, "01/01/2000", "H501", "M"), 6)
                If Left(NewCF, 6) <> cNome Then
                    lblerrore.Text += "Campo codice fiscale: Lettere non corrispondenti per Cognome e Nome</br>"
                Else 'se il controllo caratteri iniziali è corretto controllo l'ultimo carattere

                    cControllo = ClsUtility.GetCarattereControllo(Left(NewCF, 15))
                    If Right(NewCF, 1) <> cControllo Then
                        lblerrore.Text += "Campo Codice Fiscale: il codice controllo risulta errato</br>"
                    End If

                End If
            End If
            If ddlStato.SelectedValue = "" Then
                lblerrore.Text += "Campo Stato: Selezionare un'opzione</br>"
            End If

            If lblerrore.Text <> "" Then ' se ci sono errori esco
                lblerrore.Visible = True
                Exit Sub
            End If
            If TxtHdTipoOperazione.Text = "I" Then
                TxtHdIdDelegato.Text = 0
                TxtHdIdEnte.Text = Session("idEnte")

            End If
            'Dim myCommandSP As New System.Data.SqlClient.SqlCommand
            'myCommandSP.CommandText = "DelegatoInsertUpdate"
            'myCommandSP.CommandType = CommandType.StoredProcedure

            'myCommandSP.Connection = Session("conn")
            'myCommandSP.Parameters.Add("@IdDelegato", SqlDbType.Int)
            'myCommandSP.Parameters("@IdDelegato").Direction = ParameterDirection.InputOutput
            'myCommandSP.Parameters("@IdDelegato").Value = CInt(TxtHdIdDelegato.Text)
            'myCommandSP.Parameters.AddWithValue("@Cognome", txtCognome.Text)
            'myCommandSP.Parameters.AddWithValue("@Nome", txtNome.Text)
            'myCommandSP.Parameters.AddWithValue("@CodiceFiscale", txtCodiceFiscale.Text)
            'myCommandSP.Parameters.AddWithValue("@Stato", ddlStato.SelectedValue)
            'myCommandSP.Parameters.AddWithValue("@IDEnte", CInt(TxtHdIdEnte.Text))
            'myCommandSP.Parameters.AddWithValue("@TipoOperazione", TxtHdTipoOperazione.Text)
            'myCommandSP.Parameters.Add("@Esito", SqlDbType.TinyInt)
            'myCommandSP.Parameters("@Esito").Direction = ParameterDirection.Output
            'myCommandSP.Parameters.Add("@messaggio", SqlDbType.VarChar)
            'myCommandSP.Parameters("@messaggio").Size = 50
            'myCommandSP.Parameters("@messaggio").Direction = ParameterDirection.Output
            'myCommandSP.ExecuteNonQuery()


            'Dim esito = myCommandSP.Parameters("@Esito").Value
            'If TxtHdTipoOperazione.Text = "I" Then
            '    TxtHdIdDelegato.Text = myCommandSP.Parameters("@IdDelegato").Value

            'End If
            Dim esito As Integer
            Dim messaggio As String
            esito = EseguiSp(messaggio)
            pLog.Add("idEnteUtente", TxtHdIdDelegato.Text) 'aggiungo IdDelegato ai parametri per il log

            If esito = 1 Then
                If TxtHdTipoOperazione.Text = "I" Then
                    Dim Utenza As Integer
                    messaggio = ""
                    Utenza = CreaUtenzaDelegato(TxtHdIdDelegato.Text, messaggio)
                    If Utenza = 0 Then
                        lblerrore.Text = messaggio
                    End If
                    PulisciCampi()
                    lblMessaggioConferma.Text = "Inserimento Avvenuto Correttamente"
                    dtgProfiliUtente.Visible = False
                Else
                    PulisciCampi()
                    lblMessaggioConferma.Text = "Modifica Avvenuta Correttamente"
                    dtgProfiliUtente.Visible = False
                End If

                'If TxtHdTipoOperazione.Text = "I" Then
                '    TxtHdIdDelegato.Text = myCommandSP.Parameters("@IdDelegato").Value
                'End If
                newDelegato.Id = CInt(TxtHdIdDelegato.Text)  'qui mettere l'id assegnata al delegato
                newDelegato.Name = "EntiDelegati" 'inserire il codicefiscale del delegato
                If TxtHdTipoOperazione.Text = "I" Then
                    Log.Information(Logger.Data.LogEvent.DELEGATI_INSERIMENTO_CORRETTO, , pLog, newDelegato)
                Else
                    Log.Information(Logger.Data.LogEvent.DELEGATI_MODIFICA_CORRETTA, , pLog, newDelegato)
                End If
                TxtHdTipoOperazione.Text = "U"
                CmdElimina.Visible = True
                lblTitoloPrincipale.Text = "Ricerca/Modifica utenza ente"
            Else
                If TxtHdTipoOperazione.Text = "I" Then
                    TxtHdIdDelegato.Text = "0"
                End If
                'lblerrore.Text = myCommandSP.Parameters("@messaggio").Value
                lblerrore.Text = messaggio
                newDelegato.Id = CInt(TxtHdIdDelegato.Text) 'qui mettere l'id assegnata al delegato
                newDelegato.Name = "EntiDelegati" 'inserire il codicefiscale del delegato
                Dim TipoEvento As Integer

                If messaggio = "Codice Fiscale già presente per questo Ente" Then
                    TipoEvento = Logger.Data.LogEvent.DELEGATI_CF_GIA_PRESENTE
                Else
                    If TxtHdTipoOperazione.Text = "I" Then
                        TipoEvento = Logger.Data.LogEvent.DELEGATI_INSERIMENTO_ERRORE
                    Else
                        TipoEvento = Logger.Data.LogEvent.DELEGATI_MODIFICA_ERRATA
                    End If
                End If
                Log.Information(TipoEvento, , pLog, newDelegato)
            End If


            RicercaDelegatiEnte(Session("idEnte"), 1)
        Catch ex As Exception
            lblerrore.Text = "Errore Generico: " & ex.Message
            Log.Information(Logger.Data.LogEvent.DELEGATI_ERRORE_GENERICO, ex.Message)
        End Try
    End Sub

    Protected Sub CmdAnnulla_Click(sender As Object, e As EventArgs) Handles CmdAnnulla.Click
        GestisciPnlInserimento(False)
        lblTitoloPrincipale.Text = "Ricerca/Modifica utenza ente"
        dtgProfiliUtente.DataSource = Nothing
        dtgProfiliUtente.DataBind()

        PulisciCampi()
        DivGriglia.Visible = True
        RicercaDelegatiEnte(Session("idEnte"), 0)
    End Sub
    Private Shared Function DecodificaOmocodici(ByVal pValore) As String
        Dim TuttiGliOmocodici As String = "LMNPQRSTUV"

        If InStr(TuttiGliOmocodici, pValore) > 0 Then

            Select Case pValore
                Case Is = "L"
                    DecodificaOmocodici = "0"

                Case "M"
                    DecodificaOmocodici = "1"

                Case "N"
                    DecodificaOmocodici = "2"

                Case "P"
                    DecodificaOmocodici = "3"

                Case "Q"
                    DecodificaOmocodici = "4"

                Case "R"
                    DecodificaOmocodici = "5"

                Case "S"
                    DecodificaOmocodici = "6"

                Case "T"
                    DecodificaOmocodici = "7"

                Case "U"
                    DecodificaOmocodici = "8"

                Case "V"
                    DecodificaOmocodici = "9"

            End Select
        Else
            DecodificaOmocodici = pValore
        End If

    End Function
    Function EseguiSp(ByRef messaggio As String) As Integer
        Dim cmdinsert As Data.SqlClient.SqlCommand
        Dim myCommand As New System.Data.SqlClient.SqlCommand
        myCommand.Connection = Session("conn")
        Try
            Dim myCommandSP As New System.Data.SqlClient.SqlCommand
            myCommandSP.CommandText = "EnteUtenteInsertUpdate"
            myCommandSP.CommandType = CommandType.StoredProcedure

            myCommandSP.Connection = Session("conn")
            myCommandSP.Parameters.Add("@idEnteUtente", SqlDbType.Int)
            myCommandSP.Parameters("@idEnteUtente").Direction = ParameterDirection.InputOutput
            myCommandSP.Parameters("@idEnteUtente").Value = CInt(TxtHdIdDelegato.Text)
            myCommandSP.Parameters.AddWithValue("@Cognome", txtCognome.Text)
            myCommandSP.Parameters.AddWithValue("@Nome", txtNome.Text)
            myCommandSP.Parameters.AddWithValue("@CodiceFiscale", txtCodiceFiscale.Text)
            myCommandSP.Parameters.AddWithValue("@Stato", ddlStato.SelectedValue)
            myCommandSP.Parameters.AddWithValue("@IDEnte", CInt(TxtHdIdEnte.Text))
            myCommandSP.Parameters.AddWithValue("@TipoOperazione", TxtHdTipoOperazione.Text)
            myCommandSP.Parameters.Add("@Esito", SqlDbType.TinyInt)
            myCommandSP.Parameters("@Esito").Direction = ParameterDirection.Output
            myCommandSP.Parameters.Add("@messaggio", SqlDbType.VarChar)
            myCommandSP.Parameters("@messaggio").Size = 200
            myCommandSP.Parameters("@messaggio").Direction = ParameterDirection.Output
            myCommandSP.ExecuteNonQuery()
            If TxtHdTipoOperazione.Text = "I" Then
                TxtHdIdDelegato.Text = myCommandSP.Parameters("@idEnteUtente").Value

            End If
            messaggio = myCommandSP.Parameters("@messaggio").Value





            Return myCommandSP.Parameters("@Esito").Value
        Catch ex As Exception
            messaggio = "Errore Generico: " & ex.Message
            Return 0
        End Try
    End Function
    Function CreaUtenzaDelegato(idEnteUtente As Integer, ByRef messaggio As String) As Integer

        Dim cmdinsert As Data.SqlClient.SqlCommand
        Dim dtrLocal As SqlClient.SqlDataReader
        Dim myCommand As New System.Data.SqlClient.SqlCommand
        myCommand.Connection = Session("conn")
        Try
            Dim myCommandSP As New System.Data.SqlClient.SqlCommand
            myCommandSP.CommandText = "SP_ACCREDITAMENTO_CREA_UTENZA_ENTEUTENTE"
            myCommandSP.CommandType = CommandType.StoredProcedure

            '   @IdDelegato int		
            '@Esito tinyint output			-- Esito aggiornamento: 0-Errore 1-Aggiornamento effettuato
            '   @messaggio varchar(1000) output

            myCommandSP.Connection = Session("conn")
            myCommandSP.Parameters.AddWithValue("@idEnteUtente", idEnteUtente)
            myCommandSP.Parameters.Add("@Esito", SqlDbType.TinyInt)
            myCommandSP.Parameters("@Esito").Direction = ParameterDirection.Output
            myCommandSP.Parameters.Add("@messaggio", SqlDbType.VarChar)
            myCommandSP.Parameters("@messaggio").Size = 50
            myCommandSP.Parameters("@messaggio").Direction = ParameterDirection.Output
            myCommandSP.ExecuteNonQuery()
            messaggio = myCommandSP.Parameters("@messaggio").Value





            ''ADC --------------------------------------------------------------------------------------------------------------------
            'Dim stringa As String
            'Dim item As DataGridItem
            'Dim USER As String
            'stringa = "Select username from EntiPassword where idEnteUtente=" & idEnteUtente & " and idente=" & Session("idEnte") & ""

            'dtrLocal = ClsServer.CreaDatareader(stringa, Session("conn"))

            'dtrLocal.Read()

            'If dtrLocal.HasRows = True Then
            '    USER = dtrLocal("Username")

            '    If Not dtrLocal Is Nothing Then
            '        dtrLocal.Close()
            '        dtrLocal = Nothing
            '    End If
            'End If

            'If Not dtrLocal Is Nothing Then
            '    dtrLocal.Close()
            '    dtrLocal = Nothing
            'End If


            'stringa = "Delete from AssociaUtenteGruppo  WHERE username='" & USER & "'"

            'cmdinsert = New Data.SqlClient.SqlCommand(stringa, Session("conn"))
            'cmdinsert.ExecuteNonQuery()
            'cmdinsert.Dispose()
            'For Each item In dtgProfiliUtente.Items
            '    Dim check As CheckBox = DirectCast(item.FindControl("chkSeleziona"), CheckBox)
            '    If check.Checked = True Then
            '        stringa = "INSERT INTO AssociaUtenteGruppo (IdProfilo,UserName ) " & _
            '                   "VALUES(" & dtgProfiliUtente.Items(item.ItemIndex).Cells(0).Text & ", '" & USER & "')"

            '        cmdinsert = New Data.SqlClient.SqlCommand(stringa, Session("conn"))
            '        cmdinsert.ExecuteNonQuery()
            '        cmdinsert.Dispose()
            '    End If
            'Next
            ' FINE ADC--------------------------------------------------------------------------------------------------------------------
            Return myCommandSP.Parameters("@Esito").Value
        Catch ex As Exception
            'lblerrore.Text = myCommandSP.Parameters("@messaggio").Value
            messaggio = "Errore Generico: " & ex.Message
            Return 0
        End Try
    End Function

    Private Function VerificaCheck() As Boolean
        'controllo è  stata checcato almeno un settore per il salvataggio
        VerificaCheck = False
        Dim item As DataGridItem
        For Each item In dtgProfiliUtente.Items
            Dim check As CheckBox = DirectCast(item.FindControl("chkSeleziona"), CheckBox)
            If check.Checked = True Then
                VerificaCheck = True
            End If
        Next
        Return VerificaCheck
    End Function

    Protected Sub CmdChiudi_Click(sender As Object, e As EventArgs) Handles CmdChiudi.Click
        Response.Redirect("WfrmMain.aspx")
    End Sub
End Class