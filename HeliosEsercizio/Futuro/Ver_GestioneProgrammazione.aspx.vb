Public Class Ver_GestioneProgrammazione
    Inherits System.Web.UI.Page
    Dim CmdGenerico As SqlClient.SqlCommand
    Dim dtrgenerico As SqlClient.SqlDataReader
    'Protected WithEvents LblDescrizione As System.Web.UI.WebControls.Label
    'Protected WithEvents TxtDescrizione As System.Web.UI.WebControls.TextBox
    'Protected WithEvents LblDataInizio As System.Web.UI.WebControls.Label
    'Protected WithEvents TxtDataInizio As System.Web.UI.WebControls.TextBox
    'Protected WithEvents Image4 As System.Web.UI.WebControls.Image
    'Protected WithEvents LblDataFine As System.Web.UI.WebControls.Label
    'Protected WithEvents TxtDataFine As System.Web.UI.WebControls.TextBox
    'Protected WithEvents Image1 As System.Web.UI.WebControls.Image
    'Protected WithEvents LblNote As System.Web.UI.WebControls.Label
    'Protected WithEvents TxtNote As System.Web.UI.WebControls.TextBox
    'Protected WithEvents LblRegioneCompetenza As System.Web.UI.WebControls.Label
    'Protected WithEvents ddlCompetenza As System.Web.UI.WebControls.DropDownList
    'Protected WithEvents lblCircolare As System.Web.UI.WebControls.Label
    'Protected WithEvents ddlBando As System.Web.UI.WebControls.DropDownList
    Dim strSql As String
#Region " Codice generato da Progettazione Web Form "

    'Chiamata richiesta da Progettazione Web Form.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    'Protected WithEvents cmdSalva As System.Web.UI.WebControls.ImageButton
    'Protected WithEvents cmdChiudi As System.Web.UI.WebControls.ImageButton

    'NOTA: la seguente dichiarazione è richiesta da Progettazione Web Form.
    'Non spostarla o rimuoverla.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: questa chiamata al metodo è richiesta da Progettazione Web Form.
        'Non modificarla nell'editor del codice.
        InitializeComponent()
    End Sub

#End Region
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Inserire qui il codice utente necessario per inizializzare la pagina
        'Controllo generale presente in tutte le pagine che rimanda alla pagina di Logout
        If Not Session("LogIn") Is Nothing Then
            If Session("LogIn") = False Then
                Response.Redirect("LogOn.aspx")
            End If
        Else
            Response.Redirect("LogOn.aspx")

        End If
        If IsPostBack = False Then
            'Modificato il 14/11/2007 Carico regione competenza
            CaricaCompetenze()
            '13/06/2011 gestione Bandi 
            CaricaBando()
        End If
    End Sub

    Private Sub cmdChiudi_Click(sender As Object, e As EventArgs) Handles cmdChiudi.Click
        ClosePage()
    End Sub

    Private Sub cmdSalva_Click(sender As Object, e As EventArgs) Handles cmdSalva.Click
        Dim strNull As String = "null"
        Dim intCompetenza As Integer

        If ddlCompetenza.SelectedValue = "-1" Then
            intCompetenza = 22
        Else
            intCompetenza = ddlCompetenza.SelectedValue()
        End If

        'If ddlCompetenza.SelectedValue <> "" Then
        '    Select Case ddlCompetenza.SelectedValue
        '        'Case 0
        '        'strWhere = strWhere & ""
        '    Case -1
        '            intCompetenza = 22
        '            'Case -2
        '            'strWhere = strWhere & " and IdRegioneCompetenza <> 22 And not IdRegioneCompetenza is null "
        '            'Case -3
        '            'strWhere = strWhere & " and IdRegioneCompetenza is null "
        '        Case Else
        '            intCompetenza = ddlCompetenza.SelectedValue()
        '    End Select
        'End If
        Session("IdRegCompetenza") = intCompetenza

        strSql = "INSERT INTO TVerificheProgrammazione"
        strSql = strSql & " (IDStatoProgrammazione, Descrizione, DataInizioValidità,DataFineValidità, "
        strSql = strSql & "  DataInserimento, UserInseritore, Note,IdRegCompetenza,IDBando)"
        strSql = strSql & "VALUES  ( 1,'" & Replace(TxtDescrizione.Text, "'", "''") & "', '" & TxtDataInizio.Text & "','" & TxtDataFine.Text & "', "
        strSql = strSql & " getdate(),'" & Session("Utente") & "', "
        If TxtNote.Text = "" Then
            strSql = strSql & " " & strNull & " ,"
        Else
            strSql = strSql & " '" & Replace(TxtNote.Text, "'", "''") & "',"
        End If
        strSql = strSql & " " & Session("IdRegCompetenza") & ", " & ddlBando.SelectedValue & " )"
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
        CmdGenerico = ClsServer.EseguiSqlClient(strSql, Session("conn"))
        ClosePage()
    End Sub

    Private Sub ClosePage()
        If Request.QueryString("VengoDa") = "Programmazione" Then
            Response.Redirect("Ver_Programmazione.aspx?VengoDa=" & Request.QueryString("VengoDa") & "")
        Else
            'Response.Redirect("Ver_GestioneScenari.aspx?VengoDa=" & Request.QueryString("VengoDa") & "")

            Response.Write("<script>" & vbCrLf)
            Response.Write("window.opener.location.reload();" & vbCrLf)
            Response.Write("window.close();" & vbCrLf)
            Response.Write("</script>")

            'Response.Write("<script>" & vbCrLf)
            'Response.Write("window.close();" & vbCrLf)
            'Response.Write("</script>")
        End If
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
                ''strSQL = strSQL & " select '0','','','A' "
                ''strSQL = strSQL & " union "
                strSQL = strSQL & " select '-1',' NAZIONALE ','','B' "
                ''strSQL = strSQL & " union "
                ''strSQL = strSQL & " select '-2',' REGIONALE ','','C' "
                ''strSQL = strSQL & " union "
                ''strSQL = strSQL & " select '-3',' NON DEFINITO ','','D' "
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
                'CboCompetenza.Enabled = False
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
                    ddlCompetenza.SelectedValue = dtrCompetenze("IdRegioneCompetenza")
                    If dtrCompetenze("Heliosread") = True Then
                        ddlCompetenza.Enabled = True
                    End If
                End If

                If Session("TipoUtente") = "R" Then
                    ddlCompetenza.Enabled = False
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

    Private Sub CaricaBando()
        Dim strSql As String
        Dim rstBando As SqlClient.SqlDataReader
        Dim intIdReg As Integer
        'Gestione Bandi
        'carico i bandi secondo la regione di competenza della programmazione
        If ddlCompetenza.SelectedItem.Text <> "" Then
            ddlBando.Enabled = True
            If Not rstBando Is Nothing Then
                rstBando.Close()
                rstBando = Nothing
            End If
            If ddlCompetenza.SelectedValue = "-1" Then
                intIdReg = 22
            Else
                intIdReg = ddlCompetenza.SelectedValue
            End If

            strSql = "SELECT   bando.IDBando,bando.BandoBreve " & _
                    " FROM bando " & _
                    " INNER JOIN AssociaBandoRegioniCompetenze ON bando.IDBando = AssociaBandoRegioniCompetenze.IdBando " & _
                    " WHERE AssociaBandoRegioniCompetenze.IdRegioneCompetenza = " & intIdReg & " " & _
                    " order by bando.AnnoBreve desc "
            rstBando = ClsServer.CreaDatareader(strSql, Session("conn"))
            ddlBando.DataSource = rstBando
            ddlBando.DataTextField = "BandoBreve"
            ddlBando.DataValueField = "IDBando"
            ddlBando.DataBind()
        End If
        If Not rstBando Is Nothing Then
            rstBando.Close()
            rstBando = Nothing
        End If

    End Sub

    Private Sub ddlCompetenza_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlCompetenza.SelectedIndexChanged
        CaricaBando()
    End Sub

   
End Class
