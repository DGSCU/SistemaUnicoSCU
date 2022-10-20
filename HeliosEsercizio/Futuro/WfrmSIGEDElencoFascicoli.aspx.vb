Public Class WfrmSIGEDElencoFascicoli
    Inherits System.Web.UI.Page

    Dim dt As New DataTable
    Shared strNome As String
    Shared strCognome As String
    Shared intNavigazione As Integer
    Dim IDSessioneSIGED As String
    Dim SIGED As clsSiged

    Protected WithEvents dgRisultatoRicerca As System.Web.UI.WebControls.DataGrid

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Not Session("LogIn") Is Nothing Then
            If Session("LogIn") = False Then 'verifico validità log-in
                Response.Redirect("LogOn.aspx")
            End If
        Else
            Response.Redirect("LogOn.aspx")
        End If
        If Page.IsPostBack = False Then
            CaricaAnno()
            CaricaTitolario(Request.QueryString("Processo"))
            If Request.QueryString("CodiceVolontario") <> "" Then
                If Request.QueryString("Maschera") = "Volontario" Then
                    txtDescFasc.Text = "%" & Request.QueryString("CodiceVolontario")
                    cboTitolario.SelectedValue = ForzaTitolarioVolontari(Request.QueryString("CodiceVolontario"))
                End If
            End If

            If Request.QueryString("Processo") = "MONITORAGGIO" Then
                cboTitolario.SelectedValue = ForzaTitolarioVerifiche("TITOLARIO_VERIFICHE")
            End If

        End If
    End Sub
    'Declaration
    Public Overridable Sub Sort( _
    ByVal comparer As IComparer _
    )

    End Sub

    Private Function ForzaTitolarioVerifiche(ByVal Parametro As String) As String
        'creata da simona cordella il 19/09/2016 per la visualizazione del titolario delle verifiche
        Dim strSQL As String
        Dim dtrGenerico As SqlClient.SqlDataReader
        Dim Titolario As String
        If Not dtrGenerico Is Nothing Then
            dtrGenerico.Close()
            dtrGenerico = Nothing
        End If
        strSQL = " Select Valore"
        strSQL &= " FROM  Configurazioni "

        strSQL &= " WHERE Parametro = '" & Parametro & "'"
        dtrGenerico = ClsServer.CreaDatareader(strSQL, Session("conn"))
        If dtrGenerico.HasRows = True Then
            dtrGenerico.Read()
            Titolario = dtrGenerico("valore")
        End If
        If Not dtrGenerico Is Nothing Then
            dtrGenerico.Close()
            dtrGenerico = Nothing
        End If
        Return Titolario

    End Function

    Private Sub dgRisultatoRicerca_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgRisultatoRicerca.ItemCommand

        Select Case e.CommandName
            Case "Select1"
                Dim pNumeroFascicolo As String = "" & e.Item.Cells(1).Text.Replace("&nbsp;", "")
                Dim pCodiceFascicolo As String
                Dim pDescFascicolo As String = "" & e.Item.Cells(2).Text.Replace("&nbsp;", "")

                pCodiceFascicolo = "" & e.Item.Cells(4).Text.Replace("&nbsp;", "")
                pDescFascicolo = pDescFascicolo.Replace(Chr(10), " ")           'rimuovo il carattere di INVIO

 

 
                Response.Write("<script >" & vbCrLf)

                Response.Write("window.opener.document.getElementById(""" & Request.QueryString("objNumero") & """).value='" & pNumeroFascicolo & "';") '& vbcrlf
                Response.Write("window.opener.document.getElementById(""" & Request.QueryString("objCodice") & """).value='" & pCodiceFascicolo & "';") '& vbcrlf
                Response.Write("window.opener.document.getElementById(""" & Request.QueryString("objDescFasc") & """).value='" & pDescFascicolo.Replace("'", "") & "';") '& vbcrlf

                If Request.QueryString("VArUpdate") = 1 Then
                    Call UpdateFascicoloEntità(pNumeroFascicolo, pCodiceFascicolo, pDescFascicolo.Replace("'", ""), Request.QueryString("VarVolontario"))
                ElseIf Request.QueryString("VArUpdate") = 2 Then 'agg. da sc per salvataggio in cronologiaentidocumenti
                    Call SalvaProt(pNumeroFascicolo, pCodiceFascicolo, pDescFascicolo)
                ElseIf Request.QueryString("VArUpdate") = 3 Then 'agg. da sc per salvataggio in cronologiaentidocumenti
                    Call UpdateFascicoloProgetto(pNumeroFascicolo, pCodiceFascicolo, pDescFascicolo.Replace("'", ""), Request.QueryString("IdAttivita"))
                ElseIf Request.QueryString("VArUpdate") = 4 Then
                    Call UpdateFascicoloEnte(pNumeroFascicolo, pCodiceFascicolo, pDescFascicolo.Replace("'", ""))
                End If

                Response.Write("window.close()")


                Response.Write("</script>")
            Case "Navigazione"
                Navigazione("" & e.Item.Cells(4).Text.Replace("&nbsp;", ""))
        End Select

    End Sub

    Private Sub SalvaProt(ByVal pNumeroFascicolo As String, ByVal pCodiceFascicolo As String, ByVal pDescFascicolo As String)
        If Request.QueryString("Varsalva") = 1 Then
            'Response.Write("window.opener.location.reload();" & vbCrLf)
            Dim strLocal As String
            Dim dtrCancellazione As SqlClient.SqlDataReader
            Dim mycommand As New SqlClient.SqlCommand
            Dim mydatatable As New DataTable

            mycommand.Connection = Session("conn")

            'cancella
            strLocal = "select IDAttivitàSedeAssegnazione from attività inner join attivitàsediassegnazione on " & _
            "attività.idattività = attivitàsediassegnazione.idattività where(attività.idattività = " & Request.QueryString("IdAttivita") & ")"
            Try
                'If varcancella = True Then ' cancella anche le associazioni precedenti
                mydatatable = ClsServer.CreaDataTable(strLocal, False, Session("conn"))

                Dim k As Int16

                For k = 0 To mydatatable.Rows.Count - 1
                    strLocal = "update cronologiaentidocumenti set dataprot =null,  nprot = null " & _
                        " where  tipodocumento = 2 and IDAttivitàSedeAssegnazione = " & mydatatable.Rows(k).Item("IDAttivitàSedeAssegnazione")

                    mycommand.CommandText = strLocal
                    mycommand.ExecuteNonQuery()
                Next
                '*******************************************************************************
                'End If
            Catch ex As Exception
                'Response.Write(ex.Message.ToString())
            End Try

            strLocal = "update attività  set codicefascicoloai ='" & pNumeroFascicolo & "', idfascicoloai='" & pCodiceFascicolo & _
            "', descrfascicoloai='" & Replace(pDescFascicolo, "'", "''") & "' where IdAttività = " & Request.QueryString("IdAttivita") & ""
            mycommand.CommandText = strLocal
            mycommand.ExecuteNonQuery()
        End If
    End Sub

    Private Sub dgRisultatoRicerca_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgRisultatoRicerca.PageIndexChanged
        dgRisultatoRicerca.SelectedIndex = -1
        dgRisultatoRicerca.CurrentPageIndex = e.NewPageIndex
        dgRisultatoRicerca.DataSource = Session("dtRisulatato")
        dgRisultatoRicerca.DataBind()
    End Sub

    Private Sub cmdCerca_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCerca.Click
        lblmessaggio.Text = ""

        If ValidaCampi() = True Then
            Ricerca()
        End If
        cmdCerca.Visible = True
    End Sub

    Private Function NormalizzaTitolario(ByVal strTitolario) As String
        If strTitolario <> "" Then
            Dim strAppoTitolario As String
            strAppoTitolario = Mid(strTitolario, 1, InStr(strTitolario, "-") - 1)
            Return strAppoTitolario
        Else
            Return ""
        End If
    End Function
    Sub UpdateFascicoloEnte(ByVal strNumeroFascicolo As String, ByVal IdFascicolo As String, ByVal DescrizioneFascicolo As String)
        Dim strSQL As String
        Dim CmdGenerico As SqlClient.SqlCommand
        strSQL = " Update enti set CodiceFascicolo= '" & strNumeroFascicolo & "', " & _
                 " IDFascicolo ='" & IdFascicolo & "', " & _
                 " DescrFascicolo = '" & Replace(DescrizioneFascicolo, "'", "''") & "' " & _
                 " where idente = " & Session("IdEnte")
        CmdGenerico = ClsServer.EseguiSqlClient(strSQL, Session("conn"))

        

    End Sub
    Sub UpdateFascicoloEntità(ByVal strNumeroFascicolo As String, ByVal IdFascicolo As String, ByVal DescrizioneFascicolo As String, ByVal strIdEntita As String)
        Dim strSQL As String
        Dim CmdGenerico As SqlClient.SqlCommand
        strSQL = " Update entità set CodiceFascicolo= '" & strNumeroFascicolo & "', " & _
                 " IDFascicolo ='" & IdFascicolo & "', " & _
                 " DescrFascicolo = '" & Replace(DescrizioneFascicolo, "'", "''") & "' " & _
                 " where identità=" & strIdEntita & ""
        CmdGenerico = ClsServer.EseguiSqlClient(strSQL, Session("conn"))
    End Sub

    Sub UpdateFascicoloProgetto(ByVal strNumeroFascicolo As String, ByVal IdFascicolo As String, ByVal DescrizioneFascicolo As String, ByVal IdAttività As Integer)
        Dim strSQL As String
        Dim CmdGenerico As SqlClient.SqlCommand

        strSQL = " Update attività set CodiceFascicoloAI= '" & strNumeroFascicolo & "', " & _
                 " IDFascicoloAI ='" & IdFascicolo & "', " & _
                 " DescrFascicoloAI = '" & Replace(DescrizioneFascicolo, "'", "''") & "' " & _
                 " where idattività=" & IdAttività & ""
        CmdGenerico = ClsServer.EseguiSqlClient(strSQL, Session("conn"))
    End Sub

    Sub CaricaAnno()
        'creata da simona cordella il 13/12/2010
        'l'anno viene caricato della vista VW_Titolario presente in SQL
        Dim strSQL As String
        Dim dtrGenerico As SqlClient.SqlDataReader

        strSQL = " SELECT DISTINCT ANNO FROM VW_TITOLARIO ORDER BY ANNO DESC"
        dtrGenerico = ClsServer.CreaDatareader(strSQL, Session("Conn"))

        cboAnno.DataTextField = "Anno"
        cboAnno.DataSource = dtrGenerico
        cboAnno.DataBind()

        If Not dtrGenerico Is Nothing Then
            dtrGenerico.Close()
            dtrGenerico = Nothing
        End If
    End Sub
    Sub CaricaTitolario(ByVal strProcesso As String)
        'creata da simona cordella il 13/12/2010
        'i titolari vengono caricati della vista VW_Titolario presente in SQL
        Dim strSQL As String
        Dim dtrGenerico As SqlClient.SqlDataReader

        If Not dtrGenerico Is Nothing Then
            dtrGenerico.Close()
            dtrGenerico = Nothing
        End If

        strSQL = " SELECT Codice,DESCRIZIONE,LIV1,LIV2,LIV3,LIV4 FROM VW_TITOLARIO " & _
                 " Where Anno = " & cboAnno.SelectedValue & " and processo ='" & strProcesso & "' " & _
                 " UNION " & _
                 " Select '' ,'','','','','' From VW_TITOLARIO  " & _
                 " Where Anno = " & cboAnno.SelectedValue & " and processo ='" & strProcesso & "'" & _
                 " order by LIV1,LIV2,LIV3,LIV4 "
        dtrGenerico = ClsServer.CreaDatareader(strSQL, Session("Conn"))

        cboTitolario.DataTextField = "Descrizione"
        cboTitolario.DataValueField = "Codice"
        cboTitolario.DataSource = dtrGenerico
        cboTitolario.DataBind()

        'strSQL = " SELECT Codice,DESCRIZIONE FROM VW_TITOLARIO " & _
        '         " Where Anno = " & cboAnno.SelectedValue & " and processo ='" & strProcesso & "' order by codice"
        'dtrGenerico = ClsServer.CreaDatareader(strSQL, Session("Conn"))

        'cboTitolario.DataTextField = "Descrizione"
        'cboTitolario.DataValueField = "Codice"
        'cboTitolario.DataSource = dtrGenerico
        'cboTitolario.DataBind()

        If Not dtrGenerico Is Nothing Then
            dtrGenerico.Close()
            dtrGenerico = Nothing
        End If

    End Sub
    Private Sub cboAnno_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboAnno.SelectedIndexChanged
        CaricaTitolario(Request.QueryString("Processo"))
    End Sub

    Sub Ricerca()
        Dim strSQL As String
        Dim dsUser As DataSet
        Dim NumeroFascicolo As String
        Try

            'aggiunto da simona cordella il 13/12/2010
            imgBack.Visible = False
            intNavigazione = 0
            ReDim Session("IDNav_CodFasciolo")(0)
            ReDim Session("Nav_CodFasciolo")(0)

            Session("IDNav_CodFasciolo")(0) = intNavigazione
            Session("Nav_CodFasciolo")(0) = ""

            lblmessaggio.Text = ""
            dgRisultatoRicerca.CurrentPageIndex = 0

            Dim dr As DataRow
            Dim riga As Integer
            Dim strDataDa As String = txtDataDa.Text
            Dim strDataA As String = txtDataA.Text
            Dim wsOUT As WS_SIGeD.MULTI_FASCICOLO
            Dim wsELENCO() As WS_SIGeD.FASCICOLO_DOCUMENTO_TROVATO
            Dim sEsito As String

            If strDataDa <> "" Then
                strDataDa = Year(Date.Parse(strDataDa)) & "-" & Month(Date.Parse(strDataDa)) & "-" & Day(Date.Parse(strDataDa))
            End If

            If strDataA <> "" Then
                strDataA = Year(Date.Parse(strDataA)) & "-" & Month(Date.Parse(strDataA)) & "-" & Day(Date.Parse(strDataA))
            End If

            'modificato il 30/08/2011
            'estraggo il nome e cognome dell'utente loggato
            strSQL = "Select Nome, Cognome From UtentiUNSC Where UserName='" & Session("Utente") & "'"
            dsUser = ClsServer.DataSetGenerico(strSQL, Session("conn"))

            If dsUser.Tables(0).Rows.Count <> 0 Then
                strNome = dsUser.Tables(0).Rows(0).Item("Nome")
                strCognome = dsUser.Tables(0).Rows(0).Item("Cognome")
            End If
            If Trim(txtCodFasc.Text) <> "" Then
                NumeroFascicolo = Trim(cboTitolario.SelectedValue) & "/" & Trim(txtCodFasc.Text)
            Else
                NumeroFascicolo = ""
            End If
            SIGED = New clsSiged("", strNome, strCognome)
            If SIGED.Codice_Esito = 0 Then
                IDSessioneSIGED = SIGED.Esito
                wsOUT = SIGED.SIGED_Ricerca_Fascicoli(txtDescFasc.Text, txtNominativo.Text, Trim(cboTitolario.SelectedValue), NumeroFascicolo, strDataDa, strDataA)
            Else
                lblmessaggio.Text = SIGED.Esito
            End If

            sEsito = SIGED.SIGED_Codice_Esito(wsOUT.ESITO)
            If sEsito = 0 Then
                If SIGED.Esito_Occorrenze(wsOUT.ESITO) = "0" Then
                    'SIGED_Esito_Occorrenze
                    lblmessaggio.Text = "Nessun fascicolo trovato"
                    dgRisultatoRicerca.Visible = False
                Else
                    dgRisultatoRicerca.Visible = True
                    wsELENCO = wsOUT.ELENCO_DOCUMENTI

                    dt.Columns.Add(New DataColumn("N°. Fascicolo", GetType(String)))
                    dt.Columns.Add(New DataColumn("Descrizione", GetType(String)))
                    dt.Columns.Add(New DataColumn("Titolario", GetType(String)))
                    dt.Columns.Add(New DataColumn("Cod_Fasc", GetType(String)))

                    For riga = LBound(wsELENCO) To UBound(wsELENCO)
                        'L'utente loggato ha accesso al fascicolo

                        SIGED.NormalizzaCodice(wsELENCO(riga).CODICEFASCICOLO)
                        dr = dt.NewRow
                        dr(0) = Replace(wsELENCO(riga).NUMERO, " ", "")
                        dr(1) = wsELENCO(riga).DESCRIZIONE.ToString
                        dr(2) = wsELENCO(riga).TITOLARIO
                        dr(3) = SIGED.CodiceNormalizzato
                        dt.Rows.Add(dr)

                    Next

                    dgRisultatoRicerca.DataSource = dt
                    dgRisultatoRicerca.DataBind()
                    lblRisultatoRicerca.Visible = True
                    Session("dtRisulatato") = dt

                End If
            Else
                If Left(wsOUT.ESITO, 5) = "02001" Then
                    lblmessaggio.Text = "Attenzione, la ricerca ha prodotto un numero elevato di dati. Si prega di ottimizzarla valorizzando ulteriori filtri."
                Else
                    lblmessaggio.Text = Mid(wsOUT.ESITO, 6, Len(wsOUT.ESITO))
                End If
                'lblmessaggio.Text = Mid(wsOUT.ESITO, 6, Len(wsOUT.ESITO))
                dgRisultatoRicerca.Visible = False
            End If
            SIGED.SIGED_Chiudi_Autenticazione(strNome, strCognome)

        Catch ex As Exception
            lblmessaggio.Text = "Errore imprevisto: " & ex.Message
        End Try
    End Sub

    Function ValidaCampi() As Boolean
        Dim dataDa As Date
        Dim dataA As Date
        Dim campiValidi As Boolean = True
        Dim errore As String = "Il campo '{0}' contiene una data non valida. Immettere la data nel formato gg/mm/aaaa. <br/>"
        If txtDataDa.Text <> String.Empty And Date.TryParse(txtDataDa.Text, dataDa) = False Then
            lblmessaggio.Text = String.Format(errore, lblTxtDataDa.Text.Replace(":", ""))
            campiValidi = False
        ElseIf txtDataA.Text <> String.Empty And Date.TryParse(txtDataA.Text, dataA) = False Then
            lblmessaggio.Text = String.Format(errore, lblTxtDataA.Text.Replace(":", ""))
            campiValidi = False
        End If
        If cboTitolario.Text = String.Empty Then
            lblmessaggio.Text = lblmessaggio.Text & "Selezionare un titolario. <br/>"
            campiValidi = False
        End If

        Return campiValidi
    End Function

    


    Sub Navigazione(ByVal CodFascicolo As String, Optional ByVal Nav As Integer = 0)
        'creato da simona cordella il 13/12/2010
        'la funzione viene richiamata dal pulsante Navigazione della griglia che consente di visalizzare i fascicoli del fascicolo selezionata.
        Try
            imgBack.Visible = True
            lblmessaggio.Text = ""
            dgRisultatoRicerca.CurrentPageIndex = 0

            Dim wsOUT As New WS_SIGED.INDICE_FASCICOLO
            Dim wsELENCO() As WS_SIGED.COLLEGAMENTO_DOCUMENTO_TROVATO

            Dim sEsito As String

            Dim dr As DataRow
            Dim riga As Integer
            Dim strDataDa As String = txtDataDa.Text
            Dim strDataA As String = txtDataA.Text

            If strDataDa <> "" Then
                strDataDa = Year(Date.Parse(strDataDa)) & "-" & Month(Date.Parse(strDataDa)) & "-" & Day(Date.Parse(strDataDa))
            End If

            If strDataA <> "" Then
                strDataA = Year(Date.Parse(strDataA)) & "-" & Month(Date.Parse(strDataA)) & "-" & Day(Date.Parse(strDataA))
            End If
            SIGED = New clsSiged("", strNome, strCognome)

            wsOUT = SIGED.SIGED_IndiceFascicoli(CodFascicolo)
            sEsito = SIGED.SIGED_Codice_Esito(wsOUT.ESITO)

            If sEsito = 0 Then
                If Nav = 0 Then
                    intNavigazione = intNavigazione + 1
                    ReDim Preserve Session("IDNav_CodFasciolo")(UBound(Session("IDNav_CodFasciolo")) + 1)
                    ReDim Preserve Session("Nav_CodFasciolo")(UBound(Session("Nav_CodFasciolo")) + 1)
                    Session("IDNav_CodFasciolo")(UBound(Session("IDNav_CodFasciolo"))) = intNavigazione
                    Session("Nav_CodFasciolo")(UBound(Session("Nav_CodFasciolo"))) = CodFascicolo
                End If
                If Right(wsOUT.ESITO, 1) = "0" Then
                    'If SIGED.Esito_Occorrenze(wsOUT.ESITO) = "0" Then
                    lblmessaggio.Text = "Nessun fascicolo trovato"
                    dgRisultatoRicerca.Visible = False
                    lblRisultatoRicerca.Visible = False
                Else
                    dgRisultatoRicerca.Visible = True
                    lblRisultatoRicerca.Visible = True
                    wsELENCO = wsOUT.ELENCO_DOCUMENTI

                    dt.Columns.Add(New DataColumn("N°. Fascicolo", GetType(String)))
                    dt.Columns.Add(New DataColumn("Descrizione", GetType(String)))
                    dt.Columns.Add(New DataColumn("Titolario", GetType(String)))
                    dt.Columns.Add(New DataColumn("Cod_Fasc", GetType(String)))
                    'filtrare solo per i fascicolo "2011#FASC#1018/2011" leggere se è   FASC
                    If Not wsOUT.ELENCO_DOCUMENTI Is Nothing Then
                        For riga = LBound(wsELENCO) To UBound(wsELENCO)
                            If wsELENCO(riga).TIPOCOLLEGAMENTO = "Fascicolo" Then
                                dr = dt.NewRow
                                dr(0) = wsELENCO(riga).DETTAGLICOLLEGAMENTO.ToString 'numerofascicolo con titolario 
                                dr(1) = wsELENCO(riga).DESCRIZIONECOLLEGAMENTO.ToString
                                dr(2) = Replace(Mid(wsELENCO(riga).DETTAGLICOLLEGAMENTO.ToString, 1, InStr(wsELENCO(riga).DETTAGLICOLLEGAMENTO.ToString, "/") - 1), " ", "")
                                dr(3) = wsELENCO(riga).CODICEDOCUMENTOCOLLEGATO
                                dt.Rows.Add(dr)
                            End If
                        Next
                    End If
                    dgRisultatoRicerca.DataSource = dt
                    dgRisultatoRicerca.DataBind()
                    Session("dtRisulatato") = dt

                End If
            Else
                lblmessaggio.Text = Mid(wsOUT.ESITO, 6, Len(wsOUT.ESITO))
                dgRisultatoRicerca.Visible = False
                lblRisultatoRicerca.Visible = False
            End If

        Catch ex As Exception
            lblmessaggio.Text = "Errore imprevisto: " & ex.Message
        Finally
            SIGED.SIGED_Chiudi_Autenticazione(strNome, strCognome)
        End Try
    End Sub

    Private Function NormalizzaDescrizioneFascicolo(ByVal DescrFascicolo As String) As String

        Dim sDescrFascicolo() As String
        Dim strNumFascicolo As String
        Dim i As Integer

        sDescrFascicolo = Split(DescrFascicolo, "-")
        For i = 0 To UBound(sDescrFascicolo)
            If i = UBound(sDescrFascicolo) Then
                strNumFascicolo = sDescrFascicolo(i)
            End If
        Next
        Return strNumFascicolo

    End Function

    Private Sub imgBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles imgBack.Click
        'aggiunto da simona cordella il 13/12/2010
        If UBound(Session("IDNav_CodFasciolo")) > 1 Then
            ReDim Preserve Session("IDNav_CodFasciolo")(UBound(Session("IDNav_CodFasciolo")) - 1)
            ReDim Preserve Session("Nav_CodFasciolo")(UBound(Session("Nav_CodFasciolo")) - 1)
            Navigazione(Session("Nav_CodFasciolo")(UBound(Session("Nav_CodFasciolo"))), 1)
        Else
        Ricerca()
        End If
    End Sub

    Private Function ForzaTitolarioVolontari(ByVal CodiceVolontario As String) As String
        'creata da simona cordella il 30/09/2015 per la visualizazione del titolario dalla scheda del volontario
        Dim strSQL As String
        Dim dtrGenerico As SqlClient.SqlDataReader
        Dim TitolarioVolontario As String
        If Not dtrGenerico Is Nothing Then
            dtrGenerico.Close()
            dtrGenerico = Nothing
        End If
        strSQL = " Select bando.titolario "
        strSQL &= " FROM   entità "
        strSQL &= " INNER JOIN GraduatorieEntità ON entità.IDEntità = GraduatorieEntità.IdEntità "
        strSQL &= " INNER JOIN AttivitàSediAssegnazione ON GraduatorieEntità.IdAttivitàSedeAssegnazione = AttivitàSediAssegnazione.IDAttivitàSedeAssegnazione "
        strSQL &= " INNER JOIN attività ON AttivitàSediAssegnazione.IDAttività = attività.IDAttività "
        strSQL &= " INNER JOIN BandiAttività ON attività.IDBandoAttività = BandiAttività.IdBandoAttività "
        strSQL &= " INNER JOIN bando ON BandiAttività.IdBando = bando.IDBando "
        strSQL &= " WHERE entità.CodiceVolontario = '" & CodiceVolontario & "'"
        dtrGenerico = ClsServer.CreaDatareader(strSQL, Session("conn"))
        If dtrGenerico.HasRows = True Then
            dtrGenerico.Read()
            TitolarioVolontario = Replace(NormalizzaTitolario(dtrGenerico("titolario")), " ", "")
        End If
        If Not dtrGenerico Is Nothing Then
            dtrGenerico.Close()
            dtrGenerico = Nothing
        End If
        Return TitolarioVolontario

    End Function

    Protected Sub dgRisultatoRicerca_SelectedIndexChanged(sender As Object, e As EventArgs) Handles dgRisultatoRicerca.SelectedIndexChanged

    End Sub
End Class