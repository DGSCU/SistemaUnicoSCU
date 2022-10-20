Public Class WfrmInfoClassi
    Inherits System.Web.UI.Page
    Dim dtsGenerico As DataSet
    Dim strsql As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Generato da Alessandra Taballione il 25/02/2004
        'popolamento della dataGrid delle informazioni delle classi 
        'Inserire qui il codice utente necessario per inizializzare la pagina
        If Not Session("LogIn") Is Nothing Then
            If Session("LogIn") = False Then
                Response.Redirect("LogOn.aspx")
            End If
        Else
            Response.Redirect("LogOn.aspx")
        End If
        If IsPostBack = False Then
            dtsGenerico = ClsServer.DataSetGenerico(" Select * " & _
                " from classiaccreditamento where minSedi > 0 ", IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
            CaricaDataGrid(dgRisultatoRicerca)
        End If
    End Sub

    Private Sub CaricaDataGrid(ByRef GridDaCaricare As DataGrid) 'valorizzo la datagrid passata
        'Generato da Alessandra Taballione il 25/04/20004
        'CAricamento della DataGrid
        Dim appo As String
        appo = dgRisultatoRicerca.CurrentPageIndex
        GridDaCaricare.DataSource = dtsGenerico
        GridDaCaricare.DataBind()
        GridDaCaricare.Visible = True
        If GridDaCaricare.Items.Count = 0 Then
            GridDaCaricare.Visible = False
        End If

    End Sub

    Private Sub cmdChiudi_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdChiudi.Click
        'Generato da Alessandra Taballione il 25/04/20004
        'Chiudo la pagina
        Response.Write("<script>" & vbCrLf)
        Response.Write("window.close();" & vbCrLf)
        Response.Write("</script>")
    End Sub

End Class