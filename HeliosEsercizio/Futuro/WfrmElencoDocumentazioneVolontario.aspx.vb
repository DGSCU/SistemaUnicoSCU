Imports System.Data.SqlClient
Imports System.IO


Public Class WfrmElencoDocumentazioneVolontario
    Inherits System.Web.UI.Page
    Dim PROGETTO_GARANZIA_GIOVANI As String = "4"


    ''' <summary>
    ''' PATH del documento "CertificatoServizioSvolto.docx"
    ''' </summary>
    ''' <returns>String</returns>
    Public ReadOnly Property PATH_DOC_CertificatoServizioSvolto As String
        Get
            Return Server.MapPath("\download\Master\Volontari\CertificatoServizioSvolto.docx")
        End Get
    End Property


#Region " Codice generato da Progettazione Web Form "

    'Chiamata richiesta da Progettazione Web Form.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    'NOTA: la seguente dichiarazione è richiesta da Progettazione Web Form.
    'Non spostarla o rimuoverla.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: questa chiamata al metodo è richiesta da Progettazione Web Form.
        'Non modificarla nell'editor del codice.
        InitializeComponent()
    End Sub

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        'Inserire qui il codice utente necessario per inizializzare la pagina
        'controllo se è stato effettuato il login
        If Not Session("LogIn") Is Nothing Then
            'se non è stato effettuato login
            If Session("LogIn") = False Then
                'carico la pagina LogOn.aspx dove svuoto eventuali session aperte
                Response.Redirect("LogOn.aspx")
            End If
        Else
            'carico la pagina LogOn.aspx dove svuoto eventuali session aperte
            Response.Redirect("LogOn.aspx")
        End If
        'Call UpdateCodiceFascicolo()
        If Page.IsPostBack = False Then
            Call UpdateCodiceFascicolo()
            TxtNumFascicolo.Value = Request.QueryString("NumeroFascicolo")
            txtNumeroFascicoloinVisione.Text = Request.QueryString("CodiceFascicolo")
            txtdescrizionefascicolo.Text = Request.QueryString("DescFascicolo")

            If Request.Params("cancellaprotocolli") = "si" Then
                Call CancellaCodiceFascicolo()
            End If
        End If
        AttivazionePulsanti()
    End Sub

    Sub CancellaCodiceFascicolo()
        Dim strSQL As String
        Dim MyCommand As SqlClient.SqlCommand


        strSQL = "Update cronologiaentitàdocumenti SET dataprot = null , nprot='' where identità = " & Request.Params("IdVol")
        MyCommand = ClsServer.EseguiSqlClient(strSQL, Session("conn"))

    End Sub
    Sub UpdateCodiceFascicolo()
        Dim strSQL As String
        Dim MyCommand As SqlClient.SqlCommand

        strSQL = "Update entità SET CodiceFascicolo ='" & Request.QueryString("CodiceFascicolo") & "', IDFascicolo = '" & Request.QueryString("NumeroFascicolo") & _
                    "', DescrFascicolo = '" & ClsServer.NoApice(Request.QueryString("DescFascicolo")) & "' "

        strSQL = strSQL & "WHERE IDEntità=" & Request.QueryString("IdVol")

        MyCommand = ClsServer.EseguiSqlClient(strSQL, Session("conn"))


    End Sub
    Sub AttivazionePulsanti()

        Dim strsql As String
        Dim dtrgenerico As SqlClient.SqlDataReader
        Dim utility As New ClsUtility()
        Dim idTipoProgetto As String = utility.TipologiaProgettoDaIdEntita(Request.QueryString("IdVol"), Session("conn"))
        '************************************************************************************************************************************
        '****************************************************ASSEGNAZIONEVOLONTARI***********************************************************
        '************************************************************************************************************************************
        If (Session("TipoUtente") = "U" Or Session("TipoUtente") = "R") Then
            strsql = "SELECT  statientità.idstatoentità, statientità.statoentità, " & _
                    " isnull(entità.codicevolontario,'')as codicevolontario,inservizio, " & _
                    " GraduatorieEntità.Sostituito,attivitàentità.IDAttivitàEntità,GraduatorieEntità.Stato as idoneo,GraduatorieEntità.ammesso as Selezionato, entità.DataInizioServizio,GraduatorieEntità.IdAttivitàSedeAssegnazione " & _
                    " FROM  StatiEntità " & _
                    " INNER Join entità ON StatiEntità.IDStatoEntità = entità.IDStatoEntità " & _
                    " INNER Join GraduatorieEntità ON entità.IDEntità = GraduatorieEntità.IdEntità " & _
                    " LEFT Join attivitàentità ON entità.IDEntità = attivitàentità.IDEntità " & _
                    " WHERE entità.identità=" & Request.QueryString("IdVol") & ""
            If Not dtrgenerico Is Nothing Then
                dtrgenerico.Close()
                dtrgenerico = Nothing
            End If
            dtrgenerico = ClsServer.CreaDatareader(strsql, Session("conn"))

            dtrgenerico.Read()

            If dtrgenerico("inservizio") = True Then

                chkLetteraAssegnazioneVolontario.Enabled = True
                chkDuplicatoletteraAssegnazioneEnte.Enabled = True
                chkLetteraAssegnazioneRitornoMittente.Enabled = True
                chkLetteraEsclusione.Enabled = True '?
                chkLetteraChiusura.Enabled = False
                chkLetteraChiusuraInServizio.Enabled = False
                chkLetteraEsclusionePerAssenzaIngiustificata.Enabled = False
                chkLetteraEsclusionePerGiorniPermesso.Enabled = False
                chkLetteraEsclusionePerSuperamentoMalattia.Enabled = False
                chkLetteraSubentro.Enabled = True
                chkAttoAggiuntivo.Enabled = True

            ElseIf (dtrgenerico("statoentità") = "Chiuso Durante Servizio" Or dtrgenerico("statoentità") = "Rinunciatario") And dtrgenerico("sostituito") = 0 Then

                chkLetteraAssegnazioneVolontario.Enabled = False
                chkDuplicatoletteraAssegnazioneEnte.Enabled = False
                chkLetteraAssegnazioneRitornoMittente.Enabled = False
                chkAttoAggiuntivo.Enabled = False

                If dtrgenerico("codicevolontario") <> "" Then
                    chkLetteraEsclusione.Enabled = True
                End If

                If dtrgenerico("statoentità") = "Chiuso Durante Servizio" Then
                    chkLetteraChiusuraInServizio.Enabled = True
                    chkLetteraEsclusionePerAssenzaIngiustificata.Enabled = True
                    chkLetteraEsclusionePerGiorniPermesso.Enabled = True
                    chkLetteraEsclusionePerSuperamentoMalattia.Enabled = True
                    chkLetteraChiusura.Enabled = False
                Else
                    chkLetteraChiusuraInServizio.Enabled = False
                    chkLetteraEsclusionePerAssenzaIngiustificata.Enabled = False
                    chkLetteraEsclusionePerGiorniPermesso.Enabled = False
                    chkLetteraEsclusionePerSuperamentoMalattia.Enabled = False
                    chkLetteraChiusura.Enabled = True
                End If

                chkLetteraSubentro.Enabled = False
            Else
                chkLetteraAssegnazioneVolontario.Enabled = False
                chkDuplicatoletteraAssegnazioneEnte.Enabled = False
                chkLetteraAssegnazioneRitornoMittente.Enabled = False
                chkAttoAggiuntivo.Enabled = False

                If dtrgenerico("codicevolontario") <> "" Then
                    chkLetteraEsclusione.Enabled = True
                End If

                If dtrgenerico("statoentità") = "Chiuso Durante Servizio" Then
                    chkLetteraChiusuraInServizio.Enabled = True
                    chkLetteraEsclusionePerAssenzaIngiustificata.Enabled = True
                    chkLetteraEsclusionePerGiorniPermesso.Enabled = True
                    chkLetteraEsclusionePerSuperamentoMalattia.Enabled = True
                    chkLetteraChiusura.Enabled = False
                ElseIf dtrgenerico("statoentità") = "Rinunciatario" Then
                    chkLetteraChiusuraInServizio.Enabled = False
                    chkLetteraEsclusionePerAssenzaIngiustificata.Enabled = False
                    chkLetteraEsclusionePerGiorniPermesso.Enabled = False
                    chkLetteraEsclusionePerSuperamentoMalattia.Enabled = False
                    chkLetteraChiusura.Enabled = True
                End If
                If (dtrgenerico("statoentità") = "Chiuso Durante Servizio" Or dtrgenerico("statoentità") = "Rinunciatario") Then
                    chkLetteraSubentro.Enabled = True
                End If
            End If

            If Not dtrgenerico Is Nothing Then
                dtrgenerico.Close()
                dtrgenerico = Nothing
            End If
        Else
            chkLetteraAssegnazioneVolontario.Enabled = False
            chkDuplicatoletteraAssegnazioneEnte.Enabled = False
            chkLetteraAssegnazioneRitornoMittente.Enabled = False
            chkLetteraEsclusione.Enabled = False
            chkLetteraChiusura.Enabled = False
            chkLetteraChiusuraInServizio.Enabled = False
            chkLetteraEsclusionePerAssenzaIngiustificata.Enabled = False
            chkLetteraEsclusionePerGiorniPermesso.Enabled = False
            chkLetteraEsclusionePerSuperamentoMalattia.Enabled = False
            chkLetteraSubentro.Enabled = False
            chkAttoAggiuntivo.Enabled = False

            If Not dtrgenerico Is Nothing Then
                dtrgenerico.Close()
                dtrgenerico = Nothing
            End If

        End If
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
        'Lanna Mauro 10/12/2008
        Call GestisciProtocolli()
        If (idTipoProgetto = PROGETTO_GARANZIA_GIOVANI) Then

            chkLetteraChiusura.Enabled = False
            chkDuplicatoletteraAssegnazioneEnte.Enabled = False
            chkLetteraAssegnazioneRitornoMittente.Enabled = False
        End If

        'MEV 219 (Certificato servizio svolto)
        'Rende visibile la CheckBox per generare il documento se il volontario ha maturato almeno un giorno di presenza
        Me.divCertificatoServizioSvolto.Visible = Me.ImpostaVisibilitaCBCertificatoServizioSvolto(Request.QueryString("IdVol").ToString())

    End Sub
    Private Sub GestisciProtocolli() 'Gestione text, label e pulsanti dei protocolli
        Dim strsql As String
        Dim dtrgenerico As SqlClient.SqlDataReader
        Dim IdAttivitàSedeAssegnazione As Integer

        strsql = "select * from CronologiaEntitàDocumenti where identità =" & Request.QueryString("IdVol")
        '        " and username = '" & Session("Utente") & "'"

        dtrgenerico = ClsServer.CreaDatareader(strsql, Session("conn"))

        GruppoLV(False)
        GruppoLVB(False)
        GruppoDLA(False)
        GruppoLARM(False)
        GruppoLEDD(False)
        GruppoLCI(False)
        GruppoLCIS(False)
        GruppoLEPAI(False)
        GruppoLEPGP(False)
        GruppoLEPSM(False)
        GruppoLS(False)
        GruppoLSB(False)
        Do While dtrgenerico.Read()
            Select Case dtrgenerico("Documento")
                Case "Assegnazione Volontario - Nazionale"
                    GruppoLV(True)
                    GruppoLVB(True)
                    TxtNumProtocollo.Text = "" & dtrgenerico("NProt")
                    TxtDataProtocollo.Text = "" & dtrgenerico("DataProt")
                Case "AssegnazioneVolontariNazionaliB"
                    GruppoLV(True)
                    GruppoLVB(True)
                    TxtNumProtocolloB.Text = "" & dtrgenerico("NProt")
                    TxtDataProtocolloB.Text = "" & dtrgenerico("DataProt")
                Case "Sostituzione Volontario - Nazionale"
                    GruppoLV(True)
                    GruppoLVB(True)
                    TxtNumProtocollo.Text = "" & dtrgenerico("NProt")
                    TxtDataProtocollo.Text = "" & dtrgenerico("DataProt")
                Case "SostituzioneVolontariNazionaliB"
                    GruppoLV(True)
                    GruppoLVB(True)
                    TxtNumProtocolloB.Text = "" & dtrgenerico("NProt")
                    TxtDataProtocolloB.Text = "" & dtrgenerico("DataProt")
                Case "Assegnazione Volontario - Estero"
                    GruppoLV(True)
                    GruppoLVB(True)
                    TxtNumProtocollo.Text = "" & dtrgenerico("NProt")
                    TxtDataProtocollo.Text = "" & dtrgenerico("DataProt")
                Case "AssegnazioneVolontariEsteroB"
                    GruppoLV(True)
                    GruppoLVB(True)
                    TxtNumProtocolloB.Text = "" & dtrgenerico("NProt")
                    TxtDataProtocolloB.Text = "" & dtrgenerico("DataProt")
                Case "Sostituzione Volontario - Estero"
                    GruppoLV(True)
                    GruppoLVB(True)
                    TxtNumProtocollo.Text = "" & dtrgenerico("NProt")
                    TxtDataProtocollo.Text = "" & dtrgenerico("DataProt")
                Case "SostituzioneVolontariEsteriB"
                    GruppoLV(True)
                    GruppoLVB(True)
                    TxtNumProtocolloB.Text = "" & dtrgenerico("NProt")
                    TxtDataProtocolloB.Text = "" & dtrgenerico("DataProt")


                    'Case "Lettera Assegnazione Volontario - Nazionale"
                    '    GruppoLV(True)
                    '    TxtNumProtocollo.Text = dtrgenerico("NProt")
                    '    TxtDataProtocollo.Text = dtrgenerico("DataProt")

                    'Case "Lettera Assegnazione Volontario - Estero"
                    '    GruppoLVB(True)
                    '    TxtNumProtocolloB.Text = dtrgenerico("NProt")
                    '    TxtDataProtocolloB.Text = dtrgenerico("DataProt")

                Case "Lettera Assegnazione Volontario Duplicato"
                    GruppoDLA(True)
                    TxtNumProtocolloDLA.Text = "" & dtrgenerico("NProt")
                    TxtDataProtocolloDLA.Text = "" & dtrgenerico("DataProt")

                Case "Lettera Assegnazione Ritorno al mittente"
                    GruppoLARM(True)
                    TxtNumProtocolloLARM.Text = "" & dtrgenerico("NProt")
                    TxtDataProtocolloLARM.Text = "" & dtrgenerico("DataProt")

                Case "Lettera Esclusione Volontario"
                    GruppoLEDD(True)
                    TxtNumProtocolloLEDD.Text = "" & dtrgenerico("NProt")
                    TxtDataProtocolloLEDD.Text = "" & dtrgenerico("DataProt")

                Case "Chiusura Iniziale"
                    GruppoLCI(True)
                    TxtNumProtocolloLCI.Text = "" & dtrgenerico("NProt")
                    TxtDataProtocolloLCI.Text = "" & dtrgenerico("DataProt")

                Case "Chiusura In Servizio"
                    GruppoLCIS(True)
                    TxtNumProtocolloLCIS.Text = "" & dtrgenerico("NProt")
                    TxtDataProtocolloLCIS.Text = "" & dtrgenerico("DataProt")

                Case "Lettera Esclusione Per Assenza Ingiustificata"
                    GruppoLEPAI(True)
                    TxtNumProtocolloLEPAI.Text = "" & dtrgenerico("NProt")
                    TxtDataProtocolloLEPAI.Text = "" & dtrgenerico("DataProt")

                Case "Lettera Esclusione Per Giorni Permesso"
                    GruppoLEPGP(True)
                    TxtNumProtocolloLEPGP.Text = "" & dtrgenerico("NProt")
                    TxtDataProtocolloLEPGP.Text = "" & dtrgenerico("DataProt")
                Case "Lettera Esclusione Per Superamento Malattia"
                    GruppoLEPSM(True)
                    TxtNumProtocolloLEPSM.Text = "" & dtrgenerico("NProt")
                    TxtDataProtocolloLEPSM.Text = "" & dtrgenerico("DataProt")
                Case "Rinuncia Servizio Volontario" 'subentro
                    GruppoLS(True)
                    GruppoLSB(True)
                    TxtNumProtocolloLS.Text = "" & dtrgenerico("NProt")
                    TxtDataProtocolloLS.Text = "" & dtrgenerico("DataProt")
                Case "rinunciaserviziovolontarioCopiaReg" 'subentro 
                    GruppoLS(True)
                    GruppoLSB(True)
                    TxtNumProtocolloLSB.Text = "" & dtrgenerico("NProt")
                    TxtDataProtocolloLSB.Text = "" & dtrgenerico("DataProt")
            End Select
        Loop

        strsql = "SELECT GraduatorieEntità.IdAttivitàSedeAssegnazione " & _
                " FROM GraduatorieEntità " & _
                " WHERE identità=" & Request.QueryString("IdVol") & ""
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
        dtrgenerico = ClsServer.CreaDatareader(strsql, Session("conn"))
        If dtrgenerico.HasRows = True Then
            dtrgenerico.Read()
            IdAttivitàSedeAssegnazione = dtrgenerico("IdAttivitàSedeAssegnazione")
        End If
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
        'estraggo protocollo dei documenti
        strsql = " SELECT NProt, DataProt,Documento  " & _
                 " From CronologiaEntiDocumenti " & _
                 " where IdEnte ='" & Session("IdEnte") & "' " & _
                 " and IdAttivitàSedeAssegnazione = " & IdAttivitàSedeAssegnazione
        '" and username = '" & Session("Utente") & "' " & _
        ' " and CronologiaEntiDocumenti.DataProt is not null and CronologiaEntiDocumenti.nprot is not null "
        dtrgenerico = ClsServer.CreaDatareader(strsql, Session("conn"))
        Do While dtrgenerico.Read()
            Select Case dtrgenerico("Documento")
                Case "rinunciaserviziovolontariomultipla" 'subentro
                    GruppoLS(True)
                    GruppoLSB(True)
                    TxtNumProtocolloLS.Text = "" & dtrgenerico("NProt")
                    TxtDataProtocolloLS.Text = "" & dtrgenerico("DataProt")
                Case "rinunciaserviziovolontariomultiplaCopiaReg" 'subentro 
                    GruppoLS(True)
                    GruppoLSB(True)
                    TxtNumProtocolloLSB.Text = "" & dtrgenerico("NProt")
                    TxtDataProtocolloLSB.Text = "" & dtrgenerico("DataProt")
            End Select
        Loop

        'mettere il visible sull'attivazione della freccietta per stampare

        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If

    End Sub

    Private Sub GruppoLV(ByVal BlValore As Boolean)
        'cmdScSelProtocolloLV.Visible = BlValore
        'LblNumProtocollo.Visible = BlValore
        'TxtNumProtocollo.Visible = BlValore
        'cmdScAllegatiLV.Visible = BlValore
        'cmdNuovoFasciocloLV.Visible = BlValore
        'LblDataProtocollo.Visible = BlValore
        'TxtDataProtocollo.Visible = BlValore
    End Sub
    Private Sub GruppoLVB(ByVal BlValore As Boolean)
        'LblNumProtocolloB.Visible = BlValore
        'TxtNumProtocolloB.Visible = BlValore
        'cmdScSelProtocolloLVB.Visible = BlValore
        'cmdScAllegatiLVB.Visible = BlValore
        'cmdNuovoFasciocloLVB.Visible = BlValore
        'LblDataProtocolloB.Visible = BlValore
        'TxtDataProtocolloB.Visible = BlValore
    End Sub
    Private Sub GruppoDLA(ByVal BlValore As Boolean)
        'LblNumProtocolloDLA.Visible = BlValore
        'TxtNumProtocolloDLA.Visible = BlValore
        'cmdScSelProtocolloDLA.Visible = BlValore
        'cmdScAllegatiDLA.Visible = BlValore
        'cmdNuovoFasciocloDLA.Visible = BlValore
        'LblDataProtocolloDLA.Visible = BlValore
        'TxtDataProtocolloDLA.Visible = BlValore
    End Sub
    Private Sub GruppoLARM(ByVal BlValore As Boolean)
        'LblNumProtocolloLARM.Visible = BlValore
        'TxtNumProtocolloLARM.Visible = BlValore
        'cmdScSelProtocolloLARM.Visible = BlValore
        'cmdScAllegatiLARM.Visible = BlValore
        'cmdNuovoFasciocloLARM.Visible = BlValore
        'LblDataProtocolloLARM.Visible = BlValore
        'TxtDataProtocolloLARM.Visible = BlValore
    End Sub
    Private Sub GruppoLEDD(ByVal BlValore As Boolean)
        'LblNumProtocolloLEDD.Visible = BlValore
        'TxtNumProtocolloLEDD.Visible = BlValore
        'cmdScSelProtocolloLEDD.Visible = BlValore
        'cmdScAllegatiLEDD.Visible = BlValore
        'cmdNuovoFasciocloLEDD.Visible = BlValore
        'LblDataProtocolloLEDD.Visible = BlValore
        'TxtDataProtocolloLEDD.Visible = BlValore
    End Sub
    Private Sub GruppoLCI(ByVal BlValore As Boolean)
        'LblNumProtocolloLCI.Visible = BlValore
        'TxtNumProtocolloLCI.Visible = BlValore
        'cmdScSelProtocolloLCI.Visible = BlValore
        'cmdScAllegatiLCI.Visible = BlValore
        'cmdNuovoFasciocloLCI.Visible = BlValore
        'LblDataProtocolloLCI.Visible = BlValore
        'TxtDataProtocolloLCI.Visible = BlValore
    End Sub
    Private Sub GruppoLCIS(ByVal BlValore As Boolean)
        'LblNumProtocolloLCIS.Visible = BlValore
        'TxtNumProtocolloLCIS.Visible = BlValore
        'cmdScSelProtocolloLCIS.Visible = BlValore
        'cmdScAllegatiLCIS.Visible = BlValore
        'cmdNuovoFasciocloLCIS.Visible = BlValore
        'LblDataProtocolloLCIS.Visible = BlValore
        'TxtDataProtocolloLCIS.Visible = BlValore
    End Sub
    Private Sub GruppoLEPAI(ByVal BlValore As Boolean)
        'LblNumProtocolloLEPAI.Visible = BlValore
        'TxtNumProtocolloLEPAI.Visible = BlValore
        'cmdScSelProtocolloLEPAI.Visible = BlValore
        'cmdScAllegatiLEPAI.Visible = BlValore
        'cmdNuovoFasciocloLEPAI.Visible = BlValore
        'LblDataProtocolloLEPAI.Visible = BlValore
        'TxtDataProtocolloLEPAI.Visible = BlValore
    End Sub
    Private Sub GruppoLEPGP(ByVal BlValore As Boolean)
        'LblNumProtocolloLEPGP.Visible = BlValore
        'TxtNumProtocolloLEPGP.Visible = BlValore
        'cmdScSelProtocolloLEPGP.Visible = BlValore
        'cmdScAllegatiLEPGP.Visible = BlValore
        'cmdNuovoFasciocloLEPGP.Visible = BlValore
        'LblDataProtocolloLEPGP.Visible = BlValore
        'TxtDataProtocolloLEPGP.Visible = BlValore
    End Sub
    Private Sub GruppoLEPSM(ByVal BlValore As Boolean)
        'LblNumProtocolloLEPSM.Visible = BlValore
        'TxtNumProtocolloLEPSM.Visible = BlValore
        'cmdScSelProtocolloLEPSM.Visible = BlValore
        'cmdScAllegatiLEPSM.Visible = BlValore
        'cmdNuovoFasciocloLEPSM.Visible = BlValore
        'LblDataProtocolloLEPSM.Visible = BlValore
        'TxtDataProtocolloLEPSM.Visible = BlValore
    End Sub
    Private Sub GruppoLS(ByVal BlValore As Boolean)
        'LblNumProtocolloLS.Visible = BlValore
        'TxtNumProtocolloLS.Visible = BlValore
        'cmdScSelProtocolloLS.Visible = BlValore
        'cmdScAllegatiLS.Visible = BlValore
        'cmdNuovoFasciocloLS.Visible = BlValore
        'LblDataProtocolloLS.Visible = BlValore
        'TxtDataProtocolloLS.Visible = BlValore
    End Sub
    Private Sub GruppoLSB(ByVal BlValore As Boolean)
        'LblNumProtocolloLSB.Visible = BlValore
        'TxtNumProtocolloLSB.Visible = BlValore
        'cmdScSelProtocolloLSB.Visible = BlValore
        'cmdScAllegatiLSB.Visible = BlValore
        'cmdNuovoFasciocloLSB.Visible = BlValore
        'LblDataProtocolloLSB.Visible = BlValore
        'TxtDataProtocolloLSB.Visible = BlValore
    End Sub

    Private Sub imgGeneraFile_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles imgGeneraFile.Click
        Dim dtrgenerico As SqlClient.SqlDataReader
        Dim strsql As String
        Dim IdAttivitàSedeAssegnazione As Integer
        Dim utility As ClsUtility = New ClsUtility()
        Dim idTipoProgetto As String = utility.TipologiaProgettoDaIdEntita(Request.QueryString("IdVol"), Session("conn"))
        Dim strTipoModello As String = utility.TipologiaModelloDaIdEntita(Request.QueryString("IdVol"), Session("conn"))
        Dim Documento As New GeneratoreModelli
        Dim strNomeDocumento As String
        '************************************************************************************************************************************
        '****************************************************LetteraSubentro*****************************************************************
        '************************************************************************************************************************************
        'modificata aprile 2022 per suddivisione 4 modelli (ORD, GG, EST, SCD)
        If chkLetteraSubentro.Checked = True Then 'LetteraSubentro
            imgGeneraFile.Visible = False
            Select Case strTipoModello
                Case "ORD"
                    hplLetteraSubentro.Visible = True
                    'hplLetteraSubentroCopia.Visible = True
                    GruppoLS(True)
                    GruppoLSB(True)
                    If Session("StatoVolontario") = "In Servizio" Then

                        'chiamo la proprietà della classe GeneratoreModelli che ritorna la path del documento creato
                        hplLetteraSubentro.NavigateUrl = Documento.VOL_rinunciaserviziovolontariomultipla(Session("IdEnte"), Request.QueryString("IdVol"), ClsUtility.TrovaIDBando(Request.QueryString("IdAttivita"), Session("conn")), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                        'hplLetteraSubentroCopia.NavigateUrl = Documento.VOL_rinunciaserviziovolontariomultiplaCopiaReg(Session("IdEnte"), Request.QueryString("IdVol"), ClsUtility.TrovaIDBando(Request.QueryString("IdAttivita"), Session("conn")), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))

                        strsql = "SELECT GraduatorieEntità.IdAttivitàSedeAssegnazione " & _
                                " FROM GraduatorieEntità " & _
                                " WHERE identità=" & Request.QueryString("IdVol") & ""
                        If Not dtrgenerico Is Nothing Then
                            dtrgenerico.Close()
                            dtrgenerico = Nothing
                        End If
                        dtrgenerico = ClsServer.CreaDatareader(strsql, Session("conn"))
                        If dtrgenerico.HasRows = True Then
                            dtrgenerico.Read()
                            IdAttivitàSedeAssegnazione = dtrgenerico("IdAttivitàSedeAssegnazione")
                        End If
                        If Not dtrgenerico Is Nothing Then
                            dtrgenerico.Close()
                            dtrgenerico = Nothing
                        End If

                        NuovaCronologia("rinunciaserviziovolontariomultipla", IdAttivitàSedeAssegnazione, TxtDataProtocolloLS.Text, TxtNumProtocolloLS.Text)
                        'NuovaCronologia("rinunciaserviziovolontariomultiplaCopiaReg", IdAttivitàSedeAssegnazione, TxtDataProtocolloLSB.Text, TxtNumProtocolloLSB.Text)
                    Else
                        strsql = " SELECT entità.IDEntità, entità.CodiceVolontario, entità.Cognome, entità.Nome, entità.DataInizioServizio, entità.DataFineServizio " & _
                                 " FROM CronologiaSostituzioni  " & _
                                 " INNER JOIN entità ON CronologiaSostituzioni.IDEntitàSubentrante = entità.IDEntità " & _
                                 " WHERE CronologiaSostituzioni.IDEntitàSostituita =" & Request.QueryString("IdVol") & ""
                        If Not dtrgenerico Is Nothing Then
                            dtrgenerico.Close()
                            dtrgenerico = Nothing
                        End If
                        dtrgenerico = ClsServer.CreaDatareader(strsql, Session("conn"))
                        dtrgenerico.Read()
                        Session("IdVolSubentrante") = dtrgenerico("IDEntità")
                        Session("CodVolSubentrante") = dtrgenerico("CodiceVolontario")
                        Session("VolSubentrante") = dtrgenerico("Cognome") & " " & dtrgenerico("Nome")
                        Session("DataInizio") = dtrgenerico("DataInizioServizio")
                        If Not dtrgenerico Is Nothing Then
                            dtrgenerico.Close()
                            dtrgenerico = Nothing
                        End If

                        'chiamo la proprietà della classe GeneratoreModelli che ritorna la path del documento creato
                        hplLetteraSubentro.NavigateUrl = Documento.VOL_rinunciaserviziovolontario(Session("IdEnte"), Request.QueryString("IdVol"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                        'hplLetteraSubentroCopia.NavigateUrl = Documento.VOL_rinunciaserviziovolontarioCopiaReg(Session("IdEnte"), Request.QueryString("IdVol"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))

                        If Not dtrgenerico Is Nothing Then
                            dtrgenerico.Close()
                            dtrgenerico = Nothing
                        End If
                        ClsUtility.CronologiaDocEntità(Request.QueryString("IdVol"), Session("Utente"), "Rinuncia Servizio Volontario", Session("conn"), TxtDataProtocolloLS.Text, TxtNumProtocolloLS.Text)
                        'ClsUtility.CronologiaDocEntità(Request.QueryString("IdVol"), Session("Utente"), "rinunciaserviziovolontarioCopiaReg", Session("conn"), TxtDataProtocolloLSB.Text, TxtNumProtocolloLSB.Text)
                    End If
                Case "EST"
                    hplLetteraSubentro.Visible = True
                    'hplLetteraSubentroCopia.Visible = True
                    GruppoLS(True)
                    GruppoLSB(True)
                    If Session("StatoVolontario") = "In Servizio" Then

                        'chiamo la proprietà della classe GeneratoreModelli che ritorna la path del documento creato
                        hplLetteraSubentro.NavigateUrl = Documento.VOL_rinunciaserviziovolontariomultiplaEstero(Session("IdEnte"), Request.QueryString("IdVol"), ClsUtility.TrovaIDBando(Request.QueryString("IdAttivita"), Session("conn")), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                        'hplLetteraSubentroCopia.NavigateUrl = Documento.VOL_rinunciaserviziovolontariomultiplaCopiaReg(Session("IdEnte"), Request.QueryString("IdVol"), ClsUtility.TrovaIDBando(Request.QueryString("IdAttivita"), Session("conn")), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))

                        strsql = "SELECT GraduatorieEntità.IdAttivitàSedeAssegnazione " & _
                                " FROM GraduatorieEntità " & _
                                " WHERE identità=" & Request.QueryString("IdVol") & ""
                        If Not dtrgenerico Is Nothing Then
                            dtrgenerico.Close()
                            dtrgenerico = Nothing
                        End If
                        dtrgenerico = ClsServer.CreaDatareader(strsql, Session("conn"))
                        If dtrgenerico.HasRows = True Then
                            dtrgenerico.Read()
                            IdAttivitàSedeAssegnazione = dtrgenerico("IdAttivitàSedeAssegnazione")
                        End If
                        If Not dtrgenerico Is Nothing Then
                            dtrgenerico.Close()
                            dtrgenerico = Nothing
                        End If

                        NuovaCronologia("rinunciaserviziovolontariomultipla", IdAttivitàSedeAssegnazione, TxtDataProtocolloLS.Text, TxtNumProtocolloLS.Text)
                        'NuovaCronologia("rinunciaserviziovolontariomultiplaCopiaReg", IdAttivitàSedeAssegnazione, TxtDataProtocolloLSB.Text, TxtNumProtocolloLSB.Text)
                    Else
                        strsql = " SELECT entità.IDEntità, entità.CodiceVolontario, entità.Cognome, entità.Nome, entità.DataInizioServizio, entità.DataFineServizio " & _
                                 " FROM CronologiaSostituzioni  " & _
                                 " INNER JOIN entità ON CronologiaSostituzioni.IDEntitàSubentrante = entità.IDEntità " & _
                                 " WHERE CronologiaSostituzioni.IDEntitàSostituita =" & Request.QueryString("IdVol") & ""
                        If Not dtrgenerico Is Nothing Then
                            dtrgenerico.Close()
                            dtrgenerico = Nothing
                        End If
                        dtrgenerico = ClsServer.CreaDatareader(strsql, Session("conn"))
                        dtrgenerico.Read()
                        Session("IdVolSubentrante") = dtrgenerico("IDEntità")
                        Session("CodVolSubentrante") = dtrgenerico("CodiceVolontario")
                        Session("VolSubentrante") = dtrgenerico("Cognome") & " " & dtrgenerico("Nome")
                        Session("DataInizio") = dtrgenerico("DataInizioServizio")
                        If Not dtrgenerico Is Nothing Then
                            dtrgenerico.Close()
                            dtrgenerico = Nothing
                        End If

                        'chiamo la proprietà della classe GeneratoreModelli che ritorna la path del documento creato
                        hplLetteraSubentro.NavigateUrl = Documento.VOL_rinunciaserviziovolontarioEstero(Session("IdEnte"), Request.QueryString("IdVol"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                        'hplLetteraSubentroCopia.NavigateUrl = Documento.VOL_rinunciaserviziovolontarioCopiaReg(Session("IdEnte"), Request.QueryString("IdVol"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))

                        If Not dtrgenerico Is Nothing Then
                            dtrgenerico.Close()
                            dtrgenerico = Nothing
                        End If
                        ClsUtility.CronologiaDocEntità(Request.QueryString("IdVol"), Session("Utente"), "Rinuncia Servizio Volontario", Session("conn"), TxtDataProtocolloLS.Text, TxtNumProtocolloLS.Text)
                        'ClsUtility.CronologiaDocEntità(Request.QueryString("IdVol"), Session("Utente"), "rinunciaserviziovolontarioCopiaReg", Session("conn"), TxtDataProtocolloLSB.Text, TxtNumProtocolloLSB.Text)
                    End If
                Case "GG"
                    hplLetteraSubentro.Visible = True
                    GruppoLS(True)
                    If Session("StatoVolontario") = "In Servizio" Then
                        hplLetteraSubentro.Visible = True
                        hplLetteraSubentro.NavigateUrl = Documento.VOL_RinunciaServizioVolontarioMultiplaGaranziaGiovani(Session("IdEnte"), Request.QueryString("IdVol"), ClsUtility.TrovaIDBando(Request.QueryString("IdAttivita"), Session("conn")), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                        strsql = "SELECT GraduatorieEntità.IdAttivitàSedeAssegnazione " & _
                                " FROM GraduatorieEntità " & _
                                " WHERE identità=" & Request.QueryString("IdVol") & ""
                        If Not dtrgenerico Is Nothing Then
                            dtrgenerico.Close()
                            dtrgenerico = Nothing
                        End If
                        dtrgenerico = ClsServer.CreaDatareader(strsql, Session("conn"))
                        If dtrgenerico.HasRows = True Then
                            dtrgenerico.Read()
                            IdAttivitàSedeAssegnazione = dtrgenerico("IdAttivitàSedeAssegnazione")
                        End If
                        If Not dtrgenerico Is Nothing Then
                            dtrgenerico.Close()
                            dtrgenerico = Nothing
                        End If
                        NuovaCronologia("RinunciaServizioVolontarioMultiplaGaranziaGiovani", IdAttivitàSedeAssegnazione, TxtDataProtocolloLS.Text, TxtNumProtocolloLS.Text)
                    Else
                        strsql = " SELECT entità.IDEntità, entità.CodiceVolontario, entità.Cognome, entità.Nome, entità.DataInizioServizio, entità.DataFineServizio " & _
                                 " FROM CronologiaSostituzioni  " & _
                                 " INNER JOIN entità ON CronologiaSostituzioni.IDEntitàSubentrante = entità.IDEntità " & _
                                 " WHERE CronologiaSostituzioni.IDEntitàSostituita =" & Request.QueryString("IdVol") & ""
                        If Not dtrgenerico Is Nothing Then
                            dtrgenerico.Close()
                            dtrgenerico = Nothing
                        End If
                        dtrgenerico = ClsServer.CreaDatareader(strsql, Session("conn"))
                        dtrgenerico.Read()
                        Session("IdVolSubentrante") = dtrgenerico("IDEntità")
                        Session("CodVolSubentrante") = dtrgenerico("CodiceVolontario")
                        Session("VolSubentrante") = dtrgenerico("Cognome") & " " & dtrgenerico("Nome")
                        Session("DataInizio") = dtrgenerico("DataInizioServizio")
                        If Not dtrgenerico Is Nothing Then
                            dtrgenerico.Close()
                            dtrgenerico = Nothing
                        End If
                        hplLetteraSubentro.NavigateUrl = Documento.VOL_RinunciaServizioVolontarioGaranziaGiovani(Session("IdEnte"), Request.QueryString("IdVol"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                        If Not dtrgenerico Is Nothing Then
                            dtrgenerico.Close()
                            dtrgenerico = Nothing
                        End If

                        ClsUtility.CronologiaDocEntità(Request.QueryString("IdVol"), Session("Utente"), "Rinuncia Servizio Volontario Garanzia Giovani", Session("conn"), TxtDataProtocolloLS.Text, TxtNumProtocolloLS.Text)
                    End If
                Case "SCD"
                    hplLetteraSubentro.Visible = True
                    'hplLetteraSubentroCopia.Visible = True
                    GruppoLS(True)
                    GruppoLSB(True)
                    If Session("StatoVolontario") = "In Servizio" Then

                        'chiamo la proprietà della classe GeneratoreModelli che ritorna la path del documento creato
                        hplLetteraSubentro.NavigateUrl = Documento.VOL_rinunciaserviziovolontariomultiplaSCD(Session("IdEnte"), Request.QueryString("IdVol"), ClsUtility.TrovaIDBando(Request.QueryString("IdAttivita"), Session("conn")), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                        'hplLetteraSubentroCopia.NavigateUrl = Documento.VOL_rinunciaserviziovolontariomultiplaCopiaReg(Session("IdEnte"), Request.QueryString("IdVol"), ClsUtility.TrovaIDBando(Request.QueryString("IdAttivita"), Session("conn")), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))

                        strsql = "SELECT GraduatorieEntità.IdAttivitàSedeAssegnazione " & _
                                " FROM GraduatorieEntità " & _
                                " WHERE identità=" & Request.QueryString("IdVol") & ""
                        If Not dtrgenerico Is Nothing Then
                            dtrgenerico.Close()
                            dtrgenerico = Nothing
                        End If
                        dtrgenerico = ClsServer.CreaDatareader(strsql, Session("conn"))
                        If dtrgenerico.HasRows = True Then
                            dtrgenerico.Read()
                            IdAttivitàSedeAssegnazione = dtrgenerico("IdAttivitàSedeAssegnazione")
                        End If
                        If Not dtrgenerico Is Nothing Then
                            dtrgenerico.Close()
                            dtrgenerico = Nothing
                        End If

                        NuovaCronologia("rinunciaserviziovolontariomultipla", IdAttivitàSedeAssegnazione, TxtDataProtocolloLS.Text, TxtNumProtocolloLS.Text)
                        'NuovaCronologia("rinunciaserviziovolontariomultiplaCopiaReg", IdAttivitàSedeAssegnazione, TxtDataProtocolloLSB.Text, TxtNumProtocolloLSB.Text)
                    Else
                        strsql = " SELECT entità.IDEntità, entità.CodiceVolontario, entità.Cognome, entità.Nome, entità.DataInizioServizio, entità.DataFineServizio " & _
                                 " FROM CronologiaSostituzioni  " & _
                                 " INNER JOIN entità ON CronologiaSostituzioni.IDEntitàSubentrante = entità.IDEntità " & _
                                 " WHERE CronologiaSostituzioni.IDEntitàSostituita =" & Request.QueryString("IdVol") & ""
                        If Not dtrgenerico Is Nothing Then
                            dtrgenerico.Close()
                            dtrgenerico = Nothing
                        End If
                        dtrgenerico = ClsServer.CreaDatareader(strsql, Session("conn"))
                        dtrgenerico.Read()
                        Session("IdVolSubentrante") = dtrgenerico("IDEntità")
                        Session("CodVolSubentrante") = dtrgenerico("CodiceVolontario")
                        Session("VolSubentrante") = dtrgenerico("Cognome") & " " & dtrgenerico("Nome")
                        Session("DataInizio") = dtrgenerico("DataInizioServizio")
                        If Not dtrgenerico Is Nothing Then
                            dtrgenerico.Close()
                            dtrgenerico = Nothing
                        End If

                        'chiamo la proprietà della classe GeneratoreModelli che ritorna la path del documento creato
                        hplLetteraSubentro.NavigateUrl = Documento.VOL_rinunciaserviziovolontarioSCD(Session("IdEnte"), Request.QueryString("IdVol"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                        'hplLetteraSubentroCopia.NavigateUrl = Documento.VOL_rinunciaserviziovolontarioCopiaReg(Session("IdEnte"), Request.QueryString("IdVol"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))

                        If Not dtrgenerico Is Nothing Then
                            dtrgenerico.Close()
                            dtrgenerico = Nothing
                        End If
                        ClsUtility.CronologiaDocEntità(Request.QueryString("IdVol"), Session("Utente"), "Rinuncia Servizio Volontario", Session("conn"), TxtDataProtocolloLS.Text, TxtNumProtocolloLS.Text)
                        'ClsUtility.CronologiaDocEntità(Request.QueryString("IdVol"), Session("Utente"), "rinunciaserviziovolontarioCopiaReg", Session("conn"), TxtDataProtocolloLSB.Text, TxtNumProtocolloLSB.Text)
                    End If
            End Select
        End If


        '************************************************************************************************************************************
        '****************************************************LetteraChiusura***************************************************************
        '************************************************************************************************************************************
        If chkLetteraChiusura.Checked = True Then
            'lblChiusura.Text = "Scarica File  "
            imgGeneraFile.Visible = False
            hplLetteraChiusura.Visible = True
            hplLetteraChiusura.Visible = True
            '********  Mauro Lanna
            GruppoLCI(True)
            '*******
            hplLetteraChiusura.NavigateUrl = Documento.VOL_ChiusuraIniziale(Request.QueryString("IdVol"), Session("IdEnte"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
            ClsUtility.CronologiaDocEntità(Request.QueryString("IdVol"), Session("Utente"), "Chiusura Iniziale", Session("conn"), TxtDataProtocolloLCI.Text, TxtNumProtocolloLCI.Text)
        End If
        '************************************************************************************************************************************
        '****************************************************LetteraChiusuraInServizio*******************************************************
        '************************************************************************************************************************************
        'modificata aprile 2022 per suddivisione 4 modelli (ORD, GG, EST, SCD)
        If chkLetteraChiusuraInServizio.Checked = True Then
            'lblChiusura.Text = "Scarica File  "
            imgGeneraFile.Visible = False
            hplLetteraChiusuraInServizio.Visible = True
            GruppoLCIS(True)

            Select Case strTipoModello
                Case "ORD"
                    hplLetteraChiusuraInServizio.NavigateUrl = Documento.VOL_ChiusuraInServizio(Request.QueryString("IdVol"), Session("IdEnte"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                    ClsUtility.CronologiaDocEntità(Request.QueryString("IdVol"), Session("Utente"), "Chiusura In Servizio", Session("conn"), TxtDataProtocolloLCIS.Text, TxtNumProtocolloLCIS.Text)
                Case "EST"
                    hplLetteraChiusuraInServizio.NavigateUrl = Documento.VOL_ChiusuraInServizioEstero(Request.QueryString("IdVol"), Session("IdEnte"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                    ClsUtility.CronologiaDocEntità(Request.QueryString("IdVol"), Session("Utente"), "Chiusura In Servizio", Session("conn"), TxtDataProtocolloLCIS.Text, TxtNumProtocolloLCIS.Text)
                Case "GG"
                    hplLetteraChiusuraInServizio.NavigateUrl = Documento.VOL_ChiusuraInServizioGaranziaGiovani(Request.QueryString("IdVol"), Session("IdEnte"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                    ClsUtility.CronologiaDocEntità(Request.QueryString("IdVol"), Session("Utente"), "Chiusura In Servizio Garanzia Giovani", Session("conn"), TxtDataProtocolloLCIS.Text, TxtNumProtocolloLCIS.Text)
                Case "SCD"
                    hplLetteraChiusuraInServizio.NavigateUrl = Documento.VOL_ChiusuraInServizioSCD(Request.QueryString("IdVol"), Session("IdEnte"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                    ClsUtility.CronologiaDocEntità(Request.QueryString("IdVol"), Session("Utente"), "Chiusura In Servizio", Session("conn"), TxtDataProtocolloLCIS.Text, TxtNumProtocolloLCIS.Text)
            End Select
        End If
        '************************************************************************************************************************************
        '****************************************************Lettera Esclusione Per Assenza Ingiustificata***********************************
        '************************************************************************************************************************************
        'modificata aprile 2022 per suddivisione 4 modelli (ORD, GG, EST, SCD)
        If chkLetteraEsclusionePerAssenzaIngiustificata.Checked = True Then
            imgGeneraFile.Visible = False
            hplLetteraEsclusionePerAssenzaIngiustificata.Visible = True
            GruppoLEPAI(True)
            Select Case strTipoModello
                Case "ORD"
                    hplLetteraEsclusionePerAssenzaIngiustificata.NavigateUrl = Documento.VOL_LetteraEsclusionePerAssenzaIngiustificata(Request.QueryString("IdVol"), Session("IdEnte"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                    ClsUtility.CronologiaDocEntità(Request.QueryString("IdVol"), Session("Utente"), "Lettera Esclusione Per Assenza Ingiustificata", Session("conn"), TxtDataProtocolloLEPAI.Text, TxtNumProtocolloLEPAI.Text)
                Case "EST"
                    hplLetteraEsclusionePerAssenzaIngiustificata.NavigateUrl = Documento.VOL_LetteraEsclusionePerAssenzaIngiustificataEstero(Request.QueryString("IdVol"), Session("IdEnte"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                    ClsUtility.CronologiaDocEntità(Request.QueryString("IdVol"), Session("Utente"), "Lettera Esclusione Per Assenza Ingiustificata", Session("conn"), TxtDataProtocolloLEPAI.Text, TxtNumProtocolloLEPAI.Text)
                Case "GG"
                    hplLetteraEsclusionePerAssenzaIngiustificata.NavigateUrl = Documento.VOL_LetteraEsclusionePerAssenzaIngiustificataGaranziaGiovani(Request.QueryString("IdVol"), Session("IdEnte"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                    ClsUtility.CronologiaDocEntità(Request.QueryString("IdVol"), Session("Utente"), "Lettera Esclusione Per Assenza Ingiustificata Garanzia Giovani", Session("conn"), TxtDataProtocolloLEPAI.Text, TxtNumProtocolloLEPAI.Text)
                Case "SCD"
                    hplLetteraEsclusionePerAssenzaIngiustificata.NavigateUrl = Documento.VOL_LetteraEsclusionePerAssenzaIngiustificataSCD(Request.QueryString("IdVol"), Session("IdEnte"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                    ClsUtility.CronologiaDocEntità(Request.QueryString("IdVol"), Session("Utente"), "Lettera Esclusione Per Assenza Ingiustificata", Session("conn"), TxtDataProtocolloLEPAI.Text, TxtNumProtocolloLEPAI.Text)
            End Select
        End If
        '************************************************************************************************************************************
        '****************************************************Lettera Esclusione Per Giorni Permesso******************************************
        '************************************************************************************************************************************
        'modificata aprile 2022 per suddivisione 4 modelli (ORD, GG, EST, SCD)
        If chkLetteraEsclusionePerGiorniPermesso.Checked = True Then
            imgGeneraFile.Visible = False
            hplLetteraEsclusionePerGiorniPermesso.Visible = True
            GruppoLEPGP(True)

            Select Case strTipoModello
                Case "ORD"
                    hplLetteraEsclusionePerGiorniPermesso.NavigateUrl = Documento.VOL_LetteraEsclusionePerGiorniPermesso(Request.QueryString("IdVol"), Session("IdEnte"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                    ClsUtility.CronologiaDocEntità(Request.QueryString("IdVol"), Session("Utente"), "Lettera Esclusione Per Giorni Permesso", Session("conn"), TxtDataProtocolloLEPGP.Text, TxtNumProtocolloLEPGP.Text)
                Case "EST"
                    hplLetteraEsclusionePerGiorniPermesso.NavigateUrl = Documento.VOL_LetteraEsclusionePerGiorniPermessoEstero(Request.QueryString("IdVol"), Session("IdEnte"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                    ClsUtility.CronologiaDocEntità(Request.QueryString("IdVol"), Session("Utente"), "Lettera Esclusione Per Giorni Permesso", Session("conn"), TxtDataProtocolloLEPGP.Text, TxtNumProtocolloLEPGP.Text)
                Case "GG"
                    hplLetteraEsclusionePerGiorniPermesso.NavigateUrl = Documento.VOL_LetteraEsclusionePerGiorniPermessoGaranziaGiovani(Request.QueryString("IdVol"), Session("IdEnte"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                    ClsUtility.CronologiaDocEntità(Request.QueryString("IdVol"), Session("Utente"), "Lettera Esclusione Per Giorni Permesso Garanzia Giovani", Session("conn"), TxtDataProtocolloLEPGP.Text, TxtNumProtocolloLEPGP.Text)
                Case "SCD"
                    hplLetteraEsclusionePerGiorniPermesso.NavigateUrl = Documento.VOL_LetteraEsclusionePerGiorniPermessoSCD(Request.QueryString("IdVol"), Session("IdEnte"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                    ClsUtility.CronologiaDocEntità(Request.QueryString("IdVol"), Session("Utente"), "Lettera Esclusione Per Giorni Permesso", Session("conn"), TxtDataProtocolloLEPGP.Text, TxtNumProtocolloLEPGP.Text)
            End Select
        End If

        '************************************************************************************************************************************
        '****************************************************Lettera Esclusione Per Superamento Malattia*************************************
        '************************************************************************************************************************************
        'modificata aprile 2022 per suddivisione 4 modelli (ORD, GG, EST, SCD)
        If chkLetteraEsclusionePerSuperamentoMalattia.Checked = True Then
            imgGeneraFile.Visible = False
            hplLetteraEsclusionePerSuperamentoMalattia.Visible = True
            GruppoLEPSM(True)
            Select Case strTipoModello
                Case "ORD"
                    If VerificaCausale(Request.QueryString("IdVol")) = 1 Then
                        hplLetteraEsclusionePerSuperamentoMalattia.NavigateUrl = Documento.VOL_LetteraEsclusionePerSuperamentoMalattia(Request.QueryString("IdVol"), Session("IdEnte"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                        ClsUtility.CronologiaDocEntità(Request.QueryString("IdVol"), Session("Utente"), "Lettera Esclusione Per Superamento Malattia", Session("conn"), TxtDataProtocolloLEPSM.Text, TxtNumProtocolloLEPSM.Text)
                    Else
                        hplLetteraEsclusionePerSuperamentoMalattia.NavigateUrl = Documento.VOL_LetteraEsclusionePerSuperamentoMalattiaDopoi6Mesi(Request.QueryString("IdVol"), Session("IdEnte"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                        ClsUtility.CronologiaDocEntità(Request.QueryString("IdVol"), Session("Utente"), "Lettera Esclusione Per Superamento Malattia", Session("conn"), TxtDataProtocolloLEPSM.Text, TxtNumProtocolloLEPSM.Text)
                    End If
                Case "EST"
                    If VerificaCausale(Request.QueryString("IdVol")) = 1 Then
                        hplLetteraEsclusionePerSuperamentoMalattia.NavigateUrl = Documento.VOL_LetteraEsclusionePerSuperamentoMalattiaEstero(Request.QueryString("IdVol"), Session("IdEnte"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                        ClsUtility.CronologiaDocEntità(Request.QueryString("IdVol"), Session("Utente"), "Lettera Esclusione Per Superamento Malattia", Session("conn"), TxtDataProtocolloLEPSM.Text, TxtNumProtocolloLEPSM.Text)
                    Else
                        hplLetteraEsclusionePerSuperamentoMalattia.NavigateUrl = Documento.VOL_LetteraEsclusionePerSuperamentoMalattiaDopoi6MesiEstero(Request.QueryString("IdVol"), Session("IdEnte"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                        ClsUtility.CronologiaDocEntità(Request.QueryString("IdVol"), Session("Utente"), "Lettera Esclusione Per Superamento Malattia", Session("conn"), TxtDataProtocolloLEPSM.Text, TxtNumProtocolloLEPSM.Text)
                    End If
                Case "GG"
                    If VerificaCausale(Request.QueryString("IdVol")) = 1 Then
                        hplLetteraEsclusionePerSuperamentoMalattia.NavigateUrl = Documento.VOL_LetteraEsclusionePerSuperamentoMalattiaGaranziaGiovani(Request.QueryString("IdVol"), Session("IdEnte"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                    Else
                        hplLetteraEsclusionePerSuperamentoMalattia.NavigateUrl = Documento.VOL_LetteraEsclusionePerSuperamentoMalattiaDopoi6MesiGaranziaGiovani(Request.QueryString("IdVol"), Session("IdEnte"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                    End If
                    ClsUtility.CronologiaDocEntità(Request.QueryString("IdVol"), Session("Utente"), "Lettera Esclusione Per Superamento Malattia", Session("conn"), TxtDataProtocolloLEPSM.Text, TxtNumProtocolloLEPSM.Text)
                Case "SCD"
                    If VerificaCausale(Request.QueryString("IdVol")) = 1 Then
                        hplLetteraEsclusionePerSuperamentoMalattia.NavigateUrl = Documento.VOL_LetteraEsclusionePerSuperamentoMalattiaSCD(Request.QueryString("IdVol"), Session("IdEnte"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                        ClsUtility.CronologiaDocEntità(Request.QueryString("IdVol"), Session("Utente"), "Lettera Esclusione Per Superamento Malattia", Session("conn"), TxtDataProtocolloLEPSM.Text, TxtNumProtocolloLEPSM.Text)
                    Else
                        hplLetteraEsclusionePerSuperamentoMalattia.NavigateUrl = Documento.VOL_LetteraEsclusionePerSuperamentoMalattiaDopoi6MesiSCD(Request.QueryString("IdVol"), Session("IdEnte"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                        ClsUtility.CronologiaDocEntità(Request.QueryString("IdVol"), Session("Utente"), "Lettera Esclusione Per Superamento Malattia", Session("conn"), TxtDataProtocolloLEPSM.Text, TxtNumProtocolloLEPSM.Text)
                    End If
            End Select
        End If

            '************************************************************************************************************************************
            '****************************************************LetteraEsclusione***************************************************************
            '************************************************************************************************************************************
            If chkLetteraEsclusione.Checked = True Then
                imgGeneraFile.Visible = False
                hplLetteraEsclusione.Visible = True
                GruppoLEDD(True)
            If (idTipoProgetto = PROGETTO_GARANZIA_GIOVANI) Then
                hplLetteraEsclusione.NavigateUrl = Documento.VOL_LetteraEsclusioneGaranziaGiovani(Request.QueryString("IdVol"), Session("IdEnte"), Request.QueryString("idattivita"), Request.QueryString("CodiceFiscale"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                ClsUtility.CronologiaDocEntità(Request.QueryString("IdVol"), Session("Utente"), "Lettera Esclusione Volontario Garanzia Giovani", Session("conn"), TxtDataProtocolloLEDD.Text, TxtNumProtocolloLEDD.Text)
            Else
                hplLetteraEsclusione.NavigateUrl = Documento.VOL_LetteraEsclusione(Request.QueryString("IdVol"), Session("IdEnte"), Request.QueryString("idattivita"), Request.QueryString("CodiceFiscale"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                ClsUtility.CronologiaDocEntità(Request.QueryString("IdVol"), Session("Utente"), "Lettera Esclusione Volontario", Session("conn"), TxtDataProtocolloLEDD.Text, TxtNumProtocolloLEDD.Text)
            End If
        End If
        '************************************************************************************************************************************
        '****************************************************LetteraAssegnazioneRitornoMittente**********************************************
        '************************************************************************************************************************************
        If chkLetteraAssegnazioneRitornoMittente.Checked = True Then
            imgGeneraFile.Visible = False
            hplLetteraAssegnazioneRitornoMittente.Visible = True
            GruppoLARM(True)
            hplLetteraAssegnazioneRitornoMittente.NavigateUrl = Documento.VOL_letteraAssegnazioneRitornoMittente(Request.QueryString("IdVol"), Session("IdEnte"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
            ClsUtility.CronologiaDocEntità(Request.QueryString("IdVol"), Session("Utente"), "Lettera Assegnazione Ritorno al mittente", Session("conn"), TxtDataProtocolloLARM.Text, TxtNumProtocolloLARM.Text)
        End If
        '************************************************************************************************************************************
        '****************************************************DuplicatoletteraAssegnazioneEnte************************************************
        '************************************************************************************************************************************
        If chkDuplicatoletteraAssegnazioneEnte.Checked = True Then
            imgGeneraFile.Visible = False
            hplDuplicatoLetteraAssegnazioneEnte.Visible = True
            '********  Mauro Lanna
            GruppoDLA(True)
            '*******

            'chiamo la proprietà della classe GeneratoreModelli che ritorna la path del documento creato
            hplDuplicatoLetteraAssegnazioneEnte.NavigateUrl = Documento.VOL_duplicatoletteraAssegnazioneEnte(Request.QueryString("IdVol"), Session("IdEnte"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
            Documento.Dispose()
            'hplDownloadDuplicato.NavigateUrl = LetteraAssegnazioneDuplicato(Session("IdEnte"), "duplicatoletteraAssegnazioneEnte")
            'Cronologia creazione documento.
            ClsUtility.CronologiaDocEntità(Request.QueryString("IdVol"), Session("Utente"), "Lettera Assegnazione Volontario Duplicato", Session("conn"), TxtDataProtocolloDLA.Text, TxtNumProtocolloDLA.Text)
        End If
        '************************************************************************************************************************************
        '****************************************************AttoAggiuntivoVolontari************************************************
        '************************************************************************************************************************************
        If chkAttoAggiuntivo.Checked = True Then
            imgGeneraFile.Visible = False
            hplAttoAggiuntivo.Visible = True
            '********  Mauro Lanna
            GruppoDLA(True)
            '*******

            'chiamo la proprietà della classe GeneratoreModelli che ritorna la path del documento creato
            hplAttoAggiuntivo.NavigateUrl = Documento.VOL_AttoAggiuntivoVolontari(Request.QueryString("IdVol"), Session("IdEnte"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
            Documento.Dispose()
            'hplDownloadDuplicato.NavigateUrl = LetteraAssegnazioneDuplicato(Session("IdEnte"), "duplicatoletteraAssegnazioneEnte")
            'Cronologia creazione documento.
            ClsUtility.CronologiaDocEntità(Request.QueryString("IdVol"), Session("Utente"), "Atto Aggiuntivo Volontari", Session("conn"), TxtDataProtocolloDLA.Text, TxtNumProtocolloDLA.Text)
        End If
        '************************************************************************************************************************************
        '****************************************************LetteraAssegnazioneVolontario***************************************************
        '************************************************************************************************************************************
        'modificata aprile 2022 per suddivisione 4 modelli (ORD, GG, EST, SCD)
        If chkLetteraAssegnazioneVolontario.Checked = True Then

            imgGeneraFile.Visible = False
            hplLetteraAssegnazioneVolontario.Visible = True
            hplLetteraAssegnazioneVolontarioB.Visible = True
            '********  Mauro Lanna
            GruppoLV(True)
            GruppoLVB(True)
            '*******
            Dim strFlag As String = ""
            Dim strGruppo As String = ""

            strsql = "select a.idtipoprogetto as naz, isnull(identitàSubentrante,0) as  subentro, " & _
            " e.datainizioservizio, isnull(asa.datainiziodifferita, a.datainizioattività) as datainizioattività, bando.gruppo,case when datainizioservizio < '15/01/2019' then '<' else '>' end as Flag " & _
            " from Attività a " & _
            " inner join attivitàsediassegnazione asa on asa.idattività=a.idattività " & _
            " inner join graduatorieentità ge on ge.idattivitàsedeassegnazione=asa.idattivitàsedeassegnazione " & _
            " inner join entità e on e.idEntità=ge.idEntità " & _
            " inner join bandiattività ba on a.idbandoattività=ba.idbandoattività " & _
            " inner join bando on ba.idbando=bando.idbando " & _
            " left join cronologiasostituzioni cs on cs.identitàsubentrante=ge.idEntità " & _
            " where ge.idEntità=" & Request.QueryString("IdVol") & ""
            If Not dtrgenerico Is Nothing Then
                dtrgenerico.Close()
                dtrgenerico = Nothing
            End If

            dtrgenerico = ClsServer.CreaDatareader(strsql, Session("conn"))

            'Verifica se Volontario Estero o nazionale
            If dtrgenerico.HasRows = True Then
                dtrgenerico.Read()
                strFlag = dtrgenerico("Flag")
                strGruppo = dtrgenerico("gruppo")
                '-------------------------------------------------------------
                'Funzione per il calcolo dei giorni di permesso e di richiesta
                Dim dataInizServ As DateTime = dtrgenerico("datainizioservizio") '15
                Dim dataInizAtt As DateTime = dtrgenerico("datainizioattività") '20
                Dim intx As Integer = 0

                If dataInizServ <> dataInizAtt Then
                    Do While dataInizServ > dataInizAtt
                        intx += 1
                        dataInizAtt = dataInizAtt.AddDays(1)
                    Loop
                    Select Case intx
                        Case 1 To 15
                            Session("intGGP") = "20"
                            Session("intGGR") = "15"
                        Case 16 To 45
                            Session("intGGP") = "18"
                            Session("intGGR") = "14"
                        Case 46 To 75
                            Session("intGGP") = "16"
                            Session("intGGR") = "13"
                        Case Else
                            Session("intGGP") = "14"
                            Session("intGGR") = "11"
                    End Select
                End If

                '-------------------------------------------------------------

                If dtrgenerico("naz") = 1 Or dtrgenerico("naz") = 3 Or dtrgenerico("naz") = 5 Or dtrgenerico("naz") = 7 Or dtrgenerico("naz") = 9 Or dtrgenerico("naz") = 11 Then
                    If dtrgenerico("datainizioservizio") = dtrgenerico("datainizioattività") Then
                        If Not dtrgenerico Is Nothing Then
                            dtrgenerico.Close()
                            dtrgenerico = Nothing
                        End If

                        hplLetteraAssegnazioneVolontario.Visible = True
                        ''********  Mauro Lanna
                        'GruppoLV(True)
                        ''*******
                        'chiamo la proprietà della classe GeneratoreModelli che ritorna la path del documento creato
                        hplLetteraAssegnazioneVolontario.NavigateUrl = Documento.VOL_AssegnazioneVolontariNazionali(Request.QueryString("IdVol"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))

                        If strGruppo = "50" And strFlag = "<" Then
                            hplLetteraAssegnazioneVolontarioB.NavigateUrl = Documento.VOL_AssegnazioneVolontariNazionaliB_Integrativo(Request.QueryString("IdVol"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                        Else
                            Select Case strTipoModello
                                Case "ORD"
                                    hplLetteraAssegnazioneVolontarioB.NavigateUrl = Documento.VOL_AssegnazioneVolontariNazionaliB(Request.QueryString("IdVol"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                                Case "EST"
                                    hplLetteraAssegnazioneVolontarioB.NavigateUrl = Documento.VOL_AssegnazioneVolontariEsteroB(Request.QueryString("IdVol"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                                Case "GG"
                                    hplLetteraAssegnazioneVolontarioB.NavigateUrl = Documento.VOL_AssegnazioneVolontariGaranziaGiovaniB(Request.QueryString("IdVol"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                                Case "SCD"
                                    hplLetteraAssegnazioneVolontarioB.NavigateUrl = Documento.VOL_AssegnazioneVolontariNazionaliBSCD(Request.QueryString("IdVol"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                            End Select
                            'hplLetteraAssegnazioneVolontarioB.NavigateUrl = Documento.VOL_AssegnazioneVolontariNazionaliB(Request.QueryString("IdVol"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                        End If
                        Documento.Dispose()
                        ClsUtility.CronologiaDocEntità(Request.QueryString("IdVol"), Session("Utente"), "Assegnazione Volontario - Nazionale", Session("conn"), TxtDataProtocollo.Text, TxtNumProtocollo.Text)
                        ClsUtility.CronologiaDocEntità(Request.QueryString("IdVol"), Session("Utente"), "AssegnazioneVolontariNazionaliB", Session("conn"), TxtDataProtocolloB.Text, TxtNumProtocolloB.Text)

                        'Maurolanna alasar
                    Else
                        If Not dtrgenerico Is Nothing Then
                            dtrgenerico.Close()
                            dtrgenerico = Nothing
                        End If
                        'chiamo la proprietà della classe GeneratoreModelli che ritorna la path del documento creato
                        hplLetteraAssegnazioneVolontario.NavigateUrl = Documento.VOL_SostituzioneVolontariNazionali(Request.QueryString("IdVol"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                        If strGruppo = "50" And strFlag = "<" Then
                            hplLetteraAssegnazioneVolontarioB.NavigateUrl = Documento.VOL_SostituzioneVolontariNazionaliB_Integrativo(Request.QueryString("IdVol"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                        Else
                            Select Case strTipoModello
                                Case "ORD"
                                    hplLetteraAssegnazioneVolontarioB.NavigateUrl = Documento.VOL_SostituzioneVolontariNazionaliB(Request.QueryString("IdVol"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                                Case "EST"
                                    hplLetteraAssegnazioneVolontarioB.NavigateUrl = Documento.VOL_SostituzioneVolontariEsteriB(Request.QueryString("IdVol"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                                Case "GG"
                                    hplLetteraAssegnazioneVolontarioB.NavigateUrl = Documento.VOL_SostituzioneVolontariGaranziaGiovaniB(Request.QueryString("IdVol"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                                Case "SCD"
                                    hplLetteraAssegnazioneVolontarioB.NavigateUrl = Documento.VOL_SostituzioneVolontariNazionaliBSCD(Request.QueryString("IdVol"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                            End Select
                            'hplLetteraAssegnazioneVolontarioB.NavigateUrl = Documento.VOL_SostituzioneVolontariNazionaliB(Request.QueryString("IdVol"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                        End If
                        'hplLetteraAssegnazioneVolontarioB.NavigateUrl = Documento.VOL_SostituzioneVolontariNazionaliB(Request.QueryString("IdVol"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                        Documento.Dispose()

                        'Cronologia creazione documento.
                        'ClsUtility.CronologiaDocEntità(Request.QueryString("IdVol"), Session("Utente"), "Lettera Sostituzione Volontario - Nazionale", Session("conn"))
                        ClsUtility.CronologiaDocEntità(Request.QueryString("IdVol"), Session("Utente"), "Sostituzione Volontario - Nazionale", Session("conn"), TxtDataProtocollo.Text, TxtNumProtocollo.Text)
                        ClsUtility.CronologiaDocEntità(Request.QueryString("IdVol"), Session("Utente"), "SostituzioneVolontariNazionaliB", Session("conn"), TxtDataProtocolloB.Text, TxtNumProtocolloB.Text)
                    End If

                ElseIf dtrgenerico("naz") = 2 Or dtrgenerico("naz") = 6 Or dtrgenerico("naz") = 8 Or dtrgenerico("naz") = 10 Or dtrgenerico("naz") = 12 Then
                    If dtrgenerico("datainizioservizio") = dtrgenerico("datainizioattività") Then
                        If Not dtrgenerico Is Nothing Then
                            dtrgenerico.Close()
                            dtrgenerico = Nothing
                        End If

                        'chiamo la proprietà della classe GeneratoreModelli che ritorna la path del documento creato
                        hplLetteraAssegnazioneVolontario.NavigateUrl = Documento.VOL_AssegnazioneVolontariEstero(Request.QueryString("IdVol"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                        If strGruppo = "50" And strFlag = "<" Then
                            hplLetteraAssegnazioneVolontarioB.NavigateUrl = Documento.VOL_AssegnazioneVolontariEsteroB_Integrativo(Request.QueryString("IdVol"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                        Else
                            Select Case strTipoModello
                                Case "ORD"
                                    hplLetteraAssegnazioneVolontarioB.NavigateUrl = Documento.VOL_AssegnazioneVolontariNazionaliB(Request.QueryString("IdVol"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                                Case "EST"
                                    hplLetteraAssegnazioneVolontarioB.NavigateUrl = Documento.VOL_AssegnazioneVolontariEsteroB(Request.QueryString("IdVol"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                                Case "GG"
                                    hplLetteraAssegnazioneVolontarioB.NavigateUrl = Documento.VOL_AssegnazioneVolontariGaranziaGiovaniB(Request.QueryString("IdVol"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                                Case "SCD"
                                    hplLetteraAssegnazioneVolontarioB.NavigateUrl = Documento.VOL_AssegnazioneVolontariNazionaliBSCD(Request.QueryString("IdVol"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                            End Select
                            'hplLetteraAssegnazioneVolontarioB.NavigateUrl = Documento.VOL_AssegnazioneVolontariEsteroB(Request.QueryString("IdVol"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                        End If
                        'hplLetteraAssegnazioneVolontarioB.NavigateUrl = Documento.VOL_AssegnazioneVolontariEsteroB(Request.QueryString("IdVol"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                        Documento.Dispose()

                        'Cronologia creazione documento.
                        'ClsUtility.CronologiaDocEntità(Request.QueryString("IdVol"), Session("Utente"), "Lettera Assegnazione Volontario - Estero", Session("conn"))
                        ClsUtility.CronologiaDocEntità(Request.QueryString("IdVol"), Session("Utente"), "Assegnazione Volontario - Estero", Session("conn"), TxtDataProtocollo.Text, TxtNumProtocollo.Text)
                        ClsUtility.CronologiaDocEntità(Request.QueryString("IdVol"), Session("Utente"), "AssegnazioneVolontariEsteroB", Session("conn"), TxtDataProtocolloB.Text, TxtNumProtocolloB.Text)
                    Else
                        If Not dtrgenerico Is Nothing Then
                            dtrgenerico.Close()
                            dtrgenerico = Nothing
                        End If
                        'chiamo la proprietà della classe GeneratoreModelli che ritorna la path del documento creato
                        hplLetteraAssegnazioneVolontario.NavigateUrl = Documento.VOL_SostituzioneVolontariEsteri(Request.QueryString("IdVol"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                        If strGruppo = "50" And strFlag = "<" Then
                            hplLetteraAssegnazioneVolontarioB.NavigateUrl = Documento.VOL_SostituzioneVolontariEsteriB_Integrativo(Request.QueryString("IdVol"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                        Else
                            Select Case strTipoModello
                                Case "ORD"
                                    hplLetteraAssegnazioneVolontarioB.NavigateUrl = Documento.VOL_SostituzioneVolontariNazionaliB(Request.QueryString("IdVol"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                                Case "EST"
                                    hplLetteraAssegnazioneVolontarioB.NavigateUrl = Documento.VOL_SostituzioneVolontariEsteriB(Request.QueryString("IdVol"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                                Case "GG"
                                    hplLetteraAssegnazioneVolontarioB.NavigateUrl = Documento.VOL_SostituzioneVolontariGaranziaGiovaniB(Request.QueryString("IdVol"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                                Case "SCD"
                                    hplLetteraAssegnazioneVolontarioB.NavigateUrl = Documento.VOL_SostituzioneVolontariNazionaliBSCD(Request.QueryString("IdVol"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                            End Select
                            'hplLetteraAssegnazioneVolontarioB.NavigateUrl = Documento.VOL_SostituzioneVolontariEsteriB(Request.QueryString("IdVol"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                        End If
                        'hplLetteraAssegnazioneVolontarioB.NavigateUrl = Documento.VOL_SostituzioneVolontariEsteriB(Request.QueryString("IdVol"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                        Documento.Dispose()

                        'Cronologia creazione documento.
                        'ClsUtility.CronologiaDocEntità(Request.QueryString("IdVol"), Session("Utente"), "Lettera Sostituzione Volontario - Estero", Session("conn"))
                        ClsUtility.CronologiaDocEntità(Request.QueryString("IdVol"), Session("Utente"), "Sostituzione Volontario - Estero", Session("conn"), TxtDataProtocollo.Text, TxtNumProtocollo.Text)
                        ClsUtility.CronologiaDocEntità(Request.QueryString("IdVol"), Session("Utente"), "SostituzioneVolontariEsteriB", Session("conn"), TxtDataProtocolloB.Text, TxtNumProtocolloB.Text)
                    End If


                ElseIf dtrgenerico("naz") = 4 Then

                    If dtrgenerico("datainizioservizio") = dtrgenerico("datainizioattività") Then
                        If Not dtrgenerico Is Nothing Then
                            dtrgenerico.Close()
                            dtrgenerico = Nothing
                        End If

                        hplLetteraAssegnazioneVolontario.Visible = True

                        'chiamo la proprietà della classe GeneratoreModelli che ritorna la path del documento creato
                        hplLetteraAssegnazioneVolontario.NavigateUrl = Documento.VOL_AssegnazioneVolontariGaranziaGiovani(Request.QueryString("IdVol"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                        'modificato il 07/04/2015 da simona cordella
                        'If ClsUtility.ControlloEntitàSPCPerGenerazioneContratto(Request.QueryString("IdVol"), Session("conn")) = False Then
                        '    Select Case strTipoModello
                        '        Case "ORD"
                        '            hplLetteraAssegnazioneVolontarioB.NavigateUrl = Documento.VOL_AssegnazioneVolontariNazionaliB(Request.QueryString("IdVol"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                        '        Case "EST"
                        '            hplLetteraAssegnazioneVolontarioB.NavigateUrl = Documento.VOL_AssegnazioneVolontariEsteroB(Request.QueryString("IdVol"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                        '        Case "GG"
                        '            hplLetteraAssegnazioneVolontarioB.NavigateUrl = Documento.VOL_AssegnazioneVolontariGaranziaGiovaniB(Request.QueryString("IdVol"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                        '        Case "SCD"
                        '            hplLetteraAssegnazioneVolontarioB.NavigateUrl = Documento.VOL_AssegnazioneVolontariNazionaliBSCD(Request.QueryString("IdVol"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                        '    End Select
                        '    'hplLetteraAssegnazioneVolontarioB.NavigateUrl = Documento.VOL_AssegnazioneVolontariGaranziaGiovaniB(Request.QueryString("IdVol"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                        '    strNomeDocumento = "AssegnazioneVolontariGaranziaGiovaniB"
                        'Else ' genero contratto Contratto Volontario Italia (12 mesi) (Garanzia Giovani Senza Presa In Carico)
                        '    hplLetteraAssegnazioneVolontarioB.NavigateUrl = Documento.VOL_AssegnazioneVolontariGaranziaGiovaniBSPC(Request.QueryString("IdVol"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                        '    strNomeDocumento = "AssegnazioneVolontariGaranziaGiovaniBSPC"
                        'End If
                        Select Case strTipoModello
                            Case "ORD"
                                hplLetteraAssegnazioneVolontarioB.NavigateUrl = Documento.VOL_AssegnazioneVolontariNazionaliB(Request.QueryString("IdVol"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                                strNomeDocumento = "AssegnazioneVolontariNazionaliB"
                            Case "EST"
                                hplLetteraAssegnazioneVolontarioB.NavigateUrl = Documento.VOL_AssegnazioneVolontariEsteroB(Request.QueryString("IdVol"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                                strNomeDocumento = "AssegnazioneVolontariEsteroB"
                            Case "GG"
                                hplLetteraAssegnazioneVolontarioB.NavigateUrl = Documento.VOL_AssegnazioneVolontariGaranziaGiovaniB(Request.QueryString("IdVol"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                                strNomeDocumento = "AssegnazioneVolontariGaranziaGiovaniB"
                            Case "SCD"
                                hplLetteraAssegnazioneVolontarioB.NavigateUrl = Documento.VOL_AssegnazioneVolontariNazionaliBSCD(Request.QueryString("IdVol"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                                strNomeDocumento = "AssegnazioneVolontariNazionaliBSCD"
                        End Select
                        'hplLetteraAssegnazioneVolontarioB.NavigateUrl = Documento.VOL_AssegnazioneVolontariGaranziaGiovaniB(Request.QueryString("IdVol"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))

                        Documento.Dispose()

                        'Cronologia creazione documento.
                        ClsUtility.CronologiaDocEntità(Request.QueryString("IdVol"), Session("Utente"), "Assegnazione Volontario - Garanzia Giovani", Session("conn"), TxtDataProtocollo.Text, TxtNumProtocollo.Text)
                        ClsUtility.CronologiaDocEntità(Request.QueryString("IdVol"), Session("Utente"), strNomeDocumento, Session("conn"), TxtDataProtocolloB.Text, TxtNumProtocolloB.Text)

                        'Maurolanna alasar
                    Else
                        If Not dtrgenerico Is Nothing Then
                            dtrgenerico.Close()
                            dtrgenerico = Nothing
                        End If
                        'chiamo la proprietà della classe GeneratoreModelli che ritorna la path del documento creato
                        hplLetteraAssegnazioneVolontario.NavigateUrl = Documento.VOL_SostituzioneVolontariGaranziaGiovani(Request.QueryString("IdVol"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                        'modificato il 07/04/2015 da simona cordella
                        'If ClsUtility.ControlloEntitàSPCPerGenerazioneContratto(Request.QueryString("IdVol"), Session("conn")) = False Then
                        '    Select Case strTipoModello
                        '        Case "ORD"
                        '            hplLetteraAssegnazioneVolontarioB.NavigateUrl = Documento.VOL_SostituzioneVolontariNazionaliB(Request.QueryString("IdVol"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                        '        Case "EST"
                        '            hplLetteraAssegnazioneVolontarioB.NavigateUrl = Documento.VOL_SostituzioneVolontariEsteriB(Request.QueryString("IdVol"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                        '        Case "GG"
                        '            hplLetteraAssegnazioneVolontarioB.NavigateUrl = Documento.VOL_SostituzioneVolontariGaranziaGiovaniB(Request.QueryString("IdVol"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                        '        Case "SCD"
                        '            hplLetteraAssegnazioneVolontarioB.NavigateUrl = Documento.VOL_SostituzioneVolontariNazionaliBSCD(Request.QueryString("IdVol"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                        '    End Select
                        '    'hplLetteraAssegnazioneVolontarioB.NavigateUrl = Documento.VOL_SostituzioneVolontariGaranziaGiovaniB(Request.QueryString("IdVol"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                        '    strNomeDocumento = "SostituzioneVolontariGaranziaGiovaniB"
                        'Else ' genero contratto Contratto Volontario Italia (12 mesi) (Garanzia Giovani Senza Presa In Carico)
                        '    hplLetteraAssegnazioneVolontarioB.NavigateUrl = Documento.VOL_SostituzioneVolontariGaranziaGiovaniBSPC(Request.QueryString("IdVol"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                        '    strNomeDocumento = "SostituzioneVolontariGaranziaGiovaniBSPC"
                        'End If
                        Select Case strTipoModello
                            Case "ORD"
                                hplLetteraAssegnazioneVolontarioB.NavigateUrl = Documento.VOL_SostituzioneVolontariNazionaliB(Request.QueryString("IdVol"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                                strNomeDocumento = "SostituzioneVolontariNazionaliB"
                            Case "EST"
                                hplLetteraAssegnazioneVolontarioB.NavigateUrl = Documento.VOL_SostituzioneVolontariEsteriB(Request.QueryString("IdVol"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                                strNomeDocumento = "SostituzioneVolontariEsteriB"
                            Case "GG"
                                hplLetteraAssegnazioneVolontarioB.NavigateUrl = Documento.VOL_SostituzioneVolontariGaranziaGiovaniB(Request.QueryString("IdVol"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                                strNomeDocumento = "SostituzioneVolontariGaranziaGiovaniB"
                            Case "SCD"
                                hplLetteraAssegnazioneVolontarioB.NavigateUrl = Documento.VOL_SostituzioneVolontariNazionaliBSCD(Request.QueryString("IdVol"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                                strNomeDocumento = "SostituzioneVolontariNazionaliBSCD"
                        End Select


                        Documento.Dispose()

                        'Cronologia creazione documento.
                        'ClsUtility.CronologiaDocEntità(Request.QueryString("IdVol"), Session("Utente"), "Lettera Sostituzione Volontario - Nazionale", Session("conn"))
                        ClsUtility.CronologiaDocEntità(Request.QueryString("IdVol"), Session("Utente"), "Sostituzione Volontario - Garanzia Giovani", Session("conn"), TxtDataProtocollo.Text, TxtNumProtocollo.Text)
                        ClsUtility.CronologiaDocEntità(Request.QueryString("IdVol"), Session("Utente"), strNomeDocumento, Session("conn"), TxtDataProtocolloB.Text, TxtNumProtocolloB.Text)
                    End If

                End If

            End If 'dtrgenerico
        End If 'chkLetteraAssegnazioneVolontario.Checked 
        Documento.Dispose()

        '************************************************************************************************************************************
        '****************************************  Certificato servizio svolto  *************************************************************
        '************************************************************************************************************************************
        'If Me.chkCertificatoServizioSvolto.Checked Then
        '    Me.imgGeneraFile.Visible = False
        '    Me.GeneraCertificatoServizioSvolto(Request.QueryString("IdVol"))
        'End If
        If chkCertificatoServizioSvolto.Checked = True Then
            imgGeneraFile.Visible = False
            hplCertificatoServizioSvolto.Visible = True

            'chiamo la proprietà della classe GeneratoreModelli che ritorna la path del documento creato
            hplCertificatoServizioSvolto.NavigateUrl = Documento.VOL_CertificatoServizioSvolto(Request.QueryString("IdVol"), Session("IdEnte"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
            Documento.Dispose()
            'hplDownloadDuplicato.NavigateUrl = LetteraAssegnazioneDuplicato(Session("IdEnte"), "duplicatoletteraAssegnazioneEnte")
            'Cronologia creazione documento.
            ClsUtility.CronologiaDocEntità(Request.QueryString("IdVol"), Session("Utente"), "Atto Aggiuntivo Volontari", Session("conn"), TxtDataProtocolloDLA.Text, TxtNumProtocolloDLA.Text)
        End If
    End Sub

    Function VerificaCausale(ByVal intIdVol As Integer) As Integer
        Dim dtrgenerico As SqlClient.SqlDataReader
        Dim strsql As String

        'devi fa na select
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If

        'estraggo il fascicolo del progetto
        strsql = "SELECT isnull(idcausalechiusura, 1) as causale from entità where identità=" & intIdVol

        dtrgenerico = ClsServer.CreaDatareader(strsql, Session("conn"))

        If dtrgenerico.HasRows = True Then
            dtrgenerico.Read()
            'eccedenza mal dopo 6 mesi
            If dtrgenerico("causale") = 35 Then
                VerificaCausale = 0
            Else
                VerificaCausale = 1
            End If
        End If

        'devi fa na select
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If

        Return VerificaCausale

    End Function

    Private Sub imgChiudi_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles imgChiudi.Click
        Response.Redirect("WfrmVolontari.aspx?VengoDA=" & Request.QueryString("VengoDA") & "&idattivita=" & Request.QueryString("IdAttivita") & "&IdVol=" & Request.QueryString("IdVol"))

    End Sub

    Sub NuovaCronologia(ByVal strDocumento As String, ByVal IdAttivitaSedeAssegnazione As Integer, Optional ByRef DataProt As String = "", Optional ByRef NProt As String = "")
        'vado a fare la insert
        Dim cmdinsert As Data.SqlClient.SqlCommand
        Dim strsql As String

        strsql = "insert into CronologiaEntiDocumenti (IDente,UserName,DataDocumento,Documento,TipoDocumento,IdAttivitàSedeAssegnazione,DataProt,NProt) "
        strsql = strsql & "values "
        strsql = strsql & "(" & CInt(Session("IdEnte")) & ",'" & Session("Utente") & "',GetDate(),'" & strDocumento & "',2, " & IdAttivitaSedeAssegnazione & ","
        If DataProt = "" Then
            strsql = strsql & " null,"
        Else
            strsql = strsql & " '" & DataProt & "',"
        End If
        If NProt = "" Then
            strsql = strsql & " null"
        Else
            strsql = strsql & "'" & NProt & "'"
        End If
        strsql = strsql & ")"
        cmdinsert = New SqlClient.SqlCommand(strsql, Session("conn"))
        cmdinsert.ExecuteNonQuery()

        cmdinsert.Dispose()
    End Sub


    Protected Function TipologiaProgettoDaIdEntita(ByVal idEntita As String, ByVal connection As SqlConnection) As String
        Dim query As String
        Dim idTipoProgetto As String
        Dim dataReader As SqlDataReader

        query = "SELECT     attività.IdTipoProgetto" & _
        " from entità  " & _
        " INNER JOIN attività ON entità.TMPCodiceProgetto = attività.CodiceEnte " & _
        " WHERE entità.IDEntità =" & idEntita & ""

        dataReader = ClsServer.CreaDatareader(query, connection)
        If dataReader.HasRows = True Then
            dataReader.Read()
            idTipoProgetto = dataReader("IdTipoProgetto")
        End If
        dataReader.Close()
        Return idTipoProgetto
    End Function

    Protected Function TipologiaProgettoDaIdAttivita(ByVal idAttivita As String, ByVal connection As SqlConnection) As String
        Dim query As String
        Dim idTipoProgetto As String
        Dim dataReader As SqlDataReader

        query = "SELECT     attività.IdTipoProgetto" & _
        " FROM dbo.attività  " & _
        " WHERE IDAttività =" & idAttivita & ""
        dataReader = ClsServer.CreaDatareader(query, connection)

        If dataReader.HasRows = True Then
            dataReader.Read()
            idTipoProgetto = dataReader("IdTipoProgetto")
        End If
        dataReader.Close()
        Return idTipoProgetto
    End Function
    'Private Function ControlloRequisitiEntitàPerGenerazioneContratto(ByVal IDVolontario As Integer, ByVal connection As SqlConnection) As Boolean
    '    '** Creata da Simona Cordella
    '    '** Il 07/04/2015
    '    '** Funzione che verifica se il campo REQUISITO nella tabella Entità sia <> da SI; s
    '    '** stampo in modello del contratto (Contratto Volontario Italia (12 mesi) (Garanzia Giovani Senza Presa In Carico)
    '    '** Contratto Volontario Italia (subentro) (Garanzia Giovani Senza Presa In Carico))
    '    Dim query As String
    '    Dim dataReader As SqlDataReader
    '    Dim blnRequisito As Boolean

    '    query = " SELECT IDEntità " & _
    '            " FROM Entità  " & _
    '            " WHERE IDEntità =" & IDVolontario & " and UCASE(isnull(Requisito,'')) <> 'SI'"
    '    dataReader = ClsServer.CreaDatareader(query, connection)
    '    blnRequisito = dataReader.HasRows

    '    dataReader.Close()
    '    dataReader = Nothing

    '    Return blnRequisito
    'End Function

    ''' <summary>
    ''' Imposta la visibilità della CheckBox per generare il certificato di servizio svolto dal volontario
    ''' </summary>
    ''' <param name="idVolontario">ID del volontario</param>
    ''' <returns>Boolean</returns>
    Private Function ImpostaVisibilitaCBCertificatoServizioSvolto(ByVal idVolontario As String) As Boolean

        Dim stringaSQL As String
        Dim dataReader As SqlDataReader
        Dim cmd As SqlCommand
        Dim IDStatoEntita As Integer
        Dim dataFineServizio As DateTime

        'Controllo formale su ID Volontario
        If String.IsNullOrEmpty(idVolontario) Then
            Return False
        End If

        'Query SQL per estrarre il totale presenze del volontario
        stringaSQL = "SELECT IDStatoEntità, DataFineServizio FROM entità WHERE IDEntità = " & idVolontario

        Try
            'Inizializza comando
            cmd = New SqlCommand With {
                .Connection = CType(Session("conn"), SqlConnection),
                .CommandType = CommandType.Text,
                .CommandText = stringaSQL
            }

            'lettura dati
            dataReader = cmd.ExecuteReader()
            If dataReader.Read() Then
                IDStatoEntita = CInt(dataReader("IDStatoEntità"))
                dataFineServizio = CType(dataReader("DataFineServizio"), DateTime)
            End If

        Catch ex As Exception

            IDStatoEntita = 0

        Finally

            If dataReader IsNot Nothing Then
                dataReader.Close()
            End If

        End Try

        'Restituisce true se la data fine servizio è minore della data corrente
        'e lo stato del volontario è uguale ad uno dei seguenti valori:
        ' - 3 (In Servizio)
        ' - 5 (Chiuso Durante Servizio)
        ' - 6 (Servizio Terminato)
        If (dataFineServizio < DateTime.Now.Date) _
        And (IDStatoEntita = 3 Or IDStatoEntita = 5 Or IDStatoEntita = 6) Then
            Return True
        End If

        Return False

    End Function

    ''' <summary>
    ''' Restituisce i dati necessari per generare il certificato servizio svolto del volontario
    ''' </summary>
    ''' <param name="idVolontario">ID del volontario</param>
    ''' <returns>SqlDataReader con i dati del volontario</returns>
    Private Function GetDatiVolontario(ByVal idVolontario) As SqlDataReader

        Dim stringaSQL As String
        Dim dataReader As SqlDataReader
        Dim cmd As SqlCommand

        'Query SQL per estrarre i dati del volontario
        stringaSQL = " SELECT Nominativo, CodiceFiscale, DataNascita, DataInizioServizio, DataFineServizio "
        stringaSQL &= " FROM VW_EDITOR_VOLONTARI "
        stringaSQL &= " WHERE IDEntità = " & idVolontario

        Try
            'Inizializza comando
            cmd = New SqlCommand With {
                .Connection = CType(Session("conn"), SqlConnection),
                .CommandType = CommandType.Text,
                .CommandText = stringaSQL
            }

            'lettura dati
            dataReader = cmd.ExecuteReader()

        Catch ex As Exception

            If dataReader IsNot Nothing Then
                dataReader.Close()
            End If

        End Try

        Return dataReader

    End Function

    ''' <summary>
    ''' Genera il certificato di servizio svolto del volontario
    ''' </summary>
    ''' <param name="idVolontario">ID del volontario</param>
    Private Sub GeneraCertificatoServizioSvolto(ByVal idVolontario As String)

        Dim documento As AsposeWord
        Dim dataReader As SqlDataReader

        'Codice fiscale per comporre il dome del PDF generato
        Dim docCodiceFiscale As String

        'Controllo formale su ID Volontario
        If String.IsNullOrEmpty(idVolontario) Then
            Me.lblmessaggiosopra.Visible = True
            Me.lblmessaggiosopra.Text = "Si è verificato un errore durante la generazione del certificato servizio svolto"

            Exit Sub
        End If

        'lettura dei dati necessari per compilare il documento
        dataReader = Me.GetDatiVolontario(idVolontario)

        'Controllo esito lettura
        If dataReader Is Nothing Then
            Me.lblmessaggiosopra.Visible = True
            Me.lblmessaggiosopra.Text = "Si è verificato un errore durante la generazione del certificato servizio svolto"

            Exit Sub
        End If

        If Not dataReader.HasRows Then
            dataReader.Close()
            Me.lblmessaggiosopra.Visible = True
            Me.lblmessaggiosopra.Text = "Si è verificato un errore durante la generazione del certificato servizio svolto"

            Exit Sub
        End If

        Try
            'Apertura documento per inserire i dati letti da DB
            documento = New AsposeWord()
            documento.open(Me.PATH_DOC_CertificatoServizioSvolto)

            'Inserimento dati del volontario nel documento
            dataReader.Read()

            'Volontario
            documento.doc.Range.Fields(0).Result = dataReader("Nominativo").ToString()

            'CodiceFiscale
            documento.doc.Range.Fields(1).Result = dataReader("CodiceFiscale").ToString()
            docCodiceFiscale = dataReader("CodiceFiscale").ToString()

            'DataNascita
            documento.doc.Range.Fields(2).Result = CType(dataReader("DataNascita"), DateTime).ToString("dd/MM/yyyy")

            'DataInizioServizio
            documento.doc.Range.Fields(3).Result = CType(dataReader("DataInizioServizio"), DateTime).ToString("dd/MM/yyyy")

            'DataFineServizio
            documento.doc.Range.Fields(4).Result = CType(dataReader("DataFineServizio"), DateTime).ToString("dd/MM/yyyy")

            'Data corrente
            documento.doc.Range.Fields(6).Result = DateTime.Now.ToString("dd/MM/yyyy")

            dataReader.Close()

            Response.Clear()
            Response.ContentType = "Application/pdf"
            Response.AddHeader("Content-Disposition", "attachment; filename=" & "CertificatoServizioSvolto_" & docCodiceFiscale & ".pdf")
            Response.BinaryWrite(documento.pdfBytes)
            Response.End()

        Catch ex As Exception

            If dataReader IsNot Nothing Then
                If Not dataReader.IsClosed Then
                    dataReader.Close()
                End If
            End If

            Me.lblmessaggiosopra.Visible = True
            Me.lblmessaggiosopra.Text = "Si è verificato un errore durante la generazione del certificato servizio svolto"
            Exit Sub

        End Try

    End Sub


End Class
