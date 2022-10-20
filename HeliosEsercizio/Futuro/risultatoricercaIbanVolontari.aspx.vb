Public Class risultatoricercaIbanVolontari
    Inherits System.Web.UI.Page

#Region " Codice generato da Progettazione Web Form "

    'Chiamata richiesta da Progettazione Web Form.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents dgtElencoVolontari As System.Web.UI.WebControls.DataGrid

    'NOTA: la seguente dichiarazione è richiesta da Progettazione Web Form.
    'Non spostarla o rimuoverla.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        InitializeComponent()
    End Sub

#End Region
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Not Session("LogIn") Is Nothing Then
            If Session("LogIn") = False Then
                Response.Redirect("LogOn.aspx")
            End If
        Else
            Response.Redirect("LogOn.aspx")
        End If
        If IsPostBack = False Then

            Dim intChkIban As Integer = CheckExistIban(Request.QueryString("CodiceIban"), Request.QueryString("CodiceFiscale"))
            If intChkIban = 0 Then
                Response.Write("<script type=""text/javascript"">" & vbCrLf)
                Response.Write("window.close()" & vbCrLf)
                Response.Write("</script>" & vbCrLf)
            End If
        End If
    End Sub
    Private Function CheckExistIban(ByVal strIban As String, ByVal strCodiceFiscale As String) As Integer
        Dim strSql As String
        Dim intCheckExist As Integer
        Dim dtsGenerico As New DataSet
        strSql = "Select Identità, codicevolontario,Cognome + ' ' + Nome As Nominativo,iban,bic_swift From Entità Where IBAN='" & strIban & "' and CodiceFiscale <> '" & strCodiceFiscale & "'"
        dtsGenerico = ClsServer.DataSetGenerico(strSql, Session("conn"))

        dgtElencoVolontari.DataSource = dtsGenerico
        dgtElencoVolontari.DataBind()
        If dgtElencoVolontari.Items.Count = 0 Then
            intCheckExist = 0
        Else
            intCheckExist = dgtElencoVolontari.Items.Count
        End If

        Return intCheckExist
    End Function
End Class
