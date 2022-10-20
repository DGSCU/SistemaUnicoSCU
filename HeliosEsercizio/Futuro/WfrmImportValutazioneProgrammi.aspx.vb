Imports System.IO
Public Class WfrmImportValutazioneProgrammi
    Inherits System.Web.UI.Page
    Private Writer As StreamWriter
    Private strNote As String = ""
    Private Tot As Integer
    Private TotOk As Integer
    Private TotKo As Integer
    Private NomeUnivoco As String
    Private NomeUnivocoErr As String
    Private xIdAttivita As Integer
    Private dtParametri As DataTable

    Private Enum TipoImportazione
        Estero = 0
        Nazionale = 1
        Regionale = 1
    End Enum
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Session("LogIn") Is Nothing Then
            If Session("LogIn") = False Then 'verifico validità log-in
                Response.Redirect("LogOn.aspx")
            End If
        Else
            Response.Redirect("LogOn.aspx")
        End If

        If IsPostBack = False Then

            If Session("IdRegioneCompetenzaUtente") = 22 Then

                phNazionaleEstero.Visible = True

                phNote.Visible = False
            Else

                phNazionaleEstero.Visible = False

                phNote.Visible = True
            End If

        End If
    End Sub

    Private Sub cmdChiudi_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdChiudi.Click
        Response.Redirect("WfrmMain.aspx")
    End Sub


    Private Sub CmdElaboraProgettiNazionale_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdElaboraProgrammaNazionale.Click
        UpLoad(TipoImportazione.Nazionale)
    End Sub

    Private Sub UpLoad(ByVal Importazione As TipoImportazione)
        '--- salvataggio del file sul server
        Dim swErr As Boolean
        Dim txtSelFile As FileUpload

        swErr = False




        Select Case Importazione

            
            Case TipoImportazione.Nazionale
                txtSelFile = DirectCast(txtSelFileProgrammaNazionale, FileUpload)
           
        End Select

        If txtSelFile.PostedFile.FileName <> String.Empty Then
            Try
                NomeUnivoco = "valutazioneprogrammi" & "_" & Session("Utente") & "_" & Year(Now) & Month(Now) & Day(Now) & Hour(Now) & Minute(Now) & Second(Now)

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
                CreaTabTemp(Importazione)

            Catch exc As Exception
                swErr = True
                'Response.Write("<script>" & vbCrLf)
                'Response.Write("parent.fraUp.CambiaImmagine('filenoncaricato.jpg','0')" & vbCrLf)
                'Response.Write("</script>")
                CancellaTabellaTemp()
            End Try

            If swErr = False Then
                'richiamo funzione che cancella tabella temporanea
                DeleteTabelleStoricoTMP(Session("Utente"))
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

    Private Sub CreaTabTemp(ByVal Importazione As TipoImportazione)
        Dim strSql As String
        Dim cmdCreateTempTable As SqlClient.SqlCommand
        Dim NomeParametro As String
        Dim i As Integer

        CancellaTabellaTemp()
        '--- CREO TAB TEMPORANEA DINAMICAMENTE
        dtParametri = Sp_RitornaAssociazioniParametri(Session("IdRegioneCompetenzaUtente"), Importazione)

        strSql = "CREATE TABLE [#TEMP_VALUTAZIONE_PROGRAMMI] ("
        strSql &= " [Note] [nvarchar] (2000) COLLATE DATABASE_DEFAULT "

        For i = 0 To dtParametri.Rows.Count - 1
            strSql &= " ,["
            strSql &= dtParametri.Rows(i).Item(1)
            strSql &= "][nvarchar] (250) COLLATE DATABASE_DEFAULT "
        Next
        strSql &= " )"

        cmdCreateTempTable = New SqlClient.SqlCommand
        cmdCreateTempTable.CommandText = strSql
        cmdCreateTempTable.Connection = Session("conn")
        cmdCreateTempTable.ExecuteNonQuery()
        cmdCreateTempTable.Dispose()

    End Sub

    Private Function Sp_RitornaAssociazioniParametri(ByVal IdRegCompetenzaUtente As Integer, ByVal Nazionale As Integer) As DataTable
        Dim sqlDAP As New SqlClient.SqlDataAdapter
        Dim strNomeStore As String = "[SP_IMPORTVALUTAZIONI_RITORNAASSOCIAZIONEPARAMETRI_PROGRAMMI]"
        Dim dataTB As New DataTable

        Try
            sqlDAP = New SqlClient.SqlDataAdapter(strNomeStore, CType(Session("conn"), SqlClient.SqlConnection))
            sqlDAP.SelectCommand.CommandType = CommandType.StoredProcedure
            sqlDAP.SelectCommand.Parameters.Add("@IDCOMPETENZA", SqlDbType.Int).Value = IdRegCompetenzaUtente
            sqlDAP.SelectCommand.Parameters.Add("@NAZIONALE", SqlDbType.Int).Value = Nazionale
            sqlDAP.Fill(dataTB)
            Return dataTB
        Catch ex As Exception
            Response.Write(ex.Message.ToString())
            Exit Function
        End Try
    End Function

    Private Sub CancellaTabellaTemp()
        Dim strSql As String
        Dim cmdCanTempTable As SqlClient.SqlCommand
        Try
            '--- CANCELLO TAB TEMPORANEA

            strSql = "DROP TABLE [#TEMP_VALUTAZIONE_PROGRAMMI]"

            cmdCanTempTable = New SqlClient.SqlCommand
            cmdCanTempTable.CommandText = strSql
            cmdCanTempTable.Connection = Session("conn")
            cmdCanTempTable.ExecuteNonQuery()
        Catch e As Exception
            e.Message.ToString()
        End Try

        cmdCanTempTable.Dispose()
    End Sub

    Function DeleteTabelleStoricoTMP(ByVal UserName As String)
        'Creata da simona Cordella il 15/02/2013
        Dim strSql As String
        Dim rstDelete As SqlClient.SqlDataReader
        Dim idStoricoTMP As Integer
        'cancello le tabelle di storico
        strSql = "Delete storicovociProgrammiTMPImport where IdStoricoTMP in (Select IdStoricoTMP from storicoProgrammiTMPimport where username = '" & UserName & "')"
        rstDelete = ClsServer.CreaDatareader(strSql, Session("conn"))
        If Not rstDelete Is Nothing Then
            rstDelete.Close()
            rstDelete = Nothing
        End If
        strSql = "Delete storicoProgrammiTMPImport where username = '" & UserName & "'"
        rstDelete = ClsServer.CreaDatareader(strSql, Session("conn"))
        If Not rstDelete Is Nothing Then
            rstDelete.Close()
            rstDelete = Nothing
        End If
    End Function

    Private Sub LeggiCSV()
        '*** GENERATA DA SIMONA CORDELLA IL 11/02/2013
        '*** GESTIONE IMPORTAZIONE DELLA VALUTAZIONE DEI PROGETTI
        '--- lettura del file
        Dim Reader As StreamReader
        Dim xLinea As String
        Dim ArrCampi() As String
        Dim swErr As Boolean

        'Dim WriterErr As StreamWriter

        Dim i As Integer
        Dim strSQL As String
        Dim strSQLDinamica As String
        Dim CodiceParametro As String
        Dim CodiceProgramma As String ' Dim CodiceProgetto As String
        Dim IDProgramma As String
        Dim Letteracolonna As String
        Dim NumColonna As Integer
        Dim StatoProg As String
        Dim Deflettore As String = 0
        Dim PuntReg As String = 0
        Dim blnErrFile As Boolean = False
        Dim TotValFile As String = "0"
        Dim blnErrProgramma As Boolean = False 'TRUE signifa che il codice progetto è inesistene e salvo tutti i controlli relativi al punteggio della valutazione

        '--- Leggo il file di input e scrivo quello di output
        Reader = New StreamReader(Server.MapPath("upload") & "\" & NomeUnivoco & ".CSV", System.Text.Encoding.UTF7, False)
        Writer = New StreamWriter(Server.MapPath("download") & "\" & NomeUnivoco & ".CSV")

        '--- intestazione
        'xLinea = Reader.ReadLine() '
        'Writer.WriteLine("Note;" & xLinea)

        'ArrCampi = CreaArray(xLinea)

        'If UBound(ArrCampi) < dtParametri.Rows.Count Then
        '    strNote = strNote & "Il numero delle colonne inserite è diverso da quello previsto."
        '    swErr = True
        '    TotKo = TotKo + 1
        '    Writer.WriteLine("Note;" & strNote)
        '    strNote = ""
        'End If

        xLinea = Reader.ReadLine()
        While (xLinea <> "")
            swErr = False
            Tot = Tot + 1
            ArrCampi = CreaArray(xLinea)

            For i = 0 To dtParametri.Rows.Count - 1
                CodiceParametro = dtParametri.Rows(i).Item(1)
                Letteracolonna = dtParametri.Rows(i).Item(6) 'dtParametri.Tables(0).Rows(i).Item(6)
                NumColonna = ClsUtility.GetColumnNumber(Letteracolonna) - 1
                If NumColonna = "-1" Then
                    blnErrFile = True
                    swErr = True
                    Exit For
                End If
                If (ArrCampi.Length - 1) >= NumColonna Then
                    Select Case UCase(CodiceParametro)
                        Case "PRG"
                            'controllo progetto
                            If Trim(ArrCampi(NumColonna)) = vbNullString Then
                                strNote = strNote & "Il campo Codice Programma e' obbligatorio."
                                swErr = True
                            Else
                                CodiceProgramma = Trim(ArrCampi(NumColonna))
                                IDProgramma = TrovaIDProgramma(Trim(ArrCampi(NumColonna)))
                                'verifico esistenza progetto
                                If VerificaEsistenzaProgramma(Trim(ArrCampi(NumColonna))) = False Then
                                    strNote = strNote & "Il Codice Programma indicato e' inesistente."
                                    swErr = True
                                    blnErrProgramma = True
                                Else
                                    'Verifico se il Programma è in attesa di valutazione
                                    If VerificaProgramma(Trim(ArrCampi(NumColonna))) = False Then
                                        strNote = strNote & "Il Programma non e'in attesa di Valutazione."
                                        swErr = True
                                    Else
                                        If ControlloValutazioneProgetti(Trim(ArrCampi(NumColonna))) = False Then
                                            strNote = strNote & "Esistono progetti associati al programma non ancora valutati formalmente."
                                            swErr = True
                                        Else
                                            If ControlloNumeroProgettiAmmissibili(Trim(ArrCampi(NumColonna))) = False Then
                                                strNote = strNote & "Il numero di progetti ammissibili associati al programma è inferiore a 2."
                                                swErr = True
                                            End If
                                        End If
                                    End If
                                End If
                            End If
                        Case "ACC"
                            'controllo progetto POSITITVO O POSITIVO CON LIMITAZIONI
                            If Trim(ArrCampi(NumColonna)) = vbNullString Then
                                strNote = strNote & "Lo Stato Programma e' obbligatorio."
                                swErr = True
                            Else
                                StatoProg = UCase(Trim(ArrCampi(NumColonna)))
                                If StatoProgramma(UCase(Trim(ArrCampi(NumColonna)))) = False Then
                                    strNote = strNote & "Lo Stato Programma non è nel formato corretto."
                                    swErr = True
                                End If
                            End If
                        Case "DEF"
                            'controllo deflettori VEDERE MASCHERA
                            'campo no vuoto ma 0
                            If Trim(ArrCampi(NumColonna)) = vbNullString Then
                                strNote = strNote & "Il Deflettore e' obbligatorio."
                                swErr = True
                            Else
                                Deflettore = Trim(ArrCampi(NumColonna))
                                'campo numerico
                                If InStr(Trim(ArrCampi(NumColonna)), "-") > 0 Then
                                    strNote = strNote & "Il Deflettore non e' nel formato corretto.Indicare valori positivi."
                                    swErr = True
                                Else
                                    If IsNumeric(Trim(ArrCampi(NumColonna))) = False Then
                                        strNote = strNote & "Il Deflettore non e' nel formato corretto."
                                        swErr = True
                                    Else
                                        If Trim(ArrCampi(NumColonna)) > 10 Then
                                            strNote = strNote & "Nel Deflettore e' stato inserito un valore troppo elevato."
                                            swErr = True
                                        End If
                                    End If
                                End If
                            End If
                        Case "REG"
                            'controlli regioni vedere maschera
                            If Trim(ArrCampi(NumColonna)) = vbNullString Then
                                strNote = strNote & "Il Punteggio Regionale e' obbligatorio."
                                swErr = True
                            Else
                                If InStr(Trim(ArrCampi(NumColonna)), "-") > 0 Then
                                    strNote = strNote & "Il Punteggio Regionale non e' nel formato corretto.Indicare valori positivi."
                                    swErr = True
                                Else
                                    If InStr(Trim(ArrCampi(NumColonna)), ".") > 0 Then
                                        strNote = strNote & "Il Punteggio Regionale non e' nel formato corretto. Sostiturire il punto con la virgola."
                                        swErr = True
                                    Else
                                        PuntReg = Trim(ArrCampi(NumColonna).Replace(",", "."))
                                        If IsNumeric(Trim(ArrCampi(NumColonna))) = False Then
                                            strNote = strNote & "Il Punteggio Regionale non e' nel formato corretto."
                                            swErr = True
                                        End If
                                    End If
                                End If
                            End If
                        Case "TOT"
                            'verifica totale punteggi richiamare store controlli totatale
                            If Trim(ArrCampi(NumColonna)) = vbNullString Then
                                strNote = strNote & "Il Totale e' obbligatorio."
                                swErr = True
                            Else
                                If InStr(Trim(ArrCampi(NumColonna)), "-") > 0 Then
                                    strNote = strNote & "Il Totale non e' nel formato corretto.Indicare valori positivi."
                                    swErr = True
                                Else
                                    If InStr(Trim(ArrCampi(NumColonna)), ".") > 0 Then
                                        strNote = strNote & "Il Totale non e' nel formato corretto. Sostiturire il punto con la virgola."
                                        swErr = True
                                    Else
                                        TotValFile = Trim(ArrCampi(NumColonna).Replace(",", "."))
                                        If IsNumeric(Trim(ArrCampi(NumColonna))) = False Then
                                            strNote = strNote & "Il Totale non e' nel formato corretto."
                                            swErr = True
                                        End If
                                    End If
                                End If
                            End If
                        Case Else
                            'controllo i punteggio solamente se il codice progetto indicato è esistente
                            If blnErrProgramma = False Then
                                'controllo dei paramentri di valutazione 
                                If Trim(ArrCampi(NumColonna)) = vbNullString Then
                                    'strNote = strNote & "Il Parametro di Valutazione e' obbligatorio."
                                    'swErr = True
                                Else
                                    If InStr(Trim(ArrCampi(NumColonna)), "-") > 0 Then
                                        strNote = strNote & "Il Parametro di Valutazione non e' nel formato corretto.Indicare valori positivi."
                                        swErr = True
                                    Else
                                        If IsNumeric(Trim(ArrCampi(NumColonna))) = False Then
                                            strNote = strNote & "Il Parametro di Valutazione non e' nel formato corretto."
                                            swErr = True
                                        End If
                                        Dim idVoce As String
                                        idVoce = Sp_VerificaPunteggioParametri(CodiceProgramma, CodiceParametro, Trim(ArrCampi(NumColonna)))
                                        If idVoce = "-1" Then
                                            strNote = strNote & "Punteggio non previsto per il parametro  " & Trim(CodiceParametro) & "."
                                            swErr = True
                                        End If
                                    End If

                                End If
                            End If
                    End Select
                    strSQLDinamica &= "," & " '" & Trim(ArrCampi(NumColonna).Replace(",", ".")) & "'"
                Else
                    blnErrFile = True
                    swErr = True
                End If
            Next
            'controllo compatibilità valutazioni solo se nel controllo della riga del file non sono stati iscontrati errori
            If swErr = False Then
                Dim Esito As String
                Esito = VerificaCongruenzaValutazione(IDProgramma, CodiceProgramma, StatoProg, Deflettore, PuntReg, xLinea, TotValFile)
                If Esito <> "" Then
                    strNote = strNote & Esito
                    swErr = True
                End If
            End If
            If blnErrFile = False Then
                'INSERT NELLA TABELLA TEMPORANEA
                ScriviTabTemp(strNote, strSQLDinamica)
            End If
            If swErr = True Then
                TotKo = TotKo + 1
            End If
            Writer.WriteLine(strNote & ";" & xLinea)
            strNote = ""
            strSQLDinamica = vbNullString
            xLinea = Reader.ReadLine()
        End While
        Reader.Close()
        Writer.Close()

        If blnErrFile = True Then
            CancellaTabellaTemp()
        End If

        Response.Redirect("WfrmRisultatoImportValutazioneProgrammi.aspx?NomeFile=" & NomeUnivoco & "&Tot=" & Tot & "&TotOk=" & Tot - TotKo & "&TotKo=" & TotKo)

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

    Function TrovaIDProgramma(ByVal pCodiceProgramma As String) As String
        'Controllo se il codice progetto indicato esiste
        Dim dtrIDProgramma As SqlClient.SqlDataReader
        Dim strSql As String
        Dim xIdAttivita As Integer

        strSql = "SELECT IdProgramma " & _
                 "FROM Programmi " & _
                 "WHERE CodiceProgramma= '" & pCodiceProgramma & "' "

        dtrIDProgramma = ClsServer.CreaDatareader(strSql, Session("conn"))
        If dtrIDProgramma.HasRows = True Then
            dtrIDProgramma.Read()
            xIdAttivita = dtrIDProgramma("IdProgramma")
        End If
        dtrIDProgramma.Close()
        dtrIDProgramma = Nothing

        Return xIdAttivita
    End Function

    Function VerificaEsistenzaProgramma(ByVal pCodiceProgramma As String) As Boolean
        'Controllo se il codice progetto indicato esiste
        Dim dtrProgetto As SqlClient.SqlDataReader
        Dim strSql As String
        xIdAttivita = 0

        strSql = "SELECT IdProgramma " & _
                 "FROM Programmi " & _
                 "WHERE CodiceProgramma= '" & pCodiceProgramma & "' "

        dtrProgetto = ClsServer.CreaDatareader(strSql, Session("conn"))
        If dtrProgetto.HasRows = True Then
            VerificaEsistenzaProgramma = True
        Else
            VerificaEsistenzaProgramma = False
        End If
        dtrProgetto.Close()
        dtrProgetto = Nothing

        Return VerificaEsistenzaProgramma
    End Function

    Function VerificaProgramma(ByVal pCodiceProgramma As String) As Boolean
        'Controllo se il BANDO E APERTO
        'se lo tsato del progetto è PROPOSTO O IN ATTESA GGRADUATORIA
        'se lo stato dell'istanza è APPROVATA
        Dim dtrProgramma As SqlClient.SqlDataReader
        Dim strSql As String
        xIdAttivita = 0

        strSql = " SELECT Programmi.IdProgramma"
        strSql = strSql & " FROM Programmi "
        strSql = strSql & " INNER JOIN enti on Programmi.identeProponente=enti.idente "
        strSql = strSql & " LEFT JOIN BandiProgrammi  on Programmi.IdBandoProgramma=BandiProgrammi.IdBandoProgramma "
        strSql = strSql & " LEFT JOIN  statiBandiProgrammi on BandiProgrammi.IdStatoBandoProgramma=statiBandiProgrammi.IdStatoBandoProgramma "
        strSql = strSql & " LEFT JOIN bando  on bando.idbando=BandiProgrammi.idbando "
        strSql = strSql & " LEFT JOIN statibando  on bando.idstatobando=statibando.idstatobando "
        strSql = strSql & " INNER JOIN StatiProgrammi  on Programmi.idstatoProgramma=StatiProgrammi.idstatoProgramma "
        strSql = strSql & " WHERE Programmi.codiceprogramma='" & pCodiceProgramma & "' "
        strSql = strSql & " AND statibando.IDStatoBando=1 " ' -- STATO BANDO APERTO
        strSql = strSql & " AND (StatiProgrammi.idstatoProgramma=3) " '-- STATO PROGETTO PROPROSTO / IN ATTESA GRADUATORIA
        strSql = strSql & " AND statiBandiProgrammi.IDStatoBandoProgramma = 3 " '-- STATO ISTANZA APPROVATA

        dtrProgramma = ClsServer.CreaDatareader(strSql, Session("conn"))

        If dtrProgramma.HasRows = True Then
            VerificaProgramma = True
        Else
            VerificaProgramma = False
        End If

        dtrProgramma.Close()
        dtrProgramma = Nothing
        Return VerificaProgramma
    End Function

    Function ControlloValutazioneProgetti(ByVal pCodiceProgramma As String) As Boolean
        'Controllo se esistono progetti legati al programma non valutati formalmente
        Dim dtrProgramma As SqlClient.SqlDataReader
        Dim strSql As String

        strSql = " SELECT Programmi.IdProgramma"
        strSql = strSql & " FROM Programmi "
        strSql = strSql & " left JOIN attività on Programmi.idprogramma=attività.idprogramma "
        strSql = strSql & " WHERE Programmi.codiceprogramma='" & pCodiceProgramma & "' "
        strSql = strSql & " AND attività.IDStatoattività=4 " ' -- STATO PROGETTO PROPOSTO

        dtrProgramma = ClsServer.CreaDatareader(strSql, Session("conn"))

        If dtrProgramma.HasRows = True Then
            'ControlloValutazioneProgetti = False
            ControlloValutazioneProgetti = True 'bypass necessario per urgenza caricamenti 06/11/2020
        Else
            ControlloValutazioneProgetti = True
        End If

        dtrProgramma.Close()
        dtrProgramma = Nothing
        Return ControlloValutazioneProgetti
    End Function

    Function ControlloNumeroProgettiAmmissibili(ByVal pCodiceProgramma As String) As Boolean
        'Controllo se il numero di progetti ammissibili legati al programma sia almeno 2
        Dim dtrProgramma As SqlClient.SqlDataReader
        Dim strSql As String
        Dim nprog As Integer

        nprog = 0

        strSql = " SELECT count(*) AS NPROGETTI"
        strSql = strSql & " FROM Programmi "
        strSql = strSql & " left JOIN attività on Programmi.idprogramma=attività.idprogramma "
        strSql = strSql & " WHERE Programmi.codiceprogramma='" & pCodiceProgramma & "' "
        strSql = strSql & " AND attività.IDStatoattività=9 " ' -- STATO PROGETTO IN ATTESA GRADUATORIA

        dtrProgramma = ClsServer.CreaDatareader(strSql, Session("conn"))
        If dtrProgramma.HasRows = True Then
            dtrProgramma.Read()
            nprog = dtrProgramma("NPROGETTI")
        End If

        If nprog >= 2 Then
            ControlloNumeroProgettiAmmissibili = True
        Else
            ControlloNumeroProgettiAmmissibili = False
        End If


        dtrProgramma.Close()
        dtrProgramma = Nothing

        Return ControlloNumeroProgettiAmmissibili
    End Function

    Public Function StatoProgramma(ByVal StatoProg As String) As Boolean
        'la funzione verifica se il progetto è ACCETTATO: POSITIVO / POSITIVO CON LIMITAZIONE
        'Dim sOcc() As String
        'Dim i As Integer
        Dim Esito As Boolean = False
        'Const cnsStato As String = "POSITIVO,POSITIVOLIMITATO"

        Try
            StatoProg = Replace(StatoProg, " ", "")
            If (StatoProg = "POSITIVO" Or StatoProg = "POSITIVOLIMITATO") Then
                Esito = True
            End If

            Return Esito
        Catch ex As Exception
            Esito = "Errore imprevisto: " & ex.Message
        End Try
    End Function

    Private Function Sp_VerificaPunteggioParametri(ByVal CodiceProgramma As String, ByVal CodiceParametro As String, ByVal Punteggio As Integer) As String
        Dim sqlCMD As New SqlClient.SqlCommand
        Dim strNomeStore As String = "[SP_IMPORTVALUTAZIONI_VERIFICAPUNTEGGIOPARAMETRO_PROGRAMMI]"
        Dim dataTB As New DataTable

        Try
            sqlCMD = New SqlClient.SqlCommand(strNomeStore, CType(Session("conn"), SqlClient.SqlConnection))
            sqlCMD.CommandType = CommandType.StoredProcedure
            sqlCMD.Parameters.Add("@CODICEPROGRAMMA", SqlDbType.VarChar).Value = CodiceProgramma
            sqlCMD.Parameters.Add("@CODICEPARAMETRO", SqlDbType.VarChar).Value = CodiceParametro
            sqlCMD.Parameters.Add("@PUNTEGGIO", SqlDbType.Int).Value = Punteggio


            Dim sparam As SqlClient.SqlParameter
            sparam = New SqlClient.SqlParameter
            sparam.ParameterName = "@IDVOCE"
            sparam.Size = 10
            sparam.SqlDbType = SqlDbType.Int
            sparam.Direction = ParameterDirection.Output
            sqlCMD.Parameters.Add(sparam)

            sqlCMD.ExecuteScalar()
            Dim str As String
            str = sqlCMD.Parameters("@IDVOCE").Value
            Return str

        Catch ex As Exception
            Response.Write(ex.Message.ToString())
            Exit Function
        End Try
    End Function

    Private Function VerificaCongruenzaValutazione(ByVal Idprogramma As String, ByVal CodiceProgramma As String, ByVal StatoProg As String, ByVal Deflettore As String, ByVal PuntReg As String, ByVal xLinea As String, ByVal TotaleVal As String) As String
        Dim intIDStoricoTMP As Integer
        Dim StrEsito As String
        'Creata da simona Cordella il 15/02/2013
        'la funzione verifica che siano state Valutate correttamente tutte le domande

        'richiamo funzione che inserisce in tabelle temporanee
        intIDStoricoTMP = InsertTMP(Session("Utente"), CodiceProgramma, Idprogramma, StatoProg, Deflettore, PuntReg, xLinea)
        'richiamo store
        StrEsito = StoreVerificaValutazioneQual(intIDStoricoTMP)
        If StrEsito = "" Then
            'confronto TOT con la store COMPUTESCORE_IMPORT
            StrEsito = StoreVerificaTotaleValutazione(intIDStoricoTMP, TotaleVal)
            If UCase(StrEsito) = "OK" Then
                StrEsito = ""
            End If
        End If
        Return StrEsito
    End Function

    Private Sub ScriviTabTemp(ByVal strNote As String, ByVal strSQLDinamica As String)
        '--- scrive nella tab temporanea
        Dim cmdTemp As SqlClient.SqlCommand
        Dim strsql As String

        Try

            strsql = "INSERT INTO [#TEMP_VALUTAZIONE_PROGRAMMI] VALUES("
            strsql &= " '" & strNote.Replace("'", "''") & "' "
            strsql &= strSQLDinamica
            strsql &= ")"

            cmdTemp = New SqlClient.SqlCommand
            cmdTemp.CommandText = strsql
            cmdTemp.Connection = Session("conn")
            cmdTemp.ExecuteNonQuery()
            cmdTemp.Dispose()
            'TotOk = TotOk + 1

        Catch exc As Exception
            strNote = "Errore generico."
            'TotKo = TotKo + 1
        End Try
    End Sub

    Private Function InsertTMP(ByVal UserName As String, ByVal CodiceProgramma As String, ByVal Idprogramma As String, ByVal StatoProg As String, ByVal Deflettore As String, ByVal PuntReg As String, ByVal xlinea As String) As Integer
        'Creata da simona Cordella il 09/12/2009
        Dim strsql As String
        Dim comGenerico As SqlClient.SqlCommand
        Dim dtrID As SqlClient.SqlDataReader
        Dim idStoricoTMP As String

        ''Inserimento(StoricoProgetti)
        ',notestorico


        strsql = "Insert into storicoprogrammiTMPImport (Idprogramma,dataInserimento," & _
                " statoprogramma,Username,notestorico )values " & _
                "(" & Idprogramma & ",getdate(),3, '" & UserName & "', '')"

        comGenerico = ClsServer.EseguiSqlClient(strsql, Session("conn"))
        strsql = "Select SCOPE_IDENTITY() as maxid from storicoprogrammiTMPImport"
        dtrID = ClsServer.CreaDatareader(strsql, Session("conn"))
        dtrID.Read()
        If dtrID.HasRows = True Then
            idStoricoTMP = dtrID("maxid")
        End If

        dtrID.Close()
        dtrID = Nothing
        'salvo nel campo AltriEnti della tabella storicoprogettiTMPImport IDStoricoTMP che servirà per la conferma dell' importazione della valutazione(wfrmRisultatoImportValutazioneProgetti)
        strsql = "UPDATE  storicoprogrammiTMPImport set identificativoTMP=" & idStoricoTMP & " where idStoricoTMP=" & idStoricoTMP
        comGenerico = ClsServer.EseguiSqlClient(strsql, Session("conn"))


        Dim i As Integer
        Dim Letteracolonna As String
        Dim NumColonna As Integer
        Dim ArrCampi() As String
        Dim CodiceParametro As String
        Dim Limitato As Integer = 0 '0: positivo; 1: positivio Limitato
        ArrCampi = CreaArray(xlinea)

        For i = 0 To dtParametri.Rows.Count - 1
            CodiceParametro = dtParametri.Rows(i).Item(1)
            Letteracolonna = dtParametri.Rows(i).Item(6) 'dtParametri.Tables(0).Rows(i).Item(6)
            If Letteracolonna <> "" Then
                NumColonna = ClsUtility.GetColumnNumber(Letteracolonna) - 1
                Select Case UCase(CodiceParametro)
                    Case "PRG", "DEF", "REG", "TOT"
                    Case "ACC" 'progetto positivo o positivo con limitazioni
                        'controllo progetto POSITITVO O POSITIVO CON LIMITAZIONI
                        StatoProg = UCase(Trim(ArrCampi(NumColonna)))
                        If (StatoProg = "POSITIVO") Then
                            Limitato = 0
                        Else
                            Limitato = 1
                        End If
                        'salvo nel campo Gruppo della tabella storicoprogettiTMPImport Limitato SI/NO (0/1) che servirà per la conferma dell' importazione della valutazione(wfrmRisultatoImportValutazioneProgetti)
                        strsql = "UPDATE  storicoprogrammiTMPImport set Gruppo=" & Limitato & " where idStoricoTMP=" & idStoricoTMP
                        comGenerico = ClsServer.EseguiSqlClient(strsql, Session("conn"))
                        'verifica totale punteggi richiamare store controlli totatale
                    Case Else
                        Dim idVoce As String
                        If Trim(ArrCampi(NumColonna)) <> "" Then
                            idVoce = Sp_VerificaPunteggioParametri(CodiceProgramma, CodiceParametro, Trim(ArrCampi(NumColonna)))
                            If idVoce <> "-1" Then
                                strsql = "insert into storicovociProgrammiTMPImport(idStoricoTMP,idvoceTMP,Punteggio) " & _
                                " VALUES (" & idStoricoTMP & "," & idVoce & "," & Trim(ArrCampi(NumColonna)) & " )"
                                comGenerico = ClsServer.EseguiSqlClient(strsql, Session("conn"))
                            End If
                        End If
                End Select
            End If
        Next

        Return idStoricoTMP
    End Function

    Private Function StoreVerificaValutazioneQual(ByVal IdStoricoTMP As Integer) As String
        'Agg. da Simona Cordella il 15/02/2013
        'richiamo store che verifca  se ci sono anomalie sul progetto durante l'istnza di presentazione
        Dim intValore As Integer
        Dim CustOrderHist As SqlClient.SqlCommand

        CustOrderHist = New SqlClient.SqlCommand
        CustOrderHist.CommandType = CommandType.StoredProcedure
        CustOrderHist.CommandText = "SP_VERIFICA_VALUTAZIONE_QUAL_IMPORT_PROGRAMMI"
        CustOrderHist.Connection = IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn"))

        Dim sparam As SqlClient.SqlParameter
        sparam = New SqlClient.SqlParameter
        sparam.ParameterName = "@IdStoricoTMP"
        sparam.SqlDbType = SqlDbType.Int
        CustOrderHist.Parameters.Add(sparam)


        Dim sparam1 As SqlClient.SqlParameter
        sparam1 = New SqlClient.SqlParameter
        sparam1.ParameterName = "@Esito"
        sparam1.SqlDbType = SqlDbType.Int
        sparam1.Direction = ParameterDirection.Output
        CustOrderHist.Parameters.Add(sparam1)

        Dim sparam2 As SqlClient.SqlParameter
        sparam2 = New SqlClient.SqlParameter
        sparam2.ParameterName = "@Motivazione"
        sparam2.SqlDbType = SqlDbType.VarChar
        sparam2.Size = 1000
        sparam2.Direction = ParameterDirection.Output
        CustOrderHist.Parameters.Add(sparam2)

        Dim Reader As SqlClient.SqlDataReader
        CustOrderHist.Parameters("@IdStoricoTMP").Value = IdStoricoTMP
        Reader = CustOrderHist.ExecuteReader()
        'CustOrderHist.ExecuteNonQuery()
        ' Insert code to read through the datareader.
        intValore = CustOrderHist.Parameters("@Esito").Value
        If intValore <> 2 Then
            StoreVerificaValutazioneQual = CustOrderHist.Parameters("@Motivazione").Value
        Else
            StoreVerificaValutazioneQual = ""
        End If
        If Not Reader Is Nothing Then
            Reader.Close()
            Reader = Nothing
        End If
    End Function

    Private Function StoreVerificaTotaleValutazione(ByVal IdStoricoTMP As Integer, ByVal Totale As String) As String
        'Agg. da Simona Cordella il 20/02/2013
        Dim sqlCMD As New SqlClient.SqlCommand
        Dim strNomeStore As String = "[computeScoreProgrammi_Import]"
        Dim dataTB As New DataTable

        Try
            sqlCMD = New SqlClient.SqlCommand(strNomeStore, CType(Session("conn"), SqlClient.SqlConnection))
            sqlCMD.CommandType = CommandType.StoredProcedure
            sqlCMD.Parameters.Add("@idstorico", SqlDbType.Int).Value = IdStoricoTMP
            sqlCMD.Parameters.Add("@totaleprevisto", SqlDbType.NVarChar).Value = Totale


            Dim sparam As SqlClient.SqlParameter
            sparam = New SqlClient.SqlParameter
            sparam.ParameterName = "@Esito"
            sparam.Size = 255
            sparam.SqlDbType = SqlDbType.NVarChar
            sparam.Direction = ParameterDirection.Output
            sqlCMD.Parameters.Add(sparam)

            sqlCMD.ExecuteScalar()
            Dim str As String
            str = sqlCMD.Parameters("@Esito").Value
            Return str

        Catch ex As Exception
            Response.Write(ex.Message.ToString())
            Exit Function
        End Try
    End Function

End Class