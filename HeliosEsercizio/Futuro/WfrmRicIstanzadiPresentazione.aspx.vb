Imports System.Drawing.Printing
Imports System.Drawing.Imaging
Imports System.Web.UI
Imports System.Drawing
Imports System.IO
Public Class WfrmRicIstanzadiPresentazione
    Inherits System.Web.UI.Page
    Dim mydataset As DataSet
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Generata da Gianluigi Paesani in data:13/07/04
        ' popolo combo anni e effettuo la ricerca di default senza parametri
        'Inserire qui il codice utente necessario per inizializzare la pagina

        Dim strquery As String
        Dim dtrCompetenze As SqlClient.SqlDataReader

        If Not Session("LogIn") Is Nothing Then 'verifico validità log-in
            If Session("LogIn") = False Then
                Response.Redirect("LogOn.aspx")
            End If
        Else
            Response.Redirect("LogOn.aspx")
        End If

        If IsPostBack = False Then
            '*****Carico Combo Bandi Circolari
            'strquery = "Select idbando, bandobreve, annobreve from Bando UNION Select '0',' TUTTI ', 99 order by annobreve desc"
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

            '*****Carico Combo Competenze
            strquery = "select IdRegioneCompetenza,Descrizione,CodiceRegioneCompetenza,left(CodiceRegioneCompetenza,1)from RegioniCompetenze where IdRegioneCompetenza <> 22 "
            strquery = strquery & " union "
            strquery = strquery & " select '0',' TUTTI ','','A' "
            strquery = strquery & " union "
            strquery = strquery & " select '-1',' NAZIONALE ','','B' "
            strquery = strquery & " union "
            strquery = strquery & " select '-2',' REGIONALE ','','C' "
            strquery = strquery & " union "
            strquery = strquery & " select '-3',' NON DEFINITO ','','D' "
            strquery = strquery & "  from RegioniCompetenze order by left(CodiceRegioneCompetenza,1),descrizione "

            'chiudo il datareader se aperto
            If Not dtrCompetenze Is Nothing Then
                dtrCompetenze.Close()
                dtrCompetenze = Nothing
            End If

            'eseguo la query
            dtrCompetenze = ClsServer.CreaDatareader(strquery, Session("conn"))
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

            '-------------------FINE GESTIONE COMBO COMPETENZE--------------------------------------------------------

            Dim dtr As Data.SqlClient.SqlDataReader
            'seleziono distintamente anni come parametro di ricerca
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



        End If

        'Controllo abilitazione scelta
        If Session("TipoUtente") = "U" Or Session("TipoUtente") = "E" Then
            CboCompetenza.Enabled = True
        Else
            'CboCompetenza.SelectedIndex = 1
            CboCompetenza.Enabled = False
            'preparo la query
            strquery = "select b.IdRegioneCompetenza ,b.Heliosread from RegioniCompetenze a "
            strquery = strquery & "INNER JOIN utentiunsc b ON a.idregionecompetenza = b.idregionecompetenza "
            strquery = strquery & "where b.username = '" & Session("Utente") & "'"
            'chiudo il datareader se aperto
            If Not dtrCompetenze Is Nothing Then
                dtrCompetenze.Close()
                dtrCompetenze = Nothing
            End If
            'controllo se utente o ente regionale
            'eseguo la query
            dtrCompetenze = ClsServer.CreaDatareader(strquery, Session("conn"))
            dtrCompetenze.Read()
            If dtrCompetenze.HasRows = True Then

                CboCompetenza.SelectedValue = dtrCompetenze("IdRegioneCompetenza")

                If dtrCompetenze("Heliosread") = True Then

                    CboCompetenza.Enabled = True

                End If

                If Session("TipoUtente") = "R" Then
                    CboCompetenza.Enabled = False
                End If

            End If

            If Not dtrCompetenze Is Nothing Then
                dtrCompetenze.Close()
                dtrCompetenze = Nothing
            End If

        End If
    End Sub
    Sub CaricaDataGrid(ByRef GridDaCaricare As DataGrid) 'valorizzo la datagrid passata
        Dim appo As String
        GridDaCaricare.DataSource = mydataset
        GridDaCaricare.DataBind()
        'Aggiunto da Alessandra Taballione il 29.03.2005
        '*********************************************************************************
        'blocco per la creazione della datatable per la stampa della ricerca
        'nome e posizione di lettura delle colopnne a base 0
        Dim NomeColonne(5) As String
        Dim NomiCampiColonne(5) As String
        'nome della colonna 
        'e posizione nella griglia di lettura
        NomeColonne(0) = "Circolare"
        NomeColonne(1) = "Data Inizio Circolare"
        NomeColonne(2) = "Data Fine Circolare"
        NomeColonne(3) = "Stato"
        NomeColonne(4) = "N. Progetti"
        NomeColonne(5) = "Competenza"

        NomiCampiColonne(0) = "Bando"
        NomiCampiColonne(1) = "DataInizio"
        NomiCampiColonne(2) = "DataFine"
        NomiCampiColonne(3) = "Stato"
        NomiCampiColonne(4) = "progetti"
        NomiCampiColonne(5) = "descrizione"

        'carico un datatable che userò poi nella pagina di stampa
        'il numero delle colonne è a base 0
        Session("DtbRicerca") = ClsServer.CaricaDataTablePerStampa(mydataset, 5, NomeColonne, NomiCampiColonne)

        '***********************************************************************
        If dgRisultatoRicerca.Items.Count = 0 Then 'se la griglia e vuota la nascondo
            lblmessaggio.Text = "Attenzione, non sono presenti Istanze di Presentazione da modificare."
            dgRisultatoRicerca.Visible = False
            'cmdSalva.Visible = False
            imgStampa.Visible = False
        Else
            imgStampa.Visible = True
        End If
    End Sub
    Private Sub dgRisultatoRicerca_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgRisultatoRicerca.ItemCommand
        'Generata da Gianluigi Paesani in data:13/07/04
        'Modificata da Alessandra Taballione in data:09/09/04
        'alla selezione eseguo chiamata pagina per modifica con parametri (idbando)
        Select Case e.CommandName
            Case "Ricerca"
                Response.Redirect("ricercaprogetti.aspx?VengoDa=Valutare&IdBando=" & e.Item.Cells(1).Text & "&Bando=" & e.Item.Cells(2).Text & "")
            Case "Select"
                Response.Redirect("WfrmIstanzaPresentazione.aspx?DataFine=" & e.Item.Cells(4).Text & "&DataInizio=" & e.Item.Cells(3).Text & "&Verso=Mod&id=" & e.Item.Cells(1).Text & "&Stato=" & e.Item.Cells(5).Text & "&Arrivo=WfrmRicIstanzadiPresentazione.aspx&idBA=" & e.Item.Cells(9).Text & "")
        End Select
    End Sub
    Protected Sub cmdSalva_Click(sender As Object, e As EventArgs) Handles cmdSalva.Click
        'Generata da Gianluigi Paesani in data:13/07/04
        'esegue ricerca impostata dell'utente
        ' Dim mydataset As DataSet
        lblmessaggio.Text = "" 'setto parametri utenza
        dgRisultatoRicerca.Visible = True

        Dim strquery As String = "select distinct a.idbando, a.bando," & _
            " convert(varchar,a.datainiziovalidità,103) as datainizio," & _
            " convert(varchar,a.datafinevalidità,103) as datafine," & _
            " count(distinct c.idattività) as progetti, d.statobandoattività as Stato, b.idbandoattività,RegioniCompetenze.descrizione from bando a" & _
            " INNER JOIN AssociabandoTipiProgetto ab on a.idbando=ab.idbando " & _
            " INNER JOIN TipiProgetto ON ab.IdTipoProgetto = TipiProgetto.IdTipoProgetto " & _
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

        strquery = strquery & " INNER JOIN bandiattività b on a.idbando=b.idbando" & _
            " INNER JOIN attività c on b.idbandoattività=c.idbandoattività" & _
            " LEFT JOIN BANDORICORSI ON c.IDBANDORICORSO = BANDORICORSI.IDBANDORICORSO " & _
            " INNER JOIN StatiBandiAttività d on d.idstatobandoattività=b.idstatobandoattività" & _
            " INNER JOIN AssociaBandoRegioniCompetenze ON a.IDBando = AssociaBandoRegioniCompetenze.IdBando " & _
            " INNER JOIN RegioniCompetenze ON AssociaBandoRegioniCompetenze.IdRegioneCompetenza = RegioniCompetenze.IdRegioneCompetenza " & _
            " WHERE a.programmi = 0 and b.idente=" & Session("idente") & " AND AssociaUtenteGruppo.Username='" & Session("Utente") & "'"

        If Session("TipoUtente") = "E" Then
            strquery = strquery & " and CASE ISNULL(BANDORICORSI.IDBANDORICORSO,0) WHEN 0 THEN isnull(a.enteabilitato,1) ELSE isnull(BANDORICORSI.enteabilitato,1) END = 1 "
        End If
        'verifico digitazione filtri da utenza
        If DdlBando.SelectedValue <> "0" Then
            strquery = strquery & " and a.idbando = " & DdlBando.SelectedValue
        End If

        If CboCompetenza.SelectedValue <> "" Then
            Select Case CboCompetenza.SelectedValue
                Case 0
                    strquery = strquery & " "
                Case -1
                    strquery = strquery & " And RegioniCompetenze.IdRegioneCompetenza = 22"
                Case -2
                    strquery = strquery & " And RegioniCompetenze.IdRegioneCompetenza <> 22 And not RegioniCompetenze.IdRegioneCompetenza is null "
                Case -3
                    strquery = strquery & " And RegioniCompetenze.IdRegioneCompetenza is null "
                Case Else
                    strquery = strquery & " And RegioniCompetenze.IdRegioneCompetenza = " & CboCompetenza.SelectedValue
            End Select
        End If

        If ddlanno.SelectedItem.Text <> "Seleziona" Then
            strquery = strquery & " and year(a.DatainizioValidità)=" & CInt(ddlanno.SelectedItem.Text) & ""
        End If
        'FILTROVISIBILITA'
        strquery = strquery & " AND TipiProgetto.MacroTipoProgetto like '" & Session("FiltroVisibilita") & "'"

        strquery = strquery & " group by b.idente,a.idbando, a.bando," & _
            " a.datainiziovalidità, a.datafinevalidità,d.statobandoattività,b.idbandoattività,RegioniCompetenze.descrizione"

        mydataset = ClsServer.DataSetGenerico(strquery, Session("conn"))

        'dgRisultatoRicerca.DataSource = mydataset 'eseguo ricerca
        'dgRisultatoRicerca.DataBind() 'valorizzo griglia
        CaricaDataGrid(dgRisultatoRicerca)
        If dgRisultatoRicerca.Items.Count = 0 Then 'se la griglia e vuota la nascondo
            lblmessaggio.Text = "Attenzione, non sono presenti Istanze di Presentazione per i parametri inseriti."
            dgRisultatoRicerca.Visible = False
            imgStampa.Visible = False
        Else
            imgStampa.Visible = True

        End If


    End Sub

    Protected Sub cmdChiudi_Click(sender As Object, e As EventArgs) Handles cmdChiudi.Click
        'Generata da Gianluigi Paesani in data:13/07/04
        'vado alla main
        Response.Redirect("WfrmMain.aspx")
    End Sub

    Protected Sub imgStampa_Click(sender As Object, e As EventArgs) Handles imgStampa.Click
        imgStampa.Visible = False
        StampaCSV(Session("DtbRicerca"))
    End Sub
    Function StampaCSV(ByVal DTBRicerca As DataTable)

        'Dim dtrIstanza As Data.SqlClient.SqlDataReader

        Dim Writer As StreamWriter
        Dim xLinea As String
        Dim i As Int64
        Dim j As Int64
        Dim NomeUnivoco As String
        Dim xPrefissoNome As String = vbNullString
        'Dim Reader As StreamReader

        If DTBRicerca.Rows.Count = 0 Then
            ApriCSV1.Visible = False

        Else
            xPrefissoNome = Session("Utente")
            NomeUnivoco = xPrefissoNome & "ExpRicercaIstanza" & Year(Now) & Month(Now) & Day(Now) & Hour(Now) & Minute(Now) & Second(Now)
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

            ApriCSV1.Visible = True
            ApriCSV1.NavigateUrl = "download\" & NomeUnivoco & ".CSV"

            Writer.Close()
            Writer = Nothing

        End If

    End Function
End Class