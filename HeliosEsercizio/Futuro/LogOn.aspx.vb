Imports System.IO
Imports System.Xml
Imports System.Configuration.ConfigurationManager
Imports System.Data.SqlClient
Imports System.Text.RegularExpressions
Imports Logger.Data

Public Class LogOn
    Inherits SmartPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Session.Clear()

        Letturaxml()
        'FORMLOGIN.Visible = False
        'avviso.Visible = False
    End Sub
    Public Sub Letturaxml()
        Dim XmlTotale As String = ""

        Dim reader1 As New StreamReader(Server.MapPath("") & "\LOGON_CONFIG.xml", System.Text.Encoding.UTF8, False)

        XmlTotale = reader1.ReadToEnd()
        reader1.Close()
        reader1.Dispose()

        Dim valoreelemento As String = ""
        'XmlTextReader reader = new XmlTextReader(strFile);
        Dim valorestringa As New StringReader(XmlTotale)

        Dim reader As New XmlTextReader(valorestringa)
        While reader.Read()
            Select Case reader.NodeType
                Case XmlNodeType.Element
                    ' The node is an element.
                    Console.Write("<" + reader.Name)
                    While reader.MoveToNextAttribute()
                        ' Read the attributes.   
                        Console.Write((" " + reader.Name & "='") + reader.Value & "'")
                    End While

                    Console.WriteLine(">")

                    valoreelemento = reader.Name.ToString()
                    Exit Select

                Case XmlNodeType.Text
                    'Display the text in each element.
                    Console.WriteLine(reader.Value)



                    Select Case valoreelemento
                        Case "FlagAvviso"

                            If reader.Value = "SI" Then
                                avviso.Visible = True
                                Dim reader2 As New StreamReader(Server.MapPath("") & "\TestoAvviso.txt")

                                testoavviso.InnerHtml = reader2.ReadToEnd()
                                reader2.Close()
                                reader2.Dispose()


                            Else
                                avviso.Visible = False
                            End If

                            Exit Select
                            'Case "TestoAvviso"
                            '    If reader.Value <> "" Then
                            '        testoavviso.InnerHtml = reader.Value
                            '    End If

                            '    Exit Select
                        Case "FlagLogon"
                            If reader.Value = "SI" Then
                                FORMLOGIN.Visible = True
                            Else
                                FORMLOGIN.Visible = False
                            End If
                            Exit Select

                    End Select

            End Select
        End While
        'Console.ReadLine()

    End Sub

    Protected Sub btnAccedi_Click(sender As Object, e As EventArgs) Handles btnAccedi.Click
        ''controlla le abilitazioni 
        'Dim StrPass As String
        'Dim dtrTrovaUtente As SqlClient.SqlDataReader
        Dim criptString As New criptString
        Dim xmlLocal As XmlDocument
        'istanzio il Web Service 
        Dim wsAutenticazioneLocal As New WS_Autenticazione.wsAutenticazione
        Dim strControlliUserNamePassword() As String
        Dim intX As Integer



        lblError.Text = ControlloCampi(txtNomeUtente.Text, txtPassword.Text)
        If lblError.Text <> "" Then
            Exit Sub
        End If

        'Response.Redirect("default.aspx")


        ReDim strControlliUserNamePassword(14)

        '=========================================================================================================
        'CONTROLLO I CARATTERI CONTENUTI NEL CAMPO USERNAME E NEL CAMPO PWD PER VERIFICARE EVENTUALI SQL-INJECTION
        '=========================================================================================================

        strControlliUserNamePassword(0) = "--"
        strControlliUserNamePassword(1) = "/*"
        strControlliUserNamePassword(2) = "*/"
        strControlliUserNamePassword(3) = ";"
        strControlliUserNamePassword(4) = "EXEC"
        strControlliUserNamePassword(5) = "UPDATE"
        strControlliUserNamePassword(6) = "INSERT"
        strControlliUserNamePassword(7) = "DELETE"
        strControlliUserNamePassword(8) = "SELECT"
        strControlliUserNamePassword(9) = "TRUNCATE"
        strControlliUserNamePassword(10) = "DROP"
        strControlliUserNamePassword(11) = "'"
        strControlliUserNamePassword(12) = " "
        strControlliUserNamePassword(13) = "TABLE"
        strControlliUserNamePassword(14) = "COLUMN"

        For intX = 0 To UBound(strControlliUserNamePassword)
            If InStr(UCase(txtNomeUtente.Text), strControlliUserNamePassword(intX)) > 0 Then
                Session.Clear()
                'Response.Redirect("WfrmErrLogON.aspx")
                lblError.Text = "ERRORE"
            End If
            If InStr(UCase(txtPassword.Text), strControlliUserNamePassword(intX)) > 0 Then
                Session.Clear()
                'Response.Redirect("WfrmErrLogON.aspx")
                lblError.Text = "ERRORE"
            End If
        Next
        Session("conn") = New SqlClient.SqlConnection
        'Session("conn").ConnectionString = "user id=sa;password=vbra250;data source=www1;persist security info=False;connect timeout=300;Max Pool Size=3000;initial catalog=unscproduzione"
        Session("conn").ConnectionString = "user id=" & AppSettings("DB_USERNAME") & ";password=" & AppSettings("DB_PASSWORD") & ";data source=" & AppSettings("DB_DATA_SOURCE") & ";persist security info=False;connect timeout=300;Max Pool Size=3000;initial catalog=" & AppSettings("DB_NAME") & ""

        Session("conn").open()
        Session("Read") = 0

        Session("Padre30") = True
        Session("Padre41") = True
        Session("Padre69") = True
        Session("Padre173") = True
        Session("Padre84") = True
        Session("Padre38") = True
        Session("Padre208") = True

        Session("Account") = txtNomeUtente.Text.Replace("'", "''")

        'chiamare store
        Dim SP_LOGINVALORI As DataTable
        Dim SqlCmd As New SqlCommand
        Dim dataAdapter As SqlDataAdapter = New SqlDataAdapter
        SP_LOGINVALORI = New DataTable

        SqlCmd.CommandText = "SP_LOGIN"
        SqlCmd.CommandType = CommandType.StoredProcedure
        SqlCmd.Connection = Session("conn")
        SqlCmd.Parameters.AddWithValue("@Username", txtNomeUtente.Text)
        SqlCmd.Parameters.AddWithValue("@Password", txtPassword.Text)
        dataAdapter.SelectCommand = SqlCmd
        dataAdapter.Fill(SP_LOGINVALORI)
        Dim messaggio As String
        Dim NAccessi As Integer = 0
        Dim tipoUtente As String
        tipoUtente = SP_LOGINVALORI(0).Item("TipoUtente").ToString


        If tipoUtente = "" Then

            If Store_LOGIN_Controllo_Accessi(txtNomeUtente.Text.Replace("'", "''"), messaggio, NAccessi) = 0 Then
                lblError.Text = messaggio
                'lblError.Visible = True
                Exit Sub
            Else
                lblError.Text = "UTENZA O PASSWORD ERRATA"
            End If



            ClsServer.RegistrazioneLogAccessi(txtNomeUtente.Text.Replace("'", "''"), "LOGIN", 0, Session("conn"))
            'controllo accessi al sistema
            If Store_LOGIN_Controllo_Accessi(txtNomeUtente.Text.Replace("'", "''"), messaggio, NAccessi) = 0 Then
                Select Case NAccessi
                    Case 3
                        lblError.Text = "UTENZA O PASSWORD ERRATA" & ". " & messaggio
                    Case Else
                        lblError.Text = "UTENZA O PASSWORD ERRATA"
                End Select

                lblError.Visible = True
                Exit Sub

            End If



            ' lblError.Text = "UTENZA O PASSWORD ERRATA"
            'Exit Sub
        End If
        If tipoUtente = "U" Then

            If Not AppSettings("IsTest") = "1" Then 'se sono in test evito il controllo


                xmlLocal = New XmlDocument

                wsAutenticazioneLocal.Url = AppSettings("URL_WS_Autenticazione")

                xmlLocal.LoadXml(wsAutenticazioneLocal.EseguiAutenticazione(criptString.EncryptData(txtNomeUtente.Text), criptString.EncryptData(txtPassword.Text)))


                Select Case xmlLocal.InnerText
                    Case "POSITIVO"

                        'CheckLoginUNSC()
                        Session("LogIn") = True

                        Session("Utente") = SP_LOGINVALORI(0).Item("Utente").ToString
                        Session("TipoUtente") = SP_LOGINVALORI(0).Item("TipoUtente").ToString

                        ClsServer.RegistrazioneLogAccessi(txtNomeUtente.Text, "LOGIN", 1, Session("conn"))

                        'PAGINA DI SCELTA SISTEMA
                        Response.Redirect("WfrmSistema.aspx")
                        'Response.Redirect("WfrmRicercaEnti.aspx")

                    Case "NEGATIVO"
                        ClsServer.RegistrazioneLogAccessi(txtNomeUtente.Text, "LOGIN", 0, Session("conn"))
                        Session.Clear()
                        'Response.Redirect("WfrmErrLogON.aspx")
                        lblError.Text = "UTENZA O PASSWORD ERRATA"

                    Case "ERRORE"


                        Session.Clear()
                        'Response.Redirect("WfrmErrLogON.aspx")
                        lblError.Text = "ERRORE GENERICO"
                End Select
            Else
                '***************solo per sviluppo/test*********
                Session("LogIn") = True

                Session("Utente") = SP_LOGINVALORI(0).Item("Utente").ToString
                Session("TipoUtente") = SP_LOGINVALORI(0).Item("TipoUtente").ToString

                ClsServer.RegistrazioneLogAccessi(txtNomeUtente.Text, "LOGIN", 1, Session("conn"))


                Response.Redirect("WfrmSistema.aspx")

                '************Fine Solo per Sviluppo/test
            End If
        End If
        'wsAutenticazioneLocal.Url = ConfigurationSettings.AppSettings("URL_WS_Autenticazione")
        If tipoUtente = "R" Or tipoUtente = "E" Then

            Session("Utente") = SP_LOGINVALORI(0).Item("Utente").ToString
            Session("TipoUtente") = SP_LOGINVALORI(0).Item("TipoUtente").ToString

            If SP_LOGINVALORI(0).Item("LogIn") = "1" Then
                Session("LogIn") = True
            Else
                Session("LogIn") = False
            End If

            'controllo accessi al sistema
            If Store_LOGIN_Controllo_Accessi(Session("Utente"), messaggio, NAccessi) = 0 Then
                lblError.Text = messaggio
                lblError.Visible = True
                Exit Sub
            Else
                lblError.Text = String.Empty
                lblError.Visible = False
            End If


            ClsServer.RegistrazioneLogAccessi(txtNomeUtente.Text, "LOGIN", IIf(Session("LogIn") = True, 1, 0), Session("conn"))
            'Session("FiltroVisibilita") = FiltroVisibilità("FUTURO", Session("TipoUtente"))
            'controllo scadenza password
            If Session("LogIn") = True Then
                If SP_LOGINVALORI(0).Item("PasswordScaduta").ToString = True Then
                    Response.Redirect("modificapwd.aspx?operazione=login")
                Else
                    'PAGINA DI SCELTA SISTEMA
                    Response.Redirect("WfrmSistema.aspx")
                    'Response.Redirect("WfrmMain.aspx") 
                End If
            Else
                lblError.Text = "UTENZA NON ATTIVA"
                lblError.Visible = True
            End If

        End If
    End Sub

    Private Function ControlloCampi(ByVal utente As String, ByVal pws As String)
        Dim msg As String = ""


        If utente = "" And pws = "" Then
            msg = "IL NOME UTENTE E LA PASSWORD SONO OBBLIGATORI"
            Return msg
            Exit Function
        End If
        If utente = "" Then
            msg = "IL NOME UTENTE E' OBBLIGATORIO"
            Return msg
            Exit Function

        End If
        If pws = "" Then
            msg = "LA PASSWORD E' OBBLIGATORIA"
            Return msg
            Exit Function

        End If
        If (pws.Count) < 8 Then
            msg = "LA PASSWORD E' MINIMO 8 CARATTERI"
            Return msg
            Exit Function
        End If
        Return msg
    End Function

    Private Function Store_LOGIN_Controllo_Accessi(ByVal Username As String, ByRef messaggio As String, ByRef NAccessi As Integer) As Integer
        '** Aggiunto da simona cordella
        '** Funzionalità: controlla il numero di accessi nel sistema.

        Dim SqlCmd As New SqlClient.SqlCommand
        Try
            SqlCmd.CommandText = "SP_LOGIN_CONTROLLO_ACCESSI"
            SqlCmd.CommandType = CommandType.StoredProcedure
            SqlCmd.Connection = Session("Conn")

            SqlCmd.Parameters.Add("@Username", SqlDbType.VarChar).Value = Username

            SqlCmd.Parameters.Add("@messaggio", SqlDbType.VarChar)
            SqlCmd.Parameters("@messaggio").Size = 1000
            SqlCmd.Parameters("@messaggio").Direction = ParameterDirection.Output


            SqlCmd.Parameters.Add("@Ritorno", SqlDbType.TinyInt)
            SqlCmd.Parameters("@Ritorno").Direction = ParameterDirection.Output

            SqlCmd.Parameters.Add("@NAccessi", SqlDbType.TinyInt)
            SqlCmd.Parameters("@NAccessi").Direction = ParameterDirection.Output

            SqlCmd.ExecuteNonQuery()
            messaggio = SqlCmd.Parameters("@messaggio").Value
            NAccessi = SqlCmd.Parameters("@NAccessi").Value
            Return SqlCmd.Parameters("@Ritorno").Value
        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Function

    Private Sub btnAccediSpid_Click(sender As Object, e As EventArgs) Handles btnAccediSpid.Click
        Response.Redirect(AppSettings("UrlSistemaAccesso"))
    End Sub

    Protected Sub Unnamed_Click(sender As Object, e As EventArgs)
        Dim pdf As New AsposePDF("D:\Downloads\DocumentoFirmato.pdf")
        Dim names = pdf.GetSignitures()
    End Sub
End Class