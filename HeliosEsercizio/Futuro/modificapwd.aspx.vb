Public Class modificapwd
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Inserire qui il codice utente necessario per inizializzare la pagina
        'controllo se effettuato login
        Dim tipodiutenza As String
        If Not Session("LogIn") Is Nothing Then
            If Session("LogIn") = False Then 'verifico validità log-in
                Response.Redirect("LogOn.aspx")
            End If
        Else
            Response.Redirect("LogOn.aspx")
        End If
        tipodiutenza = Mid(Session("Utente"), 1, 1)
        If tipodiutenza = "R" Or tipodiutenza = "E" Then



        Else
            lblmessaggiosopra.Visible = True
            lblmessaggiosopra.Text = "Utenza non abilitata alla modifica della Password."
            imgAlert.Visible = True
            txtVecchiaPwd.Enabled = False
            txtNuovaPwd.Enabled = False
            txtConfermaNuovaPwd.Enabled = False
            CmdSalva.Visible = False
        End If
        If Request.QueryString("ModificaObbligatoria") = "True" Then
            imgChiudi.Visible = False
            'DynamicMenu1.Visible = False
        End If
        'Response.ExpiresAbsolute = #1/1/1980#
        'Response.AddHeader("Pragma", "no-cache")
    End Sub
    Function CreatePSW(ByVal cPsw As String) As String
        ' Funzione per la creazione della stringa criptata
        ' Ritorna la password criptata
        Dim KeyAscii As Integer
        Dim cNewPsw As String
        Dim i As Integer
        If Len(cPsw) = 0 Then
            CreatePSW = ""
            Exit Function
        End If
        KeyAscii = Asc(Left(cPsw, 1)) - 30
        If KeyAscii < 0 Then
            KeyAscii = 255 - Math.Abs(KeyAscii)
        End If
        cNewPsw = Chr(KeyAscii)
        For i = 2 To Len(cPsw)
            KeyAscii = Asc(Mid(cPsw, i, 1)) - Asc(Mid(cPsw, i - 1, 1)) + 128
            If KeyAscii < 0 Then
                KeyAscii = 255 - Math.Abs(KeyAscii)
            End If
            cNewPsw = cNewPsw + Chr(KeyAscii)
        Next
        CreatePSW = cNewPsw
    End Function
    Function ReadPsw(ByVal cPsw As String) As String
        'Funzione per la lettura della password
        'Ritorna la password decriptata
        Dim cNewPsw As String
        Dim KeyAscii As Integer
        Dim i As Integer
        KeyAscii = Asc(Mid(cPsw, 1, 1)) + 30
        If KeyAscii > 255 Then
            KeyAscii = Math.Abs(KeyAscii) - 255
        End If
        cNewPsw = Chr(KeyAscii)
        For i = 2 To Len(cPsw)
            KeyAscii = Asc(Mid(cPsw, i, 1)) + Asc(Mid(cNewPsw, i - 1, 1)) - 128
            If KeyAscii > 255 Then
                KeyAscii = Math.Abs(KeyAscii) - 255
            End If
            cNewPsw = cNewPsw + Chr(KeyAscii)
        Next
        ReadPsw = cNewPsw
    End Function

    Function pswChrAllowed(ByVal KeyAscii) As String
        If (KeyAscii < 32 Or KeyAscii > 122 Or KeyAscii = 38 Or KeyAscii = 39) And KeyAscii <> 8 Then
            KeyAscii = 0
        End If
        pswChrAllowed = KeyAscii
    End Function
    Private Sub imgChiudi_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles imgChiudi.Click
        If Request.QueryString("operazione") = "login" Then
            Response.Redirect("LogOn.aspx")
        Else
            Response.Redirect("WfrmMain.aspx")
        End If
    End Sub

    Private Sub CmdSalva_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles CmdSalva.Click
        Dim strSql As String
        Dim rstPw As SqlClient.SqlDataReader

        strSql = "Select * from SediPassword WHERE Username ='" & Session("Utente") & "'"
        rstPw = ClsServer.CreaDatareader(strSql, Session("conn"))
        If rstPw.HasRows = True Then 'UTENZA SEDE
            If Not rstPw Is Nothing Then
                rstPw.Close()
                rstPw = Nothing
            End If
            GestionePasswordSedi()
        Else
            If Not rstPw Is Nothing Then 'UTENZA ENTE
                rstPw.Close()
                rstPw = Nothing
            End If
            GestionePasswordEnti()
        End If
    End Sub

    Private Function verificaErroriValidazionePassword() As Boolean
        Dim errore As Boolean = False
        Dim messaggioDiErrore As String = ""

        If (txtNuovaPwd.Text = String.Empty Or txtConfermaNuovaPwd.Text = String.Empty Or txtVecchiaPwd.Text = String.Empty) Then
            messaggioDiErrore = "Attenzione. I campi Vecchia password, Nuova password e Conferma nuova password devono essere valorizzati."
            errore = True
        ElseIf (txtNuovaPwd.Text = txtVecchiaPwd.Text) Then
            messaggioDiErrore = "Attenzione. La nuova password deve essere diversa dalla vecchia."
            errore = True
        ElseIf (txtNuovaPwd.Text.Length < 8) Then
            messaggioDiErrore = "Attenzione. La lunghezza minima della password è di 8 caratteri"
            errore = True
        ElseIf (verificaComplessitaPasswordNonRispettata() = True) Then
            messaggioDiErrore = "Attenzione. La password immessa deve rispettare i criteri di complessità richiesti."
            errore = True
        ElseIf (txtConfermaNuovaPwd.Text <> txtNuovaPwd.Text) Then
            messaggioDiErrore = "Attenzione. I campi Nuova password e Conferma nuova password devono essere uguali."
            errore = True
        End If
        If (errore = True) Then
            imgAlert.Visible = True
            lblmessaggiosopra.Visible = True
            lblmessaggiosopra.Text = messaggioDiErrore
        End If

        Return errore
    End Function
    Private Function verificaComplessitaPasswordNonRispettata() As Boolean
        Dim errore As Boolean = False
        Dim MAIUSCOLE As String = "ABCDEFGHIJKLMNOPQRSTUVWXYZ"
        Dim MINUSCOLE As String = "abcdefghijklmnopqrstuvwxyz"
        Dim NUMERI As String = "1234567890"
        Dim INTERPUNZIONI As String = ": , . !"
        Dim checkMaiuscole As Integer
        Dim checkMinuscole As Integer
        Dim checkNumeri As Integer
        Dim checkInterpunzioni As Integer
        Dim arrayNuovaPwd As Char() = txtNuovaPwd.Text.ToCharArray
        Dim regoleRispettate As Integer = 0


        For index As Integer = 0 To txtNuovaPwd.Text.Length - 1
            Dim charAt As Char = arrayNuovaPwd.ElementAt(index)
            If MAIUSCOLE.Contains(charAt) Then
                checkMaiuscole = 1
            ElseIf (MINUSCOLE.Contains(charAt)) Then
                checkMinuscole = 1
            ElseIf (NUMERI.Contains(charAt)) Then
                checkNumeri = 1
            ElseIf (INTERPUNZIONI.Contains(charAt)) Then
                checkInterpunzioni = 1
            End If
            regoleRispettate = checkMaiuscole + checkMinuscole + checkNumeri + checkInterpunzioni
            If (regoleRispettate >= 3) Then
                errore = False
                Exit For
            End If
        Next
        If (regoleRispettate < 3) Then
            errore = True
        End If
        Return errore
    End Function


    Public Function VerificaEsistenzaPECFIRMA() As Boolean
        Dim dtrGenerico As SqlClient.SqlDataReader
        Dim EsistenzaPEC As String
        Dim EsistenzaFIRMA As String
        Dim StatoPEC As String
        If Not dtrGenerico Is Nothing Then
            dtrGenerico.Close()
            dtrGenerico = Nothing
        End If
        dtrGenerico = ClsServer.CreaDatareader("select isnull(EmailCertificata,'') as EmailCertificata, isnull(Firma,0) as Firma, isnull(StatoPec,0) as StatoPec from enti where enti.idente='" & Session("IdEnte") & "'", Session("conn"))

        If dtrGenerico.HasRows = True Then
            dtrGenerico.Read()
            EsistenzaPEC = dtrGenerico("EmailCertificata")
            EsistenzaFIRMA = dtrGenerico("Firma")
            StatoPEC = dtrGenerico("StatoPEC")
            If Not dtrGenerico Is Nothing Then
                dtrGenerico.Close()
                dtrGenerico = Nothing
            End If
            Dim mezzanotte As Date = "01/01/2013"
            Dim Adesso As Date
            Adesso = Now()
            If Adesso < mezzanotte Then
                If EsistenzaPEC = "" Or EsistenzaFIRMA <> 1 Or StatoPEC = 2 Then
                    Return True
                Else
                    Return False
                End If
            Else
                Return False
            End If
        End If

        If Not dtrGenerico Is Nothing Then
            dtrGenerico.Close()
            dtrGenerico = Nothing
        End If
    End Function
    Private Sub GestionePasswordEnti()
        ''controlla le abilitazioni 
        Dim StrPass As String  'VECCHIA PASSWORD
        Dim StrPass1 As String  'NUOVA PASSWORD
        Dim strsql As String
        Dim dtrTrovaUtente As SqlClient.SqlDataReader
        Dim cmdinsert As SqlClient.SqlCommand

        Dim DataCrono As Date
        Dim dataDifferenza As Long
        Dim dataAttuale As Date = Now
        Dim myRow As DataRow
        Dim Tab As DataTable
        Dim strProg As String
        Dim checkVerificaErroriValidazione As Boolean

        checkVerificaErroriValidazione = verificaErroriValidazionePassword()



        If (checkVerificaErroriValidazione = False) Then

            'command che eseguirà le query
            Dim myCommand As System.Data.SqlClient.SqlCommand




            If Session("TipoUtente") = "R" Then
                strsql = "SELECT  top 10 Username, Dbo.ReadPassword(Password)as Password, DataCronologia FROM CronologiaPasswordRegioni WHERE Username ='" & Session("Utente") & "' order by DataCronologia desc "  ' and Password='" & txtNuovaPwd.Text & "'
                Tab = ClsServer.CreaDataTable(strsql, False, Session("conn"))
            Else
                strsql = "SELECT  top 10 Username, Dbo.ReadPassword(Password)as Password, DataCronologia FROM CronologiaPasswordEnti WHERE Username ='" & Session("Utente") & "' order by DataCronologia desc "  ' and Password='" & txtNuovaPwd.Text & "'
                Tab = ClsServer.CreaDataTable(strsql, False, Session("conn"))
            End If

            'Controllo se la password nuova e' stata iserita le ultime 10 volte
            For Each myRow In Tab.Rows
                If IsDBNull(myRow.Item("Password")) = False Then

                    If myRow.Item("Password") = txtNuovaPwd.Text Then
                        imgAlert.Visible = True
                        lblmessaggiosopra.Visible = True
                        lblmessaggiosopra.Text = "Password gia' utilizzata nelle ultime 10 volte."
                        Exit Sub
                    End If


                End If

            Next

            'Controllo se la password nuova e' stata iserita oltre le ultime 10 volte e se ha superato i 180 giorni
            If Session("TipoUtente") = "U" Then
                strsql = "SELECT  Username, Dbo.ReadPassword(Password)as Password, DataCronologia FROM CronologiaPasswordRegioni WHERE Username ='" & Session("Utente") & "' order by DataCronologia desc "  ' and Password='" & txtNuovaPwd.Text & "'
                Tab = ClsServer.CreaDataTable(strsql, False, Session("conn"))
            Else
                strsql = "SELECT  Username, Dbo.ReadPassword(Password)as Password, DataCronologia FROM CronologiaPasswordEnti WHERE Username ='" & Session("Utente") & "' order by DataCronologia desc "  ' and Password='" & txtNuovaPwd.Text & "'
                Tab = ClsServer.CreaDataTable(strsql, False, Session("conn"))
            End If


            Dim y As Integer
            y = 0
            For Each myRow In Tab.Rows
                y = y + 1
                If y > 10 Then
                    If IsDBNull(myRow.Item("Password")) = False Then

                        If myRow.Item("Password") = txtNuovaPwd.Text Then
                            DataCrono = myRow.Item("DataCronologia")
                            dataDifferenza = DateDiff("d", DataCrono, dataAttuale)
                            If dataDifferenza < 180 Then
                                imgAlert.Visible = True
                                lblmessaggiosopra.Visible = True
                                lblmessaggiosopra.Text = "Password gia' utilizzata negli ultimi 180 giorni."
                                Exit Sub
                            End If
                        End If
                    End If
                End If
            Next

            StrPass = CreatePSW(txtVecchiaPwd.Text.Trim)
            StrPass1 = CreatePSW(txtNuovaPwd.Text.Trim)

            dtrTrovaUtente = ClsServer.CreaDatareader("select Denominazione, Username, Password, DataModificaPassword, Identificativo,getdate() as dataserver, Tipo from VW_Account_Utenti where username='" & Session("Utente") & "' and password='" & Replace(StrPass, "'", "''") & "'", Session("conn"))
            If dtrTrovaUtente.HasRows = True Then
                If Not dtrTrovaUtente Is Nothing Then
                    dtrTrovaUtente.Close()
                    dtrTrovaUtente = Nothing
                End If

                myCommand = New System.Data.SqlClient.SqlCommand
                myCommand.Connection = Session("conn")

                Select Case Session("TipoUtente")
                    'If (Session("TipoUtente") = "U" Or Session("TipoUtente") = "R") Then
                    'Case "U"
                    '        'modifica della pwd sulla base dati
                    '        strProg = "update UTENTIUNSC set DataModificaPassword=GetDate(), Password='"
                    Case "R"
                        'modifica della pwd sulla base dati
                        strProg = "update UTENTIUNSC set DataModificaPassword=GetDate(),CambioPassword=0, Password='"

                    Case "E"
                        If Not dtrTrovaUtente Is Nothing Then
                            dtrTrovaUtente.Close()
                            dtrTrovaUtente = Nothing
                        End If
                        dtrTrovaUtente = ClsServer.CreaDatareader("select Denominazione, Username, Password from enti where username='" & Session("Utente") & "' and password='" & Replace(StrPass, "'", "''") & "'", Session("conn"))
                        If dtrTrovaUtente.HasRows = True Then
                            'modifica della pwd sulla base dati
                            strProg = "update enti set DataModificaPassword=GetDate(), Password='"
                        Else
                            'modifica della pwd sulla base dati
                            strProg = "update entipassword set DataModificaPassword=GetDate(),CambioPassword=0, Password='"
                        End If
                        If Not dtrTrovaUtente Is Nothing Then
                            dtrTrovaUtente.Close()
                            dtrTrovaUtente = Nothing
                        End If
                End Select

                'stringa per l'aggiornamento

                'se la password inserita supera i controlli di 3 su 4
                If ClsUtility.ModificaPasswordADC(txtNuovaPwd.Text) = True Then

                    If ClsUtility.ControlloCaratteriPasswordADC(txtNuovaPwd.Text) = True Then
                        Response.Write("Verifica Password OK")
                    Else
                        imgAlert.Visible = True
                        lblmessaggiosopra.Visible = True
                        lblmessaggiosopra.Text = "Ci sono Caratteri non validi all'interno della password."
                        Exit Sub
                    End If

                    strProg = strProg & Replace(CreatePSW(Trim(txtNuovaPwd.Text)), "'", "''") & "' "
                    strProg = strProg & "where UserName='" & Session("Utente") & "'"
                    '-------Antonello------------toglierlo in caso da qui----------------------
                    myCommand.CommandText = strProg
                    myCommand.ExecuteNonQuery()
                    '-------Antonello-----------
                    If Not dtrTrovaUtente Is Nothing Then
                        dtrTrovaUtente.Close()
                        dtrTrovaUtente = Nothing
                    End If



                    'Bisogna capire quando deve inserire in CronologiaPasswordRegioni o in CronologiaPasswordEnti quale e' la condizione discriminante
                    If Session("tipoutente") = "R" Then
                        strsql = "insert into CronologiaPasswordRegioni(UserName,Password,DataCronologia) "
                        strsql = strsql & "values "
                        strsql = strsql & "('" & Session("Utente") & "','" & Replace(StrPass1, "'", "''") & "',GetDate())"
                        cmdinsert = New SqlClient.SqlCommand(strsql, Session("conn"))
                        cmdinsert.ExecuteNonQuery()
                    Else
                        strsql = "insert into CronologiaPasswordEnti(UserName,Password,DataCronologia) "
                        strsql = strsql & "values "
                        strsql = strsql & "('" & Session("Utente") & "','" & Replace(StrPass1, "'", "''") & "',GetDate())"
                        cmdinsert = New SqlClient.SqlCommand(strsql, Session("conn"))
                        cmdinsert.ExecuteNonQuery()
                    End If


                    If Not dtrTrovaUtente Is Nothing Then
                        dtrTrovaUtente.Close()
                        dtrTrovaUtente = Nothing
                    End If


                Else
                    imgAlert.Visible = True
                    lblmessaggiosopra.Visible = True
                    lblmessaggiosopra.Text = "La  password inserita non rispetta i criteri di complessita' previsti."
                    If Not dtrTrovaUtente Is Nothing Then
                        dtrTrovaUtente.Close()
                        dtrTrovaUtente = Nothing
                    End If
                    Exit Sub
                End If
                'myCommand.CommandText = strProg
                'myCommand.ExecuteNonQuery()

                '*******************************************

                StrPass = CreatePSW(Trim(txtNuovaPwd.Text))
                If Not dtrTrovaUtente Is Nothing Then
                    dtrTrovaUtente.Close()
                    dtrTrovaUtente = Nothing
                End If
                dtrTrovaUtente = ClsServer.CreaDatareader("select Denominazione, Username, Password, Identificativo,getdate() as dataserver, Tipo from VW_Account_Utenti where username='" & Session("Utente") & "' and password='" & Replace(StrPass, "'", "''") & "'", Session("conn"))
                If dtrTrovaUtente.HasRows = True Then
                    dtrTrovaUtente.Read()
                    Session("Denominazione") = dtrTrovaUtente("Denominazione") & " "
                    Session("idEnte") = dtrTrovaUtente("Identificativo")
                    Session("Utente") = dtrTrovaUtente("Username")
                    Session("TipoUtente") = dtrTrovaUtente("Tipo")
                    Session("dataserver") = Day(dtrTrovaUtente("dataserver")) & "/" & IIf(Month(dtrTrovaUtente("dataserver")) < 10, "0" & Month(dtrTrovaUtente("dataserver")), Month(dtrTrovaUtente("dataserver"))) & "/" & Year(dtrTrovaUtente("dataserver"))
                    Session("LogIn") = True
                    dtrTrovaUtente.Close()
                    If VerificaEsistenzaPECFIRMA() = True Then
                        If Not dtrTrovaUtente Is Nothing Then
                            dtrTrovaUtente.Close()
                            dtrTrovaUtente = Nothing
                        End If
                        Response.Redirect("WfrmAggiornamentoMailEnti.aspx")
                    Else
                        If Not dtrTrovaUtente Is Nothing Then
                            dtrTrovaUtente.Close()
                            dtrTrovaUtente = Nothing
                        End If
                        Response.Redirect("WfrmSistema.aspx")
                    End If

                Else
                    Session("conn").close()
                    Session("conn") = Nothing
                    Session("LogIn") = False
                    dtrTrovaUtente.Close()
                    Response.Redirect("LogOn.aspx")
                End If
                If Not dtrTrovaUtente Is Nothing Then
                    dtrTrovaUtente.Close()
                    dtrTrovaUtente = Nothing
                End If
            Else
                If Not dtrTrovaUtente Is Nothing Then
                    dtrTrovaUtente.Close()
                    dtrTrovaUtente = Nothing
                End If
                imgAlert.Visible = True
                lblmessaggiosopra.Visible = True
                lblmessaggiosopra.Text = "La vecchia password risulta essere errata."
            End If
            If Not dtrTrovaUtente Is Nothing Then
                dtrTrovaUtente.Close()
                dtrTrovaUtente = Nothing
            End If
        End If
    End Sub
    Private Sub GestionePasswordSedi()
        ''controlla le abilitazioni 
        Dim StrPass As String  'VECCHIA PASSWORD
        Dim StrPass1 As String  'NUOVA PASSWORD
        Dim strsql As String
        Dim dtrTrovaUtente As SqlClient.SqlDataReader
        Dim cmdinsert As SqlClient.SqlCommand

        Dim DataCrono As Date
        Dim dataDifferenza As Long
        Dim dataAttuale As Date = Now
        Dim myRow As DataRow
        Dim Tab As DataTable
        Dim strProg As String
        Dim checkVerificaErroriValidazione As Boolean

        checkVerificaErroriValidazione = verificaErroriValidazionePassword()


        If (checkVerificaErroriValidazione = False) Then

            'command che eseguirà le query
            Dim myCommand As System.Data.SqlClient.SqlCommand

            strsql = "SELECT  top 10 Username, Dbo.ReadPassword(Password)as Password, DataCronologia FROM CronologiaPasswordSedi WHERE Username ='" & Session("Utente") & "' order by DataCronologia desc "  ' and Password='" & txtNuovaPwd.Text & "'
            Tab = ClsServer.CreaDataTable(strsql, False, Session("conn"))

            'Controllo se la password nuova e' stata inserita le ultime 10 volte
            For Each myRow In Tab.Rows
                If IsDBNull(myRow.Item("Password")) = False Then
                    If myRow.Item("Password") = txtNuovaPwd.Text Then
                        imgAlert.Visible = True
                        lblmessaggiosopra.Visible = True
                        lblmessaggiosopra.Text = "Password gia' utilizzata nelle ultime 10 volte."
                        Exit Sub
                    End If
                End If
            Next

            'Controllo se la password nuova e' stata iserita oltre le ultime 10 volte e se ha superato i 180 giorni
            strsql = "SELECT  Username, Dbo.ReadPassword(Password)as Password, DataCronologia FROM CronologiaPasswordSedi WHERE Username ='" & Session("Utente") & "' order by DataCronologia desc "  ' and Password='" & txtNuovaPwd.Text & "'
            Tab = ClsServer.CreaDataTable(strsql, False, Session("conn"))
 

            Dim y As Integer
            y = 0
            For Each myRow In Tab.Rows
                y = y + 1
                If y > 10 Then
                    If IsDBNull(myRow.Item("Password")) = False Then

                        If myRow.Item("Password") = txtNuovaPwd.Text Then
                            DataCrono = myRow.Item("DataCronologia")
                            dataDifferenza = DateDiff("d", DataCrono, dataAttuale)
                            If dataDifferenza < 180 Then
                                imgAlert.Visible = True
                                lblmessaggiosopra.Visible = True
                                lblmessaggiosopra.Text = "Password gia' utilizzata negli ultimi 180 giorni."
                                Exit Sub
                            End If
                        End If
                    End If
                End If
            Next

        StrPass = CreatePSW(txtVecchiaPwd.Text.Trim)
        StrPass1 = CreatePSW(txtNuovaPwd.Text.Trim)

        dtrTrovaUtente = ClsServer.CreaDatareader("select Denominazione, Username, Password, DataModificaPassword, Identificativo,getdate() as dataserver, Tipo from VW_Account_Utenti where username='" & Session("Utente") & "' and password='" & Replace(StrPass, "'", "''") & "'", Session("conn"))
        If dtrTrovaUtente.HasRows = True Then
            If Not dtrTrovaUtente Is Nothing Then
                dtrTrovaUtente.Close()
                dtrTrovaUtente = Nothing
            End If

            myCommand = New System.Data.SqlClient.SqlCommand
            myCommand.Connection = Session("conn")

            Select Case Session("TipoUtente")

                Case "E"
                    If Not dtrTrovaUtente Is Nothing Then
                        dtrTrovaUtente.Close()
                        dtrTrovaUtente = Nothing
                    End If
                        'dtrTrovaUtente = ClsServer.CreaDatareader("select Denominazione, Username, Password from enti where username='" & Session("Utente") & "' and password='" & Replace(StrPass, "'", "''") & "'", Session("conn"))
                        'If dtrTrovaUtente.HasRows = True Then
                        '    'modifica della pwd sulla base dati
                        '    strProg = "update enti set DataModificaPassword=GetDate(), Password='"
                        'Else
                        'modifica della pwd sulla base dati
                        strProg = "update sedipassword set DataModificaPassword=GetDate(),CambioPassword=0, Password='"
                        'End If
                        If Not dtrTrovaUtente Is Nothing Then
                            dtrTrovaUtente.Close()
                            dtrTrovaUtente = Nothing
                        End If
                End Select

                'stringa per l'aggiornamento

                'se la password inserita supera i controlli di 3 su 4
                If ClsUtility.ModificaPasswordADC(txtNuovaPwd.Text) = True Then

                    If ClsUtility.ControlloCaratteriPasswordADC(txtNuovaPwd.Text) = True Then
                        Response.Write("Verifica Password OK")
                    Else
                        imgAlert.Visible = True
                        lblmessaggiosopra.Visible = True
                        lblmessaggiosopra.Text = "Ci sono Caratteri non validi all'interno della password."
                        Exit Sub
                    End If

                    strProg = strProg & Replace(CreatePSW(Trim(txtNuovaPwd.Text)), "'", "''") & "' "
                    strProg = strProg & "where UserName='" & Session("Utente") & "'"
                    '-------Antonello------------toglierlo in caso da qui----------------------
                    myCommand.CommandText = strProg
                    myCommand.ExecuteNonQuery()
                    '-------Antonello-----------
                    If Not dtrTrovaUtente Is Nothing Then
                        dtrTrovaUtente.Close()
                        dtrTrovaUtente = Nothing
                    End If



                    'Bisogna capire quando deve inserire in CronologiaPasswordRegioni o in CronologiaPasswordEnti quale e' la condizione discriminante

                    strsql = "insert into CronologiaPasswordSedi(UserName,Password,DataCronologia) "
                    strsql = strsql & "values "
                    strsql = strsql & "('" & Session("Utente") & "','" & Replace(StrPass1, "'", "''") & "',GetDate())"
                    cmdinsert = New SqlClient.SqlCommand(strsql, Session("conn"))
                    cmdinsert.ExecuteNonQuery()



                    If Not dtrTrovaUtente Is Nothing Then
                        dtrTrovaUtente.Close()
                        dtrTrovaUtente = Nothing
                    End If


                Else
                    imgAlert.Visible = True
                    lblmessaggiosopra.Visible = True
                    lblmessaggiosopra.Text = "La  password inserita non rispetta i criteri di complessita' previsti."
                    If Not dtrTrovaUtente Is Nothing Then
                        dtrTrovaUtente.Close()
                        dtrTrovaUtente = Nothing
                    End If
                    Exit Sub
                End If
                'myCommand.CommandText = strProg
                'myCommand.ExecuteNonQuery()

                '*******************************************

                StrPass = CreatePSW(Trim(txtNuovaPwd.Text))
                If Not dtrTrovaUtente Is Nothing Then
                    dtrTrovaUtente.Close()
                    dtrTrovaUtente = Nothing
                End If
                dtrTrovaUtente = ClsServer.CreaDatareader("select Denominazione, Username, Password, Identificativo,getdate() as dataserver, Tipo from VW_Account_Utenti where username='" & Session("Utente") & "' and password='" & Replace(StrPass, "'", "''") & "'", Session("conn"))
                If dtrTrovaUtente.HasRows = True Then
                    dtrTrovaUtente.Read()
                    Session("Denominazione") = dtrTrovaUtente("Denominazione") & " "
                    Session("idEnte") = dtrTrovaUtente("Identificativo")
                    Session("Utente") = dtrTrovaUtente("Username")
                    Session("TipoUtente") = dtrTrovaUtente("Tipo")
                    Session("dataserver") = Day(dtrTrovaUtente("dataserver")) & "/" & IIf(Month(dtrTrovaUtente("dataserver")) < 10, "0" & Month(dtrTrovaUtente("dataserver")), Month(dtrTrovaUtente("dataserver"))) & "/" & Year(dtrTrovaUtente("dataserver"))
                    Session("LogIn") = True
                    dtrTrovaUtente.Close()
                    If VerificaEsistenzaPECFIRMA() = True Then
                        If Not dtrTrovaUtente Is Nothing Then
                            dtrTrovaUtente.Close()
                            dtrTrovaUtente = Nothing
                        End If
                        Response.Redirect("WfrmAggiornamentoMailEnti.aspx")
                    Else
                        If Not dtrTrovaUtente Is Nothing Then
                            dtrTrovaUtente.Close()
                            dtrTrovaUtente = Nothing
                        End If
                        Response.Redirect("WfrmSistema.aspx")
                    End If

                Else
                    Session("conn").close()
                    Session("conn") = Nothing
                    Session("LogIn") = False
                    dtrTrovaUtente.Close()
                    Response.Redirect("LogOn.aspx")
                End If
                If Not dtrTrovaUtente Is Nothing Then
                    dtrTrovaUtente.Close()
                    dtrTrovaUtente = Nothing
                End If
            Else
                If Not dtrTrovaUtente Is Nothing Then
                    dtrTrovaUtente.Close()
                    dtrTrovaUtente = Nothing
                End If
                imgAlert.Visible = True
                lblmessaggiosopra.Visible = True
                lblmessaggiosopra.Text = "La vecchia password risulta essere errata."
            End If
            If Not dtrTrovaUtente Is Nothing Then
                dtrTrovaUtente.Close()
                dtrTrovaUtente = Nothing
            End If
        End If
    End Sub
End Class