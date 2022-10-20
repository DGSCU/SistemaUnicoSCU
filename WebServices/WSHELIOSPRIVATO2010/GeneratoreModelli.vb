Imports System.IO
Public Class GeneratoreModelli
    Inherits System.Web.UI.Page
    Private PathDocumento As String

    'FUNZIONE CHE GENERA I DOCUMENTI
    'PARAMETRI NECESSARI: 
    'inIdModello     - - - > parametro fisso (IDMODELLO su EDITOR_MODELLI)
    'strUserName     - - - > username dell'utente loggato su Helios
    'intIdCompetenza - - - > id della regione di competenza dell'utente loggato
    'Parametri       - - - > array che contiene i parametri che userò per passare alla store (parametri fatti così: nomeparametro;valoreparametro)
    Protected Function EseguiGenerazioneModello(ByVal intIdModello As Integer, ByVal strUserName As String, ByVal intIdCompetenza As Integer, ByVal Parametri As DataTable, ByVal localConn As SqlClient.SqlConnection) As String
        '*************************************************************************
        '*                   MOTORE CHE GENERA IL DOCUMENTO                      *
        '*                      GENERATO DA JONATAS CAGE                         *
        '*                             07/01/2008                                *
        '*************************************************************************
        'variabile che in stremaing va a scrivere il file letto
        Dim Writer As StreamWriter
        'variabile che in streaming legge il template
        Dim Reader As StreamReader
        'variabile che prenderà il nome del file generato 
        Dim strNomeFile As String
        'variabile che prenderà il percorso del file generato e che passerò alla funzione come valore di ritorno
        Dim strPercorsoFile As String
        'datareader locale che utilizzo per leggere i dati dal database (informazioni sul modello, sui tag e sulla store da lanciare)
        Dim dtrLocal As SqlClient.SqlDataReader
        'command che esegue tutte le letture al database
        Dim myCommand As SqlClient.SqlCommand
        Dim strContenuto As String

        'istanzio il command
        myCommand = New SqlClient.SqlCommand
        'passo la connessione al command
        myCommand.Connection = localConn
        'costruisco la query per prendere le informazioni del modello (PATH E NOME)
        myCommand.CommandText = "SELECT Editor_Modelli.NomeFisico, Editor_ModelliCompetenze.Path, Editor_ModelliCompetenze.PathLocale,isnull(contenuto,'') as contenuto from Editor_ModelliCompetenze INNER JOIN Editor_Modelli ON Editor_Modelli.IdModello = Editor_ModelliCompetenze.IdModello WHERE Editor_ModelliCompetenze.IdModello=" & intIdModello & " AND Editor_ModelliCompetenze.IdRegioneCompetenza=" & intIdCompetenza
        'eseguo la query
        dtrLocal = myCommand.ExecuteReader
        'controllo se la query restituisce le informazioni
        If dtrLocal.HasRows = True Then
            'leggo il valore restituito dalla query
            dtrLocal.Read()
            'splitto la stringa del nome fisico del file per creare poi il file 
            Dim StringaArray() As String = dtrLocal("NomeFisico").Split(".")
            'stringa che prende il nome del file senza estenzione
            Dim strFile As String = StringaArray(StringaArray.Length - 2).ToString()
            'path del temoplate
            Dim strPercorsoTemplate As String = dtrLocal("Path")
            'nome del template
            Dim strNomeTemplate As String = dtrLocal("NomeFisico")
            'pathlocale in cui verrà salvato il template che servirà poi per generare il file richiesto dall'utente
            Dim strPathLocale As String = dtrLocal("PathLocale")

            strContenuto = dtrLocal("Contenuto")

            'variabile stringa che scorre riga per riga il template
            Dim xLinea As String
            'variabile che prende l'indirizzo del file da ripassare come valore di ritorno
            Dim strPercorsoFileGenerato As String

            Dim codsede As String

            'chiudo il datareader
            If Not dtrLocal Is Nothing Then
                dtrLocal.Close()
                dtrLocal = Nothing
            End If

            'creo il nome del file
            strNomeFile = strFile & CStr(strUserName) & CStr(Day(Now)) & CStr(Month(Now)) & CStr(Year(Now)) & Hour(Now) & Minute(Now) & Second(Now) & ".doc"
            'creo il percorso del file da salvare
            strPercorsoFile = Server.MapPath("./documentazione/") & strNomeFile
            'passo alla variabile userò come valore di ritorno il percorso del file appena creato
            strPercorsoFileGenerato = "./documentazione/" & strNomeFile

            '*****************************************MODIFICA FATTA IL 07.02.2008*************************************
            '*********************************************da BAGNARACK JOBAMA******************************************
            'vado a prendere, tramite Web Service, il template di cui si vuole genererare il file
            'il template verrà salvato su una cartella locale, così che possa essere letto
            'tranquillamente dallo streamreader
            If intIdModello = 58 Or intIdModello = 60 Or intIdModello = 63 Or intIdModello = 65 Or intIdModello = 264 Or intIdModello = 265 Or intIdModello = 266 Or intIdModello = 267 Or intIdModello = 277 Or intIdModello = 204 Or intIdModello = 206 Or intIdModello = 215 Or intIdModello = 216 Then 'contratto volontari
                xLinea = strContenuto
            Else
                'istanzio il Web Service WSDocumentazione
                Dim wsLocal As New WS_Editor.WSMetodiDocumentazione

                'wsLocal.Url = ConfigurationSettings.AppSettings("URL_WS_Documentazione")

                'dichiaro una varibbile byte che bufferizza (carica in memoria) il file template richiesto
                'e trasformato in base64
                Dim dataBuffer As Byte() = Convert.FromBase64String(wsLocal.RecuperaTemplate(strPercorsoTemplate, strNomeTemplate))
                'variabile stream che in streaming scrive il template sulla macchina che richiede il documento 
                Dim fs As FileStream
                'passo il template al filestream
                fs = New FileStream(Server.MapPath(strPathLocale & strNomeTemplate), FileMode.Create, FileAccess.Write)
                'ciclo il file bufferizzato e scrivo nel file tramite lo streaming del FileStream
                If (dataBuffer.Length > 0) Then
                    fs.Write(dataBuffer, 0, dataBuffer.Length)
                End If

                'chiudo lo streaming
                fs.Close()

                'apro il file che fa da template
                Reader = New StreamReader(Server.MapPath(Replace(strPathLocale, "\", "/")) & strNomeTemplate, System.Text.Encoding.Default, False)
                xLinea = Reader.ReadToEnd()
                Reader.Close()
                Reader = Nothing
            End If

            Writer = New StreamWriter(strPercorsoFile, True, System.Text.Encoding.Default)

            'inizio a leggere il template
            'xLinea = Reader.ReadLine()



            'piccola correzione per eliminare l'accapo involontario creato in alcuni tag
            xLinea = Replace(xLinea, vbCrLf, "")

            'myCommand.CommandText = "SELECT Editor_TAG.IdInterrogazione, Editor_TAG.NomeTag, Editor_TAG.NomeCampo FROM Editor_TAG INNER JOIN Editor_ModelliTag ON Editor_TAG.IdTag = Editor_ModelliTag.IdTag WHERE (Editor_ModelliTag.IdModello = " & intIdModello & ")"

            Dim dttLocal As New DataTable

            dttLocal = CreaDataTable("SELECT Editor_TAG.IdTag, Editor_TAG.IdInterrogazione, Editor_TAG.NomeTag, Editor_TAG.TipoTag, Editor_TAG.NomeCampo, Editor_TAG.NomeRoutine, Editor_Interrogazioni.ProceduraSql FROM Editor_TAG INNER JOIN Editor_ModelliTag ON Editor_TAG.IdTag = Editor_ModelliTag.IdTag LEFT JOIN Editor_Interrogazioni ON Editor_TAG.IdInterrogazione=Editor_Interrogazioni.IdInterrogazione WHERE (Editor_ModelliTag.IdModello = " & intIdModello & ")", localConn)

            'controllo se la query restituisce le informazioni
            If dttLocal.Rows.Count > 0 Then
                Dim dttRow As DataRow
                For Each dttRow In dttLocal.Rows
                    ' *** agg. da simona cordella il 04/11/2010
                    'Ricavo dalla query il valore del TAG <CODICESEDE> che serve come paramentro allafunzione BARRACAD
                    If dttRow.Item("NomeTag") = "<CODICESEDE>" Then
                        codsede = RecuperaValore(dttRow.Item("NomeCampo"), dttRow.Item("IdTag"), dttRow.Item("IdInterrogazione"), dttRow.Item("ProceduraSql"), Parametri, localConn)
                    End If
                    '***** 
                    'controllo se il TAG è presente nel testo
                    If InStr(xLinea, dttRow.Item("NomeTag")) > 0 Then
                        'controllo se si tratta di un TAG che si aspetta un valore multiplo
                        '1:valore multiplo
                        '0:valore normale
                        If dttRow.Item("TipoTag") = 1 Then
                            'Antonello 19/11/2013 per la generazione dell'immagine del datamatrix sul documento
                            Dim dttRowParametri As DataRow
                            'scorro tutti i parametri in ingresso per poter trovare il parametro necessario per il caricamento dei valori
                            For Each dttRowParametri In Parametri.Rows
                                'se il parametro nel datatable è lo stesso che mi occorre vado a prendere il relativo valore
                                'e lo passo alla routine che mi restituisce i valori multipli in una stringa
                                If UCase("IdEntità") = UCase(dttRowParametri.Item("PARAMETRO")) Then
                                    'faccio la replace del valore con la lista
                                    xLinea = Replace(xLinea, dttRow.Item("NomeTag"), FixText(VOL_DataMatrix(dttRowParametri.Item("VALORE"), localConn)))
                                End If
                            Next
                        Else 'valore normale
                            ' Dim codsede As Integer
                            Try
                                xLinea = Replace(xLinea, dttRow.Item("NomeTag"), FixText(RecuperaValore(dttRow.Item("NomeCampo"), dttRow.Item("IdTag"), dttRow.Item("IdInterrogazione"), dttRow.Item("ProceduraSql"), Parametri, localConn)))
                            Catch ex As Exception
                                Throw ex
                            End Try

                        End If
                    End If
                Next
                'agg. da simona cordella il 02/11/2010
                '*** a secondo del tipo di dicumento che sto stampando, salvo nel fiel .rtf i dati relativi a 
                '*** sistema : H ; CHIAVEID; IDMODELLO, IDSOGGETTO 
                'If codsede = "ERRORE" Then
                '    codsede = 0
                'End If
                ' xLinea = ParamentriBarraCAD(intIdModello, xLinea, Parametri, codsede)
                '***
            End If

            'scrivo la riga del template nel nuovo file
            Writer.WriteLine(xLinea)

            'chiudo lo streaming in scrittura
            Writer.Close()
            Writer = Nothing

            'chiudo lo streaming in scrittura


            EseguiGenerazioneModello = strPercorsoFileGenerato

        End If

        Return EseguiGenerazioneModello

    End Function
    Protected Function EseguiGenerazioneModelloNew(ByVal intIdModello As Integer, ByVal strUserName As String, ByVal intIdCompetenza As Integer, ByVal Parametri As DataTable, ByVal localConn As SqlClient.SqlConnection) As String
        '*************************************************************************
        '*                   MOTORE CHE GENERA IL DOCUMENTO                      *
        '*                      GENERATO DA JONATAS CAGE                         *
        '*                             07/01/2008                                *
        '*************************************************************************
        'variabile che in stremaing va a scrivere il file letto
        'Dim Writer As StreamWriter
        'variabile che in streaming legge il template
        'Dim Reader As StreamReader
        'variabile che prenderà il nome del file generato 
        Dim strNomeFile As String
        'variabile che prenderà il percorso del file generato e che passerò alla funzione come valore di ritorno
        'Dim strPercorsoFile As String
        'datareader locale che utilizzo per leggere i dati dal database (informazioni sul modello, sui tag e sulla store da lanciare)
        Dim dtrLocal As SqlClient.SqlDataReader
        'command che esegue tutte le letture al database
        Dim myCommand As SqlClient.SqlCommand
        Dim strContenuto As String

        'istanzio il command
        myCommand = New SqlClient.SqlCommand
        'passo la connessione al command
        myCommand.Connection = localConn
        'costruisco la query per prendere le informazioni del modello (PATH E NOME)
        myCommand.CommandText = "SELECT Editor_Modelli.NomeFisico, Editor_ModelliCompetenze.Path, Editor_ModelliCompetenze.PathLocale,isnull(contenuto,'') as contenuto from Editor_ModelliCompetenze INNER JOIN Editor_Modelli ON Editor_Modelli.IdModello = Editor_ModelliCompetenze.IdModello WHERE Editor_ModelliCompetenze.IdModello=" & intIdModello & " AND Editor_ModelliCompetenze.IdRegioneCompetenza=" & intIdCompetenza
        'eseguo la query
        dtrLocal = myCommand.ExecuteReader
        'controllo se la query restituisce le informazioni
        If dtrLocal.HasRows = True Then
            'leggo il valore restituito dalla query
            dtrLocal.Read()
            'splitto la stringa del nome fisico del file per creare poi il file 
            'Dim StringaArray() As String = dtrLocal("NomeFisico").Split(".")
            'stringa che prende il nome del file senza estenzione
            'Dim strFile As String = StringaArray(StringaArray.Length - 2).ToString()
            'path del temoplate
            'Dim strPercorsoTemplate As String = dtrLocal("Path")
            'nome del template
            'Dim strNomeTemplate As String = dtrLocal("NomeFisico")
            'pathlocale in cui verrà salvato il template che servirà poi per generare il file richiesto dall'utente
            'Dim strPathLocale As String = dtrLocal("PathLocale")

            strContenuto = dtrLocal("Contenuto")

            'variabile stringa che scorre riga per riga il template
            Dim xLinea As String
            'variabile che prende l'indirizzo del file da ripassare come valore di ritorno
            'Dim strPercorsoFileGenerato As String

            Dim codsede As String

            'chiudo il datareader
            If Not dtrLocal Is Nothing Then
                dtrLocal.Close()
                dtrLocal = Nothing
            End If

            'creo il nome del file
            'strNomeFile = strFile & CStr(strUserName) & CStr(Day(Now)) & CStr(Month(Now)) & CStr(Year(Now)) & Hour(Now) & Minute(Now) & Second(Now) & ".doc"
            'creo il percorso del file da salvare
            'strPercorsoFile = Server.MapPath("./documentazione/") & strNomeFile
            'passo alla variabile userò come valore di ritorno il percorso del file appena creato
            'strPercorsoFileGenerato = "./documentazione/" & strNomeFile

            '*****************************************MODIFICA FATTA IL 07.02.2008*************************************
            '*********************************************da BAGNARACK JOBAMA******************************************
            'vado a prendere, tramite Web Service, il template di cui si vuole genererare il file
            'il template verrà salvato su una cartella locale, così che possa essere letto
            'tranquillamente dallo streamreader
            'If intIdModello = 58 Or intIdModello = 60 Or intIdModello = 63 Or intIdModello = 65 Or intIdModello = 264 Or intIdModello = 265 Or intIdModello = 266 Or intIdModello = 267 Or intIdModello = 277 Or intIdModello = 204 Or intIdModello = 206 Or intIdModello = 215 Or intIdModello = 216 Then 'contratto volontari

            xLinea = strContenuto

            'Else
            '    'istanzio il Web Service WSDocumentazione
            '    Dim wsLocal As New WS_Editor.WSMetodiDocumentazione

            '    'wsLocal.Url = ConfigurationSettings.AppSettings("URL_WS_Documentazione")

            '    'dichiaro una varibbile byte che bufferizza (carica in memoria) il file template richiesto
            '    'e trasformato in base64
            '    Dim dataBuffer As Byte() = Convert.FromBase64String(wsLocal.RecuperaTemplate(strPercorsoTemplate, strNomeTemplate))
            '    'variabile stream che in streaming scrive il template sulla macchina che richiede il documento 
            '    Dim fs As FileStream
            '    'passo il template al filestream
            '    fs = New FileStream(Server.MapPath(strPathLocale & strNomeTemplate), FileMode.Create, FileAccess.Write)
            '    'ciclo il file bufferizzato e scrivo nel file tramite lo streaming del FileStream
            '    If (dataBuffer.Length > 0) Then
            '        fs.Write(dataBuffer, 0, dataBuffer.Length)
            '    End If

            '    'chiudo lo streaming
            '    fs.Close()

            '    'apro il file che fa da template
            '    Reader = New StreamReader(Server.MapPath(Replace(strPathLocale, "\", "/")) & strNomeTemplate, System.Text.Encoding.Default, False)
            '    xLinea = Reader.ReadToEnd()
            '    Reader.Close()
            '    Reader = Nothing
            'End If

            'Writer = New StreamWriter(strPercorsoFile, True, System.Text.Encoding.Default)

            'inizio a leggere il template
            'xLinea = Reader.ReadLine()



            'piccola correzione per eliminare l'accapo involontario creato in alcuni tag
            xLinea = Replace(xLinea, vbCrLf, "")

            'myCommand.CommandText = "SELECT Editor_TAG.IdInterrogazione, Editor_TAG.NomeTag, Editor_TAG.NomeCampo FROM Editor_TAG INNER JOIN Editor_ModelliTag ON Editor_TAG.IdTag = Editor_ModelliTag.IdTag WHERE (Editor_ModelliTag.IdModello = " & intIdModello & ")"

            Dim dttLocal As New DataTable

            dttLocal = CreaDataTable("SELECT Editor_TAG.IdTag, Editor_TAG.IdInterrogazione, Editor_TAG.NomeTag, Editor_TAG.TipoTag, Editor_TAG.NomeCampo, Editor_TAG.NomeRoutine, Editor_Interrogazioni.ProceduraSql FROM Editor_TAG INNER JOIN Editor_ModelliTag ON Editor_TAG.IdTag = Editor_ModelliTag.IdTag LEFT JOIN Editor_Interrogazioni ON Editor_TAG.IdInterrogazione=Editor_Interrogazioni.IdInterrogazione WHERE (Editor_ModelliTag.IdModello = " & intIdModello & ")", localConn)

            'controllo se la query restituisce le informazioni
            If dttLocal.Rows.Count > 0 Then
                Dim dttRow As DataRow
                For Each dttRow In dttLocal.Rows
                    ' *** agg. da simona cordella il 04/11/2010
                    'Ricavo dalla query il valore del TAG <CODICESEDE> che serve come paramentro allafunzione BARRACAD
                    If dttRow.Item("NomeTag") = "<CODICESEDE>" Then
                        codsede = RecuperaValore(dttRow.Item("NomeCampo"), dttRow.Item("IdTag"), dttRow.Item("IdInterrogazione"), dttRow.Item("ProceduraSql"), Parametri, localConn)
                    End If
                    '***** 
                    'controllo se il TAG è presente nel testo
                    If InStr(xLinea, dttRow.Item("NomeTag")) > 0 Then
                        'controllo se si tratta di un TAG che si aspetta un valore multiplo
                        '1:valore multiplo
                        '0:valore normale
                        If dttRow.Item("TipoTag") = 1 Then
                            'Antonello 19/11/2013 per la generazione dell'immagine del datamatrix sul documento
                            Dim dttRowParametri As DataRow
                            'scorro tutti i parametri in ingresso per poter trovare il parametro necessario per il caricamento dei valori
                            For Each dttRowParametri In Parametri.Rows
                                'se il parametro nel datatable è lo stesso che mi occorre vado a prendere il relativo valore
                                'e lo passo alla routine che mi restituisce i valori multipli in una stringa
                                If UCase("IdEntità") = UCase(dttRowParametri.Item("PARAMETRO")) Then
                                    'faccio la replace del valore con la lista
                                    xLinea = Replace(xLinea, dttRow.Item("NomeTag"), FixText(VOL_DataMatrix(dttRowParametri.Item("VALORE"), localConn)))
                                End If
                            Next
                        Else 'valore normale
                            ' Dim codsede As Integer
                            Try
                                xLinea = Replace(xLinea, dttRow.Item("NomeTag"), FixText(RecuperaValore(dttRow.Item("NomeCampo"), dttRow.Item("IdTag"), dttRow.Item("IdInterrogazione"), dttRow.Item("ProceduraSql"), Parametri, localConn)))
                            Catch ex As Exception
                                Throw ex
                            End Try

                        End If
                    End If
                Next
                'agg. da simona cordella il 02/11/2010
                '*** a secondo del tipo di dicumento che sto stampando, salvo nel fiel .rtf i dati relativi a 
                '*** sistema : H ; CHIAVEID; IDMODELLO, IDSOGGETTO 
                'If codsede = "ERRORE" Then
                '    codsede = 0
                'End If
                ' xLinea = ParamentriBarraCAD(intIdModello, xLinea, Parametri, codsede)
                '***
            End If

            'scrivo la riga del template nel nuovo file
            'Writer.WriteLine(xLinea)

            'chiudo lo streaming in scrittura
            'Writer.Close()
            'Writer = Nothing

            'chiudo lo streaming in scrittura


            EseguiGenerazioneModelloNew = xLinea

        End If

        Return EseguiGenerazioneModelloNew

    End Function

    Function FixText(ByVal myText As String) As String

        myText = myText.Replace(vbCrLf, "\par" & vbCrLf)

        myText = myText.Replace("  ", " ")

        myText = myText.Replace(vbTab, "    ")

        Return myText

    End Function


    'funzione che recupera il singolo valore richiando la store procedure relativa
    Function RecuperaValore(ByVal strnomecampo As String, ByVal IdTag As Integer, ByVal idInterrogazione As Integer, ByVal strStore As String, ByVal Parametri As DataTable, ByVal localConn As SqlClient.SqlConnection) As String
        'data reader locale
        Dim Reader As SqlClient.SqlDataReader
        'datatable locale
        Dim dttLocal As New DataTable

        'vado a prendermi i paramtri che si aspetta la store per quel determinato tag
        dttLocal = CreaDataTable("SELECT NomeParametro, Sequenza, TipoParametro FROM Editor_Parametri WHERE IdInterrogazione=" & idInterrogazione & " ORDER BY Sequenza", localConn)

        'valore di ritorno
        Dim sReturnValue As String
        'command locale che esegue le query e la store
        Dim MyCommand As New SqlClient.SqlCommand
        'definisco il tipo di command di cui necessito (store procedure)
        MyCommand.CommandType = CommandType.StoredProcedure
        'passo al commandtext ilnome della store
        MyCommand.CommandText = strStore
        'passo al command la connection
        MyCommand.Connection = localConn

        'dichiaro un primo parametro fisso che prenderà poi il valore di ritorno
        Dim sparamFisso As SqlClient.SqlParameter
        sparamFisso = New SqlClient.SqlParameter
        'passo al parametro il nome fisso del valore di ritorno (@NomeCampo)
        sparamFisso.ParameterName = "@NomeCampo"
        'dichiaro il tipo di dato del parametro fisso
        sparamFisso.SqlDbType = SqlDbType.NVarChar
        'aggiungo al command il nuovo parametro
        MyCommand.Parameters.Add(sparamFisso)
        'al nuovo paramtro passo il valore
        MyCommand.Parameters("@NomeCampo").Value = strnomecampo

        'se ci sono parametri li ciclo e creo i relativi parametri per la store
        If dttLocal.Rows.Count > 0 Then
            Dim dttRow As DataRow
            'ciclo i parametri
            For Each dttRow In dttLocal.Rows

                'dichiaro l'oggetto parameter a cui passo il relativo nome 
                Dim sparam As SqlClient.SqlParameter
                sparam = New SqlClient.SqlParameter
                sparam.ParameterName = "@" & dttRow.Item("NomeParametro")
                sparam.SqlDbType = SqlDbType.Int
                MyCommand.Parameters.Add(sparam)

                'per associare un valore al nuovo parametro vado a ciclarmi il datatable contenente i parametri in ingresso 
                Dim dttRowParametri As DataRow
                'ciclo i parametri che ho usato per chiamare il motore e controllare il nome del parametro che si aspetta la store
                For Each dttRowParametri In Parametri.Rows
                    'se il parametro che si aspetta la store è uguale a quello in ingresso assegno al parametro della store il relativo valore del parametro in ingresso
                    If UCase(dttRow.Item("NomeParametro")) = UCase(dttRowParametri.Item("PARAMETRO")) Then
                        MyCommand.Parameters("@" & dttRow.Item("NomeParametro")).Value = dttRowParametri.Item("VALORE")
                        Exit For
                    End If
                Next
            Next
        End If


        Try

            Reader = MyCommand.ExecuteReader()

            Reader.Read()

            RecuperaValore = Reader("valore")
            'RecuperaValore = Reader.GetSqlString(0).ToString

            Reader.Close()
            Reader = Nothing

            Return RecuperaValore

        Catch ex As Exception
            If Not Reader Is Nothing Then
                Reader.Close()
                Reader = Nothing
            End If
            Return "ERRORE"
        End Try
    End Function

    Function CreaDataTable(ByVal QuerySql As String, ByVal conn As SqlClient.SqlConnection) As DataTable
        Dim CMD As New SqlClient.SqlDataAdapter(QuerySql, conn)
        Dim DttPrimario As New DataTable
        CMD.Fill(DttPrimario)
        CreaDataTable = DttPrimario
        DttPrimario = Nothing
    End Function

    '**** utilizzata ***
    Public Property VOL_AssegnazioneVolontariNazionali(ByVal IdVolontario As Integer, ByVal strUserName As String, ByVal intIdCompetenza As Integer, ByVal sqlLocalConn As SqlClient.SqlConnection) As String
        Get
            Dim dtR As DataRow
            Dim dtT As New DataTable

            dtT.Columns.Add(New DataColumn("PARAMETRO", GetType(String)))
            dtT.Columns.Add(New DataColumn("VALORE", GetType(String)))

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDENTITà"
            dtR(1) = IdVolontario

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)

            PathDocumento = EseguiGenerazioneModelloNew(55, strUserName, intIdCompetenza, dtT, sqlLocalConn)

            Return PathDocumento
        End Get
        Set(ByVal Value As String)
            PathDocumento = Value
        End Set
    End Property
    '**** utilizzata ***
    Public Property VOL_AssegnazioneVolontariNazionaliB(ByVal IdVolontario As Integer, ByVal strUserName As String, ByVal intIdCompetenza As Integer, ByVal sqlLocalConn As SqlClient.SqlConnection) As String
        Get
            Dim dtR As DataRow
            Dim dtT As New DataTable

            dtT.Columns.Add(New DataColumn("PARAMETRO", GetType(String)))
            dtT.Columns.Add(New DataColumn("VALORE", GetType(String)))

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDENTITà"
            dtR(1) = IdVolontario

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)

            PathDocumento = EseguiGenerazioneModelloNew(58, strUserName, intIdCompetenza, dtT, sqlLocalConn)

            Return PathDocumento
        End Get
        Set(ByVal Value As String)
            PathDocumento = Value
        End Set
    End Property
    '**** utilizzata ***
    Public Property VOL_AssegnazioneVolontariNazionaliBSCD(ByVal IdVolontario As Integer, ByVal strUserName As String, ByVal intIdCompetenza As Integer, ByVal sqlLocalConn As SqlClient.SqlConnection) As String
        Get
            Dim dtR As DataRow
            Dim dtT As New DataTable

            dtT.Columns.Add(New DataColumn("PARAMETRO", GetType(String)))
            dtT.Columns.Add(New DataColumn("VALORE", GetType(String)))

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDENTITà"
            dtR(1) = IdVolontario

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)

            PathDocumento = EseguiGenerazioneModelloNew(303, strUserName, intIdCompetenza, dtT, sqlLocalConn)

            Return PathDocumento
        End Get
        Set(ByVal Value As String)
            PathDocumento = Value
        End Set
    End Property

    Public Property VOL_AttoAggiuntivoVolontari(ByVal IdVolontario As Integer, ByVal strUserName As String, ByVal intIdCompetenza As Integer, ByVal sqlLocalConn As SqlClient.SqlConnection) As String
        Get
            Dim dtR As DataRow
            Dim dtT As New DataTable

            dtT.Columns.Add(New DataColumn("PARAMETRO", GetType(String)))
            dtT.Columns.Add(New DataColumn("VALORE", GetType(String)))

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDENTITà"
            dtR(1) = IdVolontario

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)

            PathDocumento = EseguiGenerazioneModelloNew(277, strUserName, intIdCompetenza, dtT, sqlLocalConn)

            Return PathDocumento
        End Get
        Set(ByVal Value As String)
            PathDocumento = Value
        End Set
    End Property
    '**** utilizzata ***
    Public Property VOL_AssegnazioneVolontariNazionaliB_Integrativo(ByVal IdVolontario As Integer, ByVal strUserName As String, ByVal intIdCompetenza As Integer, ByVal sqlLocalConn As SqlClient.SqlConnection) As String
        Get
            Dim dtR As DataRow
            Dim dtT As New DataTable

            dtT.Columns.Add(New DataColumn("PARAMETRO", GetType(String)))
            dtT.Columns.Add(New DataColumn("VALORE", GetType(String)))

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDENTITà"
            dtR(1) = IdVolontario

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)

            PathDocumento = EseguiGenerazioneModelloNew(264, strUserName, intIdCompetenza, dtT, sqlLocalConn)

            Return PathDocumento
        End Get
        Set(ByVal Value As String)
            PathDocumento = Value
        End Set
    End Property

    '**** utilizzata ***
    Public Property VOL_SostituzioneVolontariNazionali(ByVal IdVolontario As Integer, ByVal strUserName As String, ByVal intIdCompetenza As Integer, ByVal sqlLocalConn As SqlClient.SqlConnection) As String
        Get
            Dim dtR As DataRow
            Dim dtT As New DataTable

            dtT.Columns.Add(New DataColumn("PARAMETRO", GetType(String)))
            dtT.Columns.Add(New DataColumn("VALORE", GetType(String)))

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDENTITà"
            dtR(1) = IdVolontario

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)

            PathDocumento = EseguiGenerazioneModelloNew(62, strUserName, intIdCompetenza, dtT, sqlLocalConn)

            Return PathDocumento
        End Get
        Set(ByVal Value As String)
            PathDocumento = Value
        End Set
    End Property
    '**** utilizzata ***
    Public Property VOL_SostituzioneVolontariNazionaliB(ByVal IdVolontario As Integer, ByVal strUserName As String, ByVal intIdCompetenza As Integer, ByVal sqlLocalConn As SqlClient.SqlConnection) As String
        Get
            Dim dtR As DataRow
            Dim dtT As New DataTable

            dtT.Columns.Add(New DataColumn("PARAMETRO", GetType(String)))
            dtT.Columns.Add(New DataColumn("VALORE", GetType(String)))

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDENTITà"
            dtR(1) = IdVolontario

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)

            PathDocumento = EseguiGenerazioneModelloNew(63, strUserName, intIdCompetenza, dtT, sqlLocalConn)

            Return PathDocumento
        End Get
        Set(ByVal Value As String)
            PathDocumento = Value
        End Set
    End Property
    '**** utilizzata ***
    Public Property VOL_SostituzioneVolontariNazionaliBSCD(ByVal IdVolontario As Integer, ByVal strUserName As String, ByVal intIdCompetenza As Integer, ByVal sqlLocalConn As SqlClient.SqlConnection) As String
        Get
            Dim dtR As DataRow
            Dim dtT As New DataTable

            dtT.Columns.Add(New DataColumn("PARAMETRO", GetType(String)))
            dtT.Columns.Add(New DataColumn("VALORE", GetType(String)))

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDENTITà"
            dtR(1) = IdVolontario

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)

            PathDocumento = EseguiGenerazioneModelloNew(304, strUserName, intIdCompetenza, dtT, sqlLocalConn)

            Return PathDocumento
        End Get
        Set(ByVal Value As String)
            PathDocumento = Value
        End Set
    End Property

    '**** utilizzata ***
    Public Property VOL_SostituzioneVolontariNazionaliB_Integrativo(ByVal IdVolontario As Integer, ByVal strUserName As String, ByVal intIdCompetenza As Integer, ByVal sqlLocalConn As SqlClient.SqlConnection) As String
        Get
            Dim dtR As DataRow
            Dim dtT As New DataTable

            dtT.Columns.Add(New DataColumn("PARAMETRO", GetType(String)))
            dtT.Columns.Add(New DataColumn("VALORE", GetType(String)))

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDENTITà"
            dtR(1) = IdVolontario

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)

            PathDocumento = EseguiGenerazioneModelloNew(266, strUserName, intIdCompetenza, dtT, sqlLocalConn)

            Return PathDocumento
        End Get
        Set(ByVal Value As String)
            PathDocumento = Value
        End Set
    End Property

    '**** utilizzata ***
    Public Property VOL_AssegnazioneVolontariEstero(ByVal IdVolontario As Integer, ByVal strUserName As String, ByVal intIdCompetenza As Integer, ByVal sqlLocalConn As SqlClient.SqlConnection) As String
        Get
            Dim dtR As DataRow
            Dim dtT As New DataTable

            dtT.Columns.Add(New DataColumn("PARAMETRO", GetType(String)))
            dtT.Columns.Add(New DataColumn("VALORE", GetType(String)))

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDENTITà"
            dtR(1) = IdVolontario

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)

            PathDocumento = EseguiGenerazioneModelloNew(59, strUserName, intIdCompetenza, dtT, sqlLocalConn)

            Return PathDocumento
        End Get
        Set(ByVal Value As String)
            PathDocumento = Value
        End Set
    End Property

    Public Property VOL_AssegnazioneVolontariGaranziaGiovaniB(ByVal IdVolontario As Integer, ByVal strUserName As String, ByVal intIdCompetenza As Integer, ByVal sqlLocalConn As SqlClient.SqlConnection) As String
        Get
            Dim dtR As DataRow
            Dim dtT As New DataTable

            dtT.Columns.Add(New DataColumn("PARAMETRO", GetType(String)))
            dtT.Columns.Add(New DataColumn("VALORE", GetType(String)))

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDENTITà"
            dtR(1) = IdVolontario

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)

            PathDocumento = EseguiGenerazioneModelloNew(204, strUserName, intIdCompetenza, dtT, sqlLocalConn)

            Return PathDocumento
        End Get
        Set(ByVal Value As String)
            PathDocumento = Value
        End Set
    End Property
    Public Property VOL_AssegnazioneVolontariGaranziaGiovaniBSPC(ByVal IdVolontario As Integer, ByVal strUserName As String, ByVal intIdCompetenza As Integer, ByVal sqlLocalConn As SqlClient.SqlConnection) As String
        Get
            Dim dtR As DataRow
            Dim dtT As New DataTable

            dtT.Columns.Add(New DataColumn("PARAMETRO", GetType(String)))
            dtT.Columns.Add(New DataColumn("VALORE", GetType(String)))

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDENTITà"
            dtR(1) = IdVolontario

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)

            PathDocumento = EseguiGenerazioneModelloNew(215, strUserName, intIdCompetenza, dtT, sqlLocalConn)

            Return PathDocumento
        End Get
        Set(ByVal Value As String)
            PathDocumento = Value
        End Set
    End Property
    Public Property VOL_SostituzioneVolontariGaranziaGiovaniB(ByVal IdVolontario As Integer, ByVal strUserName As String, ByVal intIdCompetenza As Integer, ByVal sqlLocalConn As SqlClient.SqlConnection) As String
        Get
            Dim dtR As DataRow
            Dim dtT As New DataTable

            dtT.Columns.Add(New DataColumn("PARAMETRO", GetType(String)))
            dtT.Columns.Add(New DataColumn("VALORE", GetType(String)))

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDENTITà"
            dtR(1) = IdVolontario

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)

            PathDocumento = EseguiGenerazioneModelloNew(206, strUserName, intIdCompetenza, dtT, sqlLocalConn)

            Return PathDocumento
        End Get
        Set(ByVal Value As String)
            PathDocumento = Value
        End Set
    End Property
    Public Property VOL_SostituzioneVolontariGaranziaGiovaniBSPC(ByVal IdVolontario As Integer, ByVal strUserName As String, ByVal intIdCompetenza As Integer, ByVal sqlLocalConn As SqlClient.SqlConnection) As String
        Get
            Dim dtR As DataRow
            Dim dtT As New DataTable

            dtT.Columns.Add(New DataColumn("PARAMETRO", GetType(String)))
            dtT.Columns.Add(New DataColumn("VALORE", GetType(String)))

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDENTITà"
            dtR(1) = IdVolontario

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)

            PathDocumento = EseguiGenerazioneModelloNew(216, strUserName, intIdCompetenza, dtT, sqlLocalConn)

            Return PathDocumento
        End Get
        Set(ByVal Value As String)
            PathDocumento = Value
        End Set
    End Property
    '**** utilizzata ***
    Public Property VOL_AssegnazioneVolontariEsteroB(ByVal IdVolontario As Integer, ByVal strUserName As String, ByVal intIdCompetenza As Integer, ByVal sqlLocalConn As SqlClient.SqlConnection) As String
        Get
            Dim dtR As DataRow
            Dim dtT As New DataTable

            dtT.Columns.Add(New DataColumn("PARAMETRO", GetType(String)))
            dtT.Columns.Add(New DataColumn("VALORE", GetType(String)))

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDENTITà"
            dtR(1) = IdVolontario

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)

            PathDocumento = EseguiGenerazioneModelloNew(60, strUserName, intIdCompetenza, dtT, sqlLocalConn)

            Return PathDocumento
        End Get
        Set(ByVal Value As String)
            PathDocumento = Value
        End Set
    End Property

    '**** utilizzata ***
    Public Property VOL_AssegnazioneVolontariEsteroB_Integrativo(ByVal IdVolontario As Integer, ByVal strUserName As String, ByVal intIdCompetenza As Integer, ByVal sqlLocalConn As SqlClient.SqlConnection) As String
        Get
            Dim dtR As DataRow
            Dim dtT As New DataTable

            dtT.Columns.Add(New DataColumn("PARAMETRO", GetType(String)))
            dtT.Columns.Add(New DataColumn("VALORE", GetType(String)))

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDENTITà"
            dtR(1) = IdVolontario

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)

            PathDocumento = EseguiGenerazioneModelloNew(265, strUserName, intIdCompetenza, dtT, sqlLocalConn)

            Return PathDocumento
        End Get
        Set(ByVal Value As String)
            PathDocumento = Value
        End Set
    End Property

    Public Property VOL_CertificatoServizioSvolto(ByVal IdVolontario As Integer, ByVal strUserName As String, ByVal intIdCompetenza As Integer, ByVal sqlLocalConn As SqlClient.SqlConnection) As String
        Get
            Dim dtR As DataRow
            Dim dtT As New DataTable

            dtT.Columns.Add(New DataColumn("PARAMETRO", GetType(String)))
            dtT.Columns.Add(New DataColumn("VALORE", GetType(String)))

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDENTITà"
            dtR(1) = IdVolontario

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)

            PathDocumento = EseguiGenerazioneModelloNew(319, strUserName, intIdCompetenza, dtT, sqlLocalConn)

            Return PathDocumento
        End Get
        Set(ByVal Value As String)
            PathDocumento = Value
        End Set
    End Property

    '**** utilizzata ***
    Public Property VOL_SostituzioneVolontariEsteri(ByVal IdVolontario As Integer, ByVal strUserName As String, ByVal intIdCompetenza As Integer, ByVal sqlLocalConn As SqlClient.SqlConnection) As String
        Get
            Dim dtR As DataRow
            Dim dtT As New DataTable

            dtT.Columns.Add(New DataColumn("PARAMETRO", GetType(String)))
            dtT.Columns.Add(New DataColumn("VALORE", GetType(String)))

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDENTITà"
            dtR(1) = IdVolontario

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)

            PathDocumento = EseguiGenerazioneModelloNew(64, strUserName, intIdCompetenza, dtT, sqlLocalConn)

            Return PathDocumento
        End Get
        Set(ByVal Value As String)
            PathDocumento = Value
        End Set
    End Property
    '**** utilizzata ***
    Public Property VOL_SostituzioneVolontariEsteriB(ByVal IdVolontario As Integer, ByVal strUserName As String, ByVal intIdCompetenza As Integer, ByVal sqlLocalConn As SqlClient.SqlConnection) As String
        Get
            Dim dtR As DataRow
            Dim dtT As New DataTable

            dtT.Columns.Add(New DataColumn("PARAMETRO", GetType(String)))
            dtT.Columns.Add(New DataColumn("VALORE", GetType(String)))

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDENTITà"
            dtR(1) = IdVolontario

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)

            PathDocumento = EseguiGenerazioneModelloNew(65, strUserName, intIdCompetenza, dtT, sqlLocalConn)

            Return PathDocumento
        End Get
        Set(ByVal Value As String)
            PathDocumento = Value
        End Set
    End Property

    '**** utilizzata ***
    Public Property VOL_SostituzioneVolontariEsteriB_Integrativo(ByVal IdVolontario As Integer, ByVal strUserName As String, ByVal intIdCompetenza As Integer, ByVal sqlLocalConn As SqlClient.SqlConnection) As String
        Get
            Dim dtR As DataRow
            Dim dtT As New DataTable

            dtT.Columns.Add(New DataColumn("PARAMETRO", GetType(String)))
            dtT.Columns.Add(New DataColumn("VALORE", GetType(String)))

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDENTITà"
            dtR(1) = IdVolontario

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)

            PathDocumento = EseguiGenerazioneModelloNew(267, strUserName, intIdCompetenza, dtT, sqlLocalConn)

            Return PathDocumento
        End Get
        Set(ByVal Value As String)
            PathDocumento = Value
        End Set
    End Property

    Function VOL_DataMatrix(ByVal IdEntità As Integer, ByVal localConn As SqlClient.SqlConnection) As String
        Dim strsql As String
        Dim strCodiceVolontario As String = ""
        Dim myCommand As SqlClient.SqlCommand
        Dim dtrVol As SqlClient.SqlDataReader

        Dim strValoriMultipli As String = ""
        Dim chiamataWSHeliosUtility As New WSHeliosUtility.WSHeliosUtility
        chiamataWSHeliosUtility.Url = ConfigurationSettings.AppSettings("URL_WS_HeliosUtility")


        myCommand = New SqlClient.SqlCommand
        'passo la connessione al command
        myCommand.Connection = localConn
        'costruisco la query per prendere le informazioni del modello (PATH E NOME)
        myCommand.CommandText = "Select CodiceVolontario From Entità where identità = " & IdEntità
        'eseguo la query
        dtrVol = myCommand.ExecuteReader
        If dtrVol.HasRows = True Then
            dtrVol.Read()
            strCodiceVolontario = dtrVol("CodiceVolontario")
        End If

        'chiudo il datareader
        If Not dtrVol Is Nothing Then
            dtrVol.Close()
            dtrVol = Nothing
        End If
        Dim strData As String = Day(Now) & "/" & Month(Now) & "/" & Year(Now)
        strValoriMultipli = chiamataWSHeliosUtility.DataMatrixToRtfString(strCodiceVolontario & "#" & strData)


        'passo la stringa con i valori multipli alla funzione per il valore di ritorno
        VOL_DataMatrix = strValoriMultipli

        'ritorno la stringa
        Return VOL_DataMatrix

    End Function
 

    'Private Function ParamentriBarraCAD(ByVal intIdModello As Integer, ByVal xlinea As String, ByVal Parametri As DataTable, Optional ByRef CodiceSede As Integer = 0) As String
    '    '** CREATA DA SIMONA CORDELLA IL 02/11/2010
    '    '** MODIFICO DOCUMENTI DELLE VERIFICHE
    '    '** Ricavo i paramentri che vado a scrivere nel documento .rtf
    '    '** ChiaveID = (idprogrammazione o idverifica)
    '    '** IdSoggetto =0 (sede) 
    '    '**            >0 EN (idente) se è presente il codice SA (sedeattuazione) o altro con il CodiceSedee nel documento è presente l'indirizzo del destinatario
    '    Dim strH As String = "H"
    '    Dim Sede As Integer = 0
    '    Dim ChiaveID As Integer
    '    Dim IdSoggetto As String
    '    Dim IdFascicolo As String = 0
    '    Dim IdServizio As String = ""
    '    Dim Classificazione As String
    '    Dim Titolario As String
    '    Dim Determina As String
    '    Dim IdBando As Integer

    '    '** aggiunto da simona cordella il 21/01/2011
    '    Classificazione = TrovaClassificazioneModelli(intIdModello, Session("Conn"))
    '    Titolario = Mid(Classificazione, 1, InStr(Classificazione, "#") - 1)
    '    Determina = Mid(Classificazione, InStr(Classificazione, "#") + 1)

    '    '***
    '    xlinea = Mid(xlinea, 1, xlinea.Length - 1)
    '    xlinea = xlinea & "{\*\docvar {Sistema}{" & strH & "}}"

    '    Select Case intIdModello
    '        Case 78 'trasmissioneprogrammazione.rtf
    '            Dim dttRowParametri As DataRow
    '            For Each dttRowParametri In Parametri.Rows
    '                If UCase("IdProgrammazione") = UCase(dttRowParametri.Item("PARAMETRO")) Then
    '                    ChiaveID = dttRowParametri.Item("VALORE")
    '                End If
    '            Next
    '            IdSoggetto = 0
    '            IdFascicolo = TrovaIdFascicoloTVerificheProgrammazione(ChiaveID, Session("Conn"))
    '        Case 79 'letterainterlocutoria.rtf
    '            Dim dttRowParametri As DataRow
    '            For Each dttRowParametri In Parametri.Rows
    '                If UCase("IdVerifica") = UCase(dttRowParametri.Item("PARAMETRO")) Then
    '                    ChiaveID = dttRowParametri.Item("VALORE")
    '                End If
    '                If UCase("IdEnte") = UCase(dttRowParametri.Item("PARAMETRO")) Then
    '                    IdSoggetto = "EN" & dttRowParametri.Item("VALORE") & "#" & "SA" & CodiceSede
    '                End If
    '            Next
    '            IdFascicolo = TrovaIdFascicoloTVerifiche(ChiaveID, Session("Conn"))
    '        Case 109 'letteraincaricomultipla.rtf
    '            Dim dttRowParametri As DataRow
    '            For Each dttRowParametri In Parametri.Rows
    '                If UCase("IdGruppoStampa") = UCase(dttRowParametri.Item("PARAMETRO")) Then
    '                    ChiaveID = dttRowParametri.Item("VALORE")
    '                End If
    '            Next
    '            IdSoggetto = 0
    '        Case 80 'credenziali.rtf
    '            Dim dttRowParametri As DataRow
    '            For Each dttRowParametri In Parametri.Rows
    '                If UCase("IdVerifica") = UCase(dttRowParametri.Item("PARAMETRO")) Then
    '                    ChiaveID = dttRowParametri.Item("VALORE")
    '                End If
    '                If UCase("IdEnte") = UCase(dttRowParametri.Item("PARAMETRO")) Then
    '                    IdSoggetto = "EN" & dttRowParametri.Item("VALORE") & "#" & "SA" & CodiceSede
    '                End If
    '            Next
    '            IdFascicolo = TrovaIdFascicoloTVerifiche(ChiaveID, Session("Conn"))
    '        Case 81 'letteradiincarico.rtf
    '            Dim dttRowParametri As DataRow
    '            For Each dttRowParametri In Parametri.Rows
    '                If UCase("IdVerifica") = UCase(dttRowParametri.Item("PARAMETRO")) Then
    '                    ChiaveID = dttRowParametri.Item("VALORE")
    '                End If
    '            Next
    '            IdSoggetto = 0
    '            IdFascicolo = TrovaIdFascicoloTVerifiche(ChiaveID, Session("Conn"))
    '        Case 102 'credenzialiIGF.rtf
    '            Dim dttRowParametri As DataRow
    '            For Each dttRowParametri In Parametri.Rows
    '                If UCase("IdVerifica") = UCase(dttRowParametri.Item("PARAMETRO")) Then
    '                    ChiaveID = dttRowParametri.Item("VALORE")
    '                End If
    '                If UCase("IdEnte") = UCase(dttRowParametri.Item("PARAMETRO")) Then
    '                    IdSoggetto = "EN" & dttRowParametri.Item("VALORE") & "#" & "SA" & CodiceSede
    '                End If
    '            Next
    '            IdFascicolo = TrovaIdFascicoloTVerifiche(ChiaveID, Session("Conn"))
    '        Case 104 'letteradiincaricoIGF.rtf
    '            Dim dttRowParametri As DataRow
    '            For Each dttRowParametri In Parametri.Rows
    '                If UCase("IdVerifica") = UCase(dttRowParametri.Item("PARAMETRO")) Then
    '                    ChiaveID = dttRowParametri.Item("VALORE")
    '                End If
    '            Next
    '            IdSoggetto = 0
    '            IdFascicolo = TrovaIdFascicoloTVerifiche(ChiaveID, Session("Conn"))
    '        Case 83 'conclusioneVerificaPositiva.rtf
    '            Dim dttRowParametri As DataRow
    '            For Each dttRowParametri In Parametri.Rows
    '                If UCase("IdVerifica") = UCase(dttRowParametri.Item("PARAMETRO")) Then
    '                    ChiaveID = dttRowParametri.Item("VALORE")
    '                End If
    '                If UCase("IdEnte") = UCase(dttRowParametri.Item("PARAMETRO")) Then
    '                    IdSoggetto = "EN" & dttRowParametri.Item("VALORE")
    '                    'mod. il 24/05/2012 da s.c. la lettere viene inviata solo all'ente e nn più alla sede di progetto 
    '                    '& "#" & "SA" & CodiceSede
    '                End If
    '            Next
    '            IdFascicolo = TrovaIdFascicoloTVerifiche(ChiaveID, Session("Conn"))
    '        Case 84 'VerificasenzaIrregolaritàConRichiamo.rtf
    '            Dim dttRowParametri As DataRow
    '            For Each dttRowParametri In Parametri.Rows
    '                If UCase("IdVerifica") = UCase(dttRowParametri.Item("PARAMETRO")) Then
    '                    ChiaveID = dttRowParametri.Item("VALORE")
    '                End If
    '                If UCase("IdEnte") = UCase(dttRowParametri.Item("PARAMETRO")) Then
    '                    IdSoggetto = "EN" & dttRowParametri.Item("VALORE")
    '                    'mod. il 24/05/2012 da s.c. la lettere viene inviata solo all'ente e nn più alla sede di progetto 
    '                    '& "#" & "SA" & CodiceSede
    '                End If
    '            Next
    '            IdFascicolo = TrovaIdFascicoloTVerifiche(ChiaveID, Session("Conn"))
    '        Case 82 'trasmisisonerelazionealDG.rtf
    '            Dim dttRowParametri As DataRow
    '            For Each dttRowParametri In Parametri.Rows
    '                If UCase("IdVerifica") = UCase(dttRowParametri.Item("PARAMETRO")) Then
    '                    ChiaveID = dttRowParametri.Item("VALORE")
    '                End If
    '            Next
    '            IdSoggetto = 0
    '            IdFascicolo = TrovaIdFascicoloTVerifiche(ChiaveID, Session("Conn"))
    '        Case 93 'trasmissionerelazioneconrichiamo.rtf
    '            Dim dttRowParametri As DataRow
    '            For Each dttRowParametri In Parametri.Rows
    '                If UCase("IdVerifica") = UCase(dttRowParametri.Item("PARAMETRO")) Then
    '                    ChiaveID = dttRowParametri.Item("VALORE")
    '                End If
    '            Next
    '            IdSoggetto = 0
    '            IdFascicolo = TrovaIdFascicoloTVerifiche(ChiaveID, Session("Conn"))
    '        Case 85 'LetteraContestazioneAddebiti.rtf
    '            'NO CODICE SEDE
    '            Dim dttRowParametri As DataRow
    '            For Each dttRowParametri In Parametri.Rows
    '                If UCase("IdVerifica") = UCase(dttRowParametri.Item("PARAMETRO")) Then
    '                    ChiaveID = dttRowParametri.Item("VALORE")
    '                End If
    '                If UCase("IdEnte") = UCase(dttRowParametri.Item("PARAMETRO")) Then
    '                    IdSoggetto = "EN" & dttRowParametri.Item("VALORE")
    '                    '& "#" & "SA" & CodiceSede
    '                End If
    '            Next
    '            IdFascicolo = TrovaIdFascicoloTVerifiche(ChiaveID, Session("Conn"))
    '        Case 87 'letteratrasmdgcontestazione.rtf
    '            Dim dttRowParametri As DataRow
    '            For Each dttRowParametri In Parametri.Rows
    '                If UCase("IdVerifica") = UCase(dttRowParametri.Item("PARAMETRO")) Then
    '                    ChiaveID = dttRowParametri.Item("VALORE")
    '                End If
    '            Next
    '            IdSoggetto = 0
    '            IdFascicolo = TrovaIdFascicoloTVerifiche(ChiaveID, Session("Conn"))
    '        Case 101 'ContestazioneChiusura.rtf
    '            Dim dttRowParametri As DataRow
    '            'NO CODICE SEDE
    '            For Each dttRowParametri In Parametri.Rows
    '                If UCase("IdVerifica") = UCase(dttRowParametri.Item("PARAMETRO")) Then
    '                    ChiaveID = dttRowParametri.Item("VALORE")
    '                End If
    '                If UCase("IdEnte") = UCase(dttRowParametri.Item("PARAMETRO")) Then
    '                    IdSoggetto = "EN" & dttRowParametri.Item("VALORE")
    '                    '& "#" & "SA" & CodiceSede
    '                End If
    '            Next
    '            IdFascicolo = TrovaIdFascicoloTVerifiche(ChiaveID, Session("Conn"))
    '        Case 111 'TrasmissioneSanzioneDG.rtf
    '            Dim dttRowParametri As DataRow
    '            For Each dttRowParametri In Parametri.Rows
    '                If UCase("IdVerifica") = UCase(dttRowParametri.Item("PARAMETRO")) Then
    '                    ChiaveID = dttRowParametri.Item("VALORE")
    '                End If
    '            Next
    '            IdSoggetto = 0
    '            IdFascicolo = TrovaIdFascicoloTVerifiche(ChiaveID, Session("Conn"))
    '        Case 92 'LetteraTRasmissioneProvvedimentoSanzionatorio.rtf
    '            Dim dttRowParametri As DataRow
    '            'NO CODICE SEDE
    '            For Each dttRowParametri In Parametri.Rows
    '                If UCase("IdVerifica") = UCase(dttRowParametri.Item("PARAMETRO")) Then
    '                    ChiaveID = dttRowParametri.Item("VALORE")
    '                End If
    '                If UCase("IdEnte") = UCase(dttRowParametri.Item("PARAMETRO")) Then
    '                    IdSoggetto = "EN" & dttRowParametri.Item("VALORE")
    '                    '& "#" & "SA" & CodiceSede
    '                End If
    '            Next
    '            IdFascicolo = TrovaIdFascicoloTVerifiche(ChiaveID, Session("Conn"))
    '        Case 94 'letteatrasmisprovaiservizi.rtf
    '            Dim dttRowParametri As DataRow
    '            For Each dttRowParametri In Parametri.Rows
    '                If UCase("IdVerifica") = UCase(dttRowParametri.Item("PARAMETRO")) Then
    '                    ChiaveID = dttRowParametri.Item("VALORE")
    '                End If
    '            Next
    '            IdSoggetto = 0
    '            IdFascicolo = TrovaIdFascicoloTVerifiche(ChiaveID, Session("Conn"))


    '            'ricavo l'idservizo solo per questo documento
    '            IdServizio = TrovaIDServizioSanzione(ChiaveID, 94, Session("Conn"))


    '        Case 35 To 43
    '            'gestione della documentazione dei Progetti id modelli interessati (35 ,36,37,38,39,40,41,42,43)
    '            'aggiounto da simona cordella il 28/05/2012

    '            Dim dttRowParametri As DataRow
    '            'NO CODICE SEDE
    '            For Each dttRowParametri In Parametri.Rows

    '                If UCase("IdEnte") = UCase(dttRowParametri.Item("PARAMETRO")) Then
    '                    IdSoggetto = "EN" & dttRowParametri.Item("VALORE")
    '                    '& "#" & "SA" & CodiceSede
    '                End If
    '                If UCase("IdBando") = UCase(dttRowParametri.Item("PARAMETRO")) Then
    '                    IdBando = dttRowParametri.Item("VALORE")
    '                    'ChiaveID = dttRowParametri.Item("VALORE")
    '                End If
    '            Next
    '            ChiaveID = RecuperaIdBandoAttività(IdBando, Session("IdEnte"), Session("Conn"))
    '            IdFascicolo = TrovaIdFascicoloProgetti(ChiaveID, Session("Conn"))
    '    End Select
    '    xlinea = xlinea & "{\*\docvar {ChiaveID}{" & ChiaveID & "}}"
    '    xlinea = xlinea & "{\*\docvar {IdModello}{" & intIdModello & "}}"
    '    xlinea = xlinea & "{\*\docvar {IdSoggetto}{" & IdSoggetto & "}}"
    '    '*** modificato il 21/01/2011 (IdFascicolo, Titolario e TipoDocumento(Lettera o Determina)
    '    xlinea = xlinea & "{\*\docvar {IdFascicolo}{" & IdFascicolo & "}}"
    '    xlinea = xlinea & "{\*\docvar {Titolario}{" & Titolario & "}}"
    '    xlinea = xlinea & "{\*\docvar {TipoDocumento}{" & Determina & "}}"
    '    If IdServizio <> "" Then
    '        xlinea = xlinea & "{\*\docvar {IdServizio}{" & IdServizio & "}}"
    '    End If
    '    '****
    '    xlinea = xlinea & "}"
    '    Return xlinea
    'End Function

    'Private Function TrovaIdFascicoloTVerifiche(ByVal IdVerifica As Integer, ByVal sqlLocalConn As SqlClient.SqlConnection) As String
    '    'Creato da Simona Cordella il 21/01/2011
    '    'Trovo l'idfascicolo nella tabella TVerifiche
    '    'modificato il 02/02/2012 normalizzo l'id fascicolo di tverifche con l'id fasccilo completo di siged
    '    'function di CLSSIGED SIGED_IdFascicoloCompleto()
    '    Dim IdFascicolo As String
    '    Dim dtrIdFasc As SqlClient.SqlDataReader
    '    Dim strsql As String
    '    Dim SIGED As New clsSiged
    '    strsql = "Select isnull(IdFascicolo,0)as IdFascicolo  From TVerifiche where idVerifica = " & IdVerifica
    '    dtrIdFasc = ClsServer.CreaDatareader(strsql, sqlLocalConn)
    '    If dtrIdFasc.HasRows = True Then
    '        dtrIdFasc.Read()
    '        IdFascicolo = dtrIdFasc("IdFascicolo")
    '    End If
    '    dtrIdFasc.Close()
    '    dtrIdFasc = Nothing
    '    Return SIGED.SIGED_IdFascicoloCompleto(IdFascicolo)
    'End Function

    'Private Function TrovaIdFascicoloTVerificheProgrammazione(ByVal IdProgrammazione As Integer, ByVal sqlLocalConn As SqlClient.SqlConnection) As String
    '    'Creato da Simona Cordella il 21/01/2011
    '    'Trovo l'idfascicolo nella tabella TVerificheProgrammazione
    '    Dim IdFascicolo As String
    '    Dim dtrIdFasc As SqlClient.SqlDataReader
    '    Dim strsql As String

    '    strsql = "Select isnull(IdFascicolo,0)as IdFascicolo  From TVerificheProgrammazione where IdProgrammazione = " & IdProgrammazione
    '    dtrIdFasc = ClsServer.CreaDatareader(strsql, sqlLocalConn)
    '    If dtrIdFasc.HasRows = True Then
    '        dtrIdFasc.Read()
    '        IdFascicolo = dtrIdFasc("IdFascicolo")
    '    End If
    '    dtrIdFasc.Close()
    '    dtrIdFasc = Nothing
    '    Return IdFascicolo
    'End Function

    'Private Function TrovaClassificazioneModelli(ByVal IdModello As Integer, ByVal sqlLocalConn As SqlClient.SqlConnection) As String
    '    'Creato da Simona Cordella il 21/01/2011
    '    'Trovo il titolario a secondo del modello che ho selezionato
    '    'LA variabile Classificazione riporta il titolario e la determina divisi dal #
    '    Dim Classificazione As String

    '    Dim dtrClass As SqlClient.SqlDataReader
    '    Dim strsql As String

    '    strsql = "Select isnull(Titolario,'') as Titolario,isnull(TipoDocumento,'') as TipoDocumento From Editor_Modelli where idModello = " & IdModello
    '    dtrClass = ClsServer.CreaDatareader(strsql, sqlLocalConn)
    '    If dtrClass.HasRows = True Then
    '        dtrClass.Read()
    '        Classificazione = dtrClass("Titolario") & "#" & dtrClass("TipoDocumento")
    '    End If
    '    dtrClass.Close()
    '    dtrClass = Nothing
    '    Return Classificazione
    'End Function

    'Private Function TrovaIDServizioSanzione(ByVal IdVerifica As Integer, ByVal IDModello As Integer, ByVal sqlLocalConn As SqlClient.SqlConnection) As String
    '    'Creato da Simona Cordella il 04/03/2011
    '    'Trovo l'idserivio per le verifiche sanzionate 
    '    'viene stampato solo x il documento 94 (letteatrasmisprovaiservizi.rtf)
    '    Dim IdSerivizo As String = ""
    '    Dim strsql As String
    '    Dim dtrServizi As SqlClient.SqlDataReader

    '    strsql = " Select isnull(IdServizio,0)as IdServizio from TVerificheServizi where IdVerifica = " & IdVerifica & " and IDModello= " & IDModello & ""
    '    dtrServizi = ClsServer.CreaDatareader(strsql, sqlLocalConn)
    '    Do While dtrServizi.Read()
    '        If IdSerivizo = "" Then
    '            IdSerivizo = dtrServizi("IdServizio")
    '        Else
    '            IdSerivizo = IdSerivizo & "#" & dtrServizi("IdServizio")
    '        End If
    '    Loop
    '    dtrServizi.Close()
    '    dtrServizi = Nothing
    '    Return IdSerivizo
    'End Function

    'Private Function RecuperaIdBandoAttività(ByVal IDBando As Integer, ByVal IdEnte As Integer, ByVal sqlLocalConn As SqlClient.SqlConnection) As Integer
    '    'Creato da Simona Cordella il 25/05/2012
    '    'Tova idbandoattivita per la stampa della documentazione dei progetti

    '    Dim strsql As String
    '    Dim dtrBandi As SqlClient.SqlDataReader
    '    Dim IdBA As Integer

    '    strsql = " select idbandoattività from bandiattività where idbando = " & IDBando & " and idente = " & IdEnte & ""
    '    dtrBandi = ClsServer.CreaDatareader(strsql, sqlLocalConn)
    '    If dtrBandi.HasRows = True Then
    '        dtrBandi.Read()
    '        IdBA = dtrBandi("idbandoattività")
    '    End If
    '    dtrBandi.Close()
    '    dtrBandi = Nothing
    '    Return IdBA
    'End Function

    'Private Function TrovaIdFascicoloProgetti(ByVal IDBandoAttivita As Integer, ByVal sqlLocalConn As SqlClient.SqlConnection) As String
    '    'Creato da Simona Cordella il 28/05/2012
    '    'Trovo l'idfascicolo nella tabella TVerifiche
    '    'modificato il 02/02/2012 normalizzo l'id fascicolo di tverifche con l'id fasccilo completo di siged
    '    'function di CLSSIGED SIGED_IdFascicoloCompleto()
    '    Dim IdFascicolo As String
    '    Dim dtrIdFasc As SqlClient.SqlDataReader
    '    Dim strsql As String
    '    Dim SIGED As New clsSiged
    '    strsql = "Select isnull(IdFascicoloPC,0)as IdFascicolo  From bandiattività where idbandoattività = " & IDBandoAttivita
    '    dtrIdFasc = ClsServer.CreaDatareader(strsql, sqlLocalConn)
    '    If dtrIdFasc.HasRows = True Then
    '        dtrIdFasc.Read()
    '        IdFascicolo = dtrIdFasc("IdFascicolo")
    '    End If
    '    dtrIdFasc.Close()
    '    dtrIdFasc = Nothing
    '    If IdFascicolo = "0" Then
    '        Return IdFascicolo
    '    Else
    '        Return SIGED.SIGED_IdFascicoloCompleto(IdFascicolo)
    '    End If
    'End Function


   
End Class

