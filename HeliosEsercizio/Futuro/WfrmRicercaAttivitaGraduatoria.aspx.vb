Imports System.IO


Public Class WfrmRicercaAttivitaGraduatoria
    Inherits System.Web.UI.Page
    Dim strsql As String
    Dim dtrgenerico As SqlClient.SqlDataReader
    Dim dtsGenerico As DataSet


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Inserire qui il codice utente necessario per inizializzare la pagina
        If Not Session("LogIn") Is Nothing Then
            If Session("LogIn") = False Then 'verifico validità log-in
                Response.Redirect("LogOn.aspx")
            End If
        Else
            Response.Redirect("LogOn.aspx")
        End If
        If ClsUtility.ForzaPresenzaSanzioneProgetto(Session("Utente"), Session("conn")) = False Then
            dgRisultatoRicerca.Columns(20).Visible = False
            ddlSegnalazioneSanzione.Visible = False
            lblSanzione.Visible = False
        End If
        If Request.QueryString("Vengoda") = "Presentazione" Then
            lblTitolo.Text = "Ricerca Progetti per Presentazione Graduatoria Volontari"
        End If
        If IsPostBack = False Then
            If Request.QueryString("Vengoda") = "Conferma" Then
                lblTitolo.Text = "Ricerca Progetti per Conferma Graduatoria Volontari"
                chkAttessaGraduatoria.Visible = True
                chkAttessaGraduatoria.Text = "Progetti in attesa di Conferma Graduatoria"
                chkAttessaGraduatoria.Checked = True
                lbldenominazione.Visible = True
                txtdenominazione.Visible = True
                lblCodEnte.Visible = True
                txtcodiceEnte.Visible = True
            End If
            If Request.QueryString("Vengoda") = "Presentazione" Then
                chkAttessaGraduatoria.Visible = True
                chkAttessaGraduatoria.Text = "Progetti in attesa di Presentazione Graduatoria"
                chkAttessaGraduatoria.Checked = True
            End If
            If Request.QueryString("Vengoda") = "Visualizzazione" Then
                chkAttessaGraduatoria.Visible = False
                chkAttessaGraduatoria.Text = "Progetti in attesa di Presentazione Graduatoria"
                chkAttessaGraduatoria.Checked = False
            End If
            Call CaricaComboSet()

            'se provengo da una ricerca
            If Request.QueryString("CheckIndietro") = "true" Then
                CaricaPaginaConDati()
            End If
    
        End If
    End Sub

    Protected Sub cmdChiudi_Click(sender As Object, e As EventArgs) Handles cmdChiudi.Click
        Response.Redirect("WfrmMain.aspx")
    End Sub

    Protected Sub cmdSalva_Click(sender As Object, e As EventArgs) Handles cmdSalva.Click
        dgRisultatoRicerca.CurrentPageIndex = 0
        lblmessaggio.Text = ""
        Call CaricaGriglia()

    End Sub

    Private Sub dgRisultatoRicerca_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgRisultatoRicerca.ItemCommand
        If e.CommandName = "Select" Then
            If (Session("TipoUtente") = "U" Or Session("TipoUtente") = "R") Then
                Session("IdEnte") = e.Item.Cells(3).Text
                Session("Denominazione") = e.Item.Cells(2).Text
            End If
            'MODIFCATO DA SIMONA CORDELLA IL 30/05/2006
            Response.Redirect("WfrmElencoSediAttuazione.aspx?Segnalato=" & e.Item.Cells(19).Text & "&PaginaGrid=" & dgRisultatoRicerca.CurrentPageIndex & "&CodiceProgetto=" & Trim(txtCodProgetto.Text) & "&InAttesa=" & IIf(chkAttessaGraduatoria.Checked = True, "true", "false") & "&Settore=" & cboSettore.SelectedValue & "&Bando=" & Trim(cbobando.SelectedValue) & "&Progetto=" & Trim(txtTitoloProgetto.Text).Replace("#", "") & "&CodEnte=" & Trim(txtcodiceEnte.Text) & "&Ente=" & Trim(txtdenominazione.Text) & "&CheckIndietro=true&IdAttivita=" & e.Item.Cells(12).Text & "&IdEnte=" & Session("IdEnte") & "&Azione=" & Request.QueryString("VengoDa"))
        End If
    End Sub

    Sub CaricaGriglia()
        Dim strSql As String
        Dim MyDataSet As DataSet

        dgRisultatoRicerca.CurrentPageIndex = 0

        Try
            Select Case Request.QueryString("VengoDa")

                Case "Inserimento"
                    If chkAttessaGraduatoria.Visible = False Then
                        chkAttessaGraduatoria.Visible = True
                        chkAttessaGraduatoria.Checked = True
                    End If
                    'Case "Conferma"
                    'Case "Presentazione"

            End Select


            strSql = "SELECT distinct " & _
                     "attività.IDAttività, " & _
                     "attività.titolo + ' [' + attività.CodiceEnte + '] ' as titolo,isnull(Attività.VolontariPresentati,'0') as NvolPart, " & _
                     "attività.SegnalaGraduatorieVolontari, " & _
                     "enti.IDente, " & _
                     "enti.denominazione as ente,enti.idente,enti.Codiceregione, attività.Titolo, attività.CodiceEnte, bando.Bando, macroambitiattività.Codifica + ' - ' + macroambitiattività.MacroAmbitoAttività " & _
            "  + ' / ' + ambitiattività.Codifica + ' - ' + ambitiattività.AmbitoAttività AS ambito," & _
            " Contasedi.Conta AS NumSediAtt, " & _
            " (SELECT COUNT(*) FROM graduatorieentità s1  " & _
            " WHERE s1.idattivitàsedeassegnazione in (select attivitàsediassegnazione.idattivitàsedeassegnazione " & _
            " from attivitàsediassegnazione where attivitàsediassegnazione.idattività=attività.idattività) and ammesso=1) AS  NumVolAmm," & _
            " (SELECT  case isnull(s2.NumeroGiovaniMinoriOpportunità,0) when 0 " & _
            "then CONVERT (VARCHAR,ISNULL((s2.NumeroPostiNoVittoNoAlloggio + s2.NumeroPostiVittoAlloggio + s2.NumeroPostiVitto), 0)) " & _
            "else CONVERT (VARCHAR,ISNULL((s2.NumeroPostiNoVittoNoAlloggio + s2.NumeroPostiVittoAlloggio + s2.NumeroPostiVitto), 0)) + ' (GMO:' + CONVERT(VARCHAR,S2.NumeroGiovaniMinoriOpportunità) + ')' END " & _
            " FROM  attività S2 WHERE S2.idattività=attività.IDAttività) as NumVolRic, " & _
            " (select count(GraduatorieEntità.IdGraduatoriaEntità) from GraduatorieEntità where idattivitàsedeassegnazione in " & _
            " (select attivitàsediassegnazione.idattivitàsedeassegnazione from attivitàsediassegnazione where attivitàsediassegnazione.idattività=attività.idattività)) AS NumVolIns, attività.IDAttività,  " & _
            " (SELECT COUNT(DISTINCT(AttivitàEntità.IDEntità)) FROM AttivitàEntità INNER JOIN AttivitàEntiSediAttuazione ON AttivitàEntità.IDAttivitàEnteSedeAttuazione = AttivitàEntiSediAttuazione.IDAttivitàEnteSedeAttuazione WHERE AttivitàEntiSediAttuazione.IdAttività = attività.IDAttività) as NumVolAss," & _
            " (select case isnull(count(DISTINCT AttivitàSediAssegnazione.identesede),-1) when -1 then 0 else count(DISTINCT AttivitàSediAssegnazione.identesede) end  " & _
            " FROM AttivitàSediAssegnazione " & _
            " WHERE(AttivitàSediAssegnazione.IDAttività = attività.IDAttività And AttivitàSediAssegnazione.statograduatoria=2 ))as NsedidaConfrmare, " & _
            " (  select count(DISTINCT SUBASA.identesede) FROM AttivitàSediAssegnazione SUBASA INNER JOIN attività SUBA ON SUBASA.IDAttività = SUBA.IDAttività  INNER JOIN TipiProgetto SUBTP ON SUBA.IdTipoProgetto = SUBTP.IdTipoProgetto INNER JOIN entisedi SUBES ON SUBASA.IDEnteSede = SUBES.IDEnteSede INNER JOIN comuni SUBCOM ON SUBES.IDComune = SUBCOM.IDComune  WHERE (SUBASA.IDAttività = attività.IDAttività and SUBASA.statograduatoria=1 AND SUBTP.NazioneBase = SUBCOM.ComuneNazionale))as NsedidaPresentare " & _
            " ,(SELECT    min(GraduatorieEntità.datamodifica)" & _
            " FROM  GraduatorieEntità INNER JOIN " & _
            " AttivitàSediAssegnazione ON " & _
            " GraduatorieEntità.IdAttivitàSedeAssegnazione = AttivitàSediAssegnazione.IDAttivitàSedeAssegnazione " & _
            " INNER JOIN attività a ON AttivitàSediAssegnazione.IDAttività = a.IDAttività " & _
            " where a.idattività=attività.idattività)as datainvio, attività.datainizioprevista, attività.datainizioattività, " & _
            " Case isnull(attività.SegnalazioneSanzione,0) When 0 then 'No' When 1 then '<img src=images/Anomalie.bmp onclick=VisualizzaSanzioneProg('+ convert(varchar, attività.IDAttività) + ','+ convert(varchar, attività.IDEntePresentante) + ') STYLE=cursor:hand title=Sanzione border=0>' End as SegnalazioneSanzione" & _
            " FROM attività " & _
            " inner join enti on enti.idente=attività.identepresentante " & _
            " INNER JOIN  ambitiattività ON attività.IDAmbitoAttività = ambitiattività.IDAmbitoAttività " & _
            " INNER JOIN  macroambitiattività ON ambitiattività.IDMacroAmbitoAttività = macroambitiattività.IDMacroAmbitoAttività " & _
            " INNER JOIN  BandiAttività ON attività.IDBandoAttività = BandiAttività.IdBandoAttività " & _
            " INNER JOIN  bando ON BandiAttività.IdBando = bando.IDBando " & _
            " INNER JOIN  attivitàsediAssegnazione ON attività.IDAttività = attivitàsediassegnazione.IDAttività  " & _
            " INNER JOIN statiattività ON attività.IDStatoAttività = statiattività.IDStatoAttività  " & _
            " INNER JOIN attivitàentisediattuazione ON attività.IDAttività = attivitàentisediattuazione.IDAttività " & _
            " INNER Join entisediattuazioni ON attivitàentisediattuazione.identesedeattuazione = entisediattuazioni.identesedeattuazione " & _
            " INNER Join entisedi ON entisediattuazioni.identesede = entisedi.identesede " & _
            " INNER Join comuni ON  entisedi.idcomune = comuni.idcomune " & _
            " INNER Join provincie ON comuni.idprovincia = provincie.idprovincia " & _
            " INNER Join regioni ON  provincie.idregione = regioni.idregione " & _
            " INNER JOIN (SELECT mya.IDATTIVITà, COUNT(*) AS CONTA FROM attività mya INNER JOIN TipiProgetto MYTP ON mya.IdTipoProgetto = MYTP.IdTipoProgetto left join ATTIVITàENTISEDIATTUAZIONE myb on mya.idattività = myb.idattività LEFT JOIN entisediattuazioni MYESA ON myb.IDEnteSedeAttuazione = MYESA.IDEnteSedeAttuazione LEFT JOIN entisedi MYES ON MYESA.IDEnteSede = MYES.IDEnteSede  LEFT JOIN comuni MYCOM ON MYES.IDComune = MYCOM.IDComune  WHERE IDStatoAttività IN (1,2) AND MYTP.NazioneBase = MYCOM.ComuneNazionale GROUP BY mya.IDATTIVITà) AS CONTASEDI on contasedi.idattività = attività.idattività " & _
            " INNER JOIN TIPIPROGETTO on attività.IdTipoProgetto=TIPIPROGETTO.IdTipoProgetto " & _
            " LEFT OUTER JOIN Programmi on attività.IdProgramma = Programmi.IdProgramma " & _
            " WHERE statiattività.idstatoattività in (1,2) "
            If Request.QueryString("VengoDa") <> "Conferma" Then
                strSql = strSql & "  And attività.IDEntePresentante =" & Session("IdEnte")
            End If

            'Controllo su chkAttessaGraduatoria  dinamico a senda della chiamata
            If chkAttessaGraduatoria.Checked = True Then
                Select Case chkAttessaGraduatoria.Text
                    Case "Progetti in attesa di Conferma Graduatoria"
                        strSql = strSql & " and (select case isnull(count(DISTINCT AttivitàSediAssegnazione.identesede),-1) when -1 then 0 else count(DISTINCT AttivitàSediAssegnazione.identesede) end  " & _
                        " FROM AttivitàSediAssegnazione " & _
                        " WHERE(AttivitàSediAssegnazione.IDAttività = attività.IDAttività and AttivitàSediAssegnazione.statograduatoria=2 ))>0 "
                    Case "Progetti in attesa di Presentazione Graduatoria"
                        strSql = strSql & " and (select case isnull(count(DISTINCT AttivitàSediAssegnazione.identesede),-1) when -1 then 0 else count(DISTINCT AttivitàSediAssegnazione.identesede) end  " & _
                        " FROM  AttivitàSediAssegnazione " & _
                        " WHERE(AttivitàSediAssegnazione.IDAttività = attività.IDAttività and AttivitàSediAssegnazione.statograduatoria=1 ))>0 "
                    Case "Progetti in Attesa di Graduatoria"
                        strSql = strSql & " and (select case isnull(count(DISTINCT AttivitàSediAssegnazione.identesede),-1) when -1 then 0 else count(DISTINCT AttivitàSediAssegnazione.identesede) end  " & _
                        " FROM  AttivitàSediAssegnazione " & _
                        " WHERE(AttivitàSediAssegnazione.IDAttività = attività.IDAttività and AttivitàSediAssegnazione.statograduatoria=1 ))>0 "
                End Select
            End If

            If Not dtrgenerico Is Nothing Then
                dtrgenerico.Close()
                dtrgenerico = Nothing
            End If

            Dim strQueryControlloLOTUS As String

            'Verifica se l'uetnte puo modificare il libretto postale
            strQueryControlloLOTUS = "SELECT AssociaUtenteGruppo.username, VociMenu.IdVoceMenu, VociMenu.VoceMenu, VociMenu.ImmaginePassiva, VociMenu.ImmagineAttiva, VociMenu.Link," & _
                     " VociMenu.IdVoceMenuPadre" & _
                     " FROM VociMenu" & _
                     " INNER JOIN 	AbilitazioniProfiliVociMenu ON VociMenu.IdVoceMenu = AbilitazioniProfiliVociMenu.IdVoceMenu" & _
                     " INNER JOIN	Profili ON AbilitazioniProfiliVociMenu.IdProfilo = Profili.IdProfilo"

            If Session("Read") <> "1" Then
                strQueryControlloLOTUS = strQueryControlloLOTUS & " INNER JOIN	AssociaUtenteGruppo ON Profili.IdProfilo = AssociaUtenteGruppo.IdProfilo "
            Else
                strQueryControlloLOTUS = strQueryControlloLOTUS & " INNER JOIN	AssociaUtenteGruppo ON Profili.IdProfilo = AssociaUtenteGruppo.IdProfiloRead "
            End If
            strQueryControlloLOTUS = strQueryControlloLOTUS & " LEFT JOIN 	RestrizioniUtenteVociMenu ON AssociaUtenteGruppo.UserName = RestrizioniUtenteVociMenu.UserName and RestrizioniUtenteVociMenu.IdVoceMenu=VociMenu.IdVoceMenu" & _
                     " WHERE VociMenu.descrizione = 'Forza Bando Lotus'" & _
                     " AND AssociaUtenteGruppo.username ='" & Session("Utente") & "' and (RestrizioniUtenteVociMenu.TipoRestrizione <> 0 or RestrizioniUtenteVociMenu.TipoRestrizione is null)"
            dtrgenerico = ClsServer.CreaDatareader(strQueryControlloLOTUS, Session("conn"))

            If dtrgenerico.HasRows = True Then

                strSql = strSql & " AND bando.LOTUS = 1 "

                If Not dtrgenerico Is Nothing Then
                    dtrgenerico.Close()
                    dtrgenerico = Nothing
                End If

                'Exit Function
            End If

            If Not dtrgenerico Is Nothing Then
                dtrgenerico.Close()
                dtrgenerico = Nothing
            End If

            If txtTitoloProgetto.Text <> "" Then
                strSql = strSql & " AND attività.Titolo like '" & Replace(txtTitoloProgetto.Text, "'", "''") & "%'"
            End If
            If txtCodProgetto.Text <> "" Then
                strSql = strSql & " AND attività.CodiceEnte like '" & Replace(txtCodProgetto.Text, "'", "''") & "%'"
            End If

            If txtTitoloProgramma.Text <> "" Then
                strSql = strSql & " AND Programmi.Titolo like '" & Replace(txtTitoloProgramma.Text, "'", "''") & "%'"
            End If
            If txtCodProgramma.Text <> "" Then
                strSql = strSql & " AND Programmi.CodiceProgramma like '" & Replace(txtCodProgramma.Text, "'", "''") & "%'"
            End If

            If cbobando.SelectedIndex >= 1 Then
                strSql = strSql & " AND  bando.IdBando= " & cbobando.Items(cbobando.SelectedIndex).Value
            End If
            If cboSettore.SelectedIndex >= 1 Then
                strSql = strSql & " AND  macroambitiattività.IDMacroAmbitoAttività = " & cboSettore.Items(cboSettore.SelectedIndex).Value
            End If

            If txtdenominazione.Text <> "" Then
                strSql = strSql & " AND  enti.Denominazione like '" & Replace(txtdenominazione.Text, "'", "''") & "%'"
            End If
            If txtcodiceEnte.Text <> "" Then
                strSql = strSql & " AND  enti.CodiceRegione='" & Replace(txtcodiceEnte.Text, "'", "''") & "'"
            End If
            If txtRegione.Text <> "" Then
                strSql = strSql & " AND regioni.regione LIKE '" & txtRegione.Text.Replace("'", "''") & "%'"
            End If
            If txtProvincia.Text <> "" Then
                strSql = strSql & " AND provincie.provincia LIKE '" & txtProvincia.Text.Replace("'", "''") & "%'"
            End If
            If txtComune.Text <> "" Then
                strSql = strSql & " AND comuni.denominazione LIKE '" & txtComune.Text.Replace("'", "''") & "%'"
            End If
            If ddlSegnalazioneSanzione.SelectedItem.Text <> "Tutti" Then
                strSql = strSql & " and isnull(attività.SegnalazioneSanzione,0) =" & ddlSegnalazioneSanzione.SelectedValue
            End If

            If txtAnno.Text <> "" Then
                Dim words As String = txtAnno.Text.Replace("'", "''")

                Try

                    Dim spezza() As String = Split(words, "/", 2)

                    Dim Mese() As Char = spezza(0)
                    Dim Anno() As Char = spezza(1)

                    Dim Mese_1_int As Integer = Integer.Parse(Mese(0))
                    Dim Mese_2_int As Integer = Integer.Parse(Mese(1))
                    Dim Anno_1_int As Integer = Integer.Parse(Anno(0))
                    Dim Anno_2_int As Integer = Integer.Parse(Anno(1))
                    Dim Anno_3_int As Integer = Integer.Parse(Anno(2))
                    Dim Anno_4_int As Integer = Integer.Parse(Anno(3))

                    strSql = strSql & " and year(attività.datainizioprevista) = '" & Anno & "' and MONTH(attività.datainizioprevista) = '" & Mese & "'"

                Catch ex As Exception
                    lblmessaggio.Text = "Inserire Inizio Richiesta nel formato mm/aaaa"
                    Response.Write(ex.Message)
                    Return

                End Try

            End If

            'FiltroVisibilita 01/12/2014
            strSql = strSql & " and TipiProgetto.MacroTipoProgetto like '" & Session("FiltroVisibilita") & "' "

            Dim strCondizione As String = "AND "
            If Cbogmo.SelectedIndex > 0 Then
                If Cbogmo.SelectedValue = 1 Then
                    strSql &= strCondizione & "Attività.GiovaniMinoriOpportunità = 'True'"
                    strCondizione = "AND "
                ElseIf Cbogmo.SelectedValue = 2 Then
                    strSql &= strCondizione & "Attività.GiovaniMinoriOpportunità = 'False'"
                    strCondizione = "AND "
                End If
            End If

            If Cbofami.SelectedIndex > 0 Then
                If Cbofami.SelectedValue = 1 Then
                    strSql &= strCondizione & "Attività.FAMI = 'True'"
                    strCondizione = "AND "
                ElseIf Cbofami.SelectedValue = 2 Then
                    strSql &= strCondizione & "Attività.FAMI = 'False'"
                    strCondizione = "AND "
                End If
            End If

            If CboEsteroUe.SelectedIndex > 0 Then
                If CboEsteroUe.SelectedValue = 1 Then
                    strSql &= strCondizione & "Attività.EsteroUE = 'True'"
                    strCondizione = "AND "
                ElseIf CboEsteroUe.SelectedValue = 2 Then
                    strSql &= strCondizione & "Attività.EsteroUE = 'False'"
                    strCondizione = "AND "
                End If
            End If

            If CboTutoraggio.SelectedIndex > 0 Then
                If CboTutoraggio.SelectedValue = 1 Then
                    strSql &= strCondizione & "Attività.Tutoraggio = 'True'"
                    strCondizione = "AND "
                ElseIf CboTutoraggio.SelectedValue = 2 Then
                    strSql &= strCondizione & "Attività.Tutoraggio = 'False'"
                    strCondizione = "AND "
                End If
            End If

            If CboDurataProg.SelectedIndex > 0 Then

                strSql &= strCondizione & "Attività.NMesi = " & CboDurataProg.SelectedValue
                strCondizione = "AND "

            End If

            strSql = strSql & " Order by enti.codiceregione "
            MyDataSet = ClsServer.DataSetGenerico(strSql, Session("conn"))
            dgRisultatoRicerca.Columns(20).Visible = ClsUtility.ForzaPresenzaSanzioneProgetto(Session("Utente"), Session("conn"))
            dgRisultatoRicerca.DataSource = MyDataSet
            Session("appDtsRisRicerca") = MyDataSet
            dgRisultatoRicerca.DataBind()
            CaricaDataTablePerStampa(MyDataSet)

            Select Case chkAttessaGraduatoria.Text
                Case "Progetti in attesa di Presentazione Graduatoria"
                    dgRisultatoRicerca.Columns(5).Visible = False
                    dgRisultatoRicerca.Columns(6).Visible = True
                Case "Progetti in Attesa di Graduatoria"
                    dgRisultatoRicerca.Columns(5).Visible = False
                    dgRisultatoRicerca.Columns(6).Visible = False
                Case "Progetti in attesa di Conferma Graduatoria"
                    dgRisultatoRicerca.Columns(5).Visible = True
                    dgRisultatoRicerca.Columns(6).Visible = False
                    dgRisultatoRicerca.Columns(1).Visible = True
                    dgRisultatoRicerca.Columns(2).Visible = True
                    dgRisultatoRicerca.Columns(9).Visible = False
                    dgRisultatoRicerca.Columns(11).Visible = False
                    dgRisultatoRicerca.Columns(15).Visible = False
                    dgRisultatoRicerca.Columns(16).Visible = True
                    dgRisultatoRicerca.Columns(17).Visible = True
            End Select

            'se sto tornando indietro dalla maschera delle graduatorie
            'passo la pagina della datagrid che avevo in fase di ricerca
            If Request.QueryString("CheckIndietro") = "true" Then
                dgRisultatoRicerca.CurrentPageIndex = CInt(Request.QueryString("PaginaGrid"))
            End If

            If Session("TipoUtente") <> "E" Then
                Dim item As DataGridItem
                For Each item In dgRisultatoRicerca.Items
                    If item.Cells(19).Text = True Then
                        item.ForeColor = Drawing.Color.Red
                    End If
                Next
                '********************************************************************************
            End If
            If Session("TipoUtente") = "E" Then
                VerificaDataInizioAttivita()
            End If


            If dgRisultatoRicerca.Items.Count = 0 Then
                dgRisultatoRicerca.Caption = "Nessun dato estratto."

            Else
                dgRisultatoRicerca.Caption = "Risultato Ricerca Progetti"


                If Session("TipoUtente") = "U" Then
                    ApriCSV1.Visible = False
                    CmdEsporta.Visible = True
                End If

            End If



        Catch ex As Exception
            lblmessaggio.Text = "La ricerca richiesta ha generato troppi risultati. Si prega di ottimizzarla valorizzando ulteriori filtri."
            Response.Write(ex.Message)

        End Try



    End Sub

    Sub CaricaPaginaConDati()

        'se provengo da una ricerca
        If Request.QueryString("CheckIndietro") = "true" Then
            'passo alle txt i filtri utilizzati in ricerca
            txtCodProgetto.Text = Request.QueryString("CodiceProgetto")
            chkAttessaGraduatoria.Checked = IIf(Request.QueryString("InAttesa") = "true", True, False)
            cboSettore.SelectedValue = Request.QueryString("Settore")
            cbobando.SelectedValue = Request.QueryString("Bando")
            txtTitoloProgetto.Text = Request.QueryString("Progetto")
            txtcodiceEnte.Text = Request.QueryString("CodEnte")
            txtdenominazione.Text = Request.QueryString("Ente")
        End If

        Call CaricaGriglia()
    End Sub

    Private Sub CaricaComboSet()
        Dim MyDataset As DataSet
        Dim Strsql As String
        Try
            cboSettore.Items.Clear()
            Strsql = "Select '0' as idmacroambitoattività, '' as settore From macroambitiattività UNION select idmacroambitoattività, codifica + ' - ' + MacroAmbitoAttività as Settore from macroambitiattività"
            MyDataset = ClsServer.DataSetGenerico(Strsql, Session("conn"))
            cboSettore.DataSource = MyDataset
            cboSettore.DataTextField = "Settore"
            cboSettore.DataValueField = "idmacroambitoattività"
            cboSettore.DataBind()

            cbobando.Items.Clear()

            If Not dtrGenerico Is Nothing Then
                dtrGenerico.Close()
                dtrGenerico = Nothing
            End If

            Dim strQueryControlloLOTUS As String

            'Verifica se l'uetnte puo modificare il libretto postale
            strQueryControlloLOTUS = "SELECT AssociaUtenteGruppo.username, VociMenu.IdVoceMenu, VociMenu.VoceMenu, VociMenu.ImmaginePassiva, VociMenu.ImmagineAttiva, VociMenu.Link," & _
                     " VociMenu.IdVoceMenuPadre" & _
                     " FROM VociMenu" & _
                     " INNER JOIN 	AbilitazioniProfiliVociMenu ON VociMenu.IdVoceMenu = AbilitazioniProfiliVociMenu.IdVoceMenu" & _
                     " INNER JOIN	Profili ON AbilitazioniProfiliVociMenu.IdProfilo = Profili.IdProfilo"
            If Session("Read") <> "1" Then
                strQueryControlloLOTUS = strQueryControlloLOTUS & " INNER JOIN	AssociaUtenteGruppo ON Profili.IdProfilo = AssociaUtenteGruppo.IdProfilo "
            Else
                strQueryControlloLOTUS = strQueryControlloLOTUS & " INNER JOIN	AssociaUtenteGruppo ON Profili.IdProfilo = AssociaUtenteGruppo.IdProfiloRead "
            End If

            strQueryControlloLOTUS = strQueryControlloLOTUS & " LEFT JOIN 	RestrizioniUtenteVociMenu ON AssociaUtenteGruppo.UserName = RestrizioniUtenteVociMenu.UserName and RestrizioniUtenteVociMenu.IdVoceMenu=VociMenu.IdVoceMenu" & _
                     " WHERE VociMenu.descrizione = 'Forza Bando Lotus'" & _
                     " AND AssociaUtenteGruppo.username ='" & Session("Utente") & "' and (RestrizioniUtenteVociMenu.TipoRestrizione <> 0 or RestrizioniUtenteVociMenu.TipoRestrizione is null)"
            dtrGenerico = ClsServer.CreaDatareader(strQueryControlloLOTUS, Session("conn"))

            If dtrGenerico.HasRows = True Then

                Strsql = "Select '0' as idbando, '' as bandobreve,'9999' AS annobreve "
                Strsql = Strsql & " From bando "
                Strsql = Strsql & " where idstatoBando=1 AND NOT BANDOBREVE IS NULL "
                Strsql = Strsql & " union "
                Strsql = Strsql & " SELECT Bando.idBando,bando.bandobreve, bando.Annobreve "
                Strsql = Strsql & " FROM bando"
                Strsql = Strsql & " INNER JOIN AssociaBandoTipiProgetto abtp on abtp.idbando =  bando.idbando"
                Strsql = Strsql & " INNER JOIN TipiProgetto  tp on abtp.idtipoprogetto = tp.idtipoprogetto"
                Strsql = Strsql & " WHERE idstatoBando=1 AND NOT BANDOBREVE IS NULL AND LOTUS=1  "
                Strsql = Strsql & " AND tp.MacroTipoProgetto like '" & Session("FiltroVisibilita") & "'"
                Strsql = Strsql & " ORDER BY 3 desc"

                If Not dtrGenerico Is Nothing Then
                    dtrGenerico.Close()
                    dtrGenerico = Nothing
                End If
            Else

                Strsql = "Select '0' as idbando, '' as bandobreve,'9999' AS annobreve "
                Strsql = Strsql & " From bando "
                Strsql = Strsql & " where idstatoBando=1 AND NOT BANDOBREVE IS NULL "
                Strsql = Strsql & " union "
                Strsql = Strsql & " SELECT Bando.idBando,bando.bandobreve,bando.Annobreve "
                Strsql = Strsql & " FROM bando"
                Strsql = Strsql & " INNER JOIN AssociaBandoTipiProgetto abtp on abtp.idbando =  bando.idbando"
                Strsql = Strsql & " INNER JOIN TipiProgetto  tp on abtp.idtipoprogetto = tp.idtipoprogetto"
                Strsql = Strsql & " WHERE idstatoBando=1 AND NOT BANDOBREVE IS NULL  "
                Strsql = Strsql & " AND tp.MacroTipoProgetto like '" & Session("FiltroVisibilita") & "'"
                Strsql = Strsql & " ORDER BY 3 desc "
            End If

            If Not dtrGenerico Is Nothing Then
                dtrGenerico.Close()
                dtrGenerico = Nothing
            End If

            MyDataset = ClsServer.DataSetGenerico(Strsql, Session("conn"))
            cbobando.DataSource = MyDataset
            cbobando.DataTextField = "BandoBreve"
            cbobando.DataValueField = "idbando"
            cbobando.DataBind()

        Catch ex As Exception
            PrintLine(ex.Message)
        Finally
            If Not dtrgenerico Is Nothing Then
                dtrgenerico.Close()
                dtrgenerico = Nothing
            End If
            If Not MyDataset Is Nothing Then
                MyDataset.Dispose()
                MyDataset = Nothing
            End If

        End Try

    End Sub

    Private Sub dgRisultatoRicerca_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgRisultatoRicerca.PageIndexChanged
        'AUTORE: TESTA GUIDO    DATA: 18/10/2004
        'utilizzo la session per memorizzare il dataset generato al momento della ricerca
        'dgRisultatoRicerca.SelectedIndex = -1

        'dgRisultatoRicerca.EditItemIndex = -1
        dgRisultatoRicerca.CurrentPageIndex = e.NewPageIndex
        dgRisultatoRicerca.DataSource = Session("appDtsRisRicerca")

        Try
            dgRisultatoRicerca.DataBind()
            dgRisultatoRicerca.Visible = True

            If Session("TipoUtente") = "E" Then
                VerificaDataInizioAttivita()
            End If

         

            If Session("TipoUtente") <> "E" Then
                Dim item As DataGridItem
                For Each item In dgRisultatoRicerca.Items
                    If item.Cells(19).Text = True Then
                        item.ForeColor = Drawing.Color.Red
                    End If
                Next
            End If

        Catch ex As Exception
            Try
                dgRisultatoRicerca.CurrentPageIndex = 0
                dgRisultatoRicerca.DataBind()
                dgRisultatoRicerca.Visible = True
            Catch ex2 As Exception
                lblmessaggio.Text = ex2.Message
                lblmessaggio.Visible = True
            End Try
        End Try


    End Sub

    'La data di inizio attività non deve essere visualizza se inferiore alla data odierna 
    Private Sub VerificaDataInizioAttivita()
        Dim item As DataGridItem
        Dim dataInizioAttività As Date
        For Each item In dgRisultatoRicerca.Items
            If (Date.TryParse(item.Cells(18).Text, dataInizioAttività)) Then
                If dataInizioAttività > Date.Now Then
                    item.Cells(18).Text = ""
                End If
            End If
        Next
    End Sub

    'routine che carica la datatable che caricherà dinamicamente la datagrid della stampa delle ricerche
    Sub CaricaDataTablePerStampa(ByVal DataSetDaScorrere As DataSet)
        Dim dt As New DataTable
        Dim dr As DataRow
        Dim i As Integer
        Dim x As Integer

        Dim NomeColonne(11) As String
        Dim NomiCampiColonne(11) As String

        'nome della colonna 
        'e posizione nella griglia di lettura
        NomeColonne(0) = "Cod.Ente"
        NomeColonne(1) = "Ente"
        NomeColonne(2) = "Titolo"
        NomeColonne(3) = "Bando"
        NomeColonne(4) = "Nr.Sedi Att."
        NomeColonne(5) = "Graduatorie Da Confermare"
        NomeColonne(6) = "N.Vol Ric"
        NomeColonne(7) = "N.Vol Sel"
        NomeColonne(8) = "N.Vol Part"
        NomeColonne(9) = "Data Invio Graduatoria"
        NomeColonne(10) = "Data Inizio Richiesta"
        NomeColonne(11) = "Data Inizio Progetto"
        ' NomeColonne(12) = "Presenza Sanzione"

        NomiCampiColonne(0) = "Codiceregione"
        NomiCampiColonne(1) = "ente"
        NomiCampiColonne(2) = "Titolo"
        NomiCampiColonne(3) = "Bando"
        NomiCampiColonne(4) = "NumSediAtt"
        NomiCampiColonne(5) = "NsedidaConfrmare"
        NomiCampiColonne(6) = "NumVolRic"
        NomiCampiColonne(7) = "NumVolAmm"
        NomiCampiColonne(8) = "NumVolIns"
        NomiCampiColonne(9) = "datainvio"
        NomiCampiColonne(10) = "datainizioprevista"
        NomiCampiColonne(11) = "datainizioattività"
        'NomiCampiColonne(12) = "SegnalazioneSanzione"


        'carico i nomi delle colonne che andrò a stampare nella datagrid
        For x = 0 To 11
            dt.Columns.Add(New DataColumn(NomeColonne(x), GetType(String)))
        Next

        'carico il datatable con il risultato della query della ricerca, in qusto caso delle risorse
        If DataSetDaScorrere.Tables(0).Rows.Count > 0 Then
            For i = 1 To DataSetDaScorrere.Tables(0).Rows.Count
                dr = dt.NewRow()
                For x = 0 To 11
                    dr(x) = DataSetDaScorrere.Tables(0).Rows.Item(i - 1).Item(NomiCampiColonne(x))
                Next
                dt.Rows.Add(dr)
            Next
        End If

        'passo alla sessione la datatable che ho appena creato e che userò per il databinding della datagrid della stampa
        Session("DtbRicerca") = dt

    End Sub

    Private Sub cmdesporta_click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdEsporta.Click
        CmdEsporta.Visible = False
        Dim dtbricerca As DataTable = Session("DtbRicerca")
        stampacsv(dtbricerca)
    End Sub

    Private Sub StampaCSV(ByVal dtbRicerca As DataTable)
        Dim path As String
        Dim xPrefissoNome As String
        Dim url As String
        Dim utility As ClsUtility = New ClsUtility()

        'If dtbRicerca.Rows.Count = 0 Then
        'ApriCSV1.Visible = False
        'CmdEsporta.Visible = False
        'Else
        'CmdEsporta.Visible = True
        xPrefissoNome = Session("Utente")
        path = Server.MapPath("download")
        url = CreaFileCSV(dtbRicerca, xPrefissoNome, path)
        ApriCSV1.Visible = True
        ApriCSV1.NavigateUrl = url
        'End If

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

End Class