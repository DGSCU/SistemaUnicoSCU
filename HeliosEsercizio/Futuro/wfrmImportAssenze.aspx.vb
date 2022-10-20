Imports System.IO
Imports System.Text.RegularExpressions
Imports System.Data.SqlClient
Public Class wfrmImportAssenze
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
                NomeUnivoco = "assenzevolontari" & Session("IdEnte") & "_" & Session("Utente") & "_" & Year(Now) & Month(Now) & Day(Now) & Hour(Now) & Minute(Now) & Second(Now)
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
            strSql = "DROP TABLE [#TEMP_ASSENZE_VOLONTARI]"

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
        strSql = "CREATE TABLE [#TEMP_ASSENZE_VOLONTARI] (" & _
                 "[CodiceVolontario] [nvarchar] (15) COLLATE DATABASE_DEFAULT, " & _
                 "[Cognome] [nvarchar] (100) COLLATE DATABASE_DEFAULT, " & _
                 "[Nome] [nvarchar] (100) COLLATE DATABASE_DEFAULT, " & _
                 "[Mese] [smallint], " & _
                 "[Anno] [smallint], " & _
                 "[Giorni] [smallint], " & _
                 "[Causale] [int], " & _
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
                    If Trim(ArrCampi(3)) = vbNullString Then
                        strNote = strNote & "Il campo Mese e' un campo obbligatorio."
                        swErr = True
                    Else
                        If IsNumeric(ArrCampi(3)) = False Or Len(ArrCampi(3)) > 2 Or CInt(ArrCampi(3)) < 1 Or CInt(ArrCampi(3)) > 12 Then
                            strNote = strNote & "Il campo Mese non e' nel formato corretto."
                            swErr = True
                        End If
                    End If
                    'Anno
                    If Trim(ArrCampi(4)) = vbNullString Then
                        strNote = strNote & "Il campo Anno e' un campo obbligatorio."
                        swErr = True
                    Else
                        If IsNumeric(ArrCampi(4)) = False Or Len(ArrCampi(4)) > 4 Then
                            strNote = strNote & "Il campo Anno non e' nel formato corretto."
                            swErr = True
                        End If
                    End If
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

                    strsql = "select Mese, Anno from EntiConfermaAssenze where (Anno=" & Trim(ArrCampi(4)) & ") and IdEnte='" & Session("IdEnte") & "' and (Mese=" & Trim(ArrCampi(3)) & ") "
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


                    Dim myidCausale As Integer
                    If Trim(ArrCampi(6)) = 5 Then
                        myidCausale = 60
                    Else
                        myidCausale = CInt(Trim(ArrCampi(6))) + 14
                    End If

                    'verifico la causale per volontario anno mese
                    strsql = "select entitàassenze.identità, entitàassenze.idcausale, entitàassenze.anno, entitàassenze.mese from entitàassenze "
                    strsql = strsql & "inner join entità on entitàassenze.identità=entità.identità "
                    strsql = strsql & "where entitàassenze.idcausale=" & myidCausale & " and entitàassenze.anno='" & Trim(ArrCampi(4)) & "' and entitàassenze.mese='" & Trim(ArrCampi(3)) & "' and entità.codicevolontario='" & Trim(ArrCampi(0)) & "' "

                    dtrLeggiDati = ClsServer.CreaDatareader(strsql, Session("conn"))

                    If dtrLeggiDati.HasRows = True Then
                        strNote = strNote & "La causale di riferimento risulta essere gia' esistente in banca dati per il periodo."
                        swErr = True
                    End If

                    'chiudo il datareader
                    If Not dtrLeggiDati Is Nothing Then
                        dtrLeggiDati.Close()
                        dtrLeggiDati = Nothing
                    End If

                    'VerificaVolontario
                    If VerificaAnnoMese(ArrCampi(0), ArrCampi(3), ArrCampi(4)) = False Then
                        strNote = strNote & "Il periodo di riferimento dell'assenza deve essere compresa tra le date di inizio e fine servizio."
                        swErr = True
                    End If
                    'Giorni
                    If Trim(ArrCampi(5)) = vbNullString Then
                        strNote = strNote & "Il campo Giorni e' un campo obbligatorio."
                        swErr = True
                    Else
                        If IsNumeric(ArrCampi(5)) = False Or Len(ArrCampi(5)) > 2 Then
                            strNote = strNote & "Il campo Giorni non e' nel formato corretto."
                            swErr = True
                        Else
                            If CInt(ArrCampi(5)) <= 0 Then
                                strNote = strNote & "Il numero delle assenze deve essere maggiore di 0."
                                swErr = True
                            End If
                        End If
                    End If
                    'Causale
                    If Trim(ArrCampi(6)) = vbNullString Then
                        strNote = strNote & "Il campo Causale e' un campo obbligatorio."
                        swErr = True
                    Else
                        If CInt(Trim(ArrCampi(6))) < 1 Or CInt(Trim(ArrCampi(6))) > 5 Then
                            strNote = strNote & "La Causale inserita non è presente nella base dati."
                            swErr = True
                        End If
                    End If
                    'Note non obbligatorio
                    If Trim(ArrCampi(7)) <> vbNullString Then
                        If Len(ArrCampi(7)) > 500 Then
                            strNote = strNote & "Il campo Note puo' contenere massimo 500 caratteri."
                            swErr = True
                        End If
                    End If
                    ''Aggiunto da simona cordella il 27/02/2014
                    Dim NumGGDisponibili As Integer
                    NumGGDisponibili = GiorniDisponibili(ArrCampi(0), ArrCampi(4), ArrCampi(3), ArrCampi(6))
                    'verifico se sono già presenti assenza nella tabella temporanea

                    If Not dtrgenerico Is Nothing Then
                        dtrgenerico.Close()
                        dtrgenerico = Nothing
                    End If
                    strsql = "Select isnull(SUM(GIORNI),0) AS GIORNI from #TEMP_ASSENZE_VOLONTARI where Codicevolontario='" & ArrCampi(0) & "' and anno =" & ArrCampi(4) & " and mese =" & ArrCampi(3) & ""
                    dtrgenerico = ClsServer.CreaDatareader(strsql, Session("conn"))
                    If dtrgenerico.HasRows = True Then
                        dtrgenerico.Read()
                        If (NumGGDisponibili - CInt(dtrgenerico("GIORNI"))) < ArrCampi(5) Then
                            strNote = strNote & "Le assenze eccedono la capienza mensile."
                            swErr = True
                            TotKo = TotKo + 1
                        End If

                    End If
                    If Not dtrgenerico Is Nothing Then
                        dtrgenerico.Close()
                        dtrgenerico = Nothing
                    End If

                    If swErr = False Then
                        ScriviTabTemp(ArrCampi)
                    Else
                        TotKo = TotKo + 1
                    End If

                    'Modifica del 23/11/2005 by Di Croce & Paolella
                    'Verifica giorni in mese-anno
                    'If VerificaGiorni(ArrCampi(4), ArrCampi(3), ArrCampi(5)) = False Then
                    '    strNote = strNote & "Le assenze eccedono la capienza mensile."
                    '    swErr = True
                    '    TotKo = TotKo + 1
                    'End If
                    '***********************************************************************
                End If
            End If

            Writer.WriteLine(strNote & ";" & xLinea)
            strNote = vbNullString
            xLinea = Reader.ReadLine()
        End While

        Reader.Close()
        Writer.Close()
        Response.Redirect("WfrmRisultatoImportAssenze.aspx?NomeFile=" & NomeUnivoco & "&Tot=" & Tot & "&TotOk=" & TotOk & "&TotKo=" & TotKo)
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
        Dim myidCausale As Integer

        If Trim(pArray(6)) = 5 Then
            myidCausale = 60
        Else
            myidCausale = CInt(Trim(pArray(6))) + 14
        End If

        Try
            strsql = "INSERT INTO #TEMP_ASSENZE_VOLONTARI " & _
                     "(CodiceVolontario, " & _
                     "Cognome, " & _
                     "Nome, " & _
                     "Mese, " & _
                     "Anno, " & _
                     "Giorni, " & _
                     "Causale, " & _
                     "Note" & _
                     ") " & _
                     "values " & _
                     "('" & Trim(ClsServer.NoApice(pArray(0))) & "', " & _
                     "'" & Trim(ClsServer.NoApice(pArray(1))) & "', " & _
                     "'" & Trim(ClsServer.NoApice(pArray(2))) & "', " & _
                     Trim(pArray(3)) & ", " & _
                     Trim(pArray(4)) & ", " & _
                     Trim(pArray(5)) & ", " & _
                     myidCausale & ", " & _
                     "'" & Trim(ClsServer.NoApice(pArray(7))) & "')"

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
        '** mod. da s.c. il 27/11/2014 FiltroVisibilità
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
                 " AND TipiProgetto.MacroTipoProgetto like '" & Session("FiltroVisibilita") & "' AND TipiProgetto.MacroTipoProgetto ='SCN' "


        dtrVolontario = ClsServer.CreaDatareader(strSql, Session("conn"))
        VerificaVolontario = dtrVolontario.HasRows

        dtrVolontario.Close()
        dtrVolontario = Nothing

    End Function

    Private Function VerificaAnnoMese(ByVal pCodiceVolontario As String, ByVal pMese As String, ByVal pAnno As String) As Boolean
        Dim dtrAnnoMese As SqlClient.SqlDataReader
        Dim strSql As String
        Dim AnnoMeseInizio As String
        Dim AnnoMeseFine As String
        Dim AnnoMeseAssenza As String
        Dim TmpArr() As String

        VerificaAnnoMese = True

        If Len(pMese) = 1 Then
            pMese = "0" & pMese
        End If
        AnnoMeseAssenza = pAnno & pMese

        strSql = "SELECT DataInizioServizio, DataFineServizio FROM Entità " & _
                 "WHERE CodiceVolontario = '" & pCodiceVolontario & "'"

        dtrAnnoMese = ClsServer.CreaDatareader(strSql, Session("conn"))

        If dtrAnnoMese.HasRows = True Then
            dtrAnnoMese.Read()
            TmpArr = Split(dtrAnnoMese("DataInizioServizio"), "/")
            If Len(TmpArr(1)) = 1 Then
                TmpArr(1) = "0" & TmpArr(1)
            End If
            AnnoMeseInizio = TmpArr(2) & TmpArr(1)
            TmpArr = Split(dtrAnnoMese("DataFineServizio"), "/")
            If Len(TmpArr(1)) = 1 Then
                TmpArr(1) = "0" & TmpArr(1)
            End If
            AnnoMeseFine = TmpArr(2) & TmpArr(1)

            If AnnoMeseAssenza < AnnoMeseInizio Or AnnoMeseAssenza > AnnoMeseFine Then
                VerificaAnnoMese = False
            End If
        Else
            VerificaAnnoMese = False
        End If

        dtrAnnoMese.Close()
        dtrAnnoMese = Nothing

    End Function


    Private Function VerificaGiorni(ByVal intAnno As Int16, ByVal intMese As Int16, ByVal intGiorni As Integer) As Boolean
        If intGiorni > DateTime.DaysInMonth(intAnno, intMese) Then
            VerificaGiorni = False
        Else
            VerificaGiorni = True
        End If
    End Function

    Private Function GiorniDisponibili(ByVal CodVolontario As String, ByVal Anno As String, ByVal Mese As String, ByVal IdCasuale As Integer) As Integer

        'Creata da Simona Cordella il 26/02/2014
        'Verifica i giorni diposnibile nel mese per l'inserimento delle assenze

        Dim GiorniTotaleMese As Integer
        Dim DataInizioSer As String
        Dim DataFineSer As String

        ' Dim GiorniTotMeseDisponibili As Integer

        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If


        'giorni totali del mese
        GiorniTotaleMese = DateTime.DaysInMonth(CInt(Anno), CInt(Mese))
        GiorniDisponibili = GiorniTotaleMese
        strsql = " SELECT dbo.formatodata(DataInizioServizio) as DataInizioServizio, " & _
                 " dbo.formatodata(DatafineServizio) as DatafineServizio " & _
                 " FROM entità " & _
                 " WHERE CodiceVolontario = '" & CodVolontario & "'"
        dtrgenerico = ClsServer.CreaDatareader(strsql, Session("conn"))

        If dtrgenerico.HasRows = True Then
            dtrgenerico.Read()
            DataInizioSer = dtrgenerico("DataInizioServizio")
            DataFineSer = dtrgenerico("DatafineServizio")

            'controllo se il mese di inzio servizio corrisponde al mese delle assenze
            If Month(DataInizioSer) = Mese And Year(DataInizioSer) = Anno Then
                GiorniDisponibili = (GiorniTotaleMese - CInt(Day(DataInizioSer))) + 1
            End If
            'controllo se il mese di fine servizio corrisponde al mese delle assenze
            If Month(DataFineSer) = Mese And Year(DataFineSer) = Anno Then
                GiorniDisponibili = GiorniDisponibili - (GiorniTotaleMese - CInt(Day(DataFineSer)))
            End If
        End If

        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If


        strsql = "SELECT ISNULL(Sum(a.Giorni),0) as GiorniTotAttuali "
        strsql = strsql & "FROM EntitàAssenze a "
        strsql = strsql & "inner join entità  b on a.idEntità=b.idEntità "
        strsql = strsql & "where a.IdCausale <> " & IdCasuale & " and b.CodiceVolontario='" & CodVolontario & "' and  a.Mese=" & Mese & " and  a.anno=" & Anno & " and a.stato in(1,2)  "
        dtrgenerico = ClsServer.CreaDatareader(strsql, Session("conn"))
        If dtrgenerico.HasRows = True Then
            dtrgenerico.Read()
            GiorniDisponibili = GiorniDisponibili - CInt(dtrgenerico("GiorniTotAttuali"))
        End If

        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If

        Return GiorniDisponibili
    End Function
 
    Protected Sub CmdChiudi_Click(ByVal sender As Object, ByVal e As EventArgs) Handles CmdChiudi.Click
        'carico la home
        Response.Redirect("WfrmMain.aspx")
    End Sub
End Class