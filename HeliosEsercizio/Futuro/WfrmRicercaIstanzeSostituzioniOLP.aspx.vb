Imports Logger.Data
Public Class WfrmRicercaIstanzeSostituzioniOLP
    Inherits SmartPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        checkSpid()
        'controllo se è stato effettuato il login
        If Not Session("LogIn") Is Nothing Then
            'se non è stato effettuato login
            If Session("LogIn") = False Then
                'carico la pagina LogOut dove svuoto eventuali session aperte
                Response.Redirect("LogOn.aspx")
            End If
        Else
            'carico la pagina LogOut dove svuoto eventuali session aperte
            Response.Redirect("LogOn.aspx")
        End If

        If Not Page.IsPostBack Then
            If Request.QueryString("Messaggio") <> Nothing Then
                lblMessaggio.CssClass = "msgInfo"
                lblMessaggio.Text = Request.QueryString("Messaggio")
            Else
                lblMessaggio.Text = ""
            End If

            If Session("TipoUtente") = "U" Then
                divRicercaEnte.Visible = True
                dgIstanze.Columns(3).Visible = True
                dgIstanze.Columns(4).Visible = True
                cmdInserisci.Visible = False
            End If

        End If
    End Sub

    Protected Sub cmdInserisci_Click(sender As Object, e As EventArgs) Handles cmdInserisci.Click
        Response.Redirect("WfrmIstanzaSostituzioniOLP.aspx")
    End Sub

    Protected Sub btnRicerca_Click(sender As Object, e As EventArgs) Handles btnRicerca.Click
        RicercaIstanzeOLP()
        lblMessaggio.Text = ""
    End Sub

    Private Sub RicercaIstanzeOLP()

        Dim sqlDAP As New SqlClient.SqlDataAdapter
        Dim dataSet As New DataSet
        Dim strNomeStore As String = "[SP_ISTANZE_SOSTITUZIONI_OLP_RICERCA]"


        Try
            sqlDAP = New SqlClient.SqlDataAdapter(strNomeStore, CType(Session("conn"), SqlClient.SqlConnection))
            sqlDAP.SelectCommand.CommandType = CommandType.StoredProcedure

            If Session("TipoUtente") = "E" Then
                If Session("IdEnte") IsNot Nothing AndAlso Session("IdEnte") <> "-1" Then sqlDAP.SelectCommand.Parameters.Add("@idente", SqlDbType.Int).Value = Integer.Parse(Session("IdEnte"))
            End If

            sqlDAP.SelectCommand.Parameters.Add("@Username", SqlDbType.NVarChar, 10).Value = Session("Utente")
            If Not String.IsNullOrEmpty(txtDenominazioneEnte.Text) Then sqlDAP.SelectCommand.Parameters.Add("@denominazioneente", SqlDbType.NVarChar).Value = txtDenominazioneEnte.Text
            If Not String.IsNullOrEmpty(txtCodRegione.Text) Then sqlDAP.SelectCommand.Parameters.Add("@codiceregione", SqlDbType.NVarChar).Value = txtCodRegione.Text
            If Not String.IsNullOrEmpty(txtTitoloProgetto.Text) Then sqlDAP.SelectCommand.Parameters.Add("@titoloprogetto", SqlDbType.NVarChar).Value = txtTitoloProgetto.Text
            If Not String.IsNullOrEmpty(TxtCodProg.Text) Then sqlDAP.SelectCommand.Parameters.Add("@codiceprogetto", SqlDbType.NVarChar).Value = TxtCodProg.Text
            If Not String.IsNullOrEmpty(txtNomeSostRicerca.Text) Then sqlDAP.SelectCommand.Parameters.Add("@nomesostituito", SqlDbType.NVarChar).Value = txtNomeSostRicerca.Text
            If Not String.IsNullOrEmpty(txtCognomeSostRicerca.Text) Then sqlDAP.SelectCommand.Parameters.Add("@cognomesostituito", SqlDbType.NVarChar).Value = txtCognomeSostRicerca.Text
            If Not String.IsNullOrEmpty(txtNomeSubRicerca.Text) Then sqlDAP.SelectCommand.Parameters.Add("@nomesubentrante", SqlDbType.NVarChar).Value = txtNomeSubRicerca.Text
            If Not String.IsNullOrEmpty(txtCognomeSubRicerca.Text) Then sqlDAP.SelectCommand.Parameters.Add("@cognomesubentrante", SqlDbType.NVarChar).Value = txtCognomeSubRicerca.Text
            If Not ddlStatoIstanza.SelectedValue = "0" Then sqlDAP.SelectCommand.Parameters.Add("@statoistanza", SqlDbType.Int).Value = Integer.Parse(ddlStatoIstanza.SelectedValue)

            If txtDataInizio.Text.Trim = "" Then
                sqlDAP.SelectCommand.Parameters.Add("@DataProtocolloInizio", SqlDbType.VarChar).Value = DBNull.Value
            Else
                sqlDAP.SelectCommand.Parameters.Add("@DataProtocolloInizio", SqlDbType.VarChar).Value = DataToISO(txtDataInizio.Text.Trim)
            End If

            If txtDataFine.Text.Trim = "" Then
                sqlDAP.SelectCommand.Parameters.Add("@DataProtocolloFine", SqlDbType.VarChar).Value = DBNull.Value
            Else
                sqlDAP.SelectCommand.Parameters.Add("@DataProtocolloFine", SqlDbType.VarChar).Value = DataToISO(txtDataFine.Text.Trim)
            End If

            sqlDAP.Fill(dataSet)

            Session("ElencoIstanzeOLP") = dataSet
            dgIstanze.DataSource = dataSet
            dgIstanze.CurrentPageIndex = 0
            dgIstanze.DataBind()

        Catch ex As Exception
            Response.Write(ex.Message.ToString())
            Exit Sub
        End Try
    End Sub

    Protected Sub dgIstanze_PageIndexChanged(source As Object, e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgIstanze.PageIndexChanged
        Dim dataSet As New DataSet
        dgIstanze.CurrentPageIndex = e.NewPageIndex
        dataSet = Session("ElencoIstanzeOLP")
        dgIstanze.DataSource = dataSet
        dgIstanze.DataBind()
    End Sub

    Protected Sub dgIstanze_ItemCommand(source As Object, e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgIstanze.ItemCommand
        Select Case e.CommandName

            Case "Select"
                If Session("TipoUtente") = "U" Then
                    Session("IdEnte") = e.Item.Cells(1).Text
                    Session("Denominazione") = e.Item.Cells(4).Text
                    Session("Competenza") = e.Item.Cells(2).Text
                End If

                Response.Redirect("WfrmIstanzaSostituzioniOLP.aspx?IdIstanzaSostituzioneOLP=" & e.Item.Cells(10).Text)

        End Select

    End Sub

    Function DataToISO(data As String) As String

        Return data.Substring(6, 4) + data.Substring(3, 2) + data.Substring(0, 2)

    End Function

End Class