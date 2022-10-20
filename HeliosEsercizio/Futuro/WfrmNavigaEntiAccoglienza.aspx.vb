Imports System.Data.SqlClient
Imports System.Drawing.Printing
Imports System.Drawing.Imaging
Imports System.Web.UI
Imports System.Drawing
Imports System.IO
Public Class WfrmNavigaEntiAccoglienza
    Inherits System.Web.UI.Page
    Dim strsql As String
    Dim dtrgenerico As SqlClient.SqlDataReader
    Dim dtsGenerico As DataSet
    Dim strquery As String  'stringa sql generica
    Dim identepadre As String
Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load




        Dim dtrgenerico As Data.SqlClient.SqlDataReader 'datareader generico

        If IsPostBack = False Then

            If Request.QueryString("VengoDa") = 1 Then
                Session("Mioidentepadre") = Request.QueryString("IdEnte")
            End If



            dtrgenerico = ClsServer.CreaDatareader("select idTipologieEnti='',Descrizione ='' union select idTipologieEnti,Descrizione from TipologieEnti", Session("conn"))
            ddltipologia.DataSource = dtrgenerico
            ddltipologia.DataValueField = "idTipologieEnti"
            ddltipologia.DataTextField = "Descrizione"
            ddltipologia.DataBind()

            If Not dtrgenerico Is Nothing Then
                dtrgenerico.Close()
                dtrgenerico = Nothing
            End If


            strquery = "SELECT classeAccreditamento FROM classiaccreditamento WHERE DefaultClasse <> 1 AND EntiInPartenariato <> 1 AND IDClasseAccreditamento > 4"


            'popolo combo classeaccreditamento
            Dim myCommand As New Data.SqlClient.SqlCommand(strquery, Session("conn"))
            dtrgenerico = myCommand.ExecuteReader()
            ddlCAccreditamento.Items.Add("")
            Do While dtrgenerico.Read()
                ddlCAccreditamento.Items.Add(dtrgenerico.GetValue(0))
            Loop
            dtrgenerico.Close()
            dtrgenerico = Nothing

            'myCommand.Connection.Close() 'DS


            ddlstato.Items.Add("")
            ddlstato.Items.Add("Attivo")
            ddlstato.Items.Add("Annullato")

            'Caricamento della combo per lo stato degli accordi
            CboStatoEnte.DataSource = ClsServer.CreaDataTable("Select IdStatoEnte,StatoEnte From StatiEnti " & _
                                                              "WHERE PresentazioneProgetti = 1 OR Sospeso = 1 OR " & _
                                                              "(PresentazioneProgetti = 0 AND DefaultStato = 0 AND Chiuso = 0 AND Sospeso = 0 AND Istruttoria = 0)", True, Session("conn"))
            CboStatoEnte.DataValueField = "IdStatoEnte"
            CboStatoEnte.DataTextField = "StatoEnte"
            CboStatoEnte.DataBind()



            txtdenominazione.Text = Request.QueryString("Denominazione")
            txtCodRegione.Text = Request.QueryString("CodiceRegione")
            TxtCodiceFiscale.Text = Request.QueryString("CF")
            ddltipologia.SelectedValue = Request.QueryString("Tipologia")
            ddlCAccreditamento.SelectedValue = Request.QueryString("ClasseAccreditamento")
            ddlstato.SelectedValue = Request.QueryString("Stato")
            CboStatoEnte.SelectedValue = Request.QueryString("StatoEnte")
            If Request.QueryString("Pagina") <> Nothing Then
                PopolaGriglia(1, Request.QueryString("Pagina"))
            Else
                PopolaGriglia(1, 0)
            End If




        End If
       


        If dgRicercaEnte.Items.Count = 0 Then
            CmdEsporta.Visible = False
        Else
            CmdEsporta.Visible = True
        End If



    End Sub

    Private Sub PopolaGriglia(ByVal bytVerifica As Byte, Optional ByVal bytpage As Integer = 0)
        Dim IdEnte As String
        If Request.QueryString("VengoDa") = 2 Then
            strquery = "SELECT IdEntePadre FROM entirelazioni WHERE IdEnteFiglio =" & Request.QueryString("IdEnte")
            If Not dtrgenerico Is Nothing Then
                dtrgenerico.Close()
                dtrgenerico = Nothing
            End If
            dtrgenerico = ClsServer.CreaDatareader(strquery, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
            If dtrgenerico.HasRows = True Then
                dtrgenerico.Read()
                IdEnte = dtrgenerico("IdEntePadre")
            End If
            If Not dtrgenerico Is Nothing Then
                dtrgenerico.Close()
                dtrgenerico = Nothing
            End If
        End If



        dtsGenerico = New DataSet()
        'se successivo o precedente setto pagina di arrivo a seconda del parametro
        If bytVerifica = 1 Then dgRicercaEnte.CurrentPageIndex = bytpage

        Dim strappo As String 'variabile appoggio generica
        Dim totsedi As String
        Dim strNomeStore As String = "SP_ACCREDITAMENTO_RICERCA_ENTE_IN_ACCORDO_NEW"
        Dim sqlDAP As New SqlClient.SqlDataAdapter

        'dgRicercaEnte.Columns(3).Visible = True
        'dgRicercaEnte.Columns(10).Visible = True


        Try
            sqlDAP = New SqlClient.SqlDataAdapter(strNomeStore, CType(Session("conn"), SqlClient.SqlConnection))
            sqlDAP.SelectCommand.CommandType = CommandType.StoredProcedure

            If Request.QueryString("VengoDa") = 2 Then
                sqlDAP.SelectCommand.Parameters.Add("@idEnte", SqlDbType.Int).Value = IdEnte
            Else
                sqlDAP.SelectCommand.Parameters.Add("@idEnte", SqlDbType.Int).Value = Request.QueryString("IdEnte")
            End If
            ' sqlDAP.SelectCommand.Parameters.Add("@idEnte", SqlDbType.Int).Value = Request.QueryString("IdEnte")


            If txtdenominazione.Text <> "" Then
                sqlDAP.SelectCommand.Parameters.Add("@denominazione", SqlDbType.NVarChar, 200).Value = Trim(txtdenominazione.Text)
            End If

            If txtCodRegione.Text <> "" Then
                sqlDAP.SelectCommand.Parameters.Add("@codRegione", SqlDbType.NVarChar, 10).Value = Trim(txtCodRegione.Text)
            End If

            If ddltipologia.SelectedItem.Text <> "" Then
                sqlDAP.SelectCommand.Parameters.Add("@tipologia", SqlDbType.NVarChar, 40).Value = ddltipologia.SelectedItem.Text
            End If

            If ddlCAccreditamento.SelectedItem.Text <> "" Then
                sqlDAP.SelectCommand.Parameters.Add("@classeAccreditamento", SqlDbType.NVarChar, 255).Value = ddlCAccreditamento.SelectedItem.Text
            End If

            If TxtCodiceFiscale.Text <> "" Then
                sqlDAP.SelectCommand.Parameters.Add("@codiceFiscale", SqlDbType.NVarChar, 50).Value = Trim(TxtCodiceFiscale.Text)
            End If

            If ddlstato.SelectedItem.Text <> "" Then
                sqlDAP.SelectCommand.Parameters.Add("@stato", SqlDbType.NVarChar, 20).Value = ddlstato.SelectedValue
            End If

            If CboStatoEnte.SelectedItem.Text <> "" Then
                sqlDAP.SelectCommand.Parameters.Add("@idStastoEnte", SqlDbType.Int).Value = CboStatoEnte.SelectedValue
            End If

            sqlDAP.SelectCommand.Parameters.Add("@paginaRicercaEnte", SqlDbType.NVarChar, 1).Value = CStr(bytpage)

            sqlDAP.Fill(dtsGenerico)

        Catch ex As Exception
            Response.Write(ex.Message.ToString())
            Exit Sub
        End Try


        strappo = dgRicercaEnte.CurrentPageIndex
        dgRicercaEnte.DataSource = dtsGenerico
        dgRicercaEnte.DataBind()
        dgRicercaEnte.Visible = True

        PreparaStampa()
        'controllo eventuale presenza di record
        If dgRicercaEnte.Items.Count = 0 Then
            dgRicercaEnte.Visible = False
            lblMessaggi.Text = "Non risultano esserci Enti in accordo"
            CmdEsporta.Visible = False
            If Request.QueryString("esporta") = "si" Then
                dgRicercaEnte.Columns(0).Visible = False
                'imgEsporta.Visible = False
            End If
        Else
            dgRicercaEnte.Visible = True
            lblMessaggi.Text = "Elenco Enti"
            CmdEsporta.Visible = True
            If Request.QueryString("esporta") = "si" Then
                'imgEsporta.Visible = True
                dgRicercaEnte.Columns(0).Visible = False
            End If
        End If

    End Sub

    Private Sub PreparaStampa()
        
        Dim NomeColonne(11) As String
        Dim NomiCampiColonne(11) As String
        'nome della colonna 
        'e posizione nella griglia di lettura
        NomeColonne(0) = "Cod. Ente"
        NomeColonne(1) = "Denominazione"
        NomeColonne(2) = "Tipo di Relazione"
        NomeColonne(3) = "Tipologia"
        NomeColonne(4) = "Classe"
        NomeColonne(5) = "Http"
        NomeColonne(6) = "E-Mail"
        NomeColonne(7) = "Totale Sedi"
        NomeColonne(8) = "Stato Accordo"
        NomeColonne(9) = "Stato Ente"
        NomeColonne(10) = "Data Inserimento"
        NomeColonne(11) = "Codice Fiscale"
        'NomeColonne(11) = "Tot Sedi"


        NomiCampiColonne(0) = "Codiceregione"
        NomiCampiColonne(1) = "Denominazione"
        NomiCampiColonne(2) = "tiporelazione"
        NomiCampiColonne(3) = "tipologia"
        NomiCampiColonne(4) = "Classeaccreditamento"
        NomiCampiColonne(5) = "http"
        NomiCampiColonne(6) = "email"
        NomiCampiColonne(7) = "numerototalesedi2"
        NomiCampiColonne(8) = "Stato"
        NomiCampiColonne(9) = "statoente"
        NomiCampiColonne(10) = "DataInizioValidita"
        NomiCampiColonne(11) = "CodiceFiscale"
        CaricaDataTablePerStampa(dtsGenerico, 11, NomeColonne, NomiCampiColonne)

        '*********************************************************************************

    End Sub

    Sub CaricaDataTablePerStampa(ByVal DataSetDaScorrere As DataSet, ByVal NColonne As Integer, ByVal NomiColonne() As String, ByVal NomiCampiColonne() As String)
        Dim dt As New DataTable
        Dim dr As DataRow
        Dim i As Integer
        Dim x As Integer

        'carico i nomi delle colonne che andrò a stampare nella datagrid
        For x = 0 To NColonne
            dt.Columns.Add(New DataColumn(NomiColonne(x), GetType(String)))
        Next

        'carico il datatable con il risultato della query della ricerca, in qusto caso delle risorse
        If DataSetDaScorrere.Tables(0).Rows.Count > 0 Then
            For i = 1 To DataSetDaScorrere.Tables(0).Rows.Count
                dr = dt.NewRow()
                For x = 0 To NColonne
                    dr(x) = DataSetDaScorrere.Tables(0).Rows.Item(i - 1).Item(NomiCampiColonne(x))
                Next
                dt.Rows.Add(dr)
            Next
        End If


        Session("DtbRicerca") = dt

    End Sub

    Private Sub cmdChiudi_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdChiudi.Click

        Response.Redirect("~/WfrmNavigaEnte.aspx?IdEnte=" & Request.QueryString("IdEnte") & "&VengoDa=" & 3 & "&CodiceFiscale=" & Request.QueryString("CodiceFiscale"))
       

    End Sub

    Private Sub dgRicercaEnte_ItemCommand(source As Object, e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgRicercaEnte.ItemCommand
        If e.CommandName = "NumeroTotSedi" Then
            'RicordaParametri()
            If (Session("TipoUtente") = "U") Then

                Response.Redirect("~/WfrmNavigaSedi.aspx?IdEnte=" & e.Item.Cells(9).Text & "&VengoDa=" & 3 & "&CodiceFiscale=" & Request.QueryString("CodiceFiscale") & "&Denominazione=" & txtdenominazione.Text & "&CodiceRegione=" & txtCodRegione.Text & "&CF=" & TxtCodiceFiscale.Text & "&Tipologia=" & ddltipologia.SelectedValue & "&ClasseAccreditamento=" & ddlCAccreditamento.SelectedValue & "&Stato=" & ddlstato.SelectedValue & "&Pagina=" & dgRicercaEnte.CurrentPageIndex & "&StatoEnte=" & CboStatoEnte.SelectedValue)
                


            Else
                Response.Redirect("page_error.aspx")
            End If
        End If
        If e.CommandName = "Info" Then
            'RicordaParametri()
            
            If (Session("TipoUtente") = "U") Then

                Response.Redirect("~/WfrmNavigaInfoEnte.aspx?IdEnte=" & e.Item.Cells(9).Text & "&VengoDa=" & 3 & "&CodiceFiscale=" & Request.QueryString("CodiceFiscale") & "&identepadre=" & Session("Mioidentepadre") & "&Denominazione=" & txtdenominazione.Text & "&CodiceRegione=" & txtCodRegione.Text & "&CF=" & TxtCodiceFiscale.Text & "&Tipologia=" & ddltipologia.SelectedValue & "&ClasseAccreditamento=" & ddlCAccreditamento.SelectedValue & "&Stato=" & ddlstato.SelectedValue & "&Pagina=" & dgRicercaEnte.CurrentPageIndex & "&StatoEnte=" & CboStatoEnte.SelectedValue)
            Else
                Response.Redirect("page_error.aspx")
            End If
        End If
    End Sub

    Private Sub dgRicercaEnte_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgRicercaEnte.SelectedIndexChanged
       
        If Not dgRicercaEnte.SelectedItem Is Nothing Then

            If Request.QueryString("azione") = "Ins" Then

                Response.Redirect("WfrmGestioneEnteinAccordo.aspx?azione=Ins&id=" & dgRicercaEnte.SelectedItem.Cells(9).Text)
           
            End If
        End If
    End Sub

    Private Sub dgRicercaEnte_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgRicercaEnte.PageIndexChanged
       
        PopolaGriglia(1, e.NewPageIndex)
    End Sub

    Private Sub CmdEsporta_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdEsporta.Click
        CmdEsporta.Visible = False
        Dim dtbRicerca As DataTable = Session("DtbRicerca")
        StampaCSV(dtbRicerca)
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
    Protected Sub cmdRicerca_Click(sender As Object, e As EventArgs) Handles cmdRicerca.Click
        'If Request.QueryString("Pagina") <> 0 Then
        '    PopolaGriglia(1, Request.QueryString("Pagina"))
        'Else
        PopolaGriglia(1, 0)
        'End If

    End Sub

End Class