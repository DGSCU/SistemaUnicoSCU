Public Class WfrmElencoDocumentazioneSostituzioneVolontario
    Inherits System.Web.UI.Page

#Region " Codice generato da Progettazione Web Form "


    'Chiamata richiesta da Progettazione Web Form.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents txtNumProtAV As System.Web.UI.WebControls.TextBox
    Protected WithEvents ImgProtolloAV As System.Web.UI.WebControls.Image
    Protected WithEvents ImgApriAllegatiAV As System.Web.UI.WebControls.Image
    Protected WithEvents ImgProtocollazioneAV As System.Web.UI.WebControls.Image
    Protected WithEvents LblDataProtRSV1 As System.Web.UI.WebControls.Label
    Protected WithEvents txtDataProtRSV1 As System.Web.UI.WebControls.TextBox
    Protected WithEvents lblCodiceFascicoloP As System.Web.UI.WebControls.Label
    Protected WithEvents TxtCodiceFascicoloP As System.Web.UI.WebControls.TextBox
    Protected WithEvents TxtIdFascP As TextBox
    Protected WithEvents cmdSelFascicoloP As System.Web.UI.WebControls.Image
    Protected WithEvents cmdSelProtocollo0P As System.Web.UI.WebControls.Image
    Protected WithEvents cmdFascCancP As System.Web.UI.WebControls.ImageButton
    Protected WithEvents LblDescFascP As System.Web.UI.WebControls.Label
    Protected WithEvents txtDescFascP As System.Web.UI.WebControls.TextBox
    Protected WithEvents chkElencoVolontariAmmessi As System.Web.UI.WebControls.CheckBox
    Protected WithEvents Label1 As System.Web.UI.WebControls.Label
    Protected WithEvents hplElencoVolontariAmmessi As System.Web.UI.WebControls.HyperLink
    Protected WithEvents LblNumProtEVA As System.Web.UI.WebControls.Label
    Protected WithEvents txtNumProtEVA As System.Web.UI.WebControls.TextBox
    Protected WithEvents ImgProtolloEVA As System.Web.UI.WebControls.Image
    Protected WithEvents ImgApriAllegatiEVA As System.Web.UI.WebControls.Image
    Protected WithEvents ImgProtocollazioneEVA As System.Web.UI.WebControls.Image
    Protected WithEvents LblDataProtEVA As System.Web.UI.WebControls.Label
    Protected WithEvents txtDataProtEVA As System.Web.UI.WebControls.TextBox
    Protected WithEvents chkRinunciaserviziovolontarioMultipla As System.Web.UI.WebControls.CheckBox
    Protected WithEvents Label5 As System.Web.UI.WebControls.Label
    Protected WithEvents hplRinunciaserviziovolontarioMultipla As System.Web.UI.WebControls.HyperLink
    Protected WithEvents LblNumProtRSVM As System.Web.UI.WebControls.Label
    Protected WithEvents txtNumProtRSVM As System.Web.UI.WebControls.TextBox
    Protected WithEvents ImgProtolloRSVM As System.Web.UI.WebControls.Image
    Protected WithEvents ImgApriAllegatiRSVM As System.Web.UI.WebControls.Image
    Protected WithEvents ImgProtocollazioneRSVM As System.Web.UI.WebControls.Image
    Protected WithEvents LblDataProtRSVM As System.Web.UI.WebControls.Label
    Protected WithEvents txtDataProtRSVM As System.Web.UI.WebControls.TextBox
    Protected WithEvents hplRinunciaserviziovolontarioMultiplaCopiaReg As System.Web.UI.WebControls.HyperLink
    Protected WithEvents LblNumProtRSVMCopiaReg As System.Web.UI.WebControls.Label
    Protected WithEvents txtNumProtRSVMCopiaReg As System.Web.UI.WebControls.TextBox
    Protected WithEvents ImgProtolloRSVMCopiaReg As System.Web.UI.WebControls.Image
    Protected WithEvents ImgApriAllegatiRSVMCopiaReg As System.Web.UI.WebControls.Image
    Protected WithEvents ImgProtocollazioneRSVMCopiaReg As System.Web.UI.WebControls.Image
    Protected WithEvents LblDataProtRSVMCopiaReg As System.Web.UI.WebControls.Label
    Protected WithEvents txtDataProtRSVMCopiaReg As System.Web.UI.WebControls.TextBox
    Protected WithEvents lblCodiceFascicoloVSos As System.Web.UI.WebControls.Label
    Protected WithEvents TxtCodiceFascicoloVSos As System.Web.UI.WebControls.TextBox
    Protected WithEvents TxtIdFascVSos As TextBox
    Protected WithEvents cmdSelFascicoloVSos As System.Web.UI.WebControls.Image
    Protected WithEvents cmdSelProtocollo0VSos As System.Web.UI.WebControls.Image
    Protected WithEvents cmdFascCancVSos As System.Web.UI.WebControls.ImageButton
    Protected WithEvents LblDescFascVSos As System.Web.UI.WebControls.Label
    Protected WithEvents txtDescFascVSos As System.Web.UI.WebControls.TextBox
    Protected WithEvents chkRinunciaserviziovolontario As System.Web.UI.WebControls.CheckBox
    Protected WithEvents Label4 As System.Web.UI.WebControls.Label
    Protected WithEvents hplRinunciaserviziovolontario As System.Web.UI.WebControls.HyperLink
    Protected WithEvents LblNumProtRSV As System.Web.UI.WebControls.Label
    Protected WithEvents txtNumProtRSV As System.Web.UI.WebControls.TextBox
    Protected WithEvents ImgProtolloRSV As System.Web.UI.WebControls.Image
    Protected WithEvents ImgApriAllegatiRSV As System.Web.UI.WebControls.Image
    Protected WithEvents ImgProtocollazioneRSV As System.Web.UI.WebControls.Image
    Protected WithEvents LblDataProtRSV As System.Web.UI.WebControls.Label
    Protected WithEvents txtDataProtRSV As System.Web.UI.WebControls.TextBox
    Protected WithEvents hplRinunciaserviziovolontarioCopiaReg As System.Web.UI.WebControls.HyperLink
    Protected WithEvents LblNumProtRSVCopiaReg As System.Web.UI.WebControls.Label
    Protected WithEvents txtNumProtRSVCopiaReg As System.Web.UI.WebControls.TextBox
    Protected WithEvents ImgProtolloRSVCopiaReg As System.Web.UI.WebControls.Image
    Protected WithEvents ImgApriAllegatiRSVCopiaReg As System.Web.UI.WebControls.Image
    Protected WithEvents ImgProtocollazioneRSVCopiaReg As System.Web.UI.WebControls.Image
    Protected WithEvents LblDataProtRSVCopiaReg As System.Web.UI.WebControls.Label
    Protected WithEvents txtDataProtRSVCopiaReg As System.Web.UI.WebControls.TextBox
    Protected WithEvents chkLetteraChiusura As System.Web.UI.WebControls.CheckBox
    Protected WithEvents lblart10IIstep As System.Web.UI.WebControls.Label
    Protected WithEvents hplLetteraChiusura As System.Web.UI.WebControls.HyperLink
    Protected WithEvents LblNumProtLCI As System.Web.UI.WebControls.Label
    Protected WithEvents txtNumProtLCI As System.Web.UI.WebControls.TextBox
    Protected WithEvents ImgProtolloLCI As System.Web.UI.WebControls.Image
    Protected WithEvents ImgApriAllegatiLCI As System.Web.UI.WebControls.Image
    Protected WithEvents ImgProtocollazioneLCI As System.Web.UI.WebControls.Image
    Protected WithEvents LblDataProtLCI As System.Web.UI.WebControls.Label
    Protected WithEvents txtDataProtLCI As System.Web.UI.WebControls.TextBox
    Protected WithEvents chkLetteraChiusuraInServizio As System.Web.UI.WebControls.CheckBox
    Protected WithEvents Label2 As System.Web.UI.WebControls.Label
    Protected WithEvents hplLetteraChiusuraInServizio As System.Web.UI.WebControls.HyperLink
    Protected WithEvents LblNumProtLCS As System.Web.UI.WebControls.Label
    Protected WithEvents txtNumProtLCS As System.Web.UI.WebControls.TextBox
    Protected WithEvents ImgProtolloLCS As System.Web.UI.WebControls.Image
    Protected WithEvents ImgApriAllegatiLCS As System.Web.UI.WebControls.Image
    Protected WithEvents ImgProtocollazioneLCS As System.Web.UI.WebControls.Image
    Protected WithEvents LblDataProtLCS As System.Web.UI.WebControls.Label
    Protected WithEvents txtDataProtLCS As System.Web.UI.WebControls.TextBox
    Protected WithEvents chkLetteraEsclusionePerAssenzaIngiustificata As System.Web.UI.WebControls.CheckBox
    Protected WithEvents Label3 As System.Web.UI.WebControls.Label
    Protected WithEvents hplLetteraEsclusionePerAssenzaIngiustificata As System.Web.UI.WebControls.HyperLink
    Protected WithEvents LblNumProtLEAI As System.Web.UI.WebControls.Label
    Protected WithEvents txtNumProtLEAI As System.Web.UI.WebControls.TextBox
    Protected WithEvents ImgProtolloLEAI As System.Web.UI.WebControls.Image
    Protected WithEvents ImgApriAllegatiLEAI As System.Web.UI.WebControls.Image
    Protected WithEvents ImgProtocollazioneLEAI As System.Web.UI.WebControls.Image
    Protected WithEvents LblDataProtLEAI As System.Web.UI.WebControls.Label
    Protected WithEvents txtDataProtLEAI As System.Web.UI.WebControls.TextBox
    Protected WithEvents chkLetteraEsclusionePerGiorniPermesso As System.Web.UI.WebControls.CheckBox
    Protected WithEvents Label6 As System.Web.UI.WebControls.Label
    Protected WithEvents hplLetteraEsclusionePerGiorniPermesso As System.Web.UI.WebControls.HyperLink
    Protected WithEvents LblNumProtLEGP As System.Web.UI.WebControls.Label
    Protected WithEvents txtNumProtLEGP As System.Web.UI.WebControls.TextBox
    Protected WithEvents ImgProtolloLEGP As System.Web.UI.WebControls.Image
    Protected WithEvents ImgApriAllegatiLEGP As System.Web.UI.WebControls.Image
    Protected WithEvents ImgProtocollazioneLEGP As System.Web.UI.WebControls.Image
    Protected WithEvents LblDataProtLEGP As System.Web.UI.WebControls.Label
    Protected WithEvents txtDataProtLEGP As System.Web.UI.WebControls.TextBox
    Protected WithEvents chkLetteraEsclusionePerSuperamentoMalattia As System.Web.UI.WebControls.CheckBox
    Protected WithEvents Label7 As System.Web.UI.WebControls.Label
    Protected WithEvents hplLetteraEsclusionePerSuperamentoMalattia As System.Web.UI.WebControls.HyperLink
    Protected WithEvents LblNumProtLESM As System.Web.UI.WebControls.Label
    Protected WithEvents txtNumProtLESM As System.Web.UI.WebControls.TextBox
    Protected WithEvents ImgProtolloLESM As System.Web.UI.WebControls.Image
    Protected WithEvents ImgApriAllegatiLESM As System.Web.UI.WebControls.Image
    Protected WithEvents ImgProtocollazioneLESM As System.Web.UI.WebControls.Image
    Protected WithEvents LblDataProtLESM As System.Web.UI.WebControls.Label
    Protected WithEvents txtDataProtLESM As System.Web.UI.WebControls.TextBox
    Protected WithEvents chkLetteraEsclusione As System.Web.UI.WebControls.CheckBox
    Protected WithEvents Label8 As System.Web.UI.WebControls.Label
    Protected WithEvents hplLetteraEsclusione As System.Web.UI.WebControls.HyperLink
    Protected WithEvents LblNumProtLEDD As System.Web.UI.WebControls.Label
    Protected WithEvents txtNumProtLEDD As System.Web.UI.WebControls.TextBox
    Protected WithEvents ImgProtolloLEDD As System.Web.UI.WebControls.Image
    Protected WithEvents ImgApriAllegatiLEDD As System.Web.UI.WebControls.Image
    Protected WithEvents ImgProtocollazioneLEDD As System.Web.UI.WebControls.Image
    Protected WithEvents LblDataProtLEDD As System.Web.UI.WebControls.Label
    Protected WithEvents txtDataProtLEDD As System.Web.UI.WebControls.TextBox
    Protected WithEvents lblCodiceFascicoloVSub As System.Web.UI.WebControls.Label
    Protected WithEvents TxtCodiceFascicoloVSub As System.Web.UI.WebControls.TextBox
    Protected WithEvents TxtIdFascVSub As TextBox
    Protected WithEvents cmdSelFascicoloVSub As System.Web.UI.WebControls.Image
    Protected WithEvents cmdSelProtocollo0VSub As System.Web.UI.WebControls.Image
    Protected WithEvents cmdFascCancVSub As System.Web.UI.WebControls.ImageButton
    Protected WithEvents LblDescFascVSub As System.Web.UI.WebControls.Label
    Protected WithEvents txtDescFascVSub As System.Web.UI.WebControls.TextBox
    Protected WithEvents chkAssegnazioneVolontario As System.Web.UI.WebControls.CheckBox
    Protected WithEvents lblart2IIstep As System.Web.UI.WebControls.Label
    Protected WithEvents hplAssegnazioneVolontario As System.Web.UI.WebControls.HyperLink
    Protected WithEvents LblNumProtAV As System.Web.UI.WebControls.Label
    Protected WithEvents LblDataProtAV As System.Web.UI.WebControls.Label
    Protected WithEvents txtDataProtAV As System.Web.UI.WebControls.TextBox
    Protected WithEvents hplAssegnazioneVolontarioB As System.Web.UI.WebControls.HyperLink
    Protected WithEvents LblNumProtAVB As System.Web.UI.WebControls.Label
    Protected WithEvents txtNumProtAVB As System.Web.UI.WebControls.TextBox
    Protected WithEvents ImgProtolloAVB As System.Web.UI.WebControls.Image
    Protected WithEvents ImgApriAllegatiAVB As System.Web.UI.WebControls.Image
    Protected WithEvents ImgProtocollazioneAVB As System.Web.UI.WebControls.Image
    Protected WithEvents LblDataProtAVB As System.Web.UI.WebControls.Label
    Protected WithEvents txtDataProtAVB As System.Web.UI.WebControls.TextBox
    Protected WithEvents imgGeneraFile As System.Web.UI.WebControls.Button
    Protected WithEvents hddCodVolSostituito As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents hddCodVolSubentro As System.Web.UI.HtmlControls.HtmlInputHidden
    Dim PROGETTO_GARANZIA_GIOVANI As String = "4"
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: questa chiamata al metodo è richiesta da Progettazione Web Form.
        'Non modificarla nell'editor del codice.
        InitializeComponent()
    End Sub

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load, Me.Load
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
        If Page.IsPostBack = False Then
            AttivazionePulsanti()
            CaricaMaschera(Request.QueryString("IdAttivita"), Request.QueryString("identita"), Request.QueryString("IdEntitaSubentrante"))
        End If
    End Sub

    Sub AttivazionePulsanti()
        Dim utility As New ClsUtility()
        Dim IdTipologiaProgetto As String = utility.TipologiaProgettoDaIdAttivita(Request.QueryString("IdAttivita"), Session("conn"))


        If (Session("TipoUtente") = "U" Or Session("TipoUtente") = "R") Then


            If Not Request.QueryString("IdEntitaSubentrante") Is Nothing And Request.QueryString("IdEntitaSubentrante") <> "" Then
                chkAssegnazioneVolontario.Enabled = True
                chkRinunciaserviziovolontarioMultipla.Enabled = True
            Else
                chkAssegnazioneVolontario.Enabled = False
                chkRinunciaserviziovolontarioMultipla.Enabled = False
            End If
 
            If (IdTipologiaProgetto = PROGETTO_GARANZIA_GIOVANI) Then
                If Not Request.QueryString("IdEntitaSubentrante") Is Nothing And Request.QueryString("IdEntitaSubentrante") <> "" Then
                    chkAssegnazioneVolontario.Enabled = True
                    chkRinunciaserviziovolontarioMultipla.Enabled = True
                Else
                    chkAssegnazioneVolontario.Enabled = False
                    chkRinunciaserviziovolontarioMultipla.Enabled = False
                End If
                chkLetteraChiusura.Enabled = False 'lettera chiusura iniziale
                chkLetteraEsclusionePerAssenzaIngiustificata.Enabled = True
                chkLetteraEsclusionePerGiorniPermesso.Enabled = True
                chkLetteraEsclusionePerSuperamentoMalattia.Enabled = True
                chkLetteraChiusuraInServizio.Enabled = True
                chkRinunciaserviziovolontario.Enabled = True
                chkElencoVolontariAmmessi.Enabled = True

            Else
                If Not Request.QueryString("IdEntitaSubentrante") Is Nothing And Request.QueryString("IdEntitaSubentrante") <> "" Then
                    chkAssegnazioneVolontario.Enabled = True
                    chkRinunciaserviziovolontarioMultipla.Enabled = True
                Else
                    chkAssegnazioneVolontario.Enabled = False
                    chkRinunciaserviziovolontarioMultipla.Enabled = False
                End If
                chkLetteraChiusura.Enabled = True
                chkLetteraEsclusionePerAssenzaIngiustificata.Enabled = True
                chkLetteraEsclusionePerGiorniPermesso.Enabled = True
                chkLetteraEsclusionePerSuperamentoMalattia.Enabled = True
                chkLetteraChiusuraInServizio.Enabled = True
                chkRinunciaserviziovolontario.Enabled = True
                chkElencoVolontariAmmessi.Enabled = True
            End If

        Else
            chkAssegnazioneVolontario.Enabled = False
            chkElencoVolontariAmmessi.Enabled = False
            chkLetteraChiusura.Enabled = False
            chkLetteraChiusuraInServizio.Enabled = False
            chkLetteraEsclusionePerAssenzaIngiustificata.Enabled = False
            chkLetteraEsclusionePerGiorniPermesso.Enabled = False
            chkLetteraEsclusionePerSuperamentoMalattia.Enabled = False
            chkRinunciaserviziovolontario.Enabled = False
            chkRinunciaserviziovolontarioMultipla.Enabled = False
        End If

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

    Sub CronologiaVolontariGraduatoria(ByVal IdSedeAssegnazione As Integer, ByVal DataProt As String, ByVal NProt As String)
        Dim strsql As String
        Dim dtsGenerico As DataSet

        strsql = "Select DISTINCT entità.identità"
        strsql = strsql & " from entità "
        strsql = strsql & " INNER JOIN attivitàentità ON entità.IDEntità = attivitàentità.IDEntità "
        strsql = strsql & " INNER JOIN attivitàentisediattuazione"
        strsql = strsql & " ON attivitàentità.IDAttivitàEnteSedeAttuazione = attivitàentisediattuazione.IDAttivitàEnteSedeAttuazione "
        strsql = strsql & " inner join statientità on statientità.idstatoentità=entità.idstatoentità "
        strsql = strsql & "left join impVolontariLotus on entità.codicefiscale=impVolontariLotus.cf "
        strsql = strsql & "inner join graduatorieEntità on graduatorieEntità.identità=Entità.identità "
        strsql = strsql & "left join TipologiePosto on graduatorieEntità.idtipologiaposto=TipologiePosto.idtipologiaposto "
        strsql = strsql & "inner join comuni on comuni.idcomune=entità.idcomunenascita "
        strsql = strsql & "inner join provincie on (provincie.idprovincia=comuni.idprovincia) "
        strsql = strsql & "where(graduatorieEntità.idattivitàsedeassegnazione='" & IdSedeAssegnazione & "') and graduatorieEntità.ammesso = 1 and statientità.inservizio=1 and attivitàentità.idstatoattivitàentità=1 "

        dtsGenerico = ClsServer.DataSetGenerico(strsql, Session("conn"))

        'Cronologia documenti volontari in graduatoria
        Dim intX As Integer

        For intX = 0 To dtsGenerico.Tables(0).Rows.Count - 1
            ClsUtility.CronologiaDocEntità(dtsGenerico.Tables(0).Rows(intX).Item("identità"), Session("Utente"), "Graduatoria Volontari", Session("conn"), DataProt, NProt)
        Next

    End Sub
    Sub FascicoloProgetto(ByVal blnValore As Boolean)
        lblCodiceFascicoloP.Visible = blnValore
        LblDescFascP.Visible = blnValore
        TxtCodiceFascicoloP.Visible = blnValore
        txtDescFascP.Visible = blnValore
        cmdSelFascicoloP.Visible = blnValore
        cmdSelProtocollo0P.Visible = blnValore
        cmdFascCancP.Visible = blnValore
        Progetto_RigheNascoste.Visible = blnValore
    End Sub

    Sub FascicoloVolotarioSostituito(ByVal blnValore As Boolean)
        lblCodiceFascicoloVSos.Visible = blnValore
        LblDescFascVSos.Visible = blnValore
        TxtCodiceFascicoloVSos.Visible = blnValore
        txtDescFascVSos.Visible = blnValore
        cmdSelFascicoloVSos.Visible = blnValore
        cmdSelProtocollo0VSos.Visible = blnValore
        cmdFascCancVSos.Visible = blnValore
        Volontario_RigheNascoste.Visible = blnValore
    End Sub
    Sub FascicoloVolotarioSubentrante(ByVal blnValore As Boolean)
        lblCodiceFascicoloVSub.Visible = blnValore
        LblDescFascVSub.Visible = blnValore
        TxtCodiceFascicoloVSub.Visible = blnValore
        txtDescFascVSub.Visible = blnValore
        cmdSelFascicoloVSub.Visible = blnValore
        cmdSelProtocollo0VSub.Visible = blnValore
        cmdFascCancVSub.Visible = blnValore
        Subentrante_RigheNascoste.Visible = blnValore
    End Sub
    Sub ProtocolliElencoVolontariAmmessi(ByVal blnValore As Boolean)
        'LblNumProtEVA.Visible = blnValore
        'txtNumProtEVA.Visible = blnValore
        'LblDataProtEVA.Visible = blnValore
        'txtDataProtEVA.Visible = blnValore
        'ImgProtolloEVA.Visible = blnValore
        'ImgApriAllegatiEVA.Visible = blnValore
        'ImgProtocollazioneEVA.Visible = blnValore
        DivElencoVolontariAmmessi.Visible = blnValore
    End Sub
    Sub ProtocolliRinunuciaServizioVolontario(ByVal blnValore As Boolean)
        'LblNumProtRSV.Visible = blnValore
        'txtNumProtRSV.Visible = blnValore
        'LblDataProtRSV.Visible = blnValore
        'txtDataProtRSV.Visible = blnValore
        'ImgProtolloRSV.Visible = blnValore
        'ImgApriAllegatiRSV.Visible = blnValore
        'ImgProtocollazioneRSV.Visible = blnValore

        'LblNumProtRSVCopiaReg.Visible = blnValore
        'txtNumProtRSVCopiaReg.Visible = blnValore
        'LblDataProtRSVCopiaReg.Visible = blnValore
        'txtDataProtRSVCopiaReg.Visible = blnValore
        'ImgProtolloRSVCopiaReg.Visible = blnValore
        'ImgApriAllegatiRSVCopiaReg.Visible = blnValore
        'ImgProtocollazioneRSVCopiaReg.Visible = blnValore
        DivRinunciaserviziovolontario.Visible = blnValore

    End Sub
    Sub ProtocolliLetteraChiusuraIniziale(ByVal blnValore As Boolean)
        'LblNumProtLCI.Visible = blnValore
        'txtNumProtLCI.Visible = blnValore
        'LblDataProtLCI.Visible = blnValore
        'txtDataProtLCI.Visible = blnValore
        'ImgProtolloLCI.Visible = blnValore
        'ImgApriAllegatiLCI.Visible = blnValore
        'ImgProtocollazioneLCI.Visible = blnValore
        DivLetteraChiusura.Visible = blnValore
    End Sub
    Sub ProtocolliLetteraChiusuraServizio(ByVal blnValore As Boolean)
        'LblNumProtLCS.Visible = blnValore
        'txtNumProtLCS.Visible = blnValore
        'LblDataProtLCS.Visible = blnValore
        'txtDataProtLCS.Visible = blnValore
        'ImgProtolloLCS.Visible = blnValore
        'ImgApriAllegatiLCS.Visible = blnValore
        'ImgProtocollazioneLCS.Visible = blnValore
        DivLetteraChiusuraInServizio.Visible = blnValore
    End Sub
    Sub ProtocolliLetteraEsclusioneAssenzaIngistificata(ByVal blnValore As Boolean)
        'LblNumProtLEAI.Visible = blnValore
        'txtNumProtLEAI.Visible = blnValore
        'LblDataProtLEAI.Visible = blnValore
        'txtDataProtLEAI.Visible = blnValore
        'ImgProtolloLEAI.Visible = blnValore
        'ImgApriAllegatiLEAI.Visible = blnValore
        'ImgProtocollazioneLEAI.Visible = blnValore
        DivLetteraEsclusionePerAssenzaIngiustificata.Visible = blnValore
    End Sub
    Sub ProtocolliLetteraEsclusioneGiorniPermesso(ByVal blnValore As Boolean)
        'LblNumProtLEGP.Visible = blnValore
        'txtNumProtLEGP.Visible = blnValore
        'LblDataProtLEGP.Visible = blnValore
        'txtDataProtLEGP.Visible = blnValore
        'ImgProtolloLEGP.Visible = blnValore
        'ImgApriAllegatiLEGP.Visible = blnValore
        'ImgProtocollazioneLEGP.Visible = blnValore
        DivEsclusionePerGiorniPermesso.Visible = blnValore
    End Sub
    Sub ProtocolliLetteraEsclusioneSuperamentoMalattia(ByVal blnValore As Boolean)
        'LblNumProtLESM.Visible = blnValore
        'txtNumProtLESM.Visible = blnValore
        'LblDataProtLESM.Visible = blnValore
        'txtDataProtLESM.Visible = blnValore
        'ImgProtolloLESM.Visible = blnValore
        'ImgApriAllegatiLESM.Visible = blnValore
        'ImgProtocollazioneLESM.Visible = blnValore
        DivLetteraEsclusionePerSuperamentoMalattia.Visible = blnValore
    End Sub
    Sub ProtocolliLetteraEsclusioneDoppiaDomanda(ByVal blnValore As Boolean)
        'LblNumProtLEDD.Visible = blnValore
        'txtNumProtLEDD.Visible = blnValore
        'LblDataProtLEDD.Visible = blnValore
        'txtDataProtLEDD.Visible = blnValore
        'ImgProtolloLEDD.Visible = blnValore
        'ImgApriAllegatiLEDD.Visible = blnValore
        'ImgProtocollazioneLEDD.Visible = blnValore
        DivLetteraEsclusioneDoppiaDomanda.Visible = blnValore

    End Sub
    Sub ProtocolliRinunuciaServizioVolontarioMultipla(ByVal blnValore As Boolean)
        'LblNumProtRSVM.Visible = blnValore
        'txtNumProtRSVM.Visible = blnValore
        'LblDataProtRSVM.Visible = blnValore
        'txtDataProtRSVM.Visible = blnValore
        'ImgProtolloRSVM.Visible = blnValore
        'ImgApriAllegatiRSVM.Visible = blnValore
        'ImgProtocollazioneRSVM.Visible = blnValore

        'LblNumProtRSVMCopiaReg.Visible = blnValore
        'txtNumProtRSVMCopiaReg.Visible = blnValore
        'LblDataProtRSVMCopiaReg.Visible = blnValore
        'txtDataProtRSVMCopiaReg.Visible = blnValore
        'ImgProtolloRSVMCopiaReg.Visible = blnValore
        'ImgApriAllegatiRSVMCopiaReg.Visible = blnValore
        'ImgProtocollazioneRSVMCopiaReg.Visible = blnValore
        DivRinunciaserviziovolontarioMultipla.Visible = blnValore
    End Sub
    Sub ProtocolliAssegnazioneVolontario(ByVal blnValore As Boolean)
        'LblNumProtAV.Visible = blnValore
        'txtNumProtAV.Visible = blnValore
        'LblDataProtAV.Visible = blnValore
        'txtDataProtAV.Visible = blnValore
        'ImgProtolloAV.Visible = blnValore
        'ImgApriAllegatiAV.Visible = blnValore
        'ImgProtocollazioneAV.Visible = blnValore

        'LblNumProtAVB.Visible = blnValore
        'txtNumProtAVB.Visible = blnValore
        'LblDataProtAVB.Visible = blnValore
        'txtDataProtAVB.Visible = blnValore
        'ImgProtolloAVB.Visible = blnValore
        'ImgApriAllegatiAVB.Visible = blnValore
        'ImgProtocollazioneAVB.Visible = blnValore
        DivAssegnazioneVolontario.Visible = blnValore

    End Sub

    Private Sub CaricaMaschera(ByVal IdAttivita As String, ByVal IdEntitaSostituito As String, ByVal IdEntitàSubentrante As String)

        Dim strsql As String
        Dim dtrgenerico As SqlClient.SqlDataReader

        LblDatiNominativoSostituito.Text = Request.QueryString("NominativoSostituito")
        LblDatiNominativoSubentrante.Text = Request.QueryString("NominativoSubentrante")
        LblDatiProgetto.Text = Request.QueryString("Progetto")

        FascicoloProgetto(False)
        FascicoloVolotarioSostituito(False)
        FascicoloVolotarioSubentrante(False)

        ProtocolliElencoVolontariAmmessi(False)
        ProtocolliRinunuciaServizioVolontario(False)
        ProtocolliLetteraChiusuraIniziale(False)
        ProtocolliLetteraChiusuraServizio(False)
        ProtocolliLetteraEsclusioneAssenzaIngistificata(False)
        ProtocolliLetteraEsclusioneGiorniPermesso(False)
        ProtocolliLetteraEsclusioneSuperamentoMalattia(False)
        ProtocolliLetteraEsclusioneDoppiaDomanda(False)
        ProtocolliRinunuciaServizioVolontarioMultipla(False)
        ProtocolliAssegnazioneVolontario(False)
        '******* PROGETTO **** 
        If IdAttivita <> "" Then
            If Not dtrgenerico Is Nothing Then
                dtrgenerico.Close()
                dtrgenerico = Nothing
            End If

            'estraggo il fascicolo del progetto
            strsql = " SELECT attività.CodiceFascicoloAI, attività.IDFascicoloAI, attività.DescrFascicoloAI " & _
                     " From attività " & _
                     " where attività.idAttività =" & IdAttivita
            dtrgenerico = ClsServer.CreaDatareader(strsql, Session("conn"))
            If dtrgenerico.HasRows = True Then
                dtrgenerico.Read()
                If ("" & dtrgenerico("CodiceFascicoloAI")) <> "" Then
                    FascicoloProgetto(True)

                    TxtCodiceFascicoloP.Text = "" & dtrgenerico("CodiceFascicoloAI")
                    TxtIdFascP.Text = "" & dtrgenerico("IDFascicoloAI")
                    txtDescFascP.Text = "" & dtrgenerico("DescrFascicoloAI")

                    If Not dtrgenerico Is Nothing Then
                        dtrgenerico.Close()
                        dtrgenerico = Nothing
                    End If
                    'estraggo protocollo dei documenti
                    strsql = " SELECT CronologiaEntiDocumenti.NProt, CronologiaEntiDocumenti.DataProt,CronologiaEntiDocumenti.Documento  " & _
                            " From CronologiaEntiDocumenti " & _
                            " where IdEnte ='" & Session("IdEnte") & "' " & _
                            " and IdAttivitàSedeAssegnazione = " & Request.QueryString("IdAttivitaSedeAssegnazione")
                    ' " and CronologiaEntiDocumenti.username = '" & Session("Utente") & "'" & _
                    '" and CronologiaEntiDocumenti.DataProt is not null and CronologiaEntiDocumenti.nprot is not null "
                    dtrgenerico = ClsServer.CreaDatareader(strsql, Session("conn"))
                    Do While dtrgenerico.Read()
                        Select Case dtrgenerico("Documento")
                            Case "elencovolontariammessi"
                                'ProtocolliElencoVolontariAmmessi(True)
                                txtNumProtEVA.Text = "" & dtrgenerico("NProt")
                                txtDataProtEVA.Text = "" & dtrgenerico("DataProt")
                            Case "rinunciaserviziovolontariomultipla"
                                'ProtocolliRinunuciaServizioVolontarioMultipla(True)
                                txtNumProtRSVM.Text = "" & dtrgenerico("NProt")
                                txtDataProtRSVM.Text = "" & dtrgenerico("DataProt")
                            Case "rinunciaserviziovolontariomultiplaCopiaReg"
                                'ProtocolliRinunuciaServizioVolontarioMultipla(True)
                                txtNumProtRSVMCopiaReg.Text = "" & dtrgenerico("NProt")
                                txtDataProtRSVMCopiaReg.Text = "" & dtrgenerico("DataProt")
                        End Select
                    Loop
                End If

            End If
        End If

        If IdEntitaSostituito <> "" Then
            '**** VOLONTARIO SOSTITUITO ****
            If Not dtrgenerico Is Nothing Then
                dtrgenerico.Close()
                dtrgenerico = Nothing
            End If
            'estraggo il fascicolo del volontario sostituito
            strsql = " Select entità.CodiceVolontario,entità.CodiceFascicolo, entità.IDFascicolo, entità.DescrFascicolo " & _
                     " FROM entità   " & _
                     " where entità.identità =" & IdEntitaSostituito
            dtrgenerico = ClsServer.CreaDatareader(strsql, Session("conn"))
            If dtrgenerico.HasRows = True Then
                dtrgenerico.Read()
                hddCodVolSostituito.Value = "" & dtrgenerico("CodiceVolontario")
                If ("" & dtrgenerico("CodiceFascicolo")) <> "" Then
                    FascicoloVolotarioSostituito(True)

                    TxtCodiceFascicoloVSos.Text = "" & dtrgenerico("CodiceFascicolo")
                    TxtIdFascVSos.Text = "" & dtrgenerico("IDFascicolo")
                    txtDescFascVSos.Text = "" & dtrgenerico("DescrFascicolo")
                End If
            End If
            If Not dtrgenerico Is Nothing Then
                dtrgenerico.Close()
                dtrgenerico = Nothing
            End If
            'estraggo protocollo dei documenti
            strsql = " Select  CronologiaEntitàDocumenti.NProt, CronologiaEntitàDocumenti.DataProt,CronologiaEntitàDocumenti.Documento " & _
                     " FROM CronologiaEntitàDocumenti " & _
                     " where CronologiaEntitàDocumenti.identità =" & IdEntitaSostituito & " "
            '" and CronologiaEntitàDocumenti.username = '" & Session("Utente") & "' "
            '" and CronologiaEntitàDocumenti.DataProt is not null and  CronologiaEntitàDocumenti.nprot is not null "
            dtrgenerico = ClsServer.CreaDatareader(strsql, Session("conn"))
            Do While dtrgenerico.Read()
                Select Case dtrgenerico("Documento")
                    Case "Rinuncia Servizio Volontario"
                        '--  ProtocolliRinunuciaServizioVolontario(True)
                        txtNumProtRSV.Text = "" & dtrgenerico("NProt")
                        txtDataProtRSV.Text = "" & dtrgenerico("DataProt")
                    Case "rinunciaserviziovolontarioCopiaReg"
                        '-- ProtocolliRinunuciaServizioVolontario(True)
                        txtNumProtRSVCopiaReg.Text = "" & dtrgenerico("NProt")
                        txtDataProtRSVCopiaReg.Text = "" & dtrgenerico("DataProt")
                    Case "Chiusura Iniziale"
                        'ProtocolliLetteraChiusuraIniziale(True)
                        txtNumProtLCI.Text = "" & dtrgenerico("NProt")
                        txtDataProtLCI.Text = "" & dtrgenerico("DataProt")
                    Case "Chiusura In Servizio"
                        'ProtocolliLetteraChiusuraServizio(True)
                        txtNumProtLCS.Text = "" & dtrgenerico("NProt")
                        txtDataProtLCS.Text = "" & dtrgenerico("DataProt")
                    Case "Lettera Esclusione Per Assenza Ingiustificata"
                        'ProtocolliLetteraEsclusioneAssenzaIngistificata(True)
                        txtNumProtLEAI.Text = "" & dtrgenerico("NProt")
                        txtDataProtLEAI.Text = "" & dtrgenerico("DataProt")
                    Case "Lettera Esclusione Per Giorni Permesso"
                        'ProtocolliLetteraEsclusioneGiorniPermesso(True)
                        txtNumProtLEGP.Text = "" & dtrgenerico("NProt")
                        txtDataProtLEGP.Text = "" & dtrgenerico("DataProt")
                    Case "Lettera Esclusione Per Superamento Malattia"
                        'ProtocolliLetteraEsclusioneSuperamentoMalattia(True)
                        txtNumProtLESM.Text = "" & dtrgenerico("NProt")
                        txtDataProtLESM.Text = "" & dtrgenerico("DataProt")
                    Case "Lettera Esclusione Volontario"
                        'ProtocolliLetteraEsclusioneDoppiaDomanda(True)
                        txtNumProtLEDD.Text = "" & dtrgenerico("NProt")
                        txtDataProtLEDD.Text = "" & dtrgenerico("DataProt")

                End Select
            Loop
        End If
        If IdEntitàSubentrante <> "" Then
            '***** VOLONTARIO SUBENTRATO *****
            If Not dtrgenerico Is Nothing Then
                dtrgenerico.Close()
                dtrgenerico = Nothing
            End If
            'estraggo il fascicolo del volontario subentrante
            strsql = " Select entità.CodiceVolontario,entità.CodiceFascicolo, entità.IDFascicolo, entità.DescrFascicolo " & _
                     " FROM entità   " & _
                     " where entità.identità =" & IdEntitàSubentrante
            dtrgenerico = ClsServer.CreaDatareader(strsql, Session("conn"))
            If dtrgenerico.HasRows = True Then
                dtrgenerico.Read()
                hddCodVolSubentro.Value = "" & dtrgenerico("CodiceVolontario")
                If ("" & dtrgenerico("CodiceFascicolo")) <> "" Then
                    FascicoloVolotarioSubentrante(True)
                    TxtCodiceFascicoloVSub.Text = "" & dtrgenerico("CodiceFascicolo")
                    TxtIdFascVSub.Text = "" & dtrgenerico("IDFascicolo")
                    txtDescFascVSub.Text = "" & dtrgenerico("DescrFascicolo")
                End If
            End If
            If Not dtrgenerico Is Nothing Then
                dtrgenerico.Close()
                dtrgenerico = Nothing
            End If
            'estraggo protocollo dei documenti
            strsql = " Select  CronologiaEntitàDocumenti.NProt, CronologiaEntitàDocumenti.DataProt,CronologiaEntitàDocumenti.Documento " & _
                     " FROM CronologiaEntitàDocumenti " & _
                     " where CronologiaEntitàDocumenti.identità =" & IdEntitàSubentrante & " " & _
                     " and CronologiaEntitàDocumenti.username = '" & Session("Utente") & "' "
            '" and CronologiaEntitàDocumenti.DataProt is not null and  CronologiaEntitàDocumenti.nprot is not null "
            dtrgenerico = ClsServer.CreaDatareader(strsql, Session("conn"))
            Do While dtrgenerico.Read()
                Select Case dtrgenerico("Documento")
                    Case "Assegnazione Volontario - Nazionale"
                        'ProtocolliAssegnazioneVolontario(True)
                        txtNumProtAV.Text = "" & dtrgenerico("NProt")
                        txtDataProtAV.Text = "" & dtrgenerico("DataProt")
                    Case "AssegnazioneVolontariNazionaliB"
                        'ProtocolliAssegnazioneVolontario(True)
                        txtNumProtAVB.Text = "" & dtrgenerico("NProt")
                        txtDataProtAVB.Text = "" & dtrgenerico("DataProt")
                    Case "Sostituzione Volontario - Nazionale"
                        ProtocolliAssegnazioneVolontario(True)
                        txtNumProtAV.Text = "" & dtrgenerico("NProt")
                        txtDataProtAV.Text = "" & dtrgenerico("DataProt")
                    Case "SostituzioneVolontariNazionaliB"
                        'ProtocolliAssegnazioneVolontario(True)
                        txtNumProtAVB.Text = "" & dtrgenerico("NProt")
                        txtDataProtAVB.Text = "" & dtrgenerico("DataProt")
                    Case "Assegnazione Volontario - Estero"
                        'ProtocolliAssegnazioneVolontario(True)
                        txtNumProtAV.Text = "" & dtrgenerico("NProt")
                        txtDataProtAV.Text = "" & dtrgenerico("DataProt")
                    Case "AssegnazioneVolontariEsteroB"
                        ProtocolliAssegnazioneVolontario(True)
                        txtNumProtAVB.Text = "" & dtrgenerico("NProt")
                        txtDataProtAVB.Text = "" & dtrgenerico("DataProt")
                    Case "Sostituzione Volontario - Estero"
                        'ProtocolliAssegnazioneVolontario(True)
                        txtNumProtAV.Text = "" & dtrgenerico("NProt")
                        txtDataProtAV.Text = "" & dtrgenerico("DataProt")
                    Case "SostituzioneVolontariEsteriB"
                        'ProtocolliAssegnazioneVolontario(True)
                        txtNumProtAVB.Text = "" & dtrgenerico("NProt")
                        txtDataProtAVB.Text = "" & dtrgenerico("DataProt")
                End Select
            Loop
        End If
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
    End Sub

    Sub CancellaFascicoloProtocolloProgetto(ByVal IDAttività As Integer, ByVal IDEnte As Integer)
        Dim strSql As String

        'protocollo
        strSql = "Update CronologiaEntiDocumenti set DataProt =   null , NProt = null "
        strSql = strSql & " where IdEnte = " & Session("IdEnte")
        strSql = strSql & " and documento IN "
        strSql = strSql & " ('elencovolontariammessi',"
        strSql = strSql & " 'rinunciaserviziovolontariomultipla',"
        strSql = strSql & " 'rinunciaserviziovolontariomultiplaCopiaReg')"
        Dim cmdUpCron As New SqlClient.SqlCommand(strSql, Session("conn"))
        cmdUpCron.ExecuteNonQuery()

        'fascicolo
        strSql = "Update Attività set "
        strSql = strSql & " CodiceFascicoloAI =null, "
        strSql = strSql & " IDFascicoloAI=null, "
        strSql = strSql & " DescrFascicoloAI =null "
        strSql = strSql & " where IDAttività = " & IDAttività
        Dim cmdUpE As New SqlClient.SqlCommand(strSql, Session("conn"))
        cmdUpE.ExecuteNonQuery()

    End Sub
    Sub CancellaFascicoloProtocolloVolontario(ByVal IdVolontario As Integer, ByVal VengoDa As Integer)
        ' VengoDa: identifica chi ha richiamato la sub
        ' 1 : Volontario Sostituito
        ' 2 : Volontario Subentrante

        Dim strSql As String
        Dim Documento As String

        'protocollo
        strSql = "Update CronologiaEntitàDocumenti set DataProt = null ,NProt = null "
        strSql = strSql & "where identità = " & IdVolontario & ""
        strSql = strSql & " and documento IN "
        strSql = strSql & " ("
        If VengoDa = 1 Then
            strSql = strSql & " 'Rinuncia Servizio Volontario',"
            strSql = strSql & " 'rinunciaserviziovolontarioCopiaReg',"
            strSql = strSql & " 'Chiusura Iniziale',"
            strSql = strSql & " 'Chiusura In Servizio',"
            strSql = strSql & " 'Lettera Esclusione Per Assenza Ingiustificata',"
            strSql = strSql & " 'Lettera Esclusione Per Giorni Permesso',"
            strSql = strSql & " 'Lettera Esclusione Per Superamento Malattia',"
            strSql = strSql & " 'Lettera Esclusione Volontario'"
        Else
            strSql = strSql & " 'Assegnazione Volontario - Nazionale',"
            strSql = strSql & " 'Sostituzione Volontario - Nazionale',"
            strSql = strSql & " 'Assegnazione Volontario - Estero',"
            strSql = strSql & " 'Sostituzione Volontario - Estero',"
            strSql = strSql & " 'AssegnazioneVolontariNazionaliB',"
            strSql = strSql & " 'SostituzioneVolontariNazionaliB',"
            strSql = strSql & " 'AssegnazioneVolontariEsteroB',"
            strSql = strSql & " 'SostituzioneVolontariEsteriB'"
        End If
        strSql = strSql & " )"
        Dim cmdUpCron As New SqlClient.SqlCommand(strSql, Session("conn"))
        cmdUpCron.ExecuteNonQuery()

        'fascicolo
        strSql = "Update Entità set "
        strSql = strSql & " CodiceFascicolo = null,"
        strSql = strSql & " IDFascicolo= null ,"
        strSql = strSql & "DescrFascicolo = null "
        strSql = strSql & " where identità = " & IdVolontario
        Dim cmdUpE As New SqlClient.SqlCommand(strSql, Session("conn"))
        cmdUpE.ExecuteNonQuery()
    End Sub

    'Sub CaricaDocumentiDaStampare(ByVal NomeDocumento As String, ByVal CopiaNomeDocumento As String)
    '    'dataTable che ricorda il nome dei documenti chedevono essere stampari nel caso del volontario subentrante
    '    'per la nuova assegnazione dela volontario

    '    'nome e posizione di lettura delle colopnne a base 0
    '    'Dim NomeColonne(2) As String
    '    Dim NomiCampiColonne(2) As String
    '    'nome della colonna 
    '    'e posizione nella griglia di lettura


    '    NomiCampiColonne(0) = NomeDocumento
    '    NomiCampiColonne(1) = CopiaNomeDocumento


    '    ' Session("DtbRicerca") = Session("dtSanzione")

    '    'carico un datatable che userò poi nella pagina di stampa
    '    'il numero delle colonne è a base 0
    '    CaricaDataTable(Session("dtSanzione"), 2, NomiCampiColonne)

    'End Sub

    'Function CaricaDataTable(ByVal DataTableDaScorrere As DataTable, ByVal NColonne As Integer, ByVal NomiCampiColonne() As String) As DataTable
    '    Dim dt As New DataTable
    '    Dim dr As DataRow
    '    Dim i As Integer
    '    Dim x As Integer

    '    'carico il datatable con il risultato della query della ricerca, in qusto caso delle risorse
    '    If DataTableDaScorrere.Rows.Count > 0 Then
    '        For i = 1 To DataTableDaScorrere.Rows.Count
    '            dr = dt.NewRow()
    '            For x = 0 To NColonne
    '                dr(x) = DataTableDaScorrere.Rows.Item(i - 1).Item(NomiCampiColonne(x))
    '            Next
    '            dt.Rows.Add(dr)
    '        Next
    '    End If
    '    'passo alla sessione la datatable che ho appena creato e che userò per il databinding della datagrid della stampa
    '    Session("DtbNomeDocumenti") = dt
    'End Function


    Private Sub cmdFascCancP_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles cmdFascCancP.Click
        If TxtCodiceFascicoloP.Text <> "" Then
            'CANCELLO FASCICOLO
            TxtCodiceFascicoloP.Text = ""
            TxtIdFascP.Text = ""
            txtDescFascP.Text = ""
            'CANCELLO PROTOCOLLO
            txtNumProtEVA.Text = ""
            txtDataProtEVA.Text = ""
            txtNumProtRSVM.Text = ""
            txtDataProtRSVM.Text = ""
            txtNumProtRSVMCopiaReg.Text = ""
            txtDataProtRSVMCopiaReg.Text = ""

            CancellaFascicoloProtocolloProgetto(Request.QueryString("IdAttivita"), Session("IdEnte"))
        End If
    End Sub

    Private Sub cmdFascCancVSos_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles cmdFascCancVSos.Click
        If TxtCodiceFascicoloVSos.Text <> "" Then
            'CANCELLO FASCICOLO
            TxtCodiceFascicoloVSos.Text = ""
            TxtIdFascVSos.Text = ""
            txtDescFascVSos.Text = ""
            'CANCELLO PROTOCOLLO
            txtNumProtRSV.Text = ""
            txtDataProtRSV.Text = ""
            txtNumProtRSVCopiaReg.Text = ""
            txtDataProtRSVCopiaReg.Text = ""
            txtNumProtLCI.Text = ""
            txtDataProtLCI.Text = ""
            txtNumProtLCS.Text = ""
            txtDataProtLCS.Text = ""
            txtNumProtLEAI.Text = ""
            txtDataProtLEAI.Text = ""
            txtNumProtLEGP.Text = ""
            txtDataProtLEGP.Text = ""
            txtNumProtLESM.Text = ""
            txtDataProtLESM.Text = ""
            txtNumProtLEDD.Text = ""
            txtDataProtLEDD.Text = ""

            CancellaFascicoloProtocolloVolontario(Request.QueryString("identita"), 1)
        End If
    End Sub

    Private Sub cmdFascCancVSub_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles cmdFascCancVSub.Click
        If TxtCodiceFascicoloVSub.Text <> "" Then
            'CANCELLO FASCICOLO
            TxtCodiceFascicoloVSub.Text = ""
            TxtIdFascVSub.Text = ""
            txtDescFascVSub.Text = ""
            'CANCELLO PROTOCOLLO

            txtNumProtAV.Text = ""
            txtDataProtAV.Text = ""
            txtNumProtAVB.Text = ""
            txtDataProtAVB.Text = ""

            CancellaFascicoloProtocolloVolontario(Request.QueryString("IdEntitaSubentrante"), 2)
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

    Private Sub imgGeneraFile_Click(ByVal sender As Object, ByVal e As EventArgs) Handles imgGeneraFile.Click
        Dim strsql As String
        Dim dtrgenerico As SqlClient.SqlDataReader
        Dim utility As ClsUtility = New ClsUtility()
        Dim idTipoProgetto As String = utility.TipologiaProgettoDaIdAttivita(Request.QueryString("IdAttivita"), Session("conn"))
        Dim strTipoModello As String = utility.TipologiaModelloDaIdEntita(Request.QueryString("IdEntita"), Session("conn"))
        Dim strNomeDocumento As String
        '************************************************************************************************************************************
        '****************************************************Rinuncia Servizio Volontario****************************************************
        '************************************************************************************************************************************
        'modificata aprile 2022 per suddivisione 4 modelli (ORD, GG, EST, SCD)
        If chkRinunciaserviziovolontario.Checked = True Then
            imgGeneraFile.Visible = False

            Dim Documento As New GeneratoreModelli

            Select Case strTipoModello
                Case "ORD"
                    hplRinunciaserviziovolontario.Visible = True
                    hplRinunciaserviziovolontario.NavigateUrl = Documento.VOL_rinunciaserviziovolontario(Session("IdEnte"), Request.QueryString("IdVolVecchio"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                    ClsUtility.CronologiaDocEntità(Request.QueryString("identita"), Session("Utente"), "Rinuncia Servizio Volontario", Session("conn"), txtDataProtRSV.Text, txtNumProtRSV.Text)
                    hplRinunciaserviziovolontario.Target = "_blank"
                Case "EST"
                    hplRinunciaserviziovolontario.Visible = True
                    hplRinunciaserviziovolontario.NavigateUrl = Documento.VOL_rinunciaserviziovolontarioEstero(Session("IdEnte"), Request.QueryString("IdVolVecchio"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                    ClsUtility.CronologiaDocEntità(Request.QueryString("identita"), Session("Utente"), "Rinuncia Servizio Volontario", Session("conn"), txtDataProtRSV.Text, txtNumProtRSV.Text)
                    hplRinunciaserviziovolontario.Target = "_blank"
                Case "GG"
                    hplRinunciaserviziovolontario.Visible = True
                    hplRinunciaserviziovolontario.NavigateUrl = Documento.VOL_RinunciaServizioVolontarioGaranziaGiovani(Session("IdEnte"), Request.QueryString("IdVolVecchio"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                    ClsUtility.CronologiaDocEntità(Request.QueryString("identita"), Session("Utente"), "Rinuncia Servizio Volontario Garanzia Giovani", Session("conn"), txtDataProtRSV.Text, txtNumProtRSV.Text)
                    hplRinunciaserviziovolontario.Target = "_blank"
                Case "SCD"
                    hplRinunciaserviziovolontario.Visible = True
                    hplRinunciaserviziovolontario.NavigateUrl = Documento.VOL_rinunciaserviziovolontarioSCD(Session("IdEnte"), Request.QueryString("IdVolVecchio"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                    ClsUtility.CronologiaDocEntità(Request.QueryString("identita"), Session("Utente"), "Rinuncia Servizio Volontario", Session("conn"), txtDataProtRSV.Text, txtNumProtRSV.Text)
                    hplRinunciaserviziovolontario.Target = "_blank"
            End Select
            FascicoloVolotarioSostituito(True)
            ProtocolliRinunuciaServizioVolontario(True)
            Documento.Dispose()
        End If

        '************************************************************************************************************************************
        '****************************************************Rinuncia Servizio Volontario MULTIPLA*******************************************
        '************************************************************************************************************************************
        'modificata aprile 2022 per suddivisione 4 modelli (ORD, GG, EST, SCD)
        If chkRinunciaserviziovolontarioMultipla.Checked = True Then
            imgGeneraFile.Visible = False
            Dim Documento As New GeneratoreModelli

            Select Case strTipoModello
                Case "ORD"
                    hplRinunciaserviziovolontarioMultipla.Visible = True
                    'hplRinunciaserviziovolontarioMultiplaCopiaReg.Visible = True
                    hplRinunciaserviziovolontarioMultipla.NavigateUrl = Documento.VOL_rinunciaserviziovolontariomultipla(Session("IdEnte"), Request.QueryString("IdEntitaSubentrante"), ClsUtility.TrovaIDBando(Request.QueryString("IdAttivita"), Session("conn")), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                    NuovaCronologia("rinunciaserviziovolontariomultipla", Request.QueryString("IdAttivitaSedeAssegnazione"), txtDataProtRSVM.Text, txtNumProtRSVM.Text)
                    hplRinunciaserviziovolontarioMultipla.Target = "_blank"
                    'hplRinunciaserviziovolontarioMultiplaCopiaReg.Target = "_blank"
                Case "EST"
                    hplRinunciaserviziovolontarioMultipla.Visible = True
                    hplRinunciaserviziovolontarioMultipla.NavigateUrl = Documento.VOL_rinunciaserviziovolontariomultiplaEstero(Session("IdEnte"), Request.QueryString("IdEntitaSubentrante"), ClsUtility.TrovaIDBando(Request.QueryString("IdAttivita"), Session("conn")), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                    NuovaCronologia("rinunciaserviziovolontariomultipla", Request.QueryString("IdAttivitaSedeAssegnazione"), txtDataProtRSVM.Text, txtNumProtRSVM.Text)
                    hplRinunciaserviziovolontarioMultipla.Target = "_blank"
                Case "GG"
                    hplRinunciaserviziovolontarioMultipla.Visible = True
                    hplRinunciaserviziovolontarioMultipla.NavigateUrl = Documento.VOL_RinunciaServizioVolontarioMultiplaGaranziaGiovani(Session("IdEnte"), Request.QueryString("IdEntitaSubentrante"), ClsUtility.TrovaIDBando(Request.QueryString("IdAttivita"), Session("conn")), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                    NuovaCronologia("RinunciaServizioVolontarioMultiplaGaranziaGiovani", Request.QueryString("IdAttivitaSedeAssegnazione"), txtDataProtRSVM.Text, txtNumProtRSVM.Text)
                    hplRinunciaserviziovolontarioMultipla.Target = "_blank"
                Case "SCD"
                    hplRinunciaserviziovolontarioMultipla.Visible = True
                    hplRinunciaserviziovolontarioMultipla.NavigateUrl = Documento.VOL_rinunciaserviziovolontariomultiplaSCD(Session("IdEnte"), Request.QueryString("IdEntitaSubentrante"), ClsUtility.TrovaIDBando(Request.QueryString("IdAttivita"), Session("conn")), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                    NuovaCronologia("rinunciaserviziovolontariomultipla", Request.QueryString("IdAttivitaSedeAssegnazione"), txtDataProtRSVM.Text, txtNumProtRSVM.Text)
                    hplRinunciaserviziovolontarioMultipla.Target = "_blank"
            End Select
            Documento.Dispose()
            FascicoloProgetto(True)
            ProtocolliRinunuciaServizioVolontarioMultipla(True)
        End If

            '************************************************************************************************************************************
            '****************************************************Elenco Volontari Ammessi********************************************************
            '************************************************************************************************************************************
        If chkElencoVolontariAmmessi.Checked = True Then
            imgGeneraFile.Visible = False
            hplElencoVolontariAmmessi.Visible = True
            hplElencoVolontariAmmessi.Target = "_blank"
            Dim Documento As New GeneratoreModelli
            If (idTipoProgetto = PROGETTO_GARANZIA_GIOVANI) Then
                hplElencoVolontariAmmessi.NavigateUrl = Documento.VOL_ElencoVolontariAmmessiGaranziaGiovani(Request.QueryString("IdAttivitaSedeAssegnazione"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                CronologiaVolontariGraduatoria(Request.QueryString("IdAttivitaSedeAssegnazione"), txtDataProtEVA.Text, txtNumProtEVA.Text)
                NuovaCronologia("ElencoVolontariAmmessiGaranziaGiovani", Request.QueryString("IdAttivitaSedeAssegnazione"), txtDataProtEVA.Text, txtNumProtEVA.Text)
            Else
                hplElencoVolontariAmmessi.NavigateUrl = Documento.VOL_elencovolontariammessi(Request.QueryString("IdAttivitaSedeAssegnazione"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                CronologiaVolontariGraduatoria(Request.QueryString("IdAttivitaSedeAssegnazione"), txtDataProtEVA.Text, txtNumProtEVA.Text)
                NuovaCronologia("elencovolontariammessi", Request.QueryString("IdAttivitaSedeAssegnazione"), txtDataProtEVA.Text, txtNumProtEVA.Text)
            End If
            Documento.Dispose()
            FascicoloProgetto(True)
            ProtocolliElencoVolontariAmmessi(True)
        End If



        '************************************************************************************************************************************
        '****************************************************Assegnazione Volontario*********************************************************
        '************************************************************************************************************************************
        'modificata aprile 2022 per suddivisione 4 modelli (ORD, GG, EST, SCD)
        If chkAssegnazioneVolontario.Checked = True Then


            imgGeneraFile.Visible = False

            hplAssegnazioneVolontario.Target = "_blank"
            hplAssegnazioneVolontarioB.Target = "_blank"

            hplAssegnazioneVolontario.Visible = True
            hplAssegnazioneVolontarioB.Visible = True
            'agg. sc il 30/12/2008 Abilito text del fascicolo e dei protocolli(SIGED)
            FascicoloVolotarioSubentrante(True)
            ProtocolliAssegnazioneVolontario(True)
            Dim strFlag As String = ""
            Dim strGruppo As String = ""
            strsql = "select a.idtipoprogetto as naz, " & _
                     " isnull(identitàSubentrante,0) as  subentro, " & _
                     " e.datainizioservizio,isnull(asa.datainiziodifferita, a.datainizioattività) as datainizioattività, bando.gruppo,case when datainizioservizio < '15/01/2019' then '<' else '>' end as Flag  " & _
                     " from Attività a  " & _
                     " inner join attivitàsediassegnazione asa on asa.idattività=a.idattività  " & _
                     " inner join graduatorieentità ge on ge.idattivitàsedeassegnazione=asa.idattivitàsedeassegnazione  " & _
                     " inner join entità e on e.idEntità=ge.idEntità  " & _
                     " inner join bandiattività ba on a.idbandoattività=ba.idbandoattività " & _
                     " inner join bando on ba.idbando=bando.idbando " & _
                     " left join cronologiasostituzioni cs on cs.identitàsubentrante=ge.idEntità " & _
                     " where ge.idEntità=" & Request.QueryString("IdEntitaSubentrante") & ""
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
                'If dtrgenerico("naz") = "N" Then
                '    hplDownload3.NavigateUrl = LetteraAssegnazioneVOlontario(Session("IdEnte"), "SostituzioneVolontariNazionali")
                'Else
                '    hplDownload3.NavigateUrl = LetteraAssegnazioneVOlontario(Session("IdEnte"), "SostituzioneVolontariEsteri")
                'End If
                If dtrgenerico("naz") = 1 Or dtrgenerico("naz") = 3 Or dtrgenerico("naz") = 5 Or dtrgenerico("naz") = 7 Or dtrgenerico("naz") = 9 Or dtrgenerico("naz") = 11 Then
                    If dtrgenerico("datainizioservizio") = dtrgenerico("datainizioattività") Then
                        If Not dtrgenerico Is Nothing Then
                            dtrgenerico.Close()
                            dtrgenerico = Nothing
                        End If

                        Dim Documento As New GeneratoreModelli
                        'chiamo la proprietà della classe GeneratoreModelli che ritorna la path del documento creato
                        hplAssegnazioneVolontario.NavigateUrl = Documento.VOL_AssegnazioneVolontariNazionali(Request.QueryString("IdEntitaSubentrante"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                        If strGruppo = "50" And strFlag = "<" Then
                            hplAssegnazioneVolontarioB.NavigateUrl = Documento.VOL_AssegnazioneVolontariNazionaliB_Integrativo(Request.QueryString("IdVol"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                        Else
                            Select Case strTipoModello
                                Case "ORD"
                                    hplAssegnazioneVolontarioB.NavigateUrl = Documento.VOL_AssegnazioneVolontariNazionaliB(Request.QueryString("IdVol"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                                Case "EST"
                                    hplAssegnazioneVolontarioB.NavigateUrl = Documento.VOL_AssegnazioneVolontariEsteroB(Request.QueryString("IdVol"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                                Case "GG"
                                    hplAssegnazioneVolontarioB.NavigateUrl = Documento.VOL_AssegnazioneVolontariGaranziaGiovaniB(Request.QueryString("IdVol"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                                Case "SCD"
                                    hplAssegnazioneVolontarioB.NavigateUrl = Documento.VOL_AssegnazioneVolontariNazionaliBSCD(Request.QueryString("IdVol"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                            End Select
                            'hplAssegnazioneVolontarioB.NavigateUrl = Documento.VOL_AssegnazioneVolontariNazionaliB(Request.QueryString("IdVol"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                        End If
                        'hplAssegnazioneVolontarioB.NavigateUrl = Documento.VOL_AssegnazioneVolontariNazionaliB(Request.QueryString("IdEntitaSubentrante"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                        Documento.Dispose()

                        'hplDownload3.NavigateUrl = LetteraAssegnazioneVOlontario(Session("IdEnte"), "AssegnazioneVolontariNazionali")
                        'hplDownloadB.NavigateUrl = LetteraAssegnazioneVOlontario(Session("IdEnte"), "AssegnazioneVolontariNazionaliB")

                        'Cronologia creazione documento.
                        ClsUtility.CronologiaDocEntità(Request.QueryString("IdEntitaSubentrante"), Session("Utente"), "Assegnazione Volontario - Nazionale", Session("conn"), txtDataProtAV.Text, txtNumProtAV.Text)
                        ClsUtility.CronologiaDocEntità(Request.QueryString("IdEntitaSubentrante"), Session("Utente"), "AssegnazioneVolontariNazionaliB", Session("conn"), txtDataProtAVB.Text, txtNumProtAVB.Text)
                        'Session("NomeDocStampaAssVolontario") = "Assegnazione Volontario - Nazionale"
                        'Session("NomeDocStampaAssVolontarioB") = "AssegnazioneVolontariNazionaliB"
                    Else
                        If Not dtrgenerico Is Nothing Then
                            dtrgenerico.Close()
                            dtrgenerico = Nothing
                        End If

                        Dim Documento As New GeneratoreModelli
                        'chiamo la proprietà della classe GeneratoreModelli che ritorna la path del documento creato
                        hplAssegnazioneVolontario.NavigateUrl = Documento.VOL_SostituzioneVolontariNazionali(Request.QueryString("IdEntitaSubentrante"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                        If strGruppo = "50" And strFlag = "<" Then
                            hplAssegnazioneVolontarioB.NavigateUrl = Documento.VOL_SostituzioneVolontariNazionaliB_Integrativo(Request.QueryString("IdVol"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                        Else
                            Select Case strTipoModello
                                Case "ORD"
                                    hplAssegnazioneVolontarioB.NavigateUrl = Documento.VOL_SostituzioneVolontariNazionaliB(Request.QueryString("IdVol"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                                Case "EST"
                                    hplAssegnazioneVolontarioB.NavigateUrl = Documento.VOL_SostituzioneVolontariEsteriB(Request.QueryString("IdVol"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                                Case "GG"
                                    hplAssegnazioneVolontarioB.NavigateUrl = Documento.VOL_SostituzioneVolontariGaranziaGiovaniB(Request.QueryString("IdVol"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                                Case "SCD"
                                    hplAssegnazioneVolontarioB.NavigateUrl = Documento.VOL_SostituzioneVolontariNazionaliBSCD(Request.QueryString("IdVol"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                            End Select
                            'hplAssegnazioneVolontarioB.NavigateUrl = Documento.VOL_SostituzioneVolontariNazionaliB(Request.QueryString("IdVol"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                        End If
                        'hplAssegnazioneVolontarioB.NavigateUrl = Documento.VOL_SostituzioneVolontariNazionaliB(Request.QueryString("IdEntitaSubentrante"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                        Documento.Dispose()

                        'hplDownload3.NavigateUrl = LetteraAssegnazioneVOlontario(Session("IdEnte"), "SostituzioneVolontariNazionali")
                        'hplDownloadB.NavigateUrl = LetteraAssegnazioneVOlontario(Session("IdEnte"), "SostituzioneVolontariNazionaliB")

                        'Cronologia creazione documento.
                        ClsUtility.CronologiaDocEntità(Request.QueryString("IdEntitaSubentrante"), Session("Utente"), "Sostituzione Volontario - Nazionale", Session("conn"), txtDataProtAV.Text, txtNumProtAV.Text)
                        ClsUtility.CronologiaDocEntità(Request.QueryString("IdEntitaSubentrante"), Session("Utente"), "SostituzioneVolontariNazionaliB", Session("conn"), txtDataProtAVB.Text, txtNumProtAVB.Text)

                        ' Session("NomeDocStampaAssVolontario") = "Sostituzione Volontario - Nazionale"
                        ' Session("NomeDocStampaAssVolontarioB") = "SostituzioneVolontariNazionaliB"
                    End If
                ElseIf dtrgenerico("naz") = 2 Or dtrgenerico("naz") = 6 Or dtrgenerico("naz") = 8 Or dtrgenerico("naz") = 10 Or dtrgenerico("naz") = 12 Then
                    If dtrgenerico("datainizioservizio") = dtrgenerico("datainizioattività") Then
                        If Not dtrgenerico Is Nothing Then
                            dtrgenerico.Close()
                            dtrgenerico = Nothing
                        End If

                        Dim Documento As New GeneratoreModelli
                        'chiamo la proprietà della classe GeneratoreModelli che ritorna la path del documento creato
                        hplAssegnazioneVolontario.NavigateUrl = Documento.VOL_AssegnazioneVolontariEstero(Request.QueryString("IdEntitaSubentrante"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                        If strGruppo = "50" And strFlag = "<" Then
                            hplAssegnazioneVolontarioB.NavigateUrl = Documento.VOL_AssegnazioneVolontariEsteroB_Integrativo(Request.QueryString("IdVol"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                        Else
                            Select Case strTipoModello
                                Case "ORD"
                                    hplAssegnazioneVolontarioB.NavigateUrl = Documento.VOL_AssegnazioneVolontariNazionaliB(Request.QueryString("IdVol"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                                Case "EST"
                                    hplAssegnazioneVolontarioB.NavigateUrl = Documento.VOL_AssegnazioneVolontariEsteroB(Request.QueryString("IdVol"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                                Case "GG"
                                    hplAssegnazioneVolontarioB.NavigateUrl = Documento.VOL_AssegnazioneVolontariGaranziaGiovaniB(Request.QueryString("IdVol"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                                Case "SCD"
                                    hplAssegnazioneVolontarioB.NavigateUrl = Documento.VOL_AssegnazioneVolontariNazionaliBSCD(Request.QueryString("IdVol"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                            End Select
                            'hplAssegnazioneVolontarioB.NavigateUrl = Documento.VOL_AssegnazioneVolontariEsteroB(Request.QueryString("IdVol"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                        End If
                        'hplAssegnazioneVolontarioB.NavigateUrl = Documento.VOL_AssegnazioneVolontariEsteroB(Request.QueryString("IdEntitaSubentrante"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                        Documento.Dispose()

                        'hplDownload3.NavigateUrl = LetteraAssegnazioneVOlontario(Session("IdEnte"), "AssegnazioneVolontariEstero")
                        'hplDownloadB.NavigateUrl = LetteraAssegnazioneVOlontario(Session("IdEnte"), "AssegnazioneVolontariEsteroB")

                        'Cronologia creazione documento.
                        ClsUtility.CronologiaDocEntità(Request.QueryString("IdEntitaSubentrante"), Session("Utente"), "Assegnazione Volontario - Estero", Session("conn"), txtDataProtAV.Text, txtNumProtAV.Text)
                        ClsUtility.CronologiaDocEntità(Request.QueryString("IdEntitaSubentrante"), Session("Utente"), "AssegnazioneVolontariEsteroB", Session("conn"), txtDataProtAVB.Text, txtNumProtAVB.Text)
                        'Session("NomeDocStampaAssVolontario") = "Assegnazione Volontario - Estero"
                        'Session("NomeDocStampaAssVolontarioB") = "AssegnazioneVolontariEsteroB"
                    Else
                        If Not dtrgenerico Is Nothing Then
                            dtrgenerico.Close()
                            dtrgenerico = Nothing
                        End If

                        Dim Documento As New GeneratoreModelli
                        'chiamo la proprietà della classe GeneratoreModelli che ritorna la path del documento creato
                        hplAssegnazioneVolontario.NavigateUrl = Documento.VOL_SostituzioneVolontariEsteri(Request.QueryString("IdEntitaSubentrante"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                        If strGruppo = "50" And strFlag = "<" Then
                            hplAssegnazioneVolontarioB.NavigateUrl = Documento.VOL_SostituzioneVolontariEsteriB_Integrativo(Request.QueryString("IdVol"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                        Else
                            Select Case strTipoModello
                                Case "ORD"
                                    hplAssegnazioneVolontarioB.NavigateUrl = Documento.VOL_SostituzioneVolontariNazionaliB(Request.QueryString("IdVol"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                                Case "EST"
                                    hplAssegnazioneVolontarioB.NavigateUrl = Documento.VOL_SostituzioneVolontariEsteriB(Request.QueryString("IdVol"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                                Case "GG"
                                    hplAssegnazioneVolontarioB.NavigateUrl = Documento.VOL_SostituzioneVolontariGaranziaGiovaniB(Request.QueryString("IdVol"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                                Case "SCD"
                                    hplAssegnazioneVolontarioB.NavigateUrl = Documento.VOL_SostituzioneVolontariNazionaliBSCD(Request.QueryString("IdVol"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                            End Select
                            'hplAssegnazioneVolontarioB.NavigateUrl = Documento.VOL_SostituzioneVolontariEsteriB(Request.QueryString("IdVol"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                        End If
                        'hplAssegnazioneVolontarioB.NavigateUrl = Documento.VOL_SostituzioneVolontariEsteriB(Request.QueryString("IdEntitaSubentrante"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                        Documento.Dispose()

                        'hplDownload3.NavigateUrl = LetteraAssegnazioneVOlontario(Session("IdEnte"), "SostituzioneVolontariEsteri")
                        'hplDownloadB.NavigateUrl = LetteraAssegnazioneVOlontario(Session("IdEnte"), "SostituzioneVolontariEsteriB")

                        'Cronologia creazione documento.
                        ClsUtility.CronologiaDocEntità(Request.QueryString("IdEntitaSubentrante"), Session("Utente"), "Sostituzione Volontario - Estero", Session("conn"), txtDataProtAV.Text, txtNumProtAV.Text)
                        ClsUtility.CronologiaDocEntità(Request.QueryString("IdEntitaSubentrante"), Session("Utente"), "SostituzioneVolontariEsteriB", Session("conn"), txtDataProtAVB.Text, txtNumProtAVB.Text)
                        'Session("NomeDocStampaAssVolontario") = "Sostituzione Volontario - Estero"
                        'Session("NomeDocStampaAssVolontarioB") = "SostituzioneVolontariEsteriB"
                    End If

                ElseIf dtrgenerico("naz") = 4 Then  'GARANZIA GIOVANI
                    If dtrgenerico("datainizioservizio") = dtrgenerico("datainizioattività") Then
                        If Not dtrgenerico Is Nothing Then
                            dtrgenerico.Close()
                            dtrgenerico = Nothing
                        End If

                        Dim Documento As New GeneratoreModelli
                        'chiamo la proprietà della classe GeneratoreModelli che ritorna la path del documento creato
                        hplAssegnazioneVolontario.NavigateUrl = Documento.VOL_AssegnazioneVolontariGaranziaGiovani(Request.QueryString("IdEntitaSubentrante"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                        hplAssegnazioneVolontarioB.NavigateUrl = Documento.VOL_AssegnazioneVolontariGaranziaGiovaniB(Request.QueryString("IdEntitaSubentrante"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                        Documento.Dispose()

                        'hplDownload3.NavigateUrl = LetteraAssegnazioneVOlontario(Session("IdEnte"), "AssegnazioneVolontariNazionali")
                        'hplDownloadB.NavigateUrl = LetteraAssegnazioneVOlontario(Session("IdEnte"), "AssegnazioneVolontariNazionaliB")

                        'modificato il 08/04/2015 da simona cordella
                        'If ClsUtility.ControlloEntitàSPCPerGenerazioneContratto(Request.QueryString("IdEntitaSubentrante"), Session("conn")) = False Then
                        '    Select Case strTipoModello
                        '        Case "ORD"
                        '            hplAssegnazioneVolontarioB.NavigateUrl = Documento.VOL_AssegnazioneVolontariNazionaliB(Request.QueryString("IdEntitaSubentrante"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                        '        Case "EST"
                        '            hplAssegnazioneVolontarioB.NavigateUrl = Documento.VOL_AssegnazioneVolontariEsteroB(Request.QueryString("IdEntitaSubentrante"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                        '        Case "GG"
                        '            hplAssegnazioneVolontarioB.NavigateUrl = Documento.VOL_AssegnazioneVolontariGaranziaGiovaniB(Request.QueryString("IdEntitaSubentrante"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                        '        Case "SCD"
                        '            hplAssegnazioneVolontarioB.NavigateUrl = Documento.VOL_AssegnazioneVolontariNazionaliBSCD(Request.QueryString("IdEntitaSubentrante"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                        '    End Select
                        '    'hplAssegnazioneVolontarioB.NavigateUrl = Documento.VOL_AssegnazioneVolontariGaranziaGiovaniB(Request.QueryString("IdEntitaSubentrante"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                        '    strNomeDocumento = "AssegnazioneVolontariGaranziaGiovaniB"
                        'Else ' genero contratto Contratto Volontario Italia (12 mesi) (Garanzia Giovani Senza Presa In Carico)
                        '    hplAssegnazioneVolontario.NavigateUrl = Documento.VOL_AssegnazioneVolontariGaranziaGiovaniBSPC(Request.QueryString("IdEntitaSubentrante"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                        '    strNomeDocumento = "AssegnazioneVolontariGaranziaGiovaniBSPC"
                        'End If
                        Select Case strTipoModello
                            Case "ORD"
                                hplAssegnazioneVolontarioB.NavigateUrl = Documento.VOL_AssegnazioneVolontariNazionaliB(Request.QueryString("IdEntitaSubentrante"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                                strNomeDocumento = "AssegnazioneVolontariNazionaliB"
                            Case "EST"
                                hplAssegnazioneVolontarioB.NavigateUrl = Documento.VOL_AssegnazioneVolontariEsteroB(Request.QueryString("IdEntitaSubentrante"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                                strNomeDocumento = "AssegnazioneVolontariEsteroB"
                            Case "GG"
                                hplAssegnazioneVolontarioB.NavigateUrl = Documento.VOL_AssegnazioneVolontariGaranziaGiovaniB(Request.QueryString("IdEntitaSubentrante"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                                strNomeDocumento = "AssegnazioneVolontariGaranziaGiovaniB"
                            Case "SCD"
                                hplAssegnazioneVolontarioB.NavigateUrl = Documento.VOL_AssegnazioneVolontariNazionaliBSCD(Request.QueryString("IdEntitaSubentrante"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                                strNomeDocumento = "AssegnazioneVolontariNazionaliBSCD"
                        End Select
                        
                        Documento.Dispose()
                        'Cronologia creazione documento.
                        ClsUtility.CronologiaDocEntità(Request.QueryString("IdEntitaSubentrante"), Session("Utente"), "Assegnazione Volontario - Garanzia Giovani", Session("conn"), txtDataProtAV.Text, txtNumProtAV.Text)
                        ClsUtility.CronologiaDocEntità(Request.QueryString("IdEntitaSubentrante"), Session("Utente"), strNomeDocumento, Session("conn"), txtDataProtAVB.Text, txtNumProtAVB.Text)
                        'Session("NomeDocStampaAssVolontario") = "Assegnazione Volontario - Nazionale"
                        'Session("NomeDocStampaAssVolontarioB") = "AssegnazioneVolontariNazionaliB"
                    Else
                        If Not dtrgenerico Is Nothing Then
                            dtrgenerico.Close()
                            dtrgenerico = Nothing
                        End If

                        Dim Documento As New GeneratoreModelli
                        'chiamo la proprietà della classe GeneratoreModelli che ritorna la path del documento creato
                        hplAssegnazioneVolontario.NavigateUrl = Documento.VOL_SostituzioneVolontariGaranziaGiovani(Request.QueryString("IdEntitaSubentrante"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))

                        ''modificato il 08/04/2015 da simona cordella
                        'If ClsUtility.ControlloEntitàSPCPerGenerazioneContratto(Request.QueryString("IdEntitaSubentrante"), Session("conn")) = False Then
                        '    Select Case strTipoModello
                        '        Case "ORD"
                        '            hplAssegnazioneVolontarioB.NavigateUrl = Documento.VOL_SostituzioneVolontariNazionaliB(Request.QueryString("IdEntitaSubentrante"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                        '        Case "EST"
                        '            hplAssegnazioneVolontarioB.NavigateUrl = Documento.VOL_SostituzioneVolontariEsteriB(Request.QueryString("IdEntitaSubentrante"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                        '        Case "GG"
                        '            hplAssegnazioneVolontarioB.NavigateUrl = Documento.VOL_SostituzioneVolontariGaranziaGiovaniB(Request.QueryString("IdEntitaSubentrante"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                        '        Case "SCD"
                        '            hplAssegnazioneVolontarioB.NavigateUrl = Documento.VOL_SostituzioneVolontariNazionaliBSCD(Request.QueryString("IdEntitaSubentrante"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                        '    End Select
                        '    'hplAssegnazioneVolontarioB.NavigateUrl = Documento.VOL_SostituzioneVolontariGaranziaGiovaniB(Request.QueryString("IdEntitaSubentrante"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                        '    strNomeDocumento = "SostituzioneVolontariGaranziaGiovaniB"
                        'Else ' genero contratto Contratto Volontario Italia (12 mesi) (Garanzia Giovani Senza Presa In Carico)
                        '    hplAssegnazioneVolontarioB.NavigateUrl = Documento.VOL_SostituzioneVolontariGaranziaGiovaniBSPC(Request.QueryString("IdEntitaSubentrante"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                        '    strNomeDocumento = "SostituzioneVolontariGaranziaGiovaniBSPC"
                        'End If
                        Select Case strTipoModello
                            Case "ORD"
                                hplAssegnazioneVolontarioB.NavigateUrl = Documento.VOL_SostituzioneVolontariNazionaliB(Request.QueryString("IdEntitaSubentrante"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                                strNomeDocumento = "SostituzioneVolontariNazionaliB"
                            Case "EST"
                                hplAssegnazioneVolontarioB.NavigateUrl = Documento.VOL_SostituzioneVolontariEsteriB(Request.QueryString("IdEntitaSubentrante"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                                strNomeDocumento = "SostituzioneVolontariEsteriB"
                            Case "GG"
                                hplAssegnazioneVolontarioB.NavigateUrl = Documento.VOL_SostituzioneVolontariGaranziaGiovaniB(Request.QueryString("IdEntitaSubentrante"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                                strNomeDocumento = "SostituzioneVolontariGaranziaGiovaniB"
                            Case "SCD"
                                hplAssegnazioneVolontarioB.NavigateUrl = Documento.VOL_SostituzioneVolontariNazionaliBSCD(Request.QueryString("IdEntitaSubentrante"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                                strNomeDocumento = "SostituzioneVolontariNazionaliBSCD"
                        End Select



                        Documento.Dispose()

                        'hplDownload3.NavigateUrl = LetteraAssegnazioneVOlontario(Session("IdEnte"), "SostituzioneVolontariNazionali")
                        'hplDownloadB.NavigateUrl = LetteraAssegnazioneVOlontario(Session("IdEnte"), "SostituzioneVolontariNazionaliB")

                        'Cronologia creazione documento.
                        ClsUtility.CronologiaDocEntità(Request.QueryString("IdEntitaSubentrante"), Session("Utente"), "Sostituzione Volontario - Garanzia Giovani", Session("conn"), txtDataProtAV.Text, txtNumProtAV.Text)
                        ClsUtility.CronologiaDocEntità(Request.QueryString("IdEntitaSubentrante"), Session("Utente"), strNomeDocumento, Session("conn"), txtDataProtAVB.Text, txtNumProtAVB.Text)

                    End If
                End If


            End If
        End If

        '************************************************************************************************************************************
        '****************************************************Lettera Chiusura Iniziale*******************************************************
        '************************************************************************************************************************************
        If chkLetteraChiusura.Checked = True Then
            'Chiusura Iniziale
            imgGeneraFile.Visible = False
            hplLetteraChiusura.Target = "_blank"
            hplLetteraChiusura.Visible = True

            'agg. sc il 30/12/2008 Abilito text del fascicolo e dei protocolli (SIGED)
            FascicoloVolotarioSostituito(True)
            ProtocolliLetteraChiusuraIniziale(True)
            Dim Documento As New GeneratoreModelli
            'chiamo la proprietà della classe GeneratoreModelli che ritorna la path del documento creato
            hplLetteraChiusura.NavigateUrl = Documento.VOL_ChiusuraIniziale(Request.QueryString("IdVolVecchio"), Session("IdEnte"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
            Documento.Dispose()

            'hplChiusura.NavigateUrl = LetteraChiusuraIniziale(Session("IdEnte"), "ChiusuraIniziale")
            'Cronologia creazione documento.
            ClsUtility.CronologiaDocEntità(Request.QueryString("identita"), Session("Utente"), "Chiusura Iniziale", Session("conn"), txtDataProtLCI.Text, txtNumProtLCI.Text)
        End If

        '************************************************************************************************************************************
        '****************************************************Lettera Chiusura In Servizio****************************************************
        '************************************************************************************************************************************
        'modificata aprile 2022 per suddivisione 4 modelli (ORD, GG, EST, SCD)
        If chkLetteraChiusuraInServizio.Checked = True Then
            'Chiusura Iniziale
            imgGeneraFile.Visible = False
            hplLetteraChiusuraInServizio.Target = "_blank"
            hplLetteraChiusuraInServizio.Visible = True
            Dim Documento As New GeneratoreModelli

            Select Case strTipoModello
                Case "ORD"
                    hplLetteraChiusuraInServizio.NavigateUrl = Documento.VOL_ChiusuraInServizio(Request.QueryString("IdVolVecchio"), Session("IdEnte"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                    ClsUtility.CronologiaDocEntità(Request.QueryString("identita"), Session("Utente"), "Chiusura In Servizio", Session("conn"), txtDataProtLCS.Text, txtNumProtLCS.Text)
                Case "EST"
                    hplLetteraChiusuraInServizio.NavigateUrl = Documento.VOL_ChiusuraInServizioEstero(Request.QueryString("IdVolVecchio"), Session("IdEnte"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                    ClsUtility.CronologiaDocEntità(Request.QueryString("identita"), Session("Utente"), "Chiusura In Servizio", Session("conn"), txtDataProtLCS.Text, txtNumProtLCS.Text)
                Case "GG"
                    hplLetteraChiusuraInServizio.NavigateUrl = Documento.VOL_ChiusuraInServizioGaranziaGiovani(Request.QueryString("IdVolVecchio"), Session("IdEnte"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                    ClsUtility.CronologiaDocEntità(Request.QueryString("identita"), Session("Utente"), "Chiusura In Servizio Garanzia Giovani", Session("conn"), txtDataProtLCS.Text, txtNumProtLCS.Text)
                Case "SCD"
                    hplLetteraChiusuraInServizio.NavigateUrl = Documento.VOL_ChiusuraInServizioSCD(Request.QueryString("IdVolVecchio"), Session("IdEnte"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                    ClsUtility.CronologiaDocEntità(Request.QueryString("identita"), Session("Utente"), "Chiusura In Servizio", Session("conn"), txtDataProtLCS.Text, txtNumProtLCS.Text)
            End Select

            Documento.Dispose()
            FascicoloVolotarioSostituito(True)
            ProtocolliLetteraChiusuraServizio(True)
        End If

        '************************************************************************************************************************************
        '****************************************************Lettera Esclusione Per Assenza Ingiustificata***********************************
        '************************************************************************************************************************************
        'modificata aprile 2022 per suddivisione 4 modelli (ORD, GG, EST, SCD)
        If chkLetteraEsclusionePerAssenzaIngiustificata.Checked = True Then
            imgGeneraFile.Visible = False
            hplLetteraEsclusionePerAssenzaIngiustificata.Target = "_blank"
            hplLetteraEsclusionePerAssenzaIngiustificata.Visible = True

            Dim Documento As New GeneratoreModelli

            Select Case strTipoModello
                Case "ORD"
                    hplLetteraEsclusionePerAssenzaIngiustificata.NavigateUrl = Documento.VOL_LetteraEsclusionePerAssenzaIngiustificata(Request.QueryString("IdVolVecchio"), Session("IdEnte"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                    ClsUtility.CronologiaDocEntità(Request.QueryString("identita"), Session("Utente"), "Lettera Esclusione Per Assenza Ingiustificata", Session("conn"), txtDataProtLEAI.Text, txtNumProtLEAI.Text)
                Case "EST"
                    hplLetteraEsclusionePerAssenzaIngiustificata.NavigateUrl = Documento.VOL_LetteraEsclusionePerAssenzaIngiustificataEstero(Request.QueryString("IdVolVecchio"), Session("IdEnte"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                    ClsUtility.CronologiaDocEntità(Request.QueryString("identita"), Session("Utente"), "Lettera Esclusione Per Assenza Ingiustificata", Session("conn"), txtDataProtLEAI.Text, txtNumProtLEAI.Text)
                Case "GG"
                    hplLetteraEsclusionePerAssenzaIngiustificata.NavigateUrl = Documento.VOL_LetteraEsclusionePerAssenzaIngiustificataGaranziaGiovani(Request.QueryString("IdVolVecchio"), Session("IdEnte"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                    ClsUtility.CronologiaDocEntità(Request.QueryString("identita"), Session("Utente"), "Lettera Esclusione Per Assenza Ingiustificata Garanzia Giovani", Session("conn"), txtDataProtLEAI.Text, txtNumProtLEAI.Text)
                Case "SCD"
                    hplLetteraEsclusionePerAssenzaIngiustificata.NavigateUrl = Documento.VOL_LetteraEsclusionePerAssenzaIngiustificataSCD(Request.QueryString("IdVolVecchio"), Session("IdEnte"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                    ClsUtility.CronologiaDocEntità(Request.QueryString("identita"), Session("Utente"), "Lettera Esclusione Per Assenza Ingiustificata", Session("conn"), txtDataProtLEAI.Text, txtNumProtLEAI.Text)
            End Select

            Documento.Dispose()
            FascicoloVolotarioSostituito(True)
            ProtocolliLetteraEsclusioneAssenzaIngistificata(True)
        End If



        '************************************************************************************************************************************
        '****************************************************Lettera Esclusione Per Giorni Permesso******************************************
        '************************************************************************************************************************************
        'modificata aprile 2022 per suddivisione 4 modelli (ORD, GG, EST, SCD)
        If chkLetteraEsclusionePerGiorniPermesso.Checked = True Then
            imgGeneraFile.Visible = False
            hplLetteraEsclusionePerGiorniPermesso.Target = "_blank"
            hplLetteraEsclusionePerGiorniPermesso.Visible = True
            Dim Documento As New GeneratoreModelli

            Select Case strTipoModello
                Case "ORD"
                    hplLetteraEsclusionePerGiorniPermesso.NavigateUrl = Documento.VOL_LetteraEsclusionePerGiorniPermesso(Request.QueryString("IdVolVecchio"), Session("IdEnte"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                    ClsUtility.CronologiaDocEntità(Request.QueryString("identita"), Session("Utente"), "Lettera Esclusione Per Giorni Permesso", Session("conn"), txtDataProtLEGP.Text, txtNumProtLEGP.Text)
                Case "EST"
                    hplLetteraEsclusionePerGiorniPermesso.NavigateUrl = Documento.VOL_LetteraEsclusionePerGiorniPermessoEstero(Request.QueryString("IdVolVecchio"), Session("IdEnte"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                    ClsUtility.CronologiaDocEntità(Request.QueryString("identita"), Session("Utente"), "Lettera Esclusione Per Giorni Permesso", Session("conn"), txtDataProtLEGP.Text, txtNumProtLEGP.Text)
                Case "GG"
                    hplLetteraEsclusionePerGiorniPermesso.NavigateUrl = Documento.VOL_LetteraEsclusionePerGiorniPermessoGaranziaGiovani(Request.QueryString("IdVolVecchio"), Session("IdEnte"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                    ClsUtility.CronologiaDocEntità(Request.QueryString("identita"), Session("Utente"), "Lettera Esclusione Per Giorni Permesso Garanzia Giovani", Session("conn"), txtDataProtLEGP.Text, txtNumProtLEGP.Text)
                Case "SCD"
                    hplLetteraEsclusionePerGiorniPermesso.NavigateUrl = Documento.VOL_LetteraEsclusionePerGiorniPermessoSCD(Request.QueryString("IdVolVecchio"), Session("IdEnte"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                    ClsUtility.CronologiaDocEntità(Request.QueryString("identita"), Session("Utente"), "Lettera Esclusione Per Giorni Permesso", Session("conn"), txtDataProtLEGP.Text, txtNumProtLEGP.Text)
            End Select

            Documento.Dispose()
            FascicoloVolotarioSostituito(True)
            ProtocolliLetteraEsclusioneGiorniPermesso(True)
        End If

        '************************************************************************************************************************************
        '****************************************************Lettera Esclusione Per Superamento Malattia*************************************
        '************************************************************************************************************************************
        'modificata aprile 2022 per suddivisione 4 modelli (ORD, GG, EST, SCD)
        If chkLetteraEsclusionePerSuperamentoMalattia.Checked = True Then
            imgGeneraFile.Visible = False

            hplLetteraEsclusionePerSuperamentoMalattia.Target = "_blank"
            hplLetteraEsclusionePerSuperamentoMalattia.Visible = True
            Dim Documento As New GeneratoreModelli

            Select Case strTipoModello
                Case "ORD"
                    If VerificaCausale(Request.QueryString("identita")) = 1 Then
                        hplLetteraEsclusionePerSuperamentoMalattia.NavigateUrl = Documento.VOL_LetteraEsclusionePerSuperamentoMalattia(Request.QueryString("IdVolVecchio"), Session("IdEnte"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                        ClsUtility.CronologiaDocEntità(Request.QueryString("identita"), Session("Utente"), "Lettera Esclusione Per Superamento Malattia", Session("conn"), txtDataProtLESM.Text, txtNumProtLESM.Text)
                    Else
                        hplLetteraEsclusionePerSuperamentoMalattia.NavigateUrl = Documento.VOL_LetteraEsclusionePerSuperamentoMalattiaDopoi6Mesi(Request.QueryString("IdVolVecchio"), Session("IdEnte"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                        ClsUtility.CronologiaDocEntità(Request.QueryString("identita"), Session("Utente"), "Lettera Esclusione Per Superamento Malattia", Session("conn"), txtDataProtLESM.Text, txtNumProtLESM.Text)
                    End If
                Case "EST"
                    If VerificaCausale(Request.QueryString("identita")) = 1 Then
                        hplLetteraEsclusionePerSuperamentoMalattia.NavigateUrl = Documento.VOL_LetteraEsclusionePerSuperamentoMalattiaEstero(Request.QueryString("IdVolVecchio"), Session("IdEnte"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                        ClsUtility.CronologiaDocEntità(Request.QueryString("identita"), Session("Utente"), "Lettera Esclusione Per Superamento Malattia", Session("conn"), txtDataProtLESM.Text, txtNumProtLESM.Text)
                    Else
                        hplLetteraEsclusionePerSuperamentoMalattia.NavigateUrl = Documento.VOL_LetteraEsclusionePerSuperamentoMalattiaDopoi6MesiEstero(Request.QueryString("IdVolVecchio"), Session("IdEnte"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                        ClsUtility.CronologiaDocEntità(Request.QueryString("identita"), Session("Utente"), "Lettera Esclusione Per Superamento Malattia", Session("conn"), txtDataProtLESM.Text, txtNumProtLESM.Text)
                    End If
                Case "GG"
                    If VerificaCausale(Request.QueryString("identita")) = 1 Then
                        hplLetteraEsclusionePerSuperamentoMalattia.NavigateUrl = Documento.VOL_LetteraEsclusionePerSuperamentoMalattiaGaranziaGiovani(Request.QueryString("IdVolVecchio"), Session("IdEnte"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                    Else
                        hplLetteraEsclusionePerSuperamentoMalattia.NavigateUrl = Documento.VOL_LetteraEsclusionePerSuperamentoMalattiaDopoi6MesiGaranziaGiovani(Request.QueryString("IdVolVecchio"), Session("IdEnte"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                    End If
                    ClsUtility.CronologiaDocEntità(Request.QueryString("identita"), Session("Utente"), "Lettera Esclusione Per Superamento Malattia Garanzia Giovani", Session("conn"), txtDataProtLESM.Text, txtNumProtLESM.Text)
                Case "SCD"
                    If VerificaCausale(Request.QueryString("identita")) = 1 Then
                        hplLetteraEsclusionePerSuperamentoMalattia.NavigateUrl = Documento.VOL_LetteraEsclusionePerSuperamentoMalattiaSCD(Request.QueryString("IdVolVecchio"), Session("IdEnte"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                        ClsUtility.CronologiaDocEntità(Request.QueryString("identita"), Session("Utente"), "Lettera Esclusione Per Superamento Malattia", Session("conn"), txtDataProtLESM.Text, txtNumProtLESM.Text)
                    Else
                        hplLetteraEsclusionePerSuperamentoMalattia.NavigateUrl = Documento.VOL_LetteraEsclusionePerSuperamentoMalattiaDopoi6MesiSCD(Request.QueryString("IdVolVecchio"), Session("IdEnte"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                        ClsUtility.CronologiaDocEntità(Request.QueryString("identita"), Session("Utente"), "Lettera Esclusione Per Superamento Malattia", Session("conn"), txtDataProtLESM.Text, txtNumProtLESM.Text)
                    End If
            End Select

            Documento.Dispose()
            FascicoloVolotarioSostituito(True)
            ProtocolliLetteraEsclusioneSuperamentoMalattia(True)
        End If

        '************************************************************************************************************************************
        '****************************************************LetteraEsclusione***************************************************************
        '************************************************************************************************************************************
        If chkLetteraEsclusione.Checked = True Then
            imgGeneraFile.Visible = False
            Dim Documento As New GeneratoreModelli
            hplLetteraEsclusione.Visible = True

            If (idTipoProgetto = PROGETTO_GARANZIA_GIOVANI) Then
                hplLetteraEsclusione.NavigateUrl = Documento.VOL_LetteraEsclusioneGaranziaGiovani(Request.QueryString("IdVolVecchio"), Session("IdEnte"), Request.QueryString("idattivita"), Request.QueryString("CodiceFiscale"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                ClsUtility.CronologiaDocEntità(Request.QueryString("identita"), Session("Utente"), "Lettera Esclusione Volontario Garanzia Giovani", Session("conn"), txtDataProtLEDD.Text, txtNumProtLEDD.Text)
            Else
                hplLetteraEsclusione.NavigateUrl = Documento.VOL_LetteraEsclusione(Request.QueryString("IdVolVecchio"), Session("IdEnte"), Request.QueryString("idattivita"), Request.QueryString("CodiceFiscale"), Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn"))
                ClsUtility.CronologiaDocEntità(Request.QueryString("identita"), Session("Utente"), "Lettera Esclusione Volontario", Session("conn"), txtDataProtLEDD.Text, txtNumProtLEDD.Text)
            End If
            Documento.Dispose()
            FascicoloVolotarioSostituito(True)
            ProtocolliLetteraEsclusioneDoppiaDomanda(True)
        End If
    End Sub


    Protected Sub imgChiudi_Click(ByVal sender As Object, ByVal e As EventArgs) Handles imgChiudi.Click
        Dim url As StringBuilder = New StringBuilder()
        url.Append("WfrmGestioneSostituisciVolontari.aspx")
        url.Append("?")
        url.Append("IdAttivita=")
        url.Append(Request.QueryString("idattivita"))
        url.Append("&")
        url.Append("identita=")
        url.Append(Request.QueryString("identita"))
        url.Append("&")
        url.Append("CodiceFiscale=")
        url.Append(Request.QueryString("CodiceFiscale"))
        Response.Redirect(url.ToString())

    End Sub
End Class
