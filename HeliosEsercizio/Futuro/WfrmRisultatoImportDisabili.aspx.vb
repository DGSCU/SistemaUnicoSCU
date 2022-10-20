Imports System.Collections
Imports System.IO
Public Class WfrmRisultatoImportDisabili
    Inherits System.Web.UI.Page
    Dim MyCommand As SqlClient.SqlCommand
    Dim DefArr As String()
    Dim ArrAggiorna As String()
    Dim ArrDatePreviste As String()


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Page.IsPostBack = False Then
            'Imposto il valore della label
           
            lblTipoImport.Text = "Risultato Import per i soggetti che usufruiscono dell'accompagnamento"
              

            'Inserire qui il codice utente necessario per inizializzare la pagina
           
            CaricaGrigliaDisabili()
            

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

            If Request.QueryString("NSediSuperate") = "true" Then
                lblEsito.Visible = True
                lblEsito.Text = "Attenzione. Impossibile effettuare l'inserimento in quanto il rapporto tra sedi di enti partner/sedi proprie e' superiore a quello previsto."
            End If
        End If

    End Sub
    Protected Sub CmdConferma_Click(sender As Object, e As EventArgs) Handles CmdConferma.Click
        Dim MyTransaction As System.Data.SqlClient.SqlTransaction
        Dim swErr As Boolean
        Dim strAlbo As String = "SCU"
        strAlbo = ClsUtility.TrovaAlboEnte(Session("IdEnte"), Session("conn"))

        MyCommand = New SqlClient.SqlCommand
        MyCommand.Connection = Session("conn")
        CmdConferma.Visible = False
        Try
            MyTransaction = Session("conn").BeginTransaction(Session("IdEnte") & "_" & Session("Utente"))
            MyCommand.Transaction = MyTransaction

        
            ScriviDisabili()
              

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
            AvvisoConferma.Visible = False
            '            CmdChiudi_Click.ImageUrl = "images/chiudi.jpg"
        Else
            'Errore Insert
            lblEsito.Text = "Errore durante l'operazione di inserimento dei dati."
            lblEsito.Visible = True
            AvvisoConferma.Visible = False
            'imgAnnulla.ImageUrl = "images/annulla.jpg"
        End If

        CancellaTabellaTemp()
    End Sub
    Private Sub CancellaTabellaTemp()
        Dim strSql As String
        Dim cmdCanTempTable As SqlClient.SqlCommand
        Try
           
            strSql = "DROP TABLE #IMP_DISABILI"
               

            cmdCanTempTable = New SqlClient.SqlCommand
            cmdCanTempTable.CommandText = strSql
            cmdCanTempTable.Connection = Session("conn")
            cmdCanTempTable.ExecuteNonQuery()
        Catch e As Exception
            lblEsito.Text = e.Message + " " + e.StackTrace


        End Try

        cmdCanTempTable.Dispose()
    End Sub
    Private Function CreaArray(ByVal pLinea As String) As String()
        Dim TmpArr As String()

        Dim i As Integer
        Dim x As Integer
        Dim strvalore As String

        TmpArr = Split(pLinea, ";")

        For i = 0 To UBound(TmpArr)
            If i = 0 Then
                ReDim DefArr(0)
            Else
                ReDim Preserve DefArr(UBound(DefArr) + 1)
            End If

            If Left(TmpArr(i), 1) = Chr(34) And Right(TmpArr(i), 1) = Chr(34) Then

                TmpArr(i) = Mid(TmpArr(i), 2, Len(TmpArr(i)) - 2)
            End If

            TmpArr(i) = TmpArr(i).Replace("""""", """")


            If 1 = 2 Then 'Left(TmpArr(i), 1) = Chr(34)
                x = i
                Do While Right(TmpArr(x), 1) <> Chr(34)
                    If x = i Then
                        DefArr(UBound(DefArr)) = Mid(TmpArr(x), 2) & "; "
                    Else
                        DefArr(UBound(DefArr)) = DefArr(UBound(DefArr)) & TmpArr(x) & "; "
                    End If
                    x = x + 1
                Loop
                '***************************************SUGGERIMENTI INDIRIZZO******************************************
                'vado a controllare l'indirizzo da stampare nella datagrid
                'modifica aggiunta da jons friztgerald kennedy il 30 marzo 2009
                If InStr(DefArr(UBound(DefArr)) & Mid(TmpArr(x), 1, Len(TmpArr(x)) - 1), "L'indirizzo non e' valido.") > 0 Then
                    strvalore = Replace(DefArr(UBound(DefArr)) & Mid(TmpArr(x), 1, Len(TmpArr(x)) - 1), "L'indirizzo non e' valido.", Session("ArraySegnalazioneIndirizzo")(0))
                    Dim intX As Integer
                    'Move all the items down
                    If UBound(Session("ArraySegnalazioneIndirizzo")) = 0 Then
                        Session("ArraySegnalazioneIndirizzo") = Nothing
                    Else
                        For intX = 0 To UBound(Session("ArraySegnalazioneIndirizzo")) - 1
                            Session("ArraySegnalazioneIndirizzo")(intX) = Session("ArraySegnalazioneIndirizzo")(intX + 1)
                        Next
                        'Redimension the array
                        ReDim Preserve Session("ArraySegnalazioneIndirizzo")(UBound(Session("ArraySegnalazioneIndirizzo")) - 1)
                    End If
                Else
                    strvalore = DefArr(UBound(DefArr)) & Mid(TmpArr(x), 1, Len(TmpArr(x)) - 1)
                End If
                DefArr(UBound(DefArr)) = strvalore
                'DefArr(UBound(DefArr)) = DefArr(UBound(DefArr)) & Mid(TmpArr(x), 1, Len(TmpArr(x)) - 1)
                i = x
            Else
                '***************************************SUGGERIMENTI INDIRIZZO******************************************
                If InStr(TmpArr(i), "L'indirizzo non e' valido.") > 0 Then
                    strvalore = Replace(TmpArr(i), "L'indirizzo non e' valido.", Session("ArraySegnalazioneIndirizzo")(0))
                    Dim intX As Integer
                    'Move all the items down
                    If UBound(Session("ArraySegnalazioneIndirizzo")) = 0 Then
                        Session("ArraySegnalazioneIndirizzo") = Nothing
                    Else
                        For intX = 0 To UBound(Session("ArraySegnalazioneIndirizzo")) - 1
                            Session("ArraySegnalazioneIndirizzo")(intX) = Session("ArraySegnalazioneIndirizzo")(intX + 1)
                        Next
                        'Redimension the array
                        ReDim Preserve Session("ArraySegnalazioneIndirizzo")(UBound(Session("ArraySegnalazioneIndirizzo")) - 1)
                    End If
                Else
                    strvalore = TmpArr(i)
                End If
                DefArr(UBound(DefArr)) = strvalore
            End If
        Next

        CreaArray = DefArr

    End Function
    Protected Sub CmdChiudi_Click(sender As Object, e As EventArgs) Handles CmdChiudi.Click
        CancellaTabellaTemp()
        Response.Redirect("WfrmDisabili.aspx?IdAmbitoAttività=" & Request.QueryString("IdAmbitoAttività") & "&IdAttivita=" & Request.QueryString("IdAttivita") & "&Modifica=1" & "&Nazionale=3")
    End Sub

    Private Sub CaricaGrigliaDisabili()
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
        clCSV.ColumnName = "Codicefiscale"
        clCSV.Caption = "Codicefiscale"
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
        clCSV.ColumnName = "Data"
        clCSV.Caption = "Data"
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
            Else
                rwCSV(0) = DefArr(0)
                rwCSV(1) = DefArr(1)
                rwCSV(2) = DefArr(2)
                rwCSV(3) = DefArr(3)
                rwCSV(4) = DefArr(4)
            End If

            dtCSV.Rows.Add(rwCSV)
            xLinea = Reader.ReadLine()
        End While

        dtgCSV.DataSource = dtCSV
        dtgCSV.DataBind()

    End Sub

    Private Sub ScriviDisabili()

        Dim strsql As String

        strsql = "INSERT INTO AttivitàAccompagnamento " & _
                 "(IDAttività, " & _
                 "cognome, " & _
                 "nome, " & _
                 "IDComuneNascita, " & _
                 "DataNascita, " & _
                 "CodiceFiscale, " & _
                 "IDComuneResidenza, " & _
                 "Indirizzo, " & _
                 "Civico, " & _
                 "Cap, " & _
                 "IdAssociaAmbitoCausaleAccompagno, " & _
                 "Usernameinseritore, " & _
                 "DataCreazioneRecord) " & _
                 "SELECT " & _
                 "'" & Request.QueryString("IdAttivita") & "', " & _
                 "a.cognome, " & _
                 "a.nome, " & _
                 "a.IDComuneNascita, " & _
                 "a.Data, " & _
                 "a.CodiceFiscale, " & _
                 "a.IDComuneResidenza, " & _
                 "a.Indirizzo, " & _
                 "a.Civico, " & _
                 "a.Cap, " & _
                 "a.IdAssociaAmbitoCausaleAccompagno, " & _
                 "'" & Session("Utente") & "', " & _
                 "getdate() " & _
                 "FROM #IMP_DISABILI a "



        myCommand.CommandText = strsql
        myCommand.ExecuteNonQuery()


    End Sub
End Class