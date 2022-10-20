Imports System.Data.SqlClient
Imports System.IO

Public Class WFrmControlloProvincie
    Inherits System.Web.UI.Page

    Dim strsql As String
    Dim dataSet As DataSet
    Dim dtrgenerico As SqlClient.SqlDataReader
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

    Protected Sub cmdChiudi_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdChiudi.Click
        If Request.QueryString("VengoDa") = "70" Then
            Response.Redirect("WfrmProgrammiIstanza.aspx?id=" & Request.QueryString("idBando") & "&VengoDa=" & 70 & "&DataFine=" & Request.QueryString("DataFine") & "&DataInizio=" & Request.QueryString("DataInizio") & "&Verso=" & Request.QueryString("Verso") & "&Stato=" & Request.QueryString("Stato") & "&Arrivo=" & Request.QueryString("Arrivo") & "&IdBP=" & Request.QueryString("IdBP"))
         End If

        If Request.QueryString("VengoDa") = "8" Then
            Response.Redirect("WfrmIstanzaPresentazione.aspx?id=" & Request.QueryString("idBando") & "&VengoDa=" & 8 & "&DataFine=" & Request.QueryString("DataFine") & "&DataInizio=" & Request.QueryString("DataInizio") & "&Verso=" & Request.QueryString("Verso") & "&Stato=" & Request.QueryString("Stato") & "&Arrivo=" & Request.QueryString("Arrivo") & "&idBA=" & Request.QueryString("idBA"))
        Else
            Response.Redirect("WebGestioneSediProgettoOlp.aspx?EnteCapoFila=" & Request.QueryString("EnteCapoFila") & "&CoProgettato=" & Request.QueryString("CoProgettato") & "&Nazionale=" & Request.QueryString("Nazionale") & "&Modifica=" & Request.QueryString("Modifica") & "&IdAttivita=" & Request.QueryString("IdAttivita"))
        End If

    End Sub

    Private Sub dgRisultatoRicerca_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgRisultatoRicerca.ItemCommand
        'Carico la maschera per la visualizzazione dei progetti su quella provincia        
        If e.CommandName = "Select" Then
            Response.Write("<script>")
            Response.Write("window.open(""WFrmProgettiProvincia.aspx?IdBando=" & Request.QueryString("IdBando") & "&IdP=" & e.Item.Cells(0).Text & """, """", ""width=650,height=300,toolbar=no,location=no,menubar=no,scrollbars=yes"")")
            Response.Write("</script>")
        End If

    End Sub

    Private Sub CaricaDataGrid()
        Dim MyQuery As New System.Collections.ArrayList
        Dim blnVisTutor As Boolean
        Dim blnVisRlea As Boolean
        blnVisTutor = VisualizzaTutor(Request.QueryString("Idbando"))
        If Not Request.QueryString("IdBP") Is Nothing Then
            blnVisRlea = False
        Else
            blnVisRlea = False 'rlea sempre non visibile
        End If


        MyQuery.Add("CREATE TABLE #CONTROLLOPROVINCIE (" & _
                    "IdProvincia INT, Provincia VARCHAR(100), NVol INT, NRleaRic INT, NTutorRic INT, NRleaIns INT, NTutorIns INT)")

        'Inserimento Dei Dati Nella Tabella
        strsql = "Insert Into #CONTROLLOPROVINCIE(IdProvincia,Provincia,NVol,NRleaRic,NTutorRic) " & _
                 "SELECT  P.IdProvincia,P.Provincia,Sum(IsNull(AttivitàEntiSediAttuazione.NumeroPostiNoVittoNoAlloggio,0) + IsNull(AttivitàEntiSediAttuazione.NumeroPostiVittoAlloggio,0)+ IsNull(AttivitàEntiSediAttuazione.NumeroPostiVitto,0)) NVol," & _
                 "Case When Sum(IsNull(AttivitàEntiSediAttuazione.NumeroPostiNoVittoNoAlloggio,0) + IsNull(AttivitàEntiSediAttuazione.NumeroPostiVittoAlloggio,0)+ IsNull(AttivitàEntiSediAttuazione.NumeroPostiVitto,0)) >= 30 Then 1 Else 0 End RLEARic," & _
                 "Sum(IsNull(AttivitàEntiSediAttuazione.NumeroPostiNoVittoNoAlloggio,0) + IsNull(AttivitàEntiSediAttuazione.NumeroPostiVittoAlloggio,0)+ IsNull(AttivitàEntiSediAttuazione.NumeroPostiVitto,0))/30 TutorRic " & _
                 "FROM bando " & _
                 "INNER JOIN BandiAttività ON bando.IDBando = BandiAttività.IdBando " & _
                 "INNER JOIN Attività ON BandiAttività.IdBandoAttività = Attività.IDBandoAttività " & _
                 "INNER JOIN AttivitàEntiSediAttuazione ON Attività.IDAttività = AttivitàEntiSediAttuazione.IDAttività " & _
                 "INNER JOIN EntiSediAttuazioni ON AttivitàEntiSediAttuazione.IDEnteSedeAttuazione = entisediattuazioni.IDEnteSedeAttuazione " & _
                 "INNER JOIN EntiSedi ON EntiSediAttuazioni.IDEnteSede = EntiSedi.IDEnteSede " & _
                 "INNER JOIN Comuni ON EntiSedi.IDComune = Comuni.IDComune " & _
                 "INNER JOIN Provincie P ON Comuni.IDProvincia = P.IDProvincia " & _
                 "INNER JOIN REGIONI R on P.IdRegione = R.IdRegione " & _
                 "INNER JOIN NAZIONI N ON R.IDNAZIONE = N.IDNAZIONE " & _
                 "WHERE attività.idtipoprogetto<>2 and Attività.IdEntePresentante = " & Session("Idente") & " And Bando.IDBando = " & Request.QueryString("Idbando") & " AND N.NAZIONEBASE = 1  and entisediattuazioni.identecapofila = bandiattività.idente " & _
                 "Group by P.IdProvincia,P.Provincia "
        MyQuery.Add(strsql)

        'Metto gli elementi RLEA inseriti nelle varie provincie
        strsql = "Select COUNT(Distinct Entepersonale.IDEntePersonale) As NRLEA ,Comuni.IdProvincia " & _
                 "Into #TmpRleaIns " & _
                 "FROM EntePersonale " & _
                 "INNER JOIN EntePersonaleRuoli ON EntePersonaleRuoli.IDEntePersonale = Entepersonale.IDEntePersonale " & _
                 "INNER JOIN AssociaEntePersonaleRuoliAttivitàEntiSediAttuazione a ON a.IdEntePersonaleRuolo = EntePersonaleRuoli.IDEntePersonaleRuolo " & _
                 "INNER JOIN AttivitàEntiSediAttuazione ON AttivitàEntiSediAttuazione.IDAttivitàEnteSedeAttuazione = a.IdAttivitàEnteSedeAttuazione " & _
                 "INNER JOIN Attività ON AttivitàEntiSediAttuazione.IdAttività = Attività.IdAttività " & _
                 "INNER JOIN BandiAttività ON Attività.IdBandoAttività = BandiAttività.IdBandoAttività " & _
                 "INNER JOIN EntiSediAttuazioni ON AttivitàEntiSediAttuazione.IDEnteSedeAttuazione = EntiSediAttuazioni.IDEnteSedeAttuazione " & _
                 "INNER JOIN EntiSedi ON EntiSediAttuazioni.IDEnteSede = EntiSedi.IDEnteSede " & _
                 "INNER JOIN Comuni ON Entisedi.IDComune = Comuni.IDComune " & _
                 "WHERE BandiAttività.IdBando = " & Request.QueryString("Idbando") & " AND BandiAttività.IDEnte = " & Session("Idente") & " And EntePersonale.DataFineValidità IS NULL " & _
                 "AND EntePersonaleRuoli.datafinevalidità is null and EntePersonaleRuoli.IDRuolo = 6 AND (EntePersonaleRuoli.Accreditato = 1 OR EntePersonaleRuoli.Accreditato = 0) " & _
                 "Group By Comuni.IdProvincia"
        MyQuery.Add(strsql)

        'Metto gli elementi RLEA inseriti nelle varie provincie
        strsql = "Select COUNT(Distinct Entepersonale.IDEntePersonale) As NTUTOR ,Comuni.IdProvincia " & _
                 "Into #TmpTutorIns " & _
                 "FROM EntePersonale " & _
                 "INNER JOIN EntePersonaleRuoli ON EntePersonaleRuoli.IDEntePersonale = Entepersonale.IDEntePersonale " & _
                 "INNER JOIN AssociaEntePersonaleRuoliAttivitàEntiSediAttuazione a ON a.IdEntePersonaleRuolo = EntePersonaleRuoli.IDEntePersonaleRuolo " & _
                 "INNER JOIN AttivitàEntiSediAttuazione ON AttivitàEntiSediAttuazione.IDAttivitàEnteSedeAttuazione = a.IdAttivitàEnteSedeAttuazione " & _
                 "INNER JOIN Attività ON AttivitàEntiSediAttuazione.IdAttività = Attività.IdAttività " & _
                 "INNER JOIN BandiAttività ON Attività.IdBandoAttività = BandiAttività.IdBandoAttività " & _
                 "INNER JOIN EntiSediAttuazioni ON AttivitàEntiSediAttuazione.IDEnteSedeAttuazione = EntiSediAttuazioni.IDEnteSedeAttuazione " & _
                 "INNER JOIN EntiSedi ON EntiSediAttuazioni.IDEnteSede = EntiSedi.IDEnteSede " & _
                 "INNER JOIN Comuni ON Entisedi.IDComune = Comuni.IDComune " & _
                 "WHERE BandiAttività.IdBando = " & Request.QueryString("Idbando") & " AND BandiAttività.IDEnte = " & Session("Idente") & " And EntePersonale.DataFineValidità IS NULL " & _
                 "AND EntePersonaleRuoli.datafinevalidità is null and EntePersonaleRuoli.IDRuolo = 5 AND (EntePersonaleRuoli.Accreditato = 1 OR EntePersonaleRuoli.Accreditato = 0) " & _
                 "Group By Comuni.IdProvincia"
        MyQuery.Add(strsql)

        'Aggiornamento del numero degli RLEA
        strsql = "UpDate #CONTROLLOPROVINCIE Set #CONTROLLOPROVINCIE.NRleaIns = #TmpRleaIns.NRLEA " & _
                 "From #CONTROLLOPROVINCIE " & _
                 "INNER JOIN #TmpRleaIns ON #CONTROLLOPROVINCIE.IdProvincia = #TmpRleaIns.IdProvincia"
        MyQuery.Add(strsql)

        'Aggiornamento del numero di TUTOR
        strsql = "UpDate #CONTROLLOPROVINCIE Set #CONTROLLOPROVINCIE.NTutorIns = #TmpTutorIns.NTUTOR " & _
                 "From #CONTROLLOPROVINCIE " & _
                 "INNER JOIN #TmpTutorIns ON #CONTROLLOPROVINCIE.IdProvincia = #TmpTutorIns.IdProvincia"
        MyQuery.Add(strsql)

        'Imposto a zero tutti gli elementi con null
        MyQuery.Add("Update #CONTROLLOPROVINCIE Set NRleaIns = 0 Where NRleaIns Is Null")
        MyQuery.Add("Update #CONTROLLOPROVINCIE Set NTutorIns = 0 Where NTutorIns Is Null")

        If ClsServer.EseguiQueryColl(MyQuery, Session.SessionID, Session("Conn")) = True Then
            strsql = "Select * From #CONTROLLOPROVINCIE"
            If Request.QueryString("Messaggio") <> "" Then
                strsql = strsql & " Where NRleaRic > NRleaIns Or NTutorRic > NTutorIns"
                lblmessaggio.Text = Request.QueryString("Messaggio")
            End If
            strsql = strsql & " Order By Provincia"


            'Carico il DataSet
            dataSet = ClsServer.DataSetGenerico(strsql, Session("conn"))
            'Elimino le Tabelle di Appoggio
            ClsServer.EseguiSqlClient("DROP TABLE #CONTROLLOPROVINCIE", Session("Conn"))
            ClsServer.EseguiSqlClient("DROP TABLE #TmpRleaIns", Session("Conn"))
            ClsServer.EseguiSqlClient("DROP TABLE #TmpTutorIns", Session("Conn"))

            'Assegno il dataset alla griglia del risultato
            dgRisultatoRicerca.DataSource = dataSet
            Session("appDtsRisRicerca") = dataSet
            dgRisultatoRicerca.DataBind()

            'nome e posizione di lettura delle colopnne a base 0
            Dim NomeColonne(5) As String
            Dim NomiCampiColonne(5) As String
            'nome della colonna 
            'e posizione nella griglia di lettura
            NomeColonne(0) = "Provincia"
            NomeColonne(1) = "Num Volontari"
            If blnVisRlea = True Then
                NomeColonne(2) = "Num RLEA Ric."
                NomeColonne(3) = "Num RLEA Ins."
            End If
            If blnVisTutor = True Then
                NomeColonne(4) = "Num Tutor Ric."
                NomeColonne(5) = "Num Tutor Ins."
            End If
            NomiCampiColonne(0) = "Provincia"
            NomiCampiColonne(1) = "NVol"
            If blnVisRlea = True Then
                NomiCampiColonne(2) = "NRleaRic"
                NomiCampiColonne(3) = "NRleaIns"
            End If
            If blnVisTutor = True Then
                NomiCampiColonne(4) = "NTutorRic"
                NomiCampiColonne(5) = "NTutorIns"
            End If

            'carico un datatable che userò poi nella pagina di stampa
            'il numero delle colonne è a base 0
            If blnVisTutor = True Then
                CaricaDataTablePerStampa(dataSet, 5, NomeColonne, NomiCampiColonne)
            Else
                If blnVisRlea Then
                    CaricaDataTablePerStampa(dataSet, 3, NomeColonne, NomiCampiColonne)
                Else
                    CaricaDataTablePerStampa(dataSet, 1, NomeColonne, NomiCampiColonne)
                End If

            End If


            If blnVisRlea = True Then
                Call ColoraCelle()
            End If

        Else
            'lblmessaggio.Text = "Si sono verificati problemi durante l'accesso ai dati."
            Response.Write("<SCRIPT>" & vbCrLf)
            Response.Write("alert('Si sono verificati problemi durante l\'accesso ai dati')" & vbCrLf)
            Response.Write("self.close()" & vbCrLf)
            Response.Write("</SCRIPT>")
        End If

        ChiudiDataReader(dtrgenerico)

        If blnVisTutor = True Then
            dgRisultatoRicerca.Columns(5).Visible = True
            dgRisultatoRicerca.Columns(6).Visible = True
        Else
            dgRisultatoRicerca.Columns(5).Visible = False
            dgRisultatoRicerca.Columns(6).Visible = False
        End If

        If blnVisRlea = True Then
            dgRisultatoRicerca.Columns(3).Visible = True
            dgRisultatoRicerca.Columns(4).Visible = True
        Else
            dgRisultatoRicerca.Columns(3).Visible = False
            dgRisultatoRicerca.Columns(4).Visible = False
        End If


    End Sub
    Private Function VisualizzaTutor(ByVal IdBando As Integer) As Boolean
        'Controllo il valore del flag VisualizzaTutor dentro la tabella Bando
        Dim blnVisTutor As Boolean
        strsql = "select visualizzatutor from bando where IDBando = " & IdBando
        dtrgenerico = ClsServer.CreaDatareader(strsql, Session("conn"))
        dtrgenerico.Read()
        blnVisTutor = dtrgenerico("visualizzatutor")
        ChiudiDataReader(dtrgenerico)
        Return blnVisTutor

    End Function

    'routine che carica la datatable che caricherà dinamicamente la datagrid della stampa delle ricerche
    Sub CaricaDataTablePerStampa(ByVal dataSet As DataSet, ByVal numeroColonne As Integer, ByVal nomiColonne() As String, ByVal nomiCampiColonne() As String)
        Dim dt As New DataTable
        Dim dr As DataRow
        Dim i As Integer
        Dim x As Integer

        'carico i nomi delle colonne che andrò a stampare nella datagrid
        For x = 0 To numeroColonne
            dt.Columns.Add(New DataColumn(nomiColonne(x), GetType(String)))
        Next

        'carico il datatable con il risultato della query della ricerca, in qusto caso delle risorse
        If dataSet.Tables(0).Rows.Count > 0 Then
            For i = 1 To dataSet.Tables(0).Rows.Count
                dr = dt.NewRow()
                For x = 0 To numeroColonne
                    dr(x) = dataSet.Tables(0).Rows.Item(i - 1).Item(nomiCampiColonne(x))
                Next
                dt.Rows.Add(dr)
            Next
        End If

        'passo alla sessione la datatable che ho appena creato e che userò per il databinding della datagrid della stampa
        Session("DtbRicerca") = dt

    End Sub

    Private Sub ColoraCelle()
        Dim item As DataGridItem
        Dim color As New System.Drawing.Color
        Dim x As Integer

        For Each item In dgRisultatoRicerca.Items
            'controllo difformità rlea
            If CInt(dgRisultatoRicerca.Items(item.ItemIndex).Cells(3).Text) > CInt(dgRisultatoRicerca.Items(item.ItemIndex).Cells(4).Text) Then
                For x = 0 To 7
                    color = Drawing.Color.Khaki
                    dgRisultatoRicerca.Items(item.ItemIndex).Cells(x).BackColor = color
                Next
            End If

            'controllo difformità tutor
            If CInt(dgRisultatoRicerca.Items(item.ItemIndex).Cells(5).Text) > CInt(dgRisultatoRicerca.Items(item.ItemIndex).Cells(6).Text) Then
                For x = 0 To 7
                    color = Drawing.Color.Khaki
                    dgRisultatoRicerca.Items(item.ItemIndex).Cells(x).BackColor = color
                Next
            End If

        Next

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

    Private Sub dgRisultatoRicerca_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgRisultatoRicerca.PageIndexChanged
        'utilizzo la session per memorizzare il dataset generato al momento della ricerca
        Call CaricaDataGrid()
        dgRisultatoRicerca.CurrentPageIndex = e.NewPageIndex
        dgRisultatoRicerca.DataSource = Session("appDtsRisRicerca")
        dgRisultatoRicerca.DataBind()
        dgRisultatoRicerca.SelectedIndex = -1

    End Sub
End Class
