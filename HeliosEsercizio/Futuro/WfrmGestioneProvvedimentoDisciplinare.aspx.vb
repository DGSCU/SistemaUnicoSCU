Imports System.IO
Imports System.Data
Imports System.Drawing


Public Class WfrmGestioneProvvedimentoDisciplinare
    Inherits System.Web.UI.Page
    Dim IDProvvedimentoDisciplinare As Integer
    Dim Provv As New clsProvvedimentoDisciplinare
#Region "Utility"
    Private Sub ChiudiDataReader(ByRef dataReader As SqlClient.SqlDataReader)
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
#Region "PersonalizzazioneCampiMaschera"
    Private Sub Fascicolo(ByVal blnValore As Boolean, ByVal Colore As Color)
        TxtCodiceFascicolo.BackColor = Colore
        txtDescFasc.BackColor = Colore
        TxtCodiceFascicolo.Enabled = blnValore
        txtDescFasc.Enabled = blnValore
    End Sub
    Private Sub ImageFascicolo(ByVal blnValore As Boolean)
        cmdSelFascicolo.Visible = blnValore
        cmdSelProtocollo0.Visible = blnValore
        cmdFascCanc.Visible = blnValore
    End Sub

    Private Sub ProtocolloComunicazione(ByVal blnValore As Boolean, ByVal Colore As Color)
        TxtNumeroProtocolloComunicazione.BackColor = Colore
        TxtDataProtocolloComunicazione.BackColor = Colore
    End Sub
    Private Sub ImageProtocolloComunicazione(ByVal blnValore As Boolean)
        imgSelezionaProtComunicazione.Visible = blnValore
        imgAllegatoProtComunicazione.Visible = blnValore
        imgCancProtComunicazione.Visible = blnValore
    End Sub

    Private Sub ProtocolloAvvioProvvedimento(ByVal blnValore As Boolean, ByVal Colore As Color)
        TxtNumeroProtocolloAvvioProcedimento.BackColor = Colore
        txtDataProtocolloAvvioProcedimento.BackColor = Colore
    End Sub
    Private Sub ImageProtocolloAvvioProcedimento(ByVal blnValore As Boolean)
        ImgProtocolloAvvioProcedimento.Visible = blnValore
        ImgApriAllegatiAvvioProcedimento.Visible = blnValore
        imgCancAvvioProcedimento.Visible = blnValore
    End Sub

    Private Sub ProtocolloControdeduzioni(ByVal blnValore As Boolean, ByVal Colore As Color)
        TxtNumeroProtocolloControdeduzioni.Enabled = blnValore
        txtDataProtocolloControdeduzioni.Enabled = blnValore
    End Sub
    Private Sub ImageProtocolloControdeduzioni(ByVal blnValore As Boolean)
        ImgProtocolloControdeduzioni.Visible = blnValore
        ImgApriAllegatiControdeduzioni.Visible = blnValore
        imgCancControdeduzioni.Visible = blnValore
    End Sub

    Private Sub ProtocolloChiusuraProvvediemento(ByVal blnValore As Boolean, ByVal Colore As Color)
        txtNProtocolloChiusuraProvvedimento.Enabled = blnValore
        txtDataProtocolloChiusuraProvvedimento.Enabled = blnValore
    End Sub
    Private Sub ImageProtocolloChiusuraProvvedimento(ByVal blnValore As Boolean)
        ImgProtocolloChiusuraProvvedimento.Visible = blnValore
        ImgApriAllegatiChiusuraProvvedimento.Visible = blnValore
        imgCancChiusuraProvvedimento.Visible = blnValore
    End Sub

    Private Sub ProtocolloSanzioni(ByVal blnValore As Boolean, ByVal Colore As Color)
        txtDataProtocolloSanzione.BackColor = Colore
        txtNProtocolloSanzione.BackColor = Colore
    End Sub
    Private Sub ImageProtocolloSanzione(ByVal blnValore As Boolean)
        ImgProtocolloSanzione.Visible = blnValore
        ImgApriAllegatiSanzione.Visible = blnValore
        imgCancSanzione.Visible = blnValore
    End Sub

    Private Function FasciocoloCompleto(ByVal blnValore As Boolean, ByVal Colore As Color)
        Fascicolo(blnValore, Colore)
        ImageFascicolo(blnValore)
    End Function
    Private Function Comunicazione(ByVal blnValore As Boolean, ByVal Colore As Color)
        ProtocolloComunicazione(blnValore, Colore)
        ImageProtocolloComunicazione(blnValore)
    End Function
    Private Function AvvioProvvedimento(ByVal blnValore As Boolean, ByVal Colore As Color)
        ProtocolloAvvioProvvedimento(blnValore, Colore)
        ImageProtocolloAvvioProcedimento(blnValore)
    End Function
    Private Function Controdeduzioni(ByVal blnValore As Boolean, ByVal Colore As Color)
        ProtocolloControdeduzioni(blnValore, Colore)
        ImageProtocolloControdeduzioni(blnValore)
    End Function
    Private Function ChiusaraProvvedimento(ByVal blnValore As Boolean, ByVal Colore As Color)
        ProtocolloChiusuraProvvediemento(blnValore, Colore)
        ImageProtocolloChiusuraProvvedimento(blnValore)
    End Function
    Private Function Sanzione(ByVal blnValore As Boolean, ByVal Colore As Color)
        ProtocolloSanzioni(blnValore, Colore)
        ImageProtocolloSanzione(blnValore)
    End Function

    Private Sub PulisciFascicoloProtocolli()
        TxtCodiceFasc.Value = ""
        TxtCodiceFascicolo.Text = ""
        txtDescFasc.Text = ""
        TxtNumeroProtocolloComunicazione.Text = ""
        TxtDataProtocolloComunicazione.Text = ""
        TxtNumeroProtocolloAvvioProcedimento.Text = ""
        txtDataProtocolloAvvioProcedimento.Text = ""
        TxtNumeroProtocolloControdeduzioni.Text = ""
        txtDataProtocolloControdeduzioni.Text = ""
        txtNProtocolloChiusuraProvvedimento.Text = ""
        txtDataProtocolloChiusuraProvvedimento.Text = ""
        txtDataProtocolloSanzione.Text = ""
        txtNProtocolloSanzione.Text = ""
    End Sub
#End Region

#Region "LoadMaschera_Inserimento_Modifica"
    Private Sub LoadMascheraInserimento(ByVal IDAttivitàEntità As Integer, ByVal IDEntità As Integer)
        Dim strSql As String
        Dim dtrLoad As SqlClient.SqlDataReader

        'Creata da Simona Cordella il 19/03/2018
        Try
            ChiudiDataReader(dtrLoad)
            strSql = " SELECT DISTINCT e.IDEntità,e.CodiceVolontario,e.cognome + ' ' + e.nome as nominativo,e.IDStatoEntità,s.StatoEntità,"
            strSql &= " dbo.formatodata(ae.DataInizioAttivitàEntità) as DataInizio,"
            strSql &= " dbo.formatodata(ae.DataFineAttivitàEntità) as DataFine,"
            strSql &= " a.CodiceEnte as CodiceProgetto,a.Titolo,enti.idente,"
            strSql &= " enti.Denominazione +' (' + enti.CodiceRegione + ')' as Ente,"
            strSql &= " enteProg.Denominazione + ' (' + enteProg.CodiceRegione + ')' as EnteProg,"
            strSql &= " esa.IDEnteSedeAttuazione as CodiceSede,s.StatoEntità, "
            strSql &= " es.Denominazione as Sede,c.Denominazione +' (' + p.DescrAbb + ')' as Comune,ae.IDAttivitàEntità"
            strSql &= " FROM Entità e "
            strSql &= " INNER JOIN attivitàentità ae on e.IDEntità =ae.IDEntità"
            strSql &= " INNER JOIN attivitàentisediattuazione aes on ae.IDAttivitàEnteSedeAttuazione= aes.IDAttivitàEnteSedeAttuazione"
            strSql &= " INNER JOIN entisediattuazioni esa on esa.IDEnteSedeAttuazione=aes.IDEnteSedeAttuazione"
            strSql &= " INNER JOIN entisedi es on es.IDEnteSede= esa.IDEnteSede"
            strSql &= " INNER JOIN enti on enti.idente=es.IDEnte"
            strSql &= " INNER JOIN comuni c on es.IDComune=c.IDComune"
            strSql &= " INNER JOIN provincie p on p.IDProvincia =c.IDProvincia"
            strSql &= " INNER JOIN attività a on a.IDAttività = aes.IDAttività"
            'strSql &= " INNER JOIN TIPIPROGETTO ON a.idtipoprogetto=TIPIPROGETTO.idtipoprogetto TipiProgetto.MacroTipoProgetto like 'SCN%'"
            strSql &= " INNER JOIN StatiEntità s on s.IDStatoEntità=e.IDStatoEntità"
            strSql &= " INNER JOIN enti enteProg on enteProg.idente=a.IDEntePresentante "
            strSql &= " WHERE ae.IDAttivitàEntità = " & IDAttivitàEntità & " and ae.IDEntità=" & IDEntità

            dtrLoad = ClsServer.CreaDatareader(strSql, Session("conn"))

            If dtrLoad.HasRows = True Then
                dtrLoad.Read()
                lblDatoStatoProvedimento.Text = "DA REGISTRARE"
                lblDatoVolontario.Text = dtrLoad("CodiceVolontario") & " - " & dtrLoad("Nominativo")
                LblDatoStatoVolontario.Text = dtrLoad("StatoEntità")
                lblDatoDataInzioServizio.Text = dtrLoad("DataInizio")
                lblDatoDataFineServizio.Text = dtrLoad("DataFine")
                lblDatoSede.Text = dtrLoad("CodiceSede") & " - " & dtrLoad("Sede") & " - " & dtrLoad("Comune")
                LblDatoProgetto.Text = dtrLoad("CodiceProgetto") & " - " & dtrLoad("Titolo")
                lblDatoEnte.Text = dtrLoad("Ente")
                lblDatoEnteProg.Text = dtrLoad("EnteProg")
            End If
            ChiudiDataReader(dtrLoad)
        Catch ex As Exception
            Response.Write(ex.Message.ToString())
            Exit Sub
        End Try
    End Sub
    Private Sub LoadMascheraModifica(ByVal IdProvvedimentoDisciplinare As Integer)
        Dim strSql As String
        Dim dtrLoad As SqlClient.SqlDataReader

        'Creata da Simona Cordella il 19/03/2018
        Try
            ChiudiDataReader(dtrLoad)
            strSql = " SELECT DISTINCT e.CodiceVolontario,e.cognome + ' ' + e.nome as nominativo, s.StatoEntità, "
            strSql &= " dbo.formatodata(ae.DataInizioAttivitàEntità) as DataInizio,"
            strSql &= " dbo.formatodata(ae.DataFineAttivitàEntità) as DataFine,"
            strSql &= " p.CodiceFascicolo as NumeroFascicolo,p.idfascicolo,p.DescrFascicolo, sp.StatoProvvedimento,p.Idprovvedimentodisciplinare,		"
            strSql &= " a.CodiceEnte as CodiceProgetto,a.Titolo, "
            strSql &= " enti.Denominazione + ' (' + enti.CodiceRegione + ')' as Ente,"
            strSql &= " enteProg.Denominazione + ' (' + enteProg.CodiceRegione + ')' as EnteProg,"
            strSql &= " esa.IDEnteSedeAttuazione as CodiceSede, "
            strSql &= " es.Denominazione as Sede,c.Denominazione +' (' + pr.DescrAbb + ')' as Comune, "
            strSql &= " p.NumeroProtocolloComunicazione, p.DataProtocolloComunicazione,"
            strSql &= " p.NumeroProtocolloAvvioProvvedimento,  p.DataProtocolloAvvioProvvedimento, "
            strSql &= " p.NumeroProtocolloControdeduzioni, p.DataProtocolloControdeduzioni, "
            strSql &= " p.NumeroProtocolloChiusuraProvvedimento, p.DataProtocolloChiusuraProvvedimento, "
            strSql &= " p.NumeroProtocolloSanzioni, p.DataProtocolloSanzioni, p.Note"
            strSql &= " FROM  ProvvedimentoDisciplinare P"
            strSql &= " INNER JOIN StatiProvvedimentoDisciplinare sp on p.IDStatoProvvedimento = sp.IDStatoProvvedimento"
            strSql &= " INNER JOIN attivitàentità ae on P.IDAttivitàEntità =ae.IDAttivitàEntità"
            strSql &= " INNER JOIN Entità e  ON ae.IDEntità =E.IDEntità"
            strSql &= " INNER JOIN attivitàentisediattuazione aes on ae.IDAttivitàEnteSedeAttuazione= aes.IDAttivitàEnteSedeAttuazione"
            strSql &= " INNER JOIN entisediattuazioni esa on esa.IDEnteSedeAttuazione=aes.IDEnteSedeAttuazione"
            strSql &= " INNER JOIN entisedi es on es.IDEnteSede= esa.IDEnteSede"
            strSql &= " INNER JOIN enti on enti.idente=es.IDEnte"
            strSql &= " INNER JOIN comuni c on es.IDComune=c.IDComune"
            strSql &= " INNER JOIN provincie pr on pr.IDProvincia =c.IDProvincia"
            strSql &= " INNER JOIN attività a on a.IDAttività = aes.IDAttività"
            strSql &= " INNER JOIN StatiEntità s on s.IDStatoEntità=e.IDStatoEntità"
            strSql &= " INNER JOIN enti enteProg on enteProg.idente=a.IDEntePresentante "
            strSql &= " WHERE p.Idprovvedimentodisciplinare= " & IdProvvedimentoDisciplinare & "  "

            dtrLoad = ClsServer.CreaDatareader(strSql, Session("conn"))

            If dtrLoad.HasRows = True Then
                dtrLoad.Read()
                lblDatoStatoProvedimento.Text = dtrLoad("StatoProvvedimento")
                TxtCodiceFascicolo.Text = dtrLoad("NumeroFascicolo")
                txtDescFasc.Text = dtrLoad("DescrFascicolo")
                TxtCodiceFasc.Value = dtrLoad("idfascicolo")
                txtIDProvvedimentoDisciplinare.Value = IdProvvedimentoDisciplinare
                lblDatoVolontario.Text = dtrLoad("CodiceVolontario") & " - " & dtrLoad("Nominativo")
                LblDatoStatoVolontario.Text = dtrLoad("StatoEntità")
                lblDatoDataInzioServizio.Text = dtrLoad("DataInizio")
                lblDatoDataFineServizio.Text = dtrLoad("DataFine")
                lblDatoSede.Text = dtrLoad("CodiceSede") & " - " & dtrLoad("Sede") & " - " & dtrLoad("Comune")
                LblDatoProgetto.Text = dtrLoad("CodiceProgetto") & " - " & dtrLoad("Titolo")
                lblDatoEnte.Text = dtrLoad("Ente")
                lblDatoEnteProg.Text = dtrLoad("EnteProg")
                TxtDataProtocolloComunicazione.Text = "" & dtrLoad("DataProtocolloComunicazione")
                TxtNumeroProtocolloComunicazione.Text = "" & dtrLoad("NumeroProtocolloComunicazione")

                txtDataProtocolloAvvioProcedimento.Text = "" & dtrLoad("DataProtocolloAvvioProvvedimento")
                TxtNumeroProtocolloAvvioProcedimento.Text = "" & dtrLoad("NumeroProtocolloAvvioProvvedimento")
                txtDataProtocolloControdeduzioni.Text = "" & dtrLoad("DataProtocolloControdeduzioni")
                TxtNumeroProtocolloControdeduzioni.Text = "" & dtrLoad("NumeroProtocolloControdeduzioni")
                txtDataProtocolloChiusuraProvvedimento.Text = "" & dtrLoad("DataProtocolloChiusuraProvvedimento")
                txtNProtocolloChiusuraProvvedimento.Text = "" & dtrLoad("NumeroProtocolloChiusuraProvvedimento")
                txtDataProtocolloSanzione.Text = "" & dtrLoad("DataProtocolloSanzioni")
                txtNProtocolloSanzione.Text = "" & dtrLoad("NumeroProtocolloSanzioni")
                TxtNote.Text = "" & dtrLoad("Note")
            End If
            ChiudiDataReader(dtrLoad)
        Catch ex As Exception
            Response.Write(ex.Message.ToString())
            Exit Sub
        End Try
    End Sub

    Private Function VerificaConferma(ByVal VendoDa As String) As Boolean
        'Dim d1 As DateTime
        'Dim d2 As DateTime


        Dim campiValidi As Boolean = True
        Dim campoObbligatorio As String = "Il campo {0} è obbligatorio.<br/>"

        lblErrore.Text = ""

        If Request.QueryString("VengoDa") = "Inserimento" Then
            If (TxtCodiceFascicolo.Text = "" And txtDescFasc.Text = "") Then
                lblErrore.Text = lblErrore.Text + String.Format(campoObbligatorio, "Fascicolo")
                campiValidi = False
            End If
            If (TxtDataProtocolloComunicazione.Text = "" And TxtNumeroProtocolloComunicazione.Text = "") Then
                lblErrore.Text = lblErrore.Text + String.Format(campoObbligatorio, "Data e Numero Protocollo Comunicazione")
                campiValidi = False
            End If
        Else

        End If

        Return campiValidi
    End Function
    Private Function ImpostaStatoProvvedimento(ByVal StatoProvvedimento As String) As Integer

        Dim ImpostaStato As Integer
        FasciocoloCompleto(False, Color.LightGray)
        Comunicazione(False, Color.LightGray)
        AvvioProvvedimento(False, Color.LightGray)
        Controdeduzioni(False, Color.LightGray)
        ChiusaraProvvedimento(False, Color.LightGray)
        Sanzione(False, Color.LightGray)
        cmdAnnulla.Visible = False
        Select Case StatoProvvedimento
            Case "DA REGISTRARE"
                'ImpostaStato = 5 'aperta
                FasciocoloCompleto(True, Color.White)
                Comunicazione(True, Color.White)
                If TxtCodiceFascicolo.Text <> "" And txtDescFasc.Text <> "" _
                        And TxtDataProtocolloComunicazione.Text <> "" And TxtNumeroProtocolloComunicazione.Text <> "" Then
                    ImpostaStato = 1 'APERTO
                    FasciocoloCompleto(False, Color.LightGray)
                    Comunicazione(False, Color.LightGray)
                End If
            Case "APERTO"
                ImpostaStato = 1
                cmdAnnulla.Visible = True
                FasciocoloCompleto(False, Color.LightGray)
                Comunicazione(False, Color.LightGray)
                AvvioProvvedimento(True, Color.White)
                Controdeduzioni(True, Color.White)
                ChiusaraProvvedimento(True, Color.White)
                Sanzione(True, Color.White)
                If txtDataProtocolloAvvioProcedimento.Text <> "" And TxtNumeroProtocolloAvvioProcedimento.Text <> "" Then
                    AvvioProvvedimento(False, Color.LightGray)
                End If
                If txtDataProtocolloControdeduzioni.Text <> "" And TxtNumeroProtocolloControdeduzioni.Text <> "" Then
                    Controdeduzioni(False, Color.LightGray)
                End If
                If txtDataProtocolloChiusuraProvvedimento.Text <> "" And txtNProtocolloChiusuraProvvedimento.Text <> "" Then
                    ImpostaStato = 2 'CHIUSO CONTESTATO
                    AvvioProvvedimento(False, Color.LightGray)
                    Controdeduzioni(False, Color.LightGray)
                    ChiusaraProvvedimento(False, Color.LightGray)
                End If
                If txtDataProtocolloSanzione.Text <> "" And txtNProtocolloSanzione.Text <> "" Then
                    ImpostaStato = 3 'SANZIONATO
                    Sanzione(False, Color.LightGray)
                End If
            Case "CHIUSO CONTESTATO"
                cmdAnnulla.Visible = False
                ImpostaStato = 2
                Sanzione(True, Color.White)
                If txtDataProtocolloSanzione.Text <> "" And txtNProtocolloSanzione.Text <> "" Then
                    ImpostaStato = 3 'SANZIONATO
                    Sanzione(False, Color.LightGray)
                End If
            Case "SANZIONATO"
                cmdSalva.Visible = False
                cmdAnnulla.Visible = False
            Case "ANNULLATO"
                cmdSalva.Visible = False
                cmdAnnulla.Visible = False
        End Select
        'ImpostaStato = intStato
        Return ImpostaStato

    End Function

    'Private Function RicavaIDProvvedimentoDisciplinare(ByVal IdAttivitàEntità As Integer) As Integer

    '    Dim strSql As String
    '    Dim dtrId As SqlClient.SqlDataReader
    '    Dim idProvDisc As Integer
    '    Try
    '        ChiudiDataReader(dtrId)
    '        strSql = "SELECT idprovvedimentodisciplinare FROM ProvvedimentoDisciplinare " & _
    '                 " where IdAttivitàEntità =" & IdAttivitàEntità & " "
    '        dtrId = ClsServer.CreaDatareader(strSql, Session("conn"))
    '        dtrId.Read()
    '        idProvDisc = dtrId("idprovvedimentodisciplinare")
    '        ChiudiDataReader(dtrId)

    '        Return idProvDisc
    '    Catch ex As Exception
    '        ChiudiDataReader(dtrId)
    '    End Try
    'End Function
    Private Function RicavaIDEnte(ByVal IdAttivitàEntità As Integer) As Integer

        Dim strSql As String
        Dim dtrId As SqlClient.SqlDataReader
        Dim idEnte As Integer
        Try
            ChiudiDataReader(dtrId)
            strSql = " SELECT A.IDEntePresentante from  attivitàentità AE "
            strSql &= " INNER JOIN attivitàentisediattuazione AES ON  AE.IDAttivitàEnteSedeAttuazione=AES.IDAttivitàEnteSedeAttuazione "
            strSql &= " INNER JOIN attività A ON A.IDAttività =AES.IDAttività "
            strSql &= " WHERE IdAttivitàEntità =" & IdAttivitàEntità & " "
            dtrId = ClsServer.CreaDatareader(strSql, Session("conn"))
            dtrId.Read()
            idEnte = dtrId("IDEntePresentante")
            ChiudiDataReader(dtrId)

            Return idEnte
        Catch ex As Exception
            ChiudiDataReader(dtrId)
        End Try
    End Function
    Private Function RicavaIDEntità(ByVal IdAttivitàEntità As Integer) As Integer

        Dim strSql As String
        Dim dtrId As SqlClient.SqlDataReader
        Dim IDEntità As Integer
        Try
            ChiudiDataReader(dtrId)
            strSql = " SELECT IDEntità from  attivitàentità  "
            strSql &= " WHERE IdAttivitàEntità =" & IdAttivitàEntità & " "
            dtrId = ClsServer.CreaDatareader(strSql, Session("conn"))
            dtrId.Read()
            IDEntità = dtrId("IDEntità")
            ChiudiDataReader(dtrId)

            Return IDEntità
        Catch ex As Exception
            ChiudiDataReader(dtrId)
        End Try
    End Function
    Private Sub AnnullaProvvedimentoDisciplinare(ByVal IdProvvedimentoDisciplinare As Integer)
        Dim strsql As String
        Dim CmdGenerico As SqlClient.SqlCommand


        strsql = " INSERT INTO CronologiaProvvedimentoDisciplinare (IDProvvedimentoDisciplinare ,IDStatoProvvedimento,UserNameStato,DataCronologia)"
        strsql &= " SELECT  IDProvvedimentoDisciplinare ,IDStatoProvvedimento,'" & Session("Utente") & "',GETDATE()"
        strsql &= " FROM ProvvedimentoDisciplinare "
        strsql &= " WHERE IDProvvedimentoDisciplinare = " & IdProvvedimentoDisciplinare
        CmdGenerico = ClsServer.EseguiSqlClient(strsql, Session("conn"))

        strsql = "update ProvvedimentoDisciplinare set  IDStatoProvvedimento = 4 where IDProvvedimentoDisciplinare = " & IdProvvedimentoDisciplinare
        CmdGenerico = ClsServer.EseguiSqlClient(strsql, Session("conn"))
    End Sub
#End Region


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Inserire qui il codice utente necessario per inizializzare la pagina
        If Not Session("LogIn") Is Nothing Then
            If Session("LogIn") = False Then 'verifico validità log-in
                Response.Redirect("LogOn.aspx")
            End If
        Else
            Response.Redirect("LogOn.aspx")
        End If
        If Page.IsPostBack = False Then

            If Request.QueryString("VengoDa") = "Inserimento" Then
                LoadMascheraInserimento(Request.QueryString("IdAttivitàEntità"), Request.QueryString("IdEntità"))

                ImpostaStatoProvvedimento(lblDatoStatoProvedimento.Text)
            Else
                txtIDProvvedimentoDisciplinare.Value = Request.QueryString("IdProvvedimentoDisciplinare")
                If Request.QueryString("IdProvvedimentoDisciplinare") = Nothing Then
                    IDProvvedimentoDisciplinare = txtIDProvvedimentoDisciplinare.Value 'RicavaIDProvvedimentoDisciplinare(Request.QueryString("IdAttivitàEntità"))
                Else
                    txtIDProvvedimentoDisciplinare.Value = Request.QueryString("IdProvvedimentoDisciplinare")
                    IDProvvedimentoDisciplinare = txtIDProvvedimentoDisciplinare.Value
                End If
                LoadMascheraModifica(IDProvvedimentoDisciplinare)
                ImpostaStatoProvvedimento(lblDatoStatoProvedimento.Text)
                If lblDatoStatoProvedimento.Text = "APERTO" Then
                    cmdAnnulla.Visible = True
                Else
                    cmdAnnulla.Visible = False
                End If
            End If

        End If
    End Sub

    Protected Sub cmdSalva_Click(sender As Object, e As EventArgs) Handles cmdSalva.Click
        If lblDatoStatoProvedimento.Text = "APERTO" Then
            Provv.VengoDa = "Modifica"
        Else
            Provv.VengoDa = Request.QueryString("VengoDa")
        End If

        If VerificaConferma(Provv.VengoDa) = False Then Exit Sub

        Provv.IDAttivitàEntità = Request.QueryString("IdAttivitàEntità")
        Provv.IDEntità = Request.QueryString("IdEntità")
        Provv.IDStatoProvvedimento = ImpostaStatoProvvedimento(lblDatoStatoProvedimento.Text)
        Provv.CodiceFascicolo = Me.TxtCodiceFascicolo.Text
        Provv.IDFascicolo = Me.TxtCodiceFasc.Value
        Provv.DescFasicolo = Me.txtDescFasc.Text
        Provv.DataProtocolloComunicazione = Me.TxtDataProtocolloComunicazione.Text
        Provv.NumeroProtocolloComunicazione = Me.TxtNumeroProtocolloComunicazione.Text
        Provv.DataProtocolloAvvioProvvedimento = Me.txtDataProtocolloAvvioProcedimento.Text
        Provv.NumeroProtocolloAvvioProvvedimento = Me.TxtNumeroProtocolloAvvioProcedimento.Text
        Provv.DataProtocolloControdeduzioni = Me.txtDataProtocolloControdeduzioni.Text
        Provv.NumeroProtocolloControdeduzioni = Me.TxtNumeroProtocolloControdeduzioni.Text
        Provv.DataProtocolloChiusuraProvvedimento = Me.txtDataProtocolloChiusuraProvvedimento.Text
        Provv.NumeroProtocolloChiusuraProvvedimento = Me.txtNProtocolloChiusuraProvvedimento.Text
        Provv.DataProtocolloSanzioni = Me.txtDataProtocolloSanzione.Text
        Provv.NumeroProtocolloSanzioni = Me.txtNProtocolloSanzione.Text
        If Provv.VengoDa = "Inserimento" Then
            Provv.UserInseritore = Session("Utente")
        Else
            If Request.QueryString("IdProvvedimentoDisciplinare") = Nothing Then
                Provv.IDProvvedimentoDisciplinare = txtIDProvvedimentoDisciplinare.Value ' RicavaIDProvvedimentoDisciplinare(Provv.IDAttivitàEntità)
            Else
                Provv.IDProvvedimentoDisciplinare = Request.QueryString("IdProvvedimentoDisciplinare")
                txtIDProvvedimentoDisciplinare.Value = Request.QueryString("IdProvvedimentoDisciplinare")
            End If
            Provv.UserUltimaModifica = Session("Utente")
        End If


        Provv.Note = Me.TxtNote.Text

        Dim ReturnIdProvvDisciplinare As Integer
        If Provv.VengoDa = "Inserimento" Then
            If Provv.Inserisci(Session("conn"), ReturnIdProvvDisciplinare) Then
                ' clsGui.SvuotaCampi(Me)
                'Me.lblmessaggio.Text = "INSERIMENTO EFFETTUATO."
                'Me.lblmessaggio.Visible = True
                ' Me.cmdSalva.Visible = False
                'lblDatoStatoProvedimento.Text = "APERTO"
                txtIDProvvedimentoDisciplinare.Value = ReturnIdProvvDisciplinare
                Provv.IDProvvedimentoDisciplinare = txtIDProvvedimentoDisciplinare.Value 'RicavaIDProvvedimentoDisciplinare(Provv.IDAttivitàEntità)
                Provv.VengoDa = "Modifica"
                LoadMascheraModifica(Provv.IDProvvedimentoDisciplinare)
                ImpostaStatoProvvedimento(lblDatoStatoProvedimento.Text)
                'If lblDatoStatoProvedimento.Text = "APERTO" Then
                '    cmdAnnulla.Visible = True
                'Else
                '    cmdAnnulla.Visible = False
                'End If
            Else
                Me.lblErrore.Text = "ATTENZIONE INSERIMENTO FALLITO."
                Me.lblErrore.Visible = True
            End If
        Else
            If Provv.Modifica(Session("conn")) Then
                ' clsGui.SvuotaCampi(Me)
                Me.lblmessaggio.Text = "MODIFICA EFFETTUATA."
                Me.lblmessaggio.Visible = True
                LoadMascheraModifica(Provv.IDProvvedimentoDisciplinare)
                ImpostaStatoProvvedimento(lblDatoStatoProvedimento.Text)
            Else
                Me.lblErrore.Text = "ATTENZIONE MODIFICA FALLITA."
                Me.lblErrore.Visible = True
            End If
        End If
    End Sub

    Protected Sub cmdChiudi_Click(sender As Object, e As EventArgs) Handles cmdChiudi.Click
        If Request.QueryString("VengoDa") = "Modifica" Then
            Response.Redirect("WfrmRicercaProvvedimentoDisciplinare.aspx?VengoDa= " & Request.QueryString("VengoDa"))
        Else
            Response.Redirect("WfrmMain.aspx")
        End If
    End Sub

    Protected Sub cmdAnnulla_Click(sender As Object, e As EventArgs) Handles cmdAnnulla.Click
        Dim intIdProvvedimento As Integer
        Try
            If Request.QueryString("VengoDa") = "Inserimento" Then
                intIdProvvedimento = txtIDProvvedimentoDisciplinare.Value ' RicavaIDProvvedimentoDisciplinare(Request.QueryString("IdAttivitàEntità"))
                Provv.IDProvvedimentoDisciplinare = intIdProvvedimento
            Else
                Provv.IDProvvedimentoDisciplinare = Request.QueryString("IdProvvedimentoDisciplinare")
            End If
            AnnullaProvvedimentoDisciplinare(Provv.IDProvvedimentoDisciplinare)

            Me.lblmessaggio.Text = "PROVVEDIMENTO DISCIPLINARE ANNULLATO."
            Me.lblmessaggio.Visible = True

            LoadMascheraModifica(Provv.IDProvvedimentoDisciplinare)
            ImpostaStatoProvvedimento(lblDatoStatoProvedimento.Text)
            cmdAnnulla.Visible = False
            cmdSalva.Visible = False
        Catch ex As Exception

        End Try

    End Sub

    Protected Sub cmdFascCanc_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles cmdFascCanc.Click
        PulisciFascicoloProtocolli()
    End Sub

    Protected Sub imgCancProtComunicazione_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles imgCancProtComunicazione.Click
        If TxtDataProtocolloComunicazione.Text <> "" Then
            TxtDataProtocolloComunicazione.Text = ""
            TxtNumeroProtocolloComunicazione.Text = ""
        End If
    End Sub

    Protected Sub imgCancAvvioProcedimento_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles imgCancAvvioProcedimento.Click
        If txtDataProtocolloAvvioProcedimento.Text <> "" Then
            txtDataProtocolloAvvioProcedimento.Text = ""
            TxtNumeroProtocolloAvvioProcedimento.Text = ""
        End If
    End Sub

    Protected Sub imgCancControdeduzioni_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles imgCancControdeduzioni.Click
        If txtDataProtocolloControdeduzioni.Text <> "" Then
            txtDataProtocolloControdeduzioni.Text = ""
            TxtNumeroProtocolloControdeduzioni.Text = ""
        End If
    End Sub

    Protected Sub imgCancChiusuraProvvedimento_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles imgCancChiusuraProvvedimento.Click
        If txtDataProtocolloChiusuraProvvedimento.Text <> "" Then
            txtDataProtocolloChiusuraProvvedimento.Text = ""
            txtNProtocolloChiusuraProvvedimento.Text = ""
        End If
    End Sub

    Protected Sub imgCancSanzione_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles imgCancSanzione.Click
        If txtDataProtocolloSanzione.Text <> "" Then
            txtDataProtocolloSanzione.Text = ""
            txtNProtocolloSanzione.Text = ""
        End If
    End Sub

    Protected Sub cmdLetteraAvvio_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles cmdLetteraAvvio.Click
        Dim Documento As New GeneratoreModelli
        Dim IdEnte As Integer
        Dim IDProvvedimentoDisciplinare As Integer
        Dim IdEntità As Integer

        IDProvvedimentoDisciplinare = txtIDProvvedimentoDisciplinare.Value ' RicavaIDProvvedimentoDisciplinare(Request.QueryString("IdAttivitàEntità"))
        IdEnte = RicavaIDEnte(Request.QueryString("IdAttivitàEntità"))
        IdEntità = RicavaIDEntità(Request.QueryString("IdAttivitàEntità"))
        'LETTERA AVVIO PROCEDIMENTO
        Response.Write("<SCRIPT>" & vbCrLf)
        ' Response.Write("window.open('" & Documento.MON_letteraarchiviazproced(ClsUtility.TrovaIDEnteDaIDVerifica(Request.QueryString("IdVerifica"), Session("conn")), Request.QueryString("IdVerifica"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn")) & "');" & vbCrLf)
        Response.Write("window.open('" & Documento.MON_AvvioProvvedimentoDisciplinare(IDProvvedimentoDisciplinare, IdEnte, IdEntità, Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn")) & "');" & vbCrLf)
        Response.Write("</SCRIPT>")
        'chiamo la proprietà della classe GeneratoreModelli che ritorna la path del documento creato
        Documento.Dispose()
    End Sub

    Protected Sub cmdLetteraChiusura_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles cmdLetteraChiusura.Click
        Dim Documento As New GeneratoreModelli
        Dim IdEnte As Integer
        Dim IDProvvedimentoDisciplinare As Integer
        Dim IdEntità As Integer
        IDProvvedimentoDisciplinare = txtIDProvvedimentoDisciplinare.Value 'RicavaIDProvvedimentoDisciplinare(Request.QueryString("IdAttivitàEntità"))
        IdEnte = RicavaIDEnte(Request.QueryString("IdAttivitàEntità"))
        IdEntità = RicavaIDEntità(Request.QueryString("IdAttivitàEntità"))
        'LETTERA CHIUSURA PROCEDIMENTO
        Response.Write("<SCRIPT>" & vbCrLf)
        ' Response.Write("window.open('" & Documento.MON_letteraarchiviazproced(ClsUtility.TrovaIDEnteDaIDVerifica(Request.QueryString("IdVerifica"), Session("conn")), Request.QueryString("IdVerifica"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn")) & "');" & vbCrLf)
        Response.Write("window.open('" & Documento.MON_ChiusuraProvvedimentoDisciplinare(IDProvvedimentoDisciplinare, IdEnte, IdEntità, Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn")) & "');" & vbCrLf)
        Response.Write("</SCRIPT>")
        'chiamo la proprietà della classe GeneratoreModelli che ritorna la path del documento creato
        Documento.Dispose()
    End Sub

    Protected Sub cmdLetteraSanzione_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles cmdLetteraSanzione.Click
        Dim Documento As New GeneratoreModelli
        Dim IdEnte As Integer
        Dim IDProvvedimentoDisciplinare As Integer
        Dim IdEntità As Integer
        IDProvvedimentoDisciplinare = txtIDProvvedimentoDisciplinare.Value ' RicavaIDProvvedimentoDisciplinare(Request.QueryString("IdAttivitàEntità"))
        IdEnte = RicavaIDEnte(Request.QueryString("IdAttivitàEntità"))
        IdEntità = RicavaIDEntità(Request.QueryString("IdAttivitàEntità"))
        'LETTERA sanzione
        Response.Write("<SCRIPT>" & vbCrLf)
        ' Response.Write("window.open('" & Documento.MON_letteraarchiviazproced(ClsUtility.TrovaIDEnteDaIDVerifica(Request.QueryString("IdVerifica"), Session("conn")), Request.QueryString("IdVerifica"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn")) & "');" & vbCrLf)
        Response.Write("window.open('" & Documento.MON_SanzioneProvvedimentoDisciplinare(IDProvvedimentoDisciplinare, IdEnte, IdEntità, Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn")) & "');" & vbCrLf)
        Response.Write("</SCRIPT>")
        'chiamo la proprietà della classe GeneratoreModelli che ritorna la path del documento creato
        Documento.Dispose()
    End Sub
End Class