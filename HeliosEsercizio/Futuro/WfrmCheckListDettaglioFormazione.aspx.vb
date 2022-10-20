Imports System.IO
Public Class WfrmCheckListDettaglioFormazione
    Inherits System.Web.UI.Page



    Dim statoCheckList As String
    Dim identita As Integer
   
    Dim IdLista As Integer
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Call TuttaPaginaSess()
        If Not Session("LogIn") Is Nothing Then
            If Session("LogIn") = False Then
                Response.Redirect("LogOn.aspx")
            End If
        Else
            Response.Redirect("LogOn.aspx")
        End If

        statoCheckList = CaricaDati()

        If Page.IsPostBack = False Then



            Select Case statoCheckList

                Case 1 'da lavorare (Inizializzazione)
                    'RicercaVolontariLista()
                    cmdInizializza.Visible = True
                    'cmdRicerca.Visible = False
                    CmdConferma.Visible = False
                    CmdEsporta.Visible = False '''''''''''''''''''''''''FALSE
                    CmdStampa.Visible = False
                    dgRisultatoRicerca.Visible = False


                    divArchiviazione.Visible = False
                    cmdArchiviata.Visible = False
                    cmdRipristinata.Visible = False
                Case 2  'in lavorazione

                    RicercaVolontariLista()
                    cmdInizializza.Visible = False
                    'cmdRicerca.Visible = True
                    CmdConferma.Visible = True
                    CmdEsporta.Visible = True
                    CmdStampa.Visible = False '(stampo solo dopo conferma)
                    CercaIdLista()
                    cmdArchiviata.Visible = True
                    cmdRipristinata.Visible = False
                    CaricaComboCausaleArchiviazione()
                Case 3 'Confermata
                    RicercaVolontariLista()
                    cmdInizializza.Visible = False
                    'cmdRicerca.Visible = True
                    CmdConferma.Visible = False
                    CmdEsporta.Visible = True
                    CmdStampa.Visible = True '(stampo solo dopo conferma)
                    CercaIdLista()

                    divArchiviazione.Visible = False
                    cmdArchiviata.Visible = False
                    cmdRipristinata.Visible = False
                Case 4 'Archiviata
                    RicercaVolontariLista()

                    cmdInizializza.Visible = False
                    'cmdRicerca.Visible = True
                    CmdConferma.Visible = False
                    CmdEsporta.Visible = False
                    CmdStampa.Visible = False '(stampo solo dopo conferma)

                    divArchiviazione.Visible = True
                    cmdArchiviata.Visible = False
                    cmdRipristinata.Visible = True
                    CaricaComboCausaleArchiviazione()
                    CercaIdLista()
                    CaricaCausaleArchiviazione()

            End Select
            If Request.QueryString("menu") Is Nothing Then
                If Not Request.Cookies("SUSCNRicChkFormDett") Is Nothing Then
                    RitornaParametri()
                End If
            Else
                Response.Cookies("SUSCNRicChkFormDett")("Page") = 0
                Response.Cookies("SUSCNRicChkFormDett").Expires = DateTime.Now.AddDays(1)
            End If
        End If


    End Sub
  
    Private Function CaricoTitoli(idAttivita)
        Dim strsql As String
        Dim MyDataset As DataSet
        ' RicercaVolontariLista()

        strsql = "SELECT CodiceEnte , Titolo From Attività where idAttività=" & Request.QueryString("IdAttivita")
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
        Dim strNomeStore As String = "[SP_CHECKLIST_FORMAZIONE_DETTAGLIO]"
        'dgRisultatoRicerca.CurrentPageIndex = 0
        Try
            sqlDAP = New SqlClient.SqlDataAdapter(strNomeStore, CType(Session("conn"), SqlClient.SqlConnection))
            sqlDAP.SelectCommand.CommandType = CommandType.StoredProcedure
            sqlDAP.SelectCommand.Parameters.Add("@IdAttivita", SqlDbType.Int).Value = (Request.QueryString("idAttivita"))


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
        Dim Ritorno As String()

        If e.CommandName = "Notifica" Then
            RicordaParametri()
            If (Session("TipoUtente") = "U") Then
                Session("IdEnte") = e.Item.Cells(15).Text
                Session("Denominazione") = e.Item.Cells(16).Text
                CercaIdLista()
                OpenWindow(e.Item.Cells(0).Text, IdLista)

                'Response.Redirect("~/WfrmCAPUtility.aspx?IdEntita=" & e.Item.Cells(0).Text & "&VengoDa=" & 1)
            Else
                Response.Redirect("page_error.aspx")
            End If
        End If
        If e.CommandName = "Documenti" Then
            RicordaParametri()
            If (Session("TipoUtente") = "U") Then
                Session("IdEnte") = e.Item.Cells(15).Text
                Session("Denominazione") = e.Item.Cells(16).Text

                Response.Redirect("~/WfrmVisualizzaElencoDocumentiVolontario.aspx?IdVol=" & e.Item.Cells(0).Text & "&ProVengoDa=" & 6 & "&idLista=" & IdLista & "&idAttivita=" & Request.QueryString("idAttivita"))
            Else
                Response.Redirect("page_error.aspx")
            End If
        End If
        If e.CommandName = "DocumentiDocent" Then
            RicordaParametri()
            If (Session("TipoUtente") = "U") Then
                Session("IdEnte") = e.Item.Cells(15).Text
                Session("Denominazione") = e.Item.Cells(16).Text
                Response.Redirect("~/WfrmVisualizzaElencoDocumentiFormatori.aspx?IdVol=" & e.Item.Cells(0).Text & "&ProVengoDa=" & 6 & "&idLista=" & IdLista & "&IdProg=" & Request.QueryString("idAttivita"))
            Else
                Response.Redirect("page_error.aspx")
            End If
        End If
        If e.CommandName = "Includi" Then
            RicordaParametri()
            Ritorno = UpdateChecListFormazioneIncludi(e.Item.Cells(0).Text, IdLista)
            If Ritorno(0) = "POSITIVO" Then
                RicercaVolontariLista()
                lblmessaggio.Text = Ritorno(1)
                RitornaParametri()
            Else 'negativo
                lblmessaggio.Text = Ritorno(1)
            End If
        End If
        If e.CommandName = "Escludi" Then
            RicordaParametri()
            Ritorno = UpdateChecListFormazioneEscludi(e.Item.Cells(0).Text, IdLista)
            If Ritorno(0) = "POSITIVO" Then
                RicercaVolontariLista()
                lblmessaggio.Text = Ritorno(1)
                RitornaParametri()
            Else 'negativo
                lblmessaggio.Text = Ritorno(1)
            End If
        End If
    End Sub

    Private Function UpdateChecListFormazioneIncludi(entita As Integer, IdLista As Integer) As String()

        Dim sqlCMD As New SqlClient.SqlCommand
        Dim strNomeStore As String = "[SP_CHECKLIST_FORMAZIONE_INCLUDI]"

        Try
            Dim x As String
            Dim y As String
            Dim ArreyOutPut(1) As String
            sqlCMD = New SqlClient.SqlCommand(strNomeStore, CType(Session("conn"), SqlClient.SqlConnection))
            sqlCMD.CommandType = CommandType.StoredProcedure

            sqlCMD.Parameters.Add("@IdCheckList", SqlDbType.Int).Value = IdLista
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
    Private Function UpdateChecListFormazioneEscludi(entita As Integer, IdLista As Integer) As String()

        Dim sqlCMD As New SqlClient.SqlCommand
        Dim strNomeStore As String = "[SP_CHECKLIST_FORMAZIONE_ESCLUDI]"

        Try
            Dim x As String
            Dim y As String
            Dim ArreyOutPut(1) As String
            sqlCMD = New SqlClient.SqlCommand(strNomeStore, CType(Session("conn"), SqlClient.SqlConnection))
            sqlCMD.CommandType = CommandType.StoredProcedure

            sqlCMD.Parameters.Add("@IdCheckList", SqlDbType.Int).Value = IdLista
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



    Protected Sub OpenWindow(ByVal identita As Integer, ByVal idlista As Integer)

        Dim url As String = "WfrmCheckListNotificaMailPresenze.aspx?IdEntita=" & identita & "&idLista=" & idlista & "&VengoDa=" & 6

        Dim s As String = "window.open('" & url + "', 'popup_window', 'width=800,height=600,left=100,top=100,resizable=yes');"

        ClientScript.RegisterStartupScript(Me.GetType(), "script", s, True)

    End Sub
    Sub CaricaDataTablePerStampa(ByVal DataSetDaScorrere As DataSet)
        Dim dt As New DataTable
        Dim dr As DataRow
        Dim i As Integer
        Dim x As Integer

        Dim NomeColonne(10) As String
        Dim NomiCampiColonne(10) As String

        'nome della colonna 
        'e posizione nella griglia di lettura
        NomeColonne(0) = "Cod Vol"
        NomeColonne(1) = "Nominativo"
        NomeColonne(2) = "C.F."
        NomeColonne(3) = "Prog Fin"
        NomeColonne(4) = "Contr"
        NomeColonne(5) = "Mod F"
        NomeColonne(6) = "Period"
        NomeColonne(7) = "Firma Vol"
        NomeColonne(8) = "Docenti"
        NomeColonne(9) = "Firma Doce"
        NomeColonne(10) = "Esito"
        ' NomeColonne(15) = "Incl"

        NomiCampiColonne(0) = "CodiceVolontario"
        NomiCampiColonne(1) = "Nominativo"
        NomiCampiColonne(2) = "CodiceFiscale"
        NomiCampiColonne(3) = "ProgettoFinanziato"
        NomiCampiColonne(4) = "Contratto"
        NomiCampiColonne(5) = "ModuloF"
        NomiCampiColonne(6) = "CongruenzaPeriodo"
        NomiCampiColonne(7) = "FirmaVolontari"
        NomiCampiColonne(8) = "Docenti"
        NomiCampiColonne(9) = "FirmaDocenti"
        NomiCampiColonne(10) = "Esito"

        ' NomiCampiColonne(15) = "Incluso"
        'carico i nomi delle colonne che andrò a stampare nella datagrid
        For x = 0 To 10
            dt.Columns.Add(New DataColumn(NomeColonne(x), GetType(String)))
        Next

        'carico il datatable con il risultato della query della ricerca, in qusto caso delle risorse
        If DataSetDaScorrere.Tables(0).Rows.Count > 0 Then
            For i = 1 To DataSetDaScorrere.Tables(0).Rows.Count
                dr = dt.NewRow()
                For x = 0 To 10
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
        NomeUnivoco = xPrefissoNome & "CheckListFormazione" & Year(Now) & Month(Now) & Day(Now) & Hour(Now) & Minute(Now) & Second(Now)
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
        Response.Redirect("WfrmCheckListElencoFormazione.aspx")
    End Sub
    Protected Sub cmdInizializza_Click(sender As Object, e As EventArgs) Handles cmdInizializza.Click
        Dim sqlCMD As New SqlClient.SqlCommand
        Dim strNomeStore As String = "[SP_CHECKLIST_FORMAZIONE_INIZIALIZZA]"

        Try
            Dim x As String
            Dim y As String

            sqlCMD = New SqlClient.SqlCommand(strNomeStore, CType(Session("conn"), SqlClient.SqlConnection))
            sqlCMD.CommandType = CommandType.StoredProcedure

            sqlCMD.Parameters.Add("@IDAttivita", SqlDbType.Int).Value = Request.QueryString("idAttivita")
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
                cmdArchiviata.Visible = True
                CmdStampa.Visible = False '(stampo solo dopo conferma)
                dgRisultatoRicerca.Visible = True
            End If

            lblmessaggio.Text = y

            CercaIdLista()


        Catch ex As Exception


            Response.Write(ex.Message.ToString())
            Exit Sub
        End Try
    End Sub
    Protected Sub CmdConferma_Click(sender As Object, e As EventArgs) Handles CmdConferma.Click
        Dim sqlCMD As New SqlClient.SqlCommand
        Dim strNomeStore As String = "[SP_CHECKLIST_FORMAZIONE_CONFERMA]"

        Try
            Dim x As String
            Dim y As String

            sqlCMD = New SqlClient.SqlCommand(strNomeStore, CType(Session("conn"), SqlClient.SqlConnection))
            sqlCMD.CommandType = CommandType.StoredProcedure
            CercaIdLista()
            sqlCMD.Parameters.Add("@IdAttivita", SqlDbType.Int).Value = Request.QueryString("IdAttivita")
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
                cmdArchiviata.Visible = False
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
        Response.Write("window.open(""WfrmReportistica.aspx?sTipoStampa=46&IdCheckList=" & Request.QueryString("idLista") & """, ""Report"", ""height=800,width=800, ,dependent=no,scrollbars=no,status=no,resizable=yes"")")
        Response.Write("</script>")
    End Sub
    Public Function CaricaDati()
        Dim Titolo As DataSet
        Titolo = CaricoTitoli(Request.QueryString("IdAttivita"))
        'Dim Formazione As DataSet
        'Formazione = CaricoLista(Request.QueryString("IdAttività"))
        If Titolo.Tables(0).Rows.Count <> 0 Then

            lblTitoloDato.Text = Titolo.Tables(0).Rows(0).Item("Titolo")
            LblLista.Text = Titolo.Tables(0).Rows(0).Item("CodiceEnte")
        End If

        Dim StatoDesc As String

        Dim strsql As String
        Dim MyDataset As DataSet
        strsql = "SELECT Nome,Cognome,CodiceVolontario FROM  Entità  where IdEntità=" & identita
        MyDataset = ClsServer.DataSetGenerico(strsql, Session("conn"))

        If MyDataset.Tables(0).Rows.Count <> 0 Then
            Return MyDataset
        End If


        CercaIdLista()

        Select Case statoCheckList
            Case Nothing
                StatoDesc = "Da Lavorare"
                statoCheckList = "1"
            Case "1"
                StatoDesc = "Da Lavorare"
            Case "2"
                StatoDesc = "In Lavorazione"
            Case "3"
                StatoDesc = "Da Confermare"
            Case "4"
                StatoDesc = "Archiviata"
        End Select

        LblLista.Text = LblLista.Text & " " & StatoDesc


        Return statoCheckList
    End Function
    Public Sub CercaIdLista()
        Dim strsql As String
        Dim MyDataset As DataSet
        strsql = "SELECT IdCheckList , IdStatoCheckList,'F' + CONVERT(VARCHAR,idCheckList) AS CodiceCheckList FROM  CheckListFormazione  where idAttività=" & Request.QueryString("idAttivita")
        MyDataset = ClsServer.DataSetGenerico(strsql, Session("conn"))

        If MyDataset.Tables(0).Rows.Count <> 0 Then
            lblCodCheckList.Text = MyDataset.Tables(0).Rows(0).Item("codicechecklist")
            IdLista = MyDataset.Tables(0).Rows(0).Item("IdCheckList")
            statoCheckList = MyDataset.Tables(0).Rows(0).Item("IdStatoCheckList")
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
    Sub RicordaParametri()
        'creata il 03/20/2016
        'ricorda la pagina della griglia
        Response.Cookies("SUSCNRicChkFormDett")("Page") = dgRisultatoRicerca.CurrentPageIndex
        Response.Cookies("SUSCNRicChkFormDett").Expires = DateTime.Now.AddDays(1)
    End Sub
    Sub RitornaParametri()
        dgRisultatoRicerca.CurrentPageIndex = Server.HtmlEncode(Request.Cookies("SUSCNRicChkFormDett")("Page"))
        dgRisultatoRicerca.DataSource = Session("appDtsRisRicerca")
        dgRisultatoRicerca.DataBind()
        dgRisultatoRicerca.SelectedIndex = -1
    End Sub

    Sub CaricaComboCausaleArchiviazione()
        Dim strSql As String
        Try

            Dim MyDataset As DataSet

            CboCausaleArchiviazione.Items.Clear()
            strSql = "SELECT '' as CAUSALE ,'0' as IdCausaleArchiviazione  UNION  Select Causale as CAUSALE,IdCausaleArchiviazione from CheckListCausaliArchiviazione WHERE ABILITATO=1 AND Tipochecklist='F' order by IdCausaleArchiviazione"
            MyDataset = ClsServer.DataSetGenerico(strSql, Session("conn"))
            CboCausaleArchiviazione.DataSource = MyDataset
            CboCausaleArchiviazione.DataTextField = "CAUSALE"
            CboCausaleArchiviazione.DataValueField = "IdCausaleArchiviazione"
            CboCausaleArchiviazione.DataBind()
        Catch ex As Exception

        End Try
    End Sub
    Protected Sub cmdArchiviata_Click(sender As Object, e As EventArgs) Handles cmdArchiviata.Click

        CmdConfermaArch.Visible = True
        divArchiviazione.Visible = True

        cmdInizializza.Visible = False
        CmdConferma.Visible = False
        CmdEsporta.Visible = False
        CmdStampa.Visible = False '(stampo solo dopo conferma)
        cmdArchiviata.Visible = False
        cmdRipristinata.Visible = False
    End Sub
    Private Sub CaricaCausaleArchiviazione()
        Dim strsql As String
        Dim MyDataset As DataSet
        strsql = "SELECT IdCausaleArchiviazione,notearchiviazione FROM  CheckListFormazione  where IdCheckList=" & Request.QueryString("IdLista")
        MyDataset = ClsServer.DataSetGenerico(strsql, Session("conn"))

        If MyDataset.Tables(0).Rows.Count <> 0 Then
            CboCausaleArchiviazione.SelectedValue = MyDataset.Tables(0).Rows(0).Item("IdCausaleArchiviazione")
            txtNoteArchiviazione.Text = MyDataset.Tables(0).Rows(0).Item("notearchiviazione")
        End If
    End Sub
    Sub Archivia()
        If CboCausaleArchiviazione.SelectedValue = 0 Then
            lblerrore.Visible = True
            lblerrore.Text = "E' necessario indicare una causale di archiviazione."
            Exit Sub
        End If



        Dim sqlCMD As New SqlClient.SqlCommand
        Dim strNomeStore As String = "[SP_CHECKLIST_ARCHIVIAZIONE]"

        Try
            Dim x As String
            Dim y As String

            sqlCMD = New SqlClient.SqlCommand(strNomeStore, CType(Session("conn"), SqlClient.SqlConnection))
            sqlCMD.CommandType = CommandType.StoredProcedure
            CercaIdLista()
            sqlCMD.Parameters.Add("@IdCheckList", SqlDbType.Int).Value = Request.QueryString("IdLista")
            sqlCMD.Parameters.Add("@IdCausaleArchiviazione", SqlDbType.Int).Value = CboCausaleArchiviazione.SelectedValue
            sqlCMD.Parameters.Add("@TipoCheckList", SqlDbType.Char).Value = "F"
            sqlCMD.Parameters.Add("@NoteArchiviazione", SqlDbType.VarChar).Value = txtNoteArchiviazione.Text.Replace("'", "''")
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
                cmdArchiviata.Visible = False
                CmdEsporta.Visible = False
                CmdStampa.Visible = False '(stampo solo dopo conferma)
                cmdRipristinata.Visible = True
                CmdConfermaArch.Visible = False
                lblerrore.Visible = False
            End If

            lblmessaggio.Text = y


        Catch ex As Exception

            Response.Write(ex.Message.ToString())
            Exit Sub
        End Try
    End Sub

    Protected Sub cmdRipristinata_Click(sender As Object, e As EventArgs) Handles cmdRipristinata.Click


        Dim sqlCMD As New SqlClient.SqlCommand
        Dim strNomeStore As String = "[SP_CHECKLIST_RIPRISTINO]"

        Try
            Dim x As String
            Dim y As String

            sqlCMD = New SqlClient.SqlCommand(strNomeStore, CType(Session("conn"), SqlClient.SqlConnection))
            sqlCMD.CommandType = CommandType.StoredProcedure
            CercaIdLista()
            sqlCMD.Parameters.Add("@IdCheckList", SqlDbType.Int).Value = Request.QueryString("IdLista")
            sqlCMD.Parameters.Add("@TipoCheckList", SqlDbType.Char).Value = "F"
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
                CmdConferma.Visible = True
                CmdEsporta.Visible = True
                CmdStampa.Visible = False '(stampo solo dopo conferma)
                cmdArchiviata.Visible = True
                divArchiviazione.Visible = False
                cmdRipristinata.Visible = False
                lblerrore.Visible = False
            End If

            lblmessaggio.Text = y


        Catch ex As Exception

            Response.Write(ex.Message.ToString())
            Exit Sub
        End Try
    End Sub

    Private Sub CmdConfermaArch_Click(sender As Object, e As System.EventArgs) Handles CmdConfermaArch.Click
        Archivia()
    End Sub
    Private Sub imgStoricoNotifiche_Click(sender As Object, e As System.EventArgs) Handles imgStoricoNotifiche.Click
        Dim JScript As String

        JScript = "<script>" & vbCrLf
        JScript &= "window.open(""WfrmVisualizzaStoricoNotifiche.aspx?IdTipoNotifica=5&IdLista=" & Request.QueryString("idLista") & "&idAttività=" & Request.QueryString("idAttivita") & """, """", ""height=768,width=1024, ,dependent=no,scrollbars=yes,status=no,resizable=yes"")" & vbCrLf
        JScript &= ("</script>")
        Response.Write(JScript)
    End Sub
End Class