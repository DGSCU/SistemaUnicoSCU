Public Class ver_RicercaEnteSegnalataUNSC
    Inherits System.Web.UI.Page
    '**** CREATA DA SIMONA CORDELLA IL 04/11/2011 ****
    '**** RICERCA ENTI DA ASSEGNARE ALLA VERIFICA SEGNALATA UNSC ***

#Region " Codice generato da Progettazione Web Form "

    'Chiamata richiesta da Progettazione Web Form.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    'Protected WithEvents Label21 As System.Web.UI.WebControls.Label
    'Protected WithEvents TxtDescrEnte As System.Web.UI.WebControls.TextBox
    'Protected WithEvents Label20 As System.Web.UI.WebControls.Label
    'Protected WithEvents TxtCodEnte As System.Web.UI.WebControls.TextBox
    ' Protected WithEvents CmdRicerca As System.Web.UI.WebControls.Button
    '  Protected WithEvents cmdChiudi As System.Web.UI.WebControls.Button
    '  Protected WithEvents lblCompentenza As System.Web.UI.WebControls.Label
    ' Protected WithEvents ddlCompetenza As System.Web.UI.WebControls.DropDownList
    '  Protected WithEvents dgEnte As System.Web.UI.WebControls.DataGrid

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
        'Inserire qui il codice utente necessario per inizializzare la pagina
        If Not Session("LogIn") Is Nothing Then
            If Session("LogIn") = False Then 'verifico validità log-in
                Response.Redirect("LogOn.aspx")
            End If
        Else
            Response.Redirect("LogOn.aspx")
        End If
        If Page.IsPostBack = False Then
            CaricaCompetenze()
            CaricaCompetenzeProgetto()
        End If
    End Sub

    Private Sub CmdRicerca_Click(sender As Object, e As EventArgs) Handles CmdRicerca.Click
        RicercaEnte()
    End Sub

    Sub CaricaCompetenze()
        'stringa per la query
        Dim strSQL As String
        'datareader che conterrà l'id 
        Dim dtrCompetenze As System.Data.SqlClient.SqlDataReader

        Try
            'controllo se si tratta del primo caricamento. così leggo i dati nel db una sola volta
            If Page.IsPostBack = False Then
                'preparo la query

                strSQL = "select IdRegioneCompetenza,Descrizione,CodiceRegioneCompetenza,left(CodiceRegioneCompetenza,1)from RegioniCompetenze where IdRegioneCompetenza <> 22 "
                strSQL = strSQL & " union "
                strSQL = strSQL & " select '0',' TUTTI ','','A' "
                strSQL = strSQL & " union "
                strSQL = strSQL & " select '-1',' NAZIONALE ','','B' "
                strSQL = strSQL & " union "
                strSQL = strSQL & " select '-2',' REGIONALE ','','C' "
                strSQL = strSQL & " union "
                strSQL = strSQL & " select '-3',' NON DEFINITO ','','D' "
                strSQL = strSQL & "  from RegioniCompetenze order by left(CodiceRegioneCompetenza,1),descrizione "
                'chiudo il datareader se aperto
                If Not dtrCompetenze Is Nothing Then
                    dtrCompetenze.Close()
                    dtrCompetenze = Nothing
                End If

                'eseguo la query
                dtrCompetenze = ClsServer.CreaDatareader(strSQL, Session("conn"))
                'assegno il datadearder alla combo caricando così descrizione e id
                ddlCompetenza.DataSource = dtrCompetenze
                ddlCompetenza.Items.Add("")
                ddlCompetenza.DataTextField = "Descrizione"
                ddlCompetenza.DataValueField = "IDRegioneCompetenza"
                ddlCompetenza.DataBind()
                'chiudo il datareader se aperto
                If Not dtrCompetenze Is Nothing Then
                    dtrCompetenze.Close()
                    dtrCompetenze = Nothing
                End If
            End If
            'Controllo abilitazione scelta
            If Session("TipoUtente") = "U" Then
                ddlCompetenza.Enabled = True
                ddlCompetenza.SelectedIndex = 0

            Else
                'CboCompetenza.SelectedIndex = 1
                ddlCompetenza.Enabled = True
                'preparo la query
                strSQL = "select b.IdRegioneCompetenza ,b.Heliosread from RegioniCompetenze a "
                strSQL = strSQL & "INNER JOIN utentiunsc b ON a.idregionecompetenza = b.idregionecompetenza "
                strSQL = strSQL & "where b.username = '" & Session("Utente") & "'"
                'chiudo il datareader se aperto
                If Not dtrCompetenze Is Nothing Then
                    dtrCompetenze.Close()
                    dtrCompetenze = Nothing
                End If
                'controllo se utente o ente regionale
                'eseguo la query
                dtrCompetenze = ClsServer.CreaDatareader(strSQL, Session("conn"))
                dtrCompetenze.Read()
                If dtrCompetenze.HasRows = True Then
                    ' ddlCompetenza.SelectedValue = dtrCompetenze("IdRegioneCompetenza")
                    If dtrCompetenze("Heliosread") = True Then
                        ddlCompetenza.Enabled = True
                    End If

                End If

            End If
        Catch ex As Exception
            Response.Write(ex.Message.ToString())
            If Not dtrCompetenze Is Nothing Then
                dtrCompetenze.Close()
                dtrCompetenze = Nothing
            End If
        End Try
        If Not dtrCompetenze Is Nothing Then
            dtrCompetenze.Close()
            dtrCompetenze = Nothing
        End If
    End Sub
    Sub CaricaCompetenzeProgetto()
        'stringa per la query
        Dim strSQL As String
        'datareader che conterrà l'id 
        Dim dtrCompetenze As System.Data.SqlClient.SqlDataReader

        Try
            'controllo se si tratta del primo caricamento. così leggo i dati nel db una sola volta
            If Page.IsPostBack = False Then
                'preparo la query

                strSQL = "select IdRegioneCompetenza,Descrizione,CodiceRegioneCompetenza,left(CodiceRegioneCompetenza,1)from RegioniCompetenze where IdRegioneCompetenza <> 22 "
                strSQL = strSQL & " union "
                strSQL = strSQL & " select '0',' TUTTI ','','A' "
                strSQL = strSQL & " union "
                strSQL = strSQL & " select '-1',' NAZIONALE ','','B' "
                strSQL = strSQL & " union "
                strSQL = strSQL & " select '-2',' REGIONALE ','','C' "
                strSQL = strSQL & " union "
                strSQL = strSQL & " select '-3',' NON DEFINITO ','','D' "
                strSQL = strSQL & "  from RegioniCompetenze order by left(CodiceRegioneCompetenza,1),descrizione "
                'chiudo il datareader se aperto
                If Not dtrCompetenze Is Nothing Then
                    dtrCompetenze.Close()
                    dtrCompetenze = Nothing
                End If

                'eseguo la query
                dtrCompetenze = ClsServer.CreaDatareader(strSQL, Session("conn"))
                'assegno il datadearder alla combo caricando così descrizione e id
                ddlCompetenzaProgetto.DataSource = dtrCompetenze
                ddlCompetenzaProgetto.Items.Add("")
                ddlCompetenzaProgetto.DataTextField = "Descrizione"
                ddlCompetenzaProgetto.DataValueField = "IDRegioneCompetenza"
                ddlCompetenzaProgetto.DataBind()
                'chiudo il datareader se aperto
                If Not dtrCompetenze Is Nothing Then
                    dtrCompetenze.Close()
                    dtrCompetenze = Nothing
                End If
            End If
            'Controllo abilitazione scelta
            If Session("TipoUtente") = "U" Then
                ddlCompetenzaProgetto.Enabled = True
                ddlCompetenzaProgetto.SelectedIndex = 0

            Else
                'CboCompetenza.SelectedIndex = 1
                ddlCompetenzaProgetto.Enabled = False
                'preparo la query
                strSQL = "select b.IdRegioneCompetenza ,b.Heliosread from RegioniCompetenze a "
                strSQL = strSQL & "INNER JOIN utentiunsc b ON a.idregionecompetenza = b.idregionecompetenza "
                strSQL = strSQL & "where b.username = '" & Session("Utente") & "'"
                'chiudo il datareader se aperto
                If Not dtrCompetenze Is Nothing Then
                    dtrCompetenze.Close()
                    dtrCompetenze = Nothing
                End If
                'controllo se utente o ente regionale
                'eseguo la query
                dtrCompetenze = ClsServer.CreaDatareader(strSQL, Session("conn"))
                dtrCompetenze.Read()
                If dtrCompetenze.HasRows = True Then
                    ddlCompetenzaProgetto.SelectedValue = dtrCompetenze("IdRegioneCompetenza")
                    If dtrCompetenze("Heliosread") = True Then
                        ddlCompetenzaProgetto.Enabled = True
                    End If

                End If

            End If
        Catch ex As Exception
            Response.Write(ex.Message.ToString())
            If Not dtrCompetenze Is Nothing Then
                dtrCompetenze.Close()
                dtrCompetenze = Nothing
            End If
        End Try
        If Not dtrCompetenze Is Nothing Then
            dtrCompetenze.Close()
            dtrCompetenze = Nothing
        End If
    End Sub
    Sub RicercaEnte()
        Dim strSql As String
        Dim dtsRicerca As DataSet

        strSql = " SELECT distinct enti.IDEnte, enti.Denominazione + ' (' + enti.CodiceRegione + ')' as Ente, RegioniCompetenze.Descrizione"
        strSql &= " FROM enti INNER JOIN "
        strSql &= " RegioniCompetenze ON enti.IdRegioneCompetenza = RegioniCompetenze.IdRegioneCompetenza "
        strSql &= " INNER JOIN attività ON enti.IDEnte = attività.IDEntePresentante "
        strSql &= " INNER JOIN TipiProgetto ON Attività.IdTipoProgetto = TipiProgetto.IdTipoProgetto "
        strSql &= " WHERE enti.idstatoente in(3,9, 10) "

        If Trim(TxtDescrEnte.Text) <> "" Then
            strSql &= " AND enti.denominazione like '" & Replace(TxtDescrEnte.Text, "'", "''") & "%'"
        End If
        If Trim(TxtCodEnte.Text) <> "" Then
            strSql &= " AND enti.Codiceregione ='" & Replace(TxtCodEnte.Text, "'", "''") & "'"
        End If
        If ddlCompetenza.SelectedValue <> "" Then
            Select Case ddlCompetenza.SelectedValue
                Case 0
                    strSql = strSql & ""
                Case -1
                    strSql &= " AND  enti.IdRegioneCompetenza = 22"
                Case -2
                    strSql &= " AND  enti.IdRegioneCompetenza <> 22 And not enti.IdRegioneCompetenza is null "
                Case -3
                    strSql &= " AND  enti.IdRegioneCompetenza is null "
                Case Else
                    strSql &= " AND  enti.IdRegioneCompetenza = " & ddlCompetenza.SelectedValue
            End Select

        End If

        If ddlCompetenzaProgetto.SelectedValue <> "" Then
            Select Case ddlCompetenzaProgetto.SelectedValue
                Case 0
                    strSql = strSql & ""
                Case -1
                    strSql &= " AND  attività.IdRegioneCompetenza = 22"
                Case -2
                    strSql &= " AND  attività.IdRegioneCompetenza <> 22 And not enti.IdRegioneCompetenza is null "
                Case -3
                    strSql &= " AND  attività.IdRegioneCompetenza is null "
                Case Else
                    strSql &= " AND  attività.IdRegioneCompetenza = " & ddlCompetenzaProgetto.SelectedValue
            End Select
        End If
        strSql &= " and TipiProgetto.MacroTipoProgetto like '" & Session("FiltroVisibilita") & "' " 'CONTROLLO SU ABILITAZIONE (17.02.15)
        Session("dtsRicerca") = ClsServer.DataSetGenerico(strSql, Session("conn"))
        CaricaDataGrid(dgEnte)

    End Sub
    Private Sub CaricaDataGrid(ByRef GriddaCaricare As DataGrid)
        'assegno il dataset alla griglia del risultato
        GriddaCaricare.DataSource = Session("dtsRicerca")
        GriddaCaricare.DataBind()
    End Sub

    Private Sub dgEnte_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgEnte.ItemCommand
        Select Case e.CommandName
            Case "seleziona"
                Session("dtsRicerca") = Nothing
                Response.Write("<script>" & vbCrLf)



                Response.Write("window.opener.document.getElementById(""" & Request.QueryString("objIdEnte") & """).value='" & e.Item.Cells(3).Text & "';")
                Response.Write("window.opener.document.getElementById(""" & Request.QueryString("objDescrEnte") & """).value='" & e.Item.Cells(1).Text.Replace("'", "") & "';")
                Response.Write("window.opener.document.getElementById(""" & Request.QueryString("objIdAttivita") & """).value='';")
                Response.Write("window.opener.document.getElementById(""" & Request.QueryString("objProgetto") & """).value= '';")
                Response.Write("window.opener.document.getElementById(""" & Request.QueryString("objIdEnteSedeAttuazione") & """).value='';")
                Response.Write("window.opener.document.getElementById(""" & Request.QueryString("objSede") & """).value='';")
                Response.Write("window.opener.document.getElementById(""" & Request.QueryString("objEnteF") & """).value='';")
                ''   Response.Write("window.opener.document.getElementById(""" & Request.QueryString("IdImgRicercaProgetto") & """).disabled = false;")
                ''  Response.Write("window.opener.document.getElementById(""" & Request.QueryString("IdimgRicercaSede") & """).disabled = false;")


                Response.Write("window.close()")
                Response.Write("</script>")
        End Select
    End Sub

    Private Sub cmdChiudi_Click(sender As Object, e As EventArgs) Handles cmdChiudi.Click
        Session("dtsRicerca") = Nothing
        Response.Write("<script>" & vbCrLf)
        Response.Write("window.close()")
        Response.Write("</script>")
    End Sub

    Private Sub dgEnte_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dgEnte.SelectedIndexChanged

    End Sub

    Private Sub dgEnte_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgEnte.PageIndexChanged
        dgEnte.SelectedIndex = -1
        dgEnte.EditItemIndex = -1
        dgEnte.CurrentPageIndex = e.NewPageIndex
        CaricaDataGrid(dgEnte)
    End Sub

    
   
End Class
