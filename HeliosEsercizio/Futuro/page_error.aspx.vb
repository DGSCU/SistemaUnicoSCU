Public Class page_error
    Inherits SmartPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Log.Error(1, exception:=Session("LastError"))
        Exit Sub
        If Not Session("LogIn") Is Nothing Then
            If Session("LogIn") = False Then 'verifico validità log-in
                If Session("Read") = "1" Then
                    Response.Redirect("LogOnRead.aspx")
                Else
                    Response.Redirect("LogOn.aspx")
                End If

            End If
        Else
            Response.Redirect("LogOn.aspx")
        End If

        If Session("Read") = "1" Then
            Hyllogon.NavigateUrl = "~/LogOnRead.aspx"
        End If


    End Sub

End Class