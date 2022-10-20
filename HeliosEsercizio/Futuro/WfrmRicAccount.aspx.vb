Public Class WfrmRicAccount
    Inherits System.Web.UI.Page

    Dim rstGenerico As SqlClient.SqlCommand 'Variabile di tipo SqlClient.SqlCommand
    Dim Strsql As String 'Variabile di tipo Stringa
    Dim DTRGenerico As SqlClient.SqlDataReader 'variabile di tipo SqlClient.SqlDataReader

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Inserire qui il codice utente necessario per inizializzare la pagina
        'Generato da Alessandra Taballione il 02/02/2004
        If Not Session("LogIn") Is Nothing Then
            If Session("LogIn") = False Then 'verifico validità log-in
                Response.Redirect("LogOn.aspx")
            End If
        Else
            Response.Redirect("LogOn.aspx")
        End If
        If IsPostBack = False Then
            lblMessaggi.Text = ""
            lblMessaggi.Visible = False
            'carico i prefissi ente
            'sqlLocalConn.ConnectionString = "user id=sa;password=vbra250;data source=www1;persist security info=False;connect timeout=300;Max Pool Size=3000;initial catalog=unscproduzione"
            'sqlLocalConn.Open()
            Strsql = " select 0 as ordina ,'0' as idprefisso,'Seleziona' as descrizione  from prefissiente " & _
            " union " & _
            " select 1 as ordina, idprefisso,descrizione from prefissiente " & _
            " order by ordina,descrizione "
            DTRGenerico = ClsServer.CreaDatareader(Strsql, Session("conn"))
            ddlsuffisso.DataSource = DTRGenerico
            ddlsuffisso.DataValueField = "descrizione"
            ddlsuffisso.DataTextField = "descrizione"
            ddlsuffisso.DataBind()
            If Not DTRGenerico Is Nothing Then
                DTRGenerico.Close()
                DTRGenerico = Nothing
            End If
            TxtDataRicezioneCarta.Text = Format(Now, "dd/MM/yyyy")
            ''''Strsql = "select * from utentiunsc where username= '" & Session("Utente") & "'"
            ''''DTRGenerico = ClsServer.CreaDatareader(Strsql, Session("conn"))
            ''''If DTRGenerico.HasRows = False Then
            ''''    If Not DTRGenerico Is Nothing Then
            ''''        DTRGenerico.Close()
            ''''        DTRGenerico = Nothing
            ''''    End If
            ''''Else
            ''''    DTRGenerico.Read()
            ''''    txtIDregcomp.Text = DTRGenerico("idRegioneCompetenza")
            ''''    DTRGenerico.Close()
            ''''    DTRGenerico = Nothing
            ''''End If
        End If
    End Sub


    Private Sub cmdSalva_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdSalva.Click

        If ValidazioneServerSalva() = True Then
            SalvaDati()
        End If

    End Sub

    Private Sub Inserimento(ByVal sqlLocalConn As SqlClient.SqlConnection, ByRef intIdEnte As Integer)
        'Generato da Alessandra Taballione il 02/02/2004
        'Effettuo l'inserimento dell'ente che richiede Account

        Dim myCommand As SqlClient.SqlCommand
        'Dim intIdEnte As Integer

        'Effettuo l'Inserimento in Enti  txtIDregcomp.Text
        'Modificato il 16/02/2005 da Alessandra Taballione con implementazione del Cofice fiscale
        Strsql = "Insert into enti (albo,denominazione,email,EmailCertificata,noterichiestaregistrazione,datacreazionerecord,Telefonorichiestaregistrazione,prefissoTelefonorichiestaregistrazione,codicefiscale,IdRegioneAppartenenza,IdRegioneCompetenza,DataRicezioneCartacea)values " & _
        " ('SCU','" & UCase(Trim(Replace(txtdenominazione.Text, "'", "''"))) & "','" & Trim(Replace(txtEmail.Text, "'", "''")) & "','" & Trim(Replace(txtEmailCertificata.Text, "'", "''")) & "', " & _
        "'" & UCase(Trim(Replace(txtRichiedente.Text, "'", "''"))) & "',getdate(),'" & Trim(Replace(txtTelefono.Text, "'", "''")) & "','" & Trim(Replace(txtprefisso.Text, "'", "''")) & "','" & Trim(Replace(txtCodFis.Text, "'", "''")) & "',null,null,'" & Trim(Replace(TxtDataRicezioneCarta.Text, "'", "''")) & "')"
        If Not DTRGenerico Is Nothing Then
            DTRGenerico.Close()
            DTRGenerico = Nothing
        End If
        myCommand = New SqlClient.SqlCommand(Strsql, Session("conn"))
        myCommand.ExecuteNonQuery()
        myCommand.Dispose()
        'Ricerca dell IDEnte Inserito
        Strsql = "Select idente from enti where idente=@@identity"
        If Not DTRGenerico Is Nothing Then
            DTRGenerico.Close()
            DTRGenerico = Nothing
        End If
        DTRGenerico = ClsServer.CreaDatareader(Strsql, Session("conn"))
        DTRGenerico.Read()
        If DTRGenerico.HasRows = True Then
            intIdEnte = DTRGenerico("idente")
            'Effettuo inserimento Cronologia stato
            'con lo stato fisso a 4=Richiesta Registrazione
            Strsql = "Insert into CronologiaEntiStati (idente," & _
            "idstatoEnte,datacronologia,idtipocronologia)values " & _
            " ('" & Trim(Replace(DTRGenerico("idente"), "'", "''")) & "'," & _
            " 4,getdate(),0)"
            If Not DTRGenerico Is Nothing Then
                DTRGenerico.Close()
                DTRGenerico = Nothing
            End If
            myCommand = New SqlClient.SqlCommand(Strsql, Session("conn"))
            myCommand.ExecuteNonQuery()
            myCommand.Dispose()



            'inserimento in cronologiaMailEnti
            Strsql = "Insert into CronologiaMailEnti(idente,NuovaEmail, NuovaPEC, Username, DataModifica) " & _
                     " VALUES (" & intIdEnte & ",'" & Trim(Replace(txtEmail.Text, "'", "''")) & "','" & Trim(Replace(txtEmailCertificata.Text, "'", "''")) & "','" & Session("Utente") & "', getdate())"

            myCommand = New SqlClient.SqlCommand(Strsql, Session("conn"))
            myCommand.ExecuteNonQuery()
            myCommand.Dispose()
        End If
        If Not DTRGenerico Is Nothing Then
            DTRGenerico.Close()
            DTRGenerico = Nothing
        End If
        'sqlLocalConn.Close()
        'sqlLocalConn = Nothing
    End Sub

    Private Sub PulisciMaschera()
        'Generato da Alessandra Taballione il 3/02/2004
        'Preparazione della maschera per un nuovo inserimento
        txtdenominazione.Text = ""
        txtEmail.Text = ""
        txtRichiedente.Text = ""
        txtTelefono.Text = ""
        txtprefisso.Text = ""
        txtCodFis.Text = ""
    End Sub

    Private Sub cmdChiudi_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdChiudi.Click
        'Generato da Alessandra Taballione il 02/02/2004
        'Rimando l'Utente nella Pagina di logOn
        Response.Redirect("WfrmMain.aspx")
    End Sub

    Private Sub ddlsuffisso_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlsuffisso.SelectedIndexChanged

        txtdenominazione.Text = ddlsuffisso.SelectedItem.Text + Space(1) + txtdenominazione.Text
        ddlsuffisso.SelectedIndex = 0

    End Sub
    Private Sub SalvaDati()
        'Creato da Alessandra Taballione il 02/02/2004
        'Controlli formali per l'inserimento della richiesta di account
        'ed inserimento della Richiesta con immissione dati anagrafica Enti

        Dim messaggioRifiuta As String
        Dim BLOCCO_ACCREDITAMENTO As String
        messaggioRifiuta = ""


        Strsql = "SELECT VALORE  FROM Configurazioni where Parametro='BLOCCO_ACCREDITAMENTO'"
        If Not DTRGenerico Is Nothing Then
            DTRGenerico.Close()
            DTRGenerico = Nothing
        End If
        DTRGenerico = ClsServer.CreaDatareader(Strsql, Session("conn"))
        DTRGenerico.Read()

        BLOCCO_ACCREDITAMENTO = DTRGenerico("Valore")
        If Not DTRGenerico Is Nothing Then
            DTRGenerico.Close()
            DTRGenerico = Nothing
        End If

        If BLOCCO_ACCREDITAMENTO = "NO" Then
            Dim ESITO As String
            Dim strMsgEmail As String = ""
            Dim strMsg As String = ""
            ESITO = ClsUtility.STORE_VERIFICA_USABILITA_ENTE_ALBO(Replace(Trim(txtCodFis.Text), "'", "''"), Replace(Trim(txtdenominazione.Text), "'", "''"), "SCU", 0, strMsgEmail, strMsg, Session("conn"))
            If ESITO = "NO" Then
                messaggioRifiuta = strMsgEmail
            End If
            ''Aggiunto da Alessandra Taballione il 16.02.2005
            ''Controllo sull'univocità del Codice Fiscale
            'If Trim(txtCodFis.Text) <> "" Then

            '    If Not DTRGenerico Is Nothing Then
            '        DTRGenerico.Close()
            '        DTRGenerico = Nothing
            '    End If
            '    'Verifico se il codice fiscale è gia presente nell'anagrafica Enti
            '    'Strsql = "Select idente,IdStatoEnte,IdClasseAccreditamento,CodiceRegione "
            '    'Strsql &= " from enti where codicefiscale='" & Replace(Trim(txtCodFis.Text), "'", "''") & "'"
            '    Strsql = "Select "
            '    If Not DTRGenerico Is Nothing Then
            '        DTRGenerico.Close()
            '        DTRGenerico = Nothing
            '    End If
            '    DTRGenerico = ClsServer.CreaDatareader(Strsql, Session("conn"))
            '    'se gia esistente viene bloccato l'inserimento
            '    DTRGenerico.Read()
            '    If DTRGenerico.HasRows = True Then
            '        Dim Idente As Integer
            '        Dim IdStatoEnte As Integer
            '        Dim IdClasseAccr As Integer
            '        Dim CodiceRegione As String
            '        IdStatoEnte = IIf(Not IsDBNull(DTRGenerico("IdStatoEnte")), DTRGenerico("IdStatoEnte"), 0)
            '        IdClasseAccr = IIf(Not IsDBNull(DTRGenerico("IdClasseAccreditamento")), DTRGenerico("IdClasseAccreditamento"), 0)
            '        CodiceRegione = IIf(Not IsDBNull(DTRGenerico("CodiceRegione")), DTRGenerico("CodiceRegione"), "")
            '        Idente = DTRGenerico("IdEnte")
            '        'Image2.Visible = True
            '        'Image2.ImageUrl = "images/alert3.gif"
            '        'lblMessaggi.Visible = True
            '        'lblMessaggi.ForeColor = Color.Red
            '        'lblMessaggi.Text = "Il codice Fiscale inserito è già presente in archivio.Pertanto la richiesta non verrà inoltrata."
            '        'attivare la maschera di invio e-mail (Rifiuto accaunt)
            '        'Codice Fiscale Ente già esistente
            '        If Not DTRGenerico Is Nothing Then
            '            DTRGenerico.Close()
            '            DTRGenerico = Nothing
            '        End If

            '        If IdStatoEnte = 3 Or IdStatoEnte = 8 Or IdStatoEnte = 9 Then
            '            If IdClasseAccr <= 4 Then
            '                messaggioRifiuta = messaggioRifiuta & " l'Ente risulta già accreditato con il codice " & CodiceRegione & ". Cordiali saluti."
            '            Else
            '                Dim codregpadre As String
            '                'FARE LA QUERY Per ente figlio
            '                Strsql = "SELECT enti.CodiceRegione, enti.IDEnte, entirelazioni.IDEnteFiglio FROM enti INNER JOIN entirelazioni ON enti.IDEnte = entirelazioni.IDEntePadre where identefiglio ='" & Idente & "'"
            '                DTRGenerico = ClsServer.CreaDatareader(Strsql, Session("conn"))
            '                DTRGenerico.Read()
            '                If DTRGenerico.HasRows = True Then
            '                    codregpadre = DTRGenerico("CodiceRegione")
            '                    messaggioRifiuta &= " il suo ente risulta già accreditato ed in accordo con l'ente " & codregpadre
            '                    messaggioRifiuta &= ". Se desidera accreditarsi autonomamente l'ente capofila (" & codregpadre & ") " & _
            '                                        "deve provvedere ad annullare l'accordo seguendo le procedure previste dal Sistema Unico e," & _
            '                                        " solo successivamente, lei potrà inoltrare una nuova richiesta di accesso al Sistema Unico. Cordiali saluti."
            '                End If
            '            End If
            '        End If

            '        If (IdStatoEnte = 6) And messaggioRifiuta = "" Then
            '            If IdClasseAccr <= 4 Then
            '                messaggioRifiuta = messaggioRifiuta & " l'Ente risulta già esistente con il codice " & CodiceRegione & ". Cordiali saluti."
            '            Else
            '                Dim codregpadre As String
            '                'FARE LA QUERY Per ente figlio
            '                Strsql = "SELECT enti.CodiceRegione, enti.Denominazione,  enti.IDEnte, entirelazioni.IDEnteFiglio FROM enti INNER JOIN entirelazioni ON enti.IDEnte = entirelazioni.IDEntePadre where identefiglio ='" & Idente & "'"
            '                DTRGenerico = ClsServer.CreaDatareader(Strsql, Session("conn"))
            '                DTRGenerico.Read()
            '                If DTRGenerico.HasRows = True Then
            '                    'mod. il 15/04/2008 da s.c.
            '                    'se il codiceregione è NULL inserisco la denominazione dell'ente
            '                    'è null quando si sta Accredidando un nuovo Ente  e questo è un Ente FIglio di un Nuovo Ente che si deve ancora accreditare
            '                    If IsDBNull(DTRGenerico("CodiceRegione")) = False Then
            '                        codregpadre = DTRGenerico("CodiceRegione")
            '                    Else
            '                        codregpadre = DTRGenerico("Denominazione")
            '                    End If

            '                    messaggioRifiuta &= " il suo ente risulta già esistente ed in accordo con l'ente " & codregpadre
            '                    messaggioRifiuta &= ". Se desidera accreditarsi autonomamente l'ente capofila (" & codregpadre & ") " & _
            '                                        "deve provvedere ad annullare l'accordo seguendo le procedure previste dal Sistema Unico e," & _
            '                                       " solo successivamente, lei potrà inoltrare una nuova richiesta di accesso al Sistema Unico. Cordiali saluti."

            '                Else
            '                    messaggioRifiuta = messaggioRifiuta & " L'utenza risulta essere già stata assegnata per il codice fiscale indicato. Si prega di effettuare la richiesta di INOLTRO PASSWORD all'email heliosweb@serviziocivile.it nel caso essa sia andata smarrita. Cordiali saluti."

            '                End If
            '            End If
            '        End If

            '        'messaggioRifiuta = messaggioRifiuta & " Codice Fiscale Ente già esistente"

            '        If Not DTRGenerico Is Nothing Then
            '            DTRGenerico.Close()
            '            DTRGenerico = Nothing
            '        End If
            '        'sqlLocalConn.Close()
            '        'sqlLocalConn = Nothing
            '        'Exit Sub
            '    End If
            '    If Not DTRGenerico Is Nothing Then
            '        DTRGenerico.Close()
            '        DTRGenerico = Nothing
            '    End If

            'End If

            'If Trim(txtdenominazione.Text) <> "" And messaggioRifiuta = "" Then
            '    'Verifico se la denominazione è gia presente nell'anagrafica Enti
            '    Strsql = "Select idente from enti " & _
            '    " inner join statienti on statienti.idstatoente=enti.idstatoente " & _
            '    " where denominazione='" & Replace(Trim(txtdenominazione.Text), "'", "''") & "'" & _
            '     " and (statienti.sospeso<>1 and statienti.chiuso<>1) AND statienti.IDStatoEnte<>11 "
            '    If Not DTRGenerico Is Nothing Then
            '        DTRGenerico.Close()
            '        DTRGenerico = Nothing
            '    End If
            '    DTRGenerico = ClsServer.CreaDatareader(Strsql, Session("conn"))
            '    'se gia esistente viene bloccato l'inserimento
            '    If DTRGenerico.HasRows = True Then
            '        'Image2.Visible = True
            '        'Image2.ImageUrl = "images/alert3.gif"
            '        'lblMessaggi.Visible = True
            '        'lblMessaggi.ForeColor = Color.Red
            '        'lblMessaggi.Text = "La denominazione inserita è già presente in archivio. Pertanto la richiesta non verrà inoltrata."
            '        'attivare la maschera di invio e-email (Rifiuto accaunt)
            '        'Denominazione Ente già esistente
            '        messaggioRifiuta = " La denominazione dell'Ente risulta già presente in archivio. E' necessario inviare una nuova richiesta di accesso al sistema specificando nella denominazione dell'Ente anche l'area geografica di appartenenza (Comune o Provincia o Regione). Cordiali saluti."


            '        If Not DTRGenerico Is Nothing Then
            '            DTRGenerico.Close()
            '            DTRGenerico = Nothing
            '        End If
            '        'sqlLocalConn.Close()
            '        'sqlLocalConn = Nothing
            '        'Exit Sub
            '    End If
            '    If Not DTRGenerico Is Nothing Then
            '        DTRGenerico.Close()
            '        DTRGenerico = Nothing
            '    End If
            'End If
        Else 'BLOCCO_ACCREDITAMENTO a SI 

            messaggioRifiuta &= "Dal giorno 15 novembre 2020 e fino al 31 maggio 2021 è sospesa temporaneamente la presentazione delle istanze di iscrizione e di parte delle istanze di adeguamento all’Albo degli enti di servizio civile universale (Circolare 22 ottobre 2020)."

        End If

        '******c'era prima il controllo sul codice fiscale trasferito su *****************
        Dim intIdEnte As Integer
        Inserimento(Session("conn"), intIdEnte)

        'Image2.Visible = True
        'lblMessaggi.Visible = True
        'lblMessaggi.ForeColor = Color.Navy
        'Image2.ImageUrl = "images/conf1.jpg"
        'lblMessaggi.Text = "La richiesta di Registrazione è stata inoltrata." & vbCrLf & "La Username e Password verranno inviate all'indirizzo di posta inserito. "

        If Not DTRGenerico Is Nothing Then
            DTRGenerico.Close()
            DTRGenerico = Nothing
        End If

        If messaggioRifiuta = "" Then
            Strsql = "Select enti.* from enti where idente = " & intIdEnte

            DTRGenerico = ClsServer.CreaDatareader(Strsql, Session("conn"))
            DTRGenerico.Read()
            If DTRGenerico.HasRows = True Then
                Context.Items.Add("Ente", DTRGenerico("Denominazione"))
                Context.Items.Add("Email", DTRGenerico("EMail"))
                Context.Items.Add("Azione", "Accetta")
                Context.Items.Add("strsql", "")
                Context.Items.Add("MessaggioErrore", "")
                Session("IdEnte") = DTRGenerico("IdEnte")
                Session("Denominazione") = DTRGenerico("Denominazione")
                DTRGenerico.Close()
                DTRGenerico = Nothing
                Server.Transfer("WfrmaccettaRic.aspx?")
            Else
                'Response.Write("PIPPO")
                DTRGenerico.Close()
                DTRGenerico = Nothing
            End If
        Else
            Strsql = "Select enti.* from enti where idente = " & intIdEnte
            DTRGenerico = ClsServer.CreaDatareader(Strsql, Session("conn"))
            DTRGenerico.Read()
            If DTRGenerico.HasRows = True Then
                Context.Items.Add("Ente", DTRGenerico("Denominazione"))
                Context.Items.Add("Email", DTRGenerico("EMail"))
                Context.Items.Add("Azione", "Rifiuta")
                Context.Items.Add("strsql", "")
                Context.Items.Add("MessaggioErrore", messaggioRifiuta)
                Session("IdEnte") = DTRGenerico("IdEnte")
                Session("Denominazione") = DTRGenerico("Denominazione")
                DTRGenerico.Close()
                DTRGenerico = Nothing
                Server.Transfer("WfrmaccettaRic.aspx")
            Else
                'Response.Write("PIPPO")
                DTRGenerico.Close()
                DTRGenerico = Nothing
            End If


        End If
        'sqlLocalConn.Close()
        'sqlLocalConn = Nothing
        PulisciMaschera()
    End Sub
    Private Sub SalvaDati_OLD()
        'Creato da Alessandra Taballione il 02/02/2004
        'Controlli formali per l'inserimento della richiesta di account
        'ed inserimento della Richiesta con immissione dati anagrafica Enti
        'sqlLocalConn.ConnectionString = "user id=sa;password=vbra250;data source=www1;persist security info=False;connect timeout=300;Max Pool Size=3000;initial catalog=unscproduzione"
        'sqlLocalConn.Open()
        Dim messaggioRifiuta As String
        Dim BLOCCO_ACCREDITAMENTO As String
        messaggioRifiuta = ""


        'myQuerySql = "select valore from configurazioni where parametro = 'DURATA_ADEG'"
        'myDataReader = ClsServer.CreaDatareader(myQuerySql, Session("conn"))
        'myDataReader.Read()
        'Valore = myDataReader("valore")
        'ChiudiDataReader(myDataReader)
        Strsql = "SELECT VALORE  FROM Configurazioni where Parametro='BLOCCO_ACCREDITAMENTO'"
        If Not DTRGenerico Is Nothing Then
            DTRGenerico.Close()
            DTRGenerico = Nothing
        End If
        DTRGenerico = ClsServer.CreaDatareader(Strsql, Session("conn"))
        DTRGenerico.Read()

        BLOCCO_ACCREDITAMENTO = DTRGenerico("Valore")
        If Not DTRGenerico Is Nothing Then
            DTRGenerico.Close()
            DTRGenerico = Nothing
        End If

        If BLOCCO_ACCREDITAMENTO = "NO" Then

            'Aggiunto da Alessandra Taballione il 16.02.2005
            'Controllo sull'univocità del Codice Fiscale
            If Trim(txtCodFis.Text) <> "" Then
                'Aggiunto da Alessandra Taballione il 19/07/2005
                'Verifica deiìi codici fiscali enti  eccezioni
                Strsql = "select * from eccezionicodicifiscalienti where codicefiscale='" & Replace(Trim(txtCodFis.Text), "'", "''") & "'"
                If Not DTRGenerico Is Nothing Then
                    DTRGenerico.Close()
                    DTRGenerico = Nothing
                End If
                DTRGenerico = ClsServer.CreaDatareader(Strsql, Session("conn"))
                If DTRGenerico.HasRows = False Then
                    If Not DTRGenerico Is Nothing Then
                        DTRGenerico.Close()
                        DTRGenerico = Nothing
                    End If
                    'Verifico se il codice fiscale è gia presente nell'anagrafica Enti
                    Strsql = "Select idente,IdStatoEnte,IdClasseAccreditamento,CodiceRegione from enti where codicefiscale='" & Replace(Trim(txtCodFis.Text), "'", "''") & "'"
                    If Not DTRGenerico Is Nothing Then
                        DTRGenerico.Close()
                        DTRGenerico = Nothing
                    End If
                    DTRGenerico = ClsServer.CreaDatareader(Strsql, Session("conn"))
                    'se gia esistente viene bloccato l'inserimento
                    DTRGenerico.Read()
                    If DTRGenerico.HasRows = True Then
                        Dim Idente As Integer
                        Dim IdStatoEnte As Integer
                        Dim IdClasseAccr As Integer
                        Dim CodiceRegione As String
                        IdStatoEnte = IIf(Not IsDBNull(DTRGenerico("IdStatoEnte")), DTRGenerico("IdStatoEnte"), 0)
                        IdClasseAccr = IIf(Not IsDBNull(DTRGenerico("IdClasseAccreditamento")), DTRGenerico("IdClasseAccreditamento"), 0)
                        CodiceRegione = IIf(Not IsDBNull(DTRGenerico("CodiceRegione")), DTRGenerico("CodiceRegione"), "")
                        Idente = DTRGenerico("IdEnte")
                        'Image2.Visible = True
                        'Image2.ImageUrl = "images/alert3.gif"
                        'lblMessaggi.Visible = True
                        'lblMessaggi.ForeColor = Color.Red
                        'lblMessaggi.Text = "Il codice Fiscale inserito è già presente in archivio.Pertanto la richiesta non verrà inoltrata."
                        'attivare la maschera di invio e-mail (Rifiuto accaunt)
                        'Codice Fiscale Ente già esistente
                        If Not DTRGenerico Is Nothing Then
                            DTRGenerico.Close()
                            DTRGenerico = Nothing
                        End If

                        If IdStatoEnte = 3 Or IdStatoEnte = 8 Or IdStatoEnte = 9 Then
                            If IdClasseAccr <= 4 Then
                                messaggioRifiuta = messaggioRifiuta & " l'Ente risulta già accreditato con il codice " & CodiceRegione & ". Cordiali saluti."
                            Else
                                Dim codregpadre As String
                                'FARE LA QUERY Per ente figlio
                                Strsql = "SELECT enti.CodiceRegione, enti.IDEnte, entirelazioni.IDEnteFiglio FROM enti INNER JOIN entirelazioni ON enti.IDEnte = entirelazioni.IDEntePadre where identefiglio ='" & Idente & "'"
                                DTRGenerico = ClsServer.CreaDatareader(Strsql, Session("conn"))
                                DTRGenerico.Read()
                                If DTRGenerico.HasRows = True Then
                                    codregpadre = DTRGenerico("CodiceRegione")
                                    messaggioRifiuta &= " il suo ente risulta già accreditato ed in accordo con l'ente " & codregpadre
                                    messaggioRifiuta &= ". Se desidera accreditarsi autonomamente l'ente capofila (" & codregpadre & ") " & _
                                                        "deve provvedere ad annullare l'accordo seguendo le procedure previste dal Sistema Unico e," & _
                                                        " solo successivamente, lei potrà inoltrare una nuova richiesta di accesso al Sistema Unico. Cordiali saluti."
                                End If
                            End If
                        End If

                        If (IdStatoEnte = 6) And messaggioRifiuta = "" Then
                            If IdClasseAccr <= 4 Then
                                messaggioRifiuta = messaggioRifiuta & " l'Ente risulta già esistente con il codice " & CodiceRegione & ". Cordiali saluti."
                            Else
                                Dim codregpadre As String
                                'FARE LA QUERY Per ente figlio
                                Strsql = "SELECT enti.CodiceRegione, enti.Denominazione,  enti.IDEnte, entirelazioni.IDEnteFiglio FROM enti INNER JOIN entirelazioni ON enti.IDEnte = entirelazioni.IDEntePadre where identefiglio ='" & Idente & "'"
                                DTRGenerico = ClsServer.CreaDatareader(Strsql, Session("conn"))
                                DTRGenerico.Read()
                                If DTRGenerico.HasRows = True Then
                                    'mod. il 15/04/2008 da s.c.
                                    'se il codiceregione è NULL inserisco la denominazione dell'ente
                                    'è null quando si sta Accredidando un nuovo Ente  e questo è un Ente FIglio di un Nuovo Ente che si deve ancora accreditare
                                    If IsDBNull(DTRGenerico("CodiceRegione")) = False Then
                                        codregpadre = DTRGenerico("CodiceRegione")
                                    Else
                                        codregpadre = DTRGenerico("Denominazione")
                                    End If

                                    messaggioRifiuta &= " il suo ente risulta già esistente ed in accordo con l'ente " & codregpadre
                                    messaggioRifiuta &= ". Se desidera accreditarsi autonomamente l'ente capofila (" & codregpadre & ") " & _
                                                        "deve provvedere ad annullare l'accordo seguendo le procedure previste dal Sistema Unico e," & _
                                                       " solo successivamente, lei potrà inoltrare una nuova richiesta di accesso al Sistema Unico. Cordiali saluti."

                                Else
                                    messaggioRifiuta = messaggioRifiuta & " L'utenza risulta essere già stata assegnata per il codice fiscale indicato. Si prega di effettuare la richiesta di INOLTRO PASSWORD all'email heliosweb@serviziocivile.it nel caso essa sia andata smarrita. Cordiali saluti."

                                End If
                            End If
                        End If

                        'messaggioRifiuta = messaggioRifiuta & " Codice Fiscale Ente già esistente"

                        If Not DTRGenerico Is Nothing Then
                            DTRGenerico.Close()
                            DTRGenerico = Nothing
                        End If
                        'sqlLocalConn.Close()
                        'sqlLocalConn = Nothing
                        'Exit Sub
                    End If
                    If Not DTRGenerico Is Nothing Then
                        DTRGenerico.Close()
                        DTRGenerico = Nothing
                    End If
                End If
            End If

            If Trim(txtdenominazione.Text) <> "" And messaggioRifiuta = "" Then
                'Verifico se la denominazione è gia presente nell'anagrafica Enti
                Strsql = "Select idente from enti " & _
                " inner join statienti on statienti.idstatoente=enti.idstatoente " & _
                " where denominazione='" & Replace(Trim(txtdenominazione.Text), "'", "''") & "'" & _
                 " and (statienti.sospeso<>1 and statienti.chiuso<>1) AND statienti.IDStatoEnte<>11 "
                If Not DTRGenerico Is Nothing Then
                    DTRGenerico.Close()
                    DTRGenerico = Nothing
                End If
                DTRGenerico = ClsServer.CreaDatareader(Strsql, Session("conn"))
                'se gia esistente viene bloccato l'inserimento
                If DTRGenerico.HasRows = True Then
                    'Image2.Visible = True
                    'Image2.ImageUrl = "images/alert3.gif"
                    'lblMessaggi.Visible = True
                    'lblMessaggi.ForeColor = Color.Red
                    'lblMessaggi.Text = "La denominazione inserita è già presente in archivio. Pertanto la richiesta non verrà inoltrata."
                    'attivare la maschera di invio e-email (Rifiuto accaunt)
                    'Denominazione Ente già esistente
                    messaggioRifiuta = " La denominazione dell'Ente risulta già presente in archivio. E' necessario inviare una nuova richiesta di accesso al sistema specificando nella denominazione dell'Ente anche l'area geografica di appartenenza (Comune o Provincia o Regione). Cordiali saluti."


                    If Not DTRGenerico Is Nothing Then
                        DTRGenerico.Close()
                        DTRGenerico = Nothing
                    End If
                    'sqlLocalConn.Close()
                    'sqlLocalConn = Nothing
                    'Exit Sub
                End If
                If Not DTRGenerico Is Nothing Then
                    DTRGenerico.Close()
                    DTRGenerico = Nothing
                End If
            End If
        Else 'BLOCCO_ACCREDITAMENTO a SI 

            messaggioRifiuta &= "Dal giorno 15 novembre 2020 e fino al 31 maggio 2021 è sospesa temporaneamente la presentazione delle istanze di iscrizione e di parte delle istanze di adeguamento all’Albo degli enti di servizio civile universale (Circolare 22 ottobre 2020)."

        End If

        '******c'era prima il controllo sul codice fiscale trasferito su *****************
        Dim intIdEnte As Integer
        Inserimento(Session("conn"), intIdEnte)

        'Image2.Visible = True
        'lblMessaggi.Visible = True
        'lblMessaggi.ForeColor = Color.Navy
        'Image2.ImageUrl = "images/conf1.jpg"
        'lblMessaggi.Text = "La richiesta di Registrazione è stata inoltrata." & vbCrLf & "La Username e Password verranno inviate all'indirizzo di posta inserito. "

        If Not DTRGenerico Is Nothing Then
            DTRGenerico.Close()
            DTRGenerico = Nothing
        End If

        If messaggioRifiuta = "" Then
            Strsql = "Select enti.* from enti where idente = " & intIdEnte

            DTRGenerico = ClsServer.CreaDatareader(Strsql, Session("conn"))
            DTRGenerico.Read()
            If DTRGenerico.HasRows = True Then
                Context.Items.Add("Ente", DTRGenerico("Denominazione"))
                Context.Items.Add("Email", DTRGenerico("EMail"))
                Context.Items.Add("Azione", "Accetta")
                Context.Items.Add("strsql", "")
                Context.Items.Add("MessaggioErrore", "")
                Session("IdEnte") = DTRGenerico("IdEnte")
                Session("Denominazione") = DTRGenerico("Denominazione")
                DTRGenerico.Close()
                DTRGenerico = Nothing
                Server.Transfer("WfrmaccettaRic.aspx?")
            Else
                'Response.Write("PIPPO")
                DTRGenerico.Close()
                DTRGenerico = Nothing
            End If
        Else
            Strsql = "Select enti.* from enti where idente = " & intIdEnte
            DTRGenerico = ClsServer.CreaDatareader(Strsql, Session("conn"))
            DTRGenerico.Read()
            If DTRGenerico.HasRows = True Then
                Context.Items.Add("Ente", DTRGenerico("Denominazione"))
                Context.Items.Add("Email", DTRGenerico("EMail"))
                Context.Items.Add("Azione", "Rifiuta")
                Context.Items.Add("strsql", "")
                Context.Items.Add("MessaggioErrore", messaggioRifiuta)
                Session("IdEnte") = DTRGenerico("IdEnte")
                Session("Denominazione") = DTRGenerico("Denominazione")
                DTRGenerico.Close()
                DTRGenerico = Nothing
                Server.Transfer("WfrmaccettaRic.aspx")
            Else
                'Response.Write("PIPPO")
                DTRGenerico.Close()
                DTRGenerico = Nothing
            End If


        End If
        'sqlLocalConn.Close()
        'sqlLocalConn = Nothing
        PulisciMaschera()
    End Sub

    Function ValidazioneServerSalva() As Boolean

        If txtdenominazione.Text.Trim = String.Empty Then
            lblMessaggi.Visible = True
            lblMessaggi.Text = "Inserire Denominazione!"
            txtdenominazione.Focus()
            Return False

        End If

        If txtCodFis.Text.Trim = String.Empty Then
            lblMessaggi.Visible = True
            lblMessaggi.Text = "Inserire il Codice Fiscale!"
            txtCodFis.Focus()
            Return False

        End If

        If txtCodFis.Text.Trim.Length < 11 Then
            lblMessaggi.Visible = True
            lblMessaggi.Text = "Attenzione.Il codice fiscale deve essere di almeno 11 caratteri!"
            txtCodFis.Focus()
            Return False

        End If

        If txtEmail.Text.Trim = String.Empty Then
            lblMessaggi.Visible = True
            lblMessaggi.Text = "Inserire Email!"
            txtEmail.Focus()
            Return False

        End If

        If txtEmailCertificata.Text.Trim = String.Empty Then
            lblMessaggi.Visible = True
            lblMessaggi.Text = "Inserire la PEC!"
            txtEmail.Focus()
            Return False

        End If

        'Dim regex As Regex = New Regex("^[_a-z0-9-]+(.[a-z0-9-]+)@[a-z0-9-]+(.[a-z0-9-]+)*(.[a-z]{2,4})$")
        Dim regex As Regex = New Regex("^(?("")("".+?""@)|(([0-9a-zA-Z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-zA-Z])@))(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,6}))$")

        Dim match As Match = regex.Match(txtEmail.Text.Trim)
        If match.Success = False Then
            lblMessaggi.Visible = True
            lblMessaggi.Text = "Il formato Email non è valido!"
            txtEmail.Focus()
            Return False
        End If
        Dim matchP As Match = regex.Match(txtEmailCertificata.Text.Trim)
        If matchP.Success = False Then
            lblMessaggi.Visible = True
            lblMessaggi.Text = "Il formato PEC non è valido!"
            txtEmail.Focus()
            Return False

        End If

        Dim prefissoTelefono As Integer
        Dim prefissoTelefonoInteger As Boolean
        prefissoTelefonoInteger = Integer.TryParse(txtprefisso.Text.Trim, prefissoTelefono)
        If txtprefisso.Text.Trim <> String.Empty AndAlso prefissoTelefonoInteger = False Then
            lblMessaggi.Visible = True
            lblMessaggi.Text = "Il Prefisso Telefono può contenere solo numeri."
            txtprefisso.Focus()
            Return False

        End If

        Dim numeroTelefono As Int64
        Dim numeroTelefonoInteger As Boolean
        numeroTelefonoInteger = Int64.TryParse(txtTelefono.Text.Trim, numeroTelefono)
        If txtTelefono.Text.Trim <> String.Empty AndAlso numeroTelefonoInteger = False Then
            lblMessaggi.Visible = True
            lblMessaggi.Text = "Il Telefono può contenere solo numeri."
            txtTelefono.Focus()
            Return False

        End If

        If TxtDataRicezioneCarta.Text.Trim = String.Empty Then
            lblMessaggi.Visible = True
            lblMessaggi.Text = "Inserire Data Ricezione Cartacea!"
            TxtDataRicezioneCarta.Focus()
            Return False

        End If

        Dim dataRicezione As Date
        If (Date.TryParse(TxtDataRicezioneCarta.Text, dataRicezione) = False) Then
            lblMessaggi.Visible = True
            lblMessaggi.Text = "La 'Data Ricezione' non è valida. Inserire la data nel formato GG/MM/AAAA."
            TxtDataRicezioneCarta.Focus()
            Return False
        End If

        Return True

    End Function


End Class