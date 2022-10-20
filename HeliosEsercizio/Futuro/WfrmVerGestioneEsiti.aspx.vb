Imports System.Data.SqlClient
Public Class WfrmVerGestioneEsiti
    Inherits System.Web.UI.Page

#Region " Codice generato da Progettazione Web Form "

    'Chiamata richiesta da Progettazione Web Form.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    'Protected WithEvents cmdChiudi As System.Web.UI.WebControls.ImageButton
    'Protected WithEvents dtgElencoEsiti As System.Web.UI.WebControls.DataGrid
    'Protected WithEvents cmdNuovo As System.Web.UI.WebControls.ImageButton
    Protected WithEvents lblmsg As System.Web.UI.WebControls.Label

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

        End If
        LoadElencoEsiti()
    End Sub
    Sub LoadElencoEsiti()
        Dim strSql As String
        'Dim dtsElencoEsiti As DataSet

        strSql = " SELECT  IdEsito,Esito, " & _
                 " Case When Abilitato=1 then 'Abilitato' else 'Non Abilitato' end as stato " & _
                 " FROM TVerificheEsitoTemplate "
        Session("dtsElencoEsiti") = ClsServer.DataSetGenerico(strSql, Session("conn"))

        dtgElencoEsiti.DataSource = Session("dtsElencoEsiti")
        dtgElencoEsiti.DataBind()

    End Sub

    Private Sub cmdChiudi_Click(sender As Object, e As EventArgs) Handles cmdChiudi.Click
        Response.Redirect("WfrmMain.aspx")
    End Sub

    Private Sub dtgElencoEsiti_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dtgElencoEsiti.PageIndexChanged
        dtgElencoEsiti.CurrentPageIndex = e.NewPageIndex
        dtgElencoEsiti.DataSource = Session("dtsElencoEsiti")
        dtgElencoEsiti.DataBind()
        dtgElencoEsiti.SelectedIndex = -1
    End Sub

    Private Sub dtgElencoEsiti_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dtgElencoEsiti.ItemCommand
        Select Case e.CommandName
            Case "ModRequisiti"

            Case "Disabilita"
                If e.Item.Cells(3).Text() = "Abilitato" Then
                    UpdateTVericheEsitoTemplate(0, e.Item.Cells(1).Text)
                    LoadElencoEsiti()
                End If
        End Select
    End Sub

    Sub UpdateTVericheEsitoTemplate(ByVal Abilitato As Integer, ByVal IdEsito As Integer)
        Dim cmdUpd As SqlCommand
        Dim strSql As String
        Dim dtrPrendiId As SqlClient.SqlDataReader
        Dim IntIdUser As Integer
        Try
            strSql = " UPDATE TVerificheEsitoTemplate SET Abilitato =" & Abilitato & ",  "
            strSql &= "  UserDisabilitazione = '" & Session("Utente") & " ', DataDisabilitazione = getdate() "
            strSql &= " WHERE IdEsito = " & IdEsito
            cmdUpd = ClsServer.EseguiSqlClient(strSql, Session("conn"))

            lblmsg.Text = "Disabilitazione effettuata con successo."
        Catch ex As Exception
            lblmsg.Text = "Errore imprevisto.Contattare l'assistenza."
            'Response.Write(ex.Message.ToString())
        End Try
    End Sub

    Private Sub cmdNuovo_Click(sender As Object, e As EventArgs) Handles cmdNuovo.Click
        'WfrmVerInserimentoEsiti.aspx?VengoDa=Esiti
        Response.Redirect("WfrmVerInserimentoEsiti.aspx?VengoDa=Esiti")
    End Sub

    Private Sub dtgElencoEsiti_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dtgElencoEsiti.SelectedIndexChanged

    End Sub

   
   
End Class