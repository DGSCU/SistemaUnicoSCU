Imports System.Collections
Imports System.IO
Public Class wfrmRisultatoImportAssenze
    Inherits System.Web.UI.Page
    Dim MyCommand As SqlClient.SqlCommand
    Dim DefArr As String()
    Dim ArrAggiorna As String()
    'Dim ArrTel As String()
    Dim ArrDatePreviste As String()
    Dim dtsLocal As DataSet

    '--- CONVERSIONE DA HELIOS 23/01/2015  -IACOBUCCI 
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
        clCSV.ColumnName = "CodiceVolontario"
        clCSV.Caption = "CodiceVolontario"
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
        clCSV.ColumnName = "Mese"
        clCSV.Caption = "Mese"
        clCSV.ReadOnly = False
        clCSV.Unique = False
        dtCSV.Columns.Add(clCSV)

        clCSV = New DataColumn
        clCSV.DataType = System.Type.GetType("System.String")
        clCSV.ColumnName = "Anno"
        clCSV.Caption = "Anno"
        clCSV.ReadOnly = False
        clCSV.Unique = False
        dtCSV.Columns.Add(clCSV)

        clCSV = New DataColumn
        clCSV.DataType = System.Type.GetType("System.String")
        clCSV.ColumnName = "Giorni"
        clCSV.Caption = "Giorni"
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
                            rwCSV(4) = DefArr(4)
                            If UBound(DefArr) = 4 Then
                                rwCSV(5) = vbNullString
                            Else
                                rwCSV(5) = DefArr(5)
                                If UBound(DefArr) = 5 Then
                                    rwCSV(6) = vbNullString
                                Else
                                    rwCSV(6) = DefArr(6)
                                End If
                            End If
                        End If
                    End If
                End If
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

    Private Sub ScriviAssenze()
        Dim strsql As String

        strsql = "INSERT INTO EntitàAssenze " & _
                 "(IdEntità, " & _
                 "IdCausale, " & _
                 "Anno, " & _
                 "Mese, " & _
                 "Giorni, " & _
                 "Note, " & _
                 "Stato, " & _
                 "UsernameInseritore, " & _
                 "DataCreazione) " & _
                 "SELECT " & _
                 "b.IdEntità, " & _
                 "a.Causale, " & _
                 "a.Anno, " & _
                 "a.Mese, " & _
                 "a.Giorni, " & _
                 "a.Note, " & _
                 "1, " & _
                 "'" & Session("Utente") & "', " & _
                 "getdate() " & _
                 "FROM #TEMP_ASSENZE_VOLONTARI a " & _
                 "INNER JOIN Entità b ON b.CodiceVolontario = a.CodiceVolontario"


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
            'AggiornaTabellaTemporaneaTelefono()
            'RecuperaDatiPerGraduatoria()
            'RecuperaDatePreviste()

            MyTransaction = Session("conn").BeginTransaction(Session("IdEnte") & "_" & Session("Utente"))
            MyCommand.Transaction = MyTransaction

            ScriviAssenze()

            MyTransaction.Commit()
            ControlloAssenzeEccedenti()
        Catch exc As Exception

            MyTransaction.Rollback(Session("IdEnte") & "_" & Session("Utente"))
            swErr = True

        End Try

        MyCommand.Dispose()

      If swErr = False Then
            'Esito positivo
            lblEsito.Text = "Operazione di inserimento dei dati effettuata con successo."
            lblEsito.Visible = True

        Else
            'Errore Insert
            lblEsito.Text = "Errore durante l'operazione di inserimento dei dati."
            lblEsito.Visible = True

        End If

        CancellaTabellaTemp()
    End Sub

    Private Sub ControlloAssenzeEccedenti()
        'REALIZZATA DA: SIMONA CORDELLA 
        'DATA REALIZZAZIONE: 28/02/2014
        'FUNZIONALITA': Ciclo la tabella temporanea e richiamo la store di NotificaAssenzeEccedenze


        Dim strsql As String

        Dim i As Integer = 0
        Dim dtAss As DataTable

        strsql = " select distinct b.IdEntità,a.Causale "
        strsql &= " FROM #TEMP_ASSENZE_VOLONTARI a INNER JOIN Entità b ON b.CodiceVolontario = a.CodiceVolontario "
        dtAss = ClsServer.CreaDataTable(strsql, False, Session("conn"))
        'aggiorno lo stato del progetto
        For i = 0 To dtAss.Rows.Count - 1
            NotificaAssenzeEccedenti(dtAss.Rows(i).Item("IdEntità"), dtAss.Rows(i).Item("Causale"))
        Next


    End Sub

    Private Sub NotificaAssenzeEccedenti(ByVal IdEntita As Integer, ByVal IdCausale As Integer)
        'REALIZZATA DA: SIMONA CORDELLA 
        'DATA REALIZZAZIONE: 28/02/2014
        'FUNZIONALITA': Richiama la store per la notifica delle aasenze eccedenti(invia email)
        Dim sqlCMD As New SqlClient.SqlCommand
        Dim strNomeStore As String = "[SP_NOTIFICA_ASSENZE_ECCEDENTI]"

        Try
            sqlCMD = New SqlClient.SqlCommand(strNomeStore, CType(Session("conn"), SqlClient.SqlConnection))
            sqlCMD.CommandType = CommandType.StoredProcedure
            sqlCMD.Parameters.Add("@IdEntita", SqlDbType.Int).Value = IdEntita
            sqlCMD.Parameters.Add("@IDCausale", SqlDbType.Int).Value = IdCausale

            Dim sparam1 As SqlClient.SqlParameter
            sparam1 = New SqlClient.SqlParameter
            sparam1.ParameterName = "@Esito"
            sparam1.SqlDbType = SqlDbType.Int
            sparam1.Direction = ParameterDirection.Output
            sqlCMD.Parameters.Add(sparam1)

            sqlCMD.ExecuteScalar()
            'Dim int As 
            'str = sqlCMD.Parameters("@Esito").Value
            'Return str
        Catch ex As Exception
            Response.Write(ex.Message.ToString())
            Exit Sub
        End Try
    End Sub

    Protected Sub CmdChiudi_Click(sender As Object, e As EventArgs) Handles CmdChiudi.Click
        CancellaTabellaTemp()
        Response.Redirect("wfrmImportAssenze.aspx")
    End Sub

    Private Sub dtgCSV_PreRender(sender As Object, e As System.EventArgs) Handles dtgCSV.PreRender
        'dtgCSV.Columns(0).ItemStyle.Width = Unit.Pixel(250)
        'dtgCSV.Columns(1).ItemStyle.Width = Unit.Pixel(250)
        'dtgCSV.Columns(2).ItemStyle.Width = Unit.Pixel(250)
        'dtgCSV.Columns(3).ItemStyle.Width = Unit.Pixel(250)
    End Sub
End Class