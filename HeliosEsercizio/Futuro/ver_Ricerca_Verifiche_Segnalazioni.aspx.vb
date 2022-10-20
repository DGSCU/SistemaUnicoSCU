Imports System.Data.SqlClient
Imports System.IO
Public Class ver_Ricerca_Verifiche_Segnalazioni
    Inherits System.Web.UI.Page


#Region " Codice generato da Progettazione Web Form "

    'Chiamata richiesta da Progettazione Web Form.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    'Protected WithEvents Image1 As System.Web.UI.WebControls.Image
    'Protected WithEvents Label22 As System.Web.UI.WebControls.Label
    'Protected WithEvents Label25 As System.Web.UI.WebControls.Label
    'Protected WithEvents txtCodiceFascicolo As System.Web.UI.WebControls.TextBox
    'Protected WithEvents Label27 As System.Web.UI.WebControls.Label
    'Protected WithEvents ddlStatoVerifica As System.Web.UI.WebControls.DropDownList
    'Protected WithEvents Label23 As System.Web.UI.WebControls.Label
    'Protected WithEvents Label26 As System.Web.UI.WebControls.Label
    'Protected WithEvents ddlVerificatoreInterno As System.Web.UI.WebControls.DropDownList
    'Protected WithEvents Label29 As System.Web.UI.WebControls.Label
    Protected WithEvents ddlVerificatoreIGF As System.Web.UI.WebControls.DropDownList
    'Protected WithEvents Label28 As System.Web.UI.WebControls.Label
    'Protected WithEvents Label31 As System.Web.UI.WebControls.Label
    'Protected WithEvents txtDataDalPrevista As System.Web.UI.WebControls.TextBox
    'Protected WithEvents Label32 As System.Web.UI.WebControls.Label
    'Protected WithEvents txtDataAlPrevista As System.Web.UI.WebControls.TextBox
    'Protected WithEvents Label30 As System.Web.UI.WebControls.Label
    'Protected WithEvents Label33 As System.Web.UI.WebControls.Label
    'Protected WithEvents txtDataDalInizio As System.Web.UI.WebControls.TextBox
    'Protected WithEvents Label34 As System.Web.UI.WebControls.Label
    'Protected WithEvents txtDataAlInizio As System.Web.UI.WebControls.TextBox
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
    'Protected WithEvents Label8 As System.Web.UI.WebControls.Label
    'Protected WithEvents Label20 As System.Web.UI.WebControls.Label
    'Protected WithEvents TxtCodEnte As System.Web.UI.WebControls.TextBox
    'Protected WithEvents Label21 As System.Web.UI.WebControls.Label
    'Protected WithEvents TxtDescrEnte As System.Web.UI.WebControls.TextBox
    'Protected WithEvents Label9 As System.Web.UI.WebControls.Label
    'Protected WithEvents ddlClasse As System.Web.UI.WebControls.DropDownList
    'Protected WithEvents Label10 As System.Web.UI.WebControls.Label
    'Protected WithEvents ddlTipologia As System.Web.UI.WebControls.DropDownList
    'Protected WithEvents Label12 As System.Web.UI.WebControls.Label
    'Protected WithEvents Label13 As System.Web.UI.WebControls.Label
    'Protected WithEvents TxtComune As System.Web.UI.WebControls.TextBox
    'Protected WithEvents Label14 As System.Web.UI.WebControls.Label
    'Protected WithEvents TxtProvincia As System.Web.UI.WebControls.TextBox
    'Protected WithEvents Label15 As System.Web.UI.WebControls.Label
    'Protected WithEvents TxtRegione As System.Web.UI.WebControls.TextBox
    'Protected WithEvents txtbando1 As System.Web.UI.WebControls.TextBox
    'Protected WithEvents ddlCodAmAtt1 As System.Web.UI.WebControls.TextBox
    'Protected WithEvents txtDenominazioneEnte1 As System.Web.UI.WebControls.TextBox
    'Protected WithEvents ddlStatoAttivita1 As System.Web.UI.WebControls.TextBox
    'Protected WithEvents ddlMaccCodAmAtt1 As System.Web.UI.WebControls.TextBox
    'Protected WithEvents txtTitoloProgetto1 As System.Web.UI.WebControls.TextBox
    'Protected WithEvents cmdRicerca As System.Web.UI.WebControls.ImageButton
    'Protected WithEvents cmdChiudi As System.Web.UI.WebControls.ImageButton
    'Protected WithEvents lblmessaggio As System.Web.UI.WebControls.Label
    'Protected WithEvents imgStampa As System.Web.UI.WebControls.ImageButton
    'Protected WithEvents dgRisultatoRicerca As System.Web.UI.WebControls.DataGrid
    'Protected WithEvents Label5 As System.Web.UI.WebControls.Label
    'Protected WithEvents txtDalSegnalazione As System.Web.UI.WebControls.TextBox
    'Protected WithEvents Label6 As System.Web.UI.WebControls.Label
    'Protected WithEvents txtDaSegnalazione As System.Web.UI.WebControls.TextBox
    'Protected WithEvents Label7 As System.Web.UI.WebControls.Label
    'Protected WithEvents ddFonte As System.Web.UI.WebControls.DropDownList
    'Protected WithEvents Form1 As System.Web.UI.HtmlControls.HtmlForm
    'Protected WithEvents ddEsistoSeg As System.Web.UI.WebControls.DropDownList
    'Protected WithEvents chksegnalazione As System.Web.UI.WebControls.CheckBox
    'Protected WithEvents Label11 As System.Web.UI.WebControls.Label
    'Protected WithEvents ddlCompetenza As System.Web.UI.WebControls.DropDownList

    'NOTA: la seguente dichiarazione è richiesta da Progettazione Web Form.
    'Non spostarla o rimuoverla.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: questa chiamata al metodo è richiesta da Progettazione Web Form.
        'Non modificarla nell'editor del codice.
        InitializeComponent()
    End Sub

#End Region
    Public Sub TuttaPaginaSess()
        Session("TP") = True
    End Sub
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Inserire qui il codice utente necessario per inizializzare la pagina
        Call TuttaPaginaSess()
        If Not IsPostBack Then
            If Not Session("LogIn") Is Nothing Then
                If Session("LogIn") = False Then 'verifico validità log-in
                    Response.Redirect("LogOn.aspx")
                End If
            Else
                Response.Redirect("LogOn.aspx")
            End If
            If IsPostBack = False Then
                CaricaCombo()
                CaricaCompetenze()
                CaricaVerificatore()
            End If
        End If
    End Sub
    Sub CaricaCombo()

        ''*****Carico Combo Verificatori Interni
        'ddlVerificatoreInterno.DataSource = MakeParentTable("select IdVerificatore, (Cognome + ' ' + Nome) As Nome from TVerificatori where tipologia = 0 and abilitato=0")
        'ddlVerificatoreInterno.DataTextField = "ParentItem"
        'ddlVerificatoreInterno.DataValueField = "id"
        'ddlVerificatoreInterno.DataBind()

        ''*****Carico Combo Verificatori IGF
        'ddlVerificatoreIGF.DataSource = MakeParentTable("select IdVerificatore, (Cognome + ' ' + Nome) As Nome from TVerificatori where genericoigf = 0 and tipologia = 1 and abilitato=0")
        'ddlVerificatoreIGF.DataTextField = "ParentItem"
        'ddlVerificatoreIGF.DataValueField = "id"
        'ddlVerificatoreIGF.DataBind()

        ''controllo che se il tipo di utente è MASTER o ISPETTORE blocco o meno le due combo
        ''aggiunto da Jon Cruise il 14.06.2007
        'If TrovaProfiloUtente() = True Then
        '    ddlVerificatoreInterno.SelectedValue = TrovaIdVerificatore()
        '    ddlVerificatoreInterno.Enabled = False
        '    ddlVerificatoreIGF.Enabled = False
        'End If

        'combo fonte
        clsGui.CaricaDropDown(Me.ddFonte, clsVerificaSegnalazione.RecuperaFonte(Session("Conn")), "NOME", "IDFONTE")

        '*****Carico Combo Progtrammazione
        'ddlProgrammazione.DataSource = MakeParentTable("select IdProgrammazione, Descrizione from TVerificheProgrammazione")
        'ddlProgrammazione.DataTextField = "ParentItem"
        'ddlProgrammazione.DataValueField = "id"
        'ddlProgrammazione.DataBind()

        '*****Carico Combo Stati Verificatori per id
        ddlStatoVerifica.DataSource = MakeParentTable("select IdStatoVerifiche, StatoVerifiche from TVerificheStati WHERE IdStatoVerifiche in (1,4,5) order by IdStatoVerifiche")
        ddlStatoVerifica.DataTextField = "ParentItem"
        ddlStatoVerifica.DataValueField = "id"
        ddlStatoVerifica.DataBind()

        '*****Carico Combo Bandi
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

        ' DdlBando.DataSource = MakeParentTable("select idbando, bandobreve from Bando")
        DdlBando.DataTextField = "ParentItem"
        DdlBando.DataValueField = "id"
        DdlBando.DataBind()

        '***Carico combo settore
        ddlMaccCodAmAtt.DataSource = MakeParentTable("select idmacroambitoattività, codifica + ' - ' + MacroAmbitoAttività as Macro from macroambitiattività")
        ddlMaccCodAmAtt.DataTextField = "ParentItem"
        ddlMaccCodAmAtt.DataValueField = "id"
        ddlMaccCodAmAtt.DataBind()

        '***Carico combo area intervento
        ddlCodAmAtt.Items.Add("")
        ddlCodAmAtt.Enabled = False

        ddlClasse.DataSource = ClsServer.CreaDataTable("Select idclasseAccreditamento,classeAccreditamento from classiaccreditamento ", True, Session("conn"))
        ddlClasse.DataValueField = "idclasseAccreditamento"
        ddlClasse.DataTextField = "classeAccreditamento"
        ddlClasse.DataBind()

        ddlTipologia.DataSource = ClsServer.CreaDataTable("select idTipologieEnti,Descrizione from TipologieEnti", True, Session("conn"))
        ddlTipologia.DataValueField = "idTipologieEnti"
        ddlTipologia.DataTextField = "Descrizione"
        ddlTipologia.DataBind()

    End Sub
    'Sub CaricaCompetenze()
    '    'stringa per la query
    '    Dim strSQL As String
    '    'datareader che conterrà l'id 
    '    Dim dtrCompetenze As System.Data.SqlClient.SqlDataReader

    '    Try
    '        'controllo se si tratta del primo caricamento. così leggo i dati nel db una sola volta
    '        If Page.IsPostBack = False Then
    '            'preparo la query

    '            strSQL = "select IdRegioneCompetenza,Descrizione,CodiceRegioneCompetenza,left(CodiceRegioneCompetenza,1)from RegioniCompetenze where IdRegioneCompetenza <> 22 "
    '            strSQL = strSQL & " union "
    '            strSQL = strSQL & " select '0',' TUTTI ','','A' "
    '            strSQL = strSQL & " union "
    '            strSQL = strSQL & " select '-1',' NAZIONALE ','','B' "
    '            strSQL = strSQL & " union "
    '            strSQL = strSQL & " select '-2',' REGIONALE ','','C' "
    '            strSQL = strSQL & " union "
    '            strSQL = strSQL & " select '-3',' NON DEFINITO ','','D' "
    '            strSQL = strSQL & "  from RegioniCompetenze order by left(CodiceRegioneCompetenza,1),descrizione "
    '            'chiudo il datareader se aperto
    '            If Not dtrCompetenze Is Nothing Then
    '                dtrCompetenze.Close()
    '                dtrCompetenze = Nothing
    '            End If

    '            'eseguo la query
    '            dtrCompetenze = ClsServer.CreaDatareader(strSQL, Session("conn"))
    '            'assegno il datadearder alla combo caricando così descrizione e id
    '            ddlCompetenza.DataSource = dtrCompetenze
    '            ddlCompetenza.Items.Add("")
    '            ddlCompetenza.DataTextField = "Descrizione"
    '            ddlCompetenza.DataValueField = "IDRegioneCompetenza"
    '            ddlCompetenza.DataBind()
    '            If Not dtrCompetenze Is Nothing Then
    '                dtrCompetenze.Close()
    '                dtrCompetenze = Nothing
    '            End If

    '            dtrCompetenze = ClsServer.CreaDatareader(strSQL, Session("conn"))
    '            'ddlCompetenzaProgetto.DataSource = dtrCompetenze
    '            'ddlCompetenzaProgetto.Items.Add("")
    '            'ddlCompetenzaProgetto.DataTextField = "Descrizione"
    '            'ddlCompetenzaProgetto.DataValueField = "IDRegioneCompetenza"
    '            'ddlCompetenzaProgetto.DataBind()
    '            If Not dtrCompetenze Is Nothing Then
    '                dtrCompetenze.Close()
    '                dtrCompetenze = Nothing
    '            End If

    '            'chiudo il datareader se aperto
    '        End If
    '        'Controllo abilitazione scelta
    '        If Session("TipoUtente") = "U" Then
    '            ddlCompetenza.Enabled = True
    '            ddlCompetenza.SelectedIndex = 0

    '        Else
    '            'CboCompetenza.SelectedIndex = 1
    '            ddlCompetenza.Enabled = False
    '            'preparo la query
    '            strSQL = "select b.IdRegioneCompetenza ,b.Heliosread from RegioniCompetenze a "
    '            strSQL = strSQL & "INNER JOIN utentiunsc b ON a.idregionecompetenza = b.idregionecompetenza "
    '            strSQL = strSQL & "where b.username = '" & Session("Utente") & "'"
    '            'chiudo il datareader se aperto
    '            If Not dtrCompetenze Is Nothing Then
    '                dtrCompetenze.Close()
    '                dtrCompetenze = Nothing
    '            End If
    '            'controllo se utente o ente regionale
    '            'eseguo la query
    '            dtrCompetenze = ClsServer.CreaDatareader(strSQL, Session("conn"))
    '            dtrCompetenze.Read()
    '            If dtrCompetenze.HasRows = True Then
    '                ddlCompetenza.SelectedValue = dtrCompetenze("IdRegioneCompetenza")
    '                If dtrCompetenze("Heliosread") = True Then
    '                    ddlCompetenza.Enabled = True
    '                End If

    '            End If

    '        End If
    '    Catch ex As Exception
    '        Response.Write(ex.Message.ToString())
    '        If Not dtrCompetenze Is Nothing Then
    '            dtrCompetenze.Close()
    '            dtrCompetenze = Nothing
    '        End If
    '    End Try
    '    If Not dtrCompetenze Is Nothing Then
    '        dtrCompetenze.Close()
    '        dtrCompetenze = Nothing
    '    End If
    'End Sub
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
    'funzione che controlla il profilo dell'utente loggato
    'se l'utente loggato è MASTER può usare tutti i filtri di ricerca = TRUE
    'se l'utente loggato è ISPETTORE gli blocco le combo degli ispettori = FALSE
    Function TrovaProfiloUtente() As Integer

        Dim strLocal As String
        Dim dtrLocal As Data.SqlClient.SqlDataReader

        strLocal = "SELECT Descrizione FROM Profili "

        '============================================================================================================================
        '====================================================30/09/2008==============================================================
        '=======MODIFICA EFFETTUATA DA Kronk (99 scimmie saltavano sul letto, una cadde in terra e si ruppe il cervelletto...)=======
        '=================AGGIUNTA JOIN SU PROFILO READ PER ABILITARE LEGGERE LE ABILITAZIONI ANCHE DELL'UTENTE READ=================
        '============================================================================================================================
        If UCase(Me.TemplateSourceDirectory) <> "/HELIOSREAD" Then
            strLocal = strLocal & " INNER JOIN	AssociaUtenteGruppo ON Profili.IdProfilo = AssociaUtenteGruppo.IdProfilo "
        Else
            strLocal = strLocal & " INNER JOIN	AssociaUtenteGruppo ON Profili.IdProfilo = AssociaUtenteGruppo.IdProfiloRead "
        End If

        strLocal = strLocal & " WHERE AssociaUtenteGruppo.UserName='" & Session("Utente") & "'"

        dtrLocal = ClsServer.CreaDatareader(strLocal, Session("conn"))

        If dtrLocal.HasRows = True Then
            dtrLocal.Read()
            If (dtrLocal("Descrizione") = "VERIFICHE MASTER" Or dtrLocal("Descrizione") = "UNSC MASTER") Then
                'TrovaProfiloUtente = True
                TrovaProfiloUtente = 0
            Else
                'TrovaProfiloUtente = False
                If Not dtrLocal Is Nothing Then
                    dtrLocal.Close()
                    dtrLocal = Nothing
                End If
                strLocal = "SELECT idverificatore FROM TVerificatori INNER JOIN UtentiUnsc ON TVerificatori.IdUtente=UtentiUnsc.IdUtente WHERE UtentiUnsc.UserName='" & Session("Utente") & "'"
                dtrLocal = ClsServer.CreaDatareader(strLocal, Session("conn"))
                If dtrLocal.HasRows = True Then
                    dtrLocal.Read()
                    TrovaProfiloUtente = CInt(dtrLocal("idverificatore"))
                End If
                If Not dtrLocal Is Nothing Then
                    dtrLocal.Close()
                    dtrLocal = Nothing
                End If
            End If
        End If

        If Not dtrLocal Is Nothing Then
            dtrLocal.Close()
            dtrLocal = Nothing
        End If

        Return TrovaProfiloUtente

        'Dim strLocal As String
        'Dim dtrLocal As Data.SqlClient.SqlDataReader

        'strLocal = "SELECT Descrizione FROM Profili INNER JOIN AssociaUtenteGruppo ON AssociaUtenteGruppo.IdProfilo=Profili.IdProfilo WHERE AssociaUtenteGruppo.UserName='" & Session("Utente") & "'"

        'dtrLocal = ClsServer.CreaDatareader(strLocal, Session("conn"))

        'If dtrLocal.HasRows = True Then
        '    dtrLocal.Read()
        '    If dtrLocal("Descrizione") = "VERIFICHE MASTER" Then
        '        TrovaProfiloUtente = True
        '    Else
        '        TrovaProfiloUtente = False
        '    End If
        'End If

        'If Not dtrLocal Is Nothing Then
        '    dtrLocal.Close()
        '    dtrLocal = Nothing
        'End If

        'Return TrovaProfiloUtente

    End Function

    'funzione che mi trova l'idutente dell'utente loggato
    'aggiunta da Jonah Lomu
    'il 14.06.2007
    Function TrovaIdVerificatore() As Integer
        Dim strLocal As String
        Dim dtrLocal As Data.SqlClient.SqlDataReader

        strLocal = "SELECT IdVerificatore FROM TVerificatori INNER JOIN UtentiUNSC ON TVerificatori.IdUtente=UtentiUNSC.IdUtente WHERE UtentiUNSC.UserName='" & Session("Utente") & "'"

        dtrLocal = ClsServer.CreaDatareader(strLocal, Session("conn"))

        If dtrLocal.HasRows = True Then
            dtrLocal.Read()
            TrovaIdVerificatore = dtrLocal("IdVerificatore")
        End If

        If Not dtrLocal Is Nothing Then
            dtrLocal.Close()
            dtrLocal = Nothing
        End If

        Return TrovaIdVerificatore

    End Function


    Private Function VerificaParametri() As Boolean

        VerificaParametri = False
        Dim data As Date
        Dim data1 As Date
        If txtDataDalPrevista.Text <> "" Then
            If Not Date.TryParse(txtDataDalPrevista.Text, data) Then
                lblmessaggio.Text = "  Il formato della Data Prevista Verifica Dal deve essere GG/MM/AAAA. "
                Exit Function
            End If
        End If
        If txtDataAlPrevista.Text <> "" Then
            If Not Date.TryParse(txtDataAlPrevista.Text, data) Then
                lblmessaggio.Text = "  Il formato della Data Prevista Verifica Al deve essere GG/MM/AAAA. "
                Exit Function
            End If
        End If
        If txtDataDalPrevista.Text <> "" And txtDataAlPrevista.Text <> "" Then
            ' verifico dal < al
            data = Convert.ToDateTime(txtDataDalPrevista.Text)
            data1 = Convert.ToDateTime(txtDataAlPrevista.Text)
            If (data > data1) Then

                lblmessaggio.Text = " La data prevista verifica DAL deve essere minore di quella AL. "
                Exit Function
            End If
        End If



        If txtDataDalInizio.Text <> "" Then
            If Not Date.TryParse(txtDataDalInizio.Text, data) Then
                lblmessaggio.Text = "  Il formato della Data Inizio Verifica Dal deve essere GG/MM/AAAA. "
                Exit Function
            End If
        End If
        If txtDataAlInizio.Text <> "" Then
            If Not Date.TryParse(txtDataAlInizio.Text, data) Then
                lblmessaggio.Text = "  Il formato della Data Inizio Verifica Al deve essere GG/MM/AAAA. "
                Exit Function
            End If
        End If
        If txtDataDalInizio.Text <> "" And txtDataAlInizio.Text <> "" Then

            ' verifico dal < al
            data = Convert.ToDateTime(txtDataDalInizio.Text)
            data1 = Convert.ToDateTime(txtDataAlInizio.Text)
            If (data > data1) Then

                lblmessaggio.Text = " La data inizio prevista DAL deve essere minore di quella AL."
                Exit Function
            End If
        End If

        If txtDalSegnalazione.Text <> "" Then
            If Not Date.TryParse(txtDalSegnalazione.Text, data) Then
                lblmessaggio.Text = "  Il formato della Data Ricezione Segnalazione Dal deve essere GG/MM/AAAA. "
                Exit Function
            End If
        End If
        If txtDaSegnalazione.Text <> "" Then
            If Not Date.TryParse(txtDaSegnalazione.Text, data) Then
                lblmessaggio.Text = "  Il formato della Data Ricezione Segnalazione AL deve essere GG/MM/AAAA. "
                Exit Function
            End If
        End If


        If txtDalSegnalazione.Text <> "" And txtDaSegnalazione.Text <> "" Then

            ' verifico dal < al
            data = Convert.ToDateTime(txtDalSegnalazione.Text)
            data1 = Convert.ToDateTime(txtDaSegnalazione.Text)
            If (data > data1) Then

                lblmessaggio.Text = " La Data Ricezione Segnalazione Dal deve essere minore di quella AL."
                Exit Function
            End If
        End If



        VerificaParametri = True


    End Function


    Private Sub cmdRicerca_Click(sender As Object, e As EventArgs) Handles cmdRicerca.Click


        dgRisultatoRicerca.CurrentPageIndex = 0
        EseguiRicerca()
        'dgRisultatoRicerca.CurrentPageIndex = e.NewPageIndex
        'dgRisultatoRicerca.DataSource = Session("appDtsRisRicerca")
        'dgRisultatoRicerca.DataBind()
        'dgRisultatoRicerca.SelectedIndex = -1
    End Sub
    Private Sub EseguiRicerca()
        Dim strSql As String
        Dim strWhere As String

        lblmessaggio.Text = ""  'Reset Errore
        If (VerificaParametri() = False) Then Exit Sub


        strSql = "select isnull(V.IdVerifica,0) as Idverifica, V.Idsegnalazione,V.CodiceFascicolo, isnull(V.StatoVerifiche,'Non Associata') as StatoVerifiche, V.DataPrevistaVerifica, " & _
        " V.DataFinePrevistaVerifica, V.DataFineVerifica," & _
        " (V.Nominativo + '(' + case when V.tipoverificatore = 0 then 'Interno' when V.tipoverificatore = 1 then 'IGF' END + ')') as Nominativo," & _
        " isnull((V.Denominazione + '(' + V.CodiceEnte + ')'),'Non Associata') as Denominazione, isnull((V.Titolo + '(' + V.CodiceProgetto + ')'),'Non Associata') as Titolo," & _
        " V.DataInizioAttività, V.DataFineAttività, isnull((V.EnteFiglio + '(' + convert(varchar,V.IDEnteSedeAttuazione)  + ')'),'Non Associata') As EnteFiglio," & _
        " isnull((V.Comune + '(' + V.DescrAbb + ')'),'Non Associata') as Comune, V.Regione, 'Segnalata' as TipoVerifica, V.EsitoSegnalazione, V.Fonte, V.DataRicezioneSegnalazione" & _
        " from VER_VW_RICERCA_VERIFICHE_SEGNALAZIONE  as V where (V.Tipologia=2 or V.Tipologia is null) "

        'selezione registrata, associata e aperta
        If ddlStatoVerifica.SelectedValue = 0 Then
            strSql += " and (V.IDStatoVerifica in (1,4,5) or V.IDStatoVerifica is null) "
            strSql += " and (V.principale in (select a.principale from TVerificheVerificatori a " & _
                        " inner join tverifiche b on a.idverifica=b.idverifica where b.idverifica=V.IdVerifica and " & _
                        " idstatoverifica=V.idstatoverifica) or principale is null)"
        ElseIf ddlStatoVerifica.SelectedValue = 1 Then 'solo registrata
            strSql += " and V.IDStatoVerifica = '" & ddlStatoVerifica.SelectedValue & "'"
            strSql += " and V.principale is null "
        Else 'associata e aperta
            strSql += " and V.IDStatoVerifica = '" & ddlStatoVerifica.SelectedValue & "'"
            strSql += " and V.principale = 1 "
        End If

        If chksegnalazione.Checked Then
            strSql += " and (V.IDVerifica IS NULL) "
        End If

        If txtCodiceFascicolo.Text.Trim <> "" Then
            strSql += " and V.CodiceFascicolo like '" & ClsServer.NoApice(txtCodiceFascicolo.Text) & "%'"
        End If

        If ddlVerificatoreInterno.SelectedValue <> 0 Then
            strSql += " and V.idverificatore = '" & ddlVerificatoreInterno.SelectedValue & "'"
        End If
        If ddlVerificatoreIGF.SelectedValue <> 0 Then
            strSql += " and V.idverificatore = '" & ddlVerificatoreIGF.SelectedValue & "'"
        End If
        If txtDataDalPrevista.Text <> "" And txtDataAlPrevista.Text <> "" Then
            strSql += " and V.DataPrevistaVerifica between '" & txtDataDalPrevista.Text & "' and '" & txtDataAlPrevista.Text & "'"
        Else
            If txtDataDalPrevista.Text <> "" Then
                strSql += " and V.DataPrevistaVerifica >= '" & txtDataDalPrevista.Text & "'"
            End If
            If txtDataAlPrevista.Text <> "" Then
                strSql += " and V.DataPrevistaVerifica <= '" & txtDataAlPrevista.Text & "'"
            End If
        End If
        If txtDataDalInizio.Text <> "" And txtDataAlInizio.Text <> "" Then
            strSql += " and V.DataInizioVerifica between '" & txtDataDalInizio.Text & "' and '" & txtDataAlInizio.Text & "'"
        Else
            If txtDataDalInizio.Text <> "" Then
                strSql += " and V.DataInizioVerifica >= '" & txtDataDalInizio.Text & "'"
            End If
            If txtDataAlInizio.Text <> "" Then
                strSql += " and V.DataInizioVerifica <= '" & txtDataAlInizio.Text & "'"
            End If
        End If
        If TxtCodPog.Text.Trim <> "" Then
            strSql += " and V.codiceprogetto like '" & ClsServer.NoApice(TxtCodPog.Text) & "%'"
        End If
        If TxtDescPog.Text.Trim <> "" Then
            strSql &= " and V.titolo like '" & ClsServer.NoApice(TxtDescPog.Text) & "%'"
        End If
        If DdlBando.SelectedValue > 0 Then
            strSql &= " and V.idbando = " & DdlBando.SelectedValue
        End If
        If ddlMaccCodAmAtt.SelectedValue > 0 Then
            strSql &= " and V.idmacroambitoattività = " & ddlMaccCodAmAtt.SelectedValue
        End If
        If ddlCodAmAtt.SelectedValue <> "" Then
            strSql &= " and V.idambitoattività = " & ddlCodAmAtt.SelectedValue
        End If
        If TxtCodEnte.Text.Trim <> "" Then
            strSql &= " and V.codiceente = '" & ClsServer.NoApice(TxtCodEnte.Text) & "'"
        End If
        If TxtDescrEnte.Text.Trim <> "" Then
            strSql &= " and V.denominazione like '" & ClsServer.NoApice(TxtDescrEnte.Text) & "%'"
        End If
        If ddlClasse.SelectedValue <> "0" Then
            strSql &= " and V.idclasseaccreditamento = " & ddlClasse.SelectedValue & ""
        End If
        If TxtComune.Text.Trim <> "" Then
            strSql &= " and V.comune like '" & ClsServer.NoApice(TxtComune.Text) & "%'"
        End If
        If TxtProvincia.Text.Trim <> "" Then
            strSql &= " and V.provincia like '" & ClsServer.NoApice(TxtProvincia.Text) & "%'"
        End If
        If TxtRegione.Text.Trim <> "" Then
            strSql &= " and V.regione like '" & ClsServer.NoApice(TxtRegione.Text) & "%'"
        End If

        'condizioni aggiunte per segnalazione

        If Me.ddEsistoSeg.SelectedIndex > 0 Then
            strSql += " and V.EsitoSegnalazione=" + ddEsistoSeg.SelectedValue + ""
        End If

        If Me.ddFonte.SelectedIndex > 0 Then
            strSql += " and V.Fonte=" + ddFonte.SelectedValue + ""
        End If

        If Me.txtDalSegnalazione.Text <> "" And txtDaSegnalazione.Text <> "" Then
            strSql += " and V.DataRicezioneSegnalazione between '" & txtDalSegnalazione.Text & "' and '" & txtDaSegnalazione.Text & "'"
        Else
            If txtDalSegnalazione.Text <> "" Then
                strSql += " and V.DataRicezioneSegnalazione >= '" & txtDalSegnalazione.Text & "'"
            ElseIf txtDaSegnalazione.Text <> "" Then
                strSql += " and V.DataRicezioneSegnalazione <= '" & txtDaSegnalazione.Text & "'"
            End If
        End If
        If ddlCompetenza.SelectedValue <> "" Then
            Select Case ddlCompetenza.SelectedValue
                Case 0
                    strSql = strSql & ""
                Case -1
                    strSql = strSql & " and v.Competenza = 'Nazionale'"
                Case -2
                    strSql = strSql & " and v.Competenza <> 'Nazionale' And not IdRegioneCompetenza is null "
                Case -3
                    strSql = strSql & " and v.idRegioneCompetenza is null "
                Case Else
                    strSql = strSql & " and v.Competenza = '" & ddlCompetenza.SelectedItem.Text & "'"
            End Select
        End If
        strSql = strSql & " and (MacroTipoProgetto like '" & Session("FiltroVisibilita") & "' OR MacroTipoProgetto IS NULL)"
        Session("DataSetRicerca") = ClsServer.DataSetGenerico(strSql, Session("conn"))
        CaricaDataGrid(dgRisultatoRicerca)
    End Sub
    Private Sub CaricaDataGrid(ByRef GriddaCaricare As DataGrid)
        'assegno il dataset alla griglia del risultato
        dgRisultatoRicerca.DataSource = Session("DataSetRicerca")
        dgRisultatoRicerca.DataBind()
        'blocco per la creazione della datatable per la stampa della ricerca

        'nome e posizione di lettura delle colopnne a base 0
        Dim NomeColonne(12) As String
        Dim NomiCampiColonne(12) As String
        'nome della colonna 
        'e posizione nella griglia di lettura
        NomeColonne(0) = "Codice Fascicolo"
        NomeColonne(1) = "Stato Verifica"
        NomeColonne(2) = "Tipo Verifica"
        NomeColonne(3) = "Data Inizio Prevista Verifica"
        NomeColonne(4) = "Data Fine Prevista Verifica"
        NomeColonne(5) = "Data Chiusura Verifica"
        NomeColonne(6) = "Verificatore"
        NomeColonne(7) = "Ente Proponente"
        NomeColonne(8) = "Progetto"
        NomeColonne(9) = "Data Inizio Progetto"
        NomeColonne(10) = "Data Fine Progetto"
        NomeColonne(11) = "Sede Attuazione"
        NomeColonne(12) = "Comune"

        NomiCampiColonne(0) = "CodiceFascicolo"
        NomiCampiColonne(1) = "StatoVerifiche"
        NomiCampiColonne(2) = "TipoVerifica"
        NomiCampiColonne(3) = "DataPrevistaVerifica"
        NomiCampiColonne(4) = "DataFinePrevistaVerifica"
        NomiCampiColonne(5) = "DataFineVerifica"
        NomiCampiColonne(6) = "nominativo"
        NomiCampiColonne(7) = "Denominazione"
        NomiCampiColonne(8) = "Titolo"
        NomiCampiColonne(9) = "DataInizioAttività"
        NomiCampiColonne(10) = "DataFineAttività"
        NomiCampiColonne(11) = "entefiglio"
        NomiCampiColonne(12) = "comune"

        'carico un datatable che userò poi nella pagina di stampa
        'il numero delle colonne è a base 0
        Session("DtbRicerca") = ClsServer.CaricaDataTablePerStampa(Session("DataSetRicerca"), 12, NomeColonne, NomiCampiColonne)

        '*********************************************************************************

        GriddaCaricare.Visible = True
        If GriddaCaricare.Items.Count = 0 Then
            'GridDaCaricare.Visible = False
            ' lblmessaggio.Text = "Nessun Dato estratto."
            imgStampa.Visible = False
        Else
            'lblmessaggio.Text = "Helios - Elenco Enti."
            imgStampa.Visible = True
        End If
    End Sub

    Private Sub cmdChiudi_Click(sender As Object, e As EventArgs) Handles cmdChiudi.Click
        Response.Redirect("WfrmMain.aspx")
    End Sub
    Private Sub dgRisultatoRicerca_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgRisultatoRicerca.PageIndexChanged
        dgRisultatoRicerca.SelectedIndex = -1
        dgRisultatoRicerca.EditItemIndex = -1
        dgRisultatoRicerca.CurrentPageIndex = e.NewPageIndex
        CaricaDataGrid(dgRisultatoRicerca)
    End Sub
    Private Sub dgRisultatoRicerca_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgRisultatoRicerca.ItemCommand
        If e.CommandName = "seleziona" Then
            If e.Item.Cells(1).Text <> "0" Then
                Response.Redirect("WfrmGestioneVerModSegnalazione.aspx?idsegnalazione=" & e.Item.Cells(2).Text & "&idverifica=" & e.Item.Cells(1).Text & "", False)
            Else
                Response.Redirect("WfrmGestioneVerModSegnalazione.aspx?idsegnalazione=" & e.Item.Cells(2).Text & "", False)
            End If
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

                strSQL = "select IdRegioneCompetenza,Descrizione,CodiceRegioneCompetenza,left(CodiceRegioneCompetenza,1)from RegioniCompetenze where IdRegioneCompetenza <> 22 "
                strSQL = strSQL & " union "
                strSQL = strSQL & " select '0',' TUTTI ','','A' "
                strSQL = strSQL & " union "
                strSQL = strSQL & " select '-1',' NAZIONALE ','','B' "
                strSQL = strSQL & " union "
                strSQL = strSQL & " select '-2',' REGIONALE ','','C' "
                'strSQL = strSQL & " union "
                'strSQL = strSQL & " select '-3',' NON DEFINITO ','','D' "
                'strSQL = strSQL & "  from RegioniCompetenze "
                strSQL = strSQL & " order by left(CodiceRegioneCompetenza,1),descrizione "
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
                ddlCompetenza.DataSource = dtrCompetenze
                ddlCompetenza.Items.Add("")
                ddlCompetenza.DataTextField = "Descrizione"
                ddlCompetenza.DataValueField = "IDRegioneCompetenza"
                ddlCompetenza.DataBind()
                If Not dtrCompetenze Is Nothing Then
                    dtrCompetenze.Close()
                    dtrCompetenze = Nothing
                End If

                'chiudo il datareader se aperto
            End If
            'Controllo abilitazione scelta
            If Session("TipoUtente") = "U" Then
                ddlCompetenza.Enabled = True
                ddlCompetenza.SelectedIndex = 0

            Else
                'CboCompetenza.SelectedIndex = 1
                ddlCompetenza.Enabled = False
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
                    ddlCompetenza.SelectedValue = dtrCompetenze("IdRegioneCompetenza")
                    If dtrCompetenze("Heliosread") = True Then
                        ddlCompetenza.Enabled = True
                    End If

                End If

                If Session("TipoUtente") = "R" Then
                    ddlCompetenza.Enabled = False
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

    Private Sub CaricaVerificatore()
        Dim strsql As String
        '*****Carico Combo Verificatori Interni

        strsql = "select IdVerificatore, (Cognome + ' ' + Nome) As Nome " & _
                 " from TVerificatori WHERE Tipologia=0 AND Abilitato=0 "
        'If ddlCompetenza.SelectedValue <> 22 And ddlCompetenza.SelectedValue <> 0 Then
        '    strsql = strsql & " and IdRegCompetenza = " & ddlCompetenza.SelectedValue & " "
        'End If

        If ddlCompetenza.SelectedValue <> "" Then
            Select Case ddlCompetenza.SelectedValue
                Case 0
                    strsql = strsql & ""
                Case -1
                    strsql = strsql & " and IdRegCompetenza = 22"
                Case -2
                    strsql = strsql & " and IdRegCompetenza <> 22 And not IdRegCompetenza is null "
                Case -3
                    strsql = strsql & " and IdRegCompetenza is null "
                Case Else
                    strsql = strsql & " and IdRegCompetenza = " & ddlCompetenza.SelectedValue
            End Select
        End If


        ddlVerificatoreInterno.DataSource = MakeParentTable(strsql)
        ddlVerificatoreInterno.DataTextField = "ParentItem"
        ddlVerificatoreInterno.DataValueField = "id"
        ddlVerificatoreInterno.DataBind()

        '''controllo che se il tipo di utente è MASTER o ISPETTORE blocco o meno le due combo
        '''aggiunto da Jon Cruise il 14.06.2007
        'If TrovaProfiloUtente() <> 0 Then
        '    'aggiunto il 29/08/2007  da simona cordella
        '    'carico combo IGF a secondo del verificatoreinterno 
        '    '*****Carico Combo Verificatori IGF

        '    strsql = "SELECT V.IDVerificatore, V.Cognome + ' ' + V.Nome AS Nome " & _
        '            " FROM TVerificatori AS V " & _
        '            " INNER JOIN TVerificheAssociaUser AS VU ON V.IDVerificatore = VU.IDVerificatoreIGF " & _
        '            " WHERE  (V.Tipologia = 1) AND (V.Abilitato = 0) AND (V.GenericoIGF = 0) " & _
        '            " and vu.idverificatoreinterno = " & TrovaProfiloUtente() & "  and v.idRegCompetenza=22 "
        '    ddlVerificatoreIGF.DataSource = MakeParentTable(strsql)
        '    ddlVerificatoreIGF.DataTextField = "ParentItem"
        '    ddlVerificatoreIGF.DataValueField = "id"
        '    ddlVerificatoreIGF.DataBind()
        '    'ddlVerificatoreInterno.SelectedValue = TrovaIdVerificatore()
        '    ddlVerificatoreInterno.Enabled = False
        '    If ddlVerificatoreIGF.Items.Count = "1" Then 'riga vuota combo
        '        ddlVerificatoreIGF.Enabled = False
        '    End If

        'Else
        '*****Carico Combo Verificatori IGF
        ddlVerificatoreIGF.DataSource = MakeParentTable("select IdVerificatore, (Cognome + ' ' + Nome) As Nome from TVerificatori WHERE Tipologia=1 AND Abilitato=0 and genericoIGF =0 and idRegCompetenza=22")
        ddlVerificatoreIGF.DataTextField = "ParentItem"
        ddlVerificatoreIGF.DataValueField = "id"
        ddlVerificatoreIGF.DataBind()
        ' End If
        'If ddlCompetenza.SelectedValue <> 22 And ddlCompetenza.SelectedValue <> 0 Then
        '    ddlVerificatoreIGF.Enabled = False
        'Else
        '    ddlVerificatoreIGF.Enabled = True
        'End If
        Select Case ddlCompetenza.SelectedValue
            Case 0
                ddlVerificatoreIGF.Enabled = True
            Case -1
                ddlVerificatoreIGF.Enabled = True
                ' strsql = strsql & " and IdRegCompetenza = 22"
            Case -2
                ddlVerificatoreIGF.Enabled = False
                'strsql = strsql & " and IdRegCompetenza <> 22 And not IdRegCompetenza is null "
            Case -3
                ddlVerificatoreIGF.Enabled = False
                'strsql = strsql & " and IdRegCompetenza is null "
            Case Else
                If ddlCompetenza.SelectedValue <> 22 And ddlCompetenza.SelectedValue <> 0 Then
                    ddlVerificatoreIGF.Enabled = False
                Else
                    ddlVerificatoreIGF.Enabled = True
                End If
                'ddlVerificatoreIGF.Enabled = True
                'strsql = strsql & " and IdRegCompetenza = " & ddlCompetenza.SelectedValue
        End Select


    End Sub

    Private Sub ddlCompetenza_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlCompetenza.SelectedIndexChanged
        CaricaVerificatore()
    End Sub



    Protected Sub imgStampa_Click(sender As Object, e As EventArgs) Handles imgStampa.Click
        imgStampa.Visible = False
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
            imgStampa.Visible = False
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

    Protected Sub dgRisultatoRicerca_SelectedIndexChanged(sender As Object, e As EventArgs) Handles dgRisultatoRicerca.SelectedIndexChanged

    End Sub
End Class