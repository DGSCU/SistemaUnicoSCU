Imports System.Collections
Imports System.IO
Public Class WfrmRisultatoImportRiattivazione
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
        clCSV.ColumnName = "CodiceProgetto"
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
        clCSV.ColumnName = "CodiceSedeProgetto"
        clCSV.Caption = "CodiceSedeProgetto"
        clCSV.ReadOnly = False
        clCSV.Unique = False
        dtCSV.Columns.Add(clCSV)

        clCSV = New DataColumn
        clCSV.DataType = System.Type.GetType("System.String")
        clCSV.ColumnName = "NomeSedeProgetto"
        clCSV.Caption = "NomeSedeProgetto"
        clCSV.ReadOnly = False
        clCSV.Unique = False
        dtCSV.Columns.Add(clCSV)

        clCSV = New DataColumn
        clCSV.DataType = System.Type.GetType("System.String")
        clCSV.ColumnName = "StatoRiattivazione"
        clCSV.Caption = "StatoRiattivazione"
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
                rwCSV(4) = vbNullString
                rwCSV(5) = vbNullString

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
            strSql = "DROP TABLE [#TEMP_COVID19]"

            cmdCanTempTable = New SqlClient.SqlCommand
            cmdCanTempTable.CommandText = strSql
            cmdCanTempTable.Connection = Session("conn")
            cmdCanTempTable.ExecuteNonQuery()
        Catch e As Exception

        End Try

        cmdCanTempTable.Dispose()
    End Sub

    Private Sub ScriviCovid19()
        Dim strsql As String

        strsql = "INSERT INTO attivitàentisediattuazione_riattivazione " & _
                 "(IDAttivitàEnteSedeAttuazione, " & _
                 "IdStatoRiattivazione, " & _
                 "IdTipoModalitàServizio, " & _
                 "NumeroVolontariConsenso, " & _
                 "CodiceNuovaSedeAccreditata, " & _
                 "DescrizioneNuovaSede, " & _
                 "IdTipoAttivitàRimodulazione, " & _
                 "AltroAttivitàRimodulazione, " & _
                 "Username, " & _
                 "DataAggiornamento) " & _
                 "SELECT " & _
                 "b.IDAttivitàEnteSedeAttuazione, " & _
                 "d.IdStatoRiattivazione, " & _
                 "e.IdTipoModalitàServizio, " & _
                 "a.NumeroVolontariConConsenso, " & _
                 "a.CodiceNuovaSedeAccreditata, " & _
                 "a.NuovaSedeSvolgimentoNonAccreditata, " & _
                 "f.IdTipoAttivitàRimodulazione, " & _
                 "a.SpecificaAltro, " & _
                 "'" & Session("Utente") & "', " & _
                 "getdate() " & _
                 "FROM #TEMP_COVID19 a " & _
                 "INNER JOIN attivitàentisediattuazione b ON a.codicesedeprogetto = b.identesedeattuazione " & _
                 "INNER JOIN attività c on b.idattività = c.idattività and a.codiceprogetto = c.codiceente " & _
                 "INNER JOIN statiriattivazione d on a.StatoRiattivazione = d.codice " & _
                 "LEFT JOIN tipiModalitàServizio e on a.ModalitàServizio = e.codice " & _
                 "LEFT JOIN tipiattivitàrimodulazione f on a.CodiceAttivitàRimodulazione = f.codice and f.riattivazione = 1 " & _
                 "WHERE isnull(b.IdStatoRiattivazione,0) <> isnull(d.IdStatoRiattivazione,0) " & _
                 "or isnull(b.IdTipoModalitàServizio,0) <> isnull(e.IdTipoModalitàServizio,0) " & _
                 "or isnull(b.NumeroVolontariConsenso,-1) <> isnull(a.NumeroVolontariConConsenso,-1) " & _
                 "or isnull(b.CodiceNuovaSedeAccreditata,'--') <> isnull(a.CodiceNuovaSedeAccreditata,'--') " & _
                 "or isnull(b.DescrizioneNuovaSede,'--') <> isnull(a.NuovaSedeSvolgimentoNonAccreditata,'--') " & _
                 "or isnull(b.IdTipoAttivitàRimodulazione,0) <> isnull(f.IdTipoAttivitàRimodulazione,0) " & _
                 "or isnull(b.AltroAttivitàRimodulazione,'--') <> isnull(a.SpecificaAltro,'--') "
        MyCommand.CommandText = strsql
        MyCommand.ExecuteNonQuery()

        strsql = "UPDATE attivitàentisediattuazione SET " & _
         "IdStatoRiattivazione = d.IdStatoRiattivazione, " & _
         "IdTipoModalitàServizio = e.IdTipoModalitàServizio, " & _
         "NumeroVolontariConsenso = a.NumeroVolontariConConsenso, " & _
         "CodiceNuovaSedeAccreditata = a.CodiceNuovaSedeAccreditata, " & _
         "DescrizioneNuovaSede = a.NuovaSedeSvolgimentoNonAccreditata, " & _
         "IdTipoAttivitàRimodulazione = f.IdTipoAttivitàRimodulazione, " & _
         "AltroAttivitàRimodulazione = a.SpecificaAltro " & _
         "FROM #TEMP_COVID19 a " & _
         "INNER JOIN attivitàentisediattuazione b ON a.codicesedeprogetto = b.identesedeattuazione " & _
         "INNER JOIN attività c on b.idattività = c.idattività and a.codiceprogetto = c.codiceente " & _
         "INNER JOIN statiriattivazione d on a.StatoRiattivazione = d.codice " & _
         "LEFT JOIN tipiModalitàServizio e on a.ModalitàServizio = e.codice " & _
         "LEFT JOIN tipiattivitàrimodulazione f on a.CodiceAttivitàRimodulazione = f.codice and f.riattivazione = 1 " & _
            "WHERE isnull(b.IdStatoRiattivazione,0) <> isnull(d.IdStatoRiattivazione,0) " & _
            "or isnull(b.IdTipoModalitàServizio,0) <> isnull(e.IdTipoModalitàServizio,0) " & _
            "or isnull(b.NumeroVolontariConsenso,-1) <> isnull(a.NumeroVolontariConConsenso,-1) " & _
            "or isnull(b.CodiceNuovaSedeAccreditata,'--') <> isnull(a.CodiceNuovaSedeAccreditata,'--') " & _
            "or isnull(b.DescrizioneNuovaSede,'--') <> isnull(a.NuovaSedeSvolgimentoNonAccreditata,'--') " & _
            "or isnull(b.IdTipoAttivitàRimodulazione,0) <> isnull(f.IdTipoAttivitàRimodulazione,0) " & _
            "or isnull(b.AltroAttivitàRimodulazione,'--') <> isnull(a.SpecificaAltro,'--') "
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
            'AggiornaTabellaTemporaneaTelefono()
            'RecuperaDatiPerGraduatoria()
            'RecuperaDatePreviste()

            MyTransaction = Session("conn").BeginTransaction(Session("IdEnte") & "_" & Session("Utente"))
            MyCommand.Transaction = MyTransaction

            ScriviCovid19()

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

    'Private Sub ControlloAssenzeEccedenti()



    '    Dim strsql As String

    '    Dim i As Integer = 0
    '    Dim dtAss As DataTable

    '    strsql = " select distinct b.IdEntità,a.Causale "
    '    strsql &= " FROM #TEMP_COVID19 a INNER JOIN Entità b ON b.CodiceVolontario = a.CodiceVolontario "
    '    dtAss = ClsServer.CreaDataTable(strsql, False, Session("conn"))
    '    'aggiorno lo stato del progetto
    '    For i = 0 To dtAss.Rows.Count - 1
    '        NotificaAssenzeEccedenti(dtAss.Rows(i).Item("IdEntità"), dtAss.Rows(i).Item("Causale"))
    '    Next


    'End Sub

    'Private Sub NotificaAssenzeEccedenti(ByVal IdEntita As Integer, ByVal IdCausale As Integer)

    '    Dim sqlCMD As New SqlClient.SqlCommand
    '    Dim strNomeStore As String = "[SP_NOTIFICA_ASSENZE_ECCEDENTI]"

    '    Try
    '        sqlCMD = New SqlClient.SqlCommand(strNomeStore, CType(Session("conn"), SqlClient.SqlConnection))
    '        sqlCMD.CommandType = CommandType.StoredProcedure
    '        sqlCMD.Parameters.Add("@IdEntita", SqlDbType.Int).Value = IdEntita
    '        sqlCMD.Parameters.Add("@IDCausale", SqlDbType.Int).Value = IdCausale

    '        Dim sparam1 As SqlClient.SqlParameter
    '        sparam1 = New SqlClient.SqlParameter
    '        sparam1.ParameterName = "@Esito"
    '        sparam1.SqlDbType = SqlDbType.Int
    '        sparam1.Direction = ParameterDirection.Output
    '        sqlCMD.Parameters.Add(sparam1)

    '        sqlCMD.ExecuteScalar()
    '        'Dim int As 
    '        'str = sqlCMD.Parameters("@Esito").Value
    '        'Return str
    '    Catch ex As Exception
    '        Response.Write(ex.Message.ToString())
    '        Exit Sub
    '    End Try
    'End Sub

    Protected Sub CmdChiudi_Click(sender As Object, e As EventArgs) Handles CmdChiudi.Click
        CancellaTabellaTemp()
        Response.Redirect("wfrmImportRiattivazione.aspx")
    End Sub

    
End Class