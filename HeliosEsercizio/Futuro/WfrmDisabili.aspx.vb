Imports System.IO
Imports System.Text.RegularExpressions
Imports System.Data.SqlClient

Public Class WfrmDisabili
    Inherits System.Web.UI.Page
    Dim querySQL As String
    Dim dataReader As SqlClient.SqlDataReader
    Dim sqlCommand As SqlClient.SqlCommand
    Dim dataSet As DataSet
    Dim SELEZIONA_COMUNE_NASCITA As String = "Selezionare Provincia/Nazione di nascita"
    Dim SELEZIONA_COMUNE_RESIDENZA As String = "Selezionare Provincia/Nazione di residenza"
    Dim idcomuneRes As Integer
    Dim idComuneNascita As Integer
    Dim IdAssociaAmbitoCausaleAccompagno As Integer
    Private Writer As StreamWriter
    Private strNote As String
    Private Tot As Integer
    Private TotOk As Integer
    Private TotKo As Integer
    Private NomeUnivoco As String
    Private xIdAttivita As Integer
    Private strSql As String
    Private dtrGenreico As SqlClient.SqlDataReader

#Region "Utility"
    Private Sub ChiudiDataReader(ByRef dataReader As SqlDataReader)
        If Not dataReader Is Nothing Then
            dataReader.Close()
            dataReader = Nothing
        End If
    End Sub

    Private Sub VerificaSessione()
        If Not Session("LogIn") Is Nothing Then
            If Session("LogIn") = False Then
                Response.Redirect("LogOn.aspx")
            End If
        Else
            Response.Redirect("LogOn.aspx")
        End If
    End Sub
#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        VerificaSessione()
        If Page.IsPostBack = False Then
            ChiusoAperto.Value = "0"
            ConfiguraPostBackSezioni()
            CaricaComboAmbiti()
            CaricaComboProvinciaNazione("NASCITA", chkEsteroNascita.Checked)
            CaricaComboProvinciaNazione("RESIDENZA", chkEsteroNascita.Checked)

            CaricaGriglia()
        End If
        Dim Modifica As Integer
        If Session("TipoUtente") = "E" Then
            Modifica = CInt(ClsUtility.LoadProgettiAbilitaModificaEnte(Request.QueryString("IdAttivita"), Session("Conn")))
        Else
            Modifica = CInt(Request.QueryString("Modifica"))
        End If

        If Modifica = 0 Then
            dgDisabili.Columns(0).Visible = False
            dgDisabili.Columns(12).Visible = False
            flsDisabile.Visible = False
            cmdConferma.Visible = False
        End If
    End Sub

    Protected Sub ConfiguraPostBackSezioni()
        Dim isPostBack As Boolean = Me.IsPostBack
        Me.hdnIsPostbackDisabili.Value = isPostBack
    End Sub

    Sub CaricaComboAmbiti()
        ChiudiDataReader(dataReader)
        querySQL = "select  IdAssociaAmbitoCausaleAccompagno, Causale "
        querySQL = querySQL & "from AssociaAmbitiCausaliAccompagno "
        querySQL = querySQL & "where IdAmbitoAttività=" & Request.QueryString("IdAmbitoAttività") & ""
        querySQL = querySQL & " order by Causale"

        dataSet = ClsServer.DataSetGenerico(querySQL, Session("conn"))

        ddlAmbiti.DataSource = dataSet
        ddlAmbiti.DataTextField = "Causale"
        ddlAmbiti.DataValueField = "IdAssociaAmbitoCausaleAccompagno"
        ddlAmbiti.DataBind()
        dataSet = Nothing

    End Sub

    Sub CaricaGriglia()

        Dim MyDataSet As DataSet
        querySQL = "SELECT AttivitàAccompagnamento.IdAttivitàAccompagnamento as IdAcco, AttivitàAccompagnamento.IDAttività, AttivitàAccompagnamento.Cognome, "
        querySQL = querySQL & " AttivitàAccompagnamento.Nome, AttivitàAccompagnamento.IDComuneNascita, AttivitàAccompagnamento.DataNascita, "
        querySQL = querySQL & " AttivitàAccompagnamento.CodiceFiscale, AttivitàAccompagnamento.IDComuneResidenza, AttivitàAccompagnamento.Indirizzo, "
        querySQL = querySQL & " AttivitàAccompagnamento.Civico, AttivitàAccompagnamento.Cap, AttivitàAccompagnamento.UsernameInseritore, "
        querySQL = querySQL & " AttivitàAccompagnamento.DataCreazioneRecord,AttivitàAccompagnamento.IdAssociaAmbitoCausaleAccompagno, C1.Denominazione As ComNas, C2.Denominazione As ComRes,isnull(P1.DescrAbb,'') as ProvRes, AssociaAmbitiCausaliAccompagno.Causale"
        querySQL = querySQL & " FROM attività "
        querySQL = querySQL & " INNER Join AttivitàAccompagnamento ON attività.IDAttività = AttivitàAccompagnamento.IDAttività"
        querySQL = querySQL & " LEFT Join AssociaAmbitiCausaliAccompagno ON AttivitàAccompagnamento.IdAssociaAmbitoCausaleAccompagno = AssociaAmbitiCausaliAccompagno.IdAssociaAmbitoCausaleAccompagno"
        querySQL = querySQL & " LEFT Join comuni C1 on C1.idcomune = AttivitàAccompagnamento.IDComuneNascita"
        querySQL = querySQL & " LEFT Join comuni C2 on C2.idcomune = AttivitàAccompagnamento.IDComuneResidenza"
        querySQL = querySQL & " LEFT Join Provincie P1 on P1.idprovincia = C2.IdProvincia "
        querySQL = querySQL & " WHERE attività.IDAttività = '" & Request.QueryString("IdAttivita") & "' ORDER BY AttivitàAccompagnamento.Cognome + ' ' + AttivitàAccompagnamento.Nome"
        MyDataSet = ClsServer.DataSetGenerico(querySQL, Session("conn"))

        dgDisabili.DataSource = MyDataSet
        Session("Carica") = MyDataSet
        dgDisabili.DataBind()
        Dim Modifica As Integer
        If Session("TipoUtente") = "E" Then
            Modifica = CInt(ClsUtility.LoadProgettiAbilitaModificaEnte(Request.QueryString("IdAttivita"), Session("Conn")))
        Else
            Modifica = CInt(Request.QueryString("Modifica"))
        End If
        If Modifica = 0 Then
            dgDisabili.Columns(0).Visible = False
            dgDisabili.Columns(12).Visible = False
            cmdConferma.Visible = False
        End If


        'nome e posizione di lettura delle colopnne a base 0
        Dim NomeColonne(9) As String
        Dim NomiCampiColonne(9) As String
        'nome della colonna 
        'e posizione nella griglia di lettura
        NomeColonne(0) = "Codice Fiscale"
        NomeColonne(1) = "Cognome"
        NomeColonne(2) = "Nome"
        NomeColonne(3) = "Data di Nascita"
        NomeColonne(4) = "Comune di Nascita"
        NomeColonne(5) = "Comune di Residenza"
        NomeColonne(6) = "Indirizzo"
        NomeColonne(7) = "N°"
        NomeColonne(8) = "Provincia"
        NomeColonne(9) = "Causale"


        NomiCampiColonne(0) = "CodiceFiscale"
        NomiCampiColonne(1) = "Cognome"
        NomiCampiColonne(2) = "Nome"
        NomiCampiColonne(3) = "DataNascita"
        NomiCampiColonne(4) = "ComNas"
        NomiCampiColonne(5) = "ComRes"
        NomiCampiColonne(6) = "Indirizzo"
        NomiCampiColonne(7) = "Civico"
        NomiCampiColonne(8) = "ProvRes"
        NomiCampiColonne(9) = "Causale"


        'carico un datatable che userò poi nella pagina di stampa
        'il numero delle colonne è a base 0
        CaricaDataTablePerStampa(MyDataSet, 9, NomeColonne, NomiCampiColonne)

    End Sub

    Function CheckEsistenzaCodFis(ByVal strCodFis As String) As Boolean
        'dtr per controllo esistenza codice fiscale
        Dim dataReaderTemp As SqlClient.SqlDataReader
        Dim query As String
        CheckEsistenzaCodFis = True
        'carico la stringa della select di controllo esistenza codicefiscale per la risorsa che si vuole inserire 
        query = "select CodiceFiscale from AttivitàAccompagnamento "
        query = query & "where idAttività=" & Request.QueryString("IdAttivita") & " and CodiceFiscale='" & Trim(strCodFis) & "'"
        dataReaderTemp = ClsServer.CreaDatareader(query, Session("conn"))
        'se ci sono dei record
        If dataReaderTemp.HasRows = True Then
            CheckEsistenzaCodFis = False
        End If
        ChiudiDataReader(dataReaderTemp)

    End Function

    Private Function ControlliCodiceFiscale() As Boolean
        ControlliCodiceFiscale = True
        If NazionalitaItaliana(ddlComuneNascita.SelectedValue) = True Or NazionalitaItaliana(ddlComuneResidenza.SelectedValue) = True Then
            If CongruenzaCodiceFiscale(txtCodiceFiscale.Text, txtCognome.Text, txtNome.Text, txtDataNascita.Text) = False Then
                lblErrore.Text = "Il codice fiscale risulta non corretto."
                ControlliCodiceFiscale = False
            End If
        End If
    End Function

    Private Function NazionalitaItaliana(ByVal pComune As String) As Boolean
        Dim dtrNazioneBase As Data.SqlClient.SqlDataReader
        'Dim strsql As String

        querySQL = "SELECT Nazioni.NazioneBase FROM Nazioni " & _
                 "INNER JOIN Regioni ON Regioni.IdNazione = Nazioni.IdNazione " & _
                 "INNER JOIN Provincie ON Provincie.IdRegione = Regioni.IdRegione " & _
                 "INNER JOIN Comuni ON Comuni.IdProvincia = Provincie.IdProvincia " & _
                 "WHERE Comuni.Denominazione = '" & ClsServer.NoApice(pComune) & "'"

        dtrNazioneBase = ClsServer.CreaDatareader(querySQL, Session("conn"))

        dtrNazioneBase.Read()
        If dtrNazioneBase.HasRows = False Then
            NazionalitaItaliana = False
        Else
            NazionalitaItaliana = dtrNazioneBase("NazioneBase")
        End If

        dtrNazioneBase.Close()
        dtrNazioneBase = Nothing
    End Function

    Private Function CongruenzaCodiceFiscale(ByVal pCodiceFiscale, ByVal pCognome, ByVal pNome, ByVal pDataNascita) As Boolean
        '--- verifica la coerenza tra codfis e cognome, nome, data nascita

        Dim TutteLeLettere As String = "ABCDEFGHIJKLMNOPQRSTUVWXYZ"
        Dim TuttiINumeri As String = "0123456789"
        Dim TuttiGliOmocodici As String = "LMNPQRSTUV"
        Dim TutteLeVocali As String = "AEIOU"
        Dim TutteLeConsonanti As String = "BCDFGHJKLMNPQRSTVWXYZ"
        Dim CodMese As String = "ABCDEHLMPRST"
        Dim swErr As Boolean = False
        Dim Vocali As String
        Dim Consonanti As String
        Dim xCodCognome As String
        Dim xCodNome As String
        Dim tmpGiornoNascitaM As Integer
        Dim tmpGiornoNascitaF As Integer
        Dim tmpValore As String
        Dim i As Integer

        CongruenzaCodiceFiscale = True

        pCodiceFiscale = UCase(pCodiceFiscale)
        If Len(pCodiceFiscale) <> 16 Then
            CongruenzaCodiceFiscale = False
            Exit Function
        End If

        '--- cognome e nome stringa
        For i = 1 To 6
            If InStr(TutteLeLettere, Mid(pCodiceFiscale, i, 1)) = -1 Then
                CongruenzaCodiceFiscale = False
                Exit Function
            End If
        Next i

        '--- anno numerico
        For i = 7 To 8
            If InStr(TuttiINumeri, Mid(pCodiceFiscale, i, 1)) = -1 Then
                If InStr(TuttiGliOmocodici, Mid(pCodiceFiscale, i, 1)) = -1 Then
                    CongruenzaCodiceFiscale = False
                    Exit Function
                End If
            End If
        Next i

        '--- mese stringa
        For i = 9 To 9
            If InStr(TutteLeLettere, Mid(pCodiceFiscale, i, 1)) = -1 Then
                CongruenzaCodiceFiscale = False
                Exit Function
            End If
        Next i

        '--- giorno numerico
        For i = 10 To 11
            If InStr(TuttiINumeri, Mid(pCodiceFiscale, i, 1)) = -1 Then
                If InStr(TuttiGliOmocodici, Mid(pCodiceFiscale, i, 1)) = -1 Then
                    CongruenzaCodiceFiscale = False
                    Exit Function
                End If
            End If
        Next i

        '--- primo carattere comune stringa
        For i = 12 To 12
            If InStr(TutteLeLettere, Mid(pCodiceFiscale, i, 1)) = -1 Then
                CongruenzaCodiceFiscale = False
                Exit Function
            End If
        Next i

        '--- 3 caratteri comune numerico
        For i = 13 To 15
            If InStr(TuttiINumeri, Mid(pCodiceFiscale, i, 1)) = -1 Then
                If InStr(TuttiGliOmocodici, Mid(pCodiceFiscale, i, 1)) = -1 Then
                    CongruenzaCodiceFiscale = False
                    Exit Function
                End If
            End If
        Next i

        '--- ultimo carattere di controllo stringa
        For i = 16 To 16
            If InStr(TutteLeLettere, Mid(pCodiceFiscale, i, 1)) = -1 Then
                CongruenzaCodiceFiscale = False
                Exit Function
            End If
        Next i

        '--- FINE CONTROLLO FORMALE
        '--- Controllo Cognome
        pCognome = UCase(pCognome)
        For i = 1 To Len(pCognome)
            If InStr(TutteLeVocali, Mid(pCognome, i, 1)) > 0 Then
                Vocali = Vocali + Mid(pCognome, i, 1)
            Else
                If InStr(TutteLeConsonanti, Mid(pCognome, i, 1)) > 0 Then
                    Consonanti = Consonanti + Mid(pCognome, i, 1)
                End If
            End If
            If Len(Consonanti) = 3 Then
                Exit For
            End If
        Next i
        If Len(Consonanti) < 3 Then
            Consonanti = Consonanti + Mid(Vocali, 1, 3 - Len(Consonanti))
            For i = Len(Consonanti) + 1 To 3
                Consonanti = Consonanti & "X"
            Next i
        End If
        xCodCognome = Consonanti

        If xCodCognome <> Mid(pCodiceFiscale, 1, 3) Then
            'errore sul cognome
            CongruenzaCodiceFiscale = False
            Exit Function
        End If

        '--- Controllo Nome
        Consonanti = vbNullString
        Vocali = vbNullString
        pNome = UCase(pNome)
        For i = 1 To Len(pNome)
            If InStr(TutteLeVocali, Mid(pNome, i, 1)) > 0 Then
                Vocali = Vocali + Mid(pNome, i, 1)
            Else
                If InStr(TutteLeConsonanti, Mid(pNome, i, 1)) > 0 Then
                    Consonanti = Consonanti + Mid(pNome, i, 1)
                End If
            End If
        Next i

        If Len(Consonanti) >= 4 Then
            Consonanti = Mid(Consonanti, 1, 1) + Mid(Consonanti, 3, 2)
        Else
            If Len(Consonanti) < 3 Then
                Consonanti = Consonanti + Mid(Vocali, 1, 3 - Len(Consonanti))
                For i = Len(Consonanti) + 1 To 3
                    Consonanti = Consonanti & "X"
                Next i
            End If
        End If
        xCodNome = Consonanti

        If xCodNome <> Mid(pCodiceFiscale, 4, 3) Then
            CongruenzaCodiceFiscale = False
            Exit Function
        End If

        '--- Controllo Anno	
        tmpValore = DecodificaOmocodici(Mid(pCodiceFiscale, 7, 1)) & DecodificaOmocodici(Mid(pCodiceFiscale, 8, 1))
        If tmpValore <> Mid(pDataNascita, 9, 2) Then
            CongruenzaCodiceFiscale = False
            Exit Function
        End If

        '--- Controllo Mese				
        If Mid(pCodiceFiscale, 9, 1) <> Mid(CodMese, Mid(pDataNascita, 4, 2), 1) Then
            CongruenzaCodiceFiscale = False
            Exit Function
        End If

        '--- Controllo Giorno
        tmpGiornoNascitaF = Mid(pDataNascita, 1, 2) + 40
        tmpGiornoNascitaM = Mid(pDataNascita, 1, 2)

        tmpValore = DecodificaOmocodici(Mid(pCodiceFiscale, 10, 1)) + DecodificaOmocodici(Mid(pCodiceFiscale, 11, 1))
        If CInt(tmpValore) <> tmpGiornoNascitaF And CInt(tmpValore) <> tmpGiornoNascitaM Then
            CongruenzaCodiceFiscale = False
            Exit Function
        End If

    End Function

    Private Function DecodificaOmocodici(ByVal pValore) As String
        Dim TuttiGliOmocodici As String = "LMNPQRSTUV"

        If InStr(TuttiGliOmocodici, pValore) > 0 Then

            Select Case pValore
                Case Is = "L"
                    DecodificaOmocodici = "0"

                Case "M"
                    DecodificaOmocodici = "1"

                Case "N"
                    DecodificaOmocodici = "2"

                Case "P"
                    DecodificaOmocodici = "3"

                Case "Q"
                    DecodificaOmocodici = "4"

                Case "R"
                    DecodificaOmocodici = "5"

                Case "S"
                    DecodificaOmocodici = "6"

                Case "T"
                    DecodificaOmocodici = "7"

                Case "U"
                    DecodificaOmocodici = "8"

                Case "V"
                    DecodificaOmocodici = "9"

            End Select
        Else
            DecodificaOmocodici = pValore
        End If

    End Function

    Private Sub PulisciCampi()
        TxtCapp.Text = String.Empty
        txtCivico.Text = String.Empty
        txtCodiceFiscale.Text = String.Empty
        txtCognome.Text = String.Empty
        txtDataNascita.Text = String.Empty
        txtIndirizzo.Text = String.Empty
        txtNome.Text = String.Empty
        ddlComuneResidenza.SelectedIndex = 0
        ddlComuneNascita.SelectedIndex = 0
        ddlProvinciaNascita.SelectedIndex = 0
        ddlProvinciaResidenza.SelectedIndex = 0
        ddlComuneNascita.Enabled = False
        ddlComuneResidenza.Enabled = False
        ddlComuneNascita.SelectedItem.Text = SELEZIONA_COMUNE_NASCITA
        ddlComuneResidenza.SelectedItem.Text = SELEZIONA_COMUNE_RESIDENZA
        ddlAmbiti.SelectedIndex = 0
        If Session("msg") <> 1 Then
            lblConferma.Text = String.Empty
        End If
        TxtMod.Value = String.Empty
        TxtAcco.Value = String.Empty
        TxtIdComNas.Value = String.Empty
        TxtIdComRes.Value = String.Empty
        Session("msg") = 2
    End Sub

    Private Sub dgDisabili_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgDisabili.PageIndexChanged
        'passo il nuovo indice selezionato all'indice della pagina da visualizzare
        dgDisabili.SelectedIndex = -1
        dgDisabili.CurrentPageIndex = e.NewPageIndex
        'passo la session con i valori tenuti in memoria
        dgDisabili.DataSource = Session("Carica")
        dgDisabili.DataBind()
    End Sub

    Private Sub dgDisabili_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgDisabili.ItemCommand
        lblErrore.Text = String.Empty
        lblConferma.Text = String.Empty
        If e.CommandName = "Rimuovi" Then
            querySQL = "select * from AttivitàAccompagnamento where idattività=" & Request.QueryString("IdAttivita") & ""
            ChiudiDataReader(dataReader)
            dataReader = ClsServer.CreaDatareader(querySQL, Session("conn"))
            dataReader.Read()
            If dataReader.HasRows = True Then
                If Not dataReader Is Nothing Then
                    dataReader.Close()
                    dataReader = Nothing
                End If
                querySQL = "delete AttivitàAccompagnamento where idattività='" & Request.QueryString("IdAttivita") & "' and idAttivitàAccompagnamento=" & e.Item.Cells(1).Text & ""
                sqlCommand = ClsServer.EseguiSqlClient(querySQL, Session("conn"))
                If Not dataReader Is Nothing Then
                    dataReader.Close()
                    dataReader = Nothing
                End If
            End If
            CaricaGriglia()
            lblConferma.Text = "Cancellazione effettuata con successo."
        End If
        If e.CommandName = "Modifica" Then
            Dim selezioneComune As New clsSelezionaComune
            ChiudiDataReader(dataReader)

            CaricaComboProvinciaNazione("NASCITA", chkEsteroNascita.Checked)
            CaricaComboProvinciaNazione("RESIDENZA", chkEsteroNascita.Checked)
            ddlComuneNascita.Enabled = True
            ddlComuneResidenza.Enabled = True
            querySQL = "SELECT AttivitàAccompagnamento.IdAttivitàAccompagnamento as IdAcco, AttivitàAccompagnamento.IDAttività, AttivitàAccompagnamento.Cognome, "
            querySQL = querySQL & " AttivitàAccompagnamento.Nome, AttivitàAccompagnamento.IDComuneNascita, AttivitàAccompagnamento.DataNascita, "
            querySQL = querySQL & " AttivitàAccompagnamento.CodiceFiscale, AttivitàAccompagnamento.IDComuneResidenza, AttivitàAccompagnamento.Indirizzo, "
            querySQL = querySQL & " AttivitàAccompagnamento.Civico, AttivitàAccompagnamento.Cap, AttivitàAccompagnamento.UsernameInseritore, "
            querySQL = querySQL & " AttivitàAccompagnamento.DataCreazioneRecord,AttivitàAccompagnamento.IdAssociaAmbitoCausaleAccompagno, C1.Denominazione As ComNas, C2.Denominazione As ComRes,isnull(P1.DescrAbb,'') as ProvRes,isnull(P2.DescrAbb,'') as ProvNas, "
            querySQL = querySQL & " P2.IDProvincia AS  IDProvNascita ,"
            querySQL = querySQL & " P1.IDProvincia AS  IDProvResidenza "
            querySQL = querySQL & " FROM attività "
            querySQL = querySQL & " INNER Join AttivitàAccompagnamento ON attività.IDAttività = AttivitàAccompagnamento.IDAttività"
            querySQL = querySQL & " LEFT Join comuni C1 on C1.idcomune = AttivitàAccompagnamento.IDComuneNascita"
            querySQL = querySQL & " LEFT Join comuni C2 on C2.idcomune = AttivitàAccompagnamento.IDComuneResidenza"
            querySQL = querySQL & " LEFT Join Provincie P1 on P1.idprovincia = C2.IdProvincia "
            querySQL = querySQL & " LEFT Join Provincie P2 on P2.idprovincia = C1.IdProvincia "
            querySQL = querySQL & " WHERE attività.IDAttività = '" & Request.QueryString("IdAttivita") & "' And idAttivitàAccompagnamento = " & e.Item.Cells(1).Text & ""
            dataReader = ClsServer.CreaDatareader(querySQL, Session("conn"))
            dataReader.Read()

            If dataReader.HasRows = True Then
                lblConferma.Text = ""

                txtNome.Text = IIf(IsDBNull(dataReader("Nome")), "", dataReader("Nome"))
                TxtCapp.Text = IIf(IsDBNull(dataReader("Cap")), "", dataReader("Cap"))
                txtCivico.Text = IIf(IsDBNull(dataReader("Civico")), "", dataReader("Civico"))
                txtCodiceFiscale.Text = IIf(IsDBNull(dataReader("CodiceFiscale")), "", dataReader("CodiceFiscale"))
                txtCognome.Text = IIf(IsDBNull(dataReader("Cognome")), "", dataReader("Cognome"))
                ddlProvinciaNascita.SelectedValue = IIf(IsDBNull(dataReader("IDProvNascita")), "", dataReader("IDProvNascita"))
                TxtIdComNas.Value = IIf(IsDBNull(dataReader("IDComuneNascita")), "", dataReader("IDComuneNascita"))
                ddlProvinciaResidenza.SelectedValue = IIf(IsDBNull(dataReader("IDProvResidenza")), "", dataReader("IDProvResidenza"))
                TxtIdComRes.Value = IIf(IsDBNull(dataReader("IDComuneResidenza")), "", dataReader("IDComuneResidenza"))
                txtDataNascita.Text = IIf(IsDBNull(dataReader("DataNascita")), "", dataReader("DataNascita"))
                txtIndirizzo.Text = IIf(IsDBNull(dataReader("Indirizzo")), "", dataReader("Indirizzo"))
                TxtMod.Value = "1"
                TxtAcco.Value = dataReader("IdAcco")
                TxtIdComNas.Value = dataReader("IDComuneNascita")
                TxtIdComRes.Value = dataReader("IDComuneResidenza")
                ddlAmbiti.SelectedValue = dataReader("IdAssociaAmbitoCausaleAccompagno")
                Session("idcomune") = dataReader("IDComuneResidenza")
                ChiudiDataReader(dataReader)
                ddlComuneNascita = selezioneComune.CaricaComuniNascita(ddlComuneNascita, ddlProvinciaNascita.SelectedValue, Session("Conn"))
                ddlComuneResidenza = selezioneComune.CaricaComuni(ddlComuneResidenza, ddlProvinciaResidenza.SelectedValue, Session("Conn"))
                ddlComuneNascita.SelectedValue = TxtIdComNas.Value
                ddlComuneResidenza.SelectedValue = TxtIdComRes.Value
            End If
        End If
    End Sub

    Private Sub cmdConferma_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles cmdConferma.Click
        lblErrore.Text = String.Empty
        lblConferma.Text = String.Empty
        If VerificaCampiObbligatori() = False Then
            Exit Sub
        End If
        ChiudiDataReader(dataReader)

        'Se sono in modifica
        If TxtMod.Value = "1" Then

            If NazionalitaItaliana(ddlComuneNascita.SelectedValue) = False Or CongruenzaCodiceFiscale(txtCodiceFiscale.Text, txtCognome.Text, txtNome.Text, txtDataNascita.Text) = True Then
                'DELETE
                querySQL = "delete AttivitàAccompagnamento where idattività='" & Request.QueryString("IdAttivita") & "' and idAttivitàAccompagnamento= '" & TxtAcco.Value & "'"
                sqlCommand = ClsServer.EseguiSqlClient(querySQL, Session("conn"))

                If CheckEsistenzaCodFis(txtCodiceFiscale.Text) = True Then
                    'INSERT

                    Dim strMiaCausale As String = ""
                    Dim bandiera As Boolean = True
                    If ClsUtility.CAP_VERIFICA(Session("Conn"), _
                            strMiaCausale, bandiera, Trim(TxtCapp.Text), TxtIdComRes.Value, "", "", txtIndirizzo.Text, txtCivico.Text) = False Then
                        lblErrore.Text = strMiaCausale
                        Exit Sub
                    End If

                    querySQL = "insert into AttivitàAccompagnamento (IDAttività,"
                    querySQL = querySQL & " cognome,nome,IDComuneNascita,DataNascita,CodiceFiscale,IDComuneResidenza, " & _
                    " Indirizzo,Civico,Cap,IdAssociaAmbitoCausaleAccompagno,Usernameinseritore,DataCreazioneRecord) values "
                    querySQL = querySQL & "('" & Request.QueryString("IdAttivita") & "','" & Replace(txtCognome.Text, "'", "''") & "', " & _
                    " '" & Replace(txtNome.Text, "'", "''") & "','" & TxtIdComNas.Value & "','" & Replace(Trim(txtDataNascita.Text), "'", "''") & "'," & _
                    " '" & Trim(txtCodiceFiscale.Text) & "','" & TxtIdComRes.Value & "','" & Replace(txtIndirizzo.Text, "'", "''") & "'," & _
                    " '" & Replace(Trim(txtCivico.Text), "'", "''") & "','" & Replace(Trim(TxtCapp.Text), "'", "''") & "','" & ddlAmbiti.SelectedValue & "','" & Session("Utente") & "',getdate())"

                    sqlCommand = ClsServer.EseguiSqlClient(querySQL, Session("conn"))
                    PulisciCampi()
                    CaricaGriglia()
                    lblConferma.Text = "Modifica Effettuata con Successo."
                Else
                    lblErrore.Text = "Si sta tentando di inserire una risorsa gia' presente.Modifica non effettuata"
                    Exit Sub
                End If
            Else
                lblErrore.Text = "Codice Fiscale Errato.Modifica non effettuata"
                Exit Sub
            End If


        Else

            'controllo il cap
            Dim strMiaCausale As String = ""
            Dim bandiera As Boolean = True
            If ClsUtility.CAP_VERIFICA(Session("Conn"), _
                    strMiaCausale, bandiera, Trim(TxtCapp.Text), TxtIdComRes.Value, "", "", txtIndirizzo.Text, txtCivico.Text) = False Then
                lblErrore.Text = strMiaCausale
                Exit Sub
            End If

            If NazionalitaItaliana(ddlComuneNascita.SelectedValue) = False Or CongruenzaCodiceFiscale(txtCodiceFiscale.Text, txtCognome.Text, txtNome.Text, txtDataNascita.Text) = True Then
                If CheckEsistenzaCodFis(txtCodiceFiscale.Text) = True Then
                    querySQL = "insert into AttivitàAccompagnamento (IDAttività,"
                    querySQL = querySQL & " cognome,nome,IDComuneNascita,DataNascita,CodiceFiscale,IDComuneResidenza, " & _
                    " Indirizzo,Civico,Cap,IdAssociaAmbitoCausaleAccompagno,Usernameinseritore,DataCreazioneRecord) values "
                    querySQL = querySQL & "('" & Request.QueryString("IdAttivita") & "','" & Replace(Trim(txtCognome.Text), "'", "''") & "', " & _
                    " '" & Replace(Trim(txtNome.Text), "'", "''") & "','" & TxtIdComNas.Value & "','" & Replace(Trim(txtDataNascita.Text), "'", "''") & "'," & _
                    " '" & Replace(Trim(txtCodiceFiscale.Text), "'", "''") & "','" & TxtIdComRes.Value & "','" & Replace(Trim(txtIndirizzo.Text), "'", "''") & "'," & _
                    " '" & Replace(Trim(txtCivico.Text), "'", "''") & "','" & Replace(Trim(TxtCapp.Text), "'", "''") & "','" & ddlAmbiti.SelectedValue & "','" & Session("Utente") & "',getdate())"

                    Dim myCommand As New SqlClient.SqlCommand
                    myCommand = New SqlClient.SqlCommand(querySQL, Session("conn"))
                    myCommand.ExecuteNonQuery()
                    myCommand.Dispose()
                    lblConferma.Visible = True
                    lblConferma.Text = "Inserimento effettuato con successo."
                    Session("msg") = 1
                Else
                    lblErrore.Text = " Si sta tentando di inserire un soggetto gia' presente."
                    Exit Sub
                End If
                PulisciCampi()
                CaricaGriglia()
            Else
                lblErrore.Text = "Codice Fiscale Errato."
                Exit Sub
            End If

        End If
    End Sub

    Protected Sub imgChiudi_Click(ByVal sender As Object, ByVal e As EventArgs) Handles imgChiudi.Click
        Dim Modifica As Integer
        If Session("TipoUtente") = "E" Then
            Modifica = CInt(ClsUtility.LoadProgettiAbilitaModificaEnte(Request.QueryString("IdAttivita"), Session("Conn")))
        Else
            Modifica = CInt(Request.QueryString("Modifica"))
        End If

        Response.Redirect(ClsUtility.TrovaAlboProgetto(Request.QueryString("IdAttivita"), Session("Conn")) & "?Modifica=" & Modifica & "&Nazionale=" & Request.QueryString("Nazionale") & "&IdAttivita=" & CInt(Request.QueryString("IdAttivita")) & "&VengoDa=" & Request.QueryString("VengoDa"))
        'Response.Redirect("TabProgetti.aspx?Modifica=" & Modifica & "&Nazionale=" & Request.QueryString("Nazionale") & "&IdAttivita=" & CInt(Request.QueryString("IdAttivita")) & "&VengoDa=" & Request.QueryString("VengoDa"))
    End Sub


    Sub CaricaDataTablePerStampa(ByVal DataSetDaScorrere As DataSet, ByVal NColonne As Integer, ByVal NomiColonne() As String, ByVal NomiCampiColonne() As String)
        Dim dt As New DataTable
        Dim dr As DataRow
        Dim i As Integer
        Dim x As Integer

        'carico i nomi delle colonne che andrò a stampare nella datagrid
        For x = 0 To NColonne
            dt.Columns.Add(New DataColumn(NomiColonne(x), GetType(String)))
        Next

        'carico il datatable con il risultato della query della ricerca, in qusto caso delle risorse
        If DataSetDaScorrere.Tables(0).Rows.Count > 0 Then
            For i = 1 To DataSetDaScorrere.Tables(0).Rows.Count
                dr = dt.NewRow()
                For x = 0 To NColonne
                    dr(x) = DataSetDaScorrere.Tables(0).Rows.Item(i - 1).Item(NomiCampiColonne(x))
                Next
                dt.Rows.Add(dr)
            Next
        End If

        'passo alla sessione la datatable che ho appena creato e che userò per il databinding della datagrid della stampa
        Session("DtbRicerca") = dt

    End Sub

    Private Sub CmdEsporta_Click(ByVal sender As Object, ByVal e As EventArgs) Handles CmdEsporta.Click
        CmdEsporta.Visible = False
        Dim dtbRicerca As DataTable = Session("DtbRicerca")
        StampaCSV(dtbRicerca)
    End Sub

    Private Sub StampaCSV(ByVal dtbRicerca As DataTable)
        Dim path As String
        Dim xPrefissoNome As String
        Dim url As String
        Dim utility As ClsUtility = New ClsUtility()

        If dtbRicerca.Rows.Count = 0 Then
            ApriCSV1.Visible = False
            CmdEsporta.Visible = False
        Else
            xPrefissoNome = Session("Utente")
            path = Server.MapPath("download")
            url = CreaFileCSV(dtbRicerca, xPrefissoNome, path)
            ApriCSV1.Visible = True
            ApriCSV1.NavigateUrl = url
        End If
    End Sub

    Function CreaFileCSV(ByVal DTBRicerca As DataTable, ByVal xPrefissoNome As String, ByVal mapPath As String) As String

        Dim dtrSediAttuazione As Data.SqlClient.SqlDataReader
        Dim writer As StreamWriter
        Dim xLinea As String
        Dim i As Int64
        Dim j As Int64
        Dim nomeUnivoco As String
        Dim reader As StreamReader
        Dim url As String
        nomeUnivoco = xPrefissoNome & "ExpDati" & Year(Now) & Month(Now) & Day(Now) & Hour(Now) & Minute(Now) & Second(Now)
        writer = New StreamWriter(mapPath & "\" & nomeUnivoco & ".CSV")
        'Creazione dell'inntestazione del CSV
        Dim intNumCol As Int64 = DTBRicerca.Columns.Count
        For i = 0 To intNumCol - 1
            xLinea &= DTBRicerca.Columns.Item(CInt(i)).ColumnName() & ";"
        Next
        writer.WriteLine(xLinea)
        xLinea = vbNullString

        'Scorro tutte le righe del datatable e riempio il CSV
        For i = 0 To DTBRicerca.Rows.Count - 1

            For j = 0 To intNumCol - 1
                If IsDBNull(DTBRicerca.Rows(CInt(i)).Item(CInt(j))) = True Then
                    xLinea &= vbNullString & ";"
                Else
                    xLinea &= ClsUtility.FormatExport(DTBRicerca.Rows(CInt(i)).Item(CInt(j))) & ";"
                End If
            Next

            writer.WriteLine(xLinea)
            xLinea = vbNullString

        Next
        url = "download\" & nomeUnivoco & ".CSV"

        writer.Close()
        writer = Nothing
        Return url
    End Function

    Private Sub ChkEsteroNascita_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkEsteroNascita.CheckedChanged
        CaricaComboProvinciaNazione("NASCITA", chkEsteroNascita.Checked)
        ddlComuneNascita.DataSource = Nothing
        ddlComuneNascita.SelectedIndex = 0
        ddlComuneNascita.SelectedItem.Text = SELEZIONA_COMUNE_NASCITA

        ddlComuneNascita.Enabled = False
    End Sub

    Private Sub ChkEsteroResidenza_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ChkEsteroResidenza.CheckedChanged
        CaricaComboProvinciaNazione("RESIDENZA", ChkEsteroResidenza.Checked)
        ddlComuneResidenza.DataSource = Nothing
        ddlComuneResidenza.SelectedIndex = 0
        ddlComuneResidenza.SelectedItem.Text = SELEZIONA_COMUNE_RESIDENZA
        ddlComuneResidenza.Enabled = False
    End Sub

    Private Sub CaricaComboProvinciaNazione(ByVal RichiamoProvincia As String, ByVal blnEsetero As Boolean)
        Dim selezionaComune As New clsSelezionaComune
        Select Case RichiamoProvincia
            Case "NASCITA"
                ddlProvinciaNascita = selezionaComune.CaricaProvinciaNazione(ddlProvinciaNascita, blnEsetero, Session("Conn"))
            Case "RESIDENZA"
                ddlProvinciaResidenza = selezionaComune.CaricaProvinciaNazione(ddlProvinciaResidenza, blnEsetero, Session("Conn"))

        End Select
    End Sub

    Private Function VerificaCampiObbligatori() As Boolean
        Dim utility As ClsUtility = New ClsUtility()
        Dim data As Date
        Dim idTipoProgetto As String = utility.TipologiaProgettoDaIdAttivita(Request.QueryString("IdAttivita"), Session("conn"))
        Dim campiValidi As Boolean = True
        Dim campoObbligatorio As String = "Il campo {0} è obbligatorio.<br/>"

        If (txtCognome.Text = String.Empty) Then
            lblErrore.Text = lblErrore.Text + String.Format(campoObbligatorio, "Cognome")
            campiValidi = False
        End If
        If (txtNome.Text = String.Empty) Then
            lblErrore.Text = lblErrore.Text + String.Format(campoObbligatorio, "Nome")
            campiValidi = False
        End If

        If (txtCodiceFiscale.Text = String.Empty) Then
            lblErrore.Text = lblErrore.Text + String.Format(campoObbligatorio, "Codice Fiscale")
            campiValidi = False
        End If
        If (txtDataNascita.Text = String.Empty) Then
            lblErrore.Text = lblErrore.Text + String.Format(campoObbligatorio, "Data di nascita")
            campiValidi = False
        Else
            If ValidaData(txtDataNascita.Text, "Data di nascita") = False Then
                campiValidi = False
            End If
        End If
        If (ddlProvinciaNascita.SelectedIndex = 0) Then
            lblErrore.Text = lblErrore.Text + String.Format(campoObbligatorio, "Provincia/Nazione di nascita")
            campiValidi = False
        End If
        If (ddlComuneNascita.SelectedIndex = 0) Then
            lblErrore.Text = lblErrore.Text + String.Format(campoObbligatorio, "Comune di nascita")
            campiValidi = False
        End If

        If (ddlProvinciaResidenza.SelectedIndex = 0) Then
            lblErrore.Text = lblErrore.Text + String.Format(campoObbligatorio, "Provincia/Nazione di residenza")
            campiValidi = False
        End If
        If (ddlComuneResidenza.SelectedIndex = 0) Then
            lblErrore.Text = lblErrore.Text + String.Format(campoObbligatorio, "Comune di residenza")
            campiValidi = False
        End If
        If (txtIndirizzo.Text = String.Empty) Then
            lblErrore.Text = lblErrore.Text + String.Format(campoObbligatorio, "Indirizzo domicilio")
            campiValidi = False
        End If
        If (txtCivico.Text = String.Empty) Then
            lblErrore.Text = lblErrore.Text + String.Format(campoObbligatorio, "Numero Civico")
            campiValidi = False
        End If
        If (TxtCapp.Text = String.Empty) Then
            lblErrore.Text = lblErrore.Text + String.Format(campoObbligatorio, "C.A.P.")
            campiValidi = False
        End If




        Return campiValidi
    End Function

    Private Function ValidaData(ByVal data As String, ByVal nomeCampo As String) As Boolean
        Dim dataTmp As Date
        Dim dataValida As Boolean = True
        Dim messaggioDataValida As String = "Il valore di '{0}' non è valido. Inserire la data nel formato gg/mm/aaaa.<br/>"

        If (Date.TryParse(data, dataTmp) = False) Then
            lblErrore.Text = lblErrore.Text + String.Format(messaggioDataValida, nomeCampo)
            dataValida = False
        End If
        Return dataValida

    End Function

    Private Sub ddlProvinciaNascita_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlProvinciaNascita.SelectedIndexChanged
        Dim SelComune As New clsSelezionaComune
        ddlComuneNascita.Enabled = True
        ddlComuneNascita = SelComune.CaricaComuniNascita(ddlComuneNascita, ddlProvinciaNascita.SelectedValue, Session("Conn"))
    End Sub

    Private Sub ddlProvinciaResidenza_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlProvinciaResidenza.SelectedIndexChanged
        Dim SelComune As New clsSelezionaComune
        ddlComuneResidenza.Enabled = True
        ddlComuneResidenza = SelComune.CaricaComuni(ddlComuneResidenza, ddlProvinciaResidenza.SelectedValue, Session("Conn"))
    End Sub

    <System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()>
    Public Shared Function GetCompletionList(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As List(Of String)

        Dim conn As SqlConnection = New SqlConnection
        conn.ConnectionString = ConfigurationManager _
             .ConnectionStrings("unscproduzionenewConnectionString").ConnectionString


        Dim cmd As SqlCommand = New SqlCommand
        cmd.CommandText = " Select Top 30 CAP_INDIRIZZI.Indirizzo as CityName FROM  CAP_INDIRIZZI WHERE (CAP_INDIRIZZI.Indirizzo LIKE '%" + prefixText.Replace("'", "''") + "%') and idcomune='" & contextKey & "'  ORDER BY CAP_INDIRIZZI.Indirizzo"
        cmd.Connection = conn
        conn.Open()

        Dim oReader As SqlDataReader = cmd.ExecuteReader
        Dim indirizzi As List(Of String) = New List(Of String)

        While oReader.Read
            indirizzi.Add(oReader.GetString(0))
        End While


        If Not oReader Is Nothing Then
            oReader.Close()
            oReader = Nothing
            conn.Close()
        End If
        Return indirizzi


    End Function

    Protected Sub imgCap_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgCap.Click
        lblErrore.Text = String.Empty
        lblConferma.Text = String.Empty
        Dim selCap As New clsSelezionaComune
        If (ddlComuneResidenza.SelectedIndex = 0) Then
            lblErrore.Text = "Per ottenere il C.A.P. della residenza è necessario indicare almeno il comune."
        Else
            TxtCapp.Text = selCap.RitornaCap(ddlComuneResidenza.SelectedValue, txtIndirizzo.Text, txtCivico.Text, Session("conn"))
            If (TxtCapp.Text = String.Empty And txtIndirizzo.Text = String.Empty) Then
                lblErrore.Text = "Per ottenere il C.A.P. della residenza è necessario indicare almeno il comune e l'indirizzo di residenza."
            End If
        End If
    End Sub

    Protected Sub ddlComuneResidenza_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlComuneResidenza.SelectedIndexChanged
        TxtIdComRes.Value = ddlComuneResidenza.SelectedValue
    End Sub

    Protected Sub ddlComuneNascita_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlComuneNascita.SelectedIndexChanged
        TxtIdComNas.Value = ddlComuneNascita.SelectedValue
    End Sub

    ' INIZIO GESTIONE IMPORT DISABILI DA FILE ADC

#Region "GESTIONE IMPORT DISABILI DA FILE"

    Public Sub CaricaCausaliAccompagno()
        Dim AlboEnte As String = ClsUtility.TrovaAlboEnte(Session("IdEnte"), Session("conn"))

        Dim strSQL As String
        Dim dsCausaliAccompagno As SqlDataReader

        strSQL = "Select IdAssociaAmbitoCausaleAccompagno,Causale,Codice "
        strSQL &= " From AssociaAmbitiCausaliAccompagno "
        strSQL &= " order by 1"

        dsCausaliAccompagno = ClsServer.CreaDatareader(strSQL, Session("conn"))
        Response.Write("<fieldset>")
        Response.Write("<ul>")
        Do While dsCausaliAccompagno.Read
            Response.Write("<li><strong>" & dsCausaliAccompagno.Item("Codice") & "</strong> per <strong>" & dsCausaliAccompagno.Item("Causale") & "</strong></li>")
        Loop
        Response.Write("</ul>")
        Response.Write("</fieldset>")
        dsCausaliAccompagno.Close()
        dsCausaliAccompagno = Nothing
    End Sub

    Protected Sub CmdElabora_Click(sender As Object, e As EventArgs) Handles CmdElabora.Click
        'MaintainScrollPositionOnPostBack = True
        Me.hdnIsPostbackDisabili.Value = Me.IsPostBack
        UpLoad()
    End Sub

    Private Sub UpLoad()
        '--- salvataggio del file sul server
        Dim swErr As Boolean
        swErr = False

        If txtSelFile.PostedFile.FileName.ToString <> "" Then
            Try
                NomeUnivoco = Session("IdEnte") & "_" & Session("Utente") & "_" & Year(Now) & Month(Now) & Day(Now) & Hour(Now) & Minute(Now) & Second(Now)
                Dim file As String
                Dim estensione As String
                file = LCase(txtSelFile.FileName.ToString)
                estensione = file.Substring(file.Length - 4)
                If estensione <> ".csv" Then
                    lblMessaggioErrore.Visible = True
                    lblMessaggioErrore.Text = "Selezionare il file nel formato CSV."
                    Exit Sub
                End If

                txtSelFile.PostedFile.SaveAs(Server.MapPath("upload") & "\" & NomeUnivoco & ".csv")

                CreaTabTemp()

            Catch exc As Exception
                swErr = True

                CancellaTabellaTemp()
            End Try

            If swErr = False Then

                LeggiCSVDisabili()

            End If

        Else
            lblMessaggioErrore.Visible = True
            lblMessaggioErrore.Text = "Selezionare il file da inviare."
            Exit Sub

        End If

    End Sub

    Private Sub CreaTabTemp()
        Dim strSql As String
        Dim cmdCreateTempTable As SqlClient.SqlCommand

        CancellaTabellaTemp()


        strSql = "CREATE TABLE #IMP_DISABILI (" & _
           "[CodiceFiscale] [varchar] (255) COLLATE Latin1_General_CI_AS NULL ," & _
           "[Cognome] [varchar] (255) COLLATE Latin1_General_CI_AS NULL ," & _
           "[Nome] [varchar] (255) COLLATE Latin1_General_CI_AS NULL ," & _
           "[Data] [datetime] NULL ," & _
           "[CodiceIstat] [varchar] (255) COLLATE Latin1_General_CI_AS NULL, " & _
           "[CodiceIstatResidenza] [varchar] (255) COLLATE Latin1_General_CI_AS NULL, " & _
           "[Indirizzo] [varchar] (255) COLLATE Latin1_General_CI_AS NULL ," & _
           "[Civico] [varchar] (255) COLLATE Latin1_General_CI_AS NULL ," & _
           "[Cap] [varchar] (255) COLLATE Latin1_General_CI_AS NULL ," & _
           "[Causale] [varchar] (10) COLLATE Latin1_General_CI_AS NULL, " & _
           "[IDComuneNascita] [int] NULL, " & _
           "[IDComuneResidenza] [int] NULL, " & _
           "[IdAssociaAmbitoCausaleAccompagno] [int] NULL " & _
           ") ON [PRIMARY]"




        cmdCreateTempTable = New SqlClient.SqlCommand
        cmdCreateTempTable.CommandText = strSql
        cmdCreateTempTable.Connection = Session("conn")
        cmdCreateTempTable.ExecuteNonQuery()
        cmdCreateTempTable.Dispose()
    End Sub

    Private Sub LeggiCSVDisabili()
        '--- lettura del file
        Dim Reader As StreamReader
        Dim xLinea As String
        Dim ArrCampi() As String
        Dim swErr As Boolean
        Dim AppoNote As String

        Dim AlboEnte As String
        AlboEnte = ClsUtility.TrovaAlboEnte(Session("IdEnte"), Session("conn"))


        '--- Leggo il file di input e scrivo quello di output
        Reader = New StreamReader(Server.MapPath("upload") & "\" & NomeUnivoco & ".CSV", System.Text.Encoding.Default, False)
        Writer = New StreamWriter(Server.MapPath("download") & "\" & NomeUnivoco & "_1" & ".CSV", False, System.Text.Encoding.Default)

        '--- intestazione

        xLinea = Reader.ReadLine()
        Writer.WriteLine("Note;" & xLinea)



        '--- scorro le righe
        xLinea = Reader.ReadLine()
        While (xLinea <> "")
            swErr = False
            Tot = Tot + 1
            ArrCampi = CreaArray(xLinea)

            If UBound(ArrCampi) < 10 Then
                '--- se i campi non sono tutti errore
                strNote = "Il numero delle colonne inserite è minore di quello richieste."
                swErr = True
                TotKo = TotKo + 1
            Else
                If UBound(ArrCampi) > 10 Then
                    '--- se i campi sono troppi errore
                    strNote = "Il numero delle colonne inserite è maggiore di quello richieste."
                    swErr = True
                    TotKo = TotKo + 1
                Else
                    'INIZIO CONTROLLI ADC

                    'CodiceFiscale
                    If Trim(ArrCampi(0)) = vbNullString Then
                        strNote = strNote & "Il campo CodiceFiscale e' un campo obbligatorio."
                        swErr = True
                    Else
                        If CheckEsistenzaCodFis(ArrCampi(0)) = False Then
                            strNote = strNote & "Il CodiceFiscale e' gia' presente in banca dati."
                            swErr = True
                        End If
                        If UnivocitaCodiceFiscaleFile(Trim(ArrCampi(0))) = True Then
                            strNote = strNote & "Il CodiceFiscale e' gia' presente sul File."
                            swErr = True
                        End If
                        If CongruenzaCodiceFiscaleDaFile(Trim(ArrCampi(0)), Trim(ArrCampi(1)), Trim(ArrCampi(2)), Trim(ArrCampi(3)), "F", Trim(ArrCampi(4))) = False And CongruenzaCodiceFiscaleDaFile(Trim(ArrCampi(0)), Trim(ArrCampi(1)), Trim(ArrCampi(2)), Trim(ArrCampi(3)), "M", Trim(ArrCampi(4))) = False Then
                            strNote = strNote & "CodiceFiscale non congruente con i dati inseriti."
                            swErr = True
                        End If
                    End If
                  
                    'cognome
                    If Trim(ArrCampi(1)) = vbNullString Then
                        strNote = strNote & "Il campo Cognome e' un campo obbligatorio."
                        swErr = True
                    Else
                        If Len(ArrCampi(1)) > 100 Then
                            strNote = strNote & "Il campo Cognome puo' contenere massimo 100 caratteri."
                            swErr = True
                        End If
                    End If

                    'Nome
                    If Trim(ArrCampi(2)) = vbNullString Then
                        strNote = strNote & "Il campo Nome e' un campo obbligatorio."
                        swErr = True
                    Else
                        If Len(ArrCampi(2)) > 100 Then
                            strNote = strNote & "Il campo Nome puo' contenere massimo 100 caratteri."
                            swErr = True
                        End If
                    End If

                    'DataNascita
                    If Trim(ArrCampi(3)) = vbNullString Then
                        strNote = strNote & "Il campo DataNascita e' un campo obbligatorio."
                        swErr = True
                    Else
                        If IsDate(ArrCampi(3)) = False Then
                            strNote = strNote & "Il campo DataNascita non e' nel formato corretto."
                            swErr = True
                        End If
                    End If

                    'ComuneNascita
                    If Trim(ArrCampi(4)) = vbNullString Then
                        strNote = strNote & "Il campo CodiceISTATComuneNascita e' un campo obbligatorio."
                        swErr = True
                    Else
                        idComuneNascita = VerificaComuneNascita(Trim(ArrCampi(4)))
                        If idComuneNascita = 0 Then
                            strNote = strNote & "Il CodiceISTATComuneNascita inserito non esiste."
                            swErr = True
                        End If
                    End If

                    'comune Residenza
                    If Trim(ArrCampi(5)) = vbNullString Then
                        strNote = strNote & "Il campo CodiceISTATComuneResidenza e' un campo obbligatorio."
                        swErr = True
                    Else
                        'se ce la residenza la controllo
                        If VerificaResidenza(Trim(ArrCampi(5))) = False Then
                            strNote = strNote & "Il CodiceISTATComuneResidenza inserito non esiste."
                            swErr = True
                            'controllo anche se in errore il cap 
                            'indirizzo
                            If Trim(ArrCampi(6)) = vbNullString Then
                                strNote = strNote & "Il campo Indirizzo e' un campo obbligatorio."
                                swErr = True
                            End If
                            'civico
                            If Trim(ArrCampi(7)) = vbNullString Then
                                strNote = strNote & "Il campo Civico e' un campo obbligatorio."
                                swErr = True
                            End If

                            'cap
                            If Trim(ArrCampi(8)) = vbNullString Then
                                strNote = strNote & "Il campo Cap e' un campo obbligatorio."
                                swErr = True
                            End If

                        Else
                            'esiste istat residenza
                            'dim idcomuneRes As String
                            idcomuneRes = trovaidcomune(Trim(ArrCampi(5)))

                            'se idcomune non trovato =0 
                            If idcomuneRes = "0" Then
                                strNote = strNote & "Il CodiceISTATComuneResidenza non produce il comune di riferimento."
                                swErr = True
                            Else
                                'verifico se e' multicap
                                If verificamulticap(idcomuneRes) = True Then
                                    'indirizzo
                                    If Trim(ArrCampi(6)) = vbNullString Then
                                        strNote = strNote & "Il campo Indirizzo e' un campo obbligatorio."
                                        swErr = True
                                    Else
                                        If verificaindirizzo(idcomuneRes, Trim(ArrCampi(6))) = True Then
                                            'civico
                                            If Trim(ArrCampi(7)) = vbNullString Then
                                                strNote = strNote & "Il campo Civico e' un campo obbligatorio."
                                                swErr = True
                                            End If

                                            'cap
                                            If Trim(ArrCampi(8)) = vbNullString Then
                                                strNote = strNote & "Il campo Cap e' un campo obbligatorio."
                                                swErr = True
                                            Else
                                                If VerificaCap(idcomuneRes, Trim(ArrCampi(6)), Trim(ArrCampi(8)), Trim(ArrCampi(7))) = False Then
                                                    strNote = strNote & "Il campo Cap indicato non esiste."
                                                    swErr = True
                                                End If
                                            End If
                                        Else
                                            strNote = strNote & "L'indirizzo non è presente in banca dati per il comune indicato. Utilizzare la funzione 'Ricerca Cap/Indirizzi' nel menu 'Utilità' per trovare l'indirizzo corretto."
                                            swErr = True
                                        End If

                                    End If

                                Else
                                    ' non e' multicap
                                    If VerificaResidenza(Trim(ArrCampi(5))) = False Then
                                        strNote = strNote & "Il CodiceISTATComuneResidenza inserito non esiste."
                                        swErr = True
                                    End If

                                    'indirizzo
                                    If Trim(ArrCampi(6)) = vbNullString Then
                                        strNote = strNote & "Il campo Indirizzo e' un campo obbligatorio."
                                        swErr = True
                                    End If
                                    'civico
                                    If Trim(ArrCampi(7)) = vbNullString Then
                                        strNote = strNote & "Il campo Civico e' un campo obbligatorio."
                                        swErr = True
                                    Else
                                        'verificare Civico formato Generico???
                                    End If

                                    'cap
                                    If Trim(ArrCampi(8)) = vbNullString Then
                                        strNote = strNote & "Il campo Cap e' un campo obbligatorio."
                                        swErr = True
                                    Else
                                        If ClsUtility.ControllaEsistenzaCap(Session("conn"), Trim(ArrCampi(8)), idcomuneRes, "") = False Then
                                            strNote = strNote & "Il campo Cap indicato non esiste."
                                            swErr = True
                                        End If
                                    End If
                                End If
                            End If
                        End If
                    End If


                    'causale
                    If Trim(ArrCampi(9)) = vbNullString Then
                        strNote = strNote & "Il campo Causale e' un campo obbligatorio."
                        swErr = True
                    Else
                        Dim causale As String = Trim(Replace(ArrCampi(9), " ", ""))
                        IdAssociaAmbitoCausaleAccompagno = Verificacausali(causale)
                        If IdAssociaAmbitoCausaleAccompagno = 0 Then
                            strNote = strNote & "Il campo Causale e' errato o non congruente con il progetto."
                            swErr = True
                        End If
                    End If

                    If swErr = False Then
                        ScriviTabTemp(ArrCampi, AlboEnte, idComuneNascita, idcomuneRes, IdAssociaAmbitoCausaleAccompagno)
                    Else
                        TotKo = TotKo + 1
                    End If
                End If

            End If

            Writer.WriteLine(strNote & ";" & xLinea)
            strNote = vbNullString
            xLinea = Reader.ReadLine()
        End While

        Reader.Close()
        Writer.Close()




        'Inserisco gli errori sul file di log
        'Apro il file creato precedentemente
        Reader = New StreamReader(Server.MapPath("download") & "\" & NomeUnivoco & "_1" & ".CSV", System.Text.Encoding.Default, False)
        Writer = New StreamWriter(Server.MapPath("download") & "\" & NomeUnivoco & ".CSV", False, System.Text.Encoding.Default)

        'Reinserisco l'intestazione
        xLinea = Reader.ReadLine()
        Writer.WriteLine(xLinea)

        'Ciclo gli elementi 
        xLinea = Reader.ReadLine()
        Dim BlnTrovato As Boolean
        Do While xLinea <> ""
            BlnTrovato = False

            If BlnTrovato = False Then
                Writer.WriteLine(xLinea)
            End If
            xLinea = Reader.ReadLine()
        Loop
        Reader.Close()
        Writer.Close()

        'Elimino il file log di appoggio
        Dim MyFile As System.IO.File
        MyFile.Delete(Server.MapPath("download") & "\" & NomeUnivoco & "_1" & ".CSV")




        Response.Redirect("WfrmRisultatoImportDisabili.aspx?NomeFile=" & NomeUnivoco & "&Tot=" & Tot & "&TotOk=" & TotOk & "&TotKo=" & TotKo & "&IdAmbitoAttività=" & Request.QueryString("IdAmbitoAttività") & "&IdAttivita=" & Request.QueryString("IdAttivita") & "&Modifica=1" & "&Nazionale=3")
        ''--- reindirizzo la pagina sottostante
        'Response.Write("<script>" & vbCrLf)
        'Response.Write("parent.Naviga('WfrmRisultatoImportRisorseSediEnti.aspx?NomeFile=" & NomeUnivoco & "&Tot=" & Tot & "&TotOk=" & TotOk & "&TotKo=" & TotKo & "')" & vbCrLf)
        'Response.Write("</script>")
    End Sub

    Private Function CreaArray(ByVal pLinea As String) As String()
        Dim TmpArr As String()
        Dim DefArr As String()
        Dim i As Integer
        Dim x As Integer

        TmpArr = Split(pLinea, ";")

        For i = 0 To UBound(TmpArr)
            If i = 0 Then
                ReDim DefArr(0)
            Else
                ReDim Preserve DefArr(UBound(DefArr) + 1)
            End If
            If Left(TmpArr(i), 1) = Chr(34) And Right(TmpArr(i), 1) = Chr(34) Then

                TmpArr(i) = Mid(TmpArr(i), 2, Len(TmpArr(i)) - 2)
            End If

            TmpArr(i) = TmpArr(i).Replace("""""", """")
            DefArr(UBound(DefArr)) = TmpArr(i)


        Next

        'Nel caso si tratti dell'importazione delle risorse aggiungo una colonna relativa all'Id del ruolo

        ReDim Preserve DefArr(UBound(DefArr) + 1)

        CreaArray = DefArr

    End Function

    Private Sub ScriviTabTemp(ByVal pArray() As String, ByVal AlboEnte As String, ByVal idComuneNascita As Integer, ByVal IdComuneRes As Integer, ByVal IdAssociaAmbitoCausaleAccompagno As Integer)
        '--- scrive nella tab temporanea
        Dim cmdTemp As SqlClient.SqlCommand
        Dim strsql As String



        Try

            strsql = "INSERT INTO #IMP_DISABILI " & _
                     "(CodiceFiscale, " & _
                     "Cognome, " & _
                     "Nome, " & _
                     "Data, " & _
                     "CodiceIstat, " & _
                     "CodiceIstatResidenza, " & _
                     "Indirizzo, " & _
                     "Civico, " & _
                     "Cap, " & _
                     "Causale, " & _
                      "IDComuneNascita, " & _
                      "IDComuneResidenza, " & _
                      "IdAssociaAmbitoCausaleAccompagno " & _
                     ") " & _
                     "values " & _
                     "('" & Trim(ClsServer.NoApice(pArray(0))) & "', " & _
                     "'" & Trim(ClsServer.NoApice(pArray(1))) & "', " & _
                     "'" & Trim(ClsServer.NoApice(pArray(2))) & "', " & _
                     "convert(datetime,'" & Trim(pArray(3)) & "',103), " & _
                     "'" & Trim(ClsServer.NoApice(pArray(4))) & "', " & _
                     "'" & Trim(ClsServer.NoApice(pArray(5))) & "', " & _
                     "'" & Trim(ClsServer.NoApice(pArray(6))) & "', " & _
                     "'" & Trim(ClsServer.NoApice(pArray(7))) & "', " & _
                     "'" & Trim(ClsServer.NoApice(pArray(8))) & "', " & _
                     "'" & Trim(ClsServer.NoApice(pArray(9))) & "', " & _
                     "'" & idComuneNascita & "', " & _
                     "'" & IdComuneRes & "', " & _
                     "'" & IdAssociaAmbitoCausaleAccompagno & "') "





            cmdTemp = New SqlClient.SqlCommand
            cmdTemp.CommandText = strsql
            cmdTemp.Connection = Session("conn")
            cmdTemp.ExecuteNonQuery()
            cmdTemp.Dispose()
            TotOk = TotOk + 1

        Catch exc As Exception
            strNote = "Errore generico."
            TotKo = TotKo + 1

        End Try
    End Sub

    Private Function CongruenzaCodiceFiscaleDaFile(ByVal pCodiceFiscale As String, ByVal pCognome As String, ByVal pNome As String, ByVal pDataNascita As String, ByVal pSesso As String, ByVal pComune As String) As Boolean
        '--- verifica la coerenza tra codfis e cognome, nome, data nascita) As Boolean
        '--- verifica la coerenza tra codfis e cognome, nome, data nascita

        Dim TutteLeLettere As String = "ABCDEFGHIJKLMNOPQRSTUVWXYZ"
        Dim TuttiINumeri As String = "0123456789"
        Dim TuttiGliOmocodici As String = "LMNPQRSTUV"
        Dim TutteLeVocali As String = "AEIOU"
        Dim TutteLeConsonanti As String = "BCDFGHJKLMNPQRSTVWXYZ"
        Dim CodMese As String = "ABCDEHLMPRST"
        Dim swErr As Boolean = False
        Dim Vocali As String
        Dim Consonanti As String
        Dim xCodCognome As String
        Dim xCodNome As String
        Dim tmpGiornoNascitaM As Integer
        Dim tmpGiornoNascitaF As Integer
        Dim tmpValore As String
        Dim i As Integer
        CongruenzaCodiceFiscaleDaFile = True
        pCodiceFiscale = UCase(pCodiceFiscale)
        If Len(pCodiceFiscale) <> 16 Then
            CongruenzaCodiceFiscaleDaFile = False
            Exit Function
        End If

        '--- cognome e nome stringa
        For i = 1 To 6
            If InStr(TutteLeLettere, Mid(pCodiceFiscale, i, 1)) = -1 Then
                'CongruenzaCodiceFiscale = "La sezione Cognome Nome non è nel formato corretto."
                CongruenzaCodiceFiscaleDaFile = False
                Exit Function
            End If
        Next i

        '--- anno numerico
        For i = 7 To 8
            If InStr(TuttiINumeri, Mid(pCodiceFiscale, i, 1)) = -1 Then
                If InStr(TuttiGliOmocodici, Mid(pCodiceFiscale, i, 1)) = -1 Then
                    'CongruenzaCodiceFiscale = "La sezione Anno non è nel formato corretto."
                    CongruenzaCodiceFiscaleDaFile = False
                    Exit Function
                End If
            End If
        Next i

        '--- mese stringa
        For i = 9 To 9
            If InStr(TutteLeLettere, Mid(pCodiceFiscale, i, 1)) = -1 Then
                'CongruenzaCodiceFiscale = "La sezione Mese non è nel formato corretto."
                CongruenzaCodiceFiscaleDaFile = False
                Exit Function
            End If
        Next i

        '--- giorno numerico
        For i = 10 To 11
            If InStr(TuttiINumeri, Mid(pCodiceFiscale, i, 1)) = -1 Then
                If InStr(TuttiGliOmocodici, Mid(pCodiceFiscale, i, 1)) = -1 Then
                    'CongruenzaCodiceFiscale = "La sezione Giorno non è nel formato corretto."
                    CongruenzaCodiceFiscaleDaFile = False
                    Exit Function
                End If
            End If
        Next i

        '--- primo carattere comune stringa
        For i = 12 To 12
            If InStr(TutteLeLettere, Mid(pCodiceFiscale, i, 1)) = -1 Then
                'CongruenzaCodiceFiscale = "La sezione Comune non è nel formato corretto."
                CongruenzaCodiceFiscaleDaFile = False
                Exit Function
            End If
        Next i

        '--- 3 caratteri comune numerico
        For i = 13 To 15
            If InStr(TuttiINumeri, Mid(pCodiceFiscale, i, 1)) = -1 Then
                If InStr(TuttiGliOmocodici, Mid(pCodiceFiscale, i, 1)) = -1 Then
                    'CongruenzaCodiceFiscale = "La sezione Comune  non è nel formato corretto."
                    CongruenzaCodiceFiscaleDaFile = False
                    Exit Function
                End If
            End If
        Next i

        '--- ultimo carattere di controllo stringa
        For i = 16 To 16
            If InStr(TutteLeLettere, Mid(pCodiceFiscale, i, 1)) = -1 Then
                'CongruenzaCodiceFiscale = "La sezione Carattere di Controllo non è nel formato corretto."
                CongruenzaCodiceFiscaleDaFile = False
                Exit Function
            End If
        Next i

        '--- FINE CONTROLLO FORMALE
        '--- Controllo Cognome
        If pCognome = vbNullString Then

        End If
        pCognome = UCase(pCognome)
        For i = 1 To Len(pCognome)
            If InStr(TutteLeVocali, Mid(pCognome, i, 1)) > 0 Then
                Vocali = Vocali + Mid(pCognome, i, 1)
            Else
                If InStr(TutteLeConsonanti, Mid(pCognome, i, 1)) > 0 Then
                    Consonanti = Consonanti + Mid(pCognome, i, 1)
                End If
            End If
            If Len(Consonanti) = 3 Then
                Exit For
            End If
        Next i
        If Len(Consonanti) < 3 Then
            Consonanti = Consonanti + Mid(Vocali, 1, 3 - Len(Consonanti))
            For i = Len(Consonanti) + 1 To 3
                Consonanti = Consonanti & "X"
            Next i
        End If
        xCodCognome = Consonanti

        If xCodCognome <> Mid(pCodiceFiscale, 1, 3) Then
            'errore sul cognome
            'CongruenzaCodiceFiscale = "La sezione Cognome non è congruente."
            CongruenzaCodiceFiscaleDaFile = False
            Exit Function
        End If

        '--- Controllo Nome
        Consonanti = vbNullString
        Vocali = vbNullString
        pNome = UCase(pNome)
        For i = 1 To Len(pNome)
            If InStr(TutteLeVocali, Mid(pNome, i, 1)) > 0 Then
                Vocali = Vocali + Mid(pNome, i, 1)
            Else
                If InStr(TutteLeConsonanti, Mid(pNome, i, 1)) > 0 Then
                    Consonanti = Consonanti + Mid(pNome, i, 1)
                End If
            End If
        Next i

        If Len(Consonanti) >= 4 Then
            Consonanti = Mid(Consonanti, 1, 1) + Mid(Consonanti, 3, 2)
        Else
            If Len(Consonanti) < 3 Then
                Consonanti = Consonanti + Mid(Vocali, 1, 3 - Len(Consonanti))
                For i = Len(Consonanti) + 1 To 3
                    Consonanti = Consonanti & "X"
                Next i
            End If
        End If
        xCodNome = Consonanti

        If xCodNome <> Mid(pCodiceFiscale, 4, 3) Then
            'CongruenzaCodiceFiscale = "La sezione Nome non è congruente."
            CongruenzaCodiceFiscaleDaFile = False
            Exit Function
        End If

        '--- Controllo Anno	
        tmpValore = DecodificaOmocodiciDaFile(Mid(pCodiceFiscale, 7, 1)) & DecodificaOmocodiciDaFile(Mid(pCodiceFiscale, 8, 1))
        If IsNumeric(tmpValore) = True Then
            If tmpValore <> Mid(pDataNascita, 9, 2) Then
                'CongruenzaCodiceFiscale = "La sezione Anno non è congruente."
                CongruenzaCodiceFiscaleDaFile = False
                Exit Function
            End If
        Else
            CongruenzaCodiceFiscaleDaFile = False
            Exit Function
        End If
        '--- Controllo Mese				
        If Mid(pCodiceFiscale, 9, 1) <> Mid(CodMese, Mid(pDataNascita, 4, 2), 1) Then
            'CongruenzaCodiceFiscale = "La sezione Mese non è congruente."
            CongruenzaCodiceFiscaleDaFile = False
            Exit Function
        End If


        '--- Controllo Giorno
        tmpGiornoNascitaF = Mid(pDataNascita, 1, 2) + 40
        tmpGiornoNascitaM = Mid(pDataNascita, 1, 2)

        tmpValore = DecodificaOmocodiciDaFile(Mid(pCodiceFiscale, 10, 1)) + DecodificaOmocodiciDaFile(Mid(pCodiceFiscale, 11, 1))
        If UCase(Trim(pSesso)) = "F" Then
            If IsNumeric(tmpValore) = True Then
                If CInt(tmpValore) <> tmpGiornoNascitaF Then
                    'CongruenzaCodiceFiscale = "La sezione Giorno non è congruente."
                    CongruenzaCodiceFiscaleDaFile = False
                    Exit Function
                End If
            Else
                CongruenzaCodiceFiscaleDaFile = False
                Exit Function
            End If
        Else
            If IsNumeric(tmpValore) = True Then
                If CInt(tmpValore) <> tmpGiornoNascitaM Then
                    'CongruenzaCodiceFiscale = "La sezione Giorno non è congruente."
                    CongruenzaCodiceFiscaleDaFile = False
                    Exit Function
                End If
            Else
                CongruenzaCodiceFiscaleDaFile = False
                Exit Function
            End If
        End If

        'manca controllo comune istat
        ' pComune
    End Function

    Private Function DecodificaOmocodiciDaFile(ByVal pValore As String) As String
        Dim TuttiGliOmocodici As String = "LMNPQRSTUV"

        If InStr(TuttiGliOmocodici, pValore) > 0 Then

            Select Case pValore
                Case Is = "L"
                    DecodificaOmocodiciDaFile = "0"

                Case "M"
                    DecodificaOmocodiciDaFile = "1"

                Case "N"
                    DecodificaOmocodiciDaFile = "2"

                Case "P"
                    DecodificaOmocodiciDaFile = "3"

                Case "Q"
                    DecodificaOmocodiciDaFile = "4"

                Case "R"
                    DecodificaOmocodiciDaFile = "5"

                Case "S"
                    DecodificaOmocodiciDaFile = "6"

                Case "T"
                    DecodificaOmocodiciDaFile = "7"

                Case "U"
                    DecodificaOmocodiciDaFile = "8"

                Case "V"
                    DecodificaOmocodiciDaFile = "9"

            End Select
        Else
            DecodificaOmocodiciDaFile = pValore
        End If

    End Function

    Private Function UnivocitaCodiceFiscaleFile(ByVal pCodiceFiscale As String) As Boolean
        Dim dtrCodiceFiscale As Data.SqlClient.SqlDataReader

        strSql = "  SELECT count(*) Conta  FROM #IMP_DISABILI " & _
                 " WHERE CodiceFiscale = '" & pCodiceFiscale & "'"

        dtrCodiceFiscale = ClsServer.CreaDatareader(strSql, Session("conn"))

        dtrCodiceFiscale.Read()

        If dtrCodiceFiscale("Conta") > 0 Then
            UnivocitaCodiceFiscaleFile = True
        End If

        dtrCodiceFiscale.Close()

    End Function

    Private Function VerificaComuneNascita(ByVal pCodiceISTAT As String) As Integer

        Dim dtrComuni As SqlClient.SqlDataReader
        Dim strSql As String
        Dim IdComune As Integer
        strSql = " Select IdComune from Comuni " & _
                 " where (CodiceISTAT= '" & ClsServer.NoApice(pCodiceISTAT) & "' OR CodiceIstatDismesso='" & ClsServer.NoApice(pCodiceISTAT) & "')"
        dtrComuni = ClsServer.CreaDatareader(strSql, Session("conn"))
        dtrComuni.Read()
        If dtrComuni.HasRows Then

            IdComune = dtrComuni("IdComune")
            dtrComuni.Close()
            dtrComuni = Nothing
            Return IdComune
        Else
            dtrComuni.Close()
            dtrComuni = Nothing
            Return 0
        End If
        VerificaComuneNascita = dtrComuni.HasRows


        dtrComuni.Close()
        dtrComuni = Nothing

    End Function

    Private Function VerificaResidenza(ByVal pCodiceIstatComuneRes As String) As Boolean
        If Not dataReader Is Nothing Then
            dataReader.Close()
            dataReader = Nothing
        End If
        Dim dtrComuni As SqlClient.SqlDataReader
        Dim strSql As String

        strSql = " Select IdComune from Comuni " & _
                 " where (CodiceISTAT= '" & ClsServer.NoApice(pCodiceIstatComuneRes) & "')"
        dtrComuni = ClsServer.CreaDatareader(strSql, Session("conn"))
        dtrComuni.Read()
        If dtrComuni.HasRows = True Then
            ChiudiDataReader(dataReader)
            If Not dtrComuni Is Nothing Then
                dtrComuni.Close()
                dtrComuni = Nothing
            End If
            Return True
        Else
            ChiudiDataReader(dataReader)
            If Not dtrComuni Is Nothing Then
                dtrComuni.Close()
                dtrComuni = Nothing
            End If
            Return False
        End If

        If Not dtrComuni Is Nothing Then
            dtrComuni.Close()
            dtrComuni = Nothing
        End If
        ChiudiDataReader(dataReader)

    End Function

    Private Function trovaidcomune(ByVal pCodiceIstatComuneRes As String) As Integer
        ChiudiDataReader(dataReader)
        Dim idcomune As String = "0"
        Dim dtrComuni As SqlClient.SqlDataReader
        Dim strSql As String

        strSql = " Select IdComune from Comuni " & _
                 " where (CodiceISTAT= '" & ClsServer.NoApice(pCodiceIstatComuneRes) & "')"
        dtrComuni = ClsServer.CreaDatareader(strSql, Session("conn"))
        dtrComuni.Read()
        If dtrComuni.HasRows = True Then
            idcomune = dtrComuni("idcomune")
        End If
        If Not dtrComuni Is Nothing Then
            dtrComuni.Close()
            dtrComuni = Nothing
        End If

        ChiudiDataReader(dataReader)
        Return idcomune



    End Function

    Private Function verificamulticap(ByVal IdComuneRes As String) As Boolean

        ChiudiDataReader(dataReader)

        strSql = "select distinct (idComune) from VW_ValidazioneIndirizzo where idcomune='" & IdComuneRes & "'"
        dataReader = ClsServer.CreaDatareader(strSql, Session("Conn"))
        dataReader.Read()
        If dataReader.HasRows = True Then
            ChiudiDataReader(dataReader)
            Return True
        End If

        ChiudiDataReader(dataReader)
        Return False
    End Function

    Private Function verificaindirizzo(ByVal IdComuneRes As String, ByVal indirizzoRes As String) As Boolean
        ChiudiDataReader(dataReader)
        strSql = "select * from VW_ValidazioneIndirizzo where idcomune='" & IdComuneRes & "' and indirizzo='" & indirizzoRes & "'"
        dataReader = ClsServer.CreaDatareader(strSql, Session("Conn"))
        dataReader.Read()
        If dataReader.HasRows = True Then
            ChiudiDataReader(dataReader)
            Return True
        End If

        ChiudiDataReader(dataReader)

    End Function

    Private Function VerificaCap(ByVal IdComuneRes As String, ByVal IndirizzoRes As String, ByVal CapRes As String, ByVal CivicoRes As String)

        Dim dtrTrovaProviciaDB As SqlClient.SqlDataReader
        Dim dtrTrovaProvinciaClient As SqlClient.SqlDataReader
        Dim IntProvinciaDB As Integer
        Dim IntComuneDB As Integer
        Dim ProvinciaDB As String
        Dim ComuneDB As String
        Dim blnProvincia As Boolean
        Dim IntComuneClient As Integer
        Dim IntProvinciaClient As Integer
        Dim ProvinciaClient As String
        Dim ComuneClient As String
        blnProvincia = False

        ChiudiDataReader(dataReader)

        Dim strMiaCausale As String = ""
        If ClsUtility.CAP_VERIFICA(Session("Conn"), strMiaCausale, False, CapRes, IdComuneRes, "", "", IndirizzoRes, CivicoRes) = False Then

            ChiudiDataReader(dataReader)
            Return False
        Else
            ChiudiDataReader(dataReader)
            Return True
        End If

    End Function

    Private Function Verificacausali(ByVal pValoreCodiceCusale As String) As Integer
        Dim dataReaderTemp As SqlClient.SqlDataReader
        Dim query As String
        Verificacausali = True

        query = "SELECT  AssociaAmbitiCausaliAccompagno.IdAssociaAmbitoCausaleAccompagno"
        query = query & " FROM AssociaAmbitiCausaliAccompagno "
        query = query & " INNER JOIN ambitiattività ON AssociaAmbitiCausaliAccompagno.IdAmbitoAttività = ambitiattività.IDAmbitoAttività "
        query = query & " INNER JOIN attività ON attività.IdAmbitoAttività = ambitiattività.IDAmbitoAttività "
        query = query & " WHERE attività.IDAttività=" & Request.QueryString("IdAttivita") & " and AssociaAmbitiCausaliAccompagno.Codice='" & Trim(pValoreCodiceCusale) & "'"
        dataReaderTemp = ClsServer.CreaDatareader(query, Session("conn"))
        'se ci sono dei record
        dataReaderTemp.Read()
        If dataReaderTemp.HasRows = False Then
            Verificacausali = 0
        Else
            Verificacausali = dataReaderTemp("IdAssociaAmbitoCausaleAccompagno")
        End If
        ChiudiDataReader(dataReaderTemp)


    End Function

    Private Sub CancellaTabellaTemp()
        Dim strSql As String
        Dim cmdCanTempTable As SqlClient.SqlCommand
        Try

            strSql = "DROP TABLE #IMP_DISABILI"
            cmdCanTempTable = New SqlClient.SqlCommand
            cmdCanTempTable.CommandText = strSql
            cmdCanTempTable.Connection = Session("conn")
            cmdCanTempTable.ExecuteNonQuery()
        Catch e As Exception
        End Try

        cmdCanTempTable.Dispose()
    End Sub

    'FINE GESTIONE IMPORTO DA FILE ADC
#End Region
End Class