Imports System.IO

Public Class WfrmRicercaSediCertificate
    Inherits System.Web.UI.Page
    Dim dtsgenerico As New DataSet
    Public blnForza As Boolean

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not Session("LogIn") Is Nothing Then
            If Session("LogIn") = False Then 'verifico validità log-in
                Response.Redirect("LogOn.aspx")
            End If
        Else
            Response.Redirect("LogOn.aspx")
        End If

        '***  agg. da simona cordella il 07/07/2011
        'se l'utene NON è abilitato al menu FORZA PRESENZA SANZIONE 
        'nn visualizzo il filtro di ricerca (aspx) Presenza Sanzione
        blnForza = ClsUtility.ForzaPresenzaSanzione(Session("Utente"), Session("conn"))
        '***

        If Page.IsPostBack = False Then

            If Request.QueryString("PopSede") = "1" Then
                phCodiceEnte.Visible = False
                phDenominazioneEnte.Visible = False
            Else
                phCodiceEnte.Visible = True
                phDenominazioneEnte.Visible = True
            End If

            If blnForza = "true" Then
                phPresenzaSanzione.Visible = True
            Else
                phPresenzaSanzione.Visible = False
            End If

            txtCodiceEnte.Text = Session("txtCodEnte")

            'If Not Server.HtmlDecode(Request.Cookies("InfoRif")("txtCodiceEnte")) Is Nothing Then
            '    If Session("txtCodEnte") = Request.Cookies("InfoRif")("txtCodiceEnte") Then
            '        RecuperaCookie()
            '    End If
            'End If
            
            If Not Request.Cookies("InfoRif") Is Nothing Then
                If Session("txtCodEnte") = Request.Cookies("InfoRif")("txtCodiceEnte") Then
                    RecuperaCookie()
                End If
            End If



            CaricaCompetenze()
            'COMBO CERTIFICAZIONE

            CaricaCertificazione()
            CaricaStatoSede()
        End If

    End Sub

    Sub CaricaCompetenze()
        'stringa per la query
        Dim strSQL As String
        'datareader che conterrà l'id 
        Dim dtrCompetenze As System.Data.SqlClient.SqlDataReader

        Try
            'controllo se si tratta del primo caricamento. così leggo i dati nel db una sola volta
            If Page.IsPostBack = False Then
                'preparo la query

                strSQL = "select IdRegioneCompetenza,Descrizione,CodiceRegioneCompetenza,left(CodiceRegioneCompetenza,1)from RegioniCompetenze where IdRegioneCompetenza <> 22 "
                strSQL = strSQL & " union "
                strSQL = strSQL & " select '0',' TUTTI ','','A' "
                strSQL = strSQL & " union "
                strSQL = strSQL & " select '-1',' NAZIONALE ','','B' "
                strSQL = strSQL & " union "
                strSQL = strSQL & " select '-2',' REGIONALE ','','C' "
                strSQL = strSQL & " union "
                strSQL = strSQL & " select '-3',' NON DEFINITO ','','D' "
                strSQL = strSQL & "  from RegioniCompetenze order by left(CodiceRegioneCompetenza,1),descrizione "
                'chiudo il datareader se aperto
                If Not dtrCompetenze Is Nothing Then
                    dtrCompetenze.Close()
                    dtrCompetenze = Nothing
                End If

                'eseguo la query
                dtrCompetenze = ClsServer.CreaDatareader(strSQL, Session("conn"))
                'assegno il datadearder alla combo caricando così descrizione e id
                CboCompetenza.DataSource = dtrCompetenze
                CboCompetenza.Items.Add("")
                CboCompetenza.DataTextField = "Descrizione"
                CboCompetenza.DataValueField = "IDRegioneCompetenza"
                CboCompetenza.DataBind()
                'chiudo il datareader se aperto
                If Not dtrCompetenze Is Nothing Then
                    dtrCompetenze.Close()
                    dtrCompetenze = Nothing
                End If
            End If
            'Controllo abilitazione scelta
            If Session("TipoUtente") = "U" Then
                CboCompetenza.Enabled = True
                CboCompetenza.SelectedIndex = 0
            Else
                'CboCompetenza.SelectedIndex = 1
                CboCompetenza.Enabled = False
                'preparo la query
                strSQL = "select b.IdRegioneCompetenza ,b.Heliosread from RegioniCompetenze a "
                strSQL = strSQL & "INNER JOIN utentiunsc b ON a.idregionecompetenza = b.idregionecompetenza "
                strSQL = strSQL & "where b.username = '" & Session("Utente") & "'"
                'chiudo il datareader se aperto
                If Not dtrCompetenze Is Nothing Then
                    dtrCompetenze.Close()
                    dtrCompetenze = Nothing
                End If
                'controllo se utente o ente regionale
                'eseguo la query
                dtrCompetenze = ClsServer.CreaDatareader(strSQL, Session("conn"))
                dtrCompetenze.Read()
                If dtrCompetenze.HasRows = True Then
                    CboCompetenza.SelectedValue = dtrCompetenze("IdRegioneCompetenza")
                    If dtrCompetenze("Heliosread") = True Then
                        CboCompetenza.Enabled = True
                    End If

                End If

            End If
        Catch ex As Exception
            Response.Write(ex.Message.ToString())
            If Not dtrCompetenze Is Nothing Then
                dtrCompetenze.Close()
                dtrCompetenze = Nothing
            End If
        End Try
        If Not dtrCompetenze Is Nothing Then
            dtrCompetenze.Close()
            dtrCompetenze = Nothing
        End If
    End Sub

    Sub CaricaCertificazione()
        'SE SONO UN ENTE VEDO SOLO CERTIFICATO SI E NO
        ' SE SONO UNSC O REGIONE VEDO TUTTE LE TIPOLOGIE DELLA CERTIFICAZIONE
        ddlCertificazione.Items.Add("Tutti")
        ddlCertificazione.Items(0).Value = ""

        ddlCertificazione.Items.Add("No")
        ddlCertificazione.Items(1).Value = 0

        ddlCertificazione.Items.Add("Si")
        ddlCertificazione.Items(2).Value = 1
        'mod. il 26.03.2009 da sc 
        'carico nella combo della certificazione anche il DA VALUTARE anche nel caso che la mashera viene aperta da progetti in fase di valutazione o accettazione
        'If Request.QueryString("PopSede") = Nothing Then
        ddlCertificazione.Items.Add("Da Valutare")
        ddlCertificazione.Items(3).Value = 2
        'End If
    End Sub

    Sub CaricaStatoSede()
        Dim strSQL As String
        Dim dtrSede As SqlClient.SqlDataReader
        'Agg da s.c. il 11/12/2008
        'Carico Combo stato sedi solo x lo stato ACCREDITATE E PRESENTATE
        If Not dtrSede Is Nothing Then
            dtrSede.Close()
            dtrSede = Nothing
        End If

        strSQL = strSQL & " Select '' as IdStatoEnteSede ,'' as StatoEnteSede "
        strSQL = strSQL & " Union "
        strSQL = strSQL & " Select IdStatoEnteSede, replace(StatoEnteSede, 'Accreditata', 'Iscritta') StatoEnteSede "
        strSQL = strSQL & " From StatiEntiSedi "
        strSQL = strSQL & " Where IdStatoEnteSede in(1,4) "
        strSQL = strSQL & " Order by IdStatoEnteSede "

        'eseguo la query
        dtrSede = ClsServer.CreaDatareader(strSQL, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))

        'assegno il datadearder alla combo caricando così descrizione e id
        ddlstato.DataSource = dtrSede
        ddlstato.Items.Add("")
        ddlstato.DataTextField = "StatoEnteSede"
        ddlstato.DataValueField = "IdStatoEnteSede"
        ddlstato.DataBind()
        If Not dtrSede Is Nothing Then
            dtrSede.Close()
            dtrSede = Nothing
        End If
    End Sub

    Private Sub dgRisultatoRicerca_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgRisultatoRicerca.ItemCommand

        Select Case e.CommandName

            Case "VisualizzaSanzione"

                Dim idEnteSedeAttuazione As String
                Dim idEnte As String

                idEnteSedeAttuazione = ViewState("IDEnteSedeAttuazione")
                idEnte = ViewState("IDEnte")

                Response.Write("<script>" & vbCrLf)
                Response.Write("window.open(""WfrmSedeSanzionata.aspx?IDEnteSedeAttuazione=" & idEnteSedeAttuazione & "&IdEnte=" & idEnte & """, ""SedeSanzionata"", ""width=950, height=600, toolbar=no, location=no, menubar=no, scrollbars=yes"")" & vbCrLf)
                Response.Write("</script>")

        End Select

    End Sub

    Private Sub dgRisultatoRicerca_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgRisultatoRicerca.ItemDataBound

        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then

            Dim lblPresenzaSanzione As Label = DirectCast(e.Item.FindControl("lblPresenzaSanzione"), Label)
            Dim ImgVisualizzaSanzione As ImageButton = DirectCast(e.Item.FindControl("ImgVisualizzaSanzione"), ImageButton)

            Dim segnalazione As String = IIf(IsDBNull(e.Item.DataItem("Segnalazione")), String.Empty, e.Item.DataItem("Segnalazione"))

            If segnalazione.ToUpper = "FALSE" Then

                lblPresenzaSanzione.Visible = True
                lblPresenzaSanzione.Text = "No"
                ImgVisualizzaSanzione.Visible = False

            Else
                ViewState("IDEnteSedeAttuazione") = IIf(IsDBNull(e.Item.DataItem("IDEnteSedeAttuazione")), String.Empty, e.Item.DataItem("IDEnteSedeAttuazione"))
                ViewState("IDEnte") = IIf(IsDBNull(e.Item.DataItem("IDEnte")), String.Empty, e.Item.DataItem("IDEnte"))
                lblPresenzaSanzione.Visible = False
                ImgVisualizzaSanzione.Visible = True
            End If

        End If
    End Sub

    Private Sub dgRisultatoRicerca_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgRisultatoRicerca.PageIndexChanged
        Dim i As Integer
        Dim Mychk As CheckBox
        Dim dtGriglia As DataTable 'appoggio
        Dim rwGriglia As DataRow
        Dim clGriglia As DataColumn
        Dim ValoreRichkDt As Integer ' valore che ricorda il campo riccheck della query (si per la ricerca sedi di progetto che per le ricerca sedi)
        dtGriglia = Session("SessionDtSedi")
        If Request.QueryString("PopSede") = 1 Then
            ValoreRichkDt = 22 ' id ricerca sede di progetto (riccheck)
        Else
            ValoreRichkDt = 12 ' id ricerca sede campo (riccheck)
        End If
        clGriglia = dtGriglia.Columns(ValoreRichkDt)
        clGriglia.ReadOnly = False

        'If chkSelDesel.Checked = True Then
        '    CeccaTutti()
        'End If
        For i = 0 To dgRisultatoRicerca.Items.Count - 1
            Mychk = dgRisultatoRicerca.Items.Item(i).FindControl("chkSedi")
            rwGriglia = dtGriglia.Rows(i + (dgRisultatoRicerca.CurrentPageIndex * 10))
            If Mychk.Checked = True Then
                rwGriglia.Item(ValoreRichkDt) = "1"
            Else
                rwGriglia.Item(ValoreRichkDt) = "0"
            End If
        Next i
        Session("SessionDtSedi") = dtGriglia
        dgRisultatoRicerca.CurrentPageIndex = e.NewPageIndex
        dgRisultatoRicerca.DataSource = Session("SessionDtSedi")
        dgRisultatoRicerca.DataBind()


        '---determino cosa c'era text nella datatable di sessione in e lo visualizzo nella pag che vado a caricare
        For i = 0 To dgRisultatoRicerca.Items.Count - 1
            Mychk = dgRisultatoRicerca.Items.Item(i).FindControl("chkSedi")
            rwGriglia = dtGriglia.Rows(i + (dgRisultatoRicerca.CurrentPageIndex * 10))
            If rwGriglia.Item(ValoreRichkDt) = "1" Then
                Mychk.Checked = True
            Else
                Mychk.Checked = False
            End If
        Next i
        If chkSelDesel.Checked = True Then
            CeccaTutti()
            For i = 0 To dgRisultatoRicerca.Items.Count - 1
                Mychk = dgRisultatoRicerca.Items.Item(i).FindControl("chkSedi")
                rwGriglia = dtGriglia.Rows(i + (dgRisultatoRicerca.CurrentPageIndex * 10))
                If Mychk.Checked = True Then
                    rwGriglia.Item(ValoreRichkDt) = "1"
                Else
                    rwGriglia.Item(ValoreRichkDt) = "0"
                End If
            Next i
            Session("SessionDtSedi") = dtGriglia
            dgRisultatoRicerca.CurrentPageIndex = e.NewPageIndex
            dgRisultatoRicerca.DataSource = Session("SessionDtSedi")
            dgRisultatoRicerca.DataBind()

            '---determino cosa c'era text nella datatable di sessione in e lo visualizzo nella pag che vado a caricare
            For i = 0 To dgRisultatoRicerca.Items.Count - 1
                Mychk = dgRisultatoRicerca.Items.Item(i).FindControl("chkSedi")
                rwGriglia = dtGriglia.Rows(i + (dgRisultatoRicerca.CurrentPageIndex * 10))
                If rwGriglia.Item(ValoreRichkDt) = "1" Then
                    Mychk.Checked = True
                Else
                    Mychk.Checked = False
                End If
            Next i
        Else
            If hdd_Check.Value = "SI" Then
                EliminaTuttiCheck()
                For i = 0 To dgRisultatoRicerca.Items.Count - 1
                    Mychk = dgRisultatoRicerca.Items.Item(i).FindControl("chkSedi")
                    rwGriglia = dtGriglia.Rows(i + (dgRisultatoRicerca.CurrentPageIndex * 10))
                    If Mychk.Checked = True Then
                        rwGriglia.Item(ValoreRichkDt) = "1"
                    Else
                        rwGriglia.Item(ValoreRichkDt) = "0"
                    End If
                Next i
                Session("SessionDtSedi") = dtGriglia
                dgRisultatoRicerca.CurrentPageIndex = e.NewPageIndex
                dgRisultatoRicerca.DataSource = Session("SessionDtSedi")
                dgRisultatoRicerca.DataBind()

                '---determino cosa c'era text nella datatable di sessione in e lo visualizzo nella pag che vado a caricare
                For i = 0 To dgRisultatoRicerca.Items.Count - 1
                    Mychk = dgRisultatoRicerca.Items.Item(i).FindControl("chkSedi")
                    rwGriglia = dtGriglia.Rows(i + (dgRisultatoRicerca.CurrentPageIndex * 10))
                    If rwGriglia.Item(ValoreRichkDt) = "1" Then
                        Mychk.Checked = True
                    Else
                        Mychk.Checked = False
                    End If
                Next i
            End If
        End If
        TogliCheck()

        ColoraCelle()
    End Sub

    Sub CeccaTutti()
        Dim item As DataGridItem
        Dim chkValore As CheckBox
        'ricordo il valore ceccato nella pagina corrente
        For Each item In dgRisultatoRicerca.Items
            chkValore = DirectCast(item.FindControl("chkSedi"), CheckBox)
            chkValore.Checked = True
        Next
    End Sub

    Sub EliminaTuttiCheck()
        Dim item As DataGridItem
        Dim chkValore As CheckBox
        'ricordo il valore ceccato nella pagina corrente
        For Each item In dgRisultatoRicerca.Items
            chkValore = DirectCast(item.FindControl("chkSedi"), CheckBox)
            chkValore.Checked = False
        Next
    End Sub

    Sub TogliCheck()
        Dim item As DataGridItem
        Dim chkValore As CheckBox
        'ricordo il valore ceccato nella pagina corrente
        For Each item In dgRisultatoRicerca.Items
            hdd_Check.Value = "NO"
            chkValore = DirectCast(item.FindControl("chkSedi"), CheckBox)
            chkValore.Attributes.Add("onclick", "TogliCheck()")
        Next
    End Sub

    Private Sub ColoraCelle()
        'VAriazione del Colore secondo lo stato della sede.
        'Attiva=Verde;Presentata=gialla;Cancellata=Rossa;Sospesa=
        Dim item As DataGridItem
        Dim color As New System.Drawing.Color
        Dim x As Integer
        Dim txt As String
        For Each item In dgRisultatoRicerca.Items
            'For x = 0 To 17
            '    If dgRisultatoRicerca.Items(item.ItemIndex).Cells(18).Text = "False" Then
            '        color = color.Khaki
            '    Else
            '        color = color.LightGreen
            '    End If
            '    dgRisultatoRicerca.Items(item.ItemIndex).Cells(x).BackColor = color
            'Next

            Select Case dgRisultatoRicerca.Items(item.ItemIndex).Cells(7).Text
                Case "Iscritta"
                    'If InStr(dgRisultatoRicerca.Items(item.ItemIndex).Cells(1).Text, "(*)") > 0 Then
                    '    dgRisultatoRicerca.Items(item.ItemIndex).Cells(1).ForeColor = color.Red
                    'End If
                    For x = 0 To dgRisultatoRicerca.Columns.Count - 1
                        dgRisultatoRicerca.Items(item.ItemIndex).Cells(x).BackColor = color.LightGreen
                        'dgRisultatoRicerca.Items(item.ItemIndex).Cells(intConta).ForeColor = Color.LightGreen
                    Next
                    'se la sede è da valutare coloro il FONT DI VIOLA
                    Select Case dgRisultatoRicerca.Items(item.ItemIndex).Cells(17).Text
                        Case "Da Valutare"
                            For x = 0 To dgRisultatoRicerca.Columns.Count - 1
                                dgRisultatoRicerca.Items(item.ItemIndex).Cells(x).ForeColor = color.FromArgb(201, 96, 185)
                                dgRisultatoRicerca.Items(item.ItemIndex).Cells(x).Font.Bold = True
                            Next
                        Case "No"
                            For x = 0 To dgRisultatoRicerca.Columns.Count - 1
                                dgRisultatoRicerca.Items(item.ItemIndex).Cells(x).ForeColor = color.Red
                                'dgRisultatoRicerca.Items(item.ItemIndex).Cells(x).Font.Bold = True
                            Next
                    End Select

                Case "Presentata"
                    For x = 0 To dgRisultatoRicerca.Columns.Count - 1
                        dgRisultatoRicerca.Items(item.ItemIndex).Cells(x).BackColor = color.Khaki
                        'dgRisultatoRicerca.Items(item.ItemIndex).Cells(intConta).ForeColor = Color.Khaki
                    Next
                    'se la sede è da valutare coloro il FONT DI VIOLA
                    Select Case dgRisultatoRicerca.Items(item.ItemIndex).Cells(17).Text
                        Case "Da Valutare"
                            For x = 0 To dgRisultatoRicerca.Columns.Count - 1
                                dgRisultatoRicerca.Items(item.ItemIndex).Cells(x).ForeColor = color.FromArgb(201, 96, 185)
                                dgRisultatoRicerca.Items(item.ItemIndex).Cells(x).Font.Bold = True
                            Next
                        Case "No"
                            For x = 0 To dgRisultatoRicerca.Columns.Count - 1
                                dgRisultatoRicerca.Items(item.ItemIndex).Cells(x).ForeColor = color.Red
                                'dgRisultatoRicerca.Items(item.ItemIndex).Cells(x).Font.Bold = True
                            Next
                    End Select

                Case "Sospesa"
                    For x = 0 To dgRisultatoRicerca.Columns.Count - 1
                        dgRisultatoRicerca.Items(item.ItemIndex).Cells(x).BackColor = color.Gainsboro
                        'dgRisultatoRicerca.Items(item.ItemIndex).Cells(intConta).ForeColor = Color.Red
                    Next
                Case "Cancellata"
                    For x = 0 To dgRisultatoRicerca.Columns.Count - 1
                        dgRisultatoRicerca.Items(item.ItemIndex).Cells(x).BackColor = color.LightSalmon
                        'dgRisultatoRicerca.Items(item.ItemIndex).Cells(intConta).ForeColor = Color.Gainsboro
                    Next
            End Select
        Next
    End Sub

    Private Sub cmdRicerca_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdRicerca.Click
        ApriCSV1.Visible = False
        lblmessaggio.Text = ""
        dgRisultatoRicerca.CurrentPageIndex = 0
        ' modificato da simona cordella il 27/11/2008
        ' personalizzo la ricerca, se richamo la maschera dalla valutazione dei progetti,
        ' ricerco sono le sedi del progetto
        If Request.QueryString("PopSede") = 1 Then
            dgRisultatoRicerca.Columns(1).Visible = False 'ente
            dgRisultatoRicerca.Columns(11).Visible = False 'competenza
            RicercaSediProgetto()
            'agg da sc il 14/05/2009 gestione della maschera in lettura
            If Request.QueryString("Modifica") = 0 Then
                cmdSalva.Visible = False
                dgRisultatoRicerca.Columns(13).Visible = False 'check per la certificazione
                chkSelDesel.Visible = False
            End If
        Else ' ricerco tutte le sedi da certificare
            dgRisultatoRicerca.Columns(1).Visible = True 'ente
            dgRisultatoRicerca.Columns(11).Visible = True 'competenza
            If txtCodiceEnte.Text = "" Then
                lblmessaggio.Text = "Inserire il codice ente."
            Else
                RicercaSedi()
            End If
        End If

        AssegnaCookies()

    End Sub
    Sub AssegnaCookies()
        'CREATA IL 28/12/2016 ADC
        Response.Cookies("InfoRif")("txtCodiceEnte") = txtCodiceEnte.Text
        Response.Cookies("InfoRif")("txtIDFaseEnte") = txtIDFaseEnte.Text
        Response.Cookies("InfoRif").Expires = DateTime.Now.AddDays(1)
    End Sub
    Sub RecuperaCookie()
        'CREATA IL 28/12/2016 ADC
        If Not Server.HtmlDecode(Request.Cookies("InfoRif")("txtCodiceEnte")) Is Nothing Then
            txtCodiceEnte.Text = Server.HtmlDecode(Request.Cookies("InfoRif")("txtCodiceEnte")).ToString

        End If
        If Not Server.HtmlDecode(Request.Cookies("InfoRif")("txtIDFaseEnte")) Is Nothing Then
            txtIDFaseEnte.Text = Server.HtmlDecode(Request.Cookies("InfoRif")("txtIDFaseEnte")).ToString
        End If

    End Sub

    Sub RicercaSediProgetto()
        Dim strsql As String
        Dim dtSedi As DataTable = New DataTable
        ''  '' as idente, 
        chkSelDesel.Checked = False
        '" '' as ente,'' as Competenza,'' as idente, " & _
        If Request.QueryString("EnteCapoFila") = "True" Then
            strsql = " Select '' as Ente," &
                     " SedeFisica, Indirizzo, Comune,telefono,SedeAttuazione, replace(statoentesede, 'Accreditata', 'Iscritta') StatoEnteSede , IDEnteSede,  IDEnteSedeAttuazione,  " &
                     " '' as idatt,'' as Competenza, isnull(Certificazione,2) as Certificazione,   " &
                     " case isnull(certificazione,2)when 0 then 0 when 1 then 1 when 2 then 0 end as riccheck, " &
                     " case isnull(certificazione,2)when 0 then 'No' when 1 then 'Si' when 2 then'Da Valutare' end as codificaCert, " &
                     " case isnull(certificazione,2)when 1 then '<input onclick=javascript:ControllaSedeAttuazione(''' + convert(varchar,idEntesedeattuazione) + ''') type=checkbox value=1 name=chkCert' + convert(varchar,idEntesedeattuazione) + ' id=chkCert' + convert(varchar,idEntesedeattuazione) + ' checked >' when 1 then '<input onclick=javascript:ControllaSedeAttuazione(''' + convert(varchar,idEntesedeattuazione) + ''') type=checkbox value=0 name=chkCert' + convert(varchar,idEntesedeattuazione) + ' id=chkCert' + convert(varchar,idEntesedeattuazione) + '>' End As CheckCert," &
                     " IDEnte, " &
                     " isnull(Segnalazione,0) as Segnalazione,  " &
                     " Case isnull(SedeSottopostaVerifica,0) When 0 then 'No' When '1' then 'Si' end as verifica " &
                     " from VW_ELENCO_SEDI_PROGETTO  " &
                     " WHERE idattività=" & CInt(Request.QueryString("IdAttivita")) & " "
        Else
            strsql = " Select '' as Ente," &
                     " SedeFisica, Indirizzo, Comune,telefono,SedeAttuazione, replace(statoentesede, 'Accreditata', 'Iscritta') StatoEnteSede , IDEnteSede,  IDEnteSedeAttuazione,  " &
                     " '' as idatt,'' as Competenza, isnull(Certificazione,2) as Certificazione,   " &
                     " case isnull(certificazione,2)when 0 then 0 when 1 then 1 when 2 then 0 end as riccheck, " &
                     " case isnull(certificazione,2)when 0 then 'No' when 1 then 'Si' when 2 then'Da Valutare' end as codificaCert, " &
                     " case isnull(certificazione,2)when 1 then '<input onclick=javascript:ControllaSedeAttuazione(''' + convert(varchar,idEntesedeattuazione) + ''') type=checkbox value=1 name=chkCert' + convert(varchar,idEntesedeattuazione) + ' id=chkCert' + convert(varchar,idEntesedeattuazione) + ' checked >' when 1 then '<input onclick=javascript:ControllaSedeAttuazione(''' + convert(varchar,idEntesedeattuazione) + ''') type=checkbox value=0 name=chkCert' + convert(varchar,idEntesedeattuazione) + ' id=chkCert' + convert(varchar,idEntesedeattuazione) + '>' End As CheckCert," &
                     " IDEnte, " &
                     " isnull(Segnalazione,0) as Segnalazione,  " &
                     " Case isnull(SedeSottopostaVerifica,0) When 0 then 'No' When '1' then 'Si' end as verifica " &
                     " from VW_ELENCO_SEDI_PROGETTO_ENTI_COPROGETTANTI " &
                     " WHERE identecoprogettante=" & Session("IdEnte") & " and idattività=" & CInt(Request.QueryString("IdAttivita")) & " "
        End If
        'If chkSediProgetto.Checked = True Then
        strsql = strsql & " and idatt <>0"

        'End If
        If Session("TipoUtente") = "E" Then
            strsql = strsql & " and attiva=1"
        End If
        If Trim(txtdenominazione.Text) <> "" Then
            strsql = strsql & " and sedeattuazione like '" & Replace(txtdenominazione.Text, "'", "''") & "%'"
        End If
        If Trim(txtregione.Text) <> vbNullString Then
            strsql = strsql & " and Regione LIKE '" & Replace(txtregione.Text, "'", "''") & "%'"
        End If
        If Trim(txtProvincia.Text) <> vbNullString Then
            strsql = strsql & " and Provincia LIKE '" & Replace(txtProvincia.Text, "'", "''") & "%'"
        End If
        If Trim(txtComune.Text) <> vbNullString Then
            strsql = strsql & " and Comune LIKE '" & Replace(txtComune.Text, "'", "''") & "%'"
        End If
        If Trim(txtCodSedeAtt.Text) <> vbNullString Then
            strsql = strsql & " and identesedeattuazione ='" & txtCodSedeAtt.Text & "'"
        End If
        If ddlCertificazione.SelectedItem.Text <> "Tutti" Then
            strsql = strsql & " and certificazione =" & ddlCertificazione.SelectedValue
        End If
        'agg. da sc il 07/07/2011 combo su segnalazione della sede
        If ddlSegnalazioneSanzione.SelectedItem.Text <> "Tutti" Then
            strsql = strsql & " and isnull(Segnalazione,0) =" & ddlSegnalazioneSanzione.SelectedValue
        End If

        strsql = strsql & " order by isnull(certificazione,2)desc,identesedeattuazione "
        dtsgenerico = ClsServer.DataSetGenerico(strsql, Session("conn"))

        CaricaDataGrid(dgRisultatoRicerca)
        dtSedi = ClsServer.CreaDataTable(strsql, False, Session("conn"))
        'assegno il dataset alla griglia del risultato
        dgRisultatoRicerca.DataSource = dtSedi
        dgRisultatoRicerca.DataBind()
        Session("SessionDtSedi") = dtSedi
        TogliCheck()
        'ControllaSedeAttuazione()
        ColoraCelle()
    End Sub

    Private Sub RicercaSedi()
        'adc
        Dim UtenteRegioneCompetenza As String
        Dim strSQL As String
        'datareader che conterrà l'la descrizione della regione
        Dim dtrCompetenze As System.Data.SqlClient.SqlDataReader
        Dim dtSedi As DataTable = New DataTable

        chkSelDesel.Checked = False

        strSQL = "select a.Descrizione from RegioniCompetenze a "
        strSQL = strSQL & "INNER JOIN utentiunsc b ON a.idregionecompetenza = b.idregionecompetenza "
        strSQL = strSQL & "where b.username = '" & Session("Utente") & "'"

        'controllo se utente o ente regionale
        'eseguo la query
        dtrCompetenze = ClsServer.CreaDatareader(strSQL, Session("conn"))
        dtrCompetenze.Read()
        If dtrCompetenze.HasRows = True Then
            UtenteRegioneCompetenza = dtrCompetenze("Descrizione")
        End If
        If Not dtrCompetenze Is Nothing Then
            dtrCompetenze.Close()
            dtrCompetenze = Nothing
        End If

        '********************* BLOCCO SEDI NON MODIFICATE
        strSQL = "SELECT entifiglio.Denominazione + '(' + entifiglio.CodiceRegione + ')' as Ente,  entisedi.Denominazione AS SedeFisica, entisedi.Indirizzo,  comuni.denominazione as Comune,entisedi.telefono,entisediattuazioni.Denominazione AS SedeAttuazione,  replace(statientisedi.statoentesede, 'Accreditata', 'Iscritta') StatoEnteSede , entisedi.IDEnteSede,  entisediattuazioni.IDEnteSedeAttuazione, '' as idatt,regionicompetenze.descrizione as Competenza,  isnull(avc.Certificazione,2) as Certificazione,  case isnull(avc.Certificazione,2)when 0 then 0 when 1 then 1 when 2 then 0 end as riccheck,  case isnull(avc.Certificazione,2)when 0 then 'No' WHEN 1 then 'Si' WHEN 2 THEN 'Da Valutare' end as codificaCert,  case isnull(avc.Certificazione,2)when 1 then    '<input onclick=javascript:ControllaSedeAttuazione(''' + convert(varchar,entisediattuazioni.idEntesedeattuazione) + ''') type=checkbox value=1 name=chkCert' + convert(varchar,entisediattuazioni.idEntesedeattuazione) + ' id=chkCert' + convert(varchar,entisediattuazioni.idEntesedeattuazione) + ' checked >'    when 0 then '<input onclick=javascript:ControllaSedeAttuazione(''' + convert(varchar,entisediattuazioni.idEntesedeattuazione) + ''') type=checkbox value=0 name=chkCert' + convert(varchar,entisediattuazioni.idEntesedeattuazione) + ' id=chkCert' + convert(varchar,entisediattuazioni.idEntesedeattuazione) + '>' End As CheckCert ,  entisedi.IDEnte,  isnull(entisediattuazioni.Segnalazione,0) as Segnalazione,   Case isnull(entisediattuazioni.SedeSottopostaVerifica,0) When 0 then 'No' When '1' then 'Si' end as verifica,  EntiFasi.IdEnteFase  ,isnull(avc.certificazione,2) as AVCCertificazione, isnull(avc.NMaxVolontari,0) as NMaxVolontari " &
                " FROM entisedi " &
                " INNER JOIN entisediattuazioni ON entisedi.IDEnteSede = entisediattuazioni.IDEnteSede " &
                " INNER JOIN enti ON entisediattuazioni.IDEnteCapofila = enti.IDEnte " &
                " inner join enti entifiglio on entifiglio.IDEnte = entisedi.IDEnte " &
                " left  join statientisedi statosedefisica on(entisediattuazioni.idstatoentesede=statosedefisica.idstatoEntesede) " &
                " inner  join statientisedi on entisediattuazioni.idstatoentesede=statientisedi.idstatoEntesede " &
                " INNER JOIN entiseditipi ON entisedi.IDEnteSede = entiseditipi.IDEnteSede " &
                " INNER JOIN tipisedi ON entiseditipi.IDTipoSede = tipisedi.IDTipoSede " &
                " INNER JOIN comuni ON entisedi.IDComune = comuni.IDComune  " &
                " INNER JOIN provincie ON comuni.IDProvincia = provincie.IDProvincia " &
                " INNER JOIN regioni ON provincie.IDRegione = regioni.IDRegione " &
                " inner Join regionicompetenze on enti.idregionecompetenza = regionicompetenze.idregionecompetenza " &
                " inner JOIN EntiFasi_Sedi ON  entisediattuazioni.IDEnteSedeAttuazione = EntiFasi_Sedi.IdEnteSedeAttuazione " &
                " inner JOIN EntiFasi ON EntiFasi.IdEnteFase = EntiFasi_Sedi.IdEnteFase  " &
                " inner join entisediattuazioni avc ON entisediattuazioni.identesedeattuazione  =avc.identesedeattuazione " &
                " WHERE entisediattuazioni.IdStatoEnteSede in (1,4) and isnull(entisediattuazioni.RichiestaModifica,0) = 0 " &
                " and entiseditipi.idtiposede=4 and entifiglio.idstatoente in (3,6,8,9) and EntiFasi.Stato =3 "
        'and  EntiFasi.Stato =3 "

        If Trim(txtdenominazione.Text) <> "" Then
            strSQL = strSQL & " and entisedi.denominazione like '" & Replace(txtdenominazione.Text, "'", "''") & "%'"
        End If

        If Trim(txtregione.Text) <> vbNullString Then
            strSQL = strSQL & " and Regioni.Regione LIKE '" & Replace(txtregione.Text, "'", "''") & "%'"
        End If
        If Trim(txtProvincia.Text) <> vbNullString Then
            strSQL = strSQL & " and Provincie.Provincia LIKE '" & Replace(txtProvincia.Text, "'", "''") & "%'"
        End If
        If Trim(txtComune.Text) <> vbNullString Then
            strSQL = strSQL & " and Comuni.denominazione LIKE '" & Replace(txtComune.Text, "'", "''") & "%'"
        End If

        If Trim(TxtDenomEnte.Text) <> vbNullString Then
            strSQL = strSQL & " and enti.denominazione LIKE '" & Replace(TxtDenomEnte.Text, "'", "''") & "%'"
        End If

        If Trim(txtCodSedeAtt.Text) <> vbNullString Then
            strSQL = strSQL & " and EntiSediAttuazioni.identesedeattuazione=" & txtCodSedeAtt.Text & ""
        End If

        If ddlCertificazione.SelectedItem.Text <> "Tutti" Then
            strSQL = strSQL & " and isnull(entisediattuazioni.Certificazione,2) =" & ddlCertificazione.SelectedValue
        End If
        If Trim(txtCodiceEnte.Text) <> vbNullString Then
            strSQL = strSQL & " and Enti.CodiceRegione ='" & txtCodiceEnte.Text & "'"
        End If
        If CboCompetenza.SelectedValue <> "" Then
            Select Case CboCompetenza.SelectedValue
                Case 0
                    strSQL = strSQL & " "
                Case -1
                    strSQL = strSQL & " And enti.IdRegioneCompetenza = 22"
                Case -2
                    strSQL = strSQL & " And enti.IdRegioneCompetenza <> 22 And not enti.IdRegioneCompetenza is null "
                Case -3
                    strSQL = strSQL & " And enti.IdRegioneCompetenza is null "
                Case Else
                    strSQL = strSQL & " And enti.IdRegioneCompetenza = " & CboCompetenza.SelectedValue
            End Select
        End If
        'agg. da sc il 05/07/2011 combo su segnalazione della sede
        If ddlSegnalazioneSanzione.SelectedItem.Text <> "Tutti" Then
            strSQL = strSQL & " and isnull(entisediattuazioni.Segnalazione,0) =" & ddlSegnalazioneSanzione.SelectedValue & ""
        End If
        If ddlstato.SelectedItem.Text <> "" Then
            strSQL = strSQL & " and case isnull(statosedefisica.statoentesede,'vuoto') when 'vuoto' then statientisedi.statoentesede else  statosedefisica.statoentesede end ='" & Replace(ddlstato.SelectedItem.Text, "Iscritta", "Accreditata") & "'"
        End If
        If txtIDFaseEnte.Text <> "" Then
            strSQL = strSQL & " and  EntiFasi.IdEnteFase =" & txtIDFaseEnte.Text & ""
        End If

        strSQL = strSQL & " UNION "
        '********************* BLOCCO SEDI MODIFICATE
        strSQL = strSQL & "SELECT entifiglio.Denominazione + '(' + entifiglio.CodiceRegione + ')' as Ente,  entisedi.Denominazione AS SedeFisica, entisedi.Indirizzo,  comuni.denominazione as Comune,entisedi.telefono,entisediattuazioni.Denominazione AS SedeAttuazione,  replace(statientisedi.statoentesede, 'Accreditata', 'Iscritta') StatoEnteSede , entisedi.IDEnteSede,  entisediattuazioni.IDEnteSedeAttuazione, '' as idatt,regionicompetenze.descrizione as Competenza,  isnull(avc.Certificazione,2) as Certificazione,  case isnull(avc.Certificazione,2)when 0 then 0 when 1 then 1 when 2 then 0 end as riccheck,  case isnull(avc.Certificazione,2)when 0 then 'No' WHEN 1 then 'Si' WHEN 2 THEN 'Da Valutare' end as codificaCert,  case isnull(avc.Certificazione,2)when 1 then    '<input onclick=javascript:ControllaSedeAttuazione(''' + convert(varchar,entisediattuazioni.idEntesedeattuazione) + ''') type=checkbox value=1 name=chkCert' + convert(varchar,entisediattuazioni.idEntesedeattuazione) + ' id=chkCert' + convert(varchar,entisediattuazioni.idEntesedeattuazione) + ' checked >'    when 0 then '<input onclick=javascript:ControllaSedeAttuazione(''' + convert(varchar,entisediattuazioni.idEntesedeattuazione) + ''') type=checkbox value=0 name=chkCert' + convert(varchar,entisediattuazioni.idEntesedeattuazione) + ' id=chkCert' + convert(varchar,entisediattuazioni.idEntesedeattuazione) + '>' End As CheckCert ,  entisedi.IDEnte,  isnull(entisediattuazioni.Segnalazione,0) as Segnalazione,   Case isnull(entisediattuazioni.SedeSottopostaVerifica,0) When 0 then 'No' When '1' then 'Si' end as verifica,  EntiFasi.IdEnteFase  ,isnull(avc.certificazione,2) as AVCCertificazione, isnull(avc.NMaxVolontari,0) as NMaxVolontari " &
        " FROM entisedi " &
        " INNER JOIN entisediattuazioni ON entisedi.IDEnteSede = entisediattuazioni.IDEnteSede " &
        " INNER JOIN enti ON entisediattuazioni.IDEnteCapofila = enti.IDEnte " &
        " inner join enti entifiglio on entifiglio.IDEnte = entisedi.IDEnte " &
        " left  join statientisedi statosedefisica on(entisediattuazioni.idstatoentesede=statosedefisica.idstatoEntesede) " &
        " inner  join statientisedi on entisediattuazioni.idstatoentesede=statientisedi.idstatoEntesede " &
        " INNER JOIN entiseditipi ON entisedi.IDEnteSede = entiseditipi.IDEnteSede " &
        " INNER JOIN tipisedi ON entiseditipi.IDTipoSede = tipisedi.IDTipoSede " &
        " INNER JOIN comuni ON entisedi.IDComune = comuni.IDComune  " &
        " INNER JOIN provincie ON comuni.IDProvincia = provincie.IDProvincia " &
        " INNER JOIN regioni ON provincie.IDRegione = regioni.IDRegione " &
        " inner Join regionicompetenze on enti.idregionecompetenza = regionicompetenze.idregionecompetenza " &
        " inner JOIN EntiFasi_Sedi ON  entisediattuazioni.IDEnteSedeAttuazione = EntiFasi_Sedi.IdEnteSedeAttuazione " &
        " inner JOIN EntiFasi ON EntiFasi.IdEnteFase = EntiFasi_Sedi.IdEnteFase  " &
        " inner join Accreditamento_VariazioneSedi avc on avc.identesede = entisedi.IDEnteSede and StatoVariazione = 0 " &
        " WHERE entisediattuazioni.IdStatoEnteSede in (1,4) and isnull(entisediattuazioni.RichiestaModifica,0) = 1 " &
        " and entiseditipi.idtiposede=4 and entifiglio.idstatoente in (3,6,8,9) and EntiFasi.Stato =3 "
        'and  EntiFasi.Stato =3 "

        If Trim(txtdenominazione.Text) <> "" Then
            strSQL = strSQL & " and entisedi.denominazione like '" & Replace(txtdenominazione.Text, "'", "''") & "%'"
        End If

        If Trim(txtregione.Text) <> vbNullString Then
            strSQL = strSQL & " and Regioni.Regione LIKE '" & Replace(txtregione.Text, "'", "''") & "%'"
        End If
        If Trim(txtProvincia.Text) <> vbNullString Then
            strSQL = strSQL & " and Provincie.Provincia LIKE '" & Replace(txtProvincia.Text, "'", "''") & "%'"
        End If
        If Trim(txtComune.Text) <> vbNullString Then
            strSQL = strSQL & " and Comuni.denominazione LIKE '" & Replace(txtComune.Text, "'", "''") & "%'"
        End If

        If Trim(TxtDenomEnte.Text) <> vbNullString Then
            strSQL = strSQL & " and enti.denominazione LIKE '" & Replace(TxtDenomEnte.Text, "'", "''") & "%'"
        End If

        If Trim(txtCodSedeAtt.Text) <> vbNullString Then
            strSQL = strSQL & " and EntiSediAttuazioni.identesedeattuazione=" & txtCodSedeAtt.Text & ""
        End If

        If ddlCertificazione.SelectedItem.Text <> "Tutti" Then
            strSQL = strSQL & " and isnull(AVC.Certificazione,2) =" & ddlCertificazione.SelectedValue
        End If
        If Trim(txtCodiceEnte.Text) <> vbNullString Then
            strSQL = strSQL & " and Enti.CodiceRegione ='" & txtCodiceEnte.Text & "'"
        End If
        If CboCompetenza.SelectedValue <> "" Then
            Select Case CboCompetenza.SelectedValue
                Case 0
                    strSQL = strSQL & " "
                Case -1
                    strSQL = strSQL & " And enti.IdRegioneCompetenza = 22"
                Case -2
                    strSQL = strSQL & " And enti.IdRegioneCompetenza <> 22 And not enti.IdRegioneCompetenza is null "
                Case -3
                    strSQL = strSQL & " And enti.IdRegioneCompetenza is null "
                Case Else
                    strSQL = strSQL & " And enti.IdRegioneCompetenza = " & CboCompetenza.SelectedValue
            End Select
        End If
        'agg. da sc il 05/07/2011 combo su segnalazione della sede
        If ddlSegnalazioneSanzione.SelectedItem.Text <> "Tutti" Then
            strSQL = strSQL & " and isnull(entisediattuazioni.Segnalazione,0) =" & ddlSegnalazioneSanzione.SelectedValue & ""
        End If
        If ddlstato.SelectedItem.Text <> "" Then
            strSQL = strSQL & " and case isnull(statosedefisica.statoentesede,'vuoto') when 'vuoto' then statientisedi.statoentesede else  statosedefisica.statoentesede end ='" & Replace(ddlstato.SelectedItem.Text, "Iscritta", "Accreditata") & "'"
        End If
        If txtIDFaseEnte.Text <> "" Then
            strSQL = strSQL & " and  EntiFasi.IdEnteFase =" & txtIDFaseEnte.Text & ""
        End If

        strSQL = strSQL & "  order by isnull(AVC.certificazione,2)desc, entisediattuazioni.IDEnteSedeAttuazione "

        dtsgenerico = ClsServer.DataSetGenerico(strSQL, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))


        CaricaDataGrid(dgRisultatoRicerca)
        dtSedi = ClsServer.CreaDataTable(strSQL, False, Session("conn"))
        'assegno il dataset alla griglia del risultato
        dgRisultatoRicerca.DataSource = dtSedi
        dgRisultatoRicerca.DataBind()
        Session("SessionDtSedi") = dtSedi
        TogliCheck()
        'ControllaSedeAttuazione()
        ColoraCelle()
    End Sub

    Sub CaricaDataGrid(ByRef GridDaCaricare As DataGrid) 'valorizzo la datagrid passata

        GridDaCaricare.DataSource = dtsgenerico
        GridDaCaricare.DataBind()

        'AGG DA SIMONA CORDELLA IL 07/07/2011
        GridDaCaricare.Columns(18).Visible = ClsUtility.ForzaPresenzaSanzione(Session("Utente"), Session("conn"))
        GridDaCaricare.Columns(19).Visible = ClsUtility.ForzaSedeSottopostaVerifica(Session("Utente"), Session("conn"))


        If Request.QueryString("PopSede") = 1 Then

            Dim NomeColonne(8) As String
            Dim NomiCampiColonne(8) As String
            'nome della colonna 
            'e posizione nella griglia di lettura
            NomeColonne(0) = "Sede Fisica"
            NomeColonne(1) = "Indirizzo"
            NomeColonne(2) = "Comune"
            NomeColonne(3) = "Sede Attuazione"
            NomeColonne(4) = "Telefono"
            NomeColonne(5) = "Stato"
            NomeColonne(6) = "Cod. Sede"
            NomeColonne(7) = "Presenza Iscrizione"

            NomiCampiColonne(0) = "sedefisica"
            NomiCampiColonne(1) = "indirizzo"
            NomiCampiColonne(2) = "comune"
            NomiCampiColonne(3) = "sedeattuazione"
            NomiCampiColonne(4) = "telefono"
            NomiCampiColonne(5) = "statoentesede"
            NomiCampiColonne(6) = "idEntesedeattuazione"
            NomiCampiColonne(7) = "codificaCert"
            'carico un datatable che userò poi nella pagina di stampa
            'il numero delle colonne è a base 0
            Session("DtbRicerca") = ClsServer.CaricaDataTablePerStampa(dtsgenerico, 7, NomeColonne, NomiCampiColonne)
        Else


            '*********************************************************************************
            'blocco per la creazione della datatable per la stampa della ricerca
            'nome e posizione di lettura delle colopnne a base 0
            Dim NomeColonne(11) As String
            Dim NomiCampiColonne(11) As String
            'nome della colonna 
            'e posizione nella griglia di lettura
            NomeColonne(0) = "Ente"
            NomeColonne(1) = "Sede Fisica"
            NomeColonne(2) = "Indirizzo"
            NomeColonne(3) = "Comune"
            NomeColonne(4) = "Sede Attuazione"
            NomeColonne(5) = "Telefono"
            NomeColonne(6) = "Stato"
            NomeColonne(7) = "Cod. Sede"
            NomeColonne(8) = "Compentenza"
            NomeColonne(9) = "Presenza Iscrizione"
            NomeColonne(10) = "NMaxVol"
            NomeColonne(11) = "Rif.Fase"

            NomiCampiColonne(0) = "ente"
            NomiCampiColonne(1) = "sedefisica"
            NomiCampiColonne(2) = "indirizzo"
            NomiCampiColonne(3) = "comune"
            NomiCampiColonne(4) = "sedeattuazione"
            NomiCampiColonne(5) = "telefono"
            NomiCampiColonne(6) = "statoentesede"
            NomiCampiColonne(7) = "idEntesedeattuazione"
            NomiCampiColonne(8) = "competenza"
            NomiCampiColonne(9) = "codificaCert"
            NomiCampiColonne(10) = "NMaxVolontari"
            NomiCampiColonne(11) = "IdEnteFase"
            'carico un datatable che userò poi nella pagina di stampa
            'il numero delle colonne è a base 0
            Session("DtbRicerca") = ClsServer.CaricaDataTablePerStampa(dtsgenerico, 11, NomeColonne, NomiCampiColonne)

            '*********************************************************************************
        End If

        If GridDaCaricare.Items.Count <> 0 Then
            chkSelDesel.Visible = True
            cmdSalva.Visible = True
            CmdEsporta.Visible = True
        Else
            chkSelDesel.Visible = False
            cmdSalva.Visible = False
            CmdEsporta.Visible = False
        End If
    End Sub

    Private Sub cmdSalva_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdSalva.Click
        If Request.QueryString("PopSede") = 1 Then
            SalvataggioSediProgetto()
        Else
            SalvataggioSedi()
        End If
    End Sub

    Sub SalvataggioSediProgetto()
        Dim ContaCheck As Integer = 0
        Dim i As Integer
        Dim chkValore As CheckBox
        Dim dtGriglia As DataTable 'appoggio
        Dim rwGriglia As DataRow
        Dim clGriglia As DataColumn


        'dgRisultatoRicerca.CurrentPageIndex = 0
        lblmessaggio.Text = ""


        dtGriglia = Session("SessionDtSedi")
        clGriglia = dtGriglia.Columns(12)
        clGriglia.ReadOnly = False

        'ricordo il valore ceccato nella pagina corrente
        For i = 0 To dgRisultatoRicerca.Items.Count - 1
            chkValore = dgRisultatoRicerca.Items.Item(i).FindControl("chkSedi")
            rwGriglia = dtGriglia.Rows(i + (dgRisultatoRicerca.CurrentPageIndex * 10))
            If chkValore.Checked = True Then
                rwGriglia.Item(12) = "1"
            Else
                rwGriglia.Item(12) = "0"
            End If
        Next i

        Session("SessionDtOlp") = dtGriglia
        clGriglia = dtGriglia.Columns(5)
        clGriglia.ReadOnly = False
        '---ciclo il datatable per salvare il flag della certificazione in entisediattuazione
        For i = 0 To dtGriglia.Rows.Count - 1
            Select Case dtGriglia.Rows(i).Item(11) 'controllo campo certificazione
                Case 0 'CERTIFICATO A NO
                    If chkSelDesel.Checked = False Then
                        If dtGriglia.Rows(i).Item(12) = 1 Then 'è CECCATO
                            ModificaSede(CInt(dtGriglia.Rows(i).Item(8)), 1)
                            ContaCheck = ContaCheck + 1
                        End If
                    Else
                        ModificaSede(CInt(dtGriglia.Rows(i).Item(8)), 1)
                        ContaCheck = ContaCheck + 1
                    End If

                Case 1 'CERTIFICATO A SI
                    If dtGriglia.Rows(i).Item(12) = 0 Then 'è CECCATO
                        ModificaSede(CInt(dtGriglia.Rows(i).Item(8)), 0)
                        ContaCheck = ContaCheck + 1
                    End If
                Case 2
                    ModificaSede(CInt(dtGriglia.Rows(i).Item(8)), 1) 'IDENTESEDETEATTUAZIONE
                    ContaCheck = ContaCheck + 1
            End Select

            If dtGriglia.Rows(i).Item(11) = 0 Then
                If dtGriglia.Rows(i).Item(12) = 1 Then
                    ModificaSede(CInt(dtGriglia.Rows(i).Item(8)), 1)
                    ContaCheck = ContaCheck + 1
                End If
            End If
        Next i
        If ContaCheck = 0 Then
            lblmessaggio.Text = "Non è stata selezionata nessuna sede da certificare."
        End If
        lblmessaggio.Text = "Salvataggio eseguito con successo."
        RicercaSediProgetto()
    End Sub

    Sub SalvataggioSedi()
        Dim ContaCheck As Integer = 0
        Dim i As Integer
        Dim chkValore As CheckBox
        Dim dtGriglia As DataTable 'appoggio
        Dim rwGriglia As DataRow
        Dim clGriglia As DataColumn


        'dgRisultatoRicerca.CurrentPageIndex = 0
        lblmessaggio.Text = ""


        dtGriglia = Session("SessionDtSedi")
        clGriglia = dtGriglia.Columns(12)
        clGriglia.ReadOnly = False

        'ricordo il valore ceccato nella pagina corrente
        For i = 0 To dgRisultatoRicerca.Items.Count - 1
            chkValore = dgRisultatoRicerca.Items.Item(i).FindControl("chkSedi")
            rwGriglia = dtGriglia.Rows(i + (dgRisultatoRicerca.CurrentPageIndex * 10))
            If chkValore.Checked = True Then
                rwGriglia.Item(12) = "1"
            Else
                rwGriglia.Item(12) = "0"
            End If
        Next i

        Session("SessionDtOlp") = dtGriglia
        clGriglia = dtGriglia.Columns(5)
        clGriglia.ReadOnly = False
        '---ciclo il datatable per salvare il flag della certificazione in entisediattuazione
        For i = 0 To dtGriglia.Rows.Count - 1
            Select Case dtGriglia.Rows(i).Item(11) 'controllo campo certificazione
                Case 0 'CERTIFICATO A NO
                    If dtGriglia.Rows(i).Item(12) = 1 Then 'è CECCATO
                        ModificaSede(CInt(dtGriglia.Rows(i).Item(8)), 1) 'IDENTESEDETEATTUAZIONE
                        ContaCheck = ContaCheck + 1
                    End If
                Case 1 'CERTIFICATO A SI
                    If dtGriglia.Rows(i).Item(12) = 0 Then 'è CECCATO
                        ModificaSede(CInt(dtGriglia.Rows(i).Item(8)), 0) 'IDENTESEDETEATTUAZIONE
                        ContaCheck = ContaCheck + 1
                    End If
                Case 2 'CERTIFICATO DA VALUTARE
                    If chkSelDesel.Checked = False Then
                        If dtGriglia.Rows(i).Item(12) = 1 Then 'è CECCATO
                            ModificaSede(CInt(dtGriglia.Rows(i).Item(8)), 1) 'IDENTESEDETEATTUAZIONE
                            ContaCheck = ContaCheck + 1
                        End If
                    Else
                        ModificaSede(CInt(dtGriglia.Rows(i).Item(8)), 1) 'IDENTESEDETEATTUAZIONE
                        ContaCheck = ContaCheck + 1
                    End If


            End Select

            If dtGriglia.Rows(i).Item(11) = 0 Then
                If dtGriglia.Rows(i).Item(12) = 1 Then
                    ModificaSede(CInt(dtGriglia.Rows(i).Item(8)), 1) 'IDENTESEDETEATTUAZIONE
                    ContaCheck = ContaCheck + 1
                End If
            End If
        Next i
        If ContaCheck = 0 Then
            lblmessaggio.Text = "Non è stata selezionata nessuna sede da certificare."
        End If
        lblmessaggio.Text = "Salvataggio eseguito con successo."
        RicercaSedi()
    End Sub

    Sub ModificaSede(ByVal IDEnteSedeAttuazione As Integer, ByVal Certificazione As Integer)
        Dim strSql As String
        Dim strNull As String = "null"
        Dim CmdSede As SqlClient.SqlCommand

        strSql = "UPDATE  entisediattuazioni SET "
        strSql = strSql & "  Certificazione= " & Certificazione & " ,"
        If (Certificazione = 1 Or Certificazione = 0) Then
            strSql = strSql & " DataCertificazione =getdate(), "
            strSql = strSql & " UserCertificazione ='" & Session("Utente") & "' "
        Else
            strSql = strSql & " DataCertificazione = " & strNull & ", "
            strSql = strSql & " UserCertificazione = " & strNull & " "
        End If
        strSql = strSql & "  WHERE identesedeattuazione =" & IDEnteSedeAttuazione

        CmdSede = ClsServer.EseguiSqlClient(strSql, Session("conn"))


        strSql = "UPDATE  Accreditamento_VariazioneSedi SET "
        strSql = strSql & "  Certificazione= " & Certificazione & " ,"
        If (Certificazione = 1 Or Certificazione = 0) Then
            strSql = strSql & " DataCertificazione =getdate(), "
            strSql = strSql & " UserCertificazione ='" & Session("Utente") & "' "
        Else
            strSql = strSql & " DataCertificazione = " & strNull & ", "
            strSql = strSql & " UserCertificazione = " & strNull & " "
        End If
        strSql = strSql & "  WHERE statovariazione = 0 and identesede = (select identesede from entisediattuazioni where identesedeattuazione =" & IDEnteSedeAttuazione & ")"

        CmdSede = ClsServer.EseguiSqlClient(strSql, Session("conn"))

    End Sub

    Private Sub cmdChiudi_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdChiudi.Click
        If Request.QueryString("popup") = "1" Then
            Response.Redirect(ClsUtility.TrovaAlboProgetto(Request.QueryString("IdAttivita"), Session("Conn")) & "?PopSede=" & Request.QueryString("PopSede") & "&popup=" & Request.QueryString("popup") & "&Modifica=" & Request.QueryString("Modifica") & "&Nazionale=" & Request.QueryString("Nazionale") & "&IdAttivita=" & CInt(Request.QueryString("IdAttivita")) & "")
            'Response.Redirect("TabProgetti.aspx?PopSede=" & Request.QueryString("PopSede") & "&popup=" & Request.QueryString("popup") & "&Modifica=" & Request.QueryString("Modifica") & "&Nazionale=" & Request.QueryString("Nazionale") & "&IdAttivita=" & CInt(Request.QueryString("IdAttivita")) & "")
        Else
            Response.Redirect("WfrmMain.aspx")
        End If
    End Sub

    Private Sub CmdEsporta_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdEsporta.Click
        CmdEsporta.Visible = False
        Dim dtbRicerca As DataTable = Session("DtbRicerca")
        StampaCSV(dtbRicerca)
    End Sub

    Private Sub StampaCSV(ByVal dtbRicerca As DataTable)
        Dim path As String
        Dim xPrefissoNome As String
        Dim url As String
        Dim utility As ClsUtility = New ClsUtility()

        If dtbRicerca.Rows.Count = 0 Then
            ApriCSV1.Visible = False
            CmdEsporta.Visible = False
        Else
            xPrefissoNome = Session("Utente")
            path = Server.MapPath("download")
            url = CreaFileCSV(dtbRicerca, xPrefissoNome, path)
            ApriCSV1.Visible = True
            ApriCSV1.NavigateUrl = url
        End If

    End Sub

    Function CreaFileCSV(ByVal DTBRicerca As DataTable, ByVal xPrefissoNome As String, ByVal mapPath As String) As String

        Dim dtrSediAttuazione As Data.SqlClient.SqlDataReader
        Dim Writer As StreamWriter
        Dim xLinea As String
        Dim i As Int64
        Dim j As Int64
        Dim NomeUnivoco As String
        Dim Reader As StreamReader
        Dim url As String
        NomeUnivoco = xPrefissoNome & "ExpDati" & Year(Now) & Month(Now) & Day(Now) & Hour(Now) & Minute(Now) & Second(Now)
        Writer = New StreamWriter(mapPath & "\" & NomeUnivoco & ".CSV")
        'Creazione dell'inntestazione del CSV
        Dim intNumCol As Int64 = DTBRicerca.Columns.Count
        For i = 0 To intNumCol - 1
            xLinea &= DTBRicerca.Columns.Item(CInt(i)).ColumnName() & ";"
        Next
        Writer.WriteLine(xLinea)
        xLinea = vbNullString

        'Scorro tutte le righe del datatable e riempio il CSV
        For i = 0 To DTBRicerca.Rows.Count - 1

            For j = 0 To intNumCol - 1
                If IsDBNull(DTBRicerca.Rows(CInt(i)).Item(CInt(j))) = True Then
                    xLinea &= vbNullString & ";"
                Else
                    xLinea &= ClsUtility.FormatExport(DTBRicerca.Rows(CInt(i)).Item(CInt(j))) & ";"
                End If
            Next

            Writer.WriteLine(xLinea)
            xLinea = vbNullString

        Next
        url = "download\" & NomeUnivoco & ".CSV"

        Writer.Close()
        Writer = Nothing
        Return url
    End Function

    Private Sub chkSelDesel_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkSelDesel.CheckedChanged

        Dim i As Integer
        Dim Mychk As CheckBox

        For i = 0 To dgRisultatoRicerca.Items.Count - 1
            Mychk = dgRisultatoRicerca.Items.Item(i).FindControl("chkSedi")

            If (Mychk.Enabled = True) Then
                Mychk.Checked = chkSelDesel.Checked
                hdd_Check.Value = "SI"
            End If

        Next i
       
    End Sub

End Class