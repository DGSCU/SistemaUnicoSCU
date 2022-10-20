Imports System.Data.SqlClient
Public Class WfrmRicercaRequisitiVerifiche
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Session("LogIn") Is Nothing Then
            If Session("LogIn") = False Then 'verifico validità log-in
                Response.Redirect("LogOn.aspx")
            End If
        Else
            Response.Redirect("LogOn.aspx")
        End If
        If Page.IsPostBack = False Then

            LoadVersioneRequisiti(Request.QueryString("IDVersioneVerifiche"))
            StatoVersione(Request.QueryString("StatoVersione"))

        End If
    End Sub
    Sub LoadVersioneRequisiti(ByVal IDVersioneVerifiche As Integer)

        SP_VER_VERSIONIREQUISITI(IDVersioneVerifiche, Session("Utente"))

    End Sub
    Private Function SP_VER_VERSIONIREQUISITI(ByVal IDVersioneVerifiche As Integer, ByVal Utente As String)

        Dim sqlCMD As New SqlCommand
        Dim sqlAdapter As SqlClient.SqlDataAdapter
        Dim DsElencoReq As DataSet = New DataSet

        Try
            sqlAdapter = New SqlClient.SqlDataAdapter("SP_VER_VERSIONIREQUISITI", CType(Session("conn"), SqlClient.SqlConnection))
            sqlAdapter.SelectCommand.CommandType = CommandType.StoredProcedure
            sqlAdapter.SelectCommand.Parameters.Add("@IDVersione", SqlDbType.Int).Value = IDVersioneVerifiche
            sqlAdapter.SelectCommand.Parameters.Add("@UserName", SqlDbType.NVarChar).Value = Utente

            Dim sparam1 As SqlClient.SqlParameter
            sparam1 = New SqlClient.SqlParameter
            sparam1.ParameterName = "@esito"
            sparam1.Size = 100
            sparam1.SqlDbType = SqlDbType.NVarChar
            sparam1.Direction = ParameterDirection.Output
            sqlAdapter.SelectCommand.Parameters.Add(sparam1)

            Dim sparam2 As SqlClient.SqlParameter
            sparam2 = New SqlClient.SqlParameter
            sparam2.ParameterName = "@Versione"
            sparam2.Size = 100
            sparam2.SqlDbType = SqlDbType.NVarChar
            sparam2.Direction = ParameterDirection.Output
            sqlAdapter.SelectCommand.Parameters.Add(sparam2)

            Dim sparam3 As SqlClient.SqlParameter
            sparam3 = New SqlClient.SqlParameter
            sparam3.ParameterName = "@Note"
            sparam3.Size = 1000
            sparam3.SqlDbType = SqlDbType.NVarChar
            sparam3.Direction = ParameterDirection.Output
            sqlAdapter.SelectCommand.Parameters.Add(sparam3)

            sqlAdapter.Fill(DsElencoReq)
            Session("dtsElencoRequisiti") = DsElencoReq
            If DsElencoReq.Tables(0).Rows.Count > 0 Then
                dgElencoRequisiti.DataSource = DsElencoReq.Tables(0)
                dgElencoRequisiti.DataBind()
            End If
            If IsDBNull(sqlAdapter.SelectCommand.Parameters("@Versione").Value) Then
                txtDescrizione.Text = ""
            Else
                txtDescrizione.Text = sqlAdapter.SelectCommand.Parameters("@Versione").Value
            End If
            txtNote.Text = sqlAdapter.SelectCommand.Parameters("@Note").Value

        Catch ex As Exception
            Response.Write(ex.Message.ToString())
            Exit Function
        End Try

    End Function

    Private Sub dgElencoRequisiti_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgElencoRequisiti.ItemCommand
        Try
            Select Case e.CommandName
                Case "Seleziona"
                    Response.Redirect("WfrmGestioneRequisiti.aspx?TipoAzione=Dettaglio&IDVersioneVerifiche=" + Request.QueryString("IDVersioneVerifiche") + "&IdRequisito=" + e.Item.Cells(1).Text + "&StatoVersione=" + Request.QueryString("StatoVersione"))
                Case "Cancella"
                    CancellaVersione(e.Item.Cells(1).Text, Request.QueryString("IDVersioneVerifiche"))
                    dgElencoRequisiti.CurrentPageIndex = 0
                    LoadVersioneRequisiti(Request.QueryString("IDVersioneVerifiche"))
            End Select
        Catch ex As Exception
            Response.Write(ex.Message.ToString())
        End Try
    End Sub



    Sub SalvaVersione(ByVal IDVersioneVerifiche As Integer)
        Dim cmdSalva As SqlCommand
        Dim strSql As String
        Try
            lblmsgVersioneErrore.Text = ""
            lblmsgVersione.Text = ""

            strSql = "UPDATE TVerificheVersioni SET  " & _
                     " Descrizione ='" & txtDescrizione.Text.Replace("'", "''") & "'," & _
                     " Note ='" & txtNote.Text.Replace("'", "''") & "' " & _
                     " WHERE IDVersioneVerifiche = " & IDVersioneVerifiche
            cmdSalva = ClsServer.EseguiSqlClient(strSql, Session("conn"))
            lblmsgVersione.Visible = True
            lblmsgVersione.Text = "Salvataggio eseguito con successo."
        Catch ex As Exception

            lblmsgVersioneErrore.Visible = True
            lblmsgVersioneErrore.Text = "Errore imprevisto.Contattare l'assistenza."
            'Response.Write(ex.Message.ToString())
        End Try
    End Sub

    Sub CancellaVersione(ByVal IDRequisito As Integer, ByVal IDVersioneVerifiche As Integer)
        Dim _IdVersione As New SqlParameter("@IdVersione", IDVersioneVerifiche)
        Dim _IDRequisito As New SqlParameter("@IDRequisito", IDRequisito)
        Dim sqlCMD As New SqlCommand

        Try
            sqlCMD.Connection = (Session("conn"))
            sqlCMD.CommandType = CommandType.StoredProcedure

            sqlCMD.CommandText = "SP_VER_ELIMINAREQUISITO"
            sqlCMD.Parameters.Add(_IdVersione)
            sqlCMD.Parameters.Add(_IDRequisito)


            Dim sparam1 As SqlClient.SqlParameter
            sparam1 = New SqlClient.SqlParameter
            sparam1.ParameterName = "@esito"
            sparam1.Size = 50
            sparam1.SqlDbType = SqlDbType.NVarChar
            sparam1.Direction = ParameterDirection.Output
            sqlCMD.Parameters.Add(sparam1)

            Dim Reader As SqlClient.SqlDataReader
            Reader = sqlCMD.ExecuteReader()
            Dim sReturnValue As String
            If IsDBNull(sqlCMD.Parameters("@esito").Value) Then
                sReturnValue = ""
            Else
                sReturnValue = sqlCMD.Parameters("@esito").Value
            End If

            Reader.Close()
            Reader = Nothing

            If sReturnValue <> "" Then
                lblmsgRequisiti.Visible = True
                lblmsgRequisiti.Text = sReturnValue
            Else
                lblmsgRequisiti.Visible = True
                lblmsgRequisiti.Text = "Cancellazione eseguita con successo."
            End If
        Catch ex As Exception
            'Response.Write(ex.Message.ToString())
            lblmsgErrore.Visible = True
            lblmsgErrore.Text = "Errore imprevisto.Contattare l'assistenza."
        End Try
    End Sub

    Private Function DuplicaVersione(ByVal IDVersioneVerifiche As Integer, ByVal Utente As String) As Integer
        Dim _IdVersione As New SqlParameter("@IdVersione", IDVersioneVerifiche)
        Dim _Utente As New SqlParameter("@UserName", Utente)
        Dim sqlCMD As New SqlCommand
        Dim IdNuovaVersione As Integer
        Try
            sqlCMD.Connection = (Session("conn"))
            sqlCMD.CommandType = CommandType.StoredProcedure

            sqlCMD.CommandText = "SP_VER_DUPLICAZIONEVERSIONE"
            sqlCMD.Parameters.Add(_IdVersione)
            sqlCMD.Parameters.Add(_Utente)


            Dim sparam1 As SqlClient.SqlParameter
            sparam1 = New SqlClient.SqlParameter
            sparam1.ParameterName = "@esito"
            sparam1.Size = 100
            sparam1.SqlDbType = SqlDbType.NVarChar
            sparam1.Direction = ParameterDirection.Output
            sqlCMD.Parameters.Add(sparam1)

            Dim sparam2 As SqlClient.SqlParameter
            sparam2 = New SqlClient.SqlParameter
            sparam2.ParameterName = "@IDNuovaVersione"
            sparam2.Size = 10
            sparam2.SqlDbType = SqlDbType.Int
            sparam2.Direction = ParameterDirection.Output
            sqlCMD.Parameters.Add(sparam2)

            Dim Reader As SqlClient.SqlDataReader
            Reader = sqlCMD.ExecuteReader()

            Dim sReturnValue As String
            If IsDBNull(sqlCMD.Parameters("@esito").Value) Then
                sReturnValue = ""
            Else
                sReturnValue = sqlCMD.Parameters("@esito").Value
            End If
            IdNuovaVersione = sqlCMD.Parameters("@IDNuovaVersione").Value

            Reader.Close()
            Reader = Nothing

            If sReturnValue <> "" Then
                lblmsgRequisiti.Visible = True
                lblmsgRequisiti.Text = sReturnValue
            Else
                lblmsgRequisiti.Visible = True
                lblmsgRequisiti.Text = "Duplicazione eseguito con successo."
            End If
            Return IdNuovaVersione
        Catch ex As Exception
            'Response.Write(ex.Message.ToString())
            lblmsgErrore.Visible = True
            lblmsgErrore.Text = "Errore imprevisto.Contattare l'assistenza."
        End Try
    End Function


    Sub StatoVersione(ByVal Stato As Integer)
        Select Case Stato
            Case 0 'registrata
                txtDescrizione.ReadOnly = False
                txtNote.ReadOnly = False
                cmdSalva.Visible = True
                cmdNuovo.Visible = True
                cmdRevisione.Visible = False
                dgElencoRequisiti.Columns(4).Visible = True
            Case 1 'associata 
                txtDescrizione.ReadOnly = True
                txtNote.ReadOnly = True
                cmdSalva.Visible = False
                cmdRevisione.Visible = True
                cmdNuovo.Visible = False
                dgElencoRequisiti.Columns(4).Visible = False
        End Select
    End Sub

    Private Sub dgElencoRequisiti_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgElencoRequisiti.PageIndexChanged
        dgElencoRequisiti.CurrentPageIndex = e.NewPageIndex
        dgElencoRequisiti.DataSource = Session("dtsElencoRequisiti")
        dgElencoRequisiti.DataBind()
        dgElencoRequisiti.SelectedIndex = -1
    End Sub


    Protected Sub cmdSalva_Click(sender As Object, e As EventArgs) Handles cmdSalva.Click
        If txtDescrizione.Text = "" Then
            lblmsgVersione.Visible = True
            lblmsgVersione.Text = "Inserire la descrizione della versione."
            Exit Sub
        End If
        If Session("intIdNuovaVersione") <> Nothing Then
            SalvaVersione(Session("intIdNuovaVersione"))
        Else
            SalvaVersione(Request.QueryString("IDVersioneVerifiche"))
        End If
        'SalvaVersione(Request.QueryString("IDVersioneVerifiche"))
    End Sub


    Protected Sub cmdNuovo_Click(sender As Object, e As EventArgs) Handles cmdNuovo.Click
        'Response.Redirect("WfrmGestioneRequisiti.aspx?)
        Response.Redirect("WfrmGestioneRequisiti.aspx?TipoAzione=Inserimento&IDVersioneVerifiche=" + Request.QueryString("IDVersioneVerifiche") + "&StatoVersione=" + Request.QueryString("StatoVersione"))
    End Sub

    Protected Sub cmdRevisione_Click(sender As Object, e As EventArgs) Handles cmdRevisione.Click
        Session("intIdNuovaVersione") = DuplicaVersione(Request.QueryString("IDVersioneVerifiche"), Session("Utente"))
        txtDescrizione.Text = ""
        txtNote.Text = ""
        dgElencoRequisiti.CurrentPageIndex = 0
        LoadVersioneRequisiti(Session("intIdNuovaVersione"))
        StatoVersione(0)
    End Sub

    Protected Sub cmdChiudi_Click(sender As Object, e As EventArgs) Handles cmdChiudi.Click
         'Response.Redirect("WfrmMain.aspx")
        Response.Redirect("WfrmVerVersioni.aspx")
    End Sub

End Class