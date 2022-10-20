Imports System.Drawing.Printing
Imports System.Drawing.Imaging
Imports System.Web.UI
Imports System.Drawing
Imports System.IO
Imports System.Data.SqlClient

Public Class wfrmVariazioniEnti

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
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        VerificaSessione()
        If IsPostBack = False Then
            Dim AlboEnte As String

            AlboEnte = ClsUtility.TrovaAlboEnte(Session("IdEnte"), Session("Conn"))
            CaricaGrigliaPadre(Request.QueryString("IdEnteFase"))
            CaricaGrigliaFigli(Request.QueryString("IdEnteFase"))
            CaricaGrigliaSedi(Request.QueryString("IdEnteFase"))
            CaricaGrigliaRisorse(Request.QueryString("IdEnteFase"))
            CaricaGrigliaSistemi(Request.QueryString("IdEnteFase"))
            If AlboEnte = "SCN" Then
                CaricaGrigliaServizi(Request.QueryString("IdEnteFase"))
            Else
                DtgELENCOVARIAZIONIServizi.Visible = False
            End If
            lblDenominazioneEnte.Text = Session("Denominazione")
            lblEnte.Text = Session("CodiceRegioneEnte")
            RecuperaDatiFaseEnte(Request.QueryString("IdEnteFase"))


        End If

    End Sub

    Private Sub CaricaGrigliaPadre(ByVal IdEnteFase As Integer)
        Dim SP_Popola_Griglia_ELENCOVARIAZIONIPADRE As DataTable
        Dim SqlCmd As New SqlCommand
        Dim dataAdapter As SqlDataAdapter = New SqlDataAdapter
        SP_Popola_Griglia_ELENCOVARIAZIONIPADRE = New DataTable

        SqlCmd.CommandText = "SP_ACCREDITAMENTO_ELENCOVARIAZIONI_PADRE"
        SqlCmd.CommandType = CommandType.StoredProcedure
        SqlCmd.Connection = Session("conn")
        SqlCmd.Parameters.AddWithValue("@IdEnte", Session("IdEnte"))
        SqlCmd.Parameters.AddWithValue("@IdEnteFase", IdEnteFase)
        dataAdapter.SelectCommand = SqlCmd
        dataAdapter.Fill(SP_Popola_Griglia_ELENCOVARIAZIONIPADRE)
        dtgELENCOVARIAZIONIPadre.DataSource = SP_Popola_Griglia_ELENCOVARIAZIONIPADRE
        dtgELENCOVARIAZIONIPadre.DataBind()
        If dtgELENCOVARIAZIONIPadre.Rows.Count > 0 Then
            dtgELENCOVARIAZIONIPadre.Caption = "Variazioni Ente Titolare"
            hlApriPadre.Visible = True
            Session("VarPadre") = SP_Popola_Griglia_ELENCOVARIAZIONIPADRE
        Else
            dtgELENCOVARIAZIONIPadre.Caption = "Nessuna Variazione Ente Titolare"
            hlApriPadre.Visible = False
        End If
    End Sub
    Private Sub CaricaGrigliaFigli(ByVal IdEnteFase As Integer)
        Dim SP_Popola_Griglia_ELENCOVARIAZIONIFIGLI As DataTable
        Dim SqlCmd As New SqlCommand
        Dim dataAdapter As SqlDataAdapter = New SqlDataAdapter
        SP_Popola_Griglia_ELENCOVARIAZIONIFIGLI = New DataTable

        SqlCmd.CommandText = "SP_ACCREDITAMENTO_ELENCOVARIAZIONI_FIGLI"
        SqlCmd.CommandType = CommandType.StoredProcedure
        SqlCmd.Connection = Session("conn")
        SqlCmd.Parameters.AddWithValue("@IdEnte", Session("IdEnte"))
        SqlCmd.Parameters.AddWithValue("@IdEnteFase", IdEnteFase)
        dataAdapter.SelectCommand = SqlCmd
        dataAdapter.Fill(SP_Popola_Griglia_ELENCOVARIAZIONIFIGLI)
        dtgELENCOVARIAZIONIFIGLI.DataSource = SP_Popola_Griglia_ELENCOVARIAZIONIFIGLI
        dtgELENCOVARIAZIONIFIGLI.DataBind()
        If dtgELENCOVARIAZIONIFIGLI.Rows.Count > 0 Then
            Session("VarFigli") = SP_Popola_Griglia_ELENCOVARIAZIONIFIGLI
            dtgELENCOVARIAZIONIFIGLI.Caption = "Variazioni Enti di Accoglienza"
            hlApriFigli.Visible = True
        Else
            dtgELENCOVARIAZIONIFIGLI.Caption = "Nessuna Variazione Enti di Accoglienza"
            hlApriFigli.Visible = False
        End If
    End Sub
    Private Sub CaricaGrigliaSedi(ByVal IdEnteFase As Integer)
        Dim SP_Popola_Griglia_ELENCOVARIAZIONISEDI As DataTable
        Dim SqlCmd1 As New SqlCommand
        Dim dataAdapter1 As SqlDataAdapter = New SqlDataAdapter
        SP_Popola_Griglia_ELENCOVARIAZIONISEDI = New DataTable

        SqlCmd1.CommandText = "SP_ACCREDITAMENTO_ELENCOVARIAZIONI_SEDI"
        SqlCmd1.CommandType = CommandType.StoredProcedure
        SqlCmd1.Connection = Session("conn")
        SqlCmd1.Parameters.AddWithValue("@IdEnte", Session("IdEnte"))
        SqlCmd1.Parameters.AddWithValue("@IdEnteFase", IdEnteFase)
        SqlCmd1.Parameters.AddWithValue("@Username", Session("Utente"))
        dataAdapter1.SelectCommand = SqlCmd1
        dataAdapter1.Fill(SP_Popola_Griglia_ELENCOVARIAZIONISEDI)
        DtgELENCOVARIAZIONISEDI.DataSource = SP_Popola_Griglia_ELENCOVARIAZIONISEDI
        DtgELENCOVARIAZIONISEDI.DataBind()

        If DtgELENCOVARIAZIONISEDI.Rows.Count > 0 Then
            Session("VarSedi") = SP_Popola_Griglia_ELENCOVARIAZIONISEDI
            DtgELENCOVARIAZIONISEDI.Caption = "Variazioni Sedi"
            hlApriSedi.Visible = True
        Else
            DtgELENCOVARIAZIONISEDI.Caption = "Nessuna Variazione Sedi"
            hlApriSedi.Visible = False
        End If
    End Sub
    Private Sub CaricaGrigliaRisorse(ByVal IdEnteFase As Integer)

        Dim SP_Popola_Griglia_ELENCOVARIAZIONIRISORSE As DataTable
        Dim SqlCmd2 As New SqlCommand
        Dim dataAdapter2 As SqlDataAdapter = New SqlDataAdapter
        SP_Popola_Griglia_ELENCOVARIAZIONIRISORSE = New DataTable

        SqlCmd2.CommandText = "SP_ACCREDITAMENTO_ELENCOVARIAZIONI_RISORSE"
        SqlCmd2.CommandType = CommandType.StoredProcedure
        SqlCmd2.Connection = Session("conn")
        SqlCmd2.Parameters.AddWithValue("@IdEnte", Session("IdEnte"))
        SqlCmd2.Parameters.AddWithValue("@IdEnteFase", IdEnteFase)
        dataAdapter2.SelectCommand = SqlCmd2
        dataAdapter2.Fill(SP_Popola_Griglia_ELENCOVARIAZIONIRISORSE)
        DtgELENCOVARIAZIONIRISORSE.DataSource = SP_Popola_Griglia_ELENCOVARIAZIONIRISORSE
        DtgELENCOVARIAZIONIRISORSE.DataBind()

        If DtgELENCOVARIAZIONIRISORSE.Rows.Count > 0 Then
            Session("VarRisorse") = SP_Popola_Griglia_ELENCOVARIAZIONIRISORSE
            DtgELENCOVARIAZIONIRISORSE.Caption = "Variazioni Risorse"
            hlApriRisorse.Visible = True
        Else
            DtgELENCOVARIAZIONIRISORSE.Caption = "Nessuna Variazione Risorse"
            hlApriRisorse.Visible = False
        End If
    End Sub
    Private Sub CaricaGrigliaServizi(ByVal IdEnteFase As Integer)
        Dim SP_Popola_Griglia_ELENCOVARIAZIONISERVIZIACQUISITI As DataTable
        Dim SqlCmd2 As New SqlCommand
        Dim dataAdapter2 As SqlDataAdapter = New SqlDataAdapter
        SP_Popola_Griglia_ELENCOVARIAZIONISERVIZIACQUISITI = New DataTable

        SqlCmd2.CommandText = "SP_ACCREDITAMENTO_ELENCOVARIAZIONI_SERVIZIACQUISITI"
        SqlCmd2.CommandType = CommandType.StoredProcedure
        SqlCmd2.Connection = Session("conn")
        SqlCmd2.Parameters.AddWithValue("@IdEnte", Session("IdEnte"))
        SqlCmd2.Parameters.AddWithValue("@IdEnteFase", IdEnteFase)
        dataAdapter2.SelectCommand = SqlCmd2
        dataAdapter2.Fill(SP_Popola_Griglia_ELENCOVARIAZIONISERVIZIACQUISITI)
        DtgELENCOVARIAZIONIServizi.DataSource = SP_Popola_Griglia_ELENCOVARIAZIONISERVIZIACQUISITI
        DtgELENCOVARIAZIONIServizi.DataBind()
        If DtgELENCOVARIAZIONIServizi.Rows.Count > 0 Then
            Session("VarServiziAqcuisiti") = SP_Popola_Griglia_ELENCOVARIAZIONISERVIZIACQUISITI
            DtgELENCOVARIAZIONIServizi.Caption = "Variazioni Servizi Acquisiti"
            hlApriServiziAcquisiti.Visible = True
        Else
            DtgELENCOVARIAZIONIServizi.Caption = "Nessuna Variazione Servizi Acquisiti"
            DtgELENCOVARIAZIONIServizi.Visible = True
            hlApriServiziAcquisiti.Visible = False
        End If
    End Sub

    Private Sub CaricaGrigliaSistemi(ByVal IdEnteFase As Integer)
        Dim SP_Popola_Griglia_ELENCOVARIAZIONISISTEMI As DataTable
        Dim SqlCmd2 As New SqlCommand
        Dim dataAdapter2 As SqlDataAdapter = New SqlDataAdapter
        SP_Popola_Griglia_ELENCOVARIAZIONISISTEMI = New DataTable

        SqlCmd2.CommandText = "SP_ACCREDITAMENTO_ELENCOVARIAZIONI_SISTEMI"
        SqlCmd2.CommandType = CommandType.StoredProcedure
        SqlCmd2.Connection = Session("conn")
        SqlCmd2.Parameters.AddWithValue("@IdEnte", Session("IdEnte"))
        SqlCmd2.Parameters.AddWithValue("@IdEnteFase", IdEnteFase)
        dataAdapter2.SelectCommand = SqlCmd2
        dataAdapter2.Fill(SP_Popola_Griglia_ELENCOVARIAZIONISISTEMI)
        DtgELENCOVARIAZIONISistemi.DataSource = SP_Popola_Griglia_ELENCOVARIAZIONISISTEMI
        DtgELENCOVARIAZIONISistemi.DataBind()
        If DtgELENCOVARIAZIONISistemi.Rows.Count > 0 Then
            Session("VarSistemi") = SP_Popola_Griglia_ELENCOVARIAZIONISISTEMI
            DtgELENCOVARIAZIONISistemi.Caption = "Variazioni Sistemi"
            hlApriSistemi.Visible = True
        Else
            DtgELENCOVARIAZIONISistemi.Caption = "Nessuna Variazione Sistemi"
            DtgELENCOVARIAZIONISistemi.Visible = True
            hlApriSistemi.Visible = False
        End If

    End Sub

    Sub gridView_RowCreated(ByVal source As Object, ByVal e As GridViewRowEventArgs) Handles dtgELENCOVARIAZIONIFIGLI.RowCreated,
                                                                                                dtgELENCOVARIAZIONIPadre.RowCreated,
                                                                                                DtgELENCOVARIAZIONIRISORSE.RowCreated,
                                                                                                DtgELENCOVARIAZIONISEDI.RowCreated,
                                                                                                DtgELENCOVARIAZIONIServizi.RowCreated,
                                                                                                DtgELENCOVARIAZIONISistemi.RowCreated

        If (e.Row.RowType = DataControlRowType.Pager) Then
            e.Row.Cells(0).Attributes.Add("align", "center")
        End If
    End Sub


    Private Sub dtgELENCOVARIAZIONIPadre_PageIndexChanged(ByVal source As Object, ByVal e As GridViewPageEventArgs) Handles dtgELENCOVARIAZIONIPadre.PageIndexChanging
        dtgELENCOVARIAZIONIPadre.SelectedIndex = -1
        dtgELENCOVARIAZIONIPadre.EditIndex = -1
        dtgELENCOVARIAZIONIPadre.PageIndex = e.NewPageIndex
        CaricaGrigliaPadre(Request.QueryString("IDEnteFase"))
    End Sub
    Private Sub dtgELENCOVARIAZIONIFIGLI_PageIndexChanged(ByVal source As Object, ByVal e As GridViewPageEventArgs) Handles dtgELENCOVARIAZIONIFIGLI.PageIndexChanging
        dtgELENCOVARIAZIONIFIGLI.SelectedIndex = -1
        dtgELENCOVARIAZIONIFIGLI.EditIndex = -1
        dtgELENCOVARIAZIONIFIGLI.PageIndex = e.NewPageIndex
        CaricaGrigliaFigli(Request.QueryString("IdEnteFase"))
    End Sub
    Private Sub DtgELENCOVARIAZIONIRISORSE_PageIndexChanged(ByVal source As Object, ByVal e As GridViewPageEventArgs) Handles DtgELENCOVARIAZIONIRISORSE.PageIndexChanging
        DtgELENCOVARIAZIONIRISORSE.SelectedIndex = -1
        DtgELENCOVARIAZIONIRISORSE.EditIndex = -1
        DtgELENCOVARIAZIONIRISORSE.PageIndex = e.NewPageIndex
        CaricaGrigliaRisorse(Request.QueryString("IdEnteFase"))
    End Sub
    Private Sub DtgELENCOVARIAZIONISEDI_PageIndexChanged(ByVal source As Object, ByVal e As GridViewPageEventArgs) Handles DtgELENCOVARIAZIONISEDI.PageIndexChanging
        DtgELENCOVARIAZIONISEDI.SelectedIndex = -1
        DtgELENCOVARIAZIONISEDI.EditIndex = -1
        DtgELENCOVARIAZIONISEDI.PageIndex = e.NewPageIndex
        CaricaGrigliaSedi(Request.QueryString("IdEnteFase"))
    End Sub
    Private Sub DtgELENCOVARIAZIONIServizi_PageIndexChanged(ByVal source As Object, ByVal e As GridViewPageEventArgs) Handles DtgELENCOVARIAZIONIServizi.PageIndexChanging
        DtgELENCOVARIAZIONIServizi.SelectedIndex = -1
        DtgELENCOVARIAZIONIServizi.EditIndex = -1
        DtgELENCOVARIAZIONIServizi.PageIndex = e.NewPageIndex
        CaricaGrigliaServizi(Request.QueryString("IdEnteFase"))
    End Sub

    Private Sub DtgELENCOVARIAZIONISistemi_PageIndexChanged(ByVal source As Object, ByVal e As GridViewPageEventArgs) Handles DtgELENCOVARIAZIONISistemi.PageIndexChanging
        DtgELENCOVARIAZIONISistemi.SelectedIndex = -1
        DtgELENCOVARIAZIONISistemi.EditIndex = -1
        DtgELENCOVARIAZIONISistemi.PageIndex = e.NewPageIndex
        CaricaGrigliaSistemi(Request.QueryString("IdEnteFase"))
    End Sub

    Private Sub cmdChiudi_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles cmdChiudi.Click
        Session("VarPadre") = Nothing
        Session("VarFigli") = Nothing
        Session("VarSedi") = Nothing
        Session("VarRisorse") = Nothing
        Session("VarServiziAqcuisiti") = Nothing
        Session("VarSistemi") = Nothing
        Response.Redirect("WfrmRiepilogoFasiEnte.aspx?VengoDa=" & Request.QueryString("VengoDa") & "&tipologia=" & Request.QueryString("tipologia"))
    End Sub

    Private Sub RecuperaDatiFaseEnte(ByVal IdEnteFase As Integer)
        Dim myQuery As String
        Dim myReader As SqlClient.SqlDataReader

        myQuery = "Select case TipoFase when 1 then 'Accreditamento' when 2 then 'Adeguamento' when 3 then  'Art2' when 4  then 'Art10' end  + ' dal:' + dbo.formatodata (datainiziofase) + ' al:' + dbo.formatodata (datafinefase) as TipoFase ,"
        myQuery &= " CASE STATO  When 1 then case when  GETDATE() between DataInizioFase and DataFineFase then 'Aperta' ELSE 'Scaduta' end  when 2 then 'Annullata' when 3 then  'Presentata'	when 4  then 'Valutata' end as Stato ,"
        myQuery &= " CASE DichiarazioneImpegno WHEN 1 then 'SI' when 0 then 'NO' ELSE 'Non Disponibile' END as DichiarazioneImpegno"
        myQuery &= " from entifasi where identefase =" & IdEnteFase & " "
        myReader = ClsServer.CreaDatareader(myQuery, Session("Conn"))

        If myReader.HasRows = True Then
            myReader.Read()
            lblRifFase.Text = IdEnteFase
            LblFase.Text = myReader("TipoFase")
            LblStatoFase.Text = myReader("Stato")
            DichiarazioneUnica.Text = myReader("DichiarazioneImpegno")

            If Session("TipoUtente") <> "U" Then
                labelDichiarazioneUnica.Visible = False
                DichiarazioneUnica.Visible = False
            End If

        End If
        ChiudiDataReader(myReader)
    End Sub

    Protected Sub CmdEsportaElenco_Click(sender As Object, e As EventArgs) Handles CmdEsportaElenco.Click
        rigaprint.Visible = True
        StampaElencoCSV()
        CmdEsportaElenco.Visible = False

    End Sub


    Private Sub StampaElencoCSV()
        EsportaPadri()
        EsportaFigli()
        EsportaSedi()
        EsportaRisorse()
        EsportaServiziAcquisiti()
        EsportaSistemi()

        Session("VarPadre") = Nothing
        Session("VarFigli") = Nothing
        Session("VarSedi") = Nothing
        Session("VarRisorse") = Nothing
        Session("VarServiziAqcuisiti") = Nothing
        Session("VarSistemi") = Nothing
    End Sub


    Private Sub EsportaPadri()
        Dim dtbRicercaPadri As DataTable = Session("VarPadre")
        If Session("VarPadre") Is Nothing Then
            Exit Sub
        Else

            StampaCSVPadre(dtbRicercaPadri)
        End If

    End Sub
    Private Sub EsportaFigli()
        Dim dtbRicercaFigli As DataTable = Session("VarFigli")
        If Session("VarFigli") Is Nothing Then
            Exit Sub
        Else
            StampaCSVFigli(dtbRicercaFigli)
        End If

    End Sub
    Private Sub EsportaSedi()
        Dim dtbRicercaSedi As DataTable = Session("VarSedi")

        If Session("VarSedi") Is Nothing Then
            Exit Sub
        Else
            StampaCSVSedi(dtbRicercaSedi)
        End If

    End Sub
    Private Sub EsportaRisorse()
        Dim dtbRicercaRisorse As DataTable = Session("VarRisorse")

        If Session("VarRisorse") Is Nothing Then
            Exit Sub
        Else
            StampaCSVRisorse(dtbRicercaRisorse)
        End If

    End Sub
    Private Sub EsportaServiziAcquisiti()
        Dim dtbRicercaServiziAcquisiti As DataTable = Session("VarServiziAqcuisiti")

        If Session("VarServiziAqcuisiti") Is Nothing Then
            Exit Sub
        Else
            StampaCSVServiziAcquisiti(dtbRicercaServiziAcquisiti)
        End If

    End Sub

    Private Sub EsportaSistemi()
        Dim dtbRicercaSistemi As DataTable = Session("VarSistemi")

        If Session("VarSistemi") Is Nothing Then
            Exit Sub
        Else
            StampaCSVSistemi(dtbRicercaSistemi)
        End If

    End Sub

    Private Sub StampaCSVPadre(ByVal dtbRicercaPadri As DataTable)
        Dim path As String
        Dim xPrefissoNome As String
        Dim url As String
        Dim utility As ClsUtility = New ClsUtility()
        xPrefissoNome = Session("Utente")
        path = Server.MapPath("download")
        url = CreaFileCSVPadre(dtbRicercaPadri, xPrefissoNome, path)
        hlApriPadre.Visible = True
        hlApriPadre.NavigateUrl = url
    End Sub
    Private Sub StampaCSVFigli(ByVal dtbRicercaFigli As DataTable)
        Dim path As String
        Dim xPrefissoNome As String
        Dim url As String
        Dim utility As ClsUtility = New ClsUtility()
        xPrefissoNome = Session("Utente")
        path = Server.MapPath("download")
        url = CreaFileCSVFigli(dtbRicercaFigli, xPrefissoNome, path)
        hlApriFigli.Visible = True
        hlApriFigli.NavigateUrl = url
    End Sub
    Private Sub StampaCSVSedi(ByVal dtbRicercaSedi As DataTable)
        Dim path As String
        Dim xPrefissoNome As String
        Dim url As String
        Dim utility As ClsUtility = New ClsUtility()
        xPrefissoNome = Session("Utente")
        path = Server.MapPath("download")
        url = CreaFileCSVSedi(dtbRicercaSedi, xPrefissoNome, path)
        hlApriSedi.Visible = True
        hlApriSedi.NavigateUrl = url
    End Sub
    Private Sub StampaCSVRisorse(ByVal dtbRicercaRisorse As DataTable)
        Dim path As String
        Dim xPrefissoNome As String
        Dim url As String
        Dim utility As ClsUtility = New ClsUtility()
        xPrefissoNome = Session("Utente")
        path = Server.MapPath("download")
        url = CreaFileCSVRisorse(dtbRicercaRisorse, xPrefissoNome, path)
        hlApriRisorse.Visible = True
        hlApriRisorse.NavigateUrl = url
    End Sub
    Private Sub StampaCSVServiziAcquisiti(ByVal dtbRicercaServiziAcquisiti As DataTable)
        Dim path As String
        Dim xPrefissoNome As String
        Dim url As String
        Dim utility As ClsUtility = New ClsUtility()
        xPrefissoNome = Session("Utente")
        path = Server.MapPath("download")
        url = CreaFileCSVServiziAcquisiti(dtbRicercaServiziAcquisiti, xPrefissoNome, path)
        hlApriServiziAcquisiti.Visible = True
        hlApriServiziAcquisiti.NavigateUrl = url
    End Sub

    Private Sub StampaCSVSistemi(ByVal dtbRicercaSistemi As DataTable)
        Dim path As String
        Dim xPrefissoNome As String
        Dim url As String
        Dim utility As ClsUtility = New ClsUtility()
        xPrefissoNome = Session("Utente")
        path = Server.MapPath("download")
        url = CreaFileCSVSistemi(dtbRicercaSistemi, xPrefissoNome, path)
        hlApriSistemi.Visible = True
        hlApriSistemi.NavigateUrl = url
    End Sub

    Function CreaFileCSVPadre(ByVal dtbRicercaPadri As DataTable, ByVal xPrefissoNome As String, ByVal mapPath As String) As String

        Dim writer As StreamWriter
        Dim xLinea As String = String.Empty
        Dim i As Int64
        Dim j As Int64
        Dim nomeUnivoco As String
        Dim url As String
        nomeUnivoco = xPrefissoNome & "ExpDatiVariazioniPadre" & Year(Now) & Month(Now) & Day(Now) & Hour(Now) & Minute(Now) & Second(Now)
        writer = New StreamWriter(mapPath & "\" & nomeUnivoco & ".CSV")
        'Creazione dell'inntestazione del CSV
        Dim intNumCol As Int64 = dtbRicercaPadri.Columns.Count
        For i = 0 To intNumCol - 1
            xLinea &= dtbRicercaPadri.Columns.Item(CInt(i)).ColumnName() & ";"
        Next
        writer.WriteLine(xLinea)
        xLinea = vbNullString

        'Scorro tutte le righe del datatable e riempio il CSV
        For i = 0 To dtbRicercaPadri.Rows.Count - 1

            For j = 0 To intNumCol - 1
                If IsDBNull(dtbRicercaPadri.Rows(CInt(i)).Item(CInt(j))) = True Then
                    xLinea &= vbNullString & ";"
                Else
                    xLinea &= ClsUtility.FormatExport(dtbRicercaPadri.Rows(CInt(i)).Item(CInt(j))) & ";"
                End If
            Next

            writer.WriteLine(xLinea)
            xLinea = vbNullString

        Next
        url = "download\" & nomeUnivoco & ".CSV"

        writer.Close()
        writer = Nothing
        Return url
    End Function
    Function CreaFileCSVFigli(ByVal dtbRicercaFigli As DataTable, ByVal xPrefissoNome As String, ByVal mapPath As String) As String

        Dim writer As StreamWriter
        Dim xLinea As String = String.Empty
        Dim i As Int64
        Dim j As Int64
        Dim nomeUnivoco As String
        Dim url As String
        nomeUnivoco = xPrefissoNome & "ExpDatiVariazioniEntiAccoglienza" & Year(Now) & Month(Now) & Day(Now) & Hour(Now) & Minute(Now) & Second(Now)
        writer = New StreamWriter(mapPath & "\" & nomeUnivoco & ".CSV")
        'Creazione dell'inntestazione del CSV
        Dim intNumCol As Int64 = dtbRicercaFigli.Columns.Count
        For i = 0 To intNumCol - 1
            xLinea &= dtbRicercaFigli.Columns.Item(CInt(i)).ColumnName() & ";"
        Next
        writer.WriteLine(xLinea)
        xLinea = vbNullString

        'Scorro tutte le righe del datatable e riempio il CSV
        For i = 0 To dtbRicercaFigli.Rows.Count - 1

            For j = 0 To intNumCol - 1
                If IsDBNull(dtbRicercaFigli.Rows(CInt(i)).Item(CInt(j))) = True Then
                    xLinea &= vbNullString & ";"
                Else
                    xLinea &= ClsUtility.FormatExport(dtbRicercaFigli.Rows(CInt(i)).Item(CInt(j))) & ";"
                End If
            Next

            writer.WriteLine(xLinea)
            xLinea = vbNullString

        Next
        url = "download\" & nomeUnivoco & ".CSV"

        writer.Close()
        writer = Nothing
        Return url
    End Function
    Function CreaFileCSVSedi(ByVal dtbRicercaSedi As DataTable, ByVal xPrefissoNome As String, ByVal mapPath As String) As String

        Dim writer As StreamWriter
        Dim xLinea As String = String.Empty
        Dim i As Int64
        Dim j As Int64
        Dim nomeUnivoco As String
        Dim url As String
        nomeUnivoco = xPrefissoNome & "ExpDatiVariazioniSedi" & Year(Now) & Month(Now) & Day(Now) & Hour(Now) & Minute(Now) & Second(Now)
        writer = New StreamWriter(mapPath & "\" & nomeUnivoco & ".CSV")
        'Creazione dell'inntestazione del CSV
        Dim intNumCol As Int64 = dtbRicercaSedi.Columns.Count
        For i = 0 To intNumCol - 1
            xLinea &= dtbRicercaSedi.Columns.Item(CInt(i)).ColumnName() & ";"
        Next
        writer.WriteLine(xLinea)
        xLinea = vbNullString

        'Scorro tutte le righe del datatable e riempio il CSV
        For i = 0 To dtbRicercaSedi.Rows.Count - 1

            For j = 0 To intNumCol - 1
                If IsDBNull(dtbRicercaSedi.Rows(CInt(i)).Item(CInt(j))) = True Then
                    xLinea &= vbNullString & ";"
                Else
                    xLinea &= ClsUtility.FormatExport(dtbRicercaSedi.Rows(CInt(i)).Item(CInt(j))) & ";"
                End If
            Next

            writer.WriteLine(xLinea)
            xLinea = vbNullString

        Next
        url = "download\" & nomeUnivoco & ".CSV"

        writer.Close()
        writer = Nothing
        Return url
    End Function
    Function CreaFileCSVRisorse(ByVal dtbRicercaRisorse As DataTable, ByVal xPrefissoNome As String, ByVal mapPath As String) As String

        Dim writer As StreamWriter
        Dim xLinea As String = String.Empty
        Dim i As Int64
        Dim j As Int64
        Dim nomeUnivoco As String
        Dim url As String
        nomeUnivoco = xPrefissoNome & "ExpDatiVariazioniRisorse" & Year(Now) & Month(Now) & Day(Now) & Hour(Now) & Minute(Now) & Second(Now)
        writer = New StreamWriter(mapPath & "\" & nomeUnivoco & ".CSV")
        'Creazione dell'inntestazione del CSV
        Dim intNumCol As Int64 = dtbRicercaRisorse.Columns.Count
        For i = 0 To intNumCol - 1
            xLinea &= dtbRicercaRisorse.Columns.Item(CInt(i)).ColumnName() & ";"
        Next
        writer.WriteLine(xLinea)
        xLinea = vbNullString

        'Scorro tutte le righe del datatable e riempio il CSV
        For i = 0 To dtbRicercaRisorse.Rows.Count - 1

            For j = 0 To intNumCol - 1
                If IsDBNull(dtbRicercaRisorse.Rows(CInt(i)).Item(CInt(j))) = True Then
                    xLinea &= vbNullString & ";"
                Else
                    xLinea &= ClsUtility.FormatExport(dtbRicercaRisorse.Rows(CInt(i)).Item(CInt(j))) & ";"
                End If
            Next

            writer.WriteLine(xLinea)
            xLinea = vbNullString

        Next
        url = "download\" & nomeUnivoco & ".CSV"

        writer.Close()
        writer = Nothing
        Return url

    End Function
    Function CreaFileCSVServiziAcquisiti(ByVal dtbRicercaServiziAcquisiti As DataTable, ByVal xPrefissoNome As String, ByVal mapPath As String) As String

        Dim writer As StreamWriter
        Dim xLinea As String = String.Empty
        Dim i As Int64
        Dim j As Int64
        Dim nomeUnivoco As String
        Dim url As String
        nomeUnivoco = xPrefissoNome & "ExpDatiVariazioniServiziAcquisiti" & Year(Now) & Month(Now) & Day(Now) & Hour(Now) & Minute(Now) & Second(Now)
        writer = New StreamWriter(mapPath & "\" & nomeUnivoco & ".CSV")
        'Creazione dell'inntestazione del CSV
        Dim intNumCol As Int64 = dtbRicercaServiziAcquisiti.Columns.Count
        For i = 0 To intNumCol - 1
            xLinea &= dtbRicercaServiziAcquisiti.Columns.Item(CInt(i)).ColumnName() & ";"
        Next
        writer.WriteLine(xLinea)
        xLinea = vbNullString

        'Scorro tutte le righe del datatable e riempio il CSV
        For i = 0 To dtbRicercaServiziAcquisiti.Rows.Count - 1

            For j = 0 To intNumCol - 1
                If IsDBNull(dtbRicercaServiziAcquisiti.Rows(CInt(i)).Item(CInt(j))) = True Then
                    xLinea &= vbNullString & ";"
                Else
                    xLinea &= ClsUtility.FormatExport(dtbRicercaServiziAcquisiti.Rows(CInt(i)).Item(CInt(j))) & ";"
                End If
            Next

            writer.WriteLine(xLinea)
            xLinea = vbNullString

        Next
        url = "download\" & nomeUnivoco & ".CSV"

        writer.Close()
        writer = Nothing
        Return url
    End Function

    Function CreaFileCSVSistemi(ByVal dtbRicercaSistemi As DataTable, ByVal xPrefissoNome As String, ByVal mapPath As String) As String

        Dim writer As StreamWriter
        Dim xLinea As String = String.Empty
        Dim i As Int64
        Dim j As Int64
        Dim nomeUnivoco As String
        Dim url As String
        nomeUnivoco = xPrefissoNome & "ExpDatiVariazioniSistemi" & Year(Now) & Month(Now) & Day(Now) & Hour(Now) & Minute(Now) & Second(Now)
        writer = New StreamWriter(mapPath & "\" & nomeUnivoco & ".CSV")
        'Creazione dell'inntestazione del CSV
        Dim intNumCol As Int64 = dtbRicercaSistemi.Columns.Count
        For i = 0 To intNumCol - 1
            xLinea &= dtbRicercaSistemi.Columns.Item(CInt(i)).ColumnName() & ";"
        Next
        writer.WriteLine(xLinea)
        xLinea = vbNullString

        'Scorro tutte le righe del datatable e riempio il CSV
        For i = 0 To dtbRicercaSistemi.Rows.Count - 1

            For j = 0 To intNumCol - 1
                If IsDBNull(dtbRicercaSistemi.Rows(CInt(i)).Item(CInt(j))) = True Then
                    xLinea &= vbNullString & ";"
                Else
                    xLinea &= ClsUtility.FormatExport(dtbRicercaSistemi.Rows(CInt(i)).Item(CInt(j))) & ";"
                End If
            Next

            writer.WriteLine(xLinea)
            xLinea = vbNullString

        Next
        url = "download\" & nomeUnivoco & ".CSV"

        writer.Close()
        writer = Nothing
        Return url
    End Function

End Class
