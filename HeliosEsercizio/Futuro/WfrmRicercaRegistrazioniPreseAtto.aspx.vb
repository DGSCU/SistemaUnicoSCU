Imports System.Security.Cryptography
Imports Logger.Data
Public Class WfrmRicercaRegistrazioniPreseAtto
    Inherits SmartPage

    Const IndiceIDEntePersonaleRuolo = 8

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

        If Session("TipoUtente") <> "U" Then
            divRicerca.Visible = False
            divForbidden.Visible = True
            Exit Sub
        End If

    End Sub

    Protected Sub cmdChiudi_Click(sender As Object, e As EventArgs) Handles cmdChiudi.Click
        Response.Redirect("WfrmMain.aspx")
    End Sub

    Function ControllaCampi() As Boolean
        If Not checkData(txtDataInizio.Text) Then
            lblmess.Text = "Data di inizio periodo non valida. Inserire la data nel formato GG/MM/AAAA"
            lblmess.Visible = True
            Return False
        End If

        If Not checkData(txtDataFine.Text) Then
            lblmess.Text = "Data di fine periodo non valida. Inserire la data nel formato GG/MM/AAAA"
            lblmess.Visible = True
            Return False
        End If
        Return True

    End Function

    Protected Sub cmdRicerca_Click(sender As Object, e As EventArgs) Handles cmdRicerca.Click
        Ricerca()
        lblmessaggio.Visible = False
    End Sub

    Sub Ricerca()
        lblmess.Visible = False

        If Not ControllaCampi() Then Exit Sub

        CaricaRegistrazioni()
    End Sub

    Function checkData(strData As String) As Boolean
        Dim dataNascita As Date
        Dim data As String = strData.Trim()

        If data = "" Then Return True

        If Len(data) <> 10 Then
            Return False
        ElseIf (Date.TryParse(data, dataNascita) = False) Then
            Return False
        End If
        Return True
    End Function

    Private Sub CaricaRegistrazioni()

        Dim sqlDAP As New SqlClient.SqlDataAdapter
        Dim dataSet As New DataSet
        Dim strNomeStore As String = "[SP_REGISTRAZIONI_PRESE_ATTO_RICERCA]"


        Try
            sqlDAP = New SqlClient.SqlDataAdapter(strNomeStore, CType(Session("conn"), SqlClient.SqlConnection))
            sqlDAP.SelectCommand.CommandType = CommandType.StoredProcedure

            If txtDataInizio.Text.Trim = "" Then
                sqlDAP.SelectCommand.Parameters.Add("@DataInizioPeriodo", SqlDbType.VarChar).Value = DBNull.Value
            Else
                sqlDAP.SelectCommand.Parameters.Add("@DataInizioPeriodo", SqlDbType.VarChar).Value = DataToISO(txtDataInizio.Text.Trim, False)
            End If

            If txtDataFine.Text.Trim = "" Then
                sqlDAP.SelectCommand.Parameters.Add("@DataFinePeriodo", SqlDbType.VarChar).Value = DBNull.Value
            Else
                sqlDAP.SelectCommand.Parameters.Add("@DataFinePeriodo", SqlDbType.VarChar).Value = DataToISO(txtDataFine.Text.Trim, True)
            End If

            If txtCodiceEnte.Text.Trim = "" Then
                sqlDAP.SelectCommand.Parameters.Add("@CodiceEnte", SqlDbType.NVarChar).Value = DBNull.Value
            Else
                sqlDAP.SelectCommand.Parameters.Add("@CodiceEnte", SqlDbType.NVarChar).Value = txtCodiceEnte.Text.Trim.Replace("'", "''")
            End If

            If TxtDenEnte.Text.Trim = "" Then
                sqlDAP.SelectCommand.Parameters.Add("@DenominazioneEnte", SqlDbType.NVarChar).Value = DBNull.Value
            Else
                sqlDAP.SelectCommand.Parameters.Add("@DenominazioneEnte", SqlDbType.NVarChar).Value = TxtDenEnte.Text.Trim.Replace("'", "''")
            End If

            If txtcognome.Text.Trim = "" Then
                sqlDAP.SelectCommand.Parameters.Add("@CognomeRL", SqlDbType.NVarChar).Value = DBNull.Value
            Else
                sqlDAP.SelectCommand.Parameters.Add("@CognomeRL", SqlDbType.NVarChar).Value = txtcognome.Text.Trim.Replace("'", "''")
            End If

            If txtCF.Text.Trim = "" Then
                sqlDAP.SelectCommand.Parameters.Add("@CodiceFiscaleRL", SqlDbType.NVarChar).Value = DBNull.Value
            Else
                sqlDAP.SelectCommand.Parameters.Add("@CodiceFiscaleRL", SqlDbType.NVarChar).Value = txtCF.Text.Trim.Replace("'", "''")
            End If

            sqlDAP.SelectCommand.Parameters.Add("@StatoAntimafia", SqlDbType.Int).Value = ddlStatoAntimafia.SelectedValue
            sqlDAP.SelectCommand.Parameters.Add("@CambioRL", SqlDbType.Int).Value = ddlCambioRL.SelectedValue
            sqlDAP.SelectCommand.Parameters.Add("@PresaAtto", SqlDbType.Int).Value = ddlPresaAtto.SelectedValue

            sqlDAP.Fill(dataSet)

            If dataSet.Tables.Count > 0 AndAlso dataSet.Tables(0).Rows.Count > 0 Then
                divAzioni.Visible = True
                cmdSelezionaTutti.Visible = True
                cmdDeselezionaTutti.Visible = False
            Else
                divAzioni.Visible = False
            End If

            Session("DtsRegistrazioni") = dataSet
            dgRegistrazioni.DataSource = dataSet
            dgRegistrazioni.CurrentPageIndex = 0
            BindaGrigliaRegistrazioni()
        Catch ex As Exception
            Response.Write(ex.Message.ToString())
            Exit Sub
        End Try
    End Sub

    Function DataToISO(data As String, datafine As Boolean) As String
        Dim _ret As String = ""

        _ret = data.Substring(6, 4) + data.Substring(3, 2) + data.Substring(0, 2)
        If datafine Then _ret += " 23:59:59"

        Return _ret
    End Function

    Protected Sub dgRegistrazioni_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgRegistrazioni.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim chkPresaAtto As CheckBox = DirectCast(e.Item.FindControl("chkPresaAtto"), CheckBox)
            If e.Item.DataItem("CambioRL") = "NO" Then
                chkPresaAtto.Visible = False
            Else
                chkPresaAtto.Checked = If(e.Item.DataItem("SelPresaAtto"), True, False)
                If e.Item.DataItem("PresaAtto") OrElse e.Item.DataItem("Stato antimafia") = "Dich. antimafia non inserita" Then chkPresaAtto.Enabled = False
            End If

        End If
    End Sub

    Protected Sub cmdSelezionaTutti_Click(sender As Object, e As EventArgs) Handles cmdSelezionaTutti.Click
        SelDeseldgRegistrazioni(1)
        BindaGrigliaRegistrazioni()
        cmdDeselezionaTutti.Visible = True
        cmdSelezionaTutti.Visible = False
    End Sub

    Protected Sub cmdDeselezionaTutti_Click(sender As Object, e As EventArgs) Handles cmdDeselezionaTutti.Click
        SelDeseldgRegistrazioni(0)
        BindaGrigliaRegistrazioni()
        cmdDeselezionaTutti.Visible = False
        cmdSelezionaTutti.Visible = True
    End Sub

    Sub SelDeseldgRegistrazioni(seleziona As Integer)
        Dim dataSet As New DataSet
        dataSet = Session("DtsRegistrazioni")
        If dataSet.Tables.Count > 0 AndAlso dataSet.Tables(0).Rows.Count > 0 Then

            For Each riga As DataRow In dataSet.Tables(0).Rows
                If riga("CambioRL") = "SI" And riga("PresaAtto") = 0 Then
                    riga("SelPresaAtto") = seleziona
                End If
            Next

            Session("DtsRegistrazioni") = dataSet
            dgRegistrazioni.DataSource = dataSet
        End If
    End Sub

    Protected Sub chkPresaAtto_OnCheckedChanged(sender As Object, e As EventArgs)
        Dim chkPresaAtto As CheckBox = DirectCast(sender, CheckBox)
        Dim drow As DataGridItem = DirectCast(chkPresaAtto.NamingContainer, DataGridItem)
        Dim iDEntePersonaleRuolo As String = drow.Cells(IndiceIDEntePersonaleRuolo).Text

        Dim dataSet As New DataSet
        dataSet = Session("DtsRegistrazioni")
        If dataSet.Tables.Count > 0 AndAlso dataSet.Tables(0).Rows.Count > 0 Then

            For Each riga As DataRow In dataSet.Tables(0).Rows
                If riga("IDEntePersonaleRuolo") = iDEntePersonaleRuolo Then
                    riga("SelPresaAtto") = If(chkPresaAtto.Checked, 1, 0)
                    Exit For
                End If
            Next
        End If
        Session("DtsRegistrazioni") = dataSet
    End Sub

    Sub BindaGrigliaRegistrazioni()
        dgRegistrazioni.DataBind()
        dgRegistrazioni.Columns(IndiceIDEntePersonaleRuolo).Visible = False
        cmdPresaAtto.Visible = True
    End Sub

    Protected Sub cmdPresaAtto_Click(sender As Object, e As EventArgs) Handles cmdPresaAtto.Click
        Dim _numSel As Integer = 0
        Dim _elencoId As String
        Dim dataSet As New DataSet

        lblmessaggio.Visible = False

        dataSet = Session("DtsRegistrazioni")
        If dataSet.Tables.Count > 0 AndAlso dataSet.Tables(0).Rows.Count > 0 Then
            For Each riga As DataRow In dataSet.Tables(0).Rows
                If riga("SelPresaAtto") = True And riga("PresaAtto") = 0 Then
                    _numSel += 1
                    _elencoId = _elencoId & riga("IDEntePersonaleRuolo").ToString & ","
                End If
            Next
        End If

        If _numSel = 0 Then
            lblmessaggio.CssClass = "msgErrore"
            lblmessaggio.Text = "Nessun Rappresentante legale selezionato per la presa d’atto."
            lblmessaggio.Visible = True
            Exit Sub
        End If

        Try
            AggiornaPresaAtto(_elencoId.Substring(0, _elencoId.Length - 1), Session("Utente"))
            Ricerca()
            lblmessaggio.CssClass = "msgInfo"
            lblmessaggio.Text = "AGGIORNAMENTO EFFETTUATO."
            lblmessaggio.Visible = True
        Catch ex As Exception
            lblmessaggio.CssClass = "msgErrore"
            lblmessaggio.Text = "Errore nell'aggiornamento della presa d’atto."
            lblmessaggio.Visible = True
        End Try

    End Sub


    Sub AggiornaPresaAtto(elencoId As String, UsernameRichiesta As String)

        Dim sqlCMD As New SqlClient.SqlCommand
        Dim strNomeStore As String = "[SP_AGGIORNA_PRESA_ATTO]"

        sqlCMD = New SqlClient.SqlCommand(strNomeStore, CType(Session("conn"), SqlClient.SqlConnection))
        sqlCMD.CommandType = CommandType.StoredProcedure

        sqlCMD.Parameters.Add("@elencoId", SqlDbType.VarChar).Value = elencoId
        sqlCMD.Parameters.Add("@UsernameRichiesta", SqlDbType.NVarChar).Value = UsernameRichiesta

        sqlCMD.ExecuteScalar()

    End Sub

    Protected Sub dgRegistrazioni_PageIndexChanged(source As Object, e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgRegistrazioni.PageIndexChanged
        dgRegistrazioni.CurrentPageIndex = e.NewPageIndex
        SelDeseldgRegistrazioni(0)
        BindaGrigliaRegistrazioni()
        dgRegistrazioni.SelectedIndex = -1
    End Sub
End Class