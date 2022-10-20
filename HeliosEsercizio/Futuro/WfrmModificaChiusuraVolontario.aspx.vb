Imports System.Data.SqlClient

'*********************************************************************
'Pagina creata da Amilcare Paolella il 19/12/2005 ********************
'*********************************************************************
Public Class WfrmModificaChiusuraVolontario
    Inherits System.Web.UI.Page
    Public dtrgenerico As Data.SqlClient.SqlDataReader
    Private OPERAZIONE_OK = "Operazione effettuata con successo."

#Region " Codice generato da Progettazione Web Form "

    'Chiamata richiesta da Progettazione Web Form.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    Protected WithEvents dtgDett1 As System.Web.UI.WebControls.DataGrid
    Protected WithEvents dtgDett2 As System.Web.UI.WebControls.DataGrid
    Protected WithEvents imgChiudi As System.Web.UI.WebControls.Button
    Protected WithEvents Form1 As System.Web.UI.HtmlControls.HtmlForm

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
        'codice generato da Jonathan Baganani il 22/11/2004
        'controllo se effettuato login
        If Not Session("LogIn") Is Nothing Then
            If Session("LogIn") = False Then 'verifico validità log-in
                Response.Redirect("LogOn.aspx")
            End If
        Else
            Response.Redirect("LogOn.aspx")
        End If
        'se si tratta della prima apertaura della pagina carico i dati relativi al volontario
        If Page.IsPostBack = False Then
            'carico la combo delle causali a seconda della tipologia di operazione che sto effettuando
            caricacausali(Request.QueryString("Op"))
            caricadativolontario(IIf(Request.QueryString("IdVolontario") = "", 0, CInt(Request.QueryString("IdVolontario"))), IIf(Request.QueryString("IdProgetto") = "", 0, CInt(Request.QueryString("IdProgetto"))))
            'se si tratta di una rinuncia vado a controllare se è possibile far rinunciare il volontario
            'partendo come valore di ritorno False, altrimenti, se non è un volontario che 
            'può rinunciare ritorno True
            If Request.QueryString("Op") = "rinuncia" Then
                If ControllaRinunciaVolontario() = False Then
                    'stampo a pagina una hidden contenente 'False' così che lato client posso fare il controllo
                    CheckRinuncia.Value = "False"
                Else
                    'stampo a pagina una hidden contenente 'True' così che lato client posso fare il controllo
                    CheckRinuncia.Value = "True"
                End If
            End If
        End If
    End Sub

    Function ControllaRinunciaVolontario() As Boolean
        'imposto a false la funzione come valore di ritorno (si può effettuare la rinuncia)
        ControllaRinunciaVolontario = False
        'datareader locale che uso per legger i dati nella base dati
        Dim dtrLocale As SqlClient.SqlDataReader
        Dim strsql As String

        'controllo e chiudo il datareader
        If Not dtrLocale Is Nothing Then
            dtrLocale.Close()
            dtrLocale = Nothing
        End If

        strsql = "select "
        strsql = strsql & "isnull((select distinct a.IdEntità from EntitàRimborsi as a where a.IdEntità=" & CInt(Request.QueryString("IdVolontario")) & "),0)  as ModRim, "
        strsql = strsql & "isnull((select distinct a.IdEntità from attivitàentità as a inner join StatiAttivitàEntità as b on a.IdStatoAttivitàEntità=b.IdStatoAttivitàEntità where a.IdEntità=" & CInt(Request.QueryString("IdVolontario")) & " and b.Sospeso=1),0) as ModTran, "
        strsql = strsql & "isnull((select distinct a.IdEntità from EntitàAssenze as a where a.IdEntità=" & CInt(Request.QueryString("IdVolontario")) & " and stato <>3),0) as ModAss"


        dtrLocale = ClsServer.CreaDatareader(strsql, Session("conn"))

        If dtrLocale.HasRows = True Then
            dtrLocale.Read()
            'controllo se i campi sono tutti = 0 
            'in questo caso nn ci sono attività operative effettuate sul volontario
            If Not (CInt(dtrLocale("ModRim")) = 0 And CInt(dtrLocale("ModTran")) = 0 And CInt(dtrLocale("ModAss")) = 0) Then
                'setto a true il valore di ritorno così che stampo a pagina la hidden
                'per verificare in fase di salvataggio se può o no rinunciare
                ControllaRinunciaVolontario = True
            End If
        End If
        'controllo e chiudo il datareader
        If Not dtrLocale Is Nothing Then
            dtrLocale.Close()
            dtrLocale = Nothing
        End If
    End Function

    Sub caricadativolontario(ByVal intIdvolontario As Integer, ByVal intIdProgetto As Integer)
        'variabile stringa per generare le due query sui dati del volontario
        Dim strLocal As String
        'datareader locale che uso per legger i dati nella base dati
        Dim dtrLocale As SqlClient.SqlDataReader
        'dataset che uso per catricare i dati relativi alle sedi di attuazione e al progetto
        Dim dtsLocal As DataSet
        'carico la query relativa ai dati prettamente anagrafici del volontario
        strLocal = "select (isnull(a.Nome + ' ' + a.Cognome,'')) as Nominativo, "
        strLocal = strLocal & "(isnull(a.CodiceFiscale,'')) as CodiceFiscale, isnull(a.CodiceFiscale,'') AS CodFis, "
        strLocal = strLocal & "(isnull(case a.Sesso when 0 then 'Maschio' else 'Femmina' end,'')) as Sesso "
        strLocal = strLocal & "from entità as a "
        strLocal = strLocal & "where a.IdEntità=" & intIdvolontario & " "

        dtsLocal = ClsServer.DataSetGenerico(strLocal, Session("conn"))

        txtCodiceFiscale.Text = dtsLocal.Tables(0).Rows(0).Item("CodFis")
        txtNominativo.Text = dtsLocal.Tables(0).Rows(0).Item("Nominativo")
        txtSesso.Text = dtsLocal.Tables(0).Rows(0).Item("Sesso")



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
        strLocal = strLocal & "Convert(varchar(20), Year(a.DataFineAttivitàEntità))),'XX/XX/XXXX')) as DataFine "
        'strLocal = strLocal & "isnull(case len(day(d.DataFineAttività)) when 1 then '0' + convert(varchar(20),day(d.DataFineAttività)) "
        'strLocal = strLocal & "else convert(varchar(20),day(d.DataFineAttività))  end + '/' + "
        'strLocal = strLocal & "(case len(month(d.DataFineAttività)) when 1 then '0' + convert(varchar(20),month(d.DataFineAttività)) "
        'strLocal = strLocal & "else convert(varchar(20),month(d.DataFineAttività))  end + '/' + "
        'strLocal = strLocal & "Convert(varchar(20), Year(d.DataFineAttività))),'XX/XX/XXXX') as DataFineAttività"
        strLocal = strLocal & ",convert(varchar(10),d.DataFineAttività,103) as DataFineAttività"
        strLocal = strLocal & ",a.idattivitàentità as vecchioidattivitaentita "
        strLocal &= ",(rtrim(ltrim(str(year(a.DataInizioAttivitàEntità)))) " & _
                    "+ case len(month(a.DataInizioAttivitàEntità)) when 1 then '0' + rtrim(ltrim(str(month(a.DataInizioAttivitàEntità)))) else rtrim(ltrim(str(month(a.DataInizioAttivitàEntità)))) end " & _
                    "+ case len(day(a.DataInizioAttivitàEntità)) when 1 then '0' + rtrim(ltrim(str(day(a.DataInizioAttivitàEntità)))) else rtrim(ltrim(str(day(a.DataInizioAttivitàEntità)))) end ) as myDataInizAttivInv "

        strLocal = strLocal & ",convert(varchar(10),d.DataInizioAttività,103) as DataInizioAttività "
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

        strAnno = Mid(strData, 7, 4)
        strMese = Mid(strData, 4, 2)
        strGiorno = Mid(strData, 1, 2)
        strDataInversa = strAnno & strMese & strGiorno

        Response.Write("<input type=hidden name=DataFineAtt value=""" & strDataInversa & """>")
        Response.Write("<input type=hidden name=DataFine value=""" & strData & """>")

        dtgRisultatoRicerca.DataSource = dtsLocal
        dtgRisultatoRicerca.DataBind()

        If Request.QueryString("Op") = "esclusione" Then
            strData = dtsLocal.Tables(0).Rows.Item(0).Item("DataInizio")
            strAnno = Mid(strData, Len(strData) - 3, 4)
            strMese = Mid(strData, Len(strData) - 6, 2)
            strGiorno = Mid(strData, Len(strData) - 9, 2)
            txtDataChiusura.Text = strGiorno & "/" & strMese & "/" & strAnno
            txtDataChiusura.Enabled = False
        Else
            txtDataChiusura.Text = DateTime.Now.ToString.Substring(0, 10)
            txtDataChiusura.Enabled = True
        End If


    End Sub

    Sub caricacausali(ByVal tipologia As String)
        '***Generata da Bagnani Jonathan in data:04/11/04
        Select Case tipologia
            Case "esclusione"
                '***Generata da Bagnani Jonathan in data:04/11/04
                '***carico combo stati attivita

                ddlCausale.Items.Clear()

                ddlCausale.DataSource = MakeParentTable("SELECT IdCausale, Descrizione FROM Causali where Tipo=1")
                ddlCausale.DataTextField = "ParentItem"
                ddlCausale.DataValueField = "id"
                ddlCausale.DataBind()

            Case "rinuncia"
                '***Generata da Bagnani Jonathan in data:04/11/04
                '***carico combo stati attivita

                ddlCausale.Items.Clear()

                ddlCausale.DataSource = MakeParentTable("SELECT IdCausale, Descrizione FROM Causali where Tipo=3")
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
        If ValidaCampi() Then
            ModificaTipoChiusuraVolontario()
            'inseriscicronologia(Request.QueryString("IdVolontario"))
            If Request.QueryString("provieneda") = "Volontari" Then
                Response.Redirect("WfrmGestioneSostituisciVolontari.aspx?provieneda=" & Request.QueryString("provieneda") & "&IdAttivita=" & Request.QueryString("IdProgetto") & "&IdEntita=" & Request.QueryString("IdVolontario") & "&VecchioIdAttivitaEntita=" & Session("VecchioId") & "&Op=" & IIf(Request.QueryString("Op") = "esclusione", "rinuncia", "esclusione"))
            Else
                Response.Redirect("WfrmGestioneSostituisciVolontari.aspx?IdAttivita=" & Request.QueryString("IdProgetto") & "&IdEntita=" & Request.QueryString("IdVolontario") & "&VecchioIdAttivitaEntita=" & Session("VecchioId") & "&Op=" & IIf(Request.QueryString("Op") = "esclusione", "rinuncia", "esclusione"))
            End If
        End If
        'CheckIntervalloDate()

    End Sub

    Sub ModificaTipoChiusuraVolontario()
        Dim idEntita As String = Request.QueryString("IdVolontario")
        Dim operazione As String = Request.QueryString("Op")
        Dim messaggio As String = String.Empty
        Dim esitoOk As Boolean = True

        Dim strlocal As String
        Dim dtrlocal As SqlClient.SqlDataReader
        Dim command As SqlClient.SqlCommand = New SqlClient.SqlCommand
        command.CommandType = CommandType.StoredProcedure
        command.CommandText = "SP_VOLONTARIO_MODIFICA_TIPO_CHIUSURA"
        command.Connection = Session("conn")


        Try
            Dim parametroSql As SqlClient.SqlParameter
            parametroSql = New SqlParameter("@IdEntita", DbType.String)
            parametroSql.Value = idEntita
            command.Parameters.Add(parametroSql)
            parametroSql = New SqlParameter("@Operazione", DbType.String)
            parametroSql.Value = CType(operazione, String)
            command.Parameters.Add(parametroSql)
            parametroSql = New SqlParameter("@DataChiusura", DbType.Date)
            parametroSql.Value = txtDataChiusura.Text
            command.Parameters.Add(parametroSql)
            parametroSql = New SqlParameter("@IdCausaleChiusura", DbType.String)
            parametroSql.Value = ddlCausale.SelectedValue
            command.Parameters.Add(parametroSql)
            parametroSql = New SqlParameter("@NoteStato", DbType.String)
            parametroSql.Value = txtNote.Text
            command.Parameters.Add(parametroSql)
            parametroSql = New SqlParameter("@EsitoOk", DbType.Boolean, ParameterDirection.Output)
            parametroSql.Direction = ParameterDirection.Output
            command.Parameters.Add(parametroSql)
            parametroSql = New SqlParameter("@Messaggio", DbType.String, ParameterDirection.Output)
            parametroSql.Value = String.Empty
            parametroSql.Direction = ParameterDirection.Output
            command.Parameters.Add(parametroSql)

            command.ExecuteScalar()
            esitoOk = CType(command.Parameters("@EsitoOk").Value, Boolean)
            messaggio = command.Parameters("@Messaggio").Value.ToString()

            If Not esitoOk Then
                lblmessaggiosopra.Text = messaggio
            Else
                LblConfermaOperazione.Text = OPERAZIONE_OK
            End If


        Catch ex As Exception
            Response.Write(ex.Message.ToString)
        End Try

    End Sub

    Sub inseriscicronologia(ByVal idvolontario As Integer)
        Dim strlocal As String
        Dim dtrlocal As SqlClient.SqlDataReader

        Try
            strlocal = "select IdStatoEntità, UserNameStato, DataChiusura, DataUltimoStato, NoteStato, IdCausaleChiusura from entità "
            strlocal = strlocal & "where identità=" & idvolontario

            'chiudo il dataereader
            If Not dtrlocal Is Nothing Then
                dtrlocal.Close()
                dtrlocal = Nothing
            End If

            dtrlocal = ClsServer.CreaDatareader(strlocal, Session("conn"))

            'controllo se ci sono record e vado a comporre la stringa per la insert nella tabella della cronologia
            If dtrlocal.HasRows = True Then
                dtrlocal.Read()
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

        Catch ex As Exception
            Response.Write(ex.Message.ToString)
        End Try

        aggiornastatovolontario(idvolontario)
    End Sub

    Sub aggiornastatovolontario(ByVal idvolontario As Integer)
        Dim strlocal As String
        Dim cmdaggiornastatoentita As Data.SqlClient.SqlCommand
        Dim MyTransaction As System.Data.SqlClient.SqlTransaction

        Try
            Select Case Request.QueryString("Op")
                Case "rinuncia"
                    strlocal = "update entità set IdStatoEntità=(select IdStatoEntità from StatiEntità where Sospeso=1), "
                Case "esclusione"
                    strlocal = "update entità set IdStatoEntità=(select IdStatoEntità from StatiEntità where Rinuncia=1), "
            End Select
            strlocal = strlocal & "UserNameStato='" & Session("Utente") & "', "
            strlocal = strlocal & "DataChiusura='" & Day(txtDataChiusura.Text) & "-" & Month(txtDataChiusura.Text) & "-" & Year(txtDataChiusura.Text) & "', "
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
                       "DataFineServizio = '" & Day(txtDataChiusura.Text) & "-" & Month(txtDataChiusura.Text) & "-" & Year(txtDataChiusura.Text) & "' " & _
                       "WHERE IdEntità = " & idvolontario


            cmdaggiornastatoentita.Connection = Session("conn")
            cmdaggiornastatoentita.CommandText = strlocal
            cmdaggiornastatoentita.ExecuteNonQuery()

            '---aggiorno la data fine servizio sulla tabella attivitàentità
            strlocal = "UPDATE AttivitàEntità " & _
                       "SET " & _
                       "DataFineAttivitàEntità = '" & Day(txtDataChiusura.Text) & "-" & Month(txtDataChiusura.Text) & "-" & Year(txtDataChiusura.Text) & "' " & _
                       "WHERE IdattivitàEntità = " & _
                       " (select top 1 idattivitàentità from attivitàentità where identità= " & idvolontario & " order by datafineattivitàentità desc)"

            cmdaggiornastatoentita.Connection = Session("conn")
            cmdaggiornastatoentita.CommandText = strlocal
            cmdaggiornastatoentita.ExecuteNonQuery()


            MyTransaction.Commit()


            lblmessaggiosopra.Visible = True
            lblmessaggiosopra.Text = "Operazione effettuata con successo."
            disabilitacampi()

        Catch ex As Exception
            Response.Write(ex.Message.ToString)
            MyTransaction.Rollback(Session("IdEnte") & "_" & Session("Utente"))
        End Try
    End Sub

    Sub disabilitacampi()
        ddlCausale.Enabled = False
        txtDataChiusura.Enabled = False
        txtNote.Enabled = False
        cmdConferma.Visible = False
    End Sub

    Private Sub imgChiudi_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles imgChiudi.Click
        If Request.QueryString("provieneda") = "Volontari" Then
            Response.Redirect("WfrmGestioneSostituisciVolontari.aspx?provieneda=" & Request.QueryString("provieneda") & "&IdAttivita=" & Request.QueryString("IdProgetto") & "&IdEntita=" & Request.QueryString("IdVolontario") & "&VecchioIdAttivitaEntita=" & Session("VecchioId") & "&Op=" & Request.QueryString("Op"))
        Else
            Response.Redirect("WfrmGestioneSostituisciVolontari.aspx?IdAttivita=" & Request.QueryString("IdProgetto") & "&IdEntita=" & Request.QueryString("IdVolontario") & "&VecchioIdAttivitaEntita=" & Session("VecchioId") & "&Op=" & Request.QueryString("Op"))
        End If

    End Sub

    Function ValidaData() As Boolean
        Dim data As Date
        Dim dataValida As Boolean = True
        Dim dataFine As Date
        Dim dataInizio As Date
        Dim errore As StringBuilder = New StringBuilder()
        If (txtDataChiusura.Text = String.Empty) Then
            errore.Append("La Data di Chiusura deve essere valorizzata. <br/>")
            dataValida = False
        Else
            dataInizio = dtgRisultatoRicerca.Items.Item(0).Cells.Item(2).Text
            dataFine = dtgRisultatoRicerca.Items.Item(0).Cells.Item(3).Text
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
        If (dataValida = False) Then
            lblmessaggiosopra.Text = lblmessaggiosopra.Text + errore.ToString()
            lblmessaggiosopra.Visible = True
        End If
        Return dataValida
    End Function
    Function ValidaCampi() As Boolean
        Dim campiValidi As Boolean = True
        Dim errore As StringBuilder = New StringBuilder()
        Dim dataFineProgetto As Date
        Dim dataInizioProgetto As Date
        Dim dataInizioAttivitaVolontario As Date
        Dim dataFineAttivitaVolontario As Date
        Dim dataChiusura As Date

        dataInizioProgetto = dtgRisultatoRicerca.Items.Item(0).Cells.Item(2).Text
        dataFineProgetto = dtgRisultatoRicerca.Items.Item(0).Cells.Item(3).Text
        dataInizioAttivitaVolontario = dtgRisultatoRicerca.Items.Item(0).Cells.Item(4).Text
        dataFineAttivitaVolontario = dtgRisultatoRicerca.Items.Item(0).Cells.Item(5).Text

        If (txtDataChiusura.Text = String.Empty) Then
            errore.Append("Il campo 'Data Chiusura' deve essere valorizzato.<br/>")
        Else
            If Not Date.TryParse(txtDataChiusura.Text, dataChiusura) Then
                errore.Append("Inserire la 'Data Chiusura' in un formato valido (gg/mm/aaaa).<br/>")
                campiValidi = False
            Else
                If dataChiusura > dataFineProgetto Then
                    errore.Append("La 'Data Chiusura' non può essere successiva alla data di fine progetto.<br/>")
                    campiValidi = False
                End If
                If dataChiusura < dataInizioAttivitaVolontario Then
                    errore.Append("La 'Data Chiusura' non può essere antecedente alla data di inizio servizio del volontario.<br/>")
                    campiValidi = False
                End If
            End If
        End If
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
    Sub CheckIntervalloDate()
        Dim errore As StringBuilder = New StringBuilder()
        Dim dataFine As Date
        Dim dataInizio As Date
        dataInizio = dtgRisultatoRicerca.Items.Item(0).Cells.Item(2).Text
        If (Date.TryParse(txtDataChiusura.Text, dataFine) = True) Then
            If DateDiff("m", dataInizio, DateAdd("d", 1, dataFine)) >= 6 And (ddlCausale.SelectedValue = "38" Or ddlCausale.SelectedValue = "39" Or ddlCausale.SelectedValue = "40") Then
                errore.Append("Attenzione i mesi calcolati di servizio risultano 6 o più. Verificare la causale di chiusura. Procedere con l'operazione? <br/>")
            End If
            If DateDiff("m", dataInizio, DateAdd("d", 1, dataFine)) < 6 And (ddlCausale.SelectedValue = "35" Or ddlCausale.SelectedValue = "36" Or ddlCausale.SelectedValue = "37") Then
                errore.Append("Attenzione i mesi calcolati di servizio risultano meno di 6. Verificare la causale di chiusura. Procedere con l'operazione? <br/>")
            End If
            lblmessaggiosopra.Text = lblmessaggiosopra.Text + errore.ToString()
        End If

    End Sub
End Class
