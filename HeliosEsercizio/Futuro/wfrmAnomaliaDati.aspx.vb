Public Class wfrmAnomaliaDati
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Session.Clear()
    End Sub

    Protected Sub imgChiudi_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles imgChiudi.Click
        Response.Redirect("LogOn.aspx")
    End Sub
End Class