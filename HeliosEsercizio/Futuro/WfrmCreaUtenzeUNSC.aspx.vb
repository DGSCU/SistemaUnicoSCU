Public Class WfrmCreaUtenzeUNSC
    Inherits System.Web.UI.Page
    Dim strSql As String
    Dim dtrgenerico As SqlClient.SqlDataReader
    Dim cliccatastampa As Integer

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'controllo se è stato effettuato il login
        If Not Session("LogIn") Is Nothing Then
            'se non è stato effettuato login
            If Session("LogIn") = False Then
                'carico la pagina LogOut dove svuoto eventuali session aperte
                Response.Redirect("LogOn.aspx")
            End If
        Else
            'carico la pagina LogOut dove svuoto eventuali session aperte
            Response.Redirect("LogOn.aspx")
        End If

        If Page.IsPostBack = False Then
            If Request.QueryString("IdUtente") <> "" Then
                phAssociaArea.Visible = True
                cmdGenera.Visible = False
                imgSalva.Visible = True

                lgTitoloPagina.InnerText = "Modifica Utenze"
                lblTitoloPagina.Text = "Modifica Utenze"

                If Session("optTipUteU") = "True" Then
                    optTipUteU.Checked = True
                End If

            Else
                cmdGenera.Visible = True
                imgSalva.Visible = False
                phAssociaArea.Visible = False

                lgTitoloPagina.InnerText = "Creazione Utenze"
                lblTitoloPagina.Text = "Creazione Utenze"

            End If


            cmdAnnulla.Visible = False
            cmdChiudi.Visible = True
            lblMessaggio.Visible = False
            CaricaCombo()
            CaricaGrigliaProfili()
            'optUtenzaProfilo.Checked = True
            'optUtenzaSimile.Checked = False
            cboRegComp.Enabled = False
            '--------------IN FASE DI INSERIMENTO----------------------
            '27/08/2008
            If Request.QueryString("StampaCre") = "True" Then
                Call stampacredenziali()
            End If

            If Request.QueryString("chk") = "R" Then
                If Session("TipoUtente") = "R" Then
                    optTipUteR.Checked = True
                Else
                    optTipUteU.Checked = True
                End If
            End If
            If Request.QueryString("chk") = "U" Then
                optTipUteU.Checked = True
                Session("CreazioneOK") = 0
            End If
            '----------------------------------------------------------
            '-----------------IN FASE DI MODIFICA---------------------
            If Request.QueryString("CHKProv") = "R" Then
                optTipUteR.Checked = True
                optTipUteU.Checked = False
                optTipUteU.Enabled = False
                ChkCredenzialiEmail.Visible = False
                ChkStampaCredenziali.Visible = False
                Label1.Visible = False
                TxtDominioAD.Visible = False
                phResetPassword.Visible = True
            End If
            If Request.QueryString("CHKProv") = "U" Then
                optTipUteR.Checked = False
                optTipUteR.Enabled = False
                optTipUteU.Checked = True
                ChkCredenzialiEmail.Visible = False
                ChkStampaCredenziali.Visible = False
                TxtDurataPassword.Visible = False
                lblDurataPassword.Visible = False
                TxtDominioAD.Enabled = True
                ChkCredenzialiEmail.Enabled = False
                ChkStampaCredenziali.Enabled = False
                TxtDurataPassword.Enabled = False
                lblDurataPassword.Enabled = False
                'Session("CreazioneOK") = 0
                phResetPassword.Visible = False
            End If
            '-----------------------------------------------------------
            'optTipUteU.Checked = True

            'controllo aggiunto da Jon Cruise il 16.11.2006
            'nuova gestione che permette all'utente Regionale
            'di creare nuove utenze, ovviamente regionali

            'SE SONO UTENTE R
            If Session("TipoUtente") = "R" Then
                optTipUteR.Checked = True
                optTipUteR.Enabled = False
                optTipUteU.Enabled = False
                optTipUteU.Checked = False
                txtTipoUtenza.Text = "R"
                'txtTipoUtenzaSim.Text = "R"
                If optTipUteR.Checked = True Then
                    TxtDominioAD.Visible = False
                    TxtDominioAD.Enabled = False
                    'Label1.Enabled = False
                    'Label2.Enabled = False
                    Label1.Text = ""
                    ChkCredenzialiEmail.Enabled = True
                    ChkStampaCredenziali.Enabled = True
                    ChkCredenzialiEmail.Checked = True
                    ChkStampaCredenziali.Checked = True
                    TxtDurataPassword.Enabled = True
                    lblDurataPassword.Enabled = True
                End If
            Else
                'SE SONO UTENTE U e ho ceccato U
                If optTipUteU.Checked = True Then
                    'txtTipoUtenzaSim.Text = "U"
                    txtTipoUtenza.Text = "U"
                    TxtDominioAD.Enabled = True
                    ChkCredenzialiEmail.Enabled = False
                    ChkStampaCredenziali.Enabled = False
                    TxtDurataPassword.Enabled = False
                    lblDurataPassword.Enabled = False

                Else
                    'SE SONO UTENTE U e ho ceccato R
                    optTipUteU.Checked = False
                    txtTipoUtenza.Text = "R"
                    'txtTipoUtenzaSim.Text = "R"
                    TxtDominioAD.Enabled = False
                    Label1.Enabled = False
                    ChkCredenzialiEmail.Enabled = True
                    ChkStampaCredenziali.Enabled = True

                    TxtDurataPassword.Enabled = True
                    lblDurataPassword.Enabled = True
                End If
            End If

            Session("optTipUteU") = False

            'controllo se si tratta di una modifica di un profilo
            'modifica effettuata da Jon Connery il 12/12/2006
            If Request.QueryString("IdUtente") <> "" Then
                CaricaDatiUtenteModifica(Request.QueryString("IdUtente"))
                txtUtente.ReadOnly = True
                If Session("TipoUtente") = "R" Then
                    chkReadOnly.Enabled = False
                End If
            End If

        Else

            'Non e' la prima volta che ricarico la pagina


            'SE SONO UTENTE R 
            If Session("TipoUtente") = "R" Then
                optTipUteR.Checked = True
                optTipUteR.Enabled = False
                optTipUteU.Enabled = False
                optTipUteU.Checked = False
                txtTipoUtenza.Text = "R"
                'txtTipoUtenzaSim.Text = "R"
                If optTipUteR.Checked = True Then
                    TxtDominioAD.Visible = False
                    TxtDominioAD.Enabled = False
                    Label1.Enabled = False
                    Label1.Text = ""
                    ChkCredenzialiEmail.Enabled = True
                    ChkStampaCredenziali.Enabled = True

                    TxtDurataPassword.Enabled = True
                    lblDurataPassword.Enabled = True
                End If
            Else
                'SE SONO UTENTE U e ho ceccato U
                If optTipUteU.Checked = True Then
                    'If TxtDominioAD.Text <> "" Then
                    '    Session("CreazioneOK") = 0
                    'End If
                    optTipUteU.Checked = True
                    optTipUteR.Checked = False
                    'txtTipoUtenzaSim.Text = "U"
                    txtTipoUtenza.Text = "U"
                    TxtDominioAD.Enabled = True
                    Label1.Enabled = True
                    ChkCredenzialiEmail.Enabled = False
                    ChkStampaCredenziali.Enabled = False
                    TxtDurataPassword.Enabled = False
                    lblDurataPassword.Enabled = False

                Else
                    'SE SONO UTENTE U e ho ceccato R
                    optTipUteU.Checked = False
                    optTipUteR.Checked = True
                    txtTipoUtenza.Text = "R"
                    'txtTipoUtenzaSim.Text = "R"
                    TxtDominioAD.Enabled = False
                    Label1.Enabled = False
                    ChkCredenzialiEmail.Enabled = True
                    ChkStampaCredenziali.Enabled = True

                    TxtDurataPassword.Enabled = True
                    lblDurataPassword.Enabled = True
                End If
            End If
        End If




        If Session("CreazioneOK") = 1 And Session("TipoUtente") = "U" And optTipUteR.Checked = True Then
            optTipUteR.Checked = True
            txtTipoUtenza.Text = "R"
            ' txtTipoUtenzaSim.Text = "R"
            ChkCredenzialiEmail.Enabled = True
            ChkStampaCredenziali.Enabled = True
            Label1.Enabled = False
            'TxtDominioAD.Visible = False
            TxtDominioAD.Enabled = False
            TxtDurataPassword.Enabled = True
            lblDurataPassword.Enabled = True

        Else
            If Session("CreazioneOK") = 1 And Session("TipoUtente") = "R" And optTipUteR.Checked = True Then

            Else
                Session("CreazioneOK") = 0
            End If

        End If
        If Session("CreazioneOK") = 1 And Session("TipoUtente") = "R" And optTipUteR.Checked = True Then
            optTipUteR.Checked = True
            txtTipoUtenza.Text = "R"
            'txtTipoUtenzaSim.Text = "R"
            'ChkCredenzialiEmail.Enabled = True
            'ChkStampaCredenziali.Enabled = True
            Label1.Enabled = False
            TxtDominioAD.Visible = False
            TxtDominioAD.Enabled = False
            TxtDurataPassword.Enabled = True
            lblDurataPassword.Enabled = True

        End If
        If Request.QueryString("VengoDaReset") = 71 Then
            ChkCredenzialiEmail.Visible = False
            ChkStampaCredenziali.Visible = False
            ChkCredenzialiEmail.Enabled = False
            ChkStampaCredenziali.Enabled = False
            ChkCredenzialiEmail.Checked = False
            ChkStampaCredenziali.Checked = False
            optTipUteU.Enabled = False
            optTipUteU.Checked = False
            TxtDominioAD.Enabled = False
            Label1.Enabled = False
            TxtDurataPassword.Enabled = True
            lblDurataPassword.Enabled = True
            ' txtTipoUtenzaSim.Text = "R"
            lblMessaggio.Visible = True
            If Request.QueryString("StampaCre") = "stampa" Then
                lblMessaggio.Text = "Password Inviata Stampa Creata"
            Else
                lblMessaggio.Text = "Password Inviata"
            End If
        End If
        CheckOPT()
    End Sub

    Function CaricaCombo()
        Dim MyDataset As DataSet

        ''Profili
        'cboProfili.Items.Clear()
        'strSql = "SELECT '0' As IdProfilo,'' As Descrizione FROM Profili "
        ''controllo aggiunto da Jon Cruise il 17.11.2006
        ''se utente di tipo R vado ap prendere tutti i profili di tipo R
        ''con flag REGIONALE = 1
        'If Session("TipoUtente") = "R" Then
        '    strSql = strSql & "UNION SELECT IdProfilo,Descrizione FROM Profili "
        '    strSql = strSql & "WHERE Profili.Regionale=1 "
        'Else
        '    strSql = strSql & "UNION SELECT IdProfilo,Descrizione FROM Profili WHERE Tipo = 'U' "
        'End If

        'MyDataset = ClsServer.DataSetGenerico(strSql, Session("conn"))
        'cboProfili.DataSource = MyDataset
        'cboProfili.DataValueField = "IdProfilo"
        'cboProfili.DataTextField = "Descrizione"
        'cboProfili.DataBind()

        'Regioni di competenza
        cboRegComp.Items.Clear()
        'controllo aggiunto da Jon Cruise il 17.11.2006
        'se utente di tipo R vado a prendere solo la regione di competenza
        If Session("TipoUtente") = "R" Then
            strSql = "SELECT RegioniCompetenze.IdRegioneCompetenza, RegioniCompetenze.Descrizione FROM RegioniCompetenze "
            strSql = strSql & "INNER JOIN UtentiUNSC ON UtentiUNSC.IdRegioneCompetenza=RegioniCompetenze.IdRegioneCompetenza "
            strSql = strSql & "WHERE UtentiUNSC.UserName='" & Session("Utente") & "' AND RegioniCompetenze.IdRegioneCompetenza < 22 ORDER BY RegioniCompetenze.Descrizione "
        Else
            strSql = "SELECT IdRegioneCompetenza, Descrizione FROM RegioniCompetenze WHERE IdRegioneCompetenza < 22 ORDER BY Descrizione"
        End If

        MyDataset = ClsServer.DataSetGenerico(strSql, Session("conn"))
        cboRegComp.DataSource = MyDataset
        cboRegComp.DataValueField = "IdRegioneCompetenza"
        cboRegComp.DataTextField = "Descrizione"
        cboRegComp.DataBind()


        'ProfiliRead
        cboProfiliRead.Items.Clear()
        strSql = ""
        MyDataset = Nothing
        'controllo aggiunto da Jon Cruise il 17.11.2006
        'se utente di tipo R vado ap prendere tutti i profili di tipo R
        'con flag REGIONALE = 1
        If Session("TipoUtente") = "R" Then
            strSql = strSql & "SELECT IdProfilo,Descrizione FROM Profili "
            strSql = strSql & "WHERE Profili.Regionale=1 "
        Else
            strSql = strSql & " SELECT IdProfilo,Descrizione FROM Profili WHERE Tipo = 'U' "
            If optTipUteR.Checked = True Then
                strSql = strSql & "and  Profili.Regionale=1 "
            End If
        End If

        MyDataset = ClsServer.DataSetGenerico(strSql, Session("conn"))
        cboProfiliRead.DataSource = MyDataset
        cboProfiliRead.DataValueField = "IdProfilo"
        cboProfiliRead.DataTextField = "Descrizione"
        cboProfiliRead.DataBind()

    End Function


    Private Function CaricaGrigliaProfili(Optional ByVal IdProfilo As Integer = 0)
        ' AGGIUNTO DA SC IL 27/12/2016
        'assegnazione multipla dei profili utente
        Dim MyDataset As DataSet
        'Profili
        dtgProfiliUtente.SelectedIndex = 0
        'dtgProfiliUtente.DataSource = Nothing
        'strSql = "SELECT '0' As IdProfilo,'' As Descrizione FROM Profili "

        If Session("TipoUtente") = "R" Then
            strSql = strSql & "SELECT IdProfilo,Descrizione FROM Profili "
            strSql = strSql & "WHERE Profili.Regionale=1 "
        Else
            strSql = strSql & "SELECT IdProfilo,Descrizione FROM Profili WHERE Tipo = 'U' "
            If optTipUteR.Checked = True Then
                strSql = strSql & "and  Profili.Regionale=1 "
            End If
        End If

        MyDataset = ClsServer.DataSetGenerico(strSql, Session("conn"))
        dtgProfiliUtente.DataSource = MyDataset
        dtgProfiliUtente.DataBind()


    End Function
    Private Sub stampacredenziali()
        'myWin=window.open ("WfrmPaginaDiStampaPassword.aspx?Ciao="1,"pippo","width=850,height=600,toolbar=no,location=no,menubar=no,scrollbars=yes");
        Dim IdMax As String
        Dim NomeUtente As String
        Dim PasswordUtente As String

        cliccatastampa = 1

        If Request.QueryString("VengoDaReset") = 71 Then
            If Not dtrgenerico Is Nothing Then
                dtrgenerico.Close()
                dtrgenerico = Nothing
            End If

            strSql = "select IdUtente,Username,Password1 from UtentiUNSC where idUtente=" & Request.QueryString("IdUtente") & " "
            dtrgenerico = ClsServer.CreaDatareader(strSql, Session("conn"))
            dtrgenerico.Read()
            NomeUtente = dtrgenerico("UserName")
            PasswordUtente = dtrgenerico("Password1")
            Session("CreazioneOK") = 0
            Response.Write("<SCRIPT>" & vbCrLf)
            Response.Write("window.open('WfrmPaginaDiStampaPassword.aspx?USER=" & NomeUtente & "&PASSWORD=" & PasswordUtente & "','StampaPassword','height=800,width=800,dependent=no,scrollbars=no,status=no,resizable=yes');" & vbCrLf)
            Response.Write("</SCRIPT>")
            If Not dtrgenerico Is Nothing Then
                dtrgenerico.Close()
                dtrgenerico = Nothing
            End If
        Else


            If Session("CreazioneOK") = 1 Then
                If Session("TipoUtente") = "R" And optTipUteR.Checked = True Then
                    'fare una query
                    strSql = strSql & " select max(IdUtente)as maxid from UtentiUNSC " 'where idprogetto=" & txtidprogetto.Text & ")"

                    If Not dtrgenerico Is Nothing Then
                        dtrgenerico.Close()
                        dtrgenerico = Nothing
                    End If
                    dtrgenerico = ClsServer.CreaDatareader(strSql, Session("conn"))
                    dtrgenerico.Read()
                    IdMax = dtrgenerico("maxid")
                    If Not dtrgenerico Is Nothing Then
                        dtrgenerico.Close()
                        dtrgenerico = Nothing
                    End If
                    strSql = ""
                    strSql = strSql & " select Username,Password1 from UtentiUNSC where IdUtente=" & IdMax
                    If Not dtrgenerico Is Nothing Then
                        dtrgenerico.Close()
                        dtrgenerico = Nothing
                    End If
                    dtrgenerico = ClsServer.CreaDatareader(strSql, Session("conn"))
                    dtrgenerico.Read()
                    NomeUtente = dtrgenerico("UserName")
                    PasswordUtente = dtrgenerico("Password1")
                    If Not dtrgenerico Is Nothing Then
                        dtrgenerico.Close()
                        dtrgenerico = Nothing
                    End If
                    Session("CreazioneOK") = 0
                    Response.Write("<SCRIPT>" & vbCrLf)
                    Response.Write("window.open('WfrmPaginaDiStampaPassword.aspx?USER=" & NomeUtente & "&PASSWORD=" & PasswordUtente & "','StampaPassword','height=800,width=800,dependent=no,scrollbars=no,status=no,resizable=yes');" & vbCrLf)
                    Response.Write("</SCRIPT>")
                Else
                    'se e' U
                    If Request.QueryString("chk") = "R" Then 'If optTipUteR.Checked = True Then
                        'fare una query
                        strSql = ""
                        strSql = strSql & " select max(IdUtente)as maxid from UtentiUNSC " 'where idprogetto=" & txtidprogetto.Text & ")"

                        If Not dtrgenerico Is Nothing Then
                            dtrgenerico.Close()
                            dtrgenerico = Nothing
                        End If
                        dtrgenerico = ClsServer.CreaDatareader(strSql, Session("conn"))
                        dtrgenerico.Read()
                        IdMax = dtrgenerico("maxid")
                        If Not dtrgenerico Is Nothing Then
                            dtrgenerico.Close()
                            dtrgenerico = Nothing
                        End If
                        strSql = ""
                        strSql = strSql & " select Username,Password1 from UtentiUNSC where IdUtente=" & IdMax
                        If Not dtrgenerico Is Nothing Then
                            dtrgenerico.Close()
                            dtrgenerico = Nothing
                        End If
                        dtrgenerico = ClsServer.CreaDatareader(strSql, Session("conn"))
                        dtrgenerico.Read()
                        NomeUtente = dtrgenerico("UserName")
                        PasswordUtente = dtrgenerico("Password1")
                        If Not dtrgenerico Is Nothing Then
                            dtrgenerico.Close()
                            dtrgenerico = Nothing
                        End If

                        Session("CreazioneOK") = 0
                        Response.Write("<SCRIPT>" & vbCrLf)
                        Response.Write("window.open('WfrmPaginaDiStampaPassword.aspx?USER=" & NomeUtente & "&PASSWORD=" & PasswordUtente & "','StampaPassword','height=650,width=800,dependent=no,scrollbars=no,status=no,resizable=yes');" & vbCrLf)
                        Response.Write("</SCRIPT>")

                    End If
                End If

            Else 'se non e' stata richiesta la creazione della e-mail ma solo quella della stampa entro nell'else


            End If
            Session("CreazioneOK") = 0
        End If
    End Sub

    Sub CaricaDatiUtenteModifica(ByVal strIdUtente As String)
        Dim strsql As String
        Dim mycommand As SqlClient.SqlCommand
        Dim dtrLocal As SqlClient.SqlDataReader

        mycommand = New SqlClient.SqlCommand

        mycommand.Connection = Session("conn")

        strsql = "select isnull(Nome,'') as Nome, isnull(Cognome,'') as Cognome, left(UtentiUNSC.UserName,1) as Tipo, isnull(right(UtentiUNSC.UserName,len(UtentiUNSC.UserName)-1),'') as UserName,isnull(AccountAD,'') as AccountAD, UtentiUNSC.IdRegioneCompetenza,UtentiUNSC.DurataPassword, HeliosRead, isnull(Mail,'') as Mail,  AssociaUtenteGruppo.IdProfilo,AssociaUtenteGruppo.IdProfiloRead from UtentiUNSC "
        strsql = strsql & "INNER JOIN AssociaUtenteGruppo ON UtentiUNSC.UserName = AssociaUtenteGruppo.UserName "
        strsql = strsql & "WHERE UtentiUNSC.IdUtente='" & strIdUtente & "'"

        mycommand.CommandText = strsql

        dtrLocal = mycommand.ExecuteReader

        If dtrLocal.HasRows = True Then
            dtrLocal.Read()
            txtCognome.Text = dtrLocal("Cognome")
            txtNome.Text = dtrLocal("Nome")
            txtEmail.Text = dtrLocal("Mail")
            txtTipoUtenza.Text = dtrLocal("Tipo")
            txtUtente.Text = dtrLocal("UserName")
            'cboProfili.SelectedValue = dtrLocal("IdProfilo")
            If IsDBNull(dtrLocal("IdProfiloRead")) = False Then
                cboProfiliRead.SelectedValue = dtrLocal("IdProfiloRead")
            End If
            chkReadOnly.Checked = dtrLocal("HeliosRead")
            'qui domandare se e' corretto fare cosi in caso di mancata durata password sul regresso metterci 180?
            TxtDominioAD.Text = IIf(Not IsDBNull(dtrLocal("AccountAD")), dtrLocal("AccountAD"), "")
            TxtDurataPassword.Text = IIf(Not IsDBNull(dtrLocal("DurataPassword")), dtrLocal("DurataPassword"), "180")

            If dtrLocal("Tipo") = "R" Then
                optTipUteR.Checked = True
                cboRegComp.SelectedValue = dtrLocal("IdRegioneCompetenza")
                cboRegComp.Enabled = True
            End If
        End If

        If Not dtrLocal Is Nothing Then
            dtrLocal.Close()
            dtrLocal = Nothing
        End If
        CaricaProfiliAssociati(txtTipoUtenza.Text & Replace(txtUtente.Text, "'", "''"))
    End Sub

    Private Sub CheckOPT()
        'If optUtenzaProfilo.Checked = True Then
        '    cboProfili.Enabled = True
        '    txtTipoUtenzaSim.Enabled = False
        '    txtUtenzaSim.Enabled = False
        '    cmdRicerca.Enabled = False
        'Else
        '    cboProfili.Enabled = False
        '    txtTipoUtenzaSim.Enabled = True
        '    txtUtenzaSim.Enabled = True
        '    cmdRicerca.Enabled = True
        'End If
    End Sub

    Private Sub cmdGenera_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdGenera.Click
        lblMessaggio.Visible = False
        lblmsgConf.Visible = False
        If ValidazioneServer() = True Then
            'Session("optTipUteU") = False
            Session("CreazioneOK") = 0
            If ControllaCampiNecessari() = False Then
                Response.Write("<script>")
                Response.Write("alert('Uno o più campi necessari non sono stati valorizzati.')")
                Response.Write("</script>")
                Exit Sub
            Else

                'Generazione della password
                Session("NewPassUtente") = ClsUtility.NuovaPasswordADC() 'ClsUtility.NuovaPass()


                'se l'utenza e' di tipo R e il radio botton e checcato 
                If Session("TipoUtente") = "R" And optTipUteR.Checked = True Then
                    'lblMessaggio.Text = "Confermare la password: " & Session("NewPassUtente") & " per l'utente: " & txtTipoUtenza.Text & txtUtente.Text & " ?"
                    'lblMessaggio.Text = "Confermare la creazione dell'Utenza e della Password?"

                    cmdGenera.Visible = False

                    cmdAnnulla.Visible = False
                    cmdChiudi.Visible = True
                    'lblMessaggio.Visible = True
                    optTipUteR.Enabled = False
                    optTipUteU.Enabled = False
                Else
                    'siamo nel tipo di utenza U

                    If optTipUteR.Checked = True Then

                        'lblMessaggio.Text = "Confermare la password: " & Session("NewPassUtente") & " per l'utente: " & txtTipoUtenza.Text & txtUtente.Text & " ?"
                        cmdGenera.Visible = False

                        cmdAnnulla.Visible = False
                        cmdChiudi.Visible = True
                        'lblMessaggio.Visible = True
                        optTipUteR.Enabled = False
                        optTipUteU.Enabled = False

                    Else
                        'siamo su un utenza di tipo U e abbiamo checcato U
                        If optTipUteU.Checked = True Then

                            If ControllaDominioEsistenza(0) = True Then
                                'lblMessaggio.Text = "Confermare la creazione dell'Utenza ?"
                                cmdGenera.Visible = False

                                cmdAnnulla.Visible = False
                                cmdChiudi.Visible = True
                                lblMessaggio.Visible = True
                                optTipUteR.Enabled = False
                                optTipUteU.Enabled = False
                            Else
                                lblMessaggio.Visible = True
                                lblMessaggio.Text = "Utenza di Dominio già in uso da altro utente. Impossibile Continuare."
                                'Response.Write("<script>")
                                'Response.Write("alert('Utenza di Dominio già in uso da altro utente. Impossibile Continuare.')")
                                'Response.Write("</script>")
                                cmdGenera.Visible = True

                                cmdAnnulla.Visible = False
                                cmdChiudi.Visible = True
                                lblMessaggio.Visible = False
                                optTipUteR.Enabled = True
                                optTipUteU.Enabled = True
                                Exit Sub
                            End If
                        End If
                    End If
                End If
            End If
            Session("optTipUteU") = False

            Call SubdiConferma()
        End If

    End Sub

    'Funzione che controlla se sono stati valorizzati tutti i campi (TRUE: tutti volorizzati)
    Function ControllaCampiNecessari() As Boolean
        If Trim(txtCognome.Text) = "" Then
            ControllaCampiNecessari = False
            Exit Function
        End If
        If Trim(txtNome.Text) = "" Then
            ControllaCampiNecessari = False
            Exit Function
        End If
        If Trim(txtUtente.Text) = "" Then
            ControllaCampiNecessari = False
            Exit Function
        End If
        If Trim(txtEmail.Text) = "" Then
            ControllaCampiNecessari = False
            Exit Function
        End If
        'Dominio utente U
        If optTipUteU.Checked = True Then
            If Trim(TxtDominioAD.Text) = "" Then
                ControllaCampiNecessari = False
                Exit Function
            End If
        End If
        'Durata Password
        If optTipUteR.Checked = True Then
            If Trim(TxtDurataPassword.Text) = "" Then
                ControllaCampiNecessari = False
                Exit Function
            End If
        End If

        'If optUtenzaProfilo.Checked = True Then
        '    If cboProfili.SelectedValue = 0 Then
        '        ControllaCampiNecessari = False
        '        Exit Function
        '    End If
        'Else
        '    If txtTipoUtenzaSim.Text = "" Or txtUtenzaSim.Text = "" Then
        '        ControllaCampiNecessari = False
        '        Exit Function
        '    End If
        'End If

        'If ChkCredenzialiEmail.Checked = True Then
        '    Response.Write("TE")
        'End If
        'If ChkStampaCredenziali.Checked = True Then
        '    Response.Write("TS")
        'End If
        'If ChkCredenzialiEmail.Checked = False Then
        '    Response.Write("FE")
        'End If
        'If ChkStampaCredenziali.Checked = False Then
        '    Response.Write("FS")
        'End If

        ControllaCampiNecessari = True
    End Function

    Function ControllaDominioEsistenza(ByVal modific As Integer)
        'Antonello
        Dim strUser As String = txtTipoUtenza.Text & txtUtente.Text
        Dim dtrLocal As SqlClient.SqlDataReader
        Dim Dominio As String



        'in fase di modifica
        If modific = 1 Then

            strSql = "Select AccountAD From UtentiUNSC Where IdUtente='" & Request.QueryString("IdUtente") & "' AND Abilitato=1"

            dtrLocal = ClsServer.CreaDatareader(strSql, Session("conn"))
            dtrLocal.Read()
            Dominio = dtrLocal("AccountAD")

            If Not dtrLocal Is Nothing Then
                dtrLocal.Close()
                dtrLocal = Nothing
            End If

            If TxtDominioAD.Text = Dominio Then
                ControllaDominioEsistenza = True
            Else
                strSql = "Select AccountAD From UtentiUNSC Where AccountAD='" & TxtDominioAD.Text & "' AND Abilitato=1"

                Dim myTable As DataTable = ClsServer.CreaDataTable(strSql, False, Session("conn"))
                If myTable.Rows.Count > 0 Then
                    ControllaDominioEsistenza = False
                Else
                    ControllaDominioEsistenza = True
                End If

            End If

        Else  'sono in inserimento

            strSql = "Select AccountAD From UtentiUNSC Where AccountAD='" & TxtDominioAD.Text & "' AND Abilitato=1"

            Dim myTable As DataTable = ClsServer.CreaDataTable(strSql, False, Session("conn"))
            If myTable.Rows.Count > 0 Then
                ControllaDominioEsistenza = False
            Else
                ControllaDominioEsistenza = True
            End If

        End If


    End Function

    Private Sub SubdiConferma()
        Dim ChiHoCekkato As String
        Dim stampa As Boolean
        If SalvaPassword() = True Then
            'Pulisci(True)
            If ChkStampaCredenziali.Checked = True And ChkStampaCredenziali.Enabled = True Then
                stampa = True
            End If

            If optTipUteR.Checked = True Then
                Session("optTipUteU") = False
                ChiHoCekkato = "R"
            End If
            If optTipUteU.Checked = True Then
                Session("CreazioneOK") = 0
                Session("optTipUteU") = True
                ChiHoCekkato = "U"
            End If
            lblmsgConf.Visible = True
            lblmsgConf.Text = "Inserimento eseguito con successo."
            cmdGenera.Visible = False
            If stampa = True Then
                Response.Redirect("WfrmCreaUtenzeUNSC.aspx?StampaCre=" & stampa & "&chk=" & ChiHoCekkato & "&IdUtente=" & Request.QueryString("IdUtente"))
            End If
        Else
            If optTipUteR.Checked = True Then
                pulisciseR()

            Else
                pulisciseU()
                'Pulisci(False)
            End If
        End If

    End Sub

    'Funzione per il salvataggio dei dati sul DB
    Function SalvaPassword() As Boolean
        If checkUsername(txtTipoUtenza.Text & txtUtente.Text) = False Then
            Dim allQuery As New ArrayList
            Dim myUtenza As String = txtTipoUtenza.Text & txtUtente.Text

            'If optUtenzaProfilo.Checked = True Then
            'Insert in AssociaUtenteGruppo
            ' strSql = "INSERT INTO AssociaUtenteGruppo (UserName, IdProfilo,IdProfiloRead) VALUES ('" & Replace(myUtenza, "'", "''") & "'" & _
            '          "," & cboProfili.SelectedValue & "," & cboProfiliRead.SelectedValue & ")"
            ' allQuery.Add(strSql)
            SalvaProfiliUtente(allQuery)


            If Session("TipoUtente") = "R" Then

                'SE SONO PROFILO R E VOGLIO L'EMAIL
                If optTipUteR.Checked = True And ChkCredenzialiEmail.Checked = True Then

                    'Insert in UtentiUNSC  in fase di inserimento
                    strSql = "INSERT INTO UtentiUNSC (UserName, Password, Abilitato, Nome, " & _
                                 "Cognome, HeliosRead, Mail, Password1, IdRegioneCompetenza,UserNameCreazione,DataCreazione,AccountAD,DurataPassword,CambioPassword,PasswordDaInviare) VALUES ('" & Replace(myUtenza, "'", "''") & "'," & _
                                 "'" & ClsUtility.CreatePSW(Session("NewPassUtente")) & "',1,'" & Replace(Trim(txtNome.Text), "'", "''") & "'," & _
                                 "'" & Replace(Trim(txtCognome.Text), "'", "''") & "'," & IIf(chkReadOnly.Checked = True, 1, 0) & ",'" & Replace(txtEmail.Text, "'", "''") & "','" & Session("NewPassUtente") & "'" & _
                                 "," & IIf(optTipUteU.Checked = True, "22", cboRegComp.SelectedValue) & ",'" & Session("utente") & "',Getdate(), '" & Replace(Trim(TxtDominioAD.Text), "'", "''") & "','" & TxtDurataPassword.Text & "', 1 , 1 )"
                    allQuery.Add(strSql)
                    If ClsServer.EseguiQueryColl(allQuery, Session.SessionID, Session("conn")) = True Then
                        'Recupero l'id dell'utenza appena creata
                        Dim cmdIdUtenza As New SqlClient.SqlCommand
                        Dim intNewIdUtente As Int16
                        cmdIdUtenza.CommandType = CommandType.Text
                        cmdIdUtenza.CommandText = "SELECT @@IDENTITY"
                        cmdIdUtenza.Connection = Session("conn")
                        intNewIdUtente = cmdIdUtenza.ExecuteScalar

                        'Invio della mail di creazione dell'utenza
                        Dim cmdMailUtenza As New SqlClient.SqlCommand
                        cmdMailUtenza.CommandType = CommandType.StoredProcedure
                        cmdMailUtenza.CommandText = "SP_PROCEDURAMAIL_ADC"
                        cmdMailUtenza.Connection = Session("conn")

                        Dim prmIdUtente As SqlClient.SqlParameter
                        prmIdUtente = New SqlClient.SqlParameter
                        prmIdUtente.ParameterName = "@IDUTENTE"
                        prmIdUtente.SqlDbType = SqlDbType.Int
                        prmIdUtente.Value = intNewIdUtente
                        cmdMailUtenza.Parameters.Add(prmIdUtente)

                        Dim prmTipo As SqlClient.SqlParameter
                        prmTipo = New SqlClient.SqlParameter
                        prmTipo.ParameterName = "@TIPO"
                        prmTipo.SqlDbType = SqlDbType.Char
                        prmTipo.Value = IIf(optTipUteU.Checked = True, "U", "R")
                        cmdMailUtenza.Parameters.Add(prmTipo)

                        cmdMailUtenza.ExecuteNonQuery()
                        lblmsgConf.Visible = True
                        lblmsgConf.Text = "Creazione Utenza " & txtTipoUtenza.Text & txtUtente.Text & " eseguita con successo."


                        'Response.Write("<script>")
                        'Response.Write("alert('Creazione Utenza " & txtTipoUtenza.Text & txtUtente.Text & " eseguita con successo.')")
                        'Response.Write("</script>")

                        Session("CreazioneOK") = 1
                    End If

                    Return True

                Else
                    'SE SONO PROFILO R E NON VOGLIO L'EMAIL
                    'Insert in UtentiUNSC  in fase di inserimento
                    strSql = "INSERT INTO UtentiUNSC (UserName, Password, Abilitato, Nome, " & _
                                 "Cognome, HeliosRead, Mail, Password1, IdRegioneCompetenza,UserNameCreazione,DataCreazione,AccountAD,DurataPassword,CambioPassword,PasswordDaInviare) VALUES ('" & Replace(myUtenza, "'", "''") & "'," & _
                                 "'" & ClsUtility.CreatePSW(Session("NewPassUtente")) & "',1,'" & Replace(Trim(txtNome.Text), "'", "''") & "'," & _
                                 "'" & Replace(Trim(txtCognome.Text), "'", "''") & "'," & IIf(chkReadOnly.Checked = True, 1, 0) & ",'" & Replace(txtEmail.Text, "'", "''") & "','" & Session("NewPassUtente") & "'" & _
                                 "," & IIf(optTipUteU.Checked = True, "22", cboRegComp.SelectedValue) & ",'" & Session("utente") & "',Getdate(), '" & Replace(Trim(TxtDominioAD.Text), "'", "''") & "','" & TxtDurataPassword.Text & "', 1 , 0 )"
                    allQuery.Add(strSql)
                    ClsServer.EseguiQueryColl(allQuery, Session.SessionID, Session("conn"))
                    'non deve inviare l'email

                    'Posso Stampare
                    Session("CreazioneOK") = 1
                    lblmsgConf.Visible = True
                    lblmsgConf.Text = "Creazione Utenza " & txtTipoUtenza.Text & txtUtente.Text & " eseguita con successo."
                    'Response.Write("<script>")
                    'Response.Write("alert('Creazione Utenza " & txtTipoUtenza.Text & txtUtente.Text & " eseguita con successo.')")
                    'Response.Write("</script>")

                    Return True


                End If


            Else 'SE SONO PROFILO U
                'SE e' U
                'se sei unsc U e  hai cek su R e  vuoi l'email
                If optTipUteR.Checked = True And ChkCredenzialiEmail.Checked = True Then

                    'Insert in UtentiUNSC  in fase di inserimento
                    strSql = "INSERT INTO UtentiUNSC (UserName, Password, Abilitato, Nome, " & _
                                 "Cognome, HeliosRead, Mail, Password1, IdRegioneCompetenza,UserNameCreazione,DataCreazione,AccountAD,DurataPassword,CambioPassword,PasswordDaInviare) VALUES ('" & Replace(myUtenza, "'", "''") & "'," & _
                                 "'" & ClsUtility.CreatePSW(Session("NewPassUtente")) & "',1,'" & Replace(Trim(txtNome.Text), "'", "''") & "'," & _
                                 "'" & Replace(Trim(txtCognome.Text), "'", "''") & "'," & IIf(chkReadOnly.Checked = True, 1, 0) & ",'" & Replace(txtEmail.Text, "'", "''") & "','" & Session("NewPassUtente") & "'" & _
                                 "," & IIf(optTipUteU.Checked = True, "22", cboRegComp.SelectedValue) & ",'" & Session("utente") & "',Getdate(), '" & Replace(Trim(TxtDominioAD.Text), "'", "''") & "','" & TxtDurataPassword.Text & "',1 ,1 )"
                    allQuery.Add(strSql)

                    If ClsServer.EseguiQueryColl(allQuery, Session.SessionID, Session("conn")) = True Then
                        'Recupero l'id dell'utenza appena creata
                        Dim cmdIdUtenza As New SqlClient.SqlCommand
                        Dim intNewIdUtente As Int16
                        cmdIdUtenza.CommandType = CommandType.Text
                        cmdIdUtenza.CommandText = "SELECT @@IDENTITY"
                        cmdIdUtenza.Connection = Session("conn")
                        intNewIdUtente = cmdIdUtenza.ExecuteScalar

                        'Invio della mail di creazione dell'utenza
                        Dim cmdMailUtenza As New SqlClient.SqlCommand
                        cmdMailUtenza.CommandType = CommandType.StoredProcedure
                        cmdMailUtenza.CommandText = "SP_PROCEDURAMAIL_ADC"
                        cmdMailUtenza.Connection = Session("conn")

                        Dim prmIdUtente As SqlClient.SqlParameter
                        prmIdUtente = New SqlClient.SqlParameter
                        prmIdUtente.ParameterName = "@IDUTENTE"
                        prmIdUtente.SqlDbType = SqlDbType.Int
                        prmIdUtente.Value = intNewIdUtente
                        cmdMailUtenza.Parameters.Add(prmIdUtente)

                        Dim prmTipo As SqlClient.SqlParameter
                        prmTipo = New SqlClient.SqlParameter
                        prmTipo.ParameterName = "@TIPO"
                        prmTipo.SqlDbType = SqlDbType.Char
                        prmTipo.Value = IIf(optTipUteU.Checked = True, "U", "R")
                        cmdMailUtenza.Parameters.Add(prmTipo)

                        cmdMailUtenza.ExecuteNonQuery()


                        lblmsgConf.Visible = True
                        lblmsgConf.Text = "Creazione Utenza " & txtTipoUtenza.Text & txtUtente.Text & " eseguita con successo."
                        'Response.Write("<script>")
                        'Response.Write("alert('Creazione Utenza " & txtTipoUtenza.Text & txtUtente.Text & " eseguita con successo.')")
                        'Response.Write("</script>")

                        Session("CreazioneOK") = 1
                    End If

                    Return True

                Else
                    'se sei unsc U e  hai cek su R e non vuoi l'email
                    If optTipUteR.Checked = True And ChkCredenzialiEmail.Checked = False Then

                        'Insert in UtentiUNSC  in fase di inserimento
                        strSql = "INSERT INTO UtentiUNSC (UserName, Password, Abilitato, Nome, " & _
                                     "Cognome, HeliosRead, Mail, Password1, IdRegioneCompetenza,UserNameCreazione,DataCreazione,AccountAD,DurataPassword,CambioPassword,PasswordDaInviare) VALUES ('" & Replace(myUtenza, "'", "''") & "'," & _
                                     "'" & ClsUtility.CreatePSW(Session("NewPassUtente")) & "',1,'" & Replace(Trim(txtNome.Text), "'", "''") & "'," & _
                                     "'" & Replace(Trim(txtCognome.Text), "'", "''") & "'," & IIf(chkReadOnly.Checked = True, 1, 0) & ",'" & Replace(txtEmail.Text, "'", "''") & "','" & Session("NewPassUtente") & "'" & _
                                     "," & IIf(optTipUteU.Checked = True, "22", cboRegComp.SelectedValue) & ",'" & Session("utente") & "',Getdate(), '" & Replace(Trim(TxtDominioAD.Text), "'", "''") & "','" & TxtDurataPassword.Text & "' ,1 ,0 )"
                        allQuery.Add(strSql)
                        'non deve inviare l'email
                        ClsServer.EseguiQueryColl(allQuery, Session.SessionID, Session("conn"))

                        Session("CreazioneOK") = 1
                        lblmsgConf.Visible = True
                        lblmsgConf.Text = "Creazione Utenza " & txtTipoUtenza.Text & txtUtente.Text & " eseguita con successo."
                        'Response.Write("<script>")
                        'Response.Write("alert('Creazione Utenza " & txtTipoUtenza.Text & txtUtente.Text & " eseguita con successo.')")
                        'Response.Write("</script>")

                        Return True
                    Else
                        'se sei unsc U e non hai cek su R e QUINDI STAI LAVORANDO UNA UTENZA DI TIPO U
                        'Insert in UtentiUNSC  in fase di inserimento
                        strSql = "INSERT INTO UtentiUNSC (UserName, Password, Abilitato, Nome, " & _
                                     "Cognome, HeliosRead, Mail, Password1, IdRegioneCompetenza,UserNameCreazione,DataCreazione,AccountAD,DurataPassword,CambioPassword,PasswordDaInviare) VALUES ('" & Replace(myUtenza, "'", "''") & "'," & _
                                     "'" & ClsUtility.CreatePSW(Session("NewPassUtente")) & "',1,'" & Replace(Trim(txtNome.Text), "'", "''") & "'," & _
                                     "'" & Replace(Trim(txtCognome.Text), "'", "''") & "'," & IIf(chkReadOnly.Checked = True, 1, 0) & ",'" & Replace(txtEmail.Text, "'", "''") & "','" & Session("NewPassUtente") & "'" & _
                                     "," & IIf(optTipUteU.Checked = True, "22", cboRegComp.SelectedValue) & ",'" & Session("utente") & "',Getdate(), '" & Replace(Trim(TxtDominioAD.Text), "'", "''") & "',NULL ,0 ,0 )"
                        allQuery.Add(strSql)
                        'non deve inviare l'email
                        ClsServer.EseguiQueryColl(allQuery, Session.SessionID, Session("conn"))

                        Session("CreazioneOK") = 0
                        lblmsgConf.Visible = True
                        lblmsgConf.Text = "Creazione Utenza " & txtTipoUtenza.Text & txtUtente.Text & " eseguita con successo."
                        'Response.Write("<script>")
                        'Response.Write("alert('Creazione Utenza " & txtTipoUtenza.Text & txtUtente.Text & " eseguita con successo.')")
                        'Response.Write("</script>")
                        Return True


                    End If
                End If

            End If

            'Else 'QUESTA ELSE E' RELATIVA AL TIPO DI PROFILO optUtenzaProfilo.Checked = false

            '    If txtTipoUtenzaSim.Text <> "" And txtUtenzaSim.Text <> "" Then
            '        'Insert in AssociaUtenteGruppo
            '        strSql = "INSERT INTO AssociaUtenteGruppo (UserName, IdProfilo) Select '" & Replace(myUtenza, "'", "''") & "'" & _
            '                 ",Profili.IdProfilo FROM Profili "

            '        '============================================================================================================================
            '        '====================================================30/09/2008==============================================================
            '        '=======MODIFICA EFFETTUATA DA Kronk (99 scimmie saltavano sul letto, una cadde in terra e si ruppe il cervelletto...)=======
            '        '=================AGGIUNTA JOIN SU PROFILO READ PER ABILITARE LEGGERE LE ABILITAZIONI ANCHE DELL'UTENTE READ=================
            '        '============================================================================================================================
            '        If UCase(Me.TemplateSourceDirectory) <> "/HELIOSREAD" Then
            '            strSql = strSql & " INNER JOIN	AssociaUtenteGruppo ON Profili.IdProfilo = AssociaUtenteGruppo.IdProfilo "
            '        Else
            '            strSql = strSql & " INNER JOIN	AssociaUtenteGruppo ON Profili.IdProfilo = AssociaUtenteGruppo.IdProfiloRead "
            '        End If

            '        strSql = strSql & " WHERE AssociaUtenteGruppo.UserName = '" & txtTipoUtenzaSim.Text & txtUtenzaSim.Text & "'"

            '        allQuery.Add(strSql)

            '        'Insert in UtentiUNSC
            '        'SE SEI DI TIPO R
            '        If Session("TipoUtente") = "R" Then

            '            ' SE SEI R E HAI CHEK SU R E SU EMAIL
            '            If optTipUteR.Checked = True And ChkCredenzialiEmail.Checked = True Then

            '                'Insert in UtentiUNSC  in fase di inserimento
            '                strSql = "INSERT INTO UtentiUNSC (UserName, Password, Abilitato, Nome, " & _
            '                             "Cognome, HeliosRead, Mail, Password1, IdRegioneCompetenza,UserNameCreazione,DataCreazione,AccountAD,DurataPassword,CambioPassword,PasswordDaInviare) VALUES ('" & Replace(myUtenza, "'", "''") & "'," & _
            '                             "'" & ClsUtility.CreatePSW(Session("NewPassUtente")) & "',1,'" & Replace(Trim(txtNome.Text), "'", "''") & "'," & _
            '                             "'" & Replace(Trim(txtCognome.Text), "'", "''") & "'," & IIf(chkReadOnly.Checked = True, 1, 0) & ",'" & Replace(txtEmail.Text, "'", "''") & "','" & Session("NewPassUtente") & "'" & _
            '                             "," & IIf(optTipUteU.Checked = True, "22", cboRegComp.SelectedValue) & ",'" & Session("utente") & "',Getdate(), '" & Replace(Trim(TxtDominioAD.Text), "'", "''") & "','" & TxtDurataPassword.Text & "',1 ,1 )"
            '                allQuery.Add(strSql)
            '                If ClsServer.EseguiQueryColl(allQuery, Session.SessionID, Session("conn")) = True Then
            '                    'Recupero l'id dell'utenza appena creata
            '                    Dim cmdIdUtenza As New SqlClient.SqlCommand
            '                    Dim intNewIdUtente As Int16
            '                    cmdIdUtenza.CommandType = CommandType.Text
            '                    cmdIdUtenza.CommandText = "SELECT @@IDENTITY"
            '                    cmdIdUtenza.Connection = Session("conn")
            '                    intNewIdUtente = cmdIdUtenza.ExecuteScalar

            '                    'Invio della mail di creazione dell'utenza
            '                    Dim cmdMailUtenza As New SqlClient.SqlCommand
            '                    cmdMailUtenza.CommandType = CommandType.StoredProcedure
            '                    cmdMailUtenza.CommandText = "SP_PROCEDURAMAIL_ADC"
            '                    cmdMailUtenza.Connection = Session("conn")

            '                    Dim prmIdUtente As SqlClient.SqlParameter
            '                    prmIdUtente = New SqlClient.SqlParameter
            '                    prmIdUtente.ParameterName = "@IDUTENTE"
            '                    prmIdUtente.SqlDbType = SqlDbType.Int
            '                    prmIdUtente.Value = intNewIdUtente
            '                    cmdMailUtenza.Parameters.Add(prmIdUtente)

            '                    Dim prmTipo As SqlClient.SqlParameter
            '                    prmTipo = New SqlClient.SqlParameter
            '                    prmTipo.ParameterName = "@TIPO"
            '                    prmTipo.SqlDbType = SqlDbType.Char
            '                    prmTipo.Value = IIf(optTipUteU.Checked = True, "U", "R")
            '                    cmdMailUtenza.Parameters.Add(prmTipo)

            '                    cmdMailUtenza.ExecuteNonQuery()

            '                    lblmsgConf.Visible = True
            '                    lblmsgConf.Text = "Creazione Utenza " & txtTipoUtenza.Text & txtUtente.Text & " eseguita con successo."
            '                    'Response.Write("<script>")
            '                    'Response.Write("alert('Creazione Utenza " & txtTipoUtenza.Text & txtUtente.Text & " eseguita con successo.')")
            '                    'Response.Write("</script>")

            '                    Session("CreazioneOK") = 1
            '                End If

            '                Return True


            '            Else
            '                'SE SEI R E HAI CECK SU R  E NON VUOI L'EMAIL
            '                'Insert in UtentiUNSC  in fase di inserimento
            '                strSql = "INSERT INTO UtentiUNSC (UserName, Password, Abilitato, Nome, " & _
            '                             "Cognome, HeliosRead, Mail, Password1, IdRegioneCompetenza,UserNameCreazione,DataCreazione,AccountAD,DurataPassword,CambioPassword,PasswordDaInviare) VALUES ('" & Replace(myUtenza, "'", "''") & "'," & _
            '                             "'" & ClsUtility.CreatePSW(Session("NewPassUtente")) & "',1,'" & Replace(Trim(txtNome.Text), "'", "''") & "'," & _
            '                             "'" & Replace(Trim(txtCognome.Text), "'", "''") & "'," & IIf(chkReadOnly.Checked = True, 1, 0) & ",'" & Replace(txtEmail.Text, "'", "''") & "','" & Session("NewPassUtente") & "'" & _
            '                             "," & IIf(optTipUteU.Checked = True, "22", cboRegComp.SelectedValue) & ",'" & Session("utente") & "',Getdate(), '" & Replace(Trim(TxtDominioAD.Text), "'", "''") & "','" & TxtDurataPassword.Text & "',1 , 0 )"
            '                allQuery.Add(strSql)
            '                ' non deve inviare l'e-mail
            '                ClsServer.EseguiQueryColl(allQuery, Session.SessionID, Session("conn"))

            '                Session("CreazioneOK") = 1
            '                lblmsgConf.Visible = True
            '                lblmsgConf.Text = "Creazione Utenza " & txtTipoUtenza.Text & txtUtente.Text & " eseguita con successo."
            '                'Response.Write("<script>")
            '                'Response.Write("alert('Creazione Utenza " & txtTipoUtenza.Text & txtUtente.Text & " eseguita con successo.')")
            '                'Response.Write("</script>")

            '                Return True

            '            End If

            '        Else 'SE SEI U
            '            'SE e' U
            '            'se sei unsc U e  hai cek su R e vuoi l'email
            '            If optTipUteR.Checked = True And ChkCredenzialiEmail.Checked = True Then

            '                'Insert in UtentiUNSC  in fase di inserimento
            '                strSql = "INSERT INTO UtentiUNSC (UserName, Password, Abilitato, Nome, " & _
            '                             "Cognome, HeliosRead, Mail, Password1, IdRegioneCompetenza,UserNameCreazione,DataCreazione,AccountAD,DurataPassword,CambioPassword,PasswordDaInviare) VALUES ('" & Replace(myUtenza, "'", "''") & "'," & _
            '                             "'" & ClsUtility.CreatePSW(Session("NewPassUtente")) & "',1,'" & Replace(Trim(txtNome.Text), "'", "''") & "'," & _
            '                             "'" & Replace(Trim(txtCognome.Text), "'", "''") & "'," & IIf(chkReadOnly.Checked = True, 1, 0) & ",'" & Replace(txtEmail.Text, "'", "''") & "','" & Session("NewPassUtente") & "'" & _
            '                             "," & IIf(optTipUteU.Checked = True, "22", cboRegComp.SelectedValue) & ",'" & Session("utente") & "',Getdate(), '" & Replace(Trim(TxtDominioAD.Text), "'", "''") & "','" & TxtDurataPassword.Text & "', 1 ,1 )"
            '                allQuery.Add(strSql)

            '                If ClsServer.EseguiQueryColl(allQuery, Session.SessionID, Session("conn")) = True Then
            '                    'Recupero l'id dell'utenza appena creata
            '                    Dim cmdIdUtenza As New SqlClient.SqlCommand
            '                    Dim intNewIdUtente As Int16
            '                    cmdIdUtenza.CommandType = CommandType.Text
            '                    cmdIdUtenza.CommandText = "SELECT @@IDENTITY"
            '                    cmdIdUtenza.Connection = Session("conn")
            '                    intNewIdUtente = cmdIdUtenza.ExecuteScalar

            '                    'Invio della mail di creazione dell'utenza
            '                    Dim cmdMailUtenza As New SqlClient.SqlCommand
            '                    cmdMailUtenza.CommandType = CommandType.StoredProcedure
            '                    cmdMailUtenza.CommandText = "SP_PROCEDURAMAIL_ADC"
            '                    cmdMailUtenza.Connection = Session("conn")

            '                    Dim prmIdUtente As SqlClient.SqlParameter
            '                    prmIdUtente = New SqlClient.SqlParameter
            '                    prmIdUtente.ParameterName = "@IDUTENTE"
            '                    prmIdUtente.SqlDbType = SqlDbType.Int
            '                    prmIdUtente.Value = intNewIdUtente
            '                    cmdMailUtenza.Parameters.Add(prmIdUtente)

            '                    Dim prmTipo As SqlClient.SqlParameter
            '                    prmTipo = New SqlClient.SqlParameter
            '                    prmTipo.ParameterName = "@TIPO"
            '                    prmTipo.SqlDbType = SqlDbType.Char
            '                    prmTipo.Value = IIf(optTipUteU.Checked = True, "U", "R")
            '                    cmdMailUtenza.Parameters.Add(prmTipo)

            '                    cmdMailUtenza.ExecuteNonQuery()
            '                    lblmsgConf.Visible = True
            '                    lblmsgConf.Text = "Creazione Utenza " & txtTipoUtenza.Text & txtUtente.Text & " eseguita con successo."
            '                    'Response.Write("<script>")
            '                    'Response.Write("alert('Creazione Utenza " & txtTipoUtenza.Text & txtUtente.Text & " eseguita con successo.')")
            '                    'Response.Write("</script>")

            '                    Session("CreazioneOK") = 1
            '                End If

            '                Return True

            '            Else

            '                'se sei unsc U e  hai cek su R e non vuoi l'email
            '                If optTipUteR.Checked = True And ChkCredenzialiEmail.Checked = False Then

            '                    'Insert in UtentiUNSC  in fase di inserimento
            '                    strSql = "INSERT INTO UtentiUNSC (UserName, Password, Abilitato, Nome, " & _
            '                                 "Cognome, HeliosRead, Mail, Password1, IdRegioneCompetenza,UserNameCreazione,DataCreazione,AccountAD,DurataPassword,CambioPassword,PasswordDaInviare) VALUES ('" & Replace(myUtenza, "'", "''") & "'," & _
            '                                 "'" & ClsUtility.CreatePSW(Session("NewPassUtente")) & "',1,'" & Replace(Trim(txtNome.Text), "'", "''") & "'," & _
            '                                 "'" & Replace(Trim(txtCognome.Text), "'", "''") & "'," & IIf(chkReadOnly.Checked = True, 1, 0) & ",'" & Replace(txtEmail.Text, "'", "''") & "','" & Session("NewPassUtente") & "'" & _
            '                                 "," & IIf(optTipUteU.Checked = True, "22", cboRegComp.SelectedValue) & ",'" & Session("utente") & "',Getdate(), '" & Replace(Trim(TxtDominioAD.Text), "'", "''") & "','" & TxtDurataPassword.Text & "' ,1 ,0 )"
            '                    allQuery.Add(strSql)
            '                    'non deve inviare l'email
            '                    ClsServer.EseguiQueryColl(allQuery, Session.SessionID, Session("conn"))

            '                    Session("CreazioneOK") = 1
            '                    Response.Write("<script>")
            '                    Response.Write("alert('Creazione Utenza " & txtTipoUtenza.Text & txtUtente.Text & " eseguita con successo.')")
            '                    Response.Write("</script>")

            '                    Return True
            '                Else
            '                    'se sei unsc U e non hai cek su R QUINDI VUOI CREARE UNA UTENZA DI TIPO U
            '                    'Insert in UtentiUNSC  in fase di inserimento
            '                    strSql = "INSERT INTO UtentiUNSC (UserName, Password, Abilitato, Nome, " & _
            '                                 "Cognome, HeliosRead, Mail, Password1, IdRegioneCompetenza,UserNameCreazione,DataCreazione,AccountAD,DurataPassword,CambioPassword,PasswordDaInviare) VALUES ('" & Replace(myUtenza, "'", "''") & "'," & _
            '                                 "'" & ClsUtility.CreatePSW(Session("NewPassUtente")) & "',1,'" & Replace(Trim(txtNome.Text), "'", "''") & "'," & _
            '                                 "'" & Replace(Trim(txtCognome.Text), "'", "''") & "'," & IIf(chkReadOnly.Checked = True, 1, 0) & ",'" & Replace(txtEmail.Text, "'", "''") & "','" & Session("NewPassUtente") & "'" & _
            '                                 "," & IIf(optTipUteU.Checked = True, "22", cboRegComp.SelectedValue) & ",'" & Session("utente") & "',Getdate(), '" & Replace(Trim(TxtDominioAD.Text), "'", "''") & "',NULL ,0 ,0 )"
            '                    allQuery.Add(strSql)
            '                    'non deve inviare l'email
            '                    ClsServer.EseguiQueryColl(allQuery, Session.SessionID, Session("conn"))

            '                    Session("CreazioneOK") = 0
            '                    lblmsgConf.Visible = True
            '                    lblmsgConf.Text = "Creazione Utenza " & txtTipoUtenza.Text & txtUtente.Text & " eseguita con successo."
            '                    'Response.Write("<script>")
            '                    'Response.Write("alert('Creazione Utenza " & txtTipoUtenza.Text & txtUtente.Text & " eseguita con successo.')")
            '                    'Response.Write("</script>")
            '                    Return True

            '                End If

            '            End If

            '        End If

            '    Else
            '        Response.Write("<script>")
            '        Response.Write("alert('Selezionare un'utenza simile a quella che si vuole creare.')")
            '        Response.Write("</script>")
            '    End If
            'End If


        Else
            'lblMessaggio.Text = "Il Nome Utente " & txtTipoUtenza.Text & txtUtente.Text & " è gia utilizzata."
            lblMessaggio.Visible = True
            lblMessaggio.Text = "L'Utenza " & txtTipoUtenza.Text & txtUtente.Text & " è gia utilizzata."
            'Response.Write("<script>")
            'Response.Write("alert('L\'Utenza " & txtTipoUtenza.Text & txtUtente.Text & " è gia utilizzata.')")
            'Response.Write("</script>")
            Return False
        End If

    End Function

    Private Sub pulisciseR()
        cmdChiudi.Visible = True
        cmdAnnulla.Visible = False

        cmdGenera.Visible = True
        imgSalva.Visible = False
        phAssociaArea.Visible = False
        If Session("TipoUtente") = "U" Then
            optTipUteR.Enabled = True
            optTipUteU.Enabled = True
        End If
    End Sub

    Private Sub pulisciseU()
        cmdChiudi.Visible = True
        cmdAnnulla.Visible = False

        cmdGenera.Visible = True
        imgSalva.Visible = False
        phAssociaArea.Visible = False
        optTipUteR.Enabled = True
        optTipUteU.Enabled = True
    End Sub

    'Funzione per controllare se esiste già lo username inndicato (TRUE = la user esiste già)
    Function checkUsername(ByVal strUser As String) As Boolean
        strSql = "Select * From UtentiUNSC Where UserName ='" & strUser & "'"
        Dim myTable As DataTable = ClsServer.CreaDataTable(strSql, False, Session("conn"))
        If myTable.Rows.Count > 0 Then
            Return True
        Else
            Return False
        End If
    End Function

    Private Sub cmdChiudi_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdChiudi.Click
        Session("CreazioneOK") = 0
        Response.Redirect("WfrmMain.aspx")
    End Sub

    Private Sub cmdAnnulla_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdAnnulla.Click
        Session("CreazioneOK") = 0
        Pulisci(False)
    End Sub

    'Funzione per la pulizia dei campi della maschera (blnTot = False non cancella i campi inseriti)
    Function Pulisci(Optional ByVal blnTot As Boolean = True)
        If blnTot = True Then
            txtCognome.Text = ""
            txtNome.Text = ""
            txtUtente.Text = ""
            'cboProfili.SelectedIndex = -1
            txtEmail.Text = ""
            'txtUtenzaSim.Text = ""
            txtTipoUtenza.Text = "U"
            'txtTipoUtenzaSim.Text = "U"
            optTipUteU.Checked = True
            cboRegComp.SelectedIndex = 0
        End If



        lblMessaggio.Visible = False
        cmdChiudi.Visible = True
        cmdAnnulla.Visible = False




        'modifica
        If Request.QueryString("IdUtente") <> "" Then
            cmdGenera.Visible = False
            imgSalva.Visible = True
        Else
            cmdGenera.Visible = True
            imgSalva.Visible = False
        End If
        If Session("TipoUtente") = "R" Then
            optTipUteR.Enabled = False
            optTipUteR.Checked = True
            optTipUteU.Enabled = False
            optTipUteU.Checked = False
            optTipUteE.Checked = False
            optTipUteE.Enabled = False
        End If
        If Session("TipoUtente") = "U" Then
            If optTipUteR.Checked = True Then
                optTipUteR.Checked = True
                optTipUteU.Checked = False
            End If
            If optTipUteU.Checked = True Then
                optTipUteU.Checked = True
                optTipUteR.Checked = False
            End If
        End If

        If optTipUteU.Checked = True Then
            cboRegComp.Enabled = False
            TxtDominioAD.Enabled = True
            TxtDurataPassword.Enabled = False
            lblDurataPassword.Enabled = False

        Else
            cboRegComp.Enabled = True
            TxtDominioAD.Enabled = False
            ChkCredenzialiEmail.Enabled = True
            ChkStampaCredenziali.Enabled = True
            TxtDurataPassword.Enabled = True
            lblDurataPassword.Enabled = True
            'imgStampa.Enabled = True
            'lblstampacredenziali.Enabled = True
        End If

    End Function

    'Private Sub cmdRicerca_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles cmdRicerca.Click
    '    'Response.Write("<script>")
    '    'aggiunto da jonathan
    '    'passo l'id della regione
    '    'di competenza dell'utente se di tipo R
    '    'If Session("TipoUtente") = "R" Then
    '    '    Response.Write("window.open('WfrmRicercaUtenze.aspx?IdRegioneCompetenza=" & cboRegComp.SelectedValue & "&Utenza=" & Trim(txtUtenzaSim.Text) & "&TipoUte=" & txtTipoUtenzaSim.Text & "','ricerca','height=400,width=500,dependent=yes,scrollbars=no,status=no,resizable=no')")
    '    'Else
    '    '    Response.Write("window.open('WfrmRicercaUtenze.aspx?Utenza=" & Trim(txtUtenzaSim.Text) & "&TipoUte=" & txtTipoUtenzaSim.Text & "','ricerca','height=400,width=500,dependent=yes,scrollbars=no,status=no,resizable=no')")
    '    'End If
    '    'Response.Write("</script>")
    'End Sub

    Private Sub imgSalva_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles imgSalva.Click
        lblMessaggio.Visible = False
        lblmsgConf.Visible = False
        If ValidazioneServer() = True Then
            Session("optTipUteU") = False
            If ControllaCampiNecessari() = False Then
                Response.Write("<script>")
                Response.Write("alert('Uno o più campi necessari non sono stati valorizzati.')")
                Response.Write("</script>")
                Exit Sub
            Else
                If ModificaProfilo() = True Then
                    'Pulisci(True)
                    Session("optTipUteU") = True
                    'Response.Redirect("disabilitautenze.aspx")
                    lblmsgConf.Visible = True
                    lblmsgConf.Text = "Salvataggio eseguito con successo."
                Else
                    Pulisci(False)
                End If
            End If
        End If
    End Sub

    Function ModificaProfilo() As Boolean
        lblmsgConf.Visible = False
        lblMessaggio.Visible = False
        If optTipUteU.Checked = True Then

            '-----------------------ANTONELLO-------------------------------------------------------

            If ControllaDominioEsistenza(1) = True Then
                If checkUsernameModifica(txtTipoUtenza.Text & txtUtente.Text) = False Then
                    Dim allQuery As New ArrayList
                    Dim myUtenza As String = txtTipoUtenza.Text & txtUtente.Text
                    'If optUtenzaProfilo.Checked = True Then
                    'Insert in AssociaUtenteGruppo
                    'strSql = "UPDATE AssociaUtenteGruppo SET IdProfilo=" & cboProfili.SelectedValue & ",IdProfiloRead=" & cboProfiliRead.SelectedValue & " WHERE USERNAME='" & txtTipoUtenza.Text & Replace(txtUtente.Text, "'", "''") & "'"

                    'allQuery.Add(strSql)
                    SalvaProfiliUtente(allQuery)
                    'update in UtentiUNSC
                    strSql = "UPDATE UtentiUNSC SET Nome='" & Trim(Replace(txtNome.Text, "'", "''")) & "', "
                    strSql = strSql & "Cognome='" & Trim(Replace(txtCognome.Text, "'", "''")) & "', "
                    strSql = strSql & "AccountAD='" & Trim(Replace(TxtDominioAD.Text, "'", "''")) & "', "
                    strSql = strSql & "HeliosRead=" & IIf(chkReadOnly.Checked = True, 1, 0) & ", "
                    strSql = strSql & "Mail='" & Trim(Replace(txtEmail.Text, "'", "''")) & "' "
                    strSql = strSql & "WHERE IdUtente='" & Request.QueryString("IdUtente") & "'"
                    allQuery.Add(strSql)

                    'Else
                    '    If txtTipoUtenzaSim.Text <> "" And txtUtenzaSim.Text <> "" Then
                    '        'Insert in AssociaUtenteGruppo
                    '        strSql = "UPDATE AssociaUtenteGruppo SET IdProfilo="
                    '        strSql = strSql & "(Select Profili.IdProfilo FROM Profili "
                    '        '============================================================================================================================
                    '        '====================================================30/09/2008==============================================================
                    '        '=======MODIFICA EFFETTUATA DA Kronk (99 scimmie saltavano sul letto, una cadde in terra e si ruppe il cervelletto...)=======
                    '        '=================AGGIUNTA JOIN SU PROFILO READ PER ABILITARE LEGGERE LE ABILITAZIONI ANCHE DELL'UTENTE READ=================
                    '        '============================================================================================================================
                    '        If UCase(Me.TemplateSourceDirectory) <> "/HELIOSREAD" Then
                    '            strSql = strSql & " INNER JOIN	AssociaUtenteGruppo ON Profili.IdProfilo = AssociaUtenteGruppo.IdProfilo "
                    '        Else
                    '            strSql = strSql & " INNER JOIN	AssociaUtenteGruppo ON Profili.IdProfilo = AssociaUtenteGruppo.IdProfiloRead "
                    '        End If
                    '        strSql = strSql & " WHERE AssociaUtenteGruppo.UserName = '" & txtTipoUtenzaSim.Text & txtUtenzaSim.Text & "'),IdProfiloRead=" & cboProfiliRead.SelectedValue & " "
                    '        strSql = strSql & "WHERE AssociaUtenteGruppo.USERNAME='" & txtTipoUtenza.Text & txtUtente.Text & "'"

                    '        allQuery.Add(strSql)

                    '        'update in UtentiUNSC
                    '        strSql = "UPDATE UtentiUNSC SET Nome='" & Trim(Replace(txtNome.Text, "'", "''")) & "', "
                    '        strSql = strSql & "Cognome='" & Trim(Replace(txtCognome.Text, "'", "''")) & "', "
                    '        strSql = strSql & "AccountAD='" & Trim(Replace(TxtDominioAD.Text, "'", "''")) & "', "
                    '        strSql = strSql & "HeliosRead=" & IIf(chkReadOnly.Checked = True, 1, 0) & ", "
                    '        strSql = strSql & "Mail='" & Trim(Replace(txtEmail.Text, "'", "''")) & "' "
                    '        strSql = strSql & "WHERE IdUtente='" & Request.QueryString("IdUtente") & "'"
                    '        allQuery.Add(strSql)

                    '    Else
                    '        Response.Write("<script>")
                    '        Response.Write("alert('Selezionare un'utenza simile a quella che si vuole creare.')")
                    '        Response.Write("</script>")
                    '    End If
                    'End If


                    If ClsServer.EseguiQueryColl(allQuery, Session.SessionID, Session("conn")) = True Then
                        'Recupero l'id dell'utenza appena creata
                        'Dim cmdIdUtenza As New SqlClient.SqlCommand
                        'Dim intNewIdUtente As Int16
                        'cmdIdUtenza.CommandType = CommandType.Text
                        'cmdIdUtenza.CommandText = "SELECT @@IDENTITY"
                        'cmdIdUtenza.Connection = Session("conn")
                        'intNewIdUtente = cmdIdUtenza.ExecuteScalar

                        ''Invio della mail di creazione dell'utenza
                        'Dim cmdMailUtenza As New SqlClient.SqlCommand
                        'cmdMailUtenza.CommandType = CommandType.StoredProcedure
                        'cmdMailUtenza.CommandText = "SP_PROCEDURAMAIL_2"
                        'cmdMailUtenza.Connection = Session("conn")

                        'Dim prmIdUtente As SqlClient.SqlParameter
                        'prmIdUtente = New SqlClient.SqlParameter
                        'prmIdUtente.ParameterName = "@IDUTENTE"
                        'prmIdUtente.SqlDbType = SqlDbType.Int
                        'prmIdUtente.Value = intNewIdUtente
                        'cmdMailUtenza.Parameters.Add(prmIdUtente)

                        'Dim prmTipo As SqlClient.SqlParameter
                        'prmTipo = New SqlClient.SqlParameter
                        'prmTipo.ParameterName = "@TIPO"
                        'prmTipo.SqlDbType = SqlDbType.Char
                        'prmTipo.Value = IIf(optTipUteU.Checked = True, "U", "R")
                        'cmdMailUtenza.Parameters.Add(prmTipo)

                        'cmdMailUtenza.ExecuteNonQuery()


                        lblmsgConf.Visible = True
                        lblmsgConf.Text = "Modifica Utenza " & txtTipoUtenza.Text & txtUtente.Text & " eseguita con successo."
                        '   Response.Write("<script>")
                        '   Response.Write("alert('Modifica Utenza " & txtTipoUtenza.Text & txtUtente.Text & " eseguita con successo.')")
                        '   Response.Write("</script>")
                    End If
                    Return True
                Else
                    lblMessaggio.Visible = True
                    lblMessaggio.Text = "L'Utenza " & txtTipoUtenza.Text & txtUtente.Text & " è gia utilizzata."
                    'response.Write("<script>")
                    'response.Write("alert('L\'Utenza " & txtTipoUtenza.Text & txtUtente.Text & " è gia utilizzata.')")
                    'response.Write("</script>")
                    Return False
                End If
            Else
                lblMessaggio.Visible = True
                lblMessaggio.Text = "Dominio utilizzato da altro Utente."
                'Response.Write("<script>")
                'Response.Write("alert('Dominio utilizzato da altro Utente.')")
                'Response.Write("</script>")
                Return False
            End If

            ''''Response.Write("<script>")
            ''''Response.Write("alert('NO POSSIBILE.')")
            ''''Response.Write("</script>")
            ''''Return False
        Else
            If checkUsernameModifica(txtTipoUtenza.Text & txtUtente.Text) = False Then
                Dim allQuery As New ArrayList
                Dim myUtenza As String = txtTipoUtenza.Text & txtUtente.Text
                'If optUtenzaProfilo.Checked = True Then
                'Insert in AssociaUtenteGruppo
                'strSql = "UPDATE AssociaUtenteGruppo SET IdProfilo=" & cboProfili.SelectedValue & ",IdProfiloRead=" & cboProfiliRead.SelectedValue & " WHERE USERNAME='" & txtTipoUtenza.Text & Replace(txtUtente.Text, "'", "''") & "'"

                'allQuery.Add(strSql)

                SalvaProfiliUtente(allQuery)

                'update in UtentiUNSC  ,DurataPassword,CambioPassword,PasswordDaInviare
                strSql = "UPDATE UtentiUNSC SET Nome='" & Trim(Replace(txtNome.Text, "'", "''")) & "', "
                strSql = strSql & "Cognome='" & Trim(Replace(txtCognome.Text, "'", "''")) & "', "
                'strSql = strSql & "AccountAD='" & Trim(Replace(TxtDominioAD.Text, "'", "''")) & "', "
                strSql = strSql & "HeliosRead=" & IIf(chkReadOnly.Checked = True, 1, 0) & ", "
                strSql = strSql & "Mail='" & Trim(Replace(txtEmail.Text, "'", "''")) & "', "
                strSql = strSql & "DurataPassword='" & TxtDurataPassword.Text & "', "
                strSql = strSql & "CambioPassword=" & 0 & ", "
                strSql = strSql & "PasswordDaInviare=" & IIf(ChkCredenzialiEmail.Checked = True, 1, 0) & " "
                strSql = strSql & "WHERE IdUtente='" & Request.QueryString("IdUtente") & "'"
                allQuery.Add(strSql)

                'Else
                'If txtTipoUtenzaSim.Text <> "" And txtUtenzaSim.Text <> "" Then
                '    'Insert in AssociaUtenteGruppo
                '    strSql = "UPDATE AssociaUtenteGruppo SET IdProfilo="
                '    strSql = strSql & "(Select Profili.IdProfilo FROM Profili "
                '    '============================================================================================================================
                '    '====================================================30/09/2008==============================================================
                '    '=======MODIFICA EFFETTUATA DA Kronk (99 scimmie saltavano sul letto, una cadde in terra e si ruppe il cervelletto...)=======
                '    '=================AGGIUNTA JOIN SU PROFILO READ PER ABILITARE LEGGERE LE ABILITAZIONI ANCHE DELL'UTENTE READ=================
                '    '============================================================================================================================
                '    If UCase(Me.TemplateSourceDirectory) <> "/HELIOSREAD" Then
                '        strSql = strSql & " INNER JOIN	AssociaUtenteGruppo ON Profili.IdProfilo = AssociaUtenteGruppo.IdProfilo "
                '    Else
                '        strSql = strSql & " INNER JOIN	AssociaUtenteGruppo ON Profili.IdProfilo = AssociaUtenteGruppo.IdProfiloRead "
                '    End If
                '    strSql = strSql & " WHERE AssociaUtenteGruppo.UserName = '" & txtTipoUtenzaSim.Text & txtUtenzaSim.Text & "'),IdProfiloRead=" & cboProfiliRead.SelectedValue & " "
                '    strSql = strSql & "WHERE AssociaUtenteGruppo.USERNAME='" & txtTipoUtenza.Text & txtUtente.Text & "'"

                '    allQuery.Add(strSql)

                '    'update in UtentiUNSC
                '    strSql = "UPDATE UtentiUNSC SET Nome='" & Trim(Replace(txtNome.Text, "'", "''")) & "', "
                '    strSql = strSql & "Cognome='" & Trim(Replace(txtCognome.Text, "'", "''")) & "', "
                '    'strSql = strSql & "AccountAD='" & Trim(Replace(TxtDominioAD.Text, "'", "''")) & "', "
                '    strSql = strSql & "HeliosRead=" & IIf(chkReadOnly.Checked = True, 1, 0) & ", "
                '    strSql = strSql & "Mail='" & Trim(Replace(txtEmail.Text, "'", "''")) & "', "
                '    strSql = strSql & "DurataPassword='" & TxtDurataPassword.Text & "', "
                '    strSql = strSql & "CambioPassword=" & 0 & ", "
                '    strSql = strSql & "PasswordDaInviare=" & IIf(ChkCredenzialiEmail.Checked = True, 1, 0) & " "
                '    strSql = strSql & "WHERE IdUtente='" & Request.QueryString("IdUtente") & "'"
                '    allQuery.Add(strSql)

                'Else
                '    Response.Write("<script>")
                '    Response.Write("alert('Selezionare un'utenza simile a quella che si vuole creare.')")
                '    Response.Write("</script>")
                'End If
                'End If


                If ClsServer.EseguiQueryColl(allQuery, Session.SessionID, Session("conn")) = True Then
                    'Recupero l'id dell'utenza appena creata
                    'Dim cmdIdUtenza As New SqlClient.SqlCommand
                    'Dim intNewIdUtente As Int16
                    'cmdIdUtenza.CommandType = CommandType.Text
                    'cmdIdUtenza.CommandText = "SELECT @@IDENTITY"
                    'cmdIdUtenza.Connection = Session("conn")
                    'intNewIdUtente = cmdIdUtenza.ExecuteScalar

                    ''Invio della mail di creazione dell'utenza
                    'Dim cmdMailUtenza As New SqlClient.SqlCommand
                    'cmdMailUtenza.CommandType = CommandType.StoredProcedure
                    'cmdMailUtenza.CommandText = "SP_PROCEDURAMAIL_2"
                    'cmdMailUtenza.Connection = Session("conn")

                    'Dim prmIdUtente As SqlClient.SqlParameter
                    'prmIdUtente = New SqlClient.SqlParameter
                    'prmIdUtente.ParameterName = "@IDUTENTE"
                    'prmIdUtente.SqlDbType = SqlDbType.Int
                    'prmIdUtente.Value = intNewIdUtente
                    'cmdMailUtenza.Parameters.Add(prmIdUtente)

                    'Dim prmTipo As SqlClient.SqlParameter
                    'prmTipo = New SqlClient.SqlParameter
                    'prmTipo.ParameterName = "@TIPO"
                    'prmTipo.SqlDbType = SqlDbType.Char
                    'prmTipo.Value = IIf(optTipUteU.Checked = True, "U", "R")
                    'cmdMailUtenza.Parameters.Add(prmTipo)

                    'cmdMailUtenza.ExecuteNonQuery()
                    lblmsgConf.Visible = True
                    lblmsgConf.Text = "Modifica Utenza " & txtTipoUtenza.Text & txtUtente.Text & " eseguita con successo."
                    'Response.Write("<script>")
                    'Response.Write("alert('Modifica Utenza " & txtTipoUtenza.Text & txtUtente.Text & " eseguita con successo.')")
                    'Response.Write("</script>")
                End If
                Return True
            Else
                lblMessaggio.Visible = True
                lblMessaggio.Text = "L'Utenza " & txtTipoUtenza.Text & txtUtente.Text & " è gia utilizzata."
                'Response.Write("<script>")
                'Response.Write("alert('L\'Utenza " & txtTipoUtenza.Text & txtUtente.Text & " è gia utilizzata.')")
                'Response.Write("</script>")
                Return False
        End If


        End If
    End Function

    Function checkUsernameModifica(ByVal strUser As String) As Boolean
        strSql = "Select * From UtentiUNSC Where UserName ='" & strUser & "' AND IdUtente<>'" & Request.QueryString("IdUtente") & "'"
        Dim myTable As DataTable = ClsServer.CreaDataTable(strSql, False, Session("conn"))
        If myTable.Rows.Count > 0 Then
            Return True
        Else
            Return False
        End If
    End Function

    Private Sub ImgAssociaArea_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImgAssociaArea.Click
        Response.Redirect("WfrmAssociazUtenteArea.aspx?IdUtente=" & Request.QueryString("IdUtente"))
    End Sub

    Private Sub ImgResetPassword_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImgResetPassword.Click
        'Response.Redirect("WfrmResetPasswordUtenti.aspx?IdUtente=" & Request.QueryString("IdUtente"))
        Response.Write("<SCRIPT>" & vbCrLf)
        Response.Write("window.open('WfrmResetPasswordUtenti.aspx?IdUtente=" & Request.QueryString("IdUtente") & "','ResetPassword','height=400,width=600,dependent=no,scrollbars=no,status=no,resizable=no');" & vbCrLf)
        Response.Write("</SCRIPT>")
    End Sub

    'Private Sub optUtenzaProfilo_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles optUtenzaProfilo.CheckedChanged
    '    'cboProfili.Enabled = True
    '    'txtTipoUtenzaSim.Enabled = False
    '    'txtUtenzaSim.Enabled = False
    '    ' cmdRicerca.Enabled = False
    'End Sub

    'Private Sub optUtenzaSimile_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles optUtenzaSimile.CheckedChanged
    '    ' cboProfili.Enabled = False
    '    'txtTipoUtenzaSim.Enabled = True
    '    ' txtUtenzaSim.Enabled = True
    '    'cmdRicerca.Enabled = True
    'End Sub

    Private Sub optTipUteU_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles optTipUteU.CheckedChanged
        CaricaGrigliaProfili()
        txtTipoUtenza.Text = "U"
        'txtTipoUtenzaSim.Text = "U"
        cboRegComp.Enabled = False
        Label1.Enabled = True
        TxtDominioAD.Enabled = True
        dvCredenzialiEmail.Disabled = True
        ChkCredenzialiEmail.Enabled = False
        ChkCredenzialiEmail.Checked = False
        dvStampaCredenziali.Disabled = True
        ChkStampaCredenziali.Enabled = False
        ChkStampaCredenziali.Checked = False
        lblDurataPassword.Enabled = False
        TxtDurataPassword.Enabled = False
    End Sub

    Private Sub optTipUteR_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles optTipUteR.CheckedChanged
        CaricaGrigliaProfili()
        txtTipoUtenza.Text = "R"
        ' txtTipoUtenzaSim.Text = "R"
        cboRegComp.Enabled = True
        Label1.Enabled = False

        TxtDominioAD.Text = ""
        TxtDominioAD.Enabled = False
        lblDurataPassword.Enabled = True
        TxtDurataPassword.Enabled = True
        dvCredenzialiEmail.Disabled = False
        ChkCredenzialiEmail.Enabled = True
        ChkCredenzialiEmail.Checked = True
        dvStampaCredenziali.Disabled = False
        ChkStampaCredenziali.Enabled = True
        ChkStampaCredenziali.Checked = True

        If Request.QueryString("Op") = "71" Then
            dvCredenzialiEmail.Disabled = True
            ChkCredenzialiEmail.Enabled = False
            ChkCredenzialiEmail.Checked = False
            dvStampaCredenziali.Disabled = True
            ChkStampaCredenziali.Enabled = False
            ChkStampaCredenziali.Checked = False
        End If
    End Sub

    Private Function ValidazioneServer() As Boolean

        'Dim regex As Regex = New Regex("^[_a-z0-9-]+(.[a-z0-9-]+)@[a-z0-9-]+(.[a-z0-9-]+)*(.[a-z]{2,4})$")
        Dim regex As Regex = New Regex("^(?("")("".+?""@)|(([0-9a-zA-Z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-zA-Z])@))(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,6}))$")
        Dim match As Match = regex.Match(txtEmail.Text.Trim)

        If txtEmail.Text.Trim <> String.Empty AndAlso txtEmail.Enabled = True Then
            If match.Success = False Then
                lblMessaggio.Visible = True
                lblMessaggio.Text = "Il formato Email non è valido!"
                txtEmail.Focus()
                Return False

            End If
        End If

        Dim durataPassword As Integer
        Dim durataPasswordInteger As Boolean

        If TxtDurataPassword.Visible = True Then
            durataPasswordInteger = Integer.TryParse(TxtDurataPassword.Text.Trim, durataPassword)
            If TxtDurataPassword.Text.Trim <> String.Empty AndAlso TxtDurataPassword.Enabled = True Then
                If durataPasswordInteger = False Then
                    lblMessaggio.Visible = True
                    lblMessaggio.Text = "La Durata Password può contenere solo numeri."
                    TxtDurataPassword.Focus()
                    Return False

                End If
            End If
        End If

        If VerificaCheck() = False Then

            lblMessaggio.Visible = True
            lblMessaggio.Text = "E' necessario indicare almeno un Profilo Utente."
            Exit Function
        End If
        Return True

    End Function
    Private Sub SalvaProfiliUtente(ByRef strsql As ArrayList)
        Dim stringa As String
        Dim item As DataGridItem
        stringa = "Delete from AssociaUtenteGruppo  WHERE USERNAME='" & txtTipoUtenza.Text & Replace(txtUtente.Text, "'", "''") & "'"
        strsql.Add(stringa)
        For Each item In dtgProfiliUtente.Items
            Dim check As CheckBox = DirectCast(item.FindControl("chkSeleziona"), CheckBox)
            If check.Checked = True Then
                stringa = "INSERT INTO AssociaUtenteGruppo (IdProfilo,IdProfiloRead,USERNAME ) " & _
                           "VALUES(" & dtgProfiliUtente.Items(item.ItemIndex).Cells(0).Text & "," & cboProfiliRead.SelectedValue & ",'" & txtTipoUtenza.Text & Replace(txtUtente.Text, "'", "''") & "')"
                strsql.Add(stringa)
            End If
        Next
    End Sub
    Sub CaricaProfiliAssociati(Username As String)
        Dim dtrProfili As SqlClient.SqlDataReader
        strSql = " SELECT idProfilo "
        strSql = strSql & "from  AssociaUtenteGruppo "
        strSql = strSql & "WHERE Username='" & Username & "'"

        dtrProfili = ClsServer.CreaDatareader(strSql, Session("conn"))

        While dtrProfili.Read()
            For Each item In dtgProfiliUtente.Items
                Dim check As CheckBox = DirectCast(item.FindControl("chkSeleziona"), CheckBox)
                If dtrProfili("idProfilo") = item.Cells(0).Text() Then
                    check.Checked = True
                End If
            Next
        End While
        If Not dtrProfili Is Nothing Then
            dtrProfili.Close()
            dtrProfili = Nothing
        End If

    End Sub

    Private Function VerificaCheck() As Boolean
        'controllo è  stata checcato almeno un settore per il salvataggio
        VerificaCheck = False
        Dim item As DataGridItem
        For Each item In dtgProfiliUtente.Items
            Dim check As CheckBox = DirectCast(item.FindControl("chkSeleziona"), CheckBox)
            If check.Checked = True Then
                VerificaCheck = True
            End If
        Next
        Return VerificaCheck
    End Function
End Class