Public Class ricercavolontarialtroprogetto
    Inherits System.Web.UI.Page

#Region " Codice generato da Progettazione Web Form "

    'Chiamata richiesta da Progettazione Web Form.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents lblTitolo As System.Web.UI.WebControls.Label
    Protected WithEvents Form1 As System.Web.UI.HtmlControls.HtmlForm

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
        If Not Session("LogIn") Is Nothing Then
            If Session("LogIn") = False Then 'verifico validità log-in
                Response.Redirect("LogOn.aspx")
            End If
        Else
            Response.Redirect("LogOn.aspx")
        End If
        If IsPostBack = False Then
            lblTitolo.Text = "Ricerca Volontari altri progetti"

            CaricaDataGrid()
        End If
    End Sub

    Private Sub dgVolontari_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgVolontari.PageIndexChanged
        'Cambia pag della Griglia
        dgVolontari.CurrentPageIndex = e.NewPageIndex
        dgVolontari.DataSource = Session("appDtsRisRicerca")
        dgVolontari.DataBind()
        dgVolontari.SelectedIndex = -1
    End Sub

    Private Sub cmdRicerca_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles cmdRicerca.Click
        CaricaDataGrid()
    End Sub

    Private Sub dgVolontari_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dgVolontari.SelectedIndexChanged
        If Request.QueryString("provieneda") = "Volontari" Then
            Response.Redirect("WfrmGestioneSostituisciVolontari.aspx?IdEntita=" & Request.QueryString("IdEntita") & "&VengoDa=AltriProgetti&IdAttivita=" & Request.QueryString("IdAttivita") & "&IdNuovaEntita=" & dgVolontari.SelectedItem.Cells(0).Text & "&VecchioIdAttivitaEntita=" & Request.QueryString("VecchioIdAttivitaEntita") & "&Op=" & Request.QueryString("Op") & "&provieneda=" & Request.QueryString("provieneda"))
        Else
            Response.Redirect("WfrmGestioneSostituisciVolontari.aspx?IdEntita=" & Request.QueryString("IdEntita") & "&VengoDa=AltriProgetti&IdAttivita=" & Request.QueryString("IdAttivita") & "&IdNuovaEntita=" & dgVolontari.SelectedItem.Cells(0).Text & "&VecchioIdAttivitaEntita=" & Request.QueryString("VecchioIdAttivitaEntita") & "&Op=" & Request.QueryString("Op"))
        End If

    End Sub

    Sub CaricaDataGrid()
        Dim strsql As String
        Dim MyDataSet As DataSet

        strsql = "SELECT Gruppo "
        strsql = strsql & "FROM bando "
        strsql = strsql & "INNER JOIN BandiAttività ON bando.IDBando = BandiAttività.IdBando "
        strsql = strsql & "INNER JOIN attività ON BandiAttività.IdBandoAttività = attività.IDBandoAttività "
        strsql = strsql & "WHERE idattività = " & Request.QueryString("IdAttivita")

        MyDataSet = ClsServer.DataSetGenerico(strsql, Session("conn"))
        Dim Gruppo As Integer
        If MyDataSet.Tables(0).Rows.Count <> 0 Then
            Gruppo = MyDataSet.Tables(0).Rows(0).Item("Gruppo")
        End If
        MyDataSet.Dispose()





        lblmessaggioRicerca.Text = ""
        strsql = "SELECT entità.IDEntità, entità.Cognome + ' ' + entità.Nome AS Nominativo, entità.CodiceFiscale, entità.DataNascita, comuni.denominazione as ComuneNascita, "
        strsql = strsql & "attività.Titolo + ' [' + attività.CodiceEnte + ']' AS Progetto, entisedi.Denominazione + ' - ' + comuni_1.Denominazione AS SedeAttuazione, "
        strsql = strsql & "GraduatorieEntità.Punteggio, bando.Gruppo "
        strsql = strsql & "FROM entità INNER JOIN "
        strsql = strsql & "comuni ON entità.IDComuneNascita = comuni.IDComune INNER JOIN "
        strsql = strsql & "GraduatorieEntità ON entità.IDEntità = GraduatorieEntità.IdEntità INNER JOIN "
        strsql = strsql & "AttivitàSediAssegnazione ON GraduatorieEntità.IdAttivitàSedeAssegnazione = AttivitàSediAssegnazione.IDAttivitàSedeAssegnazione INNER JOIN "
        strsql = strsql & "attività ON AttivitàSediAssegnazione.IDAttività = attività.IDAttività INNER JOIN "
        strsql = strsql & "entisedi ON AttivitàSediAssegnazione.IDEnteSede = entisedi.IDEnteSede INNER JOIN "
        strsql = strsql & "comuni comuni_1 ON entisedi.IDComune = comuni_1.IDComune INNER JOIN "
        strsql = strsql & "StatiEntità ON entità.IDStatoEntità = StatiEntità.IDStatoEntità "
        strsql = strsql & "INNER JOIN BandiAttività ON attività.IDBandoAttività = BandiAttività.IdBandoAttività "
        strsql = strsql & "INNER JOIN  bando ON BandiAttività.IdBando = bando.IDBando "
        strsql = strsql & "INNER JOIN TipiProgetto ON Attività.IdTipoProgetto = TipiProgetto.IdTipoProgetto "
        strsql = strsql & "WHERE (entità.DisponibileAltriProg = 1) AND (attività.IDEntePresentante ='" & Session("IdEnte") & "') AND (GraduatorieEntità.Ammesso = 0) AND (GraduatorieEntità.Stato = 1) AND "
        strsql = strsql & "(StatiEntità.DefaultStato = 1) and isnull(entità.ammessorecupero,0) = " & Request.QueryString("AmmessoRecupero") & " " 'AND (attività.IDAttività ='" & Request.QueryString("IdAttivita") & "') "

        If txtNome.Text <> "" Then
            strsql = strsql & "and entità.nome like '" & ClsServer.NoApice(txtNome.Text) & "%' "
        End If
        If txtCognome.Text <> "" Then
            strsql = strsql & "and entità.cognome like '" & ClsServer.NoApice(txtCognome.Text) & "%' "
        End If
        If txtProgetto.Text <> "" Then
            strsql = strsql & "and attività.titolo like '" & ClsServer.NoApice(txtProgetto.Text) & "%' "
        End If
        If txtCodProgetto.Text <> "" Then
            strsql = strsql & "and attività.CodiceEnte like '" & ClsServer.NoApice(txtCodProgetto.Text) & "%' "
        End If
        'FiltroVisibilita
        strsql = strsql & " and TipiProgetto.MacroTipoProgetto like '" & Session("FiltroVisibilita") & "' "
        'filtro congruenza macro tipo progetto
        strsql = strsql & "and Gruppo = " & Gruppo & " and tipiprogetto.idtipoprogetto = (select idtipoprogetto from attività where idattività = " & Request.QueryString("IdAttivita") & ") "
        strsql = strsql & "ORDER BY entità.Cognome + ' ' + entità.Nome "

        MyDataSet = ClsServer.DataSetGenerico(strsql, Session("conn"))

        'assegno il dataset alla griglia del risultato
        dgVolontari.DataSource = MyDataSet
        Session("appDtsRisRicerca") = MyDataSet
        dgVolontari.DataBind()
        If dgVolontari.Items.Count = 0 Then
            lblmessaggioRicerca.Text = "La ricerca non ha prodotto risultati."
        Else

            lblmessaggioRicerca.Text = "Risultato " & lblTitolo.Text
        End If
    End Sub

    Private Sub cmdChiudi_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles cmdChiudi.Click
        If Request.QueryString("provieneda") = "Volontari" Then
            Response.Redirect("WfrmGestioneSostituisciVolontari.aspx?VengoDa=AltriProgetti&IdAttivita=" & Request.QueryString("IdAttivita") & "&IdEntita=" & Request.QueryString("IdEntita") & "&VecchioIdAttivitaEntita=" & Request.QueryString("VecchioIdAttivitaEntita") & "&Op=" & Request.QueryString("Op") & "&provieneda=" & Request.QueryString("provieneda"))
        Else
            Response.Redirect("WfrmGestioneSostituisciVolontari.aspx?VengoDa=AltriProgetti&IdAttivita=" & Request.QueryString("IdAttivita") & "&IdEntita=" & Request.QueryString("IdEntita") & "&VecchioIdAttivitaEntita=" & Request.QueryString("VecchioIdAttivitaEntita") & "&Op=" & Request.QueryString("Op"))
        End If
    End Sub


End Class
