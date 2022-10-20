Public Class WfrmPaginaDiStampaPassword
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'controllo se è stato effettuato il login
        If Not Session("LogIn") Is Nothing Then
            'se non è stato effettuato login
            If Session("LogIn") = False Then
                'carico la pagina LogOut dove svuoto eventuali session aperte
                Response.Redirect("LogOn.aspx")
            End If
        Else
            'carico la pagina LogOut dove svuoto eventuali session aperte
            Response.Redirect("LogOn.aspx")
        End If

        LblUser.Text = Request.QueryString("USER")
        LblPassword.Text = Request.QueryString("PASSWORD")

    End Sub


    Private Sub imgStampa_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles imgStampa.Click
        imgStampa.Visible = False
        'lblstampacredenziali.Visible = False
        ''With Me.PrintDialog
        ''    If .ShowDialog = Windows.Forms.DialogResult.OK Then
        ''        .Document.Print()
        ''    End If
        ''End With
        Session("CreazioneOK") = Nothing
        Response.Write("<script>" & vbCrLf)
        Response.Write("window.print()" & vbCrLf)
        Response.Write("</script>")
    End Sub
End Class