Imports System.IO
Public Class WfrmCheckListDettaglio
    Inherits System.Web.UI.Page
    Dim idlista As Integer
    Dim Regione As String
    Dim anno As String
    Dim mese As String
    Dim statoCheckList As String
    Dim idStatolista As Integer
    Dim CodCheckList As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Call TuttaPaginaSess()
        If Not Session("LogIn") Is Nothing Then
            If Session("LogIn") = False Then
                Response.Redirect("LogOn.aspx")
            End If
        Else
            Response.Redirect("LogOn.aspx")
        End If
        
        idStatolista = CaricaDati()

        If Page.IsPostBack = False Then

            'if stato inizializzato

            Select Case idStatolista

                Case 1 'da lavorare (Inizializzazione)
                    cmdInizializza.Visible = True
                    cmdRicerca.Visible = False
                    CmdConferma.Visible = False
                    CmdEsporta.Visible = False
                    CmdStampa.Visible = False

                Case 2  'in lavorazione

                    RicercaVolontariLista()
                    cmdInizializza.Visible = False
                    cmdRicerca.Visible = True
                    CmdConferma.Visible = True
                    CmdEsporta.Visible = True
                    CmdStampa.Visible = False '(stampo solo dopo conferma)
                Case 3 'Confermata
                    RicercaVolontariLista()
                    cmdInizializza.Visible = False
                    cmdRicerca.Visible = True
                    CmdConferma.Visible = False
                    CmdEsporta.Visible = True
                    CmdStampa.Visible = True '(stampo solo dopo conferma)
            End Select
            If Request.QueryString("menu") Is Nothing Then
                If Not Request.Cookies("SUSCNRicChkPagDett") Is Nothing Then
                    RitornaParametri()
                End If
            Else
                Response.Cookies("SUSCNRicChkPagDett")("Page") = 0
                Response.Cookies("SUSCNRicChkPagDett").Expires = DateTime.Now.AddDays(1)
            End If
        End If


    End Sub
    Private Sub RicercaVolontariLista()

        Dim sqlDAP As New SqlClient.SqlDataAdapter
        Dim dataSet As New DataSet
        Dim strNomeStore As String = "[SP_CHECKLIST_PRESENZE_DETTAGLIO]"
        dgRisultatoRicerca.CurrentPageIndex = 0
        '@CodiceVolontario = N'',
        '@Cognome = N'',
        '@Nome = N'',
        '@Esito = N'',
        '@Inclusione = N''

        Try
            sqlDAP = New SqlClient.SqlDataAdapter(strNomeStore, CType(Session("conn"), SqlClient.SqlConnection))
            sqlDAP.SelectCommand.CommandType = CommandType.StoredProcedure
            sqlDAP.SelectCommand.Parameters.Add("@IdCheckList", SqlDbType.Int).Value = (Request.QueryString("IdLista"))
            sqlDAP.SelectCommand.Parameters.Add("@CodiceVolontario", SqlDbType.VarChar).Value = txtCodVol.Text.Replace("'", "''")
            sqlDAP.SelectCommand.Parameters.Add("@Cognome", SqlDbType.VarChar).Value = txtCognome.Text.Replace("'", "''")
            sqlDAP.SelectCommand.Parameters.Add("@Nome", SqlDbType.VarChar).Value = txtNome.Text.Replace("'", "''")
            sqlDAP.SelectCommand.Parameters.Add("@Esito", SqlDbType.VarChar).Value = CboEsito.SelectedValue.Replace("'", "''")
            sqlDAP.SelectCommand.Parameters.Add("@Inclusione", SqlDbType.VarChar).Value = CboIncluso.SelectedValue.Replace("'", "''")

            sqlDAP.Fill(dataSet)


            CaricaDataTablePerStampa(dataSet)
            Session("appDtsRisRicerca") = dataSet
            ViewState("datasource") = dataSet

            dgRisultatoRicerca.DataSource = dataSet
            dgRisultatoRicerca.DataBind()
            If dgRisultatoRicerca.Items.Count > 0 Then
                CmdEsporta.Visible = True
            Else
                CmdEsporta.Visible = False
            End If

        Catch ex As Exception
            Response.Write(ex.Message.ToString())
            Exit Sub
        End Try
    End Sub
    Protected Sub cmdRicerca_Click(sender As Object, e As EventArgs) Handles cmdRicerca.Click
        RicercaVolontariLista()
    End Sub
    Protected Sub OpenWindow(ByVal identita As Integer, ByVal idlista As Integer)

        Dim url As String = "WfrmCheckListNotificaMailPresenze.aspx?IdEntita=" & identita & "&idLista=" & idlista & "&VengoDa=" & 1

        Dim s As String = "window.open('" & url + "', 'popup_window', 'width=800,height=600,left=100,top=100,resizable=yes');"

        ClientScript.RegisterStartupScript(Me.GetType(), "script", s, True)

    End Sub

    Private Sub dgRisultatoRicerca_ItemCommand(source As Object, e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgRisultatoRicerca.ItemCommand
        Dim Ritorno As String()

        If e.CommandName = "Notifica" Then
            RicordaParametri()
            If (Session("TipoUtente") = "U") Then
                Session("IdEnte") = e.Item.Cells(23).Text
                Session("Denominazione") = e.Item.Cells(24).Text
                OpenWindow(e.Item.Cells(0).Text, idlista)

                'Response.Redirect("~/WfrmCAPUtility.aspx?IdEntita=" & e.Item.Cells(0).Text & "&VengoDa=" & 1)
            Else
                Response.Redirect("page_error.aspx")
            End If
        End If

        If e.CommandName = "Documenti" Then
            RicordaParametri()
            If (Session("TipoUtente") = "U") Then
                Session("IdEnte") = e.Item.Cells(23).Text
                Session("Denominazione") = e.Item.Cells(24).Text
                Response.Redirect("~/WfrmVisualizzaElencoDocumentiVolontario.aspx?IdVol=" & e.Item.Cells(0).Text & "&ProVengoDa=" & 1 & "&idLista=" & idlista)
            Else
                Response.Redirect("page_error.aspx")
            End If
        End If
        If e.CommandName = "Presenze" Then
            RicordaParametri()
            Session("IdEnte") = e.Item.Cells(23).Text
            Session("Denominazione") = e.Item.Cells(24).Text
            If (Session("TipoUtente") = "U") Then
                Response.Redirect("~/Presenze.aspx?IdEntita=" & e.Item.Cells(0).Text & "&VengoDa=" & 1 & "&anno=" & anno & "&mese=" & mese & "&idLista=" & idlista)
            Else
                Response.Redirect("page_error.aspx")
            End If
        End If
        If e.CommandName = "Includi" Then
            RicordaParametri()
            Ritorno = UpdateChecListPresenzeIncludi(e.Item.Cells(0).Text)
            If Ritorno(0) = "POSITIVO" Then
                RicercaVolontariLista()
                lblmessaggio.Text = Ritorno(1)
                RitornaParametri()

                'Dim PulsanteLink As ImageButton
                'Dim clientid As String
                'If PulsanteLink.Visible = False Then
                '    PulsanteLink = e.Item.Cells(17).FindControl("CmdEscludiNo")
                '    clientid = PulsanteLink.ClientID
                '    e.Item.Cells(17).FindControl("CmdEscludiNo").Visible = False

                '    PulsanteLink.Visible = True
                '    PulsanteLink.ImageUrl = "~/Images/selezionato_small.png"
                '    PulsanteLink.CommandName = "Escludi"
                'Else
                '    PulsanteLink.Visible = False
                '    e.Item.Cells(17).FindControl("CmdEscludiNo").Visible = True
                'End If


            Else 'negativo
                lblmessaggio.Text = Ritorno(1)
            End If



        End If
        If e.CommandName = "Escludi" Then
            RicordaParametri()
            Ritorno = UpdateChecListPresenzeEscludi(e.Item.Cells(0).Text)
            If Ritorno(0) = "POSITIVO" Then
                RicercaVolontariLista()
                lblmessaggio.Text = Ritorno(1)
                RitornaParametri()
                'Dim PulsanteLink As ImageButton

                'If PulsanteLink.Visible = False Then
                '    PulsanteLink = e.Item.Cells(17).FindControl("CmdIncludiSi")
                '    e.Item.Cells(17).FindControl("CmdIncludiSi").Visible = False
                '    PulsanteLink.Visible = True
                '    PulsanteLink.ImageUrl = "~/Images/deselezionato_small.png"
                '    PulsanteLink.CommandName = "Includi"
                'Else
                '    PulsanteLink.Visible = False
                '    e.Item.Cells(17).FindControl("CmdIncludiSi").Visible = True

                'End If

            Else 'negativo
                lblmessaggio.Text = Ritorno(1)
            End If

        End If

       
    End Sub
    Private Sub dgRisultatoRicerca_PageIndexChanged(source As Object, e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgRisultatoRicerca.PageIndexChanged
        Try
            dgRisultatoRicerca.CurrentPageIndex = e.NewPageIndex
            dgRisultatoRicerca.DataSource = Session("appDtsRisRicerca")
            dgRisultatoRicerca.DataBind()
            dgRisultatoRicerca.SelectedIndex = -1
            RicordaParametri()
        Catch
            dgRisultatoRicerca.CurrentPageIndex = e.NewPageIndex - 1
            dgRisultatoRicerca.DataSource = Session("appDtsRisRicerca")
            dgRisultatoRicerca.DataBind()
            dgRisultatoRicerca.SelectedIndex = -1
            RicordaParametri()
        End Try
        
    End Sub
#Region "SortCommand"
    Private Sub dgRisultatoRicerca_SortCommand(source As Object, e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles dgRisultatoRicerca.SortCommand
        If Not IsNothing(ViewState("datasource")) Then
            ViewState("sortExpression") = e.SortExpression
            dgRisultatoRicerca.DataSource = ReturnDataSource(True)
            dgRisultatoRicerca.DataBind()
        End If
    End Sub
    Protected Function ReturnDataSource(ByVal ReverseSort As Boolean) As DataView
        Dim ds As DataSet = ViewState("datasource")
        Dim dv As DataView = New DataView(ds.Tables(0))
        If Not ViewState("sortExpression") Is Nothing Then
            dv.Sort = String.Format("{0} {1}", ViewState("sortExpression").ToString(), GetSortDirection(ViewState("sortExpression").ToString(), ReverseSort))
        End If
        Return dv

    End Function
    Public Function GetSortDirection(ByVal SortExpression As String, ByVal Reverse As Boolean) As String
        If ViewState(SortExpression) Is Nothing Then
            ViewState(SortExpression) = "Desc"
        Else
            If Reverse = True Then
                ViewState(SortExpression) = If(ViewState(SortExpression).ToString() = "Desc", "Asc", "Desc")
            End If
        End If
        Return ViewState(SortExpression).ToString()
    End Function
#End Region
    Public Sub TuttaPaginaSess()
        Session("TP") = True
    End Sub
    Protected Sub CmdEsporta_Click(sender As Object, e As EventArgs) Handles CmdEsporta.Click
        CmdEsporta.Visible = False
        Dim dtbRicerca As DataTable = Session("DtbRicerca")
        StampaCSV(dtbRicerca)
    End Sub
    Protected Sub cmdChiudi_Click(sender As Object, e As EventArgs) Handles cmdChiudi.Click
        Response.Redirect("WfrmCheckListRicerca.aspx")
    End Sub
    Protected Sub CmdConferma_Click(sender As Object, e As EventArgs) Handles CmdConferma.Click
        Dim sqlCMD As New SqlClient.SqlCommand
        Dim strNomeStore As String = "[SP_CHECKLIST_PRESENZE_CONFERMA]"

        Try
            Dim x As String
            Dim y As String

            sqlCMD = New SqlClient.SqlCommand(strNomeStore, CType(Session("conn"), SqlClient.SqlConnection))
            sqlCMD.CommandType = CommandType.StoredProcedure

            sqlCMD.Parameters.Add("@IdCheckList", SqlDbType.Int).Value = Request.QueryString("idLista")
            sqlCMD.Parameters.Add("@Username", SqlDbType.VarChar).Value = Session("Utente")


            Dim sparam1 As SqlClient.SqlParameter
            sparam1 = New SqlClient.SqlParameter
            sparam1.ParameterName = "@Esito"
            sparam1.Size = 100
            sparam1.SqlDbType = SqlDbType.NVarChar
            sparam1.Direction = ParameterDirection.Output
            sqlCMD.Parameters.Add(sparam1)

            Dim sparam2 As SqlClient.SqlParameter
            sparam2 = New SqlClient.SqlParameter
            sparam2.ParameterName = "@Messaggio"
            sparam2.Size = 1000
            sparam2.SqlDbType = SqlDbType.NVarChar
            sparam2.Direction = ParameterDirection.Output
            sqlCMD.Parameters.Add(sparam2)


            sqlCMD.ExecuteScalar()
            'Dim str As String
            'str = sqlCMD.Parameters("@Messaggio").Value

            x = CStr(sqlCMD.Parameters("@Esito").Value)
            y = sqlCMD.Parameters("@Messaggio").Value


            If x = "POSITIVO" Then

                RicercaVolontariLista()
                CaricaDati()
                cmdInizializza.Visible = False
                cmdRicerca.Visible = True
                CmdConferma.Visible = False
                CmdEsporta.Visible = True
                CmdStampa.Visible = True '(stampo solo dopo conferma)

            End If

            lblmessaggio.Text = y


        Catch ex As Exception


            Response.Write(ex.Message.ToString())
            Exit Sub
        End Try
    End Sub
    Protected Sub CmdStampa_Click(sender As Object, e As EventArgs) Handles CmdStampa.Click
        Response.Write("<script type=""text/javascript"">")
        Response.Write("window.open(""WfrmReportistica.aspx?sTipoStampa=44&IdCheckList=" & Request.QueryString("idLista") & """, ""Report"", ""height=800,width=800, ,dependent=no,scrollbars=no,status=no,resizable=yes"")")
        Response.Write("</script>")
    End Sub
    Private Function UpdateChecListPresenzeIncludi(entita As Integer) As String()

        Dim sqlCMD As New SqlClient.SqlCommand
        Dim strNomeStore As String = "[SP_CHECKLIST_PRESENZE_INCLUDI]"

        Try
            Dim x As String
            Dim y As String
            Dim ArreyOutPut(1) As String
            sqlCMD = New SqlClient.SqlCommand(strNomeStore, CType(Session("conn"), SqlClient.SqlConnection))
            sqlCMD.CommandType = CommandType.StoredProcedure

            sqlCMD.Parameters.Add("@IdCheckList", SqlDbType.Int).Value = Request.QueryString("idLista")
            sqlCMD.Parameters.Add("@IdEntita", SqlDbType.Int).Value = entita
            sqlCMD.Parameters.Add("@Username", SqlDbType.VarChar).Value = Session("Utente")


            Dim sparam1 As SqlClient.SqlParameter
            sparam1 = New SqlClient.SqlParameter
            sparam1.ParameterName = "@Esito"
            sparam1.Size = 100
            sparam1.SqlDbType = SqlDbType.NVarChar
            sparam1.Direction = ParameterDirection.Output
            sqlCMD.Parameters.Add(sparam1)

            Dim sparam2 As SqlClient.SqlParameter
            sparam2 = New SqlClient.SqlParameter
            sparam2.ParameterName = "@Messaggio"
            sparam2.Size = 100
            sparam2.SqlDbType = SqlDbType.NVarChar
            sparam2.Direction = ParameterDirection.Output
            sqlCMD.Parameters.Add(sparam2)


            sqlCMD.ExecuteScalar()
            'Dim str As String
            'str = sqlCMD.Parameters("@Messaggio").Value

            x = CStr(sqlCMD.Parameters("@Esito").Value)
            y = sqlCMD.Parameters("@Messaggio").Value


            ArreyOutPut(0) = x
            ArreyOutPut(1) = y

            Return ArreyOutPut

        Catch ex As Exception
            Dim ArreyOutPut1(1) As String
            ArreyOutPut1(0) = "0"
            ArreyOutPut1(1) = "Contattare l'assistenza Helios/Futuro"
            Return ArreyOutPut1
            Response.Write(ex.Message.ToString())
            Exit Function
        End Try
    End Function
    Private Function UpdateChecListPresenzeEscludi(entita As Integer) As String()

        Dim sqlCMD As New SqlClient.SqlCommand
        Dim strNomeStore As String = "[SP_CHECKLIST_PRESENZE_ESCLUDI]"

        Try
            Dim x As String
            Dim y As String
            Dim ArreyOutPut(1) As String
            sqlCMD = New SqlClient.SqlCommand(strNomeStore, CType(Session("conn"), SqlClient.SqlConnection))
            sqlCMD.CommandType = CommandType.StoredProcedure

            sqlCMD.Parameters.Add("@IdCheckList", SqlDbType.Int).Value = Request.QueryString("idLista")
            sqlCMD.Parameters.Add("@IdEntita", SqlDbType.Int).Value = entita
            sqlCMD.Parameters.Add("@Username", SqlDbType.VarChar).Value = Session("Utente")


            Dim sparam1 As SqlClient.SqlParameter
            sparam1 = New SqlClient.SqlParameter
            sparam1.ParameterName = "@Esito"
            sparam1.Size = 100
            sparam1.SqlDbType = SqlDbType.NVarChar
            sparam1.Direction = ParameterDirection.Output
            sqlCMD.Parameters.Add(sparam1)

            Dim sparam2 As SqlClient.SqlParameter
            sparam2 = New SqlClient.SqlParameter
            sparam2.ParameterName = "@Messaggio"
            sparam2.Size = 100
            sparam2.SqlDbType = SqlDbType.NVarChar
            sparam2.Direction = ParameterDirection.Output
            sqlCMD.Parameters.Add(sparam2)


            sqlCMD.ExecuteScalar()
            'Dim str As String
            'str = sqlCMD.Parameters("@Messaggio").Value

            x = CStr(sqlCMD.Parameters("@Esito").Value)
            y = sqlCMD.Parameters("@Messaggio").Value


            ArreyOutPut(0) = x
            ArreyOutPut(1) = y

            Return ArreyOutPut

        Catch ex As Exception
            Dim ArreyOutPut1(1) As String
            ArreyOutPut1(0) = "0"
            ArreyOutPut1(1) = "Contattare l'assistenza Helios/Futuro"
            Return ArreyOutPut1
            Response.Write(ex.Message.ToString())
            Exit Function
        End Try
    End Function
    Private Function CaricoLista(idlista)
        Dim strsql As String
        Dim MyDataset As DataSet
        strsql = "SELECT IdCheckList,CodiceProgettoRendicontazione,attività.titolo, enti.denominazione + ' (' + enti.codiceregione + ')' as Ente ,AnnoCompetenza,MeseCompetenza,StatoCheckList,CheckListPagheCollettivo.idStatoCheckList,'C' + CONVERT(VARCHAR,CheckListPagheCollettivo.idCheckList) AS CodiceCheckList FROM  CheckListPagheCollettivo INNER JOIN StatiCheckList on CheckListPagheCollettivo.idstatochecklist = StatiCheckList.idstatochecklist inner join attività on CheckListPagheCollettivo.codiceprogettorendicontazione = attività.codiceente inner join enti on attività.identepresentante = enti.idente where IdCheckList=" & idlista
        MyDataset = ClsServer.DataSetGenerico(strsql, Session("conn"))

        If MyDataset.Tables(0).Rows.Count <> 0 Then
            Return MyDataset
        End If
    End Function
    Sub CaricaDataTablePerStampa(ByVal DataSetDaScorrere As DataSet)
        Dim dt As New DataTable
        Dim dr As DataRow
        Dim i As Integer
        Dim x As Integer

        Dim NomeColonne(18) As String
        Dim NomiCampiColonne(18) As String

        'nome della colonna 
        'e posizione nella griglia di lettura
        NomeColonne(0) = "Codice Volontario"
        NomeColonne(1) = "Nominativo"
        NomeColonne(2) = "Codice Fiscale"
        NomeColonne(3) = "Inizio Servizio"
        NomeColonne(4) = "Contratto"
        NomeColonne(5) = "IBAN"
        NomeColonne(6) = "Foglio Presenze"
        NomeColonne(7) = "N P"
        NomeColonne(8) = "N MAL"
        NomeColonne(9) = "N PR"
        NomeColonne(10) = "Ass Cons"
        NomeColonne(11) = "N MAL Tot"
        NomeColonne(12) = "N PR Tot"
        NomeColonne(13) = "Ass Decur"
        NomeColonne(14) = "N Decur"
        NomeColonne(15) = "Cons Doc"
        NomeColonne(16) = "Mesi Servizio"
        NomeColonne(17) = "Senza Sanzioni"
        NomeColonne(18) = "Incl"

        NomiCampiColonne(0) = "CodiceVolontario"
        NomiCampiColonne(1) = "Nominativo"
        NomiCampiColonne(2) = "CodiceFiscale"
        NomiCampiColonne(3) = "InizioServizio"
        NomiCampiColonne(4) = "Contratto"
        NomiCampiColonne(5) = "IBAN"
        NomiCampiColonne(6) = "FoglioPresenze"
        NomiCampiColonne(7) = "NP"
        NomiCampiColonne(8) = "NMAL"
        NomiCampiColonne(9) = "NPR"
        NomiCampiColonne(10) = "AssCons"
        NomiCampiColonne(11) = "NMALTot"
        NomiCampiColonne(12) = "NPRTot"
        NomiCampiColonne(13) = "AssDecur"
        NomiCampiColonne(14) = "NDecur"
        NomiCampiColonne(15) = "ConsDoc"
        NomiCampiColonne(16) = "MesiServizio"
        NomiCampiColonne(17) = "SenzaSanzioni"
        NomiCampiColonne(18) = "Incluso"
        'carico i nomi delle colonne che andrò a stampare nella datagrid
        For x = 0 To 18
            dt.Columns.Add(New DataColumn(NomeColonne(x), GetType(String)))
        Next

        'carico il datatable con il risultato della query della ricerca, in qusto caso delle risorse
        If DataSetDaScorrere.Tables(0).Rows.Count > 0 Then
            For i = 1 To DataSetDaScorrere.Tables(0).Rows.Count
                dr = dt.NewRow()
                For x = 0 To 18
                    If x <> 18 Then
                        dr(x) = DataSetDaScorrere.Tables(0).Rows.Item(i - 1).Item(NomiCampiColonne(x))
                    Else
                        If DataSetDaScorrere.Tables(0).Rows.Item(i - 1).Item(NomiCampiColonne(x)) = 0 Then
                            dr(x) = "NO"
                        Else
                            dr(x) = "SI"
                        End If
                    End If

                Next
                dt.Rows.Add(dr)
            Next
        End If

        'passo alla sessione la datatable che ho appena creato e che userò per il databinding della datagrid della stampa
        Session("DtbRicerca") = dt

    End Sub
    Private Sub StampaCSV(ByVal dtbRicerca As DataTable)
        Dim path As String
        Dim xPrefissoNome As String
        Dim url As String
        Dim utility As ClsUtility = New ClsUtility()

        If dtbRicerca.Rows.Count = 0 Then
            ApriCSV1.Visible = False
            CmdEsporta.Visible = False
        Else
            xPrefissoNome = Session("Utente")
            path = Server.MapPath("download")
            url = CreaFileCSV(dtbRicerca, xPrefissoNome, path)
            ApriCSV1.Visible = True
            ApriCSV1.NavigateUrl = url
        End If
    End Sub
    Function CreaFileCSV(ByVal DTBRicerca As DataTable, ByVal xPrefissoNome As String, ByVal mapPath As String) As String

        Dim dtrChecklist As Data.SqlClient.SqlDataReader
        Dim Writer As StreamWriter
        Dim xLinea As String
        Dim i As Int64
        Dim j As Int64
        Dim NomeUnivoco As String
        Dim Reader As StreamReader
        Dim url As String
        NomeUnivoco = xPrefissoNome & "CheckListPresenze" & Year(Now) & Month(Now) & Day(Now) & Hour(Now) & Minute(Now) & Second(Now)
        Writer = New StreamWriter(mapPath & "\" & NomeUnivoco & ".CSV")
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
        url = "download\" & NomeUnivoco & ".CSV"

        Writer.Close()
        Writer = Nothing
        Return url
    End Function
    
    Protected Sub cmdInizializza_Click(sender As Object, e As EventArgs) Handles cmdInizializza.Click
        Dim sqlCMD As New SqlClient.SqlCommand
        Dim strNomeStore As String = "[SP_CHECKLIST_PRESENZE_INIZIALIZZA]"

        Try
            Dim x As String
            Dim y As String

            sqlCMD = New SqlClient.SqlCommand(strNomeStore, CType(Session("conn"), SqlClient.SqlConnection))
            sqlCMD.CommandType = CommandType.StoredProcedure

            sqlCMD.Parameters.Add("@IdCheckList", SqlDbType.Int).Value = Request.QueryString("idLista")
            sqlCMD.Parameters.Add("@Username", SqlDbType.VarChar).Value = Session("Utente")


            Dim sparam1 As SqlClient.SqlParameter
            sparam1 = New SqlClient.SqlParameter
            sparam1.ParameterName = "@Esito"
            sparam1.Size = 100
            sparam1.SqlDbType = SqlDbType.NVarChar
            sparam1.Direction = ParameterDirection.Output
            sqlCMD.Parameters.Add(sparam1)

            Dim sparam2 As SqlClient.SqlParameter
            sparam2 = New SqlClient.SqlParameter
            sparam2.ParameterName = "@Messaggio"
            sparam2.Size = 1000
            sparam2.SqlDbType = SqlDbType.NVarChar
            sparam2.Direction = ParameterDirection.Output
            sqlCMD.Parameters.Add(sparam2)


            sqlCMD.ExecuteScalar()
            'Dim str As String
            'str = sqlCMD.Parameters("@Messaggio").Value

            x = CStr(sqlCMD.Parameters("@Esito").Value)
            y = sqlCMD.Parameters("@Messaggio").Value


            If x = "POSITIVO" Then

                RicercaVolontariLista()
                CaricaDati()
                cmdInizializza.Visible = False
                cmdRicerca.Visible = True
                CmdConferma.Visible = True
                CmdEsporta.Visible = True
                CmdStampa.Visible = False '(stampo solo dopo conferma)

            End If
            
            lblmessaggio.Text = y


        Catch ex As Exception


            Response.Write(ex.Message.ToString())
            Exit Sub
        End Try
    End Sub

    Public Function CaricaDati()




        Dim lista As DataSet
        lista = CaricoLista(Request.QueryString("idLista"))

        If lista.Tables(0).Rows.Count <> 0 Then
            lblCodCheckList.Text = lista.Tables(0).Rows(0).Item("codicechecklist")
            idlista = lista.Tables(0).Rows(0).Item("IdCheckList")
            anno = lista.Tables(0).Rows(0).Item("AnnoCompetenza")
            mese = lista.Tables(0).Rows(0).Item("MeseCompetenza")
            Regione = lista.Tables(0).Rows(0).Item("CodiceProgettoRendicontazione")
            idStatolista = lista.Tables(0).Rows(0).Item("IdStatoCheckList")
            statoCheckList = lista.Tables(0).Rows(0).Item("StatoCheckList")
            LblEnteDato.Text = lista.Tables(0).Rows(0).Item("Ente")
            LblTitoloDato.Text = lista.Tables(0).Rows(0).Item("Titolo")

        End If

        Dim MeseScritto As String
        Select Case mese
            Case "1"
                MeseScritto = "Gennaio"
            Case "2"
                MeseScritto = "Febbraio"
            Case "3"
                MeseScritto = "Marzo"
            Case "4"
                MeseScritto = "Aprile"
            Case "5"
                MeseScritto = "Maggio"
            Case "6"
                MeseScritto = "Giugno"
            Case "7"
                MeseScritto = "Luglio"
            Case "8"
                MeseScritto = "Agosto"
            Case "9"
                MeseScritto = "Settembre"
            Case "10"
                MeseScritto = "Ottobre"
            Case "11"
                MeseScritto = "Novembre"
            Case "12"
                MeseScritto = "Dicembre"

        End Select

        LblLista.Text = Regione & " " & MeseScritto & " " & anno & " " & statoCheckList
        Return idStatolista
    End Function

    Sub RicordaParametri()
        'creata il 03/20/2016
        'ricorda la pagina della griglia
        Response.Cookies("SUSCNRicChkPagDett")("Page") = dgRisultatoRicerca.CurrentPageIndex
        Response.Cookies("SUSCNRicChkPagDett").Expires = DateTime.Now.AddDays(1)
    End Sub
    Sub RitornaParametri()
        dgRisultatoRicerca.CurrentPageIndex = Server.HtmlEncode(Request.Cookies("SUSCNRicChkPagDett")("Page"))
        dgRisultatoRicerca.DataSource = Session("appDtsRisRicerca")
        dgRisultatoRicerca.DataBind()
        dgRisultatoRicerca.SelectedIndex = -1
    End Sub

    Protected Sub dgRisultatoRicerca_SelectedIndexChanged(sender As Object, e As EventArgs) Handles dgRisultatoRicerca.SelectedIndexChanged

    End Sub

    Private Sub imgStoricoNotifiche_Click(sender As Object, e As System.EventArgs) Handles imgStoricoNotifiche.Click
        Dim JScript As String

        JScript = "<script>" & vbCrLf
        JScript &= "window.open(""WfrmVisualizzaStoricoNotifiche.aspx?IdTipoNotifica=3&IdLista=" & Request.QueryString("idLista") & """, """", ""height=768,width=1024, ,dependent=no,scrollbars=yes,status=no,resizable=yes"")" & vbCrLf
        JScript &= ("</script>")
        Response.Write(JScript)
    End Sub
End Class