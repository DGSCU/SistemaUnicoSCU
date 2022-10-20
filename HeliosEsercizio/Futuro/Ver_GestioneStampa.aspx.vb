Public Class Ver_GestioneStampa
    '***************************************************************************
    'ANTONELLO DI CROCE 18/06/2008
    'Maschera per la gestione delle stampe e produzione stampa per la verifica selezionata
    '***************************************************************************
    Inherits System.Web.UI.Page

    'Protected WithEvents dgRisultatoRicerca As System.Web.UI.WebControls.DataGrid
    'Protected WithEvents imgConferma As System.Web.UI.WebControls.ImageButton
    'Protected WithEvents lblmessaggio As System.Web.UI.WebControls.Label
    'Protected WithEvents ImgVerde As System.Web.UI.WebControls.Image
    'Protected WithEvents LblVerde As System.Web.UI.WebControls.Label
    'Protected WithEvents Image1 As System.Web.UI.WebControls.Image
    'Protected WithEvents Label3 As System.Web.UI.WebControls.Label
    'Protected WithEvents ddlVerificatoreInterno As System.Web.UI.WebControls.DropDownList
    'Protected WithEvents ddlTipologiaVerifica As System.Web.UI.WebControls.DropDownList
    'Protected WithEvents Label2 As System.Web.UI.WebControls.Label
    'Protected WithEvents ddlProgrammazione As System.Web.UI.WebControls.DropDownList
    'Protected WithEvents cmdRicerca As System.Web.UI.WebControls.ImageButton
    'Protected WithEvents cmdChiudi As System.Web.UI.WebControls.ImageButton
    'Protected WithEvents Label1 As System.Web.UI.WebControls.Label
    'Protected WithEvents chkSelDesel As System.Web.UI.WebControls.CheckBox
    Dim strsql As String
#Region " Codice generato da Progettazione Web Form "

    'Chiamata richiesta da Progettazione Web Form.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    ''  Protected WithEvents Form1 As System.Web.UI.HtmlControls.HtmlForm

    'NOTA: la seguente dichiarazione è richiesta da Progettazione Web Form.
    'Non spostarla o rimuoverla.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: questa chiamata al metodo è richiesta da Progettazione Web Form.
        'Non modificarla nell'editor del codice.
        InitializeComponent()
    End Sub

#End Region
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Inserire qui il codice utente necessario per inizializzare la pagina

        If IsPostBack = False Then
            Session("DtbStampaMultipla") = Nothing

            CaricaVerificatore()
            CaricaProgrammazione()

            ddlTipologiaVerifica.Items.Add("")
            ddlTipologiaVerifica.Items(0).Value = 0
            ddlTipologiaVerifica.Items.Add("Programmata")
            ddlTipologiaVerifica.Items(1).Value = 1
            ddlTipologiaVerifica.Items.Add("Segnalata")
            ddlTipologiaVerifica.Items(2).Value = 2
        End If
    End Sub
    Private Function MakeParentTable(ByVal strquery As String) As DataSet

        '***Inizializzo e carico datatable 
        Dim dtrgenerico As Data.SqlClient.SqlDataReader
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
        dtrgenerico = ClsServer.CreaDatareader(strquery, Session("conn"))
        'Instantiate the DataSet variable.
        'mydataset = New DataSet
        ' Add the new DataTable to the DataSet.
        'mydataset.Tables.Add(myDataTable)
        myDataRow = myDataTable.NewRow()
        myDataRow("id") = 0
        myDataRow("ParentItem") = ""
        myDataTable.Rows.Add(myDataRow)
        Do While dtrgenerico.Read
            myDataRow = myDataTable.NewRow()
            myDataRow("id") = dtrgenerico.GetValue(0)
            myDataRow("ParentItem") = dtrgenerico.GetValue(1)
            myDataTable.Rows.Add(myDataRow)
        Loop

        dtrgenerico.Close()
        dtrgenerico = Nothing
        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If

        MakeParentTable = New DataSet
        MakeParentTable.Tables.Add(myDataTable)
    End Function
    Private Sub CaricaProgrammazione()

        Dim strsql As String
        Dim dtrProg As SqlClient.SqlDataReader
        If Not dtrProg Is Nothing Then
            dtrProg.Close()
            dtrProg = Nothing
        End If
        strsql = " Select idprogrammazione , tverificheprogrammazione.Descrizione " & _
                 " From tverificheprogrammazione " & _
                 " inner Join RegioniCompetenze on tverificheprogrammazione.IdRegCompetenza = RegioniCompetenze.IdRegioneCompetenza Where tverificheprogrammazione.IdRegCompetenza = 22"

        strsql = strsql & "  union Select '0' ,'' From tverificheprogrammazione   "
        strsql = strsql & "  inner Join RegioniCompetenze on tverificheprogrammazione.IdRegCompetenza = RegioniCompetenze.IdRegioneCompetenza  "


        strsql = strsql & " Order by idprogrammazione"
        dtrProg = ClsServer.CreaDatareader(strsql, Session("conn"))
        ddlProgrammazione.DataSource = dtrProg
        ddlProgrammazione.DataValueField = "idprogrammazione"
        ddlProgrammazione.DataTextField = "descrizione"
        ddlProgrammazione.DataBind()
        If Not dtrProg Is Nothing Then
            dtrProg.Close()
            dtrProg = Nothing
        End If

    End Sub
    Private Sub CaricaVerificatore()
        Dim strsql As String

        strsql = "select IdVerificatore, (Cognome + ' ' + Nome) As Nome " & _
                 " from TVerificatori WHERE Tipologia=0 AND Abilitato=0  order by Nome"
       
        ddlVerificatoreInterno.DataSource = MakeParentTable(strsql)
        ddlVerificatoreInterno.DataTextField = "ParentItem"
        ddlVerificatoreInterno.DataValueField = "id"
        ddlVerificatoreInterno.DataBind()

    End Sub
    Private Sub EseguiRicerca()
        Dim Mydataset As New DataSet
        
        If ddlVerificatoreInterno.SelectedValue > 0 Then

            strsql = "select distinct IdVerifica, CodiceFascicolo, StatoVerifiche,programmazione, " & _
                               " dbo.FormatoData(DataPrevistaVerifica) as DataPrevistaVerifica, dbo.FormatoData(DataFinePrevistaVerifica) as DataFinePrevistaVerifica, dbo.FormatoData(DataFineVerifica) as DataFineVerifica,tipoverificatore, " & _
                               " (Nominativo + '(' + case when tipoverificatore = 0 then 'Interno' when tipoverificatore = 1 then 'IGF' END + ')') as Nominativo, " & _
                               " (Denominazione + '(' + CodiceEnte + ')') as Denominazione, " & _
                               " (Titolo + '(' + CodiceProgetto + ')') as Titolo, dbo.FormatoData(DataInizioAttività) as DataInizioAttività, dbo.FormatoData(DataFineAttività) as DataFineAttività,  " & _
                               " case when tipologia = 1 then 'Programmata' when tipologia = 2 then 'Segnalata' end as TipoVerifica,idprogrammazione,idente,(Comune + '(' + descrabb +  ')') as ComuneAbb,regione,(EnteFiglio + ' ' + '[' + CAST(Identesedeattuazione as varchar ) + ']') as SedeIn,(Indirizzo + ' ' + Civico ) as IndirizzoCiv,ver_vw_ricerca_verifiche.IdVerificheAssociate,Max(TVerificheStampa.DataStampa)as DataStampa " & _
                               " from ver_vw_ricerca_verifiche left join  TVerificheStampa on ver_vw_ricerca_verifiche.IdVerificheAssociate = TVerificheStampa.IdVerificheAssociate where idstatoverifica=5 and idRegCompetenza=22 and TipoVerificatore=0 "

            If ddlVerificatoreInterno.SelectedValue <> 0 Then
                strsql = strsql & " and idverificatore = '" & ddlVerificatoreInterno.SelectedValue & "'"
            End If

            If ddlProgrammazione.SelectedValue <> "" Then
                If ddlProgrammazione.SelectedValue <> 0 Then
                    strsql = strsql & " and  idprogrammazione = '" & ddlProgrammazione.SelectedValue & "'"
                End If
            End If

            If ddlTipologiaVerifica.SelectedValue <> 0 Then
                strsql = strsql & " and  tipologia = '" & ddlTipologiaVerifica.SelectedValue & "'"
            End If

            strsql = strsql & " group by IdVerifica, CodiceFascicolo, StatoVerifiche,programmazione,  DataPrevistaVerifica, DataFinePrevistaVerifica, DataFineVerifica,  tipoverificatore , nominativo, Denominazione , codiceente, Titolo, CodiceProgetto , DataInizioAttività,DataFineAttività,  tipologia ,idprogrammazione,idente,Comune ,descrabb ,regione,EnteFiglio, Identesedeattuazione ,Indirizzo ,Civico ,ver_vw_ricerca_verifiche.IdVerificheAssociate"

            Mydataset = ClsServer.DataSetGenerico(strsql, Session("conn"))
            dgRisultatoRicerca.DataSource = Mydataset
            dgRisultatoRicerca.DataBind()

            Call ColoraCelle()

            Session("DtbStampaMultipla") = Mydataset.Tables(0)

            If Mydataset.Tables(0).Rows.Count > 0 Then
                chkSelDesel.Visible = True
                imgConferma.Visible = True
                ImgVerde.Visible = True
                LblVerde.Visible = True
            End If
        Else
            lblmessaggio.Visible = True
            lblmessaggio.Text = "Selezionare almeno un Verificatore dalla lista"
            Exit Sub
        End If

    End Sub
    Private Sub imgConferma_Click(sender As Object, e As EventArgs) Handles imgConferma.Click
        Dim i As Integer
        Dim bolVerifica As Boolean
        Dim dtrGenerico As SqlClient.SqlDataReader
        bolVerifica = False

        Call InserisciStampe()

        For i = 0 To dgRisultatoRicerca.Items.Count - 1
            If dgRisultatoRicerca.Items.Item(i).Cells(5).Text = 1 Then
                bolVerifica = True
                Exit For
            End If
        Next

        If bolVerifica = False Then
            lblmessaggio.Visible = True
            lblmessaggio.Text = "Selezionare almeno un Verifica dalla lista"
        Else
            lblmessaggio.Visible = True
            lblmessaggio.Text = "DATI PRONTI PER LA STAMPA"

        End If

        'scrivi qui per fare la stampa
        '''dim pippo as New 'generatore modelli
        If Not dtrGenerico Is Nothing Then
            dtrGenerico.Close()
            dtrGenerico = Nothing
        End If

        Call EseguiRicerca()
    End Sub
    Private Sub InserisciStampe(Optional ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs = Nothing)
        Dim dtrGenerico As SqlClient.SqlDataReader
        Dim i As Integer
        Dim Mychk As CheckBox
        Dim STRSQL1 As String
        Dim MAXID As Integer
        Dim strsql As String
        Dim Print As New GeneratoreModelli
        Dim cmdGenerico As SqlClient.SqlCommand
        STRSQL1 = "SELECT Max(IdGruppoStampa) as MAXID FROM  TVerificheStampa"

        If Not dtrGenerico Is Nothing Then
            dtrGenerico.Close()
            dtrGenerico = Nothing
        End If
        dtrGenerico = ClsServer.CreaDatareader(STRSQL1, Session("conn"))

        dtrGenerico.Read()
        If IsDBNull(dtrGenerico("MAXID")) = True Then
            MAXID = 1
        Else
            MAXID = dtrGenerico("MAXID") + 1
        End If

        If Not dtrGenerico Is Nothing Then
            dtrGenerico.Close()
            dtrGenerico = Nothing
        End If

        For i = 0 To dgRisultatoRicerca.Items.Count - 1
            Mychk = dgRisultatoRicerca.Items.Item(i).FindControl("chkSelProg")
            If Mychk.Checked = True Then
                dgRisultatoRicerca.Items.Item(i).Cells(5).Text = 1
                strsql = "insert into TVerificheStampa (IdGruppoStampa, IdVerificheAssociate,DataStampa, UserName ) " & _
                            " VALUES (" & MAXID & "," & dgRisultatoRicerca.Items.Item(i).Cells(11).Text & ",getdate(),'" & Session("Utente") & "')"
                cmdGenerico = New SqlClient.SqlCommand(strsql, IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn")))
                cmdGenerico.ExecuteNonQuery()
                cmdGenerico.Dispose()
            Else
                dgRisultatoRicerca.Items.Item(i).Cells(5).Text = 0
            End If
        Next i

        If Not dtrGenerico Is Nothing Then
            dtrGenerico.Close()
            dtrGenerico = Nothing
        End If

        Response.Write("<script>" & vbCrLf)
        Response.Write("window.open('" & Print.MON_Letteradiincaricomultipla(MAXID, Session("Utente"), Session("IdRegioneCompetenzaUtente"), Session("conn")) & "')" & vbCrLf)
        Response.Write("</script>" & vbCrLf)

    End Sub
    Private Sub ColoraCelle()
        Dim item As DataGridItem
        Dim color As New System.Drawing.Color
        Dim Mychk As CheckBox
        Dim x As Integer

        For Each item In dgRisultatoRicerca.Items
            If dgRisultatoRicerca.Items(item.ItemIndex).Cells(12).Text <> "&nbsp;" Then
                For x = 0 To 12
                    color = color.LightGreen
                    dgRisultatoRicerca.Items(item.ItemIndex).Cells(x).BackColor = color
                Next
            End If
        Next
    End Sub
    Private Sub cmdChiudi_Click(sender As Object, e As EventArgs) Handles cmdChiudi.Click
        Response.Redirect("WfrmMain.aspx")
    End Sub
    Private Sub cmdRicerca_Click(sender As Object, e As EventArgs) Handles cmdRicerca.Click
        EseguiRicerca()
    End Sub

  
     
    Protected Sub chkSelDesel_CheckedChanged(sender As Object, e As EventArgs) Handles chkSelDesel.CheckedChanged




        Dim nRighe As Integer
        Dim x As Integer
        nRighe = dgRisultatoRicerca.Items.Count
        For x = 0 To nRighe - 1
            Dim chkoggetto As CheckBox = dgRisultatoRicerca.Items(x).Cells(0).FindControl("chkSelProg")

            chkoggetto.Checked = chkSelDesel.Checked

        Next






    End Sub
End Class