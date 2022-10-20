Imports System.IO
Imports System.Configuration.ConfigurationManager
Imports System.Web.SessionState.HttpSessionState
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

        'istanzio il command
        myCommand = New SqlClient.SqlCommand
        'passo la connessione al command
        myCommand.Connection = localConn


        'costruisco la query per prendere le informazioni del modello (PATH E NOME)
        myCommand.CommandText = "SELECT Editor_Modelli.NomeFisico, Editor_ModelliCompetenze.Path, Editor_ModelliCompetenze.PathLocale from Editor_ModelliCompetenze INNER JOIN Editor_Modelli ON Editor_Modelli.IdModello = Editor_ModelliCompetenze.IdModello WHERE Editor_ModelliCompetenze.IdModello=" & intIdModello & " AND Editor_ModelliCompetenze.IdRegioneCompetenza=" & intIdCompetenza
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
            strPercorsoFile = HttpContext.Current.Server.MapPath("./documentazione/") & strNomeFile
            'passo alla variabile userò come valore di ritorno il percorso del file appena creato
            strPercorsoFileGenerato = "./documentazione/" & strNomeFile

            '*****************************************MODIFICA FATTA IL 07.02.2008*************************************
            '*********************************************da BAGNARACK JOBAMA******************************************
            'vado a prendere, tramite Web Service, il template di cui si vuole genererare il file
            'il template verrà salvato su una cartella locale, così che possa essere letto
            'tranquillamente dallo streamreader

            'istanzio il Web Service WSDocumentazione
            Dim wsLocal As New WS_Editor.WSMetodiDocumentazione

            wsLocal.Url = AppSettings("URL_WS_Documentazione")

            If AppSettings("IsTest") <> "1" Then

                'dichiaro una varibbile byte che bufferizza (carica in memoria) il file template richiesto
                'e trasformato in base64
                Dim dataBuffer As Byte() = Convert.FromBase64String(wsLocal.RecuperaTemplate(strPercorsoTemplate, strNomeTemplate))
                'variabile stream che in streaming scrive il template sulla macchina che richiede il documento 
                Dim fs As FileStream
                'passo il template al filestream
                fs = New FileStream(HttpContext.Current.Server.MapPath(strPathLocale & strNomeTemplate), FileMode.Create, FileAccess.Write)
                'ciclo il file bufferizzato e scrivo nel file tramite lo streaming del FileStream
                If (dataBuffer.Length > 0) Then
                    fs.Write(dataBuffer, 0, dataBuffer.Length)
                End If

                'chiudo lo streaming
                fs.Close()

            End If

            'apro il file che fa da template
            Reader = New StreamReader(HttpContext.Current.Server.MapPath(Replace(strPathLocale, "\", "/")) & strNomeTemplate, System.Text.Encoding.Default, False)
            Writer = New StreamWriter(strPercorsoFile, True, System.Text.Encoding.Default)

            'inizio a leggere il template
            'xLinea = Reader.ReadLine()

            xLinea = Reader.ReadToEnd()

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
                            'controllo il nome della routine e chiamo la relativa routine
                            Select Case dttRow.Item("NomeRoutine")
                                'elenco sedi accreditate
                                Case "ACCR_ElencoSediAccreditate"
                                    Dim dttRowParametri As DataRow
                                    'scorro tutti i parametri in ingresso per poter trovare il parametro necessario per il caricamento dei valori
                                    For Each dttRowParametri In Parametri.Rows
                                        'se il parametro nel datatable è lo stesso che mi occorre vado a prendere il relativo valore
                                        'e lo passo alla routine che mi restituisce i valori multipli in una stringa
                                        If UCase("IdEnte") = UCase(dttRowParametri.Item("PARAMETRO")) Then
                                            'faccio la replace del valore con la lista
                                            xLinea = Replace(xLinea, dttRow.Item("NomeTag"), FixText(ACCR_ElencoSediAccreditate(dttRowParametri.Item("VALORE"))))
                                        End If
                                    Next
                                    'elenco sedi accreditateparziale
                                Case "ACCR_ElencoSediAccreditateParziale"
                                    Dim dttRowParametri As DataRow
                                    'scorro tutti i parametri in ingresso per poter trovare il parametro necessario per il caricamento dei valori
                                    For Each dttRowParametri In Parametri.Rows
                                        'se il parametro nel datatable è lo stesso che mi occorre vado a prendere il relativo valore
                                        'e lo passo alla routine che mi restituisce i valori multipli in una stringa
                                        If UCase("IdEnteFase") = UCase(dttRowParametri.Item("PARAMETRO")) Then
                                            'faccio la replace del valore con la lista
                                            xLinea = Replace(xLinea, dttRow.Item("NomeTag"), FixText(ACCR_ElencoSediAccreditateParziale(dttRowParametri.Item("VALORE"))))
                                        End If
                                    Next

                                    'elenco risorse accreditate
                                Case "ACCR_ElencoRisorseAccreditate"
                                    Dim dttRowParametri As DataRow
                                    'scorro tutti i parametri in ingresso per poter trovare il parametro necessario per il caricamento dei valori
                                    For Each dttRowParametri In Parametri.Rows
                                        'se il parametro nel datatable è lo stesso che mi occorre vado a prendere il relativo valore
                                        'e lo passo alla routine che mi restituisce i valori multipli in una stringa
                                        If UCase("IdEnte") = UCase(dttRowParametri.Item("PARAMETRO")) Then
                                            'faccio la replace del valore con la lista
                                            xLinea = Replace(xLinea, dttRow.Item("NomeTag"), FixText(ACCR_ElencoRisorseAccreditate(dttRowParametri.Item("VALORE"))))
                                        End If
                                    Next
                                    'elenco risorse accreditate parziale
                                Case "ACCR_ElencoRisorseAccreditateParziale"
                                    Dim dttRowParametri As DataRow
                                    'scorro tutti i parametri in ingresso per poter trovare il parametro necessario per il caricamento dei valori
                                    For Each dttRowParametri In Parametri.Rows
                                        'se il parametro nel datatable è lo stesso che mi occorre vado a prendere il relativo valore
                                        'e lo passo alla routine che mi restituisce i valori multipli in una stringa
                                        If UCase("IdEnteFase") = UCase(dttRowParametri.Item("PARAMETRO")) Then
                                            'faccio la replace del valore con la lista
                                            xLinea = Replace(xLinea, dttRow.Item("NomeTag"), FixText(ACCR_ElencoRisorseAccreditateParziale(dttRowParametri.Item("VALORE"))))
                                        End If
                                    Next
                                    'elenco servizi accreditati
                                Case "ACCR_ElencoServizi"
                                    Dim dttRowParametri As DataRow
                                    'scorro tutti i parametri in ingresso per poter trovare il parametro necessario per il caricamento dei valori
                                    For Each dttRowParametri In Parametri.Rows
                                        'se il parametro nel datatable è lo stesso che mi occorre vado a prendere il relativo valore
                                        'e lo passo alla routine che mi restituisce i valori multipli in una stringa
                                        If UCase("IdEnte") = UCase(dttRowParametri.Item("PARAMETRO")) Then
                                            'faccio la replace del valore con la lista
                                            xLinea = Replace(xLinea, dttRow.Item("NomeTag"), FixText(ACCR_ElencoServizi(dttRowParametri.Item("VALORE"))))
                                        End If
                                    Next
                                    'elenco servizi accreditati
                                Case "ACCR_ElencoServiziParziale"
                                    Dim dttRowParametri As DataRow
                                    'scorro tutti i parametri in ingresso per poter trovare il parametro necessario per il caricamento dei valori
                                    For Each dttRowParametri In Parametri.Rows
                                        'se il parametro nel datatable è lo stesso che mi occorre vado a prendere il relativo valore
                                        'e lo passo alla routine che mi restituisce i valori multipli in una stringa
                                        If UCase("IdEnteFase") = UCase(dttRowParametri.Item("PARAMETRO")) Then
                                            'faccio la replace del valore con la lista
                                            xLinea = Replace(xLinea, dttRow.Item("NomeTag"), FixText(ACCR_ElencoServiziParziale(dttRowParametri.Item("VALORE"))))
                                        End If
                                    Next
                                Case "ACCR_ElencoEntiEsclusi"
                                    Dim dttRowParametri As DataRow
                                    'scorro tutti i parametri in ingresso per poter trovare il parametro necessario per il caricamento dei valori
                                    For Each dttRowParametri In Parametri.Rows
                                        'se il parametro nel datatable è lo stesso che mi occorre vado a prendere il relativo valore
                                        'e lo passo alla routine che mi restituisce i valori multipli in una stringa
                                        If UCase("IdEnte") = UCase(dttRowParametri.Item("PARAMETRO")) Then
                                            'faccio la replace del valore con la lista
                                            xLinea = Replace(xLinea, dttRow.Item("NomeTag"), FixText(ACCR_ElencoEntiEsclusi(dttRowParametri.Item("VALORE"))))
                                        End If
                                    Next
                                Case "ACCR_ElencoRisorseEscluse"
                                    Dim dttRowParametri As DataRow
                                    'scorro tutti i parametri in ingresso per poter trovare il parametro necessario per il caricamento dei valori
                                    For Each dttRowParametri In Parametri.Rows
                                        'se il parametro nel datatable è lo stesso che mi occorre vado a prendere il relativo valore
                                        'e lo passo alla routine che mi restituisce i valori multipli in una stringa
                                        If UCase("IdEnte") = UCase(dttRowParametri.Item("PARAMETRO")) Then
                                            'faccio la replace del valore con la lista
                                            xLinea = Replace(xLinea, dttRow.Item("NomeTag"), FixText(ACCR_ElencoRisorseEscluse(dttRowParametri.Item("VALORE"))))
                                        End If
                                    Next
                                Case "PROG_ProgettiNegativiConMotivazioni"
                                    Dim intIdEnte As Integer
                                    Dim intIdBAndo As Integer
                                    Dim dttRowParametri As DataRow
                                    'scorro tutti i parametri in ingresso per poter trovare il parametro necessario per il caricamento dei valori
                                    For Each dttRowParametri In Parametri.Rows
                                        'se il parametro nel datatable è lo stesso che mi occorre vado a prendere il relativo valore
                                        'e lo passo alla routine che mi restituisce i valori multipli in una stringa
                                        If UCase("IdEnte") = UCase(dttRowParametri.Item("PARAMETRO")) Then
                                            'faccio la replace del valore con la lista
                                            intIdEnte = dttRowParametri.Item("VALORE")
                                        End If
                                        If UCase("IdBando") = UCase(dttRowParametri.Item("PARAMETRO")) Then
                                            'faccio la replace del valore con la lista
                                            intIdBAndo = dttRowParametri.Item("VALORE")
                                        End If
                                    Next
                                    xLinea = Replace(xLinea, dttRow.Item("NomeTag"), FixText(PROG_ProgettiNegativiConMotivazioni(intIdEnte, intIdBAndo)))
                                Case "PROG_ElencoProgettiPositivi"
                                    Dim intIdEnte As Integer
                                    Dim intIdBAndo As Integer
                                    Dim dttRowParametri As DataRow
                                    'scorro tutti i parametri in ingresso per poter trovare il parametro necessario per il caricamento dei valori
                                    For Each dttRowParametri In Parametri.Rows
                                        'se il parametro nel datatable è lo stesso che mi occorre vado a prendere il relativo valore
                                        'e lo passo alla routine che mi restituisce i valori multipli in una stringa
                                        If UCase("IdEnte") = UCase(dttRowParametri.Item("PARAMETRO")) Then
                                            'faccio la replace del valore con la lista
                                            intIdEnte = dttRowParametri.Item("VALORE")
                                        End If
                                        If UCase("IdBando") = UCase(dttRowParametri.Item("PARAMETRO")) Then
                                            'faccio la replace del valore con la lista
                                            intIdBAndo = dttRowParametri.Item("VALORE")
                                        End If
                                    Next
                                    xLinea = Replace(xLinea, dttRow.Item("NomeTag"), FixText(PROG_ElencoProgettiPositivi(intIdEnte, intIdBAndo)))
                                Case "PROG_ElencoProgettiPositiviLimitati"
                                    Dim intIdEnte As Integer
                                    Dim intIdBAndo As Integer
                                    Dim dttRowParametri As DataRow
                                    'scorro tutti i parametri in ingresso per poter trovare il parametro necessario per il caricamento dei valori
                                    For Each dttRowParametri In Parametri.Rows
                                        'se il parametro nel datatable è lo stesso che mi occorre vado a prendere il relativo valore
                                        'e lo passo alla routine che mi restituisce i valori multipli in una stringa
                                        If UCase("IdEnte") = UCase(dttRowParametri.Item("PARAMETRO")) Then
                                            'faccio la replace del valore con la lista
                                            intIdEnte = dttRowParametri.Item("VALORE")
                                        End If
                                        If UCase("IdBando") = UCase(dttRowParametri.Item("PARAMETRO")) Then
                                            'faccio la replace del valore con la lista
                                            intIdBAndo = dttRowParametri.Item("VALORE")
                                        End If
                                    Next
                                    xLinea = Replace(xLinea, dttRow.Item("NomeTag"), FixText(PROG_ElencoProgettiPositiviLimitati(intIdEnte, intIdBAndo)))
                                Case "PROG_ElencoProgettiPositiviLimitatiDaProgrammaPositivo"
                                    Dim intIdEnte As Integer
                                    Dim intIdBAndo As Integer
                                    Dim dttRowParametri As DataRow
                                    'scorro tutti i parametri in ingresso per poter trovare il parametro necessario per il caricamento dei valori
                                    For Each dttRowParametri In Parametri.Rows
                                        'se il parametro nel datatable è lo stesso che mi occorre vado a prendere il relativo valore
                                        'e lo passo alla routine che mi restituisce i valori multipli in una stringa
                                        If UCase("IdEnte") = UCase(dttRowParametri.Item("PARAMETRO")) Then
                                            'faccio la replace del valore con la lista
                                            intIdEnte = dttRowParametri.Item("VALORE")
                                        End If
                                        If UCase("IdBando") = UCase(dttRowParametri.Item("PARAMETRO")) Then
                                            'faccio la replace del valore con la lista
                                            intIdBAndo = dttRowParametri.Item("VALORE")
                                        End If
                                    Next
                                    xLinea = Replace(xLinea, dttRow.Item("NomeTag"), FixText(PROG_ElencoProgettiPositiviLimitatiDaProgrammaPositivo(intIdEnte, intIdBAndo)))
                                Case "PROG_ElencoProgettiPositiviLimitatiDaProgrammaRidotto"
                                    Dim intIdEnte As Integer
                                    Dim intIdBAndo As Integer
                                    Dim dttRowParametri As DataRow
                                    'scorro tutti i parametri in ingresso per poter trovare il parametro necessario per il caricamento dei valori
                                    For Each dttRowParametri In Parametri.Rows
                                        'se il parametro nel datatable è lo stesso che mi occorre vado a prendere il relativo valore
                                        'e lo passo alla routine che mi restituisce i valori multipli in una stringa
                                        If UCase("IdEnte") = UCase(dttRowParametri.Item("PARAMETRO")) Then
                                            'faccio la replace del valore con la lista
                                            intIdEnte = dttRowParametri.Item("VALORE")
                                        End If
                                        If UCase("IdBando") = UCase(dttRowParametri.Item("PARAMETRO")) Then
                                            'faccio la replace del valore con la lista
                                            intIdBAndo = dttRowParametri.Item("VALORE")
                                        End If
                                    Next
                                    xLinea = Replace(xLinea, dttRow.Item("NomeTag"), FixText(PROG_ElencoProgettiPositiviLimitatiDaProgrammaRidotto(intIdEnte, intIdBAndo)))
                                Case "PROG_ElencoProgrammiEsclusi"
                                    Dim intIdEnte As Integer
                                    Dim intIdBAndo As Integer
                                    Dim dttRowParametri As DataRow
                                    'scorro tutti i parametri in ingresso per poter trovare il parametro necessario per il caricamento dei valori
                                    For Each dttRowParametri In Parametri.Rows
                                        'se il parametro nel datatable è lo stesso che mi occorre vado a prendere il relativo valore
                                        'e lo passo alla routine che mi restituisce i valori multipli in una stringa
                                        If UCase("IdEnte") = UCase(dttRowParametri.Item("PARAMETRO")) Then
                                            'faccio la replace del valore con la lista
                                            intIdEnte = dttRowParametri.Item("VALORE")
                                        End If
                                        If UCase("IdBando") = UCase(dttRowParametri.Item("PARAMETRO")) Then
                                            'faccio la replace del valore con la lista
                                            intIdBAndo = dttRowParametri.Item("VALORE")
                                        End If
                                    Next
                                    xLinea = Replace(xLinea, dttRow.Item("NomeTag"), FixText(PROG_ElencoProgrammiEsclusi(intIdEnte, intIdBAndo)))
                                Case "PROG_ElencoProgrammiInammissibili"
                                    Dim intIdEnte As Integer
                                    Dim intIdBAndo As Integer
                                    Dim dttRowParametri As DataRow
                                    'scorro tutti i parametri in ingresso per poter trovare il parametro necessario per il caricamento dei valori
                                    For Each dttRowParametri In Parametri.Rows
                                        'se il parametro nel datatable è lo stesso che mi occorre vado a prendere il relativo valore
                                        'e lo passo alla routine che mi restituisce i valori multipli in una stringa
                                        If UCase("IdEnte") = UCase(dttRowParametri.Item("PARAMETRO")) Then
                                            'faccio la replace del valore con la lista
                                            intIdEnte = dttRowParametri.Item("VALORE")
                                        End If
                                        If UCase("IdBando") = UCase(dttRowParametri.Item("PARAMETRO")) Then
                                            'faccio la replace del valore con la lista
                                            intIdBAndo = dttRowParametri.Item("VALORE")
                                        End If
                                    Next
                                    xLinea = Replace(xLinea, dttRow.Item("NomeTag"), FixText(PROG_ElencoProgrammiInammissibili(intIdEnte, intIdBAndo)))

                                Case "PROG_ElencoProgrammiPositiviconProgettiLimitati"
                                    Dim intIdEnte As Integer
                                    Dim intIdBAndo As Integer
                                    Dim dttRowParametri As DataRow
                                    'scorro tutti i parametri in ingresso per poter trovare il parametro necessario per il caricamento dei valori
                                    For Each dttRowParametri In Parametri.Rows
                                        'se il parametro nel datatable è lo stesso che mi occorre vado a prendere il relativo valore
                                        'e lo passo alla routine che mi restituisce i valori multipli in una stringa
                                        If UCase("IdEnte") = UCase(dttRowParametri.Item("PARAMETRO")) Then
                                            'faccio la replace del valore con la lista
                                            intIdEnte = dttRowParametri.Item("VALORE")
                                        End If
                                        If UCase("IdBando") = UCase(dttRowParametri.Item("PARAMETRO")) Then
                                            'faccio la replace del valore con la lista
                                            intIdBAndo = dttRowParametri.Item("VALORE")
                                        End If
                                    Next
                                    xLinea = Replace(xLinea, dttRow.Item("NomeTag"), FixText(PROG_ElencoProgrammiPositiviconProgettiLimitati(intIdEnte, intIdBAndo)))
                                Case "PROG_ElencoProgrammiRidotti"
                                    Dim intIdEnte As Integer
                                    Dim intIdBAndo As Integer
                                    Dim dttRowParametri As DataRow
                                    'scorro tutti i parametri in ingresso per poter trovare il parametro necessario per il caricamento dei valori
                                    For Each dttRowParametri In Parametri.Rows
                                        'se il parametro nel datatable è lo stesso che mi occorre vado a prendere il relativo valore
                                        'e lo passo alla routine che mi restituisce i valori multipli in una stringa
                                        If UCase("IdEnte") = UCase(dttRowParametri.Item("PARAMETRO")) Then
                                            'faccio la replace del valore con la lista
                                            intIdEnte = dttRowParametri.Item("VALORE")
                                        End If
                                        If UCase("IdBando") = UCase(dttRowParametri.Item("PARAMETRO")) Then
                                            'faccio la replace del valore con la lista
                                            intIdBAndo = dttRowParametri.Item("VALORE")
                                        End If
                                    Next
                                    xLinea = Replace(xLinea, dttRow.Item("NomeTag"), FixText(PROG_ElencoProgrammiRidotti(intIdEnte, intIdBAndo)))
                                Case "PROG_SostituzioniApprovate"
                                    Dim intIdEnte As Integer
                                    Dim intIdIstanzaSostituzioneOLP As Integer
                                    Dim dttRowParametri As DataRow
                                    'scorro tutti i parametri in ingresso per poter trovare il parametro necessario per il caricamento dei valori
                                    For Each dttRowParametri In Parametri.Rows
                                        'se il parametro nel datatable è lo stesso che mi occorre vado a prendere il relativo valore
                                        'e lo passo alla routine che mi restituisce i valori multipli in una stringa
                                        If UCase("IdEnte") = UCase(dttRowParametri.Item("PARAMETRO")) Then
                                            'faccio la replace del valore con la lista
                                            intIdEnte = dttRowParametri.Item("VALORE")
                                        End If
                                        If UCase("IdIstanzaSostituzioneOLP") = UCase(dttRowParametri.Item("PARAMETRO")) Then
                                            'faccio la replace del valore con la lista
                                            intIdIstanzaSostituzioneOLP = dttRowParametri.Item("VALORE")
                                        End If
                                    Next
                                    xLinea = Replace(xLinea, dttRow.Item("NomeTag"), FixText(PROG_SostituzioniApprovate(intIdEnte, intIdIstanzaSostituzioneOLP)))
                                Case "PROG_SostituzioniRespinte"
                                    Dim intIdEnte As Integer
                                    Dim intIdIstanzaSostituzioneOLP As Integer
                                    Dim dttRowParametri As DataRow
                                    'scorro tutti i parametri in ingresso per poter trovare il parametro necessario per il caricamento dei valori
                                    For Each dttRowParametri In Parametri.Rows
                                        'se il parametro nel datatable è lo stesso che mi occorre vado a prendere il relativo valore
                                        'e lo passo alla routine che mi restituisce i valori multipli in una stringa
                                        If UCase("IdEnte") = UCase(dttRowParametri.Item("PARAMETRO")) Then
                                            'faccio la replace del valore con la lista
                                            intIdEnte = dttRowParametri.Item("VALORE")
                                        End If
                                        If UCase("IdIstanzaSostituzioneOLP") = UCase(dttRowParametri.Item("PARAMETRO")) Then
                                            'faccio la replace del valore con la lista
                                            intIdIstanzaSostituzioneOLP = dttRowParametri.Item("VALORE")
                                        End If
                                    Next
                                    xLinea = Replace(xLinea, dttRow.Item("NomeTag"), FixText(PROG_SostituzioniRespinte(intIdEnte, intIdIstanzaSostituzioneOLP)))

                                Case "VOL_ElencoVolontariEsclusi"
                                    Dim intIdEnte As Integer
                                    Dim intIdBAndo As Integer
                                    Dim dataavvioData As String
                                    Dim dttRowParametri As DataRow
                                    'scorro tutti i parametri in ingresso per poter trovare il parametro necessario per il caricamento dei valori
                                    For Each dttRowParametri In Parametri.Rows
                                        'se il parametro nel datatable è lo stesso che mi occorre vado a prendere il relativo valore
                                        'e lo passo alla routine che mi restituisce i valori multipli in una stringa
                                        If UCase("IdEnte") = UCase(dttRowParametri.Item("PARAMETRO")) Then
                                            'faccio la replace del valore con la lista
                                            intIdEnte = dttRowParametri.Item("VALORE")
                                        End If
                                        If UCase("IdBando") = UCase(dttRowParametri.Item("PARAMETRO")) Then
                                            'faccio la replace del valore con la lista
                                            intIdBAndo = dttRowParametri.Item("VALORE")
                                        End If

                                        If UCase("dataavvio") = UCase(dttRowParametri.Item("PARAMETRO")) Then
                                            'faccio la replace del valore con la lista
                                            dataavvioData = dttRowParametri.Item("VALORE")
                                        End If

                                    Next
                                    xLinea = Replace(xLinea, dttRow.Item("NomeTag"), FixText(VOL_ElencoVolontariEsclusi(intIdEnte, intIdBAndo, dataavvioData)))
                                Case "VOL_ElencoVolontariEsclusi_Programma"
                                    Dim intIdProgramma As Integer
                                    Dim dataavvioData As String
                                    Dim dttRowParametri As DataRow
                                    'scorro tutti i parametri in ingresso per poter trovare il parametro necessario per il caricamento dei valori
                                    For Each dttRowParametri In Parametri.Rows
                                        'se il parametro nel datatable è lo stesso che mi occorre vado a prendere il relativo valore
                                        'e lo passo alla routine che mi restituisce i valori multipli in una stringa
                                        If UCase("IdProgramma") = UCase(dttRowParametri.Item("PARAMETRO")) Then
                                            'faccio la replace del valore con la lista
                                            intIdProgramma = dttRowParametri.Item("VALORE")
                                        End If
                                        If UCase("dataavvio") = UCase(dttRowParametri.Item("PARAMETRO")) Then
                                            'faccio la replace del valore con la lista
                                            dataavvioData = dttRowParametri.Item("VALORE")
                                        End If

                                    Next
                                    xLinea = Replace(xLinea, dttRow.Item("NomeTag"), FixText(VOL_ElencoVolontariEsclusi_Programma(intIdProgramma, dataavvioData)))

                                Case "VOL_ElencoProgetti"
                                    Dim intIdEnte As Integer
                                    Dim intIdBAndo As Integer
                                    Dim dataavvioData As String
                                    Dim dttRowParametri As DataRow
                                    'scorro tutti i parametri in ingresso per poter trovare il parametro necessario per il caricamento dei valori
                                    For Each dttRowParametri In Parametri.Rows
                                        'se il parametro nel datatable è lo stesso che mi occorre vado a prendere il relativo valore
                                        'e lo passo alla routine che mi restituisce i valori multipli in una stringa
                                        If UCase("IdEnte") = UCase(dttRowParametri.Item("PARAMETRO")) Then
                                            'faccio la replace del valore con la lista
                                            intIdEnte = dttRowParametri.Item("VALORE")
                                        End If
                                        If UCase("IdBando") = UCase(dttRowParametri.Item("PARAMETRO")) Then
                                            'faccio la replace del valore con la lista
                                            intIdBAndo = dttRowParametri.Item("VALORE")
                                        End If
                                        If UCase("dataavvio") = UCase(dttRowParametri.Item("PARAMETRO")) Then
                                            'faccio la replace del valore con la lista
                                            dataavvioData = dttRowParametri.Item("VALORE")
                                        End If
                                    Next
                                    xLinea = Replace(xLinea, dttRow.Item("NomeTag"), FixText(VOL_ElencoProgetti(intIdEnte, intIdBAndo, dataavvioData)))
                                Case "VOL_ElencoProgetti_Programma"
                                    Dim intIdProgramma As Integer
                                    Dim dataavvioData As String
                                    Dim dttRowParametri As DataRow
                                    'scorro tutti i parametri in ingresso per poter trovare il parametro necessario per il caricamento dei valori
                                    For Each dttRowParametri In Parametri.Rows
                                        'se il parametro nel datatable è lo stesso che mi occorre vado a prendere il relativo valore
                                        'e lo passo alla routine che mi restituisce i valori multipli in una stringa
                                        If UCase("IdProgramma") = UCase(dttRowParametri.Item("PARAMETRO")) Then
                                            'faccio la replace del valore con la lista
                                            intIdProgramma = dttRowParametri.Item("VALORE")
                                        End If
                                        If UCase("dataavvio") = UCase(dttRowParametri.Item("PARAMETRO")) Then
                                            'faccio la replace del valore con la lista
                                            dataavvioData = dttRowParametri.Item("VALORE")
                                        End If
                                    Next
                                    xLinea = Replace(xLinea, dttRow.Item("NomeTag"), FixText(VOL_ElencoProgetti_Programma(intIdProgramma, dataavvioData)))

                                Case "VOL_ElencoVolontariConcessi"
                                    Dim dttRowParametri As DataRow
                                    'scorro tutti i parametri in ingresso per poter trovare il parametro necessario per il caricamento dei valori
                                    For Each dttRowParametri In Parametri.Rows
                                        'se il parametro nel datatable è lo stesso che mi occorre vado a prendere il relativo valore
                                        'e lo passo alla routine che mi restituisce i valori multipli in una stringa
                                        If UCase("IdAttivitàSedeAssegnazione") = UCase(dttRowParametri.Item("PARAMETRO")) Then
                                            'faccio la replace del valore con la lista
                                            xLinea = Replace(xLinea, dttRow.Item("NomeTag"), FixText(VOL_ElencoVolontariConcessi(dttRowParametri.Item("VALORE"))))
                                        End If
                                    Next
                                Case "VOL_DataMatrix" 'Antonello 19/11/2013 per la generazione dell'immagine del datamatrix sul documento
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
                                Case "VOL_ControllaDoppiaDomanda"
                                    Dim intIdProgetto As Integer
                                    Dim strCodiceFiscale As String
                                    Dim dttRowParametri As DataRow
                                    'scorro tutti i parametri in ingresso per poter trovare il parametro necessario per il caricamento dei valori
                                    For Each dttRowParametri In Parametri.Rows
                                        'se il parametro nel datatable è lo stesso che mi occorre vado a prendere il relativo valore
                                        'e lo passo alla routine che mi restituisce i valori multipli in una stringa
                                        If UCase("IdProgetto") = UCase(dttRowParametri.Item("PARAMETRO")) Then
                                            'faccio la replace del valore con la lista
                                            intIdProgetto = dttRowParametri.Item("VALORE")
                                        End If
                                        If UCase("CODICEFISCALE") = UCase(dttRowParametri.Item("PARAMETRO")) Then
                                            'faccio la replace del valore con la lista
                                            strCodiceFiscale = dttRowParametri.Item("VALORE")
                                        End If
                                    Next
                                    xLinea = Replace(xLinea, dttRow.Item("NomeTag"), FixText(VOL_ControllaDoppiaDomanda(intIdProgetto, strCodiceFiscale)))
                                Case "MON_ElencoSediVerifica"
                                    Dim dttRowParametri As DataRow
                                    'scorro tutti i parametri in ingresso per poter trovare il parametro necessario per il caricamento dei valori
                                    For Each dttRowParametri In Parametri.Rows
                                        'se il parametro nel datatable è lo stesso che mi occorre vado a prendere il relativo valore
                                        'e lo passo alla routine che mi restituisce i valori multipli in una stringa
                                        If UCase("IdVerifica") = UCase(dttRowParametri.Item("PARAMETRO")) Then
                                            'faccio la replace del valore con la lista
                                            xLinea = Replace(xLinea, dttRow.Item("NomeTag"), FixText(MON_ElencoSediVerifica(dttRowParametri.Item("VALORE"))))
                                        End If
                                    Next
                                Case "MON_ElencoFigliVerifica"
                                    Dim dttRowParametri As DataRow
                                    'scorro tutti i parametri in ingresso per poter trovare il parametro necessario per il caricamento dei valori
                                    For Each dttRowParametri In Parametri.Rows
                                        'se il parametro nel datatable è lo stesso che mi occorre vado a prendere il relativo valore
                                        'e lo passo alla routine che mi restituisce i valori multipli in una stringa
                                        If UCase("IdVerifica") = UCase(dttRowParametri.Item("PARAMETRO")) Then
                                            'faccio la replace del valore con la lista
                                            xLinea = Replace(xLinea, dttRow.Item("NomeTag"), FixText(MON_ElencoFigliVerifica(dttRowParametri.Item("VALORE"))))
                                        End If
                                    Next
                                Case "MON_ElencoRequisiti"
                                    Dim dttRowParametri As DataRow
                                    'scorro tutti i parametri in ingresso per poter trovare il parametro necessario per il caricamento dei valori
                                    For Each dttRowParametri In Parametri.Rows
                                        'se il parametro nel datatable è lo stesso che mi occorre vado a prendere il relativo valore
                                        'e lo passo alla routine che mi restituisce i valori multipli in una stringa
                                        If UCase("IdVerificaAssociata") = UCase(dttRowParametri.Item("PARAMETRO")) Then
                                            'faccio la replace del valore con la lista
                                            xLinea = Replace(xLinea, dttRow.Item("NomeTag"), FixText(MON_ElencoRequisiti(dttRowParametri.Item("VALORE"))))
                                        End If
                                    Next
                                Case "MON_ElencoVerificheMultiple"
                                    Dim dttRowParametri As DataRow
                                    'scorro tutti i parametri in ingresso per poter trovare il parametro necessario per il caricamento dei valori
                                    For Each dttRowParametri In Parametri.Rows
                                        'se il parametro nel datatable è lo stesso che mi occorre vado a prendere il relativo valore
                                        'e lo passo alla routine che mi restituisce i valori multipli in una stringa
                                        If UCase("IdGruppoStampa") = UCase(dttRowParametri.Item("PARAMETRO")) Then
                                            'faccio la replace del valore con la lista
                                            xLinea = Replace(xLinea, dttRow.Item("NomeTag"), FixText(MON_ElencoVerificheMultiple(dttRowParametri.Item("VALORE"))))
                                        End If
                                    Next
                            End Select
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
                If codsede = "ERRORE" Then
                    codsede = 0
                End If
                xLinea = ParamentriBarraCAD(intIdModello, xLinea, Parametri, codsede)
                '***
            End If

            'scrivo la riga del template nel nuovo file
            Writer.WriteLine(xLinea)

            'chiudo lo streaming in scrittura
            Writer.Close()
            Writer = Nothing

            'chiudo lo streaming in scrittura
            Reader.Close()
            Reader = Nothing

            EseguiGenerazioneModello = strPercorsoFileGenerato

        End If

        Return EseguiGenerazioneModello

    End Function

    Function FixText(ByVal myText As String) As String

        myText = myText.Replace(vbCrLf, "\par" & vbCrLf)

        myText = myText.Replace("  ", " ")

        myText = myText.Replace(vbTab, "    ")

        Return myText

    End Function

    Function NumeroSediEnte(ByVal strIdEnte As Integer) As String
        Dim strsql As String
        Dim strNumeroSedi As String
        Dim dtrLeggiDati As SqlClient.SqlDataReader

        strsql = "select "
        strsql = strsql & "count(*) as NSedi "
        strsql = strsql & "FROM VW_ELENCO_SEDI "
        strsql = strsql & "WHERE VW_ELENCO_SEDI.identefiglio = " & strIdEnte

        'eseguo la query e passo il risultato al datareader



        dtrLeggiDati = ClsServer.CreaDatareader(strsql, HttpContext.Current.Session("conn"))


        'ciclo il risultato della query e costruisco la stringa con i valori multipli
        While dtrLeggiDati.Read
            strNumeroSedi = dtrLeggiDati("nsedi")
        End While


        If Not dtrLeggiDati Is Nothing Then
            dtrLeggiDati.Close()
            dtrLeggiDati = Nothing
        End If

        Return strNumeroSedi

    End Function
    Function NumeroSediEnteParziale(ByVal strIdEnte As Integer, ByVal strIdEnteFase As Integer) As String
        Dim strsql As String
        Dim strNumeroSedi As String
        Dim dtrLeggiDati As SqlClient.SqlDataReader

        strsql = "select "
        strsql = strsql & "count(*) as NSedi "
        strsql = strsql & "FROM VW_ELENCO_SEDI_PARZIALE "
        strsql = strsql & "WHERE identefiglio = " & strIdEnte & " AND identefase= " & strIdEnteFase

        'eseguo la query e passo il risultato al datareader



        dtrLeggiDati = ClsServer.CreaDatareader(strsql, HttpContext.Current.Session("conn"))


        'ciclo il risultato della query e costruisco la stringa con i valori multipli
        While dtrLeggiDati.Read
            strNumeroSedi = dtrLeggiDati("nsedi")
        End While


        If Not dtrLeggiDati Is Nothing Then
            dtrLeggiDati.Close()
            dtrLeggiDati = Nothing
        End If

        Return strNumeroSedi

    End Function
    Function ElencoSettoriEnte(ByVal strIdEnte As Integer) As String
        Dim strsql As String
        Dim strSettori As String
        Dim dtrLeggiDati As SqlClient.SqlDataReader
        Dim conta As Integer = 0

        strsql = "select "
        strsql = strsql & "isnull(b.macroambitoattività,'Assenti') as settori "
        strsql = strsql & "FROM enti "
        strsql = strsql & "left JOIN entisettori as a ON enti.idente=a.idente "
        strsql = strsql & "left JOIN macroambitiattività b ON a.idmacroambitoattività=b.idmacroambitoattività "
        strsql = strsql & "WHERE enti.idente = " & strIdEnte

        'eseguo la query e passo il risultato al datareader
        dtrLeggiDati = ClsServer.CreaDatareader(strsql, HttpContext.Current.Session("conn"))

        strSettori = ""

        'ciclo il risultato della query e costruisco la stringa con i valori multipli
        While dtrLeggiDati.Read
            If conta <> 0 Then
                strSettori = strSettori & ", "
            End If
            strSettori = strSettori & "" & dtrLeggiDati("settori")
            conta = conta + 1
        End While


        If Not dtrLeggiDati Is Nothing Then
            dtrLeggiDati.Close()
            dtrLeggiDati = Nothing
        End If

        Return strSettori

    End Function

    Function TrovaTipoModello(ByVal IDBando As Integer) As String
        Dim strsql As String
        Dim dtrLeggiDati As SqlClient.SqlDataReader

        If Not dtrLeggiDati Is Nothing Then
            dtrLeggiDati.Close()
            dtrLeggiDati = Nothing
        End If

        strsql = " SELECT A.* from AssociaBandoTipiProgetto a"
        strsql = strsql & " INNER JOIN TipiProgetto t on a.IdTipoProgetto=t.IdTipoProgetto"
        strsql = strsql & " INNER JOIN AssociaBandoRegioniCompetenze b on a.IdBando=b.IdBando and b.IdRegioneCompetenza=22" 'solo dipartimento modelli differenziati
        strsql = strsql & " WHERE a.IdBando=" & IDBando & " and t.Scheda='SCU' "

        'eseguo la query e passo il risultato al datareader
        dtrLeggiDati = ClsServer.CreaDatareader(strsql, HttpContext.Current.Session("conn"))

        If dtrLeggiDati.HasRows = True Then
            TrovaTipoModello = "SCU"
        Else
            TrovaTipoModello = "SCN"
        End If

        If Not dtrLeggiDati Is Nothing Then
            dtrLeggiDati.Close()
            dtrLeggiDati = Nothing
        End If

        Return TrovaTipoModello

    End Function






    'funzione che mi riporta la stringa completa delle sedi da sostituire nel template
    Function ACCR_ElencoSediAccreditate(ByVal IdEnte As Integer) As String
        Dim strsql As String
        Dim strValoriMultipli As String = ""
        Dim settore As String
        Dim Mydataset As New DataSet


        strsql = "select isnull(upper([Nome Sede Fisica]), '') as Nome, "
        strsql = strsql & "isnull(upper([Indirizzo]), '') as Indirizzo, "
        strsql = strsql & "isnull(upper([Civico]), '') as Civico, "
        strsql = strsql & "isnull(upper([CAP]), '') as CAP, "
        strsql = strsql & "isnull(upper([Comune]), '') as Comune, "
        strsql = strsql & "isnull(upper([Provincia]), '') as Provincia, "
        strsql = strsql & "isnull(upper([Codice Sede Attuazione]), '') as Codice, "
        strsql = strsql & "isnull(upper([Palazzina]), '') as Palazzina, "
        strsql = strsql & "isnull(upper([Scala]), '') as Scala, "
        strsql = strsql & "isnull(upper([Piano]), '') as Piano, "
        strsql = strsql & "isnull(upper([Interno]), '') as Interno, "
        strsql = strsql & "isnull(upper([IdEnteFiglio]), '') as IdEnte, "
        strsql = strsql & "isnull(upper([Nome Ente Figlio]), '') as NomeEnte, "
        strsql = strsql & "isnull(upper([Codice Ente Figlio]), '') as CodiceEnteFiglio, "
        strsql = strsql & "isnull(upper([nmaxvolontari]), '') as NMaxVolontari, "
        strsql = strsql & "isnull(upper([TitoloGiuridico]), '') as TitoloGiuridico, "
        strsql = strsql & "isnull(upper([TipoRelazione]), '') as TipoRelazione "
        strsql = strsql & "FROM VW_ELENCO_SEDI "
        strsql = strsql & "WHERE ([idente] = " & IdEnte & ") "
        strsql = strsql & "order by [Codice Ente Figlio], [Nome Sede Fisica], [Indirizzo], [Civico], [CAP], [Comune], "
        strsql = strsql & "[Provincia], [Codice Sede Attuazione],[Palazzina],[Scala],[Piano],"
        strsql = strsql & "[Interno],[IdEnteFiglio],[Nome Ente Figlio]"


        Mydataset = ClsServer.DataSetGenerico(strsql, HttpContext.Current.Session("conn"))

        'variabile a cui passo il nome dell'ente per poter controllare se l'ho già stampato o meno
        Dim strCodiceEnte As String
        Dim contatore As Integer
        strCodiceEnte = ""
        '\ul\b\i\f0\fs20 PROVA\ulnone\b0\i0

        For contatore = 0 To Mydataset.Tables(0).Rows.Count - 1

            'verifico se necessaria sezione ente
            If Mydataset.Tables(0).Rows.Item(contatore).Item("CodiceEnteFiglio") <> strCodiceEnte Then
                'intestazione 
                '  strValoriMultipli = strValoriMultipli & " \ul\b\i " & Mydataset.Tables(0).Rows.Item(contatore).Item("NomeEnte") & " [" & Mydataset.Tables(0).Rows.Item(contatore).Item("CodiceEnteFiglio") & "]" & " \par \b0\ulnone\i0 "

                If Mydataset.Tables(0).Rows.Item(contatore).Item("Tiporelazione") = "" Then
                    'intestazione 
                    strValoriMultipli = strValoriMultipli & " \ul\b\fs22 " & Mydataset.Tables(0).Rows.Item(contatore).Item("NomeEnte") & " [" & Mydataset.Tables(0).Rows.Item(contatore).Item("CodiceEnteFiglio") & "]" & " \par \b0\ulnone "
                    strValoriMultipli = strValoriMultipli & " \ul\b\fs22 SETTORI: " & ElencoSettoriEnte(Mydataset.Tables(0).Rows.Item(contatore).Item("IdEnte")) & " - NUMERO SEDI: " & NumeroSediEnte(Mydataset.Tables(0).Rows.Item(contatore).Item("idente")) & " \b0\ulnone\fs18 \par " & vbCrLf
                Else
                    'intestazione 
                    strValoriMultipli = strValoriMultipli & " \ul\b\i\fs22 " & Mydataset.Tables(0).Rows.Item(contatore).Item("NomeEnte") & " [" & Mydataset.Tables(0).Rows.Item(contatore).Item("CodiceEnteFiglio") & "]" & " \par \b0\ulnone\i0 "
                    strValoriMultipli = strValoriMultipli & " \ul\b\i\fs22 SETTORI: " & ElencoSettoriEnte(Mydataset.Tables(0).Rows.Item(contatore).Item("IdEnte")) & " - TIPO ACCORDO: " & Mydataset.Tables(0).Rows.Item(contatore).Item("Tiporelazione") & " - NUMERO SEDI: " & NumeroSediEnte(Mydataset.Tables(0).Rows.Item(contatore).Item("idente")) & " \b0\ulnone\i0\fs18 \par " & vbCrLf
                End If
                'SETTORI 

                'aggiorno l'ultimo ente lavrato
                strCodiceEnte = Mydataset.Tables(0).Rows.Item(contatore).Item("CodiceEnteFiglio")

            End If


            'Costruisco la stringa con palazzina,scala,piano,interno 
            Dim StrDettIndirizzo As String = "("

            If Mydataset.Tables(0).Rows.Item(contatore).Item("Palazzina") <> "" Then
                StrDettIndirizzo = StrDettIndirizzo & "PALAZZINA: " & Mydataset.Tables(0).Rows.Item(contatore).Item("Palazzina")
            End If

            'MODIFICA DEL 16/08/2021 
            'RIMOSSI DALLA VISUALIZZAZIONE SCALA - PIANO - INTERNO SU RICHIESTA DEL DIPARTIMENTO DEL 13/08/2021.

            'If Mydataset.Tables(0).Rows.Item(contatore).Item("Scala") <> "" Then
            '    If StrDettIndirizzo = "(" Then
            '        StrDettIndirizzo = StrDettIndirizzo & "SCALA: " & Mydataset.Tables(0).Rows.Item(contatore).Item("Scala")
            '    Else
            '        StrDettIndirizzo = StrDettIndirizzo & "," & " SCALA: " & Mydataset.Tables(0).Rows.Item(contatore).Item("Scala")
            '    End If
            'End If

            'If Mydataset.Tables(0).Rows.Item(contatore).Item("Piano") <> "" Then
            '    If StrDettIndirizzo = "(" Then
            '        StrDettIndirizzo = StrDettIndirizzo & "PIANO: " & Mydataset.Tables(0).Rows.Item(contatore).Item("Piano")
            '    Else
            '        StrDettIndirizzo = StrDettIndirizzo & "," & " PIANO: " & Mydataset.Tables(0).Rows.Item(contatore).Item("Piano")
            '    End If
            'End If

            'If Mydataset.Tables(0).Rows.Item(contatore).Item("Interno") <> "" Then
            '    If StrDettIndirizzo = "(" Then
            '        StrDettIndirizzo = StrDettIndirizzo & "INTERNO: " & Mydataset.Tables(0).Rows.Item(contatore).Item("Interno") & ")"
            '    Else
            '        StrDettIndirizzo = StrDettIndirizzo & "," & " INTERNO: " & Mydataset.Tables(0).Rows.Item(contatore).Item("Interno") & ")"
            '    End If
            'Else
            '    StrDettIndirizzo = StrDettIndirizzo & ")"
            'End If

            StrDettIndirizzo = StrDettIndirizzo & ")"

            If StrDettIndirizzo = "()" Then
                StrDettIndirizzo = " "
            End If

            strValoriMultipli = strValoriMultipli & "\b " & Mydataset.Tables(0).Rows.Item(contatore).Item("Nome") & " \b0 - " & Mydataset.Tables(0).Rows.Item(contatore).Item("Indirizzo") & " " & Mydataset.Tables(0).Rows.Item(contatore).Item("Civico") & " " & StrDettIndirizzo & " " & Mydataset.Tables(0).Rows.Item(contatore).Item("CAP") & " " & Mydataset.Tables(0).Rows.Item(contatore).Item("Comune") & " (" & Mydataset.Tables(0).Rows.Item(contatore).Item("Provincia") & ") - NUMERO MASSIMO VOLONTARI ALLOCABILI: " & Mydataset.Tables(0).Rows.Item(contatore).Item("NMaxVolontari") & " - TITOLO GIURIDICO DI POSSEDIMENTO: " & Mydataset.Tables(0).Rows.Item(contatore).Item("TitoloGiuridico") & "  - CODICE SEDE: " & " \b " & Mydataset.Tables(0).Rows.Item(contatore).Item("Codice") & " \b0 \par" & vbCrLf

        Next

        'passo la stringa con i valori multipli alla funzione per il valore di ritorno
        ACCR_ElencoSediAccreditate = strValoriMultipli

        'ritorno la stringa
        Return ACCR_ElencoSediAccreditate

    End Function

    Function ACCR_ElencoSediAccreditateParziale(ByVal IdEnteFase As Integer) As String
        Dim strsql As String
        Dim strValoriMultipli As String = ""
        Dim settore As String
        Dim Mydataset As New DataSet


        strsql = "select isnull(upper([Nome Sede Fisica]), '') as Nome, "
        strsql = strsql & "isnull(upper([Indirizzo]), '') as Indirizzo, "
        strsql = strsql & "isnull(upper([Civico]), '') as Civico, "
        strsql = strsql & "isnull(upper([CAP]), '') as CAP, "
        strsql = strsql & "isnull(upper([Comune]), '') as Comune, "
        strsql = strsql & "isnull(upper([Provincia]), '') as Provincia, "
        strsql = strsql & "isnull(upper([Codice Sede Attuazione]), '') as Codice, "
        strsql = strsql & "isnull(upper([Palazzina]), '') as Palazzina, "
        strsql = strsql & "isnull(upper([Scala]), '') as Scala, "
        strsql = strsql & "isnull(upper([Piano]), '') as Piano, "
        strsql = strsql & "isnull(upper([Interno]), '') as Interno, "
        strsql = strsql & "isnull(upper([IdEnteFiglio]), '') as IdEnte, "
        strsql = strsql & "isnull(upper([Nome Ente Figlio]), '') as NomeEnte, "
        strsql = strsql & "isnull(upper([Codice Ente Figlio]), '') as CodiceEnteFiglio, "
        strsql = strsql & "isnull(upper([nmaxvolontari]), '') as NMaxVolontari, "
        strsql = strsql & "isnull(upper([TitoloGiuridico]), '') as TitoloGiuridico, "
        strsql = strsql & "isnull(upper([TipoRelazione]), '') as TipoRelazione "
        strsql = strsql & "FROM VW_ELENCO_SEDI_PARZIALE "
        strsql = strsql & "WHERE ([identeFASE] = " & IdEnteFase & ") "
        strsql = strsql & "order by [Codice Ente Figlio], [Nome Sede Fisica], [Indirizzo], [Civico], [CAP], [Comune], "
        strsql = strsql & "[Provincia], [Codice Sede Attuazione],[Palazzina],[Scala],[Piano],"
        strsql = strsql & "[Interno],[IdEnteFiglio],[Nome Ente Figlio]"


        Mydataset = ClsServer.DataSetGenerico(strsql, HttpContext.Current.Session("conn"))

        'variabile a cui passo il nome dell'ente per poter controllare se l'ho già stampato o meno
        Dim strCodiceEnte As String
        Dim contatore As Integer
        strCodiceEnte = ""
        '\ul\b\i\f0\fs20 PROVA\ulnone\b0\i0

        For contatore = 0 To Mydataset.Tables(0).Rows.Count - 1

            'verifico se necessaria sezione ente
            If Mydataset.Tables(0).Rows.Item(contatore).Item("CodiceEnteFiglio") <> strCodiceEnte Then
                'intestazione 
                '  strValoriMultipli = strValoriMultipli & " \ul\b\i " & Mydataset.Tables(0).Rows.Item(contatore).Item("NomeEnte") & " [" & Mydataset.Tables(0).Rows.Item(contatore).Item("CodiceEnteFiglio") & "]" & " \par \b0\ulnone\i0 "

                If Mydataset.Tables(0).Rows.Item(contatore).Item("Tiporelazione") = "" Then
                    'intestazione 
                    strValoriMultipli = strValoriMultipli & " \ul\b\fs22 " & Mydataset.Tables(0).Rows.Item(contatore).Item("NomeEnte") & " [" & Mydataset.Tables(0).Rows.Item(contatore).Item("CodiceEnteFiglio") & "]" & " \par \b0\ulnone "
                    strValoriMultipli = strValoriMultipli & " \ul\b\fs22 SETTORI: " & ElencoSettoriEnte(Mydataset.Tables(0).Rows.Item(contatore).Item("IdEnte")) & " - NUMERO SEDI: " & NumeroSediEnteParziale(Mydataset.Tables(0).Rows.Item(contatore).Item("idente"), IdEnteFase) & " \b0\ulnone\fs18 \par " & vbCrLf
                Else
                    'intestazione 
                    strValoriMultipli = strValoriMultipli & " \ul\b\i\fs22 " & Mydataset.Tables(0).Rows.Item(contatore).Item("NomeEnte") & " [" & Mydataset.Tables(0).Rows.Item(contatore).Item("CodiceEnteFiglio") & "]" & " \par \b0\ulnone\i0 "
                    strValoriMultipli = strValoriMultipli & " \ul\b\i\fs22 SETTORI: " & ElencoSettoriEnte(Mydataset.Tables(0).Rows.Item(contatore).Item("IdEnte")) & " - TIPO ACCORDO: " & Mydataset.Tables(0).Rows.Item(contatore).Item("Tiporelazione") & " - NUMERO SEDI: " & NumeroSediEnteParziale(Mydataset.Tables(0).Rows.Item(contatore).Item("idente"), IdEnteFase) & " \b0\ulnone\i0\fs18 \par " & vbCrLf
                End If
                'SETTORI 

                'aggiorno l'ultimo ente lavrato
                strCodiceEnte = Mydataset.Tables(0).Rows.Item(contatore).Item("CodiceEnteFiglio")

            End If


            'Costruisco la stringa con palazzina,scala,piano,interno 
            Dim StrDettIndirizzo As String = "("

            If Mydataset.Tables(0).Rows.Item(contatore).Item("Palazzina") <> "" Then
                StrDettIndirizzo = StrDettIndirizzo & "PALAZZINA: " & Mydataset.Tables(0).Rows.Item(contatore).Item("Palazzina")
            End If

            'MODIFICA DEL 16/08/2021 
            'RIMOSSI DALLA VISUALIZZAZIONE SCALA - PIANO - INTERNO SU RICHIESTA DEL DIPARTIMENTO DEL 13/08/2021.

            'If Mydataset.Tables(0).Rows.Item(contatore).Item("Scala") <> "" Then
            '    If StrDettIndirizzo = "(" Then
            '        StrDettIndirizzo = StrDettIndirizzo & "SCALA: " & Mydataset.Tables(0).Rows.Item(contatore).Item("Scala")
            '    Else
            '        StrDettIndirizzo = StrDettIndirizzo & "," & " SCALA: " & Mydataset.Tables(0).Rows.Item(contatore).Item("Scala")
            '    End If
            'End If

            'If Mydataset.Tables(0).Rows.Item(contatore).Item("Piano") <> "" Then
            '    If StrDettIndirizzo = "(" Then
            '        StrDettIndirizzo = StrDettIndirizzo & "PIANO: " & Mydataset.Tables(0).Rows.Item(contatore).Item("Piano")
            '    Else
            '        StrDettIndirizzo = StrDettIndirizzo & "," & " PIANO: " & Mydataset.Tables(0).Rows.Item(contatore).Item("Piano")
            '    End If
            'End If

            'If Mydataset.Tables(0).Rows.Item(contatore).Item("Interno") <> "" Then
            '    If StrDettIndirizzo = "(" Then
            '        StrDettIndirizzo = StrDettIndirizzo & "INTERNO: " & Mydataset.Tables(0).Rows.Item(contatore).Item("Interno") & ")"
            '    Else
            '        StrDettIndirizzo = StrDettIndirizzo & "," & " INTERNO: " & Mydataset.Tables(0).Rows.Item(contatore).Item("Interno") & ")"
            '    End If
            'Else
            '    StrDettIndirizzo = StrDettIndirizzo & ")"
            'End If

            StrDettIndirizzo = StrDettIndirizzo & ")"

            If StrDettIndirizzo = "()" Then
                StrDettIndirizzo = " "
            End If

            strValoriMultipli = strValoriMultipli & "\b " & Mydataset.Tables(0).Rows.Item(contatore).Item("Nome") & " \b0 - " & Mydataset.Tables(0).Rows.Item(contatore).Item("Indirizzo") & " " & Mydataset.Tables(0).Rows.Item(contatore).Item("Civico") & " " & StrDettIndirizzo & " " & Mydataset.Tables(0).Rows.Item(contatore).Item("CAP") & " " & Mydataset.Tables(0).Rows.Item(contatore).Item("Comune") & " (" & Mydataset.Tables(0).Rows.Item(contatore).Item("Provincia") & ") - NUMERO MASSIMO VOLONTARI ALLOCABILI: " & Mydataset.Tables(0).Rows.Item(contatore).Item("NMaxVolontari") & " - TITOLO GIURIDICO DI POSSEDIMENTO: " & Mydataset.Tables(0).Rows.Item(contatore).Item("TitoloGiuridico") & "  - CODICE SEDE: " & " \b " & Mydataset.Tables(0).Rows.Item(contatore).Item("Codice") & " \b0 \par" & vbCrLf

        Next

        'passo la stringa con i valori multipli alla funzione per il valore di ritorno
        ACCR_ElencoSediAccreditateParziale = strValoriMultipli

        'ritorno la stringa
        Return ACCR_ElencoSediAccreditateParziale

    End Function
    Function ACCR_ElencoSediAccreditate_old(ByVal IdEnte As Integer) As String
        Dim strsql As String
        Dim strValoriMultipli As String = ""
        Dim dtrLeggiDati As SqlClient.SqlDataReader

        'chiudo il datareader
        If Not dtrLeggiDati Is Nothing Then
            dtrLeggiDati.Close()
            dtrLeggiDati = Nothing
        End If

        'preparo la query
        strsql = "select isnull(upper([Nome Sede Fisica]), '') as Nome, "
        strsql = strsql & "isnull(upper([Indirizzo]), '') as Indirizzo, "
        strsql = strsql & "isnull(upper([Civico]), '') as Civico, "
        strsql = strsql & "isnull(upper([CAP]), '') as CAP, "
        strsql = strsql & "isnull(upper([Comune]), '') as Comune, "
        strsql = strsql & "isnull(upper([Provincia]), '') as Provincia, "
        strsql = strsql & "isnull(upper([Codice Sede Attuazione]), '') as Codice "
        strsql = strsql & "FROM VW_ELENCO_SEDI "
        strsql = strsql & "WHERE ([idente] = " & IdEnte & ") "
        strsql = strsql & "group by [Nome Sede Fisica], [Indirizzo], [Civico], [CAP], [Comune], [Provincia], [Codice Sede Attuazione]"

        'eseguo la query e passo il risultato al datareader
        dtrLeggiDati = ClsServer.CreaDatareader(strsql, HttpContext.Current.Session("conn"))

        'ciclo il risultato della query e costruisco la stringa con i valori multipli
        While dtrLeggiDati.Read
            strValoriMultipli = strValoriMultipli & dtrLeggiDati("Nome") & " - " & dtrLeggiDati("Indirizzo") & " " & dtrLeggiDati("Civico") & dtrLeggiDati("CAP") & " " & dtrLeggiDati("Comune") & " (" & dtrLeggiDati("Provincia") & ") - " & dtrLeggiDati("Codice") & "\par" & vbCrLf
        End While

        'chiudo il datareader
        If Not dtrLeggiDati Is Nothing Then
            dtrLeggiDati.Close()
            dtrLeggiDati = Nothing
        End If

        'passo la stringa con i valori multipli alla funzione per il valore di ritorno
        ACCR_ElencoSediAccreditate_old = strValoriMultipli

        'ritorno la stringa
        Return ACCR_ElencoSediAccreditate_old

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

    Public Property ACCR_allegatoA1Adeguamento(ByVal intIdEnteFase As Integer, ByVal intIdEnte As Integer, ByVal strUserName As String, ByVal intIdCompetenza As Integer, ByVal sqlLocalConn As SqlClient.SqlConnection) As String
        Get
            Dim dtR As DataRow
            Dim dtT As New DataTable

            dtT.Columns.Add(New DataColumn("PARAMETRO", GetType(String)))
            dtT.Columns.Add(New DataColumn("VALORE", GetType(String)))

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDENTE"
            dtR(1) = intIdEnte

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)


            '**Aggiunto in 08/09/2015 da simona cordella 
            'alla riga assegno L'IDENTEFASE
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDENTEFASE"
            dtR(1) = intIdEnteFase


            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)
            '**

            PathDocumento = EseguiGenerazioneModello(7, strUserName, intIdCompetenza, dtT, sqlLocalConn)

            Return PathDocumento
        End Get
        Set(ByVal Value As String)
            PathDocumento = Value
        End Set
    End Property

    Public Property ACCR_letteraavvioprocedimento(ByVal intIdEnteFase As Integer, ByVal intIdEnte As Integer, ByVal strUserName As String, ByVal intIdCompetenza As Integer, ByVal sqlLocalConn As SqlClient.SqlConnection) As String
        Get
            Dim dtR As DataRow
            Dim dtT As New DataTable

            dtT.Columns.Add(New DataColumn("PARAMETRO", GetType(String)))
            dtT.Columns.Add(New DataColumn("VALORE", GetType(String)))

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDENTE"
            dtR(1) = intIdEnte

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)


            '**Aggiunto il 11/07/2021 
            'alla riga assegno L'IDENTEFASE
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDENTEFASE"
            dtR(1) = intIdEnteFase


            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)
            '**

            PathDocumento = EseguiGenerazioneModello(10, strUserName, intIdCompetenza, dtT, sqlLocalConn)

            Return PathDocumento
        End Get
        Set(ByVal Value As String)
            PathDocumento = Value
        End Set
    End Property

    Public Property ACCR_LetteraCompleDocu(ByVal intIdEnte As Integer, ByVal strUserName As String, ByVal intIdCompetenza As Integer, ByVal sqlLocalConn As SqlClient.SqlConnection) As String
        Get
            Dim dtR As DataRow
            Dim dtT As New DataTable

            dtT.Columns.Add(New DataColumn("PARAMETRO", GetType(String)))
            dtT.Columns.Add(New DataColumn("VALORE", GetType(String)))

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDENTE"
            dtR(1) = intIdEnte

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)

            PathDocumento = EseguiGenerazioneModello(11, strUserName, intIdCompetenza, dtT, sqlLocalConn)

            Return PathDocumento
        End Get
        Set(ByVal Value As String)
            PathDocumento = Value
        End Set
    End Property

    Public Property ACCR_letteraavvioprocedimentoAdeg(ByVal intIdEnteFase As Integer, ByVal intIdEnte As Integer, ByVal strUserName As String, ByVal intIdCompetenza As Integer, ByVal sqlLocalConn As SqlClient.SqlConnection) As String
        'modificato il 08/09/2015 da s.c.
        'nuovo paramEtro in ingresso IDENTEFASE
        Get
            Dim dtR As DataRow
            Dim dtT As New DataTable
            'Dim intParametri As Integer = 2
            'Dim i As Integer
            dtT.Columns.Add(New DataColumn("PARAMETRO", GetType(String)))
            dtT.Columns.Add(New DataColumn("VALORE", GetType(String)))

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDENTE"
            dtR(1) = intIdEnte

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)


            '**Aggiunto in 08/09/2015 da simona cordella 
            'alla riga assegno L'IDENTEFASE
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDENTEFASE"
            dtR(1) = intIdEnteFase


            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)
            '**


            PathDocumento = EseguiGenerazioneModello(12, strUserName, intIdCompetenza, dtT, sqlLocalConn)

            Return PathDocumento
        End Get
        Set(ByVal Value As String)
            PathDocumento = Value
        End Set
    End Property

    Public Property ACCR_LetteraCompleDocuAdeg(ByVal intIdEnteFase As Integer, ByVal intIdEnte As Integer, ByVal strUserName As String, ByVal intIdCompetenza As Integer, ByVal sqlLocalConn As SqlClient.SqlConnection) As String
        'modificato il 08/09/2015 da s.c.
        'nuovo parametro in ingresso IDENTEFASE
        Get
            Dim dtR As DataRow
            Dim dtT As New DataTable

            dtT.Columns.Add(New DataColumn("PARAMETRO", GetType(String)))
            dtT.Columns.Add(New DataColumn("VALORE", GetType(String)))

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDENTE"
            dtR(1) = intIdEnte
            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)


            '**Aggiunto in 08/09/2015 da simona cordella 
            'alla riga assegno L'IDENTEFASE
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDENTEFASE"
            dtR(1) = intIdEnteFase


            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)
            '**

            PathDocumento = EseguiGenerazioneModello(13, strUserName, intIdCompetenza, dtT, sqlLocalConn)

            Return PathDocumento
        End Get
        Set(ByVal Value As String)
            PathDocumento = Value
        End Set
    End Property

    Public Property ACCR_letteraadegpositivoenegativo(ByVal intIdEnteFase As Integer, ByVal intIdEnte As Integer, ByVal strUserName As String, ByVal intIdCompetenza As Integer, ByVal sqlLocalConn As SqlClient.SqlConnection) As String
        Get
            Dim dtR As DataRow
            Dim dtT As New DataTable

            dtT.Columns.Add(New DataColumn("PARAMETRO", GetType(String)))
            dtT.Columns.Add(New DataColumn("VALORE", GetType(String)))

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDENTE"
            dtR(1) = intIdEnte

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)


            '**Aggiunto in 08/09/2015 da simona cordella 
            'alla riga assegno L'IDENTEFASE
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDENTEFASE"
            dtR(1) = intIdEnteFase


            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)
            '**

            PathDocumento = EseguiGenerazioneModello(14, strUserName, intIdCompetenza, dtT, sqlLocalConn)

            Return PathDocumento
        End Get
        Set(ByVal Value As String)
            PathDocumento = Value
        End Set
    End Property

    Public Property ACCR_DeterminaAdeguamentoPositivo(ByVal intIdEnteFase As Integer, ByVal intIdEnte As Integer, ByVal strUserName As String, ByVal intIdCompetenza As Integer, ByVal sqlLocalConn As SqlClient.SqlConnection) As String
        Get
            Dim dtR As DataRow
            Dim dtT As New DataTable

            dtT.Columns.Add(New DataColumn("PARAMETRO", GetType(String)))
            dtT.Columns.Add(New DataColumn("VALORE", GetType(String)))

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDENTE"
            dtR(1) = intIdEnte

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)


            '**Aggiunto in 08/09/2015 da simona cordella 
            'alla riga assegno L'IDENTEFASE
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDENTEFASE"
            dtR(1) = intIdEnteFase


            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)
            '**

            PathDocumento = EseguiGenerazioneModello(15, strUserName, intIdCompetenza, dtT, sqlLocalConn)

            Return PathDocumento
        End Get
        Set(ByVal Value As String)
            PathDocumento = Value
        End Set
    End Property

    Public Property ACCR_DeterminaAccreditamentoPositivoArt10(ByVal intIdEnte As Integer, ByVal strUserName As String, ByVal intIdCompetenza As Integer, ByVal sqlLocalConn As SqlClient.SqlConnection) As String
        Get
            Dim dtR As DataRow
            Dim dtT As New DataTable

            dtT.Columns.Add(New DataColumn("PARAMETRO", GetType(String)))
            dtT.Columns.Add(New DataColumn("VALORE", GetType(String)))

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDENTE"
            dtR(1) = intIdEnte

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)

            PathDocumento = EseguiGenerazioneModello(16, strUserName, intIdCompetenza, dtT, sqlLocalConn)

            Return PathDocumento
        End Get
        Set(ByVal Value As String)
            PathDocumento = Value
        End Set
    End Property

    Public Property ACCR_DeterminaAdeguamentoPositivoArt10(ByVal intIdenteFase As Integer, ByVal intIdEnte As Integer, ByVal strUserName As String, ByVal intIdCompetenza As Integer, ByVal sqlLocalConn As SqlClient.SqlConnection) As String
        Get
            Dim dtR As DataRow
            Dim dtT As New DataTable

            dtT.Columns.Add(New DataColumn("PARAMETRO", GetType(String)))
            dtT.Columns.Add(New DataColumn("VALORE", GetType(String)))

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDENTE"
            dtR(1) = intIdEnte

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)


            '**Aggiunto in 08/09/2015 da simona cordella 
            'alla riga assegno L'IDENTEFASE
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDENTEFASE"
            dtR(1) = intIdenteFase


            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)
            '**

            PathDocumento = EseguiGenerazioneModello(17, strUserName, intIdCompetenza, dtT, sqlLocalConn)

            Return PathDocumento
        End Get
        Set(ByVal Value As String)
            PathDocumento = Value
        End Set
    End Property

    Public Property ACCR_DeterminaAdeguamentoPositivoconLimiti(ByVal IntIdEnteFase As Integer, ByVal intIdEnte As Integer, ByVal strUserName As String, ByVal intIdCompetenza As Integer, ByVal sqlLocalConn As SqlClient.SqlConnection) As String
        Get
            Dim dtR As DataRow
            Dim dtT As New DataTable

            dtT.Columns.Add(New DataColumn("PARAMETRO", GetType(String)))
            dtT.Columns.Add(New DataColumn("VALORE", GetType(String)))

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDENTE"
            dtR(1) = intIdEnte

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)


            '**Aggiunto in 08/09/2015 da simona cordella 
            'alla riga assegno L'IDENTEFASE
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDENTEFASE"
            dtR(1) = IntIdEnteFase


            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)
            '**

            PathDocumento = EseguiGenerazioneModello(18, strUserName, intIdCompetenza, dtT, sqlLocalConn)

            Return PathDocumento
        End Get
        Set(ByVal Value As String)
            PathDocumento = Value
        End Set
    End Property
    'ACCR_determinaaccreditamentopositivoconlimitiSedioFigure
    Public Property ACCR_determinaaccreditamentopositivoconlimitiSedioFigure(ByVal intIdEnte As Integer, ByVal strUserName As String, ByVal intIdCompetenza As Integer, ByVal sqlLocalConn As SqlClient.SqlConnection) As String
        Get
            Dim dtR As DataRow
            Dim dtT As New DataTable

            dtT.Columns.Add(New DataColumn("PARAMETRO", GetType(String)))
            dtT.Columns.Add(New DataColumn("VALORE", GetType(String)))

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDENTE"
            dtR(1) = intIdEnte

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)

            PathDocumento = EseguiGenerazioneModello(268, strUserName, intIdCompetenza, dtT, sqlLocalConn)

            Return PathDocumento
        End Get
        Set(ByVal Value As String)
            PathDocumento = Value
        End Set
    End Property
    Public Property ACCR_DeterminaAdeguamentoNegativo(ByVal intIdEnteFase As Integer, ByVal intIdEnte As Integer, ByVal strUserName As String, ByVal intIdCompetenza As Integer, ByVal sqlLocalConn As SqlClient.SqlConnection) As String
        Get
            Dim dtR As DataRow
            Dim dtT As New DataTable

            dtT.Columns.Add(New DataColumn("PARAMETRO", GetType(String)))
            dtT.Columns.Add(New DataColumn("VALORE", GetType(String)))

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDENTE"
            dtR(1) = intIdEnte
            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)


            '**Aggiunto in 08/09/2015 da simona cordella 
            'alla riga assegno L'IDENTEFASE
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDENTEFASE"
            dtR(1) = intIdEnteFase


            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)
            '**

            PathDocumento = EseguiGenerazioneModello(19, strUserName, intIdCompetenza, dtT, sqlLocalConn)

            Return PathDocumento
        End Get
        Set(ByVal Value As String)
            PathDocumento = Value
        End Set
    End Property

    Public Property ACCR_allegatoA2Adeguamento(ByVal intIdEnteFase As Integer, ByVal intIdEnte As Integer, ByVal strUserName As String, ByVal intIdCompetenza As Integer, ByVal sqlLocalConn As SqlClient.SqlConnection) As String
        Get
            Dim dtR As DataRow
            Dim dtT As New DataTable

            dtT.Columns.Add(New DataColumn("PARAMETRO", GetType(String)))
            dtT.Columns.Add(New DataColumn("VALORE", GetType(String)))

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDENTE"
            dtR(1) = intIdEnte

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)


            '**Aggiunto in 08/09/2015 da simona cordella 
            'alla riga assegno L'IDENTEFASE
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDENTEFASE"
            dtR(1) = intIdEnteFase


            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)
            '**

            PathDocumento = EseguiGenerazioneModello(20, strUserName, intIdCompetenza, dtT, sqlLocalConn)

            Return PathDocumento
        End Get
        Set(ByVal Value As String)
            PathDocumento = Value
        End Set
    End Property

    Function ACCR_ElencoRisorseAccreditate(ByVal IdEnte As Integer) As String
        Dim strsql As String
        Dim strValoriMultipli As String = ""
        Dim dtrLeggiDati As SqlClient.SqlDataReader

        'chiudo il datareader
        If Not dtrLeggiDati Is Nothing Then
            dtrLeggiDati.Close()
            dtrLeggiDati = Nothing
        End If

        strsql = "select isnull(upper(EntePersonale.Nome), '') as Nome, " & _
                 "isnull(upper(EntePersonale.Cognome), '') as Cognome, " & _
                 "isnull(upper(EntePersonale.CodiceFiscale), '') as CodiceFiscale, " & _
                 "EntePersonale.DataNascita, " & _
                 "isnull(upper(Comuni.Denominazione), '') as Comune, " & _
                 "isnull(upper(Ruoli.Ruolo), '') as Ruolo " & _
                 "FROM EntePersonale " & _
                 "INNER JOIN Comuni ON EntePersonale.IdComuneNascita = Comuni.IdComune " & _
                 "INNER JOIN EntePersonaleRuoli ON EntePersonale.IdEntePersonale = EntePersonaleRuoli.IdEntePersonale " & _
                 "INNER JOIN Ruoli ON EntePersonaleRuoli.IdRuolo = Ruoli.IdRuolo " & _
                 "WHERE  entepersonale.datafinevalidità is null and entepersonaleruoli.datafinevalidità is null and  Ruoli.RuoloAccreditamento = 1 and  Ruoli.Nascosto = 0 And EntePersonale.IdEnte = " & IdEnte & " And EntePersonaleRuoli.Accreditato = 1 " & _
                 "Order By Cognome, Nome, Ruolo"

        'eseguo la query e passo il risultato al datareader
        dtrLeggiDati = ClsServer.CreaDatareader(strsql, HttpContext.Current.Session("conn"))

        'ciclo il risultato della query e costruisco la stringa con i valori multipli
        While dtrLeggiDati.Read
            strValoriMultipli = strValoriMultipli & dtrLeggiDati("Cognome") & " " & dtrLeggiDati("Nome") & " - " & dtrLeggiDati("CodiceFiscale") & " " & dtrLeggiDati("DataNascita") & " " & dtrLeggiDati("Comune") & " - " & dtrLeggiDati("Ruolo") & "\par" & vbCrLf
        End While

        'chiudo il datareader
        If Not dtrLeggiDati Is Nothing Then
            dtrLeggiDati.Close()
            dtrLeggiDati = Nothing
        End If

        'passo la stringa con i valori multipli alla funzione per il valore di ritorno
        ACCR_ElencoRisorseAccreditate = strValoriMultipli

        'ritorno la stringa
        Return ACCR_ElencoRisorseAccreditate

    End Function

    Function ACCR_ElencoRisorseAccreditateParziale(ByVal IdEnteFase As Integer) As String
        Dim strsql As String
        Dim strValoriMultipli As String = ""
        Dim dtrLeggiDati As SqlClient.SqlDataReader

        'chiudo il datareader
        If Not dtrLeggiDati Is Nothing Then
            dtrLeggiDati.Close()
            dtrLeggiDati = Nothing
        End If

        strsql = "select isnull(upper(EntePersonale.Nome), '') as Nome, " & _
                 "isnull(upper(EntePersonale.Cognome), '') as Cognome, " & _
                 "isnull(upper(EntePersonale.CodiceFiscale), '') as CodiceFiscale, " & _
                 "EntePersonale.DataNascita, " & _
                 "isnull(upper(Comuni.Denominazione), '') as Comune, " & _
                 "isnull(upper(Ruoli.Ruolo), '') as Ruolo " & _
                 "FROM EntePersonale " & _
                 "INNER JOIN Comuni ON EntePersonale.IdComuneNascita = Comuni.IdComune " & _
                 "INNER JOIN EntePersonaleRuoli ON EntePersonale.IdEntePersonale = EntePersonaleRuoli.IdEntePersonale " & _
                 "INNER JOIN Ruoli ON EntePersonaleRuoli.IdRuolo = Ruoli.IdRuolo " & _
                 "INNER JOIN EntiFasi_Risorse efr on efr.IdEntePersonaleRuolo = EntePersonaleRuoli.IDEntePersonaleRuolo " & _
                 "inner join entifasi ef on ef.IdEnteFase = efr.IdEnteFase " & _
                 "WHERE  entepersonale.datafinevalidità is null and entepersonaleruoli.datafinevalidità is null and  Ruoli.RuoloAccreditamento = 1 and  Ruoli.Nascosto = 0 And ef.IdEnteFase = " & IdEnteFase & " And EntePersonaleRuoli.Accreditato = 1 " & _
                 "Order By Cognome, Nome, Ruolo"

        'eseguo la query e passo il risultato al datareader
        dtrLeggiDati = ClsServer.CreaDatareader(strsql, HttpContext.Current.Session("conn"))

        'ciclo il risultato della query e costruisco la stringa con i valori multipli
        While dtrLeggiDati.Read
            strValoriMultipli = strValoriMultipli & dtrLeggiDati("Cognome") & " " & dtrLeggiDati("Nome") & " - " & dtrLeggiDati("CodiceFiscale") & " " & dtrLeggiDati("DataNascita") & " " & dtrLeggiDati("Comune") & " - " & dtrLeggiDati("Ruolo") & "\par" & vbCrLf
        End While

        'chiudo il datareader
        If Not dtrLeggiDati Is Nothing Then
            dtrLeggiDati.Close()
            dtrLeggiDati = Nothing
        End If

        'passo la stringa con i valori multipli alla funzione per il valore di ritorno
        ACCR_ElencoRisorseAccreditateParziale = strValoriMultipli

        'ritorno la stringa
        Return ACCR_ElencoRisorseAccreditateParziale

    End Function

    Function ACCR_ElencoServizi(ByVal IdEnte As Integer) As String
        Dim strsql As String
        Dim strValoriMultipli As String = ""
        Dim dtrLeggiDati As SqlClient.SqlDataReader

        'chiudo il datareader
        If Not dtrLeggiDati Is Nothing Then
            dtrLeggiDati.Close()
            dtrLeggiDati = Nothing
        End If

        strsql = "SELECT sistemi.idsistema,"
        strsql = strsql & " sistemi.sistema,"
        strsql = strsql & " Enti.denominazione,"
        strsql = strsql & " Enti.Codiceregione,"
        strsql = strsql & " case EntiAcquisizioneServizi.StatoRichiesta when 0 then 'Registrato' when 1 then 'Confermato' when 2 then 'Annullato' when 3 then 'Respinto' end as Stato "
        strsql = strsql & " FROM entisistemi "
        strsql = strsql & " inner join sistemi on sistemi.idsistema=entisistemi.idsistema "
        strsql = strsql & " inner join Enti on enti.idente=entisistemi.idente "
        strsql = strsql & " INNER Join "
        strsql = strsql & " EntiAcquisizioneServizi ON entisistemi.IDEnteSistema = EntiAcquisizioneServizi.idEnteSistema "
        strsql = strsql & " WHERE Sistemi.Nascosto=0 and (EntiAcquisizioneServizi.idEnteSecondario = " & IdEnte & ")"
        strsql = strsql & " UNION "
        strsql = strsql & " SELECT 0,"
        strsql = strsql & " 'Formazione' as sistema,"
        strsql = strsql & " 'Regione' as denominazione,"
        strsql = strsql & " 'Regione' as Codiceregione,"
        strsql = strsql & " case EntiAcquisizioneServizi.StatoRichiesta when 0 then 'Registrato' when 1 then 'Confermato' when 2 then 'Annullato' when 3 then 'Respinto' end as Stato "
        strsql = strsql & " FROM Enti "
        strsql = strsql & " INNER Join "
        strsql = strsql & " EntiAcquisizioneServizi ON Enti.IDEnte = EntiAcquisizioneServizi.IdEnteSecondario "
        strsql = strsql & " INNER Join "
        strsql = strsql & " statiEnti ON enti.IDstatoEnte=statienti.idstatoente "
        strsql = strsql & " WHERE EntiAcquisizioneServizi.IdEnteSistema IS NULL AND (EntiAcquisizioneServizi.idEnteSecondario = '" & IdEnte & "')"

        'eseguo la query e passo il risultato al datareader
        dtrLeggiDati = ClsServer.CreaDatareader(strsql, HttpContext.Current.Session("conn"))

        strValoriMultipli = "\par" & vbCrLf

        'ciclo il risultato della query e costruisco la stringa con i valori multipli
        While dtrLeggiDati.Read
            strValoriMultipli = strValoriMultipli & "\trowd\trgaph70\trleft-70\trbrdrl\brdrs\brdrw10 " & vbCrLf
            strValoriMultipli = strValoriMultipli & "\trbrdrt\brdrs\brdrw10 \trbrdrr\brdrs\brdrw10 " & vbCrLf
            strValoriMultipli = strValoriMultipli & "\trbrdrb\brdrs\brdrw10 " & vbCrLf
            strValoriMultipli = strValoriMultipli & "\trpaddl70\trpaddr70\trpaddfl3\trpaddfr3" & vbCrLf
            strValoriMultipli = strValoriMultipli & "\clbrdrl\brdrw10\brdrs\clbrdrt\brdrw10\brdrs\clbrdrr\brdrw10\brdrs\clbrdrb\brdrw10\brdrs " & vbCrLf
            'Writer.WriteLine("\cellx700\clbrdrl\brdrw10\brdrs\clbrdrt\brdrw10\brdrs\clbrdrr\brdrw10\brdrs\clbrdrb\brdrw10\brdrs ")
            strValoriMultipli = strValoriMultipli & "\cellx3000\clbrdrl\brdrw10\brdrs\clbrdrt\brdrw10\brdrs\clbrdrr\brdrw10\brdrs\clbrdrb\brdrw10\brdrs " & vbCrLf
            strValoriMultipli = strValoriMultipli & "\cellx6000\clbrdrl\brdrw10\brdrs\clbrdrt\brdrw10\brdrs\clbrdrr\brdrw10\brdrs\clbrdrb\brdrw10\brdrs " & vbCrLf
            strValoriMultipli = strValoriMultipli & "\cellx8000\clbrdrl\brdrw10\brdrs\clbrdrt\brdrw10\brdrs\clbrdrr\brdrw10\brdrs\clbrdrb\brdrw10\brdrs " & vbCrLf
            'Writer.WriteLine("\cellx9000\clbrdrl\brdrw10\brdrs\clbrdrt\brdrw10\brdrs\clbrdrr\brdrw10\brdrs\clbrdrb\brdrw10\brdrs ")
            strValoriMultipli = strValoriMultipli & "\cellx10040\pard\intbl\nowidctlpar\ri500\qj\f2\fs15\ " & dtrLeggiDati("Sistema") & "\cell " & dtrLeggiDati("Denominazione") & "\cell " & dtrLeggiDati("CodiceRegione") & "\cell " & dtrLeggiDati("Stato") & "\cell\row\pard\f2\fs15" & vbCrLf
            'strValoriMultipli = strValoriMultipli & dtrLeggiDati("Sistema") & " - " & dtrLeggiDati("Denominazione") & " - " & dtrLeggiDati("CodiceRegione") & " - " & dtrLeggiDati("Stato") & "\par" & vbCrLf
        End While

        strValoriMultipli = strValoriMultipli & "\par" & vbCrLf

        'chiudo il datareader
        If Not dtrLeggiDati Is Nothing Then
            dtrLeggiDati.Close()
            dtrLeggiDati = Nothing
        End If

        'passo la stringa con i valori multipli alla funzione per il valore di ritorno
        ACCR_ElencoServizi = strValoriMultipli

        'ritorno la stringa
        Return ACCR_ElencoServizi

    End Function

    Function ACCR_ElencoServiziParziale(ByVal IdEnteFase As Integer) As String
        Dim strsql As String
        Dim strValoriMultipli As String = ""
        Dim dtrLeggiDati As SqlClient.SqlDataReader

        'chiudo il datareader
        If Not dtrLeggiDati Is Nothing Then
            dtrLeggiDati.Close()
            dtrLeggiDati = Nothing
        End If

        strsql = "SELECT sistemi.idsistema,"
        strsql = strsql & " sistemi.sistema,"
        strsql = strsql & " Enti.denominazione,"
        strsql = strsql & " Enti.Codiceregione,"
        strsql = strsql & " case EntiAcquisizioneServizi.StatoRichiesta when 0 then 'Registrato' when 1 then 'Confermato' when 2 then 'Annullato' when 3 then 'Respinto' end as Stato "
        strsql = strsql & " FROM entisistemi "
        strsql = strsql & " inner join sistemi on sistemi.idsistema=entisistemi.idsistema "
        strsql = strsql & " inner join Enti on enti.idente=entisistemi.idente "
        strsql = strsql & " INNER Join "
        strsql = strsql & " EntiAcquisizioneServizi ON entisistemi.IDEnteSistema = EntiAcquisizioneServizi.idEnteSistema "
        strsql = strsql & " INNER JOIN EntiFasi_ServiziAcquisiti EFSA ON EFSA.idEnteAcquisizione =EntiAcquisizioneServizi.idEnteAcquisizione  "
        strsql = strsql & " INNER JOIN EntiFasi EF ON EF.IdEnteFase = EFSA.IDEnteFase "
        strsql = strsql & " WHERE Sistemi.Nascosto=0  and EF.IdEnteFase = " & IdEnteFase & ""
        strsql = strsql & " UNION "
        strsql = strsql & " SELECT 0,"
        strsql = strsql & " 'Formazione' as sistema,"
        strsql = strsql & " 'Regione' as denominazione,"
        strsql = strsql & " 'Regione' as Codiceregione,"
        strsql = strsql & " case EntiAcquisizioneServizi.StatoRichiesta when 0 then 'Registrato' when 1 then 'Confermato' when 2 then 'Annullato' when 3 then 'Respinto' end as Stato "
        strsql = strsql & " FROM Enti "
        strsql = strsql & " INNER Join "
        strsql = strsql & " EntiAcquisizioneServizi ON Enti.IDEnte = EntiAcquisizioneServizi.IdEnteSecondario "
        strsql = strsql & " INNER Join "
        strsql = strsql & " statiEnti ON enti.IDstatoEnte=statienti.idstatoente "
        strsql = strsql & " INNER JOIN EntiFasi_ServiziAcquisiti EFSA ON EFSA.idEnteAcquisizione =EntiAcquisizioneServizi.idEnteAcquisizione  "
        strsql = strsql & " INNER JOIN EntiFasi EF ON EF.IdEnteFase = EFSA.IDEnteFase "
        strsql = strsql & " WHERE EntiAcquisizioneServizi.IdEnteSistema IS NULL and EF.IdEnteFase = " & IdEnteFase & ""

        'eseguo la query e passo il risultato al datareader
        dtrLeggiDati = ClsServer.CreaDatareader(strsql, HttpContext.Current.Session("conn"))

        strValoriMultipli = "\par" & vbCrLf

        'ciclo il risultato della query e costruisco la stringa con i valori multipli
        While dtrLeggiDati.Read
            strValoriMultipli = strValoriMultipli & "\trowd\trgaph70\trleft-70\trbrdrl\brdrs\brdrw10 " & vbCrLf
            strValoriMultipli = strValoriMultipli & "\trbrdrt\brdrs\brdrw10 \trbrdrr\brdrs\brdrw10 " & vbCrLf
            strValoriMultipli = strValoriMultipli & "\trbrdrb\brdrs\brdrw10 " & vbCrLf
            strValoriMultipli = strValoriMultipli & "\trpaddl70\trpaddr70\trpaddfl3\trpaddfr3" & vbCrLf
            strValoriMultipli = strValoriMultipli & "\clbrdrl\brdrw10\brdrs\clbrdrt\brdrw10\brdrs\clbrdrr\brdrw10\brdrs\clbrdrb\brdrw10\brdrs " & vbCrLf
            'Writer.WriteLine("\cellx700\clbrdrl\brdrw10\brdrs\clbrdrt\brdrw10\brdrs\clbrdrr\brdrw10\brdrs\clbrdrb\brdrw10\brdrs ")
            strValoriMultipli = strValoriMultipli & "\cellx3000\clbrdrl\brdrw10\brdrs\clbrdrt\brdrw10\brdrs\clbrdrr\brdrw10\brdrs\clbrdrb\brdrw10\brdrs " & vbCrLf
            strValoriMultipli = strValoriMultipli & "\cellx6000\clbrdrl\brdrw10\brdrs\clbrdrt\brdrw10\brdrs\clbrdrr\brdrw10\brdrs\clbrdrb\brdrw10\brdrs " & vbCrLf
            strValoriMultipli = strValoriMultipli & "\cellx8000\clbrdrl\brdrw10\brdrs\clbrdrt\brdrw10\brdrs\clbrdrr\brdrw10\brdrs\clbrdrb\brdrw10\brdrs " & vbCrLf
            'Writer.WriteLine("\cellx9000\clbrdrl\brdrw10\brdrs\clbrdrt\brdrw10\brdrs\clbrdrr\brdrw10\brdrs\clbrdrb\brdrw10\brdrs ")
            strValoriMultipli = strValoriMultipli & "\cellx10040\pard\intbl\nowidctlpar\ri500\qj\f2\fs15\ " & dtrLeggiDati("Sistema") & "\cell " & dtrLeggiDati("Denominazione") & "\cell " & dtrLeggiDati("CodiceRegione") & "\cell " & dtrLeggiDati("Stato") & "\cell\row\pard\f2\fs15" & vbCrLf
            'strValoriMultipli = strValoriMultipli & dtrLeggiDati("Sistema") & " - " & dtrLeggiDati("Denominazione") & " - " & dtrLeggiDati("CodiceRegione") & " - " & dtrLeggiDati("Stato") & "\par" & vbCrLf
        End While

        strValoriMultipli = strValoriMultipli & "\par" & vbCrLf

        'chiudo il datareader
        If Not dtrLeggiDati Is Nothing Then
            dtrLeggiDati.Close()
            dtrLeggiDati = Nothing
        End If

        'passo la stringa con i valori multipli alla funzione per il valore di ritorno
        ACCR_ElencoServiziParziale = strValoriMultipli

        'ritorno la stringa
        Return ACCR_ElencoServiziParziale

    End Function
    Public Property ACCR_allegatobAdeguamento(ByVal intIdEnteFase As Integer, ByVal intIdEnte As Integer, ByVal strUserName As String, ByVal intIdCompetenza As Integer, ByVal sqlLocalConn As SqlClient.SqlConnection) As String
        Get
            Dim dtR As DataRow
            Dim dtT As New DataTable

            dtT.Columns.Add(New DataColumn("PARAMETRO", GetType(String)))
            dtT.Columns.Add(New DataColumn("VALORE", GetType(String)))

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDENTE"
            dtR(1) = intIdEnte

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)


            '**Aggiunto in 08/09/2015 da simona cordella 
            'alla riga assegno L'IDENTEFASE
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDENTEFASE"
            dtR(1) = intIdEnteFase


            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)
            '**

            PathDocumento = EseguiGenerazioneModello(21, strUserName, intIdCompetenza, dtT, sqlLocalConn)

            Return PathDocumento
        End Get
        Set(ByVal Value As String)
            PathDocumento = Value
        End Set
    End Property


    Function ACCR_ElencoEntiEsclusi(ByVal IdEnte As Integer)
        Dim strsql As String
        Dim strValoriMultipli As String = ""
        Dim dtrLeggiDati As SqlClient.SqlDataReader

        'chiudo il datareader
        If Not dtrLeggiDati Is Nothing Then
            dtrLeggiDati.Close()
            dtrLeggiDati = Nothing
        End If

        strsql = "SELECT entisedi.CAP as CAP, "
        strsql = strsql & "isnull(upper(entisedi.indirizzo), '') as Indirizzo, "
        strsql = strsql & "isnull(upper(comuni.Denominazione), '') as Comune, "
        strsql = strsql & "isnull(upper(provincie.Provincia), '') as Provincia, "
        strsql = strsql & "isnull(upper(enti_2.Denominazione), '') as Nome, "
        strsql = strsql & "isnull(upper(enti_2.CodiceRegione), '') as Codice "
        strsql = strsql & "FROM entirelazioni "
        strsql = strsql & "INNER JOIN enti enti_2 ON entirelazioni.IDEnteFiglio = enti_2.IDEnte "
        strsql = strsql & "INNER JOIN statienti statienti_2 ON enti_2.IDStatoEnte = statienti_2.IDStatoEnte "
        strsql = strsql & "LEFT JOIN entisedi ON entisedi.IDEnte = enti_2.IDEnte "
        strsql = strsql & "AND ENTISEDI.IDENTESEDE  = ANY (SELECT A.IDENTESEDE FROM ENTISEDI A INNER JOIN "
        strsql = strsql & "ENTISEDITIPI ON ENTISEDITIPI.IDENTESEDE = A.IDENTESEDE  WHERE  A.IDENTE = " & IdEnte & " AND ENTISEDITIPI.IDTIPOSEDE = 1)"
        strsql = strsql & "LEFT JOIN comuni ON entisedi.IDComune = comuni.IDComune "
        strsql = strsql & "LEFT JOIN provincie ON comuni.IDProvincia = provincie.IDProvincia "
        strsql = strsql & "WHERE (statienti_2.Sospeso = 1) AND ENTIrelazioni.IDENTEpadre = " & IdEnte & " "
        strsql = strsql & "AND enti_2.datadeterminazionenegativa>= (select isnull(max(datainiziofase),'31/12/2029') from entifasi where tipofase in (1,2) and stato <> 2 AND idente = " & IdEnte & ")"

        'eseguo la query e passo il risultato al datareader
        dtrLeggiDati = ClsServer.CreaDatareader(strsql, HttpContext.Current.Session("conn"))

        'ciclo il risultato della query e costruisco la stringa con i valori multipli
        While dtrLeggiDati.Read
            strValoriMultipli = strValoriMultipli & dtrLeggiDati("Nome") & " " & dtrLeggiDati("Codice") & " " & dtrLeggiDati("CAP") & " " & dtrLeggiDati("Indirizzo") & " " & dtrLeggiDati("Comune") & " (" & dtrLeggiDati("Provincia") & ") " & "\par" & vbCrLf
        End While

        'chiudo il datareader
        If Not dtrLeggiDati Is Nothing Then
            dtrLeggiDati.Close()
            dtrLeggiDati = Nothing
        End If

        'passo la stringa con i valori multipli alla funzione per il valore di ritorno
        ACCR_ElencoEntiEsclusi = strValoriMultipli

        'ritorno la stringa
        Return ACCR_ElencoEntiEsclusi

    End Function

    Function ACCR_ElencoRisorseEscluse(ByVal IdEnte As Integer) As String
        Dim strsql As String
        Dim strValoriMultipli As String = ""
        Dim dtrLeggiDati As SqlClient.SqlDataReader

        'chiudo il datareader
        If Not dtrLeggiDati Is Nothing Then
            dtrLeggiDati.Close()
            dtrLeggiDati = Nothing
        End If

        strsql = "select isnull(upper(entepersonale.Cognome), '') as Cognome, "
        strsql = strsql & "isnull(upper(entepersonale.Nome), '') as Nome, "
        strsql = strsql & "isnull(case len(day(entepersonale.DataNascita)) when 1 then '0' + convert(varchar(20),day(entepersonale.DataNascita)) "
        strsql = strsql & "else convert(varchar(20),day(entepersonale.DataNascita))  end + '/' + "
        strsql = strsql & "(case len(month(entepersonale.DataNascita)) when 1 then '0' + convert(varchar(20),month(entepersonale.DataNascita)) "
        strsql = strsql & "else convert(varchar(20),month(entepersonale.DataNascita))  end + '/' + "
        strsql = strsql & "Convert(varchar(20), Year(entepersonale.DataNascita))),'') as DataNascita,"
        strsql = strsql & "isnull(upper(comuni.Denominazione), '') as Comune, "
        strsql = strsql & "isnull(upper(ruoli.Ruolo), '') as Ruolo "
        strsql = strsql & "FROM entepersonaleruoli "
        strsql = strsql & "INNER JOIN entepersonale ON entepersonaleruoli.IDEntePersonale = entepersonale.IDEntePersonale "
        strsql = strsql & "INNER JOIN enti ON entepersonale.IDEnte = enti.IDEnte "
        strsql = strsql & "INNER JOIN comuni ON entepersonale.IDComuneNascita = comuni.IDComune "
        strsql = strsql & "INNER JOIN ruoli ON entepersonaleruoli.IDRuolo = ruoli.IDRuolo "
        strsql = strsql & "WHERE (entepersonale.datafinevalidità is null ) and (entepersonaleruoli.datafinevalidità is null ) and (entepersonaleruoli.Accreditato = - 1) AND (enti.IDEnte = " & IdEnte & ") "
        strsql = strsql & "AND entepersonaleruoli.dataaccreditamento >= (select isnull(max(datainiziofase),'31/12/2029') from entifasi where tipofase in (1,2) and stato <> 2 AND idente = " & IdEnte & ")"

        'eseguo la query e passo il risultato al datareader
        dtrLeggiDati = ClsServer.CreaDatareader(strsql, HttpContext.Current.Session("conn"))

        'ciclo il risultato della query e costruisco la stringa con i valori multipli
        While dtrLeggiDati.Read
            strValoriMultipli = strValoriMultipli & dtrLeggiDati("Nome") & " " & dtrLeggiDati("Cognome") & " " & dtrLeggiDati("DataNascita") & " " & dtrLeggiDati("Comune") & " " & dtrLeggiDati("Ruolo") & "\par" & vbCrLf
        End While

        'chiudo il datareader
        If Not dtrLeggiDati Is Nothing Then
            dtrLeggiDati.Close()
            dtrLeggiDati = Nothing
        End If

        'passo la stringa con i valori multipli alla funzione per il valore di ritorno
        ACCR_ElencoRisorseEscluse = strValoriMultipli

        'ritorno la stringa
        Return ACCR_ElencoRisorseEscluse

    End Function

    Public Property ACCR_letteraaccreditamentopositivo(ByVal intIdEnte As Integer, ByVal strUserName As String, ByVal intIdCompetenza As Integer, ByVal sqlLocalConn As SqlClient.SqlConnection) As String
        Get
            Dim dtR As DataRow
            Dim dtT As New DataTable

            dtT.Columns.Add(New DataColumn("PARAMETRO", GetType(String)))
            dtT.Columns.Add(New DataColumn("VALORE", GetType(String)))

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDENTE"
            dtR(1) = intIdEnte

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)

            PathDocumento = EseguiGenerazioneModello(22, strUserName, intIdCompetenza, dtT, sqlLocalConn)

            Return PathDocumento
        End Get
        Set(ByVal Value As String)
            PathDocumento = Value
        End Set
    End Property

    Public Property ACCR_letteraaccreditamentonegativo(ByVal intIdEnte As Integer, ByVal strUserName As String, ByVal intIdCompetenza As Integer, ByVal sqlLocalConn As SqlClient.SqlConnection) As String
        Get
            Dim dtR As DataRow
            Dim dtT As New DataTable

            dtT.Columns.Add(New DataColumn("PARAMETRO", GetType(String)))
            dtT.Columns.Add(New DataColumn("VALORE", GetType(String)))

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDENTE"
            dtR(1) = intIdEnte

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)

            PathDocumento = EseguiGenerazioneModello(23, strUserName, intIdCompetenza, dtT, sqlLocalConn)

            Return PathDocumento
        End Get
        Set(ByVal Value As String)
            PathDocumento = Value
        End Set
    End Property

    Public Property ACCR_determinaaccreditamentopositivoconlimiti(ByVal intIdEnte As Integer, ByVal strUserName As String, ByVal intIdCompetenza As Integer, ByVal sqlLocalConn As SqlClient.SqlConnection) As String
        Get
            Dim dtR As DataRow
            Dim dtT As New DataTable

            dtT.Columns.Add(New DataColumn("PARAMETRO", GetType(String)))
            dtT.Columns.Add(New DataColumn("VALORE", GetType(String)))

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDENTE"
            dtR(1) = intIdEnte

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)

            PathDocumento = EseguiGenerazioneModello(24, strUserName, intIdCompetenza, dtT, sqlLocalConn)

            Return PathDocumento
        End Get
        Set(ByVal Value As String)
            PathDocumento = Value
        End Set
    End Property

    Public Property ACCR_determinaaccreditamentonegativo(ByVal intIdEnte As Integer, ByVal strUserName As String, ByVal intIdCompetenza As Integer, ByVal sqlLocalConn As SqlClient.SqlConnection) As String
        Get
            Dim dtR As DataRow
            Dim dtT As New DataTable

            dtT.Columns.Add(New DataColumn("PARAMETRO", GetType(String)))
            dtT.Columns.Add(New DataColumn("VALORE", GetType(String)))

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDENTE"
            dtR(1) = intIdEnte

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)

            PathDocumento = EseguiGenerazioneModello(25, strUserName, intIdCompetenza, dtT, sqlLocalConn)

            Return PathDocumento
        End Get
        Set(ByVal Value As String)
            PathDocumento = Value
        End Set
    End Property

    Public Property ACCR_determinaaccreditamentopositivo(ByVal intIdEnte As Integer, ByVal strUserName As String, ByVal intIdCompetenza As Integer, ByVal sqlLocalConn As SqlClient.SqlConnection) As String
        Get
            Dim dtR As DataRow
            Dim dtT As New DataTable

            dtT.Columns.Add(New DataColumn("PARAMETRO", GetType(String)))
            dtT.Columns.Add(New DataColumn("VALORE", GetType(String)))

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDENTE"
            dtR(1) = intIdEnte

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)

            PathDocumento = EseguiGenerazioneModello(26, strUserName, intIdCompetenza, dtT, sqlLocalConn)

            Return PathDocumento
        End Get
        Set(ByVal Value As String)
            PathDocumento = Value
        End Set
    End Property

    Public Property ACCR_art2accreditamento(ByVal intIdEnteFase As Integer, ByVal intIdEnte As Integer, ByVal strUserName As String, ByVal intIdCompetenza As Integer, ByVal sqlLocalConn As SqlClient.SqlConnection) As String
        Get
            Dim dtR As DataRow
            Dim dtT As New DataTable

            dtT.Columns.Add(New DataColumn("PARAMETRO", GetType(String)))
            dtT.Columns.Add(New DataColumn("VALORE", GetType(String)))

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDENTE"
            dtR(1) = intIdEnte

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)

            ' Aggiunto da Luigi Leucci il 27/02/2019
            dtR = dtT.NewRow()
            dtR(0) = "IDENTEFASE"
            dtR(1) = intIdEnteFase
            dtT.Rows.Add(dtR)
            '  Fine modifica

            PathDocumento = EseguiGenerazioneModello(27, strUserName, intIdCompetenza, dtT, sqlLocalConn)

            Return PathDocumento
        End Get
        Set(ByVal Value As String)
            PathDocumento = Value
        End Set
    End Property

    Public Property ACCR_art10accreditamento(ByVal intIdEnteFase As Integer, ByVal intIdEnte As Integer, ByVal strUserName As String, ByVal intIdCompetenza As Integer, ByVal sqlLocalConn As SqlClient.SqlConnection) As String
        Get
            Dim dtR As DataRow
            Dim dtT As New DataTable

            dtT.Columns.Add(New DataColumn("PARAMETRO", GetType(String)))
            dtT.Columns.Add(New DataColumn("VALORE", GetType(String)))

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDENTE"
            dtR(1) = intIdEnte

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)

            ' Aggiunto da Luigi Leucci il 27/02/2019
            dtR = dtT.NewRow()
            dtR(0) = "IDENTEFASE"
            dtR(1) = intIdEnteFase
            dtT.Rows.Add(dtR)
            '  Fine modifica

            PathDocumento = EseguiGenerazioneModello(28, strUserName, intIdCompetenza, dtT, sqlLocalConn)

            Return PathDocumento
        End Get
        Set(ByVal Value As String)
            PathDocumento = Value
        End Set
    End Property

    Public Property ACCR_art2adeguamento(ByVal intIdEnteFase As Integer, ByVal intIdEnte As Integer, ByVal strUserName As String, ByVal intIdCompetenza As Integer, ByVal sqlLocalConn As SqlClient.SqlConnection) As String
        Get
            Dim dtR As DataRow
            Dim dtT As New DataTable

            dtT.Columns.Add(New DataColumn("PARAMETRO", GetType(String)))
            dtT.Columns.Add(New DataColumn("VALORE", GetType(String)))

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDENTE"
            dtR(1) = intIdEnte

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)


            '**Aggiunto in 08/09/2015 da simona cordella 
            'alla riga assegno L'IDENTEFASE
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDENTEFASE"
            dtR(1) = intIdEnteFase


            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)
            '**

            PathDocumento = EseguiGenerazioneModello(29, strUserName, intIdCompetenza, dtT, sqlLocalConn)

            Return PathDocumento
        End Get
        Set(ByVal Value As String)
            PathDocumento = Value
        End Set
    End Property

    Public Property ACCR_art10adeguamento(ByVal intIdEnteFase As Integer, ByVal intIdEnte As Integer, ByVal strUserName As String, ByVal intIdCompetenza As Integer, ByVal sqlLocalConn As SqlClient.SqlConnection) As String
        Get
            Dim dtR As DataRow
            Dim dtT As New DataTable

            dtT.Columns.Add(New DataColumn("PARAMETRO", GetType(String)))
            dtT.Columns.Add(New DataColumn("VALORE", GetType(String)))

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDENTE"
            dtR(1) = intIdEnte

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)


            '**Aggiunto in 08/09/2015 da simona cordella 
            'alla riga assegno L'IDENTEFASE
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDENTEFASE"
            dtR(1) = intIdEnteFase


            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)
            '**

            PathDocumento = EseguiGenerazioneModello(30, strUserName, intIdCompetenza, dtT, sqlLocalConn)

            Return PathDocumento
        End Get
        Set(ByVal Value As String)
            PathDocumento = Value
        End Set
    End Property

    Public Property ACCR_allegatoA1(ByVal intIdEnte As Integer, ByVal strUserName As String, ByVal intIdCompetenza As Integer, ByVal sqlLocalConn As SqlClient.SqlConnection) As String
        Get
            Dim dtR As DataRow
            Dim dtT As New DataTable

            dtT.Columns.Add(New DataColumn("PARAMETRO", GetType(String)))
            dtT.Columns.Add(New DataColumn("VALORE", GetType(String)))

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDENTE"
            dtR(1) = intIdEnte

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)

            PathDocumento = EseguiGenerazioneModello(31, strUserName, intIdCompetenza, dtT, sqlLocalConn)

            Return PathDocumento
        End Get
        Set(ByVal Value As String)
            PathDocumento = Value
        End Set
    End Property

    Public Property ACCR_allegatoA2(ByVal intIdEnte As Integer, ByVal strUserName As String, ByVal intIdCompetenza As Integer, ByVal sqlLocalConn As SqlClient.SqlConnection) As String
        Get
            Dim dtR As DataRow
            Dim dtT As New DataTable

            dtT.Columns.Add(New DataColumn("PARAMETRO", GetType(String)))
            dtT.Columns.Add(New DataColumn("VALORE", GetType(String)))

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDENTE"
            dtR(1) = intIdEnte

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)

            PathDocumento = EseguiGenerazioneModello(32, strUserName, intIdCompetenza, dtT, sqlLocalConn)

            Return PathDocumento
        End Get
        Set(ByVal Value As String)
            PathDocumento = Value
        End Set
    End Property

    Public Property ACCR_allegatob(ByVal intIdEnte As Integer, ByVal strUserName As String, ByVal intIdCompetenza As Integer, ByVal sqlLocalConn As SqlClient.SqlConnection) As String
        Get
            Dim dtR As DataRow
            Dim dtT As New DataTable

            dtT.Columns.Add(New DataColumn("PARAMETRO", GetType(String)))
            dtT.Columns.Add(New DataColumn("VALORE", GetType(String)))

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDENTE"
            dtR(1) = intIdEnte

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)

            PathDocumento = EseguiGenerazioneModello(33, strUserName, intIdCompetenza, dtT, sqlLocalConn)

            Return PathDocumento
        End Get
        Set(ByVal Value As String)
            PathDocumento = Value
        End Set
    End Property

    Public Property ACCR_FalseSedi(ByVal intIdEnte As Integer, ByVal strUserName As String, ByVal intIdCompetenza As Integer, ByVal sqlLocalConn As SqlClient.SqlConnection) As String
        Get
            Dim dtR As DataRow
            Dim dtT As New DataTable

            dtT.Columns.Add(New DataColumn("PARAMETRO", GetType(String)))
            dtT.Columns.Add(New DataColumn("VALORE", GetType(String)))

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDENTE"
            dtR(1) = intIdEnte

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)

            PathDocumento = EseguiGenerazioneModello(34, strUserName, intIdCompetenza, dtT, sqlLocalConn)

            Return PathDocumento
        End Get
        Set(ByVal Value As String)
            PathDocumento = Value
        End Set
    End Property

    Public Property ACCR_FalseSediAdeg(ByVal intIdEnteFase As Integer, ByVal intIdEnte As Integer, ByVal strUserName As String, ByVal intIdCompetenza As Integer, ByVal sqlLocalConn As SqlClient.SqlConnection) As String
        Get
            Dim dtR As DataRow
            Dim dtT As New DataTable

            dtT.Columns.Add(New DataColumn("PARAMETRO", GetType(String)))
            dtT.Columns.Add(New DataColumn("VALORE", GetType(String)))

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDENTE"
            dtR(1) = intIdEnte

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)


            '**Aggiunto in 08/09/2015 da simona cordella 
            'alla riga assegno L'IDENTEFASE
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDENTEFASE"
            dtR(1) = intIdEnteFase


            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)
            '**

            PathDocumento = EseguiGenerazioneModello(34, strUserName, intIdCompetenza, dtT, sqlLocalConn)

            Return PathDocumento
        End Get
        Set(ByVal Value As String)
            PathDocumento = Value
        End Set
    End Property
    Public Property PROG_letteraditrasmissione1(ByVal intIdEnte As Integer, ByVal IdBando As Integer, ByVal strUserName As String, ByVal intIdCompetenza As Integer, ByVal sqlLocalConn As SqlClient.SqlConnection) As String
        Get
            Dim dtR As DataRow
            Dim dtT As New DataTable
            Dim TipoModello As String = TrovaTipoModello(IdBando)

            dtT.Columns.Add(New DataColumn("PARAMETRO", GetType(String)))
            dtT.Columns.Add(New DataColumn("VALORE", GetType(String)))

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDENTE"
            dtR(1) = intIdEnte

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)

            '**Aggiunto in 28/05/2012 da simona cordella 
            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDBANDO"
            dtR(1) = IdBando


            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)
            '**
            If TipoModello = "SCN" Then
                PathDocumento = EseguiGenerazioneModello(35, strUserName, intIdCompetenza, dtT, sqlLocalConn)
            Else
                PathDocumento = EseguiGenerazioneModello(249, strUserName, intIdCompetenza, dtT, sqlLocalConn)
            End If


            Return PathDocumento
        End Get
        Set(ByVal Value As String)
            PathDocumento = Value
        End Set
    End Property

    Public Property PROG_comunicazionepositiva1(ByVal intIdEnte As Integer, ByVal IdBando As Integer, ByVal strUserName As String, ByVal intIdCompetenza As Integer, ByVal sqlLocalConn As SqlClient.SqlConnection) As String
        Get
            Dim dtR As DataRow
            Dim dtT As New DataTable
            Dim TipoModello As String = TrovaTipoModello(IdBando)

            dtT.Columns.Add(New DataColumn("PARAMETRO", GetType(String)))
            dtT.Columns.Add(New DataColumn("VALORE", GetType(String)))

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDENTE"
            dtR(1) = intIdEnte

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)

            'Aggiunto in 28/05/2012 da simona cordella 
            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDBANDO"
            dtR(1) = IdBando

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)

            If TipoModello = "SCN" Then
                PathDocumento = EseguiGenerazioneModello(36, strUserName, intIdCompetenza, dtT, sqlLocalConn)
            Else
                PathDocumento = EseguiGenerazioneModello(250, strUserName, intIdCompetenza, dtT, sqlLocalConn)
            End If


            Return PathDocumento
        End Get
        Set(ByVal Value As String)
            PathDocumento = Value
        End Set
    End Property

    Public Property PROG_limitataplurima1(ByVal intIdEnte As Integer, ByVal IdBando As Integer, ByVal strUserName As String, ByVal intIdCompetenza As Integer, ByVal sqlLocalConn As SqlClient.SqlConnection) As String
        Get
            Dim dtR As DataRow
            Dim dtT As New DataTable
            Dim TipoModello As String = TrovaTipoModello(IdBando)

            dtT.Columns.Add(New DataColumn("PARAMETRO", GetType(String)))
            dtT.Columns.Add(New DataColumn("VALORE", GetType(String)))

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDENTE"
            dtR(1) = intIdEnte

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)


            '***Aggiunto in 28/05/2012 da simona cordella 
            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDBANDO"
            dtR(1) = IdBando

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)
            '***
            If TipoModello = "SCN" Then
                PathDocumento = EseguiGenerazioneModello(37, strUserName, intIdCompetenza, dtT, sqlLocalConn)
            Else
                PathDocumento = EseguiGenerazioneModello(251, strUserName, intIdCompetenza, dtT, sqlLocalConn)
            End If

            Return PathDocumento
        End Get
        Set(ByVal Value As String)
            PathDocumento = Value
        End Set
    End Property

    Public Property PROG_limitatasingola1(ByVal intIdEnte As Integer, ByVal IdBando As Integer, ByVal strUserName As String, ByVal intIdCompetenza As Integer, ByVal sqlLocalConn As SqlClient.SqlConnection) As String
        Get
            Dim dtR As DataRow
            Dim dtT As New DataTable
            Dim TipoModello As String = TrovaTipoModello(IdBando)

            dtT.Columns.Add(New DataColumn("PARAMETRO", GetType(String)))
            dtT.Columns.Add(New DataColumn("VALORE", GetType(String)))

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDENTE"
            dtR(1) = intIdEnte

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDBANDO"
            dtR(1) = IdBando

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)

            If TipoModello = "SCN" Then
                PathDocumento = EseguiGenerazioneModello(38, strUserName, intIdCompetenza, dtT, sqlLocalConn)
            Else
                PathDocumento = EseguiGenerazioneModello(252, strUserName, intIdCompetenza, dtT, sqlLocalConn)
            End If


            Return PathDocumento
        End Get
        Set(ByVal Value As String)
            PathDocumento = Value
        End Set
    End Property

    Public Property PROG_negativaplurima(ByVal intIdEnte As Integer, ByVal IdBando As Integer, ByVal strUserName As String, ByVal intIdCompetenza As Integer, ByVal sqlLocalConn As SqlClient.SqlConnection) As String
        Get
            Dim dtR As DataRow
            Dim dtT As New DataTable
            Dim TipoModello As String = TrovaTipoModello(IdBando)

            dtT.Columns.Add(New DataColumn("PARAMETRO", GetType(String)))
            dtT.Columns.Add(New DataColumn("VALORE", GetType(String)))

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDENTE"
            dtR(1) = intIdEnte

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)

            '** Aggiunto in 28/05/2012 da simona cordella 
            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDBANDO"
            dtR(1) = IdBando

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)
            '**
            If TipoModello = "SCN" Then
                PathDocumento = EseguiGenerazioneModello(39, strUserName, intIdCompetenza, dtT, sqlLocalConn)
            Else
                PathDocumento = EseguiGenerazioneModello(253, strUserName, intIdCompetenza, dtT, sqlLocalConn)
            End If


            Return PathDocumento
        End Get
        Set(ByVal Value As String)
            PathDocumento = Value
        End Set
    End Property

    Public Property PROG_negativasingola1(ByVal intIdEnte As Integer, ByVal IdBando As Integer, ByVal strUserName As String, ByVal intIdCompetenza As Integer, ByVal sqlLocalConn As SqlClient.SqlConnection) As String
        Get
            Dim dtR As DataRow
            Dim dtT As New DataTable
            Dim TipoModello As String = TrovaTipoModello(IdBando)

            dtT.Columns.Add(New DataColumn("PARAMETRO", GetType(String)))
            dtT.Columns.Add(New DataColumn("VALORE", GetType(String)))

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDENTE"
            dtR(1) = intIdEnte

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDBANDO"
            dtR(1) = IdBando

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)

            If TipoModello = "SCN" Then
                PathDocumento = EseguiGenerazioneModello(40, strUserName, intIdCompetenza, dtT, sqlLocalConn)
            Else
                PathDocumento = EseguiGenerazioneModello(254, strUserName, intIdCompetenza, dtT, sqlLocalConn)
            End If

            Return PathDocumento
        End Get
        Set(ByVal Value As String)
            PathDocumento = Value
        End Set
    End Property

    Public Property PROG_allegatonegativi1(ByVal intIdEnte As Integer, ByVal IdBando As Integer, ByVal strUserName As String, ByVal intIdCompetenza As Integer, ByVal sqlLocalConn As SqlClient.SqlConnection) As String
        Get
            Dim dtR As DataRow
            Dim dtT As New DataTable


            Dim TipoModello As String = TrovaTipoModello(IdBando)

            dtT.Columns.Add(New DataColumn("PARAMETRO", GetType(String)))
            dtT.Columns.Add(New DataColumn("VALORE", GetType(String)))

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDENTE"
            dtR(1) = intIdEnte

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDBANDO"
            dtR(1) = IdBando

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)

            If TipoModello = "SCN" Then
                PathDocumento = EseguiGenerazioneModello(41, strUserName, intIdCompetenza, dtT, sqlLocalConn)
            Else
                PathDocumento = EseguiGenerazioneModello(255, strUserName, intIdCompetenza, dtT, sqlLocalConn)
            End If


            Return PathDocumento
        End Get
        Set(ByVal Value As String)
            PathDocumento = Value
        End Set
    End Property

    Public Property PROG_DecretoProgrammaEsclusoSingolo(ByVal intIdEnte As Integer, ByVal IdBando As Integer, ByVal strUserName As String, ByVal intIdCompetenza As Integer, ByVal sqlLocalConn As SqlClient.SqlConnection) As String
        Get
            Dim dtR As DataRow
            Dim dtT As New DataTable


            Dim TipoModello As String = TrovaTipoModello(IdBando)

            dtT.Columns.Add(New DataColumn("PARAMETRO", GetType(String)))
            dtT.Columns.Add(New DataColumn("VALORE", GetType(String)))

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDENTE"
            dtR(1) = intIdEnte

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDBANDO"
            dtR(1) = IdBando

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)


            PathDocumento = EseguiGenerazioneModello(269, strUserName, intIdCompetenza, dtT, sqlLocalConn)



            Return PathDocumento
        End Get
        Set(ByVal Value As String)
            PathDocumento = Value
        End Set
    End Property

    Public Property PROG_DecretoProgrammaInammissibileSingolo(ByVal intIdEnte As Integer, ByVal IdBando As Integer, ByVal strUserName As String, ByVal intIdCompetenza As Integer, ByVal sqlLocalConn As SqlClient.SqlConnection) As String
        Get
            Dim dtR As DataRow
            Dim dtT As New DataTable


            Dim TipoModello As String = TrovaTipoModello(IdBando)

            dtT.Columns.Add(New DataColumn("PARAMETRO", GetType(String)))
            dtT.Columns.Add(New DataColumn("VALORE", GetType(String)))

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDENTE"
            dtR(1) = intIdEnte

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDBANDO"
            dtR(1) = IdBando

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)


            PathDocumento = EseguiGenerazioneModello(270, strUserName, intIdCompetenza, dtT, sqlLocalConn)



            Return PathDocumento
        End Get
        Set(ByVal Value As String)
            PathDocumento = Value
        End Set
    End Property

    Public Property PROG_DecretoProgrammaPositivoconProgettiLimitatiSingolo(ByVal intIdEnte As Integer, ByVal IdBando As Integer, ByVal strUserName As String, ByVal intIdCompetenza As Integer, ByVal sqlLocalConn As SqlClient.SqlConnection) As String
        Get
            Dim dtR As DataRow
            Dim dtT As New DataTable


            Dim TipoModello As String = TrovaTipoModello(IdBando)

            dtT.Columns.Add(New DataColumn("PARAMETRO", GetType(String)))
            dtT.Columns.Add(New DataColumn("VALORE", GetType(String)))

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDENTE"
            dtR(1) = intIdEnte

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDBANDO"
            dtR(1) = IdBando

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)


            PathDocumento = EseguiGenerazioneModello(271, strUserName, intIdCompetenza, dtT, sqlLocalConn)



            Return PathDocumento
        End Get
        Set(ByVal Value As String)
            PathDocumento = Value
        End Set
    End Property

    Public Property PROG_DecretoProgrammaRidottoSingolo(ByVal intIdEnte As Integer, ByVal IdBando As Integer, ByVal strUserName As String, ByVal intIdCompetenza As Integer, ByVal sqlLocalConn As SqlClient.SqlConnection) As String
        Get
            Dim dtR As DataRow
            Dim dtT As New DataTable


            Dim TipoModello As String = TrovaTipoModello(IdBando)

            dtT.Columns.Add(New DataColumn("PARAMETRO", GetType(String)))
            dtT.Columns.Add(New DataColumn("VALORE", GetType(String)))

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDENTE"
            dtR(1) = intIdEnte

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDBANDO"
            dtR(1) = IdBando

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)


            PathDocumento = EseguiGenerazioneModello(272, strUserName, intIdCompetenza, dtT, sqlLocalConn)



            Return PathDocumento
        End Get
        Set(ByVal Value As String)
            PathDocumento = Value
        End Set
    End Property
    Public Property PROG_DecretoProgrammiEsclusiPlurimo(ByVal intIdEnte As Integer, ByVal IdBando As Integer, ByVal strUserName As String, ByVal intIdCompetenza As Integer, ByVal sqlLocalConn As SqlClient.SqlConnection) As String
        Get
            Dim dtR As DataRow
            Dim dtT As New DataTable


            Dim TipoModello As String = TrovaTipoModello(IdBando)

            dtT.Columns.Add(New DataColumn("PARAMETRO", GetType(String)))
            dtT.Columns.Add(New DataColumn("VALORE", GetType(String)))

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDENTE"
            dtR(1) = intIdEnte

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDBANDO"
            dtR(1) = IdBando

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)


            PathDocumento = EseguiGenerazioneModello(273, strUserName, intIdCompetenza, dtT, sqlLocalConn)



            Return PathDocumento
        End Get
        Set(ByVal Value As String)
            PathDocumento = Value
        End Set
    End Property
    Public Property PROG_DecretoProgrammiInammissibiliPlurimo(ByVal intIdEnte As Integer, ByVal IdBando As Integer, ByVal strUserName As String, ByVal intIdCompetenza As Integer, ByVal sqlLocalConn As SqlClient.SqlConnection) As String
        Get
            Dim dtR As DataRow
            Dim dtT As New DataTable


            Dim TipoModello As String = TrovaTipoModello(IdBando)

            dtT.Columns.Add(New DataColumn("PARAMETRO", GetType(String)))
            dtT.Columns.Add(New DataColumn("VALORE", GetType(String)))

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDENTE"
            dtR(1) = intIdEnte

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDBANDO"
            dtR(1) = IdBando

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)


            PathDocumento = EseguiGenerazioneModello(274, strUserName, intIdCompetenza, dtT, sqlLocalConn)



            Return PathDocumento
        End Get
        Set(ByVal Value As String)
            PathDocumento = Value
        End Set
    End Property

    Public Property PROG_DecretoProgrammiPositiviconProgettiLimitatiPlurimo(ByVal intIdEnte As Integer, ByVal IdBando As Integer, ByVal strUserName As String, ByVal intIdCompetenza As Integer, ByVal sqlLocalConn As SqlClient.SqlConnection) As String
        Get
            Dim dtR As DataRow
            Dim dtT As New DataTable


            Dim TipoModello As String = TrovaTipoModello(IdBando)

            dtT.Columns.Add(New DataColumn("PARAMETRO", GetType(String)))
            dtT.Columns.Add(New DataColumn("VALORE", GetType(String)))

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDENTE"
            dtR(1) = intIdEnte

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDBANDO"
            dtR(1) = IdBando

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)


            PathDocumento = EseguiGenerazioneModello(275, strUserName, intIdCompetenza, dtT, sqlLocalConn)



            Return PathDocumento
        End Get
        Set(ByVal Value As String)
            PathDocumento = Value
        End Set
    End Property

    Public Property PROG_DecretoProgrammiRidottiPlurimo(ByVal intIdEnte As Integer, ByVal IdBando As Integer, ByVal strUserName As String, ByVal intIdCompetenza As Integer, ByVal sqlLocalConn As SqlClient.SqlConnection) As String
        Get
            Dim dtR As DataRow
            Dim dtT As New DataTable


            Dim TipoModello As String = TrovaTipoModello(IdBando)

            dtT.Columns.Add(New DataColumn("PARAMETRO", GetType(String)))
            dtT.Columns.Add(New DataColumn("VALORE", GetType(String)))

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDENTE"
            dtR(1) = intIdEnte

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDBANDO"
            dtR(1) = IdBando

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)


            PathDocumento = EseguiGenerazioneModello(276, strUserName, intIdCompetenza, dtT, sqlLocalConn)



            Return PathDocumento
        End Get
        Set(ByVal Value As String)
            PathDocumento = Value
        End Set
    End Property

    Public Property PROG_LetteraTrasmissioneNegativi(ByVal intIdEnte As Integer, ByVal IdBando As Integer, ByVal strUserName As String, ByVal intIdCompetenza As Integer, ByVal sqlLocalConn As SqlClient.SqlConnection) As String
        Get
            Dim dtR As DataRow
            Dim dtT As New DataTable


            Dim TipoModello As String = TrovaTipoModello(IdBando)

            dtT.Columns.Add(New DataColumn("PARAMETRO", GetType(String)))
            dtT.Columns.Add(New DataColumn("VALORE", GetType(String)))

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDENTE"
            dtR(1) = intIdEnte

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDBANDO"
            dtR(1) = IdBando

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)


            PathDocumento = EseguiGenerazioneModello(278, strUserName, intIdCompetenza, dtT, sqlLocalConn)



            Return PathDocumento
        End Get
        Set(ByVal Value As String)
            PathDocumento = Value
        End Set
    End Property
    Public Property PROG_LetteraTrasmissionePositivi(ByVal intIdEnte As Integer, ByVal IdBando As Integer, ByVal strUserName As String, ByVal intIdCompetenza As Integer, ByVal sqlLocalConn As SqlClient.SqlConnection) As String
        Get
            Dim dtR As DataRow
            Dim dtT As New DataTable


            Dim TipoModello As String = TrovaTipoModello(IdBando)

            dtT.Columns.Add(New DataColumn("PARAMETRO", GetType(String)))
            dtT.Columns.Add(New DataColumn("VALORE", GetType(String)))

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDENTE"
            dtR(1) = intIdEnte

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDBANDO"
            dtR(1) = IdBando

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)


            PathDocumento = EseguiGenerazioneModello(279, strUserName, intIdCompetenza, dtT, sqlLocalConn)



            Return PathDocumento
        End Get
        Set(ByVal Value As String)
            PathDocumento = Value
        End Set
    End Property
    Function PROG_ProgettiNegativiConMotivazioni(ByVal IdEnte As Integer, ByVal IdBando As Integer) As String
        Dim strsql As String
        Dim strValoriMultipli As String = ""
        Dim dtrLeggiDati As SqlClient.SqlDataReader
        Dim dttLeggiDati As DataTable
        Dim myRow As DataRow
        Dim intCont As Integer = 0

        'chiudo il datareader
        If Not dtrLeggiDati Is Nothing Then
            dtrLeggiDati.Close()
            dtrLeggiDati = Nothing
        End If

        strsql = "Select IdAttività, idente, idbando, BandoBreve, Titolo from VW_MOD_PROGETTI_NEGATIVI "
        strsql = strsql & "where idbando=" & IdBando & " and IdEnte=" & IdEnte
        'eseguo la query e passo il risultato al datareader
        'dtrLeggiDati = ClsServer.CreaDatareader(strsql, Session("conn"))
        dttLeggiDati = CreaDataTable(strsql, HttpContext.Current.Session("conn"))

        For Each myRow In dttLeggiDati.Rows
            intCont = intCont + 1
            strValoriMultipli = strValoriMultipli & "\b " & intCont & ")     " & myRow.Item("Titolo") & " \b0\par" & vbCrLf
            strsql = "SELECT IdAttività, Motivazione from VW_MOD_PROGETTI_NEGATIVI_MOTIVAZIONI WHERE IdAttività = " & myRow.Item("IdAttività")

            'chiudo il datareader
            If Not dtrLeggiDati Is Nothing Then
                dtrLeggiDati.Close()
                dtrLeggiDati = Nothing
            End If

            'eseguo la query e passo il risultato al datareader
            dtrLeggiDati = ClsServer.CreaDatareader(strsql, HttpContext.Current.Session("conn"))
            'ciclo il risultato della query e costruisco la stringa con i valori multipli
            While dtrLeggiDati.Read
                strValoriMultipli = strValoriMultipli & dtrLeggiDati("Motivazione") & "\par" & vbCrLf
            End While
        Next
        'chiudo il datareader
        If Not dtrLeggiDati Is Nothing Then
            dtrLeggiDati.Close()
            dtrLeggiDati = Nothing
        End If

        'passo la stringa con i valori multipli alla funzione per il valore di ritorno
        PROG_ProgettiNegativiConMotivazioni = strValoriMultipli

        'ritorno la stringa
        Return PROG_ProgettiNegativiConMotivazioni

    End Function

    Public Property PROG_allegatopositivi1(ByVal intIdEnte As Integer, ByVal IdBando As Integer, ByVal strUserName As String, ByVal intIdCompetenza As Integer, ByVal sqlLocalConn As SqlClient.SqlConnection) As String
        Get
            Dim dtR As DataRow
            Dim dtT As New DataTable

            Dim TipoModello As String = TrovaTipoModello(IdBando)

            dtT.Columns.Add(New DataColumn("PARAMETRO", GetType(String)))
            dtT.Columns.Add(New DataColumn("VALORE", GetType(String)))

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDENTE"
            dtR(1) = intIdEnte

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDBANDO"
            dtR(1) = IdBando

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)

            If TipoModello = "SCN" Then
                PathDocumento = EseguiGenerazioneModello(42, strUserName, intIdCompetenza, dtT, sqlLocalConn)
            Else
                PathDocumento = EseguiGenerazioneModello(256, strUserName, intIdCompetenza, dtT, sqlLocalConn)
            End If

            Return PathDocumento
        End Get
        Set(ByVal Value As String)
            PathDocumento = Value
        End Set
    End Property

    Function PROG_ElencoProgettiPositivi(ByVal IdEnte As Integer, ByVal IdBando As Integer) As String
        Dim strsql As String
        Dim strValoriMultipli As String = ""
        Dim dtrLeggiDati As SqlClient.SqlDataReader
        Dim num As Integer
        Dim punteggio As String
        Dim ciclovirgola As Integer
        Dim AppoTitolo As String

        'chiudo il datareader
        If Not dtrLeggiDati Is Nothing Then
            dtrLeggiDati.Close()
            dtrLeggiDati = Nothing
        End If

        strsql = "SELECT  idattività, idente, idbando, bandobreve, Titolo, Punteggio, NVolontari, Limitato from VW_MOD_PROGETTI_POSITIVI where idbando=" & IdBando & " and IdEnte=" & IdEnte

        'eseguo la query e passo il risultato al datareader
        dtrLeggiDati = ClsServer.CreaDatareader(strsql, HttpContext.Current.Session("conn"))

        'ciclo il risultato della query e costruisco la stringa con i valori multipli
        While dtrLeggiDati.Read
            num = num + 1

            AppoTitolo = dtrLeggiDati("Titolo")

            punteggio = dtrLeggiDati("Punteggio")

            For ciclovirgola = 1 To Len(punteggio)
                If Mid(punteggio, ciclovirgola, 1) = "," Then
                    'punteggio = Mid(myRow.Item("Punteggio"), 1, ciclo - 1)
                    If Mid(dtrLeggiDati("Punteggio"), ciclovirgola + 1, 2) = "00" Then
                        'punteggio = Mid(punteggio, 1, ciclovirgola - 1)
                        punteggio = CInt(punteggio)
                    End If
                End If
            Next

            strValoriMultipli = strValoriMultipli & "\b " & num & ")" & Trim(AppoTitolo) & " - Punteggio: " & punteggio & " n.volontari " & dtrLeggiDati("NVOLONTARI") & " " & dtrLeggiDati("LIMITATO") & "\b0\par" & vbCrLf

        End While

        'chiudo il datareader
        If Not dtrLeggiDati Is Nothing Then
            dtrLeggiDati.Close()
            dtrLeggiDati = Nothing
        End If

        'passo la stringa con i valori multipli alla funzione per il valore di ritorno
        PROG_ElencoProgettiPositivi = strValoriMultipli

        'ritorno la stringa
        Return PROG_ElencoProgettiPositivi

    End Function

    Public Property PROG_allegatopositivilimitati1(ByVal intIdEnte As Integer, ByVal IdBando As Integer, ByVal strUserName As String, ByVal intIdCompetenza As Integer, ByVal sqlLocalConn As SqlClient.SqlConnection) As String
        Get
            Dim dtR As DataRow
            Dim dtT As New DataTable

            Dim TipoModello As String = TrovaTipoModello(IdBando)

            dtT.Columns.Add(New DataColumn("PARAMETRO", GetType(String)))
            dtT.Columns.Add(New DataColumn("VALORE", GetType(String)))

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDENTE"
            dtR(1) = intIdEnte

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDBANDO"
            dtR(1) = IdBando

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)
            If TipoModello = "SCN" Then
                PathDocumento = EseguiGenerazioneModello(43, strUserName, intIdCompetenza, dtT, sqlLocalConn)
            Else
                PathDocumento = EseguiGenerazioneModello(257, strUserName, intIdCompetenza, dtT, sqlLocalConn)
            End If


            Return PathDocumento
        End Get
        Set(ByVal Value As String)
            PathDocumento = Value
        End Set
    End Property

    Function PROG_ElencoProgettiPositiviLimitati(ByVal IdEnte As Integer, ByVal IdBando As Integer) As String
        Dim strsql As String
        Dim strValoriMultipli As String = ""
        Dim dtrLeggiDati As SqlClient.SqlDataReader
        Dim num As Integer
        Dim punteggio As String
        Dim ciclovirgola As Integer
        Dim AppoTitolo As String

        'chiudo il datareader
        If Not dtrLeggiDati Is Nothing Then
            dtrLeggiDati.Close()
            dtrLeggiDati = Nothing
        End If

        strsql = "SELECT  idattività, idente, idbando, bandobreve, Titolo, Punteggio, NVolontari, Limitato, Limitazioni from VW_MOD_PROGETTI_POSITIVI_LIMITATI where idbando=" & IdBando & " and IdEnte=" & IdEnte

        'eseguo la query e passo il risultato al datareader
        dtrLeggiDati = ClsServer.CreaDatareader(strsql, HttpContext.Current.Session("conn"))

        'ciclo il risultato della query e costruisco la stringa con i valori multipli
        While dtrLeggiDati.Read
            num = num + 1
            AppoTitolo = dtrLeggiDati("Titolo")
            strValoriMultipli = strValoriMultipli & "\b " & num & ") " & AppoTitolo & " - " & " n.volontari " & dtrLeggiDati("NVOLONTARI") & " " & dtrLeggiDati("LIMITATO") & "\b0\par" & vbCrLf
            strValoriMultipli = strValoriMultipli & dtrLeggiDati("Limitazioni") & "\par" & vbCrLf
        End While

        'chiudo il datareader
        If Not dtrLeggiDati Is Nothing Then
            dtrLeggiDati.Close()
            dtrLeggiDati = Nothing
        End If

        'passo la stringa con i valori multipli alla funzione per il valore di ritorno
        PROG_ElencoProgettiPositiviLimitati = strValoriMultipli

        'ritorno la stringa
        Return PROG_ElencoProgettiPositiviLimitati

    End Function

    Function PROG_ElencoProgettiPositiviLimitatiDaProgrammaPositivo(ByVal IdEnte As Integer, ByVal IdBando As Integer) As String
        Dim strsql As String
        Dim intIdProgramma As Integer = 0
        Dim dtrLeggiDati As SqlClient.SqlDataReader
        

        'chiudo il datareader
        If Not dtrLeggiDati Is Nothing Then
            dtrLeggiDati.Close()
            dtrLeggiDati = Nothing
        End If

        strsql = "SELECT top 1 idprogramma from VW_EDITOR_PROGETTI_POSITIVI_LIMITATI_DA_PROGRAMMA where Riduzioni = 'No' and idbando=" & IdBando & " and IdEnte=" & IdEnte & " order by TitoloProgramma"

        'eseguo la query e passo il risultato al datareader
        dtrLeggiDati = ClsServer.CreaDatareader(strsql, HttpContext.Current.Session("conn"))

        'ciclo il risultato della query e costruisco la stringa con i valori multipli
        While dtrLeggiDati.Read
            intIdProgramma = dtrLeggiDati("IdProgramma")
        End While

        'chiudo il datareader
        If Not dtrLeggiDati Is Nothing Then
            dtrLeggiDati.Close()
            dtrLeggiDati = Nothing
        End If

        'passo la stringa con i valori multipli alla funzione per il valore di ritorno
        PROG_ElencoProgettiPositiviLimitatiDaProgrammaPositivo = PROG_ElencoProgettiPositiviLimitatiDaProgramma(intIdProgramma)

        'ritorno la stringa
        Return PROG_ElencoProgettiPositiviLimitatiDaProgrammaPositivo

    End Function

    Function PROG_ElencoProgettiPositiviLimitatiDaProgrammaRidotto(ByVal IdEnte As Integer, ByVal IdBando As Integer) As String
        Dim strsql As String
        Dim intIdProgramma As Integer = 0
        Dim dtrLeggiDati As SqlClient.SqlDataReader


        'chiudo il datareader
        If Not dtrLeggiDati Is Nothing Then
            dtrLeggiDati.Close()
            dtrLeggiDati = Nothing
        End If

        strsql = "SELECT top 1 idprogramma from VW_EDITOR_PROGETTI_POSITIVI_LIMITATI_DA_PROGRAMMA where Riduzioni = 'Si' and idbando=" & IdBando & " and IdEnte=" & IdEnte & " order by TitoloProgramma"

        'eseguo la query e passo il risultato al datareader
        dtrLeggiDati = ClsServer.CreaDatareader(strsql, HttpContext.Current.Session("conn"))

        'ciclo il risultato della query e costruisco la stringa con i valori multipli
        While dtrLeggiDati.Read
            intIdProgramma = dtrLeggiDati("IdProgramma")
        End While

        'chiudo il datareader
        If Not dtrLeggiDati Is Nothing Then
            dtrLeggiDati.Close()
            dtrLeggiDati = Nothing
        End If

        'passo la stringa con i valori multipli alla funzione per il valore di ritorno
        PROG_ElencoProgettiPositiviLimitatiDaProgrammaRidotto = PROG_ElencoProgettiPositiviLimitatiDaProgramma(intIdProgramma)

        'ritorno la stringa
        Return PROG_ElencoProgettiPositiviLimitatiDaProgrammaRidotto

    End Function

    Function PROG_ElencoProgettiPositiviLimitatiDaProgramma(ByVal IdProgramma As Integer) As String
        Dim strsql As String
        Dim strValoriMultipli As String = ""
        Dim dtrLeggiDati As SqlClient.SqlDataReader
        Dim num As Integer
        Dim punteggio As String
        Dim ciclovirgola As Integer
        Dim AppoTitolo As String

        'chiudo il datareader
        If Not dtrLeggiDati Is Nothing Then
            dtrLeggiDati.Close()
            dtrLeggiDati = Nothing
        End If

        strsql = "SELECT IdProgramma, TitoloProgetto, NVolontari, Limitazioni from VW_EDITOR_PROGETTI_POSITIVI_LIMITATI_DA_PROGRAMMA where IdProgramma=" & IdProgramma

        'eseguo la query e passo il risultato al datareader
        dtrLeggiDati = ClsServer.CreaDatareader(strsql, HttpContext.Current.Session("conn"))

        'ciclo il risultato della query e costruisco la stringa con i valori multipli
        While dtrLeggiDati.Read
            num = num + 1
            AppoTitolo = dtrLeggiDati("TitoloProgetto")
            strValoriMultipli = strValoriMultipli & "\b " & num & ") " & AppoTitolo & " - " & " n.volontari " & dtrLeggiDati("NVOLONTARI") & "\b0\par" & vbCrLf '& " " & dtrLeggiDati("LIMITATO") & "\b0\par" & vbCrLf
            strValoriMultipli = strValoriMultipli & dtrLeggiDati("Limitazioni") & "\par" & vbCrLf
        End While

        'chiudo il datareader
        If Not dtrLeggiDati Is Nothing Then
            dtrLeggiDati.Close()
            dtrLeggiDati = Nothing
        End If

        'passo la stringa con i valori multipli alla funzione per il valore di ritorno
        PROG_ElencoProgettiPositiviLimitatiDaProgramma = strValoriMultipli

        'ritorno la stringa
        Return PROG_ElencoProgettiPositiviLimitatiDaProgramma

    End Function

    Function PROG_ElencoProgrammiEsclusi(ByVal IdEnte As Integer, ByVal IdBando As Integer) As String
        Dim strsql As String
        Dim strValoriMultipli As String = ""
        Dim dtrLeggiDati As SqlClient.SqlDataReader
        Dim num As Integer
        Dim punteggio As String
        Dim ciclovirgola As Integer
        Dim AppoTitolo As String

        'chiudo il datareader
        If Not dtrLeggiDati Is Nothing Then
            dtrLeggiDati.Close()
            dtrLeggiDati = Nothing
        End If

        strsql = "SELECT IdProgramma, TitoloProgramma, MotivazioneNegativo from VW_EDITOR_PROGRAMMI  where StatoProgramma = 'Escluso' and idbando=" & IdBando & " and IdEnte=" & IdEnte & " order by TitoloProgramma"

        'eseguo la query e passo il risultato al datareader
        dtrLeggiDati = ClsServer.CreaDatareader(strsql, HttpContext.Current.Session("conn"))

        'ciclo il risultato della query e costruisco la stringa con i valori multipli
        While dtrLeggiDati.Read
            num = num + 1
            AppoTitolo = dtrLeggiDati("TitoloProgramma")
            strValoriMultipli = strValoriMultipli & "\b " & num & ") " & AppoTitolo & " \b0\par" & vbCrLf '& " " & dtrLeggiDati("LIMITATO") & "\b0\par" & vbCrLf
            strValoriMultipli = strValoriMultipli & dtrLeggiDati("MotivazioneNegativo") & "\par" & vbCrLf
        End While

        'chiudo il datareader
        If Not dtrLeggiDati Is Nothing Then
            dtrLeggiDati.Close()
            dtrLeggiDati = Nothing
        End If

        'passo la stringa con i valori multipli alla funzione per il valore di ritorno
        PROG_ElencoProgrammiEsclusi = strValoriMultipli

        'ritorno la stringa
        Return PROG_ElencoProgrammiEsclusi

    End Function
    Function PROG_ElencoProgrammiInammissibili(ByVal IdEnte As Integer, ByVal IdBando As Integer) As String
        Dim strsql As String
        Dim strValoriMultipli As String = ""
        Dim dtrLeggiDati As SqlClient.SqlDataReader
        Dim num As Integer
        Dim punteggio As String
        Dim ciclovirgola As Integer
        Dim AppoTitolo As String

        'chiudo il datareader
        If Not dtrLeggiDati Is Nothing Then
            dtrLeggiDati.Close()
            dtrLeggiDati = Nothing
        End If

        strsql = "SELECT IdProgramma, TitoloProgramma, MotivazioneNegativo from VW_EDITOR_PROGRAMMI  where StatoProgramma = 'Non Ammissibile' and idbando=" & IdBando & " and IdEnte=" & IdEnte & " order by TitoloProgramma"

        'eseguo la query e passo il risultato al datareader
        dtrLeggiDati = ClsServer.CreaDatareader(strsql, HttpContext.Current.Session("conn"))

        'ciclo il risultato della query e costruisco la stringa con i valori multipli
        While dtrLeggiDati.Read
            num = num + 1
            AppoTitolo = dtrLeggiDati("TitoloProgramma")
            strValoriMultipli = strValoriMultipli & "\b " & num & ") " & AppoTitolo & " \b0\par" & vbCrLf '& " " & dtrLeggiDati("LIMITATO") & "\b0\par" & vbCrLf
            strValoriMultipli = strValoriMultipli & dtrLeggiDati("MotivazioneNegativo") & "\par" & vbCrLf
        End While

        'chiudo il datareader
        If Not dtrLeggiDati Is Nothing Then
            dtrLeggiDati.Close()
            dtrLeggiDati = Nothing
        End If

        'passo la stringa con i valori multipli alla funzione per il valore di ritorno
        PROG_ElencoProgrammiInammissibili = strValoriMultipli

        'ritorno la stringa
        Return PROG_ElencoProgrammiInammissibili

    End Function






   
    

    Function PROG_ElencoProgrammiPositiviconProgettiLimitati(ByVal IdEnte As Integer, ByVal IdBando As Integer) As String
        Dim strsql As String
        Dim strValoriMultipli As String = ""
        Dim dtrLeggiDati As SqlClient.SqlDataReader
        Dim num As Integer
        Dim punteggio As String
        Dim ciclovirgola As Integer
        Dim AppoTitolo As String

        'DATATABLE PROGRAMMI
        Dim dttLocal As New DataTable
        dttLocal = CreaDataTable("SELECT IdProgramma, TitoloProgramma, MotivazioneNegativo from VW_EDITOR_PROGRAMMI where StatoProgramma = 'Ammissibile' and Riduzioni = 'No' and NProgettiLimitati>0 and idbando=" & IdBando & " and IdEnte=" & IdEnte & " order by TitoloProgramma", HttpContext.Current.Session("conn"))
        'controllo se la query restituisce le informazioni
        If dttLocal.Rows.Count > 0 Then
            Dim dttRow As DataRow
            For Each dttRow In dttLocal.Rows
                'INTESTAZIONE PROGRAMMA
                num = num + 1
                AppoTitolo = dttRow.Item("TitoloProgramma")
                strValoriMultipli = strValoriMultipli & "\b " & num & ") " & AppoTitolo & " \b0\par" & vbCrLf '& " " & dtrLeggiDati("LIMITATO") & "\b0\par" & vbCrLf
                strValoriMultipli = strValoriMultipli & "ELENCO PROGETTI LIMITATI:" & "\par" & vbCrLf

                'ELENCO PROGETTI
                'chiudo il datareader
                If Not dtrLeggiDati Is Nothing Then
                    dtrLeggiDati.Close()
                    dtrLeggiDati = Nothing
                End If

                strsql = "SELECT IdProgramma, TitoloProgetto, NVolontari, Limitazioni from VW_EDITOR_PROGETTI_POSITIVI_LIMITATI_DA_PROGRAMMA where IdProgramma=" & dttRow.Item("IdProgramma")

                'eseguo la query e passo il risultato al datareader
                dtrLeggiDati = ClsServer.CreaDatareader(strsql, HttpContext.Current.Session("conn"))

                'ciclo il risultato della query e costruisco la stringa con i valori multipli
                While dtrLeggiDati.Read
                    AppoTitolo = dtrLeggiDati("TitoloProgetto")
                    strValoriMultipli = strValoriMultipli & "\b\i  " & AppoTitolo & " - " & " n.volontari " & dtrLeggiDati("NVOLONTARI") & "\b0\i0\par" & vbCrLf '& " " & dtrLeggiDati("LIMITATO") & "\b0\par" & vbCrLf
                    strValoriMultipli = strValoriMultipli & dtrLeggiDati("Limitazioni") & "\par" & vbCrLf
                End While

                'chiudo il datareader
                If Not dtrLeggiDati Is Nothing Then
                    dtrLeggiDati.Close()
                    dtrLeggiDati = Nothing
                End If
            Next
        End If

        'passo la stringa con i valori multipli alla funzione per il valore di ritorno
        PROG_ElencoProgrammiPositiviconProgettiLimitati = strValoriMultipli

        'ritorno la stringa
        Return PROG_ElencoProgrammiPositiviconProgettiLimitati

    End Function

    Function PROG_ElencoProgrammiRidotti(ByVal IdEnte As Integer, ByVal IdBando As Integer) As String
        Dim strsql As String
        Dim strValoriMultipli As String = ""
        Dim dtrLeggiDati As SqlClient.SqlDataReader
        Dim num As Integer
        Dim punteggio As String
        Dim ciclovirgola As Integer
        Dim AppoTitolo As String

        'DATATABLE PROGRAMMI
        Dim dttLocal As New DataTable
        dttLocal = CreaDataTable("SELECT IdProgramma, TitoloProgramma, MotivazioneRidotto, NProgettiLimitati from VW_EDITOR_PROGRAMMI where StatoProgramma = 'Ammissibile' and Riduzioni = 'Si' and idbando=" & IdBando & " and IdEnte=" & IdEnte & " order by TitoloProgramma", HttpContext.Current.Session("conn"))
        'controllo se la query restituisce le informazioni
        If dttLocal.Rows.Count > 0 Then
            Dim dttRow As DataRow
            For Each dttRow In dttLocal.Rows
                'INTESTAZIONE PROGRAMMA
                num = num + 1
                AppoTitolo = dttRow.Item("TitoloProgramma")
                strValoriMultipli = strValoriMultipli & "\b " & num & ") " & AppoTitolo & " \b0\par" & vbCrLf '& " " & dtrLeggiDati("LIMITATO") & "\b0\par" & vbCrLf
                strValoriMultipli = strValoriMultipli & dttRow.Item("MotivazioneRidotto") & "\par" & vbCrLf

                If dttRow.Item("NProgettiLimitati") > 0 Then
                    strValoriMultipli = strValoriMultipli & "ELENCO PROGETTI LIMITATI:" & "\par" & vbCrLf
                    'ELENCO PROGETTI
                    'chiudo il datareader
                    If Not dtrLeggiDati Is Nothing Then
                        dtrLeggiDati.Close()
                        dtrLeggiDati = Nothing
                    End If

                    strsql = "SELECT IdProgramma, TitoloProgetto, NVolontari, Limitazioni from VW_EDITOR_PROGETTI_POSITIVI_LIMITATI_DA_PROGRAMMA where IdProgramma=" & dttRow.Item("IdProgramma")

                    'eseguo la query e passo il risultato al datareader
                    dtrLeggiDati = ClsServer.CreaDatareader(strsql, HttpContext.Current.Session("conn"))

                    'ciclo il risultato della query e costruisco la stringa con i valori multipli
                    While dtrLeggiDati.Read
                        AppoTitolo = dtrLeggiDati("TitoloProgetto")
                        strValoriMultipli = strValoriMultipli & "\b\i  " & AppoTitolo & " - " & " n.volontari " & dtrLeggiDati("NVOLONTARI") & "\b0\i0\par" & vbCrLf '& " " & dtrLeggiDati("LIMITATO") & "\b0\par" & vbCrLf
                        strValoriMultipli = strValoriMultipli & dtrLeggiDati("Limitazioni") & "\par" & vbCrLf
                    End While

                    'chiudo il datareader
                    If Not dtrLeggiDati Is Nothing Then
                        dtrLeggiDati.Close()
                        dtrLeggiDati = Nothing
                    End If
                End If
            Next
        End If

        'passo la stringa con i valori multipli alla funzione per il valore di ritorno
        PROG_ElencoProgrammiRidotti = strValoriMultipli

        'ritorno la stringa
        Return PROG_ElencoProgrammiRidotti

    End Function
    Function PROG_SostituzioniApprovate(ByVal IdEnte As Integer, ByVal IdIstanzaSostituzioneOLP As Integer) As String
        Dim strsql As String
        Dim strValoriMultipli As String = ""
        Dim dtrLeggiDati As SqlClient.SqlDataReader
        Dim num As Integer
        Dim punteggio As String
        Dim ciclovirgola As Integer
        Dim AppoTitolo As String

        'DATATABLE SOSTITUZIONI -- VW_EDITOR_ELENCOSOSTITUZIONIOLP da mettere!!!!
        strsql = "SELECT  progetto + ' - ' + sede as progetto, sostituito, subentrante from VW_EDITOR_ELENCOSOSTITUZIONIOLP where Stato = 'Approvata' and IdIstanzaSostituzioneOLP=" & IdIstanzaSostituzioneOLP & " order by Progetto, Sede"
        dtrLeggiDati = ClsServer.CreaDatareader(strsql, HttpContext.Current.Session("conn"))

        strValoriMultipli = strValoriMultipli & "\trowd\trgaph70\trleft-70\trbrdrl\brdrs\brdrw10 "
        strValoriMultipli = strValoriMultipli & "\trbrdrt\brdrs\brdrw10 \trbrdrr\brdrs\brdrw10 "
        strValoriMultipli = strValoriMultipli & "\trbrdrb\brdrs\brdrw10 "
        strValoriMultipli = strValoriMultipli & "\trpaddl70\trpaddr70\trpaddfl3\trpaddfr3"
        strValoriMultipli = strValoriMultipli & "\clbrdrl\brdrw10\brdrs\clbrdrt\brdrw10\brdrs\clbrdrr\brdrw10\brdrs\clbrdrb\brdrw10\brdrs "
         strValoriMultipli = strValoriMultipli & "\cellx4000\clbrdrl\brdrw10\brdrs\clbrdrt\brdrw10\brdrs\clbrdrr\brdrw10\brdrs\clbrdrb\brdrw10\brdrs "
        strValoriMultipli = strValoriMultipli & "\cellx7000\clbrdrl\brdrw10\brdrs\clbrdrt\brdrw10\brdrs\clbrdrr\brdrw10\brdrs\clbrdrb\brdrw10\brdrs "
        strValoriMultipli = strValoriMultipli & "\cellx10040\pard\intbl\nowidctlpar\ri500\qc\b\fs24 Progetto\cell Sostituito\cell Subentrante\b0\cell\row\pard"
        'ciclo il risultato della query e costruisco la stringa con i valori multipli
        While dtrLeggiDati.Read
            strValoriMultipli = strValoriMultipli & "\trowd\trgaph70\trleft-70\trbrdrl\brdrs\brdrw10 "
            strValoriMultipli = strValoriMultipli & "\trbrdrt\brdrs\brdrw10 \trbrdrr\brdrs\brdrw10 "
            strValoriMultipli = strValoriMultipli & "\trbrdrb\brdrs\brdrw10 "
            strValoriMultipli = strValoriMultipli & "\trpaddl70\trpaddr70\trpaddfl3\trpaddfr3"
            strValoriMultipli = strValoriMultipli & "\clbrdrl\brdrw10\brdrs\clbrdrt\brdrw10\brdrs\clbrdrr\brdrw10\brdrs\clbrdrb\brdrw10\brdrs "
            strValoriMultipli = strValoriMultipli & "\cellx4000\clbrdrl\brdrw10\brdrs\clbrdrt\brdrw10\brdrs\clbrdrr\brdrw10\brdrs\clbrdrb\brdrw10\brdrs "
            strValoriMultipli = strValoriMultipli & "\cellx7000\clbrdrl\brdrw10\brdrs\clbrdrt\brdrw10\brdrs\clbrdrr\brdrw10\brdrs\clbrdrb\brdrw10\brdrs "
            strValoriMultipli = strValoriMultipli & "\cellx10040\pard\intbl\nowidctlpar\ri500\qj\fs22 " & dtrLeggiDati("Progetto") & "\cell " & dtrLeggiDati("Sostituito") & "\cell " & dtrLeggiDati("Subentrante") & "\cell\row\pard"
        End While

        strValoriMultipli = strValoriMultipli & "\par"

        'chiudo il datareader
        If Not dtrLeggiDati Is Nothing Then
            dtrLeggiDati.Close()
            dtrLeggiDati = Nothing
        End If


        'passo la stringa con i valori multipli alla funzione per il valore di ritorno
        PROG_SostituzioniApprovate = strValoriMultipli

        'ritorno la stringa
        Return PROG_SostituzioniApprovate

    End Function

    Function PROG_SostituzioniRespinte(ByVal IdEnte As Integer, ByVal IdIstanzaSostituzioneOLP As Integer) As String
        Dim strsql As String
        Dim strValoriMultipli As String = ""
        Dim dtrLeggiDati As SqlClient.SqlDataReader
        Dim num As Integer
        Dim punteggio As String
        Dim ciclovirgola As Integer
        Dim AppoTitolo As String

        'DATATABLE SOSTITUZIONI -- VW_EDITOR_ELENCOSOSTITUZIONIOLP da mettere!!!!
        strsql = "SELECT progetto + ' - ' + sede as progetto, sostituito, subentrante, MotivazioneRifiuto from VW_EDITOR_ELENCOSOSTITUZIONIOLP where Stato = 'Respinta' and IdIstanzaSostituzioneOLP=" & IdIstanzaSostituzioneOLP & " order by Progetto, Sede"
        dtrLeggiDati = ClsServer.CreaDatareader(strsql, HttpContext.Current.Session("conn"))

        strValoriMultipli = strValoriMultipli & "\trowd\trgaph70\trleft-70\trbrdrl\brdrs\brdrw10 "
        strValoriMultipli = strValoriMultipli & "\trbrdrt\brdrs\brdrw10 \trbrdrr\brdrs\brdrw10 "
        strValoriMultipli = strValoriMultipli & "\trbrdrb\brdrs\brdrw10 "
        strValoriMultipli = strValoriMultipli & "\trpaddl70\trpaddr70\trpaddfl3\trpaddfr3"
        strValoriMultipli = strValoriMultipli & "\clbrdrl\brdrw10\brdrs\clbrdrt\brdrw10\brdrs\clbrdrr\brdrw10\brdrs\clbrdrb\brdrw10\brdrs "
        strValoriMultipli = strValoriMultipli & "\cellx2127\clbrdrl\brdrw10\brdrs\clbrdrt\brdrw10\brdrs\clbrdrr\brdrw10\brdrs\clbrdrb\brdrw10\brdrs "
        strValoriMultipli = strValoriMultipli & "\cellx5103\clbrdrl\brdrw10\brdrs\clbrdrt\brdrw10\brdrs\clbrdrr\brdrw10\brdrs\clbrdrb\brdrw10\brdrs "
        strValoriMultipli = strValoriMultipli & "\cellx8080\clbrdrl\brdrw10\brdrs\clbrdrt\brdrw10\brdrs\clbrdrr\brdrw10\brdrs\clbrdrb\brdrw10\brdrs "
        strValoriMultipli = strValoriMultipli & "\cellx10040\pard\intbl\nowidctlpar\ri500\qc\b\fs24 Progetto\cell Sostituito\cell Subentrante\cell Motivazione\b0\cell\row\pard"
        'ciclo il risultato della query e costruisco la stringa con i valori multipli
        While dtrLeggiDati.Read
            strValoriMultipli = strValoriMultipli & "\trowd\trgaph70\trleft-70\trbrdrl\brdrs\brdrw10 "
            strValoriMultipli = strValoriMultipli & "\trbrdrt\brdrs\brdrw10 \trbrdrr\brdrs\brdrw10 "
            strValoriMultipli = strValoriMultipli & "\trbrdrb\brdrs\brdrw10 "
            strValoriMultipli = strValoriMultipli & "\trpaddl70\trpaddr70\trpaddfl3\trpaddfr3"
            strValoriMultipli = strValoriMultipli & "\clbrdrl\brdrw10\brdrs\clbrdrt\brdrw10\brdrs\clbrdrr\brdrw10\brdrs\clbrdrb\brdrw10\brdrs "
            strValoriMultipli = strValoriMultipli & "\cellx2127\clbrdrl\brdrw10\brdrs\clbrdrt\brdrw10\brdrs\clbrdrr\brdrw10\brdrs\clbrdrb\brdrw10\brdrs "
            strValoriMultipli = strValoriMultipli & "\cellx5103\clbrdrl\brdrw10\brdrs\clbrdrt\brdrw10\brdrs\clbrdrr\brdrw10\brdrs\clbrdrb\brdrw10\brdrs "
            strValoriMultipli = strValoriMultipli & "\cellx8080\clbrdrl\brdrw10\brdrs\clbrdrt\brdrw10\brdrs\clbrdrr\brdrw10\brdrs\clbrdrb\brdrw10\brdrs "
            strValoriMultipli = strValoriMultipli & "\cellx10040\pard\intbl\nowidctlpar\ri500\qj\fs22 " & dtrLeggiDati("Progetto") & "\cell " & dtrLeggiDati("Sostituito") & "\cell " & dtrLeggiDati("Subentrante") & "\cell " & dtrLeggiDati("MotivazioneRifiuto") & "\cell\row\pard"
        End While

        strValoriMultipli = strValoriMultipli & "\par"

        'chiudo il datareader
        If Not dtrLeggiDati Is Nothing Then
            dtrLeggiDati.Close()
            dtrLeggiDati = Nothing
        End If


        'passo la stringa con i valori multipli alla funzione per il valore di ritorno
        PROG_SostituzioniRespinte = strValoriMultipli

        'ritorno la stringa
        Return PROG_SostituzioniRespinte

    End Function
    Function VOL_ElencoProgetti(ByVal IdEnte As Integer, ByVal IdBando As Integer, ByVal dataavvioData As String) As String
        Dim strsql As String
        Dim strAppoProgramma As String = ""
        Dim strValoriMultipli As String = ""
        Dim dtrLeggiDati As SqlClient.SqlDataReader

        'chiudo il datareader
        If Not dtrLeggiDati Is Nothing Then
            dtrLeggiDati.Close()
            dtrLeggiDati = Nothing
        End If

        strsql = "Select distinct isnull(d.Titolo + ' - ' + d.codiceprogramma,'') as TitoloProgramma, isnull(a.Titolo,'') as Titolo, "
        strsql = strsql & "isnull(case len(day(a.DataInizioAttività)) when 1 then '0' + convert(varchar(20),day(a.DataInizioAttività)) else convert(varchar(20),day(a.DataInizioAttività))  end + '/' + (case len(month(a.DataInizioAttività)) when 1 then '0' + convert(varchar(20),month(a.DataInizioAttività)) else convert(varchar(20),month(a.DataInizioAttività))  end + '/' + Convert(varchar(20), Year(a.DataInizioAttività))),'xx/xx/xxxx') as DataInizioPrevista, "
        strsql = strsql & "isnull(a.CodiceEnte, 'Nessun Codice') as CodiceEnte "
        strsql = strsql & "from attività as a "
        strsql = strsql & "inner join bandiattività as ba on ba.idbandoattività = a.idbandoattività "
        strsql = strsql & "inner join attivitàsediassegnazione as b on a.idattività=b.idattività "
        strsql = strsql & "inner join enti as c on a.identepresentante=c.idente "
        strsql = strsql & "left join programmi as d on a.idprogramma=d.idprogramma "
        strsql = strsql & "where ba.idbando = " & IdBando & " and b.statograduatoria=3  and a.IdEntePresentante=" & IdEnte

        If dataavvioData <> "" Then
            strsql = strsql & " and a.DataInizioAttività ='" & dataavvioData & "'"

        End If
        strsql = strsql & " order by isnull(d.Titolo + ' - ' + d.codiceprogramma,''), isnull(a.Titolo,'')"
        'eseguo la query e passo il risultato al datareader
        dtrLeggiDati = ClsServer.CreaDatareader(strsql, HttpContext.Current.Session("conn"))

        'ciclo il risultato della query e costruisco la stringa con i valori multipli
        While dtrLeggiDati.Read
            If strAppoProgramma <> dtrLeggiDati("TitoloProgramma") Then
                strAppoProgramma = dtrLeggiDati("TitoloProgramma")
                strValoriMultipli = strValoriMultipli & "\par\b\fs24 " & strAppoProgramma & "\par\b0 "
                strValoriMultipli = strValoriMultipli & "\trowd\trgaph70\trleft-70\trbrdrl\brdrs\brdrw10 "
                strValoriMultipli = strValoriMultipli & "\trbrdrt\brdrs\brdrw10 \trbrdrr\brdrs\brdrw10 "
                strValoriMultipli = strValoriMultipli & "\trbrdrb\brdrs\brdrw10 "
                strValoriMultipli = strValoriMultipli & "\trpaddl70\trpaddr70\trpaddfl3\trpaddfr3"
                strValoriMultipli = strValoriMultipli & "\clbrdrl\brdrw10\brdrs\clbrdrt\brdrw10\brdrs\clbrdrr\brdrw10\brdrs\clbrdrb\brdrw10\brdrs "
                strValoriMultipli = strValoriMultipli & "\cellx4500\clbrdrl\brdrw10\brdrs\clbrdrt\brdrw10\brdrs\clbrdrr\brdrw10\brdrs\clbrdrb\brdrw10\brdrs "
                strValoriMultipli = strValoriMultipli & "\cellx8200\clbrdrl\brdrw10\brdrs\clbrdrt\brdrw10\brdrs\clbrdrr\brdrw10\brdrs\clbrdrb\brdrw10\brdrs "
                strValoriMultipli = strValoriMultipli & "\cellx10040\pard\intbl\nowidctlpar\ri500\qc\b\fs24 Titolo progetto\cell Codice progetto\cell Data Avvio\b0\cell\row\pard"

            End If
            strValoriMultipli = strValoriMultipli & "\trowd\trgaph70\trleft-70\trbrdrl\brdrs\brdrw10 "
            strValoriMultipli = strValoriMultipli & "\trbrdrt\brdrs\brdrw10 \trbrdrr\brdrs\brdrw10 "
            strValoriMultipli = strValoriMultipli & "\trbrdrb\brdrs\brdrw10 "
            strValoriMultipli = strValoriMultipli & "\trpaddl70\trpaddr70\trpaddfl3\trpaddfr3"
            strValoriMultipli = strValoriMultipli & "\clbrdrl\brdrw10\brdrs\clbrdrt\brdrw10\brdrs\clbrdrr\brdrw10\brdrs\clbrdrb\brdrw10\brdrs "
            strValoriMultipli = strValoriMultipli & "\cellx4500\clbrdrl\brdrw10\brdrs\clbrdrt\brdrw10\brdrs\clbrdrr\brdrw10\brdrs\clbrdrb\brdrw10\brdrs "
            strValoriMultipli = strValoriMultipli & "\cellx8200\clbrdrl\brdrw10\brdrs\clbrdrt\brdrw10\brdrs\clbrdrr\brdrw10\brdrs\clbrdrb\brdrw10\brdrs "
            strValoriMultipli = strValoriMultipli & "\cellx10040\pard\intbl\nowidctlpar\ri500\qj\fs22 " & dtrLeggiDati("Titolo") & "\cell " & dtrLeggiDati("CodiceEnte") & "\cell " & dtrLeggiDati("DatainizioPrevista") & "\cell\row\pard"
        End While

        strValoriMultipli = strValoriMultipli & "\par"

        'chiudo il datareader
        If Not dtrLeggiDati Is Nothing Then
            dtrLeggiDati.Close()
            dtrLeggiDati = Nothing
        End If

        'passo la stringa con i valori multipli alla funzione per il valore di ritorno
        VOL_ElencoProgetti = strValoriMultipli

        'ritorno la stringa
        Return VOL_ElencoProgetti

    End Function

    Function VOL_ElencoProgetti_Programma(ByVal IdProgramma As Integer, ByVal dataavvioData As String) As String
        Dim strsql As String
        Dim strAppoEnteProgetto As String = ""
        Dim strValoriMultipli As String = ""
        Dim dtrLeggiDati As SqlClient.SqlDataReader

        'chiudo il datareader
        If Not dtrLeggiDati Is Nothing Then
            dtrLeggiDati.Close()
            dtrLeggiDati = Nothing
        End If

        strsql = "Select distinct isnull(c.CodiceRegione + ' - ' + c.Denominazione,'') as EnteProgetto, isnull(a.Titolo,'') as Titolo, "
        strsql = strsql & "isnull(case len(day(a.DataInizioAttività)) when 1 then '0' + convert(varchar(20),day(a.DataInizioAttività)) else convert(varchar(20),day(a.DataInizioAttività))  end + '/' + (case len(month(a.DataInizioAttività)) when 1 then '0' + convert(varchar(20),month(a.DataInizioAttività)) else convert(varchar(20),month(a.DataInizioAttività))  end + '/' + Convert(varchar(20), Year(a.DataInizioAttività))),'xx/xx/xxxx') as DataInizioPrevista, "
        strsql = strsql & "isnull(a.CodiceEnte, 'Nessun Codice') as CodiceEnte "
        strsql = strsql & "from programmi inner join  attività as a on programmi.idprogramma = a.idprogramma "
        strsql = strsql & "inner join bandiattività as ba on ba.idbandoattività = a.idbandoattività "
        strsql = strsql & "inner join attivitàsediassegnazione as b on a.idattività=b.idattività "
        strsql = strsql & "inner join enti as c on a.identepresentante=c.idente "
        strsql = strsql & "where programmi.idprogramma = " & IdProgramma & " and b.statograduatoria=3  "

        If dataavvioData <> "" Then
            strsql = strsql & " and a.DataInizioAttività ='" & dataavvioData & "'"

        End If
        strsql = strsql & " order by isnull(c.CodiceRegione + ' - ' + c.Denominazione,''), isnull(a.Titolo,'')"
        'eseguo la query e passo il risultato al datareader
        dtrLeggiDati = ClsServer.CreaDatareader(strsql, HttpContext.Current.Session("conn"))

        'ciclo il risultato della query e costruisco la stringa con i valori multipli
        While dtrLeggiDati.Read
            If strAppoEnteProgetto <> dtrLeggiDati("EnteProgetto") Then
                strAppoEnteProgetto = dtrLeggiDati("EnteProgetto")
                strValoriMultipli = strValoriMultipli & "\par\b\fs24 " & strAppoEnteProgetto & "\par\b0 "
                strValoriMultipli = strValoriMultipli & "\trowd\trgaph70\trleft-70\trbrdrl\brdrs\brdrw10 "
                strValoriMultipli = strValoriMultipli & "\trbrdrt\brdrs\brdrw10 \trbrdrr\brdrs\brdrw10 "
                strValoriMultipli = strValoriMultipli & "\trbrdrb\brdrs\brdrw10 "
                strValoriMultipli = strValoriMultipli & "\trpaddl70\trpaddr70\trpaddfl3\trpaddfr3"
                strValoriMultipli = strValoriMultipli & "\clbrdrl\brdrw10\brdrs\clbrdrt\brdrw10\brdrs\clbrdrr\brdrw10\brdrs\clbrdrb\brdrw10\brdrs "
                strValoriMultipli = strValoriMultipli & "\cellx4500\clbrdrl\brdrw10\brdrs\clbrdrt\brdrw10\brdrs\clbrdrr\brdrw10\brdrs\clbrdrb\brdrw10\brdrs "
                strValoriMultipli = strValoriMultipli & "\cellx8200\clbrdrl\brdrw10\brdrs\clbrdrt\brdrw10\brdrs\clbrdrr\brdrw10\brdrs\clbrdrb\brdrw10\brdrs "
                strValoriMultipli = strValoriMultipli & "\cellx10040\pard\intbl\nowidctlpar\ri500\qc\b\fs24 Titolo progetto\cell Codice progetto\cell Data Avvio\b0\cell\row\pard"

            End If
            strValoriMultipli = strValoriMultipli & "\trowd\trgaph70\trleft-70\trbrdrl\brdrs\brdrw10 "
            strValoriMultipli = strValoriMultipli & "\trbrdrt\brdrs\brdrw10 \trbrdrr\brdrs\brdrw10 "
            strValoriMultipli = strValoriMultipli & "\trbrdrb\brdrs\brdrw10 "
            strValoriMultipli = strValoriMultipli & "\trpaddl70\trpaddr70\trpaddfl3\trpaddfr3"
            strValoriMultipli = strValoriMultipli & "\clbrdrl\brdrw10\brdrs\clbrdrt\brdrw10\brdrs\clbrdrr\brdrw10\brdrs\clbrdrb\brdrw10\brdrs "
            strValoriMultipli = strValoriMultipli & "\cellx4500\clbrdrl\brdrw10\brdrs\clbrdrt\brdrw10\brdrs\clbrdrr\brdrw10\brdrs\clbrdrb\brdrw10\brdrs "
            strValoriMultipli = strValoriMultipli & "\cellx8200\clbrdrl\brdrw10\brdrs\clbrdrt\brdrw10\brdrs\clbrdrr\brdrw10\brdrs\clbrdrb\brdrw10\brdrs "
            strValoriMultipli = strValoriMultipli & "\cellx10040\pard\intbl\nowidctlpar\ri500\qj\fs22 " & dtrLeggiDati("Titolo") & "\cell " & dtrLeggiDati("CodiceEnte") & "\cell " & dtrLeggiDati("DatainizioPrevista") & "\cell\row\pard"
        End While

        strValoriMultipli = strValoriMultipli & "\par"

        'chiudo il datareader
        If Not dtrLeggiDati Is Nothing Then
            dtrLeggiDati.Close()
            dtrLeggiDati = Nothing
        End If

        'passo la stringa con i valori multipli alla funzione per il valore di ritorno
        VOL_ElencoProgetti_Programma = strValoriMultipli

        'ritorno la stringa
        Return VOL_ElencoProgetti_Programma

    End Function

    Function VOL_ElencoVolontariEsclusi(ByVal IdEnte As Integer, ByVal IdBando As Integer, ByVal dataavvioData As String) As String
        Dim strsql As String
        Dim strValoriMultipli As String = ""
        Dim dtrLeggiDati As SqlClient.SqlDataReader

        'chiudo il datareader
        If Not dtrLeggiDati Is Nothing Then
            dtrLeggiDati.Close()
            dtrLeggiDati = Nothing
        End If

        strsql = "Select distinct isnull(a.Titolo,'') as Titolo, "
        strsql = strsql & "isnull(case len(day(a.DataInizioAttività)) when 1 then '0' + convert(varchar(20),day(a.DataInizioAttività)) else convert(varchar(20),day(a.DataInizioAttività))  end + '/' + (case len(month(a.DataInizioAttività)) when 1 then '0' + convert(varchar(20),month(a.DataInizioAttività)) else convert(varchar(20),month(a.DataInizioAttività))  end + '/' + Convert(varchar(20), Year(a.DataInizioAttività))),'xx/xx/xxxx') as DataInizioPrevista, "
        strsql = strsql & "isnull(a.CodiceEnte, 'Nessun Codice') as CodiceEnte,vincoli.Vincolo, isnull(vincoli.descrizione,'') as Descrizione, entità.Cognome, entità.Nome, FlagEntità.Valore , isnull(flagEntità.notaStorico,'') as Note "
        strsql = strsql & "from attività as a "
        strsql = strsql & "inner join bandiattività as ba on ba.idbandoattività = a.idbandoattività "
        strsql = strsql & "inner join attivitàsediassegnazione as b on a.idattività=b.idattività "
        strsql = strsql & "inner join enti as c on a.identepresentante=c.idente "
        strsql = strsql & "INNER JOIN GraduatorieEntità ON b.IDAttivitàSedeAssegnazione = GraduatorieEntità.IdAttivitàSedeAssegnazione "
        strsql = strsql & "INNER JOIN entità ON GraduatorieEntità.IdEntità = entità.IDEntità "
        strsql = strsql & "INNER JOIN FlagEntità ON entità.IDEntità = FlagEntità.IdEntità "
        strsql = strsql & "INNER JOIN vincoli ON FlagEntità.IdVincolo = vincoli.IDVincolo "
        strsql = strsql & "where ba.idbando = " & IdBando & "  AND FlagEntità.Valore = 0  and b.statograduatoria=3 and a.IdEntePresentante=" & IdEnte

        If dataavvioData <> "" Then
            strsql = strsql & " and a.DataInizioAttività ='" & dataavvioData & "'"
        End If

        'eseguo la query e passo il risultato al datareader
        dtrLeggiDati = ClsServer.CreaDatareader(strsql, HttpContext.Current.Session("conn"))

        'ciclo il risultato della query e costruisco la stringa con i valori multipli
        While dtrLeggiDati.Read
            strValoriMultipli = strValoriMultipli & "\trowd\trgaph70\trleft-70\trbrdrl\brdrs\brdrw10 "
            strValoriMultipli = strValoriMultipli & "\trbrdrt\brdrs\brdrw10 \trbrdrr\brdrs\brdrw10 "
            strValoriMultipli = strValoriMultipli & "\trbrdrb\brdrs\brdrw10 "
            strValoriMultipli = strValoriMultipli & "\trpaddl70\trpaddr70\trpaddfl3\trpaddfr3"
            strValoriMultipli = strValoriMultipli & "\clbrdrl\brdrw10\brdrs\clbrdrt\brdrw10\brdrs\clbrdrr\brdrw10\brdrs\clbrdrb\brdrw10\brdrs "
            strValoriMultipli = strValoriMultipli & "\cellx4000\clbrdrl\brdrw10\brdrs\clbrdrt\brdrw10\brdrs\clbrdrr\brdrw10\brdrs\clbrdrb\brdrw10\brdrs "
            strValoriMultipli = strValoriMultipli & "\cellx10040\pard\intbl\nowidctlpar\ri500\qj\b " & dtrLeggiDati("Cognome") & "  " & dtrLeggiDati("Nome") & "\cell " & dtrLeggiDati("Descrizione") & " " & dtrLeggiDati("Note") & "\b0\cell\row\pard"
        End While

        strValoriMultipli = strValoriMultipli & "\par" & vbCrLf

        'chiudo il datareader
        If Not dtrLeggiDati Is Nothing Then
            dtrLeggiDati.Close()
            dtrLeggiDati = Nothing
        End If

        'passo la stringa con i valori multipli alla funzione per il valore di ritorno
        VOL_ElencoVolontariEsclusi = strValoriMultipli

        'ritorno la stringa
        Return VOL_ElencoVolontariEsclusi

    End Function

    Function VOL_ElencoVolontariEsclusi_Programma(ByVal IdProgramma As Integer, ByVal dataavvioData As String) As String
        Dim strsql As String
        Dim strValoriMultipli As String = ""
        Dim dtrLeggiDati As SqlClient.SqlDataReader

        'chiudo il datareader
        If Not dtrLeggiDati Is Nothing Then
            dtrLeggiDati.Close()
            dtrLeggiDati = Nothing
        End If

        strsql = "Select distinct isnull(a.Titolo,'') as Titolo, "
        strsql = strsql & "isnull(case len(day(a.DataInizioAttività)) when 1 then '0' + convert(varchar(20),day(a.DataInizioAttività)) else convert(varchar(20),day(a.DataInizioAttività))  end + '/' + (case len(month(a.DataInizioAttività)) when 1 then '0' + convert(varchar(20),month(a.DataInizioAttività)) else convert(varchar(20),month(a.DataInizioAttività))  end + '/' + Convert(varchar(20), Year(a.DataInizioAttività))),'xx/xx/xxxx') as DataInizioPrevista, "
        strsql = strsql & "isnull(a.CodiceEnte, 'Nessun Codice') as CodiceEnte,vincoli.Vincolo, isnull(vincoli.descrizione,'') as Descrizione, entità.Cognome, entità.Nome, FlagEntità.Valore , isnull(flagEntità.notaStorico,'') as Note "
        strsql = strsql & "from programmi inner join  attività as a on programmi.idprogramma = a.idprogramma "
        strsql = strsql & "inner join bandiattività as ba on ba.idbandoattività = a.idbandoattività "
        strsql = strsql & "inner join attivitàsediassegnazione as b on a.idattività=b.idattività "
        strsql = strsql & "inner join enti as c on a.identepresentante=c.idente "
        strsql = strsql & "INNER JOIN GraduatorieEntità ON b.IDAttivitàSedeAssegnazione = GraduatorieEntità.IdAttivitàSedeAssegnazione "
        strsql = strsql & "INNER JOIN entità ON GraduatorieEntità.IdEntità = entità.IDEntità "
        strsql = strsql & "INNER JOIN FlagEntità ON entità.IDEntità = FlagEntità.IdEntità "
        strsql = strsql & "INNER JOIN vincoli ON FlagEntità.IdVincolo = vincoli.IDVincolo "
        strsql = strsql & "where programmi.idprogramma = " & IdProgramma & "  AND FlagEntità.Valore = 0  and b.statograduatoria=3 "

        If dataavvioData <> "" Then
            strsql = strsql & " and a.DataInizioAttività ='" & dataavvioData & "'"
        End If
        strsql = strsql & " order by isnull(a.CodiceEnte, 'Nessun Codice'), entità.Cognome, entità.Nome"
        'eseguo la query e passo il risultato al datareader
        dtrLeggiDati = ClsServer.CreaDatareader(strsql, HttpContext.Current.Session("conn"))

        If dtrLeggiDati.HasRows Then
            strValoriMultipli = strValoriMultipli & "\trowd\trgaph70\trleft-70\trbrdrl\brdrs\brdrw10 "
            strValoriMultipli = strValoriMultipli & "\trbrdrt\brdrs\brdrw10 \trbrdrr\brdrs\brdrw10 "
            strValoriMultipli = strValoriMultipli & "\trbrdrb\brdrs\brdrw10 "
            strValoriMultipli = strValoriMultipli & "\trpaddl70\trpaddr70\trpaddfl3\trpaddfr3"
            strValoriMultipli = strValoriMultipli & "\clbrdrl\brdrw10\brdrs\clbrdrt\brdrw10\brdrs\clbrdrr\brdrw10\brdrs\clbrdrb\brdrw10\brdrs "
            strValoriMultipli = strValoriMultipli & "\cellx3400\clbrdrl\brdrw10\brdrs\clbrdrt\brdrw10\brdrs\clbrdrr\brdrw10\brdrs\clbrdrb\brdrw10\brdrs "
            strValoriMultipli = strValoriMultipli & "\cellx6000\clbrdrl\brdrw10\brdrs\clbrdrt\brdrw10\brdrs\clbrdrr\brdrw10\brdrs\clbrdrb\brdrw10\brdrs "
            strValoriMultipli = strValoriMultipli & "\cellx10040\pard\intbl\nowidctlpar\ri500\qc\b\fs24 Codice progetto\cell Nominativo\cell Motivazione\b0\cell\row\pard"
        End If
 
        'ciclo il risultato della query e costruisco la stringa con i valori multipli
        While dtrLeggiDati.Read
            strValoriMultipli = strValoriMultipli & "\trowd\trgaph70\trleft-70\trbrdrl\brdrs\brdrw10 "
            strValoriMultipli = strValoriMultipli & "\trbrdrt\brdrs\brdrw10 \trbrdrr\brdrs\brdrw10 "
            strValoriMultipli = strValoriMultipli & "\trbrdrb\brdrs\brdrw10 "
            strValoriMultipli = strValoriMultipli & "\trpaddl70\trpaddr70\trpaddfl3\trpaddfr3"
            strValoriMultipli = strValoriMultipli & "\clbrdrl\brdrw10\brdrs\clbrdrt\brdrw10\brdrs\clbrdrr\brdrw10\brdrs\clbrdrb\brdrw10\brdrs "
            strValoriMultipli = strValoriMultipli & "\cellx3400\clbrdrl\brdrw10\brdrs\clbrdrt\brdrw10\brdrs\clbrdrr\brdrw10\brdrs\clbrdrb\brdrw10\brdrs "
            strValoriMultipli = strValoriMultipli & "\cellx6000\clbrdrl\brdrw10\brdrs\clbrdrt\brdrw10\brdrs\clbrdrr\brdrw10\brdrs\clbrdrb\brdrw10\brdrs "
            strValoriMultipli = strValoriMultipli & "\cellx10040\pard\intbl\nowidctlpar\ri500\qj\fs22 " & dtrLeggiDati("CodiceEnte") & "\cell " & dtrLeggiDati("Cognome") & "  " & dtrLeggiDati("Nome") & "\cell " & dtrLeggiDati("Descrizione") & " " & dtrLeggiDati("Note") & "\cell\row\pard"

            'strValoriMultipli = strValoriMultipli & "\trowd\trgaph70\trleft-70\trbrdrl\brdrs\brdrw10 "
            'strValoriMultipli = strValoriMultipli & "\trbrdrt\brdrs\brdrw10 \trbrdrr\brdrs\brdrw10 "
            'strValoriMultipli = strValoriMultipli & "\trbrdrb\brdrs\brdrw10 "
            'strValoriMultipli = strValoriMultipli & "\trpaddl70\trpaddr70\trpaddfl3\trpaddfr3"
            'strValoriMultipli = strValoriMultipli & "\clbrdrl\brdrw10\brdrs\clbrdrt\brdrw10\brdrs\clbrdrr\brdrw10\brdrs\clbrdrb\brdrw10\brdrs "
            'strValoriMultipli = strValoriMultipli & "\cellx4000\clbrdrl\brdrw10\brdrs\clbrdrt\brdrw10\brdrs\clbrdrr\brdrw10\brdrs\clbrdrb\brdrw10\brdrs "
            'strValoriMultipli = strValoriMultipli & "\cellx10040\pard\intbl\nowidctlpar\ri500\qj\b " & dtrLeggiDati("Cognome") & "  " & dtrLeggiDati("Nome") & "\cell " & dtrLeggiDati("Descrizione") & " " & dtrLeggiDati("Note") & "\b0\cell\row\pard"
        End While

        strValoriMultipli = strValoriMultipli & "\par" & vbCrLf

        'chiudo il datareader
        If Not dtrLeggiDati Is Nothing Then
            dtrLeggiDati.Close()
            dtrLeggiDati = Nothing
        End If

        'passo la stringa con i valori multipli alla funzione per il valore di ritorno
        VOL_ElencoVolontariEsclusi_Programma = strValoriMultipli

        'ritorno la stringa
        Return VOL_ElencoVolontariEsclusi_Programma

    End Function

    Public Property VOL_letteraapprovazionegraduatoria(ByVal intIdEnte As Integer, ByVal IdBando As Integer, ByVal strUserName As String, ByVal intIdCompetenza As Integer, ByVal sqlLocalConn As SqlClient.SqlConnection, ByVal dataavvio As String) As String
        Get
            Dim dtR As DataRow
            Dim dtT As New DataTable

            dtT.Columns.Add(New DataColumn("PARAMETRO", GetType(String)))
            dtT.Columns.Add(New DataColumn("VALORE", GetType(String)))

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDENTE"
            dtR(1) = intIdEnte

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDBANDO"
            dtR(1) = IdBando

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)

            'SANDOKAN
            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "DATAAVVIO"
            dtR(1) = dataavvio

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)


            PathDocumento = EseguiGenerazioneModello(44, strUserName, intIdCompetenza, dtT, sqlLocalConn)

            Return PathDocumento
        End Get
        Set(ByVal Value As String)
            PathDocumento = Value
        End Set
    End Property

    Public Property VOL_letteraapprovazionegraduatoriaCopiaReg(ByVal intIdEnte As Integer, ByVal IdBando As Integer, ByVal strUserName As String, ByVal intIdCompetenza As Integer, ByVal sqlLocalConn As SqlClient.SqlConnection) As String
        Get
            Dim dtR As DataRow
            Dim dtT As New DataTable

            dtT.Columns.Add(New DataColumn("PARAMETRO", GetType(String)))
            dtT.Columns.Add(New DataColumn("VALORE", GetType(String)))

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDENTE"
            dtR(1) = intIdEnte

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDBANDO"
            dtR(1) = IdBando

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)

            PathDocumento = EseguiGenerazioneModello(45, strUserName, intIdCompetenza, dtT, sqlLocalConn)

            Return PathDocumento
        End Get
        Set(ByVal Value As String)
            PathDocumento = Value
        End Set
    End Property

    Public Property VOL_letteraapprovazionegraduatoriaestero(ByVal intIdEnte As Integer, ByVal IdBando As Integer, ByVal strUserName As String, ByVal intIdCompetenza As Integer, ByVal sqlLocalConn As SqlClient.SqlConnection, ByVal dataavvio As String) As String
        Get
            Dim dtR As DataRow
            Dim dtT As New DataTable

            dtT.Columns.Add(New DataColumn("PARAMETRO", GetType(String)))
            dtT.Columns.Add(New DataColumn("VALORE", GetType(String)))

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDENTE"
            dtR(1) = intIdEnte

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDBANDO"
            dtR(1) = IdBando

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)

            'SANDOKAN
            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "DATAAVVIO"
            dtR(1) = dataavvio

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)

            PathDocumento = EseguiGenerazioneModello(47, strUserName, intIdCompetenza, dtT, sqlLocalConn)

            Return PathDocumento
        End Get
        Set(ByVal Value As String)
            PathDocumento = Value
        End Set
    End Property

    Public Property VOL_letteraapprovazionegraduatoriaesteronazionale(ByVal intIdEnte As Integer, ByVal IdBando As Integer, ByVal strUserName As String, ByVal intIdCompetenza As Integer, ByVal sqlLocalConn As SqlClient.SqlConnection, ByVal dataavvio As String) As String
        Get
            Dim dtR As DataRow
            Dim dtT As New DataTable

            dtT.Columns.Add(New DataColumn("PARAMETRO", GetType(String)))
            dtT.Columns.Add(New DataColumn("VALORE", GetType(String)))

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDENTE"
            dtR(1) = intIdEnte

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDBANDO"
            dtR(1) = IdBando

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)

            'SANDOKAN
            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "DATAAVVIO"
            dtR(1) = dataavvio

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)

            PathDocumento = EseguiGenerazioneModello(48, strUserName, intIdCompetenza, dtT, sqlLocalConn)

            Return PathDocumento
        End Get
        Set(ByVal Value As String)
            PathDocumento = Value
        End Set
    End Property

    Public Property VOL_letteraAssegnazioneVolontariodopoapprovazionegraduatoriaS(ByVal intIdEnte As Integer, ByVal IdBando As Integer, ByVal strUserName As String, ByVal intIdCompetenza As Integer, ByVal sqlLocalConn As SqlClient.SqlConnection) As String
        Get
            Dim dtR As DataRow
            Dim dtT As New DataTable

            dtT.Columns.Add(New DataColumn("PARAMETRO", GetType(String)))
            dtT.Columns.Add(New DataColumn("VALORE", GetType(String)))

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDENTE"
            dtR(1) = intIdEnte

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDBANDO"
            dtR(1) = IdBando

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)

            PathDocumento = EseguiGenerazioneModello(51, strUserName, intIdCompetenza, dtT, sqlLocalConn)

            Return PathDocumento
        End Get
        Set(ByVal Value As String)
            PathDocumento = Value
        End Set
    End Property

    Public Property VOL_letteraAssegnazioneVolontariodopoapprovazionegraduatoriaM(ByVal intIdEnte As Integer, ByVal IdBando As Integer, ByVal strUserName As String, ByVal intIdCompetenza As Integer, ByVal sqlLocalConn As SqlClient.SqlConnection) As String
        Get
            Dim dtR As DataRow
            Dim dtT As New DataTable

            dtT.Columns.Add(New DataColumn("PARAMETRO", GetType(String)))
            dtT.Columns.Add(New DataColumn("VALORE", GetType(String)))

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDENTE"
            dtR(1) = intIdEnte

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDBANDO"
            dtR(1) = IdBando

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)

            PathDocumento = EseguiGenerazioneModello(52, strUserName, intIdCompetenza, dtT, sqlLocalConn)

            Return PathDocumento
        End Get
        Set(ByVal Value As String)
            PathDocumento = Value
        End Set
    End Property

    Public Property VOL_elencovolontariammessi(ByVal idAttivitaSedeAssegnazione As Integer, ByVal strUserName As String, ByVal intIdCompetenza As Integer, ByVal sqlLocalConn As SqlClient.SqlConnection) As String
        Get
            Dim dtR As DataRow
            Dim dtT As New DataTable

            dtT.Columns.Add(New DataColumn("PARAMETRO", GetType(String)))
            dtT.Columns.Add(New DataColumn("VALORE", GetType(String)))

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDAttivitàSedeAssegnazione"
            dtR(1) = idAttivitaSedeAssegnazione

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)

            PathDocumento = EseguiGenerazioneModello(53, strUserName, intIdCompetenza, dtT, sqlLocalConn)

            Return PathDocumento
        End Get
        Set(ByVal Value As String)
            PathDocumento = Value
        End Set
    End Property

    Function VOL_ElencoVolontariConcessi(ByVal IdAttivitaSedeAssegnazione As Integer) As String
        Dim strsql As String
        Dim strValoriMultipli As String = ""
        Dim dtrLeggiDati As SqlClient.SqlDataReader

        'chiudo il datareader
        If Not dtrLeggiDati Is Nothing Then
            dtrLeggiDati.Close()
            dtrLeggiDati = Nothing
        End If

        strsql = "Select DISTINCT entità.identità,isnull(entità.CodiceVolontario, 'Nessun Codice') as CodiceVolontario, "
        strsql = strsql & "isnull(entità.cognome, '') + ' ' +"
        strsql = strsql & "isnull(entità.nome, '')as nominativo, "
        strsql = strsql & "entità.Codicefiscale, "
        strsql = strsql & "isnull(case len(day(entità.datanascita)) when 1 then '0' + convert(varchar(20),day(entità.datanascita)) "
        strsql = strsql & "else convert(varchar(20),day(entità.datanascita))  end + '/' + "
        strsql = strsql & "(case len(month(entità.datanascita)) when 1 then '0' + convert(varchar(20),month(entità.datanascita)) "
        strsql = strsql & "else convert(varchar(20),month(entità.datanascita))  end + '/' + "
        strsql = strsql & "Convert(varchar(20), Year(entità.datanascita))),'') as datanascita, "
        strsql = strsql & "isnull(case len(day(entità.DataInizioServizio)) when 1 then '0' + convert(varchar(20),day(entità.DataInizioServizio)) "
        strsql = strsql & "else convert(varchar(20),day(entità.DataInizioServizio))  end + '/' + "
        strsql = strsql & "(case len(month(entità.DataInizioServizio)) when 1 then '0' + convert(varchar(20),month(entità.DataInizioServizio)) "
        strsql = strsql & "else convert(varchar(20),month(entità.DataInizioServizio))  end + '/' + "
        strsql = strsql & "Convert(varchar(20), Year(entità.DataInizioServizio))),'') as DataInizioServizio, "
        strsql = strsql & "isnull(comuni.denominazione, '') + '(' + isnull(provincie.provincia, '') + ')'as comune, "
        strsql = strsql & " isnull(convert(varchar,attivitàentisediattuazione.idEntesedeattuazione),'Nessun Codice') as CodiceSede, "
        strsql = strsql & " dbo.FN_VERIFICA_ABILITAZIONE_BANDO_CONTRATTO(ENTITà.identità) AS AbilitaBandoContratto,"
        strsql = strsql & " case bando.DOL when 1 then '' else  entità.Username end as Username, case bando.DOL when 1 then '' else  dbo.readpassword(entità.Password) end as Password  ,entità.Abilitato "
        strsql = strsql & " from entità "
        strsql = strsql & " INNER JOIN attivitàentità ON entità.IDEntità = attivitàentità.IDEntità "
        strsql = strsql & " INNER JOIN attivitàentisediattuazione"
        strsql = strsql & " ON attivitàentità.IDAttivitàEnteSedeAttuazione = attivitàentisediattuazione.IDAttivitàEnteSedeAttuazione "
        strsql = strsql & " inner join attività on attivitàentisediattuazione.IDAttività = attività.IDAttività "
        strsql = strsql & " inner join BandiAttività on attività.IDBandoAttività = BandiAttività.IdBandoAttività "
        strsql = strsql & " inner join bando on BandiAttività.IdBando = bando.IDBando "
        strsql = strsql & " inner join statientità on statientità.idstatoentità=entità.idstatoentità "
        strsql = strsql & "left join impVolontariLotus on entità.codicefiscale=impVolontariLotus.cf "
        strsql = strsql & "inner join graduatorieEntità on graduatorieEntità.identità=Entità.identità "
        strsql = strsql & "left join TipologiePosto on graduatorieEntità.idtipologiaposto=TipologiePosto.idtipologiaposto "
        strsql = strsql & "inner join comuni on comuni.idcomune=entità.idcomunenascita "
        strsql = strsql & "inner join provincie on (provincie.idprovincia=comuni.idprovincia) "
        strsql = strsql & "where(graduatorieEntità.idattivitàsedeassegnazione=" & IdAttivitaSedeAssegnazione & ") and graduatorieEntità.ammesso = 1 and statientità.inservizio=1 and attivitàentità.idstatoattivitàentità=1 "
        strsql = strsql & "order by nominativo"

        'eseguo la query e passo il risultato al datareader
        dtrLeggiDati = ClsServer.CreaDatareader(strsql, HttpContext.Current.Session("conn"))

        'ciclo il risultato della query e costruisco la stringa con i valori multipli
        While dtrLeggiDati.Read
            If dtrLeggiDati("AbilitaBandoContratto") = 0 Then
                strValoriMultipli = strValoriMultipli & dtrLeggiDati("CodiceVolontario") & " " & dtrLeggiDati("Nominativo") & " nato/a il " & dtrLeggiDati("datanascita") & " a " & dtrLeggiDati("comune") & " avvio il " & dtrLeggiDati("DataInizioServizio") & " Codice Sede " & dtrLeggiDati("CodiceSede") & "\par" & vbCrLf
            Else

                strValoriMultipli = strValoriMultipli & "\trowd\trgaph70\trleft-70\trbrdrl\brdrs\brdrw10 "
                strValoriMultipli = strValoriMultipli & "\trbrdrt\brdrs\brdrw10 \trbrdrr\brdrs\brdrw10 "
                strValoriMultipli = strValoriMultipli & "\trbrdrb\brdrs\brdrw10 "
                strValoriMultipli = strValoriMultipli & "\trpaddl70\trpaddr70\trpaddfl3\trpaddfr3"
                strValoriMultipli = strValoriMultipli & "\clbrdrl\brdrw10\brdrs\clbrdrt\brdrw10\brdrs\clbrdrr\brdrw10\brdrs\clbrdrb\brdrw10\brdrs "
                'strValoriMultipli = strValoriMultipli & "\cellx3791\clbrdrl\brdrw10\brdrs\clbrdrt\brdrw10\brdrs\clbrdrr\brdrw10\brdrs\clbrdrb\brdrw10\brdrs "
                strValoriMultipli = strValoriMultipli & "\cellx10080\pard\intbl\nowidctlpar\ri500\qc\par\b " & dtrLeggiDati("CodiceVolontario") & " " & dtrLeggiDati("Nominativo") & " \b0\par  nato/a il " & dtrLeggiDati("datanascita") & " a " & dtrLeggiDati("comune") & " avvio il " & dtrLeggiDati("DataInizioServizio") & " Codice Sede " & dtrLeggiDati("CodiceSede") & ". \b0\par\cell\row\pard"


                'RIMOSSA SEMPRE SEZIONE USER E PASSWORD SEGNALAZIONE ALFONSI 28/04/2021
                'If dtrLeggiDati("Abilitato") = 0 Then
                '    'strValoriMultipli = strValoriMultipli & "\b " & dtrLeggiDati("CodiceVolontario") & " " & dtrLeggiDati("Nominativo") & " \b0  nato/a il " & dtrLeggiDati("datanascita") & " a " & dtrLeggiDati("comune") & " avvio il " & dtrLeggiDati("DataInizioServizio") & " Codice Sede " & dtrLeggiDati("CodiceSede") & ".\par UTENZA WEB: \b " & dtrLeggiDati("Username") & " \b0 PASSWORD: \b " & dtrLeggiDati("Password") & "\b0\par" & vbCrLf


                '    'strValoriMultipli = strValoriMultipli & "\trowd\trgaph70\trleft-70\trbrdrl\brdrs\brdrw10 "
                '    'strValoriMultipli = strValoriMultipli & "\trbrdrt\brdrs\brdrw10 \trbrdrr\brdrs\brdrw10 "
                '    'strValoriMultipli = strValoriMultipli & "\trbrdrb\brdrs\brdrw10 "
                '    'strValoriMultipli = strValoriMultipli & "\trpaddl70\trpaddr70\trpaddfl3\trpaddfr3"
                '    'strValoriMultipli = strValoriMultipli & "\clbrdrl\brdrw10\brdrs\clbrdrt\brdrw10\brdrs\clbrdrr\brdrw10\brdrs\clbrdrb\brdrw10\brdrs "
                '    'strValoriMultipli = strValoriMultipli & "\cellx4000\clbrdrl\brdrw10\brdrs\clbrdrt\brdrw10\brdrs\clbrdrr\brdrw10\brdrs\clbrdrb\brdrw10\brdrs "
                '    'strValoriMultipli = strValoriMultipli & "\cellx10040\pard\intbl\nowidctlpar\ri500\qj\b " & dtrLeggiDati("CodiceVolontario") & "  " & dtrLeggiDati("Nominativo") & "\cell " & dtrLeggiDati("Descrizione") & " " & dtrLeggiDati("Note") & "\b0\cell\row\pard"



                '    strValoriMultipli = strValoriMultipli & "\trowd\trgaph70\trleft-70\trbrdrl\brdrs\brdrw10 "
                '    strValoriMultipli = strValoriMultipli & "\trbrdrt\brdrs\brdrw10 \trbrdrr\brdrs\brdrw10 "
                '    strValoriMultipli = strValoriMultipli & "\trbrdrb\brdrs\brdrw10 "
                '    strValoriMultipli = strValoriMultipli & "\trpaddl70\trpaddr70\trpaddfl3\trpaddfr3"
                '    strValoriMultipli = strValoriMultipli & "\clbrdrl\brdrw10\brdrs\clbrdrt\brdrw10\brdrs\clbrdrr\brdrw10\brdrs\clbrdrb\brdrw10\brdrs "
                '    'strValoriMultipli = strValoriMultipli & "\cellx3791\clbrdrl\brdrw10\brdrs\clbrdrt\brdrw10\brdrs\clbrdrr\brdrw10\brdrs\clbrdrb\brdrw10\brdrs "
                '    strValoriMultipli = strValoriMultipli & "\cellx10080\pard\intbl\nowidctlpar\ri500\qc\par\b " & dtrLeggiDati("CodiceVolontario") & " " & dtrLeggiDati("Nominativo") & " \b0\par  nato/a il " & dtrLeggiDati("datanascita") & " a " & dtrLeggiDati("comune") & " avvio il " & dtrLeggiDati("DataInizioServizio") & " Codice Sede " & dtrLeggiDati("CodiceSede") & ".\par\par  UTENZA WEB: \b " & dtrLeggiDati("Username") & " \b0 PASSWORD: \b " & dtrLeggiDati("Password") & "\b0\par\cell\row\pard"


                'Else
                '    'strValoriMultipli = strValoriMultipli & dtrLeggiDati("CodiceVolontario") & " " & dtrLeggiDati("Nominativo") & " nato/a il " & dtrLeggiDati("datanascita") & " a " & dtrLeggiDati("comune") & " avvio il " & dtrLeggiDati("DataInizioServizio") & " Codice Sede " & dtrLeggiDati("CodiceSede") & "\par" & vbCrLf

                '    strValoriMultipli = strValoriMultipli & "\trowd\trgaph70\trleft-70\trbrdrl\brdrs\brdrw10 "
                '    strValoriMultipli = strValoriMultipli & "\trbrdrt\brdrs\brdrw10 \trbrdrr\brdrs\brdrw10 "
                '    strValoriMultipli = strValoriMultipli & "\trbrdrb\brdrs\brdrw10 "
                '    strValoriMultipli = strValoriMultipli & "\trpaddl70\trpaddr70\trpaddfl3\trpaddfr3"
                '    strValoriMultipli = strValoriMultipli & "\clbrdrl\brdrw10\brdrs\clbrdrt\brdrw10\brdrs\clbrdrr\brdrw10\brdrs\clbrdrb\brdrw10\brdrs "
                '    'strValoriMultipli = strValoriMultipli & "\cellx3791\clbrdrl\brdrw10\brdrs\clbrdrt\brdrw10\brdrs\clbrdrr\brdrw10\brdrs\clbrdrb\brdrw10\brdrs "
                '    strValoriMultipli = strValoriMultipli & "\cellx10080\pard\intbl\nowidctlpar\ri500\qc\par\b " & dtrLeggiDati("CodiceVolontario") & " " & dtrLeggiDati("Nominativo") & " \b0\par  nato/a il " & dtrLeggiDati("datanascita") & " a " & dtrLeggiDati("comune") & " avvio il " & dtrLeggiDati("DataInizioServizio") & " Codice Sede " & dtrLeggiDati("CodiceSede") & ". \b0\par\cell\row\pard"

                'End If
            End If
        End While

        strValoriMultipli = strValoriMultipli & "\par" & vbCrLf

        'chiudo il datareader
        If Not dtrLeggiDati Is Nothing Then
            dtrLeggiDati.Close()
            dtrLeggiDati = Nothing
        End If

        'passo la stringa con i valori multipli alla funzione per il valore di ritorno
        VOL_ElencoVolontariConcessi = strValoriMultipli

        'ritorno la stringa
        Return VOL_ElencoVolontariConcessi

    End Function

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

            PathDocumento = EseguiGenerazioneModello(55, strUserName, intIdCompetenza, dtT, sqlLocalConn)

            Return PathDocumento
        End Get
        Set(ByVal Value As String)
            PathDocumento = Value
        End Set
    End Property


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

            PathDocumento = EseguiGenerazioneModello(58, strUserName, intIdCompetenza, dtT, sqlLocalConn)

            Return PathDocumento
        End Get
        Set(ByVal Value As String)
            PathDocumento = Value
        End Set
    End Property
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

            PathDocumento = EseguiGenerazioneModello(303, strUserName, intIdCompetenza, dtT, sqlLocalConn)

            Return PathDocumento
        End Get
        Set(ByVal Value As String)
            PathDocumento = Value
        End Set
    End Property
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

            PathDocumento = EseguiGenerazioneModello(264, strUserName, intIdCompetenza, dtT, sqlLocalConn)

            Return PathDocumento
        End Get
        Set(ByVal Value As String)
            PathDocumento = Value
        End Set
    End Property

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

            PathDocumento = EseguiGenerazioneModello(62, strUserName, intIdCompetenza, dtT, sqlLocalConn)

            Return PathDocumento
        End Get
        Set(ByVal Value As String)
            PathDocumento = Value
        End Set
    End Property

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

            PathDocumento = EseguiGenerazioneModello(63, strUserName, intIdCompetenza, dtT, sqlLocalConn)

            Return PathDocumento
        End Get
        Set(ByVal Value As String)
            PathDocumento = Value
        End Set
    End Property

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

            PathDocumento = EseguiGenerazioneModello(304, strUserName, intIdCompetenza, dtT, sqlLocalConn)

            Return PathDocumento
        End Get
        Set(ByVal Value As String)
            PathDocumento = Value
        End Set
    End Property

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

            PathDocumento = EseguiGenerazioneModello(266, strUserName, intIdCompetenza, dtT, sqlLocalConn)

            Return PathDocumento
        End Get
        Set(ByVal Value As String)
            PathDocumento = Value
        End Set
    End Property

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

            PathDocumento = EseguiGenerazioneModello(59, strUserName, intIdCompetenza, dtT, sqlLocalConn)

            Return PathDocumento
        End Get
        Set(ByVal Value As String)
            PathDocumento = Value
        End Set
    End Property

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

            PathDocumento = EseguiGenerazioneModello(60, strUserName, intIdCompetenza, dtT, sqlLocalConn)

            Return PathDocumento
        End Get
        Set(ByVal Value As String)
            PathDocumento = Value
        End Set
    End Property

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

            PathDocumento = EseguiGenerazioneModello(265, strUserName, intIdCompetenza, dtT, sqlLocalConn)

            Return PathDocumento
        End Get
        Set(ByVal Value As String)
            PathDocumento = Value
        End Set
    End Property

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

            PathDocumento = EseguiGenerazioneModello(64, strUserName, intIdCompetenza, dtT, sqlLocalConn)

            Return PathDocumento
        End Get
        Set(ByVal Value As String)
            PathDocumento = Value
        End Set
    End Property

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

            PathDocumento = EseguiGenerazioneModello(65, strUserName, intIdCompetenza, dtT, sqlLocalConn)

            Return PathDocumento
        End Get
        Set(ByVal Value As String)
            PathDocumento = Value
        End Set
    End Property

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

            PathDocumento = EseguiGenerazioneModello(267, strUserName, intIdCompetenza, dtT, sqlLocalConn)

            Return PathDocumento
        End Get
        Set(ByVal Value As String)
            PathDocumento = Value
        End Set
    End Property

    Public Property VOL_AttoAggiuntivoVolontari(ByVal IdVolontario As Integer, ByVal IdEnte As Integer, ByVal strUserName As String, ByVal intIdCompetenza As Integer, ByVal sqlLocalConn As SqlClient.SqlConnection) As String
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

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDENTE"
            dtR(1) = IdEnte

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)

            PathDocumento = EseguiGenerazioneModello(277, strUserName, intIdCompetenza, dtT, sqlLocalConn)

            Return PathDocumento
        End Get
        Set(ByVal Value As String)
            PathDocumento = Value
        End Set
    End Property

    Public Property VOL_duplicatoletteraAssegnazioneEnte(ByVal IdVolontario As Integer, ByVal IdEnte As Integer, ByVal strUserName As String, ByVal intIdCompetenza As Integer, ByVal sqlLocalConn As SqlClient.SqlConnection) As String
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

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDENTE"
            dtR(1) = IdEnte

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)

            PathDocumento = EseguiGenerazioneModello(66, strUserName, intIdCompetenza, dtT, sqlLocalConn)

            Return PathDocumento
        End Get
        Set(ByVal Value As String)
            PathDocumento = Value
        End Set
    End Property

    Public Property VOL_letteraAssegnazioneRitornoMittente(ByVal IdVolontario As Integer, ByVal IdEnte As Integer, ByVal strUserName As String, ByVal intIdCompetenza As Integer, ByVal sqlLocalConn As SqlClient.SqlConnection) As String
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

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDENTE"
            dtR(1) = IdEnte

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)

            PathDocumento = EseguiGenerazioneModello(69, strUserName, intIdCompetenza, dtT, sqlLocalConn)

            Return PathDocumento
        End Get
        Set(ByVal Value As String)
            PathDocumento = Value
        End Set
    End Property

    Public Property VOL_LetteraEsclusione(ByVal IdVolontario As Integer, ByVal IdEnte As Integer, ByVal IdProgetto As Integer, ByVal CodiceFiscale As String, ByVal strUserName As String, ByVal intIdCompetenza As Integer, ByVal sqlLocalConn As SqlClient.SqlConnection) As String
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

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDENTE"
            dtR(1) = IdEnte

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDPROGETTO"
            dtR(1) = IdProgetto

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "CODICEFISCALE"
            dtR(1) = CodiceFiscale

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)

            PathDocumento = EseguiGenerazioneModello(71, strUserName, intIdCompetenza, dtT, sqlLocalConn)

            Return PathDocumento
        End Get
        Set(ByVal Value As String)
            PathDocumento = Value
        End Set
    End Property

    Function VOL_ControllaDoppiaDomanda(ByVal IdProgetto As Integer, ByVal codicefiscale As String) As String
        Dim strsql As String
        Dim strValoriMultipli As String = ""
        Dim dtrLeggiDati As SqlClient.SqlDataReader

        'chiudo il datareader
        If Not dtrLeggiDati Is Nothing Then
            dtrLeggiDati.Close()
            dtrLeggiDati = Nothing
        End If

        'Esiste Record in GraduatorieEntità per stesso Volontario e Stesso Bando?
        'StrSql = "SELECT attività.titolo + '(' + attività.codiceente + ')' + enti.denominazione as dato " & _
        strsql = "SELECT attività.titolo + ' (' + attività.codiceente + ') presso ' + enti.denominazione + ' - ' + entisedi.denominazione " & _
        " + ' - ' + comuni.denominazione as dato " & _
        " FROM graduatorieentità " & _
        " inner join entità on entità.identità = graduatorieentità.identità " & _
        " inner join attivitàsediassegnazione on " & _
        " (attivitàsediassegnazione.idattivitàsedeassegnazione=graduatorieentità.idattivitàsedeassegnazione)" & _
        " inner join entisedi on  entisedi.identesede=attivitàsediassegnazione.identesede " & _
        " inner join comuni on entisedi.idcomune=comuni.idcomune " & _
        " inner join attività on (attività.idattività=attivitàsediassegnazione.idattività)" & _
        " LEFT JOIN BANDORICORSI ON attività.IDBANDORICORSO = BANDORICORSI.IDBANDORICORSO " & _
        " INNER join enti on enti.idente=attività.identepresentante " & _
        " inner join bandiAttività on (Attività.idbandoattività=bandiAttività.idbandoattività)" & _
        " inner join bando on (bando.idbando=bandiAttività.idbando)" & _
        " where entità.codicefiscale='" & codicefiscale & "' and CASE ISNULL(BANDORICORSI.IDBANDORICORSO,0) WHEN 0 THEN bando.Gruppo ELSE BANDORICORSI.Gruppo END =(Select CASE ISNULL(BANDORICORSI.IDBANDORICORSO,0) WHEN 0 THEN bando.Gruppo ELSE BANDORICORSI.Gruppo END from Attività " & _
        " LEFT JOIN BANDORICORSI ON attività.IDBANDORICORSO = BANDORICORSI.IDBANDORICORSO " & _
        " inner join bandiAttività on (Attività.idbandoattività=bandiAttività.idbandoattività)" & _
        " inner join bando on (bando.idbando=bandiAttività.idbando)" & _
        " where Attività.idattività=" & IdProgetto & ")"
        '" where graduatorieentità.identità=" & dgRisultatoRicerca.Items(item.ItemIndex).Cells(19).Text & " and bando.idbando =(Select bando.idBando from Attività " & _

        'eseguo la query e passo il risultato al datareader
        dtrLeggiDati = ClsServer.CreaDatareader(strsql, HttpContext.Current.Session("conn"))

        Dim intX As Integer

        'ciclo il risultato della query e costruisco la stringa con i valori multipli
        While dtrLeggiDati.Read
            intX = intX + 1
            strValoriMultipli = strValoriMultipli & intX & ".           " & dtrLeggiDati("dato") & "\par" & vbCrLf
        End While

        'chiudo il datareader
        If Not dtrLeggiDati Is Nothing Then
            dtrLeggiDati.Close()
            dtrLeggiDati = Nothing
        End If

        'passo la stringa con i valori multipli alla funzione per il valore di ritorno
        VOL_ControllaDoppiaDomanda = strValoriMultipli

        'ritorno la stringa
        Return VOL_ControllaDoppiaDomanda

    End Function

    Public Property VOL_ChiusuraIniziale(ByVal IdVolontario As Integer, ByVal IdEnte As Integer, ByVal strUserName As String, ByVal intIdCompetenza As Integer, ByVal sqlLocalConn As SqlClient.SqlConnection) As String
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

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDENTE"
            dtR(1) = IdEnte

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)

            PathDocumento = EseguiGenerazioneModello(72, strUserName, intIdCompetenza, dtT, sqlLocalConn)

            Return PathDocumento
        End Get
        Set(ByVal Value As String)
            PathDocumento = Value
        End Set
    End Property

    Public Property VOL_ChiusuraInServizio(ByVal IdVolontario As Integer, ByVal IdEnte As Integer, ByVal strUserName As String, ByVal intIdCompetenza As Integer, ByVal sqlLocalConn As SqlClient.SqlConnection) As String
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

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDENTE"
            dtR(1) = IdEnte

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)

            PathDocumento = EseguiGenerazioneModello(73, strUserName, intIdCompetenza, dtT, sqlLocalConn)

            Return PathDocumento
        End Get
        Set(ByVal Value As String)
            PathDocumento = Value
        End Set
    End Property
    Public Property VOL_ChiusuraInServizioSCD(ByVal IdVolontario As Integer, ByVal IdEnte As Integer, ByVal strUserName As String, ByVal intIdCompetenza As Integer, ByVal sqlLocalConn As SqlClient.SqlConnection) As String
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

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDENTE"
            dtR(1) = IdEnte

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)

            PathDocumento = EseguiGenerazioneModello(307, strUserName, intIdCompetenza, dtT, sqlLocalConn)

            Return PathDocumento
        End Get
        Set(ByVal Value As String)
            PathDocumento = Value
        End Set
    End Property
    Public Property VOL_ChiusuraInServizioEstero(ByVal IdVolontario As Integer, ByVal IdEnte As Integer, ByVal strUserName As String, ByVal intIdCompetenza As Integer, ByVal sqlLocalConn As SqlClient.SqlConnection) As String
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

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDENTE"
            dtR(1) = IdEnte

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)

            PathDocumento = EseguiGenerazioneModello(308, strUserName, intIdCompetenza, dtT, sqlLocalConn)

            Return PathDocumento
        End Get
        Set(ByVal Value As String)
            PathDocumento = Value
        End Set
    End Property
    Public Property VOL_LetteraEsclusionePerAssenzaIngiustificata(ByVal IdVolontario As Integer, ByVal IdEnte As Integer, ByVal strUserName As String, ByVal intIdCompetenza As Integer, ByVal sqlLocalConn As SqlClient.SqlConnection) As String
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

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDENTE"
            dtR(1) = IdEnte

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)

            PathDocumento = EseguiGenerazioneModello(106, strUserName, intIdCompetenza, dtT, sqlLocalConn)

            Return PathDocumento
        End Get
        Set(ByVal Value As String)
            PathDocumento = Value
        End Set
    End Property
    Public Property VOL_LetteraEsclusionePerAssenzaIngiustificataSCD(ByVal IdVolontario As Integer, ByVal IdEnte As Integer, ByVal strUserName As String, ByVal intIdCompetenza As Integer, ByVal sqlLocalConn As SqlClient.SqlConnection) As String
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

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDENTE"
            dtR(1) = IdEnte

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)

            PathDocumento = EseguiGenerazioneModello(305, strUserName, intIdCompetenza, dtT, sqlLocalConn)

            Return PathDocumento
        End Get
        Set(ByVal Value As String)
            PathDocumento = Value
        End Set
    End Property
    Public Property VOL_LetteraEsclusionePerAssenzaIngiustificataEstero(ByVal IdVolontario As Integer, ByVal IdEnte As Integer, ByVal strUserName As String, ByVal intIdCompetenza As Integer, ByVal sqlLocalConn As SqlClient.SqlConnection) As String
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

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDENTE"
            dtR(1) = IdEnte

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)

            PathDocumento = EseguiGenerazioneModello(306, strUserName, intIdCompetenza, dtT, sqlLocalConn)

            Return PathDocumento
        End Get
        Set(ByVal Value As String)
            PathDocumento = Value
        End Set
    End Property
    Public Property VOL_LetteraEsclusionePerGiorniPermesso(ByVal IdVolontario As Integer, ByVal IdEnte As Integer, ByVal strUserName As String, ByVal intIdCompetenza As Integer, ByVal sqlLocalConn As SqlClient.SqlConnection) As String
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

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDENTE"
            dtR(1) = IdEnte

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)

            PathDocumento = EseguiGenerazioneModello(107, strUserName, intIdCompetenza, dtT, sqlLocalConn)

            Return PathDocumento
        End Get
        Set(ByVal Value As String)
            PathDocumento = Value
        End Set
    End Property
    Public Property VOL_LetteraEsclusionePerGiorniPermessoSCD(ByVal IdVolontario As Integer, ByVal IdEnte As Integer, ByVal strUserName As String, ByVal intIdCompetenza As Integer, ByVal sqlLocalConn As SqlClient.SqlConnection) As String
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

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDENTE"
            dtR(1) = IdEnte

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)

            PathDocumento = EseguiGenerazioneModello(313, strUserName, intIdCompetenza, dtT, sqlLocalConn)

            Return PathDocumento
        End Get
        Set(ByVal Value As String)
            PathDocumento = Value
        End Set
    End Property
    Public Property VOL_LetteraEsclusionePerGiorniPermessoEstero(ByVal IdVolontario As Integer, ByVal IdEnte As Integer, ByVal strUserName As String, ByVal intIdCompetenza As Integer, ByVal sqlLocalConn As SqlClient.SqlConnection) As String
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

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDENTE"
            dtR(1) = IdEnte

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)

            PathDocumento = EseguiGenerazioneModello(314, strUserName, intIdCompetenza, dtT, sqlLocalConn)

            Return PathDocumento
        End Get
        Set(ByVal Value As String)
            PathDocumento = Value
        End Set
    End Property
    Public Property VOL_LetteraEsclusionePerSuperamentoMalattia(ByVal IdVolontario As Integer, ByVal IdEnte As Integer, ByVal strUserName As String, ByVal intIdCompetenza As Integer, ByVal sqlLocalConn As SqlClient.SqlConnection) As String
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

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDENTE"
            dtR(1) = IdEnte

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)

            PathDocumento = EseguiGenerazioneModello(108, strUserName, intIdCompetenza, dtT, sqlLocalConn)

            Return PathDocumento
        End Get
        Set(ByVal Value As String)
            PathDocumento = Value
        End Set
    End Property
    Public Property VOL_LetteraEsclusionePerSuperamentoMalattiaSCD(ByVal IdVolontario As Integer, ByVal IdEnte As Integer, ByVal strUserName As String, ByVal intIdCompetenza As Integer, ByVal sqlLocalConn As SqlClient.SqlConnection) As String
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

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDENTE"
            dtR(1) = IdEnte

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)

            PathDocumento = EseguiGenerazioneModello(309, strUserName, intIdCompetenza, dtT, sqlLocalConn)

            Return PathDocumento
        End Get
        Set(ByVal Value As String)
            PathDocumento = Value
        End Set
    End Property
    Public Property VOL_LetteraEsclusionePerSuperamentoMalattiaEstero(ByVal IdVolontario As Integer, ByVal IdEnte As Integer, ByVal strUserName As String, ByVal intIdCompetenza As Integer, ByVal sqlLocalConn As SqlClient.SqlConnection) As String
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

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDENTE"
            dtR(1) = IdEnte

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)

            PathDocumento = EseguiGenerazioneModello(310, strUserName, intIdCompetenza, dtT, sqlLocalConn)

            Return PathDocumento
        End Get
        Set(ByVal Value As String)
            PathDocumento = Value
        End Set
    End Property
    Public Property VOL_LetteraEsclusionePerSuperamentoMalattiaDopoi6Mesi(ByVal IdVolontario As Integer, ByVal IdEnte As Integer, ByVal strUserName As String, ByVal intIdCompetenza As Integer, ByVal sqlLocalConn As SqlClient.SqlConnection) As String
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

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDENTE"
            dtR(1) = IdEnte

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)

            PathDocumento = EseguiGenerazioneModello(110, strUserName, intIdCompetenza, dtT, sqlLocalConn)

            Return PathDocumento
        End Get
        Set(ByVal Value As String)
            PathDocumento = Value
        End Set
    End Property
    Public Property VOL_LetteraEsclusionePerSuperamentoMalattiaDopoi6MesiSCD(ByVal IdVolontario As Integer, ByVal IdEnte As Integer, ByVal strUserName As String, ByVal intIdCompetenza As Integer, ByVal sqlLocalConn As SqlClient.SqlConnection) As String
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

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDENTE"
            dtR(1) = IdEnte

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)

            PathDocumento = EseguiGenerazioneModello(311, strUserName, intIdCompetenza, dtT, sqlLocalConn)

            Return PathDocumento
        End Get
        Set(ByVal Value As String)
            PathDocumento = Value
        End Set
    End Property
    Public Property VOL_LetteraEsclusionePerSuperamentoMalattiaDopoi6MesiEstero(ByVal IdVolontario As Integer, ByVal IdEnte As Integer, ByVal strUserName As String, ByVal intIdCompetenza As Integer, ByVal sqlLocalConn As SqlClient.SqlConnection) As String
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

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDENTE"
            dtR(1) = IdEnte

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)

            PathDocumento = EseguiGenerazioneModello(312, strUserName, intIdCompetenza, dtT, sqlLocalConn)

            Return PathDocumento
        End Get
        Set(ByVal Value As String)
            PathDocumento = Value
        End Set
    End Property
    Public Property VOL_rinunciaserviziovolontariomultipla(ByVal IdEnte As Integer, ByVal IdVolontarioSubentrato As Integer, ByVal IdBando As Integer, ByVal strUserName As String, ByVal intIdCompetenza As Integer, ByVal sqlLocalConn As SqlClient.SqlConnection) As String
        Get
            Dim dtR As DataRow
            Dim dtT As New DataTable

            dtT.Columns.Add(New DataColumn("PARAMETRO", GetType(String)))
            dtT.Columns.Add(New DataColumn("VALORE", GetType(String)))

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDENTE"
            dtR(1) = IdEnte

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDENTITàSUBENTRATA"
            dtR(1) = IdVolontarioSubentrato

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDBANDO"
            dtR(1) = IdBando

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)

            PathDocumento = EseguiGenerazioneModello(76, strUserName, intIdCompetenza, dtT, sqlLocalConn)

            Return PathDocumento
        End Get
        Set(ByVal Value As String)
            PathDocumento = Value
        End Set
    End Property
    Public Property VOL_rinunciaserviziovolontariomultiplaSCD(ByVal IdEnte As Integer, ByVal IdVolontarioSubentrato As Integer, ByVal IdBando As Integer, ByVal strUserName As String, ByVal intIdCompetenza As Integer, ByVal sqlLocalConn As SqlClient.SqlConnection) As String
        Get
            Dim dtR As DataRow
            Dim dtT As New DataTable

            dtT.Columns.Add(New DataColumn("PARAMETRO", GetType(String)))
            dtT.Columns.Add(New DataColumn("VALORE", GetType(String)))

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDENTE"
            dtR(1) = IdEnte

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDENTITàSUBENTRATA"
            dtR(1) = IdVolontarioSubentrato

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDBANDO"
            dtR(1) = IdBando

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)

            PathDocumento = EseguiGenerazioneModello(317, strUserName, intIdCompetenza, dtT, sqlLocalConn)

            Return PathDocumento
        End Get
        Set(ByVal Value As String)
            PathDocumento = Value
        End Set
    End Property
    Public Property VOL_rinunciaserviziovolontariomultiplaEstero(ByVal IdEnte As Integer, ByVal IdVolontarioSubentrato As Integer, ByVal IdBando As Integer, ByVal strUserName As String, ByVal intIdCompetenza As Integer, ByVal sqlLocalConn As SqlClient.SqlConnection) As String
        Get
            Dim dtR As DataRow
            Dim dtT As New DataTable

            dtT.Columns.Add(New DataColumn("PARAMETRO", GetType(String)))
            dtT.Columns.Add(New DataColumn("VALORE", GetType(String)))

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDENTE"
            dtR(1) = IdEnte

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDENTITàSUBENTRATA"
            dtR(1) = IdVolontarioSubentrato

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDBANDO"
            dtR(1) = IdBando

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)

            PathDocumento = EseguiGenerazioneModello(318, strUserName, intIdCompetenza, dtT, sqlLocalConn)

            Return PathDocumento
        End Get
        Set(ByVal Value As String)
            PathDocumento = Value
        End Set
    End Property
    Public Property VOL_rinunciaserviziovolontariomultiplaCopiaReg(ByVal IdEnte As Integer, ByVal IdVolontarioSubentrato As Integer, ByVal IdBando As Integer, ByVal strUserName As String, ByVal intIdCompetenza As Integer, ByVal sqlLocalConn As SqlClient.SqlConnection) As String
        Get
            Dim dtR As DataRow
            Dim dtT As New DataTable

            dtT.Columns.Add(New DataColumn("PARAMETRO", GetType(String)))
            dtT.Columns.Add(New DataColumn("VALORE", GetType(String)))

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDENTE"
            dtR(1) = IdEnte

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDENTITàSUBENTRATA"
            dtR(1) = IdVolontarioSubentrato

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDBANDO"
            dtR(1) = IdBando

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)

            PathDocumento = EseguiGenerazioneModello(77, strUserName, intIdCompetenza, dtT, sqlLocalConn)

            Return PathDocumento
        End Get
        Set(ByVal Value As String)
            PathDocumento = Value
        End Set
    End Property

    Public Property VOL_rinunciaserviziovolontario(ByVal IdEnte As Integer, ByVal IdVolontarioSostituito As Integer, ByVal strUserName As String, ByVal intIdCompetenza As Integer, ByVal sqlLocalConn As SqlClient.SqlConnection) As String
        Get
            Dim dtR As DataRow
            Dim dtT As New DataTable

            dtT.Columns.Add(New DataColumn("PARAMETRO", GetType(String)))
            dtT.Columns.Add(New DataColumn("VALORE", GetType(String)))

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDENTE"
            dtR(1) = IdEnte

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDENTITàSOSTITUITA"
            dtR(1) = IdVolontarioSostituito

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)

            PathDocumento = EseguiGenerazioneModello(74, strUserName, intIdCompetenza, dtT, sqlLocalConn)

            Return PathDocumento
        End Get
        Set(ByVal Value As String)
            PathDocumento = Value
        End Set
    End Property
    Public Property VOL_rinunciaserviziovolontarioSCD(ByVal IdEnte As Integer, ByVal IdVolontarioSostituito As Integer, ByVal strUserName As String, ByVal intIdCompetenza As Integer, ByVal sqlLocalConn As SqlClient.SqlConnection) As String
        Get
            Dim dtR As DataRow
            Dim dtT As New DataTable

            dtT.Columns.Add(New DataColumn("PARAMETRO", GetType(String)))
            dtT.Columns.Add(New DataColumn("VALORE", GetType(String)))

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDENTE"
            dtR(1) = IdEnte

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDENTITàSOSTITUITA"
            dtR(1) = IdVolontarioSostituito

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)

            PathDocumento = EseguiGenerazioneModello(315, strUserName, intIdCompetenza, dtT, sqlLocalConn)

            Return PathDocumento
        End Get
        Set(ByVal Value As String)
            PathDocumento = Value
        End Set
    End Property
    Public Property VOL_rinunciaserviziovolontarioEstero(ByVal IdEnte As Integer, ByVal IdVolontarioSostituito As Integer, ByVal strUserName As String, ByVal intIdCompetenza As Integer, ByVal sqlLocalConn As SqlClient.SqlConnection) As String
        Get
            Dim dtR As DataRow
            Dim dtT As New DataTable

            dtT.Columns.Add(New DataColumn("PARAMETRO", GetType(String)))
            dtT.Columns.Add(New DataColumn("VALORE", GetType(String)))

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDENTE"
            dtR(1) = IdEnte

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDENTITàSOSTITUITA"
            dtR(1) = IdVolontarioSostituito

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)

            PathDocumento = EseguiGenerazioneModello(316, strUserName, intIdCompetenza, dtT, sqlLocalConn)

            Return PathDocumento
        End Get
        Set(ByVal Value As String)
            PathDocumento = Value
        End Set
    End Property
    Public Property VOL_rinunciaserviziovolontarioCopiaReg(ByVal IdEnte As Integer, ByVal IdVolontarioSostituito As Integer, ByVal strUserName As String, ByVal intIdCompetenza As Integer, ByVal sqlLocalConn As SqlClient.SqlConnection) As String
        Get
            Dim dtR As DataRow
            Dim dtT As New DataTable

            dtT.Columns.Add(New DataColumn("PARAMETRO", GetType(String)))
            dtT.Columns.Add(New DataColumn("VALORE", GetType(String)))

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDENTE"
            dtR(1) = IdEnte

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDENTITàSOSTITUITA"
            dtR(1) = IdVolontarioSostituito

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)

            PathDocumento = EseguiGenerazioneModello(75, strUserName, intIdCompetenza, dtT, sqlLocalConn)

            Return PathDocumento
        End Get
        Set(ByVal Value As String)
            PathDocumento = Value
        End Set
    End Property

    Public Property MON_Trasmissioneprogrammazione(ByVal IdProgrammazione As Integer, ByVal strUserName As String, ByVal intIdCompetenza As Integer, ByVal sqlLocalConn As SqlClient.SqlConnection) As String
        Get
            Dim dtR As DataRow
            Dim dtT As New DataTable

            dtT.Columns.Add(New DataColumn("PARAMETRO", GetType(String)))
            dtT.Columns.Add(New DataColumn("VALORE", GetType(String)))

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDPROGRAMMAZIONE"
            dtR(1) = IdProgrammazione

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)
            'mod. il 04/10/2016 da simona cordella
            ' passo l'id modello diverso  se vengo da helios o Futuro
            Dim IntModello As Integer
            If Session("Sistema") = "Helios" Then
                IntModello = 78
            Else
                IntModello = 225
            End If
            PathDocumento = EseguiGenerazioneModello(IntModello, strUserName, intIdCompetenza, dtT, sqlLocalConn)

            Return PathDocumento
        End Get
        Set(ByVal Value As String)
            PathDocumento = Value
        End Set
    End Property

    Public Property MON_letterainterlocutoria(ByVal IdEnte As Integer, ByVal IdVerifica As Integer, ByVal strUserName As String, ByVal intIdCompetenza As Integer, ByVal sqlLocalConn As SqlClient.SqlConnection) As String
        Get
            Dim dtR As DataRow
            Dim dtT As New DataTable

            dtT.Columns.Add(New DataColumn("PARAMETRO", GetType(String)))
            dtT.Columns.Add(New DataColumn("VALORE", GetType(String)))

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDVERIFICA"
            dtR(1) = IdVerifica

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDENTE"
            dtR(1) = IdEnte

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)
            'mod. il 04/10/2016 da simona cordella
            ' passo l'id modello diverso  se vengo da helios o Futuro
            Dim IntModello As Integer
            If Session("Sistema") = "Helios" Then
                IntModello = 79
            Else
                IntModello = 226
            End If
            PathDocumento = EseguiGenerazioneModello(IntModello, strUserName, intIdCompetenza, dtT, sqlLocalConn)

            Return PathDocumento
        End Get
        Set(ByVal Value As String)
            PathDocumento = Value
        End Set
    End Property

    Function MON_ElencoSediVerifica(ByVal IdVerifica As Integer) As String
        Dim strsql As String
        Dim strValoriMultipli As String = ""
        Dim dtrLeggiDati As SqlClient.SqlDataReader


        'chiudo il datareader
        If Not dtrLeggiDati Is Nothing Then
            dtrLeggiDati.Close()
            dtrLeggiDati = Nothing
        End If

        strsql = "select DenominazioneSedeProgetto, IndirizzoSedeProgetto, CivicoSedeProgetto, CAPSedeProgetto, ComuneSedeProgetto, ProvinciaSedeProgettoBreve,DettaglioRecapito from VW_EDITOR_VERIFICHE "
        strsql = strsql & "where idverifica=" & IdVerifica

        'eseguo la query e passo il risultato al datareader
        dtrLeggiDati = ClsServer.CreaDatareader(strsql, HttpContext.Current.Session("conn"))

        'ciclo il risultato della query e costruisco la stringa con i valori multipli
        While dtrLeggiDati.Read

            If IsDBNull(dtrLeggiDati("DettaglioRecapito")) = True Then

                strValoriMultipli = strValoriMultipli & "\trowd\trgaph70\trleft-70\trbrdrl\brdrtbl\brdrnone "
                strValoriMultipli = strValoriMultipli & "\trbrdrt\brdrtbl\brdrnone \trbrdrr\brdrtbl\brdrnone "
                strValoriMultipli = strValoriMultipli & "\trbrdrb\brdrtbl\brdrnone "
                strValoriMultipli = strValoriMultipli & "\trpaddl70\trpaddr70\trpaddfl3\trpaddfr3"
                strValoriMultipli = strValoriMultipli & "\clbrdrl\brdrnone\brdrtbl\clbrdrt\brdrnone\brdrtbl\clbrdrr\brdrnone\brdrtbl\clbrdrb\brdrnone\brdrtbl "
                strValoriMultipli = strValoriMultipli & "\cellx5240\clbrdrl\brdrnone\brdrtbl\clbrdrt\brdrnone\brdrtbl\clbrdrr\brdrnone\brdrtbl\clbrdrb\brdrnone\brdrtbl "
                strValoriMultipli = strValoriMultipli & "\cellx9800\pard\nowidctl\intbl\ri500\f0\fs24\cell\par " & dtrLeggiDati("DenominazioneSedeProgetto") & " \par Ufficio per il Servizio Civile " & " \par " & dtrLeggiDati("IndirizzoSedeProgetto") & " " & dtrLeggiDati("CivicoSedeProgetto") & " " & dtrLeggiDati("CAPSedeProgetto") & " \par " & dtrLeggiDati("ComuneSedeProgetto") & " (" & dtrLeggiDati("ProvinciaSedeProgettoBreve") & ")\cell\row\pard\f0\fs24"
            Else


                If dtrLeggiDati("DettaglioRecapito") = " " Then
                    strValoriMultipli = strValoriMultipli & "\trowd\trgaph70\trleft-70\trbrdrl\brdrtbl\brdrnone "
                    strValoriMultipli = strValoriMultipli & "\trbrdrt\brdrtbl\brdrnone \trbrdrr\brdrtbl\brdrnone "
                    strValoriMultipli = strValoriMultipli & "\trbrdrb\brdrtbl\brdrnone "
                    strValoriMultipli = strValoriMultipli & "\trpaddl70\trpaddr70\trpaddfl3\trpaddfr3"
                    strValoriMultipli = strValoriMultipli & "\clbrdrl\brdrnone\brdrtbl\clbrdrt\brdrnone\brdrtbl\clbrdrr\brdrnone\brdrtbl\clbrdrb\brdrnone\brdrtbl "
                    strValoriMultipli = strValoriMultipli & "\cellx5240\clbrdrl\brdrnone\brdrtbl\clbrdrt\brdrnone\brdrtbl\clbrdrr\brdrnone\brdrtbl\clbrdrb\brdrnone\brdrtbl "
                    strValoriMultipli = strValoriMultipli & "\cellx9800\pard\nowidctl\intbl\ri500\f0\fs24\cell\par " & dtrLeggiDati("DenominazioneSedeProgetto") & " \par Ufficio per il Servizio Civile " & " \par " & dtrLeggiDati("IndirizzoSedeProgetto") & " " & dtrLeggiDati("CivicoSedeProgetto") & " " & dtrLeggiDati("CAPSedeProgetto") & " \par " & dtrLeggiDati("ComuneSedeProgetto") & " (" & dtrLeggiDati("ProvinciaSedeProgettoBreve") & ")\cell\row\pard\f0\fs24"
                Else
                    strValoriMultipli = strValoriMultipli & "\trowd\trgaph70\trleft-70\trbrdrl\brdrtbl\brdrnone "
                    strValoriMultipli = strValoriMultipli & "\trbrdrt\brdrtbl\brdrnone \trbrdrr\brdrtbl\brdrnone "
                    strValoriMultipli = strValoriMultipli & "\trbrdrb\brdrtbl\brdrnone "
                    strValoriMultipli = strValoriMultipli & "\trpaddl70\trpaddr70\trpaddfl3\trpaddfr3"
                    strValoriMultipli = strValoriMultipli & "\clbrdrl\brdrnone\brdrtbl\clbrdrt\brdrnone\brdrtbl\clbrdrr\brdrnone\brdrtbl\clbrdrb\brdrnone\brdrtbl "
                    strValoriMultipli = strValoriMultipli & "\cellx5240\clbrdrl\brdrnone\brdrtbl\clbrdrt\brdrnone\brdrtbl\clbrdrr\brdrnone\brdrtbl\clbrdrb\brdrnone\brdrtbl "
                    strValoriMultipli = strValoriMultipli & "\cellx9800\pard\nowidctl\intbl\ri500\f0\fs24\cell\par " & dtrLeggiDati("DenominazioneSedeProgetto") & " \par Ufficio per il Servizio Civile " & " \par " & dtrLeggiDati("IndirizzoSedeProgetto") & " " & dtrLeggiDati("CivicoSedeProgetto") & " " & dtrLeggiDati("CAPSedeProgetto") & " \par (" & dtrLeggiDati("DettaglioRecapito") & ")\par " & dtrLeggiDati("ComuneSedeProgetto") & " (" & dtrLeggiDati("ProvinciaSedeProgettoBreve") & ")\cell\row\pard\f0\fs24"
                End If

            End If

        End While

        'strValoriMultipli = strValoriMultipli & "\par"

        'chiudo il datareader
        If Not dtrLeggiDati Is Nothing Then
            dtrLeggiDati.Close()
            dtrLeggiDati = Nothing
        End If

        'passo la stringa con i valori multipli alla funzione per il valore di ritorno
        MON_ElencoSediVerifica = strValoriMultipli

        'ritorno la stringa
        Return MON_ElencoSediVerifica

    End Function

    Public Property MON_credenziali(ByVal IdEnte As Integer, ByVal IdVerifica As Integer, ByVal strUserName As String, ByVal intIdCompetenza As Integer, ByVal sqlLocalConn As SqlClient.SqlConnection) As String
        Get
            Dim dtR As DataRow
            Dim dtT As New DataTable

            dtT.Columns.Add(New DataColumn("PARAMETRO", GetType(String)))
            dtT.Columns.Add(New DataColumn("VALORE", GetType(String)))

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDVERIFICA"
            dtR(1) = IdVerifica

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDENTE"
            dtR(1) = IdEnte

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)

            'mod. il 04/10/2016 da simona cordella
            ' passo l'id modello diverso  se vengo da helios o Futuro
            Dim IntModello As Integer
            If Session("Sistema") = "Helios" Then
                IntModello = 80
            Else
                IntModello = 227
            End If
            PathDocumento = EseguiGenerazioneModello(IntModello, strUserName, intIdCompetenza, dtT, sqlLocalConn)

            Return PathDocumento
        End Get
        Set(ByVal Value As String)
            PathDocumento = Value
        End Set
    End Property

    Public Property MON_Letteradiincarico(ByVal IdEnte As Integer, ByVal IdVerifica As Integer, ByVal strUserName As String, ByVal intIdCompetenza As Integer, ByVal sqlLocalConn As SqlClient.SqlConnection) As String
        Get
            Dim dtR As DataRow
            Dim dtT As New DataTable

            dtT.Columns.Add(New DataColumn("PARAMETRO", GetType(String)))
            dtT.Columns.Add(New DataColumn("VALORE", GetType(String)))

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDVERIFICA"
            dtR(1) = IdVerifica

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDENTE"
            dtR(1) = IdEnte

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)
            'mod. il 04/10/2016 da simona cordella
            ' passo l'id modello diverso  se vengo da helios o Futuro
            Dim IntModello As Integer
            If Session("Sistema") = "Helios" Then
                IntModello = 81
            Else
                IntModello = 228
            End If
            PathDocumento = EseguiGenerazioneModello(IntModello, strUserName, intIdCompetenza, dtT, sqlLocalConn)

            Return PathDocumento
        End Get
        Set(ByVal Value As String)
            PathDocumento = Value
        End Set
    End Property

    Function MON_ElencoFigliVerifica(ByVal IdVerifica As Integer) As String
        Dim strsql As String
        Dim strValoriMultipli As String = ""
        Dim dtrLeggiDati As SqlClient.SqlDataReader
        Dim num As Integer = 0

        'chiudo il datareader
        If Not dtrLeggiDati Is Nothing Then
            dtrLeggiDati.Close()
            dtrLeggiDati = Nothing
        End If

        strsql = "select DenominazioneSedeProgetto, IndirizzoSedeProgetto, CivicoSedeProgetto, CAPSedeProgetto, ComuneSedeProgetto, ProvinciaSedeProgettoBreve,DettaglioRecapito from VW_EDITOR_VERIFICHE "
        strsql = strsql & "where idverifica=" & IdVerifica

        'eseguo la query e passo il risultato al datareader
        dtrLeggiDati = ClsServer.CreaDatareader(strsql, HttpContext.Current.Session("conn"))

        'ciclo il risultato della query e costruisco la stringa con i valori multipli
        While dtrLeggiDati.Read
            num = num + 1
            strValoriMultipli = strValoriMultipli & "\trowd\trgaph70\trleft-70\trbrdrl\brdrs\brdrw10 " & vbCrLf
            strValoriMultipli = strValoriMultipli & "\trbrdrt\brdrs\brdrw10 \trbrdrr\brdrs\brdrw10 " & vbCrLf
            strValoriMultipli = strValoriMultipli & "\trbrdrb\brdrs\brdrw10 " & vbCrLf
            strValoriMultipli = strValoriMultipli & "\trpaddl70\trpaddr70\trpaddfl3\trpaddfr3" & vbCrLf
            strValoriMultipli = strValoriMultipli & "\clbrdrl\brdrw10\brdrs\clbrdrt\brdrw10\brdrs\clbrdrr\brdrw10\brdrs\clbrdrb\brdrw10\brdrs " & vbCrLf
            strValoriMultipli = strValoriMultipli & "\cellx700\clbrdrl\brdrw10\brdrs\clbrdrt\brdrw10\brdrs\clbrdrr\brdrw10\brdrs\clbrdrb\brdrw10\brdrs " & vbCrLf
            strValoriMultipli = strValoriMultipli & "\cellx3500\clbrdrl\brdrw10\brdrs\clbrdrt\brdrw10\brdrs\clbrdrr\brdrw10\brdrs\clbrdrb\brdrw10\brdrs " & vbCrLf
            strValoriMultipli = strValoriMultipli & "\cellx7000\clbrdrl\brdrw10\brdrs\clbrdrt\brdrw10\brdrs\clbrdrr\brdrw10\brdrs\clbrdrb\brdrw10\brdrs " & vbCrLf
            strValoriMultipli = strValoriMultipli & "\cellx7500\clbrdrl\brdrw10\brdrs\clbrdrt\brdrw10\brdrs\clbrdrr\brdrw10\brdrs\clbrdrb\brdrw10\brdrs " & vbCrLf
            strValoriMultipli = strValoriMultipli & "\cellx9100\clbrdrl\brdrw10\brdrs\clbrdrt\brdrw10\brdrs\clbrdrr\brdrw10\brdrs\clbrdrb\brdrw10\brdrs " & vbCrLf
            strValoriMultipli = strValoriMultipli & "\cellx10040\pard\intbl\nowidctlpar\ri500\qj\f2\fs15\ " & num & "\cell " & dtrLeggiDati("DenominazioneSedeProgetto") & "\cell " & dtrLeggiDati("IndirizzoSedeProgetto") & "\cell " & dtrLeggiDati("CivicoSedeProgetto") & "\cell " & dtrLeggiDati("ComuneSedeProgetto") & "\cell " & dtrLeggiDati("ProvinciaSedeProgettoBreve") & "\cell\row\pard\f2\fs15" & vbCrLf
        End While

        strValoriMultipli = strValoriMultipli & "\par" & vbCrLf

        'chiudo il datareader
        If Not dtrLeggiDati Is Nothing Then
            dtrLeggiDati.Close()
            dtrLeggiDati = Nothing
        End If

        'passo la stringa con i valori multipli alla funzione per il valore di ritorno
        MON_ElencoFigliVerifica = strValoriMultipli

        'ritorno la stringa
        Return MON_ElencoFigliVerifica

    End Function

    Public Property MON_TrasmissionerelazionealDG(ByVal IdEnte As Integer, ByVal IdVerifica As Integer, ByVal strUserName As String, ByVal intIdCompetenza As Integer, ByVal sqlLocalConn As SqlClient.SqlConnection) As String
        Get
            Dim dtR As DataRow
            Dim dtT As New DataTable

            dtT.Columns.Add(New DataColumn("PARAMETRO", GetType(String)))
            dtT.Columns.Add(New DataColumn("VALORE", GetType(String)))

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDVERIFICA"
            dtR(1) = IdVerifica

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDENTE"
            dtR(1) = IdEnte

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)
            'mod. il 04/10/2016 da simona cordella
            ' passo l'id modello diverso  se vengo da helios o Futuro
            Dim IntModello As Integer
            If Session("Sistema") = "Helios" Then
                IntModello = 82
            Else
                IntModello = 229
            End If
            PathDocumento = EseguiGenerazioneModello(IntModello, strUserName, intIdCompetenza, dtT, sqlLocalConn)

            Return PathDocumento
        End Get
        Set(ByVal Value As String)
            PathDocumento = Value
        End Set
    End Property

    Public Property MON_ConclusioneVerificaPositiva(ByVal IdEnte As Integer, ByVal IdVerifica As Integer, ByVal strUserName As String, ByVal intIdCompetenza As Integer, ByVal sqlLocalConn As SqlClient.SqlConnection) As String
        Get
            Dim dtR As DataRow
            Dim dtT As New DataTable

            dtT.Columns.Add(New DataColumn("PARAMETRO", GetType(String)))
            dtT.Columns.Add(New DataColumn("VALORE", GetType(String)))

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDVERIFICA"
            dtR(1) = IdVerifica

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDENTE"
            dtR(1) = IdEnte

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)

            'mod. il 04/10/2016 da simona cordella
            ' passo l'id modello diverso  se vengo da helios o Futuro
            Dim IntModello As Integer
            If Session("Sistema") = "Helios" Then
                IntModello = 83
            Else
                IntModello = 230
            End If
            PathDocumento = EseguiGenerazioneModello(IntModello, strUserName, intIdCompetenza, dtT, sqlLocalConn)

            Return PathDocumento
        End Get
        Set(ByVal Value As String)
            PathDocumento = Value
        End Set
    End Property

    Public Property MON_VerificasenzaIRREGOLARITAconrichiamo(ByVal IdEnte As Integer, ByVal IdVerifica As Integer, ByVal strUserName As String, ByVal intIdCompetenza As Integer, ByVal sqlLocalConn As SqlClient.SqlConnection) As String
        Get
            Dim dtR As DataRow
            Dim dtT As New DataTable

            dtT.Columns.Add(New DataColumn("PARAMETRO", GetType(String)))
            dtT.Columns.Add(New DataColumn("VALORE", GetType(String)))

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDVERIFICA"
            dtR(1) = IdVerifica

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDENTE"
            dtR(1) = IdEnte

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)
            'mod. il 04/10/2016 da simona cordella
            ' passo l'id modello diverso  se vengo da helios o Futuro
            Dim IntModello As Integer
            If Session("Sistema") = "Helios" Then
                IntModello = 84
            Else
                IntModello = 231
            End If
            PathDocumento = EseguiGenerazioneModello(IntModello, strUserName, intIdCompetenza, dtT, sqlLocalConn)

            Return PathDocumento
        End Get
        Set(ByVal Value As String)
            PathDocumento = Value
        End Set
    End Property

    Public Property MON_LetteraContestazioneAddebiti(ByVal IdEnte As Integer, ByVal IdVerifica As Integer, ByVal strUserName As String, ByVal intIdCompetenza As Integer, ByVal sqlLocalConn As SqlClient.SqlConnection) As String
        Get
            Dim dtR As DataRow
            Dim dtT As New DataTable

            dtT.Columns.Add(New DataColumn("PARAMETRO", GetType(String)))
            dtT.Columns.Add(New DataColumn("VALORE", GetType(String)))

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDVERIFICA"
            dtR(1) = IdVerifica

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDENTE"
            dtR(1) = IdEnte

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)
            'mod. il 04/10/2016 da simona cordella
            ' passo l'id modello diverso  se vengo da helios o Futuro
            Dim IntModello As Integer
            If Session("Sistema") = "Helios" Then
                IntModello = 85
            Else
                IntModello = 232
            End If
            PathDocumento = EseguiGenerazioneModello(IntModello, strUserName, intIdCompetenza, dtT, sqlLocalConn)

            Return PathDocumento
        End Get
        Set(ByVal Value As String)
            PathDocumento = Value
        End Set
    End Property

    Public Property MON_LETTERAtrasmDGecontestazione(ByVal IdEnte As Integer, ByVal IdVerifica As Integer, ByVal strUserName As String, ByVal intIdCompetenza As Integer, ByVal sqlLocalConn As SqlClient.SqlConnection) As String
        Get
            Dim dtR As DataRow
            Dim dtT As New DataTable

            dtT.Columns.Add(New DataColumn("PARAMETRO", GetType(String)))
            dtT.Columns.Add(New DataColumn("VALORE", GetType(String)))

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDVERIFICA"
            dtR(1) = IdVerifica

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDENTE"
            dtR(1) = IdEnte

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)
            'mod. il 04/10/2016 da simona cordella
            ' passo l'id modello diverso  se vengo da helios o Futuro
            Dim IntModello As Integer
            If Session("Sistema") = "Helios" Then
                IntModello = 87
            Else
                IntModello = 233
            End If
            PathDocumento = EseguiGenerazioneModello(IntModello, strUserName, intIdCompetenza, dtT, sqlLocalConn)

            Return PathDocumento
        End Get
        Set(ByVal Value As String)
            PathDocumento = Value
        End Set
    End Property

    Public Property MON_Diffida(ByVal IdEnte As Integer, ByVal IdVerifica As Integer, ByVal strUserName As String, ByVal intIdCompetenza As Integer, ByVal sqlLocalConn As SqlClient.SqlConnection) As String
        Get
            Dim dtR As DataRow
            Dim dtT As New DataTable

            dtT.Columns.Add(New DataColumn("PARAMETRO", GetType(String)))
            dtT.Columns.Add(New DataColumn("VALORE", GetType(String)))

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDVERIFICA"
            dtR(1) = IdVerifica

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDENTE"
            dtR(1) = IdEnte

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)

            'mod. il 04/10/2016 da simona cordella
            ' passo l'id modello diverso  se vengo da helios o Futuro
            Dim IntModello As Integer
            If Session("Sistema") = "Helios" Then
                IntModello = 91
            Else
                IntModello = 234
            End If
            PathDocumento = EseguiGenerazioneModello(IntModello, strUserName, intIdCompetenza, dtT, sqlLocalConn)

            Return PathDocumento
        End Get
        Set(ByVal Value As String)
            PathDocumento = Value
        End Set
    End Property

    Public Property MON_TrasmissioneSanzioneDG(ByVal IdEnte As Integer, ByVal IdVerifica As Integer, ByVal strUserName As String, ByVal intIdCompetenza As Integer, ByVal sqlLocalConn As SqlClient.SqlConnection) As String
        Get
            Dim dtR As DataRow
            Dim dtT As New DataTable

            dtT.Columns.Add(New DataColumn("PARAMETRO", GetType(String)))
            dtT.Columns.Add(New DataColumn("VALORE", GetType(String)))

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDVERIFICA"
            dtR(1) = IdVerifica

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDENTE"
            dtR(1) = IdEnte

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)
            'mod. il 04/10/2016 da simona cordella
            ' passo l'id modello diverso  se vengo da helios o Futuro
            Dim IntModello As Integer
            If Session("Sistema") = "Helios" Then
                IntModello = 111
            Else
                IntModello = 248
            End If
            PathDocumento = EseguiGenerazioneModello(IntModello, strUserName, intIdCompetenza, dtT, sqlLocalConn)

            Return PathDocumento
        End Get
        Set(ByVal Value As String)
            PathDocumento = Value
        End Set
    End Property


    Public Property MON_Letteratrasmissioneprovvedimentosanzionatorio(ByVal IdEnte As Integer, ByVal IdVerifica As Integer, ByVal strUserName As String, ByVal intIdCompetenza As Integer, ByVal sqlLocalConn As SqlClient.SqlConnection) As String
        Get
            Dim dtR As DataRow
            Dim dtT As New DataTable

            dtT.Columns.Add(New DataColumn("PARAMETRO", GetType(String)))
            dtT.Columns.Add(New DataColumn("VALORE", GetType(String)))

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDVERIFICA"
            dtR(1) = IdVerifica

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDENTE"
            dtR(1) = IdEnte

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)
            'mod. il 04/10/2016 da simona cordella
            ' passo l'id modello diverso  se vengo da helios o Futuro
            Dim IntModello As Integer
            If Session("Sistema") = "Helios" Then
                IntModello = 92
            Else
                IntModello = 235
            End If
            PathDocumento = EseguiGenerazioneModello(IntModello, strUserName, intIdCompetenza, dtT, sqlLocalConn)

            Return PathDocumento
        End Get
        Set(ByVal Value As String)
            PathDocumento = Value
        End Set
    End Property

    Public Property MON_TrasmissionerelazionealDGconrichiamo(ByVal IdEnte As Integer, ByVal IdVerifica As Integer, ByVal strUserName As String, ByVal intIdCompetenza As Integer, ByVal sqlLocalConn As SqlClient.SqlConnection) As String
        Get
            Dim dtR As DataRow
            Dim dtT As New DataTable

            dtT.Columns.Add(New DataColumn("PARAMETRO", GetType(String)))
            dtT.Columns.Add(New DataColumn("VALORE", GetType(String)))

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDVERIFICA"
            dtR(1) = IdVerifica

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDENTE"
            dtR(1) = IdEnte

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)
            'mod. il 04/10/2016 da simona cordella
            ' passo l'id modello diverso  se vengo da helios o Futuro
            Dim IntModello As Integer
            If Session("Sistema") = "Helios" Then
                IntModello = 93
            Else
                IntModello = 236
            End If
            PathDocumento = EseguiGenerazioneModello(IntModello, strUserName, intIdCompetenza, dtT, sqlLocalConn)

            Return PathDocumento
        End Get
        Set(ByVal Value As String)
            PathDocumento = Value
        End Set
    End Property

    Public Property MON_LETTERATRASMISPROVAISERVIZI(ByVal IdEnte As Integer, ByVal IdVerifica As Integer, ByVal strUserName As String, ByVal intIdCompetenza As Integer, ByVal sqlLocalConn As SqlClient.SqlConnection) As String
        Get
            Dim dtR As DataRow
            Dim dtT As New DataTable

            dtT.Columns.Add(New DataColumn("PARAMETRO", GetType(String)))
            dtT.Columns.Add(New DataColumn("VALORE", GetType(String)))

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDVERIFICA"
            dtR(1) = IdVerifica

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDENTE"
            dtR(1) = IdEnte

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)
            'mod. il 04/10/2016 da simona cordella
            ' passo l'id modello diverso  se vengo da helios o Futuro
            Dim IntModello As Integer
            If Session("Sistema") = "Helios" Then
                IntModello = 94
            Else
                IntModello = 237
            End If
            PathDocumento = EseguiGenerazioneModello(IntModello, strUserName, intIdCompetenza, dtT, sqlLocalConn)

            Return PathDocumento
        End Get
        Set(ByVal Value As String)
            PathDocumento = Value
        End Set
    End Property

    Public Property MON_TRASMISSIONEPROVVEDIMENTOREGIONE(ByVal IdEnte As Integer, ByVal IdVerifica As Integer, ByVal strUserName As String, ByVal intIdCompetenza As Integer, ByVal sqlLocalConn As SqlClient.SqlConnection) As String
        Get
            Dim dtR As DataRow
            Dim dtT As New DataTable

            dtT.Columns.Add(New DataColumn("PARAMETRO", GetType(String)))
            dtT.Columns.Add(New DataColumn("VALORE", GetType(String)))

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDVERIFICA"
            dtR(1) = IdVerifica

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDENTE"
            dtR(1) = IdEnte

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)
            'mod. il 04/10/2016 da simona cordella
            ' passo l'id modello diverso  se vengo da helios o Futuro
            Dim IntModello As Integer
            If Session("Sistema") = "Helios" Then
                IntModello = 95
            Else
                IntModello = 238
            End If
            PathDocumento = EseguiGenerazioneModello(IntModello, strUserName, intIdCompetenza, dtT, sqlLocalConn)

            Return PathDocumento
        End Get
        Set(ByVal Value As String)
            PathDocumento = Value
        End Set
    End Property

    Public Property MON_relazionestand(ByVal IdEnte As Integer, ByVal IdVerificaAssociata As Integer, ByVal strUserName As String, ByVal intIdCompetenza As Integer, ByVal sqlLocalConn As SqlClient.SqlConnection) As String
        Get
            Dim dtR As DataRow
            Dim dtT As New DataTable

            dtT.Columns.Add(New DataColumn("PARAMETRO", GetType(String)))
            dtT.Columns.Add(New DataColumn("VALORE", GetType(String)))

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDVERIFICAASSOCIATA"
            dtR(1) = IdVerificaAssociata

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDENTE"
            dtR(1) = IdEnte

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)
            'mod. il 04/10/2016 da simona cordella
            ' passo l'id modello diverso  se vengo da helios o Futuro
            Dim IntModello As Integer
            If Session("Sistema") = "Helios" Then
                IntModello = 96
            Else
                IntModello = 239
            End If
            PathDocumento = EseguiGenerazioneModello(IntModello, strUserName, intIdCompetenza, dtT, sqlLocalConn)

            Return PathDocumento
        End Get
        Set(ByVal Value As String)
            PathDocumento = Value
        End Set
    End Property

    Public Property MON_Letteradiincaricomultipla(ByVal IdGruppoStampa As Integer, ByVal strUserName As String, ByVal intIdCompetenza As Integer, ByVal sqlLocalConn As SqlClient.SqlConnection) As String
        Get
            Dim dtR As DataRow
            Dim dtT As New DataTable

            dtT.Columns.Add(New DataColumn("PARAMETRO", GetType(String)))
            dtT.Columns.Add(New DataColumn("VALORE", GetType(String)))

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDGRUPPOSTAMPA"
            dtR(1) = IdGruppoStampa

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)
            'mod. il 04/10/2016 da simona cordella
            ' passo l'id modello diverso  se vengo da helios o Futuro
            Dim IntModello As Integer
            If Session("Sistema") = "Helios" Then
                IntModello = 109
            Else
                IntModello = 247
            End If
            PathDocumento = EseguiGenerazioneModello(IntModello, strUserName, intIdCompetenza, dtT, sqlLocalConn)

            Return PathDocumento
        End Get
        Set(ByVal Value As String)
            PathDocumento = Value
        End Set
    End Property

    Function MON_ElencoVerificheMultiple(ByVal IdGruppoStampa As Integer) As String
        Dim strsql As String
        Dim strValoriMultipli As String = ""
        Dim dtrLeggiDati As SqlClient.SqlDataReader
        Dim num As Integer = 0
        Dim flag As Integer = 1
        Dim intX As Integer
        Dim incremento As Integer = 0

        'chiudo il datareader
        If Not dtrLeggiDati Is Nothing Then
            dtrLeggiDati.Close()
            dtrLeggiDati = Nothing
        End If

        strsql = "select distinct codiceprogetto, titolo, denominazione,DettaglioRecapito from VER_VW_RICERCA_VERIFICHE a "
        strsql = strsql & "inner join tverifichestampa b on a.idverificheassociate=b.idverificheassociate "
        strsql = strsql & " where IdGruppoStampa=" & IdGruppoStampa

        'strsql = "select EnteFiglio, Comune, Provincia, indirizzo, civico, codiceprogetto, titolo, denominazione, DenEnteSede from VER_VW_RICERCA_VERIFICHE a "
        'strsql = strsql & "inner join tverifichestampa b on a.idverificheassociate=b.idverificheassociate "
        'strsql = strsql & " where IdGruppoStampa=" & IdGruppoStampa

        Dim CMD As New SqlClient.SqlDataAdapter(strsql, CType(HttpContext.Current.Session("conn"), SqlClient.SqlConnection))
        Dim dttLocal As New DataTable
        CMD.Fill(dttLocal)

        Dim riga As Data.DataRow

        For Each riga In dttLocal.Rows

            'chiudo il datareader
            If Not dtrLeggiDati Is Nothing Then
                dtrLeggiDati.Close()
                dtrLeggiDati = Nothing
            End If

            strsql = "select EnteFiglio, Comune, DescrAbb, indirizzo, civico, codiceprogetto, titolo, denominazione, DenEnteSede,DettaglioRecapito from VER_VW_RICERCA_VERIFICHE a "
            strsql = strsql & "inner join tverifichestampa b on a.idverificheassociate=b.idverificheassociate "
            strsql = strsql & " where CodiceProgetto='" & riga("CodiceProgetto") & "'"

            'eseguo la query e passo il risultato al datareader
            dtrLeggiDati = ClsServer.CreaDatareader(strsql, HttpContext.Current.Session("conn"))

            strValoriMultipli = strValoriMultipli & "\fs20 " & "Progetto: " & riga("Titolo") & vbCrLf
            strValoriMultipli = strValoriMultipli & "Codice progetto: " & riga("CodiceProgetto") & vbCrLf
            strValoriMultipli = strValoriMultipli & "Ente: " & riga("Denominazione") & vbCrLf & vbCrLf
            strValoriMultipli = strValoriMultipli & "Sede di attuazione: " & vbCrLf

            'ciclo il risultato della query e costruisco la stringa con i valori multipli
            While dtrLeggiDati.Read
                incremento = incremento + 1
                strValoriMultipli = strValoriMultipli & "\trowd\trgaph70\trleft-70\trbrdrl\brdrs\brdrw10 "
                strValoriMultipli = strValoriMultipli & "\trbrdrt\brdrs\brdrw10 \trbrdrr\brdrs\brdrw10 "
                strValoriMultipli = strValoriMultipli & "\trbrdrb\brdrs\brdrw10 "
                strValoriMultipli = strValoriMultipli & "\trpaddl70\trpaddr70\trpaddfl3\trpaddfr3"
                strValoriMultipli = strValoriMultipli & "\clbrdrl\brdrw10\brdrs\clbrdrt\brdrw10\brdrs\clbrdrr\brdrw10\brdrs\clbrdrb\brdrw10\brdrs "
                strValoriMultipli = strValoriMultipli & "\cellx700\clbrdrl\brdrw10\brdrs\clbrdrt\brdrw10\brdrs\clbrdrr\brdrw10\brdrs\clbrdrb\brdrw10\brdrs "
                strValoriMultipli = strValoriMultipli & "\cellx3500\clbrdrl\brdrw10\brdrs\clbrdrt\brdrw10\brdrs\clbrdrr\brdrw10\brdrs\clbrdrb\brdrw10\brdrs "
                strValoriMultipli = strValoriMultipli & "\cellx6300\clbrdrl\brdrw10\brdrs\clbrdrt\brdrw10\brdrs\clbrdrr\brdrw10\brdrs\clbrdrb\brdrw10\brdrs "
                strValoriMultipli = strValoriMultipli & "\cellx7300\clbrdrl\brdrw10\brdrs\clbrdrt\brdrw10\brdrs\clbrdrr\brdrw10\brdrs\clbrdrb\brdrw10\brdrs "
                strValoriMultipli = strValoriMultipli & "\cellx8700\clbrdrl\brdrw10\brdrs\clbrdrt\brdrw10\brdrs\clbrdrr\brdrw10\brdrs\clbrdrb\brdrw10\brdrs "
                strValoriMultipli = strValoriMultipli & "\cellx9700\clbrdrl\brdrw10\brdrs\clbrdrt\brdrw10\brdrs\clbrdrr\brdrw10\brdrs\clbrdrb\brdrw10\brdrs "
                strValoriMultipli = strValoriMultipli & "\cellx10040\pard\intbl\nowidctlpar\ri500\qj\f2\fs15\ " & incremento & "\cell " & dtrLeggiDati("EnteFiglio") & "\cell " & dtrLeggiDati("indirizzo") & "\cell " & dtrLeggiDati("civico") & "\cell " & dtrLeggiDati("comune") & "\cell " & dtrLeggiDati("DescrAbb") & "\cell " & dtrLeggiDati("DettaglioRecapito") & "\cell\row\pard\f2\fs15"
            End While

            strValoriMultipli = strValoriMultipli & "\par"

            incremento = 0

            'chiudo il datareader
            If Not dtrLeggiDati Is Nothing Then
                dtrLeggiDati.Close()
                dtrLeggiDati = Nothing
            End If

        Next

        'passo la stringa con i valori multipli alla funzione per il valore di ritorno
        MON_ElencoVerificheMultiple = strValoriMultipli

        'ritorno la stringa
        Return MON_ElencoVerificheMultiple

    End Function

    Function MON_ElencoRequisiti(ByVal idverificaassociata As Integer) As String
        Dim strsql As String
        Dim strValoriMultipli As String = ""
        Dim dtrLeggiDati As SqlClient.SqlDataReader
        Dim num As Integer = 0
        Dim flag As Integer = 1

        'chiudo il datareader
        If Not dtrLeggiDati Is Nothing Then
            dtrLeggiDati.Close()
            dtrLeggiDati = Nothing
        End If

        strsql = "select idtiporequisito,idrequisito,EsitoDescrizione,Esito,DESCRIZIONE,TIPOREQUISITO from VW_VER_STAMPA_REQUISITI  "
        strsql = strsql & "where idverificheassociate =" & idverificaassociata & ""
        strsql = strsql & "  order by idtiporequisito,idrequisito "

        'eseguo la query e passo il risultato al datareader
        dtrLeggiDati = ClsServer.CreaDatareader(strsql, HttpContext.Current.Session("conn"))

        'ciclo il risultato della query e costruisco la stringa con i valori multipli
        While dtrLeggiDati.Read
            Select Case dtrLeggiDati("idtiporequisito")
                Case 1
                    If flag = 1 Then
                        strValoriMultipli = strValoriMultipli & "\trowd\trgaph70\trleft-70\trbrdrl\brdrs\brdrw10 " & vbCrLf
                        strValoriMultipli = strValoriMultipli & "\trbrdrt\brdrs\brdrw10 \trbrdrr\brdrs\brdrw10 " & vbCrLf
                        strValoriMultipli = strValoriMultipli & "\trbrdrb\brdrs\brdrw10 " & vbCrLf
                        strValoriMultipli = strValoriMultipli & "\trpaddl70\trpaddr70\trpaddfl3\trpaddfr3" & vbCrLf
                        strValoriMultipli = strValoriMultipli & "\clbrdrl\brdrw10\brdrs\clbrdrt\brdrw10\brdrs\clbrdrr\brdrw10\brdrs\clbrdrb\brdrw10\brdrs " & vbCrLf
                        strValoriMultipli = strValoriMultipli & "\cellx10040\pard\intbl\nowidctlpar\ri500\qc\b\fs40\ " & dtrLeggiDati("TipoRequisito") & "\b0\cell\row\pard\f2\fs20" & vbCrLf
                        flag = 2
                    End If
                    Exit Select
                Case 2
                    If flag = 2 Then
                        strValoriMultipli = strValoriMultipli & "\trowd\trgaph70\trleft-70\trbrdrl\brdrs\brdrw10 " & vbCrLf
                        strValoriMultipli = strValoriMultipli & "\trbrdrt\brdrs\brdrw10 \trbrdrr\brdrs\brdrw10 " & vbCrLf
                        strValoriMultipli = strValoriMultipli & "\trbrdrb\brdrs\brdrw10 " & vbCrLf
                        strValoriMultipli = strValoriMultipli & "\trpaddl70\trpaddr70\trpaddfl3\trpaddfr3" & vbCrLf
                        strValoriMultipli = strValoriMultipli & "\clbrdrl\brdrw10\brdrs\clbrdrt\brdrw10\brdrs\clbrdrr\brdrw10\brdrs\clbrdrb\brdrw10\brdrs " & vbCrLf
                        strValoriMultipli = strValoriMultipli & "\cellx10040\pard\intbl\nowidctlpar\ri500\qc\b\fs40\ " & dtrLeggiDati("TipoRequisito") & "\b0\cell\row\pard\f2\fs20" & vbCrLf
                        flag = 3
                    End If
                    Exit Select
                Case 3
                    If flag = 3 Then
                        strValoriMultipli = strValoriMultipli & "\trowd\trgaph70\trleft-70\trbrdrl\brdrs\brdrw10 " & vbCrLf
                        strValoriMultipli = strValoriMultipli & "\trbrdrt\brdrs\brdrw10 \trbrdrr\brdrs\brdrw10 " & vbCrLf
                        strValoriMultipli = strValoriMultipli & "\trbrdrb\brdrs\brdrw10 " & vbCrLf
                        strValoriMultipli = strValoriMultipli & "\trpaddl70\trpaddr70\trpaddfl3\trpaddfr3" & vbCrLf
                        strValoriMultipli = strValoriMultipli & "\clbrdrl\brdrw10\brdrs\clbrdrt\brdrw10\brdrs\clbrdrr\brdrw10\brdrs\clbrdrb\brdrw10\brdrs " & vbCrLf
                        strValoriMultipli = strValoriMultipli & "\cellx10040\pard\intbl\nowidctlpar\ri500\qc\b\fs40\ " & dtrLeggiDati("TipoRequisito") & "\b0\cell\row\pard\f2\fs20" & vbCrLf
                        flag = 4
                    End If
                    Exit Select
                Case 4
                    If flag = 4 Then
                        strValoriMultipli = strValoriMultipli & "\trowd\trgaph70\trleft-70\trbrdrl\brdrs\brdrw10 " & vbCrLf
                        strValoriMultipli = strValoriMultipli & "\trbrdrt\brdrs\brdrw10 \trbrdrr\brdrs\brdrw10 " & vbCrLf
                        strValoriMultipli = strValoriMultipli & "\trbrdrb\brdrs\brdrw10 " & vbCrLf
                        strValoriMultipli = strValoriMultipli & "\trpaddl70\trpaddr70\trpaddfl3\trpaddfr3" & vbCrLf
                        strValoriMultipli = strValoriMultipli & "\clbrdrl\brdrw10\brdrs\clbrdrt\brdrw10\brdrs\clbrdrr\brdrw10\brdrs\clbrdrb\brdrw10\brdrs " & vbCrLf
                        strValoriMultipli = strValoriMultipli & "\cellx10040\pard\intbl\nowidctlpar\ri500\qc\b\fs40\ " & dtrLeggiDati("TipoRequisito") & "\b0\cell\row\pard\f2\fs20" & vbCrLf
                        flag = 5
                    End If
                    Exit Select
            End Select

            Dim esito As String
            esito = dtrLeggiDati("EsitoDescrizione")
            'Select Case dtrLeggiDati("Esito")
            '    Case 1
            '        esito = "SI"
            '    Case 2
            '        esito = "NO"
            '    Case 3
            '        esito = "NON PREVISTO"
            'End Select
            strValoriMultipli = strValoriMultipli & "\trowd\trgaph70\trleft-70\trbrdrl\brdrs\brdrw10 " & vbCrLf
            strValoriMultipli = strValoriMultipli & "\trbrdrt\brdrs\brdrw10 \trbrdrr\brdrs\brdrw10 " & vbCrLf
            strValoriMultipli = strValoriMultipli & "\trbrdrb\brdrs\brdrw10 " & vbCrLf
            strValoriMultipli = strValoriMultipli & "\trpaddl70\trpaddr70\trpaddfl3\trpaddfr3" & vbCrLf
            strValoriMultipli = strValoriMultipli & "\clbrdrl\brdrw10\brdrs\clbrdrt\brdrw10\brdrs\clbrdrr\brdrw10\brdrs\clbrdrb\brdrw10\brdrs " & vbCrLf
            strValoriMultipli = strValoriMultipli & "\cellx8000\clbrdrl\brdrw10\brdrs\clbrdrt\brdrw10\brdrs\clbrdrr\brdrw10\brdrs\clbrdrb\brdrw10\brdrs " & vbCrLf
            strValoriMultipli = strValoriMultipli & "\cellx10040\pard\intbl\nowidctlpar\ri500\qj\fs28\ " & dtrLeggiDati("Descrizione") & "\cell " & esito & "\cell\row\pard\f2\fs20" & vbCrLf
        End While

        strValoriMultipli = strValoriMultipli & "\par" & vbCrLf

        'chiudo il datareader
        If Not dtrLeggiDati Is Nothing Then
            dtrLeggiDati.Close()
            dtrLeggiDati = Nothing
        End If

        'passo la stringa con i valori multipli alla funzione per il valore di ritorno
        MON_ElencoRequisiti = strValoriMultipli

        'ritorno la stringa
        Return MON_ElencoRequisiti

    End Function

    Public Property MON_letteraarchiviazproced(ByVal IdEnte As Integer, ByVal IdVerifica As Integer, ByVal strUserName As String, ByVal intIdCompetenza As Integer, ByVal sqlLocalConn As SqlClient.SqlConnection) As String
        Get
            Dim dtR As DataRow
            Dim dtT As New DataTable

            dtT.Columns.Add(New DataColumn("PARAMETRO", GetType(String)))
            dtT.Columns.Add(New DataColumn("VALORE", GetType(String)))

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDVERIFICA"
            dtR(1) = IdVerifica

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDENTE"
            dtR(1) = IdEnte

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)

            'mod. il 04/10/2016 da simona cordella
            ' passo l'id modello diverso  se vengo da helios o Futuro
            Dim IntModello As Integer
            If Session("Sistema") = "Helios" Then
                IntModello = 97
            Else
                IntModello = 240
            End If
            PathDocumento = EseguiGenerazioneModello(IntModello, strUserName, intIdCompetenza, dtT, sqlLocalConn)

            Return PathDocumento
        End Get
        Set(ByVal Value As String)
            PathDocumento = Value
        End Set
    End Property

    Public Property MON_CANCELLAZIONE(ByVal IdEnte As Integer, ByVal IdVerifica As Integer, ByVal strUserName As String, ByVal intIdCompetenza As Integer, ByVal sqlLocalConn As SqlClient.SqlConnection) As String
        Get
            Dim dtR As DataRow
            Dim dtT As New DataTable

            dtT.Columns.Add(New DataColumn("PARAMETRO", GetType(String)))
            dtT.Columns.Add(New DataColumn("VALORE", GetType(String)))

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDVERIFICA"
            dtR(1) = IdVerifica

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDENTE"
            dtR(1) = IdEnte

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)

            'mod. il 04/10/2016 da simona cordella
            ' passo l'id modello diverso  se vengo da helios o Futuro
            Dim IntModello As Integer
            If Session("Sistema") = "Helios" Then
                IntModello = 98
            Else
                IntModello = 241
            End If

            PathDocumento = EseguiGenerazioneModello(IntModello, strUserName, intIdCompetenza, dtT, sqlLocalConn)

            Return PathDocumento
        End Get
        Set(ByVal Value As String)
            PathDocumento = Value
        End Set
    End Property

    Public Property MON_INTERDIZIONE(ByVal IdEnte As Integer, ByVal IdVerifica As Integer, ByVal strUserName As String, ByVal intIdCompetenza As Integer, ByVal sqlLocalConn As SqlClient.SqlConnection) As String
        Get
            Dim dtR As DataRow
            Dim dtT As New DataTable

            dtT.Columns.Add(New DataColumn("PARAMETRO", GetType(String)))
            dtT.Columns.Add(New DataColumn("VALORE", GetType(String)))

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDVERIFICA"
            dtR(1) = IdVerifica

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDENTE"
            dtR(1) = IdEnte

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)

            'mod. il 04/10/2016 da simona cordella
            ' passo l'id modello diverso  se vengo da helios o Futuro
            Dim IntModello As Integer
            If Session("Sistema") = "Helios" Then
                IntModello = 99
            Else
                IntModello = 242
            End If
            PathDocumento = EseguiGenerazioneModello(IntModello, strUserName, intIdCompetenza, dtT, sqlLocalConn)

            Return PathDocumento
        End Get
        Set(ByVal Value As String)
            PathDocumento = Value
        End Set
    End Property

    Public Property MON_REVOCA(ByVal IdEnte As Integer, ByVal IdVerifica As Integer, ByVal strUserName As String, ByVal intIdCompetenza As Integer, ByVal sqlLocalConn As SqlClient.SqlConnection) As String
        Get
            Dim dtR As DataRow
            Dim dtT As New DataTable

            dtT.Columns.Add(New DataColumn("PARAMETRO", GetType(String)))
            dtT.Columns.Add(New DataColumn("VALORE", GetType(String)))

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDVERIFICA"
            dtR(1) = IdVerifica

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDENTE"
            dtR(1) = IdEnte

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)
            'mod. il 04/10/2016 da simona cordella
            ' passo l'id modello diverso  se vengo da helios o Futuro
            Dim IntModello As Integer
            If Session("Sistema") = "Helios" Then
                IntModello = 100
            Else
                IntModello = 243
            End If
            PathDocumento = EseguiGenerazioneModello(IntModello, strUserName, intIdCompetenza, dtT, sqlLocalConn)

            Return PathDocumento
        End Get
        Set(ByVal Value As String)
            PathDocumento = Value
        End Set
    End Property

    Public Property MON_credenzialiIGF(ByVal IdEnte As Integer, ByVal IdVerifica As Integer, ByVal strUserName As String, ByVal intIdCompetenza As Integer, ByVal sqlLocalConn As SqlClient.SqlConnection) As String
        Get
            Dim dtR As DataRow
            Dim dtT As New DataTable

            dtT.Columns.Add(New DataColumn("PARAMETRO", GetType(String)))
            dtT.Columns.Add(New DataColumn("VALORE", GetType(String)))

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDVERIFICA"
            dtR(1) = IdVerifica

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDENTE"
            dtR(1) = IdEnte

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)

            'mod. il 04/10/2016 da simona cordella
            ' passo l'id modello diverso  se vengo da helios o Futuro
            Dim IntModello As Integer
            If Session("Sistema") = "Helios" Then
                IntModello = 102
            Else
                IntModello = 245
            End If
            PathDocumento = EseguiGenerazioneModello(IntModello, strUserName, intIdCompetenza, dtT, sqlLocalConn)

            Return PathDocumento
        End Get
        Set(ByVal Value As String)
            PathDocumento = Value
        End Set
    End Property

    Public Property MON_LetteradiincaricoIGF(ByVal IdEnte As Integer, ByVal IdVerifica As Integer, ByVal strUserName As String, ByVal intIdCompetenza As Integer, ByVal sqlLocalConn As SqlClient.SqlConnection) As String
        Get
            Dim dtR As DataRow
            Dim dtT As New DataTable

            dtT.Columns.Add(New DataColumn("PARAMETRO", GetType(String)))
            dtT.Columns.Add(New DataColumn("VALORE", GetType(String)))

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDVERIFICA"
            dtR(1) = IdVerifica

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDENTE"
            dtR(1) = IdEnte

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)
            'mod. il 04/10/2016 da simona cordella
            ' passo l'id modello diverso  se vengo da helios o Futuro
            Dim IntModello As Integer
            If Session("Sistema") = "Helios" Then
                IntModello = 104
            Else
                IntModello = 246
            End If
            PathDocumento = EseguiGenerazioneModello(IntModello, strUserName, intIdCompetenza, dtT, sqlLocalConn)

            Return PathDocumento
        End Get
        Set(ByVal Value As String)
            PathDocumento = Value
        End Set
    End Property

    Public Property MON_CONTESTAZIONECHIUSURA(ByVal IdEnte As Integer, ByVal IdVerifica As Integer, ByVal strUserName As String, ByVal intIdCompetenza As Integer, ByVal sqlLocalConn As SqlClient.SqlConnection) As String
        Get
            Dim dtR As DataRow
            Dim dtT As New DataTable

            dtT.Columns.Add(New DataColumn("PARAMETRO", GetType(String)))
            dtT.Columns.Add(New DataColumn("VALORE", GetType(String)))

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDVERIFICA"
            dtR(1) = IdVerifica

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDENTE"
            dtR(1) = IdEnte

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)

            'mod. il 04/10/2016 da simona cordella
            ' passo l'id modello diverso  se vengo da helios o Futuro
            Dim IntModello As Integer
            If Session("Sistema") = "Helios" Then
                IntModello = 101
            Else
                IntModello = 244
            End If

            PathDocumento = EseguiGenerazioneModello(IntModello, strUserName, intIdCompetenza, dtT, sqlLocalConn)

            Return PathDocumento
        End Get
        Set(ByVal Value As String)
            PathDocumento = Value
        End Set
    End Property

    Private Function ParamentriBarraCAD(ByVal intIdModello As Integer, ByVal xlinea As String, ByVal Parametri As DataTable, Optional ByRef CodiceSede As Integer = 0) As String
        '** CREATA DA SIMONA CORDELLA IL 02/11/2010
        '** MODIFICO DOCUMENTI DELLE VERIFICHE
        '** Ricavo i paramentri che vado a scrivere nel documento .rtf
        '** ChiaveID = (idprogrammazione o idverifica)
        '** IdSoggetto =0 (sede) 
        '**            >0 EN (idente) se è presente il codice SA (sedeattuazione) o altro con il CodiceSedee nel documento è presente l'indirizzo del destinatario
        Dim strH As String = "H"
        Dim Sede As Integer = 0
        Dim ChiaveID As Integer
        Dim IdSoggetto As String
        Dim IdFascicolo As String = 0
        Dim IdServizio As String = ""
        Dim Classificazione As String
        Dim Titolario As String
        Dim Determina As String
        Dim IdBando As Integer

        '** aggiunto da simona cordella il 21/01/2011
        Classificazione = TrovaClassificazioneModelli(intIdModello, HttpContext.Current.Session("Conn"))
        Titolario = Mid(Classificazione, 1, InStr(Classificazione, "#") - 1)
        Determina = Mid(Classificazione, InStr(Classificazione, "#") + 1)

        '***
        xlinea = Mid(xlinea, 1, xlinea.Length - 1)
        xlinea = xlinea & "{\*\docvar {Sistema}{" & strH & "}}"

        Select Case intIdModello
            Case 78, 225 'trasmissioneprogrammazione.rtf
                Dim dttRowParametri As DataRow
                For Each dttRowParametri In Parametri.Rows
                    If UCase("IdProgrammazione") = UCase(dttRowParametri.Item("PARAMETRO")) Then
                        ChiaveID = dttRowParametri.Item("VALORE")
                    End If
                Next
                IdSoggetto = 0
                IdFascicolo = TrovaIdFascicoloTVerificheProgrammazione(ChiaveID, HttpContext.Current.Session("Conn"))
            Case 79, 226 'letterainterlocutoria.rtf
                Dim dttRowParametri As DataRow
                For Each dttRowParametri In Parametri.Rows
                    If UCase("IdVerifica") = UCase(dttRowParametri.Item("PARAMETRO")) Then
                        ChiaveID = dttRowParametri.Item("VALORE")
                    End If
                    If UCase("IdEnte") = UCase(dttRowParametri.Item("PARAMETRO")) Then
                        IdSoggetto = "EN" & dttRowParametri.Item("VALORE") & "#" & "SA" & CodiceSede
                    End If
                Next
                IdFascicolo = TrovaIdFascicoloTVerifiche(ChiaveID, HttpContext.Current.Session("Conn"))
            Case 109, 247 'letteraincaricomultipla.rtf
                Dim dttRowParametri As DataRow
                For Each dttRowParametri In Parametri.Rows
                    If UCase("IdGruppoStampa") = UCase(dttRowParametri.Item("PARAMETRO")) Then
                        ChiaveID = dttRowParametri.Item("VALORE")
                    End If
                Next
                IdSoggetto = 0
            Case 80, 227  'credenziali.rtf
                Dim dttRowParametri As DataRow
                For Each dttRowParametri In Parametri.Rows
                    If UCase("IdVerifica") = UCase(dttRowParametri.Item("PARAMETRO")) Then
                        ChiaveID = dttRowParametri.Item("VALORE")
                    End If
                    If UCase("IdEnte") = UCase(dttRowParametri.Item("PARAMETRO")) Then
                        IdSoggetto = "EN" & dttRowParametri.Item("VALORE") & "#" & "SA" & CodiceSede
                    End If
                Next
                IdFascicolo = TrovaIdFascicoloTVerifiche(ChiaveID, HttpContext.Current.Session("Conn"))
            Case 81, 228 'letteradiincarico.rtf
                Dim dttRowParametri As DataRow
                For Each dttRowParametri In Parametri.Rows
                    If UCase("IdVerifica") = UCase(dttRowParametri.Item("PARAMETRO")) Then
                        ChiaveID = dttRowParametri.Item("VALORE")
                    End If
                Next
                IdSoggetto = 0
                IdFascicolo = TrovaIdFascicoloTVerifiche(ChiaveID, HttpContext.Current.Session("Conn"))
            Case 102, 245 'credenzialiIGF.rtf
                Dim dttRowParametri As DataRow
                For Each dttRowParametri In Parametri.Rows
                    If UCase("IdVerifica") = UCase(dttRowParametri.Item("PARAMETRO")) Then
                        ChiaveID = dttRowParametri.Item("VALORE")
                    End If
                    If UCase("IdEnte") = UCase(dttRowParametri.Item("PARAMETRO")) Then
                        IdSoggetto = "EN" & dttRowParametri.Item("VALORE") & "#" & "SA" & CodiceSede
                    End If
                Next
                IdFascicolo = TrovaIdFascicoloTVerifiche(ChiaveID, HttpContext.Current.Session("Conn"))
            Case 104, 246 'letteradiincaricoIGF.rtf
                Dim dttRowParametri As DataRow
                For Each dttRowParametri In Parametri.Rows
                    If UCase("IdVerifica") = UCase(dttRowParametri.Item("PARAMETRO")) Then
                        ChiaveID = dttRowParametri.Item("VALORE")
                    End If
                Next
                IdSoggetto = 0
                IdFascicolo = TrovaIdFascicoloTVerifiche(ChiaveID, HttpContext.Current.Session("Conn"))
            Case 83, 230 'conclusioneVerificaPositiva.rtf
                Dim dttRowParametri As DataRow
                For Each dttRowParametri In Parametri.Rows
                    If UCase("IdVerifica") = UCase(dttRowParametri.Item("PARAMETRO")) Then
                        ChiaveID = dttRowParametri.Item("VALORE")
                    End If
                    If UCase("IdEnte") = UCase(dttRowParametri.Item("PARAMETRO")) Then
                        IdSoggetto = "EN" & dttRowParametri.Item("VALORE")
                        'mod. il 24/05/2012 da s.c. la lettere viene inviata solo all'ente e nn più alla sede di progetto 
                        '& "#" & "SA" & CodiceSede
                    End If
                Next
                IdFascicolo = TrovaIdFascicoloTVerifiche(ChiaveID, HttpContext.Current.Session("Conn"))
            Case 84, 231 'VerificasenzaIrregolaritàConRichiamo.rtf
                Dim dttRowParametri As DataRow
                For Each dttRowParametri In Parametri.Rows
                    If UCase("IdVerifica") = UCase(dttRowParametri.Item("PARAMETRO")) Then
                        ChiaveID = dttRowParametri.Item("VALORE")
                    End If
                    If UCase("IdEnte") = UCase(dttRowParametri.Item("PARAMETRO")) Then
                        IdSoggetto = "EN" & dttRowParametri.Item("VALORE")
                        'mod. il 24/05/2012 da s.c. la lettere viene inviata solo all'ente e nn più alla sede di progetto 
                        '& "#" & "SA" & CodiceSede
                    End If
                Next
                IdFascicolo = TrovaIdFascicoloTVerifiche(ChiaveID, HttpContext.Current.Session("Conn"))
            Case 82, 229 'trasmisisonerelazionealDG.rtf
                Dim dttRowParametri As DataRow
                For Each dttRowParametri In Parametri.Rows
                    If UCase("IdVerifica") = UCase(dttRowParametri.Item("PARAMETRO")) Then
                        ChiaveID = dttRowParametri.Item("VALORE")
                    End If
                Next
                IdSoggetto = 0
                IdFascicolo = TrovaIdFascicoloTVerifiche(ChiaveID, HttpContext.Current.Session("Conn"))
            Case 93, 236 'trasmissionerelazioneconrichiamo.rtf
                Dim dttRowParametri As DataRow
                For Each dttRowParametri In Parametri.Rows
                    If UCase("IdVerifica") = UCase(dttRowParametri.Item("PARAMETRO")) Then
                        ChiaveID = dttRowParametri.Item("VALORE")
                    End If
                Next
                IdSoggetto = 0
                IdFascicolo = TrovaIdFascicoloTVerifiche(ChiaveID, HttpContext.Current.Session("Conn"))
            Case 85, 232 'LetteraContestazioneAddebiti.rtf
                'NO CODICE SEDE
                Dim dttRowParametri As DataRow
                For Each dttRowParametri In Parametri.Rows
                    If UCase("IdVerifica") = UCase(dttRowParametri.Item("PARAMETRO")) Then
                        ChiaveID = dttRowParametri.Item("VALORE")
                    End If
                    If UCase("IdEnte") = UCase(dttRowParametri.Item("PARAMETRO")) Then
                        IdSoggetto = "EN" & dttRowParametri.Item("VALORE")
                        '& "#" & "SA" & CodiceSede
                    End If
                Next
                IdFascicolo = TrovaIdFascicoloTVerifiche(ChiaveID, HttpContext.Current.Session("Conn"))
            Case 87, 233 'letteratrasmdgcontestazione.rtf
                Dim dttRowParametri As DataRow
                For Each dttRowParametri In Parametri.Rows
                    If UCase("IdVerifica") = UCase(dttRowParametri.Item("PARAMETRO")) Then
                        ChiaveID = dttRowParametri.Item("VALORE")
                    End If
                Next
                IdSoggetto = 0
                IdFascicolo = TrovaIdFascicoloTVerifiche(ChiaveID, HttpContext.Current.Session("Conn"))
            Case 101, 244 'ContestazioneChiusura.rtf
                Dim dttRowParametri As DataRow
                'NO CODICE SEDE
                For Each dttRowParametri In Parametri.Rows
                    If UCase("IdVerifica") = UCase(dttRowParametri.Item("PARAMETRO")) Then
                        ChiaveID = dttRowParametri.Item("VALORE")
                    End If
                    If UCase("IdEnte") = UCase(dttRowParametri.Item("PARAMETRO")) Then
                        IdSoggetto = "EN" & dttRowParametri.Item("VALORE")
                        '& "#" & "SA" & CodiceSede
                    End If
                Next
                IdFascicolo = TrovaIdFascicoloTVerifiche(ChiaveID, HttpContext.Current.Session("Conn"))
            Case 111, 248 'TrasmissioneSanzioneDG.rtf
                Dim dttRowParametri As DataRow
                For Each dttRowParametri In Parametri.Rows
                    If UCase("IdVerifica") = UCase(dttRowParametri.Item("PARAMETRO")) Then
                        ChiaveID = dttRowParametri.Item("VALORE")
                    End If
                Next
                IdSoggetto = 0
                IdFascicolo = TrovaIdFascicoloTVerifiche(ChiaveID, HttpContext.Current.Session("Conn"))
            Case 92, 235 'LetteraTRasmissioneProvvedimentoSanzionatorio.rtf
                Dim dttRowParametri As DataRow
                'NO CODICE SEDE
                For Each dttRowParametri In Parametri.Rows
                    If UCase("IdVerifica") = UCase(dttRowParametri.Item("PARAMETRO")) Then
                        ChiaveID = dttRowParametri.Item("VALORE")
                    End If
                    If UCase("IdEnte") = UCase(dttRowParametri.Item("PARAMETRO")) Then
                        IdSoggetto = "EN" & dttRowParametri.Item("VALORE")
                        '& "#" & "SA" & CodiceSede
                    End If
                Next
                IdFascicolo = TrovaIdFascicoloTVerifiche(ChiaveID, HttpContext.Current.Session("Conn"))
            Case 94, 237 'letteatrasmisprovaiservizi.rtf
                Dim dttRowParametri As DataRow
                For Each dttRowParametri In Parametri.Rows
                    If UCase("IdVerifica") = UCase(dttRowParametri.Item("PARAMETRO")) Then
                        ChiaveID = dttRowParametri.Item("VALORE")
                    End If
                Next
                IdSoggetto = 0
                IdFascicolo = TrovaIdFascicoloTVerifiche(ChiaveID, HttpContext.Current.Session("Conn"))


                'ricavo l'idservizo solo per questo documento
                IdServizio = TrovaIDServizioSanzione(ChiaveID, 94, HttpContext.Current.Session("Conn"))


            Case 35 To 43
                'gestione della documentazione dei Progetti id modelli interessati (35 ,36,37,38,39,40,41,42,43)
                'aggiounto da simona cordella il 28/05/2012

                Dim dttRowParametri As DataRow
                'NO CODICE SEDE
                For Each dttRowParametri In Parametri.Rows

                    If UCase("IdEnte") = UCase(dttRowParametri.Item("PARAMETRO")) Then
                        IdSoggetto = "EN" & dttRowParametri.Item("VALORE")
                        '& "#" & "SA" & CodiceSede
                    End If
                    If UCase("IdBando") = UCase(dttRowParametri.Item("PARAMETRO")) Then
                        IdBando = dttRowParametri.Item("VALORE")
                        'ChiaveID = dttRowParametri.Item("VALORE")
                    End If
                Next
                ChiaveID = RecuperaIdBandoAttività(IdBando, HttpContext.Current.Session("IdEnte"), HttpContext.Current.Session("Conn"))
                IdFascicolo = TrovaIdFascicoloProgetti(ChiaveID, HttpContext.Current.Session("Conn"))
            Case 269 To 276
                'gestione della documentazione dei programmi id modelli interessati (269,270,271,272,273,274,275,276)
                'aggiounto il 16/11/2020

                Dim dttRowParametri As DataRow
                'NO CODICE SEDE
                For Each dttRowParametri In Parametri.Rows

                    If UCase("IdEnte") = UCase(dttRowParametri.Item("PARAMETRO")) Then
                        IdSoggetto = "EN" & dttRowParametri.Item("VALORE")
                        '& "#" & "SA" & CodiceSede
                    End If
                    If UCase("IdBando") = UCase(dttRowParametri.Item("PARAMETRO")) Then
                        IdBando = dttRowParametri.Item("VALORE")
                        'ChiaveID = dttRowParametri.Item("VALORE")
                    End If
                Next
                ChiaveID = RecuperaIdBandoAttività(IdBando, HttpContext.Current.Session("IdEnte"), HttpContext.Current.Session("Conn"))
                IdFascicolo = TrovaIdFascicoloProgetti(ChiaveID, HttpContext.Current.Session("Conn"))
                '****************** LETTERE PER IL PROVVEDIMENTO DISCIPLINARE VOLONTARI *******************
            Case 258 To 263
                'gestione della documentazione dei provvedimenti disciplinare id modelli interessati (258,259,260,261,262,263)
                Dim dttRowParametri As DataRow
                'NO CODICE SEDE
                For Each dttRowParametri In Parametri.Rows
                    If UCase("IDProvvedimentoDisciplinare") = UCase(dttRowParametri.Item("PARAMETRO")) Then
                        ChiaveID = dttRowParametri.Item("VALORE")
                    End If
                    If UCase("IdEnte") = UCase(dttRowParametri.Item("PARAMETRO")) Then
                        IdSoggetto = "EN" & dttRowParametri.Item("VALORE") '& "#" & "VO" & eD
                    End If
                    If UCase("IdEntita") = UCase(dttRowParametri.Item("PARAMETRO")) Then
                        IdSoggetto = IdSoggetto & "#" & "VO" & dttRowParametri.Item("VALORE") '& "#" & "VO" & eD
                    End If
                Next
                IdFascicolo = TrovaIdFascicoloProvvedimentoDisciplinare(ChiaveID, HttpContext.Current.Session("Conn"))

                '****************** FINE PROVVEDIMENTO DISCIPLINARE VOLONTARI *******************

            Case 27 To 30 ' Aggiunto da Luigi Leucci il 27/02/2019
                ' Art 2 ed art 10 per fase di accreditamento o Adeguamento
                Dim dttRowParametri As DataRow

                For Each dttRowParametri In Parametri.Rows
                    If UCase("IDENTEFASE") = UCase(dttRowParametri.Item("PARAMETRO")) Then
                        ChiaveID = dttRowParametri.Item("VALORE")
                    End If
                    If UCase("IdEnte") = UCase(dttRowParametri.Item("PARAMETRO")) Then
                        IdSoggetto = "EN" & dttRowParametri.Item("VALORE") '& "#" & "VO" & eD
                    End If
                Next

            Case 16, 24, 26, 268 ' Aggiunto il 27/02/2019
                ' Decreti Iscrizione Albo SCU
                Dim dttRowParametri As DataRow

                For Each dttRowParametri In Parametri.Rows
                    If UCase("IdEnte") = UCase(dttRowParametri.Item("PARAMETRO")) Then
                        ChiaveID = dttRowParametri.Item("VALORE")
                        IdSoggetto = "EN" & dttRowParametri.Item("VALORE")
                    End If
                Next

            Case 10, 12 ' Aggiunto il 11/07/2021
                ' Lettere avvio procedimento Albo SCU
                Dim dttRowParametri As DataRow

                For Each dttRowParametri In Parametri.Rows
                    If UCase("IDENTEFASE") = UCase(dttRowParametri.Item("PARAMETRO")) Then
                        ChiaveID = dttRowParametri.Item("VALORE")
                    End If
                    If UCase("IdEnte") = UCase(dttRowParametri.Item("PARAMETRO")) Then
                        IdSoggetto = "EN" & dttRowParametri.Item("VALORE") '& "#" & "VO" & eD
                    End If
                Next
            Case 300 ' Aggiunto il 11/07/2021
                ' Esito istanza sostituzione OLP
                Dim dttRowParametri As DataRow

                For Each dttRowParametri In Parametri.Rows
                    If UCase("IdIstanzaSostituzioneOLP") = UCase(dttRowParametri.Item("PARAMETRO")) Then
                        ChiaveID = dttRowParametri.Item("VALORE")
                    End If
                    If UCase("IdEnte") = UCase(dttRowParametri.Item("PARAMETRO")) Then
                        IdSoggetto = "EN" & dttRowParametri.Item("VALORE") '& "#" & "VO" & eD
                    End If
                Next
        End Select
        xlinea = xlinea & "{\*\docvar {ChiaveID}{" & ChiaveID & "}}"
        xlinea = xlinea & "{\*\docvar {IdModello}{" & intIdModello & "}}"
        xlinea = xlinea & "{\*\docvar {IdSoggetto}{" & IdSoggetto & "}}"
        '*** modificato il 21/01/2011 (IdFascicolo, Titolario e TipoDocumento(Lettera o Determina)
        xlinea = xlinea & "{\*\docvar {IdFascicolo}{" & IdFascicolo & "}}"
        xlinea = xlinea & "{\*\docvar {Titolario}{" & Titolario & "}}"
        xlinea = xlinea & "{\*\docvar {TipoDocumento}{" & Determina & "}}"
        If IdServizio <> "" Then
            xlinea = xlinea & "{\*\docvar {IdServizio}{" & IdServizio & "}}"
        End If
        '****
        xlinea = xlinea & "}"
        Return xlinea
    End Function

    Private Function TrovaIdFascicoloTVerifiche(ByVal IdVerifica As Integer, ByVal sqlLocalConn As SqlClient.SqlConnection) As String
        'Creato da Simona Cordella il 21/01/2011
        'Trovo l'idfascicolo nella tabella TVerifiche
        'modificato il 02/02/2012 normalizzo l'id fascicolo di tverifche con l'id fasccilo completo di siged
        'function di CLSSIGED SIGED_IdFascicoloCompleto()
        Dim IdFascicolo As String
        Dim dtrIdFasc As SqlClient.SqlDataReader
        Dim strsql As String
        Dim SIGED As New clsSiged
        strsql = "Select isnull(IdFascicolo,0)as IdFascicolo  From TVerifiche where idVerifica = " & IdVerifica
        dtrIdFasc = ClsServer.CreaDatareader(strsql, sqlLocalConn)
        If dtrIdFasc.HasRows = True Then
            dtrIdFasc.Read()
            IdFascicolo = dtrIdFasc("IdFascicolo")
        End If
        dtrIdFasc.Close()
        dtrIdFasc = Nothing
        Return SIGED.SIGED_IdFascicoloCompleto(IdFascicolo)
    End Function

    Private Function TrovaIdFascicoloTVerificheProgrammazione(ByVal IdProgrammazione As Integer, ByVal sqlLocalConn As SqlClient.SqlConnection) As String
        'Creato da Simona Cordella il 21/01/2011
        'Trovo l'idfascicolo nella tabella TVerificheProgrammazione
        Dim IdFascicolo As String
        Dim dtrIdFasc As SqlClient.SqlDataReader
        Dim strsql As String

        strsql = "Select isnull(IdFascicolo,0)as IdFascicolo  From TVerificheProgrammazione where IdProgrammazione = " & IdProgrammazione
        dtrIdFasc = ClsServer.CreaDatareader(strsql, sqlLocalConn)
        If dtrIdFasc.HasRows = True Then
            dtrIdFasc.Read()
            IdFascicolo = dtrIdFasc("IdFascicolo")
        End If
        dtrIdFasc.Close()
        dtrIdFasc = Nothing
        Return IdFascicolo
    End Function

    Private Function TrovaClassificazioneModelli(ByVal IdModello As Integer, ByVal sqlLocalConn As SqlClient.SqlConnection) As String
        'Creato da Simona Cordella il 21/01/2011
        'Trovo il titolario a secondo del modello che ho selezionato
        'LA variabile Classificazione riporta il titolario e la determina divisi dal #
        Dim Classificazione As String

        Dim dtrClass As SqlClient.SqlDataReader
        Dim strsql As String

        strsql = "Select isnull(Titolario,'') as Titolario,isnull(TipoDocumento,'') as TipoDocumento From Editor_Modelli where idModello = " & IdModello
        dtrClass = ClsServer.CreaDatareader(strsql, sqlLocalConn)
        If dtrClass.HasRows = True Then
            dtrClass.Read()
            Classificazione = dtrClass("Titolario") & "#" & dtrClass("TipoDocumento")
        End If
        dtrClass.Close()
        dtrClass = Nothing
        Return Classificazione
    End Function

    Private Function TrovaIDServizioSanzione(ByVal IdVerifica As Integer, ByVal IDModello As Integer, ByVal sqlLocalConn As SqlClient.SqlConnection) As String
        'Creato da Simona Cordella il 04/03/2011
        'Trovo l'idserivio per le verifiche sanzionate 
        'viene stampato solo x il documento 94 (letteatrasmisprovaiservizi.rtf)
        Dim IdSerivizo As String = ""
        Dim strsql As String
        Dim dtrServizi As SqlClient.SqlDataReader

        strsql = " Select isnull(IdServizio,0)as IdServizio from TVerificheServizi where IdVerifica = " & IdVerifica & " and IDModello= " & IDModello & ""
        dtrServizi = ClsServer.CreaDatareader(strsql, sqlLocalConn)
        Do While dtrServizi.Read()
            If IdSerivizo = "" Then
                IdSerivizo = dtrServizi("IdServizio")
            Else
                IdSerivizo = IdSerivizo & "#" & dtrServizi("IdServizio")
            End If
        Loop
        dtrServizi.Close()
        dtrServizi = Nothing
        Return IdSerivizo
    End Function

    Private Function RecuperaIdBandoAttività(ByVal IDBando As Integer, ByVal IdEnte As Integer, ByVal sqlLocalConn As SqlClient.SqlConnection) As Integer
        'Creato da Simona Cordella il 25/05/2012
        'Tova idbandoattivita per la stampa della documentazione dei progetti

        Dim strsql As String
        Dim dtrBandi As SqlClient.SqlDataReader
        Dim IdBA As Integer

        strsql = " select idbandoattività from bandiattività where idbando = " & IDBando & " and idente = " & IdEnte & ""
        dtrBandi = ClsServer.CreaDatareader(strsql, sqlLocalConn)
        If dtrBandi.HasRows = True Then
            dtrBandi.Read()
            IdBA = dtrBandi("idbandoattività")
        End If
        dtrBandi.Close()
        dtrBandi = Nothing
        Return IdBA
    End Function

    Private Function TrovaIdFascicoloProgetti(ByVal IDBandoAttivita As Integer, ByVal sqlLocalConn As SqlClient.SqlConnection) As String
        'Creato da Simona Cordella il 28/05/2012
        'Trovo l'idfascicolo nella tabella TVerifiche
        'modificato il 02/02/2012 normalizzo l'id fascicolo di tverifche con l'id fasccilo completo di siged
        'function di CLSSIGED SIGED_IdFascicoloCompleto()
        Dim IdFascicolo As String
        Dim dtrIdFasc As SqlClient.SqlDataReader
        Dim strsql As String
        Dim SIGED As New clsSiged
        strsql = "Select isnull(IdFascicoloPC,0)as IdFascicolo  From bandiattività where idbandoattività = " & IDBandoAttivita
        dtrIdFasc = ClsServer.CreaDatareader(strsql, sqlLocalConn)
        If dtrIdFasc.HasRows = True Then
            dtrIdFasc.Read()
            IdFascicolo = dtrIdFasc("IdFascicolo")
        End If
        dtrIdFasc.Close()
        dtrIdFasc = Nothing
        If IdFascicolo = "0" Then
            Return IdFascicolo
        Else
            Return SIGED.SIGED_IdFascicoloCompleto(IdFascicolo)
        End If
    End Function

    Private Function TrovaIdFascicoloProvvedimentoDisciplinare(ByVal IdProvvedimentoDisciplinare As Integer, ByVal sqlLocalConn As SqlClient.SqlConnection) As String
        'Creato da Simona Cordella il 05/04/2018
        'Trovo l'idfascicolo nella tabella ProvvedimentoDisciplinare
         
        Dim IdFascicolo As String
        Dim dtrIdFasc As SqlClient.SqlDataReader
        Dim strsql As String
        Dim SIGED As New clsSiged
        strsql = "Select isnull(IdFascicolo,0)as IdFascicolo  From ProvvedimentoDisciplinare where IdProvvedimentoDisciplinare = " & IdProvvedimentoDisciplinare
        dtrIdFasc = ClsServer.CreaDatareader(strsql, sqlLocalConn)
        If dtrIdFasc.HasRows = True Then
            dtrIdFasc.Read()
            IdFascicolo = dtrIdFasc("IdFascicolo")
        End If
        dtrIdFasc.Close()
        dtrIdFasc = Nothing
        Return SIGED.SIGED_IdFascicoloCompleto(IdFascicolo)
    End Function


    Function VOL_DataMatrix(ByVal IdEntità As Integer, ByVal localConn As SqlClient.SqlConnection) As String
        Dim strsql As String
        Dim strCodiceVolontario As String = ""
        Dim myCommand As SqlClient.SqlCommand
        Dim dtrVol As SqlClient.SqlDataReader

        Dim strValoriMultipli As String = ""
        Dim chiamataWSHeliosUtility As New WSHeliosUtility.WSHeliosUtility
        chiamataWSHeliosUtility.Url = AppSettings("URL_WS_HeliosUtility")


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
    Public Property ACCR_DeterminaAdeguamentoPositivoTot(ByVal intIdEnteFase As Integer, ByVal intIdEnte As Integer, ByVal strUserName As String, ByVal intIdCompetenza As Integer, ByVal sqlLocalConn As SqlClient.SqlConnection) As String
        Get
            Dim dtR As DataRow
            Dim dtT As New DataTable

            dtT.Columns.Add(New DataColumn("PARAMETRO", GetType(String)))
            dtT.Columns.Add(New DataColumn("VALORE", GetType(String)))

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDENTE"
            dtR(1) = intIdEnte

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)


            '**Aggiunto in 08/09/2015 da simona cordella 
            'alla riga assegno L'IDENTEFASE
            dtR = dtT.NewRow()

            ''carico la prima riga della datatable
            'dtR(0) = "IDENTEFASE"
            'dtR(1) = intIdEnteFase


            ''aggiungo la riga appena caricata alla datatable
            'dtT.Rows.Add(dtR)
            ''**

            PathDocumento = EseguiGenerazioneModello(217, strUserName, intIdCompetenza, dtT, sqlLocalConn)

            Return PathDocumento
        End Get
        Set(ByVal Value As String)
            PathDocumento = Value
        End Set
    End Property
    Public Property ACCR_DeterminaAdeguamentoPositivoArt10Tot(ByVal intIdenteFase As Integer, ByVal intIdEnte As Integer, ByVal strUserName As String, ByVal intIdCompetenza As Integer, ByVal sqlLocalConn As SqlClient.SqlConnection) As String
        Get
            Dim dtR As DataRow
            Dim dtT As New DataTable

            dtT.Columns.Add(New DataColumn("PARAMETRO", GetType(String)))
            dtT.Columns.Add(New DataColumn("VALORE", GetType(String)))

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDENTE"
            dtR(1) = intIdEnte

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)


            ''**Aggiunto in 08/09/2015 da simona cordella 
            ''alla riga assegno L'IDENTEFASE
            'dtR = dtT.NewRow()

            ''carico la prima riga della datatable
            'dtR(0) = "IDENTEFASE"
            'dtR(1) = intIdenteFase


            ''aggiungo la riga appena caricata alla datatable
            'dtT.Rows.Add(dtR)
            ''**

            PathDocumento = EseguiGenerazioneModello(218, strUserName, intIdCompetenza, dtT, sqlLocalConn)

            Return PathDocumento
        End Get
        Set(ByVal Value As String)
            PathDocumento = Value
        End Set
    End Property
    Public Property ACCR_DeterminaAdeguamentoPositivoconLimitiTot(ByVal IntIdEnteFase As Integer, ByVal intIdEnte As Integer, ByVal strUserName As String, ByVal intIdCompetenza As Integer, ByVal sqlLocalConn As SqlClient.SqlConnection) As String
        Get
            Dim dtR As DataRow
            Dim dtT As New DataTable

            dtT.Columns.Add(New DataColumn("PARAMETRO", GetType(String)))
            dtT.Columns.Add(New DataColumn("VALORE", GetType(String)))

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDENTE"
            dtR(1) = intIdEnte

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)


            ''**Aggiunto in 08/09/2015 da simona cordella 
            ''alla riga assegno L'IDENTEFASE
            'dtR = dtT.NewRow()

            ''carico la prima riga della datatable
            'dtR(0) = "IDENTEFASE"
            'dtR(1) = IntIdEnteFase


            ''aggiungo la riga appena caricata alla datatable
            'dtT.Rows.Add(dtR)
            ''**

            PathDocumento = EseguiGenerazioneModello(219, strUserName, intIdCompetenza, dtT, sqlLocalConn)

            Return PathDocumento
        End Get
        Set(ByVal Value As String)
            PathDocumento = Value
        End Set
    End Property
    Public Property ACCR_DeterminaAdeguamentoNegativoTot(ByVal intIdEnteFase As Integer, ByVal intIdEnte As Integer, ByVal strUserName As String, ByVal intIdCompetenza As Integer, ByVal sqlLocalConn As SqlClient.SqlConnection) As String
        Get
            Dim dtR As DataRow
            Dim dtT As New DataTable

            dtT.Columns.Add(New DataColumn("PARAMETRO", GetType(String)))
            dtT.Columns.Add(New DataColumn("VALORE", GetType(String)))

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDENTE"
            dtR(1) = intIdEnte
            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)


            ''**Aggiunto in 08/09/2015 da simona cordella 
            ''alla riga assegno L'IDENTEFASE
            'dtR = dtT.NewRow()

            ''carico la prima riga della datatable
            'dtR(0) = "IDENTEFASE"
            'dtR(1) = intIdEnteFase


            ''aggiungo la riga appena caricata alla datatable
            'dtT.Rows.Add(dtR)
            ''**

            PathDocumento = EseguiGenerazioneModello(221, strUserName, intIdCompetenza, dtT, sqlLocalConn)

            Return PathDocumento
        End Get
        Set(ByVal Value As String)
            PathDocumento = Value
        End Set
    End Property
    Public Property ACCR_allegatoA1AdeguamentoTot(ByVal intIdEnteFase As Integer, ByVal intIdEnte As Integer, ByVal strUserName As String, ByVal intIdCompetenza As Integer, ByVal sqlLocalConn As SqlClient.SqlConnection) As String
        Get
            Dim dtR As DataRow
            Dim dtT As New DataTable

            dtT.Columns.Add(New DataColumn("PARAMETRO", GetType(String)))
            dtT.Columns.Add(New DataColumn("VALORE", GetType(String)))

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDENTE"
            dtR(1) = intIdEnte

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)


            ''**Aggiunto in 08/09/2015 da simona cordella 
            ''alla riga assegno L'IDENTEFASE
            'dtR = dtT.NewRow()

            ''carico la prima riga della datatable
            'dtR(0) = "IDENTEFASE"
            'dtR(1) = intIdEnteFase


            ''aggiungo la riga appena caricata alla datatable
            'dtT.Rows.Add(dtR)
            ''**

            PathDocumento = EseguiGenerazioneModello(222, strUserName, intIdCompetenza, dtT, sqlLocalConn)

            Return PathDocumento
        End Get
        Set(ByVal Value As String)
            PathDocumento = Value
        End Set
    End Property
    Public Property ACCR_allegatoA2AdeguamentoTot(ByVal intIdEnteFase As Integer, ByVal intIdEnte As Integer, ByVal strUserName As String, ByVal intIdCompetenza As Integer, ByVal sqlLocalConn As SqlClient.SqlConnection) As String
        Get
            Dim dtR As DataRow
            Dim dtT As New DataTable

            dtT.Columns.Add(New DataColumn("PARAMETRO", GetType(String)))
            dtT.Columns.Add(New DataColumn("VALORE", GetType(String)))

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDENTE"
            dtR(1) = intIdEnte

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)


            ''**Aggiunto in 08/09/2015 da simona cordella 
            ''alla riga assegno L'IDENTEFASE
            'dtR = dtT.NewRow()

            ''carico la prima riga della datatable
            'dtR(0) = "IDENTEFASE"
            'dtR(1) = intIdEnteFase


            ''aggiungo la riga appena caricata alla datatable
            'dtT.Rows.Add(dtR)
            ''**

            PathDocumento = EseguiGenerazioneModello(223, strUserName, intIdCompetenza, dtT, sqlLocalConn)

            Return PathDocumento
        End Get
        Set(ByVal Value As String)
            PathDocumento = Value
        End Set
    End Property
    Public Property ACCR_allegatobAdeguamentoTot(ByVal intIdEnteFase As Integer, ByVal intIdEnte As Integer, ByVal strUserName As String, ByVal intIdCompetenza As Integer, ByVal sqlLocalConn As SqlClient.SqlConnection) As String
        Get
            Dim dtR As DataRow
            Dim dtT As New DataTable

            dtT.Columns.Add(New DataColumn("PARAMETRO", GetType(String)))
            dtT.Columns.Add(New DataColumn("VALORE", GetType(String)))

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDENTE"
            dtR(1) = intIdEnte

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)


            ''**Aggiunto in 08/09/2015 da simona cordella 
            ''alla riga assegno L'IDENTEFASE
            'dtR = dtT.NewRow()

            ''carico la prima riga della datatable
            'dtR(0) = "IDENTEFASE"
            'dtR(1) = intIdEnteFase


            ''aggiungo la riga appena caricata alla datatable
            'dtT.Rows.Add(dtR)
            ''**

            PathDocumento = EseguiGenerazioneModello(224, strUserName, intIdCompetenza, dtT, sqlLocalConn)

            Return PathDocumento
        End Get
        Set(ByVal Value As String)
            PathDocumento = Value
        End Set
    End Property

    Public Property MON_AvvioProvvedimentoDisciplinare(ByVal IdProvvedimentoDisciplinare As Integer, ByVal IdEnte As Integer, ByVal IdEntita As Integer, ByVal strUserName As String, ByVal intIdCompetenza As Integer, ByVal sqlLocalConn As SqlClient.SqlConnection) As String
        'creato il 04/04/2018 lettera di avvio provvedimento disciplinare ad un volontario
        Get
            Dim dtR As DataRow
            Dim dtT As New DataTable

            dtT.Columns.Add(New DataColumn("PARAMETRO", GetType(String)))
            dtT.Columns.Add(New DataColumn("VALORE", GetType(String)))

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDPROVVEDIMENTODISCIPLINARE"
            dtR(1) = IdProvvedimentoDisciplinare

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDENTE"
            dtR(1) = IdEnte

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDENTITA"
            dtR(1) = IdEntita

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)

            'mod. il 04/10/2016 da simona cordella
            ' passo l'id modello diverso  se vengo da helios o Futuro
            Dim IntModello As Integer
            If Session("Sistema") = "Helios" Then
                IntModello = 258
            Else
                IntModello = 259
            End If

            PathDocumento = EseguiGenerazioneModello(IntModello, strUserName, intIdCompetenza, dtT, sqlLocalConn)

            Return PathDocumento
        End Get
        Set(ByVal Value As String)
            PathDocumento = Value
        End Set
    End Property

    Public Property MON_ChiusuraProvvedimentoDisciplinare(ByVal IdProvvedimentoDisciplinare As Integer, ByVal IdEnte As Integer, ByVal IdEntita As Integer, ByVal strUserName As String, ByVal intIdCompetenza As Integer, ByVal sqlLocalConn As SqlClient.SqlConnection) As String
        'creato il 04/04/2018 lettera di CHIUSURA provvedimento disciplinare ad un volontario
        Get
            Dim dtR As DataRow
            Dim dtT As New DataTable

            dtT.Columns.Add(New DataColumn("PARAMETRO", GetType(String)))
            dtT.Columns.Add(New DataColumn("VALORE", GetType(String)))

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDPROVVEDIMENTODISCIPLINARE"
            dtR(1) = IdProvvedimentoDisciplinare

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDENTE"
            dtR(1) = IdEnte

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDENTITA"
            dtR(1) = IdEntita

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)

            'mod. il 04/10/2016 da simona cordella
            ' passo l'id modello diverso  se vengo da helios o Futuro
            Dim IntModello As Integer
            If Session("Sistema") = "Helios" Then
                IntModello = 260
            Else
                IntModello = 261
            End If

            PathDocumento = EseguiGenerazioneModello(IntModello, strUserName, intIdCompetenza, dtT, sqlLocalConn)

            Return PathDocumento
        End Get
        Set(ByVal Value As String)
            PathDocumento = Value
        End Set
    End Property

    Public Property MON_SanzioneProvvedimentoDisciplinare(ByVal IdProvvedimentoDisciplinare As Integer, ByVal IdEnte As Integer, ByVal IdEntita As Integer, ByVal strUserName As String, ByVal intIdCompetenza As Integer, ByVal sqlLocalConn As SqlClient.SqlConnection) As String
        'creato il 04/04/2018 lettera SANZIONE provvedimento disciplinare ad un volontario
        Get
            Dim dtR As DataRow
            Dim dtT As New DataTable

            dtT.Columns.Add(New DataColumn("PARAMETRO", GetType(String)))
            dtT.Columns.Add(New DataColumn("VALORE", GetType(String)))

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDPROVVEDIMENTODISCIPLINARE"
            dtR(1) = IdProvvedimentoDisciplinare

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDENTE"
            dtR(1) = IdEnte

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDENTITA"
            dtR(1) = IdEntita

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)

            'mod. il 04/10/2016 da simona cordella
            ' passo l'id modello diverso  se vengo da helios o Futuro
            Dim IntModello As Integer
            If Session("Sistema") = "Helios" Then
                IntModello = 262
            Else
                IntModello = 263
            End If

            PathDocumento = EseguiGenerazioneModello(IntModello, strUserName, intIdCompetenza, dtT, sqlLocalConn)

            Return PathDocumento
        End Get
        Set(ByVal Value As String)
            PathDocumento = Value
        End Set
    End Property

    Public Property PROG_SostituzioniOLP(ByVal intIdEnte As Integer, ByVal IdIstanzaSostituzioneOLP As Integer, ByVal strUserName As String, ByVal intIdCompetenza As Integer, ByVal sqlLocalConn As SqlClient.SqlConnection) As String
        Get
            Dim dtR As DataRow
            Dim dtT As New DataTable
            'Dim TipoModello As String = TrovaTipoModello(IdBando)

            dtT.Columns.Add(New DataColumn("PARAMETRO", GetType(String)))
            dtT.Columns.Add(New DataColumn("VALORE", GetType(String)))

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDENTE"
            dtR(1) = intIdEnte

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDISTANZASOSTITUZIONEOLP"
            dtR(1) = IdIstanzaSostituzioneOLP

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)
            '***

            PathDocumento = EseguiGenerazioneModello(300, strUserName, intIdCompetenza, dtT, sqlLocalConn)


            Return PathDocumento
        End Get
        Set(ByVal Value As String)
            PathDocumento = Value
        End Set
    End Property


#Region "Garanzia Giovani"
    Public Property VOL_AssegnazioneVolontariGaranziaGiovani(ByVal IdVolontario As Integer, ByVal strUserName As String, ByVal intIdCompetenza As Integer, ByVal sqlLocalConn As SqlClient.SqlConnection) As String
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

            PathDocumento = EseguiGenerazioneModello(203, strUserName, intIdCompetenza, dtT, sqlLocalConn)

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

            PathDocumento = EseguiGenerazioneModello(204, strUserName, intIdCompetenza, dtT, sqlLocalConn)

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

            PathDocumento = EseguiGenerazioneModello(215, strUserName, intIdCompetenza, dtT, sqlLocalConn)

            Return PathDocumento
        End Get
        Set(ByVal Value As String)
            PathDocumento = Value
        End Set
    End Property

    Public Property VOL_SostituzioneVolontariGaranziaGiovani(ByVal IdVolontario As Integer, ByVal strUserName As String, ByVal intIdCompetenza As Integer, ByVal sqlLocalConn As SqlClient.SqlConnection) As String
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

            PathDocumento = EseguiGenerazioneModello(205, strUserName, intIdCompetenza, dtT, sqlLocalConn)

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

            PathDocumento = EseguiGenerazioneModello(206, strUserName, intIdCompetenza, dtT, sqlLocalConn)

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

            PathDocumento = EseguiGenerazioneModello(216, strUserName, intIdCompetenza, dtT, sqlLocalConn)

            Return PathDocumento
        End Get
        Set(ByVal Value As String)
            PathDocumento = Value
        End Set
    End Property
    Public Property VOL_LetteraApprovazioneGraduatoriaGaranziaGiovani(ByVal intIdEnte As Integer, ByVal IdBando As Integer, ByVal strUserName As String, ByVal intIdCompetenza As Integer, ByVal sqlLocalConn As SqlClient.SqlConnection, ByVal dataavvio As String) As String
        Get
            Dim dtR As DataRow
            Dim dtT As New DataTable

            dtT.Columns.Add(New DataColumn("PARAMETRO", GetType(String)))
            dtT.Columns.Add(New DataColumn("VALORE", GetType(String)))

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDENTE"
            dtR(1) = intIdEnte

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDBANDO"
            dtR(1) = IdBando

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)

            'SANDOKAN
            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "DATAAVVIO"
            dtR(1) = dataavvio

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)


            PathDocumento = EseguiGenerazioneModello(201, strUserName, intIdCompetenza, dtT, sqlLocalConn)

            Return PathDocumento
        End Get
        Set(ByVal Value As String)
            PathDocumento = Value
        End Set
    End Property

    Public Property VOL_LetteraApprovazioneGraduatoriaProgrammaGaranziaGiovani(ByVal intIdEnte As Integer, ByVal IdBando As Integer, ByVal intIdProgramma As Integer, ByVal strUserName As String, ByVal intIdCompetenza As Integer, ByVal sqlLocalConn As SqlClient.SqlConnection, ByVal dataavvio As String) As String
        Get
            Dim dtR As DataRow
            Dim dtT As New DataTable

            dtT.Columns.Add(New DataColumn("PARAMETRO", GetType(String)))
            dtT.Columns.Add(New DataColumn("VALORE", GetType(String)))

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDENTE"
            dtR(1) = intIdEnte

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDBANDO"
            dtR(1) = IdBando

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDPROGRAMMA"
            dtR(1) = intIdProgramma

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)

            'SANDOKAN
            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "DATAAVVIO"
            dtR(1) = dataavvio

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)


            PathDocumento = EseguiGenerazioneModello(281, strUserName, intIdCompetenza, dtT, sqlLocalConn)

            Return PathDocumento
        End Get
        Set(ByVal Value As String)
            PathDocumento = Value
        End Set
    End Property

    Public Property VOL_LetteraApprovazioneGraduatoriaProgrammaSCD(ByVal intIdEnte As Integer, ByVal IdBando As Integer, ByVal intIdProgramma As Integer, ByVal strUserName As String, ByVal intIdCompetenza As Integer, ByVal sqlLocalConn As SqlClient.SqlConnection, ByVal dataavvio As String) As String
        Get
            Dim dtR As DataRow
            Dim dtT As New DataTable

            dtT.Columns.Add(New DataColumn("PARAMETRO", GetType(String)))
            dtT.Columns.Add(New DataColumn("VALORE", GetType(String)))

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDENTE"
            dtR(1) = intIdEnte

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDBANDO"
            dtR(1) = IdBando

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDPROGRAMMA"
            dtR(1) = intIdProgramma

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)

            'SANDOKAN
            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "DATAAVVIO"
            dtR(1) = dataavvio

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)


            PathDocumento = EseguiGenerazioneModello(301, strUserName, intIdCompetenza, dtT, sqlLocalConn)

            Return PathDocumento
        End Get
        Set(ByVal Value As String)
            PathDocumento = Value
        End Set
    End Property

    Public Property VOL_LetteraApprovazioneGraduatoriaProgrammaEstero(ByVal intIdEnte As Integer, ByVal IdBando As Integer, ByVal intIdProgramma As Integer, ByVal strUserName As String, ByVal intIdCompetenza As Integer, ByVal sqlLocalConn As SqlClient.SqlConnection, ByVal dataavvio As String) As String
        Get
            Dim dtR As DataRow
            Dim dtT As New DataTable

            dtT.Columns.Add(New DataColumn("PARAMETRO", GetType(String)))
            dtT.Columns.Add(New DataColumn("VALORE", GetType(String)))

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDENTE"
            dtR(1) = intIdEnte

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDBANDO"
            dtR(1) = IdBando

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDPROGRAMMA"
            dtR(1) = intIdProgramma

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)

            'SANDOKAN
            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "DATAAVVIO"
            dtR(1) = dataavvio

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)


            PathDocumento = EseguiGenerazioneModello(302, strUserName, intIdCompetenza, dtT, sqlLocalConn)

            Return PathDocumento
        End Get
        Set(ByVal Value As String)
            PathDocumento = Value
        End Set
    End Property

    Public Property VOL_LetteraApprovazioneGraduatoriaProgramma(ByVal intIdEnte As Integer, ByVal IdBando As Integer, ByVal intIdProgramma As Integer, ByVal strUserName As String, ByVal intIdCompetenza As Integer, ByVal sqlLocalConn As SqlClient.SqlConnection, ByVal dataavvio As String) As String
        Get
            Dim dtR As DataRow
            Dim dtT As New DataTable

            dtT.Columns.Add(New DataColumn("PARAMETRO", GetType(String)))
            dtT.Columns.Add(New DataColumn("VALORE", GetType(String)))

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDENTE"
            dtR(1) = intIdEnte

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDBANDO"
            dtR(1) = IdBando

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDPROGRAMMA"
            dtR(1) = intIdProgramma

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)

            'SANDOKAN
            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "DATAAVVIO"
            dtR(1) = dataavvio

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)


            PathDocumento = EseguiGenerazioneModello(280, strUserName, intIdCompetenza, dtT, sqlLocalConn)

            Return PathDocumento
        End Get
        Set(ByVal Value As String)
            PathDocumento = Value
        End Set
    End Property

    Public Property VOL_ElencoVolontariAmmessiGaranziaGiovani(ByVal idAttivitaSedeAssegnazione As Integer, ByVal strUserName As String, ByVal intIdCompetenza As Integer, ByVal sqlLocalConn As SqlClient.SqlConnection) As String
        Get
            Dim dtR As DataRow
            Dim dtT As New DataTable

            dtT.Columns.Add(New DataColumn("PARAMETRO", GetType(String)))
            dtT.Columns.Add(New DataColumn("VALORE", GetType(String)))

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDAttivitàSedeAssegnazione"
            dtR(1) = idAttivitaSedeAssegnazione

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)

            PathDocumento = EseguiGenerazioneModello(202, strUserName, intIdCompetenza, dtT, sqlLocalConn)

            Return PathDocumento
        End Get
        Set(ByVal Value As String)
            PathDocumento = Value
        End Set
    End Property

    Public Property VOL_LetteraEsclusioneGaranziaGiovani(ByVal IdVolontario As Integer, ByVal IdEnte As Integer, ByVal IdProgetto As Integer, ByVal CodiceFiscale As String, ByVal strUserName As String, ByVal intIdCompetenza As Integer, ByVal sqlLocalConn As SqlClient.SqlConnection) As String
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

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDENTE"
            dtR(1) = IdEnte

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDPROGETTO"
            dtR(1) = IdProgetto

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "CODICEFISCALE"
            dtR(1) = CodiceFiscale

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)

            PathDocumento = EseguiGenerazioneModello(207, strUserName, intIdCompetenza, dtT, sqlLocalConn)

            Return PathDocumento
        End Get
        Set(ByVal Value As String)
            PathDocumento = Value
        End Set
    End Property

    Public Property VOL_ChiusuraInServizioGaranziaGiovani(ByVal IdVolontario As Integer, ByVal IdEnte As Integer, ByVal strUserName As String, ByVal intIdCompetenza As Integer, ByVal sqlLocalConn As SqlClient.SqlConnection) As String
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

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDENTE"
            dtR(1) = IdEnte

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)

            PathDocumento = EseguiGenerazioneModello(208, strUserName, intIdCompetenza, dtT, sqlLocalConn)

            Return PathDocumento
        End Get
        Set(ByVal Value As String)
            PathDocumento = Value
        End Set
    End Property

    Public Property VOL_RinunciaServizioVolontarioGaranziaGiovani(ByVal IdEnte As Integer, ByVal IdVolontarioSostituito As Integer, ByVal strUserName As String, ByVal intIdCompetenza As Integer, ByVal sqlLocalConn As SqlClient.SqlConnection) As String
        Get
            Dim dtR As DataRow
            Dim dtT As New DataTable

            dtT.Columns.Add(New DataColumn("PARAMETRO", GetType(String)))
            dtT.Columns.Add(New DataColumn("VALORE", GetType(String)))

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDENTE"
            dtR(1) = IdEnte

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDENTITàSOSTITUITA"
            dtR(1) = IdVolontarioSostituito

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)

            PathDocumento = EseguiGenerazioneModello(209, strUserName, intIdCompetenza, dtT, sqlLocalConn)

            Return PathDocumento
        End Get
        Set(ByVal Value As String)
            PathDocumento = Value
        End Set
    End Property

    Public Property VOL_RinunciaServizioVolontarioMultiplaGaranziaGiovani(ByVal IdEnte As Integer, ByVal IdVolontarioSubentrato As Integer, ByVal IdBando As Integer, ByVal strUserName As String, ByVal intIdCompetenza As Integer, ByVal sqlLocalConn As SqlClient.SqlConnection) As String
        Get
            Dim dtR As DataRow
            Dim dtT As New DataTable

            dtT.Columns.Add(New DataColumn("PARAMETRO", GetType(String)))
            dtT.Columns.Add(New DataColumn("VALORE", GetType(String)))

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDENTE"
            dtR(1) = IdEnte

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDENTITàSUBENTRATA"
            dtR(1) = IdVolontarioSubentrato

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDBANDO"
            dtR(1) = IdBando

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)

            PathDocumento = EseguiGenerazioneModello(210, strUserName, intIdCompetenza, dtT, sqlLocalConn)

            Return PathDocumento
        End Get
        Set(ByVal Value As String)
            PathDocumento = Value
        End Set
    End Property

    Public Property VOL_LetteraEsclusionePerAssenzaIngiustificataGaranziaGiovani(ByVal IdVolontario As Integer, ByVal IdEnte As Integer, ByVal strUserName As String, ByVal intIdCompetenza As Integer, ByVal sqlLocalConn As SqlClient.SqlConnection) As String
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

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDENTE"
            dtR(1) = IdEnte

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)

            PathDocumento = EseguiGenerazioneModello(211, strUserName, intIdCompetenza, dtT, sqlLocalConn)

            Return PathDocumento
        End Get
        Set(ByVal Value As String)
            PathDocumento = Value
        End Set
    End Property

    Public Property VOL_LetteraEsclusionePerGiorniPermessoGaranziaGiovani(ByVal IdVolontario As Integer, ByVal IdEnte As Integer, ByVal strUserName As String, ByVal intIdCompetenza As Integer, ByVal sqlLocalConn As SqlClient.SqlConnection) As String
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

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDENTE"
            dtR(1) = IdEnte

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)

            PathDocumento = EseguiGenerazioneModello(212, strUserName, intIdCompetenza, dtT, sqlLocalConn)

            Return PathDocumento
        End Get
        Set(ByVal Value As String)
            PathDocumento = Value
        End Set
    End Property

    Public Property VOL_LetteraEsclusionePerSuperamentoMalattiaGaranziaGiovani(ByVal IdVolontario As Integer, ByVal IdEnte As Integer, ByVal strUserName As String, ByVal intIdCompetenza As Integer, ByVal sqlLocalConn As SqlClient.SqlConnection) As String
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

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDENTE"
            dtR(1) = IdEnte

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)

            PathDocumento = EseguiGenerazioneModello(213, strUserName, intIdCompetenza, dtT, sqlLocalConn)

            Return PathDocumento
        End Get
        Set(ByVal Value As String)
            PathDocumento = Value
        End Set
    End Property


    Public Property VOL_LetteraEsclusionePerSuperamentoMalattiaDopoi6MesiGaranziaGiovani(ByVal IdVolontario As Integer, ByVal IdEnte As Integer, ByVal strUserName As String, ByVal intIdCompetenza As Integer, ByVal sqlLocalConn As SqlClient.SqlConnection) As String
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

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDENTE"
            dtR(1) = IdEnte

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)

            PathDocumento = EseguiGenerazioneModello(214, strUserName, intIdCompetenza, dtT, sqlLocalConn)

            Return PathDocumento
        End Get
        Set(ByVal Value As String)
            PathDocumento = Value
        End Set
    End Property
    Public Property VOL_CertificatoServizioSvolto(ByVal IdVolontario As Integer, ByVal IdEnte As Integer, ByVal strUserName As String, ByVal intIdCompetenza As Integer, ByVal sqlLocalConn As SqlClient.SqlConnection) As String
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

            'alla riga assegno la riga delle intestazioni appena creata
            dtR = dtT.NewRow()

            'carico la prima riga della datatable
            dtR(0) = "IDENTE"
            dtR(1) = IdEnte

            'aggiungo la riga appena caricata alla datatable
            dtT.Rows.Add(dtR)

            PathDocumento = EseguiGenerazioneModello(319, strUserName, intIdCompetenza, dtT, sqlLocalConn)

            Return PathDocumento
        End Get
        Set(ByVal Value As String)
            PathDocumento = Value
        End Set
    End Property

#End Region

End Class
