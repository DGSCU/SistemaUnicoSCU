Public Class WfrmAbilitaEntiCorsoOLP
    Inherits System.Web.UI.Page
    Dim strsql As String
    Dim dtrgenerico As SqlClient.SqlDataReader
    Dim dtsGenerico As DataSet
    Dim myCommand As System.Data.SqlClient.SqlCommand
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Response.ExpiresAbsolute = #1/1/1980#
        Response.AddHeader("Pragma", "no-cache")
        If Not Session("LogIn") Is Nothing Then
            If Session("LogIn") = False Then 'verifico validità log-in
                Response.Redirect("LogOn.aspx")
            End If
        Else
            Response.Redirect("LogOn.aspx")
        End If

        If Not IsPostBack = True Then
            Session("DtbRicerca") = Nothing

           
            'cerco il codice regione e lo stampo nel caso in cui ci sia
            If Not dtrgenerico Is Nothing Then
                dtrgenerico.Close()
                dtrgenerico = Nothing
            End If
            


            RicercaEnti()


        End If
       
    End Sub
    Private Sub RicercaEnti()
        strsql = "Select idente,codiceregione,Denominazione,tipologia,Idclasseaccreditamento,CodiceFiscale,isnull(AbilitazioneCorsiOLP,0)as AbilitazioneCorsiOLP from enti where idclasseaccreditamento=1 and idregionecompetenza=22 and IdStatoEnte in(3,9) order by 2 "

        dtsGenerico = ClsServer.DataSetGenerico(strsql, Session("conn"))
        dgRisultatoRicerca.DataSource = dtsGenerico
        dgRisultatoRicerca.DataBind()
    End Sub

    Private Sub dgRisultatoRicerca_ItemCommand(source As Object, e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgRisultatoRicerca.ItemCommand
        Dim InsertUpdate As Integer
        If e.CommandName = "Abilita" Then

            InsertUpdate = 1
            UpdateEnti(e.Item.Cells(0).Text, InsertUpdate)

            RicercaEnti()
            lblmessaggio.Text = "Abilitato con successo"


        End If


        If e.CommandName = "Disabilita" Then
            InsertUpdate = 0
            UpdateEnti(e.Item.Cells(0).Text, InsertUpdate)

            RicercaEnti()
            lblmessaggio.Text = "Disabilitato con successo"
           
        End If


    End Sub
    Private Sub UpdateEnti(idente As String, valore As Integer)
        strsql = "update Enti set AbilitazioneCorsiOLP=" & valore & "  where IdEnte=" & idente

        'sql command che mi esegue la insert
        myCommand = New SqlClient.SqlCommand(strsql, Session("conn"))
        myCommand.ExecuteNonQuery()
        myCommand.Dispose()


    End Sub

    Protected Sub cmdChiudi_Click(sender As Object, e As EventArgs) Handles cmdChiudi.Click
        Response.Redirect("WfrmMain.aspx")
    End Sub
End Class