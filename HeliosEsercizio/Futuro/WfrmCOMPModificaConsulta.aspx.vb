Imports System.Data.SqlClient
Imports System.Drawing
Imports System.IO
Imports System.Globalization
Imports System.Web.UI.WebControls.WebControl



Public Class WfrmCOMPModificaConsulta
    Inherits System.Web.UI.Page
    Dim strsql As String
    Dim dtsGenerico As DataSet
    Dim MyTransaction As System.Data.SqlClient.SqlTransaction
    Dim dtrgenerico As SqlClient.SqlDataReader
    Dim myCommand As System.Data.SqlClient.SqlCommand
    Dim dtsRisRicerca As DataSet
    Public Shared statoElaborazione As Integer

  
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Session("LogIn") Is Nothing Then
            If Session("LogIn") = False Then
                Response.Redirect("LogOn.aspx")
            End If
        Else
            Response.Redirect("LogOn.aspx")
        End If


        If IsPostBack = False Then
            If ClsUtility.ForzaCaricamentoPaghe(Session("Utente"), Session("conn")) = False Then
                Response.Redirect("wfrmAnomaliaDati.aspx")
            End If
            If Request.QueryString("IdElaborazione") <> "" Then
                CmdModifica.Visible = True
                CmdInserisci.Visible = False
                CaricaComboTipo()
		cboTipo.Enabled = False
                caricadati()

                Contatori()

                PagheRiproposte()

                CaricaGrigliaDettaglio()

                If statoElaborazione = 1 Then
                    CmdConferma.Visible = True
                End If
                If statoElaborazione <> 2 Then
                    CmdElimina.Visible = True
                End If

                Label1.Text = Request.QueryString("IdElaborazione")
                'CaricaGriglia(Request.QueryString("IdElaborazione"))
                CmdInserisci.Visible = False
                If dgInfoPaghe.Items.Count > 0 Then
                    CmdEsporta.Visible = True
                Else
                    'CmdEsporta.Visible = False
                End If
            Else 'inserimento
                CaricaComboTipo()
                'CaricaGriglia()
                CmdModifica.Visible = False
                CmdInserisci.Visible = True
                CmdRicercaVol.Visible = False
            End If


        Else
            Response.Write("<script language=""javascript"">" & vbCrLf)
            Response.Write("'$(function ADC () {'" & vbCrLf)
            Response.Write("'$(""#form"").keypress(function (e) {'" & vbCrLf)
            Response.Write("'if (e.which == 13) {'" & vbCrLf)
            Response.Write("'Return False'" & vbCrLf)
            Response.Write("'        }'" & vbCrLf)
            Response.Write("'    });'" & vbCrLf)
            Response.Write("'});'" & vbCrLf)
            Response.Write("</script>")
            lblmess.Visible = False
            lblmess.Text = ""
        End If

        If dgInfoPaghe.Items.Count > 0 Then
            CmdEsporta.Visible = True
        Else
            'CmdEsporta.Visible = False
        End If
    End Sub

    Private Sub CaricaComboTipo()
        strsql = "select IdTipoPagamento,Descrizione from COMP_TipiPagamento order by 1 "
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
        dtrgenerico = ClsServer.CreaDatareader(strsql, Session("conn"))
        cboTipo.DataSource = dtrgenerico
        cboTipo.DataTextField = "Descrizione"
        cboTipo.DataValueField = "IdTipoPagamento"
        cboTipo.DataBind()
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
    End Sub
    Private Sub caricadati()

        strsql = "SELECT COMP_Elaborazioni.IdStatoElaborazione, COMP_Elaborazioni.IdElaborazione, COMP_TipiPagamento.Descrizione AS Tipo, COMP_StatiElaborazioni.StatoElaborazione, COMP_Elaborazioni.DataValuta, COMP_Elaborazioni.Descrizione FROM COMP_Elaborazioni INNER JOIN COMP_TipiPagamento ON COMP_Elaborazioni.IdTipoPagamento = COMP_TipiPagamento.IdTipoPagamento INNER JOIN COMP_StatiElaborazioni ON COMP_Elaborazioni.IdStatoElaborazione = COMP_StatiElaborazioni.IdStatoElaborazione where idelaborazione=" & CInt(Request.QueryString("IdElaborazione")) & ""
        dtrgenerico = ClsServer.CreaDatareader(strsql, Session("conn"))
        If dtrgenerico.HasRows = True Then
            dtrgenerico.Read()

            TxtDescrizione.Text = IIf(Not IsDBNull(dtrgenerico("Descrizione")), dtrgenerico("Descrizione"), "")
            TxtDataValuta.Text = IIf(Not IsDBNull(dtrgenerico("DataValuta")), dtrgenerico("DataValuta"), "")
            cboTipo.SelectedItem.Text = IIf(Not IsDBNull(dtrgenerico("Tipo")), dtrgenerico("Tipo"), "")
            lblElaborazione.Text = " [ " & IIf(Not IsDBNull(dtrgenerico("StatoElaborazione")), dtrgenerico("StatoElaborazione"), "") & " ] "
            statoElaborazione = IIf(Not IsDBNull(CInt(dtrgenerico("IdStatoElaborazione"))), dtrgenerico("IdStatoElaborazione"), "")
            'hdStatoElaborazione.Value = statoElaborazione
        End If
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
    End Sub

    Protected Sub CmdInserisci_Click(sender As Object, e As EventArgs) Handles CmdInserisci.Click
        'insert
        If controlliSalvataggioServer() = True Then
            strsql = "Insert into COMP_Elaborazioni (IdStatoElaborazione,IdTipoPagamento,DataValuta,Descrizione,UserNameCreazioneRecord,DataCreazioneRecord) values (0,'" & cboTipo.SelectedValue & "', CONVERT(Datetime,'" & TxtDataValuta.Text & "',103) ,'" & TxtDescrizione.Text & "','" & Session("Utente") & "', getdate())"
            myCommand = ClsServer.EseguiSqlClient(strsql, Session("conn"))

            Dim newID As Integer
            'strsql = "select scope_identity() as id"

            'myCommand = ClsServer.EseguiSqlClient(strsql, Session("conn"))

            'newID = Convert.ToInt32(myCommand.ExecuteScalar())
            strsql = "Select SCOPE_IDENTITY() as newID "
            dtrgenerico = ClsServer.CreaDatareader(strsql, Session("conn"))
            dtrgenerico.Read()
            If dtrgenerico.HasRows = True Then
                newID = dtrgenerico("newID")
            End If

            dtrgenerico.Close()
            dtrgenerico = Nothing
            'Elaborazione(newID)
            ElaboraWS(newID)


            lblmess.Text = "PROBABILE ELABORAZIONE IN CORSO. PER VERIFICARNE LO STATO PREMERE IL TASTO CHIUDI IN MASCHERA"
            'lblmess.Visible = True
            lblmess.Visible = True
            CmdInserisci.Visible = False
            'puliscicampi()
            'CaricaGriglia()
        Else
            Exit Sub
        End If
    End Sub
    Private Sub puliscicampi()
        TxtDescrizione.Text = ""
        TxtDataValuta.Text = ""
        CmdModifica.Visible = False
    End Sub
    Function controlliSalvataggioServer() As Boolean

        If TxtDataValuta.Text.Trim = String.Empty Then
            lblmess.Visible = True
            lblmess.Text = "E' necessario inserire una Data Valuta."
            TxtDataValuta.Focus()
            Return False
        End If

        If TxtDescrizione.Text.Trim = String.Empty Then
            lblmess.Visible = True
            lblmess.Text = "E' necessario inserire una Descrizione."
            TxtDescrizione.Focus()
            Return False
        End If

        Return True
    End Function

    Protected Sub CmdModifica_Click(sender As Object, e As EventArgs) Handles CmdModifica.Click
        'update
        If controlliSalvataggioServer() = True Then
            strsql = "Update COMP_Elaborazioni  set DataValuta=Convert(DateTime,'" & TxtDataValuta.Text & "',103), Descrizione='" & TxtDescrizione.Text & "', DataCreazioneRecord=getdate()  where IdElaborazione=" & Request.QueryString("IdElaborazione") & " "
            myCommand = ClsServer.EseguiSqlClient(strsql, Session("conn"))
            'CmdModifica.Visible = False
            'CmdInserisci.Visible = True
            'Elaborazione()
            lblmess.Text = "MODIFICA EFFETTUATA."
            lblmess.Visible = True


        Else
            Exit Sub
        End If
    End Sub
    Private Function Elaborazione(ByVal nuovoID As Integer) As String
        Dim SqlCmd As New SqlClient.SqlCommand
        Try
            SqlCmd.CommandText = "SP_COMP_ELABORAZIONE"
            SqlCmd.CommandType = CommandType.StoredProcedure
            SqlCmd.Connection = Session("Conn")

            SqlCmd.Parameters.Add("@IdElaborazione", SqlDbType.Int).Value = nuovoID
            SqlCmd.Parameters.Add("@UsernameRichiesta", SqlDbType.VarChar).Value = Session("Utente")

            'Esito aggiornamento: 0-Errore 1-Aggiornamento effettuato
            SqlCmd.Parameters.Add("@Esito", SqlDbType.TinyInt)
            SqlCmd.Parameters("@Esito").Direction = ParameterDirection.Output

            SqlCmd.Parameters.Add("@messaggio", SqlDbType.VarChar)
            SqlCmd.Parameters("@messaggio").Size = 1000
            SqlCmd.Parameters("@messaggio").Direction = ParameterDirection.Output

            SqlCmd.ExecuteNonQuery()
            lblmess.Text = SqlCmd.Parameters("@messaggio").Value()
            lblmess.Visible = True
            'AbilitaDisabilitaSospensionePulsante(Request.QueryString("IdPaga"))
            CaricaGriglia(nuovoID)
            'CaricaGrigliaDettaglio(lblRiferimentoIdPaga.Text)
        Catch ex As Exception
            lblmess.Visible = True
            lblmess.Text = ex.Message

        End Try
    End Function
    Private Sub ElaboraWS(ByVal IdElaborazione As Integer)

        Dim localWS As New WS_Editor.WSMetodiDocumentazione
        Dim ResultAsinc As IAsyncResult
        localWS.Url = ConfigurationSettings.AppSettings("URL_WS_Documentazione")
        localWS.Timeout = 1000000
        ResultAsinc = localWS.BeginAsync_COMP_Elaborazione(IdElaborazione, Session("Utente"), Nothing, "")

    End Sub
    Protected Sub Chiudi_Click(sender As Object, e As EventArgs) Handles Chiudi.Click
        Response.Redirect("WfrmCOMPGestioneElaborazioni.aspx")
    End Sub
    Private Sub CaricaGriglia(ByVal IdElaborazione As Integer)

        'SP_COMP_VOLONTARI_ELABORAZIONE

        Dim sqlDAP As New SqlClient.SqlDataAdapter
        Dim dataSet As New DataSet
        Dim strNomeStore As String = "[SP_COMP_VOLONTARI_ELABORAZIONE]"

        dgInfoPaghe.CurrentPageIndex = 0
        Try
            sqlDAP = New SqlClient.SqlDataAdapter(strNomeStore, CType(Session("conn"), SqlClient.SqlConnection))
            sqlDAP.SelectCommand.CommandType = CommandType.StoredProcedure
            sqlDAP.SelectCommand.Parameters.Add("@IdElaborazione", SqlDbType.Char).Value = IdElaborazione


           
            If txtVolontario.Text <> "" Then
                sqlDAP.SelectCommand.Parameters.Add("@Volontario", SqlDbType.VarChar).Value = txtVolontario.Text
            End If
            If txtEnte.Text <> "" Then
                sqlDAP.SelectCommand.Parameters.Add("@Ente", SqlDbType.VarChar).Value = txtEnte.Text
            End If
            If txtCodiceProgetto.Text <> "" Then
                sqlDAP.SelectCommand.Parameters.Add("@Progetto", SqlDbType.VarChar).Value = txtCodiceProgetto.Text
            End If

            sqlDAP.Fill(dataSet)
            'Return dataSet
        Catch ex As Exception
            Response.Write(ex.Message.ToString())
            Exit Sub
        End Try

        dgInfoPaghe.DataSource = dataSet
        Session("appDtsElencoEnti") = dataSet
        dgInfoPaghe.DataBind()
        If dgInfoPaghe.Items.Count = 0 Then

            lblmess.Text = "Nessun Dato estratto."
            CmdEsporta.Visible = False
        Else
            lblmess.Text = "Risultato Ricerca Enti."
            CmdEsporta.Visible = True
        End If

        dgInfoPaghe.Visible = True

        Session("RisRicerca") = dataSet
        CaricaDataTablePerStampa(dataSet)

    End Sub

    Private Sub dgInfoPaghe_ItemCommand(source As Object, e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgInfoPaghe.ItemCommand
        If e.CommandName = "Dettaglio" Then
           
            Dim lunghezza As Integer
            lunghezza = e.Item.Cells(5).Text.Length
            Dim codEnte As String
            codEnte = ""
            For i As Integer = lunghezza - 8 To lunghezza - 2
                codEnte += e.Item.Cells(5).Text(i)
            Next

            Response.Redirect("WfrmCOMPInfoPaghe.aspx?IdElaborazione=" + e.Item.Cells(0).Text + "&IdPaga=" & e.Item.Cells(1).Text + "&IdVol=" & e.Item.Cells(2).Text + "&IdAttivita=" & e.Item.Cells(3).Text + "&Ente=" + codEnte)
        End If
    End Sub
    Protected Sub CmdElimina_Click(sender As Object, e As EventArgs) Handles CmdElimina.Click
        If statoElaborazione <> 2 Then
            Dim SqlCmd As New SqlClient.SqlCommand
            Try
                SqlCmd.CommandText = "SP_COMP_ELIMINAELABORAZIONE"
                SqlCmd.CommandType = CommandType.StoredProcedure
                SqlCmd.Connection = Session("Conn")

                SqlCmd.Parameters.Add("@IdElaborazione", SqlDbType.Int).Value = Request.QueryString("IdElaborazione")
                SqlCmd.Parameters.Add("@UsernameRichiesta", SqlDbType.VarChar).Value = Session("Utente")

                'Esito aggiornamento: 0-Errore 1-Aggiornamento effettuato
                SqlCmd.Parameters.Add("@Esito", SqlDbType.TinyInt)
                SqlCmd.Parameters("@Esito").Direction = ParameterDirection.Output

                SqlCmd.Parameters.Add("@messaggio", SqlDbType.VarChar)
                SqlCmd.Parameters("@messaggio").Size = 1000
                SqlCmd.Parameters("@messaggio").Direction = ParameterDirection.Output

                SqlCmd.ExecuteNonQuery()
                lblmess.Text = SqlCmd.Parameters("@messaggio").Value()
                lblmess.Visible = True
                'AbilitaDisabilitaSospensionePulsante(Request.QueryString("IdPaga"))
                'CaricaGriglia(Request.QueryString("IdElaborazione"))
                'CaricaGrigliaDettaglio(lblRiferimentoIdPaga.Text)
            Catch ex As Exception
                lblmess.Visible = True
                lblmess.Text = ex.Message

            End Try

            statoElaborazione = -1
            'hdStatoElaborazione.Value = -1
            CmdElimina.Visible = False
            CmdModifica.Visible = False
            CmdInserisci.Visible = False
            CmdConferma.Visible = False
            caricadati()
        End If
    End Sub

    Protected Sub CmdConferma_Click(sender As Object, e As EventArgs) Handles CmdConferma.Click
        If statoElaborazione = 1 Then

            'usare sored compelaborazione conferma SP_COMP_CONFERMA_ELABORAZIONE
            Dim SqlCmd As New SqlClient.SqlCommand
            Try
                SqlCmd.CommandText = "SP_COMP_CONFERMA_ELABORAZIONE"
                SqlCmd.CommandType = CommandType.StoredProcedure
                SqlCmd.Connection = Session("Conn")

                SqlCmd.Parameters.Add("@IdElaborazione", SqlDbType.Int).Value = Request.QueryString("IdElaborazione")
                SqlCmd.Parameters.Add("@UsernameRichiesta", SqlDbType.VarChar).Value = Session("Utente")

                'Esito aggiornamento: 0-Errore 1-Aggiornamento effettuato
                SqlCmd.Parameters.Add("@Esito", SqlDbType.TinyInt)
                SqlCmd.Parameters("@Esito").Direction = ParameterDirection.Output

                SqlCmd.Parameters.Add("@messaggio", SqlDbType.VarChar)
                SqlCmd.Parameters("@messaggio").Size = 1000
                SqlCmd.Parameters("@messaggio").Direction = ParameterDirection.Output

                SqlCmd.ExecuteNonQuery()
                lblmess.Text = SqlCmd.Parameters("@messaggio").Value()
                lblmess.Visible = True
                'AbilitaDisabilitaSospensionePulsante(Request.QueryString("IdPaga"))
                'CaricaGriglia(Request.QueryString("IdElaborazione"))
                'CaricaGrigliaDettaglio(lblRiferimentoIdPaga.Text)
            Catch ex As Exception
                lblmess.Visible = True
                lblmess.Text = ex.Message

            End Try

            ' strsql = "Update COMP_Elaborazioni  set IdStatoElaborazione= 2 ,UserNameConferma='" & Session("Utente") & "', DataConferma=getdate()  where IdElaborazione=" & Request.QueryString("IdElaborazione") & " "
            'myCommand = ClsServer.EseguiSqlClient(strsql, Session("conn"))

            lblmess.Text = "CONFERMA EFFETTUATA."
            lblmess.Visible = True


            statoElaborazione = -1
            'hdStatoElaborazione.Value = -1
            CmdElimina.Visible = False
            CmdModifica.Visible = False
            CmdConferma.Visible = False
            CmdInserisci.Visible = False
            caricadati()
        End If

    End Sub

    Protected Sub CmdEsporta_Click(sender As Object, e As EventArgs) Handles CmdEsporta.Click
        CaricaGriglia(Label1.Text)
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
    Sub CaricaDataTablePerStampa(ByVal DataSetDaScorrere As DataSet)
        Dim dt As New DataTable
        Dim dr As DataRow
        Dim i As Integer
        Dim x As Integer

        Dim NomeColonne(11) As String
        Dim NomiCampiColonne(5) As String

        'nome della colonna 
        'e posizione nella griglia di lettura

        NomeColonne(0) = "Elaborazione"
        NomeColonne(1) = "Volontario"
        NomeColonne(2) = "Ente"
        NomeColonne(3) = "Progetto"
        NomeColonne(4) = "Stato Paga"
        NomeColonne(5) = "Importo"


        NomiCampiColonne(0) = "IdElaborazione"
        NomiCampiColonne(1) = "Volontario"
        NomiCampiColonne(2) = "Ente"
        NomiCampiColonne(3) = "Progetto"
        NomiCampiColonne(4) = "StatoPaga"
        NomiCampiColonne(5) = "Importo"

        'carico i nomi delle colonne che andrò a stampare nella datagrid
        For x = 0 To 5
            dt.Columns.Add(New DataColumn(NomeColonne(x), GetType(String)))
        Next

        'carico il datatable con il risultato della query della ricerca, in qusto caso delle risorse
        If DataSetDaScorrere.Tables(0).Rows.Count > 0 Then
            For i = 1 To DataSetDaScorrere.Tables(0).Rows.Count
                dr = dt.NewRow()
                For x = 0 To 5
                    dr(x) = DataSetDaScorrere.Tables(0).Rows.Item(i - 1).Item(NomiCampiColonne(x))
                Next
                dt.Rows.Add(dr)
            Next
        End If

        'passo alla sessione la datatable che ho appena creato e che userò per il databinding della datagrid della stampa
        Session("DtbRicerca") = dt

    End Sub

    Private Sub dgInfoPaghe_PageIndexChanged(source As Object, e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgInfoPaghe.PageIndexChanged
        dgInfoPaghe.CurrentPageIndex = e.NewPageIndex
        dgInfoPaghe.DataSource = Session("RisRicerca")
        dgInfoPaghe.DataBind()
        dgInfoPaghe.SelectedIndex = -1
    End Sub
    Private Sub Contatori()
        strsql = "select COUNT(*) as NumeroPaghe, SUM(importo) as ImportoTotale from COMP_Paghe where IdElaborazione =" & CInt(Request.QueryString("IdElaborazione")) & ""
        dtrgenerico = ClsServer.CreaDatareader(strsql, Session("conn"))
        If dtrgenerico.HasRows = True Then
            dtrgenerico.Read()

            lblpaghe.Text = IIf(Not IsDBNull(dtrgenerico("NumeroPaghe")), dtrgenerico("NumeroPaghe"), "0")
            lblTotale.Text = IIf(Not IsDBNull(dtrgenerico("ImportoTotale")), dtrgenerico("ImportoTotale"), "0")

            Dim paghe As Double
            paghe = CDbl(lblpaghe.Text)
            Dim risultatopaghe As String 'uso la stessa per non dichiararne due
            risultatopaghe = paghe.ToString("#,#", CultureInfo.CreateSpecificCulture("it-IT"))
            lblpaghe.Text = CStr(risultatopaghe)

            Dim Totale As Double
            Totale = CDbl(lblTotale.Text)
            risultatopaghe = Totale.ToString("#,#.00", CultureInfo.CreateSpecificCulture("it-IT"))
            lblTotale.Text = CStr(risultatopaghe)


        End If
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If


        strsql = "select COUNT(*) as NumeroPagheEffettive from COMP_Paghe where IdElaborazione =" & CInt(Request.QueryString("IdElaborazione")) & " and importo>0" & ""
        dtrgenerico = ClsServer.CreaDatareader(strsql, Session("conn"))
        If dtrgenerico.HasRows = True Then
            dtrgenerico.Read()

            lblpagheeffettive.Text = IIf(Not IsDBNull(dtrgenerico("NumeroPagheEffettive")), dtrgenerico("NumeroPagheEffettive"), "0")

            Dim pagheeffettive As Double
            pagheeffettive = CDbl(lblpagheeffettive.Text)
            Dim risultatopagheeffettive As String 'uso la stessa per non dichiararne due
            risultatopagheeffettive = pagheeffettive.ToString("#,#", CultureInfo.CreateSpecificCulture("it-IT"))
            lblpagheeffettive.Text = CStr(risultatopagheeffettive)


        End If
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If


        strsql = "select count(distinct IdEntità) as NumeroVolontari from COMP_Paghe where IdElaborazione =" & CInt(Request.QueryString("IdElaborazione")) & ""
        dtrgenerico = ClsServer.CreaDatareader(strsql, Session("conn"))
        If dtrgenerico.HasRows = True Then
            dtrgenerico.Read()

            lblvolontari.Text = IIf(Not IsDBNull(dtrgenerico("NumeroVolontari")), dtrgenerico("NumeroVolontari"), "0")

            Dim volontari As Double
            volontari = CDbl(lblvolontari.Text)
            Dim risultatovolontari As String 'uso la stessa per non dichiararne due
            risultatovolontari = volontari.ToString("#,#", CultureInfo.CreateSpecificCulture("it-IT"))
            lblvolontari.Text = CStr(risultatovolontari)


        End If
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If


        strsql = "select count(distinct IdEntità) as NumeroVolontariEffettivi from COMP_Paghe where IdElaborazione =" & CInt(Request.QueryString("IdElaborazione")) & " and importo>0" & ""
        dtrgenerico = ClsServer.CreaDatareader(strsql, Session("conn"))
        If dtrgenerico.HasRows = True Then
            dtrgenerico.Read()

            lblvolontarieffettivi.Text = IIf(Not IsDBNull(dtrgenerico("NumeroVolontariEffettivi")), dtrgenerico("NumeroVolontariEffettivi"), "0")

            Dim volontarieffettivi As Double
            volontarieffettivi = CDbl(lblvolontarieffettivi.Text)
            Dim risultatovolontarieffettivi As String 'uso la stessa per non dichiararne due
            risultatovolontarieffettivi = volontarieffettivi.ToString("#,#", CultureInfo.CreateSpecificCulture("it-IT"))
            lblvolontarieffettivi.Text = CStr(risultatovolontarieffettivi)


        End If
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If


    End Sub

    Private Sub PagheRiproposte()
        strsql = "SELECT ISNULL(COUNT(*), 0) AS NumeroPaghe, ISNULL(SUM(importo), 0) AS ImportoTotale " & _
                 "FROM COMP_Paghe " & _
                 "WHERE IdElaborazione =" & CInt(Request.QueryString("IdElaborazione")) & _
                 " AND idpaga in (SELECT idpaga from COMP_StoricoPagheElaborazioni where NuovoIdElaborazione = " & CInt(Request.QueryString("IdElaborazione")) & ")"
        dtrgenerico = ClsServer.CreaDatareader(strsql, Session("conn"))
        If dtrgenerico.HasRows = True Then
            dtrgenerico.Read()
            lblNumeroPagheRiproposte.Text = Format(dtrgenerico("NumeroPaghe"), "#,##0")
            lblTotaleImportiRiproposti.Text = Format(dtrgenerico("ImportoTotale"), "#,##0.00")
        End If
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If

    End Sub

    Protected Sub CmdRicercaVol_Click(sender As Object, e As EventArgs) Handles CmdRicercaVol.Click
        ''SP_COMP_VOLONTARI_ELABORAZIONE

        'Dim sqlDAP As New SqlClient.SqlDataAdapter
        'Dim dataSet As New DataSet
        'Dim strNomeStore As String = "[SP_COMP_VOLONTARI_ELABORAZIONE]"

        'dgInfoPaghe.CurrentPageIndex = 0
        'Try
        '    sqlDAP = New SqlClient.SqlDataAdapter(strNomeStore, CType(Session("conn"), SqlClient.SqlConnection))
        '    sqlDAP.SelectCommand.CommandType = CommandType.StoredProcedure
        '    sqlDAP.SelectCommand.Parameters.Add("@IdElaborazione", SqlDbType.Char).Value = Label1.Text

        '    If txtVolontario.Text <> "" Then
        '        sqlDAP.SelectCommand.Parameters.Add("@Volontario", SqlDbType.VarChar).Value = txtVolontario.Text
        '    End If
        '    If txtEnte.Text <> "" Then
        '        sqlDAP.SelectCommand.Parameters.Add("@Ente", SqlDbType.VarChar).Value = txtEnte.Text
        '    End If
        '    If txtCodiceProgetto.Text <> "" Then
        '        sqlDAP.SelectCommand.Parameters.Add("@Progetto", SqlDbType.VarChar).Value = txtCodiceProgetto.Text
        '    End If

        '    sqlDAP.Fill(dataSet)
        '    'Return dataSet
        'Catch ex As Exception
        '    Response.Write(ex.Message.ToString())
        '    Exit Sub
        'End Try

        'dgInfoPaghe.DataSource = dataSet
        'Session("appDtsElencoEnti") = dataSet
        'dgInfoPaghe.DataBind()
        'If dgInfoPaghe.Items.Count = 0 Then

        '    lblmess.Text = "Nessun Dato estratto."

        'Else
        '    lblmess.Text = "Risultato Ricerca Enti."

        'End If

        'dgInfoPaghe.Visible = True

        'Session("RisRicerca") = dataSet
        'CaricaDataTablePerStampa(dataSet)





        CaricaGriglia(Label1.Text)
    End Sub

    Private Sub CmdRicercaVol0_Click(sender As Object, e As System.EventArgs) Handles CmdRicercaVol0.Click
        CaricaGriglia(Label1.Text)
    End Sub

    Private Sub CaricaGrigliaDettaglio()
        Dim SP_Popola_Griglia_Dettaglio As DataTable
        Dim SqlCmd1 As New SqlCommand
        Dim dataAdapter1 As SqlDataAdapter = New SqlDataAdapter
        SP_Popola_Griglia_Dettaglio = New DataTable

        SqlCmd1.CommandText = "SP_COMP_PROSPETTO_RIEPILOGATIVO"
        SqlCmd1.CommandType = CommandType.StoredProcedure
        SqlCmd1.Connection = Session("conn")
        SqlCmd1.Parameters.AddWithValue("@IdElaborazione", Request.QueryString("IdElaborazione"))
        dataAdapter1.SelectCommand = SqlCmd1
        dataAdapter1.Fill(SP_Popola_Griglia_Dettaglio)
        DtgDettagio.DataSource = SP_Popola_Griglia_Dettaglio
        DtgDettagio.DataBind()

        'If DtgDettagio.Rows.Count > 0 Then
        '    Session("VarSedi") = SP_Popola_Griglia_ELENCOVARIAZIONISEDI
        '    DtgDettagio.Caption = "Variazioni Sedi"
        DtgDettagio.Visible = True
        'Else
        '    DtgDettagio.Caption = "Nessuna Variazione Sedi"
        '    hlApriSedi.Visible = False
        'End If
    End Sub

End Class