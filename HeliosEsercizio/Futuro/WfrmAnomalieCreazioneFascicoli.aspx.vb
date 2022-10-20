Public Class WfrmAnomalieCreazioneFascicoli
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Page.IsPostBack = False Then
            CaricaAnomalìe()
        End If
    End Sub

    Sub CaricaAnomalìe()
        
        lblElenco.Text = Session("arrayErrori")(0)
    End Sub

    Protected Sub CmdChiudi_Click(sender As Object, e As EventArgs) Handles CmdChiudi.Click
        Response.Write("<script>" & vbCrLf)
        Response.Write("window.close();" & vbCrLf)
        Response.Write("</script>")
    End Sub
End Class