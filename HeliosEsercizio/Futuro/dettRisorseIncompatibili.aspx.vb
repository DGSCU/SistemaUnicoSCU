Imports System.IO

Public Class dettRisorseIncompatibili
    Inherits System.Web.UI.Page

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load
        'Inserire qui il codice utente necessario per inizializzare la pagina
        If Page.IsPostBack = False Then
            CaricaDettagli()
        End If
    End Sub
    Sub CaricaDettagli()
        Dim strsql As String
        Dim dtsLocal As DataSet

        'carico i dati nelle label
        lblNome.Text = Request.QueryString("nome")
        lblCognome.Text = Request.QueryString("cognome")
        lblCodiceFiscale.Text = Request.QueryString("codicefiscale")


        'preparo la query
        strsql = "SELECT DISTINCT attività.Titolo, attività.CodiceEnte, entisedi.Denominazione, " & _
                " entisediattuazioni.IDEnteSedeAttuazione AS codicesedeente, comuni.Denominazione AS comune, " & _
                " entepersonaleruoli.IDEntePersonaleRuolo, entisedi.Indirizzo, ruoli.descrabb" & _
                " FROM entepersonaleruoli " & _
                " INNER JOIN AssociaEntePersonaleRuoliAttivitàEntiSediAttuazione ON entepersonaleruoli.IDEntePersonaleRuolo = AssociaEntePersonaleRuoliAttivitàEntiSediAttuazione.IdEntePersonaleRuolo " & _
                " INNER JOIN attivitàentisediattuazione ON AssociaEntePersonaleRuoliAttivitàEntiSediAttuazione.IdAttivitàEnteSedeAttuazione = attivitàentisediattuazione.IDAttivitàEnteSedeAttuazione " & _
                " INNER JOIN attività ON attivitàentisediattuazione.IDAttività = attività.IDAttività " & _
                " inner join BandiAttività on  BandiAttività.IdBandoAttività = attività.IDBandoAttività" & _
                " inner join bando  ON bando.IDBando = BandiAttività.IdBando" & _
                " INNER JOIN entisediattuazioni ON attivitàentisediattuazione.IDEnteSedeAttuazione = entisediattuazioni.IDEnteSedeAttuazione " & _
                " INNER JOIN entisedi ON entisediattuazioni.IDEnteSede = entisedi.IDEnteSede " & _
                " INNER JOIN comuni ON entisedi.IDComune = comuni.IDComune  " & _
                " INNER JOIN Ruoli on entepersonaleruoli.Idruolo =ruoli.idruolo " & _
                " WHERE entepersonaleruoli.identepersonale='" & Request.QueryString("IDEntePersonale") & "'" & _
                " AND (BANDO.GRUPPO = (select GRUPPO from bando where idbando ='" & Request.QueryString("IdBando") & "'))"
        '"WHERE entepersonaleruoli.identepersonaleruolo='" & Request.QueryString("IDEntePersonaleRuolo") & "'"

        dtsLocal = ClsServer.DataSetGenerico(strsql, Session("conn"))
        'assegno il dataset alla griglia del risultato
        dgRisultatoRicerca.DataSource = dtsLocal
        Session("RisultatoRicercaDettaglioOLP") = dtsLocal
        dgRisultatoRicerca.DataBind()

        '*********************************************************************************
        'blocco per la creazione della datatable per la stampa della ricerca

        'nome e posizione di lettura delle colopnne a base 0
        Dim NomeColonne(5) As String
        Dim NomiCampiColonne(5) As String
        'nome della colonna 
        'e posizione nella griglia di lettura
        NomeColonne(0) = "Codice Progetto"
        NomeColonne(1) = "Progetto"
        NomeColonne(2) = "Codice Sede"
        NomeColonne(3) = "Comune"
        NomeColonne(4) = "Indirizzo"
        NomeColonne(5) = "Ruolo"

        NomiCampiColonne(0) = "codiceente"
        NomiCampiColonne(1) = "titolo"
        NomiCampiColonne(2) = "codicesedeente"
        NomiCampiColonne(3) = "comune"
        NomiCampiColonne(4) = "indirizzo"
        NomiCampiColonne(5) = "descrabb"

        'carico un datatable che userò poi nella pagina di stampa
        'il numero delle colonne è a base 0
        CaricaDataTablePerStampa(dtsLocal, 5, NomeColonne, NomiCampiColonne)

        '*********************************************************************************

    End Sub


    Private Sub dgRisultatoRicerca_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgRisultatoRicerca.PageIndexChanged
        'passo il nuovo indice selezionato all'indice della pagina da visualizzare
        dgRisultatoRicerca.CurrentPageIndex = e.NewPageIndex
        'riassegno il dataset dichiarato volutamente pubblico a tutta la pagina
        dgRisultatoRicerca.DataSource = Session("RisultatoRicercaDettaglioOLP")
        dgRisultatoRicerca.DataBind()
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

    End Sub
    Private Sub StampaCSV(ByVal dtbRicerca As DataTable)
        Dim path As String
        Dim xPrefissoNome As String
        Dim url As String
        Dim utility As ClsUtility = New ClsUtility()

        If dtbRicerca.Rows.Count = 0 Then
            ApriCSV1.Visible = False
            cmdEsporta.Visible = False
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
        writer.WriteLine(xLinea)
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

            writer.WriteLine(xLinea)
            xLinea = vbNullString

        Next
        url = "download\" & nomeUnivoco & ".CSV"

        writer.Close()
        writer = Nothing
        Return url
    End Function

    Protected Sub cmdEsporta_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdEsporta.Click
        cmdEsporta.Visible = False
        Dim dtbRicerca As DataTable = Session("DtbRicerca")
        StampaCSV(dtbRicerca)
    End Sub
End Class