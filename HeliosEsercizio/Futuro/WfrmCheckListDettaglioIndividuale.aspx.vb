Imports System.IO
Public Class WfrmCheckListDettaglioIndividuale
    Inherits System.Web.UI.Page
    Dim idlista As Integer
    Dim Regione As String
    Dim anno As String
    Dim mese As String
    Dim statoCheckList As String
    Dim idStatolista As Integer
    Dim identita As Integer
    Dim Nome As String
    Dim Cognome As String
    Dim CodiceVolontario As String

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
                    'RicercaVolontariLista()
                    cmdInizializza.Visible = True
                    'cmdRicerca.Visible = False
                    CmdConferma.Visible = False
                    CmdEsporta.Visible = False '''''''''''''''''''''''''FALSE
                    CmdStampa.Visible = False

                Case 2  'in lavorazione

                    RicercaVolontariLista()
                    cmdInizializza.Visible = False
                    'cmdRicerca.Visible = True
                    CmdConferma.Visible = True
                    CmdEsporta.Visible = True
                    CmdStampa.Visible = False '(stampo solo dopo conferma)
                Case 3 'Confermata
                    RicercaVolontariLista()
                    cmdInizializza.Visible = False
                    'cmdRicerca.Visible = True
                    CmdConferma.Visible = False
                    CmdEsporta.Visible = True
                    CmdStampa.Visible = True '(stampo solo dopo conferma)
            End Select


       
        End If
        
       
    End Sub
    Private Function CaricoLista(idlista)
        Dim strsql As String
        Dim MyDataset As DataSet
        RicercaVolontariLista()

        strsql = "SELECT IdCheckList,IdEntità,CodiceProgettoRendicontazione,attività.titolo, enti.denominazione + ' (' + enti.codiceregione + ')' as Ente , AnnoCompetenza,MeseCompetenza,StatoCheckList,CheckListPagheIndividuale.idStatoCheckList,'I' + CONVERT(VARCHAR,isnull(CheckListPagheIndividuale.idCheckList,0)) AS CodiceCheckList FROM  CheckListPagheIndividuale INNER JOIN StatiCheckList on CheckListPagheIndividuale.idstatochecklist = StatiCheckList.idstatochecklist inner join attività on CheckListPagheIndividuale.codiceprogettorendicontazione = attività.codiceente inner join enti on attività.identepresentante = enti.idente  where IdCheckList=" & idlista
        MyDataset = ClsServer.DataSetGenerico(strsql, Session("conn"))

        If MyDataset.Tables(0).Rows.Count <> 0 Then
            Return MyDataset
        End If
    End Function
    Public Sub TuttaPaginaSess()
        Session("TP") = True
    End Sub
    Private Sub RicercaVolontariLista()

        Dim sqlDAP As New SqlClient.SqlDataAdapter
        Dim dataSet As New DataSet
        Dim strNomeStore As String = "[SP_CHECKLIST_PRESENZE_DETTAGLIO_INDIVIDUALE]"
        dgRisultatoRicerca.CurrentPageIndex = 0
        Try
            sqlDAP = New SqlClient.SqlDataAdapter(strNomeStore, CType(Session("conn"), SqlClient.SqlConnection))
            sqlDAP.SelectCommand.CommandType = CommandType.StoredProcedure
            sqlDAP.SelectCommand.Parameters.Add("@IdCheckList", SqlDbType.Int).Value = (Request.QueryString("IdLista"))
           

            sqlDAP.Fill(dataSet)


            CaricaDataTablePerStampa(dataSet)
            'Session("appDtsRisRicerca") = dataSet
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
    Private Function ChiSono(idEntita)
        Dim strsql As String
        Dim MyDataset As DataSet
        strsql = "SELECT Nome,Cognome,CodiceVolontario FROM  Entità  where IdEntità=" & idEntita
        MyDataset = ClsServer.DataSetGenerico(strsql, Session("conn"))

        If MyDataset.Tables(0).Rows.Count <> 0 Then
            Return MyDataset
        End If
    End Function

    Private Sub dgRisultatoRicerca_ItemCommand(source As Object, e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgRisultatoRicerca.ItemCommand
        If e.CommandName = "Notifica" Then
            If (Session("TipoUtente") = "U") Then
                Session("IdEnte") = e.Item.Cells(22).Text
                Session("Denominazione") = e.Item.Cells(23).Text
                OpenWindow(e.Item.Cells(0).Text, idlista)

                'Response.Redirect("~/WfrmCAPUtility.aspx?IdEntita=" & e.Item.Cells(0).Text & "&VengoDa=" & 1)
            Else
                Response.Redirect("page_error.aspx")
            End If
        End If
        If e.CommandName = "Documenti" Then
            If (Session("TipoUtente") = "U") Then
                Session("IdEnte") = e.Item.Cells(22).Text
                Session("Denominazione") = e.Item.Cells(23).Text
                Response.Redirect("~/WfrmVisualizzaElencoDocumentiVolontario.aspx?IdVol=" & e.Item.Cells(0).Text & "&ProVengoDa=" & 2 & "&idLista=" & idlista)
            Else
                Response.Redirect("page_error.aspx")
            End If
        End If
        If e.CommandName = "Presenze" Then
            Session("IdEnte") = e.Item.Cells(22).Text
            Session("Denominazione") = e.Item.Cells(23).Text
            If (Session("TipoUtente") = "U") Then
                Response.Redirect("~/Presenze.aspx?IdEntita=" & e.Item.Cells(0).Text & "&VengoDa=" & 2 & "&anno=" & anno & "&mese=" & mese & "&idLista=" & idlista)
            Else
                Response.Redirect("page_error.aspx")
            End If
        End If
    End Sub
    Protected Sub OpenWindow(ByVal identita As Integer, ByVal idlista As Integer)

        Dim url As String = "WfrmCheckListNotificaMailPresenze.aspx?IdEntita=" & identita & "&idLista=" & idlista & "&VengoDa=" & 2

        Dim s As String = "window.open('" & url + "', 'popup_window', 'width=800,height=600,left=100,top=100,resizable=yes');"

        ClientScript.RegisterStartupScript(Me.GetType(), "script", s, True)

    End Sub
    Sub CaricaDataTablePerStampa(ByVal DataSetDaScorrere As DataSet)
        Dim dt As New DataTable
        Dim dr As DataRow
        Dim i As Integer
        Dim x As Integer

        Dim NomeColonne(16) As String
        Dim NomiCampiColonne(16) As String

        'nome della colonna 
        'e posizione nella griglia di lettura

        NomeColonne(0) = "Nominativo"
        NomeColonne(1) = "Codice Fiscale"
        NomeColonne(2) = "Inizio Servizio"
        NomeColonne(3) = "Contratto"
        NomeColonne(4) = "IBAN"
        NomeColonne(5) = "Foglio Presenze"
        NomeColonne(6) = "N P"
        NomeColonne(7) = "N MAL"
        NomeColonne(8) = "N PR"
        NomeColonne(9) = "Ass Cons"
        NomeColonne(10) = "N MAL Tot"
        NomeColonne(11) = "N PR Tot"
        NomeColonne(12) = "Ass Decur"
        NomeColonne(13) = "N Decur"
        NomeColonne(14) = "Cons Doc"
        NomeColonne(15) = "Mesi Servizio"
        NomeColonne(16) = "Senza Sanzioni"
        ' NomeColonne(15) = "Incl"


        NomiCampiColonne(0) = "Nominativo"
        NomiCampiColonne(1) = "CodiceFiscale"
        NomiCampiColonne(2) = "InizioServizio"
        NomiCampiColonne(3) = "Contratto"
        NomiCampiColonne(4) = "IBAN"
        NomiCampiColonne(5) = "FoglioPresenze"
        NomiCampiColonne(6) = "NP"
        NomiCampiColonne(7) = "NMAL"
        NomiCampiColonne(8) = "NPR"
        NomiCampiColonne(9) = "AssCons"
        NomiCampiColonne(10) = "NMALTot"
        NomiCampiColonne(11) = "NPRTot"
        NomiCampiColonne(12) = "AssDecur"
        NomiCampiColonne(13) = "NDecur"
        NomiCampiColonne(14) = "ConsDoc"
        NomiCampiColonne(15) = "MesiServizio"
        NomiCampiColonne(16) = "SenzaSanzioni"
        ' NomiCampiColonne(15) = "Incluso"
        'carico i nomi delle colonne che andrò a stampare nella datagrid
        For x = 0 To 16
            dt.Columns.Add(New DataColumn(NomeColonne(x), GetType(String)))
        Next

        'carico il datatable con il risultato della query della ricerca, in qusto caso delle risorse
        If DataSetDaScorrere.Tables(0).Rows.Count > 0 Then
            For i = 1 To DataSetDaScorrere.Tables(0).Rows.Count
                dr = dt.NewRow()
                For x = 0 To 16
                    'If x <> 15 Then
                    '    dr(x) = DataSetDaScorrere.Tables(0).Rows.Item(i - 1).Item(NomiCampiColonne(x))
                    'Else
                    '    If DataSetDaScorrere.Tables(0).Rows.Item(i - 1).Item(NomiCampiColonne(x)) = 0 Then
                    '        dr(x) = "NO"
                    '    Else
                    '        dr(x) = "SI"
                    '    End If
                    'End If
                    dr(x) = DataSetDaScorrere.Tables(0).Rows.Item(i - 1).Item(NomiCampiColonne(x))
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
        NomeUnivoco = xPrefissoNome & "CheckListPresenzeIndividuale" & Year(Now) & Month(Now) & Day(Now) & Hour(Now) & Minute(Now) & Second(Now)
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
    Protected Sub CmdEsporta_Click(sender As Object, e As EventArgs) Handles CmdEsporta.Click
        CmdEsporta.Visible = False
        Dim dtbRicerca As DataTable = Session("DtbRicerca")
        StampaCSV(dtbRicerca)
    End Sub
    Protected Sub cmdChiudi_Click(sender As Object, e As EventArgs) Handles cmdChiudi.Click
        Response.Redirect("WfrmCheckListRicerca.aspx")
    End Sub
    Protected Sub cmdInizializza_Click(sender As Object, e As EventArgs) Handles cmdInizializza.Click
        Dim sqlCMD As New SqlClient.SqlCommand
        Dim strNomeStore As String = "[SP_CHECKLIST_PRESENZE_INIZIALIZZA_INDIVIDUALE]"

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
                'cmdRicerca.Visible = True
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
    Protected Sub CmdConferma_Click(sender As Object, e As EventArgs) Handles CmdConferma.Click
        Dim sqlCMD As New SqlClient.SqlCommand
        Dim strNomeStore As String = "[SP_CHECKLIST_PRESENZE_CONFERMA_INDIVIDUALE]"

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
        Response.Write("window.open(""WfrmReportistica.aspx?sTipoStampa=42&IdCheckList=" & Request.QueryString("idLista") & """, ""Report"", ""height=800,width=800, ,dependent=no,scrollbars=no,status=no,resizable=yes"")")
        Response.Write("</script>")

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
            identita = lista.Tables(0).Rows(0).Item("IdEntità")
            LblEnteDato.Text = lista.Tables(0).Rows(0).Item("Ente")
            LblTitoloDato.Text = lista.Tables(0).Rows(0).Item("Titolo")
        End If

        lista = ChiSono(identita)
        If lista.Tables(0).Rows.Count <> 0 Then
            Nome = lista.Tables(0).Rows(0).Item("Nome")
            Cognome = lista.Tables(0).Rows(0).Item("Cognome")
            CodiceVolontario = lista.Tables(0).Rows(0).Item("CodiceVolontario")
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
        LblLista.Text = Nome & " " & Cognome & " " & MeseScritto & " " & anno & " " & statoCheckList
        Return idStatolista
    End Function

    Private Sub dgRisultatoRicerca_PageIndexChanged(source As Object, e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgRisultatoRicerca.PageIndexChanged

    End Sub

    Private Sub imgStoricoNotifiche_Click(sender As Object, e As System.EventArgs) Handles imgStoricoNotifiche.Click
        Dim JScript As String

        JScript = "<script>" & vbCrLf
        JScript &= "window.open(""WfrmVisualizzaStoricoNotifiche.aspx?IdTipoNotifica=4&IdLista=" & Request.QueryString("idLista") & """, """", ""height=768,width=1024, ,dependent=no,scrollbars=yes,status=no,resizable=yes"")" & vbCrLf
        JScript &= ("</script>")
        Response.Write(JScript)
    End Sub
End Class