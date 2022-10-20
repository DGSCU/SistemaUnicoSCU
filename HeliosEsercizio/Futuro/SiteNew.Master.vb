Public Class SiteNew
	Inherits System.Web.UI.MasterPage

	Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        '**** Creazione Menu
        'HELIOS

        Dim strSql As String
        Dim dttMenu As DataTable
        Dim HtmlMenu As New StringBuilder

        'la voce di menu "Iscrizione all'Albo" deve diventare "Adeguamento Iscrizione" quando l'ente non e' in iscrizione
        Dim testoAlternativo As String = "VociMenu.TestoAlternativo"
        If Session("IdStatoEnte") <> 6 Then testoAlternativo = "replace(VociMenu.TestoAlternativo, 'Area Iscrizione all''Albo', 'Area Adeguamento Iscrizione') as TestoAlternativo"

        If Session("Sistema") = "Helios" Then
            strSql = " SELECT  distinct  VociMenu.IdVoceMenu, VociMenu.VoceMenu, VociMenu.ImmaginePassiva, VociMenu.ImmagineAttiva, VociMenu.Link, "
            strSql = strSql & " VociMenu.IdVoceMenuPadre, VociMenu.EnteNecessario," & testoAlternativo & ",VociMenu.Ordine"
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
            strSql = strSql & " AND AssociaUtenteGruppo.username ='" & Session("Utente") & "' and (RestrizioniUtenteVociMenu.TipoRestrizione <> 0 or RestrizioniUtenteVociMenu.TipoRestrizione is null) order by VociMenu.Ordine"
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
            view.RowFilter = "IdVoceMenuPadre is NULL"

            Dim frase As String = " (clicca per Aprire menu)"
            For Each row In view
                Dim menuItem As MenuItem = New MenuItem(row("VoceMenu").ToString(), row("IdVoceMenu").ToString(), row("ImmaginePassiva").ToString())
                'menuItem.Selectable = False
                'menuItem.SeparatorImageUrl = "/images/separatorecup.png"
                menuItem.NavigateUrl = row("Link").ToString()
                menuItem.Selectable = True

                If menuItem.Value = 30 And Session("Padre30") = True Then
                    AddChildItems(dttMenu, menuItem)
                    menuItem.ImageUrl = row("ImmaginePassiva").ToString()
                    frase = " (clicca per Chiudere menu)"
                    menuItem.ToolTip = row("TestoAlternativo").ToString() & frase
                End If
                If menuItem.Value = 30 And Session("Padre30") = False Then
                    menuItem.ImageUrl = row("ImmagineAttiva").ToString()
                    frase = " (clicca per Aprire menu)"
                    menuItem.ToolTip = row("TestoAlternativo").ToString() & frase
                End If




                If menuItem.Value = 41 And Session("Padre41") = True Then
                    AddChildItems(dttMenu, menuItem)
                    menuItem.ImageUrl = row("ImmaginePassiva").ToString()
                    frase = " (clicca per Chiudere menu)"
                    menuItem.ToolTip = row("TestoAlternativo").ToString() & frase
                End If
                If menuItem.Value = 41 And Session("Padre41") = False Then
                    menuItem.ImageUrl = row("ImmagineAttiva").ToString()
                    frase = " (clicca per Aprire menu)"
                    menuItem.ToolTip = row("TestoAlternativo").ToString() & frase
                End If



                If menuItem.Value = 69 And Session("Padre69") = True Then
                    AddChildItems(dttMenu, menuItem)
                    menuItem.ImageUrl = row("ImmaginePassiva").ToString()
                    frase = " (clicca per Chiudere menu)"
                    menuItem.ToolTip = row("TestoAlternativo").ToString() & frase
                End If
                If menuItem.Value = 69 And Session("Padre69") = False Then
                    menuItem.ImageUrl = row("ImmagineAttiva").ToString()
                    frase = " (clicca per aprire menu)"
                    menuItem.ToolTip = row("TestoAlternativo").ToString() & frase
                End If




                If menuItem.Value = 173 And Session("Padre173") = True Then
                    AddChildItems(dttMenu, menuItem)
                    menuItem.ImageUrl = row("ImmaginePassiva").ToString()
                    frase = " (clicca per Chiudere menu)"
                    menuItem.ToolTip = row("TestoAlternativo").ToString() & frase
                End If
                If menuItem.Value = 173 And Session("Padre173") = False Then
                    menuItem.ImageUrl = row("ImmagineAttiva").ToString()
                    frase = " (clicca per aprire menu)"
                    menuItem.ToolTip = row("TestoAlternativo").ToString() & frase
                End If




                If menuItem.Value = 84 And Session("Padre84") = True Then
                    AddChildItems(dttMenu, menuItem)
                    menuItem.ImageUrl = row("ImmaginePassiva").ToString()
                    frase = " (clicca per Chiudere menu)"
                    menuItem.ToolTip = row("TestoAlternativo").ToString() & frase
                End If
                If menuItem.Value = 84 And Session("Padre84") = False Then
                    menuItem.ImageUrl = row("ImmagineAttiva").ToString()
                    frase = " (clicca per aprire menu)"
                    menuItem.ToolTip = row("TestoAlternativo").ToString() & frase
                End If



                If menuItem.Value = 38 And Session("Padre38") = True Then
                    AddChildItems(dttMenu, menuItem)
                    menuItem.ImageUrl = row("ImmaginePassiva").ToString()
                    frase = " (clicca per Chiudere menu)"
                    menuItem.ToolTip = row("TestoAlternativo").ToString() & frase
                End If
                If menuItem.Value = 38 And Session("Padre38") = False Then
                    menuItem.ImageUrl = row("ImmagineAttiva").ToString()
                    frase = " (clicca per Chiudere menu)"
                    menuItem.ToolTip = row("TestoAlternativo").ToString() & frase
                End If

                If menuItem.Value = 208 And Session("Padre208") = True Then
                    AddChildItems(dttMenu, menuItem)
                    menuItem.ImageUrl = row("ImmaginePassiva").ToString()
                    frase = " (clicca per Chiudere menu)"
                    menuItem.ToolTip = row("TestoAlternativo").ToString() & frase
                End If
                If menuItem.Value = 208 And Session("Padre208") = False Then
                    menuItem.ImageUrl = row("ImmagineAttiva").ToString()
                    frase = " (clicca per Chiudere menu)"
                    menuItem.ToolTip = row("TestoAlternativo").ToString() & frase
                End If

                Dim voceMenu As String = row("TestoAlternativo").ToString().Replace("Area ", "")

                If Session("IdStatoEnte") <> 6 Or voceMenu Like "Iscrizione*" Or voceMenu Like "Esci*" Then
                    HtmlMenu.Append("<li class=""nav-item dropdown"">")
                    HtmlMenu.Append("<a class=""nav-link dropdown-toggle"" href=""" & menuItem.NavigateUrl & """ data-toggle=""dropdown"" aria-expanded=""false"">")
                    HtmlMenu.Append("<span>" & voceMenu & "</span>")
                    HtmlMenu.Append("<svg class=""icon icon-xs""><use xlink:href=""/bootstrap-italia/dist/svg/sprite.svg#it-expand""></use></svg>")
                    HtmlMenu.Append("</a>")
                    HtmlMenu.Append("<div class=""dropdown-menu""><div class=""link-list-wrapper"">")
                    HtmlMenu.Append("<ul class=""link-list"">")
                    For Each subItem As MenuItem In menuItem.ChildItems
                        HtmlMenu.Append("<li><a class=""list-item"" href=""" & subItem.NavigateUrl & """><span>" & subItem.Text & "</span></a></li>")
                    Next
                    HtmlMenu.Append("</ul>")
                    HtmlMenu.Append("</div></div>")
                    HtmlMenu.Append("</li>")
                End If


            Next

        End If






        'FUTURO

        If Session("Sistema") = "Futuro" Then
            strSql = " SELECT  distinct  VociMenu.IdVoceMenu, VociMenu.VoceMenu, VociMenu.ImmaginePassiva, VociMenu.ImmagineAttiva, VociMenu.Link, "
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
            strSql = strSql & " AND AssociaUtenteGruppo.username ='" & Session("Utente") & "' and (RestrizioniUtenteVociMenu.TipoRestrizione <> 0 or RestrizioniUtenteVociMenu.TipoRestrizione is null) order by VociMenu.Ordine"
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
            view.RowFilter = "IdVoceMenuPadre is NULL"

            Dim frase As String = " (clicca per Aprire menu)"
            For Each row In view
                Dim VoceMenu = row("VoceMenu").ToString()
                Dim IdVoceMenu = row("VoceMenu").ToString()
                Dim ImmaginePassiva = row("ImmaginePassiva").ToString()

                Dim menuItem As MenuItem = New MenuItem(row("VoceMenu").ToString(), row("IdVoceMenu").ToString(), row("ImmaginePassiva").ToString())
                'menuItem.Selectable = False
                'menuItem.SeparatorImageUrl = "/images/separatorecup.png"
                menuItem.NavigateUrl = row("Link").ToString()
                menuItem.Selectable = True

                If menuItem.Value = 30 And Session("Padre30") = True Then
                    AddChildItems(dttMenu, menuItem)
                    menuItem.ImageUrl = row("ImmaginePassiva").ToString()
                    frase = " (clicca per Chiudere menu)"
                    menuItem.ToolTip = row("TestoAlternativo").ToString() & frase
                End If
                If menuItem.Value = 30 And Session("Padre30") = False Then
                    menuItem.ImageUrl = row("ImmagineAttiva").ToString()
                    frase = " (clicca per Aprire menu)"
                    menuItem.ToolTip = row("TestoAlternativo").ToString() & frase
                End If




                If menuItem.Value = 41 And Session("Padre41") = True Then
                    AddChildItems(dttMenu, menuItem)
                    menuItem.ImageUrl = row("ImmaginePassiva").ToString()
                    frase = " (clicca per Chiudere menu)"
                    menuItem.ToolTip = row("TestoAlternativo").ToString() & frase
                End If
                If menuItem.Value = 41 And Session("Padre41") = False Then
                    menuItem.ImageUrl = row("ImmagineAttiva").ToString()
                    frase = " (clicca per Aprire menu)"
                    menuItem.ToolTip = row("TestoAlternativo").ToString() & frase
                End If



                If menuItem.Value = 69 And Session("Padre69") = True Then
                    AddChildItems(dttMenu, menuItem)
                    menuItem.ImageUrl = row("ImmaginePassiva").ToString()
                    frase = " (clicca per Chiudere menu)"
                    menuItem.ToolTip = row("TestoAlternativo").ToString() & frase
                End If
                If menuItem.Value = 69 And Session("Padre69") = False Then
                    menuItem.ImageUrl = row("ImmagineAttiva").ToString()
                    frase = " (clicca per aprire menu)"
                    menuItem.ToolTip = row("TestoAlternativo").ToString() & frase
                End If




                If menuItem.Value = 173 And Session("Padre173") = True Then
                    AddChildItems(dttMenu, menuItem)
                    menuItem.ImageUrl = row("ImmaginePassiva").ToString()
                    frase = " (clicca per Chiudere menu)"
                    menuItem.ToolTip = row("TestoAlternativo").ToString() & frase
                End If
                If menuItem.Value = 173 And Session("Padre173") = False Then
                    menuItem.ImageUrl = row("ImmagineAttiva").ToString()
                    frase = " (clicca per aprire menu)"
                    menuItem.ToolTip = row("TestoAlternativo").ToString() & frase
                End If




                If menuItem.Value = 84 And Session("Padre84") = True Then
                    AddChildItems(dttMenu, menuItem)
                    menuItem.ImageUrl = row("ImmaginePassiva").ToString()
                    frase = " (clicca per Chiudere menu)"
                    menuItem.ToolTip = row("TestoAlternativo").ToString() & frase
                End If
                If menuItem.Value = 84 And Session("Padre84") = False Then
                    menuItem.ImageUrl = row("ImmagineAttiva").ToString()
                    frase = " (clicca per aprire menu)"
                    menuItem.ToolTip = row("TestoAlternativo").ToString() & frase
                End If



                If menuItem.Value = 38 And Session("Padre38") = True Then
                    AddChildItems(dttMenu, menuItem)
                    menuItem.ImageUrl = row("ImmaginePassiva").ToString()
                    frase = " (clicca per Chiudere menu)"
                    menuItem.ToolTip = row("TestoAlternativo").ToString() & frase
                End If
                If menuItem.Value = 38 And Session("Padre38") = False Then
                    menuItem.ImageUrl = row("ImmagineAttiva").ToString()
                    frase = " (clicca per Chiudere menu)"
                    menuItem.ToolTip = row("TestoAlternativo").ToString() & frase
                End If


                If menuItem.Value = 208 And Session("Padre208") = True Then
                    AddChildItems(dttMenu, menuItem)
                    menuItem.ImageUrl = row("ImmaginePassiva").ToString()
                    frase = " (clicca per Chiudere menu)"
                    menuItem.ToolTip = row("TestoAlternativo").ToString() & frase
                End If
                If menuItem.Value = 208 And Session("Padre208") = False Then
                    menuItem.ImageUrl = row("ImmagineAttiva").ToString()
                    frase = " (clicca per Chiudere menu)"
                    menuItem.ToolTip = row("TestoAlternativo").ToString() & frase
                End If

                VoceMenu = row("TestoAlternativo").ToString().Replace("Area ", "")
                HtmlMenu.Append("<li class=""nav-item dropdown"">")
                HtmlMenu.Append("<a class=""nav-link dropdown-toggle"" href=""" & menuItem.NavigateUrl & """ data-toggle=""dropdown"" aria-expanded=""false"">")
                HtmlMenu.Append("<span>" & VoceMenu & "</span>")
                HtmlMenu.Append("<svg class=""icon icon-xs""><use xlink:href=""/bootstrap-italia/dist/svg/sprite.svg#it-expand""></use></svg>")
                HtmlMenu.Append("</a>")
                HtmlMenu.Append("<div class=""dropdown-menu""><div class=""link-list-wrapper"">")
                HtmlMenu.Append("<ul class=""link-list"">")
                For Each subItem As MenuItem In menuItem.ChildItems
                    HtmlMenu.Append("<li><a class=""list-item"" href=""" & subItem.NavigateUrl & """><span>" & subItem.Text & "</span></a></li>")
                Next
                HtmlMenu.Append("</ul>")
                HtmlMenu.Append("</div></div>")
                HtmlMenu.Append("</li>")
            Next

        End If

        txtMenu.Text = HtmlMenu.ToString


    End Sub
    Sub AddChildItems(Optional ByVal dttMenu As DataTable = Nothing, Optional ByVal menuItem As MenuItem = Nothing)
        Dim viewItem As DataView = New DataView(dttMenu)
        viewItem.RowFilter = "IdVoceMenuPadre=" + menuItem.Value
        Dim childView As DataRowView

        For Each childView In viewItem
            Dim childItem As MenuItem = New MenuItem(childView("VoceMenu").ToString(), childView("IdVoceMenu").ToString(), childView("ImmaginePassiva").ToString())
            childItem.NavigateUrl = childView("Link").ToString()
            childItem.ToolTip = childView("TestoAlternativo").ToString()
            menuItem.ChildItems.Add(childItem)
            'AddChildItems(dttMenu, childItem)
        Next


    End Sub
    Private Sub HplLogOut_Click(sender As Object, e As System.EventArgs) Handles HplLogOut.Click


        ClsServer.RegistrazioneLogAccessi(Session("Account"), "LOGOUT", 1, Session("conn"))
        'Response.Redirect("LogOn.aspx?out=1")
        If Session("CodiceFiscaleUtente") IsNot Nothing Then
            Response.Redirect(ConfigurationManager.AppSettings("UrlSistemaAccesso") & "/Accesso")
        End If

        If Session("Read") = "0" Then
            Response.Redirect("LogOn.aspx?out=1")
        Else
            Response.Redirect("LogOnRead.aspx?out=1")
        End If



    End Sub

End Class