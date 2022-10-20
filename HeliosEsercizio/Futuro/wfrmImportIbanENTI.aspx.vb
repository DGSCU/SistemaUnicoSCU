Imports System.IO
Imports System.Text.RegularExpressions
Imports System.Data.SqlClient
Public Class wfrmImportIbanENTI
    Inherits System.Web.UI.Page
    Private Writer As StreamWriter
    Private strNote As String
    Private Tot As Integer
    Private TotOk As Integer
    Private TotKo As Integer
    Private NomeUnivoco As String
    Private NomeUnivocoErr As String
    Private xIdAttivita As Integer
    Dim dtrgenerico As SqlClient.SqlDataReader
    Dim strsql As String





  
    '--- CONVERSIONE DA HELIOS 23/01/2015  -IACOBUCCI 

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Session("LogIn") Is Nothing Then
            If Session("LogIn") = False Then 'verifico validità log-in
                Response.Redirect("LogOn.aspx")
            End If
        Else
            Response.Redirect("LogOn.aspx")
        End If
    End Sub



    Private Sub CmdElabora_Click(ByVal sender As Object, ByVal e As EventArgs) Handles CmdElabora.Click
        '--- salvataggio del file sul server
        Dim swErr As Boolean
        swErr = False
        If txtSelFile.PostedFile.FileName.ToString <> "" Then
            Try
                NomeUnivoco = "codiciIban" & Session("IdEnte") & "_" & Session("Utente") & "_" & Year(Now) & Month(Now) & Day(Now) & Hour(Now) & Minute(Now) & Second(Now)

                Dim file As String
                Dim estensione As String
                file = LCase(txtSelFile.FileName.ToString)
                estensione = file.Substring(file.Length - 4)
                If estensione <> ".csv" Then
                    lblMessaggioErrore.Visible = True
                    lblMessaggioErrore.Text = "Selezionare il file nel formato CSV."
                    Exit Sub
                End If
                txtSelFile.PostedFile.SaveAs(Server.MapPath("upload") & "\" & NomeUnivoco & ".csv")

                CreaTabTemp()

            Catch exc As Exception
                swErr = True
                'Response.Write("<script>" & vbCrLf)
                'Response.Write("parent.fraUp.CambiaImmagine('filenoncaricato.jpg','0')" & vbCrLf)
                'Response.Write("</script>")
                CancellaTabellaTemp()
            End Try

            If swErr = False Then
                LeggiCSV()

                'Response.Write("<script>" & vbCrLf)
                'Response.Write("parent.fraUp.CambiaImmagine('allegatocompletato.jpg','1')" & vbCrLf)
                'Response.Write("</script>")
            End If

        Else

            lblMessaggioErrore.Visible = True
            lblMessaggioErrore.Text = "Selezionare il file da inviare."
            Exit Sub
        End If

    End Sub


    Private Sub CancellaTabellaTemp()
        Dim strSql As String
        Dim cmdCanTempTable As SqlClient.SqlCommand
        Try
            '--- CANCELLO TAB TEMPORANEA
            strSql = "DROP TABLE [#TEMP_CODICI_IBAN]"

            cmdCanTempTable = New SqlClient.SqlCommand
            cmdCanTempTable.CommandText = strSql
            cmdCanTempTable.Connection = Session("conn")
            cmdCanTempTable.ExecuteNonQuery()
        Catch e As Exception
        End Try

        cmdCanTempTable.Dispose()
    End Sub

  

    Private Sub CreaTabTemp()
        Dim strSql As String
        Dim cmdCreateTempTable As SqlClient.SqlCommand

        CancellaTabellaTemp()

        '--- CREO TAB TEMPORANEA
        strSql = "CREATE TABLE [#TEMP_CODICI_IBAN] (" & _
                 "[DenominazioneEnte] [nvarchar] (200) COLLATE DATABASE_DEFAULT, " & _
                 "[CodiceEnte] [nvarchar] (10) COLLATE DATABASE_DEFAULT, " & _
                 "[CodiceVolontario] [nvarchar] (15) COLLATE DATABASE_DEFAULT, " & _
                 "[Cognome] [nvarchar] (100) COLLATE DATABASE_DEFAULT, " & _
                 "[Nome] [nvarchar] (100) COLLATE DATABASE_DEFAULT, " & _
                 "[CodiceIban] [nvarchar] (50) COLLATE DATABASE_DEFAULT, " & _
                 "[BicSwift] [nvarchar] (11) COLLATE DATABASE_DEFAULT " & _
                 ")"
        cmdCreateTempTable = New SqlClient.SqlCommand
        cmdCreateTempTable.CommandText = strSql
        cmdCreateTempTable.Connection = Session("conn")
        cmdCreateTempTable.ExecuteNonQuery()
        cmdCreateTempTable.Dispose()
    End Sub

    Private Sub LeggiCSV()
        '--- lettura del file
        Dim Reader As StreamReader
        Dim xLinea As String
        Dim ArrCampi() As String
        Dim swErr As Boolean
        Dim swErrEnte As Boolean
        Dim AppoNote As String
        Dim DenominazioneEnte As String
        Dim CodiceEnte As String
        Dim ArrCampiEnti() As String
        Dim IntestazioneDenominazione As String
        Dim IntestazioneCodice As String
        Dim IntestazioneLibretti As String
        Dim WriterErr As StreamWriter
        Dim clsIban As New CheckBancari
        Dim regex As Regex
        regex = New Regex("^[a-zA-Z0-9]*$")
        '--- Leggo il file di input e scrivo quello di output
        Reader = New StreamReader(Server.MapPath("upload") & "\" & NomeUnivoco & ".CSV", System.Text.Encoding.UTF7, False)

        Writer = New StreamWriter(Server.MapPath("download") & "\" & NomeUnivoco & ".CSV")

        '--- scrive il csv dei soli errori
        NomeUnivocoErr = CodiceEnte & "_ERR_CodiciIban_" & Year(Now) & Month(Now) & Day(Now) & Hour(Now) & Minute(Now) & Second(Now)
        WriterErr = New StreamWriter(Server.MapPath("download" & "\" & NomeUnivocoErr & ".CSV"))


        swErrEnte = False

        '--- dati ente + intestazione
        '--- denominazione
        xLinea = Reader.ReadLine()
        If xLinea = vbNullString Then
            strNote = "ATTENZIONE! File non valido."
            Writer.WriteLine("Note;" & strNote)
            WriterErr.WriteLine("Note;" & strNote)
        Else
            ArrCampiEnti = CreaArray(xLinea)
            IntestazioneDenominazione = xLinea
            If UBound(ArrCampiEnti) < 1 Then
                DenominazioneEnte = vbNullString
            Else
                DenominazioneEnte = ArrCampiEnti(1)
            End If
            Writer.WriteLine(xLinea)
            WriterErr.WriteLine(xLinea)

            '--- codice ente
            swErrEnte = False
            xLinea = Reader.ReadLine()
            If xLinea = vbNullString Then
                strNote = "ATTENZIONE! File non valido."
                Writer.WriteLine("Note;" & strNote)
                WriterErr.WriteLine("Note;" & strNote)
            Else
                ArrCampiEnti = CreaArray(xLinea)
                IntestazioneCodice = xLinea
                If UBound(ArrCampiEnti) < 1 Then
                    CodiceEnte = vbNullString
                Else
                    CodiceEnte = ArrCampiEnti(1)
                End If
                Writer.WriteLine(xLinea)
                WriterErr.WriteLine(xLinea)

                If InStr(Trim(IntestazioneDenominazione), "Denominazione Ente:") <> 1 Or InStr(Trim(IntestazioneCodice), "Codice Ente:") <> 1 Then
                    strNote = "ATTENZIONE! File non valido."
                    Writer.WriteLine("Note;" & strNote)
                    WriterErr.WriteLine("Note;" & strNote)
                Else
                    If DenominazioneEnte = vbNullString Then
                        strNote = strNote & "Il campo DenominazioneEnte e' un campo obbligatorio."
                        swErrEnte = True
                        TotKo = TotKo + 1
                    Else
                        If Len(DenominazioneEnte) > 200 Then
                            strNote = strNote & "Il campo DenominazioneEnte puo' contenere massimo 200 caratteri."
                            swErrEnte = True
                            TotKo = TotKo + 1
                        End If
                    End If

                    If CodiceEnte = vbNullString Then
                        strNote = strNote & "Il campo CodiceEnte e' un campo obbligatorio."
                        swErrEnte = True
                        TotKo = TotKo + 1
                    Else
                        If Len(CodiceEnte) > 10 Then
                            strNote = strNote & "Il campo CodiceEnte puo' contenere massimo 10 caratteri."
                            swErrEnte = True
                            TotKo = TotKo + 1
                        Else
                            If VerificaEnte(CodiceEnte, DenominazioneEnte) = False Then
                                strNote = strNote & "Il Codice e la Denominazione dell'Ente non coincidono."
                                swErrEnte = True
                                TotKo = TotKo + 1
                            End If
                        End If
                        If Session("TipoUtente") = "E" Then
                            If VerificaEnteInSessione(CodiceEnte) = False Then
                                strNote = strNote & "Attenzione.Il file non fa riferimeno all'ente."
                                swErrEnte = True
                                TotKo = TotKo + 1
                            End If
                        End If
                    End If

                    'Writer.WriteLine(xLinea)

                    '--- Note Ente
                    Writer.WriteLine("Note;" & strNote)
                    WriterErr.WriteLine("Note;" & strNote)
                    strNote = vbNullString

                    '--- intestazione volontaio-libretto
                    xLinea = Reader.ReadLine()
                    If xLinea <> "CODICE VOLONTARIO;COGNOME;NOME;CODICE IBAN;CODICE BIC/SWIFT" Then
                        strNote = "ATTENZIONE! File non valido."
                        Writer.WriteLine("Note;" & strNote)
                        WriterErr.WriteLine("Note;" & strNote)
                        strNote = vbNullString
                    Else

                        '--- scrivo intestazione
                        Writer.WriteLine("Note;" & xLinea)
                        WriterErr.WriteLine("Note;" & xLinea)
                        '--- scorro le righe
                        xLinea = Reader.ReadLine()
                        While (xLinea <> "")
                            swErr = False
                            Tot = Tot + 1
                            ArrCampi = CreaArray(xLinea)

                            If UBound(ArrCampi) < 4 Then
                                '--- se i campi non sono tutti errore
                                strNote = "Il numero delle colonne inserite e' minore di quello richieste."
                                swErr = True
                                TotKo = TotKo + 1
                            Else
                                If UBound(ArrCampi) > 4 Then
                                    '--- se i campi sono troppi errore
                                    strNote = "Il numero delle colonne inserite e' maggiore di quello richieste."
                                    swErr = True
                                    TotKo = TotKo + 1
                                Else
                                    'CodiceVolontario    
                                    If Trim(ArrCampi(0)) = vbNullString Then
                                        strNote = strNote & "Il campo CodiceVolontario e' un campo obbligatorio."
                                        swErr = True
                                    Else
                                        If Len(ArrCampi(0)) > 15 Then
                                            strNote = strNote & "Il campo CodiceVolontario puo' contenere massimo 15 caratteri."
                                            swErr = True
                                        End If
                                    End If
                                    'Cognome    
                                    If Trim(ArrCampi(1)) = vbNullString Then
                                        strNote = strNote & "Il campo Cognome e' un campo obbligatorio."
                                        swErr = True
                                    Else
                                        If Len(ArrCampi(1)) > 100 Then
                                            strNote = strNote & "Il campo Cognome puo' contenere massimo 100 caratteri."
                                            swErr = True
                                        End If
                                    End If
                                    'Nome
                                    If Trim(ArrCampi(2)) = vbNullString Then
                                        strNote = strNote & "Il campo Nome e' un campo obbligatorio."
                                        swErr = True
                                    Else
                                        If Len(ArrCampi(2)) > 100 Then
                                            strNote = strNote & "Il campo Nome puo' contenere massimo 100 caratteri."
                                            swErr = True
                                        End If
                                    End If
                                    'VerificaVolontario
                                    If VerificaVolontario(ArrCampi(0), ArrCampi(1), ArrCampi(2), CodiceEnte) = False Then
                                        strNote = strNote & "Il Volontario indicato non e' dell'Ente o il CodiceVolontario non coincide con il Nome e il Cognome."
                                        swErr = True
                                    End If
                                    'Verifico la data avvio del volontario
                                    If VerificaDataAvvioVolontario(ArrCampi(0)) = False Then
                                        strNote = strNote & "Per il volontario indicato non  è previsto il caricamento delle coordinate bancarie."
                                        swErr = True
                                    End If
                                    'Verifico la data Inizio Servizio
                                    If VerificaDataInizioServizio(ArrCampi(0)) = False Then
                                        strNote = strNote & "Il volontario indicato non  risulta ancora avviato al servizio."
                                        swErr = True
                                    End If

                                    If VerificaEsistenzaIbanVolontario(ArrCampi(0)) = True And Trim(ArrCampi(3)) = vbNullString Then
                                        strNote = strNote & "Per il Volontario e' già stato indicato un codice iban,impossibile aggiornarlo a valore vuoto."
                                        swErr = True
                                    End If
                                    'mod. il 27/10/2009 Controllo l'iban e bic/swift solo se il conto è in italia

                                    If regex.Match(ArrCampi(3)).Success = False Then
                                        strNote = strNote & "Nel campo Iban possono essere indicati solo lettere e numeri."
                                        swErr = True
                                    End If
                                    If UCase(Left(Trim(ArrCampi(3)), 2)) = "IT" Then
                                        If Len(Trim(ArrCampi(3))) > 27 Then
                                            strNote = strNote & "La lunghezza del campo Iban è di 27 caratteri."
                                            swErr = True
                                        Else
                                            If Len(Trim(ArrCampi(3))) < 27 Then
                                                strNote = strNote & "La lunghezza del codice IBAN è errata."
                                                swErr = True
                                            Else
                                                If clsIban.VerificaLetteraCin(Mid(Trim(ArrCampi(3)), 5, 1)) = "1" Then
                                                    strNote = strNote & "Codice Iban errato."
                                                    swErr = True
                                                End If
                                                'Funzione che controlla l'autenticità del codice iban indicato
                                                Dim ChkCalcolaIban As String = clsIban.CalcolaIBAN(Left(Trim(ArrCampi(3)), 2), Mid(Trim(ArrCampi(3)), 5))
                                                If UCase(ChkCalcolaIban) <> UCase(Trim(ArrCampi(3))) Then
                                                    strNote = strNote & "Codice Iban errato."
                                                    swErr = True
                                                Else
                                                    'controllo abi e cab non devono corrispondere a queste caratteristiche 
                                                    'ABI   = 07601 
                                                    'CAB = 3384
                                                    If ClsUtility.AbiCab(UCase(Trim(ArrCampi(3)))) Then
                                                        strNote = strNote & "Il codice iban indicato non fa riferimento ad un conto corrente bancario."
                                                        swErr = True
                                                    End If
                                                End If
                                            End If
                                        End If
                                    Else
                                        If Len(Trim(ArrCampi(3))) > 31 Then
                                            strNote = strNote & "La lunghezza del campo Iban per l'estero non può superare i 31 caratteri."
                                            swErr = True
                                        End If
                                    End If
                                    If Trim(ArrCampi(3)) <> vbNullString Then
                                        'se il volontario ha già un libretto postale
                                        Dim chkLib As String = CheckExistLibretto(ArrCampi(0).ToString)
                                        If chkLib <> "" Then
                                            strNote = strNote & "Per il volontario è stato già indicato un libretto postale."
                                            swErr = True
                                        End If
                                        'verifica se il codiceiban indicato nn è utilizzato da un' altro volontario
                                        Dim chkIban As String = CheckExistIban(Trim(ArrCampi(3).ToString), ArrCampi(0).ToString)
                                        If chkIban <> "" Then
                                            strNote = strNote & "Il Codice Iban è stato già assegnato al volontario " & chkIban & "."
                                            swErr = True
                                        End If
                                    End If

                                    ''CODICE BIC/SWIFT
                                    If regex.Match(ArrCampi(4)).Success = False Then
                                        strNote = strNote & "- Nel campo BIC/SWIFT possono essere indicati solo lettere e numeri."
                                        swErr = True
                                    End If
                                    If UCase(Left(Trim(ArrCampi(3)), 2)) = "IT" Then
                                        If Trim(ArrCampi(4)) <> vbNullString Then
                                            'lunghezza bicswift 8 - 11
                                            If (Len(ArrCampi(4)) <> 8) Then
                                                If (Len(ArrCampi(4)) <> 11) Then
                                                    strNote = strNote & "La lunghezza del codice  BIC/SWIFT è errata."
                                                    swErr = True
                                                End If
                                            End If
                                        End If
                                    Else
                                        If Trim(ArrCampi(3)) <> vbNullString And Trim(ArrCampi(4)) = vbNullString Then
                                            strNote = strNote & "Il codice  BIC/SWIFT è obbligatorio per il Conto Estero."
                                            swErr = True
                                        End If
                                        If Len(ArrCampi(4)) > 20 Then
                                            strNote = strNote & "Il campo BIC/SWIFT puo' contenere massimo 20 caratteri."
                                            swErr = True
                                        End If
                                    End If
                                    ' controllo se non è stato indicato il codice iban ma il bic_swift è valorizzato
                                    If Trim(ArrCampi(4)) <> vbNullString And Trim(ArrCampi(3)) = vbNullString Then
                                        strNote = strNote & "E' necessario indicare il Codice Iban."
                                        swErr = True
                                    End If


                                    If swErr = False Then
                                        'If swErrEnte = False Then
                                        ScriviTabTemp(DenominazioneEnte, CodiceEnte, ArrCampi)
                                        'End If
                                    Else
                                        TotKo = TotKo + 1
                                        '--- scrive solo i cattivi
                                        WriterErr.WriteLine(strNote & ";" & xLinea)
                                    End If
                                End If
                            End If
                            Writer.WriteLine(strNote & ";" & xLinea)
                            strNote = vbNullString
                            xLinea = Reader.ReadLine()

                        End While
                    End If
                End If
            End If
        End If
        Reader.Close()
        Writer.Close()
        WriterErr.Close()

        ''--- reindirizzo la pagina sottostante
        Response.Redirect("WfrmRisultatoImportIbanUNSC.aspx?VengoDa=" & "IbanEnti" & "&NomeFileErr=" & NomeUnivocoErr & "&NomeFile=" & NomeUnivoco & "&Tot=" & Tot & "&TotOk=" & TotOk & "&TotKo=" & TotKo)
        'Response.Write("<script>" & vbCrLf)
        'Response.Write("parent.Naviga('WfrmRisultatoImportIbanUNSC.aspx?VengoDa=" & Request.QueryString("VengoDa") & "&NomeFileErr=" & NomeUnivocoErr & "&NomeFile=" & NomeUnivoco & "&Tot=" & Tot & "&TotOk=" & TotOk & "&TotKo=" & TotKo & "')" & vbCrLf)
        'Response.Write("</script>")
    End Sub

    Private Function CreaArray(ByVal pLinea As String) As String()
        Dim TmpArr As String()
        Dim DefArr As String()
        Dim i As Integer
        Dim x As Integer

        TmpArr = Split(pLinea, ";")

        For i = 0 To UBound(TmpArr)
            If i = 0 Then
                ReDim DefArr(0)
            Else
                ReDim Preserve DefArr(UBound(DefArr) + 1)
            End If
            If Left(TmpArr(i), 1) = Chr(34) Then
                x = i
                Do While Right(TmpArr(x), 1) <> Chr(34)
                    If x = i Then
                        DefArr(UBound(DefArr)) = Mid(TmpArr(x), 2) & "; "
                    Else
                        DefArr(UBound(DefArr)) = DefArr(UBound(DefArr)) & TmpArr(x) & "; "
                    End If
                    x = x + 1
                Loop
                DefArr(UBound(DefArr)) = DefArr(UBound(DefArr)) & Mid(TmpArr(x), 1, Len(TmpArr(x)) - 1)
                i = x
            Else
                DefArr(UBound(DefArr)) = TmpArr(i)
            End If
        Next
        If UBound(TmpArr) = 2 Then
            ReDim Preserve DefArr(UBound(DefArr) + 2)
        End If
        If UBound(TmpArr) = 3 Then
            ReDim Preserve DefArr(UBound(DefArr) + 1)
        End If
        CreaArray = DefArr

    End Function

    Private Sub ScriviTabTemp(ByVal pDenominazioneEnte As String, ByVal pCodiceEnte As String, ByVal pArray() As String)
        '--- scrive nella tab temporanea
        Dim cmdTemp As SqlClient.SqlCommand
        Dim strsql As String
        Dim DefLibrettoPostale As String
        Dim i As Integer

        ''creo il libretto postale da 12 cifre
        'For i = Len(Trim(ClsServer.NoApice(pArray(3)))) + 1 To 12
        '    DefLibrettoPostale = "0" & DefLibrettoPostale
        'Next

        'DefLibrettoPostale = DefLibrettoPostale & Trim(ClsServer.NoApice(pArray(3)))

        Try
            strsql = "INSERT INTO #TEMP_CODICI_IBAN "
            strsql = strsql & "(DenominazioneEnte, "
            strsql = strsql & "CodiceEnte, "
            strsql = strsql & "CodiceVolontario, "
            strsql = strsql & "Cognome, "
            strsql = strsql & "Nome, "
            strsql = strsql & "CodiceIban, "
            strsql = strsql & "BicSwift)"
            strsql = strsql & "values "
            strsql = strsql & "('" & Trim(ClsServer.NoApice(pDenominazioneEnte)) & "', "
            strsql = strsql & "'" & Trim(ClsServer.NoApice(pCodiceEnte)) & "', "
            strsql = strsql & "'" & Trim(ClsServer.NoApice(pArray(0))) & "', "
            strsql = strsql & "'" & Trim(ClsServer.NoApice(pArray(1))) & "', "
            strsql = strsql & "'" & Trim(ClsServer.NoApice(pArray(2))) & "', "
            If ClsServer.NoApice(pArray(3)) <> vbNullString Then
                strsql = strsql & "'" & UCase(Trim(ClsServer.NoApice(pArray(3)))) & "',"
            Else
                strsql = strsql & " NULL,"
            End If
            If ClsServer.NoApice(pArray(4)) <> vbNullString Then
                strsql = strsql & "'" & UCase(Trim(ClsServer.NoApice(pArray(4)))) & "'"
            Else
                strsql = strsql & " NULL"
            End If
            strsql = strsql & ")"
            cmdTemp = New SqlClient.SqlCommand
            cmdTemp.CommandText = strsql
            cmdTemp.Connection = Session("conn")
            cmdTemp.ExecuteNonQuery()
            cmdTemp.Dispose()
            TotOk = TotOk + 1

        Catch exc As Exception
            strNote = "Errore generico."
            TotKo = TotKo + 1

        End Try
    End Sub


    Private Function VerificaVolontario(ByVal pCodiceVolontario As String, ByVal pCognomeVolontario As String, ByVal pNomeVolontario As String, ByVal pCodiceEnte As String) As Boolean
        Dim dtrComuni As SqlClient.SqlDataReader
        Dim strSql As String

        'strSql = "SELECT Entità.IdEntità FROM Entità " & _
        '         "INNER JOIN AttivitàEntità ON Entità.IdEntità = AttivitàEntità.IdEntità " & _
        '         "INNER JOIN AttivitàEntiSediAttuazione ON AttivitàEntità.IdAttivitàEnteSedeAttuazione = AttivitàEntiSediAttuazione.IdAttivitàEnteSedeAttuazione " & _
        '         "INNER JOIN Attività ON AttivitàEntiSediAttuazione.IdAttività = Attività.IdAttività " & _
        '         "INNER JOIN Enti ON Attività.IdEntePresentante = Enti.IdEnte " & _
        '         "WHERE Entità.CodiceVolontario = '" & trim(ClsServer.NoApice(pCodiceVolontario)) & "' AND " & _
        '         "Entità.Cognome = '" & trim(ClsServer.NoApice(pCognomeVolontario)) & "' AND " & _
        '         "Entità.Nome = '" & trim(ClsServer.NoApice(pNomeVolontario)) & "' AND " & _
        '         "Enti.CodiceRegione = '" & trim(ClsServer.NoApice(pCodiceEnte)) & "'"
        strSql = "SELECT Entità.IdEntità FROM Entità " & _
                 "INNER JOIN AttivitàEntità ON Entità.IdEntità = AttivitàEntità.IdEntità " & _
                 "INNER JOIN AttivitàEntiSediAttuazione ON AttivitàEntità.IdAttivitàEnteSedeAttuazione = AttivitàEntiSediAttuazione.IdAttivitàEnteSedeAttuazione " & _
                 "INNER JOIN Attività ON AttivitàEntiSediAttuazione.IdAttività = Attività.IdAttività " & _
                 "INNER JOIN Enti ON Attività.IdEntePresentante = Enti.IdEnte " & _
                 " INNER JOIN TipiProgetto ON TipiProgetto.IdTipoProgetto = Attività.IdTipoProgetto " & _
                 "WHERE Entità.CodiceVolontario = '" & Trim(ClsServer.NoApice(pCodiceVolontario)) & "' AND " & _
                 "isnull(replace(replace(replace(replace(replace(replace(replace(entità.Cognome,'°',''),'ì','i'''),'é','e'''),'à','a'''),'ò','o'''),'è','e'''),'ù','u'''), '') = '" & Trim(ClsServer.NoApice(pCognomeVolontario)) & "' AND " & _
                 "isnull(replace(replace(replace(replace(replace(replace(replace(entità.Nome,'°',''),'ì','i'''),'é','e'''),'à','a'''),'ò','o'''),'è','e'''),'ù','u'''), '') = '" & Trim(ClsServer.NoApice(pNomeVolontario)) & "' AND " & _
                 "Enti.CodiceRegione = '" & Trim(ClsServer.NoApice(pCodiceEnte)) & "'" & _
                " AND TipiProgetto.MacroTipoProgetto like '" & Session("FiltroVisibilita") & "' "

        dtrComuni = ClsServer.CreaDatareader(strSql, Session("conn"))
        VerificaVolontario = dtrComuni.HasRows

        dtrComuni.Close()
        dtrComuni = Nothing
    End Function

    Private Function VerificaDataAvvioVolontario(ByVal pCodiceVolontario As String) As Boolean
        Dim dtrComuni As SqlClient.SqlDataReader
        Dim strSql As String
        'se il volontario si trova su un progetto italia o straordinario (NAZIONEBASE=1)
        'caricare l'iban solo per i volontari avviati al servizio dal 01/12/2009 in poi
        'altrimenti (NAZIONEBASE =0) 
        'carico i volontari avviati al servzio dal 16/11/2009
        strSql = "SELECT Entità.IdEntità FROM Entità " & _
                 "INNER JOIN AttivitàEntità ON Entità.IdEntità = AttivitàEntità.IdEntità " & _
                 "INNER JOIN AttivitàEntiSediAttuazione ON AttivitàEntità.IdAttivitàEnteSedeAttuazione = AttivitàEntiSediAttuazione.IdAttivitàEnteSedeAttuazione " & _
                 "INNER JOIN Attività ON AttivitàEntiSediAttuazione.IdAttività = Attività.IdAttività " & _
                 "INNER JOIN TipiProgetto ON  TipiProgetto.IdTipoProgetto =  Attività.IdTipoProgetto " & _
                 "WHERE Entità.CodiceVolontario = '" & Trim(ClsServer.NoApice(pCodiceVolontario)) & "' " & _
                 "AND ((Entità.DataInizioServizio >= '01/12/2009' and tipiprogetto.nazionebase = 1) or (Entità.DataInizioServizio >= '16/11/2009' and tipiprogetto.nazionebase = 0)) "

        dtrComuni = ClsServer.CreaDatareader(strSql, Session("conn"))
        VerificaDataAvvioVolontario = dtrComuni.HasRows

        dtrComuni.Close()
        dtrComuni = Nothing
    End Function
    Private Function VerificaDataInizioServizio(ByVal pCodiceVolontario As String) As Boolean
        Dim dtrComuni As SqlClient.SqlDataReader
        Dim strSql As String
        'se il volontario ancora non e' partito il controllo risulta negativo
        strSql = "SELECT Entità.IdEntità FROM Entità " & _
                 "INNER JOIN AttivitàEntità ON Entità.IdEntità = AttivitàEntità.IdEntità " & _
                 "INNER JOIN AttivitàEntiSediAttuazione ON AttivitàEntità.IdAttivitàEnteSedeAttuazione = AttivitàEntiSediAttuazione.IdAttivitàEnteSedeAttuazione " & _
                 "INNER JOIN Attività ON AttivitàEntiSediAttuazione.IdAttività = Attività.IdAttività " & _
                 "INNER JOIN TipiProgetto ON  TipiProgetto.IdTipoProgetto =  Attività.IdTipoProgetto " & _
                 "WHERE Entità.CodiceVolontario = '" & Trim(ClsServer.NoApice(pCodiceVolontario)) & "' " & _
                 "AND Entità.DataInizioServizio < getdate()  "

        dtrComuni = ClsServer.CreaDatareader(strSql, Session("conn"))
        VerificaDataInizioServizio = dtrComuni.HasRows

        dtrComuni.Close()
        dtrComuni = Nothing
    End Function
    Private Function VerificaEsistenzaIbanVolontario(ByVal pCodiceVolontario As String) As Boolean
        Dim dtrComuni As SqlClient.SqlDataReader
        Dim strSql As String
        'controllo se in precedenza era già stato indicato un iban al volontario

        strSql = "SELECT IdEntità FROM Entità " & _
                 "WHERE CodiceVolontario = '" & Trim(ClsServer.NoApice(pCodiceVolontario)) & "' " & _
                 " AND  isnull(iban,'') <> '' "

        dtrComuni = ClsServer.CreaDatareader(strSql, Session("conn"))
        VerificaEsistenzaIbanVolontario = dtrComuni.HasRows

        dtrComuni.Close()
        dtrComuni = Nothing
    End Function

    Private Function VerificaEnte(ByVal pCodiceEnte As String, ByVal pDenominazioneEnte As String) As Boolean
        Dim dtrComuni As SqlClient.SqlDataReader
        Dim strSql As String

        strSql = "SELECT Idente FROM Enti " & _
                 "WHERE CodiceRegione = '" & Trim(ClsServer.NoApice(pCodiceEnte)) & "' AND " & _
                 " Replace(Denominazione,'""','') = '" & Replace(Trim(ClsServer.NoApice(pDenominazioneEnte)), """", "") & "'"


        dtrComuni = ClsServer.CreaDatareader(strSql, Session("conn"))
        VerificaEnte = dtrComuni.HasRows

        dtrComuni.Close()
        dtrComuni = Nothing

    End Function

    Private Function VerificaEnteInSessione(ByVal pCodiceEnte As String)
        Dim dtrEnte As SqlClient.SqlDataReader
        Dim strSql As String

        strSql = "SELECT CodiceRegione FROM Enti " & _
                 "WHERE IdEnte = " & Session("IdEnte") & " "
        dtrEnte = ClsServer.CreaDatareader(strSql, Session("conn"))

        If dtrEnte.HasRows = True Then
            dtrEnte.Read()
            If Trim(pCodiceEnte) = dtrEnte("CodiceRegione") Then
                VerificaEnteInSessione = True
            Else
                VerificaEnteInSessione = False
            End If

        End If
        dtrEnte.Close()
        dtrEnte = Nothing
        Return VerificaEnteInSessione
    End Function

    Private Function CheckExistLibretto(ByVal strCodVol As String) As String
        Dim strSql As String
        Dim strNominativo As String = ""
        Dim dtrGenerico As SqlClient.SqlDataReader
        strSql = "Select Cognome + ' ' + Nome As Nominativo,CodiceLibrettoPostale From Entità Where isnull(CodiceLibrettoPostale,'') <> '' and CodiceVolontario = '" & strCodVol & "'"
        dtrGenerico = ClsServer.CreaDatareader(strSql, Session("conn"))
        If dtrGenerico.HasRows = False Then
            dtrGenerico.Close()
            dtrGenerico = Nothing
            strNominativo = ""
        Else
            dtrGenerico.Read()
            strNominativo = dtrGenerico.Item("Nominativo")
            dtrGenerico.Close()
            dtrGenerico = Nothing
        End If
        If Not dtrGenerico Is Nothing Then
            dtrGenerico.Close()
            dtrGenerico = Nothing
        End If
        Return strNominativo
    End Function

    Private Function CheckExistIban(ByVal strIban As String, ByVal strCodVol As String) As String
        'Creata da simona cordella il 28/09/2009
        'Funzione che verifica se il codiceIban è già esistente per un'altro volontario

        Dim strSql As String
        Dim strNominativo As String = ""
        Dim strCodiceFiscale As String = ""
        Dim dtrGenerico As SqlClient.SqlDataReader
        strSql = "Select CodiceFiscale From Entità Where CodiceVolontario = '" & Trim(strCodVol) & "'"
        dtrGenerico = ClsServer.CreaDatareader(strSql, Session("conn"))
        If dtrGenerico.HasRows = True Then
            dtrGenerico.Read()
            strCodiceFiscale = dtrGenerico("CodiceFiscale")
            dtrGenerico.Close()
            dtrGenerico = Nothing
            strSql = "Select Cognome + ' ' + Nome As Nominativo From Entità Where IBAN='" & Trim(strIban.Replace("'", "''")) & "' and CodiceFiscale <> '" & Trim(strCodiceFiscale) & "'"
            dtrGenerico = ClsServer.CreaDatareader(strSql, Session("conn"))
            If dtrGenerico.HasRows = False Then
                strNominativo = ""
                dtrGenerico.Close()
                dtrGenerico = Nothing
            Else
                dtrGenerico.Read()
                strNominativo = dtrGenerico.Item("Nominativo")
                dtrGenerico.Close()
                dtrGenerico = Nothing
            End If

        End If
        If Not dtrGenerico Is Nothing Then
            dtrGenerico.Close()
            dtrGenerico = Nothing
        End If
        'controllo aggiunto da Jono il 15/12/2006
        'controllo nella tabella temporanea 
        'se esistono nel csv doppioni
        If strNominativo = "" Then
            strSql = "Select Cognome + ' ' + Nome As Nominativo From #TEMP_CODICI_IBAN Where CodiceIban like '%" & Trim(strIban) & "' "
            dtrGenerico = ClsServer.CreaDatareader(strSql, Session("conn"))
            If dtrGenerico.HasRows = False Then
                dtrGenerico.Close()
                dtrGenerico = Nothing
                strNominativo = ""
            Else
                dtrGenerico.Read()
                strNominativo = dtrGenerico.Item("Nominativo")
                dtrGenerico.Close()
                dtrGenerico = Nothing
            End If
        End If
        If Not dtrGenerico Is Nothing Then
            dtrGenerico.Close()
            dtrGenerico = Nothing
        End If
        Return strNominativo
    End Function



    Protected Sub CmdChiudi_Click(ByVal sender As Object, ByVal e As EventArgs) Handles CmdChiudi.Click
        'carico la home
        Response.Redirect("WfrmMain.aspx")
    End Sub
End Class