Imports System.IO
Public Class WfrmCheckListDettaglioRimborsoViaggio
    Inherits System.Web.UI.Page
    Dim idEntitaRimborso As Integer
    Dim Regione As String
    Dim Data As String
    Dim statoCheckList As String
    Dim identita As Integer
    Dim Nome As String
    Dim Cognome As String
    Dim CodiceVolontario As String
    Dim Importo As String
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

            'if stato inizializzato

            '' '' ''Select Case statoCheckList

            '' '' ''    Case 1 'da lavorare (Inizializzazione)
            '' '' ''        'RicercaVolontariLista()
            '' '' ''        cmdInizializza.Visible = True
            '' '' ''        'cmdRicerca.Visible = False
            '' '' ''        CmdConferma.Visible = False
            '' '' ''        CmdEsporta.Visible = False '''''''''''''''''''''''''FALSE
            '' '' ''        CmdStampa.Visible = False

            '' '' ''    Case 2  'in lavorazione

            '' '' ''        RicercaVolontariLista()
            '' '' ''        cmdInizializza.Visible = False
            '' '' ''        'cmdRicerca.Visible = True
            '' '' ''        CmdConferma.Visible = True
            '' '' ''        CmdEsporta.Visible = True
            '' '' ''        CmdStampa.Visible = False '(stampo solo dopo conferma)
            '' '' ''    Case 3 'Confermata
            '' '' ''        RicercaVolontariLista()
            '' '' ''        cmdInizializza.Visible = False
            '' '' ''        'cmdRicerca.Visible = True
            '' '' ''        CmdConferma.Visible = False
            '' '' ''        CmdEsporta.Visible = True
            '' '' ''        CmdStampa.Visible = True '(stampo solo dopo conferma)
            '' '' ''End Select

            Select Case statoCheckList

                Case 1 'da lavorare (Inizializzazione)
                    'RicercaVolontariLista()
                    cmdInizializza.Visible = True
                    'cmdRicerca.Visible = False
                    CmdConferma.Visible = False
                    CmdEsporta.Visible = False '''''''''''''''''''''''''FALSE
                    CmdStampa.Visible = False
                    dgRisultatoRicerca.Visible = False
                Case 2  'in lavorazione

                    RicercaVolontariLista()
                    cmdInizializza.Visible = False
                    'cmdRicerca.Visible = True
                    CmdConferma.Visible = True
                    CmdEsporta.Visible = True
                    CmdStampa.Visible = False '(stampo solo dopo conferma)
                    CercaIdLista()

                Case 3 'Confermata
                    RicercaVolontariLista()
                    cmdInizializza.Visible = False
                    'cmdRicerca.Visible = True
                    CmdConferma.Visible = False
                    CmdEsporta.Visible = True
                    CmdStampa.Visible = True '(stampo solo dopo conferma)
                    CercaIdLista()
            End Select

        End If


    End Sub
    Private Function CaricoLista(idEntitaRimborso)
        Dim strsql As String
        Dim MyDataset As DataSet
        RicercaVolontariLista()

        strsql = "SELECT entitàrimborsi.IdentitàRimborso,entitàrimborsi.IdEntità,DataRiferimento,ImportoConfermato,ISNULL(CheckListRimborsiViaggio.IdStatoCheckList,1) AS Stato,entità.Cognome,entità.Nome,entità.codicefiscale,entità.IBAN,ISNULL( 'V' + CONVERT(VARCHAR,CheckListRimborsiViaggio.idCheckList),'') AS CodiceCheckList FROM  entitàrimborsi INNER JOIN entità on EntitàRimborsi.IDEntità = entità.identità " & _
            "left join CheckListRimborsiViaggio on  entitàrimborsi.identitàrimborso = CheckListRimborsiViaggio.identitàrimborso where entitàrimborsi.IdentitàRimborso=" & idEntitaRimborso
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
        Dim strNomeStore As String = "[SP_CHECKLIST_RIMBORSI_DETTAGLIO]"
        dgRisultatoRicerca.CurrentPageIndex = 0
        Try
            sqlDAP = New SqlClient.SqlDataAdapter(strNomeStore, CType(Session("conn"), SqlClient.SqlConnection))
            sqlDAP.SelectCommand.CommandType = CommandType.StoredProcedure
            sqlDAP.SelectCommand.Parameters.Add("@IdEntitàRimborso", SqlDbType.Int).Value = (Request.QueryString("idEntitaRimborso"))


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
        If e.CommandName = "Notifica" Then
            If (Session("TipoUtente") = "U") Then
                Session("IdEnte") = e.Item.Cells(13).Text
                Session("Denominazione") = e.Item.Cells(14).Text
                CercaIdLista()
                OpenWindow(e.Item.Cells(0).Text, IdLista)

                'Response.Redirect("~/WfrmCAPUtility.aspx?IdEntita=" & e.Item.Cells(0).Text & "&VengoDa=" & 1)
            Else
                Response.Redirect("page_error.aspx")
            End If
        End If
        If e.CommandName = "Documenti" Then
            If (Session("TipoUtente") = "U") Then
                Session("IdEnte") = e.Item.Cells(13).Text
                Session("Denominazione") = e.Item.Cells(14).Text
                Response.Redirect("~/WfrmVisualizzaElencoDocumentiVolontario.aspx?IdVol=" & e.Item.Cells(0).Text & "&ProVengoDa=" & 3 & "&idLista=" & IdLista & "&idEntitaRimborso=" & e.Item.Cells(1).Text)
            Else
                Response.Redirect("page_error.aspx")
            End If
        End If
        If e.CommandName = "DocumentiRimb" Then
            If (Session("TipoUtente") = "U") Then
                Session("IdEnte") = e.Item.Cells(13).Text
                Session("Denominazione") = e.Item.Cells(14).Text
                Response.Redirect("~/WfrmGestioneRimborsoVolontari.aspx?IdEntita=" & e.Item.Cells(0).Text & "&ProVengoDa=" & 7 & "&idLista=" & Request.QueryString("IdLista") & "&idAttivita=" & e.Item.Cells(17).Text & "&idEntitaRimborso=" & e.Item.Cells(1).Text & "&Data=" & Request.QueryString("Data"))
            Else
                Response.Redirect("page_error.aspx")
            End If
        End If
    End Sub
    Protected Sub OpenWindow(ByVal identita As Integer, ByVal idlista As Integer)

        Dim url As String = "WfrmCheckListNotificaMailPresenze.aspx?IdEntita=" & identita & "&idLista=" & idlista & "&VengoDa=" & 3

        Dim s As String = "window.open('" & url + "', 'popup_window', 'width=800,height=600,left=100,top=100,resizable=yes');"

        ClientScript.RegisterStartupScript(Me.GetType(), "script", s, True)

    End Sub
    Sub CaricaDataTablePerStampa(ByVal DataSetDaScorrere As DataSet)
        Dim dt As New DataTable
        Dim dr As DataRow
        Dim i As Integer
        Dim x As Integer

        Dim NomeColonne(8) As String
        Dim NomiCampiColonne(8) As String

        'nome della colonna 
        'e posizione nella griglia di lettura

        NomeColonne(0) = "Nominativo"
        NomeColonne(1) = "Codice Fiscale"
        NomeColonne(2) = "Inizio Servizio"
        NomeColonne(3) = "Contratto"
        NomeColonne(4) = "IBAN"
        NomeColonne(5) = "Res Dom"
        NomeColonne(6) = "Ver Tit"
        NomeColonne(7) = "Ver Corr Tit"
        NomeColonne(8) = "Esito"
        ' NomeColonne(15) = "Incl"


        NomiCampiColonne(0) = "Nominativo"
        NomiCampiColonne(1) = "CodiceFiscale"
        NomiCampiColonne(2) = "InizioServizio"
        NomiCampiColonne(3) = "Contratto"
        NomiCampiColonne(4) = "IBAN"
        NomiCampiColonne(5) = "ResidenzaDomicilio"
        NomiCampiColonne(6) = "VerificaTitolo"
        NomiCampiColonne(7) = "VerificaCorrispondenzaTitolo"
        NomiCampiColonne(8) = "Esito"
       
        ' NomiCampiColonne(15) = "Incluso"
        'carico i nomi delle colonne che andrò a stampare nella datagrid
        For x = 0 To 8
            dt.Columns.Add(New DataColumn(NomeColonne(x), GetType(String)))
        Next

        'carico il datatable con il risultato della query della ricerca, in qusto caso delle risorse
        If DataSetDaScorrere.Tables(0).Rows.Count > 0 Then
            For i = 1 To DataSetDaScorrere.Tables(0).Rows.Count
                dr = dt.NewRow()
                For x = 0 To 8
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
        NomeUnivoco = xPrefissoNome & "CheckListRimborsoViaggio" & Year(Now) & Month(Now) & Day(Now) & Hour(Now) & Minute(Now) & Second(Now)
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
        Response.Redirect("WfrmCheckListElencoRimborsi.aspx")
    End Sub
    Protected Sub cmdInizializza_Click(sender As Object, e As EventArgs) Handles cmdInizializza.Click
        Dim sqlCMD As New SqlClient.SqlCommand
        Dim strNomeStore As String = "[SP_CHECKLIST_RIMBORSI_INIZIALIZZA]"

        Try
            Dim x As String
            Dim y As String

            sqlCMD = New SqlClient.SqlCommand(strNomeStore, CType(Session("conn"), SqlClient.SqlConnection))
            sqlCMD.CommandType = CommandType.StoredProcedure

            sqlCMD.Parameters.Add("@IDEntitàRimborso", SqlDbType.Int).Value = Request.QueryString("idEntitaRimborso")
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
        Dim strNomeStore As String = "[SP_CHECKLIST_RIMBORSI_CONFERMA_INDIVIDUALE]"

        Try
            Dim x As String
            Dim y As String

            sqlCMD = New SqlClient.SqlCommand(strNomeStore, CType(Session("conn"), SqlClient.SqlConnection))
            sqlCMD.CommandType = CommandType.StoredProcedure
            CercaIdLista()
            sqlCMD.Parameters.Add("@IdCheckList", SqlDbType.Int).Value = IdLista
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
            lblmessaggio.Visible = True
            lblmessaggio.Text = y


        Catch ex As Exception


            Response.Write(ex.Message.ToString())
            Exit Sub
        End Try
    End Sub
    Protected Sub CmdStampa_Click(sender As Object, e As EventArgs) Handles CmdStampa.Click
        Response.Write("<script type=""text/javascript"">")
        Response.Write("window.open(""WfrmReportistica.aspx?sTipoStampa=45&IdCheckList=" & Request.QueryString("idLista") & """, ""Report"", ""height=800,width=800, ,dependent=no,scrollbars=no,status=no,resizable=yes"")")
        Response.Write("</script>")
    End Sub
    Public Function CaricaDati()

        Dim Rimborso As DataSet
        Rimborso = CaricoLista(Request.QueryString("idEntitaRimborso"))

        If Rimborso.Tables(0).Rows.Count <> 0 Then

            lblCodCheckList.Text = Rimborso.Tables(0).Rows(0).Item("codicechecklist")
            idEntitaRimborso = Rimborso.Tables(0).Rows(0).Item("idEntitàRimborso")
            Data = Rimborso.Tables(0).Rows(0).Item("DataRiferimento")
            statoCheckList = Rimborso.Tables(0).Rows(0).Item("Stato")
            identita = Rimborso.Tables(0).Rows(0).Item("IdEntità")
            Importo = Rimborso.Tables(0).Rows(0).Item("ImportoConfermato")

        End If

        Rimborso = ChiSono(identita)
        If Rimborso.Tables(0).Rows.Count <> 0 Then
            Nome = Rimborso.Tables(0).Rows(0).Item("Nome")
            Cognome = Rimborso.Tables(0).Rows(0).Item("Cognome")
            CodiceVolontario = Rimborso.Tables(0).Rows(0).Item("CodiceVolontario")
        End If

        Dim StatoDesc As String

        Select Case statoCheckList
            Case "1"
                StatoDesc = "Da Lavorare"
            Case "2"
                StatoDesc = "In Lavorazione"
            Case "3"
                StatoDesc = "Confermata"
        End Select


        LblLista.Text = Nome & " " & Cognome & " Del " & Data & " Euro " & Importo & " " & StatoDesc
        Return statoCheckList
    End Function
    Public Sub CercaIdLista()
        Dim strsql As String
        Dim MyDataset As DataSet
        strsql = "SELECT IdCheckList FROM  CheckListRimborsiViaggio  where idEntitàRimborso=" & idEntitaRimborso
        MyDataset = ClsServer.DataSetGenerico(strsql, Session("conn"))

        If MyDataset.Tables(0).Rows.Count <> 0 Then
            IdLista = MyDataset.Tables(0).Rows(0).Item("IdCheckList")
        End If
    End Sub
End Class