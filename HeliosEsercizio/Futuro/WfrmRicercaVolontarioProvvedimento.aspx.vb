Public Class WfrmRicercaVolontarioProvvedimento
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Inserire qui il codice utente necessario per inizializzare la pagina
        If Not Session("LogIn") Is Nothing Then
            If Session("LogIn") = False Then 'verifico validità log-in
                Response.Redirect("LogOn.aspx")
            End If
        Else
            Response.Redirect("LogOn.aspx")
        End If
        If Page.IsPostBack = False Then
            CaricaCombo()
        End If
    End Sub

    Protected Sub cmdRicerca_Click(sender As Object, e As EventArgs) Handles cmdRicerca.Click
        EseguiRicerca()
    End Sub
    Private Sub CaricaCombo()
        Dim strSql As String
        Try
            Dim MyDataset As DataSet

            cboStato.Items.Clear()
            strSql = "SELECT '0' as IdStatoEntità, '' as Statoentità FROM StatiEntità " & _
                     "UNION SELECT IdStatoEntità, StatoEntità  FROM StatiEntità"
            MyDataset = ClsServer.DataSetGenerico(strSql, Session("conn"))
            cboStato.DataSource = MyDataset
            cboStato.DataValueField = "IdStatoEntità"
            cboStato.DataTextField = "StatoEntità"
            cboStato.DataBind()

        Catch

        End Try
    End Sub
    Private Function EseguiRicerca() As Boolean
        'Creata da Simona Cordella il 12/03/2018

        dtgRisultatoRicerca.CurrentPageIndex = 0
        Dim sqlDAP As New SqlClient.SqlDataAdapter
        Dim DataSet As New DataSet
        Dim strNomeStore As String = "[SP_VOLONTARI_RICERCA_PROVVEDIMENTO_DISCIPLINARE]"


        Try
            sqlDAP = New SqlClient.SqlDataAdapter(strNomeStore, CType(Session("conn"), SqlClient.SqlConnection))
            sqlDAP.SelectCommand.CommandType = CommandType.StoredProcedure

            sqlDAP.SelectCommand.Parameters.Add("@DenominazioneEnte", SqlDbType.VarChar).Value = txtDescEnte.Text.Replace("'", "''")
            sqlDAP.SelectCommand.Parameters.Add("@CodiceEnte", SqlDbType.VarChar).Value = txtCodEnte.Text.Replace("'", "''")
            sqlDAP.SelectCommand.Parameters.Add("@CodiceProgetto", SqlDbType.VarChar).Value = txtCodProgetto.Text.Replace("'", "''")
            sqlDAP.SelectCommand.Parameters.Add("@Progetto", SqlDbType.VarChar).Value = txtProgetto.Text.Replace("'", "''")
            sqlDAP.SelectCommand.Parameters.Add("@Cognome", SqlDbType.VarChar).Value = txtCognome.Text.Replace("'", "''")
            sqlDAP.SelectCommand.Parameters.Add("@Nome", SqlDbType.VarChar).Value = txtNome.Text.Replace("'", "''")
            sqlDAP.SelectCommand.Parameters.Add("@CodiceVolontario", SqlDbType.VarChar).Value = txtCodVolontario.Text.Replace("'", "''")
            sqlDAP.SelectCommand.Parameters.Add("@CodiceFiscale", SqlDbType.VarChar).Value = txtCodFiscale.Text.Replace("'", "''")
            sqlDAP.SelectCommand.Parameters.Add("@FiltroVisibilita", SqlDbType.VarChar).Value = Session("FiltroVisibilita")
            sqlDAP.SelectCommand.Parameters.Add("@IDStatoEntità", SqlDbType.VarChar).Value = IIf(cboStato.SelectedValue = 0, "", cboStato.SelectedValue)

            sqlDAP.Fill(DataSet)

            Session("appDtsRisRicerca") = DataSet
            dtgRisultatoRicerca.DataSource = DataSet
            dtgRisultatoRicerca.DataBind()

        Catch ex As Exception
            Response.Write(ex.Message.ToString())
            Exit Function
        End Try
    End Function

    Private Sub dtgRisultatoRicerca_ItemCommand(source As Object, e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dtgRisultatoRicerca.ItemCommand
        If e.CommandName = "seleziona" Then

            'MODIFICATO DA SIMONA CORDELLA IL 19/03/2018
            Response.Redirect("WfrmGestioneProvvedimentoDisciplinare.aspx?VengoDa=Inserimento&IdEntità=" & e.Item.Cells(1).Text & "&IdAttivitàEntità=" & e.Item.Cells(2).Text & "&IdEnte=" & e.Item.Cells(3).Text)
        End If
    End Sub

    Private Sub dtgRisultatoRicerca_PageIndexChanged(source As Object, e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dtgRisultatoRicerca.PageIndexChanged
        dtgRisultatoRicerca.CurrentPageIndex = e.NewPageIndex
        dtgRisultatoRicerca.DataSource = Session("appDtsRisRicerca")
        dtgRisultatoRicerca.DataBind()
    End Sub

    Protected Sub dtgRisultatoRicerca_SelectedIndexChanged(sender As Object, e As EventArgs) Handles dtgRisultatoRicerca.SelectedIndexChanged

    End Sub

    Protected Sub cmdChiudi_Click(sender As Object, e As EventArgs) Handles cmdChiudi.Click
        Response.Redirect("WfrmMain.aspx")
    End Sub
End Class