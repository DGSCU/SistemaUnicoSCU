Imports System.Data.SqlClient
Public Class WebFrmStoricoProgrammi
    Inherits System.Web.UI.Page

    Dim strsql As String
    Dim dtrGenerico As SqlClient.SqlDataReader
    Dim dtsgenerico As DataSet

#Region "Utility"
    Private Sub ChiudiDataReader(ByRef dataReader As SqlDataReader)
        If Not dataReader Is Nothing Then
            dataReader.Close()
            dataReader = Nothing
        End If
    End Sub

    Private Sub VerificaSessione()
        If Not Session("LogIn") Is Nothing Then
            If Session("LogIn") = False Then
                Response.Redirect("LogOn.aspx")
            End If
        Else
            Response.Redirect("LogOn.aspx")
        End If
    End Sub

    Private Sub CancellaMessaggi()
        msgErrore.Text = String.Empty
        msgInfo.Text = String.Empty
        msgConferma.Text = String.Empty
    End Sub
    Private Sub NascondiMenuLaterale()
        Session("TP") = True
    End Sub
#End Region
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Inserire qui il codice utente necessario per inizializzare la pagina
        VerificaSessione()
        If IsPostBack = False Then
            hf_IdProgramma.Value = IIf(Not IsNothing(Request.QueryString("IdProgramma")), Request.QueryString("IdProgramma"), hf_IdProgramma.Value)
            CaricaDataGrid(dgRisultatoRicerca)
            hf_Pagina.Value = Context.Items("Pagina")
        End If
    End Sub
    Sub CaricaDataGrid(ByRef datagrid As DataGrid)
        ChiudiDataReader(dtrGenerico)
        strsql = "select * from StoricoProgrammi Where idProgramma=" & hf_IdProgramma.Value & " order by datainserimento desc"
        dtsgenerico = ClsServer.DataSetGenerico(strsql, Session("conn"))
        datagrid.DataSource = dtsgenerico
        datagrid.DataBind()
    End Sub

    Private Sub dgRisultatoRicerca_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dgRisultatoRicerca.SelectedIndexChanged
        Response.Redirect("WfrmValutazioneQualProgrammi.aspx?idprogramma=" & hf_IdProgramma.Value & "&personalizza=Modifica&idStorico=" & dgRisultatoRicerca.SelectedItem.Cells(5).Text)
    End Sub

    Private Sub dgRisultatoRicerca_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgRisultatoRicerca.PageIndexChanged
        dgRisultatoRicerca.SelectedIndex = -1
        dgRisultatoRicerca.EditItemIndex = -1
        dgRisultatoRicerca.CurrentPageIndex = e.NewPageIndex
        CaricaDataGrid(dgRisultatoRicerca)
    End Sub

    Private Sub cmdChiudi_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles cmdChiudi.Click
        Context.Items.Add("idprogramma", hf_IdProgramma.Value)
        If (Request.QueryString("VengoDa") = Costanti.VENGO_DA_VALUTAZIONE_QUALITA) Then
            Response.Redirect("WfrmValutazioneQualProgrammi.aspx?idprogramma=" & hf_IdProgramma.Value)
        End If
    End Sub

End Class