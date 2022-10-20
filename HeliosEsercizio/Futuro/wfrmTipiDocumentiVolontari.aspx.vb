Public Class wfrmTipiDocumentiVolontari
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Inserire qui il codice utente necessario per inizializzare la pagina
        lblmsg.Text = String.Empty

        If Not Session("LogIn") Is Nothing Then
            If Session("LogIn") = False Then 'verifico validità log-in
                Response.Redirect("LogOn.aspx")
            End If
        Else
            Response.Redirect("LogOn.aspx")
        End If
        If Page.IsPostBack = False Then
            LoadTipoDocumentiVolontari()
        End If
    End Sub

    'CARICAMENTO GRIGLIA
    Private Sub LoadTipoDocumentiVolontari()
        Dim strSql As String
        Dim dtsElencoDocVol As DataSet

        strSql = " select IdTipoDocumentoVolontario,TipoDocumento,Note, Abilitato, NumeroDocumentiAssociati  " & _
                 " FROM vw_tipidocumentivolontari "
        dtsElencoDocVol = ClsServer.DataSetGenerico(strSql, Session("conn"))

        dtgTipoDocumento.DataSource = dtsElencoDocVol
        dtgTipoDocumento.DataBind()
    End Sub

    Private Sub dtgTipoDocumento_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dtgTipoDocumento.ItemCommand
        Dim strMsgReturn As String = ""
        Select Case e.CommandName
            Case "Abilita"
                strMsgReturn = TipiDocumentiVolontari_Abilitazione(e.Item.Cells(1).Text, 1, Session("Utente"))
            Case "Disabilita"
                strMsgReturn = TipiDocumentiVolontari_Abilitazione(e.Item.Cells(1).Text, 0, Session("Utente"))
        End Select
        If strMsgReturn <> "" Then
            If strMsgReturn = "OK" Then 'abilitazione/disbilitazione è andata abuon fine
                lblmsg.Text = "Aggiornamento effettuato con successo."
                LoadTipoDocumentiVolontari()
            Else 'esiti possibili : aggiornamento non necessario oppure errore durante l'operanzione
                lblmsg.Text = strMsgReturn
            End If
        End If
    End Sub

    'ABILITAZIONE/DISABILITAZIONE TIPI DOCUMENTI
    Private Function TipiDocumentiVolontari_Abilitazione(ByVal IdTipoDocumento As Integer, ByVal TipoOperazione As Byte, ByVal Username As String) As String

        Dim sqlCMD As New SqlClient.SqlCommand
        Dim strNomeStore As String = "[SP_TipiDocumentiVolontari_Abilitazione]"

        Try
            sqlCMD = New SqlClient.SqlCommand(strNomeStore, CType(Session("conn"), SqlClient.SqlConnection))
            sqlCMD.CommandType = CommandType.StoredProcedure
            sqlCMD.Parameters.Add("@IdTipoDocumento", SqlDbType.Int).Value = IdTipoDocumento
            sqlCMD.Parameters.Add("@TipoOperazione", SqlDbType.Bit).Value = TipoOperazione
            sqlCMD.Parameters.Add("@Username", SqlDbType.NVarChar).Value = Username

            Dim sparam1 As SqlClient.SqlParameter
            sparam1 = New SqlClient.SqlParameter
            sparam1.ParameterName = "@Esito"
            sparam1.Size = 100
            sparam1.SqlDbType = SqlDbType.NVarChar
            sparam1.Direction = ParameterDirection.Output
            sqlCMD.Parameters.Add(sparam1)

            sqlCMD.ExecuteScalar()
            Dim str As String
            str = sqlCMD.Parameters("@Esito").Value
            Return str
        Catch ex As Exception
            Response.Write(ex.Message.ToString())
            Exit Function
        End Try
    End Function

    Private Sub dtgTipoDocumento_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dtgTipoDocumento.PageIndexChanged
        dtgTipoDocumento.CurrentPageIndex = e.NewPageIndex
        LoadTipoDocumentiVolontari()
        dtgTipoDocumento.SelectedIndex = -1
    End Sub

    Private Sub cmdChiudi_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdChiudi.Click
        Response.Redirect("WfrmMain.aspx")
    End Sub

    Private Sub cmdInserisci_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdInserisci.Click
        Dim strMsgReturn As String

        If controlliSalvataggioServer() = True Then

            strMsgReturn = TipiDocumentiVolontari_Inserimento(txtTipoDocumento.Text, txtNote.Text, Session("Utente"))

            If strMsgReturn = "OK" Then 'se torna ok l'inserimento è andato a buon fine
                lblmsg.Text = "Inserimento effettuato con successo."
                LoadTipoDocumentiVolontari()
            Else 'esiti possibili : tipologia già essitente oppure errore durante l'inserimento
                lblmsg.Text = strMsgReturn
            End If
        End If
       
    End Sub

    'FUNZIONE PER L'INSERIMENTO DI UNA NUOVA TIPOLOGIA DI DOCUMENTO
    Private Function TipiDocumentiVolontari_Inserimento(ByVal TipoDocumento As String, ByVal Note As String, ByVal Username As String) As String

        Dim sqlCMD As New SqlClient.SqlCommand
        Dim strNomeStore As String = "[SP_TipiDocumentiVolontari_Inserimento]"

        Try
            sqlCMD = New SqlClient.SqlCommand(strNomeStore, CType(Session("conn"), SqlClient.SqlConnection))
            sqlCMD.CommandType = CommandType.StoredProcedure
            sqlCMD.Parameters.Add("@TipoDocumento", SqlDbType.NVarChar).Value = TipoDocumento
            sqlCMD.Parameters.Add("@Note", SqlDbType.NVarChar).Value = Note
            sqlCMD.Parameters.Add("@Username", SqlDbType.NVarChar).Value = Username

            Dim sparam1 As SqlClient.SqlParameter
            sparam1 = New SqlClient.SqlParameter
            sparam1.ParameterName = "@Esito"
            sparam1.Size = 100
            sparam1.SqlDbType = SqlDbType.NVarChar
            sparam1.Direction = ParameterDirection.Output
            sqlCMD.Parameters.Add(sparam1)

            sqlCMD.ExecuteScalar()
            Dim str As String
            str = sqlCMD.Parameters("@Esito").Value
            Return str
        Catch ex As Exception
            Response.Write(ex.Message.ToString())
            Exit Function
        End Try
    End Function

    Function controlliSalvataggioServer() As Boolean

        If txtTipoDocumento.Text.Trim = String.Empty Then
            lblmsg.Text = "E' necessario indicare il Tipo Documento."
            txtTipoDocumento.Focus()
            Return False
        End If

        Return True

    End Function
End Class