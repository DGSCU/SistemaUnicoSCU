Imports System.Drawing
Imports System.IO
Imports System.Data.SqlClient
Imports System.Text.RegularExpressions.Regex

Public Class WfrmGestioneDelegati
    Inherits SmartPage
    Dim dtsRisRicerca As DataSet
    Dim pLog As Hashtable

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

        'controllo se si tratta di primo caricamento della pagina
        If Page.IsPostBack = False Then

            'se è il primo accesso carico la griglia delle risorse con i criteri di ricerca
            RicercaDelegatiEnte(Session("idEnte"))
        End If

    End Sub
    Sub RicercaDelegatiEnte(ByVal IDEnte As Integer)
        'variabile stringa per la insert nella base dati
        Dim strSQL As String


        If Session("TipoUtente") <> "E" Then
            cmdNuovo.Visible = False
        End If

        Try

            'preparo la query per la ricerca dei delegati
            strSQL = "SELECT dbo.EntiDelegati.IDDelegato, dbo.EntiDelegati.IDEnte, dbo.EntiDelegati.CodiceFiscale, dbo.EntiDelegati.Cognome, dbo.EntiDelegati.Nome,"
            strSQL += " CASE WHEN dbo.EntiDelegati.Stato = 'True' THEN 'Abilitato' ELSE 'Disabilitato' END AS Stato, accessi.UltimoAccesso"
            strSQL += " FROM dbo.EntiDelegati LEFT OUTER JOIN"
            strSQL += " (select CodiceFiscale Name,"
            strSQL += " 	CONVERT(Varchar(10), MAX(Dataoraevento), 105) + ' ' + CONVERT(Varchar(10), MAX(Dataoraevento), 108) UltimoAccesso"
            strSQL += "  from "
            strSQL += " 	logaccessi L JOIN "
            strSQL += " 	EntiPassword P ON L.Username= P.Username JOIN"
            strSQL += " 	EntiDelegati D ON D.IDDelegato = P.IdDelegato"
            strSQL += " WHERE"
            strSQL += "     descrizione = 'LOGIN' "
            strSQL += " 	AND esito = 1"
            strSQL += " 	AND P.IDEnte=" & IDEnte
            strSQL += " GROUP BY "
            strSQL += " 	CodiceFiscale)"
            strSQL += "AS accessi ON dbo.EntiDelegati.CodiceFiscale = accessi.Name"
            strSQL += " where idEnte=" & IDEnte & " AND dbo.EntiDelegati.Visibile = 1"
            strSQL = strSQL & " order by Cognome"
            'fine preparazione query
            '************************
            'eseguo la query e assegno il dataset alla datagrid del risultato della ricerca 
            dtsRisRicerca = ClsServer.DataSetGenerico(strSQL, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
            dtgRisultatoRicerca.DataSource = dtsRisRicerca
            Session("appDtsRisRicerca") = dtsRisRicerca
            dtgRisultatoRicerca.DataBind()
            If (dtgRisultatoRicerca.Items.Count > 0) Then
                DivGriglia.Visible = True
            Else
                DivGriglia.Visible = False
            End If
        Catch ex As Exception
            Response.Write(ex.Message.ToString())
            Log.Information(Logger.Data.LogEvent.DELEGATI_ERRORE_GENERICO, ex.Message)
        End Try
    End Sub

    Protected Sub cmdNuovo_Click(sender As Object, e As EventArgs) Handles cmdNuovo.Click
        GestisciPnlInserimento(True)
        lblTitoloPrincipale.Text = "Inserimento Nuovo Delegato del Rappresentente Legale"
        lblerrore.Text = ""
        lblMessaggioConferma.Text = ""
        PulisciCampi()
    End Sub

    Protected Sub CmdAnnulla_Click(sender As Object, e As EventArgs) Handles CmdAnnulla.Click
        GestisciPnlInserimento(False)
        PulisciCampi()
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
            pLog.Add("IdDelegato", TxtHdIdDelegato.Text) 'aggiungo IdDelegato ai parametri per il log

            If esito = 1 Then
                If TxtHdTipoOperazione.Text = "I" Then
                    Dim Utenza As Integer
                    messaggio = ""
                    Utenza = CreaUtenzaDelegato(TxtHdIdDelegato.Text, messaggio)
                    If Utenza = 0 Then
                        lblerrore.Text = messaggio
                    End If
                    lblMessaggioConferma.Text = "Inserimento Avvenuto Correttamente"
                Else
                    lblMessaggioConferma.Text = "Modifica Avvenuta Correttamente"
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
                lblTitoloPrincipale.Text = "Visualizza/Modifica Abilitazione"
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
            RicercaDelegatiEnte(Session("idEnte"))
        Catch ex As Exception
            lblerrore.Text = "Errore Generico: " & ex.Message
            Log.Information(Logger.Data.LogEvent.DELEGATI_ERRORE_GENERICO, ex.Message)
        End Try

    End Sub

    Private Sub dtgRisultatoRicerca_PageIndexChanged(source As Object, e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dtgRisultatoRicerca.PageIndexChanged
         dtgRisultatoRicerca.CurrentPageIndex = e.NewPageIndex

        dtgRisultatoRicerca.DataSource = Session("appDtsRisRicerca")
        dtgRisultatoRicerca.DataBind()
    End Sub


    Private Sub dtgRisultatoRicerca_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles dtgRisultatoRicerca.SelectedIndexChanged
        Try
            GestisciPnlInserimento(True)
        lblTitoloPrincipale.Text = "Visualizza/Modifica Abilitazione"
        lblerrore.Text = ""
        lblMessaggioConferma.Text = ""
        TxtHdTipoOperazione.Text = "U"
        If Session("TipoUtente") = "E" Then
            CmdElimina.Visible = True
        End If

        Dim Strsql
        'preparo la query per la ricerca delle risorse acquisite e non con l'aggiunta dei filtri
        Strsql = "SELECT dbo.EntiDelegati.IDDelegato, dbo.EntiDelegati.IDEnte, dbo.EntiDelegati.CodiceFiscale, dbo.EntiDelegati.Cognome, dbo.EntiDelegati.Nome,"
        Strsql += " CASE WHEN dbo.EntiDelegati.Stato = 'True' THEN 'Abilitato' ELSE 'Disabilitato' END AS Stato, accessi.UltimoAccesso"
        Strsql += " FROM dbo.EntiDelegati LEFT OUTER JOIN"
        Strsql += " (SELECT CONVERT(Varchar(10), MAX(StartTime), 105) + ' ' + CONVERT(Varchar(10), MAX(StartTime), 108) AS UltimoAccesso, Username"
        Strsql += " FROM dbo.[Log] GROUP BY Username) AS accessi ON dbo.EntiDelegati.CodiceFiscale = accessi.Username"
        Strsql += " where idDelegato=" & CInt(dtgRisultatoRicerca.SelectedItem.Cells(1).Text)

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
        TxtHdIdDelegato.Text = tDelegato.Rows(0)("idDelegato")
        TxtHdIdEnte.Text = tDelegato.Rows(0)("idEnte")
        TxtHdTipoOperazione.Text = "U"

        If tDelegato.Rows(0)("Stato") = "Abilitato" Then
            ddlStato.SelectedIndex = 2
        Else
            ddlStato.SelectedIndex = 1
        End If
        If Session("TipoUtente") <> "E" Then
            txtCodiceFiscale.ReadOnly = True
            txtCognome.ReadOnly = True
            txtNome.ReadOnly = True
            ddlStato.Visible = False
            TxtStato.Text = tDelegato.Rows(0)("Stato")
            TxtStato.Visible = True

            End If
        Catch ex As Exception
            Log.Information(Logger.Data.LogEvent.DELEGATI_ERRORE_GENERICO, ex.Message)
        End Try
    End Sub
    Private Sub GestisciPnlInserimento(flVisible As Boolean)
        If Session("TipoUtente") = "E" Then
            If flVisible = True Then
                pnlInserimento.Visible = True
                cmdNuovo.Visible = False
                TxtHdTipoOperazione.Text = "I"
                CmdSalva.Visible = True
                CmdElimina.Visible = False
                CmdAnnulla.Visible = True
                ddlStato.SelectedIndex = -1
            Else
                pnlInserimento.Visible = False
                cmdNuovo.Visible = True
                TxtHdTipoOperazione.Text = ""
                CmdSalva.Visible = False
                CmdElimina.Visible = False
                CmdAnnulla.Visible = False
                ddlStato.SelectedIndex = -1
            End If
        Else
            If flVisible = True Then
                pnlInserimento.Visible = True
                cmdNuovo.Visible = False
                TxtHdTipoOperazione.Text = "I"
                CmdSalva.Visible = False
                CmdElimina.Visible = False
                CmdAnnulla.Visible = True
                ddlStato.SelectedIndex = -1
            Else
                pnlInserimento.Visible = False
                cmdNuovo.Visible = False
                TxtHdTipoOperazione.Text = ""
                CmdSalva.Visible = False
                CmdElimina.Visible = False
                CmdAnnulla.Visible = False
                ddlStato.SelectedIndex = -1
            End If
        End If

    End Sub
    Sub PulisciCampi()
        txtCodiceFiscale.Text = ""
        txtCognome.Text = ""
        TxtHdIdDelegato.Text = "0"
        TxtHdIdEnte.Text = Session("IdEnte")
        TxtHdTipoOperazione.Text = "I"
        txtNome.Text = ""
        ddlStato.SelectedIndex = -1

    End Sub

    Private Sub CmdElimina_Click(sender As Object, e As System.EventArgs) Handles CmdElimina.Click
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
            pLog.Add("IdDelegato", TxtHdIdDelegato.Text)
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
            esito = EseguiSp(messaggio)
            If esito = 1 Then
                lblMessaggioConferma.Text = "Cancellazione Delegato Avvenuta Correttamente"

                newDelegato.Id = CInt(TxtHdIdDelegato.Text)  'qui mettere l'id assegnata al delegato
                newDelegato.Name = "EntiDelegati"

                Log.Information(Logger.Data.LogEvent.DELEGATI_CANCELLAZIONE_CORRETTA, , pLog, newDelegato)
                'GestisciPnlInserimento(False)
                PulisciCampi()
                CmdAnnulla.Visible = True
                CmdElimina.Visible = False
                CmdSalva.Visible = True
                lblTitoloPrincipale.Text = "Inserimento Nuovo Delegato del Rappresentante Legale"
                TxtHdIdDelegato.Text = "I"
                TxtHdIdDelegato.Text = ""

            Else
                'lblerrore.Text = myCommandSP.Parameters("@messaggio").Value
                lblerrore.Text = messaggio
                newDelegato.Id = CInt(TxtHdIdDelegato.Text) 'qui mettere l'id assegnata al delegato
                newDelegato.Name = "EntiDelegati" 'inserire il codicefiscale del delegato
                Log.Information(Logger.Data.LogEvent.DELEGATI_CANCELLAZIONE_ERRATA, , pLog, newDelegato)
                CmdSalva.Visible = False
            End If
            RicercaDelegatiEnte(Session("idEnte"))
        Catch ex As Exception
            lblerrore.Text = "Errore Generico: " & ex.Message
            Log.Information(Logger.Data.LogEvent.DELEGATI_ERRORE_GENERICO, ex.Message)
        End Try
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
        Try
            Dim myCommandSP As New System.Data.SqlClient.SqlCommand
            myCommandSP.CommandText = "DelegatoInsertUpdate"
            myCommandSP.CommandType = CommandType.StoredProcedure

            myCommandSP.Connection = Session("conn")
            myCommandSP.Parameters.Add("@IdDelegato", SqlDbType.Int)
            myCommandSP.Parameters("@IdDelegato").Direction = ParameterDirection.InputOutput
            myCommandSP.Parameters("@IdDelegato").Value = CInt(TxtHdIdDelegato.Text)
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
                TxtHdIdDelegato.Text = myCommandSP.Parameters("@IdDelegato").Value

            End If
            messaggio = myCommandSP.Parameters("@messaggio").Value
            Return myCommandSP.Parameters("@Esito").value
        Catch ex As Exception
            messaggio = "Errore Generico: " & ex.Message
            Return 0
        End Try
    End Function
    Function CreaUtenzaDelegato(idDelegato As Integer, ByRef messaggio As String) As Integer
        Try
            Dim myCommandSP As New System.Data.SqlClient.SqlCommand
            myCommandSP.CommandText = "SP_ACCREDITAMENTO_CREA_UTENZA_DELEGATO"
            myCommandSP.CommandType = CommandType.StoredProcedure

            '   @IdDelegato int		
            '@Esito tinyint output			-- Esito aggiornamento: 0-Errore 1-Aggiornamento effettuato
            '   @messaggio varchar(1000) output

            myCommandSP.Connection = Session("conn")
            myCommandSP.Parameters.AddWithValue("@IdDelegato", idDelegato)
            myCommandSP.Parameters.Add("@Esito", SqlDbType.TinyInt)
            myCommandSP.Parameters("@Esito").Direction = ParameterDirection.Output
            myCommandSP.Parameters.Add("@messaggio", SqlDbType.VarChar)
            myCommandSP.Parameters("@messaggio").Size = 50
            myCommandSP.Parameters("@messaggio").Direction = ParameterDirection.Output
            myCommandSP.ExecuteNonQuery()
            messaggio = myCommandSP.Parameters("@messaggio").Value
            Return myCommandSP.Parameters("@Esito").Value

        Catch ex As Exception
            'lblerrore.Text = myCommandSP.Parameters("@messaggio").Value
            messaggio = "Errore Generico: " & ex.Message
            Return 0
        End Try
    End Function


End Class