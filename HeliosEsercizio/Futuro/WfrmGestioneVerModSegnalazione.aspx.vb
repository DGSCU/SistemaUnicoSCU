Imports System.Data.SqlClient
Imports System.IO
Public Class WfrmGestioneVerModSegnalazione
    Inherits System.Web.UI.Page
    Public dtrLeggiDati As SqlDataReader
    'Protected WithEvents cmdAssegna As System.Web.UI.WebControls.ImageButton
    'Protected WithEvents cmdApprovazione As System.Web.UI.WebControls.ImageButton
    'Protected WithEvents LblAssegna As System.Web.UI.WebControls.Label
    'Protected WithEvents lblaccredita As System.Web.UI.WebControls.Label
    'Protected WithEvents Label1 As System.Web.UI.WebControls.Label
    'Protected WithEvents Lblverificatore As System.Web.UI.WebControls.Label
    'Protected WithEvents lblDataInizioASS As System.Web.UI.WebControls.Label
    'Protected WithEvents lblDataFineASS As System.Web.UI.WebControls.Label
    'Protected WithEvents LblIspettorenonIGF As System.Web.UI.WebControls.Label
    'Protected WithEvents cmdSelFascicolo As System.Web.UI.WebControls.Image
    'Protected WithEvents cmdAllegati As System.Web.UI.WebControls.Image
    'Protected WithEvents cmdSelProtocollo As System.Web.UI.WebControls.Image
    'Protected WithEvents cmdAllegati1 As System.Web.UI.WebControls.Image
    'Protected WithEvents cmdSelProtocollo1 As System.Web.UI.WebControls.Image
    'Protected WithEvents cmdAllegati2 As System.Web.UI.WebControls.Image
    'Protected WithEvents cmdSelProtocollo2 As System.Web.UI.WebControls.Image
    'Protected WithEvents cmdSelProtocollo0 As System.Web.UI.WebControls.Image
    'Protected WithEvents TxtCodiceFasc As System.Web.UI.WebControls.TextBox
    'Protected WithEvents cmdFascCanc As System.Web.UI.WebControls.Image
    'Protected WithEvents txtDescFasc As System.Web.UI.WebControls.TextBox
    'Protected WithEvents ddlCompetenza As System.Web.UI.WebControls.DropDownList
    'Protected WithEvents LblNumFascicolo As System.Web.UI.WebControls.Label
    'Protected WithEvents ImgDataProtocollo As System.Web.UI.HtmlControls.HtmlImage
    'Protected WithEvents ImgDataSegnalazione As System.Web.UI.HtmlControls.HtmlImage
    'Protected WithEvents LblNumProtocollo As System.Web.UI.WebControls.Label
    'Protected WithEvents LblDataProtocollo As System.Web.UI.WebControls.Label
    'Protected WithEvents LblDescFasc As System.Web.UI.WebControls.Label
    'Protected WithEvents lblMsgSede As System.Web.UI.WebControls.Label
    'Protected WithEvents ImgLetteraDataProt As System.Web.UI.HtmlControls.HtmlImage
    'Protected WithEvents ImgRispostaDataProt As System.Web.UI.HtmlControls.HtmlImage
    Public Verifica As New clsVerificaSegnalazione
#Region "Proprietà"

    Public Property idSegnalazione() As Integer
        Get
            Return Me.ViewState("idsegnalazione")
        End Get
        Set(ByVal Value As Integer)
            Me.ViewState("idsegnalazione") = Value
        End Set
    End Property

    Public Property idVerifica() As Integer
        Get
            Return Me.ViewState("idverifica")
        End Get
        Set(ByVal Value As Integer)
            Me.ViewState("idverifica") = Value
        End Set
    End Property
    Public Property IDAESA() As Integer
        Get
            Return Me.ViewState("IDAESA")
        End Get
        Set(ByVal Value As Integer)
            Me.ViewState("IDAESA") = Value
        End Set
    End Property
    Public Property IDVerificheAssociate() As Integer
        Get
            Return Me.ViewState("IdVA")
        End Get
        Set(ByVal Value As Integer)
            Me.ViewState("IdVA") = Value
        End Set
    End Property

    Public Property IdRegCompetenza() As Integer
        Get
            Return Me.ViewState("IdRegCompetenza")
        End Get
        Set(ByVal Value As Integer)
            Me.ViewState("IdRegCompetenza") = Value
        End Set
    End Property

#End Region
#Region " Codice generato da Progettazione Web Form "

    'Chiamata richiesta da Progettazione Web Form.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    'Protected WithEvents lblmessaggiosopra As System.Web.UI.WebControls.Label
    'Protected WithEvents Imgerrore As System.Web.UI.WebControls.Image
    'Protected WithEvents txtNumeroFascicolo As System.Web.UI.WebControls.TextBox
    'Protected WithEvents txtNumeroProtocollo As System.Web.UI.WebControls.TextBox
    'Protected WithEvents txtDataProtocollo As System.Web.UI.WebControls.TextBox
    'Protected WithEvents ddEsitoSegnalazione As System.Web.UI.WebControls.DropDownList
    'Protected WithEvents ddFonte As System.Web.UI.WebControls.DropDownList
    'Protected WithEvents txtDataSegnalazione As System.Web.UI.WebControls.TextBox
    'Protected WithEvents txtOggetto As System.Web.UI.WebControls.TextBox
    'Protected WithEvents txtLetteraNProt As System.Web.UI.WebControls.TextBox
    'Protected WithEvents txtLetteraDataProt As System.Web.UI.WebControls.TextBox
    'Protected WithEvents CmdLetteraInterlocutoria As System.Web.UI.WebControls.ImageButton
    'Protected WithEvents txtRispostaNProt As System.Web.UI.WebControls.TextBox
    'Protected WithEvents txtRispostaDataProt As System.Web.UI.WebControls.TextBox
    'Protected WithEvents txtNote As System.Web.UI.WebControls.TextBox
    'Protected WithEvents lBttRicercaAttivitaEnteSedeAttuazione As System.Web.UI.WebControls.LinkButton
    'Protected WithEvents cmdConferma As System.Web.UI.WebControls.ImageButton
    'Protected WithEvents imgChiudi As System.Web.UI.WebControls.ImageButton
    'Protected WithEvents dgRisultatoRicerca As System.Web.UI.WebControls.DataGrid
    'Protected WithEvents frmMain As System.Web.UI.HtmlControls.HtmlForm

    'NOTA: la seguente dichiarazione è richiesta da Progettazione Web Form.
    'Non spostarla o rimuoverla.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: questa chiamata al metodo è richiesta da Progettazione Web Form.
        'Non modificarla nell'editor del codice.
        InitializeComponent()
    End Sub

#End Region
    Private Sub CaricaVerificatoreDaForm()
        Me.Verifica = Me.ViewState("Verifica")
        Me.Verifica.CodiceFascicolo = Me.txtNumeroFascicolo.Text
        Me.Verifica.CodiceFascicoloInterno = Me.TxtCodiceFasc.Value
        Me.Verifica.DescFasicolo = Me.txtDescFasc.Text
        Me.Verifica.Oggetto = Me.txtOggetto.Text
        Me.Verifica.NProtSegnalazione = Me.txtNumeroProtocollo.Text
        Me.Verifica.CodiceVerifica = Me.ddEsitoSegnalazione.SelectedValue
        Me.Verifica.DataProtInvioLetteraInterlocutoria = Me.txtLetteraDataProt.Text
        Me.Verifica.DataProtRispostaLetteraInterlocutoria = Me.txtRispostaDataProt.Text
        Me.Verifica.DataProtSegnalazione = Me.txtDataProtocollo.Text
        Me.Verifica.DataRicezioneSegnalazione = Me.txtDataSegnalazione.Text
        If Me.ddFonte.SelectedValue <> "" Then Me.Verifica.Fonte = Me.ddFonte.SelectedValue
        Me.Verifica.Note = Me.txtNote.Text
        Me.Verifica.NProtRispostaLetteraInterlocutoria = Me.txtRispostaNProt.Text
        Me.Verifica.NProtInvioLetteraInterlocutoria = Me.txtLetteraNProt.Text
        Me.Verifica.IdSegnalazione = Me.idSegnalazione
        Me.Verifica.IdVerifica = Me.idVerifica
        Me.Verifica.IdAttivitaEnteSedeAttuazione = Me.IDAESA
        Me.Verifica.IDVerificheAssociate = Me.IDVerificheAssociate
        Me.Verifica.IdRegCompetenza = Me.ddlCompetenza.SelectedValue
    End Sub
    Private Sub CaricaFormDaVerifica()
        Me.txtNumeroFascicolo.Text = Me.Verifica.CodiceFascicolo
        Me.TxtCodiceFasc.Value = Me.Verifica.CodiceFascicoloInterno
        Me.txtDescFasc.Text = Me.Verifica.DescFasicolo
        Me.txtOggetto.Text = Me.Verifica.Oggetto
        Me.txtNumeroProtocollo.Text = Me.Verifica.NProtSegnalazione
        Me.ddEsitoSegnalazione.SelectedValue = Me.Verifica.CodiceVerifica
        Me.txtLetteraDataProt.Text = Me.Verifica.DataProtInvioLetteraInterlocutoria
        Me.txtRispostaDataProt.Text = Me.Verifica.DataProtRispostaLetteraInterlocutoria
        Me.txtDataProtocollo.Text = Me.Verifica.DataProtSegnalazione
        Me.txtDataSegnalazione.Text = Me.Verifica.DataRicezioneSegnalazione
        Me.ddFonte.SelectedValue = IIf(Me.Verifica.Fonte = 0, "", Me.Verifica.Fonte)
        Me.txtNote.Text = Me.Verifica.Note
        Me.txtRispostaNProt.Text = Me.Verifica.NProtRispostaLetteraInterlocutoria
        Me.txtLetteraNProt.Text = Me.Verifica.NProtInvioLetteraInterlocutoria
        Me.idSegnalazione = Me.Verifica.IdSegnalazione
        Me.idVerifica = Me.Verifica.IdVerifica
        'Me.IDAESA = Me.Verifica.IdAttivitaEnteSedeAttuazione
        Me.IDVerificheAssociate = Me.Verifica.IDVerificheAssociate
        Me.Verifica.IdRegCompetenza = Me.IdRegCompetenza
    End Sub
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.lblmessaggiosopra.Text = ""
        Me.lblmessaggiosopra.Visible = False
        ''    Me.Imgerrore.Visible = False
        Me.lblMsgSede.Text = ""
        Me.lblMsgSede.Visible = False

        If Not Me.Page.IsPostBack Then
            clsGui.CaricaDropDown(Me.ddFonte, clsVerificaSegnalazione.RecuperaFonte(Session("Conn")), "NOME", "IDFONTE")
            Verifica = New clsVerificaSegnalazione
            Me.ViewState.Add("Verifica", Verifica)

            'TrovaCompetenzaUtente()
            CaricaCompetenze()

            'gestione da modifica dopo la ricerca verifico se la segnalazione e associata ad una sede e inizializzo form
            If Request.QueryString("idsegnalazione") <> "" Then
                InizializzaWebForm(Request.QueryString("idsegnalazione"), Request.QueryString("idverifica"))
            End If
            ControlloRegioneCompetenza(ddlCompetenza.SelectedValue, 0)

            If Me.dgRisultatoRicerca.Items.Count <> 0 Then
                If Session("Sistema") = "Futuro" And Session("TipoUtente") = "U" Then
                    ddlCompetenza.Enabled = True
                    CaricaCompetenzeFuturo(dgRisultatoRicerca.Items(0).Cells(20).Text, 22)
                Else
                    ddlCompetenza.Enabled = False
                End If

            End If


            Session("IdRegCompetenza") = ddlCompetenza.SelectedValue
            CaricaVerificatore()


            ''gestione da modifica dopo la ricerca verifico se la segnalazione e associata ad una sede e inizializzo form
            'If Request.QueryString("idsegnalazione") <> "" Then
            '    InizializzaWebForm(Request.QueryString("idsegnalazione"), Request.QueryString("idverifica"))
            'End If


            'gestione riguardante il ritorno dalla ricerca della sede per segnalazioni in modifica
            'non ancora associate
            If Request.QueryString("IDAESA") <> "" Then
                Try
                    Verifica = CType(context.Handler, WfrmRicPrgVerifica).Verifica
                    Me.ViewState.Item("verifica") = Verifica
                    Me.IDAESA = Request.QueryString("IDAESA")
                    Me.IdRegCompetenza = Request.QueryString("IdRegCompetenza")
                    Me.EsistenzaSedeVerificaProgrammazione(Request.QueryString("IDAESA"))

                    Me.CaricaFormDaVerifica()
                    'clsVerifica.RecuperaAttivitaEnteSedeAttuazione(Session("Conn"), Request.QueryString("IDAESA"))
                    CType(Me.ViewState("Verifica"), clsVerificaSegnalazione).IdAttivitaEnteSedeAttuazione = CType(Request.QueryString("IDAESA"), Integer)
                    Me.dgRisultatoRicerca.DataSource = clsVerificaSegnalazione.RecuperaAttivitaEnteSedeAttuazione(Session("conn"), CType(Me.ViewState("Verifica"), clsVerificaSegnalazione).IdAttivitaEnteSedeAttuazione)
                    Me.dgRisultatoRicerca.DataBind()
                    'Me.cmdConferma_Click(cmdConferma, System.EventArgs.Empty)
                    Salvataggio()
                    ddlCompetenza.Enabled = False
                Catch ex As Exception
                    Response.Write(ex.Message)
                End Try
            End If
            If Me.dgRisultatoRicerca.Items.Count <> 0 Then
                dvAssegnazione.Visible = True
            Else
                dvAssegnazione.Visible = False
            End If
        End If
        Try
            If (CStr(Me.idVerifica) = "" OrElse CStr(Me.idVerifica) = "0") Then
                Me.CmdLetteraInterlocutoria.Visible = False
                '*** agg. da sc il 12/10/2010 
                '*** rendo invisibili tutti i campi per la selezione del fasciolo e dei protocollo e la stampa della lettera
                'Me.cmdSelFascicolo.Visible = False
                'Me.cmdSelProtocollo0.Visible = False
                ' Me.cmdFascCanc.Visible = False
                ' Me.cmdSelProtocollo.Visible = False
                'Me.cmdAllegati.Visible = False
                Me.cmdSelProtocollo1.Visible = False
                Me.cmdAllegati1.Visible = False
                Me.cmdSelProtocollo2.Visible = False
                Me.cmdAllegati2.Visible = False


                ' txtLetteraDataProt.ReadOnly = True
                ''  ImgLetteraDataProt.Attributes.Add("style", "visibility: hidden")
                ' txtRispostaDataProt.ReadOnly = True
                '' ImgRispostaDataProt.Attributes.Add("style", "visibility: hidden")
                '****
            Else
                Me.CmdLetteraInterlocutoria.Visible = True
                '*** agg. da sc il 12/10/2010 
                '*** rendo visibili tutti i campi per la selezione del fasciolo e dei protocollo e la stampa della lettera
                'Me.cmdSelFascicolo.Visible = True
                'Me.cmdSelProtocollo0.Visible = True
                'Me.cmdFascCanc.Visible = True
                'Me.cmdSelProtocollo.Visible = True
                'Me.cmdAllegati.Visible = True
                If Session("IdRegCompetenza") = 22 Then
                    Me.cmdSelProtocollo1.Visible = True
                    Me.cmdAllegati1.Visible = True
                    Me.cmdSelProtocollo2.Visible = True
                    Me.cmdAllegati2.Visible = True
                End If
                '****
            End If
            'verifico lo stato della segnalazione (se approvata, rendo invisibile tutti i punlsanti del fascicolo)
            StatoSegnalazione(Request.QueryString("idverifica"), Request.QueryString("idsegnalazione"))

        Catch ex As Exception

        End Try
    End Sub
    Private Sub StatoSegnalazione(ByVal idVer As String, ByVal idSeg As String)
        '*** agg. da sc il 12/10/2010 
        Dim idStatoVerifica As Integer
        Dim dr As SqlDataReader = clsVerificaSegnalazione.Get_Select_Modifica(Session("conn"), idSeg, idVer)
        '+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        'ricordati valori dbnull
        '+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        If dr.HasRows Then
            dr.Read()
            If (idVer = "" Or idVer = "0") Then idVer = Nothing
            If Not idVer Is Nothing Then
                If Not IsDBNull(dr("idverifica")) Then Me.idVerifica = dr("idverifica")
                If Not IsDBNull(dr("idstatoverifica")) Then idStatoVerifica = dr("idstatoverifica") Else idStatoVerifica = 0
                dr.Close()
                dr = Nothing
            End If
        End If
        If idStatoVerifica = 5 Then
            Me.CmdLetteraInterlocutoria.Visible = False
            Me.cmdSelFascicolo.Visible = False
            Me.cmdSelProtocollo0.Visible = False
            Me.cmdFascCanc.Visible = False
            Me.cmdSelProtocollo.Visible = False
            Me.cmdAllegati.Visible = False
            Me.cmdSelProtocollo1.Visible = False
            Me.cmdAllegati1.Visible = False
            Me.cmdSelProtocollo2.Visible = False
            Me.cmdAllegati2.Visible = False
        End If
        dr.Close()
        dr = Nothing
    End Sub

    Private Sub imgChiudi_Click(sender As Object, e As EventArgs) Handles imgChiudi.Click
        Response.Redirect("ver_Ricerca_Verifiche_Segnalazioni.aspx")
    End Sub

    Private Sub lBttRicercaAttivitaEnteSedeAttuazione_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lBttRicercaAttivitaEnteSedeAttuazione.Click
        Salvataggio()
        Me.CaricaVerificatoreDaForm()
        Server.Transfer("WfrmRicPrgVerifica.aspx?modifica=si&idsegnalazione=" + CStr(Me.idSegnalazione) + "&idverifica=" + CStr(Me.idVerifica) + "&IdRegCompetenza=" + CStr(Me.IdRegCompetenza) + "")
    End Sub


    Private Function VerificaConferma() As Boolean
        Dim d1 As DateTime
        Dim d2 As DateTime


        Dim campiValidi As Boolean = True
        Dim campoObbligatorio As String = "Il campo {0} è obbligatorio.<br/>"


        lblErrore.Text = ""

        If (ddEsitoSegnalazione.SelectedValue = "") Then
            lblErrore.Text = lblErrore.Text + String.Format(campoObbligatorio, "Esito Segnalazione")
            campiValidi = False
        End If


        If (ddFonte.SelectedValue = "") Then
            lblErrore.Text = lblErrore.Text + String.Format(campoObbligatorio, "Fonte")
            campiValidi = False
        End If

        If (txtOggetto.Text = "") Then
            lblErrore.Text = lblErrore.Text + String.Format(campoObbligatorio, "Oggetto")
            campiValidi = False
        End If

        If (txtDataSegnalazione.Text = "") Then

            'lblmessaggiosopra.Text = "Attenzione ""Data Segnalazione"" è un campo obbligatorio."
            lblErrore.Text = lblErrore.Text + String.Format(campoObbligatorio, "Data Segnalazione")
            campiValidi = False
        End If
        Dim DataSegnalazione As Date
        If (txtDataSegnalazione.Text.Trim <> String.Empty AndAlso Date.TryParse(txtDataSegnalazione.Text, DataSegnalazione) = False) Then
            lblErrore.Text = lblErrore.Text + "La Data Segnalazione non è valida. Inserire la data nel formato GG/MM/AAAA.<br/>"
            campiValidi = False
        End If
        Dim LetteraDataProt As Date
        If (txtLetteraDataProt.Text.Trim <> String.Empty AndAlso Date.TryParse(txtLetteraDataProt.Text, LetteraDataProt) = False) Then
            lblErrore.Text = lblErrore.Text + "La Data Protocollo non è valida. Inserire la data nel formato GG/MM/AAAA.<br/>"
            campiValidi = False
        End If
        Dim RispostaDataProt As Date
        If (txtRispostaDataProt.Text.Trim <> String.Empty AndAlso Date.TryParse(txtRispostaDataProt.Text, RispostaDataProt) = False) Then
            lblErrore.Text = lblErrore.Text + "La Data Protocollo di Risposta non è valida. Inserire la data nel formato GG/MM/AAAA.<br/>"
            campiValidi = False
        End If
        If ((txtDataSegnalazione.Text <> String.Empty) And (txtLetteraDataProt.Text <> String.Empty)) Then

            Try
                d1 = Convert.ToDateTime(txtDataSegnalazione.Text)
                d2 = Convert.ToDateTime(txtLetteraDataProt.Text)
            Catch ex As Exception
                lblErrore.Text = lblErrore.Text + "Errore nel formato delle Date.<br/>"
                campiValidi = False
            End Try
            If (d1 > d2) Then
                lblErrore.Text = lblErrore.Text + "La Data Protocollo deve essere maggiore alla Data Segnalazione.<br/>"
                campiValidi = False
            End If
        End If


        If ((txtLetteraDataProt.Text <> String.Empty) And (txtRispostaDataProt.Text <> String.Empty)) Then
            Try
                d1 = Convert.ToDateTime(txtLetteraDataProt.Text)
                d2 = Convert.ToDateTime(txtRispostaDataProt.Text)
            Catch ex As Exception
                lblErrore.Text = lblErrore.Text + "Errore nel formato delle Date.<br/>"
                campiValidi = False
            End Try
            If (d1 > d2) Then
                lblErrore.Text = lblErrore.Text + "La Data Protocollo di Risposta deve essere maggiore alla Data Protocollo.<br/>"
                campiValidi = False
            End If
        End If

        If ddlVerificatori.SelectedItem.Text <> String.Empty Then
            If txtDataInizioASS.Text = String.Empty Then
                lblErrore.Text = lblErrore.Text + String.Format(campoObbligatorio, "Data Inizio Prevista Verifica")
                campiValidi = False
            End If
            If TxtDataFineASS.Text = String.Empty Then
                lblErrore.Text = lblErrore.Text + String.Format(campoObbligatorio, "Data Fine Prevista Verifica")
                campiValidi = False
            End If
        End If

        If txtDataInizioASS.Text <> String.Empty And ddlVerificatori.SelectedItem.Text = String.Empty Then
            lblErrore.Text = lblErrore.Text + "Non è possibile indicare la Data Inizio Prevista Verifica se non si seleziona il Verificatore.<br/>"
            campiValidi = False
        End If
        If TxtDataFineASS.Text <> String.Empty And ddlVerificatori.SelectedItem.Text = String.Empty Then
            lblErrore.Text = lblErrore.Text + "Non è possibile indicare la Data Fine Prevista Verifica se non si seleziona il Verifcatore.<br/>"
            campiValidi = False
        End If
        Dim DataInizioASS As Date
        If (txtDataInizioASS.Text.Trim <> String.Empty AndAlso Date.TryParse(txtDataInizioASS.Text, DataInizioASS) = False) Then
            lblErrore.Text = lblErrore.Text + "La Data Iniizo Prevista Verifica non è valida. Inserire la data nel formato GG/MM/AAAA.<br/>"
            campiValidi = False
        End If
        Dim DataFineASS As Date
        If (TxtDataFineASS.Text.Trim <> String.Empty AndAlso Date.TryParse(TxtDataFineASS.Text, DataFineASS) = False) Then
            lblErrore.Text = lblErrore.Text + "La Data Fine Prevista Verifica non è valida. Inserire la data nel formato GG/MM/AAAA.<br/>"
            campiValidi = False
        End If
        Dim data As Date
        If ((txtDataInizioASS.Text <> String.Empty) And (TxtDataFineASS.Text <> String.Empty)) Then
            If (Date.TryParse(TxtDataFineASS.Text, data) > Date.TryParse(txtDataInizioASS.Text, data)) Then
                lblErrore.Text = lblErrore.Text + "La Data Fine Prevista Verifica deve essere maggiore alla Data Inzio Prevista Verifica.<br/>"
                campiValidi = False
            End If
        End If

        If D1LTD2(TxtDataFineASS.Text, txtDataInizioASS.Text) Then
            lblErrore.Text = lblErrore.Text + "La Data Fine Prevista Verifica deve essere maggiore alla Data Inzio Prevista Verifica.<br/>"
            campiValidi = False
        End If
        If D1LTD2(TxtDataFineASS.Text, txtDataSegnalazione.Text) Then
            lblErrore.Text = lblErrore.Text + "La Data Fine Prevista Verifica deve essere maggiore alla Data Segnalazione.<br/>"
            campiValidi = False
        End If
        If D1LTD2(txtDataInizioASS.Text, txtDataSegnalazione.Text) Then
            lblErrore.Text = lblErrore.Text + "La Data Inzio Prevista Verifica deve essere maggiore alla Data Segnalazione.<br/>"
            campiValidi = False
        End If

        Return campiValidi
    End Function

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
    Private Sub cmdConferma_Click(sender As Object, e As EventArgs) Handles cmdConferma.Click
        If (VerificaConferma()) Then
            lblmessaggiosopra.Visible = False
            Salvataggio()
        Else
            lblErrore.Visible = True
        End If
    End Sub
    Private Sub Salvataggio()
        Dim Ver As clsVerificaSegnalazione
        Dim i As Integer
        Ver = Me.ViewState("Verifica")
        Ver.CodiceVerifica = clsConversione.DaStringaInNothing(Me.ddEsitoSegnalazione.SelectedValue)
        Ver.CodiceFascicolo = clsConversione.DaStringaInNothing(Me.txtNumeroFascicolo.Text)
        Ver.CodiceFascicoloInterno = clsConversione.DaStringaInNothing(Me.TxtCodiceFasc.Value)
        Ver.DescFasicolo = clsConversione.DaStringaInNothing(Me.txtDescFasc.Text)
        Ver.Note = Me.txtNote.Text
        Ver.Fonte = clsConversione.DaStringaInNothing(Me.ddFonte.SelectedValue)
        Ver.NProtInvioLetteraInterlocutoria = Me.txtLetteraNProt.Text
        Ver.Oggetto = Me.txtOggetto.Text
        Ver.NProtRispostaLetteraInterlocutoria = Me.txtRispostaNProt.Text
        Ver.NProtSegnalazione = Me.txtNumeroProtocollo.Text
        Ver.UserInseritore = Session("utente")
        Ver.DataRicezioneSegnalazione = Me.txtDataSegnalazione.Text
        Ver.DataProtSegnalazione = Me.txtDataProtocollo.Text
        Ver.DataProtRispostaLetteraInterlocutoria = Me.txtRispostaDataProt.Text
        Ver.DataProtInvioLetteraInterlocutoria = Me.txtLetteraDataProt.Text
        Ver.IdVerifica = Me.idVerifica
        Ver.IdAttivitaEnteSedeAttuazione = Me.IDAESA
        Ver.IdSegnalazione = Me.idSegnalazione
        Ver.IDVerificheAssociate = Me.IDVerificheAssociate
        Ver.IdRegCompetenza = Me.ddlCompetenza.SelectedValue
        Ver.DataInizioPrevista = Me.txtDataInizioASS.Text
        Ver.DataFinePrevista = Me.TxtDataFineASS.Text
        If Ver.Modifica(Session("conn"), i) Then
            'clsGui.SvuotaCampi(Me) , Request.QueryString("idsegnalazione")

            Me.lblmessaggiosopra.Text = "MODIFICA EFFETTUATA."
            Me.lblmessaggiosopra.Visible = True

            'CmdLetteraInterlocutoria.Visible = True
            If ddlVerificatori.SelectedItem.Text <> String.Empty Then
          
                clsVerificaSegnalazione.SP_InserimentoModifica_Verificatore(Session("conn"), CInt(Ver.IdVerifica), IDVERIFICAVERIFICATORI, Me.ddlVerificatori.SelectedValue, Me.txtDataInizioASS.Text, Me.TxtDataFineASS.Text, Session("utente"))
            End If
            InizializzaWebForm(Request.QueryString("idsegnalazione"), IIf(i <> 0, i, Nothing))
            If i <> 0 Then Me.idVerifica = i
        Else
            Me.lblErrore.Text = "ATTENZIONE MODIFICA FALLITA."
            Me.lblErrore.Visible = True
            ''   Me.Imgerrore.Visible = True
        End If
    End Sub
    Property IDVERIFICAVERIFICATORI() As Integer
        Get
            Return Me.ViewState("IdVerificaVerificatori")
        End Get
        Set(ByVal Value As Integer)
            Me.ViewState("IdVerificaVerificatori") = Value
        End Set
    End Property
    Private Sub InizializzaWebForm(ByVal idseg As String, ByVal idver As String)
        Dim idStatoVerifica As Integer
        Dim dataTablePerGriglia As DataTable
        Dim dr As SqlDataReader = clsVerificaSegnalazione.Get_Select_Modifica(Session("conn"), idseg, idver)
        '+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        'ricordati valori dbnull
        '+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        If dr.HasRows Then
            dr.Read()
            If Not IsDBNull(dr("nprotsegnalazione")) Then Me.txtNumeroProtocollo.Text = dr("nprotsegnalazione") Else Me.txtNumeroProtocollo.Text = ""
            If Not IsDBNull(dr("codicefascicolo")) Then Me.txtNumeroFascicolo.Text = dr("codicefascicolo") Else Me.txtNumeroFascicolo.Text = ""
            If Not IsDBNull(dr("idfascicolo")) Then Me.TxtCodiceFasc.Value = dr("idfascicolo") Else Me.TxtCodiceFasc.Value = ""
            If Not IsDBNull(dr("descrfascicolo")) Then Me.txtDescFasc.Text = dr("descrfascicolo") Else Me.txtDescFasc.Text = ""
            If Not IsDBNull(dr("dataricezionesegnalazione")) Then Me.txtDataSegnalazione.Text = dr("dataricezionesegnalazione") Else Me.txtDataSegnalazione.Text = ""
            If Not IsDBNull(dr("dataprotsegnalazione")) Then Me.txtDataProtocollo.Text = dr("dataprotsegnalazione") Else Me.txtDataProtocollo.Text = ""
            If Not IsDBNull(dr("fonte")) Then Me.ddFonte.SelectedValue = dr("fonte") Else Me.ddFonte.SelectedIndex = 0
            If Not IsDBNull(dr("oggetto")) Then Me.txtOggetto.Text = dr("oggetto") Else Me.txtOggetto.Text = ""
            If Not IsDBNull(dr("esitosegnalazione")) Then Me.ddEsitoSegnalazione.SelectedValue = dr("esitosegnalazione") Else Me.ddEsitoSegnalazione.SelectedIndex = 0
            If Not IsDBNull(dr("nprotinvioletterainterlocutoria")) Then Me.txtLetteraNProt.Text = dr("nprotinvioletterainterlocutoria") Else Me.txtLetteraNProt.Text = ""
            If Not IsDBNull(dr("dataprotinvioletterainterlocutoria")) Then Me.txtLetteraDataProt.Text = dr("dataprotinvioletterainterlocutoria") Else Me.txtLetteraDataProt.Text = ""
            If Not IsDBNull(dr("nprotrispostaletterainterlocutoria")) Then Me.txtRispostaNProt.Text = dr("nprotrispostaletterainterlocutoria") Else Me.txtRispostaNProt.Text = ""
            If Not IsDBNull(dr("dataprotrispostaletterainterlocutoria")) Then Me.txtRispostaDataProt.Text = dr("dataprotrispostaletterainterlocutoria") Else Me.txtRispostaDataProt.Text = ""
            If Not IsDBNull(dr("note")) Then Me.txtNote.Text = dr("note") Else Me.txtNote.Text = ""
            If Not IsDBNull(dr("idsegnalazione")) Then Me.idSegnalazione = dr("idsegnalazione")
            If Not IsDBNull(dr("IdRegCompetenza")) Then Me.ddlCompetenza.SelectedValue = dr("IdRegCompetenza") Else Me.ddlCompetenza.SelectedValue = 0

            If (idver = "" Or idver = "0") Then idver = Nothing

            If Not idver Is Nothing Then
                If Not IsDBNull(dr("idverifica")) Then Me.idVerifica = dr("idverifica")
                If Not IsDBNull(dr("IDAESA")) Then Me.IDAESA = dr("IDAESA")
                If Not IsDBNull(dr("idstatoverifica")) Then idStatoVerifica = dr("idstatoverifica") Else idStatoVerifica = 0
                If Not IsDBNull(dr("IDVerificheAssociate")) Then Me.IDVerificheAssociate = dr("IDVerificheAssociate")
                dr.Close()
                dr = Nothing
                Me.dgRisultatoRicerca.DataSource = clsVerificaSegnalazione.RecuperaAttivitaEnteSedeAttuazione(Session("conn"), Me.IDAESA)
                Me.dgRisultatoRicerca.DataBind()
               
            Else
                dr.Close()
                dr = Nothing
            End If
        Else
            dr.Close()
            dr = Nothing
        End If 'If dr.HasRows Then

        GestioneViusalizzazioneBottoni(Me.idVerifica, idStatoVerifica)


    End Sub
    Private Sub GestioneViusalizzazioneBottoni(ByVal idVer As Integer, ByVal idStato As Integer)
        'tipostato
        'registrata =1
        'assegnata=4
        'aperta o approvata = 5

        If idVer = 0 Then
            Me.cmdConferma.Visible = True
            Me.lBttRicercaAttivitaEnteSedeAttuazione.Visible = True
            Me.cmdApprovazione.Visible = False
            'Me.txtLetteraNProt.Enabled = False
            'Me.txtLetteraDataProt.Enabled = False
            'Me.txtRispostaNProt.Enabled = False
            'Me.txtRispostaDataProt.Enabled = False
        Else
            Select Case idStato

                Case Is = 1
                    Me.cmdConferma.Visible = True
                    Me.lBttRicercaAttivitaEnteSedeAttuazione.Visible = True
                    Me.cmdApprovazione.Visible = False
                    'Me.txtLetteraNProt.Enabled = True
                    'Me.txtLetteraDataProt.Enabled = True
                    'Me.txtRispostaNProt.Enabled = True
                    'Me.txtRispostaDataProt.Enabled = True
                Case Is = 4
                    Me.cmdConferma.Visible = True
                    Me.lBttRicercaAttivitaEnteSedeAttuazione.Visible = False
                    Me.cmdApprovazione.Visible = True
                    VisualizzaDatiInSolaLettura()
                Case Is = 5
                    Me.cmdConferma.Visible = False
                    Me.lBttRicercaAttivitaEnteSedeAttuazione.Visible = False
                    Me.cmdApprovazione.Visible = False
                    '*** agg. da sc il 12/10/2010 
                    '*** rendo invisibili tutti i campi per la selezione del fasciolo e dei protocollo e la stampa della lettera
                    Me.cmdSelFascicolo.Visible = False
                    Me.cmdSelProtocollo0.Visible = False
                    Me.cmdFascCanc.Visible = False
                    Me.cmdSelProtocollo.Visible = False
                    Me.cmdAllegati.Visible = False
                    Me.cmdSelProtocollo1.Visible = False
                    Me.cmdAllegati1.Visible = False
                    Me.cmdSelProtocollo2.Visible = False
                    Me.cmdAllegati2.Visible = False
                    Me.CmdLetteraInterlocutoria.Visible = False
                    'txtDataSegnalazione.ReadOnly = True
                    '' Me.ImgDataSegnalazione.Visible = False
                    '****
                    VisualizzaDatiInSolaLettura()

                Case Else 'ipotesi non prevista
                    Me.cmdConferma.Visible = False
                    Me.lBttRicercaAttivitaEnteSedeAttuazione.Visible = False
                    Me.cmdApprovazione.Visible = False
            End Select
        End If
    End Sub
    Private Sub VisualizzaDatiInSolaLettura()
        Dim d As SqlDataReader
        Dim pippo As String
        If CStr(Me.idVerifica) <> "" Or CStr(Me.idVerifica) <> "" Then
            d = clsVerificaSegnalazione.Get_Select_Verificatori(Session("conn"), Me.idVerifica)
            If d.HasRows Then
                d.Read()
                'Me.Lblverificatore.Text = ""
                '' Me.LblIspettorenonIGF.Text = ""
                'Me.LblAssegna.Text = ""
                'Me.lblDataInizioASS.Text = ""
                'Me.lblDataFineASS.Text = ""
                Me.lblaccredita.Text = ""
                If Not IsDBNull(d("NOMINATIVO")) Then
                    ddlVerificatori.SelectedValue = d("idverificatore")
                    IDVERIFICAVERIFICATORI = d("IDVerificaVerificatori")
                    If Not IsDBNull(d("CIRCOSCRIZIONE")) Then
                        'Me.Lblverificatore.Visible = True
                        pippo = "Circoscrizione: " & d("CIRCOSCRIZIONE")
                        LblCirc.Text = pippo
                    End If

                    'If Not IsDBNull(d("NOMINATIVO")) Then Me.Lblverificatore.Text += "Verificatore IGF: " + d("NOMINATIVO") + " " + pippo
                    ' Me.LblIspettorenonIGF.Visible = True
                    'Me.LblIspettorenonIGF.Text = "Verificatore INT: " + d("NOMINATIVOINT")
                    'Else
                    '    If Not IsDBNull(d("CIRCOSCRIZIONE")) Then
                    '        Me.Lblverificatore.Visible = True
                    '        pippo = "Circoscrizione: " & d("CIRCOSCRIZIONE")
                    '    End If
                    '    If Not IsDBNull(d("NOMINATIVO")) Then Me.Lblverificatore.Text += "Verificatore INT: " + d("NOMINATIVO") + " " + pippo
                End If
                If Not IsDBNull(d("DASS")) Then
                    Me.LblAssegna.Visible = True
                    Me.LblAssegna.Text = "Data Assegnazione: " + d("DASS")
                    Me.lblDataInizioASS.Visible = True
                    Me.lblDataFineASS.Visible = True
                    ' Me.lblDataInizioASS.Text = "Data prevista verifica "
                    If Not IsDBNull(d("DPV")) Then
                        Me.txtDataInizioASS.Text = d("DPV")
                    End If

                    'Me.lblDataFineASS.Text = "Data fine prevista verifica"
                    If Not IsDBNull(d("DFPV")) Then
                        Me.TxtDataFineASS.Text = d("DFPV")
                    End If

                End If
                If Not IsDBNull(d("DAPP")) Then
                    Me.lblaccredita.Visible = True
                    Me.lblaccredita.Text = "Data Approvazione: " + d("DAPP")
                End If
            End If
            d.Close()
            d = Nothing
        End If

    End Sub
    Private Sub cmdApprovazione_Click(sender As Object, e As EventArgs) Handles cmdApprovazione.Click
        If clsVerificaSegnalazione.Approvazione_Ver_Seg(Session("conn"), Me.idVerifica, Session("utente")) Then
            Me.lblmessaggiosopra.Text = "APPROVAZIONE EFFETTUATA."
            Me.lblmessaggiosopra.Visible = True
            'CmdLetteraInterlocutoria.Visible = True
            InizializzaWebForm(Request.QueryString("idsegnalazione"), Request.QueryString("idverifica"))
        Else
            Me.lblmessaggiosopra.Text = "ATTENZIONE APPROVAZIONE FALLITA."
            Me.lblmessaggiosopra.Visible = True
            '' Me.Imgerrore.Visible = True
        End If
    End Sub
    Private Sub CmdLetteraInterlocutoria_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles CmdLetteraInterlocutoria.Click
        Dim Documento As New GeneratoreModelli

        Response.Write("<SCRIPT>" & vbCrLf)
        Response.Write("window.open('" & Documento.MON_letterainterlocutoria(ClsUtility.TrovaIDEnteDaIDVerifica(Me.idVerifica, Session("conn")), Me.idVerifica, Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn")) & "');" & vbCrLf)
        Response.Write("</SCRIPT>")
        'chiamo la proprietà della classe GeneratoreModelli che ritorna la path del documento creato
        Documento.Dispose()

        'StampaDocumentiGen(Me.idVerifica, "letterainterlocutoria")
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
            Call VISTARoccoDati(intIdVERIFICA)
            If dtrLeggiDati.HasRows = True Then
                dtrLeggiDati.Read()
                'creo il nome del file
                strNomeFile = NomeFile & CStr(Session("IdEnte")) & CStr(Session("Username")) & CStr(Day(Now)) & CStr(Month(Now)) & CStr(Year(Now)) & Hour(Now) & Minute(Now) & Second(Now) & ".doc"
                'creo il percorso del file da salvare
                strPercorsoFile = Server.MapPath("./documentazione/") & strNomeFile
                'apro il file che fa da template
                Reader = New StreamReader(Server.MapPath("./documentazione/master/NAZ/" & Session("Path") & NomeFile & ".rtf"))
                Writer = New StreamWriter(strPercorsoFile, True, System.Text.Encoding.Default)
                'Writer.WriteLine("{\rtf1")
                xLinea = Reader.ReadLine()
                'Dim strDataCostituzione As String = dtrLeggiDati("DataCostituzione")
                'Dim strdataProt As String = dtrLeggiDati("DataProtIncarico")
                Dim strNProtInvioLetteraInterlocutoria As String = dtrLeggiDati("NProtInvioLetteraInterlocutoria")
                'Dim strnumprot As String = dtrLeggiDati("NprotIncarico")
                'Dim cognome As String = dtrLeggiDati("Cognome")
                'Dim nome As String = dtrLeggiDati("Nome")
                'Dim strAnagrafico As String = cognome & " " & nome
                'Dim DataInizioVerifica As String = dtrLeggiDati("DataInizioVerifica")
                'Dim DataFineVerifica As String = dtrLeggiDati("DataFineVerifica")
                'Dim Bando As String = dtrLeggiDati("Bando")
                'Dim settore As String = dtrLeggiDati("Settore")
                'Dim Area As String = dtrLeggiDati("Area")
                'Dim Nvol As String = dtrLeggiDati("NumeroVolontari")
                'Dim datarispostaente As String = dtrLeggiDati("DataProtRispostaLetteraContestazion")
                'Dim DataInvioFax As String = dtrLeggiDati("DataInvioFax")
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
                'Dim AntodataIA As String = dtrLeggiDati("datainizioattività")
                While xLinea <> ""
                    xLinea = Replace(xLinea, "<DataOdierna>", strDataOdierna)
                    xLinea = Replace(xLinea, "<DenominazioneEnte>", strDenominazioneEnte)
                    xLinea = Replace(xLinea, "<CodiceEnte>", strCodiceEnte)
                    xLinea = Replace(xLinea, "<Titolo>", strtitolo)
                    xLinea = Replace(xLinea, "<Cap>", strCap)
                    'xLinea = Replace(xLinea, "<DataProt>", strdataProt)
                    xLinea = Replace(xLinea, "<Nprot>", strNProtInvioLetteraInterlocutoria)
                    'xLinea = Replace(xLinea, "<CognomeNome>", strAnagrafico)
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
                    'xLinea = Replace(xLinea, "<DataInizioVerifica>", DataInizioVerifica)
                    'xLinea = Replace(xLinea, "<DataFineVerifica>", DataFineVerifica)
                    xLinea = Replace(xLinea, "<CodiceProggetto>", strCodiceProgetto)
                    'xLinea = Replace(xLinea, "<Bando>", Bando)
                    'xLinea = Replace(xLinea, "<Settore>", settore)
                    'xLinea = Replace(xLinea, "<Area>", Area)
                    'xLinea = Replace(xLinea, "<Nvol>", Nvol)
                    'xLinea = Replace(xLinea, "<datainizioattivita>", AntodataIA)
                    'xLinea = Replace(xLinea, "<datarispostaente>", datarispostaente)
                    'xLinea = Replace(xLinea, "<DataInvioFax>", DataInvioFax)

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
            StampaDocumentiGen = "documentazione/" & strNomeFile
        Catch ex As Exception
            Response.Write(ex.Message)
        Finally
            If Not dtrLeggiDati Is Nothing Then
                dtrLeggiDati.Close()
                dtrLeggiDati = Nothing
            End If
        End Try
        Response.Write("<SCRIPT>" & vbCrLf)
        Response.Write("window.open('" & StampaDocumentiGen & "');" & vbCrLf)
        Response.Write("</SCRIPT>")
    End Function
    Sub VISTARoccoDati(ByVal idverificaADC)
        Dim strsql As String
        'chiudo il datareader
        If Not dtrLeggiDati Is Nothing Then
            dtrLeggiDati.Close()
            dtrLeggiDati = Nothing
        End If
        strsql = "select * from VW_VER_RICERCA_VERIFICHE "
        strsql = strsql & "where idverifica=" & idverificaADC
        'eseguo la query e passo il risultato al datareader
        dtrLeggiDati = ClsServer.CreaDatareader(strsql, Session("conn"))
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

                strSQL = " Select IdRegioneCompetenza,case when Descrizione ='Nazionale' then UPPER(Descrizione) ELSE Descrizione end AS Descrizione,CodiceRegioneCompetenza "
                strSQL = strSQL & " from RegioniCompetenze "
                strSQL = strSQL & " ORDER BY CASE WHEN left(CodiceRegioneCompetenza,1)='N' then 1 else 2 end,descrizione "
                ''strSQL = strSQL & " union "
                ''trSQL = strSQL & " select '0',' TUTTI ','','A' "
                ''strSQL = strSQL & " union "
                ''strSQL = strSQL & " select '',' NAZIONALE ','','B' "
                ''strSQL = strSQL & " union "
                ''strSQL = strSQL & " select '-2',' REGIONALE ','','C' "
                ''strSQL = strSQL & " union "
                ''strSQL = strSQL & " select '-3',' NON DEFINITO ','','D' "
                ''strSQL = strSQL & "  from RegioniCompetenze order by left(CodiceRegioneCompetenza,1),descrizione "
                'chiudo il datareader se aperto
                If Not dtrCompetenze Is Nothing Then
                    dtrCompetenze.Close()
                    dtrCompetenze = Nothing
                End If

                'eseguo la query
                dtrCompetenze = ClsServer.CreaDatareader(strSQL, Session("conn"))
                'assegno il datadearder alla combo caricando così descrizione e id
                ddlCompetenza.DataSource = dtrCompetenze
                ddlCompetenza.Items.Add("")
                ddlCompetenza.DataTextField = "Descrizione"
                ddlCompetenza.DataValueField = "IDRegioneCompetenza"
                ddlCompetenza.DataBind()

                If Not dtrCompetenze Is Nothing Then
                    dtrCompetenze.Close()
                    dtrCompetenze = Nothing
                End If

                'chiudo il datareader se aperto
            End If
            'Controllo abilitazione scelta
            If Session("TipoUtente") = "U" Then
                ddlCompetenza.Enabled = True
                'ddlCompetenza.SelectedIndex = 0
                ddlCompetenza.SelectedValue = 0
            Else
                'CboCompetenza.SelectedIndex = 1
                ddlCompetenza.Enabled = False
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
                    ddlCompetenza.SelectedValue = dtrCompetenze("IdRegioneCompetenza")
                    If dtrCompetenze("Heliosread") = True Then
                        ddlCompetenza.Enabled = True
                    End If
                    'Session("IdRegCompetenza") = ddlCompetenza.SelectedValue
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
    Private Sub TrovaCompetenzaUtente()
        'stringa per la query
        Dim strSQL As String

        'datareader che conterrà l'id 
        Dim dtrCompetenze As System.Data.SqlClient.SqlDataReader

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
            Session("IdRegCompetenza") = dtrCompetenze("IdRegioneCompetenza")
            'If dtrCompetenze("Heliosread") = True Then
            '    ddlCompetenza.Enabled = True
            'End If
        End If
        If Not dtrCompetenze Is Nothing Then
            dtrCompetenze.Close()
            dtrCompetenze = Nothing
        End If
    End Sub
    Private Sub ControlloRegioneCompetenza(ByVal IdRegioneCompetenza As Integer, ByVal Load As Byte)
        '*** Creata il 15/11/2007 da Simona Cordella
        '*** Se competenza regionale <> 22 (Nazionale) disabiliti campi della protocollazione

        If IdRegioneCompetenza <> 22 Then
            'load = 0 (richiamato la funzione dela load della pagina)
            'load = 1 (richimao la funzione dalla c0mbo
            If Load = 1 Then

                txtNumeroFascicolo.Text = ""
                txtDescFasc.Text = ""
                txtNumeroProtocollo.Text = ""
                txtDataProtocollo.Text = ""
            End If


            'txtNumeroFascicolo.ReadOnly = False
            'txtDescFasc.ReadOnly = False
            'txtNumeroProtocollo.ReadOnly = False
            cmdSelFascicolo.Visible = False
            cmdSelProtocollo0.Visible = False
            cmdFascCanc.Visible = False
            cmdSelProtocollo.Visible = False
            cmdAllegati.Visible = False
            'txtLetteraNProt.ReadOnly = False
            'txtLetteraDataProt.ReadOnly = False
            cmdSelProtocollo1.Visible = False
            cmdAllegati1.Visible = False
            'txtRispostaNProt.ReadOnly = False
            'txtRispostaDataProt.ReadOnly = False
            cmdSelProtocollo2.Visible = False
            cmdAllegati2.Visible = False

            'ImgDataProtocollo.Attributes.Add("style", "visibility: visible")
            'ImgLetteraDataProt.Attributes.Add("style", "visibility: visible")
            'ImgRispostaDataProt.Attributes.Add("style", "visibility: visible")

            'txtDataProtocollo.ReadOnly = False
            'txtLetteraDataProt.ReadOnly = False
            'txtRispostaDataProt.ReadOnly = False
        Else
            If Load = 1 Then
                txtNumeroFascicolo.Text = ""
                txtDescFasc.Text = ""
                txtNumeroProtocollo.Text = ""
                txtDataProtocollo.Text = ""
            End If


            'txtNumeroFascicolo.ReadOnly = True
            'txtDescFasc.ReadOnly = True
            'txtNumeroProtocollo.ReadOnly = True
            cmdSelFascicolo.Visible = True
            cmdSelProtocollo0.Visible = True
            cmdFascCanc.Visible = True
            cmdSelProtocollo.Visible = True
            cmdAllegati.Visible = True
            'txtLetteraNProt.ReadOnly = True
            'txtLetteraDataProt.ReadOnly = True
            cmdSelProtocollo1.Visible = True
            cmdAllegati1.Visible = True
            'txtRispostaNProt.ReadOnly = True
            'txtRispostaDataProt.ReadOnly = True
            cmdSelProtocollo2.Visible = True
            cmdAllegati2.Visible = True
            'ImgDataProtocollo.Attributes.Add("style", "visibility: hidden")
            'ImgLetteraDataProt.Attributes.Add("style", "visibility: hidden")
            'ImgRispostaDataProt.Attributes.Add("style", "visibility: hidden")
            'txtDataProtocollo.ReadOnly = True
            'txtLetteraDataProt.ReadOnly = True
            'txtRispostaDataProt.ReadOnly = True
        End If
    End Sub
    Sub CaricaCompetenzeFuturo(ByVal IdRegioneCompetenzaProgetto As Integer, ByVal IDRegioneCompetenzaUtente As Integer)
        'stringa per la query
        Dim strSQL As String

        'datareader che conterrà l'id 
        Dim dtrCompetenze As System.Data.SqlClient.SqlDataReader

        Try
            'ddlCompetenza.SelectedIndex = 0
            ddlCompetenza.Items.Clear()

            'CboCompetenza.SelectedIndex = 1
            ' ddlCompetenza.Enabled = False
            'preparo la query
            strSQL = "  Select ltrim(rtrim(IdRegioneCompetenza)) as IdRegioneCompetenza,case when Descrizione ='Nazionale' then UPPER(Descrizione) ELSE Descrizione end AS Descrizione,"
            strSQL &= " CodiceRegioneCompetenza  "
            strSQL &= " FROM RegioniCompetenze  "
            strSQL &= " WHERE  IdRegioneCompetenza in  (" & IdRegioneCompetenzaProgetto & " ," & IDRegioneCompetenzaUtente & ")"
            strSQL &= "ORDER BY CASE WHEN left(CodiceRegioneCompetenza,1)='N' then 1 else 2 end,descrizione"
            'chiudo il datareader se aperto
            If Not dtrCompetenze Is Nothing Then
                dtrCompetenze.Close()
                dtrCompetenze = Nothing
            End If
            dtrCompetenze = ClsServer.CreaDatareader(strSQL, Session("conn"))
            'assegno il datadearder alla combo caricando così descrizione e id
            ddlCompetenza.DataSource = dtrCompetenze
            ddlCompetenza.Items.Add("")
            ddlCompetenza.DataTextField = "Descrizione"
            ddlCompetenza.DataValueField = "IDRegioneCompetenza"
            ddlCompetenza.DataBind()

            If Not dtrCompetenze Is Nothing Then
                dtrCompetenze.Close()
                dtrCompetenze = Nothing
            End If


            'If dtrCompetenze("Heliosread") = True Then
            '    ddlCompetenza.Enabled = True
            'End If


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
    Private Sub ddlCompetenza_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlCompetenza.SelectedIndexChanged
        If ddlCompetenza.SelectedValue <> "" Then
            ControlloRegioneCompetenza(ddlCompetenza.SelectedValue, 1)
            CaricaVerificatore()
        End If
    End Sub

    Private Sub EsistenzaSedeVerificaProgrammazione(ByVal IDAESA As String)
        'creato da simona cordella il 14/06/2011
        'IDAESA --> IDATTIVITàENTESEDEATTUAZIONE
        Dim strSql As String
        Dim rstProg As SqlClient.SqlDataReader

        strSql = " SELECT  dbo.FN_EsistenzaSedeVerificaSegnalata(" & IDAESA & ") as EsistenzaSede, TVerificheProgrammazione.DESCRIZIONE " & _
                 " FROM TVerificheAssociate " & _
                 " INNER JOIN TVerifiche ON TVerificheAssociate.IDVerifica = TVerifiche.IDVerifica " & _
                 " INNER JOIN TVerificheProgrammazione ON TVerifiche.IDProgrammazione = TVerificheProgrammazione.IDProgrammazione " & _
                 " WHERE IDAttivitàEnteSedeAttuazione = " & IDAESA & " "
        rstProg = ClsServer.CreaDatareader(strSql, Session("Conn"))
        If rstProg.HasRows = True Then
            rstProg.Read()
            If rstProg("EsistenzaSede") = 1 Then
                Me.lblMsgSede.Text = "ATTENZIONE!!  La sede indicata è già stata inserita nella programmazione " & rstProg("DESCRIZIONE")
                Me.lblMsgSede.Visible = True
                'Me.Imgerrore.Visible = True
            End If
        End If
        If Not rstProg Is Nothing Then
            rstProg.Close()
            rstProg = Nothing
        End If
    End Sub

    Protected Sub cmdFascCanc_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles cmdFascCanc.Click
        txtNumeroFascicolo.Text = ""
        TxtCodiceFasc.Value = ""
        txtDescFasc.Text = ""
        txtNumeroProtocollo.Text = ""
        txtDataProtocollo.Text = ""
        txtLetteraNProt.Text = ""
        txtLetteraDataProt.Text = ""
        txtRispostaNProt.Text = ""
        txtRispostaDataProt.Text = ""
    End Sub
    Private Sub CaricaVerificatore()
        Dim strsql As String
        Dim dtsVer As DataSet
        '*****Carico Combo Verificatori 

        strsql = " SELECT 0 as IdVerificatore, ''  As Nominativo " & _
                 " UNION " & _
                 " SELECT IdVerificatore, (Cognome + ' ' + Nome) As Nominativo " & _
                 " FROM TVerificatori " & _
                 " WHERE Tipologia=0 AND Abilitato=0 "

        If ddlCompetenza.SelectedValue <> "" Then
            Select Case ddlCompetenza.SelectedValue
                Case 0
                    strsql = strsql & ""
                Case -1
                    strsql = strsql & " and IdRegCompetenza = 22"
                Case -2
                    strsql = strsql & " and IdRegCompetenza <> 22 And not IdRegCompetenza is null "
                Case -3
                    strsql = strsql & " and IdRegCompetenza is null "
                Case Else
                    strsql = strsql & " and IdRegCompetenza = " & ddlCompetenza.SelectedValue
            End Select
        End If
        strsql = strsql & " ORDER BY IdVerificatore "
        dtsVer = ClsServer.DataSetGenerico(strsql, Session("conn"))

        ddlVerificatori.DataSource = dtsVer
        ddlVerificatori.DataTextField = "Nominativo"
        ddlVerificatori.DataValueField = "IdVerificatore"
        ddlVerificatori.DataBind()
    End Sub

    Private Sub dgRisultatoRicerca_ItemCommand(source As Object, e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgRisultatoRicerca.ItemCommand
        If e.CommandName = "LinkVol" Then

            Dim resp As StringBuilder = New StringBuilder()
            Dim windowOption As String = "width = 900, height = 700, dependent=no,scrollbars=yes,status=no,resizable=yes"
            '"dependent=no,scrollbars=yes,status=no,resizable=yes,width=1000px ,height=400px"
            resp.Append("<script  type=""text/javascript"">" & vbCrLf)
            resp.Append("myWin = window.open('Ver_PersonaleEnte.aspx?idattivita=" + e.Item.Cells(18).Text + "&Idente=" + e.Item.Cells(19).Text + "&IDAESA=" + e.Item.Cells(6).Text + "&IDEnteSedeAttuazione=" + e.Item.Cells(11).Text + "', 'win'" + ",'" + windowOption + "')")
            resp.Append("</script>")
            Response.Write(resp.ToString())
        End If
    End Sub

End Class