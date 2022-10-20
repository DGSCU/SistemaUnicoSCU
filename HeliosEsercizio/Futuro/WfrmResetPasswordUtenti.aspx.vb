Public Class WfrmResetPasswordUtenti
    Inherits System.Web.UI.Page
    Dim mycommand As New SqlClient.SqlCommand
    Dim dtrLocal As SqlClient.SqlDataReader

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Inserire qui il codice utente necessario per inizializzare la pagina
        Dim strsql As String

        'Inserire qui il codice utente necessario per inizializzare la pagina
        If Not Session("LogIn") Is Nothing Then
            If Session("LogIn") = False Then
                Response.Redirect("LogOn.aspx")
            End If
        Else
            Response.Redirect("LogOn.aspx")
        End If

        'mycommand = New SqlClient.SqlCommand
        If Page.IsPostBack = False Then
            mycommand.Connection = Session("conn")

            strsql = "SELECT   UtentiUNSC.IdUtente, UtentiUNSC.Nome, UtentiUNSC.Cognome,  UtentiUNSC.UserName  " & _
                     " FROM UtentiUNSC " & _
                     " WHERE UtentiUNSC.IdUtente = " & Request.QueryString("idUtente")

            mycommand.CommandText = strsql

            dtrLocal = mycommand.ExecuteReader
            dtrLocal.Read()

            If dtrLocal.HasRows = True Then
                LblUtente.Text = dtrLocal("UserName")
            Else

            End If

            If Not dtrLocal Is Nothing Then
                dtrLocal.Close()
                dtrLocal = Nothing
                mycommand = Nothing
            End If
            If Request.QueryString("IdUtente") <> "" Then
                ChkCredenzialiEmail.Checked = True
                ChkStampaCredenziali.Checked = True
            End If
        End If

    End Sub

    Private Sub cmdChiudi_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdChiudi.Click
        '''''Dim tipoutente As String
        '''''tipoutente = Mid(LblUtente.Text, 1, 1)
        '''''Response.Redirect("WfrmCreaUtenzeUNSC.aspx?CHKProv=" & tipoutente & "&IdUtente=" & Request.QueryString("IdUtente"))
        Response.Write("<script>" & vbCrLf)
        Response.Write("window.close();" & vbCrLf)
        Response.Write("</script>")
    End Sub

    Private Sub cmdSalva_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdSalva.Click
        Dim strsql As String
        Dim stampa As Boolean
        Dim MyCommand As SqlClient.SqlCommand
        Dim daCriptà As String
        Dim PWSCriptata As String
        Dim NomeUtente As String
        MyCommand = New SqlClient.SqlCommand
        MyCommand.Connection = Session("conn")

        'Generazione della password
        Session("NewPassUtente") = ClsUtility.NuovaPasswordADC() 'ClsUtility.NuovaPass()
        daCriptà = Session("NewPassUtente")

        PWSCriptata = ClsUtility.CreatePSW(daCriptà)


        'Pulisci(True)
        If ChkStampaCredenziali.Checked = True Then
            stampa = True
        End If

        strsql = "Select UserName From UtentiUnsc where IdUtente='" & Request.QueryString("IdUtente") & "'"
        dtrLocal = ClsServer.CreaDatareader(strsql, Session("conn"))
        dtrLocal.Read()
        NomeUtente = dtrLocal("UserName")

        If Not dtrLocal Is Nothing Then
            dtrLocal.Close()
            dtrLocal = Nothing
        End If

        Try

            strsql = "insert into CronologiaPasswordRegioni(UserName,Password,DataCronologia) "
            strsql = strsql & " values "
            strsql = strsql & "('" & NomeUtente & "','" & PWSCriptata & "',GetDate())"
            MyCommand.CommandText = strsql
            MyCommand.ExecuteNonQuery()
            MyCommand.Dispose()
            strsql = ""




            strsql = "UPDATE UtentiUNSC SET CambioPassword = 1 , DataModificaPassword=GetDate(), password1='" & Session("NewPassUtente") & "',password='" & PWSCriptata & "', "
            strsql = strsql & " PasswordDaInviare=" & IIf(ChkCredenzialiEmail.Checked = True, 1, 0) & " "
            strsql = strsql & " WHERE IdUtente='" & Request.QueryString("IdUtente") & "'"
            MyCommand.CommandText = strsql
            MyCommand.ExecuteNonQuery()
            MyCommand.Dispose()



        Catch es As Exception
            Throw New Exception(strsql & es.Message.ToString)
        End Try

        MyCommand = Nothing


        If ChkCredenzialiEmail.Checked = True Then
            Call invioemailCHK()
        End If
        '''' fino a qui' OK

        'tornado indietro bisogna stampare
        'Response.Redirect("WfrmCreaUtenzeUNSC.aspx?VengoDaReset=" & 71 & "&StampaCre=" & stampa & "&chk=" & "R" & "&IdUtente=" & Request.QueryString("IdUtente"))

        '-------------------------------------------------------------------------
        Response.Write("<script>" & vbCrLf)
        Response.Write("opener.navigate(""WfrmCreaUtenzeUNSC.aspx?VengoDaReset=" & 71 & "&StampaCre=" & stampa & "&IdUtente=" & Request.QueryString("IdUtente") & "&chk=" & "R" & """)" & vbCrLf)
        Response.Write("window.close();" & vbCrLf)
        Response.Write("</script>")

    End Sub

    Private Sub invioemailCHK()

        'Recupero l'id dell'utenza appena creata
        'Dim cmdIdUtenza As New SqlClient.SqlCommand
        Dim intNewIdUtente As Int16
        'cmdIdUtenza.CommandType = CommandType.Text
        'cmdIdUtenza.CommandText = "SELECT @@IDENTITY"
        'cmdIdUtenza.Connection = Session("conn")
        intNewIdUtente = Request.QueryString("IdUtente")

        'Invio della mail di creazione dell'utenza
        Dim cmdMailUtenza As New SqlClient.SqlCommand
        cmdMailUtenza.CommandType = CommandType.StoredProcedure
        cmdMailUtenza.CommandText = "SP_PROCEDURAMAIL_ADC"
        cmdMailUtenza.Connection = Session("conn")

        Dim prmIdUtente As SqlClient.SqlParameter
        prmIdUtente = New SqlClient.SqlParameter
        prmIdUtente.ParameterName = "@IDUTENTE"
        prmIdUtente.SqlDbType = SqlDbType.Int
        prmIdUtente.Value = intNewIdUtente
        cmdMailUtenza.Parameters.Add(prmIdUtente)

        Dim prmTipo As SqlClient.SqlParameter
        prmTipo = New SqlClient.SqlParameter
        prmTipo.ParameterName = "@TIPO"
        prmTipo.SqlDbType = SqlDbType.Char
        prmTipo.Value = "R"
        cmdMailUtenza.Parameters.Add(prmTipo)

        cmdMailUtenza.ExecuteNonQuery()




    End Sub

End Class