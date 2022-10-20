Imports System.Drawing.Printing
Imports System.Drawing.Imaging
Imports System.Web.UI
Imports System.Drawing
Imports System.IO
Public Class WfrmRicercaEnti
    Inherits System.Web.UI.Page
    Private WithEvents pndocument As PrintDocument
    Dim strsql As String
    Dim dtrgenerico As SqlClient.SqlDataReader
    Dim dtsGenerico As DataSet

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Response.ExpiresAbsolute = #1/1/1980#
        Response.AddHeader("Pragma", "no-cache")
        If Not Session("LogIn") Is Nothing Then
            If Session("LogIn") = False Then 'verifico validità log-in
                Response.Redirect("LogOn.aspx")
            End If
        Else
            Response.Redirect("LogOn.aspx")
        End If
        If Not IsPostBack = True Then
            Session("DtbRicerca") = Nothing

            dtrgenerico = ClsServer.CreaDatareader("select idTipologieEnti='',Descrizione ='' union select idTipologieEnti,Descrizione from TipologieEnti", Session("conn"))
            ddlTipologia.DataSource = dtrgenerico

            ddlTipologia.DataValueField = "idTipologieEnti"
            ddlTipologia.DataTextField = "Descrizione"
            ddlTipologia.DataBind()

            'cerco il codice regione e lo stampo nel caso in cui ci sia
            If Not dtrgenerico Is Nothing Then
                dtrgenerico.Close()
                dtrgenerico = Nothing
            End If

            Dim strsql As String = "SELECT '0' AS idSezione,'Seleziona' AS classeAccreditamento "
            strsql &= "FROM SezioniAlboSCU "
            strsql &= "UNION "
            strsql &= "SELECT S.idSezione , S.Sezione AS classeAccreditamento "
            strsql &= "FROM SezioniAlboSCU S JOIN classiaccreditamento C "
            strsql &= "ON S.IdClasseAccreditamento = C.IDClasseAccreditamento "
            strsql &= "WHERE(C.MINSEDI > 0) "

            'Caricamento delle DropDownList Classe Attribuita
            dtrgenerico = ClsServer.CreaDatareader(strsql, Session("conn"))
            ddlClasseAttribuita.DataSource = dtrgenerico
            ddlClasseAttribuita.DataValueField = "idSezione"
            ddlClasseAttribuita.DataTextField = "classeAccreditamento"
            ddlClasseAttribuita.DataBind()
            If Not dtrgenerico Is Nothing Then
                dtrgenerico.Close()
                dtrgenerico = Nothing
            End If
            'Caricamento delle DropDownList Classe Richiesta
            dtrgenerico = ClsServer.CreaDatareader(strsql, Session("conn"))
            ddlClasseRichiesta.DataSource = dtrgenerico
            ddlClasseRichiesta.DataValueField = "idSezione"
            ddlClasseRichiesta.DataTextField = "classeAccreditamento"
            ddlClasseRichiesta.DataBind()
            If Not dtrgenerico Is Nothing Then
                dtrgenerico.Close()
                dtrgenerico = Nothing
            End If
            dtrgenerico = ClsServer.CreaDatareader("select '0' as IdStatoEnte, 'Seleziona' as StatoEnte from StatiEnti " & _
                          " union " & _
                          " Select IdStatoEnte, StatoEnte " & _
                          " from StatiEnti " & _
                          " WHERE DefaultStato <> 1 " & _
                          " AND Chiuso <> 1 ", Session("conn"))
            ddlStato.DataSource = dtrgenerico
            ddlStato.DataValueField = "IdStatoEnte"
            ddlStato.DataTextField = "StatoEnte"
            ddlStato.DataBind()
            If Not dtrgenerico Is Nothing Then
                dtrgenerico.Close()
                dtrgenerico = Nothing
            End If

            strsql = "SELECT '0' as IDCausale, 'Seleziona' as Descrizione FROM Causali " & _
                     " UNION " & _
                     " SELECT IDCausale, Descrizione " & _
                     " FROM Causali WHERE Tipo = 12"
            dtrgenerico = ClsServer.CreaDatareader(strsql, Session("conn"))
            ddlCausaleChiusura.DataSource = dtrgenerico
            ddlCausaleChiusura.DataValueField = "IDCausale"
            ddlCausaleChiusura.DataTextField = "Descrizione"
            ddlCausaleChiusura.DataBind()
            If Not dtrgenerico Is Nothing Then
                dtrgenerico.Close()
                dtrgenerico = Nothing
            End If


            CaricaCompetenze()
            If lblmessaggio.Text <> "Utilizzare il pulsante Ricerca per ottenere la lista dei risultati." Then
                RicercaEnti()
            End If

        End If
        'If lblmessaggio.Text <> "" Then
        '    RicercaEnti()
        'End If
        'If dgRisultatoRicerca.Items.Count > 0 Then
        '    'imgStampa.Visible = True
        '    ApriCSV1.Visible = True
        'Else
        '    ApriCSV1.Visible = False
        '    'imgStampa.Visible = False
        'End If

    End Sub
    Private Sub RicercaEnti()
        strsql = "Select enti.IDente, enti.Denominazione, enti.CodiceRegione, enti.Tipologia, enti.idClasseAccreditamento, " &
                 "statienti.Statoente + CASE WHEN IdCausaleChiusura IS NULL " &
                 "                           THEN '' " &
                 "                         ELSE ' (' + (SELECT Descrizione FROM Causali WHERE IDCausale = enti.IdCausaleChiusura) +')' " &
                 "                      END AS StatoEnte, " &
                 "coalesce(sezioni.sezione, Attribuita.Classeaccreditamento) classeAttribuita, " &
                 "coalesce(sezioni.sezione, Richiesta.Classeaccreditamento) ClasseRichiesta, " &
                 "PrefissoTelefonoRichiestaRegistrazione + TelefonoRichiestaRegistrazione as Tel, " &
                 "PrefissoFax+Fax as Fax, enti.EmailCertificata, " &
                 "(SELECT (COUNT(*) + (SELECT count(*) " &
                 "                       FROM entisedi " &
                 "                         INNER JOIN entisediattuazioni ON entisedi.IDEnteSede = entisediattuazioni.IDEnteSede " &
                 "                         INNER JOIN StatiEntiSedi ON entisedi.IDStatoEnteSede = StatiEntiSedi.IdStatoEnteSede " &
                 "                         INNER JOIN StatiEntiSedi StatiEntiSedi_1 ON entisediattuazioni.IdStatoEnteSede = StatiEntiSedi_1.IdStatoEnteSede " &
                 "                       WHERE entisedi.IDEnte = enti.idente " &
                 "                         AND ((StatiEntiSedi.Attiva = 1 OR StatiEntiSedi.DaAccreditare = 1) " &
                 "                         AND (StatiEntiSedi_1.Attiva = 1 OR StatiEntiSedi_1.DaAccreditare = 1) " &
                 "                          OR (StatiEntiSedi.IdStatoEnteSede = 3 AND enti.IDStatoEnte IN (7,10))) " &
                 "                         AND (SUBSTRING(entisedi.usernamestato,1,1) <> 'N')))as nsedi " &
                 "  FROM entisedi " &
                 "    INNER JOIN entisediattuazioni ON entisedi.IDEnteSede = entisediattuazioni.IDEnteSede " &
                 "    INNER JOIN StatiEntiSedi ON entisedi.IDStatoEnteSede = StatiEntiSedi.IdStatoEnteSede " &
                 "    INNER JOIN StatiEntiSedi StatiEntiSedi_1 ON entisediattuazioni.IdStatoEnteSede = StatiEntiSedi_1.IdStatoEnteSede " &
                 "    INNER JOIN entirelazioni ON entisedi.IDEnte = entirelazioni.IDEnteFiglio  " &
                 "    INNER JOIN AssociaEntiRelazioniSediAttuazioni ON entisediattuazioni.IDEnteSedeAttuazione = AssociaEntiRelazioniSediAttuazioni.IdEnteSedeAttuazione " &
                 "              AND entirelazioni.IDEnteRelazione = AssociaEntiRelazioniSediAttuazioni.IdEnteRelazione " &
                 "  WHERE entirelazioni.IDEntePadre = enti.idente " &
                 "    AND (((StatiEntiSedi.Attiva = 1 OR StatiEntiSedi.DaAccreditare = 1) " &
                 "    AND (StatiEntiSedi_1.Attiva = 1 or StatiEntiSedi_1.DaAccreditare = 1)) " &
                 "     OR StatiEntiSedi.IdStatoEnteSede = 3 and enti.IDStatoEnte IN (7,10)) " &
                 "    AND GETDATE() BETWEEN ISNULL(entirelazioni.DataInizioValidità, '2000-01-01') AND ISNULL(entirelazioni.DataFineValidità,'2030-01-01')" &
                 "    AND SUBSTRING(entisedi.usernamestato,1,1) <> 'N') AS Numerototalesedi, " &
                 " enti.Http as Http,enti.datacontrollohttp, enti.codicefiscale, enti.email,enti.datacontrolloemail, " &
                 " isnull((select descrizione from regionicompetenze where regionicompetenze.idregionecompetenza = enti.idregionecompetenza),'Nessuna') Competenza,  " &
                 "  enti.idClasseAccreditamentoRichiesta " &
                 "  from Enti  inner join classiaccreditamento as Attribuita on  " &
                 " (Attribuita.idclasseaccreditamento = Enti.idclasseaccreditamento) " &
                 " inner join classiaccreditamento as Richiesta on  " &
                 " (Richiesta.idclasseaccreditamento=Enti.idclasseaccreditamentoRichiesta)" &
                 " inner join statienti on (statienti.idstatoente=enti.idstatoente)" &
                 "    LEFT JOIN SezioniAlboSCU AS sezioni on sezioni.IdSezione=enti.IdSezione "


        If ddlFasi.SelectedValue > 0 Then
            strsql = strsql & " inner join entifasi as fasi on enti.idente = fasi.idente and (GETDATE() BETWEEN fasi.DataInizioFase AND fasi.DataFineFase) "
        End If
        strsql = strsql & " where enti.idente > 0 and statienti.defaultstato <> 1 and statienti.chiuso <> 1 "

        If Trim(txtdenominazione.Text) <> "" Then
            strsql = strsql & " and enti.denominazione like '" & Replace(txtdenominazione.Text, "'", "''") & "%'"
        End If
        If Trim(txtCodEnte.Text) <> "" Then
            strsql = strsql & " and enti.Codiceregione ='" & Replace(txtCodEnte.Text, "'", "''") & "'"
        End If
        If Trim(txtCodFis.Text) <> "" Then
            strsql = strsql & " and enti.CodiceFiscale like '" & Replace(txtCodFis.Text, "'", "''") & "%'"
        End If
        If Trim(txtCodFisArchivio.Text) <> "" Then
            strsql = strsql & " and enti.CodiceFiscaleArchivio like '" & Replace(txtCodFisArchivio.Text, "'", "''") & "%'"
        End If
        If UCase(Trim(ddlTipologia.SelectedItem.Text)) <> "" Then
            strsql = strsql & " and enti.tipologia='" & UCase(Trim(Replace(ddlTipologia.SelectedItem.Text, "'", "''"))) & "'"
        End If
        If ddlClasseAttribuita.SelectedValue > 0 Then
            'strsql = strsql & " and  enti.idClasseAccreditamento='" & ddlClasseAttribuita.SelectedValue & "'"
            strsql &= " and enti.IdSezione = " & ddlClasseAttribuita.SelectedValue
        End If
        If ddlClasseRichiesta.SelectedValue > 0 Then
            'strsql = strsql & " and  enti.idClasseAccreditamentoRichiesta='" & ddlClasseRichiesta.SelectedValue & "'"
            strsql &= " and enti.IdSezione = " & ddlClasseRichiesta.SelectedValue

        End If
        If ddlAlbo.SelectedIndex > 0 Then
            strsql = strsql & " and  enti.Albo='" & ddlAlbo.Text & "'"
        End If
        If ddlStato.SelectedValue > 0 Then
            strsql = strsql & " and  enti.idstatoente=" & ddlStato.SelectedValue & ""
        End If
        If ddlCausaleChiusura.SelectedValue > 0 Then
            strsql = strsql & " and enti.idCausaleChiusura=" & ddlCausaleChiusura.SelectedValue & ""
        End If
        If ddlAttesaAccreditamento.SelectedValue <> "" Then
            strsql = strsql & " and enti.FlagAccreditamentoCompleto=" & ddlAttesaAccreditamento.SelectedValue & " and enti.idstatoente = 8 "
        End If
        'If chkhttp.Checked = True Then
        '    If chkhttpIns.Checked = True And chkVerhttp.Checked = False Then
        '        strsql = strsql & " and not enti.http is null and enti.datacontrollohttp is null"
        '    End If
        '    If chkhttpIns.Checked = True And chkVerhttp.Checked = True Then
        '        strsql = strsql & " and not enti.http is null and not enti.datacontrollohttp is null "
        '    End If
        '    If chkhttpIns.Checked = False And chkVerhttp.Checked = False Then
        '        strsql = strsql & " and enti.http is null "
        '    End If
        'End If
        'If chkemail.Checked = True Then
        '    If chkInsmail.Checked = True And chkVerMail.Checked = False Then
        '        strsql = strsql & " and not enti.email is null and enti.datacontrolloemail is null"
        '    End If
        '    If chkInsmail.Checked = True And chkVerMail.Checked = True Then
        '        strsql = strsql & " and not enti.email is null and not enti.datacontrolloemail is null "
        '    End If
        '    If chkInsmail.Checked = False And chkVerMail.Checked = False Then
        '        strsql = strsql & " and enti.email is null "
        '    End If
        'End If

        'Aggiunto da ALessandra Taballione il 25/05/2005
        'Filtro partita Iva e Codice fiscale ente
        'If Trim(txtCodFis.Text) <> "" Then
        '    strsql = strsql & " and enti.codiceFiscale='" & Replace(txtCodFis.Text, "'", "''") & "' "
        'End If
        ''''If Trim(txtPiva.Text) <> "" Then
        ''''    strsql = strsql & " and enti.PartitaIva='" & Replace(txtPiva.Text, "'", "''") & "' "
        ''''End If
        ' Filtro per regioni


        If CboCompetenza.SelectedValue <> "" Then
            Select Case CboCompetenza.SelectedValue
                Case 0
                    strsql = strsql & " "
                Case -1
                    strsql = strsql & " And enti.IdRegioneCompetenza = 22"
                Case -2
                    strsql = strsql & " And enti.IdRegioneCompetenza <> 22 And not enti.IdRegioneCompetenza is null "
                Case -3
                    strsql = strsql & " And enti.IdRegioneCompetenza is null "
                Case Else
                    strsql = strsql & " And enti.IdRegioneCompetenza = " & CboCompetenza.SelectedValue
            End Select

        End If

        If Trim(txtEmail.Text) <> "" Then
            strsql = strsql & " and enti.Email='" & Replace(txtEmail.Text, "'", "''") & "' "
        End If

        'filtro per PEC e verifica esistenza PEC
        If Trim(txtPEC.Text) <> "" Then
            strsql = strsql & " and enti.EmailCertificata='" & Replace(txtPEC.Text, "'", "''") & "' "
        End If
        If ddlEsistenzaPEC.SelectedValue <> "" Then
            If ddlEsistenzaPEC.SelectedValue = 0 Then 'no PEC
                strsql = strsql & " and enti.EmailCertificata IS NULL "
            Else ' SI PEC
                strsql = strsql & " and NOT enti.EmailCertificata IS NULL "
            End If
        End If


        'filtro per RichiestaVariazione
        If ddlRichistaVariazione.SelectedValue <> "" Then
            strsql = strsql & " and isnull(enti.RichiestaModifica,0)=" & ddlRichistaVariazione.SelectedValue & " "
        End If

        '****
        If ddlFasi.SelectedValue > 0 Then
            strsql = strsql & " and fasi.tipofase=" & ddlFasi.SelectedValue & " "
        End If

        Select Case ddlTitolareAccoglienza.SelectedValue
            Case 0 ' Tutti
            Case 1 ' Titolare
                'strsql = strsql & " AND enti.idEnte NOT IN (SELECT IDEnteFiglio FROM EntiRelazioni) "
                strsql = strsql & " AND enti.IdClasseAccreditamentoRichiesta NOT IN (5,7)"
            Case 2 ' Ente Accoglienza
                strsql = strsql & " AND enti.IdClasseAccreditamentoRichiesta IN (5,7)"
        End Select

        strsql = strsql & " order by ltrim(enti.denominazione) "
        dtsGenerico = ClsServer.DataSetGenerico(strsql, Session("conn"))
        'Luigi - Aggiunta
        Session("DtsRicEnti") = dtsGenerico
        'If txtRicerca.Text <> "" Then
        '    dgRisultatoRicerca.CurrentPageIndex = 0
        '    txtRicerca.Text = ""
        'End If
        'ApriCSV1.Visible = True
        CmdEsporta.Visible = True
        'imgStampa.Visible = True
        CaricaDataGrid(dgRisultatoRicerca)

    End Sub
    Sub CaricaCompetenze()
        'stringa per la query
        Dim strSQL As String
        'datareader che conterrà l'id 
        Dim dtrCompetenze As System.Data.SqlClient.SqlDataReader
        'Dim dtradc As System.Data.SqlClient.SqlDataReader

        Try
            'controllo se si tratta del primo caricamento. così leggo i dati nel db una sola volta
            If Page.IsPostBack = False Then
                'preparo la query

                strSQL = "select IdRegioneCompetenza,Descrizione,CodiceRegioneCompetenza,left(CodiceRegioneCompetenza,1)from RegioniCompetenze where IdRegioneCompetenza <> 22 "
                strSQL = strSQL & " union "
                strSQL = strSQL & " select '0',' Tutti ','','A' "
                strSQL = strSQL & " union "
                strSQL = strSQL & " select '-1',' NAZIONALE ','','B' "
                strSQL = strSQL & " union "
                strSQL = strSQL & " select '-2',' REGIONALE ','','C' "
                strSQL = strSQL & " union "
                strSQL = strSQL & " select '-3',' NON DEFINITO ','','D' "
                strSQL = strSQL & "  from RegioniCompetenze order by left(CodiceRegioneCompetenza,1),descrizione "
                'chiudo il datareader se aperto
                If Not dtrCompetenze Is Nothing Then
                    dtrCompetenze.Close()
                    dtrCompetenze = Nothing
                End If
                'dtradc = ClsServer.CreaDatareader(strSQL, Session("conn"))
                'eseguo la query
                dtrCompetenze = ClsServer.CreaDatareader(strSQL, Session("conn"))
                'assegno il datadearder alla combo caricando così descrizione e id
                CboCompetenza.DataSource = dtrCompetenze
                CboCompetenza.Items.Add("")
                CboCompetenza.DataTextField = "Descrizione"
                CboCompetenza.DataValueField = "IDRegioneCompetenza"
                CboCompetenza.DataBind()
                'chiudo il datareader se aperto
                ''ADC
                If Not dtrCompetenze Is Nothing Then
                    dtrCompetenze.Close()
                    dtrCompetenze = Nothing
                End If
                ''''
            End If
            'Controllo abilitazione scelta
            If Session("TipoUtente") = "U" Then
                CboCompetenza.Enabled = True
                CboCompetenza.SelectedIndex = 0

            Else
                'CboCompetenza.SelectedIndex = 1
                CboCompetenza.Enabled = False
                'preparo la query
                strSQL = "select b.IdRegioneCompetenza ,b.Heliosread from RegioniCompetenze a "
                strSQL = strSQL & "INNER JOIN utentiunsc b ON a.idregionecompetenza = b.idregionecompetenza "
                strSQL = strSQL & "where b.username = '" & Session("Utente") & "'"
                'chiudo il datareader se aperto
                If Not dtrCompetenze Is Nothing Then
                    dtrCompetenze.Close()
                    dtrCompetenze = Nothing
                End If
                'controllo se utente o ente regionale
                'eseguo la query
                dtrCompetenze = ClsServer.CreaDatareader(strSQL, Session("conn"))
                dtrCompetenze.Read()
                If dtrCompetenze.HasRows = True Then
                    CboCompetenza.SelectedValue = dtrCompetenze("IdRegioneCompetenza")
                    If dtrCompetenze("Heliosread") = True Then
                        CboCompetenza.Enabled = True
                    End If

                End If

            End If
        Catch ex As Exception
            Response.Write(ex.Message.ToString())
            If Not dtrCompetenze Is Nothing Then
                dtrCompetenze.Close()
                dtrCompetenze = Nothing
            End If
        End Try
        If Not dtrCompetenze Is Nothing Then
            dtrCompetenze.Close()
            dtrCompetenze = Nothing
        End If
    End Sub

    Sub CaricaDataGrid(ByRef GridDaCaricare As DataGrid) 'valorizzo la datagrid passata
        GridDaCaricare.DataSource = dtsGenerico
        GridDaCaricare.DataBind()

        'Dim dd As Integer

        'For dd = 0 To dtsGenerico.Tables(0).Rows.Count

        'Next

        '*********************************************************************************
        'blocco per la creazione della datatable per la stampa della ricerca

        'nome e posizione di lettura delle colopnne a base 0
        Dim NomeColonne(14) As String
        Dim NomiCampiColonne(14) As String
        'nome della colonna 
        'e posizione nella griglia di lettura
        NomeColonne(0) = "Cod.Ente"
        NomeColonne(1) = "Denominazione"
        NomeColonne(2) = "Stato"
        NomeColonne(3) = "Tipologia"
        NomeColonne(4) = "Sezione Richiesta"
        NomeColonne(5) = "Sezione Attribuita"
        NomeColonne(6) = "Nr.Sedi Attuazione"
        NomeColonne(7) = "http://"
        NomeColonne(8) = "Email"
        NomeColonne(9) = "PEC"
        NomeColonne(10) = "Telefono"
        NomeColonne(11) = "Fax"
        NomeColonne(12) = "Competenza"
        NomeColonne(13) = "ID Ente"
        NomeColonne(14) = "Sezione Iscrizione Richiesta"
        NomiCampiColonne(0) = "codiceregione"
        NomiCampiColonne(1) = "Denominazione"
        NomiCampiColonne(2) = "Statoente"
        NomiCampiColonne(3) = "tipologia"
        NomiCampiColonne(4) = "ClasseRichiesta"
        NomiCampiColonne(5) = "ClasseAttribuita"
        NomiCampiColonne(6) = "Numerototalesedi"
        NomiCampiColonne(7) = "http"
        NomiCampiColonne(8) = "email"
        NomiCampiColonne(9) = "EmailCertificata"
        NomiCampiColonne(10) = "Tel"
        NomiCampiColonne(11) = "Fax"
        NomiCampiColonne(12) = "Competenza"
        NomiCampiColonne(13) = "IdEnte"
        NomiCampiColonne(14) = "idClasseAccreditamentoRichiesta"

        'carico un datatable che userò poi nella pagina di stampa
        'il numero delle colonne è a base 0
        CaricaDataTablePerStampa(dtsGenerico, 14, NomeColonne, NomiCampiColonne)

        '*********************************************************************************

        GridDaCaricare.Visible = True
        If GridDaCaricare.Items.Count = 0 Then
            'GridDaCaricare.Visible = False
            lblmessaggio.Text = "Nessun Dato estratto."
            'ApriCSV1.Visible = False
            CmdEsporta.Visible = False
        Else
            lblmessaggio.Text = "Risultato Ricerca Elenco Enti."
            'ApriCSV1.Visible = True
            CmdEsporta.Visible = True
        End If
        'ControllahttpEmailVerificato()
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

        'passo alla sessione la datatable che ho appena creato e che userò per il databinding della datagrid della stampa
        Session("DtbRicerca") = dt
        Dim stampa As String
        stampa = Request.Params("Stampa")

        'dgRisultatoRicerca.DataSource = Session("DtbRicerca")
        'lblNumOcc.Text = CType(Session("DtbRicerca"), DataTable).Rows.Count.ToString("###,##0")
        'dgRisultatoRicerca.DataBind()
       
       
    End Sub

    Protected Sub CmdChiudi_Click(sender As Object, e As EventArgs) Handles CmdChiudi.Click
        Response.Redirect("WfrmMain.aspx")
    End Sub

    Protected Sub CmdRicerca_Click(sender As Object, e As EventArgs) Handles CmdRicerca.Click
        ApriCSV1.Visible = False
        dgRisultatoRicerca.CurrentPageIndex = 0
        RicercaEnti()
    End Sub

    

    Private Sub dgRisultatoRicerca_PageIndexChanged(source As Object, e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgRisultatoRicerca.PageIndexChanged
        dtsGenerico = Session("DtsRicEnti")
        dgRisultatoRicerca.SelectedIndex = -1
        dgRisultatoRicerca.EditItemIndex = -1
        dgRisultatoRicerca.CurrentPageIndex = e.NewPageIndex
        CaricaDataGrid(dgRisultatoRicerca)
    End Sub
    'Private Sub dgRisultatoRicerca_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles dgRisultatoRicerca.SelectedIndexChanged
    '    Dim ClasseAccreditamentoRichiesta As Integer

    '    ClasseAccreditamentoRichiesta = dgRisultatoRicerca.SelectedItem.Cells(15).Text
    '    If ClasseAccreditamentoRichiesta = 5 Or ClasseAccreditamentoRichiesta = 7 Then
    '        Exit Sub
    '    Else
    '        Session("IdEnte") = dgRisultatoRicerca.SelectedItem.Cells(10).Text
    '        'Session("Ente") = dgRisultatoRicerca.SelectedItem.Cells(1).Text
    '        Session("Denominazione") = dgRisultatoRicerca.SelectedItem.Cells(2).Text
    '        Session("Competenza") = dgRisultatoRicerca.SelectedItem.Cells(13).Text
    '        Session("CodiceRegioneEnte") = "[" & dgRisultatoRicerca.SelectedItem.Cells(1).Text & "]"
    '        Session("txtCodEnte") = dgRisultatoRicerca.SelectedItem.Cells(1).Text
    '        Response.Redirect("WfrmMain.aspx?regione=" & Session("Competenza"))
    '    End If
    'End Sub

    Function StampaCSV(ByVal DTBRicerca As DataTable)

        Dim dtrSediAttuazione As Data.SqlClient.SqlDataReader

        Dim Writer As StreamWriter
        Dim xLinea As String
        Dim i As Int64
        Dim j As Int64
        Dim NomeUnivoco As String
        Dim xPrefissoNome As String = vbNullString
        Dim Reader As StreamReader

        If DTBRicerca.Rows.Count = 0 Then
            'lblmessaggio.Text = lblmessaggio.Text & "La ricerca non ha prodotto nessun risultato."
            'lblStampa.Visible = False
            'hlCSVRicerca.Visible = False
            ApriCSV1.Visible = False
            CmdEsporta.Visible = False
        Else
            xPrefissoNome = Session("Utente")
            NomeUnivoco = xPrefissoNome & "ExpDati" & Year(Now) & Month(Now) & Day(Now) & Hour(Now) & Minute(Now) & Second(Now)
            Writer = New StreamWriter(Server.MapPath("download") & "\" & NomeUnivoco & ".CSV")
            'Creazione dell'inntestazione del CSV
            Dim intNumCol As Int64 = DTBRicerca.Columns.Count
            For i = 0 To intNumCol - 3
                xLinea &= DTBRicerca.Columns.Item(CInt(i)).ColumnName() & ";"
            Next
            Writer.WriteLine(xLinea)
            xLinea = vbNullString

            'Scorro tutte le righe del datatable e riempio il CSV
            For i = 0 To DTBRicerca.Rows.Count - 1

                For j = 0 To intNumCol - 3
                    If IsDBNull(DTBRicerca.Rows(CInt(i)).Item(CInt(j))) = True Then
                        xLinea &= vbNullString & ";"
                    Else
                        xLinea &= ClsUtility.FormatExport(DTBRicerca.Rows(CInt(i)).Item(CInt(j))) & ";"
                    End If
                Next

                Writer.WriteLine(xLinea)
                xLinea = vbNullString

            Next

            'lblStampa.Visible = True
            'hlCSVRicerca.Visible = True
            ApriCSV1.Visible = True
            ApriCSV1.NavigateUrl = "download\" & NomeUnivoco & ".CSV"

            Writer.Close()
            Writer = Nothing

        End If

    End Function
    Protected Sub CmdEsporta_Click(sender As Object, e As EventArgs) Handles CmdEsporta.Click
        CmdEsporta.Visible = False
        StampaCSV(Session("DtbRicerca"))
    End Sub

    Protected Sub btnSeleziona_Click(sender As Object, e As CommandEventArgs)

        'Session("IdEnte") = Convert.ToInt32(e.CommandArgument).ToString()

        'Dim indiceTot As Integer
        'indiceTot = dgRisultatoRicerca1.Rows.Count - 1
        'Dim i As Integer

        ''Dim x As New row
        'For i = 0 To indiceTot

        '    If dgRisultatoRicerca1.Rows.Item(i).Cells(1).Text = e.CommandArgument.ToString() Then
        '        Exit For

        '    End If

        'Next

        'Session("Denominazione") = dgRisultatoRicerca1.Rows.Item(i).Cells(2).Text
        ''Session("IdEnte") = dgRisultatoRicerca1.Rows.Item(0).Cells(11).Text
        ''Session("Ente") = dgRisultatoRicerca.SelectedItem.Cells(1).Text
        ''Session("Denominazione") = dgRisultatoRicerca1.Rows.Item(0).Cells(2).Text
        ''Session("Competenza") = dgRisultatoRicerca1.Rows.Item(0).Cells(13).Text
        ''Session("CodiceRegioneEnte") = "[" & dgRisultatoRicerca1.Rows.Item(0).Cells(1).Text & "]"
        ''Session("txtCodEnte") = dgRisultatoRicerca1.Rows.Item(0).Cells(1).Text
        'Response.Redirect("WfrmMain.aspx?regione=" & Session("Competenza"))


        'NomeColonne(0) = "Cod.Ente"
        'NomeColonne(1) = "Denominazione"
        'NomeColonne(2) = "Stato"
        'NomeColonne(3) = "Tipologia"
        'NomeColonne(4) = "Classe Richiesta"
        'NomeColonne(5) = "Classe Attribuita"
        'NomeColonne(6) = "Nr.Sedi Attuazione"
        'NomeColonne(7) = "http://"
        'NomeColonne(8) = "Email"
        'NomeColonne(9) = "PEC"
        'NomeColonne(10) = "Telefono"
        'NomeColonne(11) = "Fax"
        'NomeColonne(12) = "Competenza"
        'NomeColonne(13) = "ID Ente"
        'NomeColonne(14) = "Classe Accreditamento Richiesta"

        Dim dt As New DataTable
        dt = Session("DtbRicerca")
       
        Session("IdEnte") = Convert.ToInt32(e.CommandArgument).ToString()

        Dim indiceTotale As Integer
        indiceTotale = dt.Rows.Count
        Dim i As Integer

        'Dim x As New row
        For i = 0 To indiceTotale

            If dt.Rows(i).Item("ID Ente").ToString = e.CommandArgument.ToString() Then


                Exit For

            End If

        Next
        '--------------------------------------------------------------Competenza-----------------------------------Classe Richiesta
        Dim ClasseAccreditamentoRichiesta As Integer

        ClasseAccreditamentoRichiesta = dt.Rows(i).Item("Sezione Iscrizione Richiesta").ToString
        If ClasseAccreditamentoRichiesta = 5 Or ClasseAccreditamentoRichiesta = 7 Then
            Session("IdEnte") = Nothing
            Exit Sub
        Else
            'Session("IdEnte") = dgRisultatoRicerca.SelectedItem.Cells(10).Text
            'Session("Ente") = dgRisultatoRicerca.SelectedItem.Cells(1).Text
            Session("Denominazione") = dt.Rows(i).Item("Denominazione").ToString
            Session("Competenza") = dt.Rows(i).Item("Competenza")
            Session("CodiceRegioneEnte") = "[" & dt.Rows(i).Item("Cod.Ente").ToString & "]"
            Session("txtCodEnte") = dt.Rows(i).Item("Cod.Ente").ToString
            Response.Redirect("WfrmMain.aspx?regione=" & (Session("Competenza")))

        End If
        '--------------------------------------------------------------------------------------------------
    End Sub
End Class