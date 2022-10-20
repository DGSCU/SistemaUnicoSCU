Imports System.Collections
Imports System.IO

Public Class WfrmRisultatoImportDomandeVolontari
    Inherits System.Web.UI.Page

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

        Dim strRiga As String
        Dim arrRiga As String()
        Dim rdrLettura As StreamReader

        Dim rwCSV As DataRow

        dtCSV.Columns.Add(AddColumn("Note", "System.String"))
        dtCSV.Columns.Add(AddColumn("CodiceProgetto", "System.String"))
        dtCSV.Columns.Add(AddColumn("Titolo", "System.String"))
        dtCSV.Columns.Add(AddColumn("CodiceSede", "System.String"))
        dtCSV.Columns.Add(AddColumn("Domande", "System.String"))

        rdrLettura = New StreamReader(Server.MapPath("download") & "\" & Request.QueryString("NomeFile") & ".CSV")

        rdrLettura.ReadLine()

        While Not rdrLettura.EndOfStream
            strRiga = rdrLettura.ReadLine()
            arrRiga = CreaArray(strRiga)

            rwCSV = dtCSV.NewRow

            rwCSV(0) = arrRiga(0)
            rwCSV(1) = arrRiga(1)
            rwCSV(2) = arrRiga(2)
            rwCSV(3) = arrRiga(3)
            rwCSV(4) = arrRiga(6)

            dtCSV.Rows.Add(rwCSV)
        End While

        dtgCSV.DataSource = dtCSV
        dtgCSV.DataBind()
    End Sub

    Private Function AddColumn(ByVal vlCampo As String, ByVal vlType As String) As DataColumn
        Dim clCSV As New DataColumn

        clCSV.DataType = System.Type.GetType(vlType)
        clCSV.ColumnName = vlCampo
        clCSV.Caption = vlCampo
        clCSV.ReadOnly = False
        clCSV.Unique = False

        Return clCSV
    End Function

    Private Function CreaArray(ByVal pLinea As String) As String()
        ' Se esiste un caratttere ";" all'interno di una cella va ricostruita l'array
        Dim arrTMP As String()

        Dim i As Integer = 0
        Dim x As Integer

        arrTMP = Split(pLinea, ";")

        Do While i <= UBound(arrTMP)
            If arrTMP(i).StartsWith(Chr(34)) Then ' Se il primo carattere è "
                If arrTMP(i).EndsWith(Chr(34)) Then ' Se l'ultimo carattere é "
                    ' Eliminazione degi doppi apici ad inizio e fine riga
                    arrTMP(i) = arrTMP(i).Substring(1, arrTMP(i).Length - 2)
                    i = i + 1
                Else
                    arrTMP(i) = arrTMP(i) & ";" & arrTMP(i + 1)
                    ' Ricerca dei doppi apici che terminano la cella
                    For x = i + 1 To UBound(arrTMP) - 1
                        arrTMP(x) = arrTMP(x + 1)
                    Next
                    ReDim Preserve arrTMP(UBound(arrTMP) - 1)
                End If
            Else
                i = i + 1
            End If
        Loop

        Return arrTMP
    End Function

    Private Function DropTabellaTemp() As Boolean
        Dim strSQL As String
        Dim sqlTemp As SqlClient.SqlCommand
        Try
            '--- CANCELLO TAB TEMPORANEA
            strSQL = "DROP TABLE [#TEMP_DomandeVolontari]"

            sqlTemp = New SqlClient.SqlCommand
            sqlTemp.CommandText = strSQL
            sqlTemp.Connection = Session("conn")
            sqlTemp.ExecuteNonQuery()
            sqlTemp.Dispose()

            Return True

        Catch e As Exception
            sqlTemp.Dispose()
            Return False
        End Try
    End Function

    Protected Sub CmdConferma_Click(sender As Object, e As EventArgs) Handles CmdConferma.Click
        Dim MyTransaction As System.Data.SqlClient.SqlTransaction
        Dim strsql As String

        Dim MyCommand = New SqlClient.SqlCommand
        MyCommand.Connection = Session("conn")
        CmdConferma.Visible = False

        Try
            MyTransaction = Session("conn").BeginTransaction(Session("IdEnte") & "_" & Session("Utente"))
            MyCommand.Transaction = MyTransaction

            strsql = "UPDATE AttivitàEntiSediAttuazione " & _
                     "SET " & _
                     "VolontariPresentati = NumeroDomande " & _
                     "FROM [#TEMP_DomandeVolontari] a " & _
                     "INNER JOIN Attività b ON a.CodiceProgetto = b.CodiceEnte " & _
                     "INNER JOIN AttivitàEntiSediAttuazione c ON b.IDAttività = c.IDAttività AND a.CodiceSede = c.IDEnteSedeAttuazione "
            MyCommand.CommandText = strsql
            MyCommand.ExecuteNonQuery()

            strsql = "UPDATE Attività SET VolontariPresentati = Totale " & _
                     "FROM Attività a INNER JOIN (SELECT b.IDAttività, SUM(b.VolontariPresentati) Totale " & _
                     "FROM AttivitàEntiSediAttuazione b INNER JOIN attività c ON b.IDAttività = c.IDAttività " & _
                     "WHERE c.CodiceEnte IN (SELECT CodiceProgetto FROM [#TEMP_DomandeVolontari]) " & _
                     "GROUP BY b.IDAttività) d " & _
                     "ON a.IDAttività = d.IDAttività"
            MyCommand.CommandText = strsql
            MyCommand.ExecuteNonQuery()

            MyTransaction.Commit()
            lblEsito.Text = "Salvataggio dati avvenuto con successo."
            lblEsito.Visible = True

        Catch exc As Exception
            MyTransaction.Rollback(Session("IdEnte") & "_" & Session("Utente"))
            lblEsito.Text = "Errore durante l'operazione di inserimento dei dati."
            lblEsito.Visible = True
        End Try

        MyCommand.Dispose()
        DropTabellaTemp()
    End Sub

    Protected Sub CmdChiudi_Click(sender As Object, e As EventArgs) Handles CmdChiudi.Click
        DropTabellaTemp()
        Response.Redirect("WfrmCaricamentoVolontariPresentati.aspx")
    End Sub
End Class