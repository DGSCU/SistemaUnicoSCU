Public Class WfrmConfermaGeneralePresenzeMensili
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        lblValEnte.Text = Session("txtCodEnte")
        lblValProg.Text = Session("Denominazione")
        If Page.IsPostBack = False Then
            CaricaGriglietta()
        End If

    End Sub
    Sub CaricaGriglietta()
        Dim i As Integer

        DtgMesiDaInserire.DataSource = LoadStorePresenze(Session("idEnte"))
        DtgMesiDaInserire.DataBind()
        For i = 0 To DtgMesiDaInserire.Items.Count - 1
            If DtgMesiDaInserire.Items(i).Cells(3).Text = "Da Confermare" Then
                CmdConfermaPresenze.Visible = True

                Exit Sub

            End If
        Next
    End Sub

    Private Sub DtgMesiDaInserire_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles DtgMesiDaInserire.ItemCommand

        If e.CommandName = "Select" Then
            Dim MeseSelezionato As Integer
            Dim AnnoSelezionato As Integer
            MeseSelezionato = CType(e.Item.Cells(0).Text, Integer)
            AnnoSelezionato = CType(e.Item.Cells(2).Text, Integer)

            'Response.Write("<script>" & vbCrLf)
            'Response.Write("window.open(""WfrmVisualizzaPresenzeMeseVol.aspx?MeseSel=" & MeseSelezionato & "&AnnoSel=" & AnnoSelezionato & """, ""Visualizza"", ""width=900,height=400,dependent=no,scrollbars=yes,status=no"")" & vbCrLf)
            'Response.Write("</script>")
            Response.Redirect("WfrmVisualizzaPresenzeMeseVol.aspx?MeseSel=" & MeseSelezionato & "&AnnoSel=" & AnnoSelezionato)
        End If

    End Sub
    Protected Sub cmdChiudi_Click(sender As Object, e As EventArgs) Handles cmdChiudi.Click
        Response.Redirect("WfrmMain.aspx")
    End Sub
    Private Function LoadStorePresenze(ByVal IdEnte As Integer) As DataSet
        Dim sqlDAP As New SqlClient.SqlDataAdapter
        Dim dataSet As New DataSet
        Dim strNomeStore As String = "[SP_PRESENZE_STATO_CONFERMA_MENSILE]"

        Try

            sqlDAP = New SqlClient.SqlDataAdapter(strNomeStore, CType(Session("conn"), SqlClient.SqlConnection))
            sqlDAP.SelectCommand.CommandType = CommandType.StoredProcedure
            sqlDAP.SelectCommand.Parameters.Add("@Idente", SqlDbType.Int).Value = IdEnte
            'sqlDAP.SelectCommand.Parameters.Add("@ElencoCursore", ).Direction = ParameterDirection.Output

            sqlDAP.Fill(dataSet)

            ' dataSet.Tables(0).Row

            Return dataSet

        Catch ex As Exception
            Response.Write(ex.Message.ToString())
            Exit Function
        End Try
    End Function

    Protected Sub CmdConfermaPresenze_Click(sender As Object, e As EventArgs) Handles CmdConfermaPresenze.Click
        Dim mioArray As String()
        Dim Valore1 As String
        Dim Valore2 As String


        mioArray = EseguiPresenzeConferma()

        Valore1 = mioArray(0).ToString
        Valore2 = mioArray(1).ToString
        If Valore1 = "POSITIVO" Then

            CaricaGriglietta()
            CmdConfermaPresenze.Visible = False

        Else
            lblNoVol.Visible = True
            lblNoVol.Text = Valore2
            Exit Sub
        End If


      
    End Sub
    Public Function EseguiPresenzeConferma() As String()
        'AUTORE: Antonello Di Croce  
        'DESCRIZIONE: richiamo la P_PRESENZE_CONFERMA_MESE 
        'DATA: 20/03/2015
        Dim Reader As SqlClient.SqlDataReader


        Try
            Dim x As String
            Dim y As String
            Dim ArreyOutPut(1) As String


            Dim MyCommand As New SqlClient.SqlCommand
            MyCommand.CommandType = CommandType.StoredProcedure
            MyCommand.CommandText = "SP_PRESENZE_CONFERMA_MESE"
            MyCommand.Connection = Session("conn")

            'PRIMO PARAMEtrO INPUT 
            Dim sparam As SqlClient.SqlParameter
            sparam = New SqlClient.SqlParameter
            sparam.ParameterName = "@IdEnte"
            sparam.SqlDbType = SqlDbType.Int
            MyCommand.Parameters.Add(sparam)


            'SECONDO PARAMEtrO INPUT 
            Dim sparam1 As SqlClient.SqlParameter
            sparam1 = New SqlClient.SqlParameter
            sparam1.ParameterName = "@Username"
            sparam1.SqlDbType = SqlDbType.NVarChar
            MyCommand.Parameters.Add(sparam1)


            'PARAMEtrO 1=esito OUTPUT
            Dim sparam2 As SqlClient.SqlParameter
            sparam2 = New SqlClient.SqlParameter
            sparam2.ParameterName = "@Esito"
            sparam2.Size = 10
            sparam2.SqlDbType = SqlDbType.VarChar
            sparam2.Direction = ParameterDirection.Output
            MyCommand.Parameters.Add(sparam2)


            'PARAMEtrO 2=Messaggio OUTPUT
            Dim sparam3 As SqlClient.SqlParameter
            sparam3 = New SqlClient.SqlParameter
            sparam3.ParameterName = "@Messaggio"
            sparam3.Size = 255
            sparam3.SqlDbType = SqlDbType.NVarChar
            sparam3.Direction = ParameterDirection.Output
            MyCommand.Parameters.Add(sparam3)


            MyCommand.Parameters("@IdEnte").Value = Session("idEnte")
            MyCommand.Parameters("@Username").Value = Session("Utente")


            Reader = MyCommand.ExecuteReader()

            x = CStr(MyCommand.Parameters("@Esito").Value)
            y = MyCommand.Parameters("@Messaggio").Value


            ArreyOutPut(0) = x
            ArreyOutPut(1) = y

            Reader.Close()
            Reader = Nothing

            Return ArreyOutPut

        Catch ex As Exception
            If Not Reader Is Nothing Then
                Reader.Close()
                Reader = Nothing
            End If
            Dim ArreyOutPut1(1) As String
            ArreyOutPut1(0) = "0"
            ArreyOutPut1(1) = "Contattare l'assistenza Helios/Futuro"
            Return ArreyOutPut1

        End Try
    End Function
End Class