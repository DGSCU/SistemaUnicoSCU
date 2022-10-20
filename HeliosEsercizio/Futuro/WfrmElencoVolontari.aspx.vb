Imports System.IO

Public Class WfrmElencoVolontari
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        '***Generata da Gianluigi Paesani in data:05/07/04
        'Inserire qui il codice utente necessario per inizializzare la pagina
        If Not Session("LogIn") Is Nothing Then
            If Session("LogIn") = False Then 'verifico validità log-in
                Response.Redirect("LogOn.aspx")
            End If
        Else
            Response.Redirect("LogOn.aspx")
        End If

        Call ricerca()

    End Sub

    Sub ricerca()
        '***Eseguo comando di ricerca
        Dim Mydataset As New DataSet
        Dim strquery As String

        dgRisultatoRicerca.CurrentPageIndex = 0
        If Request.QueryString("Spec") <> 1 Then


            strquery = "SELECT c.idattività,a.IDEntità, a.Cognome + ' ' + a.Nome as Nominativo, a.CodiceFiscale, "
            strquery = strquery & "case len(day(a.DataInizioServizio)) when 1 then '0' + convert(varchar(20),day(a.DataInizioServizio)) "
            strquery = strquery & "else convert(varchar(20),day(a.DataInizioServizio))  end + '/' + "
            strquery = strquery & "(case len(month(a.DataInizioServizio)) when 1 then '0' + convert(varchar(20),month(a.DataInizioServizio)) "
            strquery = strquery & "else convert(varchar(20),month(a.DataInizioServizio))  end + '/' + "
            strquery = strquery & "Convert(varchar(20), Year(a.DataInizioServizio))) as DataInizioServizio,  "
            strquery = strquery & "case len(day(a.DataFineServizio)) when 1 then '0' + convert(varchar(20),day(a.DataFineServizio)) "
            strquery = strquery & "else convert(varchar(20),day(a.DataFineServizio))  end + '/' + "
            strquery = strquery & "(case len(month(a.DataFineServizio)) when 1 then '0' + convert(varchar(20),month(a.DataFineServizio)) "
            strquery = strquery & "else convert(varchar(20),month(a.DataFineServizio))  end + '/' + "
            strquery = strquery & "Convert(varchar(20), Year(a.DataFineServizio))) as DataFineServizio,  "
            strquery = strquery & "d.denominazione as DenominazioneSedeAttuazione, d.IdEnteSedeAttuazione, e.denominazione as Sede, e.IdEnteSede, "
            strquery = strquery & "max(datafineattivitàentità),(select count(*)-1 "
            strquery = strquery & "from attivitàentità where identità =a.identità) as numStorici, isnull(a.OreFormazione,0) as OreFormazione  "
            strquery = strquery & "FROM entità as a "
            strquery = strquery & "inner join attivitàentità as b on a.identità=b.identità "
            strquery = strquery & "inner join attivitàentisediattuazione as c on b.idattivitàentesedeattuazione=c.idattivitàentesedeattuazione "
            strquery = strquery & "inner join entisediattuazioni as d on c.identesedeattuazione=d.identesedeattuazione "
            strquery = strquery & "inner join entisedi as e on d.identesede=e.identesede "
            ''strquery = strquery & "inner join StatiAttivitàEntità as f on b.IdStatoAttivitàEntità=f.IdStatoAttivitàEntità "
            strquery = strquery & "inner join StatiEntità as g on a.IDStatoEntità=g.IDStatoEntità "
            strquery = strquery & "where  c.idattività=" & CInt(Request.QueryString("IdAttivita")) & " And (g.InServizio = 1 OR g.Sospeso = 1 OR g.Chiuso = 1) AND b.EscludiFormazione=0 "
            strquery = strquery & "group by c.idattività,a.IDEntità, a.Cognome + ' ' + a.Nome, "
            strquery = strquery & "a.CodiceFiscale, a.DataInizioServizio, a.DataFineServizio, d.denominazione , "
            strquery = strquery & "e.denominazione, a.OreFormazione, d.IdEnteSedeAttuazione, e.IdEnteSede "
            'strquery = strquery & "having max(datafineattivitàentità)= (select max(datafineattivitàentità) "
            'strquery = strquery & "from attivitàentità "
            'strquery = strquery & "inner join attivitàentisediattuazione on attivitàentità.IDAttivitàEnteSedeAttuazione=attivitàentisediattuazione.IDAttivitàEnteSedeAttuazione "
            'strquery = strquery & "where identità =a.identità and attivitàentisediattuazione.idattività = c.idattività) "
            strquery = strquery & "Order by a.Cognome + ' ' + a.Nome"
            'f.DefaultStato=1 and
        Else

            strquery = "SELECT c.idattività,a.IDEntità, a.Cognome + ' ' + a.Nome as Nominativo, a.CodiceFiscale, "
            strquery = strquery & "case len(day(a.DataInizioServizio)) when 1 then '0' + convert(varchar(20),day(a.DataInizioServizio)) "
            strquery = strquery & "else convert(varchar(20),day(a.DataInizioServizio))  end + '/' + "
            strquery = strquery & "(case len(month(a.DataInizioServizio)) when 1 then '0' + convert(varchar(20),month(a.DataInizioServizio)) "
            strquery = strquery & "else convert(varchar(20),month(a.DataInizioServizio))  end + '/' + "
            strquery = strquery & "Convert(varchar(20), Year(a.DataInizioServizio))) as DataInizioServizio,  "
            strquery = strquery & "case len(day(a.DataFineServizio)) when 1 then '0' + convert(varchar(20),day(a.DataFineServizio)) "
            strquery = strquery & "else convert(varchar(20),day(a.DataFineServizio))  end + '/' + "
            strquery = strquery & "(case len(month(a.DataFineServizio)) when 1 then '0' + convert(varchar(20),month(a.DataFineServizio)) "
            strquery = strquery & "else convert(varchar(20),month(a.DataFineServizio))  end + '/' + "
            strquery = strquery & "Convert(varchar(20), Year(a.DataFineServizio))) as DataFineServizio,  "
            strquery = strquery & "d.denominazione as DenominazioneSedeAttuazione, d.IdEnteSedeAttuazione, e.denominazione as Sede, e.IdEnteSede, "
            strquery = strquery & "max(datafineattivitàentità),(select count(*)-1 "
            strquery = strquery & "from attivitàentità where identità =a.identità) as numStorici, isnull(a.oreformazionespecifica,0) as OreFormazione  "
            strquery = strquery & "FROM entità as a "
            strquery = strquery & "inner join attivitàentità as b on a.identità=b.identità "
            strquery = strquery & "inner join attivitàentisediattuazione as c on b.idattivitàentesedeattuazione=c.idattivitàentesedeattuazione "
            strquery = strquery & "inner join entisediattuazioni as d on c.identesedeattuazione=d.identesedeattuazione "
            strquery = strquery & "inner join entisedi as e on d.identesede=e.identesede "
            ''strquery = strquery & "inner join StatiAttivitàEntità as f on b.IdStatoAttivitàEntità=f.IdStatoAttivitàEntità "
            strquery = strquery & "inner join StatiEntità as g on a.IDStatoEntità=g.IDStatoEntità "
            strquery = strquery & "where  c.idattività=" & CInt(Request.QueryString("IdAttivita")) & " And (g.InServizio = 1 OR g.Sospeso = 1 OR g.Chiuso = 1) AND b.EscludiFormazione=0 "
            strquery = strquery & "group by c.idattività,a.IDEntità, a.Cognome + ' ' + a.Nome, "
            strquery = strquery & "a.CodiceFiscale, a.DataInizioServizio, a.DataFineServizio, d.denominazione , "
            strquery = strquery & "e.denominazione, a.oreformazionespecifica, d.IdEnteSedeAttuazione, e.IdEnteSede "
            'strquery = strquery & "having max(datafineattivitàentità)= (select max(datafineattivitàentità) "
            'strquery = strquery & "from attivitàentità "
            'strquery = strquery & "inner join attivitàentisediattuazione on attivitàentità.IDAttivitàEnteSedeAttuazione=attivitàentisediattuazione.IDAttivitàEnteSedeAttuazione "
            'strquery = strquery & "where identità =a.identità and attivitàentisediattuazione.idattività = c.idattività) "
            strquery = strquery & "Order by a.Cognome + ' ' + a.Nome"
            'f.DefaultStato=1 and


        End If

        Mydataset = ClsServer.DataSetGenerico(strquery, Session("conn"))
        dgRisultatoRicerca.DataSource = Mydataset
        dgRisultatoRicerca.DataBind() 'valorizzo griglia

        Session("LocalDataSet") = Mydataset

        'nome e posizione di lettura delle colopnne a base 0
        Dim NomeColonne(6) As String
        Dim NomiCampiColonne(6) As String
        'nome della colonna 
        'e posizione nella griglia di lettura
        NomeColonne(0) = "Nominativo"
        NomeColonne(1) = "Codice fiscale"
        If Request.QueryString("Spec") <> 1 Then
            NomeColonne(2) = "Ore formazione"
        Else
            NomeColonne(2) = "Ore formazione Specifica"
        End If
        NomeColonne(3) = "Data inizio servizio"
        NomeColonne(4) = "Data fine servizio"
        NomeColonne(5) = "Sede attuazione"
        NomeColonne(6) = "Sede"


        NomiCampiColonne(0) = "Nominativo"
        NomiCampiColonne(1) = "Codicefiscale"
        NomiCampiColonne(2) = "Oreformazione"
        NomiCampiColonne(3) = "Datainizioservizio"
        NomiCampiColonne(4) = "Datafineservizio"
        NomiCampiColonne(5) = "DenominazioneSedeAttuazione"
        NomiCampiColonne(6) = "Sede"

        'carico un datatable che userò poi nella pagina di stampa
        'il numero delle colonne è a base 0
        CaricaDataTablePerStampa(Mydataset, 6, NomeColonne, NomiCampiColonne)

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

    Private Sub dgRisultatoRicerca_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgRisultatoRicerca.PageIndexChanged
        'passo il nuovo indice selezionato all'indice della pagina da visualizzare
        dgRisultatoRicerca.CurrentPageIndex = e.NewPageIndex
        'riassegno il dataset dichiarato volutamente pubblico a tutta la pagina
        dgRisultatoRicerca.DataSource = Session("LocalDataSet")
        dgRisultatoRicerca.DataBind()
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