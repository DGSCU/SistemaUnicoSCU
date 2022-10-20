Imports System.IO

Public Class WfrmVisualizzaMeseVol
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Inserire qui il codice utente necessario per inizializzare la pagina

        Dim dtrLeggiDati As SqlClient.SqlDataReader
        Dim strsql As String

        Dim dtrLeggiDati1 As SqlClient.SqlDataReader
        Dim strsql1 As String


        strsql = "SELECT DISTINCT entità.CodiceVolontario as cod, entità.Cognome as cogn, entità.Nome as Nom, attività.CodiceEnte AS codprogetto, attività.Titolo as tit, Causali.Descrizione as desci, EntitàAssenze.Giorni as gio "
        strsql = strsql & " FROM EntitàAssenze "
        strsql = strsql & " INNER JOIN entità ON EntitàAssenze.IDEntità = entità.IDEntità "
        strsql = strsql & " INNER JOIN Causali ON EntitàAssenze.IDCausale = Causali.IDCausale "
        strsql = strsql & " INNER JOIN attivitàentità ON entità.IDEntità = attivitàentità.IDEntità "
        strsql = strsql & " INNER JOIN attivitàentisediattuazione ON attivitàentità.IDAttivitàEnteSedeAttuazione = attivitàentisediattuazione.IDAttivitàEnteSedeAttuazione "
        strsql = strsql & " INNER JOIN attività ON attivitàentisediattuazione.IDAttività = attività.IDAttività"
        strsql = strsql & " INNER JOIN TipiProgetto ON Attività.IdTipoProgetto = TipiProgetto.IdTipoProgetto "
        strsql = strsql & " WHERE (EntitàAssenze.Stato in ('1','2')) AND (attività.IDEntePresentante = '" & Session("IdEnte") & "') AND (EntitàAssenze.Anno = '" & Request.QueryString("AnnoSel") & "') AND (EntitàAssenze.Mese = '" & Request.QueryString("MeseSel") & "')"
        strsql = strsql & " AND TipiProgetto.MacroTipoProgetto ='SCN' "
        strsql = strsql & " ORDER BY 2,3,1"
        dtrLeggiDati = ClsServer.CreaDatareader(strsql, Session("conn"))
        dtrLeggiDati.Read()
        lblValAnno.Text = Request.QueryString("AnnoSel")
        Select Case CInt(Request.QueryString("MeseSel"))
            Case Is = 1
                lblValMese.Text = "Gennaio"
            Case Is = 2
                lblValMese.Text = "Febbraio"
            Case Is = 3
                lblValMese.Text = "Marzo"
            Case Is = 4
                lblValMese.Text = "Aprile"
            Case Is = 5
                lblValMese.Text = "Maggio"
            Case Is = 6
                lblValMese.Text = "Giugno"
            Case Is = 7
                lblValMese.Text = "Luglio"
            Case Is = 8
                lblValMese.Text = "Agosto"
            Case Is = 9
                lblValMese.Text = "Settembre"
            Case Is = 10
                lblValMese.Text = "Ottobre"
            Case Is = 11
                lblValMese.Text = "Novembre"
            Case Is = 12
                lblValMese.Text = "Dicembre"
        End Select
        CmdEsporta.Visible = True
        'lblMese.Text = Request.QueryString("MeseSel")
        lblValEnte.Text = Session("txtCodEnte")
        lblValProg.Text = Session("Denominazione")
        If dtrLeggiDati.HasRows = True Then
            If Not dtrLeggiDati Is Nothing Then
                dtrLeggiDati.Close()
                dtrLeggiDati = Nothing
            End If
            Lblmessaggio.Visible = False
            Dim Mydataset As New DataSet
            Mydataset = ClsServer.DataSetGenerico(strsql, Session("conn"))
            DtgDettaglioMensileVolontari.DataSource = Mydataset
            DtgDettaglioMensileVolontari.DataBind()
            DtgDettaglioMensileVolontari.SelectedIndex = -1

            'blocco per la creazione della datatable per la stampa della ricerca
            'nome e posizione di lettura delle colopnne a base 0
            Dim NomeColonne(6) As String
            Dim NomiCampiColonne(6) As String
            'nome della colonna 
            'e posizione nella griglia di lettura
            NomeColonne(0) = "Cod Volontario"
            NomeColonne(1) = "Cognome"
            NomeColonne(2) = "Nome"
            NomeColonne(3) = "Causale"
            NomeColonne(4) = "N. Giorni"
            NomeColonne(5) = "Cod Progetto"
            NomeColonne(6) = "Progetto"


            NomiCampiColonne(0) = "cod"
            NomiCampiColonne(1) = "cogn"
            NomiCampiColonne(2) = "Nom"
            NomiCampiColonne(3) = "desci"
            NomiCampiColonne(4) = "gio"
            NomiCampiColonne(5) = "codprogetto"
            NomiCampiColonne(6) = "tit"
            'carico un datatable che userò poi nella pagina di stampa
            'il numero delle colonne è a base 0
            Session("DtbRicerca") = ClsServer.CaricaDataTablePerStampa(Mydataset, 6, NomeColonne, NomiCampiColonne)


            Dim i As Integer
            Dim SommaAssenze As Integer
            For i = 0 To CType(DtgDettaglioMensileVolontari.DataSource, DataSet).Tables(0).Rows.Count - 1
                SommaAssenze += CInt(CType(DtgDettaglioMensileVolontari.DataSource, DataSet).Tables(0).Rows(i).Item(6))
            Next
            lblValTotGiorni.Text = SommaAssenze.ToString

            If Not dtrLeggiDati Is Nothing Then
                dtrLeggiDati.Close()
                dtrLeggiDati = Nothing
            End If

            strsql1 = "SELECT count(distinct entità.identità) as conta"
            strsql1 = strsql1 & " FROM EntitàAssenze  "
            strsql1 = strsql1 & " INNER JOIN entità ON EntitàAssenze.IDEntità = entità.IDEntità "
            strsql1 = strsql1 & " INNER JOIN Causali ON EntitàAssenze.IDCausale = Causali.IDCausale "
            strsql1 = strsql1 & " INNER JOIN attivitàentità ON entità.IDEntità = attivitàentità.IDEntità "
            strsql1 = strsql1 & " INNER JOIN attivitàentisediattuazione ON attivitàentità.IDAttivitàEnteSedeAttuazione = attivitàentisediattuazione.IDAttivitàEnteSedeAttuazione "
            strsql1 = strsql1 & " INNER JOIN attività ON attivitàentisediattuazione.IDAttività = attività.IDAttività"
            strsql1 = strsql1 & " INNER JOIN  TipiProgetto ON Attività.IdTipoProgetto = TipiProgetto.IdTipoProgetto "
            strsql1 = strsql1 & " WHERE TipiProgetto.MacroTipoProgetto ='SCN' AND (EntitàAssenze.Stato in ('1','2')) AND (attività.IDEntePresentante = '" & Session("IdEnte") & "') AND (EntitàAssenze.Anno = '" & Request.QueryString("AnnoSel") & "') AND (EntitàAssenze.Mese = '" & Request.QueryString("MeseSel") & "')"

            dtrLeggiDati1 = ClsServer.CreaDatareader(strsql1, Session("conn"))
            dtrLeggiDati1.Read()
            lblTotValVol.Text = dtrLeggiDati1("conta")
            If Not dtrLeggiDati1 Is Nothing Then
                dtrLeggiDati1.Close()
                dtrLeggiDati1 = Nothing
            End If
        Else
            CmdEsporta.Visible = False
            Lblmessaggio.Visible = True
            lblValTotGiorni.Text = 0
            lblTotValVol.Text = 0
            If Not dtrLeggiDati Is Nothing Then
                dtrLeggiDati.Close()
                dtrLeggiDati = Nothing
            End If
            If Not dtrLeggiDati1 Is Nothing Then
                dtrLeggiDati1.Close()
                dtrLeggiDati1 = Nothing
            End If
        End If
        If Not dtrLeggiDati Is Nothing Then
            dtrLeggiDati.Close()
            dtrLeggiDati = Nothing
        End If
        If Not dtrLeggiDati1 Is Nothing Then
            dtrLeggiDati1.Close()
            dtrLeggiDati1 = Nothing
        End If

    End Sub

    Private Sub cmdChiudi_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdChiudi.Click
        Response.Write("<script>" & vbCrLf)
        Response.Write("window.close()" & vbCrLf)
        Response.Write("</script>")
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
End Class