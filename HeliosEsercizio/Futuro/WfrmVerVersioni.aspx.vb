Public Class WfrmVerVersioni
    Inherits System.Web.UI.Page
    '*** CREATA DA SIMONA CORDELLA IL 17/02/2012 ****
    '*** ELENCO DELLE VERSIONI DEI REQUISTI DELLE VERIFICHE
#Region " Codice generato da Progettazione Web Form "

    'Chiamata richiesta da Progettazione Web Form.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    'Protected WithEvents dtgElencoVersioni As System.Web.UI.WebControls.DataGrid
    'Protected WithEvents cmdChiudi As System.Web.UI.WebControls.ImageButton

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
        If Not Session("LogIn") Is Nothing Then
            If Session("LogIn") = False Then 'verifico validità log-in
                Response.Redirect("LogOn.aspx")
            End If
        Else
            Response.Redirect("LogOn.aspx")
        End If
        If Page.IsPostBack = False Then
            LoadElencoVersioni()
        End If
    End Sub

    Sub LoadElencoVersioni()
        Dim strSql As String
        Dim dtsElencoVer As DataSet

        strSql = " SELECT IDVersioneVerifiche, Stato as id,Descrizione, " & _
                 " Case Stato when 0 then 'Registrata' else 'Associata' end as Stato " & _
                 " FROM TVerificheVersioni "
        dtsElencoVer = ClsServer.DataSetGenerico(strSql, Session("conn"))

        dtgElencoVersioni.DataSource = dtsElencoVer
        dtgElencoVersioni.DataBind()
        Session("SessiondtsElencoVer") = dtsElencoVer
    End Sub

    Private Sub dtgElencoVersioni_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dtgElencoVersioni.ItemCommand
        Try
            Select Case e.CommandName
                Case "Requisiti"
                    Response.Redirect("WfrmRicercaRequisitiVerifiche.aspx?IDVersioneVerifiche=" + e.Item.Cells(1).Text + "&StatoVersione=" + e.Item.Cells(2).Text)
                Case "Bando"
                    Response.Redirect("WfrmVerGestioneBandiRequisiti.aspx?IDVersioneVerifiche=" + e.Item.Cells(1).Text)
            End Select
        Catch ex As Exception
            Response.Write(ex.Message.ToString())
        End Try
    End Sub

    Private Sub cmdChiudi_Click(sender As Object, e As EventArgs) Handles cmdChiudi.Click
        Response.Redirect("WfrmMain.aspx")
    End Sub

    Private Sub dtgElencoVersioni_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dtgElencoVersioni.PageIndexChanged
        dtgElencoVersioni.CurrentPageIndex = e.NewPageIndex
        dtgElencoVersioni.DataSource = Session("SessiondtsElencoVer")
        dtgElencoVersioni.DataBind()
        dtgElencoVersioni.SelectedIndex = -1
    End Sub

    Private Sub dtgElencoVersioni_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dtgElencoVersioni.SelectedIndexChanged

    End Sub

    
End Class