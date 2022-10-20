Imports System.IO

Public Class WfrmRicercaProgettiFormazione
    Inherits System.Web.UI.Page

    Dim dtrGenerico As SqlClient.SqlDataReader
    Dim strquery As String
    Public sEsitoRicerca As Boolean

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If (Session("TipoUtente") = "U" Or Session("TipoUtente") = "R") Then
            divCodiceEnte.Visible = True
        Else
            divCodiceEnte.Visible = False
        End If

        If Request.QueryString("VengoDa") = "Ricerca" Then
            divPianificazioniConfermate.Visible = False
        Else
            divPianificazioniConfermate.Visible = True
        End If

        '***Generata da Guido Testa in data:21/07/06
        If Not Session("LogIn") Is Nothing Then
            If Session("LogIn") = False Then 'verifico validità log-in
                Response.Redirect("LogOn.aspx")
            End If
        Else
            Response.Redirect("LogOn.aspx")
        End If

        If IsPostBack = False Then
            If Request.QueryString("VengoDa") = "Ricerca" Then      'estrazione questionari
                optOreTutte.Checked = False
                optOreTutte.Enabled = False
                optOreSi.Enabled = False
                optOreNo.Enabled = False

            End If
            'scarico la session della datatable per la ricerca così che in una nuova pagina non verrà 
            'erroneamente visualizzato alcun item
            Session("DtbRicVol") = Nothing

            'richiamo sub dove popolo combo
            CaricaPrima()

            lblDenEnte.Visible = False
            txtDenominazioneEnte.Visible = False

            txtCodReg.Text = Mid(Session("CodiceRegioneEnte"), 2, 7)

        End If

        'Controllo sul tipo di utente
        If Session("TipoUtente") = "E" Then
            'Disabilito la possibilità di selezionare la denominazione dell'ente
            lblDenEnte.Visible = False
            txtDenominazioneEnte.Visible = False
            lblCodReg.Visible = False
            txtCodReg.Visible = False
        Else
            lblDenEnte.Visible = True
            txtDenominazioneEnte.Visible = True
            lblCodReg.Visible = True
            txtCodReg.Visible = True
        End If

    End Sub

    Private Sub CaricaPrima()
        '***Carico combo settore
        ddlMaccCodAmAtt.DataSource = MakeParentTable("select idmacroambitoattività, codifica + ' - ' + MacroAmbitoAttività as Macro from macroambitiattività")
        ddlMaccCodAmAtt.DataTextField = "ParentItem"
        ddlMaccCodAmAtt.DataValueField = "id"
        ddlMaccCodAmAtt.DataBind()

        '***Carico combo area intervento
        ddlCodAmAtt.Items.Add("")
        ddlCodAmAtt.Enabled = False

        '***carico combo stati attivita
        ddlStatoAttivita.DataSource = MakeParentTable("select idstatoattività, statoattività from statiattività where idstatoattività < 3 ")
        ddlStatoAttivita.DataTextField = "ParentItem"
        ddlStatoAttivita.DataValueField = "id"
        ddlStatoAttivita.DataBind()

        '*****Carico Combo Bandi
        'Mod. il 03/12/2014 da simona cordella con il filtrovisibilità
        Dim strsql As String

        strsql = "SELECT DISTINCT Bando.idBando,bando.bandobreve,bando.annobreve "
        strsql = strsql & " FROM bando"
        strsql = strsql & " INNER JOIN AssociaBandoTipiProgetto abtp on abtp.idbando =  bando.idbando"
        strsql = strsql & " INNER JOIN TipiProgetto  tp on abtp.idtipoprogetto = tp.idtipoprogetto"
        strsql = strsql & " WHERE Bando.PianificazioneFormazione=1 and tp.MacroTipoProgetto like '" & Session("FiltroVisibilita") & "' "
        strsql = strsql & " ORDER BY bando.annobreve desc"
        '"select idbando, bandobreve from Bando where PianificazioneFormazione=1"
        DdlBando.DataSource = MakeParentTable(strsql)
        DdlBando.DataTextField = "ParentItem"
        DdlBando.DataValueField = "id"
        DdlBando.DataBind()

        DdlTipiProgetto.DataSource = ClsServer.CreaDataTable("Select idTipoProgetto,Descrizione from TipiProgetto  where MacroTipoProgetto like '" & Session("FiltroVisibilita") & "' ", True, Session("conn"))
        DdlTipiProgetto.DataTextField = "Descrizione"
        DdlTipiProgetto.DataValueField = "idTipoProgetto"
        DdlTipiProgetto.DataBind()
    End Sub

    Private Function MakeParentTable(ByVal strquery As String) As DataSet
        '***Inizializzo e carico datatable 
        Dim dtrgenerico As Data.SqlClient.SqlDataReader
        ' Create a new DataTable.
        Dim myDataTable As DataTable = New DataTable
        ' Declare variables for DataColumn and DataRow objects.
        Dim myDataColumn As DataColumn
        Dim myDataRow As DataRow
        ' Create new DataColumn, set DataType, ColumnName and add to DataTable.    
        myDataColumn = New DataColumn
        myDataColumn.DataType = System.Type.GetType("System.Int64")
        myDataColumn.ColumnName = "id"
        myDataColumn.Caption = "id"
        myDataColumn.ReadOnly = True
        myDataColumn.Unique = True
        ' Add the Column to the DataColumnCollection.
        myDataTable.Columns.Add(myDataColumn)
        ' Create second column.
        myDataColumn = New DataColumn
        myDataColumn.DataType = System.Type.GetType("System.String")
        myDataColumn.ColumnName = "ParentItem"
        myDataColumn.AutoIncrement = False
        myDataColumn.Caption = "ParentItem"
        myDataColumn.ReadOnly = False
        myDataColumn.Unique = False
        myDataTable.Columns.Add(myDataColumn)
        dtrgenerico = ClsServer.CreaDatareader(strquery, Session("conn"))
        myDataRow = myDataTable.NewRow()
        myDataRow("id") = 0
        myDataRow("ParentItem") = ""
        myDataTable.Rows.Add(myDataRow)
        Do While dtrgenerico.Read
            myDataRow = myDataTable.NewRow()
            myDataRow("id") = dtrgenerico.GetValue(0)
            myDataRow("ParentItem") = dtrgenerico.GetValue(1)
            myDataTable.Rows.Add(myDataRow)
        Loop

        dtrgenerico.Close()
        dtrgenerico = Nothing

        MakeParentTable = New DataSet
        MakeParentTable.Tables.Add(myDataTable)
    End Function

    Private Sub cmdChiudi_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdChiudi.Click

        Response.Redirect("WfrmImportPianificazioneCorsi.aspx")

    End Sub

    Private Sub ddlMaccCodAmAtt_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlMaccCodAmAtt.SelectedIndexChanged
        '***in questo evento gestisco il caricamento dinamico delle combo 
        '***a seconda del settore selezionato se non seleziono nulla dalla combo
        '***popolo completamente la combo delle aree di intervento
        If ddlMaccCodAmAtt.SelectedItem.Text <> "" Then
            ddlCodAmAtt.Enabled = True
            ddlCodAmAtt.DataSource = MakeParentTable("select distinct a.idambitoattività," & _
                                                     " a.codifica + ' ' + a.AmbitoAttività as Ambito from ambitiattività a" & _
                                                     " inner join macroambitiattività b" & _
                                                     " on a.IDMacroAmbitoAttività=b.IDMacroAmbitoAttività" & _
                                                     " where a.IDMacroAmbitoAttività=" & ddlMaccCodAmAtt.SelectedValue & " and attivo=1 order by 1")
            ddlCodAmAtt.DataTextField = "ParentItem"
            ddlCodAmAtt.DataValueField = "id"
            ddlCodAmAtt.DataBind()
        Else
            'popolo completamente combo aree di intervento
            ddlCodAmAtt.DataSource = Nothing
            ddlCodAmAtt.Items.Add("")
            ddlCodAmAtt.SelectedIndex = 0
            ddlCodAmAtt.Enabled = False
        End If
    End Sub

    Private Sub cmdRicerca_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdRicerca.Click
        hlVolontari.Visible = False
        If Session("TipoUtente") = "E" Then
            If DdlBando.SelectedItem.Text <> "" Then
                lblmessaggio.Text = ""
                txtTitoloProgetto1.Value = txtTitoloProgetto.Text
                txtDenominazioneEnte1.Value = txtDenominazioneEnte.Text
                ddlMaccCodAmAtt1.Value = ddlMaccCodAmAtt.SelectedIndex
                ddlCodAmAtt1.Value = ddlCodAmAtt.SelectedIndex
                ddlStatoAttivita1.Value = ddlStatoAttivita.SelectedIndex
                dgRisultatoRicerca.CurrentPageIndex = 0
                EseguiRicerca(0)
            Else
                lblmessaggio.Visible = True
                lblmessaggio.Text = "Occorre selezionare una circolare."
            End If
        Else
            lblmessaggio.Text = ""
            txtTitoloProgetto1.Value = txtTitoloProgetto.Text
            txtDenominazioneEnte1.Value = txtDenominazioneEnte.Text
            ddlMaccCodAmAtt1.Value = ddlMaccCodAmAtt.SelectedIndex
            ddlCodAmAtt1.Value = ddlCodAmAtt.SelectedIndex
            ddlStatoAttivita1.Value = ddlStatoAttivita.SelectedIndex
            dgRisultatoRicerca.CurrentPageIndex = 0
            EseguiRicerca(0)
        End If
    End Sub

    Private Sub EseguiRicerca(ByVal bytVerifica As Byte, Optional ByVal bytpage As Integer = 0)
        '*****************************************************************************************+
        'AUTORE: Guido Testa 
        'DATA: 21/04/2006
        'DESCRIZONE: effetuo la ricerca dei progetti per poi esportare il file csv dei volontari di ogni singolo progetto
        'mod. il 22/05/2007 DA SIMONA CORDELLA
        'condizione per le colonne N° Vol e N° VOl Rimb con campo EscludiRimborso =0
        Dim Mydataset As New DataSet
        dgRisultatoRicerca.Visible = True
        ' Mydataset.Dispose()

        'Antonello
        'strquery = "select distinct IdEnte,CodiceEnte,NomeEnte,NomeEnteSecondario,IdAttività,CodiceProgetto,VediTitolo,Titolo,IdBando,Bando,Ambito,IdMacroAmbitoAttività,MacroAmbitoAttività,IdAmbitoAttività,AmbitoAttività,Selezionato,IdTipoProgetto,IdStatoAttività,StatoAttività,Regione,Provincia,Comune,Volontari,DataProroga from VW_FORMAZIONE_RICERCA_PROGETTI WHERE 1=1 "


        strquery = "Select distinct IdEnte,IdAttività,IdBando,Titolo,IdTipoProgetto,CodiceProgetto,NomeEnte,Bando,Ambito,StatoAttività,Volontari,Selezionato,DatiPianificazione  from VW_FORMAZIONE_RICERCA_PROGETTI WHERE 1=1    "
        'Antonello



        'Antonello 
        'imposto eventuali parametri
        If ddlStatoAttivita.SelectedItem.Text <> "" Then
            strquery = strquery & " and idstatoattività=" & ddlStatoAttivita.SelectedValue & ""
            'OK
        End If

        If DdlTipiProgetto.SelectedItem.Text <> "" Then
            strquery = strquery & " and idTipoProgetto=" & DdlTipiProgetto.SelectedValue & ""
            'OK
        End If

        If txtCodProg.Text <> "" Then
            strquery = strquery & " and CodiceProgetto like '" & ClsServer.NoApice(Trim(txtCodProg.Text)) & "%'"
            'OK
        End If

        If txtTitoloProgetto.Text <> "" Then
            strquery = strquery & " and titolo like '" & ClsServer.NoApice(Trim(txtTitoloProgetto.Text)) & "%'"
            'OK
        End If

        If DdlBando.SelectedItem.Text <> "" Then
            strquery = strquery & " and bandobreve like '" & ClsServer.NoApice(DdlBando.SelectedItem.Text) & "'"
            'era bando breve OK

            ''''Else
            ''''    'prendo solo i bandi con il campo FormazioneGenerale=1
            ''''    'strquery = strquery & " and c.bandobreve in (Select bandobreve From bando Where FormazioneGenerale=1)"
            ''''    strquery = strquery & " and c.FormazioneGenerale=1 "
        End If

        If CStr(Session("TipoUtente")) = "E" Then
            strquery = strquery & " and CodiceEnte like '" & CStr(Session("CodiceRegioneEnte")).Substring(1, 7) & "%'"
        Else
            If txtCodReg.Text <> "" Then
                strquery = strquery & " and CodiceEnte like '" & ClsServer.NoApice(txtCodReg.Text) & "%'"
            End If
        End If

        If txtDenominazioneEnte.Text <> "" Then
            strquery = strquery & " and NomeEnte like '" & ClsServer.NoApice(Trim(txtDenominazioneEnte.Text)) & "%'"
        End If

        If txtDenominazioneEnteSecondario.Text <> "" Then
            strquery = strquery & " and NomeEnteSecondario like '" & ClsServer.NoApice(Trim(txtDenominazioneEnteSecondario.Text)) & "%'"
        End If

        If txtRegione.Text <> "" Then
            strquery = strquery & " and regione  like '" & ClsServer.NoApice(Trim(txtRegione.Text)) & "%'"
        End If

        If txtProvincia.Text <> "" Then
            strquery = strquery & " and provincia  like '" & ClsServer.NoApice(Trim(txtProvincia.Text)) & "%'"
        End If

        If txtcomune.Text <> "" Then
            strquery = strquery & " and comune  like '" & ClsServer.NoApice(Trim(txtcomune.Text)) & "%'"
        End If

        If ddlMaccCodAmAtt.SelectedItem.Text = "" And ddlCodAmAtt.SelectedItem.Text = "" Then

        Else
            If ddlCodAmAtt.SelectedItem.Text <> "" Then
                strquery = strquery & " and IdAmbitoAttività=" & ddlCodAmAtt.SelectedValue & ""
            Else
                strquery = strquery & " and IdMacroAmbitoAttività=" & ddlMaccCodAmAtt.SelectedValue & ""
            End If
        End If



        If optOreSi.Checked = True Then
            strquery = strquery & " and DatiPianificazione='Si' "
        ElseIf optOreNo.Checked = True Then
            strquery = strquery & " and DatiPianificazione='No' "
        End If
        '-----------------Antonello---------------------
        'filtrovisibilita 02/12/2014 da s.c.
        strquery = strquery & " and Macrotipoprogetto like '" & Session("FiltroVisibilita") & "'"


        'aggiunto da simona cordella il 17/02/2015 -- erogazione formazione
        'If optUnicaTranche.Checked = True Then
        strquery = strquery & " and TipoFormazioneGenerale ='" & Request.QueryString("TipoFormazioneGenerale") & "'"
        'Else
        '    strquery = strquery & " and TipoFormazioneGenerale = 2 "
        'End If

        Mydataset = ClsServer.DataSetGenerico(strquery, Session("conn"))
        dgRisultatoRicerca.DataSource = Mydataset
        dgRisultatoRicerca.DataBind() 'valorizzo griglia

        Session("DtbRicVol") = Mydataset.Tables(0)

        If dgRisultatoRicerca.Items.Count = 0 Then 'se la griglia e vuota la nascondo
            lblmessaggio.Visible = True
            lblmessaggio.Text = "La ricerca non ha prodotto alcun risultato"
            dgRisultatoRicerca.Visible = False
            chkSelDesel.Visible = False
        Else
            chkSelDesel.Visible = True
            Call ColoraCelleADC()
            'ricerca per esportazione
            If Request.QueryString("VengoDa") <> "Ricerca" Then
                ''''''Call ColoraCelle()
            Else
                ''''''ricerca per stampa modulo
                '''''Call ColoraCelleStati()
            End If
        End If

        If dgRisultatoRicerca.Items.Count > 0 Then
            chkSelDesel.Visible = True
            If Request.QueryString("VengoDa") = "Ricerca" Then      'estrazione questionari
                CmdEsporta.Visible = False
            Else
                CmdEsporta.Visible = True
            End If
        Else
            CmdEsporta.Visible = False
        End If
    End Sub

    Private Sub ColoraCelleADC()

        Dim item As DataGridItem
        Dim color As New System.Drawing.Color
        Dim x As Integer

        For Each item In dgRisultatoRicerca.Items
            If dgRisultatoRicerca.Items(item.ItemIndex).Cells(10).Text = "Si" Then
                For x = 0 To 10
                    color = System.Drawing.Color.LightGreen
                    dgRisultatoRicerca.Items(item.ItemIndex).Cells(x).BackColor = color
                Next

            Else


            End If


        Next

    End Sub

    Private Sub dgRisultatoRicerca_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgRisultatoRicerca.PageIndexChanged
        Call RiccoraItemSelezionato(e)
        Call ColoraCelleADC()
    End Sub

    Private Sub RiccoraItemSelezionato(Optional ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs = Nothing)
        '*****************************************************************************************+
        'AUTORE: Guido Testa 
        'DATA: 26/04/2006
        'DESCRIZONE: memorizzo in un datatable il cambio di stato delle checkbox di selezione del progetto
        'il datatable memorizzato nella session lo utilizzo per nella maschera di esportazione csv (WfrmEsportazioneOreVolontari)
        '*****************************************************************************************+
        Dim i As Integer
        Dim Mychk As CheckBox
        Dim dtGriglia As DataTable 'appoggio
        Dim rwGriglia As DataRow

        dtGriglia = Session("DtbRicVol")

        '---determino cosa è stato checkato nella pag corrente e lo salvo nella datatable di sessione
        For i = 0 To dgRisultatoRicerca.Items.Count - 1
            Mychk = dgRisultatoRicerca.Items.Item(i).FindControl("chkSelProg")
            rwGriglia = dtGriglia.Rows(i + (dgRisultatoRicerca.CurrentPageIndex * 10))
            If Mychk.Checked = True Then
                rwGriglia.Item(5) = 1
            Else
                rwGriglia.Item(5) = 0
            End If
        Next i

        Session("DtbRicVol") = dtGriglia
        If Not IsNothing(e) Then
            dgRisultatoRicerca.CurrentPageIndex = e.NewPageIndex
        End If
        dgRisultatoRicerca.DataSource = Session("DtbRicVol")
        dgRisultatoRicerca.DataBind()

        '---determino cosa c'era text nella datatable di sessione in e lo visualizzo nella pag che vado a caricare
        For i = 0 To dgRisultatoRicerca.Items.Count - 1
            Mychk = dgRisultatoRicerca.Items.Item(i).FindControl("chkSelProg")
            rwGriglia = dtGriglia.Rows(i + (dgRisultatoRicerca.CurrentPageIndex * 10))
            If rwGriglia.Item(5) = "1" Then
                Mychk.Checked = True
            Else
                Mychk.Checked = False
            End If
        Next i
    End Sub

    Private Sub CmdEsporta_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdEsporta.Click
        '*****************************************************************************************+
        'AUTORE: Guido Testa 
        'DATA: 26/04/2006
        'DESCRIZONE: controllo se è stato selezionato almeno un progetto prima di 
        'avviare la maschera di esportazione
        '*****************************************************************************************+


        Dim dtGriglia As DataTable
        Dim i As Integer
        Dim bolVerifica As Boolean
        bolVerifica = False

        Call RiccoraItemSelezionato()
        Call ColoraCelleADC()
        ''''''Call ColoraCelle()

        dtGriglia = Session("DtbRicVol")

        'verifico se è stato selezionato almeno un progetto
        For i = 0 To dtGriglia.Rows.Count - 1
            If dtGriglia.Rows(i).Item(5) = 1 Then
                bolVerifica = True
                Exit For
            End If
        Next



        If bolVerifica = False Then
            lblmessaggio.Visible = True
            lblmessaggio.Text = "Selezionare almeno un progetto dalla lista"
        Else

            If Request.QueryString("TipoFormazioneGenerale") = 1 Then
                EsportaVolontari() 'unica tranche
            Else
                EsportaVolontariTranche() 'due tranche
            End If

        End If


    End Sub

    Private Sub chkSelDesel_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkSelDesel.CheckedChanged
        Dim i As Integer
        Dim Mychk As CheckBox

        hlVolontari.Visible = False

        '---determino cosa è stato checkato nella pag corrente e lo salvo nella datatable di sessione
        For i = 0 To dgRisultatoRicerca.Items.Count - 1
            Mychk = dgRisultatoRicerca.Items.Item(i).FindControl("chkSelProg")

            Mychk.Checked = chkSelDesel.Checked

        Next i
        If (chkSelDesel.Checked) Then
            chkSelDesel.Text = "Deseleziona Tutto"
        Else
            chkSelDesel.Text = "Seleziona Tutto"
        End If

    End Sub

    Private Sub EsportaVolontari()
        '*****************************************************************************************+
        'AUTORE: Guido Testa 

        'DESCRIZONE: estraggo tutti progetti precedentemente selezionati

        Dim StrSql As String
        Dim dtrVolontari As Data.SqlClient.SqlDataReader

        Dim Writer As StreamWriter
        Dim xLinea As String
        Dim i As Integer
        Dim NomeUnivoco As String
        Dim xPrefissoNome As String
        Dim dtOreVolontari As DataTable
        Dim strInCondition As String


        Try

            'vado a vedere quali sono stati i progetti selezionati per usarli come filtro per la selezione dei relativi volontari
            dtOreVolontari = Session("DtbRicVol")
            i = 0
            For i = 0 To dtOreVolontari.Rows.Count - 1
                If dtOreVolontari.Rows(i).Item(5) = 1 Then
                    strInCondition = strInCondition & dtOreVolontari.Rows(i).Item(1) & ","
                End If
            Next
            strInCondition = Mid(strInCondition, 1, Len(strInCondition) - 1)
            'StrSql = "select DISTINCT IdEnte,CodiceEnte,NomeEnte,NomeEnteSecondario,IdAttività,CodiceProgetto,VediTitolo,Titolo,IdBando,Bando,Ambito,IdMacroAmbitoAttività,MacroAmbitoAttività,IdAmbitoAttività,AmbitoAttività,Selezionato,IdTipoProgetto,IdStatoAttività,StatoAttività,Regione,Provincia,Comune,Volontari,DataProroga  from VW_FORMAZIONE_RICERCA_PROGETTI WHERE 1=1 "
            StrSql = "select DISTINCT CodiceProgetto,Titolo,Bando,Volontari,DataInizioCorso,DataFineCorso,SedeCorso,RiferimentoCorso  from VW_FORMAZIONE_RICERCA_PROGETTI WHERE 1=1 "

            ''''StrSql = "SELECT distinct enti.CodiceRegione, " & _
            ''''         "entità.CodiceVolontario, " & _
            ''''         "isnull(replace(replace(replace(replace(replace(replace(replace(entità.Cognome,'°',''),'ì','i'''),'é','e'''),'à','a'''),'ò','o'''),'è','e'''),'ù','u'''), '') as Cognome, " & _
            ''''         "isnull(replace(replace(replace(replace(replace(replace(replace(entità.Nome,'°',''),'ì','i'''),'é','e'''),'à','a'''),'ò','o'''),'è','e'''),'ù','u'''), '') as Nome, " & _
            ''''         "entità.Sesso,entità.codicefiscale,entità.datanascita, " & _
            ''''         "enti.CodiceRegione AS CodiceEnte, " & _
            ''''         "isnull(replace(replace(replace(replace(replace(replace(replace(enti.Denominazione,'°',''),'ì','i'''),'é','e'''),'à','a'''),'ò','o'''),'è','e'''),'ù','u'''), '') as Denominazione, " & _
            ''''         "attività.CodiceEnte AS CodiceProgetto, " & _
            ''''         "isnull(replace(replace(replace(replace(replace(replace(replace(attività.Titolo,'°',''),'ì','i'''),'é','e'''),'à','a'''),'ò','o'''),'è','e'''),'ù','u'''), '') as Titolo, " & _
            ''''         "isnull(replace(replace(replace(replace(replace(replace(replace(comuni_1.Denominazione,'°',''),'ì','i'''),'é','e'''),'à','a'''),'ò','o'''),'è','e'''),'ù','u'''), '') AS ComuneSedeProgetto, " & _
            ''''         "isnull(replace(replace(replace(replace(replace(replace(replace(provincie_1.Provincia,'°',''),'ì','i'''),'é','e'''),'à','a'''),'ò','o'''),'è','e'''),'ù','u'''), '') AS ProvinciaSedeProgetto, " & _
            ''''         "isnull(replace(replace(replace(replace(replace(replace(replace(regioni_1.Regione,'°',''),'ì','i'''),'é','e'''),'à','a'''),'ò','o'''),'è','e'''),'ù','u'''), '') AS RegioneSedeProgetto, " & _
            ''''         "isnull(replace(replace(replace(replace(replace(replace(replace(entisedi.Indirizzo,'°',''),'ì','i'''),'é','e'''),'à','a'''),'ò','o'''),'è','e'''),'ù','u'''), '') AS IndirizzoSedeProgetto, " & _
            ''''         "entisedi.CAP AS CapSedeProgetto, isnull(entità.OreFormazione,0) as OreFormazione,attività.IDAttività " & _
            ''''         "FROM entità " & _
            ''''         "INNER JOIN attivitàentità ON entità.IDEntità = attivitàentità.IDEntità " & _
            ''''         "INNER JOIN attivitàentisediattuazione ON attivitàentità.IDAttivitàEnteSedeAttuazione = attivitàentisediattuazione.IDAttivitàEnteSedeAttuazione " & _
            ''''         "INNER JOIN attività ON attivitàentisediattuazione.IDAttività = attività.IDAttività " & _
            ''''         "INNER JOIN entisediattuazioni ON attivitàentisediattuazione.IDEnteSedeAttuazione = entisediattuazioni.IDEnteSedeAttuazione " & _
            ''''         "INNER JOIN entisedi ON entisediattuazioni.IDEnteSede = entisedi.IDEnteSede " & _
            ''''         "INNER JOIN comuni ON entità.IDComuneNascita = comuni.IDComune " & _
            ''''         "INNER JOIN provincie ON comuni.IDProvincia = provincie.IDProvincia " & _
            ''''         "INNER JOIN comuni comuni_1 ON entisedi.IDComune = comuni_1.IDComune " & _
            ''''         "INNER JOIN provincie provincie_1 ON comuni_1.IDProvincia = provincie_1.IDProvincia " & _
            ''''         "INNER JOIN comuni comuni_2 ON entità.IDComuneResidenza = comuni_2.IDComune " & _
            ''''         "INNER JOIN provincie provincie_2 ON comuni_2.IDProvincia = provincie_2.IDProvincia " & _
            ''''         "INNER JOIN enti ON attività.IDEntePresentante = enti.IDEnte " & _
            ''''         "INNER JOIN regioni ON provincie.IDRegione = regioni.IDRegione " & _
            ''''         "INNER JOIN regioni regioni_1 ON provincie_1.IDRegione = regioni_1.IDRegione " & _
            ''''         "INNER JOIN regioni regioni_2 ON provincie_2.IDRegione = regioni_2.IDRegione " & _
            ''''         "INNER JOIN StatiEntità ON Entità.IdStatoEntità = Statientità.IdStatoEntità " & _
            ''''         "WHERE (StatiEntità.InServizio = 1 OR StatiEntità.Sospeso = 1) AND attivitàentità.EscludiFormazione=0"

            If Session("TipoUtente") = "E" Then
                StrSql = StrSql & "AND IdEnte = " & Session("IdEnte") & " "
            End If


            StrSql = StrSql & " AND IdAttività in (" & strInCondition & ")"


            ''''StrSql = StrSql & " AND IDAttività in (" & strInCondition & ")"

            ''''StrSql = StrSql & " Order by IDAttività"

            dtrVolontari = ClsServer.CreaDatareader(StrSql, Session("conn"))

            NomeUnivoco = vbNullString

            If dtrVolontari.HasRows = False Then
                lblmessaggio.Text = lblmessaggio.Text & "Nessun Volontario."
            Else
                While dtrVolontari.Read
                    If NomeUnivoco = vbNullString Then
                        xPrefissoNome = Session("Utente")
                        'Antonello
                        NomeUnivoco = xPrefissoNome & "ExpProgForm" & Year(Now) & Month(Now) & Day(Now) & Hour(Now) & Minute(Now) & Second(Now)
                        Writer = New StreamWriter(Server.MapPath("download") & "\" & NomeUnivoco & ".CSV")
                        '---intestazioni
                        xLinea = "CodiceProgetto;Titolo;Bando;Numero Volontari;Data Avvio Corso;Data Fine Corso;Sede di Svolgimento Corso;Riferimento(Nominativo e Telefono)"
                        Writer.WriteLine(xLinea)
                    End If
                    xLinea = vbNullString

                    '---salto il primo elemento (nome file)
                    'codice Progetto
                    If IsDBNull(dtrVolontari(0)) = True Then
                        xLinea = vbNullString & ";"
                    Else
                        xLinea = ClsUtility.FormatExport(dtrVolontari(0)) & ";"
                    End If
                    'Titolo
                    If IsDBNull(dtrVolontari(1)) = True Then
                        xLinea = xLinea & vbNullString & ";"
                    Else
                        xLinea = xLinea & ClsUtility.FormatExport(dtrVolontari(1)) & ";"
                    End If
                    'Bando
                    If IsDBNull(dtrVolontari(2)) = True Then
                        xLinea = xLinea & vbNullString & ";"
                    Else
                        xLinea = xLinea & ClsUtility.FormatExport(dtrVolontari(2)) & ";"
                    End If
                    'Nomero Volontari
                    If IsDBNull(dtrVolontari(3)) = True Then
                        xLinea = xLinea & vbNullString & ";"
                    Else
                        xLinea = xLinea & ClsUtility.FormatExport(dtrVolontari(3)) & ";"
                    End If
                    If IsDBNull(dtrVolontari(4)) = True Then
                        xLinea = xLinea & vbNullString & ";"
                    Else
                        xLinea = xLinea & ClsUtility.FormatExport(dtrVolontari(4)) & ";"
                    End If
                    If IsDBNull(dtrVolontari(5)) = True Then
                        xLinea = xLinea & vbNullString & ";"
                    Else
                        xLinea = xLinea & ClsUtility.FormatExport(dtrVolontari(5)) & ";"
                    End If
                    If IsDBNull(dtrVolontari(6)) = True Then
                        xLinea = xLinea & vbNullString & ";"
                    Else
                        xLinea = xLinea & ClsUtility.FormatExport(dtrVolontari(6)) & ";"
                    End If
                    If IsDBNull(dtrVolontari(7)) = True Then
                        xLinea = xLinea & vbNullString & ";"
                    Else
                        xLinea = xLinea & ClsUtility.FormatExport(dtrVolontari(7)) & ""  ''c'e un motivio che l'hhabbiamo levato oohhhhkkkk
                    End If


                    Writer.WriteLine(xLinea)

                End While
                hlVolontari.Visible = True
                hlVolontari.NavigateUrl = "download\" & NomeUnivoco & ".CSV"
                Writer.Close()
                Writer = Nothing
            End If


            dtrVolontari.Close()
            dtrVolontari = Nothing



        Catch ex As Exception
            lblmessaggio.Text = lblmessaggio.Text & "Errore durante l'esportazione dei Progetti."
            If Not Writer Is Nothing Then
                Writer.Close()
                Writer = Nothing
            End If
            If Not dtrVolontari Is Nothing Then
                dtrVolontari.Close()
                dtrVolontari = Nothing
            End If
        End Try
    End Sub

    Private Sub EsportaVolontariTranche()
        '*****************************************************************************************+
        'AUTORE: Guido Testa 
        'DESCRIZONE: estraggo tutti progetti precedentemente selezionati
        'modifica da Simona Cordella il 24/02/2015
        Dim StrSql As String
        Dim dtrVolontari As Data.SqlClient.SqlDataReader

        Dim Writer As StreamWriter
        Dim xLinea As String
        Dim i As Integer
        Dim NomeUnivoco As String
        Dim xPrefissoNome As String
        Dim dtOreVolontari As DataTable
        Dim strInCondition As String


        Try

            'vado a vedere quali sono stati i progetti selezionati per usarli come filtro per la selezione dei relativi volontari
            dtOreVolontari = Session("DtbRicVol")
            i = 0
            For i = 0 To dtOreVolontari.Rows.Count - 1
                If dtOreVolontari.Rows(i).Item(5) = 1 Then
                    strInCondition = strInCondition & dtOreVolontari.Rows(i).Item(1) & ","
                End If
            Next
            strInCondition = Mid(strInCondition, 1, Len(strInCondition) - 1)

            StrSql = " Select DISTINCT CodiceProgetto,Titolo,Bando,Volontari,DataInizioCorso,DataFineCorso,SedeCorso, " & _
                     " RiferimentoCorso,  " & _
                     " DataInizioCorsoSecondaTranche,DataFineCorsoSecondaTranche,SedeCorsoSecondaTranche,RiferimentoCorsoSecondaTranche,TipoFormazioneGenerale " & _
                     " from VW_FORMAZIONE_RICERCA_PROGETTI WHERE  TipoFormazioneGenerale =" & Request.QueryString("TipoFormazioneGenerale") & ""


            If Session("TipoUtente") = "E" Then
                StrSql = StrSql & "AND IdEnte = " & Session("IdEnte") & " "
            End If


            StrSql = StrSql & " AND IdAttività in (" & strInCondition & ")"


            ''''StrSql = StrSql & " AND IDAttività in (" & strInCondition & ")"

            ''''StrSql = StrSql & " Order by IDAttività"

            dtrVolontari = ClsServer.CreaDatareader(StrSql, Session("conn"))

            NomeUnivoco = vbNullString

            If dtrVolontari.HasRows = False Then
                lblmessaggio.Text = lblmessaggio.Text & "Nessun Volontario."
            Else
                While dtrVolontari.Read
                    If NomeUnivoco = vbNullString Then
                        xPrefissoNome = Session("Utente")
                        'Antonello

                        NomeUnivoco = xPrefissoNome & "ExpProgFormTranche" & Year(Now) & Month(Now) & Day(Now) & Hour(Now) & Minute(Now) & Second(Now)

                        Writer = New StreamWriter(Server.MapPath("download") & "\" & NomeUnivoco & ".CSV")
                        '---intestazioni
                        'modificato il 17/02/2015
                        ' If IsDBNull(dtrVolontari(11)) = 1 Then

                        'Else
                        xLinea = "CodiceProgetto;Titolo;Bando;Numero Volontari;Data Avvio Corso Prima Tranche;Data Fine Corso Prima Tranche;Sede di Svolgimento Corso;Riferimento(Nominativo e Telefono);Data Avvio Corso Seconda Tranche;Data Fine Corso Seconda Tranche;Sede di Svolgimento Corso;Riferimento(Nominativo e Telefono)"
                        'End If

                        Writer.WriteLine(xLinea)
                    End If
                    xLinea = vbNullString

                    '---salto il primo elemento (nome file)
                    'codice Progetto
                    If IsDBNull(dtrVolontari(0)) = True Then
                        xLinea = vbNullString & ";"
                    Else
                        xLinea = ClsUtility.FormatExport(dtrVolontari(0)) & ";"
                    End If
                    'Titolo
                    If IsDBNull(dtrVolontari(1)) = True Then
                        xLinea = xLinea & vbNullString & ";"
                    Else
                        xLinea = xLinea & ClsUtility.FormatExport(dtrVolontari(1)) & ";"
                    End If
                    'Bando
                    If IsDBNull(dtrVolontari(2)) = True Then
                        xLinea = xLinea & vbNullString & ";"
                    Else
                        xLinea = xLinea & ClsUtility.FormatExport(dtrVolontari(2)) & ";"
                    End If
                    'Nomero Volontari
                    If IsDBNull(dtrVolontari(3)) = True Then
                        xLinea = xLinea & vbNullString & ";"
                    Else
                        xLinea = xLinea & ClsUtility.FormatExport(dtrVolontari(3)) & ";"
                    End If
                    If IsDBNull(dtrVolontari(4)) = True Then
                        xLinea = xLinea & vbNullString & ";"
                    Else
                        xLinea = xLinea & ClsUtility.FormatExport(dtrVolontari(4)) & ";"
                    End If
                    If IsDBNull(dtrVolontari(5)) = True Then
                        xLinea = xLinea & vbNullString & ";"
                    Else
                        xLinea = xLinea & ClsUtility.FormatExport(dtrVolontari(5)) & ";"
                    End If
                    If IsDBNull(dtrVolontari(6)) = True Then
                        xLinea = xLinea & vbNullString & ";"
                    Else
                        xLinea = xLinea & ClsUtility.FormatExport(dtrVolontari(6)) & ";"
                    End If
                    If IsDBNull(dtrVolontari(7)) = True Then
                        xLinea = xLinea & vbNullString & ";"
                    Else
                        xLinea = xLinea & ClsUtility.FormatExport(dtrVolontari(7)) & ";"  ''c'e un motivio che l'hhabbiamo levato oohhhhkkkk
                    End If
                    'data inizio corso secondatrance
                    If IsDBNull(dtrVolontari(8)) = True Then
                        xLinea = xLinea & vbNullString & ";"
                    Else
                        xLinea = xLinea & ClsUtility.FormatExport(dtrVolontari(8)) & ";"  ''c'e un motivio che l'hhabbiamo levato oohhhhkkkk
                    End If
                    'data fine corso secondatrance
                    If IsDBNull(dtrVolontari(9)) = True Then
                        xLinea = xLinea & vbNullString & ";"
                    Else
                        xLinea = xLinea & ClsUtility.FormatExport(dtrVolontari(9)) & ";"  ''c'e un motivio che l'hhabbiamo levato oohhhhkkkk
                    End If
                    If IsDBNull(dtrVolontari(10)) = True Then
                        xLinea = xLinea & vbNullString & ";"
                    Else
                        xLinea = xLinea & ClsUtility.FormatExport(dtrVolontari(10)) & ";"  ''c'e un motivio che l'hhabbiamo levato oohhhhkkkk
                    End If
                    If IsDBNull(dtrVolontari(11)) = True Then
                        xLinea = xLinea & vbNullString & ";"
                    Else
                        xLinea = xLinea & ClsUtility.FormatExport(dtrVolontari(11)) & ";"  ''c'e un motivio che l'hhabbiamo levato oohhhhkkkk
                    End If

                    Writer.WriteLine(xLinea)

                End While
                hlVolontari.Visible = True
                hlVolontari.NavigateUrl = "download\" & NomeUnivoco & ".CSV"
                Writer.Close()
                Writer = Nothing
            End If


            dtrVolontari.Close()
            dtrVolontari = Nothing



        Catch ex As Exception
            lblmessaggio.Text = lblmessaggio.Text & "Errore durante l'esportazione dei Progetti."
            If Not Writer Is Nothing Then
                Writer.Close()
                Writer = Nothing
            End If
            If Not dtrVolontari Is Nothing Then
                dtrVolontari.Close()
                dtrVolontari = Nothing
            End If
        End Try
    End Sub

End Class
