Public Class WfrmRicercaProgettiRicollocamento
    Inherits System.Web.UI.Page

    Public blnForza As Boolean

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Inserire qui il codice utente necessario per inizializzare la pagina
        If Not Session("LogIn") Is Nothing Then
            If Session("LogIn") = False Then 'verifico validità log-in
                Response.Redirect("LogOn.aspx")
            End If
        Else
            Response.Redirect("LogOn.aspx")
        End If
        If IsPostBack = False Then
            CaricaComboSettore()
        End If
        '***  agg. da simona cordella il 11/07/2011
        'se l'utene NON è abilitato al menu FORZA PRESENZA SANZIONE 
        'nn visualizzo il filtro di ricerca (aspx) Presenza Sanzione
        blnForza = ClsUtility.ForzaPresenzaSanzione(Session("Utente"), Session("conn"))
        '***

        If blnForza = True Then
            divlblSegnalazioneSanzione.Visible = True
            divddlSegnalazioneSanzione.Visible = True
        Else
            divlblSegnalazioneSanzione.Visible = True
            divddlSegnalazioneSanzione.Visible = True
        End If

        lblmess.Visible = False

    End Sub

    Private Sub CaricaComboSettore()
        'AUTORE: MIchele d'Ascenzio
        'DATA: 26/11/2004
        'Carico combo settore
        Dim MyDataset As DataSet
        Dim strSql As String

        cboSettore.Items.Clear()
        strSql = "SELECT '0' as IdMacroAmbitoAttività, '' as Settore FROM MacroAmbitiAttività " & _
                 "UNION SELECT IdMacroAmbitoAttività, Codifica + ' - ' + MacroAmbitoAttività as Settore FROM MacroAmbitiAttività"
        MyDataset = ClsServer.DataSetGenerico(strSql, Session("conn"))
        cboSettore.DataSource = MyDataset
        cboSettore.DataValueField = "IdMacroAmbitoAttività"
        cboSettore.DataTextField = "Settore"
        cboSettore.DataBind()

    End Sub

    Private Sub CaricaGriglia()
        Dim strSql As String
        Dim MyDataSet As DataSet
        'DESCRIZIONE: routine che carica la griglia con tutti i progetti con stato attività=1
        'AUTORE: Michele d'Ascenzio    
        'DATA: 03/11/2004

        'Try
        strSql = "SELECT " & _
                 "'<img src=images/check.png STYLE=cursor:hand title=Seleziona alt=Seleziona border=0 onClick=Javascript:SelezionaProgetto(' + CONVERT(varchar, AttivitàEntiSediAttuazione.IdAttivitàEnteSedeAttuazione) + ',' + CONVERT(varchar, Attività.IdAttività) + ') />' as Img, " & _
                 "Enti.CodiceRegione as CodEnte, " & _
                 "Attività.Titolo as TitoloProgetto, " & _
                 "MacroAmbitiAttività.Codifica + ' / ' + AmbitiAttività.Codifica as SettoreAmbito, " & _
                 "Attività.DataInizioAttività as DataInizio, " & _
                 "EntiSedi.Denominazione as SedeFisica, " & _
                 "EntiSediAttuazioni.Denominazione as SedeAttuazione, " & _
                 "EntiSediAttuazioni.IdEnteSedeAttuazione as Codice, " & _
                 "Comuni.Denominazione as Comune,Attività.IdAttività, " & _
                 "(SELECT NumeroPostiNoVittoNoAlloggio+NumeroPostiVittoAlloggio+NumeroPostiVitto from AttivitàEntiSediAttuazione s3 WHERE s3.IdAttivitàEnteSedeAttuazione = AttivitàEntiSediAttuazione.IdAttivitàEnteSedeAttuazione) - (SELECT COUNT(distinct Entità.IdEntità) FROM AttivitàEntità INNER JOIN AttivitàEntiSediAttuazione s2 ON AttivitàEntità.IdAttivitàEnteSedeAttuazione = s2.IdAttivitàEnteSedeAttuazione INNER JOIN StatiAttivitàEntità ON StatiAttivitàEntità.IdStatoAttivitàEntità = AttivitàEntità.IdStatoAttivitàEntità INNER JOIN Entità ON Entità.IdEntità = AttivitàEntità.IdEntità INNER JOIN StatiEntità ON StatiEntità.IdStatoEntità = Entità.IdStatoEntità WHERE(s2.IdAttivitàEnteSedeAttuazione = AttivitàEntiSediAttuazione.IdAttivitàEnteSedeAttuazione) AND StatiAttivitàEntità.DefaultStato=1 AND StatiEntità.InServizio=1) as NumPostiAssegnati, " & _
                 " Case isnull(entisediattuazioni.Segnalazione,0) When 0 then 'No' When 1 then '<img src=images/Anomalie.bmp onclick=VisualizzaSanzione('+ convert(varchar, entisediattuazioni.IDEnteSedeAttuazione) + ','+ convert(varchar, entisedi.IDEnte) + ') STYLE=cursor:hand title=Sanzione border=0>' End as Segnalazione, " & _
                 " Case isnull(entisediattuazioni.SedeSottopostaVerifica,0) When 0 then 'No' When '1' then 'Si' end as verifica, " & _
                 " CONVERT(varchar, AttivitàEntiSediAttuazione.IdAttivitàEnteSedeAttuazione) + '|' + CONVERT(varchar, Attività.IdAttività) as DataFields " & _
                 "FROM Attività " & _
                 "INNER JOIN Enti ON Enti.IdEnte = Attività.IdEntePresentante " & _
                 "INNER JOIN AmbitiAttività ON Attività.IDAmbitoAttività = AmbitiAttività.IDAmbitoAttività " & _
                 "INNER JOIN MacroAmbitiAttività ON AmbitiAttività.IDMacroAmbitoAttività = MacroAmbitiAttività.IDMacroAmbitoAttività " & _
                 "INNER JOIN AttivitàEntiSediAttuazione ON AttivitàEntiSediAttuazione.IdAttività = Attività.IdAttività " & _
                 "INNER JOIN EntiSediAttuazioni ON EntiSediAttuazioni.IdEnteSedeAttuazione = AttivitàEntiSediAttuazione.IdEnteSedeAttuazione " & _
                 "INNER JOIN EntiSedi ON EntiSedi.IdEntesede = EntiSediAttuazioni.IdEntesede " & _
                 "INNER JOIN Comuni ON Comuni.IdComune = EntiSedi.IdComune " & _
                 "INNER JOIN Provincie ON Provincie.IdProvincia = Comuni.IdProvincia " & _
                 "INNER JOIN Regioni ON Regioni.IdRegione = Provincie.IdRegione " & _
                 "INNER JOIN StatiAttività ON StatiAttività.IdStatoAttività = Attività.IdStatoAttività " & _
                 "INNER JOIN BandiAttività ON BandiAttività.IdBandoAttività = Attività.IdBandoattività " & _
                 "INNER JOIN Bando ON Bando.IdBando = BandiAttività.IdBando " & _
                 "INNER JOIN TipiProgetto ON Attività.IdTipoProgetto = TipiProgetto.IdTipoProgetto " & _
                 "WHERE " & _
                 "StatiAttività.Attiva = 1 And isnull(attività.datainizioattività,'') <>'' And 	(SELECT NumeroPostiNoVittoNoAlloggio+NumeroPostiVittoAlloggio+NumeroPostiVitto from AttivitàEntiSediAttuazione s3  WHERE s3.IdAttivitàEnteSedeAttuazione = AttivitàEntiSediAttuazione.IdAttivitàEnteSedeAttuazione) - (SELECT COUNT(distinct Entità.IdEntità) FROM AttivitàEntità INNER JOIN AttivitàEntiSediAttuazione s2 ON AttivitàEntità.IdAttivitàEnteSedeAttuazione = s2.IdAttivitàEnteSedeAttuazione INNER JOIN StatiAttivitàEntità ON StatiAttivitàEntità.IdStatoAttivitàEntità = AttivitàEntità.IdStatoAttivitàEntità INNER JOIN Entità ON Entità.IdEntità = AttivitàEntità.IdEntità INNER JOIN StatiEntità ON StatiEntità.IdStatoEntità = Entità.IdStatoEntità  WHERE(s2.IdAttivitàEnteSedeAttuazione = AttivitàEntiSediAttuazione.IdAttivitàEnteSedeAttuazione) AND StatiAttivitàEntità.DefaultStato=1 AND StatiEntità.InServizio=1) > 0 "

        If txtEnte.Text <> vbNullString Then
            strSql = strSql & "AND Enti.Denominazione like '" & Replace(txtEnte.Text, "'", "''") & "%' "
        End If
        If txtCodEnte.Text <> vbNullString Then
            strSql = strSql & "AND Enti.CodiceRegione like '" & Replace(txtCodEnte.Text, "'", "''") & "%' "
        End If
        If txtProgetto.Text <> vbNullString Then
            strSql = strSql & "AND Attività.Titolo like '" & Replace(txtProgetto.Text, "'", "''") & "%' "
        End If
        If txtBando.Text <> vbNullString Then
            strSql = strSql & "AND Bando.Bando like '" & Replace(txtBando.Text, "'", "''") & "%' "
        End If
        If cboSettore.SelectedIndex >= 1 Then
            strSql = strSql & " AND  MacroAmbitiAttività.IdMacroAmbitoAttività = " & cboSettore.Items(cboSettore.SelectedIndex).Value
        End If
        If cboArea.SelectedIndex >= 1 Then
            strSql = strSql & " AND  AmbitiAttività.IdAmbitoAttività = " & cboArea.Items(cboArea.SelectedIndex).Value
        End If
        If txtRegione.Text <> vbNullString Then
            strSql = strSql & "AND Regioni.Regione like '" & Replace(txtRegione.Text, "'", "''") & "%' "
        End If
        If txtProvincia.Text <> vbNullString Then
            strSql = strSql & "AND Provincie.Provincia like '" & Replace(txtProvincia.Text, "'", "''") & "%' "
        End If
        If TxtCodiceSede.Text <> vbNullString Then
            strSql = strSql & "AND EntiSediAttuazioni.IdEnteSedeAttuazione= " & TxtCodiceSede.Text & " "
        End If
        'agg. da sc il 07/07/2011 combo su segnalazione della sede
        If ddlSegnalazioneSanzione.SelectedItem.Text <> "Tutti" Then
            strSql = strSql & " and isnull(entisediattuazioni.Segnalazione,0) =" & ddlSegnalazioneSanzione.SelectedValue & ""
        End If
        strSql = strSql & " and TipiProgetto.MacroTipoProgetto like '" & Session("FiltroVisibilita") & "' "
        strSql = strSql & " and tipiprogetto.idtipoprogetto = (select idtipoprogetto from attività where idattività = " & Request.QueryString("IdProgetto") & ")"
        strSql = strSql & "ORDER BY ABS(DATEDIFF(DAY, Attività.DataInizioAttività,(SELECT DataInizioAttività FROM Attività WHERE IdAttività = " & Request.QueryString("IdProgetto") & ")))"

        MyDataSet = ClsServer.DataSetGenerico(strSql, Session("conn"))

        'assegno il dataset alla griglia del risultato
        dgRisultatoRicerca.DataSource = MyDataSet
        Session("appDtsRisRicerca") = MyDataSet
        dgRisultatoRicerca.DataBind()
        dgRisultatoRicerca.Visible = True
        'AGG DA SIMONA CORDELLA IL 11/07/2011
        dgRisultatoRicerca.Columns(11).Visible = ClsUtility.ForzaPresenzaSanzione(Session("Utente"), Session("conn"))
        dgRisultatoRicerca.Columns(12).Visible = ClsUtility.ForzaSedeSottopostaVerifica(Session("Utente"), Session("conn"))

        If dgRisultatoRicerca.Items.Count = 0 Then
            lblmessaggio.Text = "Nessun Dato estratto."
        Else
            lblmessaggio.Text = "Risultato Ricerca Progetti Ricollocamento."
        End If
    End Sub

    Private Sub CaricaComboAttivita(ByVal idSett As Integer)
        'AUTORE: MIchele d'Ascenzio
        'DATA: 26/11/2004
        'Carico combo attivita
        Dim MyDataset As DataSet
        Dim Strsql As String
        Try

            cboArea.Enabled = True
            cboArea.Items.Clear()
            Strsql = "SELECT '0' as IDAmbitoAttività, '' as area from ambitiattività UNION SELECT IDAmbitoAttività, (Codifica + ' - ' + AmbitoAttività) AS Area  FROM ambitiattività WHERE IDMacroAmbitoAttività = " & idSett
            MyDataset = ClsServer.DataSetGenerico(Strsql, Session("conn"))
            cboArea.DataSource = MyDataset
            cboArea.DataTextField = "Area"
            cboArea.DataValueField = "IDAmbitoAttività"
            cboArea.DataBind()

            If idSett = 0 Then
                cboArea.Enabled = False
            End If

        Catch ex As Exception

        End Try
    End Sub

    Private Sub dgRisultatoRicerca_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgRisultatoRicerca.ItemCommand
        If e.CommandName = "Select" Then

            Dim pIdSedeAtt As String = dgRisultatoRicerca.DataKeys(e.Item.ItemIndex).ToString.Split("|")(0)
            Dim IdDelProgetto As String = dgRisultatoRicerca.DataKeys(e.Item.ItemIndex).ToString.Split("|")(1)

            'window.navigate('RicollocaVolontario.aspx?Op=<%=request.querystring("Op")%>&Pippo=1&IdAttivita=<%=request.querystring("IdAttivita")%>&IdProgetto=' + IdDelProgetto + '&IdAttivitaEnteSedeAttuazione=' + pIdSedeAtt + '&IdVolontario=<%=Request.QueryString("IdVolontario")%>&VecchioIdAttivitaEntita=<%=Request.QueryString("VecchioIdAttivitaEntita")%>')
            Response.Redirect("RicollocaVolontario.aspx?Op=" + Request.QueryString("Op") + "&Pippo=1&IdAttivita=" + Request.QueryString("IdAttivita") + "&IdProgetto=" + IdDelProgetto + "&IdAttivitaEnteSedeAttuazione=" + pIdSedeAtt + "&IdVolontario=" + Request.QueryString("IdVolontario") + "&VecchioIdAttivitaEntita=" + Request.QueryString("VecchioIdAttivitaEntita"))
        End If
    End Sub

    Private Sub dgRisultatoRicerca_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgRisultatoRicerca.PageIndexChanged
        dgRisultatoRicerca.CurrentPageIndex = e.NewPageIndex
        dgRisultatoRicerca.DataSource = Session("appDtsRisRicerca")
        dgRisultatoRicerca.DataBind()
        dgRisultatoRicerca.SelectedIndex = -1
    End Sub

    Private Sub cboSettore_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboSettore.SelectedIndexChanged
        CaricaComboAttivita(cboSettore.SelectedValue)
    End Sub

    Private Sub cmdChiudi_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdChiudi.Click
        Response.Redirect("RicollocaVolontario.aspx?Op=" & Request.QueryString("Op") & "&IdProgetto=" & Request.QueryString("IdProgetto") & "&IdVolontario=" & Request.QueryString("IdVolontario"))
    End Sub

    Private Sub cmdRicerca_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdRicerca.Click

        If controlliRicercaServer() = True Then
            CaricaGriglia()
        End If

    End Sub

    Function controlliRicercaServer() As Boolean

        Dim codiceSede As Integer
        Dim codiceSedeInteger As Boolean

        If TxtCodiceSede.Text.Trim <> String.Empty Then

            codiceSedeInteger = Integer.TryParse(TxtCodiceSede.Text.Trim, codiceSede)

            If codiceSedeInteger = False Then
                lblmess.Visible = True
                lblmess.Text = "Il Codice Sede deve essere un numero."
                Return False
            End If
        End If
       

        Return True

    End Function

    
End Class