Public Class WfrmAnomalieVolontari
    Inherits System.Web.UI.Page
    Dim dtsgenerico As DataSet
    Dim dtrgenerico As SqlClient.SqlDataReader
    Dim strsql As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Inserire qui il codice utente necessario per inizializzare la pagina
        Dim strCodFis As String

        If IsPostBack = False Then
            If Not Session("LogIn") Is Nothing Then
                If Session("LogIn") = False Then 'verifico validità log-in
                    Response.Redirect("LogOn.aspx")
                End If
            Else
                Response.Redirect("LogOn.aspx")
            End If
            Response.ExpiresAbsolute = #1/1/1980#
            Response.AddHeader("Pragma", "no-cache")
            If Request.QueryString("cflotus") <> "" Then
                If Request.QueryString("cflotus") = "0" Then
                    lblAnomaliaLotus.Text = "Assente"
                Else
                    lblAnomaliaLotus.Text = Request.QueryString("cflotus")
                End If
            End If
            'If Request.QueryString("eta") <> "" Then
            '    lblEta.Text = Request.QueryString("eta")
            'End If
            strsql = "Select isnull(entità.CodiceFiscale,'') as CodiceFiscale, entità.Cognome + ' ' + entità.nome as nominativo , entità.datanascita , entità.DataDomanda , entità.data_registrazione, entità.data_presa_in_carico, b.DataInizioVolontari, b.DataFineVolontari, entità.data_registrazione , entità.data_registrazione_portale_cliclavoro, entità.requisiti from entità inner join attività on entità.tmpcodiceprogetto = attività.codiceente inner join bandiattività as ba on attività.idbandoattività = ba.idbandoattività inner join bando b on ba.idbando = b.idbando where identità=" & Request.QueryString("identita") & ""
            If Not dtrgenerico Is Nothing Then
                dtrgenerico.Close()
                dtrgenerico = Nothing
            End If
            dtrgenerico = ClsServer.CreaDatareader(strsql, Session("conn"))
            If dtrgenerico.HasRows = True Then
                dtrgenerico.Read()
                lblNominativo.Text = UCase(dtrgenerico("nominativo"))
                lbldatanascitaVal.Text = dtrgenerico("datanascita")

                If IsDBNull(dtrgenerico("DataInizioVolontari")) = False Then
                    lblDataInizioVal.Text = dtrgenerico("DataInizioVolontari")
                End If
                If IsDBNull(dtrgenerico("DataFineVolontari")) = False Then
                    lbldatafineVal.Text = dtrgenerico("DataFineVolontari")
                End If
                If IsDBNull(dtrgenerico("DataDomanda")) = False Then
                    lblDataPresentazioneVal.Text = dtrgenerico("DataDomanda")
                End If

                If IsDBNull(dtrgenerico("data_registrazione")) = False Then
                    lblDataRegistrazioneVal.Text = dtrgenerico("data_registrazione")
               
                End If
                If IsDBNull(dtrgenerico("data_registrazione_portale_cliclavoro")) = False Then
                    lblDataPresainCaricoVal.Text = dtrgenerico("data_registrazione_portale_cliclavoro")

                End If
                If IsDBNull(dtrgenerico("Requisiti")) = False Then
                    lblrequisitiVol.Text = UCase(dtrgenerico("Requisiti"))

                End If

                strCodFis = dtrgenerico("CodiceFiscale")
            Else
                strCodFis = ""
            End If
            ControlliVol(strCodFis)
            CaricaGriglie(Request.QueryString("identita"), 0)
        End If
    End Sub
    Private Sub ControlliVol(ByVal strCodiceFiscale As String)
        'Esiste Record in GraduatorieEntità per stesso Volontario e Stesso Bando?
        strsql = " SELECT enti.codiceregione as codente,enti.denominazione,attività.titolo, " & _
        " attività.codiceEnte as codiceprogetto ,graduatorieentità.identità,bando.bando " & _
        " ,statientità.statoentità as Statovol, entità.DataInizioServizio as Datainziserv, " & _
        "case graduatorieentità.Stato when 1 then 'Si' else 'No' end as Idoneo, " & _
        "case graduatorieentità.Ammesso when 1 then 'Si' else 'No' end as Selez" & _
        " FROM graduatorieentità " & _
        " inner join entità on entità.identità = graduatorieentità.identità " & _
        " inner join attivitàsediassegnazione on " & _
        " (attivitàsediassegnazione.idattivitàsedeassegnazione=graduatorieentità.idattivitàsedeassegnazione)" & _
        " inner join entisedi on (entisedi.identesede=attivitàsediassegnazione.identesede) " & _
        " inner join enti on enti.idente=entisedi.idente " & _
        " inner join attività on (attività.idattività=attivitàsediassegnazione.idattività)" & _
        " inner join bandiAttività on (Attività.idbandoattività=bandiAttività.idbandoattività) " & _
        " inner join bando on (bando.idbando=bandiAttività.idbando)" & _
        " inner join statientità on entità.idstatoentità = statientità.idstatoentità" & _
        " where entità.codicefiscale='" & strCodiceFiscale & "'"
        '" where Attività.idattività=" & lblidattivita.Text & ")"
        '" where graduatorieentità.identità=" & dgRisultatoRicerca.Items(item.ItemIndex).Cells(19).Text & " and bando.idbando =(Select bando.idBando from Attività " & _
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
        dtsgenerico = ClsServer.DataSetGenerico(strsql, Session("conn"))
        dgVolGrad.DataSource = dtsgenerico
        dgVolGrad.DataBind()
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
    End Sub

    Private Sub CaricaGriglie(ByVal IdEntità As Integer, ByVal Tipo As Integer)

        Dim sqlDAP As SqlClient.SqlDataAdapter
        Dim dataSet As New DataSet
        Dim strNomeStore As String = "[SP_GG_DATI_CONTROLLI_VOLONTARIO]"

        Try
            sqlDAP = New SqlClient.SqlDataAdapter(strNomeStore, CType(Session("conn"), SqlClient.SqlConnection))
            sqlDAP.SelectCommand.CommandType = CommandType.StoredProcedure

            sqlDAP.SelectCommand.Parameters.Add("@IdEntità", SqlDbType.Int).Value = IdEntità
            sqlDAP.SelectCommand.Parameters.Add("@Tipo", SqlDbType.Int).Value = Tipo

            sqlDAP.Fill(dataSet)

            If dataSet.Tables(0).Rows.Count > 0 Then
                'DivControlloVolontario.Visible = True
                dtgControlloAnomaliaVolontari.DataSource = dataSet.Tables(0)
                dtgControlloAnomaliaVolontari.DataBind()
            End If


        Catch ex As Exception
           
            Exit Sub
        End Try
    End Sub

    Protected Sub IdVedi_Click(sender As Object, e As EventArgs) Handles IdVedi.Click
        IdVediUltimo.Visible = True
        IdVedi.Visible = False
        CaricaGriglie(Request.QueryString("identita"), 1)
    End Sub

    Protected Sub IdVediUltimo_Click(sender As Object, e As EventArgs) Handles IdVediUltimo.Click
        IdVediUltimo.Visible = False
        IdVedi.Visible = True
        CaricaGriglie(Request.QueryString("identita"), 0)
    End Sub
End Class