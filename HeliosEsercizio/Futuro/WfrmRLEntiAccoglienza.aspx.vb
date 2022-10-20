Imports System.Drawing
Imports System.IO
Imports System.Data.SqlClient
Imports System.Text.RegularExpressions.Regex
Public Class WfrmRLEntiAccoglienza
    Inherits SmartPage
    Dim dtsRisRicerca As DataSet
    Dim pLog As Hashtable
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        checkSpid()
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
            RicercaRlEnteAccoglienza(Session("idEnte"))
        End If

    End Sub
    Sub RicercaRlEnteAccoglienza(ByVal IdEnte As Integer)



        Dim strSQL As String


        If Session("TipoUtente") <> "E" Then
            cmdNuovo.Visible = False
        End If

        Try

            'preparo la query per la ricerca delle abilitazioni inserite
            'strSQL = "SELECT IDRLEntiAccoglienza, IdEnte, CodiceFiscaleEnte, SedeLegaleEstera, RLCognome, RLNome, RLCodiceFiscale,"
            'strSQL = "CASE WHEN RLRegistrato = 'True' THEN 'Si' ELSE 'No' END AS RLRegistrato"
            'strSQL += " FROM RLEntiAccoglienza"
            'strSQL += " where idEnte=" & IdEnte & " And Visibile = 1"

            strSQL = "SELECT a.IDRLEntiAccoglienza, a.IdEnteTitolare, a.CodiceFiscaleEnte, a.SedeLegaleEstera, a.RLCognome, a.RLNome, a.RLCodiceFiscale,"
            strSQL += " CASE WHEN b.CF_Persona IS NULL THEN 'No' ELSE 'Si' END AS RLRegistrato"
            strSQL += " FROM dbo.RLEntiAccoglienza AS a LEFT OUTER JOIN"
            'strSQL += " dbo.VW_REGISTRAZIONE_UTENZE AS b ON a.CodiceFiscaleEnte = b.CF_Ente AND a.RLCodiceFiscale = b.CF_Persona"
            strSQL += " (Select distinct CF_Persona,CF_Ente FROM dbo.VW_REGISTRAZIONE_UTENZE) AS b ON a.CodiceFiscaleEnte = b.CF_Ente AND a.RLCodiceFiscale = b.CF_Persona"
            strSQL += " where idEnteTitolare=" & IdEnte & " And Visibile = 1"







            'fine preparazione query
            '************************
            'eseguo la query e assegno il dataset alla datagrid del risultato della ricerca 
            dtsRisRicerca = ClsServer.DataSetGenerico(strSQL, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
            dtgRisultatoRicerca.DataSource = dtsRisRicerca
            dtgRisultatoRicerca.DataBind()
            If (dtgRisultatoRicerca.Items.Count > 0) Then
                DivGriglia.Visible = True
            Else
                DivGriglia.Visible = False
            End If



        Catch ex As Exception
            Response.Write(ex.Message.ToString())
            Log.Information(Logger.Data.LogEvent.RLENTIACCOGLIENZA_ERRORE_GENERICO, ex.Message)
        End Try
        'inserire il log qui
    End Sub

    Private Sub dtgRisultatoRicerca_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgRisultatoRicerca.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.SelectedItem) Then
            Dim chk As CheckBox = DirectCast(e.Item.FindControl("chkEsteraGrid"), CheckBox)
            chk.Enabled = True
            If e.Item.DataItem("SedeLegaleEstera") Then
                chk.Checked = True
            Else
                chk.Checked = False
            End If
            If e.Item.DataItem("RLRegistrato").ToString = "Si" Then
                For intConta = 0 To 8
                    e.Item.Cells(intConta).BackColor = Color.LightGreen
                Next
            End If
            chk.Enabled = False
        End If

    End Sub
    Private Sub dtgRisultatoRicerca_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles dtgRisultatoRicerca.SelectedIndexChanged
        GestisciPnlInserimento(True)
        lblTitoloPrincipale.Text = "Abilitazione Rappresentante Legale Ente di Accoglienza"
        lblerrore.Text = ""
        lblMessaggioConferma.Text = ""
        TxtHdTipoOperazione.Text = "U"
        If Session("TipoUtente") = "E" Then
            CmdElimina.Visible = True
        End If

        Dim Strsql

        Strsql = "SELECT a.IDRLEntiAccoglienza, a.IdEnteTitolare, a.CodiceFiscaleEnte, a.SedeLegaleEstera, a.RLCognome, a.RLNome, a.RLCodiceFiscale,"
        Strsql += " CASE WHEN b.CF_Persona IS NULL THEN 'No' ELSE 'Si' END AS RLRegistrato"
        Strsql += " FROM dbo.RLEntiAccoglienza AS a LEFT OUTER JOIN"
        Strsql += " dbo.VW_REGISTRAZIONE_UTENZE AS b ON a.CodiceFiscaleEnte = b.CF_Ente AND a.RLCodiceFiscale = b.CF_Persona"
        Strsql += " where a.IDRLEntiAccoglienza =" & CInt(dtgRisultatoRicerca.SelectedItem.Cells(1).Text)




        'fine preparazione query
        '************************
        Dim SqlCmd As New SqlCommand
        Dim dataAdapter As SqlDataAdapter = New SqlDataAdapter
        Dim tRLEntiAccoglienza = New DataTable


        SqlCmd.CommandText = Strsql
        SqlCmd.CommandType = CommandType.Text
        SqlCmd.Connection = Session("conn")
        dataAdapter.SelectCommand = SqlCmd
        dataAdapter.Fill(tRLEntiAccoglienza)

        txtCognome.Text = tRLEntiAccoglienza.Rows(0)("RLCognome")
        txtNome.Text = tRLEntiAccoglienza.Rows(0)("RLNome")
        txtCodiceFiscale.Text = tRLEntiAccoglienza.Rows(0)("RLCodiceFiscale")
        txtCodFiscaleEnte.Text = tRLEntiAccoglienza.Rows(0)("CodiceFiscaleEnte")
        If tRLEntiAccoglienza.Rows(0)("SedeLegaleEstera") Then
            chkEstero.Checked = True
        Else
            chkEstero.Checked = False

        End If
        TxtHdIdRlAccoglienza.Text = tRLEntiAccoglienza.Rows(0)("IDRLEntiAccoglienza")
        TxtHdIdEnte.Text = tRLEntiAccoglienza.Rows(0)("idEnteTitolare")
        TxtHdTipoOperazione.Text = "U"

        If Session("TipoUtente") <> "E" Then
            txtCodiceFiscale.ReadOnly = True
            txtCognome.ReadOnly = True
            txtNome.ReadOnly = True
            chkEstero.Enabled = False
            chkEstero.EnableTheming = False
            txtCodFiscaleEnte.ReadOnly = True

        End If
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
                chkEstero.Checked = False
                CmdTorna.Visible = False
            Else
                pnlInserimento.Visible = False
                cmdNuovo.Visible = True
                TxtHdTipoOperazione.Text = ""
                CmdSalva.Visible = False
                CmdElimina.Visible = False
                CmdAnnulla.Visible = False
                chkEstero.Checked = False
                CmdTorna.Visible = True
            End If
        Else
            If flVisible = True Then
                pnlInserimento.Visible = True
                cmdNuovo.Visible = False
                TxtHdTipoOperazione.Text = "I"
                CmdSalva.Visible = False
                CmdElimina.Visible = False
                CmdAnnulla.Visible = True
                chkEstero.Checked = False
                CmdTorna.Visible = False
            Else
                pnlInserimento.Visible = False
                cmdNuovo.Visible = False
                TxtHdTipoOperazione.Text = ""
                CmdSalva.Visible = False
                CmdElimina.Visible = False
                CmdAnnulla.Visible = False
                chkEstero.Checked = False
                CmdTorna.Visible = True
            End If
        End If

    End Sub
    Sub PulisciCampi()
        txtCodiceFiscale.Text = ""
        txtCognome.Text = ""
        TxtHdIdRlAccoglienza.Text = "0"
        TxtHdIdEnte.Text = Session("IdEnte")
        TxtHdTipoOperazione.Text = "I"
        txtNome.Text = ""
        txtCodFiscaleEnte.Text = ""
        chkEstero.Checked = False
    End Sub

    Private Sub CmdSalva_Click(sender As Object, e As System.EventArgs) Handles CmdSalva.Click
        Dim newRLSdAcc As New Logger.Data.Entity
        Dim strSQL, RegPattern As String
        Dim cControllo, cNome As String
        Try


            pLog = New Hashtable()
            lblerrore.Text = ""
            lblMessaggioConferma.Text = ""
            'Carico Valori da passare come parameters al log
            pLog.Add("IdEnteTitolare", TxtHdIdEnte.Text)
            pLog.Add("CodFis_Ente", txtCodFiscaleEnte.Text)

            If chkEstero.Checked Then
                pLog.Add("SedeLegaleEstera", "1")
            Else
                pLog.Add("SedeLegaleEstera", "0")
            End If


            pLog.Add("Cognome", txtCognome.Text)
            pLog.Add("Nome", txtNome.Text)
            pLog.Add("CodFis_RL", txtCodiceFiscale.Text)

            txtCodFiscaleEnte.Text = txtCodFiscaleEnte.Text.ToUpper

            txtCodiceFiscale.Text = txtCodiceFiscale.Text.ToUpper
            If txtCodFiscaleEnte.Text = "" Then lblerrore.Text += "Campo Codice Fiscale Ente Obbligatorio</br>"
            If chkEstero.Checked = False Then
                If ClsUtility.CheckPartitaIVA(txtCodFiscaleEnte.Text) = False Then lblerrore.Text += "Campo Codice Fiscale Ente formalmente errato</br>"
                If txtCodFiscaleEnte.Text.Length <> 11 Then lblerrore.Text += "Il campo Codice Fiscale Ente deve contenere 11 caratteri</br>"
            End If

            If txtCognome.Text = "" Then lblerrore.Text += "Campo Cognome Rappresentante Legale Obbligatorio</br>"
            If txtNome.Text = "" Then lblerrore.Text += "Campo Nome Rappresentante Legale Obbligatorio</br>"
            If txtCodiceFiscale.Text = "" Then lblerrore.Text += "Campo Codice Fiscale Rappresentante Legale Obbligatorio</br>"
            If txtCodiceFiscale.Text.Length <> 16 Then lblerrore.Text += "Il campo Codice Fiscale Rappresentante Legale deve contenere 16 caratteri</br>"

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
                lblerrore.Text += "Il campo Codice Fiscale del Rappresentante Legale è formalmente errato</br>"
            Else ' se è formalmente corretto controllo i caratteri iniziali per cognome e nome

                cNome = Left(ClsUtility.CreaCF(txtCognome.Text, txtNome.Text, "01/01/2000", "H501", "M"), 6)
                If Left(NewCF, 6) <> cNome Then
                    lblerrore.Text += "Campo codice fiscale del Rappresentante Legale: Lettere non corrispondenti per Cognome e Nome</br>"
                Else 'se il controllo caratteri iniziali è corretto controllo l'ultimo carattere

                    cControllo = ClsUtility.GetCarattereControllo(Left(NewCF, 15))
                    If Right(NewCF, 1) <> cControllo Then
                        lblerrore.Text += "Campo Codice Fiscale del Rappresentante Legale: il codice controllo risulta errato</br>"
                    End If

                End If
            End If

            If lblerrore.Text <> "" Then ' se ci sono errori esco
                lblerrore.Visible = True
                Exit Sub
            End If
            If TxtHdTipoOperazione.Text = "I" Then
                TxtHdIdRlAccoglienza.Text = 0
                TxtHdIdEnte.Text = Session("idEnte")
            End If
            'Dim myCommandSP As New System.Data.SqlClient.SqlCommand
            'myCommandSP.CommandText = "RLEntiAccoglienza_InsertUpdate"
            'myCommandSP.CommandType = CommandType.StoredProcedure

            'myCommandSP.Connection = Session("conn")
            'myCommandSP.Parameters.Add("@IdRLEntiAccoglienza", SqlDbType.Int)
            'myCommandSP.Parameters("@IdRLEntiAccoglienza").Direction = ParameterDirection.InputOutput
            'myCommandSP.Parameters("@IdRLEntiAccoglienza").Value = CInt(TxtHdIdRlAccoglienza.Text)
            'myCommandSP.Parameters.AddWithValue("@CodiceFiscaleEnte", txtCodFiscaleEnte.Text)
            'myCommandSP.Parameters.AddWithValue("@RLCognome", txtCognome.Text)
            'myCommandSP.Parameters.AddWithValue("@RLNome", txtNome.Text)
            'myCommandSP.Parameters.AddWithValue("@RLCodiceFiscale", txtCodiceFiscale.Text)
            'myCommandSP.Parameters.AddWithValue("@SedeLegaleEstera", chkEstero.Checked)
            'myCommandSP.Parameters.AddWithValue("@IDEnte", CInt(TxtHdIdEnte.Text))
            'myCommandSP.Parameters.AddWithValue("@TipoOperazione", TxtHdTipoOperazione.Text)
            'myCommandSP.Parameters.Add("@Esito", SqlDbType.TinyInt)
            'myCommandSP.Parameters("@Esito").Direction = ParameterDirection.Output
            'myCommandSP.Parameters.Add("@messaggio", SqlDbType.VarChar)
            'myCommandSP.Parameters("@messaggio").Size = 100
            'myCommandSP.Parameters("@messaggio").Direction = ParameterDirection.Output

            ''PreparaCmdSql(myCommandSP)
            'myCommandSP.ExecuteNonQuery()


            'Dim esito = myCommandSP.Parameters("@Esito").Value

            'If TxtHdTipoOperazione.Text = "I" Then
            '    TxtHdIdRlAccoglienza.Text = myCommandSP.Parameters("@IDRLEntiAccoglienza").Value

            'End If
            Dim esito As Integer
            Dim messaggio As String
            esito = EseguiSp(messaggio)

            pLog.Add("IDRLEntiAccoglienza", TxtHdIdRlAccoglienza.Text) 'aggiungo IdRlAccoglienza ai parametri per il log

            If esito = 1 Then
                If TxtHdTipoOperazione.Text = "I" Then
                    lblMessaggioConferma.Text = "Inserimento Avvenuto Correttamente"
                Else
                    lblMessaggioConferma.Text = "Modifica Avvenuta Correttamente"
                End If

                'If TxtHdTipoOperazione.Text = "I" Then
                '    TxtHdIdRlAccoglienza.Text = myCommandSP.Parameters("@IdRLEntiAccoglienza").Value
                'End If
                newRLSdAcc.Id = CInt(TxtHdIdRlAccoglienza.Text)
                newRLSdAcc.Name = "RLEntiAccoglienza"
                If TxtHdTipoOperazione.Text = "I" Then
                    Log.Information(Logger.Data.LogEvent.RLENTIACCOGLIENZA_INSERIMENTO_CORRETTO, , pLog, newRLSdAcc)
                Else
                    Log.Information(Logger.Data.LogEvent.RLENTIACCOGLIENZA_MODIFICA_CORRETTA, , pLog, newRLSdAcc)
                End If
                TxtHdTipoOperazione.Text = "U"
                CmdElimina.Visible = True
                lblTitoloPrincipale.Text = "Visualizza/Modifica Ente di Accoglienza"
            Else
                If TxtHdTipoOperazione.Text = "I" Then
                    TxtHdIdRlAccoglienza.Text = "0"
                End If
                'lblerrore.Text = myCommandSP.Parameters("@messaggio").Value
                lblerrore.Text = messaggio
                newRLSdAcc.Id = CInt(TxtHdIdRlAccoglienza.Text)
                newRLSdAcc.Name = "RLEntiAccoglienza"
                Dim TipoEvento As Integer

                If messaggio = "Codice Fiscale già presente per questo Ente" Then
                    TipoEvento = Logger.Data.LogEvent.RLENTIACCOGLIENZA_CF_GIA_PRESENTE
                Else
                    If TxtHdTipoOperazione.Text = "I" Then
                        TipoEvento = Logger.Data.LogEvent.RLENTIACCOGLIENZA_INSERIMENTO_ERRORE
                    Else
                        TipoEvento = Logger.Data.LogEvent.RLENTIACCOGLIENZA_MODIFICA_ERRATA
                    End If
                End If
                Log.Information(TipoEvento, , pLog, newRLSdAcc)
            End If
            RicercaRlEnteAccoglienza(Session("idEnte"))
        Catch ex As Exception
            lblerrore.Text = "Errore Generico: " & ex.Message
            Log.Information(Logger.Data.LogEvent.RLENTIACCOGLIENZA_ERRORE_GENERICO, ex.Message)
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

    Protected Sub cmdNuovo_Click(sender As Object, e As EventArgs) Handles cmdNuovo.Click
        GestisciPnlInserimento(True)
        lblTitoloPrincipale.Text = "Inserimento Nuova Abilitazione"
        lblerrore.Text = ""
        lblMessaggioConferma.Text = ""
        PulisciCampi()
    End Sub


    Protected Sub CmdElimina_Click(sender As Object, e As EventArgs) Handles CmdElimina.Click
        Try
            Dim newRLSdAcc As New Logger.Data.Entity
            lblerrore.Text = ""
            lblMessaggioConferma.Text = ""
            TxtHdTipoOperazione.Text = "C"
            pLog = New Hashtable()
            'Carico Valori da passare come parameters al log
            pLog.Add("IdEnteTitolare", TxtHdIdEnte.Text)
            pLog.Add("CodFis_Ente", txtCodFiscaleEnte.Text)
            If chkEstero.Checked Then
                pLog.Add("SedeLegaleEstera", "1")
            Else
                pLog.Add("SedeLegaleEstera", "0")
            End If
            pLog.Add("Cognome", txtCognome.Text)
            pLog.Add("Nome", txtNome.Text)
            pLog.Add("CodFisF_RL", txtCodiceFiscale.Text)
            pLog.Add("IdRlEntiAccoglienza", TxtHdIdRlAccoglienza.Text)
            'Dim myCommandSP As New System.Data.SqlClient.SqlCommand
            'myCommandSP.CommandText = "RLEntiAccoglienza_InsertUpdate"
            'myCommandSP.CommandType = CommandType.StoredProcedure

            'myCommandSP.Connection = Session("conn")
            'myCommandSP.Parameters.Add("@IdRLEntiAccoglienza", SqlDbType.Int)
            'myCommandSP.Parameters("@IdRLEntiAccoglienza").Direction = ParameterDirection.InputOutput
            'myCommandSP.Parameters("@IdRLEntiAccoglienza").Value = CInt(TxtHdIdRlAccoglienza.Text)
            'myCommandSP.Parameters.AddWithValue("@CodiceFiscaleEnte", txtCodFiscaleEnte.Text)
            'myCommandSP.Parameters.AddWithValue("@RLCognome", txtCognome.Text)
            'myCommandSP.Parameters.AddWithValue("@RLNome", txtNome.Text)
            'myCommandSP.Parameters.AddWithValue("@RLCodiceFiscale", txtCodiceFiscale.Text)
            'myCommandSP.Parameters.AddWithValue("@SedeLegaleEstera", chkEstero.Checked)
            'myCommandSP.Parameters.AddWithValue("@IDEnte", CInt(TxtHdIdEnte.Text))
            'myCommandSP.Parameters.AddWithValue("@TipoOperazione", TxtHdTipoOperazione.Text)
            'myCommandSP.Parameters.Add("@Esito", SqlDbType.TinyInt)
            'myCommandSP.Parameters("@Esito").Direction = ParameterDirection.Output
            'myCommandSP.Parameters.Add("@messaggio", SqlDbType.VarChar)
            'myCommandSP.Parameters("@messaggio").Size = 100
            'myCommandSP.Parameters("@messaggio").Direction = ParameterDirection.Output


            ' ''PreparaCmdSql(myCommandSP)

            'myCommandSP.ExecuteNonQuery()
            'Dim esito = myCommandSP.Parameters("@Esito").Value
            Dim esito As Integer
            Dim messaggio As String
            esito = EseguiSp(messaggio)
            If esito = 1 Then
                lblMessaggioConferma.Text = "Cancellazione Abilitazione avvenuta Correttamente"



                newRLSdAcc.Id = CInt(TxtHdIdRlAccoglienza.Text)  'qui mettere l'id assegnata al delegato
                newRLSdAcc.Name = "RLEntiAccoglienza"

                Log.Information(Logger.Data.LogEvent.RLENTIACCOGLIENZA_CANCELLAZIONE_CORRETTA, , pLog, newRLSdAcc)
                'GestisciPnlInserimento(False)
                PulisciCampi()
                CmdAnnulla.Visible = True
                CmdElimina.Visible = False
                CmdSalva.Visible = True
                TxtHdIdEnte.Text = "I"
                TxtHdIdRlAccoglienza.Text = ""
                lblTitoloPrincipale.Text = "Inserimento Nuova Abilitazione"
            Else
                'lblerrore.Text = myCommandSP.Parameters("@messaggio").Value
                lblerrore.Text = messaggio
                newRLSdAcc.Id = CInt(TxtHdIdRlAccoglienza.Text)
                newRLSdAcc.Name = "RLEntiAccoglienza"
                Log.Information(Logger.Data.LogEvent.RLENTIACCOGLIENZA_CANCELLAZIONE_ERRATA, , pLog, newRLSdAcc)
                CmdSalva.Visible = False
            End If
            RicercaRlEnteAccoglienza(Session("idEnte"))

        Catch ex As Exception
            lblerrore.Text = "Errore Generico: " & ex.Message
            Log.Information(Logger.Data.LogEvent.RLENTIACCOGLIENZA_ERRORE_GENERICO, ex.Message)
        End Try

    End Sub

    Protected Sub CmdAnnulla_Click(sender As Object, e As EventArgs) Handles CmdAnnulla.Click
        GestisciPnlInserimento(False)
        PulisciCampi()
    End Sub

    Protected Sub CmdTorna_Click(sender As Object, e As EventArgs) Handles CmdTorna.Click
        Response.Redirect("DettaglioFunzioni.aspx?IdVoceMenu=2")
    End Sub

    Function EseguiSp(ByRef Messaggio As String) As Integer
        Try


            Dim myCommandSP As New System.Data.SqlClient.SqlCommand
            myCommandSP.CommandText = "RLEntiAccoglienza_InsertUpdate"
            myCommandSP.CommandType = CommandType.StoredProcedure

            myCommandSP.Connection = Session("conn")
            myCommandSP.Parameters.Add("@IdRLEntiAccoglienza", SqlDbType.Int)
            myCommandSP.Parameters("@IdRLEntiAccoglienza").Direction = ParameterDirection.InputOutput
            myCommandSP.Parameters("@IdRLEntiAccoglienza").Value = CInt(TxtHdIdRlAccoglienza.Text)
            myCommandSP.Parameters.AddWithValue("@CodiceFiscaleEnte", txtCodFiscaleEnte.Text)
            myCommandSP.Parameters.AddWithValue("@RLCognome", txtCognome.Text)
            myCommandSP.Parameters.AddWithValue("@RLNome", txtNome.Text)
            myCommandSP.Parameters.AddWithValue("@RLCodiceFiscale", txtCodiceFiscale.Text)
            myCommandSP.Parameters.AddWithValue("@SedeLegaleEstera", chkEstero.Checked)
            myCommandSP.Parameters.AddWithValue("@IDEnteTitolare", CInt(TxtHdIdEnte.Text))
            myCommandSP.Parameters.AddWithValue("@TipoOperazione", TxtHdTipoOperazione.Text)
            myCommandSP.Parameters.Add("@Esito", SqlDbType.TinyInt)
            myCommandSP.Parameters("@Esito").Direction = ParameterDirection.Output
            myCommandSP.Parameters.Add("@messaggio", SqlDbType.VarChar)
            myCommandSP.Parameters("@messaggio").Size = 100
            myCommandSP.Parameters("@messaggio").Direction = ParameterDirection.Output
            myCommandSP.ExecuteNonQuery()

            If TxtHdTipoOperazione.Text = "I" Then
                TxtHdIdRlAccoglienza.Text = myCommandSP.Parameters("@IDRLEntiAccoglienza").Value

            End If

            Messaggio = myCommandSP.Parameters("@messaggio").Value
            Return myCommandSP.Parameters("@Esito").Value
        Catch ex As Exception
            Messaggio = "Errore Generico: " & ex.Message
            Return 0
        End Try



    End Function
End Class