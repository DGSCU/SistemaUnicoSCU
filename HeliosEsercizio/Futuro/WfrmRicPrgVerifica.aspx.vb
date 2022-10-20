Public Class WfrmRicPrgVerifica
    Inherits System.Web.UI.Page

#Region " Codice generato da Progettazione Web Form "

    'Chiamata richiesta da Progettazione Web Form.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()

    End Sub
    ' Protected WithEvents dgRisultatoRicerca As System.Web.UI.WebControls.DataGrid
    'Protected WithEvents imgEsporta As System.Web.UI.WebControls.Button
    '   Protected WithEvents cmdChiudi As System.Web.UI.WebControls.ImageButton
    '  Protected WithEvents cmdSalva As System.Web.UI.WebControls.ImageButton
    'Protected WithEvents txtTitoloProgetto1 As System.Web.UI.WebControls.TextBox
    'Protected WithEvents ddlMaccCodAmAtt1 As System.Web.UI.WebControls.TextBox
    'Protected WithEvents ddlStatoAttivita1 As System.Web.UI.WebControls.TextBox
    'Protected WithEvents txtDenominazioneEnte1 As System.Web.UI.WebControls.TextBox
    'Protected WithEvents ddlCodAmAtt1 As System.Web.UI.WebControls.TextBox
    'Protected WithEvents txtbando1 As System.Web.UI.WebControls.TextBox
    'Protected WithEvents Image1 As System.Web.UI.WebControls.Image
    'Protected WithEvents pnlCarattProgetti As System.Web.UI.WebControls.Panel
    'Protected WithEvents Label1 As System.Web.UI.WebControls.Label
    'Protected WithEvents codpog As System.Web.UI.WebControls.Label
    'Protected WithEvents TxtCodPog As System.Web.UI.WebControls.TextBox
    'Protected WithEvents nomepog As System.Web.UI.WebControls.Label
    'Protected WithEvents TxtDescPog As System.Web.UI.WebControls.TextBox
    'Protected WithEvents Label2 As System.Web.UI.WebControls.Label
    'Protected WithEvents DdlBando As System.Web.UI.WebControls.DropDownList
    'Protected WithEvents Label3 As System.Web.UI.WebControls.Label
    'Protected WithEvents ddlMaccCodAmAtt As System.Web.UI.WebControls.DropDownList
    'Protected WithEvents Label4 As System.Web.UI.WebControls.Label
    'Protected WithEvents ddlCodAmAtt As System.Web.UI.WebControls.DropDownList
    'Protected WithEvents Label5 As System.Web.UI.WebControls.Label
    'Protected WithEvents TxtNumVolontari As System.Web.UI.WebControls.TextBox
    'Protected WithEvents Label6 As System.Web.UI.WebControls.Label
    'Protected WithEvents Label7 As System.Web.UI.WebControls.Label
    'Protected WithEvents ddlTipoProgetto As System.Web.UI.WebControls.DropDownList
    'Protected WithEvents Label8 As System.Web.UI.WebControls.Label
    'Protected WithEvents Panel1 As System.Web.UI.WebControls.Panel
    'Protected WithEvents Label9 As System.Web.UI.WebControls.Label
    'Protected WithEvents ddlClasse As System.Web.UI.WebControls.DropDownList
    'Protected WithEvents Label10 As System.Web.UI.WebControls.Label
    'Protected WithEvents ddlTipologia As System.Web.UI.WebControls.DropDownList
    'Protected WithEvents Label11 As System.Web.UI.WebControls.Label
    'Protected WithEvents ddlCompetenza As System.Web.UI.WebControls.DropDownList
    'Protected WithEvents Label12 As System.Web.UI.WebControls.Label
    'Protected WithEvents Panel2 As System.Web.UI.WebControls.Panel
    'Protected WithEvents Label13 As System.Web.UI.WebControls.Label
    'Protected WithEvents TxtComune As System.Web.UI.WebControls.TextBox
    'Protected WithEvents Label14 As System.Web.UI.WebControls.Label
    'Protected WithEvents TxtProvincia As System.Web.UI.WebControls.TextBox
    'Protected WithEvents Label15 As System.Web.UI.WebControls.Label
    'Protected WithEvents TxtRegione As System.Web.UI.WebControls.TextBox
    'Protected WithEvents Label16 As System.Web.UI.WebControls.Label
    'Protected WithEvents Label17 As System.Web.UI.WebControls.Label
    'Protected WithEvents TxtInizioPog As System.Web.UI.WebControls.TextBox
    'Protected WithEvents Label18 As System.Web.UI.WebControls.Label
    'Protected WithEvents TxtFinePog As System.Web.UI.WebControls.TextBox
    'Protected WithEvents Label19 As System.Web.UI.WebControls.Label
    'Protected WithEvents TxtAlladata As System.Web.UI.WebControls.TextBox
    'Protected WithEvents Panel3 As System.Web.UI.WebControls.Panel
    'Protected WithEvents Label20 As System.Web.UI.WebControls.Label
    'Protected WithEvents TxtCodEnte As System.Web.UI.WebControls.TextBox
    'Protected WithEvents Label21 As System.Web.UI.WebControls.Label
    'Protected WithEvents TxtDescrEnte As System.Web.UI.WebControls.TextBox
    'Protected WithEvents imgcalendarioDI As System.Web.UI.WebControls.Image
    'Protected WithEvents imgcalendarioAD As System.Web.UI.WebControls.Image
    'Protected WithEvents imgcalendarioDF As System.Web.UI.WebControls.Image
    'Protected WithEvents chkLTipoPosto As System.Web.UI.WebControls.CheckBoxList
    'Protected WithEvents Label24 As System.Web.UI.WebControls.Label
    'Protected WithEvents ddlCompetenzaProgetto As System.Web.UI.WebControls.DropDownList

    ''NOTA: la seguente dichiarazione è richiesta da Progettazione Web Form.
    ''Non spostarla o rimuoverla.
    Private designerPlaceholderDeclaration As System.Object
    'Protected WithEvents LblCrea As System.Web.UI.WebControls.Label
    'Protected WithEvents cmdInsersci As System.Web.UI.WebControls.ImageButton
    'Protected WithEvents imgStampa As System.Web.UI.WebControls.ImageButton
    'Protected WithEvents lblMsgSede As System.Web.UI.WebControls.Label
    Dim DataSetRicerca As DataSet

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: questa chiamata al metodo è richiesta da Progettazione Web Form.
        'Non modificarla nell'editor del codice.
        InitializeComponent()
    End Sub

#End Region

    Public Verifica As New clsVerificaSegnalazione

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        If Not IsPostBack Then
            If Request.QueryString("VengoDa") Is Nothing Then
                If Request.QueryString("modifica") <> "" Then
                    Verifica = CType(Context.Handler, WfrmGestioneVerModSegnalazione).Verifica
                Else
                    Verifica = CType(Context.Handler, WfrmGestioneVerSegnalazione).Verifica
                End If
            End If

            Me.ViewState.Add("Verifica", Me.Verifica)
            If Not Session("LogIn") Is Nothing Then
                If Session("LogIn") = False Then 'verifico validità log-in
                    Response.Redirect("LogOn.aspx")
                End If
            Else
                Response.Redirect("LogOn.aspx")
            End If
            If IsPostBack = False Then
                CaricaPrima()
                CaricaCompetenze()
            End If
        End If


    End Sub
    Private Sub CaricaPrima()
        '***Generata da Gianluigi Paesani in data:05/07/04
        '***Carico combo settore
        ddlMaccCodAmAtt.DataSource = MakeParentTable("select idmacroambitoattività, codifica + ' - ' + MacroAmbitoAttività as Macro from macroambitiattività")
        ddlMaccCodAmAtt.DataTextField = "ParentItem"
        ddlMaccCodAmAtt.DataValueField = "id"
        ddlMaccCodAmAtt.DataBind()

        '***Carico combo area intervento
        ddlCodAmAtt.Items.Add("")
        ddlCodAmAtt.Enabled = False


        ''*****Carico Combo Bandi

        'Mod. il 14/10/2016 da simona cordella con il filtrovisibilità
        Dim strsql As String

        strsql = "SELECT DISTINCT Bando.idBando,bando.bandobreve,bando.annobreve "
        strsql = strsql & " FROM bando"
        strsql = strsql & " INNER JOIN AssociaBandoTipiProgetto abtp on abtp.idbando =  bando.idbando"
        strsql = strsql & " INNER JOIN TipiProgetto  tp on abtp.idtipoprogetto = tp.idtipoprogetto"
        strsql = strsql & " WHERE tp.MacroTipoProgetto like '" & Session("FiltroVisibilita") & "'"
        strsql = strsql & " ORDER BY bando.annobreve desc"
        '"select idbando, bandobreve from Bando"

        DdlBando.DataSource = MakeParentTable(strsql)
        DdlBando.DataTextField = "ParentItem"
        DdlBando.DataValueField = "id"
        DdlBando.DataBind()


        ddlTipoProgetto.DataSource = ClsServer.CreaDataTable("Select idTipoProgetto,Descrizione from TipiProgetto ", True, Session("conn"))
        ddlTipoProgetto.DataTextField = "Descrizione"
        ddlTipoProgetto.DataValueField = "idTipoProgetto"
        ddlTipoProgetto.DataBind()



        ddlTipologia.DataSource = ClsServer.CreaDataTable("select idTipologieEnti='',Descrizione ='' union select idTipologieEnti,Descrizione from TipologieEnti", True, Session("conn"))
        ddlTipologia.DataValueField = "idTipologieEnti"
        ddlTipologia.DataTextField = "Descrizione"
        ddlTipologia.DataBind()

        ddlClasse.DataSource = ClsServer.CreaDataTable("select '0' as idclasseAccreditamento,'Seleziona' as classeAccreditamento  from classiaccreditamento " & _
            " union " & _
            " Select idclasseAccreditamento,classeAccreditamento " & _
            " from classiaccreditamento ", True, Session("conn"))
        ddlClasse.DataValueField = "idclasseAccreditamento"
        ddlClasse.DataTextField = "classeAccreditamento"
        ddlClasse.DataBind()



    End Sub
    Private Function MakeParentTable(ByVal strquery As String) As DataSet
        '***Generata da Gianluigi Paesani in data:05/07/04
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

        MakeParentTable = New DataSet
        MakeParentTable.Tables.Add(myDataTable)
    End Function

    Private Sub ddlMaccCodAmAtt_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlMaccCodAmAtt.SelectedIndexChanged
        If ddlMaccCodAmAtt.SelectedItem.Text <> "" Then
            ddlCodAmAtt.Enabled = True
            ddlCodAmAtt.DataSource = MakeParentTable("select distinct a.idambitoattività," & _
                                                     " a.codifica + ' ' + a.AmbitoAttività as Ambito from ambitiattività a" & _
                                                     " inner join macroambitiattività b" & _
                                                     " on a.IDMacroAmbitoAttività=b.IDMacroAmbitoAttività" & _
                                                     " where a.IDMacroAmbitoAttività=" & ddlMaccCodAmAtt.SelectedValue & " order by 1")
            ddlCodAmAtt.DataTextField = "ParentItem"
            ddlCodAmAtt.DataValueField = "id"
            ddlCodAmAtt.DataBind()
        Else
            'popolo completamente combo aree di intervento
            ddlCodAmAtt.DataSource = Nothing
            ddlCodAmAtt.Items.Add("")
            ddlCodAmAtt.SelectedIndex = 0
            ddlCodAmAtt.Enabled = False
        End If

    End Sub
    Sub CaricaCompetenze()
        'stringa per la query
        Dim strSQL As String
        'datareader che conterrà l'id 
        Dim dtrCompetenze As System.Data.SqlClient.SqlDataReader

        Try
            'controllo se si tratta del primo caricamento. così leggo i dati nel db una sola volta
            If Page.IsPostBack = False Then
                'preparo la query

                'strSQL = " Select IdRegioneCompetenza,case when Descrizione ='Nazionale' then UPPER(Descrizione) ELSE Descrizione end AS Descrizione,CodiceRegioneCompetenza "
                'strSQL = strSQL & " from RegioniCompetenze "
                'strSQL = strSQL & " ORDER BY CASE WHEN left(CodiceRegioneCompetenza,1)='N' then 1 else 2 end,descrizione "

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
                ddlCompetenza.DataSource = dtrCompetenze
                ddlCompetenza.Items.Add("")
                ddlCompetenza.DataTextField = "Descrizione"
                ddlCompetenza.DataValueField = "IDRegioneCompetenza"
                ddlCompetenza.DataBind()
                If Not dtrCompetenze Is Nothing Then
                    dtrCompetenze.Close()
                    dtrCompetenze = Nothing
                End If

                dtrCompetenze = ClsServer.CreaDatareader(strSQL, Session("conn"))
                ddlCompetenzaProgetto.DataSource = dtrCompetenze
                ddlCompetenzaProgetto.Items.Add("")
                ddlCompetenzaProgetto.DataTextField = "Descrizione"
                ddlCompetenzaProgetto.DataValueField = "IDRegioneCompetenza"
                ddlCompetenzaProgetto.DataBind()
                If Not dtrCompetenze Is Nothing Then
                    dtrCompetenze.Close()
                    dtrCompetenze = Nothing
                End If

                'chiudo il datareader se aperto
            End If
            'Controllo abilitazione scelta
            If Session("TipoUtente") = "U" Then
                'ddlCompetenza.Enabled = True
                If Session("Sistema") = "Helios" Then
                    If Me.Verifica.IdRegCompetenza = 22 Then
                        ddlCompetenzaProgetto.SelectedValue = -1
                    Else
                        ddlCompetenzaProgetto.SelectedValue = Me.Verifica.IdRegCompetenza
                    End If
                    ddlCompetenzaProgetto.Enabled = False
                End If
            Else
                'CboCompetenza.SelectedIndex = 1
                ddlCompetenzaProgetto.Enabled = False
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
                    ddlCompetenzaProgetto.SelectedValue = dtrCompetenze("IdRegioneCompetenza")
                    If dtrCompetenze("Heliosread") = True Then
                        ddlCompetenzaProgetto.Enabled = True
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


    Protected Sub cmdSalva_Click(sender As Object, e As EventArgs) Handles cmdSalva.Click
        dgRisultatoRicerca.CurrentPageIndex = 0
        EseguiRicerca()
    End Sub

    Private Sub EseguiRicerca()
        Dim strSql As String
        Dim strWhere As String

        'strSql = "select CodiceEnte,denominazione, idattivitàentesedeattuazione,codiceProgetto,Titolo, datafineattività,ambito,NumeroVolontari,comune,provincia,regione,IDEnteSedeAttuazione,EnteFiglio from ver_vw_ricercasedi "
        strSql = "select denominazione +  ' (' + CodiceEnte + ')' as EnteProponente, idattivitàentesedeattuazione, "
        strSql = strSql & "Titolo +  ' (' + CodiceProgetto + ')' as Progetto, datainizioattività, datafineattività,ambito,NumeroVolontari, "
        strSql = strSql & "Comune +  ' (' + DescrAbb + ')' as Comune, "
        strSql = strSql & "regione,IDEnteSedeAttuazione,EnteFiglio,EnteFiglio + ' (' + cast(IDEnteSedeAttuazione as nvarchar) + ')' as Unico, idente,identefiglio,IDAttività "
        'strSql = strSql & ", '<img src=images/ente-mini.png onclick=VisualizzaVol(' + convert(varchar, idattivitàentesedeattuazione) + ',' + convert(varchar, idente) + ',' + convert(varchar, IDAttività) + ',' + convert(varchar, idattivitàentesedeattuazione) + ') STYLE=cursor:hand title= border=0>'  as LinkVol"
        strSql = strSql & " from ver_vw_ricercasedi  "


        If TxtCodPog.Text.Trim <> "" Then
            strWhere = "codiceprogetto like '" & ClsServer.NoApice(TxtCodPog.Text) & "%'"
        End If
        If TxtDescPog.Text.Trim <> "" Then
            strWhere &= " and titolo like '" & ClsServer.NoApice(TxtDescPog.Text) & "%'"
        End If
        If DdlBando.SelectedValue > 0 Then
            strWhere &= " and idbando = " & DdlBando.SelectedValue
        End If
        If ddlMaccCodAmAtt.SelectedValue > 0 Then
            strWhere &= " and idmacroambitoattività = " & ddlMaccCodAmAtt.SelectedValue
        End If
        If ddlCodAmAtt.SelectedValue <> "" Then
            If ddlCodAmAtt.SelectedValue > 0 Then
                strWhere &= " and idambitoattività = " & ddlCodAmAtt.SelectedValue
            End If
        End If
        If TxtCodEnte.Text.Trim <> "" Then
            strWhere &= " and codiceente = '" & ClsServer.NoApice(TxtCodEnte.Text) & "'"
        End If
        If TxtNumVolontari.Text.Trim <> "" Then
            strWhere &= " and numerovolontari >= " & ClsServer.NoApice(TxtNumVolontari.Text) & ""
        Else
            '  strWhere &= " and numerovolontari >0"
        End If
        If ddlTipoProgetto.SelectedValue <> "0" Then
            strWhere &= " and idtipoprogetto = " & ddlTipoProgetto.SelectedValue & ""
        End If
        If chkLTipoPosto.Items(0).Selected Then
            strWhere &= " and V > 0"
        End If
        If chkLTipoPosto.Items(1).Selected Then
            strWhere &= " and VA > 0"
        End If
        If chkLTipoPosto.Items(2).Selected Then
            strWhere &= " and N > 0"
        End If
        If TxtDescrEnte.Text.Trim <> "" Then
            strWhere &= " and denominazione like '" & ClsServer.NoApice(TxtDescrEnte.Text) & "%'"
        End If
        If ddlClasse.SelectedValue <> "0" Then
            strWhere &= " and idclasseaccreditamento = " & ddlClasse.SelectedValue & ""
        End If
        If TxtComune.Text.Trim <> "" Then
            strWhere &= " and comune like '" & ClsServer.NoApice(TxtComune.Text) & "%'"
        End If
        If TxtProvincia.Text.Trim <> "" Then
            strWhere &= " and provincia like '" & ClsServer.NoApice(TxtProvincia.Text) & "%'"
        End If
        If TxtRegione.Text.Trim <> "" Then
            strWhere &= " and regione like '" & ClsServer.NoApice(TxtRegione.Text) & "%'"
        End If
        If TxtInizioPog.Text.Trim <> "" Then
            strWhere &= " and datainizioattività = '" & TxtInizioPog.Text & "'"
        End If
        If TxtFinePog.Text.Trim <> "" Then
            strWhere &= " and datafineattività = '" & TxtFinePog.Text & "'"
        End If
        If TxtAlladata.Text.Trim <> "" Then
            strWhere &= " and datainizioattività <= '" & TxtAlladata.Text & "'"
        End If
        If ddlCompetenza.SelectedValue <> "" Then
            Select Case ddlCompetenza.SelectedValue
                Case 0
                    strWhere = strWhere & ""
                Case -1
                    strWhere = strWhere & " and IdRegioneCompetenza = 22"
                Case -2
                    strWhere = strWhere & " and IdRegioneCompetenza <> 22 And not IdRegioneCompetenza is null "
                Case -3
                    strWhere = strWhere & " and IdRegioneCompetenza is null "
                Case Else
                    strWhere = strWhere & " and IdRegioneCompetenza = " & ddlCompetenza.SelectedValue
            End Select
        End If


        If ddlCompetenzaProgetto.SelectedValue <> "" Then
            Select Case ddlCompetenzaProgetto.SelectedValue
                Case 0
                    strWhere = strWhere & ""
                Case -1
                    strWhere = strWhere & " and IdRegCompetenzaProgetto = 22"
                Case -2
                    strWhere = strWhere & " and IdRegCompetenzaProgetto <> 22 And not IdRegCompetenzaProgetto is null "
                Case -3
                    strWhere = strWhere & " and IdRegCompetenzaProgetto is null "
                Case Else
                    strWhere = strWhere & " and IdRegCompetenzaProgetto = " & ddlCompetenzaProgetto.SelectedValue
            End Select
        End If

        strWhere = strWhere & " and MacroTipoProgetto like '" & Session("FiltroVisibilita") & "'"
        If strWhere <> "" Then
            strSql &= " where " & strWhere
        End If

        strSql = strSql.Replace("where  and", "where ")
        strSql &= " group by CodiceEnte,denominazione, idattivitàentesedeattuazione,codiceProgetto,Titolo,datainizioattività, datafineattività,ambito,NumeroVolontari,comune,descrabb,provincia,regione,IDEnteSedeAttuazione,EnteFiglio ,idente,identefiglio,IDAttività"

        Session("DataSetRicerca") = ClsServer.DataSetGenerico(strSql, Session("conn"))
        CaricaDataGrid(dgRisultatoRicerca)

    End Sub
    Private Sub CaricaDataGrid(ByRef GriddaCaricare As DataGrid)
        'assegno il dataset alla griglia del risultato
        GriddaCaricare.DataSource = Session("DataSetRicerca")
        GriddaCaricare.DataBind()
        'blocco per la creazione della datatable per la stampa della ricerca

        'nome e posizione di lettura delle colopnne a base 0
        Dim NomeColonne(9) As String
        Dim NomiCampiColonne(9) As String
        'nome della colonna 
        'e posizione nella griglia di lettura
        NomeColonne(0) = "Ente Proponente"
        NomeColonne(1) = "Progetto"
        NomeColonne(2) = "Settore/Ambito"
        NomeColonne(3) = "Data Inizio Progetto"
        NomeColonne(4) = "Data Fine Progetto"
        NomeColonne(5) = "Cod. Sede Attuazione"
        NomeColonne(6) = "Ente Figlio"
        NomeColonne(7) = "Num. Volontari"
        NomeColonne(8) = "Comune"
        NomeColonne(9) = "Regione"

        NomiCampiColonne(0) = "enteproponente"
        NomiCampiColonne(1) = "Progetto"
        NomiCampiColonne(2) = "ambito"
        NomiCampiColonne(3) = "datainizioattività"
        NomiCampiColonne(4) = "datafineattività"
        NomiCampiColonne(5) = "IDEnteSedeAttuazione"
        NomiCampiColonne(6) = "EnteFiglio"
        NomiCampiColonne(7) = "NumeroVolontari"
        NomiCampiColonne(8) = "comune"
        NomiCampiColonne(9) = "regione"

        'carico un datatable che userò poi nella pagina di stampa
        'il numero delle colonne è a base 0
        Session("DtbRicerca") = ClsServer.CaricaDataTablePerStampa(Session("DataSetRicerca"), 9, NomeColonne, NomiCampiColonne)

        '*********************************************************************************

        GriddaCaricare.Visible = True
        'If GriddaCaricare.Items.Count = 0 Then imgStampa.Visible = False Else imgStampa.Visible = True
        'If GriddaCaricare.Items.Count = 0 Then lblStampa.Visible = False Else lblStampa.Visible = True
    End Sub

    'Private Sub cmdSalvaScenario_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) handles 
    '    Dim strSql As String
    '    If DdlProgrammazione.SelectedValue <> "" And txtDescrProg.Text.Trim <> "" Then
    '        strSql = "insert into tverifichescenario (idprogrammazione,descrizione,idstatoscenario,datainserimento,userinseritore,note) " & _
    '                "values (" & DdlProgrammazione.SelectedValue & ",'" & txtDescrProg.Text.Replace("'", "''") & "',1,getdate(),'" & Session("Utente") & "','" & txtNoteProg.Text.Replace("'", "''") & "')"
    '        Dim InserCom As New SqlClient.SqlCommand(strSql, Session("conn"))
    '        InserCom.ExecuteNonQuery()
    '        Dim drScenario As SqlClient.SqlDataReader = ClsServer.CreaDatareader("select max(idscenario) from tverifichescenario where idprogrammazione = " & DdlProgrammazione.SelectedValue, Session("conn"))
    '        drScenario.Read()
    '        Dim IdScenario As Integer = drScenario(0)
    '        drScenario.Close()
    '        Dim sqlCommInsVer As New SqlClient.SqlCommand
    '        EseguiRicerca()
    '        Dim i As Integer = 0
    '        For i = 0 To Session("DataSetRicerca").Tables(0).Rows.Count - 1
    '            sqlCommInsVer.CommandType = CommandType.StoredProcedure
    '            sqlCommInsVer.CommandText = "SP_VER_INSERIMENTO_VERIFICHE"
    '            sqlCommInsVer.Connection = Session("conn")
    '            sqlCommInsVer.Parameters.Add("@IDSCENARIO", IdScenario)
    '            sqlCommInsVer.Parameters.Add("@IDATTIVITAENTESEDEATTUAZIONE", Session("DataSetRicerca").Tables(0).Rows(i).Item(1))
    '            sqlCommInsVer.Parameters.Add("@IDSTATOVERIFICA", 1)
    '            sqlCommInsVer.Parameters.Add("@UTENTE", Session("Utente"))
    '            'strSql = "insert into tverifiche (idscenario,idattivitàentesedeattuazione,idstatoverifica,datainserimento,userinseritore) " & _
    '            '        "values(" & IdScenario & "," & DataSetRicerca.Tables(0).Rows(i).Item(2) & ",1,getdate(),'" & Session("Utente") & "')"
    '            sqlCommInsVer.ExecuteNonQuery()
    '            sqlCommInsVer.Parameters.Clear()
    '        Next
    '        InserCom.Dispose()

    '        PulisciScenario()
    '        PulisciRicerca()
    '    Else

    '    End If
    'End Sub

    Private Sub dgRisultatoRicerca_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgRisultatoRicerca.PageIndexChanged
        dgRisultatoRicerca.CurrentPageIndex = e.NewPageIndex
        EseguiRicerca()
    End Sub

    Protected Sub cmdChiudi_Click(sender As Object, e As EventArgs) Handles cmdChiudi.Click
        If Request.QueryString("VengoDa") Is Nothing Then

            Me.Verifica = Me.ViewState("Verifica")
            If Request.QueryString("modifica") <> "" Then 'Request.QueryString("idsegnalazione"), Request.QueryString("idverifica")
                Server.Transfer("WfrmGestioneVerModSegnalazione.aspx?idsegnalazione=" + Request.QueryString("idsegnalazione") + "&idverifica=" + Request.QueryString("idverifica") + "")
            Else
                Response.Redirect("WfrmMain.aspx")
            End If
        Else
            'VengoDa=Accertamento&IdEnte=" & txtidente.Text & "&IDVerificheAssociate=" & e.Item.Cells(10).Text & "&IdVerifica=" & e.Item.Cells(9).Text)
            Redirect_su_Accertamento()
        End If
    End Sub
    Sub Redirect_su_Accertamento()
        Response.Redirect("verificarequisiti.aspx?IdEnte=" & Request.QueryString("IdEnte") & "&IDVerificheAssociate=" & Request.QueryString("IDVerificheAssociate") & "&IdVerifica=" & Request.QueryString("IdVerifica"))
    End Sub
    Private Sub dgRisultatoRicerca_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgRisultatoRicerca.ItemCommand

        Select Case e.CommandName
            Case "Associa"

                If Request.QueryString("VengoDa") Is Nothing Then 'vengo da segnalzione
                    Me.Verifica = Me.ViewState("Verifica")
                    If Request.QueryString("modifica") <> "" Then 'Request.QueryString("idsegnalazione"), Request.QueryString("idverifica")
                        Server.Transfer("WfrmGestioneVerModSegnalazione.aspx?IDAESA=" + e.Item.Cells(4).Text + "&idsegnalazione=" + Request.QueryString("idsegnalazione") + "&idverifica=" + Request.QueryString("idverifica") + "&IdRegCompetenza=" + ddlCompetenzaProgetto.SelectedValue)
                    Else
                        Server.Transfer("WfrmGestioneVerSegnalazione.aspx?IDAESA=" + e.Item.Cells(4).Text + "&IdRegCompetenza=" + ddlCompetenzaProgetto.SelectedValue)
                    End If
                Else 'vengo da accertamento
                    If EsistenzaSedeVerificaProgrammazione(e.Item.Cells(4).Text) = False Then
                        SalvataggioSostituzioneSede(Request.QueryString("IDVerificheAssociate"), Request.QueryString("IdAttivitaEnteSedeAttuazioneOld"), e.Item.Cells(4).Text, Request.QueryString("idverifica"), Session("Utente"))
                        Redirect_su_Accertamento()
                    End If
                End If
            Case "LinkVol"
                'Response.Write("<script>")
                'Response.Write("window.open(Ver_PersonaleEnte.aspx?idattivita=" + e.Item.Cells(16).Text + "&Idente=" + e.Item.Cells(17).Text + "&IDAESA=" + e.Item.Cells(4).Text + "&IDEnteSedeAttuazione=" + e.Item.Cells(18).Text, "Volontari", "width=900, height=500, toolbar=no, location=no, menubar=no, scrollbars=yes")
                'Response.Write("</script>")

                'Dim selectedItem As DataGridItem = e.Item
                'Dim IdVolontario As String = selectedItem.Cells(3).Text
                Dim resp As StringBuilder = New StringBuilder()
                Dim windowOption As String = "width = 900, height = 700, dependent=no,scrollbars=yes,status=no,resizable=yes"
                '"dependent=no,scrollbars=yes,status=no,resizable=yes,width=1000px ,height=400px"
                resp.Append("<script  type=""text/javascript"">" & vbCrLf)
                resp.Append("myWin = window.open('Ver_PersonaleEnte.aspx?idattivita=" + e.Item.Cells(16).Text + "&Idente=" + e.Item.Cells(17).Text + "&IDAESA=" + e.Item.Cells(4).Text + "&IDEnteSedeAttuazione=" + e.Item.Cells(18).Text + "', 'win'" + ",'" + windowOption + "')")
                resp.Append("</script>")
                Response.Write(resp.ToString())


        End Select
        'If e.CommandName = "Associa" Then
        'Session("IDAESA") = e.Item.Cells(4).Text
        'Session("VengoDA") = "GestioneScenario"
        'Response.Redirect("Ver_PersonaleEnte.aspx?VengoDa=GestioneScenario")
        'Response.Write("<script>")
        'Response.Write("window.open('Ver_PersonaleEnte.aspx.aspx', 'GestioneScenario', 'width=800, height=550, status=no, toolbar=no, location=no, menubar=no, scrollbars=no')")
        'Response.Write("</script>")
       
        'End If
    End Sub

    Private Sub PulisciRicerca()
        TxtCodPog.Text = ""
        TxtDescPog.Text = ""
        DdlBando.SelectedValue = 0
        ddlMaccCodAmAtt.SelectedValue = 0
        If ddlCodAmAtt.Enabled = True Then
            ddlCodAmAtt.SelectedValue = 0
        End If
        TxtCodEnte.Text = ""
        TxtNumVolontari.Text = ""
        ddlTipoProgetto.SelectedValue = 0
        chkLTipoPosto.Items(0).Selected = 0
        chkLTipoPosto.Items(1).Selected = 0
        chkLTipoPosto.Items(2).Selected = 0
        TxtDescrEnte.Text = ""
        ddlClasse.SelectedValue = "0"
        TxtComune.Text = ""
        TxtProvincia.Text = ""
        TxtRegione.Text = ""
        TxtInizioPog.Text = ""
        TxtFinePog.Text = ""
        TxtAlladata.Text = ""
        ddlCompetenzaProgetto.SelectedValue = 0
    End Sub

    Private Sub SalvataggioSostituzioneSede(ByVal IDVerificheAssociate As Integer, ByVal IdAESTOLD As Integer, ByVal IdAESTNEW As Integer, ByVal IdVerifica As Integer, ByVal strUserName As String)
        '** aggiunto da Simona Cordella il 25/11/2010
        Dim intValore As Integer

        Dim CustOrderHist As SqlClient.SqlCommand
        CustOrderHist = New SqlClient.SqlCommand
        CustOrderHist.CommandType = CommandType.StoredProcedure
        CustOrderHist.CommandText = "SP_VER_SOSTITUZIONE_SEDE"
        CustOrderHist.Connection = IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn"))

        Dim sparam As SqlClient.SqlParameter
        sparam = New SqlClient.SqlParameter
        sparam.ParameterName = "@IDAttivitàEnteSedeAttuazioneOLd"
        sparam.SqlDbType = SqlDbType.Int
        CustOrderHist.Parameters.Add(sparam)

        Dim sparam1 As SqlClient.SqlParameter
        sparam1 = New SqlClient.SqlParameter
        sparam1.ParameterName = "@IDAttivitàEnteSedeAttuazioneNew"
        sparam1.SqlDbType = SqlDbType.Int
        CustOrderHist.Parameters.Add(sparam1)

        Dim sparam2 As SqlClient.SqlParameter
        sparam2 = New SqlClient.SqlParameter
        sparam2.ParameterName = "@IDVerificheAssociate"
        sparam2.SqlDbType = SqlDbType.Int
        CustOrderHist.Parameters.Add(sparam2)

        Dim sparam3 As SqlClient.SqlParameter
        sparam3 = New SqlClient.SqlParameter
        sparam3.ParameterName = "@IDVerifica"
        sparam3.SqlDbType = SqlDbType.Int
        CustOrderHist.Parameters.Add(sparam3)


        Dim sparam4 As SqlClient.SqlParameter
        sparam4 = New SqlClient.SqlParameter
        sparam4.ParameterName = "@UserName"
        sparam4.SqlDbType = SqlDbType.VarChar
        CustOrderHist.Parameters.Add(sparam4)

        Dim sparam5 As SqlClient.SqlParameter
        sparam5 = New SqlClient.SqlParameter
        sparam5.ParameterName = "@Esito"
        sparam5.SqlDbType = SqlDbType.Int
        sparam5.Direction = ParameterDirection.Output
        CustOrderHist.Parameters.Add(sparam5)

        CustOrderHist.Parameters("@IDAttivitàEnteSedeAttuazioneOLd").Value = IdAESTOLD
        CustOrderHist.Parameters("@IDAttivitàEnteSedeAttuazioneNew").Value = IdAESTNEW
        CustOrderHist.Parameters("@IDVerificheAssociate").Value = IDVerificheAssociate
        CustOrderHist.Parameters("@IDVerifica").Value = IdVerifica
        CustOrderHist.Parameters("@UserName").Value = strUserName
        'Reader = CustOrderHist.ExecuteReader()
        CustOrderHist.ExecuteScalar()
    End Sub

    Private Sub cmdInsersci_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles cmdInsersci.Click
        Response.Redirect("Ver_GestioneProgrammazione.aspx?VengoDa=Scenario")
    End Sub

    Private Function EsistenzaSedeVerificaProgrammazione(ByVal IDAESA As String) As Boolean
        'creato da simona cordella il 29/05/2012
        'IDAESA --> IDATTIVITàENTESEDEATTUAZIONE
        Dim strSql As String
        Dim rstProg As SqlClient.SqlDataReader

        Dim blnReturn As Boolean = False 'false se nn ci sono programmazioni esistenti, true se essite su altre programamzioni dello stesso bando

        strSql = " SELECT  dbo.FN_EsistenzaSedeVerificaSegnalata(" & IDAESA & ") as EsistenzaSede, TVerificheProgrammazione.DESCRIZIONE " & _
                 " FROM TVerificheAssociate " & _
                 " INNER JOIN TVerifiche ON TVerificheAssociate.IDVerifica = TVerifiche.IDVerifica " & _
                 " INNER JOIN TVerificheProgrammazione ON TVerifiche.IDProgrammazione = TVerificheProgrammazione.IDProgrammazione " & _
                 " WHERE IDAttivitàEnteSedeAttuazione = " & IDAESA & " "
        rstProg = ClsServer.CreaDatareader(strSql, Session("Conn"))
        If rstProg.HasRows = True Then
            rstProg.Read()
            If rstProg("EsistenzaSede") = 1 Then
                Me.lblMsgSede.Text = "ATTENZIONE!!  La sede indicata è già stata inserita nella programmazione " & rstProg("DESCRIZIONE")
                Me.lblMsgSede.Visible = True
                'Me.Imgerrore.Visible = True
                blnReturn = True
            End If
        End If
        If Not rstProg Is Nothing Then
            rstProg.Close()
            rstProg = Nothing
        End If
        Return blnReturn
    End Function

    Private Function lblMsgSede() As Object
        Throw New NotImplementedException
    End Function

    Protected Sub dgRisultatoRicerca_SelectedIndexChanged(sender As Object, e As EventArgs) Handles dgRisultatoRicerca.SelectedIndexChanged

    End Sub
End Class