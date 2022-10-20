Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.Data.SqlClient
Imports System.ComponentModel
Imports System.IO

' Per consentire la chiamata di questo servizio Web dallo script utilizzando ASP.NET AJAX, rimuovere il commento dalla riga seguente.
' <System.Web.Script.Services.ScriptService()> _
<System.Web.Services.WebService(Namespace:="http://tempuri.org/")> _
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<ToolboxItem(False)> _
Public Class HeliosInterno
    Inherits System.Web.Services.WebService
    Public Esito As String 'ritorna la descrizione dell'esito (IdSessione o Messaggio di errore)
    Public Codice_Esito As Integer
    Public Connessione As String
    'Public TipoNormalizzato As String 'RIPORTA SE IL CODICE è UN FASCICOLO, PROTOCOLLO O UN DOCUMENTO
    Public CodiceNormalizzato As String 'CODICE DEL FASCICOLO,PROTOCOLLO O DEL DOCUMENTO
    <WebMethod()> _
    Public Function CreaFascicoloIstanza(ByVal IdBando As Integer, ByVal IdEnte As Integer) As String
        ''Return "Hello World"
        Dim wsOUT As WS_SIGeD.FASCICOLO_CREATO
        Dim sqlConn As New SqlClient.SqlConnection
        Dim myCommand As New SqlClient.SqlCommand
        Dim dtrLocal As SqlClient.SqlDataReader
        Dim strSql As String

        Dim strDescrizioneFascicolo As String
        Dim strTitolario As String
        Dim strCognome As String
        Dim strNome As String
        Dim strServizio As String
        Dim strDefault As String


        Try
            'RECUPERO INFO WS DA WEBCONFIG
            strCognome = System.Configuration.ConfigurationManager.AppSettings("Cognome")
            strNome = System.Configuration.ConfigurationManager.AppSettings("Nome")
            strServizio = System.Configuration.ConfigurationManager.AppSettings("Servizio")
            strDefault = System.Configuration.ConfigurationManager.AppSettings("CodiceDefault")

            'RECUPERO DATI HELIOS
            sqlConn.ConnectionString = System.Configuration.ConfigurationManager.AppSettings("cnHelios")
            sqlConn.Open()

            'COntrollo esistenza fascicolo per bando/ente
            strSql = "select CodiceFascicoloPC, IDFascicoloPC, DescrFascicoloPC from BandiAttivitàFascicoli where idbando=" & IdBando & " and idente=" & IdEnte

            'inizializzazione sql command 
            myCommand = New SqlClient.SqlCommand
            myCommand.Connection = sqlConn

            'eseguo la query
            myCommand.CommandText = strSql
            dtrLocal = myCommand.ExecuteReader


            If dtrLocal.HasRows = True Then
                'FASCICOLO ESISTENZA. Procedo alla registrazione in bandiattività dei dati del fascicolo
                dtrLocal.Read()
                Dim strIDFasc As String = dtrLocal("IDFascicoloPC")
                Dim strNumFasc As String = dtrLocal("CodiceFascicoloPC")
                Dim strDescFasc As String = dtrLocal("DescrFascicoloPC")

                'controllo e chiudo se aperto il datareader
                If Not dtrLocal Is Nothing Then
                    dtrLocal.Close()
                    dtrLocal = Nothing
                End If


                strSql = "UPDATE BandiAttività SET  CodiceFascicoloPC ='" & strNumFasc & "', IDFascicoloPC  ='" & strIDFasc & "', DescrFascicoloPC  ='" & strDescFasc & "' where idbando=" & IdBando & " and idente=" & IdEnte

                myCommand.CommandText = strSql
                myCommand.ExecuteNonQuery()
                sqlConn.Close()


            Else
                'controllo e chiudo se aperto il datareader
                If Not dtrLocal Is Nothing Then
                    dtrLocal.Close()
                    dtrLocal = Nothing
                End If

                'FASCICOLO INESISTENTE. Procedo alla generazione del fascicolo
                'estraggo descrizione abbreviata bando , titolario default, codice e denominazione ente
                strSql = "select a.BandoBreve, a.TitolarioProgetti,  c.CodiceRegione, c.Denominazione " & _
                        "from Bando a " & _
                        "inner join bandiattività b on a.idbando = b.idbando " & _
                        "inner join enti c on b.idente = c.idente " & _
                        "where isnull(b.idfascicolopc,'') = '' and b.idbando=" & IdBando & " and b.idente=" & IdEnte

                'inizializzazione sql command 
                myCommand = New SqlClient.SqlCommand
                myCommand.Connection = sqlConn

                'eseguo la query
                myCommand.CommandText = strSql
                dtrLocal = myCommand.ExecuteReader

                'controllo ESISTENZA dati ricevuti
                If dtrLocal.HasRows = True Then
                    dtrLocal.Read()
                    'memorizzo i dati necessari all'elaborazione

                    'strDescrizioneFascicolo = "ISTANZA - " & dtrLocal("CodiceRegione") + " - " + dtrLocal("Denominazione") + " - " + dtrLocal("BandoBreve")
                    strDescrizioneFascicolo = Mid(dtrLocal("BandoBreve") & " - " & dtrLocal("CodiceRegione") + " - " + dtrLocal("Denominazione"), 1, 200)

                    strTitolario = dtrLocal("TitolarioProgetti")

                    'controllo e chiudo se aperto il datareader
                    If Not dtrLocal Is Nothing Then
                        dtrLocal.Close()
                        dtrLocal = Nothing
                    End If
                    'chiudo connessione
                    sqlConn.Close()

                    SIGED_Autenticazione(strNome, strCognome)


                    wsOUT = SIGED_CreaFascicolo(strDescrizioneFascicolo, "", strTitolario, "", "", strServizio, "", strDefault)


                    CreaFascicoloIstanza = SIGED_Codice_Esito(wsOUT.ESITO)

                    If CreaFascicoloIstanza = 0 Then

                        '    'Chiamata alla funzione per la conferma dell' esecuzione del metodo loggato 
                        '    IntLocalIdLog = LogFascicoliConferma(sqllocalconn, IntLocalIdLog)
                        '    'richiamo funzione che ricava il numero fascicolo appena creato e fa l'update in entità-->vecchio webservice
                        '    '** con il nuovo webservice richiamo la funzione per fare l'udate in entità 
                        NormalizzaCodice(wsOUT.CODICEFASCICOLO)

                        sqlConn.ConnectionString = System.Configuration.ConfigurationManager.AppSettings("cnHelios")
                        sqlConn.Open()

                        myCommand.Connection = sqlConn

                        strSql = "INSERT INTO BandiAttivitàFascicoli (IdBAndo,Idente,CodiceFascicoloPC, IDFascicoloPC, DescrFascicoloPC) " & _
                            "VALUES (" & IdBando & " ," & IdEnte & ",'" & strTitolario & "/" & CInt(wsOUT.NUMEROFASCICOLO) & "','" & CodiceNormalizzato & "', '" & strDescrizioneFascicolo.Replace("'", "''") & "')"
                        'INSERT INTO BandiAttivitàFascicoli (IdBAndo,Idente,CodiceFascicoloPC, IDFascicoloPC, DescrFascicoloPC)
                        'VALUES (" & IdBando& " ," & IdEnte & ",'" & "titolario" & "/" & CInt(wsOUT.NUMEROFASCICOLO) & "','" & CodiceNormalizzato & "', "descrizionefascicolo")

                        myCommand.CommandText = strSql
                        myCommand.ExecuteNonQuery()


                        strSql = "UPDATE BandiAttività SET  CodiceFascicoloPC ='" & strTitolario & "/" & CInt(wsOUT.NUMEROFASCICOLO) & "', IDFascicoloPC  ='" & CodiceNormalizzato & "', DescrFascicoloPC  ='" & strDescrizioneFascicolo.Replace("'", "''") & "' where idbando=" & IdBando & " and idente=" & IdEnte

                        myCommand.CommandText = strSql
                        myCommand.ExecuteNonQuery()
                        sqlConn.Close()
                        Return "ok"

                    End If
                Else
                    Return "bando inesistente o fascicolo già associato"
                End If
            End If
        Catch ex As Exception
            Return "errore"
        Finally
            SIGED_Chiudi_Autenticazione(strNome, strCognome)
        End Try

    End Function

    <WebMethod()> _
    Public Function InviaPEC(ByVal CodiceEnte As String, ByVal PecDestinatario As String) As String
        Dim strCognome As String
        Dim strNome As String
        Dim strServizio As String
        Dim strEsito As String = "ok"
        Dim strPathSiged As String
        Dim wsOUT As WS_SIGeD.PROTOCOLLOEX_CREATO
        'Dim wsOUTAllegato As String
        Dim wsCollFascicolo As String
        Dim strSql As String
        Dim sqlConn As New SqlClient.SqlConnection
        Dim myCommand As New SqlClient.SqlCommand
        Dim dtrLocal As SqlClient.SqlDataReader
        Dim strNominativo As String
        Dim strCAP As String
        Dim strComune As String
        Dim strIndirizzo As String
        Dim strProvincia As String
        Dim intidente As Integer = 0

        Try

            strCognome = System.Configuration.ConfigurationManager.AppSettings("Cognome")
            strNome = System.Configuration.ConfigurationManager.AppSettings("Nome")
            strServizio = System.Configuration.ConfigurationManager.AppSettings("Servizio")

            'RECUPERO DATI HELIOS
            sqlConn.ConnectionString = System.Configuration.ConfigurationManager.AppSettings("cnHelios")
            sqlConn.Open()
            strSql = "SELECT  idente, isnull(Denominazione,'') as Denominazione , isnull(Indirizzo,'') as Indirizzo, isnull(Cap,'') as Cap, isnull(Comune,'') as Comune, isnull(ProvinciaBreve,'') as Provincia FROM vw_bo_enti WHERE (CodiceRegione ='" & CodiceEnte & "')"

            'inizializzazione sql command 
            myCommand = New SqlClient.SqlCommand
            myCommand.Connection = sqlConn

            'eseguo la query
            myCommand.CommandText = strSql
            dtrLocal = myCommand.ExecuteReader

            'controllo ESISTENZA dati ricevuti
            If dtrLocal.HasRows = True Then
                dtrLocal.Read()
                'memorizzo i dati necessari all'elaborazione
                strNominativo = "" & dtrLocal("Denominazione")
                strCAP = "" & dtrLocal("Cap")
                strComune = "" & dtrLocal("Comune")
                strIndirizzo = "" & dtrLocal("Indirizzo")
                strProvincia = "" & dtrLocal("Provincia")
                intidente = dtrLocal("idente")
            Else
                strNominativo = "" & "__"
                strCAP = "" & "00000"
                strComune = "" & "__"
                strIndirizzo = "" & "__"
                strProvincia = "" & "__"
            End If

            If strNominativo = "" Then
                strNominativo = "" & "__"
            End If

            If strComune = "" Then
                strComune = "" & "__"
            End If

            If strIndirizzo = "" Then
                strIndirizzo = "" & "__"
            End If
            If strProvincia = "" Then
                strProvincia = "" & "__"
            End If
            If strCAP = "" Then
                strCAP = "" & "00000"
            End If
            'controllo e chiudo se aperto il datareader
            If Not dtrLocal Is Nothing Then
                dtrLocal.Close()
                dtrLocal = Nothing
            End If

            strPathSiged = FileCopy()


            SIGED_Autenticazione(strNome, strCognome)
            'creazione protocolloa
            wsOUT = SIGED_CreaProtocolloExpress(Year(Now), "USCITA", "", strNominativo, strIndirizzo, strComune, strCAP, strProvincia, "", CodiceEnte, Replace(CodiceEnte & " - Notifica adeguamento PEC e firma digitale", "'", "''"), System.Configuration.ConfigurationManager.AppSettings("Servizio"), "Posta elettronica", "4.29.2.1", "", "", "", "", System.Configuration.ConfigurationManager.AppSettings("NomeFilePEC"), "", strPathSiged, "")   '"FASC0001"

            'associazione fascicolo
            wsCollFascicolo = SIGED_CreaCollegamentoFascicolo(System.Configuration.ConfigurationManager.AppSettings("FascicoloPEC"), wsOUT.CODICEPROTOCOLLO)

            'controllo se la creazione del protocollo è andata a buon fine
            If SIGED_Codice_Esito(wsOUT.ESITO) = 0 Then
                Dim WsIntranet As New WsPostProtocolloCAD.ServiceProt
                WsIntranet.InviaProtcolloPEC(strCognome, strNome, wsOUT.DATAPROTOCOLLO, wsOUT.NUMEROPROTOCOLLO, PecDestinatario)

                'Dim strAllegati As String
               ' strAllegati = Allegati(wsOUT.CODICEPROTOCOLLO)
                'SIGED_InvioProtocolloPEC(wsOUT.CODICEPROTOCOLLO, "UNSC PEC", SIGED_ConvertiMultiValore(PecDestinatario), SIGED_ConvertiMultiValore(strAllegati))
                'controllo se l'associazione al fascicolo è andata a buon fine
                If SIGED_Codice_Esito(wsCollFascicolo) <> 0 Then
                    'in caso di errore di associzione al fascicolo, flag ErroreFascicolo =1 
                    strSql = "update Loginviopec set ErroreFascicolo = 1 where idente = " & intidente & " and PEC = '" & PecDestinatario & "'"
                    myCommand.CommandText = strSql
                    myCommand.ExecuteNonQuery()
                End If
            Else
                'in caso di errore di protocollazione, flag ErroreProtocollazione =1 
                strSql = "update Loginviopec set ErroreProtocollazione = 1 where idente = " & intidente & " and PEC = '" & PecDestinatario & "'"
                myCommand.CommandText = strSql
                myCommand.ExecuteNonQuery()
            End If

            'chiudo connessione
            sqlConn.Close()

            Return strEsito
        Catch ex As Exception
            'in caso di errore di protocollazione, flag ErroreProtocollazione =1 
            strSql = "update Loginviopec set ErroreGenerico = 1, DescrizioneErroreGenerico = '" & ex.Message & "' where idente = " & intidente & " and PEC = '" & PecDestinatario & "'"
            myCommand.CommandText = strSql
            myCommand.ExecuteNonQuery()

            Return "errore"
        Finally
            SIGED_Chiudi_Autenticazione(strNome, strCognome)
        End Try

    End Function

    <WebMethod()> _
    Public Function Recupera_DomandaDOL_Protocollo(ByVal Dol_Id As Integer) As String

        Dim Cognome As String
        Dim Nome As String
        Dim CodiceAllegato As String
        Dim AnnoProtocollo As Integer
        Dim NomeFile As String
        Dim PathSiged As String
        Dim PathLocale As String
        Dim CodiceProtocollo As String


        Dim strSql As String
        Dim sqlConn As New SqlClient.SqlConnection
        Dim myCommand As New SqlClient.SqlCommand
        Dim dtrLocal As SqlClient.SqlDataReader

        Dim wsSiged As New WS_SIGeD.SIGED_WS
        Dim wsOUT As New WS_SIGeD.INDICE_ALLEGATO
        'Dim WsDati As WS_SIGeD.ALLEGATO_DOCUMENTO
        Dim dr As DataRow
        Dim riga As Integer

        Try
            'RECUPERO DATI HELIOS
            sqlConn.ConnectionString = System.Configuration.ConfigurationManager.AppSettings("cnHelios")
            sqlConn.Open()
            strSql = "SELECT CodiceProtocollo, year(dataprotocollo) as Anno FROM DOL_DomandePresentate WHERE (id = " & Dol_Id & ")"

            'inizializzazione sql command 
            myCommand = New SqlClient.SqlCommand
            myCommand.Connection = sqlConn

            'eseguo la query
            myCommand.CommandText = strSql
            dtrLocal = myCommand.ExecuteReader

            'controllo ESISTENZA dati ricevuti
            If dtrLocal.HasRows = True Then
                dtrLocal.Read()
                'memorizzo i dati necessari all'elaborazione
                CodiceProtocollo = "" & dtrLocal("CodiceProtocollo")
                AnnoProtocollo = "" & dtrLocal("Anno")
            Else
                Return "errore"
            End If
            If Not dtrLocal Is Nothing Then
                dtrLocal.Close()
                dtrLocal = Nothing
            End If


            Nome = System.Configuration.ConfigurationManager.AppSettings("Nome_AG")
            Cognome = System.Configuration.ConfigurationManager.AppSettings("Cognome_AG")

            SIGED_Autenticazione(Nome, Cognome)

            CodiceAllegato = CodiceProtocollo.Replace("#PROT", "#PROTO") & "#PRINC#0"

            wsOUT = wsSiged.RESTITUISCIALLEGATO(Connessione, AnnoProtocollo, CodiceAllegato, "SI", PathSiged & "\" & NomeFile)
            If Left(wsOUT.ESITO, 5) = "00000" Then
                'WsDati = wsOUT.DATI

                Return wsOUT.DATI.BASE64
                'If File.Exists(PathLocale) = True Then
                '    File.Delete(PathLocale)
                '    'PathSiged = Replace(PathSiged, ".serviziocivile.it", "")
                'End If
                'File.Copy(Trim(PathSiged) & "\" & Trim(NomeFile), Trim(PathLocale))

            End If
        Catch ex As Exception
            Return "errore"
        End Try

    End Function

    Private Function SIGED_Autenticazione(ByVal strNome As String, ByVal strCognome As String) As String
        'funzione per l'autentizacione
        Dim wsAuth As New WS_SIGeD_Auth.SIGED_AUTH
        Dim ws_siged_pwd As String = "webservice"
        Dim strAuth As String
        Try
            strAuth = wsAuth.SWS_NEWSESSION(strNome, strCognome, ws_siged_pwd)

            Codice_Esito = SIGED_Codice_Esito(strAuth)
            Esito = SIGED_Esito(strAuth)
            Connessione = Esito
        Catch ex As Exception
            Esito = "Errore imprevisto: " & ex.Message
        End Try
    End Function

    Private Function SIGED_Codice_Esito(ByVal strCodice As String) As Integer
        'funzione che ricava il codice esito dell'autenticazione
        Try
            Return Left(strCodice, 5)
        Catch ex As Exception
            Esito = "Errore imprevisto: " & ex.Message
        End Try
    End Function

    Private Function SIGED_Esito(ByVal strCodice As String) As String
        'funzione ritorna idsessione se la sessione è andata a buon fine altrimenti ritorna messaggio di errore
        Try
            Return strCodice.Substring(8)
        Catch ex As Exception
            Esito = "Errore imprevisto: " & ex.Message
        End Try
    End Function

    Private Sub SIGED_Chiudi_Autenticazione(ByVal strNome As String, ByVal strCognome As String)
        'funzione che chiude l'autenticazione
        Dim wsAuth As New WS_SIGeD_Auth.SIGED_AUTH

        Try
            wsAuth.Timeout = 300000
            wsAuth.SWS_CLOSESESSION(Connessione, strNome, strCognome)
            Esito = ""
        Catch ex As Exception
            Esito = "Errore imprevisto: " & ex.Message
        End Try
    End Sub

    Private Function SIGED_CreaFascicolo(ByVal Descrizione As String, ByVal Riferimento As String, ByVal CodiceTitolario As String, ByVal Stato As String, ByVal DataApertura As String, ByVal UnitaOrganizzativaResponsabile As String, ByVal Categoria As String, ByVal CodiceDefault As String) As WS_SIGeD.FASCICOLO_CREATO
        'funzione che consente la creazione di un fasciolo
        Dim wsSiged As New WS_SIGeD.SIGED_WS

        Try
            wsSiged.Timeout = 300000
            Return wsSiged.CREAFASCICOLO(Connessione, Descrizione, Riferimento, CodiceTitolario, Stato, DataApertura, UnitaOrganizzativaResponsabile, Categoria, CodiceDefault)
        Catch ex As Exception
            Esito = "Errore imprevisto: " & ex.Message
        End Try
    End Function


    Public Function SIGED_ConvertiMultiValore(ByVal Valore As String) As WS_SIGeD.DATO_MULTI
        'funzione che consente la creazione di un fasciolo
        Dim wsSiged As New WS_SIGeD.SIGED_WS
        Try
            wsSiged.Timeout = 300000
            Return wsSiged.CONVERTIINMULTIVALORE(Valore)

        Catch ex As Exception
            Esito = "Errore imprevisto: " & ex.Message
        End Try
    End Function

    Public Function SIGED_CodiceProtocolloCompleto(ByVal AnnoProtocollo As String, ByVal NumeroProtocollo As String) As String
        'funzione che genera l'idprotocollo , concatenando l'anno #  PROT # numero protocollo
        Dim strCodice As String
        Try
            strCodice = AnnoProtocollo & "#" & "PROT" & "#" & NumeroProtocollo
            Return strCodice
        Catch ex As Exception
            Esito = "Errore imprevisto: " & ex.Message
        End Try
    End Function
    Public Function SIGED_Ricerca_Protocolli(ByVal Anno As String, ByVal NumeroProtocollo As String, ByVal DataProtocollazione As String, ByVal Corrispondente As String, ByVal Oggetto As String, ByVal TipoProtocollo As String, ByVal EstremiProtocollo As String, ByVal ProtocolloRifermento As String, ByVal Stato As String) As WS_SIGeD.MULTI_PROTOCOLLO
        'funzione per la ricerca i fascicoli esistenti
        Dim wsSiged As New WS_SIGeD.SIGED_WS
        Try
            wsSiged.Timeout = 300000
            Return wsSiged.RICERCAPROTOCOLLI(Connessione, Anno, NumeroProtocollo, DataProtocollazione, Corrispondente, Oggetto, TipoProtocollo, EstremiProtocollo, ProtocolloRifermento, Stato)

        Catch ex As Exception
            Esito = "Errore imprevisto: " & ex.Message
        End Try
    End Function

    Private Function NormalizzaCodice(ByVal Codice As String) As String
        'funzione che identifica il codice se fascicolo,protocollo o documento
        'Dim strCodice
        Dim sCod() As String
        'Dim str As String
        Dim i As Integer
        Dim intLen As Integer
        sCod = Split(Codice, "#")

        intLen = sCod.Length

        If intLen = 2 Then
            CodiceNormalizzato = sCod(1)
            'TipoNormalizzato = "DOCU"
        Else
            For i = 0 To UBound(sCod)
                If i = UBound(sCod) Then
                    'TipoNormalizzato = sCod(i - 1)
                    CodiceNormalizzato = sCod(i)
                End If
            Next
        End If

    End Function

    Private Function FileCopy() As String
        'CREATA DA SIMONA CORDELLA 
        'copia il file dalla cartella di origine alla cartella del server
        'il nome del server e la cartella vengono letti dal web.config
        Dim PathServerSiged As String

        PathServerSiged = System.Configuration.ConfigurationManager.AppSettings("PathSiged") & System.Configuration.ConfigurationManager.AppSettings("NomeFilePEC")
        If File.Exists(PathServerSiged) = True Then
            File.Delete(PathServerSiged)
        End If
        File.Copy(Server.MapPath("upload") & "\" & System.Configuration.ConfigurationManager.AppSettings("NomeFilePEC"), PathServerSiged)
        Return PathServerSiged
    End Function

    Private Function SIGED_CreaProtocolloExpress(ByVal Anno As String, ByVal TipoProtocollo As String, ByVal CodAnagrafica As String, ByVal CNominativo As String, ByVal CIndirizzo As String, ByVal CCitta As String, ByVal CCap As String, ByVal CProv As String, ByVal CAzienda As String, ByVal CorrispondenteCodiceUnivoco As String, ByVal Oggetto As String, ByVal Responsabile As String, ByVal TipoDocumento As String, ByVal CodTitolario As String, ByVal Estremi As String, ByVal DataEstr As String, ByVal ProtRif As String, ByVal Allegato As String, ByVal NomeFile As String, ByVal AllB64 As String, ByVal AllegatoPath As String, ByVal CodDefault As String) As WS_SIGeD.PROTOCOLLOEX_CREATO
        'funzione che consente la creazione di un protocollo                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                               
        Dim wsSiged As New WS_SIGeD.SIGED_WS
        Dim wsAnag As New WS_SIGeD.ANAG_MULTI

        Try
            wsSiged.Timeout = 300000
            Return wsSiged.CREAPROTOCOLLOEXPRESS(Connessione, Anno, TipoProtocollo, CodAnagrafica, CNominativo, CIndirizzo, CCitta, CCap, CProv, CAzienda, CorrispondenteCodiceUnivoco, wsAnag, Oggetto, Responsabile, TipoDocumento, CodTitolario, Estremi, DataEstr, ProtRif, Allegato, NomeFile, AllB64, AllegatoPath, CodDefault, "PRINCIPALE")

        Catch ex As Exception
            Esito = "Errore imprevisto: " & ex.Message
        End Try
    End Function

    Private Function SIGED_CreaAllegato(ByVal Anno As Integer, ByVal NumeroProtocollo As String, ByVal DescrDocPrincipale As String, ByVal FlagPricipale As String, ByVal NomeFile As String, ByVal Base64 As String, ByVal Path As String) As String
        'funzione che inserisce gli allegati(indentificato come documento principale o allegato --> FlagPricipale SI o NO ) al protocollo
        Dim wsSiged As New WS_SIGeD.SIGED_WS

        Try
            wsSiged.Timeout = 300000
            Return wsSiged.CREAALLEGATO(Connessione, Anno, NumeroProtocollo, DescrDocPrincipale, FlagPricipale, NomeFile, Base64, Path)

        Catch ex As Exception
            Esito = "Errore imprevisto: " & ex.Message
        End Try
    End Function

    Private Function SIGED_CreaCollegamentoFascicolo(ByVal CodiceFascicolo As String, ByVal CodiceDocumento As String) As String
        'funzione che consente di collegare al fascicolo dei documenti esiStenti (possono essere: fascicolo,protocollo o documenti)
        'vengono specificati dal TIPODOCUMENTO per quanto riguarda il documento non ho ha definizione corretta
        Dim wsSiged As New WS_SIGeD.SIGED_WS
        Dim strTipoDocumento As String
        Try
            wsSiged.Timeout = 300000

            'NormalizzaCodice(CodiceDocumento)


            strTipoDocumento = "PROTOCOLLO"


            Return wsSiged.CREACOLLEGAMENTOFASCICOLO(Connessione, "PROTOCOLLO", CodiceFascicolo, CodiceDocumento, "", "")

        Catch ex As Exception
            Esito = "Errore imprevisto: " & ex.Message
        End Try
    End Function

    Private Function SIGED_InvioProtocolloPEC(ByVal CodiceProtocollo As String, ByVal Account As String, ByVal wsDatoDestinatario As WS_SIGeD.DATO_MULTI, ByVal wsDatoCodAllegato As WS_SIGeD.DATO_MULTI) As WS_SIGeD.INVIAPROTOCOLLOVIAPECResult
        'funzione che consente la creazione di un protocollo                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                               
        Dim wsSiged As New WS_SIGeD.SIGED_WS
        Try
            wsSiged.Timeout = 300000
            Return wsSiged.INVIAPROTOCOLLOVIAPEC(Connessione, CodiceProtocollo, Account, wsDatoDestinatario, wsDatoCodAllegato)

        Catch ex As Exception
            Esito = "Errore imprevisto: " & ex.Message
        End Try
    End Function

    Public Function SIGED_IndiceProtocollo(ByVal CodiceProtocollo As String) As WS_SIGeD.INDICE_PROTOCOLLO
        'funzione che riporta l'elenco dei protocoli di un dato fascicolo
        Dim wsSiged As New WS_SIGeD.SIGED_WS
        Try
            wsSiged.Timeout = 300000
            Return wsSiged.INDICEPROTOCOLLO(Connessione, CodiceProtocollo)

        Catch ex As Exception
            Esito = "Errore imprevisto: " & ex.Message
        End Try
    End Function

    Public Function Allegati(ByVal codiceProt As String) As String
        'Dim Prot As New WS_SIGeD.SIGED_WS
        Dim AllFile As String = ""
        Dim dtProt As New WS_SIGeD.INDICE_PROTOCOLLO
        Dim allProt As WS_SIGeD.ALLEGATO_DOCUMENTO_TROVATO

        Try
            dtProt = SIGED_IndiceProtocollo(codiceProt)
            If SIGED_Codice_Esito(dtProt.ESITO) = 0 Then
                If dtProt.ELENCO_DOCUMENTI.Length > 0 Then
                    For Each allProt In dtProt.ELENCO_DOCUMENTI
                        If AllFile <> "" Then
                            AllFile = AllFile + "@"
                        End If
                        AllFile = AllFile + allProt.CODICEALLEGATO
                    Next allProt
                End If
            Else
                Return "Error"
            End If
            Return AllFile

        Catch ex As Exception
            Return "Error"
        End Try
    End Function
    '------ DA QUI IN POI PER LA CREAZIONE DEL CONTRATTO VOLONTARIO
    <WebMethod()> _
      Public Function UploadContrattoVolontario(ByVal IdVol As Integer, ByVal File As String, ByVal NomeFile As String) As String
        Dim sqlConn As New SqlClient.SqlConnection
        Dim myCommand As New SqlClient.SqlCommand
        Dim dtrLocal As SqlClient.SqlDataReader
        Dim strSql As String

        Dim strCognome As String
        Dim strNome As String
        Dim strServizio As String
        Dim strDefault As String
        Dim strPathSiged As String
        Dim CodiceDocumento As String
        Try
            'RECUPERO INFO WS DA WEBCONFIG
            strCognome = System.Configuration.ConfigurationManager.AppSettings("Cognome_AG")
            strNome = System.Configuration.ConfigurationManager.AppSettings("Nome_AG")
            strServizio = System.Configuration.ConfigurationManager.AppSettings("Servizio_AG")
            strDefault = System.Configuration.ConfigurationManager.AppSettings("CodiceDefault")

            'RECUPERO DATI HELIOS
            sqlConn.ConnectionString = System.Configuration.ConfigurationManager.AppSettings("cnHelios")
            sqlConn.Open()

            'estraggo descrizione abbreviata bando , titolario default, codice e denominazione ente
            strSql = " SELECT IDEntità, Cognome + ' '+ Nome + ' (' + CodiceVolontario + ')' as Descrizione, CodiceFascicolo, IDFascicolo, DescrFascicolo " & _
                     " FROM  entità " & _
                     " WHERE IDEntità = " & IdVol

            'inizializzazione sql command 
            myCommand = New SqlClient.SqlCommand
            myCommand.Connection = sqlConn

            'eseguo la query
            myCommand.CommandText = strSql
            dtrLocal = myCommand.ExecuteReader

            ''controllo ESISTENZA dati ricevuti
            If dtrLocal.HasRows = True Then
                dtrLocal.Read()
                'memorizzo i dati necessari all'elaborazione

                Dim idFasc As String = dtrLocal("IDFascicolo")
                Dim Descrizione As String = "Contratto " & dtrLocal("Descrizione")
                'controllo e chiudo se aperto il datareader
                If Not dtrLocal Is Nothing Then
                    dtrLocal.Close()
                    dtrLocal = Nothing
                End If
                'chiudo connessione
                sqlConn.Close()


                strPathSiged = FileBase64ToString(File, NomeFile)

                SIGED_Autenticazione(strNome, strCognome)

                Dim IdFascicolo As String = SIGED_IdFascicoloCompleto(idFasc)
                NomeFile = "Contratto_" & NomeFile
                Dim wsDoc As String = SIGED_CreaDocumentoInterno(IdFascicolo, Descrizione, NomeFile, "", strPathSiged)
                If CInt(Left(wsDoc, 5)) = 0 Then
                    CodiceDocumento = wsDoc.Substring(8)
                Else
                    CodiceDocumento = "ERRORE"
                End If
            Else
                CodiceDocumento = "ERRORE"
            End If
            Return CodiceDocumento

        Catch ex As Exception
            Return "errore"
        Finally
            SIGED_Chiudi_Autenticazione(strNome, strCognome)
        End Try

    End Function
    Function FileBase64ToString(ByVal srcFile As String, ByVal nomeFile As String) As String
        'converte il file da base 64 a stringa
        Dim srcBT As Byte()
        Dim destFile As String

        srcBT = System.Convert.FromBase64String(srcFile)
        destFile = System.Configuration.ConfigurationManager.AppSettings("PathSiged") & nomeFile

        If IO.File.Exists(destFile) Then
            IO.File.Delete(destFile)
        End If

        Dim sw As New IO.FileStream(destFile, IO.FileMode.Create)

        sw.Write(srcBT, 0, srcBT.Length)

        sw.Close()

        Return destFile

    End Function
    Public Function SIGED_IdFascicoloCompleto(ByVal IdFascicolo As String) As String
        'funzione che genera l'idprotocollo , concatenando l'anno #  PROT # numero protocollo
        Dim strCodice As String
        Try
            strCodice = Right(IdFascicolo, 4) & "#" & "FASC" & "#" & IdFascicolo
            Return strCodice
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Function

    Public Function SIGED_CreaDocumentoInterno(ByVal CodiceFascicolo As String, ByVal Descrizione As String, ByVal NomeFile As String, ByVal Base64 As String, ByVal Path As String) As String
        'funzione che consente la creazione di un fascioloù

        'Dim wsSiged As New WS_SIGeD.SIGeD_WS_service
        Dim wsSiged As New WS_SIGeD.SIGED_WS
        Try
            'wsSiged.Timeout = 300000
            Return wsSiged.CREADOCUMENTOINTERNO(Connessione, CodiceFascicolo, Descrizione, NomeFile, Base64, Path)
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Function
    
    '<WebMethod()> _
    'Public Function PROVVISORIO_InviaPEC()
    '    SIGED_Autenticazione("Laura", "Pochesci")
    '    Dim strAllegati As String
    '    strAllegati = "2013#PROTO#7#PRINC#0"
    '    SIGED_InvioProtocolloPEC("2013#PROT#7", "UNSC PEC", SIGED_ConvertiMultiValore("diprus@pec.governo.it|usg@mailbox.governo.it"), SIGED_ConvertiMultiValore(strAllegati))
    'End Function

#Region "Creazione_Associazione_FascicoliVolontario"

    <WebMethod()> _
    Public Function GeneraFascicoloVolontari(ByVal strUserName As String, ByVal strIdEntita As String, ByVal IntIdLog As Integer) As String
        Dim wsOUT As WS_SIGeD.FASCICOLO_CREATO
        Dim sqlConn As New SqlClient.SqlConnection
        Dim myCommand As New SqlClient.SqlCommand
        Dim dtrLocal As SqlClient.SqlDataReader
        Dim strSql As String

        Dim strDescrizioneFascicolo As String
        Dim strTitolario As String
        ' Dim strCognome As String
        ' Dim strNome As String
        Dim strServizio As String
        ' Dim strDefault As String

        '  Dim wsOUT As New WS_SIGeD.MULTI_FASCICOLO_CREATO
        Dim IntLocalIdLog As Integer
        ' Dim strSQL As String
        ' Dim DataSetRicerca As DataSet
        'Dim strDescrizioneFascicolo As String
        Dim strNomeUtente As String
        Dim strCognomeUtente As String
        ' Dim strTitolario As String
        Dim strAppoTitolario As String
        Dim dtrLeggiDati As SqlClient.SqlDataReader
        '  Dim CmdGenerico As SqlClient.SqlCommand
        Dim strdefaultfascicolo As String
        Dim blnErroreFascicolo As Boolean = False
        ' Dim strServizio As String
        Try

            'RECUPERO DATI HELIOS
            sqlConn.ConnectionString = System.Configuration.ConfigurationManager.AppSettings("cnHelios")
            sqlConn.Open()

            myCommand = New SqlClient.SqlCommand
            myCommand.Connection = sqlConn

            strSql = " Select u.Nome, u.Cognome, isnull(s.descrizione,'')as descrizione " & _
                         " From UtentiUNSC u LEFT JOIN TServiziSiged  s on  u.idservizio = s.idservizio " & _
                         " Where u.UserName='" & strUserName & "'"


            'inizializzazione sql command 


            'eseguo la query
            myCommand.CommandText = strSql
            dtrLocal = myCommand.ExecuteReader

            dtrLocal.Read()

            strNomeUtente = dtrLocal("Nome")
            strCognomeUtente = dtrLocal("Cognome")
            strServizio = dtrLocal("descrizione")

            dtrLocal.Close()
            dtrLocal = Nothing

            Dim strAppoIdEntita() As String
            Dim strIDEnt As String
            Dim intLen As Integer
            Dim vDescrizione As String
            Dim vIdEntita As String
            Dim strIdFascicolo As String
            Dim strNumFascicolo As String
            Dim bnlErrore As Boolean = False
            Dim dtVolontario As DataTable
            Dim myRow As DataRow
            'Chiamata alla funzione per la scrittura del log
            IntLocalIdLog = LogFascicoliScrivi(sqlConn, IntIdLog, strUserName, "wsAuth.SWS_NEWSESSION", "@FIRSTNAME=" & strNomeUtente & ", @LASTNAME=" & strCognomeUtente & ", @PASSWORD=xxxxxx")


            SIGED_Autenticazione(strNomeUtente, strCognomeUtente)

            'Chiamata alla funzione per la conferma dell' esecuzione del metodo loggato 
            IntLocalIdLog = LogFascicoliConferma(sqlConn, IntLocalIdLog)

            strAppoIdEntita = Split(strIdEntita, "#")

            intLen = strAppoIdEntita.Length

            'If intLen = 1 Then
            '    strIDEnt = strAppoIdEntita(1)

            'Else
            For i = 0 To UBound(strAppoIdEntita)
                strIDEnt = strAppoIdEntita(i)

                strSql = "SELECT isnull(bando.Titolario, '') as Titolario, entità.identità as IdVolontario, entità.Nome, entità.Cognome, isnull(entità.CodiceVolontario,'') as CodiceVolontario, isnull(defaultfascicolo,'') as defaultfascicolo "
                strSql = strSql & "FROM entità INNER JOIN "
                strSql = strSql & "attivitàentità ON entità.IDEntità = attivitàentità.IDEntità INNER JOIN "
                strSql = strSql & "attivitàentisediattuazione ON attivitàentità.IDAttivitàEnteSedeAttuazione = attivitàentisediattuazione.IDAttivitàEnteSedeAttuazione INNER JOIN "
                strSql = strSql & "attività ON attivitàentisediattuazione.IDAttività = attività.IDAttività INNER JOIN "
                strSql = strSql & "BandiAttività ON attività.IDBandoAttività = BandiAttività.IdBandoAttività INNER JOIN "
                strSql = strSql & "bando ON BandiAttività.IdBando = bando.IDBando "
                strSql = strSql & "Where entità.IdEntità = " & strIDEnt & " and isnull(entità.CodiceFascicolo,'') = '' "

                'DataSetRicerca = ClsServer.DataSetGenerico(strSQL, sqllocalconn)

                'chiudo il datareader
                If Not dtrLeggiDati Is Nothing Then
                    dtrLeggiDati.Close()
                    dtrLeggiDati = Nothing
                End If

                'eseguo la query
                myCommand.CommandText = strSql
                dtrLeggiDati = myCommand.ExecuteReader

                If dtrLeggiDati.HasRows = True Then
                    dtrLeggiDati.Read()


                    vDescrizione = dtrLeggiDati("Cognome") & " " & dtrLeggiDati("Nome") & " (" & dtrLeggiDati("CodiceVolontario") & ")"
                    vIdEntita = dtrLeggiDati("IdVolontario")
                    strDescrizioneFascicolo = dtrLeggiDati("Cognome") & " " & dtrLeggiDati("Nome") & " (" & dtrLeggiDati("CodiceVolontario") & ")"
                    strTitolario = dtrLeggiDati("Titolario")
                    strdefaultfascicolo = dtrLeggiDati("defaultfascicolo")

                    If Not dtrLeggiDati Is Nothing Then
                        dtrLeggiDati.Close()
                        dtrLeggiDati = Nothing
                    End If

                    If strTitolario <> "" Then
                        strAppoTitolario = Mid(strTitolario, 1, InStr(strTitolario, "-") - 1)
                        strAppoTitolario = Replace(strAppoTitolario, " ", "")

                        strTitolario = strAppoTitolario
                    End If


                    IntLocalIdLog = LogFascicoliScrivi(sqlConn, IntIdLog, strUserName, "wsSiged.CREAFASCICOLO", "@SESSIONE=" & Connessione & ", @DESCRIZIONE=" & Replace(strDescrizioneFascicolo, "'", "''") & ", @RIFERIMENTO=, @CODICETITOLARIO=" & strTitolario & ", @STATO=, @DATAAPERTURA=, @UNITAORGANIZZATIVARESPONSABILE=" & strServizio & ", @CATEGORIA=, @CODICEDEFAULT=" & strdefaultfascicolo)

                    wsOUT = SIGED_CreaFascicolo(Replace(strDescrizioneFascicolo, "'", "''"), "", strTitolario, "", "", strServizio, "", strdefaultfascicolo)

                    GeneraFascicoloVolontari = SIGED_Codice_Esito(wsOUT.ESITO)

                    If GeneraFascicoloVolontari = 0 Then

                        'Chiamata alla funzione per la conferma dell' esecuzione del metodo loggato 
                        IntLocalIdLog = LogFascicoliConferma(sqlConn, IntLocalIdLog)
                        'richiamo funzione che ricava il numero fascicolo appena creato e fa l'update in entità-->vecchio webservice
                        '** con il nuovo webservice richiamo la funzione per fare l'udate in entità 
                        NormalizzaCodice(wsOUT.CODICEFASCICOLO)
                        strIdFascicolo = CodiceNormalizzato
                        strNumFascicolo = strTitolario & "/" & CInt(wsOUT.NUMEROFASCICOLO)

                        SalvaFascicolo(strUserName, strIdFascicolo, strNumFascicolo, strDescrizioneFascicolo, strIDEnt, sqlConn)

                        strSql = "select idLogProtocolliVolontari,IdEntità,parametri from LogProtocolliVolontari where Eseguito = 0 and identità= " & strIDEnt
                        dtVolontario = CreaDataTable(strSql, False, sqlConn)
                        For Each myRow In dtVolontario.Rows
                            AssociaProtocolliVolontari(strUserName, myRow.Item("idLogProtocolliVolontari"), strIDEnt, strIdFascicolo, myRow.Item("parametri"))
                        Next
                    Else
                        'gestire errore creazione fascicolo
                        bnlErrore = True
                    End If
                End If
            Next

            'chiudo il datareader
            If Not dtrLeggiDati Is Nothing Then
                dtrLeggiDati.Close()
                dtrLeggiDati = Nothing
            End If


            If bnlErrore = False Then
                strSql = "update LogFascicoliVolontari set DataOraEsecuzione = getdate(), Eseguito=1 where IdLogFascicoliVolontari = " & IntIdLog
                myCommand.CommandText = strSql
                myCommand.ExecuteNonQuery()
            End If

            GeneraFascicoloVolontari = "Fine creazione fascicoli"
            sqlConn.Close()

            Return GeneraFascicoloVolontari

        Catch ex As Exception
            sqlConn.Close()
            GeneraFascicoloVolontari = ex.Message
            'DataSetRicerca.Dispose()
        Finally

            SIGED_Chiudi_Autenticazione(strNomeUtente, strCognomeUtente)

        End Try
    End Function
    Public Shared Function LogFascicoliScrivi(ByVal sqllocalconn As SqlClient.SqlConnection, ByVal IntIdLog As Integer, ByVal strUserName As String, ByVal StrMetodo As String, ByVal StrParametri As String) As Integer
        Dim strSql As String
        Dim myCommand As System.Data.SqlClient.SqlCommand
        Dim dtrLeggiDati As SqlClient.SqlDataReader

        'loggo su logfascicolivolontariDett
        strSql = "INSERT INTO LogFascicoliVolontariDett(idlogfascicolivolontari,[Username],[Metodo],parametri,[DataOraRichiesta],[DataOraEsecuzione],[Eseguito])"
        strSql = strSql & " VALUES(" & IntIdLog & ",'" & strUserName & "','" & Replace(StrMetodo, "'", "''") & "','" & Replace(StrParametri, "'", "''") & "',getdate(),NULL,0)"
        myCommand = New System.Data.SqlClient.SqlCommand
        myCommand.Connection = sqllocalconn

        myCommand.CommandText = strSql
        myCommand.ExecuteNonQuery()

        '---recupero l'id appena inserito in logfascicolivolontariDett
        Dim strID As String
        strSql = "select @@identity as Id"
        myCommand.CommandText = strSql
        dtrLeggiDati = myCommand.ExecuteReader


        'dtrLeggiDati = ClsServer.CreaDatareader(strSql, sqllocalconn)
        dtrLeggiDati.Read()
        strID = dtrLeggiDati("Id")
        dtrLeggiDati.Close()
        dtrLeggiDati = Nothing

        LogFascicoliScrivi = strID
        'Return strID
        If Not dtrLeggiDati Is Nothing Then
            dtrLeggiDati.Close()
            dtrLeggiDati = Nothing
        End If
    End Function
    Public Shared Function LogFascicoliConferma(ByVal sqllocalconn As SqlClient.SqlConnection, ByVal idLog As Integer) As Integer
        Dim strSql As String
        Dim myCommand As System.Data.SqlClient.SqlCommand
        strSql = "update LogFascicoliVolontariDett set DataOraEsecuzione = getdate(), Eseguito=1 where IdLogFascicoliVolontariDett = " & idLog
        myCommand = New System.Data.SqlClient.SqlCommand
        myCommand.Connection = sqllocalconn
        myCommand.CommandText = strSql
        myCommand.ExecuteNonQuery()

    End Function

    Public Shared Function SalvaFascicolo(ByVal strUserName As String, ByVal IdFascicolo As String, ByVal NumFascicolo As String, ByVal DescrizioneFascicolo As String, ByVal strIdEntita As String, ByVal sqllocalconn As SqlClient.SqlConnection) As String



        Dim myCommand As System.Data.SqlClient.SqlCommand
        Dim strSQL As String

        Dim strNumeroFascicolo As String
        Dim strIdFascicolo As String

        Try


            strNumeroFascicolo = NumFascicolo
            strIdFascicolo = IdFascicolo
            strSQL = " Update entità set CodiceFascicolo= '" & strNumeroFascicolo & "', " & _
                     " IDFascicolo ='" & strIdFascicolo & "', " & _
                     " DescrFascicolo = '" & Replace(DescrizioneFascicolo, "'", "''") & "' " & _
                     " where identità=" & strIdEntita & ""
            myCommand = New System.Data.SqlClient.SqlCommand
            myCommand.Connection = sqllocalconn
            myCommand.CommandText = strSQL
            myCommand.ExecuteNonQuery()

        Catch ex As Exception
            SalvaFascicolo = ex.Message

        End Try
    End Function
#End Region

#Region "Creazione_Associazione_ProtocoloVolontario"
    '<WebMethod()> _
    Private Function AssociaProtocolliVolontari(ByVal strUserName As String, ByVal idLogProtocolliVolontari As Integer, ByVal sIdEntita As String, ByVal StrIDFascicolo As String, ByVal IdProtocolloSIGED As String) As String
        Dim sqlConn As New SqlClient.SqlConnection
        Dim myCommand As New SqlClient.SqlCommand
        Dim dtrLocal As SqlClient.SqlDataReader
        Dim strSql As String
        Dim i As Integer = 0
        Dim chkSelVol As CheckBox

        Dim wsOUT As String
        Dim wsProt As WS_SIGeD.MULTI_PROTOCOLLO
        Dim strIdFasc As String 'idfascicolo
        Dim IdFascSIGED As String 'idfascicolo
        ' Dim IdProtocolloSIGED As String

        Dim strNomeUtente As String
        Dim strCognomeUtente As String

        Dim dsUser As DataSet
        Dim intEsito As Integer = 0
        Dim intConta As Integer = 0
        Dim IntLocalIdLog As Integer
        Dim strIDEntita As String
        Try

            'RECUPERO DATI HELIOS
            sqlConn.ConnectionString = System.Configuration.ConfigurationManager.AppSettings("cnHelios")
            sqlConn.Open()

            myCommand = New SqlClient.SqlCommand
            myCommand.Connection = sqlConn

            'strSql = " Select u.Nome, u.Cognome, isnull(s.descrizione,'')as descrizione " & _
            '             " From UtentiUNSC u LEFT JOIN TServiziSiged  s on  u.idservizio = s.idservizio " & _
            '             " Where u.UserName='" & strUserName & "'"


            ''inizializzazione sql command 


            ''eseguo la query
            'myCommand.CommandText = strSql
            'dtrLocal = myCommand.ExecuteReader

            'dtrLocal.Read()

            'strNomeUtente = dtrLocal("Nome")
            'strCognomeUtente = dtrLocal("Cognome")


            'dtrLocal.Close()
            'dtrLocal = Nothing



            'Chiamata alla funzione per la conferma dell' esecuzione del metodo loggato 
            IntLocalIdLog = LogProtocolliConferma(sqlConn, idLogProtocolliVolontari, 1, "") 'in lavorazione

            IdFascSIGED = SIGED_IdFascicoloCompleto(StrIDFascicolo)

            wsOUT = SIGED_CreaCollegamentoFascicolo(IdFascSIGED, IdProtocolloSIGED)


            If Left(wsOUT, 5) = "00000" Then ' da verifica in uscita che tipo di esito esce
                IntLocalIdLog = LogProtocolliConferma(sqlConn, idLogProtocolliVolontari, 2, "") 'eseguito
            Else
                If InStr(SIGED_Esito(wsOUT), "PRIMARY KEY") <> 0 Then
                    IntLocalIdLog = LogProtocolliConferma(sqlConn, idLogProtocolliVolontari, 2, "Esistente") 'non eseguito perchè esistente
                Else
                    IntLocalIdLog = LogProtocolliConferma(sqlConn, idLogProtocolliVolontari, 0, SIGED_Esito(wsOUT)) 'non eseguito altro errore
                End If

            End If

            ''Chiamata alla funzione per la scrittura del log
            'IntLocalIdLog = LogProtocolliScrivi(sqlConn, IntIdLog, strUserName, "wsAuth.SWS_NEWSESSION", "@FIRSTNAME=" & strNomeUtente & ", @LASTNAME=" & strCognomeUtente & ", @PASSWORD=xxxxxx")

            'SIGED_Autenticazione(strNomeUtente, strCognomeUtente)

            ''           'Chiamata alla funzione per la conferma dell' esecuzione del metodo loggato 
            ''            IntLocalIdLog = LogFascicoliConferma(sqlConn, IntLocalIdLog, 1)

            'IdProtocolloSIGED = SIGED_CodiceProtocolloCompleto(AnnoProtocollo, NumeroProtocollo)

            'wsProt = SIGED_Ricerca_Protocolli(AnnoProtocollo, NumeroProtocollo, "", "", "", "", "", "", "")

            ''If SIGED_Esito(wsProt.ESITO) = 0 Then
            ''    'lblmsg.Text = "Il protocollo indicato è inesistente."
            ''    SIGED_Chiudi_Autenticazione(strNomeUtente, strCognomeUtente)
            ''End If
            ''leggo la griglia dei volontari per l'associazione del protocollo al fascicolo
            'For i = 0 To UBound(VIdEntita)
            '    strIDEntita = VIdEntita(i)
            '    If VerificaEsistenzaFascicoloVolontario(strIDEntita) = True Then
            '        'Chiamata alla funzione per la conferma dell' esecuzione del metodo loggato 
            '        IntLocalIdLog = LogProtocolliConferma(sqlConn, IntLocalIdLog, 1) 'in lavorazione

            '        intConta = intConta + 1
            '        ' strIdFasc = dtgVolontari.Items(i).Cells(7).Text 'idfascicolo
            '         IdFascSIGED = SIGED_IdFascicoloCompleto(strIDEntita)

            '        wsOUT = SIGED_CreaCollegamentoFascicolo(IdFascSIGED, IdProtocolloSIGED)


            '        If InStr(SIGED_Esito(wsOUT), "PRIMARY KEY") <> 0 Then
            '            intEsito = intEsito + 1
            '        End If
            '        IntLocalIdLog = LogProtocolliConferma(sqlConn, IntLocalIdLog, 2) 'eseguito
            '    End If

            'Next

            ''If SIGED.SIGED_Codice_Esito(wsOUT) = 0 Then
            'If intEsito <> intConta Then
            '    ' lblmsg.Text = "Aggiornamento eseguito"
            'Else
            '    'lblmsg.Text = "Protocollo già esistente."
            'End If

            'SIGED_Chiudi_Autenticazione(strNomeUtente, strCognomeUtente) 'rem Danilo


        Catch ex As Exception
            ' GeneraProtocolliVolontari = ex.Message
        Finally

            'SIGED_Chiudi_Autenticazione(strNomeUtente, strCognomeUtente) 'rem Danilo
            sqlConn.Close()

        End Try
    End Function

    'Public Shared Function LogProtocolliScrivi(ByVal sqllocalconn As SqlClient.SqlConnection, ByVal IntIdLog As Integer, ByVal strUserName As String, ByVal StrMetodo As String, ByVal StrParametri As String) As Integer
    '    Dim strSql As String
    '    Dim myCommand As System.Data.SqlClient.SqlCommand
    '    Dim dtrLeggiDati As SqlClient.SqlDataReader



    '    'loggo su logfascicolivolontariDett
    '    strSql = "INSERT INTO LogProtocolliVolontariDett(IdLogProtocolliVolontari,[Username],[Metodo],parametri,[DataOraRichiesta],[DataOraEsecuzione],[Eseguito])"
    '    strSql = strSql & " VALUES(" & IntIdLog & ",'" & strUserName & "','" & Replace(StrMetodo, "'", "''") & "','" & Replace(StrParametri, "'", "''") & "',getdate(),NULL,1)"
    '    myCommand = New System.Data.SqlClient.SqlCommand
    '    myCommand.Connection = sqllocalconn

    '    myCommand.CommandText = strSql
    '    myCommand.ExecuteNonQuery()

    '    '---recupero l'id appena inserito in logfascicolivolontariDett
    '    Dim strID As String
    '    strSql = "select @@identity as Id"
    '    myCommand.CommandText = strSql
    '    dtrLeggiDati = myCommand.ExecuteReader


    '    'dtrLeggiDati = ClsServer.CreaDatareader(strSql, sqllocalconn)
    '    dtrLeggiDati.Read()
    '    strID = dtrLeggiDati("Id")
    '    dtrLeggiDati.Close()
    '    dtrLeggiDati = Nothing

    '    LogProtocolliScrivi = strID
    '    'Return strID
    '    If Not dtrLeggiDati Is Nothing Then
    '        dtrLeggiDati.Close()
    '        dtrLeggiDati = Nothing
    '    End If
    'End Function

    Public Shared Function LogProtocolliConferma(ByVal sqllocalconn As SqlClient.SqlConnection, ByVal idLogP As Integer, ByVal Eseguito As Integer, ByVal strErrore As String) As Integer
        'Eseguito 1 in lavorazione 2 eseguito
        Dim strSql As String
        Dim myCommand As System.Data.SqlClient.SqlCommand
        strSql = "update LogProtocolliVolontari set metodo = metodo + '_WS', DataOraEsecuzione = getdate(), Eseguito=" & Eseguito & ", errore = '" & strErrore.Replace("'", "''") & "' where  IdLogProtocolliVolontari = " & idLogP
        myCommand = New System.Data.SqlClient.SqlCommand
        myCommand.Connection = sqllocalconn
        myCommand.CommandText = strSql
        myCommand.ExecuteNonQuery()

    End Function

    'Private Function VerificoLogProtocolliSospesi(ByVal IdEntità As Integer) As Boolean
    '    'CONTROLLO SE IL VOLONTARIO HA IL FASCICOLO 
    '    Dim strSQL As String
    '    Dim dtrVol As SqlClient.SqlDataReader
    '    Dim myCommand As System.Data.SqlClient.SqlCommand

    '    If Not dtrVol Is Nothing Then
    '        dtrVol.Close()
    '        dtrVol = Nothing
    '    End If
    '    strSQL = "select IdEntità,parametri from LogProtocolliVolontari where Eseguito = 0"

    '    myCommand.CommandText = strSQL
    '    dtrVol = myCommand.ExecuteReader


    '    VerificoLogProtocolliSospesi = dtrVol.HasRows

    '    If Not dtrVol Is Nothing Then
    '        dtrVol.Close()
    '        dtrVol = Nothing
    '    End If
    '    Return VerificoLogProtocolliSospesi
    'End Function


    Private Function VerificaEsistenzaFascicoloVolontario(ByVal IdEntità As Integer) As Boolean
        'CONTROLLO SE IL VOLONTARIO HA IL FASCICOLO 
        Dim strSQL As String
        Dim dtrVol As SqlClient.SqlDataReader
        Dim myCommand As System.Data.SqlClient.SqlCommand

        If Not dtrVol Is Nothing Then
            dtrVol.Close()
            dtrVol = Nothing
        End If
        strSQL = "Select CodiceFasciolo from Entità where idEntità=" & IdEntità & " and isnull(CodiceFascicolo,'') <>''"
        myCommand.CommandText = strSQL
        dtrVol = myCommand.ExecuteReader


        VerificaEsistenzaFascicoloVolontario = dtrVol.HasRows

        If Not dtrVol Is Nothing Then
            dtrVol.Close()
            dtrVol = Nothing
        End If
        Return VerificaEsistenzaFascicoloVolontario
    End Function

#End Region

#Region "UtilityWebService"

    Private Function CreaDatareader(ByVal StrSqlGrid As String, ByVal conn As SqlConnection, Optional ByVal CheckOperazione As Integer = 0, Optional ByVal transazione As System.Data.SqlClient.SqlTransaction = Nothing) As SqlDataReader 'riempe la griglia passatagli
        Dim comando As SqlCommand
        Try
            If transazione Is Nothing Then
                comando = New SqlCommand(StrSqlGrid, conn)
            Else
                comando = New SqlCommand(StrSqlGrid, conn, transazione)
            End If
            comando.CommandTimeout = 300
            CreaDatareader = comando.ExecuteReader()
            'connessione.Close()
        Catch es As Exception
            Throw New Exception(es.Message.ToString)
        End Try
    End Function
    Private Function CreaDataTable(ByVal Query As String, ByVal FirstrowBlank As Boolean, ByVal conn As SqlConnection) As System.Data.DataTable
        Dim MyTable As New System.Data.DataTable
        Dim MyRow As DataRow
        Dim Rows As DataRowCollection
        Dim DtrGenerico As System.Data.SqlClient.SqlDataReader

        DtrGenerico = CreaDatareader(Query, conn)
        Rows = DtrGenerico.GetSchemaTable.Rows

        For Each MyRow In Rows
            Dim MyCol As New DataColumn
            MyCol.ColumnName = MyRow("ColumnName").ToString
            MyCol.Unique = System.Convert.ToBoolean(MyRow("IsUnique"))
            MyCol.AllowDBNull = System.Convert.ToBoolean(MyRow("AllowDBNull"))
            MyCol.ReadOnly = System.Convert.ToBoolean(MyRow("IsReadOnly"))
            MyCol.DataType = Type.GetType(MyRow("DataType").ToString)
            MyTable.Columns.Add(MyCol)
        Next

        If FirstrowBlank = True Then
            Dim MyCol As DataColumn
            MyRow = MyTable.NewRow
            For Each MyCol In MyTable.Columns
                If MyCol.DataType.ToString = "System.Int32" Then
                    MyRow(MyCol) = 0
                Else
                    MyRow(MyCol) = ""
                End If
            Next
            MyTable.Rows.Add(MyRow)
        End If

        While DtrGenerico.Read
            Dim MyCol As DataColumn
            MyRow = MyTable.NewRow
            For Each MyCol In MyTable.Columns
                MyRow(MyCol) = DtrGenerico(MyCol.ColumnName)
            Next
            MyTable.Rows.Add(MyRow)
        End While
        DtrGenerico.Close()
        DtrGenerico = Nothing
        Return MyTable
    End Function

#End Region
End Class