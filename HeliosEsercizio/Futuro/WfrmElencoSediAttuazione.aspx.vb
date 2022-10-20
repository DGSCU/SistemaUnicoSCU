Public Class WfrmElencoSediAttuazione
    Inherits System.Web.UI.Page

    Dim strsql As String
    Protected WithEvents lblProgetto As System.Web.UI.WebControls.Label
    Protected WithEvents txtprova As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtIdAttivita As HtmlInputHidden
    Protected WithEvents Image1 As System.Web.UI.WebControls.Image
    Protected WithEvents imgCancellaGrad As System.Web.UI.WebControls.Button
    Protected WithEvents lblmessaggiosopra As System.Web.UI.WebControls.Label
    Dim MyDataSet As DataSet
    Protected WithEvents cmdFascCanc As System.Web.UI.WebControls.ImageButton
    Protected WithEvents cmdSelProtocollo0 As System.Web.UI.WebControls.ImageButton
    Protected WithEvents cmdSelFascicolo As System.Web.UI.WebControls.ImageButton
    Protected WithEvents TxtCodiceFascicolo As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtDescFasc As System.Web.UI.WebControls.TextBox
    Protected WithEvents ImgSalva As System.Web.UI.WebControls.Button
    Protected WithEvents TxtCodiceFasc As HtmlInputHidden
    Protected WithEvents LblNumFascicolo As System.Web.UI.WebControls.Label
    Protected WithEvents LblDescrFascicolo As System.Web.UI.WebControls.Label
    Dim blnRicerca As Boolean
    Protected WithEvents dtgElencoProt As System.Web.UI.WebControls.DataGrid
    Protected WithEvents LblNumProt As System.Web.UI.WebControls.Label
    Protected WithEvents txtNumProt As System.Web.UI.WebControls.TextBox
    Protected WithEvents ImgSellProtollo As System.Web.UI.WebControls.ImageButton
    Protected WithEvents LblDataProt As System.Web.UI.WebControls.Label
    Protected WithEvents txtDataProt As System.Web.UI.WebControls.TextBox
    Protected WithEvents imgSalvaProt As System.Web.UI.WebControls.Button
    Protected WithEvents dgRisultatoRicerca As System.Web.UI.WebControls.DataGrid
    Protected WithEvents lblmessaggio As System.Web.UI.WebControls.Label
    Public blnForza As Boolean

    Dim dtrGenerico As SqlClient.SqlDataReader
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load
        'carica l'elenco delle sedi di attuazioni relativamente all'attività selezionata
        If Not Session("LogIn") Is Nothing Then
            If Session("LogIn") = False Then 'verifico validità log-in
                Response.Redirect("LogOn.aspx")
            End If
        Else
            Response.Redirect("LogOn.aspx")
        End If
        AbilitaFascicolo_RigheNascoste()
        AbilitaProtocolliAssociati_RigheNascoste()
        AbilitaDivUtenteUNSC_RigheNascoste()

        If Request.QueryString("IdAttivita") <> "" Then
            'IDENTE  Session("IdEnte")
            'CODICEENTE  NZ Session("codiceregione")  or Session("txtCodEnte")

            Dim strATTIVITA As Integer = -1
            Dim strBANDOATTIVITA As Integer = -1
            Dim strENTEPERSONALE As Integer = -1
            Dim strENTITA As Integer = -1
            Dim strIDENTE As Integer = -1

            If ClsUtility.SICUREZZA_VERIFICA_AUTORIZZAZIONI(Session("conn"), Session("IdEnte"), Session("txtCodEnte"), Request.QueryString("IdAttivita"), strBANDOATTIVITA, strENTEPERSONALE, strENTITA, strIDENTE) = 1 Then

                If Not dtrGenerico Is Nothing Then
                    dtrGenerico.Close()
                    dtrGenerico = Nothing
                End If
            Else
                If Not dtrGenerico Is Nothing Then
                    dtrGenerico.Close()
                    dtrGenerico = Nothing
                End If
                Response.Redirect("wfrmAnomaliaDati.aspx")

            End If

        End If


        If IsPostBack = False Then
            txtIdAttivita.Value = Request.Params("IdAttivita")
            'titolo progetto
            strsql = "Select Titolo from attività where idattività=" & Request.Params("IdAttivita") & ""
            If Not dtrGenerico Is Nothing Then
                dtrGenerico.Close()
                dtrGenerico = Nothing
            End If
            dtrGenerico = ClsServer.CreaDatareader(strsql, Session("conn"))
            If dtrGenerico.HasRows = True Then
                dtrGenerico.Read()
                lblProgetto.Text = dtrGenerico("Titolo")
            End If
            If Not dtrGenerico Is Nothing Then
                dtrGenerico.Close()
                dtrGenerico = Nothing
            End If
            'Modificato da Alessandra Taballione il 25/10/204
            CaricaDataGrid()
            Select Case Request.QueryString("Azione")
                Case "Presentazione"
                    dgRisultatoRicerca.Columns(13).Visible = True
                    dgRisultatoRicerca.Columns(15).Visible = False
                Case "Conferma"
                    dgRisultatoRicerca.Columns(13).Visible = False
                    dgRisultatoRicerca.Columns(9).Visible = False
                    dgRisultatoRicerca.Columns(15).Visible = True
                Case "Inserimento"
                    dgRisultatoRicerca.Columns(13).Visible = False
                    dgRisultatoRicerca.Columns(15).Visible = False
                    'Aggiunto da ALessandra Taballione il 27/05/2005
                    'Menu di sola visualizzazione graduatoria
                Case "Visualizzazione"
                    dgRisultatoRicerca.Columns(13).Visible = False
                    dgRisultatoRicerca.Columns(15).Visible = False
                    dgRisultatoRicerca.Columns(9).Visible = True
                    dgRisultatoRicerca.Columns(9).HeaderText = "Visualizzazione Graduatoria"
            End Select
            'Abilito il pulsante per cancellare la graduatoria solo se l'utente ha le abilitazioni
            If Not BandoDol() Then
                Call AbilitaPulsanteCancella()
            Else
                imgCancellaGrad.Visible = False
            End If


            Call AbilitaPulsantiProroghe()



            'Mauro fascicolo
            If Not dtrGenerico Is Nothing Then
                dtrGenerico.Close()
                dtrGenerico = Nothing
            End If

            strsql = "Select CodiceFascicoloAI,IDFascicoloAI,DescrFascicoloAI  from enti where idente=" & Session("IdEnte") & ""
            dtrGenerico = ClsServer.CreaDatareader(strsql, Session("conn"))

            If dtrGenerico.HasRows = True Then
                dtrGenerico.Read()

                If IsDBNull(dtrGenerico("CodiceFascicoloAI")) = False Then
                    If dtrGenerico("CodiceFascicoloAI") <> "" Then
                        TxtCodiceFascicolo.Text = dtrGenerico("CodiceFascicoloAI")
                        TxtCodiceFasc.Value = dtrGenerico("IDFascicoloAI")
                        txtDescFasc.Text = dtrGenerico("DescrFascicoloAI")
                    End If
                End If
            End If
            If Not dtrGenerico Is Nothing Then
                dtrGenerico.Close()
                dtrGenerico = Nothing
            End If
            'abilito i campi del fascicolo solo per l'UNSC
            If Session("TipoUtente") = "U" Then
                LblNumFascicolo.Visible = True
                TxtCodiceFascicolo.Visible = True
                cmdSelFascicolo.Visible = True
                cmdSelProtocollo0.Visible = True
                cmdFascCanc.Visible = True
                LblDescrFascicolo.Visible = True
                txtDescFasc.Visible = True
                ImgSalva.Visible = True


                If ClsUtility.ForzaFascicoloInformaticoVolontari(Session("Utente"), Session("conn")) = True Then
                    'abilito visualizzazione e inserimento protocolli 
                    dtgElencoProt.Visible = True
                    LblNumProt.Visible = True
                    txtNumProt.Visible = True
                    ImgSellProtollo.Visible = True
                    LblDataProt.Visible = True
                    txtDataProt.Visible = True
                    imgSalvaProt.Visible = True
                    CaricaProtocolli(Request.Params("IdAttivita"))

                End If
            End If
        End If

        blnForza = ClsUtility.ForzaPresenzaSanzione(Session("Utente"), Session("conn"))
        If blnForza = True Then
            ddlSegnalazioneSanzione.Visible = True
            IdlblSanzione.Visible = True
        Else
            ddlSegnalazioneSanzione.Visible = False
            IdlblSanzione.Visible = False
        End If
        ' I pulsanti ProrogaEnte,ProrogaProgetto e imgCancellaGrad(Cancella graduatoria) devono utilizzare
        'l meccanismo di postback di ASP.NET
        ProrogaEnte.UseSubmitBehavior() = False
        ProrogaProgetto.UseSubmitBehavior() = False
        imgCancellaGrad.UseSubmitBehavior() = False
    End Sub
    Private Sub cmdChiudi_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CmdChiudi.Click
        Response.Redirect("WfrmRicercaAttivitaGraduatoria.aspx?CheckIndietro=" & Request.QueryString("CheckIndietro") & "&Ente=" & Request.QueryString("Ente") & "&CodEnte=" & Request.QueryString("CodEnte") & "&Progetto=" & Request.QueryString("Progetto") & "&Bando=" & Request.QueryString("Bando") & "&Settore=" & Request.QueryString("Settore") & "&Area=" & Request.QueryString("Area") & "&InAttesa=" & Request.QueryString("InAttesa") & "&CodiceProgetto=" & Request.QueryString("CodiceProgetto") & "&PaginaGrid=" & Request.QueryString("PaginaGrid") & "&Vengoda=" & Request.QueryString("Azione") & "")
    End Sub
    Private Sub dgRisultatoRicerca_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgRisultatoRicerca.PageIndexChanged
        dgRisultatoRicerca.SelectedIndex = -1
        dgRisultatoRicerca.EditItemIndex = -1
        dgRisultatoRicerca.CurrentPageIndex = e.NewPageIndex
        CaricaDataGrid()
    End Sub
    Private Sub CaricaDataGrid()
        If Not dtrGenerico Is Nothing Then
            dtrGenerico.Close()
            dtrGenerico = Nothing
        End If

        strsql = "SELECT '<img src=""images/home3.gif"" style=""width:20px;height:20px;border:0px""/>' as img," & _
               " case isnull(attivitàentisediattuazione.idstatoriattivazione,0) when 0 then entisedi.Denominazione else entisedi.Denominazione + ' (presenti info riattivazione)' end AS SedeAssegnazione,  " & _
               " entisedi.Indirizzo + '  ' + isnull(entisedi.Civico,'') AS indirizzo,  " & _
               " comuni.Denominazione AS comune, " & _
               " (SELECT case ISNULL(AESA.numeropostigmo,0) " & _
               " WHEN 0 THEN  CONVERT(VARCHAR,ISNULL((AESA.NumeroPostiNoVittoNoAlloggio + AESA.NumeroPostiVittoAlloggio + AESA.NumeroPostiVitto), 0)) " & _
               " ELSE  CONVERT(VARCHAR,ISNULL((AESA.NumeroPostiNoVittoNoAlloggio + AESA.NumeroPostiVittoAlloggio + AESA.NumeroPostiVitto), 0))+ ' (GMO: ' + CONVERT(VARCHAR,AESA.numeropostigmo) + ')'  end " & _
               "FROM attivitàentisediattuazione AS AESA " & _
               "INNER JOIN entisediattuazioni as ESA ON AESA.IDEnteSedeAttuazione=ESA.IDEnteSedeAttuazione " & _
               "INNER JOIN entisedi AS ES ON ESA.identesede=ES.identesede " & _
               "WHERE AESA.idattività=AttivitàSediAssegnazione.IDAttività AND ES.IDENTESEDE = entisedi.IDEnteSede ) " & _
               "AS VOLPREVISTI, " & _
               " (SELECT COUNT(*) FROM graduatorieentità s1 " & _
               " WHERE s1.idattivitàsedeassegnazione = attivitàsediassegnazione.idattivitàsedeassegnazione) AS  NumVolIns, " & _
               " (SELECT COUNT(*) FROM graduatorieentità s1 INNER JOIN" & _
               " entità e1 ON s1.IdEntità = e1.IDEntità" & _
               " WHERE s1.idattivitàsedeassegnazione = attivitàsediassegnazione.idattivitàsedeassegnazione and stato=1 and ammesso=1 and (e1.idstatoentità = 1 or e1.idstatoentità = 3)) AS  NumVolido, " & _
               " attivitàsediassegnazione.IDAttività as IDAtti,  entisedi.IDEnteSede as IdEnteSede, "
        If Request.QueryString("Azione") = "Presentazione" Then
            'Presentazione
            strsql = strsql & " '<img src=""images/lenteIngrandimento_small.png"" " & _
                  " onclick=""JavaScript:Associa(' + convert(varchar,attivitàsediassegnazione.IDEnteSede) +',' + convert(varchar,attivitàsediassegnazione.IDAttivitàsedeAssegnazione) +',' + convert(varchar,attivitàsediassegnazione.IDAttività) +',2);""" & _
                   " style=""border:0px"" title=""Associa Volontari"" alt=""Associa Volontari"" />' as Associa, "
        End If
        If Request.QueryString("Azione") = "Inserimento" Then
            'Inserimento
            strsql = strsql & " '<img src=""images/lenteIngrandimento_small.png"" " & _
                   " onclick=""JavaScript:Associa(' + convert(varchar,attivitàsediassegnazione.IDEnteSede) +',' + convert(varchar,attivitàsediassegnazione.IDAttivitàsedeAssegnazione) +',' + convert(varchar,attivitàsediassegnazione.IDAttività) +',0);""" & _
                " style=""border:0px"" title=""Associa Volontari"" alt=""Associa Volontari""  />' as Associa, "
        End If
        'Visualizza
        If Request.QueryString("Azione") = "Visualizzazione" Then
            'Inserimento
            strsql = strsql & " '<img src=""images/lenteIngrandimento_small.png"" " & _
                   " onclick=""JavaScript:Associa(' + convert(varchar,attivitàsediassegnazione.IDEnteSede) +',' + convert(varchar,attivitàsediassegnazione.IDAttivitàsedeAssegnazione) +',' + convert(varchar,attivitàsediassegnazione.IDAttività) +',4);""" & _
                " style=""cursor:pointer;border:0px"" title=""Visualizza Volontari"" alt=""Visualizza Volontari""  />' as Associa, "
        End If
        If Request.QueryString("Azione") = "Conferma" Then
            'Conferma
            strsql = strsql & " '<img src=""images/lenteIngrandimento_small.png"" " & _
                   " onclick=""JavaScript:Associa(' + convert(varchar,attivitàsediassegnazione.IDEnteSede) +',' + convert(varchar,attivitàsediassegnazione.IDAttivitàsedeAssegnazione) +',' + convert(varchar,attivitàsediassegnazione.IDAttività) +',0);""" & _
                " style=""cursor:pointer;border:0px"" title=""Associa Volontari"" alt=""Associa Volontari""  />' as Associa, "
        End If
        'Conferma
        strsql = strsql & " '<img src=""images/lenteIngrandimento_small.png"" " & _
                " onclick=""JavaScript:Associa(' + convert(varchar,attivitàsediassegnazione.IDEnteSede) +',' + convert(varchar,attivitàsediassegnazione.IDAttivitàsedeAssegnazione) +',' + convert(varchar,attivitàsediassegnazione.IDAttività) +',3);""" & _
                " style=""cursor:pointer;border:0px""  title=""Associa Volontari"" alt=""Associa Volontari""  />' as Conferma, "

        strsql = strsql & " '<img src=""images/lenteIngrandimento_small.png"" " & _
               " OnClientClick=""JavaScript:Presenta(' + convert(varchar,attivitàsediassegnazione.IDEnteSede) +',' + convert(varchar,attivitàsediassegnazione.IDAttivitàsedeAssegnazione) +',' + convert(varchar,attivitàsediassegnazione.IDAttività) +',1);"" " & _
               " style=""border:0px""  title=""Presentazione Graduatoria"" alt=""Presentazione Graduatoria"" >' as Presenta, "

        If Session("TipoUtente") = "U" Then 'mostro date
            strsql = strsql & "case(AttivitàSediAssegnazione.statograduatoria) when 1 then 'Registrata' when 2 then'Presentata' when 3 then 'Confermata ' + dbo.formatodata(isnull(AttivitàSediAssegnazione.datainiziodifferita,attività.datainizioattività)) when 4 then 'Respinta' end  as statograduatoria, "
        Else
            strsql = strsql & "case(AttivitàSediAssegnazione.statograduatoria) when 1 then 'Registrata' when 2 then'Presentata' when 3 then 'Confermata' when 4 then 'Respinta' end  as statograduatoria, "
        End If

        strsql = strsql & " Case isnull(entisediattuazioni.Segnalazione,0) When 0 then 'No' When 1 then '<img src=""images/Anomalie.bmp"" onclick=""VisualizzaSanzione('+ convert(varchar, entisediattuazioni.IDEnteSedeAttuazione) + ','+ convert(varchar, entisedi.IDEnte) + ')"" style=""cursor:hand;border:0px"" title=""Sanzione"" alt =""Sanzione""  />' End as Segnalazione, " & _
              " Case isnull(entisediattuazioni.SedeSottopostaVerifica,0) When 0 then 'No' When '1' then 'Si' end as verifica " & _
              " FROM entisedi " & _
              "INNER JOIN  comuni ON entisedi.IDComune = comuni.IDComune " & _
              "INNER JOIN  AttivitàSediAssegnazione  ON entisedi.IDEnteSede = AttivitàSediAssegnazione.IDEnteSede " & _
              "INNER Join entisediattuazioni ON entisedi.identesede = entisediattuazioni.identesede " & _
              "INNER JOIN attivitàentisediattuazione ON entisediattuazioni.identesedeattuazione = attivitàentisediattuazione.identesedeattuazione  AND AttivitàSediAssegnazione.IDAttività = attivitàentisediattuazione.idattività " & _
              "INNER JOIN attività on attivitàsediassegnazione.idattività = attività.idattività " & _
              "WHERE AttivitàSediAssegnazione.IDAttività = " & txtIdAttivita.Value & " "
        If blnRicerca = True Then
            If Trim(TxTComune.Text) <> vbNullString Then
                strsql = strsql & " and Comuni.denominazione LIKE '" & Replace(TxTComune.Text, "'", "''") & "%'"
            End If

            If Trim(TxTIndirizzo.Text) <> vbNullString Then
                strsql = strsql & " and entisedi.Indirizzo LIKE '" & Replace(TxTIndirizzo.Text, "'", "''") & "%'"
            End If

            If Trim(TxTCodSedAtt.Text) <> vbNullString Then
                strsql = strsql & " and attivitàentisediattuazione.identesedeattuazione =" & Replace(TxTCodSedAtt.Text, "'", "''") & " "
            End If
            If ddlSegnalazioneSanzione.SelectedItem.Text <> "Tutti" Then
                strsql = strsql & " and isnull(entisediattuazioni.Segnalazione,0) =" & ddlSegnalazioneSanzione.SelectedValue
            End If
        End If
        strsql = strsql & "GROUP BY case isnull(attivitàentisediattuazione.idstatoriattivazione,0) when 0 then entisedi.Denominazione else entisedi.Denominazione + ' (presenti info riattivazione)' end ,  entisedi.Indirizzo + '  ' + isnull(entisedi.Civico,''), " & _
               "attivitàsediassegnazione.IDEnteSede, comuni.Denominazione,attivitàsediassegnazione.idattivitàsedeassegnazione, " & _
               "attivitàsediassegnazione.IDAttività, entisedi.IDEnteSede, AttivitàSediAssegnazione.statograduatoria, isnull(entisediattuazioni.Segnalazione,0),isnull(entisediattuazioni.SedeSottopostaVerifica,0) ,entisediattuazioni.IDEnteSedeAttuazione, entisedi.IDEnte " & _
               ",AttivitàSediAssegnazione.DataInizioDifferita, attività.DataInizioAttività"

        MyDataSet = ClsServer.DataSetGenerico(strsql, Session("conn"))

        'assegno il dataset alla griglia del risultato
        If Request.QueryString("Azione") = "Visualizzazione" Then
            dgRisultatoRicerca.Columns(9).HeaderText = "Visualizzazione Graduatoria"
        End If
        dgRisultatoRicerca.DataSource = MyDataSet
        Session("appDtsRisRicerca") = MyDataSet
        dgRisultatoRicerca.DataBind()

        dgRisultatoRicerca.Columns(16).Visible = ClsUtility.ForzaPresenzaSanzione(Session("Utente"), Session("conn"))
        dgRisultatoRicerca.Columns(17).Visible = ClsUtility.ForzaSedeSottopostaVerifica(Session("Utente"), Session("conn"))

       


        If Session("TipoUtente") <> "E" Then
            Dim item As DataGridItem
            If Request.QueryString("Segnalato") = True Then
                For Each item In dgRisultatoRicerca.Items
                    item.ForeColor = item.ForeColor.Red
                Next
            End If
        End If
        If Session("TipoUtente") = "U" Then
            Dim item As DataGridItem
            Dim blnAvvisaSede As Boolean
            For Each item In dgRisultatoRicerca.Items
                blnAvvisaSede = ClsUtility.ControlloRichiestaModificaSede(dgRisultatoRicerca.Items(item.ItemIndex).Cells(10).Text, Session("conn"))
                If blnAvvisaSede = True Then
                    item.ForeColor = item.ForeColor.Red
                End If
            Next
        End If

    End Sub

    Private Sub cmdRicerca_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdRicerca.Click
        ResetLabelMessaggi()
        blnRicerca = True
        dgRisultatoRicerca.CurrentPageIndex = 0
        CaricaDataGrid()
        blnRicerca = False
    End Sub

    Private Sub AbilitaPulsanteCancella()

        If Not dtrGenerico Is Nothing Then
            dtrGenerico.Close()
            dtrGenerico = Nothing
        End If

        strsql = "SELECT AssociaUtenteGruppo.username, VociMenu.IdVoceMenu, VociMenu.VoceMenu, VociMenu.ImmaginePassiva, VociMenu.ImmagineAttiva, VociMenu.Link," & _
                " VociMenu.IdVoceMenuPadre" & _
                " FROM VociMenu" & _
                " INNER JOIN 	AbilitazioniProfiliVociMenu ON VociMenu.IdVoceMenu = AbilitazioniProfiliVociMenu.IdVoceMenu" & _
                " INNER JOIN	Profili ON AbilitazioniProfiliVociMenu.IdProfilo = Profili.IdProfilo"
        If UCase(Me.TemplateSourceDirectory) <> "/HELIOSREAD" Then
            strsql = strsql & " INNER JOIN	AssociaUtenteGruppo ON Profili.IdProfilo = AssociaUtenteGruppo.IdProfilo "
        Else
            strsql = strsql & " INNER JOIN	AssociaUtenteGruppo ON Profili.IdProfilo = AssociaUtenteGruppo.IdProfiloRead "
        End If
        strsql = strsql & " LEFT JOIN 	RestrizioniUtenteVociMenu ON AssociaUtenteGruppo.UserName = RestrizioniUtenteVociMenu.UserName and RestrizioniUtenteVociMenu.IdVoceMenu=VociMenu.IdVoceMenu" & _
                " WHERE (VociMenu.descrizione = 'Forza Cancella Graduatoria')" & _
                " AND AssociaUtenteGruppo.username ='" & Session("Utente") & "' and (RestrizioniUtenteVociMenu.TipoRestrizione <> 0 or RestrizioniUtenteVociMenu.TipoRestrizione is null)"
        dtrGenerico = ClsServer.CreaDatareader(strsql, Session("Conn"))

        If dtrGenerico.HasRows = True Then
            imgCancellaGrad.Visible = True
        Else
            imgCancellaGrad.Visible = False
        End If


        If Not dtrGenerico Is Nothing Then
            dtrGenerico.Close()
            dtrGenerico = Nothing
        End If
    End Sub
    Private Function BandoDol() As Boolean

        Dim FlagDOL As Integer

        strsql = " SELECT convert(int,d.DOL) as DOL from attività b inner join bandiattività c on b.idbandoattività = c.idbandoattività inner join bando d on c.idbando = d.idbando where b.idattività=" & Request.Params("IdAttivita")

        If Not dtrGenerico Is Nothing Then
            dtrGenerico.Close()
            dtrGenerico = Nothing
        End If
        dtrGenerico = ClsServer.CreaDatareader(strsql, Session("conn"))
        dtrGenerico.Read()

        FlagDOL = dtrGenerico("DOL")
        If Not dtrGenerico Is Nothing Then
            dtrGenerico.Close()
            dtrGenerico = Nothing
        End If

        If FlagDOL = 0 Then
            Return False
        Else
            Return True
        End If

    End Function

    Private Sub AbilitaPulsantiProroghe()

        If Not dtrGenerico Is Nothing Then
            dtrGenerico.Close()
            dtrGenerico = Nothing
        End If

        strsql = "SELECT AssociaUtenteGruppo.username, VociMenu.IdVoceMenu, VociMenu.VoceMenu, VociMenu.ImmaginePassiva, VociMenu.ImmagineAttiva, VociMenu.Link," & _
                " VociMenu.IdVoceMenuPadre" & _
                " FROM VociMenu" & _
                " INNER JOIN 	AbilitazioniProfiliVociMenu ON VociMenu.IdVoceMenu = AbilitazioniProfiliVociMenu.IdVoceMenu" & _
                " INNER JOIN	Profili ON AbilitazioniProfiliVociMenu.IdProfilo = Profili.IdProfilo"
        If UCase(Me.TemplateSourceDirectory) <> "/HELIOSREAD" Then
            strsql = strsql & " INNER JOIN	AssociaUtenteGruppo ON Profili.IdProfilo = AssociaUtenteGruppo.IdProfilo "
        Else
            strsql = strsql & " INNER JOIN	AssociaUtenteGruppo ON Profili.IdProfilo = AssociaUtenteGruppo.IdProfiloRead "
        End If

        strsql = strsql & " LEFT JOIN 	RestrizioniUtenteVociMenu ON AssociaUtenteGruppo.UserName = RestrizioniUtenteVociMenu.UserName and RestrizioniUtenteVociMenu.IdVoceMenu=VociMenu.IdVoceMenu" & _
                " WHERE (VociMenu.descrizione = 'Forza Proroga Graduatorie')" & _
                " AND AssociaUtenteGruppo.username ='" & Session("Utente") & "' and (RestrizioniUtenteVociMenu.TipoRestrizione <> 0 or RestrizioniUtenteVociMenu.TipoRestrizione is null)"
        dtrGenerico = ClsServer.CreaDatareader(strsql, Session("Conn"))

        If dtrGenerico.HasRows = True Then
            ProrogaProgetto.Visible = True
            ProrogaEnte.Visible = True
        Else
            ProrogaProgetto.Visible = False
            ProrogaEnte.Visible = False
        End If

        If Not dtrGenerico Is Nothing Then
            dtrGenerico.Close()
            dtrGenerico = Nothing
        End If
    End Sub

    Private Sub ProrogaProgetto_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ProrogaProgetto.Click

        Dim eseguiOperazione As Boolean = Boolean.Parse(confermaOperazione.Value)
        If (eseguiOperazione) Then
            Dim strLocal As String
            Try
                Dim mycommand As New SqlClient.SqlCommand
                mycommand.Connection = Session("conn")
                strLocal = " update attività set DataProrogaGraduatorie= getDate(),UsernameProrogaGraduatorie='" & Session("Utente") & "' " & _
                       " where idattività = '" & Request.Params("IdAttivita") & "'"
                mycommand.CommandText = strLocal
                mycommand.ExecuteNonQuery()
                LblInfoSediGraduatoria.Text = "Progetto prorogato."
                confermaOperazione.Value = "False"
            Catch ex As Exception
                LblErroreSediGraduatoria.Text = "Errore imprevisto durante l'operazione di 'Proroga Progetto'. Contattare l'assistenza Helios/Futuro."
                Throw ex
            End Try
        End If


    End Sub



    Private Sub ProrogaEnte_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ProrogaEnte.Click
        Dim eseguiOperazione As Boolean = Boolean.Parse(confermaOperazione.Value)
        If (eseguiOperazione) Then
            Try
                Dim strLocal As String
                Dim mycommand As New SqlClient.SqlCommand
                mycommand.Connection = Session("conn")

                strLocal = " update Enti set DataProrogaGraduatorie= getDate(),UsernameProrogaGraduatorie='" & Session("Utente") & "' " & _
                           " where idEnte = '" & Session("IdEnte") & "'"
                mycommand.CommandText = strLocal
                mycommand.ExecuteNonQuery()
                LblInfoSediGraduatoria.Text = "Ente prorogato."
                confermaOperazione.Value = "False"
            Catch ex As Exception
                LblErroreSediGraduatoria.Text = "Errore imprevisto durante l'operazione di 'Proroga Ente'. Contattare l'assistenza Helios/Futuro."
                Throw ex
            End Try
        End If
    End Sub

    Private Sub ImgSalva_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ImgSalva.Click
        Call SalvaProtocollo()
    End Sub
    Private Sub SalvaProtocollo()

        Dim strLocal As String
        Dim dtrCancellazione As SqlClient.SqlDataReader
        Dim mycommand As New SqlClient.SqlCommand
        Dim mydatatable As New DataTable

        mycommand.Connection = Session("conn")
        If TxtCodiceFascicolo.Text = "" Then
            'cancella
            strLocal = "select IDAttivitàSedeAssegnazione from attività inner join attivitàsediassegnazione on " & _
            "attività.idattività = attivitàsediassegnazione.idattività where(attività.idattività = " & Request.QueryString("IdAttivita") & ")"
            Try
                mydatatable = ClsServer.CreaDataTable(strLocal, False, Session("conn"))

                Dim k As Int16

                For k = 0 To mydatatable.Rows.Count - 1
                    strLocal = "update cronologiaentidocumenti set dataprot =null,  nprot = null where idente = " & CInt(Session("IdEnte")) & _
                        " and tipodocumento = 2 and IDAttivitàSedeAssegnazione = " & mydatatable.Rows(k).Item("IDAttivitàSedeAssegnazione")

                    mycommand.CommandText = strLocal
                    mycommand.ExecuteNonQuery()
                Next

            Catch ex As Exception
                Response.Write(ex.Message.ToString())
            End Try
        End If

        strLocal = "Update enti  set codicefascicoloai ='" & TxtCodiceFascicolo.Text & "', idfascicoloai='" & TxtCodiceFasc.Value & _
        "', descrfascicoloai='" & txtDescFasc.Text & "' where idente = " & Session("IdEnte") & ""
        mycommand.CommandText = strLocal
        mycommand.ExecuteNonQuery()
    End Sub



    Private Sub cmdFascCanc_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdFascCanc.Click
        Dim eseguiOperazione As Boolean = Boolean.Parse(confermaOperazione.Value)
        If (eseguiOperazione) Then
            ResetLabelMessaggi()
            TxtCodiceFascicolo.Text = ""
            TxtCodiceFasc.Value = ""
            txtDescFasc.Text = ""
            Call SalvaProtocollo()
            confermaOperazione.Value = "False"
        End If

    End Sub
    Sub CaricaFascicolo(ByVal IdEntità As Integer)

        ' funzione che riporta in maschera i dati del fascicolo associato al volontario
        Dim StrSql As String
        Dim myDateset As DataSet
        If Not dtrGenerico Is Nothing Then
            dtrGenerico.Close()
            dtrGenerico = Nothing
        End If
        StrSql = "SELECT entità.CodiceFascicolo, entità.IDFascicolo, entità.DescrFascicolo " & _
                 "FROM entità " & _
                 "WHERE IDEntità = " & IdEntità
        dtrGenerico = ClsServer.CreaDatareader(StrSql, Session("conn"))
        If dtrGenerico.HasRows = True Then
            dtrGenerico.Read()
            TxtCodiceFascicolo.Text = "" & dtrGenerico("CodiceFascicolo")
        End If
        If Not dtrGenerico Is Nothing Then
            dtrGenerico.Close()
            dtrGenerico = Nothing
        End If
    End Sub

    Private Sub imgSalvaProt_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles imgSalvaProt.Click
        Dim eseguiOperazione As Boolean = Boolean.Parse(confermaOperazione.Value)
        If (eseguiOperazione) Then
            ResetLabelMessaggi()
            AssociaProtocolloGraduatoria(Request.QueryString("IdAttivita"), txtNumProt.Text, txtDataProt.Text, Session("Utente"))
            dtgElencoProt.CurrentPageIndex = 0
            CaricaProtocolli(Request.QueryString("IdAttivita"))
            confermaOperazione.Value = "False"
        End If

    End Sub
    Private Sub CaricaProtocolli(ByVal IdAttività As Integer)
        Dim strSql As String
        Dim dataSet As New DataSet

        strSql = " SELECT  idAttività,NProt, dbo.FormatoData(DataProt) AS DataProt " & _
                 " FROM VW_ProtocolliGraduatorie where idattività =" & IdAttività

        dataSet = ClsServer.DataSetGenerico(strSql, Session("Conn"))
        dtgElencoProt.DataSource = dataSet
        dtgElencoProt.DataBind()

    End Sub

    Private Function AssociaProtocolloGraduatoria(ByVal IdAttivita As Integer, ByVal NProt As String, ByVal DataProt As String, ByVal Username As String) As String

        Dim sqlCMD As New SqlClient.SqlCommand
        Dim strNomeStore As String = "[Sp_AssociaProtocolloGraduatoria]"

        Try
            sqlCMD = New SqlClient.SqlCommand(strNomeStore, CType(Session("conn"), SqlClient.SqlConnection))
            sqlCMD.CommandType = CommandType.StoredProcedure
            sqlCMD.Parameters.Add("@IdAttivita", SqlDbType.Int).Value = IdAttivita
            sqlCMD.Parameters.Add("@NProt", SqlDbType.VarChar).Value = NProt
            sqlCMD.Parameters.Add("@DataProt", SqlDbType.DateTime).Value = DataProt
            sqlCMD.Parameters.Add("@Username", SqlDbType.NVarChar).Value = Username

            Dim sparam1 As SqlClient.SqlParameter
            sparam1 = New SqlClient.SqlParameter
            sparam1.ParameterName = "@Esito"
            sparam1.Size = 100
            sparam1.SqlDbType = SqlDbType.NVarChar
            sparam1.Direction = ParameterDirection.Output
            sqlCMD.Parameters.Add(sparam1)

            sqlCMD.ExecuteScalar()
            Dim str As String
            str = sqlCMD.Parameters("@Esito").Value
            Return str
        Catch ex As Exception
            Response.Write(ex.Message.ToString())
            Exit Function
        End Try
    End Function

    Private Function DisassociaProtocolloGraduatoria(ByVal IdAttivita As Integer, ByVal NProt As String, ByVal DataProt As String, ByVal Username As String) As String

        Dim sqlCMD As New SqlClient.SqlCommand
        Dim strNomeStore As String = "[Sp_DisassociaProtocolloGraduatoria]"

        Try
            sqlCMD = New SqlClient.SqlCommand(strNomeStore, CType(Session("conn"), SqlClient.SqlConnection))
            sqlCMD.CommandType = CommandType.StoredProcedure
            sqlCMD.Parameters.Add("@IdAttivita", SqlDbType.Int).Value = IdAttivita
            sqlCMD.Parameters.Add("@NProt", SqlDbType.VarChar).Value = NProt
            sqlCMD.Parameters.Add("@DataProt", SqlDbType.DateTime).Value = DataProt
            sqlCMD.Parameters.Add("@Username", SqlDbType.NVarChar).Value = Username

            Dim sparam1 As SqlClient.SqlParameter
            sparam1 = New SqlClient.SqlParameter
            sparam1.ParameterName = "@Esito"
            sparam1.Size = 100
            sparam1.SqlDbType = SqlDbType.NVarChar
            sparam1.Direction = ParameterDirection.Output
            sqlCMD.Parameters.Add(sparam1)

            sqlCMD.ExecuteScalar()
            Dim str As String
            str = sqlCMD.Parameters("@Esito").Value
            Return str
        Catch ex As Exception
            Response.Write(ex.Message.ToString())
            Exit Function
        End Try
    End Function

    Private Sub dtgElencoProt_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dtgElencoProt.ItemCommand
        ResetLabelMessaggi()
        Dim strMsgReturn As String = ""
        If e.CommandName = "Cancella" Then

            strMsgReturn = DisassociaProtocolloGraduatoria(Request.QueryString("IdAttivita"), e.Item.Cells(1).Text, e.Item.Cells(2).Text, Session("Utente"))
            dtgElencoProt.CurrentPageIndex = 0
            CaricaProtocolli(Request.QueryString("IdAttivita"))

            If strMsgReturn <> "" Then
                If strMsgReturn = "OK" Then
                    lblmessaggio.Text = "Aggiornamento effettuato con successo."
                    CaricaProtocolli(Request.QueryString("IdAttivita"))
                Else
                    lblmessaggiosopra.Text = strMsgReturn
                End If
            End If
        End If
    End Sub

    Private Sub dtgElencoProt_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dtgElencoProt.PageIndexChanged
        dtgElencoProt.CurrentPageIndex = e.NewPageIndex
        CaricaProtocolli(Request.QueryString("IdAttivita"))
        dtgElencoProt.SelectedIndex = -1
    End Sub

    Private Sub dtgElencoProt_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dtgElencoProt.SelectedIndexChanged

    End Sub

    Protected Sub imgCancellaGrad_Click(ByVal sender As Object, ByVal e As EventArgs) Handles imgCancellaGrad.Click
        Dim eseguiOperazione As Boolean = Boolean.Parse(confermaOperazione.Value)
        If (eseguiOperazione) Then
            ResetLabelMessaggi()
            Dim sEsito As String
            Dim strMessaggio As String

            sEsito = ClsServer.EseguiStoreCancellaGraduatoria(CInt(txtIdAttivita.Value), 0, Session("conn"))

            If sEsito = "ERRORE" Then
                LblErroreSediGraduatoria.Text = "Errore imprevisto durante l'operazione di 'Eliminazione Graduatoria'. Contattare l'assistenza Helios/Futuro."
            ElseIf sEsito = "KO" Then
                LblErroreSediGraduatoria.Text = "Non si può eliminare una graduatoria con stato Confermato o Respinta."
            Else
                LblInfoSediGraduatoria.Text = "Graduatoria eliminata."
                Call CaricaDataGrid()
            End If
            confermaOperazione.Value = "False"
        End If

    End Sub

    Private Sub AbilitaProtocolliAssociati_RigheNascoste()

        If ClsUtility.ForzaFascicoloInformaticoVolontari(Session("Utente"), Session("conn")) = True Then
            ProtocolliAssociati_RigheNascoste.Visible = True
        Else
            ProtocolliAssociati_RigheNascoste.Visible = False
        End If

    End Sub

    Private Sub AbilitaFascicolo_RigheNascoste()
        'If Not dtrGenerico Is Nothing Then
        '    dtrGenerico.Close()
        '    dtrGenerico = Nothing
        'End If

        'strsql = "SELECT AssociaUtenteGruppo.username, VociMenu.IdVoceMenu, VociMenu.VoceMenu, VociMenu.ImmaginePassiva, VociMenu.ImmagineAttiva, VociMenu.Link," & _
        '        " VociMenu.IdVoceMenuPadre" & _
        '        " FROM VociMenu" & _
        '        " INNER JOIN 	AbilitazioniProfiliVociMenu ON VociMenu.IdVoceMenu = AbilitazioniProfiliVociMenu.IdVoceMenu" & _
        '        " INNER JOIN	Profili ON AbilitazioniProfiliVociMenu.IdProfilo = Profili.IdProfilo"
        'If UCase(Me.TemplateSourceDirectory) <> "/HELIOSREAD" Then
        '    strsql = strsql & " INNER JOIN	AssociaUtenteGruppo ON Profili.IdProfilo = AssociaUtenteGruppo.IdProfilo "
        'Else
        '    strsql = strsql & " INNER JOIN	AssociaUtenteGruppo ON Profili.IdProfilo = AssociaUtenteGruppo.IdProfiloRead "
        'End If

        'strsql = strsql & " LEFT JOIN 	RestrizioniUtenteVociMenu ON AssociaUtenteGruppo.UserName = RestrizioniUtenteVociMenu.UserName and RestrizioniUtenteVociMenu.IdVoceMenu=VociMenu.IdVoceMenu" & _
        '        " WHERE (VociMenu.descrizione = 'Forza Proroga Graduatorie')" & _
        '        " AND AssociaUtenteGruppo.username ='" & Session("Utente") & "' and (RestrizioniUtenteVociMenu.TipoRestrizione <> 0 or RestrizioniUtenteVociMenu.TipoRestrizione is null)"
        'dtrGenerico = ClsServer.CreaDatareader(strsql, Session("Conn"))

        'If dtrGenerico.HasRows = True Then
        '    Fascicolo_RigheNascoste.Visible = True
        'Else
        '    Fascicolo_RigheNascoste.Visible = False
        'End If

        If Session("TipoUtente") = "U" Then
            Fascicolo_RigheNascoste.Visible = True
        Else
            Fascicolo_RigheNascoste.Visible = False
        End If

        If Not dtrGenerico Is Nothing Then
            dtrGenerico.Close()
            dtrGenerico = Nothing
        End If
    End Sub

    Private Sub AbilitaDivUtenteUNSC_RigheNascoste()

        If (Fascicolo_RigheNascoste.Visible = True Or ProtocolliAssociati_RigheNascoste.Visible = True) Then
            DivUtenteUNSC_RigheNascoste.Visible = True
        Else
            DivUtenteUNSC_RigheNascoste.Visible = False
        End If
    End Sub

    Private Sub ResetLabelMessaggi()
        lblmessaggio.Text = ""
        lblmessaggiosopra.Text = ""
        LblErroreSediGraduatoria.Text = ""
        LblInfoSediGraduatoria.Text = ""
    End Sub
   

End Class