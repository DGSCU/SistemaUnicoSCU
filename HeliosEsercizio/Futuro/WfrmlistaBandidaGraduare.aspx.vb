Public Class WfrmlistaBandidaGraduare
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
        'al primo load della pagina chiamo funzione per caricamento griglia
        If IsPostBack = False Then
            'dgRisultatoRicerca.DataSource = EseguiCalcoloGriglia()
            'dgRisultatoRicerca.DataBind()
            If Request.QueryString("tipo") = "Produzione" Then
                lbltitolo.Text = "Lista Bandi per Graduatoria"
                dvStato.Visible = False

            Else
                lbltitolo.Text = "Lista Bandi in Graduatoria"
                CaricaComboStato()
            End If
            'aggiuntoi l 14/7/2006 da simoan cordella
            CaricaBando()
            'aggiunto il 13/07/2006 da simona cordella
            CaricaCompetenze()
        End If
    End Sub

    Private Sub CaricaComboStato()
        Dim strsql As String
        Dim dtrgenerico As SqlClient.SqlDataReader

        strsql = "Select idstatobando, statobando from statibando " & _
                    " union " & _
                    " Select  '0',' Selezionare' from statibando order by statobando "
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
        dtrgenerico = ClsServer.CreaDatareader(strsql, Session("conn"))
        ddlStato.DataSource = dtrgenerico
        ddlStato.DataTextField = "statobando"
        ddlStato.DataValueField = "IdStatoBando"
        ddlStato.DataBind()
        dtrgenerico.Close()
        dtrgenerico = Nothing
    End Sub

    Private Sub CaricaBando()
        Dim DtrGenerico As System.Data.SqlClient.SqlDataReader
        Dim StrSql As String

        StrSql = "select idbando, bandobreve from Bando where bandobreve is not null union "
        StrSql = StrSql & "select '0','' from Bando where bandobreve is not null "
        If Not DtrGenerico Is Nothing Then
            DtrGenerico.Close()
            DtrGenerico = Nothing
        End If

        'eseguo la query
        DtrGenerico = ClsServer.CreaDatareader(StrSql, Session("conn"))
        'assegno il datadearder alla combo caricando così descrizione e id
        ddlBando.DataSource = DtrGenerico
        ddlBando.Items.Add("")
        ddlBando.DataTextField = "bandobreve"
        ddlBando.DataValueField = "idbando"
        ddlBando.DataBind()
        'chiudo il datareader se aperto
        If Not DtrGenerico Is Nothing Then
            DtrGenerico.Close()
            DtrGenerico = Nothing
        End If
        ''*****Carico Combo Bandi
        '    DdlBando.DataSource = MakeParentTable("select idbando, bandobreve from Bando")
        '    DdlBando.DataTextField = "ParentItem"
        '    DdlBando.DataValueField = "id"
        '    DdlBando.DataBind()
    End Sub

    Private Sub CaricaCompetenze()
        'stringa per la query
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
                ddlCompetenze.DataSource = dtrCompetenze
                ddlCompetenze.Items.Add("")
                ddlCompetenze.DataTextField = "Descrizione"
                ddlCompetenze.DataValueField = "IDRegioneCompetenza"
                ddlCompetenze.DataBind()
                'chiudo il datareader se aperto
                If Not dtrCompetenze Is Nothing Then
                    dtrCompetenze.Close()
                    dtrCompetenze = Nothing
                End If
            End If
            'Controllo abilitazione scelta
            If Session("TipoUtente") = "U" Then
                ddlCompetenze.Enabled = True
                ddlCompetenze.SelectedIndex = 0

            Else
                ddlCompetenze.Enabled = False
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
                    ddlCompetenze.SelectedValue = dtrCompetenze("IdRegioneCompetenza")
                    If dtrCompetenze("Heliosread") = True Then
                        ddlCompetenze.Enabled = True
                    End If

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

    Private Sub cmdChiudi_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdChiudi.Click
        '***Generata da Gianluigi Paesani in data:24/08/04
        '*** rimando alla main
        Response.Redirect("WfrmMain.aspx")
    End Sub

    Private Sub dgRisultatoRicerca_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgRisultatoRicerca.SelectedIndexChanged
        '***Generata da Gianluigi Paesani in data:24/08/04
        '***Alla selezione del bando verifico se si tratta di produrre graduatoria
        '***o confermare annullare graduatoria
        If Not dgRisultatoRicerca.SelectedItem Is Nothing Then
            If Request.QueryString("tipo") = "Produzione" Then
                Response.Redirect("WfrmGestioneGraduatoriaProg.aspx?BandoBreve=" & dgRisultatoRicerca.SelectedItem.Cells(2).Text & "&Matrix=" & dgRisultatoRicerca.SelectedItem.Cells(1).Text & "&tipo=Produzione&Name=" & dgRisultatoRicerca.SelectedItem.Cells(3).Text & "&Arrivo=WfrmlistaBandidaGraduare.aspx?tipo=Produzione")
            Else
                Response.Redirect("WfrmGestioneGraduatoriaProg.aspx?BandoBreve=" & dgRisultatoRicerca.SelectedItem.Cells(2).Text & "&Matrix=" & dgRisultatoRicerca.SelectedItem.Cells(1).Text & "&tipo=Conferma&Name=" & dgRisultatoRicerca.SelectedItem.Cells(3).Text & "&Arrivo=WfrmlistaBandidaGraduare.aspx?tipo=Conferma")
            End If
        End If
    End Sub

    Private Sub cmdRicerca_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdRicerca.Click

        If ValidazioneServerRicerca() = True Then
            dgRisultatoRicerca.CurrentPageIndex = 0
            dgRisultatoRicerca.DataSource = EseguiCalcoloGriglia()
            dgRisultatoRicerca.DataBind()
            ColoraCelle()
        End If
      
    End Sub

    Private Function EseguiCalcoloGriglia() As DataSet
        '***Generata da Gianluigi Paesani in data:24/08/04
        '***Questa routine valorizza la griglia a seconda della gestione (produzione,conferma annulla)
        Dim strquery As String
        Dim Dtrgenerico As Data.SqlClient.SqlDataReader

        If Request.QueryString("tipo") = "Produzione" Then
            lbltitolo.Text = "Lista Bandi per Graduatoria"
            'query per bandi ancora da produrre
            strquery = "select distinct c.idbando, c.bando,c.bandobreve," & _
            " convert(varchar,c.datainiziovalidità,103) as datainizio," & _
            " convert(varchar,c.datafinevalidità,103) as datafine," & _
            " count(a.idattività) as progetti, c.importostanziato, '' as Stato, '' as color from attività a" & _
            " inner join bandiattività b on a.idbandoattività=b.idbandoattività" & _
            " inner join bando c on b.idbando=c.idbando" & _
            " INNER JOIN AssociaBandoRegionicompetenze ab on c.idbando=ab.idbando " & _
            " inner join RegioniCompetenze r on r.idRegioneCompetenza = ab.idRegioneCompetenza " & _
            " inner join statibandiattività d on b.idstatobandoattività=d.idstatobandoattività" & _
            " inner join statibando e on c.idstatobando=e.idstatobando" & _
            " inner join statiattività f on a.idstatoattività=f.idstatoattività" & _
            " where (c.idbando <> ALL (SELECT idbando from graduatorieprogetti where idbando=c.idbando))" & _
            " and e.InValutazione=1 and d.DaValutare=0 and f.DaGraduare=1 and Importo is not null"

        Else
            lbltitolo.Text = "Lista Bandi in Graduatoria"
            strquery = "select distinct c.idbando, c.bando,c.bandobreve," & _
            " convert(varchar,c.datainiziovalidità,103) as datainizio," & _
            " convert(varchar,c.datafinevalidità,103) as datafine," & _
            " count(a.idattività) as progetti, c.importostanziato, e.StatoBando as Stato," & _
            " case e.invalutazione when 1 then 'Khaki'else '' end + '' + " & _
            " case e.defaultstato when 1 then 'Gainsboro' else '' end + '' + " & _
            " case e.attivo when 1 then 'Lightgreen' else '' end + '' + " & _
            " case e.annullato when 1 then 'LightSalmon' else '' end " & _
            " as color" & _
            " from attività a" & _
            " inner join bandiattività b on a.idbandoattività=b.idbandoattività" & _
            " inner join bando c on b.idbando=c.idbando" & _
            " INNER JOIN AssociaBandoRegionicompetenze ab on c.idbando=ab.idbando " & _
            " inner join RegioniCompetenze r on r.idRegioneCompetenza = ab.idRegioneCompetenza " & _
            " inner join graduatorieprogetti d on c.idbando=d.idbando and d.idattività=a.idattività" & _
            " inner join statibando e on c.idstatobando = e.idstatobando"

        End If

        Dim strwhere As String

        If Request.QueryString("tipo") = "Produzione" Then
            'If txtBando.Text <> vbNullString Then
            'modificato il 14/07/2006 da simona cordella
            'gestione combo drescizione bandobreve
            If ddlBando.SelectedItem.Text <> "" Then
                strwhere = " AND c.bandobreve = '" & ClsServer.NoApice(ddlBando.SelectedItem.Text) & "'"
            End If
            If txtInizio.Text <> vbNullString Then
                strwhere = strwhere & " AND c.datainiziovalidità = '" & txtInizio.Text & "'"
            End If
            If txtfine.Text <> vbNullString Then
                strwhere = strwhere & " AND c.datafinevalidità = '" & txtfine.Text & "'"
            End If
            If ddlCompetenze.SelectedValue <> "" Then
                Select Case ddlCompetenze.SelectedValue
                    Case 0
                        strwhere = strwhere & " "
                    Case -1
                        strwhere = strwhere & " AND  ab.IdRegioneCompetenza = 22"
                    Case -2
                        strwhere = strwhere & " AND  ab.IdRegioneCompetenza <> 22 And not ab.IdRegioneCompetenza is null "
                    Case -3
                        strwhere = strwhere & " AND  ab.IdRegioneCompetenza is null "
                    Case Else
                        strwhere = strwhere & " AND  ab.IdRegioneCompetenza = " & ddlCompetenze.SelectedValue
                End Select
            End If
        Else
            'If txtBando.Text <> vbNullString Then
            '    strwhere = " WHERE c.bando LIKE '" & txtBando.Text & "%'"
            'End If
            'modificato il 14/07/2006 da simona cordella
            'gestione combo drescizione bandobreve
            If ddlBando.SelectedItem.Text <> "" Then
                strwhere = " where c.bandobreve = '" & ClsServer.NoApice(ddlBando.SelectedItem.Text) & "'"
            End If
            If ddlStato.SelectedIndex > 0 Then
                If strwhere = vbNullString Then
                    strwhere = " WHERE e.idstatobando = " & ddlStato.SelectedItem.Value
                Else
                    strwhere = strwhere & " AND e.idstatobando = " & ddlStato.SelectedItem.Value
                End If
            End If
            If txtInizio.Text <> vbNullString Then
                If strwhere = vbNullString Then
                    strwhere = " WHERE c.datainiziovalidità = '" & txtInizio.Text & "'"
                Else
                    strwhere = strwhere & " AND c.datainiziovalidità = '" & txtInizio.Text & "'"
                End If
            End If
            If txtfine.Text <> vbNullString Then
                If strwhere = vbNullString Then
                    strwhere = " WHERE c.datafinevalidità = '" & txtfine.Text & "'"
                Else
                    strwhere = strwhere & " AND c.datafinevalidità = '" & txtfine.Text & "'"
                End If
            End If
            If ddlCompetenze.SelectedValue <> "" Then
                If strwhere = vbNullString Then
                    Select Case ddlCompetenze.SelectedValue
                        Case 0
                            strwhere = strwhere & " "
                        Case -1
                            strwhere = " WHERE  ab.IdRegioneCompetenza = 22"
                        Case -2
                            strwhere = " WHERE  ab.IdRegioneCompetenza <> 22 And not ab.IdRegioneCompetenza is null "
                        Case -3
                            strwhere = " WHERE  ab.IdRegioneCompetenza is null "
                        Case Else
                            strwhere = " WHERE  ab.IdRegioneCompetenza = " & ddlCompetenze.SelectedValue
                    End Select
                Else
                    Select Case ddlCompetenze.SelectedValue
                        Case 0
                            strwhere = strwhere & " "
                        Case -1
                            strwhere = strwhere & " AND  ab.IdRegioneCompetenza = 22"
                        Case -2
                            strwhere = strwhere & " AND  ab.IdRegioneCompetenza <> 22 And not ab.IdRegioneCompetenza is null "
                        Case -3
                            strwhere = strwhere & " AND  ab.IdRegioneCompetenza is null "
                        Case Else
                            strwhere = strwhere & " AND  ab.IdRegioneCompetenza = " & ddlCompetenze.SelectedValue
                    End Select
                End If
            End If
        End If
        strquery = strquery & strwhere

        If Request.QueryString("tipo") = "Produzione" Then
            strquery = strquery & " group by c.idbando, c.bando,c.bandobreve, c.datainiziovalidità, datafinevalidità, c.importostanziato, e.invalutazione, e.defaultstato, e.attivo, e.annullato"
        Else
            strquery = strquery & " group by c.idbando, c.bando,c.bandobreve, e.StatoBando, c.datainiziovalidità, datafinevalidità, c.importostanziato, e.invalutazione, e.defaultstato, e.attivo, e.annullato"
        End If



        Dim myDataTable As DataTable = New DataTable
        'dichiaro l'oggetto tabella e  tabella
        Dim myDataColumn As DataColumn
        Dim myDataRow As DataRow

        ' Create new DataColumn, set DataType, ColumnName and add to DataTable.    
        'setto stile
        myDataColumn = New DataColumn
        myDataColumn.DataType = System.Type.GetType("System.Int32")
        myDataColumn.ColumnName = "idbando"
        myDataColumn.Caption = "idbando"
        myDataColumn.ReadOnly = True
        myDataColumn.Unique = False

        myDataTable.Columns.Add(myDataColumn)

        myDataColumn = New DataColumn
        myDataColumn.DataType = System.Type.GetType("System.String")
        myDataColumn.ColumnName = "bando"
        myDataColumn.Caption = "bando"
        myDataColumn.ReadOnly = False
        myDataColumn.Unique = False

        myDataTable.Columns.Add(myDataColumn)

        myDataColumn = New DataColumn
        myDataColumn.DataType = System.Type.GetType("System.String")
        myDataColumn.ColumnName = "Stato"
        myDataColumn.Caption = "Stato"
        myDataColumn.ReadOnly = False
        myDataColumn.Unique = False

        myDataTable.Columns.Add(myDataColumn)

        myDataColumn = New DataColumn
        myDataColumn.DataType = System.Type.GetType("System.String")
        myDataColumn.ColumnName = "datainizio"
        myDataColumn.Caption = "datainizio"
        myDataColumn.ReadOnly = False
        myDataColumn.Unique = False

        myDataTable.Columns.Add(myDataColumn)

        myDataColumn = New DataColumn
        myDataColumn.DataType = System.Type.GetType("System.String")
        myDataColumn.ColumnName = "datafine"
        myDataColumn.Caption = "datafine"
        myDataColumn.ReadOnly = False
        myDataColumn.Unique = False

        myDataTable.Columns.Add(myDataColumn)

        myDataColumn = New DataColumn
        myDataColumn.DataType = System.Type.GetType("System.Int16")
        myDataColumn.ColumnName = "progetti"
        myDataColumn.Caption = "progetti"
        myDataColumn.ReadOnly = False
        myDataColumn.Unique = False

        myDataTable.Columns.Add(myDataColumn)

        myDataColumn = New DataColumn
        myDataColumn.DataType = System.Type.GetType("System.String")
        myDataColumn.ColumnName = "importostanziato"
        myDataColumn.Caption = "importostanziato"
        myDataColumn.ReadOnly = False
        myDataColumn.Unique = False
        'myDataColumn.AllowDBNull = True
        myDataTable.Columns.Add(myDataColumn)

        myDataColumn = New DataColumn
        myDataColumn.DataType = System.Type.GetType("System.String")
        myDataColumn.ColumnName = "color"
        myDataColumn.ReadOnly = False
        myDataColumn.Unique = False
        myDataColumn.AllowDBNull = True
        myDataTable.Columns.Add(myDataColumn)

        'Session("GrigliaBando")
        Dtrgenerico = ClsServer.CreaDatareader(strquery, Session("conn"))

        If Dtrgenerico.HasRows = True Then
            'se vi sono dati valorizzo datatable e l'Importo nel dataset
            Do While Dtrgenerico.Read
                myDataRow = myDataTable.NewRow()
                myDataRow("idbando") = Dtrgenerico("idbando")
                myDataRow("bando") = Dtrgenerico("bando")
                myDataRow("Stato") = Dtrgenerico("Stato")
                myDataRow("datainizio") = IIf(IsDBNull(Dtrgenerico("datainizio")) = True, "", Dtrgenerico("datainizio"))
                myDataRow("datafine") = IIf(IsDBNull(Dtrgenerico("datafine")) = True, "", Dtrgenerico("datafine"))
                myDataRow("progetti") = IIf(IsDBNull(Dtrgenerico("progetti")) = True, "", Dtrgenerico("progetti"))
                myDataRow("importostanziato") = ClsServer.RendiFormat(IIf(IsDBNull(Dtrgenerico("importostanziato")) = True, 0, Dtrgenerico("importostanziato")))
                myDataRow("color") = Dtrgenerico("color")
                myDataTable.Rows.Add(myDataRow)
            Loop
            Dtrgenerico.Close()
            Dtrgenerico = Nothing
            EseguiCalcoloGriglia = New DataSet
            EseguiCalcoloGriglia.Tables.Add(myDataTable)
            lblgraduatoria.Visible = False
        Else 'mando messaggio in assenza di dati
            lblgraduatoria.Visible = True
            lblgraduatoria.Text = "Attenzione, non risultano esserci bandi disponibili"
            Dtrgenerico.Close()
            Dtrgenerico = Nothing
        End If

    End Function

    Private Sub dgRisultatoRicerca_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgRisultatoRicerca.PageIndexChanged
        dgRisultatoRicerca.SelectedIndex = -1
        dgRisultatoRicerca.EditItemIndex = -1
        dgRisultatoRicerca.CurrentPageIndex = e.NewPageIndex
        'lblpage.Text = e.NewPageIndex
        dgRisultatoRicerca.DataSource = EseguiCalcoloGriglia()
        dgRisultatoRicerca.DataBind()
        ColoraCelle()
    End Sub

    Private Sub ColoraCelle()
        'Generato da Alessandra Taballione il 15/06/04
        'VAriazione del Colore secondo lo stato della sede.
        'Attiva=Verde;Presentata=gialla;Cancellata=Rossa;Sospesa=
        Dim item As DataGridItem
        Dim color As New System.Drawing.Color
        Dim x As Integer
        For Each item In dgRisultatoRicerca.Items
            For x = 0 To 7
                If dgRisultatoRicerca.Items(item.ItemIndex).Cells(8).Text = "&nbsp;" Then
                    color = System.Drawing.Color.LightSalmon
                Else
                    color = System.Drawing.Color.FromName(dgRisultatoRicerca.Items(item.ItemIndex).Cells(8).Text)
                End If
                If Request.QueryString("tipo") <> "Produzione" Then
                    dgRisultatoRicerca.Items(item.ItemIndex).Cells(x).BackColor = color
                End If
            Next
        Next
    End Sub

    Function ValidazioneServerRicerca() As Boolean

        If txtInizio.Text.Trim <> String.Empty Then
            Dim dataInizio As Date
            If (Date.TryParse(txtInizio.Text, dataInizio) = False) Then
                lblgraduatoria.Visible = True
                lblgraduatoria.Text = "La 'Data Inizio' non è valida. Inserire la data nel formato GG/MM/AAAA."
                Return False
            End If
        End If

        If txtfine.Text.Trim <> String.Empty Then
            Dim dataFine As Date
            If (Date.TryParse(txtfine.Text, dataFine) = False) Then
                lblgraduatoria.Visible = True
                lblgraduatoria.Text = "La 'Data Fine' non è valida. Inserire la data nel formato GG/MM/AAAA."
                Return False
            End If
        End If

        Return True

    End Function


End Class