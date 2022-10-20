Public Class WfrmRicercaRicAccount
    Inherits System.Web.UI.Page
    Dim strsql As String
    Dim strsql2 As String
    Dim DTRGenerico As SqlClient.SqlDataReader

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Generato da Alessandra Taballione il 9/02/2004
        If Not Session("LogIn") Is Nothing Then
            If Session("LogIn") = False Then
                Response.Redirect("LogOn.aspx")
            End If
        Else
            Response.Redirect("LogOn.aspx")
        End If
        'Caricamento della DDList dello stato della Richiesta 
        'Con Valore di default "Richiesta Registrazione"
        lblMessaggi.Text = ""
        lblMessaggi.Visible = False
        If IsPostBack = False Then
            If Not DTRGenerico Is Nothing Then
                DTRGenerico.Close()
                DTRGenerico = Nothing
            End If
            If Request.QueryString("VengoDa") = "Password" Then
                strsql = "SELECT '0' as idstatoEnte, '' as statoente from statienti " & _
                " union " & _
                " Select idstatoEnte,statoente from statienti  " & _
                " where DefaultStato=0 and Chiuso=0 and Sospeso=0 " & _
                " order by idstatoEnte "
            Else
                strsql = "Select idstatoEnte,statoente from statienti order by idstatoEnte"
            End If
            DTRGenerico = ClsServer.CreaDatareader(strsql, Session("conn"))
            ddlStato.DataSource = DTRGenerico
            ddlStato.DataTextField = "statoente"
            ddlStato.DataValueField = "IdStatoEnte"
            ddlStato.DataBind()
            If Request.QueryString("VengoDA") = "" Then
                ddlStato.SelectedValue = "4"
            End If
            If Not DTRGenerico Is Nothing Then
                DTRGenerico.Close()
                DTRGenerico = Nothing
            End If
            'agg. il 11/10/2006 da simona cordella
            CaricaCompetenze()
        End If
    End Sub

    Private Sub CaricaCompetenze()
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
                ddlCompetenze.DataSource = dtrCompetenze
                ddlCompetenze.Items.Add("")
                ddlCompetenze.DataTextField = "Descrizione"
                ddlCompetenze.DataValueField = "IDRegioneCompetenza"
                ddlCompetenze.DataBind()
                'chiudo il datareader se aperto
                If Not dtrCompetenze Is Nothing Then
                    dtrCompetenze.Close()
                    dtrCompetenze = Nothing
                End If
            End If
            'Controllo abilitazione scelta
            If Session("TipoUtente") = "U" Then
                ddlCompetenze.Enabled = True
                ddlCompetenze.SelectedIndex = 0

            Else
                ddlCompetenze.Enabled = False
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
                    ddlCompetenze.SelectedValue = dtrCompetenze("IdRegioneCompetenza")
                    If dtrCompetenze("Heliosread") = True Then
                        ddlCompetenze.Enabled = True
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

    Private Sub cmdSalva_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdSalva.Click

        If ValidazioneServerRicerca() = True Then
            Ricerca()
        End If

    End Sub

    Private Sub cmdChiudi_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdChiudi.Click
        'Generato da Alessandra Taballione il 10/02/2004
        'Rimando l'Utente alla Pagina Iniziale
        Response.Redirect("WfrmMain.aspx")
    End Sub

    Private Sub Ricerca()
        'Generato da Alessandra Taballione il 9/02/2004
        'Creazione della stringa Sql Per la ricerca Delle richieste di Registrazione
        'Controlli formali per effetuare la ricerca
        ' modificato il 12/10/2006 da simona cordella'
        ' la password dell'ente viene presa da ENTIPASSWORD
        'If ddlStato.SelectedItem.Text = "Richiesta Registrazione" Then
        'strsql = "select TOP 1 EntiPassword.password,EntiPassword.Username,denominazione,email,enti.codiceregione,codicefiscale," & _
        '        " case len(day(datacronologia)) when 1 then '0'+ convert(varchar(20),day(datacronologia))" & _
        '        " else convert(varchar(20),day(datacronologia))  end + '/'+ " & _
        '        " case len(month(datacronologia)) when 1 then '0'+ convert(varchar(20),month(datacronologia)) " & _
        '        " else convert(varchar(20),month(datacronologia)) end + '/'+ convert(varchar(20),year(datacronologia))as datacron, " & _
        '        " noterichiestaregistrazione," & _
        '        " prefissotelefonorichiestaregistrazione + '/'+ telefonorichiestaregistrazione as telefonorichiestaregistrazione,statienti.statoente,enti.IdEnte " & _
        '" from enti " & _
        '" inner join EntiPassword ON enti.IDEnte = EntiPassword.IDEnte " & _
        '" left join cronologiaentistati on (cronologiaentistati.idente=enti.idente and cronologiaentistati.idstatoente=enti.idstatoente)" & _
        '" inner join statienti on (statienti.idstatoente=enti.idstatoEnte)" & _
        '" where not denominazione is null "
        strsql = " select EntiPassword.password,EntiPassword.Username,denominazione,email, " & _
                " enti.codiceregione,codicefiscale, " & _
                " isnull(dataricezionecartacea,'01/01/2005') as datacron," & _
                " noterichiestaregistrazione, prefissotelefonorichiestaregistrazione + '/'+ telefonorichiestaregistrazione as telefonorichiestaregistrazione," & _
                " statienti.statoente, enti.IdEnte " & _
                " from enti  inner join EntiPassword ON enti.IDEnte = EntiPassword.IDEnte  " & _
                " inner join statienti on (statienti.idstatoente=enti.idstatoEnte) " & _
                " where Not denominazione Is null and EntiPassword.username like 'E96%'  "

        'Controllo se Denominazione è valorizzata
        If Trim(txtdenominazione.Text) <> "" Then
            strsql = strsql & " and denominazione like '" & Replace(txtdenominazione.Text, "'", "''") & "%'"
        End If
        If Trim(txtCodiceEnte.Text) <> "" Then
            strsql = strsql & " and codiceregione = '" & Replace(txtCodiceEnte.Text, "'", "''") & "'"
        End If
        If Trim(txtCodFis.Text) <> "" Then
            strsql = strsql & " and CodiceFiscale = '" & Replace(txtCodFis.Text, "'", "''") & "'"
        End If
        'Controllo se Richiedente è valorizzato
        If Trim(txtRichiedente.Text) <> "" Then
            strsql = strsql & " and NoterichiestaRegistrazione like '" & Replace(txtRichiedente.Text, "'", "''") & "%'"
        End If
        'Aggiunto da Alessandra Taballione il 19/05/2005
        'Tutti gli ente che hanno il codice regione attrinuito
        If chkCodiceAttribuito.Checked = True Then
            strsql = strsql & " and  not codiceregione is null "
        End If
        'Controllo se lo stato è valorizzato
        If ddlStato.SelectedIndex >= 0 And ddlStato.SelectedItem.Text <> "" Then
            strsql = strsql & " and enti.idstatoente=" & ddlStato.SelectedValue & ""
        Else
            strsql = strsql & " and statienti.DefaultStato=0 and statienti.Chiuso=0 and statienti.Sospeso=0"
        End If
        'Controllo se la data Richiesta è valorizzata
        If Trim(txtDataDal.Text) <> "" And Trim(txtdataa.Text) = "" Then
            'strsql = strsql & " and dataCronologia >= '" & txtDataDal.Text & "'"
            strsql = strsql & " and isnull(dataricezionecartacea,'01/01/2005') >= '" & txtDataDal.Text & "'"
        End If
        If Trim(txtDataDal.Text) <> "" And Trim(txtdataa.Text) <> "" Then
            'strsql = strsql & " and dataCronologia BETWEEN '" & txtDataDal.Text & "' and '" & txtdataa.Text & "'"
            strsql = strsql & " and isnull(dataricezionecartacea,'01/01/2005') BETWEEN '" & txtDataDal.Text & "' and '" & txtdataa.Text & "'"
        End If
        If Trim(txtDataDal.Text) = "" And Trim(txtdataa.Text) <> "" Then
            'strsql = strsql & " and dataCronologia <= '" & txtdataa.Text & "'" 
            strsql = strsql & " and isnull(dataricezionecartacea,'01/01/2005') <= '" & txtdataa.Text & "'"
        End If
        'If ddlStato.SelectedItem.Text = "Richiesta Registrazione" Then
        'Effettuo Controllo sullo stato dell'ente e personalizzo la datagrid 
        'in base alle azioni che possone essere effettuate
        If ddlStato.SelectedValue = 0 Then
            strsql = strsql & " and (statienti.chiuso=0 and statienti.sospeso=0)"
        End If
        'Aggiunto il 11/10/2006 da Simona Cordella
        'Gestione delle competenze
        If ddlCompetenze.SelectedValue <> "" Then
            Select Case ddlCompetenze.SelectedValue
                Case 0
                    strsql = strsql & " "
                Case -1
                    strsql = strsql & " And enti.IdRegioneCompetenza = 22"
                Case -2
                    strsql = strsql & " And enti.IdRegioneCompetenza <> 22 And not enti.IdRegioneCompetenza is null "
                Case -3
                    strsql = strsql & " And enti.IdRegioneCompetenza is null "
                Case Else
                    strsql = strsql & " And enti.IdRegioneCompetenza = " & ddlCompetenze.SelectedValue
            End Select
        End If
        strsql2 = "Select * from  statienti where idstatoEnte=" & ddlStato.SelectedValue & ""
        If Not DTRGenerico Is Nothing Then
            DTRGenerico.Close()
            DTRGenerico = Nothing
        End If
        DTRGenerico = ClsServer.CreaDatareader(strsql2, Session("conn"))
        If DTRGenerico.HasRows = True Then
            DTRGenerico.Read()
            If DTRGenerico("chiuso") = True Then 'Richiesta Respinta
                strsql = strsql & " Order by codiceregione"
                Context.Items.Add("strsql", strsql)
                Context.Items.Add("VengoDa", Request.QueryString("VengoDa"))
                Context.Items.Add("inviapassword", "no")
                'Context.Items.Add("idstatoente", ddlStato.SelectedValue)
                If Not DTRGenerico Is Nothing Then
                    DTRGenerico.Close()
                    DTRGenerico = Nothing
                End If
                Server.Transfer("WfrmElencoDomandeAccount.aspx")
            End If
            If DTRGenerico("sospeso") = True Then 'Sospeso
                strsql = strsql & " Order by codiceregione"
                Context.Items.Add("strsql", strsql)
                Context.Items.Add("VengoDa", Request.QueryString("VengoDa"))
                'Context.Items.Add("idstatoente", ddlStato.SelectedValue)
                Context.Items.Add("inviapassword", "no")
                If Not DTRGenerico Is Nothing Then
                    DTRGenerico.Close()
                    DTRGenerico = Nothing
                End If
                Server.Transfer("WfrmElencoDomandeAccount.aspx")
            End If
            If DTRGenerico("presentazioneprogetti") = True Then 'Classe attribuita
                strsql = strsql & " Order by codiceregione"
                Context.Items.Add("strsql", strsql)
                Context.Items.Add("VengoDa", Request.QueryString("VengoDa"))
                Context.Items.Add("inviapassword", "si")
                'Context.Items.Add("idstatoente", ddlStato.SelectedValue)
                If Not DTRGenerico Is Nothing Then
                    DTRGenerico.Close()
                    DTRGenerico = Nothing
                End If
                Server.Transfer("WfrmElencoDomandeAccount.aspx")
            End If
            'If DTRGenerico("defaultStato") = True Then 'Richiesta Registrazione
            '    strsql = strsql & " Order by datacronologia , denominazione"
            '    Context.Items.Add("strsql", strsql)
            '    Context.Items.Add("VengoDa", Request.QueryString("VengoDa"))
            '    If Not DTRGenerico Is Nothing Then
            '        DTRGenerico.Close()
            '        DTRGenerico = Nothing
            '    End If
            '    Server.Transfer("wfrmRisultatoRicercaRicAccount.aspx")
            'End If
            If DTRGenerico("presentazioneprogetti") = False And DTRGenerico("defaultStato") = False And DTRGenerico("sospeso") = False And DTRGenerico("chiuso") = False Then 'Classe attribuita
                strsql = strsql & " Order by codiceregione"
                Context.Items.Add("strsql", strsql)
                Context.Items.Add("VengoDa", Request.QueryString("VengoDa"))
                Context.Items.Add("inviapassword", "si")
                If Not DTRGenerico Is Nothing Then
                    DTRGenerico.Close()
                    DTRGenerico = Nothing
                End If
                Server.Transfer("WfrmElencoDomandeAccount.aspx")
            End If
        Else
            strsql = strsql & " Order by codiceregione"
            Context.Items.Add("strsql", strsql)
            Context.Items.Add("VengoDa", Request.QueryString("VengoDa"))
            Context.Items.Add("inviapassword", "si")
            'Context.Items.Add("idstatoente", ddlStato.SelectedValue)
            If Not DTRGenerico Is Nothing Then
                DTRGenerico.Close()
                DTRGenerico = Nothing
            End If
            Server.Transfer("WfrmElencoDomandeAccount.aspx")
        End If
    End Sub

    Function ValidazioneServerRicerca() As Boolean

        Dim dataRichiestaDal As Date
        If (txtDataDal.Text.Trim <> String.Empty AndAlso Date.TryParse(txtDataDal.Text, dataRichiestaDal) = False) Then
            lblMessaggi.Visible = True
            lblMessaggi.Text = "La 'Data Richiesta Dal' non è valida. Inserire la data nel formato GG/MM/AAAA."
            txtDataDal.Focus()
            Return False
        End If

        Dim dataRicezioneAl As Date
        If (txtdataa.Text.Trim <> String.Empty AndAlso Date.TryParse(txtdataa.Text, dataRicezioneAl) = False) Then
            lblMessaggi.Visible = True
            lblMessaggi.Text = "La 'Data Richiesta Al' non è valida. Inserire la data nel formato GG/MM/AAAA."
            txtdataa.Focus()
            Return False
        End If

        Return True

    End Function
End Class