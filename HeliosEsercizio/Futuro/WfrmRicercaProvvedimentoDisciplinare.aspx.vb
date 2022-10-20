Public Class WfrmRicercaProvvedimentoDisciplinare
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
        If dtgRisultatoRicerca.Items.Count = 0 Then 'se la griglia e vuota la nascondo
            LblErrore.Text = "La ricerca non ha prodotto alcun risultato"
            dtgRisultatoRicerca.Visible = False
            CmdEsporta.Visible = False
        Else
            LblErrore.Text = ""
            CmdEsporta.Visible = False
            dtgRisultatoRicerca.Visible = True
            Call CarciaDatiStampa(Session("appDtsRisRicerca"))
        End If
    End Sub

    Private Function EseguiRicerca() As Boolean
        'Creata da Simona Cordella il 12/03/2018
        dtgRisultatoRicerca.CurrentPageIndex = 0

        Dim sqlDAP As New SqlClient.SqlDataAdapter
        Dim DataSet As New DataSet
        Dim strNomeStore As String = "SP_VOLONTARI_PROVVEDIMENTO_DISCIPLINARE_RICERCA"


        Try
            sqlDAP = New SqlClient.SqlDataAdapter(strNomeStore, CType(Session("conn"), SqlClient.SqlConnection))
            sqlDAP.SelectCommand.CommandType = CommandType.StoredProcedure

            sqlDAP.SelectCommand.Parameters.Add("@NumeroFascicolo", SqlDbType.VarChar).Value = txtCodiceFascicolo.Text.Replace("'", "''")
            sqlDAP.SelectCommand.Parameters.Add("@DenominazioneEnte", SqlDbType.VarChar).Value = txtDescEnte.Text.Replace("'", "''")
            sqlDAP.SelectCommand.Parameters.Add("@CodiceEnte", SqlDbType.VarChar).Value = txtCodEnte.Text.Replace("'", "''")
            sqlDAP.SelectCommand.Parameters.Add("@CodiceProgetto", SqlDbType.VarChar).Value = txtCodProgetto.Text.Replace("'", "''")
            sqlDAP.SelectCommand.Parameters.Add("@Progetto", SqlDbType.VarChar).Value = txtProgetto.Text.Replace("'", "''")
            sqlDAP.SelectCommand.Parameters.Add("@Cognome", SqlDbType.VarChar).Value = txtCognome.Text.Replace("'", "''")
            sqlDAP.SelectCommand.Parameters.Add("@Nome", SqlDbType.VarChar).Value = txtNome.Text.Replace("'", "''")
            sqlDAP.SelectCommand.Parameters.Add("@CodiceVolontario", SqlDbType.VarChar).Value = txtCodVolontario.Text.Replace("'", "''")
            sqlDAP.SelectCommand.Parameters.Add("@CodiceFiscale", SqlDbType.VarChar).Value = txtCodFiscale.Text.Replace("'", "''")
            sqlDAP.SelectCommand.Parameters.Add("@FiltroVisibilita", SqlDbType.VarChar).Value = Session("FiltroVisibilita")
            sqlDAP.SelectCommand.Parameters.Add("@IdStatoProvvedimento", SqlDbType.VarChar).Value = IIf(ddlStatoProvvedimento.SelectedValue = 0, "", ddlStatoProvvedimento.SelectedValue)


            sqlDAP.Fill(DataSet)

            Session("appDtsRisRicerca") = DataSet
            dtgRisultatoRicerca.DataSource = DataSet
            dtgRisultatoRicerca.DataBind()

        Catch ex As Exception
            Response.Write(ex.Message.ToString())
            Exit Function
        End Try
    End Function
    Sub CarciaDatiStampa(ByVal dtsGenerico As DataSet)
        '*********************************************************************************
        'blocco per la creazione della datatable per la stampa della ricerca
        'nome e posizione di lettura delle colopnne a base 0
        Dim NomeColonne(5) As String
        Dim NomiCampiColonne(5) As String

        'nome della colonna 
        'e posizione nella griglia di lettura
        NomeColonne(0) = "Numero Fascicolo"
        NomeColonne(1) = "Stato Provvedimento"
        NomeColonne(2) = "Volontario"
        NomeColonne(3) = "Progetto"
        NomeColonne(4) = "Ente"
        NomeColonne(5) = "Sede"


        NomiCampiColonne(0) = "NumeroFascicolo"
        NomiCampiColonne(1) = "StatoProvvedimento"
        NomiCampiColonne(2) = "Nominativo"
        NomiCampiColonne(3) = "Progetto"
        NomiCampiColonne(4) = "ente"
        NomiCampiColonne(5) = "sede"


        'carico un datatable che userò poi nella pagina di stampa
        'il numero delle colonne è a base 0
        ClsServer.CaricaDataTablePerStampa(dtsGenerico, 5, NomeColonne, NomiCampiColonne)
    End Sub
    Private Sub CaricaCombo()
        Dim strSql As String
        Try
            Dim MyDataset As DataSet
 

            ddlStatoProvvedimento.Items.Clear()
            strSql = "SELECT '0' as IDStatoProvvedimento, '' as StatoProvvedimento FROM StatiEntità " & _
                     "UNION SELECT IDStatoProvvedimento, StatoProvvedimento FROM StatiProvvedimentoDisciplinare "
            MyDataset = ClsServer.DataSetGenerico(strSql, Session("conn"))
            ddlStatoProvvedimento.DataSource = MyDataset
            ddlStatoProvvedimento.DataValueField = "IDStatoProvvedimento"
            ddlStatoProvvedimento.DataTextField = "StatoProvvedimento"
            ddlStatoProvvedimento.DataBind()

        Catch

        End Try
    End Sub
    Private Sub dtgRisultatoRicerca_ItemCommand(source As Object, e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dtgRisultatoRicerca.ItemCommand
        If e.CommandName = "seleziona" Then

            'creato DA SIMONA CORDELLA IL 23/03/2018
            Response.Redirect("WfrmGestioneProvvedimentoDisciplinare.aspx?VengoDa=Modifica&IdAttivitàEntità=" & e.Item.Cells(8).Text & "&IdProvvedimentoDisciplinare=" & e.Item.Cells(1).Text)
        End If
    End Sub

    Private Sub dtgRisultatoRicerca_PageIndexChanged(source As Object, e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dtgRisultatoRicerca.PageIndexChanged

        dtgRisultatoRicerca.CurrentPageIndex = e.NewPageIndex
        dtgRisultatoRicerca.DataSource = Session("appDtsRisRicerca")
        dtgRisultatoRicerca.DataBind()
    End Sub

    Protected Sub dtgRisultatoRicerca_SelectedIndexChanged(sender As Object, e As EventArgs) Handles dtgRisultatoRicerca.SelectedIndexChanged

    End Sub

    Private Sub cmdChiudi_Click(sender As Object, e As System.EventArgs) Handles cmdChiudi.Click
        Response.Redirect("WfrmMain.aspx")
    End Sub

    Protected Sub CmdEsporta_Click(sender As Object, e As EventArgs) Handles CmdEsporta.Click

    End Sub
End Class