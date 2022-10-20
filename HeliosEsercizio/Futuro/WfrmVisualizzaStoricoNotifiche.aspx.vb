Imports System.Drawing
Public Class WfrmVisualizzaStoricoNotifiche
    Inherits System.Web.UI.Page
   
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
       
        Dim IdEntita As String = Request.QueryString("IdEntita")
        Dim IdcheckList As String = Request.QueryString("IdLista")
        Dim IdEnte As Integer
        Dim TipoNotifica As String = Request.QueryString("IdTipoNotifica")
        Dim Idattivita As String = Request.QueryString("IdAttività")

        If Page.IsPostBack = False Then
            txtDataAl.Text = Date.Today
            CaricaComboUser()
            If TipoNotifica = 3 Then
                ddlTipoNotifica.SelectedIndex = 3

                Dim strSql As String
                Dim rstVol As SqlClient.SqlDataReader
                'query per checkList
                strSql = "SELECT  enti.IDEnte, enti.Denominazione, enti.CodiceRegione, 'C' + CONVERT(VARCHAR, CheckListPagheCollettivo.IdCheckList) AS CODICECHECKLIST, CheckListPagheCollettivo.IdCheckList " & _
                         " FROM enti " & _
                         " INNER JOIN attività ON enti.IDEnte = attività.IDEntePresentante " & _
                         "  INNER JOIN CheckListPagheCollettivo ON attività.CodiceEnte = CheckListPagheCollettivo.CodiceProgettoRendicontazione " & _
                         " WHERE CheckListPagheCollettivo.IdCheckList = " & IdcheckList & ""
                rstVol = ClsServer.CreaDatareader(strSql, Session("conn"))

                If rstVol.HasRows Then
                    rstVol.Read()
                    TxtCodCheeKList.Text = rstVol("CODICECHECKLIST")
                    lblCodEnte.Text = rstVol("CodiceRegione")
                    LblDenomin.Text = rstVol("Denominazione")
                    IdEnte = rstVol("IDEnte")
                    HiddenFieldIdEnte.Value = IdEnte
                End If

                If Not rstVol Is Nothing Then
                    rstVol.Close()
                    rstVol = Nothing
                End If
            End If


            If TipoNotifica = 4 Then
               

                ddlTipoNotifica.SelectedIndex = 4


                Dim strSql As String
                Dim rstVol As SqlClient.SqlDataReader
                'query per checkList
                strSql = "SELECT  enti.IDEnte, enti.Denominazione, enti.CodiceRegione, 'I' + CONVERT(VARCHAR, CheckListPagheIndividuale.IdCheckList) AS CODICECHECKLIST, CheckListPagheIndividuale.IdCheckList " & _
                         " FROM enti " & _
                         " INNER JOIN attività ON enti.IDEnte = attività.IDEntePresentante " & _
                         " INNER JOIN CheckListPagheIndividuale ON attività.CodiceEnte = CheckListPagheIndividuale.CodiceProgettoRendicontazione " & _
                         " WHERE CheckListPagheIndividuale.IdCheckList = " & IdcheckList & ""
                rstVol = ClsServer.CreaDatareader(strSql, Session("conn"))

                If rstVol.HasRows Then
                    rstVol.Read()
                    TxtCodCheeKList.Text = rstVol("CODICECHECKLIST")
                    lblCodEnte.Text = rstVol("CodiceRegione")
                    LblDenomin.Text = rstVol("Denominazione")
                    IdEnte = rstVol("IDEnte")
                    HiddenFieldIdEnte.Value = IdEnte
                End If



                If Not rstVol Is Nothing Then
                    rstVol.Close()
                    rstVol = Nothing
                End If




            End If

            If TipoNotifica = 1 Or TipoNotifica = 2 Then
                If TipoNotifica = 1 Then
                    ddlTipoNotifica.SelectedIndex = 1
                End If
                If TipoNotifica = 2 Then
                    ddlTipoNotifica.SelectedIndex = 2
                End If
                'TxtCodCheeKList.Enabled = False
                Dim strSql As String
                Dim rstVol As SqlClient.SqlDataReader
                'query per Entità
                strSql = "SELECT     enti.IDEnte, enti.Denominazione, enti.CodiceRegione, entità.CodiceVolontario " & _
                         " FROM entità " & _
                         " INNER JOIN attivitàentità ON entità.IDEntità = attivitàentità.IDEntità " & _
                         " INNER JOIN attivitàentisediattuazione ON attivitàentità.IDAttivitàEnteSedeAttuazione = attivitàentisediattuazione.IDAttivitàEnteSedeAttuazione " & _
                         " INNER JOIN attività ON attivitàentisediattuazione.IDAttività = attività.IDAttività " & _
                         " INNER JOIN enti ON attività.IDEntePresentante = enti.IDEnte " & _
                         " WHERE entità.identità = " & IdEntita & _
                         " AND attivitàentità.idstatoattivitàentità = 1"
                rstVol = ClsServer.CreaDatareader(strSql, Session("conn"))

                If rstVol.HasRows Then
                    rstVol.Read()
                    lblCodEnte.Text = rstVol("CodiceRegione")
                    LblDenomin.Text = rstVol("Denominazione")
                    txtCodVol.Text = rstVol("CodiceVolontario")
                    IdEnte = rstVol("IDEnte")
                    HiddenFieldIdEnte.Value = IdEnte
                End If



                If Not rstVol Is Nothing Then
                    rstVol.Close()
                    rstVol = Nothing
                End If




            End If

            If TipoNotifica = 5 Then
                ddlTipoNotifica.SelectedIndex = 5

                Dim strSql As String
                Dim rstVol As SqlClient.SqlDataReader
                'query per checkList
                strSql = "SELECT  enti.IDEnte, enti.Denominazione, enti.CodiceRegione, 'F' + CONVERT(VARCHAR, CheckListFormazione.IdCheckList) AS CODICECHECKLIST, CheckListFormazione.IdCheckList " & _
                         " FROM enti " & _
                         " INNER JOIN attività ON enti.IDEnte = attività.IDEntePresentante " & _
                         "  LEFT JOIN CheckListFormazione ON attività.CodiceEnte = CheckListFormazione.CodiceProgettoRendicontazione " & _
                         " WHERE attività.IdAttività = " & Idattivita & ""
                rstVol = ClsServer.CreaDatareader(strSql, Session("conn"))

                If rstVol.HasRows Then
                    rstVol.Read()
                    If Not IsDBNull(rstVol("CODICECHECKLIST")) Then
                        If rstVol("CODICECHECKLIST") <> "" Then
                            TxtCodCheeKList.Text = rstVol("CODICECHECKLIST")
                        End If
                    End If
                    lblCodEnte.Text = rstVol("CodiceRegione")
                    LblDenomin.Text = rstVol("Denominazione")
                    IdEnte = rstVol("IDEnte")
                    HiddenFieldIdEnte.Value = IdEnte
                Else
                    Exit Sub
                End If

                If Not rstVol Is Nothing Then
                    rstVol.Close()
                    rstVol = Nothing
                End If
            End If



            CaricaGriglia(IdEnte, TipoNotifica)
        End If

    End Sub
    Private Sub CaricaGriglia(ByVal IdEnte As Integer, ByVal TipoNotifica As String)




        '@IdEnte as int,									
        '@DataInvioDal as varchar(20),					
        '@DataInvioAl as varchar(20),					
        '@TipoNotifica as varchar(10) = '',				
        '@CodiceVolontario as varchar(255) = '',			
        '@CodiceChecklist as varchar(255) = '',			
        '@Oggetto as varchar(255) = '',					
        '@Testo as varchar(255) = '',					
        '@UsernameInviatore varchar(10) = ''



        Dim sqlDAP As New SqlClient.SqlDataAdapter
        Dim dataSet As New DataSet
        Dim strNomeStore As String = "[SP_NOTIFICA_RICERCA_STORICO]"

        Try
            sqlDAP = New SqlClient.SqlDataAdapter(strNomeStore, CType(Session("conn"), SqlClient.SqlConnection))
            sqlDAP.SelectCommand.CommandType = CommandType.StoredProcedure

            sqlDAP.SelectCommand.Parameters.Add("@IdEnte", SqlDbType.Int).Value = IdEnte
            sqlDAP.SelectCommand.Parameters.Add("@DataInvioDal", SqlDbType.VarChar).Value = TxtDataDal.Text
            sqlDAP.SelectCommand.Parameters.Add("@DataInvioAl", SqlDbType.VarChar).Value = txtDataAl.Text
            sqlDAP.SelectCommand.Parameters.Add("@TipoNotifica", SqlDbType.VarChar).Value = TipoNotifica
            sqlDAP.SelectCommand.Parameters.Add("@CodiceVolontario", SqlDbType.VarChar).Value = txtCodVol.Text
            sqlDAP.SelectCommand.Parameters.Add("@CodiceChecklist", SqlDbType.VarChar).Value = TxtCodCheeKList.Text
            sqlDAP.SelectCommand.Parameters.Add("@Oggetto", SqlDbType.VarChar).Value = txtOggetto.Text
            sqlDAP.SelectCommand.Parameters.Add("@Testo", SqlDbType.VarChar).Value = txtTesto.Text
            sqlDAP.SelectCommand.Parameters.Add("@UsernameInviatore", SqlDbType.VarChar).Value = ddlUserNameInvio.SelectedValue.ToString


            sqlDAP.Fill(dataSet)

            ' Session("appDtsRisRicerca") = dataSet
            dtgNotificheEffettuate.DataSource = dataSet
            dtgNotificheEffettuate.DataBind()
            Session("dtRisulatato") = dataSet
        Catch ex As Exception
            lblmess.Visible = True
            lblmess.Text = "Si è verificato un errore non gestito. Contattare l'assistenza."
            lblmess.ForeColor = Color.Red
            Exit Sub
        End Try

    End Sub

    Protected Sub cmdRicerca_Click(sender As Object, e As EventArgs) Handles cmdRicerca.Click
        Testobody.Visible = False
        dtgNotificheEffettuate.CurrentPageIndex = 0
        CaricaGriglia(HiddenFieldIdEnte.Value, ddlTipoNotifica.SelectedValue.ToString)
    End Sub

    Private Sub CaricaComboUser()
        Dim strSql As String
        Dim rstDoc As SqlClient.SqlDataReader


        ddlUserNameInvio.Items.Clear()
        strSql = "select 0 as ordina,'Selezionare' as accountad "
        strSql = strSql + "union select distinct 1,b.accountad from dbo.EntitàNotificheDocumenti as a inner join utentiunsc as b on a.UsernameInviatore = b.username "
        strSql = strSql + "union select distinct 1,b.accountad from dbo.CheckListPagheCollettivoNotifiche as a inner join utentiunsc as b on a.UsernameInviatore = b.username "
        strSql = strSql + "union select distinct 1,b.accountad from dbo.CheckListPagheIndividualeNotifiche as a inner join utentiunsc as b on a.UsernameInviatore = b.username "
        strSql = strSql + "order by ordina, accountad"
        rstDoc = ClsServer.CreaDatareader(strSql, Session("conn"))


        If rstDoc.HasRows Then
            ddlUserNameInvio.DataSource = rstDoc
            ddlUserNameInvio.DataTextField = "accountad"
            ddlUserNameInvio.DataValueField = "accountad"
            ddlUserNameInvio.DataBind()
        End If

        If Not rstDoc Is Nothing Then
            rstDoc.Close()
            rstDoc = Nothing
        End If
    End Sub

    Private Sub dtgNotificheEffettuate_ItemCommand(source As Object, e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dtgNotificheEffettuate.ItemCommand
        Select Case e.CommandName

            Case "Select"

                Dim SqlCmd As SqlClient.SqlCommand

                SqlCmd = New SqlClient.SqlCommand
                SqlCmd.CommandText = "SP_NOTIFICA_DETTAGLIO_STORICO"
                SqlCmd.CommandType = CommandType.StoredProcedure
                SqlCmd.Connection = Session("Conn")
                SqlCmd.Parameters.Add("@IdNotifica", SqlDbType.Int).Value = e.Item.Cells(1).Text
                SqlCmd.Parameters.Add("@IdTipoNotifica", SqlDbType.NVarChar).Value = e.Item.Cells(2).Text

                Dim sparam1 As SqlClient.SqlParameter
                sparam1 = New SqlClient.SqlParameter
                sparam1.ParameterName = "@Oggetto"
                sparam1.Size = 1000
                sparam1.SqlDbType = SqlDbType.NVarChar
                sparam1.Direction = ParameterDirection.Output
                SqlCmd.Parameters.Add(sparam1)

                Dim sparam2 As SqlClient.SqlParameter
                sparam2 = New SqlClient.SqlParameter
                sparam2.ParameterName = "@Testo"
                sparam2.Size = 4000
                sparam2.SqlDbType = SqlDbType.NVarChar
                sparam2.Direction = ParameterDirection.Output
                SqlCmd.Parameters.Add(sparam2)

                SqlCmd.ExecuteNonQuery()

                Testobody.Visible = True

                lblOggetto.Text = SqlCmd.Parameters("@Oggetto").Value
                lblTestoMail.Text = SqlCmd.Parameters("@Testo").Value


        End Select


    End Sub

    Protected Sub cmdChiudi_Click(sender As Object, e As EventArgs) Handles cmdChiudi.Click
        Response.Write("<script>" & vbCrLf)
        Response.Write("window.close()" & vbCrLf)
        Response.Write("</script>" & vbCrLf)
    End Sub
   
    Private Sub dtgNotificheEffettuate_PageIndexChanged(source As Object, e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dtgNotificheEffettuate.PageIndexChanged
        dtgNotificheEffettuate.SelectedIndex = -1
        dtgNotificheEffettuate.CurrentPageIndex = e.NewPageIndex
        dtgNotificheEffettuate.DataSource = Session("dtRisulatato")
        dtgNotificheEffettuate.DataBind()
    End Sub
End Class