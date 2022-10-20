Imports System.IO
Imports System.Drawing

Public Class verificarequisiti
    Inherits System.Web.UI.Page
    Dim strIdEnteSedeAttuazione As String
    Public sTipoPRG As Integer
#Region " Codice generato da Progettazione Web Form "

    'Chiamata richiesta da Progettazione Web Form.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents txtIdVerificatore As System.Web.UI.HtmlControls.HtmlInputHidden

    Protected WithEvents Hdd_Reload As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents lblCodiceFascicolo As System.Web.UI.WebControls.Label


    'Protected WithEvents imgDataProtCredenziali As System.Web.UI.HtmlControls.HtmlImage
    'Protected WithEvents imgDataInizioVerifica As System.Web.UI.HtmlControls.HtmlImage
    'Protected WithEvents imgDataFineVerifica As System.Web.UI.HtmlControls.HtmlImage

    'Protected WithEvents imgDataProtocolloIncarico As System.Web.UI.HtmlControls.HtmlImage
    'Protected WithEvents imgDataProtocolloRelazione As System.Web.UI.HtmlControls.HtmlImage
    'Protected WithEvents imgDataProtTrasDG As System.Web.UI.HtmlControls.HtmlImage

    'Protected WithEvents imgDataProtocolloInvioLetteraContestazione As System.Web.UI.HtmlControls.HtmlImage
    'Protected WithEvents imgDataProtocolloInvioLetteraContestazioneDG As System.Web.UI.HtmlControls.HtmlImage
    'Protected WithEvents imgDataProtocolloInvioChiusura As System.Web.UI.HtmlControls.HtmlImage
    'Protected WithEvents imgDataRicezioneLetteraContestazione As System.Web.UI.HtmlControls.HtmlImage
    'Protected WithEvents imgDataProtocolloTrasmServizi As System.Web.UI.HtmlControls.HtmlImage
    'Protected WithEvents imgDataRispostaLetteraContestazione As System.Web.UI.HtmlControls.HtmlImage
    'Protected WithEvents imgDataProtocolloEsecuzioneSanzione As System.Web.UI.HtmlControls.HtmlImage
    'Protected WithEvents imgDataProtocolloRispostaContestazione As System.Web.UI.HtmlControls.HtmlImage
    'Protected WithEvents imgDataProtocolloTrasmissioneSanzione As System.Web.UI.HtmlControls.HtmlImage
    'Protected WithEvents imgDataEsecuzioneSanzione As System.Web.UI.HtmlControls.HtmlImage
    'Protected WithEvents imgDataProtocolloChiusuraContestazione As System.Web.UI.HtmlControls.HtmlImage


    Protected WithEvents imgInfoVerifiche As System.Web.UI.HtmlControls.HtmlImage

    Protected WithEvents chkRelazione As System.Web.UI.WebControls.CheckBox
    Protected WithEvents chkRichiamo As System.Web.UI.WebControls.CheckBox
    Protected WithEvents chkNonEffettuata As System.Web.UI.WebControls.CheckBox
    Protected WithEvents TxtCodiceFascicolo As System.Web.UI.WebControls.TextBox
    ' Protected WithEvents TxtCodiceFasc As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtDescFasc As System.Web.UI.WebControls.TextBox

    Protected WithEvents LblNumProtocolloInvioLetteraChiusura As System.Web.UI.WebControls.Label
    Protected WithEvents TxtNumProtocolloInvioLetteraChiusura As System.Web.UI.WebControls.TextBox

    Protected WithEvents LblDataProtTrasDG As System.Web.UI.WebControls.Label
    Protected WithEvents TxtDataProtTrasDG As System.Web.UI.WebControls.TextBox
    Protected WithEvents LblNumProtTrasDG As System.Web.UI.WebControls.Label
    Protected WithEvents TxtNumProtTrasDG As System.Web.UI.WebControls.TextBox
    Protected WithEvents lblCodiceVerifica As System.Web.UI.WebControls.Label
    'Protected WithEvents lblDataIncarico As System.Web.UI.WebControls.Label
    Protected WithEvents TextBox3 As System.Web.UI.WebControls.TextBox
    Protected WithEvents TextBox4 As System.Web.UI.WebControls.TextBox
    Protected WithEvents lblDataProtIncarico As System.Web.UI.WebControls.Label
    Protected WithEvents txtDataProtocolloIncarico As System.Web.UI.WebControls.TextBox
    Protected WithEvents TxtNumProtIncarico As System.Web.UI.WebControls.TextBox
    Protected WithEvents lblNumProtIncarico As System.Web.UI.WebControls.Label
    Protected WithEvents lblDataFaxIncarico As System.Web.UI.WebControls.Label
    Protected WithEvents LblStampaCredenzialiIncarico As System.Web.UI.WebControls.Label
    Protected WithEvents LblStampaLettereIncarico As System.Web.UI.WebControls.Label

    Protected WithEvents CmdStampaCredenzialiIncarico As System.Web.UI.WebControls.ImageButton
    Protected WithEvents CmdStampaLettereIncarico As System.Web.UI.WebControls.ImageButton
    'Protected WithEvents cmdRelazioneStand As System.Web.UI.WebControls.ImageButton
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

    Protected WithEvents lblDataInizioVerifica As System.Web.UI.WebControls.Label
    Protected WithEvents lblDataFineVerifica As System.Web.UI.WebControls.Label
    Protected WithEvents txtDataInizioVerifica As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtDataFineVerifica As System.Web.UI.WebControls.TextBox
    Protected WithEvents LblDataProtocolloRelazione As System.Web.UI.WebControls.Label
    Protected WithEvents txtDataProtocolloRelazione As System.Web.UI.WebControls.TextBox

    Protected WithEvents txtDataProtCredenziali As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtNumProtCredenziali As System.Web.UI.WebControls.TextBox

    Protected WithEvents LblDataProtocolloInvioLetteraContestazione As System.Web.UI.WebControls.Label
    Protected WithEvents txtDataProtocolloInvioLetteraContestazione As System.Web.UI.WebControls.TextBox

    Protected WithEvents Label9 As System.Web.UI.WebControls.Label
    Protected WithEvents Textbox11 As System.Web.UI.WebControls.TextBox
    Protected WithEvents Label7 As System.Web.UI.WebControls.Label
    Protected WithEvents Textbox9 As System.Web.UI.WebControls.TextBox
    Protected WithEvents Label13 As System.Web.UI.WebControls.Label
    Protected WithEvents Textbox14 As System.Web.UI.WebControls.TextBox
    Protected WithEvents Label15 As System.Web.UI.WebControls.Label
    Protected WithEvents Textbox15 As System.Web.UI.WebControls.TextBox
    Protected WithEvents Label23 As System.Web.UI.WebControls.Label
    Protected WithEvents Textbox22 As System.Web.UI.WebControls.TextBox
    Protected WithEvents Textbox12 As System.Web.UI.WebControls.TextBox
    Protected WithEvents Label29 As System.Web.UI.WebControls.Label
    Protected WithEvents Textbox28 As System.Web.UI.WebControls.TextBox


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

    Protected WithEvents LblUfficioUnsc As System.Web.UI.WebControls.Label
    'Protected WithEvents DropDownList1 As System.Web.UI.WebControls.DropDownList
    Protected WithEvents LblCompetenza As System.Web.UI.WebControls.Label
    Protected WithEvents ddlUfficio As System.Web.UI.WebControls.DropDownList
    Protected WithEvents ddlCompetenze As System.Web.UI.WebControls.DropDownList
    Protected WithEvents lblSanzione As System.Web.UI.WebControls.Label
    'Protected WithEvents ddlSanzione As System.Web.UI.WebControls.DropDownList


    Protected WithEvents lblIspettoreSupporto As System.Web.UI.WebControls.Label
    '   Protected WithEvents txtCodProgetto As System.Web.UI.WebControls.TextBox




    Protected WithEvents Label1 As System.Web.UI.WebControls.Label
    Protected WithEvents lblProgrammazione As System.Web.UI.WebControls.Label
    Protected WithEvents lblIspettore As System.Web.UI.WebControls.Label
    Protected WithEvents lblStatoVerifica As System.Web.UI.WebControls.Label
    Protected WithEvents lblEnte As System.Web.UI.WebControls.Label
    Protected WithEvents Label2 As System.Web.UI.WebControls.Label
    Protected WithEvents lblProgetto As System.Web.UI.WebControls.Label
    Protected WithEvents lblDataInizioProgetto As System.Web.UI.WebControls.Label
    Protected WithEvents lblDataFineProgetto As System.Web.UI.WebControls.Label
    Protected WithEvents lblDataAssegnazione As System.Web.UI.WebControls.Label
    Protected WithEvents lblDataApprovazione As System.Web.UI.WebControls.Label
    Protected WithEvents lblDataPrevistaVerifica As System.Web.UI.WebControls.Label
    Protected WithEvents lblDataFinePrevistaVerifica As System.Web.UI.WebControls.Label
    Protected WithEvents lblTipologiaVerifica As System.Web.UI.WebControls.Label
    Protected WithEvents dgRisultatoRicerca As System.Web.UI.WebControls.DataGrid
    Protected WithEvents LblDatiEnte As System.Web.UI.WebControls.Label
    Protected WithEvents LblDataLetteraContestazioneDG As System.Web.UI.WebControls.Label
    Protected WithEvents txtDataProtLetteraContestazioneDG As System.Web.UI.WebControls.TextBox
    Protected WithEvents LblNProtLetteraContestazioneDG As System.Web.UI.WebControls.Label
    Protected WithEvents txtNProtLetteraContestazioneDG As System.Web.UI.WebControls.TextBox
    Protected WithEvents LblDataProtChiusuraContestazione As System.Web.UI.WebControls.Label
    Protected WithEvents TxtDataProtChiusuraContestazione As System.Web.UI.WebControls.TextBox
    Protected WithEvents LblNumProtChiusuraContestazione As System.Web.UI.WebControls.Label
    Protected WithEvents TxtNumProtChiusuraContestazione As System.Web.UI.WebControls.TextBox
    Protected WithEvents CmdLetteraChiusContestazione As System.Web.UI.WebControls.ImageButton
    Protected WithEvents LblCompetenzaProg As System.Web.UI.WebControls.Label

    Protected WithEvents LblDataProtocolloTrasmServizi As System.Web.UI.WebControls.Label
    Protected WithEvents txtDataProtocolloTrasmServizi As System.Web.UI.WebControls.TextBox
    Protected WithEvents LblNProtocolloTrasmServizi As System.Web.UI.WebControls.Label
    Protected WithEvents txtNProtocolloTrasmServizi As System.Web.UI.WebControls.TextBox

    Protected WithEvents cmdSelFascicolo As System.Web.UI.WebControls.Image
    Protected WithEvents cmdSelProtocollo0 As System.Web.UI.WebControls.Image



    'CREDENZIALI
    Protected WithEvents ImgProtolloCredenziali As System.Web.UI.WebControls.Image
    Protected WithEvents ImgApriAllegatiCredenziali As System.Web.UI.WebControls.Image
    ' Protected WithEvents ImgProtocollazioneCredenziali As System.Web.UI.WebControls.Image
    'PROTOCOLLO INCARICO
    Protected WithEvents cmdSc1SelProtocollo1 As System.Web.UI.WebControls.Image
    Protected WithEvents cmdSc1Allegati1 As System.Web.UI.WebControls.Image
    'Protected WithEvents cmdNuovoFascioclo As System.Web.UI.WebControls.Image
    'TRASMISSIONE AL DG
    Protected WithEvents ImgProtocolloTrasDG As System.Web.UI.WebControls.Image
    Protected WithEvents ImgApriAllegatiTrasDG As System.Web.UI.WebControls.Image
    Protected WithEvents ImgProtocollazioneTrasDG As System.Web.UI.WebControls.Image
    'LETTERA CHIUSURA AL DG
    Protected WithEvents ImgProtocolloTrasContestataDG As System.Web.UI.WebControls.Image
    Protected WithEvents ImgApriAllegatiTrasContestataDG As System.Web.UI.WebControls.Image
    Protected WithEvents ImgProtocollazioneTrasContestataDG As System.Web.UI.WebControls.Image
    'LETTERA CHIUSURA 
    ' Protected WithEvents ImgProtocolloLettChiusura As System.Web.UI.WebControls.Image
    Protected WithEvents ImgApriAllegatiLettChiusura As System.Web.UI.WebControls.Image
    ' Protected WithEvents ImgProtocollazioneLettChiusura As System.Web.UI.WebControls.Image
    'INVIO LETTERA CONTESTAZIONE 
    'Protected WithEvents ImgProtocolloInvioLettContestazione As System.Web.UI.WebControls.Image
    Protected WithEvents ImgApriAllegatiInvioLettContestazione As System.Web.UI.WebControls.Image
    '  Protected WithEvents ImgProtocollazioneInvioLettContestazione As System.Web.UI.WebControls.Image
    'RISPOSTA LETTERA CONTESTAZIONE ( Controdeduzioni )
    ' Protected WithEvents ImgProtocolloRispContestazione As System.Web.UI.WebControls.Image
    Protected WithEvents ImgApriAllegatiRispContestazione As System.Web.UI.WebControls.Image
    ' Protected WithEvents ImgProtocollazioneRispContestazione As System.Web.UI.WebControls.Image
    'DATA PROTOCOLLO CHIUSURA LETTERA CONTESTAZIONE
    ' Protected WithEvents ImgProtocolloProtChiusuraContestazione As System.Web.UI.WebControls.Image
    Protected WithEvents ImgApriAllegatiProtChiusuraContestazione As System.Web.UI.WebControls.Image
    ' Protected WithEvents ImgProtocollazioneProtChiusuraContestazione As System.Web.UI.WebControls.Image
    'DATA PROTOCOLLO TRASMISSIONE SANZIONE
    'Protected WithEvents ImgProtocolloTrasmissioneSanzione As System.Web.UI.WebControls.Image
    Protected WithEvents ImgApriAllegatiTrasmissioneSanzione As System.Web.UI.WebControls.Image
    'Protected WithEvents ImgProtocollazioneTrasmissioneSanzione As System.Web.UI.WebControls.Image
    'DATA PROTOCOLLO TRASMISSIONE SANZIONE
    ' Protected WithEvents ImgProtocolloTrasmServizi As System.Web.UI.WebControls.Image
    Protected WithEvents ImgApriAllegatiTrasmServizi As System.Web.UI.WebControls.Image
    ' Protected WithEvents ImgProtocollazioneTrasmServizi As System.Web.UI.WebControls.Image
    'DATA PROTOCOLLO ESECUZIONE SANZIONE
    'Protected WithEvents ImgProtocolloEsecuzioneSanzione As System.Web.UI.WebControls.Image
    Protected WithEvents ImgApriAllegatiEsecuzioneSanzione As System.Web.UI.WebControls.Image
    ' Protected WithEvents ImgProtocollazioneEsecuzioneSanzione As System.Web.UI.WebControls.Image
    'sanzione 
    Protected WithEvents cmdSanzione As System.Web.UI.WebControls.ImageButton
    Protected WithEvents imgScanner As System.Web.UI.WebControls.ImageButton
    Protected WithEvents cmdStampa As System.Web.UI.WebControls.Button
    Protected WithEvents imgAssegnaVer As System.Web.UI.WebControls.Button
    Protected WithEvents cmdIncludi As System.Web.UI.WebControls.Button
    Protected WithEvents cmdSospendi As System.Web.UI.WebControls.Button
    Protected WithEvents cmdRipristina As System.Web.UI.WebControls.Button
    'Protected WithEvents CmdConferma As System.Web.UI.WebControls.ImageButton
    Protected WithEvents cmdChiusaContestata As System.Web.UI.WebControls.Button
    Protected WithEvents cmdSalva As System.Web.UI.WebControls.Button
    Protected WithEvents cmdChiudi As System.Web.UI.WebControls.Button

    Protected WithEvents imgSanzioneServizi As System.Web.UI.WebControls.Image
    Protected WithEvents lblmessaggio As System.Web.UI.WebControls.Label

    Protected WithEvents LblNote As System.Web.UI.WebControls.Label
    Protected WithEvents TxtNote As System.Web.UI.WebControls.TextBox
    Protected WithEvents cmdStampaVQ As System.Web.UI.WebControls.Button

    'NOTA: la seguente dichiarazione è richiesta da Progettazione Web Form.
    'Non spostarla o rimuoverla.
    Private designerPlaceholderDeclaration As System.Object

 
    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: questa chiamata al metodo è richiesta da Progettazione Web Form.
        'Non modificarla nell'editor del codice.
        InitializeComponent()
    End Sub

#End Region

    Public dtrLeggiDati As SqlClient.SqlDataReader
    Public TBLLeggiDati As DataTable
    Public row As TableRow
    Public myRow As DataRow
    Private Sub SetVisibile(ByRef Txt As TextBox, ByRef TxtData As TextBox, ByRef Img As ImageButton)
        If (Txt.Text = "" Or TxtData.Text = "") Then
            Img.Visible = False
        Else
            Img.Visible = True
        End If
    End Sub
    Private Sub SetVisibile(ByRef Txt As TextBox, ByRef Img As ImageButton)
        If (Txt.BackColor = Color.White) Then
            Img.Visible = True
        Else
            Img.Visible = False
        End If

    End Sub
    Private Sub SetProtocolloStatus()

        'SetVisibile(txtNumProtCredenziali, ImgProtolloCredenziali)
        'SetVisibile(TxtNumProtIncarico, cmdSc1SelProtocollo1)
        SetVisibile(TxtNumProtTrasDG, ImgProtocolloTrasDG)
        SetVisibile(TxtNumProtocolloInvioLetteraChiusura, ImgProtocolloLettChiusura)
        SetVisibile(txtNProtLetteraContestazioneDG, ImgProtocolloTrasContestataDG)
        SetVisibile(TxtNumeroProtocolloInvioLettContestazione, ImgProtocolloInvioLettContestazione)
        SetVisibile(TxtNumProtocolloRispostaContestazione, ImgProtocolloRispContestazione)
        SetVisibile(TxtNumProtChiusuraContestazione, ImgProtocolloProtChiusuraContestazione)
        SetVisibile(TxtNumeroProtocolloTrasmissioneSanzione, ImgProtocolloTrasmissioneSanzione)
        SetVisibile(TxtNumeroProtocolloEsecuzioneSanzione, ImgProtocolloEsecuzioneSanzione)
        SetVisibile(txtNProtocolloTrasmServizi, ImgProtocolloTrasmServizi)

        SetVisibile(txtNumProtCredenziali, txtDataProtCredenziali, ImgApriAllegatiCredenziali)
        SetVisibile(TxtNumProtIncarico, txtDataProtocolloIncarico, cmdSc1Allegati1)
        SetVisibile(TxtNumProtTrasDG, TxtDataProtTrasDG, ImgApriAllegatiTrasDG)
        SetVisibile(TxtNumProtocolloInvioLetteraChiusura, txtDataProtocolloInvioChiusura, ImgApriAllegatiLettChiusura)
        SetVisibile(txtNProtLetteraContestazioneDG, txtDataProtLetteraContestazioneDG, ImgApriAllegatiTrasContestataDG)
        SetVisibile(TxtNumeroProtocolloInvioLettContestazione, txtDataProtocolloInvioLetteraContestazione, ImgApriAllegatiInvioLettContestazione)
        SetVisibile(TxtNumProtocolloRispostaContestazione, txtDataProtocolloRispostaContestazione, ImgApriAllegatiRispContestazione)
        SetVisibile(TxtNumProtChiusuraContestazione, TxtDataProtChiusuraContestazione, ImgApriAllegatiProtChiusuraContestazione)
        SetVisibile(TxtNumeroProtocolloTrasmissioneSanzione, txtDataProtocolloTrasmissioneSanzione, ImgApriAllegatiTrasmissioneSanzione)
        SetVisibile(TxtNumeroProtocolloEsecuzioneSanzione, txtDataProtocolloEsecuzioneSanzione, ImgApriAllegatiEsecuzioneSanzione)
        SetVisibile(txtNProtocolloTrasmServizi, txtDataProtocolloTrasmServizi, ImgApriAllegatiTrasmServizi)


    End Sub
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
            hftxtIdVerifica.Value = Request.QueryString("IdVerifica")
            hftxtidProgrammazione.Value = Request.QueryString("idprogrammazione")
            hftxtidente.Value = Request.QueryString("Idente")

            CaricaMaschera()
            'If Hdd_Reload.Value = 1 Then
            '    Hdd_Reload.Value = 0
            'End If
            'modficato il 31/03/2011 da simona cordellla
            ' chek visibile solo per L'UNSC
            If Session("TipoUtente") = "R" Then
                chkNonEffettuata.Visible = False
            End If
            'If lblTipologiaVerifica.Text = "Segnalata" Then
            '    cmdIncludi.Visible = False
            'End If
        End If
        If Session("IdRegCompetenza") = 22 Then
            SetProtocolloStatus()
        End If

    End Sub
    Sub CaricaMaschera()
        lblErrore.Text = ""
        CaricaIntestazione()
        dgRisultatoRicerca.CurrentPageIndex = 0
        CaricaSedi()
        CaricaCombo()
        CaricaCompetenze()
        CaricaVerifiche()
        PersonalizzaMaschera(lblStatoVerifica.Text, Session("IdRegCompetenza"))
    End Sub
    Private Sub CaricaSedi()
        Dim strSql As String
        Dim DataSetRicerca As DataSet


        strSql = "SELECT  attivitàentisediattuazione.IDEnteSedeAttuazione, entisediattuazioni.Denominazione + ' (' + CONVERT(varchar, attivitàentisediattuazione.IDEnteSedeAttuazione) + ')' AS SedeAttuazione, "
        strSql = strSql & " entisedi.Indirizzo + ' ' + CONVERT(varchar, entisedi.Civico) AS Indirizzo,  comuni.Denominazione + ' (' + provincie.DescrAbb + ')' AS Comune, "
        strSql = strSql & " regioni.Regione, TVerificheAssociate.IDVerificheAssociate, TVerificheAssociate.IDVerifica, TVerificheAssociate.IDAttivitàEnteSedeAttuazione, "
        strSql = strSql & " CASE WHEN ISNULL(TVerificheAssociate.ConfermaRequisiti, 0)  = 0 THEN 'No' ELSE 'Si' END AS ConfermaRequisiti, "
        strSql = strSql & " entisedi.PrefissoTelefono + '' + entisedi.Telefono AS Telefono, entisedi.PrefissoFax + '' + entisedi.Fax AS Fax"
        strSql = strSql & " FROM TVerificheAssociate INNER JOIN "
        strSql = strSql & " attivitàentisediattuazione ON TVerificheAssociate.IDAttivitàEnteSedeAttuazione = attivitàentisediattuazione.IDAttivitàEnteSedeAttuazione INNER JOIN "
        strSql = strSql & " entisediattuazioni ON attivitàentisediattuazione.IDEnteSedeAttuazione = entisediattuazioni.IDEnteSedeAttuazione INNER JOIN "
        strSql = strSql & " entisedi ON entisediattuazioni.IDEnteSede = entisedi.IDEnteSede   "
        strSql = strSql & " INNER JOIN comuni ON entisedi.IDComune = comuni.IDComune  "
        strSql = strSql & " INNER JOIN  provincie on provincie.IDProvincia = comuni.IDProvincia "
        strSql = strSql & " INNER JOIN  regioni ON provincie.IDRegione = regioni.IDRegione "
        strSql = strSql & " WHERE TVerificheAssociate.idverifica=  " & hftxtIdVerifica.Value & " "

        DataSetRicerca = ClsServer.DataSetGenerico(strSql, Session("conn"))
        dgRisultatoRicerca.DataSource = DataSetRicerca
        dgRisultatoRicerca.DataBind()
    End Sub
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
        ' Make the ID column the primary key column. da verificare?????????
        'Dim PrimaryKeyColumns(0) As DataColumn
        'PrimaryKeyColumns(0) = myDataTable.Columns("id"))
        'myDataTable.PrimaryKey = PrimaryKeyColumns)
        dtrgenerico = ClsServer.CreaDatareader(strquery, Session("conn"))
        'Instantiate the DataSet variable.
        'mydataset = New DataSet
        ' Add the new DataTable to the DataSet.
        'mydataset.Tables.Add(myDataTable)
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
                'where IdRegioneCompetenza <> 22 "
                strSQL = strSQL & " union "
                strSQL = strSQL & " select '0','','','A' "
                'strSQL = strSQL & " union "
                'strSQL = strSQL & " select '-1',' NAZIONALE ','','B' "
                'strSQL = strSQL & " union "
                'strSQL = strSQL & " select '-2',' REGIONALE ','','C' "
                'strSQL = strSQL & " union "
                'strSQL = strSQL & " select '-3',' NON DEFINITO ','','D' "
                'strSQL = strSQL & " from RegioniCompetenze order by left(CodiceRegioneCompetenza,1),descrizione "
                strSQL = strSQL & " from RegioniCompetenze "
                'where IdRegioneCompetenza <> 22"
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
                ddlCompetenze.SelectedIndex = 0

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

        '*****Carico Ufficio
        ddlUfficio.DataSource = MakeParentTable("select IdUfficio, Ufficio from TUfficiUNSC ")
        ddlUfficio.DataTextField = "ParentItem"
        ddlUfficio.DataValueField = "id"
        ddlUfficio.DataBind()



    End Sub

    Sub CaricaIntestazione()
        Dim dtrGenerico As SqlClient.SqlDataReader
        Dim strSql As String
        Dim DataSetRicerca As DataSet

        dgRisultatoRicerca.CurrentPageIndex = 0

        strSql = "select IdVerifica, "
        strSql = strSql & " StatoVerifiche, "
        strSql = strSql & "IdIGF, "
        strSql = strSql & "Programmazione, "
        strSql = strSql & "CodiceFascicolo, "
        strSql = strSql & "StatoVerifiche, "
        strSql = strSql & "IdAttività, "
        strSql = strSql & "dbo.FormatoData(DataAssegnazione) AS DataAssegnazione, "
        strSql = strSql & "dbo.FormatoData(DataApprovazione) AS DataApprovazione, "
        strSql = strSql & "dbo.FormatoData(DataPrevistaVerifica) as DataPrevistaVerifica, "
        strSql = strSql & "dbo.FormatoData(DataFinePrevistaVerifica) as DataFinePrevistaVerifica, "
        strSql = strSql & "dbo.FormatoData(DataFineVerifica) as DataFineVerifica, "
        strSql = strSql & "dbo.FormatoData(DataInizioAttività) as DataInizioAttività, "
        strSql = strSql & "dbo.FormatoData(DataFineAttività) as DataFineAttività, "
        strSql = strSql & "(Nominativo + '(' + case when tipoverificatore = 0 then 'Interno' when tipoverificatore = 1 then 'IGF' END + ')') as Nominativo, "
        strSql = strSql & "(Denominazione + '(' + CodiceEnte + ')') as Denominazione, "
        strSql = strSql & "(Titolo + ' (' + CodiceProgetto + ')') as Titolo, "
        strSql = strSql & "IDEnteSedeAttuazione, "
        strSql = strSql & "(EnteFiglio + ' (' + convert(varchar,IDEnteSedeAttuazione)  + ')') As EnteFiglio, "
        strSql = strSql & "(Comune + '(' + DescrAbb + ')') as Comune, "
        strSql = strSql & "Regione,CodiceProgetto, IdEnte,"
        strSql = strSql & "case when tipologia = 1 then 'Programmata' when tipologia = 2 then 'Segnalata' end as TipoVerifica, CodiceEnte, competenza,idregcompetenza "
        strSql = strSql & " from ver_vw_ricerca_verifiche "
        strSql = strSql & "where idverifica = " & hftxtIdVerifica.Value & " "

        If Not dtrGenerico Is Nothing Then
            dtrGenerico.Close()
            dtrGenerico = Nothing
        End If
        'DataSetRicerca = ClsServer.DataSetGenerico(strSql, Session("conn"))
        'dgRisultatoRicerca.DataSource = DataSetRicerca
        'dgRisultatoRicerca.DataBind()

        dtrGenerico = ClsServer.CreaDatareader(strSql, Session("conn"))

        '   lblProgrammazione.Text = DataSetRicerca.Tables(3).Select

        If dtrGenerico.HasRows Then
            dtrGenerico.Read()
            lblProgrammazione.Text = IIf(IsDBNull(dtrGenerico("Programmazione")) = True, "", dtrGenerico("Programmazione"))
            lblIspettore.Text = IIf(IsDBNull(dtrGenerico("Nominativo")) = True, "", dtrGenerico("Nominativo"))
            txtIdVerificatore.Value = IIf(IsDBNull(dtrGenerico("IdIGF")) = True, "", dtrGenerico("IdIGF"))
            lblEnte.Text = IIf(IsDBNull(dtrGenerico("Denominazione")) = True, "", dtrGenerico("Denominazione"))

            lblStatoVerifica.Text = IIf(IsDBNull(dtrGenerico("StatoVerifiche")) = True, "", dtrGenerico("StatoVerifiche"))
            lblTipologiaVerifica.Text = IIf(IsDBNull(dtrGenerico("TipoVerifica")) = True, "", dtrGenerico("TipoVerifica"))
            lblProgetto.Text = IIf(IsDBNull(dtrGenerico("Titolo")) = True, "", dtrGenerico("Titolo"))
            lblDataAssegnazione.Text = IIf(IsDBNull(dtrGenerico("DataAssegnazione")) = True, "", dtrGenerico("DataAssegnazione"))
            lblDataApprovazione.Text = IIf(IsDBNull(dtrGenerico("DataApprovazione")) = True, "", dtrGenerico("DataApprovazione")) 'clsConversione.DaNullInStringaVuota(dtrGenerico("DataApprovazione"))
            lblDataPrevistaVerifica.Text = IIf(IsDBNull(dtrGenerico("DataPrevistaVerifica")) = True, "", dtrGenerico("DataPrevistaVerifica")) 'clsConversione.DaNullInStringaVuota(dtrGenerico("DataPrevistaVerifica"))
            lblDataFinePrevistaVerifica.Text = IIf(IsDBNull(dtrGenerico("DataFinePrevistaVerifica")) = True, "", dtrGenerico("DataFinePrevistaVerifica"))
            lblDataInizioProgetto.Text = IIf(IsDBNull(dtrGenerico("DataInizioAttività")) = True, "", dtrGenerico("DataInizioAttività"))
            lblDataFineProgetto.Text = IIf(IsDBNull(dtrGenerico("DataFineAttività")) = True, "", dtrGenerico("DataFineAttività"))
            'lblIndirizzo.Text = IIf(IsDBNull(dtrGenerico("EnteFiglio")) = True, "", dtrGenerico("EnteFiglio"))
            strIdEnteSedeAttuazione = IIf(IsDBNull(dtrGenerico("IDEnteSedeAttuazione")) = True, "", dtrGenerico("IDEnteSedeAttuazione"))
            hftxtIdEnteSedeAtt.Value = strIdEnteSedeAttuazione
            hftxtCodProgetto.Value = IIf(IsDBNull(dtrGenerico("CodiceProgetto")) = True, "", dtrGenerico("CodiceProgetto"))


            ' txtCodProgetto.Text = IIf(IsDBNull(dtrGenerico("CodiceProgetto")) = True, "", dtrGenerico("CodiceProgetto"))


            Session("pCodEnte") = IIf(IsDBNull(dtrGenerico("CodiceEnte")) = True, "", dtrGenerico("CodiceEnte"))
            Session("txtCodEnte") = Session("pCodEnte")
            Session("IdAttività") = dtrGenerico("IdAttività")
            Session("IdEnte") = dtrGenerico("IdEnte")
            LblCompetenzaProg.Text = dtrGenerico("competenza")
            Session("IdRegCompetenza") = dtrGenerico("idregcompetenza")
            If Not dtrGenerico Is Nothing Then
                dtrGenerico.Close()
                dtrGenerico = Nothing
            End If

            strSql = " select (indirizzo + ' ' + civico + ' - ' + cap + ' ' + comune + ' (' + provinciabreve + ')' + ' Tel. ' + PrefissoTelefonorichiestaregistrazione + Telefonorichiestaregistrazione + ' Fax ' + PrefissoFax + Fax )as Infoentepadre from VW_BO_ENTI where codiceregione ='" & Session("pCodEnte") & "' "

            'DataSetRicerca = ClsServer.DataSetGenerico(strSql, Session("conn"))
            'dgRisultatoRicerca.DataSource = DataSetRicerca
            'dgRisultatoRicerca.DataBind()

            dtrGenerico = ClsServer.CreaDatareader(strSql, Session("conn"))

            If dtrGenerico.HasRows = True Then
                dtrGenerico.Read()
                LblDatiEnte.Text = IIf(IsDBNull(dtrGenerico("Infoentepadre")) = True, "", dtrGenerico("Infoentepadre"))
            End If
            If Not dtrGenerico Is Nothing Then
                dtrGenerico.Close()
                dtrGenerico = Nothing
            End If
        End If

        If Not dtrGenerico Is Nothing Then
            dtrGenerico.Close()
            dtrGenerico = Nothing
        End If

    End Sub
    Private Sub CaricaVerifiche()
        Dim dtrGenerico As SqlClient.SqlDataReader
        Dim strSql As String

        strSql = "SELECT CodiceFascicolo,DataPrevistaVerifica,DataFinePrevistaVerifica,DataIncarico,"
        strSql = strSql & "DataProtIncarico,NProtIncarico,DataInizioVerifica,DataFineVerifica,"
        strSql = strSql & " Relazione,DataProtRelazione,NProtRelazione ,DataInvioLetteraChiusuraProc ,"
        strSql = strSql & " DataProtInvioLetteraChiusuraProc,NProtInvioLetteraChiusuraProc,DataInvioLetteraContestazione,"
        strSql = strSql & " DataProtInvioLetteraContestazione,NProtInvioLetteraContestazione,DataRicezioneLetteraContestazione,"
        strSql = strSql & " DataRispostaLetteraContestazione,DataProtRispostaLetteraContestazion,NProtRispostaLetteraContestazion,"
        strSql = strSql & " DataTrasmissioneSanzione,DataProtTrasmissioneSanzione,NProtTrasmissioneSanzione,DataEsecuzioneSanzione,"
        strSql = strSql & " DataProtEsecuzioneSanzione,NProtEsecuzioneSanzione,IDUfficioUNSC,IDRegioneCompetenza,Note,"
        'strSql = strSql & " DataRicezioneSegnalazione,DataProtSegnalazione,NProtSegnalazione,DataProtInvioLetteraInterlocutoria,"
        'strSql = strSql & " NProtInvioLetteraInterlocutoria,DataProtRispostaLetteraInterlocutoria,NProtRispostaLetteraInterlocutoria,"
        strSql = strSql & " DataInvioFax, DataProtInvioFax, NProtInvioFax, Richiamo,IdSanzione, IdFascicolo, DescrFascicolo,DataProtChiusuraContestazione, NProtChiusuraContestazione,NonEffettuata,"
        strSql = strSql & " DataProtLetteraContestazioneDG,NProtLetteraContestazioneDG,DataProtTrasmissioneServizi,NProtTrasmissioneServizi  "
        strSql = strSql & " FROM TVerifiche"
        strSql = strSql & " where idverifica = " & Request.QueryString("IdVerifica")
        If Not dtrGenerico Is Nothing Then
            dtrGenerico.Close()
            dtrGenerico = Nothing
        End If

        dtrGenerico = ClsServer.CreaDatareader(strSql, Session("Conn"))

        dtrGenerico.Read()
        TxtCodiceFascicolo.Text = "" & dtrGenerico("CodiceFascicolo")
        hfTxtCodiceFasc.Value = "" & dtrGenerico("IDFascicolo")
        ' TxtCodiceFasc.Text = "" & dtrGenerico("IDFascicolo")
        txtDescFasc.Text = "" & dtrGenerico("DescrFascicolo")

        txtDataInizioVerifica.Text = "" & dtrGenerico("DataInizioVerifica")
        txtDataFineVerifica.Text = "" & dtrGenerico("DataFineVerifica")

        txtDataProtocolloIncarico.Text = "" & dtrGenerico("DataProtIncarico")
        TxtNumProtIncarico.Text = "" & dtrGenerico("NProtIncarico")

        txtDataProtCredenziali.Text = "" & dtrGenerico("DataProtInvioFax")
        txtNumProtCredenziali.Text = "" & dtrGenerico("NProtInvioFax")

        If IsDBNull(dtrGenerico("Relazione")) = False Then
            If dtrGenerico("Relazione") = True Then
                chkRelazione.Checked = True
            Else
                chkRelazione.Checked = False
            End If
        End If
        'Agg. da s.c. il 13/07/2009. gestione nuono stato "CHIUSA NON EFFETTUATA". 
        If IsDBNull(dtrGenerico("NonEffettuata")) = False Then
            If dtrGenerico("NonEffettuata") = True Then
                chkNonEffettuata.Checked = True
            Else
                chkNonEffettuata.Checked = False
            End If
        End If
        txtDataProtocolloRelazione.Text = "" & dtrGenerico("DataProtRelazione")
        TxtDataProtTrasDG.Text = "" & dtrGenerico("DataInvioLetteraChiusuraProc")
        TxtNumProtTrasDG.Text = "" & dtrGenerico("NProtRelazione")
        txtDataProtocolloInvioChiusura.Text = "" & dtrGenerico("DataProtInvioLetteraChiusuraProc")
        TxtNumProtocolloInvioLetteraChiusura.Text = "" & dtrGenerico("NProtInvioLetteraChiusuraProc")

        txtDataProtocolloInvioLetteraContestazione.Text = "" & dtrGenerico("DataProtInvioLetteraContestazione")
        'mod da sc 14/10/2010
        txtDataProtLetteraContestazioneDG.Text = "" & dtrGenerico("DataProtLetteraContestazioneDG")
        txtNProtLetteraContestazioneDG.Text = "" & dtrGenerico("NProtLetteraContestazioneDG")
        '**
        'strsql &= "DataInvioLetteraContestazione, "
        TxtNumeroProtocolloInvioLettContestazione.Text = "" & dtrGenerico("NProtInvioLetteraContestazione")
        txtDataRicezioneLetteraContestazione.Text = "" & dtrGenerico("DataRicezioneLetteraContestazione")

        txtDataRispostaLetteraContestazione.Text = "" & dtrGenerico("DataRispostaLetteraContestazione")
        txtDataProtocolloRispostaContestazione.Text = "" & dtrGenerico("DataProtRispostaLetteraContestazion")
        TxtNumProtocolloRispostaContestazione.Text = "" & dtrGenerico("NProtRispostaLetteraContestazion")
        TxtDataProtChiusuraContestazione.Text = "" & dtrGenerico("DataProtChiusuraContestazione")
        TxtNumProtChiusuraContestazione.Text = "" & dtrGenerico("NProtChiusuraContestazione")
        'strsql &= " DataTrasmissioneSanzione,"
        txtDataProtocolloTrasmissioneSanzione.Text = "" & dtrGenerico("DataProtTrasmissioneSanzione")
        TxtNumeroProtocolloTrasmissioneSanzione.Text = "" & dtrGenerico("NProtTrasmissioneSanzione")
        'mod da sc 18/10/2010
        txtDataProtocolloTrasmServizi.Text = "" & dtrGenerico("DataProtTrasmissioneServizi")
        txtNProtocolloTrasmServizi.Text = "" & dtrGenerico("NProtTrasmissioneServizi")
        '**
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

        'txtDataProtocolloEsecuzioneSanzione.Text = "" & dtrGenerico("DataProtEsecuzioneSanzione")
        'TxtNumeroProtocolloEsecuzioneSanzione.Text = "" & dtrGenerico("NProtEsecuzioneSanzione")
        If IsDBNull(dtrGenerico("IDUfficioUNSC")) = False Then
            ddlUfficio.SelectedValue = "" & dtrGenerico("IDUfficioUNSC")
        Else '
            ddlUfficio.SelectedValue = 0
        End If
        If IsDBNull(dtrGenerico("IDRegioneCompetenza")) = False Then
            ddlCompetenze.SelectedValue = dtrGenerico("IDRegioneCompetenza")
        Else
            ddlCompetenze.SelectedValue = 0
        End If

        'If IsDBNull(dtrGenerico("IDSanzione")) = False Then
        '    ddlSanzione.SelectedValue = "" & dtrGenerico("IDSanzione")
        'Else
        '    ddlSanzione.SelectedValue = 0
        'End If
        TxtNote.Text = "" & dtrGenerico("Note")
        If IsDBNull(dtrGenerico("Richiamo")) = False Then
            If dtrGenerico("Richiamo") = True Then
                chkRichiamo.Checked = True
            Else
                chkRichiamo.Checked = False
            End If
        End If
        If Not dtrGenerico Is Nothing Then
            dtrGenerico.Close()
            dtrGenerico = Nothing
        End If
    End Sub

    Private Sub CmdStampaCredenzialiIncarico_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles CmdStampaCredenzialiIncarico.Click
        'Dim idverifica As Int16
        'idverifica = Request.QueryString("IdVerifica")
        'Call StampaDocumentiGen2(idverifica, "credenziali")

        Dim Documento As New GeneratoreModelli

        Response.Write("<SCRIPT>" & vbCrLf)
        Response.Write("window.open('" & Documento.MON_credenziali(ClsUtility.TrovaIDEnteDaIDVerifica(Request.QueryString("IdVerifica"), Session("conn")), Request.QueryString("IdVerifica"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn")) & "');" & vbCrLf)
        Response.Write("</SCRIPT>")
        'chiamo la proprietà della classe GeneratoreModelli che ritorna la path del documento creato
        Documento.Dispose()

    End Sub

    Private Sub CmdStampaLettereIncarico_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles CmdStampaLettereIncarico.Click
        'Dim idverifica As Int16
        'idverifica = Request.QueryString("IdVerifica")
        'Call StampaDocumentiGen(idverifica, "Letteradiincarico")

        Dim Documento As New GeneratoreModelli

        Response.Write("<SCRIPT>" & vbCrLf)
        Response.Write("window.open('" & Documento.MON_Letteradiincarico(ClsUtility.TrovaIDEnteDaIDVerifica(Request.QueryString("IdVerifica"), Session("conn")), Request.QueryString("IdVerifica"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn")) & "');" & vbCrLf)
        Response.Write("</SCRIPT>")
        'chiamo la proprietà della classe GeneratoreModelli che ritorna la path del documento creato
        Documento.Dispose()
    End Sub

    Function StampaDocumentiGen(ByVal intIdVERIFICA As Integer, ByVal NomeFile As String) As String
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
                Dim RiferimentoIGF As String = IIf(Not IsDBNull(dtrLeggiDati("Riferimento")), dtrLeggiDati("Riferimento"), "")

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
                    xLinea = Replace(xLinea, "<Riferimento>", RiferimentoIGF)


                    Dim intX As Integer


                    If InStr(xLinea, "<BreakPoint>") > 0 Then
                        xLinea = Replace(xLinea, "<BreakPoint>", "") & "\par"
                        Call DatiFigliverifica()
                        'If dtrLeggiDati.HasRows = True Then
                        'intX = dtrLeggiDati.FieldCount
                        'While dtrLeggiDati.Read
                        Dim num As Integer
                        num = 0
                        For Each myRow In TBLLeggiDati.Rows
                            num = num + 1
                            '''''''Writer.WriteLine("\trowd\trgaph70\trleft-70\trbrdrl\brdrs\brdrw10 ")
                            '''''''Writer.WriteLine("\trbrdrt\brdrs\brdrw10 \trbrdrr\brdrs\brdrw10 ")
                            '''''''Writer.WriteLine("\trbrdrb\brdrs\brdrw10 ")
                            '''''''Writer.WriteLine("\trpaddl70\trpaddr70\trpaddfl3\trpaddfr3")
                            '''''''Writer.WriteLine("\clbrdrl\brdrw10\brdrs\clbrdrt\brdrw10\brdrs\clbrdrr\brdrw10\brdrs\clbrdrb\brdrw10\brdrs ")
                            '''''''Writer.WriteLine("\cellx3500\clbrdrl\brdrw10\brdrs\clbrdrt\brdrw10\brdrs\clbrdrr\brdrw10\brdrs\clbrdrb\brdrw10\brdrs ")
                            '''''''Writer.WriteLine("\cellx7000\clbrdrl\brdrw10\brdrs\clbrdrt\brdrw10\brdrs\clbrdrr\brdrw10\brdrs\clbrdrb\brdrw10\brdrs ")
                            '''''''Writer.WriteLine("\cellx7500\clbrdrl\brdrw10\brdrs\clbrdrt\brdrw10\brdrs\clbrdrr\brdrw10\brdrs\clbrdrb\brdrw10\brdrs ")
                            '''''''Writer.WriteLine("\cellx9000\clbrdrl\brdrw10\brdrs\clbrdrt\brdrw10\brdrs\clbrdrr\brdrw10\brdrs\clbrdrb\brdrw10\brdrs ")
                            '''''''Writer.WriteLine("\cellx10040\pard\intbl\nowidctlpar\ri500\qj\f2\fs20\ " & dtrLeggiDati("EnteFiglio") & "\cell " & dtrLeggiDati("Indirizzo") & "\cell " & dtrLeggiDati("Civico") & "\cell " & dtrLeggiDati("Comune") & "\cell " & dtrLeggiDati("DescrAbb") & "\cell\row\pard\f2\fs20")
                            Writer.WriteLine("\trowd\trgaph70\trleft-70\trbrdrl\brdrs\brdrw10 ")
                            Writer.WriteLine("\trbrdrt\brdrs\brdrw10 \trbrdrr\brdrs\brdrw10 ")
                            Writer.WriteLine("\trbrdrb\brdrs\brdrw10 ")
                            Writer.WriteLine("\trpaddl70\trpaddr70\trpaddfl3\trpaddfr3")
                            Writer.WriteLine("\clbrdrl\brdrw10\brdrs\clbrdrt\brdrw10\brdrs\clbrdrr\brdrw10\brdrs\clbrdrb\brdrw10\brdrs ")
                            Writer.WriteLine("\cellx700\clbrdrl\brdrw10\brdrs\clbrdrt\brdrw10\brdrs\clbrdrr\brdrw10\brdrs\clbrdrb\brdrw10\brdrs ")
                            Writer.WriteLine("\cellx3500\clbrdrl\brdrw10\brdrs\clbrdrt\brdrw10\brdrs\clbrdrr\brdrw10\brdrs\clbrdrb\brdrw10\brdrs ")
                            Writer.WriteLine("\cellx7000\clbrdrl\brdrw10\brdrs\clbrdrt\brdrw10\brdrs\clbrdrr\brdrw10\brdrs\clbrdrb\brdrw10\brdrs ")
                            Writer.WriteLine("\cellx7500\clbrdrl\brdrw10\brdrs\clbrdrt\brdrw10\brdrs\clbrdrr\brdrw10\brdrs\clbrdrb\brdrw10\brdrs ")
                            Writer.WriteLine("\cellx9100\clbrdrl\brdrw10\brdrs\clbrdrt\brdrw10\brdrs\clbrdrr\brdrw10\brdrs\clbrdrb\brdrw10\brdrs ")
                            Writer.WriteLine("\cellx10040\pard\intbl\nowidctlpar\ri500\qj\f2\fs15\ " & num & "\cell " & myRow.Item("EnteFiglio") & "\cell " & myRow.Item("Indirizzo") & "\cell " & myRow.Item("Civico") & "\cell " & myRow.Item("Comune") & "\cell " & myRow.Item("DescrAbb") & "\cell\row\pard\f2\fs15")
                            'xLinea = "\b " & dtrLeggiDati("Titolo") & "\tqc " & dtrLeggiDati("CodiceEnte") & "\tqc " & dtrLeggiDati("DatainizioPrevista") & "\b0\par"
                            'Writer.WriteLine(xLinea)
                            'End While
                        Next
                        Writer.WriteLine("\par")
                    End If
                    'End If

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

            StampaDocumentiGen = "documentazione/" & strNomeFile

        Catch ex As Exception
            If Not dtrLeggiDati Is Nothing Then
                dtrLeggiDati.Close()
                dtrLeggiDati = Nothing
            End If

            Response.Write(ex.Message)
        End Try

        Response.Write("<SCRIPT>" & vbCrLf)
        Response.Write("window.open('" & StampaDocumentiGen & "');" & vbCrLf)
        Response.Write("</SCRIPT>")

    End Function

    Function StampaDocumentiVerifica(ByVal intIdVERIFICA As Integer, ByVal NomeFile As String) As String
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

            Call DatiVerifica()

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

                Dim strProgrammazione As String = dtrLeggiDati("Programmazione")
                Dim strStatoVerifica As String = dtrLeggiDati("StatoVerifiche")
                Dim strDataInizioPro As String = IIf(Not IsDBNull(dtrLeggiDati("DataInizioAttività")), dtrLeggiDati("DataInizioAttività"), "")
                Dim strDataFinePro As String = IIf(Not IsDBNull(dtrLeggiDati("DataFineAttività")), dtrLeggiDati("DataFineAttività"), "")
                Dim strDataAssegnazione As String = IIf(Not IsDBNull(dtrLeggiDati("DataAssegnazione")), dtrLeggiDati("DataAssegnazione"), "")
                Dim strDataApprovazione As String = IIf(Not IsDBNull(dtrLeggiDati("DataApprovazione")), dtrLeggiDati("DataApprovazione"), "")
                Dim strTipoVerifica As String = dtrLeggiDati("TipoVerifica")

                While xLinea <> ""
                    xLinea = Replace(xLinea, "<DataOdierna>", strDataOdierna)
                    xLinea = Replace(xLinea, "<DenominazioneEnte>", strDenominazioneEnte)
                    xLinea = Replace(xLinea, "<CodiceEnte>", strCodiceEnte)
                    xLinea = Replace(xLinea, "<Titolo>", strtitolo)
                    xLinea = Replace(xLinea, "<Programmazione>", strProgrammazione)
                    xLinea = Replace(xLinea, "<Verificatore>", strAnagrafico)
                    xLinea = Replace(xLinea, "<StatoVerifica>", strStatoVerifica)
                    xLinea = Replace(xLinea, "<DataInizioProgetto>", strDataInizioPro)
                    xLinea = Replace(xLinea, "<DataFineProgetto>", strDataFinePro)
                    xLinea = Replace(xLinea, "<DataAssegnazione>", strDataAssegnazione)
                    xLinea = Replace(xLinea, "<DataApprovazione>", strDataApprovazione)
                    xLinea = Replace(xLinea, "<TipologiaVerifica>", strTipoVerifica)
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
                    Dim intX As Integer

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

            StampaDocumentiVerifica = "documentazione/" & strNomeFile

        Catch ex As Exception
            If Not dtrLeggiDati Is Nothing Then
                dtrLeggiDati.Close()
                dtrLeggiDati = Nothing
            End If

            Response.Write(ex.Message)
        End Try

        Response.Write("<SCRIPT>" & vbCrLf)
        Response.Write("window.open('" & StampaDocumentiVerifica & "');" & vbCrLf)
        Response.Write("</SCRIPT>")

    End Function
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

    'NON ESISTE PIU'!!
    'Private Sub cmdRelazioneStand_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles cmdRelazioneStand.Click

    '    'Dim Documento As New GeneratoreModelli

    '    'Response.Write("<SCRIPT>" & vbCrLf)
    '    'Response.Write("window.open('" & Documento.MON_relazionestand(ClsUtility.TrovaIDEnteDaIDVerifica(Request.QueryString("IdVerifica"), Session("conn")), Request.QueryString("IdVerifica"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn")) & "');" & vbCrLf)
    '    'Response.Write("</SCRIPT>")
    '    ''chiamo la proprietà della classe GeneratoreModelli che ritorna la path del documento creato
    '    'Documento.Dispose()

    '    'Dim idverificaassociata As Int32
    '    'idverificaassociata = Request.QueryString("IdVerifica")
    '    'Call StampaDocumentiRelazioni(idverificaassociata, "relazionestand")
    'End Sub

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
        'Dim idverifica As Int16
        'idverifica = Request.QueryString("IdVerifica")
        'Call StampaDocumentiGen(idverifica, "VerificasenzaIRREGOLARITAconrichiamo")

        Dim Documento As New GeneratoreModelli

        Response.Write("<SCRIPT>" & vbCrLf)
        Response.Write("window.open('" & Documento.MON_VerificasenzaIRREGOLARITAconrichiamo(ClsUtility.TrovaIDEnteDaIDVerifica(Request.QueryString("IdVerifica"), Session("conn")), Request.QueryString("IdVerifica"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn")) & "');" & vbCrLf)
        Response.Write("</SCRIPT>")
        'chiamo la proprietà della classe GeneratoreModelli che ritorna la path del documento creato
        Documento.Dispose()
    End Sub

    Private Sub cmdContestazione_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles cmdContestazione.Click
        'Dim idverifica As Int16
        'idverifica = Request.QueryString("IdVerifica")
        'Call StampaDocumentiGen(idverifica, "LetteraContestazioneAddebiti")

        Dim Documento As New GeneratoreModelli

        Response.Write("<SCRIPT>" & vbCrLf)
        Response.Write("window.open('" & Documento.MON_LetteraContestazioneAddebiti(ClsUtility.TrovaIDEnteDaIDVerifica(Request.QueryString("IdVerifica"), Session("conn")), Request.QueryString("IdVerifica"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn")) & "');" & vbCrLf)
        Response.Write("</SCRIPT>")
        'chiamo la proprietà della classe GeneratoreModelli che ritorna la path del documento creato
        Documento.Dispose()
    End Sub

    Private Sub CmdLetteraTrasm_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles CmdLetteraTrasm.Click
        'Dim idverifica As Int16
        'idverifica = Request.QueryString("IdVerifica")
        'Call StampaDocumentiGen(idverifica, "LETTERAtrasmDGecontestazione")

        Dim Documento As New GeneratoreModelli

        Response.Write("<SCRIPT>" & vbCrLf)
        Response.Write("window.open('" & Documento.MON_LETTERAtrasmDGecontestazione(ClsUtility.TrovaIDEnteDaIDVerifica(Request.QueryString("IdVerifica"), Session("conn")), Request.QueryString("IdVerifica"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn")) & "');" & vbCrLf)
        Response.Write("</SCRIPT>")
        'chiamo la proprietà della classe GeneratoreModelli che ritorna la path del documento creato
        Documento.Dispose()
    End Sub

    Private Sub cmdDiffida_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles cmdDiffida.Click
        'Dim idverifica As Int16
        'idverifica = Request.QueryString("IdVerifica")
        'Call StampaDocumentiGen(idverifica, "Diffida")

        Dim Documento As New GeneratoreModelli

        Response.Write("<SCRIPT>" & vbCrLf)
        Response.Write("window.open('" & Documento.MON_Diffida(ClsUtility.TrovaIDEnteDaIDVerifica(Request.QueryString("IdVerifica"), Session("conn")), Request.QueryString("IdVerifica"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn")) & "');" & vbCrLf)
        Response.Write("</SCRIPT>")
        'chiamo la proprietà della classe GeneratoreModelli che ritorna la path del documento creato
        Documento.Dispose()
    End Sub

    Private Sub CmdLetteraTrasmissioneProvv_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles CmdLetteraTrasmissioneProvv.Click
        'Dim idverifica As Int16
        'idverifica = Request.QueryString("IdVerifica")
        'Call StampaDocumentiGen(idverifica, "Letteratrasmissioneprovvedimentosanzionatorio")

        Dim Documento As New GeneratoreModelli

        Response.Write("<SCRIPT>" & vbCrLf)
        Response.Write("window.open('" & Documento.MON_Letteratrasmissioneprovvedimentosanzionatorio(ClsUtility.TrovaIDEnteDaIDVerifica(Request.QueryString("IdVerifica"), Session("conn")), Request.QueryString("IdVerifica"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn")) & "');" & vbCrLf)
        Response.Write("</SCRIPT>")
        'chiamo la proprietà della classe GeneratoreModelli che ritorna la path del documento creato
        Documento.Dispose()
    End Sub

    Private Sub CmdTrasmRich_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles CmdTrasmRich.Click
        'Dim idverifica As Int16
        'idverifica = Request.QueryString("IdVerifica")
        'Call StampaDocumentiGen(idverifica, "TrasmissionerelazionealDGconrichiamo")

        Dim Documento As New GeneratoreModelli

        Response.Write("<SCRIPT>" & vbCrLf)
        Response.Write("window.open('" & Documento.MON_TrasmissionerelazionealDGconrichiamo(ClsUtility.TrovaIDEnteDaIDVerifica(Request.QueryString("IdVerifica"), Session("conn")), Request.QueryString("IdVerifica"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn")) & "');" & vbCrLf)
        Response.Write("</SCRIPT>")
        'chiamo la proprietà della classe GeneratoreModelli che ritorna la path del documento creato
        Documento.Dispose()
    End Sub
    Private Sub CmdLETTERATRASMISPROVAISERVIZI_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles CmdLETTERATRASMISPROVAISERVIZI.Click
        'Dim idverifica As Int16
        'idverifica = Request.QueryString("IdVerifica")
        'Call StampaDocumentiGen(idverifica, "LETTERATRASMISPROVAISERVIZI")

        Dim Documento As New GeneratoreModelli

        Response.Write("<SCRIPT>" & vbCrLf)
        Response.Write("window.open('" & Documento.MON_LETTERATRASMISPROVAISERVIZI(ClsUtility.TrovaIDEnteDaIDVerifica(Request.QueryString("IdVerifica"), Session("conn")), Request.QueryString("IdVerifica"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn")) & "');" & vbCrLf)
        Response.Write("</SCRIPT>")
        'chiamo la proprietà della classe GeneratoreModelli che ritorna la path del documento creato
        Documento.Dispose()
    End Sub
    Private Sub CmdTRASMISSIONEPROVVEDIMENTOREGIONE_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles CmdTRASMISSIONEPROVVEDIMENTOREGIONE.Click
        'Dim idverifica As Int16
        'idverifica = Request.QueryString("IdVerifica")
        'Call StampaDocumentiGen(idverifica, "TRASMISSIONEPROVVEDIMENTOREGIONE")

        Dim Documento As New GeneratoreModelli

        Response.Write("<SCRIPT>" & vbCrLf)
        Response.Write("window.open('" & Documento.MON_TRASMISSIONEPROVVEDIMENTOREGIONE(ClsUtility.TrovaIDEnteDaIDVerifica(Request.QueryString("IdVerifica"), Session("conn")), Request.QueryString("IdVerifica"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn")) & "');" & vbCrLf)
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

    Private Sub SchedaInformazioniGenerali(ByVal blnValore As Boolean, ByVal Colore As Color)
        Fascicolo(blnValore, Colore)
        DataCredenziali(blnValore, Colore)
        DataIncarico(blnValore, Colore)
        DataInizoFineVerifica(blnValore, Colore)
        DataProtocolloRelazione(blnValore, Colore)
        DataProtTrasDG(blnValore, Colore)
        DataProtocolloInvioChiusura(blnValore, Colore)
        ImageProtolloCredenziali(blnValore)
        ImageProtolloIncarico(blnValore)
    End Sub


    Private Sub Fascicolo(ByVal blnValore As Boolean, ByVal Colore As Color)
        TxtCodiceFascicolo.BackColor = Colore
        txtDescFasc.BackColor = Colore
        TxtCodiceFascicolo.Enabled = blnValore
        txtDescFasc.Enabled = blnValore
    End Sub

    Private Sub DataCredenziali(ByVal blnValore As Boolean, ByVal Colore As Color)
        TxtNumProtIncarico.BackColor = Colore
        txtDataProtocolloIncarico.BackColor = Colore
        txtDataProtCredenziali.Enabled = blnValore
        txtNumProtCredenziali.Enabled = blnValore

    End Sub

    Private Sub DataInizoFineVerifica(ByVal blnValore As Boolean, ByVal Colore As Color)
        txtDataInizioVerifica.Enabled = blnValore
        txtDataFineVerifica.Enabled = blnValore

        txtDataInizioVerifica.BackColor = Colore
        txtDataFineVerifica.BackColor = Colore
    End Sub

    Private Sub DataIncarico(ByVal blnValore As Boolean, ByVal Colore As Color)
        TxtNumProtIncarico.Enabled = blnValore
        txtDataProtocolloIncarico.Enabled = blnValore
        txtDataProtCredenziali.Enabled = blnValore
        txtNumProtCredenziali.Enabled = blnValore
    End Sub
    Private Sub ImageProtolloCredenziali(ByVal blnValore As Boolean)
        ImgProtolloCredenziali.Visible = blnValore
        ImgApriAllegatiCredenziali.Visible = blnValore
    End Sub
    Private Sub ImageProtolloIncarico(ByVal blnValore As Boolean)
        cmdSc1SelProtocollo1.Visible = blnValore
        cmdSc1Allegati1.Visible = blnValore
    End Sub
    Private Sub DataProtocolloRelazione(ByVal blnValore As Boolean, ByVal Colore As Color)
        txtDataProtocolloRelazione.BackColor = Colore

        txtDataProtocolloRelazione.Enabled = blnValore
        chkRelazione.Enabled = blnValore
    End Sub
    Private Sub DataProtTrasDG(ByVal blnValore As Boolean, ByVal Colore As Color)
        TxtDataProtTrasDG.BackColor = Colore
        TxtNumProtTrasDG.BackColor = Colore

        TxtDataProtTrasDG.Enabled = blnValore
        TxtNumProtTrasDG.BackColor = Colore
    End Sub
    Private Sub ImageProtocolloTrasDG(ByVal blnValore As Boolean)
        ImgProtocolloTrasDG.Visible = blnValore
        ImgApriAllegatiTrasDG.Visible = blnValore
    End Sub
    Private Sub ImageProtocolloLettChiusura(ByVal blnValore As Boolean)
        ImgProtocolloLettChiusura.Visible = blnValore
        ImgApriAllegatiLettChiusura.Visible = blnValore
    End Sub
    Private Sub DataProtocolloInvioChiusura(ByVal blnValore As Boolean, ByVal Colore As Color)
        txtDataProtocolloInvioChiusura.BackColor = Colore
        TxtNumProtocolloInvioLetteraChiusura.BackColor = Colore
        txtDataProtocolloInvioChiusura.Enabled = blnValore
        TxtNumProtocolloInvioLetteraChiusura.Enabled = blnValore
    End Sub
    Private Sub DataProtocolloLetteraContestazioneDG(ByVal blnValore As Boolean, ByVal Colore As Color)
        txtDataProtLetteraContestazioneDG.BackColor = Colore
        txtNProtLetteraContestazioneDG.BackColor = Colore

        txtDataProtLetteraContestazioneDG.Enabled = blnValore
        txtNProtLetteraContestazioneDG.Enabled = blnValore
    End Sub
    Private Sub DataProtocolloInvioLetteraChiusura(ByVal blnValore As Boolean, ByVal Colore As Color)
        txtDataProtocolloInvioChiusura.BackColor = Colore
        TxtNumProtocolloInvioLetteraChiusura.BackColor = Colore

        txtDataProtocolloInvioChiusura.Enabled = blnValore
        TxtNumProtocolloInvioLetteraChiusura.Enabled = blnValore
    End Sub
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
    '*** fine CONTESTAZIONE


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




    '**** sanzione
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
    '***** fine sanzione

    Sub CampiDG(ByVal blnValore As Boolean)
        dvDGPositiva.Visible = blnValore
        dvDGPositiveStampe.Visible = blnValore
        dvDGContestata.Visible = blnValore
        Label24.Visible = blnValore
        CmdLetteraTrasm.Visible = blnValore
        dvDGSanzione.Visible = blnValore
        Label26.Visible = blnValore
        CmdArchiviazione.Visible = blnValore
    End Sub

    Private Sub PersonalizzaMaschera(ByVal StatoVerifica As String, ByVal IdRegCompetenza As Integer)
        'DisabilitaImmaginiCalendario()
        'DisabilitaText()
        DisabilitaPulsantiStampa()
        'ColoraCampi()
        chkNonEffettuata.Enabled = False
        chkRichiamo.Enabled = False
        imgAssegnaVer.Visible = False

        cmdIncludi.Visible = False
        cmdSospendi.Visible = False
        cmdChiusaContestata.Visible = False
        cmdRipristina.Visible = False

        'cmdSanzione.Enabled = False
        lkbSanzione.Enabled = False

        '** agg da s.c il 15/10/2010 rendo invisibili pulsante del fascicolo
        cmdSelFascicolo.Visible = False
        cmdSelProtocollo0.Visible = False
        cmdFascCanc.Visible = False
        '***
        '** agg  da s.c. il 25/11/2010 disabilito la colonna della griglia per la sostituzione della sede
        dgRisultatoRicerca.Columns(11).Visible = False
        '**
        'Modificato Antonello Di Croce 02/10/2007
        If TrovaTipologiaVerificatore(txtIdVerificatore.Value) = 1 Then
            CmdStampaCredenzialiIncaricoIGF.Visible = True
            CmdStampaCredenzialiIncaricoIGF.Enabled = True
            CmdStampaLettereIncaricoIGF.Visible = True
            CmdStampaLettereIncaricoIGF.Enabled = True
            CmdStampaCredenzialiIncarico.Visible = False
            CmdStampaLettereIncarico.Visible = False
        Else
            CmdStampaCredenzialiIncarico.Visible = True
            CmdStampaCredenzialiIncarico.Enabled = True
            CmdStampaLettereIncarico.Visible = True
            CmdStampaLettereIncarico.Enabled = True
            CmdStampaCredenzialiIncaricoIGF.Visible = False
            CmdStampaLettereIncaricoIGF.Visible = False
        End If
        '** 25/11/2010
        'Abilito_DisabilitoDataInizioFineVerifica(False, "visibility: visible", Color.White)

        'PersonalizzaFascicolo_Regionalizzazione(IdRegCompetenza, StatoVerifica)
        If IdRegCompetenza = 22 Then
            CampiDG(False)
        Else
            CampiDG(True)
        End If
        imgSanzioneServizi.Attributes.Add("style", "visibility: hidden")
        cmdAnnullata.Visible = False

        SchedaInformazioniGenerali(False, Color.LightGray)
        SchedaContestatazione(False, Color.LightGray)
        SchedaSanzione(False, Color.LightGray)
        Dim intIDProfiloMaster As Integer
        intIDProfiloMaster = ClsUtility.TrovaProfiloUtente(Me.TemplateSourceDirectory, Session("Utente"), Session("Conn"))

        If intIDProfiloMaster <> 0 Then
            cmdAnnullata.Visible = False
            cmdSospendi.Visible = False
            cmdRipristina.Visible = False
        End If

        Select Case StatoVerifica
            Case "Aperta"
                '** agg  da s.c. il 25/11/2010 abilito la colonna della griglia per la sostituzione della sede(solo per la verifica aperta)
                dgRisultatoRicerca.Columns(11).Visible = True
                '**
                ''agg da sc il 15/10/2010
                'TxtCodiceFascicolo.BackColor = Color.White
                'txtDescFasc.BackColor = Color.White

                ''21/10/2016 da s.c.
                Fascicolo(True, Color.White)
                DataCredenziali(True, Color.White)
                ImageProtolloCredenziali(True)
                DataInizoFineVerifica(True, Color.White)
                DataIncarico(True, Color.White)
                ImageProtolloIncarico(True)

                DataProtocolloRelazione(False, Color.LightGray)
                DataProtTrasDG(False, Color.LightGray)
                DataProtocolloInvioChiusura(False, Color.LightGray)
                If IdRegCompetenza = 22 Then
                    cmdSelFascicolo.Visible = True
                    cmdSelProtocollo0.Visible = True
                    cmdFascCanc.Visible = True
                End If

                cmdIncludi.Visible = True

                cmdSalva.Visible = True
                imgAssegnaVer.Visible = True


                If intIDProfiloMaster = 0 Then
                    cmdSospendi.Visible = True
                    cmdRipristina.Visible = False
                    cmdAnnullata.Visible = True
                End If
            Case "In Esecuzione"
                '** modificato il 31/03/2011 da s.c. abilito la colonna della griglia per la sostituzione della sede(solo per la verifica in esecuzione)
                dgRisultatoRicerca.Columns(11).Visible = True
                '**

                '21/10/2016 da s.c.
                Fascicolo(True, Color.White)
                DataInizoFineVerifica(True, Color.White)
                DataIncarico(True, Color.White)
                ImageProtolloIncarico(False)
                DataCredenziali(True, Color.White)
                ImageProtolloCredenziali(False)




                ''agg. da sc il 15/10/2010

                If IdRegCompetenza = 22 Then
                    'agg. da sc il 15/10/2010 
                    cmdSelFascicolo.Visible = True
                    cmdSelProtocollo0.Visible = True
                    cmdFascCanc.Visible = True
                    '****
                End If
                imgAssegnaVer.Visible = True
                If intIDProfiloMaster = 0 Then
                    cmdSospendi.Visible = True
                    cmdRipristina.Visible = False
                    cmdAnnullata.Visible = True
                End If
            Case "Eseguita"
                If IdRegCompetenza = 22 Then
                    cmdSelProtocollo0.Visible = True
                End If
                If intIDProfiloMaster = 0 Then
                    cmdSospendi.Visible = True
                    cmdRipristina.Visible = False
                    cmdAnnullata.Visible = True
                End If

                Fascicolo(True, Color.White)
                'DataIncarico(True, Color.White)
                'DataCredenziali(True, Color.White)
                DataInizoFineVerifica(True, Color.White)
                DataIncarico(True, Color.White)
                ImageProtolloIncarico(False)
                DataCredenziali(True, Color.White)
                ImageProtolloCredenziali(False)
                DataProtocolloRelazione(True, Color.White)

                DataProtTrasDG(False, Color.LightGray)
                DataProtocolloInvioChiusura(False, Color.LightGray)

                'se 
                If txtDataProtocolloRelazione.Text <> "" Then
                    'agg da s.c. il 13.07.2009 gestione del nuovo stato "CHIUSA NON EFFETTUATA"
                    chkNonEffettuata.Enabled = True
                    chkRichiamo.Enabled = True

                    '21/10/2016
                    DataProtTrasDG(True, Color.White)
                    DataProtocolloInvioChiusura(True, Color.White)


                    cmdTrasmissione.Enabled = True
                    CmdConclusione.Enabled = True
                    cmdContestazione.Enabled = True
                    'mod da sc 14/10/2010
                    If TxtDataProtTrasDG.Text <> "" Then 'se la trasmissome lettere positiva alDG è valorizzata, disabilito lettera contestazione al DG
                        '21/10/2016
                        DataProtocolloLetteraContestazioneDG(False, Color.LightGray)
                        CmdLetteraTrasm.Enabled = False
                        cmdContestazione.Enabled = False
                    Else
                        'posso modificare lo stato in contestata
                        '21/10/2016
                        DataProtocolloLetteraContestazioneDG(True, Color.White)

                        CmdLetteraTrasm.Enabled = True

                    End If

                    If chkNonEffettuata.Checked = True Then 'se il flag è a true disabilito le text date e num protocollo chiusura positiva e quelle della contestazione

                        '21/10/2016
                        DataProtocolloInvioChiusura(False, Color.LightGray)
                        DataProtocolloLetteraContestazioneDG(False, Color.LightGray)
                        chkRichiamo.Enabled = False

                    End If
                    '21/10/2016
                    DataProtocolloInvioLetteraChiusura(True, Color.White)

                    DataProtocolloInvioLetteraContestazione(True, Color.White)
                    ImageProtocolloInvioLettContestazione(True)
                    If IdRegCompetenza <> 22 Then 'ABILITO immagine calendeario e rendo editabile le text
                        ''imgDataProtocolloInvioLetteraContestazione.Attributes.Add("style", "visibility: visible")
                        '   txtDataProtocolloInvioLetteraContestazione.ReadOnly = False
                        '                txtDataProtocolloInvioLetteraContestazione.Enabled = True

                        '  TxtNumeroProtocolloInvioLettContestazione.ReadOnly = False
                    End If
                    'TxtDataProtTrasDG.BackColor = Color.LightGray
                    'TxtNumProtTrasDG.BackColor = Color.LightGray
                    cmdTrasmissione.Enabled = True 'lettera trasmissione posivito o con richiamo al D.g.

                    If chkRichiamo.Checked = True Then
                        CmdConclusione.Enabled = False
                        CmdIrregolarita.Enabled = True 'lettera chiusura con richiamo
                    Else
                        CmdIrregolarita.Enabled = False
                        CmdConclusione.Enabled = True 'lettera chiudeura positiva
                    End If
                    cmdContestazione.Enabled = True 'lettera invio contestazione (Addebiti)
                    CmdLetteraTrasm.Enabled = True 'lettera trasmissione Contestazione al D.g.
                    'End If
                End If
            Case "Chiusa Positivamente"
                CmdConclusione.Enabled = True
                cmdTrasmissione.Enabled = True
                CmdIrregolarita.Enabled = False
                CmdTrasmRich.Enabled = False
                '** 25/11/2010
                cmdSalva.Visible = False
                If IdRegCompetenza = 22 Then
                    cmdSelProtocollo0.Visible = True
                End If
             
                'CmdConclusione.Enabled = True
            Case "Chiusa con Richiamo"
                If IdRegCompetenza = 22 Then
                    cmdSelProtocollo0.Visible = True
                End If
                CmdConclusione.Enabled = False
                cmdTrasmissione.Enabled = False
                CmdIrregolarita.Enabled = True
                CmdTrasmRich.Enabled = True
                cmdSalva.Visible = False
            Case "Chiusa non Effettuata" 'nuovo stato agg. da s.c il 13/07/2009 (spesso gestione della chiusa con richiamo) 
                CmdConclusione.Enabled = True
                cmdTrasmissione.Enabled = True
                CmdIrregolarita.Enabled = False
                CmdTrasmRich.Enabled = False
                If IdRegCompetenza = 22 Then
                    cmdSelProtocollo0.Visible = True
                End If
            Case "Contestata"
                cmdContestazione.Enabled = True
                CmdLetteraTrasm.Enabled = True
                If IdRegCompetenza = 22 Then
                    cmdSelProtocollo0.Visible = True
                End If
                'Fascicolo(False, Color.LightGray)
                'DataInizoFineVerifica(False, Color.LightGray)
                'DataIncarico(False, Color.LightGray)
                'DataCredenziali(False, Color.LightGray)
                'DataProtocolloRelazione(False, Color.LightGray)
                'DataProtTrasDG(False, Color.LightGray)
                'DataProtocolloInvioChiusura(False, Color.LightGray)

                DataProtocolloInvioLetteraContestazione(True, Color.White)
                ImageProtocolloInvioLettContestazione(True)


                If txtDataProtocolloInvioLetteraContestazione.Text <> "" Then
                    DataProtocolloInvioLetteraContestazione(False, Color.LightGray)
                    DataRicezioneLetteraContestazione(True, Color.White)
                    DataProtocolloRispostaContestazione(True, Color.White)
                    ImageProtocolloRispContestazione(True)
                    DataProtocolloTrasmissioneSanzione(True, Color.White)
                    ImageProtocolloTrasmissioneSanzione(True)

                    DataProtocolloEsecuzioneSanzione(True, Color.White)
                    ImageProtocolloEsecuzioneSanzione(True)
                    CmdArchiviazione.Enabled = True 'sanzione al d.g.


                    'If txtDataRicezioneLetteraContestazione.Text <> "" Then
                    '    DataRispostaLetteraContestazione(True, Color.White) 'controdeduzioni
                    '    DataProtocolloRispostaContestazione(True, Color.White)
                    '    ImageProtocolloRispContestazione(True)

                    '    DataProtocolloTrasmissioneSanzione(True, Color.White)
                    '    ImageProtocolloTrasmissioneSanzione(True)

                    '    DataProtocolloEsecuzioneSanzione(True, Color.White)
                    '    ImageProtocolloEsecuzioneSanzione(True)
                    '    CmdArchiviazione.Enabled = True 'sanzione al d.g.

                    'End If

                    If txtDataProtocolloRispostaContestazione.Text <> "" Then 'txtDataProtocolloRispostaContestazione

                        'Data Ricezione Lettera Contestazione  --> txtDataRicezioneLetteraContestazione()
                        'Data Lettera Controdeduzioni	       --> txtDataRispostaLetteraContestazione
                        'Data Prot. Lettera Controdeduzioni    --> txtDataProtocolloRispostaContestazione	
                        'Data Prot. Chiusura Contestazione     --> TxtDataProtChiusuraContestazion
                        ImageProtocolloRispContestazione(True)
                        DataRispostaLetteraContestazione(True, Color.White) 'controdeduzioni

                        DataProtocolloRispostaContestazione(True, Color.White)
                        DataProtChiusuraContestazione(False, Color.LightGray)

                        cmdChiusaContestata.Visible = True
                    End If
                    cmdContestazione.Enabled = True
                    CmdLetteraTrasm.Enabled = True
                End If
                If txtDataProtocolloTrasmissioneSanzione.Text <> "" Then 'trasmissione sanziona la d.g. se valorizzato abilito invio sanzione all'ente
                    DataProtocolloEsecuzioneSanzione(True, Color.White)
                    ImageProtocolloEsecuzioneSanzione(True)
                    If IdRegCompetenza <> 22 Then
                        If txtDataProtocolloEsecuzioneSanzione.Text = "" Then
                            cmdChiusaContestata.Visible = True
                            lkbSanzione.Enabled = False
                        Else
                            cmdChiusaContestata.Visible = False
                            lkbSanzione.Enabled = True
                        End If

                    End If
                End If
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
                If IdRegCompetenza = 22 Then
                    cmdSelProtocollo0.Visible = True
                End If
                'SchedaInformazioniGenerali(False, Color.LightGray)
                'SchedaContestatazione(False, Color.LightGray)

                'visualizzo anche i dati per la sanzione
                DataProtocolloEsecuzioneSanzione(True, Color.White)
                ImageProtocolloEsecuzioneSanzione(True)
                DataProtocolloTrasmissioneServizi(True, Color.White)
                ImageProtocolloTrasmServizi(True)
                If txtDataProtocolloEsecuzioneSanzione.Text <> "" Then 'abilito trasmissione servizio
                    'controllo se ho indicato il/i servizio/i a cui appicare il protocolloTrasmissione Servizi
                    If IdRegCompetenza = 22 Then
                        If txtDataEsecuzioneSanzione.Text = "" Then
                            txtDataEsecuzioneSanzione.Text = txtDataProtocolloEsecuzioneSanzione.Text
                        End If
                        imgSanzioneServizi.Attributes.Add("style", "visibility: visible")
                    Else
                        imgSanzioneServizi.Attributes.Add("style", "visibility: hidden")
                        DataProtocolloTrasmissioneServizi(True, Color.White)
                        ImageProtocolloTrasmServizi(True)
                    End If
                End If


                'If IdRegCompetenza = 22 Then
                '    ' ReadOnlyProcolliSANZIONE(True, "visibility: hidden")
                'Else
                '    ' ReadOnlyProcolliSANZIONERPA()
                '    'If txtDataProtocolloTrasmServizi.Text = "" Then
                '    '    txtDataProtocolloTrasmServizi.Text = txtDataProtocolloEsecuzioneSanzione.Text
                '    '    txtNProtocolloTrasmServizi.Text = TxtNumeroProtocolloEsecuzioneSanzione.Text
                '    'End If
                'End If


                lkbSanzione.Enabled = True
                VerificaCompetenzaEnte(hftxtidente.Value)


                cmdDiffida.Enabled = True
                CmdLetteraTrasmissioneProvv.Enabled = True
                CmdLETTERATRASMISPROVAISERVIZI.Enabled = True
                CmdTRASMISSIONEPROVVEDIMENTOREGIONE.Enabled = True
                CmdArchiviazione.Enabled = True
                CmdInterdizione.Enabled = True
                CmdRevoca.Enabled = True
                CmdCancellazione.Enabled = True

                cmdIncludi.Visible = False
                If txtDataEsecuzioneSanzione.Text <> "" Then
                    If txtDataProtocolloTrasmServizi.Text <> "" Then
                        DataProtocolloEsecuzioneSanzione(False, Color.LightGray)
                        DataProtocolloTrasmissioneServizi(False, Color.LightGray)
                        DataEsecuzioneSanzione(False, Color.LightGray)
                    End If
                End If
            Case "Sospesa"
                If IdRegCompetenza = 22 Then
                    cmdSelProtocollo0.Visible = True
                End If
                cmdRipristina.Visible = True
                cmdSalva.Visible = False
                cmdIncludi.Visible = False
            Case "Chiusa Contestata"
                If IdRegCompetenza = 22 Then
                    cmdSelProtocollo0.Visible = True
                End If
                'tutto disabilitato

                cmdChiusaContestata.Visible = False
                CmdLetteraChiusContestazione.Enabled = True

                DataProtChiusuraContestazione(True, Color.White)
                ImageProtocolloProtChiusuraContestazione(True)
                If IdRegCompetenza = 22 Then
                    DataProtChiusuraContestazione(False, Color.LightGray)
                    ImageProtocolloProtChiusuraContestazione(False)
                    cmdSalva.Visible = False
                Else
                    DataProtChiusuraContestazione(True, Color.White)
                    ImageProtocolloProtChiusuraContestazione(True)
                    cmdSalva.Visible = True
                End If
                If TxtDataProtChiusuraContestazione.Text <> "" Then
                    cmdSalva.Visible = False
                    DataProtChiusuraContestazione(False, Color.LightGray)
                    ImageProtocolloProtChiusuraContestazione(False)
                End If
                '** 25/11/2010
                DataInizoFineVerifica(False, Color.LightGray)
                'aggiunto da simona cordella 26/10/2011
                If ControlloSanzione() = True Then
                    'cmdSanzione.Enabled = True
                    lkbSanzione.Enabled = True
                End If
            Case "Annullata"
                If IdRegCompetenza = 22 Then
                    cmdSelProtocollo0.Visible = True
                End If
                cmdSalva.Visible = False
                cmdStampaVQ.Visible = False
        End Select
        If IdRegCompetenza <> 22 Then
            ImageProtolloCredenziali(False)
            ImageProtolloIncarico(False)
            ImageProtocolloTrasDG(False)
            ImageProtocolloLettChiusura(False)
            ImageProtocolloTrasContestataDG(False)
            ImageProtocolloInvioLettContestazione(False)
            ImageProtocolloRispContestazione(False)
            ImageProtocolloProtChiusuraContestazione(False)
            ImageProtocolloTrasmissioneSanzione(False)
            ImageProtocolloEsecuzioneSanzione(False)
            ImageProtocolloTrasmServizi(False)
        End If
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
                Strsql = Strsql & " VALUES (" & hftxtidente.Value & " ," & IdSanzione & " ," & hftxtIdVerifica.Value & ")"
            Else 'TVerificheSanzioniProgetto
                Strsql = "INSERT INTO TVerificheSanzioniProgetto (IDAttività, IDSanzione, IDVerifica)"
                Strsql = Strsql & " VALUES (" & Session("IDAttività") & "," & IdSanzione & " ," & hftxtIdVerifica.Value & ")"
            End If
            CmdGenerico = ClsServer.EseguiSqlClient(Strsql, Session("conn"))
        End If
    End Sub

    Private Sub AggiornaTVerifiche()
        Dim strNull As String = "Null"
        Dim CmdGenerico As SqlClient.SqlCommand
        Dim strsql As String
        Dim intStatoVerifiche As Integer

        intStatoVerifiche = ImpostaStato(lblStatoVerifica.Text)
        strsql = "Update TVerifiche set "

        strsql &= "IDStatoVerifica=" & intStatoVerifiche & ","
        If TxtCodiceFascicolo.Text <> "" Then
            strsql &= "CodiceFascicolo = '" & TxtCodiceFascicolo.Text & "' ,IDFascicolo ='" & hfTxtCodiceFasc.Value & "' , DescrFascicolo ='" & txtDescFasc.Text.Replace("'", "''") & "' , "
            ' strsql &= "CodiceFascicolo = '" & TxtCodiceFascicolo.Text & "' ,IDFascicolo ='" & TxtCodiceFasc.Text & "' , DescrFascicolo ='" & txtDescFasc.Text.Replace("'", "''") & "' , "
        Else
            strsql &= " CodiceFascicolo = " & strNull & ", IDFascicolo = " & strNull & ", DescrFascicolo = " & strNull & ",  "
        End If
        If txtDataInizioVerifica.Text <> "" Then
            strsql &= " DataInizioVerifica = '" & txtDataInizioVerifica.Text & "' , "
        Else
            strsql &= " DataInizioVerifica = " & strNull & ", "
        End If
        If txtDataFineVerifica.Text <> "" Then
            strsql &= " DataFineVerifica = '" & txtDataFineVerifica.Text & "' , "
        Else
            strsql &= " DataFineVerifica = " & strNull & ", "
        End If
        If txtDataProtocolloIncarico.Text <> "" Then
            strsql &= " DataProtIncarico= '" & txtDataProtocolloIncarico.Text & "', "
        Else
            strsql &= " DataProtIncarico= " & strNull & ", "
        End If
        If txtDataProtCredenziali.Text <> "" Then
            strsql &= " DataProtInvioFax= '" & txtDataProtCredenziali.Text & "', "
        Else
            strsql &= " DataProtInvioFax= " & strNull & ", "
        End If
        If txtNumProtCredenziali.Text <> "" Then
            strsql &= " NProtInvioFax= '" & txtNumProtCredenziali.Text & "', "
        Else
            strsql &= " NProtInvioFax= " & strNull & ", "
        End If

        If TxtNumProtIncarico.Text <> "" Then
            strsql &= " NProtIncarico = '" & TxtNumProtIncarico.Text & "', "
        Else
            strsql &= " NProtIncarico= " & strNull & ", "
        End If
        If chkRelazione.Checked = True Then
            strsql &= " Relazione= 1,"
        Else
            strsql &= " Relazione= 0,"
        End If
        If chkNonEffettuata.Checked = True Then
            strsql &= " NonEffettuata= 1,"
        Else
            strsql &= " NonEffettuata= 0,"
        End If
        If txtDataProtocolloRelazione.Text <> "" Then
            strsql &= " DataProtRelazione = '" & txtDataProtocolloRelazione.Text & "', "
        Else
            strsql &= " DataProtRelazione = " & strNull & ", "
        End If
        ' scarico il data protocollo tasmissione al d.g.
        If TxtDataProtTrasDG.Text <> "" Then
            strsql &= " DataInvioLetteraChiusuraProc = '" & TxtDataProtTrasDG.Text & "', "
        Else
            strsql &= " DataInvioLetteraChiusuraProc = " & strNull & ", "
        End If
        'scarico il numero protocollo trasmissione al d.g.
        If TxtNumProtTrasDG.Text <> "" Then
            strsql &= " NProtRelazione = '" & TxtNumProtTrasDG.Text & "', "
        Else
            strsql &= " NProtRelazione = " & strNull & ", "
        End If
        'If txtnum Then
        'strsql &= " NProtRelazione, "
        'If txtData Then
        '    strsql &= " DataInvioLetteraChiusuraProc, "
        If txtDataProtocolloInvioChiusura.Text <> "" Then
            strsql &= " DataProtInvioLetteraChiusuraProc = '" & txtDataProtocolloInvioChiusura.Text & "', "
        Else
            strsql &= " DataProtInvioLetteraChiusuraProc = " & strNull & ", "
        End If
        If TxtNumProtocolloInvioLetteraChiusura.Text <> "" Then
            strsql &= " NProtInvioLetteraChiusuraProc= '" & TxtNumProtocolloInvioLetteraChiusura.Text & "',"
        Else
            strsql &= " NProtInvioLetteraChiusuraProc =" & strNull & ","
        End If
        If txtDataProtocolloInvioLetteraContestazione.Text <> "" Then
            strsql &= "DataProtInvioLetteraContestazione ='" & txtDataProtocolloInvioLetteraContestazione.Text & "', "
        Else
            strsql &= "DataProtInvioLetteraContestazione =" & strNull & ", "
        End If

        'strsql &= "DataInvioLetteraContestazione, "
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
            strsql &= "  DataProtRispostaLetteraContestazion = '" & txtDataProtocolloRispostaContestazione.Text & "', "
        Else
            strsql &= "  DataProtRispostaLetteraContestazion = " & strNull & ","
        End If
        If TxtNumProtocolloRispostaContestazione.Text <> "" Then
            strsql &= "  NProtRispostaLetteraContestazion = '" & TxtNumProtocolloRispostaContestazione.Text & "', "
        Else
            strsql &= "  NProtRispostaLetteraContestazion = " & strNull & ","
        End If

        'strsql &= " DataTrasmissioneSanzione,"
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
        If ddlUfficio.SelectedValue <> 0 Then
            strsql &= " IDUfficioUNSC =" & ddlUfficio.SelectedValue & ","
        Else
            strsql &= " IDUfficioUNSC =" & strNull & ","
        End If
        If ddlCompetenze.SelectedValue <> 0 Then
            strsql &= " IDRegioneCompetenza =" & ddlCompetenze.SelectedValue & ","
        Else
            strsql &= " IDRegioneCompetenza =" & strNull & ","
        End If

        strsql &= " UserUltimaModifica ='" & Session("Utente") & "', "
        strsql &= " DataUltimaModifica =getdate(), "
        strsql &= " Note ='" & Replace(TxtNote.Text, "'", "''") & "', "

        If chkRichiamo.Checked = True Then
            strsql &= " Richiamo =1 ,"
        Else
            strsql &= " Richiamo =0 ,"
        End If
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
            strsql &= " NProtTrasmissioneServizi ='" & txtNProtocolloTrasmServizi.Text & "' "
        Else
            strsql &= " NProtTrasmissioneServizi =" & strNull & " "
        End If
        '****

        strsql &= " Where idverifica = " & Request.QueryString("IdVerifica")

        CmdGenerico = ClsServer.EseguiSqlClient(strsql, Session("conn"))
        lblErrore.Text = "Salvataggio eseguito con successo."
    End Sub

    Private Sub DisabilitaPulsantiStampa()
        CmdStampaCredenzialiIncarico.Enabled = False
        CmdStampaLettereIncarico.Enabled = False
        CmdStampaCredenzialiIncarico.Enabled = False
        CmdStampaLettereIncarico.Enabled = False
        CmdStampaCredenzialiIncaricoIGF.Enabled = False
        CmdStampaLettereIncaricoIGF.Enabled = False
        'cmdRelazioneStand.Enabled = False
        cmdTrasmissione.Enabled = False
        CmdConclusione.Enabled = False
        CmdIrregolarita.Enabled = False
        cmdContestazione.Enabled = False
        CmdLetteraTrasm.Enabled = False
        cmdDiffida.Enabled = False
        CmdLetteraTrasmissioneProvv.Enabled = False
        CmdTrasmRich.Enabled = False

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
        'Creata da Simona Cordella il 02/07/2007 
        'Controlli per il modificare lo stato della Verifiche
        ' Dim intStato As Integer
        Select Case StatoPartenza
            Case "Aperta"
                If VerificaDatiStato(StatoPartenza) = False Then
                    ImpostaStato = 5
                Else
                    ImpostaStato = 6 'in esecuzione
                End If
            Case "In Esecuzione"
                If ControllaEsito() = False Then ' se non ho confermato gli esiti non posso cambiare lo stato
                    ImpostaStato = 6 'in esecuzione
                Else
                    'manca controllo su requisiti
                    If VerificaDatiStato(StatoPartenza) = False Then
                        ImpostaStato = 6 'in esecuzione
                    Else
                        ImpostaStato = 7 'eseguita
                    End If
                End If
            Case "Eseguita"
                ImpostaStato = 7
                ' se chkNonEffettuata = TRUE  la verifica è CHIUSA NON EFFETTUATA se ha indicato la data è il protocollo al DG

                'If TxtDataProtTrasDG.Text <> "" And TxtNumProtTrasDG.Text <> "" Then
                If chkNonEffettuata.Checked = True Then 'gestione nuovo stato Chiusa non effettuata
                    ImpostaStato = 15
                End If
                'End If
                If txtDataProtocolloInvioChiusura.Text <> "" And TxtNumProtocolloInvioLetteraChiusura.Text <> "" Then

                    If chkRichiamo.Checked = False Then 'chiusa positivamente
                        ImpostaStato = 8
                    Else
                        ImpostaStato = 9
                    End If
                ElseIf txtDataProtocolloInvioLetteraContestazione.Text <> "" And _
                         TxtNumeroProtocolloInvioLettContestazione.Text <> "" Then 'CONTESTATA
                    'mod da sc 14/10/2010 nn è più così diventa contestato se ho indicato il protocollo invio lettera contestazione
                    'ElseIf txtDataProtLetteraContestazioneDG.Text <> "" And _
                    '         txtNProtLetteraContestazioneDG.Text <> "" Then 'CONTESTATA
                    ImpostaStato = 10

                End If


            Case "Chiusa Positivamente"
                ImpostaStato = 8
            Case "Chiusa con Richiamo"
                ImpostaStato = 9
            Case "Contestata"
                If VerificaDatiStato(StatoPartenza) = False Then
                    ImpostaStato = 10 'contestata
                Else
                    If ControlloSanzioniCompleto(CInt(hftxtIdVerifica.Value)) = True Then
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
        'lblErrore.Text = ""

        Select Case StatoPartenza
            Case "Aperta"

                If txtDataProtocolloIncarico.Text = "" And TxtNumProtIncarico.Text = "" Then
                    VerificaDatiStato = False
                End If
                If txtDataProtocolloIncarico.Text <> "" And TxtNumProtIncarico.Text = "" Then
                    VerificaDatiStato = False
                End If
                If txtDataProtocolloIncarico.Text = "" And TxtNumProtIncarico.Text <> "" Then
                    VerificaDatiStato = False
                End If
            Case "In Esecuzione"
                If txtDataInizioVerifica.Text = "" And txtDataFineVerifica.Text <> "" Then
                    'lblErrore.Text = "E' neccessario indicare la data inizio verifica."
                    VerificaDatiStato = False
                End If
                If txtDataInizioVerifica.Text <> "" And txtDataFineVerifica.Text = "" Then
                    ' lblErrore.Text = "E' neccessario indicare la data fine verifica."
                    VerificaDatiStato = False
                End If
                If txtDataInizioVerifica.Text = "" And txtDataFineVerifica.Text = "" Then
                    'lblErrore.Text = "E' neccessario indicare la data inizio verifica."
                    VerificaDatiStato = False
                End If
                If txtDataInizioVerifica.Text > txtDataFineVerifica.Text Then
                    'lblErrore.Text = "La data inizio verifica deve essere minore o uguale alla data fine verifica."
                    VerificaDatiStato = False
                End If
            Case "Eseguita"
                If txtDataProtocolloInvioChiusura.Text = "" And TxtNumProtocolloInvioLetteraChiusura.Text = "" Then
                    'mod da sc 14/10/2010
                    'If txtDataProtocolloInvioLetteraContestazione.Text = "" And TxtNumeroProtocolloInvioLettContestazione.Text = "" Then
                    '    VerificaDatiStato = False
                    'End If
                    If txtDataProtLetteraContestazioneDG.Text = "" And txtNProtLetteraContestazioneDG.Text = "" Then
                        VerificaDatiStato = False
                    End If
                End If
                If txtDataProtocolloInvioChiusura.Text = "" And TxtNumProtocolloInvioLetteraChiusura.Text = "" Then
                    VerificaDatiStato = False
                End If
                If txtDataProtocolloInvioChiusura.Text = "" And TxtNumProtocolloInvioLetteraChiusura.Text <> "" Then
                    VerificaDatiStato = False
                End If
                If txtDataProtocolloInvioChiusura.Text <> "" And TxtNumProtocolloInvioLetteraChiusura.Text = "" Then
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

    Private Sub chkRichiamo_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkRichiamo.CheckedChanged
        Irregolarità()
    End Sub
    Private Function ControllaEsito() As Boolean
        Dim strsql As String
        Dim dtrgenerico As SqlClient.SqlDataReader
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
        strsql = " Select ISNULL(TVerifiche.ConfermaRequisiti, 0) AS ConfermaRequisiti" & _
                " FROM TVerifiche  where idverifica =" & Request.QueryString("IdVerifica") & " "
        dtrgenerico = ClsServer.CreaDatareader(strsql, Session("conn"))
        If dtrgenerico.HasRows = True Then
            dtrgenerico.Read()
            If dtrgenerico("ConfermaRequisiti") = True Then
                ControllaEsito = True
            Else
                ControllaEsito = False
            End If
        End If
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
        Return ControllaEsito
    End Function
    Private Sub cmdAssegnaVer_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        '   Response.Redirect("ver_AssegnaVerificatoriSupp.aspx?IdVerifica=" & txtIdVerifica.Text & "&IdEnteSedeAttuazione=" & txtIdEnteSedeAtt.Text & " &StatoVErifica=" & lblStatoVerifica.Text & " &CodiceProgetto=" & hftxtCodProgetto.Text & "&IdVerificatore=" & txtIdVerificatore.Value & "")
    End Sub
    Private Function CaricaVerificatoreSupporto() As String
        Dim StrSql As String
        Dim dtrVer As SqlClient.SqlDataReader

        If Not dtrVer Is Nothing Then
            dtrVer.Close()
            dtrVer = Nothing
        End If

        StrSql = " SELECT TVerificatori.Cognome + ' ' + TVerificatori.Nome + ' (' + CASE WHEN TVerificatori.Tipologia = 0 THEN 'Interno' ELSE 'IGF' END + ' )' AS Verificatore"
        StrSql = StrSql & " FROM TVerificheVerificatori "
        StrSql = StrSql & " INNER JOIN  TVerificatori ON TVerificheVerificatori.IDVerificatore = TVerificatori.IDVerificatore"
        StrSql = StrSql & " WHERE TVerificheVerificatori.Principale = 0  AND TVerificheVerificatori.IDVerifica = " & hftxtIdVerifica.Value & ""
        dtrVer = ClsServer.CreaDatareader(StrSql, Session("conn"))
        If dtrVer.HasRows = True Then
            dtrVer.Read()
            CaricaVerificatoreSupporto = dtrVer("Verificatore")
        Else
            CaricaVerificatoreSupporto = ""
        End If

        If Not dtrVer Is Nothing Then
            dtrVer.Close()
            dtrVer = Nothing
        End If
        Return CaricaVerificatoreSupporto
    End Function

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

        End If
        If Not dtrComp Is Nothing Then
            dtrComp.Close()
            dtrComp = Nothing
        End If
    End Sub

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
    Private Function VericaAssocazioneVersione(ByVal IDAttivita As Integer) As Boolean
        'true è associata ad una versione
        'false non è associata a nessuna versione
        Dim strSql As String
        Dim dtrgenerico As SqlClient.SqlDataReader

        strSql = " SELECT  isnull(bando.IDVersioneVerifiche,0) as IDVersioneVerifiche "
        strSql &= " FROM attività "
        strSql &= " INNER JOIN BandiAttività ON attività.IDBandoAttività = BandiAttività.IdBandoAttività "
        strSql &= " INNER JOIN bando ON BandiAttività.IdBando = bando.IDBando"
        strSql &= " WHERE attività.IdAttività =  " & IDAttivita

        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
        dtrgenerico = ClsServer.CreaDatareader(strSql, Session("conn"))
        If dtrgenerico.HasRows = True Then
            dtrgenerico.Read()
            If dtrgenerico("IDVersioneVerifiche") = 0 Then
                VericaAssocazioneVersione = False
            Else
                VericaAssocazioneVersione = True
            End If
        End If
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
    End Function

    Private Sub dgRisultatoRicerca_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgRisultatoRicerca.ItemCommand
        Dim idverifica As Int16
        Select Case e.CommandName
            Case "Requisiti"
                If VericaAssocazioneVersione(Session("IdAttività")) = True Then
                    Response.Redirect("dettaglioverificarequisiti.aspx?StatoRequisiti=" & e.Item.Cells(8).Text & "&IdEnte=" & hftxtidente.Value & "&IdEnteSedeAttuazione=" & e.Item.Cells(1).Text & "&StatoVerifica=" & lblStatoVerifica.Text & "&IDVerificheAssociate=" & e.Item.Cells(10).Text & "&IdVerifica=" & e.Item.Cells(9).Text)
                Else
                    Response.Write("<SCRIPT>" & vbCrLf)
                    Response.Write("alert('Non esistono requisiti associati alla verifica.')" & vbCrLf)
                    Response.Write("</SCRIPT>")
                End If
            Case "Relazione"
                'agg. da sc non è possibile stampare se non sono stati confermati i requisiti
                If e.Item.Cells(8).Text = "No" Then
                    Response.Write("<SCRIPT>" & vbCrLf)
                    Response.Write("alert('Non è possibile stampare la relazione se non sono stati confermati i requisiti.')" & vbCrLf)
                    Response.Write("</SCRIPT>")
                Else
                    Dim idverificaassociata As Integer
                    idverificaassociata = e.Item.Cells(10).Text
                    'idattivitaentesedeattuazione = txtIdEnteSedeAtt.Text
                    'idverifica = Request.QueryString("IdVerifica")

                    Dim Documento As New GeneratoreModelli

                    Response.Write("<SCRIPT>" & vbCrLf)
                    Response.Write("window.open('" & Documento.MON_relazionestand(ClsUtility.TrovaIDEnteDaIDVerifica(Request.QueryString("IdVerifica"), Session("conn")), idverificaassociata, Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn")) & "');" & vbCrLf)
                    Response.Write("</SCRIPT>")
                    'chiamo la proprietà della classe GeneratoreModelli che ritorna la path del documento creato
                    Documento.Dispose()
                End If
                'Call StampaDocumentiRelazioni(idverificaassociata, "relazionestand")
            Case "Sede"
                '** Agg. da simona cordella il 25/11/2010
                '** Richiamo la maschera per la sostituzione della sede di attuazione del progetto
                Response.Redirect("WfrmRicPrgVerifica.aspx?VengoDa=Accertamento&IdEnte=" & hftxtidente.Value & "&IDVerificheAssociate=" & e.Item.Cells(10).Text & "&IdAttivitaEnteSedeAttuazioneOld=" & e.Item.Cells(13).Text & "&IdVerifica=" & e.Item.Cells(9).Text)
        End Select
    End Sub

    Private Sub dgRisultatoRicerca_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgRisultatoRicerca.PageIndexChanged
        dgRisultatoRicerca.SelectedIndex = -1
        dgRisultatoRicerca.EditItemIndex = -1
        dgRisultatoRicerca.CurrentPageIndex = e.NewPageIndex
        CaricaSedi()
    End Sub

    Public Sub cmdStampa_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles cmdStampa.Click

        'Dim idverifica As Int32
        'idverifica = Request.QueryString("IdVerifica")

        'Call StampaDocumentiVerifica(idverifica, "relazionecompleta")
        'Mando in esecuzione la stampa della valutazione di qualità

        Response.Write("<script type=""text/javascript"">")
        Response.Write("window.open(""WfrmReportistica.aspx?sTipoStampa=30&IdVerifica=" & CInt(Request.QueryString("IdVerifica")) & """, ""Report"", ""height=800,width=800, ,dependent=no,scrollbars=no,status=no,resizable=yes"")")
        'Response.Write("window.open(""WfrmReportistica.aspx?sTipoStampa=30&IdVerifica=" & Request.QueryString("IdVerifica")""", ""Report"", ""height=800,width=800, ,dependent=no,scrollbars=no,status=no,resizable=yes"")")
        Response.Write("</script>")


        'Response.Write("<SCRIPT>" & vbCrLf)
        'Response.Write("window.open('WfrmReportistica.aspx?sTipoStampa=30&IdVerifica=" & Request.QueryString("IdVerifica") & ",'report','height=800,width=800,dependent=no,scrollbars=no,status=no,resizable=yes');" & vbCrLf)
        'Response.Write("</SCRIPT>")

        ' myWin = window.open ('WfrmReportistica.aspx?sTipoStampa=30&IdVerifica=' + <%=Request.QueryString("IdVerifica")%>,'Report','height=800,width=800, ,dependent=no,scrollbars=no,status=no,resizable=yes')
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
        Response.Write("window.open('" & Documento.MON_TrasmissioneSanzioneDG(ClsUtility.TrovaIDEnteDaIDVerifica(Request.QueryString("IdVerifica"), Session("conn")), Request.QueryString("IdVerifica"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn")) & "');" & vbCrLf)
        Response.Write("</SCRIPT>")
        'chiamo la proprietà della classe GeneratoreModelli che ritorna la path del documento creato
        Documento.Dispose()
    End Sub

    Private Sub CmdCancellazione_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles CmdCancellazione.Click
        'Dim idverifica As Int16
        'idverifica = Request.QueryString("IdVerifica")
        'Call StampaDocumentiGen(idverifica, "CANCELLAZIONE")

        Dim Documento As New GeneratoreModelli

        Response.Write("<script>" & vbCrLf)
        Response.Write("window.open('" & Documento.MON_CANCELLAZIONE(ClsUtility.TrovaIDEnteDaIDVerifica(Request.QueryString("IdVerifica"), Session("conn")), Request.QueryString("IdVerifica"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn")) & "');" & vbCrLf)
        Response.Write("</script>")
        'chiamo la proprietà della classe GeneratoreModelli che ritorna la path del documento creato
        Documento.Dispose()
    End Sub

    Private Sub CmdInterdizione_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles CmdInterdizione.Click
        'Dim idverifica As Int16
        'idverifica = Request.QueryString("IdVerifica")
        'Call StampaDocumentiGen(idverifica, "INTERDIZIONE")

        Dim Documento As New GeneratoreModelli

        Response.Write("<SCRIPT>" & vbCrLf)
        Response.Write("window.open('" & Documento.MON_INTERDIZIONE(ClsUtility.TrovaIDEnteDaIDVerifica(Request.QueryString("IdVerifica"), Session("conn")), Request.QueryString("IdVerifica"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn")) & "');" & vbCrLf)
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
        Response.Write("window.open('" & Documento.MON_REVOCA(ClsUtility.TrovaIDEnteDaIDVerifica(Request.QueryString("IdVerifica"), Session("conn")), Request.QueryString("IdVerifica"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn")) & "');" & vbCrLf)
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
        Response.Write("window.open('" & Documento.MON_CONTESTAZIONECHIUSURA(ClsUtility.TrovaIDEnteDaIDVerifica(Request.QueryString("IdVerifica"), Session("conn")), Request.QueryString("IdVerifica"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn")) & "');" & vbCrLf)
        Response.Write("</SCRIPT>")
        'chiamo la proprietà della classe GeneratoreModelli che ritorna la path del documento creato
        Documento.Dispose()
    End Sub

    Private Sub PersonalizzaFascicolo_Regionalizzazione(ByVal IdRegCompetenza As Integer, ByVal StatoVerifica As String)
        If IdRegCompetenza <> 22 Then

            cmdSelFascicolo.Visible = False
            cmdSelProtocollo0.Visible = False
            cmdFascCanc.Visible = False

            'CREDENZIALI
            ImgProtolloCredenziali.Visible = False
            ImgApriAllegatiCredenziali.Visible = False

            'PROTOCOLLO INCARICO
            cmdSc1SelProtocollo1.Visible = False
            cmdSc1Allegati1.Visible = False
            'TRASMISSIONE AL DG
            ImgProtocolloTrasDG.Visible = False
            ImgApriAllegatiTrasDG.Visible = False
            'LETTERA CHIUSURA 

            ImgApriAllegatiLettChiusura.Visible = False
            'INVIO LETTERA CONTESTAZIONE AL D.G. 15/10/2010
            ImgProtocolloTrasContestataDG.Visible = False
            ImgApriAllegatiTrasContestataDG.Visible = False
            'INVIO LETTERA CONTESTAZIONE 

            ImgApriAllegatiInvioLettContestazione.Visible = False
            'RISPOSTA LETTERA CONTESTAZIONE ( Controdeduzioni )
            ImgApriAllegatiRispContestazione.Visible = False
            'DATA PROTOCOLLO CHIUSURA LETTERA CONTESTAZIONE
            ImgApriAllegatiProtChiusuraContestazione.Visible = False
            'DATA PROTOCOLLO TRASMISSIONE SANZIONE
            ImgApriAllegatiTrasmissioneSanzione.Visible = False
            'DATA PROTOCOLLO ESECUZIONE SANZIONE
            ImgApriAllegatiEsecuzioneSanzione.Visible = False
            'DATA PROTOCOLLO TRASMISSIONE SERVIZIO(SANZIONE)
            ImgApriAllegatiTrasmServizi.Visible = False

        Else
            'AGG. DA sc 15/10/2010
            If StatoVerifica = "Aperta" Or StatoVerifica = "In Esecuzione" Or StatoVerifica = "Eseguita" Then
                cmdSelFascicolo.Visible = True
                ' cmdSelProtocollo0.Visible = True
                cmdFascCanc.Visible = True
            Else
                cmdSelFascicolo.Visible = False
                'cmdSelProtocollo0.Visible = False
                cmdFascCanc.Visible = False
            End If

            'mod il 20/05/2011 la consultazione dei documenti del fasciolo è sempre visibile in qualunque stato della verifica
            cmdSelProtocollo0.Visible = True

            'CREDENZIALI
            'ImgProtolloCredenziali.Visible = True
            'ImgApriAllegatiCredenziali.Visible = True
            '28-07-2016 ImgProtocollazioneCredenziali.Visible = True
            'IGF --> CmdStampaCredenzialiIncaricoIGF
            'PROTOCOLLO INCARICO
            cmdSc1SelProtocollo1.Visible = True
            cmdSc1Allegati1.Visible = True
            '28-07-2016 cmdNuovoFascioclo.Visible = True
            'IGF --> CmdStampaLettereIncaricoIGF
            'TRASMISSIONE AL DG
            ImgProtocolloTrasDG.Visible = True
            ImgApriAllegatiTrasDG.Visible = True
            ' ImgProtocollazioneTrasDG.Visible = True
            'LETTERA CHIUSURA 
            '28-07-2016 ImgProtocolloLettChiusura.Visible = True
            ImgApriAllegatiLettChiusura.Visible = True
            '28-07-2016 ImgProtocollazioneLettChiusura.Visible = True
            'INVIO LETTERA CONTESTAZIONE 
            '28-07-2016 ImgProtocolloInvioLettContestazione.Visible = True
            ImgApriAllegatiInvioLettContestazione.Visible = True
            '28-07-2016 ImgProtocollazioneInvioLettContestazione.Visible = True
            'RISPOSTA LETTERA CONTESTAZIONE ( Controdeduzioni )
            '28-07-2016 ImgProtocolloRispContestazione.Visible = True
            ImgApriAllegatiRispContestazione.Visible = True
            '28-07-2016 ImgProtocollazioneRispContestazione.Visible = True
            'DATA PROTOCOLLO CHIUSURA LETTERA CONTESTAZIONE
            '28-07-2016 ImgProtocolloProtChiusuraContestazione.Visible = True
            ImgApriAllegatiProtChiusuraContestazione.Visible = True
            '28-07-2016 ImgProtocollazioneProtChiusuraContestazione.Visible = True
            'DATA PROTOCOLLO TRASMISSIONE SANZIONE
            '28-07-2016 ImgProtocolloTrasmissioneSanzione.Visible = True
            ImgApriAllegatiTrasmissioneSanzione.Visible = True
            '28-07-2016 ImgProtocollazioneTrasmissioneSanzione.Visible = True
            'DATA PROTOCOLLO ESECUZIONE SANZIONE
            '28-07-2016 ImgProtocolloEsecuzioneSanzione.Visible = True
            ImgApriAllegatiEsecuzioneSanzione.Visible = True
            '28-07-2016 ImgProtocollazioneEsecuzioneSanzione.Visible = True
            'DATA PROTOCOLLO TRASMISSIONE SERVIZIO(SANZIONE)
            '28-07-2016 ImgProtocolloTrasmServizi.Visible = True
            ImgApriAllegatiTrasmServizi.Visible = True
            '28-07-2016 ImgProtocollazioneTrasmServizi.Visible = True
        End If
    End Sub

    Private Sub cmdSanzione_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles cmdSanzione.Click
        Session("VengoDa") = "VerificaRequisiti"
        Response.Redirect("ver_Sanzione.aspx?NumProtEsecSanzione= " & TxtNumeroProtocolloEsecuzioneSanzione.Text & " &DataProtEsecSanzione= " & txtDataProtocolloEsecuzioneSanzione.Text & " &idprogrammazione=" & Trim(Request.QueryString("idprogrammazione")) & "&VengoDa=" & Session("VengoDa") & "&IdEnte=" & Request.QueryString("Idente") & "&IdVerifica=" & Request.QueryString("IdVerifica") & "")
    End Sub

    Private Sub chkNonEffettuata_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkNonEffettuata.CheckedChanged
        If chkNonEffettuata.Checked = True Then
            CmdIrregolarita.Enabled = True
            CmdTrasmRich.Enabled = True
            CmdConclusione.Enabled = False
            cmdTrasmissione.Enabled = False
            chkRichiamo.Checked = False

            SchedaContestatazione(False, Color.LightGray)
            DataProtTrasDG(False, Color.LightGray)
            chkRichiamo.Enabled = False

            If Session("IdRegCompetenza") <> 22 Then
                DataProtocolloInvioChiusura(False, Color.LightGray)
                DataProtLetteraContestazioneDG(False, Color.LightGray)
                DataProtTrasDG(False, Color.LightGray)
            End If
        Else
            CmdIrregolarita.Enabled = False
            CmdTrasmRich.Enabled = False
            CmdConclusione.Enabled = True
            cmdTrasmissione.Enabled = True

            SchedaContestatazione(True, Color.White)

            ''abilito  e coloro in bianco i campi per la chiusura positiva e contestazione
            DataProtTrasDG(True, Color.White)
            If Session("IdRegCompetenza") <> 22 Then
                DataProtocolloInvioChiusura(True, Color.White)
                DataProtLetteraContestazioneDG(True, Color.White)
                DataProtTrasDG(True, Color.White)
            End If
            chkRichiamo.Enabled = True
        End If
    End Sub

    Private Sub cmdIncludi_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles cmdIncludi.Click
        Dim strProg As String
        Dim StrVerif As String
        strProg = lblProgetto.Text
        StrVerif = lblIspettore.Text
        Response.Redirect("ver_RicercaAssociaVerifiche.aspx?TipoVerifica=" & lblTipologiaVerifica.Text & "&Verificatore=" & StrVerif & "&Progetto=" & strProg & "&IDProgrammazione=" & hftxtidProgrammazione.Value & "&IdVerifica=" & hftxtIdVerifica.Value & "&IdEnteSedeAttuazione=" & hftxtIdEnteSedeAtt.Value & " &StatoVErifica=" & lblStatoVerifica.Text & " &CodiceProgetto=" & hftxtCodProgetto.Value & "&IdVerificatore=" & txtIdVerificatore.Value & "")

    End Sub

    Private Sub cmdSospendi_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles cmdSospendi.Click
        Dim strsql As String
        Dim CmdGenerico As SqlClient.SqlCommand

        strsql = "update tverifiche set idstatoverifica = 12 where idverifica = " & hftxtIdVerifica.Value
        CmdGenerico = ClsServer.EseguiSqlClient(strsql, Session("conn"))
        CaricaMaschera()
    End Sub

    Private Sub cmdRipristina_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles cmdRipristina.Click
        Dim strsql As String
        Dim CmdGenerico As SqlClient.SqlCommand

        strsql = "update tverifiche set idstatoverifica = 5 where idverifica = " & hftxtIdVerifica.Value
        CmdGenerico = ClsServer.EseguiSqlClient(strsql, Session("conn"))
        CaricaMaschera()
    End Sub

    Private Sub cmdChiusaContestata_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles cmdChiusaContestata.Click
        'Aggiunto il 11/10/2007 da Simona Cordella
        'Gestione Chiusura della VErifica Contesta -- idstato 14
        Dim strsql As String
        Dim CmdGenerico As SqlClient.SqlCommand

        If Session("IdRegCompetenza") <> 22 Then
            If (txtDataProtocolloEsecuzioneSanzione.Text = "" Or TxtNumeroProtocolloEsecuzioneSanzione.Text = "") Then
                'controllo se ho applicato almeno una sanzione all'ente ma nn ho indicato la data e numero invio sanzione
                If ControlloSanzioniCompleto(CInt(hftxtIdVerifica.Value)) = True Then
                    'se non ho indicato nessuna sanzione non posso salvare i dati messaggio informativo
                    lblErrore.Text = "E' necessario indicare la Data e Numero Protocollo invio Sanzione."
                    Exit Sub
                End If

            Else
                ''--NEW CODE 20/07/2016

                lblErrore.Text = "Attenzione.Non è possibile chiudere la verifica in CHIUDI CONTESTATA,e' stata indicata la data e/o il Protocollo Esecuzione Sanzione."
                cmdChiusaContestata.Visible = False
                'cmdSanzione.Enabled = True
                lkbSanzione.Enabled = True
                Exit Sub
            End If
        End If

        strsql = "update tverifiche set idstatoverifica = 14 where idverifica = " & hftxtIdVerifica.Value
        CmdGenerico = ClsServer.EseguiSqlClient(strsql, Session("conn"))
        CaricaMaschera()
    End Sub

    Protected Sub cmdChiudi_Click(sender As Object, e As EventArgs) Handles cmdChiudi.Click
        Session("VengoDa") = "VerificaRequisiti"
        Response.Redirect("ver_RicercaVerifichePerRequisiti.aspx?VengoDa= " & Session("VengoDa"))
    End Sub

    Sub Irregolarità()

        If chkRichiamo.Checked = False Then 'CHIUSA POSITIVA
            cmdTrasmissione.Enabled = True
            CmdTrasmRich.Enabled = False
            'controll ose ho valorizzato trasmissiom al dg
            If TxtDataProtTrasDG.Text <> "" Then
                'abilito stampa trasmissione al dg.
                cmdTrasmissione.Enabled = True
                CmdTrasmRich.Enabled = False 'CON RICHIAMO
                CmdLetteraTrasm.Enabled = False
            End If
            'controllo se è valorizzata trasmissione dg contestazione
            If txtDataProtLetteraContestazioneDG.Text <> "" Then
                CmdTrasmRich.Enabled = False
                cmdTrasmissione.Enabled = False
                CmdLetteraTrasm.Enabled = True
            End If
            'abilito protocollo invio lettera positiva e stampa lettera positiva
            DataProtocolloInvioChiusura(True, Color.White)

            CmdConclusione.Enabled = True
            CmdIrregolarita.Enabled = False

            chkNonEffettuata.Checked = False
            chkNonEffettuata.Enabled = False
        Else 'CHIUSA POSITIVA CON RICHIAMO
            cmdTrasmissione.Enabled = False
            CmdTrasmRich.Enabled = True
            'controll ose ho valorizzato trasmissiom  al dg
            If TxtDataProtTrasDG.Text <> "" Then
                'abilito stampa trasmissione al dg.
                cmdTrasmissione.Enabled = False
                CmdTrasmRich.Enabled = True 'CON RICHIAMO
                CmdLetteraTrasm.Enabled = False
            End If
            'controllo se è valorizzata trasmissione dg contestazione
            If txtDataProtLetteraContestazioneDG.Text <> "" Then
                CmdTrasmRich.Enabled = False
                cmdTrasmissione.Enabled = False
                CmdLetteraTrasm.Enabled = True
            End If

            DataProtocolloInvioChiusura(True, Color.White)
            CmdConclusione.Enabled = False
            CmdIrregolarita.Enabled = True

            chkNonEffettuata.Checked = False
            chkNonEffettuata.Enabled = False

        End If
    End Sub

    'Sub ReadOnlyProcolliSANZIONE(ByVal blnValore As Boolean, ByVal Visibility As String)
    '    'Creato da simona cordella 22/02/2011
    '    'rendo editabile o no i protocolli e le date relative alla sanzione
    '    'se sono Regione posso scrivere Numero e Data
    '    'se sono UNSC richiamo numero e protOcollo da SIGED
    '    ''txtDataProtocolloTrasmissioneSanzione.ReadOnly = blnValore
    '    ''TxtNumeroProtocolloTrasmissioneSanzione.ReadOnly = blnValore
    '    ''txtDataProtocolloEsecuzioneSanzione.ReadOnly = blnValore
    '    ''TxtNumeroProtocolloEsecuzioneSanzione.ReadOnly = blnValore
    '    ''txtDataProtocolloRispostaContestazione.ReadOnly = blnValore
    '    ''TxtNumProtocolloRispostaContestazione.ReadOnly = blnValore
    '    '** agg da sc il 15/10/2010 gestione DATA protocollo trasmissione servizi
    '    'txtDataProtocolloTrasmServizi.ReadOnly = blnValore
    '    'txtNProtocolloTrasmServizi.ReadOnly = blnValore
    '    'imgDataProtocolloRispostaContestazione.Attributes.Add("style", Visibility)
    '    'imgDataProtocolloTrasmissioneSanzione.Attributes.Add("style", Visibility)
    '    'imgDataProtocolloEsecuzioneSanzione.Attributes.Add("style", Visibility)
    '    'imgDataProtocolloTrasmServizi.Attributes.Add("style", Visibility)
    'End Sub
    'Sub ReadOnlyProcolliSANZIONERPA()
    '    'Creato da simona cordella 22/02/2011
    '    'rendo editabile o no i protocolli e le date relative alla sanzione
    '    'se sono Regione posso scrivere Numero e Data
    '    'se sono UNSC richiamo numero e protOcollo da SIGED
    '    If txtDataProtocolloRispostaContestazione.Text <> "" Then 'controdeduzioni della contestazione
    '        'txtDataProtocolloRispostaContestazione.ReadOnly = True
    '        ' txtDataProtocolloRispostaContestazione.Enabled = False


    '        '   TxtNumProtocolloRispostaContestazione.ReadOnly = True
    '        'imgDataProtocolloRispostaContestazione.Attributes.Add("style", "visibility: hidden")
    '    Else
    '        ' txtDataProtocolloRispostaContestazione.ReadOnly = False
    '        '   txtDataProtocolloRispostaContestazione.Enabled = True

    '        '  TxtNumProtocolloRispostaContestazione.ReadOnly = False


    '        'imgDataProtocolloRispostaContestazione.Attributes.Add("style", "visibility: visible")
    '    End If
    '    '** Trasmissione Sanzione al D.G.
    '    If txtDataProtocolloTrasmissioneSanzione.Text <> "" Then
    '        'txtDataProtocolloTrasmissioneSanzione.ReadOnly = True
    '        txtDataProtocolloTrasmissioneSanzione.Enabled = False

    '        ' TxtNumeroProtocolloTrasmissioneSanzione.ReadOnly = True
    '        'imgDataProtocolloTrasmissioneSanzione.Attributes.Add("style", "visibility: hidden")
    '        TxtNumeroProtocolloEsecuzioneSanzione.BackColor = Color.LightGray
    '        txtDataProtocolloEsecuzioneSanzione.BackColor = Color.LightGray
    '    Else
    '        '  txtDataProtocolloTrasmissioneSanzione.ReadOnly = False
    '        txtDataProtocolloTrasmissioneSanzione.Enabled = True

    '        '  TxtNumeroProtocolloTrasmissioneSanzione.ReadOnly = False
    '        'imgDataProtocolloTrasmissioneSanzione.Attributes.Add("style", "visibility: visible")
    '        TxtNumeroProtocolloEsecuzioneSanzione.BackColor = Color.White
    '        txtDataProtocolloEsecuzioneSanzione.BackColor = Color.White
    '    End If
    '    '**
    '    '** Protocollo Invio Sanzione 
    '    If txtDataProtocolloEsecuzioneSanzione.Text <> "" Then
    '        '  txtDataProtocolloEsecuzioneSanzione.ReadOnly = True
    '        txtDataProtocolloEsecuzioneSanzione.Enabled = False

    '        ' TxtNumeroProtocolloEsecuzioneSanzione.ReadOnly = True
    '        'imgDataProtocolloEsecuzioneSanzione.Attributes.Add("style", "visibility: hidden")
    '        TxtNumeroProtocolloEsecuzioneSanzione.BackColor = Color.LightGray
    '        txtDataProtocolloEsecuzioneSanzione.BackColor = Color.LightGray
    '    Else
    '        ' txtDataProtocolloEsecuzioneSanzione.ReadOnly = False
    '        txtDataProtocolloEsecuzioneSanzione.Enabled = True

    '        ' TxtNumeroProtocolloEsecuzioneSanzione.ReadOnly = False
    '        'imgDataProtocolloEsecuzioneSanzione.Attributes.Add("style", "visibility: visible")
    '        TxtNumeroProtocolloEsecuzioneSanzione.BackColor = Color.White
    '        txtDataProtocolloEsecuzioneSanzione.BackColor = Color.White
    '    End If
    '    '**
    '    '** Protocollo Trasmissione Servizi
    '    If txtDataProtocolloTrasmServizi.Text <> "" Then
    '        '** agg da sc il 15/10/2010 gestione DATA protocollo trasmissione servizi
    '        '  txtDataProtocolloTrasmServizi.ReadOnly = True
    '        'txtDataProtocolloTrasmServizi.Enabled = 'False

    '        '  txtNProtocolloTrasmServizi.ReadOnly = True
    '        ' imgDataProtocolloTrasmServizi.Attributes.Add("style", "visibility: hidden")
    '        txtNProtocolloTrasmServizi.BackColor = Color.LightGray
    '        txtDataProtocolloTrasmServizi.BackColor = Color.LightGray
    '    Else
    '        '  txtDataProtocolloTrasmServizi.ReadOnly = False
    '        ' txtDataProtocolloTrasmServizi.Enabled = True

    '        ' txtNProtocolloTrasmServizi.ReadOnly = False
    '        ' imgDataProtocolloTrasmServizi.Attributes.Add("style", "visibility: visible")
    '        txtNProtocolloTrasmServizi.BackColor = Color.White
    '        txtDataProtocolloTrasmServizi.BackColor = Color.White
    '    End If
    '    '**
    '    '** Data Esecuzione Sanzione agg il 19/02/2013 da simona cordella
    '    If txtDataEsecuzioneSanzione.Text <> "" Then
    '        '  txtDataEsecuzioneSanzione.ReadOnly = True
    '        txtDataEsecuzioneSanzione.Enabled = False

    '        txtDataEsecuzioneSanzione.BackColor = Color.LightGray
    '        '  imgDataEsecuzioneSanzione.Attributes.Add("style", "visibility: hidden")          
    '    Else
    '        '  txtDataEsecuzioneSanzione.ReadOnly = False
    '        txtDataEsecuzioneSanzione.Enabled = True

    '        txtDataEsecuzioneSanzione.BackColor = Color.White
    '        '  imgDataEsecuzioneSanzione.Attributes.Add("style", "visibility: visible")
    '    End If
    'End Sub
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
    Private Function TipologiaProgetti(ByVal IdProgetto As Integer)
        Dim strsql As String
        Dim intTipoPrg As Integer

        '0=visualizza punteggio vecchi prg
        '1=visualizza punteggio nuovi prg
        '2=non visualizza punteggio nuovi prg

        strsql = "SELECT bando.AnnoBreve, RegioniCompetenze.AutonomiaProgetti " & _
                 " FROM attività INNER JOIN " & _
                 " BandiAttività ON attività.IDBandoAttività = BandiAttività.IdBandoAttività INNER JOIN " & _
                 " bando ON BandiAttività.IdBando = bando.IDBando INNER JOIN " & _
                 " RegioniCompetenze ON attività.IDRegioneCompetenza = RegioniCompetenze.IdRegioneCompetenza " & _
                 " WHERE(attività.IDAttività = " & IdProgetto & ")"

        If Not dtrLeggiDati Is Nothing Then
            dtrLeggiDati.Close()
            dtrLeggiDati = Nothing
        End If
        dtrLeggiDati = ClsServer.CreaDatareader(strsql, Session("conn"))

        If dtrLeggiDati.HasRows Then
            dtrLeggiDati.Read()

            If CInt(dtrLeggiDati.Item("AnnoBreve")) < 7 Then
                intTipoPrg = 0
            Else
                If CInt(dtrLeggiDati.Item("AnnoBreve")) < 10 Then
                    If dtrLeggiDati.Item("AutonomiaProgetti") = True Then
                        intTipoPrg = 1
                    Else
                        intTipoPrg = 2
                    End If
                Else
                    If dtrLeggiDati.Item("AutonomiaProgetti") = True Then
                        intTipoPrg = 3
                    Else
                        intTipoPrg = 4
                    End If
                End If
            End If
        End If
        If Not dtrLeggiDati Is Nothing Then
            dtrLeggiDati.Close()
            dtrLeggiDati = Nothing
        End If

        'ControllaNoteAbilitate()

        Return intTipoPrg


    End Function
    Private Sub cmdStampaVQ_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles cmdStampaVQ.Click

        sTipoPRG = TipologiaProgetti(Session("IdAttività"))
        Dim intTipoStampa As Integer
        Dim strSql As String
        strSql = "Select Max(IdStorico) As MaxIdS From storicoprogetti Where IdProgetto=" & Session("IdAttività")
        If Not dtrLeggiDati Is Nothing Then
            dtrLeggiDati.Close()
            dtrLeggiDati = Nothing
        End If
        dtrLeggiDati = ClsServer.CreaDatareader(strSql, Session("conn"))
        dtrLeggiDati.Read()

        If sTipoPRG = 0 Then
            intTipoStampa = 17
        ElseIf sTipoPRG = 1 Then
            intTipoStampa = 23
        ElseIf sTipoPRG = 2 Then
            intTipoStampa = 24
        ElseIf sTipoPRG = 3 Then
            intTipoStampa = 33
        ElseIf sTipoPRG = 4 Then
            intTipoStampa = 34
        End If

        'Mando in esecuzione la stampa della valutazione di qualità
        Response.Write("<SCRIPT>" & vbCrLf)
        Response.Write("window.open('WfrmReportistica.aspx?sTipoStampa=" & intTipoStampa & "&IdAttivita=" & Session("IdAttività") & "&IdStorico=" & dtrLeggiDati("MaxIdS") & "','report','height=800,width=800,dependent=no,scrollbars=no,status=no,resizable=yes');" & vbCrLf)
        Response.Write("</SCRIPT>")
        dtrLeggiDati.Close()
        dtrLeggiDati = Nothing
    End Sub

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


    Private Function VerificaDate() As Boolean



        VerificaDate = False
        Dim data As Date
        Dim data1 As Date
        If txtDataProtCredenziali.Text <> "" Then
            If Not Date.TryParse(txtDataProtCredenziali.Text, data) Then
                lblErrore.Text = "  Il formato della Data Prot.Credenziali deve essere GG/MM/AAAA. "
                Exit Function
            End If
        End If

        If txtDataInizioVerifica.Text <> "" Then
            If Not Date.TryParse(txtDataInizioVerifica.Text, data) Then
                lblErrore.Text = "  Il formato della Data Inizio Verifica deve essere GG/MM/AAAA. "
                Exit Function
            End If
        End If

        If txtDataFineVerifica.Text <> "" Then
            If Not Date.TryParse(txtDataFineVerifica.Text, data) Then
                lblErrore.Text = "  Il formato della Data Fine Verifica deve essere GG/MM/AAAA. "
                Exit Function
            End If
        End If

        If txtDataProtocolloIncarico.Text <> "" Then
            If Not Date.TryParse(txtDataProtocolloIncarico.Text, data) Then
                lblErrore.Text = "  Il formato della Data Prot. Incarico deve essere GG/MM/AAAA. "
                Exit Function
            End If
        End If

        If txtDataProtocolloRelazione.Text <> "" Then
            If Not Date.TryParse(txtDataProtocolloRelazione.Text, data) Then
                lblErrore.Text = "  Il formato della Data Relazione deve essere GG/MM/AAAA. "
                Exit Function
            End If
        End If

        If TxtDataProtTrasDG.Text <> "" Then
            If Not Date.TryParse(TxtDataProtTrasDG.Text, data) Then
                lblErrore.Text = "  Il formato della Data Prot. Trasmissione al D.G. deve essere GG/MM/AAAA. "
                Exit Function
            End If
        End If

        If txtDataProtocolloInvioChiusura.Text <> "" Then
            If Not Date.TryParse(txtDataProtocolloInvioChiusura.Text, data) Then
                lblErrore.Text = "  Il formato della Data Prot. Invio Lettera di Chiusura deve essere GG/MM/AAAA. "
                Exit Function
            End If
        End If
        If txtDataProtLetteraContestazioneDG.Text <> "" Then
            If Not Date.TryParse(txtDataProtLetteraContestazioneDG.Text, data) Then
                lblErrore.Text = "  Il formato della Data Prot. Trasmissiome Contest. al D.G. deve essere GG/MM/AAAA. "
                Exit Function
            End If
        End If
        If txtDataProtocolloInvioLetteraContestazione.Text <> "" Then
            If Not Date.TryParse(txtDataProtocolloInvioLetteraContestazione.Text, data) Then
                lblErrore.Text = "  Il formato della Data Prot. Invio Contestazione deve essere GG/MM/AAAA. "
                Exit Function
            End If
        End If



        If txtDataRicezioneLetteraContestazione.Text <> "" Then
            If Not Date.TryParse(txtDataRicezioneLetteraContestazione.Text, data) Then
                lblErrore.Text = "  Il formato della Data Ricezione Lettera Contestazione deve essere GG/MM/AAAA. "
                Exit Function
            End If
        End If
        If txtDataRispostaLetteraContestazione.Text <> "" Then
            If Not Date.TryParse(txtDataRispostaLetteraContestazione.Text, data) Then
                lblErrore.Text = "  Il formato della Data Lettera Controdeduzioni deve essere GG/MM/AAAA. "
                Exit Function
            End If
        End If
        If txtDataProtocolloRispostaContestazione.Text <> "" Then
            If Not Date.TryParse(txtDataProtocolloRispostaContestazione.Text, data) Then
                lblErrore.Text = "  Il formato della Data Prot. Lettera Controdeduzioni deve essere GG/MM/AAAA. "
                Exit Function
            End If
        End If

        If TxtDataProtChiusuraContestazione.Text <> "" Then
            If Not Date.TryParse(TxtDataProtChiusuraContestazione.Text, data) Then
                lblErrore.Text = "  Il formato della Data Prot. Chiusura Contestazione deve essere GG/MM/AAAA. "
                Exit Function
            End If
        End If
        If txtDataProtocolloTrasmissioneSanzione.Text <> "" Then
            If Not Date.TryParse(txtDataProtocolloTrasmissioneSanzione.Text, data) Then
                lblErrore.Text = "  Il formato della Data Protocollo Trasmissione Sanzione al D.G. deve essere GG/MM/AAAA. "
                Exit Function
            End If
        End If
        If txtDataProtocolloEsecuzioneSanzione.Text <> "" Then
            If Not Date.TryParse(txtDataProtocolloEsecuzioneSanzione.Text, data) Then
                lblErrore.Text = "  Il formato della Data Protocollo Invio Sanzione deve essere GG/MM/AAAA. "
                Exit Function
            End If
        End If


        If txtDataProtocolloTrasmServizi.Text <> "" Then
            If Not Date.TryParse(txtDataProtocolloTrasmServizi.Text, data) Then
                lblErrore.Text = "  Il formato della Data Protocollo Trasmissione Servizi deve essere GG/MM/AAAA. "
                Exit Function
            End If
        End If
        If txtDataEsecuzioneSanzione.Text <> "" Then
            If Not Date.TryParse(txtDataEsecuzioneSanzione.Text, data) Then
                lblErrore.Text = "  Il formato della Data Esecuzione Sanzione deve essere GG/MM/AAAA. "
                Exit Function
            End If
        End If
        If txtDataProtCredenziali.Text <> "" Then
            If Not Date.TryParse(txtDataProtCredenziali.Text, data) Then
                lblErrore.Text = "  Il formato della Data Prot.Credenziali deve essere GG/MM/AAAA. "
                Exit Function
            End If
        End If
        VerificaDate = True

    End Function

    Private Sub PulisciCampiFascicolo()

        TxtCodiceFascicolo.Text = ""
        hfTxtCodiceFasc.Value = ""
        txtDescFasc.Text = ""
        txtDataProtocolloIncarico.Text = ""
        TxtNumProtIncarico.Text = ""
        txtNumProtCredenziali.Text = ""
        txtDataProtCredenziali.Text = ""
        TxtNumProtTrasDG.Text = ""
        TxtDataProtTrasDG.Text = ""
        TxtNumProtocolloInvioLetteraChiusura.Text = ""
        txtDataProtocolloInvioChiusura.Text = ""
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


    Protected Sub cmdFascCanc_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs) Handles cmdFascCanc.Click

        PulisciCampiFascicolo()
    End Sub


    Protected Sub cmdSalva_Click(sender As Object, e As EventArgs) Handles cmdSalva.Click
        '--- NEW CODE 20/07/2016
        Dim idModelloSanz As Integer
        If VerificaDate() = False Then Exit Sub

        If txtDataInizioVerifica.Text <> "" Then
            If txtDataFineVerifica.Text = "" Then
                lblErrore.Text = "Inserire la Data Fine Verifica."
                txtDataFineVerifica.Focus()
                Exit Sub
            End If
        End If

        If txtDataFineVerifica.Text <> "" Then
            If txtDataInizioVerifica.Text = "" Then
                lblErrore.Text = "Inserire la Data Inizio Verifica."
                txtDataInizioVerifica.Focus()
                Exit Sub
            End If
        End If


        If D1LTD2(txtDataFineVerifica.Text, txtDataInizioVerifica.Text) Then
            lblErrore.Text = "La data Fine verifica deve essere maggiore o uguale alla data inizio verifica."
            txtDataFineVerifica.Focus()
            Exit Sub
        End If



        If D1LTD2(txtDataInizioVerifica.Text, txtDataProtocolloIncarico.Text) Then
            lblErrore.Text = "La data Protocollo Incarico deve essere minore o uguale alla Data Inizio Verifica."
            txtDataProtocolloIncarico.Focus()
            Exit Sub
        End If


        If D1LTD2(txtDataInizioVerifica.Text, txtDataProtCredenziali.Text) Then
            lblErrore.Text = "La data Protocollo credenziali deve essere minore o uguale alla Data Inizio Verifica."
            txtDataProtCredenziali.Focus()
            Exit Sub
        End If


        If D1LTD2(txtDataInizioVerifica.Text, txtDataProtCredenziali.Text) Then
            lblErrore.Text = "La data Protocollo credenziali deve essere minore o uguale alla Data Inizio Verifica."
            txtDataProtCredenziali.Focus()
            Exit Sub
        End If

        If D1LTD2(txtDataProtocolloRelazione.Text, txtDataFineVerifica.Text) Then
            lblErrore.Text = "La data Relazione deve essere maggiore o uguale alla data fine verifica."
            txtDataProtocolloRelazione.Focus()
            Exit Sub
        End If

        'If D1LTD2(txtDataProtocolloRelazione.Text, txtDataProtLetteraContestazioneDG.Text) Then
        If D1LTD2(txtDataProtLetteraContestazioneDG.Text, txtDataProtocolloRelazione.Text) Then
            lblErrore.Text = "La Data Protocollo Lettere Contestazione al D.G. deve essere maggiore o uguale alla Data Protocollo Relazione."
            txtDataProtLetteraContestazioneDG.Focus()
            Exit Sub
        End If


        If TxtDataProtTrasDG.Text <> "" Then
            'If D1LTD2(txtDataProtocolloRelazione.Text, TxtDataProtTrasDG.Text) Then
            If D1LTD2(TxtDataProtTrasDG.Text, txtDataProtocolloRelazione.Text) Then
                lblErrore.Text = "La Data Protocollo Trasmissione al D.G. deve essere maggiore o uguale alla Data Protocollo Relazione."
                TxtDataProtTrasDG.Focus()
                Exit Sub
            End If
        End If

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


        'If D1LTD2(txtDataRicezioneLetteraContestazione.Text, txtDataRispostaLetteraContestazione.Text) Then
        '    lblErrore.Text = "La Data Ricezione Lettera Contestazione deve essere maggiore o uguale alla Data Lettera Controdeduzioni."
        '    txtDataRicezioneLetteraContestazione.Focus()
        '    Exit Sub
        'End If

        'If txtDataProtocolloRispostaContestazione.Text <> "" Then
        '    If D1LTD2(txtDataProtocolloRispostaContestazione.Text, txtDataRispostaLetteraContestazione.Text) Then
        '        lblErrore.Text = "La Data Protocollo Lettera Controdeduzioni deve essere maggiore o uguale alla Data Lettera Controdeduzioni."
        '        txtDataProtocolloRispostaContestazione.Focus()
        '        Exit Sub
        '    End If
        'End If



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


        If txtDataProtocolloInvioChiusura.Text <> "" Or TxtNumProtocolloInvioLetteraChiusura.Text <> "" Then
            If txtDataProtocolloInvioLetteraContestazione.Text <> "" Then
                lblErrore.Text = "Non è possibile indicare la Data Protocollo Invio Lettera Contestazione."
                txtDataProtocolloInvioLetteraContestazione.Focus()
                Exit Sub
            End If

            If TxtNumeroProtocolloInvioLettContestazione.Text <> "" Then
                lblErrore.Text = "Non è possibile indicare Numero Protocollo Invio Lettera Contestazione."
                TxtNumeroProtocolloInvioLettContestazione.Focus()
                Exit Sub
            End If

        End If
        '---FINE NEW CODE

        If txtDataProtocolloRelazione.Text <> "" Then
            If chkRelazione.Checked = False Then
                lblErrore.Text = "E' necessario selezionare la Relazione."
                Exit Sub
            End If
        Else
            If chkRelazione.Checked = True Then
                lblErrore.Text = "E' necessario indicare la Data Relazione."
                Exit Sub
            End If
        End If
        If chkNonEffettuata.Checked = True Then
            If txtDataProtocolloInvioChiusura.Text <> "" And TxtNumProtocolloInvioLetteraChiusura.Text <> "" Then
                lblErrore.Text = "Non è possibile chiudere la verifica in Chiusa non Effettuata."
                Exit Sub
            End If
        End If
        If chkRichiamo.Checked = True And txtDataProtocolloInvioChiusura.Text = "" Then
            lblErrore.Text = "E' necessario indicara la Data Protocollo Invio Lettera Chiusura."
            Exit Sub
        End If
        'se valorizzo la data e numero protocollo sanzione controllo se ho indicato la sanzione
        If (txtDataProtocolloTrasmissioneSanzione.Text <> "" Or TxtNumeroProtocolloTrasmissioneSanzione.Text <> "") Then
            'controllo se la data e numero protocollo srasmiossione al DG è prensete del DB
            If (txtDataProtocolloEsecuzioneSanzione.Text <> "" Or TxtNumeroProtocolloEsecuzioneSanzione.Text <> "") Then
                'controllo se ho applicato almeno una sanzione all'ente
                If ControlloSanzioniCompleto(CInt(hftxtIdVerifica.Value)) = False Then
                    'se non ho indicato nessuna sanzione non posso salvare i dati messaggio informativo
                    lblErrore.Text = "E' necessario indicare almeno una tipologia di sanzione per rendere la verifica sanzionata."
                    'cmdSanzione.Enabled = True
                    lkbSanzione.Enabled = True
                    cmdChiusaContestata.Visible = False
                    Exit Sub
                End If
            Else
                'controllo se ho applicato almeno una sanzione all'ente ma nn ho indicato la data e numero invio sanzione
                If ControlloSanzioniCompleto(CInt(hftxtIdVerifica.Value)) = True Then
                    'se non ho indicato nessuna sanzione non posso salvare i dati messaggio informativo
                    lblErrore.Text = "E' necessario indicare la Data e Numero Protocollo invio Sanzione."
                    Exit Sub
                End If
            End If
        End If
        If Session("IdRegCompetenza") = 22 Then
            If Session("Sistema") = "Helios" Then
                idModelloSanz = 94
            Else
                idModelloSanz = 237
            End If
            If lblStatoVerifica.Text = "Sanzionata" Then
                If ControlloSanzioneServizi(idModelloSanz, CInt(hftxtIdVerifica.Value)) = False Then
                    lblErrore.Text = "E' necessario indicare il Servizio."
                    Exit Sub
                End If
            End If
        End If
        AggiornaTVerifiche()
        CaricaMaschera()

    End Sub

    Protected Sub cmdAnnullata_Click(sender As Object, e As EventArgs) Handles cmdAnnullata.Click
        Dim strsql As String
        Dim CmdGenerico As SqlClient.SqlCommand

        strsql = "update tverifiche set idstatoverifica = 16 where idverifica = " & hftxtIdVerifica.Value
        CmdGenerico = ClsServer.EseguiSqlClient(strsql, Session("conn"))
        CaricaMaschera()
    End Sub

    Protected Sub imgElencoDocumentiProg_Click(sender As Object, e As EventArgs) Handles imgElencoDocumentiProg.Click

        Response.Redirect("wfrmDocumentiProgetto.aspx?Modifica=" & Request.QueryString("Modifica") & "&Nazionale=" & Request.QueryString("Nazionale") & "&IDVerifica=" & hftxtIdVerifica.Value & "&IdAttivita=" & Session("IdAttività") & "&VengoDa=" & Costanti.VENGO_DA_VERIFICA)
    End Sub

    Protected Sub chkRelazione_CheckedChanged(sender As Object, e As EventArgs) Handles chkRelazione.CheckedChanged
        'If chkRelazione.Checked = True Then
        '    DataProtocolloRelazione(True, Color.White)
        'Else
        '    DataProtocolloRelazione(False, Color.LightGray)
        'End If
    End Sub

    Protected Sub lkbSanzione_Click(sender As Object, e As EventArgs) Handles lkbSanzione.Click
        lblErrore.Text = ""
        lblErrore.Visible = False
        If TxtNumeroProtocolloEsecuzioneSanzione.Text = "" And txtDataProtocolloEsecuzioneSanzione.Text = "" Then
            lblErrore.Text = "Attenzione. E' necessario indicare la Data e Numero Protocollo Sanzione  per applicare la Tipologia di Sanzione."
            lblErrore.Visible = True
        Else

            Session("VengoDa") = "VerificaRequisiti"
            Response.Redirect("ver_Sanzione.aspx?NumProtEsecSanzione= " & TxtNumeroProtocolloEsecuzioneSanzione.Text & " &DataProtEsecSanzione= " & txtDataProtocolloEsecuzioneSanzione.Text & " &idprogrammazione=" & Trim(Request.QueryString("idprogrammazione")) & "&VengoDa=" & Session("VengoDa") & "&IdEnte=" & Request.QueryString("Idente") & "&IdVerifica=" & Request.QueryString("IdVerifica") & "")
        End If
        '   Response.Redirect("ver_Sanzione.aspx?Segnalata=SegnalataUNSC&NumProtEsecSanzione= " & TxtNumeroProtocolloEsecuzioneSanzione.Text & " &DataProtEsecSanzione= " & txtDataProtocolloEsecuzioneSanzione.Text & " &idprogrammazione=" & Trim(Request.QueryString("idprogrammazione")) & "&VengoDa=" & Session("VengoDa") & "&IdEnte=" & Request.QueryString("Idente") & "&IdVerifica=" & Request.QueryString("IdVerifica") & "")
    End Sub

End Class