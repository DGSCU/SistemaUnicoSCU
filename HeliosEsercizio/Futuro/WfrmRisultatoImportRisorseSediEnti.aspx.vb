Imports System.Collections
Imports System.IO
Imports Logger.Data

Public Class WfrmRisultatoImportRisorseSediEnti
    Inherits SmartPage
    Dim MyCommand As SqlClient.SqlCommand
    Dim DefArr As String()
    Dim ArrAggiorna As String()
    Dim ArrDatePreviste As String()
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Page.IsPostBack = False Then
            'Imposto il valore della label
            Select Case Session("TipoImport")
                Case "riso"
                    lblTipoImport.Text = "Risultato Import Risorse"
                Case "sedi"
                    lblTipoImport.Text = "Risultato Import Sedi"
                    divWarning.Visible = True
                Case "enti"
                    lblTipoImport.Text = "Risultato Import Enti"
            End Select

            'Inserire qui il codice utente necessario per inizializzare la pagina
            Select Case Session("TipoImport")
                Case "riso"
                    CaricaGrigliariso()
                Case "sedi"
                    CaricaGrigliasedi()
                Case "enti"
                    CaricaGrigliaenti()
            End Select

            lblTotali.Text = "Sono state inviate " & Request.QueryString("Tot") & " righe. " & _
                             Request.QueryString("TotOk") & " con esito positivo. " & Request.QueryString("TotKo") & " con esito negativo."

            hlDownLoad.NavigateUrl = "download\" & Request.QueryString("NomeFile") & ".CSV"

            If CInt(Request.QueryString("TotKo")) > 0 Then
                CmdConferma.Visible = False
                If Session("TipoImport") = "sedi" Then
                    Log.Information(LogEvent.IMPORTAZIONE_SEDI_INSERIMENTO_ERRORE, lblTotali.Text)
                End If

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

    Protected Sub CmdChiudi_Click(sender As Object, e As EventArgs) Handles CmdChiudi.Click
        CancellaTabellaTemp()
        Response.Redirect("WfrmImportRisorseSediEnti.aspx?TipoImp=" & Session("TipoImport"))
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

            Select Case Session("TipoImport")
                Case "riso"
                    ScriviRisorse()
                Case "sedi"
                    ScriviSedi(MyTransaction)
                    Log.Information(LogEvent.IMPORTAZIONE_SEDI_INSERIMENTO_CORRETTO, "Nuove sedi importate")
                Case "enti"
                    ScriviEnti(strAlbo)
                    Log.Information(LogEvent.ENTI_IMPORTAZIONE_MASSIVA, "Nuovi enti importati")
            End Select

            MyTransaction.Commit()

        Catch exc As Exception

            MyTransaction.Rollback(Session("IdEnte") & "_" & Session("Utente"))
            swErr = True
            If Session("TipoImport") = "sedi" Then
                Log.Error(LogEvent.IMPORTAZIONE_SEDI_INSERIMENTO_ERRORE, "Errore nell'importazione", exc)
            End If
            If Session("TipoImport") = "enti" Then
                Log.Error(LogEvent.ENTI_ERRORE_IMPORTAZIONE_MASSIVA, "Errore nell'importazione", exc)
            End If

        End Try

        MyCommand.Dispose()

        If swErr = False Then
            'Esito positivo
            lblEsito.Text = "Operazione di inserimento dei dati effettuata con successo."
            lblEsito.Visible = True
            '            CmdChiudi_Click.ImageUrl = "images/chiudi.jpg"
        Else
            'Errore Insert
            lblEsito.Text = "Errore durante l'operazione di inserimento dei dati."
            lblEsito.Visible = True
            'imgAnnulla.ImageUrl = "images/annulla.jpg"
        End If

        CancellaTabellaTemp()
    End Sub

    Private Sub CancellaTabellaTemp()
        Dim strSql As String
        Dim cmdCanTempTable As SqlClient.SqlCommand
        Try
            Select Case Session("TipoImport")
                Case "riso"
                    strSql = "DROP TABLE #IMP_RISORSE"
                Case "sedi"
                    strSql = "DROP TABLE #IMP_SEDI"
                Case "enti"
                    strSql = "DROP TABLE #IMP_ENTI"
            End Select

            cmdCanTempTable = New SqlClient.SqlCommand
            cmdCanTempTable.CommandText = strSql
            cmdCanTempTable.Connection = Session("conn")
            cmdCanTempTable.ExecuteNonQuery()
        Catch e As Exception
            lblEsito.Text = e.Message + " " + e.StackTrace


        End Try

        cmdCanTempTable.Dispose()
    End Sub
#Region "Codice per le RISORSE"
    Private Sub CaricaGrigliariso()
        Dim dtCSV As DataTable = New DataTable
        Dim rwCSV As DataRow
        Dim clCSV As DataColumn

        Dim strSql As String
        Dim i As Integer

        Dim TmpArr() As String

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
        clCSV.ColumnName = "Ruolo"
        clCSV.Caption = "Ruolo"
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
                rwCSV(2) = DefArr(3)
                rwCSV(3) = DefArr(4)
                rwCSV(4) = DefArr(5)
            End If

            dtCSV.Rows.Add(rwCSV)
            xLinea = Reader.ReadLine()
        End While

        dtgCSV.DataSource = dtCSV
        dtgCSV.DataBind()

    End Sub

    Private Sub ScriviRisorse()
        Dim strsql As String
        strsql = "Insert Into EntePersonale (IdEnte,Cognome,Nome,IdComuneNascita,DataNascita,CodiceFiscale,DataCreazioneRecord,UsernameInseritore,EsperienzaServizioCivile,Corso) " & _
                 "SELECT DISTINCT '" & Session("IdEnte") & "',A.Cognome, A.Nome,B.IdComune,A.Data,A.CodiceFiscale,Getdate(),'" & Session("Utente") & "',0,0 FROM #IMP_RISORSE A " & _
                 "INNER JOIN comuni B ON (A.CodiceIstat = B.CodiceIstat OR A.CodiceIstat = B.CodiceIstatDismesso) " & _
                 " Where Not A.CodiceFiscale IN " & _
                 "(Select #IMP_RISORSE.CodiceFiscale From #IMP_RISORSE " & _
                 "INNER JOIN EntePersonale ON #IMP_RISORSE.CodiceFiscale = EntePersonale.CodiceFiscale AND EntePersonale.IdEnte = '" & Session("IdEnte") & "')"

        'strsql = " INSERT INTO EntePersonale (IdEnte,Cognome,Nome,IdComuneNascita,DataNascita,CodiceFiscale,DataCreazioneRecord,UsernameInseritore,EsperienzaServizioCivile,Corso) " & _
        '         " SELECT DISTINCT '" & Session("IdEnte") & "',A.Cognome, A.Nome,B.IdComune,A.Data,A.CodiceFiscale,Getdate(),'" & Session("Utente") & "',0,0 " & _
        '         " FROM #IMP_RISORSE A " & _
        '         " INNER JOIN comuni B ON (A.CodiceIstat = B.CodiceIstat OR A.CodiceIstat = B.CodiceIstatDismesso) " & _
        '         " LEFT JOIN EntePersonale on EntePersonale.IdEnte = '" & Session("IdEnte") & "' and a.CodiceFiscale = EntePersonale.CodiceFiscale " & _
        '         " WHERE EntePersonale.IdEntePersonale is null "

        MyCommand.CommandTimeout = 180
        MyCommand.CommandText = strsql
        MyCommand.ExecuteNonQuery()

        'Inserimento nella tabella Ente Personale Ruoli
        strsql = "Insert Into EntePersonaleRuoli (IdEntePersonale,IdRuolo,DataInizioValidità,Accreditato," & _
                 "UsernameInseritore,Principale,Visibilità,Forzatura,DataInseritore) " & _
                 "SELECT A.IdEntePersonale, C.IdRuolo,Convert(Datetime,'" & Format(Today.Today, "dd/MM/yyyy") & "',103)," & _
                 "'0', CASE C.IdRuolo WHEN 1 THEN REPLACE('" & Session("Utente") & "','E','N')  ELSE '" & Session("Utente") & "' END , case (select count(*) from entepersonaleruoli where entepersonaleruoli.identepersonale =a.identepersonale) when 0 then '1' else '0' end ,'0','0',GetDate() FROM entepersonale A " & _
                 "INNER JOIN #IMP_RISORSE B ON A.CodiceFiscale = B.CodiceFiscale And A.Idente = '" & Session("IdEnte") & "' " & _
                 "INNER JOIN Ruoli C ON B.Ruolo = C.DescrAbb"

        MyCommand.CommandText = strsql
        MyCommand.ExecuteNonQuery()

        'Inserimento nella tabella Cronologia Ente Personale Ruoli
        strsql = "Insert Into CronologiaEntePersonaleRuoli (IdEntePersonaleRuolo,Accreditato,DataCronologia,IdTipoCronologia,Note," & _
                 "UsernameAccreditatore,Forzatura) " & _
                 "SELECT C.IdEntePersonaleRuolo, '0',Convert(Datetime,'" & Format(Today.Today, "dd/MM/yyyy") & "',103)," & _
                 "'0','Importazione risorsa','" & Session("Utente") & "','0' FROM entepersonale A " & _
                 "INNER JOIN #IMP_RISORSE B ON A.CodiceFiscale = B.CodiceFiscale And A.IdEnte = '" & Session("IdEnte") & "' " & _
                 "INNER JOIN EntePersonaleRuoli C ON A.IdEntePersonale = C.IdEntePersonale "

        MyCommand.CommandText = strsql
        MyCommand.ExecuteNonQuery()

        'aggiunto il 19/07/2017 aggiorno il campo esperienza servizo civile e corso formazione
        strsql = " UPDATE EntePersonale SET EntePersonale.EsperienzaServizioCivile=#IMP_RISORSE.EsperienzaServizioCivile, EntePersonale.Corso=#IMP_RISORSE.Corso " & _
                 " From #IMP_RISORSE " & _
                 " INNER JOIN EntePersonale ON #IMP_RISORSE.CodiceFiscale = EntePersonale.CodiceFiscale AND EntePersonale.IdEnte = '" & Session("IdEnte") & "' " & _
                 " WHERE isnull(#IMP_RISORSE.EsperienzaServizioCivile,0) <> 0 "
        MyCommand.CommandText = strsql
        MyCommand.ExecuteNonQuery()


    End Sub
#End Region
#Region "Codice per le SEDI"
    'ok
    Private Sub CaricaGrigliasedi()
        Dim dtCSV As DataTable = New DataTable
        Dim rwCSV As DataRow
        Dim clCSV As DataColumn

        Dim strSql As String
        Dim i As Integer

        Dim TmpArr() As String

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
        clCSV.ColumnName = "Codice Fiscale"
        clCSV.Caption = "Codice Fiscale"
        clCSV.ReadOnly = False
        clCSV.Unique = False
        dtCSV.Columns.Add(clCSV)

        clCSV = New DataColumn
        clCSV.DataType = System.Type.GetType("System.String")
        clCSV.ColumnName = "NomeSede"
        clCSV.Caption = "Nome Sede"
        clCSV.ReadOnly = False
        clCSV.Unique = False
        dtCSV.Columns.Add(clCSV)

        clCSV = New DataColumn
        clCSV.DataType = System.Type.GetType("System.String")
        clCSV.ColumnName = "Indirizzo"
        clCSV.Caption = "Indirizzo"
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
            Else
                rwCSV(0) = DefArr(0)
                rwCSV(1) = DefArr(1)
                rwCSV(2) = DefArr(2)
                rwCSV(3) = DefArr(5) & " " & DefArr(6) & " " & DefArr(7)
            End If

            dtCSV.Rows.Add(rwCSV)
            xLinea = Reader.ReadLine()
        End While

        dtgCSV.DataSource = dtCSV
        dtgCSV.DataBind()

    End Sub

    'ok
    Private Sub ScriviSedi(ByVal myTransaction As SqlClient.SqlTransaction)
        Dim strsql As String
        Dim DtrGen As SqlClient.SqlDataReader
        Dim strCodiceFiscale As String
        'Ricavo il Codice Fiscale dell'Ente padre
        strsql = "SELECT CodiceFiscale FROM Enti WHERE IdEnte=" & Session("IdEnte")
        DtrGen = ClsServer.CreaDatareader(strsql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")), , myTransaction)
        DtrGen.Read()
        strCodiceFiscale = DtrGen.Item("CodiceFiscale")
        DtrGen.Close()
        DtrGen = Nothing
        'Inserimento nella tabella delle Sedi

        strsql = "Insert Into EntiSedi " &
                 "(IdEnte,Denominazione,Indirizzo,Civico,IdComune,CAP,PrefissoTelefono,Telefono," &
                 "PrefissoFax,Fax,DataControlloHTTP,HTTPValido,EMail,DataControlloEMail," &
                 "EMailValido,IdStatoEnteSede,DataCreazioneRecord,UsernameStato,DataUltimoStato," &
                 "RiferimentoRimborsi,Palazzina,Scala,Piano,Interno,IdTitoloGiuridico,UserNameUltimaModifica,CittaEstera, AnomaliaIndirizzo, AnomaliaNome, NormativaTutela, SoggettoEstero, NonDisponibilitaSede, DisponibilitaSede, AnomaliaIndirizzoGoogle) " &
                 "Select Distinct B.IdEnte,A.Denominazione,A.Indirizzo,A.Civico,C.IdComune,replicate('0',5-len(A.CAP)) + A.Cap,'0' + A.PrefissoTel,A.Telefono,'','',Null,0," &
                 "'',Null,0,(Select IdStatoEnteSede From StatiEntiSedi Where DefaultStato = 1),GetDate(),'" & Session("Utente") & "',Null,0,A.Palazzina,A.Scala,A.Piano,A.Interno,A.IdTitoloGiuridico,A.UserNameUltimaModifica,A.CittaEstera, " &
                 "A.Flgalertindirizzo, A.Flgalertnomesede, A.Normativa81, A.Soggettoestero, A.Dichiarazionesoggettoestero, A.Conformita, A.Flgalertindirizzogoogle " &
                 "From #IMP_SEDI A " &
                 "INNER JOIN Enti B ON A.CodiceFiscaleSede = B.CodiceFiscale And (B.idente = " & Session("IdEnte") & " Or b.idente in (select identefiglio from entirelazioni where identepadre = " & Session("IdEnte") & ")) " &
                 "INNER JOIN Comuni C ON A.CodiceIstatComune = C.CODICEISTAT " &
                 "LEFT JOIN EntiSedi D on B.IdEnte = D.IdEnte And A.Denominazione = D.Denominazione And A.Indirizzo = D.Indirizzo And C.IdComune = D.IdComune " &
                 "And A.Civico = D.Civico And A.Palazzina = D.Palazzina And A.Scala = D.Scala And isnull(A.Piano,0) = isnull(D.Piano,0) And A.Interno = D.Interno " &
                 "Where D.IdEnteSede Is NULL And B.IdStatoEnte IN (3,6,8,9)"
        MyCommand.CommandText = strsql
        MyCommand.ExecuteNonQuery()

        ''Aggiornamento della Mail e del Telefono
        'strsql = "UpDate EntiSedi Set Telefono = #IMP_SEDI.Telefono, PrefissoTelefono = '0' + #IMP_SEDI.PrefissoTel " & _
        '         "FROM EntiSedi A INNER JOIN Enti B ON A.IdEnte = B.IdEnte " & _
        '         "INNER JOIN COMUNI C ON A.IdComune = C.IdComune " & _
        '         "INNER JOIN #IMP_SEDI ON B.CodiceFiscale = #IMP_SEDI.CodiceFiscaleSede  AND A.Denominazione = #IMP_SEDI.Denominazione And A.Indirizzo = #IMP_SEDI.Indirizzo And A.Civico = #IMP_SEDI.Civico And C.CodiceIstat = #IMP_SEDI.CodiceIstatComune And B.IdStatoEnte in (3,6,8,9) " & _
        '         "WHERE (B.IdEnte = " & Session("IdEnte") & " OR B.IdEnte IN (Select IdEnteFiglio From EntiRelazioni Where IdEntePadre = " & Session("IdEnte") & "))"
        'MyCommand.CommandText = strsql
        'MyCommand.ExecuteNonQuery()

        'Inserimento nella tabella entiseditipi (Tutte Operative)
        strsql = "Insert into EntiSediTipi(IdEnteSede,IdTipoSede) " & _
                 "Select DISTINCT C.IdEnteSede,'4' From #IMP_SEDI A " & _
                 "INNER JOIN Enti B ON A.CodiceFiscaleSede = B.CodiceFiscale " & _
                 "INNER JOIN Comuni D ON A.CodiceIstatComune = D.CodiceIstat " & _
                 "INNER JOIN EntiSedi C ON B.IdEnte = C.IdEnte AND A.Denominazione = C.Denominazione AND A.Indirizzo = C.Indirizzo AND A.Civico = C.Civico And D.IdComune = C.IdComune " & _
                 "And A.Palazzina = C.Palazzina And A.Scala = C.Scala And isnull(A.Piano,0) = isnull(C.Piano,0) And A.Interno = C.Interno " & _
                 "LEFT JOIN EntiSediTipi E ON C.IdEnteSede = E.IdEnteSede " & _
                 "Where (B.IdEnte = " & Session("IdEnte") & " OR B.IdEnte IN (Select IdEnteFiglio From EntiRelazioni Where IdEntePadre = " & Session("IdEnte") & ")) And B.IdStatoEnte IN (3,6,8,9) And E.IdEnteSede Is Null "
        MyCommand.CommandText = strsql
        MyCommand.ExecuteNonQuery()

        'Inserimento nella tabella delle sedi di attuazione
        strsql = "Insert Into EntiSediAttuazioni (Denominazione,IdEnteSede,Note,IdStatoEnteSede,UsernameStato,DataUltimoStato,UsernameInseritore,DataInserimento,IdEnteCapoFila,NMaxVolontari,FlagMaxVolontari,UserNameMaxVolontari,Certificazione) " & _
                 "Select DISTINCT A.Denominazione,C.IdEnteSede,'',(Select IdStatoEnteSede From StatiEntiSedi Where DefaultStato = 1),'" & Session("Utente") & "',Null,'" & Session("Utente") & "',getDate()," & Session("IdEnte") & ", NMaxVolontari,FlagMaxVolontari,UserNameMaxVolontari,2 " & _
                 "From #IMP_SEDI A " & _
                 "INNER JOIN Comuni D ON A.CodiceIstatComune = D.CodiceIstat " & _
                 "INNER JOIN Enti B ON A.CodiceFiscaleSede = B.CodiceFiscale " & _
                 "INNER JOIN EntiSedi C ON B.IdEnte = C.IdEnte AND A.Denominazione = C.Denominazione AND A.Indirizzo = C.Indirizzo AND A.Civico = C.Civico AND D.IdComune = C.IdComune " & _
                 "And A.Palazzina = C.Palazzina And A.Scala = C.Scala And isnull(A.Piano,0) = isnull(C.Piano,0) And A.Interno = C.Interno " & _
                 "Where (B.IdEnte = " & Session("IdEnte") & " OR B.IdEnte IN (Select IdEnteFiglio From EntiRelazioni Where IdEntePadre = " & Session("IdEnte") & ")) And B.IdStatoEnte IN (3,6,8,9)"
        MyCommand.CommandText = strsql
        MyCommand.ExecuteNonQuery()


        strsql = " INSERT INTO AssociaEntiRelazioniSedi"
        strsql &= " SELECT A.IDEnteSede, ER.IDEnteRelazione,GETDATE() from entisediattuazioni A"
        strsql &= " INNER JOIN entisedi B ON A.IDEnteSede = B.IDEnteSede "
        strsql &= " INNER JOIN enti CAPOFILA on A.IdEnteCapofila = CAPOFILA.IdEnte"
        strsql &= " INNER JOIN enti ACCOGLIENZA ON B.IDEnte = ACCOGLIENZA.IDEnte "
        strsql &= " INNER JOIN entirelazioni ER ON ER.IDEntePadre=CAPOFILA.IDEnte AND ER.IDEnteFiglio = ACCOGLIENZA.IDEnte"
        strsql &= " LEFT JOIN AssociaEntiRelazioniSedi D ON D.IdEnteRelazione = ER.IDEnteRelazione AND D.IdEnteSede = B.IDEnteSede "
        strsql &= " WHERE (D.IdAssociaEntiRelazioniSedi Is NULL And CAPOFILA.IDEnte = " & Session("IdEnte") & ")"

        MyCommand.CommandText = strsql
        MyCommand.ExecuteNonQuery()


        strsql = " INSERT INTO AssociaEntiRelazioniSediAttuazioni "
        strsql &= " SELECT A.IDEnteSedeAttuazione, ER.IDEnteRelazione,GETDATE() from entisediattuazioni A"
        strsql &= " INNER JOIN entisedi B ON A.IDEnteSede = B.IDEnteSede "
        strsql &= " inner join enti CAPOFILA on A.IdEnteCapofila = CAPOFILA.IdEnte"
        strsql &= " INNER JOIN enti ACCOGLIENZA ON B.IDEnte = ACCOGLIENZA.IDEnte "
        strsql &= " INNER JOIN entirelazioni ER ON ER.IDEntePadre=CAPOFILA.IDEnte AND ER.IDEnteFiglio = ACCOGLIENZA.IDEnte"
        strsql &= " LEFT JOIN AssociaEntiRelazioniSediAttuazioni D ON D.IdEnteRelazione = ER.IDEnteRelazione AND D.IdEnteSedeAttuazione = A.IDEnteSedeAttuazione "
        strsql &= " WHERE (D.IdAssociaEntiRelazioniSediAttuazioni Is NULL And CAPOFILA.IDEnte = " & Session("IdEnte") & ")"

        MyCommand.CommandText = strsql
        MyCommand.ExecuteNonQuery()


        ' ''Popolamento della tabella associaentirelazionisedi
        ''strsql = "Insert Into AssociaEntiRelazioniSedi (IdEnteSede,IdEnteRelazione,DataCreazioneRecord) " & _
        ''         "SELECT EntiSedi.IDEnteSede, EntiRelazioni.IDEnteRelazione,GetDate() FROM EntiSedi " & _
        ''         "INNER JOIN EntiRelazioni ON EntiSedi.IDEnte = EntiRelazioni.IDEnteFiglio " & _
        ''         "LEFT JOIN AssociaEntiRelazioniSedi E ON EntiSedi.IdEnteSede = E.IdEnteSede " & _
        ''         "WHERE E.IdEnteSede IS NULL And EntiSedi.IDEnteSede IN " & _
        ''         "(Select Distinct C.IdEnteSede FROM #IMP_SEDI A " & _
        ''         "INNER JOIN Enti B ON A.CodiceFiscaleSede = B.CodiceFiscale And (B.IdEnte = " & Session("IdEnte") & " OR B.IdEnte IN (Select IdEnteFiglio From EntiRelazioni Where IdEntePadre = " & Session("IdEnte") & ")) " & _
        ''         "INNER JOIN Comuni D ON A.CodiceIstatComune = D.CodiceIstat " & _
        ''         "INNER JOIN Entisedi C ON B.IdEnte = C.IdEnte AND A.Denominazione = C.Denominazione AND A.Indirizzo = C.Indirizzo AND A.Civico = C.Civico AND D.IdComune = C.IdComune " & _
        ''         "And A.Palazzina = C.Palazzina And A.Scala = C.Scala And isnull(A.Piano,0) = isnull(C.Piano,0) And A.Interno = C.Interno " & _
        ''         "WHERE (B.IdEnte = " & Session("IdEnte") & " OR B.IdEnte IN (Select IdEnteFiglio From EntiRelazioni Where IdEntePadre = " & Session("IdEnte") & ")) and B.CodiceFiscale <> '" & ClsServer.NoApice(strCodiceFiscale) & "' and b.idstatoente in (3,6,8,9))"
        ''MyCommand.CommandText = strsql
        ''MyCommand.ExecuteNonQuery()

        ' ''Popolamento della tabella AssociaEntiRelazioniSediAttuazioni
        ''strsql = "Insert into AssociaEntiRelazioniSediAttuazioni (identesedeattuazione,identerelazione,datacreazionerecord) " & _
        ''         "SELECT entisediattuazioni.IDEnteSedeAttuazione, entirelazioni.IDEnteRelazione, GETDATE() FROM entisedi " & _
        ''         "INNER JOIN entisediattuazioni ON entisedi.IDEnteSede = entisediattuazioni.IDEnteSede " & _
        ''         "INNER JOIN entirelazioni ON entisedi.IDEnte = entirelazioni.IDEnteFiglio " & _
        ''         "LEFT JOIN AssociaEntiRelazioniSediAttuazioni E on entisediattuazioni.identesedeattuazione = E.identesedeattuazione " & _
        ''         "WHERE E.identesedeattuazione is null and entisediattuazioni.identesedeattuazione IN " & _
        ''         "(Select Distinct D.IdEnteSedeAttuazione FROM #IMP_SEDI A " & _
        ''         "INNER JOIN Enti B ON A.CodiceFiscaleSede = B.CodiceFiscale AND (B.idente = " & Session("IdEnte") & " or b.idente in (select identefiglio from entirelazioni where identepadre = " & Session("IdEnte") & ")) " & _
        ''         "INNER JOIN Comuni E ON A.CodiceIstatComune = E.CodiceIstat " & _
        ''         "INNER JOIN EntiSedi C ON B.IdEnte = C.IdEnte AND A.Denominazione = C.Denominazione AND A.Indirizzo = C.Indirizzo AND A.Civico = C.Civico AND E.IdComune = C.IdComune " & _
        ''         "And A.Palazzina = C.Palazzina And A.Scala = C.Scala And isnull(A.Piano,0) = isnull(C.Piano,0) And A.Interno = C.Interno " & _
        ''         "INNER JOIN entisediattuazioni D ON C.IdEnteSede = D.IdEnteSede " & _
        ''         "WHERE (B.idente = " & Session("IdEnte") & " or b.idente in (select identefiglio from entirelazioni where identepadre = " & Session("IdEnte") & ")) and B.CodiceFiscale <> '" & ClsServer.NoApice(strCodiceFiscale) & "' and b.idstatoente in (3,6,8,9))"
        ''MyCommand.CommandText = strsql
        ''MyCommand.ExecuteNonQuery()
    End Sub

#End Region
#Region "Codice per gli ENTI"
    'ok
    Private Sub CaricaGrigliaenti()
        Dim dtCSV As DataTable = New DataTable
        Dim rwCSV As DataRow
        Dim clCSV As DataColumn

        Dim strSql As String
        Dim i As Integer

        Dim TmpArr() As String

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
        clCSV.ColumnName = "Denominazione"
        clCSV.Caption = "Denominazione"
        clCSV.ReadOnly = False
        clCSV.Unique = False
        dtCSV.Columns.Add(clCSV)

        clCSV = New DataColumn
        clCSV.DataType = System.Type.GetType("System.String")
        clCSV.ColumnName = "Codice Fiscale"
        clCSV.Caption = "CodiceFiscale"
        clCSV.ReadOnly = False
        clCSV.Unique = False
        dtCSV.Columns.Add(clCSV)

        clCSV = New DataColumn
        clCSV.DataType = System.Type.GetType("System.String")
        clCSV.ColumnName = "Tipologia"
        clCSV.Caption = "Tipologia"
        clCSV.ReadOnly = False
        clCSV.Unique = False
        dtCSV.Columns.Add(clCSV)

        clCSV = New DataColumn
        clCSV.DataType = System.Type.GetType("System.String")
        clCSV.ColumnName = "TipoRelazione"
        clCSV.Caption = "Tipo Relazione"
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

    'ok
    Private Sub ScriviEnti(ByVal strAlbo As String)
        Dim strsql As String
        Dim dtrlocal As SqlClient.SqlDataReader
        Dim i As Integer
        ' Dim strAlbo As String = "SCU"


        'Inserimento nella tabella Enti
        strsql = "Insert Into Enti " &
                 "(Albo,Tipologia,Denominazione,IdStatoEnte,Http,HttpValido,EMail,EMailValido, " &
                 "DataCreazioneRecord,IdClasseAccreditamento,DataUltimaClasseAccreditamento," &
                 "IdClasseAccreditamentoRichiesta,ClasseAccreditamentoSubIudice,CodiceRegione,NoteRichiestaRegistrazione,CodiceFiscale," &
                 "PartitaIva,NumeroTotaleSedi,PrefissoTelefonoRichiestaRegistrazione,TelefonoRichiestaRegistrazione," &
                 "PrefissoFax,Fax,IdRegioneAppartenenza,IdRegioneCompetenza,CodiceFiscaleRL,DataNominaRL,AttivitaUltimiTreAnni,AttivitaFiniIstituzionali ,AttivitaSenzaLucro, AltroTipoEnte) " &
                 "Select " &
                 "'" & strAlbo & "',TipologieEnti.Descrizione,Denominazione,'6','',0,'',0,CONVERT(DateTime,'" & Format(Today.Today, "dd/MM/yyyy") & "',103)," &
                 "(SELECT IDClasseAccreditamento FROM classiaccreditamento WHERE DefaultClasse=1),CONVERT(DateTime,'" & Format(Today.Today, "dd/MM/yyyy") & "',103)," &
                 "'7','0','','" & Session("Utente") & "',CodiceFiscale,CodiceFiscale,0,'0' + PrefissoTel,Telefono,'',''," &
                 "(Select IdRegioneAppartenenza From Enti Where IdEnte = " & Session("IdEnte") & ")," &
                 "(Select IdRegioneCompetenza From Enti Where IdEnte = " & Session("IdEnte") & ")," &
                 "[Codice Fiscale Rappresentante Legale]," &
                 "convert(datetime,[Data Nomina Rappresentante Legale],103)," &
                 "[Attivita negli ultimi tre anni]," &
                 "[Attivita per Fini Istituzionali]," &
                 "[Attivita senza scopo di Lucro]," &
                 "AltroTipoEnte " &
                 "From #IMP_ENTI " &
                 "INNER JOIN TipologieEnti ON TipologieEnti.CodiceImport = #IMP_ENTI.Tipologia and ordinamento is not null"
        MyCommand.CommandText = strsql
        MyCommand.ExecuteNonQuery()

        'Update della classe richiesta dell'ente e dello stato
        strsql = "Update Enti Set IdStatoEnte = 6," & _
                 "IdClasseAccreditamentoRichiesta = TipiRelazioni.IdClasseAccreditamento " & _
                 "From Enti " & _
                 "INNER JOIN #IMP_ENTI ON Enti.CodiceFiscale = #IMP_ENTI.CodiceFiscale AND  Enti.IdStatoEnte = 4  " & _
                 "INNER JOIN TipiRelazioni ON #IMP_ENTI.TipoRelazione = TipiRelazioni.CodiceImport "
        MyCommand.CommandText = strsql
        MyCommand.ExecuteNonQuery()

        'Inserimento nella tabella EntiRelazioni
        strsql = "INSERT INTO EntiRelazioni (IdEntePadre,IdEnteFiglio,IdTipoRelazione,DataInizioValidità) " & _
                 "Select '" & Session("IdEnte") & "', B.IdEnte,TipiRelazioni.IdTipoRelazione,CONVERT(DateTime,'" & Format(Today.Today, "dd/MM/yyyy") & "',103) " & _
                 "From #IMP_ENTI AS A " & _
                 "INNER JOIN Enti As B ON A.Denominazione = B.Denominazione And A.CodiceFiscale = B.CodiceFiscale AND  b.IdStatoEnte = 6 " & _
                 "INNER JOIN TipiRelazioni ON A.TipoRelazione = TipiRelazioni.CodiceImport  "
        MyCommand.CommandText = strsql
        MyCommand.ExecuteNonQuery()

        'Inserimento della sede principale dell'ente
        strsql = "Insert Into EntiSedi (IDEnte,Denominazione,Indirizzo,Civico,IDComune,CAP,PrefissoTelefono,Telefono,IDStatoEnteSede,DataCreazioneRecord,UsernameStato) " & _
                 "Select B.IdEnte,'SEDE PRINC. - ' + B.Denominazione,A.Indirizzo,A.Civico,C.IdComune,replicate('0',5-len(A.CAP)) + A.CAP,'0' + A.PrefissoTel,A.Telefono,(Select IdStatoEnteSede From StatiEntiSedi Where DefaultStato = 1)," & _
                 "CONVERT(DateTime,'" & Format(Today.Today, "dd/MM/yyyy") & "',103),'" & ClsServer.NoApice(Session("Utente")) & "' " & _
                 "FROM #IMP_ENTI AS A " & _
                 "INNER JOIN Enti As B ON A.Denominazione = B.Denominazione And A.CodiceFiscale = B.CodiceFiscale AND  b.IdStatoEnte = 6 " & _
                 "INNER JOIN Comuni AS C ON A.CodiceIstatComune = C.CodiceIstat"
        MyCommand.CommandText = strsql
        MyCommand.ExecuteNonQuery()

        'Inserimento nella tabella tipo sedi
        strsql = "Insert Into EntiSediTipi (IDEnteSede,IDTipoSede) " & _
                 "Select C.IdEnteSede,'1'" & _
                 "FROM #IMP_ENTI AS A " & _
                 "INNER JOIN Enti As B ON A.Denominazione = B.Denominazione And A.CodiceFiscale = B.CodiceFiscale AND  b.IdStatoEnte = 6 " & _
                 "INNER JOIN EntiSedi AS C ON B.IdEnte = C.IdEnte"
        MyCommand.CommandText = strsql
        MyCommand.ExecuteNonQuery()

        'MODIFICA EFFETTUATA DA JOHN FITZGERALD BANNADY IL 16 giugno 2009
        'inserisco in EntiSettori se nella colonne dei settore è presente il "SI"
        strsql = "Select a.*, b.idente as identefiglio From [#IMP_ENTI] AS A " & _
                 "INNER JOIN Enti As B ON A.Denominazione = B.Denominazione And A.CodiceFiscale = B.CodiceFiscale  AND  b.IdStatoEnte = 6 "

        MyCommand.CommandText = strsql
        dtrlocal = MyCommand.ExecuteReader

        Dim vOperazioni() As String

        ReDim vOperazioni(0)

        vOperazioni(0) = ""

        Do While dtrlocal.Read
            If dtrlocal("Assistenza") = 1 Then
                strsql = "Insert Into EntiSettori (IdEnte,IdMacroAmbitoAttività,UsernameInserimento,DataInserimento) values "
                strsql = strsql & "("
                strsql = strsql & dtrlocal("identefiglio") & ", "
                strsql = strsql & "1, "
                strsql = strsql & "'" & Session("Utente") & "', "
                strsql = strsql & "GetDate()"
                strsql = strsql & ")"
                If vOperazioni(0) = "" Then
                    vOperazioni(0) = strsql
                Else
                    ReDim Preserve vOperazioni(UBound(vOperazioni) + 1)
                    vOperazioni(UBound(vOperazioni)) = strsql
                End If
            End If
            If dtrlocal("ProtezioneCivile") = 1 Then
                strsql = "Insert Into EntiSettori (IdEnte,IdMacroAmbitoAttività,UsernameInserimento,DataInserimento) values "
                strsql = strsql & "("
                strsql = strsql & dtrlocal("identefiglio") & ", "
                strsql = strsql & "2, "
                strsql = strsql & "'" & Session("Utente") & "', "
                strsql = strsql & "GetDate()"
                strsql = strsql & ")"
                If vOperazioni(0) = "" Then
                    vOperazioni(0) = strsql
                Else
                    ReDim Preserve vOperazioni(UBound(vOperazioni) + 1)
                    vOperazioni(UBound(vOperazioni)) = strsql
                End If
            End If
            If dtrlocal("Ambiente") = 1 Then
                strsql = "Insert Into EntiSettori (IdEnte,IdMacroAmbitoAttività,UsernameInserimento,DataInserimento) values "
                strsql = strsql & "("
                strsql = strsql & dtrlocal("identefiglio") & ", "
                strsql = strsql & "3, "
                strsql = strsql & "'" & Session("Utente") & "', "
                strsql = strsql & "GetDate()"
                strsql = strsql & ")"
                If vOperazioni(0) = "" Then
                    vOperazioni(0) = strsql
                Else
                    ReDim Preserve vOperazioni(UBound(vOperazioni) + 1)
                    vOperazioni(UBound(vOperazioni)) = strsql
                End If
            End If
            If dtrlocal("PatrimonioArtistico") = 1 Then
                strsql = "Insert Into EntiSettori (IdEnte,IdMacroAmbitoAttività,UsernameInserimento,DataInserimento) values "
                strsql = strsql & "("
                strsql = strsql & dtrlocal("identefiglio") & ", "
                strsql = strsql & "4, "
                strsql = strsql & "'" & Session("Utente") & "', "
                strsql = strsql & "GetDate()"
                strsql = strsql & ")"
                If vOperazioni(0) = "" Then
                    vOperazioni(0) = strsql
                Else
                    ReDim Preserve vOperazioni(UBound(vOperazioni) + 1)
                    vOperazioni(UBound(vOperazioni)) = strsql
                End If
            End If
            If dtrlocal("PromozioneCulturale") = 1 Then
                strsql = "Insert Into EntiSettori (IdEnte,IdMacroAmbitoAttività,UsernameInserimento,DataInserimento) values "
                strsql = strsql & "("
                strsql = strsql & dtrlocal("identefiglio") & ", "
                strsql = strsql & "5, "
                strsql = strsql & "'" & Session("Utente") & "', "
                strsql = strsql & "GetDate()"
                strsql = strsql & ")"
                If vOperazioni(0) = "" Then
                    vOperazioni(0) = strsql
                Else
                    ReDim Preserve vOperazioni(UBound(vOperazioni) + 1)
                    vOperazioni(UBound(vOperazioni)) = strsql
                End If
            End If
            If dtrlocal("Estero") = 1 Then
                strsql = "Insert Into EntiSettori (IdEnte,IdMacroAmbitoAttività,UsernameInserimento,DataInserimento) values "
                strsql = strsql & "("
                strsql = strsql & dtrlocal("identefiglio") & ", "
                strsql = strsql & "6, "
                strsql = strsql & "'" & Session("Utente") & "', "
                strsql = strsql & "GetDate()"
                strsql = strsql & ")"
                If vOperazioni(0) = "" Then
                    vOperazioni(0) = strsql
                Else
                    ReDim Preserve vOperazioni(UBound(vOperazioni) + 1)
                    vOperazioni(UBound(vOperazioni)) = strsql
                End If
            End If
            If dtrlocal("Agricoltura") = 1 Then
                strsql = "Insert Into EntiSettori (IdEnte,IdMacroAmbitoAttività,UsernameInserimento,DataInserimento) values "
                strsql = strsql & "("
                strsql = strsql & dtrlocal("identefiglio") & ", "
                strsql = strsql & "7, "
                strsql = strsql & "'" & Session("Utente") & "', "
                strsql = strsql & "GetDate()"
                strsql = strsql & ")"
                If vOperazioni(0) = "" Then
                    vOperazioni(0) = strsql
                Else
                    ReDim Preserve vOperazioni(UBound(vOperazioni) + 1)
                    vOperazioni(UBound(vOperazioni)) = strsql
                End If
            End If
        Loop

        dtrlocal.Close()
        dtrlocal = Nothing

        For i = 0 To UBound(vOperazioni)
            MyCommand.CommandText = vOperazioni(i)
            MyCommand.ExecuteNonQuery()
        Next

        'Update Codice Ente tramite store procedure
        MyCommand.CommandType = CommandType.StoredProcedure
        MyCommand.CommandText = "SP_GENERAZIONE_CODICI_FIGLI"

        Dim sparam As SqlClient.SqlParameter
        sparam = New SqlClient.SqlParameter
        sparam.ParameterName = "@IdEnte"
        sparam.SqlDbType = SqlDbType.Int
        MyCommand.Parameters.Add(sparam)

        Dim sparam1 As SqlClient.SqlParameter
        sparam1 = New SqlClient.SqlParameter
        sparam1.ParameterName = "@Valore"
        sparam1.SqlDbType = SqlDbType.Int
        sparam1.Direction = ParameterDirection.Output
        MyCommand.Parameters.Add(sparam1)

        Dim Reader As SqlClient.SqlDataReader
        MyCommand.Parameters("@IdEnte").Value = Session("IdEnte")
        Reader = MyCommand.ExecuteReader()
        If MyCommand.Parameters("@Valore").Value = True Then
            'Errore

        End If
        Reader.Close()
        Reader = Nothing

        'Inserisco nelle tabelle EnteEsperienzaSettore e VariazioneEnteSettoreArea tramite stored procedure
        MyCommand.Parameters.Clear()
        MyCommand.CommandText = "SP_INSERT_MASSIVO_EnteEsperienzaSettore"
        MyCommand.ExecuteNonQuery()

    End Sub


#End Region

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

End Class