Public Class WfrmRicercaVolontariInServizio
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
        If Not Session("LogIn") Is Nothing Then
            If Session("LogIn") = False Then 'verifico validità log-in
                Response.Redirect("LogOn.aspx")
            End If
        Else
            Response.Redirect("LogOn.aspx")
        End If
        If Session("TipoUtente") = "U" Then
            lblEnte.Visible = True
            txtEnte.Visible = True
            lblCodEnte.Visible = True
            txtCodEnte.Visible = True
        End If

        Select Case Request.QueryString("Op")
            Case "rinuncia"
                lblTitolo.Text = "Ricerca Volontari Nuova Rinuncia"

            Case "esclusione"
                lblTitolo.Text = "Ricerca Volontari Nuova Esclusione"

            Case "rimborso"
                lblTitolo.Text = "Ricerca Volontari Presentazione Rimborso"

            Case "trasferimento"
                lblTitolo.Text = "Ricerca Volontari da Ricollocare"
        End Select
        lblmessaggioRicerca.Text = ""
        'If IsPostBack = False Then
        'CaricaGriglia()
        'End If
    End Sub

    Private Sub cmdChiudi_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles cmdChiudi.Click
        'chiamo homepage
        Response.Redirect("WfrmMain.aspx")
    End Sub

    Sub CaricaGriglia()
        Dim strSql As String
        Dim strWhere As String
        Dim MyDataSet As DataSet
        'DESCRIZIONE: routine che carica la griglia con tutti i progetti con stato attività=1
        'AUTORE: Michele d'Ascenzio    
        'DATA: 03/11/2004
        ' modificato da simona cordella il 06/07/2008
        'ricerco tutti i volontari in servizio che hanno la data chiusura ISNULL

        Try
            strSql = "SELECT Entità.IDEntità as IdEntità, " & _
                     "Attività.IDAttività as IdAttività, " & _
                     "Enti.IdEnte as IdEnte, " & _
                     "Entità.Cognome + ' ' + Entità.Nome as Nominativo, " & _
                     "Entità.CodiceFiscale, " & _
                     "Entità.DataNascita, " & _
                     "Comuni.Denominazione as ComuneNascita, " & _
                     "Attività.Titolo as Progetto, " & _
                     "EntiSediAttuazioni.Denominazione as SedeAttuazione, " & _
                     "Enti.Denominazione as Ente " & _
                     "FROM Entità " & _
                     "INNER JOIN Comuni ON Entità.IdComuneNascita = Comuni.IdComune " & _
                     "INNER JOIN AttivitàEntità ON Entità.IdEntità = AttivitàEntità.IdEntità " & _
                     "INNER JOIN StatiAttivitàEntità ON StatiAttivitàEntità.IdStatoAttivitàEntità = AttivitàEntità.IdStatoAttivitàEntità AND StatiAttivitàEntità.DefaultStato = 1 " & _
                     "INNER JOIN AttivitàEntiSediAttuazione ON AttivitàEntità.IDAttivitàEnteSedeAttuazione = AttivitàEntiSediAttuazione.IDAttivitàEnteSedeAttuazione " & _
                     "INNER JOIN EntiSediAttuazioni ON AttivitàEntiSediAttuazione.IDEnteSedeAttuazione = EntiSediAttuazioni.IDEnteSedeAttuazione " & _
                     "INNER JOIN EntiSedi ON EntiSediAttuazioni.IdEnteSede = EntiSedi.IdEnteSede " & _
                     "INNER JOIN Attività ON AttivitàEntiSediAttuazione.IdAttività = Attività.IdAttività " & _
                     "INNER JOIN Enti ON attività.IdEntepresentante = Enti.IdEnte " & _
                     "INNER JOIN StatiEntità ON Entità.IdStatoEntità = Statientità.IdStatoEntità " & _
                     "INNER JOIN TipiProgetto ON Attività.IdTipoProgetto = TipiProgetto.IdTipoProgetto " & _
                     "WHERE StatiEntità.InServizio = 1 " & _
                     " AND Entità.DataChiusura is null "
            If Session("TipoUtente") = "E" Then
                strSql = strSql & "AND Enti.IdEnte = " & Session("IdEnte") & " "
            End If
            If txtEnte.Text <> vbNullString Then
                strSql = strSql & "AND Enti.Denominazione like '" & Replace(txtEnte.Text, "'", "''") & "%' "
            End If
            If txtCodEnte.Text <> vbNullString Then
                strSql = strSql & "AND Enti.CodiceRegione like '" & Replace(txtCodEnte.Text, "'", "''") & "%' "
            End If
            If txtCognome.Text <> vbNullString Then
                strSql = strSql & "AND Entità.Cognome like '" & Replace(txtCognome.Text, "'", "''") & "%' "
            End If
            If txtNome.Text <> vbNullString Then
                strSql = strSql & "AND Entità.Nome like '" & Replace(txtNome.Text, "'", "''") & "%' "
            End If
            'aggiunto il 24/02/2015 da s.c.
            If txtCodiceVolontario.Text <> vbNullString Then
                strSql = strSql & "AND Entità.CodiceVolontario = '" & Replace(txtCodiceVolontario.Text, "'", "''") & "'"
            End If
            If txtCodiceFiscale.Text <> vbNullString Then
                strSql = strSql & "AND Entità.CodiceFiscale  = '" & Replace(txtCodiceFiscale.Text, "'", "''") & "'"
            End If
            '**
            If txtProgetto.Text <> vbNullString Then
                strSql = strSql & "AND Attività.Titolo like '" & Replace(txtProgetto.Text, "'", "''") & "%' "
            End If
            If txtCodProgetto.Text <> vbNullString Then
                strSql = strSql & "AND Attività.codiceente like '" & Replace(txtCodProgetto.Text, "'", "''") & "%'"
            End If
            'FiltroVisibilita
            strSql = strSql & " and TipiProgetto.MacroTipoProgetto like '" & Session("FiltroVisibilita") & "' "
            MyDataSet = ClsServer.DataSetGenerico(strSql, Session("conn"))

            'assegno il dataset alla griglia del risultato
            dgVolontari.DataSource = MyDataSet

            Session("appDtsRisRicerca") = MyDataSet
            If (Session("TipoUtente") = "U" Or Session("TipoUtente") = "R") Then
                dgVolontari.Columns(10).Visible = True
            Else
                dgVolontari.Columns(10).Visible = False
            End If
            dgVolontari.DataBind()
            dgVolontari.Visible = True
            If dgVolontari.Items.Count = 0 Then
                lblmessaggioRicerca.Text = "La ricerca non ha prodotto risultati."
            Else

                lblmessaggioRicerca.Text = "Risultato " & lblTitolo.Text
            End If
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Private Sub dgVolontari_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgVolontari.PageIndexChanged
        'AUTORE: MIchele d'Ascenzio
        'DATA: 03/11/2004
        'Cambia pag della Griglia
        dgVolontari.CurrentPageIndex = e.NewPageIndex
        dgVolontari.DataSource = Session("appDtsRisRicerca")
        dgVolontari.DataBind()
        dgVolontari.SelectedIndex = -1
    End Sub

    Private Sub cmdRicerca_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles cmdRicerca.Click

        CaricaGriglia()
    End Sub

    Private Sub dgVolontari_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dgVolontari.SelectedIndexChanged

        If (Session("TipoUtente") = "U" Or Session("TipoUtente") = "R") Then
            Session("IdEnte") = dgVolontari.SelectedItem.Cells(2).Text
            Session("Denominazione") = dgVolontari.SelectedItem.Cells(10).Text
        End If
        Select Case Request.QueryString("Op")
            Case Is = "rinuncia"
                Response.Redirect("DettagliVolontario.aspx?IdProgetto=" & dgVolontari.SelectedItem.Cells(1).Text & "&IdVolontario=" & dgVolontari.SelectedItem.Cells(0).Text & "&Op=" & Request.QueryString("Op"))
            Case Is = "esclusione"
                Response.Redirect("DettagliVolontario.aspx?IdProgetto=" & dgVolontari.SelectedItem.Cells(1).Text & "&IdVolontario=" & dgVolontari.SelectedItem.Cells(0).Text & "&Op=" & Request.QueryString("Op"))
            Case Is = "rimborso"
                Response.Redirect("PresentazioneRimborso.aspx?IdProgetto=" & dgVolontari.SelectedItem.Cells(1).Text & "&IdVolontario=" & dgVolontari.SelectedItem.Cells(0).Text & "&Op=" & Request.QueryString("Op"))
            Case Is = "trasferimento"
                Response.Redirect("RicollocaVolontario.aspx?IdProgetto=" & dgVolontari.SelectedItem.Cells(1).Text & "&IdVolontario=" & dgVolontari.SelectedItem.Cells(0).Text & "&Op=" & Request.QueryString("Op"))

                'Response.Redirect("TrasferimentoVolontari.aspx?IdProgetto=" & dgVolontari.SelectedItem.Cells(1).Text & "&IdVolontario=" & dgVolontari.SelectedItem.Cells(0).Text & "&Op=" & Request.QueryString("Op"))
        End Select


    End Sub



End Class

