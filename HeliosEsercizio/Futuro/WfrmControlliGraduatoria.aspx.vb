Imports System.Data.SqlClient
Public Class WfrmControlliGraduatoria
    Inherits System.Web.UI.Page

   

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

    Private Sub NascondiMenuLaterale()
        Session("TP") = True
    End Sub

#End Region
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        VerificaSessione()
        If IsPostBack = False Then
            CaricaGriglie(Request.QueryString("IdAttivita"), Request.QueryString("DataAvvio"))
        End If
    End Sub

    Private Sub CaricaGriglie(ByVal IdAttivita As Integer, ByVal DataAvvio As String)

        Dim sqlDAP As SqlClient.SqlDataAdapter
        Dim dataSet As New DataSet
        Dim strNomeStore As String = "[SP_GRADUATORIA_CONTROLLI_V3]"

        Try
            sqlDAP = New SqlClient.SqlDataAdapter(strNomeStore, CType(Session("conn"), SqlClient.SqlConnection))
            sqlDAP.SelectCommand.CommandType = CommandType.StoredProcedure

            sqlDAP.SelectCommand.Parameters.Add("@IdAttivita", SqlDbType.Int).Value = IdAttivita
            sqlDAP.SelectCommand.Parameters.Add("@DataAvvio", SqlDbType.Date).Value = CDate(DataAvvio)
            If Not Request.QueryString("IdAttivitaEnteSedeAttuazione") Is Nothing Then
                sqlDAP.SelectCommand.Parameters.Add("@IDAttivitàEnteSedeAttuazione", SqlDbType.Int).Value = Request.QueryString("IdAttivitaEnteSedeAttuazione")
            Else
                sqlDAP.SelectCommand.Parameters.Add("@IDAttivitàEnteSedeAttuazione", SqlDbType.Int).Value = -1
            End If

            Dim sparam1 As SqlClient.SqlParameter
            sparam1 = New SqlClient.SqlParameter
            sparam1.ParameterName = "@Esito"
            sparam1.Size = 100
            sparam1.SqlDbType = SqlDbType.NVarChar
            sparam1.Direction = ParameterDirection.Output
            sqlDAP.SelectCommand.Parameters.Add(sparam1)

            sqlDAP.Fill(dataSet)

            For i = 0 To dataSet.Tables.Count
                If dataSet.Tables(i).Rows.Count > 0 Then
                    Select Case i
                        Case 0
                            DivControlloCapienzaSedi.Visible = True
                            dtgControlloCapienzaSedi.DataSource = dataSet.Tables(i)
                            dtgControlloCapienzaSedi.DataBind()
                        Case 1
                            DivControlloOLP.Visible = True
                            dtgControlloOLP.DataSource = dataSet.Tables(i)
                            dtgControlloOLP.DataBind()
                        Case 2
                            DivControlloOLP1.Visible = True
                            dtgControlloOLP1.DataSource = dataSet.Tables(i)
                            dtgControlloOLP1.DataBind()
                        Case 3
                            DivControlloOLP2.Visible = True
                            dtgControlloOLP2.DataSource = dataSet.Tables(i)
                            dtgControlloOLP2.DataBind()
                        Case 4
                            DivControlloRLEA.Visible = True
                            dtgControlloRLEA.DataSource = dataSet.Tables(i)
                            dtgControlloRLEA.DataBind()
                    End Select
                End If

            Next


        Catch ex As Exception
            'LblMsgFile.Visible = True
            'LblMsgFile.Text = "Si è verificato un errore non gestito. Contattare l'assistenza."
            'LblMsgFile.ForeColor = Color.Red
            Exit Sub
        End Try
    End Sub

    Protected Sub cmdChiudi_Click(sender As Object, e As EventArgs) Handles cmdChiudi.Click

    End Sub

    Protected Sub cmdSbloccaProgetto_Click(sender As Object, e As EventArgs) Handles cmdSbloccaProgetto.Click

        Dim myCommand As System.Data.SqlClient.SqlCommand
        Dim strsql As String

        myCommand = New System.Data.SqlClient.SqlCommand
        myCommand.Connection = Session("conn")

        strsql = "insert into EccezioniControlliGraduatorie (CodiceProgetto,Richiedente,DataRichiesta,Note) " & _
                 "select CodiceEnte,'" & Session("Utente") & "',getdate(),'richiesta forzatura per confermare la graduatoria' " & _
                 "from attività " & _
                 "where IDAttività = " & Request.QueryString("IdAttivita") & ""

        myCommand.CommandText = strsql
        myCommand.ExecuteNonQuery()

        lblMessaggioAlert.Visible = True
        lblMessaggioAlert.Text = "Sblocco progetto avvenuto con successo. Ora è possibile confermare le graduatorie."

    End Sub
End Class