Imports System.Drawing.Printing
Imports System.Drawing.Imaging
Imports System.Web.UI
Imports System.Drawing
Imports System.IO

Public Class WfrmNavigaEnte
    Inherits System.Web.UI.Page
    Private WithEvents pndocument As PrintDocument
    Dim strsql As String
    Dim dtrgenerico As SqlClient.SqlDataReader
    Dim dtsGenerico As DataSet
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        RicercaEnti()
        lblmessaggio.Text = "ELENCO ENTI CON CODICE FISCALE" & " " & Request.QueryString("CodiceFiscale")
    End Sub
    Private Sub RicercaEnti()


        Dim sqlDAP As New SqlClient.SqlDataAdapter
        dtsGenerico = New DataSet
        Dim strNomeStore As String = "SP_ACCREDITAMENTO_RICERCA_ENTE_NEW"

        Try
            sqlDAP = New SqlClient.SqlDataAdapter(strNomeStore, CType(Session("conn"), SqlClient.SqlConnection))
            sqlDAP.SelectCommand.CommandType = CommandType.StoredProcedure
            sqlDAP.SelectCommand.Parameters.Add("@codiceFiscale", SqlDbType.NVarChar, 50).Value = Request.QueryString("CodiceFiscale")
            sqlDAP.Fill(dtsGenerico)

            CaricaDataGrid(dgRisultatoRicerca)

            'sqlDAP.SelectCommand.Connection.Close()


        Catch ex As Exception
            Response.Write(ex.Message.ToString())
            Exit Sub
        End Try


    End Sub
    Sub CaricaDataGrid(ByRef GridDaCaricare As DataGrid) 'valorizzo la datagrid passata
        GridDaCaricare.DataSource = dtsGenerico
        GridDaCaricare.DataBind()
        If Not dtsGenerico Is Nothing Then
            dtsGenerico = Nothing
        End If
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
    End Sub


    Private Sub dgRisultatoRicerca_PageIndexChanged(source As Object, e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgRisultatoRicerca.PageIndexChanged
        RicercaEnti()
        dgRisultatoRicerca.SelectedIndex = -1
        dgRisultatoRicerca.EditItemIndex = -1
        dgRisultatoRicerca.CurrentPageIndex = e.NewPageIndex
        CaricaDataGrid(dgRisultatoRicerca)
    End Sub
    Private Sub dgRisultatoRicerca_ItemCommand(source As Object, e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgRisultatoRicerca.ItemCommand

        Dim CodiceFiscale As String = Request.QueryString("CodiceFiscale")
        If e.CommandName = "NumeroTotSedi" Then
            'RicordaParametri()
            If (Session("TipoUtente") = "U") Then

                Response.Redirect("~/WfrmNavigaSedi.aspx?IdEnte=" & e.Item.Cells(0).Text & "&VengoDa=" & 1 & "&CodiceFiscale=" & CodiceFiscale)
            Else
                Response.Redirect("page_error.aspx")
            End If
        End If
        If e.CommandName = "NumeroEntiAccoglienza" Then
            'RicordaParametri()
            If (Session("TipoUtente") = "U") Then

                Response.Redirect("~/WfrmNavigaEntiAccoglienza.aspx?IdEnte=" & e.Item.Cells(0).Text & "&VengoDa=" & 1 & "&CodiceFiscale=" & CodiceFiscale)
            Else
                Response.Redirect("page_error.aspx")
            End If
        End If
        If e.CommandName = "Info" Then
            'RicordaParametri()
            If (Session("TipoUtente") = "U") Then
                'Session("IdEnte") = e.Item.Cells(15).Text
                'Session("Denominazione") = e.Item.Cells(16).Text
                Response.Redirect("~/WfrmNavigaInfoEnte.aspx?IdEnte=" & e.Item.Cells(0).Text & "&VengoDa=" & 1 & "&CodiceFiscale=" & CodiceFiscale)
            Else
                Response.Redirect("page_error.aspx")
            End If
        End If


    End Sub

    Protected Sub cmdChiudi_Click(sender As Object, e As EventArgs) Handles cmdChiudi.Click
        'Me.close()
    End Sub
End Class