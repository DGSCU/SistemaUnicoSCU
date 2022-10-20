Imports System.IO

Public Class WfrmGestioneSottoUtenza
    Inherits System.Web.UI.Page
    'REALIZZATA DA: SIMONA CORDELLA 
    'DATA REALIZZAZIONE:  05/03/2015
    'FUNZIONALITA': GESTIONE SOTTOUTENZE

    Dim UtenzeSPID As Boolean

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Session("LogIn") Is Nothing Then
            If Session("LogIn") = False Then 'verifico validità log-in
                Response.Redirect("LogOn.aspx")
            End If
        Else
            Response.Redirect("LogOn.aspx")
        End If

        UtenzeSPID = UtenzeSedeSPID()
        If UtenzeSPID Then
            lblTitolo.Text = "Gestore degli operatori volontari per sede"
            myLegend.InnerText = "Gestore degli operatori volontari per sede"
        End If

        If Page.IsPostBack = False Then
            If VerificaAbilitazione(Session("Utente"), Session("conn")) = False Then
                Response.Redirect("wfrmAnomaliaDati.aspx")
            End If
            LoadMaschera(Request.QueryString("IdEnteSedeAttuazione"))
        End If
        cmdAttiva.UseSubmitBehavior() = False
        cmdDisattiva.UseSubmitBehavior() = False
        cmdInoltroPW.UseSubmitBehavior() = False
        cmdAggiungi.UseSubmitBehavior() = False
    End Sub

    Protected Sub cmdChiudi_Click(sender As Object, e As EventArgs) Handles cmdChiudi.Click
        Response.Redirect("WfrmRicercaUtenzeSedi.aspx")
    End Sub

    Private Sub CaricaMaschera(IdEnteSedeAttuazione)
        Dim strsql As String
        Dim MyDataset As DataSet
        strsql = "SELECT esa.IdEnteSedeAttuazione, es.denominazione as Denominazionesede,es.email,es.indirizzo  + ' ' + es.civico as indirizzo,c.denominazione as comune, "
        strsql &= " case isnull(sp.abilitato,0) when 0 then 'Non Attiva' Else 'Attiva' end as Abilitato, sp.Username "
        strsql &= " FROM entisediattuazioni ESA"
        strsql &= " INNER JOIN entisedi ES ON ESA.IDEnteSede = ES.IDEnteSede"
        strsql &= " inner join comuni c on c.idcomune = es.idcomune "
        strsql &= " left JOIN sedipassword sp on sp.IdEnteSedeAttuazione=esa.IdEnteSedeAttuazione"
        If UtenzeSPID Then
            strsql &= " and left(sp.Username,3)='EDS'"
        Else
            strsql &= " and left(sp.Username,3)!='EDS'"
        End If
        strsql &= " WHERE esa.identeCapofila=" & Session("IdEnte") & " and  esa.IdEnteSedeAttuazione =" & IdEnteSedeAttuazione & ""


        MyDataset = ClsServer.DataSetGenerico(strsql, Session("conn"))

        If MyDataset.Tables(0).Rows.Count <> 0 Then
            lblCodiceSede.Text = MyDataset.Tables(0).Rows(0).Item("IdEnteSedeAttuazione")
            lblSede.Text = MyDataset.Tables(0).Rows(0).Item("Denominazionesede")
            LblIndirizzo.Text = MyDataset.Tables(0).Rows(0).Item("indirizzo")
            LblComune.Text = MyDataset.Tables(0).Rows(0).Item("comune")
            TxtEmail.Text = "" & MyDataset.Tables(0).Rows(0).Item("email")
            LblAbilitato.Text = MyDataset.Tables(0).Rows(0).Item("Abilitato")
            If LblAbilitato.Text = "Non Attiva" Then
                LabelUser.Visible = False
                LblUserName.Visible = False
            Else
                LblUserName.Text = "" & MyDataset.Tables(0).Rows(0).Item("Username")
                LabelUser.Visible = True
                If LblUserName.Text <> "" Then
                    LblUserName.Visible = True
                End If
            End If

        End If

        MyDataset.Dispose()
    End Sub

    Private Sub CaricaDettaglioSedi(IdEnteSedeAttuazione)
        Dim strsql As String
        Dim MyDataset As DataSet

        dgSediAssegnate.CurrentPageIndex = 0

        If Not UtenzeSPID Then

            strsql = " Select DISTINCT"
            strsql &= " spd.IdEnteSedeAttuazioneGestita as IdEnteSedeAttuazione, es.denominazione as Denominazionesede,"
            strsql &= " es.indirizzo  + ' ' + es.civico as indirizzo,c.denominazione as comune, spd.idsedepassworddelega "
            'strsql &= " ,case spd.IdEnteSedeAttuazioneGestita  "
            'strsql &= " WHEN " & IdEnteSedeAttuazione & " THEN '' "
            'strsql &= " else '<img src=''images/canc_small.png'' title=''Cancella Sede'' border=''0&gt'' style=''cursor:hand'' alt=''Cancella Sede Gestita''></img>' end as CancellaUtenza"

            strsql &= " FROM SediPassword sp"
            strsql &= " inner join sedipasswordDelega spd on sp.idSedepassword= spd.idsedepassword"
            strsql &= " inner join  entisediattuazioni ESA  on  spd.IdEnteSedeAttuazioneGestita=esa.IdEnteSedeAttuazione"
            strsql &= " inner join entisedi es on esa.IDEnteSede = es.IDEnteSede  "
            strsql &= " inner join comuni c on c.idcomune = es.idcomune  "
            strsql &= " WHERE esa.identecapofila  =" & Session("IdEnte") & " And sp.IdEnteSedeAttuazione = " & IdEnteSedeAttuazione & " and LEFT(sp.Username,3)!='EDS'"
        Else

            strsql = " Select DISTINCT"
            strsql &= " spd.IdEnteSedeAttuazioneGestita as IdEnteSedeAttuazione, es.denominazione as Denominazionesede,"
            strsql &= " es.indirizzo  + ' ' + es.civico as indirizzo,c.denominazione as comune, 0 idsedepassworddelega" ' idsedepassworddelega non usata per UtenzeSedeSPID
            strsql &= " FROM SediPassword sp"
            strsql &= " inner join sedipasswordDelega spd on sp.idSedepassword= spd.idsedepassword"
            strsql &= " inner join  entisediattuazioni ESA  on  spd.IdEnteSedeAttuazioneGestita=esa.IdEnteSedeAttuazione"
            strsql &= " inner join entisedi es on esa.IDEnteSede = es.IDEnteSede  "
            strsql &= " inner join comuni c on c.idcomune = es.idcomune  "
            strsql &= " WHERE esa.identecapofila  =" & Session("IdEnte") & " And sp.IdEnteSedeAttuazione = " & IdEnteSedeAttuazione & " and LEFT(sp.Username,3)='EDS'"

        End If
        MyDataset = ClsServer.DataSetGenerico(strsql, Session("conn"))
        Session("MyDataset") = MyDataset
        dgSediAssegnate.DataSource = Session("MyDataset")
        dgSediAssegnate.DataBind()

        MyDataset.Dispose()
    End Sub

    Private Sub dgSediAssegnate_ItemCommand(source As Object, e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgSediAssegnate.ItemCommand
        Dim strsql As String
        Dim blnCanc As Boolean
        Dim sqlCmd As SqlClient.SqlCommand
        Select Case e.CommandName
            Case "Cancella"

                If Not UtenzeSPID Then
                    Dim rstInfo As SqlClient.SqlDataReader
                    strsql = "Select * from Sedipassword where Username='" & LblUserName.Text & "' and IDEnteSedeAttuazione =" & dgSediAssegnate.Items(e.Item.ItemIndex).Cells(0).Text
                    rstInfo = ClsServer.CreaDatareader(strsql, Session("conn"))
                    blnCanc = rstInfo.HasRows

                    If Not rstInfo Is Nothing Then
                        rstInfo.Close()
                        rstInfo = Nothing

                    End If
                Else
                    blnCanc = False
                End If

                If blnCanc = False Then 'cancello la sede gestita

                    If Not UtenzeSPID Then
                        strsql = "Delete from sedipassworddelega where idsedepassworddelega = " & dgSediAssegnate.Items(e.Item.ItemIndex).Cells(5).Text
                    Else
                        'cancello tutte le deleghe (ma non quelle per le utenze principali)
                        strsql = "delete SediPasswordDelega"
                        strsql += " from SediPasswordDelega inner join SediPassword on SediPasswordDelega.IdSedePassword=SediPassword.IDSedePassword"
                        strsql += " where SediPasswordDelega.IdEnteSedeAttuazioneGestita = " & dgSediAssegnate.Items(e.Item.ItemIndex).Cells(0).Text & " And SediPassword.IDEnteSedeAttuazione <> " & dgSediAssegnate.Items(e.Item.ItemIndex).Cells(0).Text
                    End If
                    sqlCmd = ClsServer.EseguiSqlClient(strsql, Session("conn"))
                    lblmessaggio.Text = "Rimossa sede gestita."

                    If Not UtenzeSPID Then
                        CaricaDettaglioSedi(Request.QueryString("IdEnteSedeAttuazione"))
                    Else
                        lblMsgUtenzaSPID.Text = ""
                        LoadMaschera(Request.QueryString("IdEnteSedeAttuazione"))
                    End If
                Else
                    lblMsgUtenzaSPID.Text = ""
                    lblmessaggio.Text = "Impossibile rimuovere la sede."
                End If


        End Select
    End Sub

    Protected Sub cmdAggiungi_Click(sender As Object, e As EventArgs) Handles cmdAggiungi.Click
        Dim strsql As String
        Dim sqlCmd As SqlClient.SqlCommand
        Dim intIdSedePassword As Integer
        Dim intAbilitato As Integer
        lblmessaggio.Text = ""

        If TxtCodiceSede.Text = "" Then
            lblmessaggio.Text = "E' necessario indicate il Codice Sede."
            Exit Sub
        End If
        InformazioniUtenza(Request.QueryString("IdEnteSedeAttuazione"), intIdSedePassword, intAbilitato)

        If Not UtenzeSPID Then
            lblmessaggio.Text = AggiungiSedeDelega(intIdSedePassword, TxtCodiceSede.Text)
        Else
            lblmessaggio.Text = AggiungiSedeDelegaSPID(TxtCodiceSede.Text)
        End If
        lblMsgUtenzaSPID.Text = ""
        LoadMaschera(Request.QueryString("IdEnteSedeAttuazione"))

    End Sub
    Private Function GeneraUserNameSede(idEnteSedeAttuazione As String) As String
        '*** Creato da Simona Cordella
        '*** il 09/03/2015
        '*** Genero la username della sede 
        Dim strCodiceUtenzaSede As String
        Dim LenSede As Integer = Len(idEnteSedeAttuazione) + 1
        Dim Zeri As String
        Dim i As Integer

        For i = LenSede To 9
            Zeri = Zeri & "0"
        Next
        strCodiceUtenzaSede = "E" & Zeri & idEnteSedeAttuazione
        Return strCodiceUtenzaSede
    End Function

    Protected Sub cmdAttiva_Click(sender As Object, e As EventArgs) Handles cmdAttiva.Click
        LblMsgUtenza.Text = ""
        If TxtEmail.Text = "" Then
            LblMsgUtenza.Text = "E' necessario indicare l'email."
            Exit Sub
        End If

        Dim strCodiceUtenzaSede As String = GeneraUserNameSede(Request.QueryString("IdEnteSedeAttuazione"))
        Dim strPasswordUtenzaCriptata As String = ClsUtility.CreatePSW(ClsUtility.NuovaPasswordADC)
        LblMsgUtenza.Text = InsertCreazioneUtenzaSede(Request.QueryString("IdEnteSedeAttuazione"), strCodiceUtenzaSede, strPasswordUtenzaCriptata)

        StoreInviaEmailSedi(Request.QueryString("IdEnteSedeAttuazione"), 8)
        LoadMaschera(Request.QueryString("IdEnteSedeAttuazione"))
    End Sub

    Private Function InsertCreazioneUtenzaSede(IdEnteSedeAttuazione As Integer, UsernameUtenza As String, PasswordUtenza As String) As String
        'REALIZZATA DA: SIMONA CORDELLA 
        'DATA REALIZZAZIONE:  09/03/2015
        'FUNZIONALITA': RICHIAMO STORE PER L'INSERIMENTO DELL'UTENZA

        Dim sqlCMD As New SqlClient.SqlCommand
        Dim strNomeStore As String = "[SP_UTENZA_SEDE_ATTIVA]"

        Try
            sqlCMD = New SqlClient.SqlCommand(strNomeStore, CType(Session("conn"), SqlClient.SqlConnection))
            sqlCMD.CommandType = CommandType.StoredProcedure

            sqlCMD.Parameters.Add("@IDEnteSedeAttuazione", SqlDbType.Int).Value = IdEnteSedeAttuazione
            sqlCMD.Parameters.Add("@UsernameUtenza", SqlDbType.VarChar).Value = UsernameUtenza
            sqlCMD.Parameters.Add("@Password", SqlDbType.VarChar).Value = PasswordUtenza
            sqlCMD.Parameters.Add("@Email", SqlDbType.VarChar).Value = TxtEmail.Text.Replace("'", "''")

            Dim sparam1 As SqlClient.SqlParameter
            sparam1 = New SqlClient.SqlParameter
            sparam1.ParameterName = "@Esito"
            sparam1.Size = 100
            sparam1.SqlDbType = SqlDbType.NVarChar
            sparam1.Direction = ParameterDirection.Output
            sqlCMD.Parameters.Add(sparam1)

            Dim sparam2 As SqlClient.SqlParameter
            sparam2 = New SqlClient.SqlParameter
            sparam2.ParameterName = "@Messaggio"
            sparam2.Size = 100
            sparam2.SqlDbType = SqlDbType.NVarChar
            sparam2.Direction = ParameterDirection.Output
            sqlCMD.Parameters.Add(sparam2)


            sqlCMD.ExecuteScalar()
            Dim str As String
            str = sqlCMD.Parameters("@Messaggio").Value

            Return str

        Catch ex As Exception
            Response.Write(ex.Message.ToString())
            Exit Function
        End Try
    End Function

    Private Function AggiungiSedeDelega(IdSedePassword As Integer, IdEnteSedeAttuazione As Integer) As String
        'REALIZZATA DA: SIMONA CORDELLA 
        'DATA REALIZZAZIONE:  10/03/2015
        'FUNZIONALITA': RICHIAMO STORE PER AGGIUNGERE SEDE DELEGA
        Dim sqlCMD As New SqlClient.SqlCommand
        Dim strNomeStore As String = "[SP_UTENZA_SEDE_AGGIUNGI_DELEGA]"


        Try
            sqlCMD = New SqlClient.SqlCommand(strNomeStore, CType(Session("conn"), SqlClient.SqlConnection))
            sqlCMD.CommandType = CommandType.StoredProcedure

            sqlCMD.Parameters.Add("@IdSedePassword", SqlDbType.Int).Value = IdSedePassword
            sqlCMD.Parameters.Add("@IdEnteSedeAttuazioneGestita", SqlDbType.Int).Value = IdEnteSedeAttuazione
            sqlCMD.Parameters.Add("@IdEnte", SqlDbType.Int).Value = Session("IDEnte")

            Dim sparam1 As SqlClient.SqlParameter
            sparam1 = New SqlClient.SqlParameter
            sparam1.ParameterName = "@Esito"
            sparam1.Size = 100
            sparam1.SqlDbType = SqlDbType.NVarChar
            sparam1.Direction = ParameterDirection.Output
            sqlCMD.Parameters.Add(sparam1)

            Dim sparam2 As SqlClient.SqlParameter
            sparam2 = New SqlClient.SqlParameter
            sparam2.ParameterName = "@Messaggio"
            sparam2.Size = 100
            sparam2.SqlDbType = SqlDbType.NVarChar
            sparam2.Direction = ParameterDirection.Output
            sqlCMD.Parameters.Add(sparam2)


            sqlCMD.ExecuteScalar()
            Dim str As String
            str = sqlCMD.Parameters("@Messaggio").Value

            Return str

        Catch ex As Exception
            Response.Write(ex.Message.ToString())
            Exit Function
        End Try
    End Function

    Protected Sub cmdDisattiva_Click(sender As Object, e As EventArgs) Handles cmdDisattiva.Click
        Dim strsql As String
        Dim sqlCmd As SqlClient.SqlCommand

        LblMsgUtenza.Text = ""
        strsql = "Update Sedipassword set Abilitato= 0 where IDEnteSedeAttuazione =" & Request.QueryString("IdEnteSedeAttuazione") & " "
        sqlCmd = ClsServer.EseguiSqlClient(strsql, Session("conn"))
        LblMsgUtenza.Text = "Utenza Disabilitata"

        LoadMaschera(Request.QueryString("IdEnteSedeAttuazione"))
    End Sub

    Private Sub InformazioniUtenza(ByVal IdEnteSedeAttuazione As Integer, ByRef IdSedePassword As Integer, ByRef Abilitato As Integer)
        Dim blnStatoUtenza As Boolean
        Dim Strsql As String

        Dim rstInfo As SqlClient.SqlDataReader

        Strsql = "Select top 1 * from Sedipassword where IDEnteSedeAttuazione =" & IdEnteSedeAttuazione & ""
        If UtenzeSPID Then
            Strsql += " and left(Username,3)='EDS'"
        Else
            Strsql += " and left(Username,3)!='EDS'"
        End If

        rstInfo = ClsServer.CreaDatareader(Strsql, Session("conn"))
        If rstInfo.HasRows = True Then
            rstInfo.Read()
            IdSedePassword = rstInfo("IdSedePassword")
            Abilitato = rstInfo("Abilitato")
        End If
        rstInfo.Close()
        rstInfo = Nothing

    End Sub

    Private Sub VerificaStatoUtenza(ByVal IdEnteSedeAttuazione As Integer)
        'verifico se l'utenza è gia esistenze
        'si -->disattivo pulsante attiva utenza
        'no -->disattivo pulsante disattiva,inoltra e aggiungi sede
        Dim blnStatoUtenza As Boolean
        Dim Strsql As String
        Dim rstInfo As SqlClient.SqlDataReader

        cmdAttiva.Visible = False
        cmdDisattiva.Visible = False
        cmdInoltroPW.Visible = False
        cmdAggiungi.Visible = False

        Strsql = "Select top 1 * from Sedipassword where IDEnteSedeAttuazione =" & IdEnteSedeAttuazione & ""
        If UtenzeSPID Then
            Strsql += " and left(Username,3)='EDS'"
        Else
            Strsql += " and left(Username,3)!='EDS'"
        End If

        rstInfo = ClsServer.CreaDatareader(Strsql, Session("conn"))
        If rstInfo.HasRows = True Then
            rstInfo.Read()

            If rstInfo("Abilitato") = True Then 'utenza attiva
                cmdDisattiva.Visible = True
                cmdInoltroPW.Visible = True
                cmdAggiungi.Visible = True
            Else 'utenza disabilitata
                cmdAttiva.Visible = True
            End If
        Else
            cmdAttiva.Visible = True
        End If
        rstInfo.Close()
        rstInfo = Nothing

    End Sub

    Sub LoadMaschera(ByVal IdEnteSedeAttuazione As Integer)
        VerificaStatoUtenza(IdEnteSedeAttuazione)
        CaricaMaschera(IdEnteSedeAttuazione)
        CaricaDettaglioSedi(IdEnteSedeAttuazione)
        If UtenzeSPID Then
            CaricaUtenzeSede(IdEnteSedeAttuazione)
            divUtenzaSede.Visible = False
            divUtenzeSede.Visible = True
        Else
            divUtenzaSede.Visible = True
            divUtenzeSede.Visible = False
        End If
    End Sub

    Private Function StoreInviaEmailSedi(ByVal IdEnteSedeAttuazione As String, ByVal intTipologiaRichiesta As Integer)
        'REALIZZATA DA: SIMONA CORDELLA 
        'DATA REALIZZAZIONE:  13/03/2015
        'FUNZIONALITA': RICHIAMO STORE PER l'INVIO EMAIL DI NUOVA UTENZA E INOLTRO UTENZA

        Dim sqlCMD As New SqlClient.SqlCommand
        Dim strNomeStore As String = "[SP_MAIL_ACCOUNT_SEDI]"

        Try
            sqlCMD = New SqlClient.SqlCommand(strNomeStore, CType(Session("conn"), SqlClient.SqlConnection))
            sqlCMD.CommandType = CommandType.StoredProcedure

            sqlCMD.Parameters.Add("@IdEnteSedeAttuazione", SqlDbType.VarChar).Value = IdEnteSedeAttuazione
            sqlCMD.Parameters.Add("@TIPO", SqlDbType.Int).Value = intTipologiaRichiesta

            Dim sparam1 As SqlClient.SqlParameter
            sparam1 = New SqlClient.SqlParameter
            sparam1.ParameterName = "@Esito"
            sparam1.Size = 100
            sparam1.SqlDbType = SqlDbType.NVarChar
            sparam1.Direction = ParameterDirection.Output
            sqlCMD.Parameters.Add(sparam1)

            sqlCMD.ExecuteScalar()

        Catch ex As Exception
            Response.Write(ex.Message.ToString())
            Exit Function
        End Try
    End Function

    Protected Sub cmdInoltroPW_Click(sender As Object, e As EventArgs) Handles cmdInoltroPW.Click
        LblMsgUtenza.Text = ""
        If TxtEmail.Text = "" Then
            LblMsgUtenza.Text = "E' necessario indicare l'email."
            Exit Sub
        End If

        Dim strCodiceUtenzaSede As String = GeneraUserNameSede(Request.QueryString("IdEnteSedeAttuazione"))
        Dim strPasswordUtenzaCriptata As String = ClsUtility.CreatePSW(ClsUtility.NuovaPasswordADC)
        LblMsgUtenza.Text = InsertCreazioneUtenzaSede(Request.QueryString("IdEnteSedeAttuazione"), strCodiceUtenzaSede, strPasswordUtenzaCriptata)

        StoreInviaEmailSedi(Request.QueryString("IdEnteSedeAttuazione"), 9)
        LblMsgUtenza.Text = "Inoltro credenziali."
        LoadMaschera(Request.QueryString("IdEnteSedeAttuazione"))
    End Sub


    Private Sub dgSediAssegnate_PageIndexChanged(source As Object, e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgSediAssegnate.PageIndexChanged
        dgSediAssegnate.CurrentPageIndex = e.NewPageIndex
        dgSediAssegnate.DataSource = Session("MyDataset")
        dgSediAssegnate.DataBind()
        dgSediAssegnate.SelectedIndex = -1
    End Sub
    Private Function VerificaAbilitazione(ByVal Utente As String, ByVal Conn As SqlClient.SqlConnection) As Boolean

        '** Verifico se l'utenza è abilitata alla visibilità del flag che consente il caricamento della programamzione con volontari e progetti terminati
        '** profilio menu creato appositamente per le richieste regionali
        Dim strSql As String
        Dim dtrgenerico As SqlClient.SqlDataReader
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If

        'Verifica menu sicurezza su funzione accredita
        strSql = "SELECT AssociaUtenteGruppo.username, VociMenu.IdVoceMenu, VociMenu.VoceMenu, VociMenu.ImmaginePassiva, VociMenu.ImmagineAttiva, VociMenu.Link," & _
                 " VociMenu.IdVoceMenuPadre" & _
                 " FROM VociMenu " & _
                 " INNER JOIN AbilitazioniProfiliVociMenu ON VociMenu.IdVoceMenu = AbilitazioniProfiliVociMenu.IdVoceMenu" & _
                 " INNER JOIN Profili ON AbilitazioniProfiliVociMenu.IdProfilo = Profili.IdProfilo" & _
                 " INNER JOIN AssociaUtenteGruppo ON Profili.IdProfilo = AssociaUtenteGruppo.IdProfilo" & _
                 " LEFT JOIN  RestrizioniUtenteVociMenu ON AssociaUtenteGruppo.UserName = RestrizioniUtenteVociMenu.UserName and RestrizioniUtenteVociMenu.IdVoceMenu=VociMenu.IdVoceMenu" & _
                 " WHERE VociMenu.descrizione = 'Gestione Utenze Sedi'" & _
                 " AND AssociaUtenteGruppo.username ='" & Utente & "' and (RestrizioniUtenteVociMenu.TipoRestrizione <> 0 or RestrizioniUtenteVociMenu.TipoRestrizione is null)"
        dtrgenerico = ClsServer.CreaDatareader(strSql, Conn)

        VerificaAbilitazione = dtrgenerico.Read()
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
        Return VerificaAbilitazione
    End Function

    Private Sub CaricaUtenzeSede(IdEnteSedeAttuazione)
        Dim strsql As String
        Dim MyDataset As DataSet

        dtgUtenzeSede.CurrentPageIndex = 0

        strsql = "SELECT IDSedePassword, CodiceFiscale, Cognome, Nome, case Abilitato when 1 then 'Attiva' else 'Non Attiva' end as Stato FROM SediPassword"
        strsql += " WHERE LEFT(Username,3)='EDS' AND IDEnteSedeAttuazione=" & IdEnteSedeAttuazione
        strsql += " ORDER BY Abilitato desc, CodiceFiscale"

        MyDataset = ClsServer.DataSetGenerico(strsql, Session("conn"))
        Session("DtUtenzeSede") = MyDataset
        dtgUtenzeSede.DataSource = Session("DtUtenzeSede")
        dtgUtenzeSede.DataBind()
    End Sub

    Private Sub dtgUtenzeSede_PageIndexChanged(source As Object, e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dtgUtenzeSede.PageIndexChanged
        dtgUtenzeSede.CurrentPageIndex = e.NewPageIndex
        dtgUtenzeSede.DataSource = Session("DtUtenzeSede")
        dtgUtenzeSede.DataBind()
        dtgUtenzeSede.SelectedIndex = -1
    End Sub

    Protected Sub cmdInserisci_Click(sender As Object, e As EventArgs) Handles cmdInserisci.Click
        lblMsgUtenzaSPID.Text = ""
        If TxtCodiceFiscale.Text = "" Then
            lblMsgUtenzaSPID.Text = "E' necessario indicare il Codice Fiscale."
            Exit Sub
        End If
        Dim regex = New Regex("^(?:[A-Z][AEIOU][AEIOUX]|[B-DF-HJ-NP-TV-Z]{2}[A-Z]){2}(?:[\dLMNP-V]{2}(?:[A-EHLMPR-T](?:[04LQ][1-9MNP-V]|[15MR][\dLMNP-V]|[26NS][0-8LMNP-U])|[DHPS][37PT][0L]|[ACELMRT][37PT][01LM]|[AC-EHLMPR-T][26NS][9V])|(?:[02468LNQSU][048LQU]|[13579MPRTV][26NS])B[26NS][9V])(?:[A-MZ][1-9MNP-V][\dLMNP-V]{2}|[A-M][0L](?:[1-9MNP-V][\dLMNP-V]|[0L][1-9MNP-V]))[A-Z]$")
        If regex.Match(TxtCodiceFiscale.Text.ToUpper).Success = False Then
            lblMsgUtenzaSPID.Text = "Codice Fiscale non corretto."
            Exit Sub
        End If
        If TxtCognome.Text = "" Then
            lblMsgUtenzaSPID.Text = "E' necessario indicare il Cognome."
            Exit Sub
        End If
        If TxtNome.Text = "" Then
            lblMsgUtenzaSPID.Text = "E' necessario indicare il Nome."
            Exit Sub
        End If

        lblMsgUtenzaSPID.Text = InsertCreazioneUtenzaSpidSede(Request.QueryString("IdEnteSedeAttuazione"), TxtCognome.Text, TxtNome.Text, TxtCodiceFiscale.Text.ToUpper)
        lblmessaggio.Text = ""
        LoadMaschera(Request.QueryString("IdEnteSedeAttuazione"))
    End Sub

    Private Function InsertCreazioneUtenzaSpidSede(IdEnteSedeAttuazione As Integer, Cognome As String, Nome As String, CodiceFiscale As String) As String

        Dim sqlCMD As New SqlClient.SqlCommand
        Dim strNomeStore As String = "[SP_UTENZA_SPID_SEDE_ATTIVA]"

        Try
            sqlCMD = New SqlClient.SqlCommand(strNomeStore, CType(Session("conn"), SqlClient.SqlConnection))
            sqlCMD.CommandType = CommandType.StoredProcedure

            sqlCMD.Parameters.Add("@IDEnteSedeAttuazione", SqlDbType.Int).Value = IdEnteSedeAttuazione

            sqlCMD.Parameters.Add("@Cognome", SqlDbType.VarChar).Value = Cognome.Replace("'", "''")
            sqlCMD.Parameters.Add("@Nome", SqlDbType.VarChar).Value = Nome.Replace("'", "''")
            sqlCMD.Parameters.Add("@CodiceFiscale", SqlDbType.VarChar).Value = CodiceFiscale.Replace("'", "''")

            Dim sparam1 As SqlClient.SqlParameter
            sparam1 = New SqlClient.SqlParameter
            sparam1.ParameterName = "@Esito"
            sparam1.Size = 100
            sparam1.SqlDbType = SqlDbType.NVarChar
            sparam1.Direction = ParameterDirection.Output
            sqlCMD.Parameters.Add(sparam1)

            Dim sparam2 As SqlClient.SqlParameter
            sparam2 = New SqlClient.SqlParameter
            sparam2.ParameterName = "@Messaggio"
            sparam2.Size = 100
            sparam2.SqlDbType = SqlDbType.NVarChar
            sparam2.Direction = ParameterDirection.Output
            sqlCMD.Parameters.Add(sparam2)


            sqlCMD.ExecuteScalar()
            Dim str As String
            str = sqlCMD.Parameters("@Messaggio").Value

            If sqlCMD.Parameters("@Esito").Value = "POSITIVO" Then
                TxtCodiceFiscale.Text = ""
                TxtCognome.Text = ""
                TxtNome.Text = ""
            End If

            Return str

        Catch ex As Exception
            Response.Write(ex.Message.ToString())
            Exit Function
        End Try
    End Function

    Private Sub dtgUtenzeSede_ItemCommand(source As Object, e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dtgUtenzeSede.ItemCommand
        Dim strsql As String
        Dim sqlCmd As SqlClient.SqlCommand

        Select Case e.CommandName
            Case "CambiaStato"

                strsql = "Update SediPassword set Abilitato = Abilitato ^ 1 where IdSedePassword=" & dtgUtenzeSede.Items(e.Item.ItemIndex).Cells(0).Text
                sqlCmd = ClsServer.EseguiSqlClient(strsql, Session("conn"))
                lblMsgUtenzaSPID.Text = "Cambio stato effettuato."
                lblmessaggio.Text = ""
                CaricaUtenzeSede(Request.QueryString("IdEnteSedeAttuazione"))
        End Select
    End Sub

    Private Function UtenzeSedeSPID() As Boolean
        Dim _ret As Boolean = False
        Dim strsql As String
        Dim dtrGen As SqlClient.SqlDataReader
        strsql = "SELECT VALORE  FROM Configurazioni where Parametro='UTENZE_SEDE_SPID'"
        If Not dtrGen Is Nothing Then
            dtrGen.Close()
            dtrGen = Nothing
        End If
        dtrGen = ClsServer.CreaDatareader(strsql, Session("conn"))
        dtrGen.Read()
        If dtrGen.HasRows Then
            If dtrGen("Valore") = "1" Then _ret = True
        End If
        If Not dtrGen Is Nothing Then
            dtrGen.Close()
            dtrGen = Nothing
        End If
        Return _ret
    End Function

    Private Function AggiungiSedeDelegaSPID(IdEnteSedeAttuazioneDelega As Integer) As String

        Dim sqlCMD As New SqlClient.SqlCommand
        Dim strNomeStore As String = "[SP_UTENZA_SPID_SEDE_AGGIUNGI_DELEGA]"


        Try
            sqlCMD = New SqlClient.SqlCommand(strNomeStore, CType(Session("conn"), SqlClient.SqlConnection))
            sqlCMD.CommandType = CommandType.StoredProcedure

            sqlCMD.Parameters.Add("@IdEnteSedeAttuazioneGestita", SqlDbType.Int).Value = Request.QueryString("IdEnteSedeAttuazione")
            sqlCMD.Parameters.Add("@IdEnteSedeAttuazioneGestitaDelega", SqlDbType.Int).Value = IdEnteSedeAttuazioneDelega
            sqlCMD.Parameters.Add("@IdEnte", SqlDbType.Int).Value = Session("IDEnte")

            Dim sparam1 As SqlClient.SqlParameter
            sparam1 = New SqlClient.SqlParameter
            sparam1.ParameterName = "@Esito"
            sparam1.Size = 100
            sparam1.SqlDbType = SqlDbType.NVarChar
            sparam1.Direction = ParameterDirection.Output
            sqlCMD.Parameters.Add(sparam1)

            Dim sparam2 As SqlClient.SqlParameter
            sparam2 = New SqlClient.SqlParameter
            sparam2.ParameterName = "@Messaggio"
            sparam2.Size = 100
            sparam2.SqlDbType = SqlDbType.NVarChar
            sparam2.Direction = ParameterDirection.Output
            sqlCMD.Parameters.Add(sparam2)


            sqlCMD.ExecuteScalar()
            Dim str As String
            str = sqlCMD.Parameters("@Messaggio").Value

            Return str

        Catch ex As Exception
            Response.Write(ex.Message.ToString())
            Exit Function
        End Try
    End Function

    Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Response.Redirect("WfrmRicercaUtenzeSedi.aspx")
    End Sub
End Class