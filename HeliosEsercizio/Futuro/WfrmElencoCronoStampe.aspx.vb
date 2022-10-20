Public Class WfrmElencoCronoStampe
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Session("LogIn") Is Nothing Then
            If Session("LogIn") = False Then 'verifico validità log-in
                Response.Redirect("LogOn.aspx")
            End If
        Else
            Response.Redirect("LogOn.aspx")
        End If

        Call ricerca()
    End Sub

    Sub ricerca()
        '***Eseguo comando di ricerca
        Dim Mydataset As New DataSet
        Dim strquery As String

        dgRisultatoRicerca.CurrentPageIndex = 0

        strquery = "SELECT ('Rif. ' + CONVERT(varchar,QuestionarioCronologiaStampe.IdCronologiaStampa)) as IdCronologiaStampa, attività.Titolo, QuestionarioCronologiaStampe.DataStampa, " _
                 & " QuestionarioCronologiaStampe.Username " _
                 & " FROM QuestionarioCronologiaStampe INNER JOIN " _
                 & " QuestionarioStampaProgetti ON QuestionarioCronologiaStampe.IdCronologiaStampa = QuestionarioStampaProgetti.IdCronologiaStampa INNER JOIN " _
                 & " attività ON QuestionarioStampaProgetti.IDAttività = attività.IDAttività " _
                 & " WHERE (QuestionarioStampaProgetti.IDAttività = " & Request.QueryString("IdAttivita") & " ) " _
                 & " ORDER BY QuestionarioCronologiaStampe.DataStampa DESC"

        Mydataset = ClsServer.DataSetGenerico(strquery, Session("conn"))
        dgRisultatoRicerca.DataSource = Mydataset
        dgRisultatoRicerca.DataBind() 'valorizzo griglia

        Session("LocalDataSet") = Mydataset

    End Sub

    Private Sub dgRisultatoRicerca_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgRisultatoRicerca.PageIndexChanged
        'passo il nuovo indice selezionato all'indice della pagina da visualizzare
        dgRisultatoRicerca.CurrentPageIndex = e.NewPageIndex
        'riassegno il dataset dichiarato volutamente pubblico a tutta la pagina
        dgRisultatoRicerca.DataSource = Session("LocalDataSet")
        dgRisultatoRicerca.DataBind()
    End Sub

    Private Sub cmdChiudi_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdChiudi.Click
        Response.Write("<script>" & vbCrLf)
        Response.Write("window.close()" & vbCrLf)
        Response.Write("</script>")
    End Sub

End Class