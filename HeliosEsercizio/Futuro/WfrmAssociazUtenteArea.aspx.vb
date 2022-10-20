Public Class WfrmAssociazUtenteArea
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim strsql As String
        Dim mycommand As New SqlClient.SqlCommand
        Dim dtrLocal As SqlClient.SqlDataReader

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

            strsql = "SELECT AssociaUtenteGruppo.IdProfilo, Editor_Aree.Area, Editor_Aree.IdArea, UtentiUNSC.IdUtente, UtentiUNSC.Nome, UtentiUNSC.Cognome, " & _
            " UtentiUNSC.UserName, Profili.Descrizione AS DescProfilo, RegioniCompetenze.Descrizione AS DescCompetenza " & _
            " FROM         RegioniCompetenze INNER JOIN " & _
            " Profili "
            '============================================================================================================================
            '====================================================30/09/2008==============================================================
            '=======MODIFICA EFFETTUATA DA Kronk (99 scimmie saltavano sul letto, una cadde in terra e si ruppe il cervelletto...)=======
            '=================AGGIUNTA JOIN SU PROFILO READ PER ABILITARE LEGGERE LE ABILITAZIONI ANCHE DELL'UTENTE READ=================
            '============================================================================================================================
            If UCase(Me.TemplateSourceDirectory) <> "/HELIOSREAD" Then
                strsql = strsql & " INNER JOIN	AssociaUtenteGruppo ON Profili.IdProfilo = AssociaUtenteGruppo.IdProfilo "
            Else
                strsql = strsql & " INNER JOIN	AssociaUtenteGruppo ON Profili.IdProfilo = AssociaUtenteGruppo.IdProfiloRead "
            End If

            strsql = strsql & " INNER JOIN UtentiUNSC ON AssociaUtenteGruppo.UserName = UtentiUNSC.UserName ON " & _
            "RegioniCompetenze.IdRegioneCompetenza = UtentiUNSC.IdRegioneCompetenza LEFT OUTER JOIN " & _
            "Editor_Aree RIGHT OUTER JOIN " & _
            "Editor_UtentiAree ON Editor_Aree.IdArea = Editor_UtentiAree.IdArea ON UtentiUNSC.IdUtente = Editor_UtentiAree.IdUtente " & _
            " WHERE (UtentiUNSC.IdUtente = " & Request.QueryString("idUtente") & ")"

            mycommand.CommandText = strsql

            dtrLocal = mycommand.ExecuteReader
            LblSalvataggio.Visible = False
            ChkAccredit.Checked = False
            ChKMonitoraggio.Checked = False
            ChKProgetti.Checked = False
            ChKVolontari.Checked = False

            If dtrLocal.HasRows = True Then
                While dtrLocal.Read()
                    caricacampi(dtrLocal)
                    If Not IsDBNull((dtrLocal("idarea"))) Then
                        Select Case dtrLocal("idarea")
                            Case 1
                                ChkAccredit.Checked = True

                            Case 2
                                ChKProgetti.Checked = True

                            Case 3
                                ChKVolontari.Checked = True

                            Case 4
                                ChKMonitoraggio.Checked = True

                        End Select
                    End If
                End While
            End If

            If Not dtrLocal Is Nothing Then
                dtrLocal.Close()
                dtrLocal = Nothing
                mycommand = Nothing
            End If
        End If

    End Sub

    Private Sub caricacampi(ByVal dtrLocal As SqlClient.SqlDataReader)
        LblCognome.Text = dtrLocal("Cognome")
        LblNome.Text = dtrLocal("Nome")
        LblUtente.Text = dtrLocal("UserName")
        LblCompetenza.Text = dtrLocal("desccompetenza")
        LblProfilo.Text = dtrLocal("descProfilo")
    End Sub


    Private Sub cmdSalva_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdSalva.Click
        Dim StrSql As String
        Dim mycommand As New SqlClient.SqlCommand

        'Elimino tutte le occorrenze...

        StrSql = "delete from editor_utentiaree where idutente = " & Request.QueryString("idUtente")

        mycommand.Connection = Session("conn")

        mycommand.CommandText = StrSql
        mycommand.ExecuteNonQuery()

        'inserimento associazione delle aree all'utente

        If ChkAccredit.Checked = True Then
            StrSql = "insert into editor_utentiaree (idutente, idarea) values (" & Request.QueryString("idUtente") & _
            ", 1 )"
            mycommand.CommandText = StrSql
            mycommand.ExecuteNonQuery()
        End If

        If ChKProgetti.Checked = True Then
            StrSql = "insert into editor_utentiaree (idutente, idarea) values (" & Request.QueryString("idUtente") & _
            ", 2 )"
            mycommand.CommandText = StrSql
            mycommand.ExecuteNonQuery()
        End If

        If ChKVolontari.Checked = True Then
            StrSql = "insert into editor_utentiaree (idutente, idarea) values (" & Request.QueryString("idUtente") & _
            ", 3)"
            mycommand.CommandText = StrSql
            mycommand.ExecuteNonQuery()
        End If

        If ChKMonitoraggio.Checked = True Then
            StrSql = "insert into editor_utentiaree (idutente, idarea) values (" & Request.QueryString("idUtente") & _
            ", 4)"
            mycommand.CommandText = StrSql
            mycommand.ExecuteNonQuery()
        End If

        LblSalvataggio.Visible = True
    End Sub

    Private Sub cmdChiudi_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdChiudi.Click
        Dim tipoutente As String
        tipoutente = Mid(LblUtente.Text, 1, 1)
        Response.Redirect("WfrmCreaUtenzeUNSC.aspx?CHKProv=" & tipoutente & "&IdUtente=" & Request.QueryString("IdUtente"))
    End Sub

End Class