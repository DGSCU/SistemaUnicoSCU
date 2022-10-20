Public Class cronologiaMailEnti
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Inserire qui il codice utente necessario per inizializzare la pagina
        If Not Session("LogIn") Is Nothing Then
            If Session("LogIn") = False Then
                Response.Redirect("LogOn.aspx")
            End If
        Else
            Response.Redirect("LogOn.aspx")
        End If

        If Page.IsPostBack = False Then
            cronologiaMailEnti()
        End If
    End Sub

    Sub CronologiaMailEnti()

        Dim strSql As String
        Dim Mydataset As DataSet

        strSql = " Select  VecchiaEmail, VecchiaPEC, NuovaEmail, NuovaPEC, Username, dbo.FormatoData(DataModifica)as DataModifica " & _
                 " from CronologiaMailEnti Where idente=" & Session("IdEnte") & ""
        Mydataset = ClsServer.DataSetGenerico(strSql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))

        dtgCronologia.DataSource = Mydataset
        dtgCronologia.DataBind()
        Session("LocalDataSet") = Mydataset
    End Sub

    Private Sub dtgCronologia_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dtgCronologia.PageIndexChanged
        dtgCronologia.CurrentPageIndex = e.NewPageIndex
        'riassegno il dataset dichiarato volutamente pubblico a tutta la pagina
        dtgCronologia.DataSource = Session("LocalDataSet")
        dtgCronologia.DataBind()
    End Sub

    Private Sub cmdChiudi_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdChiudi.Click
        Response.Write("<script>" & vbCrLf)
        Response.Write("window.close()" & vbCrLf)
        Response.Write("</script>")
    End Sub

End Class