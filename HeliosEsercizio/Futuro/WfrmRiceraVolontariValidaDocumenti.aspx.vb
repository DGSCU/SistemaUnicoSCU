Imports System.Drawing
Imports System.IO

Public Class WfrmRiceraVolontariConvalidaDocumenti
    Inherits System.Web.UI.Page

    Const DOCUMENTO_ORE_FORMAZIONE_SPECIFICA As String = "FORMSPECVOL"
    Dim INDEX_DGVOLONTARI_DETTAGLIO_PRESENZE_RIMBORSO As Byte = 15
    Dim INDEX_DGVOLONTARI_ORE_FORMAZIONE_SPECIFICA As Byte = 16
    Dim INDEX_DGVOLONTARI_DOCUMENTO As Byte = 7

    'specchio ADC
    Const DOCUMENTO_ORE_FORMAZIONE As String = "FORMGENVOL"
    Dim INDEX_DGVOLONTARI_DETTAGLIO_PRESENZE_GEN As Byte = 15
    Dim INDEX_DGVOLONTARI_ORE_FORMAZIONE_GEN As Byte = 19
    Dim INDEX_DGVOLONTARI_DOCUMENTO_GEN As Byte = 7
   
    

    '**** creata da simona cordella il 23/04/2015
    '*** maschera di validazione della documentazione del volontario

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        Call TuttaPaginaSess()
        If (Session("TipoUtente") = "U" Or Session("TipoUtente") = "R") Then
            divTipoUtente.Visible = True
        Else
            divTipoUtente.Visible = False
        End If

        'Inserire qui il codice utente necessario per inizializzare la pagina
        If Not Session("LogIn") Is Nothing Then
            If Session("LogIn") = False Then 'verifico validità log-in
                Response.Redirect("LogOn.aspx")
            End If
        Else
            Response.Redirect("LogOn.aspx")
        End If
        If Page.IsPostBack = False Then
            CaricaComboTipoDocumenti()
            CaricaComboRegione()
            'LoadRiceraVolontari()
            If CStr(Request.QueryString("VengoDa")) = "ValidaDocumento" Then 'maschera delle presenze
                ImpostaFiltri()
                LoadRiceraVolontari()

            End If
            If CStr(Request.QueryString("VengoDa")) = "GestioneRimborso" Then 'maschera delle presenze
                ImpostaFiltri()
                LoadRiceraVolontari()

            End If
            If CStr(Request.QueryString("ProVengoDa")) = 4 Then 'maschera delle presenze
                ImpostaFiltri()
                LoadRiceraVolontari()

            End If
        End If

    End Sub
    Public Sub TuttaPaginaSess()
        Session("TP") = True
    End Sub
    'store ricerca per validazionedocumenti SP_RICERCA_VALIDAZIONE_DOCUMENTI_VOLONTARIO
    Private Sub LoadRiceraVolontari()
        'REALIZZATA DA: SIMONA CORDELLA 
        'DATA REALIZZAZIONE:  23/04/2015
        'FUNZIONALITA': RICHIAMO STORE PER LA RICERCA DEI VOLONTARI 
        Dim sqlDAP As New SqlClient.SqlDataAdapter
        Dim dataSet As New DataSet
        Dim strNomeStore As String = "[SP_RICERCA_VALIDAZIONE_DOCUMENTI_VOLONTARIO]"

        dgVolontari.CurrentPageIndex = 0

        Try
            sqlDAP = New SqlClient.SqlDataAdapter(strNomeStore, CType(Session("conn"), SqlClient.SqlConnection))
            sqlDAP.SelectCommand.CommandType = CommandType.StoredProcedure

            sqlDAP.SelectCommand.Parameters.Add("@Ente", SqlDbType.VarChar).Value = txtEnte.Text.Replace("'", "''")
            sqlDAP.SelectCommand.Parameters.Add("@CodiceEnte", SqlDbType.VarChar).Value = txtCodEnte.Text.Replace("'", "''")
            sqlDAP.SelectCommand.Parameters.Add("@Cognome", SqlDbType.VarChar).Value = txtCognome.Text.Replace("'", "''")
            sqlDAP.SelectCommand.Parameters.Add("@Nome", SqlDbType.VarChar).Value = txtNome.Text.Replace("'", "''")
            sqlDAP.SelectCommand.Parameters.Add("@CodiceVolontario", SqlDbType.VarChar).Value = TxtCodVolontario.Text.Replace("'", "''")
            sqlDAP.SelectCommand.Parameters.Add("@CodiceProgetto", SqlDbType.VarChar).Value = txtCodProgetto.Text.Replace("'", "''")
            sqlDAP.SelectCommand.Parameters.Add("@TitoloProgetto", SqlDbType.VarChar).Value = txtProgetto.Text.Replace("'", "''")
            sqlDAP.SelectCommand.Parameters.Add("@CodiceSede", SqlDbType.VarChar).Value = TxtCodiceSedeAttuazione.Text
            sqlDAP.SelectCommand.Parameters.Add("@IDEnte", SqlDbType.VarChar).Value = Session("IdEnte")
            sqlDAP.SelectCommand.Parameters.Add("@StatoDocumento", SqlDbType.VarChar).Value = ddlStatoDocumenti.SelectedValue
            sqlDAP.SelectCommand.Parameters.Add("@Documento", SqlDbType.VarChar).Value = IIf(ddlPrefissiDocumenti.SelectedValue = 0, "", ddlPrefissiDocumenti.SelectedValue)
            sqlDAP.SelectCommand.Parameters.Add("@DataInizioServizio", SqlDbType.VarChar).Value = TxtDataInizioServizio.Text
            sqlDAP.SelectCommand.Parameters.Add("@IdEntita", SqlDbType.VarChar).Value = ""
            sqlDAP.SelectCommand.Parameters.Add("@IdRegione", SqlDbType.VarChar).Value = IIf(ddlRegione.SelectedValue = 0, "", ddlRegione.SelectedValue)
            sqlDAP.Fill(dataSet)

            Session("appDtsRisRicerca") = dataSet
            dgVolontari.DataSource = dataSet
            dgVolontari.DataBind()

            If dgVolontari.Items.Count = 0 Then
                lblmessaggio.Text = "Nessun Dato estratto."
                CmdEsporta.Visible = False
            Else
                lblmessaggio.Text = "Risultato Ricerca Validazione Documenti."
                dgVolontari.Visible = True
                CmdEsporta.Visible = True
            End If
        Catch ex As Exception
            Response.Write(ex.Message.ToString())
            Exit Sub
        End Try
    End Sub

    'CARICAMENTO COMBO Tipologia Documenti
    Private Sub CaricaComboTipoDocumenti()
        Dim strSql As String
        Dim rstDoc As SqlClient.SqlDataReader

        ddlPrefissiDocumenti.Items.Clear()
        strSql = "SELECT idPrefisso as idPrefisso, left(PrefissiEntitàDocumenti.prefisso, charindex('_',PrefissiEntitàDocumenti.Prefisso)-1)  as Prefisso,	 ordine FROM  PrefissiEntitàDocumenti where tipoinserimento<>3 and davalidare=1 and Sistema='" & Session("Sistema") & "' UNION Select '', '',0 order by Ordine"
        rstDoc = ClsServer.CreaDatareader(strSql, Session("conn"))

        If rstDoc.HasRows Then
            ddlPrefissiDocumenti.DataSource = rstDoc
            ddlPrefissiDocumenti.DataTextField = "Prefisso"
            ddlPrefissiDocumenti.DataValueField = "IdPrefisso"
            ddlPrefissiDocumenti.DataBind()
        End If

        If Not rstDoc Is Nothing Then
            rstDoc.Close()
            rstDoc = Nothing
        End If
    End Sub
    'CARICAMENTO COMBO Regione competenza
    Private Sub CaricaComboRegione()
        Dim strSql As String
        Dim rstReg As SqlClient.SqlDataReader

        ddlRegione.Items.Clear()
        strSql = "SELECT IDRegione,regione  FROM REGIONI  where IDNazione =1 AND IDRegione<>140 UNION Select 0 as idregione, '' order by idregione"
        rstReg = ClsServer.CreaDatareader(strSql, Session("conn"))

        If rstReg.HasRows Then
            ddlRegione.DataSource = rstReg
            ddlRegione.DataTextField = "regione"
            ddlRegione.DataValueField = "IDRegione"
            ddlRegione.DataBind()
        End If

        If Not rstReg Is Nothing Then
            rstReg.Close()
            rstReg = Nothing
        End If
    End Sub
    Protected Sub cmdChiudi_Click(sender As Object, e As EventArgs) Handles cmdChiudi.Click
        'chiamo homepage
        Response.Redirect("WfrmMain.aspx")
    End Sub

    Protected Sub cmdRicerca_Click(sender As Object, e As EventArgs) Handles cmdRicerca.Click
        Lblmsg.Text = ""
        LoadRiceraVolontari()
    End Sub

    Private Sub dgVolontari_ItemCommand(source As Object, e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgVolontari.ItemCommand
        'Dim strSql As String
        'Dim rstDoc As SqlClient.SqlDataReader
        Dim msg As String
        Dim Esito As String

        Select Case e.CommandName
            Case "Volontario"
                Dim idstatodocumento As Integer = ddlStatoDocumenti.SelectedValue
                Dim documento As Integer = ddlPrefissiDocumenti.SelectedValue
                Dim idRegione As Integer = ddlRegione.SelectedValue
                If (Session("TipoUtente") = "U" Or Session("TipoUtente") = "R") Then
                    Session("IdEnte") = e.Item.Cells(1).Text
                    Session("Denominazione") = e.Item.Cells(3).Text
                End If
                'Response.Redirect("WfrmVisualizzaElencoDocumentiVolontario.aspx?IdVol=" & e.Item.Cells(0).Text & "&ProVengoDa=" & 4)
                Response.Redirect("WfrmVisualizzaElencoDocumentiVolontario.aspx?IdVol=" & e.Item.Cells(0).Text & "&Ente=" & txtEnte.Text & "&CodEnte=" & txtCodEnte.Text & "&Cognome=" & txtCognome.Text & "&Nome=" & txtNome.Text & "&CodVolontario=" & TxtCodVolontario.Text & "&CodProgetto=" & txtCodProgetto.Text & "&CodSede=" & TxtCodiceSedeAttuazione.Text & "&Doc=" & documento & "&StatoDoc=" & idstatodocumento & "&DataIS=" & TxtDataInizioServizio.Text & "&IdAttivita=" & e.Item.Cells(11).Text & "&IdEntita=" & e.Item.Cells(0).Text & "&IdRegione=" & idRegione & "&ProVengoDa=" & 4)


            Case "Download"
                Response.Write("<SCRIPT>" & vbCrLf)
                Response.Write("window.open('WfrmDocFileDownload.aspx?Origine=Volontario&IdDocumentoEntità=" & e.Item.Cells(2).Text & "','report','height=600,width=600,dependent=no,scrollbars=no,status=no,resizable=yes');" & vbCrLf)
                Response.Write("</SCRIPT>")
            Case "Valida"
                ClsServer.ConfermaDocumento(Session("Utente"), e.Item.Cells(2).Text, 1, e.Item.Cells(7).Text, e.Item.Cells(1).Text, Session("Conn"), msg, Esito)
                If Esito = "NEGATIVO" Then
                    Lblmsg.ForeColor = Color.Red
                Else
                    Lblmsg.ForeColor = Color.Navy
                End If
                Lblmsg.Visible = True
                Lblmsg.Text = msg
                LoadRiceraVolontari()
            Case "NonValida"
                ClsServer.ConfermaDocumento(Session("Utente"), e.Item.Cells(2).Text, 2, e.Item.Cells(7).Text, e.Item.Cells(1).Text, Session("Conn"), msg, Esito)
                If Esito = "NEGATIVO" Then
                    Lblmsg.ForeColor = Color.Red
                Else
                    Lblmsg.ForeColor = Color.Navy
                End If
                Lblmsg.Visible = True
                Lblmsg.Text = msg
                LoadRiceraVolontari()
            Case "Dettaglio"
                If e.Item.Cells(7).Text = "PRESENZE" Then
                    If (Session("TipoUtente") = "U" Or Session("TipoUtente") = "R") Then
                        Session("IdEnte") = e.Item.Cells(1).Text
                        Session("Denominazione") = e.Item.Cells(3).Text
                    End If

                    Dim strsql As String
                    Dim MyDataset As DataSet
                    Dim anno As String
                    Dim mese As String
                    strsql = "SELECT * from EntitàDocumenti where IdEntitàDocumento=" & e.Item.Cells(2).Text
                    MyDataset = ClsServer.DataSetGenerico(strsql, Session("conn"))

                    If MyDataset.Tables(0).Rows.Count <> 0 Then
                        anno = MyDataset.Tables(0).Rows(0).Item("anno")
                        mese = MyDataset.Tables(0).Rows(0).Item("mese")
                    End If
                    MyDataset.Dispose()
                    Dim idstatodocumento As Integer = ddlStatoDocumenti.SelectedValue
                    Dim documento As Integer = ddlPrefissiDocumenti.SelectedValue
                    Dim idRegione As Integer = ddlRegione.SelectedValue
                    'Richiamo dettaglio presenze
                    Response.Redirect("Presenze.aspx?Ente=" & txtEnte.Text & "&CodEnte=" & txtCodEnte.Text & "&Cognome=" & txtCognome.Text & "&Nome=" & txtNome.Text & "&CodVolontario=" & TxtCodVolontario.Text & "&CodProgetto=" & txtCodProgetto.Text & "&CodSede=" & TxtCodiceSedeAttuazione.Text & "&Doc=" & documento & "&StatoDoc=" & idstatodocumento & "&DataIS=" & TxtDataInizioServizio.Text & "&VengoDa=ValidaDocumento&Mese=" & mese & "&Anno=" & anno & "&IdEntita=" & e.Item.Cells(0).Text & "&IdRegione=" & idRegione)
                Else
                    If (Session("TipoUtente") = "U" Or Session("TipoUtente") = "R") Then
                        Session("IdEnte") = e.Item.Cells(1).Text
                        Session("Denominazione") = e.Item.Cells(3).Text
                    End If
                    Dim idstatodocumento As Integer = ddlStatoDocumenti.SelectedValue
                    Dim documento As Integer = ddlPrefissiDocumenti.SelectedValue
                    Dim idRegione As Integer = ddlRegione.SelectedValue
                    'richiamo dettaglio rimborsi
                    Response.Redirect("WfrmGestioneRimborsoVolontari.aspx?Ente=" & txtEnte.Text & "&CodEnte=" & txtCodEnte.Text & "&Cognome=" & txtCognome.Text & "&Nome=" & txtNome.Text & "&CodVolontario=" & TxtCodVolontario.Text & "&CodProgetto=" & txtCodProgetto.Text & "&CodSede=" & TxtCodiceSedeAttuazione.Text & "&Doc=" & documento & "&StatoDoc=" & idstatodocumento & "&DataIS=" & TxtDataInizioServizio.Text & "&IdAttivita=" & e.Item.Cells(11).Text & "&IdEntita=" & e.Item.Cells(0).Text & "&IdRegione=" & idRegione & "&VengoDa=ValidaDocumento&Op=" & Request.QueryString("Op"))
                End If
        End Select
    End Sub

    Protected Sub CmdEsporta_Click(sender As Object, e As EventArgs) Handles CmdEsporta.Click

        CmdEsporta.Visible = False
        CreaStampaCSV(Session("appDtsRisRicerca"))
        Dim dtbRicerca As DataTable = Session("DtbRicerca")
        StampaCSV(dtbRicerca)
    End Sub
    'creastampaCSV
    Private Sub CreaStampaCSV(ByVal MyDataSet As DataSet)
        Dim NomeColonne(8) As String
        Dim NomiCampiColonne(8) As String
        'nome della colonna 
        'e posizione nella griglia di lettura
        NomeColonne(0) = "Ente"
        NomeColonne(1) = "Codice Volontario"
        NomeColonne(2) = "Nominativo"
        NomeColonne(3) = "Cod. Fiscale"
        NomeColonne(4) = "Documento"
        NomeColonne(5) = "Riferimento Temporale"
        NomeColonne(6) = "Stato Documento"
        NomeColonne(7) = "IBAN"
        NomeColonne(8) = "Da Pagare"

        NomiCampiColonne(0) = "Ente"
        NomiCampiColonne(1) = "CodiceVolontario"
        NomiCampiColonne(2) = "Nominativo"
        NomiCampiColonne(3) = "CodiceFiscale"
        NomiCampiColonne(4) = "Tipodocumento"
        NomiCampiColonne(5) = "RiferimentoTemporale"
        NomiCampiColonne(6) = "statodocumento"
        NomiCampiColonne(7) = "iban"
        NomiCampiColonne(8) = "DaPagare"
        'carico un datatable che userò poi nella pagina di stampa
        'il numero delle colonne è a base 0
        Session("DtbRicerca") = ClsServer.CaricaDataTablePerStampa(MyDataSet, 8, NomeColonne, NomiCampiColonne)
    End Sub

    'StampaCSV
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
    'crea CSV
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

    'immposto filtri di ritorno dalla maschera presenze/assenze e riborsi
    Sub ImpostaFiltri()
        txtEnte.Text = Request.QueryString("Ente")
        txtCodEnte.Text = Request.QueryString("CodEnte")
        txtCognome.Text = Request.QueryString("Cognome")
        txtNome.Text = Request.QueryString("Nome")
        TxtCodVolontario.Text = Request.QueryString("CodVolontario")
        txtCodProgetto.Text = Request.QueryString("CodProgetto")
        TxtCodiceSedeAttuazione.Text = Request.QueryString("CodProgetto")
        ddlPrefissiDocumenti.SelectedValue = Request.QueryString("Doc")
        ddlStatoDocumenti.SelectedValue = Request.QueryString("StatoDoc")
        TxtDataInizioServizio.Text = Request.QueryString("DataIS")
        ddlRegione.SelectedValue = Request.QueryString("IdRegione")
    End Sub

    Private Sub dgVolontari_PageIndexChanged(source As Object, e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgVolontari.PageIndexChanged
        dgVolontari.CurrentPageIndex = e.NewPageIndex
        dgVolontari.DataSource = Session("appDtsRisRicerca")
        dgVolontari.DataBind()
        dgVolontari.SelectedIndex = -1
    End Sub

    Private Sub dgVolontari_ItemDataBound(ByVal source As Object, ByVal e As DataGridItemEventArgs) Handles dgVolontari.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            If String.Equals(e.Item.Cells(INDEX_DGVOLONTARI_DOCUMENTO).Text, DOCUMENTO_ORE_FORMAZIONE_SPECIFICA, StringComparison.InvariantCultureIgnoreCase) Then
                e.Item.Cells(INDEX_DGVOLONTARI_DETTAGLIO_PRESENZE_RIMBORSO).Text = e.Item.Cells(INDEX_DGVOLONTARI_ORE_FORMAZIONE_SPECIFICA).Text
                e.Item.Cells(INDEX_DGVOLONTARI_DETTAGLIO_PRESENZE_RIMBORSO).Visible = True
            End If
            If String.Equals(e.Item.Cells(INDEX_DGVOLONTARI_DOCUMENTO_GEN).Text, DOCUMENTO_ORE_FORMAZIONE, StringComparison.InvariantCultureIgnoreCase) Then
                e.Item.Cells(INDEX_DGVOLONTARI_DETTAGLIO_PRESENZE_GEN).Text = e.Item.Cells(INDEX_DGVOLONTARI_ORE_FORMAZIONE_GEN).Text
                e.Item.Cells(INDEX_DGVOLONTARI_DETTAGLIO_PRESENZE_GEN).Visible = True
            End If
        End If
    End Sub
End Class