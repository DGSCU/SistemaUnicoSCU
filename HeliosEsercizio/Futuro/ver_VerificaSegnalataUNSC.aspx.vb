Imports System.IO
Imports System.Drawing


Public Class ver_VerificaSegnalataUNSC
    Inherits System.Web.UI.Page
    Dim strIdEnteSedeAttuazione As String
    Dim SIGED As clsSiged
#Region " Codice generato da Progettazione Web Form "

    'Chiamata richiesta da Progettazione Web Form.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    Protected WithEvents lblCodiceFascicolo As System.Web.UI.WebControls.Label




    Protected WithEvents imgInfoVerifiche As System.Web.UI.HtmlControls.HtmlImage

    Protected WithEvents TxtCodiceFascicolo As System.Web.UI.WebControls.TextBox


    Protected WithEvents LblNumProtocolloInvioLetteraChiusura As System.Web.UI.WebControls.Label
    Protected WithEvents TxtNumProtocolloInvioLetteraChiusura As System.Web.UI.WebControls.TextBox

    Protected WithEvents LblDataProtTrasDG As System.Web.UI.WebControls.Label
    '' Protected WithEvents TxtDataProtTrasDG As System.Web.UI.WebControls.TextBox
    Protected WithEvents LblNumProtTrasDG As System.Web.UI.WebControls.Label
    '' Protected WithEvents TxtNumProtTrasDG As System.Web.UI.WebControls.TextBox
    Protected WithEvents lblCodiceVerifica As System.Web.UI.WebControls.Label
    'Protected WithEvents lblDataIncarico As System.Web.UI.WebControls.Label
    Protected WithEvents TextBox3 As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextBox4 As System.Web.UI.WebControls.TextBox

    Protected WithEvents LblStampaCredenzialiIncarico As System.Web.UI.WebControls.Label
    Protected WithEvents LblStampaLettereIncarico As System.Web.UI.WebControls.Label

    Protected WithEvents CmdStampaCredenzialiIncarico As System.Web.UI.WebControls.ImageButton
    Protected WithEvents CmdStampaLettereIncarico As System.Web.UI.WebControls.ImageButton
    Protected WithEvents cmdRelazioneStand As System.Web.UI.WebControls.ImageButton
    Protected WithEvents cmdTrasmissione As System.Web.UI.WebControls.ImageButton
    Protected WithEvents CmdConclusione As System.Web.UI.WebControls.ImageButton
    Protected WithEvents CmdIrregolarita As System.Web.UI.WebControls.ImageButton
    Protected WithEvents cmdContestazione As System.Web.UI.WebControls.ImageButton
    Protected WithEvents CmdLetteraTrasm As System.Web.UI.WebControls.ImageButton
    Protected WithEvents cmdDiffida As System.Web.UI.WebControls.ImageButton
    Protected WithEvents CmdLetteraTrasmissioneProvv As System.Web.UI.WebControls.ImageButton
    Protected WithEvents CmdTrasmRich As System.Web.UI.WebControls.ImageButton
    Protected WithEvents CmdLETTERATRASMISPROVAISERVIZI As System.Web.UI.WebControls.ImageButton
    Protected WithEvents CmdTRASMISSIONEPROVVEDIMENTOREGIONE As System.Web.UI.WebControls.ImageButton
    Protected WithEvents CmdArchiviazione As System.Web.UI.WebControls.ImageButton
    Protected WithEvents CmdRevoca As System.Web.UI.WebControls.ImageButton
    Protected WithEvents CmdCancellazione As System.Web.UI.WebControls.ImageButton
    Protected WithEvents CmdInterdizione As System.Web.UI.WebControls.ImageButton
    Protected WithEvents CmdStampaLettereIncaricoIGF As System.Web.UI.WebControls.ImageButton
    Protected WithEvents CmdStampaCredenzialiIncaricoIGF As System.Web.UI.WebControls.ImageButton


    '' Protected WithEvents txtDataProtCredenziali As System.Web.UI.WebControls.TextBox
    '' Protected WithEvents txtNumProtCredenziali As System.Web.UI.WebControls.TextBox

    Protected WithEvents LblDataProtocolloInvioLetteraContestazione As System.Web.UI.WebControls.Label
    Protected WithEvents txtDataProtocolloInvioLetteraContestazione As System.Web.UI.WebControls.TextBox

    Protected WithEvents LblDataProtSegnalazione As System.Web.UI.WebControls.Label
    Protected WithEvents txtDataProtSegnalazione As System.Web.UI.WebControls.TextBox
    Protected WithEvents lblNumProtSegnalazione As System.Web.UI.WebControls.Label
    Protected WithEvents txtNumProtSegnalazione As System.Web.UI.WebControls.TextBox
    Protected WithEvents lblDataProtRicSegnalazione As System.Web.UI.WebControls.Label
    Protected WithEvents txtDataProtRicSegnalazione As System.Web.UI.WebControls.TextBox
    Protected WithEvents lblNumProtRicSegnalazione As System.Web.UI.WebControls.Label
    Protected WithEvents TxtNumProtRicSegnalazione As System.Web.UI.WebControls.TextBox

    Protected WithEvents lblDataProtApprovazione As System.Web.UI.WebControls.Label
    Protected WithEvents TxtDataProtApprovazione As System.Web.UI.WebControls.TextBox
    Protected WithEvents lblNumProtApprovazione As System.Web.UI.WebControls.Label
    Protected WithEvents TxtNumProtApprovazione As System.Web.UI.WebControls.TextBox

    Protected WithEvents LblProgetto As System.Web.UI.WebControls.Label
    ' Protected WithEvents txtIdAttivita As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtProgetto As System.Web.UI.WebControls.TextBox
    Protected WithEvents LblSede As System.Web.UI.WebControls.Label
    ''  Protected WithEvents txtIdEnteSedeAttuazione As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtSede As System.Web.UI.WebControls.TextBox
    Protected WithEvents LblEnte As System.Web.UI.WebControls.Label
    Protected WithEvents txtEnte As System.Web.UI.WebControls.TextBox
    '''Protected WithEvents txtIdEnte As System.Web.UI.WebControls.TextBox
    Protected WithEvents LblEnteFiglio As System.Web.UI.WebControls.Label
    Protected WithEvents TxtEnteFiglio As System.Web.UI.WebControls.TextBox


    Protected WithEvents imgRicercaEnte As System.Web.UI.WebControls.ImageButton
    Protected WithEvents ImgRicercaProgetto As System.Web.UI.WebControls.ImageButton
    Protected WithEvents imgRicercaSede As System.Web.UI.WebControls.ImageButton


    Protected WithEvents LblDataProtocolloInvioChiusura As System.Web.UI.WebControls.Label
    Protected WithEvents txtDataProtocolloInvioChiusura As System.Web.UI.WebControls.TextBox
    Protected WithEvents LblNumeroProtocolloInvioLettContestazione As System.Web.UI.WebControls.Label
    Protected WithEvents TxtNumeroProtocolloInvioLettContestazione As System.Web.UI.WebControls.TextBox
    Protected WithEvents LblDataRicezioneLetteraContestazione As System.Web.UI.WebControls.Label
    Protected WithEvents txtDataRicezioneLetteraContestazione As System.Web.UI.WebControls.TextBox
    Protected WithEvents LblDataRispostaLetteraContestazione As System.Web.UI.WebControls.Label
    Protected WithEvents txtDataRispostaLetteraContestazione As System.Web.UI.WebControls.TextBox
    Protected WithEvents LblDataProtocolloRispostaContestazione As System.Web.UI.WebControls.Label
    Protected WithEvents txtDataProtocolloRispostaContestazione As System.Web.UI.WebControls.TextBox
    Protected WithEvents LblNumProtocolloRispostaContestazione As System.Web.UI.WebControls.Label
    Protected WithEvents TxtNumProtocolloRispostaContestazione As System.Web.UI.WebControls.TextBox

    Protected WithEvents LblDataProtocolloTrasmissioneSanzione As System.Web.UI.WebControls.Label
    Protected WithEvents txtDataProtocolloTrasmissioneSanzione As System.Web.UI.WebControls.TextBox
    Protected WithEvents LblNumeroProtocolloTrasmissioneSanzione As System.Web.UI.WebControls.Label
    Protected WithEvents TxtNumeroProtocolloTrasmissioneSanzione As System.Web.UI.WebControls.TextBox
    Protected WithEvents LblDataEsecuzioneSanzione As System.Web.UI.WebControls.Label
    Protected WithEvents txtDataEsecuzioneSanzione As System.Web.UI.WebControls.TextBox
    Protected WithEvents LblDataProtocolloEsecuzioneSanzione As System.Web.UI.WebControls.Label
    Protected WithEvents txtDataProtocolloEsecuzioneSanzione As System.Web.UI.WebControls.TextBox
    Protected WithEvents LblNumeroProtocolloEsecuzioneSanzione As System.Web.UI.WebControls.Label
    Protected WithEvents TxtNumeroProtocolloEsecuzioneSanzione As System.Web.UI.WebControls.TextBox

    Protected WithEvents ddlServizi As System.Web.UI.WebControls.DropDownList
    Protected WithEvents txtServizi As System.Web.UI.WebControls.TextBox

    Protected WithEvents LblUfficioUnsc As System.Web.UI.WebControls.Label
    'Protected WithEvents DropDownList1 As System.Web.UI.WebControls.DropDownList
    Protected WithEvents LblCompetenza As System.Web.UI.WebControls.Label
    Protected WithEvents ddlUfficio As System.Web.UI.WebControls.DropDownList
    Protected WithEvents ddlCompetenze As System.Web.UI.WebControls.DropDownList
    Protected WithEvents lblSanzione As System.Web.UI.WebControls.Label


    Protected WithEvents lblIspettoreSupporto As System.Web.UI.WebControls.Label
    Protected WithEvents LblDataLetteraContestazioneDG As System.Web.UI.WebControls.Label
    Protected WithEvents txtDataProtLetteraContestazioneDG As System.Web.UI.WebControls.TextBox
    Protected WithEvents LblNProtLetteraContestazioneDG As System.Web.UI.WebControls.Label
    Protected WithEvents txtNProtLetteraContestazioneDG As System.Web.UI.WebControls.TextBox
    Protected WithEvents LblDataProtChiusuraContestazione As System.Web.UI.WebControls.Label
    Protected WithEvents TxtDataProtChiusuraContestazione As System.Web.UI.WebControls.TextBox
    Protected WithEvents LblNumProtChiusuraContestazione As System.Web.UI.WebControls.Label
    Protected WithEvents TxtNumProtChiusuraContestazione As System.Web.UI.WebControls.TextBox
    Protected WithEvents CmdLetteraChiusContestazione As System.Web.UI.WebControls.ImageButton

    Protected WithEvents LblDataProtocolloTrasmServizi As System.Web.UI.WebControls.Label
    Protected WithEvents txtDataProtocolloTrasmServizi As System.Web.UI.WebControls.TextBox
    Protected WithEvents LblNProtocolloTrasmServizi As System.Web.UI.WebControls.Label
    Protected WithEvents txtNProtocolloTrasmServizi As System.Web.UI.WebControls.TextBox

    Protected WithEvents cmdSelFascicolo As System.Web.UI.WebControls.ImageButton
    Protected WithEvents cmdSelProtocollo0 As System.Web.UI.WebControls.ImageButton
    Protected WithEvents cmdFascCanc As System.Web.UI.WebControls.ImageButton

    'CREDENZIALI
    Protected WithEvents ImgProtolloCredenziali As System.Web.UI.WebControls.Image
    Protected WithEvents ImgApriAllegatiCredenziali As System.Web.UI.WebControls.Image
    Protected WithEvents ImgProtocollazioneCredenziali As System.Web.UI.WebControls.Image
    'PROTOCOLLO INCARICO
    Protected WithEvents cmdSc1SelProtocollo1 As System.Web.UI.WebControls.Image
    Protected WithEvents cmdSc1Allegati1 As System.Web.UI.WebControls.Image
    Protected WithEvents cmdNuovoFascioclo As System.Web.UI.WebControls.Image
    'TRASMISSIONE AL DG
    Protected WithEvents ImgProtocolloTrasDG As System.Web.UI.WebControls.Image
    Protected WithEvents ImgApriAllegatiTrasDG As System.Web.UI.WebControls.Image
    Protected WithEvents ImgProtocollazioneTrasDG As System.Web.UI.WebControls.Image
    'LETTERA CHIUSURA AL DG
    Protected WithEvents ImgProtocolloTrasContestataDG As System.Web.UI.WebControls.Image
    Protected WithEvents ImgApriAllegatiTrasContestataDG As System.Web.UI.WebControls.Image
    Protected WithEvents ImgProtocollazioneTrasContestataDG As System.Web.UI.WebControls.Image
    'LETTERA CHIUSURA 
    Protected WithEvents ImgProtocolloLettChiusura As System.Web.UI.WebControls.Image
    Protected WithEvents ImgApriAllegatiLettChiusura As System.Web.UI.WebControls.Image
    Protected WithEvents ImgProtocollazioneLettChiusura As System.Web.UI.WebControls.Image
    'INVIO LETTERA CONTESTAZIONE 
    Protected WithEvents ImgProtocolloInvioLettContestazione As System.Web.UI.WebControls.Image
    Protected WithEvents ImgApriAllegatiInvioLettContestazione As System.Web.UI.WebControls.Image
    Protected WithEvents ImgProtocollazioneInvioLettContestazione As System.Web.UI.WebControls.Image
    'RISPOSTA LETTERA CONTESTAZIONE ( Controdeduzioni )
    Protected WithEvents ImgProtocolloRispContestazione As System.Web.UI.WebControls.Image
    Protected WithEvents ImgApriAllegatiRispContestazione As System.Web.UI.WebControls.Image
    Protected WithEvents ImgProtocollazioneRispContestazione As System.Web.UI.WebControls.Image
    'DATA PROTOCOLLO CHIUSURA LETTERA CONTESTAZIONE
    Protected WithEvents ImgProtocolloProtChiusuraContestazione As System.Web.UI.WebControls.Image
    Protected WithEvents ImgApriAllegatiProtChiusuraContestazione As System.Web.UI.WebControls.Image
    Protected WithEvents ImgProtocollazioneProtChiusuraContestazione As System.Web.UI.WebControls.Image
    'DATA PROTOCOLLO TRASMISSIONE SANZIONE
    Protected WithEvents ImgProtocolloTrasmissioneSanzione As System.Web.UI.WebControls.Image
    Protected WithEvents ImgApriAllegatiTrasmissioneSanzione As System.Web.UI.WebControls.Image
    Protected WithEvents ImgProtocollazioneTrasmissioneSanzione As System.Web.UI.WebControls.Image
    'DATA PROTOCOLLO TRASMISSIONE SANZIONE
    Protected WithEvents ImgProtocolloTrasmServizi As System.Web.UI.WebControls.Image
    Protected WithEvents ImgApriAllegatiTrasmServizi As System.Web.UI.WebControls.Image
    Protected WithEvents ImgProtocollazioneTrasmServizi As System.Web.UI.WebControls.Image
    'DATA PROTOCOLLO ESECUZIONE SANZIONE
    Protected WithEvents ImgProtocolloEsecuzioneSanzione As System.Web.UI.WebControls.Image
    Protected WithEvents ImgApriAllegatiEsecuzioneSanzione As System.Web.UI.WebControls.Image
    Protected WithEvents ImgProtocollazioneEsecuzioneSanzione As System.Web.UI.WebControls.Image
    'sanzione 
    Protected WithEvents cmdSanzione As System.Web.UI.WebControls.ImageButton
    Protected WithEvents imgScanner As System.Web.UI.WebControls.Button
    Protected WithEvents cmdStampa As System.Web.UI.WebControls.Button
    Protected WithEvents CmdSospendi As System.Web.UI.WebControls.Button
    Protected WithEvents cmdSalva As System.Web.UI.WebControls.Button
    Protected WithEvents cmdChiudi As System.Web.UI.WebControls.Button

    Protected WithEvents imgSanzioneServizi As System.Web.UI.WebControls.ImageButton
    Protected WithEvents lblmessaggio As System.Web.UI.WebControls.Label
    Protected WithEvents LblNote As System.Web.UI.WebControls.Label
    Protected WithEvents TxtNote As System.Web.UI.WebControls.TextBox

    Protected WithEvents txtIdVerifica As System.Web.UI.WebControls.TextBox
    Protected WithEvents cmdChiusaContestata As System.Web.UI.WebControls.Button
    Protected WithEvents LblDescrStatoVerifica As System.Web.UI.WebControls.Label
    Protected WithEvents Label1 As System.Web.UI.WebControls.Label
    Protected WithEvents LblStatoVerifica As System.Web.UI.WebControls.Label
    'Protected WithEvents Lbl As System.Web.UI.WebControls.Label


    Protected WithEvents lblNumDoc As System.Web.UI.WebControls.Label
    Protected WithEvents txtNumDocInterno As System.Web.UI.WebControls.TextBox
    'Protected WithEvents txtIDDocInterno As System.Web.UI.WebControls.TextBox

    Protected WithEvents lblNomeFile As System.Web.UI.WebControls.Label
    Protected WithEvents txtNomeFile As System.Web.UI.WebControls.TextBox

    Protected WithEvents imgSelDoc As System.Web.UI.WebControls.ImageButton
    Protected WithEvents imgDownolad As System.Web.UI.WebControls.ImageButton
    Protected WithEvents hlScarica As System.Web.UI.WebControls.HyperLink



    Protected WithEvents imgCancDoc As System.Web.UI.WebControls.ImageButton
    Protected WithEvents imcCancProtSeg As System.Web.UI.WebControls.ImageButton
    Protected WithEvents imgCancProtRicSeg As System.Web.UI.WebControls.ImageButton
    Protected WithEvents imgCancProtAppSeg As System.Web.UI.WebControls.ImageButton

    'NOTA: la seguente dichiarazione è richiesta da Progettazione Web Form.
    'Non spostarla o rimuoverla.
    Private designerPlaceholderDeclaration As System.Object

    'Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
    '    'CODEGEN: questa chiamata al metodo è richiesta da Progettazione Web Form.
    '    'Non modificarla nell'editor del codice.
    '    InitializeComponent()
    'End Sub

#End Region

    'CREATA DA SIMONA CORDELLA 03/10/2011 
    'GESTIONE DELLA NUOVA TIPOLOGIA DI VERIFICA 
    'SEGNALATA UNSC
    Public dtrLeggiDati As SqlClient.SqlDataReader
    Public TBLLeggiDati As DataTable
    Public row As TableRow
    Public myRow As DataRow
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load, Me.Load
        'Inserire qui il codice utente necessario per inizializzare la pagina

        If Not Session("LogIn") Is Nothing Then
            If Session("LogIn") = False Then 'verifico validità log-in
                Response.Redirect("LogOn.aspx")
            End If
        Else
            Response.Redirect("LogOn.aspx")
        End If
        If Page.IsPostBack = False Then
            'txtidProgrammazione.Text = Request.QueryString("idprogrammazione")
            'txtidente.Text = Request.QueryString("Idente")
            CaricaMaschera(Request.QueryString("StatoSegnalata"), Request.QueryString("IdVerifica"))
        End If
    End Sub

    Sub CaricaMaschera(ByVal StatoSegnalata As String, ByVal intIdVerifica As Integer)
        lblmessaggio.Text = ""
        CaricaCombo()
        CaricaCompetenze()
        Session("pCodEnte") = Nothing
        If StatoSegnalata = "Modifica" Then
            txtIdVerifica.Text = intIdVerifica
            CaricaVerifiche(intIdVerifica)
        Else
            LblStatoVerifica.Text = "Da Registrare"
        End If
        PersonalizzaMaschera(LblStatoVerifica.Text, 22)
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

                strSQL = "select IdRegioneCompetenza,Descrizione,CodiceRegioneCompetenza,left(CodiceRegioneCompetenza,1)from RegioniCompetenze "
                strSQL = strSQL & " union "
                strSQL = strSQL & " select '0','','','A' "
                strSQL = strSQL & " from RegioniCompetenze "
                strSQL = strSQL & " order by left(CodiceRegioneCompetenza,1),descrizione "
                'chiudo il datareader se aperto
                If Not dtrCompetenze Is Nothing Then
                    dtrCompetenze.Close()
                    dtrCompetenze = Nothing
                End If

                'eseguo la query
                dtrCompetenze = ClsServer.CreaDatareader(strSQL, Session("conn"))
                'assegno il datadearder alla combo caricando così descrizione e id
                ddlCompetenze.DataSource = dtrCompetenze
                ddlCompetenze.Items.Add("")
                ddlCompetenze.DataTextField = "Descrizione"
                ddlCompetenze.DataValueField = "IDRegioneCompetenza"
                ddlCompetenze.DataBind()

                If Not dtrCompetenze Is Nothing Then
                    dtrCompetenze.Close()
                    dtrCompetenze = Nothing
                End If

                'chiudo il datareader se aperto
            End If
            'Controllo abilitazione scelta
            If Session("TipoUtente") = "U" Then
                ddlCompetenze.Enabled = True
                ddlCompetenze.SelectedValue = 0

            Else
                'CboCompetenza.SelectedIndex = 1
                ddlCompetenze.Enabled = False
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
                    ddlCompetenze.SelectedValue = dtrCompetenze("IdRegioneCompetenza")
                    If dtrCompetenze("Heliosread") = True Then
                        ddlCompetenze.Enabled = True
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
    Sub CaricaCombo()
        Dim strsql As String

        strsql = " Select IdUfficio, Ufficio from TUfficiUNSC"
        ddlServizi.DataSource = ClsServer.CreaDataTable(strsql, True, Session("conn"))
        ddlServizi.DataValueField = "IdUfficio"
        ddlServizi.DataTextField = "Ufficio"
        ddlServizi.DataBind()


        ''*****Carico Ufficio
        'ddlUfficio.DataSource = MakeParentTable("select IdUfficio, Ufficio from TUfficiUNSC ")
        'ddlUfficio.DataTextField = "ParentItem"
        'ddlUfficio.DataValueField = "id"
        'ddlUfficio.DataBind()
    End Sub

    Private Sub CaricaVerifiche(ByVal idVerifica As Integer)
        Dim dtrGenerico As SqlClient.SqlDataReader
        Dim strSql As String
        Try
            strSql = "SELECT TVerificheStati.StatoVerifiche,TVerifiche.CodiceFascicolo,TVerifiche.IdFascicolo, TVerifiche.DescrFascicolo, "
            strSql = strSql & " TVerifiche.DataInvioLetteraContestazione, TVerifiche.DataProtInvioLetteraContestazione,TVerifiche.NProtInvioLetteraContestazione,TVerifiche.DataRicezioneLetteraContestazione,"
            strSql = strSql & " TVerifiche.DataRispostaLetteraContestazione,TVerifiche.DataProtRispostaLetteraContestazion,TVerifiche.NProtRispostaLetteraContestazion,"
            strSql = strSql & " TVerifiche.DataTrasmissioneSanzione,TVerifiche.DataProtTrasmissioneSanzione,TVerifiche.NProtTrasmissioneSanzione,TVerifiche.DataEsecuzioneSanzione,"
            strSql = strSql & " TVerifiche.DataProtEsecuzioneSanzione,TVerifiche.NProtEsecuzioneSanzione,TVerifiche.IDUfficioUNSC,TVerifiche.IDRegioneCompetenza,TVerifiche.Note,"
            strSql = strSql & " TVerifiche.IdSanzione, TVerifiche.DataProtChiusuraContestazione, TVerifiche.NProtChiusuraContestazione,TVerifiche.NonEffettuata,"
            strSql = strSql & " TVerifiche.DataProtLetteraContestazioneDG, TVerifiche.NProtLetteraContestazioneDG,TVerifiche.DataProtTrasmissioneServizi,TVerifiche.NProtTrasmissioneServizi,  "
            strSql = strSql & " enti.Denominazione + ' (' + enti.CodiceRegione + ')' as Ente, enti.CodiceRegione,"
            strSql = strSql & " attività.Titolo + ' (' + attività.codiceente + ')' as progetto,"
            strSql = strSql & " entisedi.Denominazione + ' (' + CONVERT(varchar, entisediattuazioni.IDEnteSedeAttuazione) + ')'  + ' - ' +  comuni.Denominazione AS Sede,"
            strSql = strSql & " enti_1.idente as IdEntePartner,enti_1.Denominazione as EntePartner,      "
            strSql = strSql & " TVerifiche.DataProtSegnalazioneUNSC, TVerifiche.NProtSegnalazioneUNSC, "
            strSql = strSql & " TVerifiche.DataProtRicezioneSegnalazione, TVerifiche.NProtRicezioneSegnalazione, TVerifiche.DataProtApprovazioneSegnalazione, "
            strSql = strSql & " TVerifiche.NProtApprovazioneSegnalazione, TVerifiche.IDServizio, TVerifiche.IDAttivita, "
            strSql = strSql & " TVerifiche.IDEnteSedeAttuazione, TVerifiche.IdEnte,TVerifiche.IDDocInterno, TVerifiche.CodiceDocInterno, TVerifiche.FileDocInterno,TVerifiche.DataSegnalazione "
            strSql = strSql & " FROM TVerifiche "
            strSql = strSql & " INNER JOIN TVerificheStati ON TVerifiche.IDStatoVerifica = TVerificheStati.IDStatoVerifiche "
            strSql = strSql & " INNER JOIN enti ON TVerifiche.IDEnte = enti.IDEnte "
            strSql = strSql & " LEFT JOIN attività ON TVerifiche.IDAttivita = attività.IDAttività "
            strSql = strSql & " LEFT JOIN entisediattuazioni ON TVerifiche.IDEnteSedeAttuazione = entisediattuazioni.IDEnteSedeAttuazione "
            strSql = strSql & " LEFT JOIN entisedi ON entisedi.IDEnteSede = entisediattuazioni.IDEnteSede"
            strSql = strSql & " LEFT JOIN enti enti_1 on entisedi.idente =enti_1.idente"
            strSql = strSql & " LEFT JOIN comuni ON entisedi.IDComune = comuni.IDComune"
            strSql = strSql & " WHERE TVerifiche.idverifica = " & idVerifica
            If Not dtrGenerico Is Nothing Then
                dtrGenerico.Close()
                dtrGenerico = Nothing
            End If

            dtrGenerico = ClsServer.CreaDatareader(strSql, Session("Conn"))

            dtrGenerico.Read()
            LblStatoVerifica.Text = "" & dtrGenerico("StatoVerifiche")
            '*** SCHEDA INFORMAZIONI GENERALI *** 
            TxtCodiceFascicolo.Text = "" & dtrGenerico("CodiceFascicolo")
            TxtCodiceFasc.Value = "" & dtrGenerico("IDFascicolo")
            txtDescFasc.Text = "" & dtrGenerico("DescrFascicolo")
            txtDataProtSegnalazione.Text = "" & dtrGenerico("DataProtSegnalazioneUNSC")
            txtNumProtSegnalazione.Text = "" & dtrGenerico("NProtSegnalazioneUNSC")
            txtDataProtRicSegnalazione.Text = "" & dtrGenerico("DataProtRicezioneSegnalazione")
            TxtNumProtRicSegnalazione.Text = "" & dtrGenerico("NProtRicezioneSegnalazione")
            TxtDataProtApprovazione.Text = "" & dtrGenerico("DataProtApprovazioneSegnalazione")
            TxtNumProtApprovazione.Text = "" & dtrGenerico("NProtApprovazioneSegnalazione")
            ddlServizi.SelectedValue = "" & dtrGenerico("IDServizio")
            txtDataSegnalazione.Text = "" & dtrGenerico("DataSegnalazione")
            'If ddlServizi.SelectedValue <> 0 Then
            '    txtServizi.Text = ddlServizi.SelectedItem.Text
            'End If
            Session("pCodEnte") = "" & dtrGenerico("CodiceRegione")
            txtEnte.Text = "" & dtrGenerico("Ente")
            txtIdEnte.Value = "" & dtrGenerico("IdEnte")
            txtProgetto.Text = "" & dtrGenerico("Progetto")
            txtIdAttivita.Value = "" & dtrGenerico("IDAttivita")
            txtSede.Text = "" & dtrGenerico("Sede")
            txtIdEnteSedeAttuazione.Value = "" & dtrGenerico("IDEnteSedeAttuazione")
            If Not IsDBNull(dtrGenerico("IdEntePartner")) Then
                If Request.QueryString("IdEnte") <> dtrGenerico("IdEntePartner") Then
                    TxtEnteFiglio.Text = "" & dtrGenerico("EntePartner")
                End If
            End If
            '*** aggiounto da simona cordella il 14/01/2013 gestione del documento interno ***
            txtNumDocInterno.Text = "" & dtrGenerico("CodiceDocInterno")
            txtIDDocInterno.Value = "" & dtrGenerico("IDDocInterno")
            txtNomeFile.Text = "" & dtrGenerico("FileDocInterno")
            '****
            '**** fine SCHEDA INFORMAZIONI GENERALI *** ****
            '**** SCHEDA CONTENZIOSO ****
            txtDataProtocolloInvioLetteraContestazione.Text = "" & dtrGenerico("DataProtInvioLetteraContestazione")
            txtDataProtLetteraContestazioneDG.Text = "" & dtrGenerico("DataProtLetteraContestazioneDG")
            txtNProtLetteraContestazioneDG.Text = "" & dtrGenerico("NProtLetteraContestazioneDG")
            TxtNumeroProtocolloInvioLettContestazione.Text = "" & dtrGenerico("NProtInvioLetteraContestazione")
            txtDataRicezioneLetteraContestazione.Text = "" & dtrGenerico("DataRicezioneLetteraContestazione")
            txtDataRispostaLetteraContestazione.Text = "" & dtrGenerico("DataRispostaLetteraContestazione")
            txtDataProtocolloRispostaContestazione.Text = "" & dtrGenerico("DataProtRispostaLetteraContestazion")
            TxtNumProtocolloRispostaContestazione.Text = "" & dtrGenerico("NProtRispostaLetteraContestazion")
            TxtDataProtChiusuraContestazione.Text = "" & dtrGenerico("DataProtChiusuraContestazione")
            TxtNumProtChiusuraContestazione.Text = "" & dtrGenerico("NProtChiusuraContestazione")
            '**** FINE SCHEDA CONTENZIOSO ****
            '**** SCHEDA SANZIONE *****
            txtDataProtocolloTrasmissioneSanzione.Text = "" & dtrGenerico("DataProtTrasmissioneSanzione")
            TxtNumeroProtocolloTrasmissioneSanzione.Text = "" & dtrGenerico("NProtTrasmissioneSanzione")

            If Not Request.QueryString("DataProtTrasmSanzioneDG") Is Nothing Then
                txtDataProtocolloTrasmissioneSanzione.Text = Trim(Request.QueryString("DataProtTrasmSanzioneDG"))
            Else
                txtDataProtocolloTrasmissioneSanzione.Text = "" & dtrGenerico("DataProtTrasmissioneSanzione")
            End If
            If Not Request.QueryString("NumProtTrasmSanzioneDG") Is Nothing Then
                TxtNumeroProtocolloTrasmissioneSanzione.Text = Trim(Request.QueryString("NumProtTrasmSanzioneDG"))
            Else
                TxtNumeroProtocolloTrasmissioneSanzione.Text = "" & dtrGenerico("NProtTrasmissioneSanzione")
            End If









            txtDataProtocolloTrasmServizi.Text = "" & dtrGenerico("DataProtTrasmissioneServizi")
            txtNProtocolloTrasmServizi.Text = "" & dtrGenerico("NProtTrasmissioneServizi")
            txtDataEsecuzioneSanzione.Text = "" & dtrGenerico("DataEsecuzioneSanzione")
            If Not Request.QueryString("DataProtEsecSanzione") Is Nothing Then
                txtDataProtocolloEsecuzioneSanzione.Text = Trim(Request.QueryString("DataProtEsecSanzione"))
            Else
                txtDataProtocolloEsecuzioneSanzione.Text = "" & dtrGenerico("DataProtEsecuzioneSanzione")
            End If
            If Not Request.QueryString("NumProtEsecSanzione") Is Nothing Then
                TxtNumeroProtocolloEsecuzioneSanzione.Text = Trim(Request.QueryString("NumProtEsecSanzione"))
            Else
                TxtNumeroProtocolloEsecuzioneSanzione.Text = "" & dtrGenerico("NProtEsecuzioneSanzione")
            End If





            '**** FINE SCHEDA SANZIONE *****
        Catch ex As Exception

            Response.Write(ex.Message)
        Finally
            If Not dtrGenerico Is Nothing Then
                dtrGenerico.Close()
                dtrGenerico = Nothing
            End If
        End Try
    End Sub

    Private Sub CmdStampaCredenzialiIncarico_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles CmdStampaCredenzialiIncarico.Click
        Dim Documento As New GeneratoreModelli

        Response.Write("<SCRIPT>" & vbCrLf)
        Response.Write("window.open('" & Documento.MON_credenziali(ClsUtility.TrovaIDEnteDaIDVerifica(Request.QueryString("IdVerifica"), Session("conn")), Request.QueryString("IdVerifica"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn")) & "');" & vbCrLf)
        Response.Write("</SCRIPT>")
        'chiamo la proprietà della classe GeneratoreModelli che ritorna la path del documento creato
        Documento.Dispose()

    End Sub

    Private Sub CmdStampaLettereIncarico_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles CmdStampaLettereIncarico.Click
        Dim Documento As New GeneratoreModelli

        Response.Write("<SCRIPT>" & vbCrLf)
        Response.Write("window.open('" & Documento.MON_Letteradiincarico(ClsUtility.TrovaIDEnteDaIDVerifica(Request.QueryString("IdVerifica"), Session("conn")), Request.QueryString("IdVerifica"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn")) & "');" & vbCrLf)
        Response.Write("</SCRIPT>")
        'chiamo la proprietà della classe GeneratoreModelli che ritorna la path del documento creato
        Documento.Dispose()
    End Sub

    'Function StampaDocumentiGen(ByVal intIdVERIFICA As Integer, ByVal NomeFile As String) As String
    '    Dim xStr As String
    '    Dim xLinea As String
    '    Dim Writer As StreamWriter
    '    Dim Reader As StreamReader
    '    Dim strPercorsoFile As String
    '    Dim strsql As String
    '    Dim strDataOdierna As String
    '    Dim strNomeFile As String

    '    Dim datafineattività As String

    '    If Not dtrLeggiDati Is Nothing Then
    '        dtrLeggiDati.Close()
    '        dtrLeggiDati = Nothing
    '    End If
    '    dtrLeggiDati = ClsServer.CreaDatareader("select top 1 datafineattività,titolo from attività where identepresentante = 14 order by datafineattività desc", Session("conn"))
    '    dtrLeggiDati.Read()
    '    datafineattività = dtrLeggiDati("datafineattività")
    '    If Not dtrLeggiDati Is Nothing Then
    '        dtrLeggiDati.Close()
    '        dtrLeggiDati = Nothing
    '    End If

    '    Try
    '        'chiudo il datareader
    '        If Not dtrLeggiDati Is Nothing Then
    '            dtrLeggiDati.Close()
    '            dtrLeggiDati = Nothing
    '        End If

    '        'prendo la data dal server
    '        dtrLeggiDati = ClsServer.CreaDatareader("select getdate() as dataOggi", Session("conn"))
    '        dtrLeggiDati.Read()
    '        'passo la data odierna ad una variabile locale
    '        strDataOdierna = IIf(Len(CStr(Day(dtrLeggiDati("dataOggi")))) < 2, "0" & CStr(Day(dtrLeggiDati("dataOggi"))), CStr(Day(dtrLeggiDati("dataOggi")))) & "/" & IIf(Len(CStr(Month(dtrLeggiDati("dataOggi")))) < 2, "0" & CStr(Month(dtrLeggiDati("dataOggi"))), CStr(Month(dtrLeggiDati("dataOggi")))) & "/" & CStr(Year(dtrLeggiDati("dataOggi")))

    '        Call Dati()

    '        If dtrLeggiDati.HasRows = True Then
    '            dtrLeggiDati.Read()
    '            'creo il nome del file
    '            strNomeFile = NomeFile & CStr(Session("IdEnte")) & CStr(Session("Username")) & CStr(Day(Now)) & CStr(Month(Now)) & CStr(Year(Now)) & Hour(Now) & Minute(Now) & Second(Now) & ".doc"
    '            'creo il percorso del file da salvare
    '            strPercorsoFile = Server.MapPath("./documentazione/") & strNomeFile

    '            'apro il file che fa da template
    '            Reader = New StreamReader(Server.MapPath("./documentazione/master/Naz/" & Session("Path") & NomeFile & ".rtf"))

    '            Writer = New StreamWriter(strPercorsoFile, True, System.Text.Encoding.Default)

    '            'Writer.WriteLine("{\rtf1")


    '            xLinea = Reader.ReadLine()


    '            'Dim strDataCostituzione As String = dtrLeggiDati("DataCostituzione")

    '            Dim strdataProt As String = IIf(Not IsDBNull(dtrLeggiDati("DataProtIncarico")), dtrLeggiDati("DataProtIncarico"), "")
    '            Dim strnumprot As String = IIf(Not IsDBNull(dtrLeggiDati("NprotIncarico")), dtrLeggiDati("NprotIncarico"), "")
    '            Dim cognome As String = dtrLeggiDati("Cognome")
    '            Dim nome As String = dtrLeggiDati("Nome")
    '            Dim strAnagrafico As String = cognome & " " & nome
    '            Dim DataInizioVerifica As String = IIf(Not IsDBNull(dtrLeggiDati("DataInizioVerifica")), dtrLeggiDati("DataInizioVerifica"), "")
    '            Dim DataFineVerifica As String = IIf(Not IsDBNull(dtrLeggiDati("DataFineVerifica")), dtrLeggiDati("DataFineVerifica"), "")
    '            Dim Bando As String = dtrLeggiDati("Bando")
    '            Dim settore As String = dtrLeggiDati("Settore")
    '            Dim Area As String = dtrLeggiDati("Area")
    '            Dim Nvol As String = dtrLeggiDati("NumeroVolontari")
    '            Dim datarispostaente As String = IIf(Not IsDBNull(dtrLeggiDati("DataProtRispostaLetteraContestazion")), dtrLeggiDati("DataProtRispostaLetteraContestazion"), "")
    '            Dim DataInvioFax As String = IIf(Not IsDBNull(dtrLeggiDati("DataInvioFax")), dtrLeggiDati("DataInvioFax"), "")
    '            Dim DataTrasmissioneSanzione As String = IIf(Not IsDBNull(dtrLeggiDati("DataTrasmissioneSanzione")), dtrLeggiDati("DataTrasmissioneSanzione"), "")
    '            Dim DataProtTrasmissioneSanzione As String = IIf(Not IsDBNull(dtrLeggiDati("DataProtTrasmissioneSanzione")), dtrLeggiDati("DataProtTrasmissioneSanzione"), "")
    '            Dim RiferimentoIGF As String = IIf(Not IsDBNull(dtrLeggiDati("Riferimento")), dtrLeggiDati("Riferimento"), "")

    '            'dati figlio
    '            Dim EnteFiglio As String = dtrLeggiDati("EnteFiglio")
    '            Dim Civico As String = dtrLeggiDati("Civico")
    '            Dim Indirizzo As String = dtrLeggiDati("Indirizzo")
    '            Dim strCap As String = dtrLeggiDati("Cap")
    '            Dim strComune As String = dtrLeggiDati("Comune")
    '            Dim ProvinciaBreve As String = dtrLeggiDati("DescrAbb")


    '            'dati padre
    '            Dim strDenominazioneEnte As String = dtrLeggiDati("Denominazione")
    '            Dim strCodiceSede As String = dtrLeggiDati("CodiceSede")
    '            Dim strCodiceEnte As String = dtrLeggiDati("CodiceRegione")
    '            Dim strCodiceProgetto As String = dtrLeggiDati("CodiceEnte")
    '            Dim strtitolo As String = dtrLeggiDati("Titolo")
    '            Dim IndirizzoLegale As String = dtrLeggiDati("IndirizzoLegale")
    '            Dim CivicoLegale As String = dtrLeggiDati("CivicoLegale")
    '            Dim CapLegale As String = dtrLeggiDati("CapLegale")
    '            Dim ComuneLegale As String = dtrLeggiDati("ComuneLegale")
    '            Dim ProvinciaLegale As String = dtrLeggiDati("ProvinciaLegale")
    '            Dim AntodataIA As String = IIf(Not IsDBNull(dtrLeggiDati("datainizioattività")), dtrLeggiDati("datainizioattività"), "")

    '            Dim GU As String = dtrLeggiDati("GU")
    '            Dim Classe As String = dtrLeggiDati("classe")
    '            Dim DataProtRelazione As String = IIf(Not IsDBNull(dtrLeggiDati("DataProtRelazione")), dtrLeggiDati("DataProtRelazione"), "")

    '            Dim DataProtCD As String = IIf(Not IsDBNull(dtrLeggiDati("DataProtCD")), dtrLeggiDati("DataProtCD"), "")
    '            Dim NProtCD As String = IIf(Not IsDBNull(dtrLeggiDati("NProtCD")), dtrLeggiDati("NProtCD"), "")
    '            Dim NProtRisLetCont As String = IIf(Not IsDBNull(dtrLeggiDati("NProtRispostaLetteraContestazion")), dtrLeggiDati("NProtRispostaLetteraContestazion"), "")
    '            Dim DataProtRisLetCont As String = IIf(Not IsDBNull(dtrLeggiDati("DataProtRispostaLetteraContestazion")), dtrLeggiDati("DataProtRispostaLetteraContestazion"), "")

    '            While xLinea <> ""
    '                xLinea = Replace(xLinea, "<DataOdierna>", strDataOdierna)
    '                xLinea = Replace(xLinea, "<DenominazioneEnte>", strDenominazioneEnte)
    '                xLinea = Replace(xLinea, "<CodiceEnte>", strCodiceEnte)
    '                xLinea = Replace(xLinea, "<Titolo>", strtitolo)
    '                xLinea = Replace(xLinea, "<Cap>", strCap)
    '                xLinea = Replace(xLinea, "<DataProt>", strdataProt)
    '                xLinea = Replace(xLinea, "<Nprot>", strnumprot)
    '                xLinea = Replace(xLinea, "<CognomeNome>", strAnagrafico)
    '                xLinea = Replace(xLinea, "<CodiceSEDE>", strCodiceSede)
    '                xLinea = Replace(xLinea, "<EnteFiglio>", EnteFiglio)
    '                xLinea = Replace(xLinea, "<Comune>", strComune)
    '                xLinea = Replace(xLinea, "<Indirizzo>", Indirizzo)
    '                xLinea = Replace(xLinea, "<Civico>", Civico)
    '                xLinea = Replace(xLinea, "<DescrAbb>", ProvinciaBreve)
    '                xLinea = Replace(xLinea, "<IndirizzoLegale>", IndirizzoLegale)
    '                xLinea = Replace(xLinea, "<CivicoLegale>", CivicoLegale)
    '                xLinea = Replace(xLinea, "<CapLegale>", CapLegale)
    '                xLinea = Replace(xLinea, "<ComuneLegale>", ComuneLegale)
    '                xLinea = Replace(xLinea, "<ProvinciaLegale>", ProvinciaLegale)
    '                xLinea = Replace(xLinea, "<DataInizioVerifica>", DataInizioVerifica)
    '                xLinea = Replace(xLinea, "<DataFineVerifica>", DataFineVerifica)
    '                xLinea = Replace(xLinea, "<CodiceProggetto>", strCodiceProgetto)
    '                xLinea = Replace(xLinea, "<Bando>", Bando)
    '                xLinea = Replace(xLinea, "<Settore>", settore)
    '                xLinea = Replace(xLinea, "<Area>", Area)
    '                xLinea = Replace(xLinea, "<Nvol>", Nvol)
    '                xLinea = Replace(xLinea, "<datainizioattivita>", AntodataIA)
    '                xLinea = Replace(xLinea, "<datarispostaente>", datarispostaente)
    '                xLinea = Replace(xLinea, "<DataInvioFax>", DataInvioFax)
    '                xLinea = Replace(xLinea, "<DataTrasmissioneSanzione>", DataTrasmissioneSanzione)
    '                xLinea = Replace(xLinea, "<DataProtTrasmissioneSanzione>", DataProtTrasmissioneSanzione)
    '                xLinea = Replace(xLinea, "<DFA>", datafineattività)
    '                xLinea = Replace(xLinea, "<GU>", GU)
    '                xLinea = Replace(xLinea, "<Classe>", Classe)
    '                xLinea = Replace(xLinea, "<DataProtRelazione>", DataProtRelazione)
    '                xLinea = Replace(xLinea, "<DataProtCD>", DataProtCD)
    '                xLinea = Replace(xLinea, "<NProtCD>", NProtCD)
    '                xLinea = Replace(xLinea, "<NProtRisLetCont>", NProtRisLetCont)
    '                xLinea = Replace(xLinea, "<DataProtRisLetCont>", DataProtRisLetCont)
    '                xLinea = Replace(xLinea, "<Riferimento>", RiferimentoIGF)


    '                Dim intX As Integer


    '                If InStr(xLinea, "<BreakPoint>") > 0 Then
    '                    xLinea = Replace(xLinea, "<BreakPoint>", "") & "\par"
    '                    Call DatiFigliverifica()
    '                    'If dtrLeggiDati.HasRows = True Then
    '                    'intX = dtrLeggiDati.FieldCount
    '                    'While dtrLeggiDati.Read
    '                    Dim num As Integer
    '                    num = 0
    '                    For Each myRow In TBLLeggiDati.Rows
    '                        num = num + 1
    '                        '''''''Writer.WriteLine("\trowd\trgaph70\trleft-70\trbrdrl\brdrs\brdrw10 ")
    '                        '''''''Writer.WriteLine("\trbrdrt\brdrs\brdrw10 \trbrdrr\brdrs\brdrw10 ")
    '                        '''''''Writer.WriteLine("\trbrdrb\brdrs\brdrw10 ")
    '                        '''''''Writer.WriteLine("\trpaddl70\trpaddr70\trpaddfl3\trpaddfr3")
    '                        '''''''Writer.WriteLine("\clbrdrl\brdrw10\brdrs\clbrdrt\brdrw10\brdrs\clbrdrr\brdrw10\brdrs\clbrdrb\brdrw10\brdrs ")
    '                        '''''''Writer.WriteLine("\cellx3500\clbrdrl\brdrw10\brdrs\clbrdrt\brdrw10\brdrs\clbrdrr\brdrw10\brdrs\clbrdrb\brdrw10\brdrs ")
    '                        '''''''Writer.WriteLine("\cellx7000\clbrdrl\brdrw10\brdrs\clbrdrt\brdrw10\brdrs\clbrdrr\brdrw10\brdrs\clbrdrb\brdrw10\brdrs ")
    '                        '''''''Writer.WriteLine("\cellx7500\clbrdrl\brdrw10\brdrs\clbrdrt\brdrw10\brdrs\clbrdrr\brdrw10\brdrs\clbrdrb\brdrw10\brdrs ")
    '                        '''''''Writer.WriteLine("\cellx9000\clbrdrl\brdrw10\brdrs\clbrdrt\brdrw10\brdrs\clbrdrr\brdrw10\brdrs\clbrdrb\brdrw10\brdrs ")
    '                        '''''''Writer.WriteLine("\cellx10040\pard\intbl\nowidctlpar\ri500\qj\f2\fs20\ " & dtrLeggiDati("EnteFiglio") & "\cell " & dtrLeggiDati("Indirizzo") & "\cell " & dtrLeggiDati("Civico") & "\cell " & dtrLeggiDati("Comune") & "\cell " & dtrLeggiDati("DescrAbb") & "\cell\row\pard\f2\fs20")
    '                        Writer.WriteLine("\trowd\trgaph70\trleft-70\trbrdrl\brdrs\brdrw10 ")
    '                        Writer.WriteLine("\trbrdrt\brdrs\brdrw10 \trbrdrr\brdrs\brdrw10 ")
    '                        Writer.WriteLine("\trbrdrb\brdrs\brdrw10 ")
    '                        Writer.WriteLine("\trpaddl70\trpaddr70\trpaddfl3\trpaddfr3")
    '                        Writer.WriteLine("\clbrdrl\brdrw10\brdrs\clbrdrt\brdrw10\brdrs\clbrdrr\brdrw10\brdrs\clbrdrb\brdrw10\brdrs ")
    '                        Writer.WriteLine("\cellx700\clbrdrl\brdrw10\brdrs\clbrdrt\brdrw10\brdrs\clbrdrr\brdrw10\brdrs\clbrdrb\brdrw10\brdrs ")
    '                        Writer.WriteLine("\cellx3500\clbrdrl\brdrw10\brdrs\clbrdrt\brdrw10\brdrs\clbrdrr\brdrw10\brdrs\clbrdrb\brdrw10\brdrs ")
    '                        Writer.WriteLine("\cellx7000\clbrdrl\brdrw10\brdrs\clbrdrt\brdrw10\brdrs\clbrdrr\brdrw10\brdrs\clbrdrb\brdrw10\brdrs ")
    '                        Writer.WriteLine("\cellx7500\clbrdrl\brdrw10\brdrs\clbrdrt\brdrw10\brdrs\clbrdrr\brdrw10\brdrs\clbrdrb\brdrw10\brdrs ")
    '                        Writer.WriteLine("\cellx9100\clbrdrl\brdrw10\brdrs\clbrdrt\brdrw10\brdrs\clbrdrr\brdrw10\brdrs\clbrdrb\brdrw10\brdrs ")
    '                        Writer.WriteLine("\cellx10040\pard\intbl\nowidctlpar\ri500\qj\f2\fs15\ " & num & "\cell " & myRow.Item("EnteFiglio") & "\cell " & myRow.Item("Indirizzo") & "\cell " & myRow.Item("Civico") & "\cell " & myRow.Item("Comune") & "\cell " & myRow.Item("DescrAbb") & "\cell\row\pard\f2\fs15")
    '                        'xLinea = "\b " & dtrLeggiDati("Titolo") & "\tqc " & dtrLeggiDati("CodiceEnte") & "\tqc " & dtrLeggiDati("DatainizioPrevista") & "\b0\par"
    '                        'Writer.WriteLine(xLinea)
    '                        'End While
    '                    Next
    '                    Writer.WriteLine("\par")
    '                End If
    '                'End If

    '                Writer.WriteLine(xLinea)
    '                xLinea = Reader.ReadLine()

    '            End While

    '            'close the RTF string and file
    '            'Writer.WriteLine("}")
    '            Writer.Close()
    '            Writer = Nothing

    '            ''chiudo lo streaming in scrittura
    '            Reader.Close()
    '            Reader = Nothing

    '        End If

    '        'chiudo il datareader
    '        If Not dtrLeggiDati Is Nothing Then
    '            dtrLeggiDati.Close()
    '            dtrLeggiDati = Nothing
    '        End If

    '        StampaDocumentiGen = "documentazione/" & strNomeFile

    '    Catch ex As Exception
    '        If Not dtrLeggiDati Is Nothing Then
    '            dtrLeggiDati.Close()
    '            dtrLeggiDati = Nothing
    '        End If

    '        Response.Write(ex.Message)
    '    End Try

    '    Response.Write("<SCRIPT>" & vbCrLf)
    '    Response.Write("window.open('" & StampaDocumentiGen & "');" & vbCrLf)
    '    Response.Write("</SCRIPT>")

    'End Function

    'Function StampaDocumentiVerifica(ByVal intIdVERIFICA As Integer, ByVal NomeFile As String) As String
    '    Dim xStr As String
    '    Dim xLinea As String
    '    Dim Writer As StreamWriter
    '    Dim Reader As StreamReader
    '    Dim strPercorsoFile As String
    '    Dim strsql As String
    '    Dim strDataOdierna As String
    '    Dim strNomeFile As String

    '    Try
    '        'chiudo il datareader
    '        If Not dtrLeggiDati Is Nothing Then
    '            dtrLeggiDati.Close()
    '            dtrLeggiDati = Nothing
    '        End If

    '        'prendo la data dal server
    '        dtrLeggiDati = ClsServer.CreaDatareader("select getdate() as dataOggi", Session("conn"))
    '        dtrLeggiDati.Read()
    '        'passo la data odierna ad una variabile locale
    '        strDataOdierna = IIf(Len(CStr(Day(dtrLeggiDati("dataOggi")))) < 2, "0" & CStr(Day(dtrLeggiDati("dataOggi"))), CStr(Day(dtrLeggiDati("dataOggi")))) & "/" & IIf(Len(CStr(Month(dtrLeggiDati("dataOggi")))) < 2, "0" & CStr(Month(dtrLeggiDati("dataOggi"))), CStr(Month(dtrLeggiDati("dataOggi")))) & "/" & CStr(Year(dtrLeggiDati("dataOggi")))

    '        Call DatiVerifica()

    '        If dtrLeggiDati.HasRows = True Then
    '            dtrLeggiDati.Read()
    '            'creo il nome del file
    '            strNomeFile = NomeFile & CStr(Session("IdEnte")) & CStr(Session("Username")) & CStr(Day(Now)) & CStr(Month(Now)) & CStr(Year(Now)) & Hour(Now) & Minute(Now) & Second(Now) & ".doc"
    '            'creo il percorso del file da salvare
    '            strPercorsoFile = Server.MapPath("./documentazione/") & strNomeFile

    '            'apro il file che fa da template
    '            Reader = New StreamReader(Server.MapPath("./documentazione/master/Naz/" & Session("Path") & NomeFile & ".rtf"))

    '            Writer = New StreamWriter(strPercorsoFile, True, System.Text.Encoding.Default)

    '            'Writer.WriteLine("{\rtf1")


    '            xLinea = Reader.ReadLine()


    '            'Dim strDataCostituzione As String = dtrLeggiDati("DataCostituzione")

    '            Dim strdataProt As String = IIf(Not IsDBNull(dtrLeggiDati("DataProtIncarico")), dtrLeggiDati("DataProtIncarico"), "")
    '            Dim strnumprot As String = IIf(Not IsDBNull(dtrLeggiDati("NprotIncarico")), dtrLeggiDati("NprotIncarico"), "")
    '            Dim cognome As String = dtrLeggiDati("Cognome")
    '            Dim nome As String = dtrLeggiDati("Nome")
    '            Dim strAnagrafico As String = cognome & " " & nome
    '            Dim DataInizioVerifica As String = IIf(Not IsDBNull(dtrLeggiDati("DataInizioVerifica")), dtrLeggiDati("DataInizioVerifica"), "")
    '            Dim DataFineVerifica As String = IIf(Not IsDBNull(dtrLeggiDati("DataFineVerifica")), dtrLeggiDati("DataFineVerifica"), "")
    '            Dim Bando As String = dtrLeggiDati("Bando")
    '            Dim settore As String = dtrLeggiDati("Settore")
    '            Dim Area As String = dtrLeggiDati("Area")
    '            Dim Nvol As String = dtrLeggiDati("NumeroVolontari")
    '            Dim datarispostaente As String = IIf(Not IsDBNull(dtrLeggiDati("DataProtRispostaLetteraContestazion")), dtrLeggiDati("DataProtRispostaLetteraContestazion"), "")
    '            Dim DataInvioFax As String = IIf(Not IsDBNull(dtrLeggiDati("DataInvioFax")), dtrLeggiDati("DataInvioFax"), "")
    '            Dim DataTrasmissioneSanzione As String = IIf(Not IsDBNull(dtrLeggiDati("DataTrasmissioneSanzione")), dtrLeggiDati("DataTrasmissioneSanzione"), "")
    '            Dim DataProtTrasmissioneSanzione As String = IIf(Not IsDBNull(dtrLeggiDati("DataProtTrasmissioneSanzione")), dtrLeggiDati("DataProtTrasmissioneSanzione"), "")


    '            'dati figlio
    '            Dim EnteFiglio As String = dtrLeggiDati("EnteFiglio")
    '            Dim Civico As String = dtrLeggiDati("Civico")
    '            Dim Indirizzo As String = dtrLeggiDati("Indirizzo")
    '            Dim strCap As String = dtrLeggiDati("Cap")
    '            Dim strComune As String = dtrLeggiDati("Comune")
    '            Dim ProvinciaBreve As String = dtrLeggiDati("DescrAbb")


    '            'dati padre
    '            Dim strDenominazioneEnte As String = dtrLeggiDati("Denominazione")
    '            Dim strCodiceSede As String = dtrLeggiDati("CodiceSede")
    '            Dim strCodiceEnte As String = dtrLeggiDati("CodiceRegione")
    '            Dim strCodiceProgetto As String = dtrLeggiDati("CodiceEnte")
    '            Dim strtitolo As String = dtrLeggiDati("Titolo")
    '            Dim IndirizzoLegale As String = dtrLeggiDati("IndirizzoLegale")
    '            Dim CivicoLegale As String = dtrLeggiDati("CivicoLegale")
    '            Dim CapLegale As String = dtrLeggiDati("CapLegale")
    '            Dim ComuneLegale As String = dtrLeggiDati("ComuneLegale")
    '            Dim ProvinciaLegale As String = dtrLeggiDati("ProvinciaLegale")
    '            Dim AntodataIA As String = IIf(Not IsDBNull(dtrLeggiDati("datainizioattività")), dtrLeggiDati("datainizioattività"), "")

    '            Dim strProgrammazione As String = dtrLeggiDati("Programmazione")
    '            Dim strStatoVerifica As String = dtrLeggiDati("StatoVerifiche")
    '            Dim strDataInizioPro As String = IIf(Not IsDBNull(dtrLeggiDati("DataInizioAttività")), dtrLeggiDati("DataInizioAttività"), "")
    '            Dim strDataFinePro As String = IIf(Not IsDBNull(dtrLeggiDati("DataFineAttività")), dtrLeggiDati("DataFineAttività"), "")
    '            Dim strDataAssegnazione As String = IIf(Not IsDBNull(dtrLeggiDati("DataAssegnazione")), dtrLeggiDati("DataAssegnazione"), "")
    '            Dim strDataApprovazione As String = IIf(Not IsDBNull(dtrLeggiDati("DataApprovazione")), dtrLeggiDati("DataApprovazione"), "")
    '            Dim strTipoVerifica As String = dtrLeggiDati("TipoVerifica")

    '            While xLinea <> ""
    '                xLinea = Replace(xLinea, "<DataOdierna>", strDataOdierna)
    '                xLinea = Replace(xLinea, "<DenominazioneEnte>", strDenominazioneEnte)
    '                xLinea = Replace(xLinea, "<CodiceEnte>", strCodiceEnte)
    '                xLinea = Replace(xLinea, "<Titolo>", strtitolo)
    '                xLinea = Replace(xLinea, "<Programmazione>", strProgrammazione)
    '                xLinea = Replace(xLinea, "<Verificatore>", strAnagrafico)
    '                xLinea = Replace(xLinea, "<StatoVerifica>", strStatoVerifica)
    '                xLinea = Replace(xLinea, "<DataInizioProgetto>", strDataInizioPro)
    '                xLinea = Replace(xLinea, "<DataFineProgetto>", strDataFinePro)
    '                xLinea = Replace(xLinea, "<DataAssegnazione>", strDataAssegnazione)
    '                xLinea = Replace(xLinea, "<DataApprovazione>", strDataApprovazione)
    '                xLinea = Replace(xLinea, "<TipologiaVerifica>", strTipoVerifica)
    '                xLinea = Replace(xLinea, "<Cap>", strCap)
    '                xLinea = Replace(xLinea, "<DataProt>", strdataProt)
    '                xLinea = Replace(xLinea, "<Nprot>", strnumprot)
    '                xLinea = Replace(xLinea, "<CognomeNome>", strAnagrafico)
    '                xLinea = Replace(xLinea, "<CodiceSEDE>", strCodiceSede)
    '                xLinea = Replace(xLinea, "<EnteFiglio>", EnteFiglio)
    '                xLinea = Replace(xLinea, "<Comune>", strComune)
    '                xLinea = Replace(xLinea, "<Indirizzo>", Indirizzo)
    '                xLinea = Replace(xLinea, "<Civico>", Civico)
    '                xLinea = Replace(xLinea, "<DescrAbb>", ProvinciaBreve)
    '                xLinea = Replace(xLinea, "<IndirizzoLegale>", IndirizzoLegale)
    '                xLinea = Replace(xLinea, "<CivicoLegale>", CivicoLegale)
    '                xLinea = Replace(xLinea, "<CapLegale>", CapLegale)
    '                xLinea = Replace(xLinea, "<ComuneLegale>", ComuneLegale)
    '                xLinea = Replace(xLinea, "<ProvinciaLegale>", ProvinciaLegale)
    '                xLinea = Replace(xLinea, "<DataInizioVerifica>", DataInizioVerifica)
    '                xLinea = Replace(xLinea, "<DataFineVerifica>", DataFineVerifica)
    '                xLinea = Replace(xLinea, "<CodiceProggetto>", strCodiceProgetto)
    '                xLinea = Replace(xLinea, "<Bando>", Bando)
    '                xLinea = Replace(xLinea, "<Settore>", settore)
    '                xLinea = Replace(xLinea, "<Area>", Area)
    '                xLinea = Replace(xLinea, "<Nvol>", Nvol)
    '                xLinea = Replace(xLinea, "<datainizioattivita>", AntodataIA)
    '                xLinea = Replace(xLinea, "<datarispostaente>", datarispostaente)
    '                xLinea = Replace(xLinea, "<DataInvioFax>", DataInvioFax)
    '                xLinea = Replace(xLinea, "<DataTrasmissioneSanzione>", DataTrasmissioneSanzione)
    '                xLinea = Replace(xLinea, "<DataProtTrasmissioneSanzione>", DataProtTrasmissioneSanzione)
    '                Dim intX As Integer

    '                Writer.WriteLine(xLinea)

    '                xLinea = Reader.ReadLine()
    '            End While

    '            'close the RTF string and file
    '            'Writer.WriteLine("}")
    '            Writer.Close()
    '            Writer = Nothing

    '            ''chiudo lo streaming in scrittura
    '            Reader.Close()
    '            Reader = Nothing

    '        End If

    '        'chiudo il datareader
    '        If Not dtrLeggiDati Is Nothing Then
    '            dtrLeggiDati.Close()
    '            dtrLeggiDati = Nothing
    '        End If

    '        StampaDocumentiVerifica = "documentazione/" & strNomeFile

    '    Catch ex As Exception
    '        If Not dtrLeggiDati Is Nothing Then
    '            dtrLeggiDati.Close()
    '            dtrLeggiDati = Nothing
    '        End If

    '        Response.Write(ex.Message)
    '    End Try

    '    Response.Write("<SCRIPT>" & vbCrLf)
    '    Response.Write("window.open('" & StampaDocumentiVerifica & "');" & vbCrLf)
    '    Response.Write("</SCRIPT>")

    'End Function

    Sub Dati()
        Dim strsql As String
        'chiudo il datareader
        If Not dtrLeggiDati Is Nothing Then
            dtrLeggiDati.Close()
            dtrLeggiDati = Nothing
        End If
        strsql = "select * from VW_VER_RICERCA_VERIFICHE "
        strsql = strsql & "where idverifica =" & Request.QueryString("IdVerifica") ' Session("verificaselezionata")
        'eseguo la query e passo il risultato al datareader
        dtrLeggiDati = ClsServer.CreaDatareader(strsql, Session("conn"))
    End Sub
    Sub DatiVerifica()
        Dim strsql As String
        'chiudo il datareader
        If Not dtrLeggiDati Is Nothing Then
            dtrLeggiDati.Close()
            dtrLeggiDati = Nothing
        End If
        strsql = "select * from VW_StampaVerificaCompleta "
        strsql = strsql & "where idverifica =" & Request.QueryString("IdVerifica") ' Session("verificaselezionata")
        'eseguo la query e passo il risultato al datareader
        dtrLeggiDati = ClsServer.CreaDatareader(strsql, Session("conn"))
    End Sub

    Private Sub cmdTrasmissione_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles cmdTrasmissione.Click
        'Dim idverifica As Int16
        'idverifica = Request.QueryString("IdVerifica")
        'Call StampaDocumentiGen(idverifica, "TrasmissionerelazionealDG")

        Dim Documento As New GeneratoreModelli

        Response.Write("<SCRIPT>" & vbCrLf)
        Response.Write("window.open('" & Documento.MON_TrasmissionerelazionealDG(ClsUtility.TrovaIDEnteDaIDVerifica(Request.QueryString("IdVerifica"), Session("conn")), Request.QueryString("IdVerifica"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn")) & "');" & vbCrLf)
        Response.Write("</SCRIPT>")
        'chiamo la proprietà della classe GeneratoreModelli che ritorna la path del documento creato
        Documento.Dispose()
    End Sub

    Private Sub CmdConclusione_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles CmdConclusione.Click
        'Dim idverifica As Int16
        'idverifica = Request.QueryString("IdVerifica")
        'Call StampaDocumentiGen(idverifica, "ConclusioneVerificaPositiva")

        Dim Documento As New GeneratoreModelli

        Response.Write("<SCRIPT>" & vbCrLf)
        Response.Write("window.open('" & Documento.MON_ConclusioneVerificaPositiva(ClsUtility.TrovaIDEnteDaIDVerifica(Request.QueryString("IdVerifica"), Session("conn")), Request.QueryString("IdVerifica"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn")) & "');" & vbCrLf)
        Response.Write("</SCRIPT>")
        'chiamo la proprietà della classe GeneratoreModelli che ritorna la path del documento creato
        Documento.Dispose()
    End Sub

    Private Sub CmdIrregolarita_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles CmdIrregolarita.Click

        Dim Documento As New GeneratoreModelli

        Response.Write("<SCRIPT>" & vbCrLf)
        Response.Write("window.open('" & Documento.MON_VerificasenzaIRREGOLARITAconrichiamo(ClsUtility.TrovaIDEnteDaIDVerifica(Request.QueryString("IdVerifica"), Session("conn")), Request.QueryString("IdVerifica"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn")) & "');" & vbCrLf)
        Response.Write("</SCRIPT>")
        'chiamo la proprietà della classe GeneratoreModelli che ritorna la path del documento creato
        Documento.Dispose()
    End Sub

    Private Sub cmdContestazione_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles cmdContestazione.Click
        Dim Documento As New GeneratoreModelli

        Response.Write("<SCRIPT>" & vbCrLf)
        Response.Write("window.open('" & Documento.MON_LetteraContestazioneAddebiti(ClsUtility.TrovaIDEnteDaIDVerifica(Request.QueryString("IdVerifica"), Session("conn"), "SU"), Request.QueryString("IdVerifica"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn")) & "');" & vbCrLf)
        Response.Write("</SCRIPT>")
        'chiamo la proprietà della classe GeneratoreModelli che ritorna la path del documento creato
        Documento.Dispose()
    End Sub

    Private Sub CmdLetteraTrasm_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles CmdLetteraTrasm.Click
        Dim Documento As New GeneratoreModelli

        Response.Write("<SCRIPT>" & vbCrLf)
        Response.Write("window.open('" & Documento.MON_LETTERAtrasmDGecontestazione(ClsUtility.TrovaIDEnteDaIDVerifica(Request.QueryString("IdVerifica"), Session("conn"), "SU"), Request.QueryString("IdVerifica"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn")) & "');" & vbCrLf)
        Response.Write("</SCRIPT>")
        'chiamo la proprietà della classe GeneratoreModelli che ritorna la path del documento creato
        Documento.Dispose()
    End Sub

    Private Sub cmdDiffida_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles cmdDiffida.Click
        Dim Documento As New GeneratoreModelli

        Response.Write("<SCRIPT>" & vbCrLf)
        Response.Write("window.open('" & Documento.MON_Diffida(ClsUtility.TrovaIDEnteDaIDVerifica(Request.QueryString("IdVerifica"), Session("conn"), "SU"), Request.QueryString("IdVerifica"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn")) & "');" & vbCrLf)
        Response.Write("</SCRIPT>")
        'chiamo la proprietà della classe GeneratoreModelli che ritorna la path del documento creato
        Documento.Dispose()
    End Sub

    Private Sub CmdLetteraTrasmissioneProvv_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles CmdLetteraTrasmissioneProvv.Click
        Dim Documento As New GeneratoreModelli

        Response.Write("<SCRIPT>" & vbCrLf)
        Response.Write("window.open('" & Documento.MON_Letteratrasmissioneprovvedimentosanzionatorio(ClsUtility.TrovaIDEnteDaIDVerifica(Request.QueryString("IdVerifica"), Session("conn"), "SU"), Request.QueryString("IdVerifica"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn")) & "');" & vbCrLf)
        Response.Write("</SCRIPT>")
        'chiamo la proprietà della classe GeneratoreModelli che ritorna la path del documento creato
        Documento.Dispose()
    End Sub

    Private Sub CmdTrasmRich_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles CmdTrasmRich.Click
        Dim Documento As New GeneratoreModelli

        Response.Write("<SCRIPT>" & vbCrLf)
        Response.Write("window.open('" & Documento.MON_TrasmissionerelazionealDGconrichiamo(ClsUtility.TrovaIDEnteDaIDVerifica(Request.QueryString("IdVerifica"), Session("conn")), Request.QueryString("IdVerifica"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn")) & "');" & vbCrLf)
        Response.Write("</SCRIPT>")
        'chiamo la proprietà della classe GeneratoreModelli che ritorna la path del documento creato
        Documento.Dispose()
    End Sub
    Private Sub CmdLETTERATRASMISPROVAISERVIZI_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles CmdLETTERATRASMISPROVAISERVIZI.Click
        Dim Documento As New GeneratoreModelli

        Response.Write("<SCRIPT>" & vbCrLf)
        Response.Write("window.open('" & Documento.MON_LETTERATRASMISPROVAISERVIZI(ClsUtility.TrovaIDEnteDaIDVerifica(Request.QueryString("IdVerifica"), Session("conn"), "SU"), Request.QueryString("IdVerifica"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn")) & "');" & vbCrLf)
        Response.Write("</SCRIPT>")
        'chiamo la proprietà della classe GeneratoreModelli che ritorna la path del documento creato
        Documento.Dispose()
    End Sub
    Private Sub CmdTRASMISSIONEPROVVEDIMENTOREGIONE_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles CmdTRASMISSIONEPROVVEDIMENTOREGIONE.Click
        Dim Documento As New GeneratoreModelli

        Response.Write("<SCRIPT>" & vbCrLf)
        Response.Write("window.open('" & Documento.MON_TRASMISSIONEPROVVEDIMENTOREGIONE(ClsUtility.TrovaIDEnteDaIDVerifica(Request.QueryString("IdVerifica"), Session("conn"), "SU"), Request.QueryString("IdVerifica"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn")) & "');" & vbCrLf)
        Response.Write("</SCRIPT>")
        'chiamo la proprietà della classe GeneratoreModelli che ritorna la path del documento creato
        Documento.Dispose()
    End Sub
    Private Function TrovaTipologiaVerificatore(ByVal IdVerificatore) As Integer
        Dim strsql As String
        Dim dtrInt As SqlClient.SqlDataReader
        ' 0:  VERIFICATORE INTERNO 
        ' 1:  VERIFICATORE IGF 
        strsql = "Select Tipologia " & _
                " From Tverificatori " & _
                " where IdVerificatore =" & IdVerificatore & " "
        dtrInt = ClsServer.CreaDatareader(strsql, Session("conn"))
        If dtrInt.HasRows = True Then
            dtrInt.Read()
            If dtrInt("Tipologia") = True Then
                TrovaTipologiaVerificatore = 1
            Else
                TrovaTipologiaVerificatore = 0
            End If
        End If
        If Not dtrInt Is Nothing Then
            dtrInt.Close()
            dtrInt = Nothing
        End If
        Return TrovaTipologiaVerificatore
    End Function
    
    Private Sub PersonalizzaMaschera(ByVal StatoVerifica As String, ByVal IdRegCompetenza As Integer)
        'DisabilitaImmaginiCalendario()
        ' DisabilitaText()
        DisabilitaPulsantiStampa()
        'ColoraCampi()
        'imgAssegnaVer.Visible = False

        'cmdIncludi.Visible = False
        CmdSospendi.Visible = False
        cmdChiusaContestata.Visible = False
        'cmdSanzione.Enabled = False
        lkbSanzione.Enabled = False
        cmdStampa.Visible = False
        '** agg da s.c il 15/10/2010 rendo invisibili pulsante del fascicolo
        cmdSelFascicolo.Visible = False
        cmdSelProtocollo0.Visible = False
        cmdFascCanc.Visible = False
        '***
        '** 25/11/2010
        'PersonalizzaFascicolo_Regionalizzazione(IdRegCompetenza, StatoVerifica)
        imgSanzioneServizi.Attributes.Add("style", "visibility: hidden")
        CampiDG(False)
        SchedaInformazioniGenerali(False, Color.LightGray)
        SchedaContestatazione(False, Color.LightGray)
        SchedaSanzione(False, Color.LightGray)
        Select Case StatoVerifica
            Case "Da Registrare"
                SchedaInformazioniGenerali(True, Color.White)

                cmdSelFascicolo.Visible = True
                cmdSelProtocollo0.Visible = True
                cmdFascCanc.Visible = True
                cmdSalva.Visible = True
                'txtServizi.Visible = False
            Case "Aperta"
                SchedaInformazioniGenerali(True, Color.White)
                cmdSelFascicolo.Visible = True
                cmdSelProtocollo0.Visible = True
                cmdFascCanc.Visible = True
                CmdSospendi.Visible = True
                'aggiunto il 15/01/2013 gestione del documento interno
                Abilita_DisabilitaDocumentiInterni(True, Color.White)
                ImageSelDoc(True)

                Abilita_DisabilitaProtocolloSegnalazione(Color.White, True)
                ImageProtolloRichiesta(True)
                Abilita_DisabilitaProtocolloRicezioneSegnalazione(Color.White, True)
                ImageProtolloAcquisizione(True)
            Case "Eseguita"
                Abilita_DisabilitaServizi(False, Color.LightGray)
                'aggiunto il 15/01/2013 gestione del documento interno
                Abilita_DisabilitaDocumentiInterni(False, Color.LightGray)
                ImageProtolloRichiesta(False)
                Abilita_DisabilitaProtocolloSegnalazione(Color.LightGray, False)
                ImageProtolloAcquisizione(False)
                Abilita_DisabilitaProtocolloRicezioneSegnalazione(Color.LightGray, False)
                ImageProtolloAcquisizione(False)
                '*** mod. da simoan cordella il 15/01/2013 
                If (txtNumDocInterno.Text = "" And txtDataSegnalazione.Text = "") Then
                    'se non ho indicato il documento interno abilito il protocollo di approvazione
                    Abilita_DisabilitaProtocolloApprovazioneSegnalazione(Color.White, True)
                    ImageProtolloApprovazione(True)
                Else
                    'se ho indicato il documento interno disabilito il protocollo di approvazione
                    Abilita_DisabilitaProtocolloApprovazioneSegnalazione(Color.LightGray, False)
                    ImageProtolloApprovazione(False)
                End If
                '****
                Abilita_DisabilitaOggettoSegnalazione(True, Color.White)

                DataProtLetteraContestazioneDG(True, Color.White)
                ImageProtocolloTrasContestataDG(True)
                'modificato il 08/03/2013 da s.c.
                'aggiunto da s.c. il 15/12/2010
                DataProtocolloInvioLetteraContestazione(True, Color.White)
                ImageProtocolloInvioLettContestazione(True)

                'modificata il 11/06/2012 da Danilo Spagnulo spostata abilitazione fuori dal controllo IF richiesta da Rossi
                CmdLetteraTrasm.Enabled = True 'lettera trasmissione Contestazione al D.g.
                cmdContestazione.Enabled = True 'lettera invio contestazione (Addebiti)
                'fine modifica

                CmdSospendi.Visible = True
            Case "Chiusa Positivamente"
                CmdConclusione.Enabled = True
                cmdTrasmissione.Enabled = True
                CmdIrregolarita.Enabled = False
                CmdTrasmRich.Enabled = False
            Case "Chiusa con Richiamo"
                CmdConclusione.Enabled = False
                cmdTrasmissione.Enabled = False
                CmdIrregolarita.Enabled = True
                CmdTrasmRich.Enabled = True
                '** 25/11/2010

            Case "Chiusa non Effettuata" 'nuovo stato agg. da s.c il 13/07/2009 (spesso gestione della chiusa con richiamo) 
                CmdConclusione.Enabled = True
                cmdTrasmissione.Enabled = True
                CmdIrregolarita.Enabled = False
                CmdTrasmRich.Enabled = False
            Case "Contestata"
                SchedaInformazioniGenerali(False, Color.LightGray)

                cmdContestazione.Enabled = True
                CmdLetteraTrasm.Enabled = True
                'DataProtLetteraContestazioneDG(True, Color.White)
                'ImageProtocolloTrasContestataDG(True)
                DataProtocolloInvioLetteraContestazione(True, Color.White)
                ImageProtocolloInvioLettContestazione(True)

                'DataProtocolloRispostaContestazione(False, Color.LightGray)
                'ImageProtocolloRispContestazione(False)
                'DataProtocolloTrasmissioneSanzione(False, Color.LightGray)
                'ImageProtocolloTrasmissioneSanzione(False)
                'DataProtocolloEsecuzioneSanzione(False, Color.LightGray)
                'ImageProtocolloEsecuzioneSanzione(False)
                If txtDataProtocolloInvioLetteraContestazione.Text <> "" Then
                    DataProtocolloInvioLetteraContestazione(False, Color.LightGray)
                    DataRicezioneLetteraContestazione(True, Color.White)
                    DataProtocolloRispostaContestazione(True, Color.White)
                    ImageProtocolloRispContestazione(True)
                    DataProtocolloTrasmissioneSanzione(True, Color.White)
                    ImageProtocolloTrasmissioneSanzione(True)

                    DataProtocolloEsecuzioneSanzione(True, Color.White)
                    ImageProtocolloEsecuzioneSanzione(True)


                    'DataProtLetteraContestazioneDG(False, Color.LightGray)
                    ' ''ImageProtocolloTrasContestataDG(False)
                    ''DataProtocolloInvioLetteraContestazione(False, Color.LightGray)
                    ''ImageProtocolloInvioLettContestazione(False)

                    ''DataRicezioneLetteraContestazione(True, Color.White)
                    If txtDataProtocolloRispostaContestazione.Text <> "" Then 'txtDataProtocolloRispostaContestazione

                        'Data Ricezione Lettera Contestazione  --> txtDataRicezioneLetteraContestazione()
                        'Data Lettera Controdeduzioni	       --> txtDataRispostaLetteraContestazione
                        'Data Prot. Lettera Controdeduzioni    --> txtDataProtocolloRispostaContestazione	
                        'Data Prot. Chiusura Contestazione     --> TxtDataProtChiusuraContestazion
                        ImageProtocolloRispContestazione(True)
                        DataRicezioneLetteraContestazione(True, Color.White) 'controdeduzioni
                        DataProtocolloRispostaContestazione(True, Color.White)
                        ImageProtocolloRispContestazione(False)

                        DataProtChiusuraContestazione(False, Color.LightGray)
                        cmdChiusaContestata.Visible = True
                    End If
                    cmdContestazione.Enabled = True
                    CmdLetteraTrasm.Enabled = True
                End If
                'modificato da sc il 08/03/2013 
                DataProtocolloTrasmissioneSanzione(True, Color.White)
                ImageProtocolloTrasmissioneSanzione(True)
                DataProtocolloEsecuzioneSanzione(True, Color.White)
                ImageProtocolloEsecuzioneSanzione(True)

                If txtDataProtocolloEsecuzioneSanzione.Text <> "" Then
                    cmdChiusaContestata.Visible = False
                    lkbSanzione.Enabled = True
                End If
                cmdDiffida.Enabled = True

                '***** Richista mod. 1l 06/08/2008 da silavano pennessi 
                '***** Abilitazione oltre alla DIFFIDA anche della lettere Revoca,Interdizone e Cancellazione
                CmdInterdizione.Enabled = True
                CmdRevoca.Enabled = True
                CmdCancellazione.Enabled = True
                '*****
                CmdLetteraTrasmissioneProvv.Enabled = True
                CmdLETTERATRASMISPROVAISERVIZI.Enabled = True
                CmdTRASMISSIONEPROVVEDIMENTOREGIONE.Enabled = True
                'VerificaCompetenzaEnte(txtidente.Text)
            Case "Sanzionata"
                SchedaInformazioniGenerali(False, Color.LightGray)
                SchedaContestatazione(False, Color.LightGray)
                
                DataProtocolloEsecuzioneSanzione(True, Color.White)
                ImageProtocolloEsecuzioneSanzione(True)
                DataProtocolloTrasmissioneServizi(True, Color.White)
                ImageProtocolloTrasmServizi(True)
                If txtDataProtocolloEsecuzioneSanzione.Text <> "" Then 'abilito trasmissione servizio
                    If txtDataEsecuzioneSanzione.Text = "" Then
                        txtDataEsecuzioneSanzione.Text = txtDataProtocolloEsecuzioneSanzione.Text
                    End If
                    'controllo se ho indicato il/i servizio/i a cui appicare il protocolloTrasmissione Servizi
                    imgSanzioneServizi.Attributes.Add("style", "visibility: visible")
                End If
                If txtDataProtocolloTrasmServizi.Text <> "" Then
                    DataEsecuzioneSanzione(True, Color.White)
                    DataProtocolloEsecuzioneSanzione(True, Color.White)
                    ImageProtocolloEsecuzioneSanzione(True)
                End If
                If IdRegCompetenza = 22 Then
                    DataProtocolloTrasmissioneServizi(True, Color.White)
                    ImageProtocolloTrasmissioneSanzione(True)
                Else
                    If txtDataProtocolloTrasmServizi.Text = "" Then
                        txtDataProtocolloTrasmServizi.Text = txtDataProtocolloEsecuzioneSanzione.Text
                        txtNProtocolloTrasmServizi.Text = TxtNumeroProtocolloEsecuzioneSanzione.Text
                    End If
                End If
                'cmdSanzione.Enabled = True
                lkbSanzione.Enabled = True
                VerificaCompetenzaEnte(txtIdEnte.Value)
                cmdDiffida.Enabled = True
                CmdLetteraTrasmissioneProvv.Enabled = True
                CmdLETTERATRASMISPROVAISERVIZI.Enabled = True
                CmdTRASMISSIONEPROVVEDIMENTOREGIONE.Enabled = True
                CmdArchiviazione.Enabled = True
                CmdInterdizione.Enabled = True
                CmdRevoca.Enabled = True
                CmdCancellazione.Enabled = True
                If txtDataEsecuzioneSanzione.Text <> "" Then
                    If txtDataProtocolloTrasmServizi.Text <> "" Then
                        DataProtocolloEsecuzioneSanzione(False, Color.LightGray)
                        ImageProtocolloEsecuzioneSanzione(False)
                        DataProtocolloTrasmissioneServizi(False, Color.LightGray)
                        ImageProtocolloTrasmServizi(False)
                        DataEsecuzioneSanzione(False, Color.LightGray)
                    End If
                End If
            Case "Sospesa"
                Abilita_DisabilitaServizi(False, Color.LightGray)
                'aggiunto il 15/01/2013 gestione del documento interno
                Abilita_DisabilitaDocumentiInterni(False, Color.LightGray)
                Abilita_DisabilitaProtocolloSegnalazione(Color.LightGray, False)
                Abilita_DisabilitaProtocolloRicezioneSegnalazione(Color.LightGray, False)
                Abilita_DisabilitaProtocolloApprovazioneSegnalazione(Color.LightGray, False)
                Abilita_DisabilitaOggettoSegnalazione(False, Color.LightGray)
                cmdSalva.Visible = False
                'cmdIncludi.Visible = False
            Case "Chiusa Contestata"
                'tutto disabilitato
                Abilita_DisabilitaServizi(False, Color.LightGray)
                'aggiunto il 15/01/2013 gestione del documento interno
                Abilita_DisabilitaDocumentiInterni(False, Color.LightGray)
                Abilita_DisabilitaProtocolloSegnalazione(Color.LightGray, False)
                Abilita_DisabilitaProtocolloRicezioneSegnalazione(Color.LightGray, False)
                Abilita_DisabilitaProtocolloApprovazioneSegnalazione(Color.LightGray, False)
                Abilita_DisabilitaOggettoSegnalazione(False, Color.LightGray)

                CmdLetteraChiusContestazione.Enabled = True

                DataProtChiusuraContestazione(True, Color.White)
                ImageProtocolloProtChiusuraContestazione(True)

                If TxtDataProtChiusuraContestazione.Text <> "" Then
                    cmdSalva.Visible = False
                    DataProtChiusuraContestazione(False, Color.LightGray)
                    ImageProtocolloProtChiusuraContestazione(False)

                End If
                If ControlloSanzione() = True Then
                    'cmdSanzione.Enabled = True
                    lkbSanzione.Enabled = True
                End If
        End Select
        'Personalizzazione per la REGIONE 
        ' modificato il 07/06/2016
        If Session("IdRegioneCompetenzaUtente") <> 22 Then
            PersonalizzaFascicolo_Regionalizzazione()
        End If

    End Sub

    Sub CampiDG(ByVal blnValore As Boolean)

        dvDGContestazione.Visible = blnValore
        Label24.Visible = blnValore
        CmdLetteraTrasm.Visible = blnValore
        dvDGSanzione.Visible = blnValore
        Label26.Visible = blnValore
        CmdArchiviazione.Visible = blnValore
    End Sub
    Private Function ControlloSanzione() As Boolean
        Dim strSql As String
        Dim dtrSan As SqlClient.SqlDataReader
        If Not dtrSan Is Nothing Then
            dtrSan.Close()
            dtrSan = Nothing
        End If
        'verifico se sono state applicate delle sanzioni alla verifica
        strSql = " SELECT DataAnnullamentoSanzione, UserAnnullaSanzione " & _
                 " FROM VW_SANZIONI_ANNULLATE " & _
                 " WHERE IDVerifica = " & Request.QueryString("IdVerifica") & " " & _
                 " Order by DataAnnullamentoSanzione desc "
        dtrSan = ClsServer.CreaDatareader(strSql, Session("conn"))
        If dtrSan.HasRows = True Then
            dtrSan.Read()
            Dim strMessaggio As String
            strMessaggio = "Sanzione annullata il " & dtrSan("DataAnnullamentoSanzione") & " da " & dtrSan("UserAnnullaSanzione") & "."
            TxtNote.Text = TxtNote.Text & vbCrLf & strMessaggio
            ControlloSanzione = True
        End If

        If Not dtrSan Is Nothing Then
            dtrSan.Close()
            dtrSan = Nothing
        End If
    End Function

    Private Sub InsertSanzione(ByVal IdSanzione As Integer)
        'Aggiunto il 10/10/2008 da Simona Cordella
        'A secondo del tipo di sanzione selezionato, inserisco in TVerificheSanzioniEnte o TVerificheSanzioniProgetto
        ' TIPOLOGIA SANZIONE :
        ' 0: ente
        ' 1: progetto
        Dim dtrSan As SqlClient.SqlDataReader
        Dim CmdGenerico As SqlClient.SqlCommand
        Dim Strsql As String
        Dim intTipologia As Integer

        If Not dtrSan Is Nothing Then
            dtrSan.Close()
            dtrSan = Nothing
        End If
        Strsql = "SELECT Tipologia FROM  TVerificheSanzioni where idsanzione =" & IdSanzione & " "
        dtrSan = ClsServer.CreaDatareader(Strsql, Session("conn"))
        If dtrSan.HasRows = True Then
            dtrSan.Read()
            intTipologia = dtrSan("Tipologia")
            If Not dtrSan Is Nothing Then
                dtrSan.Close()
                dtrSan = Nothing
            End If
            If intTipologia = 0 Then 'insert in TVerificheSanzioniEnte
                Strsql = "INSERT INTO TVerificheSanzioniEnte (IDEnte,IDSanzione,IDVerifica)"
                Strsql = Strsql & " VALUES (" & txtIdEnte.Value & " ," & IdSanzione & " ," & txtIdVerifica.Text & ")"
            Else 'TVerificheSanzioniProgetto 
                Strsql = "INSERT INTO TVerificheSanzioniProgetto (IDAttività, IDSanzione, IDVerifica)"
                Strsql = Strsql & " VALUES (" & Session("IDAttività") & "," & IdSanzione & " ," & txtIdVerifica.Text & ")"
            End If
            CmdGenerico = ClsServer.EseguiSqlClient(Strsql, Session("conn"))
        End If
    End Sub

    Private Sub InsertTVerifiche()
        Dim strNull As String = "Null"
        Dim CmdGenerico As SqlClient.SqlCommand
        Dim strsql As String
        Dim intStatoVerifiche As Integer

        intStatoVerifiche = ImpostaStato(LblStatoVerifica.Text)
        strsql = " INSERT INTO TVerifiche (IDStatoVerifica,Tipologia,IdRegCompetenza,CodiceFascicolo,IDFascicolo,DescrFascicolo,"
        strsql &= " DataProtInvioLetteraContestazione, NProtInvioLetteraContestazione, DataRicezioneLetteraContestazione, "
        strsql &= " DataRispostaLetteraContestazione,DataProtRispostaLetteraContestazion,NProtRispostaLetteraContestazion,"
        strsql &= " DataProtTrasmissioneSanzione,NProtTrasmissioneSanzione,DataEsecuzioneSanzione,DataProtEsecuzioneSanzione,"
        strsql &= " NProtEsecuzioneSanzione,DataProtChiusuraContestazione,NProtChiusuraContestazione,UserUltimaModifica,"
        strsql &= " DataUltimaModifica,Note,DataProtLetteraContestazioneDG,NProtLetteraContestazioneDG,"
        strsql &= " DataProtTrasmissioneServizi,NProtTrasmissioneServizi, DataProtSegnalazioneUNSC, NProtSegnalazioneUNSC, DataProtRicezioneSegnalazione, NProtRicezioneSegnalazione, DataProtApprovazioneSegnalazione, NProtApprovazioneSegnalazione, IDServizio, IDEnte, IDAttivita, IDEnteSedeAttuazione,"
        strsql &= " IDDocInterno, CodiceDocInterno,FileDocInterno,DataSegnalazione)"
        strsql &= " VALUES ("
        strsql &= "  " & intStatoVerifiche & " , 3," & Session("IdRegioneCompetenzaUtente") & ","
        If TxtCodiceFascicolo.Text <> "" Then
            strsql &= " '" & TxtCodiceFascicolo.Text & "','" & TxtCodiceFasc.Value & "','" & txtDescFasc.Text.Replace("'", "''") & "' , "
        Else
            strsql &= " " & strNull & ", " & strNull & ", " & strNull & ",  "
        End If
        If txtDataProtocolloInvioLetteraContestazione.Text <> "" Then
            strsql &= "'" & txtDataProtocolloInvioLetteraContestazione.Text & "', "
        Else
            strsql &= "" & strNull & ", "
        End If
        If TxtNumeroProtocolloInvioLettContestazione.Text <> "" Then
            strsql &= "'" & TxtNumeroProtocolloInvioLettContestazione.Text & "', "
        Else
            strsql &= " " & strNull & ", "
        End If
        If txtDataRicezioneLetteraContestazione.Text <> "" Then
            strsql &= "'" & txtDataRicezioneLetteraContestazione.Text & "', "
        Else
            strsql &= "" & strNull & ", "
        End If
        If txtDataRispostaLetteraContestazione.Text <> "" Then
            strsql &= " '" & txtDataRispostaLetteraContestazione.Text & "', "
        Else
            strsql &= " " & strNull & ", "
        End If
        If txtDataProtocolloRispostaContestazione.Text <> "" Then
            strsql &= " '" & txtDataProtocolloRispostaContestazione.Text & "', "
        Else
            strsql &= "" & strNull & ","
        End If
        If TxtNumProtocolloRispostaContestazione.Text <> "" Then
            strsql &= " '" & TxtNumProtocolloRispostaContestazione.Text & "', "
        Else
            strsql &= " " & strNull & ","
        End If
        If txtDataProtocolloTrasmissioneSanzione.Text <> "" Then
            strsql &= " '" & txtDataProtocolloTrasmissioneSanzione.Text & "', "
        Else
            strsql &= " " & strNull & ","
        End If
        If TxtNumeroProtocolloTrasmissioneSanzione.Text <> "" Then
            strsql &= " '" & TxtNumeroProtocolloTrasmissioneSanzione.Text & "', "
        Else
            strsql &= " " & strNull & ", "
        End If
        If txtDataEsecuzioneSanzione.Text <> "" Then
            strsql &= " '" & txtDataEsecuzioneSanzione.Text & "' , "
        Else
            strsql &= " " & strNull & " , "
        End If
        If txtDataProtocolloEsecuzioneSanzione.Text <> "" Then
            strsql &= " '" & txtDataProtocolloEsecuzioneSanzione.Text & "', "
        Else
            strsql &= " " & strNull & ", "
        End If
        If TxtNumeroProtocolloEsecuzioneSanzione.Text <> "" Then
            strsql &= " '" & TxtNumeroProtocolloEsecuzioneSanzione.Text & "',"
        Else
            strsql &= " " & strNull & ","
        End If
        'aggiunti il 06/11/2007 da simona cordella
        If TxtDataProtChiusuraContestazione.Text <> "" Then
            strsql &= " '" & TxtDataProtChiusuraContestazione.Text & "', "
        Else
            strsql &= "  " & strNull & ", "
        End If
        If TxtNumProtChiusuraContestazione.Text <> "" Then
            strsql &= " '" & TxtNumProtChiusuraContestazione.Text & "',"
        Else
            strsql &= " " & strNull & ","
        End If
        strsql &= " '" & Session("Utente") & "', "
        strsql &= " getdate(), "
        strsql &= " '" & Replace(TxtNote.Text, "'", "''") & "', "
        '*** agg da sc il 14/10/2010 gestione numero e data protocollo trasmissione al D.G per le contestate
        ' scarico la data protocollo tasmissione al d.g.
        If txtDataProtLetteraContestazioneDG.Text <> "" Then
            strsql &= " '" & txtDataProtLetteraContestazioneDG.Text & "', "
        Else
            strsql &= "  " & strNull & ", "
        End If
        If txtNProtLetteraContestazioneDG.Text <> "" Then
            strsql &= " '" & txtNProtLetteraContestazioneDG.Text & "' ,"
        Else
            strsql &= " " & strNull & " ,"
        End If
        '****
        '*** agg da sc il 18/10/2010 gestione numero e data protocollo trasmissione servizi per le sanzionate
        ' scarico la data protocollo trasmissione servizi 
        If txtDataProtocolloTrasmServizi.Text <> "" Then
            strsql &= "  '" & txtDataProtocolloTrasmServizi.Text & "', "
        Else
            strsql &= "  " & strNull & ", "
        End If
        If txtNProtocolloTrasmServizi.Text <> "" Then
            strsql &= " '" & txtNProtocolloTrasmServizi.Text & "', "
        Else
            strsql &= " " & strNull & ", "
        End If
        '****
        'DataProtSegnalazioneUNSC, NProtSegnalazioneUNSC, 
        If txtDataProtSegnalazione.Text <> "" Then
            strsql &= "  '" & txtDataProtSegnalazione.Text & "', "
        Else
            strsql &= "  " & strNull & ", "
        End If
        If txtNumProtSegnalazione.Text <> "" Then
            strsql &= " '" & txtNumProtSegnalazione.Text & "', "
        Else
            strsql &= " " & strNull & ", "
        End If
        'DataProtRicezioneSegnalazione , NProtRicezioneSegnalazione, 
        If txtDataProtRicSegnalazione.Text <> "" Then
            strsql &= "  '" & txtDataProtRicSegnalazione.Text & "', "
        Else
            strsql &= "  " & strNull & ", "
        End If
        If TxtNumProtRicSegnalazione.Text <> "" Then
            strsql &= " '" & TxtNumProtRicSegnalazione.Text & "', "
        Else
            strsql &= " " & strNull & ", "
        End If
        'DataProtApprovazioneSegnalazione, NProtApprovazioneSegnalazione, 
        If TxtDataProtApprovazione.Text <> "" Then
            strsql &= "  '" & TxtDataProtApprovazione.Text & "', "
        Else
            strsql &= "  " & strNull & ", "
        End If
        If TxtNumProtApprovazione.Text <> "" Then
            strsql &= "  '" & TxtNumProtApprovazione.Text & "', "
        Else
            strsql &= "  " & strNull & ", "
        End If
        If ddlServizi.SelectedValue <> 0 Then
            strsql &= " " & ddlServizi.SelectedValue & ", "
        Else
            strsql &= " " & strNull & ", "
        End If

        strsql &= " " & txtIdEnte.Value & " ,"
        If txtIdAttivita.Value <> "" Then
            strsql &= " " & txtIdAttivita.Value & ",  "
        Else
            strsql &= " " & strNull & " , "
        End If
        If txtIdEnteSedeAttuazione.Value <> "" Then
            strsql &= " " & txtIdEnteSedeAttuazione.Value & " , "
        Else
            strsql &= " " & strNull & " , "
        End If

        '*** aggiunto da simona cordella il 14/01/2013 gestione del documento interno ***
        If txtNumDocInterno.Text <> "" Then
            strsql &= " '" & Replace(txtIDDocInterno.Value, "%", "#") & "', '" & txtNumDocInterno.Text & "','" & txtNomeFile.Text.Replace("'", "''") & "', "
        Else
            strsql &= " " & strNull & ", " & strNull & ",  " & strNull & " , "
        End If
        '****
        'aggiunto il 28/10/2016
        If txtDataSegnalazione.Text <> "" Then
            strsql &= "'" & txtDataSegnalazione.Text & "' "
        Else
            strsql &= " " & strNull & "  "
        End If
        strsql &= " )"
        'IDServizio, IDEnte, IDAttivita, IDAttivitaEnteSedeAttuazione 

        CmdGenerico = ClsServer.EseguiSqlClient(strsql, Session("conn"))

    End Sub

    Private Sub AggiornaTVerifiche(ByVal IdVerifica As Integer)
        Dim strNull As String = "Null"
        Dim CmdGenerico As SqlClient.SqlCommand
        Dim strsql As String
        Dim intStatoVerifiche As Integer

        intStatoVerifiche = ImpostaStato(LblStatoVerifica.Text)
        strsql = "Update TVerifiche set "

        strsql &= "IDStatoVerifica=" & intStatoVerifiche & ","
        If TxtCodiceFascicolo.Text <> "" Then
            strsql &= "CodiceFascicolo = '" & TxtCodiceFascicolo.Text & "' ,IDFascicolo ='" & TxtCodiceFasc.Value & "' , DescrFascicolo ='" & txtDescFasc.Text.Replace("'", "''") & "' , "
        Else
            strsql &= " CodiceFascicolo = " & strNull & ", IDFascicolo = " & strNull & ", DescrFascicolo = " & strNull & ",  "
        End If
        'DataProtSegnalazioneUNSC, NProtSegnalazioneUNSC, 
        If txtDataProtSegnalazione.Text <> "" Then
            strsql &= " DataProtSegnalazioneUNSC = '" & txtDataProtSegnalazione.Text & "', "
        Else
            strsql &= " DataProtSegnalazioneUNSC = " & strNull & ", "
        End If
        If txtNumProtSegnalazione.Text <> "" Then
            strsql &= " NProtSegnalazioneUNSC ='" & txtNumProtSegnalazione.Text & "', "
        Else
            strsql &= " NProtSegnalazioneUNSC =" & strNull & ", "
        End If
        'DataProtRicezioneSegnalazione , NProtRicezioneSegnalazione, 
        If txtDataProtRicSegnalazione.Text <> "" Then
            strsql &= "  DataProtRicezioneSegnalazione ='" & txtDataProtRicSegnalazione.Text & "', "
        Else
            strsql &= "  DataProtRicezioneSegnalazione =" & strNull & ", "
        End If
        If TxtNumProtRicSegnalazione.Text <> "" Then
            strsql &= " NProtRicezioneSegnalazione ='" & TxtNumProtRicSegnalazione.Text & "', "
        Else
            strsql &= " NProtRicezioneSegnalazione =" & strNull & ", "
        End If
        'DataProtApprovazioneSegnalazione, NProtApprovazioneSegnalazione, 
        If TxtDataProtApprovazione.Text <> "" Then
            strsql &= " DataProtApprovazioneSegnalazione= '" & TxtDataProtApprovazione.Text & "', "
        Else
            strsql &= "  DataProtApprovazioneSegnalazione =" & strNull & ", "
        End If
        If TxtNumProtApprovazione.Text <> "" Then
            strsql &= " NProtApprovazioneSegnalazione= '" & TxtNumProtApprovazione.Text & "', "
        Else
            strsql &= " NProtApprovazioneSegnalazione = " & strNull & ", "
        End If
        If ddlServizi.SelectedValue <> 0 Then
            strsql &= " IDServizio = " & ddlServizi.SelectedValue & ", "
        Else
            strsql &= " IDServizio = " & strNull & ", "
        End If
        strsql &= " IDEnte = " & txtIdEnte.Value & " ,"
        If txtIdAttivita.Value <> "" Then
            strsql &= " IDAttivita =" & txtIdAttivita.Value & ",  "
        Else
            strsql &= " IDAttivita =" & strNull & " , "
        End If
        If txtIdEnteSedeAttuazione.Value <> "" Then
            strsql &= " IDEnteSedeAttuazione =" & txtIdEnteSedeAttuazione.Value & " , "
        Else
            strsql &= " IDEnteSedeAttuazione = " & strNull & " , "
        End If

        If txtDataProtocolloInvioLetteraContestazione.Text <> "" Then
            strsql &= "DataProtInvioLetteraContestazione ='" & txtDataProtocolloInvioLetteraContestazione.Text & "', "
        Else
            strsql &= "DataProtInvioLetteraContestazione =" & strNull & ", "
        End If
        If TxtNumeroProtocolloInvioLettContestazione.Text <> "" Then
            strsql &= "NProtInvioLetteraContestazione ='" & TxtNumeroProtocolloInvioLettContestazione.Text & "', "
        Else
            strsql &= "NProtInvioLetteraContestazione = " & strNull & ", "
        End If
        If txtDataRicezioneLetteraContestazione.Text <> "" Then
            strsql &= "DataRicezioneLetteraContestazione ='" & txtDataRicezioneLetteraContestazione.Text & "', "
        Else
            strsql &= "DataRicezioneLetteraContestazione =" & strNull & ", "
        End If
        If txtDataRispostaLetteraContestazione.Text <> "" Then
            strsql &= " DataRispostaLetteraContestazione = '" & txtDataRispostaLetteraContestazione.Text & "', "
        Else
            strsql &= " DataRispostaLetteraContestazione =" & strNull & ", "
        End If
        If txtDataProtocolloRispostaContestazione.Text <> "" Then
            strsql &= " DataProtRispostaLetteraContestazion = '" & txtDataProtocolloRispostaContestazione.Text & "', "
        Else
            strsql &= " DataProtRispostaLetteraContestazion = " & strNull & ","
        End If
        If TxtNumProtocolloRispostaContestazione.Text <> "" Then
            strsql &= " NProtRispostaLetteraContestazion = '" & TxtNumProtocolloRispostaContestazione.Text & "', "
        Else
            strsql &= " NProtRispostaLetteraContestazion = " & strNull & ","
        End If
        If txtDataProtocolloTrasmissioneSanzione.Text <> "" Then
            strsql &= " DataProtTrasmissioneSanzione = '" & txtDataProtocolloTrasmissioneSanzione.Text & "', "
        Else
            strsql &= " DataProtTrasmissioneSanzione = " & strNull & ","
        End If
        If TxtNumeroProtocolloTrasmissioneSanzione.Text <> "" Then
            strsql &= " NProtTrasmissioneSanzione = '" & TxtNumeroProtocolloTrasmissioneSanzione.Text & "', "
        Else
            strsql &= " NProtTrasmissioneSanzione =" & strNull & ", "
        End If
        If txtDataEsecuzioneSanzione.Text <> "" Then
            strsql &= " DataEsecuzioneSanzione='" & txtDataEsecuzioneSanzione.Text & "' , "
        Else
            strsql &= " DataEsecuzioneSanzione=" & strNull & " , "
        End If
        If txtDataProtocolloEsecuzioneSanzione.Text <> "" Then
            strsql &= " DataProtEsecuzioneSanzione ='" & txtDataProtocolloEsecuzioneSanzione.Text & "', "
        Else
            strsql &= " DataProtEsecuzioneSanzione = " & strNull & ", "
        End If
        If TxtNumeroProtocolloEsecuzioneSanzione.Text <> "" Then
            strsql &= " NProtEsecuzioneSanzione ='" & TxtNumeroProtocolloEsecuzioneSanzione.Text & "',"
        Else
            strsql &= " NProtEsecuzioneSanzione =" & strNull & ","
        End If
        'aggiunti il 06/11/2007 da simona cordella
        If TxtDataProtChiusuraContestazione.Text <> "" Then
            strsql &= " DataProtChiusuraContestazione ='" & TxtDataProtChiusuraContestazione.Text & "', "
        Else
            strsql &= " DataProtChiusuraContestazione = " & strNull & ", "
        End If
        If TxtNumProtChiusuraContestazione.Text <> "" Then
            strsql &= " NProtChiusuraContestazione ='" & TxtNumProtChiusuraContestazione.Text & "',"
        Else
            strsql &= " NProtChiusuraContestazione =" & strNull & ","
        End If
        strsql &= " UserUltimaModifica ='" & Session("Utente") & "', "
        strsql &= " DataUltimaModifica =getdate(), "
        strsql &= " Note ='" & Replace(TxtNote.Text, "'", "''") & "', "
        '*** agg da sc il 14/10/2010 gestione numero e data protocollo trasmissiona al D.G per le contestate
        ' scarico la data protocollo tasmissione al d.g.
        If txtDataProtLetteraContestazioneDG.Text <> "" Then
            strsql &= " DataProtLetteraContestazioneDG = '" & txtDataProtLetteraContestazioneDG.Text & "', "
        Else
            strsql &= " DataProtLetteraContestazioneDG = " & strNull & ", "
        End If
        If txtNProtLetteraContestazioneDG.Text <> "" Then
            strsql &= " NProtLetteraContestazioneDG ='" & txtNProtLetteraContestazioneDG.Text & "' ,"
        Else
            strsql &= " NProtLetteraContestazioneDG =" & strNull & " ,"
        End If
        '****
        '*** agg da sc il 18/10/2010 gestione numero e data protocollo trasmissione servizi per le sanzionate
        ' scarico la data protocollo trasmissione servizi 
        If txtDataProtocolloTrasmServizi.Text <> "" Then
            strsql &= " DataProtTrasmissioneServizi = '" & txtDataProtocolloTrasmServizi.Text & "', "
        Else
            strsql &= " DataProtTrasmissioneServizi = " & strNull & ", "
        End If
        If txtNProtocolloTrasmServizi.Text <> "" Then
            strsql &= " NProtTrasmissioneServizi ='" & txtNProtocolloTrasmServizi.Text & "', "
        Else
            strsql &= " NProtTrasmissioneServizi =" & strNull & " ,"
        End If
        '****
        '*** aggiunto da simona cordella il 14/01/2013 gestione del documento interno ***
        If txtNumDocInterno.Text <> "" Then
            strsql &= " IDDocInterno = '" & Replace(txtIDDocInterno.Value, "%", "#") & "', CodiceDocInterno ='" & txtNumDocInterno.Text & "' , FileDocInterno ='" & txtNomeFile.Text.Replace("'", "''") & "',  "
        Else
            strsql &= " IDDocInterno = " & strNull & ", CodiceDocInterno = " & strNull & ", FileDocInterno = " & strNull & " , "
        End If
        '****
        '*** aggiunto da simona cordella il 28/10/2016 ***
        If txtDataSegnalazione.Text <> "" Then
            strsql &= " DataSegnalazione ='" & txtDataSegnalazione.Text & "' "
        Else
            strsql &= " DataSegnalazione =" & strNull & " "
        End If
        strsql &= " Where idverifica = " & IdVerifica

        CmdGenerico = ClsServer.EseguiSqlClient(strsql, Session("conn"))
        lblmessaggio.Text = "Salvataggio eseguito con successo."
    End Sub
    Private Sub DisabilitaText()

        txtDataRicezioneLetteraContestazione.ReadOnly = True
        txtDataRispostaLetteraContestazione.ReadOnly = True

        txtDataEsecuzioneSanzione.ReadOnly = True
        txtDataProtocolloEsecuzioneSanzione.ReadOnly = True

        ddlUfficio.Enabled = False
        ddlCompetenze.Enabled = False

        'cmdSanzione.Enabled = True
        lkbSanzione.Enabled = False
    End Sub
    Private Sub DisabilitaImmaginiCalendario()
        ''imgDataInizioVerifica.Attributes.Add("style", "visibility: hidden")
        'disabilito immagini del calendario
        ''imgDataFineVerifica.Attributes.Add("style", "visibility: hidden")
        ''imgDataProtocolloRelazione.Attributes.Add("style", "visibility: hidden")
        ''''imgDataRicezioneLetteraContestazione.Attributes.Add("style", "visibility: hidden")
        txtDataRicezioneLetteraContestazione.Enabled = False


        '''''' imgDataRispostaLetteraContestazione.Attributes.Add("style", "visibility: hidden")
        txtDataRispostaLetteraContestazione.Enabled = False
        '''''  imgDataEsecuzioneSanzione.Attributes.Add("style", "visibility: hidden")
        txtDataEsecuzioneSanzione.Enabled = False
        'aggiunto il 06/12/2007 per regionalizzazione
        '''imgDataProtCredenziali.Attributes.Add("style", "visibility: hidden")
        '''imgDataProtocolloIncarico.Attributes.Add("style", "visibility: hidden")
        '''imgDataProtTrasDG.Attributes.Add("style", "visibility: hidden")
        '''imgDataProtocolloInvioChiusura.Attributes.Add("style", "visibility: hidden")
        ''' 






        txtDataProtocolloInvioLetteraContestazione.Enabled = False
        txtNProtLetteraContestazioneDG.Enabled = False
        txtDataProtocolloRispostaContestazione.Enabled = False
        TxtNumProtChiusuraContestazione.Enabled = False
        txtDataProtocolloTrasmissioneSanzione.Enabled = False
        txtDataProtocolloEsecuzioneSanzione.Enabled = False
        txtDataProtocolloTrasmServizi.Enabled = False
        txtDataProtLetteraContestazioneDG.Enabled = False


        TxtDataProtChiusuraContestazione.Enabled = False
        'imgDataProtocolloInvioLetteraContestazione.Attributes.Add("style", "visibility: hidden")
        'imgDataProtocolloInvioLetteraContestazioneDG.Attributes.Add("style", "visibility: hidden")
        'imgDataProtocolloRispostaContestazione.Attributes.Add("style", "visibility: hidden")
        'imgDataProtocolloChiusuraContestazione.Attributes.Add("style", "visibility: hidden")
        'imgDataProtocolloTrasmissioneSanzione.Attributes.Add("style", "visibility: hidden")
        'imgDataProtocolloEsecuzioneSanzione.Attributes.Add("style", "visibility: hidden")
        'imgDataProtocolloTrasmServizi.Attributes.Add("style", "visibility: hidden")

    End Sub
    Private Sub ColoraCampi()
        'coloro le celle disabilitate

        txtDataProtocolloInvioLetteraContestazione.BackColor = Color.LightGray
        'mod da sc 14/10/2010
        txtDataProtLetteraContestazioneDG.BackColor = Color.LightGray
        txtDataRicezioneLetteraContestazione.BackColor = Color.LightGray
        txtDataRispostaLetteraContestazione.BackColor = Color.LightGray
        txtDataProtocolloRispostaContestazione.BackColor = Color.LightGray
        txtDataProtocolloTrasmissioneSanzione.BackColor = Color.LightGray
        txtDataEsecuzioneSanzione.BackColor = Color.LightGray
        txtDataProtocolloEsecuzioneSanzione.BackColor = Color.LightGray
        '** agg da sc il 14/10/2010 gestione lettera contestazione al DG
        txtDataProtLetteraContestazioneDG.BackColor = Color.LightGray
        txtNProtLetteraContestazioneDG.BackColor = Color.LightGray
        '**
        '** agg da sc il 15/10/2010 gestione DATA protocollo trasmissione servizi
        txtDataProtocolloTrasmServizi.BackColor = Color.LightGray
        txtNProtocolloTrasmServizi.BackColor = Color.LightGray
        '**
        txtNProtLetteraContestazioneDG.BackColor = Color.LightGray
        TxtNumeroProtocolloInvioLettContestazione.BackColor = Color.LightGray
        txtDataProtocolloInvioLetteraContestazione.BackColor = Color.LightGray
        TxtNumProtocolloRispostaContestazione.BackColor = Color.LightGray
        TxtNumeroProtocolloTrasmissioneSanzione.BackColor = Color.LightGray
        TxtNumeroProtocolloEsecuzioneSanzione.BackColor = Color.LightGray
        ddlUfficio.BackColor = Color.LightGray
        ddlCompetenze.BackColor = Color.LightGray


        TxtDataProtChiusuraContestazione.BackColor = Color.LightGray

        TxtNumProtChiusuraContestazione.BackColor = Color.LightGray

        'agg. da sc il 15/10/2010 il fascicolo nn è più modificabile
        TxtCodiceFascicolo.BackColor = Color.LightGray
        txtDescFasc.BackColor = Color.LightGray

    End Sub


    Private Sub DisabilitaPulsantiStampa()
        '''CmdStampaCredenzialiIncarico.Enabled = False
        '''CmdStampaLettereIncarico.Enabled = False
        '''CmdStampaCredenzialiIncarico.Enabled = False
        '''CmdStampaLettereIncarico.Enabled = False
        '''CmdStampaCredenzialiIncaricoIGF.Enabled = False
        '''CmdStampaLettereIncaricoIGF.Enabled = False
        '''cmdRelazioneStand.Enabled = False
        '''cmdTrasmissione.Enabled = False
        '''CmdConclusione.Enabled = False
        '''CmdIrregolarita.Enabled = False
        cmdContestazione.Enabled = False
        CmdLetteraTrasm.Enabled = False
        cmdDiffida.Enabled = False
        CmdLetteraTrasmissioneProvv.Enabled = False
        '''CmdTrasmRich.Enabled = False

        CmdArchiviazione.Enabled = False
        CmdLetteraTrasmissioneProvv.Enabled = False
        CmdLETTERATRASMISPROVAISERVIZI.Enabled = False
        CmdTRASMISSIONEPROVVEDIMENTOREGIONE.Enabled = False
        cmdDiffida.Enabled = False
        CmdInterdizione.Enabled = False
        CmdRevoca.Enabled = False
        CmdCancellazione.Enabled = False
        CmdLetteraChiusContestazione.Enabled = False
    End Sub
    Private Function ImpostaStato(ByVal StatoPartenza As String) As Integer
        'Funzione che imposta lo stato della veriifca a secondo dei cdate che ho valorizzato
        'Le verifica su SEGNALAZIONE UNSC può ssumere solo i seguenti stati:
        'DA REGISTRARE: quando ancora non è stata inserita nel db
        'APERTA : quando ho indicato la DATA/NUM SEGNALAZIONE UNSC  e la DATA/NUM. RICEZIONE SEGNALAZIONE UNSC
        'ESEGUITA : quando ho indicato la DATA/NUM APPROVAZIONE SEGNALAZIONE UNSC o il DOCUMENTO INTERNO o DATA SEGNALAZIONE
        'CONTESTATA : quando ho indicato la DATA/NUM INVIO LETTERA CONTESTAZIONE
        'CHIUSA CONTESTATA : 
        'SANZIONATA :
        Select Case StatoPartenza
            Case "Da Registrare"
                'ImpostaStato = 5 'aperta
                If VerificaDatiStato(StatoPartenza) = False Then
                    ImpostaStato = 5 'aperta
                Else
                    ImpostaStato = 7 'eseguita
                End If
            Case "Aperta"
                If VerificaDatiStato(StatoPartenza) = False Then
                    ImpostaStato = 5 'aperta
                Else
                    ImpostaStato = 7 'eseguita
                End If
            Case "Eseguita"
                ' se chkNonEffettuata = TRUE  la verifica è CHIUSA NON EFFETTUATA se ha indicato la data è il protocollo al DG
                If VerificaDatiStato(StatoPartenza) = False Then
                    ImpostaStato = 7 'eseguita
                Else
                    ImpostaStato = 10 'contestata
                End If
            Case "Chiusa Positivamente"
                ImpostaStato = 8
            Case "Chiusa con Richiamo"
                ImpostaStato = 9
            Case "Contestata"
                If VerificaDatiStato(StatoPartenza) = False Then
                    ImpostaStato = 10 'contestata
                Else
                    If ControlloSanzioniCompleto(CInt(txtIdVerifica.Text)) = True Then
                        ImpostaStato = 11 'sanzionata
                    Else
                        ImpostaStato = 10 'contestata
                    End If
                End If
            Case "Sanzionata"
                ImpostaStato = 11
            Case "Sospesa"
                ImpostaStato = 12
            Case "Non Eseguita"
                ImpostaStato = 13
            Case "Chiusa Contestata"
                ImpostaStato = 14
        End Select
        'ImpostaStato = intStato
        Return ImpostaStato
    End Function

    Private Function VerificaDatiStato(ByVal StatoPartenza As String) As Boolean
        'Creata da Simona Cordella
        'Verifico se i campi obbligatori per lo stato Apert sono stati valorizzati
        VerificaDatiStato = True

        Select Case StatoPartenza
            Case "Da Registrare"
                If txtDataSegnalazione.Text = "" And txtNumDocInterno.Text = "" And TxtDataProtApprovazione.Text = "" And TxtNumProtApprovazione.Text = "" Then
                    VerificaDatiStato = False
                End If
                'If txtNumDocInterno.Text = "" Then
                '    VerificaDatiStato = False
                'End If
            Case "Aperta"
                If txtDataSegnalazione.Text = "" And txtNumDocInterno.Text = "" And TxtDataProtApprovazione.Text = "" And TxtNumProtApprovazione.Text = "" Then
                    VerificaDatiStato = False
                End If
            Case "In Esecuzione"
            Case "Eseguita"
                'If txtDataProtLetteraContestazioneDG.Text = "" And txtNProtLetteraContestazioneDG.Text = "" Then
                '    VerificaDatiStato = False
                'End If
                If txtDataProtocolloInvioLetteraContestazione.Text = "" And TxtNumeroProtocolloInvioLettContestazione.Text = "" Then
                    VerificaDatiStato = False
                End If
            Case "Contestata"
                ''la verifica passa da contestata a sanzionata se indivo la txtDataProtocolloEsecuzioneSanzione
                If txtDataProtocolloEsecuzioneSanzione.Text = "" Or TxtNumeroProtocolloEsecuzioneSanzione.Text = "" Then
                    VerificaDatiStato = False
                End If
                If txtDataProtocolloEsecuzioneSanzione.Text <> "" And TxtNumeroProtocolloEsecuzioneSanzione.Text = "" Then
                    VerificaDatiStato = False
                End If
                If txtDataProtocolloEsecuzioneSanzione.Text = "" And TxtNumeroProtocolloEsecuzioneSanzione.Text <> "" Then
                    VerificaDatiStato = False
                End If
        End Select
        Return VerificaDatiStato
    End Function
    Function StampaDocumentiRelazioni(ByVal intidverificaassociata As Integer, ByVal NomeFile As String) As String
        Dim xStr As String
        Dim xLinea As String
        Dim Writer As StreamWriter
        Dim Reader As StreamReader
        Dim strPercorsoFile As String
        Dim strsql As String
        Dim strDataOdierna As String
        Dim strNomeFile As String

        Try
            'chiudo il datareader
            If Not dtrLeggiDati Is Nothing Then
                dtrLeggiDati.Close()
                dtrLeggiDati = Nothing
            End If

            'prendo la data dal server
            dtrLeggiDati = ClsServer.CreaDatareader("select getdate() as dataOggi", Session("conn"))
            dtrLeggiDati.Read()
            'passo la data odierna ad una variabile locale
            strDataOdierna = IIf(Len(CStr(Day(dtrLeggiDati("dataOggi")))) < 2, "0" & CStr(Day(dtrLeggiDati("dataOggi"))), CStr(Day(dtrLeggiDati("dataOggi")))) & "/" & IIf(Len(CStr(Month(dtrLeggiDati("dataOggi")))) < 2, "0" & CStr(Month(dtrLeggiDati("dataOggi"))), CStr(Month(dtrLeggiDati("dataOggi")))) & "/" & CStr(Year(dtrLeggiDati("dataOggi")))

            Call DatiRelazioni(intidverificaassociata)

            If dtrLeggiDati.HasRows = True Then
                dtrLeggiDati.Read()
                'creo il nome del file
                strNomeFile = NomeFile & CStr(Session("IdEnte")) & CStr(Session("Username")) & CStr(Day(Now)) & CStr(Month(Now)) & CStr(Year(Now)) & Hour(Now) & Minute(Now) & Second(Now) & ".doc"
                'creo il percorso del file da salvare
                strPercorsoFile = Server.MapPath("./documentazione/") & strNomeFile

                'apro il file che fa da template
                Reader = New StreamReader(Server.MapPath("./documentazione/master/Naz/" & Session("Path") & NomeFile & ".rtf"))

                Writer = New StreamWriter(strPercorsoFile, True, System.Text.Encoding.Default)

                'Writer.WriteLine("{\rtf1")


                xLinea = Reader.ReadLine()


                'Dim strDataCostituzione As String = dtrLeggiDati("DataCostituzione")

                Dim strdataProt As String = IIf(Not IsDBNull(dtrLeggiDati("DataProtIncarico")), dtrLeggiDati("DataProtIncarico"), "")
                Dim strnumprot As String = IIf(Not IsDBNull(dtrLeggiDati("NprotIncarico")), dtrLeggiDati("NprotIncarico"), "")
                Dim cognome As String = dtrLeggiDati("Cognome")
                Dim nome As String = dtrLeggiDati("Nome")
                Dim strAnagrafico As String = cognome & " " & nome
                Dim DataInizioVerifica As String = IIf(Not IsDBNull(dtrLeggiDati("DataInizioVerifica")), dtrLeggiDati("DataInizioVerifica"), "")
                Dim DataFineVerifica As String = IIf(Not IsDBNull(dtrLeggiDati("DataFineVerifica")), dtrLeggiDati("DataFineVerifica"), "")
                Dim Bando As String = dtrLeggiDati("Bando")
                Dim settore As String = dtrLeggiDati("Settore")
                Dim Area As String = dtrLeggiDati("Area")
                Dim Nvol As String = dtrLeggiDati("NumeroVolontari")
                Dim datarispostaente As String = IIf(Not IsDBNull(dtrLeggiDati("DataProtRispostaLetteraContestazion")), dtrLeggiDati("DataProtRispostaLetteraContestazion"), "")
                Dim DataInvioFax As String = IIf(Not IsDBNull(dtrLeggiDati("DataInvioFax")), dtrLeggiDati("DataInvioFax"), "")
                Dim DataTrasmissioneSanzione As String = IIf(Not IsDBNull(dtrLeggiDati("DataTrasmissioneSanzione")), dtrLeggiDati("DataTrasmissioneSanzione"), "")
                Dim DataProtTrasmissioneSanzione As String = IIf(Not IsDBNull(dtrLeggiDati("DataProtTrasmissioneSanzione")), dtrLeggiDati("DataProtTrasmissioneSanzione"), "")
                Dim Classe As String = dtrLeggiDati("Classe")

                'dati figlio
                Dim EnteFiglio As String = dtrLeggiDati("EnteFiglio")
                Dim Civico As String = dtrLeggiDati("Civico")
                Dim Indirizzo As String = dtrLeggiDati("Indirizzo")
                Dim strCap As String = dtrLeggiDati("Cap")
                Dim strComune As String = dtrLeggiDati("Comune")
                Dim ProvinciaBreve As String = dtrLeggiDati("DescrAbb")


                'dati padre
                Dim strDenominazioneEnte As String = dtrLeggiDati("Denominazione")
                Dim strCodiceSede As String = dtrLeggiDati("CodiceSede")
                Dim strCodiceEnte As String = dtrLeggiDati("CodiceRegione")
                Dim strCodiceProgetto As String = dtrLeggiDati("CodiceEnte")
                Dim strtitolo As String = dtrLeggiDati("Titolo")
                Dim IndirizzoLegale As String = dtrLeggiDati("IndirizzoLegale")
                Dim CivicoLegale As String = dtrLeggiDati("CivicoLegale")
                Dim CapLegale As String = dtrLeggiDati("CapLegale")
                Dim ComuneLegale As String = dtrLeggiDati("ComuneLegale")
                Dim ProvinciaLegale As String = dtrLeggiDati("ProvinciaLegale")
                Dim AntodataIA As String = IIf(Not IsDBNull(dtrLeggiDati("datainizioattività")), dtrLeggiDati("datainizioattività"), "")

                Dim blnSalta As Boolean
                Call caricarequisiti(intidverificaassociata)

                While xLinea <> ""
                    blnSalta = False
                    xLinea = Replace(xLinea, "<DataOdierna>", strDataOdierna)
                    xLinea = Replace(xLinea, "<DenominazioneEnte>", strDenominazioneEnte)
                    xLinea = Replace(xLinea, "<CodiceEnte>", strCodiceEnte)
                    xLinea = Replace(xLinea, "<Titolo>", strtitolo)
                    xLinea = Replace(xLinea, "<Cap>", strCap)
                    xLinea = Replace(xLinea, "<DataProt>", strdataProt)
                    xLinea = Replace(xLinea, "<Nprot>", strnumprot)
                    xLinea = Replace(xLinea, "<CognomeNome>", strAnagrafico)
                    xLinea = Replace(xLinea, "<CodiceSEDE>", strCodiceSede)
                    xLinea = Replace(xLinea, "<EnteFiglio>", EnteFiglio)
                    xLinea = Replace(xLinea, "<Comune>", strComune)
                    xLinea = Replace(xLinea, "<Indirizzo>", Indirizzo)
                    xLinea = Replace(xLinea, "<Civico>", Civico)
                    xLinea = Replace(xLinea, "<DescrAbb>", ProvinciaBreve)
                    xLinea = Replace(xLinea, "<IndirizzoLegale>", IndirizzoLegale)
                    xLinea = Replace(xLinea, "<CivicoLegale>", CivicoLegale)
                    xLinea = Replace(xLinea, "<CapLegale>", CapLegale)
                    xLinea = Replace(xLinea, "<ComuneLegale>", ComuneLegale)
                    xLinea = Replace(xLinea, "<ProvinciaLegale>", ProvinciaLegale)
                    xLinea = Replace(xLinea, "<DataInizioVerifica>", DataInizioVerifica)
                    xLinea = Replace(xLinea, "<DataFineVerifica>", DataFineVerifica)
                    xLinea = Replace(xLinea, "<CodiceProggetto>", strCodiceProgetto)
                    xLinea = Replace(xLinea, "<Bando>", Bando)
                    xLinea = Replace(xLinea, "<Settore>", settore)
                    xLinea = Replace(xLinea, "<Area>", Area)
                    xLinea = Replace(xLinea, "<Nvol>", Nvol)
                    xLinea = Replace(xLinea, "<datainizioattivita>", AntodataIA)
                    xLinea = Replace(xLinea, "<datarispostaente>", datarispostaente)
                    xLinea = Replace(xLinea, "<DataInvioFax>", DataInvioFax)
                    xLinea = Replace(xLinea, "<DataTrasmissioneSanzione>", DataTrasmissioneSanzione)
                    xLinea = Replace(xLinea, "<DataProtTrasmissioneSanzione>", DataProtTrasmissioneSanzione)
                    xLinea = Replace(xLinea, "<Classe>", Classe)
                    Dim intX As Integer



                    '-------------------------------  pezzo per ciclare si/no----------------------------------------


                    If InStr(xLinea, "<BreakPoint>") > 0 Then
                        blnSalta = True
                        xLinea = Replace(xLinea, "<BreakPoint>", "") & "\par"

                        If dtrLeggiDati.HasRows = True Then
                            intX = dtrLeggiDati.FieldCount

                            Dim flag As Integer
                            flag = 1
                            While dtrLeggiDati.Read

                                'Dim entrato As Boolean

                                'pippo = dtrLeggiDati("TipoRequisito")
                                'Writer.WriteLine(pippo)
                                Select Case dtrLeggiDati("idtiporequisito")
                                    Case 1
                                        If flag = 1 Then
                                            Writer.WriteLine("\trowd\trgaph70\trleft-70\trbrdrl\brdrs\brdrw10 ")
                                            Writer.WriteLine("\trbrdrt\brdrs\brdrw10 \trbrdrr\brdrs\brdrw10 ")
                                            Writer.WriteLine("\trbrdrb\brdrs\brdrw10 ")
                                            Writer.WriteLine("\trpaddl70\trpaddr70\trpaddfl3\trpaddfr3")
                                            Writer.WriteLine("\clbrdrl\brdrw10\brdrs\clbrdrt\brdrw10\brdrs\clbrdrr\brdrw10\brdrs\clbrdrb\brdrw10\brdrs ")
                                            Writer.WriteLine("\cellx10040\pard\intbl\nowidctlpar\ri500\qc\b\fs40\ " & dtrLeggiDati("TipoRequisito") & "\b0\cell\row\pard\f2\fs20")
                                            flag = 2
                                        End If
                                        Exit Select
                                    Case 2
                                        If flag = 2 Then
                                            Writer.WriteLine("\trowd\trgaph70\trleft-70\trbrdrl\brdrs\brdrw10 ")
                                            Writer.WriteLine("\trbrdrt\brdrs\brdrw10 \trbrdrr\brdrs\brdrw10 ")
                                            Writer.WriteLine("\trbrdrb\brdrs\brdrw10 ")
                                            Writer.WriteLine("\trpaddl70\trpaddr70\trpaddfl3\trpaddfr3")
                                            Writer.WriteLine("\clbrdrl\brdrw10\brdrs\clbrdrt\brdrw10\brdrs\clbrdrr\brdrw10\brdrs\clbrdrb\brdrw10\brdrs ")
                                            Writer.WriteLine("\cellx10040\pard\intbl\nowidctlpar\ri500\qc\b\fs40\ " & dtrLeggiDati("TipoRequisito") & "\b0\cell\row\pard\f2\fs20")
                                            flag = 3
                                        End If
                                        Exit Select
                                    Case 3
                                        If flag = 3 Then
                                            Writer.WriteLine("\trowd\trgaph70\trleft-70\trbrdrl\brdrs\brdrw10 ")
                                            Writer.WriteLine("\trbrdrt\brdrs\brdrw10 \trbrdrr\brdrs\brdrw10 ")
                                            Writer.WriteLine("\trbrdrb\brdrs\brdrw10 ")
                                            Writer.WriteLine("\trpaddl70\trpaddr70\trpaddfl3\trpaddfr3")
                                            Writer.WriteLine("\clbrdrl\brdrw10\brdrs\clbrdrt\brdrw10\brdrs\clbrdrr\brdrw10\brdrs\clbrdrb\brdrw10\brdrs ")
                                            Writer.WriteLine("\cellx10040\pard\intbl\nowidctlpar\ri500\qc\b\fs40\ " & dtrLeggiDati("TipoRequisito") & "\b0\cell\row\pard\f2\fs20")
                                            flag = 4
                                        End If
                                        Exit Select
                                    Case 4
                                        If flag = 4 Then
                                            Writer.WriteLine("\trowd\trgaph70\trleft-70\trbrdrl\brdrs\brdrw10 ")
                                            Writer.WriteLine("\trbrdrt\brdrs\brdrw10 \trbrdrr\brdrs\brdrw10 ")
                                            Writer.WriteLine("\trbrdrb\brdrs\brdrw10 ")
                                            Writer.WriteLine("\trpaddl70\trpaddr70\trpaddfl3\trpaddfr3")
                                            Writer.WriteLine("\clbrdrl\brdrw10\brdrs\clbrdrt\brdrw10\brdrs\clbrdrr\brdrw10\brdrs\clbrdrb\brdrw10\brdrs ")
                                            Writer.WriteLine("\cellx10040\pard\intbl\nowidctlpar\ri500\qc\b\fs40\ " & dtrLeggiDati("TipoRequisito") & "\b0\cell\row\pard\f2\fs20")
                                            flag = 5
                                        End If
                                        Exit Select
                                End Select

                                Dim esito As String
                                Select Case dtrLeggiDati("Esito")
                                    Case 1
                                        esito = "SI"
                                    Case 2
                                        esito = "NO"
                                    Case 3
                                        esito = "NON PREVISTO"
                                End Select
                                Writer.WriteLine("\trowd\trgaph70\trleft-70\trbrdrl\brdrs\brdrw10 ")
                                Writer.WriteLine("\trbrdrt\brdrs\brdrw10 \trbrdrr\brdrs\brdrw10 ")
                                Writer.WriteLine("\trbrdrb\brdrs\brdrw10 ")
                                Writer.WriteLine("\trpaddl70\trpaddr70\trpaddfl3\trpaddfr3")
                                Writer.WriteLine("\clbrdrl\brdrw10\brdrs\clbrdrt\brdrw10\brdrs\clbrdrr\brdrw10\brdrs\clbrdrb\brdrw10\brdrs ")
                                Writer.WriteLine("\cellx8000\clbrdrl\brdrw10\brdrs\clbrdrt\brdrw10\brdrs\clbrdrr\brdrw10\brdrs\clbrdrb\brdrw10\brdrs ")
                                Writer.WriteLine("\cellx10040\pard\intbl\nowidctlpar\ri500\qj\fs28\ " & dtrLeggiDati("Descrizione") & "\cell " & esito & "\cell\row\pard\f2\fs20")
                            End While
                            Writer.WriteLine("\par")
                        End If
                    End If
                    '-----------------------------------------------------------------------
                    Writer.WriteLine(xLinea)

                    xLinea = Reader.ReadLine()
                End While

                'While xLinea <> ""
                '    Writer.WriteLine(xLinea)
                '    Writer.WriteLine(xLinea)

                'End While

                'close the RTF string and file
                'Writer.WriteLine("}")
                Writer.Close()
                Writer = Nothing

                ''chiudo lo streaming in scrittura
                Reader.Close()
                Reader = Nothing

            End If

            'chiudo il datareader
            If Not dtrLeggiDati Is Nothing Then
                dtrLeggiDati.Close()
                dtrLeggiDati = Nothing
            End If

            StampaDocumentiRelazioni = "documentazione/" & strNomeFile

        Catch ex As Exception
            If Not dtrLeggiDati Is Nothing Then
                dtrLeggiDati.Close()
                dtrLeggiDati = Nothing
            End If

            Response.Write(ex.Message)
        End Try

        Response.Write("<SCRIPT>" & vbCrLf)
        Response.Write("window.open('" & StampaDocumentiRelazioni & "');" & vbCrLf)
        Response.Write("</SCRIPT>")

    End Function
    Sub DatiRelazioni(ByVal idverificaassociata As Int32)
        Dim strsql As String
        'chiudo il datareader
        If Not dtrLeggiDati Is Nothing Then
            dtrLeggiDati.Close()
            dtrLeggiDati = Nothing
        End If
        strsql = "select * from VW_VER_RICERCA_VERIFICHE "
        strsql = strsql & "where idverificheassociate =" & idverificaassociata ' Session("verificaselezionata")
        'eseguo la query e passo il risultato al datareader
        dtrLeggiDati = ClsServer.CreaDatareader(strsql, Session("conn"))
    End Sub

    Sub caricarequisiti(ByVal idverificaassociata As Int32)

        Dim strsql As String
        'chiudo il datareader
        If Not dtrLeggiDati Is Nothing Then
            dtrLeggiDati.Close()
            dtrLeggiDati = Nothing
        End If
        strsql = "select idtiporequisito,idrequisito,ESITO,DESCRIZIONE,TIPOREQUISITO from VW_VER_STAMPA_REQUISITI  "
        strsql = strsql & "where idverificheassociate =" & idverificaassociata & ""
        strsql = strsql & "  order by idtiporequisito,idrequisito "
        'eseguo la query e passo il risultato al datareader
        dtrLeggiDati = ClsServer.CreaDatareader(strsql, Session("conn"))

    End Sub
    Protected Sub cmdStampa_Click(sender As Object, e As EventArgs) Handles cmdStampa.Click
        'Dim idverifica As Int32
        'idverifica = Request.QueryString("IdVerifica")

        'Call StampaDocumentiVerifica(idverifica, "relazionecompleta")
    End Sub

    Sub DatiFigliverifica()
        Dim strsql As String
        'chiudo il datareader
        If Not dtrLeggiDati Is Nothing Then
            dtrLeggiDati.Close()
            dtrLeggiDati = Nothing
        End If
        strsql = "select * from VW_VER_RICERCA_VERIFICHE "
        strsql = strsql & "where idverifica =" & Request.QueryString("IdVerifica")
        TBLLeggiDati = ClsServer.CreaDataTable(strsql, False, Session("conn"))
    End Sub

    Private Sub CmdArchiviazione_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles CmdArchiviazione.Click
        'Dim idverifica As Int16
        'idverifica = Request.QueryString("IdVerifica")
        'Call StampaDocumentiGen(idverifica, "letteraarchiviazproced")

        Dim Documento As New GeneratoreModelli
        'sostituzione documento da letteraarchiviazproced con TrasmissioneSanzioneDG
        Response.Write("<SCRIPT>" & vbCrLf)
        ' Response.Write("window.open('" & Documento.MON_letteraarchiviazproced(ClsUtility.TrovaIDEnteDaIDVerifica(Request.QueryString("IdVerifica"), Session("conn")), Request.QueryString("IdVerifica"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn")) & "');" & vbCrLf)
        Response.Write("window.open('" & Documento.MON_TrasmissioneSanzioneDG(ClsUtility.TrovaIDEnteDaIDVerifica(Request.QueryString("IdVerifica"), Session("conn"), "SU"), Request.QueryString("IdVerifica"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn")) & "');" & vbCrLf)
        Response.Write("</SCRIPT>")
        'chiamo la proprietà della classe GeneratoreModelli che ritorna la path del documento creato
        Documento.Dispose()
    End Sub

    Private Sub CmdCancellazione_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles CmdCancellazione.Click
        'Dim idverifica As Int16
        'idverifica = Request.QueryString("IdVerifica")
        'Call StampaDocumentiGen(idverifica, "CANCELLAZIONE")

        Dim Documento As New GeneratoreModelli

        Response.Write("<SCRIPT>" & vbCrLf)
        Response.Write("window.open('" & Documento.MON_CANCELLAZIONE(ClsUtility.TrovaIDEnteDaIDVerifica(Request.QueryString("IdVerifica"), Session("conn"), "SU"), Request.QueryString("IdVerifica"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn")) & "');" & vbCrLf)
        Response.Write("</SCRIPT>")
        'chiamo la proprietà della classe GeneratoreModelli che ritorna la path del documento creato
        Documento.Dispose()
    End Sub

    Private Sub CmdInterdizione_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles CmdInterdizione.Click
        'Dim idverifica As Int16
        'idverifica = Request.QueryString("IdVerifica")
        'Call StampaDocumentiGen(idverifica, "INTERDIZIONE")

        Dim Documento As New GeneratoreModelli

        Response.Write("<SCRIPT>" & vbCrLf)
        Response.Write("window.open('" & Documento.MON_INTERDIZIONE(ClsUtility.TrovaIDEnteDaIDVerifica(Request.QueryString("IdVerifica"), Session("conn"), "SU"), Request.QueryString("IdVerifica"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn")) & "');" & vbCrLf)
        Response.Write("</SCRIPT>")
        'chiamo la proprietà della classe GeneratoreModelli che ritorna la path del documento creato
        Documento.Dispose()
    End Sub

    Private Sub CmdRevoca_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles CmdRevoca.Click
        'Dim idverifica As Int16
        'idverifica = Request.QueryString("IdVerifica")
        'Call StampaDocumentiGen(idverifica, "REVOCA")

        Dim Documento As New GeneratoreModelli

        Response.Write("<SCRIPT>" & vbCrLf)
        Response.Write("window.open('" & Documento.MON_REVOCA(ClsUtility.TrovaIDEnteDaIDVerifica(Request.QueryString("IdVerifica"), Session("conn"), "SU"), Request.QueryString("IdVerifica"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn")) & "');" & vbCrLf)
        Response.Write("</SCRIPT>")
        'chiamo la proprietà della classe GeneratoreModelli che ritorna la path del documento creato
        Documento.Dispose()
    End Sub
    Function StampaDocumentiGen2(ByVal intIdVERIFICA As Integer, ByVal NomeFile As String) As String
        Dim xStr As String
        Dim xLinea As String
        Dim Writer As StreamWriter
        Dim Reader As StreamReader
        Dim strPercorsoFile As String
        Dim strsql As String
        Dim strDataOdierna As String
        Dim strNomeFile As String

        Dim datafineattività As String

        If Not dtrLeggiDati Is Nothing Then
            dtrLeggiDati.Close()
            dtrLeggiDati = Nothing
        End If
        dtrLeggiDati = ClsServer.CreaDatareader("select top 1 datafineattività,titolo from attività where identepresentante = 14 order by datafineattività desc", Session("conn"))
        dtrLeggiDati.Read()
        datafineattività = dtrLeggiDati("datafineattività")
        If Not dtrLeggiDati Is Nothing Then
            dtrLeggiDati.Close()
            dtrLeggiDati = Nothing
        End If

        Try
            'chiudo il datareader
            If Not dtrLeggiDati Is Nothing Then
                dtrLeggiDati.Close()
                dtrLeggiDati = Nothing
            End If

            'prendo la data dal server
            dtrLeggiDati = ClsServer.CreaDatareader("select getdate() as dataOggi", Session("conn"))
            dtrLeggiDati.Read()
            'passo la data odierna ad una variabile locale
            strDataOdierna = IIf(Len(CStr(Day(dtrLeggiDati("dataOggi")))) < 2, "0" & CStr(Day(dtrLeggiDati("dataOggi"))), CStr(Day(dtrLeggiDati("dataOggi")))) & "/" & IIf(Len(CStr(Month(dtrLeggiDati("dataOggi")))) < 2, "0" & CStr(Month(dtrLeggiDati("dataOggi"))), CStr(Month(dtrLeggiDati("dataOggi")))) & "/" & CStr(Year(dtrLeggiDati("dataOggi")))

            Call Dati()

            If dtrLeggiDati.HasRows = True Then
                dtrLeggiDati.Read()
                'creo il nome del file
                strNomeFile = NomeFile & CStr(Session("IdEnte")) & CStr(Session("Username")) & CStr(Day(Now)) & CStr(Month(Now)) & CStr(Year(Now)) & Hour(Now) & Minute(Now) & Second(Now) & ".doc"
                'creo il percorso del file da salvare
                strPercorsoFile = Server.MapPath("./documentazione/") & strNomeFile

                'apro il file che fa da template
                Reader = New StreamReader(Server.MapPath("./documentazione/master/Naz/" & Session("Path") & NomeFile & ".rtf"))

                Writer = New StreamWriter(strPercorsoFile, True, System.Text.Encoding.Default)

                'Writer.WriteLine("{\rtf1")


                xLinea = Reader.ReadLine()


                'Dim strDataCostituzione As String = dtrLeggiDati("DataCostituzione")

                Dim strdataProt As String = IIf(Not IsDBNull(dtrLeggiDati("DataProtIncarico")), dtrLeggiDati("DataProtIncarico"), "")
                Dim strnumprot As String = IIf(Not IsDBNull(dtrLeggiDati("NprotIncarico")), dtrLeggiDati("NprotIncarico"), "")
                Dim cognome As String = dtrLeggiDati("Cognome")
                Dim nome As String = dtrLeggiDati("Nome")
                Dim strAnagrafico As String = cognome & " " & nome
                Dim DataInizioVerifica As String = IIf(Not IsDBNull(dtrLeggiDati("DataInizioVerifica")), dtrLeggiDati("DataInizioVerifica"), "")
                Dim DataFineVerifica As String = IIf(Not IsDBNull(dtrLeggiDati("DataFineVerifica")), dtrLeggiDati("DataFineVerifica"), "")
                Dim Bando As String = dtrLeggiDati("Bando")
                Dim settore As String = dtrLeggiDati("Settore")
                Dim Area As String = dtrLeggiDati("Area")
                Dim Nvol As String = dtrLeggiDati("NumeroVolontari")
                Dim datarispostaente As String = IIf(Not IsDBNull(dtrLeggiDati("DataProtRispostaLetteraContestazion")), dtrLeggiDati("DataProtRispostaLetteraContestazion"), "")
                Dim DataInvioFax As String = IIf(Not IsDBNull(dtrLeggiDati("DataInvioFax")), dtrLeggiDati("DataInvioFax"), "")
                Dim DataTrasmissioneSanzione As String = IIf(Not IsDBNull(dtrLeggiDati("DataTrasmissioneSanzione")), dtrLeggiDati("DataTrasmissioneSanzione"), "")
                Dim DataProtTrasmissioneSanzione As String = IIf(Not IsDBNull(dtrLeggiDati("DataProtTrasmissioneSanzione")), dtrLeggiDati("DataProtTrasmissioneSanzione"), "")


                'dati figlio
                Dim EnteFiglio As String = dtrLeggiDati("EnteFiglio")
                Dim Civico As String = dtrLeggiDati("Civico")
                Dim Indirizzo As String = dtrLeggiDati("Indirizzo")
                Dim strCap As String = dtrLeggiDati("Cap")
                Dim strComune As String = dtrLeggiDati("Comune")
                Dim ProvinciaBreve As String = dtrLeggiDati("DescrAbb")


                'dati padre
                Dim strDenominazioneEnte As String = dtrLeggiDati("Denominazione")
                Dim strCodiceSede As String = dtrLeggiDati("CodiceSede")
                Dim strCodiceEnte As String = dtrLeggiDati("CodiceRegione")
                Dim strCodiceProgetto As String = dtrLeggiDati("CodiceEnte")
                Dim strtitolo As String = dtrLeggiDati("Titolo")
                Dim IndirizzoLegale As String = dtrLeggiDati("IndirizzoLegale")
                Dim CivicoLegale As String = dtrLeggiDati("CivicoLegale")
                Dim CapLegale As String = dtrLeggiDati("CapLegale")
                Dim ComuneLegale As String = dtrLeggiDati("ComuneLegale")
                Dim ProvinciaLegale As String = dtrLeggiDati("ProvinciaLegale")
                Dim AntodataIA As String = IIf(Not IsDBNull(dtrLeggiDati("datainizioattività")), dtrLeggiDati("datainizioattività"), "")

                Dim GU As String = dtrLeggiDati("GU")
                Dim Classe As String = dtrLeggiDati("classe")
                Dim DataProtRelazione As String = IIf(Not IsDBNull(dtrLeggiDati("DataProtRelazione")), dtrLeggiDati("DataProtRelazione"), "")

                Dim DataProtCD As String = IIf(Not IsDBNull(dtrLeggiDati("DataProtCD")), dtrLeggiDati("DataProtCD"), "")
                Dim NProtCD As String = IIf(Not IsDBNull(dtrLeggiDati("NProtCD")), dtrLeggiDati("NProtCD"), "")
                Dim NProtRisLetCont As String = IIf(Not IsDBNull(dtrLeggiDati("NProtRispostaLetteraContestazion")), dtrLeggiDati("NProtRispostaLetteraContestazion"), "")
                Dim DataProtRisLetCont As String = IIf(Not IsDBNull(dtrLeggiDati("DataProtRispostaLetteraContestazion")), dtrLeggiDati("DataProtRispostaLetteraContestazion"), "")

                While xLinea <> ""
                    xLinea = Replace(xLinea, "<DataOdierna>", strDataOdierna)
                    xLinea = Replace(xLinea, "<DenominazioneEnte>", strDenominazioneEnte)
                    xLinea = Replace(xLinea, "<CodiceEnte>", strCodiceEnte)
                    xLinea = Replace(xLinea, "<Titolo>", strtitolo)
                    xLinea = Replace(xLinea, "<Cap>", strCap)
                    xLinea = Replace(xLinea, "<DataProt>", strdataProt)
                    xLinea = Replace(xLinea, "<Nprot>", strnumprot)
                    xLinea = Replace(xLinea, "<CognomeNome>", strAnagrafico)
                    xLinea = Replace(xLinea, "<CodiceSEDE>", strCodiceSede)
                    xLinea = Replace(xLinea, "<EnteFiglio>", EnteFiglio)
                    xLinea = Replace(xLinea, "<Comune>", strComune)
                    xLinea = Replace(xLinea, "<Indirizzo>", Indirizzo)
                    xLinea = Replace(xLinea, "<Civico>", Civico)
                    xLinea = Replace(xLinea, "<DescrAbb>", ProvinciaBreve)
                    xLinea = Replace(xLinea, "<IndirizzoLegale>", IndirizzoLegale)
                    xLinea = Replace(xLinea, "<CivicoLegale>", CivicoLegale)
                    xLinea = Replace(xLinea, "<CapLegale>", CapLegale)
                    xLinea = Replace(xLinea, "<ComuneLegale>", ComuneLegale)
                    xLinea = Replace(xLinea, "<ProvinciaLegale>", ProvinciaLegale)
                    xLinea = Replace(xLinea, "<DataInizioVerifica>", DataInizioVerifica)
                    xLinea = Replace(xLinea, "<DataFineVerifica>", DataFineVerifica)
                    xLinea = Replace(xLinea, "<CodiceProggetto>", strCodiceProgetto)
                    xLinea = Replace(xLinea, "<Bando>", Bando)
                    xLinea = Replace(xLinea, "<Settore>", settore)
                    xLinea = Replace(xLinea, "<Area>", Area)
                    xLinea = Replace(xLinea, "<Nvol>", Nvol)
                    xLinea = Replace(xLinea, "<datainizioattivita>", AntodataIA)
                    xLinea = Replace(xLinea, "<datarispostaente>", datarispostaente)
                    xLinea = Replace(xLinea, "<DataInvioFax>", DataInvioFax)
                    xLinea = Replace(xLinea, "<DataTrasmissioneSanzione>", DataTrasmissioneSanzione)
                    xLinea = Replace(xLinea, "<DataProtTrasmissioneSanzione>", DataProtTrasmissioneSanzione)
                    xLinea = Replace(xLinea, "<DFA>", datafineattività)
                    xLinea = Replace(xLinea, "<GU>", GU)
                    xLinea = Replace(xLinea, "<Classe>", Classe)
                    xLinea = Replace(xLinea, "<DataProtRelazione>", DataProtRelazione)
                    xLinea = Replace(xLinea, "<DataProtCD>", DataProtCD)
                    xLinea = Replace(xLinea, "<NProtCD>", NProtCD)
                    xLinea = Replace(xLinea, "<NProtRisLetCont>", NProtRisLetCont)
                    xLinea = Replace(xLinea, "<DataProtRisLetCont>", DataProtRisLetCont)
                    xLinea = Replace(xLinea, "<StringaFissa>", "Ufficio per il Servizio civile nazionale")

                    If InStr(xLinea, "<BreakPoint>") > 0 Then
                        xLinea = Replace(xLinea, "<BreakPoint>", "") & "\par"
                        Call DatiFigliverifica()
                        Dim num As String
                        Dim V As String
                        Dim Ps As String
                        Dim Pd As String
                        Dim StringaFissa As String
                        StringaFissa = "Ufficio del Servizio civile nazionale"
                        Ps = "("
                        Pd = ")"
                        V = ","
                        num = "Alla SEDE: "


                        For Each myRow In TBLLeggiDati.Rows
                            'num = num + 1
                            Writer.WriteLine("\trowd\trgaph70\trleft-70\trbrdrl\brdrs\brdrnone ")
                            Writer.WriteLine("\trbrdrt\brdrs\brdrnone \trbrdrr\brdrs\brdrnone ")
                            Writer.WriteLine("\trbrdrb\brdrs\brdrnone ")
                            Writer.WriteLine("\trpaddl70\trpaddr70\trpaddfl3\trpaddfr3")
                            Writer.WriteLine("\clbrdrl\brdrnone\brdrs\clbrdrt\brdrnone\brdrs\clbrdrr\brdrnone\brdrs\clbrdrb\brdrnone\brdrs ")
                            Writer.WriteLine("\cellx5240\clbrdrl\brdrnone\brdrs\clbrdrt\brdrnone\brdrs\clbrdrr\brdrnone\brdrs\clbrdrb\brdrnone\brdrs ")
                            Writer.WriteLine("\cellx9800\pard\nowidctl\intbl\ri500\f0\fs24\cell\par " & num & "" & myRow.Item("EnteFiglio") & " \par " & StringaFissa & "" & V & "" & " \par " & myRow.Item("Indirizzo") & "" & V & "" & myRow.Item("Civico") & "" & V & " \par " & myRow.Item("Comune") & "" & V & "" & Ps & "" & myRow.Item("DescrAbb") & "" & Pd & "\cell\row\pard\f0\fs24")

                            'Writer.WriteLine("\pard\f0\fs24\" & num & " " & myRow.Item("EnteFiglio") & " \par" & myRow.Item("Indirizzo") & " " " & myRow.Item("Civico") & " \par" & myRow.Item("Comune") & " " & myRow.Item("DescrAbb") & "\par\pard\f0\fs24")
                        Next
                        Writer.WriteLine("\par")
                    End If
                    '''''''''End If
                    Writer.WriteLine(xLinea)
                    xLinea = Reader.ReadLine()
                End While

                'close the RTF string and file
                'Writer.WriteLine("}")
                Writer.Close()
                Writer = Nothing

                ''chiudo lo streaming in scrittura
                Reader.Close()
                Reader = Nothing

            End If

            'chiudo il datareader
            If Not dtrLeggiDati Is Nothing Then
                dtrLeggiDati.Close()
                dtrLeggiDati = Nothing
            End If

            StampaDocumentiGen2 = "documentazione/" & strNomeFile

        Catch ex As Exception
            If Not dtrLeggiDati Is Nothing Then
                dtrLeggiDati.Close()
                dtrLeggiDati = Nothing
            End If

            Response.Write(ex.Message)
        End Try

        Response.Write("<SCRIPT>" & vbCrLf)
        Response.Write("window.open('" & StampaDocumentiGen2 & "');" & vbCrLf)
        Response.Write("</SCRIPT>")

    End Function

    Private Sub CmdStampaCredenzialiIncaricoIGF_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles CmdStampaCredenzialiIncaricoIGF.Click
        'Dim idverifica As Int16
        'idverifica = Request.QueryString("IdVerifica")
        'Call StampaDocumentiGen2(idverifica, "credenzialiIGF")

        Dim Documento As New GeneratoreModelli

        Response.Write("<SCRIPT>" & vbCrLf)
        Response.Write("window.open('" & Documento.MON_credenzialiIGF(ClsUtility.TrovaIDEnteDaIDVerifica(Request.QueryString("IdVerifica"), Session("conn")), Request.QueryString("IdVerifica"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn")) & "');" & vbCrLf)
        Response.Write("</SCRIPT>")
        'chiamo la proprietà della classe GeneratoreModelli che ritorna la path del documento creato
        Documento.Dispose()
    End Sub

    Private Sub CmdStampaLettereIncaricoIGF_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles CmdStampaLettereIncaricoIGF.Click
        'Dim idverifica As Int16
        'idverifica = Request.QueryString("IdVerifica")
        'Call StampaDocumentiGen(idverifica, "LetteradiincaricoIGF")

        Dim Documento As New GeneratoreModelli

        Response.Write("<SCRIPT>" & vbCrLf)
        Response.Write("window.open('" & Documento.MON_LetteradiincaricoIGF(ClsUtility.TrovaIDEnteDaIDVerifica(Request.QueryString("IdVerifica"), Session("conn")), Request.QueryString("IdVerifica"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn")) & "');" & vbCrLf)
        Response.Write("</SCRIPT>")
        'chiamo la proprietà della classe GeneratoreModelli che ritorna la path del documento creato
        Documento.Dispose()
    End Sub

    Private Sub CmdLetteraChiusContestazione_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles CmdLetteraChiusContestazione.Click
        'Dim idverifica As Int16
        'idverifica = Request.QueryString("IdVerifica")
        'Call StampaDocumentiGen2(idverifica, "CONTESTAZIONE CHIUSURA")

        Dim Documento As New GeneratoreModelli

        Response.Write("<SCRIPT>" & vbCrLf)
        Response.Write("window.open('" & Documento.MON_CONTESTAZIONECHIUSURA(ClsUtility.TrovaIDEnteDaIDVerifica(Request.QueryString("IdVerifica"), Session("conn"), "SU"), Request.QueryString("IdVerifica"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn")) & "');" & vbCrLf)
        Response.Write("</SCRIPT>")
        'chiamo la proprietà della classe GeneratoreModelli che ritorna la path del documento creato
        Documento.Dispose()
    End Sub
    Private Sub PersonalizzaFascicolo_Regionalizzazione()
        Dim blnValore As Boolean = False
        cmdSelFascicolo.Visible = blnValore
        cmdSelProtocollo0.Visible = blnValore
        cmdFascCanc.Visible = blnValore
        CmdSospendi.Visible = blnValore
        
        ImageSelDoc(blnValore)

        ImageProtolloRichiesta(blnValore)
        ImageProtolloAcquisizione(blnValore)
        ImageProtolloApprovazione(blnValore)

        ImageProtocolloTrasContestataDG(blnValore)
        ImageProtocolloInvioLettContestazione(blnValore)
        ImageProtocolloRispContestazione(blnValore)
        ImageProtocolloProtChiusuraContestazione(blnValore)



        ImageProtocolloTrasmissioneSanzione(blnValore)
        ImageProtocolloEsecuzioneSanzione(blnValore)
        ImageProtocolloTrasmServizi(blnValore)
        ImageProtocolloTrasmissioneSanzione(blnValore)
        ImageProtocolloEsecuzioneSanzione(blnValore)
        ImageProtocolloTrasmServizi(blnValore)
        imgSanzioneServizi.Visible = blnValore
    End Sub



    Private Sub PersonalizzaFascicolo_Regionalizzazione1(ByVal IdRegCompetenza As Integer, ByVal StatoVerifica As String)
        If IdRegCompetenza <> 22 Then
            'TxtCodiceFascicolo.ReadOnly = False
            'txtDescFasc.ReadOnly = False
            cmdSelFascicolo.Visible = False
            cmdSelProtocollo0.Visible = False
            cmdFascCanc.Visible = False

            'CREDENZIALI
            ImgProtolloCredenziali.Visible = False
            ImgApriAllegatiCredenziali.Visible = False
            ImgProtocollazioneCredenziali.Visible = False
            'IGF --> CmdStampaCredenzialiIncaricoIGF
            'PROTOCOLLO INCARICO
            cmdSc1SelProtocollo1.Visible = False
            cmdSc1Allegati1.Visible = False
            cmdNuovoFascioclo.Visible = False
            'IGF --> CmdStampaLettereIncaricoIGF
            'TRASMISSIONE AL DG
            ImgProtocolloTrasDG.Visible = False
            ImgApriAllegatiTrasDG.Visible = False
            ImgProtocollazioneTrasDG.Visible = False
            'LETTERA CHIUSURA 
            ImgProtocolloLettChiusura.Visible = False
            ImgApriAllegatiLettChiusura.Visible = False
            ImgProtocollazioneLettChiusura.Visible = False
            'INVIO LETTERA CONTESTAZIONE AL D.G. 15/10/2010
            ImgProtocolloTrasContestataDG.Visible = False
            ImgApriAllegatiTrasContestataDG.Visible = False
            ImgProtocollazioneTrasContestataDG.Visible = False
            'INVIO LETTERA CONTESTAZIONE 
            ImgProtocolloInvioLettContestazione.Visible = False
            ImgApriAllegatiInvioLettContestazione.Visible = False
            ImgProtocollazioneInvioLettContestazione.Visible = False
            'RISPOSTA LETTERA CONTESTAZIONE ( Controdeduzioni )
            ' ImgProtocolloRispContestazione.Visible = False
            ' ImgApriAllegatiRispContestazione.Visible = False
            ImgProtocollazioneRispContestazione.Visible = False
            'DATA PROTOCOLLO CHIUSURA LETTERA CONTESTAZIONE
            ImgProtocolloProtChiusuraContestazione.Visible = False
            ImgApriAllegatiProtChiusuraContestazione.Visible = False
            ImgProtocollazioneProtChiusuraContestazione.Visible = False
            'DATA PROTOCOLLO TRASMISSIONE SANZIONE
            ImgProtocolloTrasmissioneSanzione.Visible = False
            ImgApriAllegatiTrasmissioneSanzione.Visible = False
            ImgProtocollazioneTrasmissioneSanzione.Visible = False
            'DATA PROTOCOLLO ESECUZIONE SANZIONE
            ImgProtocolloEsecuzioneSanzione.Visible = False
            ImgApriAllegatiEsecuzioneSanzione.Visible = False
            ImgProtocollazioneEsecuzioneSanzione.Visible = False
            'DATA PROTOCOLLO TRASMISSIONE SERVIZIO(SANZIONE)
            ImgProtocolloTrasmServizi.Visible = False
            ImgApriAllegatiTrasmServizi.Visible = False
            ImgProtocollazioneTrasmServizi.Visible = False

        Else
            'AGG. DA sc 15/10/2010
            If StatoVerifica = "Aperta" Or StatoVerifica = "In Esecuzione" Or StatoVerifica = "Eseguita" Then
                cmdSelFascicolo.Visible = True
                'cmdSelProtocollo0.Visible = True
                cmdFascCanc.Visible = True
            Else
                cmdSelFascicolo.Visible = False
                'cmdSelProtocollo0.Visible = False
                cmdFascCanc.Visible = False
            End If

            TxtCodiceFascicolo.ReadOnly = True
            txtDescFasc.ReadOnly = True
            'mod il 20/05/2011 la consultazione dei documenti del fasciolo è sempre visibile in qualunque stato della verifica
            cmdSelProtocollo0.Visible = True

            'CREDENZIALI
            ImgProtolloCredenziali.Visible = True
            ImgApriAllegatiCredenziali.Visible = True
            ImgProtocollazioneCredenziali.Visible = True
            'IGF --> CmdStampaCredenzialiIncaricoIGF
            'PROTOCOLLO INCARICO
            cmdSc1SelProtocollo1.Visible = True
            cmdSc1Allegati1.Visible = True
            cmdNuovoFascioclo.Visible = True
            'IGF --> CmdStampaLettereIncaricoIGF
            'TRASMISSIONE AL DG
            ImgProtocolloTrasDG.Visible = True
            ImgApriAllegatiTrasDG.Visible = True
            ImgProtocollazioneTrasDG.Visible = True
            'LETTERA CHIUSURA 
            ImgProtocolloLettChiusura.Visible = True
            ImgApriAllegatiLettChiusura.Visible = True
            ImgProtocollazioneLettChiusura.Visible = True
            'INVIO LETTERA CONTESTAZIONE 
            ImgProtocolloInvioLettContestazione.Visible = True
            ImgApriAllegatiInvioLettContestazione.Visible = True
            ImgProtocollazioneInvioLettContestazione.Visible = True
            'RISPOSTA LETTERA CONTESTAZIONE ( Controdeduzioni )
            'ImgProtocolloRispContestazione.Visible = True
            ' ImgApriAllegatiRispContestazione.Visible = True
            ImgProtocollazioneRispContestazione.Visible = True
            'DATA PROTOCOLLO CHIUSURA LETTERA CONTESTAZIONE
            ImgProtocolloProtChiusuraContestazione.Visible = True
            ImgApriAllegatiProtChiusuraContestazione.Visible = True
            ImgProtocollazioneProtChiusuraContestazione.Visible = True
            'DATA PROTOCOLLO TRASMISSIONE SANZIONE
            ImgProtocolloTrasmissioneSanzione.Visible = True
            ImgApriAllegatiTrasmissioneSanzione.Visible = True
            ImgProtocollazioneTrasmissioneSanzione.Visible = True
            'DATA PROTOCOLLO ESECUZIONE SANZIONE
            ImgProtocolloEsecuzioneSanzione.Visible = True
            ImgApriAllegatiEsecuzioneSanzione.Visible = True
            ImgProtocollazioneEsecuzioneSanzione.Visible = True
            'DATA PROTOCOLLO TRASMISSIONE SERVIZIO(SANZIONE)
            ImgProtocolloTrasmServizi.Visible = True
            ImgApriAllegatiTrasmServizi.Visible = True
            ImgProtocollazioneTrasmServizi.Visible = True
        End If
    End Sub



    Private Sub cmdRipristina_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim strsql As String
        Dim CmdGenerico As SqlClient.SqlCommand

        strsql = "update tverifiche set idstatoverifica = 5 where idverifica = " & txtIdVerifica.Text
        CmdGenerico = ClsServer.EseguiSqlClient(strsql, Session("conn"))
        CaricaMaschera(Request.QueryString("StatoSegnalata"), CInt(txtIdVerifica.Text))
    End Sub

    Private Function ControlloCampi() As Boolean
        Dim campiValidi As Boolean = True
        Dim campoObbligatorio As String = "Il campo {0} è obbligatorio.<br/>"
        Dim campoOpzione As String = "E' necessario indicare la {0} o il {1} o il {2} .<br/>"
        lblErrore.Text = ""

        If Session("IdRegioneCompetenzaUtente") = 22 Then
            If (TxtCodiceFascicolo.Text = String.Empty) Then
                lblErrore.Text = lblErrore.Text + String.Format(campoObbligatorio, "Fascicolo")
                campiValidi = False
            End If
        End If
        If (ddlServizi.SelectedValue = 0) Then
            lblErrore.Text = lblErrore.Text + String.Format(campoObbligatorio, "Servizi")
            campiValidi = False
        End If
        If txtDataSegnalazione.Text = String.Empty Then
            If txtNumDocInterno.Text = String.Empty Then
                If (txtDataProtSegnalazione.Text = String.Empty Or txtDataProtRicSegnalazione.Text = String.Empty Or TxtDataProtApprovazione.Text = String.Empty) Then
                    lblErrore.Text = lblErrore.Text + String.Format(campoOpzione, "Data Segnalazione", "Documento Interno", "Protocollo Segnalazione")
                    campiValidi = False
                End If
            End If
        End If

        If txtEnte.Text = String.Empty Then
            lblErrore.Text = lblErrore.Text + String.Format(campoObbligatorio, "Ente")
            campiValidi = False
        End If

        Return campiValidi
    End Function

    Protected Sub cmdSalva_Click(sender As Object, e As EventArgs) Handles cmdSalva.Click
        Dim idModelloSanz As Integer
        Dim strsql As String
        Dim dtrIdSeg As SqlClient.SqlDataReader
        Dim intIdVerifica As Integer

        If ControlloCampi() = False Then
            lblErrore.Visible = True
            Exit Sub
            'Else
            '    lblErrore.Visible = False
        End If
        lblErrore.Text = ""

        If D1LTD2(txtDataProtocolloInvioLetteraContestazione.Text, txtDataProtLetteraContestazioneDG.Text) Then
            lblErrore.Text = "La Data Protocollo Lettera Contestazione deve essere maggiore o uguale alla Data Protocollo Contestazione al D.G."
            txtDataProtocolloInvioLetteraContestazione.Focus()
            Exit Sub
        End If

        If D1LTD2(txtDataRicezioneLetteraContestazione.Text, txtDataProtocolloInvioLetteraContestazione.Text) Then

            lblErrore.Text = "La Data Ricezione Lettera Contestazione deve essere maggiore o uguale alla Data Protocollo Invio Contestazione."
            txtDataRicezioneLetteraContestazione.Focus()
            Exit Sub
        End If

        If D1LTD2(TxtDataProtChiusuraContestazione.Text, txtDataProtocolloRispostaContestazione.Text) Then
            lblErrore.Text = "La Data Protocollo Chiusura Contestazione deve essere maggiore o uguale alla Data Protocollo Lettera Controdeduzioni."
            TxtDataProtChiusuraContestazione.Focus()
            Exit Sub
        End If

        If D1LTD2(txtDataProtocolloTrasmissioneSanzione.Text, txtDataProtocolloRispostaContestazione.Text) Then
            lblErrore.Text = "La Data Protocollo Trasmissione Sanzione al D.G. deve essere maggiore o uguale alla Data Protocollo Lettera Controdeduzioni."
            txtDataProtocolloTrasmissioneSanzione.Focus()
            Exit Sub
        End If

        If D1LTD2(txtDataProtocolloEsecuzioneSanzione.Text, txtDataProtocolloTrasmissioneSanzione.Text) Then
            lblErrore.Text = "La Data Protocollo Invio Sanzione deve essere maggiore o uguale alla Data Protocollo Trasmissione Sanzione al D.G."
            txtDataProtocolloEsecuzioneSanzione.Focus()
            Exit Sub
        End If



        If D1LTD2(txtDataProtocolloTrasmServizi.Text, txtDataProtocolloEsecuzioneSanzione.Text) Then
            lblErrore.Text = "La Data Protocollo Trasmissione Servizi deve essere maggiore o uguale alla Data Protocollo Esecuzione Sanzione."
            txtDataProtocolloTrasmServizi.Focus()
            Exit Sub
        End If


        If D1LTD2(txtDataEsecuzioneSanzione.Text, txtDataProtocolloTrasmServizi.Text) Then
            lblErrore.Text = "La Data Esecuzione Sanzione deve essere maggiore o uguale alla Data Protocollo Trasmissione Servizi."
            txtDataEsecuzioneSanzione.Focus()
            Exit Sub
        End If






        'se valorizzo la data e numero protocollo sanzione controllo se ho indicato la sanzione
        If (txtDataProtocolloTrasmissioneSanzione.Text <> "" Or TxtNumeroProtocolloTrasmissioneSanzione.Text <> "") Then
            'controllo se la data e numero protocollo srasmiossione al DG è prensete del DB
            If (txtDataProtocolloEsecuzioneSanzione.Text <> "" Or TxtNumeroProtocolloEsecuzioneSanzione.Text <> "") Then
                'controllo se ho applicato almeno una sanzione all'ente
                If ControlloSanzioniCompleto(CInt(txtIdVerifica.Text)) = False Then
                    'se non ho indicato nessuna sanzione non posso salvare i dati messaggio informativo
                    lblErrore.Text = "E' necessario indicare almeno una tipologia di sanzione per rendere la verifica sanzionata."
                    'cmdSanzione.Enabled = True
                    lkbSanzione.Enabled = True
                    Exit Sub
                End If
            Else
                'controllo se ho applicato almeno una sanzione all'ente ma nn ho indicato la data e numero invio sanzione
                If ControlloSanzioniCompleto(CInt(txtIdVerifica.Text)) = True Then
                    'se non ho indicato nessuna sanzione non posso salvare i dati messaggio informativo
                    lblErrore.Text = "E' necessario indicare la Data e Numero Protocollo invio Sanzione."
                    Exit Sub
                End If
            End If
        End If
        ' If Request.QueryString("StatoSegnalata") = "Modifica" Then
        If LblStatoVerifica.Text <> "Da Registrare" Then
            If Session("IdRegCompetenza") = 22 Then
                If Session("Sistema") = "Helios" Then
                    idModelloSanz = 94
                Else
                    idModelloSanz = 237
                End If
                If LblStatoVerifica.Text = "Sanzionata" Then
                    If ControlloSanzioneServizi(idModelloSanz, CInt(txtIdVerifica.Text)) = False Then
                        lblErrore.Text = "E' necessario indicare il Servizio."
                        Exit Sub
                    End If
                End If
            End If


            AggiornaTVerifiche(CInt(txtIdVerifica.Text))
            CaricaMaschera("Modifica", CInt(txtIdVerifica.Text))
        Else
            InsertTVerifiche()

            'strSql = "Select SCOPE_IDENTITY() as maxid from TVerificheSegnalazione"
            strsql = "Select SCOPE_IDENTITY() as IdVer "
            dtrIdSeg = ClsServer.CreaDatareader(strsql, Session("conn"))
            dtrIdSeg.Read()
            If dtrIdSeg.HasRows = True Then
                intIdVerifica = dtrIdSeg("IdVer")
            End If

            dtrIdSeg.Close()
            dtrIdSeg = Nothing
            AggiornaTVerifiche(intIdVerifica)
            CaricaMaschera("Modifica", intIdVerifica)
            lblmessaggio.Text = "Inserimento eseguito con successo."
            ' cmdSalva.Visible = False
            'LblStatoVerifica.Text = "Aperta"
        End If
    End Sub

    Private Sub cmdChiudi_Click(sender As Object, e As EventArgs) Handles cmdChiudi.Click
        If Request.QueryString("StatoSegnalata") = "Modifica" Then
            Session("VengoDa") = "VerificaRequisiti"
            Response.Redirect("ver_RicercaVerifichePerRequisiti.aspx?VengoDa= " & Session("VengoDa"))
        Else
            Response.Redirect("WfrmMain.aspx")
        End If
    End Sub

    Sub Abilito_DisabilitoProtocolloTrasmissioneDG_Positiva_PositivaConRichiamo(ByVal blnValoreTxt As Boolean, ByVal Visibility As String, ByVal Colore As Color)
        'Creato da simona cordella 25/11/2010
        'rendo visibile o meno la data inizio e fine verifica sempre finche nn viene
        'chiusa positivamente, chiusa con richiamo, contestata e sanzionata
        '' TxtDataProtTrasDG.ReadOnly = blnValoreTxt
        '' TxtNumProtTrasDG.ReadOnly = blnValoreTxt
        ''   TxtDataProtTrasDG.BackColor = Colore
        '' TxtNumProtTrasDG.BackColor = Colore
    End Sub

    Sub ReadOnlyProcolliSANZIONE(ByVal blnValore As Boolean, ByVal Visibility As String)
        'Creato da simona cordella 22/02/2011
        'rendo editabile o no i protocolli e le date relative alla sanzione
        'se sono Regione posso scrivere Numero e Data
        'se sono UNSC richiamo numero e protocollo da SIGED
        txtDataProtocolloTrasmissioneSanzione.Enabled = Not blnValore
        TxtNumeroProtocolloTrasmissioneSanzione.ReadOnly = blnValore
        txtDataProtocolloEsecuzioneSanzione.Enabled = Not blnValore
        TxtNumeroProtocolloEsecuzioneSanzione.ReadOnly = blnValore
        txtDataProtocolloRispostaContestazione.Enabled = Not blnValore
        TxtNumProtocolloRispostaContestazione.ReadOnly = blnValore
        '** agg da sc il 15/10/2010 gestione DATA protocollo trasmissione servizi
        txtDataProtocolloTrasmServizi.Enabled = Not blnValore
        txtNProtocolloTrasmServizi.ReadOnly = blnValore

        txtDataProtocolloRispostaContestazione.Enabled = False
        txtDataProtocolloTrasmissioneSanzione.Enabled = False
        txtDataProtocolloEsecuzioneSanzione.Enabled = False
        txtDataProtocolloTrasmServizi.Enabled = False



    End Sub

    Private Function ControlloSanzioniCompleto(ByVal IdVerifica As Integer) As Boolean
        'Creata da Simona Cordella il 24/02/2011
        'Sub controlla che sia stata inserita almeno una sanzione
        Dim strSql As String
        Dim dtrSanzione As SqlClient.SqlDataReader

        If Not dtrSanzione Is Nothing Then
            dtrSanzione.Close()
            dtrSanzione = Nothing
        End If

        strSql = " Select  * from tverifiche v " & _
                 " left join dbo.VW_SANZIONI_ENTE_Completa s on v.idverifica = s.idverifica " & _
                 " where s.idverifica = " & IdVerifica & ""
        dtrSanzione = ClsServer.CreaDatareader(strSql, Session("conn"))
        ControlloSanzioniCompleto = dtrSanzione.HasRows


        If Not dtrSanzione Is Nothing Then
            dtrSanzione.Close()
            dtrSanzione = Nothing
        End If
    End Function

    Private Function MakeParentTable(ByVal strquery As String) As DataSet
        '***Generata da Gianluigi Paesani in data:05/07/04
        '***Inizializzo e carico datatable 
        Dim dtrgenerico As Data.SqlClient.SqlDataReader
        ' Create a new DataTable.
        Dim myDataTable As DataTable = New DataTable
        ' Declare variables for DataColumn and DataRow objects.
        Dim myDataColumn As DataColumn
        Dim myDataRow As DataRow
        ' Create new DataColumn, set DataType, ColumnName and add to DataTable.    
        myDataColumn = New DataColumn
        myDataColumn.DataType = System.Type.GetType("System.Int64")
        myDataColumn.ColumnName = "id"
        myDataColumn.Caption = "id"
        myDataColumn.ReadOnly = True
        myDataColumn.Unique = True
        ' Add the Column to the DataColumnCollection.
        myDataTable.Columns.Add(myDataColumn)
        ' Create second column.
        myDataColumn = New DataColumn
        myDataColumn.DataType = System.Type.GetType("System.String")
        myDataColumn.ColumnName = "ParentItem"
        myDataColumn.AutoIncrement = False
        myDataColumn.Caption = "ParentItem"
        myDataColumn.ReadOnly = False
        myDataColumn.Unique = False
        ' Add the column to the table.
        myDataTable.Columns.Add(myDataColumn)
        dtrgenerico = ClsServer.CreaDatareader(strquery, Session("conn"))
        myDataRow = myDataTable.NewRow()
        myDataRow("id") = 0
        myDataRow("ParentItem") = ""
        myDataTable.Rows.Add(myDataRow)
        Do While dtrgenerico.Read
            myDataRow = myDataTable.NewRow()
            myDataRow("id") = dtrgenerico.GetValue(0)
            myDataRow("ParentItem") = dtrgenerico.GetValue(1)
            myDataTable.Rows.Add(myDataRow)
        Loop

        dtrgenerico.Close()
        dtrgenerico = Nothing

        MakeParentTable = New DataSet
        MakeParentTable.Tables.Add(myDataTable)
    End Function

    Protected Sub cmdChiusaContestata_Click(sender As Object, e As EventArgs) Handles cmdChiusaContestata.Click
        'Aggiunto il 11/10/2007 da Simona Cordella
        'Gestione Chiusura della VErifica Contesta -- idstato 14
        Dim strsql As String
        Dim CmdGenerico As SqlClient.SqlCommand

        'If Session("IdRegCompetenza") <> 22 Then
        '    If (txtDataProtocolloEsecuzioneSanzione.Text = "" Or TxtNumeroProtocolloEsecuzioneSanzione.Text = "") Then
        '        'controllo se ho applicato almeno una sanzione all'ente ma nn ho indicato la data e numero invio sanzione
        '        If ControlloSanzioniCompleto(CInt(txtIdVerifica.Text)) = True Then
        '            'se non ho indicato nessuna sanzione non posso salvare i dati messaggio informativo
        '            lblmessaggio.Text = "E' necessario indicare la Data e Numero Protocollo invio Sanzione."
        '            Exit Sub
        '        End If
        '    End If
        'End If

        strsql = "update tverifiche set idstatoverifica = 14 where idverifica = " & txtIdVerifica.Text
        CmdGenerico = ClsServer.EseguiSqlClient(strsql, Session("conn"))
        CaricaMaschera(Request.QueryString("StatoSegnalata"), CInt(txtIdVerifica.Text))
    End Sub

    Private Sub cmdSospendi_Click(sender As Object, e As EventArgs) Handles CmdSospendi.Click
        Dim strsql As String
        Dim CmdGenerico As SqlClient.SqlCommand

        strsql = "update tverifiche set idstatoverifica = 12 where idverifica = " & txtIdVerifica.Text
        CmdGenerico = ClsServer.EseguiSqlClient(strsql, Session("conn"))
        CaricaVerifiche(CInt(txtIdVerifica.Text))
    End Sub
    Private Sub VerificaCompetenzaEnte(ByVal idEnte As Integer)
        Dim strsql As String
        Dim dtrComp As SqlClient.SqlDataReader

        If Not dtrComp Is Nothing Then
            dtrComp.Close()
            dtrComp = Nothing
        End If
        ddlCompetenze.BackColor = Color.White
        ddlUfficio.BackColor = Color.White
        strsql = "SELECT   IdRegioneCompetenza FROM enti where idEnte =" & idEnte & ""
        dtrComp = ClsServer.CreaDatareader(strsql, Session("conn"))

        If dtrComp.HasRows = True Then
            dtrComp.Read()
            'modificato il 15/10/2010
            If dtrComp("IdRegioneCompetenza") = 22 Then
                '15/10/2010 IMPOSTO PER DEFAULT COMBO UFFICIO con DIRETTORE GENERALE se NAZIONALE
                ddlUfficio.SelectedValue = 1
                ddlCompetenze.SelectedValue = 22
            Else
                ddlCompetenze.Enabled = True
                ddlCompetenze.SelectedValue = dtrComp("IdRegioneCompetenza")
                ddlCompetenze.Enabled = False
                ddlUfficio.Enabled = False
                ddlUfficio.SelectedValue = 17
            End If
            '-- VECCHIA GESTIONE SE NAZIONALE ABILITAVO ddlUfficio, se RGIONALE blocco combo competenze CON REGIONE APPARTENENZA 
            'If dtrComp("IdRegioneCompetenza") = 22 Then
            '    'controllo competenza... id =22 nazionale --> ufficio unsc
            '    ddlUfficio.BackColor = Color.White
            '    'ddlUfficio.Enabled = True
            '    ddlCompetenze.BackColor = Color.White
            '    ddlCompetenze.Enabled = False

            'Else
            '    'regionale --> combo con regione in visualizzazione e bloccata
            '    ddlCompetenze.BackColor = Color.White
            '    ddlCompetenze.Enabled = True
            '    ddlCompetenze.SelectedValue = dtrComp("IdRegioneCompetenza")
            '    ddlCompetenze.Enabled = False
            '    ddlUfficio.BackColor = Color.White
            '    ddlUfficio.Enabled = False
            'End If
        End If
        If Not dtrComp Is Nothing Then
            dtrComp.Close()
            dtrComp = Nothing
        End If
    End Sub

    Sub Abilita_DisabilitaServizi(ByVal blnValore As Boolean, ByVal Colore As Color)
        ddlServizi.BackColor = Colore
        ddlServizi.Enabled = blnValore
        'txtServizi.Visible = True
        'txtServizi.BackColor = Colore
    End Sub

    Sub Abilita_DisabilitaProtocolloSegnalazione(ByVal Colore As Color, ByVal blnValore As Boolean)
        txtDataProtSegnalazione.BackColor = Colore
        txtNumProtSegnalazione.BackColor = Colore
        '  ImageProtolloRichiesta(blnValore)
        ' imcCancProtSeg.Attributes.Add("style", visible)

    End Sub

    Sub Abilita_DisabilitaProtocolloRicezioneSegnalazione(ByVal Colore As Color, ByVal blnValore As Boolean)
        txtDataProtRicSegnalazione.BackColor = Colore
        TxtNumProtRicSegnalazione.BackColor = Colore
        '(ImageProtolloAcquisizione(blnValore))

        'imgCancProtRicSeg.Attributes.Add("style", visible)
    End Sub

    Sub Abilita_DisabilitaProtocolloApprovazioneSegnalazione(ByVal Colore As Color, ByVal blnValore As Boolean)
        TxtDataProtApprovazione.BackColor = Colore
        TxtNumProtApprovazione.BackColor = Colore
        'ImageProtolloApprovazione(blnValore)
        ' imgCancProtAppSeg.Attributes.Add("style", visible)
    End Sub

    Sub Abilita_DisabilitaOggettoSegnalazione(ByVal blnValore As Boolean, ByVal Colore As Color)
        txtEnte.BackColor = Colore
        txtProgetto.BackColor = Colore
        txtSede.BackColor = Colore
        TxtEnteFiglio.BackColor = Colore
        imgRicercaEnte.Visible = blnValore
        ImgRicercaProgetto.Visible = blnValore
        imgRicercaSede.Visible = blnValore
    End Sub

    Sub Abilita_DisabilitaDocumentiInterni(ByVal blnValore As Boolean, ByVal Colore As Color)
        txtNumDocInterno.BackColor = Colore
        txtNomeFile.BackColor = Colore
        ImageSelDoc(blnValore)
        ' imgCancDoc.Attributes.Add("style", "visibility: hidden")
    End Sub
    Private Sub DocumentoInterno(ByVal CodiceAllegato As String)
        'Funzione  che consente di scaricare il documemto interno al fascicolo
        Dim SIGED As clsSiged
        Dim wsOut As WS_SIGeD.INDICE_ALLEGATO
        Dim objHLink As HyperLink
        Dim strNome As String
        Dim strCognome As String
        Dim strSQL As String
        Dim dsUser As DataSet
        Dim PathServerSiged As String
        Dim NomeFile As String
        Dim myPath As New System.Web.UI.Page
        Dim PathLocale As String
        Try
            'verifica che l'utente sia autorizzato all'accesso al sistema documentale
            strSQL = "Select Nome, Cognome From UtentiUNSC Where UserName='" & Session("Utente") & "'"
            dsUser = ClsServer.DataSetGenerico(strSQL, Session("conn"))

            If dsUser.Tables(0).Rows.Count <> 0 Then
                strNome = dsUser.Tables(0).Rows(0).Item("Nome")
                strCognome = dsUser.Tables(0).Rows(0).Item("Cognome")
            End If
            SIGED = New clsSiged("", strNome, strCognome)
            If SIGED.Codice_Esito <> 0 Then
                lblNomeFile.Text = SIGED.Esito
                Exit Sub
            End If
            'Ottiene il percorso per recuperare il file
            PathServerSiged = "\\" & ConfigurationSettings.AppSettings("SERVER_SIGED") & "\" & ConfigurationSettings.AppSettings("CARTELLA_SIGED")  '& "\" & pNomeFile

            wsOut = SIGED.SIGED_RestituisciDocumentoInterno(CodiceAllegato, "", PathServerSiged & "\" & Trim(NomeFile))
            If SIGED.SIGED_Codice_Esito(wsOut.ESITO) = 0 Then
                PathLocale = myPath.Server.MapPath("download\") & wsOut.DATI.NOMEFILE  '& NomeFile
                If File.Exists(PathLocale) = True Then
                    File.Delete(PathLocale)
                End If
                File.Copy(PathServerSiged & "\" & wsOut.DATI.NOMEFILE, Trim(PathLocale))

                imgDownolad.Visible = False
                hlScarica.Visible = True
                hlScarica.NavigateUrl = "download\" & wsOut.DATI.NOMEFILE
            Else
                lblmessaggio.Text = Mid(wsOut.ESITO, 6, Len(wsOut.ESITO))
                hlScarica.Visible = False
                imgDownolad.Visible = True
            End If

        Catch ex As Exception
            lblmessaggio.Text = "Errore imprevisto. Contattare l'assistenza."
            hlScarica.Visible = False
            imgDownolad.Visible = True
        Finally
            SIGED.SIGED_Chiudi_Autenticazione(strNome, strCognome)
        End Try
    End Sub

    Private Sub imgDownolad_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgDownolad.Click
        Dim IDDocumentoInterno As String
        IDDocumentoInterno = TxtCodiceFasc.Value & "#" & txtNumDocInterno.Text
        DocumentoInterno(IDDocumentoInterno)
    End Sub

    Private Sub imgCancDoc_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgCancDoc.Click
        If txtNumDocInterno.Text <> "" Then
            'If txtNumDocInterno.Style.Add = Color.White Then
            txtNumDocInterno.Text = ""
            txtIDDocInterno.Value = ""
            txtNomeFile.Text = ""
            imgDownolad.Visible = True
            hlScarica.Visible = False
            txtDataProtSegnalazione.BackColor = Color.White
            txtNumProtSegnalazione.BackColor = Color.White
            txtDataProtRicSegnalazione.BackColor = Color.White
            TxtNumProtRicSegnalazione.BackColor = Color.White
            TxtDataProtApprovazione.BackColor = Color.White
            TxtNumProtApprovazione.BackColor = Color.White
            'End If

        End If

    End Sub
    Private Sub PulisciCampiFascicolo()

        TxtCodiceFascicolo.Text = ""
        TxtCodiceFasc.Value = ""
        txtDescFasc.Text = ""


        TxtNumeroProtocolloInvioLettContestazione.Text = ""
        txtDataProtocolloInvioLetteraContestazione.Text = ""
        TxtNumProtocolloRispostaContestazione.Text = ""
        txtDataProtocolloRispostaContestazione.Text = ""
        TxtNumeroProtocolloTrasmissioneSanzione.Text = ""
        txtDataProtocolloTrasmissioneSanzione.Text = ""
        TxtNumeroProtocolloEsecuzioneSanzione.Text = ""
        txtDataProtocolloEsecuzioneSanzione.Text = ""
        txtDataProtLetteraContestazioneDG.Text = ""
        txtNProtLetteraContestazioneDG.Text = ""


    End Sub
    Protected Sub cmdFascCanc_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles cmdFascCanc.Click

        PulisciCampiFascicolo()

    End Sub



    Protected Sub imcCancProtSeg_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles imcCancProtSeg.Click

        If (txtDataProtSegnalazione.Text <> "") Then

            txtDataProtSegnalazione.Text = ""
            txtNumProtSegnalazione.Text = ""

        End If

    End Sub

    Protected Sub imgCancProtRicSeg_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles imgCancProtRicSeg.Click


        If (txtDataProtRicSegnalazione.Text <> "") Then

            txtDataProtRicSegnalazione.Text = ""
            TxtNumProtRicSegnalazione.Text = ""

        End If



    End Sub

    Protected Sub imgCancProtAppSeg_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles imgCancProtAppSeg.Click

        If (TxtDataProtApprovazione.Text <> "") Then

            TxtDataProtApprovazione.Text = ""
            TxtNumProtApprovazione.Text = ""

        End If


    End Sub

    Protected Sub cmdAnnullata_Click(sender As Object, e As EventArgs) Handles cmdAnnullata.Click
        Dim strsql As String
        Dim CmdGenerico As SqlClient.SqlCommand

        strsql = "update tverifiche set idstatoverifica = 16 where idverifica = " & txtIdVerifica.Text
        CmdGenerico = ClsServer.EseguiSqlClient(strsql, Session("conn"))
        CaricaVerifiche(CInt(txtIdVerifica.Text))
    End Sub

    Private Sub lkbSanzione_Click(sender As Object, e As System.EventArgs) Handles lkbSanzione.Click
        Response.Redirect("ver_Sanzione.aspx?Segnalata=SegnalataUNSC&NumProtTrasmSanzioneDG= " & TxtNumeroProtocolloTrasmissioneSanzione.Text & " &DataProtTrasmSanzioneDG= " & txtDataProtocolloTrasmissioneSanzione.Text & " &NumProtEsecSanzione= " & TxtNumeroProtocolloEsecuzioneSanzione.Text & " &DataProtEsecSanzione= " & txtDataProtocolloEsecuzioneSanzione.Text & " &idprogrammazione=" & Trim(Request.QueryString("idprogrammazione")) & "&VengoDa=" & Session("VengoDa") & "&IdEnte=" & Request.QueryString("Idente") & "&IdVerifica=" & Request.QueryString("IdVerifica") & "")
    End Sub




    '*** scheda informazioni generali
    Private Sub SchedaInformazioniGenerali(ByVal blnValore As Boolean, ByVal Colore As Color)
        Fascicolo(blnValore, Colore)
        Abilita_DisabilitaServizi(blnValore, Colore)
        'aggiunto il 15/01/2013 gestione del documento interno
        Abilita_DisabilitaDocumentiInterni(blnValore, Colore)
        Abilita_DisabilitaProtocolloSegnalazione(Colore, blnValore)
        Abilita_DisabilitaProtocolloRicezioneSegnalazione(Colore, blnValore)
        Abilita_DisabilitaProtocolloApprovazioneSegnalazione(Colore, blnValore)
        Abilita_DisabilitaOggettoSegnalazione(blnValore, Colore)
        ImageSelDoc(blnValore)
        ImageProtolloRichiesta(blnValore)
        ImageProtolloAcquisizione(blnValore)
        ImageProtolloApprovazione(blnValore)
        DataSegnalazione(blnValore, Colore)
        'DataCredenziali(blnValore, Colore)
        'DataIncarico(blnValore, Colore)
        'DataInizoFineVerifica(blnValore, Colore)
        'DataProtocolloRelazione(blnValore, Colore)
        'DataProtTrasDG(blnValore, Colore)
        'DataProtocolloInvioChiusura(blnValore, Colore)
    End Sub


    Private Sub Fascicolo(ByVal blnValore As Boolean, ByVal Colore As Color)
        TxtCodiceFascicolo.BackColor = Colore
        txtDescFasc.BackColor = Colore
        TxtCodiceFascicolo.Enabled = blnValore
        txtDescFasc.Enabled = blnValore
    End Sub
    Private Sub ImageSelDoc(ByVal blnValore As Boolean)
        imgSelDoc.Visible = blnValore
        imgDownolad.Visible = blnValore
        imgCancDoc.Visible = blnValore
    End Sub
    Private Sub ImageProtolloRichiesta(ByVal blnValore As Boolean)
        ImgProtolloRichiesta.Visible = blnValore
        ImgApriAllegatiRichiesta.Visible = blnValore
        imcCancProtSeg.Visible = blnValore
    End Sub

    Private Sub ImageProtolloAcquisizione(ByVal blnValore As Boolean)
        ImgProtolloAcquisizione.Visible = blnValore
        ImgApriAllegatiAcquisizione.Visible = blnValore
        imgCancProtRicSeg.Visible = blnValore
    End Sub

    Private Sub ImageProtolloApprovazione(ByVal blnValore As Boolean)
        ImgProtolloApprovazione.Visible = blnValore
        ImgApriAllegatiApprovazione.Visible = blnValore
        imgCancProtAppSeg.Visible = blnValore
    End Sub

    Private Sub DataSegnalazione(ByVal blnValore As Boolean, ByVal Colore As Color)
        txtDataSegnalazione.BackColor = Colore
        txtDataSegnalazione.Enabled = blnValore
    End Sub
    '****
    Private Sub SchedaContestatazione(ByVal blnValore As Boolean, ByVal Colore As Color)
        DataProtocolloInvioLetteraContestazione(blnValore, Colore)
        DataProtLetteraContestazioneDG(blnValore, Colore)
        DataRicezioneLetteraContestazione(blnValore, Colore)
        DataRispostaLetteraContestazione(blnValore, Colore)
        DataProtocolloRispostaContestazione(blnValore, Colore)
        DataProtChiusuraContestazione(blnValore, Colore)
        ImageProtocolloTrasContestataDG(blnValore)
        ImageProtocolloInvioLettContestazione(blnValore)
        ImageProtocolloRispContestazione(blnValore)
        ImageProtocolloProtChiusuraContestazione(blnValore)
    End Sub

    '---- contestazione 
    Private Sub DataProtocolloInvioLetteraContestazione(ByVal blnValore As Boolean, ByVal Colore As Color)
        txtDataProtocolloInvioLetteraContestazione.BackColor = Colore
        TxtNumeroProtocolloInvioLettContestazione.BackColor = Colore

        txtDataProtocolloInvioLetteraContestazione.Enabled = blnValore
        TxtNumeroProtocolloInvioLettContestazione.Enabled = blnValore

    End Sub
    Private Sub DataProtLetteraContestazioneDG(ByVal blnValore As Boolean, ByVal Colore As Color)
        txtDataProtLetteraContestazioneDG.BackColor = Colore
        txtNProtLetteraContestazioneDG.BackColor = Colore

        txtDataProtLetteraContestazioneDG.Enabled = blnValore
        txtNProtLetteraContestazioneDG.Enabled = blnValore


    End Sub
    Private Sub DataRicezioneLetteraContestazione(ByVal blnValore As Boolean, ByVal Colore As Color)
        txtDataRicezioneLetteraContestazione.BackColor = Colore

        txtDataRicezioneLetteraContestazione.Enabled = blnValore
    End Sub
    Private Sub DataRispostaLetteraContestazione(ByVal blnValore As Boolean, ByVal Colore As Color)
        txtDataRispostaLetteraContestazione.BackColor = Colore

        txtDataRispostaLetteraContestazione.Enabled = blnValore
    End Sub
    Private Sub DataProtocolloRispostaContestazione(ByVal blnValore As Boolean, ByVal Colore As Color)
        '(Controdeduzioni)
        txtDataProtocolloRispostaContestazione.BackColor = Colore
        TxtNumProtocolloRispostaContestazione.BackColor = Colore

        TxtNumProtocolloRispostaContestazione.Enabled = blnValore
        txtDataProtocolloRispostaContestazione.Enabled = blnValore
    End Sub
    Private Sub DataProtChiusuraContestazione(ByVal blnValore As Boolean, ByVal Colore As Color)
        TxtDataProtChiusuraContestazione.BackColor = Colore
        TxtNumProtChiusuraContestazione.BackColor = Colore

        TxtDataProtChiusuraContestazione.Enabled = blnValore
        TxtNumProtChiusuraContestazione.Enabled = blnValore
    End Sub
    Private Sub ImageProtocolloTrasContestataDG(ByVal blnValore As Boolean)
        ImgProtocolloTrasContestataDG.Visible = blnValore
        ImgApriAllegatiTrasContestataDG.Visible = blnValore
    End Sub

    Private Sub ImageProtocolloInvioLettContestazione(ByVal blnValore As Boolean)
        ImgProtocolloInvioLettContestazione.Visible = blnValore
        ImgApriAllegatiInvioLettContestazione.Visible = blnValore
    End Sub

    Private Sub ImageProtocolloRispContestazione(ByVal blnValore As Boolean)
        ImgProtocolloRispContestazione.Visible = blnValore
        ImgApriAllegatiRispContestazione.Visible = blnValore
    End Sub

    Private Sub ImageProtocolloProtChiusuraContestazione(ByVal blnValore As Boolean)
        ImgProtocolloProtChiusuraContestazione.Visible = blnValore
        ImgApriAllegatiProtChiusuraContestazione.Visible = blnValore
    End Sub

    '*** fine CONTESTAZIONe
    '**** sanzione
    Private Sub SchedaSanzione(ByVal blnValore As Boolean, ByVal Colore As Color)
        DataProtocolloTrasmissioneSanzione(blnValore, Colore)
        DataProtocolloEsecuzioneSanzione(blnValore, Colore)
        DataProtocolloTrasmissioneServizi(blnValore, Colore)
        DataEsecuzioneSanzione(blnValore, Colore)
        Ufficio(blnValore, Colore)
        Competenze(blnValore, Colore)
        ImageProtocolloTrasmissioneSanzione(blnValore)
        ImageProtocolloEsecuzioneSanzione(blnValore)
        ImageProtocolloTrasmServizi(blnValore)

    End Sub

    Private Sub DataProtocolloTrasmissioneSanzione(ByVal blnValore As Boolean, ByVal Colore As Color)
        txtDataProtocolloTrasmissioneSanzione.BackColor = Colore
        TxtNumeroProtocolloTrasmissioneSanzione.BackColor = Colore

        txtDataProtocolloTrasmissioneSanzione.BackColor = Colore
        TxtNumeroProtocolloTrasmissioneSanzione.BackColor = Colore
    End Sub
    Private Sub DataProtocolloEsecuzioneSanzione(ByVal blnValore As Boolean, ByVal Colore As Color)
        txtDataProtocolloEsecuzioneSanzione.BackColor = Colore
        TxtNumeroProtocolloEsecuzioneSanzione.BackColor = Colore

        txtDataProtocolloEsecuzioneSanzione.Enabled = blnValore
        TxtNumeroProtocolloEsecuzioneSanzione.Enabled = blnValore
    End Sub
    Private Sub DataProtocolloTrasmissioneServizi(ByVal blnValore As Boolean, ByVal Colore As Color)
        txtDataProtocolloTrasmServizi.BackColor = Colore
        txtNProtocolloTrasmServizi.BackColor = Colore

        txtDataProtocolloTrasmServizi.Enabled = blnValore
        txtNProtocolloTrasmServizi.Enabled = blnValore
    End Sub
    Private Sub DataEsecuzioneSanzione(ByVal blnValore As Boolean, ByVal Colore As Color)
        txtDataEsecuzioneSanzione.Enabled = blnValore
        txtDataEsecuzioneSanzione.BackColor = Colore
    End Sub
    Private Sub Ufficio(ByVal blnValore As Boolean, ByVal Colore As Color)
        ddlUfficio.BackColor = Colore

        ddlUfficio.Enabled = blnValore
    End Sub

    Private Sub Competenze(ByVal blnValore As Boolean, ByVal Colore As Color)
        ddlCompetenze.BackColor = Colore
        ddlCompetenze.Enabled = blnValore
    End Sub

    Private Sub ImageProtocolloTrasmissioneSanzione(ByVal blnValore As Boolean)
        ImgProtocolloTrasmissioneSanzione.Visible = blnValore
        ImgApriAllegatiTrasmissioneSanzione.Visible = blnValore
    End Sub

    Private Sub ImageProtocolloEsecuzioneSanzione(ByVal blnValore As Boolean)
        ImgProtocolloEsecuzioneSanzione.Visible = blnValore
        ImgApriAllegatiEsecuzioneSanzione.Visible = blnValore
    End Sub

    Private Sub ImageProtocolloTrasmServizi(ByVal blnValore As Boolean)
        ImgProtocolloTrasmServizi.Visible = blnValore
        ImgApriAllegatiTrasmServizi.Visible = blnValore
    End Sub
    '***** fine sanzione
    Private Function D1LTD2(ByVal d1 As String, d2 As String) As Boolean

        ' ritorna true se la data d1< d2 - > messaggio errore 
        Dim data1 As Date
        Dim data2 As Date

        D1LTD2 = False

        Try
            If (d1 <> "" And d2 <> "") Then
                data1 = Convert.ToDateTime(d1)
                data2 = Convert.ToDateTime(d2)

                If data1 < data2 Then
                    D1LTD2 = True
                End If
            End If
        Catch ex As Exception
            D1LTD2 = True
        End Try

    End Function

    Private Function ControlloSanzioneServizi(ByVal IdModello As Integer, ByVal IdVerifica As Integer) As Boolean
        'Creata da Simona Cordella il 22/02/2011
        'Sub controlla che siamo stati inseriti i Servizi prima di protoccalare la Lettera Trasmissione Servizi
        Dim strSql As String
        Dim dtrServizi As SqlClient.SqlDataReader
        Dim bln As Boolean = False
        If Not dtrServizi Is Nothing Then
            dtrServizi.Close()
            dtrServizi = Nothing
        End If


        'ritorna TRUE: Sono stati indicati i servizi
        '        FALSE: non sno stati indicati i servizio quidni invio un messaggio informativo all'utente
        strSql = " Select IdServizio from TVerificheServizi where IDModello = " & IdModello & " AND IdVerifica =" & IdVerifica & " "
        dtrServizi = ClsServer.CreaDatareader(strSql, Session("conn"))
        bln = dtrServizi.HasRows
        'If dtrServizi.HasRows = True Then
        '    txtDataProtocolloTrasmServizi.BackColor = Color.White
        '    txtNProtocolloTrasmServizi.BackColor = Color.White
        'Else
        '    txtDataProtocolloTrasmServizi.BackColor = Color.LightGray
        '    txtNProtocolloTrasmServizi.BackColor = Color.LightGray
        'End If
        If Not dtrServizi Is Nothing Then
            dtrServizi.Close()
            dtrServizi = Nothing
        End If
        Return bln
    End Function
End Class