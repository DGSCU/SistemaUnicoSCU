Imports System.Data.SqlClient

Public Class WfrmPopUpEntiFiglio
    Inherits System.Web.UI.Page
    Dim dtrgenerico As SqlClient.SqlDataReader
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
#End Region
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load
        VerificaSessione()
    End Sub

    Private Sub cmdRicerca_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles cmdRicerca.Click
        Dim strsql As String
        strsql = "select distinct entirelazioni.IdEnteFiglio, enti.Denominazione,enti.codiceregione,enti.codicefiscale from entirelazioni "
        strsql = strsql & "inner join enti on enti.IdEnte=entirelazioni.IdEnteFiglio "
        strsql = strsql & "where entirelazioni.identepadre=" & CInt(Session("IdEnte"))
        '''FILTRI''''
        If Trim(txtdenominazione.Text) <> "" Then
            strsql = strsql & " and enti.denominazione like '" & Replace(txtdenominazione.Text, "'", "''") & "%'"
        End If
        If Trim(txtCodFis.Text) <> "" Then
            strsql = strsql & " and enti.codicefiscale ='" & Replace(txtCodFis.Text, "'", "''") & "' "
        End If
        If Trim(txtEnteFiglio.Text) <> "" Then
            strsql = strsql & " and enti.codiceregione = '" & Replace(txtEnteFiglio.Text, "'", "''") & "' "
        End If
        strsql = strsql & " and entirelazioni.datafinevalidità is null order by enti.Denominazione"
        ChiudiDataReader(dtrgenerico)
        dtsgenerico = ClsServer.DataSetGenerico(strsql, Session("conn"))
        dgRisultatoRicerca.DataSource = dtsgenerico
        dgRisultatoRicerca.DataBind()
        If (dgRisultatoRicerca.Items.Count = 0) Then
            dgRisultatoRicerca.Caption = "La Ricerca non ha prodotto risultati."
        Else
            dgRisultatoRicerca.Caption = "Risultato Ricerca Ente."
        End If
        Session("LocalDataSet") = dtsgenerico
        ChiudiDataReader(dtrgenerico)
    End Sub


    Private Sub dgRisultatoRicerca_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgRisultatoRicerca.PageIndexChanged
        dgRisultatoRicerca.CurrentPageIndex = e.NewPageIndex
        'riassegno il dataset dichiarato volutamente pubblico a tutta la pagina
        dgRisultatoRicerca.DataSource = Session("LocalDataSet")
        dgRisultatoRicerca.DataBind()
    End Sub

    Private Sub dgRisultatoRicerca_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dgRisultatoRicerca.SelectedIndexChanged
        If Not dgRisultatoRicerca.SelectedItem Is Nothing Then

            Context.Items.Add("identefiglio", dgRisultatoRicerca.Items(dgRisultatoRicerca.SelectedIndex).Cells(1).Text)
            Response.Write("<script>" & vbCrLf)
            Response.Write("window.opener.document.all.MainContent_ddlEntiFigli.value='" & dgRisultatoRicerca.Items(dgRisultatoRicerca.SelectedIndex).Cells(1).Text & "';" & vbCrLf)
            Response.Write("window.close();" & vbCrLf)
            Response.Write("</script>")


        End If

    End Sub

End Class
