Imports System.IO

Public Class WfrmRisultatoImportProgettiFormazione
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

        'Inserire qui il codice utente necessario per inizializzare la pagina
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
        clCSV.ColumnName = "Codice Progetto"
        clCSV.Caption = "Codice Progetto"
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
        clCSV.ColumnName = "Bando"
        clCSV.Caption = "Bando"
        clCSV.ReadOnly = False
        clCSV.Unique = False
        dtCSV.Columns.Add(clCSV)

        clCSV = New DataColumn
        clCSV.DataType = System.Type.GetType("System.String")
        clCSV.ColumnName = "Numero Volontari"
        clCSV.Caption = "Numero Volontari"
        clCSV.ReadOnly = False
        clCSV.Unique = False
        dtCSV.Columns.Add(clCSV)

        clCSV = New DataColumn
        clCSV.DataType = System.Type.GetType("System.String")
        clCSV.ColumnName = "Data Avvio Corso"
        clCSV.Caption = "Data Avvio Corso"
        clCSV.ReadOnly = False
        clCSV.Unique = False
        dtCSV.Columns.Add(clCSV)

        clCSV = New DataColumn
        clCSV.DataType = System.Type.GetType("System.String")
        clCSV.ColumnName = "Data Fine Corso"
        clCSV.Caption = "Data Fine Corso"
        clCSV.ReadOnly = False
        clCSV.Unique = False
        dtCSV.Columns.Add(clCSV)

        clCSV = New DataColumn
        clCSV.DataType = System.Type.GetType("System.String")
        clCSV.ColumnName = "Sede Svolgimento Corso"
        clCSV.Caption = "Sede Svolgimento Corso"
        clCSV.ReadOnly = False
        clCSV.Unique = False
        dtCSV.Columns.Add(clCSV)

        clCSV = New DataColumn
        clCSV.DataType = System.Type.GetType("System.String")
        clCSV.ColumnName = "Riferimento"
        clCSV.Caption = "Riferimento"
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
                rwCSV(8) = vbNullString
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
                            rwCSV(4) = DefArr(4)
                            If UBound(DefArr) = 4 Then
                                rwCSV(5) = vbNullString
                            Else
                                rwCSV(5) = DefArr(5)
                                If UBound(DefArr) = 5 Then
                                    rwCSV(6) = vbNullString
                                Else
                                    rwCSV(6) = DefArr(6)
                                    If UBound(DefArr) = 6 Then
                                        rwCSV(7) = vbNullString
                                    Else
                                        rwCSV(7) = DefArr(7)
                                        If UBound(DefArr) = 7 Then
                                            rwCSV(8) = vbNullString
                                        Else

                                            rwCSV(8) = DefArr(UBound(DefArr)) 'DefArr(16)
                                        End If
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
        clCSV.ColumnName = "Bando"
        clCSV.Caption = "Bando"
        clCSV.ReadOnly = False
        clCSV.Unique = False
        dtCSV.Columns.Add(clCSV)

        clCSV = New DataColumn
        clCSV.DataType = System.Type.GetType("System.String")
        clCSV.ColumnName = "Numero Volontari"
        clCSV.Caption = "NumeroVolontari"
        clCSV.ReadOnly = False
        clCSV.Unique = False
        dtCSV.Columns.Add(clCSV)

        clCSV = New DataColumn
        clCSV.DataType = System.Type.GetType("System.String")
        clCSV.ColumnName = "Data Avvio Corso Prima Tranche"
        clCSV.Caption = "DataAvvioCorsoPrimaTranche"
        clCSV.ReadOnly = False
        clCSV.Unique = False
        dtCSV.Columns.Add(clCSV)

        clCSV = New DataColumn
        clCSV.DataType = System.Type.GetType("System.String")
        clCSV.ColumnName = "Data Fine Corso Prima Tranche"
        clCSV.Caption = "DataFineCorsoPrimaTranche"
        clCSV.ReadOnly = False
        clCSV.Unique = False
        dtCSV.Columns.Add(clCSV)

        clCSV = New DataColumn
        clCSV.DataType = System.Type.GetType("System.String")
        clCSV.ColumnName = "Sede Svolgimento Corso Prima Tranche"
        clCSV.Caption = "SedeSvolgimentoCorsoPrimaTranche"
        clCSV.ReadOnly = False
        clCSV.Unique = False
        dtCSV.Columns.Add(clCSV)

        clCSV = New DataColumn
        clCSV.DataType = System.Type.GetType("System.String")
        clCSV.ColumnName = "Riferimento Prima Tranche"
        clCSV.Caption = "RiferimentoPrimaTranche"
        clCSV.ReadOnly = False
        clCSV.Unique = False
        dtCSV.Columns.Add(clCSV)

        clCSV = New DataColumn
        clCSV.DataType = System.Type.GetType("System.String")
        clCSV.ColumnName = "Data Avvio Corso Seconda Tranche"
        clCSV.Caption = "DataAvvioCorsoSecondaTranche"
        clCSV.ReadOnly = False
        clCSV.Unique = False
        dtCSV.Columns.Add(clCSV)

        clCSV = New DataColumn
        clCSV.DataType = System.Type.GetType("System.String")
        clCSV.ColumnName = "Data Fine Corso Seconda Tranche"
        clCSV.Caption = "DataFineCorsoSecondaTranche"
        clCSV.ReadOnly = False
        clCSV.Unique = False
        dtCSV.Columns.Add(clCSV)

        clCSV = New DataColumn
        clCSV.DataType = System.Type.GetType("System.String")
        clCSV.ColumnName = "Sede Svolgimento Corso Seconda Tranche"
        clCSV.Caption = "SedeSvolgimentoCorsoSecondaTranche"
        clCSV.ReadOnly = False
        clCSV.Unique = False
        dtCSV.Columns.Add(clCSV)

        clCSV = New DataColumn
        clCSV.DataType = System.Type.GetType("System.String")
        clCSV.ColumnName = "Riferimento Seconda Tranche"
        clCSV.Caption = "RiferimentoSecondaTranche"
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
                rwCSV(8) = vbNullString
                rwCSV(9) = vbNullString
                rwCSV(10) = vbNullString
                rwCSV(11) = vbNullString
                rwCSV(12) = vbNullString
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
                            rwCSV(4) = DefArr(4)
                            If UBound(DefArr) = 4 Then
                                rwCSV(5) = vbNullString
                            Else
                                rwCSV(5) = DefArr(5)
                                If UBound(DefArr) = 5 Then
                                    rwCSV(6) = vbNullString
                                Else
                                    rwCSV(6) = DefArr(6)
                                    If UBound(DefArr) = 6 Then
                                        rwCSV(7) = vbNullString
                                    Else
                                        ''''
                                        rwCSV(7) = DefArr(7)
                                        If UBound(DefArr) = 7 Then
                                            rwCSV(8) = vbNullString
                                        Else

                                            rwCSV(8) = DefArr(8)
                                            If UBound(DefArr) = 8 Then
                                                rwCSV(9) = vbNullString
                                            Else

                                                rwCSV(9) = DefArr(9)
                                                If UBound(DefArr) = 9 Then
                                                    rwCSV(10) = vbNullString
                                                Else

                                                    rwCSV(10) = DefArr(10)
                                                    If UBound(DefArr) = 10 Then
                                                        rwCSV(11) = vbNullString
                                                    Else

                                                        rwCSV(11) = DefArr(11)
                                                        If UBound(DefArr) = 11 Then
                                                            rwCSV(12) = vbNullString
                                                        Else
                                                            rwCSV(12) = DefArr(UBound(DefArr)) 'DefArr(16)
                                                        End If
                                                    End If
                                                End If
                                            End If
                                        End If
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

    Private Sub CmdConferma_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdConferma.Click
        If Request.QueryString("TipoFormazioneGenerale") = 1 Then
            UpdateFormazioneUnicaTranche()
        Else
            UpdateFormazioneDueTranche()
        End If
    End Sub

    Sub UpdateFormazioneUnicaTranche()
        Dim MyTransaction As System.Data.SqlClient.SqlTransaction
        Dim swErr As Boolean
        Dim strsql As String
        Dim strsql1 As String
        Dim IdAttività As Integer
        Dim i As Integer
        Dim myDS As New DataSet
        Dim dtrgenerico As SqlClient.SqlDataReader

        CmdConferma.Visible = False

        Try

            strsql = "Select * From [#TEMP_FORMAZIONE_VOLONATARIO]"
            myDS = ClsServer.DataSetGenerico(strsql, Session("conn"))

            MyCommand = New SqlClient.SqlCommand
            MyCommand.Connection = Session("conn")

            MyTransaction = Session("conn").BeginTransaction(Session("IdEnte") & "_" & Session("Utente"))

            MyCommand.Transaction = MyTransaction           'select

            For i = 0 To myDS.Tables(0).Rows.Count - 1

                ''''''--------------------Recupero idattività ---------------------------------

                '''''strsql1 = "Select IdAttivita from Attività where CodiceEnte='" & myDS.Tables(0).Rows(i).Item("CodiceProgetto") & "'"
                '''''dtrgenerico = ClsServer.CreaDatareader(strsql1, Session("conn"))
                '''''dtrgenerico.Read()
                '''''IdAttività = dtrGenerico("IdAttività")

                ''''''-----------------------------------------------------
                strsql = ""
                strsql = "Update AttivitàFormazioneGenerale Set " & _
                         " DataInizioCorso='" & myDS.Tables(0).Rows(i).Item("DataAvvio") & "', " & _
                         " DataFineCorso='" & myDS.Tables(0).Rows(i).Item("DataFine") & "', " & _
                         " SedeCorso='" & Replace(myDS.Tables(0).Rows(i).Item("SedeSvolgimento"), "'", "''") & "', " & _
                         " RiferimentoCorso='" & Replace(myDS.Tables(0).Rows(i).Item("Riferimento"), "'", "''") & "', " & _
                         " DataUltimaPianificazione=getdate(), UserNamePianificazione='" & Session("Utente") & "' " & _
                         " from attivitàformazionegenerale a inner join attività b on a.idattività = b.idattività Where b.codiceente ='" & myDS.Tables(0).Rows(i).Item("CodiceProgetto") & "' "

                MyCommand.CommandText = strsql
                MyCommand.ExecuteNonQuery()

                If Not dtrgenerico Is Nothing Then
                    dtrgenerico.Close()
                    dtrgenerico = Nothing
                End If

                strsql = ""
                strsql = "Insert into CronologiaPianificazioneFormazione (IdAttività,DataUltimaPianificazione,UsernamePianificazione,DataInizioCorso,DataFineCorso,SedeCorso,RiferimentoCorso) select  idattività,getdate(),'" & Session("Utente") & "','" & myDS.Tables(0).Rows(i).Item("DataAvvio") & "','" & myDS.Tables(0).Rows(i).Item("DataFine") & "','" & Replace(myDS.Tables(0).Rows(i).Item("SedeSvolgimento"), "'", "''") & "','" & Replace(myDS.Tables(0).Rows(i).Item("Riferimento"), "'", "''") & "' from attività where codiceente = '" & myDS.Tables(0).Rows(i).Item("CodiceProgetto") & "'"
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

    Sub UpdateFormazioneDueTranche()
        Dim MyTransaction As System.Data.SqlClient.SqlTransaction
        Dim swErr As Boolean
        Dim strsql As String
        Dim strsql1 As String
        Dim IdAttività As Integer
        Dim i As Integer
        Dim myDS As New DataSet
        Dim dtrgenerico As SqlClient.SqlDataReader

        CmdConferma.Visible = False

        Try

            strsql = "Select * From [#TEMP_FORMAZIONE_VOLONATARIO]"
            myDS = ClsServer.DataSetGenerico(strsql, Session("conn"))

            MyCommand = New SqlClient.SqlCommand
            MyCommand.Connection = Session("conn")

            MyTransaction = Session("conn").BeginTransaction(Session("IdEnte") & "_" & Session("Utente"))

            MyCommand.Transaction = MyTransaction           'select

            For i = 0 To myDS.Tables(0).Rows.Count - 1

                strsql = ""
                strsql = "Update AttivitàFormazioneGenerale Set " & _
                         " DataInizioCorso='" & myDS.Tables(0).Rows(i).Item("DataAvvio") & "', " & _
                         " DataFineCorso='" & myDS.Tables(0).Rows(i).Item("DataFine") & "', " & _
                         " SedeCorso='" & Replace(myDS.Tables(0).Rows(i).Item("SedeSvolgimento"), "'", "''") & "', " & _
                         " RiferimentoCorso='" & Replace(myDS.Tables(0).Rows(i).Item("Riferimento"), "'", "''") & "', " & _
                         " DataInizioCorsoSecondaTranche='" & myDS.Tables(0).Rows(i).Item("DataAvvioTranche") & "', " & _
                         " DataFineCorsoSecondaTranche='" & myDS.Tables(0).Rows(i).Item("DataFineTranche") & "', " & _
                         " SedeCorsoSecondaTranche='" & Replace(myDS.Tables(0).Rows(i).Item("SedeSvolgimentoTranche"), "'", "''") & "', " & _
                         " RiferimentoCorsoSecondaTranche='" & Replace(myDS.Tables(0).Rows(i).Item("RiferimentoTranche"), "'", "''") & "', " & _
                         " DataUltimaPianificazione=getdate(), UserNamePianificazione='" & Session("Utente") & "' " & _
                         " from attivitàformazionegenerale a inner join attività b on a.idattività = b.idattività Where b.codiceente ='" & myDS.Tables(0).Rows(i).Item("CodiceProgetto") & "' "

                MyCommand.CommandText = strsql
                MyCommand.ExecuteNonQuery()

                If Not dtrgenerico Is Nothing Then
                    dtrgenerico.Close()
                    dtrgenerico = Nothing
                End If

                strsql = ""
                strsql = "Insert into CronologiaPianificazioneFormazione (IdAttività,DataUltimaPianificazione,UsernamePianificazione,DataInizioCorso,DataFineCorso,SedeCorso,RiferimentoCorso) select  idattività,getdate(),'" & Session("Utente") & "','" & myDS.Tables(0).Rows(i).Item("DataAvvio") & "','" & myDS.Tables(0).Rows(i).Item("DataFine") & "','" & Replace(myDS.Tables(0).Rows(i).Item("SedeSvolgimento"), "'", "''") & "','" & Replace(myDS.Tables(0).Rows(i).Item("Riferimento"), "'", "''") & "' from attività where codiceente = '" & myDS.Tables(0).Rows(i).Item("CodiceProgetto") & "'"
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
            strSql = "DROP TABLE [#TEMP_FORMAZIONE_VOLONATARIO]"

            cmdCanTempTable = New SqlClient.SqlCommand
            cmdCanTempTable.CommandText = strSql
            cmdCanTempTable.Connection = Session("conn")
            cmdCanTempTable.ExecuteNonQuery()
        Catch e As Exception

        End Try

        cmdCanTempTable.Dispose()

        Session("DtbRicerca") = Nothing
    End Sub

    Private Sub CmdChiudi_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdChiudi.Click
        CancellaTabellaTemp()
        Response.Redirect("WfrmImportPianificazioneCorsi.aspx")
    End Sub

End Class