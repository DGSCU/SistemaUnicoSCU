Public Class WfrmGestioneVerSegnalazione
    Inherits System.Web.UI.Page
    Public Verifica As New clsVerificaSegnalazione
#Region " Codice generato da Progettazione Web Form "

    'Chiamata richiesta da Progettazione Web Form.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
   

    'NOTA: la seguente dichiarazione è richiesta da Progettazione Web Form.
    'Non spostarla o rimuoverla.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: questa chiamata al metodo è richiesta da Progettazione Web Form.
        'Non modificarla nell'editor del codice.
        InitializeComponent()
    End Sub

#End Region

    Public Property IDD() As String
        Get
            Return Me.ViewState("idd")
        End Get
        Set(ByVal Value As String)
            Me.ViewState("idd") = Value
        End Set
    End Property

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
        Me.Verifica.DataRicezioneSegnalazione = Me.txtDataSegnalazione.Text
        Me.Verifica.NProtRispostaLetteraInterlocutoria = Me.txtRispostaNProt.Text
        Me.Verifica.NProtInvioLetteraInterlocutoria = Me.txtLetteraNProt.Text
        Me.Verifica.IdRegCompetenza = Me.ddlCompetenza.SelectedValue

    End Sub

    Private Sub CaricaFormDaVerifica()
        Me.txtNumeroProtocollo.Text = Me.Verifica.NProtSegnalazione
        Me.txtNumeroFascicolo.Text = Me.Verifica.CodiceFascicolo
        Me.TxtCodiceFasc.Value = Me.Verifica.CodiceFascicoloInterno
        Me.txtDescFasc.Text = Me.Verifica.DescFasicolo
        Me.txtOggetto.Text = Me.Verifica.Oggetto
        Me.ddFonte.SelectedValue = IIf(Me.Verifica.Fonte = 0, "", Me.Verifica.Fonte)
        Me.txtRispostaDataProt.Text = Me.Verifica.DataProtRispostaLetteraInterlocutoria
        Me.txtLetteraDataProt.Text = Me.Verifica.DataProtInvioLetteraInterlocutoria
        Me.txtRispostaNProt.Text = Me.Verifica.NProtRispostaLetteraInterlocutoria
        Me.txtLetteraNProt.Text = Me.Verifica.NProtInvioLetteraInterlocutoria
        Me.txtNote.Text = Me.Verifica.Note
        Me.txtDataSegnalazione.Text = Me.Verifica.DataRicezioneSegnalazione
        Me.txtDataProtocollo.Text = Me.Verifica.DataProtSegnalazione
        Me.ddEsitoSegnalazione.SelectedValue = Me.Verifica.CodiceVerifica
        Me.Verifica.IdRegCompetenza = Me.ddlCompetenza.SelectedValue
    End Sub


    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Inserire qui il codice utente necessario per inizializzare la pagina
        Me.lblmessaggiosopra.Text = ""
        Me.lblmessaggiosopra.Visible = False
        ''   Me.Imgerrore.Visible = False
        Me.lblMsgSede.Text = ""
        Me.lblMsgSede.Visible = False

        CmdLetteraInterlocutoria.Visible = False


        If Not Me.Page.IsPostBack Then
            clsGui.CaricaDropDown(Me.ddFonte, clsVerificaSegnalazione.RecuperaFonte(Session("Conn")), "NOME", "IDFONTE")
            Verifica = New clsVerificaSegnalazione
            Me.ViewState.Add("Verifica", Verifica)

            TrovaCompetenzaUtente()
            CaricaCompetenze()
            ControlloRegioneCompetenza(ddlCompetenza.SelectedValue)
            If Request.QueryString("IDAESA") <> "" Then
                Try
                    Verifica = CType(context.Handler, WfrmRicPrgVerifica).Verifica
                    Me.ViewState.Item("verifica") = Verifica
                    Me.CaricaFormDaVerifica()
                    Me.IDD = Request.QueryString("IDAESA")

                    Me.EsistenzaSedeVerificaProgrammazione(Request.QueryString("IDAESA"))
                    'clsVerifica.RecuperaAttivitaEnteSedeAttuazione(Session("Conn"), Request.QueryString("IDAESA"))
                    CType(Me.ViewState("Verifica"), clsVerificaSegnalazione).IdAttivitaEnteSedeAttuazione = CType(Request.QueryString("IDAESA"), Integer)
                    Me.dgRisultatoRicerca.Visible = True
                    Me.dgRisultatoRicerca.DataSource = clsVerificaSegnalazione.RecuperaAttivitaEnteSedeAttuazione(Session("conn"), CType(Me.ViewState("Verifica"), clsVerificaSegnalazione).IdAttivitaEnteSedeAttuazione)
                    Me.dgRisultatoRicerca.DataBind()

                    'Me.txtLetteraNProt.Enabled = True
                    'Me.txtLetteraDataProt.Enabled = True
                    'Me.txtRispostaNProt.Enabled = True
                    'Me.txtRispostaDataProt.Enabled = True
                    '*** mod. da simona cordella il 11/10/2010
                    '****rendo i controlli invisibili e non più enabled
                    Me.cmdSelProtocollo1.Visible = False
                    Me.cmdSelProtocollo2.Visible = False
                    Me.cmdAllegati1.Visible = False
                    Me.cmdAllegati2.Visible = False
                    '****
                    If Request.QueryString("IdRegCompetenza") = "-1" Then
                        Me.ddlCompetenza.SelectedValue = 22
                    Else
                        Me.ddlCompetenza.SelectedValue = Request.QueryString("IdRegCompetenza")
                    End If
                Catch ex As Exception
                    Response.Write(ex.Message)
                End Try

            End If
            'CmdLetteraInterlocutoria.Visible = False

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
                ddlCompetenza.SelectedIndex = 0
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
    Private Sub ControlloRegioneCompetenza(ByVal IdRegioneCompetenza As Integer)
        '*** Creata il 15/11/2007 da Simona Cordella
        '*** Se competenza regionale <> 22 (Nazionale) disabiliti campi della protocollazione

        If IdRegioneCompetenza <> 22 Then
            txtNumeroFascicolo.Text = ""
            txtDescFasc.Text = ""
            txtNumeroProtocollo.Text = ""
            txtDataProtocollo.Text = ""

            'txtNumeroFascicolo.ReadOnly = False
            'txtDescFasc.ReadOnly = False
            'txtNumeroProtocollo.ReadOnly = False
            'txtDataProtocollo.ReadOnly = False
            cmdSelFascicolo.Visible = False
            cmdSelProtocollo0.Visible = False
            cmdFascCanc.Visible = False
            cmdSelProtocollo.Visible = False
            cmdAllegati.Visible = False
            'txtLetteraNProt.ReadOnly = False
            ' txtLetteraDataProt.ReadOnly = False
            cmdSelProtocollo1.Visible = False
            cmdAllegati1.Visible = False
            'txtRispostaNProt.Enabled = True
            'txtRispostaDataProt.Enabled = True
            cmdSelProtocollo2.Visible = False
            cmdAllegati2.Visible = False
            '' ImgDataProtocollo.Attributes.Add("style", "visibility: visible")
            ' txtDataProtocollo.ReadOnly = False
        Else
            txtNumeroFascicolo.Text = ""
            txtDescFasc.Text = ""
            txtNumeroProtocollo.Text = ""
            txtDataProtocollo.Text = ""

            'txtNumeroFascicolo.ReadOnly = True
            'txtDescFasc.ReadOnly = True
            'txtNumeroProtocollo.ReadOnly = True
            'txtDataProtocollo.ReadOnly = True
            cmdSelFascicolo.Visible = True
            cmdSelProtocollo0.Visible = True
            cmdFascCanc.Visible = True
            cmdSelProtocollo.Visible = True
            cmdAllegati.Visible = True
            'txtLetteraNProt.ReadOnly = True
            'txtLetteraDataProt.ReadOnly = True
            'mod. da sc il 11/10/2010 in insermento non sono visibili
            'cmdSelProtocollo1.Visible = True
            'cmdAllegati1.Visible = True
            'txtRispostaNProt.ReadOnly = True
            'txtRispostaDataProt.ReadOnly = True
            'mod. da sc il 11/10/2010 in insermento non sono visibili
            'cmdSelProtocollo2.Visible = True
            'cmdAllegati2.Visible = True

            ''ImgDataProtocollo.Attributes.Add("style", "visibility: hidden")
            ' txtDataProtocollo.ReadOnly = True
        End If
    End Sub

    Private Sub ddlCompetenza_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlCompetenza.SelectedIndexChanged
        If ddlCompetenza.SelectedValue <> "" Then
            ControlloRegioneCompetenza(ddlCompetenza.SelectedValue)
        End If
    End Sub

    Private Sub lBttRicercaAttivitaEnteSedeAttuazione_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lBttRicercaAttivitaEnteSedeAttuazione.Click      
        Me.CaricaVerificatoreDaForm()
        Server.Transfer("WfrmRicPrgVerifica.aspx")
        'Response.Redirect("WfrmRicPrgVerifica.aspx")
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
                lblmessaggiosopra.Text = lblmessaggiosopra.Text + "Errore nel formato delle Date.<br/>"
                campiValidi = False
            End Try
            If (d1 > d2) Then
                lblmessaggiosopra.Text = lblmessaggiosopra.Text + "La Data Protocollo di Risposta deve essere maggiore alla Data Protocollo.<br/>"
                campiValidi = False
            End If
        End If

        Return campiValidi

    End Function


    Private Sub cmdConferma_Click(sender As Object, e As EventArgs) Handles cmdConferma.Click
        Dim Ver As clsVerificaSegnalazione
        Dim strSql As String
        Dim dtrIdSeg As SqlClient.SqlDataReader
        Dim intIdSegnalazione As Integer

        If (VerificaConferma()) Then
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


            Ver.IdRegCompetenza = ddlCompetenza.SelectedValue
            Ver.IdStatoVerifica = 1


            If Ver.Inserisci(Session("conn"), Me.ViewState("idd")) Then
                ' clsGui.SvuotaCampi(Me)
                Me.lblmessaggiosopra.Text = "INSERIMENTO EFFETTUATO."
                Me.lblmessaggiosopra.Visible = True
                Me.dgRisultatoRicerca.DataSource = Nothing
                Me.dgRisultatoRicerca.Visible = False
                'CmdLetteraInterlocutoria.Visible = True
                Me.IDD = ""
                Me.ddlCompetenza.SelectedValue = Ver.IdRegCompetenza

                'RELOAD ALLA MASCHERA DI MODIFICA DELLA SEGNALAZIONI

                'strSql = "Select SCOPE_IDENTITY() as maxid from TVerificheSegnalazione"
                strSql = "Select @@identity as IdSeg  from TVerificheSegnalazione"
                dtrIdSeg = ClsServer.CreaDatareader(strSql, Session("conn"))
                dtrIdSeg.Read()
                If dtrIdSeg.HasRows = True Then
                    intIdSegnalazione = dtrIdSeg("IdSeg")
                End If

                dtrIdSeg.Close()
                dtrIdSeg = Nothing

                Response.Redirect("WfrmGestioneVerModSegnalazione.aspx?idsegnalazione=" & intIdSegnalazione & "", False)
            Else
                Me.lblErrore.Text = "ATTENZIONE INSERIMENTO FALLITO."
                Me.lblErrore.Visible = True
                ''  Me.Imgerrore.Visible = True
            End If

            'If Not Request.QueryString("IDAESA") Is Nothing Then CmdLetteraInterlocutoria.Visible = True
        Else

            Me.lblErrore.Visible = True
        End If

    End Sub

    Private Sub imgChiudi_Click(sender As Object, e As EventArgs) Handles imgChiudi.Click
        Response.Redirect("WfrmMain.aspx")
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

    Protected Sub CmdLetteraInterlocutoria_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles CmdLetteraInterlocutoria.Click

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
