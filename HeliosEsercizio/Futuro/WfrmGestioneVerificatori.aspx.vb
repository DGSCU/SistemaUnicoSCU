Public Class WfrmGestioneVerificatori
    Inherits System.Web.UI.Page

#Region " Codice generato da Progettazione Web Form "

    'Chiamata richiesta da Progettazione Web Form.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents Imgerrore As System.Web.UI.WebControls.Image
    Protected WithEvents lblmessaggiosopra As System.Web.UI.WebControls.Label
    Protected WithEvents ddCircoscrizione As System.Web.UI.WebControls.DropDownList
    Protected WithEvents cmdConferma As System.Web.UI.WebControls.Button
    Protected WithEvents imgChiudi As System.Web.UI.WebControls.Button
    Protected WithEvents txtCognome As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtNome As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtRiferimento As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtEmail As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtTelCell As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtTelInterno As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtNote As System.Web.UI.WebControls.TextBox
    Protected WithEvents ddTipologia As System.Web.UI.WebControls.DropDownList
    Protected WithEvents ddVerificatoreInterno As System.Web.UI.WebControls.DropDownList
    Protected WithEvents frmMain As System.Web.UI.HtmlControls.HtmlForm
    Protected WithEvents ddlCompetenza As System.Web.UI.WebControls.DropDownList
    Protected WithEvents ddAbilitato As System.Web.UI.WebControls.DropDownList
    Protected WithEvents ddlUsername As System.Web.UI.WebControls.DropDownList
    Protected WithEvents lblIntestazione As System.Web.UI.WebControls.Label

    'NOTA: la seguente dichiarazione è richiesta da Progettazione Web Form.
    'Non spostarla o rimuoverla.
    Private designerPlaceholderDeclaration As System.Object



    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: questa chiamata al metodo è richiesta da Progettazione Web Form.
        'Non modificarla nell'editor del codice.
        InitializeComponent()

    End Sub

#End Region


    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Inserire qui il codice utente necessario per inizializzare la pagina

        If Not Me.Page.IsPostBack Then
            CaricaCompetenze()
            If Request.QueryString("IdVerificatore") <> "" Then
                If Request.QueryString("IDREGCOMPETENZA") <> 22 Then
                    ddlCompetenza.SelectedValue = Request.QueryString("IDREGCOMPETENZA")
                End If
            End If

            CaricaUsername(ddlCompetenza.SelectedValue)
            clsGui.CaricaDropDown(Me.ddVerificatoreInterno, clsVerificatore.RecuperaVerificatoriInterni(Session("conn"), ddlCompetenza.SelectedValue), "NomeCompleto", "IdVerificatore")
            clsGui.CaricaDropDown(Me.ddCircoscrizione, clsVerificatore.RecuperaCircoscrizioni(Session("conn"), ddlCompetenza.SelectedValue), "Circoscrizione", "IdCircoscrizione")
            Me.ddCircoscrizione.SelectedIndex = 1
            Me.ddCircoscrizione.Enabled = False
            'If Session("TipoUtente") = "U" Then
            '    Me.ddCircoscrizione.Enabled = True
            '    If ddlCompetenza.SelectedValue <> 22 Then
            '        Me.ddCircoscrizione.SelectedIndex = 1
            '        Me.ddCircoscrizione.Enabled = False
            '    End If
            'Else
            '    Me.ddCircoscrizione.SelectedIndex = 1
            '    Me.ddCircoscrizione.Enabled = False
            'End If
            Me.ddVerificatoreInterno.Enabled = False
            If ddlCompetenza.SelectedValue <> 22 Then
                Me.ddTipologia.SelectedIndex = 1
                Me.ddTipologia.Enabled = False
            End If

            If Request.QueryString("IdVerificatore") = "" Then
                lblIntestazione.Text = "Inserimento Verificatore"
                Me.ddTipologia.SelectedValue = 0
                If Me.ddTipologia.SelectedItem.Text = "Interno" Then 'interno
                    Me.ddlUsername.Enabled = True
                Else 'igf
                    Me.ddlUsername.Enabled = False
                End If
            Else
                lblIntestazione.Text = "Modifica Verificatore"
                clsGui.EliminaItemDropDrown(Me.ddVerificatoreInterno, Request.QueryString("IdVerificatore"))
                Dim Verificatore As clsVerificatore = clsVerificatore.RecuperaVerificatore(Request.QueryString("IdVerificatore"), Session("conn"), Request.QueryString("IdVerificatoreInterno"))
                Dim par As String
                Me.txtCognome.Text = Verificatore.Cognome
                Me.txtNome.Text = Verificatore.Nome
                Me.ddCircoscrizione.SelectedValue = Verificatore.IdCircoscrizione
                Me.ddlCompetenza.SelectedValue = Verificatore.IdRegCompetenza
                Me.ddlUsername.SelectedValue = Verificatore.IdUser
                'Modificato Antonello Di Croce 02/10/2007 Gestione \par
                'par = Replace(Verificatore.Riferimento, Chr(10), "\par")
                par = Replace(Verificatore.Riferimento, "\par", Chr(10))
                'Me.txtRiferimento.Text = Verificatore.Riferimento
                Me.txtRiferimento.Text = par


                Me.txtEmail.Text = Verificatore.Email
                Me.ddAbilitato.SelectedValue = Verificatore.Abilitato
                Me.ddTipologia.SelectedValue = Verificatore.Tipologia
                Me.txtNote.Text = Verificatore.Note
                Me.txtTelInterno.Text = Verificatore.TelInterno
                Me.txtTelCell.Text = Verificatore.TelCell
                Me.ddVerificatoreInterno.SelectedValue = clsConversione.DaZeroInStringa(Verificatore.VerificatoreInterno)

                If Me.ddTipologia.SelectedValue = 0 Then 'interno
                    Me.ddlUsername.Enabled = True
                Else 'igf
                    Me.ddlUsername.Enabled = False
                End If

                If Verificatore.Tipologia = 1 Then Me.ddVerificatoreInterno.Enabled = True
                Me.ViewState.Add("Verificatore", Verificatore)

            End If
            lblIntestazione1.Text = lblIntestazione.Text
        End If

    End Sub

    Private Sub MostraMessaggioErrore(ByVal Testo As String)
        Me.lblmessaggiosopra.Text = Testo
        Me.lblmessaggiosopra.Visible = True
        'Me.Imgerrore.Visible = True
    End Sub
    Private Sub NascondiMessaggioErrore()
        Me.lblmessaggiosopra.Visible = False
        'Me.Imgerrore.Visible = False
    End Sub

    Private Function VerificaConferma(ByRef Ver As clsVerificatore) As Boolean

        Dim Tipologia_Old As Integer
        Dim IdVerInterno_Old As Integer
        Dim par As String
        Ver = Me.ViewState.Item("Verificatore")
        If Ver Is Nothing Then
            Ver = New clsVerificatore
        End If
        Ver.Cognome = Me.txtCognome.Text
        Ver.Nome = Me.txtNome.Text

        Dim campiValidi As Boolean = True
        Dim campoObbligatorio As String = "Il campo {0} è obbligatorio.<br/>"


        lblErrore.Text = ""

        If (Ver.Cognome = String.Empty) Then
            lblErrore.Text = lblErrore.Text + String.Format(campoObbligatorio, "Cognome")
            campiValidi = False
        End If

        If (Ver.Nome = String.Empty) Then
            lblErrore.Text = lblErrore.Text + String.Format(campoObbligatorio, "Nome")
            campiValidi = False
        End If

        'If Me.ddCircoscrizione.SelectedValue = String.Empty Then
        '    lblErrore.Text = lblErrore.Text + String.Format(campoObbligatorio, "Circoscrizione")
        '    campiValidi = False
        'End If
        If Me.ddCircoscrizione.SelectedValue = String.Empty Then
            lblErrore.Text = lblErrore.Text + String.Format(campoObbligatorio, "Circoscrizione")
            campiValidi = False
        Else
            Ver.IdCircoscrizione = Me.ddCircoscrizione.SelectedValue
        End If
        If Me.ddTipologia.SelectedValue = String.Empty Then
            lblErrore.Text = lblErrore.Text + String.Format(campoObbligatorio, "Tipologia")
            campiValidi = False
        Else
            'interno
            If Me.ddTipologia.SelectedValue = "0" And Me.ddlUsername.SelectedValue = "0" Then
                lblErrore.Text = lblErrore.Text + String.Format(campoObbligatorio, "Username")
                campiValidi = False
            Else
                'Ver.VerificatoreInterno = clsConversione.DaStringaInNothing(Me.ddlUsername.SelectedValue)
            End If
            'igf
            If Me.ddTipologia.SelectedValue = "1" And Me.ddVerificatoreInterno.SelectedValue = "" Then
                lblErrore.Text = lblErrore.Text + String.Format(campoObbligatorio, "Verificatore Interno")
                campiValidi = False
            Else
                Ver.VerificatoreInterno = clsConversione.DaStringaInNothing(Me.ddVerificatoreInterno.SelectedValue)
            End If
            Ver.Tipologia = Me.ddTipologia.SelectedValue
        End If
        Return campiValidi
    End Function




    Private Sub Salvataggio()
        Dim Ver As clsVerificatore
        Dim Tipologia_Old As Integer
        Dim IdVerInterno_Old As Integer
        Dim par As String
        Ver = Me.ViewState.Item("Verificatore")
        If Ver Is Nothing Then
            Ver = New clsVerificatore
        End If


        If VerificaConferma(Ver) = True Then
            lblErrore.Visible = False

        Else
            lblErrore.Visible = True
            Exit Sub
        End If
    

        '----------------------------------------------------------
        Ver.Note = Me.txtNote.Text
        Ver.Email = Me.txtEmail.Text
        Ver.TelCell = Me.txtTelCell.Text
        Ver.TelInterno = Me.txtTelInterno.Text
        Ver.Abilitato = Me.ddAbilitato.SelectedValue
        Ver.IdRegCompetenza = Me.ddlCompetenza.SelectedValue
        Ver.IdUser = Me.ddlUsername.SelectedValue

        Ver.UserUltimaModifica = Session("UTENTE")
        'Modificato Antonello Di Croce 02/10/2007 Gestione \par
        par = Replace(Me.txtRiferimento.Text, Chr(10), "\par")
        Ver.Riferimento = par
        'Ver.Riferimento = Me.txtRiferimento.Text

        If Ver.IdVerificatore > 0 Then
            If Ver.Modifica(Session("conn")) Then
                'clsGui.SvuotaCampi(Me)
                Me.lblmessaggiosopra.Text = "MODIFICA EFFETTUATA."
                Me.lblmessaggiosopra.Visible = True
                'Me.ViewState.Remove("Verificatore")
                Me.ddlCompetenza.SelectedValue = Ver.IdRegCompetenza
                Me.ddTipologia.SelectedValue = Ver.Tipologia
            End If
        Else
            Ver.UserInseritore = Session("UTENTE")
            If Ver.Inserisci(Session("conn")) Then
                clsGui.SvuotaCampi(Me)
                Me.lblmessaggiosopra.Text = "INSERIMENTO EFFETTUATO."
                Me.lblmessaggiosopra.Visible = True
                Me.ddlCompetenza.SelectedValue = Ver.IdRegCompetenza
                Me.ddTipologia.SelectedValue = Ver.Tipologia
            End If
        End If
    End Sub
    Private Sub cmdConferma_Click(sender As Object, e As EventArgs) Handles cmdConferma.Click
        Salvataggio()


    End Sub

    Private Sub ddTipologia_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddTipologia.SelectedIndexChanged
        If Me.ddTipologia.SelectedValue = "1" Then 'igf
            Me.ddVerificatoreInterno.Enabled = True
            Me.ddlUsername.SelectedValue = 0
            Me.ddlUsername.Enabled = False
        Else 'interno
            Me.ddVerificatoreInterno.SelectedValue = ""
            Me.ddlUsername.Enabled = True
            Me.ddVerificatoreInterno.Enabled = False
        End If

    End Sub

    Private Sub imgChiudi_Click(sender As Object, e As EventArgs) Handles imgChiudi.Click
        'If Request.QueryString("IdVerificatore") = "" Then
        'Response.Redirect("WfrmMain.aspx")
        'Else
        Response.Redirect("WfrmRicercaVerificatore.aspx")
        'End If
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
                'chiudo il datareader se aperto
                If Not dtrCompetenze Is Nothing Then
                    dtrCompetenze.Close()
                    dtrCompetenze = Nothing
                End If
            End If

            'Controllo abilitazione scelta
            If Session("TipoUtente") = "U" Then
                ddlCompetenza.Enabled = True
                ddlCompetenza.SelectedIndex = 0

            Else

                'CboCompetenza.SelectedIndex = 1
                'CboCompetenza.Enabled = False
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
                End If

                If Session("TipoUtente") = "R" Then
                    ddlCompetenza.Enabled = False
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

    Private Sub ddlCompetenza_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlCompetenza.SelectedIndexChanged
        clsGui.CaricaDropDown(Me.ddCircoscrizione, clsVerificatore.RecuperaCircoscrizioni(Session("conn"), ddlCompetenza.SelectedValue), "Circoscrizione", "IdCircoscrizione")
        If ddlCompetenza.SelectedValue <> 22 Then
            Me.ddTipologia.SelectedIndex = 1
            Me.ddTipologia.Enabled = False
            Me.ddVerificatoreInterno.SelectedIndex = 0
            Me.ddVerificatoreInterno.Enabled = False
            Me.ddCircoscrizione.SelectedIndex = 1
            Me.ddCircoscrizione.Enabled = False
        Else
            Me.ddTipologia.SelectedIndex = 0
            Me.ddTipologia.Enabled = True
            Me.ddVerificatoreInterno.Enabled = True
            Me.ddCircoscrizione.SelectedIndex = 0
            Me.ddCircoscrizione.Enabled = True
        End If
    End Sub
    Private Sub CaricaUsername(ByVal IdRegCompetenza As Integer)
        Dim strsql As String
        Dim dtrUser As SqlClient.SqlDataReader
        If Not dtrUser Is Nothing Then
            dtrUser.Close()
            dtrUser = Nothing
        End If
        strsql = " SELECT UserName, IdUtente " & _
                 " FROM  UtentiUNSC  " & _
                 " where IdRegioneCompetenza= " & IdRegCompetenza & " and abilitato=1 and HeliosRead=0 " & _
                 " Union " & _
                 " Select '','' from UtentiUNSC " & _
                 " where IdRegioneCompetenza= " & IdRegCompetenza & " and abilitato=1 and HeliosRead=0" & _
                 " Order by UserName"
        dtrUser = ClsServer.CreaDatareader(strsql, Session("conn"))

        ddlUsername.DataSource = dtrUser
        ddlUsername.Items.Add("")
        ddlUsername.DataTextField = "UserName"
        ddlUsername.DataValueField = "IdUtente"
        ddlUsername.DataBind()

        If Not dtrUser Is Nothing Then
            dtrUser.Close()
            dtrUser = Nothing
        End If
    End Sub

    Private Sub ddlUsername_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlUsername.SelectedIndexChanged

    End Sub



End Class