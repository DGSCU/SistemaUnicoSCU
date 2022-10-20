Imports System.Collections
Imports System.IO
Public Class WfrmRisultatoImportIbanUNSC
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

            CmdConferma.Visible = False
            If lblNoteEnte.Text <> "Note ATTENZIONE! File non valido." Then
                lblTotali.Text = "Sono state inviate " & Request.QueryString("Tot") & " righe. " & _
                                 Request.QueryString("TotOk") & " con esito positivo. " & Request.QueryString("TotKo") & " con esito negativo."

                hlDownLoad.NavigateUrl = "download\" & Request.QueryString("NomeFile") & ".CSV"

                If CInt(Request.QueryString("TotKo")) = 0 Then
                    AvvisoConferma.Visible = True
                    avviso.Visible = True
                    CmdConferma.Visible = True
                    testoavviso.InnerHtml = "LA VERIFICA DEI DATI IMMESSI NEL FILE CSV RISULTA CORRETTA. PER SALVARE DEFINITIVAMENTE I DATI PREMERE IL TASTO CONFERMA."
                End If
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
        clCSV.ColumnName = "CodiceIban"
        clCSV.Caption = "CodiceIban"
        clCSV.ReadOnly = False
        clCSV.Unique = False
        dtCSV.Columns.Add(clCSV)

        clCSV = New DataColumn
        clCSV.DataType = System.Type.GetType("System.String")
        clCSV.ColumnName = "BicSwift"
        clCSV.Caption = "BicSwift"
        clCSV.ReadOnly = False
        clCSV.Unique = False
        dtCSV.Columns.Add(clCSV)

        Reader = New StreamReader(Server.MapPath("download") & "\" & Request.QueryString("NomeFile") & ".CSV")
        ' Lettura prima linea - Denominanzione
        xLinea = Reader.ReadLine()
        If xLinea = "Note;ATTENZIONE! File non valido." Then
            ArrEnte = CreaArray(xLinea)
            lblNoteEnte.Text = ArrEnte(0) & " " & ArrEnte(1)
        Else
            If xLinea <> vbNullString Then
                ArrEnte = CreaArray(xLinea)
                If UBound(ArrEnte) < 1 Then
                    lblDenominazioneEnte.Text = ArrEnte(0)
                Else
                    lblDenominazioneEnte.Text = ArrEnte(0) & " " & ArrEnte(1)
                End If
            End If
        End If

        ' Lettura seconda linea - Codice Ente
        xLinea = Reader.ReadLine()
        If xLinea = "Note;ATTENZIONE! File non valido." Then
            ArrEnte = CreaArray(xLinea)
            lblNoteEnte.Text = ArrEnte(0) & " " & ArrEnte(1)
        Else
            If xLinea <> vbNullString Then
                ArrEnte = CreaArray(xLinea)
                If UBound(ArrEnte) < 1 Then
                    lblCodiceEnte.Text = ArrEnte(0)
                Else
                    lblCodiceEnte.Text = ArrEnte(0) & " " & ArrEnte(1)
                End If
            End If
        End If

        ' Lettura terza Linea - Note file
        xLinea = Reader.ReadLine()
        If xLinea <> vbNullString Then
            ArrEnte = CreaArray(xLinea)
            lblNoteEnte.Text = ArrEnte(0) & " " & ArrEnte(1)
        End If

        ' Lettura quarta Linea - Intestazioni colonne
        xLinea = Reader.ReadLine()
        If xLinea = "Note;ATTENZIONE! File non valido." Then
            ArrEnte = CreaArray(xLinea)
            lblNoteEnte.Text = ArrEnte(0) & " " & ArrEnte(1)
        End If

        If lblNoteEnte.Text = "Note ATTENZIONE! File non valido." Then
            hlDownLoad.Visible = False
            hlErrore.Visible = False
        End If

        'primo volontario
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
            Else
                rwCSV(1) = DefArr(1)
                If UBound(DefArr) = 1 Then
                    rwCSV(2) = vbNullString
                Else
                    rwCSV(1) = DefArr(1)
                    If UBound(DefArr) = 1 Then
                        rwCSV(2) = vbNullString
                    Else
                        rwCSV(2) = DefArr(2)
                        If UBound(DefArr) = 3 Then
                            rwCSV(3) = vbNullString
                        Else
                            rwCSV(3) = DefArr(3)
                            If UBound(DefArr) = 4 Then
                                rwCSV(4) = vbNullString
                            Else
                                rwCSV(4) = UCase(DefArr(4))
                                If UBound(DefArr) > 4 Then
                                    rwCSV(5) = UCase(DefArr(5))
                                Else
                                    rwCSV(5) = vbNullString
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
        If UBound(TmpArr) = 3 Then
            ReDim Preserve DefArr(UBound(DefArr) + 2)
        End If
        If UBound(TmpArr) = 4 Then
            ReDim Preserve DefArr(UBound(DefArr) + 1)
        End If
        CreaArray = DefArr

    End Function

   

    Private Sub CancellaTabellaTemp()
        Dim strSql As String
        Dim cmdCanTempTable As SqlClient.SqlCommand

        Try
            '--- CANCELLO TAB TEMPORANEA
            strSql = "DROP TABLE [#TEMP_CODICI_IBAN]"

            cmdCanTempTable = New SqlClient.SqlCommand
            cmdCanTempTable.CommandText = strSql
            cmdCanTempTable.Connection = Session("conn")
            cmdCanTempTable.ExecuteNonQuery()
        Catch e As Exception

        End Try

        cmdCanTempTable.Dispose()
    End Sub

    Private Sub ScriviCodiciIban()
        Dim strsql As String

        strsql = "UPDATE Entità "
        strsql = strsql & " SET Entità.IBAN = UPPER(b.CodiceIban), "
        strsql = strsql & " Entità.BIC_SWIFT = UPPER(b.BicSwift) "
        strsql = strsql & "FROM Entità "
        strsql = strsql & "INNER JOIN #TEMP_CODICI_IBAN b on Entità.CodiceVolontario = b.CodiceVolontario"


        MyCommand.CommandText = strsql
        MyCommand.ExecuteNonQuery()

    End Sub

    Private Sub ScriviCronogliaEntitaDettagli()
        Dim strsql As String

        strsql = "select B.IDENTITà, max(CASE WHEN a.DataFineValidità IS NULL THEN  b.DataUltimoStato ELSE  a.DataFineValidità END) as DIV"
        strsql = strsql & " into #tmpcrono "
        strsql = strsql & " from cronologiaentitàdettagli a "
        strsql = strsql & " right join entità b on a.identità = b.identità "
        strsql = strsql & " INNER JOIN  #TEMP_CODICI_IBAN C on B.CodiceVolontario = C.CodiceVolontario  "
        strsql = strsql & " GROUP BY B.IDENTITà"

        MyCommand.CommandText = strsql
        MyCommand.ExecuteNonQuery()

        strsql = "INSERT INTO CronologiaEntitàDettagli " & _
                "(IdEntità, " & _
                "Indirizzo, " & _
                "CAP, " & _
                "NumeroCivico, " & _
                "IdComuneResidenza, " & _
                "IdUfficioPostale, " & _
                "CodiceLibrettoPostale, " & _
                "Banca, " & _
                "CC, " & _
                "ABI, " & _
                "CAB, " & _
                "CIN, " & _
                "IdComuneDomicilio, " & _
                "IndirizzoDomicilio, " & _
                "NumeroCivicoDomicilio, " & _
                "CAPDomicilio, " & _
                "DataInizioValidità, " & _
                "DataFineValidità, " & _
                "UserNameUltimaModifica, " & _
                "DataUltimaModifica, " & _
                "IBAN, " & _
                "BIC_SWIFT ) " & _
                "SELECT " & _
                "Entità.IdEntità, " & _
                "Entità.Indirizzo, " & _
                "Entità.CAP, " & _
                "Entità.NumeroCivico, " & _
                "Entità.IdComuneResidenza, " & _
                "Entità.IdUfficioPostale, " & _
                "Entità.CodiceLibrettoPostale, " & _
                "Entità.Banca, " & _
                "Entità.CC, " & _
                "Entità.ABI, " & _
                "Entità.CAB, " & _
                "Entità.CIN, " & _
                "Entità.IdComuneDomicilio, " & _
                "Entità.IndirizzoDomicilio, " & _
                "Entità.NumeroCivicoDomicilio, " & _
                "Entità.CAPDomicilio, " & _
                " div, " & _
                "getdate(), '" & Session("Utente") & _
                "',getdate() " & _
                ",Entità.IBAN " & _
                ",Entità.BIC_SWIFT " & _
                "FROM Entità " & _
                "INNER JOIN #TEMP_CODICI_IBAN b on Entità.CodiceVolontario = b.CodiceVolontario " & _
                "inner join #tmpcrono c on entità.IDEntità = c.identità "
        '"LEFT JOIN CronologiaEntitàDettagli on Entità.IdEntità = CronologiaEntitàDettagli.IdEntità"
        'mod. il 03/06/2015 
        '' (select max(CASE WHEN a.DataFineValidità IS NULL THEN  b.DataUltimoStato ELSE  a.DataFineValidità END) from cronologiaentitàdettagli a right join entità b on a.identità = b.identità where b.identità = entità.identità) as DataInizioValidità
        MyCommand.CommandText = strsql
        MyCommand.ExecuteNonQuery()

        strsql = "DROP TABLE [#tmpcrono]"

        ' MyCommand = New SqlClient.SqlCommand
        MyCommand.CommandText = strsql
        ' MyCommand.Connection = Session("conn")
        MyCommand.ExecuteNonQuery()

    End Sub

    Protected Sub CmdConferma_Click(ByVal sender As Object, ByVal e As EventArgs) Handles CmdConferma.Click
        Dim MyTransaction As System.Data.SqlClient.SqlTransaction
        Dim swErr As Boolean
        MyCommand = New SqlClient.SqlCommand
        MyCommand.Connection = Session("conn")
        CmdConferma.Visible = False
        Try
            MyTransaction = Session("conn").BeginTransaction(Session("IdEnte") & "_" & Session("Utente"))
            MyCommand.Transaction = MyTransaction

            ScriviCronogliaEntitaDettagli()
            ScriviCodiciIban()

            MyTransaction.Commit()
            If Session("Sistema") = "Futuro" Then
                ControlloIbanModificati()
            End If

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
        Else
            'Errore Insert
            lblEsito.ForeColor = Drawing.Color.Red
            lblEsito.Text = "Errore durante l'operazione di inserimento dei dati."
            lblEsito.Visible = True
            'imgAnnulla.ImageUrl = "images/annulla.jpg"
        End If

        CancellaTabellaTemp()
    End Sub

    Protected Sub CmdChiudi_Click(ByVal sender As Object, ByVal e As EventArgs) Handles CmdChiudi.Click
        CancellaTabellaTemp()
        'If Request.QueryString("VengoDa") = Nothing Then
        '    Response.Redirect("wfrmImportLibrettiUNSC.aspx")
        'Else
        Response.Redirect("wfrmImportIbanEnti.aspx")
        'End If

    End Sub
    Private Sub ControlloIbanModificati()
        'REALIZZATA DA: SIMONA CORDELLA 
        'DATA REALIZZAZIONE: 19/05/2015
        'FUNZIONALITA': Ciclo la tabella temporanea e verifico se 
        Dim strsql As String
        Dim i As Integer = 0
        Dim dtIBan As DataTable

        strsql = " select distinct b.IdEntità  "
        strsql &= " FROM #TEMP_CODICI_IBAN a "
        strsql &= " INNER JOIN entità  e ON e.CodiceVolontario = a.CodiceVolontario "
        strsql &= " inner join CronologiaEntitàDettagli b on e.identità = b.identità "
        strsql &= " where e.iban<>b.iban and not b.iban is null "
        dtIBan = ClsServer.CreaDataTable(strsql, False, Session("conn"))
        'aggiorno lo stato del progetto
        For i = 0 To dtIBan.Rows.Count - 1
            NotificaModificaIban(dtIBan.Rows(i).Item("IdEntità"), Session("IDEnte"))
        Next
    End Sub

    Private Sub NotificaModificaIban(ByVal IdEntita As Integer, ByVal IdEnte As Integer)
        'REALIZZATA DA: SIMONA CORDELLA 
        'DATA REALIZZAZIONE: 19/05/2015
        'FUNZIONALITA': Richiama la store per la notifica della modifica dell'iban (invia email)
        Dim sqlCMD As New SqlClient.SqlCommand
        Dim strNomeStore As String = "[SP_NOTIFICA_MODIFICA_IBAN_VOLONTARI]"

        Try
            sqlCMD = New SqlClient.SqlCommand(strNomeStore, CType(Session("conn"), SqlClient.SqlConnection))
            sqlCMD.CommandType = CommandType.StoredProcedure
            sqlCMD.Parameters.Add("@IdEntita", SqlDbType.Int).Value = IdEntita
            sqlCMD.Parameters.Add("@IdEnte", SqlDbType.Int).Value = IdEnte

            Dim sparam1 As SqlClient.SqlParameter
            sparam1 = New SqlClient.SqlParameter
            sparam1.ParameterName = "@Esito"
            sparam1.SqlDbType = SqlDbType.Int
            sparam1.Direction = ParameterDirection.Output
            sqlCMD.Parameters.Add(sparam1)

            sqlCMD.ExecuteScalar()
   
        Catch ex As Exception
            Response.Write(ex.Message.ToString())
            Exit Sub
        End Try
    End Sub
End Class