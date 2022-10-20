Public Class DettaglioFunzioni
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
        If Not IsPostBack = True Then

            iscrizione_titolare.Visible = False
            iscrizione_accoglienza.Visible = False
            adeguamento_titolare.Visible = False
            adeguamento_accoglienza.Visible = False


            Dim IdNodo As Integer = Request.QueryString("IdVoceMenu")
            If IdNodo = 2 Then 'si tratta della voce di menu "Compilazione domanda", che deve comportarsi in modo diverso dallo standard
                lblsottomenudi.Text = "Compilazione domanda"
                'comportamento differente a seconda che si tratti di Ente Titolare, Ente di Accoglienza, Iscrizione, Adeguamento
                iscrizione_titolare.Visible = (Session("IdStatoEnte") = 6 And Session("IdEntePadre") = 0)
                iscrizione_accoglienza.Visible = (Session("IdStatoEnte") = 6 And Session("IdEntePadre") > 0)
                adeguamento_titolare.Visible = (Session("IdStatoEnte") <> 6 And Session("IdEntePadre") = 0)
                adeguamento_accoglienza.Visible = (Session("IdStatoEnte") <> 6 And Session("IdEntePadre") > 0)
            Else
                'lancio la routine ricorsiva che carica il menu
                CreaMenu(Request.QueryString("IdVoceMenu").ToString)
            End If
        End If
        errorAlert()

    End Sub
    Private Sub errorAlert()
        If Session("errorMessage") & "" = "" Then Return
        Response.Write("<" & "script>alert(""" & Replace(Session("errorMessage"), """", "\""") & """)</script>")
        Session("errorMessage") = Nothing
    End Sub
    'routine ricorsiva che carica il menu
    Sub CreaMenu(Optional ByVal IdNodo As Integer = -1)
        'variabile stringa che contiene la query
        Dim strSql As String
        'datatable che caricherò ricorsivamente dei risultati delle query
        Dim dttMenu As DataTable
        'oggetto nodo che mi crea dinamicamente il menu
        'Dim Nodi As MenuItem
        'riga che carico e assegno alla datatable
        'Dim myRow As DataRow

        If controllaAbilitazione(IdNodo) <> "" Then

            'se nodopadre è null si tratta di unnodo padre
            'If NodoPadre Is Nothing Then
            If Session("Sistema") = "Helios" Then
                strSql = " SELECT  distinct   VociMenu.IdVoceMenu, VociMenu.VoceMenu, VociMenu.ImmaginePassiva, VociMenu.ImmagineAttiva, VociMenu.Link, "
                strSql = strSql & " VociMenu.IdVoceMenuPadre, VociMenu.EnteNecessario,VociMenu.TestoAlternativo,VociMenu.Ordine"
                strSql = strSql & " FROM VociMenuHelios as VociMenu"
                strSql = strSql & " INNER JOIN 	AbilitazioniProfiliVociMenu ON VociMenu.IdVoceMenu = AbilitazioniProfiliVociMenu.IdVoceMenu "
                strSql = strSql & " INNER JOIN	Profili ON AbilitazioniProfiliVociMenu.IdProfilo = Profili.IdProfilo "
                If Session("Read") <> "1" Then
                    strSql = strSql & " INNER JOIN	AssociaUtenteGruppo ON Profili.IdProfilo = AssociaUtenteGruppo.IdProfilo"
                Else
                    strSql = strSql & " INNER JOIN	AssociaUtenteGruppo ON Profili.IdProfilo = AssociaUtenteGruppo.IdProfiloRead"
                End If
                strSql = strSql & " LEFT JOIN 	RestrizioniUtenteVociMenu ON AssociaUtenteGruppo.UserName = RestrizioniUtenteVociMenu.UserName and RestrizioniUtenteVociMenu.IdVoceMenu=VociMenu.IdVoceMenu"
                strSql = strSql & " WHERE   	(VociMenu.Visibile = 1) "
                If Not CInt(Session("IdEnte")) > 0 Then
                    strSql = strSql & " AND VociMenu.EnteNecessario=0"
                End If
                strSql = strSql & " AND AssociaUtenteGruppo.username ='" & Session("Utente") & "' and (RestrizioniUtenteVociMenu.TipoRestrizione <> 0 or RestrizioniUtenteVociMenu.TipoRestrizione is null)"
                strSql = strSql & " AND VociMenu.idvocemenupadre = " & IdNodo
                strSql = strSql & " order by VociMenu.Ordine"

            End If


            If Session("Sistema") = "Futuro" Then
                strSql = " SELECT  distinct   VociMenu.IdVoceMenu, VociMenu.VoceMenu, VociMenu.ImmaginePassiva, VociMenu.ImmagineAttiva, VociMenu.Link, "
                strSql = strSql & " VociMenu.IdVoceMenuPadre, VociMenu.EnteNecessario,VociMenu.TestoAlternativo,VociMenu.Ordine"
                strSql = strSql & " FROM VociMenuFuturo as VociMenu"
                strSql = strSql & " INNER JOIN 	AbilitazioniProfiliVociMenu ON VociMenu.IdVoceMenu = AbilitazioniProfiliVociMenu.IdVoceMenu "
                strSql = strSql & " INNER JOIN	Profili ON AbilitazioniProfiliVociMenu.IdProfilo = Profili.IdProfilo "
                If Session("Read") <> "1" Then
                    strSql = strSql & " INNER JOIN	AssociaUtenteGruppo ON Profili.IdProfilo = AssociaUtenteGruppo.IdProfilo"
                Else
                    strSql = strSql & " INNER JOIN	AssociaUtenteGruppo ON Profili.IdProfilo = AssociaUtenteGruppo.IdProfiloRead"
                End If
                strSql = strSql & " LEFT JOIN 	RestrizioniUtenteVociMenu ON AssociaUtenteGruppo.UserName = RestrizioniUtenteVociMenu.UserName and RestrizioniUtenteVociMenu.IdVoceMenu=VociMenu.IdVoceMenu"
                strSql = strSql & " WHERE   	(VociMenu.Visibile = 1) "
                If Not CInt(Session("IdEnte")) > 0 Then
                    strSql = strSql & " AND VociMenu.EnteNecessario=0"
                End If
                strSql = strSql & " AND AssociaUtenteGruppo.username ='" & Session("Utente") & "' and (RestrizioniUtenteVociMenu.TipoRestrizione <> 0 or RestrizioniUtenteVociMenu.TipoRestrizione is null)"
                strSql = strSql & " AND VociMenu.idvocemenupadre = " & IdNodo
                strSql = strSql & " order by VociMenu.Ordine"

            End If
            'Else
            '' '' ''    'query per il nodo figlio 
            '' '' ''    strSql = " SELECT     VociMenu.IdVoceMenu, VociMenu.VoceMenu, VociMenu.ImmaginePassiva, VociMenu.ImmagineAttiva, VociMenu.Link, "
            '' '' ''    strSql = strSql & " VociMenu.IdVoceMenuPadre, VociMenu.EnteNecessario"
            '' '' ''    strSql = strSql & " FROM VociMenu"
            '' '' ''    strSql = strSql & " INNER JOIN 	AbilitazioniProfiliVociMenu ON VociMenu.IdVoceMenu = AbilitazioniProfiliVociMenu.IdVoceMenu "
            '' '' ''    strSql = strSql & " INNER JOIN	Profili ON AbilitazioniProfiliVociMenu.IdProfilo = Profili.IdProfilo "
            '' '' ''    If UCase(Me.TemplateSourceDirectory) <> "/HELIOSREAD" Then
            '' '' ''        strSql = strSql & " INNER JOIN	AssociaUtenteGruppo ON Profili.IdProfilo = AssociaUtenteGruppo.IdProfilo"
            '' '' ''    Else
            '' '' ''        strSql = strSql & " INNER JOIN	AssociaUtenteGruppo ON Profili.IdProfilo = AssociaUtenteGruppo.IdProfiloRead"
            '' '' ''    End If
            '' '' ''    strSql = strSql & " LEFT JOIN 	RestrizioniUtenteVociMenu ON AssociaUtenteGruppo.UserName = RestrizioniUtenteVociMenu.UserName and RestrizioniUtenteVociMenu.IdVoceMenu=VociMenu.IdVoceMenu"
            '' '' ''    strSql = strSql & " WHERE   	idvocemenupadre=" & IdNodo & "  AND (VociMenu.Visibile = 1) "
            '' '' ''    strSql = strSql & " AND AssociaUtenteGruppo.username ='" & Session("Utente") & "' and (RestrizioniUtenteVociMenu.TipoRestrizione <> 0 or RestrizioniUtenteVociMenu.TipoRestrizione is null)  order by VociMenu.Ordine"

            'End If
            '' '' ''carico il datatable locale con il risultato della query
            dttMenu = ClsServer.CreaDataTable(strSql, False, Session("conn"))
            'e per ogni sua riga compongo il menu
            'Dim cmd As SqlCommand = New SqlCommand(strSql, Session("conn"))
            'Dim da As SqlDataAdapter = New SqlDataAdapter(cmd)
            'da.Fill(dttMenu)

            Dim view As DataView = New DataView(dttMenu)
            Dim row As DataRowView
            'view.RowFilter = "IdVoceMenuPadre is NULL"

            For Each row In view
                Dim menuItem As MenuItem = New MenuItem(row("VoceMenu").ToString(), row("IdVoceMenu").ToString(), row("ImmaginePassiva").ToString())
                menuItem.NavigateUrl = row("Link").ToString()
                menuItem.ToolTip = row("TestoAlternativo").ToString()
                MenuHelios.Items.Add(menuItem)
                AddChildItems(dttMenu, menuItem)

            Next
        Else
            lblmsgerror.Text = "Utenza non abilitata alla funzione richiesta"

        End If

    End Sub
    Sub AddChildItems(Optional ByVal dttMenu As DataTable = Nothing, Optional ByVal menuItem As MenuItem = Nothing)
        Dim viewItem As DataView = New DataView(dttMenu)
        viewItem.RowFilter = "IdVoceMenuPadre=" + menuItem.Value
        Dim childView As DataRowView

        For Each childView In viewItem
            Dim childItem As MenuItem = New MenuItem(childView("VoceMenu").ToString(), childView("IdVoceMenu").ToString(), childView("ImmaginePassiva").ToString())
            'If childView("VoceMenu").ToString() = "Info/Note Generali" Then
            '    childItem.NavigateUrl = childView("Link").ToString()
            'Else
            '    childItem.NavigateUrl = childView("Link").ToString()
            'End If
            childItem.NavigateUrl = childView("Link").ToString()
            menuItem.ToolTip = childView("TestoAlternativo").ToString()
            menuItem.ChildItems.Add(childItem)
            AddChildItems(dttMenu, childItem)
        Next


    End Sub
    Function controllaAbilitazione(ByVal idNodo As Integer) As String
        Dim dtr As SqlClient.SqlDataReader
        Dim strSql As String



        If Session("Sistema") = "Helios" Then
            strSql = " SELECT  distinct   VociMenu.IdVoceMenu, VociMenu.VoceMenu, VociMenu.ImmaginePassiva, VociMenu.ImmagineAttiva, VociMenu.Link, "
            strSql = strSql & " VociMenu.IdVoceMenuPadre, VociMenu.EnteNecessario,VociMenu.TestoAlternativo,VociMenu.Ordine"
            strSql = strSql & " FROM VociMenuHelios as VociMenu"
            strSql = strSql & " INNER JOIN 	AbilitazioniProfiliVociMenu ON VociMenu.IdVoceMenu = AbilitazioniProfiliVociMenu.IdVoceMenu "
            strSql = strSql & " INNER JOIN	Profili ON AbilitazioniProfiliVociMenu.IdProfilo = Profili.IdProfilo "
            If Session("Read") <> "1" Then
                strSql = strSql & " INNER JOIN	AssociaUtenteGruppo ON Profili.IdProfilo = AssociaUtenteGruppo.IdProfilo"
            Else
                strSql = strSql & " INNER JOIN	AssociaUtenteGruppo ON Profili.IdProfilo = AssociaUtenteGruppo.IdProfiloRead"
            End If
            strSql = strSql & " LEFT JOIN 	RestrizioniUtenteVociMenu ON AssociaUtenteGruppo.UserName = RestrizioniUtenteVociMenu.UserName and RestrizioniUtenteVociMenu.IdVoceMenu=VociMenu.IdVoceMenu"
            strSql = strSql & " WHERE   	(VociMenu.Visibile = 1) "
            If Not CInt(Session("IdEnte")) > 0 Then
                strSql = strSql & " AND VociMenu.EnteNecessario=0"
            End If
            strSql = strSql & " AND AssociaUtenteGruppo.username ='" & Session("Utente") & "' and (RestrizioniUtenteVociMenu.TipoRestrizione <> 0 or RestrizioniUtenteVociMenu.TipoRestrizione is null)"
            strSql = strSql & " AND VociMenu.idvocemenu = " & idNodo
            strSql = strSql & " order by VociMenu.Ordine"
            dtr = ClsServer.CreaDatareader(strSql, Session("conn"))
            dtr.Read()
        End If

        If Session("Sistema") = "Futuro" Then
            strSql = " SELECT  distinct   VociMenu.IdVoceMenu, VociMenu.VoceMenu, VociMenu.ImmaginePassiva, VociMenu.ImmagineAttiva, VociMenu.Link, "
            strSql = strSql & " VociMenu.IdVoceMenuPadre, VociMenu.EnteNecessario,VociMenu.TestoAlternativo,VociMenu.Ordine"
            strSql = strSql & " FROM VociMenuFuturo as VociMenu"
            strSql = strSql & " INNER JOIN 	AbilitazioniProfiliVociMenu ON VociMenu.IdVoceMenu = AbilitazioniProfiliVociMenu.IdVoceMenu "
            strSql = strSql & " INNER JOIN	Profili ON AbilitazioniProfiliVociMenu.IdProfilo = Profili.IdProfilo "
            If Session("Read") <> "1" Then
                strSql = strSql & " INNER JOIN	AssociaUtenteGruppo ON Profili.IdProfilo = AssociaUtenteGruppo.IdProfilo"
            Else
                strSql = strSql & " INNER JOIN	AssociaUtenteGruppo ON Profili.IdProfilo = AssociaUtenteGruppo.IdProfiloRead"
            End If
            strSql = strSql & " LEFT JOIN 	RestrizioniUtenteVociMenu ON AssociaUtenteGruppo.UserName = RestrizioniUtenteVociMenu.UserName and RestrizioniUtenteVociMenu.IdVoceMenu=VociMenu.IdVoceMenu"
            strSql = strSql & " WHERE   	(VociMenu.Visibile = 1) "
            If Not CInt(Session("IdEnte")) > 0 Then
                strSql = strSql & " AND VociMenu.EnteNecessario=0"
            End If
            strSql = strSql & " AND AssociaUtenteGruppo.username ='" & Session("Utente") & "' and (RestrizioniUtenteVociMenu.TipoRestrizione <> 0 or RestrizioniUtenteVociMenu.TipoRestrizione is null)"
            strSql = strSql & " AND VociMenu.idvocemenu = " & idNodo
            strSql = strSql & " order by VociMenu.Ordine"
            dtr = ClsServer.CreaDatareader(strSql, Session("conn"))
            dtr.Read()
        End If


        If dtr.HasRows = True Then

            lblsottomenudi.Text = dtr.Item("VoceMenu").ToString
            If Not dtr Is Nothing Then
                dtr.Close()
                dtr = Nothing
            End If
            Return lblsottomenudi.Text
        Else
            If Not dtr Is Nothing Then
                dtr.Close()
                dtr = Nothing
            End If
            Return ""
        End If


    End Function

   
End Class