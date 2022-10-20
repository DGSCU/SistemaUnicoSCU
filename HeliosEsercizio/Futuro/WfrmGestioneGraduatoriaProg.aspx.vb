Public Class WfrmGestioneGraduatoriaProg
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Inserire qui il codice utente necessario per inizializzare la pagina
        '***Generata da Gianluigi Paesani in data:24/08/04
        If Not Session("LogIn") Is Nothing Then 'verifico validità log-in
            If Session("LogIn") = False Then
                Response.Redirect("LogOn.aspx")
            End If
        Else
            Response.Redirect("LogOn.aspx")
        End If
        If IsPostBack = False Then 'richiamo procedura per valorizzazione griglia
            EseguiCalcoloGriglia(Request.QueryString("matrix"))
        End If
    End Sub

    Private Function EseguiCalcoloGriglia(ByVal matrix As String)
        '***Generata da Gianluigi Paesani in data:24/08/04
        '***questa routine valorizza la griglia a seconda 
        '***la produzione o la conferma - annullamento  
        '***della graduatoria
        Dim totale As Double
        Dim mydataset As DataSet
        Dim Dtrgenerico As Data.SqlClient.SqlDataReader
        Dim strquery As String
        Dim i As Integer
        'If bytVerifica = 1 Then dgRisultatoRicerca.CurrentPageIndex = bytpage
        Session("GrigliaBando") = Nothing 'pulisco session
        If Request.QueryString("tipo") = "Produzione" Then
            CmdConferma.Visible = False
            CmdAnnulla.Visible = False
            CmdSalva.Visible = True
            'eseguo query per produzione
            lbltitolo.Text = "Lista Progetti per Graduatoria Bando: " & Request.QueryString("Name")
            strquery = "select a.idattività,a.titolo,a.Importo," & _
            " c.idbando,g.codifica + '-' + g.macroambitoattività + ' \ ' + f.codifica + '-' + f.ambitoattività as Ambito," & _
            " c.importostanziato,d.idente,d.denominazione, '0' as idgraduatoriaprogetto,a.punteggiofinale," & _
            " (SELECT  ISNULL(SUM(s2.NumeroPostiNoVittoNoAlloggio + s2.NumeroPostiVittoAlloggio + s2.NumeroPostiVitto), 0) FROM  attivitàentisediattuazione S2 WHERE S2.idattività=a.idattività) as NumVolRic" & _
            " from attività a" & _
            " inner join bandiattività b on a.idbandoattività=b.idbandoattività" & _
            " inner join bando c on b.idbando=c.idbando" & _
            " inner join enti d on b.idente=d.idente" & _
            " inner join statiattività e on e.idstatoattività=a.idstatoattività" & _
            " inner join ambitiattività f on f.idambitoattività=a.idambitoattività" & _
            " inner join macroambitiattività g on f.IDMacroAmbitoAttività=g.IDMacroAmbitoAttività" & _
            " where a.Importo Is Not null And a.punteggiofinale Is Not null" & _
            " and c.idbando=" & CInt(matrix) & " and e.dagraduare = 1" & _
            " order by a.punteggiofinale desc"
            'setto stile datateble
            Dim myDataTable As DataTable = New DataTable
            'dichiaro l'oggetto tabella e  tabella
            Dim myDataColumn As DataColumn
            Dim myDataRow As DataRow
            ' Create new DataColumn, set DataType, ColumnName and add to DataTable.    
            myDataColumn = New DataColumn
            myDataColumn.DataType = System.Type.GetType("System.Int32")
            myDataColumn.ColumnName = "idbando"
            myDataColumn.Caption = "idbando"
            myDataColumn.ReadOnly = True
            myDataColumn.Unique = False

            myDataTable.Columns.Add(myDataColumn)

            myDataColumn = New DataColumn
            myDataColumn.DataType = System.Type.GetType("System.String")
            myDataColumn.ColumnName = "Ambito"
            myDataColumn.Caption = "Ambito"
            myDataColumn.ReadOnly = False
            myDataColumn.Unique = False

            myDataTable.Columns.Add(myDataColumn)

            myDataColumn = New DataColumn
            myDataColumn.DataType = System.Type.GetType("System.Int32")
            myDataColumn.ColumnName = "idente"
            myDataColumn.Caption = "idente"
            myDataColumn.ReadOnly = False
            myDataColumn.Unique = False

            myDataTable.Columns.Add(myDataColumn)

            myDataColumn = New DataColumn
            myDataColumn.DataType = System.Type.GetType("System.String")
            myDataColumn.ColumnName = "denominazione"
            myDataColumn.Caption = "denominazione"
            myDataColumn.ReadOnly = False
            myDataColumn.Unique = False

            myDataTable.Columns.Add(myDataColumn)

            myDataColumn = New DataColumn
            myDataColumn.DataType = System.Type.GetType("System.Int32")
            myDataColumn.ColumnName = "idattività"
            myDataColumn.Caption = "idattività"
            myDataColumn.ReadOnly = False
            myDataColumn.Unique = False

            myDataTable.Columns.Add(myDataColumn)

            myDataColumn = New DataColumn
            myDataColumn.DataType = System.Type.GetType("System.String")
            myDataColumn.ColumnName = "titolo"
            myDataColumn.Caption = "titolo"
            myDataColumn.ReadOnly = False
            myDataColumn.Unique = False

            myDataTable.Columns.Add(myDataColumn)

            myDataColumn = New DataColumn
            myDataColumn.DataType = System.Type.GetType("System.String")
            myDataColumn.ColumnName = "Importo"
            myDataColumn.Caption = "Importo"
            myDataColumn.ReadOnly = False
            myDataColumn.Unique = False

            myDataTable.Columns.Add(myDataColumn)

            myDataColumn = New DataColumn
            myDataColumn.DataType = System.Type.GetType("System.Int32")
            myDataColumn.ColumnName = "idgraduatoriaprogetto"
            myDataColumn.Caption = "idgraduatoriaprogetto"
            myDataColumn.ReadOnly = True
            myDataColumn.Unique = False
            myDataTable.Columns.Add(myDataColumn)

            'Aggiunto da Alessandra Taballione il 24.03.2005
            'implementazione nella griglia dei campi punteggio finale e Numero volontari richiesti
            myDataColumn = New DataColumn
            myDataColumn.DataType = System.Type.GetType("System.Int32")
            myDataColumn.ColumnName = "punteggiofinale"
            myDataColumn.Caption = "Punteggio"
            myDataColumn.ReadOnly = True
            myDataColumn.Unique = False
            myDataTable.Columns.Add(myDataColumn)

            myDataColumn = New DataColumn
            myDataColumn.DataType = System.Type.GetType("System.Int32")
            myDataColumn.ColumnName = "NumVolRic"
            myDataColumn.Caption = "NumVolRic"
            myDataColumn.ReadOnly = True
            myDataColumn.Unique = False
            myDataTable.Columns.Add(myDataColumn)

            myDataColumn = New DataColumn
            myDataColumn.DataType = System.Type.GetType("System.Byte")
            myDataColumn.ColumnName = "Value"
            myDataColumn.Caption = "Value"
            myDataColumn.ReadOnly = True
            myDataColumn.Unique = False

            myDataTable.Columns.Add(myDataColumn)

            Dtrgenerico = ClsServer.CreaDatareader(strquery, Session("conn"))

            If Dtrgenerico.HasRows = True Then
                Dim booUnaVolta As Boolean
                Do While Dtrgenerico.Read
                    If booUnaVolta = False Then ' salvo il totale del bando
                        totale = ClsServer.RendiFormat(IIf(IsDBNull(Dtrgenerico.GetValue(5)) = True, 0, Dtrgenerico.GetValue(5)))
                        booUnaVolta = True
                    End If
                    myDataRow = myDataTable.NewRow()
                    myDataRow("idbando") = Dtrgenerico.GetValue(3)
                    myDataRow("Ambito") = IIf(IsDBNull(Dtrgenerico.GetValue(4)) = True, "", Dtrgenerico.GetValue(4))
                    myDataRow("idente") = Dtrgenerico.GetValue(6)
                    myDataRow("denominazione") = IIf(IsDBNull(Dtrgenerico.GetValue(7)) = True, "", Dtrgenerico.GetValue(7))
                    myDataRow("idattività") = Dtrgenerico.GetValue(0)
                    myDataRow("titolo") = IIf(IsDBNull(Dtrgenerico.GetValue(1)) = True, "", Dtrgenerico.GetValue(1))
                    myDataRow("Importo") = ClsServer.RendiFormat(IIf(IsDBNull(Dtrgenerico.GetValue(2)) = True, 0, Dtrgenerico.GetValue(2)))
                    'sottraggo il totale dei progetti dal totale del bando
                    totale = totale - ClsServer.RendiFormat(IIf(IsDBNull(Dtrgenerico.GetValue(2)) = True, 0, Dtrgenerico.GetValue(2)))
                    If Mid(CStr(totale), 1, 1) = "-" Then 'valorizzo a seconda se negativo o positivo
                        myDataRow("Value") = 1
                    Else
                        myDataRow("Value") = 0
                    End If
                    myDataRow("idgraduatoriaprogetto") = Dtrgenerico.GetValue(8)
                    myDataRow("punteggiofinale") = Dtrgenerico.GetValue(9)
                    myDataRow("NumVolRic") = Dtrgenerico.GetValue(10)
                    myDataTable.Rows.Add(myDataRow)
                Loop
                Dtrgenerico.Close()
                Dtrgenerico = Nothing
                Session("GrigliaBando") = myDataTable 'inizzializzo la session con la datatable
                mydataset = New DataSet
                mydataset.Tables.Add(myDataTable)
                dgRisultatoRicerca.DataSource = mydataset
                dgRisultatoRicerca.DataBind()
            Else
                'altrimenti blocco wform
                CmdConferma.Visible = False
                CmdAnnulla.Visible = False
                CmdSalva.Visible = False
                lblMessaggio.Visible = True
                lblMessaggio.Text = "Attenzione non è presente nessuna graduatoria"
                Dtrgenerico.Close()
                Dtrgenerico = Nothing
            End If
            'setto stile griglia
            If dgRisultatoRicerca.Items.Count <> 0 Then
                For intj As Int16 = 0 To dgRisultatoRicerca.Items.Count - 1
                    If dgRisultatoRicerca.Items(intj).Cells(9).Text = "1" Then
                        dgRisultatoRicerca.Items(intj).CssClass = "BackColor=Yellow"
                        dgRisultatoRicerca.Items(intj).BackColor = System.Drawing.Color.Yellow
                    Else
                        dgRisultatoRicerca.Items(intj).CssClass = "BackColor=GreenYellow"
                        dgRisultatoRicerca.Items(intj).BackColor = System.Drawing.Color.GreenYellow
                    End If
                Next
            End If
        Else
            lbltitolo.Text = "Lista Progetti in Graduatoria Bando: " & Request.QueryString("Name")
            CmdConferma.Visible = True
            CmdAnnulla.Visible = True
            CmdSalva.Visible = False
            'eseguo query per conferma annulla graduatoria
            strquery = "select a.idattività,a.titolo,a.Importo," & _
            " c.idbando,g.codifica + '-' + g.macroambitoattività + ' \ ' + f.codifica + '-' + f.ambitoattività as Ambito," & _
            " c.importostanziato,d.idente,d.denominazione,e.idgraduatoriaprogetto, e.statograduatoria, " & _
            " a.punteggiofinale, " & _
            " (SELECT  ISNULL(SUM(s2.NumeroPostiNoVittoNoAlloggio + s2.NumeroPostiVittoAlloggio + s2.NumeroPostiVitto), 0) FROM  attivitàentisediattuazione S2 WHERE S2.idattività=a.idattività) as NumVolRic" & _
            " from attività a " & _
            " inner join bandiattività b on a.idbandoattività=b.idbandoattività " & _
            " inner join bando c on b.idbando=c.idbando" & _
            " inner join enti d on b.idente=d.idente" & _
            " inner join graduatorieprogetti e on e.idbando=c.idbando and e.idattività=a.idattività" & _
            " inner join ambitiattività f on f.idambitoattività=a.idambitoattività" & _
            " inner join macroambitiattività g on f.IDMacroAmbitoAttività=g.IDMacroAmbitoAttività" & _
            " where c.idbando=" & CInt(matrix) & "" & _
            " order by e.ordine asc"

            'setto stile datatable
            Dim myDataTable As DataTable = New DataTable
            'dichiaro l'oggetto tabella e  tabella
            Dim myDataColumn As DataColumn
            Dim myDataRow As DataRow
            ' Create new DataColumn, set DataType, ColumnName and add to DataTable.    
            myDataColumn = New DataColumn
            myDataColumn.DataType = System.Type.GetType("System.Int32")
            myDataColumn.ColumnName = "idbando"
            myDataColumn.Caption = "idbando"
            myDataColumn.ReadOnly = True
            myDataColumn.Unique = False

            myDataTable.Columns.Add(myDataColumn)

            myDataColumn = New DataColumn
            myDataColumn.DataType = System.Type.GetType("System.String")
            myDataColumn.ColumnName = "Ambito"
            myDataColumn.Caption = "Ambito"
            myDataColumn.ReadOnly = False
            myDataColumn.Unique = False

            myDataTable.Columns.Add(myDataColumn)

            myDataColumn = New DataColumn
            myDataColumn.DataType = System.Type.GetType("System.Int32")
            myDataColumn.ColumnName = "idente"
            myDataColumn.Caption = "idente"
            myDataColumn.ReadOnly = False
            myDataColumn.Unique = False

            myDataTable.Columns.Add(myDataColumn)

            myDataColumn = New DataColumn
            myDataColumn.DataType = System.Type.GetType("System.String")
            myDataColumn.ColumnName = "denominazione"
            myDataColumn.Caption = "denominazione"
            myDataColumn.ReadOnly = False
            myDataColumn.Unique = False

            myDataTable.Columns.Add(myDataColumn)

            myDataColumn = New DataColumn
            myDataColumn.DataType = System.Type.GetType("System.Int32")
            myDataColumn.ColumnName = "idattività"
            myDataColumn.Caption = "idattività"
            myDataColumn.ReadOnly = False
            myDataColumn.Unique = False

            myDataTable.Columns.Add(myDataColumn)

            myDataColumn = New DataColumn
            myDataColumn.DataType = System.Type.GetType("System.String")
            myDataColumn.ColumnName = "titolo"
            myDataColumn.Caption = "titolo"
            myDataColumn.ReadOnly = False
            myDataColumn.Unique = False

            myDataTable.Columns.Add(myDataColumn)

            myDataColumn = New DataColumn
            myDataColumn.DataType = System.Type.GetType("System.String")
            myDataColumn.ColumnName = "Importo"
            myDataColumn.Caption = "Importo"
            myDataColumn.ReadOnly = False
            myDataColumn.Unique = False

            myDataTable.Columns.Add(myDataColumn)

            myDataColumn = New DataColumn
            myDataColumn.DataType = System.Type.GetType("System.Int32")
            myDataColumn.ColumnName = "idgraduatoriaprogetto"
            myDataColumn.Caption = "idgraduatoriaprogetto"
            myDataColumn.ReadOnly = True
            myDataColumn.Unique = False
            myDataTable.Columns.Add(myDataColumn)

            'Aggiunto da Alessandra Taballione il 24.03.2005
            'implementazione nella griglia dei campi punteggio finale e Numero volontari richiesti
            myDataColumn = New DataColumn
            myDataColumn.DataType = System.Type.GetType("System.Int32")
            myDataColumn.ColumnName = "punteggiofinale"
            myDataColumn.Caption = "Punteggio"
            myDataColumn.ReadOnly = True
            myDataColumn.Unique = False
            myDataTable.Columns.Add(myDataColumn)

            myDataColumn = New DataColumn
            myDataColumn.DataType = System.Type.GetType("System.Int32")
            myDataColumn.ColumnName = "NumVolRic"
            myDataColumn.Caption = "NumVolRic"
            myDataColumn.ReadOnly = True
            myDataColumn.Unique = False

            myDataTable.Columns.Add(myDataColumn)

            myDataColumn = New DataColumn
            myDataColumn.DataType = System.Type.GetType("System.Byte")
            myDataColumn.ColumnName = "Value"
            myDataColumn.Caption = "Value"
            myDataColumn.ReadOnly = True
            myDataColumn.Unique = False

            myDataTable.Columns.Add(myDataColumn)

            Dtrgenerico = ClsServer.CreaDatareader(strquery, Session("conn"))

            If Dtrgenerico.HasRows = True Then

                Dim booUnaVolta As Boolean = False
                Do While Dtrgenerico.Read
                    If Dtrgenerico.GetValue(9) = 1 Then
                        'verifico se la grauatoria è stata pubblicata bloccando il form
                        CmdConferma.Visible = False
                        CmdAnnulla.Visible = False
                        lblMessaggio.Visible = True
                        lblMessaggio.Text = "La graduatoria risulta pubblicata e pertanto non può essere modificata"
                    End If
                    If booUnaVolta = False Then ' salvo il totale del bando
                        totale = ClsServer.RendiFormat(IIf(IsDBNull(Dtrgenerico.GetValue(5)) = True, 0, Dtrgenerico.GetValue(5)))
                        booUnaVolta = True
                    End If
                    myDataRow = myDataTable.NewRow()
                    myDataRow("idbando") = Dtrgenerico.GetValue(3)
                    myDataRow("Ambito") = IIf(IsDBNull(Dtrgenerico.GetValue(4)) = True, "", Dtrgenerico.GetValue(4))
                    myDataRow("idente") = Dtrgenerico.GetValue(6)
                    myDataRow("denominazione") = IIf(IsDBNull(Dtrgenerico.GetValue(7)) = True, "", Dtrgenerico.GetValue(7))
                    myDataRow("idattività") = Dtrgenerico.GetValue(0)
                    myDataRow("titolo") = IIf(IsDBNull(Dtrgenerico.GetValue(1)) = True, "", Dtrgenerico.GetValue(1))
                    myDataRow("Importo") = ClsServer.RendiFormat(IIf(IsDBNull(Dtrgenerico.GetValue(2)) = True, 0, Dtrgenerico.GetValue(2)))
                    totale = totale - ClsServer.RendiFormat(IIf(IsDBNull(Dtrgenerico.GetValue(2)) = True, 0, Dtrgenerico.GetValue(2)))
                    If Mid(CStr(totale), 1, 1) = "-" Then 'sottraggo il totale dei progetti dal totale del bando
                        myDataRow("Value") = 1
                    Else
                        myDataRow("Value") = 0
                    End If
                    myDataRow("idgraduatoriaprogetto") = Dtrgenerico.GetValue(8)
                    myDataRow("punteggiofinale") = Dtrgenerico.GetValue(10)
                    myDataRow("NumVolRic") = Dtrgenerico.GetValue(11)
                    myDataTable.Rows.Add(myDataRow)
                Loop
                'inizzializzo la session con la datatable
                Dtrgenerico.Close()
                Dtrgenerico = Nothing
                Session("GrigliaBando") = myDataTable  'inizzializzo la session con la datatable
                mydataset = New DataSet
                mydataset.Tables.Add(myDataTable)
                dgRisultatoRicerca.DataSource = mydataset
                dgRisultatoRicerca.DataBind()
            Else
                CmdConferma.Visible = False
                CmdAnnulla.Visible = False
                CmdSalva.Visible = False
                lblMessaggio.Visible = True
                lblMessaggio.Text = "Attenzione non è presente nessuna graduatoria"
                Dtrgenerico.Close()
                Dtrgenerico = Nothing
            End If
            'setto stile griglia
            If dgRisultatoRicerca.Items.Count <> 0 Then
                For intj As Int16 = 0 To dgRisultatoRicerca.Items.Count - 1
                    If dgRisultatoRicerca.Items(intj).Cells(9).Text = "1" Then
                        dgRisultatoRicerca.Items(intj).CssClass = "BackColor=Yellow"
                        dgRisultatoRicerca.Items(intj).BackColor = System.Drawing.Color.Yellow
                    Else
                        dgRisultatoRicerca.Items(intj).CssClass = "BackColor=GreenYellow"
                        dgRisultatoRicerca.Items(intj).BackColor = System.Drawing.Color.GreenYellow
                    End If
                Next
            End If
        End If
        If dgRisultatoRicerca.Items.Count > 1 Then
            For i = 0 To dgRisultatoRicerca.Items.Count - 1
                If i = 0 Then
                    dgRisultatoRicerca.Items(i).Cells(12).Text = dgRisultatoRicerca.Items(i).Cells(11).Text
                Else
                    dgRisultatoRicerca.Items(i).Cells(12).Text = CDbl(dgRisultatoRicerca.Items(i - 1).Cells(12).Text) + CDbl(dgRisultatoRicerca.Items(i).Cells(11).Text)
                End If
            Next
        Else
            dgRisultatoRicerca.Items(i).Cells(12).Text = dgRisultatoRicerca.Items(i).Cells(11).Text
        End If
    End Function

    Private Sub CmdChiudi_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdChiudi.Click
        '***Generata da Gianluigi Paesani in data:24/08/04
        '***Gestisco l'uscita dalla form
        If Not Request.QueryString("Arrivo") Is Nothing Then
            If Request.QueryString("tipo") = "Produzione" Then
                Session("GrigliaBando") = Nothing
                Response.Redirect("" & Request.QueryString("Arrivo") & "")
            Else
                Session("GrigliaBando") = Nothing
                Response.Redirect("" & Request.QueryString("Arrivo") & "")
            End If
        Else
            Session("GrigliaBando") = Nothing
            Response.Redirect("WfrmMain.aspx")
        End If
    End Sub

    Private Sub CmdSalva_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdSalva.Click
        Dim myDataTable As DataTable = New DataTable
        Dim CmdInsert As Data.SqlClient.SqlCommand
        lblMessaggio.Visible = False

        Dim intk As Integer = 1
        Dim VerSal As Byte = 0
        myDataTable = Session("GrigliaBando")
        If myDataTable.Rows.Count <> 0 Then
            For intj As Integer = 0 To myDataTable.Rows.Count - 1
                'Modificato da alessandra Taballione il (item(8) a (10) )
                If myDataTable.Rows(intj).Item(10) = 0 Then
                    CmdInsert = New SqlClient.SqlCommand("insert" & _
                    " into graduatorieprogetti(idbando,idattività," & _
                    " ordine,finanziato,StatoGraduatoria)" & _
                    " values (" & myDataTable.Rows(intj).Item(0) & "," & myDataTable.Rows(intj).Item(4) & "," & intk & ",1,0)", Session("conn"))
                    CmdInsert.ExecuteNonQuery()
                    CmdInsert.Dispose()
                    intk = intk + 1
                    VerSal = 1
                Else
                    CmdInsert = New SqlClient.SqlCommand("insert" & _
                    " into graduatorieprogetti(idbando,idattività," & _
                    " ordine,finanziato,StatoGraduatoria)" & _
                    " values (" & myDataTable.Rows(intj).Item(0) & "," & myDataTable.Rows(intj).Item(4) & "," & intk & ",0,0)", Session("conn"))
                    CmdInsert.ExecuteNonQuery()
                    CmdInsert.Dispose()
                    intk = intk + 1
                    VerSal = 1
                End If
            Next
        End If
        If VerSal = 1 Then
            CmdSalva.Visible = False
            lblMessaggio.Visible = True
            lblMessaggio.Text = "La graduatoria è stata salvata con successo"
        End If
    End Sub

    Private Sub CmdConferma_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdConferma.Click
        Dim ComModifica As Data.SqlClient.SqlCommand
        Dim myDataTable As DataTable = New DataTable
        lblMessaggio.Visible = False
        myDataTable = Session("GrigliaBando")
        If myDataTable.Rows.Count <> 0 Then
            For intj As Integer = 0 To myDataTable.Rows.Count - 1
                'se è verde ovvero si 
                If myDataTable.Rows(intj).Item(10) = 0 Then
                    'modifico prima la cronologia dell'attività
                    ComModifica = New Data.SqlClient.SqlCommand("insert into CronologiaAttività" & _
                    " (idattività,idstatoattività,datacronologia,idTipoCronologia," & _
                    " usernameaccreditatore)" & _
                    " select " & myDataTable.Rows(intj).Item(4) & "," & _
                    " idstatoattività,getdate(),0,'" & ClsServer.NoApice(Session("Utente")) & "'" & _
                    " from attività where idattività=" & myDataTable.Rows(intj).Item(4) & "", Session("conn"))
                    ComModifica.ExecuteNonQuery()
                    ComModifica.Dispose()
                    'modifico attività
                    ComModifica = New SqlClient.SqlCommand("update attività set" & _
                    " idstatoattività=(select idstatoattività from statiattività where attiva=1) , " & _
                    " DataUltimoStato= getdate() , " & _
                    " UsernameStato = '" & ClsServer.NoApice(Session("Utente")) & "' " & _
                    " where idattività=" & myDataTable.Rows(intj).Item(4) & "", Session("conn"))
                    ComModifica.ExecuteNonQuery()
                    ComModifica.Dispose()

                    ComModifica = New SqlClient.SqlCommand("update graduatorieprogetti" & _
                    " set StatoGraduatoria=1 where" & _
                    " idgraduatoriaprogetto=" & myDataTable.Rows(intj).Item(7) & "", Session("conn"))
                    ComModifica.ExecuteNonQuery()
                    ComModifica.Dispose()
                Else
                    'modifico prima la cronologia dell'attività
                    ComModifica = New Data.SqlClient.SqlCommand("insert into CronologiaAttività" & _
                    " (idattività,idstatoattività,datacronologia,idTipoCronologia," & _
                    " usernameaccreditatore)" & _
                    " select " & myDataTable.Rows(intj).Item(4) & "," & _
                    " idstatoattività,getdate(),0,'" & ClsServer.NoApice(Session("Utente")) & "'" & _
                    " from attività where idattività=" & myDataTable.Rows(intj).Item(4) & "", Session("conn"))
                    ComModifica.ExecuteNonQuery()
                    ComModifica.Dispose()
                    'modifico attività
                    ComModifica = New SqlClient.SqlCommand("update attività set" & _
                    " idstatoattività=6 , " & _
                    " DataUltimoStato= getdate() , " & _
                    " UsernameStato = '" & ClsServer.NoApice(Session("Utente")) & "' " & _
                    " where idattività=" & myDataTable.Rows(intj).Item(4) & "", Session("conn"))
                    ComModifica.ExecuteNonQuery()
                    ComModifica.Dispose()

                    ComModifica = New SqlClient.SqlCommand("update graduatorieprogetti" & _
                    " set StatoGraduatoria=1 where" & _
                    " idgraduatoriaprogetto=" & myDataTable.Rows(intj).Item(7) & "", Session("conn"))
                    ComModifica.ExecuteNonQuery()
                    ComModifica.Dispose()
                End If
            Next
            CmdConferma.Visible = False
            CmdAnnulla.Visible = False
            CmdSalva.Visible = False
            lblMessaggio.Visible = True
            lblMessaggio.Text = "La Graduatoria è stata approvata con successo"
        Else

        End If
    End Sub

    Private Sub CmdAnnulla_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdAnnulla.Click
        Dim ComElimina As Data.SqlClient.SqlCommand
        Dim myDataTable As DataTable = New DataTable
        lblMessaggio.Visible = False
        myDataTable = Session("GrigliaBando")
        If myDataTable.Rows.Count <> 0 Then
            For intj As Integer = 0 To myDataTable.Rows.Count - 1
                'ComElimina = New SqlClient.SqlCommand("update attività set" & _
                '" idstatoattività=(select idstatoattività from statiattività where dagraduare=1)" & _
                '" where idattività=" & CInt(dgRisultatoRicerca.Items(intj).Cells(5).Text) & "", Session("conn"))
                'ComElimina.ExecuteNonQuery()
                'ComElimina.Dispose()
                ComElimina = New SqlClient.SqlCommand("delete from" & _
                " GraduatorieProgetti where" & _
                " idgraduatoriaProgetto=" & myDataTable.Rows(intj).Item(7) & "", Session("conn"))
                ComElimina.ExecuteNonQuery()
                ComElimina.Dispose()
            Next
            CmdConferma.Visible = False
            CmdAnnulla.Visible = False
            CmdSalva.Visible = False
            lblMessaggio.Visible = True
            lblMessaggio.Text = "La Graduatoria è stata eliminata con successo"
        Else

        End If
    End Sub

    Private Sub dgRisultatoRicerca_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgRisultatoRicerca.PageIndexChanged
        dgRisultatoRicerca.CurrentPageIndex = e.NewPageIndex
        dgRisultatoRicerca.DataSource = Session("GrigliaBando")
        dgRisultatoRicerca.DataBind()
        If dgRisultatoRicerca.Items.Count <> 0 Then
            For intj As Int16 = 0 To dgRisultatoRicerca.Items.Count - 1
                If dgRisultatoRicerca.Items(intj).Cells(9).Text = "1" Then
                    dgRisultatoRicerca.Items(intj).CssClass = "BackColor=Yellow"
                    dgRisultatoRicerca.Items(intj).BackColor = System.Drawing.Color.Yellow
                Else
                    dgRisultatoRicerca.Items(intj).CssClass = "BackColor=GreenYellow"
                    dgRisultatoRicerca.Items(intj).BackColor = System.Drawing.Color.GreenYellow
                End If
            Next
        End If
    End Sub

End Class