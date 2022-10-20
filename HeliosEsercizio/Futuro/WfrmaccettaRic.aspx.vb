Imports System.Drawing

Public Class WfrmaccettaRic
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Generato da Alessandra Taballione il 12/02/2004
        'Inserimento del codice necessario per inizializzare la pagina
        Dim strPass As New ClsUtility
        Dim i As Integer 'Variabile Contatore
        Dim intLen As Integer 'Variabile intera che identifica la lungezza di una stringa
        Dim StrSql As String 'Variabile di tipo stringa
        Dim DTRGenerico As SqlClient.SqlDataReader 'Variabile di tipo SqlClient.SqlDataReader

        If Not Session("LogIn") Is Nothing Then
            If Session("LogIn") = False Then
                Response.Redirect("LogOn.aspx")
            End If
        Else
            Response.Redirect("LogOn.aspx")
        End If
        lblMessaggio.Visible = False
        If IsPostBack = False Then
            txtNote.Text = Context.Items("MessaggioErrore").ToString
            txtrisultatoricerca.Value = Context.Items("strsql")
            lblEnte.Text = Trim(Context.Items("Ente").ToString)
            txtA.Text = Context.Items("Email").ToString
            txtDA.Text = "HELIOSWEB"
            'agg. il 11/09/2008 da simona cordella
            'imposto per defoult la durata della password che è modificabile in fase di invio dell'utenza
            DurataPassword()
            'personalizzazione Maschera A seconda dell'azione che svolge Accetta o Respingi
            If Context.Items("Azione") = "Accetta" Then
                ddlOperazione.SelectedValue = 1
                Accetta()
            Else
                ddlOperazione.SelectedValue = 2
                Rifiuta()
            End If
        End If
    End Sub

    Private Sub DurataPassword()
        'agg. il 11/09/2008 da simona cordella
        'imposto per defoult la durata della password che è modificabile in fase di invio dell'utenza
        Dim strPass As String
        Dim dtrPw As SqlClient.SqlDataReader
        If Not dtrPw Is Nothing Then
            dtrPw.Close()
            dtrPw = Nothing
        End If

        strPass = "Select DurataPassword from duratapwd"
        dtrPw = ClsServer.CreaDatareader(strPass, Session("conn"))
        If dtrPw.HasRows = True Then
            dtrPw.Read()
            txtDurataPw.Text = dtrPw("DurataPassword")
        End If
        If Not dtrPw Is Nothing Then
            dtrPw.Close()
            dtrPw = Nothing
        End If
    End Sub

    Private Sub Accetta()
        Dim strPass As New ClsUtility
        Dim i As Integer 'Variabile Contatore
        Dim intLen As Integer 'Variabile intera che identifica la lungezza di una stringa
        Dim StrSql As String 'Variabile di tipo stringa
        Dim DTRGenerico As SqlClient.SqlDataReader 'Variabile di tipo SqlClient.SqlDataReader

        lblTitolo.Text = "Accettazione Domanda Account"
        'mod. il 11/09/2008

        'txtPassword.Text = strPass.NuovaPass()
        txtPassword.Text = strPass.NuovaPasswordADC
        'txtPassword.Attributes.Add("value", strPass.NuovaPasswordADC)

        '************************************nuovo algoritmo per le regioni***********************
        'E960007245
        'variabili locali che uso per costruire la stringa contenente la nuova UserId
        'E96 è il suffisso, il codice è composto dall'Id sommato a 7218
        'nella variabile dbl Totale metto il codice che utilizzerò poi per controllarne la lunghezza
        'per far si che il codice sia lungo 10
        Dim strCodiceFissoIniziale As String = "E96"
        Dim intCodiceFissodaSommare As Integer = 7218
        Dim dblTotale As Double
        Dim strCodiceTemporaneo As String
        Dim strDataricezioneCartacea As String


        dblTotale = CDbl(Session("IdEnte")) + intCodiceFissodaSommare

        'vado a controllare la lunghezza del codice generato così da aggiungere gli zeri necessari
        Select Case Len(dblTotale.ToString)
            Case 4 'nel caso restituisca 4 dovrò aggiungere 4 zeri
                strCodiceTemporaneo = strCodiceFissoIniziale & "000" & dblTotale.ToString
            Case 5 'nel caso restituisca 4 dovrò aggiungere 3 zeri
                strCodiceTemporaneo = strCodiceFissoIniziale & "00" & dblTotale.ToString
            Case 6 'nel caso restituisca 4 dovrò aggiungere 2 zeri
                strCodiceTemporaneo = strCodiceFissoIniziale & "0" & dblTotale.ToString
        End Select

        txtUser.Text = strCodiceTemporaneo

        '*****************************************************************************************

        '*************************vado a prendere l'indirizzo email di riferimento****************
        If Not DTRGenerico Is Nothing Then
            DTRGenerico.Close()
            DTRGenerico = Nothing
        End If

        StrSql = "SELECT RegioniCompetenze.Mail AS MailCC "
        StrSql = StrSql & "FROM UtentiUNSC "
        StrSql = StrSql & "INNER JOIN RegioniCompetenze ON UtentiUNSC.IdRegioneCompetenza = RegioniCompetenze.IdRegioneCompetenza "
        StrSql = StrSql & "WHERE UtentiUNSC.UserName='" & Session("Utente") & "'"

        DTRGenerico = ClsServer.CreaDatareader(StrSql, Session("conn"))
        'metto nel campo CC l'indirizzo email della regione competente
        If DTRGenerico.HasRows = True Then
            DTRGenerico.Read()
            txtCC.Text = DTRGenerico("MailCC")
        Else 'metto una stringa vuota
            txtCC.Text = ""
        End If
        If Not DTRGenerico Is Nothing Then
            DTRGenerico.Close()
            DTRGenerico = Nothing
        End If
        '*****************************************************************************************


        '*****************************************************************************************
        'vecchio algoritmo
        'Creazione dell'utenza data dall'idEnte per l'intera lunghezza del tipo di dato
        'intLen = ClsServer.trovaId("enti", "idente", "idente", Session("IdEnte"), Session("conn"))
        'If Len(Trim(CInt(intLen))) < 10 Then
        '    txtUser.Text = "E"
        '    For i = 1 To (9 - Len(Trim(CInt(intLen))))
        '        txtUser.Text = txtUser.Text & "0"
        '    Next i
        '    txtUser.Text = txtUser.Text & intLen
        'End If
        '*****************************************************************************************
        txtOggetto.Text = "ACCETTAZIONE DOMANDA ACCOUNT"
        '********************GESTIONE LETTURA MESSAGGIO DAL DATABASE******************************

        If Not DTRGenerico Is Nothing Then
            DTRGenerico.Close()
            DTRGenerico = Nothing
        End If

        StrSql = "SELECT TestiEmail.TestoEmailFormattato "
        StrSql = StrSql & "FROM TestiEmail "
        StrSql = StrSql & "WHERE TestiEmail.TipoEmail=1"

        DTRGenerico = ClsServer.CreaDatareader(StrSql, Session("conn"))
        'metto nel campo CC l'indirizzo email della regione competente
        If DTRGenerico.HasRows = True Then
            DTRGenerico.Read()
            txtmessaggio.Text = DTRGenerico("TestoEmailFormattato")
        Else 'metto una stringa vuota
            txtmessaggio.Text = ""
        End If

        If Not DTRGenerico Is Nothing Then
            DTRGenerico.Close()
            DTRGenerico = Nothing
        End If

        StrSql = "SELECT "
        StrSql = StrSql & "isnull(case len(day(enti.DataRicezioneCartacea)) when 1 then '0' + convert(varchar(20),day(enti.DataRicezioneCartacea)) "
        StrSql = StrSql & "else convert(varchar(20),day(enti.DataRicezioneCartacea))  end + '/' + "
        StrSql = StrSql & "(case len(month(enti.DataRicezioneCartacea)) when 1 then '0' + convert(varchar(20),month(enti.DataRicezioneCartacea)) "
        StrSql = StrSql & "else convert(varchar(20),month(enti.DataRicezioneCartacea))  end + '/' + "
        StrSql = StrSql & "Convert(varchar(20), Year(enti.DataRicezioneCartacea))), 'XX/XX/XXXX') as DataRicezioneCartacea  "
        StrSql = StrSql & "FROM enti "
        StrSql = StrSql & "WHERE enti.idente='" & Session("IdEnte") & "'"

        DTRGenerico = ClsServer.CreaDatareader(StrSql, Session("conn"))
        'metto nel campo CC l'indirizzo email della regione competente
        If DTRGenerico.HasRows = True Then
            DTRGenerico.Read()
            strDataricezioneCartacea = DTRGenerico("DataRicezioneCartacea")
        Else 'metto una stringa vuota
            strDataricezioneCartacea = ""
        End If

        If Not DTRGenerico Is Nothing Then
            DTRGenerico.Close()
            DTRGenerico = Nothing
        End If

        'faccio la replace della username, password, ente, dataricezionecartacea
        txtmessaggio.Text = Replace(txtmessaggio.Text, "<USERNAME>", txtUser.Text)
        txtmessaggio.Text = Replace(txtmessaggio.Text, "<PASSWORD>", txtPassword.Text)
        txtmessaggio.Text = Replace(txtmessaggio.Text, "<ENTE>", lblEnte.Text)
        txtmessaggio.Text = Replace(txtmessaggio.Text, "<DATARICEZIONECARTACEA>", strDataricezioneCartacea)

        '*****************************************************************************************
        'txtmessaggio.Text = "Si Comunica che la Richiesta Inoltrata in data  " & Context.Items("Data").ToString & " è stata ACCETTATA." & vbCrLf & "Il nome Utente assegnato è " & txtUser.Text & vbCrLf & ""
        'txtmessaggio.Text = txtmessaggio.Text & "e la Password è " & txtPassword.Text & "."
        'txtmessaggio.Text = txtmessaggio.Text & vbCrLf & "Attenzione: la password è sensibile alle maiuscole e minuscole. "
    End Sub

    Private Sub Rifiuta()
        Dim strPass As New ClsUtility
        Dim i As Integer 'Variabile Contatore
        Dim intLen As Integer 'Variabile intera che identifica la lungezza di una stringa
        Dim StrSql As String 'Variabile di tipo stringa
        Dim DTRGenerico As SqlClient.SqlDataReader 'Variabile di tipo SqlClient.SqlDataReader
        Dim strDataricezioneCartacea As String

        lblTitolo.Text = "Rifiuto Domanda Account"
        'lblEnte.Text = Trim(Context.Items("Ente").ToString)
        'txtA.Text = Context.Items("Email").ToString
        'txtDA.Text = "HELIOSWEB"
        '*************************vado a prendere l'indirizzo email di riferimento****************
        If Not DTRGenerico Is Nothing Then
            DTRGenerico.Close()
            DTRGenerico = Nothing
        End If

        StrSql = "SELECT RegioniCompetenze.Mail AS MailCC "
        StrSql = StrSql & "FROM UtentiUNSC "
        StrSql = StrSql & "INNER JOIN RegioniCompetenze ON UtentiUNSC.IdRegioneCompetenza = RegioniCompetenze.IdRegioneCompetenza "
        StrSql = StrSql & "WHERE UtentiUNSC.UserName='" & Session("Utente") & "'"

        DTRGenerico = ClsServer.CreaDatareader(StrSql, Session("conn"))
        'metto nel campo CC l'indirizzo email della regione competente
        If DTRGenerico.HasRows = True Then
            DTRGenerico.Read()
            txtCC.Text = DTRGenerico("MailCC")
        Else 'metto una stringa vuota
            txtCC.Text = ""
        End If
        If Not DTRGenerico Is Nothing Then
            DTRGenerico.Close()
            DTRGenerico = Nothing
        End If
        '*****************************************************************************************
        txtPassword.Enabled = False
        txtPassword.BackColor = Color.Gainsboro
        txtUser.BackColor = Color.Gainsboro
        txtUser.Enabled = False
        txtOggetto.Text = "RIFIUTO DOMANDA ACCOUNT"

        '***********************************testo email**********************************************


        If Not DTRGenerico Is Nothing Then
            DTRGenerico.Close()
            DTRGenerico = Nothing
        End If

        StrSql = "SELECT TestiEmail.TestoEmailFormattato "
        StrSql = StrSql & "FROM TestiEmail "
        StrSql = StrSql & "WHERE TestiEmail.TipoEmail=2"

        DTRGenerico = ClsServer.CreaDatareader(StrSql, Session("conn"))
        'metto nel campo CC l'indirizzo email della regione competente
        If DTRGenerico.HasRows = True Then
            DTRGenerico.Read()
            txtmessaggio.Text = DTRGenerico("TestoEmailFormattato")
        Else 'metto una stringa vuota
            txtmessaggio.Text = ""
        End If

        If Not DTRGenerico Is Nothing Then
            DTRGenerico.Close()
            DTRGenerico = Nothing
        End If

        StrSql = "SELECT "
        StrSql = StrSql & "isnull(case len(day(enti.DataRicezioneCartacea)) when 1 then '0' + convert(varchar(20),day(enti.DataRicezioneCartacea)) "
        StrSql = StrSql & "else convert(varchar(20),day(enti.DataRicezioneCartacea))  end + '/' + "
        StrSql = StrSql & "(case len(month(enti.DataRicezioneCartacea)) when 1 then '0' + convert(varchar(20),month(enti.DataRicezioneCartacea)) "
        StrSql = StrSql & "else convert(varchar(20),month(enti.DataRicezioneCartacea))  end + '/' + "
        StrSql = StrSql & "Convert(varchar(20), Year(enti.DataRicezioneCartacea))), 'XX/XX/XXXX') as DataRicezioneCartacea  "
        StrSql = StrSql & "FROM enti "
        StrSql = StrSql & "WHERE enti.idente='" & Session("IdEnte") & "'"

        DTRGenerico = ClsServer.CreaDatareader(StrSql, Session("conn"))
        'metto nel campo CC l'indirizzo email della regione competente
        If DTRGenerico.HasRows = True Then
            DTRGenerico.Read()
            strDataricezioneCartacea = DTRGenerico("DataRicezioneCartacea")
        Else 'metto una stringa vuota
            strDataricezioneCartacea = ""
        End If

        If Not DTRGenerico Is Nothing Then
            DTRGenerico.Close()
            DTRGenerico = Nothing
        End If

        'faccio la replace della username, password, ente, dataricezionecartacea
        txtmessaggio.Text = Replace(txtmessaggio.Text, "<USERNAME>", txtUser.Text)
        'txtmessaggio.Text = Replace(txtmessaggio.Text, "<PASSWORD>", txtPassword.Text)
        txtmessaggio.Text = Replace(txtmessaggio.Text, "<ENTE>", lblEnte.Text)
        txtmessaggio.Text = Replace(txtmessaggio.Text, "<DATARICEZIONECARTACEA>", strDataricezioneCartacea)

        '*****************************************************************************************
        'txtmessaggio.Text = "Si Comunica che la Richiesta Inoltrata in data  " & Context.Items("Data").ToString & " è stata RESPINTA."
    End Sub

    Private Sub cmdInvia_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdInvia.Click

        If ValidazioneServerSalva() = True Then
            Invia()
        End If

    End Sub

    Private Function StoreInviaEmail(ByVal IdEnte As String, ByVal intTipologiaRichiesta As Integer, ByVal strCC As String, ByVal strNote As String) As Boolean
        Dim CustOrderHist As SqlClient.SqlCommand
        CustOrderHist = New SqlClient.SqlCommand
        CustOrderHist.CommandType = CommandType.StoredProcedure
        CustOrderHist.CommandText = "SP_MAIL_ACCOUNT_ENTI"
        CustOrderHist.Connection = Session("conn")

        '@IdEnte varchar(100),
        '@Tipo INT,
        '@CC NVARCHAR(100),
        '@NoteAggiuntive NVARCHAR(1200),

        Dim paramEnte As SqlClient.SqlParameter
        paramEnte = New SqlClient.SqlParameter
        paramEnte.ParameterName = "@IdEnte"
        paramEnte.SqlDbType = SqlDbType.VarChar
        CustOrderHist.Parameters.Add(paramEnte)

        Dim paramTipo As SqlClient.SqlParameter
        paramTipo = New SqlClient.SqlParameter
        paramTipo.ParameterName = "@Tipo"
        paramTipo.SqlDbType = SqlDbType.Int
        CustOrderHist.Parameters.Add(paramTipo)

        Dim paramCC As SqlClient.SqlParameter
        paramCC = New SqlClient.SqlParameter
        paramCC.ParameterName = "@CC"
        paramCC.SqlDbType = SqlDbType.NVarChar
        CustOrderHist.Parameters.Add(paramCC)

        Dim paramNote As SqlClient.SqlParameter
        paramNote = New SqlClient.SqlParameter
        paramNote.ParameterName = "@NoteAggiuntive"
        paramNote.SqlDbType = SqlDbType.NVarChar
        CustOrderHist.Parameters.Add(paramNote)

        Dim paramEsito As SqlClient.SqlParameter
        paramEsito = New SqlClient.SqlParameter
        paramEsito.ParameterName = "@Esito"
        paramEsito.SqlDbType = SqlDbType.Int
        paramEsito.Direction = ParameterDirection.Output
        CustOrderHist.Parameters.Add(paramEsito)

        Dim Reader As SqlClient.SqlDataReader
        CustOrderHist.Parameters("@IdEnte").Value = IdEnte
        CustOrderHist.Parameters("@Tipo").Value = intTipologiaRichiesta
        CustOrderHist.Parameters("@CC").Value = strCC
        CustOrderHist.Parameters("@NoteAggiuntive").Value = strNote & " "
        Reader = CustOrderHist.ExecuteReader()
        ' Insert code to read through the datareader.
        'If CustOrderHist.Parameters("@Valore").Value = 0 Then
        StoreInviaEmail = CustOrderHist.Parameters("@Esito").Value
        If Not Reader Is Nothing Then
            Reader.Close()
            Reader = Nothing
        End If
        'End If
        'connessione.Close()
    End Function

    Private Sub PulisciCampi()
        'Creato da Alessandra taballione il 05/02/2004
        'Effettuo la pulizia degli oggetti della maschera 
        'dopo aver effettuato l'accettazione o il rifiuto della Domanda
        'Modificato da alessandra Taballione il 10/06/2004
        'Non Effettuo la pulizia dei campi dando così la possibilità all'utente di vedere 
        ' idati dopo la conferma.
        'txtA.Text = ""
        'txtA.Enabled = False
        txtA.BackColor = Color.Gainsboro
        txtA.ReadOnly = True
        ddlOperazione.Enabled = False
        'txtDA.Text = ""
        'txtDA.Enabled = False
        txtDA.BackColor = Color.Gainsboro
        txtDA.ReadOnly = True
        'txtCC.Text = ""
        'txtCC.Enabled = False
        'txtCC.BackColor = Color.Gainsboro
        'txtCC.ReadOnly = True
        'txtmessaggio.Text = ""
        'txtmessaggio.Enabled = False
        txtmessaggio.ReadOnly = True
        txtmessaggio.BackColor = Color.Gainsboro
        'txtNote.Text = ""
        'txtNote.Enabled = False
        txtNote.ReadOnly = True
        txtNote.BackColor = Color.Gainsboro
        'txtOggetto.Text = ""
        'txtOggetto.Enabled = False
        txtOggetto.ReadOnly = True
        txtOggetto.BackColor = Color.Gainsboro
        'txtPassword.Text = ""
        'txtPassword.Enabled = False
        txtPassword.BackColor = Color.Gainsboro
        txtPassword.ReadOnly = True
        'txtUser.Text = ""
        'txtUser.Enabled = False
        txtUser.ReadOnly = True
        txtUser.BackColor = Color.Gainsboro
        'ChKAccodamess.Checked = False
        ChKAccodamess.Enabled = False
        cmdInvia.Enabled = False
    End Sub

    Private Sub cmdChiudi_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdChiudi.Click
        'Generato da Alessandra taballione il 15/02/2004
        'Richiamo la Pagina ri Risultato Richieste Account Inviado la Query di Partenza.
        If txtrisultatoricerca.Value = "" Then
            Response.Redirect("WfrmMain.aspx")
        Else
            Context.Items.Add("strsql", txtrisultatoricerca.Value)
            Server.Transfer("wfrmRisultatoRicercaRicAccount.aspx")
        End If
    End Sub

    Private Sub ddlOperazione_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlOperazione.SelectedIndexChanged
        'txtmessaggio.Text = ""
        Select Case ddlOperazione.SelectedValue
            Case 2
                txtUser.Text = ""
                txtPassword.Text = ""
                Rifiuta()
            Case 1
                Accetta()
        End Select
    End Sub

    Private Sub ChKAccodamess_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ChKAccodamess.CheckedChanged

        If ChKAccodamess.Checked = True And txtNote.Text.Trim <> String.Empty Then
            txtmessaggio.Text = txtmessaggio.Text & vbCrLf & "Note: " & txtNote.Text
        End If
        
    End Sub

    Private Sub Invia()
        'Generato da Alessandra taballione il 13/02/2004
        'Accetazione  e Rifiuto Richiesta Account 
        Dim StrSql As String 'Variabile di tipo stringa
        Dim DTRGenerico As SqlClient.SqlDataReader 'Variabile di tipo SqlClient.SqlDataReader
        Dim DTGenerico As DataTable 'Variabile di tipo DataTable
        Dim IntStato As Integer 'Variabile intera che conserva lo stato dell'Ente
        Dim rstGenerico As SqlClient.SqlCommand
        If lblTitolo.Text = "Accettazione Domanda Account" Then
            'Inizio Controlli Formali

            'Lerrura Valore idstato= Registrato
            If Not DTRGenerico Is Nothing Then
                DTRGenerico.Close()
                DTRGenerico = Nothing
            End If
            DTRGenerico = ClsServer.CreaDatareader("Select idStatoEnte from statienti where statoEnte='Registrato'", Session("conn"))
            DTRGenerico.Read()
            If DTRGenerico.HasRows = True Then
                'Effettuo modifica dati
                IntStato = DTRGenerico("idStatoEnte")
                'agg. da simona il salvataggio del FlagForzaturaAccreditamento =1 per Accreditamento 2014
                StrSql = "Update enti set idstatoente=" & IntStato & ", FlagForzaturaAccreditamento=1 where idente=" & Session("idente")
                If Not DTRGenerico Is Nothing Then
                    DTRGenerico.Close()
                    DTRGenerico = Nothing
                End If
                rstGenerico = ClsServer.EseguiSqlClient(StrSql, Session("conn"))
                'aggiunto da jonathani
                'scarico su entipassword la nuova utenza
                'mod. il 11/09/2008
                'agg. nuovi campi :  DataModificaPassword, DurataPassword, CambioPassword, PasswordDaInviare
                StrSql = " insert into entipassword (IdEnte, Username, Password, Password1, " & _
                         " DataModificaPassword, DurataPassword, CambioPassword, PasswordDaInviare) " & _
                         " values ('" & Session("idente") & "','" & Replace(txtUser.Text, "'", "''") & "','" & ClsUtility.CreatePSW(txtPassword.Text) & "','" & txtPassword.Text & "', " & _
                         " getdate()," & txtDurataPw.Text & ",1,1) "
                '" where idente='" & IntEnte & "'"
                If Not DTRGenerico Is Nothing Then
                    DTRGenerico.Close()
                    DTRGenerico = Nothing
                End If
                rstGenerico = ClsServer.EseguiSqlClient(StrSql, Session("conn"))

                'Effettuo inserimento Cronologia stato
                'con lo stato Registrato
                StrSql = "Insert into CronologiaEntiStati (idente, idstatoEnte,datacronologia,note,idtipocronologia,UsernameAccreditatore)values " & _
                " (" & Session("idente") & "," & _
                " " & IntStato & ",getdate(),'" & Replace(txtNote.Text, "'", "''") & "', 0,'" & Replace(Session("Utente"), "'", "''") & "')"
                If Not DTRGenerico Is Nothing Then
                    DTRGenerico.Close()
                    DTRGenerico = Nothing
                End If
                rstGenerico = ClsServer.EseguiSqlClient(StrSql, Session("conn"))
                'agg. il 11/09/2008 da s.c.
                'Effettuo inserimento in CronologiaPasswordEnti
                StrSql = "Insert into CronologiaPasswordEnti (Username, Password, DataCronologia) values " & _
                        " ('" & Replace(txtUser.Text, "'", "''") & "','" & ClsUtility.CreatePSW(txtPassword.Text) & "', getdate())"
                If Not DTRGenerico Is Nothing Then
                    DTRGenerico.Close()
                    DTRGenerico = Nothing
                End If
                rstGenerico = ClsServer.EseguiSqlClient(StrSql, Session("conn"))
                'Agg il 11/09/2014 da simona per nuovo Accreditamento 20014
                'Effettuo inserimento in EntiFasi
                StrSql = "Insert into EntiFasi (idente, tipofase, datainiziofase, " & _
                         " datafinefase, stato,UserNameInizioFase) " & _
                         " select " & Session("idente") & ", 1, getdate(), " & _
                         " DATEADD(S,-1,DATEADD(D,convert(int,(select valore from configurazioni where parametro = 'durata_accr'))+1,DBO.FORMATODATADT(GetDate()))), " & _
                         " 1, '" & Session("Utente") & "'"
                If Not DTRGenerico Is Nothing Then
                    DTRGenerico.Close()
                    DTRGenerico = Nothing
                End If
                rstGenerico = ClsServer.EseguiSqlClient(StrSql, Session("conn"))

            End If
            'Modificato da Alessandra Taballione il 10/06/2004
            'inserisco il Profilo del Nuovo Utente
            StrSql = "insert into AssociaUtenteGruppo(username,idprofilo) select  '" & txtUser.Text & "' ," & _
                    " idprofilo from Profili where abilitato=1 and Tipo='E' and defaultTipo=1"
            rstGenerico = ClsServer.EseguiSqlClient(StrSql, Session("conn"))
            'invio E-mail di avvenuta Registrazione
            'Call invioEmail(txtDA.Text, txtA.Text, txtCC.Text, txtOggetto.Text, txtmessaggio.Text)
            'messaggio Utente
            Try
                'false andata bene
                'true andata male
                If StoreInviaEmail(Session("IdEnte"), ddlOperazione.SelectedValue, txtCC.Text, txtNote.Text) = False Then
                    lblMessaggio.Visible = True
                    lblMessaggio.Text = "Registrazione Effettuata!"
                    Call PulisciCampi()
                Else
                    lblMessaggio.Visible = True
                    'Image5.Visible = True
                    lblMessaggio.Text = "Attenzione, si sono verificati dei problemi nell'invio della posta. Contattare l'assistenza."
                    Call PulisciCampi()
                End If
            Catch ex As Exception
                lblMessaggio.Visible = True
                'Image5.Visible = True
                lblMessaggio.Text = "Attenzione, si sono verificati dei problemi nell'invio della posta. Contattare l'assistenza."
                Call PulisciCampi()
            End Try
            'If LeggiStore(CInt(Request.Form("IdPersonale")), CInt(dtgRuoliSecondari.SelectedItem.Cells(1).Text)) = False Then

        Else

            'Lerrura Valore idstato= Registrato
            If Not DTRGenerico Is Nothing Then
                DTRGenerico.Close()
                DTRGenerico = Nothing
            End If
            DTRGenerico = ClsServer.CreaDatareader("Select idStatoEnte from statienti where statoEnte='Richiesta Respinta'", Session("conn"))
            DTRGenerico.Read()
            If DTRGenerico.HasRows = True Then
                IntStato = DTRGenerico("idStatoEnte")
                'Effettuo modifica dati
                StrSql = "Update enti set " & _
                " idstatoente=" & IntStato & ", codicefiscalearchivio = codicefiscale, codicefiscale =null where idente=" & Session("idente")
                If Not DTRGenerico Is Nothing Then
                    DTRGenerico.Close()
                    DTRGenerico = Nothing
                End If
                rstGenerico = ClsServer.EseguiSqlClient(StrSql, Session("conn"))
                'Effettuo inserimento Cronologia stato
                'con lo stato Registrato
                If Not DTRGenerico Is Nothing Then
                    DTRGenerico.Close()
                    DTRGenerico = Nothing
                End If
                StrSql = "Insert into CronologiaEntiStati (idente," & _
                "idstatoEnte,datacronologia,note,idtipocronologia,UsernameAccreditatore)values " & _
                " (" & Session("idente") & "," & _
                " " & IntStato & ",getdate(),'" & Replace(txtNote.Text, "'", "''") & "', 0,'" & Replace(Session("Utente"), "'", "''") & "')"
                If Not DTRGenerico Is Nothing Then
                    DTRGenerico.Close()
                    DTRGenerico = Nothing
                End If
                rstGenerico = ClsServer.EseguiSqlClient(StrSql, Session("conn"))
            End If
            'invio E-mail di Registrazione Respinta
            Try
                'false andata bene
                'true andata male
                If StoreInviaEmail(Session("IdEnte"), ddlOperazione.SelectedValue, txtCC.Text, txtNote.Text) = False Then
                    'Call invioEmail(txtDA.Text, txtA.Text, txtCC.Text, txtOggetto.Text, txtmessaggio.Text)
                    'messaggio Utente
                    lblMessaggio.Visible = True
                    lblMessaggio.Text = "Domanda Richiesta Account RIFIUTATA!"
                    Call PulisciCampi()
                Else
                    lblMessaggio.Visible = True
                    'Image5.Visible = True
                    lblMessaggio.Text = "Attenzione, si sono verificati dei problemi nell'invio della posta. Contattare l'assistenza."
                    Call PulisciCampi()
                End If
            Catch ex As Exception
                lblMessaggio.Visible = True
                'Image5.Visible = True
                lblMessaggio.Text = "Attenzione, si sono verificati dei problemi nell'invio della posta. Contattare l'assistenza."
                Call PulisciCampi()
            End Try

        End If
        If Not DTRGenerico Is Nothing Then
            DTRGenerico.Close()
            DTRGenerico = Nothing
        End If
    End Sub

    Function ValidazioneServerSalva() As Boolean

        If txtDA.Text.Trim = String.Empty Then
            lblMessaggio.Visible = True
            lblMessaggio.Text = "Inserire il Mittente!"
            txtDA.Focus()
            Return False

        End If

        If txtA.Text.Trim = String.Empty Then
            lblMessaggio.Visible = True
            lblMessaggio.Text = "Inserire il Destinatario!"
            txtA.Focus()
            Return False

        End If

        If txtOggetto.Text.Trim = String.Empty Then
            lblMessaggio.Visible = True
            lblMessaggio.Text = "Inserire Oggetto!"
            txtOggetto.Focus()
            Return False

        End If

        If txtmessaggio.Text.Trim = String.Empty Then
            lblMessaggio.Visible = True
            lblMessaggio.Text = "Inserire il Messaggio!"
            txtmessaggio.Focus()
            Return False

        End If

        If (txtDurataPw.Text.Trim = String.Empty Or txtDurataPw.Text.Trim = "0") Then
            lblMessaggio.Visible = True
            lblMessaggio.Text = "Inserire la durata della password!"
            txtDurataPw.Focus()
            Return False

        End If

        Return True

    End Function

End Class