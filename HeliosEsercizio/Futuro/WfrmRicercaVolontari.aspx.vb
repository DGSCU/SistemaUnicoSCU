Imports System.Data.SqlClient
Imports System.Drawing
Imports System.IO

Public Class WfrmRicercaVolontari
    Inherits System.Web.UI.Page
    Public dtsRisRicerca As DataSet

#Region " Codice generato da Progettazione Web Form "

    'Chiamata richiesta da Progettazione Web Form.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents canc As System.Web.UI.WebControls.CheckBox

    'NOTA: la seguente dichiarazione è richiesta da Progettazione Web Form.
    'Non spostarla o rimuoverla.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: questa chiamata al metodo è richiesta da Progettazione Web Form.
        'Non modificarla nell'editor del codice.
        InitializeComponent()
    End Sub

#End Region

#Region "Commento generazione"
    'routine e codice generato e chiuso da Bagnani Jonathan il 21/04/2004 (ricerca)
#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load
        'DESCRIZIONE:controllo se si tratta di una ricerca, controllo necessario per far si che non si perde il pageindex
        'AUTORE: TESTA GUIDO    DATA: 14/10/2004        
        'controllo login effettuato
        If Not Session("LogIn") Is Nothing Then
            If Session("LogIn") = False Then
                Response.Redirect("LogOn.aspx")
            End If
        Else
            Response.Redirect("LogOn.aspx")
        End If

        If Page.IsPostBack = False Then
            Call CaricaCombo()
            If (Request.Params("Ente") <> "OK") Then
                DivCampiRicercaEnte.Visible = True
            Else
                DivCampiRicercaEnte.Visible = False
            End If
            If Request.QueryString("Paghe") = 1 Then
                If ClsUtility.ForzaCaricamentoPaghe(Session("Utente"), Session("conn")) = True Then
                    filtripaghe.Visible = True
                Else
                    Response.Redirect("wfrmAnomaliaDati.aspx")
                End If

            Else
                filtripaghe.Visible = False
            End If

        End If
        If dtgRisultatoRicerca.Items.Count > 0 Then
            CmdEsporta.Visible = True
        Else
            CmdEsporta.Visible = False
        End If

    End Sub

    'DESCRIZIONE: routine che carica la griglia delle risorse al primo caricamento della pagina a seconda dell'ente loggato
    'AUTORE: TESTA GUIDO    DATA: 14/10/2004
    'REVISIONATO DA: MICHELE D'ASCENZIO     DATA: 10/12/2004
    'MODIFICATO DA: AMILCARE PAOLELLA  DATA: 5/8/2005
    Sub CaricaGriglia()
        dtgRisultatoRicerca.CurrentPageIndex = 0
        Dim strSQL As String
        Dim strCondizione As String
        'preparo la query sulle risorse relative all'ente loggato Acquisito e Non
        strSQL = "SELECT DISTINCT " & _
                 "Entità.IDEntità AS IDEntità, " & _
                 "case isnull(entità.datainiziointerruzione,'01/01/1900') when '01/01/1900' then '' else 'Interruzione Temporanea ' END + isnull(Entità.AltreInformazioni,'') + case isnull(entità.notestato,'')when '' then '' " & _
                 " else ' note chiusura: ' end +  isnull(entità.notestato,'') AS AltreInfo,  " & _
                 "isnull(rtrim(ltrim(Entità.CodiceLibrettoPostale)),'ASSENTE') AS CLP, " & _
                 "Attività.IDAttività AS IdAttività, " & _
                 "Entità.Cognome + ' ' + Entità.Nome AS Nominativo, " & _
                 "Comuni.Denominazione + ' (' + isnull(Provincie.DescrAbb,Provincie.Provincia) + ') ' AS ComProv, " & _
                 "Comuni.Denominazione AS Comune, " & _
                 "Provincie.Provincia AS Provincia, " & _
                 "CASE Entità.Sesso WHEN 0 THEN 'UOMO' WHEN 1 THEN 'DONNA' END AS Sesso, " & _
                 "CASE Entità.Abilitato	WHEN 0 THEN 'NO' WHEN 1 THEN 'SI'  END AS Abilitato, " & _
                 "COUNT(Attività.Titolo) as NumeroAttivita, " & _
                 "CONVERT(varchar, Entità.DataInizioServizio, 103)as DataInizio, " & _
                 "CONVERT(varchar, Entità.DataFineServizio, 103) as DataFine, " & _
                 "StatiEntità.StatoEntità as Stato, " & _
                 "enti.CodiceRegione, " & _
                 "Enti.IdEnte as IdEnte, " & _
                 "Enti.Denominazione + ' (' + isnull(attività.CodiceEnte,'') + ') ' as Ente ,entità.codicevolontario, " & _
                 "Enti.Denominazione as Ente1,Entità.codicefiscale as cf, isnull(rtrim(ltrim(Entità.iban)),'ASSENTE')as iban "

        'aggiungo il pulsante per vedere la maschera delle cronologie solo per i tipo utente U e R
        'If (Session("TipoUtente") = "U" Or Session("TipoUtente") = "R") Then
        '    strSQL = strSQL & ",'<img src='Images/Cronologia.gif' onClientclick='VisualizzaAtt(' + convert(varchar,Entità.IDEntità) + ')' STYLE='cursor:hand;' alt='Seleziona Nominativo' title='Seleziona Nominativo' /> ' as Crono "
        'Else
        strSQL = strSQL & ",'' as Crono ,bando.Bando "
        'End If

        strSQL = strSQL & "FROM AttivitàEntiSediAttuazione " & _
                 "INNER JOIN AttivitàEntità ON AttivitàEntiSediAttuazione.IDAttivitàEnteSedeAttuazione = AttivitàEntità.IDAttivitàEnteSedeAttuazione " & _
                 "RIGHT OUTER JOIN Entità ON Attivitàentità.IDEntità = Entità.IDEntità " & _
                 "INNER JOIN StatiEntità ON Entità.IDStatoEntità = StatiEntità.IDStatoEntità " & _
                 "INNER JOIN GRADUATORIEENTITà on graduatorieentità.identità = entità.identità " & _
                 "INNER JOIN attivitàsediassegnazione on attivitàsediassegnazione.idattivitàsedeassegnazione = graduatorieentità.idattivitàsedeassegnazione " & _
                 "INNER JOIN Entisedi on attivitàsediassegnazione.identesede = entisedi.identesede " & _
                 "INNER JOIN Comuni ON Entisedi.IDComune = Comuni.IDComune " & _
                 "INNER JOIN Provincie ON Comuni.IDProvincia = Provincie.IDProvincia " & _
                 "INNER JOIN Regioni ON Provincie.IDRegione = Regioni.IDRegione " & _
                 "inner join attività on attività.idattività = attivitàsediassegnazione.idattività " & _
                 "INNER JOIN BandiAttività on BandiAttività.IdBandoAttività=attività.IDBandoAttività " & _
                 "INNER JOIN Bando on Bando.IdBando=BandiAttività.IDBando " & _
                 "inner join enti on attività.identepresentante = enti.idente " & _
                 "INNER JOIN TIPIPROGETTO ON attività.idtipoprogetto=TIPIPROGETTO.idtipoprogetto " & _
                 "LEFT JOIN StatiVerificaCFEntità ON Entità.IDStatiVerificaCFEntità = StatiVerificaCFEntità.IDStatiVerificaCFEntità "
        '"INNER JOIN Attività ON AttivitàEntiSediAttuazione.IDAttività = Attività.IDAttività " & _
        '"INNER JOIN Enti ON Attività.IDEntePresentante = Enti.IDEnte " & _
        strCondizione = "WHERE "

        'se viene selezionato l'ente faccio il filtro su di lui
        If Request.Params("Ente") = "OK" Then
            strSQL = strSQL & strCondizione & "Attività.IDEntePresentante = " & Session("IdEnte") & " "
            strCondizione = "AND "
        End If
        'imposto le condizioni dinamicamente
        If cboSesso.SelectedIndex >= 1 Then
            strSQL = strSQL & strCondizione & "Sesso = '" & cboSesso.Items(cboSesso.SelectedIndex).Value.ToString & "' "
            strCondizione = "AND "
        End If
        If cboStato.SelectedIndex >= 1 Then
            strSQL = strSQL & strCondizione & "Entità.IDStatoEntità = '" & cboStato.Items(cboStato.SelectedIndex).Value.ToString & "' "
            strCondizione = "AND "
        End If
        If txtCognome.Text <> vbNullString Then
            strSQL = strSQL & strCondizione & "Cognome LIKE '" & Replace(txtCognome.Text, "'", "''") & "%' "
            strCondizione = "AND "
        End If
        If txtNome.Text <> vbNullString Then
            strSQL = strSQL & strCondizione & "Nome LIKE '" & Replace(txtNome.Text, "'", "''") & "%' "
            strCondizione = "AND "
        End If
        If txtRegione.Text <> vbNullString Then
            strSQL = strSQL & strCondizione & "Regioni.Regione LIKE '" & Replace(txtRegione.Text, "'", "''") & "%' "
            strCondizione = "AND "
        End If
        If txtProvincia.Text <> vbNullString Then
            strSQL = strSQL & strCondizione & "Provincie.Provincia LIKE '" & Replace(txtProvincia.Text, "'", "''") & "%' "
            strCondizione = "AND "
        End If
        If txtComune.Text <> vbNullString Then
            strSQL = strSQL & strCondizione & "Comuni.Denominazione LIKE '" & Replace(txtComune.Text, "'", "''") & "%' "
            strCondizione = "AND "
        End If
        If txtDescEnte.Text <> vbNullString Then
            strSQL = strSQL & strCondizione & "Enti.Denominazione LIKE '" & Replace(txtDescEnte.Text, "'", "''") & "%' "
            strCondizione = "AND "
        End If
        If txtCodEnte.Text <> vbNullString Then
            strSQL = strSQL & strCondizione & "Enti.CodiceRegione = '" & Replace(txtCodEnte.Text, "'", "''") & "' "
            strCondizione = "AND "
        End If
        If txtProgetto.Text <> vbNullString Then
            strSQL = strSQL & strCondizione & "Attività.Titolo LIKE '" & Replace(txtProgetto.Text, "'", "''") & "%' "
            strCondizione = "AND "
        End If
        If txtCodProgetto.Text <> vbNullString Then
            strSQL = strSQL & strCondizione & "Attività.CodiceEnte LIKE '" & Replace(txtCodProgetto.Text, "'", "''") & "%' "
            strCondizione = "AND "
        End If
        If txtBando.Text <> vbNullString Then
            strSQL = strSQL & strCondizione & "Bando.bando LIKE '%" & Replace(txtBando.Text, "'", "''") & "' "
            strCondizione = "AND "
        End If
        'Controllo Libretto Postale
        If CboLibPost.SelectedValue > 0 Then
            If CboLibPost.SelectedValue = 1 Then
                strSQL = strSQL & strCondizione & "IsNull(Entità.CodiceLibrettoPostale,'') <> '' "
                strCondizione = "AND "
            ElseIf CboLibPost.SelectedValue = 2 Then
                strSQL = strSQL & strCondizione & "IsNull(Entità.CodiceLibrettoPostale,'') = '' "
                strCondizione = "AND "
                'Aggiunta colonna Codice Iban - Ilaria Lombardi 05-01-2010
            ElseIf CboLibPost.SelectedValue = 3 Then
                strSQL = strSQL & strCondizione & "IsNull(Entità.iban,'') <> '' "
                strCondizione = "AND "
            ElseIf CboLibPost.SelectedValue = 4 Then
                strSQL = strSQL & strCondizione & "IsNull(Entità.iban,'') = '' "
                strCondizione = "AND "
            End If

        End If


        'Controllo Data Inizio Servizio
        If txtDataInizServ.Text <> vbNullString Then
            strSQL = strSQL & strCondizione & "Entità.DataInizioServizio = '" & txtDataInizServ.Text & "'"
            strCondizione = "AND "
        End If

        If txtalladata.Text <> vbNullString Then
            strSQL = strSQL & strCondizione & "'" & txtalladata.Text & "' between Entità.datainizioservizio and Entità.datafineservizio and Entità.datainizioservizio <> Entità.datafineservizio "
            strCondizione = "AND "

        End If
        If txtdatanascita.Text <> vbNullString Then
            strSQL = strSQL & strCondizione & "Entità.DataNascita = '" & txtdatanascita.Text & "'"
            strCondizione = "AND "
        End If

        If txtCodVolontario.Text <> vbNullString Then
            strSQL = strSQL & strCondizione & " entità.codicevolontario= '" & Replace(txtCodVolontario.Text, "'", "''") & "' "
            strCondizione = "AND "
        End If

        If txtCodFiscale.Text <> vbNullString Then
            strSQL = strSQL & strCondizione & " Entità.codicefiscale= '" & Replace(txtCodFiscale.Text, "'", "''") & "' "
            strCondizione = "AND "
        End If
        '
        '20/06/2010 Francesco Lorusso
        'Controllo lo Stato Assicurazione
        If CboStatoAss.SelectedValue > 0 Then
            strSQL &= strCondizione & "entità.IdStatoAssicurativo = " & CboStatoAss.SelectedValue
            strCondizione = "AND "
        End If

        If CboCategoria.SelectedIndex >= 1 Then
            strSQL = strSQL & strCondizione & "isnull(Entità.IDCategoriaEntità,1) = " & CboCategoria.Items(CboCategoria.SelectedIndex).Value.ToString & " "
            strCondizione = "AND "
        End If
        'FiltroVisibilita 01/12/20104 da s.c.
        If Session("FiltroVisibilita") <> Nothing Then
            strSQL = strSQL & strCondizione & " TipiProgetto.MacroTipoProgetto like '" & Session("FiltroVisibilita") & "' "
            strCondizione = "AND "
        End If

      



    
        If Cbogmo.SelectedIndex > 0 Then
            If Cbogmo.SelectedValue = 1 Then
                strSQL &= strCondizione & "IsNull(Entità.GMO,'') <> '' "
                strCondizione = "AND "
            ElseIf Cbogmo.SelectedValue = 2 Then
                strSQL &= strCondizione & "IsNull(Entità.GMO,'') = '' "
                strCondizione = "AND "
            End If
        End If

        If Cbofami.SelectedIndex > 0 Then
            If Cbofami.SelectedValue = 1 Then
                strSQL &= strCondizione & "IsNull(Entità.FAMI,'') <> '' "
                strCondizione = "AND "
            ElseIf Cbofami.SelectedValue = 2 Then
                strSQL &= strCondizione & "IsNull(Entità.FAMI,'') = '' "
                strCondizione = "AND "
            End If
        End If

        If CboEsteroUe.SelectedIndex > 0 Then
            If CboEsteroUe.SelectedValue = 1 Then
                strSQL &= strCondizione & "Attività.EsteroUE = 'True'"
                strCondizione = "AND "
            ElseIf CboEsteroUe.SelectedValue = 2 Then
                strSQL &= strCondizione & "Attività.EsteroUE = 'False'"
                strCondizione = "AND "
            End If
        End If

        If CboTutoraggio.SelectedIndex > 0 Then
            If CboTutoraggio.SelectedValue = 1 Then
                strSQL &= strCondizione & "Attività.Tutoraggio = 'True'"
                strCondizione = "AND "
            ElseIf CboTutoraggio.SelectedValue = 2 Then
                strSQL &= strCondizione & "Attività.Tutoraggio = 'False'"
                strCondizione = "AND "
            End If
        End If

        If CboDurataProg.SelectedIndex > 0 Then

            strSQL &= strCondizione & "Attività.NMesi = " & CboDurataProg.SelectedValue
            strCondizione = "AND "

        End If

        If CboPagamentiHelios.SelectedValue > 0 Then
            If CboPagamentiHelios.SelectedValue = 1 Then
                strSQL &= strCondizione & "Bando.DocumentiVolontari =1 "
                strCondizione = "AND "
            End If
            If CboPagamentiHelios.SelectedValue = 2 Then
                strSQL &= strCondizione & "Bando.DocumentiVolontari = 0"
                strCondizione = "AND "
            End If
          
        End If
        If CboDebitori.SelectedValue > 0 Then

            If CboDebitori.SelectedValue = 1 Then
                strSQL &= strCondizione & "Entità.IDEntità in(select distinct identità from COMP_ElementiRetributivi where IdTipoElemento =98  and IdPaga is null)"
                strCondizione = "AND "
            End If
            If CboDebitori.SelectedValue = 2 Then
                strSQL &= strCondizione & "Not Entità.IDEntità in(select distinct identità from COMP_ElementiRetributivi where IdTipoElemento =98  and IdPaga is null)"
                strCondizione = "AND "
            End If
          
        End If

        If CboVolSospeso.SelectedValue = 1 Then
            strSQL &= strCondizione & " Entità.IDEntità IN (SELECT DISTINCT identità FROM COMP_SospensioneVolontari WHERE Valida = 1)"
            strCondizione = "AND "
        End If

        If CboPrgSospeso.SelectedValue = 1 Then
            strSQL &= strCondizione & " Attività.IDAttività IN (SELECT DISTINCT IDAttività FROM COMP_SospensioneProgetti WHERE Valida = 1)"
            strCondizione = "AND "
        End If

        ' -- Aggiunta da Luigi Leucci il 05/03/2019
        Select Case cboStatoVerificaCF.SelectedValue
            Case -1
            Case 0
                strSQL &= strCondizione & " Entità.IDStatiVerificaCFEntità IS NULL "
                strCondizione = "AND "
            Case Is > 0
                strSQL &= strCondizione & " Entità.IDStatiVerificaCFEntità = " & cboStatoVerificaCF.SelectedValue
                strCondizione = "AND "
        End Select
        ' ------

        'eseguo la group by per il numero attività
        strSQL = strSQL & " GROUP BY " & _
                          "Entità.IDEntità, " & _
                          "Attività.IDAttività, " & _
                          "case isnull(entità.datainiziointerruzione,'01/01/1900') when '01/01/1900' then '' else 'Interruzione Temporanea ' END + isnull(Entità.AltreInformazioni,'') + case isnull(entità.notestato,'')when '' then '' " & _
                          " else ' note chiusura: ' end +  isnull(entità.notestato,''), " & _
                          "Entità.Cognome + ' ' + Entità.Nome, " & _
                          "Comuni.Denominazione, " & _
                          "Provincie.Provincia,  " & _
                          "CASE Entità.Sesso WHEN 0 THEN 'UOMO' WHEN 1 THEN 'DONNA' END, " & _
                          "CASE Entità.Abilitato	WHEN 0 THEN 'NO' WHEN 1 THEN 'SI'  END, " & _
                          "CONVERT(varchar, Entità.DataInizioServizio, 103), " & _
                          "CONVERT(varchar, Entità.DataFineServizio, 103), " & _
                          "Attività.Titolo, " & _
                          "Enti.CodiceRegione, " & _
                          "StatiEntità.StatoEntità, " & _
                          "Enti.CodiceRegione, " & _
                          "Enti.IdEnte, " & _
                          "Enti.Denominazione, " & _
                          "attività.CodiceEnte, " & _
                          "Entità.CodiceLibrettoPostale, " & _
                          "Provincie.DescrAbb, " & _
                          "entità.codicevolontario,Entità.codicefiscale,entità.iban,bando.Bando order by Entità.Cognome + ' ' + Entità.Nome "
        'eseguo la query
        dtsRisRicerca = ClsServer.DataSetGenerico(strSQL, Session("conn"))
        'assegno il dataset alla griglia del risultato
        dtgRisultatoRicerca.DataSource = dtsRisRicerca

        CaricaDataTablePerStampa(dtsRisRicerca)

        Session("appDtsRisRicerca") = dtsRisRicerca
        dtgRisultatoRicerca.DataBind()

        If dtgRisultatoRicerca.Items.Count = 0 Then
            CmdEsporta.Visible = False
            lblmessaggioRicerca.Text = "La ricerca non ha prodotto risultati."
            rigaLegenda.Visible = False
        Else
            CmdEsporta.Visible = True
            lblmessaggioRicerca.Text = "Risultato " & lblTitolo.Text
            If (Session("TipoUtente") = "U" Or Session("TipoUtente") = "R") Then
                rigaLegenda.Visible = True


            End If



        End If
    End Sub

    'routine che carica la datatable che caricherà dinamicamente la datagrid della stampa delle ricerche
    Sub CaricaDataTablePerStampa(ByVal DataSetDaScorrere As DataSet)
        Dim dt As New DataTable
        Dim dr As DataRow
        Dim i As Integer
        Dim x As Integer

        Dim NomeColonne(11) As String
        Dim NomiCampiColonne(11) As String

        'nome della colonna 
        'e posizione nella griglia di lettura

        NomeColonne(0) = "Cod.Volontario"
        NomeColonne(1) = "Nominativo"
        NomeColonne(2) = "Cod. Ente"
        NomeColonne(3) = "Ente"
        NomeColonne(4) = "Data Inizio Servizio"
        NomeColonne(5) = "Data Fine Servizio"
        NomeColonne(6) = "Comune"
        NomeColonne(7) = "Altre Info"
        NomeColonne(8) = "Stato"
        NomeColonne(9) = "Cod.Libretto Postale"
        NomeColonne(10) = "Cod.Iban"
        NomeColonne(11) = "Circolare"

        NomiCampiColonne(0) = "codicevolontario"
        NomiCampiColonne(1) = "Nominativo"
        NomiCampiColonne(2) = "CodiceRegione"
        NomiCampiColonne(3) = "Ente"
        NomiCampiColonne(4) = "DataInizio"
        NomiCampiColonne(5) = "DataFine"
        NomiCampiColonne(6) = "ComProv"
        NomiCampiColonne(7) = "AltreInfo"
        NomiCampiColonne(8) = "Stato"
        NomiCampiColonne(9) = "CLP"
        NomiCampiColonne(10) = "Iban"
        NomiCampiColonne(11) = "bando"
        'carico i nomi delle colonne che andrò a stampare nella datagrid
        For x = 0 To 11
            dt.Columns.Add(New DataColumn(NomeColonne(x), GetType(String)))
        Next

        'carico il datatable con il risultato della query della ricerca, in qusto caso delle risorse
        If DataSetDaScorrere.Tables(0).Rows.Count > 0 Then
            For i = 1 To DataSetDaScorrere.Tables(0).Rows.Count
                dr = dt.NewRow()
                For x = 0 To 11
                    dr(x) = DataSetDaScorrere.Tables(0).Rows.Item(i - 1).Item(NomiCampiColonne(x))
                Next
                dt.Rows.Add(dr)
            Next
        End If

        'passo alla sessione la datatable che ho appena creato e che userò per il databinding della datagrid della stampa
        Session("DtbRicerca") = dt

    End Sub

    'carico la combo nazioni e sesso
    Sub CaricaCombo()
        Dim strSql As String
        Try
            'SESSO
            cboSesso.Items.Add("")
            cboSesso.Items(0).Value = ""
            cboSesso.Items.Add("Uomo")
            cboSesso.Items(1).Value = 0
            cboSesso.Items.Add("Donna")
            cboSesso.Items(2).Value = 1

            Dim MyDataset As DataSet

            cboStato.Items.Clear()
            strSql = "SELECT '0' as IdStatoEntità, '' as Statoentità FROM StatiEntità " & _
                     "UNION SELECT IdStatoEntità, StatoEntità  FROM StatiEntità"
            MyDataset = ClsServer.DataSetGenerico(strSql, Session("conn"))
            cboStato.DataSource = MyDataset
            cboStato.DataValueField = "IdStatoEntità"
            cboStato.DataTextField = "StatoEntità"
            cboStato.DataBind()



            CboStatoAss.Items.Clear()
            strSql = "SELECT '0' as IdStatoAssicurativo, '' as StatoAssicurativo FROM StatiAssicurativi " & _
                       "UNION SELECT IdStatoAssicurativo, StatoAssicurativo  FROM StatiAssicurativi"
            MyDataset = ClsServer.DataSetGenerico(strSql, Session("conn"))
            CboStatoAss.DataSource = MyDataset
            CboStatoAss.DataValueField = "IdStatoAssicurativo"
            CboStatoAss.DataTextField = "StatoAssicurativo"
            CboStatoAss.DataBind()

            CboCategoria.Items.Clear()
            strSql = "SELECT '0' as IdCategoriaEntità, '' as CategoriaAbbreviata " & _
            "UNION SELECT IdCategoriaEntità, CategoriaAbbreviata FROM CategorieEntità Order by IdCategoriaEntità"
            MyDataset = ClsServer.DataSetGenerico(strSql, Session("conn"))
            CboCategoria.DataSource = MyDataset
            CboCategoria.DataTextField = "CategoriaAbbreviata"
            CboCategoria.DataValueField = "IdCategoriaEntità"
            CboCategoria.DataBind()


            ' -- Aggiunta da Luigi Leucci il 05/03/2019
            cboStatoVerificaCF.Items.Clear()
            strSql = "SELECT '-1' AS IDStatiVerificaCFEntità, '' AS Descrizione FROM StatiVerificaCFEntità " & _
                     "UNION SELECT '0' AS IDStatiVerificaCFEntità, 'Validi' AS Descrizione FROM StatiVerificaCFEntità  " & _
                     "UNION SELECT IDStatiVerificaCFEntità, Descrizione FROM StatiVerificaCFEntità"
            MyDataset = ClsServer.DataSetGenerico(strSql, Session("conn"))
            cboStatoVerificaCF.DataSource = MyDataset
            cboStatoVerificaCF.DataValueField = "IDStatiVerificaCFEntità"
            cboStatoVerificaCF.DataTextField = "Descrizione"
            cboStatoVerificaCF.DataBind()
            ' ------

        Catch ex As Exception

        End Try
    End Sub

    Private Sub cmdRicerca_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles cmdRicerca.Click
        rigaLegenda.Visible = False
        CmdEsporta.Visible = False
        ApriCSV1.Visible = False
        ApriCSV1.Visible = False
        If (ValidaData() = True) Then
            Call CaricaGriglia()
            ControlloVolontariNew()
        End If

    End Sub

    Private Sub dtgRisultatoRicerca_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dtgRisultatoRicerca.PageIndexChanged
        'utilizzo la session per memorizzare il dataset generato al momento della ricerca
        Call CaricaGriglia()
        dtgRisultatoRicerca.CurrentPageIndex = e.NewPageIndex
        dtgRisultatoRicerca.DataSource = Session("appDtsRisRicerca")
        dtgRisultatoRicerca.DataBind()
        dtgRisultatoRicerca.SelectedIndex = -1
        ControlloVolontariNew()
    End Sub
    Private Sub cmdChiudi_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdChiudi.Click
        'carico la home
        Session("appDtsRisRicerca") = Nothing
        Response.Redirect("WfrmMain.aspx")
    End Sub

    Private Sub dtgRisultatoRicerca_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dtgRisultatoRicerca.ItemCommand
        If e.CommandName = "seleziona" Then
            If (Session("TipoUtente") = "U" Or Session("TipoUtente") = "R") Then
                If e.Item.Cells(11).Text = "&nbsp;" Or e.Item.Cells(11).Text = "" Or e.Item.Cells(11).Text = "&nbsp" Then
                    Session("IdEnte") = -1
                Else
                    Session("IdEnte") = e.Item.Cells(11).Text
                End If
                Session("Denominazione") = e.Item.Cells(12).Text
            End If
            If (Session("TipoUtente") = "U" Or Session("TipoUtente") = "R") Then
                Response.Redirect("WfrmVolontari.aspx?IdVol=" & e.Item.Cells(3).Text & "&IdAttivita=" & e.Item.Cells(2).Text)
            Else
                Response.Redirect("WfrmVolontari.aspx?IdVol=" & e.Item.Cells(3).Text & "&IdAttivita=" & e.Item.Cells(2).Text & "&Ente=OK")
            End If
        End If
        If e.CommandName = "selezionaCrono" Then

            Dim selectedItem As DataGridItem = e.Item
            Dim IdVolontario As String = selectedItem.Cells(3).Text
            Dim resp As StringBuilder = New StringBuilder()
            Dim windowOption As String = "dependent=no,scrollbars=yes,status=no,resizable=yes,width=1000px ,height=400px"
            resp.Append("<script  type=""text/javascript"">" & vbCrLf)
            resp.Append("myWin = window.open('WfrmAttivitaVolontari.aspx?IdVolontario=" + IdVolontario + "', 'win'" + ",'" + windowOption + "')")
            resp.Append("</script>")
            Response.Write(resp.ToString())
        End If

    End Sub

    Private Sub ControlloVolontariNew()
        Dim item As DataGridItem
        Dim strRisultato As String
        Dim strVisualizzaEsito As String = ""
        Dim i As Integer
        Dim numeroCelle As Integer

        numeroCelle = dtgRisultatoRicerca.Columns.Count

        For Each item In dtgRisultatoRicerca.Items
            strRisultato = UCase(LeggiStoreVolontariControlli(dtgRisultatoRicerca.Items(item.ItemIndex).Cells(3).Text, strVisualizzaEsito))
            If strRisultato = "DA VERIFICARE" Then
                For i = 0 To numeroCelle - 1
                    dtgRisultatoRicerca.Items(item.ItemIndex).Cells(i).BackColor = Color.Khaki
                Next
            End If
            If strRisultato = "ANOMALIA" Then
                For i = 0 To numeroCelle - 1
                    dtgRisultatoRicerca.Items(item.ItemIndex).Cells(i).BackColor = Color.LightSalmon

                Next
            End If
            ' colonna Info Anomalie
            dtgRisultatoRicerca.Items(item.ItemIndex).Cells(16).Text = strVisualizzaEsito


        Next

    End Sub

    Private Function LeggiStoreVolontariControlli(ByVal IDEntità As Integer, ByRef VisualizzaEsito As String) As String
        'Agg. da Simona Cordella il 16/06/2009
        'richiamo store che verifca se l'ente ha completato tutti gli inserimeni e gli aggiornamenti necessari per effettuare la presentazione della domanda di accrditamento /adeguamento
        Dim intValore As Integer

        Dim myCommand As SqlClient.SqlCommand
        myCommand = New SqlClient.SqlCommand
        myCommand.CommandType = CommandType.StoredProcedure
        myCommand.CommandText = "SP_VOLONTARI_CONTROLLI"
        myCommand.Connection = Session("Conn")

        Dim sparam As SqlClient.SqlParameter
        sparam = New SqlClient.SqlParameter
        sparam.ParameterName = "@IdEntita"
        sparam.SqlDbType = SqlDbType.Int
        myCommand.Parameters.Add(sparam)


        Dim sparam1 As SqlClient.SqlParameter
        sparam1 = New SqlClient.SqlParameter
        sparam1.ParameterName = "@Esito"
        sparam1.SqlDbType = SqlDbType.VarChar
        sparam1.Size = 50
        sparam1.Direction = ParameterDirection.Output
        myCommand.Parameters.Add(sparam1)

        Dim sparam2 As SqlClient.SqlParameter
        sparam2 = New SqlClient.SqlParameter
        sparam2.ParameterName = "@VisualizzaEsito"
        sparam2.SqlDbType = SqlDbType.VarChar
        sparam2.Size = 100
        sparam2.Direction = ParameterDirection.Output
        myCommand.Parameters.Add(sparam2)

        myCommand.Parameters("@IdEntita").Value = IDEntità
        myCommand.Parameters("@VisualizzaEsito").Value = VisualizzaEsito
        'Reader = CustOrderHist.ExecuteReader()
        myCommand.ExecuteScalar()

        LeggiStoreVolontariControlli = myCommand.Parameters("@Esito").Value
        VisualizzaEsito = myCommand.Parameters("@VisualizzaEsito").Value

    End Function
    Private Sub CmdEsporta_Click(ByVal sender As Object, ByVal e As EventArgs) Handles CmdEsporta.Click
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

    Function ValidaData() As Boolean
        Dim data As Date
        Dim dataValida As Boolean = True

        Dim errore As StringBuilder = New StringBuilder()
        If (txtdatanascita.Text <> String.Empty) Then
            If Date.TryParse(txtdatanascita.Text, data) = False Then
                errore.AppendLine("Il campo 'Data di Nascita' contiene una data non valida. Immettere la data nel formato gg/mm/aaaa. <br/>")
                dataValida = False
            End If
        End If

        If (txtalladata.Text <> String.Empty) Then
            If Date.TryParse(txtalladata.Text, data) = False Then
                errore.AppendLine("Il campo 'In servizio alla data del' contiene una data non valida. Immettere la data nel formato gg/mm/aaaa. <br/>")
                dataValida = False
            End If
        End If
        If (txtDataInizServ.Text <> String.Empty) Then
            If Date.TryParse(txtDataInizServ.Text, data) = False Then
                errore.AppendLine("Il campo 'Data Inizio Servizio' contiene una data non valida. Immettere la data nel formato gg/mm/aaaa. <br/>")
                dataValida = False
            End If
        End If
        If (dataValida = False) Then
            LblErrore.Text = errore.ToString()
            LblErrore.Visible = True
        End If
        Return dataValida
    End Function
    Public Sub TuttaPaginaSess()
        Session("TP") = True
    End Sub
End Class
