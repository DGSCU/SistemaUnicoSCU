
Imports System.Data.SqlClient
Public Class WfrmCronologiaRiattivazione
    Inherits System.Web.UI.Page

    Dim strsql As String
    Dim dtrGenerico As SqlClient.SqlDataReader
    Dim dtsGenerico As DataSet
    Dim cmdGenerico As SqlClient.SqlCommand
    Dim volonatrio As String
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Page.IsPostBack = False Then
            Session("MyLocalDataSet") = Nothing

            If Request.QueryString("idattivitaSedeAssegnazione") <> "" Then
                CaricaGrigliaSede()
                cmdIndietro.Visible = False
                lblNominativo.Visible = False
            Else
                CaricaGriglia()
                CaricaGriglia1()
                lblNominativo.Text = lblNominativo.Text & Request.QueryString("Nominativo")
            End If


        End If
    End Sub
    'Private Sub dtgRisultatoRicerca_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dtgRisultatoRicerca.PageIndexChanged
    '    dtgRisultatoRicerca.CurrentPageIndex = e.NewPageIndex
    '    dtgRisultatoRicerca.DataSource = Session("appDtsRisRicerca")
    '    dtgRisultatoRicerca.DataBind()
    'End Sub

    Sub CaricaGriglia()

        Dim sqlDAP As New SqlClient.SqlDataAdapter
        Dim dataSet As New DataSet
        Dim strNomeStore As String = "[SP_RIATTIVAZIONE_CRONO_SEDE]"

        Try
            sqlDAP = New SqlClient.SqlDataAdapter(strNomeStore, CType(Session("conn"), SqlClient.SqlConnection))
            sqlDAP.SelectCommand.CommandType = CommandType.StoredProcedure

            sqlDAP.SelectCommand.Parameters.Add("@IDEntità", SqlDbType.Int).Value = Request.QueryString("IdVol")
            
            sqlDAP.Fill(dataSet)

            Session("appDtsRisRicerca") = dataSet
            dtgRisultatoRicerca.DataSource = dataSet
            dtgRisultatoRicerca.DataBind()

        Catch ex As Exception
            Response.Write(ex.Message.ToString())
            Exit Sub
        End Try


    End Sub
    Sub CaricaGriglia1()
        Dim sqlDAP1 As New SqlClient.SqlDataAdapter
        Dim dataSet1 As New DataSet
        Dim strNomeStore1 As String = "[SP_RIATTIVAZIONE_CRONO_VOLONTARIO]"

        Try
            sqlDAP1 = New SqlClient.SqlDataAdapter(strNomeStore1, CType(Session("conn"), SqlClient.SqlConnection))
            sqlDAP1.SelectCommand.CommandType = CommandType.StoredProcedure

            sqlDAP1.SelectCommand.Parameters.Add("@IDEntità", SqlDbType.Int).Value = Request.QueryString("IdVol")


            sqlDAP1.Fill(dataSet1)

            Session("appDtsRisRicerca1") = dataSet1
            dtgRisultatoRicerca1.DataSource = dataSet1
            dtgRisultatoRicerca1.DataBind()

        Catch ex As Exception
            Response.Write(ex.Message.ToString())
            Exit Sub
        End Try
    End Sub
   
    'Private Sub dtgRisultatoRicerca1_PageIndexChanged(source As Object, e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dtgRisultatoRicerca1.PageIndexChanged
    '    dtgRisultatoRicerca1.CurrentPageIndex = e.NewPageIndex
    '    dtgRisultatoRicerca1.DataSource = Session("appDtsRisRicerca1")
    '    dtgRisultatoRicerca1.DataBind()
    'End Sub


    Protected Sub cmdIndietro_Click(sender As Object, e As EventArgs) Handles cmdIndietro.Click
        Dim resp As StringBuilder = New StringBuilder()
        Dim windowOption As String = "dependent=no,scrollbars=yes,status=no,resizable=yes,width=1000px ,height=400px"
        resp.Append("<script  type=""text/javascript"">" & vbCrLf)
        resp.Append("myWin = window.open('WfrmAttivitaVolontari.aspx?IdVolontario=" + Request.QueryString("IdVol") + "', 'win'" + ",'" + windowOption + "')")
        resp.Append("</script>")
        Response.Write(resp.ToString())
    End Sub

    Sub CaricaGrigliaSede()

        Dim sqlDAP As New SqlClient.SqlDataAdapter
        Dim dataSet As New DataSet
        Dim strNomeStore As String = "[SP_RIATTIVAZIONE_CRONO_SEDE]"

        Try
            sqlDAP = New SqlClient.SqlDataAdapter(strNomeStore, CType(Session("conn"), SqlClient.SqlConnection))
            sqlDAP.SelectCommand.CommandType = CommandType.StoredProcedure

            sqlDAP.SelectCommand.Parameters.Add("@IDAttivitàSedeAssegnazione", SqlDbType.Int).Value = Request.QueryString("idattivitaSedeAssegnazione")
            
            sqlDAP.Fill(dataSet)

            Session("appDtsRisRicerca") = dataSet
            dtgRisultatoRicerca.DataSource = dataSet
            dtgRisultatoRicerca.DataBind()

        Catch ex As Exception
            Response.Write(ex.Message.ToString())
            Exit Sub
        End Try


    End Sub
End Class