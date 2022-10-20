Public Class ver_RicercaSedeSegnalataUNSC
    Inherits System.Web.UI.Page

#Region " Codice generato da Progettazione Web Form "

    'Chiamata richiesta da Progettazione Web Form.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    'Protected WithEvents CmdRicerca As System.Web.UI.WebControls.ImageButton
    'Protected WithEvents cmdChiudi As System.Web.UI.WebControls.ImageButton
    'Protected WithEvents LblProgetto As System.Web.UI.WebControls.Label
    'Protected WithEvents LblSede As System.Web.UI.WebControls.Label
    'Protected WithEvents TxtSede As System.Web.UI.WebControls.TextBox
    'Protected WithEvents TxtCodSede As System.Web.UI.WebControls.TextBox
    'Protected WithEvents LblCodSedeAtt As System.Web.UI.WebControls.Label
    'Protected WithEvents LblComune As System.Web.UI.WebControls.Label
    'Protected WithEvents LblProvincia As System.Web.UI.WebControls.Label
    'Protected WithEvents LblRegione As System.Web.UI.WebControls.Label
    'Protected WithEvents TxtComune As System.Web.UI.WebControls.TextBox
    'Protected WithEvents TxtProvincia As System.Web.UI.WebControls.TextBox
    'Protected WithEvents TxtRegione As System.Web.UI.WebControls.TextBox
    'Protected WithEvents dgSedi As System.Web.UI.WebControls.DataGrid

    'NOTA: la seguente dichiarazione è richiesta da Progettazione Web Form.
    'Non spostarla o rimuoverla.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: questa chiamata al metodo è richiesta da Progettazione Web Form.
        'Non modificarla nell'editor del codice.
        InitializeComponent()
    End Sub

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Inserire qui il codice utente necessarioper inizializzare la pagina
        If Not Session("LogIn") Is Nothing Then
            If Session("LogIn") = False Then 'verifico validità log-in
                Response.Redirect("LogOn.aspx")
            End If
        Else
            Response.Redirect("LogOn.aspx")
        End If
        If Page.IsPostBack = False Then
            CaricaProgetto(Request.QueryString("IdAttivita"))
        End If
    End Sub

    Sub CaricaProgetto(ByVal IdAttività As Integer)
        Dim strSql As String
        Dim rstProg As SqlClient.SqlDataReader

        strSql = "Select Titolo + ' (' + CodiceEnte + ')' as Titolo From Attività where idAttività = " & IdAttività
        rstProg = ClsServer.CreaDatareader(strSql, Session("Conn"))

        If rstProg.HasRows = True Then
            rstProg.Read()
            LblProgetto.Text = LblProgetto.Text & " " & rstProg("Titolo")
        End If
        rstProg.Close()
        rstProg = Nothing
    End Sub

    Private Sub CmdRicerca_Click(sender As Object, e As EventArgs) Handles CmdRicerca.Click
        RicercaSede(Request.QueryString("IdAttivita"))
    End Sub
    Sub RicercaSede(ByVal IdAttività As Integer)
        Dim strSql As String
        Dim strWhere As String
        Dim dtsRicerca As DataSet

        strSql = "  Select enti.denominazione + ' (' +   enti.codiceregione +  ')' as Ente, "
        strSql &= " entisedi.Denominazione + ' (' + CONVERT(varchar, entisediattuazioni.IDEnteSedeAttuazione) + ')'  as Sede ,"
        strSql &= " Comuni.denominazione + ' (' + provincie.descrabb +')' as comune,regioni.regione, "
        strSql &= " entisediattuazioni.IDEnteSedeAttuazione , entisedi.idente "
        strSql &= " FROM attivitàentisediattuazione "
        strSql &= " INNER JOIN entisediattuazioni ON attivitàentisediattuazione.IDEnteSedeAttuazione = entisediattuazioni.IDEnteSedeAttuazione "
        strSql &= " INNER JOIN entisedi ON entisediattuazioni.IDEnteSede = entisedi.IDEnteSede "
        strSql &= " INNER JOIN enti on entisedi.idente=enti.idente"
        strSql &= " INNER JOIN  comuni ON entisedi.IDComune = comuni.IDComune "
        strSql &= " INNER JOIN provincie ON comuni.IDProvincia = provincie.IDProvincia "
        strSql &= " INNER JOIN regioni ON provincie.IDRegione = regioni.IDRegione"
        strSql &= " WHERE  attivitàentisediattuazione.IDAttività = " & IdAttività

        If Trim(TxtSede.Text) <> "" Then
            strSql &= " AND entisedi.Denominazione like '%" & Replace(TxtSede.Text, "'", "''") & "%'"
        End If
        If Trim(TxtCodSede.Text) <> "" Then
            strSql &= " AND entisediattuazioni.IDEnteSedeAttuazione = " & TxtCodSede.Text & ""
        End If
        If Trim(TxtComune.Text) <> "" Then
            strSql &= " AND Comuni.denominazione = '" & TxtComune.Text & "'"
        End If
        If Trim(TxtProvincia.Text) <> "" Then
            strSql &= " AND provincie.provincia = '" & TxtProvincia.Text & "'"
        End If
        If Trim(TxtRegione.Text) <> "" Then
            strSql &= " AND regioni.regione = '" & TxtRegione.Text & "'"
        End If
        Session("dtsRicerca") = ClsServer.DataSetGenerico(strSql, Session("conn"))
        CaricaDataGrid(dgSedi)

    End Sub
    Private Sub CaricaDataGrid(ByRef GriddaCaricare As DataGrid)
        'assegno il dataset alla griglia del risultato
        GriddaCaricare.DataSource = Session("dtsRicerca")
        GriddaCaricare.DataBind()
    End Sub

    Private Sub dgSedi_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgSedi.ItemCommand
        Dim strSede As String
        Select Case e.CommandName
            Case "seleziona"
                strSede = e.Item.Cells(2).Text.Replace("'", "") & " - " & e.Item.Cells(3).Text.Replace("'", "")
                Session("dtsRicerca") = Nothing
                Response.Write("<script>" & vbCrLf)
                Response.Write("window.opener.document.getElementById(""" & Request.QueryString("objIdEnteSedeAttuazione") & """).value='" & e.Item.Cells(5).Text & "';")
                Response.Write("window.opener.document.getElementById(""" & Request.QueryString("objSede") & """).value='" & strSede & "';")
                Response.Write("window.opener.document.getElementById(""" & Request.QueryString("objEnteF") & """).value='" & e.Item.Cells(1).Text.Replace("'", "") & "';")
                If Request.QueryString("IdEnte") <> e.Item.Cells(4).Text Then
                    Response.Write("window.opener.getElementById(""" & Request.QueryString("objEnteF") & """).value='" & e.Item.Cells(1).Text.Replace("'", "") & "';")
                Else
                    Response.Write("window.opener.document.getElementById(""" & Request.QueryString("objEnteF") & """).value='';")
                End If
                Response.Write("window.close()")
                Response.Write("</script>")



                'Session("dtsRicerca") = Nothing

                'Response.Write("<script>" & vbCrLf)
                'Response.Write("window.opener.document.getElementById(""" & Request.QueryString("objIdEnteSedeAttuazione") & """).value='" & e.Item.Cells(5).Text & "';")
                'Response.Write("window.opener.document.getElementById(""" & Request.QueryString("objSede") & """).value='" & strSede & "';")
                'If Request.QueryString("IdEnte") <> e.Item.Cells(4).Text Then
                '    Response.Write("window.opener.getElementById(""" & Request.QueryString("objEnteF") & """).value='" & e.Item.Cells(1).Text.Replace("'", "") & "';")
                'End If
                'Response.Write("window.close()")
                'Response.Write("</script>")
        End Select


       
    End Sub

    Private Sub cmdChiudi_Click(sender As Object, e As EventArgs) Handles cmdChiudi.Click
        Session("dtsRicerca") = Nothing
        Response.Write("<script>" & vbCrLf)
        Response.Write("window.close()")
        Response.Write("</script>")
    End Sub

    Private Sub dgSedi_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgSedi.PageIndexChanged
        dgSedi.SelectedIndex = -1
        dgSedi.EditItemIndex = -1
        dgSedi.CurrentPageIndex = e.NewPageIndex
        CaricaDataGrid(dgSedi)

    End Sub

    Private Sub dgSedi_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dgSedi.SelectedIndexChanged

    End Sub
     
End Class