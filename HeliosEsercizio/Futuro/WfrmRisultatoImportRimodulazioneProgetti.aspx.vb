Imports System.Collections
Imports System.IO
Public Class WfrmRisultatoImportRimodulazioneProgetti
    Inherits System.Web.UI.Page
    Dim MyCommand As SqlClient.SqlCommand
    Dim DefArr As String()
    Dim ArrAggiorna As String()
    'Dim ArrTel As String()
    Dim ArrDatePreviste As String()

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
        clCSV.ColumnName = "Titolo Progetto"
        clCSV.Caption = "TitoloProgetto"
        clCSV.ReadOnly = False
        clCSV.Unique = False
        dtCSV.Columns.Add(clCSV)

        clCSV = New DataColumn
        clCSV.DataType = System.Type.GetType("System.String")
        clCSV.ColumnName = "Codice Sede Progetto"
        clCSV.Caption = "CodiceSedeProgetto"
        clCSV.ReadOnly = False
        clCSV.Unique = False
        dtCSV.Columns.Add(clCSV)

        clCSV = New DataColumn
        clCSV.DataType = System.Type.GetType("System.String")
        clCSV.ColumnName = "Nome Sede Progetto"
        clCSV.Caption = "NomeSedeProgetto"
        clCSV.ReadOnly = False
        clCSV.Unique = False
        dtCSV.Columns.Add(clCSV)

        clCSV = New DataColumn
        clCSV.DataType = System.Type.GetType("System.String")
        clCSV.ColumnName = "Stato Attivazione"
        clCSV.Caption = "StatoAttivazione"
        clCSV.ReadOnly = False
        clCSV.Unique = False
        dtCSV.Columns.Add(clCSV)

        clCSV = New DataColumn
        clCSV.DataType = System.Type.GetType("System.String")
        clCSV.ColumnName = "Data Validità"
        clCSV.Caption = "DataValidità"
        clCSV.ReadOnly = False
        clCSV.Unique = False
        dtCSV.Columns.Add(clCSV)

        clCSV = New DataColumn
        clCSV.DataType = System.Type.GetType("System.String")
        clCSV.ColumnName = "Tipo Emergenza"
        clCSV.Caption = "TipoEmergenza"
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
                rwCSV(11) = vbNullString
                rwCSV(12) = vbNullString
            Else
                rwCSV(1) = DefArr(3)
                If UBound(DefArr) = 3 Then
                    rwCSV(2) = vbNullString
                Else
                    rwCSV(2) = DefArr(4)
                    If UBound(DefArr) = 4 Then
                        rwCSV(3) = vbNullString
                    Else
                        rwCSV(3) = DefArr(5)
                        If UBound(DefArr) = 5 Then
                            rwCSV(4) = vbNullString
                        Else
                            rwCSV(4) = DefArr(6)
                            If UBound(DefArr) = 6 Then
                                rwCSV(5) = vbNullString
                            Else
                                rwCSV(5) = DefArr(7)
                                If UBound(DefArr) = 8 Then
                                    rwCSV(6) = vbNullString
                                Else
                                    rwCSV(6) = DefArr(11)
                                    If UBound(DefArr) < 12 Then
                                        rwCSV(7) = vbNullString
                                    Else
                                        rwCSV(7) = DefArr(12)
                                    End If
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
            strSql = "DROP TABLE [#TEMP_Rimodulazione]"

            cmdCanTempTable = New SqlClient.SqlCommand
            cmdCanTempTable.CommandText = strSql
            cmdCanTempTable.Connection = Session("conn")
            cmdCanTempTable.ExecuteNonQuery()
        Catch e As Exception

        End Try

        cmdCanTempTable.Dispose()
    End Sub

    'Private Sub RipristinoSedi(ByVal strCodiceProgetto As String, ByVal strCodiceSedeProgetto As String, strDataRipresaServizio As String)
    '    'Creata il:		06/04/2020
    '    'Funzionalità: richiamo store SP_RIATTIVAZIONE_CONTROLLI_IMPORT per i controlli sui dati ricevuti nel file
    '    Dim strUtente As String

    '    strUtente = Session("Utente")

    '    'Try
    '    MyCommand.CommandText = "SP_RIATTIVAZIONE_RIPRISTINO_SEDI"   'ADC SP_RimodulazionePROGETTI_CONTROLLI_IMPORT 20/04/20221
    '    MyCommand.CommandType = CommandType.StoredProcedure
    '    MyCommand.Connection = Session("Conn")

    '    If MyCommand.Parameters.Count = 0 Then
    '        MyCommand.Parameters.Add("@Utente", SqlDbType.VarChar, 100).Value = strUtente
    '        MyCommand.Parameters.Add("@CodiceProgetto", SqlDbType.VarChar, 1000).Value = strCodiceProgetto
    '        MyCommand.Parameters.Add("@CodiceSedeProgetto", SqlDbType.VarChar, 1000).Value = strCodiceSedeProgetto
    '        MyCommand.Parameters.Add("@DataRipresaServizio", SqlDbType.VarChar, 1000).Value = strDataRipresaServizio

    '        MyCommand.Parameters.Add("@Esito", SqlDbType.TinyInt)
    '        MyCommand.Parameters("@Esito").Direction = ParameterDirection.Output
    '    Else
    '        MyCommand.Parameters("@CodiceProgetto").Value = strCodiceProgetto
    '        MyCommand.Parameters("@CodiceSedeProgetto").Value = strCodiceSedeProgetto
    '        MyCommand.Parameters("@DataRipresaServizio").Value = strDataRipresaServizio
    '    End If


    '    MyCommand.ExecuteNonQuery()

    '    'Catch ex As Exception
    '    '    intEsito = 0
    '    '    strMessaggio = "Errore generico"
    '    'Finally

    '    'End Try
    'End Sub

    Private Sub ScriviCovid19()
        Dim strsql As String
        'Dim intX As Integer

        'If dtsGenerico.Tables(0).Rows.Count > 0 Then
        '    For intX = 0 To dtsGenerico.Tables(0).Rows.Count - 1
        '        'ClsUtility.CronologiaDocEntità(dtsGenerico.Tables(0).Rows(intX).Item("identità"), Session("Utente"), "Graduatoria Volontari", Session("conn"), DataProt, NProt)
        '        RipristinoSedi(dtsGenerico.Tables(0).Rows(intX).Item("Codiceprogetto"), dtsGenerico.Tables(0).Rows(intX).Item("Codicesedeprogetto"), dtsGenerico.Tables(0).Rows(intX).Item("DataRipresaServizio"))

        '    Next
        'End If
        MyCommand.CommandType = CommandType.Text

        strsql = "INSERT INTO attivitàentisediattuazione_riattivazione " & _
                 "(IDAttivitàEnteSedeAttuazione, " & _
                 "IdStatoRiattivazione, " & _
                 "IdTipoModalitàServizio, " & _
                 "IdTipoAttivitàRimodulazione, " & _
                 "AltroAttivitàRimodulazione, " & _
                 "Username, " & _
                 "DataAggiornamento, " & _
                 "DataValiditàRimodulazione) " & _
                 "SELECT " & _
                 "b.IDAttivitàEnteSedeAttuazione, " & _
                 "d.IdStatoRiattivazione, " & _
                 "e.IdTipoModalitàServizio, " & _
                 "f.IdTipoAttivitàRimodulazione, " & _
                 "a.SpecificaAltro, " & _
                 "'" & Session("Utente") & "', " & _
                 "getdate(), " & _
                 "case a.DataValidità when '' then null else a.DataValidità end " & _
                 "FROM #TEMP_Rimodulazione a " & _
                 "INNER JOIN attivitàentisediattuazione b ON a.codicesedeprogetto = b.identesedeattuazione " & _
                 "INNER JOIN attività c on b.idattività = c.idattività and a.codiceprogetto = c.codiceente " & _
                 "INNER JOIN statiriattivazione d on a.StatoRiattivazione = d.codice " & _
                 "LEFT JOIN tipiModalitàServizio e on a.ModalitàServizio = e.codice and e.IdTipoEmergenza=1 " & _
                 "LEFT JOIN tipiattivitàrimodulazione f on a.CodiceAttivitàRimodulazione = f.codice and f.rimodulazione = 1 and f.IdTipoEmergenza=1 " & _
                 "WHERE isnull(b.IdStatoRiattivazione,0) <> isnull(d.IdStatoRiattivazione,0) " & _
                 "or isnull(b.IdTipoModalitàServizio,0) <> isnull(e.IdTipoModalitàServizio,0) " & _
                 "or isnull(b.IdTipoAttivitàRimodulazione,0) <> isnull(f.IdTipoAttivitàRimodulazione,0) " & _
                 "or isnull(b.AltroAttivitàRimodulazione,'--') <> isnull(a.SpecificaAltro,'--') " & _
                 "or isnull(b.DataValiditàRimodulazione,'01/01/1900') <> isnull(a.DataValidità,'01/01/1900') "
        MyCommand.CommandText = strsql
        MyCommand.ExecuteNonQuery()

        strsql = "UPDATE attivitàentisediattuazione SET " & _
         "IdStatoRiattivazione = d.IdStatoRiattivazione, " & _
         "IdTipoModalitàServizio = e.IdTipoModalitàServizio, " & _
         "IdTipoAttivitàRimodulazione = f.IdTipoAttivitàRimodulazione, " & _
         "AltroAttivitàRimodulazione = a.SpecificaAltro, " & _
         "DataValiditàRimodulazione = case a.DataValidità when '' then null else a.DataValidità end,  " & _
         "NumeroVolontariConsenso = null " & _
         "FROM #TEMP_Rimodulazione a " & _
         "INNER JOIN attivitàentisediattuazione b ON a.codicesedeprogetto = b.identesedeattuazione " & _
         "INNER JOIN attività c on b.idattività = c.idattività and a.codiceprogetto = c.codiceente " & _
         "INNER JOIN statiriattivazione d on a.StatoRiattivazione = d.codice " & _
         "LEFT JOIN tipiModalitàServizio e on a.ModalitàServizio = e.codice and e.IdTipoEmergenza=1 " & _
         "LEFT JOIN tipiattivitàrimodulazione f on a.CodiceAttivitàRimodulazione = f.codice and f.rimodulazione = 1 and f.IdTipoEmergenza=1 " & _
            "WHERE isnull(b.IdStatoRiattivazione,0) <> isnull(d.IdStatoRiattivazione,0) " & _
            "or isnull(b.IdTipoModalitàServizio,0) <> isnull(e.IdTipoModalitàServizio,0) " & _
            "or isnull(b.IdTipoAttivitàRimodulazione,0) <> isnull(f.IdTipoAttivitàRimodulazione,0) " & _
            "or isnull(b.AltroAttivitàRimodulazione,'--') <> isnull(a.SpecificaAltro,'--') " & _
            "or isnull(b.DataValiditàRimodulazione,'01/01/1900') <> isnull(a.DataValidità,'01/01/1900') "
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
            Dim strsql As String
            'Dim dtsGenerico As DataSet
            'strsql = "select Codiceprogetto, codicesedeprogetto, DataValidità from #TEMP_Rimodulazione where DataRipresaServizio <> ''"
            'dtsGenerico = ClsServer.DataSetGenerico(strsql, Session("conn"))

            MyTransaction = Session("conn").BeginTransaction(Session("IdEnte") & "_" & Session("Utente"))
            MyCommand.Transaction = MyTransaction

            'ScriviCovid19(dtsGenerico)
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

   

    Protected Sub CmdChiudi_Click(sender As Object, e As EventArgs) Handles CmdChiudi.Click
        CancellaTabellaTemp()
        Response.Redirect("wfrmImportRimodulazioneProgetti.aspx")
    End Sub
End Class