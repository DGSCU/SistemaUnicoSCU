Imports System.Collections
Imports System.IO
Public Class WfrmRisultatoImportCorsoOlp
    Inherits System.Web.UI.Page
    Dim MyCommand As SqlClient.SqlCommand
    Dim DefArr As String()
    Dim ArrAggiorna As String()
    'Dim ArrTel As String()
    Dim ArrDatePreviste As String()
    Dim dtsLocal As DataSet
    Dim codRif As String
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Session("LogIn") Is Nothing Then
            If Session("LogIn") = False Then 'verifico validità log-in
                Response.Redirect("LogOn.aspx")
            End If
        Else
            Response.Redirect("LogOn.aspx")
        End If

        If Page.IsPostBack = False Then

            'dtgCSV.Columns("CustomerName").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
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

    Protected Sub CmdConferma_Click(sender As Object, e As EventArgs) Handles CmdConferma.Click
        Dim MyTransaction As System.Data.SqlClient.SqlTransaction
        Dim swErr As Boolean
        MyCommand = New SqlClient.SqlCommand
        MyCommand.Connection = Session("conn")
        CmdConferma.Visible = False

        Try

 
            MyTransaction = Session("conn").BeginTransaction(Session("IdEnte") & "_" & Session("Utente"))
            MyCommand.Transaction = MyTransaction

            ScriviCorso()
            ScriviCorsoDettaglio()
          

            MyTransaction.Commit()




        Catch exc As Exception

            MyTransaction.Rollback(Session("IdEnte") & "_" & Session("Utente"))
            swErr = True

        End Try

        MyCommand.Dispose()

        If swErr = False Then
           
                'Esito positivo
            'lblEsito.Text = "Operazione di inserimento dei dati effettuata con successo."
            'lblEsito.Visible = True
            'CmdChiudi.Visible = True
            lblCodRif.Visible = True
            codiciAssegnati.Visible = True
            lblCodRif.Text = "IL CODICE DI RIFERIMENTO DEL CORSO E'" + " " + codRif
        Else
            'Errore Insert
            lblEsito.Text = "Errore durante l'operazione di inserimento dei dati."
            lblEsito.Visible = True
            'CmdChiudi.Visible = True
        End If

        CancellaTabellaTemp()


    End Sub

    Protected Sub CmdChiudi_Click(sender As Object, e As EventArgs) Handles CmdChiudi.Click
        CancellaTabellaTemp()
        Response.Redirect("wfrmImportCorsoOLP.aspx")
    End Sub
    Private Sub CancellaTabellaTemp()
        Dim strSql As String
        Dim cmdCanTempTable As SqlClient.SqlCommand

        Try
            '--- CANCELLO TAB TEMPORANEA
            strSql = "DROP TABLE [#TEMP_CORSO_OLP]"

            cmdCanTempTable = New SqlClient.SqlCommand
            cmdCanTempTable.CommandText = strSql
            cmdCanTempTable.Connection = Session("conn")
            cmdCanTempTable.ExecuteNonQuery()
        Catch e As Exception

        End Try

        cmdCanTempTable.Dispose()
    End Sub

    Private Sub CaricaGriglia()
        Dim dtCSV As DataTable = New DataTable
        Dim rwCSV As DataRow
        Dim clCSV As DataColumn

        'Dim strSql As String
        'Dim i As Integer

        'Dim TmpArr() As String

        Dim Reader As StreamReader
        Dim xLinea As String


        clCSV = New DataColumn
        clCSV.DataType = System.Type.GetType("System.String")
        clCSV.ColumnName = "Note"
        clCSV.Caption = "Note"
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
        clCSV.ColumnName = "Ente Appartenenza"
        clCSV.Caption = "Ente Appartenenza"
        clCSV.ReadOnly = False
        clCSV.Unique = False
        dtCSV.Columns.Add(clCSV)

        clCSV = New DataColumn
        clCSV.DataType = System.Type.GetType("System.String")
        clCSV.ColumnName = "Luogo Svolgimento"
        clCSV.Caption = "Luogo Svolgimento"
        clCSV.ReadOnly = False
        clCSV.Unique = False
        dtCSV.Columns.Add(clCSV)

        clCSV = New DataColumn
        clCSV.DataType = System.Type.GetType("System.String")
        clCSV.ColumnName = "Data Svolgimento Corso"
        clCSV.Caption = "Data Svolgimento Corso"
        clCSV.ReadOnly = False
        clCSV.Unique = False
        dtCSV.Columns.Add(clCSV)


        clCSV = New DataColumn
        clCSV.DataType = System.Type.GetType("System.String")
        clCSV.ColumnName = "Numero Ore"
        clCSV.Caption = "Numero Ore"
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
            Else
                rwCSV(1) = DefArr(1)
                rwCSV(2) = DefArr(2)
                rwCSV(3) = DefArr(3)
                rwCSV(4) = DefArr(4)
                rwCSV(5) = DefArr(5)
                rwCSV(6) = DefArr(6)

                'rwCSV(1) = DefArr(1)
                'If UBound(DefArr) = 1 Then
                '    rwCSV(2) = vbNullString
                'Else
                '    rwCSV(2) = DefArr(2)
                '    If UBound(DefArr) > 3 Then
                '        rwCSV(3) = DefArr(4)
                '    Else
                '        rwCSV(3) = vbNullString
                '    End If
                'End If
            End If

            'vado a leggere 

            dtCSV.Rows.Add(rwCSV)
            xLinea = Reader.ReadLine()
        End While

        dtgCSV.DataSource = dtCSV


        dtgCSV.DataBind()


    End Sub

    'OK 1
    Private Function CreaArray(ByVal pLinea As String) As String()
        Dim TmpArr As String()

        Dim i As Integer
        Dim x As Integer
        Dim strValore As String

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
            
                strValore = DefArr(UBound(DefArr)) & Mid(TmpArr(x), 1, Len(TmpArr(x)) - 1)
               
                
                DefArr(UBound(DefArr)) = strValore
                i = x
            Else
                strValore = TmpArr(i)
                DefArr(UBound(DefArr)) = strValore
            End If
        Next

        CreaArray = DefArr

    End Function
    Private Sub ScriviCorso()

        Dim strsql As String


        strsql = "INSERT INTO CorsiFormazioneOLP " & _
                 "(IdEnte, " & _
                 "StatoRichiesta, " & _
                 "UsernameRichiesta, " & _
                 "DataRichiesta, " & _
                 "UsernameValutazione, " & _
                 "DataValutazione " & _
                  ") " & _
                     "values " & _
                     "('" & Session("IdEnte") & "', " & _
                     "'" & 1 & "', " & _
                     "'" & Session("Utente") & "', " & _
                     "getDate(), " & _
                     "null, " & _
                     "null) "
        MyCommand.CommandText = strsql
        MyCommand.ExecuteNonQuery()


    End Sub

    Private Sub ScriviCorsoDettaglio()
        Dim cmdGraduatorie As SqlClient.SqlCommand
        Dim strsql As String


        Dim newID As Integer
        strsql = "select scope_identity() as id"
        MyCommand.CommandText = strsql
        newID = MyCommand.ExecuteScalar()
        strsql = ""
       

        strsql = "INSERT INTO CorsiFormazioneOLPDettaglio " & _
                 "(IdCorsoFormazioneOLP, " & _
                 "Cognome, " & _
                 "Nome, " & _
                 "EnteRiferimento, " & _
                 "LuogoSvolgimento, " & _
                 "DataSvolgimentoCorso, " & _
                 "NumeroOre) " & _
                     "select" & _
                     " " & newID & ", " & _
                     " upper(Cognome), " & _
                     " upper(Nome), " & _
                     " upper(EnteRiferimento), " & _
                     " upper(LuogoSvolgimento), " & _
                     " DataSvolgimentoCorso, " & _
                     " NumeroOre" & _
         " FROM #TEMP_CORSO_OLP "

        MyCommand.CommandText = strsql
        MyCommand.ExecuteNonQuery()

        codRif = newID
    End Sub
End Class