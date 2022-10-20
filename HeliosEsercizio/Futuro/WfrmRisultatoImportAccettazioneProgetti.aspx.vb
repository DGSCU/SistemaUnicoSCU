Imports System.Collections
Imports System.IO
Public Class WfrmRisultatoImportAccettazioneProgetti
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
        clCSV.ColumnName = "Tipo Esito Positivo"
        clCSV.Caption = "TipoEsitoPositivo"
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
                
            Else
                rwCSV(1) = DefArr(1)
                If UBound(DefArr) = 1 Then
                    rwCSV(2) = vbNullString
                Else
                    rwCSV(2) = DefArr(2)
                   
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
            strSql = "DROP TABLE [#TEMP_ACCETTAZIONE_PROGETTI]"

            cmdCanTempTable = New SqlClient.SqlCommand
            cmdCanTempTable.CommandText = strSql
            cmdCanTempTable.Connection = Session("conn")
            cmdCanTempTable.ExecuteNonQuery()
        Catch e As Exception

        End Try

        cmdCanTempTable.Dispose()
    End Sub

    'Private Sub RipristinoSedi(ByVal strCodiceProgetto As String, ByVal strTipoEsitoPositivo As String)

    '    Dim strUtente As String

    '    strUtente = Session("Utente")

    '    'Try
    '    MyCommand.CommandText = "SP_PROGETTI_CONTROLLI_IMPORT_ACCETTAZIONE"
    '    MyCommand.CommandType = CommandType.StoredProcedure
    '    MyCommand.Connection = Session("Conn")

    '    If MyCommand.Parameters.Count = 0 Then
    '        MyCommand.Parameters.Add("@Utente", SqlDbType.VarChar, 100).Value = strUtente
    '        MyCommand.Parameters.Add("@CodiceProgetto", SqlDbType.VarChar, 1000).Value = strCodiceProgetto
    '        MyCommand.Parameters.Add("@TipoEsitoPositivo", SqlDbType.VarChar, 1000).Value = strTipoEsitoPositivo


    '        MyCommand.Parameters.Add("@Esito", SqlDbType.TinyInt)
    '        MyCommand.Parameters("@Esito").Direction = ParameterDirection.Output
    '    Else
    '        MyCommand.Parameters("@CodiceProgetto").Value = strCodiceProgetto
    '        MyCommand.Parameters("@TipoEsitoPositivo").Value = strTipoEsitoPositivo

    '    End If


    '    MyCommand.ExecuteNonQuery()


    'End Sub

    Private Sub ScriviAccettazioneProgetti()
        Dim strsql As String
        Dim intX As Integer

        'If dtsGenerico.Tables(0).Rows.Count > 0 Then
        '    For intX = 0 To dtsGenerico.Tables(0).Rows.Count - 1
        '        'ClsUtility.CronologiaDocEntità(dtsGenerico.Tables(0).Rows(intX).Item("identità"), Session("Utente"), "Graduatoria Volontari", Session("conn"), DataProt, NProt)
        '        RipristinoSedi(dtsGenerico.Tables(0).Rows(intX).Item("Codiceprogramma"), dtsGenerico.Tables(0).Rows(intX).Item("TipoEsitoPositivo"))

        '    Next
        'End If
        MyCommand.CommandType = CommandType.Text

        strsql = "INSERT INTO cronologiaattività " & _
                 "(idattività,idstatoattività,datacronologia,idtipocronologia,usernameaccreditatore) " & _
                 "select b.idattività,b.idstatoattività,getdate(),0,'" & Session("Utente") & "'" & _
                 "FROM #TEMP_ACCETTAZIONE_PROGETTI a " & _
                 "INNER JOIN attività b ON a.codiceprogetto = b.codiceente "

        MyCommand.CommandText = strsql
        MyCommand.ExecuteNonQuery()

        strsql = "UPDATE attività SET idstatoattività=9,dataultimostato=getdate(), usernamestato = '" & Session("Utente") & "', Limitazioni=1, statovalutazione = 1   " & _
         "FROM #TEMP_ACCETTAZIONE_PROGETTI a " & _
         "INNER JOIN attività b ON a.CODICEPROGETTO = b.CODICEENTE " & _
         "WHERE a.TipoEsitoPositivo = 'AMMISSIBILE CON LIMITAZIONI'"
        MyCommand.CommandText = strsql
        MyCommand.ExecuteNonQuery()

        strsql = "UPDATE attività SET idstatoattività=9,dataultimostato=getdate(), usernamestato = '" & Session("Utente") & "', Limitazioni=0, statovalutazione = 1   " & _
         "FROM #TEMP_ACCETTAZIONE_PROGETTI a " & _
         "INNER JOIN attività b ON a.CODICEPROGETTO = b.CODICEENTE " & _
         "WHERE a.TipoEsitoPositivo = 'AMMISSIBILE'"
        MyCommand.CommandText = strsql
        MyCommand.ExecuteNonQuery()
        'DA FARE UPDATE!!!!



    End Sub

    Protected Sub CmdConferma_Click(sender As Object, e As EventArgs) Handles CmdConferma.Click
        Dim MyTransaction As System.Data.SqlClient.SqlTransaction
        Dim swErr As Boolean
        MyCommand = New SqlClient.SqlCommand
        MyCommand.Connection = Session("conn")
        CmdConferma.Visible = False

        Try

            MyTransaction = Session("conn").BeginTransaction(Session("IdEnte") & "_" & Session("Utente"))
            MyCommand.Transaction = MyTransaction

            ScriviAccettazioneProgetti()

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
        Response.Redirect("wfrmImportAccettazioneProgetti.aspx")
    End Sub

End Class
