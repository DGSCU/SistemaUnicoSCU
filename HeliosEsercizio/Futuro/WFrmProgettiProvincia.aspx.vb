Imports System.IO
Imports System.Data.SqlClient

Public Class WFrmProgettiProvincia
    Inherits System.Web.UI.Page
    Dim strsql As String
    Dim MyDataSet As DataSet

#Region "Utility"
    Private Sub ChiudiDataReader(ByRef dataReader As SqlDataReader)
        If Not dataReader Is Nothing Then
            dataReader.Close()
            dataReader = Nothing
        End If
    End Sub

    Private Sub VerificaSessione()
        If Not Session("LogIn") Is Nothing Then
            If Session("LogIn") = False Then
                Response.Redirect("LogOn.aspx")
            End If
        Else
            Response.Redirect("LogOn.aspx")
        End If
    End Sub
#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load
        VerificaSessione()
        If IsPostBack = False Then
            Session("DtbRicerca") = Nothing
            CaricaDataGrid()
        End If
    End Sub



    Private Sub CaricaDataGrid()
        strsql = "Select Attività.CodiceEnte,Attività.Titolo,StatiAttività.StatoAttività As Stato,SUM(IsNULL(AttivitàEntiSediAttuazione.NumeroPostiNoVittoNoAlloggio,0) + IsNULL(AttivitàEntiSediAttuazione.NumeroPostiVittoAlloggio,0) + IsNULL(AttivitàEntiSediAttuazione.NumeroPostiVitto,0)) As Volontari " & _
                 "From BandiAttività " & _
                 "INNER JOIN Attività ON BandiAttività.IdBandoAttività = Attività.IdBandoAttività " & _
                 "INNER JOIN StatiAttività ON Attività.IdStatoAttività = StatiAttività.IdStatoAttività " & _
                 "INNER JOIN AttivitàEntiSediAttuazione ON Attività.IdAttività = AttivitàEntiSediAttuazione.IdAttività " & _
                 "INNER JOIN EntiSediAttuazioni ON AttivitàEntiSediAttuazione.IdEnteSedeAttuazione = EntiSediAttuazioni.IdEnteSedeAttuazione " & _
                 "INNER JOIN EntiSedi ON EntiSediAttuazioni.IdEnteSede = EntiSedi.IdEnteSede " & _
                 "INNER JOIN Comuni ON EntiSedi.IdComune = Comuni.IdComune " & _
                 "WHERE BandiAttività.IdBando = " & Request.QueryString("IdBando") & " AND BandiAttività.IdEnte = " & Session("IdEnte") & " AND Comuni.IdProvincia = " & Request.QueryString("IdP") & " " & _
                 "GROUP BY Attività.CodiceEnte,Attività.Titolo,StatiAttività.StatoAttività"

        'Carico il DataSet
        MyDataSet = ClsServer.DataSetGenerico(strsql, Session("conn"))
        'Assegno il dataset alla griglia del risultato
        dgRisultatoRicerca.DataSource = MyDataSet
        dgRisultatoRicerca.DataBind()

        'nome e posizione di lettura delle colopnne a base 0
        Dim NomeColonne(3) As String
        Dim NomiCampiColonne(3) As String
        'nome della colonna 
        'e posizione nella griglia di lettura
        NomeColonne(0) = "Codice Progetto"
        NomeColonne(1) = "Titolo"
        NomeColonne(2) = "Stato"
        NomeColonne(3) = "Volontari"

        NomiCampiColonne(0) = "CodiceEnte"
        NomiCampiColonne(1) = "Titolo"
        NomiCampiColonne(2) = "Stato"
        NomiCampiColonne(3) = "Volontari"

        'carico un datatable che userò poi nella pagina di stampa
        'il numero delle colonne è a base 0
        CaricaDataTablePerStampa(MyDataSet, 3, NomeColonne, NomiCampiColonne)
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
    Protected Sub cmdEsporta_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdEsporta.Click
        cmdEsporta.Visible = False
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


End Class
