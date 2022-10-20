Imports System.IO

Public Class WFrmAssociaEntiComuni
    Inherits System.Web.UI.Page
    Dim dtsGenerico As DataSet
    Dim dtrgenerico As SqlClient.SqlDataReader
    Dim strsql As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not Session("LogIn") Is Nothing Then
            If Session("LogIn") = False Then
                Response.Redirect("LogOn.aspx")
            End If
        Else
            Response.Redirect("LogOn.aspx")
        End If
        Response.ExpiresAbsolute = #1/1/1980#
        Response.AddHeader("Pragma", "no-cache")

        Dim SelComune As New clsSelezionaComune
        Dim blnEstero As Boolean = False

        ApriCSV1.Visible = False

        If Request.QueryString("Id") <> "" Or (Request.QueryString("Id2") <> "" And Request.QueryString("Id2") <> "-1") Then



            If Request.QueryString("Id") <> "" Then


                'IDENTE  Session("IdEnte")
                'CODICEENTE  NZ Session("codiceregione")  or Session("txtCodEnte")

                Dim strATTIVITA As Integer = -1
                Dim strBANDOATTIVITA As Integer = -1
                Dim strENTEPERSONALE As Integer = -1
                Dim strENTITA As Integer = -1
                Dim strIDENTE As Integer = -1


                If ClsUtility.SICUREZZA_VERIFICA_AUTORIZZAZIONI(Session("conn"), Session("IdEnte"), Session("txtCodEnte"), strATTIVITA, strBANDOATTIVITA, strENTEPERSONALE, strENTITA, Request.QueryString("Id")) = 1 Then

                    If Not dtrgenerico Is Nothing Then
                        dtrgenerico.Close()
                        dtrgenerico = Nothing
                    End If
                Else
                    If Not dtrgenerico Is Nothing Then
                        dtrgenerico.Close()
                        dtrgenerico = Nothing
                    End If
                    Response.Redirect("wfrmAnomaliaDati.aspx")

                End If

            End If


            If Request.QueryString("Id2") <> "" Then


                'IDENTE  Session("IdEnte")
                'CODICEENTE  NZ Session("codiceregione")  or Session("txtCodEnte")

                Dim strATTIVITA As Integer = -1
                Dim strBANDOATTIVITA As Integer = -1
                Dim strENTEPERSONALE As Integer = -1
                Dim strENTITA As Integer = -1
                Dim strIDENTE As Integer = -1


                If ClsUtility.SICUREZZA_VERIFICA_AUTORIZZAZIONI(Session("conn"), Session("IdEnte"), Session("txtCodEnte"), strATTIVITA, strBANDOATTIVITA, strENTEPERSONALE, strENTITA, Request.QueryString("Id2")) = 1 Then

                    If Not dtrgenerico Is Nothing Then
                        dtrgenerico.Close()
                        dtrgenerico = Nothing
                    End If
                Else
                    If Not dtrgenerico Is Nothing Then
                        dtrgenerico.Close()
                        dtrgenerico = Nothing
                    End If
                    Response.Redirect("wfrmAnomaliaDati.aspx")

                End If

            End If


            If Page.IsPostBack = False Then
                If Request.QueryString("Blocco") = "FALSE" Then
                    'If controllaambito() = True Then
                    SelComune.CaricaProvinciaNazione(ddlProvincia, blnEstero, Session("Conn"))
                    CaricaComuniSelezionati()

                    ''''Else
                    ''''    'non deve mettere i comuni
                    ''''    NascondiOggetti()
                    ''''    Imgerrore.Visible = True
                    ''''    lblmessaggiosopra.Visible = True
                    ''''    lblmessaggiosopra.Text = "Attenzione! La tipologia indicata non puo' inserire gli ambiti territoriali"
                    ''''    Exit Sub
                    'End If

                Else
                    'If controllaambito() = True Then
                    SelComune.CaricaProvinciaNazione(ddlProvincia, blnEstero, Session("Conn"))
                    CaricaComuniSelezionati()
                    'imgConferma.Visible = False
                    CmdRicerca.Visible = False
                    'CmdRimuoviTutti.Visible = False
                    'CmdSelezionaTutti.Visible = False
                    'ddlProvincia.Visible = False
                    'txtComune.Visible = False
                    'lblComune.Visible = False
                    'lblProvincia.Visible = False
                    'Imgerrore.Visible = True
                    lblmessaggiosopra.Visible = True
                    lblmessaggiosopra.Text = "Attenzione! Maschera in sola visualizzazione." ''' Le modifiche apportate non verranno salvate"
                    ''''Else
                    ''''    NascondiOggetti()
                    ''''    Imgerrore.Visible = True
                    ''''    lblmessaggiosopra.Visible = True
                    ''''    lblmessaggiosopra.Text = "Attenzione! Maschera in sola visualizazione. La tipologia indicata non puo' inserire/modificare gli ambiti territoriali"
                    ''''    Exit Sub

                    'End If
                End If
            Else

            End If
        End If

    End Sub

    Private Sub ddlProvincia_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlProvincia.SelectedIndexChanged
        Dim SelComune As New clsSelezionaComune
        SelComune.CaricaComuni(ddlComune, ddlProvincia.SelectedValue, Session("Conn"))
    End Sub

    Private Sub ChkEstero_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ChkEstero.CheckedChanged
        Dim SelComune As New clsSelezionaComune
        Dim blnEstero As Boolean = ChkEstero.Checked
        SelComune.CaricaProvinciaNazione(ddlProvincia, blnEstero, Session("Conn"))
        ddlComune.Items.Clear()
    End Sub

    Sub CaricaComuniSelezionati()
        Dim dtsComuniSelezionati As DataSet
        dtgComuniSelezionati.CurrentPageIndex = 0
        'variabile stringa locale per costruire la query per le aree
        Dim strSql As String
        'preparo la query per i settori
        If Request.QueryString("Id") <> "" Then
            strSql = "SELECT  comuni.Denominazione as Comune, provincie.Provincia ,Regioni.Regione, (select count(identesedeattuazione) from entisediattuazioni a inner join entisedi b on a.identesede = b.identesede where(b.idcomune = entiambitoterritoriale.idcomune) and (a.identecapofila = " & Request.QueryString("Id") & " or b.idente = " & Request.QueryString("Id") & ") and a.idstatoentesede in (1,4)) as NumeroSedi FROM  EntiAmbitoTerritoriale INNER JOIN comuni ON EntiAmbitoTerritoriale.IdComune = comuni.IDComune INNER JOIN provincie ON comuni.IDProvincia = provincie.IDProvincia INNER JOIN Regioni ON Provincie.idRegione = Regioni.IdRegione WHERE EntiAmbitoTerritoriale.IdEnte  = " & Request.QueryString("Id") & "  AND  ComuneNazionale = 1  AND  (ISNULL(comuni.CodiceISTAT, '') <> '')"
        Else
            strSql = "SELECT  comuni.Denominazione as Comune, provincie.Provincia ,Regioni.Regione, (select count(identesedeattuazione) from entisediattuazioni a inner join entisedi b on a.identesede = b.identesede where(b.idcomune = entiambitoterritoriale.idcomune) and (a.identecapofila = " & Request.QueryString("Id2") & " or b.idente = " & Request.QueryString("Id2") & ") and a.idstatoentesede in (1,4)) as NumeroSedi  FROM  EntiAmbitoTerritoriale INNER JOIN comuni ON EntiAmbitoTerritoriale.IdComune = comuni.IDComune INNER JOIN provincie ON comuni.IDProvincia = provincie.IDProvincia INNER JOIN Regioni ON Provincie.idRegione = Regioni.IdRegione WHERE EntiAmbitoTerritoriale.IdEnte  = " & Request.QueryString("Id2") & " AND  ComuneNazionale = 1  AND  (ISNULL(comuni.CodiceISTAT, '') <> '')"
        End If
        If ddlProvincia.SelectedValue <> "0" Then
            strSql = strSql & " and comuni.IDProvincia ='" & ddlProvincia.SelectedValue & "'"
        End If
        If ddlComune.SelectedIndex > 0 Then
            strSql = strSql & " and comuni.IDComune = " & ddlComune.SelectedValue
        End If
        'Comune comuni.Denominazione
        'eseguo la query e passo il risultato al datareader
        dtsComuniSelezionati = ClsServer.DataSetGenerico(strSql, Session("conn"))
        'controllo se ci sono dei record
        If dtsComuniSelezionati.Tables(0).Rows.Count > 0 Then
            'al datasource sella combo passo il datareader
            dtgComuniSelezionati.DataSource = dtsComuniSelezionati
            If Request.QueryString("blocco") = "TRUE" Then
                dtgComuniSelezionati.Columns(0).Visible = False
            End If
            dtsGenerico = ClsServer.DataSetGenerico(strSql, Session("conn"))
            Session("RisultatoGriglia") = dtsComuniSelezionati
        End If
        dtgComuniSelezionati.DataBind()

        'carico i dati per la stampa
        Dim NomeColonne(dtsComuniSelezionati.Tables(0).Columns.Count) As String
        Dim NomiCampiColonne(dtsComuniSelezionati.Tables(0).Columns.Count) As String
        Dim intX As Integer
        For intX = 0 To dtsComuniSelezionati.Tables(0).Columns.Count - 1
            NomeColonne(intX) = dtsComuniSelezionati.Tables(0).Columns(intX).ColumnName
            NomiCampiColonne(intX) = dtsComuniSelezionati.Tables(0).Columns(intX).ColumnName
        Next

        If dtgComuniSelezionati.Items.Count = 0 Then
            CmdEsporta.Visible = False
            lblmessaggio.Visible = True
            lblmessaggio.Text = "Nessun Dato estratto."
        Else
            CmdEsporta.Visible = True
            lblmessaggio.Visible = True
            lblmessaggio.Text = "Risultato Elenco Comuni."
        End If

        CaricaDataTablePerStampa(dtsComuniSelezionati, dtsComuniSelezionati.Tables(0).Columns.Count - 1, NomeColonne, NomiCampiColonne)

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

        'carico il datatable con il risultato della stored
        If DataSetDaScorrere.Tables(0).Rows.Count > 0 Then
            For i = 1 To DataSetDaScorrere.Tables(0).Rows.Count
                dr = dt.NewRow()
                For x = 0 To NColonne
                    dr(x) = DataSetDaScorrere.Tables(0).Rows.Item(i - 1).Item(NomiCampiColonne(x))
                Next
                dt.Rows.Add(dr)
            Next
        End If

        'passo alla sessione la datatable che ho appena creato e che userò per il databinding della datagrid della stampa
        Session("DtbRicerca") = dt
    End Sub

    Private Sub cmdChiudi_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdChiudi.Click
        Response.Write("<script>" & vbCrLf)
        Response.Write("window.close()" & vbCrLf)
        Response.Write("</script>")
    End Sub

    Private Sub CmdRicerca_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdRicerca.Click
        CaricaComuniSelezionati()
    End Sub

    Private Sub dtgComuniSelezionati_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dtgComuniSelezionati.PageIndexChanged
        dtgComuniSelezionati.CurrentPageIndex = e.NewPageIndex
        dtgComuniSelezionati.DataSource = Session("RisultatoGriglia")
        dtgComuniSelezionati.DataBind()
        dtgComuniSelezionati.SelectedIndex = -1
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

End Class