Imports System.Data.SqlClient

Public Class stampaprogettiattesagraduatoria
    Inherits System.Web.UI.Page
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

    Private Sub NascondiMenuLaterale()
        Session("TP") = True
    End Sub

#End Region
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load
        VerificaSessione()
        If Page.IsPostBack = False Then
            ' Creo manualmente l'istanza del controllo DataGrid
            Dim grid As New DataGrid
            Dim strsql As String
            Dim dataprogetti As DataTable

            ' chiamo la routine che imposta alcune proprietà di visualizzazione della griglia
            SetGridLayout(grid)
            'se si tratta di utenza regionale
            If Request.QueryString("strIdRegioneCompetenza") = 22 Then
                strsql = "SELECT isnull(replace(replace(replace(replace(replace(replace(replace(CODICEENTE,'°',''),'ì','i'''),'é','e'''),'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'') as CODICEENTE, "
                strsql = strsql & "isnull(replace(replace(replace(replace(replace(replace(replace(DENOMINAZIONEENTE,'°',''),'ì','i'''),'é','e'''),'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'') as DENOMINAZIONEENTE, "
                strsql = strsql & "isnull(replace(replace(replace(replace(replace(replace(replace(CODICEPROGETTO,'°',''),'ì','i'''),'é','e'''),'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'') as CODICEPROGETTO, "
                strsql = strsql & "isnull(replace(replace(replace(replace(replace(replace(replace(TITOLOPROGETTO,'°',''),'ì','i'''),'é','e'''),'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'') as TITOLOPROGETTO, "
                strsql = strsql & "isnull(replace(replace(replace(replace(replace(replace(replace(SETTORE,'°',''),'ì','i'''),'é','e'''),'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'') as SETTORE, "
                strsql = strsql & "isnull(replace(replace(replace(replace(replace(replace(replace(AREAINTERVENTO,'°',''),'ì','i'''),'é','e'''),'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'') as AREAINTERVENTO, "
                strsql = strsql & "VolontariRic, "
                strsql = strsql & "VolontariEff, "
                strsql = strsql & "PUNTEGGIO, "
                strsql = strsql & "isnull(replace(replace(replace(replace(replace(replace(replace(Regione,'°',''),'ì','i'''),'é','e'''),'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'') as Regione, "
                strsql = strsql & "isnull(replace(replace(replace(replace(replace(replace(replace(Provincia,'°',''),'ì','i'''),'é','e'''),'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'') as Provincia, "
                strsql = strsql & "isnull(replace(replace(replace(replace(replace(replace(replace(Comune,'°',''),'ì','i'''),'é','e'''),'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'') as Comune, "
                strsql = strsql & "Limitazioni "
                strsql = strsql & "FROM VW_ELENCO_PROGETTI_GRADUATORIA_PROVINCIA "
                strsql = strsql & "Where IdRegioneCompetenza=" & Request.QueryString("strIdRegioneCompetenza") & " AND IdStatoAttività=9 AND AssociazioneAutomatica=1 "
                strsql = strsql & " AND MacroTipoProgetto like '" & Session("FiltroVisibilita") & "'"
                strsql = strsql & " ORDER BY Punteggio desc, codiceente asc, codiceprogetto asc"
            Else
                strsql = "SELECT isnull(replace(replace(replace(replace(replace(replace(replace(CODICEENTE,'°',''),'ì','i'''),'é','e'''),'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'') as CODICEENTE, "
                strsql = strsql & "isnull(replace(replace(replace(replace(replace(replace(replace(DENOMINAZIONEENTE,'°',''),'ì','i'''),'é','e'''),'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'') as DENOMINAZIONEENTE, "
                strsql = strsql & "isnull(replace(replace(replace(replace(replace(replace(replace(CODICEPROGETTO,'°',''),'ì','i'''),'é','e'''),'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'') as CODICEPROGETTO, "
                strsql = strsql & "isnull(replace(replace(replace(replace(replace(replace(replace(TITOLOPROGETTO,'°',''),'ì','i'''),'é','e'''),'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'') as TITOLOPROGETTO, "
                strsql = strsql & "isnull(replace(replace(replace(replace(replace(replace(replace(SETTORE,'°',''),'ì','i'''),'é','e'''),'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'') as SETTORE, "
                strsql = strsql & "isnull(replace(replace(replace(replace(replace(replace(replace(AREAINTERVENTO,'°',''),'ì','i'''),'é','e'''),'à','a'''),'ò','o'''),'è','e'''),'ù','u'''),'') as AREAINTERVENTO, "
                strsql = strsql & "VolontariRic, "
                strsql = strsql & "VolontariEff, "
                strsql = strsql & "PUNTEGGIO, "
                strsql = strsql & "Limitazioni "
                strsql = strsql & "FROM VW_ELENCO_PROGETTI "
                strsql = strsql & "WHERE IdRegioneCompetenza=" & Request.QueryString("strIdRegioneCompetenza") & " AND IdStatoAttività=9 AND AssociazioneAutomatica=1 "
                strsql = strsql & " AND MacroTipoProgetto like '" & Session("FiltroVisibilita") & "'"
                strsql = strsql & "ORDER BY Punteggio desc, codiceente asc, codiceprogetto asc"
            End If

            dataprogetti = ClsServer.CreaDataTable(strsql, False, Session("conn"))

            ' 1) imposto la sorgente dei dati per la griglia
            'grid.DataSource = Session("appDtsRisRicerca").Tables(0)
            grid.DataSource = dataprogetti
            grid.AutoGenerateColumns = False

            Dim bc1 As New BoundColumn
            bc1.DataField = "CODICEENTE"
            bc1.HeaderText = "Codice Ente"
            grid.Columns.Add(bc1)

            Dim bc2 As New BoundColumn
            bc2.DataField = "DENOMINAZIONEENTE"
            bc2.HeaderText = "Denominazione Ente"
            grid.Columns.Add(bc2)

            Dim bc3 As New BoundColumn
            bc3.DataField = "CODICEPROGETTO"
            bc3.HeaderText = "Codice Progetto"
            grid.Columns.Add(bc3)

            Dim bc4 As New BoundColumn
            bc4.DataField = "TITOLOPROGETTO"
            bc4.HeaderText = "Titolo Progetto"
            grid.Columns.Add(bc4)

            Dim bc5 As New BoundColumn
            bc5.DataField = "SETTORE"
            bc5.HeaderText = "Settore"
            grid.Columns.Add(bc5)

            Dim bc6 As New BoundColumn
            bc6.DataField = "AREAINTERVENTO"
            bc6.HeaderText = "Area di Intervento"
            grid.Columns.Add(bc6)

            If Request.QueryString("strIdRegioneCompetenza") = 22 Then
                Dim bc13 As New BoundColumn
                bc13.DataField = "Regione"
                bc13.HeaderText = "Regione"
                grid.Columns.Add(bc13)

                Dim bc11 As New BoundColumn
                bc11.DataField = "Provincia"
                bc11.HeaderText = "Provincia"
                grid.Columns.Add(bc11)

                Dim bc12 As New BoundColumn
                bc12.DataField = "Comune"
                bc12.HeaderText = "Comune"
                grid.Columns.Add(bc12)
            End If

            Dim bc7 As New BoundColumn
            bc7.DataField = "VolontariRic"
            bc7.HeaderText = "Volontari Richiesti"
            grid.Columns.Add(bc7)

            Dim bc8 As New BoundColumn
            bc8.DataField = "VolontariEff"
            bc8.HeaderText = "Volontari Effettivi"
            grid.Columns.Add(bc8)

            Dim bc10 As New BoundColumn
            bc10.DataField = "PUNTEGGIO"
            bc10.HeaderText = "Punteggio"
            grid.Columns.Add(bc10)

            Dim bc14 As New BoundColumn
            bc14.DataField = "Limitazioni"
            bc14.HeaderText = "Limitazioni"
            grid.Columns.Add(bc14)

            grid.DataBind()

            ' 2) imposto il tipo di ContentType e la codifica per il Response
            Response.Clear()
            Response.ContentType = "application/vnd.ms-excel"
            Response.ContentEncoding = System.Text.Encoding.Default
            Me.EnableViewState = False

            ' 3) creo l'HtmlTextWriter su cui renderizzo la griglia
            Dim tw As New System.IO.StringWriter
            Dim hw As New System.Web.UI.HtmlTextWriter(tw)
            grid.RenderControl(hw)

            ' 4) ottiene la stringa HTML dallo stream, e lo manda al browser, per essere intrpretato da Excel
            Dim str_tw As String = tw.ToString()
            Response.Write(str_tw)
            Response.Flush()
            Response.Close()
        End If

    End Sub

    Private Sub SetGridLayout(ByRef grid As DataGrid)
        grid.ShowHeader = True
        grid.HeaderStyle.Font.Bold = True
    End Sub

End Class