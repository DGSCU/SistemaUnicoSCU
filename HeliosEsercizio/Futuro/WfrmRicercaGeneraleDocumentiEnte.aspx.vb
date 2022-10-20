Imports System.Data.SqlClient
Imports System.IO
Public Class WfrmRicercaGeneraleDocumentiEnte
    Inherits System.Web.UI.Page
    Dim dataReader As SqlClient.SqlDataReader
    Dim dtsRisRicerca As DataSet
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
    Private Sub CancellaMessaggiInfo()
        msgErrore.Text = String.Empty
        msgInfo.Text = String.Empty
        msgConferma.Text = String.Empty
    End Sub
    Private Sub NascondiMenuLaterale()
        Session("TP") = True
    End Sub

#End Region
#Region "ROUTINE_MASCHERA"
    Private Sub CaricaTipiDocumento()
        Dim StrSql As String
        ChiudiDataReader(dataReader)
        ddlTipoDoc.Items.Clear()
        StrSql = " SELECT idPrefisso as idPrefisso, Prefisso as Prefisso, ordine  FROM  PrefissiEntiDocumenti " & _
                 " WHERE ModalitàInvio in ('HELIOS','AUTOMATICO')  " & _
                 " UNION Select 0, 'Seleziona',0  order by Ordine"
        dataReader = ClsServer.CreaDatareader(StrSql, Session("conn"))


        If dataReader.HasRows Then
            ddlTipoDoc.DataSource = dataReader
            ddlTipoDoc.DataTextField = "Prefisso"
            ddlTipoDoc.DataValueField = "IdPrefisso"
            ddlTipoDoc.DataBind()
        End If
        ChiudiDataReader(dataReader)
    End Sub
    Private Sub RicercaDocumenti()
        Dim query As String
        CancellaMessaggiInfo()
        ChiudiDataReader(dataReader)

        query = " Select e.idente,a.IdEnteDocumento,e.CodiceRegione,e.Denominazione,EntiFasi.IdEnteFase,"
        query &= " CASE EntiFasi.TipoFase "
        query &= "   WHEN 1 THEN 'Iscrizione' "
        query &= "   WHEN 2 THEN 'Adeguamento'	"
        query &= "   WHEN 3 THEN 'Art2' + ISNULL(' ('  + case ef.TipoFase when 1 then 'Accr.' when 2 then 'Adeg.' end + ' Rif: ' + convert(varchar,EntiFasi.IdEnteFaseRiferimento)+ ')','')"
        query &= "   WHEN 4  THEN 'Art10' + ISNULL(' ('  + case ef.TipoFase when 1 then 'Accr.' 	when 2 then 'Adeg.' end + ' Rif: ' + convert(varchar,EntiFasi.IdEnteFaseRiferimento)+ ')','') END "
        query &= "     + ' dal:' + dbo.formatodata (EntiFasi.datainiziofase) + ' al:' + dbo.formatodata (EntiFasi.datafinefase) as TipoFase , "
        query &= " CASE EntiFasi.stato  "
        query &= " WHEN 1 THEN CASE WHEN  GETDATE() between EntiFasi.DataInizioFase and EntiFasi.DataFineFase THEN 'Aperta' ELSE 'Scaduta' END  WHEN 2 THEN 'Annullata' WHEN 3 THEN  'Presentata' WHEN 4  THEN 'Valutata' END AS  StatoFase, 	"
        query &= " Left(b.Prefisso,Len(b.Prefisso)-1) as Prefisso,  "
        query &= " CASE  "
        query &= "   WHEN b.DaValidare=0 THEN ''  "
        query &= "   WHEN a.Stato=0 then 'Da Validare' "
        query &= "   WHEN a.Stato=1 then 'Validato' "
        query &= "   WHEN a.Stato=2 then 'Non Valido' END as StatoDocumento,"
        query &= " a.FileName,a.DataInserimento,a.DataStato "
        query &= " FROM EntiFasi "
        query &= " LEFT JOIN EntiFasi ef on EntiFasi.IdEnteFaseRiferimento = ef.IdEnteFase"
        query &= " INNER JOIN EntiDocumenti a ON EntiFasi.IdEnteFase = A.IdEnteFase  "
        query &= " INNER JOIN enti E on e.IDEnte=EntiFasi.IdEnte"
        query &= " LEFT JOIN PrefissiEntiDocumenti b on left(a.filename, charindex('_',a.filename)) = b.prefisso "
        query &= " WHERE entifasi.stato <> 2 "


        If TxtDenominazioneEnte.Text <> "" Then
            query &= " AND E.Denominazione like '%" & TxtDenominazioneEnte.Text.Replace("'", "''") & "%'"
        End If
        If TxtCodiceEnte.Text <> "" Then
            query &= " AND e.CodiceRegione like  '" & TxtCodiceEnte.Text.Replace("'", "''") & "%'"
        End If
        If ddlTipoDoc.SelectedValue <> 0 Then
            query &= " AND b.Prefisso =  '" & ddlTipoDoc.SelectedItem.Text & "'"
        End If
        If ddlStatoDocumento.SelectedItem.Text <> "TUTTI" Then
            query &= " AND a.Stato =  " & ddlStatoDocumento.SelectedValue & ""
            If ddlStatoDocumento.SelectedItem.Text = "Da Validare" Then
                query &= " AND b.davalidare = 1 "
            End If
        End If
        If TxtRifFase.Text <> "" Then
            query &= " AND EntiFasi.IdEnteFase =  " & TxtRifFase.Text & ""
        End If
        If ddlFase.SelectedItem.Text <> "TUTTI" Then
            query &= " AND EntiFasi.TipoFase = " & ddlFase.SelectedValue & ""
        End If
        If ddlStatoFase.SelectedItem.Text <> "TUTTI" Then
            If ddlStatoFase.SelectedItem.Text = "Scaduta" Then
                query &= " (EntiFasi.stato = 1 OR GETDATE() between EntiFasi.DataInizioFase and EntiFasi.DataFineFase )"
            Else
                query &= " AND EntiFasi.Stato = " & ddlStatoFase.SelectedValue & ""
            End If
        End If

        If txtDataInsFileDal.Text <> vbNullString And txtDataInsFileAl.Text <> vbNullString Then
            query &= " AND a.DataInserimento between '" & txtDataInsFileDal.Text & "' and '" & txtDataInsFileAl.Text & "' "
        Else
            If txtDataInsFileDal.Text <> vbNullString Then
                query &= " AND a.DataInserimento >= '" & txtDataInsFileDal.Text & "'"
            End If
            If txtDataInsFileAl.Text <> vbNullString Then
                query &= " AND a.DataInserimento <= '" & txtDataInsFileAl.Text & "'"
            End If

        End If
        If txtDataValidazioneFileDal.Text <> vbNullString And txtDataValidazioneFileAl.Text <> vbNullString Then
            query &= " AND a.DataStato between '" & txtDataValidazioneFileDal.Text & "' and '" & txtDataValidazioneFileAl.Text & "' "
        Else
            If txtDataValidazioneFileDal.Text <> vbNullString Then
                query &= " AND a.DataStato >= '" & txtDataValidazioneFileDal.Text & "'"
            End If
            If txtDataValidazioneFileAl.Text <> vbNullString Then
                query &= " AND a.DataStato <= '" & txtDataValidazioneFileAl.Text & "'"
            End If

        End If

        query &= " order by entifasi.datafinefase desc, isnull(b.ordine,99) "

        dtsRisRicerca = ClsServer.DataSetGenerico(query, Session("conn"))


        Session("reader") = dtsRisRicerca

        dtgConsultaDocumenti.DataSource = dtsRisRicerca
        dtgConsultaDocumenti.DataBind()

        CaricaDataTablePerStampa(Session("reader"))


        If (dtgConsultaDocumenti.Items.Count > 0) Then
            cmdEsportaCSV.Visible = True
            ApriCSV1.Visible = False
            dtgConsultaDocumenti.Caption = "Elenco Documenti"
        Else
            cmdEsportaCSV.Visible = False
            ApriCSV1.Visible = False
            dtgConsultaDocumenti.Caption = "La ricerca non ha prodotto alcun risultato"
        End If

    End Sub
    Sub CaricaDataTablePerStampa(ByVal DataSetDaScorrere As DataSet)
        Dim dt As New DataTable
        Dim dr As DataRow
        Dim i As Integer
        Dim x As Integer

        Dim NomeColonne(9) As String
        Dim NomiCampiColonne(9) As String

        'nome della colonna 
        'e posizione nella griglia di lettura
        NomeColonne(0) = "Cod. Ente"
        NomeColonne(1) = "Ente"
        NomeColonne(2) = "Rif.Fase"
        NomeColonne(3) = "Fase"
        NomeColonne(4) = "Stato Fase"
        NomeColonne(5) = "Tipo Documetno"
        NomeColonne(6) = "Stato Documetno"
        NomeColonne(7) = "Nome File"
        NomeColonne(8) = "Data Inserimento File"
        NomeColonne(9) = "Data Validazione"


        NomiCampiColonne(0) = "codiceregione"
        NomiCampiColonne(1) = "denominazione"
        NomiCampiColonne(2) = "identefase"
        NomiCampiColonne(3) = "TipoFase"
        NomiCampiColonne(4) = "StatoFase"
        NomiCampiColonne(5) = "Prefisso"
        NomiCampiColonne(6) = "StatoDocumento"
        NomiCampiColonne(7) = "FileName"
        NomiCampiColonne(8) = "DataInserimento"
        NomiCampiColonne(9) = "DataStato"

        'carico i nomi delle colonne che andrò a stampare nella datagrid
        For x = 0 To 9
            dt.Columns.Add(New DataColumn(NomeColonne(x), GetType(String)))
        Next

        'carico il datatable con il risultato della query della ricerca, in qusto caso delle risorse
        If DataSetDaScorrere.Tables(0).Rows.Count > 0 Then
            For i = 1 To DataSetDaScorrere.Tables(0).Rows.Count
                dr = dt.NewRow()
                For x = 0 To 9
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
        NomeUnivoco = xPrefissoNome & "ExpDati" & Year(Now) & Month(Now) & Day(Now) & Hour(Now) & Minute(Now) & Second(Now)
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


#End Region
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim AlboEnte As String
        VerificaSessione()
        NascondiMenuLaterale()
        If Page.IsPostBack = False Then

            CaricaTipiDocumento()
            
        End If
    End Sub

    Protected Sub cmdRicerca_Click(sender As Object, e As EventArgs) Handles cmdRicerca.Click
        dtgConsultaDocumenti.CurrentPageIndex = 0
        RicercaDocumenti()
    End Sub

    Protected Sub cmdEsportaCSV_Click(sender As Object, e As EventArgs) Handles cmdEsportaCSV.Click
        cmdEsportaCSV.Visible = False
        Dim dtbRicerca As DataTable = Session("DtbRicerca")
        StampaCSV(dtbRicerca)
    End Sub

    Private Sub dtgConsultaDocumenti_ItemCommand(source As Object, e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dtgConsultaDocumenti.ItemCommand
        Dim objHLink As HyperLink
        Dim msg As String
        Dim Esito As String
        msgErrore.Visible = False
        msgConferma.Visible = False
        Select e.CommandName
            Case "Download"
                objHLink = clsGestioneDocumentiAccreditamento.RecuperaDocumentoEnte(e.Item.Cells(2).Text, Session("Conn"))
                divDownloadFile.Visible = True
                hlScarica.Visible = True
                hlScarica.Text = objHLink.Text
                hlScarica.NavigateUrl = objHLink.NavigateUrl
        End Select
    End Sub

    Private Sub dtgConsultaDocumenti_PageIndexChanged(source As Object, e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dtgConsultaDocumenti.PageIndexChanged
        dtgConsultaDocumenti.SelectedIndex = -1
        dtgConsultaDocumenti.EditItemIndex = -1
        dtgConsultaDocumenti.CurrentPageIndex = e.NewPageIndex
        'lblpage.Value = e.NewPageIndex

        RicercaDocumenti()
    End Sub

    Protected Sub cmdChiudi_Click(sender As Object, e As EventArgs) Handles cmdChiudi.Click
        Response.Redirect("WfrmMain.aspx")
    End Sub
End Class