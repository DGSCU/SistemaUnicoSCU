Imports System.IO
Imports System.Text.RegularExpressions
Imports System.Data.SqlClient
Public Class WfrmImportPresenzeVolontari
    Inherits System.Web.UI.Page
    Private Writer As StreamWriter
    Private strNote As String
    Private Tot As Integer
    Private TotOk As Integer
    Private TotKo As Integer
    Private NomeUnivoco As String
    Private xIdAttivita As Integer
    Dim dtrgenerico As SqlClient.SqlDataReader
    Dim strsql As String
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
                NomeUnivoco = "presenzevolontari" & Session("IdEnte") & "_" & Session("Utente") & "_" & Year(Now) & Month(Now) & Day(Now) & Hour(Now) & Minute(Now) & Second(Now)


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

                CancellaTabellaTemp()
            End Try

            If swErr = False Then
                LeggiCSV()


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
            strSql = "DROP TABLE [#TEMP_PRESENZE_VOLONTARI]"

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
        strSql = "CREATE TABLE [#TEMP_PRESENZE_VOLONTARI] (" & _
                 "[CodiceVolontario] [nvarchar] (15) COLLATE DATABASE_DEFAULT, " & _
                 "[Cognome] [nvarchar] (100) COLLATE DATABASE_DEFAULT, " & _
                 "[Nome] [nvarchar] (100) COLLATE DATABASE_DEFAULT, " & _
                 "[Giorno] [datetime], " & _
                 "[Causale]  [nvarchar] (100) COLLATE DATABASE_DEFAULT , " & _
                 "[Note] [nvarchar] (500) COLLATE DATABASE_DEFAULT, " & _
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

        Dim intMaxGiorniMalattia As Integer
        Dim intMaxGiorniPermesso As Integer
        Dim intMaxGiorniQuarantenaCovid As Integer = 30
        Dim intMaxGiorniVaccinoCovid As Integer = 2
        Dim intMaxGiorni104 As Integer = 3
        Dim intGiorniMalattia As Integer = 0
        Dim intGiorniPermesso As Integer = 0
        Dim intGiorniQuarantenaCovid As Integer = 0
        Dim intGiorniVaccinoCovid As Integer = 0
        Dim intGiorni104 As Integer = 0
        ' Dim AppoNote As String

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

            If UBound(ArrCampi) < 5 Then
                '--- se i campi non sono tutti errore
                strNote = "Il numero delle colonne inserite è minore di quello richieste."
                swErr = True
                TotKo = TotKo + 1
            Else
                If UBound(ArrCampi) > 5 Then
                    '--- se i campi sono troppi errore
                    strNote = "Il numero delle colonne inserite è maggiore di quello richieste."
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
                    If VerificaVolontario(ArrCampi(0), ArrCampi(1), ArrCampi(2)) = False Then
                        strNote = strNote & "Il Volontario indicato non è dell'Ente."
                        swErr = True
                    End If
                    'Mese
                    'If Trim(ArrCampi(3)) = vbNullString Then
                    '    strNote = strNote & "Il campo Mese e' un campo obbligatorio."
                    '    swErr = True
                    'Else
                    '    If IsNumeric(ArrCampi(3)) = False Or Len(ArrCampi(3)) > 2 Or CInt(ArrCampi(3)) < 1 Or CInt(ArrCampi(3)) > 12 Then
                    '        strNote = strNote & "Il campo Mese non e' nel formato corretto."
                    '        swErr = True
                    '    End If
                    'End If
                    ''Anno
                    'If Trim(ArrCampi(4)) = vbNullString Then
                    '    strNote = strNote & "Il campo Anno e' un campo obbligatorio."
                    '    swErr = True
                    'Else
                    '    If IsNumeric(ArrCampi(4)) = False Or Len(ArrCampi(4)) > 4 Then
                    '        strNote = strNote & "Il campo Anno non e' nel formato corretto."
                    '        swErr = True
                    '    End If
                    'End If
                    'verifico l'esistenza del record di quell'anno e di quel mese per quel volontario
                    'qui dovrebbe andare il controllo del mese, alemno credo
                    '***********************************************************************
                    Dim dtrLeggiDati As SqlClient.SqlDataReader
                    Dim strsql As String

                    'chiudo il datareader
                    If Not dtrLeggiDati Is Nothing Then
                        dtrLeggiDati.Close()
                        dtrLeggiDati = Nothing
                    End If
                    '***********************************************************************

                    strsql = "select Mese, Anno from EntiConfermaPresenze where (Anno=" & Mid(ArrCampi(3), 7, 4) & ") and IdEnte='" & Session("IdEnte") & "' and (Mese=" & Mid(ArrCampi(3), 4, 2) & ") "
                    dtrLeggiDati = ClsServer.CreaDatareader(strsql, Session("conn"))

                    If dtrLeggiDati.HasRows = True Then
                        strNote = strNote & "Il periodo di riferimento risulta essere gia' inserito e confermato."
                        swErr = True
                    End If

                    'chiudo il datareader
                    If Not dtrLeggiDati Is Nothing Then
                        dtrLeggiDati.Close()
                        dtrLeggiDati = Nothing
                    End If

                    ''verifico la causale per volontario anno mese
                    'strsql = "select EntitàPresenze.identità, EntitàPresenze.idcausalePresenza, EntitàPresenze.Giorno from EntitàPresenze "
                    'strsql = strsql & "inner join entità on EntitàPresenze.identità=entità.identità "
                    'strsql = strsql & "where EntitàPresenze.idcausalePresenza=" & Trim(ArrCampi(4)) & " and EntitàPresenze.Giorno='" & Trim(ArrCampi(3)) & "' and entità.codicevolontario='" & Trim(ArrCampi(0)) & "' "

                    'dtrLeggiDati = ClsServer.CreaDatareader(strsql, Session("conn"))

                    'If dtrLeggiDati.HasRows = True Then
                    '    strNote = strNote & "La causale di riferimento risulta essere gia' esistente in banca dati per il periodo."
                    '    swErr = True
                    'End If

                    ''chiudo il datareader
                    'If Not dtrLeggiDati Is Nothing Then
                    '    dtrLeggiDati.Close()
                    '    dtrLeggiDati = Nothing
                    'End If

                    'VerificaVolontario
                    If VerificaAnnoMese(ArrCampi(0), ArrCampi(3)) = False Then
                        strNote = strNote & "Il periodo di riferimento della presenza/assenza deve essere compresa tra le date di inizio e fine servizio."
                        swErr = True
                    End If
                    'Giorni
                    If Trim(ArrCampi(3)) = vbNullString Then
                        strNote = strNote & "Il campo Giorno e' un campo obbligatorio."
                        swErr = True
                    Else
                        'migliorare il controllo sul formato della data ANTONELLO
                        'If IsNumeric(ArrCampi(3)) = False Or Len(ArrCampi(3)) > 10 Then
                        '    strNote = strNote & "Il campo Giorni non e' nel formato corretto."
                        '    swErr = True

                        'End

                        Dim data As Date 'ANTONELLO
                        If (Trim(ArrCampi(3)) <> String.Empty) Then
                            If Date.TryParse(Trim(ArrCampi(3)), data) = False Then
                                strNote = strNote & "Il campo Giorno contiene una data non valida. Immettere la data nel formato gg/mm/aaaa."
                                swErr = True
                            End If
                        End If

                        'Causale
                        If Trim(ArrCampi(4)) = vbNullString Then
                            strNote = strNote & "Il campo Causale e' un campo obbligatorio."
                            swErr = True
                        Else
                            'verifica causale decr abrr
                            If VerificaCausaleAbbr(Trim(ArrCampi(4))) = False Then
                                strNote = strNote & "La Causale inserita non è presente nella base dati."
                                swErr = True
                            End If
                        End If
                        If UCase(Trim(ArrCampi(4))) = "P" And (Date.Parse(ArrCampi(3)) > Today) Then
                            strNote = strNote & "Il giorno di presenza selezionato è successivo alla data odierna."
                            swErr = True
                        End If

                        'nuovi controlli per presenze v2
                        If swErr = False Then
                            If UCase(Trim(ArrCampi(4))) = "POR" Or UCase(Trim(ArrCampi(4))) = "MAL" Or UCase(Trim(ArrCampi(4))) = "QCOVID" Or UCase(Trim(ArrCampi(4))) = "VCOVID" Then
                                RecuperaMaxMalattiePermessi(ArrCampi(0), intMaxGiorniMalattia, intMaxGiorniPermesso)
                                RecuperaMalattiePermessi(ArrCampi(0), intGiorniMalattia, intGiorniPermesso, intGiorniQuarantenaCovid, intGiorniVaccinoCovid)


                                'CONTROLLO MAL
                                If UCase(Trim(ArrCampi(4))) = "MAL" And intGiorniMalattia >= intMaxGiorniMalattia Then
                                    strNote = strNote & "Il volontario ha superato il numero massimo di assenze per malattia."
                                    swErr = True
                                End If

                                'CONTROLLO POR
                                If UCase(Trim(ArrCampi(4))) = "POR" And intGiorniPermesso >= intMaxGiorniPermesso Then
                                    strNote = strNote & "Il volontario ha superato il numero massimo di assenze per permesso ordinario retribuito."
                                    swErr = True
                                End If

                                'CONTROLLO QCOVID
                                If UCase(Trim(ArrCampi(4))) = "QCOVID" And intGiorniQuarantenaCovid >= intMaxGiorniQuarantenaCovid Then
                                    strNote = strNote & "Il volontario ha superato il numero massimo di assenze per malattia straordinaria quarantena COVID."
                                    swErr = True
                                End If

                                'CONTROLLO VCOVID
                                If UCase(Trim(ArrCampi(4))) = "VCOVID" And intGiorniVaccinoCovid >= intMaxGiorniVaccinoCovid Then
                                    strNote = strNote & "Il volontario ha superato il numero massimo di assenze per vaccino COVID."
                                    swErr = True
                                End If

                            End If
                            'CONTROLLO 104

                            If UCase(Trim(ArrCampi(4))) = "P104" Then
                                Recupera104(ArrCampi(0), ArrCampi(3), intGiorni104)
                                If intGiorni104 >= intMaxGiorni104 Then
                                    strNote = strNote & "Il volontario ha superato il numero massimo di assenze mensili per Permesso 104/92."
                                    swErr = True
                                End If
                            End If

                        End If

                        'Note non obbligatorio
                        If Trim(ArrCampi(5)) <> vbNullString Then
                            If Len(ArrCampi(5)) > 500 Then
                                strNote = strNote & "Il campo Note puo' contenere massimo 500 caratteri."
                                swErr = True
                            End If
                        End If

                        If swErr = False Then
                            ScriviTabTemp(ArrCampi)
                        Else
                            TotKo = TotKo + 1
                        End If


                    End If
                End If
            End If
            Writer.WriteLine(strNote & ";" & xLinea)
            strNote = vbNullString
            xLinea = Reader.ReadLine()
        End While

        Reader.Close()
        Writer.Close()
        Response.Redirect("WfrmRisultatoImportPresenze.aspx?NomeFile=" & NomeUnivoco & "&Tot=" & Tot & "&TotOk=" & TotOk & "&TotKo=" & TotKo)
        '--- reindirizzo la pagina sottostante
        'Response.Write("<script>" & vbCrLf)
        'Response.Write("parent.Naviga('WfrmRisultatoImportAssenze.aspx?NomeFile=" & NomeUnivoco & "&Tot=" & Tot & "&TotOk=" & TotOk & "&TotKo=" & TotKo & "')" & vbCrLf)
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

    Private Sub ScriviTabTemp(ByVal pArray() As String)
        '--- scrive nella tab temporanea
        Dim cmdTemp As SqlClient.SqlCommand
        Dim strsql As String

        Try
            strsql = "INSERT INTO #TEMP_PRESENZE_VOLONTARI " & _
                     "(CodiceVolontario, " & _
                     "Cognome, " & _
                     "Nome, " & _
                     "Giorno, " & _
                     "Causale, " & _
                     "Note" & _
                     ") " & _
                     "values " & _
                     "('" & Trim(ClsServer.NoApice(pArray(0))) & "', " & _
                     "'" & Trim(ClsServer.NoApice(pArray(1))) & "', " & _
                     "'" & Trim(ClsServer.NoApice(pArray(2))) & "', '" & _
                     Trim(pArray(3)) & "', '" & _
                     Trim(pArray(4)) & "'," & _
                     "'" & Trim(ClsServer.NoApice(pArray(5))) & "')"

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

    Private Function VerificaVolontario(ByVal pCodiceVolontario As String, ByVal pCognomeVolontario As String, ByVal pNomeVolontario As String) As Boolean
        Dim dtrVolontario As SqlClient.SqlDataReader
        Dim strSql As String


        strSql = "SELECT Entità.IdEntità FROM Entità " & _
                 "INNER JOIN AttivitàEntità ON Entità.IdEntità = AttivitàEntità.IdEntità " & _
                 "INNER JOIN AttivitàEntiSediAttuazione ON AttivitàEntità.IdAttivitàEnteSedeAttuazione = AttivitàEntiSediAttuazione.IdAttivitàEnteSedeAttuazione " & _
                 "INNER JOIN Attività ON AttivitàEntiSediAttuazione.IdAttività = Attività.IdAttività " & _
                 "INNER JOIN Enti ON Attività.IdEntePresentante = Enti.IdEnte " & _
                 " INNER JOIN TipiProgetto ON TipiProgetto.IdTipoProgetto = Attività.IdTipoProgetto " & _
                 "WHERE Entità.CodiceVolontario = '" & Trim(ClsServer.NoApice(pCodiceVolontario)) & "' AND " & _
                 "Entità.Cognome = '" & Trim(ClsServer.NoApice(pCognomeVolontario)) & "' AND " & _
                 "Entità.Nome = '" & Trim(ClsServer.NoApice(pNomeVolontario)) & "' AND " & _
                 " Enti.IdEnte = " & Session("IdEnte") & " " & _
                 " AND TipiProgetto.MacroTipoProgetto like '" & Session("FiltroVisibilita") & "' AND TipiProgetto.MacroTipoProgetto ='GG' "



        dtrVolontario = ClsServer.CreaDatareader(strSql, Session("conn"))
        VerificaVolontario = dtrVolontario.HasRows

        dtrVolontario.Close()
        dtrVolontario = Nothing

    End Function

    Private Function VerificaAnnoMese(ByVal pCodiceVolontario As String, ByVal giorno As String) As Boolean
        Dim dtrData As SqlClient.SqlDataReader
        Dim strSql As String
        Dim DataInizio As Date
        Dim DataFine As Date
        Dim DataPresenza As Date

        VerificaAnnoMese = True

        Try
            DataPresenza = giorno
        Catch ex As Exception
            VerificaAnnoMese = False
            Exit Function
        End Try
        DataPresenza = giorno

        strSql = "SELECT DataInizioServizio, DataFineServizio FROM Entità " & _
                 "WHERE CodiceVolontario = '" & pCodiceVolontario & "'"

        dtrData = ClsServer.CreaDatareader(strSql, Session("conn"))

        If dtrData.HasRows = True Then
            dtrData.Read()

            DataInizio = dtrData("DataInizioServizio")
            DataFine = dtrData("DataFineServizio")

            If DataPresenza < DataInizio Or DataPresenza > DataFine Then
                VerificaAnnoMese = False
            End If
        Else
            VerificaAnnoMese = False
        End If

        dtrData.Close()
        dtrData = Nothing

    End Function


    Private Function VerificaGiorni(ByVal intAnno As Int16, ByVal intMese As Int16, ByVal intGiorni As Integer) As Boolean
        If intGiorni > DateTime.DaysInMonth(intAnno, intMese) Then
            VerificaGiorni = False
        Else
            VerificaGiorni = True
        End If
    End Function

    'Private Function GiorniDisponibili(ByVal CodVolontario As String, ByVal Giorno As String, ByVal IdCasuale As Integer) As Integer
    '    Dim Anno As String
    '    Dim Mese As String

    '    'Verifica i giorni diposnibile nel mese per l'inserimento delle assenze

    '    Dim GiorniTotaleMese As Integer
    '    Dim DataInizioSer As String
    '    Dim DataFineSer As String

    '    Mese = Mid(Giorno, 4, 2)
    '    Anno = Mid(Giorno, 7, 4)
    '    ' Dim GiorniTotMeseDisponibili As Integer

    '    If Not dtrgenerico Is Nothing Then
    '        dtrgenerico.Close()
    '        dtrgenerico = Nothing
    '    End If


    '    'giorni totali del mese
    '    GiorniTotaleMese = DateTime.DaysInMonth(CInt(Anno), CInt(Mese))
    '    GiorniDisponibili = GiorniTotaleMese
    '    strsql = " SELECT dbo.formatodata(DataInizioServizio) as DataInizioServizio, " & _
    '             " dbo.formatodata(DatafineServizio) as DatafineServizio " & _
    '             " FROM entità " & _
    '             " WHERE CodiceVolontario = '" & CodVolontario & "'"
    '    dtrgenerico = ClsServer.CreaDatareader(strsql, Session("conn"))

    '    If dtrgenerico.HasRows = True Then
    '        dtrgenerico.Read()
    '        DataInizioSer = dtrgenerico("DataInizioServizio")
    '        DataFineSer = dtrgenerico("DatafineServizio")

    '        'controllo se il mese di inzio servizio corrisponde al mese delle assenze
    '        If Month(DataInizioSer) = Mese And Year(DataInizioSer) = Anno Then
    '            GiorniDisponibili = (GiorniTotaleMese - CInt(Day(DataInizioSer))) + 1
    '        End If
    '        'controllo se il mese di fine servizio corrisponde al mese delle assenze
    '        If Month(DataFineSer) = Mese And Year(DataFineSer) = Anno Then
    '            GiorniDisponibili = GiorniDisponibili - (GiorniTotaleMese - CInt(Day(DataFineSer)))
    '        End If
    '    End If

    '    If Not dtrgenerico Is Nothing Then
    '        dtrgenerico.Close()
    '        dtrgenerico = Nothing
    '    End If


    '    strsql = "SELECT ISNULL(Sum(a.Giorni),0) as GiorniTotAttuali "
    '    strsql = strsql & "FROM EntitàPresenza a "
    '    strsql = strsql & "inner join entità  b on a.idEntità=b.idEntità "
    '    strsql = strsql & "where a.IdCausale <> " & IdCasuale & " and b.CodiceVolontario='" & CodVolontario & "' and  a.Mese=" & Mese & " and  a.anno=" & Anno & " and a.stato in(1,2)  "
    '    dtrgenerico = ClsServer.CreaDatareader(strsql, Session("conn"))
    '    If dtrgenerico.HasRows = True Then
    '        dtrgenerico.Read()
    '        GiorniDisponibili = GiorniDisponibili - CInt(dtrgenerico("GiorniTotAttuali"))
    '    End If

    '    If Not dtrgenerico Is Nothing Then
    '        dtrgenerico.Close()
    '        dtrgenerico = Nothing
    '    End If

    '    Return GiorniDisponibili
    'End Function
    Private Function VerificaCausaleAbbr(ByVal descrabbr As String) As Boolean
        Dim dtrCausaleAbbr As SqlClient.SqlDataReader
        Dim strSql As String


        strSql = "SELECT codice from CausaliPresenze where codice='" & descrabbr & "'"
        dtrCausaleAbbr = ClsServer.CreaDatareader(strSql, Session("conn"))

        If dtrCausaleAbbr.HasRows Then
            dtrCausaleAbbr.Close()
            dtrCausaleAbbr = Nothing
            Return True
        Else
            dtrCausaleAbbr.Close()
            dtrCausaleAbbr = Nothing
            Return False
        End If


    End Function

    Private Sub RecuperaMaxMalattiePermessi(ByVal strcodicevolontario As String, ByRef intMalattie As Integer, ByRef intPermessi As Integer)
        Dim dtrMalattiePermessi As SqlClient.SqlDataReader
        Dim strSql As String


        strSql = "select 15+[dbo].[CalcoloGiorniMalattia_SCU] (DataInizioServizio,DataInizioAttività,entità.IdEntità) as MaxGiorniMalattia"
        strSql = strSql & ", [dbo].[CalcoloGiorniPermesso_SCU] (DataInizioServizio,DataInizioAttività,entità.IdEntità) as MaxGiorniPermesso"
        strSql = strSql & " from entità"
        strSql = strSql & " INNER JOIN graduatorieentità on graduatorieentità.identità = entità.identità "
        strSql = strSql & " INNER JOIN attivitàsediassegnazione on attivitàsediassegnazione.idattivitàsedeassegnazione = graduatorieentità.idattivitàsedeassegnazione "
        strSql = strSql & " inner join attività on attività.idattività = attivitàsediassegnazione.idattività "
        strSql = strSql & " where entità.codicevolontario='" & strcodicevolontario & "'"
        dtrMalattiePermessi = ClsServer.CreaDatareader(strSql, Session("conn"))

        If dtrMalattiePermessi.HasRows Then
            dtrMalattiePermessi.Read()
            intMalattie = dtrMalattiePermessi("MaxGiorniMalattia")
            intPermessi = dtrMalattiePermessi("MaxGiorniPermesso")
        Else
            intPermessi = 0
            intMalattie = 0
        End If
        If Not dtrMalattiePermessi Is Nothing Then
            dtrMalattiePermessi.Close()
            dtrMalattiePermessi = Nothing
        End If

    End Sub

    Private Sub RecuperaMalattiePermessi(ByVal strcodicevolontario As String, ByRef intMalattie As Integer, ByRef intPermessi As Integer, ByRef intQuarantenaCovid As Integer, ByRef intVaccinoCovid As Integer)
        Dim dtrMalattiePermessi As SqlClient.SqlDataReader
        Dim strSql As String

        strSql = "SELECT COUNT(distinct mal.IDEntitàPresenza) as Malattie, COUNT(distinct per.IDEntitàPresenza) as Permessi, COUNT(distinct qcov.IDEntitàPresenza) as QuarantenaCovid, COUNT(distinct vcov.IDEntitàPresenza) as VaccinoCovid "
        strSql = strSql & " FROM entità "
        strSql = strSql & " left JOIN entitàpresenze mal on mal.identità = entità.identità and mal.IdCausalePresenza = 4 "
        strSql = strSql & " left join entitàpresenze per on per.identità = entità.identità and per.IdCausalePresenza = 3 "
        strSql = strSql & " left join entitàpresenze qcov on qcov.identità = entità.identità and qcov.IdCausalePresenza = 15 "
        strSql = strSql & " left join entitàpresenze vcov on vcov.identità = entità.identità and vcov.IdCausalePresenza = 16 "
        strSql = strSql & " where entità.codicevolontario = '" & strcodicevolontario.Replace("'", "''") & "'"
        dtrMalattiePermessi = ClsServer.CreaDatareader(strSql, Session("conn"))

        If dtrMalattiePermessi.HasRows Then
            dtrMalattiePermessi.Read()
            intMalattie = dtrMalattiePermessi("Malattie")
            intPermessi = dtrMalattiePermessi("Permessi")
            intQuarantenaCovid = dtrMalattiePermessi("QuarantenaCovid")
            intVaccinoCovid = dtrMalattiePermessi("VaccinoCovid")
        Else
            intPermessi = 0
            intMalattie = 0
            intQuarantenaCovid = 0
            intVaccinoCovid = 0
        End If

        If Not dtrMalattiePermessi Is Nothing Then
            dtrMalattiePermessi.Close()
            dtrMalattiePermessi = Nothing
        End If

        strSql = "SELECT COUNT(*) as MalattieTMP FROM #TEMP_PRESENZE_VOLONTARI "
        strSql = strSql & " where causale = 'MAL' and codicevolontario = '" & strcodicevolontario.Replace("'", "''") & "'"
        dtrMalattiePermessi = ClsServer.CreaDatareader(strSql, Session("conn"))

        If dtrMalattiePermessi.HasRows Then
            dtrMalattiePermessi.Read()
            intMalattie = intMalattie + dtrMalattiePermessi("MalattieTMP")
        End If

        If Not dtrMalattiePermessi Is Nothing Then
            dtrMalattiePermessi.Close()
            dtrMalattiePermessi = Nothing
        End If

        strSql = "SELECT COUNT(*) as PermessiTMP FROM #TEMP_PRESENZE_VOLONTARI "
        strSql = strSql & " where causale = 'POR' and codicevolontario = '" & strcodicevolontario.Replace("'", "''") & "'"
        dtrMalattiePermessi = ClsServer.CreaDatareader(strSql, Session("conn"))

        If dtrMalattiePermessi.HasRows Then
            dtrMalattiePermessi.Read()
            intPermessi = intPermessi + dtrMalattiePermessi("PermessiTMP")
        End If

        If Not dtrMalattiePermessi Is Nothing Then
            dtrMalattiePermessi.Close()
            dtrMalattiePermessi = Nothing
        End If

        strSql = "SELECT COUNT(*) as QuarantenaCovidTMP FROM #TEMP_PRESENZE_VOLONTARI "
        strSql = strSql & " where causale = 'QCOVID' and codicevolontario = '" & strcodicevolontario.Replace("'", "''") & "'"
        dtrMalattiePermessi = ClsServer.CreaDatareader(strSql, Session("conn"))

        If dtrMalattiePermessi.HasRows Then
            dtrMalattiePermessi.Read()
            intQuarantenaCovid = intQuarantenaCovid + dtrMalattiePermessi("QuarantenaCovidTMP")
        End If

        If Not dtrMalattiePermessi Is Nothing Then
            dtrMalattiePermessi.Close()
            dtrMalattiePermessi = Nothing
        End If

        strSql = "SELECT COUNT(*) as VaccinoCovidTMP FROM #TEMP_PRESENZE_VOLONTARI "
        strSql = strSql & " where causale = 'VCOVID' and codicevolontario = '" & strcodicevolontario.Replace("'", "''") & "'"
        dtrMalattiePermessi = ClsServer.CreaDatareader(strSql, Session("conn"))

        If dtrMalattiePermessi.HasRows Then
            dtrMalattiePermessi.Read()
            intVaccinoCovid = intVaccinoCovid + dtrMalattiePermessi("VaccinoCovidTMP")
        End If

        If Not dtrMalattiePermessi Is Nothing Then
            dtrMalattiePermessi.Close()
            dtrMalattiePermessi = Nothing
        End If
    End Sub

    Private Sub Recupera104(ByVal strcodicevolontario As String, ByVal MyData As String, ByRef INT104 As Integer)
        Dim dtrMalattiePermessi As SqlClient.SqlDataReader
        Dim strSql As String

        strSql = "SELECT COUNT(distinct p104.IDEntitàPresenza) as p104"
        strSql = strSql & " FROM entità "
        strSql = strSql & " left JOIN entitàpresenze p104 on p104.identità = entità.identità and p104.IdCausalePresenza = 12 "
        strSql = strSql & " where entità.codicevolontario = '" & strcodicevolontario.Replace("'", "''") & "' and year(p104.giorno) = year('" & MyData & "') and month(p104.giorno) = month('" & MyData & "')"
        dtrMalattiePermessi = ClsServer.CreaDatareader(strSql, Session("conn"))

        If dtrMalattiePermessi.HasRows Then
            dtrMalattiePermessi.Read()
            INT104 = dtrMalattiePermessi("p104")
        Else
            INT104 = 0
        End If

        If Not dtrMalattiePermessi Is Nothing Then
            dtrMalattiePermessi.Close()
            dtrMalattiePermessi = Nothing
        End If

        strSql = "SELECT COUNT(*) as P104TMP FROM #TEMP_PRESENZE_VOLONTARI "
        strSql = strSql & " where causale = 'P104' and codicevolontario = '" & strcodicevolontario.Replace("'", "''") & "' and year(giorno) = year('" & MyData & "') and month(giorno) = month('" & MyData & "')"
        dtrMalattiePermessi = ClsServer.CreaDatareader(strSql, Session("conn"))

        If dtrMalattiePermessi.HasRows Then
            dtrMalattiePermessi.Read()
            INT104 = INT104 + dtrMalattiePermessi("P104TMP")
        End If

        If Not dtrMalattiePermessi Is Nothing Then
            dtrMalattiePermessi.Close()
            dtrMalattiePermessi = Nothing
        End If

    End Sub

    Sub caricalista()
        Dim strsql As String
       

        'AUTORE: ANTONELLO DI CROCE
        'DESCRIZIONE: caricamento dei Titoli Studio Conseguimento codificati in tabella
        'DATA: 10/12/2013


        Dim dsCausale As SqlDataReader

        strsql = "SELECT CausaliPresenze.Descrizione ,CausaliPresenze.Codice, CausaliPresenze.Ordine FROM   CausaliPresenze  Order by CausaliPresenze.Ordine "
        dsCausale = ClsServer.CreaDatareader(strsql, Session("conn"))
        'Response.Write("<fieldset>")
        Response.Write("<ul>")
        Do While dsCausale.Read
            Response.Write("<li><strong>" & dsCausale.Item("Codice") & "</strong> per <strong>" & dsCausale.Item("Descrizione") & "</strong></li>")
        Loop
        Response.Write("</ul>")
        'Response.Write("</fieldset>")
        dsCausale.Close()
        dsCausale = Nothing
    End Sub
   
    Protected Sub CmdChiudi_Click(ByVal sender As Object, ByVal e As EventArgs) Handles CmdChiudi.Click
        'carico la home
        Response.Redirect("WfrmMain.aspx")
    End Sub
End Class