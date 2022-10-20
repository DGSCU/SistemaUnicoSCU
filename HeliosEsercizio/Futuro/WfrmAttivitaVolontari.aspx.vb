Public Class WfrmAttivitaVolontari
    Inherits System.Web.UI.Page

#Region " Codice generato da Progettazione Web Form "

    'Chiamata richiesta da Progettazione Web Form.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents txtDataNascita As System.Web.UI.WebControls.Label
    Protected WithEvents txtCF As System.Web.UI.WebControls.Label

    'NOTA: la seguente dichiarazione è richiesta da Progettazione Web Form.
    'Non spostarla o rimuoverla.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: questa chiamata al metodo è richiesta da Progettazione Web Form.
        'Non modificarla nell'editor del codice.
        InitializeComponent()
    End Sub

#End Region
    'dataset pubblico alla pagina così da mantenerlo in vita
    'in fase di pageindexchange
    Dim dtsDettagli As DataSet
    Dim dtsStoricoAssegnazione As DataSet

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load, Me.Load
        'caricamento attività progetti relativi al singolo volonatrio
        'AUTORE: TESTA GUIDO    DATA: 15/10/2004
        'controllo se è stato effettuato il login
        If Not Session("LogIn") Is Nothing Then
            'se non è stato effettuato login
            If Session("LogIn") = False Then
                'carico la pagina LogOn.aspx dove svuoto eventuali session aperte
                Response.Redirect("LogOn.aspx")
            End If
        Else
            'carico la pagina LogOn.aspx dove svuoto eventuali session aperte
            Response.Redirect("LogOn.aspx")
        End If

        If abilitalink(Request.Params("IdVolontario")) = "SI" Then
            imgCronoRiattivazione.Visible = True
        Else
            imgCronoRiattivazione.Visible = False
        End If

        If abilitalinkRimodulazione(Request.Params("IdVolontario")) = "SI" Then
            imgCronoRimodulazione.Visible = True
        Else
            imgCronoRimodulazione.Visible = False
        End If


        'Call CaricaUserName(Request.Params("IdVolontario"))
        Call CaricaDati(Request.Params("IdVolontario"))

    End Sub


    Private Sub dtgRisultatoRicerca_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dtgRisultatoRicerca.PageIndexChanged
        'passo il nuovo indice selezionato all'indice della pagina da visualizzare
        dtgRisultatoRicerca.CurrentPageIndex = e.NewPageIndex
        'riassegno il dataset dichiarato volutamente pubblico a tutta la pagina
        dtgRisultatoRicerca.DataSource = Session("appDtsRisRicerca")
        dtgRisultatoRicerca.DataBind()
    End Sub
    Private Sub CaricaDati(ByVal idVol As Integer)
   
            Dim strSql As String
        strSql = "SELECT " & _
        "attività.Titolo + ' [' + Attività.CodiceEnte + ']' as Titolo, entità.codicefiscale as CF, " & _
        "CONVERT(varchar, attivitàentità.DataInizioAttivitàEntità, 103) AS DataInizioAttivitàEntità, " & _
        "CONVERT(varchar,attivitàentità.DataFineAttivitàEntità, 103) AS DataFineAttivitàEntità, " & _
        "attivitàentità.Usernamericollocamento, " & _
        "statiattività.StatoAttività, " & _
        "entisedi.Denominazione AS Sede, " & _
        "cronologiaentità.usernamestato as UserPres, " & _
        "CONVERT(varchar, cronologiaentità.datacronologia, 103) AS DataPres, " & _
        "entisediattuazioni.Denominazione AS SedeAttuazione, " & _
        "comuni_2.Denominazione AS comuneSede, " & _
        "CONVERT(varchar, attività.DataInizioAttività, 103) AS DataInizioAssegnazione, " & _
        "CONVERT(varchar, attività.DataFineAttività, 103) AS DataFineAssegnazione, " & _
        "entità.Cognome + ' ' + entità.Nome AS Nominativo, " & _
        "entità.DataNascita, entità.usernamestato," & _
        "entità.TMPIdSedeAttuazioneOriginale, " & _
        "Causali.Descrizione, " & _
        "CONVERT(varchar, entità.DataUltimoStato, 103) AS DataUltimoStato, " & _
        "StatiEntità.StatoEntità, entità.idstatoentità, " & _
        "AttivitàSediAssegnazione.usernamestato as UserConf, " & _
        "CONVERT(varchar, AttivitàSediAssegnazione.DataUltimoStato, 103) AS DataConf, " & _
        "comuni_1.Denominazione AS ComuneNascita, " & _
        "SubentratoA.Cognome + ' ' + SubentratoA.Nome + ' [' + isnull(SubentratoA.CodiceVolontario,'') + ']' as Subentrato, " & _
        "SostituitoDa.Cognome + ' ' + SostituitoDa.Nome + ' [' + isnull(SostituitoDa.CodiceVolontario,'') + ']' as Sostituito, " & _
        "entità.Indirizzo + ' ' + entità.NumeroCivico + ' ' + comuni_res.Denominazione + ' ' + entità.cap + ' (' + provincie_res.DescrAbb + ')' AS IndirizzoVolontario, " & _
        "StatiAttestato.StatoAttestato, entità.UsernameAttestato, enti.codiceregione as CodReg, enti.denominazione as deno, convert(varchar,entità.DataAttestato,103) as DataAttestato, bando.BandoBreve as descrizioneBando, CASE ISNULL(BANDORICORSI.IDBANDORICORSO,0) WHEN 0 THEN BANDO.VOLONTARI ELSE BANDORICORSI.VOLONTARI END  as Volo, CASE ISNULL(BANDORICORSI.IDBANDORICORSO,0) WHEN 0 THEN BANDO.gazzettaufficiale ELSE BANDORICORSI.GAZZETTAUFFICIALE END  as Gazzetta  " & _
        ", ISNULL(entità.TMPIdSedeAttuazioneSecondaria,0) as TMPIdSedeAttuazioneSecondaria " & _
        "FROM comuni comuni_1 " & _
        "INNER JOIN entità ON comuni_1.IDComune = entità.IDComuneNascita " & _
        "INNER JOIN comuni comuni_res ON comuni_res.IDComune = entità.IDComuneResidenza " & _
        "INNER JOIN provincie provincie_res ON comuni_res.IDProvincia = provincie_res.IDProvincia " & _
        "LEFT OUTER JOIN StatiAttestato ON entità.IdStatoAttestato = StatiAttestato.IdStatoAttestato " & _
        "INNER JOIN provincie ON comuni_1.IDProvincia = provincie.IDProvincia " & _
        "INNER JOIN StatiEntità ON StatiEntità.IdStatoEntità = Entità.IdStatoEntità " & _
        "left join CronologiaSostituzioni a ON Entità.IdEntità = a.IdEntitàSubentrante " & _
        "left JOIN Entità SubentratoA ON a.IdEntitàSostituita = SubentratoA.IdEntità " & _
        "left join CronologiaSostituzioni b ON Entità.IdEntità = b.IdEntitàSostituita " & _
        "left JOIN Entità SostituitoDa ON b.IdEntitàSubentrante = SostituitoDa.IdEntità " & _
        "LEFT OUTER JOIN  attivitàentisediattuazione " & _
        "INNER JOIN attivitàentità ON attivitàentisediattuazione.IDAttivitàEnteSedeAttuazione = attivitàentità.IDAttivitàEnteSedeAttuazione " & _
        "INNER JOIN entisediattuazioni ON attivitàentisediattuazione.IDEnteSedeAttuazione = entisediattuazioni.IDEnteSedeAttuazione " & _
        "INNER JOIN attività ON attivitàentisediattuazione.IDAttività = attività.IDAttività " & _
        "INNER JOIN statiattività ON attività.IDStatoAttività = statiattività.IDStatoAttività " & _
        "INNER JOIN entisedi ON entisediattuazioni.IDEnteSede = entisedi.IDEnteSede " & _
        "INNER JOIN comuni comuni_2 ON entisedi.IDComune = comuni_2.IDComune " & _
        "ON entità.IDEntità = attivitàentità.IDEntità " & _
        "Left JOIN Causali ON entità.IDCausaleChiusura = Causali.IDCausale " & _
        "left join cronologiaentità on entità.identità = cronologiaentità.identità " & _
        "INNER JOIN GraduatorieEntità ON entità.IDEntità = GraduatorieEntità.IdEntità " & _
        "INNER JOIN AttivitàSediAssegnazione ON GraduatorieEntità.IdAttivitàSedeAssegnazione = AttivitàSediAssegnazione.IDAttivitàSedeAssegnazione " & _
        "INNER JOIN Enti ON attività.IDEntepresentante = enti.IDEnte " & _
        "INNER JOIN BandiAttività ON attività.IDBandoAttività = BandiAttività.IdBandoAttività " & _
        "INNER JOIN bando ON BandiAttività.IdBando = bando.IDBando " & _
        "LEFT JOIN BANDORICORSI ON attività.IDBANDORICORSO = BANDORICORSI.IDBANDORICORSO " & _
        "WHERE entità.IDEntità = " & idVol & " And cronologiaentità.idcronologiaentità = (select min(idcronologiaentità) from cronologiaentità where identità = " & idVol & ") " & _
        "Order by  convert(datetime,DataInizioAttivitàEntità,103) ,convert(datetime,DataFineAttivitàEntità,103) "

            'eseguo la query
            dtsDettagli = ClsServer.DataSetGenerico(strSql, Session("conn"))
            If dtsDettagli.Tables(0).Rows.Count > 0 Then
                If IsDBNull(dtsDettagli.Tables(0).Rows(0).Item("StatoEntità")) = False Then
                    lblStato.Text = dtsDettagli.Tables(0).Rows(0).Item("StatoEntità")
                End If
                If IsDBNull(dtsDettagli.Tables(0).Rows(0).Item("Descrizione")) = False Then
                    lblCausale.Text = dtsDettagli.Tables(0).Rows(0).Item("Descrizione")
                End If
                txtCodente.Text = "" & dtsDettagli.Tables(0).Rows(0).Item("CodReg")
                txtDenominazione.Text = "" & dtsDettagli.Tables(0).Rows(0).Item("deno")
                txtNominativo.Text = "" & dtsDettagli.Tables(0).Rows(0).Item("nominativo")
                txtComuneNascita.Text = "" & dtsDettagli.Tables(0).Rows(0).Item("ComuneNascita")
                txtDataNascita.Text = "" & dtsDettagli.Tables(0).Rows(0).Item("DataNascita")
                txtCF.Text = dtsDettagli.Tables(0).Rows(0).Item("CF")
                txtIndirizzo.Text = dtsDettagli.Tables(0).Rows(0).Item("IndirizzoVolontario")
                lblNomeBando.Text = dtsDettagli.Tables(0).Rows(0).Item("descrizioneBando")
                If IsDBNull(dtsDettagli.Tables(0).Rows(0).Item("Volo")) = False Then
                    lblNVol.Text = dtsDettagli.Tables(0).Rows(0).Item("Volo")
                End If
                If IsDBNull(dtsDettagli.Tables(0).Rows(0).Item("Gazzetta")) = False Then
                    lblGazzetta.Text = dtsDettagli.Tables(0).Rows(0).Item("Gazzetta")
                End If

                'INFO Presentazione
                If IsDBNull(dtsDettagli.Tables(0).Rows(0).Item("userPres")) = False Then
                    txtUPresentazione.Text = dtsDettagli.Tables(0).Rows(0).Item("userPres")
                End If
                If IsDBNull(dtsDettagli.Tables(0).Rows(0).Item("DataPres")) = False Then
                    txtDPresentazione.Text = dtsDettagli.Tables(0).Rows(0).Item("DataPres")
                End If
                'INFO Conferma
                If IsDBNull(dtsDettagli.Tables(0).Rows(0).Item("userConf")) = False Then
                    txtUConferma.Text = dtsDettagli.Tables(0).Rows(0).Item("userConf")
                End If
                If IsDBNull(dtsDettagli.Tables(0).Rows(0).Item("DataConf")) = False Then
                    txtDConferma.Text = dtsDettagli.Tables(0).Rows(0).Item("DataConf")
                End If
                'INFO Chiusura
                If CInt(dtsDettagli.Tables(0).Rows(0).Item("idstatoentità")) > 3 Then
                    If IsDBNull(dtsDettagli.Tables(0).Rows(0).Item("usernamestato")) = False Then
                        txtUChiusura.Text = dtsDettagli.Tables(0).Rows(0).Item("usernamestato")
                    End If
                    If IsDBNull(dtsDettagli.Tables(0).Rows(0).Item("DataUltimoStato")) = False Then
                        txtDChiusura.Text = dtsDettagli.Tables(0).Rows(0).Item("DataUltimoStato")
                    End If
                End If
                'INFO Sub. e Sost.
                If IsDBNull(dtsDettagli.Tables(0).Rows(0).Item("Subentrato")) = False Then
                    txtSubentrato.Text = dtsDettagli.Tables(0).Rows(0).Item("Subentrato")
                End If
                If IsDBNull(dtsDettagli.Tables(0).Rows(0).Item("Sostituito")) = False Then
                    txtSostituito.Text = dtsDettagli.Tables(0).Rows(0).Item("Sostituito")
                End If

                If IsDBNull(dtsDettagli.Tables(0).Rows(0).Item("StatoAttestato")) = False Then
                    txtStatoAttestato.Text = dtsDettagli.Tables(0).Rows(0).Item("StatoAttestato")
                End If

                If IsDBNull(dtsDettagli.Tables(0).Rows(0).Item("DataAttestato")) = False Then
                    txtDataAttestato.Text = dtsDettagli.Tables(0).Rows(0).Item("DataAttestato")
                End If

                If IsDBNull(dtsDettagli.Tables(0).Rows(0).Item("UsernameAttestato")) = False Then
                    txtUserAttestato.Text = dtsDettagli.Tables(0).Rows(0).Item("UsernameAttestato")
                End If


                If IsDBNull(dtsDettagli.Tables(0).Rows(0).Item("Titolo")) = True Then
                    lblError.Text = "Nessuna attività è associata al volonatrio selezionato!"
                End If
                'assegno il dataset alla griglia del risultato


                dtgRisultatoRicerca.DataSource = dtsDettagli
                Session("appDtsRisRicerca") = dtsDettagli

                dtgRisultatoRicerca.DataBind()
                If dtgRisultatoRicerca.Items.Count = 0 Then
                    lblRisultato.Visible = False
                Else
                    lblRisultato.Visible = True
                End If
                'SANDOKAN
            If IsDBNull(dtsDettagli.Tables(0).Rows(0).Item("TMPIdSedeAttuazioneOriginale")) Then
                DtgStoricoAssegnazione.Visible = False
                lblStoricoAssegnazioni.Visible = False
            Else
                DtgStoricoAssegnazione.Visible = True
                lblStoricoAssegnazioni.Visible = True
                Call CaricaDtgStoricoAssociazione(Request.Params("IdVolontario"))
            End If
            If dtsDettagli.Tables(0).Rows(0).Item("TMPIdSedeAttuazioneSecondaria") <> 0 Then

                CaricaSedeSecondaria(dtsDettagli.Tables(0).Rows(0).Item("TMPIdSedeAttuazioneSecondaria"))
            End If
        End If
        If dtgRisultatoRicerca.Items.Count > 0 Then
            DivDatiVolontario.Visible = True
        Else
            DivDatiVolontario.Visible = False
        End If


    End Sub
    Private Sub CaricaDtgStoricoAssociazione(ByVal idVol As Integer)
        Dim strSql As String
        strSql = "SELECT " & _
        "entità.IDEntità, entità.DataAssegnazioneAltraSede, entità.TMPIdSedeAttuazioneOriginale, attività.Titolo + ' [' + Attività.CodiceEnte + ']' as Titolo, attività.DataInizioAttività, " & _
        "attività.DataFineAttività , entità.IDAttivitàSedeAssegnazioneOriginale, entità.UserNameAssegnazioneAltraSede, entisedi.Denominazione, enti.Denominazione AS DenominazioneEnte, enti.CodiceRegione, comuni.Denominazione as ComuneSede " & _
        "FROM attività " & _
        "INNER JOIN AttivitàSediAssegnazione ON attività.IDAttività = AttivitàSediAssegnazione.IDAttività " & _
        "INNER JOIN entità ON AttivitàSediAssegnazione.IDAttivitàSedeAssegnazione = entità.IDAttivitàSedeAssegnazioneOriginale " & _
        "INNER JOIN entisedi ON AttivitàSediAssegnazione.IDEnteSede = entisedi.IDEnteSede " & _
        "INNER JOIN enti ON attività.IDEntePresentante = enti.IDEnte " & _
        "INNER JOIN comuni ON entisedi.IDComune = comuni.IDComune " & _
        "WHERE entità.IDEntità = " & idVol

        dtsStoricoAssegnazione = ClsServer.DataSetGenerico(strSql, Session("conn"))
        Session("appDtsStoricoAssegnazione") = dtsStoricoAssegnazione
        DtgStoricoAssegnazione.DataSource = dtsStoricoAssegnazione
        DtgStoricoAssegnazione.DataBind()
        If DtgStoricoAssegnazione.Items.Count = 0 Then
            lblStoricoAssegnazioni.Visible = False
        Else
            lblStoricoAssegnazioni.Visible = True
        End If
    End Sub

    Private Sub CaricaSedeSecondaria(ByVal IdSedeSecondaria As Integer)
        Dim drt As SqlClient.SqlDataReader
        Dim strSql As String

        strSql = "SELECT 'Cod.' + convert(varchar,esa.IDEnteSedeAttuazione) + ' - ' + convert(varchar,es.Denominazione) "
        strSql &= " + ' ' + convert(varchar,es.Indirizzo) + ' ' + convert(varchar,es.civico) + ' ' +  convert(varchar,Es.CAP) "
        strSql &= " + ' ' + convert(varchar,c.Denominazione) as Sede"
        strSql &= " from entisedi es"
        strSql &= " inner join entisediattuazioni esa on es.IDEnteSede=esa.IDEnteSede"
        strSql &= " inner join comuni c on c.IDComune =es.IDComune"
        strSql &= " inner join provincie p on p.IDProvincia =c.IDProvincia"
        strSql &= " WHERE esa.IDEnteSedeAttuazione = " & IdSedeSecondaria

        drt = ClsServer.CreaDatareader(strSql, Session("conn"))
        If drt.HasRows = True Then
            drt.Read()
            LblSedeSecondaria.Text = drt("Sede")
        End If

        drt.Close()
        drt = Nothing

    End Sub


    Private Sub imgCronoDocu_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles imgCronoDocu.Click
        Response.Redirect("WfrmCronologiaDocumentiVolontario.aspx?IdVol=" & Request.Params("IdVolontario") & " &Nominativo=" & txtNominativo.Text)
    End Sub

    Private Sub Imagebutton1_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles Imagebutton1.Click
        Response.Redirect("WfrmCronologiaDettaglioAnagrafica.aspx?IdVol=" & Request.Params("IdVolontario") & " &Nominativo=" & txtNominativo.Text)
    End Sub
    Protected Sub imgCronoRiattivazione_Click(sender As Object, e As EventArgs) Handles imgCronoRiattivazione.Click
        Response.Redirect("WfrmCronologiaRiattivazione.aspx?IdVol=" & Request.Params("IdVolontario") & " &Nominativo=" & txtNominativo.Text)
    End Sub


    Public Function abilitalinkRimodulazione(ByVal identità As Integer)
        'SP_RIATTIVAZIONE_VERIFICA_CRONO
        Dim strUtente As String
        Dim SqlCmd As New SqlClient.SqlCommand


        Try
            SqlCmd.CommandText = "SP_RIMODULAZIONEPROGETTI_VERIFICA_CRONO"
            SqlCmd.CommandType = CommandType.StoredProcedure
            SqlCmd.Connection = Session("Conn")

            SqlCmd.Parameters.Add("@IdEntità", SqlDbType.Int).Value = identità

            SqlCmd.Parameters.Add("@Esito", SqlDbType.VarChar, 5)
            SqlCmd.Parameters("@Esito").Direction = ParameterDirection.Output



            SqlCmd.ExecuteNonQuery()

            Return SqlCmd.Parameters("@Esito").Value()


        Catch ex As Exception
            'Esito = "ERR"

        Finally

        End Try
    End Function

    Public Function abilitalink(ByVal identità As Integer)
        'SP_RIATTIVAZIONE_VERIFICA_CRONO
        Dim strUtente As String
        Dim SqlCmd As New SqlClient.SqlCommand


        Try
            SqlCmd.CommandText = "SP_RIATTIVAZIONE_VERIFICA_CRONO"
            SqlCmd.CommandType = CommandType.StoredProcedure
            SqlCmd.Connection = Session("Conn")

            SqlCmd.Parameters.Add("@IdEntità", SqlDbType.Int).Value = identità

            SqlCmd.Parameters.Add("@Esito", SqlDbType.VarChar, 5)
            SqlCmd.Parameters("@Esito").Direction = ParameterDirection.Output



            SqlCmd.ExecuteNonQuery()

            Return SqlCmd.Parameters("@Esito").Value()


        Catch ex As Exception
            'Esito = "ERR"

        Finally

        End Try
    End Function

    Private Sub imgCronoRimodulazione_Click(sender As Object, e As System.EventArgs) Handles imgCronoRimodulazione.Click
        Response.Redirect("WfrmCronologiaRimodulazioneProgetti.aspx?IdVol=" & Request.Params("IdVolontario") & " &Nominativo=" & txtNominativo.Text)
    End Sub
End Class