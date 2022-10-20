Imports System.IO
Imports System.Text.RegularExpressions
Imports System.Data.SqlClient
Public Class wfrmImportRimodulazioneProgettiUcraina
    Inherits System.Web.UI.Page
    Dim myDataSet As New DataSet
    Dim myDataTable As DataTable
    Dim myDataReader As SqlClient.SqlDataReader
    Dim querySql As String
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
        hf_IdEnte.Value = Session("idEnte")
    End Sub
    Private Sub CmdElabora_Click(ByVal sender As Object, ByVal e As EventArgs) Handles CmdElabora.Click

        Dim swErr As Boolean
        swErr = False
        If txtSelFile.PostedFile.FileName.ToString <> "" Then
            Try
                NomeUnivoco = "RimodulazioneUcraina" & Session("IdEnte") & "_" & Session("Utente") & "_" & Year(Now) & Month(Now) & Day(Now) & Hour(Now) & Minute(Now) & Second(Now)
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
            strSql = "DROP TABLE [#TEMP_RimodulazioneUcraina]"

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
        strSql = "CREATE TABLE [#TEMP_RimodulazioneUcraina] (" & _
                 "[CodiceProgramma] [nvarchar] (1000) COLLATE DATABASE_DEFAULT, " & _
                 "[TitoloProgramma] [nvarchar] (1000) COLLATE DATABASE_DEFAULT, " & _
                 "[CodiceProgetto] [nvarchar] (1000) COLLATE DATABASE_DEFAULT, " & _
                 "[TitoloProgetto] [nvarchar] (1000) COLLATE DATABASE_DEFAULT, " & _
                 "[CodiceSedeProgetto] [nvarchar] (1000) COLLATE DATABASE_DEFAULT, " & _
                 "[NomeSedeProgetto] [nvarchar] (1000) COLLATE DATABASE_DEFAULT, " & _
                 "[StatoRiattivazione] [varchar] (1000) COLLATE DATABASE_DEFAULT, " & _
                 "[ModalitàServizio] [varchar] (1000) COLLATE DATABASE_DEFAULT, " & _
                 "[CodiceAttivitàRimodulazione] [varchar] (1000) COLLATE DATABASE_DEFAULT, " & _
                 "[SpecificaAltro] [varchar] (1000) COLLATE DATABASE_DEFAULT, " & _
                 "[NumeroVolontariCoinvolti] [varchar] (1000) COLLATE DATABASE_DEFAULT, " & _
                 "[DataValidità] [varchar] (1000) COLLATE DATABASE_DEFAULT, " & _
                 "[TipoEmergenza] [varchar] (1000) COLLATE DATABASE_DEFAULT, " & _
                 "[Note] [nvarchar] (1000) COLLATE DATABASE_DEFAULT " & _
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

        Dim intEsito As Integer
        Dim strMessaggio As String
        ' Dim AppoNote As String

        '--- Leggo il file di input e scrivo quello di output
        Reader = New StreamReader(Server.MapPath("upload") & "\" & NomeUnivoco & ".CSV", System.Text.Encoding.UTF7, False)

        Writer = New StreamWriter(Server.MapPath("download") & "\" & NomeUnivoco & ".CSV", True, System.Text.Encoding.Default)

        '--- intestazione

        xLinea = Reader.ReadLine()
        Writer.WriteLine("Note;" & xLinea)

        '--- scorro le righe
        xLinea = Reader.ReadLine()
        While (xLinea <> "")
            swErr = False
            Tot = Tot + 1
            ArrCampi = CreaArray(xLinea)


            If UBound(ArrCampi) > 13 Then
                '--- se i campi sono troppi errore
                strNote = "Il numero delle colonne inserite è maggiore di quello richieste."
                swErr = True
                TotKo = TotKo + 1
            End If



            If Not dtrgenerico Is Nothing Then
                dtrgenerico.Close()
                dtrgenerico = Nothing
            End If

            If swErr = False Then
                'effetto ulteriori controlli SP_RIATTIVAZIONE_CONTROLLI_IMPORT
                ControlliImport(ArrCampi(2), ArrCampi(4), ArrCampi(6), ArrCampi(7), ArrCampi(9), ArrCampi(10), ArrCampi(11), ArrCampi(8), If(UBound(ArrCampi) > 11, ArrCampi(12), ""), intEsito, strMessaggio)

                If intEsito = 0 Then
                    strNote = strMessaggio
                    TotKo = TotKo + 1
                    swErr = True
                Else
                    ScriviTabTemp(ArrCampi)
                End If

            Else
                TotKo = TotKo + 1
            End If


            Writer.WriteLine(strNote & ";" & xLinea)
            strNote = vbNullString
            xLinea = Reader.ReadLine()
        End While

        Reader.Close()
        Writer.Close()
        Response.Redirect("WfrmRisultatoImportRimodulazioneProgettiUcraina.aspx?NomeFile=" & NomeUnivoco & "&Tot=" & Tot & "&TotOk=" & TotOk & "&TotKo=" & TotKo)

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
            strsql = "INSERT INTO #TEMP_RimodulazioneUcraina " & _
                     "(CodiceProgetto, " & _
                     "TitoloProgetto, " & _
                     "CodiceSedeProgetto, " & _
                     "NomeSedeProgetto, " & _
                     "StatoRiattivazione, " & _
                     "ModalitàServizio, " & _
                     "CodiceAttivitàRimodulazione," & _
                     "SpecificaAltro, " & _
                     "NumeroVolontariCoinvolti, " & _
                     "DataValidità, " & _
                     "TipoEmergenza" & _
                     ") " & _
                     "values " & _
                     "('" & Trim(ClsServer.NoApice(pArray(2))) & "', " & _
                     "'" & Trim(ClsServer.NoApice(pArray(3))) & "', " & _
                     "'" & Trim(ClsServer.NoApice(pArray(4))) & "', " & _
                     "'" & Trim(ClsServer.NoApice(pArray(5))) & "', " & _
                     "'" & Trim(ClsServer.NoApice(pArray(6))) & "', " & _
                     "'" & Trim(ClsServer.NoApice(pArray(7))) & "', " & _
                     "'" & Trim(ClsServer.NoApice(pArray(9))) & "', " & _
                     "'" & Trim(ClsServer.NoApice(pArray(10))) & "', " & _
                     "'" & Trim(ClsServer.NoApice(pArray(8))) & "', " & _
                     "'" & Trim(ClsServer.NoApice(pArray(11))) & "', " & _
                     "'" & Trim(ClsServer.NoApice(pArray(12))) & "' " & _
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

    Protected Sub CmdChiudi_Click(ByVal sender As Object, ByVal e As EventArgs) Handles CmdChiudi.Click
        'carico la home
        Response.Redirect("WfrmMain.aspx")
    End Sub

    Protected Sub imgEsporta_Click(sender As Object, e As EventArgs) Handles imgEsporta.Click

        EsportazioneRimodulazione()
    End Sub

    Sub EsportazioneRimodulazione()
        Dim arrParam() As SqlParameter
        Dim nomeFile As String = String.Empty
        ReDim arrParam(1)
        arrParam(0) = New SqlClient.SqlParameter
        arrParam(0).ParameterName = "@idente"
        arrParam(0).SqlDbType = SqlDbType.Int
        arrParam(0).Value = hf_IdEnte.Value

        arrParam(1) = New SqlClient.SqlParameter
        arrParam(1).ParameterName = "@versione"
        arrParam(1).SqlDbType = SqlDbType.Int
        arrParam(1).Value = 1

        myDataTable = New DataTable("Covid19")
        myDataTable = ExecuteDataTable("SP_RimodulazionePROGETTIUCRAINA_EXPORT_SEDI", arrParam)
        myDataSet.Tables.Add(myDataTable)
        OutputXls("ExpRimodulazioneUcraina", nomeFile)

        If myDataTable.Rows.Count > 0 Then
            hlVolontari.NavigateUrl = "download" & "\" + nomeFile
            hlVolontari.Target = "_blank"
            hlVolontari.Visible = True
            imgEsporta.Visible = False
        Else
            lblMessaggioErrore.Visible = True
            lblMessaggioErrore.Text = " Non risultano Progetti disponibili per la Rimodulazione"
            hlVolontari.Visible = False
            imgEsporta.Visible = False

        End If


    End Sub

    Public Function ExecuteDataTable(ByVal storedProcedureName As String, ByVal ParamArray arrParam() As SqlParameter) As DataTable
        Dim dataTable As DataTable
        Dim cmd As New SqlCommand
        cmd.Connection = Session("Conn")
        cmd.CommandType = CommandType.StoredProcedure
        cmd.CommandText = storedProcedureName
        If Not arrParam.Length = 0 Then
            For Each param As SqlParameter In arrParam
                cmd.Parameters.Add(param)
            Next
        End If

        Dim dataAdapter As New SqlDataAdapter(cmd)
        dataTable = New DataTable
        dataAdapter.Fill(dataTable)
        Return dataTable
    End Function

    Private Sub OutputXls(ByVal tipofile As String, ByRef nomeFile As String)
        nomeFile = Session("Utente") & "_" & tipofile & "_" & Format(DateTime.Now, "ddMMyyyyhhmmss") & ".csv"
        If File.Exists(Server.MapPath("download") & "\" & nomeFile) Then
            File.Delete((Server.MapPath("download") & "\" & nomeFile))
        End If
        SaveTextToFile(myDataTable, nomeFile)
    End Sub

    Sub SaveTextToFile(ByVal DTBRicerca As DataTable, ByVal nomeFile As String)

        Dim myWriter As StreamWriter
        Dim xLinea As String = String.Empty
        Dim i As Int64
        Dim j As Int64
        Dim nomeUnivoco As String

        If Not DTBRicerca.Rows.Count = 0 Then
            nomeUnivoco = nomeFile
            myWriter = New StreamWriter(Server.MapPath("download") & "\" & nomeUnivoco, True, System.Text.Encoding.Default)
            Dim intNumCol As Int64 = DTBRicerca.Columns.Count
            For i = 0 To intNumCol - 1
                xLinea &= DTBRicerca.Columns.Item(CInt(i)).ColumnName() & ";"
            Next
            myWriter.WriteLine(xLinea)
            xLinea = vbNullString
            For i = 0 To DTBRicerca.Rows.Count - 1
                For j = 0 To intNumCol - 1
                    If IsDBNull(DTBRicerca.Rows(CInt(i)).Item(CInt(j))) = True Then
                        xLinea &= vbNullString & ";"
                    Else
                        xLinea &= ClsUtility.FormatExport(DTBRicerca.Rows(CInt(i)).Item(CInt(j))) & ";"
                    End If
                Next
                myWriter.WriteLine(xLinea)
                xLinea = vbNullString
            Next
            myWriter.Close()
            myWriter = Nothing
        End If
    End Sub

    'NOTE
    Public Sub StatiRiattivazione()


        Dim strSQL As String
        Dim dsStatiRiattivazione As SqlDataReader

        strSQL = "Select Descrizione, Codice From StatiRiattivazione where rimodulazione = 1 "
        dsStatiRiattivazione = ClsServer.CreaDatareader(strSQL, Session("conn"))
        Response.Write("<fieldset>")
        Response.Write("<ul>")
        Do While dsStatiRiattivazione.Read
            Response.Write("<li><strong>" & dsStatiRiattivazione.Item("Codice") & "</strong> se <strong>" & dsStatiRiattivazione.Item("Descrizione") & "</strong></li>")
        Loop
        Response.Write("</ul>")
        Response.Write("</fieldset>")
        dsStatiRiattivazione.Close()
        dsStatiRiattivazione = Nothing
    End Sub


    Public Sub TipoModalitàServizio()

        Dim strSQL As String
        Dim dsTipoModalitàServizio As SqlDataReader

        strSQL = "Select Descrizione, Codice From TipiModalitàServizio where rimodulazione = 1 and IdTipoEmergenza=2"
        dsTipoModalitàServizio = ClsServer.CreaDatareader(strSQL, Session("conn"))
        Response.Write("<fieldset>")
        Response.Write("<ul>")
        Do While dsTipoModalitàServizio.Read
            Response.Write("<li><strong>" & dsTipoModalitàServizio.Item("Codice") & "</strong> se <strong>" & dsTipoModalitàServizio.Item("Descrizione") & "</strong></li>")
        Loop
        Response.Write("</ul>")
        Response.Write("</fieldset>")
        dsTipoModalitàServizio.Close()
        dsTipoModalitàServizio = Nothing
    End Sub

    Public Sub CodiceAttivitàRimodulazione()

        Dim strSQL As String
        Dim dsTipoModalitàServizio As SqlDataReader

        strSQL = "Select Descrizione, Codice From TipiAttivitàRimodulazione where rimodulazione = 1 and IdTipoEmergenza=2 order by len(codice),codice "
        dsTipoModalitàServizio = ClsServer.CreaDatareader(strSQL, Session("conn"))
        Response.Write("<fieldset>")
        Response.Write("<ul>")
        Do While dsTipoModalitàServizio.Read
            Response.Write("<li><strong>" & dsTipoModalitàServizio.Item("Codice") & "</strong> se <strong>" & dsTipoModalitàServizio.Item("Descrizione") & "</strong></li>")
        Loop
        Response.Write("</ul>")
        Response.Write("</fieldset>")
        dsTipoModalitàServizio.Close()
        dsTipoModalitàServizio = Nothing
    End Sub

    Public Sub DataRipresaServizio()

        Dim strSQL As String
        Dim dsStatiRiattivazione As SqlDataReader

        strSQL = "Select DataRipresaServizio From RiattivazioneDateRipristino "
        dsStatiRiattivazione = ClsServer.CreaDatareader(strSQL, Session("conn"))
        Response.Write("<fieldset>")
        Response.Write("<ul>")
        Do While dsStatiRiattivazione.Read
            Response.Write("<li><strong>" & dsStatiRiattivazione.Item("DataRipresaServizio") & "</strong>")
        Loop
        Response.Write("</ul>")
        Response.Write("</fieldset>")
        dsStatiRiattivazione.Close()
        dsStatiRiattivazione = Nothing
    End Sub

    Private Sub ControlliImport(ByVal strCodiceProgetto As String, ByVal strCodiceSedeProgetto As String, ByVal strStatoRiattivazione As String, ByVal strModalitàServizio As String, strCodiceAttivitàRimodulazione As String, strAltro As String, strDataValidità As String, NumeroVolontariCoinvolti As String, TipoEmergenza As String, ByRef intEsito As Integer, ByRef strMessaggio As String)

        Dim strUtente As String
        Dim SqlCmd As New SqlClient.SqlCommand

        strUtente = Session("Utente")

        Try
            SqlCmd.CommandText = "SP_RimodulazionePROGETTIUCRAINA_CONTROLLI_IMPORT"
            SqlCmd.CommandType = CommandType.StoredProcedure
            SqlCmd.Connection = Session("Conn")

            SqlCmd.Parameters.Add("@Utente", SqlDbType.VarChar, 100).Value = strUtente
            SqlCmd.Parameters.Add("@IdEnte", SqlDbType.Int).Value = Session("idEnte")
            SqlCmd.Parameters.Add("@CodiceProgetto", SqlDbType.VarChar, 1000).Value = strCodiceProgetto
            SqlCmd.Parameters.Add("@CodiceSedeProgetto", SqlDbType.VarChar, 1000).Value = strCodiceSedeProgetto
            SqlCmd.Parameters.Add("@StatoRiattivazione", SqlDbType.VarChar, 1000).Value = strStatoRiattivazione
            SqlCmd.Parameters.Add("@ModalitàServizio", SqlDbType.VarChar, 1000).Value = strModalitàServizio
            SqlCmd.Parameters.Add("@CodiceAttivitàRimodulazione", SqlDbType.VarChar, 1000).Value = strCodiceAttivitàRimodulazione
            SqlCmd.Parameters.Add("@Altro", SqlDbType.VarChar, 1000).Value = strAltro
            SqlCmd.Parameters.Add("@DataValidità", SqlDbType.VarChar, 1000).Value = strDataValidità
            SqlCmd.Parameters.Add("@NumeroVolontariCoinvolti", SqlDbType.VarChar, 1000).Value = NumeroVolontariCoinvolti
            SqlCmd.Parameters.Add("@TipoEmergenza", SqlDbType.VarChar, 1000).Value = TipoEmergenza

            SqlCmd.Parameters.Add("@Esito", SqlDbType.TinyInt)
            SqlCmd.Parameters("@Esito").Direction = ParameterDirection.Output

            SqlCmd.Parameters.Add("@messaggio", SqlDbType.VarChar)
            SqlCmd.Parameters("@messaggio").Size = 1000
            SqlCmd.Parameters("@messaggio").Direction = ParameterDirection.Output

            SqlCmd.ExecuteNonQuery()

            intEsito = SqlCmd.Parameters("@Esito").Value()
            strMessaggio = SqlCmd.Parameters("@messaggio").Value()

        Catch ex As Exception
            intEsito = 0
            strMessaggio = "Errore generico"
        Finally

        End Try
    End Sub
End Class