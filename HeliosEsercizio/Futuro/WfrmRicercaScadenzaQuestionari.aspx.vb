Imports System.Drawing.Printing
Imports System.Drawing.Imaging
Imports System.Web.UI
Imports System.Drawing
Imports System.IO
Public Class WfrmRicercaScadenzaQuestionari
    Inherits System.Web.UI.Page
    Dim dtrGenerico As SqlClient.SqlDataReader
    Dim strquery As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        '***Generata da Guido Testa in data:21/07/06
        If Not Session("LogIn") Is Nothing Then
            If Session("LogIn") = False Then 'verifico validità log-in
                Response.Redirect("LogOn.aspx")
            End If
        Else
            Response.Redirect("LogOn.aspx")
        End If

        If IsPostBack = False Then
            'scarico la session della datatable per la ricerca così che in una nuova pagina non verrà 
            'erroneamente visualizzato alcun item
            Session("DtbRicVol") = Nothing

            'richiamo sub dove popolo combo
            CaricaPrima()

            If Session("CodiceRegioneEnte").ToString <> "[]" Then
                txtCodReg.Text = Mid(Session("CodiceRegioneEnte"), 2, 7)
            Else
                txtCodReg.Text = ""
            End If

        End If
        'Controllo sul tipo di utente
        If Session("TipoUtente") = "E" Then
            'Disabilito la possibilità di selezionare la denominazione dell'ente            
            lblCodReg.Visible = False
            txtCodReg.Visible = False
            lblDenEnte.Visible = False
            txtDenEnte.Visible = False
        Else
            lblCodReg.Visible = True
            txtCodReg.Visible = True
            lblDenEnte.Visible = True
            txtDenEnte.Visible = True
        End If
    End Sub

    Private Sub CaricaPrima()

        '*****Carico Combo Bandi
        'Mod. il 03/12/2014 da simona cordella con il filtrovisibilità
        Dim strsql As String

        strsql = "SELECT DISTINCT Bando.idBando,bando.bandobreve,bando.annobreve "
        strsql = strsql & " FROM bando"
        strsql = strsql & " INNER JOIN AssociaBandoTipiProgetto abtp on abtp.idbando =  bando.idbando"
        strsql = strsql & " INNER JOIN TipiProgetto  tp on abtp.idtipoprogetto = tp.idtipoprogetto"
        strsql = strsql & " WHERE Bando.FormazioneGenerale=1 and tp.MacroTipoProgetto like '" & Session("FiltroVisibilita") & "'"
        strsql = strsql & " ORDER BY bando.annobreve desc"
        '"select idbando, bandobreve from Bando where FormazioneGenerale=1"
        DdlBando.DataSource = MakeParentTable(strsql)
        DdlBando.DataTextField = "ParentItem"
        DdlBando.DataValueField = "id"
        DdlBando.DataBind()

    End Sub

    Protected Sub cmdChiudi_Click(sender As Object, e As EventArgs) Handles cmdChiudi.Click
        Response.Redirect("WfrmMain.aspx")
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
        myDataTable.Columns.Add(myDataColumn)
        dtrgenerico = ClsServer.CreaDatareader(strquery, Session("conn"))
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

    Protected Sub cmdSalva_Click(sender As Object, e As EventArgs) Handles cmdSalva.Click
        lblmessaggio.Text = ""
        dgRisultatoRicerca.CurrentPageIndex = 0
        EseguiRicerca(0)
    End Sub
    Private Sub EseguiRicerca(ByVal bytVerifica As Byte, Optional ByVal bytpage As Integer = 0)
        '*****************************************************************************************+
        'AUTORE: Guido Testa 
        'DATA: 18/12/2006
        'DESCRIZONE: ricerco tutti i progetti con scadenza questionario entro i 180gg dalla data inizio ultimo progetto

        Dim Mydataset As New DataSet
        Dim strCond As String
        strCond = " Where "
        dgRisultatoRicerca.Visible = True

        strquery = "Select IdBandoAttività,codiceregione, bando, UltimaDataInizioProgetto, giornirestanti, datascadenza, ente,email,telefono,fax,TipoFormazioneGeneraleDescr From VW_SCADENZE_QUESTIONARI_2 "

        'imposto eventuali parametri  
        If DdlBando.SelectedItem.Text <> "" Then
            strquery = strquery & strCond & " idbando =" & DdlBando.SelectedValue
            strCond = " And "
        End If

        If chkEscludi.Checked = True Then
            strquery = strquery & strCond & " giornirestanti > 0"
            strCond = " And "
        End If

        If txtDataScadenzaDal.Text <> "" And txtDataScadenzaAl.Text <> "" Then
            strquery = strquery & strCond & " convert(datetime,datascadenza) between '" & CDate(txtDataScadenzaDal.Text) & "' And '" & CDate(txtDataScadenzaAl.Text) & "'"
            strCond = " And "
        ElseIf txtDataScadenzaDal.Text <> "" And txtDataScadenzaAl.Text = "" Then
            strquery = strquery & strCond & " convert(datetime,datascadenza) >='" & CDate(txtDataScadenzaDal.Text) & "'"
            strCond = " And "
        ElseIf txtDataScadenzaDal.Text = "" And txtDataScadenzaAl.Text <> "" Then
            strquery = strquery & strCond & " convert(datetime,datascadenza) <='" & CDate(txtDataScadenzaAl.Text) & "'"
            strCond = " And "
        End If

        ''If txtDataScadenza.Text <> "" Then
        ''    If optUguale.Checked = True Then        'uguale alla data specificata
        ''        strquery = strquery & strCond & " convert(datetime,datascadenza) ='" & CDate(txtDataScadenza.Text) & "'"
        ''        strCond = " And "
        ''    ElseIf optFinoA.Checked = True Then     'fino alla data specificata
        ''        strquery = strquery & strCond & " convert(datetime,datascadenza) <='" & CDate(txtDataScadenza.Text) & "'"
        ''        strCond = " And "
        ''    End If
        ''End If

        If CStr(Session("TipoUtente")) = "E" Then
            strquery = strquery & strCond & " CodiceRegione = '" & CStr(Session("CodiceRegioneEnte")).Substring(1, 7) & "'"
            strCond = " And "
        End If

        If txtCodReg.Text <> "" Then
            strquery = strquery & strCond & " CodiceRegione like '" & ClsServer.NoApice(txtCodReg.Text) & "%'"
            strCond = " And "
        End If

        If txtDenEnte.Text <> "" Then
            strquery = strquery & strCond & " Ente like '" & ClsServer.NoApice(txtDenEnte.Text) & "%'"
            strCond = " And "
        End If
        'FiltroVisibilita 03/12/20104 da s.c.
        If Session("FiltroVisibilita") <> Nothing Then
            strquery = strquery & strCond & " MacroTipoProgetto like '" & Session("FiltroVisibilita") & "' "
            strCond = "AND "
        End If
        strquery = strquery & " Order by GiorniRestanti, CodiceRegione "

        Mydataset = ClsServer.DataSetGenerico(strquery, Session("conn"))
        dgRisultatoRicerca.DataSource = Mydataset
        dgRisultatoRicerca.DataBind() 'valorizzo griglia

        If dgRisultatoRicerca.Items.Count = 0 Then 'se la griglia e vuota la nascondo
            lblmessaggio.Text = "La ricerca non ha prodotto alcun risultato"
            dgRisultatoRicerca.Visible = False
            imgStampa.Visible = False
        Else
            imgStampa.Visible = True
            Call CarciaDatiStampa(Mydataset)
        End If
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

    Sub CarciaDatiStampa(ByVal dtsGenerico As DataSet)
        '*********************************************************************************
        'blocco per la creazione della datatable per la stampa della ricerca
        'nome e posizione di lettura delle colopnne a base 0
        Dim NomeColonne(9) As String
        Dim NomiCampiColonne(9) As String

        'nome della colonna 
        'e posizione nella griglia di lettura
        NomeColonne(0) = "Cod.Ente"
        NomeColonne(1) = "Bando"
        NomeColonne(2) = "Tipo Formazione"
        NomeColonne(3) = "Data Inizio Ultimo Progetto"
        NomeColonne(4) = "Giorni alla scadenza"
        NomeColonne(5) = "Data scadenza"
        NomeColonne(6) = "Ente"
        NomeColonne(7) = "E-mail"
        NomeColonne(8) = "Telefono"
        NomeColonne(9) = "Fax"

        NomiCampiColonne(0) = "codiceregione"
        NomiCampiColonne(1) = "bando"
        NomiCampiColonne(2) = "TipoFormazioneGeneraleDescr"
        NomiCampiColonne(3) = "UltimaDataInizioProgetto"
        NomiCampiColonne(4) = "giornirestanti"
        NomiCampiColonne(5) = "datascadenza"
        NomiCampiColonne(6) = "ente"
        NomiCampiColonne(7) = "email"
        NomiCampiColonne(8) = "telefono"
        NomiCampiColonne(9) = "fax"

        'carico un datatable che userò poi nella pagina di stampa
        'il numero delle colonne è a base 0
        CaricaDataTablePerStampa(dtsGenerico, 9, NomeColonne, NomiCampiColonne)

    End Sub
    Private Sub dgRisultatoRicerca_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgRisultatoRicerca.PageIndexChanged
        dgRisultatoRicerca.CurrentPageIndex = e.NewPageIndex
        EseguiRicerca(0)
        dgRisultatoRicerca.SelectedIndex = -1
    End Sub

    Private Sub dgRisultatoRicerca_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgRisultatoRicerca.ItemCommand
        If e.CommandName = "Proroga" Then

            If e.Item.Cells(4).Text <= 0 Then
                Dim strLocal As String
                Dim mycommand As New SqlClient.SqlCommand
                mycommand.Connection = Session("conn")

                strLocal = " update BandiAttività set DataProrogaQuestionarioFormazione= getDate(),UsernameProrogaQuestionarioFormazione='" & Session("Utente") & "' " & _
                           " where idBandoAttività = '" & e.Item.Cells(7).Text & "'"
                mycommand.CommandText = strLocal
                mycommand.ExecuteNonQuery()
                'Messaggio di effettuazione proroga
                lblmessaggio.Visible = True
                lblmessaggio.Text = "Proroga Effettuata!"
            Else
                lblmessaggio.Visible = True
                lblmessaggio.Text = "L'ente non necessita di proroga!"
            End If

        End If
    End Sub

    Protected Sub imgStampa_Click(sender As Object, e As EventArgs) Handles imgStampa.Click
        imgStampa.Visible = False
        StampaCSV(Session("DtbRicerca"))
    End Sub
    Function StampaCSV(ByVal DTBRicerca As DataTable)

        Dim dtrSediAttuazione As Data.SqlClient.SqlDataReader
        Dim Writer As StreamWriter
        Dim xLinea As String
        Dim i As Int64
        Dim j As Int64
        Dim NomeUnivoco As String
        Dim xPrefissoNome As String = vbNullString
        Dim Reader As StreamReader

        If DTBRicerca.Rows.Count = 0 Then
            ApriCSV1.Visible = False

        Else
            xPrefissoNome = Session("Utente")
            NomeUnivoco = xPrefissoNome & "ExpScadenzarioQuestionario" & Year(Now) & Month(Now) & Day(Now) & Hour(Now) & Minute(Now) & Second(Now)
            Writer = New StreamWriter(Server.MapPath("download") & "\" & NomeUnivoco & ".CSV")
            'Creazione dell'inntestazione del CSV
            Dim intNumCol As Int64 = DTBRicerca.Columns.Count
            For i = 0 To intNumCol - 1
                xLinea &= DTBRicerca.Columns.Item(CInt(i)).ColumnName() & ";"
            Next
            Writer.WriteLine(xLinea)
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

                Writer.WriteLine(xLinea)
                xLinea = vbNullString

            Next

            ApriCSV1.Visible = True
            ApriCSV1.NavigateUrl = "download\" & NomeUnivoco & ".CSV"

            Writer.Close()
            Writer = Nothing

        End If

    End Function

End Class