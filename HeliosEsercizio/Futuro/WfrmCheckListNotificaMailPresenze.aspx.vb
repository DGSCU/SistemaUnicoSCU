Imports System
Imports System.Web
Imports System.IO
Public Class WfrmCheckListNotificaMailPresenze
    Inherits System.Web.UI.Page
    Dim cmdCommand As SqlClient.SqlCommand
    Dim myCommand As System.Data.SqlClient.SqlCommand
    Dim strsql As String
    Dim dtrGenerico As SqlClient.SqlDataReader
    Dim dtsGenerico As DataSet
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        If Page.IsPostBack = False Then
            If Request.QueryString("VengoDa") = 1 Then
                ImpostaEmailCHECKLISTANOMALIAVOLONTARIO(1)
            End If
            If Request.QueryString("VengoDa") = 2 Then
                ImpostaEmailCHECKLISTANOMALIAVOLONTARIO(2)
            End If
            If Request.QueryString("VengoDa") = 3 Then
                ImpostaEmailCHECKLISTANOMALIAVOLONTARIO(3)
                'ImpostaEmailCHECKLISTRimborso(3)
            End If
            If Request.QueryString("VengoDa") = 5 Then
                ImpostaEmailPresenze(Request.QueryString("Mese"), Request.QueryString("Anno"))
            End If
            If Request.QueryString("VengoDa") = 4 Then
                ImpostaEmail()
            End If
            If Request.QueryString("VengoDa") = 6 Then
                ImpostaEmailCHECKLISTANOMALIAVOLONTARIO(6)
                'ImpostaEmailCHECKLISTRimborso(3)
            End If

        End If


    End Sub

    Protected Sub CmdInvioEmail_Click(sender As Object, e As EventArgs) Handles CmdInvioEmail.Click
        Dim Testo As String
        Dim ProvenienzaPagina As Integer
        ProvenienzaPagina = Request.QueryString("VengoDa")

        Select Case ProvenienzaPagina

            Case 1 ' Vengo Dalla pagina WfrmCheckListDettaglio.aspx
               Dim miotesto As String
                miotesto = Replace(txtNote.Text, vbLf, "<br />")

                InviaEmailCheckList(miotesto, 1)

                Response.Write("<script>window.close();</" + "script>")
                Response.End()

            Case 2  ' Vengo Dalla pagina WfrmCheckListDettaglioIndividuale.aspx
                 Dim miotesto As String
                miotesto = Replace(txtNote.Text, vbLf, "<br />")

                InviaEmailCheckList(miotesto, 2)

                Response.Write("<script>window.close();</" + "script>")
                Response.End()
            Case 3  'Vengo Dalla pagina wfrmCheckListDettaglioRimborsoViaggio.aspx
                 Dim miotesto As String
                miotesto = Replace(txtNote.Text, vbLf, "<br />")

                InviaEmailCheckList(miotesto, 3)

                Response.Write("<script>window.close();</" + "script>")
                Response.End()


            Case 4 ' vengoda WfrmVisualizzaElencoDocumentiVolontario.aspx
                Dim miotesto As String
                miotesto = Replace(txtNote.Text, vbLf, "<br />")

                InviaEmail(miotesto)

                Response.Write("<script>window.close();</" + "script>")
                Response.End()

            Case 5 ' vengoda presenze.aspx
                Dim miotesto As String
                miotesto = Replace(txtNote.Text, vbLf, "<br />")

                InviaEmail(miotesto)

                Response.Write("<script>window.close();</" + "script>")
                Response.End()
            Case 6 ' vengoda WfrmCheckListDettaglioFormazione
                Dim miotesto As String
                miotesto = Replace(txtNote.Text, vbLf, "<br />")

                InviaEmailCheckList(miotesto, 6)

                Response.Write("<script>window.close();</" + "script>")
                Response.End()
        End Select

    End Sub

    Protected Sub CmdChiudi_Click(sender As Object, e As EventArgs) Handles CmdChiudi.Click
        Response.Write("<script>window.close();</" + "script>")
        Response.End()
    End Sub

    Private Sub ImpostaEmail()
        Dim sqlCMD As New SqlClient.SqlCommand
        Dim strNomeStore As String = "[SP_NOTIFICA_DOCUMENTI_RICHIEDI]"

        Try
            Dim x As String
            Dim y As String
            Dim z As String
            sqlCMD = New SqlClient.SqlCommand(strNomeStore, CType(Session("conn"), SqlClient.SqlConnection))
            sqlCMD.CommandType = CommandType.StoredProcedure

            sqlCMD.Parameters.Add("@IdEntita", SqlDbType.Int).Value = Request.QueryString("IdVol")


            Dim sparam1 As SqlClient.SqlParameter
            sparam1 = New SqlClient.SqlParameter
            sparam1.ParameterName = "@Destinatario"
            sparam1.Size = 255
            sparam1.SqlDbType = SqlDbType.NVarChar
            sparam1.Direction = ParameterDirection.Output
            sqlCMD.Parameters.Add(sparam1)

            Dim sparam2 As SqlClient.SqlParameter
            sparam2 = New SqlClient.SqlParameter
            sparam2.ParameterName = "@Oggetto"
            sparam2.Size =
            sparam2.SqlDbType = SqlDbType.NVarChar
            sparam2.Direction = ParameterDirection.Output
            sqlCMD.Parameters.Add(sparam2)


            Dim sparam3 As SqlClient.SqlParameter
            sparam3 = New SqlClient.SqlParameter
            sparam3.ParameterName = "@Testo"
            sparam3.Size = 4000
            sparam3.SqlDbType = SqlDbType.NVarChar
            sparam3.Direction = ParameterDirection.Output
            sqlCMD.Parameters.Add(sparam3)


            sqlCMD.ExecuteScalar()
            'Dim str As String
            'str = sqlCMD.Parameters("@Messaggio").Value

            x = sqlCMD.Parameters("@Destinatario").Value
            y = sqlCMD.Parameters("@Oggetto").Value
            z = sqlCMD.Parameters("@Testo").Value


            If x = "POSITIVO" Then



            End If



            'Dim myString As String

            'myString = z
            'Dim myEncodedString As String
            '' Encode the string.
            'myEncodedString = HttpUtility.HtmlEncode(myString)

            'Dim myWriter As New StringWriter()
            '' Decode the encoded string.
            'HttpUtility.HtmlDecode(myEncodedString, myWriter)


            txtNote.Text = z

            lblTo.Text = x
            lblOggetto.Text = y
            'txtNote.Text = RemoveHtml(z)



        Catch ex As Exception


            Response.Write(ex.Message.ToString())
            Exit Sub
        End Try
    End Sub

    Private Sub ImpostaEmailPresenze(ByVal Mese As String, ByVal Anno As String)
        Dim sqlCMD As New SqlClient.SqlCommand
        Dim strNomeStore As String = "[SP_NOTIFICA_PRESENZE_RICHIEDI]"

        Try
            Dim x As String
            Dim y As String
            Dim z As String
            sqlCMD = New SqlClient.SqlCommand(strNomeStore, CType(Session("conn"), SqlClient.SqlConnection))
            sqlCMD.CommandType = CommandType.StoredProcedure

            sqlCMD.Parameters.Add("@IdEntita", SqlDbType.Int).Value = Request.QueryString("IdVol")
            sqlCMD.Parameters.Add("@Mese", SqlDbType.VarChar).Value = Mese
            sqlCMD.Parameters.Add("@Anno", SqlDbType.VarChar).Value = Anno

            Dim sparam1 As SqlClient.SqlParameter
            sparam1 = New SqlClient.SqlParameter
            sparam1.ParameterName = "@Destinatario"
            sparam1.Size = 255
            sparam1.Direction = ParameterDirection.Output
            sparam1.SqlDbType = SqlDbType.NVarChar
            sqlCMD.Parameters.Add(sparam1)

            Dim sparam2 As SqlClient.SqlParameter
            sparam2 = New SqlClient.SqlParameter
            sparam2.ParameterName = "@Oggetto"
            sparam2.Size =
            sparam2.SqlDbType = SqlDbType.NVarChar
            sparam2.Direction = ParameterDirection.Output
            sqlCMD.Parameters.Add(sparam2)


            Dim sparam3 As SqlClient.SqlParameter
            sparam3 = New SqlClient.SqlParameter
            sparam3.ParameterName = "@Testo"
            sparam3.Size = 4000
            sparam3.SqlDbType = SqlDbType.NVarChar
            sparam3.Direction = ParameterDirection.Output
            sqlCMD.Parameters.Add(sparam3)


            sqlCMD.ExecuteScalar()

            x = sqlCMD.Parameters("@Destinatario").Value
            y = sqlCMD.Parameters("@Oggetto").Value
            z = sqlCMD.Parameters("@Testo").Value

            If x = "POSITIVO" Then

            End If

            txtNote.Text = z

            lblTo.Text = x
            lblOggetto.Text = y

        Catch ex As Exception
            Response.Write(ex.Message.ToString())
            Exit Sub
        End Try
    End Sub

    Public Function RemoveHtml(text)
        Return Regex.Replace(text, "<[^>]*>", String.Empty)
    End Function

    Private Sub InviaEmail(ByRef testo As String)
        Dim sqlCMD As New SqlClient.SqlCommand
        Dim strNomeStore As String = "[SP_NOTIFICA_DOCUMENTI_INVIA]"

        Try
            Dim x As String
            Dim y As String
            Dim z As String
            sqlCMD = New SqlClient.SqlCommand(strNomeStore, CType(Session("conn"), SqlClient.SqlConnection))
            sqlCMD.CommandType = CommandType.StoredProcedure

            sqlCMD.Parameters.Add("@IdEntita", SqlDbType.Int).Value = Request.QueryString("IdVol")
            sqlCMD.Parameters.Add("@Destinatario", SqlDbType.VarChar).Value = lblTo.Text
            sqlCMD.Parameters.Add("@Oggetto", SqlDbType.VarChar).Value = lblOggetto.Text
            sqlCMD.Parameters.Add("@Testo", SqlDbType.VarChar).Value = testo
            sqlCMD.Parameters.Add("@Username", SqlDbType.VarChar).Value = Session("Utente")



            sqlCMD.ExecuteScalar()


            CmdInvioEmail.Visible = False

        Catch ex As Exception


            Response.Write(ex.Message.ToString())
            Exit Sub
        End Try
    End Sub
    Private Sub ImpostaEmailCHECKLISTANOMALIAVOLONTARIO(ByVal tipologia As Integer)
        Dim sqlCMD As New SqlClient.SqlCommand
        Dim strNomeStore As String = "[SP_NOTIFICA_CHECKLIST_ANOMALIA_VOLONTARIO]"

        Try
            Dim x As String
            Dim y As String
            Dim z As String
            sqlCMD = New SqlClient.SqlCommand(strNomeStore, CType(Session("conn"), SqlClient.SqlConnection))
            sqlCMD.CommandType = CommandType.StoredProcedure

            sqlCMD.Parameters.Add("@IdEntita", SqlDbType.Int).Value = Request.QueryString("IdEntita")
            sqlCMD.Parameters.Add("@IdCheckList", SqlDbType.VarChar).Value = Request.QueryString("Idlista")
            sqlCMD.Parameters.Add("@TipoCheckList", SqlDbType.VarChar).Value = tipologia
            'sqlCMD.Parameters.Add("@Anno", SqlDbType.VarChar).Value = Anno

            Dim sparam1 As SqlClient.SqlParameter
            sparam1 = New SqlClient.SqlParameter
            sparam1.ParameterName = "@Destinatario"
            sparam1.Size = 255
            sparam1.Direction = ParameterDirection.Output
            sparam1.SqlDbType = SqlDbType.NVarChar
            sqlCMD.Parameters.Add(sparam1)

            Dim sparam2 As SqlClient.SqlParameter
            sparam2 = New SqlClient.SqlParameter
            sparam2.ParameterName = "@Oggetto"
            sparam2.Size =
            sparam2.SqlDbType = SqlDbType.NVarChar
            sparam2.Direction = ParameterDirection.Output
            sqlCMD.Parameters.Add(sparam2)


            Dim sparam3 As SqlClient.SqlParameter
            sparam3 = New SqlClient.SqlParameter
            sparam3.ParameterName = "@Testo"
            sparam3.Size = 4000
            sparam3.SqlDbType = SqlDbType.NVarChar
            sparam3.Direction = ParameterDirection.Output
            sqlCMD.Parameters.Add(sparam3)


            sqlCMD.ExecuteScalar()

            x = sqlCMD.Parameters("@Destinatario").Value
            y = sqlCMD.Parameters("@Oggetto").Value
            z = sqlCMD.Parameters("@Testo").Value

            If x = "POSITIVO" Then

            End If

            txtNote.Text = z

            lblTo.Text = x
            lblOggetto.Text = y

        Catch ex As Exception
            Response.Write(ex.Message.ToString())
            Exit Sub
        End Try
    End Sub
    Private Sub InviaEmailCheckList(ByRef testo As String, ByVal tipologia As Integer)
        Dim sqlCMD As New SqlClient.SqlCommand
        Dim strNomeStore As String = "[SP_NOTIFICA_CHECKLIST_INVIA]"

        Try
            Dim x As String
            Dim y As String
            Dim z As String
            sqlCMD = New SqlClient.SqlCommand(strNomeStore, CType(Session("conn"), SqlClient.SqlConnection))
            sqlCMD.CommandType = CommandType.StoredProcedure

            sqlCMD.Parameters.Add("@IdEntita", SqlDbType.Int).Value = Request.QueryString("IdEntita")
            sqlCMD.Parameters.Add("@IdCheckList", SqlDbType.Int).Value = Request.QueryString("IdLista")
            sqlCMD.Parameters.Add("@TipoCheckList", SqlDbType.Int).Value = tipologia
            sqlCMD.Parameters.Add("@Destinatario", SqlDbType.VarChar).Value = lblTo.Text
            sqlCMD.Parameters.Add("@Oggetto", SqlDbType.VarChar).Value = lblOggetto.Text
            sqlCMD.Parameters.Add("@Testo", SqlDbType.VarChar).Value = testo
            sqlCMD.Parameters.Add("@Username", SqlDbType.VarChar).Value = Session("Utente")



            sqlCMD.ExecuteScalar()


            CmdInvioEmail.Visible = False

        Catch ex As Exception


            Response.Write(ex.Message.ToString())
            Exit Sub
        End Try
    End Sub

End Class