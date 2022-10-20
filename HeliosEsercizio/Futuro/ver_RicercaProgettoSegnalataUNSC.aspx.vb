Public Class ver_RicercaProgettoSegnalataUNSC
    Inherits System.Web.UI.Page
    '*** CREATA DA SIMONA CORDELLA IL 07/11/2011 ***
    '*** RICERCA PROGETTO PER LE SEGNALAZIONI UNSC ***
#Region " Codice generato da Progettazione Web Form "

    'Chiamata richiesta da Progettazione Web Form.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    'Protected WithEvents LblTitolo As System.Web.UI.WebControls.Label
    Protected WithEvents TxtTitolo As System.Web.UI.WebControls.TextBox
    'Protected WithEvents LblCodiceProgetto As System.Web.UI.WebControls.Label
    Protected WithEvents TxtCodProgetto As System.Web.UI.WebControls.TextBox
    Protected WithEvents CmdRicerca As System.Web.UI.WebControls.Button
    Protected WithEvents cmdChiudi As System.Web.UI.WebControls.Button
    'Protected WithEvents LblSettore As System.Web.UI.WebControls.Label
    'Protected WithEvents LblArea As System.Web.UI.WebControls.Label
    Protected WithEvents ddlSettore As System.Web.UI.WebControls.DropDownList
    Protected WithEvents ddlArea As System.Web.UI.WebControls.DropDownList
    Protected WithEvents dgProgetti As System.Web.UI.WebControls.DataGrid
    'Protected WithEvents LblCircolare As System.Web.UI.WebControls.Label
    Protected WithEvents ddCircolare As System.Web.UI.WebControls.DropDownList

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
            CaricaBando()
            CaricaSettore()
            CaricaCompetenzeProgetto()
        End If
    End Sub

    Sub CaricaBando()
        Dim MyDataset As DataSet
        Dim strSql As String
        ddCircolare.Items.Clear()
        'strSql = "SELECT '' AS IDBANDO,'' AS BANDOBREVE,'9999' as ANNObreve UNION select idbando, bandobreve,ANNObreve from Bando ORDER BY ANNObreve DESC "

        strSql = " SELECT '' AS IDBANDO,'' AS BANDOBREVE,'9999' as ANNObreve "
        strSql &= " UNION "
        strSql &= " SELECT bando.idbando, bando.bandobreve,bando.ANNObreve "
        strSql &= " FROM bando "
        strSql &= " INNER JOIN AssociaBandoTipiProgetto abtp on abtp.idbando =  bando.idbando"
        strSql &= " INNER JOIN TipiProgetto  tp on abtp.idtipoprogetto = tp.idtipoprogetto"
        strSql &= " WHERE tp.MacroTipoProgetto like '" & Session("FiltroVisibilita") & "' "
        strSql &= " ORDER BY ANNObreve DESC "


        MyDataset = ClsServer.DataSetGenerico(strSql, Session("conn"))
        ddCircolare.DataSource = MyDataset

        ddCircolare.DataTextField = "bandobreve"
        ddCircolare.DataValueField = "idbando"
        ddCircolare.DataBind()
    End Sub
    Private Sub CaricaSettore()

        Dim MyDataset As DataSet
        Dim strSql As String

        ddlSettore.Items.Clear()
        strSql = "SELECT '0' as IdMacroAmbitoAttività, '' as Settore FROM MacroAmbitiAttività " & _
                 "UNION SELECT IdMacroAmbitoAttività, Codifica + ' - ' + MacroAmbitoAttività as Settore FROM MacroAmbitiAttività"
        MyDataset = ClsServer.DataSetGenerico(strSql, Session("conn"))
        ddlSettore.DataSource = MyDataset
        ddlSettore.DataValueField = "IdMacroAmbitoAttività"
        ddlSettore.DataTextField = "Settore"
        ddlSettore.DataBind()

    End Sub
    Private Sub CaricaArea(ByVal idSett As Integer)

        'Carico combo area
        Dim MyDataset As DataSet
        Dim Strsql As String
        Try

            ddlArea.Enabled = True
            ddlArea.Items.Clear()
            Strsql = "SELECT '0' as IDAmbitoAttività, '' as area from ambitiattività UNION SELECT IDAmbitoAttività, (Codifica + ' - ' + AmbitoAttività) AS Area  FROM ambitiattività WHERE IDMacroAmbitoAttività = " & idSett
            MyDataset = ClsServer.DataSetGenerico(Strsql, Session("conn"))
            ddlArea.DataSource = MyDataset
            ddlArea.DataTextField = "Area"
            ddlArea.DataValueField = "IDAmbitoAttività"
            ddlArea.DataBind()

            If idSett = 0 Then
                ddlArea.Enabled = False
            End If

        Catch ex As Exception

        End Try
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
    Private Sub ddlSettore_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlSettore.SelectedIndexChanged
        If ddlSettore.SelectedItem.Text <> "" Then
            ddlArea.Enabled = True
            CaricaArea(ddlSettore.SelectedValue)
        Else
            ddlArea.Items.Add("")
            ddlArea.SelectedIndex = 0
            ddlArea.Enabled = False
        End If
    End Sub

    Sub RicercaProgetto()
        Dim strSql As String
        Dim strWhere As String
        Dim dtsRicerca As DataSet

        strSql = "  SELECT distinct a.Titolo + ' (' + a.codiceente + ')' as progetto, "
        strSql &= " macroambitiattività.macroambitoattività   + ' - ' + ambitiattività.ambitoattività as settore, "
        strSql &= " a.IDAttività as  IDAttivita, BANDO.bandobreve as circolare,"
        strSql &= " count(distinct e.identità) as NumeroVolontari "
        strSql &= " FROM attività a "
        strSql &= " INNER JOIN ambitiattività ON a.IDAmbitoAttività = ambitiattività.IDAmbitoAttività "
        strSql &= " INNER JOIN macroambitiattività ON ambitiattività.IDMacroAmbitoAttività = macroambitiattività.IDMacroAmbitoAttività "
        strSql &= " INNER join attivitàentisediattuazione aesa on a.IDAttività =aesa.IDAttività "
        strSql &= " INNER join attivitàentità ae on ae.IDAttivitàentesedeattuazione =aesa.IDAttivitàentesedeattuazione "
        strSql &= " INNER join entità e on ae.identità =e.identità "
        strSql &= " INNER JOIN BandiAttività ON a.IDBandoAttività = BandiAttività.IdBandoAttività  "
        strSql &= " INNER JOIN bando ON BandiAttività.IdBando = bando.IDBando "
        strSql &= " INNER JOIN enti ON enti.IDEnte = a.IDEntePresentante "
        strSql &= " INNER JOIN TipiProgetto on TipiProgetto.IdTipoProgetto = a.IdTipoProgetto"
        strSql &= " WHERE IdEntePresentante = " & Request.QueryString("IDEnte") & " "
        'MODIFICATO IL 30/11/2011 
        'And a.idStatoAttività = 1 AND a.DataFineAttività>getdate()
        'titolo progetto
        If Trim(TxtTitolo.Text) <> "" Then
            strSql &= " and a.Titolo like '" & Replace(TxtTitolo.Text, "'", "''") & "%'"
        End If
        'codiceprogetto
        If Trim(TxtCodProgetto.Text) <> "" Then
            strSql &= " and a.codiceente ='" & Replace(TxtCodProgetto.Text, "'", "''") & "'"
        End If
        'settore intervento
        If ddlSettore.SelectedValue <> 0 Then
            strSql &= "  and ambitiattività.IDAmbitoAttività  =" & ddlSettore.SelectedValue & ""
        End If
        'are intervento
        If ddlArea.SelectedValue <> "" Then
            strSql &= "  and macroambitiattività.IDMacroAmbitoAttività =" & ddlArea.SelectedValue & ""
        End If
        'circolare 
        If ddCircolare.SelectedValue <> 0 Then
            strSql &= "  and BandiAttività.IdBando =" & ddCircolare.SelectedValue & ""
        End If

        If ddlCompetenzaProgetto.SelectedValue <> "" Then
            Select Case ddlCompetenzaProgetto.SelectedValue
                Case 0
                    strSql = strSql & ""
                Case -1
                    strSql &= " AND  a.IdRegioneCompetenza = 22"
                Case -2
                    strSql &= " AND  a.IdRegioneCompetenza <> 22 And not enti.IdRegioneCompetenza is null "
                Case -3
                    strSql &= " AND  a.IdRegioneCompetenza is null "
                Case Else
                    strSql &= " AND  a.IdRegioneCompetenza = " & ddlCompetenzaProgetto.SelectedValue
            End Select
        End If



        strSql &= " AND TipiProgetto.MacroTipoProgetto like '" & Session("FiltroVisibilita") & "' "

        strSql &= " GROUP BY a.Titolo + ' (' + a.codiceente + ')' , "
        strSql &= " macroambitiattività.macroambitoattività   + ' - ' + ambitiattività.ambitoattività, a.IDAttività, BANDO.bandobreve "
        'strSql &= " having count(distinct e.identità) > 0 "

        Session("dtsRicerca") = ClsServer.DataSetGenerico(strSql, Session("conn"))
        CaricaDataGrid(dgProgetti)

    End Sub
    Private Sub CaricaDataGrid(ByRef GriddaCaricare As DataGrid)
        'assegno il dataset alla griglia del risultato
        GriddaCaricare.DataSource = Session("dtsRicerca")
        GriddaCaricare.DataBind()
    End Sub

    Private Sub CmdRicerca_Click(sender As Object, e As EventArgs) Handles CmdRicerca.Click
        RicercaProgetto()
    End Sub

    Private Sub dgProgetti_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgProgetti.ItemCommand
        Select Case e.CommandName
            Case "seleziona"

                Session("dtsRicerca") = Nothing
                Response.Write("<script>" & vbCrLf)
                Response.Write("window.opener.document.getElementById(""" & Request.QueryString("objIdAttivita") & """).value='" & e.Item.Cells(5).Text & "';")
                Response.Write("window.opener.document.getElementById(""" & Request.QueryString("objProgetto") & """).value='" & e.Item.Cells(1).Text.Replace("'", "") & "';")
                Response.Write("window.opener.document.getElementById(""" & Request.QueryString("objIdEnteSedeAttuazione") & """).value='';")
                Response.Write("window.opener.document.getElementById(""" & Request.QueryString("objSede") & """).value='';")
                Response.Write("window.opener.document.getElementById(""" & Request.QueryString("objEnteF") & """).value='';")
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

    Private Sub dgProgetti_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgProgetti.PageIndexChanged
        dgProgetti.SelectedIndex = -1
        dgProgetti.EditItemIndex = -1
        dgProgetti.CurrentPageIndex = e.NewPageIndex
        CaricaDataGrid(dgProgetti)
    End Sub

    Private Sub dgProgetti_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dgProgetti.SelectedIndexChanged

    End Sub

   
End Class
