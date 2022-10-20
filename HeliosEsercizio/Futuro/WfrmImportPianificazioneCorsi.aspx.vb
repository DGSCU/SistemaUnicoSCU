Imports System.IO

Public Class WfrmImportPianificazioneCorsi
    Inherits System.Web.UI.Page
    Private Writer As StreamWriter
    Private strNote As String
    Private Tot As Integer
    Private TotOk As Integer
    Private TotKo As Integer
    Private NomeUnivoco As String
    Private xIdEnteRiferimento As Integer

    Private Const UNICA_TRANCE As Short = 1
    Private Const DOPPIA_TRANCE As Short = 2

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not Session("LogIn") Is Nothing Then
            If Session("LogIn") = False Then 'verifico validità log-in
                Response.Redirect("LogOn.aspx")
            End If
        Else
            Response.Redirect("LogOn.aspx")
        End If

    End Sub

    Private Sub cmdChiudi_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdChiudi.Click
        'carico la home
        Response.Redirect("WfrmMain.aspx")
    End Sub

    Private Sub CmdElaboraUnicaTrance_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdElaboraUnicaTrance.Click
        UpLoad(UNICA_TRANCE)
    End Sub

    Private Sub CmdElaboraDoppiaTrance_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdElaboraDoppiaTrance.Click
        UpLoad(DOPPIA_TRANCE)
    End Sub

    Private Sub UpLoad(ByVal TipoFormazioneGenerale As Short)
        '--- salvataggio del file sul server
        Dim swErr As Boolean
        swErr = False

        If TipoFormazioneGenerale = UNICA_TRANCE Then
            If txtSelFileUnicaTrance.PostedFile.FileName.ToString = "" Then

                lblMessaggioErroreUnicaTrance.Visible = True
                lblMessaggioErroreUnicaTrance.Text = "Selezionare il file da inviare."
                Exit Sub
            End If
        End If
        If TipoFormazioneGenerale = DOPPIA_TRANCE Then
            If txtSelFileDoppiaTrance.PostedFile.FileName.ToString = "" Then

                lblMessaggioErroreDoppiaTrance.Visible = True
                lblMessaggioErroreDoppiaTrance.Text = "Selezionare il file da inviare."
                Exit Sub
            End If

        End If
        If (TipoFormazioneGenerale = UNICA_TRANCE And (txtSelFileUnicaTrance.PostedFile.FileName.Trim <> String.Empty)) Or (TipoFormazioneGenerale = DOPPIA_TRANCE And (txtSelFileDoppiaTrance.PostedFile.FileName.Trim <> String.Empty)) Then

            Try
                Dim file As String
                Dim estensione As String
                NomeUnivoco = "Formazionevolontari" & Session("IdEnte") & "_" & Session("Utente") & "_" & Year(Now) & Month(Now) & Day(Now) & Hour(Now) & Minute(Now) & Second(Now)

                If TipoFormazioneGenerale = UNICA_TRANCE Then
                    file = LCase(txtSelFileUnicaTrance.FileName.ToString)
                    estensione = file.Substring(file.Length - 4)
                    If estensione <> ".csv" Then
                        lblMessaggioErroreUnicaTrance.Visible = True
                        lblMessaggioErroreUnicaTrance.Text = "Selezionare il file nel formato CSV."
                        Exit Sub
                    End If
                    txtSelFileUnicaTrance.PostedFile.SaveAs(Server.MapPath("upload") & "\" & NomeUnivoco & ".csv")
                    CreaTabTemp()
                Else
                    file = LCase(txtSelFileDoppiaTrance.FileName.ToString)
                    estensione = file.Substring(file.Length - 4)
                    If estensione <> ".csv" Then
                        lblMessaggioErroreDoppiaTrance.Visible = True
                        lblMessaggioErroreDoppiaTrance.Text = "Selezionare il file nel formato CSV."
                        Exit Sub
                    End If
                    txtSelFileDoppiaTrance.PostedFile.SaveAs(Server.MapPath("upload") & "\" & NomeUnivoco & ".csv")
                    CreaTabTempTranche()
                End If

            Catch exc As Exception
                swErr = True
                'Response.Write("<script>" & vbCrLf)
                'Response.Write("parent.fraUp.CambiaImmagine('filenoncaricato.jpg','0')" & vbCrLf)
                'Response.Write("</script>")
                CancellaTabellaTemp()
            End Try

            If swErr = False Then

                If TipoFormazioneGenerale = UNICA_TRANCE Then
                    LeggiCSV()
                Else
                    LeggiCSVTranche()
                End If

                'Response.Write("<script>" & vbCrLf)
                'Response.Write("parent.fraUp.CambiaImmagine('allegatocompletato.jpg','1')" & vbCrLf)
                'Response.Write("</script>")
            End If

        End If

    End Sub

    Private Sub CreaTabTemp()
        Dim strSql As String
        Dim cmdCreateTempTable As SqlClient.SqlCommand

        CancellaTabellaTemp()

        '--- CREO TAB TEMPORANEA
        Try
            strSql = "CREATE TABLE [#TEMP_FORMAZIONE_VOLONATARIO] (" & _
                     "[CodiceProgetto] [nvarchar] (22) , " & _
                     "[Titolo] [nvarchar](255) , " & _
                     "[Bando] [nvarchar](255) , " & _
                     "[NumeroVolontari] [nvarchar](10) , " & _
                     "[DataAvvio] [nvarchar](20) , " & _
                     "[DataFine] [nvarchar] (20), " & _
                     "[SedeSvolgimento] [nvarchar] (1000) , " & _
                     "[Riferimento] [nvarchar](100),  " & _
                     ")"
            cmdCreateTempTable = New SqlClient.SqlCommand
            cmdCreateTempTable.CommandText = strSql
            cmdCreateTempTable.Connection = Session("conn")
            cmdCreateTempTable.ExecuteNonQuery()
            cmdCreateTempTable.Dispose()

        Catch exc As Exception
            Response.Write(exc.Message)
        End Try
    End Sub

    Private Sub CancellaTabellaTemp()
        Dim strSql As String
        Dim cmdCanTempTable As SqlClient.SqlCommand
        Try
            '--- CANCELLO TAB TEMPORANEA
            strSql = "DROP TABLE [#TEMP_FORMAZIONE_VOLONATARIO]"

            cmdCanTempTable = New SqlClient.SqlCommand
            cmdCanTempTable.CommandText = strSql
            cmdCanTempTable.Connection = Session("conn")
            cmdCanTempTable.ExecuteNonQuery()
        Catch e As Exception
            Response.Write(e.Message)
        End Try

        cmdCanTempTable.Dispose()
    End Sub

    Private Sub CreaTabTempTranche()
        Dim strSql As String
        Dim cmdCreateTempTable As SqlClient.SqlCommand

        CancellaTabellaTemp()

        '--- CREO TAB TEMPORANEA
        Try
            strSql = "CREATE TABLE [#TEMP_FORMAZIONE_VOLONATARIO] (" & _
                     "[CodiceProgetto] [nvarchar] (22) , " & _
                     "[Titolo] [nvarchar](255) , " & _
                     "[Bando] [nvarchar](255) , " & _
                     "[NumeroVolontari] [nvarchar](10) , " & _
                     "[DataAvvio] [nvarchar](20) , " & _
                     "[DataFine] [nvarchar] (20), " & _
                     "[SedeSvolgimento] [nvarchar] (1000) , " & _
                     "[Riferimento] [nvarchar](100),  " & _
                     "[DataAvvioTranche] [nvarchar](20) , " & _
                     "[DataFineTranche] [nvarchar] (20), " & _
                     "[SedeSvolgimentoTranche] [nvarchar] (1000) , " & _
                     "[RiferimentoTranche] [nvarchar](100) " & _
                     ")"
            cmdCreateTempTable = New SqlClient.SqlCommand
            cmdCreateTempTable.CommandText = strSql
            cmdCreateTempTable.Connection = Session("conn")
            cmdCreateTempTable.ExecuteNonQuery()
            cmdCreateTempTable.Dispose()

        Catch exc As Exception
            Response.Write(exc.Message)
        End Try
    End Sub

    Private Sub LeggiCSV()
        '--- lettura del file
        Dim Reader As StreamReader
        Dim xLinea As String
        Dim ArrCampi() As String
        Dim swErr As Boolean
        Dim AppoNote As String


        '--- Leggo il file di input e scrivo quello di output
        Reader = New StreamReader(Server.MapPath("upload") & "\" & NomeUnivoco & ".CSV", System.Text.Encoding.UTF7, False)

        Writer = New StreamWriter(Server.MapPath("download") & "\" & NomeUnivoco & ".CSV")

        '--- intestazione

        xLinea = Reader.ReadLine()
        Writer.WriteLine("Note;" & xLinea)

        '--- scorro le righe
        xLinea = Reader.ReadLine()
        While (xLinea <> "")
            swErr = False
            Tot = Tot + 1
            ArrCampi = CreaArray(xLinea)

            If UBound(ArrCampi) < 7 Then
                '--- se i campi non sono tutti errore
                strNote = "Il numero delle colonne inserite è minore di quello richieste."
                swErr = True
                TotKo = TotKo + 1
            Else
                If UBound(ArrCampi) > 7 Then
                    '--- se i campi sono troppi errore
                    strNote = "Il numero delle colonne inserite è maggiore di quello richieste."
                    swErr = True
                    TotKo = TotKo + 1

                Else
                    'CodiceProgetto
                    If Trim(ArrCampi(0)) = vbNullString Then
                        strNote = strNote & "Il campo CodiceProgetto e' un campo obbligatorio."
                        swErr = True
                    End If
                    'Stato Progetto Attivo
                    If Trim(ArrCampi(0)) <> "" Then
                        If VerificaStato(Trim(ArrCampi(0))) = 0 Then
                            strNote = strNote & "Il progetto non risulta 'Attivo' per l'ente."
                            swErr = True
                        End If
                    End If
                    'Verifica Progetto Dati di Pianificazione
                    If VerificaPrevisioneCaricamento(Trim(ArrCampi(0))) = False Then
                        strNote = strNote & "Per il progetto indicato non è previsto il caricamento dei dati di pianificazione."
                        swErr = True
                    End If

                    'verifica partenza progetto
                    If VerificaPartenzaProgetto(Trim(ArrCampi(0))) = False Then
                        strNote = strNote & "Il progetto in questione non risulta ancora avviato."
                        swErr = True
                    Else
                        'verifica termini progetto
                        If VerificaScadenzaProgrammazioneCorsiProroga(Trim(ArrCampi(0))) = False Then
                            strNote = strNote & "Per il progetto in questione sono scaduti i termini per il caricamento."
                            swErr = True
                        End If
                        'Data Avvio
                        If Trim(ArrCampi(4)) = vbNullString Then
                            strNote = strNote & "Il campo Data Avvio e' un campo obbligatorio."
                            swErr = True
                        Else
                            If IsDate(ArrCampi(4)) = False Or InStr(ArrCampi(4), "-") <> 0 Then
                                strNote = strNote & "Il campo DataAvvio non e' nel formato corretto."
                                swErr = True
                            Else
                                If Len(Trim(ArrCampi(4))) = 8 Then
                                    ArrCampi(4) = Trim(ArrCampi(4))
                                    ArrCampi(4) = Left(ArrCampi(4), 6) & "20" & Right(ArrCampi(4), 2)
                                End If
                            End If
                        End If
                        If IsDate(ArrCampi(4)) = True Then
                            If VerificaDataAvvioCorso(Trim(ArrCampi(0)), Trim(ArrCampi(4))) = False Then
                                strNote = strNote & "Il campo DataAvvio non e' compreso nell'intervallo corretto."
                                swErr = True
                            End If
                        Else
                            strNote = strNote & "Impossibile controllare l'intervallo di tempo in quanto il formato della data di avvio non e' corretto."
                            swErr = True
                        End If

                        'Data Fine
                        If Trim(ArrCampi(5)) = vbNullString Then
                            strNote = strNote & "Il campo Data Fine e' un campo obbligatorio."
                            swErr = True
                        Else
                            If IsDate(ArrCampi(5)) = False Or InStr(ArrCampi(5), "-") <> 0 Then
                                strNote = strNote & "Il campo Data Fine non e' nel formato corretto."
                                swErr = True
                            Else
                                If Len(Trim(ArrCampi(5))) = 8 Then
                                    ArrCampi(5) = Trim(ArrCampi(5))
                                    ArrCampi(5) = Left(ArrCampi(5), 6) & "20" & Right(ArrCampi(5), 2)
                                End If
                            End If
                        End If
                        If IsDate(ArrCampi(4)) = True And IsDate(ArrCampi(5)) = True Then
                            If VerificaDataFineCorso(Trim(ArrCampi(0)), Trim(ArrCampi(4)), Trim(ArrCampi(5))) = False Then
                                strNote = strNote & "Il campo Data Fine deve essere successivo alla data inizio e non superiore ai 180 gg successivi alla data di inizio progetto."
                                swErr = True
                            End If
                        Else
                            strNote = strNote & "Impossibile Controllare la data di fine corso o la validità dei 180 giorni in quanto il formato data risulta errato(Data Avvio Corso o Data Fine Corso)."
                            swErr = True
                        End If

                    End If

                    'Titolo
                    If Trim(ArrCampi(1)) = vbNullString Then
                        strNote = strNote & "Il campo Titolo e' un campo obbligatorio."
                        swErr = True
                    End If

                    'Bando
                    If Trim(ArrCampi(2)) = vbNullString Then
                        strNote = strNote & "Il campo Bando e' un campo obbligatorio."
                        swErr = True
                    End If

                    'NumeroVolontari
                    If Trim(ArrCampi(3)) = vbNullString Then
                        strNote = strNote & "Il campo NumeroVolontari e' un campo obbligatorio."
                        swErr = True
                    End If

                    'Luogo di svolgimento
                    If Trim(ArrCampi(6)) = vbNullString Then
                        strNote = strNote & "Il campo Luogo di svolgimento e' un campo obbligatorio."
                        swErr = True
                    End If
                    'Lunghezza Luogo di svolgimento
                    If Len(Trim(ArrCampi(6))) > 1000 Then
                        strNote = strNote & "Il campo Luogo di svolgimento puo' contenere massimo 1000 caratteri."
                        swErr = True
                    End If

                    'Riferimento
                    If Trim(ArrCampi(7)) = vbNullString Then
                        strNote = strNote & "Il campo Riferimento e' un campo obbligatorio."
                        swErr = True
                    End If
                    'Lunghezza Riferimento
                    If Len(Trim(ArrCampi(7))) > 100 Then
                        strNote = strNote & "Il campo Riferimento puo' contenere massimo 100 caratteri."
                        swErr = True
                    End If


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
        Response.Redirect("WfrmRisultatoImportProgettiFormazione.aspx?TipoFormazioneGenerale=" & UNICA_TRANCE & "&NomeFile=" & NomeUnivoco & "&Tot=" & Tot & "&TotOk=" & TotOk & "&TotKo=" & TotKo)

        'Response.Write("<script>" & vbCrLf)
        'Response.Write("parent.Naviga('WfrmRisultatoImportProgettiFormazione.aspx?TipoFormazioneGenerale=" & Request.QueryString("TipoFormazioneGenerale") & "&NomeFile=" & NomeUnivoco & "&Tot=" & Tot & "&TotOk=" & TotOk & "&TotKo=" & TotKo & "')" & vbCrLf)
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

        CreaArray = DefArr

    End Function

    Private Function VerificaStato(ByVal strCodProgetto As String) As Integer
        'verifico lo stato del progetto 
        Dim dtrStato As SqlClient.SqlDataReader
        Dim strSql As String
        Dim intStato As Integer

        strSql = " SELECT attività.IdStatoAttività FROM attività " & _
                 " INNER JOIN TipiProgetto on attività.idtipoprogetto = TipiProgetto.idtipoprogetto " & _
                 " WHERE (attività.CodiceEnte = '" & strCodProgetto & "' " & _
                 " and attività.identepresentante= " & Session("IdEnte") & ")" & _
                 " and TipiProgetto.Macrotipoprogetto like '" & Session("FiltroVisibilita") & "'"


        dtrStato = ClsServer.CreaDatareader(strSql, Session("conn"))

        dtrStato.Read()
        If dtrStato.HasRows Then
            intStato = dtrStato("IdStatoAttività")
        Else
            intStato = 0
        End If

        dtrStato.Close()
        dtrStato = Nothing

        Return intStato

    End Function

    Private Function VerificaPrevisioneCaricamento(ByVal strCodProgetto As String) As Boolean
        Dim dtrStato As SqlClient.SqlDataReader
        Dim strSql As String
        Dim bEsitoPrevisto As Boolean


        'strSql = "SELECT isnull(datediff(dd,DataInizioAttività,'" & dataavvio & "'),0) as DiffGG " & _
        '        " FROM attività WHERE CodiceEnte = '" & strCodProgetto & "' and IdStatoAttività=1"

        strSql = "SELECT bando.PianificazioneFormazione as PianificazioneFormazioneCorsi  FROM attività INNER JOIN BandiAttività ON attività.IDBandoAttività = BandiAttività.IdBandoAttività INNER JOIN bando ON BandiAttività.IdBando = bando.IDBando where CodiceEnte='" & strCodProgetto & "'"

        dtrStato = ClsServer.CreaDatareader(strSql, Session("conn"))

        dtrStato.Read()
        If dtrStato.HasRows Then
            If dtrStato("PianificazioneFormazioneCorsi") = True Then
                bEsitoPrevisto = True
            Else
                bEsitoPrevisto = False
            End If

        Else
            bEsitoPrevisto = False
        End If
        dtrStato.Close()
        dtrStato = Nothing

        Return bEsitoPrevisto

    End Function

    Private Function VerificaPartenzaProgetto(ByVal strCodProgetto As String) As Boolean
        Dim dtrStato As SqlClient.SqlDataReader
        Dim strSql As String
        Dim bEsitoData As Boolean
        Dim strDataProroga As String

        'controllo che la data inizio del progetto esista e che sia gia passatta (progetto effettivamente in corso)
        strSql = "SELECT idattività " & _
                " FROM attività WHERE CodiceEnte = '" & strCodProgetto & "' and IdStatoAttività=1 and isnull(datainizioattività,dateadd(day,1,getdate()))<= dbo.formatodatadt(getdate())" ' is not null"


        dtrStato = ClsServer.CreaDatareader(strSql, Session("conn"))

        dtrStato.Read()


        If dtrStato.HasRows Then
            bEsitoData = True
        Else
            bEsitoData = False
        End If
        dtrStato.Close()
        dtrStato = Nothing

        Return bEsitoData

    End Function

    Private Function VerificaDataAvvioCorso(ByVal strCodProgetto As String, ByVal dataavvio As Date) As Boolean
        Dim dtrStato As SqlClient.SqlDataReader
        Dim strSql As String
        Dim bEsitoData As Boolean
        'Dim DataAvvioCorso As Date
        'Dim Datedifferenza As Integer

        'strSql = "SELECT isnull(datediff(dd,DataInizioAttività,'" & dataavvio & "'),0) as DiffGG " & _
        '        " FROM attività WHERE CodiceEnte = '" & strCodProgetto & "' and IdStatoAttività=1"

        'controllo solo i progetti attivi e terminati
        ' strSql = " Select isnull(DataInizioAttività, dateadd(day,365,getdate())) as DataInizioAttività  from Attività where CodiceEnte='" & strCodProgetto & "' and IdStatoAttività in(1,2)"

        'modificato da s.c. il 05/10/2015
        'la data avvio Pianificazione UNICA TRANCHE a 180 gg  (datascadenzaunicatranche)

        strSql = " Select a.IDAttività "
        strSql &= " FROM Attività a "
        strSql &= " INNER JOIN AttivitàFormazioneGenerale afg on a.IDAttività=afg.IdAttività"
        strSql &= " WHERE a.CodiceEnte='" & strCodProgetto & "'  AND a.IdStatoAttività in(1,2) "
        strSql &= " AND  convert(datetime,'" & dataavvio & "') < afg.DataScadenzaUnicaTranche "
        strSql &= " AND  convert(datetime,'" & dataavvio & "') >= a.DataInizioAttività "

        dtrStato = ClsServer.CreaDatareader(strSql, Session("conn"))
        bEsitoData = dtrStato.HasRows



        'dtrStato.Read()

        'DataAvvioCorso = dataavvio
        ''Datedifferenza = DateDiff(DateInterval.DayOfYear, DataAvvioCorso, dtrStato("DataInizioAttività"))
        'Datedifferenza = DateDiff(DateInterval.DayOfYear, dtrStato("DataInizioAttività"), DataAvvioCorso)

        'If Datedifferenza <= 181 And Datedifferenza >= 0 Then
        '    bEsitoData = True
        'Else
        '    bEsitoData = False
        'End If

        dtrStato.Close()
        dtrStato = Nothing

        Return bEsitoData

    End Function

    Private Function VerificaDataFineCorso(ByVal strCodProgetto As String, ByVal DataInizio As Date, ByVal dataFine As Date) As Boolean
        Dim dtrStato As SqlClient.SqlDataReader
        Dim strSql As String
        Dim bEsitoData As Boolean
        'Dim DataInizioCorso As Date
        'Dim DataFineCorso As Date
        'Dim Datedifferenza As Integer
        'Dim DatedifferenzaInizioConFine As Integer
 


        'modificato da s.c. il 05/10/2015
        'la scadenza PIANificazione UNICA TRANCHE a 180 gg  (datascadenzaunicatranche)

        'strSql = " Select isnull(DataInizioAttività, dateadd(day,365,getdate())) as DataInizioAttività from Attività where CodiceEnte='" & strCodProgetto & "' and IdStatoAttività in(1,2)"

        strSql = " Select a.IDAttività "
        strSql &= " FROM Attività a "
        strSql &= " INNER JOIN AttivitàFormazioneGenerale afg on a.IDAttività=afg.IdAttività"
        strSql &= " WHERE a.CodiceEnte='" & strCodProgetto & "'  and a.IdStatoAttività in(1,2) "
        strSql &= " AND convert(datetime,'" & dataFine & "') > convert(datetime,'" & DataInizio & "') "
        strSql &= " AND convert(datetime,'" & dataFine & "') < afg.DataScadenzaUnicaTranche "

        dtrStato = ClsServer.CreaDatareader(strSql, Session("conn"))
        bEsitoData = dtrStato.HasRows

        'dtrStato.Read()

        'DataInizioCorso = DataInizio
        'DataFineCorso = dataFine

        'Datedifferenza = DateDiff(DateInterval.DayOfYear, dtrStato("DataInizioAttività"), DataFineCorso)
        'DatedifferenzaInizioConFine = DateDiff(DateInterval.DayOfYear, DataInizioCorso, DataFineCorso)

        'If Datedifferenza <= 181 And Datedifferenza >= 0 Then
        '    If DatedifferenzaInizioConFine > 0 Then
        '        bEsitoData = True
        '    End If
        'Else
        '    bEsitoData = False
        'End If

        dtrStato.Close()
        dtrStato = Nothing

        Return bEsitoData

    End Function

    Private Function VerificaScadenzaProgrammazioneCorsiProroga(ByVal strCodProgetto As String) As Boolean
        Dim dtrStato As SqlClient.SqlDataReader
        Dim strSql As String
        Dim bEsitoData As Boolean
        Dim strDataProroga As String


        'vedo se è stata impostata la data proroga
        strSql = "Select ISNULL(CONVERT(varchar, DataProroga, 103), 0) AS DataProroga " & _
                 " From AttivitàFormazioneGenerale INNER JOIN " & _
                 " attività ON AttivitàFormazioneGenerale.IdAttività = attività.IDAttività " & _
                 " Where CodiceEnte = '" & strCodProgetto & "'"
        dtrStato = ClsServer.CreaDatareader(strSql, Session("conn"))
        dtrStato.Read()

        If dtrStato.HasRows Then
            strDataProroga = dtrStato("DataProroga")
        End If
        dtrStato.Close()
        dtrStato = Nothing

        If strDataProroga <> "0" Then

            strSql = "SELECT isnull(datediff(dd,DataProroga,getdate()),-1) as DiffGG " & _
                        " From AttivitàFormazioneGenerale INNER JOIN " & _
                 " attività ON AttivitàFormazioneGenerale.IdAttività = attività.IDAttività " & _
                 " Where CodiceEnte = '" & strCodProgetto & "'"

            dtrStato = ClsServer.CreaDatareader(strSql, Session("conn"))
            dtrStato.Read()

            If dtrStato("DiffGG") > 7 Then
                bEsitoData = False              'proroga 7 giorni valida
            Else
                bEsitoData = True               'proroga scaduta
            End If
            dtrStato.Close()
            dtrStato = Nothing

            Return bEsitoData

            Exit Function

        End If

        ''controllo la data inizio solo per il progetto attivo, in questo modo escludo i progetti coprogettati che hanno lo stato di archiviato
        'strSql = "SELECT isnull(datediff(dd,DataInizioAttività,getdate()),0) as DiffGG " & _
        '        " FROM attività WHERE CodiceEnte = '" & strCodProgetto & "' and IdStatoAttività=1"


        'dtrStato = ClsServer.CreaDatareader(strSql, Session("conn"))

        'dtrStato.Read()


        'If dtrStato("DiffGG") <= 180 And dtrStato("DiffGG") >= 0 Then
        '    bEsitoData = True
        'Else
        '    bEsitoData = False
        'End If


        'modificato da s.c. il 05/10/2015
        ' controllo la DataScadenzaUnicaTranche (entro i 180 gg) viente utilizzata anche per le duetranche
        strSql = " Select a.IDAttività "
        strSql &= " FROM Attività a "
        strSql &= " INNER JOIN AttivitàFormazioneGenerale afg on a.IDAttività=afg.IdAttività"
        strSql &= " WHERE a.CodiceEnte='" & strCodProgetto & "'  and a.IdStatoAttività =1  "
        strSql &= " AND GETDATE() < afg.DataScadenzaUnicaTranche "

        dtrStato = ClsServer.CreaDatareader(strSql, Session("conn"))
        bEsitoData = dtrStato.HasRows

        dtrStato.Close()
        dtrStato = Nothing

        Return bEsitoData

    End Function

    Private Function VerificaScadenzaProgrammazioneCorsiProrogaTranche(ByVal strCodProgetto As String) As Boolean
        Dim dtrStato As SqlClient.SqlDataReader
        Dim strSql As String
        Dim bEsitoData As Boolean
        Dim strDataProroga As String


        'vedo se è stata impostata la data proroga
        strSql = "Select ISNULL(CONVERT(varchar, DataProroga, 103), 0) AS DataProroga " & _
                 " From AttivitàFormazioneGenerale INNER JOIN " & _
                 " attività ON AttivitàFormazioneGenerale.IdAttività = attività.IDAttività " & _
                 " Where CodiceEnte = '" & strCodProgetto & "'"
        dtrStato = ClsServer.CreaDatareader(strSql, Session("conn"))
        dtrStato.Read()

        If dtrStato.HasRows Then
            strDataProroga = dtrStato("DataProroga")
        End If
        dtrStato.Close()
        dtrStato = Nothing

        If strDataProroga <> "0" Then

            strSql = "SELECT isnull(datediff(dd,DataProroga,getdate()),-1) as DiffGG " & _
                        " From AttivitàFormazioneGenerale INNER JOIN " & _
                 " attività ON AttivitàFormazioneGenerale.IdAttività = attività.IDAttività " & _
                 " Where CodiceEnte = '" & strCodProgetto & "'"

            dtrStato = ClsServer.CreaDatareader(strSql, Session("conn"))
            dtrStato.Read()

            If dtrStato("DiffGG") > 7 Then
                bEsitoData = False              'proroga 7 giorni valida
            Else
                bEsitoData = True               'proroga scaduta
            End If
            dtrStato.Close()
            dtrStato = Nothing

            Return bEsitoData

            Exit Function

        End If

        'modificato da s.c. il 05/10/2015
        ' controllo la DataScadenzaPrimaTranche (entro i 180 gg) 
        strSql = " Select a.IDAttività "
        strSql &= " FROM Attività a "
        strSql &= " INNER JOIN AttivitàFormazioneGenerale afg on a.IDAttività=afg.IdAttività"
        strSql &= " WHERE a.CodiceEnte='" & strCodProgetto & "'  and a.IdStatoAttività =1  "
        strSql &= " AND GETDATE() < afg.DataScadenzaPrimaTranche "

        dtrStato = ClsServer.CreaDatareader(strSql, Session("conn"))
        bEsitoData = dtrStato.HasRows

        dtrStato.Close()
        dtrStato = Nothing

        Return bEsitoData

    End Function
    Private Sub ScriviTabTemp(ByVal pArray() As String)
        '--- scrive nella tab temporanea
        Dim cmdTemp As SqlClient.SqlCommand
        Dim strsql As String

        Try

            strsql = "INSERT INTO #TEMP_FORMAZIONE_VOLONATARIO " & _
                 "(CodiceProgetto  , " & _
                 "Titolo , " & _
                 "Bando , " & _
                 "NumeroVolontari , " & _
                 "DataAvvio , " & _
                 "DataFine , " & _
                 "SedeSvolgimento , " & _
                 "Riferimento " & _
                     ") " & _
                     "values " & _
                     "('" & Trim(ClsServer.NoApice(pArray(0))) & "', '" & _
                     Trim(ClsServer.NoApice(pArray(1))) & "', '" & _
                     Trim(ClsServer.NoApice(pArray(2))) & "', '" & _
                     Trim(ClsServer.NoApice(pArray(3))) & "', '" & _
                     Trim(ClsServer.NoApice(pArray(4))) & "', '" & _
                     Trim(ClsServer.NoApice(pArray(5))) & "', '" & _
                     Trim(ClsServer.NoApice(pArray(6))) & "', '" & _
                     Trim(ClsServer.NoApice(pArray(7))) & "' " & _
                     ")"

            cmdTemp = New SqlClient.SqlCommand
            cmdTemp.CommandText = strsql
            cmdTemp.Connection = Session("conn")
            cmdTemp.ExecuteNonQuery()
            cmdTemp.Dispose()
            TotOk = TotOk + 1

        Catch exc As Exception
            Response.Write(exc.Message)
            strNote = ("Response.Write(exc.Message)")
            strNote = "Errore generico."
            TotKo = TotKo + 1

        End Try
    End Sub

   Private Sub LeggiCSVTranche()
        '--- lettura del file
        Dim Reader As StreamReader
        Dim xLinea As String
        Dim ArrCampi() As String
        Dim swErr As Boolean
        Dim AppoNote As String


        '--- Leggo il file di input e scrivo quello di output
        Reader = New StreamReader(Server.MapPath("upload") & "\" & NomeUnivoco & ".CSV", System.Text.Encoding.UTF7, False)

        Writer = New StreamWriter(Server.MapPath("download") & "\" & NomeUnivoco & ".CSV")

        '--- intestazione

        xLinea = Reader.ReadLine()
        Writer.WriteLine("Note;" & xLinea)

        '--- scorro le righe
        xLinea = Reader.ReadLine()
        While (xLinea <> "")
            swErr = False
            Tot = Tot + 1
            ArrCampi = CreaArray(xLinea)

            If UBound(ArrCampi) < 11 Then
                '--- se i campi non sono tutti errore
                strNote = "Il numero delle colonne inserite è minore di quello richieste."
                swErr = True
                TotKo = TotKo + 1
            Else
                If UBound(ArrCampi) > 11 Then
                    '--- se i campi sono troppi errore
                    strNote = "Il numero delle colonne inserite è maggiore di quello richieste."
                    swErr = True
                    TotKo = TotKo + 1

                Else
                    'CodiceProgetto
                    If Trim(ArrCampi(0)) = vbNullString Then
                        strNote = strNote & "Il campo CodiceProgetto e' un campo obbligatorio."
                        swErr = True
                    End If
                    'Stato Progetto Attivo
                    If Trim(ArrCampi(0)) <> "" Then
                        If VerificaStato(Trim(ArrCampi(0))) = 0 Then
                            strNote = strNote & "Il progetto non risulta 'Attivo' per l'ente."
                            swErr = True
                        End If
                    End If
                    'Verifica Progetto Dati di Pianificazione
                    If VerificaPrevisioneCaricamento(Trim(ArrCampi(0))) = False Then
                        strNote = strNote & "Per il progetto indicato non è previsto il caricamento dei dati di pianificazione."
                        swErr = True
                    End If

                    'verifica partenza progetto
                    If VerificaPartenzaProgetto(Trim(ArrCampi(0))) = False Then
                        strNote = strNote & "Il progetto in questione non risulta ancora avviato."
                        swErr = True
                    Else
                        'verifica termini progetto
                        If VerificaScadenzaProgrammazioneCorsiProrogaTranche(Trim(ArrCampi(0))) = False Then
                            strNote = strNote & "Per il progetto in questione sono scaduti i termini per il caricamento."
                            swErr = True
                        End If
                        'Data Avvio prima tranche
                        If Trim(ArrCampi(4)) = vbNullString Then
                            strNote = strNote & "Il campo Data Avvio Prima Tranche e' un campo obbligatorio."
                            swErr = True
                        Else
                            If IsDate(ArrCampi(4)) = False Or InStr(ArrCampi(4), "-") <> 0 Then
                                strNote = strNote & "Il campo Data Avvio Prima Tranche non e' nel formato corretto."
                                swErr = True
                            Else
                                If Len(Trim(ArrCampi(4))) = 8 Then
                                    ArrCampi(4) = Trim(ArrCampi(4))
                                    ArrCampi(4) = Left(ArrCampi(4), 6) & "20" & Right(ArrCampi(4), 2)
                                End If
                            End If
                        End If
                        If IsDate(ArrCampi(4)) = True Then
                            If VerificaDataAvvioCorsoPrimaTranche(Trim(ArrCampi(0)), Trim(ArrCampi(4))) = False Then
                                strNote = strNote & "Il campo Data Avvio Prima Tranche non e' compreso nell'intervallo corretto."
                                swErr = True
                            End If
                        Else
                            strNote = strNote & "Impossibile controllare l'intervallo di tempo in quanto il formato della data di avvio non e' corretto."
                            swErr = True
                        End If

                        'Data Fine
                        If Trim(ArrCampi(5)) = vbNullString Then
                            strNote = strNote & "Il campo Data Fine Prima Tranche e' un campo obbligatorio."
                            swErr = True
                        Else
                            If IsDate(ArrCampi(5)) = False Or InStr(ArrCampi(5), "-") <> 0 Then
                                strNote = strNote & "Il campo Data Fine Prima Tranche non e' nel formato corretto."
                                swErr = True
                            Else
                                If Len(Trim(ArrCampi(5))) = 8 Then
                                    ArrCampi(5) = Trim(ArrCampi(5))
                                    ArrCampi(5) = Left(ArrCampi(5), 6) & "20" & Right(ArrCampi(5), 2)
                                End If
                            End If
                        End If
                        If IsDate(ArrCampi(4)) = True And IsDate(ArrCampi(5)) = True Then
                            If VerificaDataFineCorsoPrimaTranche(Trim(ArrCampi(0)), Trim(ArrCampi(4)), Trim(ArrCampi(5))) = False Then
                                strNote = strNote & "Il campo Data Fine Prima Tranche deve essere successivo alla data inizio e non superiore ai 180 gg successivi alla data di inizio progetto."
                                swErr = True
                            End If
                        Else
                            strNote = strNote & "Impossibile Controllare la data di fine corso o la validità dei 180 giorni in quanto il formato data risulta errato(Data Avvio Corso Prima Tranche o Data Fine Corso Prima Tranche)."
                            swErr = True
                        End If
                        ''Verifico se la data iniizo seconda tranche è uguale alla data fine prima tranche
                        'If Trim(IsDate(ArrCampi(8))) <= Trim(IsDate(ArrCampi(5))) Then
                        '    strNote = strNote & "Il campo Data Avvio Seconda Tranche deve essere maggiore della Data Fine Corso Prima Tranche."
                        '    swErr = True
                        'End If

                        'AVVIO SECONDA TRANCHE
                        'Data Avvio SECONDA TRANCHE
                        If Trim(ArrCampi(8)) = vbNullString Then
                            strNote = strNote & "Il campo Data Avvio Seconda Tranche e' un campo obbligatorio."
                            swErr = True
                        Else
                            If IsDate(ArrCampi(8)) = False Or InStr(ArrCampi(8), "-") <> 0 Then
                                strNote = strNote & "Il campo Data Avvio Seconda Tranche non e' nel formato corretto."
                                swErr = True
                            Else
                                If Len(Trim(ArrCampi(8))) = 8 Then
                                    ArrCampi(8) = Trim(ArrCampi(8))
                                    ArrCampi(8) = Left(ArrCampi(8), 6) & "20" & Right(ArrCampi(8), 2)
                                End If
                            End If
                        End If
                        If IsDate(ArrCampi(8)) = True Then
                            If VerificaDataAvvioCorsoSecondaTranche(Trim(ArrCampi(0)), Trim(ArrCampi(8))) = False Then
                                strNote = strNote & "Il campo Data Avvio Seconda Tranche non e' compreso nell'intervallo previsto."
                                swErr = True
                            End If
                        Else
                            strNote = strNote & "Impossibile controllare l'intervallo di tempo in quanto il formato della data di avvio seconda tranche non e' corretto."
                            swErr = True
                        End If

                        'Data Fine SECONDA TRANCHE
                        If Trim(ArrCampi(9)) = vbNullString Then
                            strNote = strNote & "Il campo Data Fine Seconda Tranche e' un campo obbligatorio."
                            swErr = True
                        Else
                            If IsDate(ArrCampi(9)) = False Or InStr(ArrCampi(9), "-") <> 0 Then
                                strNote = strNote & "Il campo Data Fine Seconda Tranche non e' nel formato previsto."
                                swErr = True
                            Else
                                If Len(Trim(ArrCampi(9))) = 8 Then
                                    ArrCampi(9) = Trim(ArrCampi(9))
                                    ArrCampi(9) = Left(ArrCampi(9), 6) & "20" & Right(ArrCampi(9), 2)
                                End If
                            End If
                        End If
                        If IsDate(ArrCampi(8)) = True And IsDate(ArrCampi(9)) = True Then
                            If VerificaDataFineCorsoSecondaTranche(Trim(ArrCampi(0)), Trim(ArrCampi(8)), Trim(ArrCampi(9))) = False Then
                                strNote = strNote & "Il campo Data Fine Seconda Tranche non è compreso nell'intervallo previsto."
                                swErr = True
                            End If
                        Else
                            strNote = strNote & "Impossibile Controllare la data di fine corso o la validità in quanto il formato data risulta errato(Data Avvio Corso Seconda Tranche o Data Fine Corso Seconda Tranche)."
                            swErr = True
                        End If
                    End If

                    'Titolo
                    If Trim(ArrCampi(1)) = vbNullString Then
                        strNote = strNote & "Il campo Titolo e' un campo obbligatorio."
                        swErr = True
                    End If

                    'Bando
                    If Trim(ArrCampi(2)) = vbNullString Then
                        strNote = strNote & "Il campo Bando e' un campo obbligatorio."
                        swErr = True
                    End If

                    'NumeroVolontari
                    If Trim(ArrCampi(3)) = vbNullString Then
                        strNote = strNote & "Il campo NumeroVolontari e' un campo obbligatorio."
                        swErr = True
                    End If


                    'Luogo di svolgimento
                    If Trim(ArrCampi(6)) = vbNullString Then
                        strNote = strNote & "Il campo Luogo di svolgimento Prima Tranche e' un campo obbligatorio."
                        swErr = True
                    End If
                    'Lunghezza Luogo di svolgimento
                    If Len(Trim(ArrCampi(6))) > 1000 Then
                        strNote = strNote & "Il campo Luogo di svolgimento Prima Tranche puo' contenere massimo 1000 caratteri."
                        swErr = True
                    End If

                    'Riferimento
                    If Trim(ArrCampi(7)) = vbNullString Then
                        strNote = strNote & "Il campo Riferimento Prima Tranche e' un campo obbligatorio."
                        swErr = True
                    End If
                    'Lunghezza Riferimento
                    If Len(Trim(ArrCampi(7))) > 100 Then
                        strNote = strNote & "Il campo Riferimento Prima Tranche puo' contenere massimo 100 caratteri."
                        swErr = True
                    End If

                    'seconda tranche
                    'Luogo di svolgimento
                    If Trim(ArrCampi(10)) = vbNullString Then
                        strNote = strNote & "Il campo Luogo di svolgimento Seconda Tranche e' un campo obbligatorio."
                        swErr = True
                    End If
                    'Lunghezza Luogo di svolgimento
                    If Len(Trim(ArrCampi(10))) > 1000 Then
                        strNote = strNote & "Il campo Luogo di svolgimento Seconda Tranche puo' contenere massimo 1000 caratteri."
                        swErr = True
                    End If

                    'Riferimento
                    If Trim(ArrCampi(11)) = vbNullString Then
                        strNote = strNote & "Il campo Riferimento Seconda Tranche e' un campo obbligatorio."
                        swErr = True
                    End If
                    'Lunghezza Riferimento
                    If Len(Trim(ArrCampi(11))) > 100 Then
                        strNote = strNote & "Il campo Riferimento Seconda Tranche puo' contenere massimo 100 caratteri."
                        swErr = True
                    End If
                    If swErr = False Then
                        ScriviTabTempTranche(ArrCampi)
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

        ''--- reindirizzo la pagina sottostante
        Response.Redirect("WfrmRisultatoImportProgettiFormazione.aspx?TipoFormazioneGenerale=" & DOPPIA_TRANCE & "&NomeFile=" & NomeUnivoco & "&Tot=" & Tot & "&TotOk=" & TotOk & "&TotKo=" & TotKo)
        'Response.Write("<script>" & vbCrLf)
        'Response.Write("parent.Naviga('WfrmRisultatoImportProgettiFormazione.aspx?TipoFormazioneGenerale=" & Request.QueryString("TipoFormazioneGenerale") & "&NomeFile=" & NomeUnivoco & "&Tot=" & Tot & "&TotOk=" & TotOk & "&TotKo=" & TotKo & "')" & vbCrLf)
        'Response.Write("</script>")

    End Sub

     Private Function VerificaDataAvvioCorsoPrimaTranche(ByVal strCodProgetto As String, ByVal dataavvio As Date) As Boolean
        'controllo data avvio corsi a tranche
        'PRIMA TRANCHE   80% - entro il 180° gg dalla Data Inizio Progetto
        'SECONDA TRANCHE 20% - tra il 210° al 270° gg dalla Data Inizio Progetto
        Dim dtrStato As SqlClient.SqlDataReader
        Dim strSql As String
        Dim bEsitoData As Boolean
        Dim DataAvvioCorso As Date
        Dim Datedifferenza As Integer

        'strSql = "SELECT isnull(datediff(dd,DataInizioAttività,'" & dataavvio & "'),0) as DiffGG " & _
        '        " FROM attività WHERE CodiceEnte = '" & strCodProgetto & "' and IdStatoAttività=1"

        'controllo solo i progetti attivi e terminati
        'strSql = " Select isnull(DataInizioAttività, dateadd(day,365,getdate())) as DataInizioAttività  from Attività where CodiceEnte='" & strCodProgetto & "' and IdStatoAttività in(1,2)"

        'dtrStato = ClsServer.CreaDatareader(strSql, Session("conn"))

        'dtrStato.Read()

        'DataAvvioCorso = dataavvio
        ''Datedifferenza = DateDiff(DateInterval.DayOfYear, DataAvvioCorso, dtrStato("DataInizioAttività"))
        'Datedifferenza = DateDiff(DateInterval.DayOfYear, dtrStato("DataInizioAttività"), DataAvvioCorso)

        'If Datedifferenza <= 181 And Datedifferenza >= 0 Then
        '    bEsitoData = True
        'Else
        '    bEsitoData = False
        'End If


        'modificato il 05/10/2015 da simona cordella
        'il controllo viene fatto utilizzando la datascadencaprimatranche  


        strSql = " Select a.IDAttività "
        strSql &= " FROM Attività a "
        strSql &= " INNER JOIN AttivitàFormazioneGenerale afg on a.IDAttività=afg.IdAttività"
        strSql &= " WHERE a.CodiceEnte='" & strCodProgetto & "'  AND a.IdStatoAttività in(1,2) "
        strSql &= "  AND convert(datetime,'" & dataavvio & "') < afg.DataScadenzaPrimaTranche "
        strSql &= "  AND convert(datetime,'" & dataavvio & "') >= a.DataInizioAttività "

        dtrStato = ClsServer.CreaDatareader(strSql, Session("conn"))
        bEsitoData = dtrStato.HasRows


        dtrStato.Close()
        dtrStato = Nothing

        Return bEsitoData

    End Function
    Private Function VerificaDataAvvioCorsoSecondaTranche(ByVal strCodProgetto As String, ByVal dataavvioST As Date) As Boolean
        'controllo data avvio corsi a tranche
        'PRIMA TRANCHE   80% - entro il 180° gg dalla Data Inizio Progetto
        'SECONDA TRANCHE 20% - tra il 210° al 270° gg dalla Data Inizio Progetto
        Dim dtrStato As SqlClient.SqlDataReader
        Dim strSql As String
        Dim bEsitoData As Boolean
        'Dim DataAvvioCorso As Date
        'Dim Datedifferenza As Integer

        'strSql = "SELECT isnull(datediff(dd,DataInizioAttività,'" & dataavvio & "'),0) as DiffGG " & _
        '        " FROM attività WHERE CodiceEnte = '" & strCodProgetto & "' and IdStatoAttività=1"
        '" isnull(DataInizioAttività, dateadd(day,365,getdate())) as DataInizioAttività " & _

        'controllo solo i progetti attivi e terminati
        'strSql = " Select  " & _
        '         " dateadd(day,210,DataInizioAttività) as DataPrimoIntervallo," & _
        '         " dateadd(day,270,DataInizioAttività) as DataSecondoIntervallo  " & _
        '         " from Attività where CodiceEnte='" & strCodProgetto & "' and IdStatoAttività in(1,2)"

        'dtrStato = ClsServer.CreaDatareader(strSql, Session("conn"))

        'dtrStato.Read()

        'DataAvvioCorso = dataavvioST
        ''data avvST  25/04/2015
        ''iterve 25/04/2015 al 25/05/2015
        'If (DataAvvioCorso >= dtrStato("DataPrimoIntervallo") And DataAvvioCorso <= dtrStato("DataSecondoIntervallo")) Then
        '    bEsitoData = True
        'Else
        '    bEsitoData = False
        'End If

        'Datedifferenza = DateDiff(DateInterval.DayOfYear, dtrStato("DataInizioAttività"), DataAvvioCorso)

        'If Datedifferenza <= 181 And Datedifferenza >= 0 Then
        '    bEsitoData = True
        'Else
        '    bEsitoData = False
        'End If



        'modificato il 05/10/2015 da simona cordella
        'il controllo viene fatto utilizzando la datascadencaprimatranche  


        strSql = " Select a.IDAttività "
        strSql &= " FROM Attività a "
        strSql &= " INNER JOIN AttivitàFormazioneGenerale afg on a.IDAttività=afg.IdAttività"
        strSql &= " WHERE a.CodiceEnte='" & strCodProgetto & "'  AND a.IdStatoAttività in(1,2) "
        strSql &= " AND convert(datetime,'" & dataavvioST & "')>= dateadd(day,210,DataInizioAttività) "
        strSql &= " AND convert(datetime,'" & dataavvioST & "')< DataScadenzaSecondaTranche "

        dtrStato = ClsServer.CreaDatareader(strSql, Session("conn"))
        bEsitoData = dtrStato.HasRows

        dtrStato.Close()
        dtrStato = Nothing

        Return bEsitoData

    End Function

    Private Function VerificoCaricamentoPianificazione(ByVal strCodProgetto As String, ByVal dataavvioPT As Date, ByVal dataFinePT As Date) As Boolean
        'agg il 09/04/2015 da simona cordella
        'verifica se è stata gia importita una pianificazione.
        'controllo che le date prima tranche non siano state modificate

        Dim dtrStato As SqlClient.SqlDataReader
        Dim strSql As String
        Dim bEsitoData As Boolean

        strSql = " Select isnull(DataInizioCorso,0) as DataInizioCorso ," & _
                 " isnull(DatafineCorso,0) as DatafineCorso " & _
                 " From AttivitàFormazioneGenerale " & _
                 " INNER JOIN  attività ON AttivitàFormazioneGenerale.IdAttività = attività.IDAttività  " & _
                 " WHERE CodiceEnte='" & strCodProgetto & "'  AND NOT DataInizioCorso IS NULL AND NOT DatafineCorso IS NULL "

        dtrStato = ClsServer.CreaDatareader(strSql, Session("conn"))

        If dtrStato.HasRows = True Then
            dtrStato.Read()
            If (dataavvioPT <> dtrStato("DataInizioCorso") Or dataFinePT <> dtrStato("DatafineCorso")) Then
                bEsitoData = False
            Else
                bEsitoData = True
            End If
        Else
            bEsitoData = True
        End If

        dtrStato.Close()
        dtrStato = Nothing

        Return bEsitoData

    End Function

      Private Function VerificaDataFineCorsoPrimaTranche(ByVal strCodProgetto As String, ByVal DataInizio As Date, ByVal dataFine As Date) As Boolean
        Dim dtrStato As SqlClient.SqlDataReader
        Dim strSql As String
        Dim bEsitoData As Boolean
        'Dim DataInizioCorso As Date
        'Dim DataFineCorso As Date
        'Dim Datedifferenza As Integer
        'Dim DatedifferenzaInizioConFine As Integer


        'strSql = " Select isnull(DataInizioAttività, dateadd(day,365,getdate())) as DataInizioAttività from Attività where CodiceEnte='" & strCodProgetto & "' and IdStatoAttività in(1,2)"

        'dtrStato = ClsServer.CreaDatareader(strSql, Session("conn"))

        'dtrStato.Read()
        'DataInizioCorso = DataInizio
        'DataFineCorso = dataFine

        'Datedifferenza = DateDiff(DateInterval.DayOfYear, dtrStato("DataInizioAttività"), DataFineCorso)
        'DatedifferenzaInizioConFine = DateDiff(DateInterval.DayOfYear, DataInizioCorso, DataFineCorso)

        'If Datedifferenza <= 180 And Datedifferenza >= 0 Then
        '    If DatedifferenzaInizioConFine > 0 Then
        '        bEsitoData = True
        '    End If
        'Else
        '    bEsitoData = False
        'End If
        '05/10/2015 da s.c.
        strSql = " Select a.IDAttività "
        strSql &= " FROM Attività a "
        strSql &= " INNER JOIN AttivitàFormazioneGenerale afg on a.IDAttività=afg.IdAttività"
        strSql &= " WHERE a.CodiceEnte='" & strCodProgetto & "'  and a.IdStatoAttività in(1,2)  "
        strSql &= " AND convert(datetime,'" & dataFine & "') < afg.DataScadenzaPrimaTranche "
        strSql &= " AND convert(datetime,'" & dataFine & "') > convert(datetime,'" & DataInizio & "') "

        dtrStato = ClsServer.CreaDatareader(strSql, Session("conn"))
        bEsitoData = dtrStato.HasRows

        dtrStato.Close()
        dtrStato = Nothing

        Return bEsitoData

    End Function
    Private Function VerificaDataFineCorsoSecondaTranche(ByVal strCodProgetto As String, ByVal DataInizio As Date, ByVal dataFine As Date) As Boolean
        Dim dtrStato As SqlClient.SqlDataReader
        Dim strSql As String
        Dim bEsitoData As Boolean
        'Dim DataInizioCorso As Date
        'Dim DataFineCorso As Date



        ''strSql = " Select isnull(DataInizioAttività, dateadd(day,365,getdate())) as DataInizioAttività from Attività where CodiceEnte='" & strCodProgetto & "' and IdStatoAttività in(1,2)"

        'strSql = " Select  " & _
        '         " dateadd(day,210,DataInizioAttività) as DataPrimoIntervallo," & _
        '         " dateadd(day,270,DataInizioAttività) as DataSecondoIntervallo  " & _
        '         " from Attività where CodiceEnte='" & strCodProgetto & "' and IdStatoAttività in(1,2)"
        'dtrStato = ClsServer.CreaDatareader(strSql, Session("conn"))

        'dtrStato.Read()
        'DataInizioCorso = DataInizio
        'DataFineCorso = dataFine

        'If (dataFine >= dtrStato("DataPrimoIntervallo") And dataFine <= dtrStato("DataSecondoIntervallo")) Then
        '    bEsitoData = True
        'Else
        '    bEsitoData = False
        'End If

        'MOD. IL 05/10/2015  da s.c.
        strSql = " Select a.IDAttività "
        strSql &= " FROM Attività a "
        strSql &= " INNER JOIN AttivitàFormazioneGenerale afg on a.IDAttività=afg.IdAttività"
        strSql &= " WHERE a.CodiceEnte='" & strCodProgetto & "'  and a.IdStatoAttività in(1,2)  "
        strSql &= " AND convert(datetime,'" & dataFine & "') < afg.DataScadenzaSecondaTranche "
        strSql &= " AND convert(datetime,'" & dataFine & "') > convert(datetime,'" & DataInizio & "')"

        dtrStato = ClsServer.CreaDatareader(strSql, Session("conn"))
        bEsitoData = dtrStato.HasRows

        dtrStato.Close()
        dtrStato = Nothing

        Return bEsitoData

    End Function

    Private Sub ScriviTabTempTranche(ByVal pArray() As String)
        '--- scrive nella tab temporanea
        Dim cmdTemp As SqlClient.SqlCommand
        Dim strsql As String

        Try

            strsql = "INSERT INTO #TEMP_FORMAZIONE_VOLONATARIO " & _
                 "(CodiceProgetto  , " & _
                 "Titolo , " & _
                 "Bando , " & _
                 "NumeroVolontari , " & _
                 "DataAvvio , " & _
                 "DataFine , " & _
                 "SedeSvolgimento , " & _
                 "Riferimento, " & _
                 "DataAvvioTranche, " & _
                 "DataFineTranche, " & _
                 "SedeSvolgimentoTranche, " & _
                 "RiferimentoTranche " & _
                     ") " & _
                     "values " & _
                     "('" & Trim(ClsServer.NoApice(pArray(0))) & "', '" & _
                     Trim(ClsServer.NoApice(pArray(1))) & "', '" & _
                     Trim(ClsServer.NoApice(pArray(2))) & "', '" & _
                     Trim(ClsServer.NoApice(pArray(3))) & "', '" & _
                     Trim(ClsServer.NoApice(pArray(4))) & "', '" & _
                     Trim(ClsServer.NoApice(pArray(5))) & "', '" & _
                     Trim(ClsServer.NoApice(pArray(6))) & "', '" & _
                     Trim(ClsServer.NoApice(pArray(7))) & "', '" & _
                     Trim(ClsServer.NoApice(pArray(8))) & "', '" & _
                     Trim(ClsServer.NoApice(pArray(9))) & "', '" & _
                     Trim(ClsServer.NoApice(pArray(10))) & "', '" & _
                     Trim(ClsServer.NoApice(pArray(11))) & "' " & _
                     ")"

            cmdTemp = New SqlClient.SqlCommand
            cmdTemp.CommandText = strsql
            cmdTemp.Connection = Session("conn")
            cmdTemp.ExecuteNonQuery()
            cmdTemp.Dispose()
            TotOk = TotOk + 1

        Catch exc As Exception
            Response.Write(exc.Message)
            strNote = ("Response.Write(exc.Message)")
            strNote = "Errore generico."
            TotKo = TotKo + 1

        End Try
    End Sub

End Class