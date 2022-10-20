
Imports System.Data.SqlClient
Imports System.IO
Public Class ver_Sanzione
    Inherits System.Web.UI.Page
    Dim DtbStampa As New DataTable
    Shared IdEnte As Integer
#Region " Codice generato da Progettazione Web Form "

    'Chiamata richiesta da Progettazione Web Form.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    'Protected WithEvents Label1 As System.Web.UI.WebControls.Label
    'Protected WithEvents lblProgrammazione As System.Web.UI.WebControls.Label
    'Protected WithEvents lblIspettore As System.Web.UI.WebControls.Label
    'Protected WithEvents lblStatoVerifica As System.Web.UI.WebControls.Label
    Protected WithEvents lblEnte As System.Web.UI.WebControls.Label
    Protected WithEvents LblDatiEnte As System.Web.UI.WebControls.Label
    'Protected WithEvents Label2 As System.Web.UI.WebControls.Label
    'Protected WithEvents lblProgetto As System.Web.UI.WebControls.Label
    'Protected WithEvents lblDataInizioProgetto As System.Web.UI.WebControls.Label
    'Protected WithEvents lblDataFineProgetto As System.Web.UI.WebControls.Label
    'Protected WithEvents lblDataAssegnazione As System.Web.UI.WebControls.Label
    'Protected WithEvents lblDataApprovazione As System.Web.UI.WebControls.Label
    'Protected WithEvents lblDataPrevistaVerifica As System.Web.UI.WebControls.Label
    'Protected WithEvents lblDataFinePrevistaVerifica As System.Web.UI.WebControls.Label
    'Protected WithEvents lblTipologiaVerifica As System.Web.UI.WebControls.Label
    'Protected WithEvents LblCompetenzaProg As System.Web.UI.WebControls.Label
    'Protected WithEvents LblDataProtTrasmSanzione As System.Web.UI.WebControls.Label
    'Protected WithEvents LblNumProtTrasmSanzione As System.Web.UI.WebControls.Label
    'Protected WithEvents LblDataProtEsecSanzione As System.Web.UI.WebControls.Label
    'Protected WithEvents LblNumProtEsecSanzione As System.Web.UI.WebControls.Label
    'Protected WithEvents LblDataEsecSanzione As System.Web.UI.WebControls.Label
    'Protected WithEvents LblCompetenza As System.Web.UI.WebControls.Label
    'Protected WithEvents LblUfficio As System.Web.UI.WebControls.Label
    'Protected WithEvents lblmessaggio As System.Web.UI.WebControls.Label
    'Protected WithEvents LblSanzione As System.Web.UI.WebControls.Label
    'Protected WithEvents ddlSanzione As System.Web.UI.WebControls.DropDownList
    'Protected WithEvents dgProgetti As System.Web.UI.WebControls.DataGrid
    'Protected WithEvents dgEnteCapofila As System.Web.UI.WebControls.DataGrid
    'Protected WithEvents dgEnteDipendente As System.Web.UI.WebControls.DataGrid
    'Protected WithEvents dgSedi As System.Web.UI.WebControls.DataGrid
    'Protected WithEvents dgSediAttuazione As System.Web.UI.WebControls.DataGrid
    'Protected WithEvents dgSediProgetto As System.Web.UI.WebControls.DataGrid
    'Protected WithEvents cmdSalva As System.Web.UI.WebControls.ImageButton
    'Protected WithEvents cmdChiudi As System.Web.UI.WebControls.ImageButton
    'Protected WithEvents dgSanzione As System.Web.UI.WebControls.DataGrid
    'Protected WithEvents cmdApplica As System.Web.UI.WebControls.ImageButton
    Protected WithEvents dtEnteCapofila As System.Web.UI.WebControls.DataGrid
    Protected WithEvents dtEnteDipendente As System.Web.UI.WebControls.DataGrid
    Protected WithEvents dtSedi As System.Web.UI.WebControls.DataGrid
    Protected WithEvents dtSediAttuazione As System.Web.UI.WebControls.DataGrid
    Protected WithEvents dtSediProgetto As System.Web.UI.WebControls.DataGrid

    'Protected WithEvents imgStampa As System.Web.UI.WebControls.ImageButton
    'Protected WithEvents hddControllo As System.Web.UI.HtmlControls.HtmlInputHidden
    'NOTA: la seguente dichiarazione è richiesta da Progettazione Web Form.
    'Non spostarla o rimuoverla.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: questa chiamata al metodo è richiesta da Progettazione Web Form.
        'Non modificarla nell'editor del codice.
        InitializeComponent()
    End Sub

#End Region
    '************* CREATA IL 09/01/2008  DA SIMONA CORDELLA*****************
    '************* GESTIONE DELLA SANZIONE *****************
    '************* PROGETTO, ENTE CAPOFILA, ENTE FIGLIO, SEDI, SEDI ATTUAZIONE, SEDI DI PROGETTO ********

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Inserire qui il codice utente necessario per inizializzare la pagina
        If Not Session("LogIn") Is Nothing Then
            If Session("LogIn") = False Then 'verifico validità log-in
                Response.Redirect("LogOn.aspx")
            End If
        Else
            Response.Redirect("LogOn.aspx")
        End If
        If Page.IsPostBack = False Then
            ' Page.SmartNavigation = True
            Session("dtSanzione") = Nothing
            IdEnte = Request.QueryString("Idente")
            CaricaMaschera()
        End If
    End Sub

    Sub CaricaMaschera()
        CaricaIntestazione()
        CaricaCombo()
        If Request.QueryString("Segnalata") = Nothing Then ' VENGO DA verificarequisiti.aspx
            CaricaProgetto()
            CaricaEnteCapofila()
            CaricaEnteDipendenti()
            CaricaSedi()
            CaricaSediAttuazione()
            CaricaSediProgetto()
            CaricaSanzione()
        Else 'VENGO DA ver_VerificaSegnalataUNSC.aspx
            CaricaEnteSEGNALATAUNSC()
            CaricaProgettoSEGNALATAUNSC()
            CaricaSediAttuazioneSEGNALATAUNSC()
            CaricaEnteDipendenteSEGNALATAUNSC()
            CaricaSanzione()
        End If
        'modificato il 02/08/2012 
        'Sospensione controllo sullo stato sanzionata per inibizione pulsante Applica da verificare per gestione dei deflettori 

        'If lblStatoVerifica.Text = "Sanzionata" Then
        '    cmdApplica.Visible = False
        'End If
    End Sub

    Sub CaricaIntestazione()
        Dim dtrGenerico As SqlClient.SqlDataReader
        Dim strSql As String
        Dim DataSetRicerca As DataSet

        'dgRisultatoRicerca.CurrentPageIndex = 0

        strSql = "select IdVerifica, "
        strSql = strSql & " StatoVerifiche, "
        strSql = strSql & "IdIGF, "
        strSql = strSql & "Programmazione, "
        strSql = strSql & "CodiceFascicolo, "
        strSql = strSql & "StatoVerifiche, "
        strSql = strSql & "IdAttività, "
        strSql = strSql & "dbo.FormatoData(DataAssegnazione) AS DataAssegnazione, "
        strSql = strSql & "dbo.FormatoData(DataApprovazione) AS DataApprovazione, "
        strSql = strSql & "dbo.FormatoData(DataPrevistaVerifica) as DataPrevistaVerifica, "
        strSql = strSql & "dbo.FormatoData(DataFinePrevistaVerifica) as DataFinePrevistaVerifica, "
        strSql = strSql & "dbo.FormatoData(DataFineVerifica) as DataFineVerifica, "
        strSql = strSql & "dbo.FormatoData(DataInizioAttività) as DataInizioAttività, "
        strSql = strSql & "dbo.FormatoData(DataFineAttività) as DataFineAttività, "
        strSql = strSql & "(Nominativo + '(' + case when tipoverificatore = 0 then 'Interno' when tipoverificatore = 1 then 'IGF' END + ')') as Nominativo, "
        strSql = strSql & "(Denominazione + '(' + CodiceEnte + ')') as Denominazione, "
        strSql = strSql & "(Titolo + ' (' + CodiceProgetto + ')') as Titolo, "
        strSql = strSql & "IDEnteSedeAttuazione, "
        strSql = strSql & "(EnteFiglio + ' (' + convert(varchar,IDEnteSedeAttuazione)  + ')') As EnteFiglio, "
        strSql = strSql & "(Comune + '(' + DescrAbb + ')') as Comune, "
        strSql = strSql & "Regione,CodiceProgetto, "
        strSql = strSql & "case when tipologia = 1 then 'Programmata' when tipologia = 2 then 'Segnalata' end as TipoVerifica, CodiceEnte, competenza,idregcompetenza "
        strSql = strSql & " from ver_vw_ricerca_verifiche "
        strSql = strSql & "where idverifica = " & Request.QueryString("idverifica") & " "

        If Not dtrGenerico Is Nothing Then
            dtrGenerico.Close()
            dtrGenerico = Nothing
        End If
        dtrGenerico = ClsServer.CreaDatareader(strSql, Session("conn"))
        If dtrGenerico.HasRows Then
            dtrGenerico.Read()
            lblProgrammazione.Text = IIf(IsDBNull(dtrGenerico("Programmazione")) = True, "", dtrGenerico("Programmazione"))
            lblIspettore.Text = IIf(IsDBNull(dtrGenerico("Nominativo")) = True, "", dtrGenerico("Nominativo"))
            lblEnte.Text = IIf(IsDBNull(dtrGenerico("Denominazione")) = True, "", dtrGenerico("Denominazione"))

            lblStatoVerifica.Text = IIf(IsDBNull(dtrGenerico("StatoVerifiche")) = True, "", dtrGenerico("StatoVerifiche"))
            lblTipologiaVerifica.Text = IIf(IsDBNull(dtrGenerico("TipoVerifica")) = True, "", dtrGenerico("TipoVerifica"))
            lblProgetto.Text = IIf(IsDBNull(dtrGenerico("Titolo")) = True, "", dtrGenerico("Titolo"))
            lblDataAssegnazione.Text = IIf(IsDBNull(dtrGenerico("DataAssegnazione")) = True, "", dtrGenerico("DataAssegnazione"))
            lblDataApprovazione.Text = IIf(IsDBNull(dtrGenerico("DataApprovazione")) = True, "", dtrGenerico("DataApprovazione")) 'clsConversione.DaNullInStringaVuota(dtrGenerico("DataApprovazione"))
            lblDataPrevistaVerifica.Text = IIf(IsDBNull(dtrGenerico("DataPrevistaVerifica")) = True, "", dtrGenerico("DataPrevistaVerifica")) 'clsConversione.DaNullInStringaVuota(dtrGenerico("DataPrevistaVerifica"))
            lblDataFinePrevistaVerifica.Text = IIf(IsDBNull(dtrGenerico("DataFinePrevistaVerifica")) = True, "", dtrGenerico("DataFinePrevistaVerifica"))
            lblDataInizioProgetto.Text = IIf(IsDBNull(dtrGenerico("DataInizioAttività")) = True, "", dtrGenerico("DataInizioAttività"))
            lblDataFineProgetto.Text = IIf(IsDBNull(dtrGenerico("DataFineAttività")) = True, "", dtrGenerico("DataFineAttività"))
            Session("pCodEnte") = IIf(IsDBNull(dtrGenerico("CodiceEnte")) = True, "", dtrGenerico("CodiceEnte"))
            Session("IdAttività") = dtrGenerico("IdAttività")

            LblCompetenzaProg.Text = dtrGenerico("competenza")
            Session("IdRegCompetenza") = dtrGenerico("idregcompetenza")
            If Not dtrGenerico Is Nothing Then
                dtrGenerico.Close()
                dtrGenerico = Nothing
            End If

            strSql = " select (indirizzo + ' ' + civico + ' - ' + cap + ' ' + comune + ' (' + provinciabreve + ')' + ' Tel. ' + PrefissoTelefonorichiestaregistrazione + Telefonorichiestaregistrazione + ' Fax ' + PrefissoFax + Fax )as Infoentepadre from VW_BO_ENTI where codiceregione ='" & Session("pCodEnte") & "' "
            dtrGenerico = ClsServer.CreaDatareader(strSql, Session("conn"))

            If dtrGenerico.HasRows = True Then
                dtrGenerico.Read()
                LblDatiEnte.Text = IIf(IsDBNull(dtrGenerico("Infoentepadre")) = True, "", dtrGenerico("Infoentepadre"))
            End If
            If Not dtrGenerico Is Nothing Then
                dtrGenerico.Close()
                dtrGenerico = Nothing
            End If
        End If

        If Not dtrGenerico Is Nothing Then
            dtrGenerico.Close()
            dtrGenerico = Nothing
        End If

        strSql = "SELECT "
        strSql = strSql & " DataTrasmissioneSanzione,DataProtTrasmissioneSanzione,NProtTrasmissioneSanzione,DataEsecuzioneSanzione,"
        strSql = strSql & " DataProtEsecuzioneSanzione,NProtEsecuzioneSanzione, TUfficiUNSC.Ufficio, RegioniCompetenze.Descrizione"
        strSql = strSql & " FROM TVerifiche "
        strSql = strSql & " LEFT JOIN TUfficiUNSC ON TVerifiche.IDUfficioUNSC = TUfficiUNSC.IdUfficio "
        strSql = strSql & " LEFT JOIN RegioniCompetenze ON TVerifiche.IDRegioneCompetenza = RegioniCompetenze.IdRegioneCompetenza "
        strSql = strSql & " Where IdVerifica = " & Request.QueryString("IdVerifica")
        If Not dtrGenerico Is Nothing Then
            dtrGenerico.Close()
            dtrGenerico = Nothing
        End If
        dtrGenerico = ClsServer.CreaDatareader(strSql, Session("Conn"))

        dtrGenerico.Read()

        LblDataProtTrasmSanzione.Text = "" & dtrGenerico("DataProtTrasmissioneSanzione")
        LblNumProtTrasmSanzione.Text = "" & dtrGenerico("NProtTrasmissioneSanzione")
        LblDataEsecSanzione.Text = "" & dtrGenerico("DataEsecuzioneSanzione")
        LblDataProtEsecSanzione.Text = "" & dtrGenerico("DataProtEsecuzioneSanzione")
        LblNumProtEsecSanzione.Text = "" & dtrGenerico("NProtEsecuzioneSanzione")
        If IsDBNull(dtrGenerico("Ufficio")) = False Then
            LblUfficio.Text = "" & dtrGenerico("Ufficio")
        End If
        If IsDBNull(dtrGenerico("Descrizione")) = False Then
            LblCompetenza.Text = "" & dtrGenerico("Descrizione")
        End If
        If Not dtrGenerico Is Nothing Then
            dtrGenerico.Close()
            dtrGenerico = Nothing
        End If
    End Sub
    Sub CaricaCombo()
        ddlSanzione.DataSource = MakeParentTable("SELECT  IDSanzione  , Descrizione  FROM TVerificheSanzioni  WHERE  Abilitato = 0 ")
        ddlSanzione.DataTextField = "ParentItem"
        ddlSanzione.DataValueField = "id"
        ddlSanzione.DataBind()
    End Sub

    Private Sub CaricaProgetto()
        Dim strSql As String
        Dim DataSetProg As DataSet

        strSql = "Select distinct CodiceProgetto, (Titolo + ' (' + CodiceProgetto + ')') as Titolo,idattività as idattivita"
        strSql = strSql & " from ver_vw_ricerca_verifiche "
        strSql = strSql & " where idverifica = " & Request.QueryString("idverifica") & " "

        DataSetProg = ClsServer.DataSetGenerico(strSql, Session("conn"))
        dgProgetti.DataSource = DataSetProg
        dgProgetti.DataBind()
    End Sub
    Private Sub CaricaEnteCapofila()
        Dim strSql As String
        Dim DataSetEnteC As DataSet

        strSql = " Select distinct(Denominazione + '(' + CodiceEnte + ')') as Denominazione,idente "
        strSql = strSql & " from ver_vw_ricerca_verifiche "
        strSql = strSql & "where idverifica = " & Request.QueryString("idverifica") & " "
        DataSetEnteC = ClsServer.DataSetGenerico(strSql, Session("conn"))
        dgEnteCapofila.DataSource = DataSetEnteC
        dgEnteCapofila.DataBind()
    End Sub
    Private Sub CaricaEnteSEGNALATAUNSC()
        'aggiunto il 10/11/2011 da simona cordella
        Dim strSql As String
        Dim DataSetEnteC As DataSet

        strSql = " Select distinct( enti.Denominazione + '(' +  enti.CodiceRegione + ')') as Denominazione,TVERIFICHE.idente "
        strSql = strSql & " from TVERIFICHE "
        strSql = strSql & " INNER JOIN enti ON TVerifiche.IDEnte = enti.IDEnte "
        strSql = strSql & "where TVerifiche.idverifica = " & Request.QueryString("idverifica") & " "
        DataSetEnteC = ClsServer.DataSetGenerico(strSql, Session("conn"))
        dgEnteCapofila.DataSource = DataSetEnteC
        dgEnteCapofila.DataBind()
    End Sub
    Private Sub CaricaProgettoSEGNALATAUNSC()
        'aggiunto il 10/11/2011 da simona cordella
        Dim strSql As String
        Dim DataSetProg As DataSet

        strSql = "Select distinct '' as  CodiceProgetto, (attività.Titolo + ' (' + attività.CodiceEnte + ')') as Titolo,TVerifiche.IDAttivita "
        strSql = strSql & " from TVerifiche "
        strSql = strSql & " INNER JOIN attività ON TVerifiche.IDAttivita = attività.IDAttività "
        strSql = strSql & " where TVerifiche.idverifica = " & Request.QueryString("idverifica") & " "

        DataSetProg = ClsServer.DataSetGenerico(strSql, Session("conn"))
        dgProgetti.DataSource = DataSetProg
        dgProgetti.DataBind()
    End Sub
    Private Sub CaricaSediAttuazioneSEGNALATAUNSC()
        'sedi attuazione
        'modifica il 20/06/2011 
        'modifica query , sono state aggiunte le colonne indirizzo e comune della sede di attuazione
        Dim strSql As String
        Dim DataSetEnteC As DataSet
        strSql = " Select  distinct TVerifiche.identesedeattuazione, "
        strSql = strSql & " entisediattuazioni.denominazione + ' (' + CONVERT(varchar, TVerifiche.identesedeattuazione) + ')' as denominazione,"
        strSql = strSql & " entisedi.indirizzo + ' ' + entisedi.civico + ' ' + entisedi.cap  as indirizzo , "
        strSql = strSql & " comuni.denominazione + ' (' + provincie.descrabb + ') ' as comune,'' as Irregolarità "
        strSql = strSql & " FROM TVerifiche "
        strSql = strSql & " INNER join entisediattuazioni  on entisediattuazioni.identesedeattuazione = TVerifiche.identesedeattuazione "
        strSql = strSql & " INNER join entisedi on entisedi.identesede = entisediattuazioni.identesede "
        strSql = strSql & " INNER join comuni on entisedi.idcomune = comuni.idcomune "
        strSql = strSql & " INNER join provincie on comuni.idprovincia = provincie.idprovincia "
        strSql = strSql & " where TVerifiche.idverifica  = " & Request.QueryString("idverifica") & " "
        DataSetEnteC = ClsServer.DataSetGenerico(strSql, Session("conn"))

        dgSediAttuazione.DataSource = DataSetEnteC
        dgSediAttuazione.DataBind()
    End Sub
    Private Sub CaricaEnteDipendenteSEGNALATAUNSC()
        'enti figlio
        Dim strSql As String
        Dim DataSetEnteC As DataSet

        strSql = " Select  distinct "
        strSql = strSql & " entisedi.idente as identefiglio, ENTI.denominazione + ' (' + CONVERT(varchar, ENTI.CodiceRegione ) + ')' as entefiglio "
        strSql = strSql & " FROM TVerifiche"
        strSql = strSql & " INNER join entisediattuazioni  on entisediattuazioni.identesedeattuazione = TVerifiche.identesedeattuazione  "
        strSql = strSql & " INNER join entisedi on entisedi.identesede = entisediattuazioni.identesede  "
        strSql = strSql & " INNER JOIN ENTI ON enti.idente=entisedi.idente"
        strSql = strSql & " where TVerifiche.idverifica = " & Request.QueryString("idverifica") & " and entisedi.idente  <> " & IdEnte & " "
        DataSetEnteC = ClsServer.DataSetGenerico(strSql, Session("conn"))

        dgEnteDipendente.DataSource = DataSetEnteC
        dgEnteDipendente.DataBind()
    End Sub
    Private Sub CaricaEnteDipendenti()
        'enti figlio
        Dim strSql As String
        Dim DataSetEnteC As DataSet

        strSql = " Select DISTINCT entefiglio,identefiglio "
        strSql = strSql & " from ver_vw_ricerca_verifiche "
        strSql = strSql & "where idverifica = " & Request.QueryString("idverifica") & " and identefiglio <> " & IdEnte & " "
        DataSetEnteC = ClsServer.DataSetGenerico(strSql, Session("conn"))

        dgEnteDipendente.DataSource = DataSetEnteC
        dgEnteDipendente.DataBind()
    End Sub

    Private Sub CaricaSedi()
        'sedi
        Dim strSql As String
        Dim DataSetEnteC As DataSet

        strSql = " Select DISTINCT DenEnteSede  + ' (' + CONVERT(varchar, vista.identesede) + ')' as DenEnteSede,vista.identesede,"
        strSql = strSql & " entisedi.indirizzo + ' ' + entisedi.civico + ' ' + entisedi.cap  as indirizzo , "
        strSql = strSql & " comuni.denominazione + ' (' + provincie.descrabb + ') ' as comune,"
        strSql = strSql & " prefissotelefono + ''+ telefono as telefono,"
        strSql = strSql & " prefissofax + '' + fax as fax"
        strSql = strSql & " from ver_vw_ricerca_verifiche as vista"
        strSql = strSql & " inner join entisedi on entisedi.identesede = vista.identesede"
        strSql = strSql & " inner join comuni on entisedi.idcomune = comuni.idcomune"
        strSql = strSql & " inner join provincie on comuni.idprovincia = provincie.idprovincia"
        strSql = strSql & " where idverifica = " & Request.QueryString("idverifica") & " "
        DataSetEnteC = ClsServer.DataSetGenerico(strSql, Session("conn"))

        dgSedi.DataSource = DataSetEnteC
        dgSedi.DataBind()
    End Sub
    Private Sub CaricaSediAttuazione()
        'sedi attuazione
        'modifica il 20/06/2011 
        'modifica query , sono state aggiunte le colonne indirizzo e comune della sede di attuazione
        Dim strSql As String
        Dim DataSetEnteC As DataSet

        strSql = " Select  distinct vista.identesedeattuazione, "
        strSql = strSql & " entisediattuazioni.denominazione + ' (' + CONVERT(varchar, vista.identesedeattuazione) + ')' as denominazione,"
        strSql = strSql & " entisedi.indirizzo + ' ' + entisedi.civico + ' ' + entisedi.cap  as indirizzo , "
        strSql = strSql & " comuni.denominazione + ' (' + provincie.descrabb + ') ' as comune,"
        strSql = strSql & " (Select isnull(count(esito),0) "
        strSql = strSql & "    from TVerificheEsiti   TVE"
        strSql = strSql & "   where esito = 2  and "
        strSql = strSql & "   TVE.IDVerificheAssociate = TVerificheAssociate.IDVerificheAssociate) as Irregolarità"
        strSql = strSql & " from ver_vw_ricerca_verifiche as vista  "
        strSql = strSql & " INNER JOIN  attivitàentisediattuazione ON vista.IDAttivitàEnteSedeAttuazione = attivitàentisediattuazione.IDAttivitàEnteSedeAttuazione  "
        strSql = strSql & " inner join entisediattuazioni  on entisediattuazioni.identesedeattuazione = attivitàentisediattuazione.identesedeattuazione "
        strSql = strSql & " inner join entisedi on entisedi.identesede = vista.identesede"
        strSql = strSql & " inner join comuni on entisedi.idcomune = comuni.idcomune"
        strSql = strSql & " inner join provincie on comuni.idprovincia = provincie.idprovincia"
        strSql = strSql & " inner join TVerificheAssociate  on TVerificheAssociate.IDVerifica =vista.IDVerifica  and vista.IDAttivitàEnteSedeAttuazione = tverificheassociate.IDAttivitàEnteSedeAttuazione"
        strSql = strSql & " where vista.idverifica = " & Request.QueryString("idverifica") & " "
        DataSetEnteC = ClsServer.DataSetGenerico(strSql, Session("conn"))

        dgSediAttuazione.DataSource = DataSetEnteC
        dgSediAttuazione.DataBind()
    End Sub
    Private Sub CaricaSediProgetto()
        Dim strSql As String
        Dim DataSetSediP As DataSet


        strSql = " SELECT distinct TVerificheAssociate.IDAttivitàEnteSedeAttuazione, attivitàentisediattuazione.IDEnteSedeAttuazione,"
        strSql = strSql & " entisediattuazioni.Denominazione  + ' (' + CONVERT(varchar, entisediattuazioni.identesedeattuazione) + ')'  AS DenomSede,"
        strSql = strSql & " Attività.Titolo + ' (' +  Attività.codiceEnte + ')' as progetto ,"
        'strSql = strSql & " (SELECT COUNT(DISTINCT ae.IDEntità) "
        'strSql = strSql & "    FROM attività AS a INNER JOIN "
        'strSql = strSql & "    attivitàentisediattuazione AS aesa ON a.IDAttività = aesa.IDAttività INNER JOIN"
        'strSql = strSql & "    attivitàentità AS ae ON ae.IDAttivitàEnteSedeAttuazione = aesa.IDAttivitàEnteSedeAttuazione INNER JOIN"
        'strSql = strSql & "    entità AS e ON ae.IDEntità = e.IDEntità"
        'strSql = strSql & "    WHERE  e.IDStatoEntità = 3 AND ae.IDAttivitàEnteSedeAttuazione = attivitàentisediattuazione.IDAttivitàEnteSedeAttuazione) "
        'strSql = strSql & " AS NumeroVolontari,"

        strSql = strSql & " case attività.IdStatoAttività when  1 then "
        strSql = strSql & " (SELECT "
        strSql = strSql & " ISNULL(COUNT (DISTINCT ae.IDEntità),0) "
        strSql = strSql & " FROM attività AS a INNER JOIN    "
        strSql = strSql & " attivitàentisediattuazione AS aesa ON a.IDAttività = aesa.IDAttività "
        strSql = strSql & " INNER JOIN    attivitàentità AS ae ON ae.IDAttivitàEnteSedeAttuazione = aesa.IDAttivitàEnteSedeAttuazione "
        strSql = strSql & " INNER JOIN    entità AS e ON ae.IDEntità = e.IDEntità   "
        strSql = strSql & " WHERE(e.IDStatoEntità = 3)"
        strSql = strSql & " AND ae.IDAttivitàEnteSedeAttuazione = attivitàentisediattuazione.IDAttivitàEnteSedeAttuazione) "
        strSql = strSql & " when 2 then"
        strSql = strSql & " (SELECT ISNULL(COUNT (DISTINCT ae.IDEntità),0) "
        strSql = strSql & " FROM attività AS a INNER JOIN    "
        strSql = strSql & " attivitàentisediattuazione AS aesa ON a.IDAttività = aesa.IDAttività "
        strSql = strSql & " INNER JOIN    attivitàentità AS ae ON ae.IDAttivitàEnteSedeAttuazione = aesa.IDAttivitàEnteSedeAttuazione "
        strSql = strSql & " INNER JOIN    entità AS e ON ae.IDEntità = e.IDEntità   "
        strSql = strSql & " inner join causali as c on c.idcausale = e.idcausalechiusura"
        strSql = strSql & " WHERE(e.IDStatoEntità = 6 OR (e.IDStatoEntità = 5 And c.tipo = 3))"
        strSql = strSql & " AND ae.IDAttivitàEnteSedeAttuazione = attivitàentisediattuazione.IDAttivitàEnteSedeAttuazione )"
        'strSql = strSql & " UNION "
        'strSql = strSql & " SELECT ISNULL(COUNT (DISTINCT ae.IDEntità),0) "
        'strSql = strSql & " FROM attività AS a INNER JOIN    "
        'strSql = strSql & " attivitàentisediattuazione AS aesa ON a.IDAttività = aesa.IDAttività "
        'strSql = strSql & " INNER JOIN    attivitàentità AS ae ON ae.IDAttivitàEnteSedeAttuazione = aesa.IDAttivitàEnteSedeAttuazione "
        'strSql = strSql & " INNER JOIN    entità AS e ON ae.IDEntità = e.IDEntità  "
        'strSql = strSql & " inner join causali as c on c.idcausale = e.idcausalechiusura "
        'strSql = strSql & " WHERE(e.IDStatoEntità = 5 And c.tipo = 3)"
        'strSql = strSql & " AND ae.IDAttivitàEnteSedeAttuazione = attivitàentisediattuazione.IDAttivitàEnteSedeAttuazione) "
        'strSql = strSql & " group by e.IDStatoEntità"
        'strSql = strSql & " having  COUNT(DISTINCT ae.IDEntità)>0 ) 
        strSql = strSql & " end AS NumeroVolontari, "
        strSql = strSql & " Attività.idAttività,(Select isnull(count(esito),0) from TVerificheEsiti "
        strSql = strSql & "  inner join TVerificheAssociate TVA on  TVA.IDVerificheAssociate = TVerificheEsiti.IDVerificheAssociate  "
        strSql = strSql & "    where esito =2  And TVerificheAssociate.IDAttivitàEnteSedeAttuazione = TVA.IDAttivitàEnteSedeAttuazione "
        strSql = strSql & "    and TVerificheAssociate.IDVerifica = " & Request.QueryString("idverifica") & " ) as Irregolarità "
        strSql = strSql & " FROM TVerificheAssociate "
        strSql = strSql & " INNER JOIN  attivitàentisediattuazione ON TVerificheAssociate.IDAttivitàEnteSedeAttuazione = attivitàentisediattuazione.IDAttivitàEnteSedeAttuazione "
        strSql = strSql & " INNER JOIN entisediattuazioni ON attivitàentisediattuazione.IDEnteSedeAttuazione = entisediattuazioni.IDEnteSedeAttuazione "
        strSql = strSql & " INNER JOIN Attività on attivitàentisediattuazione.idAttività = Attività.idAttività"
        strSql = strSql & " WHERE TVerificheAssociate.IDVerifica = " & Request.QueryString("idverifica") & ""


        'strSql = " SELECT distinct "
        'strSql = strSql & " <input onclick=javascript:ControllaCheck(''' + convert(varchar,Irregolarita) + ''') type=checkbox value=1 name=chkIrregolarita' + convert(varchar,Irregolarita) + ' id=chkIrregolarita' + convert(varchar,Irregolarita) + ' checked>' AS Irregolarita, "
        'strSql = strSql & " TVerificheAssociate.IDAttivitàEnteSedeAttuazione(, attivitàentisediattuazione.IDEnteSedeAttuazione, "")"
        'strSql = strSql & " entisediattuazioni.Denominazione  + ' (' + CONVERT(varchar, entisediattuazioni.identesedeattuazione) + ')'  AS DenomSede,"
        'strSql = strSql & " Attività.Titolo + ' (' +  Attività.codiceEnte + ')' as progetto ,"
        'strSql = strSql & " (SELECT COUNT(DISTINCT ae.IDEntità) "
        'strSql = strSql & "    FROM attività AS a INNER JOIN "
        'strSql = strSql & "    attivitàentisediattuazione AS aesa ON a.IDAttività = aesa.IDAttività INNER JOIN"
        'strSql = strSql & "    attivitàentità AS ae ON ae.IDAttivitàEnteSedeAttuazione = aesa.IDAttivitàEnteSedeAttuazione INNER JOIN"
        'strSql = strSql & "    entità AS e ON ae.IDEntità = e.IDEntità"
        'strSql = strSql & "    WHERE  e.IDStatoEntità = 3 AND ae.IDAttivitàEnteSedeAttuazione = attivitàentisediattuazione.IDAttivitàEnteSedeAttuazione) "
        'strSql = strSql & " AS NumeroVolontari,Attività.idAttività,"
        'strSql = strSql & " (Select isnull(count(esito),0) from TVerificheEsiti "
        'strSql = strSql & "  inner join TVerificheAssociate TVA on  TVA.IDVerificheAssociate = TVerificheEsiti.IDVerificheAssociate  "
        'strSql = strSql & "    where esito =2  And TVerificheAssociate.IDAttivitàEnteSedeAttuazione = TVA.IDAttivitàEnteSedeAttuazione "
        'strSql = strSql & "    and TVerificheAssociate.IDVerifica = " & Request.QueryString("idverifica") & " ) as Irregolarità "
        'strSql = strSql & " FROM TVerificheAssociate "
        'strSql = strSql & " INNER JOIN  attivitàentisediattuazione ON TVerificheAssociate.IDAttivitàEnteSedeAttuazione = attivitàentisediattuazione.IDAttivitàEnteSedeAttuazione "
        'strSql = strSql & " INNER JOIN entisediattuazioni ON attivitàentisediattuazione.IDEnteSedeAttuazione = entisediattuazioni.IDEnteSedeAttuazione "
        'strSql = strSql & " INNER JOIN Attività on attivitàentisediattuazione.idAttività = Attività.idAttività"
        'strSql = strSql & " WHERE TVerificheAssociate.IDVerifica = " & Request.QueryString("idverifica") & ""



        DataSetSediP = ClsServer.DataSetGenerico(strSql, Session("conn"))
        dgSediProgetto.DataSource = DataSetSediP
        dgSediProgetto.DataBind()
    End Sub
    Private Function MakeParentTable(ByVal strquery As String) As DataSet
        '***Generata da Gianluigi Paesani in data:05/07/04
        '***Inizializzo e carico datatable 
        Dim dtrgenerico As Data.SqlClient.SqlDataReader
        ' Create a new DataTable.
        Dim myDataTable As DataTable = New DataTable
        ' Declare variables for DataColumn and DataRow objects.
        Dim myDataColumn As DataColumn
        Dim myDataRow As DataRow
        ' Create new DataColumn, set DataType, ColumnName and add to DataTable.    
        myDataColumn = New DataColumn
        myDataColumn.DataType = System.Type.GetType("System.Int64")
        myDataColumn.ColumnName = "id"
        myDataColumn.Caption = "id"
        myDataColumn.ReadOnly = True
        myDataColumn.Unique = True
        ' Add the Column to the DataColumnCollection.
        myDataTable.Columns.Add(myDataColumn)
        ' Create second column.
        myDataColumn = New DataColumn
        myDataColumn.DataType = System.Type.GetType("System.String")
        myDataColumn.ColumnName = "ParentItem"
        myDataColumn.AutoIncrement = False
        myDataColumn.Caption = "ParentItem"
        myDataColumn.ReadOnly = False
        myDataColumn.Unique = False
        ' Add the column to the table.
        myDataTable.Columns.Add(myDataColumn)
        ' Make the ID column the primary key column. da verificare?????????
        'Dim PrimaryKeyColumns(0) As DataColumn
        'PrimaryKeyColumns(0) = myDataTable.Columns("id"))
        'myDataTable.PrimaryKey = PrimaryKeyColumns)
        dtrgenerico = ClsServer.CreaDatareader(strquery, Session("conn"))
        'Instantiate the DataSet variable.
        'mydataset = New DataSet
        ' Add the new DataTable to the DataSet.
        'mydataset.Tables.Add(myDataTable)
        myDataRow = myDataTable.NewRow()
        myDataRow("id") = 0
        myDataRow("ParentItem") = ""
        myDataTable.Rows.Add(myDataRow)
        Do While dtrgenerico.Read
            myDataRow = myDataTable.NewRow()
            myDataRow("id") = dtrgenerico.GetValue(0)
            myDataRow("ParentItem") = dtrgenerico.GetValue(1)
            myDataTable.Rows.Add(myDataRow)
        Loop

        dtrgenerico.Close()
        dtrgenerico = Nothing

        MakeParentTable = New DataSet
        MakeParentTable.Tables.Add(myDataTable)
    End Function

    Private Sub cmdChiudi_Click(sender As Object, e As EventArgs) Handles cmdChiudi.Click
        Dim Item As DataGridItem
        lblmessaggio.Text = ""
        'Dim ContaSanzioneAnnullata As Integer

        'For Each Item In dgSanzione.Items
        '    'controllo lo stato della sanzione
        '    Select Case Item.Cells(10).Text
        '        Case "&nbsp;", "Sanzione Valida"
        '        Case "Sanzione Annullata"
        '            ContaSanzioneAnnullata = ContaSanzioneAnnullata + 1
        '        Case "Sanzione Ripristinata"
        '    End Select
        'Next
        ''verifico se la variabile che conta le sanzioni annullate è uguale alla count della griglia sanzioni
        'If dgSanzione.Items.Count <> 0 Then
        '    If ContaSanzioneAnnullata = dgSanzione.Items.Count Then
        '        'imposto lo stato della verifica da " SANZIONATA " a " CHIUSA CONTESTATA "
        '        UpdateTVerifiche(Request.QueryString("idverifica"), 14) 'imposto a chiusa contestata
        '    Else
        '        If lblStatoVerifica.Text = "Chiusa Contestata" Then
        '            UpdateTVerifiche(Request.QueryString("idverifica"), 11) 'imposto a sanzionata
        '        End If
        '    End If
        'End If



        Session("dtSanzione") = Nothing
        If Request.QueryString("Segnalata") = Nothing Then
            Response.Redirect("verificarequisiti.aspx?Sanzione=SI&NumProtEsecSanzione= " & Trim(Request.QueryString("NumProtEsecSanzione")) & "&DataProtEsecSanzione= " & Trim(Request.QueryString("DataProtEsecSanzione")) & "&idprogrammazione=" & Request.QueryString("idprogrammazione") & "&VengoDa=" & Session("VengoDa") & "&IdEnte=" & Request.QueryString("Idente") & "&IdVerifica=" & Request.QueryString("IdVerifica") & "")
        Else
            '&NumProtTrasmSanzioneDG= " & TxtNumeroProtocolloTrasmissioneSanzione.Text & " &DataProtTrasmSanzioneDG=
            Response.Redirect("ver_VerificaSegnalataUNSC.aspx?StatoSegnalata=Modifica&Sanzione=SI&NumProtTrasmSanzioneDG= " & Trim(Request.QueryString("NumProtTrasmSanzioneDG")) & "&DataProtTrasmSanzioneDG= " & Trim(Request.QueryString("DataProtTrasmSanzioneDG")) & "&NumProtEsecSanzione= " & Trim(Request.QueryString("NumProtEsecSanzione")) & "&DataProtEsecSanzione= " & Trim(Request.QueryString("DataProtEsecSanzione")) & "&idprogrammazione=" & Request.QueryString("idprogrammazione") & "&VengoDa=" & Session("VengoDa") & "&IdEnte=" & Request.QueryString("Idente") & "&IdVerifica=" & Request.QueryString("IdVerifica") & "")
        End If
    End Sub

    Sub CaricaSanzione()
        'Aggiunto il 14/01/2008 da Simona Cordella
        'carico la griglia se sto richiamamdno già una verifica sanzionata
        Dim Item As DataGridItem
        Dim StrSql As String
        Dim dtSanzione As New DataTable
        Dim drSanzione As DataRow
        Dim DtrSanzione As SqlClient.SqlDataReader

        If Session("dtSanzione") Is Nothing Then 'creo la datatable
            Session("dtSanzione") = New DataTable
            Session("dtSanzione").Columns.Add(New DataColumn("Sanzione", GetType(String)))
            Session("dtSanzione").Columns.Add(New DataColumn("Tipo", GetType(String)))
            Session("dtSanzione").Columns.Add(New DataColumn("Soggetto", GetType(String)))
            Session("dtSanzione").Columns.Add(New DataColumn("IdEnte", GetType(String)))
            Session("dtSanzione").Columns.Add(New DataColumn("IdAttivita", GetType(String)))
            Session("dtSanzione").Columns.Add(New DataColumn("IdEnteSede", GetType(String)))
            Session("dtSanzione").Columns.Add(New DataColumn("IdEnteSedeAttuazione", GetType(String)))
            Session("dtSanzione").Columns.Add(New DataColumn("IDAttivitàEnteSedeAttuazione", GetType(String)))
            Session("dtSanzione").Columns.Add(New DataColumn("IdSanzione", GetType(String)))
            Session("dtSanzione").Columns.Add(New DataColumn("Stato", GetType(String)))
        End If
        If Not DtrSanzione Is Nothing Then
            DtrSanzione.Close()
            DtrSanzione = Nothing
        End If
        'PROGETTO

        StrSql = " SELECT  distinct attività.CodiceEnte, (attività.Titolo + ' (' + attività.CodiceEnte + ')') as Titolo, " & _
                " TVerificheSanzioniProgetto.idattività, " & _
                " TVerificheSanzioniProgetto.IDSanzioneProgetto, " & _
                " TVerificheSanzioniProgetto.IDSanzione,TVerificheSanzioni.Descrizione as Sanzione,  " & _
                " TVerificheSanzioniProgetto.DataProtocolloEsecuzione, TVerificheSanzioniProgetto.NProtocolloEsecuzione, " & _
                " TVerificheSanzioniProgetto.CodiceFascicolo, TVerificheSanzioniProgetto.IDFascicolo, " & _
                " TVerificheSanzioniProgetto.DescrFascicolo, TVerificheSanzioniProgetto.Note,   " & _
                " (CASE isnull(TVerificheSanzioniProgetto.StatoSanzione,0) " & _
                " WHEN 0 THEN 'Sanzione Valida' " & _
                " WHEN 1 THEN 'Sanzione Annullata' " & _
                " WHEN 2 THEN 'Sanzione Ripristinata' END) AS StatoSanzione " & _
                " FROM TVerificheSanzioniProgetto " & _
                " LEFT JOIN TVerificheSanzioni on TVerificheSanzioniProgetto.IDSanzione = TVerificheSanzioni.IDSanzione  " & _
                " inner join attività on attività.idattività = TVerificheSanzioniProgetto.idattività " & _
                " WHERE TVerificheSanzioniProgetto.IDVerifica = " & Request.QueryString("idverifica") & ""


        DtrSanzione = ClsServer.CreaDatareader(StrSql, Session("Conn"))
        '"Sanzione" "Tipo" "Soggetto" "IdEnte" "IdAttivita" "IdEnteSede" "IdEnteSedeAttuazione" "IDAttivitàEnteSedeAttuazione" "IdSanzione"
        If DtrSanzione.HasRows = True Then
            Do While DtrSanzione.Read()
                drSanzione = Session("dtSanzione").NewRow()
                drSanzione(0) = DtrSanzione("Sanzione") '"Sanzione"
                drSanzione(1) = "Progetto" '"Tipo"
                drSanzione(2) = DtrSanzione("Titolo") '"Soggetto"
                drSanzione(3) = "" '"IdEnte"
                drSanzione(4) = DtrSanzione("idattività") '"IdAttivita"
                drSanzione(5) = "" '"IdEnteSede"
                drSanzione(6) = "" '"IdEnteSedeAttuazione"
                drSanzione(7) = "" '"IDAttivitàEnteSedeAttuazione"
                drSanzione(8) = DtrSanzione("IDSanzione") '"IdSanzione"
                drSanzione(9) = DtrSanzione("StatoSanzione")
                Session("dtSanzione").Rows.Add(drSanzione)
            Loop
        End If
        If Not DtrSanzione Is Nothing Then
            DtrSanzione.Close()
            DtrSanzione = Nothing
        End If
        'ENTE
        StrSql = " SELECT  DISTINCT " & _
                " VER_VW_RICERCA_VERIFICHE.Denominazione + '(' + VER_VW_RICERCA_VERIFICHE.CodiceEnte + ')' AS Denominazione, " & _
                "  TVerificheSanzioniEnte.IDEnte, TVerificheSanzioniEnte.IDSanzioneEnte, TVerificheSanzioniEnte.IDSanzione,TVerificheSanzioni.Descrizione as Sanzione, " & _
                " TVerificheSanzioniEnte.DataProtocolloEsecuzione, TVerificheSanzioniEnte.NProtocolloEsecuzione, TVerificheSanzioniEnte.CodiceFascicolo, TVerificheSanzioniEnte.IDFascicolo, TVerificheSanzioniEnte.DescrFascicolo, TVerificheSanzioniEnte.Note,  " & _
                " (CASE isnull(TVerificheSanzioniEnte.StatoSanzione,0) " & _
                " WHEN 0 THEN 'Sanzione Valida' " & _
                " WHEN 1 THEN 'Sanzione Annullata' " & _
                " WHEN 2 THEN 'Sanzione Ripristinata' END) AS StatoSanzione " & _
                " FROM TVerificheSanzioniEnte " & _
                " INNER JOIN TVerificheSanzioni on TVerificheSanzioniEnte.IDSanzione = TVerificheSanzioni.IDSanzione " & _
                " INNER JOIN VER_VW_RICERCA_VERIFICHE ON VER_VW_RICERCA_VERIFICHE.IDVerifica = TVerificheSanzioniEnte.IDVerifica AND " & _
                " VER_VW_RICERCA_VERIFICHE.IDEnte = TVerificheSanzioniEnte.IDEnte " & _
                " WHERE TVerificheSanzioniEnte.IDVerifica = " & Request.QueryString("idverifica") & ""

        DtrSanzione = ClsServer.CreaDatareader(StrSql, Session("Conn"))
        '"Sanzione" "Tipo" "Soggetto" "IdEnte" "IdAttivita" "IdEnteSede" "IdEnteSedeAttuazione" "IDAttivitàEnteSedeAttuazione" "IdSanzione"
        If DtrSanzione.HasRows = True Then
            Do While DtrSanzione.Read()
                drSanzione = Session("dtSanzione").NewRow()
                drSanzione(0) = DtrSanzione("Sanzione") '"Sanzione"
                drSanzione(1) = "Ente Capofila" '"Tipo"
                drSanzione(2) = DtrSanzione("Denominazione") '"Soggetto"
                drSanzione(3) = DtrSanzione("IdEnte") '"IdEnte"
                drSanzione(4) = "" '"IdAttivita"
                drSanzione(5) = "" '"IdEnteSede"
                drSanzione(6) = "" '"IdEnteSedeAttuazione"
                drSanzione(7) = "" '"IDAttivitàEnteSedeAttuazione"
                drSanzione(8) = DtrSanzione("IDSanzione") '"IdSanzione"
                drSanzione(9) = DtrSanzione("StatoSanzione")
                Session("dtSanzione").Rows.Add(drSanzione)
            Loop
        End If
        If Not DtrSanzione Is Nothing Then
            DtrSanzione.Close()
            DtrSanzione = Nothing
        End If
        'ENTI FIGLIO
        StrSql = " SELECT  DISTINCT " & _
                 " VER_VW_RICERCA_VERIFICHE.entefiglio,VER_VW_RICERCA_VERIFICHE.identefiglio, " & _
                 " TVerificheSanzioniEnte.IDEnte, TVerificheSanzioniEnte.IDSanzioneEnte, TVerificheSanzioniEnte.IDSanzione,TVerificheSanzioni.Descrizione as Sanzione, " & _
                 " TVerificheSanzioniEnte.DataProtocolloEsecuzione, TVerificheSanzioniEnte.NProtocolloEsecuzione, TVerificheSanzioniEnte.CodiceFascicolo, TVerificheSanzioniEnte.IDFascicolo, TVerificheSanzioniEnte.DescrFascicolo, TVerificheSanzioniEnte.Note,  " & _
                 " (CASE isnull(TVerificheSanzioniEnte.StatoSanzione,0) " & _
                 " WHEN 0 THEN 'Sanzione Valida' " & _
                 " WHEN 1 THEN 'Sanzione Annullata' " & _
                 " WHEN 2 THEN 'Sanzione Ripristinata' END) AS StatoSanzione " & _
                 " FROM TVerificheSanzioniEnte " & _
                 " INNER JOIN TVerificheSanzioni on TVerificheSanzioniEnte.IDSanzione = TVerificheSanzioni.IDSanzione " & _
                 " INNER JOIN VER_VW_RICERCA_VERIFICHE ON VER_VW_RICERCA_VERIFICHE.IDVerifica = TVerificheSanzioniEnte.IDVerifica AND " & _
                 " VER_VW_RICERCA_VERIFICHE.IDEnteFiglio = TVerificheSanzioniEnte.IDEnte " & _
                 " WHERE TVerificheSanzioniEnte.IDVerifica = " & Request.QueryString("idverifica") & " AND VER_VW_RICERCA_VERIFICHE.IDEnteFiglio <> " & IdEnte & ""

        DtrSanzione = ClsServer.CreaDatareader(StrSql, Session("Conn"))
        '"Sanzione" "Tipo" "Soggetto" "IdEnte" "IdAttivita" "IdEnteSede" "IdEnteSedeAttuazione" "IDAttivitàEnteSedeAttuazione" "IdSanzione"

        If DtrSanzione.HasRows = True Then
            Do While DtrSanzione.Read()
                drSanzione = Session("dtSanzione").NewRow()
                drSanzione(0) = DtrSanzione("Sanzione") '"Sanzione"
                drSanzione(1) = "Ente Dipendente" '"Tipo"
                drSanzione(2) = DtrSanzione("entefiglio") '"Soggetto"
                drSanzione(3) = DtrSanzione("identefiglio") '"IdEnte"
                drSanzione(4) = "" '"IdAttivita"
                drSanzione(5) = "" '"IdEnteSede"
                drSanzione(6) = "" '"IdEnteSedeAttuazione"
                drSanzione(7) = "" '"IDAttivitàEnteSedeAttuazione"
                drSanzione(8) = DtrSanzione("IDSanzione") '"IdSanzione"
                drSanzione(9) = DtrSanzione("StatoSanzione")
                Session("dtSanzione").Rows.Add(drSanzione)
            Loop
        End If
        If Not DtrSanzione Is Nothing Then
            DtrSanzione.Close()
            DtrSanzione = Nothing
        End If
        'SEDE
        StrSql = " Select DISTINCT DenEnteSede  + ' (' + CONVERT(varchar, vista.identesede) + ')' as DenEnteSede, " & _
              " vista.identesede,TVerificheSanzioniSede.IDSanzioneSede, TVerificheSanzioniSede.IDSanzione, " & _
              " TVerificheSanzioni.Descrizione as Sanzione,  TVerificheSanzioniSede.DataProtocolloEsecuzione, " & _
              " TVerificheSanzioniSede.NProtocolloEsecuzione , TVerificheSanzioniSede.CodiceFascicolo, " & _
              " TVerificheSanzioniSede.IDFascicolo, TVerificheSanzioniSede.DescrFascicolo, TVerificheSanzioniSede.Note, " & _
              " (CASE isnull(TVerificheSanzioniSede.StatoSanzione,0) " & _
              " WHEN 0 THEN 'Sanzione Valida' " & _
              " WHEN 1 THEN 'Sanzione Annullata' " & _
              " WHEN 2 THEN 'Sanzione Ripristinata' END) AS StatoSanzione " & _
              " FROM ver_vw_ricerca_verifiche as vista " & _
              " inner join TVerificheSanzioniSede on vista.identesede =TVerificheSanzioniSede.identesede " & _
              " INNER JOIN TVerificheSanzioni on TVerificheSanzioniSede.IDSanzione = TVerificheSanzioni.IDSanzione " & _
              " WHERE TVerificheSanzioniSede.idverifica = " & Request.QueryString("idverifica") & ""

        DtrSanzione = ClsServer.CreaDatareader(StrSql, Session("Conn"))
        '"Sanzione" "Tipo" "Soggetto" "IdEnte" "IdAttivita" "IdEnteSede" "IdEnteSedeAttuazione" "IDAttivitàEnteSedeAttuazione" "IdSanzione"
        If DtrSanzione.HasRows = True Then
            Do While DtrSanzione.Read()
                drSanzione = Session("dtSanzione").NewRow()
                drSanzione(0) = DtrSanzione("Sanzione") '"Sanzione"
                drSanzione(1) = "Sedi" '"Tipo"
                drSanzione(2) = DtrSanzione("DenEnteSede") '"Soggetto"
                drSanzione(3) = "" '"IdEnte"
                drSanzione(4) = "" '"IdAttivita"
                drSanzione(5) = DtrSanzione("identesede") '"IdEnteSede"
                drSanzione(6) = "" '"IdEnteSedeAttuazione"
                drSanzione(7) = "" '"IDAttivitàEnteSedeAttuazione"
                drSanzione(8) = DtrSanzione("IDSanzione") '"IdSanzione"
                drSanzione(9) = DtrSanzione("StatoSanzione")
                Session("dtSanzione").Rows.Add(drSanzione)
            Loop
        End If
        If Not DtrSanzione Is Nothing Then
            DtrSanzione.Close()
            DtrSanzione = Nothing
        End If
        'SEDE ATTUAZIONE
        StrSql = " Select DISTINCT vista.identesedeattuazione, " & _
                " entisediattuazioni.denominazione + ' (' + CONVERT(varchar, vista.identesedeattuazione) + ')' as denominazione, " & _
                " TVerificheSanzioniSedeAttuazione.IDSanzioneSedeAttuazione, " & _
                " TVerificheSanzioniSedeAttuazione.IDSanzione,TVerificheSanzioni.Descrizione as Sanzione,   " & _
                " TVerificheSanzioniSedeAttuazione.DataProtocolloEsecuzione, TVerificheSanzioniSedeAttuazione.NProtocolloEsecuzione, " & _
                " TVerificheSanzioniSedeAttuazione.CodiceFascicolo, TVerificheSanzioniSedeAttuazione.IDFascicolo, " & _
                " TVerificheSanzioniSedeAttuazione.DescrFascicolo, TVerificheSanzioniSedeAttuazione.Note,  " & _
                " (CASE isnull(TVerificheSanzioniSedeAttuazione.StatoSanzione,0) " & _
                " WHEN 0 THEN 'Sanzione Valida' " & _
                " WHEN 1 THEN 'Sanzione Annullata' " & _
                " WHEN 2 THEN 'Sanzione Ripristinata' END) AS StatoSanzione " & _
                " FROM ver_vw_ricerca_verifiche as vista   " & _
                " INNER JOIN TVerificheSanzioniSedeAttuazione on  vista.identesedeattuazione =TVerificheSanzioniSedeAttuazione.identesedeattuazione " & _
                " INNER JOIN entisediattuazioni  on entisediattuazioni.identesedeattuazione = vista.identesedeattuazione " & _
                " INNER JOIN TVerificheSanzioni on TVerificheSanzioniSedeAttuazione.IDSanzione = TVerificheSanzioni.IDSanzione " & _
                " WHERE TVerificheSanzioniSedeAttuazione.IDVerifica  = " & Request.QueryString("idverifica") & ""

        DtrSanzione = ClsServer.CreaDatareader(StrSql, Session("Conn"))
        '"Sanzione" "Tipo" "Soggetto" "IdEnte" "IdAttivita" "IdEnteSede" "IdEnteSedeAttuazione" "IDAttivitàEnteSedeAttuazione" "IdSanzione"
        If DtrSanzione.HasRows = True Then
            Do While DtrSanzione.Read()
                drSanzione = Session("dtSanzione").NewRow()
                drSanzione(0) = DtrSanzione("Sanzione") '"Sanzione"
                drSanzione(1) = "Sedi Attuazione" '"Tipo"
                drSanzione(2) = DtrSanzione("denominazione") '"Soggetto"
                drSanzione(3) = "" '"IdEnte"
                drSanzione(4) = "" '"IdAttivita"
                drSanzione(5) = "" '"IdEnteSede"
                drSanzione(6) = DtrSanzione("identesedeattuazione") '"IdEnteSedeAttuazione"
                drSanzione(7) = "" '"IDAttivitàEnteSedeAttuazione"
                drSanzione(8) = DtrSanzione("IDSanzione") '"IdSanzione"
                drSanzione(9) = DtrSanzione("StatoSanzione")
                Session("dtSanzione").Rows.Add(drSanzione)
            Loop
        End If
        If Not DtrSanzione Is Nothing Then
            DtrSanzione.Close()
            DtrSanzione = Nothing
        End If
        'SEDE PROGETTO

        StrSql = "SELECT DISTINCT TVerificheSanzioniSedeProgetto.IDAttivitàSedeAttuazione, " & _
                " attivitàentisediattuazione.IDEnteSedeAttuazione, " & _
                " entisediattuazioni.Denominazione  + ' (' + CONVERT(varchar, entisediattuazioni.identesedeattuazione) + ')'  AS DenomSede," & _
                " TVerificheSanzioniSedeProgetto.IDSanzione,TVerificheSanzioni.Descrizione as Sanzione,   " & _
                " Attività.Titolo + ' (' +  Attività.codiceEnte + ')' as progetto ,attività.idAttività,   " & _
                " (CASE isnull(TVerificheSanzioniSedeProgetto.StatoSanzione,0) " & _
                " WHEN 0 THEN 'Sanzione Valida' " & _
                " WHEN 1 THEN 'Sanzione Annullata' " & _
                " WHEN 2 THEN 'Sanzione Ripristinata' END) AS StatoSanzione " & _
                " FROM TVerificheSanzioniSedeProgetto  " & _
                " INNER JOIN  attivitàentisediattuazione ON TVerificheSanzioniSedeProgetto.IDAttivitàSedeAttuazione = attivitàentisediattuazione.IDAttivitàEnteSedeAttuazione  " & _
                " INNER JOIN entisediattuazioni ON attivitàentisediattuazione.IDEnteSedeAttuazione = entisediattuazioni.IDEnteSedeAttuazione  " & _
                " INNER JOIN Attività on attivitàentisediattuazione.idAttività = Attività.idAttività  " & _
                " INNER JOIN TVerificheSanzioni on TVerificheSanzioniSedeProgetto.IDSanzione = TVerificheSanzioni.IDSanzione " & _
                " WHERE TVerificheSanzioniSedeProgetto.IDVerifica = " & Request.QueryString("idverifica") & ""

        DtrSanzione = ClsServer.CreaDatareader(StrSql, Session("Conn"))
        '"Sanzione" "Tipo" "Soggetto" "IdEnte" "IdAttivita" "IdEnteSede" "IdEnteSedeAttuazione" "IDAttivitàEnteSedeAttuazione" "IdSanzione"
        If DtrSanzione.HasRows = True Then
            Do While DtrSanzione.Read()
                drSanzione = Session("dtSanzione").NewRow()
                drSanzione(0) = DtrSanzione("Sanzione") '"Sanzione"
                drSanzione(1) = "Sedi di Progetto" '"Tipo"
                drSanzione(2) = DtrSanzione("DenomSede") '"Soggetto"
                drSanzione(3) = "" '"IdEnte"
                drSanzione(4) = "" '"IdAttivita"
                drSanzione(5) = "" '"IdEnteSede"
                drSanzione(6) = "" '"IdEnteSedeAttuazione"
                drSanzione(7) = DtrSanzione("IDAttivitàSedeAttuazione") '"IDAttivitàEnteSedeAttuazione"
                drSanzione(8) = DtrSanzione("IDSanzione") '"IdSanzione"
                drSanzione(9) = DtrSanzione("StatoSanzione")
                Session("dtSanzione").Rows.Add(drSanzione)
            Loop
        End If
        dgSanzione.DataSource = Session("dtSanzione")
        dgSanzione.DataBind()
        dgSanzione.SelectedIndex = -1

        If dgSanzione.Items.Count <> 0 Then
            StampaElencoSanzione()

        Else

        End If
        '''CONTROLLO LO STATO DELLA VERIFICA
        ''If lblStatoVerifica.Text = "Contestata" Then
        ''    'dgSanzione.Columns(3).Visible = True 'delete record
        ''    dgSanzione.Columns(10).Visible = False 'colonna stato
        ''    dgSanzione.Columns(11).Visible = False 'colonna annullo sanzione
        ''Else
        ''    'dgSanzione.Columns(3).Visible = False 'delete record
        ''    dgSanzione.Columns(10).Visible = True 'colonna stato
        ''    dgSanzione.Columns(11).Visible = True 'colonna annullo sanzione
        ''End If
        If Not DtrSanzione Is Nothing Then
            DtrSanzione.Close()
            DtrSanzione = Nothing
        End If
    End Sub
    Private Sub cmdApplica_Click(sender As Object, e As EventArgs) Handles cmdApplica.Click
        '****** CREATA IL 11/01/2008 DA SIMONA CORDELLA *** 
        '**** CARICO UN DATATABLE CON TUTTI I VALORI CHE SONO STATI CHECCATI PER OGNI GRIGLIA E RIPORTO IL RISULTATO NELLA GRIGLIA SANZIONE ***
        Dim ContaCheck As Integer = 0
        Dim Item As DataGridItem
        Dim chkValore As CheckBox
        Dim dtSanzione As New DataTable
        Dim drSanzione As DataRow
        Dim bytEnteCapofila As Byte = 0
        Dim IdEnteCapofila As Integer = 0
        Dim IdAttività As Integer
        Dim strsql As String
        Dim dtrProgetti As SqlClient.SqlDataReader
        lblmessaggio.Text = ""




        If (ddlSanzione.SelectedValue = "0") Then

            lblmessaggio.Text = "E' necessario indicare la sanzione."

            Exit Sub
        End If






        ControlloIrregolarità()
        If hddControllo.Value = 0 Then

        ElseIf hddControllo.Value = 1 Then
            Exit Sub
        Else
            hddControllo.Value = 0
            'ControlloIrregolarità( 4, 2)
            'creo il datatable
            If Session("dtSanzione") Is Nothing Then 'creo la datatable
                Session("dtSanzione") = New DataTable
                Session("dtSanzione").Columns.Add(New DataColumn("Sanzione", GetType(String)))
                Session("dtSanzione").Columns.Add(New DataColumn("Tipo", GetType(String)))
                Session("dtSanzione").Columns.Add(New DataColumn("Soggetto", GetType(String)))
                Session("dtSanzione").Columns.Add(New DataColumn("IdEnte", GetType(String)))
                Session("dtSanzione").Columns.Add(New DataColumn("IdAttivita", GetType(String)))
                Session("dtSanzione").Columns.Add(New DataColumn("IdEnteSede", GetType(String)))
                Session("dtSanzione").Columns.Add(New DataColumn("IdEnteSedeAttuazione", GetType(String)))
                Session("dtSanzione").Columns.Add(New DataColumn("IDAttivitàEnteSedeAttuazione", GetType(String)))
                Session("dtSanzione").Columns.Add(New DataColumn("IdSanzione", GetType(String)))
                Session("dtSanzione").Columns.Add(New DataColumn("Stato", GetType(String)))
                'Session("dtRigheVuote").Columns.Add()
            End If

            ' salvo i data in un datatable e carico la griglia della sanzione
            'GRIGLIA PROGETTO
            '"Sanzione" "Tipo" "Soggetto" "IdEnte" "IdAttivita" "IdEnteSede" "IdEnteSedeAttuazione" "IDAttivitàEnteSedeAttuazione" "IdSanzione"
            For Each Item In dgProgetti.Items
                chkValore = Item.FindControl("chkProg")
                If chkValore.Checked = True Then
                    ContaCheck = ContaCheck + 1
                    If ControlloDuplicati(ddlSanzione.SelectedValue, Item.Cells(3).Text, 4, Item.Cells(2).Text) = False Then
                        drSanzione = Session("dtSanzione").NewRow()
                        drSanzione(0) = "" & ddlSanzione.SelectedItem.Text '"Sanzione"
                        drSanzione(1) = "Progetto" '"Tipo"
                        drSanzione(2) = Item.Cells(2).Text '"Soggetto"
                        drSanzione(3) = "" '"IdEnte"
                        drSanzione(4) = Item.Cells(3).Text '"IdAttivita"
                        drSanzione(5) = "" '"IdEnteSede"
                        drSanzione(6) = "" '"IdEnteSedeAttuazione"
                        drSanzione(7) = "" '"IDAttivitàEnteSedeAttuazione"
                        drSanzione(8) = ddlSanzione.SelectedValue '"IdSanzione"
                        drSanzione(9) = ""
                        Session("dtSanzione").Rows.Add(drSanzione)
                    End If
                End If
            Next
            If ContaCheck = 0 Then 'controllo se sono stati ceccati i valori nella lista
                lblmessaggio.Text = "Non è stato selezionato nessun valore a cui applicare la sanzione"

            Else

                dgSanzione.DataSource = Session("dtSanzione")
                dgSanzione.DataBind()
                dgSanzione.SelectedIndex = -1
                StampaElencoSanzione()
            End If
            'GRLIGLIA ENTE CAPOFILA

            '"Sanzione" "Tipo" "Soggetto" "IdEnte" "IdAttivita" "IdEnteSede" "IdEnteSedeAttuazione" "IDAttivitàEnteSedeAttuazione" "IdSanzione"
            For Each Item In dgEnteCapofila.Items
                chkValore = Item.FindControl("chkEnteCap")
                If chkValore.Checked = True Then
                    IdEnteCapofila = Item.Cells(2).Text
                    ContaCheck = ContaCheck + 1
                    If ControlloDuplicati(ddlSanzione.SelectedValue, Item.Cells(2).Text, 5, Item.Cells(1).Text) = False Then
                        drSanzione = Session("dtSanzione").NewRow()
                        drSanzione(0) = "" & ddlSanzione.SelectedItem.Text
                        drSanzione(1) = "Ente Capofila"
                        drSanzione(2) = Item.Cells(1).Text
                        drSanzione(3) = IdEnteCapofila
                        drSanzione(4) = ""
                        drSanzione(5) = ""
                        drSanzione(6) = ""
                        drSanzione(7) = ""
                        drSanzione(8) = ddlSanzione.SelectedValue
                        drSanzione(9) = ""
                        Session("dtSanzione").Rows.Add(drSanzione)
                        bytEnteCapofila = 1
                    End If
                End If
            Next

            'agg. il 24/06/2008 da simona cordella
            'controllo se sto applicando una sanzione di Cancellazione dall'albo per l'ente capofila, applica a tutti i progetti la revoca(in automatico)
            If bytEnteCapofila = 1 Then
                'ddlSanzione.SelectedValue = 2 'revoca del progetto
                If ddlSanzione.SelectedValue = 4 Or ddlSanzione.SelectedValue = 5 Then 'id cancellazione albo nazionale e regionale
                    For Each Item In dgProgetti.Items
                        'ContaCheck = ContaCheck + 1
                        IdAttività = Item.Cells(3).Text
                        If ControlloDuplicati(2, IdAttività, 4, Item.Cells(2).Text) = False Then
                            drSanzione = Session("dtSanzione").NewRow()
                            drSanzione(0) = "" & "Revoca Progetto"  '"Sanzione"
                            drSanzione(1) = "Progetto" '"Tipo"
                            drSanzione(2) = Item.Cells(2).Text '"Soggetto"
                            drSanzione(3) = "" '"IdEnte"
                            drSanzione(4) = IdAttività '"IdAttivita"
                            drSanzione(5) = "" '"IdEnteSede"
                            drSanzione(6) = "" '"IdEnteSedeAttuazione"
                            drSanzione(7) = "" '"IDAttivitàEnteSedeAttuazione"
                            drSanzione(8) = 2 '"IdSanzione"
                            drSanzione(9) = ""
                            Session("dtSanzione").Rows.Add(drSanzione)
                        End If
                    Next


                    strsql = " Select IDAttività, (attività.Titolo + ' (' + attività.CodiceEnte + ')') as Titolo " & _
                             " FROM attività " & _
                             " WHERE  IDEntePresentante = " & IdEnteCapofila & " AND IDStatoAttività = 1 " & _
                             " and (getdate() BETWEEN DataInizioAttività AND DataFineAttività) and idattività <>" & IdAttività
                    If Not dtrProgetti Is Nothing Then
                        dtrProgetti.Close()
                        dtrProgetti = Nothing
                    End If
                    dtrProgetti = ClsServer.CreaDatareader(strsql, Session("conn"))
                    If dtrProgetti.HasRows = True Then
                        Do While dtrProgetti.Read()
                            If ControlloDuplicati(2, dtrProgetti("IDAttività"), 4, dtrProgetti("Titolo")) = False Then
                                drSanzione = Session("dtSanzione").NewRow()
                                drSanzione(0) = "" & "Revoca Progetto"
                                'ddlSanzione.SelectedItem.Text() '"Sanzione"
                                drSanzione(1) = "Progetto" '"Tipo"
                                drSanzione(2) = "" & dtrProgetti("Titolo") '"Soggetto"
                                drSanzione(3) = "" '"IdEnte"
                                drSanzione(4) = "" & dtrProgetti("IDAttività") '"IdAttivita"
                                drSanzione(5) = "" '"IdEnteSede"
                                drSanzione(6) = "" '"IdEnteSedeAttuazione"
                                drSanzione(7) = "" '"IDAttivitàEnteSedeAttuazione"
                                drSanzione(8) = 2 '"IdSanzione"
                                drSanzione(9) = ""
                                Session("dtSanzione").Rows.Add(drSanzione)
                            End If
                        Loop
                    End If

                    If Not dtrProgetti Is Nothing Then
                        dtrProgetti.Close()
                        dtrProgetti = Nothing
                    End If
                End If

            End If

            If ContaCheck = 0 Then 'controllo se sono stati ceccati i valori nella lista
                lblmessaggio.Text = "Non è stato selezionato nessun valore a cui applicare la sanzione"

            Else

                dgSanzione.DataSource = Session("dtSanzione")
                dgSanzione.DataBind()
                dgSanzione.SelectedIndex = -1
                StampaElencoSanzione()
            End If
            ' GRIGLIA ENTE DIPENDENTE
            '"Sanzione" "Tipo" "Soggetto" "IdEnte" "IdAttivita" "IdEnteSede" "IdEnteSedeAttuazione" "IDAttivitàEnteSedeAttuazione" "IdSanzione"
            For Each Item In dgEnteDipendente.Items
                chkValore = Item.FindControl("chkEnteDip")
                If chkValore.Checked = True Then
                    ContaCheck = ContaCheck + 1
                    If ControlloDuplicati(ddlSanzione.SelectedValue, Item.Cells(2).Text, 5, Item.Cells(1).Text) = False Then
                        drSanzione = Session("dtSanzione").NewRow()
                        drSanzione(0) = "" & ddlSanzione.SelectedItem.Text
                        drSanzione(1) = "Ente Dipendente"
                        drSanzione(2) = Item.Cells(1).Text
                        drSanzione(3) = Item.Cells(2).Text
                        drSanzione(4) = ""
                        drSanzione(5) = ""
                        drSanzione(6) = ""
                        drSanzione(7) = ""
                        drSanzione(8) = ddlSanzione.SelectedValue
                        drSanzione(9) = ""
                        Session("dtSanzione").Rows.Add(drSanzione)
                    End If
                End If
            Next
            If ContaCheck = 0 Then 'controllo se sono stati ceccati i valori nella lista
                lblmessaggio.Text = "Non è stato selezionato nessun valore a cui applicare la sanzione"

            Else

                dgSanzione.DataSource = Session("dtSanzione")
                dgSanzione.DataBind()
                dgSanzione.SelectedIndex = -1
                StampaElencoSanzione()
            End If
            'GRIGLIE SEDI
            '"Sanzione" "Tipo" "Soggetto" "IdEnte" "IdAttivita" "IdEnteSede" "IdEnteSedeAttuazione" "IDAttivitàEnteSedeAttuazione" "IdSanzione"
            For Each Item In dgSedi.Items
                chkValore = Item.FindControl("chkSedi")
                If chkValore.Checked = True Then
                    ContaCheck = ContaCheck + 1
                    If ControlloDuplicati(ddlSanzione.SelectedValue, Item.Cells(1).Text, 6, Item.Cells(2).Text) = False Then
                        drSanzione = Session("dtSanzione").NewRow()
                        drSanzione(0) = "" & ddlSanzione.SelectedItem.Text
                        drSanzione(1) = "Sedi"
                        drSanzione(2) = Item.Cells(2).Text
                        drSanzione(3) = ""
                        drSanzione(4) = ""
                        drSanzione(5) = Item.Cells(1).Text
                        drSanzione(6) = ""
                        drSanzione(7) = ""
                        drSanzione(8) = ddlSanzione.SelectedValue
                        drSanzione(9) = ""
                        Session("dtSanzione").Rows.Add(drSanzione)
                    End If
                End If
            Next
            If ContaCheck = 0 Then 'controllo se sono stati ceccati i valori nella lista
                lblmessaggio.Text = "Non è stato selezionato nessun valore a cui applicare la sanzione"

            Else

                dgSanzione.DataSource = Session("dtSanzione")
                dgSanzione.DataBind()
                dgSanzione.SelectedIndex = -1
                StampaElencoSanzione()
            End If
            'GRIGLIA SEDI ATTUAZIONE
            '"Sanzione" "Tipo" "Soggetto" "IdEnte" "IdAttivita" "IdEnteSede" "IdEnteSedeAttuazione" "IDAttivitàEnteSedeAttuazione" "IdSanzione"
            For Each Item In dgSediAttuazione.Items
                chkValore = Item.FindControl("chkSediAtt")
                If chkValore.Checked = True Then
                    ContaCheck = ContaCheck + 1
                    If ControlloDuplicati(ddlSanzione.SelectedValue, Item.Cells(2).Text, 7, Item.Cells(1).Text) = False Then
                        drSanzione = Session("dtSanzione").NewRow()
                        drSanzione(0) = "" & ddlSanzione.SelectedItem.Text
                        drSanzione(1) = "Sedi Attuazione"
                        drSanzione(2) = Item.Cells(1).Text
                        drSanzione(3) = ""
                        drSanzione(4) = ""
                        drSanzione(5) = ""
                        drSanzione(6) = Item.Cells(2).Text
                        drSanzione(7) = ""
                        drSanzione(8) = ddlSanzione.SelectedValue
                        drSanzione(9) = ""
                        Session("dtSanzione").Rows.Add(drSanzione)
                    End If
                End If
            Next
            If ContaCheck = 0 Then 'controllo se sono stati ceccati i valori nella lista
                lblmessaggio.Text = "Non è stato selezionato nessun valore a cui applicare la sanzione"

            Else

                dgSanzione.DataSource = Session("dtSanzione")
                dgSanzione.DataBind()
                dgSanzione.SelectedIndex = -1
                StampaElencoSanzione()
            End If
            'GRIGLIA SEDI PROGETTO
            Dim blnIrr As Boolean = False
            '"Sanzione" "Tipo" "Soggetto" "IdEnte" "IdAttivita" "IdEnteSede" "IdEnteSedeAttuazione" "IDAttivitàEnteSedeAttuazione" "IdSanzione"
            For Each Item In dgSediProgetto.Items
                chkValore = Item.FindControl("chkSediProg")
                If chkValore.Checked = True Then
                    ContaCheck = ContaCheck + 1
                    'If ControlloIrregolarità() = True Then
                    '    blnIrr = True
                    '    Exit For
                    'End If
                    If ControlloDuplicati(ddlSanzione.SelectedValue, Item.Cells(5).Text, 8, Item.Cells(1).Text) = False Then
                        drSanzione = Session("dtSanzione").NewRow()
                        drSanzione(0) = "" & ddlSanzione.SelectedItem.Text
                        drSanzione(1) = "Sedi di Progetto"
                        drSanzione(2) = Item.Cells(1).Text
                        drSanzione(3) = ""
                        drSanzione(4) = ""
                        drSanzione(5) = ""
                        drSanzione(6) = ""
                        drSanzione(7) = Item.Cells(5).Text
                        drSanzione(8) = ddlSanzione.SelectedValue
                        drSanzione(9) = ""
                        Session("dtSanzione").Rows.Add(drSanzione)
                    End If
                End If
            Next

            If ContaCheck = 0 Then 'controllo se sono stati ceccati i valori nella lista
                lblmessaggio.Text = "Non è stato selezionato nessun valore a cui applicare la sanzione"

            Else

                dgSanzione.DataSource = Session("dtSanzione")
                dgSanzione.DataBind()
                dgSanzione.SelectedIndex = -1
                StampaElencoSanzione()
            End If
        End If
    End Sub
    Private Function ControlloIrregolarità()
        Dim Item As DataGridItem
        Dim chkValore As CheckBox
        Dim blnIrr As Boolean = False
        ' ControlloIrregolarità = False
        If hddControllo.Value = 2 Then Exit Function
        For Each Item In dgSediAttuazione.Items

            chkValore = Item.FindControl("chkSediAtt") 'griglia sedi attuaizone
            '    chkValore = Item.FindControl("chkSediProg") 'griglia sedi progetto
            If chkValore.Checked = True Then
                If Item.Cells(3).Text = "0" Then
                    'blnIrr = True
                    'ControlloIrregolarità = True
                    hddControllo.Value = 1
                    Exit Function
                End If
            End If
        Next

        For Each Item In dgSediProgetto.Items
            chkValore = Item.FindControl("chkSediProg") 'griglia sedi attuaizone
            '    chkValore = Item.FindControl("chkSediProg") 'griglia sedi progetto
            If chkValore.Checked = True Then
                If Item.Cells(4).Text = "0" Then
                    'blnIrr = True
                    'ControlloIrregolarità = True
                    hddControllo.Value = 1
                    Exit Function
                End If
            End If
        Next
        hddControllo.Value = 2
        'Return ControlloIrregolarità
    End Function

    Sub StampaElencoSanzione()

        'nome e posizione di lettura delle colopnne a base 0
        Dim NomeColonne(3) As String
        Dim NomiCampiColonne(3) As String
        'nome della colonna 
        'e posizione nella griglia di lettura
        NomeColonne(0) = "Sanzione"
        NomeColonne(1) = "Tipo"
        NomeColonne(2) = "Soggetto"
        NomeColonne(3) = "Stato Sanzione"

        NomiCampiColonne(0) = "Sanzione"
        NomiCampiColonne(1) = "Tipo"
        NomiCampiColonne(2) = "Soggetto"
        NomiCampiColonne(3) = "Stato"

        ' Session("DtbRicerca") = Session("dtSanzione")

        'carico un datatable che userò poi nella pagina di stampa
        'il numero delle colonne è a base 0
        CaricaDataTablePerStampa(Session("dtSanzione"), 3, NomeColonne, NomiCampiColonne)

    End Sub

    Function CaricaDataTablePerStampa(ByVal DataTableDaScorrere As DataTable, ByVal NColonne As Integer, ByVal NomiColonne() As String, ByVal NomiCampiColonne() As String) As DataTable
        Dim dt As New DataTable
        Dim dr As DataRow
        Dim i As Integer
        Dim x As Integer
        'carico i nomi delle colonne che andrò a stampare nella datagrid
        For x = 0 To NColonne
            dt.Columns.Add(New DataColumn(NomiColonne(x), GetType(String)))
        Next
        'carico il datatable con il risultato della query della ricerca, in qusto caso delle risorse
        If DataTableDaScorrere.Rows.Count > 0 Then
            For i = 1 To DataTableDaScorrere.Rows.Count
                dr = dt.NewRow()
                For x = 0 To NColonne
                    dr(x) = DataTableDaScorrere.Rows.Item(i - 1).Item(NomiCampiColonne(x))
                Next
                dt.Rows.Add(dr)
            Next
        End If
        'passo alla sessione la datatable che ho appena creato e che userò per il databinding della datagrid della stampa
        Session("DtbRicerca") = dt
    End Function

    Private Sub dgSanzione_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgSanzione.ItemCommand
        lblmessaggio.Text = ""
        Select Case e.CommandName
            Case "Delete"
                If e.Item.Cells(10).Text = "&nbsp;" Then
                    Session("dtSanzione").rows(e.Item.ItemIndex.ToString).delete()
                    dgSanzione.DataSource = Session("dtSanzione")
                    dgSanzione.DataBind()
                    dgSanzione.SelectedIndex = -1
                    StampaElencoSanzione()
                Else
                    lblmessaggio.Visible = False
                    lblErrore.Visible = True
                    lblErrore.Text = "Non è possibile cancellare la sanzione perchè è nello stato: " & e.Item.Cells(10).Text & "."
                End If
            Case "Annulla"
                If e.Item.Cells(10).Text <> "&nbsp;" Then
                    Dim IdA As String = Replace(e.Item.Cells(4).Text, "&nbsp;", "0")  'idattività 
                    Dim IdE As String = Replace(e.Item.Cells(5).Text, "&nbsp;", "0") 'idente
                    Dim IdES As String = Replace(e.Item.Cells(6).Text, "&nbsp;", "0") 'identesede
                    Dim IdESA As String = Replace(e.Item.Cells(7).Text, "&nbsp;", "0") 'identesedeattuazione
                    Dim IdAESA As String = Replace(e.Item.Cells(8).Text, "&nbsp;", "0")  'Idattivitàentesedeattuazione
                    Dim idVer As Integer = Request.QueryString("idverifica")

                    'Response.Write("<SCRIPT>" & vbCrLf)
                    'Response.Write("window.open('ver_AnnullamentoSanzione.aspx?IdEnte=" & IdEnte & "&IdSanzione=" & Replace(e.Item.Cells(9).Text, "&nbsp;", "") & "&IdVer=" & idVer & "&IdA=" & IdA & "&IdE=" & IdE & "&IdES=" & IdES & "&IdESA=" & IdESA & "&IdAESA=" & IdAESA & "','Visualizzazione','height=700,width=800,dependent=yes,scrollbars=no,status=yes,resizable=no');" & vbCrLf)
                    'Response.Write("</SCRIPT>")
                    Response.Redirect("ver_AnnullamentoSanzione.aspx?Segnalata=" & Request.QueryString("Segnalata") & "&IdEnte=" & IdEnte & "&IdSanzione=" & Replace(e.Item.Cells(9).Text, "&nbsp;", "") & "&IdVer=" & idVer & "&IdA=" & IdA & "&IdE=" & IdE & "&IdES=" & IdES & "&IdESA=" & IdESA & "&IdAESA=" & IdAESA)
                Else
                    lblmessaggio.Visible = False
                    lblErrore.Visible = True
                    lblErrore.Text = "Non è possibile annullare la sanzione perchè non è una Sanzione Valida."
                End If
        End Select
    End Sub

    Private Sub ddlSanzione_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlSanzione.SelectedIndexChanged
        If ddlSanzione.SelectedValue <> 0 Then
            TogliCheck(dgProgetti, 1)
            TogliCheck(dgEnteCapofila, 2)
            TogliCheck(dgEnteDipendente, 3)
            TogliCheck(dgSedi, 4)
            TogliCheck(dgSediAttuazione, 5)
            TogliCheck(dgSediProgetto, 6)
        End If

    End Sub
    Private Sub TogliCheck(ByVal Griglia As DataGrid, ByVal NumGriglia As Integer)
        'tolgo i check dalle griglie
        Dim Item As DataGridItem
        Dim chkValore As CheckBox

        For Each Item In Griglia.Items
            Select Case NumGriglia
                Case 1 'PROGETTI
                    chkValore = Item.FindControl("chkProg")
                Case 2 'ENTE CAPOFILA
                    chkValore = Item.FindControl("chkEnteCap")
                Case 3 'ENTE DIPENDENTE (FIGLIO)
                    chkValore = Item.FindControl("chkEnteDip")
                Case 4 'SEDI
                    chkValore = Item.FindControl("chkSedi")
                Case 5 'SEDI ATTUAZIONE
                    chkValore = Item.FindControl("chkSediAtt")
                Case 6 'SEDE PROGETTO
                    chkValore = Item.FindControl("chkSediProg")
            End Select

            If chkValore.Checked = True Then
                chkValore.Checked = False
            End If
        Next
    End Sub
    Private Function ControlloDuplicati(ByVal IdSanzione As Integer, ByVal IdTipo As String, ByVal IdItem As Integer, ByVal Tipo As String) As Boolean
        'Aggiunto il 14/01/2008 da simona cordella
        'Controllo se sono già stati inserite stesse sanzioni per stesso Soggetto
        Dim Item As DataGridItem
        ControlloDuplicati = False
        lblmessaggio.Text = ""
        'GRIGLIA SANZIONE
        '"Sanzione" "Tipo" "Soggetto" "IdEnte" "IdAttivita" "IdSede" "IdSedeAttuazione" "IDAttivitàEnteSedeAttuazione" "IdSanzione"
        For Each Item In dgSanzione.Items
            If IdTipo = "&nbsp;" Then
                IdTipo = 0
            End If
            If Item.Cells(9).Text = IdSanzione And Item.Cells(IdItem).Text = IdTipo Then
                'Session("dtSanzione").rows(Item.Cells(IdItem).Text).delete()
                ControlloDuplicati = True
                Exit For
            End If
        Next
        If ControlloDuplicati = True Then
            'Session("dtSanzione") = Nothing

            'lblmessaggio.Text = "Attenzione.E' già presenta la sanzione  " & ddlSanzione.SelectedItem.Text & " per  " & Tipo & "."
        End If
        Return ControlloDuplicati
    End Function

    Sub InsertProgetto(ByVal IDAttività As Integer, ByVal IDSanzione As Integer, ByVal StatoSanzione As Integer)
        Dim strSql As String
        Dim CmdProgetti As SqlClient.SqlCommand
        'SELECT     IDSanzioneProgetto, IDAttività, IDSanzione, IDVerifica, UtenteEsecutore, DataEsecuzione, DataProtocolloEsecuzione, NProtocolloEsecuzione, 
        '                      CodiceFascicolo, IDFascicolo, DescrFascicolo, Note
        'FROM         TVerificheSanzioniProgetto
        strSql = "INSERT INTO TVerificheSanzioniProgetto( IDAttività, IDSanzione, IDVerifica, UtenteEsecutore, DataEsecuzione,StatoSanzione)" & _
                "   VALUES (" & IDAttività & "," & IDSanzione & "," & Request.QueryString("idverifica") & ", '" & Session("Utente") & "' ,getdate()," & StatoSanzione & ")"
        CmdProgetti = ClsServer.EseguiSqlClient(strSql, Session("conn"))

    End Sub
    Sub InsertEnte(ByVal IDEnte As Integer, ByVal IDSanzione As Integer, ByVal StatoSanzione As Integer)
        Dim strSql As String
        Dim CmdEnte As SqlClient.SqlCommand
        'SELECT     IDEnte, IDSanzione, IDVerifica, UtenteEsecutore, DataEsecuzione, DataProtocolloEsecuzione, NProtocolloEsecuzione, CodiceFascicolo, IDFascicolo, 
        'DescrFascicolo(, Note)
        'FROM(TVerificheSanzioniEnte)
        strSql = "INSERT INTO TVerificheSanzioniEnte( IDEnte, IDSanzione, IDVerifica, UtenteEsecutore, DataEsecuzione,StatoSanzione)" & _
        "   VALUES (" & IDEnte & "," & IDSanzione & "," & Request.QueryString("idverifica") & ", '" & Session("Utente") & "' ,getdate()," & StatoSanzione & ")"
        CmdEnte = ClsServer.EseguiSqlClient(strSql, Session("conn"))
    End Sub
    Sub InsertSede(ByVal IDEnteSede As Integer, ByVal IDSanzione As Integer, ByVal StatoSanzione As Integer)
        Dim strSql As String
        Dim CmdSede As SqlClient.SqlCommand
        'SELECT     IDSanzioneSede, IDEnteSede, IDSanzione, IDVerifica, UtenteEsecutore, DataEsecuzione, DataProtocolloEsecuzione, NProtocolloEsecuzione, 
        'CodiceFascicolo(, IDFascicolo, DescrFascicolo, Note)
        'FROM(TVerificheSanzioniSede)
        strSql = " INSERT INTO TVerificheSanzioniSede( IDEnteSede, IDSanzione, IDVerifica, UtenteEsecutore, DataEsecuzione,StatoSanzione)" & _
                 " VALUES (" & IDEnteSede & "," & IDSanzione & "," & Request.QueryString("idverifica") & ", '" & Session("Utente") & "' ,getdate()," & StatoSanzione & ")"
        CmdSede = ClsServer.EseguiSqlClient(strSql, Session("conn"))
    End Sub
    Sub InsertSedeAttuazione(ByVal IDEnteSedeAttuazione As Integer, ByVal IDSanzione As Integer, ByVal StatoSanzione As Integer)
        Dim strSql As String
        Dim CmdSedeAtt As SqlClient.SqlCommand
        'SELECT     IDSanzioneSedeAttuazione, IDEnteSedeAttuazione, IDSanzione, IDVerifica, UtenteEsecutore, DataEsecuzione, DataProtocolloEsecuzione, 
        'NProtocolloEsecuzione(, CodiceFascicolo, IDFascicolo, DescrFascicolo, Note)
        'FROM(TVerificheSanzioniSedeAttuazione)
        strSql = " INSERT INTO TVerificheSanzioniSedeAttuazione( IDEnteSedeAttuazione, IDSanzione, IDVerifica, UtenteEsecutore, DataEsecuzione,StatoSanzione)" & _
                 " VALUES (" & IDEnteSedeAttuazione & "," & IDSanzione & "," & Request.QueryString("idverifica") & ", '" & Session("Utente") & "' ,getdate()," & StatoSanzione & ")"
        CmdSedeAtt = ClsServer.EseguiSqlClient(strSql, Session("conn"))
    End Sub
    Sub UpdateSedeAttuazione(ByVal IDEnteSedeAttuazione As Integer, ByVal Segnalazione As Integer)
        Dim strSql As String
        Dim CmdSedeAtt As SqlClient.SqlCommand
        'controllo se la se
        strSql = " UPDATE entisediattuazioni " & _
                 " SET Segnalazione= " & Segnalazione & " " & _
                 " WHERE IDEnteSedeAttuazione =" & IDEnteSedeAttuazione & ""
        CmdSedeAtt = ClsServer.EseguiSqlClient(strSql, Session("conn"))
    End Sub
    Sub InsertSedeProgetto(ByVal IDAttivitàSedeAttuazione As Integer, ByVal IDSanzione As Integer, ByVal StatoSanzione As Integer)
        Dim strSql As String
        Dim CmdSedePro As SqlClient.SqlCommand

        'SELECT     IDSanzioneSedeProgetto, IDAttivitàSedeAttuazione, IDSanzione, IDVerifica, UtenteEsecutore, DataEsecuzione, DataProtocolloEsecuzione, 
        'NProtocolloEsecuzione(, CodiceFascicolo, IDFascicolo, DescrFascicolo, Note)
        'FROM(TVerificheSanzioniSedeProgetto)
        strSql = " INSERT INTO TVerificheSanzioniSedeProgetto( IDAttivitàSedeAttuazione, IDSanzione, IDVerifica, UtenteEsecutore, DataEsecuzione,StatoSanzione)" & _
                 " VALUES (" & IDAttivitàSedeAttuazione & "," & IDSanzione & "," & Request.QueryString("idverifica") & ", '" & Session("Utente") & "' ,getdate()," & StatoSanzione & ")"
        CmdSedePro = ClsServer.EseguiSqlClient(strSql, Session("conn"))
    End Sub

    Private Sub cmdSalva_Click(sender As Object, e As EventArgs) Handles cmdSalva.Click
        Dim Item As DataGridItem
        lblmessaggio.Text = ""
        Dim ContaSanzioneAnnullata As Integer
        'Dim StatoSanzione As Integer = 0
        'controllo se la griglia sono stati eliminati tutte le sanzioni
        If dgSanzione.Items.Count = 0 And lblStatoVerifica.Text = "Sanzionata" Then
            lblErrore.Text = "E' necessario indicare almeno una tipologia di Sanzione."
        Else

            'DeleteSanzione()
            'Ciclo la griglia sanzione per l'inserimento nelle tabelle interessate
            For Each Item In dgSanzione.Items
                'controllo lo stato della sanzione
                Select Case Item.Cells(10).Text
                    'MODIFICATA IL 27/10/2011 DA SIMONA CORDELLA --> 
                    '*** INSERISCO SOLO LE SANZIONI CHE HANNO LA COLONNA STATOSANZIONE VUOTA
                    Case "&nbsp;"
                        'StatoSanzione = 0
                        If Item.Cells(5).Text <> "&nbsp;" Then 'IDENTE
                            InsertEnte(Item.Cells(5).Text, Item.Cells(9).Text, 0)
                        ElseIf Item.Cells(4).Text <> "&nbsp;" Then 'IDATTIVITA 
                            InsertProgetto(Item.Cells(4).Text, Item.Cells(9).Text, 0)
                            UpdateProgetto(Item.Cells(4).Text, 1)
                        ElseIf Item.Cells(6).Text <> "&nbsp;" Then 'IDENTESEDE
                            InsertSede(Item.Cells(6).Text, Item.Cells(9).Text, 0)
                        ElseIf Item.Cells(7).Text <> "&nbsp;" Then 'IDENTESEDEATTUAZIONE
                            InsertSedeAttuazione(Item.Cells(7).Text, Item.Cells(9).Text, 0)
                            UpdateSedeAttuazione(Item.Cells(7).Text, 1)
                        ElseIf Item.Cells(8).Text <> "&nbsp;" Then 'IDATTIVITAENTESEDEATTUAZIONE
                            InsertSedeProgetto(Item.Cells(8).Text, Item.Cells(9).Text, 0)
                        End If
                        UpdateTVerifiche(Request.QueryString("idverifica"), 11)
                    Case "Sanzione Valida"
                        'StatoSanzione = 0
                    Case "Sanzione Annullata"
                        ' StatoSanzione = 1
                        If Item.Cells(7).Text <> "&nbsp;" Then 'IDENTESEDEATTUAZIONE
                            '   UpdateSedeAttuazione(Item.Cells(7).Text, 0)
                        End If
                        ContaSanzioneAnnullata = ContaSanzioneAnnullata + 1
                    Case "Sanzione Ripristinata"
                        If Item.Cells(7).Text <> "&nbsp;" Then 'IDENTESEDEATTUAZIONE
                            '    UpdateSedeAttuazione(Item.Cells(7).Text, 1)
                        End If
                        'StatoSanzione = 2
                End Select


            Next
            ''verifico se la variabile che conta le sanzioni annullate è uguale alla count della griglia sanzioni
            'If ContaSanzioneAnnullata = dgSanzione.Items.Count Then
            '    'imposto lo stato della verifica da " SANZIONATA " a " CHIUSA CONTESTATA "
            '    UpdateTVerifiche(Request.QueryString("idverifica"), 14) 'imposto a chiusa contestata
            'Else
            '    If lblStatoVerifica.Text = "Chiusa Contestata" Then
            '        UpdateTVerifiche(Request.QueryString("idverifica"), 11) 'imposto a sanzionata
            '    End If
            'End If
            lblmessaggio.Text = "Salvataggio eseguito con successo"
            Session("dtSanzione") = Nothing
            dgSanzione.DataSource = Session("dtSanzione")
            dgSanzione.DataBind()
            CaricaIntestazione()
            CaricaSanzione()
        End If
    End Sub
    Sub DeleteSanzione()
        'Aggiunto il 14/01/2008 da simona cordella
        Dim strSql As String
        Dim CmdDelete As SqlClient.SqlCommand
        'TVerificheSanzioniProgetto()
        strSql = "Delete from TVerificheSanzioniProgetto where idverifica=" & Request.QueryString("idverifica") & ""
        CmdDelete = ClsServer.EseguiSqlClient(strSql, Session("conn"))
        'TVerificheSanzioniProgetto()
        strSql = "Delete from TVerificheSanzioniProgetto where idverifica=" & Request.QueryString("idverifica") & ""
        CmdDelete = ClsServer.EseguiSqlClient(strSql, Session("conn"))
        'TVerificheSanzioniEnte()
        strSql = "Delete from TVerificheSanzioniEnte where idverifica=" & Request.QueryString("idverifica") & ""
        CmdDelete = ClsServer.EseguiSqlClient(strSql, Session("conn"))
        'TVerificheSanzioniSede()
        strSql = "Delete from TVerificheSanzioniSede where idverifica=" & Request.QueryString("idverifica") & ""
        CmdDelete = ClsServer.EseguiSqlClient(strSql, Session("conn"))
        'TVerificheSanzioniSedeAttuazione()
        strSql = "Delete from TVerificheSanzioniSedeAttuazione where idverifica=" & Request.QueryString("idverifica") & ""
        CmdDelete = ClsServer.EseguiSqlClient(strSql, Session("conn"))
        'TVerificheSanzioniSedeProgetto()
        strSql = "Delete from TVerificheSanzioniSedeProgetto where idverifica=" & Request.QueryString("idverifica") & ""
        CmdDelete = ClsServer.EseguiSqlClient(strSql, Session("conn"))
    End Sub
    Sub UpdateTVerifiche(ByVal IdVerifica As Integer, ByVal IdStatoVerifica As Integer)
        'Aggiunto il 26/10/2011 da simona cordella
        Dim strSql As String
        Dim CmdUpdate As SqlClient.SqlCommand
        strSql = "Update Tverifiche set IdStatoVerifica = " & IdStatoVerifica & "  "
        strSql &= " where IdVerifica = " & IdVerifica & ""
        CmdUpdate = ClsServer.EseguiSqlClient(strSql, Session("conn"))

    End Sub

    Private Sub dgSediProgetto_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgSediProgetto.ItemCommand


        '    Response.Write("<SCRIPT>" & vbCrLf)
        '    Response.Write("window.open('ver_PopupDettSanzione.aspx?VengoDa=SedeProgetti&IDAttivitàSedeAttuazione=" & e.Item.Cells(5).Text & "&IdVerifica=" & Request.QueryString("IdVerifica") & "','report','height=600,width=600,dependent=no,scrollbars=no,status=no,resizable=yes');" & vbCrLf)
        '    Response.Write("</SCRIPT>")

    End Sub

    Private Sub dgProgetti_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgProgetti.ItemCommand
        If e.CommandName = "Select" Then
            Response.Write("<SCRIPT>" & vbCrLf)
            Response.Write("window.open('ver_PopupDettSanzione.aspx?VengoDa=Progetti&IDAttivita=" & e.Item.Cells(3).Text & "&IdVerifica=" & Request.QueryString("IdVerifica") & "','report','height=600,width=600,dependent=no,scrollbars=no,status=no,resizable=yes');" & vbCrLf)
            Response.Write("</SCRIPT>")
        End If
    End Sub

    Private Sub dgEnteCapofila_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgEnteCapofila.ItemCommand
        If e.CommandName = "Select" Then
            Response.Write("<SCRIPT>" & vbCrLf)
            Response.Write("window.open('ver_PopupDettSanzione.aspx?VengoDa=EnteCapofila&IDEnte=" & e.Item.Cells(2).Text & "&IdVerifica=" & Request.QueryString("IdVerifica") & "','report','height=600,width=600,dependent=no,scrollbars=no,status=no,resizable=yes');" & vbCrLf)
            Response.Write("</SCRIPT>")
        End If
    End Sub
    Private Sub dgEnteDipendente_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgEnteDipendente.ItemCommand
        If e.CommandName = "Select" Then
            Response.Write("<SCRIPT>" & vbCrLf)
            Response.Write("window.open('ver_PopupDettSanzione.aspx?VengoDa=EnteDipendente&IDEnteFiglio=" & e.Item.Cells(2).Text & "&IdVerifica=" & Request.QueryString("IdVerifica") & "','report','height=600,width=600,dependent=no,scrollbars=no,status=no,resizable=yes');" & vbCrLf)
            Response.Write("</SCRIPT>")
        End If
    End Sub

    Private Sub dgSedi_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgSedi.ItemCommand
        If e.CommandName = "Select" Then
            Response.Write("<SCRIPT>" & vbCrLf)
            Response.Write("window.open('ver_PopupDettSanzione.aspx?VengoDa=Sede&IDEnteSede=" & e.Item.Cells(1).Text & "&IdVerifica=" & Request.QueryString("IdVerifica") & "','report','height=600,width=600,dependent=no,scrollbars=no,status=no,resizable=yes');" & vbCrLf)
            Response.Write("</SCRIPT>")
        End If
    End Sub

    Private Sub dgSediAttuazione_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgSediAttuazione.ItemCommand
        If e.CommandName = "Select" Then
            Response.Write("<SCRIPT>" & vbCrLf)
            Response.Write("window.open('ver_PopupDettSanzione.aspx?VengoDa=SediAttuazione&IDEnteSedeAttuazione=" & e.Item.Cells(2).Text & "&IdVerifica=" & Request.QueryString("IdVerifica") & "','report','height=600,width=600,dependent=no,scrollbars=no,status=no,resizable=yes');" & vbCrLf)
            Response.Write("</SCRIPT>")
        End If
    End Sub

    Sub UpdateProgetto(ByVal IDAttività As Integer, ByVal Segnalazione As Integer)
        Dim strSql As String
        Dim CmdSedeAtt As SqlClient.SqlCommand
        'controllo se la se
        strSql = " UPDATE Attività " & _
                 " SET SegnalazioneSanzione = " & Segnalazione & " " & _
                 " WHERE IDAttività =" & IDAttività & ""
        CmdSedeAtt = ClsServer.EseguiSqlClient(strSql, Session("conn"))
    End Sub



    'Private Sub StampaCSV(ByVal dtbRicerca As DataTable)
    '    Dim path As String
    '    Dim xPrefissoNome As String
    '    Dim url As String
    '    Dim utility As ClsUtility = New ClsUtility()

    '    If dtbRicerca.Rows.Count = 0 Then
    '        ApriCSV1.Visible = False
    '        imgStampa.Visible = False
    '    Else
    '        xPrefissoNome = Session("Utente")
    '        path = Server.MapPath("download")
    '        url = CreaFileCSV(dtbRicerca, xPrefissoNome, path)
    '        ApriCSV1.Visible = True
    '        ApriCSV1.NavigateUrl = url
    '    End If
    'End Sub
    Function CreaFileCSV(ByVal DTBRicerca As DataTable, ByVal xPrefissoNome As String, ByVal mapPath As String) As String

        Dim writer As StreamWriter
        Dim xLinea As String = String.Empty
        Dim i As Int64
        Dim j As Int64
        Dim nomeUnivoco As String
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


    Protected Sub dgSanzione_SelectedIndexChanged(sender As Object, e As EventArgs) Handles dgSanzione.SelectedIndexChanged

    End Sub
End Class
