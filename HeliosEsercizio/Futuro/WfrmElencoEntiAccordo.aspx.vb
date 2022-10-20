Imports System.Drawing
Imports System.IO

Public Class WfrmElencoEntiAccordo
    Inherits System.Web.UI.Page
    Dim strsql As String
    Dim dtsgenerico As DataSet
    Dim dtrgenerico As SqlClient.SqlDataReader

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Inserire qui il codice utente necessario per inizializzare la pagina
        If Not Session("LogIn") Is Nothing Then
            If Session("LogIn") = False Then
                Response.Redirect("LogOn.aspx")
            End If
        Else
            Response.Redirect("LogOn.aspx")
        End If
        If Page.IsPostBack = False Then
            If Not Request.Cookies("InfoRif") Is Nothing Then
                If Session("txtCodEnte") = Request.Cookies("InfoRif")("txtCodiceEnte") Then
                    RecuperaCookie()
                End If
            End If
            dgRisultatoRicerca.CurrentPageIndex = 0
            If Request.QueryString("CheckProvenienza") = "VisualizzazioneEntiInAccordo" Then
                EseguiQuery(1, Request.QueryString("Pagina"))
            Else
                EseguiQuery(0)
            End If
            TrovaEntePadre()
        End If
    End Sub
    Sub AssegnaCookies()
        'CREATA IL 28/12/2016 ADC
        Response.Cookies("InfoRif")("txtCodiceEnte") = Session("txtCodEnte")
        Response.Cookies("InfoRif")("txtIDFaseEnte") = txtIdEnteFase.Text
        Response.Cookies("InfoRif").Expires = DateTime.Now.AddDays(1)
    End Sub
    Sub RecuperaCookie()
        'CREATA IL 28/12/2016 ADC
       
        If Not Server.HtmlDecode(Request.Cookies("InfoRif")("txtIDFaseEnte")) Is Nothing Then
            txtIdEnteFase.Text = Server.HtmlDecode(Request.Cookies("InfoRif")("txtIDFaseEnte")).ToString
        End If

    End Sub
    Sub EseguiQuery(ByVal bytVerifica As Byte, Optional ByVal bytpage As Integer = 0)

        'se successivo o precedente setto pagina di arrivo a seconda del parametro
        If bytVerifica = 1 Then dgRisultatoRicerca.CurrentPageIndex = bytpage

        '" (SELECT COUNT(*) FROM EntiSediAttuazioni " & _
        strsql = "Select top 1  0 as ordina,'si' as entePadre,entirelazioni.identepadre, Codiceregione,enti.denominazione,tipologia, " & _
                " ca.classeaccreditamento as classeAttribuita, " & _
                " cr.classeaccreditamento as classeRichiesta, entirelazioni.datafinevalidità , " & _
                "(SELECT convert(varchar, COUNT(*)) FROM EntiSediAttuazioni " & _
                " INNER JOIN StatiEntiSedi ON EntiSediAttuazioni.IdStatoEnteSede = StatiEntiSedi.IdStatoEnteSede " & _
                " INNER JOIN EntiSedi ON EntiSedi.IdEnteSede = EntiSediAttuazioni.IdEnteSede " & _
                " WHERE (StatiEntiSedi.Attiva = 1 or StatiEntiSedi.DaAccreditare = 1 )AND IdEnte = entirelazioni.identepadre) as nsedi, " & _
                " case when entirelazioni.datafinevalidità is null then 'Attivo' " & _
                " when entirelazioni.datafinevalidità is not null then 'Annullato' end as StatoRelazione, " & _
                " statienti.statoente as statoaccreditamento,enti.idente, isnull(enti.codicefiscale,'') as CodiceFiscale,'' as IdEnteFase " & _
                " from entirelazioni " & _
                " inner join enti on enti.idente=entirelazioni.identepadre " & _
                " inner join statienti on (statienti.idstatoente=enti.idstatoente)" & _
                " inner join classiaccreditamento ca on ca.idclasseaccreditamento=enti.idclasseaccreditamento " & _
                " inner join classiaccreditamento cr on cr.idclasseaccreditamento=enti.idclasseaccreditamentorichiesta " & _
                " where entirelazioni.identepadre=" & Request.QueryString("idente") & " and entirelazioni.datafinevalidità is null  union"
        strsql = strsql & " Select distinct  1 as ordina,'no' as entePadre,entirelazioni.identepadre as identepadre, Codiceregione,enti.denominazione,tipologia, " &
        " S.Sezione classeAttribuita, " &
        " S.Sezione  classeRichiesta,  entirelazioni.datafinevalidità ," &
        "(SELECT convert(varchar, COUNT(*))  FROM EntiSediAttuazioni " &
        " INNER JOIN StatiEntiSedi ON EntiSediAttuazioni.IdStatoEnteSede = StatiEntiSedi.IdStatoEnteSede " &
        " INNER JOIN EntiSedi ON EntiSedi.IdEnteSede = EntiSediAttuazioni.IdEnteSede " &
        " WHERE (StatiEntiSedi.Attiva = 1 or StatiEntiSedi.DaAccreditare = 1 )AND IdEnte = entirelazioni.identefiglio) as nsedi, " &
        " case when entirelazioni.datafinevalidità is null then 'Attivo' " &
        " when entirelazioni.datafinevalidità is not null then 'Annullato' end as StatoRelazione, " &
        " statienti.statoente as statoaccreditamento,enti.idente, isnull(enti.codicefiscale,'') as CodiceFiscale,CONVERT(varchar,efe.IdEnteFase) as IdEnteFase " &
        " from entirelazioni " &
        " inner join enti on enti.idente=entirelazioni.identefiglio " &
        " inner join statienti on (statienti.idstatoente=enti.idstatoente)" &
        " inner join classiaccreditamento ca on ca.idclasseaccreditamento=enti.idclasseaccreditamento " &
        " inner join classiaccreditamento cr on cr.idclasseaccreditamento=enti.idclasseaccreditamentorichiesta " &
        " inner join EntiFasi_Enti efe on efe.IdEnte = entirelazioni.identefiglio " &
        " inner join EntiFasi  ef  on ef.IdEnteFase = efe.IdEnteFase " &
        " left join SezioniAlboSCU S on S.idSezione = enti.idSezione " &
        " where entirelazioni.identepadre=" & Request.QueryString("idente") & " And entirelazioni.datafinevalidità Is null " &
         " and (statienti.Presentazioneprogetti =0 and statienti.defaultstato=0 " &
            " and statienti.chiuso=0 and statienti.sospeso=0 and statienti.istruttoria=0)  and  ef.Stato =3 "
        '-----------------------------------INIZIO------------------------------------------------------
        'FILTRI DA COPIARE SU www1
        If Trim(txtDenominazione.Text) <> "" Then
            'dgRisultatoRicerca.CurrentPageIndex = 0
            strsql = strsql & "and enti.Denominazione like '" & Replace(txtDenominazione.Text, "'", "''") & "%'"
        End If

        If Trim(txtcodicefiscale.Text) <> "" Then
            'dgRisultatoRicerca.CurrentPageIndex = 0
            strsql = strsql & "and enti.CodiceFiscale like '" & Replace(txtcodicefiscale.Text, "'", "''") & "%'"
        End If
        If txtIdEnteFase.Text <> "" Then
            'dgRisultatoRicerca.CurrentPageIndex = 0
            strsql = strsql & "and ef.IdEnteFase = '" & txtIdEnteFase.Text & "'"
        End If
        '--------------------------------------FINE---------------------------------------------------
        strsql = strsql & " order by ordina,entirelazioni.datafinevalidità,statienti.statoente,enti.denominazione  "
        dtsgenerico = ClsServer.DataSetGenerico(strsql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))

        'txtcodicefiscale.Text = ""
        'txtDernominazione.Text = ""
        If bytpage = 0 Then
            dgRisultatoRicerca.CurrentPageIndex = 0
        End If
        ApriCSV1.Visible = False
        CaricaDataGrid(dgRisultatoRicerca)
        CaricaDataTablePerStampa(dtsgenerico)
    End Sub

    Private Sub TrovaEntePadre()
        'Generato da Alessandra Taballione il 31/03/05
        'VAriazione del Colore per l'ente padre
        Dim item As DataGridItem
        Dim color As New System.Drawing.Color
        Dim x As Integer
        For Each item In dgRisultatoRicerca.Items
            For x = 0 To 12
                If dgRisultatoRicerca.Items(item.ItemIndex).Cells(10).Text = "si" Then
                    color = Drawing.Color.LightYellow
                    dgRisultatoRicerca.Items(item.ItemIndex).Cells(x).BackColor = color
                Else
                    If dgRisultatoRicerca.Items(item.ItemIndex).Cells(8).Text = "Classe Attribuita" Then
                        color = Drawing.Color.LightGreen
                        dgRisultatoRicerca.Items(item.ItemIndex).Cells(x).BackColor = color
                    Else
                        If dgRisultatoRicerca.Items(item.ItemIndex).Cells(7).Text <> "Attivo" Then
                            color = Drawing.Color.LightSalmon()
                            dgRisultatoRicerca.Items(item.ItemIndex).Cells(x).BackColor = color
                        End If
                        If dgRisultatoRicerca.Items(item.ItemIndex).Cells(8).Text <> "Registrato" Then
                            color = Drawing.Color.LightSalmon
                            dgRisultatoRicerca.Items(item.ItemIndex).Cells(x).BackColor = color
                        End If
                    End If
                End If
            Next
        Next
    End Sub

    Sub CaricaDataGrid(ByRef GridDaCaricare As DataGrid) 'valorizzo la datagrid passata
        Dim appo As String

        GridDaCaricare.DataSource = dtsgenerico
        GridDaCaricare.DataBind()
    End Sub

    Private Sub dgRisultatoRicerca_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgRisultatoRicerca.PageIndexChanged
        'DA COPIARE SU www1
        txtcodicefiscale.Text = ""
        txtDenominazione.Text = ""
        '---------FINE---------------------



        'dgRisultatoRicerca.SelectedIndex = -1
        'dgRisultatoRicerca.EditItemIndex = -1
        'dgRisultatoRicerca.CurrentPageIndex = e.NewPageIndex
        'CaricaDataGrid(dgRisultatoRicerca)
        EseguiQuery(1, e.NewPageIndex)
        TrovaEntePadre()
    End Sub

    'routine che carica la datatable che caricherà dinamicamente la datagrid della stampa delle ricerche
    Sub CaricaDataTablePerStampa(ByVal DataSetDaScorrere As DataSet)
        Dim dt As New DataTable
        Dim dr As DataRow
        Dim i As Integer
        Dim x As Integer

        Dim NomeColonne(9) As String
        Dim NomiCampiColonne(9) As String

        'nome della colonna 
        'e posizione nella griglia di lettura
        NomeColonne(0) = "Cod.Ente"
        NomeColonne(1) = "Denominazione"
        NomeColonne(2) = "Codice Fiscale"
        NomeColonne(3) = "Sezione Attribuita"
        NomeColonne(4) = "Sezione Richiesta"
        NomeColonne(5) = "Nr.Sedi"
        NomeColonne(6) = "Stato Relazione"
        NomeColonne(7) = "Stato Accr."
        NomeColonne(8) = "Rif.Fase"

        NomiCampiColonne(0) = "Codiceregione"
        NomiCampiColonne(1) = "Denominazione"
        NomiCampiColonne(2) = "CodiceFiscale"
        NomiCampiColonne(3) = "classeAttribuita"
        NomiCampiColonne(4) = "classeRichiesta"
        NomiCampiColonne(5) = "nsedi"
        NomiCampiColonne(6) = "statorelazione"
        NomiCampiColonne(7) = "statoaccreditamento"
        NomiCampiColonne(8) = "IdEnteFase"


        'carico i nomi delle colonne che andrò a stampare nella datagrid
        For x = 0 To 8
            dt.Columns.Add(New DataColumn(NomeColonne(x), GetType(String)))
        Next

        'carico il datatable con il risultato della query della ricerca, in qusto caso delle risorse
        If DataSetDaScorrere.Tables(0).Rows.Count > 0 Then
            For i = 1 To DataSetDaScorrere.Tables(0).Rows.Count
                dr = dt.NewRow()
                For x = 0 To 8
                    dr(x) = DataSetDaScorrere.Tables(0).Rows.Item(i - 1).Item(NomiCampiColonne(x))
                Next
                dt.Rows.Add(dr)
            Next
        End If

        'passo alla sessione la datatable che ho appena creato e che userò per il databinding della datagrid della stampa
        Session("DtbRicerca") = dt

    End Sub

    Private Sub dgRisultatoRicerca_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgRisultatoRicerca.ItemCommand
        If e.CommandName = "SelezionaEnte" Then
            'Verifica dell'accreditamento effettuato per tutti gli enti figli
            If e.Item.Cells(10).Text = "si" Then
                'strsql = "select identefiglio, statienti.Presentazioneprogetti,statienti.defaultstato," & _
                '" statienti.chiuso, statienti.sospeso, statienti.istruttoria, statienti.idstatoente, statienti.statoente " & _
                '" from entirelazioni " & _
                '" inner join enti on enti.idente=entirelazioni.identeFiglio " & _
                '" inner join statienti on statienti.idstatoente=enti.idstatoente " & _
                '" where(identepadre = " & Session("IdEnte") & " And datafinevalidità Is null) " & _
                '" and (statienti.Presentazioneprogetti =0 and statienti.defaultstato=0 " & _
                '" and statienti.chiuso=0 and statienti.sospeso=0 and statienti.istruttoria=0)"
                'If Not dtrgenerico Is Nothing Then
                '    dtrgenerico.Close()
                '    dtrgenerico = Nothing
                'End If
                'dtrgenerico = ClsServer.CreaDatareader(strsql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
                'If dtrgenerico.HasRows = True Then
                '    ImgMess.Visible = True
                '    lblMessaggi.Visible = True
                '    lblMessaggi.ForeColor = Color.Red
                '    lblMessaggi.Text = "Non è possibile Accreditare l'Ente se non risultano Accreditati tutti gli Enti Figli."
                '    If Not dtrgenerico Is Nothing Then
                '        dtrgenerico.Close()
                '        dtrgenerico = Nothing
                '    End If
                'Else
                '    If Not dtrgenerico Is Nothing Then
                '        dtrgenerico.Close()
                '        dtrgenerico = Nothing
                '    End If
                Session("denominazione") = e.Item.Cells(2).Text
                Session("idente") = CInt(e.Item.Cells(9).Text)
                Response.Redirect("WfrmAnagraficaEnte.aspx?VediEnte=1" & e.Item.Cells(11).Text) '"Wfrmalbero.aspx?tipologia=Enti&Arrivo=WfrmElencoEntiAccordo.aspx&identePadre=" + e.Item.Cells(11).Text)
                'End If
            Else
                If Not dtrgenerico Is Nothing Then
                    dtrgenerico.Close()
                    dtrgenerico = Nothing
                End If
                If e.Item.Cells(7).Text = "Annullato" Then 'Or dgRisultatoRicerca.SelectedItem.Cells(8).Text = "Sospeso" Then

                    lblMessaggi.Visible = True
                    lblMessaggi.ForeColor = Color.Red
                    lblMessaggi.Text = "Non è possibile iscrivere l'Ente perchè risulta Annullato."
                End If
                If e.Item.Cells(8).Text = "Sospeso" Then
                    If e.Item.Cells(10).Text = "si" Then

                        lblMessaggi.Visible = True
                        lblMessaggi.ForeColor = Color.Red
                        lblMessaggi.Text = "Non è possibile iscrivere l'Ente perchè risulta Sospeso."
                    Else
                        Session("denominazione") = e.Item.Cells(2).Text
                        Session("idente") = CInt(e.Item.Cells(9).Text)
                        Response.Redirect("Wfrmalbero.aspx?tipologia=Enti&Arrivo=WfrmElencoEntiAccordo.aspx&identePadre=" + e.Item.Cells(11).Text)
                    End If
                End If
                If e.Item.Cells(7).Text = "Attivo" And e.Item.Cells(8).Text = "Registrato" Then
                    Session("denominazione") = e.Item.Cells(2).Text
                    Session("idente") = CInt(e.Item.Cells(9).Text)
                    Response.Redirect("Wfrmalbero.aspx?tipologia=Enti&Arrivo=WfrmElencoEntiAccordo.aspx&identePadre=" + e.Item.Cells(11).Text)
                End If
                If e.Item.Cells(8).Text = "Classe Attribuita" Then
                    Session("denominazione") = e.Item.Cells(2).Text
                    Session("idente") = CInt(e.Item.Cells(9).Text)
                    Response.Redirect("Wfrmalbero.aspx?tipologia=Enti&Arrivo=WfrmElencoEntiAccordo.aspx&identePadre=" + e.Item.Cells(11).Text)
                End If
            End If
        End If

        If e.CommandName = "ElencoEnti" Then
            Response.Redirect("WfrmRicercaSede.aspx?Pagina=" + dgRisultatoRicerca.CurrentPageIndex.ToString + "&CheckProvenienza=VisualizzazioneEntiInAccordo&idente=" + Request.QueryString("idente") + "&DenominazioneEnte=" + e.Item.Cells(2).Text)
        End If

    End Sub

    Private Sub cmdChiudi_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdChiudi.Click
        Response.Redirect("WfrmEntidaAccreditare.aspx?VediEnte=0")
    End Sub

    Private Sub cmdRicerca_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdRicerca.Click
        'Da Capiare su www1 anche la SUB---------INIZIO------------------------
        If Request.QueryString("CheckProvenienza") = "VisualizzazioneEntiInAccordo" Then
            EseguiQuery(1, Request.QueryString("Pagina"))
        Else
            EseguiQuery(0)
        End If
        AssegnaCookies()
        TrovaEntePadre()
        '----------------FINE---------------------------------
    End Sub

    Private Sub CmdEsporta_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdEsporta.Click
        'CmdEsporta.Visible = False
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