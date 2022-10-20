Imports System.Xml
Imports System.Configuration.ConfigurationManager
Imports System.Data.SqlClient
Imports System.IO

Public Class LogOnRead
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Session.Clear()
        'Letturaxml()
        avviso.Visible = False 'da togliere se ripristino letturaxml
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
        Console.ReadLine()

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
        Session("conn").ConnectionString = "user id=" & AppSettings("DB_USERNAMELETTURA") & ";password=" & AppSettings("DB_PASSWORDLETTURA") & ";data source=" & AppSettings("DB_DATA_SOURCE") & ";persist security info=False;connect timeout=300;Max Pool Size=3000;initial catalog=" & AppSettings("DB_NAME") & ""

        Session("conn").open()
        Session("Read") = 1

        Session("Padre30") = True
        Session("Padre41") = True
        Session("Padre69") = True
        Session("Padre173") = True
        Session("Padre84") = True
        Session("Padre38") = True
        Session("Padre208") = True


        Session("Account") = txtNomeUtente.Text.Replace("'", "''")

        '--------------------------------------------------------------------------

        'chiamare(store)
        Dim SP_LOGINVALORI As DataTable
        Dim SqlCmd As New SqlCommand
        Dim dataAdapter As SqlDataAdapter = New SqlDataAdapter
        SP_LOGINVALORI = New DataTable

        SqlCmd.CommandText = "SP_LOGINREAD"
        SqlCmd.CommandType = CommandType.StoredProcedure
        SqlCmd.Connection = Session("conn")
        SqlCmd.Parameters.AddWithValue("@Username", txtNomeUtente.Text)
        SqlCmd.Parameters.AddWithValue("@Password", txtPassword.Text)
        dataAdapter.SelectCommand = SqlCmd
        dataAdapter.Fill(SP_LOGINVALORI)

        Dim tipoUtente As String
        tipoUtente = SP_LOGINVALORI(0).Item("TipoUtente").ToString

        ''Forzatura introdotta il 21/09/2015 da Danilo per gestire utente Read con visualizzazione checklist
        Dim Utente As String
        Utente = SP_LOGINVALORI(0).Item("Utente").ToString

        '---------------------------------------------------------------------------------



        Dim blnCheckList As Boolean
        blnCheckList = False

        Dim blnAutoritàCertificazione As Boolean
        blnAutoritàCertificazione = False



        If tipoUtente = "U" Or tipoUtente = "" Then
            xmlLocal = New XmlDocument

            wsAutenticazioneLocal.Url = AppSettings("URL_WS_Autenticazione")

            xmlLocal.LoadXml(wsAutenticazioneLocal.EseguiAutenticazione(criptString.EncryptData(txtNomeUtente.Text), criptString.EncryptData(txtPassword.Text)))

            Select Case xmlLocal.InnerText
                Case "POSITIVO"
                    Dim strsql As String
                    Dim dtrGenerico As SqlClient.SqlDataReader

                    'If tipoUtente = "U" Then
                    strsql = "select idprofilo from associautentegruppo a inner join utentiunsc b on a.username = b.username where accountad = '" & txtNomeUtente.Text & "' and idprofilo = 71"
                    dtrGenerico = ClsServer.CreaDatareader(strsql, Session("conn"))
                    If dtrGenerico.HasRows = True Then
                        blnCheckList = True
                    End If
                    'End If

                    If Not dtrGenerico Is Nothing Then
                        dtrGenerico.Close()
                        dtrGenerico = Nothing
                    End If

                    strsql = "select idprofilo from associautentegruppo a inner join utentiunsc b on a.username = b.username where accountad = '" & txtNomeUtente.Text & "' and idprofiloread = 75"
                    dtrGenerico = ClsServer.CreaDatareader(strsql, Session("conn"))
                    If dtrGenerico.HasRows = True Then
                        blnAutoritàCertificazione = True
                    End If
                    'End If

                    If Not dtrGenerico Is Nothing Then
                        dtrGenerico.Close()
                        dtrGenerico = Nothing
                    End If

                    strsql = "select username, dbo.formatodata(getdate()) as DataServer from utentiunsc where utenteospite=1"

                    If blnCheckList = True Then
                        strsql = "select username, dbo.formatodata(getdate()) as DataServer from utentiunsc where username = 'UREADCL'"
                    End If

                    If blnAutoritàCertificazione = True Then
                        strsql = "select username, dbo.formatodata(getdate()) as DataServer from utentiunsc where username = 'UREAD_AC'"
                    End If

                    dtrGenerico = ClsServer.CreaDatareader(strsql, Session("conn"))
                    If dtrGenerico.HasRows = True Then
                        dtrGenerico.Read()
                        Session("Utente") = dtrGenerico("username")
                        Session("dataserver") = dtrGenerico("DataServer")
                        'CheckLoginUNSC()
                        Session("Denominazione") = ""
                        Session("idEnte") = "-1"

                        Session("TipoUtente") = "U"
                        Session("IdStatoEnte") = "0"
                        Session("FlagForzatura") = "0"

                        Session("VisualizzaStatoProgetti") = "0"
                        Session("IdRegioneCompetenzaUtente") = "22"
                        Session("Competenza") = "Nazionale"
                        Session("CodiceRegioneEnte") = "[" & "" & "]"
                        Session("txtCodEnte") = ""
                        Session("LogIn") = True

                        If Not dtrGenerico Is Nothing Then
                            dtrGenerico.Close()
                            dtrGenerico = Nothing
                        End If

                        Session("FiltroVisibilita") = FiltroVisibilità("FUTURO", Session("TipoUtente"))

                        ClsServer.RegistrazioneLogAccessi(txtNomeUtente.Text, "LOGIN", 1, Session("conn"))

                        Response.Redirect("WfrmSistema.aspx")

                    End If

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
        End If
        ''wsAutenticazioneLocal.Url = ConfigurationSettings.AppSettings("URL_WS_Autenticazione")
        'If tipoUtente = "R" Or tipoUtente = "E" Then
        '    Session("Denominazione") = SP_LOGINVALORI(0).Item("Denominazione").ToString
        '    Session("idEnte") = SP_LOGINVALORI(0).Item("idEnte").ToString
        '    Session("Utente") = SP_LOGINVALORI(0).Item("Utente").ToString
        '    Session("TipoUtente") = SP_LOGINVALORI(0).Item("TipoUtente").ToString
        '    Session("IdStatoEnte") = SP_LOGINVALORI(0).Item("IdStatoEnte").ToString
        '    Session("FlagForzatura") = SP_LOGINVALORI(0).Item("FlagForzatura").ToString
        '    Session("dataserver") = SP_LOGINVALORI(0).Item("dataserver").ToString
        '    Session("VisualizzaStatoProgetti") = SP_LOGINVALORI(0).Item("VisualizzaStatoProgetti").ToString
        '    Session("IdRegioneCompetenzaUtente") = SP_LOGINVALORI(0).Item("IdRegioneCompetenzaUtente").ToString
        '    Session("Competenza") = SP_LOGINVALORI(0).Item("Competenza").ToString
        '    Session("CodiceRegioneEnte") = "[" & SP_LOGINVALORI(0).Item("CodiceRegioneEnte").ToString & "]"
        '    Session("txtCodEnte") = SP_LOGINVALORI(0).Item("CodiceRegioneEnte").ToString
        '    If SP_LOGINVALORI(0).Item("LogIn") = "1" Then
        '        Session("LogIn") = True
        '    Else
        '        Session("LogIn") = False
        '    End If
        '    Session("FiltroVisibilita") = FiltroVisibilità("FUTURO", Session("TipoUtente"))
        '    'controllo scadenza password
        '    If SP_LOGINVALORI(0).Item("PasswordScaduta").ToString = True Then

        '        Response.Redirect("modificapwd.aspx?operazione=login")
        '    Else

        '        Response.Redirect("WfrmMain.aspx")
        '    End If

        'End If



        If tipoUtente = "R" Then
            ClsServer.RegistrazioneLogAccessi(txtNomeUtente.Text, "LOGIN", 1, Session("conn"))

            Session("Utente") = SP_LOGINVALORI(0).Item("Utente").ToString
            Session("TipoUtente") = SP_LOGINVALORI(0).Item("TipoUtente").ToString

            If SP_LOGINVALORI(0).Item("LogIn") = "1" Then
                Session("LogIn") = True
            Else
                Session("LogIn") = False
            End If
            'Session("FiltroVisibilita") = FiltroVisibilità("FUTURO", Session("TipoUtente"))
            'controllo scadenza password
            If SP_LOGINVALORI(0).Item("PasswordScaduta").ToString = True Then

                Response.Redirect("modificapwd.aspx?operazione=login")
            Else
                'PAGINA DI SCELTA SISTEMA
                Response.Redirect("WfrmSistema.aspx")
                'Response.Redirect("WfrmMain.aspx")
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
    Private Function FiltroVisibilità(ByVal strApplicazione As String, ByVal strTipoUtente As String) As String

        '*** DESCRIZIONE: La function richiama la store SP_FILTROVISIBILITA' che ritorna, a secondo dei parametri indicati 
        '***              in ingresso, il filtro da applicare per la visualizzazione dei progetti SCN o GG

        Dim sqlCMD As New SqlClient.SqlCommand
        Dim strNomeStore As String = "[SP_FiltroVisibilita]"


        Dim hostName As String
        hostName = System.Net.Dns.GetHostName

        Try
            sqlCMD = New SqlClient.SqlCommand(strNomeStore, CType(Session("conn"), SqlClient.SqlConnection))
            sqlCMD.CommandType = CommandType.StoredProcedure
            sqlCMD.Parameters.Add("@NomeServer", SqlDbType.VarChar).Value = hostName
            sqlCMD.Parameters.Add("@Applicazione", SqlDbType.VarChar).Value = strApplicazione
            sqlCMD.Parameters.Add("@TipoUtente", SqlDbType.Char).Value = strTipoUtente

            Dim sparam1 As SqlClient.SqlParameter
            sparam1 = New SqlClient.SqlParameter
            sparam1.ParameterName = "@FiltroMacroTipoProgetto"
            sparam1.Size = 5
            sparam1.SqlDbType = SqlDbType.VarChar
            sparam1.Direction = ParameterDirection.Output
            sqlCMD.Parameters.Add(sparam1)

            sqlCMD.ExecuteScalar()
            Dim str As String
            str = sqlCMD.Parameters("@FiltroMacroTipoProgetto").Value
            Return str
        Catch ex As Exception
            Response.Write(ex.Message.ToString())
            Exit Function
        End Try
    End Function
End Class