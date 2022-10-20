Public Class dettaglirisorsa
    Inherits System.Web.UI.Page

    Dim dtsDettagli As DataSet
    Dim dtrgenerico As SqlClient.SqlDataReader

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Inserire qui il codice utente necessario per inizializzare la pagina
        'Jonathan Bagnani 03/06/2004
        'variabile contatore
        Dim intX As Integer
        'stringa locale che conterrà la query per i dettagli della risorsa
        Dim strSqlLocal As String
        'controllo se è stato effettuato il login
        If Not Session("LogIn") Is Nothing Then
            'se non è stato effettuato login
            If Session("LogIn") = False Then
                'carico la pagina LogOut dove svuoto eventuali session aperte
                Response.Redirect("LogOn.aspx")
            End If
        Else
            'carico la pagina LogOut dove svuoto eventuali session aperte
            Response.Redirect("LogOn.aspx")
        End If


        If Request.QueryString("idrisorsa") <> "" Then


            'IDENTE  Session("IdEnte")
            'CODICEENTE  NZ Session("codiceregione")  or Session("txtCodEnte")

            Dim strATTIVITA As Integer = -1
            Dim strBANDOATTIVITA As Integer = -1
            Dim strENTEPERSONALE As Integer = -1
            Dim strENTITA As Integer = -1
            Dim strIDENTE As Integer = -1


            If ClsUtility.SICUREZZA_VERIFICA_AUTORIZZAZIONI(Session("conn"), Session("IdEnte"), Session("txtCodEnte"), strATTIVITA, strBANDOATTIVITA, Request.QueryString("idrisorsa"), strENTITA, strIDENTE) = 1 Then

                If Not dtrgenerico Is Nothing Then
                    dtrgenerico.Close()
                    dtrgenerico = Nothing
                End If
            Else
                If Not dtrgenerico Is Nothing Then
                    dtrgenerico.Close()
                    dtrgenerico = Nothing
                End If
                Response.Redirect("wfrmAnomaliaDati.aspx")

            End If

        End If




        'controllo dell'esistenza di un id della risorsa (per evitare l'apertura forzata della pagina)
        If Request.QueryString("idrisorsa") Is Nothing Then
            Response.Write("<script>" & vbCrLf)
            Response.Write("window.close()")
            Response.Write("</script>")
        End If

        strSqlLocal = "SELECT isnull(replicate('0',2-len(day(AssociaEntePersonaleRuoliAttivitàEntiSediAttuazione.DataAssegnazione))) + convert(varchar(2),day(AssociaEntePersonaleRuoliAttivitàEntiSediAttuazione.DataAssegnazione)) + '/' + "
        strSqlLocal = strSqlLocal & "replicate('0',2-len(month(AssociaEntePersonaleRuoliAttivitàEntiSediAttuazione.DataAssegnazione))) + convert(varchar(2),month(AssociaEntePersonaleRuoliAttivitàEntiSediAttuazione.DataAssegnazione)) +	'/' + "
        strSqlLocal = strSqlLocal & "convert(varchar(4),year(AssociaEntePersonaleRuoliAttivitàEntiSediAttuazione.DataAssegnazione)),'') DataAssegnazione, "
        strSqlLocal = strSqlLocal & "isnull(replicate('0',2-len(day(AssociaEntePersonaleRuoliAttivitàEntiSediAttuazione.DataFineAssegnazione))) + convert(varchar(2),day(AssociaEntePersonaleRuoliAttivitàEntiSediAttuazione.DataFineAssegnazione)) + '/' + "
        strSqlLocal = strSqlLocal & "replicate('0',2-len(month(AssociaEntePersonaleRuoliAttivitàEntiSediAttuazione.DataFineAssegnazione))) + convert(varchar(2),month(AssociaEntePersonaleRuoliAttivitàEntiSediAttuazione.DataFineAssegnazione)) +	'/' + "
        strSqlLocal = strSqlLocal & "convert(varchar(4),year(AssociaEntePersonaleRuoliAttivitàEntiSediAttuazione.DataFineAssegnazione)),'') DataFineAssegnazione, "
        strSqlLocal = strSqlLocal & "entepersonale.Cognome + ' ' + entepersonale.nome nominativo, "
        strSqlLocal = strSqlLocal & "entepersonale.datanascita, "
        strSqlLocal = strSqlLocal & "case when len(c1.denominazione) >23  then left(c1.denominazione,20) + '... (' + p1.provincia + ')'  else c1.denominazione + ' (' + p1.provincia + ')' end comunenascita , "
        strSqlLocal = strSqlLocal & "ruoli.ruolo, "
        strSqlLocal = strSqlLocal & "isnull(replicate('0',2-len(day(entepersonaleruoli.dataaccreditamento))) + convert(varchar(2),day(entepersonaleruoli.dataaccreditamento)) + '/' + "
        strSqlLocal = strSqlLocal & "replicate('0',2-len(month(entepersonaleruoli.dataaccreditamento))) + convert(varchar(2),month(entepersonaleruoli.dataaccreditamento)) +	'/' + "
        strSqlLocal = strSqlLocal & "convert(varchar(4),year(entepersonaleruoli.dataaccreditamento)),'') dataaccreditamento, "
        strSqlLocal = strSqlLocal & "attività.TITOLO AS DESCRIZIONE, attività.CodiceEnte, entisediattuazioni.identesedeattuazione, "
        strSqlLocal = strSqlLocal & "isnull(replicate('0',2-len(day(attività.DataInizioAttività))) + convert(varchar(2),day(attività.DataInizioAttività)) + '/' + "
        strSqlLocal = strSqlLocal & "replicate('0',2-len(month(attività.DataInizioAttività))) + convert(varchar(2),month(attività.DataInizioAttività)) +	'/' + "
        strSqlLocal = strSqlLocal & "convert(varchar(4),year(attività.DataInizioAttività)),'') DataInizioAttività, "
        strSqlLocal = strSqlLocal & "isnull(replicate('0',2-len(day(attività.DataFineAttività))) + convert(varchar(2),day(attività.DataFineAttività)) + '/' + "
        strSqlLocal = strSqlLocal & "replicate('0',2-len(month(attività.DataFineAttività))) + convert(varchar(2),month(attività.DataFineAttività)) +	'/' + "
        strSqlLocal = strSqlLocal & "convert(varchar(4),year(attività.DataFineAttività)),'') DataFineAttività, "
        strSqlLocal = strSqlLocal & "statiattività.StatoAttività, "
        strSqlLocal = strSqlLocal & "entisedi.denominazione nomesede, "
        strSqlLocal = strSqlLocal & "entisediattuazioni.denominazione nomesedeattuazione, "
        strSqlLocal = strSqlLocal & "case when len(c2.denominazione) >23  then left(c2.denominazione,20) + '... (' + p2.provincia + ')'  else c2.denominazione + ' (' + p2.provincia + ')' end comunesede , "
        strSqlLocal = strSqlLocal & "macroambitiattività.Codifica + ' - ' + macroambitiattività.MacroAmbitoAttività Settore "
        strSqlLocal = strSqlLocal & "FROM entepersonaleruoli "
        strSqlLocal = strSqlLocal & " JOIN entepersonale ON entepersonaleruoli.identepersonale = entepersonale.identepersonale "
        strSqlLocal = strSqlLocal & "INNER JOIN comuni c1 ON entepersonale.idcomunenascita = c1.idcomune "
        strSqlLocal = strSqlLocal & "RIGHT JOIN Provincie p1 ON c1.Idprovincia = p1.idprovincia "
        strSqlLocal = strSqlLocal & "RIGHT JOIN ruoli ON entepersonaleruoli.idruolo=ruoli.idruolo "
        strSqlLocal = strSqlLocal & "RIGHT JOIN AssociaEntePersonaleRuoliAttivitàEntiSediAttuazione ON AssociaEntePersonaleRuoliAttivitàEntiSediAttuazione.IdEntePersonaleRuolo = entepersonaleruoli.IDEntePersonaleRuolo "
        strSqlLocal = strSqlLocal & "INNER JOIN attivitàentisediattuazione ON AssociaEntePersonaleRuoliAttivitàEntiSediAttuazione.IdAttivitàEnteSedeAttuazione = attivitàentisediattuazione.IDAttivitàEnteSedeAttuazione  "
        strSqlLocal = strSqlLocal & "INNER JOIN entisediattuazioni ON attivitàentisediattuazione.identesedeattuazione = entisediattuazioni.identesedeattuazione "
        strSqlLocal = strSqlLocal & "INNER JOIN entisedi ON entisediattuazioni.identesede = entisedi.identesede "
        strSqlLocal = strSqlLocal & "INNER JOIN comuni c2 ON entisedi.idcomune = c2.idcomune "
        strSqlLocal = strSqlLocal & "INNER JOIN Provincie p2 ON c2.Idprovincia = p2.idprovincia "
        strSqlLocal = strSqlLocal & "INNER JOIN attività ON attivitàentisediattuazione.IDAttività = attività.IDAttività "
        strSqlLocal = strSqlLocal & "INNER JOIN statiattività ON attività.IDStatoAttività = statiattività.IDStatoAttività "
        strSqlLocal = strSqlLocal & "inner join ambitiattività on ambitiattività.IDAmbitoAttività=attività.IDAmbitoAttività "
        strSqlLocal = strSqlLocal & "inner join macroambitiattività on macroambitiattività.IDMacroAmbitoAttività=ambitiattività.IDMacroAmbitoAttività "
        strSqlLocal = strSqlLocal & "WHERE entepersonaleruoli.IDEntePersonale=" & CInt(Request.QueryString("idrisorsa")) & " and entepersonaleruoli.IDRuolo=" & CInt(Request.QueryString("idruolo")) & "  order by attività.IDAttività desc"

        'eseguo la query e carico il dataset del risultato
        dtsDettagli = ClsServer.DataSetGenerico(strSqlLocal, Session("conn"))
        'controllo se ci sono dei record
        If dtsDettagli.Tables(0).Rows.Count > 0 Then
            'se ci sono carico e bindo la datagrid e carico i campi
            dtgRisultatoRicerca.DataSource = dtsDettagli
            dtgRisultatoRicerca.DataBind()
            txtComuneNascita.Text = IIf(IsDBNull(dtsDettagli.Tables(0).Rows(0).Item("comunenascita")) = True, "", dtsDettagli.Tables(0).Rows(0).Item("comunenascita"))
            txtDataAccreditamento.Text = IIf(CStr(dtsDettagli.Tables(0).Rows(0).Item("dataaccreditamento")) = "", "", CStr(dtsDettagli.Tables(0).Rows(0).Item("dataaccreditamento")))
            txtDataNascita.Text = IIf(CStr(dtsDettagli.Tables(0).Rows(0).Item("datanascita")) = "", "", CStr(dtsDettagli.Tables(0).Rows(0).Item("datanascita")))
            txtNominativo.Text = IIf(IsDBNull(dtsDettagli.Tables(0).Rows(0).Item("nominativo")) = True, "", dtsDettagli.Tables(0).Rows(0).Item("nominativo"))
            txtRuolo.Text = IIf(IsDBNull(dtsDettagli.Tables(0).Rows(0).Item("ruolo")) = True, "", dtsDettagli.Tables(0).Rows(0).Item("ruolo"))
            'controllo se c'è almeno un record nella griglia
            If IsDBNull(dtsDettagli.Tables(0).Rows(0).Item("descrizione")) = True Then
                'informo l'utente della mancanza di dettagli
                lblErr.Visible = True
                lblErr.Text = "Non ci sono attività associate al ruolo selezionato."
            End If
            'comunque chiudo il dataset
            dtsDettagli.Dispose()
        Else
            'comunque chiudo il dataset
            dtsDettagli.Dispose()
            'informo l'utente della mancanza di dettagli
            lblErr.Visible = True
            lblErr.Text = "Non ci sono attività associate al ruolo selezionato."
        End If
    End Sub

    Private Sub dtgRisultatoRicerca_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dtgRisultatoRicerca.PageIndexChanged
        'passo il nuovo indice selezionato all'indice della pagina da visualizzare
        dtgRisultatoRicerca.CurrentPageIndex = e.NewPageIndex
        'riassegno il dataset dichiarato volutamente pubblico a tutta la pagina
        dtgRisultatoRicerca.DataSource = dtsDettagli
        dtgRisultatoRicerca.DataBind()
    End Sub

    Private Sub cmdChiudi_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdChiudi.Click
        Response.Write("<script>" & vbCrLf)
        Response.Write("window.close()" & vbCrLf)
        Response.Write("</script>")
    End Sub

End Class