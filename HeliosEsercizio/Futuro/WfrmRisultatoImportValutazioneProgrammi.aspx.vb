Public Class WfrmRisultatoImportValutazioneProgrammi
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

        Dim sqlDS As DataSet
        Dim strSql As String

        strSql = "select Note,PRG as CodiceProgetto from #TEMP_VALUTAZIONE_PROGRAMMI"
        sqlDS = ClsServer.DataSetGenerico(strSql, Session("conn"))

        dtgCSV.DataSource = sqlDS
        dtgCSV.DataBind()

    End Sub

    Private Sub CmdConferma_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdConferma.Click
        Dim swErr As Boolean

        CmdConferma.Visible = False
        Try
            AggiornaStatoProgramma(Session("Utente"))
            InserimentoValutazione(Session("Utente"))
        Catch exc As Exception
            swErr = True
        End Try


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

    Private Sub AggiornaStatoProgramma(ByVal UserName As String)
        'creata da simona cordella il 19/02/2013
        Dim strsql As String
        Dim comGenerico As SqlClient.SqlCommand
        Dim i As Integer = 0
        Dim dtAgg As DataTable

        strsql = " select  s.IdProgramma "
        strsql &= " from storicoprogrammiTMPImport s inner join Programmi a on a.idProgramma = s.idProgramma"
        strsql &= " where s.username ='" & UserName & "'"
        dtAgg = ClsServer.CreaDataTable(strsql, False, Session("conn"))
        'aggiorno lo stato del progetto
        For i = 0 To dtAgg.Rows.Count - 1

            'strsql = "INSERT INTO CronologiaProgrammi "
            'strsql &= " (IDProgramma,IDStatoProgramma,DataCronologia,UsernameAccreditatore,idTipoCronologia) "
            'strsql &= " VALUES(" & dtAgg.Rows(i).Item("IdProgramma") & ", "
            'strsql &= " " & dtAgg.Rows(i).Item("IDStatoProgramma") & ", getdate(),'" & UserName & "',0) "
            'comGenerico = ClsServer.EseguiSqlClient(strsql, Session("conn"))

            strsql = "UPDATE Programmi "
            strsql &= " SET ConfermaValutazione=1 WHERE IDProgramma=" & dtAgg.Rows(i).Item("IdProgramma") & ""
            comGenerico = ClsServer.EseguiSqlClient(strsql, Session("conn"))
        Next
    End Sub

    Private Sub InserimentoValutazione(ByVal UserName As String)
        Dim intIDStoricoTMP As Integer
        Dim StrEsito As String
        Dim strsql As String
        Dim comGenerico As SqlClient.SqlCommand
        Dim dtrID As SqlClient.SqlDataReader
        Dim idStorico As String
        Dim i As Integer = 0
        Dim dtRisposta As DataTable


        ''Inserimento(StoricoProgetti)
        strsql = "insert into storicoprogrammi "
        strsql &= " SELECT [IDProgramma],[DataInserimento] "
        strsql &= " ,[StatoProgramma],[PunteggioFinale]"
        strsql &= " ,[PunteggioCP],[PunteggioCOE],[username] "
        strsql &= " ,[noteStorico],identificativoTMP"
        strsql &= " FROM[storicoprogrammiTMPimport] where username ='" & UserName & "'"

        comGenerico = ClsServer.EseguiSqlClient(strsql, Session("conn"))

        'inserimento storicooci
        strsql = " INSERT INTO storicovociProgrammi"
        strsql &= " select b.idstorico, a.idvocetmp, a.punteggio from storicovociProgrammitmpimport a"
        strsql &= " inner join [storicoProgrammi] b on a.idstoricotmp = b.identificativoTMP where b.username ='" & UserName & "'"
        comGenerico = ClsServer.EseguiSqlClient(strsql, Session("conn"))


        strsql = " select  distinct b.idstorico from storicovociProgrammiTMPimport a"
        strsql &= " inner join [storicoProgrammi] b on a.idstoricotmp = b.identificativoTMP where b.username ='" & UserName & "'"
        dtRisposta = ClsServer.CreaDataTable(strsql, False, Session("conn"))
        'aggironamento totali eseguo store "computescore"
        Dim Reader As SqlClient.SqlDataReader
        For i = 0 To dtRisposta.Rows.Count - 1
            'EseguiStoreStoricoProgetti ma e' per i Programmi
            Reader = ClsServer.EseguiStoreStoricoProgetti(dtRisposta.Rows(i).Item("idstorico"), "computeScoreProgrammi", Session("conn"))
            Reader.Close()
            Reader = Nothing
        Next
    End Sub

    Private Sub CancellaTabellaTemp()
        Dim strSql As String
        Dim cmdCanTempTable As SqlClient.SqlCommand

        Try
            '--- CANCELLO TAB TEMPORANEA
            strSql = "DROP TABLE [#TEMP_VALUTAZIONE_PROGRAMMI]"

            cmdCanTempTable = New SqlClient.SqlCommand
            cmdCanTempTable.CommandText = strSql
            cmdCanTempTable.Connection = Session("conn")
            cmdCanTempTable.ExecuteNonQuery()
        Catch e As Exception

        End Try

        cmdCanTempTable.Dispose()
    End Sub

    Private Sub CmdChiudi_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdChiudi.Click
        CancellaTabellaTemp()
        Response.Redirect("WfrmImportValutazioneProgrammi.aspx")
    End Sub

End Class