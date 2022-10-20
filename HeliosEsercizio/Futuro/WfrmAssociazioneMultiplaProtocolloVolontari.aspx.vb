Imports System.IO
Public Class WfrmAssociazioneMultiplaProtocolloVolontari
    Inherits System.Web.UI.Page
    Dim SIGED As clsSiged
    Dim strsql As String
    Dim cmdCommand As SqlClient.SqlCommand
    Dim dtrGenerico As SqlClient.SqlDataReader
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Session("LogIn") Is Nothing Then
            If Session("LogIn") = False Then 'verifico validità log-in
                Response.Redirect("LogOn.aspx")
            End If
        Else
            Response.Redirect("LogOn.aspx")
        End If

        If Page.IsPostBack = False Then
            Dim idEnteSede As String
            idEnteSede = Request.QueryString("idEnteSede")
            LoadVolontari(Request.QueryString("IdAttivitaSedeAssegnazione"), Request.QueryString("IdAttivita"), ddlFiltro.SelectedValue)
            strsql = "select CodiceEnte,Titolo FROM attività WHERE IDAttività =" & Request.QueryString("IdAttivita")
            If Not dtrGenerico Is Nothing Then
                dtrGenerico.Close()
                dtrGenerico = Nothing
            End If
            



            dtrGenerico = ClsServer.CreaDatareader(strsql, Session("conn"))
            If dtrGenerico.HasRows = True Then
                dtrGenerico.Read()
                lblCodProg.Text = dtrGenerico("CodiceEnte")
                lblTitoloprog.Text = dtrGenerico("Titolo")
                If Not dtrGenerico Is Nothing Then
                    dtrGenerico.Close()
                    dtrGenerico = Nothing
                End If
            End If
        End If
    End Sub
    Private Sub LoadVolontari(ByVal IdAttivitaSedeAssegnazione As Integer, ByVal IdAttivita As Integer, ByVal TipoFiltro As Integer)
        dtgVolontari.DataSource = LoadStoreVolontari(IdAttivitaSedeAssegnazione, IdAttivita, TipoFiltro)
        dtgVolontari.DataBind()
    End Sub

    Private Function LoadStoreVolontari(ByVal IdAttivitaSedeAssegnazione As Integer, ByVal IdAttivita As Integer, ByVal TipoFiltro As Integer) As DataSet
        Dim sqlDAP As New SqlClient.SqlDataAdapter
        Dim dataSet As New DataSet
        Dim strNomeStore As String = "[SP_RITORNA_VOLONTARI_AVVIATI]"

        Try

            sqlDAP = New SqlClient.SqlDataAdapter(strNomeStore, CType(Session("conn"), SqlClient.SqlConnection))
            sqlDAP.SelectCommand.CommandType = CommandType.StoredProcedure
            sqlDAP.SelectCommand.Parameters.Add("@IdAttivitaSedeAssegnazione", SqlDbType.Int).Value = IdAttivitaSedeAssegnazione
            sqlDAP.SelectCommand.Parameters.Add("@IdAttivita", SqlDbType.Int).Value = IdAttivita
            sqlDAP.SelectCommand.Parameters.Add("@TipoFiltro", SqlDbType.Int).Value = TipoFiltro
            sqlDAP.Fill(dataSet)

            Return dataSet

        Catch ex As Exception
            Response.Write(ex.Message.ToString())
            Exit Function
        End Try
    End Function

    Private Sub ddlFiltro_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlFiltro.SelectedIndexChanged
        If ddlFiltro.SelectedItem.Text <> "" Then
            lblmsg.Text = ""
            LoadVolontari(Request.QueryString("IdAttivitaSedeAssegnazione"), Request.QueryString("IdAttivita"), ddlFiltro.SelectedValue)
        Else
            ddlFiltro.SelectedValue = 1
        End If
    End Sub
    Private Sub AggiungiProtocolloEsistente(ByVal IdProtocollo As String, ByVal Anno As String, ByVal IdFascicolo As String)
        '*** creata da simona cordella il 28/11/2013
        '*** consento di inserire un protocollo esistente nel fascicolo

        Dim wsOUT As String

        Dim strIdFasc As String 'idfascicolo
        Dim IdFascSIGED As String 'idfascicolo
        Dim IdProtocolloSIGED As String
        Dim sEsito As String
        Dim sNumeroDoc As String
        Dim dr As DataRow
        Dim riga As Integer

        Dim strNome As String
        Dim strCognome As String
        Dim strSQL As String
        Dim dsUser As DataSet
        lblmsg.Text = ""

        strSQL = "Select Nome, Cognome From UtentiUNSC Where UserName='" & Session("Utente") & "'"
        dsUser = ClsServer.DataSetGenerico(strSQL, Session("conn"))

        If dsUser.Tables(0).Rows.Count <> 0 Then
            strNome = dsUser.Tables(0).Rows(0).Item("Nome")
            strCognome = dsUser.Tables(0).Rows(0).Item("Cognome")
        End If

        SIGED = New clsSiged("", strNome, strCognome)
        If SIGED.Codice_Esito <> 0 Then
            lblmsg.Text = SIGED.Esito
            Exit Sub
        End If

        IdProtocolloSIGED = SIGED.SIGED_CodiceProtocolloCompleto(Anno, IdProtocollo)

        strIdFasc = Request.QueryString("NumeroFascicolo") 'idfascicolo
        IdFascSIGED = SIGED.SIGED_IdFascicoloCompleto(strIdFasc)

        wsOUT = SIGED.SIGED_CreaCollegamentoFascicolo(IdFascSIGED, IdProtocolloSIGED)


        If SIGED.SIGED_Codice_Esito(wsOUT) = 0 Then
            lblmsg.Text = "Aggiornamento eseguito"
            'CaricaGliglia()
        Else
            'modificato il 11/01/2013 da simona cordella
            'verifico se il protocollo è già presente nel fascicolo
            If InStr(SIGED.SIGED_Esito(wsOUT), "PRIMARY KEY") <> 0 Then
                lblmsg.Text = "Protocollo già esistente."
            Else
                lblmsg.Text = Mid(wsOUT, 8, Len(wsOUT))
            End If
        End If
        SIGED.SIGED_Chiudi_Autenticazione(strNome, strCognome)
    End Sub

    Private Function ControllaCheck() As Boolean
        
        'FUNZIONALITA': CONTROLLO SELEZIONE DELLA GRIGLIA DELLE TIPOLOGIE
        Dim item As DataGridItem
        ControllaCheck = False
        For Each item In dtgVolontari.Items
            Dim check As CheckBox = DirectCast(item.FindControl("chkSelVol"), CheckBox)
            If check.Checked = True Then
                ControllaCheck = True
                Exit For
            End If
        Next
        If ControllaCheck = False Then
            lblmsg.Visible = True
            lblmsg.Text = "Attenzione, non è stato selezionato nessun volontario."
        End If
    End Function

    Private Function MemorizzaIdEntità(ByVal chk As CheckBox) As String
        Dim sIdEntità As String
        Dim i As Integer
        'FUNZIONALITA': CONTROLLO SELEZIONE DELLA GRIGLIA DELLE TIPOLOGIE
        Dim item As DataGridItem
        MemorizzaIdEntità = False
        For i = 0 To dtgVolontari.Items.Count - 1
            chk = dtgVolontari.Items(i).FindControl("chk")
            If chk.Checked = True Then
                sIdEntità = sIdEntità & dtgVolontari.Items(i).Cells(7).Text & "#"
            End If
        Next
        MemorizzaIdEntità = sIdEntità
    End Function


    Protected Sub cmdApplica_Click(sender As Object, e As EventArgs) Handles cmdApplica.Click
        Dim i As Integer = 0
        Dim chkSelVol As CheckBox

        Dim wsOUT As String
        Dim wsProt As WS_SIGeD.MULTI_PROTOCOLLO
        Dim strIdFasc As String 'idfascicolo
        Dim IdFascSIGED As String 'idfascicolo
        Dim IdProtocolloSIGED As String

        Dim strNome As String
        Dim strCognome As String
        Dim strSQL As String
        Dim dsUser As DataSet
        Dim intEsito As Integer = 0
        Dim intConta As Integer = 0

        Dim blnLog As Boolean = False
        lblmsg.Text = ""

        If TxtAnno.Text = "" Then
            lblmsg.Text = "E' necessario indicare l'Anno del protocollo."
            Exit Sub
        End If
        If TxtNumProtocollo.Text = "" Then
            lblmsg.Text = "E' necessario indicare il Numero di protocollo."
            Exit Sub
        End If

        'controllo selezione della griglia delle tipologie
        If ControllaCheck() = False Then Exit Sub

        '*** modificato il 13/06/2017 da simona cordella
        'viene spostata anche nel WSINTERNO e verra esaguita dopo la geenrazione del fascicolo
        'verifico se per i volontari selezionati hanno i fascicoli (solo in questo caso eseguo l'associzione, altrimenti richiamo metodo wsinterno)
        strSQL = "Select Nome, Cognome From UtentiUNSC Where UserName='" & Session("Utente") & "'"
        dsUser = ClsServer.DataSetGenerico(strSQL, Session("conn"))

        If dsUser.Tables(0).Rows.Count <> 0 Then
            strNome = dsUser.Tables(0).Rows(0).Item("Nome")
            strCognome = dsUser.Tables(0).Rows(0).Item("Cognome")
        End If

        SIGED = New clsSiged("", strNome, strCognome)
        If SIGED.Codice_Esito <> 0 Then
            lblmsg.Text = SIGED.Esito
            Exit Sub
        End If
        IdProtocolloSIGED = SIGED.SIGED_CodiceProtocolloCompleto(TxtAnno.Text, TxtNumProtocollo.Text)

        wsProt = SIGED.SIGED_Ricerca_Protocolli(TxtAnno.Text, TxtNumProtocollo.Text, "", "", "", "", "", "", "")

        If SIGED.SIGED_Esito(wsProt.ESITO) = 0 Then
            lblmsg.Text = "Il protocollo indicato è inesistente."
            SIGED.SIGED_Chiudi_Autenticazione(strNome, strCognome)
            Exit Sub
        End If
        Dim strIDLog As String
        'leggo la griglia dei volontari per l'associazione del protocollo al fascicolo
        For i = 0 To dtgVolontari.Items.Count - 1
            chkSelVol = dtgVolontari.Items(i).FindControl("chkSelVol")
            If chkSelVol.Checked = True Then
                'scrivo log

                'strIDLog = LogProtocolliVolontari(dtgVolontari.Items(i).Cells(7).Text, Session("Utente"), "CREACOLLEGAMENTOFASCICOLO", IdProtocolloSIGED, 0)
                'strIDLog = LogProtocolliConferma()
                'controllo se il volonario ha il fascicolo
                If VerificaEsistenzaFascicoloVolontario(dtgVolontari.Items(i).Cells(1).Text, strIdFasc) = True Then
                    'impegno il volontario nel log padre 
                    strIDLog = LogProtocolliVolontari(dtgVolontari.Items(i).Cells(1).Text, Session("Utente"), "CREACOLLEGAMENTOFASCICOLO", IdProtocolloSIGED, 1)
                    '  intConta = intConta + 1
                    ' strIdFasc = dtgVolontari.Items(i).Cells(7).Text 'idfascicolo
                    IdFascSIGED = SIGED.SIGED_IdFascicoloCompleto(strIdFasc)

                    'inserisco nel dettaGLIO
                    wsOUT = SIGED.SIGED_CreaCollegamentoFascicolo(IdFascSIGED, IdProtocolloSIGED)
                    'INSERISCONELDETTAGLIO in lavorazione
                    'LogProtocolliConferma(strIDLog, IdProtocolloSIGED, 1)


                    If Left(wsOUT, 5) = "00000" Then ' da verifica in uscita che tipo di esito esce
                        LogProtocolliConferma(strIDLog, IdProtocolloSIGED, 2, "") 'eseguito
                    Else
                        If InStr(wsOUT, "PRIMARY KEY") <> 0 Then
                            LogProtocolliConferma(strIDLog, IdProtocolloSIGED, 2, "Esistente") 'non eseguito perchè esistente
                        Else
                            LogProtocolliConferma(strIDLog, IdProtocolloSIGED, 0, wsOUT) 'non eseguito altro errore
                        End If

                    End If



                    'If InStr(SIGED.SIGED_Esito(wsOUT), "PRIMARY KEY") <> 0 Then 'gia esiste (?) quindi ok Danilo
                    '    'intEsito = intEsito + 1
                    '    'imposto esito ESEGUITO nel log PADRE
                    '    LogProtocolliConferma(strIDLog, IdProtocolloSIGED, 2)
                    'End If
                    'If InStr(SIGED.SIGED_Esito(wsOUT), "PRIMARY KEY") = 0 Then
                    '    LogProtocolliConferma(strIDLog, IdProtocolloSIGED, 2)
                    'End If
                Else
                    strIDLog = LogProtocolliVolontari(dtgVolontari.Items(i).Cells(1).Text, Session("Utente"), "CREACOLLEGAMENTOFASCICOLO", IdProtocolloSIGED, 0)
                    blnLog = True
                End If
            End If
        Next

        'If SIGED.SIGED_Codice_Esito(wsOUT) = 0 Then

        'rem Danilo
        'If intEsito <> intConta Then
        '    lblmsg.Text = "Aggiornamento eseguito"
        'Else
        '    lblmsg.Text = "Protocollo già esistente."
        'End If
        'fine rem Danilo

        SIGED.SIGED_Chiudi_Autenticazione(strNome, strCognome)
        If blnLog Then
            lblmsg.Text = "Associazione protocolli registrata. Per i fascicoli mancanti l'associazione sarà effettuata automaticamente appena disponibili."
        Else
            lblmsg.Text = "Associazione protocolli registrata."
        End If


    End Sub

    Private Function VerificaEsistenzaFascicoloVolontario(ByVal IdEntità As Integer, ByRef IdFascicolo As String) As Boolean
        'CONTROLLO SE IL VOLONTARIO HA IL FASCICOLO 
        Dim strSQL As String
        Dim dtrVol As SqlClient.SqlDataReader
        Dim bln As Boolean = False
        If Not dtrVol Is Nothing Then
            dtrVol.Close()
            dtrVol = Nothing
        End If
        strSQL = "Select CodiceFascicolo,IdFascicolo from Entità where idEntità=" & IdEntità & " and isnull(CodiceFascicolo,'') <>''"
        dtrVol = ClsServer.CreaDatareader(strSQL, Session("conn"))
        If dtrVol.HasRows = True Then
            dtrVol.Read()
            IdFascicolo = dtrVol("IdFascicolo")
            bln = True
        End If
        If Not dtrVol Is Nothing Then
            dtrVol.Close()
            dtrVol = Nothing
        End If
        Return bln
    End Function

    Private Function LogProtocolliVolontari(ByVal strIdEntita As String, ByVal strUserName As String, ByVal StrMetodo As String, ByVal StrParametri As String, ByVal Eseguito As Integer) As Integer
        Dim strSql As String
        Dim myCommand As System.Data.SqlClient.SqlCommand
        myCommand = New System.Data.SqlClient.SqlCommand
        myCommand.Connection = Session("conn")
        'loggo su logfascicolivolontari
        strSql = "INSERT INTO LogProtocolliVolontari([Username],[Metodo],[IdEntità],parametri,[DataOraRichiesta],[DataOraEsecuzione],[Eseguito])"
        strSql = strSql & " VALUES('" & Session("Utente") & "','" & StrMetodo & "','" & strIdEntita & "','" & Replace(StrParametri, "'", "''") & "',getdate(),NULL," & Eseguito & ")"


        myCommand.CommandText = strSql
        myCommand.ExecuteNonQuery()

        '---recupero l'id appena inserito
        Dim strID As String
        strSql = "select @@identity as Id"
        dtrGenerico = ClsServer.CreaDatareader(strSql, Session("conn"))
        dtrGenerico.Read()
        strID = dtrGenerico("Id")
        dtrGenerico.Close()
        dtrGenerico = Nothing
        Return strID
    End Function
    'Private Function LogProtocolliScrivi(ByVal IntIdLog As Integer, ByVal strUserName As String, ByVal StrMetodo As String, ByVal StrParametri As String, ByVal Eseguito As Integer) As Integer
    '    Dim strSql As String
    '    Dim myCommand As System.Data.SqlClient.SqlCommand
    '    Dim dtrLeggiDati As SqlClient.SqlDataReader


    '    'loggo su logfascicolivolontariDett
    '    strSql = "INSERT INTO LogProtocolliVolontariDett(IdLogProtocolliVolontari,[Username],[Metodo],parametri,[DataOraRichiesta],[DataOraEsecuzione],[Eseguito])"
    '    strSql = strSql & " VALUES(" & IntIdLog & ",'" & strUserName & "','" & Replace(StrMetodo, "'", "''") & "','" & Replace(StrParametri, "'", "''") & "',getdate(),NULL," & Eseguito & ")"
    '    myCommand = New System.Data.SqlClient.SqlCommand
    '    myCommand.Connection = Session("conn")

    '    myCommand.CommandText = strSql
    '    myCommand.ExecuteNonQuery()

    '    '---recupero l'id appena inserito in logfascicolivolontariDett
    '    Dim strID As String
    '    strSql = "select @@identity as Id"
    '    myCommand.CommandText = strSql
    '    dtrLeggiDati = myCommand.ExecuteReader


    '    'dtrLeggiDati = ClsServer.CreaDatareader(strSql, sqllocalconn)
    '    dtrLeggiDati.Read()
    '    strID = dtrLeggiDati("Id")
    '    dtrLeggiDati.Close()
    '    dtrLeggiDati = Nothing

    '    LogProtocolliScrivi = strID
    '    'Return strID
    '    If Not dtrLeggiDati Is Nothing Then
    '        dtrLeggiDati.Close()
    '        dtrLeggiDati = Nothing
    '    End If
    'End Function

    Private Function LogProtocolliConferma(ByVal idLog As Integer, ByVal StrParametri As String, ByVal Eseguito As Integer, ByVal strErrore As String)
        'Eseguito 1 in lavorazione 2 eseguito
        Dim strSql As String
        Dim myCommand As System.Data.SqlClient.SqlCommand
        strSql = "update LogProtocolliVolontari set Parametri= '" & StrParametri & "', DataOraEsecuzione = getdate(), Eseguito=" & Eseguito & " , errore = '" & strErrore.Replace("'", "''") & "' where idLogProtocolliVolontari = " & idLog
        myCommand = New System.Data.SqlClient.SqlCommand
        myCommand.Connection = Session("conn")
        myCommand.CommandText = strSql
        myCommand.ExecuteNonQuery()

    End Function
    Private Sub chkSelDesel_CheckedChanged(sender As Object, e As System.EventArgs) Handles chkSelDesel.CheckedChanged
        Dim chkSelVol As CheckBox
        If chkSelDesel.Checked = True Then
            For i = 0 To dtgVolontari.Items.Count - 1
                chkSelVol = dtgVolontari.Items(i).FindControl("chkSelVol")
                chkSelVol.Checked = True
            Next

        Else
            For i = 0 To dtgVolontari.Items.Count - 1
                chkSelVol = dtgVolontari.Items(i).FindControl("chkSelVol")
                chkSelVol.Checked = False
            Next
        End If
    End Sub


    Protected Sub CmdChiudi_Click(sender As Object, e As EventArgs) Handles CmdChiudi.Click
        Response.Redirect("WfrmAssociaVolontari.aspx?CheckIndietro=" & Request.QueryString("CheckIndietro") & "&Ente=" & Request.QueryString("Ente") & "&CodEnte=" & Request.QueryString("CodEnte") & "&Progetto=" & Request.QueryString("Progetto") & "&Bando=" & Request.QueryString("Bando") & "&Settore=" & Request.QueryString("Settore") & "&Area=" & Request.QueryString("Area") & "&InAttesa=" & Request.QueryString("InAttesa") & "&CodiceProgetto=" & Request.QueryString("CodiceProgetto") & "&PaginaGrid=" & Request.QueryString("PaginaGrid") & "&Vengoda=" & Request.QueryString("Vengoda") & "&IdAttivitaSedeAssegnazione=" & Request.QueryString("IDAttivitasedeAssegnazione") & "&IdAttivita=" & Request.QueryString("IdAttivita") & "&IdEnteSede=" & Request.QueryString("IdEnteSede") & "&presenta=" & Request.QueryString("presenta") & "&CodiceFascicolo=" & Request.QueryString("CodiceFascicolo") & "&DescFascicolo=" & Request.QueryString("DescFascicolo") & "&NumeroFascicolo=" & Request.QueryString("NumeroFascicolo"))
    End Sub
End Class