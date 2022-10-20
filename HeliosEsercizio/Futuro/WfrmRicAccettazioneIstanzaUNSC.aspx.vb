Imports System.Drawing.Printing
Imports System.Drawing.Imaging
Imports System.Web.UI
Imports System.Drawing
Imports System.IO
Public Class WfrmRicAccettazioneIstanzaUNSC
    Inherits System.Web.UI.Page

    Public mydataset As DataSet
    'Protected WithEvents imgStampa As System.Web.UI.WebControls.ImageButton
    Protected WithEvents Textbox3 As System.Web.UI.WebControls.TextBox
    Protected WithEvents CboCompetenza As System.Web.UI.WebControls.DropDownList
    Protected WithEvents DdlBando As System.Web.UI.WebControls.DropDownList
    Public dtrgenerico As Data.SqlClient.SqlDataReader
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Generata da Gianluigi Paesani in data:13/07/04
        ' popolo combo anni e effettuo la ricerca di default senza parametri
        'Inserire qui il codice utente necessario per inizializzare la pagina
        If Not Session("LogIn") Is Nothing Then 'verifico validità log-in
            If Session("LogIn") = False Then
                Response.Redirect("LogOn.aspx")
            End If
        Else
            Response.Redirect("LogOn.aspx")
        End If
        'controllo se si tratta di una ricerca, controllo necessario per far si che non si perde il pageindex
        'If Not Request.Form("checkpage") Is Nothing Then
        '    If Request.Form("checkpage") = "True" Then
        '        dgRisultatoRicerca.CurrentPageIndex = 0
        '    End If
        'End If

        'se non è stata effettuata una selezione esco dalla routine
        If Not dgRisultatoRicerca.SelectedItem Is Nothing Then
            Exit Sub
        End If

        Dim strquery As String
        Dim dtrCompetenze As SqlClient.SqlDataReader

        If IsPostBack = False Then
            '*****Carico Combo Bandi Circolari
            'strquery = "Select idbando, bandobreve, annobreve from Bando UNION Select '0',' TUTTI ', 99  order by annobreve desc "
            'mod il 04/12/2014 da s.c. filtrovisibilità
            strquery = " SELECT Bando.idBando,bando.bandobreve,bando.annobreve  "
            strquery = strquery & " FROM bando"
            strquery = strquery & " INNER JOIN AssociaBandoTipiProgetto abtp on abtp.idbando =  bando.idbando"
            strquery = strquery & " INNER JOIN TipiProgetto  tp on abtp.idtipoprogetto = tp.idtipoprogetto"
            strquery = strquery & " WHERE bando.programmi = 0 and tp.MacroTipoProgetto like '" & Session("FiltroVisibilita") & "'"
            strquery = strquery & " UNION "
            strquery = strquery & " SELECT  '0',' TUTTI ', 99  from bando "
            strquery = strquery & " ORDER BY Bando.annobreve desc"
            'chiudo il datareader se aperto
            If Not dtrCompetenze Is Nothing Then
                dtrCompetenze.Close()
                dtrCompetenze = Nothing
            End If
            dtrCompetenze = ClsServer.CreaDatareader(strquery, Session("conn"))
            DdlBando.DataSource = dtrCompetenze
            DdlBando.DataTextField = "bandobreve"
            DdlBando.DataValueField = "idbando"
            DdlBando.DataBind()
            If Not dtrCompetenze Is Nothing Then
                dtrCompetenze.Close()
                dtrCompetenze = Nothing
            End If

            CaricaCompetenze()
            'carico la combo degli stati
            caricacomboanno()
            CaricaPrima()
            'mydataset = ClsServer.DataSetGenerico("select a.idbando,e.idente, e.CodiceRegione, e.denominazione, a.bando," & _
            '" convert(varchar,a.datainiziovalidità,103) as datainizio," & _
            '" convert(varchar,a.datafinevalidità,103) as datafine," & _
            '" count(c.idattività) as progetti, d.statobandoattività as Stato,b.idbandoattività from bando a" & _
            '" inner join bandiattività b on a.idbando=b.idbando" & _
            '" inner join attività c on b.idbandoattività=c.idbandoattività" & _
            '" inner join StatiBandiAttività d on d.idstatobandoattività=b.idstatobandoattività" & _
            '" inner join enti e on b.idente=e.idente" & _
            '" where d.DaValutare =1" & _
            '" group by b.idente,a.idbando, a.bando," & _
            '" a.datainiziovalidità, a.datafinevalidità,d.statobandoattività,e.denominazione,e.CodiceRegione,e.idente,b.idbandoattività order by a.datainiziovalidità asc", Session("conn"))

            'dgRisultatoRicerca.DataSource = mydataset 'eseguo ricerca senza parametri
            'dgRisultatoRicerca.DataBind() 'valorizzo griglia
            'ricerca()
            If Session("CodiceRegioneEnte") <> "" Then
                txtCodEnte.Text = Session("txtCodEnte")
            End If

        Else
            If dgRisultatoRicerca.SelectedItem Is Nothing = True Then
                If (txtDenominazioneEnte.Text = "" And DdlBando.SelectedValue = "0" And txtCodEnte.Text = "" And ddlStatoAttivita.SelectedValue = 0 And ddlanno.SelectedItem.Text = "Seleziona") Then
                    'carico la griglia con le risorse dell'ente loggato
                    'ricerca()
                End If
            End If
            'chiamo la procedura che mi fa la ricerca e mi carica la griglia
            'ricerca()
        End If
        If dgRisultatoRicerca.Items.Count > 0 Then
            CmdEsporta.Visible = True
        Else
            CmdEsporta.Visible = False
        End If
    End Sub
    Sub CaricaCompetenze()
        Dim strSQL As String
        'datareader che conterrà l'id 
        Dim dtrCompetenze As System.Data.SqlClient.SqlDataReader

        Try
            'controllo se si tratta del primo caricamento. così leggo i dati nel db una sola volta
            If Page.IsPostBack = False Then
                'preparo la query

                strSQL = "select IdRegioneCompetenza,Descrizione,CodiceRegioneCompetenza,left(CodiceRegioneCompetenza,1)from RegioniCompetenze where IdRegioneCompetenza <> 22 "
                strSQL = strSQL & " union "
                strSQL = strSQL & " select '0',' TUTTI ','','A' "
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

                'eseguo la query
                dtrCompetenze = ClsServer.CreaDatareader(strSQL, Session("conn"))
                'assegno il datadearder alla combo caricando così descrizione e id
                CboCompetenza.DataSource = dtrCompetenze
                CboCompetenza.Items.Add("")
                CboCompetenza.DataTextField = "Descrizione"
                CboCompetenza.DataValueField = "IDRegioneCompetenza"
                CboCompetenza.DataBind()
                'chiudo il datareader se aperto
                If Not dtrCompetenze Is Nothing Then
                    dtrCompetenze.Close()
                    dtrCompetenze = Nothing
                End If
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

                If Session("TipoUtente") = "R" Then
                    CboCompetenza.Enabled = False
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
    Sub caricacomboanno()
        Dim dtr As Data.SqlClient.SqlDataReader
        dtr = ClsServer.CreaDatareader("select ' Seleziona ' as Appoggio from bando" & _
        " union" & _
        " select distinct convert(varchar,(year(DataInizioValidità))) as Anno from bando order by 1", Session("conn"))
        If dtr.HasRows = True Then 'eseguo query per anni data bando
            Do While dtr.Read() 'popolo combo
                ddlanno.Items.Add(Trim(dtr.GetValue(0)))
            Loop
            dtr.Close()
            dtr = Nothing
        Else
            dtr.Close()
            dtr = Nothing
        End If
    End Sub

    Private Sub CaricaPrima()
        '***Generata da Gianluigi Paesani in data:05/07/04
        '***Carico combo settore

        '***carico combo stati attivita
        ddlStatoAttivita.DataSource = MakeParentTable("select IdStatoBandoAttività, StatoBandoAttività from statiBandiAttività")
        ddlStatoAttivita.DataTextField = "ParentItem"
        ddlStatoAttivita.DataValueField = "id"
        ddlStatoAttivita.DataBind()

        'visualizzo valore predefinito
        'If Request.QueryString("VengoDa") = "Valutare" Then
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
        dtrgenerico = ClsServer.CreaDatareader("select IdStatoBandoAttività from statiBandiAttività where DaValutare=1", Session("conn"))
        Do While dtrgenerico.Read
            'posizione combo stato su item predefinito
            ddlStatoAttivita.SelectedValue = dtrgenerico.GetValue(0)
        Loop
        dtrgenerico.Close()
        dtrgenerico = Nothing
        'End If
    End Sub
    Private Function MakeParentTable(ByVal strquery As String) As DataSet
        '***Generata da Gianluigi Paesani in data:05/07/04
        '***Inizializzo e carico datatable 

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
        ' Add the column to the table.
        myDataTable.Columns.Add(myDataColumn)
        ' Make the ID column the primary key column. da verificare?????????
        'Dim PrimaryKeyColumns(0) As DataColumn
        'PrimaryKeyColumns(0) = myDataTable.Columns("id"))
        'myDataTable.PrimaryKey = PrimaryKeyColumns)
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
        dtrgenerico = ClsServer.CreaDatareader(strquery, Session("conn"))
        'Instantiate the DataSet variable.
        'mydataset = New DataSet
        ' Add the new DataTable to the DataSet.
        'mydataset.Tables.Add(myDataTable)
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
        'stringa per la query
        MakeParentTable.Tables.Add(myDataTable)
    End Function
    Sub ricerca()
        'Generata da Gianluigi Paesani in data:13/07/04
        'esegue ricerca impostata dell'utente
        lblmessaggio.Text = "" 'setto parametri utenza
        dgRisultatoRicerca.Visible = True
        dgRisultatoRicerca.CurrentPageIndex = 0

        Dim strquery As String = "select a.idbando,e.idente,e.denominazione, e.CodiceRegione, a.bando," & _
            " convert(varchar,a.datainiziovalidità,103) as datainizio," & _
            " convert(varchar,a.datafinevalidità,103) as datafine," & _
            " count(c.idattività) as progetti, d.statobandoattività as Stato,b.idbandoattività,RegioniCompetenze.Descrizione as Competenza from bando a" & _
            " inner join bandiattività b on a.idbando=b.idbando" & _
            " inner join attività c on b.idbandoattività=c.idbandoattività" & _
            " INNER JOIN TipiProgetto ON c.IdTipoProgetto = TipiProgetto.IdTipoProgetto " & _
            " INNER JOIN AssociaProfiliTipiProgetto ON TipiProgetto.IdTipoProgetto = AssociaProfiliTipiProgetto.IdTipoProgetto " & _
            " INNER JOIN Profili ON AssociaProfiliTipiProgetto.IdProfilo = Profili.IdProfilo "

        '============================================================================================================================
        '====================================================30/09/2008==============================================================
        '=======MODIFICA EFFETTUATA DA Kronk (99 scimmie saltavano sul letto, una cadde in terra e si ruppe il cervelletto...)=======
        '=================AGGIUNTA JOIN SU PROFILO READ PER ABILITARE LEGGERE LE ABILITAZIONI ANCHE DELL'UTENTE READ=================
        '============================================================================================================================
        If UCase(Me.TemplateSourceDirectory) <> "/HELIOSREAD" Then
            strquery = strquery & " INNER JOIN	AssociaUtenteGruppo ON Profili.IdProfilo = AssociaUtenteGruppo.IdProfilo "
        Else
            strquery = strquery & " INNER JOIN	AssociaUtenteGruppo ON Profili.IdProfilo = AssociaUtenteGruppo.IdProfiloRead "
        End If

        strquery = strquery & " LEFT JOIN BANDORICORSI ON c.IDBANDORICORSO = BANDORICORSI.IDBANDORICORSO " & _
            " inner join StatiBandiAttività d on d.idstatobandoattività=b.idstatobandoattività" & _
            " inner join enti e on b.idente=e.idente" & _
            " Left Join AssociaBandoRegioniCompetenze On a.IdBando = AssociaBandoRegioniCompetenze.IdBando" & _
            " Left Join RegioniCompetenze On AssociaBandoRegioniCompetenze.IdRegioneCompetenza = RegioniCompetenze.IdRegioneCompetenza" & _
            " where 1=1 and a.programmi=0 and AssociaUtenteGruppo.username='" & Replace(Session("Utente"), "'", "''") & "' "
        If Session("TipoUtente") = "E" Then
            strquery = strquery & " and CASE ISNULL(BANDORICORSI.IDBANDORICORSO,0) WHEN 0 THEN isnull(a.enteabilitato,1) ELSE isnull(BANDORICORSI.enteabilitato,1) END = 1 "
        End If
        'verifico digitazione filtri da utenza

        If txtDenominazioneEnte.Text <> "" Then
            strquery = strquery & " and e.denominazione like '" & ClsServer.NoApice(txtDenominazioneEnte.Text) & "%'"
        End If

        If DdlBando.SelectedValue <> "0" Then
            strquery = strquery & " and a.idbando = " & DdlBando.SelectedValue
        End If

        If ddlanno.SelectedItem.Text <> "Seleziona" Then
            strquery = strquery & " and year(a.DatainizioValidità)=" & CInt(ddlanno.SelectedItem.Text) & ""
        End If

        If txtCodEnte.Text <> "" Then
            strquery = strquery & " and e.CodiceRegione='" & ClsServer.NoApice(txtCodEnte.Text) & "'"
        End If

        If ddlStatoAttivita.SelectedValue <> 0 Then
            strquery = strquery & " and d.idstatobandoattività=" & ddlStatoAttivita.SelectedValue & ""
        End If
        ' Filtro per regioni
        If CboCompetenza.SelectedValue <> "" Then
            Select Case CboCompetenza.SelectedValue
                Case 0
                    strquery = strquery & " "
                Case -1
                    strquery = strquery & " And AssociaBandoRegioniCompetenze.IdRegioneCompetenza = 22"
                Case -2
                    strquery = strquery & " And AssociaBandoRegioniCompetenze.IdRegioneCompetenza <> 22 And not AssociaBandoRegioniCompetenze.IdRegioneCompetenza is null "
                Case -3
                    strquery = strquery & " And AssociaBandoRegioniCompetenze.IdRegioneCompetenza is null "
                Case Else
                    strquery = strquery & " And AssociaBandoRegioniCompetenze.IdRegioneCompetenza = " & CboCompetenza.SelectedValue
            End Select
        End If
        strquery = strquery & " AND TipiProgetto.MacroTipoProgetto like '" & Session("FiltroVisibilita") & "'"
        strquery = strquery & " group by b.idente,a.idbando, a.bando," & _
            " a.datainiziovalidità, a.datafinevalidità,d.statobandoattività,e.denominazione,e.CodiceRegione,e.idente,b.idbandoattività,RegioniCompetenze.Descrizione order by a.datainiziovalidità asc"

        mydataset = ClsServer.DataSetGenerico(strquery, Session("conn"))
        dgRisultatoRicerca.DataSource = mydataset 'eseguo ricerca
        dgRisultatoRicerca.DataBind() 'valorizzo griglia
        Session("LocalDataSet") = mydataset
        CaricaDataTablePerStampa(mydataset)
        If dgRisultatoRicerca.Items.Count = 0 Then 'se la griglia e vuota la nascondo
            lblmessaggio.Text = "Attenzione, non sono presenti Istanze di Presentazione per i parametri inseriti."
            dgRisultatoRicerca.Visible = False
            CmdEsporta.Visible = False
        Else
            CmdEsporta.Visible = True
        End If
    End Sub

    'routine che carica la datatable che caricherà dinamicamente la datagrid della stampa delle ricerche
    Sub CaricaDataTablePerStampa(ByVal DataSetDaScorrere As DataSet)
        Dim dt As New DataTable
        Dim dr As DataRow
        Dim i As Integer
        Dim x As Integer

        Dim NomeColonne(6) As String
        Dim NomiCampiColonne(6) As String

        'nome della colonna 
        'e posizione nella griglia di lettura
        NomeColonne(0) = "Ente"
        NomeColonne(1) = "Cod. Ente"
        NomeColonne(2) = "Bando"
        NomeColonne(3) = "Data Inizio Bando"
        NomeColonne(4) = "Data Fine Bando"
        NomeColonne(5) = "N. Progetti"
        NomeColonne(6) = "Competenza"

        NomiCampiColonne(0) = "denominazione"
        NomiCampiColonne(1) = "CodiceRegione"
        NomiCampiColonne(2) = "Bando"
        NomiCampiColonne(3) = "datainizio"
        NomiCampiColonne(4) = "datafine"
        NomiCampiColonne(5) = "progetti"
        NomiCampiColonne(6) = "Competenza"

        'carico i nomi delle colonne che andrò a stampare nella datagrid
        For x = 0 To 6
            dt.Columns.Add(New DataColumn(NomeColonne(x), GetType(String)))
        Next

        'carico il datatable con il risultato della query della ricerca, in qusto caso delle risorse
        If DataSetDaScorrere.Tables(0).Rows.Count > 0 Then
            For i = 1 To DataSetDaScorrere.Tables(0).Rows.Count
                dr = dt.NewRow()
                For x = 0 To 6
                    dr(x) = DataSetDaScorrere.Tables(0).Rows.Item(i - 1).Item(NomiCampiColonne(x))
                Next
                dt.Rows.Add(dr)
            Next
        End If

        'passo alla sessione la datatable che ho appena creato e che userò per il databinding della datagrid della stampa
        Session("DtbRicerca") = dt

    End Sub


    Private Sub dgRisultatoRicerca_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgRisultatoRicerca.PageIndexChanged
        'passo il nuovo indice selezionato all'indice della pagina da visualizzare
        dgRisultatoRicerca.CurrentPageIndex = e.NewPageIndex
        'riassegno il dataset dichiarato volutamente pubblico a tutta la pagina
        dgRisultatoRicerca.DataSource = Session("LocalDataSet")
        dgRisultatoRicerca.DataBind()
    End Sub

    Private Sub dgRisultatoRicerca_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgRisultatoRicerca.ItemCommand
        Select Case e.CommandName
            Case "Select"
                'Generata da Gianluigi Paesani in data:13/07/04
                'alla selezione eseguo chiamata pagina per modifica con parametri (idbando)
                'e valorizzo sessioni ente
                'If Not dgRisultatoRicerca.SelectedItem Is Nothing Then
                Session("idente") = CInt(e.Item.Cells(2).Text)
                Session("denominazione") = e.Item.Cells(3).Text
                Response.Redirect("WfrmIstanzaPresentazione.aspx?Verso=Mod&id=" & e.Item.Cells(1).Text & "&DataFine=" & e.Item.Cells(7).Text & "&DataInizio=" & e.Item.Cells(6).Text & "&Stato=" & e.Item.Cells(12).Text & "&Arrivo=WfrmRicAccettazioneIstanzaUNSC.aspx&idBA=" & e.Item.Cells(10).Text & "")
                'End If
            Case "Dettagli"
                Session("txtCodEnte") = e.Item.Cells(4).Text
                Session("Denominazione") = e.Item.Cells(3).Text
                Session("idEnte") = e.Item.Cells(2).Text
                Response.Redirect("ricercaprogetti.aspx?VengoDa=Valutare&IdBando=" & e.Item.Cells(1).Text & "&Bando=" & e.Item.Cells(5).Text & "")
        End Select
    End Sub

    Protected Sub cmdSalva_Click(sender As Object, e As EventArgs) Handles cmdSalva.Click
        hlVolontari.Visible = False
        ricerca()
    End Sub

    Protected Sub cmdChiudi_Click(sender As Object, e As EventArgs) Handles cmdChiudi.Click
        'Generata da Gianluigi Paesani in data:13/07/04
        'vado alla main
        Response.Redirect("WfrmMain.aspx")
    End Sub

    Protected Sub CmdEsporta_Click(sender As Object, e As EventArgs) Handles CmdEsporta.Click
        CmdEsporta.Visible = False
        StampaCSV(Session("DtbRicerca"))
    End Sub
    Function StampaCSV(ByVal DTBRicerca As DataTable)

        Dim dtrIstanze As Data.SqlClient.SqlDataReader

        Dim Writer As StreamWriter
        Dim xLinea As String
        Dim i As Int64
        Dim j As Int64
        Dim NomeUnivoco As String
        Dim xPrefissoNome As String = vbNullString
        Dim Reader As StreamReader

        If DTBRicerca.Rows.Count = 0 Then
            hlVolontari.Visible = False
            CmdEsporta.Visible = False
        Else
            xPrefissoNome = Session("Utente")
            NomeUnivoco = xPrefissoNome & "ExpElencoIstanze" & Year(Now) & Month(Now) & Day(Now) & Hour(Now) & Minute(Now) & Second(Now)
            Writer = New StreamWriter(Server.MapPath("download") & "\" & NomeUnivoco & ".CSV")
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

            hlVolontari.Visible = True
            hlVolontari.NavigateUrl = "download\" & NomeUnivoco & ".CSV"

            Writer.Close()
            Writer = Nothing

        End If

    End Function
End Class