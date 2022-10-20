Imports System.Drawing

Public Class dettaglivolontario
    Inherits System.Web.UI.Page
    Dim PROGETTO_GARANZIA_GIOVANI As String = "4"
#Region " Codice generato da Progettazione Web Form "

    'Chiamata richiesta da Progettazione Web Form.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents ImgSuccesso As System.Web.UI.WebControls.Image
    Protected WithEvents lblmessaggiosopra As System.Web.UI.WebControls.Label
    Protected WithEvents lblSenzaVitoSenzaAlloggio As System.Web.UI.WebControls.Label
    Protected WithEvents lblTotOLP As System.Web.UI.WebControls.Label
    Protected WithEvents lblImportoPrevistoOLP As System.Web.UI.WebControls.Label
    Protected WithEvents lblTotCalcolato As System.Web.UI.WebControls.Label
    Protected WithEvents Form1 As System.Web.UI.HtmlControls.HtmlForm
    Protected WithEvents txtDataInizioPrevista As System.Web.UI.WebControls.TextBox

    Protected WithEvents ImgSostitu As System.Web.UI.WebControls.Button
    Protected WithEvents txtDataInizioServizio As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents CheckFormazione As HtmlInputHidden
    Protected WithEvents CheckRinuncia As HtmlInputHidden
    Protected WithEvents txtCodiceFiscale As System.Web.UI.WebControls.Label
    Protected WithEvents txtDataNascita As System.Web.UI.WebControls.Label
    Protected WithEvents txtComuneNascita As System.Web.UI.WebControls.Label
    Protected WithEvents txtComuneResidenza As System.Web.UI.WebControls.Label
    'NOTA: la seguente dichiarazione è richiesta da Progettazione Web Form.
    'Non spostarla o rimuoverla.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: questa chiamata al metodo è richiesta da Progettazione Web Form.
        'Non modificarla nell'editor del codice.
        InitializeComponent()
    End Sub

#End Region
    Public dtrgenerico As Data.SqlClient.SqlDataReader
    Dim StrDataOrdierna As String

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Inserire qui il codice utente necessario per inizializzare la pagina
        'codice generato da Jonathan Baganani il 22/11/2004
        'controllo se effettuato login
        If Not Session("LogIn") Is Nothing Then
            If Session("LogIn") = False Then 'verifico validità log-in
                Response.Redirect("LogOn.aspx")
            End If
        Else
            Response.Redirect("LogOn.aspx")
        End If

        If Len(CStr(Day(Session("dataserver")))) < 2 Then
            StrDataOrdierna = "0" & Day(Session("dataserver"))
        Else
            StrDataOrdierna = Day(Session("dataserver"))
        End If

        StrDataOrdierna = StrDataOrdierna & "/"
        If Len(CStr(Month(Session("dataserver")))) < 2 Then
            StrDataOrdierna = StrDataOrdierna & "0" & Month(Session("dataserver"))
        Else
            StrDataOrdierna = StrDataOrdierna & Month(Session("dataserver"))
        End If
        StrDataOrdierna = StrDataOrdierna & "/"
        StrDataOrdierna = StrDataOrdierna & Year(Session("dataserver"))

        'se si tratta della prima apertaura della pagina carico i dati relativi al volontario
        If Page.IsPostBack = False Then
            'carico la combo delle causali a seconda della tipologia di operazione che sto effettuando
            caricacausali(Request.QueryString("Op"))
            caricadativolontario(IIf(Request.QueryString("IdVolontario") = "", 0, CInt(Request.QueryString("IdVolontario"))), IIf(Request.QueryString("IdProgetto") = "", 0, CInt(Request.QueryString("IdProgetto"))))

            Dim messaggio As String = String.Empty


            Dim dataChiusura As Date
            dataChiusura = IIf(txtDataChiusura.Text.Trim = String.Empty, Nothing, txtDataChiusura.Text.Trim)

            ControllaRinunciaVolontario(Request.QueryString("IdVolontario"), Request.QueryString("Op").ToString.ToUpper, dataChiusura, messaggio)

            ' ''se si tratta di una rinuncia vado a controllare se è possibile far rinunciare il colontario
            ' ''portendo come valore di ritorno False, altrimenti, se non è un volontario che 
            ' ''può rinunciare ritorno True
            ' ''If Request.QueryString("Op") = "rinuncia" Then
            ' ''    If CheckRinuncia Is Nothing Then
            ' ''        CheckRinuncia = New HtmlInputHidden()
            ' ''    End If
            ' ''    If ControllaRinunciaVolontario() = False Then
            ' ''        'stampo a pagina una hidden contenente 'False' così che lato client posso fare il controllo
            ' ''        CheckRinuncia.Value = "False"
            ' ''        'nel caso questa funzione toglie il ceek ricontrollo le ore formazione
            ' ''    Else
            ' ''        CheckRinuncia.Value = "True"
            ' ''    End If
            ' ''End If


        End If

        caricalabeloreformazione(Request.QueryString("IdVolontario"))
    End Sub

    ' ''Function ControllaRinunciaVolontario() As Boolean
    ' ''    'imposto a false la funzione come valore di ritorno (si può effettuare la rinuncia)
    ' ''    ControllaRinunciaVolontario = False
    ' ''    'datareader locale che uso per legger i dati nella base dati
    ' ''    Dim dtrLocale As SqlClient.SqlDataReader
    ' ''    Dim strsql As String

    ' ''    'controllo e chiudo il datareader
    ' ''    If Not dtrLocale Is Nothing Then
    ' ''        dtrLocale.Close()
    ' ''        dtrLocale = Nothing
    ' ''    End If

    ' ''    strsql = "select "
    ' ''    strsql = strsql & "isnull((select distinct a.IdEntità from EntitàRimborsi as a where a.IdEntità=" & CInt(Request.QueryString("IdVolontario")) & "),0)  as ModRim, "
    ' ''    strsql = strsql & "isnull((select distinct a.IdEntità from attivitàentità as a inner join StatiAttivitàEntità as b on a.IdStatoAttivitàEntità=b.IdStatoAttivitàEntità where a.IdEntità=" & CInt(Request.QueryString("IdVolontario")) & " and b.Sospeso=1),0) as ModTran, "
    ' ''    strsql = strsql & "isnull((select distinct a.IdEntità from EntitàAssenze as a where a.IdEntità=" & CInt(Request.QueryString("IdVolontario")) & " and stato <>3),0) as ModAss"


    ' ''    dtrLocale = ClsServer.CreaDatareader(strsql, Session("conn"))

    ' ''    If dtrLocale.HasRows = True Then
    ' ''        dtrLocale.Read()
    ' ''        'controllo se i campi sono tutti = 0 
    ' ''        'in questo caso nn ci sono attività operative effettuate sul volontario
    ' ''        If Not (CInt(dtrLocale("ModRim")) = 0 And CInt(dtrLocale("ModTran")) = 0 And CInt(dtrLocale("ModAss")) = 0) Then
    ' ''            'setto a true il valore di ritorno così che stampo a pagina la hidden
    ' ''            'per verificare in fase di salvataggio se può o no rinunciare
    ' ''            ControllaRinunciaVolontario = True
    ' ''        End If
    ' ''    End If
    ' ''    'controllo e chiudo il datareader
    ' ''    If Not dtrLocale Is Nothing Then
    ' ''        dtrLocale.Close()
    ' ''        dtrLocale = Nothing
    ' ''    End If
    ' ''End Function

    Private Function ControllaRinunciaVolontario(ByVal IdVolontario As Integer, ByVal TipoOperazione As String, ByVal DataChiusura As Date?, ByRef messaggio As String) As Boolean

        Dim esito As Boolean
        Dim MySqlCommand As SqlClient.SqlCommand

        MySqlCommand = New SqlClient.SqlCommand
        MySqlCommand.CommandType = CommandType.StoredProcedure
        MySqlCommand.CommandText = "[SP_VERIFICA_CHIUSURA_VOLONTARIO]"
        MySqlCommand.Connection = Session("conn")

        Try

            MySqlCommand.Parameters.Add("@IdVolontario", SqlDbType.Int).Value = IdVolontario
            MySqlCommand.Parameters("@IdVolontario").Direction = ParameterDirection.Input

            MySqlCommand.Parameters.Add("@TipoOperazione", SqlDbType.VarChar).Value = TipoOperazione
            MySqlCommand.Parameters("@TipoOperazione").Direction = ParameterDirection.Input

            MySqlCommand.Parameters.Add("@DataChiusura", SqlDbType.DateTime).Value = DataChiusura
            MySqlCommand.Parameters("@DataChiusura").Direction = ParameterDirection.Input

            MySqlCommand.Parameters.Add("@Esito", SqlDbType.Bit)
            MySqlCommand.Parameters("@Esito").Direction = ParameterDirection.Output

            MySqlCommand.Parameters.Add("@MESSAGGIO", SqlDbType.VarChar)
            MySqlCommand.Parameters("@MESSAGGIO").Direction = ParameterDirection.Output
            MySqlCommand.Parameters("@MESSAGGIO").Size = 1000

            MySqlCommand.ExecuteNonQuery()

            esito = MySqlCommand.Parameters("@Esito").Value
            messaggio = MySqlCommand.Parameters("@MESSAGGIO").Value

        Catch ex As Exception
            'Response.Write(ex.Message.ToString())
        End Try

        Return esito

    End Function

    Sub caricadativolontario(ByVal intIdvolontario As Integer, ByVal intIdProgetto As Integer)
        'variabile stringa per generare le due query sui dati del volontario
        Dim strLocal As String
        'datareader locale che uso per legger i dati nella base dati
        Dim dtrLocale As SqlClient.SqlDataReader
        'dataset che uso per catricare i dati relativi alle sedi di attuazione e al progetto
        Dim dtsLocal As DataSet
        Dim strSql As String
        Dim dtrMod As SqlClient.SqlDataReader
        'carico la query relativa ai dati prettamente anagrafici del volontario
        strLocal = "select (isnull(a.Nome + ' ' + a.Cognome,'')) as Nominativo, "
        strLocal = strLocal & "(isnull(a.CodiceFiscale,'')) as CodiceFiscale, isnull(a.CodiceFiscale,'') AS CodFis, "
        strLocal = strLocal & "(isnull(case a.Sesso when 0 then 'Maschio' else 'Femmina' end,'')) as Sesso, "
        strLocal = strLocal & "dbo.formatoData(DataInizioServizio) as DataInizioServizio,dbo.formatoData(DatafineServizio) as DatafineServizio "
        strLocal = strLocal & "from entità as a "
        strLocal = strLocal & "where a.IdEntità=" & intIdvolontario & " "

        dtsLocal = ClsServer.DataSetGenerico(strLocal, Session("conn"))

        txtCodiceFiscale.Text = dtsLocal.Tables(0).Rows(0).Item("CodFis")
        txtNominativo.Text = dtsLocal.Tables(0).Rows(0).Item("Nominativo")
        txtSesso.Text = dtsLocal.Tables(0).Rows(0).Item("Sesso")
        txtDataIniServ.Text = dtsLocal.Tables(0).Rows(0).Item("DataInizioServizio")
        txtDataFineServ.Text = dtsLocal.Tables(0).Rows(0).Item("DatafineServizio")

        strLocal = "select ( isnull(case len(day(a.DataNascita)) when 1 then '0' + convert(varchar(20),day(a.DataNascita)) "
        strLocal = strLocal & "else convert(varchar(20),day(a.DataNascita))  end + '/' + "
        strLocal = strLocal & "(case len(month(a.DataNascita)) when 1 then '0' + convert(varchar(20),month(a.DataNascita)) "
        strLocal = strLocal & "else convert(varchar(20),month(a.DataNascita))  end + '/' + "
        strLocal = strLocal & "Convert(varchar(20), Year(a.DataNascita))),'XX/XX/XXXX')) as DataNascita, "
        strLocal = strLocal & "( isnull(b.Denominazione,'')) as ComunediNascita, "
        strLocal = strLocal & "( isnull(c.Denominazione,'')) as ComunediResidenza "
        strLocal = strLocal & "from entità as a "
        strLocal = strLocal & "inner join comuni as b on a.IdComunenascita=b.IdComune "
        strLocal = strLocal & "inner join comuni as c on a.IdComuneResidenza=c.IdComune "
        strLocal = strLocal & "where a.IdEntità=" & intIdvolontario & " "

        dtsLocal = ClsServer.DataSetGenerico(strLocal, Session("conn"))


        txtDataNascita.Text = dtsLocal.Tables(0).Rows(0).Item("DataNascita")
        txtComuneNascita.Text = dtsLocal.Tables(0).Rows(0).Item("ComunediNascita")
        txtComuneResidenza.Text = dtsLocal.Tables(0).Rows(0).Item("ComunediResidenza")

        strLocal = "select (isnull(c.Denominazione, '') ) as SedeAttuazione, "
        strLocal = strLocal & "(isnull(d.Titolo, '') ) as Progetto, "
        strLocal = strLocal & "(isnull(case len(day(a.DataInizioAttivitàEntità)) when 1 then '0' + convert(varchar(20),day(a.DataInizioAttivitàEntità)) "
        strLocal = strLocal & "else convert(varchar(20),day(a.DataInizioAttivitàEntità))  end + '/' + "
        strLocal = strLocal & "(case len(month(a.DataInizioAttivitàEntità)) when 1 then '0' + convert(varchar(20),month(a.DataInizioAttivitàEntità)) "
        strLocal = strLocal & "else convert(varchar(20),month(a.DataInizioAttivitàEntità))  end + '/' + "
        strLocal = strLocal & "Convert(varchar(20), Year(a.DataInizioAttivitàEntità))),'XX/XX/XXXX')) as DataInizio, "
        strLocal = strLocal & "( isnull(case len(day(a.DataFineAttivitàEntità)) when 1 then '0' + convert(varchar(20),day(a.DataFineAttivitàEntità)) "
        strLocal = strLocal & "else convert(varchar(20),day(a.DataFineAttivitàEntità))  end + '/' + "
        strLocal = strLocal & "(case len(month(a.DataFineAttivitàEntità)) when 1 then '0' + convert(varchar(20),month(a.DataFineAttivitàEntità)) "
        strLocal = strLocal & "else convert(varchar(20),month(a.DataFineAttivitàEntità))  end + '/' + "
        strLocal = strLocal & "Convert(varchar(20), Year(a.DataFineAttivitàEntità))),'XX/XX/XXXX')) as DataFine, "
        strLocal = strLocal & "isnull(case len(day(d.DataFineAttività)) when 1 then '0' + convert(varchar(20),day(d.DataFineAttività)) "
        strLocal = strLocal & "else convert(varchar(20),day(d.DataFineAttività))  end + '/' + "
        strLocal = strLocal & "(case len(month(d.DataFineAttività)) when 1 then '0' + convert(varchar(20),month(d.DataFineAttività)) "
        strLocal = strLocal & "else convert(varchar(20),month(d.DataFineAttività))  end + '/' + "
        strLocal = strLocal & "Convert(varchar(20), Year(d.DataFineAttività))),'XX/XX/XXXX') as DataFineAttività,a.idattivitàentità as vecchioidattivitaentita "
        strLocal &= ",(rtrim(ltrim(str(year(a.DataInizioAttivitàEntità)))) " & _
                    "+ case len(month(a.DataInizioAttivitàEntità)) when 1 then '0' + rtrim(ltrim(str(month(a.DataInizioAttivitàEntità)))) else rtrim(ltrim(str(month(a.DataInizioAttivitàEntità)))) end " & _
                    "+ case len(day(a.DataInizioAttivitàEntità)) when 1 then '0' + rtrim(ltrim(str(day(a.DataInizioAttivitàEntità)))) else rtrim(ltrim(str(day(a.DataInizioAttivitàEntità)))) end ) as myDataInizAttivInv "
        strLocal = strLocal & "from attivitàentità as a "
        strLocal = strLocal & "inner join attivitàentisediattuazione as b on a.IDAttivitàEnteSedeAttuazione=b.IDAttivitàEnteSedeAttuazione "
        strLocal = strLocal & "inner join entisediattuazioni as c on b.IdEnteSedeAttuazione=c.IdEnteSedeAttuazione "
        strLocal = strLocal & "inner join attività as d on b.IdAttività=d.IdAttività "
        strLocal = strLocal & "inner join TipiProgetto as e on d.IdTipoProgetto=e.IdTipoProgetto "
        strLocal = strLocal & "where a.IdEntità=" & intIdvolontario & " and b.IdAttività=" & intIdProgetto & " order by a.DataInizioAttivitàEntità"

        dtsLocal = ClsServer.DataSetGenerico(strLocal, Session("conn"))

        Dim strData As String = dtsLocal.Tables(0).Rows.Item(0).Item("DataFineAttività")
        Session("VecchioId") = dtsLocal.Tables(0).Rows.Item(0).Item("vecchioidattivitaentita")

        Dim strAnno As String
        Dim strMese As String
        Dim strGiorno As String
        Dim strDataInversa As String
        Dim strDataInversa2 As String

        strAnno = Mid(strData, 7, 4)
        strMese = Mid(strData, 4, 2)
        strGiorno = Mid(strData, 1, 2)
        strDataInversa = strAnno & strMese & strGiorno
        strDataInversa2 = dtsLocal.Tables(0).Rows.Item(0).Item("myDataInizAttivInv")

        dtgRisultatoRicerca.DataSource = dtsLocal
        dtgRisultatoRicerca.DataBind()

        If Request.QueryString("Op") = "rinuncia" Then
            strData = dtsLocal.Tables(0).Rows.Item(0).Item("DataInizio")
            strAnno = Mid(strData, Len(strData) - 3, 4)
            strMese = Mid(strData, Len(strData) - 6, 2)
            strGiorno = Mid(strData, Len(strData) - 9, 2)
            txtDataChiusura.Text = strGiorno & "/" & strMese & "/" & strAnno
            txtDataChiusura.ReadOnly = True
        End If


        If Request.QueryString("VengoDa") = "Modifica" Then
            strSql = "Select IdCausaleChiusura, isnull(NoteStato,'') NoteStato, isnull(DataChiusura,'') DataChiusura  From Entità Where IdEntità=" & intIdvolontario
            dtrMod = ClsServer.CreaDatareader(strSql, Session("conn"))

            If dtrMod.HasRows Then
                dtrMod.Read()
                ddlCausale.SelectedValue = dtrMod.Item("IdCausaleChiusura")
                txtNote.Text = dtrMod.Item("NoteStato")
                txtDataChiusura.Text = dtrMod.Item("DataChiusura")
            End If
            dtrMod.Close()
            dtrMod = Nothing
        End If

        strSql = "SELECT dbo.FormatoData(DataInizioServizio) as DataInizioServizio FROM entità WHERE IdEntità=" & intIdvolontario
        dtrMod = ClsServer.CreaDatareader(strSql, Session("conn"))

        If dtrMod.HasRows = True Then
            dtrMod.Read()
            txtDataInizioServizio.Value = dtrMod("DataInizioServizio")
        End If
        dtrMod.Close()
        dtrMod = Nothing

    End Sub

    Sub caricacausali(ByVal tipologia As String)
        '***Generata da Bagnani Jonathan in data:04/11/04

        Select Case tipologia
            Case "rinuncia"
                '***Generata da Bagnani Jonathan in data:04/11/04
                '***carico combo stati attivita

                ddlCausale.Items.Clear()

                ddlCausale.DataSource = MakeParentTable("SELECT IdCausale, Descrizione FROM Causali where Tipo=1 and abilitato=1 order by descrizione")
                ddlCausale.DataTextField = "ParentItem"
                ddlCausale.DataValueField = "id"
                ddlCausale.DataBind()

            Case "esclusione"
                '***Generata da Bagnani Jonathan in data:04/11/04
                '***carico combo stati attivita

                ddlCausale.Items.Clear()

                ddlCausale.DataSource = MakeParentTable("SELECT IdCausale, Descrizione FROM Causali where Tipo=3 and abilitato=1 order by descrizione")
                ddlCausale.DataTextField = "ParentItem"
                ddlCausale.DataValueField = "id"
                ddlCausale.DataBind()

        End Select
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

    Private Sub cmdConferma_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles cmdConferma.Click
        lblmessaggiosopra.Text = ""
        LblConfermaOperazione.Text = ""

        Dim messaggio As String = String.Empty
        Dim RinVolontario As Boolean

        Dim dataChiusura As Date
        dataChiusura = IIf(txtDataChiusura.Text.Trim = String.Empty, Nothing, txtDataChiusura.Text.Trim)

        RinVolontario = ControllaRinunciaVolontario(Request.QueryString("IdVolontario"), Request.QueryString("Op").ToString.ToUpper, dataChiusura, messaggio)

        If RinVolontario = False Then
            lblmessaggiosopra.Text = lblmessaggiosopra.Text + messaggio
            Exit Sub
        End If
        If CheckIntervalloDate() = False Then
            Exit Sub
        End If
        If (ValidaData() = True And ValidaCampi() = True) Then
            inseriscicronologia(CInt(Request.QueryString("IdVolontario")))
        End If

    End Sub

    Sub inseriscicronologia(ByVal idvolontario As Integer)
        '** Modificato il 06/08/2008 da simona cordella
        '** Se la data odierna è < della data Chiusura
        '** creo la cronologia del volontario, non modifico lo stato da 3(INServizio) a 5(Chiuso Durante il Servizio)
        '** scrivo cmq tutti gli altri dati,anche la causale di chiusura
        '** un job provvederà a chiudere il volontario al termine del servizio.
        If Request.QueryString("VengoDa") <> "Modifica" Then

            Dim strlocal As String
            Dim dtrlocal As SqlClient.SqlDataReader

            Try
                '' DataChiusura, DataUltimoStato,
                strlocal = "select IdStatoEntità, UserNameStato,"
                strlocal = strlocal & " case len(day(datachiusura)) when 1 then '0' + convert(varchar(20),day(datachiusura)) "
                strlocal = strlocal & " else convert(varchar(20),day(datachiusura))  end + '/' + "
                strlocal = strlocal & " (case len(month(datachiusura)) when 1 then '0' + convert(varchar(20),month(datachiusura)) "
                strlocal = strlocal & " else convert(varchar(20),month(datachiusura))  end + '/' + "
                strlocal = strlocal & " Convert(varchar(20), Year(datachiusura))) as datachiusura,"
                strlocal = strlocal & " case len(day(DataUltimoStato)) when 1 then '0' + convert(varchar(20),day(DataUltimoStato)) "
                strlocal = strlocal & " else convert(varchar(20),day(DataUltimoStato))  end + '/' + "
                strlocal = strlocal & "(case len(month(DataUltimoStato)) when 1 then '0' + convert(varchar(20),month(DataUltimoStato)) "
                strlocal = strlocal & " else convert(varchar(20),month(DataUltimoStato))  end + '/' + "
                strlocal = strlocal & " Convert(varchar(20), Year(DataUltimoStato))) as DataUltimoStato,"
                strlocal = strlocal & " NoteStato, IdCausaleChiusura "
                strlocal = strlocal & " from entità "
                strlocal = strlocal & " where identità=" & idvolontario

                'chiudo il dataereader
                If Not dtrlocal Is Nothing Then
                    dtrlocal.Close()
                    dtrlocal = Nothing
                End If

                dtrlocal = ClsServer.CreaDatareader(strlocal, Session("conn"))

                'controllo se ci sono record e vado a comporre la stringa per la insert nella tabella della cronologia
                If dtrlocal.HasRows = True Then
                    dtrlocal.Read()

                    'If StrDataOrdierna >= txtDataChiusura.Text Then
                    strlocal = "insert into CronologiaEntità "
                    strlocal = strlocal & "(IdEntità, IDStatoEntità, UserNameStato, DataChiusura, DataCronologia, NoteStato, IDCausaleChiusura) "
                    strlocal = strlocal & "values "
                    strlocal = strlocal & "(" & idvolontario & ", "
                    If IsDBNull(dtrlocal("IdStatoEntità")) = True Then
                        strlocal = strlocal & "null, "
                    Else
                        strlocal = strlocal & dtrlocal("IdStatoEntità") & ", "
                    End If
                    If IsDBNull(dtrlocal("UserNameStato")) = True Then
                        strlocal = strlocal & "null, "
                    Else
                        strlocal = strlocal & "'" & dtrlocal("UserNameStato") & "', "
                    End If
                    If IsDBNull(dtrlocal("DataChiusura")) = True Then
                        strlocal = strlocal & "null, "
                    Else
                        strlocal = strlocal & "'" & dtrlocal("DataChiusura") & "', "
                    End If
                    If IsDBNull(dtrlocal("DataUltimoStato")) = True Then
                        strlocal = strlocal & "null, "
                    Else
                        strlocal = strlocal & "'" & dtrlocal("DataUltimoStato") & "', "
                    End If
                    If IsDBNull(dtrlocal("NoteStato")) = True Then
                        strlocal = strlocal & "null, "
                    Else
                        strlocal = strlocal & "'" & Replace(dtrlocal("NoteStato"), "'", "''") & "', "
                    End If
                    If IsDBNull(dtrlocal("IDCausaleChiusura")) = True Then
                        strlocal = strlocal & "null)"
                    Else
                        strlocal = strlocal & dtrlocal("IDCausaleChiusura") & ")"
                    End If
                    'chiudo il dataereader
                    If Not dtrlocal Is Nothing Then
                        dtrlocal.Close()
                        dtrlocal = Nothing
                    End If

                    Dim cmdinsertcronologia As Data.SqlClient.SqlCommand
                    cmdinsertcronologia = New SqlClient.SqlCommand(strlocal, Session("conn"))
                    cmdinsertcronologia.ExecuteNonQuery()
                    cmdinsertcronologia.Dispose()
                    'End If
                    'chiudo il dataereader
                    If Not dtrlocal Is Nothing Then
                        dtrlocal.Close()
                        dtrlocal = Nothing
                    End If
                End If
            Catch ex As Exception
                Response.Write(ex.Message.ToString)
            End Try
        End If

        aggiornastatovolontario(idvolontario)

    End Sub

    Sub aggiornastatovolontario(ByVal idvolontario As Integer)
        Dim strlocal As String
        Dim cmdaggiornastatoentita As Data.SqlClient.SqlCommand
        Dim MyTransaction As System.Data.SqlClient.SqlTransaction

        '** Modificato il 06/08/2008 da simona cordella
        '** Se la data odierna è < della data Chiusura
        '** creo la cronologia del volontario, non modifico lo stato da 3(INServizio) a 5(Chiuso Durante il Servizio)
        '**scrivo cmq tutti gli altri dati,anche la causale di chiusura
        '** un job provvederà a chiudere il volontario al termine del servizio.
        If VerificaFormazione(idvolontario) = False Then
            Exit Sub

        Else

            Try
                Dim dataChiusura As Date = Date.Parse(txtDataChiusura.Text)
                strlocal = "UPDATE Entità SET "
                Select Case Request.QueryString("Op")
                    Case "rinuncia"
                        strlocal = strlocal & "IdStatoEntità=(select IdStatoEntità from StatiEntità where Rinuncia=1), "
                        'Agg.il 17/04/2008 da simona cordella campo POSTOOCCUPATO = 0
                        strlocal = strlocal & " POSTOOCCUPATO = 0, "
                    Case "esclusione"
                        'la txt data nascita contiene in realtà la data di chiusura
                        If CDate(StrDataOrdierna) >= CDate(txtDataChiusura.Text) Then
                            strlocal = strlocal & "IdStatoEntità=(select IdStatoEntità from StatiEntità where Sospeso=1), "
                        End If
                        'Agg.il 17/04/2008 da simona cordella campo POSTOOCCUPATO = 1
                        ' strlocal = strlocal & " POSTOOCCUPATO = 1, "
                End Select
                strlocal = strlocal & "UserNameStato='" & Session("Utente") & "', "
                strlocal = strlocal & "DataChiusura='" & Day(dataChiusura) & "-" & Month(dataChiusura) & "-" & Year(dataChiusura) & "', "
                strlocal = strlocal & "DataUltimoStato=GetDate(), "
                strlocal = strlocal & "NoteStato='" & Replace(txtNote.Text, "'", "''") & "', "
                strlocal = strlocal & "IdCausaleChiusura=" & ddlCausale.SelectedValue & " "
                strlocal = strlocal & "where IdEntità=" & idvolontario

                MyTransaction = Session("conn").BeginTransaction(Session("IdEnte") & "_" & Session("Utente"))
                cmdaggiornastatoentita = New SqlClient.SqlCommand(strlocal, Session("conn"))
                cmdaggiornastatoentita.Transaction = MyTransaction
                cmdaggiornastatoentita.ExecuteNonQuery()

                '---aggiorno la data fine servizio sulla tabella entità
                strlocal = "UPDATE Entità " & _
                           "SET " & _
                           "DataFineServizio = '" & Day(dataChiusura) & "-" & Month(dataChiusura) & "-" & Year(dataChiusura) & "' " & _
                           "WHERE IdEntità = " & idvolontario


                cmdaggiornastatoentita.Connection = Session("conn")
                cmdaggiornastatoentita.CommandText = strlocal
                cmdaggiornastatoentita.ExecuteNonQuery()

                '---aggiorno la data fine servizio sulla tabella attivitàentità
                strlocal = "UPDATE AttivitàEntità " & _
                           "SET " & _
                           "DataFineAttivitàEntità = '" & Day(dataChiusura) & "-" & Month(dataChiusura) & "-" & Year(dataChiusura) & "' " & _
                           "WHERE IdattivitàEntità = " & _
                           " (select top 1 idattivitàentità from attivitàentità where identità= " & idvolontario & " order by datafineattivitàentità desc)"

                cmdaggiornastatoentita.Connection = Session("conn")
                cmdaggiornastatoentita.CommandText = strlocal
                cmdaggiornastatoentita.ExecuteNonQuery()


                MyTransaction.Commit()
                '**********
                'agg il 28/08/2009 da danilo e simona
                ' gestione della formazione nel caso della chisura iniziale (rinuncia)
                ''''''''VerificaFormazione(idvolontario) ''''' ANTONELLOOOOOO
                '********

                LblConfermaOperazione.Visible = True
                LblConfermaOperazione.Text = "Operazione effettuata con successo."
                ImgSostitu.Visible = True
                disabilitacampi()

            Catch ex As Exception
                Response.Write(ex.Message.ToString)
                MyTransaction.Rollback(Session("IdEnte") & "_" & Session("Utente"))
            End Try
        End If
    End Sub

    Sub disabilitacampi()
        ddlCausale.Enabled = False
        txtDataChiusura.Enabled = False
        txtNote.Enabled = False
        cmdConferma.Visible = False
    End Sub

    Private Sub ImgSostitu_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ImgSostitu.Click
        Response.Redirect("WfrmGestioneSostituisciVolontari.aspx?CodiceFiscale=" & txtCodiceFiscale.Text & "&IdAttivita=" & Request.QueryString("IdProgetto") & "&IdEntita=" & Request.QueryString("IdVolontario") & "&VecchioIdAttivitaEntita=" & Session("VecchioId") & "&Op=" & Request.QueryString("Op"))
    End Sub

    Private Sub cmdChiudi_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdChiudi.Click
        'Modificato il 02/05/2006 da Simona Cordella
        'Aggiunta gestione delle maschere chisura in servizio e chiusura iniziale
        If Request.QueryString("provieneda") = "ChiusuraServizio" Then
            Response.Redirect("wfrmVolontari.aspx?provieneda=ChiusuraServizio&IdVol=" & CInt(Request.QueryString("IdVolontario")) & "&IdProgetto=" & CInt(Request.QueryString("IdProgetto")) & "&IdAttivita=" & CInt(Request.QueryString("IdAttivita")) & "&Op=" & Request.QueryString("Op") & "")
        ElseIf Request.QueryString("provieneda") = "ChiusuraIniziale" Then
            Response.Redirect("wfrmVolontari.aspx?provieneda=ChiusuraIniziale&IdVol=" & CInt(Request.QueryString("IdVolontario")) & "&IdProgetto=" & CInt(Request.QueryString("IdProgetto")) & "&IdAttivita=" & CInt(Request.QueryString("IdAttivita")) & "&Op=" & Request.QueryString("Op") & "")
        Else
            Response.Redirect("WfrmRicercaVolontariInServizio.aspx?Op=" & CStr(Request.QueryString("Op")) & "")
        End If
    End Sub
    Public Sub caricalabeloreformazione(ByVal IdVolontario As Integer)
        Dim strsql As String

        strsql = "Select isnull(OreFormazione,0) as OreFormazione from entità where identità=" & IdVolontario
        dtrgenerico = ClsServer.CreaDatareader(strsql, Session("conn"))
        dtrgenerico.Read()
        If CheckFormazione Is Nothing Then
            CheckFormazione = New HtmlInputHidden()

        End If
        If dtrgenerico("OreFormazione") <> 0 Then
            lblOreFormazione.Text = "Esistono ore di formazione per il volontario"
            lblOreFormazione.ForeColor = Color.Red
            imgalert.Visible = True
            txtoreformazione.Text = dtrgenerico("OreFormazione")
            txtoreformazione.ForeColor = Color.Red
            CheckFormazione.Value = "True"
            If Not dtrgenerico Is Nothing Then
                dtrgenerico.Close()
                dtrgenerico = Nothing
            End If

        Else
            lblOreFormazione.Text = "Non esistono ore di formazione per il volontario"
            lblOreFormazione.ForeColor = Color.Green
            imgalert.Visible = False
            txtoreformazione.Text = dtrgenerico("OreFormazione")
            txtoreformazione.ForeColor = Color.Green
            CheckFormazione.Value = "False"
        End If

        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If

        Dim strsql1 As String

        strsql1 = "select isnull(sum(giorni),0) as giorniassenze from EntitàAssenze where identità=" & IdVolontario & " and stato <> 3"
        dtrgenerico = ClsServer.CreaDatareader(strsql1, Session("conn"))
        dtrgenerico.Read()
        If dtrgenerico("giorniassenze") <> 0 Then
            lblAssenze.Text = "Esistono ore di assenze per il volontari"
            lblAssenze.ForeColor = Color.Red
            Imgalert1.Visible = True
            txtAssenze.ForeColor = Color.Red
            txtAssenze.Text = dtrgenerico("giorniassenze")
        Else
            lblAssenze.Text = "Non esistono ore di assenze per il volontario"
            Imgalert1.Visible = False
            txtAssenze.ForeColor = Color.Green
            txtAssenze.Text = dtrgenerico("giorniassenze")
            lblAssenze.ForeColor = Color.Green
        End If

        If Not dtrgenerico Is Nothing Then
            dtrgenerico.Close()
            dtrgenerico = Nothing
        End If

    End Sub
    Function VerificaFormazione(ByVal IdVolontario As Integer) As Boolean
        ''''''''creato il 28/08/2009
        ''''''''gestisce le ore di formazione in caso di chiusura iniziale
        '''''''Dim strsql As String
        '''''''Dim dtrForm As SqlClient.SqlDataReader
        '''''''Dim cmdFormazione As Data.SqlClient.SqlCommand
        '''''''Dim intStatoFormazione As Integer


        '''''''If Request.QueryString("Op") = "rinuncia" Then '''rinuncia equivale a chiusura iniziale
        '''''''    strsql = "Select isnull(OreFormazione,0) as OreFormazione from entità where identità=" & IdVolontario
        '''''''    dtrForm = ClsServer.CreaDatareader(strsql, Session("conn"))
        '''''''    'nuovo controllo bloccante chiusurainiziale
        '''''''    If dtrForm("OreFormazione") <> 0 Then
        '''''''        Response.Write("<script>")
        '''''''        Response.Write("alert('BLOCCO Chiusura Iniziale.')")
        '''''''        Response.Write("</script>")
        '''''''        Return False
        '''''''    Else
        '''''''        Return True
        '''''''    End If

        '''''''    'rimosso vecchio controllo che considerava le varie casistiche.
        '''''''    'If dtrForm.HasRows = True Then
        '''''''    '    dtrForm.Read()
        '''''''    '    If dtrForm("OreFormazione") <> 0 Then
        '''''''    '        If Not dtrForm Is Nothing Then
        '''''''    '            dtrForm.Close()
        '''''''    '            dtrForm = Nothing
        '''''''    '        End If
        '''''''    '        'recupero lo stato della formazione per il progetto
        '''''''    '        strsql = "Select isnull(statoformazione,0) as statoformazione from attivitàformazionegenerale where idattività =" & Request.QueryString("IdProgetto")
        '''''''    '        dtrForm = ClsServer.CreaDatareader(strsql, Session("conn"))
        '''''''    '        If dtrForm.HasRows = True Then
        '''''''    '            dtrForm.Read()
        '''''''    '            intStatoFormazione = dtrForm("statoformazione")
        '''''''    '            If Not dtrForm Is Nothing Then
        '''''''    '                dtrForm.Close()
        '''''''    '                dtrForm = Nothing
        '''''''    '            End If
        '''''''    '            Select Case intStatoFormazione
        '''''''    '                Case 0 'registrato
        '''''''    '                    AzzeraEntitàOreFormazione(IdVolontario)
        '''''''    '                Case 1 'confermato
        '''''''    '                    VerificaRimborsoVolontari(IdVolontario)
        '''''''    '                    AzzeraEntitàOreFormazione(IdVolontario)
        '''''''    '                Case Else 'approvato  e in pagamento
        '''''''    '                    VerificaApprovati_Pagamento(IdVolontario, intStatoFormazione)
        '''''''    '            End Select
        '''''''    '        End If
        '''''''    '    End If
        '''''''    'End If
        '''''''End If

        '''''''If Request.QueryString("Op") = "esclusione" Then ''esclusione equivale a chiusura in servizio
        '''''''    strsql = "Select isnull(OreFormazione,0) as OreFormazione from entità where identità=" & IdVolontario
        '''''''    dtrForm = ClsServer.CreaDatareader(strsql, Session("conn"))
        '''''''    'nuovo controllo bloccante chiusurainiziale
        '''''''    If dtrForm("OreFormazione") <> 0 Then
        '''''''        'messaggio()
        '''''''        Exit Function
        '''''''    End If
        '''''''    If Not dtrForm Is Nothing Then
        '''''''        dtrForm.Close()
        '''''''        dtrForm = Nothing
        '''''''    End If
        '''''''End If
        If Request.QueryString("Op") = "rinuncia" Then '''rinuncia equivale a chiusura iniziale
            If imgalert.Visible = True Then

                Return False
            Else
                Return True
            End If
        End If
        If Request.QueryString("Op") = "esclusione" Then ''esclusione equivale a chiusura in servizio

            Return True
        End If
    End Function


    Sub AzzeraEntitàOreFormazione(ByVal IdVolontario As Integer)
        '        Dim cmdFormazione As Data.SqlClient.SqlCommand
        Dim strsql As String
        strsql = "update entità set oreformazione = 0 where identità = " & IdVolontario

        ClsServer.EseguiSqlClient(strsql, Session("conn"))
    End Sub
    Private Sub VerificaRimborsoVolontari(ByVal IdVolontario As Integer)
        'creata il 28/08/2009 simona e danilo
        'verifica se il volontario è rimborsabile
        Dim strsql As String
        Dim dtrForm As SqlClient.SqlDataReader
        Dim blnRimb As Boolean
        'Dim cmdFormazione As Data.SqlClient.SqlCommand
        Dim intImportoFormazioneItalia As Integer = 0
        Dim intImportoFormazioneEstero As Integer = 0
        Dim nazioneBase As Integer
        If Not dtrForm Is Nothing Then
            dtrForm.Close()
            dtrForm = Nothing
        End If
        strsql = "SELECT  ENTITà.IDENTITà,bando.RimborsoFormazioneItalia,bando.RimborsoFormazioneEstero, convert(varchar,tipiprogetto.nazionebase) as nazionebase " & _
                " FROM  entità " & _
                " INNER JOIN attivitàentità ON entità.IDEntità = attivitàentità.IDEntità " & _
                " INNER JOIN  attivitàentisediattuazione ON attivitàentità.IDAttivitàEnteSedeAttuazione = attivitàentisediattuazione.IDAttivitàEnteSedeAttuazione " & _
                " INNER JOIN attività ON attivitàentisediattuazione.IDAttività = attività.IDAttività " & _
                " INNER JOIN ATTIVITàFORMAZIONEGENERALE ON ATTIVITà.IDATTIVITà = ATTIVITàFORMAZIONEGENERALE.IDATTIVITà " & _
                " INNER JOIN  BANDIATTIVITà ON ATTIVITà.IDBANDOATTIVITà = BANDIATTIVITà.IDBANDOATTIVITà " & _
                " INNER JOIN BANDO ON BANDIATTIVITà.IDBANDO = BANDO.IDBANDO " & _
                " INNER JOIN enti ON attività.IDEntePresentante = enti.IDEnte " & _
                " inner join tipiprogetto on attività.idtipoprogetto = tipiprogetto.idtipoprogetto " & _
                " WHERE ENTITà.IDENTITà = " & IdVolontario & " " & _
                " And entità.OreFormazione >= CASE " & _
                "   WHEN BANDO.REVISIONEFORMAZIONE = 0 THEN 30 ELSE ATTIVITàFORMAZIONEGENERALE.DURATAFORMAZIONEGENERALE " & _
                "	END and attivitàentità.EscludiFormazione=0 "
        dtrForm = ClsServer.CreaDatareader(strsql, Session("conn"))
        If dtrForm.HasRows = True Then
            dtrForm.Read()
            nazioneBase = dtrForm("nazionebase")
            intImportoFormazioneItalia = dtrForm("RimborsoFormazioneItalia")
            intImportoFormazioneEstero = dtrForm("RimborsoFormazioneEstero")
            If Not dtrForm Is Nothing Then
                dtrForm.Close()
                dtrForm = Nothing
            End If

            If nazioneBase = 1 Then   'ITALIA
                'update alertChiusuraIniziale
                strsql = " Update attivitàformazionegenerale set alertchiusureiniziali = 1,   " & _
                         " numerovolontaririmborsabiliitalia =  (numerovolontaririmborsabiliitalia - 1), " & _
                         " importoRimborsabileItalia = (importoRimborsabileItalia - " & intImportoFormazioneItalia & " )" & _
                         " where idattività =" & Request.QueryString("IdProgetto")
            Else 'ESTERO 
                'update alertChiusuraIniziale
                strsql = " Update attivitàformazionegenerale set alertchiusureiniziali = 1,   " & _
                         " numerovolontaririmborsabiliestero =  (numerovolontaririmborsabiliestero - 1), " & _
                         " importoRimborsabileEstero = (importoRimborsabileEstero - " & intImportoFormazioneEstero & " )" & _
                         " where idattività =" & Request.QueryString("IdProgetto")
            End If
            ClsServer.EseguiSqlClient(strsql, Session("conn"))
        End If
        If Not dtrForm Is Nothing Then
            dtrForm.Close()
            dtrForm = Nothing
        End If

    End Sub
    Private Sub VerificaApprovati_Pagamento(ByVal IdVolontario As Integer, ByVal intStatoFormazione As Integer)
        'creata il 28/08/2009 simona e danilo
        'gestisce il caso della formazione se è approvata o in pagamento

        Dim strsql As String
        Dim dtrForm As SqlClient.SqlDataReader
        If Not dtrForm Is Nothing Then
            dtrForm.Close()
            dtrForm = Nothing
        End If
        strsql = "SELECT  ENTITà.IDENTITà,bando.RimborsoFormazioneItalia,bando.RimborsoFormazioneEstero, convert(varchar,tipiprogetto.nazionebase) as nazionebase " & _
                " FROM  entità " & _
                " INNER JOIN attivitàentità ON entità.IDEntità = attivitàentità.IDEntità " & _
                " INNER JOIN  attivitàentisediattuazione ON attivitàentità.IDAttivitàEnteSedeAttuazione = attivitàentisediattuazione.IDAttivitàEnteSedeAttuazione " & _
                " INNER JOIN attività ON attivitàentisediattuazione.IDAttività = attività.IDAttività " & _
                " INNER JOIN ATTIVITàFORMAZIONEGENERALE ON ATTIVITà.IDATTIVITà = ATTIVITàFORMAZIONEGENERALE.IDATTIVITà " & _
                " INNER JOIN  BANDIATTIVITà ON ATTIVITà.IDBANDOATTIVITà = BANDIATTIVITà.IDBANDOATTIVITà " & _
                " INNER JOIN BANDO ON BANDIATTIVITà.IDBANDO = BANDO.IDBANDO " & _
                " INNER JOIN enti ON attività.IDEntePresentante = enti.IDEnte " & _
                " inner join tipiprogetto on attività.idtipoprogetto = tipiprogetto.idtipoprogetto " & _
                " WHERE ENTITà.IDENTITà = " & IdVolontario & " " & _
                " And entità.OreFormazione >= CASE " & _
                "   WHEN BANDO.REVISIONEFORMAZIONE = 0 THEN 30 ELSE ATTIVITàFORMAZIONEGENERALE.DURATAFORMAZIONEGENERALE " & _
                "	END and attivitàentità.EscludiFormazione=0 "
        dtrForm = ClsServer.CreaDatareader(strsql, Session("conn"))
        If dtrForm.HasRows = True Then
            dtrForm.Read()
            If Not dtrForm Is Nothing Then
                dtrForm.Close()
                dtrForm = Nothing
            End If
            'email
            EseguiStoreMailAnomalieFormazione(Session("IdEnte"), Request.QueryString("IdProgetto"), IdVolontario, intStatoFormazione)
        Else
            'azzero ore
            If Not dtrForm Is Nothing Then
                dtrForm.Close()
                dtrForm = Nothing
            End If
            AzzeraEntitàOreFormazione(IdVolontario)
        End If
        If Not dtrForm Is Nothing Then
            dtrForm.Close()
            dtrForm = Nothing
        End If
    End Sub
    Private Sub EseguiStoreMailAnomalieFormazione(ByVal IdEnte As Integer, ByVal IdAttività As Integer, ByVal IdEntità As Integer, ByVal StatoFormazione As Integer)

        'Agg. da Simona Cordella il 28/08/2009
        'eseguo store per l'invio della mail per anomalie sulla formazione
        Dim intValore As Integer

        Dim CustOrderHist As SqlClient.SqlCommand
        CustOrderHist = New SqlClient.SqlCommand
        CustOrderHist.CommandType = CommandType.StoredProcedure
        CustOrderHist.CommandText = "SP_MAIL_ANOMALIA_FORMAZIONE"
        CustOrderHist.Connection = IIf(ClsServer.Connessione(Session("TipoUtente"), Session("IdStatoEnte"), Session("ConnSnapshot"), Session("conn"), Session("FlagForzatura")) = False, Session("ConnSnapshot"), Session("Conn"))

        Dim sparam As SqlClient.SqlParameter
        sparam = New SqlClient.SqlParameter
        sparam.ParameterName = "@IdEnte"
        sparam.SqlDbType = SqlDbType.Int
        CustOrderHist.Parameters.Add(sparam)

        Dim sparam1 As SqlClient.SqlParameter
        sparam1 = New SqlClient.SqlParameter
        sparam1.ParameterName = "@IdAttività"
        sparam1.SqlDbType = SqlDbType.Int
        CustOrderHist.Parameters.Add(sparam1)

        Dim sparam2 As SqlClient.SqlParameter
        sparam2 = New SqlClient.SqlParameter
        sparam2.ParameterName = "@IdEntità"
        sparam2.SqlDbType = SqlDbType.Int
        CustOrderHist.Parameters.Add(sparam2)

        Dim sparam3 As SqlClient.SqlParameter
        sparam3 = New SqlClient.SqlParameter
        sparam3.ParameterName = "@StatoFormazione"
        sparam3.SqlDbType = SqlDbType.Int
        CustOrderHist.Parameters.Add(sparam3)


        Dim sparam4 As SqlClient.SqlParameter
        sparam4 = New SqlClient.SqlParameter
        sparam4.ParameterName = "@Esito"
        sparam4.SqlDbType = SqlDbType.Int
        sparam4.Direction = ParameterDirection.Output
        CustOrderHist.Parameters.Add(sparam4)



        '      Dim Reader As SqlClient.SqlDataReade       CustOrderHist.Parameters("@IdEnte").Value = IDEnter
        CustOrderHist.Parameters("@IdEnte").Value = IdEnte
        CustOrderHist.Parameters("@IdAttività").Value = IdAttività
        CustOrderHist.Parameters("@IdEntità").Value = IdEntità
        CustOrderHist.Parameters("@StatoFormazione").Value = StatoFormazione
        'Reader = CustOrderHist.ExecuteReader()
        CustOrderHist.ExecuteScalar()
        ' Insert code to read through the datareader.
        '        intValore = CustOrderHist.Parameters("@Esito").Value

        'If intValore = 0 Then ' l'ente nn ha completato le operazioni necessarie
        '    LeggiStoreVerificaVerificaCompletamentoAccreditamento = CustOrderHist.Parameters("@Motivazione").Value
        'Else
        '    LeggiStoreVerificaVerificaCompletamentoAccreditamento = ""
        'End If



    End Sub

    Function ValidaData() As Boolean
        Dim data As Date
        Dim dataValida As Boolean = True
        Dim dataFine As Date
        Dim dataInizio As Date
        Dim errore As StringBuilder = New StringBuilder()

        'chiamo la proprietà della classe GeneratoreModelli che ritorna la path del documento creato
        Dim utility As New ClsUtility()
        Dim IdTipologiaProgetto As String = utility.TipologiaProgettoDaIdAttivita(Request.QueryString("IdProgetto"), Session("conn"))

  

        If (txtDataChiusura.Text = String.Empty) Then
            errore.Append("La Data di Chiusura deve essere valorizzata. <br/>")
            dataValida = False
        Else

            If (IdTipologiaProgetto = PROGETTO_GARANZIA_GIOVANI) Then
                'volontari di garanzia giovani predeno dal data fine servizio 
                dataInizio = txtDataIniServ.Text
                dataFine = DateAdd(DateInterval.Year, 1, dataInizio)
                dataFine = DateAdd(DateInterval.Day, -1, dataFine)
                If Date.TryParse(txtDataChiusura.Text, data) = False Then
                    Dim err As String = "Il campo '{0}' contiene una data non valida. Immettere la data nel formato gg/mm/aaaa. <br/>"
                    lblmessaggiosopra.Text = String.Format(err, IdlblDataChiusura.Text.Replace(":", ""))
                    dataValida = False
                ElseIf (Date.Parse(txtDataChiusura.Text) > dataFine) Then
                    errore.AppendLine("La data di chiusura deve essere inferiore ad un anno di servizio. <br/>")
                    dataValida = False
                ElseIf (Date.Parse(txtDataChiusura.Text) < dataInizio) Then
                    errore.AppendLine("La data di chiusura deve essere maggiore della data di inizio progetto.<br/>")
                    dataValida = False
                End If
            Else
                'volontari di scn predeno dal data fine progetto
                dataInizio = dtgRisultatoRicerca.Items.Item(0).Cells.Item(2).Text
                'dataFine = dtgRisultatoRicerca.Items.Item(0).Cells.Item(4).Text
                dataFine = DataFineServizioVolontario(Request.QueryString("IdVolontario"))

                If Date.TryParse(txtDataChiusura.Text, data) = False Then
                    Dim err As String = "Il campo '{0}' contiene una data non valida. Immettere la data nel formato gg/mm/aaaa. <br/>"
                    lblmessaggiosopra.Text = String.Format(err, IdlblDataChiusura.Text.Replace(":", ""))
                    dataValida = False
                ElseIf (Date.Parse(txtDataChiusura.Text) > dataFine) Then
                    errore.AppendLine("La data di chiusura deve essere minore della data di fine progetto. <br/>")
                    dataValida = False
                ElseIf (Date.Parse(txtDataChiusura.Text) < dataInizio) Then
                    errore.AppendLine("La data di chiusura deve essere maggiore della data di inizio progetto.<br/>")
                    dataValida = False
                End If
            End If


        End If
        If (dataValida = False) Then
            lblmessaggiosopra.Text = lblmessaggiosopra.Text + errore.ToString()
            lblmessaggiosopra.Visible = True
        End If
        Return dataValida
    End Function
    Function ValidaCampi() As Boolean
        Dim campiValidi As Boolean = True
        Dim errore As StringBuilder = New StringBuilder()
        Dim dataFine As Date
        Dim dataInizio As Date
        dataInizio = dtgRisultatoRicerca.Items.Item(0).Cells.Item(2).Text
        dataFine = dtgRisultatoRicerca.Items.Item(0).Cells.Item(3).Text
        If (ddlCausale.SelectedValue = "0") Then
            errore.Append("Selezionare una Causale.<br/>")
            campiValidi = False
        End If
        If (campiValidi = False) Then
            lblmessaggiosopra.Text = lblmessaggiosopra.Text + errore.ToString()
            lblmessaggiosopra.Visible = True
        End If


        Return campiValidi
    End Function
    Private Function CheckIntervalloDate() As Boolean
        Dim errore As StringBuilder = New StringBuilder()
        Dim dataFine As Date
        Dim dataInizio As Date
        Dim DataLimite As Date

        CheckIntervalloDate = True
        dataInizio = dtgRisultatoRicerca.Items.Item(0).Cells.Item(2).Text
        DataLimite = DateAdd(DateInterval.Month, 6, dataInizio)

        If (Date.TryParse(txtDataChiusura.Text, dataFine) = True) Then

            If dataFine >= DataLimite And (ddlCausale.SelectedValue = "38" Or ddlCausale.SelectedValue = "39" Or ddlCausale.SelectedValue = "40" Or ddlCausale.SelectedValue = "42") Then
                errore.Append("Attenzione i mesi calcolati di servizio risultano 6 o più. Verificare la causale di chiusura.")
                CheckIntervalloDate = False
            End If
            If dataFine < DataLimite And (ddlCausale.SelectedValue = "35" Or ddlCausale.SelectedValue = "36" Or ddlCausale.SelectedValue = "37" Or ddlCausale.SelectedValue = "41") Then
                errore.Append("Attenzione i mesi calcolati di servizio risultano meno di 6. Verificare la causale di chiusura. ")
                CheckIntervalloDate = False
            End If
            'If DateDiff("m", dataInizio, DateAdd("d", 1, dataFine)) >= 6 And (ddlCausale.SelectedValue = "38" Or ddlCausale.SelectedValue = "39" Or ddlCausale.SelectedValue = "40" Or ddlCausale.SelectedValue = "42") Then
            '    errore.Append("Attenzione i mesi calcolati di servizio risultano 6 o più. Verificare la causale di chiusura.")
            '    CheckIntervalloDate = False
            'End If
            'If DateDiff("m", dataInizio, DateAdd("d", 1, dataFine)) < 6 And (ddlCausale.SelectedValue = "35" Or ddlCausale.SelectedValue = "36" Or ddlCausale.SelectedValue = "37" Or ddlCausale.SelectedValue = "41") Then
            '    errore.Append("Attenzione i mesi calcolati di servizio risultano meno di 6. Verificare la causale di chiusura. ")
            '    CheckIntervalloDate = False
            'End If
            lblmessaggiosopra.Text = lblmessaggiosopra.Text + errore.ToString()
        End If

    End Function

    Private Function DataFineServizioVolontario(ByVal IdVolontario As Integer) As Date
        Dim dtrLocale As SqlClient.SqlDataReader

        Dim strSql As String

        If Not dtrLocale Is Nothing Then
            dtrLocale.Close()
            dtrLocale = Nothing
        End If
        'carico la query relativa ai dati prettamente anagrafici del volontario
        strSql = "select  dbo.formatoData(DataInizioServizio) as DataInizioServizio,dbo.formatoData(DatafineServizio) as DatafineServizio "
        strSql = strSql & "from entità as a "
        strSql = strSql & "where a.IdEntità=" & IdVolontario & " "

        dtrLocale = ClsServer.CreaDatareader(strSql, Session("conn"))
        If dtrLocale.HasRows = True Then
            dtrLocale.Read()
            DataFineServizioVolontario = dtrLocale("DatafineServizio")
        End If
        If Not dtrLocale Is Nothing Then
            dtrLocale.Close()
            dtrLocale = Nothing
        End If
        Return DataFineServizioVolontario
    End Function
End Class
