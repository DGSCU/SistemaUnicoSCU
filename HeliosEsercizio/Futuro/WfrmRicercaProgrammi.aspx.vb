Imports System.Drawing.Printing
Imports System.Drawing.Imaging
Imports System.Web.UI
Imports System.Drawing
Imports System.IO
Imports System.Data.SqlClient
Public Class WfrmRicercaProgrammi
    Inherits System.Web.UI.Page
    Dim dtrGenerico As SqlClient.SqlDataReader
    Dim strquery As String
#Region "Utilità"

    Private Sub ChiudiDataReader(ByRef dataReader As SqlDataReader)
        If Not dataReader Is Nothing Then
            dataReader.Close()
            dataReader = Nothing
        End If
    End Sub

#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not Session("LogIn") Is Nothing Then
            If Session("LogIn") = False Then 'verifico validità log-in
                Response.Redirect("LogOn.aspx")
            End If
        Else
            Response.Redirect("LogOn.aspx")
        End If
        
        If IsPostBack = False Then
            If Session("txtCodEnte") <> "" Then
                txtCodiceEnteProponente.Text = Session("txtCodEnte")
            End If
            Session("DtbRicerca") = Nothing
            CaricaComboAvvisi()
            CaricaCombo()
            CaricaDDLRegioni() ' MEV 224

            ''''' claudio 2022-03
            If Not Request.QueryString("idBP") Is Nothing Then
                CaricaGrigliaRicercaIstanza()
            End If


        End If
    End Sub
    Private Sub CaricaComboDurata()
        Dim strSql As String

        ChiudiDataReader(dtrGenerico)
        strSql = " SELECT 0 as NumMesi,'' as nmesi UNION "
        strSql &= " SELECT nmesi as NumMesi ,convert(varchar,nmesi) as nmesi FROM TipiProgettoDettaglio "

        dtrGenerico = ClsServer.CreaDatareader(strSql, Session("conn"))
        If dtrGenerico.HasRows = True Then
            ddlDurataMesi.DataSource = dtrGenerico
            ddlDurataMesi.DataTextField = "nmesi"
            ddlDurataMesi.DataValueField = "NumMesi"
            ddlDurataMesi.DataBind()
        End If
        ChiudiDataReader(dtrGenerico)
    End Sub
    Private Sub CaricaComboStatoProgramma()
        'variabile stringa locale per costruire la query per i settori
        Dim strSql As String

          ChiudiDataReader(dtrGenerico)
        strSql = " SELECT 0 as IdStatoProgramma,'' as StatoProgramma UNION "
        strSql &= " SELECT IdStatoProgramma as IdStatoProgramma ,convert(varchar,StatoProgramma) as StatoProgramma FROM StatiProgrammi "

        dtrGenerico = ClsServer.CreaDatareader(strSql, Session("conn"))
        If dtrGenerico.HasRows = True Then
            ddlStatoProgramma.DataSource = dtrGenerico
            ddlStatoProgramma.DataTextField = "StatoProgramma"
            ddlStatoProgramma.DataValueField = "IdStatoProgramma"
            ddlStatoProgramma.DataBind()
        End If
        ChiudiDataReader(dtrGenerico)
    End Sub
    Private Sub CaricaComboTerritorio()

        Dim strSql As String

        strSql = "SELECT 0 as IdTerritorio,'' as Descrizione UNION " & _
                 "SELECT IdTerritorio AS IdTerritorio, Descrizione From Territori order by IdTerritorio"
        dtrGenerico = ClsServer.CreaDatareader(strSql, Session("conn"))
        If dtrGenerico.HasRows = True Then
            ddlTerritorio.DataSource = dtrGenerico
            ddlTerritorio.DataTextField = "Descrizione"
            ddlTerritorio.DataValueField = "IdTerritorio"
            ddlTerritorio.DataBind()
        End If
        ChiudiDataReader(dtrGenerico)
    End Sub
    Private Sub CaricaComboObiettivi()
        Dim strSql As String
        strSql = "SELECT 0 as IdObiettivo,'' as Descrizione UNION " & _
                 "SELECT IdObiettivo,Descrizione from obiettivi order by IdObiettivo  "
        dtrGenerico = ClsServer.CreaDatareader(strSql, Session("conn"))
        If dtrGenerico.HasRows = True Then
            ddlObiettivo.DataSource = dtrGenerico
            ddlObiettivo.DataTextField = "Descrizione"
            ddlObiettivo.DataValueField = "IdObiettivo"
            ddlObiettivo.DataBind()
        End If
        ChiudiDataReader(dtrGenerico)

    End Sub
    Private Sub CaricaComboAmbitoAzione()
        Dim strSql As String
        strSql = "SELECT 0 as IdAmbitoAzione,'' as Descrizione UNION " & _
                 "SELECT IdAmbitoAzione,Descrizione from AmbitiAzione order by 1 "
        dtrGenerico = ClsServer.CreaDatareader(strSql, Session("conn"))
        If dtrGenerico.HasRows = True Then
            ddlAmbito.DataSource = dtrGenerico
            ddlAmbito.DataTextField = "Descrizione"
            ddlAmbito.DataValueField = "IdAmbitoAzione"
            ddlAmbito.DataBind()
        End If
        ChiudiDataReader(dtrGenerico)
    End Sub
    Private Sub CaricaComboMacroAmbitoAzione()
        Dim strSql As String
        strSql = "SELECT 0 as IDMacroAmbitoAttività,'' as MacroAmbitoAttività UNION " & _
                 "SELECT IDMacroAmbitoAttività,MacroAmbitoAttività from MacroAmbitiAttività order by 1 "
        dtrGenerico = ClsServer.CreaDatareader(strSql, Session("conn"))
        If dtrGenerico.HasRows = True Then
            ddlMacroAmbito.DataSource = dtrGenerico
            ddlMacroAmbito.DataTextField = "MacroAmbitoAttività"
            ddlMacroAmbito.DataValueField = "IDMacroAmbitoAttività"
            ddlMacroAmbito.DataBind()
        End If
        ChiudiDataReader(dtrGenerico)
    End Sub
    Private Sub CaricaComboTipoProgramma()
        Dim strSql As String
        strSql = "SELECT 0 as IDTipoProgramma,'' as Descrizione UNION " & _
                 "SELECT IdTipoProgramma,Descrizione from TipiProgramma UNION " & _
                 "SELECT 98 as IDTipoProgramma,'Servizio Civile Universale - Italia' UNION " & _
                 "SELECT 99 as IDTipoProgramma,'Servizio Civile Universale - Estero' order by 1 "
        dtrGenerico = ClsServer.CreaDatareader(strSql, Session("conn"))
        If dtrGenerico.HasRows = True Then
            ddlTipoProgramma.DataSource = dtrGenerico
            ddlTipoProgramma.DataTextField = "Descrizione"
            ddlTipoProgramma.DataValueField = "IdTipoProgramma"
            ddlTipoProgramma.DataBind()
        End If
        ChiudiDataReader(dtrGenerico)
    End Sub
    Private Sub CaricaCombo()
        CaricaComboDurata()
        CaricaComboStatoProgramma()
        CaricaComboTerritorio()
        CaricaComboObiettivi()
        CaricaComboAmbitoAzione()
        CaricaComboMacroAmbitoAzione()
        CaricaComboTipoProgramma()
    End Sub
    Protected Sub CmdChiudi_Click(sender As Object, e As EventArgs) Handles CmdChiudi.Click
        
        Response.Redirect("WfrmMain.aspx")
    End Sub
    Protected Sub CmdRicerca_Click(sender As Object, e As EventArgs) Handles CmdRicerca.Click

        'Nasconde pulsante e link per esportare il CSV con il risultato della ricerca
        CmdEsporta.Visible = False
        ApriCSV1.Visible = False

        'Esegue ricerca dati su DB
        CaricaGrigliaRicerca()

        'Visualizza il pulsante esporta se la ricerca ha restituito dei dati 
        If dgRisultatoRicerca.Visible And dgRisultatoRicerca.Items.Count > 0 Then
            CmdEsporta.Visible = True
        End If

    End Sub
    Private Sub dgRisultatoRicerca_ItemCommand(source As Object, e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgRisultatoRicerca.ItemCommand
        Select Case e.CommandName
            Case "Select"
                MaintainScrollPositionOnPostBack = True

                If Session("TipoUtente") = "U" Or Session("TipoUtente") = "R" Then
                    If Session("IdEnte") <> e.Item.Cells(9).Text Then
                        Dim strSql As String
                        strSql = "SELECT enti.Idente,enti.codiceregione, enti.denominazione,RegioniCompetenze.Descrizione from enti INNER JOIN RegioniCompetenze ON  enti.IdRegioneCompetenza = RegioniCompetenze.IdRegioneCompetenza where idente = " & e.Item.Cells(9).Text
                        dtrGenerico = ClsServer.CreaDatareader(strSql, Session("conn"))
                        dtrGenerico.Read()
                        If dtrGenerico.HasRows = True Then
                            Session("IdEnte") = dtrGenerico("IdEnte")
                            Session("Denominazione") = dtrGenerico("Denominazione")
                            Session("Competenza") = dtrGenerico("Descrizione")
                            Session("CodiceRegioneEnte") = "[" & dtrGenerico("Codiceregione") & "]"
                            Session("txtCodEnte") = dtrGenerico("Codiceregione")
                        End If
                        ChiudiDataReader(dtrGenerico)
                    End If
                End If

                'If Session("TipoUtente") = "R" Then
                '    ChiudiDataReader(dtrGenerico)
                'End If

                Response.Redirect("WfrmProgrammi.aspx?idProgramma=" & CInt(e.Item.Cells(3).Text))

            Case "accettazione"
                If Session("tipoutente") <> "E" Then
                    Session("idente") = CInt(e.Item.Cells(9).Text)
                End If

                'Verifico se è possibile effettuare la valutazione del programma
                strquery = "select isnull(b.idstatobandoprogramma,0) as idstatobandoprogramma " & _
                " from programmi a left join bandiprogrammi b on a.idbandoprogramma = b.idbandoprogramma" & _
                " where a.idprogramma=" & CInt(e.Item.Cells(3).Text) & ""
                If Not dtrGenerico Is Nothing Then
                    dtrGenerico.Close()
                    dtrGenerico = Nothing
                End If

                dtrGenerico = ClsServer.CreaDatareader(strquery, Session("conn"))
                dtrGenerico.Read()
                If dtrGenerico("idstatobandoprogramma") = 3 Then
                    If Not dtrGenerico Is Nothing Then
                        dtrGenerico.Close()
                        dtrGenerico = Nothing
                    End If
                    'Response.Redirect("WfrmAlbero.aspx?idattivita=" & CInt(e.Item.Cells(8).Text) & "&tipologia=Progetti&Arrivo=ricercaprogetti.aspx")
                    Response.Redirect("assegnazionevincoliprogrammi.aspx?idprogramma=" & CInt(e.Item.Cells(3).Text) & "&tipologia=Programmi&Vengoda=" & Request.QueryString("VengoDa"))
                Else
                    'Messaggio
                    lblmessaggio.Text = "Non è possibile accedere alla verifica formale del Programma."
                    If Not dtrGenerico Is Nothing Then
                        dtrGenerico.Close()
                        dtrGenerico = Nothing
                    End If
                    Exit Sub
                End If

            Case "valutazione"
                'Verifico se è possibile effettuare la valutazione del programma
                If Session("tipoutente") <> "E" Then
                    Session("idente") = CInt(e.Item.Cells(9).Text)
                End If

                strquery = "select a.idstatoprogramma " & _
                " from programmi a left join bandiprogrammi b on a.idbandoprogramma = b.idbandoprogramma" & _
                " where a.idprogramma=" & CInt(e.Item.Cells(3).Text) & ""
                If Not dtrGenerico Is Nothing Then
                    dtrGenerico.Close()
                    dtrGenerico = Nothing
                End If
                dtrGenerico = ClsServer.CreaDatareader(strquery, Session("conn"))
                dtrGenerico.Read()

                'AMMISSIBILE,ATTIVO,NON ATTIVABILE
                If dtrGenerico("idstatoprogramma") = 3 Or dtrGenerico("idstatoprogramma") = 6 Or dtrGenerico("idstatoprogramma") = 7 Then
                    If Not dtrGenerico Is Nothing Then
                        dtrGenerico.Close()
                        dtrGenerico = Nothing
                    End If

                    Response.Redirect("WfrmValutazioneQualProgrammi.aspx?idprogramma=" & CInt(e.Item.Cells(3).Text))

                Else
                    'Messaggio
                    lblmessaggio.Text = "Non è possibile accedere alla valutazione di merito del Programma."
                    If Not dtrGenerico Is Nothing Then
                        dtrGenerico.Close()
                        dtrGenerico = Nothing
                    End If
                    Exit Sub
                End If
        End Select
    End Sub
    Private Sub dgRisultatoRicerca_PageIndexChanged(source As Object, e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgRisultatoRicerca.PageIndexChanged
        dgRisultatoRicerca.CurrentPageIndex = e.NewPageIndex
        dgRisultatoRicerca.DataSource = Session("appRicercaProgrammi")
        dgRisultatoRicerca.DataBind()
    End Sub
    Protected Sub CmdEsporta_Click(sender As Object, e As EventArgs) Handles CmdEsporta.Click
        CmdEsporta.Visible = False
        StampaCSV(Session("DtbRicerca"))
    End Sub
    Function StampaCSV(ByVal DTBRicerca As DataTable)
        Dim dtrSediAttuazione As Data.SqlClient.SqlDataReader
        Dim Writer As StreamWriter
        Dim xLinea As String
        Dim i As Int64
        Dim j As Int64
        Dim NomeUnivoco As String
        Dim xPrefissoNome As String = vbNullString
        Dim Reader As StreamReader

        If DTBRicerca.Rows.Count = 0 Then
            ApriCSV1.Visible = False
            CmdEsporta.Visible = False
        Else
            xPrefissoNome = Session("Utente")
            NomeUnivoco = xPrefissoNome & "ExpElencoProgrammi" & Year(Now) & Month(Now) & Day(Now) & Hour(Now) & Minute(Now) & Second(Now)
            Writer = New StreamWriter(Server.MapPath("download") & "\" & NomeUnivoco & ".CSV")
            'Creazione dell'inntestazione del CSV
            Dim intNumCol As Int64 = DTBRicerca.Columns.Count
            For i = 0 To intNumCol - 1
                xLinea &= DTBRicerca.Columns.Item(CInt(i)).ColumnName() & ";"
            Next
            Writer.WriteLine(xLinea)
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

                Writer.WriteLine(xLinea)
                xLinea = vbNullString

            Next

            ApriCSV1.Visible = True
            ApriCSV1.NavigateUrl = "download\" & NomeUnivoco & ".CSV"

            Writer.Close()
            Writer = Nothing

        End If

    End Function
    Sub CaricaDataTablePerStampaDinamica(ByVal DataSetDaScorrere As DataSet, ByVal NSkipColonne As Integer)
        Dim dt As New DataTable
        Dim dr As DataRow
        Dim i As Integer
        Dim x As Integer

        'carico i nomi delle colonne che andrò a stampare nella datagrid
        For x = 0 + NSkipColonne To DataSetDaScorrere.Tables(0).Columns.Count - 1
            dt.Columns.Add(New DataColumn(DataSetDaScorrere.Tables(0).Columns.Item(x).ColumnName, GetType(String)))
        Next

        'carico il datatable con il risultato della query della ricerca
        If DataSetDaScorrere.Tables(0).Rows.Count > 0 Then
            For i = 1 To DataSetDaScorrere.Tables(0).Rows.Count
                dr = dt.NewRow()
                For x = 0 To dt.Columns.Count - 1
                    dr(x) = DataSetDaScorrere.Tables(0).Rows.Item(i - 1).Item(x + NSkipColonne)
                Next
                dt.Rows.Add(dr)
            Next
        End If

        'passo alla sessione la datatable che ho appena creato e che userò per il databinding della datagrid della stampa
        Session("DtbRicerca") = dt

    End Sub
    Sub CaricaGrigliaRicerca()
        Dim sqlDAP As New SqlClient.SqlDataAdapter
        Dim dataSet As New DataSet
        Dim strNomeStore As String = "[SP_PROGRAMMI_RICERCA]"

        Try
            sqlDAP = New SqlClient.SqlDataAdapter(strNomeStore, CType(Session("conn"), SqlClient.SqlConnection))
            sqlDAP.SelectCommand.CommandType = CommandType.StoredProcedure
            sqlDAP.SelectCommand.Parameters.Add("@UserName", SqlDbType.VarChar).Value = Session("Utente")
            sqlDAP.SelectCommand.Parameters.Add("@TitoloProgramma", SqlDbType.VarChar).Value = txtTitoloProgramma.Text.Replace("'", "''")
            sqlDAP.SelectCommand.Parameters.Add("@CodiceProgramma", SqlDbType.VarChar).Value = TxtCodPog.Text.Replace("'", "''")
            sqlDAP.SelectCommand.Parameters.Add("@CodiceEnteProponente", SqlDbType.VarChar).Value = txtCodiceEnteProponente.Text.Replace("'", "''")
            sqlDAP.SelectCommand.Parameters.Add("@DenominazioneEnteProponente", SqlDbType.VarChar).Value = txtEnteProponente.Text.Replace("'", "''")
            sqlDAP.SelectCommand.Parameters.Add("@IdStatoProgramma", SqlDbType.Int).Value = ddlStatoProgramma.SelectedValue
            sqlDAP.SelectCommand.Parameters.Add("@CoProgrammazione", SqlDbType.VarChar).Value = ddlCoprogrammazione.SelectedItem.Text
            sqlDAP.SelectCommand.Parameters.Add("@DurataMesi", SqlDbType.Int).Value = ddlDurataMesi.SelectedValue
            sqlDAP.SelectCommand.Parameters.Add("@IdObiettivo", SqlDbType.Int).Value = ddlObiettivo.SelectedValue
            sqlDAP.SelectCommand.Parameters.Add("@IdAmbitoAzione", SqlDbType.Int).Value = ddlAmbito.SelectedValue
            sqlDAP.SelectCommand.Parameters.Add("@IdTerritorio", SqlDbType.Int).Value = ddlTerritorio.SelectedValue
            sqlDAP.SelectCommand.Parameters.Add("@IdMacroAmbitoAttività", SqlDbType.Int).Value = ddlMacroAmbito.SelectedValue
            sqlDAP.SelectCommand.Parameters.Add("@Reti", SqlDbType.VarChar).Value = ddlReti.SelectedItem.Text
            sqlDAP.SelectCommand.Parameters.Add("@IdTipoProgramma", SqlDbType.Int).Value = ddlTipoProgramma.SelectedValue
            sqlDAP.SelectCommand.Parameters.Add("@IdBando", SqlDbType.Int).Value = DdlBando.SelectedValue
            sqlDAP.SelectCommand.Parameters.Add("@IdRegione", SqlDbType.Int).Value = ddlRegioni.SelectedValue 'MEV 224
            sqlDAP.Fill(dataSet)

            Session("appRicercaProgrammi") = dataSet
            dgRisultatoRicerca.CurrentPageIndex = 0
            dgRisultatoRicerca.DataSource = Session("appRicercaProgrammi")
            dgRisultatoRicerca.DataBind()


            'il numero 2 indica quante colonne iniziali non devono essere messe nel dataset per l'esportazione in csv
            CaricaDataTablePerStampaDinamica(dataSet, 2)

            '******************************************************
            'Verifico le Abilitazione Dell'Utente
            strquery = "SELECT AssociaUtenteGruppo.username, VociMenu.IdVoceMenu,VociMenu.descrizione, VociMenu.VoceMenu, VociMenu.ImmaginePassiva, VociMenu.ImmagineAttiva, VociMenu.Link,"
            strquery = strquery & " VociMenu.IdVoceMenuPadre"
            strquery = strquery & " FROM VociMenu"
            strquery = strquery & " INNER JOIN AbilitazioniProfiliVociMenu ON VociMenu.IdVoceMenu = AbilitazioniProfiliVociMenu.IdVoceMenu"
            strquery = strquery & " INNER JOIN	Profili ON AbilitazioniProfiliVociMenu.IdProfilo = Profili.IdProfilo"
            '============================================================================================================================
            '====================================================30/09/2008==============================================================
            '=======MODIFICA EFFETTUATA DA Kronk (99 scimmie saltavano sul letto, una cadde in terra e si ruppe il cervelletto...)=======
            '=================AGGIUNTA JOIN SU PROFILO READ PER ABILITARE LEGGERE LE ABILITAZIONI ANCHE DELL'UTENTE READ=================
            '============================================================================================================================
            If Session("Read") <> "1" Then
                strquery = strquery & " INNER JOIN	AssociaUtenteGruppo ON Profili.IdProfilo = AssociaUtenteGruppo.IdProfilo "
            Else
                strquery = strquery & " INNER JOIN	AssociaUtenteGruppo ON Profili.IdProfilo = AssociaUtenteGruppo.IdProfiloRead "
            End If
            strquery = strquery & " LEFT JOIN 	RestrizioniUtenteVociMenu ON AssociaUtenteGruppo.UserName = RestrizioniUtenteVociMenu.UserName and RestrizioniUtenteVociMenu.IdVoceMenu=VociMenu.IdVoceMenu"
            strquery = strquery & " WHERE (VociMenu.descrizione = 'Accettazione Programmi'or VociMenu.descrizione = 'Valutazione Qualità Programmi')"
            strquery = strquery & " AND AssociaUtenteGruppo.username ='" & Session("Utente") & "' and (RestrizioniUtenteVociMenu.TipoRestrizione <> 0 or RestrizioniUtenteVociMenu.TipoRestrizione is null)"

            If Not dtrGenerico Is Nothing Then
                dtrGenerico.Close()
                dtrGenerico = Nothing
            End If

            dtrGenerico = ClsServer.CreaDatareader(strquery, Session("conn"))

            Do While dtrGenerico.Read
                Select Case dtrGenerico("descrizione")
                    Case "Accettazione Programmi"
                        dgRisultatoRicerca.Columns(1).Visible = True
                    Case "Valutazione Qualità Programmi"
                        dgRisultatoRicerca.Columns(2).Visible = True
                End Select
            Loop
            If Not dtrGenerico Is Nothing Then
                dtrGenerico.Close()
                dtrGenerico = Nothing
            End If
            '***********************************
        Catch ex As Exception
            Response.Write(ex.Message.ToString())
            Exit Sub
        End Try
    End Sub
    Sub CaricaGrigliaRicercaIstanza()
        Dim sqlDAP As New SqlClient.SqlDataAdapter
        Dim dataSet As New DataSet
        Dim strNomeStore As String = "[SP_PROGRAMMI_RICERCA_ISTANZA]"

        Try
            sqlDAP = New SqlClient.SqlDataAdapter(strNomeStore, CType(Session("conn"), SqlClient.SqlConnection))
            sqlDAP.SelectCommand.CommandType = CommandType.StoredProcedure
            sqlDAP.SelectCommand.Parameters.Add("@UserName", SqlDbType.VarChar).Value = Session("Utente")
            sqlDAP.SelectCommand.Parameters.Add("@IdBandoProgramma", SqlDbType.Int).Value = Request.QueryString("idBP")
          
            sqlDAP.Fill(dataSet)

            Session("appRicercaProgrammi") = dataSet
            dgRisultatoRicerca.DataSource = Session("appRicercaProgrammi")
            dgRisultatoRicerca.DataBind()
            CmdEsporta.Visible = True



            'il numero 2 indica quante colonne iniziali non devono essere messe nel dataset per l'esportazione in csv
            CaricaDataTablePerStampaDinamica(dataSet, 2)

            '******************************************************
            'Verifico le Abilitazione Dell'Utente
            strquery = "SELECT AssociaUtenteGruppo.username, VociMenu.IdVoceMenu,VociMenu.descrizione, VociMenu.VoceMenu, VociMenu.ImmaginePassiva, VociMenu.ImmagineAttiva, VociMenu.Link,"
            strquery = strquery & " VociMenu.IdVoceMenuPadre"
            strquery = strquery & " FROM VociMenu"
            strquery = strquery & " INNER JOIN AbilitazioniProfiliVociMenu ON VociMenu.IdVoceMenu = AbilitazioniProfiliVociMenu.IdVoceMenu"
            strquery = strquery & " INNER JOIN	Profili ON AbilitazioniProfiliVociMenu.IdProfilo = Profili.IdProfilo"
            '============================================================================================================================
            '====================================================30/09/2008==============================================================
            '=======MODIFICA EFFETTUATA DA Kronk (99 scimmie saltavano sul letto, una cadde in terra e si ruppe il cervelletto...)=======
            '=================AGGIUNTA JOIN SU PROFILO READ PER ABILITARE LEGGERE LE ABILITAZIONI ANCHE DELL'UTENTE READ=================
            '============================================================================================================================
            If Session("Read") <> "1" Then
                strquery = strquery & " INNER JOIN	AssociaUtenteGruppo ON Profili.IdProfilo = AssociaUtenteGruppo.IdProfilo "
            Else
                strquery = strquery & " INNER JOIN	AssociaUtenteGruppo ON Profili.IdProfilo = AssociaUtenteGruppo.IdProfiloRead "
            End If
            strquery = strquery & " LEFT JOIN 	RestrizioniUtenteVociMenu ON AssociaUtenteGruppo.UserName = RestrizioniUtenteVociMenu.UserName and RestrizioniUtenteVociMenu.IdVoceMenu=VociMenu.IdVoceMenu"
            strquery = strquery & " WHERE (VociMenu.descrizione = 'Accettazione Programmi'or VociMenu.descrizione = 'Valutazione Qualità Programmi')"
            strquery = strquery & " AND AssociaUtenteGruppo.username ='" & Session("Utente") & "' and (RestrizioniUtenteVociMenu.TipoRestrizione <> 0 or RestrizioniUtenteVociMenu.TipoRestrizione is null)"

            If Not dtrGenerico Is Nothing Then
                dtrGenerico.Close()
                dtrGenerico = Nothing
            End If

            dtrGenerico = ClsServer.CreaDatareader(strquery, Session("conn"))

            Do While dtrGenerico.Read
                Select Case dtrGenerico("descrizione")
                    Case "Accettazione Programmi"
                        dgRisultatoRicerca.Columns(1).Visible = True
                    Case "Valutazione Qualità Programmi"
                        dgRisultatoRicerca.Columns(2).Visible = True
                End Select
            Loop
            If Not dtrGenerico Is Nothing Then
                dtrGenerico.Close()
                dtrGenerico = Nothing
            End If
            '***********************************
        Catch ex As Exception
            Response.Write(ex.Message.ToString())
            Exit Sub
        End Try
    End Sub
    Sub ImpostaFiltri()
        Dim Ente As String = Request.QueryString("Ente")
        Dim Bando As Integer = CInt(Request.QueryString("Bando"))
        Dim Anno As Integer = CInt(Request.QueryString("Anno"))
        Dim StatoIstanza As Integer = CInt(Request.QueryString("StatoIstanza"))
        Dim Competenza As Integer = CInt(Request.QueryString("Competenza"))
    End Sub
    Private Sub CaricaComboAvvisi()
        ChiudiDataReader(dtrGenerico)
        'strquery = " SELECT Bando.idBando,bando.bandobreve,bando.annobreve  "
        'strquery = strquery & " FROM bando"
        'strquery = strquery & " WHERE Programmi =1 "
        'strquery = strquery & " UNION "
        'strquery = strquery & " SELECT  '0',' TUTTI ', 99  from bando "
        'strquery = strquery & " ORDER BY Bando.annobreve desc"

        strquery = "SELECT Bando.idBando,bando.bandobreve,bando.annobreve  "
        strquery = strquery & " FROM bando"
        strquery = strquery & " INNER JOIN AssociaBandoTipiProgetto abtp on abtp.idbando =  bando.idbando"
        strquery = strquery & " INNER JOIN TipiProgetto  tp on abtp.idtipoprogetto = tp.idtipoprogetto"
        strquery = strquery & " WHERE tp.MacroTipoProgetto like '" & Session("FiltroVisibilita") & "' and bando.programmi = 1"
        strquery = strquery & " UNION "
        strquery = strquery & " SELECT  '0','','9999'  from bando "
        strquery = strquery & " ORDER BY Bando.annobreve desc ,Bando.idbando"
        If Not dtrGenerico Is Nothing Then
            dtrGenerico.Close()
            dtrGenerico = Nothing
        End If

        dtrGenerico = ClsServer.CreaDatareader(strquery, Session("conn"))
        DdlBando.DataSource = dtrGenerico
        DdlBando.DataTextField = "bandobreve"
        DdlBando.DataValueField = "idbando"
        DdlBando.DataBind()

        ChiudiDataReader(dtrGenerico)
    End Sub

    ''' <summary>
    ''' Carica l'elenco delle regioni/provincie autonome + estero che hanno programmi con almeno una sede nella regione/provincia autonoma dell'elenco (MEV 224)
    ''' </summary>
    Private Sub CaricaDDLRegioni()

        'Query SQL per estrarre lista
        'strquery = " SELECT DISTINCT "
        'strquery &= " CASE WHEN RegioniCompetenze.Descrizione IS NULL THEN 'Estero' ELSE 'Italia' END AS Optgroup "
        'strquery &= " , regioni.IDRegione "
        'strquery &= " , regioni.Regione "
        'strquery &= " From Programmi "
        'strquery &= " INNER JOIN attività                   ON Programmi.IdProgramma = attività.IdProgramma "
        'strquery &= " INNER JOIN attivitàentisediattuazione ON attività.IDAttività = attivitàentisediattuazione.IDAttività "
        'strquery &= " INNER JOIN entisediattuazioni         ON attivitàentisediattuazione.IDEnteSedeAttuazione = entisediattuazioni.IDEnteSedeAttuazione "
        'strquery &= " INNER JOIN entisedi                   ON entisediattuazioni.IDEnteSede = entisedi.IDEnteSede "
        'strquery &= " INNER JOIN comuni                     ON entisedi.IDComune = comuni.IDComune "
        'strquery &= " INNER JOIN provincie                  ON comuni.IDProvincia = provincie.IDProvincia "
        'strquery &= " LEFT  JOIN RegioniCompetenze          ON provincie.IdRegioneCompetenza = RegioniCompetenze.IdRegioneCompetenza "
        'strquery &= " INNER JOIN regioni                    ON provincie.IDRegione = regioni.IDRegione "
        'strquery &= " INNER JOIN TipiProgetto               ON attività.IdTipoProgetto = TipiProgetto.IdTipoProgetto		"
        'strquery &= " WHERE COMUNI.ComuneNazionale = TipiProgetto.NazioneBase"
        'strquery &= " ORDER BY 1, 3 "

        strquery = " select 1 as Ordine,IdRegioneCompetenza, Descrizione "
        strquery &= " from RegioniCompetenze"
        strquery &= " where Descrizione <> 'Nazionale'"
        strquery &= " union "
        strquery &= " select 99,99, 'Estero'"
        strquery &= " order by Ordine,Descrizione"

        'Aggiunge un elemento vuoto ad inizio lista
        Me.ddlRegioni.Items.Add(New ListItem(String.Empty, "0"))

        Try
            ChiudiDataReader(dtrGenerico)

            'Lettura dati
            dtrGenerico = ClsServer.CreaDatareader(strquery, Session("conn"))

            'Carica la lista
            While dtrGenerico.Read()

                Me.ddlRegioni.Items.Add(New ListItem(dtrGenerico("Descrizione").ToString(), dtrGenerico("IdRegioneCompetenza").ToString()))

            End While

        Catch ex As Exception

            Me.ddlRegioni.Items.Clear()
            Me.ddlRegioni.Items.Add(New ListItem(String.Empty, "0"))

        Finally

            ChiudiDataReader(dtrGenerico)

        End Try


    End Sub



End Class