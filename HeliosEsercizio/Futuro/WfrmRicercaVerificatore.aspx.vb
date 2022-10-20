Public Class WfrmRicercaVerificatore
    Inherits System.Web.UI.Page

#Region " Codice generato da Progettazione Web Form "

    'Chiamata richiesta da Progettazione Web Form.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    'Protected WithEvents lblTitolo As System.Web.UI.WebControls.Label
    'Protected WithEvents txtCognome As System.Web.UI.WebControls.TextBox
    'Protected WithEvents txtNome As System.Web.UI.WebControls.TextBox
    'Protected WithEvents cmdRicerca As System.Web.UI.WebControls.Button
    'Protected WithEvents cmdChiudi As System.Web.UI.WebControls.Button
    'Protected WithEvents ddCircoscrizione As System.Web.UI.WebControls.DropDownList
    'Protected WithEvents cmbAbilitato As System.Web.UI.WebControls.DropDownList
    'Protected WithEvents cmbTipologia As System.Web.UI.WebControls.DropDownList
    'Protected WithEvents cmbVerificatoreInterno As System.Web.UI.WebControls.DropDownList
    'Protected WithEvents dgRicerca As System.Web.UI.WebControls.DataGrid
    'Protected WithEvents Form1 As System.Web.UI.HtmlControls.HtmlForm
    'Protected WithEvents ddlCompetenza As System.Web.UI.WebControls.DropDownList

    'NOTA: la seguente dichiarazione è richiesta da Progettazione Web Form.
    'Non spostarla o rimuoverla.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: questa chiamata al metodo è richiesta da Progettazione Web Form.
        'Non modificarla nell'editor del codice.
        InitializeComponent()
        Dim SqlCmd As SqlClient.SqlCommand
        SqlCmd = New SqlClient.SqlCommand
        SqlCmd.Connection = Session("conn")
        SqlCmd.CommandText = "SELECT IdVerificatore,Cognome,Nome From Tverificatori Where Tipologia=0"
        Dim Dr As SqlClient.SqlDataReader
        Dr = SqlCmd.ExecuteReader
        Dim I As Integer = 1
        Me.cmbVerificatoreInterno.Items.Clear()
        Me.cmbVerificatoreInterno.Items.Add("")
        Me.cmbVerificatoreInterno.Items(0).Value = ""
        While Dr.Read
            Me.cmbVerificatoreInterno.Items.Add(Dr("Cognome") + " " + Dr("Nome"))
            Me.cmbVerificatoreInterno.Items(I).Value = Dr("IdVerificatore")
            I += 1
        End While
        Dr.Close()

    End Sub

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Inserire qui il codice utente necessario per inizializzare la pagina
        If Not Me.IsPostBack Then
            CaricaCompetenze()
            clsGui.CaricaDropDown(Me.ddCircoscrizione, clsVerificatore.RecuperaCircoscrizioni(Session("conn"), Session("IdRegCompetenza")), "Circoscrizione", "IdCircoscrizione")
            If ddlCompetenza.SelectedValue <> 22 Then
                Me.cmbTipologia.SelectedIndex = 1
                Me.cmbTipologia.Enabled = False
            End If
        End If
    End Sub

    Private Sub cmdRicerca_Click(sender As Object, e As EventArgs) Handles cmdRicerca.Click
        Me.dgRicerca.CurrentPageIndex = 0
        Me.dgRicerca.DataSource = clsVerificatore.Ricerca(Session("conn"), Replace(Me.txtCognome.Text, "'", "''"), Replace(Me.txtNome.Text, "'", "''"), Me.ddCircoscrizione.SelectedValue, Me.cmbVerificatoreInterno.SelectedValue, Me.cmbTipologia.SelectedValue, Me.cmbAbilitato.SelectedValue, Me.ddlCompetenza.SelectedValue)
        Me.dgRicerca.DataBind()
    End Sub

    Private Sub dgRicerca_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dgRicerca.SelectedIndexChanged
        Response.Redirect("WfrmGestioneVerificatori.aspx?IdVerificatore=" & Me.dgRicerca.SelectedItem.Cells(1).Text & "&IdVerificatoreInterno=" & Me.dgRicerca.SelectedItem.Cells(7).Text & "&IDREGCOMPETENZA=" & Me.dgRicerca.SelectedItem.Cells(8).Text)
    End Sub

    Private Sub dgRicerca_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgRicerca.PageIndexChanged
        Me.dgRicerca.DataSource = clsVerificatore.Ricerca(Session("conn"), Replace(Me.txtCognome.Text, "'", "''"), Replace(Me.txtNome.Text, "'", "''"), Me.ddCircoscrizione.SelectedValue, Me.cmbVerificatoreInterno.SelectedValue, Me.cmbTipologia.SelectedValue, Me.cmbAbilitato.SelectedValue, Me.ddlCompetenza.SelectedValue)
        Me.dgRicerca.SelectedIndex = -1
        Me.dgRicerca.EditItemIndex = -1
        Me.dgRicerca.CurrentPageIndex = e.NewPageIndex
        Me.dgRicerca.DataBind()
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

    Private Sub cmdChiudi_Click(sender As Object, e As EventArgs) Handles cmdChiudi.Click
        Response.Redirect("WfrmMain.aspx")
    End Sub

   
End Class