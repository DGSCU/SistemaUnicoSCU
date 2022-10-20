Imports System.IO

Public Class WfrmRisultatoImportOreVolontari
    Inherits System.Web.UI.Page
    Dim MyCommand As SqlClient.SqlCommand
    Dim DefArr As String()
    Dim ArrAggiorna As String()
    Dim ArrDatePreviste As String()

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Session("LogIn") Is Nothing Then
            If Session("LogIn") = False Then 'verifico validità log-in
                Response.Redirect("LogOn.aspx")
            End If
        Else
            Response.Redirect("LogOn.aspx")
        End If

        If Request.QueryString("TipoFormazioneGenerale") = 1 Then
            CaricaGriglia()
        Else
            CaricaGrigliaTranche()
        End If

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
        clCSV.ColumnName = "Codice Volontario"
        clCSV.Caption = "Codice Volontario"
        clCSV.ReadOnly = False
        clCSV.Unique = False
        dtCSV.Columns.Add(clCSV)

        clCSV = New DataColumn
        clCSV.DataType = System.Type.GetType("System.String")
        clCSV.ColumnName = "Cognome"
        clCSV.Caption = "Cognome"
        clCSV.ReadOnly = False
        clCSV.Unique = False
        dtCSV.Columns.Add(clCSV)

        clCSV = New DataColumn
        clCSV.DataType = System.Type.GetType("System.String")
        clCSV.ColumnName = "Nome"
        clCSV.Caption = "Nome"
        clCSV.ReadOnly = False
        clCSV.Unique = False
        dtCSV.Columns.Add(clCSV)

        clCSV = New DataColumn
        clCSV.DataType = System.Type.GetType("System.String")
        clCSV.ColumnName = "Codice Fiscale"
        clCSV.Caption = "Codice Fiscale"
        clCSV.ReadOnly = False
        clCSV.Unique = False
        dtCSV.Columns.Add(clCSV)

        clCSV = New DataColumn
        clCSV.DataType = System.Type.GetType("System.String")
        clCSV.ColumnName = "Codice Progetto"
        clCSV.Caption = "Codice Progetto"
        clCSV.ReadOnly = False
        clCSV.Unique = False
        dtCSV.Columns.Add(clCSV)

        clCSV = New DataColumn
        clCSV.DataType = System.Type.GetType("System.String")
        clCSV.ColumnName = "Ore Formazione"
        clCSV.Caption = "Ore Formazione"
        clCSV.ReadOnly = False
        clCSV.Unique = False
        dtCSV.Columns.Add(clCSV)

        Reader = New StreamReader(Server.MapPath("download") & "\" & Request.QueryString("NomeFile") & ".CSV")
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
                rwCSV(4) = vbNullString
                rwCSV(5) = vbNullString
                rwCSV(6) = vbNullString
            Else
                rwCSV(1) = DefArr(1)
                If UBound(DefArr) = 1 Then
                    rwCSV(2) = vbNullString
                Else
                    rwCSV(2) = DefArr(2)
                    If UBound(DefArr) = 2 Then
                        rwCSV(3) = vbNullString
                    Else
                        rwCSV(3) = DefArr(3)
                        If UBound(DefArr) = 3 Then
                            rwCSV(4) = vbNullString
                        Else
                            rwCSV(4) = DefArr(5)
                            If UBound(DefArr) = 4 Then
                                rwCSV(5) = vbNullString
                            Else
                                rwCSV(5) = DefArr(9)
                                If UBound(DefArr) = 5 Then
                                    rwCSV(6) = vbNullString
                                Else
                                    rwCSV(6) = DefArr(UBound(DefArr)) 'DefArr(16)
                                End If
                            End If
                        End If
                    End If
                End If
            End If

            rwCSV(2) = DefArr(2)
            If UBound(DefArr) > 3 Then
                rwCSV(3) = DefArr(3)
            Else
                rwCSV(3) = vbNullString
            End If


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

    Private Sub CaricaGrigliaTranche()
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
        clCSV.ColumnName = "Codice Volontario"
        clCSV.Caption = "Codice Volontario"
        clCSV.ReadOnly = False
        clCSV.Unique = False
        dtCSV.Columns.Add(clCSV)

        clCSV = New DataColumn
        clCSV.DataType = System.Type.GetType("System.String")
        clCSV.ColumnName = "Cognome"
        clCSV.Caption = "Cognome"
        clCSV.ReadOnly = False
        clCSV.Unique = False
        dtCSV.Columns.Add(clCSV)

        clCSV = New DataColumn
        clCSV.DataType = System.Type.GetType("System.String")
        clCSV.ColumnName = "Nome"
        clCSV.Caption = "Nome"
        clCSV.ReadOnly = False
        clCSV.Unique = False
        dtCSV.Columns.Add(clCSV)

        clCSV = New DataColumn
        clCSV.DataType = System.Type.GetType("System.String")
        clCSV.ColumnName = "Codice Fiscale"
        clCSV.Caption = "Codice Fiscale"
        clCSV.ReadOnly = False
        clCSV.Unique = False
        dtCSV.Columns.Add(clCSV)

        clCSV = New DataColumn
        clCSV.DataType = System.Type.GetType("System.String")
        clCSV.ColumnName = "Codice Progetto"
        clCSV.Caption = "Codice Progetto"
        clCSV.ReadOnly = False
        clCSV.Unique = False
        dtCSV.Columns.Add(clCSV)

        clCSV = New DataColumn
        clCSV.DataType = System.Type.GetType("System.String")
        clCSV.ColumnName = "Ore Formazione Prima Tranche"
        clCSV.Caption = "Ore Formazione Prima Tranche"
        clCSV.ReadOnly = False
        clCSV.Unique = False
        dtCSV.Columns.Add(clCSV)

        clCSV = New DataColumn
        clCSV.DataType = System.Type.GetType("System.String")
        clCSV.ColumnName = "Ore Formazione Seconda Tranche"
        clCSV.Caption = "Ore Formazione Seconda Tranche"
        clCSV.ReadOnly = False
        clCSV.Unique = False
        dtCSV.Columns.Add(clCSV)

        Reader = New StreamReader(Server.MapPath("download") & "\" & Request.QueryString("NomeFile") & ".CSV")
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
                rwCSV(4) = vbNullString
                rwCSV(5) = vbNullString
                rwCSV(6) = vbNullString
                rwCSV(7) = vbNullString
            Else
                rwCSV(1) = DefArr(1)
                If UBound(DefArr) = 1 Then
                    rwCSV(2) = vbNullString
                Else
                    rwCSV(2) = DefArr(2)
                    If UBound(DefArr) = 2 Then
                        rwCSV(3) = vbNullString
                    Else
                        rwCSV(3) = DefArr(3)
                        If UBound(DefArr) = 3 Then
                            rwCSV(4) = vbNullString
                        Else
                            rwCSV(4) = DefArr(5)
                            If UBound(DefArr) = 4 Then
                                rwCSV(5) = vbNullString
                            Else
                                rwCSV(5) = DefArr(9)
                                If UBound(DefArr) = 5 Then
                                    rwCSV(6) = vbNullString
                                Else
                                    rwCSV(6) = DefArr(16) 'ore prima tranche
                                    If UBound(DefArr) = 6 Then
                                        rwCSV(7) = vbNullString
                                    Else
                                        rwCSV(7) = DefArr(UBound(DefArr)) 'ore sendodatranche(ultima colonna
                                    End If
                                End If
                            End If
                        End If
                    End If
                End If
            End If

            rwCSV(2) = DefArr(2)
            If UBound(DefArr) > 3 Then
                rwCSV(3) = DefArr(3)
            Else
                rwCSV(3) = vbNullString
            End If


            dtCSV.Rows.Add(rwCSV)
            xLinea = Reader.ReadLine()
        End While

        dtgCSV.DataSource = dtCSV
        dtgCSV.DataBind()

    End Sub

    Private Sub CmdConferma_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdConferma.Click
        If Request.QueryString("TipoFormazioneGenerale") = 1 Then
            UpdateFormazioneUnicaTranche()
        Else
            UpdateFormazioneDueTranche()
        End If
    End Sub

    Private Sub UpdateFormazioneUnicaTranche()
        ' If imgConferma.ToolTip = "Vai al questionario" Then
        ' Response.Redirect("WfrmQuestionarioProgetto.aspx")
        'Else
        Dim MyTransaction As System.Data.SqlClient.SqlTransaction
        Dim swErr As Boolean
        Dim strsql As String
        Dim i As Integer
        Dim myDS As New DataSet

        CmdConferma.Visible = False
        'TestoSu.Visible = False
        'lblTestoSu1.Visible = False
        Try

            strsql = "Select * From [#TEMP_ORE_VOLONATARIO]"
            myDS = ClsServer.DataSetGenerico(strsql, Session("conn"))

            MyCommand = New SqlClient.SqlCommand
            MyCommand.Connection = Session("conn")

            MyTransaction = Session("conn").BeginTransaction(Session("IdEnte") & "_" & Session("Utente"))

            MyCommand.Transaction = MyTransaction           'select

            For i = 0 To myDS.Tables(0).Rows.Count - 1
                strsql = "Update entità Set OreFormazione=" & myDS.Tables(0).Rows(i).Item("OreFormazione") _
                    & " Where CodiceVolontario ='" & myDS.Tables(0).Rows(i).Item("CodiceVolontario") & "' "

                MyCommand.CommandText = strsql
                MyCommand.ExecuteNonQuery()
            Next

            MyTransaction.Commit()

        Catch exc As Exception
            MyTransaction.Rollback(Session("IdEnte") & "_" & Session("Utente"))
            swErr = True
        End Try

        MyCommand.Dispose()

        If swErr = False Then
            'Esito positivo
            lblEsito.Text = "Operazione di inserimento dei dati effettuata con successo."

            lblEsito.Visible = True
            'imgAnnulla.ImageUrl = "images/chiudi.jpg"

            CmdConferma.Visible = False
            'TestoSu.Visible = False
            'lblTestoSu1.Visible = False
            'imgConferma.ToolTip = "Vai al questionario"
            'imgConferma.ImageUrl = "images/elabora.jpg"
        Else
            'Errore Insert
            lblEsito.Text = "Errore durante l'operazione di inserimento dei dati."
            lblEsito.Visible = True
            'imgAnnulla.ImageUrl = "images/annulla.jpg"
        End If

        CancellaTabellaTemp()
        'End If
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

        Session("DtbRicerca") = Nothing
    End Sub

    Private Sub UpdateFormazioneDueTranche()

        Dim MyTransaction As System.Data.SqlClient.SqlTransaction
        Dim swErr As Boolean
        Dim strsql As String
        Dim i As Integer
        Dim myDS As New DataSet
        Dim OreFormazione As String
        Dim OreFormazioneST As String


        CmdConferma.Visible = False
        'TestoSu.Visible = False
        'lblTestoSu1.Visible = False
        Try

            strsql = "Select * From [#TEMP_ORE_VOLONATARIO]"
            myDS = ClsServer.DataSetGenerico(strsql, Session("conn"))

            MyCommand = New SqlClient.SqlCommand
            MyCommand.Connection = Session("conn")

            MyTransaction = Session("conn").BeginTransaction(Session("IdEnte") & "_" & Session("Utente"))

            MyCommand.Transaction = MyTransaction           'select

            For i = 0 To myDS.Tables(0).Rows.Count - 1
                ' OreFormazione = CInt(myDS.Tables(0).Rows(i).Item("OreFormazionePrimaTranche")) + CInt(myDS.Tables(0).Rows(i).Item("OreFormazioneSecondaTranche"))
                OreFormazioneST = "" & myDS.Tables(0).Rows(i).Item("OreFormazioneSecondaTranche")
                If OreFormazioneST <> "" Then
                    OreFormazione = CInt(myDS.Tables(0).Rows(i).Item("OreFormazionePrimaTranche")) + CInt(OreFormazioneST)
                Else
                    OreFormazione = "null"
                End If
                strsql = "Update entità Set " & _
                         " OreFormazione=" & OreFormazione & ", " & _
                         " OreFormazionePrimaTranche=" & myDS.Tables(0).Rows(i).Item("OreFormazionePrimaTranche") & " , " & _
                         " OreFormazioneSecondaTranche=" & IIf(OreFormazioneST = "", "Null", OreFormazioneST) & "" & _
                         " Where CodiceVolontario ='" & myDS.Tables(0).Rows(i).Item("CodiceVolontario") & "' "

                MyCommand.CommandText = strsql
                MyCommand.ExecuteNonQuery()
            Next

            MyTransaction.Commit()

        Catch exc As Exception
            MyTransaction.Rollback(Session("IdEnte") & "_" & Session("Utente"))
            swErr = True
        End Try

        MyCommand.Dispose()

        If swErr = False Then
            'Esito positivo
            lblEsito.Text = "Operazione di inserimento dei dati effettuata con successo."

            lblEsito.Visible = True
            'imgAnnulla.ImageUrl = "images/chiudi.jpg"

            CmdConferma.Visible = False
            'TestoSu.Visible = False
            'lblTestoSu1.Visible = False
            'imgConferma.ToolTip = "Vai al questionario"
            'imgConferma.ImageUrl = "images/elabora.jpg"
        Else
            'Errore Insert
            lblEsito.Text = "Errore durante l'operazione di inserimento dei dati."
            lblEsito.Visible = True
            'imgAnnulla.ImageUrl = "images/annulla.jpg"
        End If

        CancellaTabellaTemp()
        'End If
    End Sub

    Private Sub CmdChiudi_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdChiudi.Click
        CancellaTabellaTemp()
        Response.Redirect("WfrmImportOreVolontari.aspx")
    End Sub

End Class