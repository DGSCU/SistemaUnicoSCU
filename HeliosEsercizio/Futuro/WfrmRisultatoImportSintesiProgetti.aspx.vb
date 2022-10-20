Imports System.Collections
Imports System.IO
Public Class WfrmRisultatoImportSintesiProgetti

    Inherits System.Web.UI.Page
    Dim MyCommand As SqlClient.SqlCommand
    Dim DefArr As String()
    Dim ArrAggiorna As String()
    'Dim ArrTel As String()
    Dim ArrDatePreviste As String()
    Dim dtsLocal As DataSet

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not Session("LogIn") Is Nothing Then
            If Session("LogIn") = False Then 'verifico validità log-in
                Response.Redirect("LogOn.aspx")
            End If
        Else
            Response.Redirect("LogOn.aspx")
        End If
        If Page.IsPostBack = False Then

            CaricaGriglia()

            lblTotali.Text = "Sono state inviate " & Request.QueryString("Tot") & " righe. " & _
                             Request.QueryString("TotOk") & " con esito positivo. " & Request.QueryString("TotKo") & " con esito negativo."

            hlDownLoad.NavigateUrl = "download\" & Request.QueryString("NomeFile") & ".CSV"

            If CInt(Request.QueryString("TotKo")) > 0 Then
                CmdConferma.Visible = False
            Else
                AvvisoConferma.Visible = True
                avviso.Visible = True
                testoavviso.InnerHtml = "LA VERIFICA DEI DATI IMMESSI NEL FILE CSV RISULTA CORRETTA. PER SALVARE DEFINITIVAMENTE I DATI PREMERE IL TASTO CONFERMA."
            End If
        End If
    End Sub


    Private Sub CaricaGriglia()
        Dim dtCSV As DataTable = New DataTable
        Dim rwCSV As DataRow
        Dim clCSV As DataColumn

        Dim strSql As String
        Dim i As Integer

        Dim TmpArr() As String

        Dim Reader As StreamReader
        Dim xLinea As String
        Dim ArrEnte() As String


        clCSV = New DataColumn
        clCSV.DataType = System.Type.GetType("System.String")
        clCSV.ColumnName = "Note"
        clCSV.Caption = "Note"
        clCSV.ReadOnly = False
        clCSV.Unique = False
        dtCSV.Columns.Add(clCSV)

        clCSV = New DataColumn
        clCSV.DataType = System.Type.GetType("System.String")
        clCSV.ColumnName = "Codice Progetto"
        clCSV.Caption = "CodiceProgetto"
        clCSV.ReadOnly = False
        clCSV.Unique = False
        dtCSV.Columns.Add(clCSV)

        clCSV = New DataColumn
        clCSV.DataType = System.Type.GetType("System.String")
        clCSV.ColumnName = "Titolo"
        clCSV.Caption = "Titolo"
        clCSV.ReadOnly = False
        clCSV.Unique = False
        dtCSV.Columns.Add(clCSV)

        clCSV = New DataColumn
        clCSV.DataType = System.Type.GetType("System.String")
        clCSV.ColumnName = "URL"
        clCSV.Caption = "url"
        clCSV.ReadOnly = False
        clCSV.Unique = False
        dtCSV.Columns.Add(clCSV)




        Reader = New StreamReader(Server.MapPath("download") & "\" & Request.QueryString("NomeFile") & ".CSV", System.Text.Encoding.Default, False)
        xLinea = Reader.ReadLine()
        xLinea = Reader.ReadLine()

        While (xLinea <> "")

            DefArr = CreaArray(xLinea)
            rwCSV = dtCSV.NewRow

            rwCSV(0) = DefArr(0)

            If UBound(DefArr) = 0 Then
                rwCSV(1) = vbNullString
                rwCSV(2) = vbNullString
                rwCSV(3) = vbNullString


            Else
                rwCSV(1) = DefArr(1)
                If UBound(DefArr) = 1 Then
                    rwCSV(2) = vbNullString
                Else
                    rwCSV(2) = DefArr(2)
                    If UBound(DefArr) = 2 Then
                        rwCSV(3) = vbNullString
                    Else
                        If Len(DefArr(3)) > 50 Then
                            DefArr(3) = DefArr(3).Substring(0, 50) & "[...]"
                            'Dim str As String = DefArr(3)
                            'Dim parts(str.Length \ 100) As String
                            'Dim y As Integer
                            'For x As Integer = 0 To str.Length - 1 Step 100
                            '    y = x
                            '    If (Len(DefArr(3)) - x) < 100 Then
                            '        parts(x \ 100) = str.Substring(x, (Len(DefArr(3)) - x))
                            '    Else
                            '        parts(x \ 100) = str.Substring(x, 100)
                            '    End If
                            'Next
                            'DefArr(3) = String.Join(vbCrLf, parts)
                        End If



                        rwCSV(3) = DefArr(3)

                    End If

                    ' & vbcrlf & "
                End If
            End If
            'ciaoooo
            dtCSV.Rows.Add(rwCSV)
            xLinea = Reader.ReadLine()
        End While

        dtgCSV.DataSource = dtCSV
        dtgCSV.DataBind()

    End Sub


    Private Function CreaArray(ByVal pLinea As String) As String()
        Dim TmpArr As String()

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


    Private Sub CancellaTabellaTemp()
        Dim strSql As String
        Dim cmdCanTempTable As SqlClient.SqlCommand

        Try
            '--- CANCELLO TAB TEMPORANEA
            strSql = "DROP TABLE [#TEMP_SINTESI_PROG]"

            cmdCanTempTable = New SqlClient.SqlCommand
            cmdCanTempTable.CommandText = strSql
            cmdCanTempTable.Connection = Session("conn")
            cmdCanTempTable.ExecuteNonQuery()
        Catch e As Exception

        End Try

        cmdCanTempTable.Dispose()
    End Sub


    Private Sub ScriviSintesiProgetti(ByRef dtsGenerico As DataSet)
        Dim strsql As String
        Dim intX As Integer

        If dtsGenerico.Tables(0).Rows.Count > 0 Then
            For intX = 0 To dtsGenerico.Tables(0).Rows.Count - 1
                'ClsUtility.CronologiaDocEntità(dtsGenerico.Tables(0).Rows(intX).Item("identità"), Session("Utente"), "Graduatoria Volontari", Session("conn"), DataProt, NProt)
                'RipristinoSedi(dtsGenerico.Tables(0).Rows(intX).Item("Codiceprogetto"), dtsGenerico.Tables(0).Rows(intX).Item("Codicesedeprogetto"), dtsGenerico.Tables(0).Rows(intX).Item("DataRipresaServizio"))

            Next
        End If
        MyCommand.CommandType = CommandType.Text

        strsql = "DELETE FROM attivitàsintesi " & _
                 "FROM #TEMP_SINTESI_PROG a " & _
                 "INNER JOIN attività b on a.codiceprogetto = b.codiceente " & _
                 "INNER JOIN attivitàsintesi c on b.idattività = c.idattività"

        MyCommand.CommandText = strsql
        MyCommand.ExecuteNonQuery()

        strsql = "INSERT INTO AttivitàSintesi " & _
                 "(IdAttività, " & _
                 "Url, " & _
                 "UsernameInseritore, " & _
                 "DataCreazioneRecord) " & _
                 "SELECT " & _
                 "b.IdAttività, " & _
                 "a.Url, " & _
                 "'" & Session("Utente") & "', " & _
                 "getdate() " & _
                 "FROM #TEMP_SINTESI_PROG  a " & _
                 "INNER JOIN attività b on a.codiceprogetto = b.codiceente"

        MyCommand.CommandText = strsql
        MyCommand.ExecuteNonQuery()

        strsql = "INSERT INTO AttivitàSintesiCrono " & _
                "(IdAttività, " & _
                "Url, " & _
                "UsernameInseritore, " & _
                "DataCreazioneRecord) " & _
                "SELECT " & _
                "b.IdAttività, " & _
                "a.Url, " & _
                "'" & Session("Utente") & "', " & _
                "getdate() " & _
                "FROM #TEMP_SINTESI_PROG  a " & _
                "INNER JOIN attività b on a.codiceprogetto = b.codiceente"
        MyCommand.CommandText = strsql
        MyCommand.ExecuteNonQuery()




    End Sub


    Protected Sub CmdConferma_Click(sender As Object, e As EventArgs) Handles CmdConferma.Click
        Dim MyTransaction As System.Data.SqlClient.SqlTransaction
        Dim swErr As Boolean
        MyCommand = New SqlClient.SqlCommand
        MyCommand.Connection = Session("conn")
        CmdConferma.Visible = False

        Try

            Dim strsql As String
            Dim dtsGenerico As DataSet
            strsql = "select Codiceprogetto, titolo, url from #TEMP_SINTESI_PROG where url <> ''"
            dtsGenerico = ClsServer.DataSetGenerico(strsql, Session("conn"))

            MyTransaction = Session("conn").BeginTransaction(Session("IdEnte") & "_" & Session("Utente"))
            MyCommand.Transaction = MyTransaction

            ScriviSintesiProgetti(dtsGenerico)

            MyTransaction.Commit()
            'ControlloAssenzeEccedenti()
        Catch exc As Exception

            MyTransaction.Rollback(Session("IdEnte") & "_" & Session("Utente"))
            swErr = True

        End Try

        MyCommand.Dispose()

        If swErr = False Then
            'Esito positivo
            lblEsito.Text = "Operazione di inserimento dei dati effettuata con successo."
            lblEsito.Visible = True
            AvvisoConferma.Visible = False
        Else
            'Errore Insert
            lblEsito.Text = "Errore durante l'operazione di inserimento dei dati."
            lblEsito.Visible = True

        End If

        CancellaTabellaTemp()
    End Sub


    Protected Sub CmdChiudi_Click(sender As Object, e As EventArgs) Handles CmdChiudi.Click
        CancellaTabellaTemp()
        Response.Redirect("wfrmImportSintesiProgetti.aspx")
    End Sub
End Class