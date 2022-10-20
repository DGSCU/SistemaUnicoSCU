Imports System.Object
Imports System.Web.UI.Page
Imports System.Text.RegularExpressions
Imports System.Text
Imports System
Imports System.Net
Imports System.IO
Imports System.Web.Mail
Imports System.Security.Cryptography
Imports System.Data.SqlClient


Public Class ClsUtility

    Public Shared Function TrovaTitolario(ByVal intIdAttivita As Integer, ByVal sqllocalconn As SqlClient.SqlConnection) As Boolean
        Dim strsql As String
        Dim dtrLeggiDati As SqlClient.SqlDataReader

        'chiudo il datareader
        If Not dtrLeggiDati Is Nothing Then
            dtrLeggiDati.Close()
            dtrLeggiDati = Nothing
        End If

        strsql = "SELECT isnull(bando.Titolario, '') as Titolario "
        strsql = strsql & "FROM attivit‡ "
        strsql = strsql & "INNER JOIN BandiAttivit‡ ON attivit‡.IDBandoAttivit‡ = BandiAttivit‡.IdBandoAttivit‡ INNER JOIN "
        strsql = strsql & "bando ON BandiAttivit‡.IdBando = bando.IDBando "
        strsql = strsql & "Where attivit‡.IdAttivit‡=" & intIdAttivita & " "
        strsql = strsql & "and bando.Titolario<>''"

        'prendo la data dal server
        dtrLeggiDati = ClsServer.CreaDatareader(strsql, sqllocalconn)

        TrovaTitolario = dtrLeggiDati.HasRows

        'chiudo il datareader
        If Not dtrLeggiDati Is Nothing Then
            dtrLeggiDati.Close()
            dtrLeggiDati = Nothing
        End If

        Return TrovaTitolario

    End Function
    Public Shared Function SICUREZZA_VERIFICA_AUTORIZZAZIONI(ByVal conn As SqlClient.SqlConnection, ByVal strIDENTE As Integer, ByVal strCODICEENTE As String, ByVal strATTIVITA As Integer, ByVal strBANDOATTIVITA As Integer, ByVal strENTEPERSONALE As Integer, ByVal strENTITA As Integer, ByVal strENTE As Integer, Optional ByVal strPROGRAMMA As Integer = -1, Optional ByVal strBANDOPROGRAMMA As Integer = -1) As Integer
        'AUTORE: Antonello Di Croce  ---------------------------SANDOKAN--------
        'DESCRIZIONE: richiamo la SP_SICUREZZA_VERIFICA_AUTORIZZAZIONI per la verifica sulla validit‡ delle autorizzazioni del ente  
        'DATA: 04/07/2013
        Dim Reader As SqlClient.SqlDataReader
        Dim x As Integer
        Try


            Dim MyCommand As New SqlClient.SqlCommand
            MyCommand.CommandType = CommandType.StoredProcedure
            MyCommand.CommandText = "SP_SICUREZZA_VERIFICA_AUTORIZZAZIONI"
            MyCommand.Connection = conn

            'PRIMO PARAMETRO A=SESSION_IDENTE INPUT
            Dim sparam As SqlClient.SqlParameter
            sparam = New SqlClient.SqlParameter
            sparam.ParameterName = "@SESSION_IDENTE"
            sparam.SqlDbType = SqlDbType.Int
            MyCommand.Parameters.Add(sparam)

            'SECONDO PARAMETRO B=@SESSION_CODICEENTE INPUT
            Dim sparam1 As SqlClient.SqlParameter
            sparam1 = New SqlClient.SqlParameter
            sparam1.ParameterName = "@SESSION_CODICEENTE"
            sparam1.SqlDbType = SqlDbType.NVarChar
            MyCommand.Parameters.Add(sparam1)



            'TERZO PARAMETRO C=@IDATTIVITA INPUT
            Dim sparam2 As SqlClient.SqlParameter
            sparam2 = New SqlClient.SqlParameter
            sparam2.ParameterName = "@IDATTIVITA"
            sparam2.SqlDbType = SqlDbType.Int
            MyCommand.Parameters.Add(sparam2)

            'QUARTO PARAMETRO D=@IDBANDOATTIVITA INPUT
            Dim sparam3 As SqlClient.SqlParameter
            sparam3 = New SqlClient.SqlParameter
            sparam3.ParameterName = "@IDBANDOATTIVITA"
            sparam3.SqlDbType = SqlDbType.Int
            MyCommand.Parameters.Add(sparam3)

            'QUINTO PARAMETRO E=@IDENTEPERSONALE INPUT
            Dim sparam4 As SqlClient.SqlParameter
            sparam4 = New SqlClient.SqlParameter
            sparam4.ParameterName = "@IDENTEPERSONALE"
            sparam4.SqlDbType = SqlDbType.NVarChar
            MyCommand.Parameters.Add(sparam4)

            'SESTO PARAMETRO E=@IDENTITA INPUT
            Dim sparam5 As SqlClient.SqlParameter
            sparam5 = New SqlClient.SqlParameter
            sparam5.ParameterName = "@IDENTITA"
            sparam5.SqlDbType = SqlDbType.Int
            MyCommand.Parameters.Add(sparam5)


            'SETTIMO PARAMETRO C=@IDENTE INPUT
            Dim sparam6 As SqlClient.SqlParameter
            sparam6 = New SqlClient.SqlParameter
            sparam6.ParameterName = "@IDENTE"
            sparam6.SqlDbType = SqlDbType.Int
            MyCommand.Parameters.Add(sparam6)

            'SETTIMO PARAMETRO C=@IDPROGRAMMA INPUT
            Dim sparam8 As SqlClient.SqlParameter
            sparam8 = New SqlClient.SqlParameter
            sparam8.ParameterName = "@IDPROGRAMMA"
            sparam8.SqlDbType = SqlDbType.Int
            MyCommand.Parameters.Add(sparam8)

            'QUARTO PARAMETRO D=@IDBANDOATTIVITA INPUT
            Dim sparam9 As SqlClient.SqlParameter
            sparam9 = New SqlClient.SqlParameter
            sparam9.ParameterName = "@IDBANDOPROGRAMMA"
            sparam9.SqlDbType = SqlDbType.Int
            MyCommand.Parameters.Add(sparam9)

            'PARAMETRO 1=esito OUTPUT
            Dim sparam7 As SqlClient.SqlParameter
            sparam7 = New SqlClient.SqlParameter
            sparam7.ParameterName = "@ESITO"
            sparam7.SqlDbType = SqlDbType.Int
            sparam7.Direction = ParameterDirection.Output
            MyCommand.Parameters.Add(sparam7)


            MyCommand.Parameters("@SESSION_IDENTE").Value = strIDENTE
            If strCODICEENTE = Nothing Then
                strCODICEENTE = ""
            End If
            MyCommand.Parameters("@SESSION_CODICEENTE").Value = strCODICEENTE
            MyCommand.Parameters("@IDATTIVITA").Value = strATTIVITA
            MyCommand.Parameters("@IDBANDOATTIVITA").Value = strBANDOATTIVITA
            MyCommand.Parameters("@IDENTEPERSONALE").Value = strENTEPERSONALE
            MyCommand.Parameters("@IDENTITA").Value = strENTITA
            MyCommand.Parameters("@IDENTE").Value = strENTE
            MyCommand.Parameters("@IDPROGRAMMA").Value = strPROGRAMMA
            MyCommand.Parameters("@IDBANDOPROGRAMMA").Value = strBANDOPROGRAMMA
            Reader = MyCommand.ExecuteReader()

            x = CStr(MyCommand.Parameters("@ESITO").Value)


            Reader.Close()
            Reader = Nothing

            Return x

        Catch ex As Exception
            If Not Reader Is Nothing Then
                Reader.Close()
                Reader = Nothing
            End If


        End Try
    End Function

    Public Shared Function GeneraFascicoloCumulati(ByVal strUserName As String, ByVal strIdEntita As String, ByVal sqllocalconn As SqlClient.SqlConnection, ByVal IntIdLog As Integer) As String
        'Dim wsIN As New WS_Verifiche.VERIFICHEs
        'Dim wsOUT As New WS_Verifiche.FASCICOLAZIONE_DOCUMENTO_RISPOSTA
        Dim SIGED As clsSiged
        '''''Dim wsOUT As New WS_SIGeD.FASCICOLO_CREATO
        ' Dim wsMultiValore As New WS_SIGeD.DATO_MULTI
        Dim wsOUT As New WS_SIGeD.MULTI_FASCICOLO_CREATO
        Dim IntLocalIdLog As Integer
        Dim strSQL As String
        Dim DataSetRicerca As DataSet
        Dim strDescrizioneFascicolo As String
        Dim strNomeUtente As String
        Dim strCognomeUtente As String
        Dim strTitolario As String
        Dim strAppoTitolario As String
        Dim dtrLeggiDati As SqlClient.SqlDataReader
        Dim CmdGenerico As SqlClient.SqlCommand
        Dim strdefaultfascicolo As String
        Dim blnErroreFascicolo As Boolean = False
        Dim strServizio As String
        Try
            ''modificato il 30/08/2011
            'SIGED = New clsSiged("", strNome, strCognome)

            'CodFascicolo = SIGED.SIGED_IdFascicoloCompleto(IdFascicolo)

            'wsOUT = SIGED.SIGED_IndiceFascicoli(CodFascicolo)
            'sEsito = SIGED.SIGED_Codice_Esito(wsOUT.ESITO)

            'If sEsito = 0 Then
            '    If Right(wsOUT.ESITO, 1) = "0" Then

            '    Else

            '        strNumeroFascicolo = wsELENCO.NUMERO
            '        DescrizioneFascicolo = wsELENCO.DESCRIZIONE


            '***Destinatario per tutte le richieste di protocollazione = dall'incarico
            strSQL = " Select u.Nome, u.Cognome, isnull(s.descrizione,'')as descrizione " & _
              " From UtentiUNSC u LEFT JOIN TServiziSiged  s on  u.idservizio = s.idservizio " & _
              " Where u.UserName='" & strUserName & "'"


            DataSetRicerca = ClsServer.DataSetGenerico(strSQL, sqllocalconn)

            strNomeUtente = DataSetRicerca.Tables(0).Rows(0).Item("Nome")
            strCognomeUtente = DataSetRicerca.Tables(0).Rows(0).Item("Cognome")
            strServizio = DataSetRicerca.Tables(0).Rows(0).Item("descrizione")
            'Chiamata alla funzione per la scrittura del log
            IntLocalIdLog = LogFascicoliScrivi(sqllocalconn, IntIdLog, strUserName, "wsAuth.SWS_NEWSESSION", "@FIRSTNAME=" & strNomeUtente & ", @LASTNAME=" & strCognomeUtente & ", @PASSWORD=webservice")

            SIGED = New clsSiged("", strNomeUtente, strCognomeUtente)

            If SIGED.Codice_Esito <> 0 Then
                Exit Function
            End If
            'Chiamata alla funzione per la conferma dell' esecuzione del metodo loggato 
            IntLocalIdLog = LogFascicoliConferma(sqllocalconn, IntLocalIdLog)

            DataSetRicerca.Clear()

            strSQL = "SELECT isnull(bando.Titolario, '') as Titolario, entit‡.identit‡ as IdVolontario, entit‡.Nome, entit‡.Cognome, isnull(entit‡.CodiceVolontario,'') as CodiceVolontario, isnull(defaultfascicolo,'') as defaultfascicolo "
            strSQL = strSQL & "FROM entit‡ INNER JOIN "
            strSQL = strSQL & "attivit‡entit‡ ON entit‡.IDEntit‡ = attivit‡entit‡.IDEntit‡ INNER JOIN "
            strSQL = strSQL & "attivit‡entisediattuazione ON attivit‡entit‡.IDAttivit‡EnteSedeAttuazione = attivit‡entisediattuazione.IDAttivit‡EnteSedeAttuazione INNER JOIN "
            strSQL = strSQL & "attivit‡ ON attivit‡entisediattuazione.IDAttivit‡ = attivit‡.IDAttivit‡ INNER JOIN "
            strSQL = strSQL & "BandiAttivit‡ ON attivit‡.IDBandoAttivit‡ = BandiAttivit‡.IdBandoAttivit‡ INNER JOIN "
            strSQL = strSQL & "bando ON BandiAttivit‡.IdBando = bando.IDBando "
            strSQL = strSQL & "Where entit‡.IdEntit‡ in (" & Replace(strIdEntita, "#", ",") & ") and isnull(entit‡.CodiceFascicolo,'') = '' "

            'DataSetRicerca = ClsServer.DataSetGenerico(strSQL, sqllocalconn)

            'chiudo il datareader
            If Not dtrLeggiDati Is Nothing Then
                dtrLeggiDati.Close()
                dtrLeggiDati = Nothing
            End If

            'prendo la data dal server
            dtrLeggiDati = ClsServer.CreaDatareader(strSQL, sqllocalconn)

            Dim vDescrizione() As String
            Dim vIdEntita() As String

            ReDim vDescrizione(0)
            ReDim vIdEntita(0)

            vDescrizione(0) = ""
            vIdEntita(0) = ""

            If dtrLeggiDati.HasRows = True Then
                Do While dtrLeggiDati.Read
                    If vDescrizione(0) = "" Then
                        vDescrizione(0) = dtrLeggiDati("Cognome") & " " & dtrLeggiDati("Nome") & " (" & dtrLeggiDati("CodiceVolontario") & ")"
                        vIdEntita(0) = dtrLeggiDati("IdVolontario")
                    Else
                        ReDim Preserve vDescrizione(UBound(vDescrizione) + 1)
                        ReDim Preserve vIdEntita(UBound(vIdEntita) + 1)

                        vDescrizione(UBound(vDescrizione)) = dtrLeggiDati("Cognome") & " " & dtrLeggiDati("Nome") & " (" & dtrLeggiDati("CodiceVolontario") & ")"
                        vIdEntita(UBound(vIdEntita)) = dtrLeggiDati("IdVolontario")
                    End If
                    If strDescrizioneFascicolo = "" Then
                        strDescrizioneFascicolo = dtrLeggiDati("Cognome") & " " & dtrLeggiDati("Nome") & " (" & dtrLeggiDati("CodiceVolontario") & ")"
                    Else
                        strDescrizioneFascicolo = strDescrizioneFascicolo & "@" & dtrLeggiDati("Cognome") & " " & dtrLeggiDati("Nome") & " (" & dtrLeggiDati("CodiceVolontario") & ")"
                    End If
                    'strDescrizioneFascicolo = strDescrizioneFascicolo & dtrLeggiDati("Cognome") & " " & dtrLeggiDati("Nome") & " (" & dtrLeggiDati("CodiceVolontario") & ")" & "@" 'DataSetRicerca.Tables(0).Rows(0).Item("Cognome") & " " & DataSetRicerca.Tables(0).Rows(0).Item("Nome") & " (" & DataSetRicerca.Tables(0).Rows(0).Item("CodiceVolontario") & ")"
                    strTitolario = dtrLeggiDati("Titolario")
                    strdefaultfascicolo = dtrLeggiDati("defaultfascicolo")
                Loop

                'chiudo il datareader
                If Not dtrLeggiDati Is Nothing Then
                    dtrLeggiDati.Close()
                    dtrLeggiDati = Nothing
                End If

                If strTitolario <> "" Then
                    strAppoTitolario = Mid(strTitolario, 1, InStr(strTitolario, "-") - 1)
                    strAppoTitolario = Replace(strAppoTitolario, " ", "")

                    strTitolario = strAppoTitolario
                End If

                'avvio la fascicolazione
                'wsOUT = wsIN.FASCICOLAZIONE_DOCUMENTO(strCognomeUtente, strNomeUtente, "NO", "*****", "", "", "FASC0001", Mid(strDescrizioneFascicolo, 1, Len(strDescrizioneFascicolo) - 1), strCognomeUtente & " " & strNomeUtente, strTitolario, "", "", "", "", "", "", "", "", "", "", "", "")
                'wsOUT = wsIN.FASCICOLAZIONE_DOCUMENTOFILE(strCognomeUtente, strNomeUtente, "NO", "*****", "", "FASC0001", Mid(strDescrizioneFascicolo, 1, Len(strDescrizioneFascicolo) - 1), strCognomeUtente & " " & strNomeUtente, strTitolario, "", "", "", "", "", "", "", "", "", "", "", "")
                'wsOUT = wsIN.FASCICOLAZIONE_DOCUMENTOFILE(strCognomeUtente, strNomeUtente, "NO", "*****", "", "FASC0001", Mid(strDescrizioneFascicolo, 1, Len(strDescrizioneFascicolo) - 1), strCognomeUtente & " " & strNomeUtente, strTitolario, "", "", "", "", "", "", "", "", "", "", "", "")

                'INDICE FASCICOLO SERVE SOLO NUMERO FASCICOLO E CODCIE FASCICOLO
                'wsOUT = wsIN.FASCICOLAZIONE_DOCUMENTOFILE(strCognomeUtente, strNomeUtente, "NO", "*****", "", strdefaultfascicolo, Mid(strDescrizioneFascicolo, 1, Len(strDescrizioneFascicolo) - 1), strCognomeUtente & " " & strNomeUtente, strTitolario, "", "", "", "", "", "", "", "", "", "", "", "")
                Dim wsDatoMulti As WS_SIGeD.DATO_MULTI
                Dim wsMultiValore As WS_SIGeD.MULTI_VALORE
                Dim wsElencoFasc() As WS_SIGeD.FASCICOLO_CREATO
                Dim intZ As Integer
                Dim strNumFasc As String
                Dim strCodFasc As String
                Dim strAppoNumFasc As String
                Dim strAppoCodFasc As String
                Dim intX As Integer

                'Chiamata alla funzione per la scrittura del log
                IntLocalIdLog = LogFascicoliScrivi(sqllocalconn, IntIdLog, strUserName, "wsSiged.CONVERTIINMULTIVALORE", "@VALORESINGLE=" & strDescrizioneFascicolo)
                wsDatoMulti = SIGED.SIGED_ConvertiMultiValore(strDescrizioneFascicolo)
                'Chiamata alla funzione per la conferma dell' esecuzione del metodo loggato 
                IntLocalIdLog = LogFascicoliConferma(sqllocalconn, IntLocalIdLog)


                'Chiamata alla funzione per la scrittura del log
                IntLocalIdLog = LogFascicoliScrivi(sqllocalconn, IntIdLog, strUserName, "wsSiged.CREAFASCICOLOMULTIPLO", "@SESSIONE=" & SIGED.Connessione & ", @DESCRIZIONE=wsDatoMulti, @RIFERIMENTO=, @CODICETITOLARIO=" & strTitolario & ", @STATO=, @DATAAPERTURA=, @UNITAORGANIZZATIVARESPONSABILE=" & strServizio & ", @CATEGORIA=, @CODICEDEFAULT=" & strdefaultfascicolo)
                wsOUT = SIGED.SIGED_CreaFascicoloMultiplo(wsDatoMulti, "", strTitolario, "", "", strServizio, "", strdefaultfascicolo)


                wsElencoFasc = wsOUT.ELENCO_FASCICOLI
                For intZ = 1 To UBound(wsElencoFasc)
                    '*** agg. il 15/01/2014
                    If SIGED.SIGED_Codice_Esito(wsElencoFasc(intZ).ESITO) = 0 Then
                        SIGED.NormalizzaCodice(wsElencoFasc(intZ).CODICEFASCICOLO)
                        strAppoCodFasc = SIGED.CodiceNormalizzato
                        strAppoNumFasc = strTitolario & "/" & CInt(wsElencoFasc(intZ).NUMEROFASCICOLO)

                        strNumFasc = strNumFasc & strAppoNumFasc & "-"
                        strCodFasc = strCodFasc & strAppoCodFasc & "-"
                    Else
                        strNumFasc = strNumFasc & "ERRORE" & "-"
                        strCodFasc = strCodFasc & "ERRORE" & "-"
                        blnErroreFascicolo = True
                    End If

                    '***
                    'SIGED.NormalizzaCodice(wsElencoFasc(intZ).CODICEFASCICOLO)
                    'strAppoCodFasc = SIGED.CodiceNormalizzato
                    'strAppoNumFasc = strTitolario & "/" & CInt(wsElencoFasc(intZ).NUMEROFASCICOLO)

                    'strNumFasc = strNumFasc & strAppoNumFasc & "-"
                    'strCodFasc = strCodFasc & strAppoCodFasc & "-"
                Next
                GeneraFascicoloCumulati = SIGED.SIGED_Codice_Esito(wsOUT.ESITO)
                'GeneraFascicoloCumulati = wsOUT.ESITO
                If GeneraFascicoloCumulati = "0" Then
                    'Chiamata alla funzione per la conferma dell' esecuzione del metodo loggato 
                    IntLocalIdLog = LogFascicoliConferma(sqllocalconn, IntLocalIdLog)
                    For intX = 0 To UBound(vIdEntita)

                        'agg. il 15/01/2014 update solo se non c'È errore nella generazione del fascicolo del volontario
                        If Mid(strNumFasc, 1, InStr(strNumFasc, "-") - 1) <> "ERRORE" Then
                            strSQL = " Update entit‡ set CodiceFascicolo= '" & Mid(strNumFasc, 1, InStr(strNumFasc, "-") - 1) & "', " & _
                                     " IDFascicolo ='" & Mid(strCodFasc, 1, InStr(strCodFasc, "-") - 1) & "', " & _
                                     " DescrFascicolo = '" & Replace(vDescrizione(intX), "'", "''") & "' " & _
                                     " where identit‡=" & vIdEntita(intX) & ""

                            CmdGenerico = ClsServer.EseguiSqlClient(strSQL, sqllocalconn)
                        End If

                        strNumFasc = Mid(strNumFasc, InStr(strNumFasc, "-") + 1)
                        strCodFasc = Mid(strCodFasc, InStr(strCodFasc, "-") + 1)
                    Next
                    If blnErroreFascicolo = True Then
                        GeneraFascicoloCumulati = "11111"
                    End If
                End If

            Else
                'assegno come valore di ritorno un codice di errore fittizio che indica che la query nn ha prodotto risultato
                'GeneraFascicoloCumulati = "11111"
                GeneraFascicoloCumulati = "0"
            End If

            'Dim strNumFasc As String = wsOUT.NUMERO_FASCICOLO

            'Dim strCodFasc As String = wsOUT.CODICE_FASCICOLO


            'strNumFasc = strNumFasc & "-"
            'strCodFasc = strCodFasc & "-"



            ''''''Dim strNumFasc As String
            ''''''Dim strCodFasc As String
            ''''''Dim intX As Integer


            ''''''Dim strAppoDescrizioneFasc As String
            ''''''For intX = 0 To UBound(vIdEntita)
            ''''''    strAppoDescrizioneFasc = vDescrizione(intX)
            ''''''    Dim xxx As WS_SIGeD.DATO_MULTI
            ''''''    Dim WS As WS_SIGeD.MULTI_FASCICOLO_CREATO

            ''''''    Dim YYY As WS_SIGeD.MULTI_VALORE
            ''''''    YYY.VALORE = "AA"
            ''''''    YYY.VALORE =
            ''''''    wsOUT = SIGED.SIGED_CreaFascicolo(Replace(strAppoDescrizioneFasc, "'", "''"), "", strTitolario, "", "", strServizio, "", strdefaultfascicolo)
            ''''''    ''wsOUT = SIGED.SIGED_CreaFascicoloExpress("", strAppoDescrizioneFasc, "", strTitolario, "", "", strServizio, "", "", "", "", "", strdefaultfascicolo)

            ''''''    ''GeneraFascicoloCumulati = SIGED.SIGED_Codice_Esito(wsOUT.ESITO)
            ''''''    GeneraFascicoloCumulati = wsOUT.ESITO
            ''''''    'richiamo funzione che ricava il numero fascicolo appena creato e fa l'update in entit‡-->vecchio webservice
            ''''''    '** con il nuovo webservice richiamo la funzione per fare l'udate in entit‡ 
            ''''''    SIGED.NormalizzaCodice(wsOUT.CODICEFASCICOLO)
            ''''''    strCodFasc = SIGED.CodiceNormalizzato
            ''''''    strNumFasc = strTitolario & "/" & CInt(wsOUT.NUMEROFASCICOLO)




            ''''''    'strNumFasc = strNumFasc & "-"
            ''''''    'strCodFasc = strCodFasc & "-"

            ''''''    'strNumeroFascicolo = wsELENCO(riga).NUMERO_FASCICOLO
            ''''''    'strIdFascicolo = wsELENCO(riga).CODICE_FASCICOLO
            ''''''    If SIGED.SIGED_Codice_Esito(GeneraFascicoloCumulati) = 0 Then
            ''''''        strSQL = " Update entit‡ set CodiceFascicolo= '" & strNumFasc & "', " & _
            ''''''                    " IDFascicolo ='" & strCodFasc & "', " & _
            ''''''                    " DescrFascicolo = '" & Replace(vDescrizione(intX), "'", "''") & "' " & _
            ''''''                    " where identit‡=" & vIdEntita(intX) & ""

            ''''''        CmdGenerico = ClsServer.EseguiSqlClient(strSQL, sqllocalconn)
            ''''''    End If
            ''''''    'assegno come valore di ritorno un codice di errore fittizio che indica che la query nn ha prodotto risultato

            ''''''    'strNumFasc = Mid(strNumFasc, InStr(strNumFasc, "-") + 1)
            ''''''    'strCodFasc = Mid(strCodFasc, InStr(strCodFasc, "-") + 1)

            ''''''    'richiamo funzione che ricava il numero fascicolo appena creato e fa l'update in entit‡
            ''''''    'MODIFICA DEL 22/10/2010 PER EVITARE DI RICHIAMARE ULTERIORMENTE IL WEB SERVICES SIGED
            ''''''    'TrovaFascicolo(strUserName, vDescrizione(intX), vIdEntita(intX), sqllocalconn) 
            ''''''Next
            ''''''End If
            '''''Else
            ''''''assegno come valore di ritorno un codice di errore fittizio che indica che la query nn ha prodotto risultato
            '''''GeneraFascicoloCumulati = "11111"
            '''''End If

            'chiudo il datareader
            If Not dtrLeggiDati Is Nothing Then
                dtrLeggiDati.Close()
                dtrLeggiDati = Nothing
            End If

            Return GeneraFascicoloCumulati

        Catch ex As Exception
            GeneraFascicoloCumulati = ex.Message
            DataSetRicerca.Dispose()
        Finally

            SIGED.SIGED_Chiudi_Autenticazione(strNomeUtente, strCognomeUtente)

        End Try
    End Function
    Public Shared Function GeneraFascicoloCumulatiVerifiche(ByVal strUserName As String, ByVal strIdVerifica As String, ByVal sqllocalconn As SqlClient.SqlConnection) As String
        '*** CREATA DA SIMONA CORDELLA IL 04/05/2012 ***
        '*** GENERAZIONE AUTOMATICA DEI FASCIOLO PER LE VERIIFCHE ***
        Dim SIGED As clsSiged
        Dim wsOUT As New WS_SIGeD.MULTI_FASCICOLO_CREATO
        Dim strSQL As String
        Dim DataSetRicerca As DataSet
        Dim strDescrizioneFascicolo As String
        Dim strNomeUtente As String
        Dim strCognomeUtente As String
        Dim strTitolario As String
        Dim strAppoTitolario As String
        Dim dtrLeggiDati As SqlClient.SqlDataReader
        Dim CmdGenerico As SqlClient.SqlCommand
        Dim strdefaultfascicolo As String

        Dim strServizio As String
        Try

            '***Destinatario per tutte le richieste di protocollazione = dall'incarico
            strSQL = " Select u.Nome, u.Cognome, isnull(s.descrizione,'')as descrizione " & _
                    " From UtentiUNSC u LEFT JOIN TServiziSiged  s on  u.idservizio = s.idservizio " & _
                    " Where u.UserName='" & strUserName & "'"


            DataSetRicerca = ClsServer.DataSetGenerico(strSQL, sqllocalconn)

            strNomeUtente = DataSetRicerca.Tables(0).Rows(0).Item("Nome")
            strCognomeUtente = DataSetRicerca.Tables(0).Rows(0).Item("Cognome")
            strServizio = DataSetRicerca.Tables(0).Rows(0).Item("descrizione")

            SIGED = New clsSiged("", strNomeUtente, strCognomeUtente)

            If SIGED.Codice_Esito <> 0 Then
                Exit Function
            End If

            DataSetRicerca.Clear()


            strSQL = " SELECT isnull(TVerificheProgrammazione.DefaultTitolarioVerifica, '') as Titolario, TVerifiche.IdVerifica ,isnull(TVerificheProgrammazione.DefaultFascicoloVerifica,'') as defaultfascicolo, "
            strSQL = strSQL & " enti.codiceregione + ' ' + enti.Denominazione  as Ente, Replace(attivit‡.Titolo,'@','A') AS Titolo, comuni.Denominazione + ' (' +  provincie.DescrAbb +')' as Comune "
            strSQL = strSQL & " FROM comuni "
            strSQL = strSQL & " INNER JOIN entisedi ON comuni.IDComune = entisedi.IDComune "
            strSQL = strSQL & " INNER JOIN provincie ON comuni.IDProvincia = provincie.IDProvincia "
            strSQL = strSQL & " INNER JOIN entisediattuazioni ON entisedi.IDEnteSede = entisediattuazioni.IDEnteSede "
            strSQL = strSQL & " INNER JOIN TVerifiche "
            strSQL = strSQL & " INNER JOIN TVerificheProgrammazione ON TVerifiche.IDProgrammazione = TVerificheProgrammazione.IDProgrammazione "
            strSQL = strSQL & " INNER JOIN TVerificheAssociate ON TVerifiche.IDVerifica = TVerificheAssociate.IDVerifica "
            strSQL = strSQL & " INNER JOIN attivit‡entisediattuazione ON TVerificheAssociate.IDAttivit‡EnteSedeAttuazione = attivit‡entisediattuazione.IDAttivit‡EnteSedeAttuazione "
            strSQL = strSQL & " INNER JOIN attivit‡ ON attivit‡entisediattuazione.IDAttivit‡ = attivit‡.IDAttivit‡ "
            strSQL = strSQL & " INNER JOIN enti ON attivit‡.IDEntePresentante = enti.IDEnte ON entisediattuazioni.IDEnteSedeAttuazione = attivit‡entisediattuazione.IDEnteSedeAttuazione"
            strSQL = strSQL & " WHERE TVerifiche.IdVerifica in (" & Replace(strIdVerifica, "#", ",") & ")"

            'DataSetRicerca = ClsServer.DataSetGenerico(strSQL, sqllocalconn)

            'chiudo il datareader
            If Not dtrLeggiDati Is Nothing Then
                dtrLeggiDati.Close()
                dtrLeggiDati = Nothing
            End If

            'prendo la data dal server
            dtrLeggiDati = ClsServer.CreaDatareader(strSQL, sqllocalconn)

            Dim strMAXDescrizoneFascicolo As String
            Dim vDescrizione() As String
            Dim vIdVerifica() As String

            ReDim vDescrizione(0)
            ReDim vIdVerifica(0)

            vDescrizione(0) = ""
            vIdVerifica(0) = ""

            If dtrLeggiDati.HasRows = True Then
                Do While dtrLeggiDati.Read
                    strMAXDescrizoneFascicolo = LunghezzaMaxDescrizioneFascicoloVerifiche(dtrLeggiDati("Ente"), dtrLeggiDati("Titolo"), dtrLeggiDati("Comune"))
                    If vDescrizione(0) = "" Then
                        'gestione della lunghezza massima del fascicolo se la lunghezza Ë maggione di 200 bisogna tagliare le stringhe
                        'vDescrizione(0) = dtrLeggiDati("Ente") & " - " & dtrLeggiDati("Titolo") & " - " & dtrLeggiDati("Comune") & ""
                        vDescrizione(0) = strMAXDescrizoneFascicolo
                        vIdVerifica(0) = dtrLeggiDati("IdVerifica")
                    Else
                        ReDim Preserve vDescrizione(UBound(vDescrizione) + 1)
                        ReDim Preserve vIdVerifica(UBound(vIdVerifica) + 1)

                        'vDescrizione(UBound(vDescrizione)) = dtrLeggiDati("Ente") & " - " & dtrLeggiDati("Titolo") & " - " & dtrLeggiDati("Comune") & ""
                        vDescrizione(UBound(vDescrizione)) = strMAXDescrizoneFascicolo
                        vIdVerifica(UBound(vIdVerifica)) = dtrLeggiDati("IdVerifica")
                    End If
                    If strDescrizioneFascicolo = "" Then
                        'strDescrizioneFascicolo = dtrLeggiDati("Ente") & " - " & dtrLeggiDati("Titolo") & " - " & dtrLeggiDati("Comune") & ""
                        strDescrizioneFascicolo = strMAXDescrizoneFascicolo
                    Else
                        'strDescrizioneFascicolo = strDescrizioneFascicolo & "@" & dtrLeggiDati("Ente") & " - " & dtrLeggiDati("Titolo") & " - " & dtrLeggiDati("Comune") & ""
                        strDescrizioneFascicolo = strDescrizioneFascicolo & "@" & strMAXDescrizoneFascicolo
                    End If
                    strTitolario = dtrLeggiDati("Titolario")
                    strdefaultfascicolo = dtrLeggiDati("defaultfascicolo")
                Loop

                'chiudo il datareader
                If Not dtrLeggiDati Is Nothing Then
                    dtrLeggiDati.Close()
                    dtrLeggiDati = Nothing
                End If

                'INDICE FASCICOLO SERVE SOLO NUMERO FASCICOLO E CODCIE FASCICOLO
                Dim wsDatoMulti As WS_SIGeD.DATO_MULTI
                Dim wsMultiValore As WS_SIGeD.MULTI_VALORE
                Dim wsElencoFasc() As WS_SIGeD.FASCICOLO_CREATO
                Dim intZ As Integer
                Dim strNumFasc As String
                Dim strCodFasc As String
                Dim strAppoNumFasc As String
                Dim strAppoCodFasc As String
                Dim intX As Integer


                wsDatoMulti = SIGED.SIGED_ConvertiMultiValore(strDescrizioneFascicolo)
                wsOUT = SIGED.SIGED_CreaFascicoloMultiplo(wsDatoMulti, "", strTitolario, "", "", strServizio, "", strdefaultfascicolo)


                wsElencoFasc = wsOUT.ELENCO_FASCICOLI
                For intZ = 1 To UBound(wsElencoFasc)
                    SIGED.NormalizzaCodice(wsElencoFasc(intZ).CODICEFASCICOLO)
                    strAppoCodFasc = SIGED.CodiceNormalizzato

                    strAppoNumFasc = strTitolario & "/" & CInt(wsElencoFasc(intZ).NUMEROFASCICOLO)

                    strNumFasc = strNumFasc & strAppoNumFasc & "-"
                    strCodFasc = strCodFasc & strAppoCodFasc & "-"
                Next
                GeneraFascicoloCumulatiVerifiche = SIGED.SIGED_Codice_Esito(wsOUT.ESITO)
                'GeneraFascicoloCumulati = wsOUT.ESITO
                If GeneraFascicoloCumulatiVerifiche = "0" Then
                    For intX = 0 To UBound(vIdVerifica)

                        strSQL = " Update TVerifiche set CodiceFascicolo= '" & Mid(strNumFasc, 1, InStr(strNumFasc, "-") - 1) & "', " & _
                                 " IDFascicolo ='" & Mid(strCodFasc, 1, InStr(strCodFasc, "-") - 1) & "', " & _
                                 " DescrFascicolo = '" & Replace(vDescrizione(intX), "'", "''") & "' " & _
                                 " where IdVerifica=" & vIdVerifica(intX) & ""

                        CmdGenerico = ClsServer.EseguiSqlClient(strSQL, sqllocalconn)

                        strNumFasc = Mid(strNumFasc, InStr(strNumFasc, "-") + 1)
                        strCodFasc = Mid(strCodFasc, InStr(strCodFasc, "-") + 1)
                    Next
                End If
                GeneraFascicoloCumulatiVerifiche = "Fascicolazione effettuata con successo."
            Else
                'assegno come valore di ritorno un codice di errore fittizio che indica che la query nn ha prodotto risultato
                GeneraFascicoloCumulatiVerifiche = "11111"
            End If

            'chiudo il datareader
            If Not dtrLeggiDati Is Nothing Then
                dtrLeggiDati.Close()
                dtrLeggiDati = Nothing
            End If

            Return GeneraFascicoloCumulatiVerifiche

        Catch ex As Exception
            GeneraFascicoloCumulatiVerifiche = ex.Message
            DataSetRicerca.Dispose()
        Finally
            SIGED.SIGED_Chiudi_Autenticazione(strNomeUtente, strCognomeUtente)
        End Try
    End Function

    Private Shared Function LunghezzaMaxDescrizioneFascicoloVerifiche(ByVal Ente As String, ByVal Titolo As String, ByVal Comune As String) As String
        Dim intTotaleLughezza As Integer
        Dim intLughezzaDisponibile As Integer
        Dim strDescFasc As String
        Dim intLenEnte As Integer
        Dim intLenTitolo As Integer
        Dim intLenComune As Integer
        Dim intLunghezzaMinima As Integer = 50
        Dim strTitolo As String
        Dim strEnte As String

        Titolo = " - " & Titolo
        Comune = " - " & Comune

        intLenEnte = Len(Ente)
        intLenTitolo = Len(Titolo)
        intLenComune = Len(Comune)


        intTotaleLughezza = intLenEnte + intLenTitolo + intLenComune
        If intTotaleLughezza > 200 Then
            intLughezzaDisponibile = 200 - intLenComune
            If intLughezzaDisponibile - intLenTitolo < intLunghezzaMinima Then
                'strTitolo = Mid(Titolo, 1, intLunghezzaMinima)
                'strEnte = Mid(Ente, 1, intLughezzaDisponibile - Len(strTitolo))
                strEnte = Mid(Ente, 1, intLunghezzaMinima)
                strTitolo = Mid(Titolo, 1, intLughezzaDisponibile - Len(strEnte))
                strDescFasc = strEnte & strTitolo & Comune
            Else
                strDescFasc = Mid(Ente, 1, intLughezzaDisponibile - intLunghezzaMinima) & Mid(Titolo, 1, intLunghezzaMinima) & Comune
            End If
        Else
            strDescFasc = Ente & Titolo & Comune
        End If
        Return strDescFasc
    End Function

    Public Shared Function GeneraFascicolo(ByVal strUserName As String, ByVal strIdEntita As String, ByVal sqllocalconn As SqlClient.SqlConnection, ByVal IntIdLog As Integer) As String
        'Dim wsIN As New WS_Verifiche.VERIFICHEs
        'Dim wsOUT As New WS_Verifiche.FASCICOLAZIONE_DOCUMENTO_RISPOSTA
        Dim SIGED As clsSiged
        Dim wsOUT As New WS_SIGeD.FASCICOLO_CREATO
        Dim IntLocalIdLog As Integer
        Dim strSQL As String
        Dim DataSetRicerca As DataSet
        Dim strDescrizioneFascicolo As String
        Dim strNomeUtente As String
        Dim strCognomeUtente As String
        Dim strTitolario As String
        Dim strAppoTitolario As String
        Dim strdefaultfascicolo As String
        Dim strServizio As String
        Dim strIdFascicolo As String
        Dim strNumFascicolo As String

        Try

            '***Destinatario per tutte le richieste di protocollazione = dall'incarico
            strSQL = " Select u.Nome, u.Cognome, isnull(s.descrizione,'')as descrizione " & _
              " From UtentiUNSC u left JOIN TServiziSiged  s on  u.idservizio = s.idservizio " & _
              " Where u.UserName='" & strUserName & "'"


            DataSetRicerca = ClsServer.DataSetGenerico(strSQL, sqllocalconn)

            strNomeUtente = DataSetRicerca.Tables(0).Rows(0).Item("Nome")
            strCognomeUtente = DataSetRicerca.Tables(0).Rows(0).Item("Cognome")
            strServizio = DataSetRicerca.Tables(0).Rows(0).Item("descrizione")

            'Chiamata alla funzione per la scrittura del log
            IntLocalIdLog = LogFascicoliScrivi(sqllocalconn, IntIdLog, strUserName, "wsAuth.SWS_NEWSESSION", "@FIRSTNAME=" & strNomeUtente & ", @LASTNAME=" & strCognomeUtente & ", @PASSWORD=webservice")



            SIGED = New clsSiged("", strNomeUtente, strCognomeUtente)
            If SIGED.Codice_Esito <> 0 Then
                Exit Function
            End If

            'Chiamata alla funzione per la conferma dell' esecuzione del metodo loggato 
            IntLocalIdLog = LogFascicoliConferma(sqllocalconn, IntLocalIdLog)

            DataSetRicerca.Clear()

            strSQL = "SELECT isnull(bando.Titolario, '') as Titolario, entit‡.Nome, entit‡.Cognome, isnull(entit‡.CodiceVolontario,'') as CodiceVolontario, isnull(defaultfascicolo,'') as defaultfascicolo "
            strSQL = strSQL & "FROM entit‡ INNER JOIN "
            strSQL = strSQL & "attivit‡entit‡ ON entit‡.IDEntit‡ = attivit‡entit‡.IDEntit‡ INNER JOIN "
            strSQL = strSQL & "attivit‡entisediattuazione ON attivit‡entit‡.IDAttivit‡EnteSedeAttuazione = attivit‡entisediattuazione.IDAttivit‡EnteSedeAttuazione INNER JOIN "
            strSQL = strSQL & "attivit‡ ON attivit‡entisediattuazione.IDAttivit‡ = attivit‡.IDAttivit‡ INNER JOIN "
            strSQL = strSQL & "BandiAttivit‡ ON attivit‡.IDBandoAttivit‡ = BandiAttivit‡.IdBandoAttivit‡ INNER JOIN "
            strSQL = strSQL & "bando ON BandiAttivit‡.IdBando = bando.IDBando "
            strSQL = strSQL & "Where entit‡.IdEntit‡ = '" & strIdEntita & "'"

            DataSetRicerca = ClsServer.DataSetGenerico(strSQL, sqllocalconn)

            If DataSetRicerca.Tables(0).Rows.Count > 0 Then
                strDescrizioneFascicolo = DataSetRicerca.Tables(0).Rows(0).Item("Cognome") & " " & DataSetRicerca.Tables(0).Rows(0).Item("Nome") & " (" & DataSetRicerca.Tables(0).Rows(0).Item("CodiceVolontario") & ")"
                strTitolario = DataSetRicerca.Tables(0).Rows(0).Item("Titolario")
                strdefaultfascicolo = DataSetRicerca.Tables(0).Rows(0).Item("defaultfascicolo")
                If strTitolario <> "" Then
                    strAppoTitolario = Mid(strTitolario, 1, InStr(strTitolario, "-") - 1)
                    strAppoTitolario = Replace(strAppoTitolario, " ", "")

                    strTitolario = strAppoTitolario

                End If

                'avvio la fascicolazione

                ' wsOUT = wsIN.FASCICOLAZIONE_DOCUMENTO(strCognomeUtente, strNomeUtente, "NO", "*****", "", "", strdefaultfascicolo, strDescrizioneFascicolo, strCognomeUtente & " " & strNomeUtente, strTitolario, "", "", "", "", "", "", "", "", "", "", "", "")

                'Chiamata alla funzione per la scrittura del log
                IntLocalIdLog = LogFascicoliScrivi(sqllocalconn, IntIdLog, strUserName, "wsSiged.CREAFASCICOLO", "@SESSIONE=" & SIGED.Connessione & ", @DESCRIZIONE=" & Replace(strDescrizioneFascicolo, "'", "''") & ", @RIFERIMENTO=, @CODICETITOLARIO=" & strTitolario & ", @STATO=, @DATAAPERTURA=, @UNITAORGANIZZATIVARESPONSABILE=" & strServizio & ", @CATEGORIA=, @CODICEDEFAULT=" & strdefaultfascicolo)

                wsOUT = SIGED.SIGED_CreaFascicolo(Replace(strDescrizioneFascicolo, "'", "''"), "", strTitolario, "", "", strServizio, "", strdefaultfascicolo)

                DataSetRicerca.Dispose()

                GeneraFascicolo = SIGED.SIGED_Codice_Esito(wsOUT.ESITO)

                If GeneraFascicolo = 0 Then

                    'Chiamata alla funzione per la conferma dell' esecuzione del metodo loggato 
                    IntLocalIdLog = LogFascicoliConferma(sqllocalconn, IntLocalIdLog)
                    'richiamo funzione che ricava il numero fascicolo appena creato e fa l'update in entit‡-->vecchio webservice
                    '** con il nuovo webservice richiamo la funzione per fare l'udate in entit‡ 
                    SIGED.NormalizzaCodice(wsOUT.CODICEFASCICOLO)
                    strIdFascicolo = SIGED.CodiceNormalizzato
                    strNumFascicolo = strTitolario & "/" & CInt(wsOUT.NUMEROFASCICOLO)

                    TrovaFascicolo(strUserName, strIdFascicolo, strNumFascicolo, strDescrizioneFascicolo, strIdEntita, sqllocalconn)
                End If
            Else
                'assegno come valore di ritorno un codice di errore fittizio che indica che la query nn ha prodotto risultato
                GeneraFascicolo = "1111"
            End If

            Return GeneraFascicolo

        Catch ex As Exception
            GeneraFascicolo = ex.Message
            DataSetRicerca.Dispose()
        Finally
            SIGED.SIGED_Chiudi_Autenticazione(strNomeUtente, strCognomeUtente)
        End Try
    End Function

    Public Shared Function TrovaFascicolo(ByVal strUserName As String, ByVal IdFascicolo As String, ByVal NumFascicolo As String, ByVal DescrizioneFascicolo As String, ByVal strIdEntita As String, ByVal sqllocalconn As SqlClient.SqlConnection) As String
        '*************** la funzione Ë stata modifica in seguito all'utilizzo dei nuovi webservice *****
        '** il nuovo metedo CREAFASCICOLOEXPRESS ritorna gi‡ il numero e l'id del fascicolo appena creata,
        '** quindi Ë stata tolto il richiamo al webservice in questa parte per ricerca l'idfascicolo e il numero fascicolo

        'Dim wsIN As New WS_Verifiche.VERIFICHEs
        'Dim wsOUT As New WS_Verifiche.RICERCA_FASCICOLO_RISPOSTA
        'Dim wsELENCO() As WS_Verifiche.FASCICOLO_TROVATO
        Dim sEsito As String

        Dim strSQL As String
        Dim DataSetRicerca As DataSet
        Dim strDescrizioneFascicolo As String
        Dim strNomeUtente As String
        Dim strCognomeUtente As String
        Dim riga As Integer
        Dim strNumeroFascicolo As String
        Dim strIdFascicolo As String
        Dim CmdGenerico As SqlClient.SqlCommand
        Try


            strNumeroFascicolo = NumFascicolo
            strIdFascicolo = IdFascicolo
            strSQL = " Update entit‡ set CodiceFascicolo= '" & strNumeroFascicolo & "', " & _
                     " IDFascicolo ='" & strIdFascicolo & "', " & _
                     " DescrFascicolo = '" & Replace(DescrizioneFascicolo, "'", "''") & "' " & _
                     " where identit‡=" & strIdEntita & ""
            CmdGenerico = ClsServer.EseguiSqlClient(strSQL, sqllocalconn)

            ''***Destinatario per tutte le richieste di protocollazione = dall'incarico
            'strSQL = "Select Nome, Cognome From UtentiUNSC Where UserName='" & strUserName & "'"

            'DataSetRicerca = ClsServer.DataSetGenerico(strSQL, sqllocalconn)

            'strNomeUtente = DataSetRicerca.Tables(0).Rows(0).Item("Nome")
            'strCognomeUtente = DataSetRicerca.Tables(0).Rows(0).Item("Cognome")

            'DataSetRicerca.Clear()

            'wsOUT = wsIN.RICERCA_FASCICOLI(strCognomeUtente, strNomeUtente, "500", "", "", "", DescrizioneFascicolo, "", "", "")

            'sEsito = Left(wsOUT.ESITO, 4)

            'If sEsito = "0000" Then

            '    If Right(wsOUT.ESITO, 1) = "0" Then

            '    Else
            'wsELENCO = wsOUT.ELENCO_FASCICOLI
            '        For riga = LBound(wsELENCO) To UBound(wsELENCO)
            '            If Not wsELENCO(riga).NUMERO_FASCICOLO Is Nothing Then
            '                'L'utente loggato ha accesso al fascicolo
            '                If wsELENCO(riga).DATA_CREAZIONE <> "" Then
            '                    strNumeroFascicolo = wsELENCO(riga).NUMERO_FASCICOLO
            '                    strIdFascicolo = wsELENCO(riga).CODICE_FASCICOLO
            '                    strSQL = " Update entit‡ set CodiceFascicolo= '" & strNumeroFascicolo & "', " & _
            '                             " IDFascicolo ='" & strIdFascicolo & "', " & _
            '                             " DescrFascicolo = '" & Replace(DescrizioneFascicolo, "'", "''") & "' " & _
            '                             " where identit‡=" & strIdEntita & ""
            '                    CmdGenerico = ClsServer.EseguiSqlClient(strSQL, sqllocalconn)
            '                End If
            '            End If
            '        Next
            '    End If
            'Else
            '    TrovaFascicolo = Mid(wsOUT.ESITO, 6, Len(wsOUT.ESITO))
            'End If

        Catch ex As Exception
            TrovaFascicolo = ex.Message
            DataSetRicerca.Dispose()
        End Try
    End Function

    Public Shared Function ConvertFileToBase64(ByVal strPercorsoFile As String) As String
        Dim bFile() As Byte
        Dim fs As FileStream
        Dim _textB64 As String
        Try
            fs = New FileStream(strPercorsoFile, FileMode.Open)
            ReDim bFile(fs.Length - 1)
            fs.Read(bFile, 0, fs.Length)
            _textB64 = Convert.ToBase64String(bFile)

            ConvertFileToBase64 = _textB64

        Catch ex As Exception
            _textB64 = ex.Message
            'Finally
            '    fs.Close()
        End Try

        fs.Close()

        Return ConvertFileToBase64
    End Function


    Public Shared Function TrovaPathLocale(ByVal strPath As String, ByVal sqllocalconn As SqlClient.SqlConnection) As String
        Dim dtrLeggiDati As SqlClient.SqlDataReader

        'chiudo il datareader
        If Not dtrLeggiDati Is Nothing Then
            dtrLeggiDati.Close()
            dtrLeggiDati = Nothing
        End If
        'prendo la data dal server
        dtrLeggiDati = ClsServer.CreaDatareader("select top 1 PathLocale FROM EDITOR_ModelliCompetenze WHERE Path='" & strPath & "'", sqllocalconn)

        If dtrLeggiDati.HasRows = True Then
            dtrLeggiDati.Read()

            TrovaPathLocale = dtrLeggiDati("PathLocale")
        End If

        'chiudo il datareader
        If Not dtrLeggiDati Is Nothing Then
            dtrLeggiDati.Close()
            dtrLeggiDati = Nothing
        End If

        Return TrovaPathLocale
    End Function

    Public Shared Function TrovaIDBando(ByVal IdAttivita As Integer, ByVal sqllocalconn As SqlClient.SqlConnection) As Integer
        Dim dtrLeggiDati As SqlClient.SqlDataReader

        'chiudo il datareader
        If Not dtrLeggiDati Is Nothing Then
            dtrLeggiDati.Close()
            dtrLeggiDati = Nothing
        End If
        'prendo la data dal server
        dtrLeggiDati = ClsServer.CreaDatareader("select bandiattivit‡.IdBando from bandiattivit‡ INNER JOIN attivit‡ ON attivit‡.idbandoattivit‡ = bandiattivit‡.idbandoattivit‡ Where attivit‡.idattivit‡=" & IdAttivita, sqllocalconn)

        If dtrLeggiDati.HasRows = True Then
            dtrLeggiDati.Read()

            TrovaIDBando = dtrLeggiDati("IdBando")
        End If

        'chiudo il datareader
        If Not dtrLeggiDati Is Nothing Then
            dtrLeggiDati.Close()
            dtrLeggiDati = Nothing
        End If

        Return TrovaIDBando
    End Function

    Public Shared Function TrovaIDEnteDaIDVerifica(ByVal IdVerifica As Integer, ByVal sqllocalconn As SqlClient.SqlConnection, Optional ByVal TipoVerifica As String = "PS") As Integer
        Dim dtrLeggiDati As SqlClient.SqlDataReader

        'chiudo il datareader
        If Not dtrLeggiDati Is Nothing Then
            dtrLeggiDati.Close()
            dtrLeggiDati = Nothing
        End If
        'prendo la data dal server
        If TipoVerifica = "PS" Then 'tipo verifica PROGRAMMATA/SEGNALATA
            dtrLeggiDati = ClsServer.CreaDatareader("select IdEnte FROM VW_EDITOR_VERIFICHE WHERE IdVerifica=" & IdVerifica, sqllocalconn)
        Else 'TIPO VERIFICA SEGNALATA UNSC
            dtrLeggiDati = ClsServer.CreaDatareader("select IdEnte FROM TVERIFICHE WHERE IdVerifica=" & IdVerifica, sqllocalconn)
        End If

        If dtrLeggiDati.HasRows = True Then
            dtrLeggiDati.Read()

            TrovaIDEnteDaIDVerifica = dtrLeggiDati("IdEnte")
        End If

        'chiudo il datareader
        If Not dtrLeggiDati Is Nothing Then
            dtrLeggiDati.Close()
            dtrLeggiDati = Nothing
        End If

        Return TrovaIDEnteDaIDVerifica
    End Function

    Public Shared Function TrovaTipoFile(ByVal File As String) As String
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

    Public Shared Function CriptaNuovaPass() As String

        ''CriptaNuovaPass = CreatePSW(NuovaPass)
        'moficato da simona cordella 
        '  'moficato da Francesco Lorusso 10/03/2011
        '
        Dim passwordCreata As String = "'"
        '

        While InStr(passwordCreata, "'") > 0
            passwordCreata = CreatePSW(GeneraPasswordVolontari)
        End While
        '
        Return passwordCreata
        '
    End Function


    Public Shared Function CheckPartitaIVA(ByVal sz_Codice)
        Dim n_Val
        Dim n_Som1
        Dim n_Som2
        Dim lcv
        CheckPartitaIVA = False

        If IsNumeric(sz_Codice) = False Then Exit Function

        If CDbl(sz_Codice) <= 0 Then Exit Function

        If Len(sz_Codice) < 11 Then Exit Function

        For lcv = 1 To 9 Step 2
            n_Val = Asc(Mid(sz_Codice, lcv, 1)) - Asc("0")
            n_Som1 = n_Som1 + n_Val
            n_Val = Asc(Mid(sz_Codice, lcv + 1, 1)) - Asc("0")
            n_Som1 = n_Som1 + Int((n_Val * 2) / 10) + ((n_Val * 2) Mod 10)
        Next
        n_Som2 = 10 - (n_Som1 Mod 10)
        n_Val = CDbl(Mid(sz_Codice, 11, 1))
        If n_Som2 = n_Val Then CheckPartitaIVA = True
    End Function

    'funzione che controlla se si deve bloccare le maschere dell'accreditamento
    'controlla il flag su enti se = 1 non blocco, se = 1
    'vado a controllare se il getdate() Ë compreso nelle date in Processi temporali
    'se nn Ë compresa blocco la maschera, se Ë compresa vado a controllare lo stato dell'ente
    'se Ë diverso da registrato o attivo blocco la maschera, altrimenti non la blocco
    '********************************************************************************
    'in ingresso mi aspetto l'id dell'ente e la connessione
    'restituisco false se NON devo bloccare la maschera
    'TRUE se DEVO bloccare la maschera
    Public Shared Function ControllaStatoEntePerBloccareMaschereAnagrafica(ByVal strIdEnte As String, ByVal conn As SqlClient.SqlConnection) As Boolean
        'funzione creata da jonziponzi
        'il 30/03/2006 DOPO L'ESAME 70-310 e, invece di tornare a casa sto qui a fare 'sta cavolata!!!!!
        'variabile stringa che uso per creare le query
        Dim Strsql As String
        'datareader klocale che uso per eseguire le query
        Dim dtrControlloEnte As SqlClient.SqlDataReader

        ControllaStatoEntePerBloccareMaschereAnagrafica = False

        'vado a controllare il FLAGFORZATURAACCREDITAMENTO sulla tabella degli enti
        Strsql = "select isnull(FlagForzaturaAccreditamento,0) as FlagForzaturaAccreditamento from enti where idente='" & strIdEnte & "'"

        'chiudo il datareader
        If Not dtrControlloEnte Is Nothing Then
            dtrControlloEnte.Close()
            dtrControlloEnte = Nothing
        End If

        'eseguo la query e la passo al datareader locale
        dtrControlloEnte = ClsServer.CreaDatareader(Strsql, conn)

        'controllo se ci sono righe
        If dtrControlloEnte.HasRows = True Then
            'leggo il datareader
            dtrControlloEnte.Read()
            'controllo il flag, se Ë FALSE, proseguo con i controlli
            'altrimenti se Ë TRUE eso e NON blocco la maschera
            Select Case dtrControlloEnte("FlagForzaturaAccreditamento")
                Case True
                    'chiudo il datareader
                    If Not dtrControlloEnte Is Nothing Then
                        dtrControlloEnte.Close()
                        dtrControlloEnte = Nothing
                    End If
                    'se Ë = 1, quindi TRUE, chiudo la funzione perchË non devo bloccare la maschera
                    ControllaStatoEntePerBloccareMaschereAnagrafica = False
                    Return ControllaStatoEntePerBloccareMaschereAnagrafica
                Case False
                    'chiudo il datareader
                    If Not dtrControlloEnte Is Nothing Then
                        dtrControlloEnte.Close()
                        dtrControlloEnte = Nothing
                    End If
                    'qui vado a controllare la data odienrna se Ë compresa in ProcessiTemporali
                    'se non Ë compresa chiudo la funzione e NON blocco la maschera, altrimenti continuo con i controlli

                    'vado a controllare il la data odierna
                    Strsql = "select IdProcessoTemporale from ProcessiTemporali where (Accreditamento=1) and (GetDate() between DataInizio and DataFine)"

                    'eseguo la query e la passo al datareader locale
                    dtrControlloEnte = ClsServer.CreaDatareader(Strsql, conn)

                    'controllo se ci sono righe
                    If dtrControlloEnte.HasRows = False Then
                        'chiudo il datareader
                        If Not dtrControlloEnte Is Nothing Then
                            dtrControlloEnte.Close()
                            dtrControlloEnte = Nothing
                        End If
                        'se non ci sono righe chiudo la funzione
                        ControllaStatoEntePerBloccareMaschereAnagrafica = True
                        Return ControllaStatoEntePerBloccareMaschereAnagrafica
                    Else
                        'chiudo il datareader
                        If Not dtrControlloEnte Is Nothing Then
                            dtrControlloEnte.Close()
                            dtrControlloEnte = Nothing
                        End If
                        'vado a controllare il la data odierna
                        Strsql = "select StatoEnte from statienti inner join enti on statienti.idstatoente=enti.idstatoente where enti.idente='" & strIdEnte & "'"

                        'eseguo la query e la passo al datareader locale
                        dtrControlloEnte = ClsServer.CreaDatareader(Strsql, conn)

                        'controllo se ho righe
                        If dtrControlloEnte.HasRows = True Then
                            'leggo il risultato
                            dtrControlloEnte.Read()
                            'controllo lo stato dell'ente
                            'se Attivo o Registrato NON blocco la pagina
                            'altrimenti BLOCCO la maschera
                            'If dtrControlloEnte("StatoEnte") <> ("Attivo" And "Registrato") Then
                            If dtrControlloEnte("StatoEnte") <> "Attivo" And dtrControlloEnte("StatoEnte") <> "Registrato" Then
                                'chiudo il datareader
                                If Not dtrControlloEnte Is Nothing Then
                                    dtrControlloEnte.Close()
                                    dtrControlloEnte = Nothing
                                End If
                                'se lo stato dell'ente Ë diverso da attivo e registrato
                                ControllaStatoEntePerBloccareMaschereAnagrafica = True
                                Return ControllaStatoEntePerBloccareMaschereAnagrafica
                            End If
                        End If

                        'chiudo il datareader
                        If Not dtrControlloEnte Is Nothing Then
                            dtrControlloEnte.Close()
                            dtrControlloEnte = Nothing
                        End If
                    End If
                Case Else
                    'chiudo il datareader
                    If Not dtrControlloEnte Is Nothing Then
                        dtrControlloEnte.Close()
                        dtrControlloEnte = Nothing
                    End If
            End Select
        End If

        'chiudo il datareader
        If Not dtrControlloEnte Is Nothing Then
            dtrControlloEnte.Close()
            dtrControlloEnte = Nothing
        End If

    End Function

    'funzione creata da jonarnold schwarzenegger il 31.01.2007
    'controllo se il cap che si ste inserendo per il comune selezionato esiste
    'TRUE ESISTE
    'FALSE NON ESISTE
    Public Shared Function ControllaEsistenzaCap(ByVal conn As SqlClient.SqlConnection, ByVal strCap As String, Optional ByVal strIdComune As String = "", Optional ByVal strDenominazione As String = "") As Boolean
        ', Optional ByVal strIdComune As String = "", Optional ByVal strDenominazione As String = ""
        Dim Strsql As String
        Dim dtrCAP As SqlClient.SqlDataReader


        'se se tratta di estero non controllo il cap
        Strsql = "SELECT Nazioni.NazioneBase, isnull(Comuni.cf,'') as CodCat FROM Nazioni " & _
                  "INNER JOIN Regioni ON Regioni.IdNazione = Nazioni.IdNazione " & _
                  "INNER JOIN Provincie ON Provincie.IdRegione = Regioni.IdRegione " & _
                  "INNER JOIN Comuni ON Comuni.IdProvincia = Provincie.IdProvincia "
        If strIdComune <> "" Then
            Strsql = Strsql & "WHERE Comuni.IdComune='" & strIdComune & "'"
        Else
            Strsql = Strsql & "WHERE Comuni.denominazione='" & Replace(strDenominazione, "'", "''") & "' "
        End If
        'chiudo il datareader
        If Not dtrCAP Is Nothing Then
            dtrCAP.Close()
            dtrCAP = Nothing
        End If
        dtrCAP = ClsServer.CreaDatareader(Strsql, conn)

        dtrCAP.Read()
        If dtrCAP.Item("NazioneBase") = True Then          'italia allora verifico il cap

            If Not dtrCAP Is Nothing Then
                dtrCAP.Close()
                dtrCAP = Nothing
            End If

            Strsql = "SELECT VW_CAPCOMUNI.CAP from VW_CAPCOMUNI "
            If strIdComune <> "" Then
                Strsql = Strsql & "WHERE VW_CAPCOMUNI.IdComune='" & strIdComune & "' AND "
            Else
                Strsql = Strsql & "INNER JOIN comuni ON comuni.idcomune=VW_CAPCOMUNI.idcomune "
                Strsql = Strsql & "WHERE comuni.denominazione='" & Replace(strDenominazione, "'", "''") & "' AND "
            End If
            'modificata da SPAGNULO IL 08/03/2008 (DA ERRORE NEL CONFRONTO CON STRINGHE ALFABETICHE Strsql = Strsql & "convert(int,VW_CAPCOMUNI.CAP) =" & strCap
            If IsNumeric(strCap) Then
                Strsql = Strsql & "convert(int,VW_CAPCOMUNI.CAP) =" & strCap
            Else
                Strsql = Strsql & "VW_CAPCOMUNI.CAP ='" & strCap & "'"
            End If

            'chiudo il datareader
            If Not dtrCAP Is Nothing Then
                dtrCAP.Close()
                dtrCAP = Nothing
            End If

            dtrCAP = ClsServer.CreaDatareader(Strsql, conn)

            'controllo se ci sono record
            ControllaEsistenzaCap = dtrCAP.HasRows
        Else
            If Not dtrCAP Is Nothing Then
                dtrCAP.Close()
                dtrCAP = Nothing
            End If
            Return True
        End If

        'chiudo il datareader
        If Not dtrCAP Is Nothing Then
            dtrCAP.Close()
            dtrCAP = Nothing
        End If

        Return ControllaEsistenzaCap

    End Function

    'funzione che verifica se il CAP del comune selezionato fa parte 
    'di un gruppo di MULTI CAP 
    'e vado a verificare se quello inserito abbia la terza cifra
    'uguale a quello d'origine
    'true = il controllo Ë andato a buon fine
    'false = il controllo non Ë andato a buon fine
    Public Shared Function ControllaMultiCAP(ByVal conn As SqlClient.SqlConnection, ByVal strCapNuovo As String, Optional ByVal strIdComune As String = "", Optional ByVal strDenominazione As String = "") As Boolean
        'Variabile che prende il cap lungo 5 caratteri
        Dim strCAPLungo5 As String
        'generata da arthur jonzarelli
        'il 24.03.06
        Dim Strsql As String
        Dim dtrMultiCAP As SqlClient.SqlDataReader
        'stringa che conterr‡ la terza cifra del nuovo CAP
        Dim strTerzaCifraNuovoCAP As String
        'variabile che conterr‡ il CAP della citt‡ che si vuole inserire preso dal DB
        Dim strDataBaseCAP As String
        'stringa che conterr‡ la terza cifra del CAP preso da DATABASE
        Dim strTerzaCifraCAPDataBase As String

        'faccio la ricerca del comune per id oppure per denominazione, a seconda se vengo da importazione
        If strIdComune <> "" Then
            'Strsql = "select isnull(CAP,'') as CAP, isnull(MultiCap,0) as MultiCap from comuni where idcomune='" & strIdComune & "'"
            Strsql = "SELECT ISNULL(comuni.CAP, '') AS CAP, ISNULL(comuni.MultiCAP, 0) AS MultiCap, nazioni.NazioneBase " _
                   & " FROM provincie INNER JOIN " _
                   & " comuni ON provincie.IDProvincia = comuni.IDProvincia INNER JOIN " _
                   & " regioni ON provincie.IDRegione = regioni.IDRegione INNER JOIN " _
                   & " nazioni ON regioni.IDNazione = nazioni.IDNazione " _
                   & " WHERE (comuni.idcomune = '" & strIdComune & "')"
        Else
            'Strsql = "select isnull(CAP,'') as CAP, isnull(MultiCap,0) as MultiCap from comuni where denominazione='" & Replace(strDenominazione, "'", "''") & "'"
            Strsql = "SELECT ISNULL(comuni.CAP, '') AS CAP, ISNULL(comuni.MultiCAP, 0) AS MultiCap, nazioni.NazioneBase " _
                   & " FROM provincie INNER JOIN " _
                   & " comuni ON provincie.IDProvincia = comuni.IDProvincia INNER JOIN " _
                   & " regioni ON provincie.IDRegione = regioni.IDRegione INNER JOIN " _
                   & " nazioni ON regioni.IDNazione = nazioni.IDNazione " _
                   & " WHERE isnull(comuni.codiceistat,'') <> '' And (comuni.denominazione='" & Replace(strDenominazione, "'", "''") & "')"
        End If

        'chiudo il datareader
        If Not dtrMultiCAP Is Nothing Then
            dtrMultiCAP.Close()
            dtrMultiCAP = Nothing
        End If

        dtrMultiCAP = ClsServer.CreaDatareader(Strsql, conn)

        'controllo se ci sono record
        If dtrMultiCAP.HasRows = True Then
            'leggo il datareader
            dtrMultiCAP.Read()
            If dtrMultiCAP("NazioneBase") = True Then         'se la nazione base Ë Italia faccio il controllo del CAP
                'controllo se si tratta di un comune con MultiCAP = dtr("MultiCap") = true
                If dtrMultiCAP("MultiCap") = True Then
                    '*************************************************************************************************
                    'vado a controllare se la terza cifra del nuovo cap sia la stessa del comune che si vuole inserire
                    'ad. es. 00100 quello di nel DB, 00356 quello che si vuole inserire, cosÏ non va bene
                    '*************************************************************************************************

                    'estrapolo la terza cifra dal CAP del comune preso dal DB
                    strTerzaCifraCAPDataBase = Mid(dtrMultiCAP("CAP"), 3, 1)
                    'controllo la lunghezza del cap che si vuole inserire, questo perchË dal csv mi arriva senza 0 davanti
                    Select Case Len(strCapNuovo)
                        Case 1
                            strCAPLungo5 = "0000" & strCapNuovo
                        Case 2
                            strCAPLungo5 = "000" & strCapNuovo
                        Case 3
                            strCAPLungo5 = "00" & strCapNuovo
                        Case 4
                            strCAPLungo5 = "0" & strCapNuovo
                        Case Is >= 5
                            strCAPLungo5 = strCapNuovo
                    End Select

                    'e quella del cap che si vuole inserire
                    strTerzaCifraNuovoCAP = Mid(strCAPLungo5, 3, 1)

                    'se la terza cifra del cap che si vuole inserire Ë diversa da quella del database
                    'blocco la funzione
                    'altrimenti procedo con i controlli
                    If strTerzaCifraCAPDataBase <> strTerzaCifraNuovoCAP Then
                        'terza cifra diversa
                        ControllaMultiCAP = False

                    Else
                        'terza cifra uguale
                        ControllaMultiCAP = True

                    End If
                Else

                    Select Case Len(strCapNuovo)
                        Case 1
                            strCapNuovo = "0000" & strCapNuovo
                        Case 2
                            strCapNuovo = "000" & strCapNuovo
                        Case 3
                            strCapNuovo = "00" & strCapNuovo
                        Case 4
                            strCapNuovo = "0" & strCapNuovo
                        Case Is >= 5
                            strCapNuovo = strCapNuovo
                    End Select

                    'vado a controllare se l'utente ha inserito un CAP valido 
                    If dtrMultiCAP("CAP") <> strCapNuovo Then
                        'se il cap che si vuole inserire risulta diverso da quello del db blocco la funzione
                        ControllaMultiCAP = False

                    Else
                        'se il cap che si vuole inserire Ë lo stesso del db, lascio che il flusso continui
                        ControllaMultiCAP = True

                    End If
                End If
            Else
                'se nn c'Ë il cap passo true perchË Ë probabilmente estero
                ControllaMultiCAP = True
            End If
        Else                'NAZIONE ESTERA
            ControllaMultiCAP = True
        End If

        'chiudo il datareader
        If Not dtrMultiCAP Is Nothing Then
            dtrMultiCAP.Close()
            dtrMultiCAP = Nothing
        End If

    End Function

    Public Shared Function GeneraPasswordVolontari() As String
        Dim Minuscole As String
        Dim Maiuscole As String
        Dim Numeri As String

        Dim password As String

        Dim originepassword As String

        Dim Appoggiomin As String
        Dim Appoggiomin1 As String
        Dim AppoggioMaiu As String
        Dim AppoggioMaiu1 As String
        Dim AppoggioNum As String
        Dim AppoggioNum1 As String
        Dim Appoggio As String
        Dim Appoggio1 As String

        Minuscole = "abcdefghijkmnpqrstuvwxyz"
        Maiuscole = "ABCDEFGHJKLMNPQRSTUVWXYZ"
        Numeri = "23456789"
        Randomize()

        Appoggiomin = CInt(Int((Minuscole.Length) * Rnd() + 1))
        Appoggiomin1 = CInt(Int((Minuscole.Length) * Rnd() + 1))
        Appoggiomin = Mid(Minuscole, Appoggiomin, 1)
        Appoggiomin1 = Mid(Minuscole, Appoggiomin1, 1)

        AppoggioMaiu = CInt(Int((Maiuscole.Length) * Rnd() + 1))
        AppoggioMaiu1 = CInt(Int((Maiuscole.Length) * Rnd() + 1))
        AppoggioMaiu = Mid(Maiuscole, AppoggioMaiu, 1)
        AppoggioMaiu1 = Mid(Maiuscole, AppoggioMaiu1, 1)

        AppoggioNum = CInt(Int((Numeri.Length) * Rnd() + 1))
        AppoggioNum1 = CInt(Int((Numeri.Length) * Rnd() + 1))
        AppoggioNum = Mid(Numeri, AppoggioNum, 1)
        AppoggioNum1 = Mid(Numeri, AppoggioNum1, 1)

        Appoggio = CInt(Int((Minuscole.Length) * Rnd() + 1))
        Appoggio1 = CInt(Int((Minuscole.Length) * Rnd() + 1))
        Appoggio = Mid(Minuscole, Appoggio, 1)
        Appoggio1 = Mid(Minuscole, Appoggio1, 1)

        password = Appoggiomin & Appoggiomin1 & AppoggioMaiu & AppoggioMaiu1 & AppoggioNum & AppoggioNum1 & Appoggio & Appoggio1
        originepassword = password
        Dim X As Integer
        Dim DESTINAZIONE As String
        DESTINAZIONE = ""

        Do While Len(password) <> 0
            If Len(password) = 1 Then
                DESTINAZIONE = DESTINAZIONE + password
                password = ""
            Else
                X = Len(password) * Rnd()
                DESTINAZIONE = DESTINAZIONE + Mid(password, IIf(X = 0, 1, X), 1)
                password = Left(password, IIf(X = 0, 1, X) - 1) + Mid(password, IIf(X = 0, 1, X) + 1, Len(password) - IIf(X = 0, 1, X))
            End If
        Loop
        GeneraPasswordVolontari = DESTINAZIONE
    End Function

    Public Shared Function NuovaPass() As String
        'fisso come base d'inizio un carattere e successivamente aggiungo 
        'come ultimo carattere una cifra
        'variabile che conterr‡ il codice ASCII che costruir‡ la nuova pass.
        Dim MyValue As Integer
        'Variabile che conterr‡ la nuova password
        Dim NewPass As String
        'variabile contatore
        Dim intX As Integer
        Randomize() ' lancio il generatore di numeri casuale
        'primo carattere
        MyValue = CInt(Int((122 - 97 + 1) * Rnd() + 97)) ' genero un codice tra 97 e 122 (a - z)
        NewPass = Chr(MyValue).ToString
        For intX = 1 To 6
            MyValue = CInt(Int((2 - 1 + 1) * Rnd() + 1)) ' genero un codice tra 1 e 2 perchË poi randomizzerÚ o un numero o una lettera
            If CInt(MyValue) = 1 Then ' genero un numero
                MyValue = CInt(Int((57 - 48 + 1) * Rnd() + 48))  ' genero un codice tra 30 e 39 (0 - 9)
                NewPass = NewPass & Chr(MyValue).ToString
            Else ' genero un carattere
                MyValue = CInt(Int((122 - 97 + 1) * Rnd() + 97)) ' genero un codice tra 97 e 122 (a - z)
                NewPass = NewPass & Chr(MyValue).ToString
            End If
        Next
        MyValue = CInt(Int((57 - 48 + 1) * Rnd() + 48)) ' genero un codice tra 97 e 122 (a - z)
        NewPass = NewPass & Chr(MyValue).ToString
        NuovaPass = NewPass
    End Function

    Public Shared Function ReadPsw(ByVal cPsw As String) As String
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
    Public Shared Function MinSedi(ByVal idente As Double, ByVal conn As SqlClient.SqlConnection) As Integer
        Dim Strsql As String
        Dim dtrgenerico As SqlClient.SqlDataReader
        Strsql = "Select enti.idente,enti.idclasseaccreditamentorichiesta," & _
               " classiaccreditamento.minsedi, classiaccreditamento.maxsedi " & _
               " from enti " & _
               " inner join classiaccreditamento on " & _
               " (classiaccreditamento.idclasseaccreditamento = enti.idclasseaccreditamentorichiesta)" & _
               " where enti.idente=" & idente & ""
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
        dtrgenerico = ClsServer.CreaDatareader(Strsql, conn)
        dtrgenerico.Read()
        MinSedi = dtrgenerico("minsedi")
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
    End Function
    Public Shared Function MaxSedi(ByVal idente As Double, ByVal conn As SqlClient.SqlConnection) As Integer
        Dim Strsql As String
        Dim dtrgenerico As SqlClient.SqlDataReader
        Strsql = "Select enti.idente,enti.idclasseaccreditamentorichiesta," & _
               " classiaccreditamento.minsedi, classiaccreditamento.maxsedi " & _
               " from enti " & _
               " inner join classiaccreditamento on " & _
               " (classiaccreditamento.idclasseaccreditamento = enti.idclasseaccreditamentorichiesta)" & _
               " where enti.idente=" & idente & ""
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
        dtrgenerico = ClsServer.CreaDatareader(Strsql, conn)
        dtrgenerico.Read()
        MaxSedi = dtrgenerico("maxsedi")
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
    End Function
    Public Shared Function CreatePSW(ByVal cPsw As String) As String
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
    Public Shared Function CheckEmail(ByVal str)
        ' Dichiarazione delle variabili
        Dim i, j, first, last, strChar
        ' Controlli di tipo dimensionale
        ' posizione del carattere '@'
        i = InStr(str, "@")
        If (i > 0) And (i < Len(str)) Then
            ' testo che precede '@'
            first = Left(str, i - 1)
            ' testo che segue '@'
            last = Mid(str, i + 1, Len(str))
        Else
            CheckEmail = False
            Exit Function
        End If
        ' almeno 1 carattere PRIMA di '@'
        If (Len(first) = 0) Then
            CheckEmail = False
            Exit Function
        End If
        ' posizione dell'ultimo carattere '.'
        i = InStrRev(last, ".")
        ' almeno 1 carattere DOPO '@' e PRIMA dell'ultimo '.'
        If Len(Mid(last, 1, i)) = 0 Then
            CheckEmail = False
            Exit Function
        End If
        ' suffisso finale Ë lungo 2 o 3 caratteri
        j = Len(last) - i
        If (j <= 1) Or (j >= 4) Then
            CheckEmail = False
            Exit Function
        End If
        ' Controlla i caratteri PRIMA di '@'
        i = 0
        Do Until (i = Len(first))
            i = i + 1
            strChar = Mid(first, i, 1)
            ' caratteri leciti sono [a-zA-Z0-9_.-]
            If (Asc(strChar) <> 45) And (Asc(strChar) <> 46) And _
              (Asc(strChar) < 48 Or Asc(strChar) > 57) And _
              (Asc(strChar) < 65 Or Asc(strChar) > 90) And _
              (Asc(strChar) <> 95) And _
              (Asc(strChar) < 97 Or Asc(strChar) > 122) Then
                CheckEmail = False
                Exit Function
            End If
        Loop
        ' Controlla i caratteri DOPO la '@'
        i = 0
        Do Until i = Len(last)
            i = i + 1
            strChar = Mid(last, i, 1)
            ' caratteri leciti sono [a-zA-Z0-9_.-]
            If (Asc(strChar) <> 45) And (Asc(strChar) <> 46) And _
              (Asc(strChar) < 48 Or Asc(strChar) > 57) And _
              (Asc(strChar) < 65 Or Asc(strChar) > 90) And _
              (Asc(strChar) <> 95) And _
              (Asc(strChar) < 97 Or Asc(strChar) > 122) Then
                CheckEmail = False
                Exit Function
            End If
        Loop
        ' Email adesso Ë valida...
        ' dopo tutti questi controlli !!!
        CheckEmail = True
    End Function
    Public Shared Function IsValidEmail(ByVal strIn As String) As Boolean
        ' true se il formato email Ë valido.
        Return Regex.IsMatch(strIn, ("^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$"))
    End Function

    Public Shared Sub invioEmail(ByVal strFrom As String, ByVal strTo As String, ByVal strCC As String, ByVal strSubject As String, ByVal strTesto As String)
        'Dim ObjSMTP As System.Web.Mail.SmtpMail
        'Dim myMail As New System.Web.Mail.MailMessage
        'myMail.From = strFrom
        'myMail.To = strTo
        'myMail.Cc = strCC
        'myMail.Subject = strSubject
        'myMail.BodyFormat = System.Web.Mail.MailFormat.Html
        'myMail.Body = strTesto
        ''ObjSMTP.SmtpServer = "smtp.fastweb.it"
        'ObjSMTP.SmtpServer = "smtp.serviziocivile.it"
        'ObjSMTP.Send(myMail)

        Dim mailMessage As New MailMessage
        mailMessage.From = strFrom
        mailMessage.To = strTo
        mailMessage.Cc = strCC
        mailMessage.Subject = strSubject
        mailMessage.Body = strTesto
        mailMessage.BodyFormat = MailFormat.Html
        'mailMessage.BodyEncoding = System.Text.Encoding.UTF8
        SmtpMail.SmtpServer = "smtp.serviziocivile.it"
        SmtpMail.Send(mailMessage)
    End Sub

    Public Shared Function FormatExport(ByVal pStringa As String) As String
        pStringa = Replace(pStringa, vbCrLf, " ")
        If InStr(pStringa, ";") > 0 Then
            pStringa = Chr(34) & pStringa & Chr(34)
        End If
        FormatExport = pStringa

    End Function

    'Funzione creata da Amilcare Paolella il 4/4/2006;
    'Gestione della cronologia dei documenti generati per il volontario.
    Public Shared Function CronologiaDocEntit‡(ByVal myIdEntit‡ As String, ByVal myUserName As String, ByVal myNomeDocu As String, ByVal myConn As SqlClient.SqlConnection, Optional ByRef myDataProt As String = "", Optional ByRef myNumProt As String = "")
        Dim strSql As String
        strSql = "INSERT INTO CronologiaEntit‡Documenti (IdEntit‡, UserName, DataDocumento, Documento,DataProt,NProt) " & _
                 "VALUES (" & myIdEntit‡ & ",'" & myUserName & "',getdate(),'" & myNomeDocu & "',"
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
    'Funzione creata da FZ il 13/9/2006;
    'controllo per disabilitare la maschera nel caso sia un'"R" che sta 
    'controllando i progetti di un' ente che nn ha la stessa regiojneee di competenza
    'La funzione restituisce TRUE se l'ente REGIONE puo' gestire i dati visualizzati,
    'FALSE in caso contrario... il blocco della maschera viene gestito dalla pagina stessa
    Public Shared Function ControlloRegioneCompetenza(ByVal TipoUtente As String, ByVal idEnte As String, ByVal Utente As String, ByVal IdStatoEnte As String, ByVal conn As SqlClient.SqlConnection, Optional ByVal VengoDaAlbero As String = "") As Boolean
        ControlloRegioneCompetenza = True

        Try
            If TipoUtente = "R" Then

                Dim StrREgioneEnte As String = ""
                Dim StrREgione As String = ""
                Dim StrSql As String
                Dim dtrgenerico As SqlClient.SqlDataReader
                Dim AbilitatoReadRegione As Boolean
                'Controllo il codice regione di competenza dell'ente selezionato
                StrSql = "SELECT RegioniCompetenze.IdRegioneCompetenza " & _
                         "FROM enti LEFT OUTER JOIN RegioniCompetenze " & _
                         "ON enti.IdRegioneCompetenza = RegioniCompetenze.IdRegioneCompetenza " & _
                         "WHERE enti.IDEnte = " & idEnte
                dtrgenerico = ClsServer.CreaDatareader(StrSql, conn)

                If dtrgenerico.HasRows = True Then
                    dtrgenerico.Read()
                    StrREgioneEnte = "" & dtrgenerico("IdRegioneCompetenza")

                End If
                dtrgenerico.Close()
                dtrgenerico = Nothing

                'Controllo il codice regione di competenza della regione loggata
                StrSql = "SELECT UtentiUNSC.UserName, UtentiUNSC.HeliosRead, RegioniCompetenze.IdRegioneCompetenza " & _
                         "FROM UtentiUNSC INNER JOIN RegioniCompetenze  " & _
                         "ON UtentiUNSC.IdRegioneCompetenza = RegioniCompetenze.IdRegioneCompetenza " & _
                         "WHERE UtentiUNSC.UserName = '" & Utente & "'"



                dtrgenerico = ClsServer.CreaDatareader(StrSql, conn) 'IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))

                If dtrgenerico.HasRows = True Then
                    dtrgenerico.Read()
                    StrREgione = "" & dtrgenerico("IdRegioneCompetenza")
                    AbilitatoReadRegione = dtrgenerico("HeliosRead")
                End If


                If VengoDaAlbero = "" Then

                    If StrREgioneEnte <> StrREgione Then
                        ControlloRegioneCompetenza = False
                    Else
                        ControlloRegioneCompetenza = True
                    End If

                Else 'Abilito le regioni READ a visualizzare tutto VengoDaAlbero=true
                    If StrREgioneEnte <> StrREgione Then
                        If AbilitatoReadRegione = True Then
                            ControlloRegioneCompetenza = True
                        Else
                            ControlloRegioneCompetenza = False
                        End If
                    Else
                        ControlloRegioneCompetenza = True
                    End If
                End If
                dtrgenerico.Close()
                dtrgenerico = Nothing
            End If

        Catch ex As Exception

        End Try

    End Function

    Public Shared Function CreaCF(ByVal pCognome As String, ByVal pNome As String, ByVal pDataNascita As String, ByVal pCodCatasto As String, ByVal pSesso As String) As String
        Dim i As Integer
        Dim TutteLeLettere As String = "ABCDEFGHIJKLMNOPQRSTUVWXYZ"
        Dim TuttiINumeri As String = "0123456789"
        Dim TuttiGliOmocodici As String = "LMNPQRSTUV"
        Dim TutteLeVocali As String = "AEIOU"
        Dim TutteLeConsonanti As String = "BCDFGHJKLMNPQRSTVWXYZ"
        Dim CodMese As String = "ABCDEHLMPRST"
        Dim Vocali As String
        Dim Consonanti As String
        Dim sNewCF As String
        Dim tmpGiornoNascitaM As Integer
        Dim tmpGiornoNascitaF As Integer
        Dim tmpValore As String

        '--- Controllo Cognome
        pCognome = UCase(pCognome)
        For i = 1 To Len(pCognome)
            If InStr(TutteLeVocali, Mid(pCognome, i, 1)) > 0 Then
                Vocali = Vocali + Mid(pCognome, i, 1)
            Else
                If InStr(TutteLeConsonanti, Mid(pCognome, i, 1)) > 0 Then
                    Consonanti = Consonanti + Mid(pCognome, i, 1)
                End If
            End If
            If Len(Consonanti) = 3 Then
                Exit For
            End If
        Next i
        If Len(Consonanti) < 3 Then
            Consonanti = Consonanti + Mid(Vocali, 1, 3 - Len(Consonanti))
            For i = Len(Consonanti) + 1 To 3
                Consonanti = Consonanti & "X"
            Next i
        End If
        sNewCF = Consonanti
        '----------------------------------------------------------------------------------------------------
        '--- Controllo Nome
        Consonanti = vbNullString
        Vocali = vbNullString
        pNome = UCase(pNome)
        For i = 1 To Len(pNome)
            If InStr(TutteLeVocali, Mid(pNome, i, 1)) > 0 Then
                Vocali = Vocali + Mid(pNome, i, 1)
            Else
                If InStr(TutteLeConsonanti, Mid(pNome, i, 1)) > 0 Then
                    Consonanti = Consonanti + Mid(pNome, i, 1)
                End If
            End If
        Next i

        If Len(Consonanti) >= 4 Then
            Consonanti = Mid(Consonanti, 1, 1) + Mid(Consonanti, 3, 2)
        Else
            If Len(Consonanti) < 3 Then
                Consonanti = Consonanti + Mid(Vocali, 1, 3 - Len(Consonanti))
                For i = Len(Consonanti) + 1 To 3
                    Consonanti = Consonanti & "X"
                Next i
            End If
        End If
        sNewCF = sNewCF & Consonanti
        '----------------------------------------------------------------------------------------------------
        '--- Controllo Anno	
        tmpValore = DecodificaOmocodici(Mid(pDataNascita, 9, 1)) & DecodificaOmocodici(Mid(pDataNascita, 10, 1))
        sNewCF = sNewCF & tmpValore
        '----------------------------------------------------------------------------------------------------
        '--- Controllo Mese	
        Select Case Mid(pDataNascita, 4, 2)
            Case "01"
                sNewCF = sNewCF & "A"
            Case "02"
                sNewCF = sNewCF & "B"
            Case "03"
                sNewCF = sNewCF & "C"
            Case "04"
                sNewCF = sNewCF & "D"
            Case "05"
                sNewCF = sNewCF & "E"
            Case "06"
                sNewCF = sNewCF & "H"
            Case "07"
                sNewCF = sNewCF & "L"
            Case "08"
                sNewCF = sNewCF & "M"
            Case "09"
                sNewCF = sNewCF & "P"
            Case "10"
                sNewCF = sNewCF & "R"
            Case "11"
                sNewCF = sNewCF & "S"
            Case "12"
                sNewCF = sNewCF & "T"
        End Select
        '----------------------------------------------------------------------------------------------------      
        If UCase(pSesso) = "F" Then          'donna
            sNewCF = sNewCF & Mid(pDataNascita, 1, 2) + 40
        Else                    'uomo
            sNewCF = sNewCF & Mid(pDataNascita, 1, 2)
        End If
        '----------------------------------------------------------------------------------------------------      
        'catasto
        sNewCF = sNewCF & pCodCatasto.Trim
        '----------------------------------------------------------------------------------------------------      
        sNewCF = sNewCF & GetCarattereControllo(sNewCF)

        Return sNewCF

    End Function

    Public Shared Function GetCarattereControllo(ByVal CodiceFiscale As String) As String

        Dim Lettere(35, 2)

        Dim ConfrontoCarattereControllo(25)

        Dim I
        Dim J

        Dim Carattere
        Dim ValorePari
        Dim ValoreDispari
        Dim SommaCaratteri
        Dim PariDispari
        Dim Risultato

        Dim CarattereControllo
        Dim Temp
        Dim Test

        Try

            Lettere(0, 0) = "A"
            Lettere(0, 1) = "0"
            Lettere(0, 2) = "1"

            Lettere(1, 0) = "B"
            Lettere(1, 1) = "1"
            Lettere(1, 2) = "0"

            Lettere(2, 0) = "C"
            Lettere(2, 1) = "2"
            Lettere(2, 2) = "5"

            Lettere(3, 0) = "D"
            Lettere(3, 1) = "3"
            Lettere(3, 2) = "7"

            Lettere(4, 0) = "E"
            Lettere(4, 1) = "4"
            Lettere(4, 2) = "9"

            Lettere(5, 0) = "F"
            Lettere(5, 1) = "5"
            Lettere(5, 2) = "13"

            Lettere(6, 0) = "G"
            Lettere(6, 1) = "6"
            Lettere(6, 2) = "15"

            Lettere(7, 0) = "H"
            Lettere(7, 1) = "7"
            Lettere(7, 2) = "17"

            Lettere(8, 0) = "I"
            Lettere(8, 1) = "8"
            Lettere(8, 2) = "19"

            Lettere(9, 0) = "J"
            Lettere(9, 1) = "9"
            Lettere(9, 2) = "21"

            Lettere(10, 0) = "K"
            Lettere(10, 1) = "10"
            Lettere(10, 2) = "2"

            Lettere(11, 0) = "L"
            Lettere(11, 1) = "11"
            Lettere(11, 2) = "4"

            Lettere(12, 0) = "M"
            Lettere(12, 1) = "12"
            Lettere(12, 2) = "18"

            Lettere(13, 0) = "N"
            Lettere(13, 1) = "13"
            Lettere(13, 2) = "20"

            Lettere(14, 0) = "O"
            Lettere(14, 1) = "14"
            Lettere(14, 2) = "11"

            Lettere(15, 0) = "P"
            Lettere(15, 1) = "15"
            Lettere(15, 2) = "3"

            Lettere(16, 0) = "Q"
            Lettere(16, 1) = "16"
            Lettere(16, 2) = "6"

            Lettere(17, 0) = "R"
            Lettere(17, 1) = "17"
            Lettere(17, 2) = "8"

            Lettere(18, 0) = "S"
            Lettere(18, 1) = "18"
            Lettere(18, 2) = "12"

            Lettere(19, 0) = "T"
            Lettere(19, 1) = "19"
            Lettere(19, 2) = "14"

            Lettere(20, 0) = "U"
            Lettere(20, 1) = "20"
            Lettere(20, 2) = "16"

            Lettere(21, 0) = "V"
            Lettere(21, 1) = "21"
            Lettere(21, 2) = "10"

            Lettere(22, 0) = "W"
            Lettere(22, 1) = "22"
            Lettere(22, 2) = "22"

            Lettere(23, 0) = "X"
            Lettere(23, 1) = "23"
            Lettere(23, 2) = "25"

            Lettere(24, 0) = "Y"
            Lettere(24, 1) = "24"
            Lettere(24, 2) = "24"

            Lettere(25, 0) = "Z"
            Lettere(25, 1) = "25"
            Lettere(25, 2) = "23"

            Lettere(26, 0) = "0"
            Lettere(26, 1) = "0"
            Lettere(26, 2) = "1"

            Lettere(27, 0) = "1"
            Lettere(27, 1) = "1"
            Lettere(27, 2) = "0"

            Lettere(28, 0) = "2"
            Lettere(28, 1) = "2"
            Lettere(28, 2) = "5"

            Lettere(29, 0) = "3"
            Lettere(29, 1) = "3"
            Lettere(29, 2) = "7"

            Lettere(30, 0) = "4"
            Lettere(30, 1) = "4"
            Lettere(30, 2) = "9"

            Lettere(31, 0) = "5"
            Lettere(31, 1) = "5"
            Lettere(31, 2) = "13"

            Lettere(32, 0) = "6"
            Lettere(32, 1) = "6"
            Lettere(32, 2) = "15"

            Lettere(33, 0) = "7"
            Lettere(33, 1) = "7"
            Lettere(33, 2) = "17"

            Lettere(34, 0) = "8"
            Lettere(34, 1) = "8"
            Lettere(34, 2) = "19"

            Lettere(35, 0) = "9"
            Lettere(35, 1) = "9"
            Lettere(35, 2) = "21"

            For I = 0 To 25

                ConfrontoCarattereControllo(I) = Chr(65 + I) 'creo in ConfrontoCarattereControllo tutte le lettere maiuscole dalla A (chr(65)) alla Z(chr(90))

            Next

            Carattere = 0
            ValorePari = 1 'indice della seconda colonna della matrice Lettere
            ValoreDispari = 2 'indice della terza colonna della matrice Lettere
            SommaCaratteri = 0


            For I = 1 To Len(CodiceFiscale)
                If (I Mod 2) = 0 Then
                    PariDispari = "P"
                Else
                    PariDispari = "D"
                End If

                Temp = Mid(CodiceFiscale, I, 1)
                J = 0
                Do
                    Test = Lettere(J, Carattere)
                    J = J + 1
                Loop Until Temp = Test
                J = J - 1

                If PariDispari = "P" Then
                    SommaCaratteri = SommaCaratteri + CInt(Lettere(J, ValorePari))
                Else
                    SommaCaratteri = SommaCaratteri + CInt(Lettere(J, ValoreDispari))
                End If
            Next

            Risultato = SommaCaratteri Mod 26

            Risultato = ConfrontoCarattereControllo(Risultato)


        Catch ex As Exception

            Risultato = ""

        End Try

        Return Risultato


    End Function
  
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

    Public Shared Function GetCodiceCatasto(ByVal conn As SqlClient.SqlConnection, ByVal pComune As Integer) As String
        Dim strsql As String
        Dim strError As String
        Dim myReader As SqlClient.SqlDataReader
        Dim pCodCatasto As String

        Try

            strsql = "SELECT Nazioni.NazioneBase, isnull(Comuni.cf,'') as CodCat FROM Nazioni " & _
                    "INNER JOIN Regioni ON Regioni.IdNazione = Nazioni.IdNazione " & _
                    "INNER JOIN Provincie ON Provincie.IdRegione = Regioni.IdRegione " & _
                    "INNER JOIN Comuni ON Comuni.IdProvincia = Provincie.IdProvincia " & _
                    "WHERE Comuni.IdComune = " & pComune & " AND (ISNULL(comuni.CodiceISTAT, '') <> '' OR ISNULL(comuni.CodiceISTATDismesso, '') <> '')"


            myReader = ClsServer.CreaDatareader(strsql, conn)

            myReader.Read()
            If myReader.HasRows = False Then
                pCodCatasto = ""
            Else
                pCodCatasto = Trim(myReader("CodCat"))      'codice catastale
            End If

            myReader.Close()
            myReader = Nothing

            Return pCodCatasto

        Catch ex As Exception
            strError = ex.Message
        End Try
    End Function
    Public Shared Function VerificaOmocodia(ByVal sNewCF As String, ByVal sOldCF As String) As Boolean
        Dim sLettereOmo(9) As String
        Dim i As Integer
        Dim bolEsito As Boolean
        Dim strCarattere1 As String
        Dim strCarattere2 As String
        Dim strCarattere3 As String
        Dim strCarattere4 As String
        Dim strCarattere5 As String
        Dim strCarattere6 As String
        Dim strCarattere7 As String
        Dim strNumero1 As Integer = -1
        Dim strNumero2 As Integer = -1
        Dim strNumero3 As Integer = -1
        Dim strNumero4 As Integer = -1
        Dim strNumero5 As Integer = -1
        Dim strNumero6 As Integer = -1
        Dim strNumero7 As Integer = -1
        Dim strAppoCF As String
        Dim bolSostituzione As Boolean

        If GetCarattereControllo(Mid(sOldCF, 1, 15)) <> Mid(sOldCF, 16, 1) Then
            Return False
        End If

        sLettereOmo(0) = "L"
        sLettereOmo(1) = "M"
        sLettereOmo(2) = "N"
        sLettereOmo(3) = "P"
        sLettereOmo(4) = "Q"
        sLettereOmo(5) = "R"
        sLettereOmo(6) = "S"
        sLettereOmo(7) = "T"
        sLettereOmo(8) = "U"
        sLettereOmo(9) = "V"

        bolEsito = False
        bolSostituzione = False


        strCarattere1 = Mid(sOldCF, 7, 1)
        strCarattere2 = Mid(sOldCF, 8, 1)
        strCarattere3 = Mid(sOldCF, 10, 1)
        strCarattere4 = Mid(sOldCF, 11, 1)
        strCarattere5 = Mid(sOldCF, 13, 1)
        strCarattere6 = Mid(sOldCF, 14, 1)
        strCarattere7 = Mid(sOldCF, 15, 1)


        If IsNumeric(strCarattere1) = False Then
            For i = 0 To UBound(sLettereOmo)
                If UCase(strCarattere1) = sLettereOmo(i) Then
                    strNumero1 = i
                    bolSostituzione = True
                    Exit For
                End If
            Next
        Else
            strNumero1 = strCarattere1
        End If
        i = 0
        If IsNumeric(strCarattere2) = False Then
            For i = 0 To UBound(sLettereOmo)
                If UCase(strCarattere2) = sLettereOmo(i) Then
                    strNumero2 = i
                    bolSostituzione = True
                    Exit For
                End If
            Next
        Else
            strNumero2 = strCarattere2
        End If
        i = 0
        If IsNumeric(strCarattere3) = False Then
            For i = 0 To UBound(sLettereOmo)
                If UCase(strCarattere3) = sLettereOmo(i) Then
                    strNumero3 = i
                    bolSostituzione = True
                    Exit For
                End If
            Next
        Else
            strNumero3 = strCarattere3
        End If
        i = 0
        If IsNumeric(strCarattere4) = False Then
            For i = 0 To UBound(sLettereOmo)
                If UCase(strCarattere4) = sLettereOmo(i) Then
                    strNumero4 = i
                    bolSostituzione = True
                    Exit For
                End If
            Next
        Else
            strNumero4 = strCarattere4
        End If
        i = 0
        If IsNumeric(strCarattere5) = False Then
            For i = 0 To UBound(sLettereOmo)
                If UCase(strCarattere5) = sLettereOmo(i) Then
                    strNumero5 = i
                    bolSostituzione = True
                    Exit For
                End If
            Next
        Else
            strNumero5 = strCarattere5
        End If
        i = 0
        If IsNumeric(strCarattere6) = False Then
            For i = 0 To UBound(sLettereOmo)
                If UCase(strCarattere6) = sLettereOmo(i) Then
                    strNumero6 = i
                    bolSostituzione = True
                    Exit For
                End If
            Next
        Else
            strNumero6 = strCarattere6
        End If
        i = 0
        If IsNumeric(strCarattere7) = False Then
            For i = 0 To UBound(sLettereOmo)
                If UCase(strCarattere7) = sLettereOmo(i) Then
                    strNumero7 = i
                    bolSostituzione = True
                    Exit For
                End If
            Next
        Else
            strNumero7 = strCarattere7
        End If

        If strNumero1 <> -1 Then
            strCarattere1 = strNumero1
        End If
        If strNumero2 <> -1 Then
            strCarattere2 = strNumero2
        End If
        If strNumero3 <> -1 Then
            strCarattere3 = strNumero3
        End If
        If strNumero4 <> -1 Then
            strCarattere4 = strNumero4
        End If
        If strNumero5 <> -1 Then
            strCarattere5 = strNumero5
        End If
        If strNumero6 <> -1 Then
            strCarattere6 = strNumero6
        End If
        If strNumero7 <> -1 Then
            strCarattere7 = strNumero7
        End If

        strAppoCF = Mid(sOldCF, 1, 6) & strCarattere1 & strCarattere2 & Mid(sOldCF, 9, 1) & strCarattere3 & strCarattere4 & Mid(sOldCF, 12, 1) & strCarattere5 & strCarattere6 & strCarattere7 & Mid(sOldCF, 16, 1)
        If bolSostituzione = False Then
            Return False
        Else
            If Mid(UCase(strAppoCF), 1, 15) & GetCarattereControllo(Mid(UCase(strAppoCF), 1, 15)) <> UCase(sNewCF) Then
                Return False
            Else
                Return True
            End If
        End If


    End Function

    Public Shared Function NuovaPasswordADC() As String
        '--------ANTONELLO DI CROCE-------------------SANDOKAN--------
        Dim Minuscole As String
        Dim Maiuscole As String
        Dim Numeri As String
        Dim Interpunzioni As String
        Dim password As String

        Dim originepassword As String
        'Dim appolunghezza As Integer

        Dim Appoggiomin As String
        Dim Appoggiomin1 As String
        Dim AppoggioMaiu As String
        Dim AppoggioMaiu1 As String
        Dim AppoggioNum As String
        Dim AppoggioNum1 As String
        Dim AppoggioInter As String
        Dim AppoggioInter1 As String
        Minuscole = "abcdefghijkmnopqrstuvwxyz"
        Maiuscole = "ABCDEFGHJKLMNPQRSTUVWXYZ"
        Numeri = "1234567890"
        Interpunzioni = ":,.!"              '";:,.?!-@*^)(/%\"
        Randomize()

        Appoggiomin = CInt(Int((Minuscole.Length) * Rnd() + 1))
        Appoggiomin1 = CInt(Int((Minuscole.Length) * Rnd() + 1))
        Appoggiomin = Mid(Minuscole, Appoggiomin, 1)
        Appoggiomin1 = Mid(Minuscole, Appoggiomin1, 1)


        AppoggioMaiu = CInt(Int((Maiuscole.Length) * Rnd() + 1))
        AppoggioMaiu1 = CInt(Int((Maiuscole.Length) * Rnd() + 1))
        AppoggioMaiu = Mid(Maiuscole, AppoggioMaiu, 1)
        AppoggioMaiu1 = Mid(Maiuscole, AppoggioMaiu1, 1)


        AppoggioNum = CInt(Int((Numeri.Length) * Rnd() + 1))
        AppoggioNum1 = CInt(Int((Numeri.Length) * Rnd() + 1))
        AppoggioNum = Mid(Numeri, AppoggioNum, 1)
        AppoggioNum1 = Mid(Numeri, AppoggioNum1, 1)


        AppoggioInter = CInt(Int((Interpunzioni.Length) * Rnd() + 1))
        AppoggioInter1 = CInt(Int((Interpunzioni.Length) * Rnd() + 1))
        AppoggioInter = Mid(Interpunzioni, AppoggioInter, 1)
        AppoggioInter1 = Mid(Interpunzioni, AppoggioInter1, 1)


        password = Appoggiomin & Appoggiomin1 & AppoggioMaiu & AppoggioMaiu1 & AppoggioNum & AppoggioNum1 & AppoggioInter & AppoggioInter1
        originepassword = password
        Dim X As Integer
        Dim DESTINAZIONE As String
        DESTINAZIONE = ""

        '''Generazione ammischio Antonello Funzionante a che ne dica danilo
        ''Do While password.Length <> 0
        ''    X = (password.Length * Rnd() + 1)
        ''    DESTINAZIONE = DESTINAZIONE + Mid(password, X, 1)
        ''    password = Left(password, X - 1) + Mid(password, X + 1, password.Length - 1)

        ''Loop

        'Generazione ammischio Danilo
        Do While Len(password) <> 0
            If Len(password) = 1 Then
                DESTINAZIONE = DESTINAZIONE + password
                password = ""
            Else
                X = Len(password) * Rnd()
                DESTINAZIONE = DESTINAZIONE + Mid(password, IIf(X = 0, 1, X), 1)
                password = Left(password, IIf(X = 0, 1, X) - 1) + Mid(password, IIf(X = 0, 1, X) + 1, Len(password) - IIf(X = 0, 1, X))
            End If
        Loop

        'lbloriginePassword.Text = originepassword
        'lbllunghezza.Text = DESTINAZIONE.Length
        'lblpassword.Text = DESTINAZIONE

        NuovaPasswordADC = DESTINAZIONE


    End Function
    Public Shared Function ModificaPasswordADC(ByVal pws) As String
        '-------ANTONELLO DI CROCE--------------------SANDOKAN--------
        Dim PwsDaControllare As String
        Dim Minuscole As String
        Dim Maiuscole As String
        Dim Numeri As String
        Dim Interpunzioni As String
        Dim risultatoverifica As Integer
        Dim MyStr As String
        Dim lunghezzaPWS As Integer
        Dim lunghezzaConfronto As Integer
        Dim esito As Integer
        Dim ContaEsito As Integer

        PwsDaControllare = pws
        Maiuscole = "ABCDEFGHIJKLMNOPQRSTUVWXYZ"
        Minuscole = "abcdefghijklmnopqrstuvwxyz"
        Numeri = "1234567890"
        Interpunzioni = ":,.!"
        risultatoverifica = 0
        If Len(PwsDaControllare) > 7 Then
            'controllo Maiuscole in stringa

            MyStr = PwsDaControllare
            lunghezzaPWS = CInt(Len(MyStr))
            lunghezzaConfronto = CInt(Len(Maiuscole))
            ContaEsito = 0
            esito = 0
            '----------------------------------------------------------------------------------------------------
            'controllo(Maiuscole)
            Do While lunghezzaPWS <> 0

                'CInt(Len(MyStr) - i)
                Maiuscole = "ABCDEFGHIJKLMNOPQRSTUVWXYZ"


                'Ciclo controllo Maiuscole
                Do While lunghezzaConfronto <> 0
                    lunghezzaConfronto = CInt(Len(Maiuscole))
                    If InStr(Mid(MyStr, 1, 1), Mid(Maiuscole, 1, 1)) <> 0 Then
                        esito = 1

                        Exit Do
                    End If

                    If Len(Maiuscole) = 1 Then
                        Exit Do
                    Else
                        Maiuscole = Maiuscole.Remove(0, 1)
                    End If
                Loop


                MyStr = MyStr.Remove(0, 1)
                lunghezzaPWS = CInt(Len(MyStr))
                If esito = 1 Then
                    ContaEsito = 1
                    Exit Do
                End If
            Loop
            '---------------------------------------------------------------------------------------------------
            'controllo Minuscole
            MyStr = PwsDaControllare
            lunghezzaPWS = CInt(Len(MyStr))
            Minuscole = "abcdefghijklmnopqrstuvwxyz"
            lunghezzaConfronto = CInt(Len(Minuscole))
            Do While lunghezzaPWS <> 0

                'CInt(Len(MyStr) - i)
                Minuscole = "abcdefghijklmnopqrstuvwxyz"


                'Ciclo controllo Maiuscole
                Do While lunghezzaConfronto <> 0
                    lunghezzaConfronto = CInt(Len(Minuscole))
                    If InStr(Mid(MyStr, 1, 1), Mid(Minuscole, 1, 1)) <> 0 Then
                        esito = 2

                        Exit Do
                    End If

                    If Len(Minuscole) = 1 Then
                        Exit Do
                    Else
                        Minuscole = Minuscole.Remove(0, 1)
                    End If
                Loop
                MyStr = MyStr.Remove(0, 1)
                lunghezzaPWS = CInt(Len(MyStr))
                If esito = 2 Then
                    ContaEsito = ContaEsito + 1
                    Exit Do
                End If
            Loop

            '---------------------------------------------------------------------------------------------------
            'controllo Numeri
            MyStr = PwsDaControllare
            lunghezzaPWS = CInt(Len(MyStr))
            Numeri = "1234567890"
            lunghezzaConfronto = CInt(Len(Numeri))
            Do While lunghezzaPWS <> 0

                'CInt(Len(MyStr) - i)
                Numeri = "1234567890"

                'Ciclo controllo Numeri
                Do While lunghezzaConfronto <> 0
                    lunghezzaConfronto = CInt(Len(Numeri))
                    If InStr(Mid(MyStr, 1, 1), Mid(Numeri, 1, 1)) <> 0 Then
                        esito = 3

                        Exit Do
                    End If

                    If Len(Numeri) = 1 Then
                        Exit Do
                    Else
                        Numeri = Numeri.Remove(0, 1)
                    End If
                Loop
                MyStr = MyStr.Remove(0, 1)
                lunghezzaPWS = CInt(Len(MyStr))
                If esito = 3 Then
                    ContaEsito = ContaEsito + 1
                    Exit Do
                End If
            Loop

            '-------------------------------------------------------------------------------------------------
            'controllo Interpunsioni
            MyStr = PwsDaControllare
            lunghezzaPWS = CInt(Len(MyStr))
            Interpunzioni = ":,.!"
            lunghezzaConfronto = CInt(Len(Interpunzioni))
            Do While lunghezzaPWS <> 0

                'CInt(Len(MyStr) - i)
                Interpunzioni = ":,.!"

                'Ciclo controllo Numeri
                Do While lunghezzaConfronto <> 0
                    lunghezzaConfronto = CInt(Len(Interpunzioni))
                    If InStr(Mid(MyStr, 1, 1), Mid(Interpunzioni, 1, 1)) <> 0 Then
                        esito = 4

                        Exit Do
                    End If

                    If Len(Interpunzioni) = 1 Then
                        Exit Do
                    Else
                        Interpunzioni = Interpunzioni.Remove(0, 1)
                    End If
                Loop
                MyStr = MyStr.Remove(0, 1)
                lunghezzaPWS = CInt(Len(MyStr))
                If esito = 4 Then
                    ContaEsito = ContaEsito + 1
                    Exit Do
                End If
            Loop
            If ContaEsito >= 3 Then
                Return True
            Else
                Return False
            End If
        Else
            Return False
        End If

    End Function
    Public Shared Function ControlloCaratteriPasswordADC(ByVal stringa) As String
        '--------ANTONELLO DI CROCE-------------------SANDOKAN--------
        Dim stringadacontrollare As String
        Dim Stringachecontrolla As String
        Dim MyStr As String
        Dim LunghezzaStringaDaControllate As Integer
        Dim lunghezzaStringachecontrolla As Integer
        Dim esito As Integer

        'dichiaro un arrey di carattere errati
        Dim Caratterierrati As String



        stringadacontrollare = Trim(stringa)
        Stringachecontrolla = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz1234567890:,.!"
        MyStr = stringadacontrollare
        LunghezzaStringaDaControllate = CInt(Len(MyStr))
        LunghezzaStringaDaControllate = CInt(Len(Stringachecontrolla))


        Do While LunghezzaStringaDaControllate <> 0

            'CInt(Len(MyStr) - i)
            Stringachecontrolla = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz1234567890:,.!"


            Do While LunghezzaStringaDaControllate <> 0
                LunghezzaStringaDaControllate = CInt(Len(Stringachecontrolla))
                If InStr(Mid(MyStr, 1, 1), Mid(Stringachecontrolla, 1, 1)) <> 0 Then
                    esito = 0
                    Stringachecontrolla = Stringachecontrolla.Remove(0, 1)
                    Exit Do
                Else
                    Stringachecontrolla = Stringachecontrolla.Remove(0, 1)
                    esito = 1
                End If

                If Len(Stringachecontrolla) = 0 Then
                    Caratterierrati = "ERRORE"
                    esito = 1
                    Return False
                    Exit Do
                    'Else
                    '    Stringachecontrolla = Stringachecontrolla.Remove(0, 1)
                    '    'Return True
                    '    esito = 0
                End If
            Loop

            'Return True
            If Caratterierrati <> "" Then
                esito = 1
                Return False
                Exit Function
            End If

            MyStr = MyStr.Remove(0, 1)
            LunghezzaStringaDaControllate = CInt(Len(MyStr))

        Loop
        '----------------------
        'solo una prova
        'Return True
        '----------------------
        If esito = 1 Then
            Return False
        Else
            Return True
        End If

    End Function
    Public Shared Function CAP_VERIFICA(ByVal conn As SqlClient.SqlConnection, ByRef strCausale As String, ByRef blnBandiera As Boolean, ByVal strCap As String, Optional ByVal strIdComune As String = "", Optional ByVal strDenominazione As String = "", Optional ByVal strCodiceIstat As String = "", Optional ByVal strIndirizzo As String = "", Optional ByVal strCivico As String = "") As Boolean
        'AUTORE: Antonello Di Croce  ---------------------------SANDOKAN--------
        'DESCRIZIONE: richiamo la SP_CAP_CONTROLLOINDIRIZZI per la verifica sulla validit‡ del cap 
        'DATA: 06/04/2009
        Dim Reader As SqlClient.SqlDataReader

        Try
            Dim x As Boolean
            Dim y As String
            Dim z As Boolean
            'Dim ArreyOutPut(1) As String


            Dim MyCommand As New SqlClient.SqlCommand
            MyCommand.CommandType = CommandType.StoredProcedure
            MyCommand.CommandText = "SP_CAP_CONTROLLOINDIRIZZI"
            MyCommand.Connection = conn

            'PRIMO PARAMETRO A=IdComune INPUT
            Dim sparam As SqlClient.SqlParameter
            sparam = New SqlClient.SqlParameter
            sparam.ParameterName = "@IdComune"
            sparam.SqlDbType = SqlDbType.Int
            MyCommand.Parameters.Add(sparam)

            'SECONDO PARAMETRO B=CodiceIstat INPUT
            Dim sparam1 As SqlClient.SqlParameter
            sparam1 = New SqlClient.SqlParameter
            sparam1.ParameterName = "@CodiceIStat"
            sparam1.SqlDbType = SqlDbType.NVarChar
            MyCommand.Parameters.Add(sparam1)

            'TERZO PARAMETRO C=Indirizzo INPUT
            Dim sparam2 As SqlClient.SqlParameter
            sparam2 = New SqlClient.SqlParameter
            sparam2.ParameterName = "@Indirizzo"
            sparam2.SqlDbType = SqlDbType.NVarChar
            MyCommand.Parameters.Add(sparam2)

            'QUARTO PARAMETRO D=Civico INPUT
            Dim sparam3 As SqlClient.SqlParameter
            sparam3 = New SqlClient.SqlParameter
            sparam3.ParameterName = "@Civico"
            sparam3.SqlDbType = SqlDbType.NVarChar
            MyCommand.Parameters.Add(sparam3)

            'QUINTO PARAMETRO E=Cap INPUT
            Dim sparam4 As SqlClient.SqlParameter
            sparam4 = New SqlClient.SqlParameter
            sparam4.ParameterName = "@Cap"
            sparam4.SqlDbType = SqlDbType.NVarChar
            MyCommand.Parameters.Add(sparam4)

            'PARAMETRO 1=esito OUTPUT
            Dim sparam5 As SqlClient.SqlParameter
            sparam5 = New SqlClient.SqlParameter
            sparam5.ParameterName = "@ESITO"
            sparam5.SqlDbType = SqlDbType.Bit
            sparam5.Direction = ParameterDirection.Output
            MyCommand.Parameters.Add(sparam5)


            'PARAMETRO 2=causale OUTPUT
            Dim sparam6 As SqlClient.SqlParameter
            sparam6 = New SqlClient.SqlParameter
            sparam6.ParameterName = "@Causale"
            sparam6.Size = 100
            sparam6.SqlDbType = SqlDbType.NVarChar
            sparam6.Direction = ParameterDirection.Output
            MyCommand.Parameters.Add(sparam6)

            'PARAMETRO 3=bandiera OUTPUT
            Dim sparam7 As SqlClient.SqlParameter
            sparam7 = New SqlClient.SqlParameter
            sparam7.ParameterName = "@Bandiera"
            sparam7.SqlDbType = SqlDbType.Bit
            sparam7.Direction = ParameterDirection.Output
            MyCommand.Parameters.Add(sparam7)

            MyCommand.Parameters("@IdComune").Value = strIdComune
            MyCommand.Parameters("@CodiceIstat").Value = strCodiceIstat
            MyCommand.Parameters("@Indirizzo").Value = strIndirizzo
            MyCommand.Parameters("@Civico").Value = strCivico
            MyCommand.Parameters("@Cap").Value = strCap

            Reader = MyCommand.ExecuteReader()

            x = CStr(MyCommand.Parameters("@Esito").Value)
            strCausale = MyCommand.Parameters("@Causale").Value
            blnBandiera = MyCommand.Parameters("@Bandiera").Value

            'ArreyOutPut(0) = x
            'ArreyOutPut(1) = y

            Reader.Close()
            Reader = Nothing

            Return x

        Catch ex As Exception
            If Not Reader Is Nothing Then
                Reader.Close()
                Reader = Nothing
            End If
            Dim x1 As Boolean
            x1 = False
            blnBandiera = False
            strCausale = "Contattare l'assistenza Helios/Futuro"
            Return x1

        End Try
    End Function
    Public Shared Function SETTORI_VERIFICA(ByVal conn As SqlClient.SqlConnection, ByRef Attivita As Integer, Optional ByVal macroambito As Integer = 1) As Boolean
        'AUTORE: Antonello Di Croce  ---------------------------SANDOKAN--------
        'DESCRIZIONE: richiamo la SP_VERIFICA_CONGRUENZA_SETTORI_PROGETTO per la dei settori congrui
        'DATA: 20/10/2009

        Dim Reader As SqlClient.SqlDataReader

        Try

            Dim x As Boolean


            Dim MyCommand As New SqlClient.SqlCommand
            MyCommand.CommandType = CommandType.StoredProcedure
            MyCommand.CommandText = "SP_VERIFICA_CONGRUENZA_SETTORI_PROGETTO"
            MyCommand.Connection = conn

            'PRIMO PARAMETRO A=IdAttivita INPUT
            Dim sparam As SqlClient.SqlParameter
            sparam = New SqlClient.SqlParameter
            sparam.ParameterName = "@IdAttivita"
            sparam.SqlDbType = SqlDbType.Int
            MyCommand.Parameters.Add(sparam)

            'SECONDO PARAMETRO B=IdMacroAmbitoAttivita INPUT
            Dim sparam1 As SqlClient.SqlParameter
            sparam1 = New SqlClient.SqlParameter
            sparam1.ParameterName = "@IdMacroAmbitoAttivita"
            sparam1.SqlDbType = SqlDbType.Int
            MyCommand.Parameters.Add(sparam1)


            'PARAMETRO 1=VALORE OUTPUT
            Dim sparam2 As SqlClient.SqlParameter
            sparam2 = New SqlClient.SqlParameter
            sparam2.ParameterName = "@Valore"
            sparam2.SqlDbType = SqlDbType.Bit
            sparam2.Direction = ParameterDirection.Output
            MyCommand.Parameters.Add(sparam2)

            MyCommand.Parameters("@IdAttivita").Value = Attivita
            MyCommand.Parameters("@IdMacroAmbitoAttivita").Value = macroambito

            Reader = MyCommand.ExecuteReader()

            x = MyCommand.Parameters("@Valore").Value

            Reader.Close()
            Reader = Nothing

            Return x
        Catch ex As Exception
            If Not Reader Is Nothing Then
                Reader.Close()
                Reader = Nothing
            End If
        End Try
    End Function

    Public Shared Function SETTORI_VERIFICA_FLAGCVOLP(ByVal conn As SqlClient.SqlConnection, ByRef Attivita As Integer, Optional ByVal macroambito As Integer = 1) As Boolean
        'AUTORE: Antonello Di Croce  ---------------------------SANDOKAN--------
        'DESCRIZIONE: richiamo la SP_VERIFICA_CONGRUENZA_SETTORI_PROGETTO per la dei settori congrui
        'DATA: 20/10/2009

        Dim Reader As SqlClient.SqlDataReader

        Try

            Dim x As Boolean


            Dim MyCommand As New SqlClient.SqlCommand
            MyCommand.CommandType = CommandType.StoredProcedure
            MyCommand.CommandText = "SP_VERIFICA_CONGRUENZA_SETTORI_FLAGCVOLP"
            MyCommand.Connection = conn

            'PRIMO PARAMETRO A=IdAttivita INPUT
            Dim sparam As SqlClient.SqlParameter
            sparam = New SqlClient.SqlParameter
            sparam.ParameterName = "@IdAttivita"
            sparam.SqlDbType = SqlDbType.Int
            MyCommand.Parameters.Add(sparam)

            'SECONDO PARAMETRO B=IdMacroAmbitoAttivita INPUT
            Dim sparam1 As SqlClient.SqlParameter
            sparam1 = New SqlClient.SqlParameter
            sparam1.ParameterName = "@IdMacroAmbitoAttivita"
            sparam1.SqlDbType = SqlDbType.Int
            MyCommand.Parameters.Add(sparam1)


            'PARAMETRO 1=VALORE OUTPUT
            Dim sparam2 As SqlClient.SqlParameter
            sparam2 = New SqlClient.SqlParameter
            sparam2.ParameterName = "@Valore"
            sparam2.SqlDbType = SqlDbType.Bit
            sparam2.Direction = ParameterDirection.Output
            MyCommand.Parameters.Add(sparam2)

            MyCommand.Parameters("@IdAttivita").Value = Attivita
            MyCommand.Parameters("@IdMacroAmbitoAttivita").Value = macroambito

            Reader = MyCommand.ExecuteReader()

            x = MyCommand.Parameters("@Valore").Value

            Reader.Close()
            Reader = Nothing

            Return x
        Catch ex As Exception
            If Not Reader Is Nothing Then
                Reader.Close()
                Reader = Nothing
            End If
        End Try
    End Function

    Public Shared Function CAUSALI_ACCOMPAGNA(ByVal conn As SqlClient.SqlConnection, ByRef Attivita As Integer, ByVal AmbitoAttivita As Integer) As Boolean
        'AUTORE: Antonello Di Croce  ---------------------------SANDOKAN--------
        'DESCRIZIONE: richiamo la SP_VERIFICA_CONGRUENZA_CAUSALI_ACCOMPAGNO  congrue
        'DATA: 27/10/2009

        Dim Reader As SqlClient.SqlDataReader

        Try

            Dim x As Boolean


            Dim MyCommand As New SqlClient.SqlCommand
            MyCommand.CommandType = CommandType.StoredProcedure
            MyCommand.CommandText = "SP_VERIFICA_CONGRUENZA_CAUSALI_ACCOMPAGNO"
            MyCommand.Connection = conn

            'PRIMO PARAMETRO A=IdAttivita INPUT
            Dim sparam As SqlClient.SqlParameter
            sparam = New SqlClient.SqlParameter
            sparam.ParameterName = "@IdAttivita"
            sparam.SqlDbType = SqlDbType.Int
            MyCommand.Parameters.Add(sparam)

            'SECONDO PARAMETRO B=IdMacroAmbitoAttivita INPUT
            Dim sparam1 As SqlClient.SqlParameter
            sparam1 = New SqlClient.SqlParameter
            sparam1.ParameterName = "@IdAmbitoAttivita"
            sparam1.SqlDbType = SqlDbType.Int
            MyCommand.Parameters.Add(sparam1)


            'PARAMETRO 1=VALORE OUTPUT
            Dim sparam2 As SqlClient.SqlParameter
            sparam2 = New SqlClient.SqlParameter
            sparam2.ParameterName = "@Valore"
            sparam2.SqlDbType = SqlDbType.Bit
            sparam2.Direction = ParameterDirection.Output
            MyCommand.Parameters.Add(sparam2)

            MyCommand.Parameters("@IdAttivita").Value = Attivita
            MyCommand.Parameters("@IdAmbitoAttivita").Value = AmbitoAttivita

            Reader = MyCommand.ExecuteReader()

            x = MyCommand.Parameters("@Valore").Value

            Reader.Close()
            Reader = Nothing

            Return x
        Catch ex As Exception
            If Not Reader Is Nothing Then
                Reader.Close()
                Reader = Nothing
            End If
        End Try
    End Function

    Public Shared Function AbiCab(ByVal strIban As String) As Boolean
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

    Public Shared Function ForzaPresenzaSanzione(ByVal Utente As String, ByVal conn As SqlClient.SqlConnection) As Boolean
        'Agg da simona cordella il 07/07/2011
        'Verifico se l'utene U/R Ë autorizzato alla visualizzazione del menu FORZA PRESENZA SANZIONE
        Dim strSql As String
        Dim dtrgenerico As SqlClient.SqlDataReader
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
        'Verifica menu sicurezza su funzione accredita
        strSql = "SELECT AssociaUtenteGruppo.username, VociMenu.IdVoceMenu, VociMenu.VoceMenu, VociMenu.ImmaginePassiva, VociMenu.ImmagineAttiva, VociMenu.Link," & _
                 " VociMenu.IdVoceMenuPadre" & _
                 " FROM VociMenu " & _
                 " INNER JOIN AbilitazioniProfiliVociMenu ON VociMenu.IdVoceMenu = AbilitazioniProfiliVociMenu.IdVoceMenu" & _
                 " INNER JOIN Profili ON AbilitazioniProfiliVociMenu.IdProfilo = Profili.IdProfilo" & _
                 " INNER JOIN AssociaUtenteGruppo ON Profili.IdProfilo = AssociaUtenteGruppo.IdProfilo" & _
                 " LEFT JOIN  RestrizioniUtenteVociMenu ON AssociaUtenteGruppo.UserName = RestrizioniUtenteVociMenu.UserName and RestrizioniUtenteVociMenu.IdVoceMenu=VociMenu.IdVoceMenu" & _
                 " WHERE VociMenu.descrizione = 'Forza Presenza Sanzione'" & _
                 " AND AssociaUtenteGruppo.username ='" & Utente & "' and (RestrizioniUtenteVociMenu.TipoRestrizione <> 0 or RestrizioniUtenteVociMenu.TipoRestrizione is null)"
        dtrgenerico = ClsServer.CreaDatareader(strSql, conn)

        ForzaPresenzaSanzione = dtrgenerico.Read()
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
    End Function
    Public Shared Function ForzaPresenzaSanzioneProgetto(ByVal Utente As String, ByVal conn As SqlClient.SqlConnection) As Boolean
        'Agg da  SANDOKAN il 16/12/2011
        'Verifico se l'utene U/R Ë autorizzato alla visualizzazione del menu Forza Presenza Sanzione Progetto
        Dim strSql As String
        Dim dtrgenerico As SqlClient.SqlDataReader
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
        'Verifica menu sicurezza su funzione accredita
        strSql = "SELECT AssociaUtenteGruppo.username, VociMenu.IdVoceMenu, VociMenu.VoceMenu, VociMenu.ImmaginePassiva, VociMenu.ImmagineAttiva, VociMenu.Link," & _
                 " VociMenu.IdVoceMenuPadre" & _
                 " FROM VociMenu " & _
                 " INNER JOIN AbilitazioniProfiliVociMenu ON VociMenu.IdVoceMenu = AbilitazioniProfiliVociMenu.IdVoceMenu" & _
                 " INNER JOIN Profili ON AbilitazioniProfiliVociMenu.IdProfilo = Profili.IdProfilo" & _
                 " INNER JOIN AssociaUtenteGruppo ON Profili.IdProfilo = AssociaUtenteGruppo.IdProfilo" & _
                 " LEFT JOIN  RestrizioniUtenteVociMenu ON AssociaUtenteGruppo.UserName = RestrizioniUtenteVociMenu.UserName and RestrizioniUtenteVociMenu.IdVoceMenu=VociMenu.IdVoceMenu" & _
                 " WHERE VociMenu.descrizione = 'Forza Presenza Sanzione Progetto'" & _
                 " AND AssociaUtenteGruppo.username ='" & Utente & "' and (RestrizioniUtenteVociMenu.TipoRestrizione <> 0 or RestrizioniUtenteVociMenu.TipoRestrizione is null)"
        dtrgenerico = ClsServer.CreaDatareader(strSql, conn)

        ForzaPresenzaSanzioneProgetto = dtrgenerico.Read()
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
    End Function
    Public Shared Function ForzaSedeSottopostaVerifica(ByVal Utente As String, ByVal conn As SqlClient.SqlConnection) As Boolean
        'Agg da simona cordella il 07/07/2011
        'Verifico se l'utene U/R Ë autorizzato alla visualizzazione del menu FORZA SEDE SOTTOPOSTA VERIFICA
        Dim strSql As String
        Dim dtrgenerico As SqlClient.SqlDataReader
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
        'Verifica menu sicurezza su funzione accredita
        strSql = "SELECT AssociaUtenteGruppo.username, VociMenu.IdVoceMenu, VociMenu.VoceMenu, VociMenu.ImmaginePassiva, VociMenu.ImmagineAttiva, VociMenu.Link," & _
                 " VociMenu.IdVoceMenuPadre" & _
                 " FROM VociMenu " & _
                 " INNER JOIN AbilitazioniProfiliVociMenu ON VociMenu.IdVoceMenu = AbilitazioniProfiliVociMenu.IdVoceMenu" & _
                 " INNER JOIN Profili ON AbilitazioniProfiliVociMenu.IdProfilo = Profili.IdProfilo" & _
                 " INNER JOIN AssociaUtenteGruppo ON Profili.IdProfilo = AssociaUtenteGruppo.IdProfilo" & _
                 " LEFT JOIN  RestrizioniUtenteVociMenu ON AssociaUtenteGruppo.UserName = RestrizioniUtenteVociMenu.UserName and RestrizioniUtenteVociMenu.IdVoceMenu=VociMenu.IdVoceMenu" & _
                 " WHERE VociMenu.descrizione = 'Forza Sede Sottoposta Verifica'" & _
                 " AND AssociaUtenteGruppo.username ='" & Utente & "' and (RestrizioniUtenteVociMenu.TipoRestrizione <> 0 or RestrizioniUtenteVociMenu.TipoRestrizione is null)"
        dtrgenerico = ClsServer.CreaDatareader(strSql, conn)

        ForzaSedeSottopostaVerifica = dtrgenerico.Read()
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
    End Function
    Public Shared Function ForzaRipristinoSanzione(ByVal Utente As String, ByVal conn As SqlClient.SqlConnection) As Boolean
        'Agg da simona cordella il 12/07/2011
        'Verifico se l'utene U/R Ë autorizzato alla visualizzazione del menu FORZA RIPRISTINO SANZIONE
        Dim strSql As String
        Dim dtrgenerico As SqlClient.SqlDataReader
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
        'Verifica menu sicurezza su funzione accredita
        strSql = "SELECT AssociaUtenteGruppo.username, VociMenu.IdVoceMenu, VociMenu.VoceMenu, VociMenu.ImmaginePassiva, VociMenu.ImmagineAttiva, VociMenu.Link," & _
                 " VociMenu.IdVoceMenuPadre" & _
                 " FROM VociMenu " & _
                 " INNER JOIN AbilitazioniProfiliVociMenu ON VociMenu.IdVoceMenu = AbilitazioniProfiliVociMenu.IdVoceMenu" & _
                 " INNER JOIN Profili ON AbilitazioniProfiliVociMenu.IdProfilo = Profili.IdProfilo" & _
                 " INNER JOIN AssociaUtenteGruppo ON Profili.IdProfilo = AssociaUtenteGruppo.IdProfilo" & _
                 " LEFT JOIN  RestrizioniUtenteVociMenu ON AssociaUtenteGruppo.UserName = RestrizioniUtenteVociMenu.UserName and RestrizioniUtenteVociMenu.IdVoceMenu=VociMenu.IdVoceMenu" & _
                 " WHERE VociMenu.descrizione = 'Forza Ripristino Sanzione'" & _
                 " AND AssociaUtenteGruppo.username ='" & Utente & "' and (RestrizioniUtenteVociMenu.TipoRestrizione <> 0 or RestrizioniUtenteVociMenu.TipoRestrizione is null)"
        dtrgenerico = ClsServer.CreaDatareader(strSql, conn)

        ForzaRipristinoSanzione = dtrgenerico.Read()
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
    End Function
    Public Shared Function ForzaFascicoloInformaticoVolontari(ByVal Utente As String, ByVal conn As SqlClient.SqlConnection) As Boolean
        'Agg da Simona Cordella il 15/05/2012
        'Verifico se l'utente U Ë autorizzato alla visualizzazione del menu Forza Fascicolo Informatico Volontari
        Dim strSql As String
        Dim dtrgenerico As SqlClient.SqlDataReader
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If

        strSql = " SELECT AssociaUtenteGruppo.username, VociMenu.IdVoceMenu, VociMenu.VoceMenu, VociMenu.ImmaginePassiva, VociMenu.ImmagineAttiva, VociMenu.Link," & _
                 " VociMenu.IdVoceMenuPadre" & _
                 " FROM VociMenu " & _
                 " INNER JOIN AbilitazioniProfiliVociMenu ON VociMenu.IdVoceMenu = AbilitazioniProfiliVociMenu.IdVoceMenu" & _
                 " INNER JOIN Profili ON AbilitazioniProfiliVociMenu.IdProfilo = Profili.IdProfilo" & _
                 " INNER JOIN AssociaUtenteGruppo ON Profili.IdProfilo = AssociaUtenteGruppo.IdProfilo" & _
                 " LEFT JOIN  RestrizioniUtenteVociMenu ON AssociaUtenteGruppo.UserName = RestrizioniUtenteVociMenu.UserName and RestrizioniUtenteVociMenu.IdVoceMenu=VociMenu.IdVoceMenu" & _
                 " WHERE VociMenu.descrizione = 'Forza Fascicolo Informatico Volontari'" & _
                 " AND AssociaUtenteGruppo.username ='" & Utente & "' and (RestrizioniUtenteVociMenu.TipoRestrizione <> 0 or RestrizioniUtenteVociMenu.TipoRestrizione is null)"
        dtrgenerico = ClsServer.CreaDatareader(strSql, conn)

        ForzaFascicoloInformaticoVolontari = dtrgenerico.Read()
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
    End Function

    Public Shared Function ForzaFascicoloInformaticoProgetti(ByVal Utente As String, ByVal conn As SqlClient.SqlConnection) As Boolean
        'Agg da Simona Cordella il 15/05/2012
        'Verifico se l'utente U Ë autorizzato alla visualizzazione del menu Forza Fascicolo Informatico Volontari
        Dim strSql As String
        Dim dtrgenerico As SqlClient.SqlDataReader
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If

        strSql = " SELECT AssociaUtenteGruppo.username, VociMenu.IdVoceMenu, VociMenu.VoceMenu, VociMenu.ImmaginePassiva, VociMenu.ImmagineAttiva, VociMenu.Link," & _
                 " VociMenu.IdVoceMenuPadre" & _
                 " FROM VociMenu " & _
                 " INNER JOIN AbilitazioniProfiliVociMenu ON VociMenu.IdVoceMenu = AbilitazioniProfiliVociMenu.IdVoceMenu" & _
                 " INNER JOIN Profili ON AbilitazioniProfiliVociMenu.IdProfilo = Profili.IdProfilo" & _
                 " INNER JOIN AssociaUtenteGruppo ON Profili.IdProfilo = AssociaUtenteGruppo.IdProfilo" & _
                 " LEFT JOIN  RestrizioniUtenteVociMenu ON AssociaUtenteGruppo.UserName = RestrizioniUtenteVociMenu.UserName and RestrizioniUtenteVociMenu.IdVoceMenu=VociMenu.IdVoceMenu" & _
                 " WHERE VociMenu.descrizione = 'Forza Fascicolo Informatico Progetti'" & _
                 " AND AssociaUtenteGruppo.username ='" & Utente & "' and (RestrizioniUtenteVociMenu.TipoRestrizione <> 0 or RestrizioniUtenteVociMenu.TipoRestrizione is null)"
        dtrgenerico = ClsServer.CreaDatareader(strSql, conn)

        ForzaFascicoloInformaticoProgetti = dtrgenerico.Read()
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
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
        dtrLeggiDati = ClsServer.CreaDatareader(strSql, sqllocalconn)
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

    Public Shared Function GetColumnNumber(ByVal str As String) As Integer
        Dim i As Integer

        str = str.ToUpper()
        GetColumnNumber = 0
        For i = str.Length - 1 To 0 Step -1
            GetColumnNumber = GetColumnNumber + (26 ^ (str.Length - i - 1)) * (Asc(Mid(str, i + 1, 1)) - Asc("A") + 1)
        Next
    End Function

    Public Shared Function ForzaPresenzaVerificaProgetto(ByVal Utente As String, ByVal conn As SqlClient.SqlConnection) As Boolean
        'Agg da  Simona Cordella il 05/04/2013
        'Verifico se l'utene U/R Ë autorizzato alla visualizzazione del menu Forza Presenza Verifica Progetto
        Dim strSql As String
        Dim dtrgenerico As SqlClient.SqlDataReader
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
        'Verifica menu sicurezza su funzione accredita
        strSql = "SELECT AssociaUtenteGruppo.username, VociMenu.IdVoceMenu, VociMenu.VoceMenu, VociMenu.ImmaginePassiva, VociMenu.ImmagineAttiva, VociMenu.Link," & _
                 " VociMenu.IdVoceMenuPadre" & _
                 " FROM VociMenu " & _
                 " INNER JOIN AbilitazioniProfiliVociMenu ON VociMenu.IdVoceMenu = AbilitazioniProfiliVociMenu.IdVoceMenu" & _
                 " INNER JOIN Profili ON AbilitazioniProfiliVociMenu.IdProfilo = Profili.IdProfilo" & _
                 " INNER JOIN AssociaUtenteGruppo ON Profili.IdProfilo = AssociaUtenteGruppo.IdProfilo" & _
                 " LEFT JOIN  RestrizioniUtenteVociMenu ON AssociaUtenteGruppo.UserName = RestrizioniUtenteVociMenu.UserName and RestrizioniUtenteVociMenu.IdVoceMenu=VociMenu.IdVoceMenu" & _
                 " WHERE VociMenu.descrizione = 'Forza Presenza Verifica Progetto'" & _
                 " AND AssociaUtenteGruppo.username ='" & Utente & "' and (RestrizioniUtenteVociMenu.TipoRestrizione <> 0 or RestrizioniUtenteVociMenu.TipoRestrizione is null)"
        dtrgenerico = ClsServer.CreaDatareader(strSql, conn)

        ForzaPresenzaVerificaProgetto = dtrgenerico.Read()
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
    End Function

    Public Shared Function ForzaStatoValutazione(ByVal Utente As String, ByVal conn As SqlClient.SqlConnection) As Boolean
        '*** aggiunto da Simona Cordella il 05/05/2014
        '** Verifico l'utene U/R Ë abilitato alla visualizzazione del campo SatoValutazione(menu Forza Stato Valutazione)
        Dim strSql As String
        Dim dtrgenerico As SqlClient.SqlDataReader
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
        'Verifica menu sicurezza su funzione accredita
        strSql = "SELECT AssociaUtenteGruppo.username, VociMenu.IdVoceMenu, VociMenu.VoceMenu, VociMenu.ImmaginePassiva, VociMenu.ImmagineAttiva, VociMenu.Link," & _
                 " VociMenu.IdVoceMenuPadre" & _
                 " FROM VociMenu " & _
                 " INNER JOIN AbilitazioniProfiliVociMenu ON VociMenu.IdVoceMenu = AbilitazioniProfiliVociMenu.IdVoceMenu" & _
                 " INNER JOIN Profili ON AbilitazioniProfiliVociMenu.IdProfilo = Profili.IdProfilo" & _
                 " INNER JOIN AssociaUtenteGruppo ON Profili.IdProfilo = AssociaUtenteGruppo.IdProfilo" & _
                 " LEFT JOIN  RestrizioniUtenteVociMenu ON AssociaUtenteGruppo.UserName = RestrizioniUtenteVociMenu.UserName and RestrizioniUtenteVociMenu.IdVoceMenu=VociMenu.IdVoceMenu" & _
                 " WHERE VociMenu.descrizione = 'Forza Stato Valutazione'" & _
                 " AND AssociaUtenteGruppo.username ='" & Utente & "' and (RestrizioniUtenteVociMenu.TipoRestrizione <> 0 or RestrizioniUtenteVociMenu.TipoRestrizione is null)"
        dtrgenerico = ClsServer.CreaDatareader(strSql, conn)

        ForzaStatoValutazione = dtrgenerico.Read()
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
    End Function


    Function TipologiaProgettoDaIdEntita(ByVal idEntita As String, ByVal connection As SqlConnection) As String
        Dim query As String
        Dim idTipoProgetto As String
        Dim dataReader As SqlDataReader

        query = "SELECT     attivit‡.IdTipoProgetto" & _
        " from entit‡  " & _
        " INNER JOIN attivit‡ ON entit‡.TMPCodiceProgetto = attivit‡.CodiceEnte " & _
        " WHERE entit‡.IDEntit‡ =" & idEntita & ""

        dataReader = ClsServer.CreaDatareader(query, connection)
        If dataReader.HasRows = True Then
            dataReader.Read()
            idTipoProgetto = dataReader("IdTipoProgetto")
        End If
        dataReader.Close()
        Return idTipoProgetto
    End Function

    Function TipologiaModelloDaIdEntita(ByVal idEntita As Integer, ByVal connection As SqlConnection) As String
        'ritorna la tipologia di modello da generare:
        'EST per progetti estero
        'GG per progetti garanzia giovani
        'SCD per progetti servizio civile digitale
        'ORD per prgetti ordinari
        Dim TipoModello As String = ""
        Dim dtrLocale As SqlClient.SqlDataReader
        Dim strsql As String

        strsql = "Select isnull(b.particolarit‡,0) as particolarit‡, tp.MacroTipoProgetto, tp.NazioneBase" & _
                " from entit‡ e" & _
                " inner join GraduatorieEntit‡ ge on e.IDEntit‡ = ge.IDEntit‡" & _
                " inner join Attivit‡SediAssegnazione asa on ge.IdAttivit‡SedeAssegnazione = asa.IDAttivit‡SedeAssegnazione" & _
                " inner join attivit‡ a on asa.IDAttivit‡ = a.IDAttivit‡" & _
                " inner join TipiProgetto tp on a.IdTipoProgetto = tp.IdTipoProgetto" & _
                " inner join BandiAttivit‡ ba on a.IDBandoAttivit‡ = ba.IDBandoAttivit‡" & _
                " inner join bando b on ba.IdBando = b.IDBando" & _
                " where e.identit‡ = " & idEntita
        dtrLocale = ClsServer.CreaDatareader(strsql, connection)

        If dtrLocale.HasRows = True Then
            dtrLocale.Read()
            If dtrLocale("nazionebase") = 0 Then
                TipoModello = "EST"
            ElseIf dtrLocale("MacroTipoProgetto") = "GG" Then
                TipoModello = "GG"
            ElseIf dtrLocale("particolarit‡") = 1 Then
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

    Function TipologiaProgettoDaIdAttivita(ByVal idAttivita As String, ByVal connection As SqlConnection) As String
        Dim query As String
        Dim idTipoProgetto As String
        Dim dataReader As SqlDataReader

        query = "SELECT     attivit‡.IdTipoProgetto" & _
        " FROM dbo.attivit‡  " & _
        " WHERE IDAttivit‡ =" & idAttivita & ""
        dataReader = ClsServer.CreaDatareader(query, connection)

        If dataReader.HasRows = True Then
            dataReader.Read()
            idTipoProgetto = dataReader("IdTipoProgetto")
        End If
        dataReader.Close()
        Return idTipoProgetto
    End Function

    Public Shared Function ControlloEntit‡SPCPerGenerazioneContratto(ByVal IDVolontario As Integer, ByVal connection As SqlConnection) As Boolean
        '** Creata da Simona Cordella
        '** Il 07/04/2015
        '** Funzione che verifica se il campo REQUISITI nella tabella Entit‡ sia <> da SI; s
        '** stampo in modello del contratto (Contratto Volontario Italia (12 mesi) (Garanzia Giovani Senza Presa In Carico)
        '** Contratto Volontario Italia (subentro) (Garanzia Giovani Senza Presa In Carico))
        Dim query As String
        Dim dataReader As SqlDataReader
        Dim blnRequisito As Boolean

        query = " SELECT IDEntit‡ " & _
                " FROM Entit‡  " & _
                " WHERE IDEntit‡ =" & IDVolontario & " and isnull(UPPER(Requisiti),'') <> 'SI'"
        dataReader = ClsServer.CreaDatareader(query, connection)
        blnRequisito = dataReader.HasRows

        dataReader.Close()
        dataReader = Nothing

        Return blnRequisito
    End Function
    Function IsValidEmailFormat(ByVal s As String) As Boolean
        Return Regex.IsMatch(s, "^([0-9a-zA-Z]([-\.\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,9})$")
    End Function

    Public Shared Function LoadProgettiAbilitaModificaEnte(ByVal IdAttivita As Integer, ByVal connection As SqlConnection) As Boolean
        'Creata da Simona Cordella il 28/12/2015
        'Richiamo la store che verifica se l'ENTE puÚ la modifica o meno un progetto 
        'FALSE ABILITATO =0 TRUEN ABILITATO=1

        Dim sqlCommand As New SqlClient.SqlCommand
        Dim progettoDuplicato As Boolean
        Dim abilitato As Boolean

        Try
            sqlCommand.CommandText = "SP_PROGETTI_ABILITA_MODIFICA_ENTE"
            sqlCommand.CommandType = CommandType.StoredProcedure
            sqlCommand.Connection = connection
            sqlCommand.Parameters.AddWithValue("@IdAttivita", IdAttivita)
            sqlCommand.Parameters.Add("@abilitato", SqlDbType.SmallInt, 1000)
            sqlCommand.Parameters("@abilitato").Direction = ParameterDirection.Output
            sqlCommand.ExecuteNonQuery()
            ' progettoDuplicato = sqlCommand.Parameters("@Esito").Value()
            abilitato = sqlCommand.Parameters("@abilitato").Value()
        Catch ex As Exception
            'ex.Message()
            LoadProgettiAbilitaModificaEnte = False
        End Try
        Return abilitato
    End Function

    Public Shared Function LoadProgrammiAbilitaModificaEnte(ByVal IdProgramma As Integer, ByVal connection As SqlConnection) As Boolean
        'Creata il 11/12/2019
        'Richiamo la store che verifica se l'ENTE puÚ la modifica o meno un programma 
        'FALSE ABILITATO =0 TRUEN ABILITATO=1

        Dim sqlCommand As New SqlClient.SqlCommand
        Dim progettoDuplicato As Boolean
        Dim abilitato As Boolean

        Try
            sqlCommand.CommandText = "SP_PROGRAMMI_ABILITA_MODIFICA_ENTE"
            sqlCommand.CommandType = CommandType.StoredProcedure
            sqlCommand.Connection = connection
            sqlCommand.Parameters.AddWithValue("@IdProgramma", IdProgramma)
            sqlCommand.Parameters.Add("@abilitato", SqlDbType.SmallInt, 1000)
            sqlCommand.Parameters("@abilitato").Direction = ParameterDirection.Output
            sqlCommand.ExecuteNonQuery()
            ' progettoDuplicato = sqlCommand.Parameters("@Esito").Value()
            abilitato = sqlCommand.Parameters("@abilitato").Value()
        Catch ex As Exception
            'ex.Message()
            LoadProgrammiAbilitaModificaEnte = False
        End Try
        Return abilitato
    End Function

    Public Shared Function ProgrammiLimitaFunzioniCoprogrammante(ByVal IdProgramma As Integer, ByVal identesessione As Integer, ByVal connection As SqlConnection) As Boolean
        'Creata il 18/12/2019
        'torna true se ente in sessione Ë coprogrammante

        Try
            Dim query As String
            Dim dataReader As SqlDataReader
            Dim blncoprogrammante As Boolean

            query = " SELECT idente " & _
                    " FROM ProgrammiEntiCoprogrammazione  " & _
                    " WHERE idprogramma =" & IdProgramma & " and idente = " & identesessione
            dataReader = ClsServer.CreaDatareader(query, connection)
            blncoprogrammante = dataReader.HasRows

            dataReader.Close()
            dataReader = Nothing

            Return blncoprogrammante
        Catch ex As Exception
            'ex.Message()
            ProgrammiLimitaFunzioniCoprogrammante = False
        End Try

    End Function

    Public Shared Function ProgettiLimitaFunzioniEnteNonProponente(ByVal IdProgetto As Integer, ByVal identesessione As Integer, ByVal connection As SqlConnection) As Boolean
        'Creata il 18/12/2019
        'torna true se ente in sessione non Ë titolare progetto

        Try
            Dim query As String
            Dim dataReader As SqlDataReader
            Dim blntitolare As Boolean

            query = " SELECT IDEntePresentante " & _
                    " FROM attivit‡  " & _
                    " WHERE idattivit‡ =" & IdProgetto & " and IDEntePresentante = " & identesessione
            dataReader = ClsServer.CreaDatareader(query, connection)
            blntitolare = dataReader.HasRows

            dataReader.Close()
            dataReader = Nothing

            Return Not blntitolare
        Catch ex As Exception
            'ex.Message()
            ProgettiLimitaFunzioniEnteNonProponente = False
        End Try

    End Function


    Public Shared Function CAP_VERIFICA_VOLONTARI(ByVal conn As SqlClient.SqlConnection, ByRef strCausale As String, ByRef blnBandiera As Boolean, ByVal strCap As String, Optional ByVal strIdComune As String = "", Optional ByVal strDenominazione As String = "", Optional ByVal strCodiceIstat As String = "", Optional ByVal strIndirizzo As String = "", Optional ByVal strCivico As String = "") As Boolean
        'AUTORE: Antonello Di Croce  ---------------------------SANDOKAN--------
        'DESCRIZIONE: richiamo la SP_CAP_CONTROLLOINDIRIZZI per la verifica sulla validit‡ del cap 
        'DATA: 06/04/2009
        Dim Reader As SqlClient.SqlDataReader

        Try
            Dim x As Boolean
            Dim y As String
            Dim z As Boolean
            'Dim ArreyOutPut(1) As String


            Dim MyCommand As New SqlClient.SqlCommand
            MyCommand.CommandType = CommandType.StoredProcedure
            MyCommand.CommandText = "SP_CAP_CONTROLLOINDIRIZZI_VOLONTARI"
            MyCommand.Connection = conn

            'PRIMO PARAMETRO A=IdComune INPUT
            Dim sparam As SqlClient.SqlParameter
            sparam = New SqlClient.SqlParameter
            sparam.ParameterName = "@IdComune"
            sparam.SqlDbType = SqlDbType.Int
            MyCommand.Parameters.Add(sparam)

            'SECONDO PARAMETRO B=CodiceIstat INPUT
            Dim sparam1 As SqlClient.SqlParameter
            sparam1 = New SqlClient.SqlParameter
            sparam1.ParameterName = "@CodiceIStat"
            sparam1.SqlDbType = SqlDbType.NVarChar
            MyCommand.Parameters.Add(sparam1)

            'TERZO PARAMETRO C=Indirizzo INPUT
            Dim sparam2 As SqlClient.SqlParameter
            sparam2 = New SqlClient.SqlParameter
            sparam2.ParameterName = "@Indirizzo"
            sparam2.SqlDbType = SqlDbType.NVarChar
            MyCommand.Parameters.Add(sparam2)

            'QUARTO PARAMETRO D=Civico INPUT
            Dim sparam3 As SqlClient.SqlParameter
            sparam3 = New SqlClient.SqlParameter
            sparam3.ParameterName = "@Civico"
            sparam3.SqlDbType = SqlDbType.NVarChar
            MyCommand.Parameters.Add(sparam3)

            'QUINTO PARAMETRO E=Cap INPUT
            Dim sparam4 As SqlClient.SqlParameter
            sparam4 = New SqlClient.SqlParameter
            sparam4.ParameterName = "@Cap"
            sparam4.SqlDbType = SqlDbType.NVarChar
            MyCommand.Parameters.Add(sparam4)

            'PARAMETRO 1=esito OUTPUT
            Dim sparam5 As SqlClient.SqlParameter
            sparam5 = New SqlClient.SqlParameter
            sparam5.ParameterName = "@ESITO"
            sparam5.SqlDbType = SqlDbType.Bit
            sparam5.Direction = ParameterDirection.Output
            MyCommand.Parameters.Add(sparam5)


            'PARAMETRO 2=causale OUTPUT
            Dim sparam6 As SqlClient.SqlParameter
            sparam6 = New SqlClient.SqlParameter
            sparam6.ParameterName = "@Causale"
            sparam6.Size = 100
            sparam6.SqlDbType = SqlDbType.NVarChar
            sparam6.Direction = ParameterDirection.Output
            MyCommand.Parameters.Add(sparam6)

            'PARAMETRO 3=bandiera OUTPUT
            Dim sparam7 As SqlClient.SqlParameter
            sparam7 = New SqlClient.SqlParameter
            sparam7.ParameterName = "@Bandiera"
            sparam7.SqlDbType = SqlDbType.Bit
            sparam7.Direction = ParameterDirection.Output
            MyCommand.Parameters.Add(sparam7)

            MyCommand.Parameters("@IdComune").Value = strIdComune
            MyCommand.Parameters("@CodiceIstat").Value = strCodiceIstat
            MyCommand.Parameters("@Indirizzo").Value = strIndirizzo
            MyCommand.Parameters("@Civico").Value = strCivico
            MyCommand.Parameters("@Cap").Value = strCap

            Reader = MyCommand.ExecuteReader()

            x = CStr(MyCommand.Parameters("@Esito").Value)
            strCausale = MyCommand.Parameters("@Causale").Value
            blnBandiera = MyCommand.Parameters("@Bandiera").Value

            'ArreyOutPut(0) = x
            'ArreyOutPut(1) = y

            Reader.Close()
            Reader = Nothing

            Return x

        Catch ex As Exception
            If Not Reader Is Nothing Then
                Reader.Close()
                Reader = Nothing
            End If
            Dim x1 As Boolean
            x1 = False
            blnBandiera = False
            strCausale = "Contattare l'assistenza Helios/Futuro"
            Return x1

        End Try
    End Function
    Public Shared Function VerificaValidit‡CodiceFiscaleConiuge(ByVal CodiceFiscale As String) As Boolean
        Dim ArrayCharPosizioneDispari() As Integer = {1, 0, 5, 7, 9, 13, 15, 17, 19, 21, 2, 4, 18, 20, 11, 3, 6, 8, 12, 14, 16, 10, 22, 25, 24, 23}
        Dim ArrayChar() As String = {"A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z"}
        Dim ArrayPari As New ArrayList
        Dim ArrayDispari As New ArrayList


        Dim risultato As Integer = 0
        Dim sommapari As Integer = 0
        Dim sommadispari As Integer = 0
        Try
            If CodiceFiscale.ToString <> "" Then
                For i As Integer = 0 To CodiceFiscale.Length - 2 Step 1
                    'Vi ricordo che l'array parte da zero ma noi dobbiamo calcolare come se fosse 1 
                    'e quindi Dispari si trovano nella posizione Pari e i Pari si trovano nella posizione Dispari
                    If i Mod (2) = 1 Then
                        'Indice Dispari - Posizione Dispari)
                        ArrayPari.Add(CodiceFiscale.Substring(i, 1).ToString)
                    Else
                        'Indice Pari - Posizione Pari
                        ArrayDispari.Add(CodiceFiscale.Substring(i, 1).ToString)
                    End If
                Next
                For i As Integer = 0 To ArrayPari.Count - 1 Step 1
                    If IsNumeric(ArrayPari(i).ToString) Then
                        sommapari = sommapari + ArrayPari(i).ToString
                        'Console.WriteLine(ArrayPari(i).ToString & " ArrayPari(" & i & ").ToString = " & ArrayPari(i).ToString)
                    Else
                        For j As Integer = 0 To ArrayChar.Length - 1 Step 1
                            If (UCase(ArrayPari(i).ToString) = ArrayChar(j).ToString) Then
                                sommapari = sommapari + j
                                'Console.WriteLine(ArrayPari(i).ToString & " ArrayPari(" & i & ").ToString = " & j)
                                Exit For
                            End If
                        Next
                    End If
                Next
                For i As Integer = 0 To ArrayDispari.Count - 1 Step 1
                    If IsNumeric(ArrayDispari(i).ToString) Then
                        sommadispari = sommadispari + ArrayCharPosizioneDispari(ArrayDispari(i).ToString).ToString
                        'Console.WriteLine(ArrayDispari(i).ToString & " ArrayDispari(" & i & ").ToString = " & ArrayCharPosizioneDispari(ArrayDispari(i).ToString).ToString)
                    Else
                        For j As Integer = 0 To ArrayChar.Length - 1 Step 1
                            If (UCase(ArrayDispari(i).ToString) = ArrayChar(j).ToString) Then
                                sommadispari = sommadispari + ArrayCharPosizioneDispari(j).ToString
                                'Console.WriteLine(ArrayDispari(i).ToString & " ArrayCharPosizioneDispari(" & j & ").ToString = " & ArrayCharPosizioneDispari(j).ToString)
                                Exit For
                            End If
                        Next
                    End If
                Next
            End If
            risultato = 0
            ArrayPari.Clear()
            ArrayDispari.Clear()
            risultato = (sommapari + sommadispari) Mod 26 'Restituisce il Resto con il comando Mod 
            'Console.WriteLine(ArrayChar(risultato).ToString & " = " & CodiceFiscale.Substring(CodiceFiscale.Length - 1, 1).ToString)
            'Controllo che la lettera che si trova nella posizione risultato o resto corrisponde a l'ultima lettera del codice fiscale passato
            If (ArrayChar(risultato).ToString = UCase(CodiceFiscale.Substring(CodiceFiscale.Length - 1, 1).ToString)) Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            'Errore generato da qualche numero e carattere strano
            Return False
        End Try
    End Function
    'funzione che controlla il profilo dell'utente loggato
    'se l'utente loggato Ë MASTER puÚ usare tutti i filtri di ricerca = TRUE
    'se l'utente loggato Ë ISPETTORE gli blocco le combo degli ispettori = FALSE
    'Public Shared Function TrovaProfiloUtente(ByVal Directory As String, ByVal Utente As String, ByVal conn As SqlConnection) As Integer
    '    Dim strLocal As String
    '    Dim dtrLocal As Data.SqlClient.SqlDataReader

    '    strLocal = "SELECT Descrizione FROM Profili "

    '    '============================================================================================================================
    '    '====================================================30/09/2008==============================================================
    '    '=======MODIFICA EFFETTUATA DA Kronk (99 scimmie saltavano sul letto, una cadde in terra e si ruppe il cervelletto...)=======
    '    '=================AGGIUNTA JOIN SU PROFILO READ PER ABILITARE LEGGERE LE ABILITAZIONI ANCHE DELL'UTENTE READ=================
    '    '============================================================================================================================
    '    If UCase(Directory) <> "/HELIOSREAD" Then
    '        strLocal = strLocal & " INNER JOIN	AssociaUtenteGruppo ON Profili.IdProfilo = AssociaUtenteGruppo.IdProfilo "
    '    Else
    '        strLocal = strLocal & " INNER JOIN	AssociaUtenteGruppo ON Profili.IdProfilo = AssociaUtenteGruppo.IdProfiloRead "
    '    End If

    '    strLocal = strLocal & " WHERE AssociaUtenteGruppo.UserName='" & Utente & "'"

    '    dtrLocal = ClsServer.CreaDatareader(strLocal, conn)

    '    If dtrLocal.HasRows = True Then
    '        dtrLocal.Read()
    '        '*********************************************************************************************
    '        'MODIFICATO IL 12/11/2008
    '        'ANCHE IL PROFILO REGIONI MASTER DEVE VEDERE COMPORTARSI COME VERFICHE MASTER E COME UNSC MASTER
    '        '*********************************************************************************************
    '        If (dtrLocal("Descrizione") = "VERIFICHE MASTER" Or dtrLocal("Descrizione") = "UNSC MASTER" Or dtrLocal("Descrizione") = "REGIONI MASTER") Then
    '            'TrovaProfiloUtente = True
    '            TrovaProfiloUtente = 0
    '        Else
    '            'TrovaProfiloUtente = False
    '            If Not dtrLocal Is Nothing Then
    '                dtrLocal.Close()
    '                dtrLocal = Nothing
    '            End If
    '            strLocal = "SELECT idverificatore FROM TVerificatori INNER JOIN UtentiUnsc ON TVerificatori.IdUtente=UtentiUnsc.IdUtente WHERE UtentiUnsc.UserName='" & Utente & "'"
    '            dtrLocal = ClsServer.CreaDatareader(strLocal, conn)
    '            If dtrLocal.HasRows = True Then
    '                dtrLocal.Read()
    '                TrovaProfiloUtente = CInt(dtrLocal("idverificatore"))
    '            End If
    '            If Not dtrLocal Is Nothing Then
    '                dtrLocal.Close()
    '                dtrLocal = Nothing
    '            End If
    '        End If
    '    End If

    '    If Not dtrLocal Is Nothing Then
    '        dtrLocal.Close()
    '        dtrLocal = Nothing
    '    End If

    '    Return TrovaProfiloUtente

    'End Function
    Public Shared Function TrovaProfiloUtente(ByVal Directory As String, ByVal Utente As String, ByVal conn As SqlConnection) As Integer
        Dim strLocal As String
        Dim dtrLocal As Data.SqlClient.SqlDataReader

        strLocal = "SELECT Descrizione FROM Profili "

        '============================================================================================================================
        '====================================================30/09/2008==============================================================
        '=======MODIFICA EFFETTUATA DA Kronk (99 scimmie saltavano sul letto, una cadde in terra e si ruppe il cervelletto...)=======
        '=================AGGIUNTA JOIN SU PROFILO READ PER ABILITARE LEGGERE LE ABILITAZIONI ANCHE DELL'UTENTE READ=================
        '============================================================================================================================
        If UCase(Directory) <> "/HELIOSREAD" Then
            strLocal = strLocal & " INNER JOIN	AssociaUtenteGruppo ON Profili.IdProfilo = AssociaUtenteGruppo.IdProfilo "
        Else
            strLocal = strLocal & " INNER JOIN	AssociaUtenteGruppo ON Profili.IdProfilo = AssociaUtenteGruppo.IdProfiloRead "
        End If

        strLocal = strLocal & " WHERE AssociaUtenteGruppo.UserName='" & Utente & "'"
        strLocal = strLocal & " AND Descrizione IN ('VERIFICHE MASTER','UNSC MASTER','REGIONI MASTER')"

        dtrLocal = ClsServer.CreaDatareader(strLocal, conn)

        If dtrLocal.HasRows = True Then
            dtrLocal.Read()
            TrovaProfiloUtente = 0
        Else
            'TrovaProfiloUtente = False
            If Not dtrLocal Is Nothing Then
                dtrLocal.Close()
                dtrLocal = Nothing
            End If
            strLocal = "SELECT idverificatore FROM TVerificatori INNER JOIN UtentiUnsc ON TVerificatori.IdUtente=UtentiUnsc.IdUtente WHERE UtentiUnsc.UserName='" & Utente & "'"
            dtrLocal = ClsServer.CreaDatareader(strLocal, conn)
            If dtrLocal.HasRows = True Then
                dtrLocal.Read()
                TrovaProfiloUtente = CInt(dtrLocal("idverificatore"))
            End If
            If Not dtrLocal Is Nothing Then
                dtrLocal.Close()
                dtrLocal = Nothing
            End If
        End If

        If Not dtrLocal Is Nothing Then
            dtrLocal.Close()
            dtrLocal = Nothing
        End If

        Return TrovaProfiloUtente

    End Function
    Public Shared Function TrovaAlboEnte(ByVal IdEnte As Integer, ByVal conn As SqlConnection) As String
        'creata da simona cordella il 18/07/2017
        'funzione che ricava in base alla classe di accreditamento l'albo dell'ente
        Dim strLocal As String
        Dim dtrLocal As Data.SqlClient.SqlDataReader
        Dim ALBO As String

        If Not dtrLocal Is Nothing Then
            dtrLocal.Close()
            dtrLocal = Nothing
        End If
        strLocal = "  SELECT isnull(e.ALBO,'') as albo "
        strLocal &= " FROM Enti e"
        strLocal &= " LEFT JOIN classiaccreditamento c ON e.IdClasseAccreditamentoRichiesta =c.IDClasseAccreditamento"
        strLocal &= " WHERE IDEnte = " & IdEnte

        dtrLocal = ClsServer.CreaDatareader(strLocal, conn)

        If dtrLocal.HasRows = True Then
            dtrLocal.Read()
            ALBO = Trim(dtrLocal("ALBO"))
        End If
        If Not dtrLocal Is Nothing Then
            dtrLocal.Close()
            dtrLocal = Nothing
        End If
        Return ALBO
    End Function

    Public Shared Function TrovaAlboProgetto(ByVal IdAttivita As Integer, ByVal conn As SqlConnection) As String
        'creata da simona cordella il 31/07/2017
        'funzione che apre in base al tipo progetto la tab progetti giusta 

        Dim strLocal As String
        Dim dtrLocal As Data.SqlClient.SqlDataReader
        Dim ApriTabProgetti As String

        If Not dtrLocal Is Nothing Then
            dtrLocal.Close()
            dtrLocal = Nothing
        End If
        strLocal = "  SELECT tp.Scheda, TP.IDTIPOPROGETTO, YEAR(A.DATACREAZIONERECORD) AS Anno "
        strLocal &= " from attivit‡ a "
        strLocal &= " inner join TipiProgetto tp on a.IdTipoProgetto=tp.IdTipoProgetto"
        strLocal &= " where IDAttivit‡ = " & IdAttivita

        dtrLocal = ClsServer.CreaDatareader(strLocal, conn)

        If dtrLocal.HasRows = True Then
            dtrLocal.Read()

            Select Case Trim(dtrLocal("Scheda"))
                Case "SCN"
                    If dtrLocal("IDTIPOPROGETTO") = 3 And dtrLocal("Anno") >= 2020 Then
                        ApriTabProgetti = "TabProgettiStraordinari2020.aspx"
                    Else
                        If dtrLocal("IDTIPOPROGETTO") = 4 And dtrLocal("Anno") >= 2019 Then
                            ApriTabProgetti = "TabProgetti2020.aspx"
                        Else
                            ApriTabProgetti = "TabProgetti.aspx"
                        End If
                    End If

                Case "SPE"
                    ApriTabProgetti = "TabProgettiSCU.aspx"
                Case "SCU"
                    ApriTabProgetti = "TabProgettiSCU_DEF.aspx"
                Case "S20"
                    ApriTabProgetti = "TabProgetti2020.aspx"
            End Select
           
        End If
        If Not dtrLocal Is Nothing Then
            dtrLocal.Close()
            dtrLocal = Nothing
        End If
        Return ApriTabProgetti

    End Function

    Public Shared Function RitornaMascheraValutazioneProgetto(ByVal IdAttivita As Integer, ByVal conn As SqlConnection) As String
        'creata da simona cordella il 31/07/2017
        'funzione che apre in base al tipo progetto la tab progetti giusta 

        Dim strLocal As String
        Dim dtrLocal As Data.SqlClient.SqlDataReader
        Dim ApriValProgetti As String = ""

        If Not dtrLocal Is Nothing Then
            dtrLocal.Close()
            dtrLocal = Nothing
        End If
        strLocal = "  SELECT case c.programmi when 1 then 'PROGRAMMI' else 'NO' END AS MACROTIPO "
        strLocal &= " from attivit‡ a "
        strLocal &= " inner join BANDIATTIVIT‡ B on a.IdBANDOATTIVIT‡=b.idbandoattivit‡"
        strLocal &= " inner join bando c on b.IdBANDO=c.idbando"
        strLocal &= " where a.IDAttivit‡ = " & IdAttivita

        dtrLocal = ClsServer.CreaDatareader(strLocal, conn)

        If dtrLocal.HasRows = True Then
            dtrLocal.Read()

            Select Case Trim(dtrLocal("MACROTIPO"))
                Case "PROGRAMMI"
                    ApriValProgetti = "WfrmValutazioneQual2020.aspx"
                Case "NO"
                    ApriValProgetti = "WfrmValutazioneQual.aspx"
            End Select

        End If
        If Not dtrLocal Is Nothing Then
            dtrLocal.Close()
            dtrLocal = Nothing
        End If
        Return ApriValProgetti

    End Function

    Public Shared Function STORE_VERIFICA_USABILITA_ENTE_ALBO(ByVal CodiceFiscale As String, ByVal DenominazioneEnte As String, ByVal Albo As String, ByVal IdEnte As Integer, ByRef messaggioEmail As String, ByRef messaggio As String, ByVal conn As SqlConnection) As String
        'REALIZZATA DA: SIMONA CORDELLA 
        'DATA REALIZZAZIONE: 17/10/2017
        'FUNZIONALITA': RICHIAMO STORE PER LA VERIFICA_USABILITA_ENTE_ALBO

        Dim sqlCMD As New SqlClient.SqlCommand
        Dim strNomeStore As String = "[SP_VERIFICA_USABILITA_ENTE_ALBO]"

        Try
            sqlCMD = New SqlClient.SqlCommand(strNomeStore, conn)
            sqlCMD.CommandType = CommandType.StoredProcedure

            sqlCMD.Parameters.Add("@CodiceFiscale", SqlDbType.VarChar).Value = CodiceFiscale
            sqlCMD.Parameters.Add("@DenominazioneEnte", SqlDbType.VarChar).Value = DenominazioneEnte
            sqlCMD.Parameters.Add("@Albo", SqlDbType.VarChar).Value = Albo
            sqlCMD.Parameters.Add("@IdEnte", SqlDbType.Int).Value = IdEnte

            Dim sparam1 As SqlClient.SqlParameter
            sparam1 = New SqlClient.SqlParameter
            sparam1.ParameterName = "@Esito"
            sparam1.Size = 4
            sparam1.SqlDbType = SqlDbType.NVarChar
            sparam1.Direction = ParameterDirection.Output
            sqlCMD.Parameters.Add(sparam1)

            Dim sparam2 As SqlClient.SqlParameter
            sparam2 = New SqlClient.SqlParameter
            sparam2.ParameterName = "@messaggio"
            sparam2.Size = 1000
            sparam2.SqlDbType = SqlDbType.NVarChar
            sparam2.Direction = ParameterDirection.Output
            sqlCMD.Parameters.Add(sparam2)

            Dim sparam3 As SqlClient.SqlParameter
            sparam3 = New SqlClient.SqlParameter
            sparam3.ParameterName = "@messaggioEmail"
            sparam3.Size = 1000
            sparam3.SqlDbType = SqlDbType.NVarChar
            sparam3.Direction = ParameterDirection.Output
            sqlCMD.Parameters.Add(sparam3)

            sqlCMD.ExecuteScalar()

            Dim ESITO As String
            ESITO = sqlCMD.Parameters("@Esito").Value
            messaggio = sqlCMD.Parameters("@messaggio").Value
            messaggioEmail = sqlCMD.Parameters("@messaggioEmail").Value
            Return ESITO

        Catch ex As Exception

            Exit Function
        End Try

    End Function

    Public Shared Function ControlloRichiestaModificaSede(ByVal IdEnteSede As Integer, ByVal Conn As SqlClient.SqlConnection) As Boolean
        'REALIZZATA DA: SIMONA CORDELLA 
        'DATA REALIZZAZIONE: 31/01/2018
        'FUNZIONALITA': VERIFICO SE LA SEDE INDICATA E' STATA ATTIVATA UNA RICHIETSA DI MODIFICA
        Dim strLocal As String
        Dim dtrLocal As Data.SqlClient.SqlDataReader
        Dim blnAvvisoSede As Boolean
        If Not dtrLocal Is Nothing Then
            dtrLocal.Close()
            dtrLocal = Nothing
        End If

        strLocal = "select isnull(AvvisoSede,0)as AvvisoSede from entisedi where IdEntesede=" & IdEnteSede

        dtrLocal = ClsServer.CreaDatareader(strLocal, Conn)
        If dtrLocal.HasRows = True Then
            dtrLocal.Read()
            blnAvvisoSede = dtrLocal("AvvisoSede")
        End If

        If Not dtrLocal Is Nothing Then
            dtrLocal.Close()
            dtrLocal = Nothing
        End If
        Return blnAvvisoSede
    End Function
    Public Shared Function TipoDiEnte(ByVal Ente As String, ByVal Conn As SqlClient.SqlConnection) As Boolean
       
        Dim strLocal As String
        Dim dtrLocal As Data.SqlClient.SqlDataReader
        Dim classeaccreditamento As String
        Dim albo As String
        Dim SiNo As Boolean
        If Not dtrLocal Is Nothing Then
            dtrLocal.Close()
            dtrLocal = Nothing
        End If

        strLocal = "select idclasseaccreditamento,albo  from enti where idente=" & Ente

        dtrLocal = ClsServer.CreaDatareader(strLocal, Conn)
        If dtrLocal.HasRows = True Then
            dtrLocal.Read()
            classeaccreditamento = dtrLocal("idclasseaccreditamento")
            albo = dtrLocal("Albo")
            If classeaccreditamento = 1 Or classeaccreditamento = 2 Or albo = "SCU" Then
                SiNo = True
            Else
                SiNo = False
            End If
        End If

        If Not dtrLocal Is Nothing Then
            dtrLocal.Close()
            dtrLocal = Nothing
        End If
        Return SiNo
    End Function
Public Shared Function ForzaCaricamentoPaghe(ByVal Utente As String, ByVal Conn As SqlClient.SqlConnection) As Boolean

        '** Verifico se l'utenza Ë abilitata alla visibilit‡ del flag che consente il caricamento della programamzione con volontari e progetti terminati
        '** profilio menu creato appositamente per le richieste regionali
        Dim strSql As String
        Dim dtrgenerico As SqlClient.SqlDataReader
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If

        'Verifica menu sicurezza su funzione accredita
        strSql = "SELECT AssociaUtenteGruppo.username, VociMenu.IdVoceMenu, VociMenu.VoceMenu, VociMenu.ImmaginePassiva, VociMenu.ImmagineAttiva, VociMenu.Link," & _
                 " VociMenu.IdVoceMenuPadre" & _
                 " FROM VociMenu " & _
                 " INNER JOIN AbilitazioniProfiliVociMenu ON VociMenu.IdVoceMenu = AbilitazioniProfiliVociMenu.IdVoceMenu" & _
                 " INNER JOIN Profili ON AbilitazioniProfiliVociMenu.IdProfilo = Profili.IdProfilo" & _
                 " INNER JOIN AssociaUtenteGruppo ON Profili.IdProfilo = AssociaUtenteGruppo.IdProfilo" & _
                 " LEFT JOIN  RestrizioniUtenteVociMenu ON AssociaUtenteGruppo.UserName = RestrizioniUtenteVociMenu.UserName and RestrizioniUtenteVociMenu.IdVoceMenu=VociMenu.IdVoceMenu" & _
                 " WHERE VociMenu.descrizione = 'Gestione Paghe'" & _
                 " AND AssociaUtenteGruppo.username ='" & Utente & "' and (RestrizioniUtenteVociMenu.TipoRestrizione <> 0 or RestrizioniUtenteVociMenu.TipoRestrizione is null)"
        dtrgenerico = ClsServer.CreaDatareader(strSql, Conn)

        ForzaCaricamentoPaghe = dtrgenerico.Read()
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
        Return ForzaCaricamentoPaghe
    End Function
    Public Shared Function CreaArrayMessaggi(ByVal Messaggio As String, ByVal carattere As String) As String()
        Dim MessDaStror As String()
        Dim MessFinale As String()
        Dim i As Integer
        Dim x As Integer

        MessDaStror = Split(Messaggio, carattere)

        For i = 0 To UBound(MessDaStror)
            If i = 0 Then
                ReDim MessFinale(0)
            Else
                ReDim Preserve MessFinale(UBound(MessFinale) + 1)
            End If
            If Left(MessDaStror(i), 1) = Chr(34) And Right(MessDaStror(i), 1) = Chr(34) Then

                MessDaStror(i) = Mid(MessDaStror(i), 2, Len(MessDaStror(i)) - 2)
            End If

            MessDaStror(i) = MessDaStror(i).Replace("""""", """")
            MessFinale(UBound(MessFinale)) = MessDaStror(i)
            Dim messdapassare As String()
            messdapassare = MessDaStror
            Return messdapassare
        Next
    End Function
End Class
