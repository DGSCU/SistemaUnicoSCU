Imports System.Data.SqlClient

Public Class riepilogoassenzeprogetto
    Inherits System.Web.UI.Page


    Protected WithEvents lblTotRegistrate As System.Web.UI.WebControls.Label
    Protected WithEvents lblTotConfermate As System.Web.UI.WebControls.Label
    Dim dataReader As SqlClient.SqlDataReader
    Dim querySql As String
    Dim dataSet As DataSet

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
#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load
        VerificaSessione()
        If Request.QueryString("idattivita") <> "" Then
            Dim strATTIVITA As Integer = -1
            Dim strBANDOATTIVITA As Integer = -1
            Dim strENTEPERSONALE As Integer = -1
            Dim strENTITA As Integer = -1
            Dim strIDENTE As Integer = -1

            If ClsUtility.SICUREZZA_VERIFICA_AUTORIZZAZIONI(Session("conn"), Session("IdEnte"), Session("txtCodEnte"), Request.QueryString("idattivita"), strBANDOATTIVITA, strENTEPERSONALE, strENTITA, strIDENTE) = 1 Then
                ChiudiDataReader(dataReader)
            Else
                ChiudiDataReader(dataReader)
                Response.Redirect("wfrmAnomaliaDati.aspx")

            End If


        End If
        If Page.IsPostBack = False Then
            caricadati()

            caricagrigliagiornicausale()
            caricagrigliagiorniperiodo()
        End If
    End Sub

    Private Sub caricadati()
        Dim dataOdierna As Date
        ChiudiDataReader(dataReader)
        querySql = "select a.titolo, a.datainizioattività, a.datafineattività,  getdate()  as dataOdierna " & _
        " from attività as a " & _
        " where a.idattività=" & CInt(Request.QueryString("idattivita")) & ""
        dataReader = ClsServer.CreaDatareader(querySql, Session("conn"))
        If dataReader.HasRows = True Then
            dataReader.Read()
            lblProgetto.Text = IIf(Not IsDBNull(dataReader("titolo")), dataReader("titolo"), "")
            lblDataInizio.Text = IIf(Not IsDBNull(dataReader("datainizioattività")), dataReader("datainizioattività"), "")
            lbldataFine.Text = IIf(Not IsDBNull(dataReader("datafineattività")), dataReader("datafineattività"), "")

            dataOdierna = dataReader("dataOdierna")
        End If
        ChiudiDataReader(dataReader)
        If CDate(lblDataInizio.Text) > dataOdierna Then
            divData.Visible = False
        Else
            divData.Visible = True
        End If
    End Sub

    Sub caricagrigliagiorniperiodo()
        querySql = "select distinct  a.Anno, "
        querySql = querySql & "(case a.Mese "
        querySql = querySql & "when 1 then 'Gennaio' "
        querySql = querySql & "when 2 then 'Febbraio' "
        querySql = querySql & "when 3 then 'Marzo' "
        querySql = querySql & "when 4 then 'Aprile' "
        querySql = querySql & "when 5 then 'Maggio' "
        querySql = querySql & "when 6 then 'Giugno' "
        querySql = querySql & "when 7 then 'Luglio' "
        querySql = querySql & "when 8 then 'Agosto' "
        querySql = querySql & "when 9 then 'Settembre' "
        querySql = querySql & "when 10 then 'Ottobre' "
        querySql = querySql & "when 11 then 'Novembre' "
        querySql = querySql & "when 12 then 'Dicembre' "
        querySql = querySql & "end) as MeseTesto, "
        querySql = querySql & "isnull((select sum(Giorni) from entitàassenze where Stato=1 and entitàassenze.identità=a.IdEntità and Mese=a.Mese and Anno=a.Anno),0) as Registrate, "
        querySql = querySql & "isnull((select sum(Giorni) from entitàassenze where Stato=2 and entitàassenze.identità=a.IdEntità and Mese=a.Mese and Anno=a.Anno),0) as Confermate, a.mese "
        querySql = querySql & "from EntitàAssenze as a where Stato in (1,2) and a.identità in (select distinct s1.identità from attivitàentità s1 inner join attivitàentisediattuazione s2 on s1.idattivitàentesedeattuazione = s2.idattivitàentesedeattuazione where s2.idattività =" & CInt(Request.QueryString("idattivita")) & ") "
        querySql = querySql & "order by a.Anno, a.Mese"
        dataSet = ClsServer.DataSetGenerico(querySql, Session("conn"))
        If dataSet.Tables(0).Rows.Count > 0 Then
            dtgPeriodoAssenze.DataSource = dataSet
            dtgPeriodoAssenze.DataBind()
            Dim item As DataGridItem
            Dim intTotRegistrate As Integer
            Dim intTotConfermate As Integer
            For Each item In dtgPeriodoAssenze.Items
                intTotRegistrate = intTotRegistrate + CInt(dtgPeriodoAssenze.Items(item.ItemIndex).Cells(2).Text()) + 0
                intTotConfermate = intTotConfermate + CInt(dtgPeriodoAssenze.Items(item.ItemIndex).Cells(3).Text()) + 0
            Next
            lblTotConfermate.Text = intTotConfermate.ToString
            lblTotRegistrate.Text = intTotRegistrate.ToString
        Else
            PulisciDataGridPeriodo(dtgPeriodoAssenze)
        End If
        dataSet.Dispose()
    End Sub

    Sub caricagrigliagiornicausale()
        querySql = "select distinct  b.Descrizione as Causale, "
        querySql = querySql & "isnull((select sum(Giorni) from entitàassenze where Stato=1 and entitàassenze.identità=a.IdEntità and IdCausale=a.IdCausale),0) as Registrate, "
        querySql = querySql & "isnull((select sum(Giorni) from entitàassenze where Stato=2 and entitàassenze.identità=a.IdEntità and IdCausale=a.IdCausale),0) as Confermate "
        querySql = querySql & "from EntitàAssenze as a "
        querySql = querySql & "inner join causali as b on a.Idcausale=b.IdCausale "
        querySql = querySql & "where a.Stato in (1,2) and a.identità in (select distinct s1.identità from attivitàentità s1 inner join attivitàentisediattuazione s2 on s1.idattivitàentesedeattuazione = s2.idattivitàentesedeattuazione where s2.idattività =" & CInt(Request.QueryString("idattivita")) & ")"
        dataSet = ClsServer.DataSetGenerico(querySql, Session("conn"))
        If dataSet.Tables(0).Rows.Count > 0 Then
            dtgCausaliAssenze.DataSource = dataSet
            dtgCausaliAssenze.DataBind()
        Else
            PulisciDataGridCausale(dtgCausaliAssenze)
        End If
        dataSet.Dispose()
    End Sub

    Sub PulisciDataGridPeriodo(ByVal GridDaPulire As DataGrid)
        Dim dtRigheVuote As New DataTable
        Dim drRigheVuote As DataRow
        Dim i As Integer
        dtRigheVuote.Columns.Add()
        dtRigheVuote.Columns.Add(New DataColumn("Anno", GetType(String)))
        dtRigheVuote.Columns.Add(New DataColumn("MeseTesto", GetType(String)))
        dtRigheVuote.Columns.Add(New DataColumn("Registrate", GetType(String)))
        dtRigheVuote.Columns.Add(New DataColumn("Confermate", GetType(String)))
        drRigheVuote = dtRigheVuote.NewRow()
        drRigheVuote(1) = ""
        drRigheVuote(2) = "Nessuna Assenza Registrata."
        drRigheVuote(3) = ""
        drRigheVuote(4) = ""
        dtRigheVuote.Rows.Add(drRigheVuote)
        For i = 1 To 9
            drRigheVuote = dtRigheVuote.NewRow()
            drRigheVuote(1) = ""
            drRigheVuote(2) = ""
            drRigheVuote(3) = ""
            drRigheVuote(4) = ""
            dtRigheVuote.Rows.Add(drRigheVuote)
        Next
        GridDaPulire.DataSource = dtRigheVuote
        GridDaPulire.DataBind()
        GridDaPulire.SelectedIndex = -1
    End Sub

    Sub PulisciDataGridCausale(ByVal GridDaPulire As DataGrid)
        Dim dtRigheVuote As New DataTable
        Dim drRigheVuote As DataRow
        Dim i As Integer
        dtRigheVuote.Columns.Add()
        dtRigheVuote.Columns.Add(New DataColumn("Causale", GetType(String)))
        dtRigheVuote.Columns.Add(New DataColumn("Registrate", GetType(String)))
        dtRigheVuote.Columns.Add(New DataColumn("Confermate", GetType(String)))
        drRigheVuote = dtRigheVuote.NewRow()
        drRigheVuote(1) = ""
        drRigheVuote(2) = "Nessuna Assenza Registrata."
        drRigheVuote(3) = ""
        dtRigheVuote.Rows.Add(drRigheVuote)
        For i = 1 To 9
            drRigheVuote = dtRigheVuote.NewRow()
            drRigheVuote(1) = ""
            drRigheVuote(2) = ""
            drRigheVuote(3) = ""
            dtRigheVuote.Rows.Add(drRigheVuote)
        Next
        GridDaPulire.DataSource = dtRigheVuote
        GridDaPulire.DataBind()
        GridDaPulire.SelectedIndex = -1
    End Sub

    Protected Sub cmdChiudi_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdChiudi.Click
        Response.Redirect(ClsUtility.TrovaAlboProgetto(Request.QueryString("IdAttivita"), Session("Conn")) & "?Modifica=" & Request.QueryString("Modifica") & "&Nazionale=" & Request.QueryString("Nazionale") & "&IdAttivita=" & CInt(Request.QueryString("IdAttivita")) & "&VengoDa=" & Request.QueryString("VengoDa"))
        'Response.Redirect("TabProgetti.aspx?Modifica=" & Request.QueryString("Modifica") & "&Nazionale=" & Request.QueryString("Nazionale") & "&IdAttivita=" & CInt(Request.QueryString("IdAttivita")) & "&VengoDa=" & Request.QueryString("VengoDa"))
    End Sub
End Class