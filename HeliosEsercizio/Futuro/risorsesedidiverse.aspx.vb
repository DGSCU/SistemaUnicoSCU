Imports System.Drawing.Printing
Imports System.Drawing.Imaging
Imports System.Web.UI
Imports System.Drawing
Imports System.IO
Public Class risorsesedidiverse
    Inherits System.Web.UI.Page
    Dim dtsLocalRuoli As DataSet
    Dim dtsLocal As DataSet
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Inserire qui il codice utente necessario per inizializzare la pagina
        If Page.IsPostBack = False Then
            If CaricaOLP(Session("IdEnte"), Request.QueryString("idbando")) = False Then

                lblmessaggio.Visible = True
                lblmessaggio.Text = "Non sono presenti OLP su più sedi."

            End If
            If CaricaOLPIncompatibili(Session("IdEnte"), Request.QueryString("idbando")) = False Then

                lblmessaggio.Visible = True
                lblmessaggio.Text = lblmessaggio.Text & " " & "Non sono presenti figure con ruoli incompatibili."
            End If


            lblmessaggio.Visible = True
            lblmessaggio.Text = Request.QueryString("messaggio")
            If lblmessaggio.Text <> "" Then

            End If
        End If
    End Sub
    Function CaricaOLP(ByVal strIdEnte As String, ByVal strIdBando As String) As Boolean
        Dim strsql As String
        'Dim dtsLocal As DataSet

        strsql = "select entepersonaleruoli.IDEntePersonaleRuolo, entepersonaleruoli.IDEntePersonale, entepersonale.Cognome, entepersonale.Nome, "
        strsql = strsql & "entepersonale.CodiceFiscale, COUNT(DISTINCT attivitàentisediattuazione.IDEnteSedeAttuazione) AS nsedi "
        strsql = strsql & "from attività "
        strsql = strsql & "inner join BandiAttività ON attività.IDBandoAttività = BandiAttività.IdBandoAttività "
        strsql = strsql & "inner join bando ON BANDIATTIVITà.IDBANDO = BANDO.IDBANDO "
        strsql = strsql & "inner join attivitàentisediattuazione ON attività.IDAttività = attivitàentisediattuazione.IDAttività "
        strsql = strsql & "inner join AssociaEntePersonaleRuoliAttivitàEntiSediAttuazione ON attivitàentisediattuazione.IDAttivitàEnteSedeAttuazione = AssociaEntePersonaleRuoliAttivitàEntiSediAttuazione.IdAttivitàEnteSedeAttuazione "
        strsql = strsql & "inner join entepersonaleruoli ON AssociaEntePersonaleRuoliAttivitàEntiSediAttuazione.IdEntePersonaleRuolo = entepersonaleruoli.IDEntePersonaleRuolo "
        strsql = strsql & "inner join entepersonale ON entepersonaleruoli.IDEntePersonale = entepersonale.IDEntePersonale "
        strsql = strsql & "WHERE (attività.IDEntePresentante ='" & strIdEnte & "') AND (entepersonaleruoli.IDRuolo = 1) AND (BANDO.GRUPPO = (select GRUPPO from bando where idbando = '" & strIdBando & "') ) "
        'strsql = strsql & "WHERE (attività.IDEntePresentante ='" & strIdEnte & "') AND (entepersonaleruoli.IDRuolo = 1) AND (BandiAttività.IdBando ='63') "
        strsql = strsql & "GROUP BY entepersonaleruoli.IDEntePersonaleRuolo, entepersonaleruoli.IDEntePersonale, entepersonale.Cognome, entepersonale.Nome, "
        strsql = strsql & "entepersonale.CodiceFiscale "
        strsql = strsql & "HAVING (COUNT(DISTINCT attivitàentisediattuazione.IDEnteSedeAttuazione) > 1)"

        dtsLocal = ClsServer.DataSetGenerico(strsql, Session("conn"))
        'assegno il dataset alla griglia del risultato
        dgRisultatoRicerca.DataSource = dtsLocal
        CaricaDataTablePerStampa(dtsLocal)
        Session("RisultatoRicercaOLP") = dtsLocal
        dgRisultatoRicerca.DataBind()

        If dgRisultatoRicerca.Items.Count > 0 Then
            CaricaOLP = True
            CmdEsporta.Visible = True
        Else
            CaricaOLP = False
        End If

        ' Return CaricaOLP

    End Function

    Function CaricaOLPIncompatibili(ByVal strIdEnte As String, ByVal strIdBando As String) As Boolean
        Dim strsql As String

        strsql = "SELECT  entepersonale.Cognome, entepersonale.Nome, entepersonale.CodiceFiscale, COUNT(DISTINCT entepersonaleruoli.IDRuolo) AS NRuoli,entepersonale.IdentePersonale"
        strsql = strsql & " FROM bando INNER JOIN"
        strsql = strsql & " BandiAttività ON bando.IDBando = BandiAttività.IdBando INNER JOIN"
        strsql = strsql & " attività ON BandiAttività.IdBandoAttività = attività.IDBandoAttività INNER JOIN"
        strsql = strsql & " attivitàentisediattuazione ON attività.IDAttività = attivitàentisediattuazione.IDAttività INNER JOIN"
        strsql = strsql & " AssociaEntePersonaleRuoliAttivitàEntiSediAttuazione ON "
        strsql = strsql & " attivitàentisediattuazione.IDAttivitàEnteSedeAttuazione = AssociaEntePersonaleRuoliAttivitàEntiSediAttuazione.IdAttivitàEnteSedeAttuazione INNER JOIN"
        strsql = strsql & " entepersonaleruoli ON "
        strsql = strsql & " AssociaEntePersonaleRuoliAttivitàEntiSediAttuazione.IdEntePersonaleRuolo = entepersonaleruoli.IDEntePersonaleRuolo INNER JOIN"
        strsql = strsql & " entepersonale ON entepersonaleruoli.IDEntePersonale = entepersonale.IDEntePersonale"
        strsql = strsql & " WHERE (attività.IDEntePresentante ='" & strIdEnte & "') "
        strsql = strsql & " AND (BANDO.GRUPPO = (select GRUPPO from bando where idbando = '" & strIdBando & "') ) "
        strsql = strsql & " GROUP BY entepersonale.Cognome, entepersonale.Nome, entepersonale.CodiceFiscale,entepersonale.IdentePersonale "
        strsql = strsql & " HAVING (COUNT(DISTINCT entepersonaleruoli.IDRuolo) > 1)"


        dtsLocalRuoli = ClsServer.DataSetGenerico(strsql, Session("conn"))
        'assegno il dataset alla griglia del risultato
        DtgIncompatibilitaRuoli.DataSource = dtsLocalRuoli

        CaricaDataTablePerStampaRuoli(dtsLocalRuoli)
        Session("RisultatoRicercaIncompatibili") = dtsLocalRuoli
        DtgIncompatibilitaRuoli.DataBind()



        If DtgIncompatibilitaRuoli.Items.Count > 0 Then
            CaricaOLPIncompatibili = True
            CmdEsportaCSV.Visible = True
        Else
            CaricaOLPIncompatibili = False
        End If

        'Return CaricaOLPIncompatibili

    End Function
    Private Sub dgRisultatoRicerca_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgRisultatoRicerca.PageIndexChanged
        'passo il nuovo indice selezionato all'indice della pagina da visualizzare
        dgRisultatoRicerca.CurrentPageIndex = e.NewPageIndex
        'riassegno il dataset dichiarato volutamente pubblico a tutta la pagina
        dgRisultatoRicerca.DataSource = Session("RisultatoRicercaOLP")
        dgRisultatoRicerca.DataBind()
    End Sub

    Private Sub dgRisultatoRicerca_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgRisultatoRicerca.ItemCommand
        Select Case e.CommandName
            Case "Select"
                'Response.Redirect("TabProgetti.aspx?VengoDa=" & strVengoda & "&Nazionale=" & dgRisultatoRicerca.SelectedItem.Cells(9).Text & "&Modifica=" & dgRisultatoRicerca.SelectedItem.Cells(8).Text & "&IdAttivita=" & dgRisultatoRicerca.SelectedItem.Cells(6).Text & "")
                Response.Redirect("dettagliolp.aspx?nome=" & e.Item.Cells(2).Text & "&cognome=" & e.Item.Cells(1).Text & "&codicefiscale=" & e.Item.Cells(3).Text & "&idbando=" & Request.QueryString("idbando") & "&identepersonaleruolo=" & e.Item.Cells(0).Text & "")
                'Response.Redirect("dettaglioolp.aspx?identepersonaleruolo=" & e.Item.Cells(0) & "&idbando=" & Request.QueryString("idbando") & "")
        End Select
    End Sub

    'routine che carica la datatable che caricherà dinamicamente la datagrid della stampa delle ricerche
    Sub CaricaDataTablePerStampa(ByVal DataSetDaScorrere As DataSet)
        Dim dt As New DataTable
        Dim dr As DataRow
        Dim i As Integer
        Dim x As Integer


        'nome e posizione di lettura delle colopnne a base 0
        Dim NomeColonne(3) As String
        Dim NomiCampiColonne(3) As String
        'nome della colonna 
        'e posizione nella griglia di lettura
        NomeColonne(0) = "Cognome"
        NomeColonne(1) = "Nome"
        NomeColonne(2) = "Codice Fiscale"
        NomeColonne(3) = "N° Sedi"


        NomiCampiColonne(0) = "cognome"
        NomiCampiColonne(1) = "nome"
        NomiCampiColonne(2) = "codicefiscale"
        NomiCampiColonne(3) = "nsedi"



        'carico i nomi delle colonne che andrò a stampare nella datagrid
        For x = 0 To 3
            dt.Columns.Add(New DataColumn(NomeColonne(x), GetType(String)))
        Next

        'carico il datatable con il risultato della query della ricerca, in qusto caso delle risorse
        If DataSetDaScorrere.Tables(0).Rows.Count > 0 Then
            For i = 1 To DataSetDaScorrere.Tables(0).Rows.Count
                dr = dt.NewRow()
                For x = 0 To 3
                    dr(x) = DataSetDaScorrere.Tables(0).Rows.Item(i - 1).Item(NomiCampiColonne(x))
                Next
                dt.Rows.Add(dr)
            Next
        End If

        'passo alla sessione la datatable che ho appena creato e che userò per il databinding della datagrid della stampa
        Session("DtbRicerca") = dt

    End Sub
    Sub CaricaDataTablePerStampaRuoli(ByVal DataSetDaScorrere As DataSet)
        Dim dt As New DataTable
        Dim dr As DataRow
        Dim i As Integer
        Dim x As Integer

        Dim NomeColonne(3) As String
        Dim NomiCampiColonne(3) As String

        'nome della colonna 
        'e posizione nella griglia di lettura
        NomeColonne(0) = "Cognome"
        NomeColonne(1) = "Nome"
        NomeColonne(2) = "Codice Fiscale"
        NomeColonne(3) = "N° Ruoli"

        NomiCampiColonne(0) = "Cognome"
        NomiCampiColonne(1) = "Nome"
        NomiCampiColonne(2) = "Codicefiscale"
        NomiCampiColonne(3) = "NRuoli"


        'carico i nomi delle colonne che andrò a stampare nella datagrid
        For x = 0 To 3
            dt.Columns.Add(New DataColumn(NomeColonne(x), GetType(String)))
        Next

        'carico il datatable con il risultato della query della ricerca, in qusto caso delle risorse
        If DataSetDaScorrere.Tables(0).Rows.Count > 0 Then
            For i = 1 To DataSetDaScorrere.Tables(0).Rows.Count
                dr = dt.NewRow()
                For x = 0 To 3
                    dr(x) = DataSetDaScorrere.Tables(0).Rows.Item(i - 1).Item(NomiCampiColonne(x))
                Next
                dt.Rows.Add(dr)
            Next
        End If

        'passo alla sessione la datatable che ho appena creato e che userò per il databinding della datagrid della stampa
        Session("DtbRicerca1") = dt

    End Sub
    Private Sub DtgIncompatibilitaRuoli_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles DtgIncompatibilitaRuoli.PageIndexChanged
        'passo il nuovo indice selezionato all'indice della pagina da visualizzare
        DtgIncompatibilitaRuoli.CurrentPageIndex = e.NewPageIndex
        'riassegno il dataset dichiarato volutamente pubblico a tutta la pagina
        DtgIncompatibilitaRuoli.DataSource = Session("RisultatoRicercaIncompatibili")
        DtgIncompatibilitaRuoli.DataBind()
    End Sub

    Private Sub DtgIncompatibilitaRuoli_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles DtgIncompatibilitaRuoli.ItemCommand
        Select Case e.CommandName
            Case "Select"
                Response.Redirect("dettRisorseIncompatibili.aspx?nome=" & e.Item.Cells(1).Text & "&cognome=" & e.Item.Cells(0).Text & "&codicefiscale=" & e.Item.Cells(2).Text & "&idbando=" & Request.QueryString("idbando") & "&identepersonale=" & e.Item.Cells(5).Text & "")
        End Select
    End Sub

    Protected Sub cmdChiudi_Click(sender As Object, e As EventArgs) Handles cmdChiudi.Click
        Response.Write("<script>window.close();</" + "script>")
        Response.End()
    End Sub
    Protected Sub CmdEsporta_Click(ByVal sender As Object, ByVal e As EventArgs) Handles CmdEsporta.Click
        CmdEsporta.Visible = False
        StampaCSV(Session("DtbRicerca"))

    End Sub
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
            ApriCSV1.Visible = False

        Else
            xPrefissoNome = Session("Utente")
            NomeUnivoco = xPrefissoNome & "ExpElencoGriglia" & Year(Now) & Month(Now) & Day(Now) & Hour(Now) & Minute(Now) & Second(Now)
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


    Protected Sub CmdEsportaCSV_Click(sender As Object, e As EventArgs) Handles CmdEsportaCSV.Click
        CmdEsportaCSV.Visible = False
        StampaCSV(Session("DtbRicerca1"))
    End Sub
End Class