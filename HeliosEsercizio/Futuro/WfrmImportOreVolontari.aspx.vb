Imports System.IO

Public Class WfrmImportOreVolontari
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
                NomeUnivoco = "orevolontari" & Session("IdEnte") & "_" & Session("Utente") & "_" & Year(Now) & Month(Now) & Day(Now) & Hour(Now) & Minute(Now) & Second(Now)




                Dim file As String
                Dim estensione As String
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
        strSql = "CREATE TABLE [#TEMP_ORE_VOLONATARIO] (" & _
                 "[CodiceVolontario] [nvarchar] (15), " & _
                 "[Cognome] [nvarchar] (100), " & _
                 "[Nome] [nvarchar] (100), " & _
                 "[CodiceFiscale] [nvarchar] (16), " & _
                 "[CodiceProgetto] [nvarchar] (22), " & _
                 "[OreFormazione] [int] " & _
                 ")"
        cmdCreateTempTable = New SqlClient.SqlCommand
        cmdCreateTempTable.CommandText = strSql
        cmdCreateTempTable.Connection = Session("conn")
        cmdCreateTempTable.ExecuteNonQuery()
        cmdCreateTempTable.Dispose()
    End Sub

    Private Sub CreaTabTempTranche()
        Dim strSql As String
        Dim cmdCreateTempTable As SqlClient.SqlCommand

        CancellaTabellaTemp()

        '--- CREO TAB TEMPORANEA
        strSql = "CREATE TABLE [#TEMP_ORE_VOLONATARIO] (" & _
            "[CodiceVolontario] [nvarchar] (15), " & _
            "[Cognome] [nvarchar] (100), " & _
            "[Nome] [nvarchar] (100), " & _
            "[CodiceFiscale] [nvarchar] (16), " & _
            "[CodiceProgetto] [nvarchar] (22), " & _
            "[OreFormazionePrimaTranche] [int], " & _
            "[OreFormazioneSecondaTranche] [int] " & _
            ")"
        cmdCreateTempTable = New SqlClient.SqlCommand
        cmdCreateTempTable.CommandText = strSql
        cmdCreateTempTable.Connection = Session("conn")
        cmdCreateTempTable.ExecuteNonQuery()
        cmdCreateTempTable.Dispose()
    End Sub

    Private Sub CancellaTabellaTemp()
        Dim strSql As String
        Dim cmdCanTempTable As SqlClient.SqlCommand
        Try
            '--- CANCELLO TAB TEMPORANEA
            strSql = "DROP TABLE [#TEMP_ORE_VOLONATARIO]"

            cmdCanTempTable = New SqlClient.SqlCommand
            cmdCanTempTable.CommandText = strSql
            cmdCanTempTable.Connection = Session("conn")
            cmdCanTempTable.ExecuteNonQuery()
        Catch e As Exception
        End Try

        cmdCanTempTable.Dispose()
    End Sub

    Private Sub LeggiCSV()
        '--- lettura del file
        Dim Reader As StreamReader
        Dim xLinea As String
        Dim ArrCampi() As String
        Dim swErr As Boolean
        Dim AppoNote As String
        Dim intOreForm As Integer

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

            If UBound(ArrCampi) < 15 Then
                '--- se i campi non sono tutti errore
                strNote = "Il numero delle colonne inserite è minore di quello richieste."
                swErr = True
                TotKo = TotKo + 1
            Else
                If UBound(ArrCampi) > 15 Then
                    '--- se i campi sono troppi errore
                    strNote = "Il numero delle colonne inserite è maggiore di quello richieste."
                    swErr = True
                    TotKo = TotKo + 1

                Else
                    'CodiceVolontario
                    If Trim(ArrCampi(0)) = vbNullString Then
                        strNote = strNote & "Il campo CodiceVolontario e' un campo obbligatorio."
                        swErr = True
                    End If

                    'Cognome
                    If Trim(ArrCampi(1)) = vbNullString Then
                        strNote = strNote & "Il campo Cognome e' un campo obbligatorio."
                        swErr = True
                    End If

                    'Nome
                    If Trim(ArrCampi(2)) = vbNullString Then
                        strNote = strNote & "Il campo Nome e' un campo obbligatorio."
                        swErr = True
                    End If

                    'CodiceFiscale
                    If Trim(ArrCampi(4)) = vbNullString Then
                        strNote = strNote & "Il campo CodiceFiscale e' un campo obbligatorio."
                        swErr = True
                    End If

                    'CodiceProgetto
                    If Trim(ArrCampi(8)) = vbNullString Then
                        strNote = strNote & "Il campo CodiceProgetto e' un campo obbligatorio."
                        swErr = True
                    Else
                        'verificacompatibilità agg. il 02/12/2014 da s.c.
                        If VerificaCompatibilita(ArrCampi(8)) = False Then
                            strNote = strNote & "Progetto non esistente."
                            swErr = True
                        End If
                        'Statoprogetto
                        If VerificaStato(ArrCampi(8)) <> 0 Then
                            strNote = strNote & "Il progetto è stato 'Confermato'. Impossibile procedere con l'importazione."
                            swErr = True
                        End If

                        If VerificaBandoProgetto(ArrCampi(8)) = False Then
                            strNote = strNote & "Il progetto è associato ad un bando per il quale non è ancora possibile caricare ore di formazione. "
                            swErr = True
                        End If

                        If VerificaScadenzaCaricamentoORE(ArrCampi(8)) = False Then
                            strNote = strNote & "È stato superato il tempo massimo di 180 gg dalla data inizio progetto. Impossibile procedere con l'importazione."
                            swErr = True
                        End If
                        'corsi di formazione
                        If VerificaCaricamentoCorsiFormazioneADC(ArrCampi(8)) = False Then
                            strNote = strNote & "Il progetto prevede che ci siano caricati i Corsi Formazione. "
                            swErr = True
                        Else
                            'verifca data pianificazione corsi
                            If VerificaDataPianificazioneCorsi(ArrCampi(8)) = False Then
                                strNote = strNote & "Il Corso di Formazione non risulta ancora terminato secondo quanto indicato in fase di pianificazione corso. "
                                swErr = True
                            End If
                        End If

                    End If


                    'OreFormazione

                    If Trim(ArrCampi(15)) = vbNullString Then
                        strNote = strNote & "Il campo OreFormazione e' un campo obbligatorio."
                        swErr = True
                        intOreForm = 9999
                    Else
                        If IsNumeric(ArrCampi(15)) = False Then
                            strNote = strNote & "Il campo OreFormazione non e' nel formato corretto."
                            swErr = True
                            intOreForm = 9999
                        ElseIf InStr(ArrCampi(15), ".") <> 0 Or InStr(ArrCampi(15), "-") <> 0 Or InStr(ArrCampi(15), ",") <> 0 Then
                            strNote = strNote & "Il campo OreFormazione deve essere un numero intero."
                            swErr = True
                            intOreForm = 9999
                        Else
                            intOreForm = ArrCampi(15)
                        End If
                    End If

                    'verifico se il volontario e' in possesso del codice IBAN
                    If VerificaIbanVolontario(ArrCampi(0), intOreForm, ArrCampi(8)) = False Then
                        strNote = strNote & "Il Volontario non è in possesso del codice IBAN pertanto non è possibile indicare le ore di formazione."
                        swErr = True
                    End If


                    'verifico l'esistenza in banca dati                    
                    If VerificaEntita(Trim(ArrCampi(0)), Trim(ArrCampi(1)), Trim(ArrCampi(2)), Trim(ArrCampi(4)), Trim(ArrCampi(8))) = False Then
                        strNote = strNote & "Volontario non trovato o per il quale non è previsto il caricamento delle ore di formazione."
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
        Response.Redirect("WfrmRisultatoImportOreVolontari.aspx?TipoFormazioneGenerale=" & UNICA_TRANCE & "&NomeFile=" & NomeUnivoco & "&Tot=" & Tot & "&TotOk=" & TotOk & "&TotKo=" & TotKo)

        'Response.Write("<script>" & vbCrLf)
        'Response.Write("parent.Naviga('WfrmRisultatoImportOreVolontari.aspx?NomeFile=" & NomeUnivoco & "&Tot=" & Tot & "&TotOk=" & TotOk & "&TotKo=" & TotKo & "')" & vbCrLf)
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

    Private Function VerificaCompatibilita(ByVal strCodProgetto As String) As Boolean
        'creata da simona cordella il 02/12/2014
        Dim dtrStato As SqlClient.SqlDataReader
        Dim strSql As String
        Dim blnCompatibilita As Boolean

        strSql = "SELECT idattività " _
               & " FROM attività INNER JOIN " _
               & " TipiProgetto ON attività.IDtipoProgetto = TipiProgetto.IDtipoProgetto " _
               & " WHERE  (attività.CodiceEnte = '" & strCodProgetto & "') " _
               & " AND TipiProgetto.Macrotipoprogetto like '" & Session("FiltroVisibilita") & "'"

        dtrStato = ClsServer.CreaDatareader(strSql, Session("conn"))

        dtrStato.Read()
        If dtrStato.HasRows Then
            blnCompatibilita = True
        Else
            blnCompatibilita = False
        End If

        dtrStato.Close()
        dtrStato = Nothing

        Return blnCompatibilita

    End Function

    Private Function VerificaStato(ByVal strCodProgetto As String) As Integer

        Dim dtrStato As SqlClient.SqlDataReader
        Dim strSql As String
        Dim intStato As Integer

        strSql = "SELECT ISNULL(AttivitàFormazioneGenerale.StatoFormazione, 0) AS StatoFormazione " _
               & "  FROM attività INNER JOIN " _
               & " AttivitàFormazioneGenerale ON attività.IDAttività = AttivitàFormazioneGenerale.IdAttività " _
               & " WHERE  (attività.CodiceEnte = '" & strCodProgetto & "') "

        dtrStato = ClsServer.CreaDatareader(strSql, Session("conn"))

        dtrStato.Read()
        If dtrStato.HasRows Then
            intStato = dtrStato("StatoFormazione")
        Else
            intStato = 0
        End If

        dtrStato.Close()
        dtrStato = Nothing

        Return intStato

    End Function

    Private Function VerificaBandoProgetto(ByVal strCodProgetto As String) As Boolean
        'VerificaScadenzaCaricamentoORE
        Dim dtrStato As SqlClient.SqlDataReader
        Dim strSql As String
        Dim bolBando As Boolean


        'verifico che il bando abbia il campo FormazioneGenerale=1
        strSql = "SELECT attività.IDAttività FROM attività INNER JOIN " & _
                 " BandiAttività ON attività.IDBandoAttività = BandiAttività.IdBandoAttività " & _
                 " INNER JOIN  bando ON BandiAttività.IdBando = bando.IDBando " & _
                 " WHERE (bando.FormazioneGenerale = 1) AND (attività.CodiceEnte = '" & strCodProgetto & "')"


        dtrStato = ClsServer.CreaDatareader(strSql, Session("conn"))

        dtrStato.Read()
        If dtrStato.HasRows Then
            bolBando = True
        Else
            bolBando = False
        End If

        dtrStato.Close()
        dtrStato = Nothing

        Return bolBando

    End Function

    Private Function VerificaScadenzaCaricamentoORE(ByVal strCodProgetto As String) As Boolean
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
        ''23/01/2008 da simona cordella
        ''controllo la data inizio solo per il progetto attivo, in questo modo escludo i progetti coprogettati che hanno lo stato di archiviato
        'strSql = "SELECT isnull(datediff(dd,DataInizioAttività,getdate()),0) as DiffGG " & _
        '        " FROM attività WHERE CodiceEnte = '" & strCodProgetto & "' and IdStatoAttività in(1,2)"


        'dtrStato = ClsServer.CreaDatareader(strSql, Session("conn"))

        'dtrStato.Read()


        'If dtrStato("DiffGG") <= 180 And dtrStato("DiffGG") > 0 Then
        '    bEsitoData = True
        'Else
        '    bEsitoData = False
        'End If

        'modificato da s.c. il 05/10/2015
        ' controllo la DataScadenzaUnicaTranche (entro i 180 gg) viente utilizzata anche per le duetranche
        strSql = " Select a.IDAttività "
        strSql &= " FROM Attività a "
        strSql &= " INNER JOIN AttivitàFormazioneGenerale afg on a.IDAttività=afg.IdAttività"
        strSql &= " WHERE a.CodiceEnte='" & strCodProgetto & "'  and a.IdStatoAttività in(1,2)"
        strSql &= " and GETDATE() < afg.DataScadenzaUnicaTranche "

        dtrStato = ClsServer.CreaDatareader(strSql, Session("conn"))
        bEsitoData = dtrStato.HasRows



        dtrStato.Close()
        dtrStato = Nothing

        Return bEsitoData

    End Function

    Private Function VerificaIbanVolontario(ByVal strCodVolontario As String, ByVal OreFormazione As Integer, ByVal CodProgetto As String) As Boolean
        'creato il 04/11/2013 da Antonello 
        Dim strSql As String
        Dim bEsitoVerifica As Boolean
        Dim dtrIbanVerifica As SqlClient.SqlDataReader
        Dim intDurataformazionegenerale As Integer

        'verifico le ore previste sul progetto

        strSql = "SELECT  afg.durataformazionegenerale FROM attività a inner join attivitàformazionegenerale afg on a.idattività = afg.idattività WHERE a.codiceente = '" & CodProgetto & "'"
        dtrIbanVerifica = ClsServer.CreaDatareader(strSql, Session("conn"))

        If dtrIbanVerifica.HasRows Then
            dtrIbanVerifica.Read()
            intDurataformazionegenerale = dtrIbanVerifica("durataformazionegenerale")
        Else
            intDurataformazionegenerale = 0
        End If
        If Not dtrIbanVerifica Is Nothing Then
            dtrIbanVerifica.Close()
            dtrIbanVerifica = Nothing
        End If



        If intDurataformazionegenerale <= OreFormazione Then
            'verifico che volontario sia in possesso del codice iban
            strSql = "SELECT isnull(IBAN,'') as IBAN FROM entità WHERE CodiceVolontario = '" & strCodVolontario & "'"
            dtrIbanVerifica = ClsServer.CreaDatareader(strSql, Session("conn"))
            dtrIbanVerifica.Read()
            If dtrIbanVerifica.HasRows Then
                If dtrIbanVerifica("IBAN") <> "" Then
                    bEsitoVerifica = True
                Else
                    bEsitoVerifica = False
                End If
            Else
                bEsitoVerifica = True
            End If

            If Not dtrIbanVerifica Is Nothing Then
                dtrIbanVerifica.Close()
                dtrIbanVerifica = Nothing
            End If
        Else
            bEsitoVerifica = True
        End If

        Return bEsitoVerifica
    End Function
    Private Function VerificaIbanVolontarioTranche(ByVal strCodVolontario As String, ByVal CodProgetto As String, ByVal OreFormazioneST As String) As Boolean
        'creato il 29/05/2015 da Simona
        Dim strSql As String
        Dim bEsitoVerifica As Boolean
        Dim dtrIbanVerifica As SqlClient.SqlDataReader
        Dim dtrOre As SqlClient.SqlDataReader
        Dim intDurataformazionegenerale As Integer
        Dim OreFormazioneTot As Integer

        'verifico le ore previste sul progetto
        strSql = "SELECT  afg.durataformazionegenerale FROM attività a inner join attivitàformazionegenerale afg on a.idattività = afg.idattività WHERE a.codiceente = '" & CodProgetto & "'"
        dtrIbanVerifica = ClsServer.CreaDatareader(strSql, Session("conn"))

        If dtrIbanVerifica.HasRows Then
            dtrIbanVerifica.Read()
            intDurataformazionegenerale = dtrIbanVerifica("durataformazionegenerale")
        Else
            intDurataformazionegenerale = 0
        End If
        If Not dtrIbanVerifica Is Nothing Then
            dtrIbanVerifica.Close()
            dtrIbanVerifica = Nothing
        End If

        'controllare sum prime e seconda tranche 
        strSql = "SELECT isnull(OreFormazionePrimaTranche,0) as OreFormazionePrimaTranche, isnull(OreFormazioneSecondaTranche,0) as OreFormazioneSecondaTranche FROM entità WHERE CodiceVolontario = '" & strCodVolontario & "'"
        dtrOre = ClsServer.CreaDatareader(strSql, Session("conn"))
        dtrOre.Read()
        OreFormazioneTot = CInt(dtrOre("OreFormazionePrimaTranche")) + CInt(OreFormazioneST)
        If Not dtrOre Is Nothing Then
            dtrOre.Close()
            dtrOre = Nothing
        End If


        If intDurataformazionegenerale <= OreFormazioneTot Then
            'verifico che volontario sia in possesso del codice iban
            strSql = "SELECT isnull(IBAN,'') as IBAN FROM entità WHERE CodiceVolontario = '" & strCodVolontario & "'"
            dtrIbanVerifica = ClsServer.CreaDatareader(strSql, Session("conn"))
            dtrIbanVerifica.Read()
            If dtrIbanVerifica.HasRows Then
                If dtrIbanVerifica("IBAN") <> "" Then
                    bEsitoVerifica = True
                Else
                    bEsitoVerifica = False
                End If
            Else
                bEsitoVerifica = True
            End If

            If Not dtrIbanVerifica Is Nothing Then
                dtrIbanVerifica.Close()
                dtrIbanVerifica = Nothing
            End If
        Else
            bEsitoVerifica = True
        End If

        Return bEsitoVerifica
    End Function
    Private Function VerificaCaricamentoCorsiFormazioneADC(ByVal strCodProgetto As String) As Boolean
        'Antonello
        Dim dtrStato As SqlClient.SqlDataReader
        Dim strSql As String
        Dim bEsitoFormazione As Boolean


        'verifico che il bando abbia il campo PianificazioneFormazione=1
        strSql = "SELECT PianificazioneFormazione FROM attività INNER JOIN " & _
                 " BandiAttività ON attività.IDBandoAttività = BandiAttività.IdBandoAttività " & _
                 " INNER JOIN  bando ON BandiAttività.IdBando = bando.IDBando " & _
                 " WHERE attività.CodiceEnte = '" & strCodProgetto & "'"


        dtrStato = ClsServer.CreaDatareader(strSql, Session("conn"))

        dtrStato.Read()
        If dtrStato.HasRows Then

            If dtrStato("PianificazioneFormazione") = True Then

                dtrStato.Close()
                dtrStato = Nothing
                'verifico che ci sia la pianificazione corso 'Si'
                strSql = "select distinct case isnull(ltrim(rtrim(sedecorso)),'') when '' then 'No' else 'Si' end as DatiPianificazione "
                strSql = strSql & " FROM attività a "
                strSql = strSql & " INNER JOIN BandiAttività h ON a.IDBandoAttività = h.IdBandoAttività "
                strSql = strSql & " INNER JOIN bando c ON c.IDBando = h.IdBando "
                strSql = strSql & " INNER JOIN AttivitàFormazioneGenerale On a.idattività = AttivitàFormazioneGenerale.idattività"
                strSql = strSql & " WHERE A.idstatoattività =1 and c.pianificazioneformazione = 1"
                strSql = strSql & " and A.CODICEENTE = '" & ClsServer.NoApice(Trim(strCodProgetto)) & "'"
                dtrStato = ClsServer.CreaDatareader(strSql, Session("conn"))
                dtrStato.Read()

                If dtrStato.HasRows Then
                    If dtrStato("DatiPianificazione") = "Si" Then
                        bEsitoFormazione = True
                    Else
                        bEsitoFormazione = False
                    End If
                    dtrStato.Close()
                    dtrStato = Nothing
                    Return bEsitoFormazione
                    Exit Function
                End If

            Else
                bEsitoFormazione = True
            End If

        End If

        dtrStato.Close()
        dtrStato = Nothing

        Return bEsitoFormazione

    End Function

    Private Function VerificaDataPianificazioneCorsi(ByVal strCodProgetto As String) As Boolean
        'creato il 28/08/2009 da simona e danilo
        'blocco il caricamento della formazione se non risulta ancora finito il corso 
        Dim strSql As String
        Dim bEsitoFormazione As Boolean
        Dim dtrPianificazione As SqlClient.SqlDataReader

        'verifico che il bando abbia il campo PianificazioneFormazione=1
        strSql = "SELECT af.DataFineCorso FROM attività a INNER JOIN " & _
                 " attivitàformazionegenerale af on a.idattività=af.idattività " & _
                 " WHERE a.CodiceEnte = '" & strCodProgetto & "' and af.datafinecorso < getdate()"
        dtrPianificazione = ClsServer.CreaDatareader(strSql, Session("conn"))
        If dtrPianificazione.HasRows Then
            bEsitoFormazione = True
        Else
            bEsitoFormazione = False
        End If
        dtrPianificazione.Close()
        dtrPianificazione = Nothing

        Return bEsitoFormazione
    End Function

    Private Function VerificaEntita(ByVal strCodVolontario As String, ByVal strCognome As String, ByVal strNome As String, ByVal strCodFiscale As String, ByVal strCodProgetto As String) As Boolean
        Dim dtrEntita As SqlClient.SqlDataReader
        Dim strSql As String
        Dim bolEsito As Boolean


        strSql = "SELECT * " _
        & " FROM   entità inner join statientità on entità.idstatoentità = statientità.idstatoentità INNER JOIN " _
        & " attivitàentità ON entità.IDEntità = attivitàentità.IDEntità INNER JOIN " _
        & " attivitàentisediattuazione ON attivitàentità.IDAttivitàEnteSedeAttuazione = attivitàentisediattuazione.IDAttivitàEnteSedeAttuazione INNER JOIN " _
        & " attività ON attivitàentisediattuazione.IDAttività = attività.IDAttività " _
        & " WHERE  attività.IDEntePresentante ='" & Session("IdEnte") & "' " _
        & " And CodiceVolontario='" & strCodVolontario.Replace("'", "''") & "' " _
        & " And isnull(replace(replace(replace(replace(replace(replace(replace(Cognome,'°',''),'ì','i'''),'é','e'''),'à','a'''),'ò','o'''),'è','e'''),'ù','u'''), '')='" & strCognome.Replace("'", "''") & "' " _
        & " And isnull(replace(replace(replace(replace(replace(replace(replace(Nome,'°',''),'ì','i'''),'é','e'''),'à','a'''),'ò','o'''),'è','e'''),'ù','u'''), '')='" & strNome.Replace("'", "''") & "' " _
        & " And CodiceFiscale ='" & strCodFiscale.Replace("'", "''") & "' " _
        & " And attività.codiceente='" & strCodProgetto.Replace("'", "''") & "' " _
        & " And (StatiEntità.InServizio = 1 OR StatiEntità.Sospeso = 1 OR StatiEntità.Chiuso = 1) AND attivitàentità.EscludiFormazione=0 "
        dtrEntita = ClsServer.CreaDatareader(strSql, Session("conn"))

        bolEsito = dtrEntita.HasRows

        dtrEntita.Close()
        dtrEntita = Nothing

        Return bolEsito

    End Function

    Private Sub ScriviTabTemp(ByVal pArray() As String)
        '--- scrive nella tab temporanea
        Dim cmdTemp As SqlClient.SqlCommand
        Dim strsql As String

        Try

            strsql = "INSERT INTO #TEMP_ORE_VOLONATARIO " & _
                     "(CodiceVolontario, " & _
                     "Cognome, " & _
                     "Nome, " & _
                     "CodiceFiscale, " & _
                     "CodiceProgetto, " & _
                     "OreFormazione " & _
                     ") " & _
                     "values " & _
                     "('" & Trim(ClsServer.NoApice(pArray(0))) & "', '" & _
                     Trim(ClsServer.NoApice(pArray(1))) & "', '" & _
                     Trim(ClsServer.NoApice(pArray(2))) & "', '" & _
                     Trim(ClsServer.NoApice(pArray(4))) & "', '" & _
                     Trim(ClsServer.NoApice(pArray(8))) & "', '" & _
                     Trim(ClsServer.NoApice(pArray(15))) & "' " & _
                     ")"

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

   Private Sub LeggiCSVTranche()
        '--- lettura del file
        Dim Reader As StreamReader
        Dim xLinea As String
        Dim ArrCampi() As String
        Dim swErr As Boolean
        Dim AppoNote As String
        Dim intOreForm As Integer

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

            If UBound(ArrCampi) < 16 Then
                '--- se i campi non sono tutti errore
                strNote = "Il numero delle colonne inserite è minore di quello richieste."
                swErr = True
                TotKo = TotKo + 1
            Else
                If UBound(ArrCampi) > 16 Then
                    '--- se i campi sono troppi errore
                    strNote = "Il numero delle colonne inserite è maggiore di quello richieste."
                    swErr = True
                    TotKo = TotKo + 1

                Else
                    'CodiceVolontario
                    If Trim(ArrCampi(0)) = vbNullString Then
                        strNote = strNote & "Il campo CodiceVolontario e' un campo obbligatorio."
                        swErr = True
                    End If

                    'Cognome
                    If Trim(ArrCampi(1)) = vbNullString Then
                        strNote = strNote & "Il campo Cognome e' un campo obbligatorio."
                        swErr = True
                    End If

                    'Nome
                    If Trim(ArrCampi(2)) = vbNullString Then
                        strNote = strNote & "Il campo Nome e' un campo obbligatorio."
                        swErr = True
                    End If

                    'CodiceFiscale
                    If Trim(ArrCampi(4)) = vbNullString Then
                        strNote = strNote & "Il campo CodiceFiscale e' un campo obbligatorio."
                        swErr = True
                    End If

                    'CodiceProgetto
                    If Trim(ArrCampi(8)) = vbNullString Then
                        strNote = strNote & "Il campo CodiceProgetto e' un campo obbligatorio."
                        swErr = True
                    Else
                        'verificacompatibilità agg. il 02/12/2014 da s.c.
                        If VerificaCompatibilita(ArrCampi(8)) = False Then
                            strNote = strNote & "Progetto non esistente."
                            swErr = True
                        End If
                        'Statoprogetto (in attivitàformazionegenerale)
                        If VerificaStato(ArrCampi(8)) <> 0 Then
                            strNote = strNote & "Il progetto è stato 'Confermato'. Impossibile procedere con l'importazione."
                            swErr = True
                        End If

                        If VerificaBandoProgetto(ArrCampi(8)) = False Then
                            strNote = strNote & "Il progetto è associato ad un bando per il quale non è ancora possibile caricare ore di formazione. "
                            swErr = True
                        End If
                        'verifca data pianificazione corsi prima tranche
                        If VerificaDataPianificazioneCorsi(ArrCampi(8)) = False Then
                            strNote = strNote & "La prima Tranche del corso di Formazione non risulta ancora terminato secondo quanto indicato in fase di pianificazione corso."
                            swErr = True
                        End If
                        'corsi di formazione
                        If VerificaCaricamentoCorsiFormazioneADC(ArrCampi(8)) = False Then
                            strNote = strNote & "Il progetto prevede che sia caricata la Pianificazione dei Corsi. "
                            swErr = True
                        Else
                            'verifico se per il progetto indicato è previsto il caricamento della formazione a tranche
                            If VerificaCaricamentoTipoFormazioneGenerale(ArrCampi(8)) = 1 Then '(TipoFormazioneGenerale=1 progetto con Formazione UNICATRANCHE)
                                strNote = strNote & "Il progetto indicato prevede la formazione in una UNICA TRANCHE."
                                swErr = True
                            Else
                                'tranche
                                If Trim(ArrCampi(15)) = vbNullString And Trim(ArrCampi(16)) <> vbNullString Then
                                    strNote = strNote & "Il campo OreFormazionePrimaTranche e' un campo obbligatorio se viene indicato il campo OreFormazioneSecondaTranche."
                                    swErr = True
                                End If
                                'controllo OreFormazionePrimaTranche()
                                If Trim(ArrCampi(15)) = vbNullString Then
                                    strNote = strNote & "Il campo OreFormazionePrimaTranche e' un campo obbligatorio."
                                    swErr = True
                                    intOreForm = 9999
                                Else
                                    If IsNumeric(ArrCampi(15)) = False Then
                                        strNote = strNote & "Il campo OreFormazionePrimaTranche non e' nel formato corretto."
                                        swErr = True
                                        intOreForm = 9999
                                    ElseIf InStr(ArrCampi(15), ".") <> 0 Or InStr(ArrCampi(15), "-") <> 0 Or InStr(ArrCampi(15), ",") <> 0 Then
                                        strNote = strNote & "Il campo OreFormazione deve essere un numero intero."
                                        swErr = True
                                        intOreForm = 9999
                                    Else
                                        intOreForm = ArrCampi(15)
                                    End If
                                    'controllo se sono nei termini di caricamento della PRIMA TRANCE
                                    If VerificoCaricamentoTerminiPrimaTranche(ArrCampi(8)) = False Then
                                        'controllo se ho una proroga
                                        If VerificoEsitenzaDataProroga(ArrCampi(8)) = False Then
                                            'controllo se ore formazione sul db sono uguali a quello del file
                                            If VerificoCaricamentoOrePrimaTranche(ArrCampi(0), Trim(ArrCampi(15))) = False Then
                                                strNote = strNote & "Impossibile modificare le OreFormazionePrimaTranche perchè sono scaduti i termini per il caricamento. "
                                                swErr = True
                                            End If
                                        End If
                                    End If
                                    'Else 'ok primatranche
                                    'controllo se nel file ho indicato delle ore seconda tranche
                                    If Trim(ArrCampi(16)) <> vbNullString Then 'controllo se la prima tranche è stata indicata
                                        If IsNumeric(ArrCampi(16)) = False Then 'controllo se è un campo numerio
                                            strNote = strNote & "Il campo OreSecondaFormazione non e' nel formato corretto."
                                            swErr = True
                                            intOreForm = 9999
                                        ElseIf InStr(ArrCampi(16), ".") <> 0 Or InStr(ArrCampi(16), "-") <> 0 Or InStr(ArrCampi(16), ",") <> 0 Then
                                            strNote = strNote & "Il campo OreFormazioneSecondaTranche deve essere un numero intero."
                                            swErr = True
                                            intOreForm = 9999
                                        Else
                                            intOreForm = ArrCampi(16)
                                        End If
                                        'cotrollo se rietrno nei 270 gg per la pianificazione seconda tranch
                                        If VerificoCaricamentoTerminiSecondaTranche(ArrCampi(8)) = False Then
                                            'controllo se esiste una proroga > scadenza seconda tranche
                                            If VerificoEsitenzaDataProrogaScadenzaSecondaTranche(ArrCampi(8)) = False Then
                                                strNote = strNote & "Impossibile modificare le OreFormazioneSecondaTranche perchè sono scaduti i termini per il caricamento. "
                                                swErr = True
                                                'Else
                                            End If
                                        Else    'controllo se data seconda tranche sia terminata ok (secondatranche terminata)
                                            If VerificoDataPianificazioneSecondaTranche(ArrCampi(8)) = False Then
                                                strNote = strNote & "Il corso della seconda Tranche non è ancora terminato."
                                                swErr = True
                                            End If
                                        End If
                                    End If
                                    'carico le ore prima tranche
                                    'End If
                                End If
                            End If
                        End If
                    End If
                    If Trim(ArrCampi(16)) <> vbNullString Then
                        'verifico se il volontario e' in possesso del codice IBAN
                        If VerificaIbanVolontarioTranche(ArrCampi(0), ArrCampi(8), ArrCampi(16)) = False Then
                            strNote = strNote & "Il Volontario non è in possesso del codice IBAN pertanto non è possibile indicare le ore di formazione."
                            swErr = True
                        End If
                    End If

                    'verifico l'esistenza in banca dati                    
                    If VerificaEntita(Trim(ArrCampi(0)), Trim(ArrCampi(1)), Trim(ArrCampi(2)), Trim(ArrCampi(4)), Trim(ArrCampi(8))) = False Then
                        strNote = strNote & "Volontario non trovato o per il quale non è previsto il caricamento delle ore di formazione."
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

        '--- reindirizzo la pagina sottostante
        Response.Redirect("WfrmRisultatoImportOreVolontari.aspx?TipoFormazioneGenerale=" & DOPPIA_TRANCE & "&NomeFile=" & NomeUnivoco & "&Tot=" & Tot & "&TotOk=" & TotOk & "&TotKo=" & TotKo)

        'Response.Write("<script>" & vbCrLf)
        'Response.Write("parent.Naviga('WfrmRisultatoImportOreVolontari.aspx?NomeFile=" & NomeUnivoco & "&Tot=" & Tot & "&TotOk=" & TotOk & "&TotKo=" & TotKo & "')" & vbCrLf)
        'Response.Write("</script>")

    End Sub

    Private Sub CmdElaboraDoppiaTrance_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdElaboraDoppiaTrance.Click
        UpLoad(DOPPIA_TRANCE)
    End Sub

    Private Function VerificaCaricamentoTipoFormazioneGenerale(ByVal strCodProgetto As String) As Integer
        'verifico il tipo di formazione che è stato indicato il fase di progettazione
        Dim dtrStato As SqlClient.SqlDataReader
        Dim strSql As String
        Dim TipoFormazioneGenerale As Integer

        strSql = " SELECT TipoFormazioneGenerale " & _
                 " FROM attività  " & _
                 " INNER JOIN  AttivitàFormazioneGenerale ON attività.IDAttività = AttivitàFormazioneGenerale.IdAttività " & _
                 " WHERE attività.CodiceEnte = '" & strCodProgetto & "'"
        dtrStato = ClsServer.CreaDatareader(strSql, Session("conn"))
        If dtrStato.HasRows = True Then
            dtrStato.Read()
            TipoFormazioneGenerale = dtrStato("TipoFormazioneGenerale")
        End If
        dtrStato.Close()
        dtrStato = Nothing
        Return TipoFormazioneGenerale
    End Function

    Private Function VerificaDataPianificazioneCorsiSecondaTranche(ByVal strCodProgetto As String) As Boolean
        'creato il 10/04/2015 da simona 
        'blocco il caricamento della formazione se non risulta ancora finito il corso della seconda tranche
        Dim strSql As String
        Dim bEsitoFormazione As Boolean
        Dim dtrPianificazione As SqlClient.SqlDataReader

        'verifico che il bando abbia il campo PianificazioneFormazione=1
        strSql = "SELECT af.DataFineCorsoSecondaTranche FROM attività a INNER JOIN " & _
                 " attivitàformazionegenerale af on a.idattività=af.idattività " & _
                 " WHERE a.CodiceEnte = '" & strCodProgetto & "' and af.datafinecorso < getdate()"
        dtrPianificazione = ClsServer.CreaDatareader(strSql, Session("conn"))
        If dtrPianificazione.HasRows Then
            bEsitoFormazione = True
        Else
            bEsitoFormazione = False
        End If
        dtrPianificazione.Close()
        dtrPianificazione = Nothing

        Return bEsitoFormazione
    End Function

    Private Sub ScriviTabTempTranche(ByVal pArray() As String)
        '--- scrive nella tab temporanea
        Dim cmdTemp As SqlClient.SqlCommand
        Dim strsql As String

        Try

            strsql = "INSERT INTO #TEMP_ORE_VOLONATARIO " & _
                      "(CodiceVolontario, " & _
                      "Cognome, " & _
                      "Nome, " & _
                      "CodiceFiscale, " & _
                      "CodiceProgetto, " & _
                      "OreFormazionePrimaTranche, " & _
                      "OreFormazioneSecondaTranche " & _
                      ") " & _
                      "values " & _
                      "('" & Trim(ClsServer.NoApice(pArray(0))) & "', '" & _
                      Trim(ClsServer.NoApice(pArray(1))) & "', '" & _
                      Trim(ClsServer.NoApice(pArray(2))) & "', '" & _
                      Trim(ClsServer.NoApice(pArray(4))) & "', '" & _
                      Trim(ClsServer.NoApice(pArray(8))) & "', " & _
                      IIf(Trim(ClsServer.NoApice(pArray(15))) = "", "NULL", Trim(ClsServer.NoApice(pArray(15)))) & "," & _
                      IIf(Trim(ClsServer.NoApice(pArray(16))) = "", "NULL", Trim(ClsServer.NoApice(pArray(16)))) & "  " & _
                      ")"

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

    Private Function VerificoCaricamentoOrePrimaTranche(ByVal strCodVolontario As String, ByVal OrePrimaTranche As String) As Boolean
        'agg il 27/05/2015 da simona cordella
        'verifico se sono nei termini previsti per il caricamento della ore prima tranche (Entro i 180 dalla datainizioprogetto)


        'verifica se sono state modificate le oreprimatranche

        Dim dtrStato As SqlClient.SqlDataReader
        Dim strSql As String
        Dim bEsitoOre As Boolean

        strSql = " Select OreFormazionePrimaTranche" & _
                 " From Entità" & _
                 " WHERE CodiceVolontario ='" & strCodVolontario & "'  " & _
                 " and (OreFormazionePrimaTranche  is null or OreFormazionePrimaTranche <> " & OrePrimaTranche & " )"

        dtrStato = ClsServer.CreaDatareader(strSql, Session("conn"))

        If dtrStato.HasRows = True Then
            dtrStato.Read()
            bEsitoOre = False
        Else
            bEsitoOre = True
        End If

        dtrStato.Close()
        dtrStato = Nothing

        Return bEsitoOre

    End Function
    Private Function VerificoCaricamentoTerminiPrimaTranche(ByVal strCodProgetto As String) As Boolean
        'agg il 27/05/2015 da simona cordella
        'verifico se sono nei termini previsti per il caricamento della ore prima tranche (Entro i 180 dalla datainizioprogetto)

        Dim dtrTermine As SqlClient.SqlDataReader
        Dim strSql As String
        Dim bEsitoTermine As Boolean

        'Dim DataTerminiPrimaTranche As Date
        'Dim DataOdierna As Date

        ''dateadd(day,180,DataInizioAttività) as DataTerminePrimaTranche,
        'strSql = "SELECT dateadd(day,180,DataInizioAttività) as DataTerminePrimaTranche," & _
        '         " dbo.formatodatadt(getdate()) as DataOdierna " & _
        '         " FROM attività a INNER JOIN " & _
        '         " attivitàformazionegenerale af on a.idattività=af.idattività " & _
        '         " WHERE a.CodiceEnte = '" & strCodProgetto & "' "
        '' and af.datafinecorso < getdate()"
        'dtrTermine = ClsServer.CreaDatareader(strSql, Session("conn"))
        'dtrTermine.Read()

        ''posso caricare la ore prima tranche entro il 180° dalla datainizio attività e dopo il terminde della pianificazione della seconda tranche
        'DataTerminiPrimaTranche = dtrTermine("DataTerminePrimaTranche")
        'DataOdierna = dtrTermine("DataOdierna")
        'If DataOdierna <= DataTerminiPrimaTranche Then
        '    bEsitoTermine = True
        'Else
        '    bEsitoTermine = False
        'End If


        'modificato da s.c. il 05/10/2015 
        'controllo la scadenza 
        strSql = " Select a.IDAttività "
        strSql &= " FROM Attività a "
        strSql &= " INNER JOIN AttivitàFormazioneGenerale afg on a.IDAttività=afg.IdAttività"
        strSql &= " WHERE a.CodiceEnte='" & strCodProgetto & "' "
        strSql &= "  AND GETDATE() < afg.DataScadenzaPrimaTranche "
        ''posso caricare la ore prima tranche entro il 180° dalla datainizio attività e dopo il terminde della pianificazione della seconda tranche
        dtrTermine = ClsServer.CreaDatareader(strSql, Session("conn"))
        bEsitoTermine = dtrTermine.HasRows


        dtrTermine.Close()
        dtrTermine = Nothing
        Return bEsitoTermine
    End Function

    Private Function VerificoCaricamentoTerminiSecondaTranche(ByVal strCodProgetto As String) As Boolean
        'agg il 28/05/2015 da simona cordella
        'verifico se sono nei termini previsti per il caricamento della ore seconda tranche (tra il 210 e 270 giorno dalla datainizioprogetto)

        Dim dtrStato As SqlClient.SqlDataReader
        Dim strSql As String
        Dim bEsitoData As Boolean


   

        'strSql = " Select Dbo.formatodata(dateadd(day,270,DataInizioAttività)) as DataSecondoIntervallo," & _
        '         " Dbo.formatodata(getdate()) as DataOdierna," & _
        '         " Dbo.formatodata(AttivitàFormazioneGenerale.DataFineCorsoSecondaTranche)" & _
        '         " FROM Attività " & _
        '         " INNER JOIN AttivitàFormazioneGenerale on attività.IDAttività = AttivitàFormazioneGenerale.IdAttività" & _
        '         " WHERE CodiceEnte ='" & strCodProgetto & "' " & " " & _
        '        " AND Dbo.formatodataDT(getdate())<= dateadd(day,270,DataInizioAttività)"

        'dtrStato = ClsServer.CreaDatareader(strSql, Session("conn"))

        'If dtrStato.HasRows Then
        '    bEsitoData = True
        'Else
        '    bEsitoData = False
        'End If

        'modificato da s.c. il 05/10/2015 
        'controllo la scadenza 
        strSql = " Select a.IDAttività "
        strSql &= " FROM Attività a "
        strSql &= " INNER JOIN AttivitàFormazioneGenerale afg on a.IDAttività=afg.IdAttività"
        strSql &= " WHERE a.CodiceEnte='" & strCodProgetto & "' "
        'strSql &= " AND GETDATE() > DateAdd(Day,210,DataInizioAttività)"
        strSql &= " AND GETDATE() < afg.DataScadenzaSecondaTranche "



        ''posso caricare la ore prima tranche entro il 180° dalla datainizio attività e dopo il terminde della pianificazione della seconda tranche
        dtrStato = ClsServer.CreaDatareader(strSql, Session("conn"))
        bEsitoData = dtrStato.HasRows

        If Not dtrStato Is Nothing Then
            dtrStato.Close()
            dtrStato = Nothing
        End If
        Return bEsitoData
    End Function

    Private Function VerificoEsitenzaDataProroga(ByVal strCodProgetto As String) As Boolean
        '28/05/2015 da simona cordella
        'verifica se esiste una proroga attiva
        Dim strSql As String
        Dim dtrStato As SqlClient.SqlDataReader
        Dim strDataProroga As String
        'Dim strDataTerminePrimaTranche As String
        Dim bEsitoData As Boolean

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
        If Not dtrStato Is Nothing Then
            dtrStato.Close()
            dtrStato = Nothing
        End If

        If strDataProroga <> "0" Then

            strSql = "SELECT isnull(datediff(dd,DataProroga,getdate()),-1) as DiffGG " & _
                        " From AttivitàFormazioneGenerale INNER JOIN " & _
                 " attività ON AttivitàFormazioneGenerale.IdAttività = attività.IDAttività " & _
                 " Where CodiceEnte = '" & strCodProgetto & "'"

            dtrStato = ClsServer.CreaDatareader(strSql, Session("conn"))
            dtrStato.Read()

            If dtrStato("DiffGG") > 7 Then
                bEsitoData = False              'proroga scaduta 
            Else
                bEsitoData = True               'proroga 7 giorni valida
            End If
            If Not dtrStato Is Nothing Then
                dtrStato.Close()
                dtrStato = Nothing
            End If


            Return bEsitoData

            Exit Function
        Else
            bEsitoData = False
            Return bEsitoData
        End If
        ''vedo se è stata impostata la data proroga
        'strSql = "Select ISNULL(CONVERT(varchar, DataProroga, 103), 0) AS DataProroga, " & _
        '         " dateadd(day,180,DataInizioAttività) as DataTerminePrimaTranche " & _
        '         " From AttivitàFormazioneGenerale INNER JOIN " & _
        '         " attività ON AttivitàFormazioneGenerale.IdAttività = attività.IDAttività " & _
        '         " Where CodiceEnte = '" & strCodProgetto & "' and  DataProroga<>0 and DataProroga >= dbo.formatodata(getdate())"
        'dtrData = ClsServer.CreaDatareader(strSql, Session("conn"))
        'dtrData.Read()

        'If dtrData.HasRows Then
        '    bEsito = True
        'Else
        '    bEsito = False
        'End If



    End Function

    Private Function VerificoEsitenzaDataProrogaScadenzaSecondaTranche(ByVal strCodProgetto As String) As Boolean
        '29/05/2015 da simona cordella
        'verifica se esiste una proroga attiva
        Dim strSql As String
        Dim dtrData As SqlClient.SqlDataReader
        Dim strDataProroga As String
        Dim strDataProrogaDT As Date
        Dim strDataTermineSecondaTranche As Date
        Dim bEsito As Boolean

        'vedo se è stata impostata la data proroga
        strSql = "Select ISNULL(CONVERT(varchar, DataProroga, 103), 0) AS DataProroga,isnull(DataProroga,'01/01/2000') as DataProrogaDT, " & _
                 " dateadd(day,270,DataInizioAttività) as DataTermineSecondaTranche, dbo.formatoDataDT(AttivitàFormazioneGenerale.DataScadenzaSecondaTranche) as DataScadenzaSecondaTranche " & _
                 " From AttivitàFormazioneGenerale INNER JOIN " & _
                 " attività ON AttivitàFormazioneGenerale.IdAttività = attività.IDAttività " & _
                 " Where CodiceEnte = '" & strCodProgetto & "'"
  
        dtrData = ClsServer.CreaDatareader(strSql, Session("conn"))

        If dtrData.HasRows Then
            dtrData.Read()

            strDataTermineSecondaTranche = dtrData("DataScadenzaSecondaTranche")
            strDataProroga = dtrData("DataProroga")
            strDataProrogaDT = dtrData("DataProrogaDT")
        End If
        If Not dtrData Is Nothing Then
            dtrData.Close()
            dtrData = Nothing
        End If
        If strDataProroga <> "0" Then

            strSql = "SELECT isnull(datediff(dd,DataProroga,getdate()),-1) as DiffGG " & _
                              " From AttivitàFormazioneGenerale INNER JOIN " & _
                       " attività ON AttivitàFormazioneGenerale.IdAttività = attività.IDAttività " & _
                       " Where CodiceEnte = '" & strCodProgetto & "'"

            dtrData = ClsServer.CreaDatareader(strSql, Session("conn"))
            dtrData.Read()

            If dtrData("DiffGG") > 7 Then
                bEsito = False              'proroga scaduta 
            Else
                'bEsito = True               'proroga 7 giorni valida


                If strDataProrogaDT > strDataTermineSecondaTranche Then
                    bEsito = True
                Else
                    bEsito = False
                End If

            End If
            If Not dtrData Is Nothing Then
                dtrData.Close()
                dtrData = Nothing
            End If
        Else
            bEsito = False
        End If
        If Not dtrData Is Nothing Then
            dtrData.Close()
            dtrData = Nothing
        End If
        Return bEsito
    End Function

    Private Function VerificoDataPianificazioneSecondaTranche(ByVal strCodProgetto As String) As Boolean
        Dim dtrStato As SqlClient.SqlDataReader
        Dim strSql As String
        Dim bEsitoData As Boolean
        Dim DataOdierna As String
        Dim DataPrimoIntervallo As String
        Dim DataSecondoIntervallo As String

        strSql = " Select " & _
                 " Dbo.formatodata(getdate()) as DataOdierna," & _
                 " Dbo.formatodata(AttivitàFormazioneGenerale.DataFineCorsoSecondaTranche)" & _
                 " FROM Attività " & _
                 " INNER JOIN AttivitàFormazioneGenerale on attività.IDAttività = AttivitàFormazioneGenerale.IdAttività" & _
                 " WHERE CodiceEnte ='" & strCodProgetto & "' " & " " & _
                " AND Dbo.formatodatadt(getdate())>= Dbo.formatodatadt(AttivitàFormazioneGenerale.DataFineCorsoSecondaTranche)"

        dtrStato = ClsServer.CreaDatareader(strSql, Session("conn"))

        If dtrStato.HasRows Then
            bEsitoData = True
        Else
            bEsitoData = False
        End If
        If Not dtrStato Is Nothing Then
            dtrStato.Close()
            dtrStato = Nothing
        End If
        Return bEsitoData

    End Function
End Class