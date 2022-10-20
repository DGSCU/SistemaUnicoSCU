Imports System.IO
Imports System.Data.SqlClient

Public Class WfrmImportOreFormazioneSpecificaVolontari
    Inherits System.Web.UI.Page

    Private Writer As StreamWriter
    Private strNote As String
    Private Tot As Integer
    Private TotOk As Integer
    Private TotKo As Integer
    Private NomeUnivoco As String
    Private xIdEnteRiferimento As Integer

#Region "Utility"
    Private Sub ChiudiDataReader(ByRef dataReader As SqlDataReader)
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
    End Sub


    Private Sub cmdChiudi_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdChiudi.Click
        'carico la home
        Response.Redirect("WfrmMain.aspx")
    End Sub


    Private Function VerificaCompatibilita(ByVal strCodProgetto As String) As Boolean
        'creata da simona cordella il 02/12/2014
        Dim dtrStato As SqlClient.SqlDataReader
        Dim strSql As String
        Dim blnCompatibilita As Boolean

        strSql = "SELECT idattività  as IdAttivita" _
               & " FROM attività INNER JOIN " _
               & " TipiProgetto ON attività.IDtipoProgetto = TipiProgetto.IDtipoProgetto " _
               & " WHERE  (attività.CodiceEnte = '" & strCodProgetto & "') " _
               & " AND TipiProgetto.Macrotipoprogetto like '" & Session("FiltroVisibilita") & "'"

        dtrStato = ClsServer.CreaDatareader(strSql, Session("conn"))

        dtrStato.Read()
        If dtrStato.HasRows Then
            blnCompatibilita = True
            idAttivita.Value = dtrStato("IdAttivita")
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
    Private Function VerificaProgettoGG(ByVal codiceEnte As String) As Boolean
        Dim dataReader As SqlClient.SqlDataReader
        Dim query As String
        Dim verificaOk As Boolean

        'verifico che il bando si di tipo 4=GG
        query = "   Select IdTipoProgetto" & _
                 " FROM [dbo].[attività] AS attivita " & _
                "  WHERE " & _
                " IdTipoProgetto = 4 And CodiceEnte = '" & codiceEnte & "' "
        Try
            dataReader = ClsServer.CreaDatareader(query, Session("conn"))

            dataReader.Read()
            If dataReader.HasRows Then
                verificaOk = True
            Else
                verificaOk = False
            End If

        Catch ex As Exception
            Throw ex
        Finally
            ChiudiDataReader(dataReader)
        End Try

        Return verificaOk
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
        '23/01/2008 da simona cordella
        'controllo la data inizio solo per il progetto attivo, in questo modo escludo i progetti coprogettati che hanno lo stato di archiviato
        strSql = "SELECT isnull(datediff(dd,DataInizioAttività,getdate()),0) as DiffGG " & _
                " FROM attività WHERE CodiceEnte = '" & strCodProgetto & "' and IdStatoAttività in(1,2)"


        dtrStato = ClsServer.CreaDatareader(strSql, Session("conn"))

        dtrStato.Read()


        If dtrStato("DiffGG") <= 150 And dtrStato("DiffGG") > 0 Then
            bEsitoData = True
        Else
            bEsitoData = False
        End If

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
                strSql = "SELECT distinct DatiPianificazione from VW_FORMAZIONE_RICERCA_PROGETTI WHERE 1=1"
                strSql = strSql & " and CodiceProgetto like '" & ClsServer.NoApice(Trim(strCodProgetto)) & "%'"
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
    Protected Sub CmdElabora_Click(ByVal sender As Object, ByVal e As EventArgs) Handles CmdElabora.Click
        NomeUnivoco = "orevolontari" & Session("IdEnte") & "_" & Session("Utente") & "_" & Year(Now) & Month(Now) & Day(Now) & Hour(Now) & Minute(Now) & Second(Now)

        If txtSelFile.PostedFile.FileName.ToString <> "" Then
            Dim file As String
            Dim estensione As String
            file = LCase(txtSelFile.FileName.ToString)
            estensione = file.Substring(file.Length - 4)
            If estensione <> ".csv" Then
                lblErrore.Visible = True
                lblErrore.Text = "Selezionare il file nel formato CSV."
                Exit Sub
            End If

            txtSelFile.PostedFile.SaveAs(Server.MapPath("upload") & "\" & NomeUnivoco & ".csv")
            CreaTabTemp()
            LeggiCSV()

        Else
            lblErrore.Visible = True
            lblErrore.Text = "Selezionare il file da inviare."
            Exit Sub
        End If

    End Sub

    Private Sub ScriviTabTemp(ByVal pArray() As String)
        '--- scrive nella tab temporanea
        Dim cmdTemp As SqlClient.SqlCommand
        Dim strsql As String

        Try

            strsql = "INSERT INTO #TEMP_ORE_FORMAZIONE_SPECIFICA_VOLONATARIO " & _
                     "(CodiceVolontario, " & _
                     "Cognome, " & _
                     "Nome, " & _
                     "CodiceFiscale, " & _
                     "CodiceProgetto, " & _
                     "OreFormazioneSpecifica " & _
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
                    'verifico l'esistenza in banca dati                    
                    If VerificaEntita(Trim(ArrCampi(0)), Trim(ArrCampi(1)), Trim(ArrCampi(2)), Trim(ArrCampi(4)), Trim(ArrCampi(8))) = False Then
                        strNote = strNote & "Volontario non trovato o per il quale non è previsto il caricamento delle ore di formazione."
                        swErr = True
                    Else

                        'VerificaTipoProgetto
                        If VerificaProgettoGG(ArrCampi(8)) = False Then
                            strNote = strNote & "Il progetto non è un Progetto Garanzia Giovani."
                            swErr = True

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

                            ''CodiceProgetto
                            If Trim(ArrCampi(8)) = vbNullString Then
                                strNote = strNote & "Il campo CodiceProgetto e' un campo obbligatorio."
                                swErr = True
                            Else
                                'verificacompatibilità agg. il 02/12/2014 da s.c.
                                If VerificaCompatibilita(ArrCampi(8)) = False Then
                                    strNote = strNote & "Progetto non esistente."
                                    swErr = True
                                End If

                            End If


                            'OreFormazione

                            If Trim(ArrCampi(15)) = vbNullString Then
                                strNote = strNote & "Il campo OreFormazioneSpecifica e' un campo obbligatorio."
                                swErr = True
                                intOreForm = 9999
                            Else
                                If IsNumeric(ArrCampi(15)) = False Then
                                    strNote = strNote & "Il campo OreFormazioneSpecifica non e' nel formato corretto."
                                    swErr = True
                                    intOreForm = 9999
                                ElseIf InStr(ArrCampi(15), ".") <> 0 Or InStr(ArrCampi(15), "-") <> 0 Or InStr(ArrCampi(15), ",") <> 0 Then
                                    strNote = strNote & "Il campo OreFormazione deve essere un numero intero."
                                    swErr = True
                                    intOreForm = 9999
                                Else
                                    intOreForm = ArrCampi(15)
                                End If
                                If (intOreForm <> 9999 And Trim(ArrCampi(0)) <> vbNullString) Then
                                    If (VerificaValidazioneOreFormazioneSpecifica(ArrCampi(0))) Then
                                        strNote = strNote & "Per il volontario le ore di formazione specifica risultano già validate."
                                        swErr = True
                                    End If
                                End If
                            End If
                        End If
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
        Response.Redirect("WfrmRisultatoImportOreFormazioneSpecificaVolontari.aspx?NomeFile=" & NomeUnivoco & "&Tot=" & Tot & "&TotOk=" & TotOk & "&TotKo=" & TotKo)

    End Sub
    Private Sub CreaTabTemp()
        Dim strSql As String
        Dim cmdCreateTempTable As SqlClient.SqlCommand

        CancellaTabellaTemp()

        '--- CREO TAB TEMPORANEA
        strSql = "CREATE TABLE [#TEMP_ORE_FORMAZIONE_SPECIFICA_VOLONATARIO] (" & _
                 "[CodiceVolontario] [nvarchar] (15), " & _
                 "[Cognome] [nvarchar] (100), " & _
                 "[Nome] [nvarchar] (100), " & _
                 "[CodiceFiscale] [nvarchar] (16), " & _
                 "[CodiceProgetto] [nvarchar] (22), " & _
                 "[OreFormazioneSpecifica] [int] " & _
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
            strSql = "DROP TABLE [#TEMP_ORE_FORMAZIONE_SPECIFICA_VOLONATARIO]"

            cmdCanTempTable = New SqlClient.SqlCommand
            cmdCanTempTable.CommandText = strSql
            cmdCanTempTable.Connection = Session("conn")
            cmdCanTempTable.ExecuteNonQuery()
        Catch e As Exception

        End Try

        cmdCanTempTable.Dispose()

        Session("DtbRicerca") = Nothing
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

    Private Function VerificaValidazioneOreFormazioneSpecifica(ByVal codiceVolontario As String) As Boolean
        Dim myDataReader As SqlDataReader
        Dim myQuery As StringBuilder = New StringBuilder()
        Dim PREFISSO_DOCUMENTO_OREFORMAZIONESPECIFICA As String = "FORMSPECVOL_"
        Dim STATO_VALIDATO As String = "1"
        Dim oreValidate As Boolean


        myQuery.Append("SELECT ")
        myQuery.Append("COUNT(Entita.IDEntità) as OreValidate ")
        myQuery.Append("FROM ")
        myQuery.Append("[dbo].[EntitàDocumenti] AS EntitaDocumenti ")
        myQuery.Append("JOIN [dbo].[Entità] AS Entita ON EntitaDocumenti.IdEntità = Entita.IDEntità ")
        myQuery.Append("WHERE SUBSTRING([FileName],1,12) = '")
        myQuery.Append(PREFISSO_DOCUMENTO_OREFORMAZIONESPECIFICA)
        myQuery.Append("' ")
        myQuery.Append("AND stato  = ")
        myQuery.Append(STATO_VALIDATO)
        myQuery.Append(" ")
        myQuery.Append("AND CodiceVolontario = '")
        myQuery.Append(codiceVolontario)
        myQuery.Append("' ")

        Try
            myDataReader = ClsServer.CreaDatareader(myQuery.ToString, Session("conn"))

            myDataReader.Read()

            If myDataReader.HasRows Then
                oreValidate = CBool(myDataReader("OreValidate"))
            End If
            Return oreValidate
        Catch ex As Exception
            Throw ex
        Finally
            ChiudiDataReader(myDataReader)
        End Try

    End Function
End Class