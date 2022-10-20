Imports System.Drawing
Imports System.IO

Public Class WfrmRicEnteinAccordo
    Inherits System.Web.UI.Page

    Dim strquery As String  'stringa sql generica
    Dim dtsGenerico As DataSet 'dataset generico

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim Albo As String
        '***Generata da Gianluigi Paesani in data:04/05/04
        '***Inizializzo combo con dati per ricerche
        'Inserire qui il codice utente necessario per inizializzare la pagina
        If Not Session("LogIn") Is Nothing Then 'controlli formali per la session utente
            If Session("LogIn") = False Then
                Response.Redirect("LogOn.aspx")
            End If
        Else
            Response.Redirect("LogOn.aspx")
        End If

        lblControlloClasse.Visible = False
        lblControlloClasse.Text = String.Empty

        If Page.IsPostBack = False Then
            Albo = ClsUtility.TrovaAlboEnte(Session("idEnte"), IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))

            If Albo = "SCN" Then
                LblRiserva.Visible = False
                ddlRiserva.Visible = False
            Else
                If Session("TipoUtente") = "U" Then
                    LblRiserva.Visible = True
                    ddlRiserva.Visible = True
                Else
                    LblRiserva.Visible = False
                    ddlRiserva.Visible = False
                End If

            End If

            If Request.QueryString("esporta") <> "si" Then

                phStatoAccreditamento.Visible = True
            Else

                phStatoAccreditamento.Visible = False
            End If

            'visibilità filtroFase
            If Session("TipoUtente") = "U" Then
                lblFiltroFase.Visible = True
                txtFiltroFase.Visible = True
            Else
                lblFiltroFase.Visible = False
                txtFiltroFase.Visible = False
            End If

            Dim dtrgenerico As Data.SqlClient.SqlDataReader 'datareader generico

            dtrgenerico = ClsServer.CreaDatareader("select a.idclasseaccreditamento from enti a" & _
                            " inner join classiaccreditamento b" & _
                            " on a.idclasseaccreditamento=b.idclasseaccreditamento" & _
                            " inner join classiaccreditamento c" & _
                            " on a.idclasseaccreditamentorichiesta=c.idclasseaccreditamento" & _
                            " where idente=" & Session("idEnte") & "" & _
                            " and (b.minsedi > 0 or c.minsedi>0)", IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))

            If dtrgenerico.HasRows = False Then
                If (Session("TipoUtente") = "U" Or Session("TipoUtente") = "R") Then
                    lblControlloClasse.Text = "Attenzione l'Ente selezionato non può accedere alla gestione [ Accordo tra Enti ]"
                    lblControlloClasse.Visible = True
                    cmdRicerca.Visible = False
                Else
                    lblControlloClasse.Text = "Attenzione l'Ente non può accedere alla gestione [ Accordo tra Enti ]"
                    lblControlloClasse.Visible = True
                    cmdRicerca.Visible = False
                End If

            End If
            dtrgenerico.Close()
            dtrgenerico = Nothing

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
            Dim myCommand As New Data.SqlClient.SqlCommand(strquery, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
            dtrgenerico = myCommand.ExecuteReader()
            ddlCAccreditamento.Items.Add("")
            Do While dtrgenerico.Read()
                ddlCAccreditamento.Items.Add(dtrgenerico.GetValue(0))
            Loop
            dtrgenerico.Close()
            dtrgenerico = Nothing

            'Aggiunto da Alessandra TAballione il 09/03/2005
            ddlstato.Items.Add("")
            ddlstato.Items.Add("Attivo")
            ddlstato.Items.Add("Annullato")

            'Caricamento della combo per lo stato degli accordi
            CboStatoEnte.DataSource = ClsServer.CreaDataTable("Select IdStatoEnte,StatoEnte From StatiEnti " & _
                                                              "WHERE PresentazioneProgetti = 1 OR Sospeso = 1 OR " & _
                                                              "(PresentazioneProgetti = 0 AND DefaultStato = 0 AND Chiuso = 0 AND Sospeso = 0 AND Istruttoria = 0)", True, Session("Conn"))
            CboStatoEnte.DataValueField = "IdStatoEnte"
            CboStatoEnte.DataTextField = "StatoEnte"
            CboStatoEnte.DataBind()

            'controllo se vengo dalla maschera di ricerca delle sedi
            If Request.QueryString("CheckProvenienza") = "RicercaEnteInAccordo" Then
                txtdenominazione.Text = Request.QueryString("Denominazione")
                txtCodRegione.Text = Request.QueryString("CodiceRegione")
                TxtCodiceFiscale.Text = Request.QueryString("CF")
                ddltipologia.SelectedValue = Request.QueryString("Tipologia")
                ddlCAccreditamento.SelectedValue = Request.QueryString("ClasseAccreditamento")
                ddlstato.SelectedValue = Request.QueryString("Stato")
                PopolaGriglia(1, Request.QueryString("Pagina"))
            End If

        End If
        If dgRicercaEnte.Items.Count = 0 Then
            CmdEsporta.Visible = False
        Else
            CmdEsporta.Visible = True
        End If

    End Sub

    Function checkFiltroFase() As Boolean
        lblErroreFiltroFase.Visible = False
        If Not String.IsNullOrEmpty(txtFiltroFase.Text) AndAlso Not Integer.TryParse(txtFiltroFase.Text, Nothing) Then
            lblErroreFiltroFase.Visible = True
            Return False
        End If
        Return True
    End Function

    Private Sub PopolaGriglia(ByVal bytVerifica As Byte, Optional ByVal bytpage As Integer = 0)
        '***Generata da Gianluigi Paesani in data:04/05/04
        '***Questa routine esegue la ricerca degl'enti se inserimento o modifica

        If Not checkFiltroFase() Then Exit Sub

        'se successivo o precedente setto pagina di arrivo a seconda del parametro
        If bytVerifica = 1 Then dgRicercaEnte.CurrentPageIndex = bytpage

        Dim strappo As String 'variabile appoggio generica
        Dim totsedi As String
        'Al primo controllo verifico se inserimento per tirarmi fuori tutti gl' enti che non sono relazionati
        'Sono in modifica e cerco di tirarmi fuori tutti gl'enti che sono relazionati con me(Utente) sia attivi che annullati
        'imposto colonne DG 
        dgRicercaEnte.Columns(3).Visible = True
        dgRicercaEnte.Columns(10).Visible = True
        If txtdenominazione.Text = "" And TxtCodiceFiscale.Text = "" And ddlCAccreditamento.SelectedItem.Text = "" And ddltipologia.SelectedItem.Text = "" Then
            strquery = "select a.identefiglio as idente, case when len (b.denominazione)>200 then left(b.denominazione,200) + '...' else b.denominazione end as denominazione,b.tipologia,d.classeaccreditamento," & _
            " c.tiporelazione, b.http, case when len(b.email)>10 then left(b.email,10) + '...' else b.email end as email,b.CodiceRegione,a.DataInizioValidità as DataInizioValidita," & _
            "(SELECT '<a href=""WfrmRicercaSede.aspx?IdEnteFiglio=' + Convert(varchar(20), a.identefiglio) + '&codiceente=' + isnull(b.CodiceRegione,'') +  '&Pagina=" & dgRicercaEnte.CurrentPageIndex & "&CheckProvenienza=RicercaEnteInAccordo&Stato=" & ddlstato.SelectedValue & "&CF=" & Replace(TxtCodiceFiscale.Text, "'", "''") & "&ClasseAccreditamento=" & ddlCAccreditamento.SelectedValue & "&Tipologia=" & ddltipologia.SelectedValue & "&CodiceRegione=" & Replace(txtCodRegione.Text, "'", "''") & "&Denominazione=" & Replace(txtdenominazione.Text, "'", "''") & "&VediEnte=1&DenominazioneEnte=' + replace(b.denominazione,'""','%') + '"">' + convert(varchar, COUNT(*)) + '</a>' " & _
            " FROM entisedi " & _
            " INNER JOIN entisediattuazioni ON entisedi.IDEnteSede = entisediattuazioni.IDEnteSede  " & _
            " INNER JOIN StatiEntiSedi ON entisedi.IDStatoEnteSede = StatiEntiSedi.IdStatoEnteSede  " & _
            " INNER JOIN StatiEntiSedi StatiEntiSedi_1 ON entisediattuazioni.IdStatoEnteSede = StatiEntiSedi_1.IdStatoEnteSede  " & _
            " INNER JOIN entirelazioni ON entisedi.IDEnte = entirelazioni.IDEnteFiglio   " & _
            " INNER JOIN AssociaEntiRelazioniSediAttuazioni  " & _
            " ON entisediattuazioni.IDEnteSedeAttuazione = AssociaEntiRelazioniSediAttuazioni.IdEnteSedeAttuazione " & _
            " AND  entirelazioni.IDEnteRelazione = AssociaEntiRelazioniSediAttuazioni.IdEnteRelazione  " & _
            " WHERE(StatiEntiSedi.Attiva = 1 Or StatiEntiSedi.DaAccreditare = 1)" & _
            " AND (StatiEntiSedi_1.Attiva = 1 or  StatiEntiSedi_1.DaAccreditare = 1) " & _
            " AND (entirelazioni.IDEntefiglio = a.identefiglio) AND " & _
            " (GETDATE() BETWEEN  ISNULL(entirelazioni.DataInizioValidità, '2000-01-01') " & _
            " AND ISNULL(entirelazioni.DataFineValidità,'2030-01-01'))) as numerototalesedi, " & _
            "(SELECT convert(varchar, COUNT(*)) " & _
            " FROM entisedi " & _
            " INNER JOIN entisediattuazioni ON entisedi.IDEnteSede = entisediattuazioni.IDEnteSede  " & _
            " INNER JOIN StatiEntiSedi ON entisedi.IDStatoEnteSede = StatiEntiSedi.IdStatoEnteSede  " & _
            " INNER JOIN StatiEntiSedi StatiEntiSedi_1 ON entisediattuazioni.IdStatoEnteSede = StatiEntiSedi_1.IdStatoEnteSede  " & _
            " INNER JOIN entirelazioni ON entisedi.IDEnte = entirelazioni.IDEnteFiglio   " & _
            " INNER JOIN AssociaEntiRelazioniSediAttuazioni  " & _
            " ON entisediattuazioni.IDEnteSedeAttuazione = AssociaEntiRelazioniSediAttuazioni.IdEnteSedeAttuazione " & _
            " AND  entirelazioni.IDEnteRelazione = AssociaEntiRelazioniSediAttuazioni.IdEnteRelazione  " & _
            " WHERE(StatiEntiSedi.Attiva = 1 Or StatiEntiSedi.DaAccreditare = 1)" & _
            " AND (StatiEntiSedi_1.Attiva = 1 or  StatiEntiSedi_1.DaAccreditare = 1) " & _
            " AND (entirelazioni.IDEntefiglio = a.identefiglio) AND " & _
            " (GETDATE() BETWEEN  ISNULL(entirelazioni.DataInizioValidità, '2000-01-01') " & _
            " AND ISNULL(entirelazioni.DataFineValidità,'2030-01-01'))) as numerototalesedi2, " & _
            " case when a.datafinevalidità is null then 'Attivo'" & _
            " when a.datafinevalidità is not null then 'Annullato' end as Stato,a.identerelazione,statienti.idstatoente," & _
            " case b.Riserva when 0 then statienti.statoente else statienti.statoente + ' (' + 'Ris.' + ')' end AS statoente " & _
            " from entirelazioni a " & _
            " inner join enti b on b.idente=a.identefiglio" & _
            " inner join statienti on statienti.idstatoente=b.idstatoente" & _
            " inner join tipirelazioni c on a.idtiporelazione=c.idtiporelazione" & _
            " inner join  classiaccreditamento d on b.idclasseaccreditamentorichiesta=d.idclasseaccreditamento"

            'la validità del FiltroFase è stata controllata prima
            If Not String.IsNullOrEmpty(txtFiltroFase.Text) Then
                strquery = strquery & " left join EntiFasi_Enti efe on efe.IdEnte=a.identefiglio "
            End If

            strquery = strquery & " where EntiInPartenariato=0 and Defaultclasse=0 and a.identepadre=" & Session("idEnte") & ""
        Else
            strquery = "select a.identefiglio as idente, case when len (b.denominazione)>200 then left(b.denominazione,200) + '...' else b.denominazione end as denominazione,b.tipologia,d.classeaccreditamento," & _
            " c.tiporelazione, b.http,case when len(b.email)>10 then left(b.email,10) + '...' else b.email end as email,b.CodiceRegione,a.DataInizioValidità as DataInizioValidita," & _
            "(SELECT '<a href=""WfrmRicercaSede.aspx?IdEnteFiglio=' + Convert(varchar(20), a.identefiglio) + '&Pagina=" & dgRicercaEnte.CurrentPageIndex & "&CheckProvenienza=RicercaEnteInAccordo&Stato=" & ddlstato.SelectedValue & "&CF=" & Replace(TxtCodiceFiscale.Text, "'", "''") & "&ClasseAccreditamento=" & ddlCAccreditamento.SelectedValue & "&Tipologia=" & ddltipologia.SelectedValue & "&CodiceRegione=" & Replace(txtCodRegione.Text, "'", "''") & "&Denominazione=" & Replace(txtdenominazione.Text, "'", "''") & "&VediEnte=1&DenominazioneEnte=' + replace(b.denominazione,'""','%') + '"">' + convert(varchar, COUNT(*)) + '</a>' " & _
            " FROM entisedi " & _
            " INNER JOIN entisediattuazioni ON entisedi.IDEnteSede = entisediattuazioni.IDEnteSede  " & _
            " INNER JOIN StatiEntiSedi ON entisedi.IDStatoEnteSede = StatiEntiSedi.IdStatoEnteSede  " & _
            " INNER JOIN StatiEntiSedi StatiEntiSedi_1 ON entisediattuazioni.IdStatoEnteSede = StatiEntiSedi_1.IdStatoEnteSede  " & _
            " INNER JOIN entirelazioni ON entisedi.IDEnte = entirelazioni.IDEnteFiglio   " & _
            " INNER JOIN AssociaEntiRelazioniSediAttuazioni  " & _
            " ON entisediattuazioni.IDEnteSedeAttuazione = AssociaEntiRelazioniSediAttuazioni.IdEnteSedeAttuazione " & _
            " AND  entirelazioni.IDEnteRelazione = AssociaEntiRelazioniSediAttuazioni.IdEnteRelazione  " & _
            " WHERE(StatiEntiSedi.Attiva = 1 Or StatiEntiSedi.DaAccreditare = 1)" & _
            " AND (StatiEntiSedi_1.Attiva = 1 or  StatiEntiSedi_1.DaAccreditare = 1) " & _
            " AND (entirelazioni.IDEntefiglio = a.identefiglio) AND " & _
            " (GETDATE() BETWEEN  ISNULL(entirelazioni.DataInizioValidità, '2000-01-01') " & _
            " AND ISNULL(entirelazioni.DataFineValidità,'2030-01-01'))) as numerototalesedi, " & _
            "(SELECT convert(varchar, COUNT(*)) " & _
            " FROM entisedi " & _
            " INNER JOIN entisediattuazioni ON entisedi.IDEnteSede = entisediattuazioni.IDEnteSede  " & _
            " INNER JOIN StatiEntiSedi ON entisedi.IDStatoEnteSede = StatiEntiSedi.IdStatoEnteSede  " & _
            " INNER JOIN StatiEntiSedi StatiEntiSedi_1 ON entisediattuazioni.IdStatoEnteSede = StatiEntiSedi_1.IdStatoEnteSede  " & _
            " INNER JOIN entirelazioni ON entisedi.IDEnte = entirelazioni.IDEnteFiglio   " & _
            " INNER JOIN AssociaEntiRelazioniSediAttuazioni  " & _
            " ON entisediattuazioni.IDEnteSedeAttuazione = AssociaEntiRelazioniSediAttuazioni.IdEnteSedeAttuazione " & _
            " AND  entirelazioni.IDEnteRelazione = AssociaEntiRelazioniSediAttuazioni.IdEnteRelazione  " & _
            " WHERE(StatiEntiSedi.Attiva = 1 Or StatiEntiSedi.DaAccreditare = 1)" & _
            " AND (StatiEntiSedi_1.Attiva = 1 or  StatiEntiSedi_1.DaAccreditare = 1) " & _
            " AND (entirelazioni.IDEntefiglio = a.identefiglio) AND " & _
            " (GETDATE() BETWEEN  ISNULL(entirelazioni.DataInizioValidità, '2000-01-01') " & _
            " AND ISNULL(entirelazioni.DataFineValidità,'2030-01-01'))) as numerototalesedi2, " & _
            " case when a.datafinevalidità is null then 'Attivo'" & _
            " when a.datafinevalidità is not null then 'Annullato' end as Stato, a.identerelazione,statienti.idstatoente, " & _
            " case b.Riserva when 0 then statienti.statoente else statienti.statoente + ' (' + 'Ris.' + ')' end AS statoente " & _
            " from entirelazioni a " & _
            " inner join enti b on b.idente=a.identefiglio" & _
            " inner join statienti on statienti.idstatoente=b.idstatoente" & _
            " inner join tipirelazioni c on a.idtiporelazione=c.idtiporelazione" & _
            " inner join  classiaccreditamento d on b.idclasseaccreditamentorichiesta=d.idclasseaccreditamento"

            'la validità del FiltroFase è stata controllata prima
            If Not String.IsNullOrEmpty(txtFiltroFase.Text) Then
                strquery = strquery & " left join EntiFasi_Enti efe on efe.IdEnte=a.identefiglio "
            End If

            strquery = strquery & " where EntiInPartenariato=0 and Defaultclasse=0 and a.identepadre=" & Session("idEnte") & ""
        End If
        'imposto eventuali parametri nella query
        If txtdenominazione.Text <> "" Then
            strquery = strquery & " and b.denominazione like '" & ClsServer.NoApice(txtdenominazione.Text) & "%'"
        End If
        If txtCodRegione.Text <> "" Then
            strquery = strquery & " and b.CodiceRegione = '" & ClsServer.NoApice(txtCodRegione.Text) & "'"
        End If
        If ddltipologia.SelectedItem.Text <> "" Then
            strquery = strquery & " and b.tipologia='" & ClsServer.NoApice(ddltipologia.SelectedItem.Text) & "'"
        End If
        If ddlCAccreditamento.SelectedItem.Text <> "" Then
            strquery = strquery & " and d.ClasseAccreditamento='" & ClsServer.NoApice(ddlCAccreditamento.SelectedItem.Text) & "'"
        End If

        If TxtCodiceFiscale.Text <> "" Then
            strquery = strquery & " and b.CodiceFiscale = '" & ClsServer.NoApice(TxtCodiceFiscale.Text) & "'"
        End If
        If Request.QueryString("esporta") = "si" Then
            strquery = strquery & " and a.datafinevalidità is null "
        Else
            If ddlstato.SelectedItem.Text = "Attivo" Then
                strquery = strquery & " and a.datafinevalidità is null "
            ElseIf ddlstato.SelectedItem.Text = "Annullato" Then
                strquery = strquery & " and not a.datafinevalidità is null "
            End If
        End If

        If CboStatoEnte.SelectedItem.Text <> "" Then
            strquery = strquery & " And b.IdStatoEnte = " & CboStatoEnte.SelectedValue
        End If

        If TxtDataInserimentoDal.Text <> "" And TxtDataInserimentoAl.Text <> "" Then
            strquery = strquery & " And DataInizioValidità BETWEEN Convert(Datetime,'" & TxtDataInserimentoDal.Text & "',103) And Convert(Datetime,'" & TxtDataInserimentoAl.Text & "',103) "
        ElseIf TxtDataInserimentoDal.Text <> "" And TxtDataInserimentoAl.Text = "" Then
            strquery = strquery & " And DataInizioValidità >= Convert(Datetime,'" & TxtDataInserimentoDal.Text & "',103) "
        End If
        'filtro per RichiestaVariazione
        If ddlRichistaVariazione.SelectedValue <> "" Then
            strquery = strquery & " and isnull(b.RichiestaModifica,0)=" & ddlRichistaVariazione.SelectedValue & " "
        End If
        'aggiunto il 20/12/2017 filtro RISERVA
        If ddlRiserva.SelectedItem.Text <> "Tutti" Then
            strquery = strquery & " and isnull(b.Riserva,0) = " & ddlRiserva.SelectedValue
        End If

        'la validità del FiltroFase è stata controllata prima
        If Not String.IsNullOrEmpty(txtFiltroFase.Text) Then
            strquery = strquery & " and efe.IdEnteFase =" & Trim(txtFiltroFase.Text)
        End If

        '***         strquery = strquery & " order by denominazione"
        'eseguo query e valorizzo oggetto datagrid
        dtsGenerico = ClsServer.DataSetGenerico(strquery, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
        strappo = dgRicercaEnte.CurrentPageIndex
        dgRicercaEnte.DataSource = dtsGenerico
        dgRicercaEnte.DataBind()
        dgRicercaEnte.Visible = True
        ColoraCelle()
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

    Private Sub ColoraCelle()
        'Generato da Alessandra Taballione il 22/07/2005
        'VAriazione del Colore secondo lo stato dello stato.
        'Attivo=Verde;Registato=gialla;Cancellata=Rossa;Sospesa=Rosso
        Dim item As DataGridItem
        Dim intConta As Integer
        For Each item In dgRicercaEnte.Items
            If dgRicercaEnte.Items(item.ItemIndex).Cells(10).Text = "Annullato" Then
                For intConta = 0 To 14
                    dgRicercaEnte.Items(item.ItemIndex).Cells(intConta).BackColor = Color.LightSalmon
                    'dgRisultatoRicerca.Items(item.ItemIndex).Cells(intConta).ForeColor = Color.Red
                Next
            Else
                Select Case dgRicercaEnte.Items(item.ItemIndex).Cells(13).Text
                    Case "3" 'Attivo
                        For intConta = 0 To 14
                            dgRicercaEnte.Items(item.ItemIndex).Cells(intConta).BackColor = Color.LightGreen
                            'dgRisultatoRicerca.Items(item.ItemIndex).Cells(intConta).ForeColor = Color.LightGreen
                        Next
                    Case "6" 'registrato
                        For intConta = 0 To 14
                            dgRicercaEnte.Items(item.ItemIndex).Cells(intConta).BackColor = Color.Khaki
                        Next
                    Case "7" 'Chiuso
                        For intConta = 0 To 14
                            dgRicercaEnte.Items(item.ItemIndex).Cells(intConta).BackColor = Color.LightSalmon
                            'dgRisultatoRicerca.Items(item.ItemIndex).Cells(intConta).ForeColor = Color.Red
                        Next
                End Select
            End If

        Next
    End Sub

    Private Sub PreparaStampa()
        '*********************************************************************************
        'blocco per la creazione della datatable per la stampa della ricerca

        'nome e posizione di lettura delle colopnne a base 0
        Dim NomeColonne(10) As String
        Dim NomiCampiColonne(10) As String
        'nome della colonna 
        'e posizione nella griglia di lettura
        NomeColonne(0) = "Cod. Ente"
        NomeColonne(1) = "Denominazione"
        NomeColonne(2) = "Tipo di Relazione"
        NomeColonne(3) = "Tipologia"
        NomeColonne(4) = "Classe/Sezione"
        NomeColonne(5) = "Http"
        NomeColonne(6) = "E-Mail"
        NomeColonne(7) = "Totale Sedi"
        NomeColonne(8) = "Stato Accordo"
        NomeColonne(9) = "Stato Ente"
        NomeColonne(10) = "Data Inserimento"
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
        'NomiCampiColonne(11) = "numerototalesedi2"
        'carico un datatable che userò poi nella pagina di stampa
        'il numero delle colonne è a base 0
        CaricaDataTablePerStampa(dtsGenerico, 10, NomeColonne, NomiCampiColonne)

        '*********************************************************************************

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

    Private Sub cmdChiudi_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdChiudi.Click
        '***Generata da Gianluigi Paesani in data:04/05/04
        'richiamo homepage se esco
        'controllo se vengo dall'albero in gestione enti
        If Not Request.QueryString("VengoDa") Is Nothing Then
            'controllo se la variabile è valorizzata
            If Request.QueryString("VengoDa") <> "" Then
                'faccio la response.redirect verso l'albero
                Response.Redirect(Request.QueryString("VengoDa").ToString)
            End If
        End If
        Response.Redirect("WfrmMain.aspx")
    End Sub

    Private Sub cmdRicerca_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdRicerca.Click

        If ValidazioneServerRicerca() = True Then
            '***Generata da Gianluigi Paesani in data:04/05/04
            '***Questa routine richiama procedura per ricerca
            dgRicercaEnte.CurrentPageIndex = 0
            PopolaGriglia(0)
        End If

    End Sub

    Private Sub dgRicercaEnte_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgRicercaEnte.SelectedIndexChanged
        '***Generata da Gianluigi Paesani in data:04/05/04
        'questa routine verifica il parametro (per l'apertura della web 
        'form di gestione) dopo la selezione dell'ente per capire se è in inserimento o modifica
        Dim strAzione As String
        If Not dgRicercaEnte.SelectedItem Is Nothing Then

            If Request.QueryString("azione") = "Ins" Then
                'strAzione = "Ins"
                Response.Redirect("WfrmGestioneEnteinAccordo.aspx?azione=Ins&id=" & dgRicercaEnte.SelectedItem.Cells(9).Text)
            Else
                Dim strstato As String = "" & dgRicercaEnte.SelectedItem.Cells(10).Text
                'strAzione = "Mod"
                'Response.Redirect("WfrmGestioneEnteinAccordo.aspx?azione=Mod&id=" & dgRicercaEnte.SelectedItem.Cells(9).Text & "&Stato=" & strstato)
                Response.Redirect("WfrmAnagraficaEnteAccordo.aspx?azione=Mod&id=" & dgRicercaEnte.SelectedItem.Cells(9).Text & "&identerelazione=" & dgRicercaEnte.SelectedItem.Cells(11).Text & "&Stato=" & strstato)
            End If
        End If
    End Sub

    Private Sub dgRicercaEnte_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgRicercaEnte.PageIndexChanged
        '***Generata da Gianluigi Paesani in data:04/05/04
        '***Questa routine richiama funzione per pagina successiva o precedente
        If Not checkFiltroFase() Then Exit Sub
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

    Function ValidazioneServerRicerca() As Boolean

        If (TxtDataInserimentoDal.Text.Trim = String.Empty And TxtDataInserimentoAl.Text.Trim <> String.Empty) Then
            lblControlloClasse.Visible = True
            lblControlloClasse.Text = "Inserire la data inizio del range per la ricerca"
            Return False
        End If

        If TxtDataInserimentoDal.Text.Trim <> String.Empty Then
            Dim dataInserimentoDal As Date
            If (Date.TryParse(TxtDataInserimentoDal.Text, dataInserimentoDal) = False) Then
                lblControlloClasse.Visible = True
                lblControlloClasse.Text = "La 'Data Inserimento Accordo Dal' non è valida. Inserire la data nel formato GG/MM/AAAA."
                Return False
            End If
        End If
        
        If TxtDataInserimentoAl.Text.Trim <> String.Empty Then
            Dim dataInserimentoAl As Date
            If (Date.TryParse(TxtDataInserimentoAl.Text, dataInserimentoAl) = False) Then
                lblControlloClasse.Visible = True
                lblControlloClasse.Text = "La 'Data Inserimento Accordo Al' non è valida. Inserire la data nel formato GG/MM/AAAA."
                Return False
            End If
        End If


        If (TxtDataInserimentoDal.Text.Trim <> String.Empty And TxtDataInserimentoAl.Text.Trim <> String.Empty) Then
            Dim arrDateFrom As String()
            Dim arrDateTo As String()
            Dim dtFrom As Date
            Dim dtTo As Date

            arrDateFrom = TxtDataInserimentoDal.Text.Trim.Split("/")
            arrDateTo = TxtDataInserimentoAl.Text.Trim.Split("/")

            dtFrom = New Date(arrDateFrom(2), arrDateFrom(1), arrDateFrom(0))  'Year, Month, Date
            dtTo = New Date(arrDateTo(2), arrDateTo(1), arrDateTo(0)) 'Year, Month, Date

            If dtFrom > dtTo Then
                lblControlloClasse.Visible = True
                lblControlloClasse.Text = "La data di inizio e' maggiore della data finale"
                Return False
            End If
          
        End If

        Return True

    End Function


End Class