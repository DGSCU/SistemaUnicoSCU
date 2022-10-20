Public Class assegnazionevincolivolontario
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
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
            caricadativolontario(CInt(Request.QueryString("IdVolontario")))
            caricacombogrigliavincoli(CInt(Request.QueryString("IdVolontario")))
        End If
    End Sub

    Sub caricadativolontario(ByVal IdEntita As Integer)
        '***Generata da Bagnani Jonathan in data:04/11/04
        Dim strLocal As String
        'datareader locale 
        Dim dtrLocal As SqlClient.SqlDataReader

        'controllo se ci sono sedi di attuazione assegnate all'ente selezionato
        'Identita
        strLocal = "select (a.Cognome + ' ' + a.Nome) as Nominativo, a.CodiceFiscale, "
        strLocal = strLocal & "case len(day(a.DataNascita)) when 1 then '0' + convert(varchar(20),day(a.DataNascita)) "
        strLocal = strLocal & "else convert(varchar(20),day(a.DataNascita))  end + '/' + "
        strLocal = strLocal & "(case len(month(a.DataNascita)) when 1 then '0' + convert(varchar(20),month(a.DataNascita)) "
        strLocal = strLocal & "else convert(varchar(20),month(a.DataNascita))  end + '/' + "
        strLocal = strLocal & "Convert(varchar(20), Year(a.DataNascita))) as DataNascita, b.Denominazione "
        strLocal = strLocal & "from entità as a "
        strLocal = strLocal & "inner join comuni as b on a.IDComuneNascita=b.IdComune "
        strLocal = strLocal & "where a.IDEntità=" & IdEntita

        'eseguo la query e passo il risultato al datareader
        dtrLocal = ClsServer.CreaDatareader(strLocal, Session("conn"))

        'controllo se ci sono sedi di attuazione assegnate al volontario selezionato

        If dtrLocal.HasRows = True Then
            dtrLocal.Read()
            lblNominativo.Text = dtrLocal("Nominativo")
            lblCodFis.Text = dtrLocal("CodiceFiscale")
            lblComuneNascita.Text = dtrLocal("Denominazione")
            lblDataNascita.Text = dtrLocal("DataNascita")
        End If
        If Not dtrLocal Is Nothing Then
            dtrLocal.Close()
            dtrLocal = Nothing
        End If

    End Sub

    Sub caricacombogrigliavincoli(ByVal identita As Integer)
        '***Generata da Bagnani Jonathan in data:04/11/04
        Dim strLocal As String
        'datareader locale 
        Dim dtsLocal As DataSet

        strLocal = "select a.IdVincolo, a.Vincolo, isnull(b.Valore,1) as Valore, b.NotaStorico as ValoreNote from Vincoli as a "
        strLocal = strLocal & "left join FlagEntità as b on a.IDVincolo=b.IDVincolo and b.IdEntità=" & identita
        strLocal = strLocal & "where a.Volontari=1 "

        dtsLocal = ClsServer.DataSetGenerico(strLocal, Session("conn"))

        dtgVincoliVolontari.DataSource = dtsLocal
        dtgVincoliVolontari.DataBind()

        Dim dtgItem As DataGridItem

        For Each dtgItem In dtgVincoliVolontari.Items
            Dim ddlEsitoVincolo As DropDownList = DirectCast(dtgItem.FindControl("ddlEsito"), DropDownList)
            Dim txtNotaStorico As TextBox = DirectCast(dtgItem.FindControl("txtNote"), TextBox)
            Select Case dtgVincoliVolontari.Items(dtgItem.ItemIndex).Cells(3).Text()
                Case True
                    ddlEsitoVincolo.SelectedValue = 1
                Case False
                    ddlEsitoVincolo.SelectedValue = 0
            End Select
            If dtgVincoliVolontari.Items(dtgItem.ItemIndex).Cells(5).Text() = "&nbsp;" Then
                txtNotaStorico.Text = vbNullString
            Else
                txtNotaStorico.Text = dtgVincoliVolontari.Items(dtgItem.ItemIndex).Cells(5).Text()
            End If
        Next

    End Sub

    Sub aggiornavincoli(ByVal identita As Integer)
        '***Generata da Bagnani Jonathan in data:04/11/04
        Dim strLocal As String
        Try
            strLocal = "delete from FlagEntità where identità=" & identita

            'cancello i vincoli esistenti
            Dim cmdDelete As Data.SqlClient.SqlCommand
            cmdDelete = New SqlClient.SqlCommand(strLocal, Session("conn"))
            cmdDelete.ExecuteNonQuery()
            cmdDelete.Dispose()

            Dim dtgItem As DataGridItem
            'vado a fare la insert
            Dim cmdinsert As Data.SqlClient.SqlCommand
            For Each dtgItem In dtgVincoliVolontari.Items
                Dim ddlEsitoVincolo As DropDownList = DirectCast(dtgItem.FindControl("ddlEsito"), DropDownList)
                Dim txtNotaStorico As TextBox = DirectCast(dtgItem.FindControl("txtNote"), TextBox)
                strLocal = "insert into FlagEntità (IDentità,IdVincolo,Valore,DataModiFICA,NotaStorico) "
                strLocal = strLocal & "values "
                strLocal = strLocal & "(" & identita & "," & CInt(dtgVincoliVolontari.Items(dtgItem.ItemIndex).Cells(0).Text()) & "," & ddlEsitoVincolo.SelectedValue & ",GetDate(),'" & Trim(ClsServer.NoApice(txtNotaStorico.Text)) & "')"
                cmdinsert = New SqlClient.SqlCommand(strLocal, Session("conn"))
                cmdinsert.ExecuteNonQuery()
            Next
            cmdinsert.Dispose()


            lblmessaggiosopra.Visible = True
            lblmessaggiosopra.Text = "Operazione effettuata con successo."
        Catch ex As Exception
            Response.Write(ex.Message)
        End Try
    End Sub
    Protected Sub CmdSalva_Click(sender As Object, e As EventArgs) Handles CmdSalva.Click
        aggiornavincoli(CInt(Request.QueryString("IdVolontario")))
    End Sub

End Class