Imports System.IO

Public Class Site
    Inherits System.Web.UI.MasterPage
    'Public Function PrevInstance() As Boolean
    '    Return (UBound(Diagnostics.Process.GetProcessesByName(Diagnostics.Process.GetCurrentProcess.ProcessName)) > 0)
    'End Function

    
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ' ''Session("conn") = New SqlClient.SqlConnection
        ' ''Session("conn").ConnectionString = "user id=sa;password=vbra250;data source=www1;persist security info=False;connect timeout=300;Max Pool Size=3000;initial catalog=test"
        ' ''Session("conn").open()
        ' ''Session("Utente") = "USPAGNULO"
        'Session("idEnte") = 347
       

        If Session("Sistema") = Nothing Then
            Response.Redirect("LogOn.aspx")
        End If
        Session("LogIn") = "True"
        If Not IsPostBack = True Then
            'lancio la routine ricorsiva che carica il menu
            CreaMenu()
            'cerco il codice regione e lo stampo nel caso in cui ci sia
            TrovaCodiceRegione()


        End If
        If Session("Denominazione") <> "" Or Session("Denominazione") <> Nothing Then
            CmdInforEnte.Visible = True
            lblInfoEnte.Visible = True
        End If

        CmdApri.UseSubmitBehavior() = False
        CmdChiudi.UseSubmitBehavior() = False
        CmdInforEnte.UseSubmitBehavior() = False
        If Session("TP") = True Then
            TuttaPagina()
        End If
        'Ciao
        If Session("IdStatoEnte") = 6 Then
            Menu1.Visible = False
        End If
    End Sub

    Sub CreaMenu(Optional ByVal NodoPadre As MenuItem = Nothing, Optional ByVal IdNodo As Integer = -1)
        'variabile stringa che contiene la query
        Dim strSql As String
        'datatable che caricherò ricorsivamente dei risultati delle query
        Dim dttMenu As DataTable
        'oggetto nodo che mi crea dinamicamente il menu
        'Dim Nodi As MenuItem
        'riga che carico e assegno alla datatable
        'Dim myRow As DataRow

        'se nodopadre è null si tratta di unnodo padre
        'If NodoPadre Is Nothing Then



        'HELIOS


        If Session("Sistema") = "Helios" Then
            strSql = " SELECT  distinct  VociMenu.IdVoceMenu, VociMenu.VoceMenu, VociMenu.ImmaginePassiva, VociMenu.ImmagineAttiva, VociMenu.Link, "
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
                Menu1.Items.Add(menuItem)



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
                Dim menuItem As MenuItem = New MenuItem(row("VoceMenu").ToString(), row("IdVoceMenu").ToString(), row("ImmaginePassiva").ToString())
                'menuItem.Selectable = False
                'menuItem.SeparatorImageUrl = "/images/separatorecup.png"
                menuItem.NavigateUrl = row("Link").ToString()
                menuItem.Selectable = True
                Menu1.Items.Add(menuItem)



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

            Next


        End If
      
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

    Sub TrovaCodiceRegione()
        'variabile stringa che contiene la query
        Dim strSql As String
        'datareader locale
        Dim dtrLocal As SqlClient.SqlDataReader

        If (Session("IdEnte") > -1) And Not (Session("IdEnte") Is Nothing) Then
            strSql = "select CodiceRegione from enti where idEnte=" & CInt(Session("IdEnte"))

            'controllo e chiudo il datareader
            If Not dtrLocal Is Nothing Then
                dtrLocal.Close()
                dtrLocal = Nothing
            End If

            'eseguo la query
            dtrLocal = ClsServer.CreaDatareader(strSql, Session("conn"))

            'controllo se il dtr contiene delle righe
            If dtrLocal.HasRows = True Then
                'leggo la prima (e univoca)
                dtrLocal.Read()
                'controllo se è a null
                If IsDBNull(dtrLocal("CodiceRegione")) = False Then
                    Session("CodiceRegioneEnte") = "[" & dtrLocal("CodiceRegione") & "]"
                    Session("txtCodEnte") = dtrLocal("CodiceRegione") 'Sessione utilizzata per il campo txtCodEnte nelle ricerche dell'UNSC
                Else 'se è null setto a nothing la sessione che utilizzo per il codice
                    Session("CodiceRegioneEnte") = Nothing
                End If
            End If

            'controllo e chiudo il datareader
            If Not dtrLocal Is Nothing Then
                dtrLocal.Close()
                dtrLocal = Nothing
            End If
            'solo se è un untente UNSC
            If (Session("TipoUtente") = "U") Then
                'LblInfoVerifiche.Visible = False
                ''cmdDettVerifica.Visible = False
                AbilitaVerifica()
            End If
        End If
    End Sub

    Private Sub AbilitaVerifica()
        Dim strsql As String
        Dim dtrGenerico As SqlClient.SqlDataReader
        'aggiunto il 11/10/2007 da simona cordella
        'Verifico se l'utente è abilitato al Menu' "Forza Modifica Comune" 
        'per visualizzare se l'ente è sottosposto a verifica ispettiva
        If Not dtrGenerico Is Nothing Then
            dtrGenerico.Close()
            dtrGenerico = Nothing
        End If
        strsql = " SELECT AssociaUtenteGruppo.username, VociMenu.IdVoceMenu, VociMenu.VoceMenu, VociMenu.ImmaginePassiva, VociMenu.ImmagineAttiva, VociMenu.Link,"
        strsql = strsql & " VociMenu.IdVoceMenuPadre"
        strsql = strsql & " FROM VociMenu"
        strsql = strsql & " INNER JOIN 	AbilitazioniProfiliVociMenu ON VociMenu.IdVoceMenu = AbilitazioniProfiliVociMenu.IdVoceMenu"
        strsql = strsql & " INNER JOIN	Profili ON AbilitazioniProfiliVociMenu.IdProfilo = Profili.IdProfilo"
        If Session("Read") <> "1" Then
            strsql = strsql & " INNER JOIN	AssociaUtenteGruppo ON Profili.IdProfilo = AssociaUtenteGruppo.IdProfilo"
        Else
            strsql = strsql & " INNER JOIN	AssociaUtenteGruppo ON Profili.IdProfilo = AssociaUtenteGruppo.IdProfiloRead"
        End If
        strsql = strsql & " LEFT JOIN 	RestrizioniUtenteVociMenu ON AssociaUtenteGruppo.UserName = RestrizioniUtenteVociMenu.UserName and RestrizioniUtenteVociMenu.IdVoceMenu=VociMenu.IdVoceMenu"
        strsql = strsql & " WHERE VociMenu.descrizione = 'Forza Visualizza Verifiche'"
        strsql = strsql & " AND AssociaUtenteGruppo.username ='" & Session("Utente") & "' and (RestrizioniUtenteVociMenu.TipoRestrizione <> 0 or RestrizioniUtenteVociMenu.TipoRestrizione is null)"
        dtrGenerico = ClsServer.CreaDatareader(strsql, Session("conn"))
        If dtrGenerico.HasRows = True Then
            If Not dtrGenerico Is Nothing Then
                dtrGenerico.Close()
                dtrGenerico = Nothing
            End If
            ControlloVerifiche()
        Else
            LblInfoVerifiche.Visible = False
            imgDettVerifica.Visible = False
        End If
        If Not dtrGenerico Is Nothing Then
            dtrGenerico.Close()
            dtrGenerico = Nothing
        End If
    End Sub

    Private Sub ControlloVerifiche()
        Dim strsql As String
        Dim dtrGenerico As SqlClient.SqlDataReader
        'aggiunto il 11/10/2007 da simona cordella
        'Controllo se l'ente è sottoposto a verifica
        'modificato il 01/02/2011 da simona cordella
        'è stato tolto dalla where IDSTATOVERIFICA =6 (in esecuzione)
        If Not dtrGenerico Is Nothing Then
            dtrGenerico.Close()
            dtrGenerico = Nothing
        End If

        strsql = "SELECT IDVerifica,IDStatoVerifica,CodiceProgetto,IDEnte ,IDAttività"
        strsql = strsql & " FROM VER_VW_RICERCA_VERIFICHE"
        strsql = strsql & " WHERE IDEnte = " & Session("IdEnte") & " "
        strsql = strsql & " and IDSTATOVERIFICA IN (11,10,7)"
        strsql = strsql & " ORDER BY IDSTATOVERIFICA DESC"
        dtrGenerico = ClsServer.CreaDatareader(strsql, Session("conn"))
        If dtrGenerico.HasRows = True Then
            dtrGenerico.Read()
            LblInfoVerifiche.Visible = True
            imgDettVerifica.Visible = True
            If dtrGenerico("IDSTATOVERIFICA") = 11 Then
                LblInfoVerifiche.Text = "Ente Sanzionato"
            Else
                LblInfoVerifiche.Text = "Ente sottoposto a verifica"
            End If
        End If
        If Not dtrGenerico Is Nothing Then
            dtrGenerico.Close()
            dtrGenerico = Nothing
        End If
    End Sub

    Protected Sub CmdChiudi_Click(sender As Object, e As EventArgs) Handles CmdChiudi.Click
        menusx.Visible = False
        CmdChiudi.Visible = False
        lblMasterChiudi.Visible = False
        CmdApri.Visible = True
        lblMasterApri.Visible = True
        'Session("TP") = Nothing
    End Sub

    Protected Sub CmdApri_Click(sender As Object, e As EventArgs) Handles CmdApri.Click
        menusx.Visible = True
        CmdChiudi.Visible = True
        lblMasterChiudi.Visible = True
        CmdApri.Visible = False
        lblMasterApri.Visible = False
        'Session("TP") = Nothing
    End Sub
    Public Sub TuttaPagina()
        menusx.Visible = False
        CmdChiudi.Visible = False
        lblMasterChiudi.Visible = False
        CmdApri.Visible = True
        lblMasterApri.Visible = True
        Session("TP") = Nothing
    End Sub
    Protected Sub OpenWindow()

        Dim url As String = "informazioniente.aspx"
        Dim s As String = "window.open('" & url + "', 'popup_window', 'width=800,height=750,left=100,top=100,resizable=yes');"
        Page.ClientScript.RegisterStartupScript(Me.GetType(), "script", s, True)

    End Sub

    Protected Sub CmdInforEnte_Click(sender As Object, e As EventArgs) Handles CmdInforEnte.Click
        OpenWindow()
    End Sub
    Private Sub Menu1_MenuItemClick(sender As Object, e As System.Web.UI.WebControls.MenuEventArgs) Handles Menu1.MenuItemClick
        Dim MenuCollapsApertoChiuso As Integer
        ' Display the text of the menu item selected by the user.
        LblInfoVerifiche.Visible = True
        LblInfoVerifiche.Text = "You selected " & e.Item.Value & "." & e.ToString

        'Session("MenuApertoChiuso") = "0"

        Select Case e.Item.Value

            Case 30
                LblInfoVerifiche.Text = "nascondi menu 30"
                'Session("MenuApertoChiuso") = "1"
                Session("Padre30") = Not Session("Padre30")

            Case 41
                LblInfoVerifiche.Text = "nascondi menu 41"
                'Session("MenuApertoChiuso") = "1"
                Session("Padre41") = Not Session("Padre41")

            Case 69
                LblInfoVerifiche.Text = "nascondi menu 69"
                'Session("MenuApertoChiuso") = "1"
                Session("Padre69") = Not Session("Padre69")

            Case 173
                LblInfoVerifiche.Text = "nascondi menu 173"
                'Session("MenuApertoChiuso") = "1"
                Session("Padre173") = Not Session("Padre173")

            Case 84
                LblInfoVerifiche.Text = "nascondi menu 84"
                'Session("MenuApertoChiuso") = "1"
                Session("Padre84") = Not Session("Padre84")

            Case 38
                LblInfoVerifiche.Text = "nascondi menu 38"
                'Session("MenuApertoChiuso") = "1"
                Session("Padre38") = Not Session("Padre38")

            Case 208
                LblInfoVerifiche.Text = "nascondi menu 208"
                'Session("MenuApertoChiuso") = "1"
                Session("Padre208") = Not Session("Padre208")

        End Select

        'Dim menuItem As MenuItem

        'For Each menuItem In Menu1.Items

        '    Menu1.it()

        'Next
        'CreaMenuChiuso
        Menu1.Items.Clear()


        CreaMenu()
    End Sub

    Private Sub HplLogOut_Click(sender As Object, e As System.EventArgs) Handles HplLogOut.Click


        ClsServer.RegistrazioneLogAccessi(Session("Account"), "LOGOUT", 1, Session("conn"))
        'Response.Redirect("LogOn.aspx?out=1")


        If Session("Read") = "0" Then
            Response.Redirect("LogOn.aspx?out=1")
        Else
            Response.Redirect("LogOnRead.aspx?out=1")
        End If

        '        Dim StrSql As String
        '        Dim cmdNote As New SqlClient.SqlCommand
        '        StrSql = "Insert Into LogAccessi"
        ' (DataOraEvento,IndirizzoIp,UserNameHelios,AccountAD,Descrizione) Values  (getdate(),'" & Request.UserHostAddress & "','" & Session("Utente") &  "','" &
        ' IIf(System.Configuration.ConfigurationManager.AppSettings("Username") Is  Nothing, Request.Headers("UserName"),
        ' System.Configuration.ConfigurationManager.AppSettings("Username")) &  "','LogOut')"

        '        cmdNote.CommandText = StrSql
        '        cmdNote.Connection =
        'VerificaSessionConn.StatoSessione(Session("conn"))
        '        cmdNote.ExecuteNonQuery()
        '        cmdNote.Connection.Close() 'DS
        '        Dim urlRE As String
        '        urlRE = RER.Tools.Configuration.UrlLogOutApplicazioni.ToString
        '        'HplLogOut1.ResolveUrl(urlRE)
        '        'HplLogOut1.ResolveClientUrl(urlRE)
        '        Response.Redirect(urlRE)

    End Sub

    'Protected Sub imgPulhelios_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles imgPulhelios.Click
    '    Response.Redirect("wfrmSistema.aspx")
    'End Sub

   
End Class