Public Class WfrmCronologiaDettaglioAnagrafica
    Inherits System.Web.UI.Page

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
        If Page.IsPostBack = False Then
            Session("MyLocalDataSet") = Nothing
            CaricaGriglia(Request.QueryString("IdVol"))
            lblNominativo.Text = lblNominativo.Text & Request.QueryString("Nominativo")
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
        'strSql = "SELECT * FROM CronologiaEntitàDocumenti WHERE IdEntità =" & MyIdVol
        'modifica da s.c. il 26/08/2011 sono state inserite due nuone colonne IBAN E BIC_SWIFT
        strSql = "SELECT comuni.Denominazione AS ComuneResidenza, CronologiaEntitàDettagli.Indirizzo as Indi, CronologiaEntitàDettagli.CAP as Cap, " & _
                  "CronologiaEntitàDettagli.NumeroCivico as Nciv, CronologiaEntitàDettagli.CodiceLibrettoPostale as Libretto, CronologiaEntitàDettagli.DataInizioValidità as dataval, " & _
                  "CronologiaEntitàDettagli.DataFineValidità as datafine, CronologiaEntitàDettagli.UserNameUltimaModifica as usermod, entità.Cognome as cognome, entità.Nome as Nome, " & _
                  "CronologiaEntitàDettagli.IBAN,CronologiaEntitàDettagli.BIC_SWIFT, " & _
                  "pa.Descrizione as GMO, pa1.Descrizione AS FAMI, CronologiaEntitàDettagli.TMPIdSedeAttuazioneSecondaria as SedeSecondaria  " & _
                  "FROM CronologiaEntitàDettagli  " & _
                  "INNER JOIN entità ON CronologiaEntitàDettagli.IdEntità = entità.IDEntità " & _
                  "LEFT OUTER JOIN comuni ON CronologiaEntitàDettagli.IDComuneResidenza = comuni.IDComune " & _
                  "LEFT JOIN ParticolaritàEntità pa on CronologiaEntitàDettagli.GMO = pa.codice and pa.Macrotipo = 'GMO' " & _
                  "LEFT JOIN ParticolaritàEntità pa1 on CronologiaEntitàDettagli.FAMI = pa1.codice and pa1.Macrotipo = 'FAMI' " & _
                  "WHERE CronologiaEntitàDettagli.IdEntità = " & MyIdVol & " " & _
         "ORDER BY CronologiaEntitàDettagli.DataInizioValidità "
        Mydataset = ClsServer.DataSetGenerico(strSql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
        dtgRisultatoRicerca.DataSource = Mydataset
        dtgRisultatoRicerca.DataBind()
        Session("MyLocalDataSet") = Mydataset
    End Sub

End Class
