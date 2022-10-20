'Generata da Amilcare Paolella il 5/4/2006
'Form per la visualizzazione della cronoligia dei documenti creati per il Volontario

Public Class WfrmCronologiaDocumentiVolontario
    Inherits System.Web.UI.Page
    Dim strsql As String
    Dim dtrGenerico As SqlClient.SqlDataReader
    Dim dtsGenerico As DataSet
    Dim cmdGenerico As SqlClient.SqlCommand
    Dim volonatrio As String

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
        If Page.IsPostBack = False Then
            Session("MyLocalDataSet") = Nothing
            CaricaGriglia(Request.QueryString("IdVol"))
            lblNominativo.Text = lblNominativo.Text & Request.QueryString("Nominativo")
            AbilitaUtenteCacellaDoc()
        End If
    End Sub

    Private Sub dtgRisultatoRicerca_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dtgRisultatoRicerca.PageIndexChanged
        dtgRisultatoRicerca.CurrentPageIndex = e.NewPageIndex
        dtgRisultatoRicerca.DataSource = Session("MyLocalDataSet")
        dtgRisultatoRicerca.DataBind()
    End Sub

    Sub CaricaGriglia(ByVal MyIdVol As String)
        Dim strSql As String
        Dim Mydataset As New DataSet
        strSql = "SELECT * FROM CronologiaEntitàDocumenti WHERE IdEntità =" & MyIdVol
        Mydataset = ClsServer.DataSetGenerico(strSql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
        dtgRisultatoRicerca.DataSource = Mydataset
        dtgRisultatoRicerca.DataBind()
        Session("MyLocalDataSet") = Mydataset

    End Sub
    'Antonello Di Croce
    Private Sub AbilitaUtenteCacellaDoc()
        If Session("TipoUtente") <> "" Then ' in caso poi si deve distinguere il tipo di utenza c'e' gia' la if 
            If Not dtrGenerico Is Nothing Then
                dtrGenerico.Close()
                dtrGenerico = Nothing
            End If

            strsql = "SELECT AssociaUtenteGruppo.username, VociMenu.IdVoceMenu, VociMenu.VoceMenu, VociMenu.ImmaginePassiva, VociMenu.ImmagineAttiva, VociMenu.Link," & _
                    " VociMenu.IdVoceMenuPadre" & _
                    " FROM VociMenu" & _
                    " INNER JOIN 	AbilitazioniProfiliVociMenu ON VociMenu.IdVoceMenu = AbilitazioniProfiliVociMenu.IdVoceMenu" & _
                    " INNER JOIN	Profili ON AbilitazioniProfiliVociMenu.IdProfilo = Profili.IdProfilo"
            '============================================================================================================================
            '====================================================30/09/2008==============================================================
            '=======MODIFICA EFFETTUATA DA Kronk (99 scimmie saltavano sul letto, una cadde in terra e si ruppe il cervelletto...)=======
            '=================AGGIUNTA JOIN SU PROFILO READ PER ABILITARE LEGGERE LE ABILITAZIONI ANCHE DELL'UTENTE READ=================
            '============================================================================================================================
            If Session("Read") <> "1" Then
                strsql = strsql & " INNER JOIN	AssociaUtenteGruppo ON Profili.IdProfilo = AssociaUtenteGruppo.IdProfilo "
            Else
                strsql = strsql & " INNER JOIN	AssociaUtenteGruppo ON Profili.IdProfilo = AssociaUtenteGruppo.IdProfiloRead "
            End If
            strsql = strsql & " LEFT JOIN 	RestrizioniUtenteVociMenu ON AssociaUtenteGruppo.UserName = RestrizioniUtenteVociMenu.UserName and RestrizioniUtenteVociMenu.IdVoceMenu=VociMenu.IdVoceMenu" & _
                    " WHERE (VociMenu.descrizione = 'Forza Cancella Documenti')" & _
                    " AND AssociaUtenteGruppo.username ='" & Session("Utente") & "' and (RestrizioniUtenteVociMenu.TipoRestrizione <> 0 or RestrizioniUtenteVociMenu.TipoRestrizione is null)"
            dtrGenerico = ClsServer.CreaDatareader(strsql, Session("Conn"))

            If dtrGenerico.HasRows = True Then
                dtgRisultatoRicerca.Columns.Item(4).Visible = True
            Else
                dtgRisultatoRicerca.Columns.Item(4).Visible = False
            End If

            If Not dtrGenerico Is Nothing Then
                dtrGenerico.Close()
                dtrGenerico = Nothing
            End If
        End If
    End Sub
    'Antonello Di Croce
    Private Sub dtgRisultatoRicerca_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dtgRisultatoRicerca.ItemCommand
        If e.CommandName = "Rimuovi" Then
            lblConferma.Text = ""
            strsql = "delete CronologiaEntitàDocumenti where idCronologiaEntitàDocumento='" & e.Item.Cells(0).Text & "'"
            cmdGenerico = ClsServer.EseguiSqlClient(strsql, Session("conn"))
            CaricaGriglia(Request.QueryString("IdVol"))
            lblConferma.Text = "Documento Eliminato."
        End If
    End Sub
End Class
