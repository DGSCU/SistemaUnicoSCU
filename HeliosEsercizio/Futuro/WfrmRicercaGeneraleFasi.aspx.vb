Imports System.Data.SqlClient
Imports System.IO
Public Class WfrmRicercaGeneraleFasi
    Inherits System.Web.UI.Page
    Dim dataReader As SqlClient.SqlDataReader
    Dim dtsRisRicerca As DataSet
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
    Private Sub CancellaMessaggiInfo()
        msgErrore.Text = String.Empty
        msgInfo.Text = String.Empty
        msgConferma.Text = String.Empty
    End Sub
    Private Sub NascondiMenuLaterale()
        'Session("TP") = True
    End Sub
    Private Sub RicercaFasi()
        Dim query As String
        CancellaMessaggiInfo()
        ChiudiDataReader(dataReader)

        query = " Select e.idente,e.CodiceRegione,e.Denominazione,EntiFasi.IdEnteFase,"
        query &= " CASE EntiFasi.TipoFase "
        query &= "   WHEN 1 THEN 'Iscrizione' "
        query &= "   WHEN 2 THEN 'Adeguamento'	"
        query &= "   WHEN 3 THEN 'Art2' + ISNULL(' ('  + case ef.TipoFase when 1 then 'Iscr.' when 2 then 'Adeg.' end + ' Rif: ' + convert(varchar,EntiFasi.IdEnteFaseRiferimento)+ ')','')"
        query &= "   WHEN 4  THEN 'Art10' + ISNULL(' ('  + case ef.TipoFase when 1 then 'Iscr.' 	when 2 then 'Adeg.' end + ' Rif: ' + convert(varchar,EntiFasi.IdEnteFaseRiferimento)+ ')','') END "
        query &= "      as TipoFase , "
        query &= " CASE EntiFasi.stato  "
        query &= " WHEN 1 THEN CASE WHEN  GETDATE() between EntiFasi.DataInizioFase and EntiFasi.DataFineFase THEN 'Aperta' ELSE 'Scaduta' END  WHEN 2 THEN 'Annullata' WHEN 3 THEN  'Presentata' WHEN 4  THEN 'Valutata' END "
        query &= "    +'<br>'+CASE when pd.Id is null then 'non protocollata' 	when pd.NumeroProtocollo is null then 'in attesa di protocollazione'  else 'prot. n. '+pd.NumeroProtocollo+' del '+CONVERT(varchar(20), pd.dataProtocollo, 103) end "
        query &= " AS  StatoFase,EntiFasi.DataInizioFase,EntiFasi.DataFineFase, EntiFasi.DataValutazione as DataValutazione"
        query &= " FROM EntiFasi "
        query &= " LEFT JOIN EntiFasi ef on EntiFasi.IdEnteFaseRiferimento = ef.IdEnteFase"
        query &= " INNER JOIN enti e on e.IDEnte=EntiFasi.IdEnte"
        query &= " Left join ProtocolloDomanda pd on pd.idEnteFase = EntiFasi.idEnteFase "
        query &= " WHERE entifasi.stato <> 2 "





        If TxtDenominazioneEnte.Text <> "" Then
            query &= " AND e.Denominazione like '%" & TxtDenominazioneEnte.Text.Replace("'", "''") & "%'"
        End If
        If TxtCodiceEnte.Text <> "" Then
            query &= " AND e.CodiceRegione like '%" & TxtCodiceEnte.Text.Replace("'", "''") & "%'"
        End If
        
        If TxtRifFase.Text <> "" Then
            query &= " AND EntiFasi.IdEnteFase =  " & TxtRifFase.Text & ""
        End If

        If ddlFase.SelectedItem.Text <> "TUTTI" Then
            query &= " AND EntiFasi.TipoFase = " & ddlFase.SelectedValue & ""
        End If

        If ddlStatoFase.SelectedItem.Text <> "TUTTI" Then
            If ddlStatoFase.SelectedItem.Text = "Scaduta" Then
                query &= " AND (EntiFasi.stato = 1 AND NOT GETDATE() between EntiFasi.DataInizioFase and EntiFasi.DataFineFase )"
            Else
                query &= " AND EntiFasi.Stato = " & ddlStatoFase.SelectedValue & ""
            End If
        End If

        If txtDataInizioFaseDal.Text <> vbNullString And txtDataInizioFaseAl.Text <> vbNullString Then
            query &= " AND EntiFasi.DataInizioFase between '" & txtDataInizioFaseDal.Text & "' and '" & txtDataInizioFaseAl.Text & "' "
        Else
            If txtDataInizioFaseDal.Text <> vbNullString Then
                query &= " AND EntiFasi.DataInizioFase >= '" & txtDataInizioFaseDal.Text & "'"
            End If
            If txtDataInizioFaseAl.Text <> vbNullString Then
                query &= " AND EntiFasi.DataInizioFase <= '" & txtDataInizioFaseAl.Text & " 23:59:59'"
            End If

        End If
        If txtDataFineFaseDal.Text <> vbNullString And txtDataFineFaseAl.Text <> vbNullString Then
            query &= " AND EntiFasi.DataFineFase  between '" & txtDataFineFaseDal.Text & "' and '" & txtDataFineFaseAl.Text & "' "
        Else
            If txtDataFineFaseDal.Text <> vbNullString Then
                query &= " AND EntiFasi.DataFineFase >= '" & txtDataFineFaseDal.Text & "'"
            End If
            If txtDataFineFaseAl.Text <> vbNullString Then
                query &= " AND EntiFasi.DataFineFase <= '" & txtDataFineFaseAl.Text & " 23:59:59'"
            End If

        End If
        If txtDataValutazioneFaseDal.Text <> vbNullString And txtDataValutazioneFaseAl.Text <> vbNullString Then
            query &= " AND EntiFasi.DataValutazione between '" & txtDataValutazioneFaseDal.Text & "' and '" & txtDataValutazioneFaseAl.Text & "' "
        Else
            If txtDataValutazioneFaseDal.Text <> vbNullString Then
                query &= " AND EntiFasi.DataValutazione >= '" & txtDataValutazioneFaseDal.Text & "'"
            End If
            If txtDataValutazioneFaseAl.Text <> vbNullString Then
                query &= " AND EntiFasi.DataValutazione <= '" & txtDataValutazioneFaseAl.Text & " 23:59:59'"
            End If

        End If

        query &= " order by entifasi.datafinefase desc"

        dtsRisRicerca = ClsServer.DataSetGenerico(query, Session("conn"))


        Session("reader") = dtsRisRicerca

        dtgConsultaFasi.DataSource = dtsRisRicerca
        dtgConsultaFasi.DataBind()

        CaricaDataTablePerStampa(Session("reader"))


        If (dtgConsultaFasi.Items.Count > 0) Then
            cmdEsportaCSV.Visible = True
            ApriCSV1.Visible = False
            dtgConsultaFasi.Caption = "Elenco Fasi"
        Else
            cmdEsportaCSV.Visible = False
            ApriCSV1.Visible = False
            dtgConsultaFasi.Caption = "La ricerca non ha prodotto alcun risultato"
        End If

    End Sub
    Sub CaricaDataTablePerStampa(ByVal DataSetDaScorrere As DataSet)
        Dim dt As New DataTable
        Dim dr As DataRow
        Dim i As Integer
        Dim x As Integer

        Dim NomeColonne(7) As String
        Dim NomiCampiColonne(7) As String

        'nome della colonna 
        'e posizione nella griglia di lettura
        NomeColonne(0) = "Cod. Ente"
        NomeColonne(1) = "Ente"
        NomeColonne(2) = "Rif.Fase"
        NomeColonne(3) = "Fase"
        NomeColonne(4) = "Stato Fase"
        NomeColonne(5) = "DataInizioFase"
        NomeColonne(6) = "DataFineFase"
        NomeColonne(7) = "Data Validazione"


        NomiCampiColonne(0) = "codiceregione"
        NomiCampiColonne(1) = "denominazione"
        NomiCampiColonne(2) = "identefase"
        NomiCampiColonne(3) = "TipoFase"
        NomiCampiColonne(4) = "StatoFase"
        NomiCampiColonne(5) = "DataInizioFase"
        NomiCampiColonne(6) = "DataFineFase"
        NomiCampiColonne(7) = "DataValutazione"

        'carico i nomi delle colonne che andrò a stampare nella datagrid
        For x = 0 To 7
            dt.Columns.Add(New DataColumn(NomeColonne(x), GetType(String)))
        Next

        'carico il datatable con il risultato della query della ricerca, in qusto caso delle risorse
        If DataSetDaScorrere.Tables(0).Rows.Count > 0 Then
            For i = 1 To DataSetDaScorrere.Tables(0).Rows.Count
                dr = dt.NewRow()
                For x = 0 To 7
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
            cmdEsportaCSV.Visible = False
        Else
            xPrefissoNome = Session("Utente")
            path = Server.MapPath("download")
            url = CreaFileCSV(dtbRicerca, xPrefissoNome, path)
            ApriCSV1.Visible = True
            ApriCSV1.NavigateUrl = url
        End If
    End Sub

    Function CreaFileCSV(ByVal DTBRicerca As DataTable, ByVal xPrefissoNome As String, ByVal mapPath As String) As String

        Dim dtrSediAttuazione As Data.SqlClient.SqlDataReader
        Dim Writer As StreamWriter
        Dim xLinea As String
        Dim i As Int64
        Dim j As Int64
        Dim NomeUnivoco As String
        Dim Reader As StreamReader
        Dim url As String
        NomeUnivoco = xPrefissoNome & "ExpDatiFasi" & Year(Now) & Month(Now) & Day(Now) & Hour(Now) & Minute(Now) & Second(Now)
        Writer = New StreamWriter(mapPath & "\" & NomeUnivoco & ".CSV", False, System.Text.Encoding.Default)
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
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        VerificaSessione()
        NascondiMenuLaterale()
        If Page.IsPostBack = False Then

            'CaricaTipiDocumento()

        End If
    End Sub
    Protected Sub cmdRicerca_Click(sender As Object, e As EventArgs) Handles cmdRicerca.Click
        dtgConsultaFasi.CurrentPageIndex = 0
        RicercaFasi()
    End Sub

    Protected Sub cmdEsportaCSV_Click(sender As Object, e As EventArgs) Handles cmdEsportaCSV.Click
        cmdEsportaCSV.Visible = False
        Dim dtbRicerca As DataTable = Session("DtbRicerca")
        StampaCSV(dtbRicerca)
    End Sub

    'Private Sub dtgConsultaFasi_ItemCommand(source As Object, e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dtgConsultaFasi.ItemCommand
    '    Dim objHLink As HyperLink
    '    Dim msg As String
    '    Dim Esito As String
    '    msgErrore.Visible = False
    '    msgConferma.Visible = False
    '    Select Case e.CommandName
    '        Case "Download"
    '            objHLink = clsGestioneDocumentiAccreditamento.RecuperaDocumentoEnte(e.Item.Cells(2).Text, Session("Conn"))
    '            divDownloadFile.Visible = True
    '            hlScarica.Visible = True
    '            hlScarica.Text = objHLink.Text
    '            hlScarica.NavigateUrl = objHLink.NavigateUrl
    '    End Select
    'End Sub

    Private Sub dtgConsultaFasi_PageIndexChanged(source As Object, e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dtgConsultaFasi.PageIndexChanged
        dtgConsultaFasi.SelectedIndex = -1
        dtgConsultaFasi.EditItemIndex = -1
        dtgConsultaFasi.CurrentPageIndex = e.NewPageIndex
        'lblpage.Value = e.NewPageIndex

        RicercaFasi()
    End Sub

    Protected Sub cmdChiudi_Click(sender As Object, e As EventArgs) Handles cmdChiudi.Click
        Response.Redirect("WfrmMain.aspx")

    End Sub
End Class