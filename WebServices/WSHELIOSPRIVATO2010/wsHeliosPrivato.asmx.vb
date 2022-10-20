Imports System.Web.Services
Imports System.Security.Cryptography
Imports System.Data.SqlClient
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Imports CrystalDecisions.CrystalReports
Imports System.IO
Imports System.Web.Mail
Imports System.Configuration
Imports System.Text
Imports System.ComponentModel
Imports System.Net

'Imports iTextSharp.text
'Imports iTextSharp.text.pdf



' Per consentire la chiamata di questo servizio Web dallo script utilizzando ASP.NET AJAX, rimuovere il commento dalla riga seguente.
' <System.Web.Script.Services.ScriptService()> _
<System.Web.Services.WebService(Namespace:="http://tempuri.org/")> _
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<ToolboxItem(False)> _
Public Class wsHeliosPrivato
    Inherits System.Web.Services.WebService

    Private Sub AccessoVolontariHelios(ByRef dtT As DataTable, ByVal strDecriptUser As String, ByVal strDecriptPSW As String)
        Dim sqlConn As New SqlClient.SqlConnection
        Dim strSql As String
        Dim myCommand As New SqlClient.SqlCommand
        Dim dtrLocal As SqlClient.SqlDataReader
        Dim dtR As DataRow
        Dim strCodiceFiscale As String
        Dim intCambioPWD As Integer
        Dim my3Des As New Simple3Des


        Try


            sqlConn.ConnectionString = System.Configuration.ConfigurationManager.AppSettings("cnHelios")
            sqlConn.Open()
            'strSql = "select a.Denominazione, a.Username, a.Password, a.Identificativo,a.IdStatoEnte,getdate() as dataserver, a.Tipo, a.FlagForzaturaAccreditamento, a.CambioPWD, b.codicefiscale from VW_Account_Utenti a inner join entità b on a.identificativo = b.identità where a.tipo = 'V' and a.username='" & strDecriptUser & "' and a.password='" & strDecriptPSW & "' and a.heliosRead=0 "

            strSql = "select a.Denominazione, a.Username, a.Password, a.Identificativo,a.IdStatoEnte,getdate() as dataserver, a.Tipo, a.FlagForzaturaAccreditamento, a.CambioPWD, b.codicefiscale " & _
                    " from VW_Account_Utenti a " & _
                    " inner join entità b on a.identificativo = b.identità " & _
                    " inner join GraduatorieEntità c on b.IDEntità = c.IdEntità " & _
                    " inner join AttivitàSediAssegnazione d on c.IdAttivitàSedeAssegnazione = d.IDAttivitàSedeAssegnazione " & _
                    " inner join attività e on d.IDAttività = e.IDAttività " & _
                    " inner join BandiAttività f on e.IDBandoAttività = f.IdBandoAttività" & _
                    " inner join bando g on f.IdBando = g.IDBando " & _
                    " where a.tipo = 'V' and g.DOL = 0 and a.username='" & strDecriptUser & "' and a.password='" & strDecriptPSW & "' and a.heliosRead=0 "

            myCommand = New SqlClient.SqlCommand
            myCommand.Connection = sqlConn

            'eseguo la query
            myCommand.CommandText = strSql
            dtrLocal = myCommand.ExecuteReader



            'controllo se l'autanticazine è andata a buon fine
            If dtrLocal.HasRows = True Then
                dtrLocal.Read()
                strCodiceFiscale = dtrLocal("CodiceFiscale")
                intCambioPWD = dtrLocal("CambioPWD")
                If Not dtrLocal Is Nothing Then
                    dtrLocal.Close()
                    dtrLocal = Nothing
                End If

                'ESTRAGGO IL CODICE ENTE
                strSql = "Select distinct VW_Account_Utenti.username, dbo.readpassword(VW_Account_Utenti.password) as Password, entità.codicevolontario,entità.codicefiscale,bando.bandobreve ,enti.CodiceRegione,DBO.FN_WS_ABILITA_DOWNLOAD_CONTRATTO(entità.IdEntità) AS ABILITA_CONTRATTO, DBO.FN_WS_ABILITA_DOWNLOAD_CONTRATTO_INTEGRATIVO(entità.IdEntità) AS ABILITA_CONTRATTO_INTEGRATIVO" & _
                         " ,TipiProgetto.NazioneBase, TipiProgetto.MacroTipoProgetto, isnull(bando.Particolarità,0) as Particolarità" & _
                         " FROM  attività INNER JOIN  attivitàentisediattuazione ON attività.IDAttività = attivitàentisediattuazione.IDAttività INNER JOIN " & _
                         " attivitàentità ON attivitàentisediattuazione.IDAttivitàEnteSedeAttuazione = attivitàentità.IDAttivitàEnteSedeAttuazione INNER JOIN  enti ON attività.IDEntePresentante = enti.IDEnte INNER JOIN " & _
                         " entità ON attivitàentità.IDEntità = entità.IDEntità INNER JOIN bandiattività on attività.idbandoattività = bandiattività.idbandoattività inner join bando on bandiattività.idbando = bando.idbando " & _
                         " inner join VW_Account_Utenti on entità.identità = VW_Account_Utenti.identificativo and VW_Account_Utenti.tipo = 'V' " & _
                         " INNER JOIN TipiProgetto on attività.IdTipoProgetto = TipiProgetto.IdTipoProgetto" & _
                         " WHERE entità.codicefiscale = '" & strCodiceFiscale.Replace("'", "''") & "'"
                myCommand = New SqlClient.SqlCommand
                myCommand.Connection = sqlConn
                myCommand.CommandText = strSql
                dtrLocal = myCommand.ExecuteReader

                While dtrLocal.Read()
                    dtR = dtT.NewRow()

                    If intCambioPWD = 0 Then
                        'carico la prima riga della datatable
                        dtR(0) = "CAMBIO PASSWORD"
                    Else
                        dtR(0) = "POSITIVO"
                    End If


                    dtR(1) = dtrLocal("CodiceRegione")
                    dtR(2) = ""
                    dtR(3) = "V"
                    dtR(4) = dtrLocal("CodiceVolontario")
                    dtR(5) = ReadStatoElettorato(dtrLocal("CodiceVolontario"))
                    dtR(6) = ReadStatoQuestionario(dtrLocal("CodiceVolontario"))
                    'verifica abilitazione a scaricare il contratto 0: non abilitato ,1: abilitato
                    If dtrLocal("ABILITA_CONTRATTO") = "OK" Then
                        dtR(7) = "1"
                    Else
                        dtR(7) = "0"
                    End If
                    dtR(8) = dtrLocal("BandoBreve")
                    dtR(9) = dtrLocal("CodiceFiscale")

                    dtR(10) = my3Des.EncryptData(dtrLocal("Username"))
                    dtR(11) = my3Des.EncryptData(dtrLocal("password"))
                    If dtrLocal("ABILITA_CONTRATTO_INTEGRATIVO") = "OK" Then
                        dtR(12) = "1"
                    Else
                        dtR(12) = "0"
                    End If
                    'RITORNO TIPO CONTRATTO
                    If dtrLocal("nazionebase") = 0 Then
                        dtR(13) = "EST"
                    ElseIf dtrLocal("MacroTipoProgetto") = "GG" Then
                        dtR(13) = "GG"
                    ElseIf dtrLocal("Particolarità") = 1 Then
                        dtR(13) = "SCD"
                    Else
                        dtR(13) = "ITA"
                    End If
                    dtT.Rows.Add(dtR)
                End While

            Else 'non autenticato

                dtR = dtT.NewRow()

                'carico la prima riga della datatable
                dtR(0) = "NEGATIVO"
                dtR(5) = "0"
                dtR(6) = "0"
                dtR(7) = "0"
                dtR(8) = ""
                dtR(9) = ""
                dtR(10) = ""
                dtR(11) = ""
                dtR(12) = "0"
                dtR(13) = ""
                dtT.Rows.Add(dtR)
            End If
            'controllo e chiudo se aperto il datareader
            If Not dtrLocal Is Nothing Then
                dtrLocal.Close()
                dtrLocal = Nothing
            End If






        Catch ex As Exception

        End Try
    End Sub

    Private Sub AccessoVolontariDOL(ByRef dtT As DataTable, ByVal strDecriptUser As String, ByVal strDecriptPSW As String)
        Dim sqlConn As New SqlClient.SqlConnection
        Dim strSql As String
        Dim myCommand As New SqlClient.SqlCommand
        Dim dtrLocal As SqlClient.SqlDataReader
        Dim dtR As DataRow

        Dim intCambioPWD As Integer
        Dim my3Des As New Simple3Des


        Try
            Dim x As New Service
            Dim y As ResponseData

            y = x.LoginVolontario(strDecriptUser, strDecriptPSW)

            If y.Success Then
                sqlConn.ConnectionString = System.Configuration.ConfigurationManager.AppSettings("cnHelios")
                sqlConn.Open()

                strSql = "Select distinct VW_Account_Utenti.username, dbo.readpassword(VW_Account_Utenti.password) as Password, entità.codicevolontario,entità.codicefiscale,bando.bandobreve ,enti.CodiceRegione,DBO.FN_WS_ABILITA_DOWNLOAD_CONTRATTO(entità.IdEntità) AS ABILITA_CONTRATTO, DBO.FN_WS_ABILITA_DOWNLOAD_CONTRATTO_INTEGRATIVO(entità.IdEntità) AS ABILITA_CONTRATTO_INTEGRATIVO  " & _
                         " ,TipiProgetto.NazioneBase, TipiProgetto.MacroTipoProgetto, isnull(bando.Particolarità,0) as Particolarità" & _
                         " FROM  attività INNER JOIN  attivitàentisediattuazione ON attività.IDAttività = attivitàentisediattuazione.IDAttività INNER JOIN " & _
                         " attivitàentità ON attivitàentisediattuazione.IDAttivitàEnteSedeAttuazione = attivitàentità.IDAttivitàEnteSedeAttuazione INNER JOIN  enti ON attività.IDEntePresentante = enti.IDEnte INNER JOIN " & _
                         " entità ON attivitàentità.IDEntità = entità.IDEntità INNER JOIN bandiattività on attività.idbandoattività = bandiattività.idbandoattività inner join bando on bandiattività.idbando = bando.idbando " & _
                         " inner join VW_Account_Utenti on entità.identità = VW_Account_Utenti.identificativo and VW_Account_Utenti.tipo = 'V' LEFT JOIN DOL_CF_Alias on entità.CodiceFiscale = DOL_CF_Alias.CFSUSC " & _
                         " INNER JOIN TipiProgetto on attività.IdTipoProgetto = TipiProgetto.IdTipoProgetto" & _
                         " WHERE case isnull(DOL_CF_Alias.CFDOL,'') when '' then entità.codicefiscale else DOL_CF_Alias.CFDOL end = left('" & strDecriptUser.Replace("'", "''") & "',16)"
                myCommand = New SqlClient.SqlCommand
                myCommand.Connection = sqlConn
                myCommand.CommandText = strSql
                dtrLocal = myCommand.ExecuteReader

                If dtrLocal.HasRows() Then
                    While dtrLocal.Read()
                        dtR = dtT.NewRow()


                        dtR(0) = "POSITIVO"

                        dtR(1) = dtrLocal("CodiceRegione")
                        dtR(2) = ""
                        dtR(3) = "V"
                        dtR(4) = dtrLocal("CodiceVolontario")
                        dtR(5) = ReadStatoElettorato(dtrLocal("CodiceVolontario"))
                        dtR(6) = ReadStatoQuestionario(dtrLocal("CodiceVolontario"))
                        'verifica abilitazione a scaricare il contratto 0: non abilitato ,1: abilitato
                        If dtrLocal("ABILITA_CONTRATTO") = "OK" Then
                            dtR(7) = "1"
                        Else
                            dtR(7) = "0"
                        End If
                        dtR(8) = dtrLocal("BandoBreve")
                        dtR(9) = dtrLocal("CodiceFiscale")

                        dtR(10) = my3Des.EncryptData(dtrLocal("Username"))
                        dtR(11) = my3Des.EncryptData(dtrLocal("password"))
                        If dtrLocal("ABILITA_CONTRATTO_INTEGRATIVO") = "OK" Then
                            dtR(12) = "1"
                        Else
                            dtR(12) = "0"
                        End If
                        'RITORNO TIPO CONTRATTO
                        If dtrLocal("nazionebase") = 0 Then
                            dtR(13) = "EST"
                        ElseIf dtrLocal("MacroTipoProgetto") = "GG" Then
                            dtR(13) = "GG"
                        ElseIf dtrLocal("Particolarità") = 1 Then
                            dtR(13) = "SCD"
                        Else
                            dtR(13) = "ITA"
                        End If
                        dtT.Rows.Add(dtR)
                    End While
                Else
                    dtR = dtT.NewRow()

                    'carico la prima riga della datatable
                    dtR(0) = "NEGATIVO"
                    dtR(5) = "0"
                    dtR(6) = "0"
                    dtR(7) = "0"
                    dtR(8) = ""
                    dtR(9) = ""
                    dtR(10) = ""
                    dtR(11) = ""
                    dtR(12) = "0"
                    dtR(13) = ""
                    dtT.Rows.Add(dtR)
                End If




            Else
                dtR = dtT.NewRow()

                'carico la prima riga della datatable
                dtR(0) = "NEGATIVO"
                dtR(5) = "0"
                dtR(6) = "0"
                dtR(7) = "0"
                dtR(8) = ""
                dtR(9) = ""
                dtR(10) = ""
                dtR(11) = ""
                dtR(12) = "0"
                dtR(13) = ""
                dtT.Rows.Add(dtR)
            End If

               

            'controllo e chiudo se aperto il datareader
            If Not dtrLocal Is Nothing Then
                dtrLocal.Close()
                dtrLocal = Nothing
            End If
            sqlConn.Close()

        Catch ex As Exception

        End Try
    End Sub

    Private Sub AccessoVolontariSPID(ByRef dtT As DataTable, ByVal strToken As String)
        Dim sqlConn As New SqlClient.SqlConnection
        Dim strSql As String
        Dim myCommand As New SqlClient.SqlCommand
        Dim dtrLocal As SqlClient.SqlDataReader
        Dim dtR As DataRow
        Dim strEsito As String
        Dim intCambioPWD As Integer
        Dim my3Des As New Simple3Des


        Try
            Dim x As New VerificaTokenSPID.ServiceClient

            strEsito = x.getUserCF(strToken)

            If Left(strEsito, 1) = "-" Then
                dtR = dtT.NewRow()

                'carico la prima riga della datatable
                dtR(0) = "NEGATIVO"
                dtR(5) = "0"
                dtR(6) = "0"
                dtR(7) = "0"
                dtR(8) = ""
                dtR(9) = ""
                dtR(10) = ""
                dtR(11) = ""
                dtR(12) = "0"
                dtR(13) = ""
                dtT.Rows.Add(dtR)
            Else
                sqlConn.ConnectionString = System.Configuration.ConfigurationManager.AppSettings("cnHelios")
                sqlConn.Open()

                strSql = "Select distinct VW_Account_Utenti.username, dbo.readpassword(VW_Account_Utenti.password) as Password, entità.codicevolontario,entità.codicefiscale,bando.bandobreve ,enti.CodiceRegione,DBO.FN_WS_ABILITA_DOWNLOAD_CONTRATTO(entità.IdEntità) AS ABILITA_CONTRATTO, DBO.FN_WS_ABILITA_DOWNLOAD_CONTRATTO_INTEGRATIVO(entità.IdEntità) AS ABILITA_CONTRATTO_INTEGRATIVO  " & _
                         " ,TipiProgetto.NazioneBase, TipiProgetto.MacroTipoProgetto, isnull(bando.Particolarità,0) as Particolarità" & _
                         " FROM  attività INNER JOIN  attivitàentisediattuazione ON attività.IDAttività = attivitàentisediattuazione.IDAttività INNER JOIN " & _
                         " attivitàentità ON attivitàentisediattuazione.IDAttivitàEnteSedeAttuazione = attivitàentità.IDAttivitàEnteSedeAttuazione INNER JOIN  enti ON attività.IDEntePresentante = enti.IDEnte INNER JOIN " & _
                         " entità ON attivitàentità.IDEntità = entità.IDEntità INNER JOIN bandiattività on attività.idbandoattività = bandiattività.idbandoattività inner join bando on bandiattività.idbando = bando.idbando " & _
                         " inner join VW_Account_Utenti on entità.identità = VW_Account_Utenti.identificativo and VW_Account_Utenti.tipo = 'V' " & _
                         " INNER JOIN TipiProgetto on attività.IdTipoProgetto = TipiProgetto.IdTipoProgetto" & _
                         " WHERE entità.codicefiscale = '" & strEsito.Replace("'", "''") & "'"
                myCommand = New SqlClient.SqlCommand
                myCommand.Connection = sqlConn
                myCommand.CommandText = strSql
                dtrLocal = myCommand.ExecuteReader

                If dtrLocal.HasRows() Then
                    While dtrLocal.Read()
                        dtR = dtT.NewRow()


                        dtR(0) = "POSITIVO"

                        dtR(1) = dtrLocal("CodiceRegione")
                        dtR(2) = ""
                        dtR(3) = "V"
                        dtR(4) = dtrLocal("CodiceVolontario")
                        dtR(5) = ReadStatoElettorato(dtrLocal("CodiceVolontario"))
                        dtR(6) = ReadStatoQuestionario(dtrLocal("CodiceVolontario"))
                        'verifica abilitazione a scaricare il contratto 0: non abilitato ,1: abilitato
                        If dtrLocal("ABILITA_CONTRATTO") = "OK" Then
                            dtR(7) = "1"
                        Else
                            dtR(7) = "0"
                        End If
                        dtR(8) = dtrLocal("BandoBreve")
                        dtR(9) = dtrLocal("CodiceFiscale")

                        dtR(10) = my3Des.EncryptData(dtrLocal("Username"))
                        dtR(11) = my3Des.EncryptData(dtrLocal("password"))
                        If dtrLocal("ABILITA_CONTRATTO_INTEGRATIVO") = "OK" Then
                            dtR(12) = "1"
                        Else
                            dtR(12) = "0"
                        End If
                        'RITORNO TIPO CONTRATTO
                        If dtrLocal("nazionebase") = 0 Then
                            dtR(13) = "EST"
                        ElseIf dtrLocal("MacroTipoProgetto") = "GG" Then
                            dtR(13) = "GG"
                        ElseIf dtrLocal("Particolarità") = 1 Then
                            dtR(13) = "SCD"
                        Else
                            dtR(13) = "ITA"
                        End If
                        dtT.Rows.Add(dtR)
                    End While
                Else
                    dtR = dtT.NewRow()

                    'carico la prima riga della datatable
                    dtR(0) = "NEGATIVO"
                    dtR(5) = "0"
                    dtR(6) = "0"
                    dtR(7) = "0"
                    dtR(8) = ""
                    dtR(9) = ""
                    dtR(10) = ""
                    dtR(11) = ""
                    dtR(12) = "0"
                    dtR(13) = ""
                    dtT.Rows.Add(dtR)
                End If
            End If


            'controllo e chiudo se aperto il datareader
            If Not dtrLocal Is Nothing Then
                dtrLocal.Close()
                dtrLocal = Nothing
            End If
            sqlConn.Close()

        Catch ex As Exception

        End Try
    End Sub
    <WebMethod()> _
    Public Function EseguiAutenticazioneVolontario_NEW(ByVal strTipoAutenticazione As String, ByVal strToken As String, ByVal strUserid As String, ByVal strPassword As String) As String
        Dim wrapper As New Simple3Des
        Dim strSql As String
        Dim sqlConn As New SqlClient.SqlConnection
        Dim xmlLocal As String
        Dim dtR As DataRow
        Dim dtT As New DataTable
        Dim dtsLocal As New DataSet
        Dim myCommand As New SqlClient.SqlCommand
        Dim dtrLocal As SqlClient.SqlDataReader




        'Dim strDecriptUser As String = strUserName.ToString.Replace("'", "''")
        'Dim strDecriptPSW As String = CreatePSW(strPWD).ToString.Replace("'", "''")
        Try

            dtT.Columns.Add(New DataColumn("Esito", GetType(String)))                       '0
            dtT.Columns.Add(New DataColumn("CodiceEnte", GetType(String)))                  '1
            dtT.Columns.Add(New DataColumn("Competenza", GetType(String)))                  '2
            dtT.Columns.Add(New DataColumn("TipoUtente", GetType(String)))                  '3
            dtT.Columns.Add(New DataColumn("CodiceVolontario", GetType(String)))            '4
            dtT.Columns.Add(New DataColumn("Elettorato", GetType(String)))                  '5
            dtT.Columns.Add(New DataColumn("QuestionarioFineServizio", GetType(String)))    '6
            dtT.Columns.Add(New DataColumn("ContrattoVolontario", GetType(String)))         '7
            dtT.Columns.Add(New DataColumn("DescrizioneBando", GetType(String)))            '8
            dtT.Columns.Add(New DataColumn("CodiceFiscale", GetType(String)))               '9
            dtT.Columns.Add(New DataColumn("UsernameCrypt", GetType(String)))               '10
            dtT.Columns.Add(New DataColumn("PasswordCrypt", GetType(String)))               '11
            dtT.Columns.Add(New DataColumn("ContrattoIntegrativo", GetType(String)))         '12
            dtT.Columns.Add(New DataColumn("TipoContratto", GetType(String)))                '13

            Select Case strTipoAutenticazione
                Case "1" 'CREDENZIALI HELIOS
                    Dim strDecriptUser As String = wrapper.DecryptData(strUserid).ToString.Replace("'", "''")
                    Dim strDecriptPSW As String = CreatePSW(wrapper.DecryptData(strPassword)).ToString.Replace("'", "''")
                    AccessoVolontariHelios(dtT, strDecriptUser, strDecriptPSW)

                Case "2" 'CREDENZIALI DOL
                    Dim strDecriptUser As String = wrapper.DecryptData(strUserid).ToString
                    Dim strDecriptPSW As String = wrapper.DecryptData(strPassword).ToString

                    AccessoVolontariDOL(dtT, strDecriptUser, strDecriptPSW)

                  

                Case "3" 'CREDENZIALI SPID
                    AccessoVolontariSPID(dtT, strToken)
                   

                Case Else
                    dtR = dtT.NewRow()
                    dtR(0) = "NEGATIVO"
                    dtR(5) = "0"
                    dtR(6) = "0"
                    dtR(7) = "0"
                    dtR(12) = "0"
                    dtR(13) = ""
                    dtT.Rows.Add(dtR)
            End Select






            dtsLocal.Tables.Add(dtT)

            dtsLocal.DataSetName = "EsitoAutenticazione"

            dtsLocal.Tables(0).TableName = "DettaglioEsito"

            xmlLocal = dtsLocal.GetXml

            EseguiAutenticazioneVolontario_NEW = xmlLocal

            sqlConn.Close()

            Return EseguiAutenticazioneVolontario_NEW

        Catch ex As Exception
            Dim myLog As New GeneraLog(ex.Message, "EseguiAutenticazioneVolontario_NEW")
            myLog.GeneraFileLog()
        End Try
    End Function

    <WebMethod()> _
    Public Function EseguiModificaPasswordDOL(ByVal strUsernameCrypt As String, ByVal strPasswordCrypt As String, ByVal strVecchiaPassword As String, ByVal strNuovaPassword As String) As String
        Dim wrapper As New Simple3Des
        Dim strSql, strSqlElez As String
        Dim sqlConn As New SqlClient.SqlConnection
        Dim sqlConnElez As New SqlClient.SqlConnection
        Dim xmlLocal As String
        Dim dtR As DataRow
        Dim dtT As New DataTable
        Dim dtsLocal As New DataSet
        Dim myCommand As New SqlClient.SqlCommand
        Dim myCommandElez As New SqlClient.SqlCommand
        Dim dtrLocal As SqlClient.SqlDataReader
        Dim strTipoUtente As String
        Dim intIdEntita As Integer

        Try
            'strVecchiaPWD = wrapper.EncryptData(strVecchiaPWD)
            'strNuovaPWD = wrapper.EncryptData(strNuovaPWD)
            'strUserName = wrapper.EncryptData(strUserName)

            'aggiungo cmq la colonna dell'esito
            dtT.Columns.Add(New DataColumn("Esito", GetType(String)))
            dtT.Columns.Add(New DataColumn("CausaleEsitoNegativo", GetType(String)))

            If strUsernameCrypt = "" Or strPasswordCrypt = "" Or strVecchiaPassword = "" Or strNuovaPassword = "" Then
                'dtT.Columns.Add()
                'alla riga assegno la riga delle intestazioni appena creata
                dtR = dtT.NewRow()

                'carico la prima riga della datatable
                dtR(0) = "NEGATIVO"
                dtR(1) = "TUTTI I CAMPI SONO NECESSARI"

                'aggiungo la riga appena caricata alla datatable
                dtT.Rows.Add(dtR)

                dtsLocal.Tables.Add(dtT)

                dtsLocal.DataSetName = "EsitoModificaPasswordDOL"

                dtsLocal.Tables(0).TableName = "DettaglioEsito"

                xmlLocal = dtsLocal.GetXml

                EseguiModificaPasswordDOL = xmlLocal

                Return EseguiModificaPasswordDOL
            Else
                If wrapper.DecryptData(strUsernameCrypt) = "" Or wrapper.DecryptData(strPasswordCrypt) = "" Or wrapper.DecryptData(strVecchiaPassword) = "" Or wrapper.DecryptData(strNuovaPassword) = "" Then
                    'dtT.Columns.Add()
                    'alla riga assegno la riga delle intestazioni appena creata
                    dtR = dtT.NewRow()

                    'carico la prima riga della datatable
                    dtR(0) = "NEGATIVO"
                    dtR(1) = "TUTTI I CAMPI SONO NECESSARI"

                    'aggiungo la riga appena caricata alla datatable
                    dtT.Rows.Add(dtR)

                    dtsLocal.Tables.Add(dtT)

                    dtsLocal.DataSetName = "EsitoModificaPasswordDOL"

                    dtsLocal.Tables(0).TableName = "DettaglioEsito"

                    xmlLocal = dtsLocal.GetXml

                    EseguiModificaPasswordDOL = xmlLocal

                    Return EseguiModificaPasswordDOL
                End If
            End If

            'sqlConn.ConnectionString = "user id=sa;password=vbra250;data source=SQLHELIOS;persist security info=False;connect timeout=300;Max Pool Size=3000;initial catalog=unscproduzione"
            sqlConn.ConnectionString = System.Configuration.ConfigurationManager.AppSettings("cnHelios")

            sqlConn.Open()

            strSql = "select b.codicefiscale from VW_Account_Utenti a inner join entità b on a.identificativo = b.identità where a.username='" & wrapper.DecryptData(strUsernameCrypt) & "' and a.password='" & CreatePSW(wrapper.DecryptData(strPasswordCrypt)).Replace("'", "''") & "' and a.heliosRead=0 "

            'sql command che mi esegue la insert
            myCommand = New SqlClient.SqlCommand
            myCommand.Connection = sqlConn

            'controllo e chiudo se aperto il datareader
            If Not dtrLocal Is Nothing Then
                dtrLocal.Close()
                dtrLocal = Nothing
            End If

            'eseguo la query
            myCommand.CommandText = strSql
            dtrLocal = myCommand.ExecuteReader

            'se l'account esiste vado a controllare la correttezza della nuova pwd
            If dtrLocal.HasRows = True Then
                Try
                    dtrLocal.Read()

                    'CHIAMARE SERVIZIO REST DOL (username dol = cf volontario) 
                    Dim x As New Service
                    Dim y As ResponseData

                    y = x.CambioPassword(dtrLocal("codicefiscale").ToString, wrapper.DecryptData(strVecchiaPassword), wrapper.DecryptData(strNuovaPassword))




                    dtR = dtT.NewRow()


                    If y.Success Then 'ESITO SERVIZIO REST DOL
                        dtR(0) = "POSITIVO"
                        dtR(1) = ""
                    Else
                        dtR(0) = "NEGATIVO"
                        dtR(1) = y.Message
                    End If

                Catch ex As Exception
                    Dim myLog As New GeneraLog(ex.Message, "EseguiModificaPasswordDOL - V")
                    myLog.GeneraFileLog()

                    If dtR Is Nothing Then
                        dtR = dtT.NewRow()
                    End If

                    'carico la prima riga della datatable
                    dtR(0) = "NEGATIVO"
                    dtR(1) = "ERRORE: " & ex.Message
                Finally
                    'controllo e chiudo se aperto il datareader
                    If Not dtrLocal Is Nothing Then
                        dtrLocal.Close()
                        dtrLocal = Nothing
                    End If

                    'aggiungo la riga appena caricata alla datatable
                    dtT.Rows.Add(dtR)

                    dtsLocal.Tables.Add(dtT)

                    dtsLocal.DataSetName = "EsitoModificaPasswordDOL"

                    dtsLocal.Tables(0).TableName = "DettaglioEsito"

                    xmlLocal = dtsLocal.GetXml

                    EseguiModificaPasswordDOL = xmlLocal

                    'chiudo le connessioni se serve
                    If sqlConn.State = ConnectionState.Open Then
                        sqlConn.Close()
                    End If

                    If sqlConnElez.State = ConnectionState.Open Then
                        sqlConnElez.Close()
                    End If

                End Try

                Return EseguiModificaPasswordDOL


            Else 'se l'account inserito non esiste rimando un messaggio di esito negativo
                'controllo e chiudo se aperto il datareader
                If Not dtrLocal Is Nothing Then
                    dtrLocal.Close()
                    dtrLocal = Nothing
                End If

                'dtT.Columns.Add()
                'alla riga assegno la riga delle intestazioni appena creata
                dtR = dtT.NewRow()

                'carico la prima riga della datatable
                dtR(0) = "NEGATIVO"
                dtR(1) = "ERRORE DI AUTENTICAZIONE DELL'ACCOUNT GESTIONALE"

                'aggiungo la riga appena caricata alla datatable
                dtT.Rows.Add(dtR)

                dtsLocal.Tables.Add(dtT)

                dtsLocal.DataSetName = "EsitoModificaPasswordDOL"

                dtsLocal.Tables(0).TableName = "DettaglioEsito"

                xmlLocal = dtsLocal.GetXml

                EseguiModificaPasswordDOL = xmlLocal

                sqlConn.Close()

                Return EseguiModificaPasswordDOL

            End If

            'controllo e chiudo se aperto il datareader
            If Not dtrLocal Is Nothing Then
                dtrLocal.Close()
                dtrLocal = Nothing
            End If

            Return EseguiModificaPasswordDOL
        Catch ex As Exception
            Dim myLog As New GeneraLog(ex.Message, "EseguiModificaPasswordDOL")
            myLog.GeneraFileLog()
        End Try
    End Function

    <WebMethod()> _
    Public Function EseguiResetPasswordDOL(ByVal strCodiceFiscale As String, ByVal strReturnUrl As String) As String
        Dim wrapper As New Simple3Des
        Dim xmlLocal As String
        Dim dtR As DataRow
        Dim dtT As New DataTable
        Dim dtsLocal As New DataSet


        Try

            'aggiungo cmq la colonna dell'esito
            dtT.Columns.Add(New DataColumn("Esito", GetType(String)))
            dtT.Columns.Add(New DataColumn("CausaleEsitoNegativo", GetType(String)))

            If strCodiceFiscale = "" Or strReturnUrl = "" Then
                'dtT.Columns.Add()
                'alla riga assegno la riga delle intestazioni appena creata
                dtR = dtT.NewRow()

                'carico la prima riga della datatable
                dtR(0) = "NEGATIVO"
                dtR(1) = "TUTTI I CAMPI SONO NECESSARI"

                'aggiungo la riga appena caricata alla datatable
                dtT.Rows.Add(dtR)

                dtsLocal.Tables.Add(dtT)

                dtsLocal.DataSetName = "EsitoResetPasswordDOL"

                dtsLocal.Tables(0).TableName = "DettaglioEsito"

                xmlLocal = dtsLocal.GetXml

                EseguiResetPasswordDOL = xmlLocal

                Return EseguiResetPasswordDOL
            Else
                If wrapper.DecryptData(strCodiceFiscale) = "" Then
                    'dtT.Columns.Add()
                    'alla riga assegno la riga delle intestazioni appena creata
                    dtR = dtT.NewRow()

                    'carico la prima riga della datatable
                    dtR(0) = "NEGATIVO"
                    dtR(1) = "TUTTI I CAMPI SONO NECESSARI"

                    'aggiungo la riga appena caricata alla datatable
                    dtT.Rows.Add(dtR)

                    dtsLocal.Tables.Add(dtT)

                    dtsLocal.DataSetName = "EsitoResetPasswordDOL"

                    dtsLocal.Tables(0).TableName = "DettaglioEsito"

                    xmlLocal = dtsLocal.GetXml

                    EseguiResetPasswordDOL = xmlLocal

                    Return EseguiResetPasswordDOL
                End If
            End If

            Try

                'CHIAMARE SERVIZIO REST DOL (username dol = cf volontario) 
                Dim x As New Service
                Dim y As ResponseData

                y = x.RecuperoPassword(wrapper.DecryptData(strCodiceFiscale), strReturnUrl)

                dtR = dtT.NewRow()

                If y.Success Then 'ESITO SERVIZIO REST DOL
                    dtR(0) = "POSITIVO"
                    dtR(1) = ""
                Else
                    dtR(0) = "NEGATIVO"
                    dtR(1) = y.Message 'MESSAGGIO ESITO NEGATIVO DOL
                End If

            Catch ex As Exception
                Dim myLog As New GeneraLog(ex.Message, "EseguiResetPasswordDOL - V")
                myLog.GeneraFileLog()

                If dtR Is Nothing Then
                    dtR = dtT.NewRow()
                End If

                'carico la prima riga della datatable
                dtR(0) = "NEGATIVO"
                dtR(1) = "ERRORE: " & ex.Message
            Finally

                'aggiungo la riga appena caricata alla datatable
                dtT.Rows.Add(dtR)

                dtsLocal.Tables.Add(dtT)

                dtsLocal.DataSetName = "EsitoResetPasswordDOL"

                dtsLocal.Tables(0).TableName = "DettaglioEsito"

                xmlLocal = dtsLocal.GetXml

                EseguiResetPasswordDOL = xmlLocal

            End Try

            Return EseguiResetPasswordDOL



        Catch ex As Exception
            Dim myLog As New GeneraLog(ex.Message, "EseguiResetPasswordDOL")
            myLog.GeneraFileLog()
        End Try
    End Function

    <WebMethod()> _
    Public Function EseguiRicercaEnti(ByVal strRegione As String, ByVal strProvincia As String, ByVal strComune As String, ByVal strTipoEnte As String, ByVal strCompentenza As String, ByVal strClasse As String, ByVal strDenominazioneEnte As String, ByVal strCodiceEnte As String) As String
        Dim strSql As String
        Dim sqlConn As New SqlClient.SqlConnection
        Dim dtsLocal As New DataSet
        Dim xmlLocal As String
        Dim strFiltri As String = ""

        Try

            sqlConn.ConnectionString = System.Configuration.ConfigurationManager.AppSettings("cnHelios")

            sqlConn.Open()

            If strRegione <> "" Then
                strFiltri = strFiltri & " AND REGIONE='" & Replace(strRegione, "'", "''") & "'"
            End If

            If strProvincia <> "" Then
                strFiltri = strFiltri & " AND PROVINCIA='" & Replace(strProvincia, "'", "''") & "'"
            End If

            If strComune <> "" Then
                strFiltri = strFiltri & " AND COMUNE='" & Replace(strComune, "'", "''") & "'"
            End If
            'modificato il 22/05/2015 da simona cordella
            '	Se riceve come valore PRIVATO allora effettua la ricerca degli enti con Tipologia = ‘PRIVATO’
            '	Se riceve come valore PUBBLICO allora effettua la ricerca degli enti con Tipologia <> ‘PRIVATO’
            If Trim(strTipoEnte) <> "" Then
                If UCase(Trim(strTipoEnte)) = "PRIVATO" Then
                    strFiltri = strFiltri & " AND TIPOENTE='" & Replace(UCase(Trim(strTipoEnte)), "'", "''") & "'"
                Else
                    strFiltri = strFiltri & " AND TIPOENTE<>'PRIVATO'"
                End If
            End If

            'If strTipoEnte <> "" Then
            '    strFiltri = strFiltri & " AND TIPOENTE='" & Replace(strTipoEnte, "'", "''") & "'"
            'End If

            If strClasse <> "" Then
                strFiltri = strFiltri & " AND CLASSE='" & Replace(strClasse, "'", "''") & "'"
            End If


            If strCodiceEnte <> "" Then
                strFiltri = strFiltri & " AND CODICEENTE='" & Replace(strCodiceEnte, "'", "''") & "'"
            End If

            If strCompentenza <> "" Then
                strFiltri = strFiltri & " AND COMPETENZA='" & Replace(strCompentenza, "'", "''") & "'"
            End If

            If strDenominazioneEnte <> "" Then
                strFiltri = strFiltri & " AND DENOMINAZIONEENTE LIKE '%" & Replace(strDenominazioneEnte, "'", "''") & "%'"
            End If

            strSql = "SELECT DISTINCT DENOMINAZIONEENTE, TIPOENTE, SITO, Email, CLASSE, CODICEENTE, TELEFONO, COMPETENZA FROM VW_WS_RICERCAENTI WHERE 1=1 " & strFiltri

            Dim CMD As New SqlClient.SqlDataAdapter(strSql, sqlConn)

            dtsLocal.DataSetName = "Enti"

            'dtsLocal.Tables(0).TableName = "ENTI"

            CMD.Fill(dtsLocal)

            dtsLocal.Tables(0).TableName = "DettaglioEnte"

            xmlLocal = dtsLocal.GetXml

            EseguiRicercaEnti = xmlLocal

            sqlConn.Close()

            Return EseguiRicercaEnti
        Catch ex As Exception
            Dim myLog As New GeneraLog(ex.Message, "EseguiRicercaEnti")
            myLog.GeneraFileLog()
        End Try
    End Function

    <WebMethod()> _
    Public Function EseguiRicercaEntiSCU(ByVal strRegione As String, ByVal strProvincia As String, ByVal strComune As String, ByVal strTipoEnte As String, ByVal strCompentenza As String, ByVal strClasse As String, ByVal strDenominazioneEnte As String, ByVal strCodiceEnte As String) As String
        Dim strSql As String
        Dim sqlConn As New SqlClient.SqlConnection
        Dim dtsLocal As New DataSet
        Dim xmlLocal As String
        Dim strFiltri As String = ""

        Try

            sqlConn.ConnectionString = System.Configuration.ConfigurationManager.AppSettings("cnHelios")

            sqlConn.Open()

            If strRegione <> "" Then
                strFiltri = strFiltri & " AND REGIONE='" & Replace(strRegione, "'", "''") & "'"
            End If

            If strProvincia <> "" Then
                strFiltri = strFiltri & " AND PROVINCIA='" & Replace(strProvincia, "'", "''") & "'"
            End If

            If strComune <> "" Then
                strFiltri = strFiltri & " AND COMUNE='" & Replace(strComune, "'", "''") & "'"
            End If
            'modificato il 22/05/2015 da simona cordella
            '	Se riceve come valore PRIVATO allora effettua la ricerca degli enti con Tipologia = ‘PRIVATO’
            '	Se riceve come valore PUBBLICO allora effettua la ricerca degli enti con Tipologia <> ‘PRIVATO’
            If Trim(strTipoEnte) <> "" Then
                If UCase(Trim(strTipoEnte)) = "PRIVATO" Then
                    strFiltri = strFiltri & " AND TIPOENTE='" & Replace(UCase(Trim(strTipoEnte)), "'", "''") & "'"
                Else
                    strFiltri = strFiltri & " AND TIPOENTE<>'PRIVATO'"
                End If
            End If

            'If strTipoEnte <> "" Then
            '    strFiltri = strFiltri & " AND TIPOENTE='" & Replace(strTipoEnte, "'", "''") & "'"
            'End If

            If strClasse <> "" Then
                strFiltri = strFiltri & " AND CLASSE='" & Replace(strClasse, "'", "''") & "'"
            End If


            If strCodiceEnte <> "" Then
                strFiltri = strFiltri & " AND CODICEENTE='" & Replace(strCodiceEnte, "'", "''") & "'"
            End If

            If strCompentenza <> "" Then
                strFiltri = strFiltri & " AND COMPETENZA='" & Replace(strCompentenza, "'", "''") & "'"
            End If

            If strDenominazioneEnte <> "" Then
                strFiltri = strFiltri & " AND DENOMINAZIONEENTE LIKE '%" & Replace(strDenominazioneEnte, "'", "''") & "%'"
            End If

            strSql = "SELECT DISTINCT DENOMINAZIONEENTE, TIPOENTE, SITO, Email, CLASSE, CODICEENTE, TELEFONO, COMPETENZA FROM VW_WS_RICERCAENTI_SCU WHERE 1=1 " & strFiltri

            Dim CMD As New SqlClient.SqlDataAdapter(strSql, sqlConn)

            dtsLocal.DataSetName = "Enti"

            'dtsLocal.Tables(0).TableName = "ENTI"

            CMD.Fill(dtsLocal)

            dtsLocal.Tables(0).TableName = "DettaglioEnte"

            xmlLocal = dtsLocal.GetXml

            EseguiRicercaEntiSCU = xmlLocal

            sqlConn.Close()

            Return EseguiRicercaEntiSCU
        Catch ex As Exception
            Dim myLog As New GeneraLog(ex.Message, "EseguiRicercaEntiSCU")
            myLog.GeneraFileLog()
        End Try
    End Function

    <WebMethod()> _
    Public Function EseguiRicercaEntiSCU_NEW(ByVal strLocalizzazione As String, ByVal strNazione As String, ByVal strRegione As String, ByVal strProvincia As String, ByVal strComune As String, ByVal strSezione As String, ByVal strDenominazioneEnte As String, ByVal strCodiceEnte As String, ByVal strTitolareAccoglienza As String) As String
        Dim strSql As String
        Dim sqlConn As New SqlClient.SqlConnection
        Dim dtsLocal As New DataSet
        Dim xmlLocal As String
        Dim strFiltri As String = ""

        Try

            sqlConn.ConnectionString = System.Configuration.ConfigurationManager.AppSettings("cnHelios")

            sqlConn.Open()

            If strNazione <> "" Then
                strFiltri = strFiltri & " AND NAZIONE='" & Replace(strNazione, "'", "''") & "'"
            End If

            If strRegione <> "" Then
                strFiltri = strFiltri & " AND REGIONE='" & Replace(strRegione, "'", "''") & "'"
            End If

            If strProvincia <> "" Then
                strFiltri = strFiltri & " AND PROVINCIA='" & Replace(strProvincia, "'", "''") & "'"
            End If

            If strComune <> "" Then
                strFiltri = strFiltri & " AND COMUNE='" & Replace(strComune, "'", "''") & "'"
            End If

            If strSezione <> "" Then
                If strSezione = "SCU - Sezione RPA" Then
                    strFiltri = strFiltri & " AND SEZIONE LIKE 'SCU - Sezione RPA%'"
                Else
                    strFiltri = strFiltri & " AND SEZIONE='" & Replace(strSezione, "'", "''") & "'"
                End If
            End If

            If strTitolareAccoglienza <> "" Then
                strFiltri = strFiltri & " AND LIVELLO='" & Replace(strTitolareAccoglienza, "'", "''") & "'"
            End If

            If strCodiceEnte <> "" Then
                strFiltri = strFiltri & " AND CODICEENTE='" & Replace(strCodiceEnte, "'", "''") & "'"
            End If


            If strDenominazioneEnte <> "" Then
                strFiltri = strFiltri & " AND DENOMINAZIONEENTE LIKE '%" & Replace(strDenominazioneEnte, "'", "''") & "%'"
            End If

            If strLocalizzazione <> "" Then
                strFiltri = strFiltri & " AND LOCALIZZAZIONE='" & Replace(strLocalizzazione, "'", "''") & "'"
            End If

            If strFiltri = "" Then 'nessun filtro indicato quindi estraggo solo gli enti titolari
                strFiltri = strFiltri & " AND LIVELLO='TITOLARE'"
            End If

            strSql = "SELECT DISTINCT CODICEENTE, DENOMINAZIONEENTE, CODICEFISCALE, TIPOENTE, SEZIONE, LIVELLO, SITOWEB, EMAIL,  TELEFONO FROM VW_WS_RICERCAENTI_ALBO_SCU WHERE 1=1 " & strFiltri
            strSql = strSql & " ORDER BY DENOMINAZIONEENTE"
            Dim CMD As New SqlClient.SqlDataAdapter(strSql, sqlConn)

            dtsLocal.DataSetName = "Enti"

            'dtsLocal.Tables(0).TableName = "ENTI"

            CMD.Fill(dtsLocal)

            dtsLocal.Tables(0).TableName = "DettaglioEnte"

            xmlLocal = dtsLocal.GetXml

            EseguiRicercaEntiSCU_NEW = xmlLocal

            sqlConn.Close()

            Return EseguiRicercaEntiSCU_NEW
        Catch ex As Exception
            Dim myLog As New GeneraLog(ex.Message, "EseguiRicercaEntiSCU_NEW")
            myLog.GeneraFileLog()
        End Try
    End Function

    <WebMethod()> _
    Public Function EseguiElencoSediEnteSCU_NEW(ByVal strLocalizzazione As String, ByVal strNazione As String, ByVal strRegione As String, ByVal strProvincia As String, ByVal strComune As String, ByVal strCodiceEnte As String) As String
        Dim strSql As String
        Dim sqlConn As New SqlClient.SqlConnection
        Dim dtsLocal As New DataSet
        Dim xmlLocal As String
        Dim strFiltri As String = ""

        Try

            sqlConn.ConnectionString = System.Configuration.ConfigurationManager.AppSettings("cnHelios")

            sqlConn.Open()


            If strLocalizzazione <> "" Then
                strFiltri = strFiltri & " AND LOCALIZZAZIONE='" & Replace(strLocalizzazione, "'", "''") & "'"
            End If

            If strNazione <> "" Then
                strFiltri = strFiltri & " AND NAZIONE='" & Replace(strNazione, "'", "''") & "'"
            End If

            If strRegione <> "" Then
                strFiltri = strFiltri & " AND REGIONE='" & Replace(strRegione, "'", "''") & "'"
            End If

            If strProvincia <> "" Then
                strFiltri = strFiltri & " AND PROVINCIA='" & Replace(strProvincia, "'", "''") & "'"
            End If

            If strComune <> "" Then
                strFiltri = strFiltri & " AND COMUNE='" & Replace(strComune, "'", "''") & "'"
            End If

            If strCodiceEnte <> "" Then
                strFiltri = strFiltri & " AND CODICEENTE='" & Replace(strCodiceEnte, "'", "''") & "'"
            Else
                strFiltri = strFiltri & " AND CODICEENTE='ND' "
            End If



            strSql = "SELECT DISTINCT LOCALIZZAZIONE, NAZIONE, NUMEROSEDI, REGIONE, PROVINCIA, COMUNE FROM VW_WS_RICERCAENTI_ALBO_SCU WHERE 1=1 " & strFiltri
            strSql = strSql & " ORDER BY LOCALIZZAZIONE DESC, NAZIONE ASC, REGIONE ASC, PROVINCIA ASC, COMUNE ASC"

            Dim CMD As New SqlClient.SqlDataAdapter(strSql, sqlConn)

            dtsLocal.DataSetName = "Sedi"

            'dtsLocal.Tables(0).TableName = "ENTI"

            CMD.Fill(dtsLocal)

            dtsLocal.Tables(0).TableName = "DettaglioSede"

            xmlLocal = dtsLocal.GetXml

            EseguiElencoSediEnteSCU_NEW = xmlLocal

            sqlConn.Close()

            Return EseguiElencoSediEnteSCU_NEW
        Catch ex As Exception
            Dim myLog As New GeneraLog(ex.Message, "EseguiElencoSediEnteSCU_NEW")
            myLog.GeneraFileLog()
        End Try
    End Function

    <WebMethod()> _
    Public Function EseguiElencoEntiAccoglienzaEnteSCU_NEW(ByVal strLocalizzazione As String, ByVal strNazione As String, ByVal strRegione As String, ByVal strProvincia As String, ByVal strComune As String, ByVal strCodiceEnteTitolare As String) As String
        Dim strSql As String
        Dim sqlConn As New SqlClient.SqlConnection
        Dim dtsLocal As New DataSet
        Dim xmlLocal As String
        Dim strFiltri As String = ""

        Try

            sqlConn.ConnectionString = System.Configuration.ConfigurationManager.AppSettings("cnHelios")

            sqlConn.Open()


            If strLocalizzazione <> "" Then
                strFiltri = strFiltri & " AND LOCALIZZAZIONE='" & Replace(strLocalizzazione, "'", "''") & "'"
            End If

            If strNazione <> "" Then
                strFiltri = strFiltri & " AND NAZIONE='" & Replace(strNazione, "'", "''") & "'"
            End If

            If strRegione <> "" Then
                strFiltri = strFiltri & " AND REGIONE='" & Replace(strRegione, "'", "''") & "'"
            End If

            If strProvincia <> "" Then
                strFiltri = strFiltri & " AND PROVINCIA='" & Replace(strProvincia, "'", "''") & "'"
            End If

            If strComune <> "" Then
                strFiltri = strFiltri & " AND COMUNE='" & Replace(strComune, "'", "''") & "'"
            End If



            If strCodiceEnteTitolare <> "" Then
                strFiltri = strFiltri & " AND CODICEENTETITOLARE='" & Replace(strCodiceEnteTitolare, "'", "''") & "' "
            Else
                strFiltri = strFiltri & " AND CODICEENTETITOLARE='ND' " 'SE NON SPECIFICO L'ENTE TITOLARE NON ESTRAGGO NULLA
            End If

            strFiltri = strFiltri & " AND LIVELLO = 'ACCOGLIENZA'"


            strSql = "SELECT DISTINCT CODICEENTE, DENOMINAZIONEENTE, CODICEFISCALE, TIPOENTE FROM VW_WS_RICERCAENTI_ALBO_SCU WHERE 1=1 " & strFiltri
            strSql = strSql & " ORDER BY DENOMINAZIONEENTE"

            Dim CMD As New SqlClient.SqlDataAdapter(strSql, sqlConn)

            dtsLocal.DataSetName = "EntiAccoglienza"

            'dtsLocal.Tables(0).TableName = "ENTI"

            CMD.Fill(dtsLocal)

            dtsLocal.Tables(0).TableName = "DettaglioEnteAccoglienza"

            xmlLocal = dtsLocal.GetXml

            EseguiElencoEntiAccoglienzaEnteSCU_NEW = xmlLocal

            sqlConn.Close()

            Return EseguiElencoEntiAccoglienzaEnteSCU_NEW
        Catch ex As Exception
            Dim myLog As New GeneraLog(ex.Message, "EseguiElencoEntiAccoglienzaEnteSCU_NEW")
            myLog.GeneraFileLog()
        End Try
    End Function

    <WebMethod()> _
    Public Function EseguiRicercaSezioni_NEW() As String
        Dim strSql As String
        Dim sqlConn As New SqlClient.SqlConnection
        Dim dtsLocal As New DataSet
        Dim xmlLocal As String
        Dim strFiltri As String = ""

        Try

            sqlConn.ConnectionString = System.Configuration.ConfigurationManager.AppSettings("cnHelios")

            sqlConn.Open()

         

            strSql = "select * from VW_WS_SEZIONI order by 1 "

            Dim CMD As New SqlClient.SqlDataAdapter(strSql, sqlConn)

            dtsLocal.DataSetName = "Sezioni"

            'dtsLocal.Tables(0).TableName = "ENTI"

            CMD.Fill(dtsLocal)

            dtsLocal.Tables(0).TableName = "DettaglioSezioni"

            xmlLocal = dtsLocal.GetXml

            EseguiRicercaSezioni_NEW = xmlLocal

            sqlConn.Close()

            Return EseguiRicercaSezioni_NEW
        Catch ex As Exception
            Dim myLog As New GeneraLog(ex.Message, "EseguiRicercaSezioni_NEW")
            myLog.GeneraFileLog()
        End Try
    End Function

    ' NUOVO METODO CON FILTRO AGGIUNTIVO STRACCREDITAMENTO (CAPOFILA O FIGLIO) 
    ' SE MESSA IN ESERCIZIO VA AGGIORNATA VISTA [VW_WS_RICERCAENTI] DI WWW1.UNSCPRODUZIONENEW E WSHELIOSPUBBLICO
    '<WebMethod()> _
    '   Public Function EseguiRicercaEnti(ByVal strRegione As String, ByVal strProvincia As String, ByVal strComune As String, ByVal strTipoEnte As String, ByVal strCompentenza As String, ByVal strClasse As String, ByVal strDenominazioneEnte As String, ByVal strCodiceEnte As String, ByVal strAccreditamento As String) As String
    '    Dim strSql As String
    '    Dim sqlConn As New SqlClient.SqlConnection
    '    Dim dtsLocal As New DataSet
    '    Dim xmlLocal As String
    '    Dim strFiltri As String = ""

    '    Try
    '        
    '        sqlConn.ConnectionString = System.Configuration.ConfigurationManager.AppSettings("cnHelios")

    '        sqlConn.Open()

    '        If strRegione <> "" Then
    '            strFiltri = strFiltri & " AND REGIONE='" & Replace(strRegione, "'", "''") & "'"
    '        End If

    '        If strProvincia <> "" Then
    '            strFiltri = strFiltri & " AND PROVINCIA='" & Replace(strProvincia, "'", "''") & "'"
    '        End If

    '        If strComune <> "" Then
    '            strFiltri = strFiltri & " AND COMUNE='" & Replace(strComune, "'", "''") & "'"
    '        End If
    '        'modificato il 22/05/2015 da simona cordella
    '        '	Se riceve come valore PRIVATO allora effettua la ricerca degli enti con Tipologia = ‘PRIVATO’
    '        '	Se riceve come valore PUBBLICO allora effettua la ricerca degli enti con Tipologia <> ‘PRIVATO’
    '        If Trim(strTipoEnte) <> "" Then
    '            If UCase(Trim(strTipoEnte)) = "PRIVATO" Then
    '                strFiltri = strFiltri & " AND TIPOENTE='" & Replace(UCase(Trim(strTipoEnte)), "'", "''") & "'"
    '            Else
    '                strFiltri = strFiltri & " AND TIPOENTE<>'PRIVATO'"
    '            End If
    '        End If

    '        'If strTipoEnte <> "" Then
    '        '    strFiltri = strFiltri & " AND TIPOENTE='" & Replace(strTipoEnte, "'", "''") & "'"
    '        'End If

    '        If strClasse <> "" Then
    '            strFiltri = strFiltri & " AND CLASSE='" & Replace(strClasse, "'", "''") & "'"
    '        End If


    '        If strCodiceEnte <> "" Then
    '            strFiltri = strFiltri & " AND CODICEENTE='" & Replace(strCodiceEnte, "'", "''") & "'"
    '        End If

    '        If strCompentenza <> "" Then
    '            strFiltri = strFiltri & " AND COMPETENZA='" & Replace(strCompentenza, "'", "''") & "'"
    '        End If

    '        If strDenominazioneEnte <> "" Then
    '            strFiltri = strFiltri & " AND DENOMINAZIONEENTE LIKE '%" & Replace(strDenominazioneEnte, "'", "''") & "%'"
    '        End If

    '        'AGGIUNTO IL 22/05/20154 DA SIMONA CORDELLA
    '        'Se ricerco solo i TITOLARI estraggo solo gli enti capofila
    '        'se inserisco TUTTI estraggo sia i capofila che i figli
    '        If UCase(Trim(strAccreditamento)) = "TITOLARI" Then
    '            strFiltri = strFiltri & " AND Accreditamento='CAPOFILA'"
    '        End If

    '        strSql = "SELECT DISTINCT DENOMINAZIONEENTE, TIPOENTE, SITO, Email, CLASSE, CODICEENTE, TELEFONO, COMPETENZA FROM VW_WS_RICERCAENTI WHERE 1=1 " & strFiltri

    '        Dim CMD As New SqlClient.SqlDataAdapter(strSql, sqlConn)

    '        dtsLocal.DataSetName = "Enti"

    '        'dtsLocal.Tables(0).TableName = "ENTI"

    '        CMD.Fill(dtsLocal)

    '        dtsLocal.Tables(0).TableName = "DettaglioEnte"

    '        xmlLocal = dtsLocal.GetXml

    '        EseguiRicercaEnti = xmlLocal

    '        sqlConn.Close()

    '        Return EseguiRicercaEnti
    '    Catch ex As Exception
    '        Dim myLog As New GeneraLog(ex.Message, "EseguiRicercaEnti")
    '        myLog.GeneraFileLog()
    '    End Try
    'End Function

    Public Function FileToBase64(ByVal fileName As String) As String
        Dim bFile() As Byte
        Dim fs As FileStream
        Dim _textB64 As String
        Try
            fs = New FileStream(fileName, FileMode.Open)
            ReDim bFile(fs.Length - 1)
            fs.Read(bFile, 0, fs.Length)
            _textB64 = Convert.ToBase64String(bFile)
        Catch ex As Exception
            'gestione eccezione 
            Dim myLog As New GeneraLog(ex.Message + " File:" + fileName, "FileToBase64")
            myLog.GeneraFileLog()
        Finally
            fs.Close()
        End Try
        Return _textB64
    End Function

    Public Shared Function CreatePdf(ByVal NomeReport As String, ByVal StrDati As String, ByVal strUserName As String, Optional ByVal SottoReport As String = "", Optional ByVal ReportStorico As Int16 = 1) As String
        '*************************************************************************************************
        'DESCRIZIONE: Genera il PDF nella directory Reports/Export del report selezionato
        'AUTORE: TESTA GUIDO    DATA: 04/10/2004
        '*************************************************************************************************
        Dim paramFieldDt As New ParameterField
        Dim discreteValDt As New ParameterDiscreteValue
        Dim myPath As New System.Web.UI.Page
        Dim crReportDocument As New ReportDocument
        Dim logOnInfo As New TableLogOnInfo
        Dim NameReportNew As String
        Dim i As Integer
        Dim sGruppo() As String         'matrice parametri/valori
        Dim sGruppo1() As String        'matrice sottoreport
        Dim sElemt() As String
        Dim GetPdfError As String
        Dim dbg As Char = System.Configuration.ConfigurationManager.AppSettings("debugApp")
        Dim myLog As GeneraLog


        If dbg = "s" Then
            myLog = New GeneraLog("Sono nella function CreatePdf; parm: Report:" + NomeReport + " Dati:" + StrDati + " Username:" + strUserName, "CreatePdf")
            myLog.GeneraFileLog()
        End If

        GetPdfError = ""

        NameReportNew = UCase(strUserName) & "-" & Format(Now, "dd-MM-yyyyhh-mm-ss")

        Try
            If dbg = "s" Then
                myLog = New GeneraLog("Load: " + myPath.Server.MapPath(NomeReport), "CreatePdf")
                myLog.GeneraFileLog()
            End If
            crReportDocument.Load(myPath.Server.MapPath(NomeReport))

            sGruppo = Split(StrDati, ":")
            '1)parametri report*****************************
            For i = 0 To UBound(sGruppo) - 1
                sElemt = Split(sGruppo(i), ",")
                paramFieldDt.ParameterFieldName = "@" & sElemt(0)       'nome campo
                discreteValDt.Value = sElemt(1)                           'valore campo
                paramFieldDt.CurrentValues.Add(discreteValDt)

                Dim paramFieldDefDt As ParameterFieldDefinition = crReportDocument.DataDefinition.ParameterFields.Item(sElemt(0))

                Dim ParameterValuesDt As ParameterValues = paramFieldDt.CurrentValues
                paramFieldDefDt.ApplyCurrentValues(ParameterValuesDt)
            Next i
            '******************************************

            '2)parametri connessione*********************
            logOnInfo = crReportDocument.Database.Tables.Item(0).LogOnInfo

            If ReportStorico = 1 Then
                With logOnInfo
                    .ConnectionInfo.Password = System.Configuration.ConfigurationManager.AppSettings("connectionPassword")
                    .ConnectionInfo.ServerName = System.Configuration.ConfigurationManager.AppSettings("connectionServerName")
                    .ConnectionInfo.DatabaseName = System.Configuration.ConfigurationManager.AppSettings("PDFConnectionDatabaseName")
                    .ConnectionInfo.UserID = System.Configuration.ConfigurationManager.AppSettings("connectionUserid")
                End With
            End If


            crReportDocument.Database.Tables(0).ApplyLogOnInfo(logOnInfo)
            '******************************************

            '3)gestione sotto report*********************
            sGruppo1 = Split(SottoReport, ":")
            i = 0
            For i = 0 To UBound(sGruppo1) - 1
                crReportDocument.OpenSubreport(sGruppo1(i)).Database.Tables(0).ApplyLogOnInfo(logOnInfo)
            Next i
            '******************************************


            '4)esportazione report in PDF***************
            Dim crDiskFileDestinationOptions As New CrystalDecisions.Shared.DiskFileDestinationOptions
            Dim crExportOptions As CrystalDecisions.Shared.ExportOptions


            crDiskFileDestinationOptions.DiskFileName = myPath.Server.MapPath("reports/export/" & NameReportNew & ".pdf")
            crExportOptions = crReportDocument.ExportOptions
            crExportOptions.ExportDestinationType = CrystalDecisions.[Shared].ExportDestinationType.DiskFile
            crExportOptions.ExportFormatType = CrystalDecisions.[Shared].ExportFormatType.PortableDocFormat
            crExportOptions.DestinationOptions = crDiskFileDestinationOptions
            If dbg = "s" Then
                myLog = New GeneraLog("DiskFileName: " + crDiskFileDestinationOptions.DiskFileName, "CreatePdf")
                myLog.GeneraFileLog()
            End If


            crReportDocument.Export()
            crReportDocument.Close()

            Return "reports/export/" & NameReportNew & ".pdf"

            '******************************************

        Catch ex As Exception
            myLog = New GeneraLog(ex.Message, "CreatePdf")
            myLog.GeneraFileLog()

            GetPdfError = "ERRORE GENERICO. CONTATTARE L'ASSISTENZA" 'ex.Message
        End Try
    End Function
    Private Function TipologiaModelloDaIdEntita(ByVal idEntita As Integer, ByVal connection As SqlConnection) As String
        'ritorna la tipologia di modello da generare:
        'EST per progetti estero
        'GG per progetti garanzia giovani
        'SCD per progetti servizio civile digitale
        'ORD per prgetti ordinari
        Dim TipoModello As String = ""
        Dim dtrLocale As SqlClient.SqlDataReader
        Dim strsql As String
        Dim myCommand As SqlCommand

        strsql = "Select isnull(b.particolarità,0) as particolarità, tp.MacroTipoProgetto, tp.NazioneBase" & _
                " from entità e" & _
                " inner join GraduatorieEntità ge on e.IDEntità = ge.IDEntità" & _
                " inner join AttivitàSediAssegnazione asa on ge.IdAttivitàSedeAssegnazione = asa.IDAttivitàSedeAssegnazione" & _
                " inner join attività a on asa.IDAttività = a.IDAttività" & _
                " inner join TipiProgetto tp on a.IdTipoProgetto = tp.IdTipoProgetto" & _
                " inner join BandiAttività ba on a.IDBandoAttività = ba.IDBandoAttività" & _
                " inner join bando b on ba.IdBando = b.IDBando" & _
                " where e.identità = " & idEntita
        myCommand = New SqlClient.SqlCommand
        myCommand.Connection = connection

        'eseguo la query
        myCommand.CommandText = strsql
        dtrLocale = myCommand.ExecuteReader

        If dtrLocale.HasRows = True Then
            dtrLocale.Read()
            If dtrLocale("nazionebase") = 0 Then
                TipoModello = "EST"
            ElseIf dtrLocale("MacroTipoProgetto") = "GG" Then
                TipoModello = "GG"
            ElseIf dtrLocale("particolarità") = 1 Then
                TipoModello = "SCD"
            Else
                TipoModello = "ORD"
            End If

        End If
        If Not dtrLocale Is Nothing Then
            dtrLocale.Close()
            dtrLocale = Nothing
        End If

        Return TipoModello
    End Function

    Private Function CreaContrattoVolontario(ByVal IdVol As Integer, ByVal strUserName As String) As String
        'creata da simoma e danilo il 04/09/2012
        'generazione contratto volontario
        Dim strsql As String
        Dim sqlConn As New SqlConnection
        Dim dtrLocal As SqlDataReader
        Dim myCommand As SqlCommand
        Dim strPercorsoDoc As String
        Dim strFlag As String = ""
        Dim strGruppo As String = ""

        Try
            sqlConn.ConnectionString = System.Configuration.ConfigurationManager.AppSettings("cnHelios")
            sqlConn.Open()
            Dim strTipoModello As String = TipologiaModelloDaIdEntita(IdVol, sqlConn)

            strsql = "select a.idtipoprogetto as naz, isnull(identitàSubentrante,0) as  subentro, " & _
                    " e.datainizioservizio, isnull(asa.datainiziodifferita, a.datainizioattività) as datainizioattività, bando.gruppo,case when datainizioservizio < '15/01/2019' then '<' else '>' end as Flag  " & _
                    " ,DBO.FN_WS_ABILITA_DOWNLOAD_CONTRATTO_INTEGRATIVO(e.IdEntità) AS ABILITA_CONTRATTO_INTEGRATIVO " & _
                    " from Attività a " & _
                    " inner join attivitàsediassegnazione asa on asa.idattività=a.idattività " & _
                    " inner join graduatorieentità ge on ge.idattivitàsedeassegnazione=asa.idattivitàsedeassegnazione " & _
                    " inner join entità e on e.idEntità=ge.idEntità " & _
                    " inner join bandiattività ba on a.idbandoattività=ba.idbandoattività " & _
                    " inner join bando on ba.idbando=bando.idbando " & _
                    " left join cronologiasostituzioni cs on cs.identitàsubentrante=ge.idEntità " & _
                    " where ge.idEntità=" & IdVol
            myCommand = New SqlClient.SqlCommand
            myCommand.Connection = sqlConn

            'eseguo la query
            myCommand.CommandText = strsql
            dtrLocal = myCommand.ExecuteReader
            If dtrLocal.HasRows = True Then
                dtrLocal.Read()
                strFlag = dtrLocal("Flag")
                strGruppo = dtrLocal("gruppo")
                If dtrLocal("ABILITA_CONTRATTO_INTEGRATIVO") = "OK" Then
                    If Not dtrLocal Is Nothing Then
                        dtrLocal.Close()
                        dtrLocal = Nothing
                    End If
                    Dim Documento As New GeneratoreModelli
                    strPercorsoDoc = Documento.VOL_AttoAggiuntivoVolontari(IdVol, strUserName, 22, sqlConn)
                    Documento.Dispose()
                    'Cronologia creazione documento.
                    CronologiaDocEntità(IdVol, strUserName, "AttoAggiuntivoVolontari", sqlConn, "", "")

                Else
                    If dtrLocal("naz") = 1 Or dtrLocal("naz") = 3 Or dtrLocal("naz") = 5 Or dtrLocal("naz") = 7 Or dtrLocal("naz") = 9 Or dtrLocal("naz") = 11 Then
                        'italia
                        If dtrLocal("datainizioservizio") = dtrLocal("datainizioattività") Then
                            If Not dtrLocal Is Nothing Then
                                dtrLocal.Close()
                                dtrLocal = Nothing
                            End If

                            Dim Documento As New GeneratoreModelli
                            'chiamo la proprietà della classe GeneratoreModelli che ritorna la path del documento creato
                            'Dim pippo As String = Documento.VOL_AssegnazioneVolontariNazionali(IdVol, strUserName, 22, sqlConn)



                            If strGruppo = "50" And strFlag = "<" Then
                                strPercorsoDoc = Documento.VOL_AssegnazioneVolontariNazionaliB_Integrativo(IdVol, strUserName, 22, sqlConn)
                                Documento.Dispose()
                                'Cronologia creazione documento.
                                CronologiaDocEntità(IdVol, strUserName, "AssegnazioneVolontariNazionaliB2", sqlConn, "", "")
                            Else
                                Select Case strTipoModello
                                    Case "ORD"
                                        strPercorsoDoc = Documento.VOL_AssegnazioneVolontariNazionaliB(IdVol, strUserName, 22, sqlConn)
                                        CronologiaDocEntità(IdVol, strUserName, "AssegnazioneVolontariNazionaliB", sqlConn, "", "")
                                    Case "EST"
                                        strPercorsoDoc = Documento.VOL_AssegnazioneVolontariEsteroB(IdVol, strUserName, 22, sqlConn)
                                        CronologiaDocEntità(IdVol, strUserName, "AssegnazioneVolontariEsteroB", sqlConn, "", "")
                                    Case "GG"
                                        strPercorsoDoc = Documento.VOL_AssegnazioneVolontariGaranziaGiovaniB(IdVol, strUserName, 22, sqlConn)
                                        CronologiaDocEntità(IdVol, strUserName, "AssegnazioneVolontariGaranziaGiovaniB", sqlConn, "", "")
                                    Case "SCD"
                                        strPercorsoDoc = Documento.VOL_AssegnazioneVolontariNazionaliBSCD(IdVol, strUserName, 22, sqlConn)
                                        CronologiaDocEntità(IdVol, strUserName, "AssegnazioneVolontariNazionaliBSCD", sqlConn, "", "")
                                End Select
                                'strPercorsoDoc = Documento.VOL_AssegnazioneVolontariNazionaliB(IdVol, strUserName, 22, sqlConn)
                                Documento.Dispose()
                                'Cronologia creazione documento.
                                'CronologiaDocEntità(IdVol, strUserName, "AssegnazioneVolontariNazionaliB", sqlConn, "", "")
                            End If


                        Else
                            If Not dtrLocal Is Nothing Then
                                dtrLocal.Close()
                                dtrLocal = Nothing
                            End If
                            Dim Documento As New GeneratoreModelli
                            'chiamo la proprietà della classe GeneratoreModelli che ritorna la path del documento creato
                            'Dim pippo As String = Documento.VOL_SostituzioneVolontariNazionali(IdVol, strUserName, 22, sqlConn)

                            If strGruppo = "50" And strFlag = "<" Then
                                strPercorsoDoc = Documento.VOL_SostituzioneVolontariNazionaliB_Integrativo(IdVol, strUserName, 22, sqlConn)
                                Documento.Dispose()
                                'Cronologia creazione documento.
                                CronologiaDocEntità(IdVol, strUserName, "SostituzioneVolontariNazionaliB2", sqlConn, "", "")
                            Else
                                Select Case strTipoModello
                                    Case "ORD"
                                        strPercorsoDoc = Documento.VOL_SostituzioneVolontariNazionaliB(IdVol, strUserName, 22, sqlConn)
                                        CronologiaDocEntità(IdVol, strUserName, "SostituzioneVolontariNazionaliB", sqlConn, "", "")
                                    Case "EST"
                                        strPercorsoDoc = Documento.VOL_SostituzioneVolontariEsteriB(IdVol, strUserName, 22, sqlConn)
                                        CronologiaDocEntità(IdVol, strUserName, "SostituzioneVolontariEsteriB", sqlConn, "", "")
                                    Case "GG"
                                        strPercorsoDoc = Documento.VOL_SostituzioneVolontariGaranziaGiovaniB(IdVol, strUserName, 22, sqlConn)
                                        CronologiaDocEntità(IdVol, strUserName, "SostituzioneVolontariGaranziaGiovaniB", sqlConn, "", "")
                                    Case "SCD"
                                        strPercorsoDoc = Documento.VOL_SostituzioneVolontariNazionaliBSCD(IdVol, strUserName, 22, sqlConn)
                                        CronologiaDocEntità(IdVol, strUserName, "SostituzioneVolontariNazionaliBSCD", sqlConn, "", "")
                                End Select
                                'strPercorsoDoc = Documento.VOL_SostituzioneVolontariNazionaliB(IdVol, strUserName, 22, sqlConn)
                                Documento.Dispose()
                                'Cronologia creazione documento.
                                'CronologiaDocEntità(IdVol, strUserName, "SostituzioneVolontariNazionaliB", sqlConn, "", "")
                            End If

                            'strPercorsoDoc = Documento.VOL_SostituzioneVolontariNazionaliB(IdVol, strUserName, 22, sqlConn)
                            'Documento.Dispose()

                            ''Cronologia creazione documento.
                            ''CronologiaDocEntità(IdVol, strUserName, "Sostituzione Volontario - Nazionale", sqlConn, "", "")
                            'CronologiaDocEntità(IdVol, strUserName, "SostituzioneVolontariNazionaliB", sqlConn, "", "")
                        End If

                    Else
                        If dtrLocal("naz") = 2 Or dtrLocal("naz") = 6 Or dtrLocal("naz") = 8 Or dtrLocal("naz") = 10 Or dtrLocal("naz") = 12 Then
                            'estero
                            If dtrLocal("datainizioservizio") = dtrLocal("datainizioattività") Then
                                If Not dtrLocal Is Nothing Then
                                    dtrLocal.Close()
                                    dtrLocal = Nothing
                                End If

                                Dim Documento As New GeneratoreModelli
                                'chiamo la proprietà della classe GeneratoreModelli che ritorna la path del documento creato
                                'Dim pippo As String = Documento.VOL_AssegnazioneVolontariEstero(IdVol, strUserName, 22, sqlConn)

                                If strGruppo = "50" And strFlag = "<" Then
                                    strPercorsoDoc = Documento.VOL_AssegnazioneVolontariEsteroB_Integrativo(IdVol, strUserName, 22, sqlConn)
                                    Documento.Dispose()
                                    'Cronologia creazione documento.
                                    CronologiaDocEntità(IdVol, strUserName, "AssegnazioneVolontariEsteroB2", sqlConn, "", "")
                                Else
                                    Select Case strTipoModello
                                        Case "ORD"
                                            strPercorsoDoc = Documento.VOL_AssegnazioneVolontariNazionaliB(IdVol, strUserName, 22, sqlConn)
                                            CronologiaDocEntità(IdVol, strUserName, "AssegnazioneVolontariNazionaliB", sqlConn, "", "")
                                        Case "EST"
                                            strPercorsoDoc = Documento.VOL_AssegnazioneVolontariEsteroB(IdVol, strUserName, 22, sqlConn)
                                            CronologiaDocEntità(IdVol, strUserName, "AssegnazioneVolontariEsteroB", sqlConn, "", "")
                                        Case "GG"
                                            strPercorsoDoc = Documento.VOL_AssegnazioneVolontariGaranziaGiovaniB(IdVol, strUserName, 22, sqlConn)
                                            CronologiaDocEntità(IdVol, strUserName, "AssegnazioneVolontariGaranziaGiovaniB", sqlConn, "", "")
                                        Case "SCD"
                                            strPercorsoDoc = Documento.VOL_AssegnazioneVolontariNazionaliBSCD(IdVol, strUserName, 22, sqlConn)
                                            CronologiaDocEntità(IdVol, strUserName, "AssegnazioneVolontariNazionaliBSCD", sqlConn, "", "")
                                    End Select
                                    'strPercorsoDoc = Documento.VOL_AssegnazioneVolontariEsteroB(IdVol, strUserName, 22, sqlConn)
                                    Documento.Dispose()
                                    'Cronologia creazione documento.
                                    'CronologiaDocEntità(IdVol, strUserName, "AssegnazioneVolontariEsteroB", sqlConn, "", "")
                                End If

                                'strPercorsoDoc = Documento.VOL_AssegnazioneVolontariEsteroB(IdVol, strUserName, 22, sqlConn)
                                'Documento.Dispose()

                                ''Cronologia creazione documento.
                                ''CronologiaDocEntità(IdVol, strUserName, "Assegnazione Volontario - Estero", sqlConn, "", "")
                                'CronologiaDocEntità(IdVol, strUserName, "AssegnazioneVolontariEsteroB", sqlConn, "", "")
                            Else
                                If Not dtrLocal Is Nothing Then
                                    dtrLocal.Close()
                                    dtrLocal = Nothing
                                End If
                                Dim Documento As New GeneratoreModelli
                                'chiamo la proprietà della classe GeneratoreModelli che ritorna la path del documento creato
                                'Dim pippo As String = Documento.VOL_SostituzioneVolontariEsteri(IdVol, strUserName, 22, sqlConn)

                                If strGruppo = "50" And strFlag = "<" Then
                                    strPercorsoDoc = Documento.VOL_SostituzioneVolontariEsteriB_Integrativo(IdVol, strUserName, 22, sqlConn)
                                    Documento.Dispose()
                                    'Cronologia creazione documento.
                                    CronologiaDocEntità(IdVol, strUserName, "SostituzioneVolontariEsteriB2", sqlConn, "", "")
                                Else
                                    Select Case strTipoModello
                                        Case "ORD"
                                            strPercorsoDoc = Documento.VOL_SostituzioneVolontariNazionaliB(IdVol, strUserName, 22, sqlConn)
                                            CronologiaDocEntità(IdVol, strUserName, "SostituzioneVolontariNazionaliB", sqlConn, "", "")
                                        Case "EST"
                                            strPercorsoDoc = Documento.VOL_SostituzioneVolontariEsteriB(IdVol, strUserName, 22, sqlConn)
                                            CronologiaDocEntità(IdVol, strUserName, "SostituzioneVolontariEsteriB", sqlConn, "", "")
                                        Case "GG"
                                            strPercorsoDoc = Documento.VOL_SostituzioneVolontariGaranziaGiovaniB(IdVol, strUserName, 22, sqlConn)
                                            CronologiaDocEntità(IdVol, strUserName, "SostituzioneVolontariGaranziaGiovaniB", sqlConn, "", "")
                                        Case "SCD"
                                            strPercorsoDoc = Documento.VOL_SostituzioneVolontariNazionaliBSCD(IdVol, strUserName, 22, sqlConn)
                                            CronologiaDocEntità(IdVol, strUserName, "SostituzioneVolontariNazionaliBSCD", sqlConn, "", "")
                                    End Select
                                    'strPercorsoDoc = Documento.VOL_SostituzioneVolontariEsteriB(IdVol, strUserName, 22, sqlConn)
                                    Documento.Dispose()
                                    'Cronologia creazione documento.
                                    'CronologiaDocEntità(IdVol, strUserName, "SostituzioneVolontariEsteriB", sqlConn, "", "")
                                End If

                                'strPercorsoDoc = Documento.VOL_SostituzioneVolontariEsteriB(IdVol, strUserName, 22, sqlConn)
                                'Documento.Dispose()

                                ''Cronologia creazione documento.
                                ''CronologiaDocEntità(IdVol, strUserName, "Sostituzione Volontario - Estero", sqlConn, "", "")
                                'CronologiaDocEntità(IdVol, strUserName, "SostituzioneVolontariEsteriB", sqlConn, "", "")
                            End If
                        Else
                            Dim strNomeDocumento As String
                            'Garanzia Giovani
                            If dtrLocal("datainizioservizio") = dtrLocal("datainizioattività") Then
                                If Not dtrLocal Is Nothing Then
                                    dtrLocal.Close()
                                    dtrLocal = Nothing
                                End If

                                Dim Documento As New GeneratoreModelli
                                'chiamo la proprietà della classe GeneratoreModelli che ritorna la path del documento creato
                                'Dim pippo As String = Documento.VOL_AssegnazioneVolontariEstero(IdVol, strUserName, 22, sqlConn)

                                ''modificato il 08/04/2015 da simona cordella
                                'If ControlloEntitàSPCPerGenerazioneContratto(IdVol, sqlConn) = False Then
                                '    Select Case strTipoModello
                                '        Case "ORD"
                                '            strPercorsoDoc = Documento.VOL_AssegnazioneVolontariNazionaliB(IdVol, strUserName, 22, sqlConn)
                                '            strNomeDocumento = "AssegnazioneVolontariNazionaliB"
                                '        Case "EST"
                                '            strPercorsoDoc = Documento.VOL_AssegnazioneVolontariEsteroB(IdVol, strUserName, 22, sqlConn)
                                '            strNomeDocumento = "AssegnazioneVolontariEsteroB"
                                '        Case "GG"
                                '            strPercorsoDoc = Documento.VOL_AssegnazioneVolontariGaranziaGiovaniB(IdVol, strUserName, 22, sqlConn)
                                '            strNomeDocumento = "AssegnazioneVolontariGaranziaGiovaniB"
                                '        Case "SCD"
                                '            strPercorsoDoc = Documento.VOL_AssegnazioneVolontariNazionaliBSCD(IdVol, strUserName, 22, sqlConn)
                                '            strNomeDocumento = "AssegnazioneVolontariNazionaliBSCD"
                                '    End Select
                                '    'strPercorsoDoc = Documento.VOL_AssegnazioneVolontariGaranziaGiovaniB(IdVol, strUserName, 22, sqlConn)
                                '    'strNomeDocumento = "AssegnazioneVolontariGaranziaGiovaniB"
                                'Else ' genero contratto Contratto Volontario Italia (12 mesi) (Garanzia Giovani Senza Presa In Carico)
                                '    strPercorsoDoc = Documento.VOL_AssegnazioneVolontariGaranziaGiovaniBSPC(IdVol, strUserName, 22, sqlConn)
                                '    strNomeDocumento = "AssegnazioneVolontariGaranziaGiovaniBSPC"
                                'End If
                                Select Case strTipoModello
                                    Case "ORD"
                                        strPercorsoDoc = Documento.VOL_AssegnazioneVolontariNazionaliB(IdVol, strUserName, 22, sqlConn)
                                        strNomeDocumento = "AssegnazioneVolontariNazionaliB"
                                    Case "EST"
                                        strPercorsoDoc = Documento.VOL_AssegnazioneVolontariEsteroB(IdVol, strUserName, 22, sqlConn)
                                        strNomeDocumento = "AssegnazioneVolontariEsteroB"
                                    Case "GG"
                                        strPercorsoDoc = Documento.VOL_AssegnazioneVolontariGaranziaGiovaniB(IdVol, strUserName, 22, sqlConn)
                                        strNomeDocumento = "AssegnazioneVolontariGaranziaGiovaniB"
                                    Case "SCD"
                                        strPercorsoDoc = Documento.VOL_AssegnazioneVolontariNazionaliBSCD(IdVol, strUserName, 22, sqlConn)
                                        strNomeDocumento = "AssegnazioneVolontariNazionaliBSCD"
                                End Select
                                Documento.Dispose()

                                'Cronologia creazione documento.
                                'CronologiaDocEntità(IdVol, strUserName, "Assegnazione Volontario - Estero", sqlConn, "", "")
                                CronologiaDocEntità(IdVol, strUserName, strNomeDocumento, sqlConn, "", "")
                            Else
                                If Not dtrLocal Is Nothing Then
                                    dtrLocal.Close()
                                    dtrLocal = Nothing
                                End If
                                Dim Documento As New GeneratoreModelli
                                'chiamo la proprietà della classe GeneratoreModelli che ritorna la path del documento creato
                                'Dim pippo As String = Documento.VOL_SostituzioneVolontariEsteri(IdVol, strUserName, 22, sqlConn)

                                ''modificato il 08/04/2015 da simona cordella
                                'If ControlloEntitàSPCPerGenerazioneContratto(IdVol, sqlConn) = False Then
                                '    Select Case strTipoModello
                                '        Case "ORD"
                                '            strPercorsoDoc = Documento.VOL_SostituzioneVolontariNazionaliB(IdVol, strUserName, 22, sqlConn)
                                '            strNomeDocumento = "SostituzioneVolontariNazionaliB"
                                '        Case "EST"
                                '            strPercorsoDoc = Documento.VOL_SostituzioneVolontariEsteriB(IdVol, strUserName, 22, sqlConn)
                                '            strNomeDocumento = "SostituzioneVolontariEsteriB"
                                '        Case "GG"
                                '            strPercorsoDoc = Documento.VOL_SostituzioneVolontariGaranziaGiovaniB(IdVol, strUserName, 22, sqlConn)
                                '            strNomeDocumento = "SostituzioneVolontariGaranziaGiovaniB"
                                '        Case "SCD"
                                '            strPercorsoDoc = Documento.VOL_SostituzioneVolontariNazionaliBSCD(IdVol, strUserName, 22, sqlConn)
                                '            strNomeDocumento = "SostituzioneVolontariNazionaliBSCD"
                                '    End Select

                                '    'strPercorsoDoc = Documento.VOL_SostituzioneVolontariGaranziaGiovaniB(IdVol, strUserName, 22, sqlConn)
                                '    'strNomeDocumento = "SostituzioneVolontariGaranziaGiovaniB"
                                'Else ' genero contratto Contratto Volontario Italia (12 mesi) (Garanzia Giovani Senza Presa In Carico)
                                '    strPercorsoDoc = Documento.VOL_SostituzioneVolontariGaranziaGiovaniBSPC(IdVol, strUserName, 22, sqlConn)
                                '    strNomeDocumento = "SostituzioneVolontariGaranziaGiovaniBSPC"
                                'End If
                                Select Case strTipoModello
                                    Case "ORD"
                                        strPercorsoDoc = Documento.VOL_SostituzioneVolontariNazionaliB(IdVol, strUserName, 22, sqlConn)
                                        strNomeDocumento = "SostituzioneVolontariNazionaliB"
                                    Case "EST"
                                        strPercorsoDoc = Documento.VOL_SostituzioneVolontariEsteriB(IdVol, strUserName, 22, sqlConn)
                                        strNomeDocumento = "SostituzioneVolontariEsteriB"
                                    Case "GG"
                                        strPercorsoDoc = Documento.VOL_SostituzioneVolontariGaranziaGiovaniB(IdVol, strUserName, 22, sqlConn)
                                        strNomeDocumento = "SostituzioneVolontariGaranziaGiovaniB"
                                    Case "SCD"
                                        strPercorsoDoc = Documento.VOL_SostituzioneVolontariNazionaliBSCD(IdVol, strUserName, 22, sqlConn)
                                        strNomeDocumento = "SostituzioneVolontariNazionaliBSCD"
                                End Select
                                Documento.Dispose()

                                'Cronologia creazione documento.
                                'CronologiaDocEntità(IdVol, strUserName, "Sostituzione Volontario - Estero", sqlConn, "", "")
                                CronologiaDocEntità(IdVol, strUserName, strNomeDocumento, sqlConn, "", "")
                            End If
                        End If
                    End If
                End If

            End If
            Return strPercorsoDoc
        Catch ex As Exception
            'Dim myLog As New GeneraLog(ex.Message, "EseguiRicercaClassi")
            'myLog.GeneraFileLog()
            Return "ERRORE GENERICO. CONTATTARE L'ASSISTENZA"
        Finally
            'controllo e chiudo se aperto il datareader
            If Not dtrLocal Is Nothing Then
                dtrLocal.Close()
                dtrLocal = Nothing
            End If
        End Try


    End Function


    Public Shared Function CronologiaDocEntità(ByVal myIdEntità As String, ByVal myUserName As String, ByVal myNomeDocu As String, ByVal myConn As SqlClient.SqlConnection, Optional ByRef myDataProt As String = "", Optional ByRef myNumProt As String = "")
        'Gestione della cronologia dei documenti generati per il volontario.
        Dim strSql As String
        strSql = "INSERT INTO CronologiaEntitàDocumenti (IdEntità, UserName, DataDocumento, Documento,DataProt,NProt) " & _
                 "VALUES (" & myIdEntità & ",'" & myUserName & "',getdate(),'" & myNomeDocu & "',"
        If myDataProt = "" Then
            strSql = strSql & " null,"
        Else
            strSql = strSql & " '" & myDataProt & "',"
        End If
        If myNumProt = "" Then
            strSql = strSql & " null"
        Else
            strSql = strSql & "'" & myNumProt & "'"
        End If
        strSql = strSql & ")"

        Dim cmdInsertCron As New SqlClient.SqlCommand(strSql, myConn)
        cmdInsertCron.ExecuteNonQuery()
    End Function


    <WebMethod()> _
    Public Function EseguiRicercaClassi() As String
        Dim strSql As String
        Dim sqlConn As New SqlClient.SqlConnection
        Dim dtsLocal As New DataSet
        Dim xmlLocal As String

        Try

            sqlConn.ConnectionString = System.Configuration.ConfigurationManager.AppSettings("cnHelios")

            sqlConn.Open()

            strSql = "SELECT Classe FROM VW_WS_RICERCACLASSI "

            Dim CMD As New SqlClient.SqlDataAdapter(strSql, sqlConn)

            dtsLocal.DataSetName = "Classi"

            'dtsLocal.Tables(0).TableName = "ENTI"

            CMD.Fill(dtsLocal)

            dtsLocal.Tables(0).TableName = "DettaglioClasse"

            xmlLocal = dtsLocal.GetXml

            EseguiRicercaClassi = xmlLocal

            sqlConn.Close()

            Return EseguiRicercaClassi
        Catch ex As Exception
            Dim myLog As New GeneraLog(ex.Message, "EseguiRicercaClassi")
            myLog.GeneraFileLog()
        End Try
    End Function

    <WebMethod()> _
    Public Function EseguiRicercaClassiSCU() As String
        Dim strSql As String
        Dim sqlConn As New SqlClient.SqlConnection
        Dim dtsLocal As New DataSet
        Dim xmlLocal As String

        Try

            sqlConn.ConnectionString = System.Configuration.ConfigurationManager.AppSettings("cnHelios")

            sqlConn.Open()

            strSql = "SELECT Classe FROM VW_WS_RICERCACLASSI_SCU "

            Dim CMD As New SqlClient.SqlDataAdapter(strSql, sqlConn)

            dtsLocal.DataSetName = "Classi"

            'dtsLocal.Tables(0).TableName = "ENTI"

            CMD.Fill(dtsLocal)

            dtsLocal.Tables(0).TableName = "DettaglioClasse"

            xmlLocal = dtsLocal.GetXml

            EseguiRicercaClassiSCU = xmlLocal

            sqlConn.Close()

            Return EseguiRicercaClassiSCU
        Catch ex As Exception
            Dim myLog As New GeneraLog(ex.Message, "EseguiRicercaClassiSCU")
            myLog.GeneraFileLog()
        End Try
    End Function

    <WebMethod()> _
    Public Function EseguiRicercaSettoreDiIntervento() As String
        Dim strSql As String
        Dim sqlConn As New SqlClient.SqlConnection
        Dim dtsLocal As New DataSet
        Dim xmlLocal As String

        Try

            sqlConn.ConnectionString = System.Configuration.ConfigurationManager.AppSettings("cnHelios")

            sqlConn.Open()

            strSql = "SELECT  macroambitiattività.MacroAmbitoAttività As SETTORE,  ambitiattività.AmbitoAttività AS AREA "
            strSql = strSql & "FROM macroambitiattività INNER JOIN ambitiattività ON macroambitiattività.IDMacroAmbitoAttività = ambitiattività.IDMacroAmbitoAttività "
            strSql = strSql & "INNER JOIN AssociaAmbitiTipiProgetto b on ambitiattività.IDAmbitoAttività = b.IdAmbitoAttività "
            strSql = strSql & "where ambitiattività.attivo=1 and IdTipoProgetto in (11,12,3) ORDER BY macroambitiattività.Codifica, ambitiattività.Codifica"

            Dim CMD As New SqlClient.SqlDataAdapter(strSql, sqlConn)

            dtsLocal.DataSetName = "SettoreAreaDiIntervento"

            'dtsLocal.Tables(0).TableName = "ENTI"

            CMD.Fill(dtsLocal)

            dtsLocal.Tables(0).TableName = "DettaglioSettoreAreaDiIntervento"

            xmlLocal = dtsLocal.GetXml

            EseguiRicercaSettoreDiIntervento = xmlLocal

            sqlConn.Close()

            Return EseguiRicercaSettoreDiIntervento
        Catch ex As Exception
            Dim myLog As New GeneraLog(ex.Message, "EseguiRicercaSettoreDiIntervento")
            myLog.GeneraFileLog()
        End Try
    End Function

    '<WebMethod()> _
    Public Function EseguiRicercaSettoreDiInterventoXSD() As System.Xml.XmlDocument
        Dim strSql As String
        Dim sqlConn As New SqlClient.SqlConnection
        Dim dtsLocal As New DataSet
        Dim xmlLocal As String
        Dim dtR As DataRow
        Dim dtT As New DataTable

        Try
            dtT.Columns.Add(New DataColumn("SETTORE", GetType(String)))
            dtT.Columns.Add(New DataColumn("AREA", GetType(String)))

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = ""
            dtR(1) = ""

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)

            dtsLocal.Tables.Add(dtT)

            dtsLocal.DataSetName = "SettoreAreaDiIntervento"

            dtsLocal.Tables(0).TableName = "DettaglioSettoreAreaDiIntervento"

            Dim testXML As System.Xml.XmlNode
            'Dim xmlDOC As New System.Xml.XmlDataDocument
            Dim xmlDOC As New System.Xml.XmlDocument

            xmlDOC.InnerXml = dtsLocal.GetXmlSchema

            ''Create an XML declaration. 
            'Dim xmldecl As System.Xml.XmlDeclaration
            'xmldecl = xmlDOC.CreateXmlDeclaration("1.0", Nothing, Nothing)

            ''Add the new node to the document.
            'Dim root As System.Xml.XmlElement = xmlDOC.DocumentElement
            'xmlDOC.InsertBefore(xmldecl, root)

            EseguiRicercaSettoreDiInterventoXSD = xmlDOC

        Catch ex As Exception

        End Try

        Return EseguiRicercaSettoreDiInterventoXSD

    End Function

    <WebMethod()> _
    Public Function EseguiRicercaNazioniAccreditamentoSCU_NEW() As String
        Dim strSql As String
        Dim sqlConn As New SqlClient.SqlConnection
        Dim dtsLocal As New DataSet
        Dim xmlLocal As String

        Try

            sqlConn.ConnectionString = System.Configuration.ConfigurationManager.AppSettings("cnHelios")

            sqlConn.Open()

            strSql = "select NAZIONE from [VW_WS_NAZIONI_ACCREDITAMENTO_SCU] order by NazioneBase desc, NAZIONE "

            Dim CMD As New SqlClient.SqlDataAdapter(strSql, sqlConn)

            dtsLocal.DataSetName = "NazioniAccreditamento"

            'dtsLocal.Tables(0).TableName = "ENTI"

            CMD.Fill(dtsLocal)

            dtsLocal.Tables(0).TableName = "DettaglioNazioni"

            xmlLocal = dtsLocal.GetXml

            EseguiRicercaNazioniAccreditamentoSCU_NEW = xmlLocal

            sqlConn.Close()

            Return EseguiRicercaNazioniAccreditamentoSCU_NEW
        Catch ex As Exception
            Dim myLog As New GeneraLog(ex.Message, "EseguiRicercaNazioniAccreditamentoSCU_NEW")
            myLog.GeneraFileLog()
        End Try
    End Function

    <WebMethod()> _
Public Function EseguiRicercaGeografico() As String
        Dim strSql As String
        Dim sqlConn As New SqlClient.SqlConnection
        Dim dtsLocal As New DataSet
        Dim xmlLocal As String

        Try

            sqlConn.ConnectionString = System.Configuration.ConfigurationManager.AppSettings("cnHelios")

            sqlConn.Open()

            strSql = "SELECT REGIONE, PROVINCIA, COMUNE FROM VW_WS_GEOGRAFICO order by REGIONE, PROVINCIA, COMUNE "

            Dim CMD As New SqlClient.SqlDataAdapter(strSql, sqlConn)

            dtsLocal.DataSetName = "Geografico"

            'dtsLocal.Tables(0).TableName = "ENTI"

            CMD.Fill(dtsLocal)

            dtsLocal.Tables(0).TableName = "DettaglioGeografico"

            xmlLocal = dtsLocal.GetXml

            EseguiRicercaGeografico = xmlLocal

            sqlConn.Close()

            Return EseguiRicercaGeografico
        Catch ex As Exception
            Dim myLog As New GeneraLog(ex.Message, "EseguiRicercaGeografico")
            myLog.GeneraFileLog()
        End Try
    End Function
    <WebMethod()> _
    Public Function EseguiRicercaGeograficoDomicilio() As String
        Dim strSql As String
        Dim sqlConn As New SqlClient.SqlConnection
        Dim dtsLocal As New DataSet
        Dim xmlLocal As String

        Try

            sqlConn.ConnectionString = System.Configuration.ConfigurationManager.AppSettings("cnHelios")

            sqlConn.Open()

            strSql = "SELECT NAZIONE, REGIONE, PROVINCIA, COMUNE FROM VW_WS_GEOGRAFICO_DOMICILIO order by NAZIONE, REGIONE, PROVINCIA, COMUNE "

            Dim CMD As New SqlClient.SqlDataAdapter(strSql, sqlConn)

            dtsLocal.DataSetName = "GeograficoDomicilio"

            'dtsLocal.Tables(0).TableName = "ENTI"

            CMD.Fill(dtsLocal)

            dtsLocal.Tables(0).TableName = "DettaglioGeograficoDomicilio"

            xmlLocal = dtsLocal.GetXml

            EseguiRicercaGeograficoDomicilio = xmlLocal

            sqlConn.Close()

            Return EseguiRicercaGeograficoDomicilio
        Catch ex As Exception
            Dim myLog As New GeneraLog(ex.Message, "EseguiRicercaGeograficoDomicilio")
            myLog.GeneraFileLog()
        End Try
    End Function
    '<WebMethod()> _
    Public Function EseguiRicercaGeograficoXSD() As System.Xml.XmlDocument
        Dim strSql As String
        Dim sqlConn As New SqlClient.SqlConnection
        Dim dtsLocal As New DataSet
        Dim xmlLocal As String
        Dim dtR As DataRow
        Dim dtT As New DataTable

        Try
            dtT.Columns.Add(New DataColumn("Regione", GetType(String)))
            dtT.Columns.Add(New DataColumn("Provincia", GetType(String)))
            dtT.Columns.Add(New DataColumn("Comune", GetType(String)))

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = ""
            dtR(1) = ""
            dtR(2) = ""

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)

            dtsLocal.Tables.Add(dtT)

            dtsLocal.DataSetName = "Geografico"

            dtsLocal.Tables(0).TableName = "DettaglioGeografico"

            Dim testXML As System.Xml.XmlNode
            'Dim xmlDOC As New System.Xml.XmlDataDocument
            Dim xmlDOC As New System.Xml.XmlDocument

            xmlDOC.InnerXml = dtsLocal.GetXmlSchema

            ''Create an XML declaration. 
            'Dim xmldecl As System.Xml.XmlDeclaration
            'xmldecl = xmlDOC.CreateXmlDeclaration("1.0", Nothing, Nothing)

            ''Add the new node to the document.
            'Dim root As System.Xml.XmlElement = xmlDOC.DocumentElement
            'xmlDOC.InsertBefore(xmldecl, root)

            EseguiRicercaGeograficoXSD = xmlDOC

        Catch ex As Exception

        End Try

        Return EseguiRicercaGeograficoXSD

    End Function

    '<WebMethod()> _
    Public Function EseguiRicercaClassiXSD() As System.Xml.XmlDocument
        Dim dtsLocal As New DataSet
        Dim xmlLocal As String
        Dim dtR As DataRow
        Dim dtT As New DataTable

        dtT.Columns.Add(New DataColumn("Classe", GetType(Integer)))

        'alla riga assegno la riga delle intestazioni appena creata
        dtR = dtT.NewRow()

        'carico la prima riga della datatable
        dtR(0) = 0

        'aggiungo la riga appena caricata alla datatable
        dtT.Rows.Add(dtR)

        dtsLocal.Tables.Add(dtT)

        dtsLocal.DataSetName = "Classi"

        dtsLocal.Tables(0).TableName = "DettaglioClasse"

        Dim testXML As System.Xml.XmlNode
        'Dim xmlDOC As New System.Xml.XmlDataDocument
        Dim xmlDOC As New System.Xml.XmlDocument

        xmlDOC.InnerXml = dtsLocal.GetXmlSchema

        EseguiRicercaClassiXSD = xmlDOC

        Return EseguiRicercaClassiXSD
        'Return "Salve gente!"
    End Function

    '<WebMethod()> _
    Public Function EseguiRicercaEntiXSD() As System.Xml.XmlDocument
        Dim dtsLocal As New DataSet
        Dim xmlLocal As String
        Dim dtR As DataRow
        Dim dtT As New DataTable
        Dim intx As Integer

        dtT.Columns.Add(New DataColumn("DenominazioneEnte", GetType(String)))
        dtT.Columns.Add(New DataColumn("TipoEnte", GetType(String)))
        dtT.Columns.Add(New DataColumn("Sito", GetType(String)))
        dtT.Columns.Add(New DataColumn("Email", GetType(String)))
        dtT.Columns.Add(New DataColumn("Classe", GetType(Integer)))
        dtT.Columns.Add(New DataColumn("CodiceEnte", GetType(String)))
        dtT.Columns.Add(New DataColumn("Telefono", GetType(String)))
        dtT.Columns.Add(New DataColumn("Competenza", GetType(String)))

        'alla riga assegno la riga delle intestazioni appena creata
        dtR = dtT.NewRow()

        dtR(0) = ""
        dtR(1) = ""
        dtR(2) = ""
        dtR(3) = ""
        dtR(4) = 0
        dtR(5) = ""
        dtR(6) = ""
        dtR(7) = ""

        'aggiungo la riga appena caricata alla datatable
        dtT.Rows.Add(dtR)

        dtsLocal.Tables.Add(dtT)

        dtsLocal.DataSetName = "Enti"

        Dim testXML As System.Xml.XmlNode
        'Dim xmlDOC As New System.Xml.XmlDataDocument
        Dim xmlDOC As New System.Xml.XmlDocument

        dtsLocal.Tables(0).TableName = "DettaglioEnte"

        xmlDOC.InnerXml = dtsLocal.GetXmlSchema

        EseguiRicercaEntiXSD = xmlDOC

        Return EseguiRicercaEntiXSD
        'Return "Salve gente!"
    End Function

    '<WebMethod()> _
    Public Function EseguiRicercaContatoriXSD() As System.Xml.XmlDocument
        Dim strSql As String
        Dim sqlConn As New SqlClient.SqlConnection
        Dim dtsLocal As New DataSet
        Dim xmlLocal As String
        Dim dtR As DataRow
        Dim dtT As New DataTable

        dtT.Columns.Add(New DataColumn("NumeroVolontari", GetType(Integer)))
        dtT.Columns.Add(New DataColumn("NumeroEnti", GetType(Integer)))

        'alla riga assegno la riga delle intestazioni appena creata
        dtR = dtT.NewRow()

        'carico la prima riga della datatable
        dtR(0) = 0
        dtR(1) = 0

        'aggiungo la riga appena caricata alla datatable
        dtT.Rows.Add(dtR)

        dtsLocal.Tables.Add(dtT)

        dtsLocal.DataSetName = "Contatori"

        dtsLocal.Tables(0).TableName = "DettaglioContatori"

        Dim testXML As System.Xml.XmlNode
        'Dim xmlDOC As New System.Xml.XmlDataDocument
        Dim xmlDOC As New System.Xml.XmlDocument

        xmlDOC.InnerXml = dtsLocal.GetXmlSchema

        EseguiRicercaContatoriXSD = xmlDOC

        Return EseguiRicercaContatoriXSD
        'Return "Salve gente!"
    End Function

    <WebMethod()> _
Public Function EseguiRicercaContatori() As String
        Dim strSql As String
        Dim sqlConn As New SqlClient.SqlConnection
        Dim dtsLocal As New DataSet
        Dim xmlLocal As String

        Try

            sqlConn.ConnectionString = System.Configuration.ConfigurationManager.AppSettings("cnHelios")

            sqlConn.Open()
            strSql = "SELECT * FROM VW_WS_CONTATORI " 'nuova versione
            'strSql = "SELECT NumeroVolontari, NumeroEntiTitolari, NumeroEntiTotali FROM VW_WS_CONTATORI " 'nuova versione
            'strSql = "SELECT NumeroVolontari, NumeroEnti FROM VW_WS_CONTATORI " ' vecchia versione
            Dim CMD As New SqlClient.SqlDataAdapter(strSql, sqlConn)

            dtsLocal.DataSetName = "Contatori"

            'dtsLocal.Tables(0).TableName = "ENTI"

            CMD.Fill(dtsLocal)

            dtsLocal.Tables(0).TableName = "DettaglioContatori"

            xmlLocal = dtsLocal.GetXml

            EseguiRicercaContatori = xmlLocal

            sqlConn.Close()

            Return EseguiRicercaContatori
        Catch ex As Exception
            Dim myLog As New GeneraLog(ex.Message, "EseguiRicercaContatori")
            myLog.GeneraFileLog()
        End Try
    End Function

    <WebMethod()> _
    Public Function EseguiAutenticazioneVolontario(ByVal strUserName As String, ByVal strPWD As String) As String
        Dim wrapper As New Simple3Des
        Dim strSql As String
        Dim sqlConn As New SqlClient.SqlConnection
        Dim xmlLocal As String
        Dim dtR As DataRow
        Dim dtT As New DataTable
        Dim dtsLocal As New DataSet
        Dim myCommand As New SqlClient.SqlCommand
        Dim dtrLocal As SqlClient.SqlDataReader
        Dim strIdentificativo As String
        Dim strDecriptUser As String = wrapper.DecryptData(strUserName).ToString.Replace("'", "''")
        Dim strDecriptPSW As String = CreatePSW(wrapper.DecryptData(strPWD)).ToString.Replace("'", "''")

        'Dim strDecriptUser As String = strUserName.ToString.Replace("'", "''")
        'Dim strDecriptPSW As String = CreatePSW(strPWD).ToString.Replace("'", "''")
        Try


            sqlConn.ConnectionString = System.Configuration.ConfigurationManager.AppSettings("cnHelios")

            sqlConn.Open()

            strSql = "select Denominazione, Username, Password, Identificativo,IdStatoEnte,getdate() as dataserver, Tipo, FlagForzaturaAccreditamento, CambioPWD from VW_Account_Utenti where username='" & strDecriptUser & "' and password='" & strDecriptPSW & "' and heliosRead=0 "

            myCommand = New SqlClient.SqlCommand
            myCommand.Connection = sqlConn

            'controllo e chiudo se aperto il datareader
            If Not dtrLocal Is Nothing Then
                dtrLocal.Close()
                dtrLocal = Nothing
            End If

            'eseguo la query
            myCommand.CommandText = strSql
            dtrLocal = myCommand.ExecuteReader

            dtT.Columns.Add(New DataColumn("Esito", GetType(String)))
            dtT.Columns.Add(New DataColumn("CodiceEnte", GetType(String)))
            dtT.Columns.Add(New DataColumn("Competenza", GetType(String)))
            dtT.Columns.Add(New DataColumn("TipoUtente", GetType(String)))
            dtT.Columns.Add(New DataColumn("CodiceVolontario", GetType(String)))
            dtT.Columns.Add(New DataColumn("Elettorato", GetType(String)))
            dtT.Columns.Add(New DataColumn("QuestionarioFineServizio", GetType(String)))
            dtT.Columns.Add(New DataColumn("ContrattoVolontario", GetType(String)))
            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'imposto a 0 (Non compilabile) il flag del questionario di fine servizio
            dtR(6) = "0"

            'controllo se l'autanticazine è andata a buon fine
            If dtrLocal.HasRows = True Then
                dtrLocal.Read()
                strIdentificativo = dtrLocal("Identificativo")

                If dtrLocal("CambioPWD") = False And dtrLocal("Tipo") = "V" Then

                    'carico la prima riga della datatable
                    dtR(0) = "CAMBIO PASSWORD"

                    'controllo e chiudo se aperto il datareader
                    If Not dtrLocal Is Nothing Then
                        dtrLocal.Close()
                        dtrLocal = Nothing
                    End If

                    'ESTRAGGO IL CODICE ENTE
                    strSql = "Select entità.codicevolontario, enti.CodiceRegione,DBO.FN_WS_ABILITA_DOWNLOAD_CONTRATTO(entità.IdEntità) AS ABILITA_CONTRATTO  FROM  attività INNER JOIN  attivitàentisediattuazione ON attività.IDAttività = attivitàentisediattuazione.IDAttività INNER JOIN " & _
                             " attivitàentità ON attivitàentisediattuazione.IDAttivitàEnteSedeAttuazione = attivitàentità.IDAttivitàEnteSedeAttuazione INNER JOIN  enti ON attività.IDEntePresentante = enti.IDEnte INNER JOIN " & _
                             " entità ON attivitàentità.IDEntità = entità.IDEntità  WHERE attivitàentità.IDEntità = " & strIdentificativo
                    myCommand = New SqlClient.SqlCommand
                    myCommand.Connection = sqlConn
                    myCommand.CommandText = strSql
                    dtrLocal = myCommand.ExecuteReader
                    dtrLocal.Read()
                    If dtrLocal.HasRows = True Then
                        dtR(1) = dtrLocal("CodiceRegione")
                        dtR(4) = dtrLocal("CodiceVolontario")
                        dtR(5) = ReadStatoElettorato(dtrLocal("CodiceVolontario"))
                        dtR(6) = ReadStatoQuestionario(dtrLocal("CodiceVolontario"))
                        'verifica abilitazione a scaricare il contratto 0: non abilitato ,1: abilitato
                        If dtrLocal("ABILITA_CONTRATTO") = "OK" Then
                            dtR(7) = "1"
                        Else
                            dtR(7) = "0"
                        End If

                    Else
                        dtR(1) = ""
                        dtR(4) = ""
                        dtR(5) = "0"
                        dtR(6) = "0"
                        dtR(7) = "0"
                    End If
                    dtR(2) = ""
                    dtR(3) = "V"

                Else
                    'carico la prima riga della datatable
                    dtR(0) = "POSITIVO"

                    Select Case dtrLocal("Tipo")
                        Case "V"                        'VOLONTARIO

                            'controllo e chiudo se aperto il datareader
                            If Not dtrLocal Is Nothing Then
                                dtrLocal.Close()
                                dtrLocal = Nothing
                            End If

                            'ESTRAGGO IL CODICE ENTE
                            strSql = "Select entità.codicevolontario, enti.CodiceRegione, DBO.FN_WS_ABILITA_DOWNLOAD_CONTRATTO(entità.IdEntità) AS ABILITA_CONTRATTO  FROM  attività INNER JOIN  attivitàentisediattuazione ON attività.IDAttività = attivitàentisediattuazione.IDAttività INNER JOIN " & _
                                     " attivitàentità ON attivitàentisediattuazione.IDAttivitàEnteSedeAttuazione = attivitàentità.IDAttivitàEnteSedeAttuazione INNER JOIN  enti ON attività.IDEntePresentante = enti.IDEnte INNER JOIN " & _
                                     " entità ON attivitàentità.IDEntità = entità.IDEntità  WHERE attivitàentità.IDEntità = " & strIdentificativo
                            myCommand = New SqlClient.SqlCommand
                            myCommand.Connection = sqlConn
                            myCommand.CommandText = strSql
                            dtrLocal = myCommand.ExecuteReader
                            dtrLocal.Read()
                            If dtrLocal.HasRows = True Then
                                dtR(1) = dtrLocal("CodiceRegione")
                                dtR(4) = dtrLocal("CodiceVolontario")
                                dtR(5) = ReadStatoElettorato(dtrLocal("CodiceVolontario"))
                                dtR(6) = ReadStatoQuestionario(dtrLocal("CodiceVolontario"))
                                'verifica abilitazione a scaricare il contratto 0: non abilitato ,1: abilitato
                                If dtrLocal("ABILITA_CONTRATTO") = "OK" Then
                                    dtR(7) = "1"
                                Else
                                    dtR(7) = "0"
                                End If
                            Else
                                dtR(1) = ""
                                dtR(4) = ""
                                dtR(5) = "0"
                                dtR(6) = "0"
                                dtR(7) = "0"
                            End If
                            dtR(2) = ""
                            dtR(3) = "V"

                        Case "R"                        'REGIONE

                            'controllo e chiudo se aperto il datareader
                            If Not dtrLocal Is Nothing Then
                                dtrLocal.Close()
                                dtrLocal = Nothing
                            End If

                            strSql = "SELECT DESCRIZIONE AS REGIONECOMPETENZA FROM REGIONICOMPETENZE A " & _
                                     " INNER JOIN UTENTIUNSC B ON A.IDREGIONECOMPETENZA = B.IDREGIONECOMPETENZA " & _
                                     " WHERE USERNAME = '" & strDecriptUser & "'"
                            myCommand = New SqlClient.SqlCommand
                            myCommand.Connection = sqlConn
                            myCommand.CommandText = strSql
                            dtrLocal = myCommand.ExecuteReader
                            dtrLocal.Read()
                            If dtrLocal.HasRows = True Then
                                dtR(2) = dtrLocal("REGIONECOMPETENZA")
                            Else
                                dtR(2) = ""
                            End If
                            dtR(1) = ""
                            dtR(3) = "R"
                            dtR(4) = ""
                            dtR(5) = "0"
                            dtR(6) = "0"
                            dtR(7) = "0"
                        Case "E"                        'ENTE

                            'controllo e chiudo se aperto il datareader
                            If Not dtrLocal Is Nothing Then
                                dtrLocal.Close()
                                dtrLocal = Nothing
                            End If

                            strSql = "Select isnull(codiceregione,'') AS CODICEENTE, isnull(descrizione,'') AS REGIONECOMPETENZA " & _
                                     " from enti inner join regionicompetenze on enti.idregionecompetenza = regionicompetenze.idregionecompetenza " & _
                                     " where idente = " & strIdentificativo
                            myCommand = New SqlClient.SqlCommand
                            myCommand.Connection = sqlConn
                            myCommand.CommandText = strSql
                            dtrLocal = myCommand.ExecuteReader
                            dtrLocal.Read()
                            If dtrLocal.HasRows = True Then
                                dtR(1) = dtrLocal("CODICEENTE")
                                dtR(2) = dtrLocal("REGIONECOMPETENZA")
                            Else
                                dtR(1) = ""
                                dtR(2) = ""
                            End If
                            dtR(3) = "E"
                            dtR(4) = ""
                            dtR(5) = "0"
                            dtR(6) = "0"
                            dtR(7) = "0"
                        Case "U"                        'UNSC

                            dtR(1) = ""
                            dtR(2) = ""
                            dtR(3) = "U"
                            dtR(4) = ""
                            dtR(5) = "0"
                            dtR(6) = "0"
                            dtR(7) = "0"
                    End Select
                End If
            Else
                'carico la prima riga della datatable
                dtR(0) = "NEGATIVO"
                dtR(5) = "0"
                dtR(6) = "0"
                dtR(7) = "0"
            End If
            'controllo e chiudo se aperto il datareader
            If Not dtrLocal Is Nothing Then
                dtrLocal.Close()
                dtrLocal = Nothing
            End If


            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)

            dtsLocal.Tables.Add(dtT)

            dtsLocal.DataSetName = "EsitoAutenticazione"

            dtsLocal.Tables(0).TableName = "DettaglioEsito"

            xmlLocal = dtsLocal.GetXml

            EseguiAutenticazioneVolontario = xmlLocal

            sqlConn.Close()

            Return EseguiAutenticazioneVolontario

        Catch ex As Exception
            Dim myLog As New GeneraLog(ex.Message, "EseguiAutenticazioneVolontario")
            myLog.GeneraFileLog()
        End Try
    End Function

    '<WebMethod()> _
    Public Function EseguiAutenticazioneVolontarioXSD() As System.Xml.XmlDocument
        Dim xmlLocal As String
        Dim dtR As DataRow
        Dim dtT As New DataTable
        Dim dtsLocal As New DataSet

        dtT.Columns.Add(New DataColumn("Esito", GetType(String)))
        dtT.Columns.Add(New DataColumn("CodiceEnte", GetType(String)))
        dtT.Columns.Add(New DataColumn("Competenza", GetType(String)))
        dtT.Columns.Add(New DataColumn("TipoUtente", GetType(String)))
        dtT.Columns.Add(New DataColumn("Elettorato", GetType(String)))
        dtT.Columns.Add(New DataColumn("QuestionarioFineServizio", GetType(String)))
        dtT.Columns.Add(New DataColumn("ContrattoVolontario", GetType(String)))

        'dtT.Columns.Add()
        'alla riga assegno la riga delle intestazioni appena creata
        dtR = dtT.NewRow()

        'carico la prima riga della datatable
        dtR(0) = ""

        'aggiungo la riga appena caricata alla datatable
        dtT.Rows.Add(dtR)

        dtsLocal.Tables.Add(dtT)

        dtsLocal.DataSetName = "EsitoAutenticazione"

        dtsLocal.Tables(0).TableName = "DettaglioEsito"

        Dim testXML As System.Xml.XmlNode
        'Dim xmlDOC As New System.Xml.XmlDataDocument
        Dim xmlDOC As New System.Xml.XmlDocument

        xmlDOC.InnerXml = dtsLocal.GetXmlSchema

        EseguiAutenticazioneVolontarioXSD = xmlDOC

        Return EseguiAutenticazioneVolontarioXSD
        'Return "Salve gente!"
    End Function

    <WebMethod()> _
Public Function EseguiRicercaCandidatiPerEvento(ByVal annoEvento As Integer) As String
        Dim strSql As String
        Dim sqlConn As New SqlClient.SqlConnection
        Dim xmlLocal As String
        Dim dtT As New DataTable
        Dim dtsLocal As New DataSet

        'crea le intestazioni del candidato per il file xml
        dtT.Columns.Add(New DataColumn("CodiceVolontario", GetType(String)))
        dtT.Columns.Add(New DataColumn("Cognome", GetType(String)))
        dtT.Columns.Add(New DataColumn("Nome", GetType(String)))
        dtT.Columns.Add(New DataColumn("Email", GetType(String)))
        dtT.Columns.Add(New DataColumn("IDRegione", GetType(String)))

        Try
            sqlConn.ConnectionString = System.Configuration.ConfigurationManager.AppSettings("cnElezioni")

            sqlConn.Open()

            'cerca i dati dei candidati per quell'evento
            strSql = "select CodiceVolontario, Cognome, Nome, Email, IdRegione from wwwElencoCandidati where AnnoCandidatura=" & annoEvento

            Dim CMD As New SqlClient.SqlDataAdapter(strSql, sqlConn)

            CMD.Fill(dtsLocal)

        Catch ex As Exception
            Dim myLog As New GeneraLog(ex.Message, "EseguiRicercaCandidatiPerEvento")
            myLog.GeneraFileLog()
        Finally
            dtsLocal.DataSetName = "CandidatiPerEvento"

            If Not dtT Is Nothing Then
                dtsLocal.Tables.Add(dtT)
                dtsLocal.Tables(0).TableName = "Candidato"
            End If

            xmlLocal = dtsLocal.GetXml

            EseguiRicercaCandidatiPerEvento = xmlLocal

            If sqlConn.State = ConnectionState.Open Then
                sqlConn.Close()
            End If
        End Try

        Return EseguiRicercaCandidatiPerEvento

    End Function

    '<WebMethod()> _
    Public Function EseguiRicercaCandidatiPerEventoXSD() As System.Xml.XmlDocument
        Dim strSql As String
        Dim sqlConn As New SqlClient.SqlConnection
        Dim xmlLocal As String
        Dim dtR As DataRow
        Dim dtT As New DataTable
        Dim dtsLocal As New DataSet

        dtT.Columns.Add(New DataColumn("CodiceVolontario", GetType(String)))
        dtT.Columns.Add(New DataColumn("Cognome", GetType(String)))
        dtT.Columns.Add(New DataColumn("Nome", GetType(String)))
        dtT.Columns.Add(New DataColumn("Email", GetType(String)))
        dtT.Columns.Add(New DataColumn("IDRegione", GetType(String)))
        dtR = dtT.NewRow()
        'aggiungo la riga appena caricata alla datatable
        dtT.Rows.Add(dtR)

        dtsLocal.DataSetName = "CandidatiPerEvento"

        If Not dtT Is Nothing Then
            dtsLocal.Tables.Add(dtT)
            dtsLocal.Tables(0).TableName = "Candidato"
        End If

        Dim testXML As System.Xml.XmlNode
        Dim xmlDOC As New System.Xml.XmlDocument

        xmlDOC.InnerXml = dtsLocal.GetXmlSchema

        EseguiRicercaCandidatiPerEventoXSD = xmlDOC

        sqlConn.Close()

        Return EseguiRicercaCandidatiPerEventoXSD

    End Function

    <WebMethod()> _
Public Function EseguiRicercaVotiPerEvento(ByVal annoEvento As Integer) As String
        Dim strSql As String
        Dim sqlConn As New SqlClient.SqlConnection
        Dim xmlLocal As String
        Dim dtT As New DataTable
        Dim dtsLocal As New DataSet

        'crea le intestazioni del candidato per il file xml
        dtT.Columns.Add(New DataColumn("CodiceVolontario", GetType(String)))
        dtT.Columns.Add(New DataColumn("NumeroVoti", GetType(Integer)))

        Try
            sqlConn.ConnectionString = System.Configuration.ConfigurationManager.AppSettings("cnElezioni")

            sqlConn.Open()

            'strSql = "SELECT top 1 DescrizioneEvento from wwwEventi where " & annoEvento & " between year(dataInizioEvento) and year(dataFineEvento)"

            'cerca i dati dei candidati per quell'evento
            strSql = "select CodiceVolontario, NumeroVoti from wwwElencoCandidati where AnnoCandidatura=" & annoEvento

            Dim CMD As New SqlClient.SqlDataAdapter(strSql, sqlConn)

            CMD.Fill(dtT)

        Catch ex As Exception
            Dim myLog As New GeneraLog(ex.Message, "EseguiRicercaVotiPerEvento")
            myLog.GeneraFileLog()
        Finally
            dtsLocal.DataSetName = "VotiPerEvento"

            If Not dtT Is Nothing Then
                dtsLocal.Tables.Add(dtT)
                dtsLocal.Tables(0).TableName = "Candidato"
            End If

            xmlLocal = dtsLocal.GetXml

            EseguiRicercaVotiPerEvento = xmlLocal

            If sqlConn.State = ConnectionState.Open Then
                sqlConn.Close()
            End If
        End Try

        Return EseguiRicercaVotiPerEvento

    End Function

    '<WebMethod()> _
    Public Function EseguiRicercaVotiPerEventoXSD() As System.Xml.XmlDocument
        Dim strSql As String
        Dim sqlConn As New SqlClient.SqlConnection
        Dim xmlLocal As String
        Dim dtR As DataRow
        Dim dtT As New DataTable
        Dim dtsLocal As New DataSet

        dtT.Columns.Add(New DataColumn("CodiceVolontario", GetType(String)))
        dtT.Columns.Add(New DataColumn("NumeroVoti", GetType(Integer)))
        dtR = dtT.NewRow()

        'aggiungo la riga appena caricata alla datatable
        dtT.Rows.Add(dtR)

        dtsLocal.DataSetName = "VotiPerEvento"

        If Not dtT Is Nothing Then
            dtsLocal.Tables.Add(dtT)
            dtsLocal.Tables(0).TableName = "Candidato"
        End If

        Dim testXML As System.Xml.XmlNode
        Dim xmlDOC As New System.Xml.XmlDocument

        xmlDOC.InnerXml = dtsLocal.GetXmlSchema

        EseguiRicercaVotiPerEventoXSD = xmlDOC

        sqlConn.Close()

        Return EseguiRicercaVotiPerEventoXSD

    End Function

    Function ReadStatoElettorato(ByVal strCodVol As String) As String
        Dim strSql As String
        Dim sqlConn As New SqlClient.SqlConnection
        Dim myCommand As New SqlClient.SqlCommand
        Dim myParameter As New SqlParameter
        Dim dtrLocal As SqlClient.SqlDataReader
        Dim strElettorato As String

        'imposto se il volontario è candidato o meno
        strElettorato = "0"

        Try

            sqlConn.ConnectionString = System.Configuration.ConfigurationManager.AppSettings("cnElezioni")

            sqlConn.Open()
            myCommand = New SqlClient.SqlCommand
            myCommand.Connection = sqlConn

            strSql = "wwwStatoElettore"
            myParameter.ParameterName = "@codiceVolontario"
            myParameter.Value = strCodVol

            'controllo e chiudo se aperto il datareader
            If Not dtrLocal Is Nothing Then
                dtrLocal.Close()
                dtrLocal = Nothing
            End If

            'eseguo la query
            myCommand.CommandType = CommandType.StoredProcedure
            myCommand.Parameters.Add(myParameter)
            myCommand.CommandText = strSql
            strElettorato = Convert.ToString(myCommand.ExecuteScalar())

        Catch ex As Exception
            strElettorato = "0"

            Dim myLog As New GeneraLog(ex.Message, "ReadStatoelettorato")
            myLog.GeneraFileLog()
        Finally
            'controllo e chiudo se aperto il datareader
            If Not dtrLocal Is Nothing Then
                dtrLocal.Close()
                dtrLocal = Nothing
            End If

            If sqlConn.State = ConnectionState.Open Then
                sqlConn.Close()
            End If
        End Try

        ReadStatoElettorato = strElettorato
    End Function

    Function ReadStatoQuestionario(ByVal strCodVol As String) As String
        Dim strSql As String
        Dim sqlConn As New SqlClient.SqlConnection
        Dim myCommand As New SqlClient.SqlCommand
        Dim myParameter As New SqlParameter
        Dim dtrLocal As SqlClient.SqlDataReader
        Dim strQuestionario As String

        'imposto il flag del questionario di fine servizio
        strQuestionario = "0"

        Try

            sqlConn.ConnectionString = System.Configuration.ConfigurationManager.AppSettings("cnHelios")
            sqlConn.Open()

            strSql = "select idStatoQuestionario from VW_Stato_Questionario where codicevolontario ='" + strCodVol + "'"

            myCommand = New SqlClient.SqlCommand
            myCommand.Connection = sqlConn

            'controllo e chiudo se aperto il datareader
            If Not dtrLocal Is Nothing Then
                dtrLocal.Close()
                dtrLocal = Nothing
            End If

            'eseguo la query
            myCommand.CommandText = strSql
            dtrLocal = myCommand.ExecuteReader

            If dtrLocal.HasRows = True Then
                dtrLocal.Read()

                ' Estraggo il Flag per il questionario di fine swervizio
                '   (0 ==> 'Non compilabile', 1: 'Compilabile', 2: 'Compilato')
                strQuestionario = dtrLocal("idStatoQuestionario")

                'controllo e chiudo se aperto il datareader
                If Not dtrLocal Is Nothing Then
                    dtrLocal.Close()
                    dtrLocal = Nothing
                End If
            End If

        Catch ex As Exception
            strQuestionario = "0"

            Dim myLog As New GeneraLog(ex.Message, "ReadStatoQuestionario")
            myLog.GeneraFileLog()
        Finally
            'controllo e chiudo se aperto il datareader
            If Not dtrLocal Is Nothing Then
                dtrLocal.Close()
                dtrLocal = Nothing
            End If

            If sqlConn.State = ConnectionState.Open Then
                sqlConn.Close()
            End If
        End Try

        ReadStatoQuestionario = strQuestionario
    End Function

    Function ReadPsw(ByVal cPsw As String) As String
        'Funzione per la lettura della password
        'Ritorna la password decriptata
        Try
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
        Catch ex As Exception
            Dim myLog As New GeneraLog(ex.Message, "EseguiAutenticazioneVolontario")
            myLog.GeneraFileLog()
        End Try
    End Function


    Function CreatePSW(ByVal cPsw As String) As String
        ' Funzione per la creazione della stringa criptata
        ' Ritorna la password criptata
        Try
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
        Catch ex As Exception
            Dim myLog As New GeneraLog(ex.Message, "CreatePSW")
            myLog.GeneraFileLog()
        End Try
    End Function

    Function pswChrAllowed(ByVal KeyAscii) As String
        If (KeyAscii < 32 Or KeyAscii > 122 Or KeyAscii = 38 Or KeyAscii = 39) And KeyAscii <> 8 Then
            KeyAscii = 0
        End If
        pswChrAllowed = KeyAscii
    End Function

    <WebMethod()> _
Public Function EseguiModificaPwdVolontario(ByVal strUserName As String, ByVal strVecchiaPWD As String, ByVal strNuovaPWD As String) As String
        Dim wrapper As New Simple3Des
        Dim strSql, strSqlElez As String
        Dim sqlConn As New SqlClient.SqlConnection
        Dim sqlConnElez As New SqlClient.SqlConnection
        Dim xmlLocal As String
        Dim dtR As DataRow
        Dim dtT As New DataTable
        Dim dtsLocal As New DataSet
        Dim myCommand As New SqlClient.SqlCommand
        Dim myCommandElez As New SqlClient.SqlCommand
        Dim dtrLocal As SqlClient.SqlDataReader
        Dim strTipoUtente As String
        Dim intIdEntita As Integer

        Try
            'strVecchiaPWD = wrapper.EncryptData(strVecchiaPWD)
            'strNuovaPWD = wrapper.EncryptData(strNuovaPWD)
            'strUserName = wrapper.EncryptData(strUserName)

            'aggiungo cmq la colonna dell'esito
            dtT.Columns.Add(New DataColumn("Esito", GetType(String)))
            dtT.Columns.Add(New DataColumn("CausaleEsitoNegativo", GetType(String)))

            If strUserName = "" Or strVecchiaPWD = "" Or strNuovaPWD = "" Then
                'dtT.Columns.Add()
                'alla riga assegno la riga delle intestazioni appena creata
                dtR = dtT.NewRow()

                'carico la prima riga della datatable
                dtR(0) = "NEGATIVO"
                dtR(1) = "TUTTI I CAMPI SONO NECESSARI"

                'aggiungo la riga appena caricata alla datatable
                dtT.Rows.Add(dtR)

                dtsLocal.Tables.Add(dtT)

                dtsLocal.DataSetName = "EsitoAutenticazione"

                dtsLocal.Tables(0).TableName = "DettaglioEsito"

                xmlLocal = dtsLocal.GetXml

                EseguiModificaPwdVolontario = xmlLocal

                Return EseguiModificaPwdVolontario
            Else
                If wrapper.DecryptData(strUserName) = "" Or wrapper.DecryptData(strVecchiaPWD) = "" Or wrapper.DecryptData(strNuovaPWD) = "" Then
                    'dtT.Columns.Add()
                    'alla riga assegno la riga delle intestazioni appena creata
                    dtR = dtT.NewRow()

                    'carico la prima riga della datatable
                    dtR(0) = "NEGATIVO"
                    dtR(1) = "TUTTI I CAMPI SONO NECESSARI"

                    'aggiungo la riga appena caricata alla datatable
                    dtT.Rows.Add(dtR)

                    dtsLocal.Tables.Add(dtT)

                    dtsLocal.DataSetName = "EsitoAutenticazione"

                    dtsLocal.Tables(0).TableName = "DettaglioEsito"

                    xmlLocal = dtsLocal.GetXml

                    EseguiModificaPwdVolontario = xmlLocal

                    Return EseguiModificaPwdVolontario
                End If
            End If


            sqlConn.ConnectionString = System.Configuration.ConfigurationManager.AppSettings("cnHelios")

            sqlConn.Open()

            strSql = "select Denominazione, Username, Password, Identificativo,IdStatoEnte,getdate() as dataserver, Tipo, FlagForzaturaAccreditamento from VW_Account_Utenti where username='" & wrapper.DecryptData(strUserName) & "' and password='" & CreatePSW(wrapper.DecryptData(strVecchiaPWD)).Replace("'", "''") & "' and heliosRead=0 "

            'sql command che mi esegue la insert
            myCommand = New SqlClient.SqlCommand
            myCommand.Connection = sqlConn

            'controllo e chiudo se aperto il datareader
            If Not dtrLocal Is Nothing Then
                dtrLocal.Close()
                dtrLocal = Nothing
            End If

            'eseguo la query
            myCommand.CommandText = strSql
            dtrLocal = myCommand.ExecuteReader

            'se l'account esiste vado a controllare la correttezza della nuova pwd
            If dtrLocal.HasRows = True Then
                Dim intX As Integer
                Dim blnCheckNum As Boolean = False
                Dim blnCheck As Boolean = False
                Dim strTutteLettereMaiuscole As String = "ABCDEFGHIJKLMNOPQRSTUVWXYZ"
                Dim strTutteLettereMinuscole As String = "abcdefghijklmnopqrstuvwxyz"
                Dim strTuttiNumeri As String = "1234567890"

                'controllo la lunghezza della nuova pwd
                If Len(wrapper.DecryptData(strNuovaPWD)) = 8 Then
                    'se la nuova pws è lunga 8 vado a controllare se c'è almeno un numero e un carattere 
                    For intX = 1 To Len(wrapper.DecryptData((strNuovaPWD)))
                        If InStr(strTutteLettereMaiuscole, Mid(wrapper.DecryptData(strNuovaPWD), intX, 1)) > 0 Or InStr(strTutteLettereMinuscole, Mid(wrapper.DecryptData(strNuovaPWD), intX, 1)) > 0 Then
                            blnCheck = True
                        End If
                    Next

                    'se la pwd NON è alfanumerica rimando all'utente la segnalazione
                    If blnCheck = True Then
                        For intX = 1 To Len(wrapper.DecryptData(strNuovaPWD))
                            If InStr(strTuttiNumeri, Mid(wrapper.DecryptData(strNuovaPWD), intX, 1)) > 0 Then
                                blnCheckNum = True
                            End If
                        Next

                        If blnCheckNum = False Then
                            'controllo e chiudo se aperto il datareader
                            If Not dtrLocal Is Nothing Then
                                dtrLocal.Close()
                                dtrLocal = Nothing
                            End If

                            'dtT.Columns.Add()
                            'alla riga assegno la riga delle intestazioni appena creata
                            dtR = dtT.NewRow()

                            'carico la prima riga della datatable
                            dtR(0) = "NEGATIVO"
                            dtR(1) = "LA NUOVA PASSWORD DEV'ESSERE ALFANUMERICA"

                            'aggiungo la riga appena caricata alla datatable
                            dtT.Rows.Add(dtR)

                            dtsLocal.Tables.Add(dtT)

                            dtsLocal.DataSetName = "EsitoAutenticazione"

                            dtsLocal.Tables(0).TableName = "DettaglioEsito"

                            xmlLocal = dtsLocal.GetXml

                            EseguiModificaPwdVolontario = xmlLocal

                            sqlConn.Close()

                            Return EseguiModificaPwdVolontario
                        Else
                            dtrLocal.Read()

                            strTipoUtente = dtrLocal("Tipo")
                            intIdEntita = dtrLocal("Identificativo")

                            'controllo il tipo di utente per controllare su che tabella fare la query
                            Select Case strTipoUtente

                                Case "U"
                                    'controllo e chiudo se aperto il datareader
                                    If Not dtrLocal Is Nothing Then
                                        dtrLocal.Close()
                                        dtrLocal = Nothing
                                    End If

                                    'dtT.Columns.Add()
                                    'alla riga assegno la riga delle intestazioni appena creata
                                    dtR = dtT.NewRow()

                                    'carico la prima riga della datatable
                                    dtR(0) = "NEGATIVO"
                                    dtR(1) = "VERIFICARE ACCOUNT"

                                    'aggiungo la riga appena caricata alla datatable
                                    dtT.Rows.Add(dtR)

                                    dtsLocal.Tables.Add(dtT)

                                    dtsLocal.DataSetName = "EsitoAutenticazione"

                                    dtsLocal.Tables(0).TableName = "DettaglioEsito"

                                    xmlLocal = dtsLocal.GetXml

                                    EseguiModificaPwdVolontario = xmlLocal

                                    sqlConn.Close()

                                    Return EseguiModificaPwdVolontario
                                Case "E"
                                    'controllo e chiudo se aperto il datareader
                                    If Not dtrLocal Is Nothing Then
                                        dtrLocal.Close()
                                        dtrLocal = Nothing
                                    End If

                                    'dtT.Columns.Add()
                                    'alla riga assegno la riga delle intestazioni appena creata
                                    dtR = dtT.NewRow()

                                    'carico la prima riga della datatable
                                    dtR(0) = "NEGATIVO"
                                    dtR(1) = "VERIFICARE ACCOUNT"

                                    'aggiungo la riga appena caricata alla datatable
                                    dtT.Rows.Add(dtR)

                                    dtsLocal.Tables.Add(dtT)

                                    dtsLocal.DataSetName = "EsitoAutenticazione"

                                    dtsLocal.Tables(0).TableName = "DettaglioEsito"

                                    xmlLocal = dtsLocal.GetXml

                                    EseguiModificaPwdVolontario = xmlLocal

                                    sqlConn.Close()

                                    Return EseguiModificaPwdVolontario
                                Case "V"
                                    'controllo e chiudo se aperto il datareader
                                    If Not dtrLocal Is Nothing Then
                                        dtrLocal.Close()
                                        dtrLocal = Nothing
                                    End If

                                    Try
                                        'sql command che mi esegue la insert
                                        myCommand = New SqlClient.SqlCommand
                                        myCommand.Connection = sqlConn

                                        Dim strPWD As String = CreatePSW(wrapper.DecryptData(strNuovaPWD))

                                        If strPWD Is Nothing Or strPWD = "" Then
                                            'controllo e chiudo se aperto il datareader
                                            If Not dtrLocal Is Nothing Then
                                                dtrLocal.Close()
                                                dtrLocal = Nothing
                                            End If

                                            'dtT.Columns.Add()
                                            'alla riga assegno la riga delle intestazioni appena creata
                                            dtR = dtT.NewRow()

                                            'carico la prima riga della datatable
                                            dtR(0) = "NEGATIVO"
                                            dtR(1) = "LA PASSWORD INSERITA CONTIENE CARATTERI NON VALIDI"

                                            'aggiungo la riga appena caricata alla datatable
                                            dtT.Rows.Add(dtR)

                                            dtsLocal.Tables.Add(dtT)

                                            dtsLocal.DataSetName = "EsitoAutenticazione"

                                            dtsLocal.Tables(0).TableName = "DettaglioEsito"

                                            xmlLocal = dtsLocal.GetXml

                                            EseguiModificaPwdVolontario = xmlLocal

                                            sqlConn.Close()

                                            Return EseguiModificaPwdVolontario
                                        Else

                                            strSql = "INSERT INTO CronologiaEntitàWebPassword (IdEntità, Username, DataCronologia, VecchiaPassword, NuovaPassword) values (" & intIdEntita & ", '" & wrapper.DecryptData(strUserName) & "', GetDate(), '" & wrapper.DecryptData(strVecchiaPWD) & "', '" & wrapper.DecryptData(strNuovaPWD) & "')"

                                            'eseguo la query
                                            myCommand.CommandText = strSql
                                            myCommand.ExecuteNonQuery()

                                            strSql = "UPDATE entità set Password='" & strPWD.Replace("'", "''") & "', abilitato=1 WHERE identità=" & intIdEntita

                                            'eseguo la query
                                            myCommand.CommandText = strSql
                                            myCommand.ExecuteNonQuery()

                                            'aggiorno la password del volontario nel DB Elezioni
                                            sqlConnElez.ConnectionString = System.Configuration.ConfigurationManager.AppSettings("cnElezioni")
                                            sqlConnElez.Open()

                                            myCommandElez = New SqlClient.SqlCommand
                                            myCommandElez.Connection = sqlConnElez

                                            strSqlElez = "UPDATE VolontariPerElezione set Password='" & wrapper.DecryptData(strNuovaPWD) & "' WHERE Username='" & wrapper.DecryptData(strUserName) & "' AND CodiceEvento in (select CodiceEvento FROM evsc_eventi WHERE year(DataFineEvento)=year(getdate()))"

                                            myCommandElez.CommandText = strSqlElez
                                            myCommandElez.ExecuteNonQuery()

                                            If dtR Is Nothing Then
                                                dtR = dtT.NewRow()
                                            End If

                                            'carico la prima riga della datatable
                                            dtR(0) = "POSITIVO"
                                            dtR(1) = ""

                                        End If

                                    Catch ex As Exception
                                        Dim myLog As New GeneraLog(ex.Message, "EseguiModificaPwdVolontario - V")
                                        myLog.GeneraFileLog()

                                        If dtR Is Nothing Then
                                            dtR = dtT.NewRow()
                                        End If

                                        'carico la prima riga della datatable
                                        dtR(0) = "NEGATIVO"
                                        dtR(1) = "ERRORE: " & ex.Message
                                    Finally
                                        'controllo e chiudo se aperto il datareader
                                        If Not dtrLocal Is Nothing Then
                                            dtrLocal.Close()
                                            dtrLocal = Nothing
                                        End If

                                        'aggiungo la riga appena caricata alla datatable
                                        dtT.Rows.Add(dtR)

                                        dtsLocal.Tables.Add(dtT)

                                        dtsLocal.DataSetName = "EsitoAutenticazione"

                                        dtsLocal.Tables(0).TableName = "DettaglioEsito"

                                        xmlLocal = dtsLocal.GetXml

                                        EseguiModificaPwdVolontario = xmlLocal

                                        'chiudo le connessioni se serve
                                        If sqlConn.State = ConnectionState.Open Then
                                            sqlConn.Close()
                                        End If

                                        If sqlConnElez.State = ConnectionState.Open Then
                                            sqlConnElez.Close()
                                        End If

                                    End Try

                                    'dtT.Columns.Add()
                                    'alla riga assegno la riga delle intestazioni appena creata
                                    'dtR = dtT.NewRow()

                                    'carico la prima riga della datatable
                                    'dtR(0) = "POSITIVO"
                                    'dtR(1) = ""

                                    Return EseguiModificaPwdVolontario

                            End Select

                        End If

                    Else

                        'controllo e chiudo se aperto il datareader
                        If Not dtrLocal Is Nothing Then
                            dtrLocal.Close()
                            dtrLocal = Nothing
                        End If

                        'dtT.Columns.Add()
                        'alla riga assegno la riga delle intestazioni appena creata
                        dtR = dtT.NewRow()

                        'carico la prima riga della datatable
                        dtR(0) = "NEGATIVO"
                        dtR(1) = "LA NUOVA PASSWORD DEV'ESSERE ALFANUMERICA"

                        'aggiungo la riga appena caricata alla datatable
                        dtT.Rows.Add(dtR)

                        dtsLocal.Tables.Add(dtT)

                        dtsLocal.DataSetName = "EsitoAutenticazione"

                        dtsLocal.Tables(0).TableName = "DettaglioEsito"

                        xmlLocal = dtsLocal.GetXml

                        EseguiModificaPwdVolontario = xmlLocal

                        sqlConn.Close()

                        Return EseguiModificaPwdVolontario
                    End If
                Else 'se la pwd non è lunga 8 ritorno con l'esito NEGATIVO
                    'controllo e chiudo se aperto il datareader
                    If Not dtrLocal Is Nothing Then
                        dtrLocal.Close()
                        dtrLocal = Nothing
                    End If

                    'dtT.Columns.Add()
                    'alla riga assegno la riga delle intestazioni appena creata
                    dtR = dtT.NewRow()

                    'carico la prima riga della datatable
                    dtR(0) = "NEGATIVO"
                    dtR(1) = "LA NUOVA PASSWORD DEV'ESSERE DI 8 CARATTERI"

                    'aggiungo la riga appena caricata alla datatable
                    dtT.Rows.Add(dtR)

                    dtsLocal.Tables.Add(dtT)

                    dtsLocal.DataSetName = "EsitoAutenticazione"

                    dtsLocal.Tables(0).TableName = "DettaglioEsito"

                    xmlLocal = dtsLocal.GetXml

                    EseguiModificaPwdVolontario = xmlLocal

                    sqlConn.Close()

                    Return EseguiModificaPwdVolontario

                End If

            Else 'se l'account inserito non esiste rimando un messaggio di esito negativo
                'controllo e chiudo se aperto il datareader
                If Not dtrLocal Is Nothing Then
                    dtrLocal.Close()
                    dtrLocal = Nothing
                End If

                'dtT.Columns.Add()
                'alla riga assegno la riga delle intestazioni appena creata
                dtR = dtT.NewRow()

                'carico la prima riga della datatable
                dtR(0) = "NEGATIVO"
                dtR(1) = "VECCHIA PASSWORD ERRATA"

                'aggiungo la riga appena caricata alla datatable
                dtT.Rows.Add(dtR)

                dtsLocal.Tables.Add(dtT)

                dtsLocal.DataSetName = "EsitoAutenticazione"

                dtsLocal.Tables(0).TableName = "DettaglioEsito"

                xmlLocal = dtsLocal.GetXml

                EseguiModificaPwdVolontario = xmlLocal

                sqlConn.Close()

                Return EseguiModificaPwdVolontario

            End If

            'controllo e chiudo se aperto il datareader
            If Not dtrLocal Is Nothing Then
                dtrLocal.Close()
                dtrLocal = Nothing
            End If

            Return EseguiModificaPwdVolontario
        Catch ex As Exception
            Dim myLog As New GeneraLog(ex.Message, "EseguiModificaPwdVolontario")
            myLog.GeneraFileLog()
        End Try
    End Function

    '<WebMethod()> _
    Public Function EseguiModificaPwdVolontarioXSD() As System.Xml.XmlDocument
        Dim strSql As String
        Dim sqlConn As New SqlClient.SqlConnection
        Dim xmlLocal As String
        Dim dtR As DataRow
        Dim dtT As New DataTable
        Dim dtsLocal As New DataSet
        Dim myCommand As New SqlClient.SqlCommand
        Dim dtrLocal As SqlClient.SqlDataReader

        dtT.Columns.Add(New DataColumn("Esito", GetType(String)))
        dtT.Columns.Add(New DataColumn("CausaleEsitoNegativo", GetType(String)))

        'alla riga assegno la riga delle intestazioni appena creata
        dtR = dtT.NewRow()

        'carico la prima riga della datatable
        dtR(0) = ""
        dtR(1) = ""

        'aggiungo la riga appena caricata alla datatable
        dtT.Rows.Add(dtR)

        dtsLocal.Tables.Add(dtT)

        dtsLocal.DataSetName = "EsitoAutenticazione"

        dtsLocal.Tables(0).TableName = "DettaglioEsito"

        Dim testXML As System.Xml.XmlNode
        'Dim xmlDOC As New System.Xml.XmlDataDocument
        Dim xmlDOC As New System.Xml.XmlDocument

        xmlDOC.InnerXml = dtsLocal.GetXmlSchema

        EseguiModificaPwdVolontarioXSD = xmlDOC

        Return EseguiModificaPwdVolontarioXSD

    End Function

    '<WebMethod()> _
    Public Function EseguiRicercaDettaglioEnteXSD() As System.Xml.XmlDocument
        Dim dtsLocal As New DataSet
        Dim xmlLocal As String
        Dim dtR As DataRow
        Dim dtT As New DataTable

        dtT.Columns.Add(New DataColumn("NSedi", GetType(Integer)))
        dtT.Columns.Add(New DataColumn("Regione", GetType(String)))
        dtT.Columns.Add(New DataColumn("Provincia", GetType(String)))
        dtT.Columns.Add(New DataColumn("Comune", GetType(String)))

        'alla riga assegno la riga delle intestazioni appena creata
        dtR = dtT.NewRow()

        'carico la prima riga della datatable
        dtR(0) = 0
        dtR(1) = ""
        dtR(2) = ""
        dtR(3) = ""

        'aggiungo la riga appena caricata alla datatable
        dtT.Rows.Add(dtR)

        dtsLocal.Tables.Add(dtT)

        dtsLocal.DataSetName = "DettaglioEnti"

        dtsLocal.Tables(0).TableName = "Dettaglio"

        Dim testXML As System.Xml.XmlNode
        'Dim xmlDOC As New System.Xml.XmlDataDocument
        Dim xmlDOC As New System.Xml.XmlDocument

        xmlDOC.InnerXml = dtsLocal.GetXmlSchema

        EseguiRicercaDettaglioEnteXSD = xmlDOC

        Return EseguiRicercaDettaglioEnteXSD

    End Function

    '<WebMethod()> _
    Public Function EseguiRicercaProgettoXSD() As System.Xml.XmlDocument
        Dim strSql As String
        Dim xmlLocal As String
        Dim dtR As DataRow
        Dim dtT As New DataTable
        Dim dtsLocal As New DataSet

        dtT.Columns.Add(New DataColumn("CODICEENTE", GetType(String)))
        dtT.Columns.Add(New DataColumn("ENTE", GetType(String)))
        dtT.Columns.Add(New DataColumn("SITO", GetType(String)))
        dtT.Columns.Add(New DataColumn("TITOLOPROGETTO", GetType(String)))
        dtT.Columns.Add(New DataColumn("TIPOPROGETTO", GetType(String)))
        dtT.Columns.Add(New DataColumn("REGIONE", GetType(String)))
        dtT.Columns.Add(New DataColumn("PROVINCIA", GetType(String)))
        dtT.Columns.Add(New DataColumn("COMUNE", GetType(String)))
        dtT.Columns.Add(New DataColumn("SETTORE", GetType(String)))
        dtT.Columns.Add(New DataColumn("AREA", GetType(String)))
        dtT.Columns.Add(New DataColumn("NVOL", GetType(Integer)))
        dtT.Columns.Add(New DataColumn("BANDOBREVE", GetType(String)))
        dtT.Columns.Add(New DataColumn("DATAFINEVOLONTARI", GetType(String)))
        dtT.Columns.Add(New DataColumn("IDGAZZETTA", GetType(String)))
        dtT.Columns.Add(New DataColumn("COMPETENZAENTE", GetType(String)))
        dtT.Columns.Add(New DataColumn("COMPETENZAPROGETTO", GetType(String)))

        'dtT.Columns.Add()
        'alla riga assegno la riga delle intestazioni appena creata
        dtR = dtT.NewRow()
        'controllo se l'autanticazine è andata a buon fine
        Dim intX As Integer

        For intX = 0 To 9
            dtR(intX) = ""
        Next
        dtR(10) = 0
        dtR(11) = ""
        dtR(12) = ""
        dtR(13) = ""
        dtR(14) = ""
        dtR(15) = ""

        'controllo e chiudo se aperto il datareader
        'If Not dtrLocal Is Nothing Then
        '    dtrLocal.Close()
        '    dtrLocal = Nothing
        'End If
        'aggiungo la riga appena caricata alla datatable
        dtT.Rows.Add(dtR)

        dtsLocal.Tables.Add(dtT)

        dtsLocal.DataSetName = "DettaglioProgetto"

        dtsLocal.Tables(0).TableName = "Dettaglio"

        Dim testXML As System.Xml.XmlNode
        'Dim xmlDOC As New System.Xml.XmlDataDocument
        Dim xmlDOC As New System.Xml.XmlDocument

        xmlDOC.InnerXml = dtsLocal.GetXmlSchema

        EseguiRicercaProgettoXSD = xmlDOC

        Return EseguiRicercaProgettoXSD
        'Return "Salve gente!"
    End Function

    <WebMethod()> _
    Public Function EseguiRichiestaStatiAttestato(ByVal strUserName As String, ByVal strPWD As String) As String
        Dim wrapper As New Simple3Des
        Dim strSql As String
        Dim sqlConn As New SqlClient.SqlConnection
        Dim xmlLocal As String
        Dim dtR As DataRow
        Dim dtT As New DataTable
        Dim dtsLocal As New DataSet
        Dim myCommand As New SqlClient.SqlCommand
        Dim dtrLocal As SqlClient.SqlDataReader
        Dim strTipoUtente As String
        Dim intIdEntita As Integer

        Try
            'strVecchiaPWD = wrapper.EncryptData(strVecchiaPWD)
            'strPWD = wrapper.EncryptData(strPWD)
            'strUserName = wrapper.EncryptData(strUserName)

            'aggiungo cmq la colonna dell'esito
            dtT.Columns.Add(New DataColumn("DataStato", GetType(String)))
            dtT.Columns.Add(New DataColumn("StatoAttestato", GetType(String)))
            dtT.Columns.Add(New DataColumn("Esito", GetType(String)))
            dtT.Columns.Add(New DataColumn("CausaleEsitoNegativo", GetType(String)))

            If strUserName = "" Or strPWD = "" Then
                'dtT.Columns.Add()
                'alla riga assegno la riga delle intestazioni appena creata
                dtR = dtT.NewRow()

                'carico la prima riga della datatable
                dtR(0) = ""
                dtR(1) = ""
                dtR(2) = "NEGATIVO"
                dtR(3) = "USERNAME O PASSWORD ERRATI"

                'aggiungo la riga appena caricata alla datatable
                dtT.Rows.Add(dtR)

                dtsLocal.Tables.Add(dtT)

                dtsLocal.DataSetName = "EsitoRichiestaStatiAttestato"

                dtsLocal.Tables(0).TableName = "DettaglioStatiAttestato"

                xmlLocal = dtsLocal.GetXml

                EseguiRichiestaStatiAttestato = xmlLocal

                Return EseguiRichiestaStatiAttestato
            Else
                If wrapper.DecryptData(strUserName) = "" Or wrapper.DecryptData(strPWD) = "" Then
                    'dtT.Columns.Add()
                    'alla riga assegno la riga delle intestazioni appena creata
                    dtR = dtT.NewRow()

                    'carico la prima riga della datatable
                    dtR(0) = ""
                    dtR(1) = ""
                    dtR(2) = "NEGATIVO"
                    dtR(3) = "USERNAME O PASSWORD ERRATI"

                    'aggiungo la riga appena caricata alla datatable
                    dtT.Rows.Add(dtR)

                    dtsLocal.Tables.Add(dtT)

                    dtsLocal.DataSetName = "EsitoRichiestaStatiAttestato"

                    dtsLocal.Tables(0).TableName = "DettaglioStatiAttestato"

                    xmlLocal = dtsLocal.GetXml

                    EseguiRichiestaStatiAttestato = xmlLocal

                    Return EseguiRichiestaStatiAttestato
                End If
            End If

            sqlConn.ConnectionString = System.Configuration.ConfigurationManager.AppSettings("cnHelios")

            sqlConn.Open()

            strSql = "select Denominazione, Username, Password, Identificativo,IdStatoEnte,getdate() as dataserver, Tipo, FlagForzaturaAccreditamento from VW_Account_Utenti where username='" & wrapper.DecryptData(strUserName) & "' and password='" & CreatePSW(wrapper.DecryptData(strPWD)).Replace("'", "''") & "' and heliosRead=0 "

            'sql command che mi esegue la insert
            myCommand = New SqlClient.SqlCommand
            myCommand.Connection = sqlConn

            'controllo e chiudo se aperto il datareader
            If Not dtrLocal Is Nothing Then
                dtrLocal.Close()
                dtrLocal = Nothing
            End If

            'eseguo la query
            myCommand.CommandText = strSql
            dtrLocal = myCommand.ExecuteReader

            If dtrLocal.HasRows = True Then
                dtrLocal.Read()

                'controllo e chiudo se aperto il datareader
                If Not dtrLocal Is Nothing Then
                    dtrLocal.Close()
                    dtrLocal = Nothing
                End If

                strSql = "select IdEntità from entità where username='" & wrapper.DecryptData(strUserName) & "' and password='" & CreatePSW(wrapper.DecryptData(strPWD)).Replace("'", "''") & "' "

                'sql command che mi esegue la insert
                myCommand = New SqlClient.SqlCommand
                myCommand.Connection = sqlConn

                'controllo e chiudo se aperto il datareader
                If Not dtrLocal Is Nothing Then
                    dtrLocal.Close()
                    dtrLocal = Nothing
                End If

                'eseguo la query
                myCommand.CommandText = strSql
                dtrLocal = myCommand.ExecuteReader

                If dtrLocal.HasRows = True Then
                    dtrLocal.Read()
                    Dim intIdVol As Integer = dtrLocal("IdEntità")

                    'controllo e chiudo se aperto il datareader
                    If Not dtrLocal Is Nothing Then
                        dtrLocal.Close()
                        dtrLocal = Nothing
                    End If

                    strSql = "SELECT isnull(CronologiaStatiAttestato.DataStato,'') AS DataStato, isnull(StatiAttestato.StatoAttestato, 'Indefinito') AS StatoAttestato, 'POSITIVO' AS Esito, '' AS CausaleEsitoNegativo FROM CronologiaStatiAttestato INNER JOIN StatiAttestato ON StatiAttestato.IdStatoAttestato=CronologiaStatiAttestato.IdStato WHERE CronologiaStatiAttestato.IdEntità=" & intIdVol & " ORDER BY CronologiaStatiAttestato.DataStato "

                    Dim CMD As New SqlClient.SqlDataAdapter(strSql, sqlConn)

                    dtsLocal.DataSetName = "EsitoRichiestaStatiAttestato"

                    'dtsLocal.Tables(0).TableName = "ENTI"

                    CMD.Fill(dtsLocal)

                    If dtsLocal.Tables(0).Rows.Count > 0 Then
                        dtsLocal.Tables(0).TableName = "DettaglioStatiAttestato"

                        xmlLocal = dtsLocal.GetXml

                        EseguiRichiestaStatiAttestato = xmlLocal

                        Return EseguiRichiestaStatiAttestato
                    Else
                        'dtT.Columns.Add()
                        'alla riga assegno la riga delle intestazioni appena creata
                        dtR = dtT.NewRow()

                        'carico la prima riga della datatable
                        dtR(0) = ""
                        dtR(1) = ""
                        dtR(2) = "NEGATIVO"
                        dtR(3) = "INDEFINITO"

                        'aggiungo la riga appena caricata alla datatable
                        dtT.Rows.Add(dtR)

                        dtsLocal.Tables.Add(dtT)

                        dtsLocal.DataSetName = "EsitoRichiestaStatiAttestato"

                        dtsLocal.Tables(0).TableName = "DettaglioStatiAttestato"

                        xmlLocal = dtsLocal.GetXml

                        EseguiRichiestaStatiAttestato = xmlLocal

                        Return EseguiRichiestaStatiAttestato
                    End If
                Else
                    'controllo e chiudo se aperto il datareader
                    If Not dtrLocal Is Nothing Then
                        dtrLocal.Close()
                        dtrLocal = Nothing
                    End If
                End If

                'controllo e chiudo se aperto il datareader
                If Not dtrLocal Is Nothing Then
                    dtrLocal.Close()
                    dtrLocal = Nothing
                End If
            Else
                'dtT.Columns.Add()
                'alla riga assegno la riga delle intestazioni appena creata
                dtR = dtT.NewRow()

                'carico la prima riga della datatable
                dtR(0) = ""
                dtR(1) = ""
                dtR(2) = "NEGATIVO"
                dtR(3) = "UTENTE INESISTENTE"

                'aggiungo la riga appena caricata alla datatable
                dtT.Rows.Add(dtR)

                dtsLocal.Tables.Add(dtT)

                dtsLocal.DataSetName = "EsitoRichiestaStatiAttestato"

                dtsLocal.Tables(0).TableName = "DettaglioStatiAttestato"

                xmlLocal = dtsLocal.GetXml

                EseguiRichiestaStatiAttestato = xmlLocal

                Return EseguiRichiestaStatiAttestato
            End If

            sqlConn.Close()

        Catch ex As Exception
            'dtT.Columns.Add()
            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = ""
            dtR(1) = ""
            dtR(2) = "ERRORE GENERICO"

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)

            dtsLocal.Tables.Add(dtT)

            dtsLocal.DataSetName = "EsitoRichiestaStatiAttestato"

            dtsLocal.Tables(0).TableName = "DettaglioStatiAttestato"

            xmlLocal = dtsLocal.GetXml

            EseguiRichiestaStatiAttestato = xmlLocal

            Return EseguiRichiestaStatiAttestato
        End Try

    End Function

    <WebMethod()> _
    Public Function EseguiRichiestaAttestato(ByVal strUserName As String, ByVal strPWD As String) As String
        Dim wrapper As New Simple3Des
        Dim strSql As String
        Dim sqlConn As New SqlClient.SqlConnection
        Dim xmlLocal As String
        Dim dtR As DataRow
        Dim dtT As New DataTable
        Dim dtsLocal As New DataSet
        Dim myCommand As New SqlClient.SqlCommand
        Dim dtrLocal As SqlClient.SqlDataReader
        Dim strTipoUtente As String
        Dim intIdEntita As Integer
        Dim dbg As Char = System.Configuration.ConfigurationManager.AppSettings("debugApp")

        Try
            'strVecchiaPWD = wrapper.EncryptData(strVecchiaPWD)
            'MP ==> strPWD = wrapper.EncryptData(strPWD)
            'MP ==> strUserName = wrapper.EncryptData(strUserName)

            'aggiungo cmq la colonna dell'esito
            dtT.Columns.Add(New DataColumn("Attestato", GetType(String)))
            dtT.Columns.Add(New DataColumn("Esito", GetType(String)))
            dtT.Columns.Add(New DataColumn("CausaleEsitoNegativo", GetType(String)))

            If strUserName = "" Or strPWD = "" Then
                'dtT.Columns.Add()
                'alla riga assegno la riga delle intestazioni appena creata
                dtR = dtT.NewRow()

                'carico la prima riga della datatable
                dtR(0) = ""
                dtR(1) = "NEGATIVO"
                dtR(2) = "TUTTI I CAMPI SONO NECESSARI"

                'aggiungo la riga appena caricata alla datatable
                dtT.Rows.Add(dtR)

                dtsLocal.Tables.Add(dtT)

                dtsLocal.DataSetName = "EsitoRichiestaAttestato"

                dtsLocal.Tables(0).TableName = "DettaglioAttestato"

                xmlLocal = dtsLocal.GetXml

                EseguiRichiestaAttestato = xmlLocal

                Return EseguiRichiestaAttestato
            Else
                If wrapper.DecryptData(strUserName) = "" Or wrapper.DecryptData(strPWD) = "" Then
                    'dtT.Columns.Add()
                    'alla riga assegno la riga delle intestazioni appena creata
                    dtR = dtT.NewRow()

                    'carico la prima riga della datatable
                    dtR(0) = ""
                    dtR(1) = "NEGATIVO"
                    dtR(2) = "TUTTI I CAMPI SONO NECESSARI"

                    'aggiungo la riga appena caricata alla datatable
                    dtT.Rows.Add(dtR)

                    dtsLocal.Tables.Add(dtT)

                    dtsLocal.DataSetName = "EsitoRichiestaAttestato"

                    dtsLocal.Tables(0).TableName = "DettaglioAttestato"

                    xmlLocal = dtsLocal.GetXml

                    EseguiRichiestaAttestato = xmlLocal

                    Return EseguiRichiestaAttestato
                End If
            End If

            sqlConn.ConnectionString = System.Configuration.ConfigurationManager.AppSettings("cnHelios")

            sqlConn.Open()

            strSql = "select Denominazione, Username, Password, Identificativo,IdStatoEnte,getdate() as dataserver, Tipo, FlagForzaturaAccreditamento from VW_Account_Utenti where username='" & wrapper.DecryptData(strUserName) & "' and password='" & CreatePSW(wrapper.DecryptData(strPWD)).Replace("'", "''") & "' and heliosRead=0 "

            'sql command che mi esegue la insert
            myCommand = New SqlClient.SqlCommand
            myCommand.Connection = sqlConn

            'controllo e chiudo se aperto il datareader
            If Not dtrLocal Is Nothing Then
                dtrLocal.Close()
                dtrLocal = Nothing
            End If

            'eseguo la query
            myCommand.CommandText = strSql
            dtrLocal = myCommand.ExecuteReader

            If dtrLocal.HasRows = True Then
                dtrLocal.Read()

                'controllo e chiudo se aperto il datareader
                If Not dtrLocal Is Nothing Then
                    dtrLocal.Close()
                    dtrLocal = Nothing
                End If

                'modificato da Simona Cordella il 18/05/2016 
                'gestione attestato per GG
                'strSql = "select IdStatoAttestato, IdEntità from entità where username='" & wrapper.DecryptData(strUserName) & "' and password='" & CreatePSW(wrapper.DecryptData(strPWD)) & "' and IdStatoAttestato in (2,4) "

                strSql = " Select e.IdStatoAttestato, e.IdEntità,a.IdTipoProgetto " & _
                         " from entità e " & _
                         " inner join attività a on e.TMPCodiceProgetto= a.CodiceEnte " & _
                         " where username='" & wrapper.DecryptData(strUserName) & "' and password='" & CreatePSW(wrapper.DecryptData(strPWD)).Replace("'", "''") & "' and IdStatoAttestato in (2,4) "

                'sql command che mi esegue la insert
                myCommand = New SqlClient.SqlCommand
                myCommand.Connection = sqlConn

                'controllo e chiudo se aperto il datareader
                If Not dtrLocal Is Nothing Then
                    dtrLocal.Close()
                    dtrLocal = Nothing
                End If

                'eseguo la query
                myCommand.CommandText = strSql
                dtrLocal = myCommand.ExecuteReader

                If dtrLocal.HasRows = True Then
                    dtrLocal.Read()
                    Dim sDati As String
                    Dim strEsito As String
                    Dim intStatoAttestato As Integer = dtrLocal("IdStatoAttestato")
                    Dim intIdVol As Integer = dtrLocal("IdEntità")
                    Dim intIdTipoProgetto As Integer = dtrLocal("IdTipoProgetto")
                    Dim strUserNameVol As Integer

                    'controllo e chiudo se aperto il datareader
                    If Not dtrLocal Is Nothing Then
                        dtrLocal.Close()
                        dtrLocal = Nothing
                    End If

                    'controllo lo stato del volontario per preparare i due update, uno in entità e uno in cronologia
                    Select Case intStatoAttestato
                        Case 2
                            myCommand.CommandText = "UPDATE entità SET IdStatoAttestato=4, UsernameAttestato='" & wrapper.DecryptData(strUserName) & "', DataAttestato=GetDate(), DataOraUltimaModifica=GetDate()  WHERE IdEntità=" & intIdVol
                            myCommand.ExecuteNonQuery()
                        Case 4
                            myCommand.CommandText = "UPDATE entità SET UsernameAttestato='" & wrapper.DecryptData(strUserName) & "', DataAttestato=GetDate(), DataOraUltimaModifica=GetDate()  WHERE IdEntità=" & intIdVol
                            myCommand.ExecuteNonQuery()
                    End Select

                    'inserisco la cronologia
                    myCommand.CommandText = "Insert Into CronologiaEntitàWebDoc (IdEntità, Username, DataCronologia, TipoDocumento)  VALUES (" & intIdVol & ", '" & wrapper.DecryptData(strUserName) & "', GetDate(),'ATTESTATO')"
                    myCommand.ExecuteNonQuery()

                    'inserisco la cronologia
                    myCommand.CommandText = "Insert Into CronologiaStatiAttestato (IDEntità,IDStato,DataStato,UserNameStato)  VALUES (" & intIdVol & ", 4, GetDate(),'" & wrapper.DecryptData(strUserName) & "')"
                    myCommand.ExecuteNonQuery()

                    sDati = "IdVolontario," & intIdVol & ":"

                    Try
                        If dbg = "s" Then
                            Dim myLog As New GeneraLog("strEsito PRIMA della chiamata a CreatePdf:*" + strEsito + "*", "EseguiRichiestaAttestato")
                            myLog.GeneraFileLog()
                        End If
                        If intIdTipoProgetto <> 4 Then
                            strEsito = CreatePdf("crpVolontariAttestato.rpt", sDati, wrapper.DecryptData(strUserName))
                        Else
                            'richiamo l'attestato per i volontari di GG
                            strEsito = CreatePdf("crpVolontariAttestatoGG.rpt", sDati, wrapper.DecryptData(strUserName))
                        End If


                        If dbg = "s" Then
                            Dim myLog As New GeneraLog("strEsito DOPO la chiamata a CreatePdf:*" + strEsito + "*", "EseguiRichiestaAttestato")
                            myLog.GeneraFileLog()
                        End If

                    Catch ex As Exception
                        'dtT.Columns.Add()
                        'alla riga assegno la riga delle intestazioni appena creata
                        dtR = dtT.NewRow()

                        'converto il file in base64
                        Dim testoBase64 As String

                        If dbg = "s" Then
                            Dim myLog As New GeneraLog("Path on Catch: " + Server.MapPath(strEsito), "EseguiRichiestaAttestato")
                            myLog.GeneraFileLog()
                        End If
                        testoBase64 = FileToBase64(Server.MapPath(strEsito))

                        'carico la prima riga della datatable
                        dtR(0) = testoBase64
                        dtR(1) = "NEGATIVO"
                        dtR(2) = ex.Message

                        'aggiungo la riga appena caricata alla datatable
                        dtT.Rows.Add(dtR)

                        dtsLocal.Tables.Add(dtT)

                        dtsLocal.DataSetName = "EsitoRichiestaAttestato"

                        dtsLocal.Tables(0).TableName = "DettaglioAttestato"

                        xmlLocal = dtsLocal.GetXml

                        EseguiRichiestaAttestato = xmlLocal

                        Return EseguiRichiestaAttestato
                    End Try

                    If strEsito = "ERRORE GENERICO. CONTATTARE L'ASSISTENZA" Then
                        'dtT.Columns.Add()
                        'alla riga assegno la riga delle intestazioni appena creata
                        dtR = dtT.NewRow()

                        'carico la prima riga della datatable
                        dtR(0) = ""
                        dtR(1) = "NEGATIVO"
                        dtR(2) = strEsito

                        'aggiungo la riga appena caricata alla datatable
                        dtT.Rows.Add(dtR)

                        dtsLocal.Tables.Add(dtT)

                        dtsLocal.DataSetName = "EsitoRichiestaAttestato"

                        dtsLocal.Tables(0).TableName = "DettaglioAttestato"

                        xmlLocal = dtsLocal.GetXml

                        EseguiRichiestaAttestato = xmlLocal

                        Return EseguiRichiestaAttestato
                    Else
                        'dtT.Columns.Add()
                        'alla riga assegno la riga delle intestazioni appena creata
                        dtR = dtT.NewRow()

                        'converto il file in base64
                        Dim testoBase64 As String
                        If dbg = "s" Then
                            Dim myLog As New GeneraLog("Path: " + Server.MapPath(strEsito), "EseguiRichiestaAttestato")
                            myLog.GeneraFileLog()
                        End If
                        testoBase64 = FileToBase64(Server.MapPath(strEsito))

                        'carico la prima riga della datatable
                        dtR(0) = testoBase64
                        dtR(1) = "POSITIVO"
                        dtR(2) = ""

                        'aggiungo la riga appena caricata alla datatable
                        dtT.Rows.Add(dtR)

                        dtsLocal.Tables.Add(dtT)

                        dtsLocal.DataSetName = "EsitoRichiestaAttestato"

                        dtsLocal.Tables(0).TableName = "DettaglioAttestato"

                        xmlLocal = dtsLocal.GetXml

                        EseguiRichiestaAttestato = xmlLocal

                        Return EseguiRichiestaAttestato
                    End If
                Else

                    'controllo e chiudo se aperto il datareader
                    If Not dtrLocal Is Nothing Then
                        dtrLocal.Close()
                        dtrLocal = Nothing
                    End If

                    'dtT.Columns.Add()
                    'alla riga assegno la riga delle intestazioni appena creata
                    dtR = dtT.NewRow()

                    'carico la prima riga della datatable
                    dtR(0) = ""
                    dtR(1) = "NEGATIVO"
                    dtR(2) = "USERNAME O PASSWORD ERRATI"

                    'aggiungo la riga appena caricata alla datatable
                    dtT.Rows.Add(dtR)

                    dtsLocal.Tables.Add(dtT)

                    dtsLocal.DataSetName = "EsitoRichiestaAttestato"

                    dtsLocal.Tables(0).TableName = "DettaglioAttestato"

                    xmlLocal = dtsLocal.GetXml

                    EseguiRichiestaAttestato = xmlLocal

                    Return EseguiRichiestaAttestato
                End If
            Else
                'dtT.Columns.Add()
                'alla riga assegno la riga delle intestazioni appena creata
                dtR = dtT.NewRow()

                'carico la prima riga della datatable
                dtR(0) = ""
                dtR(1) = "NEGATIVO"
                dtR(2) = "TUTTI I CAMPI SONO NECESSARI"

                'aggiungo la riga appena caricata alla datatable
                dtT.Rows.Add(dtR)

                dtsLocal.Tables.Add(dtT)

                dtsLocal.DataSetName = "EsitoRichiestaAttestato"

                dtsLocal.Tables(0).TableName = "DettaglioAttestato"

                xmlLocal = dtsLocal.GetXml

                EseguiRichiestaAttestato = xmlLocal

                Return EseguiRichiestaAttestato
            End If

            sqlConn.Close()

        Catch ex As Exception

            If Not sqlConn Is Nothing Then
                sqlConn.Close()
            End If

            'dtT.Columns.Add()
            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = ""
            dtR(1) = "NEGATIVO"
            'dtR(2) = "ERRORE GENERICO. CONTATTARE L'ASSISTENZA"
            dtR(2) = ex.Message

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)

            dtsLocal.Tables.Add(dtT)

            dtsLocal.DataSetName = "EsitoRichiestaAttestato"

            dtsLocal.Tables(0).TableName = "DettaglioAttestato"

            xmlLocal = dtsLocal.GetXml

            EseguiRichiestaAttestato = xmlLocal

            Return EseguiRichiestaAttestato

        End Try

    End Function

    '<WebMethod()> _
    Public Function EseguiRichiestaStatiAttestatoXSD() As System.Xml.XmlDocument
        Dim strSql As String
        Dim xmlLocal As String
        Dim dtR As DataRow
        Dim dtT As New DataTable
        Dim dtsLocal As New DataSet

        'aggiungo cmq la colonna dell'esito
        dtT.Columns.Add(New DataColumn("DataStato", GetType(String)))
        dtT.Columns.Add(New DataColumn("StatoAttestato", GetType(String)))
        dtT.Columns.Add(New DataColumn("Esito", GetType(String)))
        dtT.Columns.Add(New DataColumn("CausaleEsitoNegativo", GetType(String)))

        'dtT.Columns.Add()
        'alla riga assegno la riga delle intestazioni appena creata
        dtR = dtT.NewRow()
        'controllo se l'autanticazine è andata a buon fine
        Dim intX As Integer

        For intX = 0 To 3
            dtR(intX) = ""
        Next

        'controllo e chiudo se aperto il datareader
        'If Not dtrLocal Is Nothing Then
        '    dtrLocal.Close()
        '    dtrLocal = Nothing
        'End If
        'aggiungo la riga appena caricata alla datatable
        dtT.Rows.Add(dtR)

        dtsLocal.Tables.Add(dtT)

        dtsLocal.DataSetName = "EsitoRichiestaStatiAttestato"

        dtsLocal.Tables(0).TableName = "DettaglioStatiAttestato"

        Dim testXML As System.Xml.XmlNode
        Dim xmlDOC As New System.Xml.XmlDocument

        xmlDOC.InnerXml = dtsLocal.GetXmlSchema

        EseguiRichiestaStatiAttestatoXSD = xmlDOC

        Return EseguiRichiestaStatiAttestatoXSD

    End Function


    '<WebMethod()> _
    Public Function EseguiRichiestaAttestatoXSD() As System.Xml.XmlDocument
        Dim strSql As String
        Dim xmlLocal As String
        Dim dtR As DataRow
        Dim dtT As New DataTable
        Dim dtsLocal As New DataSet

        'aggiungo cmq la colonna dell'esito
        dtT.Columns.Add(New DataColumn("Attestato", GetType(String)))
        dtT.Columns.Add(New DataColumn("Esito", GetType(String)))
        dtT.Columns.Add(New DataColumn("CausaleEsitoNegativo", GetType(String)))

        'dtT.Columns.Add()
        'alla riga assegno la riga delle intestazioni appena creata
        dtR = dtT.NewRow()
        'controllo se l'autanticazine è andata a buon fine
        Dim intX As Integer

        For intX = 0 To 2
            dtR(intX) = ""
        Next

        'controllo e chiudo se aperto il datareader
        'If Not dtrLocal Is Nothing Then
        '    dtrLocal.Close()
        '    dtrLocal = Nothing
        'End If
        'aggiungo la riga appena caricata alla datatable
        dtT.Rows.Add(dtR)

        dtsLocal.Tables.Add(dtT)

        dtsLocal.DataSetName = "EsitoRichiestaAttestato"

        dtsLocal.Tables(0).TableName = "DettaglioAttestato"

        Dim testXML As System.Xml.XmlNode
        Dim xmlDOC As New System.Xml.XmlDocument

        xmlDOC.InnerXml = dtsLocal.GetXmlSchema

        EseguiRichiestaAttestatoXSD = xmlDOC

        Return EseguiRichiestaAttestatoXSD

    End Function

    <WebMethod()> _
    Public Function EseguiRicercaProgetto(ByVal strCodiceEnte As String, ByVal strEnte As String, ByVal strTitolo As String, ByVal strRegione As String, ByVal strProvincia As String, ByVal strComune As String, ByVal strSettore As String, ByVal strArea As String, ByVal strGazzetta As String, ByVal strCompetenzaEnte As String, ByVal strCompetenzaProgetto As String, ByVal strTipoProgetto As String, ByVal strMisure As String, ByVal strDurata As String, ByVal strGiovaniMinoriOpportunita As String, ByVal strEsteroUE As String, ByVal strTutoraggio As String, ByVal strFAMI As String, ByVal strCodiceProgetto As String, ByVal strDigitale As String, ByVal strAmbientale As String) As String
        Dim strSql As String
        Dim sqlConn As New SqlClient.SqlConnection
        Dim dtsLocal As New DataSet
        Dim xmlLocal As String

        Try

            sqlConn.ConnectionString = System.Configuration.ConfigurationManager.AppSettings("cnHelios")

            sqlConn.Open()


            strSql = "SELECT CODICEENTE, ENTE, SITO, CODICEPROGETTO, TITOLOPROGETTO, TIPOPROGETTO, CODICESEDE, INDIRIZZO, REGIONE, PROVINCIA, COMUNE, SETTORE, AREA, NVOL, BANDOBREVE, dbo.FormatoData(DATAFINEVOLONTARI) AS DATAFINEVOLONTARI, IDGAZZETTA, COMPETENZAENTE, COMPETENZAPROGETTO, MISURE, DURATA, NUMEROGIOVANIMINORIOPPORTUNITA, ESTEROUE, TUTORAGGIO, NUMEROFAMI, NDOMANDEPERVENUTE, TIPOGMO, GG, TIPOGG, CODICEPROGRAMMA, TITOLOPROGRAMMA, AMBITOAZIONE, URLSINTESIPROGETTO, ENTEATTUATORE, DIGITALE, AMBIENTALE FROM [dbo].[FN_WS_SCEGLIPROGETTO]('" & Replace(strCodiceEnte, "'", "''") & "', '" & Replace(strEnte, "'", "''") & "', '" & Replace(strTitolo, "'", "''") & "', '" & Replace(strRegione, "'", "''") & "', '" & Replace(strProvincia, "'", "''") & "', '" & Replace(strComune, "'", "''") & "', '" & Replace(strSettore, "'", "''") & "', '" & Replace(strArea, "'", "''") & "','" & strGazzetta & "','" & Replace(strCompetenzaEnte, "'", "''") & "','" & Replace(strCompetenzaProgetto, "'", "''") & "','" & Replace(strTipoProgetto, "'", "''") & "','" & Replace(strMisure, "'", "''") & "','" & Replace(strDurata, "'", "''") & "','" & Replace(strGiovaniMinoriOpportunita, "'", "''") & "','" & Replace(strEsteroUE, "'", "''") & "','" & Replace(strTutoraggio, "'", "''") & "','" & Replace(strFAMI, "'", "''") & "','" & Replace(strCodiceProgetto, "'", "''") & "','" & Replace(strDigitale, "'", "''") & "','" & Replace(strAmbientale, "'", "''") & "') "

            Dim CMD As New SqlClient.SqlDataAdapter(strSql, sqlConn)

            dtsLocal.DataSetName = "DettaglioProgetto"

            'dtsLocal.Tables(0).TableName = "ENTI"

            CMD.Fill(dtsLocal)

            dtsLocal.Tables(0).TableName = "Dettaglio"

            xmlLocal = dtsLocal.GetXml

            EseguiRicercaProgetto = xmlLocal

            sqlConn.Close()

            Return EseguiRicercaProgetto
        Catch ex As Exception
            Dim myLog As New GeneraLog(ex.Message, "EseguiRicercaProgetto")
            myLog.GeneraFileLog()
        End Try
    End Function

    <WebMethod()> _
Public Function EseguiRicercaDettaglioEnte(ByVal strCodiceEnte As String, ByVal strRegione As String, ByVal strProvincia As String, ByVal strComune As String) As String
        Dim strSql As String
        Dim sqlConn As New SqlClient.SqlConnection
        Dim dtsLocal As New DataSet
        Dim xmlLocal As String

        Try

            sqlConn.ConnectionString = System.Configuration.ConfigurationManager.AppSettings("cnHelios")

            sqlConn.Open()

            strSql = "SELECT NSEDI, REGIONE, PROVINCIA, COMUNE FROM [dbo].[FN_WS_DETTAGLIOENTE]('" & Replace(strCodiceEnte, "'", "''") & "','" & Replace(strRegione, "'", "''") & "','" & Replace(strProvincia, "'", "''") & "','" & Replace(strComune, "'", "''") & "') "

            Dim CMD As New SqlClient.SqlDataAdapter(strSql, sqlConn)

            dtsLocal.DataSetName = "DettaglioEnti"

            'dtsLocal.Tables(0).TableName = "ENTI"

            CMD.Fill(dtsLocal)

            dtsLocal.Tables(0).TableName = "Dettaglio"

            xmlLocal = dtsLocal.GetXml

            EseguiRicercaDettaglioEnte = xmlLocal

            sqlConn.Close()

            Return EseguiRicercaDettaglioEnte
        Catch ex As Exception
            Dim myLog As New GeneraLog(ex.Message, "EseguiRicercaDettaglioEnte")
            myLog.GeneraFileLog()
        End Try
    End Function

    <WebMethod()> _
Public Function EseguiRichiestaDatiVolontario(ByVal strUserName As String, ByVal strPWD As String) As String
        Dim wrapper As New Simple3Des
        Dim strSql As String
        Dim sqlConn As New SqlClient.SqlConnection
        Dim xmlLocal As String
        Dim dtR As DataRow
        Dim dtT As New DataTable
        Dim dtsLocal As New DataSet
        Dim myCommand As New SqlClient.SqlCommand
        Dim dtrLocal As SqlClient.SqlDataReader
        Dim strIdEntita, strModalitaPagamento, strIban, strBicSwift, strCodiceLibrettoPostale As String

        'strPWD = wrapper.EncryptData(strPWD)
        'strUserName = wrapper.EncryptData(strUserName)

        Try

            sqlConn.ConnectionString = System.Configuration.ConfigurationManager.AppSettings("cnHelios")

            sqlConn.Open()

            strSql = "select Denominazione, Username, Password, Identificativo,IdStatoEnte,getdate() as dataserver, Tipo, FlagForzaturaAccreditamento from VW_Account_Utenti where username='" & wrapper.DecryptData(strUserName) & "' and password='" & CreatePSW(wrapper.DecryptData(strPWD)).Replace("'", "''") & "' and heliosRead=0 "

            'sql command che mi esegue la insert
            myCommand = New SqlClient.SqlCommand
            myCommand.Connection = sqlConn

            'controllo e chiudo se aperto il datareader
            If Not dtrLocal Is Nothing Then
                dtrLocal.Close()
                dtrLocal = Nothing
            End If

            'eseguo la query
            myCommand.CommandText = strSql
            dtrLocal = myCommand.ExecuteReader

            dtT.Columns.Add(New DataColumn("Esito", GetType(String)))
            dtT.Columns.Add(New DataColumn("CausaleEsitoNegativo", GetType(String)))
            dtT.Columns.Add(New DataColumn("Nome", GetType(String)))
            dtT.Columns.Add(New DataColumn("Cognome", GetType(String)))
            dtT.Columns.Add(New DataColumn("DataNascita", GetType(String)))
            dtT.Columns.Add(New DataColumn("ComuneNascita", GetType(String)))
            dtT.Columns.Add(New DataColumn("ProvinciaNascita", GetType(String)))
            dtT.Columns.Add(New DataColumn("IndirizzoResidenza", GetType(String)))
            dtT.Columns.Add(New DataColumn("IndirizzoDomicilio", GetType(String)))
            'aggiunte il 06/06/2022
            dtT.Columns.Add(New DataColumn("NumeroCivicoDomicilio", GetType(String)))
            dtT.Columns.Add(New DataColumn("CapDomicilio", GetType(String)))
            dtT.Columns.Add(New DataColumn("ProvinciaDomicilio", GetType(String)))
            dtT.Columns.Add(New DataColumn("ComuneDomicilio", GetType(String)))
            'fine aggiunte
            dtT.Columns.Add(New DataColumn("CodiceFiscale", GetType(String)))
            dtT.Columns.Add(New DataColumn("Email", GetType(String)))
            dtT.Columns.Add(New DataColumn("Telefono", GetType(String)))
            dtT.Columns.Add(New DataColumn("DescrizioneModalitaPagamento", GetType(String)))
            dtT.Columns.Add(New DataColumn("CodiceIBAN", GetType(String)))
            dtT.Columns.Add(New DataColumn("CodiceSWIFT", GetType(String)))

            'dtT.Columns.Add()
            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()
            'controllo se l'autanticazine è andata a buon fine
            If dtrLocal.HasRows = True Then

                dtrLocal.Read()

                strIdEntita = IIf(IsDBNull(dtrLocal("identificativo")) = True, "", dtrLocal("identificativo"))

                'controllo e chiudo se aperto il datareader
                If Not dtrLocal Is Nothing Then
                    dtrLocal.Close()
                    dtrLocal = Nothing
                End If

                strSql = "SELECT Nome, Cognome, DataNascita, ComuneNascita, ProvinciaNascita, IndirizzoResidenza, IndirizzoDomicilio, NumeroCivicoDomicilio, CAPDomicilio, ProvinciaDomicilio, ComuneDomicilio, CodiceFiscale, Telefono, Email, CodiceLibrettoPostale, IBAN, BIC_SWIFT FROM VW_WS_DATIVOLONTARIO_V2 WHERE IdEntità='" & strIdEntita & "'"

                'sql command che mi esegue la insert
                myCommand = New SqlClient.SqlCommand
                myCommand.Connection = sqlConn

                'eseguo la query
                myCommand.CommandText = strSql
                dtrLocal = myCommand.ExecuteReader

                If dtrLocal.HasRows = True Then

                    dtrLocal.Read()

                    'carico la prima riga della datatable
                    dtR("Esito") = "POSITIVO"
                    dtR("CausaleEsitoNegativo") = ""
                    dtR("Nome") = IIf(IsDBNull(dtrLocal("Nome")) = True, "", dtrLocal("Nome"))
                    dtR("Cognome") = IIf(IsDBNull(dtrLocal("Cognome")) = True, "", dtrLocal("Cognome"))
                    dtR("DataNascita") = IIf(IsDBNull(dtrLocal("DataNascita")) = True, "", dtrLocal("DataNascita"))
                    dtR("ComuneNascita") = IIf(IsDBNull(dtrLocal("ComuneNascita")) = True, "", dtrLocal("ComuneNascita"))
                    dtR("ProvinciaNascita") = IIf(IsDBNull(dtrLocal("ProvinciaNascita")) = True, "", dtrLocal("ProvinciaNascita"))
                    dtR("IndirizzoResidenza") = IIf(IsDBNull(dtrLocal("IndirizzoResidenza")) = True, "", dtrLocal("IndirizzoResidenza"))
                    dtR("IndirizzoDomicilio") = IIf(IsDBNull(dtrLocal("IndirizzoDomicilio")) = True, "", dtrLocal("IndirizzoDomicilio"))

                    dtR("NumeroCivicoDomicilio") = IIf(IsDBNull(dtrLocal("NumeroCivicoDomicilio")) = True, "", dtrLocal("NumeroCivicoDomicilio"))
                    dtR("CAPDomicilio") = IIf(IsDBNull(dtrLocal("CAPDomicilio")) = True, "", dtrLocal("CAPDomicilio"))
                    dtR("ProvinciaDomicilio") = IIf(IsDBNull(dtrLocal("ProvinciaDomicilio")) = True, "", dtrLocal("ProvinciaDomicilio"))
                    dtR("ComuneDomicilio") = IIf(IsDBNull(dtrLocal("ComuneDomicilio")) = True, "", dtrLocal("ComuneDomicilio"))

                    dtR("CodiceFiscale") = IIf(IsDBNull(dtrLocal("CodiceFiscale")) = True, "", dtrLocal("CodiceFiscale"))
                    dtR("Email") = IIf(IsDBNull(dtrLocal("Email")) = True, "", dtrLocal("Email"))
                    dtR("Telefono") = IIf(IsDBNull(dtrLocal("Telefono")) = True, "", dtrLocal("Telefono"))

                    'esamino il codice Iban e il codice libretto postale per verificare la tipologia di pagamento:
                    '   - se non esistono ne Iban nè CodiceLibrettoPostale ==> "assente"
                    '   - se esiste l'Iban allora ==> "iban"
                    '   - se esiste solo il CodiceLibrettoPostale ==> "libretto postale"
                    strIban = IIf(IsDBNull(dtrLocal("IBAN")) = True, "", dtrLocal("IBAN"))
                    strBicSwift = IIf(IsDBNull(dtrLocal("BIC_SWIFT")) = True, "", dtrLocal("BIC_SWIFT"))
                    strCodiceLibrettoPostale = IIf(IsDBNull(dtrLocal("CodiceLibrettoPostale")) = True, "", dtrLocal("CodiceLibrettoPostale"))

                    If (String.IsNullOrEmpty(strIban)) Then
                        If (String.IsNullOrEmpty(strCodiceLibrettoPostale)) Then
                            strModalitaPagamento = "assente"
                        Else
                            strModalitaPagamento = "libretto"
                        End If
                    Else
                        strModalitaPagamento = "iban"
                    End If

                    Select Case LCase(strModalitaPagamento)
                        Case "iban"
                            dtR("DescrizioneModalitaPagamento") = "Coordinate bancarie"
                            dtR("CodiceIBAN") = strIban 'IIf(String.IsNullOrEmpty(strBicSwift), strIban, strIban & " " & strBicSwift)
                            dtR("CodiceSWIFT") = strBicSwift
                        Case "libretto"
                            dtR("DescrizioneModalitaPagamento") = "Libretto postale"
                            dtR("CodiceIBAN") = IIf(IsDBNull(dtrLocal("CodiceLibrettoPostale")) = True, "", dtrLocal("CodiceLibrettoPostale"))
                            dtR("CodiceSWIFT") = ""
                        Case Else
                            dtR("DescrizioneModalitaPagamento") = ""
                            dtR("CodiceIBAN") = ""
                            dtR("CodiceSWIFT") = ""
                    End Select

                Else
                    'carico la prima riga della datatable
                    dtR("Esito") = "NEGATIVO"
                    dtR("CausaleEsitoNegativo") = "VERIFICARE USERNAME O PASSWORD"
                    dtR("Nome") = ""
                    dtR("Cognome") = ""
                    dtR("DataNascita") = ""
                    dtR("ComuneNascita") = ""
                    dtR("ProvinciaNascita") = ""
                    dtR("IndirizzoResidenza") = ""
                    dtR("IndirizzoDomicilio") = ""
                    dtR("NumeroCivicoDomicilio") = ""
                    dtR("CapDomicilio") = ""
                    dtR("ProvinciaDomicilio") = ""
                    dtR("ComuneDomicilio") = ""
                    dtR("CodiceFiscale") = ""
                    dtR("Email") = ""
                    dtR("Telefono") = ""
                    dtR("DescrizioneModalitaPagamento") = ""
                    dtR("CodiceIBAN") = ""
                    dtR("CodiceSWIFT") = ""
                End If

            Else

                'controllo e chiudo se aperto il datareader
                If Not dtrLocal Is Nothing Then
                    dtrLocal.Close()
                    dtrLocal = Nothing
                End If

                'carico la prima riga della datatable
                dtR("Esito") = "NEGATIVO"
                dtR("CausaleEsitoNegativo") = "VERIFICARE USERNAME O PASSWORD"
                dtR("Nome") = ""
                dtR("Cognome") = ""
                dtR("DataNascita") = ""
                dtR("ComuneNascita") = ""
                dtR("ProvinciaNascita") = ""
                dtR("IndirizzoResidenza") = ""
                dtR("IndirizzoDomicilio") = ""
                dtR("NumeroCivicoDomicilio") = ""
                dtR("CapDomicilio") = ""
                dtR("ProvinciaDomicilio") = ""
                dtR("ComuneDomicilio") = ""
                dtR("CodiceFiscale") = ""
                dtR("Email") = ""
                dtR("Telefono") = ""
                dtR("DescrizioneModalitaPagamento") = ""
                dtR("CodiceIBAN") = ""
                dtR("CodiceSWIFT") = ""
            End If
            'controllo e chiudo se aperto il datareader
            If Not dtrLocal Is Nothing Then
                dtrLocal.Close()
                dtrLocal = Nothing
            End If

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)

            dtsLocal.Tables.Add(dtT)

            dtsLocal.DataSetName = "DatiVolontario"

            dtsLocal.Tables(0).TableName = "Dettagli"

            xmlLocal = dtsLocal.GetXml

            EseguiRichiestaDatiVolontario = xmlLocal

            sqlConn.Close()

            Return EseguiRichiestaDatiVolontario
        Catch ex As Exception
            Dim myLog As New GeneraLog(ex.Message, "EseguiRichiestaDatiVolontario")
            myLog.GeneraFileLog()
        End Try
    End Function

    '<WebMethod()> _
    Public Function EseguiRichiestaDatiVolontarioXSD() As System.Xml.XmlDocument
        Dim strSql As String
        Dim sqlConn As New SqlClient.SqlConnection
        Dim xmlLocal As String
        Dim dtR As DataRow
        Dim dtT As New DataTable
        Dim dtsLocal As New DataSet
        Dim myCommand As New SqlClient.SqlCommand
        Dim dtrLocal As SqlClient.SqlDataReader


        dtT.Columns.Add(New DataColumn("Esito", GetType(String)))
        dtT.Columns.Add(New DataColumn("CausaleEsitoNegativo", GetType(String)))
        dtT.Columns.Add(New DataColumn("Nome", GetType(String)))
        dtT.Columns.Add(New DataColumn("Cognome", GetType(String)))
        dtT.Columns.Add(New DataColumn("DataNascita", GetType(String)))
        dtT.Columns.Add(New DataColumn("ComuneNascita", GetType(String)))
        dtT.Columns.Add(New DataColumn("ProvinciaNascita", GetType(String)))
        dtT.Columns.Add(New DataColumn("IndirizzoResidenza", GetType(String)))
        dtT.Columns.Add(New DataColumn("IndirizzoDomicilio", GetType(String)))
        dtT.Columns.Add(New DataColumn("CodiceFiscale", GetType(String)))
        dtT.Columns.Add(New DataColumn("Email", GetType(String)))
        dtT.Columns.Add(New DataColumn("Telefono", GetType(String)))
        dtT.Columns.Add(New DataColumn("DescrizioneModalitaPagamento", GetType(String)))
        dtT.Columns.Add(New DataColumn("CoordinatePagamento", GetType(String)))

        'dtT.Columns.Add()
        'alla riga assegno la riga delle intestazioni appena creata
        dtR = dtT.NewRow()
        'controllo se l'autanticazine è andata a buon fine
        Dim intX As Integer

        For intX = 0 To 13
            dtR(intX) = ""
        Next

        'controllo e chiudo se aperto il datareader
        'If Not dtrLocal Is Nothing Then
        '    dtrLocal.Close()
        '    dtrLocal = Nothing
        'End If
        'aggiungo la riga appena caricata alla datatable
        dtT.Rows.Add(dtR)

        dtsLocal.Tables.Add(dtT)

        dtsLocal.DataSetName = "DatiVolontario"

        dtsLocal.Tables(0).TableName = "Dettagli"

        Dim testXML As System.Xml.XmlNode
        Dim xmlDOC As New System.Xml.XmlDocument

        xmlDOC.InnerXml = dtsLocal.GetXmlSchema

        EseguiRichiestaDatiVolontarioXSD = xmlDOC

        sqlConn.Close()

        Return EseguiRichiestaDatiVolontarioXSD

    End Function

    <WebMethod()> _
Public Function EseguiRecuperoPassword(ByVal strUserName As String, ByVal strCodiceVol As String, ByVal strEmail As String) As String
        Dim wrapper As New Simple3Des
        Dim strSql As String
        Dim sqlConn As New SqlClient.SqlConnection
        Dim xmlLocal, username, codicevol, email, strIdEntita, resMail As String
        Dim dtR As DataRow
        Dim dtT As New DataTable
        Dim dtsLocal As New DataSet
        Dim myCommand As New SqlClient.SqlCommand
        Dim dtrLocal As SqlClient.SqlDataReader
        Dim intX As Integer


        'strPWD = wrapper.EncryptData(strPWD)
        username = wrapper.DecryptData(strUserName)
        codicevol = wrapper.DecryptData(strCodiceVol)
        email = wrapper.DecryptData(strEmail)

        'carico le intestazioni per il file xml
        dtT.Columns.Add(New DataColumn("Esito", GetType(String)))
        dtT.Columns.Add(New DataColumn("CausaleEsitoNegativo", GetType(String)))
        dtT.Columns.Add(New DataColumn("RispostaWeb", GetType(String)))
        dtT.Columns.Add(New DataColumn("Nominativo", GetType(String)))
        dtT.Columns.Add(New DataColumn("Sesso", GetType(String)))
        dtT.Columns.Add(New DataColumn("Email", GetType(String)))
        dtT.Columns.Add(New DataColumn("Username", GetType(String)))
        dtT.Columns.Add(New DataColumn("Password", GetType(String)))
        dtT.Columns.Add(New DataColumn("DataInizioServizio", GetType(String)))
        dtT.Columns.Add(New DataColumn("DataFineServizio", GetType(String)))
        dtT.Columns.Add(New DataColumn("CodiceVolontario", GetType(String)))

        'alla riga assegno la riga delle intestazioni appena creata
        dtR = dtT.NewRow()

        Try

            sqlConn.ConnectionString = System.Configuration.ConfigurationManager.AppSettings("cnHelios")

            sqlConn.Open()

            strSql = "select b.IDEntità as Identificativo, b.Nome + ' ' + b.Cognome as Nominativo, b.Sesso, isnull(b.Email, '') as Email, b.Username, dbo.readpassword(b.[Password]) as Password, b.DataInizioServizio, b.DataFineServizio, b.CodiceVolontario " & _
                " from Entità b " & _
                " inner join GraduatorieEntità c on b.IDEntità = c.IdEntità " & _
                " inner join AttivitàSediAssegnazione d on c.IdAttivitàSedeAssegnazione = d.IDAttivitàSedeAssegnazione " & _
                " inner join attività e on d.IDAttività = e.IDAttività " & _
                " inner join BandiAttività f on e.IDBandoAttività = f.IdBandoAttività" & _
                " inner join bando g on f.IdBando = g.IDBando " & _
                " where b.Username='" & username & "' and b.CodiceVolontario='" & codicevol & "' and g.DOL = 0 "

            'sql command che mi esegue la insert
            myCommand = New SqlClient.SqlCommand
            myCommand.Connection = sqlConn

            'eseguo la query
            myCommand.CommandText = strSql
            dtrLocal = myCommand.ExecuteReader


            'controllo se l'autenticazione è andata a buon fine
            If dtrLocal.HasRows = True Then

                dtrLocal.Read()

                strIdEntita = IIf(IsDBNull(dtrLocal("Identificativo")) = True, "", dtrLocal("Identificativo"))

                'controllo se esiste la e-mail in helios. Se non esiste registro in helios quella inserita dal volontario,
                ' invio la password a tale e-mail e restituisco il msg corrispondente
                If dtrLocal("Email") = "" Then


                    'Invio e-mail
                    resMail = MandaEmail(email, IIf(IsDBNull(dtrLocal("Nominativo")) = True, "", dtrLocal("Nominativo")), IIf(dtrLocal("Sesso") = 0, "M", "F"), IIf(IsDBNull(dtrLocal("Password")) = True, "", dtrLocal("Password")))

                    If resMail = "" Then
                        'carico la prima riga della datatable
                        dtR(0) = "POSITIVO"
                        dtR(1) = ""
                        dtR(2) = "La mail dichiarata è stata registrata nel Sistema e la password è stata inviata a " & email
                        dtR(3) = IIf(IsDBNull(dtrLocal("Nominativo")) = True, "", dtrLocal("Nominativo"))
                        dtR(4) = IIf(dtrLocal("Sesso") = 0, "M", "F")
                        dtR(5) = IIf(IsDBNull(dtrLocal("Email")) = True, "", dtrLocal("Email"))
                        dtR(6) = IIf(IsDBNull(dtrLocal("Username")) = True, "", dtrLocal("Username"))
                        dtR(7) = IIf(IsDBNull(dtrLocal("Password")) = True, "", dtrLocal("Password"))
                        dtR(8) = IIf(IsDBNull(dtrLocal("DataInizioServizio")) = True, "", dtrLocal("DataInizioServizio"))
                        dtR(9) = IIf(IsDBNull(dtrLocal("DataFineServizio")) = True, "", dtrLocal("DataFineServizio"))
                        dtR(10) = IIf(IsDBNull(dtrLocal("CodiceVolontario")) = True, "", dtrLocal("CodiceVolontario"))

                        'controllo e chiudo se aperto il datareader
                        If Not dtrLocal Is Nothing Then
                            dtrLocal.Close()
                            dtrLocal = Nothing
                        End If

                        'aggiorno la tabella Entità con il valore della email inserita dal volontario
                        myCommand.CommandText = "update Entità set Email = '" & email & "' where IDEntità = " & strIdEntita
                        myCommand.ExecuteNonQuery()
                    Else
                        dtR(0) = "NEGATIVO"
                        dtR(1) = "MAIL NON INVIATA PER CASELLA DI POSTA ELETTRONICA NON VALIDA O PROBLEMI TECNICI"
                        For intX = 2 To 10 : dtR(intX) = "" : Next
                    End If

                ElseIf String.Compare(Trim(email), Trim(dtrLocal("Email")), True) = 0 Then
                    'la e-mail inserita dal volontario coincide con quella registrata in helios.
                    ' invio la password al volontario e restituisco il msg corrispondente

                    'Invio e-mail
                    resMail = MandaEmail(email, IIf(IsDBNull(dtrLocal("Nominativo")) = True, "", dtrLocal("Nominativo")), IIf(dtrLocal("Sesso") = 0, "M", "F"), IIf(IsDBNull(dtrLocal("Password")) = True, "", dtrLocal("Password")))

                    If resMail = "" Then
                        'carico la prima riga della datatable
                        dtR(0) = "POSITIVO"
                        dtR(1) = ""
                        dtR(2) = "La password è stata inviata all'indirizzo di posta elettronica registrato nel Sistema: " & email
                        dtR(3) = IIf(IsDBNull(dtrLocal("Nominativo")) = True, "", dtrLocal("Nominativo"))
                        dtR(4) = IIf(dtrLocal("Sesso") = 0, "M", "F")
                        dtR(5) = IIf(IsDBNull(dtrLocal("Email")) = True, "", dtrLocal("Email"))
                        dtR(6) = IIf(IsDBNull(dtrLocal("Username")) = True, "", dtrLocal("Username"))
                        dtR(7) = IIf(IsDBNull(dtrLocal("Password")) = True, "", dtrLocal("Password"))
                        dtR(8) = IIf(IsDBNull(dtrLocal("DataInizioServizio")) = True, "", dtrLocal("DataInizioServizio"))
                        dtR(9) = IIf(IsDBNull(dtrLocal("DataFineServizio")) = True, "", dtrLocal("DataFineServizio"))
                        dtR(10) = IIf(IsDBNull(dtrLocal("CodiceVolontario")) = True, "", dtrLocal("CodiceVolontario"))
                    Else
                        dtR(0) = "NEGATIVO"
                        dtR(1) = "MAIL NON INVIATA PER CASELLA DI POSTA ELETTRONICA NON VALIDA O PROBLEMI TECNICI"
                        For intX = 2 To 10 : dtR(intX) = "" : Next
                    End If

                Else    'la mail inserita dal volontario non coincide con quella registrata in helios

                    'carico la prima riga della datatable
                    dtR(0) = "NEGATIVO"
                    dtR(1) = "INDIRIZZO DI POSTA ELETTRONICA NON CORRISPONDENTE A QUELLO REGISTRATO NEL SISTEMA. PER OTTENERE LA PASSWORD INVIARE UNA EMAIL A urp@serviziocivile.it INDICANDO IL CODICE UTENTE E IL CODICE VOLONTARIO"
                    For intX = 2 To 10 : dtR(intX) = "" : Next
                End If


            Else
                'carico la prima riga della datatable
                dtR(0) = "NEGATIVO"
                dtR(1) = "VERIFICARE USERNAME O CODICE VOLONTARIO"
                For intX = 2 To 10 : dtR(intX) = "" : Next
            End If



        Catch ex As Exception
            Dim myLog As New GeneraLog(ex.Message, "EseguiRecuperoPassword")
            myLog.GeneraFileLog()

            If dtR Is Nothing Then
                dtR = dtT.NewRow()
            End If

            dtR(0) = "NEGATIVO"
            dtR(1) = "ERRORE: " & ex.Message
            For intX = 2 To 10 : dtR(intX) = "" : Next

        Finally
            'controllo e chiudo se aperto il datareader
            If Not dtrLocal Is Nothing Then
                dtrLocal.Close()
                dtrLocal = Nothing
            End If

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)

            dtsLocal.Tables.Add(dtT)

            dtsLocal.DataSetName = "DatiVolontario"

            dtsLocal.Tables(0).TableName = "Dettagli"

            xmlLocal = dtsLocal.GetXml

            EseguiRecuperoPassword = xmlLocal

            If sqlConn.State = ConnectionState.Open Then
                sqlConn.Close()
            End If
        End Try

        Return EseguiRecuperoPassword

    End Function

    '<WebMethod()> _
    Public Function EseguiRecuperoPasswordXSD() As System.Xml.XmlDocument
        Dim strSql As String
        Dim sqlConn As New SqlClient.SqlConnection
        Dim xmlLocal As String
        Dim dtR As DataRow
        Dim dtT As New DataTable
        Dim dtsLocal As New DataSet
        Dim myCommand As New SqlClient.SqlCommand
        Dim dtrLocal As SqlClient.SqlDataReader


        dtT.Columns.Add(New DataColumn("Esito", GetType(String)))
        dtT.Columns.Add(New DataColumn("CausaleEsitoNegativo", GetType(String)))
        dtT.Columns.Add(New DataColumn("RispostaWeb", GetType(String)))
        dtT.Columns.Add(New DataColumn("Nominativo", GetType(String)))
        dtT.Columns.Add(New DataColumn("Sesso", GetType(String)))
        dtT.Columns.Add(New DataColumn("Email", GetType(String)))
        dtT.Columns.Add(New DataColumn("Username", GetType(String)))
        dtT.Columns.Add(New DataColumn("Password", GetType(String)))
        dtT.Columns.Add(New DataColumn("DataInizioServizio", GetType(String)))
        dtT.Columns.Add(New DataColumn("DataFineServizio", GetType(String)))
        dtT.Columns.Add(New DataColumn("CodiceVolontario", GetType(String)))

        dtR = dtT.NewRow()

        Dim intX As Integer
        For intX = 0 To 10
            dtR(intX) = ""
        Next

        'aggiungo la riga appena caricata alla datatable
        dtT.Rows.Add(dtR)

        dtsLocal.Tables.Add(dtT)

        dtsLocal.DataSetName = "DatiVolontario"

        dtsLocal.Tables(0).TableName = "Dettagli"

        Dim testXML As System.Xml.XmlNode
        Dim xmlDOC As New System.Xml.XmlDocument

        xmlDOC.InnerXml = dtsLocal.GetXmlSchema

        EseguiRecuperoPasswordXSD = xmlDOC

        sqlConn.Close()

        Return EseguiRecuperoPasswordXSD

    End Function

    <WebMethod()> _
    Public Function EseguiRichiestaModificaDatiVolontario(ByVal strUserName As String, ByVal strPWD As String, ByVal strNuovaEmail As String, ByVal strNuovoTelefono As String, ByVal strNuovoIBAN As String, ByVal strNuovoSWIFT As String, ByVal strNuovaProvinciaDomicilio As String, ByVal strNuovoComuneDomicilio As String, ByVal strNuovoIndirizzoDomicilio As String, ByVal strNuovoCivicoDomicilio As String, ByVal strNuovoCAPDomicilio As String) As String
        Dim wrapper As New Simple3Des
        Dim strSql, strSqlElez As String
        Dim sqlConn As New SqlClient.SqlConnection
        Dim sqlConnElez As New SqlClient.SqlConnection
        Dim xmlLocal As String
        Dim dtR As DataRow
        Dim dtT As New DataTable
        Dim dtsLocal As New DataSet
        Dim myCommand As New SqlClient.SqlCommand
        Dim myCommandElez As New SqlClient.SqlCommand
        Dim dtrLocal As SqlClient.SqlDataReader
        Dim strIdEntita As String
        Dim strMessaggio As String
        Dim strMessaggioIbanSwift As String
        Dim blnVerificaIBAN As Boolean = True
        Dim blnVerificaStore As Boolean = True


        'strUserName = wrapper.EncryptData(strUserName)
        'strPWD = wrapper.EncryptData(strPWD)



        sqlConn.ConnectionString = System.Configuration.ConfigurationManager.AppSettings("cnHelios")

        sqlConn.Open()

        strSql = "select Denominazione, Username, Password, Identificativo,IdStatoEnte,getdate() as dataserver, Tipo, FlagForzaturaAccreditamento from VW_Account_Utenti where username='" & wrapper.DecryptData(strUserName) & "' and password='" & CreatePSW(wrapper.DecryptData(strPWD)).Replace("'", "''") & "' and heliosRead=0 "

        'sql command che mi esegue la insert
        myCommand = New SqlClient.SqlCommand
        myCommand.Connection = sqlConn

        'controllo e chiudo se aperto il datareader
        If Not dtrLocal Is Nothing Then
            dtrLocal.Close()
            dtrLocal = Nothing
        End If

        'eseguo la query
        myCommand.CommandText = strSql
        dtrLocal = myCommand.ExecuteReader

        dtT.Columns.Add(New DataColumn("Esito", GetType(String)))
        dtT.Columns.Add(New DataColumn("CausaleEsitoNegativo", GetType(String)))

        'dtT.Columns.Add()
        'alla riga assegno la riga delle intestazioni appena creata
        dtR = dtT.NewRow()
        'controllo se l'autanticazine è andata a buon fine
        If dtrLocal.HasRows = True Then

            dtrLocal.Read()

            strIdEntita = IIf(IsDBNull(dtrLocal("identificativo")) = True, "", dtrLocal("identificativo"))

            'controllo e chiudo se aperto il datareader
            If Not dtrLocal Is Nothing Then
                dtrLocal.Close()
                dtrLocal = Nothing
            End If

            Try

                'Dim strStringaPattern As String = "^.+\\@(\\[?)[a-zA-Z0-9\\-\\.]+\\.([a-zA-Z]{2,4}|[0-9]{1,3})(\\]?)$)"
                Dim strStringaPattern As String = "^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$"

                Dim FormatEmail As System.Text.RegularExpressions.Regex = New System.Text.RegularExpressions.Regex(strStringaPattern)

                'CONTROLLI
                blnVerificaIBAN = ControllaIBANSWIFT(strNuovoIBAN, strNuovoSWIFT, strMessaggioIbanSwift)
                blnVerificaStore = EseguiStoreSP_WS_AGGIORNAVOLONTARIO_V2(strIdEntita, strNuovaEmail, strNuovoTelefono, strNuovoIBAN, strNuovoSWIFT, strNuovaProvinciaDomicilio, strNuovoComuneDomicilio, strNuovoIndirizzoDomicilio, strNuovoCivicoDomicilio, strNuovoCAPDomicilio, wrapper.DecryptData(strUserName), "NO", "SP_WS_AGGIORNAVOLONTARIO_V2", sqlConn, strMessaggio)
                If strNuovaEmail <> "" AndAlso FormatEmail.IsMatch(strNuovaEmail) = False Then
                    strMessaggioIbanSwift = strMessaggioIbanSwift & "- Il formato dell'indirizzo email non è valido."
                    blnVerificaIBAN = False
                End If

                If blnVerificaIBAN And blnVerificaStore Then

                    EseguiStoreSP_WS_AGGIORNAVOLONTARIO_V2(strIdEntita, strNuovaEmail, strNuovoTelefono, strNuovoIBAN, strNuovoSWIFT, strNuovaProvinciaDomicilio, strNuovoComuneDomicilio, strNuovoIndirizzoDomicilio, strNuovoCivicoDomicilio, strNuovoCAPDomicilio, wrapper.DecryptData(strUserName), "SI", "SP_WS_AGGIORNAVOLONTARIO_V2", sqlConn, strMessaggio)

                    'aggiorno e-mail e telefono anche sul DB Elezioni
                    sqlConnElez.ConnectionString = System.Configuration.ConfigurationManager.AppSettings("cnElezioni")
                    sqlConnElez.Open()

                    myCommandElez = New SqlClient.SqlCommand
                    myCommandElez.Connection = sqlConnElez

                    strSqlElez = "UPDATE VolontariPerElezione set email='" & strNuovaEmail & "', TelefonoAbit='" & strNuovoTelefono & "' WHERE Username='" & wrapper.DecryptData(strUserName) & "' AND CodiceEvento in (select CodiceEvento FROM evsc_eventi WHERE year(DataFineEvento)=year(getdate()))"

                    myCommandElez.CommandText = strSqlElez
                    myCommandElez.ExecuteNonQuery()

                    If sqlConnElez.State = ConnectionState.Open Then
                        sqlConnElez.Close()
                    End If

                    'carico la prima riga della datatable
                    dtR(0) = "POSITIVO"
                    dtR(1) = ""

                Else
                    'carico la prima riga della datatable
                    dtR(0) = "NEGATIVO"
                    dtR(1) = strMessaggio & strMessaggioIbanSwift
                End If



            Catch ex As Exception

                'controllo e chiudo se aperto il datareader
                If Not dtrLocal Is Nothing Then
                    dtrLocal.Close()
                    dtrLocal = Nothing
                End If

                sqlConn.Close()

                'carico la prima riga della datatable
                dtR(0) = "NEGATIVO"
                dtR(1) = "ERRORE GENERICO"

                Dim myLog As New GeneraLog(ex.Message, "EseguiRichiestaModificaDatiVolontario")
                myLog.GeneraFileLog()

            End Try

        Else

            'controllo e chiudo se aperto il datareader
            If Not dtrLocal Is Nothing Then
                dtrLocal.Close()
                dtrLocal = Nothing
            End If

            'carico la prima riga della datatable
            dtR(0) = "NEGATIVO"
            dtR(1) = "VERIFICARE USERNAME O PASSWORD"

        End If
        'controllo e chiudo se aperto il datareader
        If Not dtrLocal Is Nothing Then
            dtrLocal.Close()
            dtrLocal = Nothing
        End If

        'aggiungo la riga appena caricata alla datatable
        dtT.Rows.Add(dtR)

        dtsLocal.Tables.Add(dtT)

        dtsLocal.DataSetName = "DatiVolontario"

        dtsLocal.Tables(0).TableName = "Dettagli"

        xmlLocal = dtsLocal.GetXml

        EseguiRichiestaModificaDatiVolontario = xmlLocal

        sqlConn.Close()

        Return EseguiRichiestaModificaDatiVolontario

    End Function

    Private Function ControllaIBANSWIFT(ByVal strNuovoIBAN As String, ByVal strNuovoSWIFT As String, ByRef strMessaggioIbanSwift As String) As Boolean
        Dim blnVerificaIBAN As Boolean = True
        Dim clsIban As New CheckBancari

        strMessaggioIbanSwift = ""
        'CONTROLLI SU IBAN
        Dim regex As Regex
        regex = New Regex("^[a-zA-Z0-9]*$")

        If regex.Match(strNuovoIBAN).Success = False Then
            strMessaggioIbanSwift = strMessaggioIbanSwift & "- Nel campo Iban possono essere indicati solo lettere e numeri."
            blnVerificaIBAN = False
        End If
        If UCase(Left(Trim(strNuovoIBAN), 2)) = "IT" Then
            If Len(Trim(strNuovoIBAN)) > 27 Then
                strMessaggioIbanSwift = strMessaggioIbanSwift & "- La lunghezza del campo Iban è di 27 caratteri."
                blnVerificaIBAN = False
            Else
                If Len(Trim(strNuovoIBAN)) < 27 Then
                    strMessaggioIbanSwift = strMessaggioIbanSwift & "- La lunghezza del codice IBAN è errata."
                    blnVerificaIBAN = False
                Else
                    If clsIban.VerificaLetteraCin(Mid(Trim(strNuovoIBAN), 5, 1)) = "1" Then
                        strMessaggioIbanSwift = strMessaggioIbanSwift & "- Codice Iban errato."
                        blnVerificaIBAN = False
                    End If
                    'Funzione che controlla l'autenticità del codice iban indicato
                    Dim ChkCalcolaIban As String = clsIban.CalcolaIBAN(Left(Trim(strNuovoIBAN), 2), Mid(Trim(strNuovoIBAN), 5))
                    If UCase(ChkCalcolaIban) <> UCase(Trim(strNuovoIBAN)) Then
                        strMessaggioIbanSwift = strMessaggioIbanSwift & "- Codice Iban errato."
                        blnVerificaIBAN = False
                    Else
                        'controllo abi e cab non devono corrispondere a queste caratteristiche 
                        'ABI   = 07601 
                        'CAB = 3384
                        If AbiCab(UCase(Trim(strNuovoIBAN))) Then
                            strMessaggioIbanSwift = strMessaggioIbanSwift & "- Il codice iban indicato non fa riferimento ad un conto corrente bancario."
                            blnVerificaIBAN = False
                        End If
                    End If
                End If
            End If
        Else
            If Len(Trim(strNuovoIBAN)) > 31 Then
                strMessaggioIbanSwift = strMessaggioIbanSwift & "- La lunghezza del campo Iban per l'estero non può superare i 31 caratteri."
                blnVerificaIBAN = False
            End If
        End If

        'FINE CONTROLLI IBAN
        'CONTROLLI SU CODICE BIC/SWIFT
        If regex.Match(strNuovoSWIFT).Success = False Then
            strMessaggioIbanSwift = strMessaggioIbanSwift & "- Nel campo BIC/SWIFT possono essere indicati solo lettere e numeri."
            blnVerificaIBAN = False
        End If
        If UCase(Left(Trim(strNuovoIBAN), 2)) = "IT" Then
            If Trim(strNuovoSWIFT) <> vbNullString Then
                'lunghezza bicswift 8 - 11
                If (Len(strNuovoSWIFT) <> 8) Then
                    If (Len(strNuovoSWIFT) <> 11) Then
                        strMessaggioIbanSwift = strMessaggioIbanSwift & "- La lunghezza del codice  BIC/SWIFT è errata."
                        blnVerificaIBAN = False
                    End If
                End If
            End If
        Else
            If Trim(strNuovoIBAN) <> vbNullString And Trim(strNuovoSWIFT) = vbNullString Then
                strMessaggioIbanSwift = strMessaggioIbanSwift & "- Il codice  BIC/SWIFT è obbligatorio per il Conto Estero."
                blnVerificaIBAN = False
            End If
            If Len(strNuovoSWIFT) > 20 Then
                strMessaggioIbanSwift = strMessaggioIbanSwift & "- Il campo BIC/SWIFT puo' contenere massimo 20 caratteri."
                blnVerificaIBAN = False
            End If
        End If

        Return blnVerificaIBAN
        'FINE CONTROLLI SU CODICE BIC/SWIFT
    End Function
    Private Function AbiCab(ByVal strIban As String) As Boolean
        'aggiunto da Simona Cordella il 05/05/2011 
        'messaggio di errore se l'iba ha abi= 07601 e cab =03384
        'la function viene utilizzata nella maschera wfrmUpLoadIbanUNSC e WfrmVolontari
        Dim appoABICAB As String
        appoABICAB = Mid(strIban, 6, 10)

        If appoABICAB = "0760103384" Then
            Return True 'errore
        Else
            Return False
        End If
    End Function

    '<WebMethod()> _
    Public Function EseguiRichiestaModificaDatiVolontarioXSD() As System.Xml.XmlDocument
        Dim strSql As String
        Dim sqlConn As New SqlClient.SqlConnection
        Dim xmlLocal As String
        Dim dtR As DataRow
        Dim dtT As New DataTable
        Dim dtsLocal As New DataSet
        Dim myCommand As New SqlClient.SqlCommand
        Dim dtrLocal As SqlClient.SqlDataReader

        dtT.Columns.Add(New DataColumn("Esito", GetType(String)))
        dtT.Columns.Add(New DataColumn("CausaleEsitoNegativo", GetType(String)))

        'alla riga assegno la riga delle intestazioni appena creata
        dtR = dtT.NewRow()
        'controllo se l'autanticazine è andata a buon fine
        Dim intX As Integer

        For intX = 0 To 1
            dtR(intX) = ""
        Next

        'aggiungo la riga appena caricata alla datatable
        dtT.Rows.Add(dtR)

        dtsLocal.Tables.Add(dtT)

        dtsLocal.DataSetName = "DatiVolontario"

        dtsLocal.Tables(0).TableName = "Dettagli"

        Dim testXML As System.Xml.XmlNode
        'Dim xmlDOC As New System.Xml.XmlDataDocument
        Dim xmlDOC As New System.Xml.XmlDocument

        xmlDOC.InnerXml = dtsLocal.GetXmlSchema

        EseguiRichiestaModificaDatiVolontarioXSD = xmlDOC

        sqlConn.Close()

        Return EseguiRichiestaModificaDatiVolontarioXSD

    End Function

    Function EseguiStoreSP_WS_AGGIORNAVOLONTARIO(ByVal strIdEntita As Integer, ByVal strNuovaMail As String, ByVal strNuovoTelefono As String, ByVal strUserName As String, ByVal StrStoreProcedure As String, ByVal conn As SqlClient.SqlConnection) As Boolean

        Dim MySqlCommand As SqlClient.SqlCommand
        MySqlCommand = New SqlClient.SqlCommand
        MySqlCommand.CommandType = CommandType.StoredProcedure
        MySqlCommand.CommandText = StrStoreProcedure
        MySqlCommand.Connection = conn

        Dim sparamIdEntita As SqlClient.SqlParameter
        sparamIdEntita = New SqlClient.SqlParameter
        sparamIdEntita.ParameterName = "@IdEntita"
        sparamIdEntita.SqlDbType = SqlDbType.Int
        MySqlCommand.Parameters.Add(sparamIdEntita)

        Dim sparamNuovaEmail As SqlClient.SqlParameter
        sparamNuovaEmail = New SqlClient.SqlParameter
        sparamNuovaEmail.ParameterName = "@NuovaEmail"
        sparamNuovaEmail.SqlDbType = SqlDbType.NVarChar
        MySqlCommand.Parameters.Add(sparamNuovaEmail)

        Dim sparamNuovoTelefono As SqlClient.SqlParameter
        sparamNuovoTelefono = New SqlClient.SqlParameter
        sparamNuovoTelefono.ParameterName = "@NuovoTelefono"
        sparamNuovoTelefono.SqlDbType = SqlDbType.NVarChar
        MySqlCommand.Parameters.Add(sparamNuovoTelefono)

        Dim sparamUserName As SqlClient.SqlParameter
        sparamUserName = New SqlClient.SqlParameter
        sparamUserName.ParameterName = "@UserName"
        sparamUserName.SqlDbType = SqlDbType.NVarChar
        MySqlCommand.Parameters.Add(sparamUserName)

        Dim sparamEsito As SqlClient.SqlParameter
        sparamEsito = New SqlClient.SqlParameter
        sparamEsito.ParameterName = "@Esito"
        sparamEsito.SqlDbType = SqlDbType.TinyInt
        sparamEsito.Direction = ParameterDirection.Output
        MySqlCommand.Parameters.Add(sparamEsito)

        Dim Reader As SqlClient.SqlDataReader

        MySqlCommand.Parameters("@IdEntita").Value = strIdEntita
        MySqlCommand.Parameters("@NuovaEmail").Value = strNuovaMail
        MySqlCommand.Parameters("@NuovoTelefono").Value = strNuovoTelefono
        MySqlCommand.Parameters("@UserName").Value = strUserName

        Reader = MySqlCommand.ExecuteReader

        EseguiStoreSP_WS_AGGIORNAVOLONTARIO = MySqlCommand.Parameters("@Esito").Value
        If Not Reader Is Nothing Then
            Reader.Close()
            Reader = Nothing
        End If
    End Function

    Function EseguiStoreSP_WS_AGGIORNAVOLONTARIO_V2(ByVal strIdEntita As Integer, ByVal strNuovaMail As String, ByVal strNuovoTelefono As String, ByVal strNuovoIBAN As String, ByVal strNuovoSWIFT As String, ByVal strNuovaProvinciaDomicilio As String, ByVal strNuovoComuneDomicilio As String, ByVal strNuovoIndirizzoDomicilio As String, ByVal strNuovoCivicoDomicilio As String, ByVal strNuovoCAPDomicilio As String, ByVal strUserName As String, ByVal strEsegui As String, ByVal StrStoreProcedure As String, ByVal conn As SqlClient.SqlConnection, ByRef strMessaggio As String) As Boolean

        Dim MySqlCommand As SqlClient.SqlCommand
        MySqlCommand = New SqlClient.SqlCommand
        MySqlCommand.CommandType = CommandType.StoredProcedure
        MySqlCommand.CommandText = StrStoreProcedure
        MySqlCommand.Connection = conn

        Dim sparamIdEntita As SqlClient.SqlParameter
        sparamIdEntita = New SqlClient.SqlParameter
        sparamIdEntita.ParameterName = "@IdEntita"
        sparamIdEntita.SqlDbType = SqlDbType.Int
        MySqlCommand.Parameters.Add(sparamIdEntita)

        Dim sparamNuovaEmail As SqlClient.SqlParameter
        sparamNuovaEmail = New SqlClient.SqlParameter
        sparamNuovaEmail.ParameterName = "@NuovaEmail"
        sparamNuovaEmail.SqlDbType = SqlDbType.NVarChar
        MySqlCommand.Parameters.Add(sparamNuovaEmail)

        Dim sparamNuovoTelefono As SqlClient.SqlParameter
        sparamNuovoTelefono = New SqlClient.SqlParameter
        sparamNuovoTelefono.ParameterName = "@NuovoTelefono"
        sparamNuovoTelefono.SqlDbType = SqlDbType.NVarChar
        MySqlCommand.Parameters.Add(sparamNuovoTelefono)

        Dim sparamNuovoIBAN As SqlClient.SqlParameter
        sparamNuovoIBAN = New SqlClient.SqlParameter
        sparamNuovoIBAN.ParameterName = "@NuovoIBAN"
        sparamNuovoIBAN.SqlDbType = SqlDbType.NVarChar
        MySqlCommand.Parameters.Add(sparamNuovoIBAN)

        Dim sparamNuovoSWIFT As SqlClient.SqlParameter
        sparamNuovoSWIFT = New SqlClient.SqlParameter
        sparamNuovoSWIFT.ParameterName = "@NuovoSWIFT"
        sparamNuovoSWIFT.SqlDbType = SqlDbType.NVarChar
        MySqlCommand.Parameters.Add(sparamNuovoSWIFT)

        Dim sparamNuovaProvinciaDomicilio As SqlClient.SqlParameter
        sparamNuovaProvinciaDomicilio = New SqlClient.SqlParameter
        sparamNuovaProvinciaDomicilio.ParameterName = "@NuovaProvinciaDomicilio"
        sparamNuovaProvinciaDomicilio.SqlDbType = SqlDbType.NVarChar
        MySqlCommand.Parameters.Add(sparamNuovaProvinciaDomicilio)

        Dim sparamNuovoComuneDomicilio As SqlClient.SqlParameter
        sparamNuovoComuneDomicilio = New SqlClient.SqlParameter
        sparamNuovoComuneDomicilio.ParameterName = "@NuovoComuneDomicilio"
        sparamNuovoComuneDomicilio.SqlDbType = SqlDbType.NVarChar
        MySqlCommand.Parameters.Add(sparamNuovoComuneDomicilio)

        Dim sparamNuovoIndirizzoDomicilio As SqlClient.SqlParameter
        sparamNuovoIndirizzoDomicilio = New SqlClient.SqlParameter
        sparamNuovoIndirizzoDomicilio.ParameterName = "@NuovoIndirizzoDomicilio"
        sparamNuovoIndirizzoDomicilio.SqlDbType = SqlDbType.NVarChar
        MySqlCommand.Parameters.Add(sparamNuovoIndirizzoDomicilio)

        Dim sparamNuovoCivicoDomicilio As SqlClient.SqlParameter
        sparamNuovoCivicoDomicilio = New SqlClient.SqlParameter
        sparamNuovoCivicoDomicilio.ParameterName = "@NuovoCivicoDomicilio"
        sparamNuovoCivicoDomicilio.SqlDbType = SqlDbType.NVarChar
        MySqlCommand.Parameters.Add(sparamNuovoCivicoDomicilio)

        Dim sparamNuovoCAPDomicilio As SqlClient.SqlParameter
        sparamNuovoCAPDomicilio = New SqlClient.SqlParameter
        sparamNuovoCAPDomicilio.ParameterName = "@NuovoCAPDomicilio"
        sparamNuovoCAPDomicilio.SqlDbType = SqlDbType.NVarChar
        MySqlCommand.Parameters.Add(sparamNuovoCAPDomicilio)

        Dim sparamUserName As SqlClient.SqlParameter
        sparamUserName = New SqlClient.SqlParameter
        sparamUserName.ParameterName = "@UserName"
        sparamUserName.SqlDbType = SqlDbType.NVarChar
        MySqlCommand.Parameters.Add(sparamUserName)

        Dim sparamEsegui As SqlClient.SqlParameter
        sparamEsegui = New SqlClient.SqlParameter
        sparamEsegui.ParameterName = "@Esegui"
        sparamEsegui.SqlDbType = SqlDbType.NVarChar
        MySqlCommand.Parameters.Add(sparamEsegui)

        Dim sparamEsito As SqlClient.SqlParameter
        sparamEsito = New SqlClient.SqlParameter
        sparamEsito.ParameterName = "@Esito"
        sparamEsito.SqlDbType = SqlDbType.TinyInt
        sparamEsito.Direction = ParameterDirection.Output
        MySqlCommand.Parameters.Add(sparamEsito)

        Dim sparamMessaggio As SqlClient.SqlParameter
        sparamMessaggio = New SqlClient.SqlParameter
        sparamMessaggio.ParameterName = "@Messaggio"
        sparamMessaggio.SqlDbType = SqlDbType.VarChar
        sparamMessaggio.Size = 8000
        sparamMessaggio.Direction = ParameterDirection.Output
        MySqlCommand.Parameters.Add(sparamMessaggio)

        Dim Reader As SqlClient.SqlDataReader

        MySqlCommand.Parameters("@IdEntita").Value = strIdEntita
        MySqlCommand.Parameters("@NuovaEmail").Value = strNuovaMail
        MySqlCommand.Parameters("@NuovoTelefono").Value = strNuovoTelefono
        MySqlCommand.Parameters("@NuovoTelefono").Value = strNuovoTelefono
        MySqlCommand.Parameters("@NuovoIBAN").Value = strNuovoIBAN
        MySqlCommand.Parameters("@NuovoSWIFT").Value = strNuovoSWIFT
        MySqlCommand.Parameters("@NuovaProvinciaDomicilio").Value = strNuovaProvinciaDomicilio
        MySqlCommand.Parameters("@NuovoComuneDomicilio").Value = strNuovoComuneDomicilio
        MySqlCommand.Parameters("@NuovoIndirizzoDomicilio").Value = strNuovoIndirizzoDomicilio
        MySqlCommand.Parameters("@NuovoCivicoDomicilio").Value = strNuovoCivicoDomicilio
        MySqlCommand.Parameters("@NuovoCAPDomicilio").Value = strNuovoCAPDomicilio
        MySqlCommand.Parameters("@UserName").Value = strUserName
        MySqlCommand.Parameters("@Esegui").Value = strEsegui

        Reader = MySqlCommand.ExecuteReader

        EseguiStoreSP_WS_AGGIORNAVOLONTARIO_V2 = MySqlCommand.Parameters("@Esito").Value
        strMessaggio = MySqlCommand.Parameters("@Messaggio").Value

        If Not Reader Is Nothing Then
            Reader.Close()
            Reader = Nothing
        End If
    End Function

    <WebMethod()> _
     Public Function EseguiRicercaPagamenti(ByVal strUserName As String, ByVal strPWD As String) As DataSet
        Dim wrapper As New Simple3Des
        Dim strSql As String
        Dim sqlConn As New SqlClient.SqlConnection
        Dim dtsLocal As New DataSet("DettaglioPagamenti")
        Dim myCommand As New SqlClient.SqlCommand
        Dim dtrLocal As SqlClient.SqlDataReader
        Dim strIdentificativo As String
        Dim BloccaDatiPaghe As String = "NO"

        'strVecchiaPWD = wrapper.EncryptData(strVecchiaPWD)
        'strPWD = wrapper.EncryptData(strPWD)
        'strUserName = wrapper.EncryptData(strUserName)

        Try


            sqlConn.ConnectionString = System.Configuration.ConfigurationManager.AppSettings("cnHelios")

            sqlConn.Open()

            'verifico la validatà dell'utenza e della password
            strSql = "select codicevolontario from entità where username='" & wrapper.DecryptData(strUserName) & "' and password='" & CreatePSW(wrapper.DecryptData(strPWD)).Replace("'", "''") & "'"

            myCommand = New SqlClient.SqlCommand
            myCommand.Connection = sqlConn

            'controllo e chiudo se aperto il datareader
            If Not dtrLocal Is Nothing Then
                dtrLocal.Close()
                dtrLocal = Nothing
            End If

            'eseguo la query
            myCommand.CommandText = strSql
            dtrLocal = myCommand.ExecuteReader

            'controllo se l'autanticazine è andata a buon fine
            If dtrLocal.HasRows = True Then
                dtrLocal.Read()
                'ricavo il codice volonario dall'utenza e la password di INPUT
                strIdentificativo = dtrLocal("codicevolontario")
                'controllo e chiudo se aperto il datareader
                If Not dtrLocal Is Nothing Then
                    dtrLocal.Close()
                    dtrLocal = Nothing
                End If

                'estraggo i dati dei pagamenti dell'utenza che si è autentificata
                'strSql = "select DPCODVOL CodiceVolontario, DP__ANNO AnnoRiferimento, DP__MESE MeseRiferimento, DPLIBPOS LibrettoPostale, DPIMPTOT Importo, DPDATAVA DataValuta, DPSTATOP CodiceStato, DESCRISR DescrizioneStato from [sqldati2\sqldati205].[newmoneytest].dbo.[vw_interroga_pagamenti] WHERE DPCODVOL = '" & strIdentificativo & "'"
                strSql = "select DPCODVOL CodiceVolontario, DP__ANNO AnnoRiferimento, DP__MESE MeseRiferimento, DPLIBPOS LibrettoPostale, DPIMPTOT Importo, DPDATAVA DataValuta, DPSTATOP CodiceStato, DESCRISR DescrizioneStato from [sqldati].[newmoney].dbo.[vw_interroga_pagamenti] WHERE DPCODVOL = '" & strIdentificativo & "'"
                BloccaDatiPaghe = System.Configuration.ConfigurationManager.AppSettings("BloccaDatiPaghe")
                If BloccaDatiPaghe = "SI" Then
                    strSql = strSql & " AND 1=2"
                End If
                Dim daLocal As New SqlClient.SqlDataAdapter(strSql, sqlConn)

                daLocal.FillSchema(dtsLocal, SchemaType.Source, "DettaglioPagamenti")
                daLocal.Fill(dtsLocal, "DettaglioPagamenti")
            End If

            sqlConn.Close()
            Return dtsLocal

        Catch ex As Exception
            Dim myLog As New GeneraLog(ex.Message, "EseguiRicercaPagamenti")
            myLog.GeneraFileLog()
        End Try

    End Function
    'Public Function EseguiRicercaPagamenti(ByVal strUserName As String, ByVal strPWD As String) As String
    '    Dim wrapper As New Simple3Des
    '    Dim strSql As String
    '    Dim sqlConn As New SqlClient.SqlConnection
    '    Dim xmlLocal As String
    '    Dim dtR As DataRow
    '    Dim dtT As New DataTable
    '    Dim dtsLocal As New DataSet
    '    Dim myCommand As New SqlClient.SqlCommand
    '    Dim dtrLocal As SqlClient.SqlDataReader
    '    Dim strIdentificativo As String

    '    Try

    '        
    '        

    '        sqlConn.Open()

    '        dtT.Columns.Add(New DataColumn("Esito", GetType(String)))
    '        dtT.Columns.Add(New DataColumn("CodiceVolontario", GetType(String)))
    '        dtT.Columns.Add(New DataColumn("AnnoRiferimento", GetType(String)))
    '        dtT.Columns.Add(New DataColumn("MeseRiferimento", GetType(String)))
    '        dtT.Columns.Add(New DataColumn("LibrettoPostale", GetType(String)))
    '        dtT.Columns.Add(New DataColumn("Importo", GetType(Decimal)))
    '        dtT.Columns.Add(New DataColumn("DataValuta", GetType(String)))
    '        dtT.Columns.Add(New DataColumn("CodiceStato", GetType(String)))
    '        dtT.Columns.Add(New DataColumn("DescrizioneStato", GetType(String)))

    '        'alla riga assegno la riga delle intestazioni appena creata
    '        dtR = dtT.NewRow()

    '        'verifico la validatà dell'utenza e della password
    '        strSql = "select codicevolontario from entità where username='" & wrapper.DecryptData(strUserName) & "' and password='" & CreatePSW(wrapper.DecryptData(strPWD)) & "'"

    '        myCommand = New SqlClient.SqlCommand
    '        myCommand.Connection = sqlConn

    '        'controllo e chiudo se aperto il datareader
    '        If Not dtrLocal Is Nothing Then
    '            dtrLocal.Close()
    '            dtrLocal = Nothing
    '        End If

    '        'eseguo la query
    '        myCommand.CommandText = strSql
    '        dtrLocal = myCommand.ExecuteReader

    '        'controllo se l'autanticazine è andata a buon fine
    '        If dtrLocal.HasRows = True Then
    '            dtrLocal.Read()
    '            'ricavo il codice volonario dall'utenza e la password di INPUT
    '            strIdentificativo = dtrLocal("codicevolontario")
    '            'controllo e chiudo se aperto il datareader
    '            If Not dtrLocal Is Nothing Then
    '                dtrLocal.Close()
    '                dtrLocal = Nothing
    '            End If

    '            'estraggo i dati dei pagamenti dell'utenza che si è autentificata
    '            strSql = "select DPCODVOL, DP__ANNO, DP__MESE, DPLIBPOS, DPIMPTOT, DPDATAVA, DPSTATOP, DESCRISR from [sqldati2\sqldati205].[newmoneytest].dbo.[vw_interroga_pagamenti] WHERE DPCODVOL = '" & strIdentificativo & "'"

    '            myCommand = New SqlClient.SqlCommand
    '            myCommand.Connection = sqlConn
    '            myCommand.CommandText = strSql
    '            dtrLocal = myCommand.ExecuteReader

    '            If dtrLocal.HasRows = True Then
    '                While dtrLocal.Read()
    '                    dtR = dtT.NewRow()
    '                    dtR(0) = "POSITIVO"
    '                    dtR(1) = dtrLocal("DPCODVOL").ToString.TrimEnd.TrimStart
    '                    dtR(2) = dtrLocal("DP__ANNO").ToString.TrimEnd.TrimStart
    '                    dtR(3) = dtrLocal("DP__MESE").ToString.TrimEnd.TrimStart
    '                    dtR(4) = dtrLocal("DPLIBPOS").ToString.TrimEnd.TrimStart
    '                    dtR(5) = dtrLocal("DPIMPTOT")
    '                    dtR(6) = FormatDateTime(dtrLocal("DPDATAVA"), DateFormat.ShortDate)
    '                    dtR(7) = dtrLocal("DPSTATOP").ToString.TrimEnd.TrimStart
    '                    dtR(8) = dtrLocal("DESCRISR").ToString.TrimEnd.TrimStart
    '                    'aggiungo la riga appena caricata alla datatable
    '                    dtT.Rows.Add(dtR)
    '                End While
    '            Else
    '                dtR(0) = "NON CI SONO PAGAMENTI"
    '                dtT.Rows.Add(dtR)
    '            End If
    '        Else
    '            'autenticazione non valida
    '            dtR(0) = "NEGATIVO"
    '            dtT.Rows.Add(dtR)
    '        End If

    '        'controllo e chiudo se aperto il datareader
    '        'controllo e chiudo se aperto il datareader
    '        If Not dtrLocal Is Nothing Then
    '            dtrLocal.Close()
    '            dtrLocal = Nothing
    '        End If

    '        dtsLocal.Tables.Add(dtT)

    '        dtsLocal.DataSetName = "EsitoPagamenti"

    '        dtsLocal.Tables(0).TableName = "DettaglioPagamenti"

    '        xmlLocal = dtsLocal.GetXml

    '        EseguiRicercaPagamenti = xmlLocal

    '        sqlConn.Close()

    '        Return EseguiRicercaPagamenti

    '    Catch ex As Exception
    '        Dim myLog As New GeneraLog(ex.Message, "EseguiRicercaPagamenti")
    '        myLog.GeneraFileLog()
    '    End Try

    'End Function

    ' <WebMethod()> _
    Public Function EseguiRicercaPagamentiXSD() As System.Xml.XmlDocument
        Dim strSql As String
        Dim sqlConn As New SqlClient.SqlConnection
        Dim xmlLocal As String
        Dim dtR As DataRow
        Dim dtT As New DataTable
        Dim dtsLocal As New DataSet
        Dim myCommand As New SqlClient.SqlCommand
        Dim dtrLocal As SqlClient.SqlDataReader

        dtT.Columns.Add(New DataColumn("Esito", GetType(String)))
        dtT.Columns.Add(New DataColumn("CodiceVolontario", GetType(String)))
        dtT.Columns.Add(New DataColumn("AnnoRiferimento", GetType(String)))
        dtT.Columns.Add(New DataColumn("MeseRiferimento", GetType(String)))
        dtT.Columns.Add(New DataColumn("LibrettoPostale", GetType(String)))
        dtT.Columns.Add(New DataColumn("Importo", GetType(Decimal)))
        dtT.Columns.Add(New DataColumn("DataValuta", GetType(String)))
        dtT.Columns.Add(New DataColumn("CodiceStato", GetType(String)))
        dtT.Columns.Add(New DataColumn("DescrizioneStato", GetType(String)))

        'alla riga assegno la riga delle intestazioni appena creata
        dtR = dtT.NewRow()
        'controllo se l'autanticazine è andata a buon fine
        Dim intX As Integer

        For intX = 0 To 8
            If intX = 5 Then
                dtR(intX) = 0
            Else
                dtR(intX) = ""
            End If
        Next

        'aggiungo la riga appena caricata alla datatable
        dtT.Rows.Add(dtR)

        dtsLocal.Tables.Add(dtT)

        dtsLocal.DataSetName = "EsitoPagamenti"

        dtsLocal.Tables(0).TableName = "DettaglioPagamenti"

        Dim testXML As System.Xml.XmlNode
        'Dim xmlDOC As New System.Xml.XmlDataDocument
        Dim xmlDOC As New System.Xml.XmlDocument

        xmlDOC.InnerXml = dtsLocal.GetXmlSchema

        EseguiRicercaPagamentiXSD = xmlDOC

        sqlConn.Close()

        Return EseguiRicercaPagamentiXSD

    End Function

    Public Function MandaEmail(ByVal dest As String, ByVal nominativo As String, ByVal sesso As Char, ByVal psw As String) As String
        Dim email As New System.Net.Mail.MailMessage ' System.Web.Mail.MailMessage
        Dim msgBody As New StringBuilder
        'Dim msgBodyFormat As Integer
        Dim msgBodyText, resultMail As String

        If LCase(System.Configuration.ConfigurationManager.AppSettings("mailBodyFormat")) = "html" Then
            msgBody.Append(IIf(sesso = "M", "Egregio ", "Egregia "))
            msgBody.Append(nominativo)
            msgBody.Append(",<br/>")
            msgBody.Append(System.Configuration.ConfigurationManager.AppSettings("mailBody"))
            msgBody.Append("<strong>")
            msgBody.Append(psw)
            msgBody.Append("</strong>")
            msgBody.Append("<p/>")
            msgBody.Append("cordiali saluti")
            msgBody.Append("<br/>")
            msgBody.Append("<i>")
            msgBody.Append("GestioneSito")
            msgBody.Append("<i/>")
            msgBody.Append("</p>")

            'msgBodyFormat = Web.Mail.MailFormat.Html
        Else
            msgBodyText = IIf(sesso = "M", "Egregio ", "Egregia ") & nominativo & "," & vbCrLf _
                            & System.Configuration.ConfigurationManager.AppSettings("mailBody") & psw & vbCrLf & vbCrLf & "Cordiali saluti" & vbCrLf & "GestioneSito"

            msgBody.Append(msgBodyText)
            'msgBodyFormat = Web.Mail.MailFormat.Text
        End If

        Dim mailTo As System.Net.Mail.MailAddress

        mailTo = New System.Net.Mail.MailAddress(dest)

        email.To.Add(mailTo)

		Dim mailToBcc As System.Net.Mail.MailAddress

        mailToBcc = New System.Net.Mail.MailAddress(System.Configuration.ConfigurationManager.AppSettings("mailBcc"))

        email.Bcc.Add(mailToBcc)
        Dim mailFrom As System.Net.Mail.MailAddress

        mailFrom = New System.Net.Mail.MailAddress(System.Configuration.ConfigurationManager.AppSettings("mailFrom"))

        email.From = mailFrom

        email.Body = msgBody.ToString
        email.Subject = System.Configuration.ConfigurationManager.AppSettings("mailSubject")

        email.IsBodyHtml = True

        Dim smtp As System.Net.Mail.SmtpClient = New System.Net.Mail.SmtpClient(System.Configuration.ConfigurationManager.AppSettings("SmtpServer"))

        resultMail = ""

        Try
            smtp.Send(email)

            'System.Web.Mail.SmtpMail.Send(email)

        Catch ex As Exception
            Dim myLog As New GeneraLog(ex.Message, "MandaEmail")
            myLog.GeneraFileLog()
            resultMail = ex.Message
        Finally
            MandaEmail = resultMail
        End Try

    End Function


    <WebMethod()> _
Public Function EseguiModificaStatoQuestionario(ByVal strUserName As String, ByVal strPWD As String, ByVal strCodVol As String, ByVal intStatoQuestionario As Integer) As String
        Dim wrapper As New Simple3Des
        Dim strSql As String
        Dim sqlConn As New SqlClient.SqlConnection
        Dim xmlLocal As String
        Dim dtR As DataRow
        Dim dtT As New DataTable
        Dim dtsLocal As New DataSet
        Dim myCommand As New SqlClient.SqlCommand
        Dim dtrLocal As SqlClient.SqlDataReader
        Dim strTipoUtente As String
        Dim intIdEntita As Integer

        Try
            'strPWD = wrapper.EncryptData(strPWD)
            'strUserName = wrapper.EncryptData(strUserName)

            'aggiungo cmq la colonna dell'esito
            dtT.Columns.Add(New DataColumn("Esito", GetType(String)))
            dtT.Columns.Add(New DataColumn("CausaleEsitoNegativo", GetType(String)))

            If strUserName = "" Or strPWD = "" Then
                'dtT.Columns.Add()
                'alla riga assegno la riga delle intestazioni appena creata
                dtR = dtT.NewRow()

                'carico la prima riga della datatable
                dtR(0) = "NEGATIVO"
                dtR(1) = "USERNAME O PASSWORD ERRATI"

                'aggiungo la riga appena caricata alla datatable
                dtT.Rows.Add(dtR)

                dtsLocal.Tables.Add(dtT)

                dtsLocal.DataSetName = "EsitoModificaQuestionario"

                dtsLocal.Tables(0).TableName = "Dettagli"

                xmlLocal = dtsLocal.GetXml

                EseguiModificaStatoQuestionario = xmlLocal

                Return EseguiModificaStatoQuestionario
            Else
                If wrapper.DecryptData(strUserName) = "" Or wrapper.DecryptData(strPWD) = "" Then
                    'dtT.Columns.Add()
                    'alla riga assegno la riga delle intestazioni appena creata
                    dtR = dtT.NewRow()

                    'carico la prima riga della datatable
                    dtR(0) = "NEGATIVO"
                    dtR(1) = "USERNAME O PASSWORD ERRATI"

                    'aggiungo la riga appena caricata alla datatable
                    dtT.Rows.Add(dtR)

                    dtsLocal.Tables.Add(dtT)

                    dtsLocal.DataSetName = "EsitoModificaQuestionario"

                    dtsLocal.Tables(0).TableName = "Dettagli"

                    xmlLocal = dtsLocal.GetXml

                    EseguiModificaStatoQuestionario = xmlLocal

                    Return EseguiModificaStatoQuestionario
                End If
            End If

            sqlConn.ConnectionString = System.Configuration.ConfigurationManager.AppSettings("cnHelios")

            sqlConn.Open()

            strSql = "select Denominazione, Username, Password, Identificativo,IdStatoEnte,getdate() as dataserver, Tipo, FlagForzaturaAccreditamento from VW_Account_Utenti where username='" & wrapper.DecryptData(strUserName) & "' and password='" & CreatePSW(wrapper.DecryptData(strPWD)).Replace("'", "''") & "' and heliosRead=0 "

            'sql command che mi esegue la select
            myCommand = New SqlClient.SqlCommand
            myCommand.Connection = sqlConn

            'controllo e chiudo se aperto il datareader
            If Not dtrLocal Is Nothing Then
                dtrLocal.Close()
                dtrLocal = Nothing
            End If

            'eseguo la query
            myCommand.CommandText = strSql
            dtrLocal = myCommand.ExecuteReader

            If dtrLocal.HasRows = True Then
                dtrLocal.Read()

                'prendo l'identificativo dell'entità
                intIdEntita = dtrLocal("identificativo")

                'controllo e chiudo se aperto il datareader
                If Not dtrLocal Is Nothing Then
                    dtrLocal.Close()
                    dtrLocal = Nothing
                End If

                strSql = "select CodiceVolontario from entità where identità=" & intIdEntita

                'sql command che mi esegue la insert
                myCommand = New SqlClient.SqlCommand
                myCommand.Connection = sqlConn

                'controllo e chiudo se aperto il datareader
                If Not dtrLocal Is Nothing Then
                    dtrLocal.Close()
                    dtrLocal = Nothing
                End If

                'eseguo la query
                myCommand.CommandText = strSql
                dtrLocal = myCommand.ExecuteReader

                If dtrLocal.HasRows = True Then
                    dtrLocal.Read()
                    Dim strCodiceVolontario As String = dtrLocal("CodiceVolontario")

                    'controllo e chiudo se aperto il datareader
                    If Not dtrLocal Is Nothing Then
                        dtrLocal.Close()
                        dtrLocal = Nothing
                    End If

                    If LCase(strCodiceVolontario) = LCase(strCodVol) Then
                        myCommand.CommandText = "UPDATE entità SET IdStatoQuestionario=" & intStatoQuestionario & ", UsernameQuestionario ='" & UCase(wrapper.DecryptData(strUserName)) & "', DataQuestionario = getdate() WHERE identità =" & intIdEntita
                        myCommand.ExecuteNonQuery()

                        'alla riga assegno la riga delle intestazioni appena creata
                        dtR = dtT.NewRow()

                        'carico la prima riga della datatable
                        dtR(0) = "POSITIVO"
                        dtR(1) = ""

                        'aggiungo la riga appena caricata alla datatable
                        dtT.Rows.Add(dtR)

                        dtsLocal.Tables.Add(dtT)

                        dtsLocal.DataSetName = "EsitoModificaQuestionario"

                        dtsLocal.Tables(0).TableName = "Dettagli"

                        xmlLocal = dtsLocal.GetXml

                        EseguiModificaStatoQuestionario = xmlLocal

                        Return EseguiModificaStatoQuestionario
                    Else
                        'alla riga assegno la riga delle intestazioni appena creata
                        dtR = dtT.NewRow()

                        'carico la prima riga della datatable
                        dtR(0) = "NEGATIVO"
                        dtR(1) = "CODICE VOLONTARIO NON CORRISPONDENTE"

                        'aggiungo la riga appena caricata alla datatable
                        dtT.Rows.Add(dtR)

                        dtsLocal.Tables.Add(dtT)

                        dtsLocal.DataSetName = "EsitoModificaStatoQuestionario"

                        dtsLocal.Tables(0).TableName = "Dettagli"

                        xmlLocal = dtsLocal.GetXml

                        EseguiModificaStatoQuestionario = xmlLocal

                        Return EseguiModificaStatoQuestionario
                    End If
                Else
                    'controllo e chiudo se aperto il datareader
                    If Not dtrLocal Is Nothing Then
                        dtrLocal.Close()
                        dtrLocal = Nothing
                    End If
                End If

                'controllo e chiudo se aperto il datareader
                If Not dtrLocal Is Nothing Then
                    dtrLocal.Close()
                    dtrLocal = Nothing
                End If
            Else
                'dtT.Columns.Add()
                'alla riga assegno la riga delle intestazioni appena creata
                dtR = dtT.NewRow()

                'carico la prima riga della datatable
                dtR(0) = "NEGATIVO"
                dtR(1) = "UTENTE INESISTENTE"

                'aggiungo la riga appena caricata alla datatable
                dtT.Rows.Add(dtR)

                dtsLocal.Tables.Add(dtT)

                dtsLocal.DataSetName = "EsitoModificaStatoQuestionario"

                dtsLocal.Tables(0).TableName = "Dettagli"

                xmlLocal = dtsLocal.GetXml

                EseguiModificaStatoQuestionario = xmlLocal

                Return EseguiModificaStatoQuestionario
            End If

            sqlConn.Close()

        Catch ex As Exception
            'dtT.Columns.Add()
            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "NEGATIVO"
            dtR(1) = "ERRORE GENERICO"

            'scrivo l'errore nel file di log
            Dim myLog As New GeneraLog(ex.Message, "EseguiModificaStatoQuestionario")
            myLog.GeneraFileLog()

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)

            dtsLocal.Tables.Add(dtT)

            dtsLocal.DataSetName = "EsitoModificaQuestionario"

            dtsLocal.Tables(0).TableName = "Dettagli"

            xmlLocal = dtsLocal.GetXml

            EseguiModificaStatoQuestionario = xmlLocal

            Return EseguiModificaStatoQuestionario
        End Try

    End Function

    '<WebMethod()> _
    Public Function EseguiModificaStatoQuestionarioXSD() As System.Xml.XmlDocument
        'Dim strSql As String
        Dim xmlLocal As String
        Dim dtR As DataRow
        Dim dtT As New DataTable
        Dim dtsLocal As New DataSet

        'aggiungo cmq la colonna dell'esito
        dtT.Columns.Add(New DataColumn("Esito", GetType(String)))
        dtT.Columns.Add(New DataColumn("CausaleEsitoNegativo", GetType(String)))

        'dtT.Columns.Add()
        'alla riga assegno la riga delle intestazioni appena creata
        dtR = dtT.NewRow()
        'controllo se l'autanticazine è andata a buon fine
        Dim intX As Integer

        For intX = 0 To 1
            dtR(intX) = ""
        Next

        'controllo e chiudo se aperto il datareader
        'If Not dtrLocal Is Nothing Then
        '    dtrLocal.Close()
        '    dtrLocal = Nothing
        'End If
        'aggiungo la riga appena caricata alla datatable
        dtT.Rows.Add(dtR)

        dtsLocal.Tables.Add(dtT)

        dtsLocal.DataSetName = "EsitoModificaQuestionario"

        dtsLocal.Tables(0).TableName = "Dettagli"

        Dim testXML As System.Xml.XmlNode
        Dim xmlDOC As New System.Xml.XmlDocument

        xmlDOC.InnerXml = dtsLocal.GetXmlSchema

        EseguiModificaStatoQuestionarioXSD = xmlDOC

        Return EseguiModificaStatoQuestionarioXSD()

    End Function
    '<WebMethod()> _
    Public Function EseguiRichiestaContrattoXSD() As System.Xml.XmlDocument
        Dim strSql As String
        Dim xmlLocal As String
        Dim dtR As DataRow
        Dim dtT As New DataTable
        Dim dtsLocal As New DataSet

        'aggiungo cmq la colonna dell'esito
        dtT.Columns.Add(New DataColumn("Contratto", GetType(String)))
        dtT.Columns.Add(New DataColumn("Esito", GetType(String)))
        dtT.Columns.Add(New DataColumn("CausaleEsitoNegativo", GetType(String)))

        'dtT.Columns.Add()
        'alla riga assegno la riga delle intestazioni appena creata
        dtR = dtT.NewRow()
        'controllo se l'autanticazine è andata a buon fine
        Dim intX As Integer

        For intX = 0 To 2
            dtR(intX) = ""
        Next

        'controllo e chiudo se aperto il datareader
        'If Not dtrLocal Is Nothing Then
        '    dtrLocal.Close()
        '    dtrLocal = Nothing
        'End If
        'aggiungo la riga appena caricata alla datatable
        dtT.Rows.Add(dtR)

        dtsLocal.Tables.Add(dtT)

        dtsLocal.DataSetName = "EsitoRichiestaContratto"

        dtsLocal.Tables(0).TableName = "DettaglioContratto"

        Dim testXML As System.Xml.XmlNode
        Dim xmlDOC As New System.Xml.XmlDocument

        xmlDOC.InnerXml = dtsLocal.GetXmlSchema

        EseguiRichiestaContrattoXSD = xmlDOC

        Return EseguiRichiestaContrattoXSD

    End Function

    <WebMethod()> _
    Public Function EseguiRichiestaContratto(ByVal strUserName As String, ByVal strPWD As String) As String
        Dim wrapper As New Simple3Des
        Dim strSql As String
        Dim sqlConn As New SqlClient.SqlConnection
        Dim dtR As DataRow
        Dim dtT As New DataTable
        Dim dtsLocal As New DataSet
        Dim myCommand As New SqlClient.SqlCommand
        Dim dtrLocal As SqlClient.SqlDataReader
        Dim xmlLocal As String

        Dim strDecriptUser As String = wrapper.DecryptData(strUserName).ToString.Replace("'", "''")
        Dim strDecriptPSW As String = CreatePSW(wrapper.DecryptData(strPWD)).ToString.Replace("'", "''")

        'Dim strDecriptUser As String = strUserName.ToString.Replace("'", "''")
        'Dim strDecriptPSW As String = CreatePSW(strPWD).ToString.Replace("'", "''")


        Dim dbg As Char = System.Configuration.ConfigurationManager.AppSettings("debugApp")
        Try
            'aggiungo cmq la colonna dell'esito
            dtT.Columns.Add(New DataColumn("Contratto", GetType(String)))
            dtT.Columns.Add(New DataColumn("Esito", GetType(String)))
            dtT.Columns.Add(New DataColumn("CausaleEsitoNegativo", GetType(String)))

            If strUserName = "" Or strPWD = "" Then
                'dtT.Columns.Add()
                'alla riga assegno la riga delle intestazioni appena creata
                dtR = dtT.NewRow()

                'carico la prima riga della datatable
                dtR(0) = ""
                dtR(1) = "NEGATIVO"
                dtR(2) = "TUTTI I CAMPI SONO NECESSARI"

                'aggiungo la riga appena caricata alla datatable
                dtT.Rows.Add(dtR)

                dtsLocal.Tables.Add(dtT)

                dtsLocal.DataSetName = "EsitoRichiestaContratto"

                dtsLocal.Tables(0).TableName = "DettaglioContratto"

                xmlLocal = dtsLocal.GetXml

                EseguiRichiestaContratto = xmlLocal

                Return EseguiRichiestaContratto
            Else
                If strDecriptUser = "" Or strDecriptPSW = "" Then
                    'If strUserName = "" Or strPWD = "" Then
                    'dtT.Columns.Add()
                    'alla riga assegno la riga delle intestazioni appena creata
                    dtR = dtT.NewRow()

                    'carico la prima riga della datatable
                    dtR(0) = ""
                    dtR(1) = "NEGATIVO"
                    dtR(2) = "TUTTI I CAMPI SONO NECESSARI"

                    'aggiungo la riga appena caricata alla datatable
                    dtT.Rows.Add(dtR)

                    dtsLocal.Tables.Add(dtT)

                    dtsLocal.DataSetName = "EsitoRichiestaContratto"

                    dtsLocal.Tables(0).TableName = "DettaglioContratto"

                    xmlLocal = dtsLocal.GetXml

                    EseguiRichiestaContratto = xmlLocal

                    Return EseguiRichiestaContratto
                End If
            End If


            sqlConn.ConnectionString = System.Configuration.ConfigurationManager.AppSettings("cnHelios")
            sqlConn.Open()
            strSql = "select Denominazione, Username, Password, Identificativo,IdStatoEnte,getdate() as dataserver, Tipo, FlagForzaturaAccreditamento from VW_Account_Utenti where username='" & strDecriptUser & "' and password='" & strDecriptPSW & "' and heliosRead=0 "

            'sql command che mi esegue la insert
            myCommand = New SqlClient.SqlCommand
            myCommand.Connection = sqlConn

            'controllo e chiudo se aperto il datareader
            If Not dtrLocal Is Nothing Then
                dtrLocal.Close()
                dtrLocal = Nothing
            End If

            'eseguo la query
            myCommand.CommandText = strSql
            dtrLocal = myCommand.ExecuteReader

            If dtrLocal.HasRows = True Then
                dtrLocal.Read()

                'controllo e chiudo se aperto il datareader
                If Not dtrLocal Is Nothing Then
                    dtrLocal.Close()
                    dtrLocal = Nothing
                End If

                strSql = "select IdEntità, DBO.FN_WS_ABILITA_DOWNLOAD_CONTRATTO(IdEntità) AS ABILITA_CONTRATTO, DBO.FN_WS_ABILITA_DOWNLOAD_CONTRATTO_INTEGRATIVO(IdEntità) AS ABILITA_CONTRATTO_INTEGRATIVO from entità where username='" & strDecriptUser & "' and password='" & strDecriptPSW & "' "

                'sql command che mi esegue la insert
                myCommand = New SqlClient.SqlCommand
                myCommand.Connection = sqlConn

                'controllo e chiudo se aperto il datareader
                If Not dtrLocal Is Nothing Then
                    dtrLocal.Close()
                    dtrLocal = Nothing
                End If

                'eseguo la query
                myCommand.CommandText = strSql
                dtrLocal = myCommand.ExecuteReader

                If dtrLocal.HasRows = True Then
                    dtrLocal.Read()
                    Dim sDati As String
                    Dim strEsito As String
                    Dim strABILITA_CONTRATTO As String = dtrLocal("ABILITA_CONTRATTO")
                    Dim strABILITA_CONTRATTO_INTEGRATIVO As String = dtrLocal("ABILITA_CONTRATTO_INTEGRATIVO")
                    Dim intIdVol As Integer = dtrLocal("IdEntità")
                    Dim strUserNameVol As Integer

                    'controllo e chiudo se aperto il datareader
                    If Not dtrLocal Is Nothing Then
                        dtrLocal.Close()
                        dtrLocal = Nothing
                    End If
                    Dim strPercorsoFile As String

                    'controllo ABILITA_CONTRATTO
                    If strABILITA_CONTRATTO = "OK" Or strABILITA_CONTRATTO_INTEGRATIVO = "OK" Then
                        'verifico esistenza contratto pregenerato
                        strSql = "select contratto from EntitàContrattiSpot where identità = " & intIdVol
                        myCommand.CommandText = strSql
                        dtrLocal = myCommand.ExecuteReader
                        If dtrLocal.HasRows = True Then
                            dtrLocal.Read()
                            strEsito = dtrLocal("contratto")
                            If Not dtrLocal Is Nothing Then
                                dtrLocal.Close()
                                dtrLocal = Nothing
                            End If
                        Else
                            If Not dtrLocal Is Nothing Then
                                dtrLocal.Close()
                                dtrLocal = Nothing
                            End If
                            strPercorsoFile = CreaContrattoVolontario(intIdVol, strDecriptUser)

                            strEsito = CreaPDFdaStringaRTF(strPercorsoFile)
                        End If



                        If strEsito = "ERRORE GENERICO. CONTATTARE L'ASSISTENZA" Or strPercorsoFile = "ERRORE GENERICO. CONTATTARE L'ASSISTENZA" Then
                            'dtT.Columns.Add()
                            'alla riga assegno la riga delle intestazioni appena creata
                            dtR = dtT.NewRow()

                            'carico la prima riga della datatable
                            dtR(0) = ""
                            dtR(1) = "NEGATIVO"
                            dtR(2) = strEsito

                            'aggiungo la riga appena caricata alla datatable
                            dtT.Rows.Add(dtR)

                            dtsLocal.Tables.Add(dtT)

                            dtsLocal.DataSetName = "EsitoRichiestaContratto"

                            dtsLocal.Tables(0).TableName = "DettaglioContratto"

                            xmlLocal = dtsLocal.GetXml

                            EseguiRichiestaContratto = xmlLocal

                            Return EseguiRichiestaContratto
                        Else
                            'aggiunto il 13/11/2013
                            'inserisco nella tabella della cronologia
                            If strABILITA_CONTRATTO_INTEGRATIVO = "OK" Then
                                strSql = "INSERT INTO cronologiaentitàwebdoc (Identità,username,datacronologia,tipodocumento) VALUES (" & intIdVol & ",'" & strDecriptUser & "',GETDATE(),'ATTOAGGIUNTIVO')"
                            Else
                                strSql = "INSERT INTO cronologiaentitàwebdoc (Identità,username,datacronologia,tipodocumento) VALUES (" & intIdVol & ",'" & strDecriptUser & "',GETDATE(),'CONTRATTO')"
                            End If

                            myCommand = New SqlClient.SqlCommand
                            myCommand.Connection = sqlConn
                            myCommand.CommandText = strSql
                            myCommand.ExecuteNonQuery()
                            'dtT.Columns.Add()
                            'alla riga assegno la riga delle intestazioni appena creata
                            dtR = dtT.NewRow()

                            'converto il file in base64
                            'Dim testoBase64 As String

                            'testoBase64 = FileToBase64(Server.MapPath(strEsito))
                            'testoBase64 = FileToBase64(strEsito)

                            'carico la prima riga della datatable
                            dtR(0) = strEsito
                            dtR(1) = "POSITIVO"
                            dtR(2) = ""

                            'aggiungo la riga appena caricata alla datatable
                            dtT.Rows.Add(dtR)

                            dtsLocal.Tables.Add(dtT)

                            dtsLocal.DataSetName = "EsitoRichiestaContratto"

                            dtsLocal.Tables(0).TableName = "DettaglioContratto"

                            xmlLocal = dtsLocal.GetXml

                            EseguiRichiestaContratto = xmlLocal

                            Return EseguiRichiestaContratto
                        End If
                    Else
                        'dtT.Columns.Add()
                        'alla riga assegno la riga delle intestazioni appena creata
                        dtR = dtT.NewRow()

                        'carico la prima riga della datatable
                        dtR(0) = ""
                        dtR(1) = "NEGATIVO"
                        dtR(2) = strABILITA_CONTRATTO

                        'aggiungo la riga appena caricata alla datatable
                        dtT.Rows.Add(dtR)

                        dtsLocal.Tables.Add(dtT)

                        dtsLocal.DataSetName = "EsitoRichiestaContratto"

                        dtsLocal.Tables(0).TableName = "DettaglioContratto"

                        xmlLocal = dtsLocal.GetXml

                        EseguiRichiestaContratto = xmlLocal

                        Return EseguiRichiestaContratto
                    End If
                Else

                    'controllo e chiudo se aperto il datareader
                    If Not dtrLocal Is Nothing Then
                        dtrLocal.Close()
                        dtrLocal = Nothing
                    End If

                    'dtT.Columns.Add()
                    'alla riga assegno la riga delle intestazioni appena creata
                    dtR = dtT.NewRow()

                    'carico la prima riga della datatable
                    dtR(0) = ""
                    dtR(1) = "NEGATIVO"
                    dtR(2) = "USERNAME O PASSWORD ERRATI"

                    'aggiungo la riga appena caricata alla datatable
                    dtT.Rows.Add(dtR)

                    dtsLocal.Tables.Add(dtT)

                    dtsLocal.DataSetName = "EsitoRichiestaContratto"

                    dtsLocal.Tables(0).TableName = "DettaglioContratto"

                    xmlLocal = dtsLocal.GetXml

                    EseguiRichiestaContratto = xmlLocal

                    Return EseguiRichiestaContratto
                End If

            Else
                'dtT.Columns.Add()
                'alla riga assegno la riga delle intestazioni appena creata
                dtR = dtT.NewRow()

                'carico la prima riga della datatable
                dtR(0) = ""
                dtR(1) = "NEGATIVO"
                dtR(2) = "USERNAME O PASSWORD ERRATI"

                'aggiungo la riga appena caricata alla datatable
                dtT.Rows.Add(dtR)

                dtsLocal.Tables.Add(dtT)

                dtsLocal.DataSetName = "EsitoRichiestaContratto"

                dtsLocal.Tables(0).TableName = "DettaglioContratto"

                xmlLocal = dtsLocal.GetXml

                EseguiRichiestaContratto = xmlLocal

                Return EseguiRichiestaContratto
            End If

            sqlConn.Close()
        Catch ex As Exception

            If Not sqlConn Is Nothing Then
                sqlConn.Close()
            End If

            'dtT.Columns.Add()
            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = ""
            dtR(1) = "NEGATIVO"
            dtR(2) = ex.Message

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)

            dtsLocal.Tables.Add(dtT)

            dtsLocal.DataSetName = "EsitoRichiestaContratto"

            dtsLocal.Tables(0).TableName = "DettaglioContratto"

            xmlLocal = dtsLocal.GetXml

            EseguiRichiestaContratto = xmlLocal

            Return EseguiRichiestaContratto


        End Try

    End Function

    Function CreaPDFdaRTF(ByVal strPercorsoFile As String) As String
        Try
            Dim pdfPath As String
            Dim myPath As New System.Web.UI.Page
            Dim strNomeUnivocoFile As String
            Dim str As String
            strPercorsoFile = Replace(strPercorsoFile, "/", "\")
            'estraggo il nome del file 
            For i = Len(strPercorsoFile) To 1 Step -1
                If InStr(Mid(strPercorsoFile, i, 1), "\") Then
                    Exit For
                End If
                strNomeUnivocoFile = Mid(strPercorsoFile, i, 1) & strNomeUnivocoFile
            Next
            'splitto la stringa del nome fisico del file per creare poi il file 
            Dim StringaArray() As String = strNomeUnivocoFile.Split(".")
            'stringa che prende il nome del file senza estenzione
            Dim strFileNoEstenzione As String = StringaArray(StringaArray.Length - 2).ToString()

            strPercorsoFile = myPath.Server.MapPath("documentazione") & "\" & strNomeUnivocoFile 'strNomeFile & "_" &

            Dim p As New SautinSoft.PdfMetamorphosis()
            p.Serial = "10071755221"
            p.PageStyle.PageOrientation.Portrait()


            If p IsNot Nothing Then

                Dim rtfPath As String = strPercorsoFile
                pdfPath = myPath.Server.MapPath("documentazione") & "\" & strFileNoEstenzione & ".pdf"

                Dim i As Integer = p.RtfToPdfConvertFile(rtfPath, pdfPath)

                'Str = i & " " & rtfPath & " " & pdfPath

                If i <> 0 Then
                    pdfPath = "ERRORE GENERICO. CONTATTARE L'ASSISTENZA"
                End If

            End If
            Return pdfPath
        Catch ex As Exception
            Return "ERRORE GENERICO. CONTATTARE L'ASSISTENZA"
        End Try
    End Function

    Function CreaPDFdaStringaRTF(ByVal strPercorsoFile As String) As String
        Try
            'Dim pdfPath As String
            'Dim myPath As New System.Web.UI.Page
            'Dim strNomeUnivocoFile As String
            'Dim str As String
            'strPercorsoFile = Replace(strPercorsoFile, "/", "\")
            'estraggo il nome del file 
            'For i = Len(strPercorsoFile) To 1 Step -1
            '    If InStr(Mid(strPercorsoFile, i, 1), "\") Then
            '        Exit For
            '    End If
            '    strNomeUnivocoFile = Mid(strPercorsoFile, i, 1) & strNomeUnivocoFile
            'Next
            'splitto la stringa del nome fisico del file per creare poi il file 
            ' Dim StringaArray() As String = strNomeUnivocoFile.Split(".")
            'stringa che prende il nome del file senza estenzione
            'Dim strFileNoEstenzione As String = StringaArray(StringaArray.Length - 2).ToString()

            'strPercorsoFile = myPath.Server.MapPath("documentazione") & "\" & strNomeUnivocoFile 'strNomeFile & "_" &

            Dim p As New SautinSoft.PdfMetamorphosis()
            p.Serial = "10071755221"
            p.PageStyle.PageOrientation.Portrait()

            Dim i As Byte()
            If p IsNot Nothing Then

                Dim rtfPath As String = strPercorsoFile
                ' pdfPath = myPath.Server.MapPath("documentazione") & "\" & strFileNoEstenzione & ".pdf"

                'Dim i As Integer = p.RtfToPdfConvertFile(rtfPath, pdfPath)



                'i = p.RtfToPdfConvertByte(System.Text.Encoding.UTF8.GetBytes(strPercorsoFile))

                i = p.RtfToPdfConvertByte(strPercorsoFile)

                'Str = i & " " & rtfPath & " " & pdfPath

                'If i <> 0 Then
                '    pdfPath = "ERRORE GENERICO. CONTATTARE L'ASSISTENZA"
                'End If

            End If
            Return Convert.ToBase64String(i)
        Catch ex As Exception
            Return "ERRORE GENERICO. CONTATTARE L'ASSISTENZA"
        End Try
    End Function

    <WebMethod()> _
    Public Function EseguiUploadContrattoVolontario(ByVal strUserName As String, ByVal strPWD As String, ByVal File As String, ByVal NomeFile As String) As String
        '*** Generato da simona cordella il 10/09/2012 *** 
        '*** Metodo che consente al volontario di eseguire l'upload del contratto firmato in formta .pdf ***
        Dim wrapper As New Simple3Des
        Dim strSql As String
        Dim sqlConn As New SqlClient.SqlConnection
        Dim dtR As DataRow
        Dim dtT As New DataTable
        Dim dtsLocal As New DataSet
        Dim myCommand As New SqlClient.SqlCommand
        Dim dtrLocal As SqlClient.SqlDataReader
        Dim xmlLocal As String

        Dim strDecriptUser As String = wrapper.DecryptData(strUserName).ToString.Replace("'", "''")
        Dim strDecriptPSW As String = CreatePSW(wrapper.DecryptData(strPWD)).ToString.Replace("'", "''")

        Dim dbg As Char = System.Configuration.ConfigurationManager.AppSettings("debugApp")

        Dim pdfPath As String
        Dim myPath As New System.Web.UI.Page
        Dim strNomeUnivocoFile As String
        Dim str As String
        Dim HeliosInterno As New WSHeliosInterno.HeliosInterno
        Dim EsitoWsInterno As String
        Dim idEntità As Integer
        Dim strAbilitaContratto As String

        Try
            'aggiungo cmq la colonna dell'esito
            dtT.Columns.Add(New DataColumn("Esito", GetType(String)))
            dtT.Columns.Add(New DataColumn("CausaleEsito", GetType(String)))

            Dim TipoFile As String
            TipoFile = UCase(TrovaTipoFile(NomeFile))
            If TipoFile = "PDF" Or TipoFile = "TIF" Or TipoFile = "TIFF" Then
                sqlConn.ConnectionString = System.Configuration.ConfigurationManager.AppSettings("cnHelios")
                sqlConn.Open()
                strSql = "select IdEntità, DBO.FN_WS_ABILITA_UPLOAD_CONTRATTO(IdEntità) AS ABILITA_CONTRATTO from entità where username='" & strDecriptUser & "' and password='" & strDecriptPSW & "' "

                'sql command che mi esegue la insert
                myCommand = New SqlClient.SqlCommand
                myCommand.Connection = sqlConn
                'controllo e chiudo se aperto il datareader
                If Not dtrLocal Is Nothing Then
                    dtrLocal.Close()
                    dtrLocal = Nothing
                End If

                'eseguo la query
                myCommand.CommandText = strSql
                dtrLocal = myCommand.ExecuteReader

                If dtrLocal.HasRows = True Then
                    dtrLocal.Read()
                    idEntità = dtrLocal("IdEntità")
                    strAbilitaContratto = dtrLocal("ABILITA_CONTRATTO")
                    'controllo e chiudo se aperto il datareader
                    If Not dtrLocal Is Nothing Then
                        dtrLocal.Close()
                        dtrLocal = Nothing
                    End If
                    'verifico se non sono scaduti i termini per l' upload del contratto
                    If strAbilitaContratto = "OK" Then
                        'richiamo metodo del WSInterno per interazione con SIGED
                        EsitoWsInterno = HeliosInterno.UploadContrattoVolontario(idEntità, File, NomeFile)
                        'EsitoWsInterno = "ERRORE"  il file non è stato registrato sul sistema SIGED
                        If EsitoWsInterno = "ERRORE" Then
                            dtR = dtT.NewRow()

                            'carico la prima riga della datatable
                            dtR(0) = "NEGATIVO"
                            dtR(1) = "IMPOSSIBILE CARICARE IL FILE."

                            'aggiungo la riga appena caricata alla datatable
                            dtT.Rows.Add(dtR)

                            dtsLocal.Tables.Add(dtT)

                            dtsLocal.DataSetName = "EsitoUploadContratto"

                            dtsLocal.Tables(0).TableName = "DettaglioContratto"

                            xmlLocal = dtsLocal.GetXml

                            EseguiUploadContrattoVolontario = xmlLocal

                            Return EseguiUploadContrattoVolontario
                        Else
                            'EsitoWsIntern= codicedocumento (es. 1111/aaaa#numero) il file è stato registrato sul sistema SIGED

                            'UPDATE ECC...
                            strSql = "UPDATE Entità SET  RiferimentoContrattoVolontario = '" & EsitoWsInterno & "',StatoContrattoVolontario = 1, NomeFileContratto='" & NomeFile & "' where identità = " & idEntità
                            myCommand.CommandText = strSql
                            myCommand.ExecuteNonQuery()
							'agg. il 11/11/2013
                            'INSERT IN CRONOLOGIAENTITA'DOCUMENTI
                            strSql = "INSERT INTO CronologiaEntitàContratto (IdEntità, RiferimentoContrattoVolontario, StatoContratto, NomeFileContratto, UserNameStato, DataCronologiaStato)"
                            strSql = strSql & " (SELECT IDEntità, RiferimentoContrattoVolontario, StatoContrattoVolontario, NomeFileContratto, Username, DataValutazioneContratto "
                            strSql = strSql & "  FROM entità "
                            strSql = strSql & "  where IdEntità = " & idEntità & " )"
                            myCommand.CommandText = strSql
                            myCommand.ExecuteNonQuery()

                            'alla riga assegno la riga delle intestazioni appena creata
                            dtR = dtT.NewRow()

                            dtR(0) = "POSITIVO"
                            dtR(1) = "CARICAMENTO EFFETTUATO CON SUCCESSO."

                            'aggiungo la riga appena caricata alla datatable
                            dtT.Rows.Add(dtR)

                            dtsLocal.Tables.Add(dtT)

                            dtsLocal.DataSetName = "EsitoUploadContratto"

                            dtsLocal.Tables(0).TableName = "DettaglioContratto"

                            xmlLocal = dtsLocal.GetXml

                            EseguiUploadContrattoVolontario = xmlLocal
                            Return EseguiUploadContrattoVolontario
                        End If
                    Else
                        dtR = dtT.NewRow()
                        'carico la prima riga della datatable
                        dtR(0) = "NEGATIVO"
                        dtR(1) = strAbilitaContratto

                        'aggiungo la riga appena caricata alla datatable
                        dtT.Rows.Add(dtR)

                        dtsLocal.Tables.Add(dtT)

                        dtsLocal.DataSetName = "EsitoUploadContratto"

                        dtsLocal.Tables(0).TableName = "DettaglioContratto"

                        xmlLocal = dtsLocal.GetXml

                        EseguiUploadContrattoVolontario = xmlLocal

                        Return EseguiUploadContrattoVolontario
                    End If
                Else
                    If Not dtrLocal Is Nothing Then
                        dtrLocal.Close()
                        dtrLocal = Nothing
                    End If
                    dtR = dtT.NewRow()

                    'carico la prima riga della datatable

                    dtR(0) = "NEGATIVO"
                    dtR(1) = "USERNAME O PASSWORD ERRATI."

                    'aggiungo la riga appena caricata alla datatable
                    dtT.Rows.Add(dtR)

                    dtsLocal.Tables.Add(dtT)

                    dtsLocal.DataSetName = "EsitoUploadContratto"

                    dtsLocal.Tables(0).TableName = "DettaglioContratto"

                    xmlLocal = dtsLocal.GetXml

                    EseguiUploadContrattoVolontario = xmlLocal

                    Return EseguiUploadContrattoVolontario
                End If

                sqlConn.Close()
            Else
                'MESSAGGIO PER TIPOLOGIA DI FILE NON CONSENTITA
                dtR = dtT.NewRow()

                'carico la prima riga della datatable
                dtR(0) = "NEGATIVO"
                dtR(1) = "IMPOSSIBILE CARICARE IL FILE. TIPOLOGIA DI FILE NON CONSENTITA."

                'aggiungo la riga appena caricata alla datatable
                dtT.Rows.Add(dtR)

                dtsLocal.Tables.Add(dtT)

                dtsLocal.DataSetName = "EsitoUploadContratto"

                dtsLocal.Tables(0).TableName = "DettaglioContratto"

                xmlLocal = dtsLocal.GetXml

                EseguiUploadContrattoVolontario = xmlLocal

                Return EseguiUploadContrattoVolontario
            End If
        Catch ex As Exception
            If Not sqlConn Is Nothing Then
                sqlConn.Close()
            End If

            'dtT.Columns.Add()
            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "NEGATIVO"
            dtR(1) = "ERRORE GENERICO."

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)

            dtsLocal.Tables.Add(dtT)

            dtsLocal.DataSetName = "EseguiUploadContrattoVolontario"

            dtsLocal.Tables(0).TableName = "DettaglioContratto"

            xmlLocal = dtsLocal.GetXml

            EseguiUploadContrattoVolontario = xmlLocal

            Return EseguiUploadContrattoVolontario
        End Try

    End Function

    Private Function TrovaTipoFile(ByVal File As String) As String
        'variabile contatore che uso per ciclarmi la stringa della PATH del file
        Dim intX As Integer
        'variabile che uso per calcolarmi il punto di inizio del nome del file
        Dim intInizioFile As Integer

        For intX = 1 To Len(File)
            If Mid(File, intX, 1) = "." Then
                intInizioFile = intX
            End If
        Next
        TrovaTipoFile = Mid(File, intInizioFile + 1)
    End Function

    Private Function ControlloEntitàSPCPerGenerazioneContratto(ByVal IDVolontario As Integer, ByVal connection As SqlConnection) As Boolean
        '** Creata da Simona Cordella
        '** Il 07/04/2015
        '** Funzione che verifica se il campo REQUISITI nella tabella Entità sia <> da SI; s
        '** stampo in modello del contratto (Contratto Volontario Italia (12 mesi) (Garanzia Giovani Senza Presa In Carico)
        '** Contratto Volontario Italia (subentro) (Garanzia Giovani Senza Presa In Carico))
        Dim query As String



        Dim dtsLocal As New DataSet
        Dim myCommand As New SqlClient.SqlCommand

        Dim dataReader As SqlClient.SqlDataReader
        Dim blnRequisito As Boolean

        query = " SELECT IDEntità " & _
                " FROM Entità  " & _
                " WHERE IDEntità =" & IDVolontario & " and isnull(UPPER(Requisiti),'') <> 'SI'"
        'sql command che mi esegue la insert
        myCommand = New SqlClient.SqlCommand
        myCommand.Connection = connection
        'eseguo la query
        myCommand.CommandText = query
        dataReader = myCommand.ExecuteReader

        blnRequisito = dataReader.HasRows

        dataReader.Close()
        dataReader = Nothing

        Return blnRequisito
    End Function
    <WebMethod()> _
    Public Function EseguiRicercaObiettiviProgrammi() As String
        Dim strSql As String
        Dim sqlConn As New SqlClient.SqlConnection
        Dim dtsLocal As New DataSet
        Dim xmlLocal As String

        Try

            sqlConn.ConnectionString = System.Configuration.ConfigurationManager.AppSettings("cnHelios")

            sqlConn.Open()


            strSql = "select CODICEPROGRAMMA, OBIETTIVO from VW_WS_RICERCAOBIETTIVIPROGRAMMI "

            Dim CMD As New SqlClient.SqlDataAdapter(strSql, sqlConn)

            dtsLocal.DataSetName = "DettaglioObiettiviProgrammi"

            'dtsLocal.Tables(0).TableName = "ENTI"

            CMD.Fill(dtsLocal)

            dtsLocal.Tables(0).TableName = "Dettaglio"

            xmlLocal = dtsLocal.GetXml

            EseguiRicercaObiettiviProgrammi = xmlLocal

            sqlConn.Close()

            Return EseguiRicercaObiettiviProgrammi
        Catch ex As Exception
            Dim myLog As New GeneraLog(ex.Message, "EseguiRicercaObiettiviProgrammi")
            myLog.GeneraFileLog()
        End Try
    End Function

    <WebMethod()> _
    Public Function EseguiRicercaAmbitiProgrammi() As String
        Dim strSql As String
        Dim sqlConn As New SqlClient.SqlConnection
        Dim dtsLocal As New DataSet
        Dim xmlLocal As String

        Try

            sqlConn.ConnectionString = System.Configuration.ConfigurationManager.AppSettings("cnHelios")

            sqlConn.Open()


            strSql = "select AMBITOAZIONE from VW_WS_RICERCAAMBITIPROGRAMMI "

            Dim CMD As New SqlClient.SqlDataAdapter(strSql, sqlConn)

            dtsLocal.DataSetName = "DettaglioAmbitiProgrammi"

            'dtsLocal.Tables(0).TableName = "ENTI"

            CMD.Fill(dtsLocal)

            dtsLocal.Tables(0).TableName = "Dettaglio"

            xmlLocal = dtsLocal.GetXml

            EseguiRicercaAmbitiProgrammi = xmlLocal

            sqlConn.Close()

            Return EseguiRicercaAmbitiProgrammi
        Catch ex As Exception
            Dim myLog As New GeneraLog(ex.Message, "EseguiRicercaAmbitiProgrammi")
            myLog.GeneraFileLog()
        End Try
    End Function

    <WebMethod()> _
    Public Function EseguiRichiestaCertificatoServizioSvolto(ByVal strUserName As String, ByVal strPWD As String, ByVal strSoloVerifica As String) As String
        'PATH del Certificato di servizio svolto
        'Dim PATH_DOC_CertificatoServizioSvolto As String = Server.MapPath("\documentazione\template\NAZ\volontari\CertificatoServizioSvolto.docx")
        'Dim documento As AsposeWord

        Dim wrapper As New Simple3Des
        Dim sqlConn As New SqlConnection
        Dim sqlCmd As New SqlCommand
        Dim strSql As String
        Dim drEsito As DataRow
        Dim dtsLocal As New DataSet
        Dim dtrLocal As SqlDataReader

        Const dataSetName As String = "EsitoGenerazioneDocumento"
        Const tableName As String = "Dati"

        Dim idVolontario As String
        Dim idStatoentita As Integer
        Dim dataFineServizio As DateTime

        Dim esito As String
        Dim messaggioEsito As String
        Dim documentoPDF As String

        'DataTable per restituire dati esito richieta
        Dim dtEsito As New DataTable
        dtEsito.Columns.Add(New DataColumn("Esito", GetType(String)))
        dtEsito.Columns.Add(New DataColumn("MessaggioEsito", GetType(String)))
        dtEsito.Columns.Add(New DataColumn("Certificato", GetType(String)))

        'Controlli formali su parametri di input al metodo
        If String.IsNullOrEmpty(strUserName) Or String.IsNullOrEmpty(strPWD) Then

            'Imposta esito negativo con messaggio di errore
            drEsito = dtEsito.NewRow()
            drEsito("Esito") = "NEGATIVO"
            drEsito("MessaggioEsito") = "USERNAME O PASSWORD ERRATI"

            'aggiungo la riga appena caricata alla datatable
            dtEsito.Rows.Add(drEsito)
            dtsLocal.Tables.Add(dtEsito)
            dtsLocal.DataSetName = dataSetName
            dtsLocal.Tables(0).TableName = tableName

            Return dtsLocal.GetXml

        End If

        'Decripta username e password del volontario
        Dim strDecriptUser As String = wrapper.DecryptData(strUserName).ToString.Replace("'", "''")
        Dim strDecriptPSW As String = CreatePSW(wrapper.DecryptData(strPWD)).ToString.Replace("'", "''")

        'Query SQL per lettura dati su DB
        strSql = " SELECT E.identità AS IdVolontario, E.IDStatoEntità AS IDStatoEntita, E.DataFineServizio "
        strSql &= " FROM entità E "
        strSql &= " INNER JOIN VW_Account_Utenti U ON E.identità = U.Identificativo "
        strSql &= " WHERE U.username = '" & strDecriptUser & "' "
        strSql &= " AND   U.password = '" & strDecriptPSW & "' "
        strSql &= " AND   U.tipo = 'V' AND U.heliosRead = 0 "

        Try
            esito = String.Empty

            'Apertura connessione
            sqlConn = New SqlConnection(System.Configuration.ConfigurationManager.AppSettings("cnHelios"))
            sqlConn.Open()

            'Inizializza comando
            sqlCmd = New SqlCommand With {
                .Connection = sqlConn,
                .CommandType = CommandType.Text,
                .CommandText = strSql
            }

            'lettura dati del volontario
            dtrLocal = sqlCmd.ExecuteReader()

            If dtrLocal.HasRows Then
                dtrLocal.Read()

                idVolontario = dtrLocal("IdVolontario").ToString()
                idStatoentita = CInt(dtrLocal("IDStatoEntita"))
                dataFineServizio = CType(dtrLocal("DataFineServizio"), DateTime)

                'Per generare il documento, il volontario deve avere i seguenti requisiti:
                ' - La data fine servizio deve essere minore della data corrente;
                ' - Lo stato del volontario (IDStatoEntità) deve essere uguale ad uno dei seguenti valori:
                '   . 3 (In Servizio);
                '   . 5 (Chiuso Durante Servizio);
                '   . 6 (Servizio Terminato);
                If (dataFineServizio >= DateTime.Now.Date) Then
                    esito = "NEGATIVO"
                    messaggioEsito = "PER RICHIEDERE IL CERTIFICATO IL VOLONTARIO DEVE AVER TERMINATO IL SERVIZIO"
                Else
                    If (idStatoentita <> 3 And idStatoentita <> 5 And idStatoentita <> 6) Then
                        esito = "NEGATIVO"
                        messaggioEsito = "IL VOLONTARIO NON RISULTA AVER SVOLTO IL SERVIZIO"
                    End If
                End If

            Else
                esito = "NEGATIVO"
                messaggioEsito = "USERNAME O PASSWORD ERRATI"
            End If

        Catch ex As Exception

            'Tracciatura eccezione sul LOG
            Dim myLog As New GeneraLog(ex.Message, "EseguiGenerazioneCertificatoServizioSvolto")
            myLog.GeneraFileLog()

            esito = "NEGATIVO"
            messaggioEsito = "ERRORE DURANTE LA LETTURA DEI DATI"

        Finally

            If dtrLocal IsNot Nothing Then
                dtrLocal.Close()
            End If

            If sqlConn.State = ConnectionState.Open Then
                sqlConn.Close()
            End If

        End Try

        'Verifica esito lettura dati del volontario
        If Not String.IsNullOrEmpty(esito) Then
            'Imposta esito negativo con messaggio di errore
            drEsito = dtEsito.NewRow()
            drEsito("Esito") = esito
            drEsito("MessaggioEsito") = messaggioEsito

            'aggiungo la riga appena caricata alla datatable
            dtEsito.Rows.Add(drEsito)
            dtsLocal.Tables.Add(dtEsito)
            dtsLocal.DataSetName = dataSetName
            dtsLocal.Tables(0).TableName = tableName

            Return dtsLocal.GetXml
        End If

        Try
            If Not dtrLocal Is Nothing Then
                dtrLocal.Close()
                dtrLocal = Nothing
            End If

            Dim strFile As String
            Dim strEsito As String
            If strSoloVerifica.ToUpper = "SI" Then
                strFile = ""
                strEsito = ""
            Else
                strFile = CreaCertificatoVolontario(idVolontario, strDecriptUser)
                strEsito = CreaPDFdaStringaRTF(strFile)
            End If

            If strEsito = "ERRORE GENERICO. CONTATTARE L'ASSISTENZA" Or strFile = "ERRORE GENERICO. CONTATTARE L'ASSISTENZA" Then
                drEsito = dtEsito.NewRow()
                drEsito("Esito") = "NEGATIVO"
                drEsito("MessaggioEsito") = strEsito

                'aggiungo la riga appena caricata alla datatable
                dtEsito.Rows.Add(drEsito)
                dtsLocal.Tables.Add(dtEsito)
                dtsLocal.DataSetName = dataSetName
                dtsLocal.Tables(0).TableName = tableName

                Return dtsLocal.GetXml

            Else
                drEsito = dtEsito.NewRow()
                drEsito("Esito") = "POSITIVO"
                drEsito("MessaggioEsito") = ""
                drEsito("Certificato") = strEsito

                'aggiungo la riga appena caricata alla datatable
                dtEsito.Rows.Add(drEsito)
                dtsLocal.Tables.Add(dtEsito)
                dtsLocal.DataSetName = dataSetName
                dtsLocal.Tables(0).TableName = tableName

                Return dtsLocal.GetXml
            End If


        Catch ex As Exception

            'Tracciatura eccezione sul LOG
            Dim myLog As New GeneraLog(ex.Message, "EseguiGenerazioneCertificatoServizioSvolto")
            myLog.GeneraFileLog()

            esito = "NEGATIVO"
            messaggioEsito = "ERRORE DURANTE LA GENERAZIONE DEL DOCUMENTO"

        Finally

            If dtrLocal IsNot Nothing Then
                dtrLocal.Close()
            End If

            If sqlConn.State = ConnectionState.Open Then
                sqlConn.Close()
            End If
        End Try



    End Function

    Private Function CreaCertificatoVolontario(ByVal IdVol As Integer, ByVal strUserName As String) As String
        'creata da simoma e danilo il 04/09/2012
        'generazione contratto volontario
        Dim strsql As String
        Dim sqlConn As New SqlConnection
        Dim dtrLocal As SqlDataReader
        Dim myCommand As SqlCommand
        Dim strDoc As String
        Dim strFlag As String = ""
        Dim strGruppo As String = ""

        Try
            sqlConn.ConnectionString = System.Configuration.ConfigurationManager.AppSettings("cnHelios")
            sqlConn.Open()

            Dim Documento As New GeneratoreModelli
            strDoc = Documento.VOL_CertificatoServizioSvolto(IdVol, strUserName, 22, sqlConn)
            Documento.Dispose()
            'Cronologia creazione documento.
            CronologiaDocEntità(IdVol, strUserName, "CertificatoServizioSvolto", sqlConn, "", "")

            
            Return strDoc
        Catch ex As Exception
            'Dim myLog As New GeneraLog(ex.Message, "EseguiRicercaClassi")
            'myLog.GeneraFileLog()
            Return "ERRORE GENERICO. CONTATTARE L'ASSISTENZA"
        Finally
            'controllo e chiudo se aperto il datareader
            If Not dtrLocal Is Nothing Then
                dtrLocal.Close()
                dtrLocal = Nothing
            End If
        End Try


    End Function
 End Class