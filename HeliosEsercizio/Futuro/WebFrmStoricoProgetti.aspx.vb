Imports System.Data.SqlClient

Public Class WebFrmStoricoProgetti
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

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Inserire qui il codice utente necessario per inizializzare la pagina
        VerificaSessione()
        If IsPostBack = False Then
            hf_IdProgetto.Value = IIf(Not IsNothing(Request.QueryString("idprogetto")), Request.QueryString("idprogetto"), hf_IdProgetto.Value)
            CaricaDataGrid(dgRisultatoRicerca)
            hf_Pagina.Value = Context.Items("Pagina")
        End If
    End Sub
    Sub CaricaDataGrid(ByRef datagrid As DataGrid)
        ChiudiDataReader(dtrGenerico)
        strsql = "select * from StoricoProgetti Where idProgetto=" & hf_IdProgetto.Value & " order by datainserimento desc"
        dtsgenerico = ClsServer.DataSetGenerico(strsql, Session("conn"))
        datagrid.DataSource = dtsgenerico
        datagrid.DataBind()
    End Sub

    Private Sub dgRisultatoRicerca_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dgRisultatoRicerca.SelectedIndexChanged
        Response.Redirect(ClsUtility.RitornaMascheraValutazioneProgetto(hf_IdProgetto.Value, Session("conn")) & "?idprogetto=" & hf_IdProgetto.Value & "&personalizza=Modifica&idStorico" & dgRisultatoRicerca.SelectedItem.Cells(7).Text)
    End Sub

    Private Sub dgRisultatoRicerca_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgRisultatoRicerca.PageIndexChanged
        dgRisultatoRicerca.SelectedIndex = -1
        dgRisultatoRicerca.EditItemIndex = -1
        dgRisultatoRicerca.CurrentPageIndex = e.NewPageIndex
        CaricaDataGrid(dgRisultatoRicerca)
    End Sub

    Private Sub cmdChiudi_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles cmdChiudi.Click
        Context.Items.Add("idprogetto", hf_IdProgetto.Value)
        If (Request.QueryString("VengoDa") = Costanti.VENGO_DA_VALUTAZIONE_QUALITA) Then
            Response.Redirect(ClsUtility.RitornaMascheraValutazioneProgetto(hf_IdProgetto.Value, Session("conn")) & "?idprogetto=" & hf_IdProgetto.Value)
        End If
    End Sub

End Class