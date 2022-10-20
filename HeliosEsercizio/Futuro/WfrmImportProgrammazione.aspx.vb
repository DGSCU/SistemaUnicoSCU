Imports System.IO

Public Class WfrmImportProgrammazione
    Inherits System.Web.UI.Page
    Private Writer As StreamWriter
    Private strNote As String
    Private Tot As Integer
    Private TotOk As Integer
    Private TotKo As Integer
    Private NomeUnivoco As String
    Private xIdAttivita As Integer
    Private xIdCompetenza As Integer
    Private xIdBando As Integer

#Region "Utility"
    Private Sub ChiudiDataReader(ByRef dataReader As SqlClient.SqlDataReader)
        If Not dataReader Is Nothing Then
            dataReader.Close()
            dataReader = Nothing
        End If
    End Sub

    Private Sub VerificaSessione()
        If Not Session("LogIn") Is Nothing Then
            If Session("LogIn") = False Then
                Response.Redirect("LogOn.aspx")
            End If
        Else
            Response.Redirect("LogOn.aspx")
        End If
    End Sub
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        VerificaSessione()

        If ForzaCaricamentoProgrammazione(Session("Utente")) = True Then
            divForzaCaricamentoVerifiche.Visible = True
        End If
    End Sub

    Protected Sub cmdEsporta_Click(sender As Object, e As EventArgs) Handles cmdEsporta.Click
        EsportaVerificatore()
    End Sub

    Sub EsportaVerificatore()
        '*****************************************************************************************+
        'AUTORE: Simona Cordella
        'DATA: 01/07/2011
        'DESCRIZONE: Elenco Verificatori Interni

        Dim StrSql As String
        Dim dtrVer As SqlClient.SqlDataReader

        Dim Writer As StreamWriter
        Dim xLinea As String
        Dim NomeUnivoco As String
        Dim xPrefissoNome As String
        Dim IdRegCompetenzaUtente As Integer

        Try

            xPrefissoNome = Session("Utente")
            'ricavo la competenza dell'utente in sessione
            IdRegCompetenzaUtente = CompetenzaUtente(xPrefissoNome)

            'ricerco tutti i verificatori interni a secondo della competenza dell'Utente in sessione
            StrSql = " SELECT IDVerificatore, Cognome, Nome FROM TVerificatori WHERE Abilitato = 0 AND Tipologia = 0 AND IdRegCompetenza = " & IdRegCompetenzaUtente & ""
            dtrVer = ClsServer.CreaDatareader(StrSql, Session("conn"))
            NomeUnivoco = vbNullString

            If dtrVer.HasRows = False Then
                lblErr.Text = lblErr.Text & "Nessun Verificatore."
            Else

                NomeUnivoco = xPrefissoNome & "ExpVerificatori" & Year(Now) & Month(Now) & Day(Now) & Hour(Now) & Minute(Now) & Second(Now)
                Writer = New StreamWriter(Server.MapPath("download") & "\" & NomeUnivoco & ".CSV")
                '---intestazioni file
                xLinea = "CODICE VERIFICATORE;COGNOME;NOME"
                Writer.WriteLine(xLinea)

                xLinea = vbNullString

                While dtrVer.Read

                    'codice veirificatore
                    If IsDBNull(dtrVer(0)) = True Then
                        xLinea = vbNullString & ";"
                    Else
                        xLinea = ClsUtility.FormatExport(dtrVer(0)) & ";"
                    End If
                    'cognome
                    If IsDBNull(dtrVer(1)) = True Then
                        xLinea = xLinea & vbNullString & ";"
                    Else
                        xLinea = xLinea & ClsUtility.FormatExport(dtrVer(1)) & ";"
                    End If
                    'nome
                    If IsDBNull(dtrVer(2)) = True Then
                        xLinea = xLinea & vbNullString & ""
                    Else
                        xLinea = xLinea & ClsUtility.FormatExport(dtrVer(2)) & ""
                    End If

                    Writer.WriteLine(xLinea)

                End While
                hlVerificatore.NavigateUrl = "download\" & NomeUnivoco & ".CSV"

                hlVerificatore.Visible = True
                cmdEsporta.Visible = False
                Writer.Close()
                Writer = Nothing
            End If
            dtrVer.Close()
            dtrVer = Nothing
        Catch ex As Exception
            lblErr.Text = lblErr.Text & "Errore durante l'esportazione dei Verificatori."
            cmdEsporta.Visible = True
            If Not Writer Is Nothing Then
                Writer.Close()
                Writer = Nothing
            End If
            If Not dtrVer Is Nothing Then
                dtrVer.Close()
                dtrVer = Nothing
            End If
        End Try
    End Sub

    Private Function CompetenzaUtente(ByVal Utente As String) As Integer
        'Ricavo compentenza utente in sessione
        Dim dtrUte As SqlClient.SqlDataReader
        Dim strSql As String
        Dim IdReg As Integer

        strSql = "select IDREGIONECOMPETENZA from utentiUNSC  " & _
                " WHERE USERNAME= '" & Utente & "' "
        dtrUte = ClsServer.CreaDatareader(strSql, Session("conn"))


        dtrUte.Read()
        IdReg = dtrUte("IdRegioneCompetenza")

        dtrUte.Close()
        dtrUte = Nothing

        Return IdReg
    End Function

    Protected Sub CmdElabora_Click(sender As Object, e As EventArgs) Handles CmdElabora.Click

        lblMessaggioErrore.Visible = False
        lblMessaggioErrore.Text = ""
        If txtSelFile.FileName.ToString <> "" Then
            Dim file As String
            Dim estensione As String
            file = LCase(txtSelFile.FileName.ToString)
            estensione = file.Substring(file.Length - 4)
            If estensione = ".csv" Then
                If Session("Sistema") = "Helios" Then
                    UpLoad()
                Else
                    UpLoadGG()
                End If
            Else
                lblMessaggioErrore.Visible = True
                lblMessaggioErrore.Text = "Selezionare il file nel formato CSV."
                Exit Sub
            End If
        Else
            lblMessaggioErrore.Visible = True
            lblMessaggioErrore.Text = "Selezionare il file da inviare."
            Exit Sub
        End If

    End Sub

    Private Sub UpLoad()
        '--- salvataggio del file sul server
        Dim swErr As Boolean
        swErr = False
        If Not (txtSelFile.PostedFile Is Nothing) Then
            Try
                NomeUnivoco = "programmazione" & Session("Utente") & "_" & Year(Now) & Month(Now) & Day(Now) & Hour(Now) & Minute(Now) & Second(Now)
                txtSelFile.PostedFile.SaveAs(Server.MapPath("upload") & "\" & NomeUnivoco & ".csv")

                CreaTabTemp()

            Catch exc As Exception
                swErr = True
                CancellaTabellaTemp()
            End Try

            If swErr = False Then
                LeggiCSV()
            End If

        End If

    End Sub
    'OK 1
    Private Sub CancellaTabellaTemp()
        Dim strSql As String
        Dim cmdCanTempTable As SqlClient.SqlCommand
        Try
            '--- CANCELLO TAB TEMPORANEA
            strSql = "DROP TABLE [#TEMP_PROGRAMMAZIONE]"

            cmdCanTempTable = New SqlClient.SqlCommand
            cmdCanTempTable.CommandText = strSql
            cmdCanTempTable.Connection = Session("conn")
            cmdCanTempTable.ExecuteNonQuery()
        Catch e As Exception
        End Try

        cmdCanTempTable.Dispose()
    End Sub
    'OK 1
    Private Sub CreaTabTemp()
        'Gestione nuovi campi nel file d'importazione 
        'email,CodiceIstatiComuneDomicilio,IndirizzoDomicilio,NumeroDomicilio,CapDomicilio

        Dim strSql As String
        Dim cmdCreateTempTable As SqlClient.SqlCommand

        CancellaTabellaTemp()

        '--- CREO TAB TEMPORANEA
        strSql = "CREATE TABLE [#TEMP_PROGRAMMAZIONE] (" & _
                 "[CodiceProgetto] [nvarchar] (22) COLLATE DATABASE_DEFAULT, " & _
                 "[CodiceSede] [int], [CodiceVerificatore] [nvarchar] (100)) "

        cmdCreateTempTable = New SqlClient.SqlCommand
        cmdCreateTempTable.CommandText = strSql
        cmdCreateTempTable.Connection = Session("conn")
        cmdCreateTempTable.ExecuteNonQuery()
        cmdCreateTempTable.Dispose()
    End Sub

    'OK 1
    Private Sub LeggiCSV()
        '--- lettura del file
        Dim Reader As StreamReader
        Dim xLinea As String
        Dim ArrCampi() As String
        Dim swErr As Boolean
        Dim AppoNote As String
        Dim strsqlvol As String
        Dim dtrcontrollocodsede As SqlClient.SqlDataReader
        Dim mycommand As SqlClient.SqlCommand
        Dim blnCheckComune As Boolean
        Dim strIdComune As String
        Dim strCodCatasto As String
        Dim strNewCF As String
        Dim blnControlloDom As Boolean = False 'controllo campi domicilio
        Dim intIdRegioneCompetenza As Integer = 0
        Dim intIdBando As Integer = 0
        '--- Leggo il file di input e scrivo quello di output
        Reader = New StreamReader(Server.MapPath("upload") & "\" & NomeUnivoco & ".CSV", System.Text.Encoding.UTF7, False)

        Writer = New StreamWriter(Server.MapPath("download") & "\" & NomeUnivoco & ".CSV")

        '--- intestazione

        xLinea = Reader.ReadLine()
        Writer.WriteLine("Note;" & xLinea)

        ArrCampi = CreaArray(xLinea)
        If UBound(ArrCampi) < 2 Or UBound(ArrCampi) > 2 Then
            strNote = strNote & "Il numero delle colonne inserite è diverso da quello previsto."
            swErr = True
            TotKo = TotKo + 1
            Writer.WriteLine("Note;" & strNote)
            strNote = vbNullString
        End If

        Dim intRiga As Integer = 1
        '--- scorro le righe
        xLinea = Reader.ReadLine()
        While (xLinea <> "")
            swErr = False
            Tot = Tot + 1
            ArrCampi = CreaArray(xLinea)

            If UBound(ArrCampi) < 2 Then '21
                '--- se i campi non sono tutti errore
                strNote = "Il numero delle colonne inserite è minore di quello richieste."
                swErr = True
                TotKo = TotKo + 1
            Else
                If UBound(ArrCampi) > 2 Then '21
                    '--- se i campi sono troppi errore
                    strNote = "Il numero delle colonne inserite è maggiore di quello richieste."
                    swErr = True
                    TotKo = TotKo + 1
                Else
                    'CodiceProgetto
                    If Trim(ArrCampi(0)) = vbNullString Then
                        strNote = strNote & "Il campo CodiceProgetto e' un campo obbligatorio."
                        swErr = True
                    Else
                        If VerificaProgetto(Trim(ArrCampi(0))) = False Then

                            strNote = strNote & "Il CodiceProgetto indicato non e' un progetto esistente."
                            swErr = True
                        Else
                            Select Case Session("TipoUtente")
                                Case "U" 'UTENTE DI TIPO UNSC
                                    'controllo il primo progetto e salvo 'idcompetenza della regione
                                    If intRiga = 1 Then
                                        intIdRegioneCompetenza = VerificaCompetenzaProgetto(Trim(ArrCampi(0)))
                                        intIdBando = VerificaBandoProgetto(Trim(ArrCampi(0)))
                                        intRiga = intRiga + 1
                                        'CodiceSede
                                        If Trim(ArrCampi(1)) = vbNullString Then
                                            strNote = strNote & "Il campo CodiceSede e' un campo obbligatorio."
                                            swErr = True
                                        Else
                                            If IsNumeric(Trim(ArrCampi(1))) = False Then
                                                strNote = strNote & "Il campo CodiceSede non e' nel formato corretto."
                                                swErr = True
                                            Else
                                                'Verifico che la sede di attuazione sia legata al progetto
                                                If VerificaAttivitaEnteSede(Trim(ArrCampi(1)), Trim(ArrCampi(0))) = False Then
                                                    strNote = strNote & "La Sede indicata non e' tra le sedi del Progetto."
                                                    swErr = True
                                                End If
                                                'Verifico che ci siano volontari in servizio sulla sede
                                                If VerificaPresenzaVolontari(Trim(ArrCampi(1)), Trim(ArrCampi(0))) = False Then
                                                    strNote = strNote & "Non risultano Volontari In Servizo nella sede indicata."
                                                    swErr = True
                                                End If
                                            End If
                                        End If

                                    Else 'COMTROLLO I SUCCESSIVI PROGETTI
                                        'controllo se la regione di competenza del primo progetto è uguale al resto dei progetti del file
                                        If intIdRegioneCompetenza <> VerificaCompetenzaProgetto(Trim(ArrCampi(0))) Then
                                            strNote = strNote & "Il progetto indicato risulta diverso dalla competenza della programmazione che si sta importando."
                                            swErr = True
                                        Else
                                            '14/06/2011 Verifica Bando di Competenza
                                            'controllo se il bando del primo progetto è uguale al resto dei progetti del file
                                            If intIdBando <> VerificaBandoProgetto(Trim(ArrCampi(0))) Then
                                                strNote = strNote & "Il progetto indicato risulta diverso dalla circolare della programmazione che si sta importando."
                                                swErr = True
                                            Else
                                                'CodiceSede
                                                If Trim(ArrCampi(1)) = vbNullString Then
                                                    strNote = strNote & "Il campo CodiceSede e' un campo obbligatorio."
                                                    swErr = True
                                                Else
                                                    If IsNumeric(Trim(ArrCampi(1))) = False Then
                                                        strNote = strNote & "Il campo CodiceSede non e' nel formato corretto."
                                                        swErr = True
                                                    Else
                                                        'Verifico che la sede di attuazione sia legata al progetto
                                                        If VerificaAttivitaEnteSede(Trim(ArrCampi(1)), Trim(ArrCampi(0))) = False Then
                                                            strNote = strNote & "La Sede indicata non e' tra le sedi del Progetto."
                                                            swErr = True
                                                        End If
                                                        'Verifico che ci siano volontari in servizio sulla sede
                                                        If VerificaPresenzaVolontari(Trim(ArrCampi(1)), Trim(ArrCampi(0))) = False Then
                                                            strNote = strNote & "Non risultano Volontari In Servizo nella sede indicata."
                                                            swErr = True
                                                        End If
                                                    End If
                                                End If
                                            End If 'fine controllo idbando
                                        End If 'fine controllo Regione competenza
                                    End If ' fine controllo intriga
                                Case "R" 'UTENTE REGIONALE
                                    If intIdRegioneCompetenza = 0 Then
                                        intIdRegioneCompetenza = TrovaCompetenzaUtente()
                                    End If
                                    If intIdBando = 0 Then
                                        intIdBando = VerificaBandoProgetto(Trim(ArrCampi(0)))
                                    End If
                                    'controllo se la regione di competenza del primo progetto è uguale al resto dei progetti del file
                                    If intIdRegioneCompetenza <> VerificaCompetenzaProgetto(Trim(ArrCampi(0))) Then
                                        strNote = strNote & "Il progetto indicato risulta diverso dalla propria competenza."
                                        swErr = True
                                    Else
                                        '14/06/2011 Verifica Bando di Competenza
                                        'controllo se il bando del primo progetto è uguale al resto dei progetti del file
                                        If intIdBando <> VerificaBandoProgetto(Trim(ArrCampi(0))) Then
                                            strNote = strNote & "Il progetto indicato risulta diverso dalla circolare della programmazione che si sta importando."
                                            swErr = True
                                        Else
                                            'CodiceSede
                                            If Trim(ArrCampi(1)) = vbNullString Then
                                                strNote = strNote & "Il campo CodiceSede e' un campo obbligatorio."
                                                swErr = True
                                            Else
                                                If IsNumeric(Trim(ArrCampi(1))) = False Then
                                                    strNote = strNote & "Il campo CodiceSede non e' nel formato corretto."
                                                    swErr = True
                                                Else
                                                    'Verifico che la sede di attuazione sia legata al progetto
                                                    If VerificaAttivitaEnteSede(Trim(ArrCampi(1)), Trim(ArrCampi(0))) = False Then
                                                        strNote = strNote & "La Sede indicata non e' tra le sedi del Progetto."
                                                        swErr = True
                                                    End If
                                                    'Verifico che ci siano volontari in servizio sulla sede
                                                    If VerificaPresenzaVolontari(Trim(ArrCampi(1)), Trim(ArrCampi(0))) = False Then

                                                        strNote = strNote & "Non risultano Volontari In Servizo nella sede indicata."
                                                        swErr = True
                                                    End If
                                                End If
                                            End If
                                        End If 'fine controllo idbando
                                    End If 'fine controllo Regione competenza
                            End Select 'controllo TIPOUTENTE
                            'AGG. IL 03/04/2012 DA S.C. CONTROLLO SE LA SEDE è DUPLICATA NEL FILE
                            mycommand = New SqlClient.SqlCommand
                            mycommand.Connection = Session("conn")
                            strsqlvol = "select CodiceSede from #TEMP_PROGRAMMAZIONE where CodiceSede=" & Trim(ArrCampi(1)) & " AND CODICEPROGETTO ='" & Trim(ArrCampi(0)) & "' "
                            mycommand.CommandText = strsqlvol
                            dtrcontrollocodsede = mycommand.ExecuteReader

                            If dtrcontrollocodsede.HasRows = True Then

                                strNote = strNote & "Il CodiceSede per il progetto " & Trim(ArrCampi(0)) & " e' gia' presente nel file CSV."
                                swErr = True

                            End If

                            If Not dtrcontrollocodsede Is Nothing Then
                                dtrcontrollocodsede.Close()
                                dtrcontrollocodsede = Nothing
                            End If
                            'Dim strReturnPrg As String

                            'strReturnPrg = EsistenzaSedeVerificaProgrammazione(Trim(ArrCampi(0)), Trim(ArrCampi(1)))
                            'If strReturnPrg <> "" Then
                            '    strNote = strNote & " La sede e il progetto indicati sono stati già inseriti nella programmazione " & strReturnPrg & "."
                            '    swErr = True
                            'End If

                            If VerificaEsistenzaSedeVerifica(Trim(ArrCampi(0)), Trim(ArrCampi(1))) = True Then
                                strNote = strNote & " La sede e il progetto indicati sono stati già inseriti in una programmazione."
                                swErr = True
                            End If
                            'Verifico l'esistenza del Verificatore 
                            If Trim(ArrCampi(2)) = vbNullString Then
                                strNote = strNote & "Il campo CodiceVerificatore e' un campo obbligatorio."
                                swErr = True
                            Else
                                If IsNumeric(ArrCampi(2)) = False Then
                                    strNote = strNote & "Codice verificatore non numerico."
                                    swErr = True
                                Else
                                    If VerificaEsistenzaVerificatore(Trim(ArrCampi(2))) = False Then
                                        strNote = strNote & "Il verificatore indicato è inesistente."
                                        swErr = True
                                    End If
                                End If
                            End If
                    End If ' fine if VErificaprogetto
                    End If ' fine if codiceprogetto nullo
                    If swErr = False Then
                        ScriviTabTemp(ArrCampi)
                    Else
                        TotKo = TotKo + 1
                    End If
                End If
            End If
            Writer.WriteLine(strNote & ";" & xLinea)
            strNote = vbNullString
            xLinea = Reader.ReadLine()

        End While

        Reader.Close()
        Writer.Close()

        '--- reindirizzo la pagina sottostante
        Response.Redirect("WfrmRisultatoImportProgrammazione.aspx?IDBando=" & Trim(intIdBando) & " &IdRegCompetenza=" & Trim(intIdRegioneCompetenza) & " &NomeFile=" & NomeUnivoco & "&Tot=" & Tot & "&TotOk=" & TotOk & "&TotKo=" & TotKo)
        'Response.Write("<script>" & vbCrLf)
        'Response.Write("parent.Naviga('WfrmRisultatoImportProgrammazione.aspx?IDBando=" & Trim(intIdBando) & " &IdRegCompetenza=" & Trim(intIdRegioneCompetenza) & " &NomeFile=" & NomeUnivoco & "&Tot=" & Tot & "&TotOk=" & TotOk & "&TotKo=" & TotKo & "')" & vbCrLf)
        'Response.Write("</script>")

    End Sub

    'OK 1
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

        CreaArray = DefArr

    End Function

    'OK 1
    Private Sub ScriviTabTemp(ByVal pArray() As String)
        '--- scrive nella tab temporanea
        Dim cmdTemp As SqlClient.SqlCommand
        Dim strsql As String
        Dim Verificatore As String = ""


        Try
            'Verificatore = RicavaVerificatore(Trim(pArray(2)))

            strsql = "INSERT INTO #TEMP_PROGRAMMAZIONE "
            strsql = strsql & " (CodiceProgetto, "
            strsql = strsql & " CodiceSede,CodiceVerificatore )"
            strsql = strsql & " Values "
            strsql = strsql & " ('" & Trim(pArray(0)) & "',"
            strsql = strsql & " " & Trim(pArray(1)) & ", " & Trim(pArray(1)) & ")"

            cmdTemp = New SqlClient.SqlCommand
            cmdTemp.CommandText = strsql 'insert
            cmdTemp.Connection = Session("conn")
            cmdTemp.ExecuteNonQuery()
            cmdTemp.Dispose()
            TotOk = TotOk + 1

        Catch exc As Exception
            strNote = "Errore generico."
            TotKo = TotKo + 1

        End Try
    End Sub


#Region "GaranziaGiovani"
    Private Sub UpLoadGG()
        '--- salvataggio del file sul server
        Dim swErr As Boolean
        swErr = False
        If Not (txtSelFile.PostedFile Is Nothing) Then
            Try
                NomeUnivoco = "programmazione" & Session("Utente") & "_" & Year(Now) & Month(Now) & Day(Now) & Hour(Now) & Minute(Now) & Second(Now)

                txtSelFile.PostedFile.SaveAs(Server.MapPath("upload") & "\" & NomeUnivoco & ".csv")

                CreaTabTempGG()

            Catch exc As Exception
                swErr = True
                ''Response.Write("<script>" & vbCrLf)
                ''Response.Write("parent.fraUp.CambiaImmagine('filenoncaricato.jpg','0')" & vbCrLf)
                ''Response.Write("</script>")
                CancellaTabellaTempGG()
            End Try

            If swErr = False Then

                LeggiCSVGG()

                'Response.Write("<script>" & vbCrLf)
                'Response.Write("parent.fraUp.CambiaImmagine('allegatocompletato.jpg','1')" & vbCrLf)
                'Response.Write("</script>")
            End If

        End If

    End Sub

    Private Sub CreaTabTempGG()
        'Gestione nuovi campi nel file d'importazione 
        'email,CodiceIstatiComuneDomicilio,IndirizzoDomicilio,NumeroDomicilio,CapDomicilio

        Dim strSql As String
        Dim cmdCreateTempTable As SqlClient.SqlCommand

        CancellaTabellaTempGG()

        '--- CREO TAB TEMPORANEA
        strSql = "CREATE TABLE [#TEMP_PROGRAMMAZIONEGG] (" & _
                 "[CodiceProgetto] [nvarchar] (22) COLLATE DATABASE_DEFAULT, " & _
                 "[CodiceSede] [int], [CodiceVerificatore] [nvarchar] (100)) "

        cmdCreateTempTable = New SqlClient.SqlCommand
        cmdCreateTempTable.CommandText = strSql
        cmdCreateTempTable.Connection = Session("conn")
        cmdCreateTempTable.ExecuteNonQuery()
        cmdCreateTempTable.Dispose()
    End Sub
    Private Sub CancellaTabellaTempGG()
        Dim strSql As String
        Dim cmdCanTempTable As SqlClient.SqlCommand
        Try
            '--- CANCELLO TAB TEMPORANEA
            strSql = "DROP TABLE [#TEMP_PROGRAMMAZIONEGG]"

            cmdCanTempTable = New SqlClient.SqlCommand
            cmdCanTempTable.CommandText = strSql
            cmdCanTempTable.Connection = Session("conn")
            cmdCanTempTable.ExecuteNonQuery()
        Catch e As Exception
        End Try

        cmdCanTempTable.Dispose()
    End Sub
    Private Sub LeggiCSVGG()
        '--- lettura del file
        Dim Reader As StreamReader
        Dim xLinea As String
        Dim ArrCampi() As String
        Dim swErr As Boolean
        Dim AppoNote As String
        Dim strsqlvol As String
        Dim dtrcontrollocodsede As SqlClient.SqlDataReader
        Dim mycommand As SqlClient.SqlCommand
        Dim blnCheckComune As Boolean
        Dim strIdComune As String
        Dim strCodCatasto As String
        Dim strNewCF As String
        Dim blnControlloDom As Boolean = False 'controllo campi domicilio
        Dim intIdRegioneCompetenza As Integer = 0
        Dim intIdBando As Integer = 0
        '--- Leggo il file di input e scrivo quello di output
        Reader = New StreamReader(Server.MapPath("upload") & "\" & NomeUnivoco & ".CSV", System.Text.Encoding.UTF7, False)

        Writer = New StreamWriter(Server.MapPath("download") & "\" & NomeUnivoco & ".CSV")

        '--- intestazione

        xLinea = Reader.ReadLine()
        Writer.WriteLine("Note;" & xLinea)

        ArrCampi = CreaArray(xLinea)
        If UBound(ArrCampi) < 2 Or UBound(ArrCampi) > 2 Then
            strNote = strNote & "Il numero delle colonne inserite è diverso da quello previsto."
            swErr = True
            TotKo = TotKo + 1
            Writer.WriteLine("Note;" & strNote)
            strNote = vbNullString
        End If

        Dim intRiga As Integer = 1
        '--- scorro le righe
        xLinea = Reader.ReadLine()
        While (xLinea <> "")
            swErr = False
            Tot = Tot + 1
            ArrCampi = CreaArray(xLinea)

            If UBound(ArrCampi) < 2 Then '21
                '--- se i campi non sono tutti errore
                strNote = "Il numero delle colonne inserite è minore di quello richieste."
                swErr = True
                TotKo = TotKo + 1
            Else
                If UBound(ArrCampi) > 2 Then '21
                    '--- se i campi sono troppi errore
                    strNote = "Il numero delle colonne inserite è maggiore di quello richieste."
                    swErr = True
                    TotKo = TotKo + 1
                Else
                    'CodiceProgetto
                    If Trim(ArrCampi(0)) = vbNullString Then
                        strNote = strNote & "Il campo CodiceProgetto e' un campo obbligatorio."
                        swErr = True
                    Else
                        If VerificaProgetto(Trim(ArrCampi(0))) = False Then
                            strNote = strNote & "Il CodiceProgetto indicato non e' un progetto esistente."
                            swErr = True
                        Else
                            Select Case Session("TipoUtente")
                                Case "U" 'UTENTE DI TIPO UNSC
                                    'controllo il primo progetto e salvo 'idcompetenza della regione
                                    If intRiga = 1 Then
                                        intIdRegioneCompetenza = VerificaCompetenzaProgetto(Trim(ArrCampi(0)))
                                        intIdBando = VerificaBandoProgetto(Trim(ArrCampi(0)))
                                        intRiga = intRiga + 1
                                        'CodiceSede
                                        If Trim(ArrCampi(1)) = vbNullString Then
                                            strNote = strNote & "Il campo CodiceSede e' un campo obbligatorio."
                                            swErr = True
                                        Else
                                            If IsNumeric(Trim(ArrCampi(1))) = False Then
                                                strNote = strNote & "Il campo CodiceSede non e' nel formato corretto."
                                                swErr = True
                                            Else
                                                'Verifico che la sede di attuazione sia legata al progetto
                                                If VerificaAttivitaEnteSede(Trim(ArrCampi(1)), Trim(ArrCampi(0))) = False Then
                                                    strNote = strNote & "La Sede indicata non e' tra le sedi del Progetto."
                                                    swErr = True
                                                End If
                                                'Verifico che ci siano volontari in servizio sulla sede
                                                If VerificaPresenzaVolontari(Trim(ArrCampi(1)), Trim(ArrCampi(0))) = False Then
                                                    strNote = strNote & "Non risultano Volontari In Servizo nella sede indicata."
                                                    swErr = True
                                                End If
                                            End If
                                        End If

                                    Else 'COMTROLLO I SUCCESSIVI PROGETTI
                                        'controllo se la regione di competenza del primo progetto è uguale al resto dei progetti del file
                                        If intIdRegioneCompetenza <> VerificaCompetenzaProgetto(Trim(ArrCampi(0))) Then
                                            strNote = strNote & "Il progetto indicato risulta diverso dalla competenza della programmazione che si sta importando."
                                            swErr = True
                                        Else
                                            '14/06/2011 Verifica Bando di Competenza
                                            'controllo se il bando del primo progetto è uguale al resto dei progetti del file
                                            If intIdBando <> VerificaBandoProgetto(Trim(ArrCampi(0))) Then
                                                strNote = strNote & "Il progetto indicato risulta diverso dalla circolare della programmazione che si sta importando."
                                                swErr = True
                                            Else
                                                'CodiceSede
                                                If Trim(ArrCampi(1)) = vbNullString Then
                                                    strNote = strNote & "Il campo CodiceSede e' un campo obbligatorio."
                                                    swErr = True
                                                Else
                                                    If IsNumeric(Trim(ArrCampi(1))) = False Then
                                                        strNote = strNote & "Il campo CodiceSede non e' nel formato corretto."
                                                        swErr = True
                                                    Else
                                                        'Verifico che la sede di attuazione sia legata al progetto
                                                        If VerificaAttivitaEnteSede(Trim(ArrCampi(1)), Trim(ArrCampi(0))) = False Then
                                                            strNote = strNote & "La Sede indicata non e' tra le sedi del Progetto."
                                                            swErr = True
                                                        End If
                                                        'Verifico che ci siano volontari in servizio sulla sede
                                                        If VerificaPresenzaVolontari(Trim(ArrCampi(1)), Trim(ArrCampi(0))) = False Then
                                                            strNote = strNote & "Non risultano Volontari In Servizo nella sede indicata."
                                                            swErr = True
                                                        End If
                                                    End If
                                                End If
                                            End If 'fine controllo idbando
                                        End If 'fine controllo Regione competenza
                                    End If ' fine controllo intriga
                                Case "R" 'UTENTE REGIONALE
                                    intIdRegioneCompetenza = TrovaCompetenzaUtente()
                                    intIdBando = VerificaBandoProgetto(Trim(ArrCampi(0)))
                                    'controllo se la regione di competenza del primo progetto è uguale al resto dei progetti del file
                                    If intIdRegioneCompetenza <> VerificaCompetenzaProgetto(Trim(ArrCampi(0))) Then
                                        strNote = strNote & "Il progetto indicato risulta diverso dalla propria competenza."
                                        swErr = True
                                    Else
                                        '14/06/2011 Verifica Bando di Competenza
                                        'controllo se il bando del primo progetto è uguale al resto dei progetti del file
                                        If intIdBando <> VerificaBandoProgetto(Trim(ArrCampi(0))) Then
                                            strNote = strNote & "Il progetto indicato risulta diverso dalla circolare della programmazione che si sta importando."
                                            swErr = True
                                        Else
                                            'CodiceSede
                                            If Trim(ArrCampi(1)) = vbNullString Then
                                                strNote = strNote & "Il campo CodiceSede e' un campo obbligatorio."
                                                swErr = True
                                            Else
                                                If IsNumeric(Trim(ArrCampi(1))) = False Then
                                                    strNote = strNote & "Il campo CodiceSede non e' nel formato corretto."
                                                    swErr = True
                                                Else
                                                    'Verifico che la sede di attuazione sia legata al progetto
                                                    If VerificaAttivitaEnteSede(Trim(ArrCampi(1)), Trim(ArrCampi(0))) = False Then
                                                        strNote = strNote & "La Sede indicata non e' tra le sedi del Progetto."
                                                        swErr = True
                                                    End If
                                                    'Verifico che ci siano volontari in servizio sulla sede
                                                    If VerificaPresenzaVolontari(Trim(ArrCampi(1)), Trim(ArrCampi(0))) = False Then
                                                        strNote = strNote & "Non risultano Volontari In Servizo nella sede indicata."
                                                        swErr = True
                                                    End If
                                                End If
                                            End If
                                        End If 'fine controllo idbando
                                    End If 'fine controllo Regione competenza
                            End Select 'controllo TIPOUTENTE
                            'AGG. IL 03/04/2012 DA S.C. CONTROLLO SE LA SEDE è DUPLICATA NEL FILE
                            mycommand = New SqlClient.SqlCommand
                            mycommand.Connection = Session("conn")
                            strsqlvol = "select CodiceSede from #TEMP_PROGRAMMAZIONEGG where CodiceSede=" & Trim(ArrCampi(1)) & " AND CODICEPROGETTO ='" & Trim(ArrCampi(0)) & "' "
                            mycommand.CommandText = strsqlvol
                            dtrcontrollocodsede = mycommand.ExecuteReader

                            If dtrcontrollocodsede.HasRows = True Then

                                strNote = strNote & "Il CodiceSede per il progetto " & Trim(ArrCampi(0)) & " e' gia' presente nel file CSV."
                                swErr = True

                            End If

                            If Not dtrcontrollocodsede Is Nothing Then
                                dtrcontrollocodsede.Close()
                                dtrcontrollocodsede = Nothing
                            End If
                            'Dim strReturnPrg As String

                            'strReturnPrg = EsistenzaSedeVerificaProgrammazione(Trim(ArrCampi(0)), Trim(ArrCampi(1)))
                            'If strReturnPrg <> "" Then
                            '    strNote = strNote & " La sede e il progetto indicati sono stati già inseriti nella programmazione " & strReturnPrg & "."
                            '    swErr = True
                            'End If

                            If VerificaEsistenzaSedeVerifica(Trim(ArrCampi(0)), Trim(ArrCampi(1))) = True Then
                                strNote = strNote & " La sede e il progetto indicati sono stati già inseriti in una programmazione."
                                swErr = True
                            End If
                            'Verifico l'esistenza del Verificatore 
                            If Trim(ArrCampi(2)) = vbNullString Then
                                strNote = strNote & "Il campo CodiceVerificatore e' un campo obbligatorio."
                                swErr = True
                            Else
                                If IsNumeric(ArrCampi(2)) = False Then
                                    strNote = strNote & "Codice verificatore non numerico."
                                    swErr = True
                                Else
                                    If VerificaEsistenzaVerificatore(Trim(ArrCampi(2))) = False Then
                                        strNote = strNote & "Il verificatore indicato è inesistente."
                                        swErr = True
                                    End If
                                End If
                            End If
                    End If ' fine if VErificaprogetto
                    End If ' fine if codiceprogetto nullo
                    If swErr = False Then
                        ScriviTabTempGG(ArrCampi)
                    Else
                        TotKo = TotKo + 1
                    End If
                End If
            End If
            Writer.WriteLine(strNote & ";" & xLinea)
            strNote = vbNullString
            xLinea = Reader.ReadLine()

        End While

        Reader.Close()
        Writer.Close()

        '--- reindirizzo la pagina sottostante
        Response.Redirect("WfrmRisultatoImportProgrammazione.aspx?IDBando=" & Trim(intIdBando) & " &IdRegCompetenza=" & Trim(intIdRegioneCompetenza) & " &NomeFile=" & NomeUnivoco & "&Tot=" & Tot & "&TotOk=" & TotOk & "&TotKo=" & TotKo)
        'Response.Write("<script>" & vbCrLf)
        'Response.Write("parent.Naviga('WfrmRisultatoImportProgrammazione.aspx?IDBando=" & Trim(intIdBando) & " &IdRegCompetenza=" & Trim(intIdRegioneCompetenza) & " &NomeFile=" & NomeUnivoco & "&Tot=" & Tot & "&TotOk=" & TotOk & "&TotKo=" & TotKo & "')" & vbCrLf)
        'Response.Write("</script>")

    End Sub
    Private Sub ScriviTabTempGG(ByVal pArray() As String)
        '--- scrive nella tab temporanea
        Dim cmdTemp As SqlClient.SqlCommand
        Dim strsql As String
        Dim Verificatore As String = ""


        Try
            'Verificatore = RicavaVerificatore(Trim(pArray(2)))

            strsql = "INSERT INTO #TEMP_PROGRAMMAZIONEGG "
            strsql = strsql & " (CodiceProgetto, "
            strsql = strsql & " CodiceSede,CodiceVerificatore )"
            strsql = strsql & " Values "
            strsql = strsql & " ('" & Trim(pArray(0)) & "',"
            strsql = strsql & " " & Trim(pArray(1)) & ", " & Trim(pArray(1)) & ")"

            cmdTemp = New SqlClient.SqlCommand
            cmdTemp.CommandText = strsql 'insert
            cmdTemp.Connection = Session("conn")
            cmdTemp.ExecuteNonQuery()
            cmdTemp.Dispose()
            TotOk = TotOk + 1

        Catch exc As Exception
            strNote = "Errore generico."
            TotKo = TotKo + 1

        End Try
    End Sub

#End Region

    Private Function VerificaProgetto(ByVal pCodiceProgetto As String) As Boolean
        Dim dtrProgetto As SqlClient.SqlDataReader
        Dim strSql As String
        xIdAttivita = 0

        strSql = "SELECT DISTINCT attività.CodiceEnte, attività.IDAttività, attivitàentisediattuazione.IDEnteSedeAttuazione  " & _
                " FROM  attività " & _
                " INNER JOIN  attivitàentisediattuazione ON attività.IDAttività = attivitàentisediattuazione.IDAttività " & _
                " INNER JOIN  attivitàentità ON attivitàentisediattuazione.IDAttivitàEnteSedeAttuazione = attivitàentità.IDAttivitàEnteSedeAttuazione " & _
                " INNER JOIN TIPIPROGETTO ON tipiprogetto.idtipoprogetto = attività.idtipoprogetto " & _
                " WHERE TipiProgetto.MacroTipoProgetto like '" & Session("FiltroVisibilita") & "' " & _
                " AND attività.CodiceEnte= '" & pCodiceProgetto & "' "

        If ChkForzaCaricamento.Checked = False Then
            strSql &= " AND attività.IDStatoAttività = 1 AND attivitàentità.DataFineAttivitàEntità > GETDATE()"
        Else
            strSql &= " AND attività.IDStatoAttività in (1,2) "
        End If

        'AND entità.IDStatoEntità = 3
        dtrProgetto = ClsServer.CreaDatareader(strSql, Session("conn"))

        If dtrProgetto.HasRows = True Then
            dtrProgetto.Read()
            xIdAttivita = dtrProgetto("IDAttività")
        End If
        VerificaProgetto = dtrProgetto.HasRows

        dtrProgetto.Close()
        dtrProgetto = Nothing

    End Function
    Private Function VerificaCompetenzaProgetto(ByVal pCodiceProgetto As String) As Integer
        Dim dtrProgetto As SqlClient.SqlDataReader
        Dim strSql As String
        xIdCompetenza = 0

        strSql = "SELECT IdRegioneCompetenza  " & _
                " FROM  attività " & _
                " WHERE CodiceEnte= '" & pCodiceProgetto & "' "
        dtrProgetto = ClsServer.CreaDatareader(strSql, Session("conn"))

        If dtrProgetto.HasRows = True Then
            dtrProgetto.Read()
            xIdCompetenza = dtrProgetto("IdRegioneCompetenza")
        End If
        VerificaCompetenzaProgetto = xIdCompetenza

        dtrProgetto.Close()
        dtrProgetto = Nothing
    End Function
    Private Function VerificaAttivitaEnteSede(ByVal pCodiceSede As String, ByVal pCodiceAttivita As String) As Boolean
        Dim dtrAttivitaEnteSede As SqlClient.SqlDataReader
        Dim strSql As String

        strSql = "SELECT IdAttivitàEnteSedeAttuazione FROM AttivitàEntiSediAttuazione " & _
                 "INNER JOIN Attività ON AttivitàEntiSediAttuazione.IdAttività = Attività.IdAttività " & _
                 "WHERE Attività.CodiceEnte = '" & pCodiceAttivita & "' " & _
                 "AND AttivitàEntiSediAttuazione.IdEnteSedeAttuazione = " & pCodiceSede

        dtrAttivitaEnteSede = ClsServer.CreaDatareader(strSql, Session("conn"))

        'Se esiste ritorna True 
        VerificaAttivitaEnteSede = dtrAttivitaEnteSede.HasRows

        dtrAttivitaEnteSede.Close()
        dtrAttivitaEnteSede = Nothing
    End Function

    Private Function VerificaPresenzaVolontari(ByVal pCodiceSede As String, ByVal pCodiceAttivita As String) As Boolean
        'aggiunto da simona cordella il 05/04/2011
        'verifica se ci sono volntari in serivzio sulla sede di progetto
        Dim dtrAttivitaVol As SqlClient.SqlDataReader
        Dim strSql As String

        strSql = "select count(distinct ae.identità) AS VOL " & _
                " from attività a  " & _
                " inner join   attivitàentisediattuazione aesa on a.IDAttività =aesa.IDAttività " & _
                " inner join attivitàentità ae on ae.IDAttivitàentesedeattuazione =aesa.IDAttivitàentesedeattuazione " & _
                " inner join entità e on ae.identità =e.identità " & _
                " where A.CodiceEnte = '" & pCodiceAttivita & "' " & _
                " AND aesa.IdEnteSedeAttuazione = " & pCodiceSede & " And ae.idstatoattivitàentità = 1 "

        If ChkForzaCaricamento.Checked = False Then
            strSql &= " AND E.idstatoentità = 3 "
        Else
            strSql &= " AND E.idstatoentità in (3,5,6) "
        End If

        dtrAttivitaVol = ClsServer.CreaDatareader(strSql, Session("conn"))

        If dtrAttivitaVol.HasRows = True Then
            dtrAttivitaVol.Read()
            If dtrAttivitaVol("VOL") = 0 Then 'non ci sno volontari
                VerificaPresenzaVolontari = False
            Else 'ci sono volontari
                VerificaPresenzaVolontari = True
            End If
        End If

        dtrAttivitaVol.Close()
        dtrAttivitaVol = Nothing
    End Function

    Private Function TrovaCompetenzaUtente() As Integer
        '01/07/2011
        'Trovo la competenza dell'Utente
        Dim strSQL As String
        Dim dtrCompetenze As System.Data.SqlClient.SqlDataReader
        Dim intIdCompetenza As Integer = 0

        strSQL = "select b.IdRegioneCompetenza ,b.Heliosread from RegioniCompetenze a "
        strSQL = strSQL & "INNER JOIN utentiunsc b ON a.idregionecompetenza = b.idregionecompetenza "
        strSQL = strSQL & "where b.username = '" & Session("Utente") & "'"

        If Not dtrCompetenze Is Nothing Then
            dtrCompetenze.Close()
            dtrCompetenze = Nothing
        End If
        dtrCompetenze = ClsServer.CreaDatareader(strSQL, Session("conn"))
        dtrCompetenze.Read()
        If dtrCompetenze.HasRows = True Then
            intIdCompetenza = dtrCompetenze("IdRegioneCompetenza")
        End If
        If Not dtrCompetenze Is Nothing Then
            dtrCompetenze.Close()
            dtrCompetenze = Nothing
        End If
        TrovaCompetenzaUtente = intIdCompetenza
    End Function
    Private Function VerificaBandoProgetto(ByVal pCodiceProgetto As String) As Integer
        Dim dtrProgetto As SqlClient.SqlDataReader
        Dim strSql As String
        xIdBando = 0

        strSql = " Select b.IdBando " & _
                " FROM attività a  " & _
                " INNER JOIN BandiAttività ba ON a.IDBandoAttività = ba.IdBandoAttività" & _
                " inner join AssociaBandoRegioniCompetenze  b on b.idbando=ba.idbando " & _
                " WHERE CodiceEnte= '" & pCodiceProgetto & "' "
        dtrProgetto = ClsServer.CreaDatareader(strSql, Session("conn"))

        If dtrProgetto.HasRows = True Then
            dtrProgetto.Read()
            xIdBando = dtrProgetto("IdBando")
        End If
        VerificaBandoProgetto = xIdBando

        dtrProgetto.Close()
        dtrProgetto = Nothing
    End Function

    Private Function VerificaEsistenzaVerificatore(ByVal pCodiceVerificatore As String) As Boolean
        Dim idCompetenzaUtente As Integer
        Dim StrSql As String
        Dim dtrVer As SqlClient.SqlDataReader
        idCompetenzaUtente = TrovaCompetenzaUtente()

        'ricerco tutti i verificatori interni a secondo della competenza dell'Utente in sessione
        StrSql = " SELECT IDVerificatore, Cognome, Nome FROM TVerificatori WHERE IDVerificatore = " & pCodiceVerificatore & " And IdRegCompetenza = " & idCompetenzaUtente & ""
        dtrVer = ClsServer.CreaDatareader(StrSql, Session("conn"))

        'Se esiste ritorna True 
        VerificaEsistenzaVerificatore = dtrVer.HasRows

        dtrVer.Close()
        dtrVer = Nothing

    End Function

    Private Function EsistenzaSedeVerificaProgrammazione(ByVal CodiceProgetto As String, ByVal IDESA As String) As String
        'creato da simona cordella il 29/05/2012
        'IDAESA --> IDATTIVITàENTESEDEATTUAZIONE
        Dim strSql As String
        Dim rstProg As SqlClient.SqlDataReader
        Dim IDAESA As String = "0"

        Dim strReturnProg As String = ""
        'ricavo l'idattivitàentesedeattuazione
        strSql = " Select attivitàentisediattuazione.IDAttivitàEnteSedeAttuazione " & _
                " FROM attività " & _
                " INNER JOIN attivitàentisediattuazione ON attività.IDAttività = attivitàentisediattuazione.IDAttività " & _
                " WHERE attività.CodiceEnte='" & CodiceProgetto & "' and attivitàentisediattuazione.IDEnteSedeAttuazione = '" & IDESA & "' "

        rstProg = ClsServer.CreaDatareader(strSql, Session("Conn"))
        If rstProg.HasRows = True Then
            rstProg.Read()
            IDAESA = rstProg("IDAttivitàEnteSedeAttuazione")
        End If
        If Not rstProg Is Nothing Then
            rstProg.Close()
            rstProg = Nothing
        End If

        strSql = " SELECT  dbo.FN_EsistenzaSedeVerificaSegnalata(" & IDAESA & ") as EsistenzaSede, TVerificheProgrammazione.DESCRIZIONE " & _
                 " FROM TVerificheAssociate " & _
                 " INNER JOIN TVerifiche ON TVerificheAssociate.IDVerifica = TVerifiche.IDVerifica " & _
                 " INNER JOIN TVerificheProgrammazione ON TVerifiche.IDProgrammazione = TVerificheProgrammazione.IDProgrammazione " & _
                 " WHERE IDAttivitàEnteSedeAttuazione = " & IDAESA & " "
        rstProg = ClsServer.CreaDatareader(strSql, Session("Conn"))
        If rstProg.HasRows = True Then
            rstProg.Read()
            If rstProg("EsistenzaSede") = 1 Then
                strReturnProg = rstProg("DESCRIZIONE")
            End If
        End If

        If Not rstProg Is Nothing Then
            rstProg.Close()
            rstProg = Nothing
        End If
        Return strReturnProg
    End Function
    Private Function VerificaEsistenzaSedeVerifica(ByVal CodiceProgetto As String, ByVal IDESA As String) As Boolean
        'creato da simona cordella il 29/05/2012
        'IDAESA --> IDATTIVITàENTESEDEATTUAZIONE
        Dim strSql As String
        Dim rstProg As SqlClient.SqlDataReader
        Dim IDAESA As String = "0"
        Dim bytSede As Boolean = False

        strSql = " SELECT DBO.FN_EsistenzaSedeVerifica('" & CodiceProgetto & "'," & IDESA & ")  AS EsistenzaSede   "
        rstProg = ClsServer.CreaDatareader(strSql, Session("Conn"))
        If rstProg.HasRows = True Then
            rstProg.Read()
            bytSede = rstProg("EsistenzaSede")
        End If

        If Not rstProg Is Nothing Then
            rstProg.Close()
            rstProg = Nothing
        End If
        Return bytSede

    End Function

    Private Function ForzaCaricamentoProgrammazione(ByVal Utente As String) As Boolean
        '*** CREATO da Simona Cordella il 21/03/2017
        '** Verifico se l'utenza è abilitata alla visibilità del flag che consente il caricamento della programamzione con volontari e progetti terminati
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
                 " WHERE VociMenu.descrizione = 'Forza Caricamento Programmazione'" & _
                 " AND AssociaUtenteGruppo.username ='" & Utente & "' and (RestrizioniUtenteVociMenu.TipoRestrizione <> 0 or RestrizioniUtenteVociMenu.TipoRestrizione is null)"
        dtrgenerico = ClsServer.CreaDatareader(strSql, Session("Conn"))

        ForzaCaricamentoProgrammazione = dtrgenerico.Read()
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
    End Function


    Protected Sub CmdChiudi_Click(sender As Object, e As EventArgs) Handles CmdChiudi.Click
        Response.Redirect("WfrmMain.aspx")
    End Sub



End Class