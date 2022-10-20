Imports System.Xml
Imports System.Configuration.ConfigurationManager
Imports System.Data.SqlClient
Public Class WfrmSistema
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Session("Sistema") = Nothing
        If Not Session("LogIn") Is Nothing Then
            If Session("LogIn") = False Then 'verifico validità log-in
                Response.Redirect("LogOn.aspx")
            End If
        Else
            Response.Redirect("LogOn.aspx")
        End If
        'If Session("Read") = "0" Then
        '    logoUnico.Visible = True
        'End If
        'If Session("Read") = "1" Then
        '    LogoDiConsultazione.Visible = True
        'End If
        'ImgFuturoAccesso

        Dim Strsql As String
        Dim myDTR As SqlClient.SqlDataReader

        Strsql = "select b.idstatoente as idstatoente, 'Ente' as TipoEnte "
        Strsql = Strsql & " FROM entipassword a "
        Strsql = Strsql & " inner join enti b on a.idente = b.idente "
        Strsql = Strsql & " WHERE  a.username = '" & Session("Utente") & "'"
        Strsql = Strsql & " UNION "
        Strsql = Strsql & "select b.idstatoente as idstatoente, 'Sede' as TipoEnte "
        Strsql = Strsql & " FROM sedipassword a "
        Strsql = Strsql & " inner join entisediattuazioni c on a.identesedeattuazione = c.identesedeattuazione "
        Strsql = Strsql & " inner join enti b on c.identecapofila = b.idente "
        Strsql = Strsql & " WHERE  a.username = '" & Session("Utente") & "'"

        myDTR = ClsServer.CreaDatareader(Strsql, Session("conn"))
        Dim IdStatoEnte As Integer
        Dim TipoEnte As String
        If myDTR.Read = True Then
            IdStatoEnte = myDTR.Item("idstatoente")
            TipoEnte = "Sede" 'myDTR.Item("TipoEnte")
        End If
        'Sistema.Attributes.Add("class", "FadcLogIn")
        If TipoEnte = "Ente" And (IdStatoEnte = 6 Or IdStatoEnte = 8) Then
            ImgFuturoAccesso.Visible = False
            ImgFuturoAccessoGrey.Visible = True
        End If
        If TipoEnte = "Sede" Then
            'ImgHeliosAccesso.Visible = False
            'ImgHeliosAccessoGrey.Visible = True
        End If
        myDTR.Close()
        myDTR = Nothing

        Strsql = "select idprofilo from associautentegruppo a inner join utentiunsc b on a.username = b.username where A.USERNAME = '" & Session("Utente") & "' and idprofiloread = 75"
        myDTR = ClsServer.CreaDatareader(Strsql, Session("conn"))
        If myDTR.HasRows = True Then
            ImgHeliosAccesso.Visible = False
            ImgHeliosAccessoGrey.Visible = True
        End If
        myDTR.Close()
        myDTR = Nothing
    End Sub

    Protected Sub ImgFuturoAccesso_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles ImgFuturoAccesso.Click
        Session("Sistema") = Nothing
        'chiamare store
        Dim SP_LOGINVALORI As DataTable
        Dim SqlCmd As New SqlCommand
        Dim dataAdapter As SqlDataAdapter = New SqlDataAdapter
        SP_LOGINVALORI = New DataTable

        SqlCmd.CommandText = "SP_ACCESSO_SISTEMA_UNICO"
        SqlCmd.CommandType = CommandType.StoredProcedure
        SqlCmd.Connection = Session("conn")
        SqlCmd.Parameters.AddWithValue("@Username", Session("Utente"))
        SqlCmd.Parameters.AddWithValue("@TipoUtente", Session("TipoUtente"))
        dataAdapter.SelectCommand = SqlCmd
        dataAdapter.Fill(SP_LOGINVALORI)

        'Dim tipoUtente As String
        'tipoUtente = SP_LOGINVALORI(0).Item("TipoUtente").ToString

       
        If Session("TipoUtente") = "U" Then
            If (Not Session("idEnte") Is Nothing And Session("idEnte") <> "-1") Then
                Session("FiltroVisibilita") = FiltroVisibilità("FUTURO", Session("TipoUtente"))
                Session("Sistema") = "Futuro"
                Response.Redirect("WfrmMain.aspx?regione=" & (Session("Competenza")))
            Else
                'CheckLoginUNSC()
                Session("LogIn") = True
                Session("Denominazione") = SP_LOGINVALORI(0).Item("Denominazione").ToString
                Session("idEnte") = SP_LOGINVALORI(0).Item("idEnte").ToString
                Session("IdStatoEnte") = SP_LOGINVALORI(0).Item("IdStatoEnte").ToString
                Session("FlagForzatura") = SP_LOGINVALORI(0).Item("FlagForzatura").ToString
                Session("dataserver") = SP_LOGINVALORI(0).Item("dataserver").ToString
                Session("VisualizzaStatoProgetti") = SP_LOGINVALORI(0).Item("VisualizzaStatoProgetti").ToString
                Session("IdRegioneCompetenzaUtente") = SP_LOGINVALORI(0).Item("IdRegioneCompetenzaUtente").ToString
                Session("Competenza") = SP_LOGINVALORI(0).Item("Competenza").ToString
                Session("CodiceRegioneEnte") = "[" & SP_LOGINVALORI(0).Item("CodiceRegioneEnte").ToString & "]"
                Session("txtCodEnte") = SP_LOGINVALORI(0).Item("CodiceRegioneEnte").ToString
                Session("FiltroVisibilita") = FiltroVisibilità("FUTURO", Session("TipoUtente"))
                Session("Sistema") = "Futuro"

                Response.Redirect("WfrmRicercaEnti.aspx")
            End If
            ''CheckLoginUNSC()
            'Session("LogIn") = True
            'Session("Denominazione") = SP_LOGINVALORI(0).Item("Denominazione").ToString
            'Session("idEnte") = SP_LOGINVALORI(0).Item("idEnte").ToString
            'Session("IdStatoEnte") = SP_LOGINVALORI(0).Item("IdStatoEnte").ToString
            'Session("FlagForzatura") = SP_LOGINVALORI(0).Item("FlagForzatura").ToString
            'Session("dataserver") = SP_LOGINVALORI(0).Item("dataserver").ToString
            'Session("VisualizzaStatoProgetti") = SP_LOGINVALORI(0).Item("VisualizzaStatoProgetti").ToString
            'Session("IdRegioneCompetenzaUtente") = SP_LOGINVALORI(0).Item("IdRegioneCompetenzaUtente").ToString
            'Session("Competenza") = SP_LOGINVALORI(0).Item("Competenza").ToString
            'Session("CodiceRegioneEnte") = "[" & SP_LOGINVALORI(0).Item("CodiceRegioneEnte").ToString & "]"
            'Session("txtCodEnte") = SP_LOGINVALORI(0).Item("CodiceRegioneEnte").ToString
            'Session("FiltroVisibilita") = FiltroVisibilità("FUTURO", Session("TipoUtente"))
            'Session("Sistema") = "Futuro"

            'Response.Redirect("WfrmRicercaEnti.aspx")


        End If
            'wsAutenticazioneLocal.Url = ConfigurationSettings.AppSettings("URL_WS_Autenticazione")
            If Session("TipoUtente") = "R" Or Session("TipoUtente") = "E" Then
                Session("Denominazione") = SP_LOGINVALORI(0).Item("Denominazione").ToString
                Session("idEnte") = SP_LOGINVALORI(0).Item("idEnte").ToString
                Session("Utente") = SP_LOGINVALORI(0).Item("Utente").ToString
                Session("TipoUtente") = SP_LOGINVALORI(0).Item("TipoUtente").ToString
                Session("IdStatoEnte") = SP_LOGINVALORI(0).Item("IdStatoEnte").ToString
                Session("FlagForzatura") = SP_LOGINVALORI(0).Item("FlagForzatura").ToString
                Session("dataserver") = SP_LOGINVALORI(0).Item("dataserver").ToString
                Session("VisualizzaStatoProgetti") = SP_LOGINVALORI(0).Item("VisualizzaStatoProgetti").ToString
                Session("IdRegioneCompetenzaUtente") = SP_LOGINVALORI(0).Item("IdRegioneCompetenzaUtente").ToString
                Session("Competenza") = SP_LOGINVALORI(0).Item("Competenza").ToString
                Session("CodiceRegioneEnte") = "[" & SP_LOGINVALORI(0).Item("CodiceRegioneEnte").ToString & "]"
                Session("txtCodEnte") = SP_LOGINVALORI(0).Item("CodiceRegioneEnte").ToString
                Session("Sistema") = "Futuro"

                If SP_LOGINVALORI(0).Item("LogIn") = "1" Then
                    Session("LogIn") = True
                Else
                    Session("LogIn") = False
                End If


                Session("FiltroVisibilita") = FiltroVisibilità("FUTURO", Session("TipoUtente"))
                'controllo scadenza password
                Response.Redirect("WfrmMain.aspx")

            End If

    End Sub

    Protected Sub ImgHeliosAccesso_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles ImgHeliosAccesso.Click
        Session("Sistema") = Nothing

        'chiamare store
        Dim SP_LOGINVALORI As DataTable
        Dim SqlCmd As New SqlCommand
        Dim dataAdapter As SqlDataAdapter = New SqlDataAdapter
        SP_LOGINVALORI = New DataTable

        SqlCmd.CommandText = "SP_ACCESSO_SISTEMA_UNICO"
        SqlCmd.CommandType = CommandType.StoredProcedure
        SqlCmd.Connection = Session("conn")
        SqlCmd.Parameters.AddWithValue("@Username", Session("Utente"))
        SqlCmd.Parameters.AddWithValue("@TipoUtente", Session("TipoUtente"))
        dataAdapter.SelectCommand = SqlCmd
        dataAdapter.Fill(SP_LOGINVALORI)

      

        If Session("TipoUtente") = "U" Then


            'CheckLoginUNSC()
            If (Not Session("idEnte") Is Nothing And Session("idEnte") <> "-1") Then
                Session("FiltroVisibilita") = FiltroVisibilità("HELIOS", Session("TipoUtente"))
                Session("Sistema") = "Helios"
                Response.Redirect("WfrmMain.aspx?regione=" & (Session("Competenza")))
            Else
                Session("Denominazione") = SP_LOGINVALORI(0).Item("Denominazione").ToString
                Session("idEnte") = SP_LOGINVALORI(0).Item("idEnte").ToString
                Session("IdStatoEnte") = SP_LOGINVALORI(0).Item("IdStatoEnte").ToString
                Session("FlagForzatura") = SP_LOGINVALORI(0).Item("FlagForzatura").ToString
                Session("dataserver") = SP_LOGINVALORI(0).Item("dataserver").ToString
                Session("VisualizzaStatoProgetti") = SP_LOGINVALORI(0).Item("VisualizzaStatoProgetti").ToString
                Session("IdRegioneCompetenzaUtente") = SP_LOGINVALORI(0).Item("IdRegioneCompetenzaUtente").ToString
                Session("Competenza") = SP_LOGINVALORI(0).Item("Competenza").ToString
                Session("CodiceRegioneEnte") = "[" & SP_LOGINVALORI(0).Item("CodiceRegioneEnte").ToString & "]"
                Session("txtCodEnte") = SP_LOGINVALORI(0).Item("CodiceRegioneEnte").ToString
                Session("FiltroVisibilita") = FiltroVisibilità("HELIOS", Session("TipoUtente"))
                Session("Sistema") = "Helios"
                Response.Redirect("WfrmRicercaEnti.aspx")
            End If
            'Session("Denominazione") = SP_LOGINVALORI(0).Item("Denominazione").ToString
            'Session("idEnte") = SP_LOGINVALORI(0).Item("idEnte").ToString
            'Session("IdStatoEnte") = SP_LOGINVALORI(0).Item("IdStatoEnte").ToString
            'Session("FlagForzatura") = SP_LOGINVALORI(0).Item("FlagForzatura").ToString
            'Session("dataserver") = SP_LOGINVALORI(0).Item("dataserver").ToString
            'Session("VisualizzaStatoProgetti") = SP_LOGINVALORI(0).Item("VisualizzaStatoProgetti").ToString
            'Session("IdRegioneCompetenzaUtente") = SP_LOGINVALORI(0).Item("IdRegioneCompetenzaUtente").ToString
            'Session("Competenza") = SP_LOGINVALORI(0).Item("Competenza").ToString
            'Session("CodiceRegioneEnte") = "[" & SP_LOGINVALORI(0).Item("CodiceRegioneEnte").ToString & "]"
            'Session("txtCodEnte") = SP_LOGINVALORI(0).Item("CodiceRegioneEnte").ToString
            'Session("FiltroVisibilita") = FiltroVisibilità("HELIOS", Session("TipoUtente"))
            'Session("Sistema") = "Helios"
            'Response.Redirect("WfrmRicercaEnti.aspx")

        End If
        'wsAutenticazioneLocal.Url = ConfigurationSettings.AppSettings("URL_WS_Autenticazione")
        If Session("TipoUtente") = "R" Or Session("TipoUtente") = "E" Then
            Session("Denominazione") = SP_LOGINVALORI(0).Item("Denominazione").ToString
            Session("idEnte") = SP_LOGINVALORI(0).Item("idEnte").ToString
            Session("IdStatoEnte") = SP_LOGINVALORI(0).Item("IdStatoEnte").ToString
            Session("FlagForzatura") = SP_LOGINVALORI(0).Item("FlagForzatura").ToString
            Session("dataserver") = SP_LOGINVALORI(0).Item("dataserver").ToString
            Session("VisualizzaStatoProgetti") = SP_LOGINVALORI(0).Item("VisualizzaStatoProgetti").ToString
            Session("IdRegioneCompetenzaUtente") = SP_LOGINVALORI(0).Item("IdRegioneCompetenzaUtente").ToString
            Session("Competenza") = SP_LOGINVALORI(0).Item("Competenza").ToString
            Session("CodiceRegioneEnte") = "[" & SP_LOGINVALORI(0).Item("CodiceRegioneEnte").ToString & "]"
            Session("txtCodEnte") = SP_LOGINVALORI(0).Item("CodiceRegioneEnte").ToString
            Session("Sistema") = "Helios"


            If SP_LOGINVALORI(0).Item("LogIn") = "1" Then
                Session("LogIn") = True
            Else
                Session("LogIn") = False
            End If



            Session("FiltroVisibilita") = FiltroVisibilità("HELIOS", Session("TipoUtente"))
            'controllo scadenza password
            Response.Redirect("WfrmMain.aspx")

        End If

    End Sub
    Private Function FiltroVisibilità(ByVal strApplicazione As String, ByVal strTipoUtente As String) As String
        '*** CREATA DA SIMONA CORDELLA
        '*** DATA CREAZIONE: 27/11/2014
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

    'Private Sub HplLogOut_Click(sender As Object, e As System.EventArgs) Handles HplLogOut.Click
    '    ClsServer.RegistrazioneLogAccessi(Session("Account"), "LOGOUT", 1, Session("conn"))
    '    If Session("Read") = "0" Then
    '        Response.Redirect("LogOn.aspx?out=1")
    '    Else
    '        Response.Redirect("LogOnRead.aspx?out=1")
    '    End If
    'End Sub
End Class