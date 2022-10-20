Public Class WfrmRisultatoImportValutazioneProgetti
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

        strSql = "select Note,PRG as CodiceProgetto from #TEMP_VALUTAZIONE_PROGETTI"
        sqlDS = ClsServer.DataSetGenerico(strSql, Session("conn"))

        dtgCSV.DataSource = sqlDS
        dtgCSV.DataBind()

    End Sub

    Private Sub CmdConferma_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdConferma.Click
        Dim swErr As Boolean

        CmdConferma.Visible = False
        Try
            AggiornaStatoProgetto(Session("Utente"))
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

    Private Sub AggiornaStatoProgetto(ByVal UserName As String)
        'creata da simona cordella il 19/02/2013
        Dim strsql As String
        Dim comGenerico As SqlClient.SqlCommand
        Dim i As Integer = 0
        Dim dtAgg As DataTable

        strsql = " select  s.IdProgetto,s.Gruppo,a.IDStatoAttività  "
        strsql &= " from storicoprogettiTMPImport s inner join attività a on a.idattività = s.idprogetto"
        strsql &= " where s.username ='" & UserName & "'"
        dtAgg = ClsServer.CreaDataTable(strsql, False, Session("conn"))
        'aggiorno lo stato del progetto
        For i = 0 To dtAgg.Rows.Count - 1

            strsql = "INSERT INTO CronologiaAttività "
            strsql &= " (IDAttività,IDStatoAttività,DataCronologia,UsernameAccreditatore,idTipoCronologia) "
            strsql &= " VALUES(" & dtAgg.Rows(i).Item("IdProgetto") & ", "
            strsql &= " " & dtAgg.Rows(i).Item("IDStatoAttività") & ", getdate(),'" & UserName & "',0) "
            comGenerico = ClsServer.EseguiSqlClient(strsql, Session("conn"))

            strsql = "UPDATE ATTIVITÀ "
            strsql &= " SET IDStatoattività=9, "
            strsql &= " DataUltimoStato=getdate(), "
            strsql &= " ConfermaValutazione=1, "
            strsql &= " NoteRiservate=0, "
            strsql &= " Limitazioni= " & dtAgg.Rows(i).Item("gruppo") & ""
            strsql &= " WHERE IDAttività=" & dtAgg.Rows(i).Item("IdProgetto") & ""
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

        'Creata da simona Cordella il 15/02/2013
        ''Inserimento(StoricoProgetti)
        strsql = "insert into storicoprogetti "
        strsql &= " SELECT [IDProgetto],[IDUser],[DataInserimento],[Gruppo],[Semestre],[AltriEnti],[IDArea1]"
        strsql &= " ,[IDArea2],[IDArea3],[IDArea4],[IDArea5],[NoSettorePrevisto],[NoteSettore]"
        strsql &= " ,[NoAderenzaFinalità],[NoteFinalità],[NoFormazioneVolontari],[NoteFormazione]"
        strsql &= " ,[NoDurataFormazione],[NoteDurataFormazione],[ContestoTerritoriale],[NoteContestoTerritoriale]"
        strsql &= " ,[ObiettivoProgetto],[NoteObiettivoProgetto],[DescrizioneProgetto],[NoteDescrizioneProgetto]"
        strsql &= " ,[ObiettivoAnnuale],[NoteObiettivoAnnuale],[ModalitàFormazione],[NoteModalitàFormazione]"
        strsql &= " ,[ContestoSocioPolitico],[NoteContestoSocioPolitico],[SicurezzaProgetto],[NoteSicurezzaProgetto]"
        strsql &= " ,[StatoProgetto],[NoteContestoTerritoriale7],[ParereCTS],[SuntoParere],[PunteggioFinale]"
        strsql &= " ,[PunteggioCP],[PunteggioCKH],[PunteggioCO],[PunteggioAggiuntivo],[NoteSospensione]"
        strsql &= " ,[Limitazioni],[MotivazioneNonApprovazione],[puntRilevanza],[puntCoerenza]"
        strsql &= " ,[puntInternet],[defCopertura],[defRinunce],[defInfrazione],[username]"
        strsql &= " ,[importo],[numvolontari],[costoannuo],[rimborso],[imprisaggprog]"
        strsql &= " ,[imprisaggform],[volrichiesti],[giorniestero],[importoestero]"
        strsql &= " ,[noteStorico],[puntRegionale],[Note24A],[Note24B],[Note24C]"
        strsql &= " ,[Note25],[Note28],[Note29],[punteggioCOE],[defSanzioni],[defInfortuni]"
        strsql &= " FROM[storicoprogettiTMPimport] where username ='" & UserName & "'"

        comGenerico = ClsServer.EseguiSqlClient(strsql, Session("conn"))

        'inserimento storicooci
        strsql = " INSERT INTO storicovoci"
        strsql &= " select b.idstorico, a.idvocetmp, a.punteggio from storicovocitmpimport a"
        strsql &= " inner join [storicoprogetti] b on a.idstoricotmp = b.altrienti where b.username ='" & UserName & "'"
        comGenerico = ClsServer.EseguiSqlClient(strsql, Session("conn"))


        strsql = " select  distinct b.idstorico from storicovocitmpimport a"
        strsql &= " inner join [storicoprogetti] b on a.idstoricotmp = b.altrienti where b.username ='" & UserName & "'"
        dtRisposta = ClsServer.CreaDataTable(strsql, False, Session("conn"))
        'aggironamento totali eseguo store "computescore"
        Dim Reader As SqlClient.SqlDataReader
        For i = 0 To dtRisposta.Rows.Count - 1
            Reader = ClsServer.EseguiStoreStoricoProgetti(dtRisposta.Rows(i).Item("idstorico"), "computeScore", Session("conn"))
            Reader.Close()
            Reader = Nothing
        Next
    End Sub

    Private Sub CancellaTabellaTemp()
        Dim strSql As String
        Dim cmdCanTempTable As SqlClient.SqlCommand

        Try
            '--- CANCELLO TAB TEMPORANEA
            strSql = "DROP TABLE [#TEMP_VALUTAZIONE_PROGETTI]"

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
        Response.Redirect("WfrmImportValutazioneProgetti.aspx")
    End Sub

End Class