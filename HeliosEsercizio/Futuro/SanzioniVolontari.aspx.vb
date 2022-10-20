Imports System.Drawing
Public Class SanzioniVolontari
    Inherits System.Web.UI.Page

    Dim strsql As String
    Dim dtsGenerico As DataSet
    Dim MyTransaction As System.Data.SqlClient.SqlTransaction
    Dim dtrgenerico As SqlClient.SqlDataReader
    Dim myCommand As System.Data.SqlClient.SqlCommand

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Inserire qui il codice utente necessario per inizializzare la pagina
        If Not Session("LogIn") Is Nothing Then
            If Session("LogIn") = False Then
                Response.Redirect("LogOn.aspx")
            End If
        Else
            Response.Redirect("LogOn.aspx")
        End If
        If IsPostBack = False Then
            caricacombocausali()
            caricadati()
            CaricaSediAttuazione()
            CaricaAssenze()
        Else
            lblmess.Visible = False
            lblmess.Text = ""
        End If

        'Verifica menu sicurezza su funzione accredita
        strsql = "SELECT AssociaUtenteGruppo.username, VociMenu.IdVoceMenu, VociMenu.VoceMenu, VociMenu.ImmaginePassiva, VociMenu.ImmagineAttiva, VociMenu.Link,"
        strsql = strsql & " VociMenu.IdVoceMenuPadre"
        strsql = strsql & " FROM VociMenu"
        strsql = strsql & " INNER JOIN 	AbilitazioniProfiliVociMenu ON VociMenu.IdVoceMenu = AbilitazioniProfiliVociMenu.IdVoceMenu"
        strsql = strsql & " INNER JOIN	Profili ON AbilitazioniProfiliVociMenu.IdProfilo = Profili.IdProfilo"

        '============================================================================================================================
        '====================================================30/09/2008==============================================================
        '=======MODIFICA EFFETTUATA DA Kronk (99 scimmie saltavano sul letto, una cadde in terra e si ruppe il cervelletto...)=======
        '=================AGGIUNTA JOIN SU PROFILO READ PER ABILITARE LEGGERE LE ABILITAZIONI ANCHE DELL'UTENTE READ=================
        '============================================================================================================================
        If Session("Read") <> "1" Then
            strsql = strsql & " INNER JOIN	AssociaUtenteGruppo ON Profili.IdProfilo = AssociaUtenteGruppo.IdProfilo "
        Else
            strsql = strsql & " INNER JOIN	AssociaUtenteGruppo ON Profili.IdProfilo = AssociaUtenteGruppo.IdProfiloRead "
        End If

        strsql = strsql & " LEFT JOIN 	RestrizioniUtenteVociMenu ON AssociaUtenteGruppo.UserName = RestrizioniUtenteVociMenu.UserName and RestrizioniUtenteVociMenu.IdVoceMenu=VociMenu.IdVoceMenu"
        strsql = strsql & " WHERE VociMenu.descrizione = 'Forza Modifica Assenza'"
        strsql = strsql & " AND AssociaUtenteGruppo.username ='" & Session("Utente") & "' and (RestrizioniUtenteVociMenu.TipoRestrizione <> 0 or RestrizioniUtenteVociMenu.TipoRestrizione is null)"

        dtrgenerico = ClsServer.CreaDatareader(strsql, Session("conn"))

        If dtrgenerico.HasRows = True Then
            Session("utemod") = "SI"
        Else
            Session("utemod") = "NO"
        End If
        dtrgenerico.Close()
        dtrgenerico = Nothing
    End Sub

    Sub caricacombocausali()
        '***Generata da Bagnani Jonathan in data:06/12/04
        '***carico combo delle causali

        ddlCausale.Items.Clear()
        If (Session("TipoUtente") = "U" Or Session("TipoUtente") = "R") Then
            strsql = "select idcausale,descrizione from causali where tipo=10 "
        Else
            strsql = "select idcausale,descrizione from causali where tipo=10 and utenteUnsc<>1"
        End If
        ddlCausale.DataSource = MakeParentTable(strsql)
        ddlCausale.DataTextField = "ParentItem"
        ddlCausale.DataValueField = "id"
        ddlCausale.DataBind()
    End Sub

    Private Sub caricadati()
        'Generata da Bagnani Jonathan il 04.12.2004
        'carico i dati necessari a stabilire nuove assenze o gestire quelle vecchie
        strsql = "select entità.Identità,entità.cognome, entità.nome,entità.datachiusura,statientità.Statoentità, entità.datanascita,entità.idcomunenascita," & _
        " cn.denominazione as comuneNascita,cr.denominazione as comuneresidenza," & _
        " entità.idcomuneresidenza,entità.codicefiscale,case entità.sesso when 1 then 'F' else 'M' end as sesso " & _
        " from entità " & _
        " inner join statientità on statientità.idstatoEntità=entità.idstatoentità " & _
        " inner join comuni cn on cn.idcomune=entità.idcomunenascita " & _
        " inner join comuni cr on cr.idcomune=entità.idcomuneresidenza " & _
        " where entità.identità=" & CInt(Request.QueryString("identita")) & ""
        dtrgenerico = ClsServer.CreaDatareader(strsql, Session("conn"))
        If dtrgenerico.HasRows = True Then
            dtrgenerico.Read()
            Session("Persona") = (dtrgenerico("Identità"))
            lblCognome.Text = IIf(Not IsDBNull(dtrgenerico("Cognome")), dtrgenerico("Cognome"), "")
            lblNome.Text = IIf(Not IsDBNull(dtrgenerico("nome")), dtrgenerico("nome"), "")
            lblComuneNascita.Text = IIf(Not IsDBNull(dtrgenerico("comuneNascita")), dtrgenerico("comuneNascita"), "")
            lblComuneResidenza.Text = IIf(Not IsDBNull(dtrgenerico("comuneresidenza")), dtrgenerico("comuneresidenza"), "")
            lblCodFis.Text = IIf(Not IsDBNull(dtrgenerico("codicefiscale")), dtrgenerico("codicefiscale"), "")
            lblsesso.Text = dtrgenerico("sesso")
            lbldataNascita.Text = dtrgenerico("DataNascita")
            lblStato.Text = dtrgenerico("Statoentità")
            'txtdatachiusuraEV.Text = dtrgenerico("datachiusura")
        End If
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
        'Antonello
        strsql = "SELECT DataInizioServizio, DataFineServizio, IDEntità, Nome, Cognome,dateadd(day,90,DataInizioServizio) as dataLimite FROM entità WHERE IDEntità = '" & Session("Persona") & "'"
        dtrgenerico = ClsServer.CreaDatareader(strsql, Session("conn"))
        If dtrgenerico.HasRows = True Then
            dtrgenerico.Read()
            '-----------------------------------------------------------------------------------
            'lblProgetto.Text = IIf(Not IsDBNull(dtrgenerico("titolo")), dtrgenerico("titolo"), "")
            lblDataInizio.Text = IIf(Not IsDBNull(dtrgenerico("DataInizioServizio")), dtrgenerico("DataInizioServizio"), "")
            lbldataFine.Text = IIf(Not IsDBNull(dtrgenerico("DataFineServizio")), dtrgenerico("DataFineServizio"), "")
            'carico le combo del periodo di assenza e controllo che carico gli anni 
            'uguali a quelli che sono previsti per il progetto di riferimento 
            ddlAnno.Items.Add("Selezionare")
            If Year(dtrgenerico("DataInizioServizio")) <> Year(dtrgenerico("DataFineServizio")) Then
                ddlAnno.Items.Add(Year(dtrgenerico("DataInizioServizio")))
                ddlAnno.Items.Add(Year(dtrgenerico("DataFineServizio")))
            Else
                ddlAnno.Items.Add(Year(dtrgenerico("DataInizioServizio")))
            End If

            If Not dtrgenerico Is Nothing Then
                dtrgenerico.Close()
                dtrgenerico = Nothing
            End If
        End If
        strsql = "Select titolo, datainizioattività,datafineattività,dateadd(day,90,datainizioattività) as dataLimite from attività where idattività=" & CInt(Request.QueryString("IdAttivita")) & ""
        dtrgenerico = ClsServer.CreaDatareader(strsql, Session("conn"))
        If dtrgenerico.HasRows = True Then
            dtrgenerico.Read()
            lblProgetto.Text = IIf(Not IsDBNull(dtrgenerico("titolo")), dtrgenerico("titolo"), "")
            If Not dtrgenerico Is Nothing Then
                dtrgenerico.Close()
                dtrgenerico = Nothing
            End If
        End If
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
    End Sub

    Private Sub CaricaSediAttuazione()
        strsql = "select '<img src=images/home3.gif Width=20 Height=20 border=0 >' as img, entisedi.denominazione as sedefisica, entisediattuazioni.denominazione as sedeAttuazione," & _
       " entisedi.indirizzo,Comuni.denominazione + '(' + provincie.provincia + ')' as Comune,attivitàentità.identità, " & _
       "attivitàentità.identità,attivitàentità.idattivitàentesedeattuazione,attivitàentità.datafineattivitàentità," & _
       " (select idstatoattivitàentità from statiattivitàentità where defaultstato=1) as statodefault," & _
       " attivitàentità.note,attivitàentità.percentualeutilizzo,attivitàentità.idtipologiaposto " & _
       " from attivitàentisediattuazione" & _
       " inner join attivitàentità on " & _
       " attivitàentità.idattivitàentesedeattuazione = attivitàentisediattuazione.idattivitàentesedeattuazione " & _
       " INNER JOIN StatiAttivitàEntità ON StatiAttivitàEntità.IdStatoAttivitàEntità = AttivitàEntità.IdStatoAttivitàEntità AND StatiAttivitàEntità.DefaultStato = 1 " & _
       " inner join entisediattuazioni on " & _
       " attivitàentisediattuazione.IdEnteSedeAttuazione = entisediattuazioni.IdEnteSedeAttuazione " & _
       " inner join entisedi on entisedi.identesede=entisediattuazioni.identesede " & _
       " inner join comuni on comuni.idcomune=entisedi.idcomune " & _
       " inner join provincie on provincie.idprovincia=comuni.idprovincia" & _
       " inner join attività on attivitàentisediattuazione.IdAttività=attività.IdAttività " & _
       " where attivitàentità.identità=" & CInt(Request.QueryString("identita")) & ""
        dtsGenerico = ClsServer.DataSetGenerico(strsql, Session("conn"))
        CaricaDataGrid(dgRisultatoRicercaSedi)
    End Sub

    Private Sub CaricaAssenze()
        strsql = "SELECT a.IDEntitàAssenza, "
        strsql = strsql & "a.Anno, "
        strsql = strsql & "(case a.Mese "
        strsql = strsql & "when 1 then 'Gennaio' "
        strsql = strsql & "when 2 then 'Febbraio' "
        strsql = strsql & "when 3 then 'Marzo' "
        strsql = strsql & "when 4 then 'Aprile' "
        strsql = strsql & "when 5 then 'Maggio' "
        strsql = strsql & "when 6 then 'Giugno' "
        strsql = strsql & "when 7 then 'Luglio' "
        strsql = strsql & "when 8 then 'Agosto' "
        strsql = strsql & "when 9 then 'Settembre' "
        strsql = strsql & "when 10 then 'Ottobre' "
        strsql = strsql & "when 11 then 'Novembre' "
        strsql = strsql & "when 12 then 'Dicembre' "
        strsql = strsql & "end) as Mese, "
        strsql = strsql & "a.Giorni, a.Note, "
        strsql = strsql & "(case a.Stato "
        strsql = strsql & " when 1 then 'Registrato' "
        strsql = strsql & "when 2 then 'Confermato' "
        strsql = strsql & "when 3 then 'Respinto' "
        strsql = strsql & "end) as Stato, a.IdCausale, "
        strsql = strsql & "case len(day(a.DataConferma)) when 1 then '0' + convert(varchar(20),day(a.DataConferma)) "
        strsql = strsql & "else convert(varchar(20),day(a.DataConferma))  end + '/' + "
        strsql = strsql & "(case len(month(a.DataConferma)) when 1 then '0' + convert(varchar(20),month(a.DataConferma)) "
        strsql = strsql & "else convert(varchar(20),month(a.DataConferma))  end + '/' + "
        strsql = strsql & "Convert(varchar(20), Year(a.DataConferma))) as DataConferma, "
        strsql = strsql & "b.Descrizione as Causale "
        strsql = strsql & "FROM EntitàAssenze as a "
        strsql = strsql & "inner join causali as b on a.IdCausale=b.IDCausale "
        strsql = strsql & "where a.idEntità=" & CInt(Request.QueryString("identita")) & " and b.tipo=10 "
        dtsGenerico = ClsServer.DataSetGenerico(strsql, Session("conn"))
        CaricaDataGrid(dgRisultatoRicercaAssenze)
        ColoraCelle()
    End Sub

    Private Function MakeParentTable(ByVal strquery As String) As DataSet
        '***Generata da Gianluigi Paesani in data:05/07/04
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
        myDataRow("ParentItem") = "Selezionare"
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
        MakeParentTable.Tables.Add(myDataTable)
    End Function

    Private Sub CaricaDataGrid(ByRef GridDaCaricare As DataGrid) 'valorizzo la datagrid passata
        GridDaCaricare.DataSource = dtsGenerico
        GridDaCaricare.DataBind()
        'If Session("TipoUtente") = "U" Then
        '    dgRisultatoRicercaAssenze.Columns(9).Visible = True
        '    dgRisultatoRicercaAssenze.Columns(10).Visible = True
        'Else
        '    dgRisultatoRicercaAssenze.Columns(9).Visible = False
        '    dgRisultatoRicercaAssenze.Columns(10).Visible = False
        'End If
    End Sub

    Private Sub ColoraCelle()
        'Generato da Alessandra Taballione il 15/06/04
        'VAriazione del Colore secondo lo stato della sede.99FF99
        'Attiva=Verde;Presentata=gialla;Cancellata=Rossa;Sospesa=
        Dim item As DataGridItem
        Dim color As New System.Drawing.Color
        Dim x As Integer
        For Each item In dgRisultatoRicercaAssenze.Items
            For x = 0 To 10
                If dgRisultatoRicercaAssenze.Items(item.ItemIndex).Cells(7).Text = "Confermato" Then
                    color = ColorTranslator.FromHtml("#99FF99") 'verde
                End If
                If dgRisultatoRicercaAssenze.Items(item.ItemIndex).Cells(7).Text = "Registrato" Then
                    'color = Khaki
                    color = ColorTranslator.FromHtml("#FFFF99") 'Khaki
                End If
                If dgRisultatoRicercaAssenze.Items(item.ItemIndex).Cells(7).Text = "Respinto" Then
                    'color = LightSalmon
                    color = ColorTranslator.FromHtml("#FF9966") 'LightSalmon
                End If
                dgRisultatoRicercaAssenze.Items(item.ItemIndex).Cells(x).BackColor = color
            Next
        Next
    End Sub

    Private Sub cmdSalva_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdSalva.Click

        If controlliSalvataggioServer() = True Then
            ControlloMensile()
            If CInt(Txttot.Value) > Session("INTgg") Then

                lblmess.Visible = True
                lblmess.Text = "Impossibile Procedere: Totale Giorni superiori alla capienza mensile"
                Exit Sub
            Else
                AggiornaDati()
                PulisciMaschera()
                txtcontrollomese.Value = ""
                Txttot.Value = ""
                Txtmodprece.Value = ""
            End If
        End If

    End Sub

    Private Sub ControlloMensile()
        'Creata da Antonello Di Croce 21/11/2005
        'in fase di inserimento
        Session("INTgg") = Nothing
        Dim dtrGenerico1 As SqlClient.SqlDataReader
        If Not dtrGenerico1 Is Nothing Then
            dtrGenerico1.Close()
            dtrGenerico1 = Nothing
        End If
        strsql = "SELECT "
        strsql = strsql & "Sum(a.Giorni) as GiorniTotAttuali "
        strsql = strsql & "FROM EntitàAssenze as a "
        strsql = strsql & "inner join causali as b on a.IdCausale=b.IDCausale "
        strsql = strsql & "where a.idEntità=" & CInt(Request.QueryString("identita")) & " and  a.Mese=" & ddlMesi.SelectedValue & " and  a.anno=" & ddlAnno.SelectedValue & " and stato= 1  "
        dtrGenerico1 = ClsServer.CreaDatareader(strsql, Session("conn"))
        dtrGenerico1.Read()
        Dim mese As String
        Dim Anno As String
        mese = ddlMesi.SelectedValue
        Anno = ddlAnno.SelectedValue
        Session("INTgg") = DateTime.DaysInMonth(CInt(Anno), CInt(mese))


        If dtrGenerico1.HasRows = True Then
            If IsDBNull(dtrGenerico1("GiorniTotAttuali")) Then
                txtcontrollomese.Value = 0
            Else
                txtcontrollomese.Value = dtrGenerico1("GiorniTotAttuali")
            End If
            Txttot.Value = CInt(txtcontrollomese.Value) + CInt(txtNumGiorni.Text)
            dtrGenerico1.Close()
            dtrGenerico1 = Nothing
        Else
            dtrGenerico1.Close()
            dtrGenerico1 = Nothing
        End If
    End Sub

    Private Sub PulisciMaschera()
        'txtdataRiferimento.Text = ""
        txtNote.Text = ""
        ddlCausale.SelectedIndex = 0
        txtNumGiorni.Text = ""
        txtidentitaassenza.Value = ""
        ddlAnno.SelectedValue = "Selezionare"
        ddlMesi.SelectedValue = 0
    End Sub

    Private Sub AggiornaDati()
        Dim null As String = "null"
        Dim item As DataGridItem
        'Se sono un utente che e' abilitato alla modifica
        If Session("utemod") = "SI" Then
            Try
                'se ho superato tutti i controlli di validita per questo utente
                If controllivari() = False Then
                    'faccio una query che mi controlla il mese che sto inserendo se risulta gia' confermato
                    strsql = " SELECT *,Convert(Datetime,DataConferma,103) as dataConf"
                    strsql = strsql & " FROM EntiConfermaAssenze "
                    strsql = strsql & " WHERE IdEnte = '" & Session("IdEnte") & "' AND Mese ='" & ddlMesi.SelectedValue & "'  AND Anno = '" & CInt(ddlAnno.SelectedItem.Text) & "'"
                    dtrgenerico = ClsServer.CreaDatareader(strsql, Session("conn"))
                    'se risulta confermato faccio la insert con stato a 2

                    If dtrgenerico.HasRows = True Then
                        Dim data As String
                        dtrgenerico.Read()
                        data = Format(dtrgenerico("dataConf"), "Short Date")
                        If Not dtrgenerico Is Nothing Then
                            dtrgenerico.Close()
                            dtrgenerico = Nothing
                        End If
                        Try
                            If controllivari() = False Then
                                myCommand = New System.Data.SqlClient.SqlCommand
                                myCommand.Connection = Session("conn")
                                If cmdSalva.Visible = True Then
                                    strsql = "INSERT INTO EntitàAssenze (IDEntità, IDCausale, Anno, Mese, Giorni, Note, Stato, UsernameInseritore, DataCreazione, DataConferma) "
                                    strsql = strsql & "VALUES (" & CInt(Request.QueryString("identita")) & "," & ddlCausale.SelectedValue & ", "
                                    strsql = strsql & CInt(ddlAnno.SelectedItem.Text) & ", " & ddlMesi.SelectedValue & ", "
                                    strsql = strsql & CInt(txtNumGiorni.Text) & ",'" & Replace(txtNote.Text, "'", "''") & "',2, "
                                    strsql = strsql & "'" & Replace(Session("Utente"), "'", "''") & "',GetDate(), CONVERT(Datetime,'" & data & "',103))"
                                Else
                                    strsql = " Update EntitàAssenze set IdCausale=" & ddlCausale.SelectedValue & ",anno=" & CInt(ddlAnno.SelectedItem.Text) & ", " & _
                                    " mese=" & ddlMesi.SelectedValue & ",Giorni=" & CInt(txtNumGiorni.Text) & ",stato = 2,note='" & Replace(txtNote.Text, "'", "''") & "',usernameUltimaModifica='" & Replace(Session("Utente"), "'", "''") & "'," & _
                                    " dataUltimaModifica=getdate(),DataConferma = CONVERT(datetime,'" & data & "',103) where IDEntitàAssenza=" & txtidentitaassenza.Value & " "
                                End If
                                MyTransaction = Session("conn").BeginTransaction(Session("IdEnte") & "_" & Session("Utente"))
                                myCommand.Transaction = MyTransaction
                                myCommand.CommandText = strsql
                                myCommand.ExecuteNonQuery()
                                MyTransaction.Commit()
                                cmdModifica.Visible = False
                                cmdSalva.Visible = True
                                PulisciMaschera()
                                CaricaAssenze()
                                MyTransaction.Dispose()
                            End If
                        Catch e As Exception
                            Response.Write(strsql)
                            Response.Write("<br>")
                            Response.Write(e.Message.ToString)
                            MyTransaction.Rollback(Session("IdEnte") & "_" & Session("Utente"))
                        End Try

                    Else
                        'se non ci sono righe faccio la insert normale
                        If Not dtrgenerico Is Nothing Then
                            dtrgenerico.Close()
                            dtrgenerico = Nothing
                        End If
                        myCommand = New System.Data.SqlClient.SqlCommand
                        myCommand.Connection = Session("conn")
                        If cmdSalva.Visible = True Then
                            strsql = "INSERT INTO EntitàAssenze (IDEntità, IDCausale, Anno, Mese, Giorni, Note, Stato, UsernameInseritore, DataCreazione) "
                            strsql = strsql & "VALUES (" & CInt(Request.QueryString("identita")) & "," & ddlCausale.SelectedValue & ", "
                            strsql = strsql & CInt(ddlAnno.SelectedItem.Text) & ", " & ddlMesi.SelectedValue & ", "
                            strsql = strsql & CInt(txtNumGiorni.Text) & ",'" & Replace(txtNote.Text, "'", "''") & "',1, "
                            strsql = strsql & "'" & Replace(Session("Utente"), "'", "''") & "',GetDate())"
                        Else
                            strsql = " Update EntitàAssenze set IdCausale=" & ddlCausale.SelectedValue & ",anno=" & CInt(ddlAnno.SelectedItem.Text) & ", " & _
                            " mese=" & ddlMesi.SelectedValue & ",Giorni=" & CInt(txtNumGiorni.Text) & ",stato = 1,DataConferma=NULL,note='" & Replace(txtNote.Text, "'", "''") & "',usernameUltimaModifica='" & Replace(Session("Utente"), "'", "''") & "'," & _
                            " dataUltimaModifica=getdate() where IDEntitàAssenza=" & txtidentitaassenza.Value & " "
                        End If
                        MyTransaction = Session("conn").BeginTransaction(Session("IdEnte") & "_" & Session("Utente"))
                        myCommand.Transaction = MyTransaction
                        myCommand.CommandText = strsql
                        myCommand.ExecuteNonQuery()
                        MyTransaction.Commit()
                        cmdModifica.Visible = False
                        cmdSalva.Visible = True
                        PulisciMaschera()
                        CaricaAssenze()
                        MyTransaction.Dispose()
                    End If
                End If
            Catch e As Exception
                Response.Write(strsql)
                Response.Write("<br>")
                Response.Write(e.Message.ToString)
                MyTransaction.Rollback(Session("IdEnte") & "_" & Session("Utente"))
            End Try
        Else
            'se non sono un utente con il profilo abilitato alla modifica
            Try
                If controllivari() = False Then
                    myCommand = New System.Data.SqlClient.SqlCommand
                    myCommand.Connection = Session("conn")
                    If cmdSalva.Visible = True Then
                        strsql = "INSERT INTO EntitàAssenze (IDEntità, IDCausale, Anno, Mese, Giorni, Note, Stato, UsernameInseritore, DataCreazione) "
                        strsql = strsql & "VALUES (" & CInt(Request.QueryString("identita")) & "," & ddlCausale.SelectedValue & ", "
                        strsql = strsql & CInt(ddlAnno.SelectedItem.Text) & ", " & ddlMesi.SelectedValue & ", "
                        strsql = strsql & CInt(txtNumGiorni.Text) & ",'" & Replace(txtNote.Text, "'", "''") & "',1, "
                        strsql = strsql & "'" & Replace(Session("Utente"), "'", "''") & "',GetDate())"
                    Else
                        strsql = " Update EntitàAssenze set IdCausale=" & ddlCausale.SelectedValue & ",anno=" & CInt(ddlAnno.SelectedItem.Text) & ", " & _
                        " mese=" & ddlMesi.SelectedValue & ",Giorni=" & CInt(txtNumGiorni.Text) & ",note='" & Replace(txtNote.Text, "'", "''") & "',usernameUltimaModifica='" & Replace(Session("Utente"), "'", "''") & "'," & _
                        " dataUltimaModifica=getdate() where IDEntitàAssenza=" & txtidentitaassenza.Value & " "
                    End If
                    MyTransaction = Session("conn").BeginTransaction(Session("IdEnte") & "_" & Session("Utente"))
                    myCommand.Transaction = MyTransaction
                    myCommand.CommandText = strsql
                    myCommand.ExecuteNonQuery()
                    MyTransaction.Commit()
                    cmdModifica.Visible = False
                    cmdSalva.Visible = True
                    PulisciMaschera()
                    CaricaAssenze()
                    MyTransaction.Dispose()
                End If
            Catch e As Exception
                Response.Write(strsql)
                Response.Write("<br>")
                Response.Write(e.Message.ToString)
                MyTransaction.Rollback(Session("IdEnte") & "_" & Session("Utente"))
            End Try
        End If
    End Sub

    Function controllivari() As Boolean
        controllivari = False
        'serie di controlli prima di effettuare operazioni sulla base dati
        '*********controllo se la causale d'assenza è stata già usata per il mese e l'anno selezionati
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
        strsql = "select IDEntitàAssenza from EntitàAssenze "
        strsql = strsql & "where stato in (1,2) and identità=" & CInt(Request.QueryString("identita")) & " and "
        strsql = strsql & "idcausale=" & ddlCausale.SelectedValue & " and "
        strsql = strsql & "Anno=" & CInt(ddlAnno.SelectedItem.Text) & " and "
        strsql = strsql & "Mese=" & ddlMesi.SelectedValue & " "

        If cmdSalva.Visible = False Then
            strsql = strsql & "and IdEntitàAssenza<>" & CInt(txtidentitaassenza.Value) & ""
        End If

        dtrgenerico = ClsServer.CreaDatareader(strsql, Session("conn"))

        If dtrgenerico.HasRows = True Then
            'imposto il valore di ritonrno = true così che non eseguo la insert per la nuova assenza
            lblmess.Visible = True
            lblmess.Text = "La Causale è già prevista per il periodo indicato."
            controllivari = True
            If Not dtrgenerico Is Nothing Then
                dtrgenerico.Close()
                dtrgenerico = Nothing
            End If
            Exit Function
        End If

        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If
        '****************************************************************************************
        '*****controllo se il mese immesso è compreso fra la data d'inizio e la data di fine*****
        If Year(lblDataInizio.Text) <> Year(lbldataFine.Text) Then
            If (ddlAnno.SelectedItem.Text = Year(lblDataInizio.Text)) And (ddlMesi.SelectedValue < CInt(Month(lblDataInizio.Text))) Then
                'imposto il valore di ritonrno = true così che non eseguo la insert per la nuova assenza
                lblmess.Visible = True
                lblmess.Text = "Il periodo indicato è precedente alla data di inizio del Progetto."
                controllivari = True
                If Not dtrgenerico Is Nothing Then
                    dtrgenerico.Close()
                    dtrgenerico = Nothing
                End If
                Exit Function
            End If
            If (ddlAnno.SelectedItem.Text = Year(lbldataFine.Text)) And (ddlMesi.SelectedValue > CInt(Month(lbldataFine.Text))) Then
                'imposto il valore di ritonrno = true così che non eseguo la insert per la nuova assenza
                lblmess.Visible = True
                lblmess.Text = "Il periodo indicato è successivo alla data di fine del Progetto."
                controllivari = True
                If Not dtrgenerico Is Nothing Then
                    dtrgenerico.Close()
                    dtrgenerico = Nothing
                End If
                Exit Function
            End If
        End If
        '****************************************************************************************
        'Aggiunto da Alessandra Taballione  il 01/08/2005
        'Verifico se assenze confermate per quel mese e anno
        If Session("utemod") = "SI" Then
            controllivari = False
        Else
            strsql = "select * from enticonfermaAssenze " & _
            " where idente=" & Session("idEnte") & " and anno=" & ddlAnno.SelectedItem.Text & " and mese=" & ddlMesi.SelectedValue & ""
            If Not dtrgenerico Is Nothing Then
                dtrgenerico.Close()
                dtrgenerico = Nothing
            End If
            dtrgenerico = ClsServer.CreaDatareader(strsql, Session("conn"))
            If dtrgenerico.HasRows = True Then
                lblmess.Visible = True
                lblmess.Text = "Non è possibile effettuare l'inserimento dell'assenza perchè l'Ente ha gia' Confermato le assenze per l'anno " & ddlAnno.SelectedItem.Text & " e per il mese di " & ddlMesi.SelectedItem.Text & "."
                controllivari = True
                If Not dtrgenerico Is Nothing Then
                    dtrgenerico.Close()
                    dtrgenerico = Nothing
                End If
                Exit Function
            End If
            If Not dtrgenerico Is Nothing Then
                dtrgenerico.Close()
                dtrgenerico = Nothing
            End If
        End If
    End Function

    Private Sub cmdModifica_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdModifica.Click

        If controlliSalvataggioServer() = True Then
            ControlloMensileMod()
            If CInt(Txttot.Value) > Session("INTgg") Then
                lblmess.Visible = True
                lblmess.Text = "Impossibile Procedere: Totale Giorni superiori alla capienza mensile"
                Exit Sub
            Else
                AggiornaDati()
                txtcontrollomese.Value = ""
                Txttot.Value = ""
                Txtmodprece.Value = ""
                'PulisciMaschera()
            End If
        End If

    End Sub

    Private Sub ControlloMensileMod()
        'Creata da Antonello Di Croce 21/11/2005
        'in fase di modifica
        Session("INTgg") = Nothing
        Dim dtrGenerico2 As SqlClient.SqlDataReader
        If Not dtrGenerico2 Is Nothing Then
            dtrGenerico2.Close()
            dtrGenerico2 = Nothing
        End If
        strsql = "SELECT "
        strsql = strsql & "Sum(a.Giorni) as GiorniTotAttuali "
        strsql = strsql & "FROM EntitàAssenze as a "
        strsql = strsql & "inner join causali as b on a.IdCausale=b.IDCausale "
        strsql = strsql & "where a.idEntità=" & CInt(Request.QueryString("identita")) & " and  a.Mese=" & ddlMesi.SelectedValue & " and  a.anno=" & ddlAnno.SelectedValue & " and stato= 1  "
        dtrGenerico2 = ClsServer.CreaDatareader(strsql, Session("conn"))
        dtrGenerico2.Read()


        Dim mese As String
        Dim Anno As String
        mese = ddlMesi.SelectedValue
        Anno = ddlAnno.SelectedValue
        Session("INTgg") = DateTime.DaysInMonth(CInt(Anno), CInt(mese))

        If dtrGenerico2.HasRows = True Then
            If IsDBNull(dtrGenerico2("GiorniTotAttuali")) Then
                txtcontrollomese.Value = 0
            Else
                txtcontrollomese.Value = dtrGenerico2("GiorniTotAttuali") - CInt(Txtmodprece.Value)
            End If
            Txttot.Value = CInt(txtcontrollomese.Value) + CInt(txtNumGiorni.Text)
            dtrGenerico2.Close()
            dtrGenerico2 = Nothing
        Else
            dtrGenerico2.Close()
            dtrGenerico2 = Nothing
        End If
    End Sub

    Private Sub cmdChiudi_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdChiudi.Click
        Response.Redirect("RicercaSanzioniVolontari.aspx?VengoDa=" & Request.QueryString("VengoDa") & "")
    End Sub

    Private Sub dgRisultatoRicercaAssenze_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgRisultatoRicercaAssenze.PageIndexChanged
        dgRisultatoRicercaAssenze.SelectedIndex = -1
        dgRisultatoRicercaAssenze.EditItemIndex = -1
        dgRisultatoRicercaAssenze.CurrentPageIndex = e.NewPageIndex
        CaricaAssenze()
    End Sub

    Private Sub dgRisultatoRicercaAssenze_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgRisultatoRicercaAssenze.ItemCommand
        If e.CommandName = "Conferma" Then
            If e.Item.Cells(7).Text = "Registrato" Then
                strsql = "Update EntitàAssenze set stato=2, usernameConferma='" & Session("Utente") & "',dataConferma=getdate()  where IDEntitàAssenza=" & e.Item.Cells(1).Text & " "
                myCommand = ClsServer.EseguiSqlClient(strsql, Session("conn"))
                CaricaAssenze()
                lblmess.Visible = False
                lblmess.Text = ""
            Else
                lblmess.Visible = True
                lblmess.Text = "Non è possibile Confermare l'Assenza Selezionata."
            End If
        End If

        'se sono un utente abilitato alla modifica
        If Session("utemod") = "SI" Then
            If e.CommandName = "Respingi" Then
                strsql = "Update EntitàAssenze set stato=3, usernameConferma='" & Session("Utente") & "',dataConferma=getdate()  where idEntitàAssenza=" & e.Item.Cells(1).Text & ""
                myCommand = ClsServer.EseguiSqlClient(strsql, Session("conn"))
                CaricaAssenze()
                lblmess.Visible = False
                lblmess.Text = ""
            End If
        Else
            If e.CommandName = "Respingi" Then
                If e.Item.Cells(7).Text = "Registrato" Then
                    strsql = "Update EntitàAssenze set stato=3, usernameConferma='" & Session("Utente") & "',dataConferma=getdate()  where idEntitàAssenza=" & e.Item.Cells(1).Text & ""
                    myCommand = ClsServer.EseguiSqlClient(strsql, Session("conn"))
                    CaricaAssenze()
                    lblmess.Visible = False
                    lblmess.Text = ""
                Else
                    lblmess.Visible = True
                    lblmess.Text = "Non è possibile Rimuovere l'Assenza Selezionata."
                End If
            End If
        End If
        'se sono un utente abilitato alla modifica
        If Session("utemod") = "SI" Then
            If e.CommandName = "Modifica" Then
                lblmess.Visible = False
                lblmess.Text = ""
                cmdSalva.Visible = False
                cmdModifica.Visible = True
                'txtdataRiferimento.Text = e.Item.Cells(1).Text
                ddlCausale.SelectedValue = e.Item.Cells(11).Text
                txtNumGiorni.Text = Replace(e.Item.Cells(4).Text, ",", ".")
                Txtmodprece.Value = txtNumGiorni.Text

                Dim item As ListItem

                For Each item In ddlAnno.Items
                    If e.Item.Cells(2).Text = item.Text Then
                        ddlAnno.SelectedValue = item.Value
                    End If
                Next

                For Each item In ddlMesi.Items
                    If e.Item.Cells(3).Text = item.Text Then
                        ddlMesi.SelectedValue = item.Value
                    End If
                Next
                txtNote.Text = IIf(e.Item.Cells(8).Text = "&nbsp;", "", e.Item.Cells(8).Text)
                txtidentitaassenza.Value = e.Item.Cells(1).Text
            End If
        Else
            If e.CommandName = "Modifica" Then
                If e.Item.Cells(7).Text = "Registrato" Then
                    lblmess.Visible = False
                    lblmess.Text = ""
                    cmdSalva.Visible = False
                    cmdModifica.Visible = True
                    'txtdataRiferimento.Text = e.Item.Cells(1).Text
                    ddlCausale.SelectedValue = e.Item.Cells(11).Text
                    txtNumGiorni.Text = Replace(e.Item.Cells(4).Text, ",", ".")
                    Txtmodprece.Value = txtNumGiorni.Text
                    Dim item As ListItem
                    For Each item In ddlAnno.Items
                        If e.Item.Cells(2).Text = item.Text Then
                            ddlAnno.SelectedValue = item.Value
                        End If
                    Next

                    For Each item In ddlMesi.Items
                        If e.Item.Cells(3).Text = item.Text Then
                            ddlMesi.SelectedValue = item.Value
                        End If
                    Next
                    txtNote.Text = IIf(e.Item.Cells(8).Text = "&nbsp;", "", e.Item.Cells(8).Text)
                    txtidentitaassenza.Value = e.Item.Cells(1).Text
                Else
                    lblmess.Visible = True
                    lblmess.Text = "Non è possibile Modificare l'Assenza Selezionata."
                End If
            End If
        End If
    End Sub

    Private Sub dgRisultatoRicercaSedi_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgRisultatoRicercaSedi.PageIndexChanged
        dgRisultatoRicercaSedi.SelectedIndex = -1
        dgRisultatoRicercaSedi.EditItemIndex = -1
        dgRisultatoRicercaSedi.CurrentPageIndex = e.NewPageIndex
        CaricaSediAttuazione()
    End Sub

    Function controlliSalvataggioServer() As Boolean

        If ddlAnno.SelectedIndex = 0 Then
            lblmess.Visible = True
            lblmess.Text = "E' necessario selezionare l'Anno."
            Return False
        End If

        If ddlMesi.SelectedIndex = 0 Then
            lblmess.Visible = True
            lblmess.Text = "E' necessario selezionare il Mese."
            Return False
        End If

        If ddlCausale.SelectedIndex = 0 Then
            lblmess.Visible = True
            lblmess.Text = "E' necessario selezionare la Causale."
            Return False
        End If

        If txtNumGiorni.Text.Trim = String.Empty Then
            lblmess.Visible = True
            lblmess.Text = "E' necessario inserire il numero dei giorni."
            Return False
        End If

        Dim numeroGiorni As Integer
        Dim numeroGiorniInteger As Boolean
        numeroGiorniInteger = Integer.TryParse(txtNumGiorni.Text.Trim, numeroGiorni)

        If numeroGiorniInteger = True Then
            If numeroGiorni = 0 Then
                lblmess.Visible = True
                lblmess.Text = "Il numero dei giorni inseriti deve essere maggiore di Zero."
                Return False
            End If
        Else
            lblmess.Visible = True
            lblmess.Text = "Il numero dei giorni inseriti deve essere un numero intero."
            Return False
        End If

        Return True

    End Function

End Class