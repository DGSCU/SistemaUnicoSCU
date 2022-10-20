Public Class wfrmAssociaImportazioneValutazioniProgetti
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


        Response.ExpiresAbsolute = #1/1/1980#
        Response.AddHeader("Pragma", "no-cache")
        If Page.IsPostBack = False Then
            If Session("TipoUtente") = "U" Then
                ddlTipoProgetto.SelectedValue = 2
                cmdSalva.Visible = False
            End If
            If Session("TipoUtente") = "R" Then
                ddlTipoProgetto.SelectedValue = 1
                ddlTipoProgetto.Enabled = False
                Dim DtAppoggio As DataTable = EseguiStoreAssociaImportazioneValutazioniProgetti(Session("IdRegioneCompetenzaUtente"), 1, Session("conn"))
                dtgAssociazioneImportazioneProgetti.DataSource() = DtAppoggio
                dtgAssociazioneImportazioneProgetti.DataBind()

                Dim dtgItem As DataGridItem
                For Each dtgItem In dtgAssociazioneImportazioneProgetti.Items
                    Dim txtColonna As TextBox = DirectCast(dtgItem.FindControl("txtColonna"), TextBox)
                    If dtgAssociazioneImportazioneProgetti.Items(dtgItem.ItemIndex).Cells(5).Text() = "&nbsp;" Then
                        txtColonna.Text = vbNullString
                    Else
                        txtColonna.Text = dtgAssociazioneImportazioneProgetti.Items(dtgItem.ItemIndex).Cells(5).Text()
                    End If
                Next
                cmdSalva.Visible = True
            End If
            If Session("TipoUtente") = "E" Then
                ddlTipoProgetto.SelectedValue = 2
                ddlTipoProgetto.Enabled = False
                cmdSalva.Visible = False
            End If

        End If
    End Sub

    Public Function EseguiStoreAssociaImportazioneValutazioniProgetti(ByVal idCompetenza As Integer, ByVal TipoProgetto As Integer, ByVal conn As SqlClient.SqlConnection) As DataTable

        Dim sqlDAP As New SqlClient.SqlDataAdapter
        Dim strNomeStore As String = "[SP_IMPORTVALUTAZIONI_RITORNAASSOCIAZIONEPARAMETRI]"
        EseguiStoreAssociaImportazioneValutazioniProgetti = New DataTable

        Try
            sqlDAP = New SqlClient.SqlDataAdapter(strNomeStore, conn)
            sqlDAP.SelectCommand.CommandType = CommandType.StoredProcedure
            sqlDAP.SelectCommand.Parameters.Add("@IDCOMPETENZA", SqlDbType.Int).Value = idCompetenza
            sqlDAP.SelectCommand.Parameters.Add("@NAZIONALE", SqlDbType.Int).Value = TipoProgetto
            sqlDAP.Fill(EseguiStoreAssociaImportazioneValutazioniProgetti)
            Return EseguiStoreAssociaImportazioneValutazioniProgetti
        Catch ex As Exception
            Response.Write(ex.Message.ToString())
            Exit Function
        End Try
    End Function

    Private Sub ddlTipoProgetto_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlTipoProgetto.SelectedIndexChanged
        lblmess.Text = ""
        If ddlTipoProgetto.SelectedValue = 1 Then
            'EseguiStoreAssociaImportazioneValutazioniProgetti(Session("IdRegioneCompetenzaUtente"), 1, Session("conn"))
            Dim DtAppoggio As DataTable = EseguiStoreAssociaImportazioneValutazioniProgetti(Session("IdRegioneCompetenzaUtente"), 1, Session("conn"))
            dtgAssociazioneImportazioneProgetti.DataSource() = DtAppoggio
            dtgAssociazioneImportazioneProgetti.DataBind()
            Dim dtgItem As DataGridItem
            For Each dtgItem In dtgAssociazioneImportazioneProgetti.Items
                Dim txtColonna As TextBox = DirectCast(dtgItem.FindControl("txtColonna"), TextBox)
                If dtgAssociazioneImportazioneProgetti.Items(dtgItem.ItemIndex).Cells(5).Text() = "&nbsp;" Then
                    txtColonna.Text = vbNullString
                Else
                    txtColonna.Text = dtgAssociazioneImportazioneProgetti.Items(dtgItem.ItemIndex).Cells(5).Text()
                End If
            Next
            cmdSalva.Visible = True
        End If
        If ddlTipoProgetto.SelectedValue = 0 Then
            'EseguiStoreAssociaImportazioneValutazioniProgetti(Session("IdRegioneCompetenzaUtente"), 0, Session("conn"))
            Dim DtAppoggio As DataTable = EseguiStoreAssociaImportazioneValutazioniProgetti(Session("IdRegioneCompetenzaUtente"), 0, Session("conn"))
            dtgAssociazioneImportazioneProgetti.DataSource() = DtAppoggio
            dtgAssociazioneImportazioneProgetti.DataBind()
            Dim dtgItem As DataGridItem
            For Each dtgItem In dtgAssociazioneImportazioneProgetti.Items
                Dim txtColonna As TextBox = DirectCast(dtgItem.FindControl("txtColonna"), TextBox)
                If dtgAssociazioneImportazioneProgetti.Items(dtgItem.ItemIndex).Cells(5).Text() = "&nbsp;" Then
                    txtColonna.Text = vbNullString
                Else
                    txtColonna.Text = dtgAssociazioneImportazioneProgetti.Items(dtgItem.ItemIndex).Cells(5).Text()
                End If
            Next
            cmdSalva.Visible = True
        End If
        If ddlTipoProgetto.SelectedValue = 2 Then
            svuota()
            cmdSalva.Visible = False
        End If
    End Sub

    Private Sub svuota()
        Dim DtAppoggio As DataTable
        dtgAssociazioneImportazioneProgetti.DataSource() = DtAppoggio
        dtgAssociazioneImportazioneProgetti.DataBind()

        Dim dtgItem As DataGridItem
        For Each dtgItem In dtgAssociazioneImportazioneProgetti.Items
            Dim txtColonna As TextBox = DirectCast(dtgItem.FindControl("txtColonna"), TextBox)
            txtColonna.Text = vbNullString
            'txtColonna.Text = dtgAssociazioneImportazioneProgetti.Items(dtgItem.ItemIndex).Cells(5).Text()
        Next
    End Sub

    Private Sub cmdSalva_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdSalva.Click
        lblmess.Text = ""
        Dim strLocal As String
        Dim dtgItem As DataGridItem
        If ddlTipoProgetto.SelectedValue <> 2 Then


            For Each dtgItem In dtgAssociazioneImportazioneProgetti.Items
                'Dim indicemassimo As Integer = dtgAssociazioneImportazioneProgetti.Items.Count
                Dim txtColonna As TextBox = DirectCast(dtgItem.FindControl("txtColonna"), TextBox)
                If txtColonna.Text = "" Then
                    lblmess.Text = "Inserire tutti i valore nei campi Colonna"
                    Exit Sub

                End If
            Next
            strLocal = "delete from AssociaImportazioneValutazioniProgetti where idcompetenza=" & Session("IdRegioneCompetenzaUtente") & " and Nazionale=" & ddlTipoProgetto.SelectedValue & ""
            'cancello  esistenti
            Dim cmdDelete As Data.SqlClient.SqlCommand
            cmdDelete = New SqlClient.SqlCommand(strLocal, Session("conn"))
            cmdDelete.ExecuteNonQuery()
            cmdDelete.Dispose()
            For Each dtgItem In dtgAssociazioneImportazioneProgetti.Items
                ''vado a fare la insert
                Dim cmdinsert As Data.SqlClient.SqlCommand
                Dim txtColonna As TextBox = DirectCast(dtgItem.FindControl("txtColonna"), TextBox)
                Dim CodiceParametro As String = dtgAssociazioneImportazioneProgetti.Items(dtgItem.ItemIndex).Cells(0).Text()

                strLocal = "insert into AssociaImportazioneValutazioniProgetti (IdCompetenza,Nazionale,CodiceParametro,Colonna) "
                strLocal = strLocal & "values "
                strLocal = strLocal & "('" & Session("IdRegioneCompetenzaUtente") & "'," & ddlTipoProgetto.SelectedValue & ",'" & CodiceParametro & "','" & UCase(Trim(ClsServer.NoApice(txtColonna.Text))) & "')"
                cmdinsert = New SqlClient.SqlCommand(strLocal, Session("conn"))
                cmdinsert.ExecuteNonQuery()
                cmdinsert.Dispose()
            Next

            lblmess.Text = "Salvataggio avvenuto con successo"
            caricadopo()

        End If
    End Sub

    Private Sub caricadopo()
        Dim DtAppoggio As DataTable = EseguiStoreAssociaImportazioneValutazioniProgetti(Session("IdRegioneCompetenzaUtente"), ddlTipoProgetto.SelectedValue, Session("conn"))
        dtgAssociazioneImportazioneProgetti.DataSource() = DtAppoggio
        dtgAssociazioneImportazioneProgetti.DataBind()

        Dim dtgItem As DataGridItem
        For Each dtgItem In dtgAssociazioneImportazioneProgetti.Items
            Dim txtColonna As TextBox = DirectCast(dtgItem.FindControl("txtColonna"), TextBox)
            If dtgAssociazioneImportazioneProgetti.Items(dtgItem.ItemIndex).Cells(5).Text() = "&nbsp;" Then
                txtColonna.Text = vbNullString
            Else
                txtColonna.Text = dtgAssociazioneImportazioneProgetti.Items(dtgItem.ItemIndex).Cells(5).Text()
            End If
        Next
    End Sub

    Private Sub cmdChiudi_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdChiudi.Click
        Response.Redirect("WfrmMain.aspx")
    End Sub

End Class