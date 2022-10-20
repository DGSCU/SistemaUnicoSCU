Imports System.Collections.Generic
Imports System.Data.SqlClient
Imports System.Drawing
Imports System.Security.Cryptography
'Imports System.Text.RegularExpressions.Regex
Imports System.IO
Imports System.Text.RegularExpressions
Imports Logger.Data
Public Class WfrmListaEntiFuoriAdeguamento
    Inherits SmartPage
    Dim dtrGenerico As SqlClient.SqlDataReader
    Dim dtsGenerico As DataSet
    Dim mydataset As DataSet
    Dim strsql As String
    Dim blnRicerca As Boolean
    Public blnForza As Boolean
    Public ShowPopUPControllo As String
    Const INDEX_DGRISULTATORICERCA_IdImgSelEnti As Byte = 0
    Const INDEX_DGRISULTATORICERCA_DATAVARIAZIONE As Byte = 1
    Const INDEX_DGRISULTATORICERCA_CODICEENTE As Byte = 2
    Const INDEX_DGRISULTATORICERCA_ENTE As Byte = 3
    Const INDEX_DGRISULTATORICERCA_VARIAZIONI As Byte = 4
    Const INDEX_DGRISULTATORICERCA_VISTO As Byte = 5
    Const INDEX_DGRISULTATORICERCA_FLGVALIDAZIONE As Byte = 6
    Const INDEX_DGRISULTATORICERCA_IDENTE As Byte = 7
    Const INDEX_DGRISULTATORICERCA_IDVARIAZIONE As Byte = 8
    Const INDEX_DGRISULTATORICERCA_IDENTERELAZIONE As Byte = 9



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
        Dim dtrGenericoLocal As SqlClient.SqlDataReader
        ShowPopUPControllo = ""
        Session("Seleziona") = 0
        Session("ValidaModifiche") = 0

        If IsPostBack = False Then
            Session("BloccaChK") = 0

            Try

                ChiudiDataReader(dtrGenericoLocal)
                ChiudiDataReader(dtrGenerico)
                lblpage.Value = IIf(Not IsNothing(Context.Items("page")), Context.Items("page"), "")
                txtstrsql.Value = IIf(Not IsNothing(Context.Items("strsql")), Context.Items("strsql"), "")

                ddlTipologia.Items.Add("")
                ddlTipologia.Items.Add("Pubblico")
                ddlTipologia.Items.Add("Privato")
                'popolo combo classiaccreditamento
                Dim strsql As String = "SELECT '0' AS idSezione,'Seleziona' AS classeAccreditamento "
                strsql &= "FROM SezioniAlboSCU "
                strsql &= "UNION "
                strsql &= "SELECT S.idSezione , S.Sezione AS classeAccreditamento "
                strsql &= "FROM SezioniAlboSCU S JOIN classiaccreditamento C "
                strsql &= "ON S.IdClasseAccreditamento = C.IDClasseAccreditamento "
                strsql &= "WHERE(C.MINSEDI > 0) "

                mydataset = ClsServer.DataSetGenerico(strsql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
                ddlClasseAttribuita.DataSource = mydataset
                ddlClasseAttribuita.DataValueField = "idSezione"
                ddlClasseAttribuita.DataTextField = "classeAccreditamento"
                ddlClasseAttribuita.DataBind()

                mydataset.Dispose()
                mydataset = Nothing

                If Session("CodiceRegioneEnte") <> "" Then
                    txtCodiceEnte.Text = Session("txtCodEnte")
                End If


            Catch ex As Exception
                ex.Message.ToString()
            End Try
        End If

        If dgRisultatoRicerca.Items.Count = 0 Then
            CmdEsporta.Visible = False
        Else
            CmdEsporta.Visible = True
        End If
    End Sub

#Region "Eventi"
    Private Sub cmdChiudi_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles cmdChiudi.Click

        If Request.QueryString("VengoDaProgetti") = "AccettazioneProgetti" Then
            Response.Redirect("assegnazionevincoliprogetti.aspx?idattivita=" & Request.QueryString("idattivita") & "&tipologia=" & Request.QueryString("tipologia") & "&Nazionale=" & Request.QueryString("Nazionale"))
        Else
            If Not Request.QueryString("VengoDa") Is Nothing Then
                'controllo se la variabile è valorizzata
                If Request.QueryString("VengoDa") <> String.Empty Then
                    'faccio la response.redirect verso l'albero
                    Response.Redirect(Request.QueryString("VengoDa").ToString)
                End If
            End If
            If Not Request.QueryString("DenominazioneEnte") Is Nothing Then
                If Request.QueryString("CheckProvenienza") = "VisualizzazioneEntiInAccordo" Then
                    Response.Redirect("WfrmElencoEntiAccordo.aspx?Pagina=" & Request.QueryString("Pagina") & "&CheckProvenienza=" & Request.QueryString("CheckProvenienza") & "&Denominazione=" & Request.QueryString("Denominazione") & "&idente=" & Request.QueryString("idente") & "&DenominazioneEnte=" & Request.QueryString("DenomonazioneEnte"))
                Else
                    Response.Redirect("WfrmRicEnteinAccordo.aspx?Pagina=" & Request.QueryString("Pagina") & "&CheckProvenienza=" & Request.QueryString("CheckProvenienza") & "&Stato=" & Request.QueryString("Stato") & "&CF=" & Request.QueryString("CF") & "&ClasseAccreditamento=" & Request.QueryString("ClasseAccreditamento") & "&Tipologia=" & Request.QueryString("Tipologia") & "&CodiceRegione=" & Request.QueryString("CodicerRegione") & "&Denominazione=" & Request.QueryString("Denominazione") & "&VediEnte=1&DenominazioneEnte=" & Request.QueryString("DenomonazioneEnte"))
                End If
            Else
                Response.Redirect("WfrmMain.aspx")
            End If
        End If
    End Sub
    Public Sub cmdRicerca_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles cmdRicerca.Click
        lblpage.Value = String.Empty
        lblErrore.Text = String.Empty
        chkSelDesel.Checked = False
        RicercaEnti()

    End Sub

    Private Sub dgRisultatoRicerca_ItemCommand(source As Object, e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgRisultatoRicerca.ItemCommand
        Select Case e.CommandName
            Case "Select"
                Dim idente As String = e.Item.Cells(INDEX_DGRISULTATORICERCA_IDENTE).Text
                Dim identerelazione As String = e.Item.Cells(INDEX_DGRISULTATORICERCA_IDENTERELAZIONE).Text
                'attivo solo se e' attualmente selezionato un ente titolare
                If Session("IdEnte") & "" <> "-1" Then
                    If Session("IdEnte") & "" = idente Then
                        Response.Redirect("WfrmAnagraficaEnte.aspx?VediEnte=1")
                    ElseIf identerelazione <> "&nbsp;" Then
                        Response.Redirect("WfrmAnagraficaEnteAccordo.aspx?azione=Mod&id=" & idente & "&identerelazione=" & identerelazione & "&Stato=Attivo")
                    End If

                End If

        End Select
    End Sub


    Private Sub dgRisultatoRicerca_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgRisultatoRicerca.PageIndexChanged
        Dim i As Integer
        Dim Mychk As CheckBox
        Dim dtGriglia As DataTable 'appoggio
        Dim rwGriglia As DataRow
        Dim clGriglia As DataColumn
        Dim ValoreRichkDt As Integer = 3 ' valore che ricorda il campo flgValidazione della query 
        dtGriglia = Session("SessiondtEnti")
        'If Request.QueryString("PopSede") = 1 Then
        '    ValoreRichkDt =  ' id ricerca sede di progetto (riccheck)
        'Else
        '    ValoreRichkDt = 12 ' id ricerca sede campo (riccheck)
        'End If
        clGriglia = dtGriglia.Columns(ValoreRichkDt)
        clGriglia.ReadOnly = False

        'If chkSelDesel.Checked = True Then
        '    CeccaTutti()
        'End If
        For i = 0 To dgRisultatoRicerca.Items.Count - 1
            Mychk = dgRisultatoRicerca.Items.Item(i).FindControl("chkEnti")
            rwGriglia = dtGriglia.Rows(i + (dgRisultatoRicerca.CurrentPageIndex * 10))
            'If Mychk.Checked = True Then
            '    rwGriglia.Item(ValoreRichkDt) = "1"
            'Else
            '    rwGriglia.Item(ValoreRichkDt) = "0"
            'End If
            rwGriglia.Item(ValoreRichkDt) = Mychk.Checked
        Next i
        Session("SessiondtEnti") = dtGriglia
        dgRisultatoRicerca.CurrentPageIndex = e.NewPageIndex
        dgRisultatoRicerca.DataSource = Session("SessiondtEnti")
        dgRisultatoRicerca.DataBind()


        '---determino cosa c'era text nella datatable di sessione in e lo visualizzo nella pag che vado a caricare
        For i = 0 To dgRisultatoRicerca.Items.Count - 1
            Mychk = dgRisultatoRicerca.Items.Item(i).FindControl("chkEnti")
            rwGriglia = dtGriglia.Rows(i + (dgRisultatoRicerca.CurrentPageIndex * 10))
            If rwGriglia.Item(ValoreRichkDt) = "True" Then
                Mychk.Checked = True
            Else
                Mychk.Checked = False
            End If
        Next i
        If chkSelDesel.Checked = True Then
            CeccaTutti()
            For i = 0 To dgRisultatoRicerca.Items.Count - 1
                Mychk = dgRisultatoRicerca.Items.Item(i).FindControl("chkEnti")
                rwGriglia = dtGriglia.Rows(i + (dgRisultatoRicerca.CurrentPageIndex * 10))
                'If Mychk.Checked = True Then
                '    rwGriglia.Item(ValoreRichkDt) = "1"
                'Else
                '    rwGriglia.Item(ValoreRichkDt) = "0"
                'End If
                rwGriglia.Item(ValoreRichkDt) = Mychk.Checked
            Next i
            Session("SessiondtEnti") = dtGriglia
            dgRisultatoRicerca.CurrentPageIndex = e.NewPageIndex
            dgRisultatoRicerca.DataSource = Session("SessiondtEnti")
            dgRisultatoRicerca.DataBind()

            '---determino cosa c'era text nella datatable di sessione in e lo visualizzo nella pag che vado a caricare
            For i = 0 To dgRisultatoRicerca.Items.Count - 1
                Mychk = dgRisultatoRicerca.Items.Item(i).FindControl("chkEnti")
                rwGriglia = dtGriglia.Rows(i + (dgRisultatoRicerca.CurrentPageIndex * 10))
                If rwGriglia.Item(ValoreRichkDt) = "True" Then
                    Mychk.Checked = True
                Else
                    Mychk.Checked = False
                End If
            Next i
        Else
            If hdd_Check.Value = "SI" Then
                EliminaTuttiCheck()
                For i = 0 To dgRisultatoRicerca.Items.Count - 1
                    Mychk = dgRisultatoRicerca.Items.Item(i).FindControl("chkEnti")
                    rwGriglia = dtGriglia.Rows(i + (dgRisultatoRicerca.CurrentPageIndex * 10))
                    'If Mychk.Checked = True Then
                    '    rwGriglia.Item(ValoreRichkDt) = "1"
                    'Else
                    '    rwGriglia.Item(ValoreRichkDt) = "0"
                    'End If
                    rwGriglia.Item(ValoreRichkDt) = Mychk.Checked
                Next i
                Session("SessiondtEnti") = dtGriglia
                dgRisultatoRicerca.CurrentPageIndex = e.NewPageIndex
                dgRisultatoRicerca.DataSource = Session("SessiondtEnti")
                dgRisultatoRicerca.DataBind()

                '---determino cosa c'era text nella datatable di sessione in e lo visualizzo nella pag che vado a caricare
                For i = 0 To dgRisultatoRicerca.Items.Count - 1
                    Mychk = dgRisultatoRicerca.Items.Item(i).FindControl("chkEnti")
                    rwGriglia = dtGriglia.Rows(i + (dgRisultatoRicerca.CurrentPageIndex * 10))
                    If rwGriglia.Item(ValoreRichkDt) = "True" Then
                        Mychk.Checked = True
                    Else
                        Mychk.Checked = False
                    End If
                Next i
            End If
        End If
        TogliCheck()
        FormattaVariazioni()
        ColoraCelle()
    End Sub
#End Region
#Region "Funzionalita"
    Private Function calcDiff(ByVal nomeCampoVecchio As String, ByVal nomeCampoNuovo As String, ByVal etichetta As String) As String
        Return String.Format(" Case Isnull(Coalesce(v.{0},v.{1}),'') when '' then '' else '{2}: <b>' + rtrim(ltrim(v.{1})) + '</b> (' + rtrim(ltrim(coalesce(v.{0}, '<i>nessun valore</i>'))) + ') | ' End ", nomeCampoVecchio, nomeCampoNuovo, etichetta)
    End Function
    Private Function calcDiffComune(ByVal nomeCampoVecchio As String, ByVal nomeCamponuovo As String, ByVal etichetta As String) As String
        Return String.Format(" case when {0} is null then '' else '{2}: <b>' + (select denominazione from comuni where idComune=v.{0}) + " &
            "'</b> ('+ coalesce((select denominazione from comuni where idComune=v.{1}), '<i>nessun valore</i>') + ') | ' End ", nomeCampoVecchio, nomeCamponuovo, etichetta)
    End Function
    Private Sub RicercaEnti()
        Dim dtEnti As DataTable = New DataTable
        Dim IdEnteRicerca As String = String.Empty

        strsql = "SELECT v.IDEnteFuoriAdeguamento ,v.IDEnte" &
        " , Case Isnull(Coalesce(v.VecchiaDenominazione,v.Denominazione),'') when '' then '' else 'Denominazione: ' + rtrim(ltrim(v.Denominazione)) + ' | ' End +" &
        " Case Isnull(Coalesce(v.VecchiaEmail,v.Email),'') when '' then '' else 'Email: ' + rtrim(ltrim(v.Email)) + ' | ' End +" &
        " Case Isnull(Coalesce(v.VecchiaEmailCertificata,v.EmailCertificata),'') when '' then '' else 'Email Certificata: ' + rtrim(ltrim(v.EmailCertificata)) + ' | ' End +" &
        " Case Isnull(Coalesce(v.VecchioPrefissoTelefonoRichiestaRegistrazione,v.PrefissoTelefonoRichiestaRegistrazione),'') when '' then '' else 'Prefisso Telefono: ' + rtrim(ltrim(v.PrefissoTelefonoRichiestaRegistrazione)) + ' | ' End +" &
        " Case Isnull(Coalesce(v.VecchioTelefonoRichiestaRegistrazione,v.TelefonoRichiestaRegistrazione),'') when '' then '' else 'Telefono: ' + rtrim(ltrim(v.TelefonoRichiestaRegistrazione)) + ' | ' End +" &
        " Case Isnull(Coalesce(v.VecchioHttp,v.Http),'') when '' then '' else 'Sito Web: ' + rtrim(ltrim(v.Http)) + ' | ' End +" &
        " Case Isnull(Coalesce(v.VecchioIdComune,v.IdComune),0) when 0 then '' else 'Comune: ' + rtrim(ltrim(comuni.denominazione)) + ' | ' End +" &
        " Case Isnull(Coalesce(v.VecchioCAP,v.CAP),'') when '' then '' else 'CAP: ' + rtrim(ltrim(v.CAP)) + ' | ' End +" &
        " Case Isnull(Coalesce(v.VecchioIndirizzo,v.Indirizzo),'') when '' then '' else 'Indirizzo: ' + rtrim(ltrim(v.Indirizzo)) + ' | ' End +" &
        " Case Isnull(Coalesce(v.VecchioCivico,v.Civico),'') when '' then '' else 'Civico: ' + rtrim(ltrim(v.Civico)) + ' | ' End +" &
        " Case Isnull(Coalesce(v.VecchioDettaglioRecapito,v.DettaglioRecapito),'') when '' then '' else 'Dettaglio Recapito: ' + rtrim(ltrim(v.DettaglioRecapito)) + ' | ' End as Variazioni" &
        " , v.FlgValidazione ,v.FlgAttivo ,v.DataVariazione ,v.FlgMail, a.codiceregione as CodiceEnte, a.Denominazione+coalesce('<br>'+a.CodiceFiscale, '') as ENTE" &
        " FROM dbo.EntiFuoriAdeguamento v Left join comuni on v.IdComune = comuni.idComune" &
        " inner join Enti a on v.IDEnte = a.IDEnte" &
        " inner join classiaccreditamento b on b.idclasseaccreditamento = a.idclasseaccreditamento" &
        " inner join classiaccreditamento c on c.idclasseaccreditamento=a.idclasseaccreditamentoRichiesta" &
        " inner join TipologieEnti t on a.tipologia =t.descrizione" &
        " where v.FlgAttivo=1"
        strsql = "SELECT v.IDEnteFuoriAdeguamento ,v.IDEnte, " &
        calcDiff("VecchiaDenominazione", "Denominazione", "Denominazione") & " + " & vbCrLf &
        calcDiff("VecchiaEmail", "Email", "Email") & " + " & vbCrLf &
        calcDiff("VecchiaEmailCertificata", "EmailCertificata", "Email Certificata") & " + " & vbCrLf &
        calcDiff("VecchioPrefissoTelefonoRichiestaRegistrazione", "PrefissoTelefonoRichiestaRegistrazione", "Prefisso Telefono") & " + " & vbCrLf &
        calcDiff("VecchioTelefonoRichiestaRegistrazione", "TelefonoRichiestaRegistrazione", "Telefono") & " + " & vbCrLf &
        calcDiff("VecchioHttp", "Http", "Sito Web") & " + " & vbCrLf &
        calcDiffComune("VecchioIdComune", "IdComune", "Comune") & " + " & vbCrLf &
        calcDiff("VecchioCAP", "CAP", "CAP") & " + " & vbCrLf &
        calcDiff("VecchioIndirizzo", "Indirizzo", "Indirizzo") & " + " & vbCrLf &
        calcDiff("VecchioCivico", "Civico", "Civico") & " + " & vbCrLf &
        calcDiff("VecchioDettaglioRecapito", "DettaglioRecapito", "Dettaglio Recapito") & " as Variazioni  " & vbCrLf &
        " , v.FlgValidazione ,v.FlgAttivo ,v.DataVariazione ,v.FlgMail, a.codiceregione as CodiceEnte, a.Denominazione+coalesce('<br>CF '+a.CodiceFiscale, '') as ENTE,identerelazione" &
        " FROM dbo.EntiFuoriAdeguamento v Left join comuni on v.IdComune = comuni.idComune" &
        " inner join Enti a on v.IDEnte = a.IDEnte" &
        " inner join classiaccreditamento b on b.idclasseaccreditamento = a.idclasseaccreditamento" &
        " inner join classiaccreditamento c on c.idclasseaccreditamento=a.idclasseaccreditamentoRichiesta" &
        " inner join TipologieEnti t on a.tipologia =t.descrizione" &
        " left join entirelazioni on identefiglio=a.IDEnte " &
        " where v.FlgAttivo=1"

        'imposto eventuali parametri
        If txtdenominazione.Text <> "" Then
            strsql = strsql & " and a.denominazione like '" & ClsServer.NoApice(txtdenominazione.Text) & "%'"
        End If
        If txtCodiceEnte.Text <> "" Then
            strsql = strsql & " and a.CodiceRegione ='" & ClsServer.NoApice(txtCodiceEnte.Text) & "'"
        End If

        If ddlTipologia.SelectedItem.Text <> "" Then
            If ddlTipologia.SelectedItem.Text = "Privato" Then
                strsql = strsql & " and t.privato =1"
            Else
                strsql = strsql & " and t.privato =0"
            End If

        End If

        If ddlClasseAttribuita.SelectedItem.Text <> "Seleziona" Then
            strsql = strsql & " and  a.IdSezione='" & ddlClasseAttribuita.SelectedValue & "'"
        End If
        If ddlVisto.SelectedItem.Text <> "TUTTI" Then
            strsql = strsql & " and v.FlgValidazione = " & ddlVisto.SelectedValue
        End If

        strsql = strsql & " order by a.denominazione"

        dtsGenerico = ClsServer.DataSetGenerico(strsql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
        If txtRicerca.Value <> "" Then
            dgRisultatoRicerca.CurrentPageIndex = 0
            txtRicerca.Value = ""
        End If
        If lblpage.Value <> "" Then
            dgRisultatoRicerca.CurrentPageIndex = CInt(lblpage.Value)
        Else
            dgRisultatoRicerca.CurrentPageIndex = 0
        End If

        'txtstrsql.Value = strsql
        CaricaDataGrid(dgRisultatoRicerca)
        If (dgRisultatoRicerca.Items.Count > 0) Then
            CmdEsporta.Visible = True
            ApriCSV1.Visible = False
            dgRisultatoRicerca.Caption = "Risultato Ricerca Modifiche fuori adeguamento Enti"
            chkSelDesel.Visible = True
            cmdSalva.Visible = True
        Else
            CmdEsporta.Visible = False
            ApriCSV1.Visible = False
            dgRisultatoRicerca.Caption = "La ricerca non ha prodotto alcun risultato"
            chkSelDesel.Visible = False
            cmdSalva.Visible = False
        End If
        dtEnti = ClsServer.CreaDataTable(strsql, False, Session("conn"))
        'assegno il dataset alla griglia del risultato
        dgRisultatoRicerca.DataSource = dtEnti
        dgRisultatoRicerca.DataBind()
        Session("SessiondtEnti") = dtEnti
        TogliCheck()
        controllaChek()
        ColoraCelle()
        'End If
    End Sub

    Sub CaricaDataGrid(ByRef datagrid As DataGrid) 'valorizzo la datagrid passata
        'Verifico l'abilitazione alla visualizzazione di determinate colonne
        'Dim strMess As String
        'If Session("TipoUtente") = "E" Then
        '    datagrid.Columns(16).Visible = False
        '    ' agg da simona cordella il 11/12/2008
        '    ' se sono un utente di tipo E rendo la colonna invisibile certificazione invisibile
        '    datagrid.Columns(17).Visible = False

        'End If
        'datagrid.Columns(18).Visible = ClsUtility.ForzaPresenzaSanzione(Session("Utente"), Session("conn"))
        'datagrid.Columns(19).Visible = ClsUtility.ForzaSedeSottopostaVerifica(Session("Utente"), Session("conn"))

        datagrid.DataSource = dtsGenerico
        datagrid.DataBind()


        '*********************************************************************************
        'blocco per la creazione della datatable per la stampa della ricerca
        'nome e posizione di lettura delle colopnne a base 0
        Dim NomeColonne(4) As String
        Dim NomiCampiColonne(4) As String
        'nome della colonna 
        'e posizione nella griglia di lettura
        NomeColonne(0) = "Codice Ente"
        NomeColonne(1) = "Ente"
        NomeColonne(2) = "Data Variazione"
        NomeColonne(3) = "Variazioni"
        NomeColonne(4) = "Visto"
        'NomeColonne(5) = "Indirizzo"
        'NomeColonne(6) = "Comune"
        'NomeColonne(7) = "Telefono"
        'NomeColonne(8) = "NMaxVolontari"
        'NomeColonne(9) = "Titolo di Possedimento"
        'NomeColonne(10) = "Normativa81"
        'NomeColonne(11) = "Conformita"
        'NomeColonne(12) = "Soggetto Estero"
        'NomeColonne(13) = "Dichiarazione di Soggetto Estero"


        'If Session("TipoUtente") <> "E" Then
        '    NomeColonne(14) = "Presenza Certificazione"
        'End If


        NomiCampiColonne(0) = "CodiceEnte"
        NomiCampiColonne(1) = "Ente"
        NomiCampiColonne(2) = "DataVariazione"
        NomiCampiColonne(3) = "Variazioni"
        NomiCampiColonne(4) = "FlgValidazione"
        'NomiCampiColonne(5) = "Indirizzo"
        'NomiCampiColonne(6) = "Comune"
        'NomiCampiColonne(7) = "Telefono"
        'NomiCampiColonne(8) = "NMaxVolontari"
        'NomiCampiColonne(9) = "TitoloPossedimento"
        'NomiCampiColonne(10) = "Normativa81"
        'NomiCampiColonne(11) = "Conformita"
        'NomiCampiColonne(12) = "Soggettoestero"
        'NomiCampiColonne(13) = "DichSoggettoEstero"
        'If Session("TipoUtente") <> "E" Then
        '    NomiCampiColonne(14) = "Certificazione"
        'End If

        'carico un datatable che userò poi nella pagina di stampa
        'il numero delle colonne è a base 0
        'If Session("TipoUtente") <> "E" Then
        '    CaricaDataTablePerStampa(dtsGenerico, 14, NomeColonne, NomiCampiColonne)
        'Else
        '    CaricaDataTablePerStampa(dtsGenerico, 13, NomeColonne, NomiCampiColonne)
        'End If
        CaricaDataTablePerStampa(dtsGenerico, 4, NomeColonne, NomiCampiColonne)
        'CaricaDataTablePerStampa(dtsGenerico, 8, NomeColonne, NomiCampiColonne)

        '*********************************************************************************

        If datagrid.Items.Count = 0 Then

            If Request.QueryString("esporta") = "si" Then
                CmdEsporta.Visible = False
            End If
        Else
            ColoraCelle()
            'ControllaSede()
            If Request.QueryString("esporta") = "si" Then
                CmdEsporta.Visible = True
                datagrid.Columns(0).Visible = False
            End If
        End If
        'If Request.QueryString("VengoDaProgetti") = "AccettazioneProgetti" Then
        '    datagrid.Columns(0).Visible = False
        'End If

    End Sub

    'routine che carica la datatable che caricherà dinamicamente la datagrid della stampa delle ricerche
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

        'passo alla sessione la datatable che ho appena creato e che userò per il databinding della datagrid della stampa
        Session("DtbRicerca") = dt

    End Sub


    Private Sub controllaChek()
        Dim item As DataGridItem
        For Each item In dgRisultatoRicerca.Items
            Dim check As CheckBox = DirectCast(item.FindControl("chkEnti"), CheckBox)
            Select Case dgRisultatoRicerca.Items(item.ItemIndex).Cells(INDEX_DGRISULTATORICERCA_FLGVALIDAZIONE).Text.ToUpper
                Case "TRUE"
                    check.Checked = True
                Case Else
                    check.Checked = False
            End Select
            'check.Enabled = False
            dgRisultatoRicerca.Items(item.ItemIndex).Cells(INDEX_DGRISULTATORICERCA_VARIAZIONI).Text = Replace(dgRisultatoRicerca.Items(item.ItemIndex).Cells(INDEX_DGRISULTATORICERCA_VARIAZIONI).Text, " | ", "</br>")
        Next
    End Sub
    Private Sub ColoraCelle()

        ''VAriazione del Colore secondo lo stato della sede.
        ''Attiva=Verde;Presentata=gialla;Cancellata=Rossa;Sospesa=
        'Dim item As DataGridItem
        'Dim intConta As Integer
        'Dim img As ImageButton
        'For Each item In dgRisultatoRicerca.Items

        '    Select Case dgRisultatoRicerca.Items(item.ItemIndex).Cells(INDEX_DGRISULTATORICERCA_STATOENTESEDE).Text
        '        'Case "1"
        '            'For intConta = 0 To dgRisultatoRicerca.Columns.Count - 1
        '            '    dgRisultatoRicerca.Items(item.ItemIndex).Cells(intConta).BackColor = Color.LightGreen
        '            '    'dgRisultatoRicerca.Items(item.ItemIndex).Cells(intConta).ForeColor = Color.LightGreen
        '            'Next
        '            'aggiunto il 04/12/2008 da simona cordella
        '            '    'se sono un utene U o R coloro il font della griglio in viola sono le sede è da valutare
        '            '    If (Session("TipoUtente") = "U" Or Session("TipoUtente") = "R") Then
        '            '        'se la sede è da valutare coloro il FONT DI VIOLA
        '            '        Select Case dgRisultatoRicerca.Items(item.ItemIndex).Cells(17).Text
        '            '            Case "Da Valutare"
        '            '                For intConta = 0 To dgRisultatoRicerca.Columns.Count - 1
        '            '                    'dgRisultatoRicerca.Items(item.ItemIndex).Cells(intConta).ForeColor = Color.FromArgb(201, 96, 185)
        '            '                    dgRisultatoRicerca.Items(item.ItemIndex).Cells(intConta).Font.Bold = True
        '            '                Next
        '            '            Case "No"
        '            '                For intConta = 0 To dgRisultatoRicerca.Columns.Count - 1
        '            '                    'dgRisultatoRicerca.Items(item.ItemIndex).Cells(intConta).ForeColor = Color.Red
        '            '                Next
        '            '        End Select
        '            '    End If

        '        Case "Presentata"
        '            For intConta = 0 To dgRisultatoRicerca.Columns.Count - 1
        '                dgRisultatoRicerca.Items(item.ItemIndex).Cells(intConta).BackColor = Color.Khaki
        '            Next
        '        Case "Accreditata"
        '            For intConta = 0 To dgRisultatoRicerca.Columns.Count - 1
        '                dgRisultatoRicerca.Items(item.ItemIndex).Cells(intConta).BackColor = Color.LightGreen
        '            Next

        '        Case "Sospesa"
        '            For intConta = 0 To dgRisultatoRicerca.Columns.Count - 1
        '                dgRisultatoRicerca.Items(item.ItemIndex).Cells(intConta).BackColor = Color.Gainsboro
        '            Next
        '        Case "Cancellata"
        '            For intConta = 0 To dgRisultatoRicerca.Columns.Count - 1
        '                dgRisultatoRicerca.Items(item.ItemIndex).Cells(intConta).BackColor = Color.LightSalmon
        '            Next

        '    End Select

        'Next
    End Sub
#End Region

    Protected Sub cmdEsporta_Click(ByVal sender As Object, ByVal e As EventArgs) Handles CmdEsporta.Click
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

        Dim writer As StreamWriter
        Dim xLinea As String = String.Empty
        Dim i As Int64
        Dim j As Int64
        Dim nomeUnivoco As String
        Dim url As String
        nomeUnivoco = xPrefissoNome & "ExpDati" & Year(Now) & Month(Now) & Day(Now) & Hour(Now) & Minute(Now) & Second(Now)
        writer = New StreamWriter(mapPath & "\" & nomeUnivoco & ".CSV")
        'Creazione dell'inntestazione del CSV
        Dim intNumCol As Int64 = DTBRicerca.Columns.Count
        For i = 0 To intNumCol - 1
            xLinea &= DTBRicerca.Columns.Item(CInt(i)).ColumnName() & ";"
        Next
        writer.WriteLine(xLinea.Substring(0, xLinea.Length - 1))
        'writer.WriteLine(xLinea)
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
            writer.WriteLine(xLinea.Substring(0, xLinea.Length - 1))
            'writer.WriteLine(xLinea)
            xLinea = vbNullString

        Next
        url = "download\" & nomeUnivoco & ".CSV"

        writer.Close()
        writer = Nothing
        Return url
    End Function

    Sub CeccaTutti()
        Dim item As DataGridItem
        Dim chkValore As CheckBox
        'ricordo il valore ceccato nella pagina corrente
        For Each item In dgRisultatoRicerca.Items
            chkValore = DirectCast(item.FindControl("chkEnti"), CheckBox)
            chkValore.Checked = True
        Next
    End Sub

    Sub EliminaTuttiCheck()
        Dim item As DataGridItem
        Dim chkValore As CheckBox
        'ricordo il valore ceccato nella pagina corrente
        For Each item In dgRisultatoRicerca.Items
            chkValore = DirectCast(item.FindControl("chkEnti"), CheckBox)
            chkValore.Checked = False
        Next
    End Sub

    Sub TogliCheck()
        Dim item As DataGridItem
        Dim chkValore As CheckBox
        'ricordo il valore ceccato nella pagina corrente
        For Each item In dgRisultatoRicerca.Items
            hdd_Check.Value = "NO"
            chkValore = DirectCast(item.FindControl("chkEnti"), CheckBox)
            chkValore.Attributes.Add("onclick", "TogliCheck()")
        Next
    End Sub


    Private Sub chkSelDesel_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkSelDesel.CheckedChanged
        If hdPerSelezione.Value.ToUpper <> "BLOCCA" Then
            ShowPopUPControllo = 1
            If chkSelDesel.Checked Then
                lblMsgSeleziona.Text = "Vuoi Selezionare tutte le variazioni presenti nella lista di ricerca ?"
                cmdSeleziona.Text = "Seleziona Tutti"
            Else
                lblMsgSeleziona.Text = "Vuoi deselezionare tutte le variazioni presenti nella lista di ricerca ?"
                cmdSeleziona.Text = "Deseleziona Tutti"
            End If
        Else
            hdPerSelezione.Value = ""
        End If


        'Dim i As Integer
        'Dim Mychk As CheckBox

        'For i = 0 To dgRisultatoRicerca.Items.Count - 1
        '    Mychk = dgRisultatoRicerca.Items.Item(i).FindControl("chkEnti")

        '    If (Mychk.Enabled = True) Then
        '        Mychk.Checked = chkSelDesel.Checked
        '        hdd_Check.Value = "SI"
        '    End If

        'Next i

    End Sub

    Private Sub cmdSeleziona_Click(sender As Object, e As EventArgs) Handles cmdSeleziona.Click


        Dim i, j, page As Integer
        Dim Mychk As CheckBox
        Dim dtGriglia As DataTable 'appoggio
        Dim rwGriglia As DataRow
        Dim clGriglia As DataColumn
        Dim ValoreRichkDt As Integer = 3 ' valore che ricorda il campo flgValidazione 
        page = dgRisultatoRicerca.CurrentPageIndex
        dtGriglia = Session("SessiondtEnti")
        clGriglia = dtGriglia.Columns(ValoreRichkDt)
        clGriglia.ReadOnly = False
        For i = 0 To dgRisultatoRicerca.Items.Count - 1
            Mychk = dgRisultatoRicerca.Items.Item(i).FindControl("chkEnti")

            If (Mychk.Enabled = True) Then
                Mychk.Checked = chkSelDesel.Checked
                hdd_Check.Value = "SI"
            End If
        Next i
        For j = 0 To dtGriglia.Rows.Count - 1
            rwGriglia = dtGriglia.Rows(j)
            If chkSelDesel.Checked Then
                rwGriglia.Item(ValoreRichkDt) = 1
            Else
                rwGriglia.Item(ValoreRichkDt) = 0
            End If

        Next

        Session("SessiondtEnti") = dtGriglia
        dgRisultatoRicerca.DataSource = Session("SessiondtEnti")
        dgRisultatoRicerca.DataBind()
        TogliCheck()
        FormattaVariazioni()
        ColoraCelle()

    End Sub

    Protected Sub cmdSalva_Click(sender As Object, e As EventArgs) Handles cmdSalva.Click
        ShowPopUPControllo = 2

    End Sub


    Sub ModificaEnte(ByVal IDEnteVariazione As Integer, ByVal Validazione As Integer)
        Dim strSql As String
        Dim CmdSede As SqlClient.SqlCommand
        strSql = "UPDATE  EntifuoriAdeguamento SET "
        strSql = strSql & " flgValidazione = " & Validazione & "  WHERE IDEnteFuoriAdeguamento = " & IDEnteVariazione

        CmdSede = ClsServer.EseguiSqlClient(strSql, Session("conn"))
    End Sub

    Private Sub cmdSalvaDati_Click(sender As Object, e As EventArgs) Handles cmdSalvaDati.Click
        Dim ContaCheck As Integer = 0
        Dim i As Integer
        Dim chkValore As CheckBox
        Dim dtGriglia As DataTable 'appoggio
        Dim rwGriglia As DataRow
        Dim clGriglia As DataColumn


        'dgRisultatoRicerca.CurrentPageIndex = 0
        lblErrore.Text = ""


        dtGriglia = Session("SessiondtEnti")
        clGriglia = dtGriglia.Columns(3)
        clGriglia.ReadOnly = False

        'ricordo il valore ceccato nella pagina corrente
        For i = 0 To dgRisultatoRicerca.Items.Count - 1
            chkValore = dgRisultatoRicerca.Items.Item(i).FindControl("chkEnti")
            rwGriglia = dtGriglia.Rows(i + (dgRisultatoRicerca.CurrentPageIndex * 10))
            If chkValore.Checked = True Then
                rwGriglia.Item(3) = 1
            Else
                rwGriglia.Item(3) = 0
            End If
        Next i


        For i = 0 To dtGriglia.Rows.Count - 1
            ModificaEnte(CInt(dtGriglia.Rows(i).Item(0)), CInt(dtGriglia.Rows(i).Item(3))) 'item(6) IDVariazione Item(3) FlgValidazione
        Next i
        lblErrore.CssClass = "msgConferma"
        lblErrore.Text = "Salvataggio eseguito con successo."

        RicercaEnti()
    End Sub
    Sub FormattaVariazioni()
        Dim item As DataGridItem
        For Each item In dgRisultatoRicerca.Items
            dgRisultatoRicerca.Items(item.ItemIndex).Cells(INDEX_DGRISULTATORICERCA_VARIAZIONI).Text = Replace(dgRisultatoRicerca.Items(item.ItemIndex).Cells(INDEX_DGRISULTATORICERCA_VARIAZIONI).Text, " | ", "</br>")
        Next
    End Sub

End Class